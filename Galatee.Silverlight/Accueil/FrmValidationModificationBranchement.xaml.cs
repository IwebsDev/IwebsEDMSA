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
    public partial class FrmValidationModificationBranchement : ChildWindow
    {
        private CsDemande laDetailDemande = new CsDemande();
        public ObservableCollection<ObjDOCUMENTSCANNE> LstPiece = new ObservableCollection<ObjDOCUMENTSCANNE>();
        public ObservableCollection<CsTypeDOCUMENTSCANNE> LstTypeDocument = new ObservableCollection<CsTypeDOCUMENTSCANNE>();
        private List<ObjDOCUMENTSCANNE> ObjetScanne = new List<ObjDOCUMENTSCANNE>();

        private UcImageScanne formScanne = null;

        private byte[] image;
        public FrmValidationModificationBranchement(int iddemande)
        {
            try
            {
                InitializeComponent();
                ChargeDetailDEvis(iddemande);
                ChargerTypeDocument();
                prgBar.Visibility = System.Windows.Visibility.Visible;
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, "Demande");
            }
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
                    RetourneInfoAdresseduClient(leCleint);
                }
                 
            };
            client.ChargerDetailDemandeAsync (IdDemandeDevis, string.Empty );
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
                    AfficherInfoAbonnement(laDetailDemande.Branchement , laDDe.Branchement );
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
        int InitValue = 0;
        private void AfficherInfoAbonnement(CsBrt _NouvelInfoBrt, CsBrt _LeBrtDemande)
        {
            try
            {
                this.Txt_CodeCentre.Text = string.IsNullOrEmpty(laDetailDemande.LaDemande.CENTRE) ? string.Empty : laDetailDemande.LaDemande.CENTRE;
                this.Txt_LibelleCentre.Text = string.IsNullOrEmpty(laDetailDemande.LaDemande.LIBELLECENTRE) ? string.Empty : laDetailDemande.LaDemande.LIBELLECENTRE;
                this.Txt_CodeProduit.Text = string.IsNullOrEmpty(laDetailDemande.LaDemande.PRODUIT) ? string.Empty : laDetailDemande.LaDemande.PRODUIT;
                this.Txt_LibelleProduit.Text = string.IsNullOrEmpty(laDetailDemande.LaDemande.LIBELLEPRODUIT) ? string.Empty : laDetailDemande.LaDemande.LIBELLEPRODUIT;
                this.Txt_NumDemande.Text = string.IsNullOrEmpty(laDetailDemande.LaDemande.NUMDEM) ? string.Empty : laDetailDemande.LaDemande.NUMDEM;
                this.Txt_Client.Text = string.IsNullOrEmpty(laDetailDemande.LaDemande.CLIENT) ? string.Empty : laDetailDemande.LaDemande.CLIENT;

                this.Txt_TypeBrancehment.Text = string.IsNullOrEmpty(_LeBrtDemande.CODETYPEBRANCHEMENT) ? string.Empty : _LeBrtDemande.CODETYPEBRANCHEMENT;
                this.Txt_LibelleTypeBRT.Text = string.IsNullOrEmpty(_LeBrtDemande.LIBELLETYPEBRANCHEMENT) ? string.Empty : _LeBrtDemande.LIBELLETYPEBRANCHEMENT;
                this.Txt_NbrTransformateur.Text = (_LeBrtDemande.NOMBRETRANSFORMATEUR == null) ? string.Empty : _LeBrtDemande.NOMBRETRANSFORMATEUR.Value.ToString();

                this.Txt_LongueurBrt.Text = string.IsNullOrEmpty(_LeBrtDemande.LONGBRT.ToString()) ? InitValue.ToString() : _LeBrtDemande.LONGBRT.ToString();
                this.Txt_DateRacordement.Text = string.IsNullOrEmpty(_LeBrtDemande.DRAC.ToString()) ? string.Empty : Convert.ToDateTime(_LeBrtDemande.DRAC).ToShortDateString();
                this.Txt_Longitude.Text = string.IsNullOrEmpty(_LeBrtDemande.LONGITUDE) ? string.Empty : _LeBrtDemande.LONGITUDE;
                this.Txt_Latitude.Text = string.IsNullOrEmpty(_LeBrtDemande.LATITUDE) ? string.Empty : _LeBrtDemande.LATITUDE;
                this.Txt_AdresseElectrique.Text = string.IsNullOrEmpty(_LeBrtDemande.ADRESSERESEAU) ? string.Empty : _LeBrtDemande.ADRESSERESEAU;


                this.Txt_PosteSource.Text = string.IsNullOrEmpty(_LeBrtDemande.CODEPOSTESOURCE) ? string.Empty : _LeBrtDemande.CODEPOSTESOURCE;
                this.Txt_LibellePosteSource.Text = string.IsNullOrEmpty(_LeBrtDemande.LIBELLEPOSTESOURCE) ? string.Empty : _LeBrtDemande.LIBELLEPOSTESOURCE;

                this.Txt_PosteTransformateur.Text = string.IsNullOrEmpty(_LeBrtDemande.CODETRANSFORMATEUR) ? string.Empty : _LeBrtDemande.CODETRANSFORMATEUR;
                this.Txt_LibellePosteTransformateur.Text = string.IsNullOrEmpty(_LeBrtDemande.LIBELLETRANSFORMATEUR) ? string.Empty : _LeBrtDemande.LIBELLETRANSFORMATEUR;

                this.Txt_DepartBt.Text = string.IsNullOrEmpty(_LeBrtDemande.DEPARTBT) ? string.Empty : _LeBrtDemande.DEPARTBT;

                this.Txt_DepartHTA.Text = string.IsNullOrEmpty(_LeBrtDemande.CODEDEPARTHTA) ? string.Empty : _LeBrtDemande.CODEDEPARTHTA;
                this.Txt_LibelleDepartHTA.Text = string.IsNullOrEmpty(_LeBrtDemande.LIBELLEDEPARTHTA) ? string.Empty : _LeBrtDemande.LIBELLEDEPARTHTA;


                this.Txt_QuarteirPoste.Text = string.IsNullOrEmpty(_LeBrtDemande.CODEQUARTIER) ? string.Empty : _LeBrtDemande.CODEQUARTIER;
                this.Txt_LibelleQuartier.Text = string.IsNullOrEmpty(_LeBrtDemande.LIBELLEQUARTIER) ? string.Empty : _LeBrtDemande.LIBELLEQUARTIER;

                this.Txt_NeoudFinal.Text = string.IsNullOrEmpty(_LeBrtDemande.NEOUDFINAL) ? string.Empty : _LeBrtDemande.NEOUDFINAL;

                this.Txt_NouvTypeBrancehment.Text = string.IsNullOrEmpty(_NouvelInfoBrt.CODETYPEBRANCHEMENT) ? string.Empty : _NouvelInfoBrt.CODETYPEBRANCHEMENT;
                this.Txt_NouvLibelleTypeBRT.Text = string.IsNullOrEmpty(_NouvelInfoBrt.LIBELLETYPEBRANCHEMENT) ? string.Empty : _NouvelInfoBrt.LIBELLETYPEBRANCHEMENT;
                this.Txt_NouvNombreTransformateur.Text = (_NouvelInfoBrt.NOMBRETRANSFORMATEUR == null) ? string.Empty : _NouvelInfoBrt.NOMBRETRANSFORMATEUR.Value.ToString();

                this.Txt_NouvLongueurBrt.Text = string.IsNullOrEmpty(_NouvelInfoBrt.LONGBRT.ToString()) ? InitValue.ToString() : _NouvelInfoBrt.LONGBRT.ToString();
                this.Txt_NouvDateRacordement.Text = string.IsNullOrEmpty(_NouvelInfoBrt.DRAC.ToString()) ? string.Empty : Convert.ToDateTime(_NouvelInfoBrt.DRAC).ToShortDateString();
                this.Txt_NouvLongitude.Text = string.IsNullOrEmpty(_NouvelInfoBrt.LONGITUDE) ? string.Empty : _NouvelInfoBrt.LONGITUDE;
                this.Txt_NouvLatitude.Text = string.IsNullOrEmpty(_NouvelInfoBrt.LATITUDE) ? string.Empty : _NouvelInfoBrt.LATITUDE;
                this.Txt_NouvAdresseElectrique.Text = string.IsNullOrEmpty(_NouvelInfoBrt.ADRESSERESEAU) ? string.Empty : _NouvelInfoBrt.ADRESSERESEAU;


                this.Txt_NouvPosteSource.Text = string.IsNullOrEmpty(_NouvelInfoBrt.CODEPOSTESOURCE) ? string.Empty : _NouvelInfoBrt.CODEPOSTESOURCE;
                this.Txt_NouvLibellePosteSource.Text = string.IsNullOrEmpty(_NouvelInfoBrt.LIBELLEPOSTESOURCE) ? string.Empty : _NouvelInfoBrt.LIBELLEPOSTESOURCE;

                this.Txt_NouvPosteTransformateur.Text = string.IsNullOrEmpty(_NouvelInfoBrt.CODETRANSFORMATEUR) ? string.Empty : _NouvelInfoBrt.CODETRANSFORMATEUR;
                this.Txt_NouvLibellePosteTransformateur.Text = string.IsNullOrEmpty(_NouvelInfoBrt.LIBELLETRANSFORMATEUR) ? string.Empty : _NouvelInfoBrt.LIBELLETRANSFORMATEUR;

                this.Txt_NouvDepartBt.Text = string.IsNullOrEmpty(_NouvelInfoBrt.DEPARTBT) ? string.Empty : _NouvelInfoBrt.DEPARTBT;

                this.Txt_NouvDepartHTA.Text = string.IsNullOrEmpty(_NouvelInfoBrt.CODEDEPARTHTA) ? string.Empty : _NouvelInfoBrt.CODEDEPARTHTA;
                this.Txt_NouvLibelleDepartHTA.Text = string.IsNullOrEmpty(_NouvelInfoBrt.LIBELLEDEPARTHTA) ? string.Empty : _NouvelInfoBrt.LIBELLEDEPARTHTA;

                this.Txt_NouvQuarteirPoste.Text = string.IsNullOrEmpty(_NouvelInfoBrt.CODEQUARTIER) ? string.Empty : _NouvelInfoBrt.CODEQUARTIER;
                this.Txt_NouvLibelleQuartier.Text = string.IsNullOrEmpty(_NouvelInfoBrt.LIBELLEQUARTIER) ? string.Empty : _NouvelInfoBrt.LIBELLEQUARTIER;

                this.Txt_NouvNeoudFinal.Text = string.IsNullOrEmpty(_NouvelInfoBrt.NEOUDFINAL) ? string.Empty : _NouvelInfoBrt.NEOUDFINAL;
                this.Txt_NouvPuissanceInstalle.Text = _NouvelInfoBrt.PUISSANCEINSTALLEE == null ? string.Empty : _NouvelInfoBrt.PUISSANCEINSTALLEE.Value.ToString("N2");
                this.Txt_PuissanceInstalle.Text = _LeBrtDemande.PUISSANCEINSTALLEE == null ? string.Empty : _LeBrtDemande.PUISSANCEINSTALLEE.Value.ToString("N2");

                if (this.Txt_TypeBrancehment.Text != this.Txt_NouvTypeBrancehment.Text)
                {
                    this.Txt_NouvTypeBrancehment.FontWeight = FontWeights.ExtraBold;
                    this.Txt_NouvLibelleTypeBRT.FontWeight = FontWeights.ExtraBold;
                    this.Txt_NouvTypeBrancehment.FontStyle = FontStyles.Italic;
                    this.Txt_NouvLibelleTypeBRT.FontStyle = FontStyles.Italic;
                }
                if (this.Txt_LongueurBrt.Text != this.Txt_NouvLongueurBrt.Text)
                {
                    this.Txt_NouvLongueurBrt.FontWeight = FontWeights.ExtraBold;
                    this.Txt_NouvLongueurBrt.FontStyle = FontStyles.Italic;
                }
                if (this.Txt_DateRacordement.Text != this.Txt_NouvDateRacordement.Text)
                {
                    this.Txt_NouvDateRacordement.FontWeight = FontWeights.ExtraBold;
                    this.Txt_NouvDateRacordement.FontStyle = FontStyles.Italic;
                }
                if (this.Txt_DateRacordement.Text != this.Txt_NouvDateRacordement.Text)
                {
                    this.Txt_NouvDateRacordement.FontWeight = FontWeights.ExtraBold;
                    this.Txt_NouvDateRacordement.FontStyle = FontStyles.Italic;
                }
                if (this.Txt_Longitude.Text != this.Txt_NouvLongitude.Text)
                {
                    this.Txt_NouvLongitude.FontWeight = FontWeights.ExtraBold;
                    this.Txt_NouvLongitude.FontStyle = FontStyles.Italic;
                }
                if (this.Txt_Latitude.Text != this.Txt_NouvLatitude.Text)
                {
                    this.Txt_NouvLatitude.FontWeight = FontWeights.ExtraBold;
                    this.Txt_NouvLatitude.FontStyle = FontStyles.Italic;
                }
                if (this.Txt_PuissanceInstalle.Text != this.Txt_NouvPuissanceInstalle.Text)
                {
                    this.Txt_NouvPuissanceInstalle.FontWeight = FontWeights.ExtraBold;
                    this.Txt_NouvPuissanceInstalle.FontStyle = FontStyles.Italic;
                }

                if (this.Txt_NouvNombreTransformateur.Text != this.Txt_NbrTransformateur.Text)
                {
                    this.Txt_NouvNombreTransformateur.FontWeight = FontWeights.ExtraBold;
                    this.Txt_NouvNombreTransformateur.FontStyle = FontStyles.Italic;
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

