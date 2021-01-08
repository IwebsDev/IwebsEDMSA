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
using Galatee.Silverlight.ServiceAccueil ;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading;
using Galatee.Silverlight.Resources.Devis;
using Galatee.Silverlight.Library;
using Galatee.Silverlight.Shared;

namespace Galatee.Silverlight.Devis
{
    public partial class UcCalculFactureClient : ChildWindow
    {
        private ObjDEVIS ObjetDevisSelectionne = null;
        private ObjDEVIS MyDevis = new ObjDEVIS();
        public Galatee.Silverlight.SessionObject.ExecMode ExecMode {get;set;}
        private CsDemandeBase laDemandeSelect = null;
        private CsDemande laDetailDemande = new CsDemande();


        public UcCalculFactureClient()
        {
            InitializeComponent();
        }
        public UcCalculFactureClient( int idDevis)
        {
            InitializeComponent();
            ChargeDetailDEvis(idDevis);
            ChargerTypeDocument();
        }
        public UcCalculFactureClient(ObjDEVIS pDevis)
        {
            InitializeComponent();
            ObjetDevisSelectionne = pDevis;
            ChargerTypeDocument();
            AfficherOuMasquer(tabItemDemandeur, false);
            AfficherOuMasquer(tabItemAppareils, false);

        }
        private void ChargeDetailDEvis(int IdDemandeDevis)
        {

            AcceuilServiceClient client = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
            client.GetDevisByNumIdDevisCompleted += (ssender, args) =>
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
                {
                    laDetailDemande = args.Result;
                    #region DocumentScanne
                    if (laDetailDemande.ObjetScanne != null && laDetailDemande.ObjetScanne.Count != 0)
                    {
                        foreach (var item in laDetailDemande.ObjetScanne)
                        {
                            LstPiece.Add(item);
                            ObjetScanne.Add(item);
                        }
                        dgListePiece.ItemsSource = ObjetScanne;
                    }
                    #endregion

                    laDemandeSelect = laDetailDemande.LaDemande;
                    RenseignerInformationsDevis(laDetailDemande);
                    RenseignerInformationsDemandeDevis(laDetailDemande);
                    RenseignerInformationsEvtCanalisation(laDetailDemande);
                    LayoutRoot.Cursor = Cursors.Arrow;
                }
                LayoutRoot.Cursor = Cursors.Arrow;



            };
            client.GetDevisByNumIdDevisAsync(IdDemandeDevis);
        }
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.OKButton.IsEnabled = false;
                this.CancelButton.IsEnabled = false;
                string rdlc = string.Empty;
                Dictionary<string, string> param = new Dictionary<string, string>();

                if (laDetailDemande.LstEvenement.First().PRODUIT ==SessionObject.Enumere.ElectriciteMT)
                    rdlc = "FactureSimpleMT";
                else if (laDetailDemande.LstEvenement.First().PRODUIT ==SessionObject.Enumere.Eau)
                    rdlc = "FactureSimpleO";
                else
                    rdlc = "FactureSimple";

                string key = Utility.getKey();
                param.Add("TypeEdition", "Originale");

                string print = "Imprimé le " + DateTime.Now + " sur le poste " + SessionObject.LePosteCourant.NOMPOSTE + " par " + UserConnecte.nomUtilisateur + "(" + UserConnecte.matricule + ") du centre " + UserConnecte.LibelleCentre;
                param.Add("Print", print);

                 foreach (CsEvenement  item in laDetailDemande.LstEvenement)
                 {
                     item.LOTRI = item.CENTRE + SessionObject.Enumere.LotriTermination;
                     item.FK_IDTOURNEE = (laDetailDemande.Ag != null) ? laDetailDemande.Ag.FK_IDTOURNEE : null;
                     item.TOURNEE  = (laDetailDemande.Ag != null) ? laDetailDemande.Ag.TOURNEE : string.Empty;
                     item.ORDTOUR = (laDetailDemande.Ag != null) ? laDetailDemande.Ag.ORDTOUR : string.Empty;
                     item.CATEGORIECLIENT = (laDetailDemande.LeClient != null) ? laDetailDemande.LeClient.CATEGORIE : string.Empty;
                     item.PERFAC = (laDetailDemande.Abonne != null) ? laDetailDemande.Abonne.PERFAC : string.Empty;
                     item.STATUS = SessionObject.Enumere.EvenementReleve;
                     item.USERCREATION = UserConnecte.matricule;
                     item.MATRICULE  = UserConnecte.matricule;
                 }

