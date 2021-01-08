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
using Galatee.Silverlight.Resources.Accueil;
//using Galatee.Silverlight.serviceWeb;
using Galatee.Silverlight.MainView;
using Galatee.Silverlight.Shared;
using System.IO;
using System.Collections.ObjectModel;

namespace Galatee.Silverlight.Accueil
{
    public partial class FrmValidationModificationAdresse : ChildWindow
    {
        private CsDemande laDetailDemande = new CsDemande();
        public ObservableCollection<ObjDOCUMENTSCANNE> LstPiece = new ObservableCollection<ObjDOCUMENTSCANNE>();
        public ObservableCollection<CsTypeDOCUMENTSCANNE> LstTypeDocument = new ObservableCollection<CsTypeDOCUMENTSCANNE>();
        private List<ObjDOCUMENTSCANNE> ObjetScanne = new List<ObjDOCUMENTSCANNE>();

        private UcImageScanne formScanne = null;

        private byte[] image;
        public FrmValidationModificationAdresse()
        {
            InitializeComponent();
        }
        void Translate()
        {
            //Gestion de la langue
            this.lbl_Commune.Content =this.lbl_NouvCommune.Content = Langue.lbl_Commune;
            this.lbl_Etage.Content =this.lbl_NouvEtage.Content = Langue.lbl_Etage;
            this.lbl_NomProprietaire.Content =this.lbl_NouvNomProprietaire.Content= Langue.lbl_NomProprietaire;
            this.lbl_NumRue.Content =this.lbl_NouvNumRue.Content = Langue.lbl_NumRue;
            this.lbl_Quartier.Content = this.lbl_NouvQuartier.Content = Langue.lbl_Quartier;
            this.lbl_Rue.Content =this.lbl_NouvRue.Content = Langue.lbl_Rue;
            this.lbl_Secteur.Content =  this.lbl_NouvSecteur.Content = Langue.lbl_Secteur;
            this.lbl_Sequence.Content = this.lbl_NouvSequence.Content = Langue.lbl_OrdreTourne;
            this.lbl_Tournee.Content =this.lbl_Tournee_Nouv.Content = Langue.lbl_Tournee;
        }
        public FrmValidationModificationAdresse(CsDemande _LaDemande)
        {
            InitializeComponent();
            Translate();
        }
        public FrmValidationModificationAdresse(int iddemande)
        {
            InitializeComponent();
            Translate();
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
                    this.Txt_Motif.Text = string.IsNullOrEmpty(laDetailDemande.LaDemande.MOTIF) ? string.Empty  : laDetailDemande.LaDemande.MOTIF;
                    ServiceAccueil.CsClient leCleint = new CsClient() 
                    {
                     CENTRE = laDetailDemande.LaDemande.CENTRE ,
                     REFCLIENT = laDetailDemande.LaDemande.CLIENT ,
                     ORDRE = laDetailDemande.LaDemande.ORDRE ,
                     FK_IDCENTRE = laDetailDemande.LaDemande.FK_IDCENTRE ,
                     TYPEDEMANDE = laDetailDemande.LaDemande.TYPEDEMANDE 
                    };
                    RetourneInfoAdresseduClient(leCleint);
                }
                LayoutRoot.Cursor = Cursors.Arrow;
            };
            client.GetDevisByNumIdDevisAsync(IdDemandeDevis);
        }
        ServiceAccueil.CsDemande laDDe = new CsDemande();
        private void RetourneInfoAdresseduClient(CsClient leClient)
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
                        laDDe = args.Result;
                    AfficherInfoNouvelleAdresse(laDetailDemande.Ag, laDDe.Ag);
                    AfficherDocumentScanne(laDetailDemande.ObjetScanne);

                    laDetailDemande.LeClient = null;

                };
                client.GeDetailByFromClientAsync(leClient);
            }
            catch (Exception)
            {

                throw;
            }
        }
        private void AfficherInfoNouvelleAdresse(CsAg leNouvAg, CsAg AdresseDemande)
        {
            try
            {
                this.Txt_NumDemande.Text = string.IsNullOrEmpty(leNouvAg.NUMDEM) ? string.Empty : leNouvAg.NUMDEM;
                this.Txt_Client.Text = string.IsNullOrEmpty(leNouvAg.CENTRE) ? string.Empty : leNouvAg.CENTRE;
                this.Txt_Ordre.Text = string.IsNullOrEmpty(laDetailDemande.LaDemande.ORDRE) ? string.Empty : laDetailDemande.LaDemande.ORDRE;

                this.Txt_CodeCentre.Text = string.IsNullOrEmpty(laDetailDemande.LaDemande.CENTRE) ? string.Empty : laDetailDemande.LaDemande.CENTRE;
                this.Txt_LibelleCentre.Text = string.IsNullOrEmpty(laDetailDemande.LaDemande.LIBELLECENTRE) ? string.Empty : laDetailDemande.LaDemande.LIBELLECENTRE;


                this.Txt_CodeProduit.Text = string.IsNullOrEmpty(laDetailDemande.LaDemande.PRODUIT) ? string.Empty : laDetailDemande.LaDemande.PRODUIT;
                this.Txt_LibelleProduit.Text = string.IsNullOrEmpty(laDetailDemande.LaDemande.LIBELLEPRODUIT) ? string.Empty : laDetailDemande.LaDemande.LIBELLEPRODUIT;

                this.Txt_Client.Text = string.IsNullOrEmpty(AdresseDemande.CLIENT) ? string.Empty : AdresseDemande.CLIENT;
                this.Txt_NomClient.Text = string.IsNullOrEmpty(AdresseDemande.NOMP) ? string.Empty : AdresseDemande.NOMP;

                this.Txt_CodeCommune.Text = string.IsNullOrEmpty(AdresseDemande.COMMUNE) ? string.Empty : AdresseDemande.COMMUNE;
                this.Txt_CodeCommune.Tag = AdresseDemande.FK_IDCOMMUNE == null ? null : (int?)AdresseDemande.FK_IDCOMMUNE;
                this.Txt_LibelleCommune.Text = string.IsNullOrEmpty(AdresseDemande.LIBELLECOMMUNE) ? string.Empty : AdresseDemande.LIBELLECOMMUNE;

                this.Txt_CodeQuartier.Text = string.IsNullOrEmpty(AdresseDemande.QUARTIER) ? string.Empty : AdresseDemande.QUARTIER;
                this.Txt_LibelleQuartier.Text = string.IsNullOrEmpty(AdresseDemande.LIBELLEQUARTIER) ? string.Empty : AdresseDemande.LIBELLEQUARTIER;
                this.Txt_CodeQuartier.Tag = AdresseDemande.FK_IDQUARTIER == null ? null : (int?)AdresseDemande.FK_IDQUARTIER;

                this.Txt_CodeSecteur.Text = string.IsNullOrEmpty(AdresseDemande.SECTEUR) ? string.Empty : AdresseDemande.SECTEUR;
                this.Txt_LibelleSecteur.Text = string.IsNullOrEmpty(AdresseDemande.LIBELLESECTEUR) ? string.Empty : AdresseDemande.LIBELLESECTEUR;
                this.Txt_CodeSecteur.Tag = AdresseDemande.FK_IDSECTEUR == null ? null : (int?)AdresseDemande.FK_IDSECTEUR;

                this.Txt_CodeNomRue.Text = string.IsNullOrEmpty(AdresseDemande.RUE) ? string.Empty : AdresseDemande.RUE;
                this.Txt_CodeNomRue.Tag = AdresseDemande.FK_IDRUE == null ? null : (int?)AdresseDemande.FK_IDRUE;


                this.Txt_Etage.Text = string.IsNullOrEmpty(AdresseDemande.ETAGE) ? string.Empty : AdresseDemande.ETAGE;
                this.Txt_OrdreTour.Text = string.IsNullOrEmpty(AdresseDemande.ORDTOUR) ? string.Empty : AdresseDemande.ORDTOUR;
                this.Txt_Tournee.Text = string.IsNullOrEmpty(AdresseDemande.TOURNEE) ? string.Empty : AdresseDemande.TOURNEE;
                this.Txt_Tournee.Tag = AdresseDemande.FK_IDTOURNEE == null ? null : (int?)AdresseDemande.FK_IDTOURNEE;

                this.Txt_Porte.Text = string.IsNullOrEmpty(AdresseDemande.PORTE) ? string.Empty : AdresseDemande.PORTE;



                this.Txt_NouvNomClient.Text = string.IsNullOrEmpty(leNouvAg.NOMP) ? string.Empty : leNouvAg.NOMP;

                this.Txt_NouvCodeCommune.Text = string.IsNullOrEmpty(leNouvAg.COMMUNE) ? string.Empty : leNouvAg.COMMUNE;
                this.Txt_NouvCodeCommune.Tag = leNouvAg.FK_IDCOMMUNE == null ? null : (int?)leNouvAg.FK_IDCOMMUNE;
                this.Txt_NouvLibelleCommune.Text = string.IsNullOrEmpty(leNouvAg.LIBELLECOMMUNE) ? string.Empty : leNouvAg.LIBELLECOMMUNE;

                this.Txt_NouvCodeQuartier.Text = string.IsNullOrEmpty(leNouvAg.QUARTIER) ? string.Empty : leNouvAg.QUARTIER;
                this.Txt_NouvLibelleQuartier.Text = string.IsNullOrEmpty(leNouvAg.LIBELLEQUARTIER) ? string.Empty : leNouvAg.LIBELLEQUARTIER;
                this.Txt_NouvCodeQuartier.Tag = leNouvAg.FK_IDQUARTIER == null ? null : (int?)leNouvAg.FK_IDQUARTIER;

                this.Txt_NouvCodeSecteur.Text = string.IsNullOrEmpty(leNouvAg.SECTEUR) ? string.Empty : leNouvAg.SECTEUR;
                this.Txt_NouvLibelleSecteur.Text = string.IsNullOrEmpty(leNouvAg.LIBELLESECTEUR) ? string.Empty : leNouvAg.LIBELLESECTEUR;
                this.Txt_NouvCodeSecteur.Tag = AdresseDemande.FK_IDSECTEUR == null ? null : (int?)leNouvAg.FK_IDSECTEUR;

                this.Txt_NouvCodeNomRue.Text = string.IsNullOrEmpty(leNouvAg.RUE) ? string.Empty : leNouvAg.RUE;
                this.Txt_NouvCodeNomRue.Tag = leNouvAg.FK_IDRUE == null ? null : (int?)leNouvAg.FK_IDRUE;


                this.Txt_NouvEtage.Text = string.IsNullOrEmpty(leNouvAg.ETAGE) ? string.Empty : leNouvAg.ETAGE;
                this.Txt_NouvOrdreTour.Text = string.IsNullOrEmpty(leNouvAg.ORDTOUR) ? string.Empty : leNouvAg.ORDTOUR;
                this.Txt_NouvTournee.Text = string.IsNullOrEmpty(leNouvAg.TOURNEE) ? string.Empty : leNouvAg.TOURNEE;
                this.Txt_NouvTournee.Tag = leNouvAg.FK_IDTOURNEE == null ? null : (int?)leNouvAg.FK_IDTOURNEE;

                this.Txt_NouvPorte.Text = string.IsNullOrEmpty(leNouvAg.PORTE) ? string.Empty : leNouvAg.PORTE;

                #region DocumentScanne
                if (laDetailDemande.ObjetScanne != null && laDetailDemande.ObjetScanne.Count != 0)
                {
                    foreach (var item in laDetailDemande.ObjetScanne)
                        ObjetScanne.Add(item);
                    dgListePiece.ItemsSource = ObjetScanne;
                }
                #endregion

                if (this.Txt_NouvTournee.Text != this.Txt_Tournee.Text)
                {
                    this.Txt_NouvTournee.FontWeight = FontWeights.ExtraBold;
                    this.Txt_NouvTournee.FontStyle = FontStyles.Italic;
                }
                if (this.Txt_NomClient.Text != this.Txt_NouvNomClient.Text)
                {
                    this.Txt_NouvNomClient.FontWeight = FontWeights.ExtraBold;
                    this.Txt_NouvNomClient.FontStyle = FontStyles.Italic;
                }
                if (this.Txt_CodeCommune.Text != this.Txt_NouvCodeCommune.Text)
                {
                    this.Txt_NouvCodeCommune.FontWeight = FontWeights.ExtraBold;
                    this.Txt_NouvLibelleCommune .FontWeight = FontWeights.ExtraBold;
                    this.Txt_NouvLibelleCommune .FontStyle = FontStyles.Italic;
                    this.Txt_NouvCodeCommune.FontStyle = FontStyles.Italic;
                }
                if (this.Txt_CodeQuartier.Text != this.Txt_NouvCodeQuartier.Text)
                {
                    this.Txt_NouvCodeQuartier.FontWeight = FontWeights.ExtraBold;
                    this.Txt_NouvLibelleQuartier .FontWeight = FontWeights.ExtraBold;
                    this.Txt_NouvLibelleQuartier .FontStyle = FontStyles.Italic;
                    this.Txt_NouvCodeQuartier.FontStyle = FontStyles.Italic;
                }
                if (this.Txt_CodeSecteur.Text != this.Txt_NouvCodeSecteur.Text)
                {
                    this.Txt_NouvCodeSecteur.FontWeight = FontWeights.ExtraBold;
                    this.Txt_NouvLibelleSecteur .FontWeight = FontWeights.ExtraBold;
                    this.Txt_NouvLibelleSecteur.FontStyle = FontStyles.Italic;
                    this.Txt_NouvCodeSecteur.FontStyle = FontStyles.Italic;
                }
                if (this.Txt_CodeNomRue.Text != this.Txt_NouvCodeNomRue.Text)
                {
                    this.Txt_NouvCodeNomRue.FontWeight = FontWeights.ExtraBold;
                    this.Txt_NouvCodeNomRue.FontStyle = FontStyles.Italic;
                }
                if (this.Txt_Etage.Text != this.Txt_NouvEtage.Text)
                {
                    this.Txt_NouvEtage.FontWeight = FontWeights.ExtraBold;
                    this.Txt_NouvEtage.FontWeight = FontWeights.ExtraBold;

                    this.Txt_NouvEtage.FontStyle = FontStyles.Italic;
                    this.Txt_NouvEtage.FontStyle = FontStyles.Italic;
                }
                if (this.Txt_Porte.Text != this.Txt_NouvPorte.Text)
                {
                    this.Txt_NouvPorte.FontWeight = FontWeights.ExtraBold;
                    this.Txt_NouvPorte.FontWeight = FontWeights.ExtraBold;
                    this.Txt_NouvPorte.FontStyle = FontStyles.Italic;
                    this.Txt_NouvPorte.FontStyle = FontStyles.Italic;
                }
                if (this.Txt_OrdreTour.Text != this.Txt_NouvOrdreTour .Text)
                {
                    this.Txt_NouvOrdreTour.FontWeight = FontWeights.ExtraBold;
                    this.Txt_NouvOrdreTour.FontWeight = FontWeights.ExtraBold;
                    this.Txt_NouvOrdreTour.FontStyle = FontStyles.Italic;
                    this.Txt_NouvOrdreTour.FontStyle = FontStyles.Italic;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }


        }
        private void ActionControle(bool Etat)
        {

            this.Txt_NomClient.IsEnabled = Etat;
            this.Txt_CodeCommune.IsEnabled = Etat;
            this.Txt_LibelleCommune.IsEnabled = Etat;
            this.Txt_CodeQuartier.IsEnabled = Etat;
            this.Txt_LibelleQuartier.IsEnabled = Etat;
            this.Txt_CodeSecteur.IsEnabled = Etat;
            this.Txt_LibelleSecteur.IsEnabled = Etat;
            this.Txt_CodeNomRue.IsEnabled = Etat;
            this.Txt_Etage.IsEnabled = Etat;

            this.Txt_NumRue.IsEnabled = Etat;
            this.Txt_OrdreTour.IsEnabled = Etat;
            this.Txt_Tournee.IsEnabled = Etat;
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
        private void dgListePiece_CurrentCellChanged(object sender, EventArgs e)
        {
            dgListePiece.BeginEdit();
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
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            ClasseMEthodeGenerique.FermetureEcran(this);
        }
        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
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
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            Cloturedemande();  
        }
 
        public event EventHandler Closed;

        private void Btn_Rejeter_Click(object sender, RoutedEventArgs e)
        {
            ClasseMEthodeGenerique.RejeterDemande(laDetailDemande);
        }

    }
}

