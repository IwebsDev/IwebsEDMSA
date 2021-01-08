using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Galatee.Silverlight.ServiceAccueil;
using Galatee.Silverlight.Shared;
using Galatee.Silverlight.MainView;
using Galatee.Silverlight.Resources.Accueil ;
using System.IO;
using System.Collections.ObjectModel;

namespace Galatee.Silverlight.Accueil
{
    public partial class FrmValidationModificationAbonnement : ChildWindow
    {
        private CsDemande laDetailDemande = new CsDemande();
        public ObservableCollection<ObjDOCUMENTSCANNE> LstPiece = new ObservableCollection<ObjDOCUMENTSCANNE>();
        public ObservableCollection<CsTypeDOCUMENTSCANNE> LstTypeDocument = new ObservableCollection<CsTypeDOCUMENTSCANNE>();
        private List<ObjDOCUMENTSCANNE> ObjetScanne = new List<ObjDOCUMENTSCANNE>();

        private UcImageScanne formScanne = null;

        private byte[] image;
        public FrmValidationModificationAbonnement(int iddemande)
        {
            InitializeComponent();
            this.Txt_NouvTypeComptage.Visibility = System.Windows.Visibility.Collapsed ;
            this.Txt_TypeComptage.Visibility = System.Windows.Visibility.Collapsed;

            lbl_TypeComptage.Visibility = System.Windows.Visibility.Collapsed;
            lbl_NouvTypeComptage.Visibility = System.Windows.Visibility.Collapsed;
            ChargeDetailDEvis(iddemande);
        }
        private void ChargeDetailDEvis(int IdDemandeDevis)
        {

            AcceuilServiceClient client = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
            client.GetDevisByNumIdDevisCompleted += (ssender, args) =>
            {
                if (args.Cancelled || args.Error != null)
                {
                    string error = args.Error.Message;
                    Message.ShowError(error, Silverlight.Resources.Devis.Languages.txtDevis);
                    return;
                }
                if (args.Result == null)
                {
                    Message.ShowError(Silverlight.Resources.Devis.Languages.AucunesDonneesTrouvees, Silverlight.Resources.Devis.Languages.txtDevis);
                    return;
                }
                else
                {
                    laDetailDemande = args.Result;
                    ServiceAccueil.CsClient leCleint = new CsClient() 
                    {
                     CENTRE = laDetailDemande.LaDemande.CENTRE ,
                     REFCLIENT = laDetailDemande.LaDemande.CLIENT ,
                     ORDRE = laDetailDemande.LaDemande.ORDRE ,
                     FK_IDCENTRE = laDetailDemande.LaDemande.FK_IDCENTRE ,
                     TYPEDEMANDE = laDetailDemande.LaDemande.TYPEDEMANDE 
                    };
                    RetourneAbonClient(leCleint);
                }
                LayoutRoot.Cursor = Cursors.Arrow;
            };
            client.GetDevisByNumIdDevisAsync(IdDemandeDevis);
        }
        private void AfficherDocumentScanne(List<ObjDOCUMENTSCANNE> _LesDocScanne)
        {
            try
            {
                if (_LesDocScanne != null && _LesDocScanne.Count != 0)
                {
                    this.dgListePiece.ItemsSource = null;
                    this.dgListePiece.ItemsSource = _LesDocScanne;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        ServiceAccueil.CsDemande laDDe = new CsDemande();
        private void RetourneAbonClient(CsClient leClient)
        {
            try
            {
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient client = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                client.GeDetailByFromClientCompleted += (ssender, args) =>
                {
                    if (args.Cancelled || args.Error != null)
                    {
                        LayoutRoot.Cursor = Cursors.Arrow;
                        string error = args.Error.Message;
                        Message.ShowError(error, Silverlight.Resources.Devis.Languages.txtDevis);
                        return;
                    }
                    if (args.Result == null)
                    {
                        LayoutRoot.Cursor = Cursors.Arrow;
                        Message.ShowError(Silverlight.Resources.Devis.Languages.AucunesDonneesTrouvees, Silverlight.Resources.Devis.Languages.txtDevis);
                        return;
                    }
                    else
                        laDDe = args.Result  ;
                    if (laDDe.Abonne.PRODUIT == SessionObject.Enumere.ElectriciteMT)
                    {
                        this.Txt_NouvTypeComptage.Visibility = System.Windows.Visibility.Visible;
                        this.Txt_TypeComptage.Visibility = System.Windows.Visibility.Visible;

                        lbl_TypeComptage.Visibility = System.Windows.Visibility.Visible;
                        lbl_NouvTypeComptage.Visibility = System.Windows.Visibility.Visible;
                    
                    }
                   
                    AfficherInfoAbonnement(laDetailDemande.Abonne,laDDe.Abonne );
                    AfficherDocumentScanne(laDetailDemande.ObjetScanne);

                };
                client.GeDetailByFromClientAsync(leClient);
            }
            catch (Exception)
            {

                throw;
            }
        }
        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {


        }

        private void translate()
        {
            // Gestion de la langue
            this.lbl_DateAbonnement.Content = Langue.lbl_DateAbonnement;
            this.lbl_DateResiliation.Content = Langue.lbl_DateResiliation;
            this.lbl_Forfait.Content = this.lbl_NouvForfait.Content = Langue.lbl_Forfait;
            this.lbl_MoisFact.Content =this.lbl_NouvMoisFact.Content = Langue.lbl_MoisFact;
            this.lbl_Ordre.Content = Langue.lbl_Ordre;
            this.lbl_Periodicite.Content =this.lbl_NouvPeriodicite.Content = Langue.lbl_Periodicite;
            this.lbl_PuissanceSouscrite.Content = this.lbl_NouvPuissanceSouscrite.Content = Langue.lbl_PuissanceSouscrite;
            this.lbl_PuissanceUtilise.Content = this.lbl_NouvPuissanceUtilise.Content = Langue.lbl_PuissanceUtilise;
            this.lbl_Tarif.Content = this.lbl_NouvTarif.Content = Langue.lbl_Tarif;

        }
        private void dgListePiece_CurrentCellChanged(object sender, EventArgs e)
        {
            dgListePiece.BeginEdit();
        }
        private void AfficherInfoAbonnement(CsAbon _NouvelInfoAbon, CsAbon _LeAbonnementdemande)
        {
            try
            {
                this.Txt_CodeCentre.Text = string.IsNullOrEmpty(laDetailDemande.LaDemande.CENTRE) ? string.Empty : laDetailDemande.LaDemande.CENTRE;
                this.Txt_LibelleCentre.Text = string.IsNullOrEmpty(laDetailDemande.LaDemande.LIBELLECENTRE) ? string.Empty : laDetailDemande.LaDemande.LIBELLECENTRE;
                this.Txt_CodeProduit.Text = string.IsNullOrEmpty(laDetailDemande.LaDemande.PRODUIT) ? string.Empty : laDetailDemande.LaDemande.PRODUIT;
                this.Txt_LibelleProduit.Text = string.IsNullOrEmpty(laDetailDemande.LaDemande.LIBELLEPRODUIT) ? string.Empty : laDetailDemande.LaDemande.LIBELLEPRODUIT;
                this.Txt_NumDemande.Text = string.IsNullOrEmpty(laDetailDemande.LaDemande.NUMDEM) ? string.Empty : laDetailDemande.LaDemande.NUMDEM;
                this.Txt_Client.Text = string.IsNullOrEmpty(laDetailDemande.LaDemande.CLIENT) ? string.Empty : laDetailDemande.LaDemande.CLIENT;
                this.Txt_Ordre.Text = string.IsNullOrEmpty(laDetailDemande.LaDemande.ORDRE) ? string.Empty : laDetailDemande.LaDemande.ORDRE;

                if (_LeAbonnementdemande != null)
                {
                    if (_LeAbonnementdemande.PUISSANCE != null)
                        this.Txt_CodePussanceSoucrite.Text = Convert.ToDecimal(_LeAbonnementdemande.PUISSANCE.ToString()).ToString("N2");
                    if (_LeAbonnementdemande.PUISSANCEUTILISEE != null)
                        this.Txt_CodePuissanceUtilise.Text = Convert.ToDecimal(_LeAbonnementdemande.PUISSANCEUTILISEE.Value).ToString("N2");
                    this.Txt_CodeForfait.Text = string.IsNullOrEmpty(_LeAbonnementdemande.FORFAIT) ? string.Empty : _LeAbonnementdemande.FORFAIT;
                    this.Txt_LibelleForfait.Text = string.IsNullOrEmpty(_LeAbonnementdemande.LIBELLEFORFAIT) ? string.Empty : _LeAbonnementdemande.LIBELLEFORFAIT;

                    this.Txt_CodeTarif.Text = string.IsNullOrEmpty(_LeAbonnementdemande.TYPETARIF) ? string.Empty : _LeAbonnementdemande.TYPETARIF;
                    this.Txt_LibelleTarif.Text = string.IsNullOrEmpty(_LeAbonnementdemande.LIBELLETARIF) ? string.Empty : _LeAbonnementdemande.LIBELLETARIF;

                    this.Txt_CodeFrequence.Text = string.IsNullOrEmpty(_LeAbonnementdemande.PERFAC) ? string.Empty : _LeAbonnementdemande.PERFAC;
                    this.Txt_LibelleFrequence.Text = string.IsNullOrEmpty(_LeAbonnementdemande.LIBELLEFREQUENCE) ? string.Empty : _LeAbonnementdemande.LIBELLEFREQUENCE;

                    this.Txt_CodeMoisFacturation.Text = string.IsNullOrEmpty(_LeAbonnementdemande.MOISFAC) ? string.Empty : _LeAbonnementdemande.MOISFAC;
                    this.Txt_LibMoisFact.Text = string.IsNullOrEmpty(_LeAbonnementdemande.LIBELLEMOISFACT) ? string.Empty : _LeAbonnementdemande.LIBELLEMOISFACT;
                    this.Txt_DateAbonnement.Text = (_LeAbonnementdemande.DABONNEMENT == null) ? string.Empty : Convert.ToDateTime(_LeAbonnementdemande.DABONNEMENT.Value).ToShortDateString();
                    this.Txt_DateResiliation.Text = (_LeAbonnementdemande.DRES == null) ? string.Empty : Convert.ToDateTime(_LeAbonnementdemande.DRES.Value).ToShortDateString();


                    this.txt_DebutPeriodeExo.Text = string.IsNullOrEmpty(_LeAbonnementdemande.DEBUTEXONERATIONTVA) ? string.Empty : ClasseMEthodeGenerique.FormatPeriodeMMAAAA(_LeAbonnementdemande.DEBUTEXONERATIONTVA);
                    this.txt_FinPeriodeExo.Text = string.IsNullOrEmpty(_LeAbonnementdemande.FINEXONERATIONTVA) ? string.Empty : ClasseMEthodeGenerique.FormatPeriodeMMAAAA(_LeAbonnementdemande.FINEXONERATIONTVA);

                    this.Txt_AvanceSurConso.Text = _LeAbonnementdemande.AVANCE == null ? "0" : Convert.ToDecimal(_LeAbonnementdemande.AVANCE.Value).ToString(SessionObject.FormatMontant);
                    this.Txt_TypeComptage.Text = string.IsNullOrEmpty(_LeAbonnementdemande.LIBELLETYPECOMPTAGE) ? string.Empty : _LeAbonnementdemande.LIBELLETYPECOMPTAGE;  
                }
                if (_NouvelInfoAbon != null)
                {
                    if (_NouvelInfoAbon.PUISSANCE != null)
                        this.Txt_NouvCodePussanceSoucrite.Text = Convert.ToDecimal(_NouvelInfoAbon.PUISSANCE.ToString()).ToString("N2");
                    if (_NouvelInfoAbon.PUISSANCEUTILISEE != null)
                        this.Txt_NouvCodePuissanceUtilise.Text = Convert.ToDecimal(_NouvelInfoAbon.PUISSANCEUTILISEE.Value).ToString("N2");
                    this.Txt_NouvCodeForfait.Text = string.IsNullOrEmpty(_NouvelInfoAbon.FORFAIT) ? string.Empty : _NouvelInfoAbon.FORFAIT;
                    this.Txt_NouvLibelleForfait.Text = string.IsNullOrEmpty(_NouvelInfoAbon.LIBELLEFORFAIT) ? string.Empty : _NouvelInfoAbon.LIBELLEFORFAIT;

                    this.Txt_NouvCodeTarif.Text = string.IsNullOrEmpty(_NouvelInfoAbon.TYPETARIF) ? string.Empty : _NouvelInfoAbon.TYPETARIF;
                    this.Txt_NouvLibelleTarif.Text = string.IsNullOrEmpty(_NouvelInfoAbon.LIBELLETARIF) ? string.Empty : _NouvelInfoAbon.LIBELLETARIF;


                    this.Txt_NouvCodeFrequence.Text = string.IsNullOrEmpty(_NouvelInfoAbon.PERFAC) ? string.Empty : _NouvelInfoAbon.PERFAC;
                    this.Txt_NouvLibelleFrequence.Text = string.IsNullOrEmpty(_NouvelInfoAbon.LIBELLEFREQUENCE) ? string.Empty : _NouvelInfoAbon.LIBELLEFREQUENCE;

                    this.Txt_NouvCodeMoisFacturation.Text = string.IsNullOrEmpty(_NouvelInfoAbon.MOISFAC) ? string.Empty : _NouvelInfoAbon.MOISFAC;
                    this.Txt_NouvLibMoisFact.Text = string.IsNullOrEmpty(_NouvelInfoAbon.LIBELLEMOISFACT) ? string.Empty : _NouvelInfoAbon.LIBELLEMOISFACT;

                    this.txt_NouvelleDebutPeriodeExo.Text = string.IsNullOrEmpty(_NouvelInfoAbon.DEBUTEXONERATIONTVA) ? string.Empty : ClasseMEthodeGenerique.FormatPeriodeMMAAAA(_NouvelInfoAbon.DEBUTEXONERATIONTVA);
                    this.txt_NouvelleFinPeriodeExo.Text = string.IsNullOrEmpty(_NouvelInfoAbon.FINEXONERATIONTVA) ? string.Empty : ClasseMEthodeGenerique.FormatPeriodeMMAAAA(_NouvelInfoAbon.FINEXONERATIONTVA);


                    this.Txt_NouvDateAbonnement.Text = (_NouvelInfoAbon.DABONNEMENT == null) ? string.Empty : Convert.ToDateTime(_NouvelInfoAbon.DABONNEMENT.Value).ToShortDateString();
                    this.Txt_NouvDateResiliation.Text = (_NouvelInfoAbon.DRES == null) ? string.Empty : Convert.ToDateTime(_NouvelInfoAbon.DRES.Value).ToShortDateString();

                    this.Txt_NouvAvanceSurConso.Text = _NouvelInfoAbon.AVANCE == null ? "0" : Convert.ToDecimal(_NouvelInfoAbon.AVANCE.Value).ToString(SessionObject.FormatMontant);
                    this.Txt_NouvTypeComptage.Text = string.IsNullOrEmpty(_NouvelInfoAbon.LIBELLETYPECOMPTAGE) ? string.Empty  : _NouvelInfoAbon.LIBELLETYPECOMPTAGE;  

                }
                if (laDetailDemande.ObjetScanne != null && laDetailDemande.ObjetScanne.Count != 0)
                    dgListePiece.ItemsSource = this.LstPiece;


                if (this.Txt_NouvCodeTarif.Text != this.Txt_CodeTarif.Text)
                {
                    this.Txt_NouvCodeTarif.FontWeight = FontWeights.ExtraBold ;
                    this.Txt_NouvLibelleTarif.FontWeight = FontWeights.ExtraBold;
                    this.Txt_NouvLibelleTarif.FontStyle = FontStyles.Italic;
                    this.Txt_NouvCodeTarif.FontStyle = FontStyles.Italic;
                }
                if (this.Txt_NouvCodeForfait.Text != this.Txt_CodeForfait.Text)
                {
                    this.Txt_NouvCodeForfait.FontWeight = FontWeights.ExtraBold;
                    this.Txt_NouvCodeForfait.FontWeight = FontWeights.ExtraBold;
                    this.Txt_NouvCodeForfait.FontStyle = FontStyles.Italic;
                    this.Txt_NouvLibelleTarif.FontStyle = FontStyles.Italic;
                }
                if (this.Txt_NouvCodeFrequence.Text != this.Txt_CodeFrequence.Text)
                {
                    this.Txt_NouvCodeFrequence.FontWeight = FontWeights.ExtraBold;
                    this.Txt_NouvLibelleFrequence.FontWeight = FontWeights.ExtraBold;
                    this.Txt_NouvLibelleFrequence .FontStyle = FontStyles.Italic;
                    this.Txt_NouvCodeFrequence.FontStyle = FontStyles.Italic;
                }
                if (this.Txt_NouvCodeMoisFacturation.Text != this.Txt_CodeMoisFacturation.Text)
                {
                    this.Txt_NouvCodeMoisFacturation.FontWeight = FontWeights.Bold;
                    this.Txt_NouvCodeMoisFacturation.FontStyle = FontStyles.Italic;
                    this.Txt_NouvLibMoisFact.FontWeight = FontWeights.ExtraBold;
                    this.Txt_NouvLibMoisFact.FontStyle = FontStyles.Italic;

                }
                if (this.Txt_NouvDateAbonnement.Text != this.Txt_DateAbonnement.Text)
                {
                    this.Txt_NouvDateAbonnement.FontWeight = FontWeights.Bold;
                    this.Txt_NouvDateAbonnement.FontStyle = FontStyles.Italic;
                }
                if (this.Txt_NouvDateResiliation.Text != this.Txt_DateResiliation.Text)
                {
                    this.Txt_NouvDateResiliation.FontWeight = FontWeights.Bold;
                    this.Txt_NouvDateResiliation.FontStyle = FontStyles.Italic;
                }
                if (this.Txt_NouvCodePussanceSoucrite.Text != this.Txt_CodePussanceSoucrite.Text)
                {
                    this.Txt_NouvCodePussanceSoucrite.FontWeight = FontWeights.Bold;
                    this.Txt_NouvCodePussanceSoucrite.FontStyle = FontStyles.Italic;
                }
                if (this.Txt_NouvCodePuissanceUtilise.Text != this.Txt_CodePuissanceUtilise.Text)
                {
                    this.Txt_NouvCodePuissanceUtilise.FontWeight = FontWeights.Bold;
                    this.Txt_NouvCodePuissanceUtilise.FontStyle = FontStyles.Italic;
                }
                if (this.txt_NouvelleDebutPeriodeExo.Text != this.txt_DebutPeriodeExo.Text)
                {
                    this.txt_NouvelleDebutPeriodeExo.FontWeight = FontWeights.Bold;
                    this.txt_NouvelleDebutPeriodeExo.FontStyle = FontStyles.Italic;
                }
                if (this.Txt_NouvAvanceSurConso.Text != this.Txt_AvanceSurConso.Text)
                {
                    this.Txt_NouvAvanceSurConso.FontWeight = FontWeights.Bold;
                    this.Txt_NouvAvanceSurConso.FontStyle = FontStyles.Italic;
                }
                if (this.Txt_NouvTypeComptage.Text != this.Txt_TypeComptage.Text)
                {
                    this.Txt_NouvTypeComptage.FontWeight = FontWeights.Bold;
                    this.Txt_NouvTypeComptage.FontStyle = FontStyles.Italic;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        private void Cloturedemande()
        {
            try
            {
                AcceuilServiceClient clientDevis = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                clientDevis.ClotureValiderDemandeCompleted += (ss, b) =>
                {
                    if (b.Cancelled || b.Error != null)
                    {
                        string error = b.Error.Message;
                        Message.ShowError(error, Silverlight.Resources.Devis.Languages.txtDevis);
                        return;
                    }
                    if (b.Result == true)
                    {
                        List<string> codes = new List<string>();
                        codes.Add(laDetailDemande.InfoDemande.CODE);
                        Shared.ClasseMEthodeGenerique.TransmettreDemande(codes, true, this);
                    }
                };
                clientDevis.ClotureValiderDemandeAsync(laDetailDemande);

            }
            catch (Exception ex)
            {
                this.DialogResult = false;
                Message.Show(ex.Message, "Cloturedemande");
            }
        }
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            Cloturedemande();
        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void ChargerTypeDocument()
        {
            try
            {
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.ChargerTypeDocumentCompleted += (s, args) =>
                {
                    if ((args != null && args.Cancelled) || (args.Error != null))
                        return;

                    foreach (var item in args.Result)
                    {
                        LstTypeDocument.Add(item);
                    }
                    cbo_typedoc.ItemsSource = LstTypeDocument;
                    cbo_typedoc.DisplayMemberPath = "LIBELLE";
                    cbo_typedoc.SelectedValuePath = "PK_ID";

                    //this.Resources.Add("FuelList", LstTypeDocument);

                };
                service.ChargerTypeDocumentAsync();
                service.CloseAsync();

            }
            catch (Exception)
            {

                throw;
            }
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (cbo_typedoc.SelectedItem != null)
            {
                // Create an instance of the open file dialog box.
                var openDialog = new OpenFileDialog();
                // Set filter options and filter index.
                openDialog.Filter =
                    "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";
                openDialog.FilterIndex = 1;
                openDialog.Multiselect = false;
                // Call the ShowDialog method to show the dialog box.
                bool? userClickedOk = openDialog.ShowDialog();
                // Process input if the user clicked OK.
                if (userClickedOk == true)
                {
                    if (openDialog.Files != null && openDialog.Files.Count() > 0 && openDialog.File != null)
                    {
                        FileStream stream = openDialog.File.OpenRead();
                        var memoryStream = new MemoryStream();
                        stream.CopyTo(memoryStream);
                        image = memoryStream.GetBuffer();
                        formScanne = new UcImageScanne(memoryStream, SessionObject.ExecMode.Creation);
                        formScanne.Closed += new EventHandler(GetInformationFromChildWindowImagePreuve);
                        formScanne.Show();
                    }
                }
            }
        }

        private void GetInformationFromChildWindowImagePreuve(object sender, EventArgs e)
        {
            this.LstPiece.Add(new ObjDOCUMENTSCANNE { PK_ID = Guid.NewGuid(), NOMDOCUMENT = ((CsTypeDOCUMENTSCANNE)cbo_typedoc.SelectedItem).LIBELLE, FK_IDTYPEDOCUMENT = ((CsTypeDOCUMENTSCANNE)cbo_typedoc.SelectedItem).PK_ID, CONTENU = image, DATECREATION = DateTime.Now, DATEMODIFICATION = DateTime.Now, USERCREATION = UserConnecte.matricule, USERMODIFICATION = UserConnecte.matricule });
            this.dgListePiece.ItemsSource = this.LstPiece;
        }

        private void hyperlinkButtonPropScannee__Click(object sender, RoutedEventArgs e)
        {
            MemoryStream memoryStream = new MemoryStream(((HyperlinkButton)sender).Tag as byte[]);
            var ucImageScanne = new UcImageScanne(memoryStream, SessionObject.ExecMode.Modification);
            ucImageScanne.Show();
        }
      
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            var messageBox = new MessageBoxControl.MessageBoxChildWindow("Attention", "Ête-vous sure de vouloire supprimer la ligne?", MessageBoxControl.MessageBoxButtons.OkCancel, MessageBoxControl.MessageBoxIcon.Information);
            messageBox.OnMessageBoxClosed += (_, result) =>
            {
                if (messageBox.Result == MessageBoxResult.OK)
                {
                    ObjDOCUMENTSCANNE Fraix = (ObjDOCUMENTSCANNE)dgListePiece.SelectedItem;
                    this.LstPiece.Remove(Fraix);
                    this.dgListePiece.ItemsSource = this.LstPiece;
                }
                else
                {
                    return;
                }
            };
            messageBox.Show();
        }

        private void Btn_Rejeter_Click_1(object sender, RoutedEventArgs e)
        {
            Galatee.Silverlight.Shared.ClasseMEthodeGenerique.RejeterDemande(laDetailDemande);
            this.DialogResult = false;
        }
    }
}