                 AcceuilServiceClient clientDevis = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                clientDevis.CalculFactureResilationCompleted += (ss, b) =>
                {
                    if (b.Cancelled || b.Error != null)
                    {
                        string error = b.Error.Message;
                        Message.ShowError(error, Silverlight.Resources.Devis.Languages.txtDevis);
                        return;
                    }
                    List<CsFactureBrut> _laStat = b.Result;
                    Galatee.Silverlight.Devis.UcResultatFacturation ctrl = new Galatee.Silverlight.Devis.UcResultatFacturation(_laStat, laDetailDemande);
                    ctrl.Show();
                    //Utility.ActionImpressionDirectOrientation(SessionObject.CheminImpression, key, rdlc, "Facturation", true);
                    this.DialogResult = false;
                };
                clientDevis.CalculFactureResilationAsync(laDetailDemande.LstEvenement );
            }
            catch (Exception ex)
            {
                this.DialogResult = false;
                Message.Show(ex.Message, Languages.txtDevis);
            }
        }
        
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
        void Translate()
        {

        }
        private void Btn_Etape_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ObjetDevisSelectionne != null)
                {
                    var form = new UcSuiviDevis(ObjetDevisSelectionne);
                    form.Show();
                }
            }
            catch (Exception ex)
            {
                Message.Show(ex.Message, Languages.txtDevis);
            }
        }


        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            int resultLoding = 0;
            
        }

        private void AfficherOuMasquer(TabItem pTabItem, bool pValue)
        {
            try
            {
                pTabItem.Visibility = pValue ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void RenseignerInformationsDevis(CsDemande laDemande)
        {
            try
            {
                if (laDemande != null)
                {
                   ServiceAccueil.CsCentre leCentre = SessionObject.LstCentre.FirstOrDefault(t => t.PK_ID  == laDemande.LaDemande.FK_IDCENTRE );

                    Txt_NumeroDevis.Text = !string.IsNullOrEmpty(laDemande.LaDemande.NUMDEM) ? laDemande.LaDemande.NUMDEM : string.Empty;
                    Txt_Ordre.Text = !string.IsNullOrEmpty(laDemande.LaDemande.ORDRE.ToString()) ? laDemande.LaDemande.ORDRE.ToString() : string.Empty;
                    Txt_CodeSite.Text = !string.IsNullOrEmpty(leCentre.CODESITE) ? leCentre.CODESITE  : string.Empty;
                    Txt_CodeCentre.Text = !string.IsNullOrEmpty(laDemande.LaDemande.CENTRE) ? laDemande.LaDemande.CENTRE : string.Empty;
                    Txt_LibelleCentre.Text = !string.IsNullOrEmpty(laDemande.LaDemande.LIBELLECENTRE) ? laDemande.LaDemande.LIBELLECENTRE : string.Empty;
                    Txt_CodeProduit.Text = !string.IsNullOrEmpty(laDemande.LaDemande.PRODUIT) ? laDemande.LaDemande.PRODUIT : string.Empty;
                    Txt_LibelleSite.Text = !string.IsNullOrEmpty(leCentre.LIBELLESITE) ? leCentre.LIBELLESITE : string.Empty;
                    Txt_LibelleProduit.Text = !string.IsNullOrEmpty(laDemande.LaDemande.LIBELLEPRODUIT) ? laDemande.LaDemande.LIBELLEPRODUIT : string.Empty;
                    Txt_LibelleTypeDevis.Text = !string.IsNullOrEmpty(laDemande.LaDemande.LIBELLETYPEDEMANDE) ? laDemande.LaDemande.LIBELLETYPEDEMANDE : string.Empty;
                    txtPropriete.Text = !string.IsNullOrEmpty(laDemande.LeClient.NUMPROPRIETE) ? laDemande.LeClient.NUMPROPRIETE : string.Empty;
                    Txt_Client.Text = !string.IsNullOrEmpty(laDemande.LaDemande.CLIENT) ? laDemande.LaDemande.CLIENT : string.Empty;
                    this.Txt_EtapeCourante.Text = !string.IsNullOrEmpty(laDemande.InfoDemande.ETAPE_ACTUELLE) ? laDemande.InfoDemande.ETAPE_ACTUELLE : string.Empty;
                    this.Txt_EtapeSuivante.Text = !string.IsNullOrEmpty(laDemande.InfoDemande.ETAPE_SUIVANTE) ? laDemande.InfoDemande.ETAPE_SUIVANTE : string.Empty; 
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void RenseignerInformationsDemandeDevis(CsDemande laDemande)
        {
            try
            {
                if (laDemande != null && laDemande.LeClient != null && laDemande.Ag != null)
                {
                    Txt_NomClient.Text = !string.IsNullOrEmpty(laDemande.LeClient.NOMABON) ? laDemande.LeClient.NOMABON : string.Empty;
                    txt_Commune.Text = !string.IsNullOrEmpty(laDemande.Ag.COMMUNE) ? laDemande.Ag.COMMUNE : string.Empty;
                    txt_Quartier.Text = !string.IsNullOrEmpty(laDemande.Ag.QUARTIER) ? laDemande.Ag.QUARTIER : string.Empty;
                    txt_NumRue.Text = !string.IsNullOrEmpty(laDemande.Ag.RUE) ? laDemande.Ag.RUE : string.Empty;
                    txtAdresse.Text = !string.IsNullOrEmpty(laDemande.Branchement.ADRESSERESEAU) ? laDemande.Branchement.ADRESSERESEAU : string.Empty;
                    txtNumeroPiece.Text = !string.IsNullOrEmpty(laDemande.LeClient.NUMEROPIECEIDENTITE) ? laDemande.LeClient.NUMEROPIECEIDENTITE : string.Empty;
                    txt_Telephone.Text = !string.IsNullOrEmpty(laDemande.LeClient.TELEPHONE) ? laDemande.LeClient.TELEPHONE : string.Empty;
                    txt_NumLot.Text = !string.IsNullOrEmpty(laDemande.LeClient.NUMPROPRIETE) ? laDemande.LeClient.NUMPROPRIETE : string.Empty;
                    Txt_LibelleCommune.Text = !string.IsNullOrEmpty(laDemande.Ag.LIBELLECOMMUNE) ? laDemande.Ag.LIBELLECOMMUNE : string.Empty;
                    Txt_LibelleQuartier.Text = !string.IsNullOrEmpty(laDemande.Ag.LIBELLEQUARTIER) ? laDemande.Ag.LIBELLEQUARTIER : string.Empty;
                    Txt_LibelleRue.Text = !string.IsNullOrEmpty(laDemande.Ag.LIBELLERUE) ? laDemande.Ag.LIBELLERUE : string.Empty;
                    Txt_LibelleDiametre.Text = !string.IsNullOrEmpty(laDemande.Branchement.LIBELLETYPEBRANCHEMENT) ? laDemande.Branchement.LIBELLETYPEBRANCHEMENT : string.Empty;
                    Txt_LibelleCategorie.Text = !string.IsNullOrEmpty(laDemande.LeClient.LIBELLECATEGORIE) ? laDemande.LeClient.LIBELLECATEGORIE : string.Empty;
                    Txt_TypePiece.Text = !string.IsNullOrEmpty(laDemande.LeClient.LIBELLETYPEPIECE) ? laDemande.LeClient.LIBELLETYPEPIECE : string.Empty;
                    Txt_LibelleTournee.Text = !string.IsNullOrEmpty(laDemande.Ag.TOURNEE) ? laDemande.Ag.TOURNEE : string.Empty;

                    TxtLongitude.Text = !string.IsNullOrEmpty(laDemande.Branchement.LONGITUDE) ? laDemande.Branchement.LONGITUDE : string.Empty;
                    TxtLatitude.Text = !string.IsNullOrEmpty(laDemande.Branchement.LATITUDE) ? laDemande.Branchement.LATITUDE : string.Empty;
                    AfficherOuMasquer(tabItemDemandeur, true);
                }
                else
                    AfficherOuMasquer(tabItemDemandeur, false);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void RenseignerInformationsEvtCanalisation(CsDemande laDemande)
        {
            try
            {
                if (laDemande != null && laDemande.LstEvenement != null && laDemande.LstEvenement.Count > 0)
                {
                    AfficherOuMasquer(tabItemAppareils, true);
                    dg_compteur.ItemsSource = laDemande.LstEvenement ;
                }
                else
                    AfficherOuMasquer(tabItemAppareils, false);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private void dgListePiece_CurrentCellChanged(object sender, EventArgs e)
        {
            dgListePiece.BeginEdit();
        }
        private UcImageScanne formScanne = null;
        public ObservableCollection<ObjDOCUMENTSCANNE> LstPiece = new ObservableCollection<ObjDOCUMENTSCANNE>();
        private byte[] image, imageFraix;
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
        private List<ObjDOCUMENTSCANNE> ObjetScanne = new List<ObjDOCUMENTSCANNE>();
        public ObservableCollection<CsTypeDOCUMENTSCANNE> LstTypeDocument = new ObservableCollection<CsTypeDOCUMENTSCANNE>();

        private void GetInformationFromChildWindowImagePreuve(object sender, EventArgs e)
        {
            this.LstPiece.Add(new ObjDOCUMENTSCANNE { PK_ID = Guid.NewGuid(), NOMDOCUMENT = ((CsTypeDOCUMENTSCANNE)cbo_typedoc.SelectedItem).LIBELLE, FK_IDTYPEDOCUMENT = ((CsTypeDOCUMENTSCANNE)cbo_typedoc.SelectedItem).PK_ID, CONTENU = image, DATECREATION = DateTime.Now, DATEMODIFICATION = DateTime.Now, USERCREATION = UserConnecte.matricule, USERMODIFICATION = UserConnecte.matricule });
            this.dgListePiece.ItemsSource = this.LstPiece;
            ObjetScanne = this.LstPiece.ToList();
        }
        private void hyperlinkButtonPropScannee__Click(object sender, RoutedEventArgs e)
        {
            MemoryStream memoryStream = new MemoryStream(((HyperlinkButton)sender).Tag as byte[]);
            var ucImageScanne = new UcImageScanne(memoryStream, SessionObject.ExecMode.Modification);
            ucImageScanne.Show();
        }
        private void ChargerTypeDocument()
        {
            try
            {
                AcceuilServiceClient service = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
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
                };
                service.ChargerTypeDocumentAsync();
                service.CloseAsync();

            }
            catch (Exception)
            {

                throw;
            }
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
                    ObjetScanne = this.LstPiece.ToList();

                }
                else
                {
                    return;
                }
            };
            messageBox.Show();
        }
        DateTime DtpPose = new DateTime();
        private void DtpPose_SelectedDateChanged_1(object sender, SelectionChangedEventArgs e)
        {
            DtpPose = ((DatePicker)sender).SelectedDate.Value;
        }

        private void btn_Rejeter_Click(object sender, RoutedEventArgs e)
        {
            RejeterDemande(laDetailDemande);
        }
        public  void RejeterDemande(CsDemande laDetailDemande)
        {
            try
            {
                ServiceAccueil.CsDemande laDemande = new ServiceAccueil.CsDemande();
                laDemande.LaDemande = Utility.ConvertType<ServiceAccueil.CsDemandeBase, CsDemandeBase>(laDetailDemande.LaDemande);
                if (laDetailDemande.InfoDemande != null)
                {
                    laDemande.InfoDemande = new ServiceAccueil.CsInfoDemandeWorkflow()
                    {
                        CODE = laDetailDemande.InfoDemande.CODE,
                        CODE_DEMANDE_TABLE_TRAVAIL = laDetailDemande.InfoDemande.CODE_DEMANDE_TABLE_TRAVAIL,
                        CODETDEM = laDetailDemande.InfoDemande.CODETDEM,

                    };
                    RejeterDemande(laDemande, true);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public  void RejeterDemande(CsDemande LaDemande, bool isVerbose = false)
        {
             
            if (LaDemande.InfoDemande != null)
            {
                if (null != LaDemande.InfoDemande.LiteRejet && LaDemande.InfoDemande.LiteRejet.Count > 1)
                {
                    //Et on appelle la fenetre
                    Galatee.Silverlight.Workflow.UcWKFSelectEtape ucform = new Galatee.Silverlight.Workflow.UcWKFSelectEtape(LaDemande.InfoDemande, LaDemande.InfoDemande.LiteRejet);

                    ucform.Show();

                }
                else
                {
                    if (isVerbose)
                    {
                        Galatee.Silverlight.Workflow.UcWKFMotifRejet ucMotif = new Galatee.Silverlight.Workflow.UcWKFMotifRejet(LaDemande.InfoDemande);
                        ucMotif.Closed += (_sender, args) =>
                        {
                            SupprimeDevenement(LaDemande);
                        };
                        ucMotif.Show();
                    }
                    else
                    {
                        Galatee.Silverlight.ServiceWorkflow.WorkflowClient client = new Galatee.Silverlight.ServiceWorkflow.WorkflowClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Workflow"));
                        client.Endpoint.Binding.OpenTimeout = new TimeSpan(0, 1, 0);
                        client.Endpoint.Binding.CloseTimeout = new TimeSpan(5, 0, 0);
                        client.Endpoint.Binding.SendTimeout = new TimeSpan(5, 0, 0);
                        client.ExecuterActionSurDemandeCompleted += (sender, args) =>
                        {

                            if (args.Cancelled || args.Error != null)
                            {
                                string error = args.Error.Message;
                                Message.Show(error, "Rejet demande");
                                return;
                            }
                            if (args.Result == null)
                            {
                                Message.ShowError("", "Rejet demande");
                                return;
                            }
                            if (args.Result.StartsWith("ERR"))
                            {
                                Message.ShowError(args.Result, "Rejet demande");
                            }

                        };
                        client.ExecuterActionSurDemandeAsync(LaDemande.InfoDemande.CODE, Galatee.Silverlight.SessionObject.Enumere.REJETER, UserConnecte.matricule, "Rejet");
                    }
                }

            }
        }
        private void SupprimeDevenement(CsDemande laDemande)
        {
            try
            {
                AcceuilServiceClient service = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.SupprimerDevenementCompleted += (s, args) =>
                {
                    if ((args != null && args.Cancelled) || (args.Error != null))
                        return;
                     
                };
                service.SupprimerDevenementAsync(laDemande);
                service.CloseAsync();

            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}

