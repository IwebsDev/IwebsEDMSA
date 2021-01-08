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
    public partial class FrmValidationModificationCompteur : ChildWindow
    {

        CsCanalisation CanalisationAfficher = new CsCanalisation();
        List<CsCentre> LstCentre = new List<CsCentre>();
        List<CsProduit> ListeDesProduitDuSite = new List<CsProduit>();
        private CsDemande laDetailDemande = new CsDemande();
        public ObservableCollection<ObjDOCUMENTSCANNE> LstPiece = new ObservableCollection<ObjDOCUMENTSCANNE>();
        public ObservableCollection<CsTypeDOCUMENTSCANNE> LstTypeDocument = new ObservableCollection<CsTypeDOCUMENTSCANNE>();
        private List<ObjDOCUMENTSCANNE> ObjetScanne = new List<ObjDOCUMENTSCANNE>();
        private UcImageScanne formScanne = null;
        private byte[] image;

        public FrmValidationModificationCompteur(int iddemande)
        {
            InitializeComponent();
            ChargeDetailDEvis(iddemande);
            prgBar.Visibility = System.Windows.Visibility.Visible;
        }
        private void ChargeDetailDEvis(int IdDemandeDevis)
        {

            AcceuilServiceClient client = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
            client.ChargerDetailDemandeCompleted  += (ssender, args) =>
            {
                prgBar.Visibility = System.Windows.Visibility.Collapsed ;

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
                    RetourneInfoCompteurduClient(leCleint);
                }
                LayoutRoot.Cursor = Cursors.Arrow;
            };
            client.ChargerDetailDemandeAsync (IdDemandeDevis,string.Empty );
        }
        private void dgListePiece_CurrentCellChanged(object sender, EventArgs e)
        {
            dgListePiece.BeginEdit();
        }
        ServiceAccueil.CsDemande laDDe = new CsDemande();
        private void RetourneInfoCompteurduClient(CsClient leClient)
        {
            try
            {
                prgBar.Visibility = System.Windows.Visibility.Visible;
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient client = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                client.ChargerDetailClientCompleted  += (ssender, args) =>
                {
                    prgBar.Visibility = System.Windows.Visibility.Collapsed ;
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
                    {
                        laDDe = args.Result;
                        this.Txt_CodeCentre.Text = string.IsNullOrEmpty(laDetailDemande.LaDemande.CENTRE) ? string.Empty : laDetailDemande.LaDemande.CENTRE;
                        this.Txt_LibelleCentre.Text = string.IsNullOrEmpty(laDetailDemande.LaDemande.LIBELLECENTRE) ? string.Empty : laDetailDemande.LaDemande.LIBELLECENTRE;
                        this.Txt_CodeProduit.Text = string.IsNullOrEmpty(laDetailDemande.LaDemande.PRODUIT) ? string.Empty : laDetailDemande.LaDemande.PRODUIT;
                        this.Txt_LibelleProduit.Text = string.IsNullOrEmpty(laDetailDemande.LaDemande.LIBELLEPRODUIT) ? string.Empty : laDetailDemande.LaDemande.LIBELLEPRODUIT;
                        this.Txt_NumDemande.Text = string.IsNullOrEmpty(laDetailDemande.LaDemande.NUMDEM) ? string.Empty : laDetailDemande.LaDemande.NUMDEM;
                        this.Txt_Client.Text = string.IsNullOrEmpty(laDetailDemande.LaDemande.CLIENT) ? string.Empty : laDetailDemande.LaDemande.CLIENT;
                        if (laDetailDemande.LstCanalistion != null && laDetailDemande.LstCanalistion.Count != 0 &&
                            laDDe.LstCanalistion != null && laDDe.LstCanalistion.Count != 0)
                        {
                            if (laDDe.LstCanalistion.First().PRODUIT == SessionObject.Enumere.ElectriciteMT)
                            {
                                this.Txt_LibelleDiametre.IsEnabled = false;
                                laDDe.LstCanalistion.First().NUMERO = laDDe.LstCanalistion.First().NUMERO.Substring(5, (laDDe.LstCanalistion.First().NUMERO.Length - 5));
                            }
                            AfficherCannalisation(laDetailDemande.LstCanalistion.First(), laDDe.LstCanalistion.First());
                            AfficherDocumentScanne(laDetailDemande.ObjetScanne);
                        }
                        Txt_Motif.Text = string.IsNullOrEmpty(laDetailDemande.LaDemande.MOTIF) ? string.Empty : laDetailDemande.LaDemande.MOTIF; 
                    }
                };
                client.ChargerDetailClientAsync (leClient);
            }
            catch (Exception)
            {

                throw;
            }
        }
        void AfficherCannalisation(CsCanalisation NouvoCompteur, CsCanalisation AncienCompteur)
        {

            this.Txt_NumCompteur.Text = (string.IsNullOrEmpty(AncienCompteur.NUMERO)) ? string.Empty : AncienCompteur.NUMERO;
            this.Txt_AnneeFab.Text = string.IsNullOrEmpty(AncienCompteur.ANNEEFAB) ? string.Empty : AncienCompteur.ANNEEFAB;

            this.Txt_CodeTypeCompteur.Text = (string.IsNullOrEmpty(AncienCompteur.TYPECOMPTEUR)) ? string.Empty : AncienCompteur.TYPECOMPTEUR;
            this.Txt_LibelleTypeCompteur.Text = (string.IsNullOrEmpty(AncienCompteur.LIBELLETYPECOMPTEUR)) ? string.Empty : AncienCompteur.LIBELLETYPECOMPTEUR;

            this.Txt_CodeMarque.Text = (string.IsNullOrEmpty(AncienCompteur.MARQUE)) ? string.Empty : AncienCompteur.MARQUE;
            this.Txt_LibelleMarque.Text = (string.IsNullOrEmpty(AncienCompteur.LIBELLEMARQUE)) ? string.Empty : AncienCompteur.LIBELLEMARQUE;

            this.Txt_LibelleDiametre.Text = (string.IsNullOrEmpty(AncienCompteur.CODECALIBRECOMPTEUR)) ? string.Empty : AncienCompteur.CODECALIBRECOMPTEUR;
            
            this.Txt_CodeCadran.Text = AncienCompteur.CADRAN == null ? string.Empty : AncienCompteur.CADRAN.Value.ToString(); 


            this.Txt_NouvNumCompteur.Text = (string.IsNullOrEmpty(AncienCompteur.NUMERO)) ? string.Empty : NouvoCompteur.NUMERO;
            this.Txt_NouvAnneeFab.Text = string.IsNullOrEmpty(NouvoCompteur.ANNEEFAB) ? string.Empty : NouvoCompteur.ANNEEFAB;

            this.Txt_NouvCodeTypeCompteur.Text = (string.IsNullOrEmpty(NouvoCompteur.TYPECOMPTEUR )) ? string.Empty : NouvoCompteur.TYPECOMPTEUR;
            this.Txt_NouvLibelleTypeCompteur.Text = (string.IsNullOrEmpty(NouvoCompteur.LIBELLETYPECOMPTEUR)) ? string.Empty : NouvoCompteur.LIBELLETYPECOMPTEUR;

            this.Txt_NouvCodeMarque.Text = (string.IsNullOrEmpty(NouvoCompteur.MARQUE)) ? string.Empty : NouvoCompteur.MARQUE;
            this.Txt_NouvLibelleMarque.Text = (string.IsNullOrEmpty(NouvoCompteur.LIBELLEMARQUE)) ? string.Empty : NouvoCompteur.LIBELLEMARQUE;

            this.Txt_NouvLibelleDiametre.Text = (string.IsNullOrEmpty(NouvoCompteur.CODECALIBRECOMPTEUR)) ? string.Empty : NouvoCompteur.CODECALIBRECOMPTEUR;

            this.Txt_NouvCodeCadran.Text = NouvoCompteur.CADRAN == null ? string.Empty : NouvoCompteur.CADRAN.Value.ToString(); 

            if (this.Txt_NumCompteur.Text != this.Txt_NouvNumCompteur.Text)
            {
                this.Txt_NouvNumCompteur.FontWeight = FontWeights.ExtraBold;
                this.Txt_NouvNumCompteur.FontStyle = FontStyles.Italic;
            }
            if (this.Txt_NouvAnneeFab.Text != this.Txt_AnneeFab.Text)
            {
                this.Txt_NouvAnneeFab.FontWeight = FontWeights.ExtraBold;
                this.Txt_NouvAnneeFab.FontStyle = FontStyles.Italic;
            }
            if (this.Txt_CodeTypeCompteur.Text != this.Txt_NouvCodeTypeCompteur.Text)
            {
                this.Txt_NouvCodeTypeCompteur.FontWeight = FontWeights.ExtraBold;
                this.Txt_NouvLibelleTypeCompteur .FontWeight = FontWeights.ExtraBold;
                this.Txt_NouvCodeTypeCompteur.FontStyle = FontStyles.Italic;
                this.Txt_NouvLibelleTypeCompteur.FontStyle = FontStyles.Italic;
            }
            if (this.Txt_NouvCodeMarque.Text != this.Txt_CodeMarque.Text)
            {
                this.Txt_NouvCodeMarque.FontWeight = FontWeights.ExtraBold;
                this.Txt_NouvLibelleMarque.FontWeight = FontWeights.ExtraBold;
                this.Txt_NouvCodeMarque.FontStyle = FontStyles.Italic;
                this.Txt_NouvLibelleMarque .FontStyle = FontStyles.Italic;
            }
            if (this.Txt_NouvLibelleDiametre.Text != this.Txt_LibelleDiametre.Text)
            {
                this.Txt_NouvLibelleDiametre .FontWeight = FontWeights.ExtraBold;
                this.Txt_NouvLibelleDiametre.FontStyle = FontStyles.Italic;
            }
            if (this.Txt_NouvCodeCadran.Text != this.Txt_CodeCadran.Text)
            {
                this.Txt_NouvCodeCadran.FontWeight = FontWeights.ExtraBold;
                this.Txt_NouvCodeCadran.FontStyle = FontStyles.Italic;
            }
        }
        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {


        }
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            laDetailDemande.LeClient = null;
            laDetailDemande.Ag  = null;
            laDetailDemande.Abonne = null;
            Cloturedemande();
            this.DialogResult = false;
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
                        Message.ShowError("Erreur a la cloture de la demande", "Cloturedemande");
                };
                clientDevis.ClotureValiderDemandeAsync(laDetailDemande);

            }
            catch (Exception ex)
            {
                this.DialogResult = false;
                Message.Show(ex.Message, "Cloturedemande");
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        public event EventHandler Closed;

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

        private void Btn_Rejeter_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ClasseMEthodeGenerique.RejeterDemande(laDetailDemande);
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, "");
            }
        }
    }
}

