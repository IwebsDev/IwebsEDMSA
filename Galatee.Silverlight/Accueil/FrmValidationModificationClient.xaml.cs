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
using Galatee.Silverlight.Resources.Accueil;
using System.IO;
using System.Collections.ObjectModel;

namespace Galatee.Silverlight.Accueil
{
    public partial class FrmValidationModificationClient : ChildWindow
    {
        private CsDemande laDetailDemande = new CsDemande();
        public ObservableCollection<ObjDOCUMENTSCANNE> LstPiece = new ObservableCollection<ObjDOCUMENTSCANNE>();
        public ObservableCollection<CsTypeDOCUMENTSCANNE> LstTypeDocument = new ObservableCollection<CsTypeDOCUMENTSCANNE>();
        private List<ObjDOCUMENTSCANNE> ObjetScanne = new List<ObjDOCUMENTSCANNE>();

        private UcImageScanne formScanne = null;

        private byte[] image;
        public FrmValidationModificationClient(int iddemande)
        {
            InitializeComponent();
            ChargeDetailDEvis(iddemande);
            prgBar.Visibility = System.Windows.Visibility.Visible ;
        }
        private void ChargeDetailDEvis(int IdDemandeDevis)
        {

            AcceuilServiceClient client = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
            client.ChargerDetailDemandeCompleted  += (ssender, args) =>
            {
                prgBar.Visibility = System.Windows.Visibility.Collapsed;

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
                        CENTRE = laDetailDemande.LaDemande.CENTRE,
                        REFCLIENT = laDetailDemande.LaDemande.CLIENT,
                        ORDRE = laDetailDemande.LaDemande.ORDRE,
                        FK_IDCENTRE = laDetailDemande.LaDemande.FK_IDCENTRE,
                        TYPEDEMANDE = laDetailDemande.LaDemande.TYPEDEMANDE
                    };
                    RetourneInfoAdresseduClient(leCleint);
                }
                LayoutRoot.Cursor = Cursors.Arrow;
            };
            client.ChargerDetailDemandeAsync(IdDemandeDevis,string.Empty );
        }
        ServiceAccueil.CsDemande laDDe = new CsDemande();
        private void RetourneInfoAdresseduClient(CsClient leClient)
        {
            try
            {
                prgBar.Visibility = System.Windows.Visibility.Visible ;

                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient client = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                client.ChargerDetailClientCompleted  += (ssender, args) =>
                {
                    prgBar.Visibility = System.Windows.Visibility.Collapsed;

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
                        laDDe = args.Result;
                    AfficherInfoClient(laDetailDemande  , laDDe );
                    AfficherDocumentScanne(laDetailDemande.ObjetScanne);
                    this.Txt_Motif.Text = string.IsNullOrEmpty(laDetailDemande.LaDemande.MOTIF) ? string.Empty : laDetailDemande.LaDemande.MOTIF; 
                };
                client.ChargerDetailClientAsync(leClient);
            }
            catch (Exception)
            {

                throw;
            }
        }
        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {


        }

        private void dgListePiece_CurrentCellChanged(object sender, EventArgs e)
        {
            dgListePiece.BeginEdit();
        }
        private void AfficherInfoClient(CsDemande   _NouvelInfoClient, CsDemande  _LeClientdemande)
        {
            try
            {
                this.Txt_CodeCentre.Text = string.IsNullOrEmpty(_LeClientdemande.Abonne.CENTRE) ? string.Empty : _LeClientdemande.Abonne.CENTRE;
                this.Txt_LibelleCentre.Text = string.IsNullOrEmpty(_LeClientdemande.Abonne.LIBELLECENTRE) ? string.Empty : _LeClientdemande.Abonne.LIBELLECENTRE;
                this.Txt_CodeProduit.Text = string.IsNullOrEmpty(_LeClientdemande.Abonne.PRODUIT) ? string.Empty : _LeClientdemande.Abonne.PRODUIT;
                this.Txt_LibelleProduit.Text = string.IsNullOrEmpty(_LeClientdemande.Abonne.LIBELLEPRODUIT) ? string.Empty : _LeClientdemande.Abonne.LIBELLEPRODUIT;
                this.Txt_NumDemande.Text = string.IsNullOrEmpty(_LeClientdemande.Abonne.NUMDEM) ? string.Empty : _LeClientdemande.Abonne.NUMDEM;
                this.Txt_Client.Text = string.IsNullOrEmpty(_LeClientdemande.Abonne.CLIENT) ? string.Empty : _LeClientdemande.Abonne.CLIENT;
                this.Txt_Ordre.Text = string.IsNullOrEmpty(_LeClientdemande.Abonne.ORDRE) ? string.Empty : _LeClientdemande.Abonne.ORDRE;


                this.Txt_NomClientAbon.Text = (string.IsNullOrEmpty(_LeClientdemande.LeClient.NOMABON) ? string.Empty : _LeClientdemande.LeClient.NOMABON);
                this.Txt_telephone.Text = string.IsNullOrEmpty(_LeClientdemande.LeClient.TELEPHONE) ? string.Empty : _LeClientdemande.LeClient.TELEPHONE;
                this.Txt_Addresse1.Text = string.IsNullOrEmpty(_LeClientdemande.LeClient.ADRMAND1) ? string.Empty : _LeClientdemande.LeClient.ADRMAND1;
                this.Txt_adresse2.Text = string.IsNullOrEmpty(_LeClientdemande.LeClient.ADRMAND2) ? string.Empty : _LeClientdemande.LeClient.ADRMAND2;

                this.Txt_CodeConsomateur.Text = string.IsNullOrEmpty(_LeClientdemande.LeClient.CODECONSO) ? string.Empty : _LeClientdemande.LeClient.CODECONSO;
                this.Txt_LibelleCodeConso.Text = string.IsNullOrEmpty(_LeClientdemande.LeClient.LIBELLECODECONSO) ? string.Empty : _LeClientdemande.LeClient.LIBELLECODECONSO;

                this.Txt_CodeCategorie.Text = string.IsNullOrEmpty(_LeClientdemande.LeClient.CATEGORIE) ? string.Empty : _LeClientdemande.LeClient.CATEGORIE;
                this.Txt_LibelleCategorie.Text = string.IsNullOrEmpty(_LeClientdemande.LeClient.LIBELLECATEGORIE) ? string.Empty : _LeClientdemande.LeClient.LIBELLECATEGORIE;

                this.Txt_CodeFermableClient.Text = string.IsNullOrEmpty(_LeClientdemande.LeClient.CODERELANCE) ? string.Empty : _LeClientdemande.LeClient.CODERELANCE;
                this.Txt_LibelleFermable.Text = string.IsNullOrEmpty(_LeClientdemande.LeClient.LIBELLERELANCE) ? string.Empty : _LeClientdemande.LeClient.LIBELLERELANCE;

                this.Txt_CodeRegroupement.Text = string.IsNullOrEmpty(_LeClientdemande.LeClient.REGROUPEMENT) ? string.Empty : _LeClientdemande.LeClient.REGROUPEMENT;
                this.Txt_LibelleGroupeCode.Text = string.IsNullOrEmpty(_LeClientdemande.LeClient.LIBELLEREGCLI) ? string.Empty : _LeClientdemande.LeClient.LIBELLEREGCLI;

                this.Txt_CodeNationalite.Text = string.IsNullOrEmpty(_LeClientdemande.LeClient.NATIONNALITE) ? string.Empty : _LeClientdemande.LeClient.NATIONNALITE;
                this.Txt_Nationnalite.Text = string.IsNullOrEmpty(_LeClientdemande.LeClient.LIBELLENATIONALITE) ? string.Empty : _LeClientdemande.LeClient.LIBELLENATIONALITE;

                this.Txt_CodeCivilite.Text = string.IsNullOrEmpty(_LeClientdemande.LeClient.DENABON) ? string.Empty : _LeClientdemande.LeClient.DENABON;
                this.Txt_Civilite.Text = string.IsNullOrEmpty(_LeClientdemande.LeClient.LIBELLEDENOMINATION) ? string.Empty : _LeClientdemande.LeClient.LIBELLEDENOMINATION;


                this.Txt_Matricule.Text = string.IsNullOrEmpty(_LeClientdemande.LeClient.MATRICULE) ? string.Empty : _LeClientdemande.LeClient.MATRICULE;
                if (_LeClientdemande.Abonne != null)
                {
                    this.Txt_AncCodeTarif.Text = string.IsNullOrEmpty(_LeClientdemande.Abonne.TYPETARIF) ? string.Empty : _LeClientdemande.Abonne.TYPETARIF;
                    this.Txt_AncLibelleTarif.Text = string.IsNullOrEmpty(_LeClientdemande.Abonne.LIBELLETARIF) ? string.Empty : _LeClientdemande.Abonne.LIBELLETARIF;
                }
               
                this.Txt_NouvNomClientAbon.Text =string.IsNullOrEmpty(_NouvelInfoClient.LeClient.NOMABON) ? string.Empty : _NouvelInfoClient.LeClient.NOMABON;
                this.Txt_Nouvtelephone.Text = string.IsNullOrEmpty(_NouvelInfoClient.LeClient.TELEPHONE) ? string.Empty : _NouvelInfoClient.LeClient.TELEPHONE;
                this.Txt_NouvAddresse1.Text = string.IsNullOrEmpty(_NouvelInfoClient.LeClient.ADRMAND1) ? string.Empty : _NouvelInfoClient.LeClient.ADRMAND1;
                this.Txt_Nouvadresse2.Text = string.IsNullOrEmpty(_NouvelInfoClient.LeClient.ADRMAND2) ? string.Empty : _NouvelInfoClient.LeClient.ADRMAND2;

                this.Txt_NouvCodeConsomateur.Text = string.IsNullOrEmpty(_NouvelInfoClient.LeClient.CODECONSO) ? string.Empty : _NouvelInfoClient.LeClient.CODECONSO;
                this.Txt_NouvLibelleCodeConso.Text = string.IsNullOrEmpty(_NouvelInfoClient.LeClient.LIBELLECODECONSO) ? string.Empty : _NouvelInfoClient.LeClient.LIBELLECODECONSO;

                this.Txt_NouvCodeCategorie.Text = string.IsNullOrEmpty(_NouvelInfoClient.LeClient.CATEGORIE) ? string.Empty : _NouvelInfoClient.LeClient.CATEGORIE;
                this.Txt_NouvLibelleCategorie.Text = string.IsNullOrEmpty(_NouvelInfoClient.LeClient.LIBELLECATEGORIE) ? string.Empty : _NouvelInfoClient.LeClient.LIBELLECATEGORIE;

                this.Txt_NouvCodeFermableClient.Text = string.IsNullOrEmpty(_NouvelInfoClient.LeClient.CODERELANCE) ? string.Empty : _NouvelInfoClient.LeClient.CODERELANCE;
                this.Txt_NouvLibelleFermable.Text = string.IsNullOrEmpty(_NouvelInfoClient.LeClient.LIBELLERELANCE) ? string.Empty : _NouvelInfoClient.LeClient.LIBELLERELANCE;

                this.Txt_NouvCodeNationalite.Text = string.IsNullOrEmpty(_NouvelInfoClient.LeClient.NATIONNALITE) ? string.Empty : _NouvelInfoClient.LeClient.NATIONNALITE;
                this.Txt_NouvNationnalite.Text = string.IsNullOrEmpty(_NouvelInfoClient.LeClient.LIBELLENATIONALITE) ? string.Empty : _NouvelInfoClient.LeClient.LIBELLENATIONALITE;

                this.Txt_NouvCodeCivilite.Text = string.IsNullOrEmpty(_NouvelInfoClient.LeClient.DENABON) ? string.Empty : _NouvelInfoClient.LeClient.DENABON;
                this.Txt_NouvCivilite.Text = string.IsNullOrEmpty(_NouvelInfoClient.LeClient.LIBELLEDENOMINATION) ? string.Empty : _NouvelInfoClient.LeClient.LIBELLEDENOMINATION;

                this.Txt_NouvCodeRegroupement.Text = string.IsNullOrEmpty(_NouvelInfoClient.LeClient.REGROUPEMENT) ? string.Empty : _NouvelInfoClient.LeClient.REGROUPEMENT;
                this.Txt_NouvLibelleGroupeCode.Text = string.IsNullOrEmpty(_NouvelInfoClient.LeClient.LIBELLEREGCLI) ? string.Empty : _NouvelInfoClient.LeClient.LIBELLEREGCLI;

                this.Txt_NouvMatricule.Text = string.IsNullOrEmpty(_NouvelInfoClient.LeClient.MATRICULE) ? string.Empty : _NouvelInfoClient.LeClient.MATRICULE;

                if (_NouvelInfoClient.Abonne != null)
                {
                    this.Txt_NouvCodeTarif.Text = string.IsNullOrEmpty(_NouvelInfoClient.Abonne.TYPETARIF) ? string.Empty : _NouvelInfoClient.Abonne.TYPETARIF;
                    this.Txt_NouvLibelleTarif.Text = string.IsNullOrEmpty(_NouvelInfoClient.Abonne.LIBELLETARIF) ? string.Empty : _NouvelInfoClient.Abonne.LIBELLETARIF;
                }

                if (laDetailDemande.ObjetScanne != null && laDetailDemande.ObjetScanne.Count != 0)
                    dgListePiece.ItemsSource = this.LstPiece;

                if (this.Txt_NouvNomClientAbon.Text != this.Txt_NomClientAbon.Text)
                {
                    this.Txt_NouvNomClientAbon.FontWeight = FontWeights.ExtraBold;
                    this.Txt_NouvNomClientAbon.FontStyle = FontStyles.Italic;
                }
                if (this.Txt_Nouvtelephone.Text != this.Txt_telephone.Text)
                {
                    this.Txt_Nouvtelephone.FontWeight = FontWeights.ExtraBold;
                    this.Txt_Nouvtelephone.FontStyle = FontStyles.Italic;
                }
                if (this.Txt_NouvAddresse1.Text != this.Txt_Addresse1.Text)
                {
                    this.Txt_NouvAddresse1.FontWeight = FontWeights.ExtraBold;
                    this.Txt_NouvAddresse1.FontStyle = FontStyles.Italic;
                }
                if (this.Txt_Nouvadresse2.Text != this.Txt_adresse2.Text)
                {
                    this.Txt_Nouvadresse2.FontWeight = FontWeights.ExtraBold;
                    this.Txt_Nouvadresse2.FontStyle = FontStyles.Italic;
                }
                if (this.Txt_NouvCodeConsomateur.Text != this.Txt_CodeConsomateur.Text)
                {
                    this.Txt_NouvCodeConsomateur.FontWeight = FontWeights.ExtraBold;
                    this.Txt_NouvLibelleCodeConso.FontWeight = FontWeights.ExtraBold;
                    this.Txt_NouvLibelleCodeConso.FontStyle = FontStyles.Italic;
                    this.Txt_NouvCodeConsomateur.FontStyle = FontStyles.Italic;
                }
                if (this.Txt_NouvCodeCategorie.Text != this.Txt_CodeCategorie.Text)
                {
                    this.Txt_NouvCodeCategorie.FontWeight = FontWeights.ExtraBold;
                    this.Txt_NouvLibelleCategorie.FontWeight = FontWeights.ExtraBold;
                    this.Txt_NouvLibelleCategorie.FontStyle = FontStyles.Italic;
                    this.Txt_NouvCodeCategorie.FontStyle = FontStyles.Italic;
                }
                if (this.Txt_NouvCodeFermableClient.Text != this.Txt_CodeFermableClient.Text)
                {
                    this.Txt_NouvCodeFermableClient.FontWeight = FontWeights.ExtraBold;
                    this.Txt_NouvLibelleFermable.FontWeight = FontWeights.ExtraBold;
                    this.Txt_NouvLibelleFermable.FontStyle = FontStyles.Italic;
                    this.Txt_NouvCodeFermableClient.FontStyle = FontStyles.Italic;
                }
                if (this.Txt_NouvCodeNationalite.Text != this.Txt_CodeNationalite.Text)
                {
                    this.Txt_NouvCodeNationalite.FontWeight = FontWeights.ExtraBold;
                    this.Txt_NouvNationnalite.FontWeight = FontWeights.ExtraBold;
                    this.Txt_NouvNationnalite.FontStyle = FontStyles.Italic;
                    this.Txt_NouvCodeNationalite.FontStyle = FontStyles.Italic;
                }

                if (this.Txt_NouvCodeCivilite.Text != this.Txt_CodeCivilite.Text)
                {
                    this.Txt_NouvCodeCivilite.FontWeight = FontWeights.ExtraBold;
                    this.Txt_NouvCivilite.FontWeight = FontWeights.ExtraBold;
                    this.Txt_NouvCivilite.FontStyle = FontStyles.Italic;
                    this.Txt_NouvCodeCivilite.FontStyle = FontStyles.Italic;
                }


                if (this.Txt_NouvMatricule.Text != this.Txt_Matricule.Text)
                {
                    this.Txt_NouvMatricule.FontWeight = FontWeights.ExtraBold;
                    this.Txt_NouvMatricule.FontStyle = FontStyles.Italic;
                }
                if (this.Txt_AncCodeTarif.Text != this.Txt_NouvCodeTarif.Text)
                {
                    this.Txt_NouvCodeTarif.FontWeight = FontWeights.ExtraBold;
                    this.Txt_NouvCodeTarif.FontStyle = FontStyles.Italic;

                    this.Txt_NouvLibelleTarif .FontWeight = FontWeights.ExtraBold;
                    this.Txt_NouvLibelleTarif.FontStyle = FontStyles.Italic;
                }
                if (this.Txt_NouvCodeRegroupement.Text != this.Txt_CodeRegroupement.Text)
                {
                    this.Txt_NouvCodeRegroupement.FontWeight = FontWeights.ExtraBold;
                    this.Txt_NouvCodeRegroupement.FontWeight = FontWeights.ExtraBold;
                    this.Txt_NouvCodeRegroupement.FontStyle = FontStyles.Italic;
                    this.Txt_NouvCodeRegroupement.FontStyle = FontStyles.Italic;

                    this.Txt_NouvLibelleGroupeCode.FontWeight = FontWeights.ExtraBold;
                    this.Txt_NouvLibelleGroupeCode.FontWeight = FontWeights.ExtraBold;
                    this.Txt_NouvLibelleGroupeCode.FontStyle = FontStyles.Italic;
                    this.Txt_NouvLibelleGroupeCode.FontStyle = FontStyles.Italic;
                    
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
                        Message.ShowInformation("Demande cloturée avec succès", "Cloturedemande");
                        this.DialogResult = false;
                    }
                    else
                        Message.ShowError("Erreur à la clôture de la demande", "Cloturedemande");
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
            var messageBox = new MessageBoxControl.MessageBoxChildWindow("Attention", "Êtes-vous sûr de vouloir supprimer la ligne?", MessageBoxControl.MessageBoxButtons.OkCancel, MessageBoxControl.MessageBoxIcon.Information);
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

        private void cbo_typedoc_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Btn_Rejeter_Click(object sender, RoutedEventArgs e)
        {
            ClasseMEthodeGenerique.RejeterDemande(laDetailDemande);
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
    }
}

