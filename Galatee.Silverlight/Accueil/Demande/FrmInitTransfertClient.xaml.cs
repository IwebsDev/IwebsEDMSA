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
using System.Text.RegularExpressions;

namespace Galatee.Silverlight.Accueil
{
    public partial class FrmInitTransfertClient : ChildWindow
    {
        public Galatee.Silverlight.SessionObject.ExecMode ExecMode {get;set;}
        int IdDemandeDevis = 0;
        CsDemandeBase laDemandeSelect = null;
        bool isPreuveSelectionnee = false;
        private UcImageScanne formScanne = null;
        public ObservableCollection<CsTypeDOCUMENTSCANNE> LstTypeDocument = new ObservableCollection<CsTypeDOCUMENTSCANNE>();
        CsDemande laDetailDemande = null;

        private List<CsStatutJuridique> ListStatuJuridique = new List<CsStatutJuridique>();
        public List<CsCATEGORIECLIENT_TYPECLIENT> LstCategorieClient_TypeClient = new List<CsCATEGORIECLIENT_TYPECLIENT>();
        public List<CsNATURECLIENT_TYPECLIENT> LstNatureClient_TypeClient = new List<CsNATURECLIENT_TYPECLIENT>();
        public List<CsUSAGE_NATURECLIENT> LstUsage_NatureClient = new List<CsUSAGE_NATURECLIENT>();
        public List<CsCATEGORIECLIENT_USAGE> LstCategorieClient_Usage = new List<CsCATEGORIECLIENT_USAGE>();

        public FrmInitTransfertClient()
        {
            InitializeComponent();
            ChargerListDesSite();
            ChargerTypeDocument();
            Tdem = SessionObject.Enumere.TransfertAbonnement;
            List<string> ListOperation = new List<string>();
            ListOperation = SessionObject.TypeOperationClasseur().ToList();
            tab4_cbo_Operation.ItemsSource = null;
            tab4_cbo_Operation.ItemsSource = ListOperation;
            if (ListOperation != null && ListOperation.Count != 0)
                tab4_cbo_Operation.SelectedItem = ListOperation[0];
            //this.Rdb_VersGrandCompte.IsChecked = true;
            prgBar.Visibility = System.Windows.Visibility.Collapsed;
            this.Txt_CodeRegroupement.MaxLength = SessionObject.Enumere.TailleCodeRegroupement;
            RemplirCodeRegroupement();
            ChargerCategorieClient_TypeClient();
            ChargerNatureClient_TypeClient();
            ChargerUsage_NatureClient();
            ChargerCategorieClient_Usage();
            RemplirStatutJuridique();
            Txt_ReferenceClient.MaxLength = SessionObject.Enumere.TailleClient;
        }
        public FrmInitTransfertClient(string TypeDemande)
        {
            InitializeComponent();
            ChargerTypeDocument();
            Tdem = SessionObject.Enumere.TransfertAbonnement;
        }
        string Tdem = string.Empty;
        public FrmInitTransfertClient(string TypeDemande,string IsInit)
        {
            InitializeComponent();
            ChargerTypeDocument();
            Tdem = SessionObject.Enumere.TransfertAbonnement ;

        }
  
        private void ChargerCategorieClient_TypeClient()
        {
            try
            {
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.ChargerCategorieClient_TypeClientCompleted += (s, args) =>
                {
                    if ((args != null && args.Cancelled) || (args.Error != null))
                        return;

                    foreach (var item in args.Result)
                    {
                        LstCategorieClient_TypeClient.Add(item);
                    }
                    //this.Resources.Add("FuelList", LstTypeDocument);

                };
                service.ChargerCategorieClient_TypeClientAsync();
                service.CloseAsync();

            }
            catch (Exception)
            {

                throw;
            }
        }
        private void ChargerNatureClient_TypeClient()
        {
            try
            {
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.ChargerNatureClient_TypeClientCompleted += (s, args) =>
                {
                    if ((args != null && args.Cancelled) || (args.Error != null))
                        return;

                    foreach (var item in args.Result)
                    {
                        LstNatureClient_TypeClient.Add(item);
                    }
                    //this.Resources.Add("FuelList", LstTypeDocument);

                };
                service.ChargerNatureClient_TypeClientAsync();
                service.CloseAsync();

            }
            catch (Exception)
            {

                throw;
            }
        }
        private void ChargerUsage_NatureClient()
        {
            try
            {
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.ChargerUsage_NatureClientCompleted += (s, args) =>
                {
                    if ((args != null && args.Cancelled) || (args.Error != null))
                        return;

                    foreach (var item in args.Result)
                    {
                        LstUsage_NatureClient.Add(item);
                    }
                    //this.Resources.Add("FuelList", LstTypeDocument);

                };
                service.ChargerUsage_NatureClientAsync();
                service.CloseAsync();

            }
            catch (Exception)
            {

                throw;
            }
        }
        private void ChargerCategorieClient_Usage()
        {
            try
            {
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));

                service.ChargerCategorieClient_UsageCompleted += (s, args) =>
                {
                    if ((args != null && args.Cancelled) || (args.Error != null))
                        return;

                    foreach (var item in args.Result)
                    {
                        LstCategorieClient_Usage.Add(item);
                    }
                    //this.Resources.Add("FuelList", LstTypeDocument);

                };
                service.ChargerCategorieClient_UsageAsync();
                service.CloseAsync();

            }
            catch (Exception)
            {

                throw;
            }
        }
        private void RemplirStatutJuridique()
        {
            try
            {
                LayoutRoot.Cursor = Cursors.Wait;
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.GetAllStatutJuridiqueAsync();
                service.GetAllStatutJuridiqueCompleted += (ssender, args) =>
                {
                    if (args.Cancelled || args.Error != null)
                    {
                        LayoutRoot.Cursor = Cursors.Arrow;
                        string error = args.Error.Message;
                        Message.ShowError(error, Galatee.Silverlight.Resources.Devis.Languages.txtDevis);
                        return;
                    }
                    Cbo_StatutJuridique.Items.Clear();
                    if (args.Result != null && args.Result.Count > 0)
                        foreach (var item in args.Result)
                        {
                            Cbo_StatutJuridique.Items.Add(item);
                        }
                    ListStatuJuridique = args.Result;
                    Cbo_StatutJuridique.SelectedValuePath = "PK_ID";
                    Cbo_StatutJuridique.DisplayMemberPath = "LIBELLE";

                    LayoutRoot.Cursor = Cursors.Arrow;
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        List<ServiceAccueil.CsCentre> lesCentre = new List<ServiceAccueil.CsCentre>();
        List<ServiceAccueil.CsSite> lesSite = new List<ServiceAccueil.CsSite>();
        List<int> lstIdCentre = new List<int>();
        bool IsScaUser = false;
        List<CsCentre> _listeDesCentreExistant = null;
        void ChargerListDesSite()
        {
            try
            {

                if (SessionObject.LstCentre != null && SessionObject.LstCentre.Count != 0)
                {
                    lesCentre = Shared.ClasseMEthodeGenerique.RetourCentreByPerimetre(SessionObject.LstCentre, UserConnecte.listeProfilUser);
                    lesSite = Shared.ClasseMEthodeGenerique.RetourneSiteByCentre(lesCentre);
                    _listeDesCentreExistant = lesCentre;
                    if (lesSite.Where(t => t.CODE == SessionObject.Enumere.CodeSiteScaBT || t.CODE == SessionObject.Enumere.CodeSiteScaMT) != null)
                    {
                        IsScaUser = true;
                        lesCentre = SessionObject.LstCentre;
                    }
                    foreach (ServiceAccueil.CsCentre item in lesCentre)
                        lstIdCentre.Add(item.PK_ID);
                    return;
                }
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.ListeDesDonneesDesSiteAsync(false);
                service.ListeDesDonneesDesSiteCompleted += (s, args) =>
                {
                    try
                    {
                        if (args != null && args.Cancelled)
                            return;
                        SessionObject.LstCentre = args.Result;
                        if (SessionObject.LstCentre.Count != 0)
                        {
                            lesCentre = Shared.ClasseMEthodeGenerique.RetourCentreByPerimetre(SessionObject.LstCentre, UserConnecte.listeProfilUser);
                            lesSite = Shared.ClasseMEthodeGenerique.RetourneSiteByCentre(lesCentre);
                            _listeDesCentreExistant = lesCentre;
                            if (lesSite.Where(t => t.CODE == SessionObject.Enumere.CodeSiteScaBT || t.CODE == SessionObject.Enumere.CodeSiteScaMT) != null)
                            {
                                IsScaUser = true;
                                lesCentre = SessionObject.LstCentre;
                            }
                            foreach (ServiceAccueil.CsCentre item in lesCentre)
                                lstIdCentre.Add(item.PK_ID);

                        }
                        else
                        {
                            Message.ShowInformation("Aucun site trouvé en base.", "Erreur");
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        Message.ShowError(ex, "Erreur");
                    }
                };
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
            }
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
                };
                service.ChargerTypeDocumentAsync();
                service.CloseAsync();

            }
            catch (Exception)
            {

                throw;
            }
        }

        private void dgListePiece_CurrentCellChanged(object sender, EventArgs e)
        {
            dgListePiece.BeginEdit();
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
            this.LstPiece.Add(new ObjDOCUMENTSCANNE { PK_ID = Guid.NewGuid(), NOMDOCUMENT = ((CsTypeDOCUMENTSCANNE)cbo_typedoc.SelectedItem).LIBELLE, FK_IDTYPEDOCUMENT = ((CsTypeDOCUMENTSCANNE)cbo_typedoc.SelectedItem).PK_ID, CONTENU = image, DATECREATION = DateTime.Now, DATEMODIFICATION = DateTime.Now, USERCREATION = UserConnecte.matricule, USERMODIFICATION = UserConnecte.matricule, ISNEW = true });
            this.dgListePiece.ItemsSource = this.LstPiece;
            if (LstPiece.Count() > 0)
            {
                this.isPreuveSelectionnee = true;
                //EnabledDevisInformations(true);
            }
            else
            {
                this.isPreuveSelectionnee = false;
                //EnabledDevisInformations(false);
            }
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
        public ObservableCollection<ObjDOCUMENTSCANNE> LstPiece = new ObservableCollection<ObjDOCUMENTSCANNE>();
        private byte[] image;
   
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
        private void ChargeDetailDEvis(CsClient leclient)
        {

            try
            {
                prgBar.Visibility = System.Windows.Visibility.Visible;
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient client = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                client.GeDetailByFromClientCompleted += (ssender, args) =>
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
                        laDetailDemande = new CsDemande();
                        laDetailDemande = args.Result;
                        this.txtSite.Text = string.IsNullOrEmpty(laDetailDemande.LeClient.LIBELLESITE) ? string.Empty : laDetailDemande.LeClient.LIBELLESITE;
                        this.txtCentre.Text = string.IsNullOrEmpty(laDetailDemande.LeClient.LIBELLECENTRE) ? string.Empty : laDetailDemande.LeClient.LIBELLECENTRE;
                        this.txtCentre.Tag  = laDetailDemande.LeClient.FK_IDCENTRE;
                        this.txt_Produit.Text = string.IsNullOrEmpty(laDetailDemande.Abonne.LIBELLEPRODUIT) ? string.Empty : laDetailDemande.Abonne.LIBELLEPRODUIT;
                        this.txt_Produit.Tag = laDetailDemande.Abonne.FK_IDPRODUIT;
                        this.txt_tdem.Text = SessionObject.LstTypeDemande.FirstOrDefault(t => t.CODE == Tdem).LIBELLE;
                        txt_tdem.Tag = SessionObject.LstTypeDemande.FirstOrDefault(t => t.CODE == Tdem);

                        CsCentre leCentreSelect = SessionObject.LstCentre.FirstOrDefault(t => t.PK_ID == laDetailDemande.LeClient.FK_IDCENTRE);
                        if (leCentreSelect.CODESITE != SessionObject.Enumere.CodeSiteScaBT)
                        {
                            CsCentre leCentreDestiniation = SessionObject.LstCentre.FirstOrDefault(t => t.CODE == laDetailDemande.LeClient.CENTRE &&
                                                                                                         t.CODESITE == SessionObject.Enumere.CodeSiteScaBT);
                            this.txtCentreTransfert.Text = leCentreDestiniation.LIBELLE;
                            this.txtCentreTransfert.Tag = leCentreDestiniation;

                            this.txtSiteTransfert.Text = leCentreDestiniation.LIBELLESITE;
                            this.txtSiteTransfert.Tag = leCentreDestiniation.FK_IDCODESITE;

                            Txt_CodeRegroupement.Visibility = System.Windows.Visibility.Visible;
                            lbl_CodeRegroupement_Copy1.Visibility = System.Windows.Visibility.Visible;
                            Cbo_Regroupement.Visibility = System.Windows.Visibility.Visible;
                        }
                        else
                        {
                            CsCentre leCentreDestiniation = SessionObject.LstCentre.FirstOrDefault(t => t.CODE == laDetailDemande.LeClient.CENTRE && (t.CODESITE != SessionObject.Enumere.CodeSiteScaBT && t.CODESITE != SessionObject.Enumere.CodeSiteScaMT ));
                            this.txtCentreTransfert.Text = leCentreDestiniation.LIBELLE;
                            this.txtCentreTransfert.Tag = leCentreDestiniation;

                            this.txtSiteTransfert.Text = leCentreDestiniation.LIBELLESITE;
                            this.txtSiteTransfert.Tag  = leCentreDestiniation.FK_IDCODESITE ;

                            Txt_CodeRegroupement.Visibility = System.Windows.Visibility.Collapsed ;
                            lbl_CodeRegroupement_Copy1.Visibility = System.Windows.Visibility.Collapsed;
                            Cbo_Regroupement.Visibility = System.Windows.Visibility.Collapsed;
                        }

                        RemplireOngletClient(laDetailDemande.LeClient);
                        RemplirOngletAbonnement(laDetailDemande.Abonne);
                        RemplireOngletCanalisation(laDetailDemande.LstCanalistion);
                        RemplireBranchementParProduit(laDetailDemande.Branchement);
                        RemplireOngletAdresse(laDetailDemande.Ag );
                    }
                };
                client.GeDetailByFromClientAsync(leclient);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        CsCompteClient leClasseurClient = null;
        List<CsLclient> LstReglementClient = null ;
        List<CsLclient> LstFactureClient = null;

        private void RetourneLeCompteClient(CsClientRechercher leClient)
        {
            try
            {
                prgBar.Visibility = System.Windows.Visibility.Visible ;
                AcceuilServiceClient service = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.RetourneLeCompteClientCompleted += (s, args) =>
                {
                    prgBar.Visibility = System.Windows.Visibility.Collapsed;
                    leClasseurClient = new CsCompteClient();
                    if (args != null && args.Cancelled)
                        return;

                    leClasseurClient = args.Result;
                    if (leClasseurClient != null)
                    {
                        decimal _totalDebit = 0;
                        decimal _totalCredit = 0;

                        _totalDebit = decimal.Parse(leClasseurClient.ToutLClient.Where(t => t.DC == SessionObject.Enumere.Debit).Sum(p => p.MONTANT).ToString());
                        this.tab4_txt_TotalDebit.Text = _totalDebit.ToString(SessionObject.FormatMontant);

                        _totalCredit = leClasseurClient.ToutLClient.Where(t => t.DC == SessionObject.Enumere.Credit).Sum(p => decimal.Parse(p.MONTANT.ToString()));
                        this.tab4_txt_TotalCredit.Text = _totalCredit.ToString(SessionObject.FormatMontant);
                        tab4_txt_balance.Text = (_totalDebit - _totalCredit).ToString(SessionObject.FormatMontant); ;

                        LstReglementClient = new List<CsLclient>();
                        LstFactureClient = new List<CsLclient>();
                        if (leClasseurClient.LstFacture != null)
                            LstFactureClient = leClasseurClient.LstFacture;
                        if (leClasseurClient.LstReglement != null)
                            LstReglementClient = leClasseurClient.LstReglement;
                        RemplirTypeAction(0);
                    }
                };
                service.RetourneLeCompteClientAsync(leClient.FK_IDCENTRE, leClient.CENTRE, leClient.CLIENT, leClient.ORDRE);
                service.CloseAsync();
            }
            catch (Exception ex)
            {
            }
        }
        private void RemplirTypeAction(int Index)
        {
            try
            {
                int caseSwitch = Index;
                switch (caseSwitch)
                {
                    case 0:
                        {
                            this.tab4_dataGrid1.Visibility = System.Windows.Visibility.Collapsed;
                            this.tab4_dataGrid2.Visibility = System.Windows.Visibility.Collapsed;
                            this.tab4_dataGrid3.Visibility = System.Windows.Visibility.Visible;
                            if (leClasseurClient != null)
                            {
                                //_LeClasseur.LeCompteClient.ToutLClient = new List<CsLclient>();

                                tab4_dataGrid3.ItemsSource = null;
                                List<CsLclient> _ToutLeCompteClient = ClasseMEthodeGenerique.RetourneListCopy<CsLclient>(leClasseurClient.ToutLClient.OrderBy(t => t.REFEM).ToList());
                                List<CsLclient> _ToutLeCompteClientReg = leClasseurClient.ToutLClient.Where(p => p.DC == "C").ToList();
                                RemplireOngletToutLeCompte(_ToutLeCompteClient.OrderBy(t => t.REFEM).ToList());
                            }
                        }
                        break;
                    case 1:
                        {
                            this.tab4_dataGrid1.Visibility = System.Windows.Visibility.Visible;
                            this.tab4_dataGrid2.Visibility = System.Windows.Visibility.Collapsed;
                            this.tab4_dataGrid3.Visibility = System.Windows.Visibility.Collapsed;
                            tab4_dataGrid1.ItemsSource = null;
                            RemplireOngletReglement(LstReglementClient.OrderBy(t => t.REFEM).ToList());
                        }
                        break;
                    case 2:
                        {
                            this.tab4_dataGrid1.Visibility = System.Windows.Visibility.Collapsed;
                            this.tab4_dataGrid2.Visibility = System.Windows.Visibility.Visible;
                            this.tab4_dataGrid3.Visibility = System.Windows.Visibility.Collapsed;
                            this.tab4_dataGrid2.ItemsSource = null;
                            RemplireOngletFacture(LstFactureClient.OrderBy(t => t.REFEM).ToList());
                        }
                        break;

                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        private void RemplireOngletToutLeCompte(List<CsLclient> _LeCompteClient)
        {
            try
            {
                tab4_dataGrid3.ItemsSource = null;
                tab4_dataGrid3.ItemsSource = FormateListe(_LeCompteClient);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        private List<CsLclient> FormateListe(List<CsLclient> _LeCompteClient)
        {
            List<CsLclient> _LstFactureFinal = new List<CsLclient>();
            if (_LeCompteClient != null && ((_LeCompteClient != null && _LeCompteClient.Count != 0)))
            {
                List<CsLclient> _LstFacture = _LeCompteClient.Where(p => p.DC == SessionObject.Enumere.Debit).ToList();
                List<CsLclient> _LstEncaissement = _LeCompteClient.Where(p => p.DC == SessionObject.Enumere.Credit).ToList();
                if (_LstFacture != null && _LstFacture.Count != 0)
                    foreach (var item in _LstFacture)
                    {
                        _LstFactureFinal.Add(item);
                        List<CsLclient> lstFacture = _LstEncaissement.Where(p => p.REFEM == item.REFEM && p.NDOC == item.NDOC).ToList();
                        if (lstFacture != null && lstFacture.Count != 0)
                            _LstFactureFinal.AddRange(TransLClient(lstFacture));
                    }
                else
                    _LstFactureFinal.AddRange(_LstEncaissement);

                //if (_LstEncaissement != null && _LstEncaissement.Count != 0)
                // foreach (CsLclient item in _LstEncaissement)
                // {
                //     if (_LstFactureFinal.FirstOrDefault(t => t.REFEM == item.REFEM && t.NDOC == item.NDOC) == null)
                //         _LstFactureFinal.Add(item);
                // }
            }
            return _LstFactureFinal;
        }
        private List<CsLclient> TransLClient(List<CsLclient> _LeTranscaisse)
        {
            List<CsLclient> _LeReglt = new List<CsLclient>();
            foreach (var item in _LeTranscaisse)
            {
                item.REFEM = string.Empty;
                item.NDOC = string.Empty;
                item.ACQUIT = string.Empty;
                _LeReglt.Add(item);
            }
            return _LeReglt;
        }

        private void RemplireOngletFacture(List<CsLclient> _LesFacture)
        {
            try
            {
                tab4_dataGrid2.ItemsSource = null;
                tab4_dataGrid2.ItemsSource = _LesFacture.OrderByDescending(p => p.DENR);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        private void RemplireOngletReglement(List<CsLclient> _LesReglement)
        {
            try
            {

                var reglemntParModereg = (from p in _LesReglement
                                          group new { p } by new { p.ACQUIT, p.DTRANS, p.NOMCAISSIERE } into pResult
                                          select new
                                          {
                                              pResult.Key.ACQUIT,
                                              pResult.Key.NOMCAISSIERE,
                                              pResult.Key.DTRANS,
                                              MONTANT = (decimal?)pResult.Where(t => t.p.ACQUIT == pResult.Key.ACQUIT).Sum(o => o.p.MONTANT)
                                          });
                tab4_dataGrid1.ItemsSource = null;
                tab4_dataGrid1.ItemsSource = reglemntParModereg.OrderByDescending(p => p.DTRANS);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        private void RemplireOngletClient( CsClient _LeClient)
        {
            try
            {
                if (_LeClient != null)
                {

                     this.txt_NomClient.Text = (string.IsNullOrEmpty(_LeClient.NOMABON) ? string.Empty : _LeClient.NOMABON);
                     this.txt_Telephone.Text = string.IsNullOrEmpty(_LeClient.TELEPHONE) ? string.Empty : _LeClient.TELEPHONE;
                     this.txt_Adresse.Text = string.IsNullOrEmpty(_LeClient.ADRMAND1) ? string.Empty : _LeClient.ADRMAND1;
                     this.txt_Nina.Text = string.IsNullOrEmpty(_LeClient.NUMEROIDCLIENT) ? string.Empty : _LeClient.NUMEROIDCLIENT;

                    //this.tab12_txt_addresse.Text = string.IsNullOrEmpty(_LeClient.ADRMAND1) ? string.Empty : _LeClient.ADRMAND1;
                    //this.tab12_txt_addresse2.Text = string.IsNullOrEmpty(_LeClient.ADRMAND2) ? string.Empty : _LeClient.ADRMAND2;
                    this.tab12_Txt_LibelleCodeConso.Text = string.IsNullOrEmpty(_LeClient.LIBELLECODECONSO) ? string.Empty : _LeClient.LIBELLECODECONSO;
                    this.tab12_Txt_LibelleCategorie.Text = string.IsNullOrEmpty(_LeClient.LIBELLECATEGORIE) ? string.Empty : _LeClient.LIBELLECATEGORIE;
                    this.tab12_Txt_LibelleEtatClient.Text = string.IsNullOrEmpty(_LeClient.LIBELLERELANCE) ? string.Empty : _LeClient.LIBELLERELANCE;
                    this.tab12_Txt_LibelleTypeClient.Text = string.IsNullOrEmpty(_LeClient.LIBELLENATURECLIENT) ? string.Empty : _LeClient.LIBELLENATURECLIENT;
                    this.tab12_Txt_Nationnalite.Text = string.IsNullOrEmpty(_LeClient.LIBELLENATIONALITE) ? string.Empty : _LeClient.LIBELLENATIONALITE;
                    this.tab12_Txt_Datecreate.Text = string.IsNullOrEmpty(_LeClient.DATECREATION.ToString()) ? string.Empty : Convert.ToDateTime(_LeClient.DATECREATION).ToShortDateString();
                    //this.tab12_Txt_DateModif.Text = string.IsNullOrEmpty(_LeClient.DATEMODIFICATION.ToString()) ? string.Empty : Convert.ToDateTime(_LeClient.DATEMODIFICATION).ToShortDateString();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void RemplirOngletAbonnement(CsAbon _LeAbon)
        {
            if (_LeAbon != null)
            {
                this.Txt_CodeTarif.Text = !string.IsNullOrEmpty(_LeAbon.TYPETARIF) ? _LeAbon.TYPETARIF : string.Empty;
                this.Txt_CodePussanceSoucrite.Text = !string.IsNullOrEmpty(_LeAbon.PUISSANCE.Value.ToString()) ? _LeAbon.PUISSANCE.Value.ToString() : string.Empty;

                if (_LeAbon.PUISSANCE != null)
                    this.Txt_CodePussanceSoucrite.Text = Convert.ToDecimal(_LeAbon.PUISSANCE.ToString()).ToString("N2");
                if (_LeAbon.PUISSANCEUTILISEE != null)
                    this.Txt_CodePuissanceUtilise.Text = Convert.ToDecimal(_LeAbon.PUISSANCEUTILISEE.Value).ToString("N2");

                this.Txt_CodeForfait.Text = string.IsNullOrEmpty(_LeAbon.FORFAIT) ? string.Empty : _LeAbon.FORFAIT;
                this.Txt_LibelleForfait.Text = string.IsNullOrEmpty(_LeAbon.LIBELLEFORFAIT) ? string.Empty : _LeAbon.LIBELLEFORFAIT;

                this.Txt_CodeTarif.Text = string.IsNullOrEmpty(_LeAbon.TYPETARIF) ? string.Empty : _LeAbon.TYPETARIF;
                this.Txt_LibelleTarif.Text = !string.IsNullOrEmpty(_LeAbon.LIBELLETARIF) ? _LeAbon.LIBELLETARIF : string.Empty;

                this.Txt_CodeFrequence.Text = string.IsNullOrEmpty(_LeAbon.PERFAC) ? string.Empty : _LeAbon.PERFAC;
                this.Txt_LibelleFrequence.Text = !string.IsNullOrEmpty(_LeAbon.LIBELLEFREQUENCE) ? _LeAbon.LIBELLEFREQUENCE : string.Empty;

                this.Txt_CodeMoisIndex.Text = string.IsNullOrEmpty(_LeAbon.MOISREL) ? string.Empty : _LeAbon.MOISREL;
                this.Txt_LibelleMoisIndex.Text = !string.IsNullOrEmpty(_LeAbon.LIBELLEMOISIND) ? _LeAbon.LIBELLEMOISIND : string.Empty;

                this.Txt_CodeMoisFacturation.Text = string.IsNullOrEmpty(_LeAbon.MOISFAC) ? string.Empty : _LeAbon.MOISFAC;
                this.Txt_LibMoisFact.Text = !string.IsNullOrEmpty(_LeAbon.LIBELLEMOISFACT) ? _LeAbon.LIBELLEMOISFACT : string.Empty;

                this.Txt_DateAbonnement.Text = (_LeAbon.DABONNEMENT == null) ? string.Empty : Convert.ToDateTime(_LeAbon.DABONNEMENT.Value).ToShortDateString();
            }
        }

        private void RemplireBranchementParProduit(CsBrt _LeBrtSelectionne)
        {
            try
            {
                this.Txt_Distance.Text = laDetailDemande.Branchement.LONGBRT == null ? string.Empty : laDetailDemande.Branchement.LONGBRT.ToString();
                this.Txt_PuissanceInstalle .Text = laDetailDemande.Branchement.PUISSANCEINSTALLEE == null ? string.Empty : laDetailDemande.Branchement.PUISSANCEINSTALLEE.Value.ToString(SessionObject.FormatMontant);
                this.TxtLongitude.Text = string.IsNullOrEmpty(laDetailDemande.Branchement.LONGITUDE) ? string.Empty : laDetailDemande.Branchement.LONGITUDE;
                this.TxtLatitude.Text = string.IsNullOrEmpty(laDetailDemande.Branchement.LATITUDE) ? string.Empty : laDetailDemande.Branchement.LATITUDE;

                Txt_LibelleTypeBrt.Text = string.IsNullOrEmpty(laDetailDemande.Branchement.LIBELLETYPEBRANCHEMENT) ? string.Empty : laDetailDemande.Branchement.LIBELLETYPEBRANCHEMENT;
                Txt_LibellePosteSource.Text = string.IsNullOrEmpty(laDetailDemande.Branchement.LIBELLEPOSTESOURCE) ? string.Empty : laDetailDemande.Branchement.LIBELLEPOSTESOURCE;
                Txt_LibelleDepartHTA.Text = string.IsNullOrEmpty(laDetailDemande.Branchement.LIBELLEDEPARTHTA) ? string.Empty : laDetailDemande.Branchement.LIBELLEDEPARTHTA;
                Txt_LibelleQuartierPoste.Text = string.IsNullOrEmpty(laDetailDemande.Branchement.LIBELLEQUARTIER) ? string.Empty : laDetailDemande.Branchement.LIBELLEQUARTIER;
                Txt_LibellePosteTransformateur.Text = string.IsNullOrEmpty(laDetailDemande.Branchement.LIBELLETRANSFORMATEUR) ? string.Empty : laDetailDemande.Branchement.LIBELLETRANSFORMATEUR;
                Txt_LibelleDepartBt.Text = string.IsNullOrEmpty(laDetailDemande.Branchement.DEPARTBT) ? string.Empty : laDetailDemande.Branchement.DEPARTBT;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void RemplireCannalisationProduit(CsCanalisation _LaCannalisationSelect)
        {
            try
            {
                this.tab5_txt_NumCompteur.Text = string.IsNullOrEmpty(_LaCannalisationSelect.NUMERO) ? string.Empty : _LaCannalisationSelect.NUMERO;
                this.tab5_txt_AnnefabricCompteur.Text = string.IsNullOrEmpty(_LaCannalisationSelect.ANNEEFAB) ? string.Empty : _LaCannalisationSelect.ANNEEFAB;
                this.tab5_txt_LibelleTypeCompteur.Text = string.IsNullOrEmpty(_LaCannalisationSelect.LIBELLETYPECOMPTEUR) ? string.Empty : _LaCannalisationSelect.LIBELLETYPECOMPTEUR;
                this.tab5_txt_NumCompteur.Text = string.IsNullOrEmpty(_LaCannalisationSelect.NUMERO) ? string.Empty : _LaCannalisationSelect.NUMERO;
                this.tab5_txt_MarqueCompteur.Text = string.IsNullOrEmpty(_LaCannalisationSelect.LIBELLEMARQUE) ? string.Empty : _LaCannalisationSelect.LIBELLEMARQUE;


                if (_LaCannalisationSelect.PRODUIT != SessionObject.Enumere.ElectriciteMT)
                    this.tab5_txt_LibelleDiametreCompteur.Text = string.IsNullOrEmpty(_LaCannalisationSelect.LIBELLEREGLAGECOMPTEUR) ? string.Empty : _LaCannalisationSelect.LIBELLEREGLAGECOMPTEUR;
                else
                    this.tab5_txt_LibelleDiametreCompteur.Text = string.IsNullOrEmpty(_LaCannalisationSelect.LIBELLETYPECOMPTAGE) ? string.Empty : _LaCannalisationSelect.LIBELLETYPECOMPTAGE;

                this.tab5_txt_CoefDeMultiplication.Text = _LaCannalisationSelect.COEFLECT.ToString();
                if (_LaCannalisationSelect.COEFLECT == 0)
                    this.tab5_Chk_CoefMultiplication.IsChecked = false;
                else
                    tab5_Chk_CoefMultiplication.IsChecked = true;

                if (_LaCannalisationSelect.POSE != null)
                    this.tab5_txt_DateMiseEnService.Text = Convert.ToDateTime(_LaCannalisationSelect.POSE).ToShortDateString();

                if (_LaCannalisationSelect.DEPOSE != null)
                    this.tab5_txt_DateFinServce.Text = Convert.ToDateTime(_LaCannalisationSelect.DEPOSE).ToShortDateString();


                this.tab5_txt_LibelleDigit.Text = _LaCannalisationSelect.CADRAN.ToString();
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        private void RemplireOngletCanalisation(List<CsCanalisation> _LstCannalisation)
        {
            try
            {

                if (_LstCannalisation != null && _LstCannalisation.Count != 0)
                {
                    foreach (CsCanalisation item in _LstCannalisation)
                    {
                        //RetourneEvenement(item);
                        if (item.DEPOSE != null)
                            item.LIBELLEETATCOMPTEUR = Galatee.Silverlight.Resources.Accueil.Langue.lbl_EtatCompteDepose;
                        else
                            item.LIBELLEETATCOMPTEUR = Galatee.Silverlight.Resources.Accueil.Langue.lbl_EtatCompteActif;
                       
                    }
                    dtgCompteur .ItemsSource = _LstCannalisation;
                    dtgCompteur.SelectedItem = _LstCannalisation[0];

                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        private void RemplirCodeRegroupement()
        {
            try
            {
                if (SessionObject.LstCodeRegroupement.Count != 0)
                {
                    Cbo_Regroupement.Items.Clear();
                    Cbo_Regroupement.ItemsSource = SessionObject.LstCodeRegroupement;
                    Cbo_Regroupement.SelectedValuePath = "PK_ID";
                    Cbo_Regroupement.DisplayMemberPath = "NOM";
                }
                else
                {
                    Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                    service.RetourneCodeRegroupementCompleted += (s, args) =>
                    {
                        if (args != null && args.Cancelled)
                            return;
                        SessionObject.LstCodeRegroupement = args.Result;
                        Cbo_Regroupement.Items.Clear();
                        Cbo_Regroupement.ItemsSource = SessionObject.LstCodeRegroupement;
                        Cbo_Regroupement.SelectedValuePath = "PK_ID";
                        Cbo_Regroupement.DisplayMemberPath = "NOM";
                    };
                    service.RetourneCodeRegroupementAsync();
                    service.CloseAsync();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        private void RemplireOngletAdresse(CsAg _LeAdresse)
        {
            try
            {
                if (_LeAdresse != null)
                {

                    this.tab3_txt_NomClientBrt.Text = string.IsNullOrEmpty(_LeAdresse.NOMP) ? string.Empty : _LeAdresse.NOMP;
                    this.tab3_txt_LibelleCommune.Text = string.IsNullOrEmpty(_LeAdresse.LIBELLECOMMUNE) ? string.Empty : _LeAdresse.LIBELLECOMMUNE;
                    this.tab3_txt_LibelleQuartier.Text = string.IsNullOrEmpty(_LeAdresse.LIBELLEQUARTIER) ? string.Empty : _LeAdresse.LIBELLEQUARTIER;
                    this.tab3_txt_Secteur.Text = string.IsNullOrEmpty(_LeAdresse.LIBELLESECTEUR) ? string.Empty : _LeAdresse.LIBELLESECTEUR;
                    this.tab3_txt_NomRue.Text = string.IsNullOrEmpty(_LeAdresse.LIBELLERUE) ? string.Empty : _LeAdresse.LIBELLERUE;
                    this.tab3_txt_NumRue.Text = string.IsNullOrEmpty(_LeAdresse.RUE) ? string.Empty : _LeAdresse.RUE;

                    this.tab3_txt_etage.Text = string.IsNullOrEmpty(_LeAdresse.ETAGE) ? string.Empty : _LeAdresse.ETAGE;
                    this.tab3_txt_NumLot.Text = string.IsNullOrEmpty(_LeAdresse.CADR) ? string.Empty : _LeAdresse.CADR;
                    //this.tab3_txt_Email.Text = string.IsNullOrEmpty(_LeAdresse.EMAIL) ? string.Empty : _LeAdresse.EMAIL;

                    this.tab3_txt_Telephone.Text = string.IsNullOrEmpty(_LeAdresse.TELEPHONE) ? string.Empty : _LeAdresse.TELEPHONE;
                    //this.tab3_txt_Fax.Text = string.IsNullOrEmpty(_LeAdresse.FAX) ? string.Empty : _LeAdresse.FAX;
                    this.tab3_txt_OrdreTour.Text = string.IsNullOrEmpty(_LeAdresse.ORDTOUR) ? string.Empty : _LeAdresse.ORDTOUR;
                    this.tab3_txt_tournee.Text = string.IsNullOrEmpty(_LeAdresse.TOURNEE) ? string.Empty : _LeAdresse.TOURNEE;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        private void btn_transmetre_Click(object sender, RoutedEventArgs e)
        {
            this.btn_transmetre.IsEnabled = false;
            this.CancelButton.IsEnabled = false;
            ValiderInitialisation(null, true);
        }
        private void PropietaireWindows(Visibility stat)
        {
            this.lbl_NomProprio.Visibility = stat;
            this.Txt_NomProprio_PersonePhysiq.Visibility = stat;
            this.lbl_PrenomProprio.Visibility = stat;
            this.Txt_PrenomProprio_PersonePhysiq.Visibility = stat;
            this.lbl_DateNaissanceProprio.Visibility = stat;
            this.dtp_DateNaissanceProprio.Visibility = stat;
            this.lbl_NaturePieceIdentiteProprio.Visibility = stat;
            this.Cbo_TypePiecePersonnePhysiqueProprio.Visibility = stat;
            this.lbl_NumPieceProprio.Visibility = stat;
            this.txtNumeroPieceProprio.Visibility = stat;
            this.lbl_DateFinValiditeProprio.Visibility = stat;
            this.dtp_finvalidationProprio.Visibility = stat;
            this.txt_Telephone_Proprio.Visibility = stat;
            this.Txt_Email_Proprio.Visibility = stat;
            this.label7_Copy4.Visibility = stat;
            this.label7_Copy5.Visibility = stat;
            this.Txt_Faxe_Proprio.Visibility = stat;
            this.Txt_BoitePosta_Proprio.Visibility = stat;
            this.lbl_Nationalite_Copy1.Visibility = stat;
            this.Cbo_Nationalite_Proprio.Visibility = stat;
            this.label7_Copy6.Visibility = stat;
            this.label7_Copy7.Visibility = stat;
        }
        private bool VerifieChampObligation()
        {
            try
            {
                bool ReturnValue = true;

                //if (this.Cbo_CentreTransfert.SelectedItem == null)
                //    throw new Exception("Séléctionnez le centre de destination ");

                if (Txt_CodeRegroupement.Visibility == System.Windows.Visibility.Visible ) 
                  {
                    if (this.Cbo_Regroupement.SelectedItem == null )
                        throw new Exception("Selectionnez le regroupement ");
                  }
                  return ReturnValue;

            }
            catch (Exception ex)
            {
                Message.ShowInformation(ex.Message, "Accueil");
                return false;
            }

        }
        private void ValiderInitialisation(CsDemande demandedevis, bool IsTransmetre)
        {

            try
            {
                // Get Devis informations from screen
                if (demandedevis != null)
                    demandedevis = GetDemandeDevisFromScreen(demandedevis, false);
                else
                {

                    demandedevis = GetDemandeDevisFromScreen(demandedevis, false);
                }
                // Get DemandeDevis informations from screen
                if (demandedevis != null)
                {
                    demandedevis.LaDemande.ETAPEDEMANDE = (int)DataReferenceManager.EtapeDevis.Accueil;
                    if (IsTransmetre)
                        demandedevis.LaDemande.ETAPEDEMANDE = null;
                    demandedevis.LaDemande.MATRICULE = UserConnecte.matricule;

                    prgBar.Visibility = System.Windows.Visibility.Visible;
                    laDetailDemande.LaDemande = demandedevis.LaDemande;
                    laDetailDemande.Transfert = demandedevis.Transfert;
                    laDetailDemande.LeClient = laDetailDemande.LeClient; 



                    //Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient client = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                    //client.ValiderDemandeInitailisationCompleted += (ss, b) =>
                    //{
                    //    prgBar.Visibility = System.Windows.Visibility.Collapsed ;
                    //    if (b.Cancelled || b.Error != null)
                    //    {
                    //        string error = b.Error.Message;
                    //        Message.ShowError(error, Silverlight.Resources.Devis.Languages.txtDevis);
                    //        return;
                    //    }
                    //    string numedemande = string.Empty;
                    //    string Client = string.Empty;
                    //    if (IsTransmetre)
                    //    {
                    //        string Retour = b.Result;
                    //        string[] coupe = Retour.Split('.');
                    //        Shared.ClasseMEthodeGenerique.InitWOrkflow(coupe[0], demandedevis.LaDemande.FK_IDCENTRE, coupe[1], demandedevis.LaDemande.FK_IDTYPEDEMANDE);
                    //        numedemande = coupe[1];
                    //        Client = coupe[2];
                    //    }
                    //    List<CsDemandeBase> leDemandeAEditer = new List<CsDemandeBase>();
                    //    demandedevis.LaDemande.NOMCLIENT = txt_NomClient.Text ;
                    //    demandedevis.LaDemande.LIBELLETYPEDEMANDE = txt_tdem.Text;
                    //    demandedevis.LaDemande.NUMDEM = numedemande;
                    //    demandedevis.LaDemande.CLIENT = Client;
                    //    demandedevis.LaDemande.LIBELLECENTRE = txtCentre.Text ;
                    //    demandedevis.LaDemande.LIBELLECOMMUNE = tab3_txt_LibelleCommune.Text ;
                    //    demandedevis.LaDemande.LIBELLEQUARTIER = tab3_txt_LibelleQuartier.Text ;
                    //    demandedevis.LaDemande.ANNOTATION = tab3_txt_Telephone.Text;
                    //    demandedevis.LaDemande.NOMPERE = tab3_txt_NomRue.Text ;
                    //    demandedevis.LaDemande.NOMMERE = string.IsNullOrEmpty(demandedevis.LeClient.PORTE) ? string.Empty : demandedevis.Ag.PORTE;
                    //    demandedevis.LaDemande.LIBELLEPRODUIT = demandedevis.Abonne .LIBELLEPRODUIT ;
                    //    leDemandeAEditer.Add(demandedevis.LaDemande);
                    //    Utility.ActionDirectOrientation<ServicePrintings.CsDemandeBase, CsDemandeBase>(leDemandeAEditer, null, SessionObject.CheminImpression, "AccuseRecption", "Accueil", true);
                    //};
                    //client.ValiderDemandeInitailisationAsync(laDetailDemande);






                    Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient client = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                    client.CreeDemandeCompleted += (ss, b) =>
                    {
                        if (b.Cancelled || b.Error != null)
                        {
                            string error = b.Error.Message;
                            Message.ShowError(error, Silverlight.Resources.Devis.Languages.txtDevis);
                            return;
                        }
                        if (b.Result != null)
                        {
                            List<CsDemandeBase> leDemandeAEditer = new List<CsDemandeBase>();


                            demandedevis.LaDemande.NOMCLIENT = txt_NomClient.Text;
                            demandedevis.LaDemande.LIBELLETYPEDEMANDE = txt_tdem.Text;
                            demandedevis.LaDemande.NUMDEM = b.Result.NUMDEM;
                            demandedevis.LaDemande.CLIENT = b.Result.CLIENT;
                            demandedevis.LaDemande.LIBELLECENTRE = txtCentre.Text;
                            demandedevis.LaDemande.LIBELLECOMMUNE = tab3_txt_LibelleCommune.Text;
                            demandedevis.LaDemande.LIBELLEQUARTIER = tab3_txt_LibelleQuartier.Text;
                            demandedevis.LaDemande.ANNOTATION = tab3_txt_Telephone.Text;
                            demandedevis.LaDemande.NOMPERE = tab3_txt_NomRue.Text;
                            demandedevis.LaDemande.NOMMERE = string.IsNullOrEmpty(demandedevis.LeClient.PORTE) ? string.Empty : demandedevis.Ag.PORTE;
                            demandedevis.LaDemande.LIBELLEPRODUIT = demandedevis.Abonne.LIBELLEPRODUIT;
                            demandedevis.LaDemande.LIBELLE = "Imprimé le " + DateTime.Now + " sur le poste " + SessionObject.LePosteCourant.NOMPOSTE + " par " + UserConnecte.nomUtilisateur + "(" + UserConnecte.matricule + ") du centre " + UserConnecte.LibelleCentre;
                            
                            
                            leDemandeAEditer.Add(demandedevis.LaDemande);
                            Utility.ActionDirectOrientation<ServicePrintings.CsDemandeBase, CsDemandeBase>(leDemandeAEditer, null, SessionObject.CheminImpression, "AccuseRecption", "Accueil", true);

                            Message.ShowInformation("La demande a été créée avec succès. Numéro de votre demande : " + b.Result.NUMDEM,
                            Silverlight.Resources.Devis.Languages.txtDevis);
                        }
                    };
                    client.CreeDemandeAsync(laDetailDemande, true);




                    this.DialogResult = true;
                }
            }
            catch (Exception ex)
            {
                Message.ShowError("Une erreur s'est produite a la validation ", "ValiderDemandeInitailisation");
            }
        }

        private CsDemande GetDemandeDevisFromScreen(CsDemande pDemandeDevis, bool isTransmettre)
        {
            try
            {
                if (pDemandeDevis == null)
                {
                    pDemandeDevis = new CsDemande();
                    pDemandeDevis.LaDemande = new CsDemandeBase();
                    pDemandeDevis.Abonne = new CsAbon();
                    pDemandeDevis.Ag = new CsAg();
                    pDemandeDevis.Branchement = new CsBrt();
                    pDemandeDevis.LeClient = new CsClient();
                    pDemandeDevis.ObjetScanne = new List<ObjDOCUMENTSCANNE>();
                    pDemandeDevis.AppareilDevis = new List<ObjAPPAREILSDEVIS>();
                    pDemandeDevis.LaDemande.DATECREATION = DateTime.Now;
                    pDemandeDevis.LaDemande.USERCREATION = UserConnecte.matricule;
                    pDemandeDevis.LaDemande.FK_IDADMUTILISATEUR = UserConnecte.PK_ID;
                }
                #region Demande

                if (pDemandeDevis.LaDemande == null) pDemandeDevis.LaDemande = new CsDemandeBase();
                if (pDemandeDevis.LaDemande.TYPEDEMANDE != SessionObject.Enumere.BranchementAbonement)
                    pDemandeDevis.LaDemande.CLIENT = string.IsNullOrEmpty(this.Txt_ReferenceClient.Text) ? string.Empty : this.Txt_ReferenceClient .Text;

                pDemandeDevis.LaDemande.DATEMODIFICATION = DateTime.Now;
                //pDemandeDevis.LaDemande.MOTIF = txt_Motif.Text;
 
                pDemandeDevis.LaDemande.FK_IDTYPEDEMANDE = SessionObject.LstTypeDemande.FirstOrDefault(t=>t.CODE == SessionObject.Enumere.TransfertAbonnement).PK_ID ;
                pDemandeDevis.LaDemande.TYPEDEMANDE = SessionObject.Enumere.TransfertAbonnement;
                 
                #region Doc Scanne
                if (pDemandeDevis.ObjetScanne == null) pDemandeDevis.ObjetScanne = new List<ObjDOCUMENTSCANNE>();
                pDemandeDevis.ObjetScanne.AddRange(this.LstPiece);
                #endregion




                #endregion
                pDemandeDevis.LaDemande.ISNEW = true;
                pDemandeDevis.LaDemande.ORDRE = laDetailDemande.LeClient.ORDRE;
                pDemandeDevis.LaDemande.DATECREATION = DateTime.Now;
                pDemandeDevis.LaDemande.PRODUIT = laDetailDemande.Abonne.PRODUIT;
                pDemandeDevis.LaDemande.FK_IDPRODUIT = laDetailDemande.Abonne.FK_IDPRODUIT;
                pDemandeDevis.LaDemande.CENTRE = laDetailDemande.Abonne.CENTRE;
                pDemandeDevis.LaDemande.FK_IDCENTRE = laDetailDemande.Abonne.FK_IDCENTRE;
                CsDtransfert letrft = new CsDtransfert()
                {
                  FK_IDCENTREORIGINE =(int) this.txtCentre.Tag  ,
                  FK_IDCENTRETRANSFERT = ((CsCentre)this.txtCentreTransfert.Tag).PK_ID ,
                };
                if (Txt_CodeRegroupement.Visibility == System.Windows.Visibility.Visible )
                        letrft.FK_IDREGROUPEMENT = ((CsRegCli)this.Cbo_Regroupement.SelectedItem).PK_ID;
                pDemandeDevis.Transfert = letrft;

                return pDemandeDevis;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public const string MatchEmailPattern =
@"^(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@"
+ @"((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?
				[0-9]{1,2}|25[0-5]|2[0-4][0-9])\."
+ @"([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?
				[0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|"
+ @"([a-zA-Z0-9]+[\w-]+\.)+[a-zA-Z]{1}[a-zA-Z0-9-]{1,23})$";
        public static bool IsEmail(string email)
        {
            if (email != null) return Regex.IsMatch(email, MatchEmailPattern);
            else return false;
        }

        private void cbo_typedoc_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void txtSite_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void txtCentre_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void dtgCompteur_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (this.dtgCompteur .SelectedIndex >= 0)
                {
                    CsCanalisation _LeCompteurSelect = (CsCanalisation)this.dtgCompteur.SelectedItem;
                    RemplireCannalisationProduit(_LeCompteurSelect);
                    if (laDetailDemande.LstEvenement != null && laDetailDemande.LstEvenement.Count != 0)
                        RemplireOngletEvenement(laDetailDemande.LstEvenement.Where(p => p.PRODUIT == _LeCompteurSelect.PRODUIT && p.POINT == _LeCompteurSelect.POINT).ToList());

                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Galatee.Silverlight.Resources.Accueil.Langue.lbl_Menu);

            }
        }
        private void RemplireOngletEvenement(List<CsEvenement> _LstEvenement)
        {
            try
            {
                tab5_Stab2dataGrid2.ItemsSource = null;
                if (_LstEvenement != null && _LstEvenement.Count != 0)
                {
                    if (_LstEvenement != null && _LstEvenement.Count != 0)
                    {
                        if (_LstEvenement.First().PRODUIT == SessionObject.Enumere.ElectriciteMT)
                            _LstEvenement.ForEach(t => t.REGLAGECOMPTEUR = t.TYPECOMPTAGE);
                        tab5_Stab2dataGrid2.ItemsSource = _LstEvenement.OrderBy(t => t.NUMEVENEMENT);
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void tab4_cbo_Operation_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.tab4_cbo_Operation.SelectedIndex >= 0)
                RemplirTypeAction(this.tab4_cbo_Operation.SelectedIndex);
        }

        private void Rdb_VersAgence_Checked_1(object sender, RoutedEventArgs e)
        {
            this.Cbo_Regroupement.SelectedItem = null;
        }

        private void Cbo_Regroupement_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (Cbo_Regroupement.SelectedItem != null)
                {
                    var Regroupement = ((ServiceAccueil.CsRegCli)Cbo_Regroupement.SelectedItem);
                    if (Regroupement != null)
                        Txt_CodeRegroupement.Text = Regroupement.CODE ?? string.Empty;
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.txtDevis);
            }
        }
        private void Txt_CodeRegroupement_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (this.Txt_CodeRegroupement.Text.Length == SessionObject.Enumere.TailleCodeRegroupement)
            {
                CsRegCli leRegroupement = SessionObject.LstCodeRegroupement.FirstOrDefault(t => t.CODE == this.Txt_CodeRegroupement.Text);
                if (leRegroupement != null)
                {
                    if ((this.Cbo_Regroupement.SelectedItem != null && (CsRegCli)this.Cbo_Regroupement.SelectedItem != leRegroupement) || this.Cbo_Regroupement.SelectedItem == null)
                        this.Cbo_Regroupement.SelectedItem = leRegroupement;
                }
                else
                {
                    Message.ShowInformation("Le code saisie n'existe pas", "Accueil");
                    return;
                }
            }

        }

        private void CheckBox_Checked_1(object sender, RoutedEventArgs e)
        {
            this.tabNewClient.Visibility = System.Windows.Visibility.Visible;
        }

        private void CheckBox_Unchecked_1(object sender, RoutedEventArgs e)
        {
            this.tabNewClient.Visibility = System.Windows.Visibility.Collapsed ;

        }

        private void Txt_Capital_TextChanged(object sender, TextChangedEventArgs e)
        {
            decimal capital = 0;
            if (!decimal.TryParse(Txt_Capital.Text, out capital))
            {
                Message.Show("veuillez saisir une valeur numerique", "Demande");
            }
            
        }


        private void Txt_DateFinvalidite_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!Shared.ClasseMEthodeGenerique.IsDateValide(Txt_DateFinvalidite.Text) && Txt_DateFinvalidite.Text.Length == SessionObject.Enumere.TailleDate)
            {
                Message.ShowError("La date n'est pas valide", "Accueil");
                this.Txt_DateFinvalidite.Focus();
            }

        }

        private void Txt_DateNaissance_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!Shared.ClasseMEthodeGenerique.IsDateValide(Txt_DateNaissance.Text) && Txt_DateNaissance.Text.Length == SessionObject.Enumere.TailleDate)
            {
                Message.ShowError("La date n'est pas valide", "Accueil");
                this.Txt_DateNaissance.Focus();
            }
        }

        private void chk_Email_Checked(object sender, RoutedEventArgs e)
        {
            if (!chk_Email.IsChecked.Value)
            {
                Txt_Email.Text = string.Empty;
            }
            Txt_Email.IsEnabled = chk_Email.IsChecked.Value;
        }

        private void Txt_Email_TextChanged_1(object sender, TextChangedEventArgs e)
        {

        }

        private void Cbo_Type_Proprietaire_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Cbo_Type_Client.SelectedItem == null)
            {
                Message.ShowInformation("Selectionnez le type de client", "Demande");
                return;
            }
            if (Cbo_Type_Proprietaire.SelectedItem != null)
            {
                var typeproprio = (CsProprietaire)Cbo_Type_Proprietaire.SelectedItem;
                if (typeproprio.CODE == "1" || typeproprio.CODE == "2")
                {
                    tab_proprio.Visibility = Visibility.Visible;
                    PropietaireWindows(System.Windows.Visibility.Visible);
                    this.tbControleClient.SelectedItem = tab_proprio;
                }
                else
                {
                    tab_proprio.Visibility = Visibility.Collapsed;
                    PropietaireWindows(System.Windows.Visibility.Visible);
                    if (((CsTypeClient)Cbo_Type_Client.SelectedItem).CODE.Trim() == "001".Trim())
                        tbControleClient.SelectedItem = this.tabItemPersonnePhysique;

                    if (((CsTypeClient)Cbo_Type_Client.SelectedItem).CODE.Trim() == "002".Trim())
                        tbControleClient.SelectedItem = this.tabItemPersoneMoral;

                    if (((CsTypeClient)Cbo_Type_Client.SelectedItem).CODE.Trim() == "003".Trim())
                        tbControleClient.SelectedItem = this.tabItemPersoneAdministration;

                }
            }
        }

        private void Cbo_TypeClient_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                CsTypeClient typeclient = ((CsTypeClient)Cbo_Type_Client.SelectedItem);
                switch (typeclient.CODE.Trim())
                {
                    case "001":
                        {
                            this.tabItemPersoneMoral.Visibility = System.Windows.Visibility.Collapsed;
                            this.tabItemPersoneAdministration.Visibility = System.Windows.Visibility.Collapsed;
                            this.tabItemPersonnePhysique.Visibility = System.Windows.Visibility.Visible;
                            tbControleClient.SelectedItem = this.tabItemPersonnePhysique;
                            break;
                        }
                    case "002":
                        {
                            tabItemPersonnePhysique.Visibility = System.Windows.Visibility.Collapsed;
                            tabItemPersoneAdministration.Visibility = System.Windows.Visibility.Collapsed;
                            tabItemPersoneMoral.Visibility = System.Windows.Visibility.Visible;
                            tbControleClient.SelectedItem = this.tabItemPersoneMoral;
                            break;
                        }
                    case "003":
                        {
                            tabItemPersonnePhysique.Visibility = System.Windows.Visibility.Collapsed;
                            tabItemPersoneMoral.Visibility = System.Windows.Visibility.Collapsed;
                            tabItemPersoneAdministration.Visibility = System.Windows.Visibility.Visible;
                            tbControleClient.SelectedItem = this.tabItemPersoneAdministration;
                            break;
                        }
                    default:
                        break;
                }
              
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.txtDevis);
            }
        }

        private void btn_RechercheClient_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                prgBar.Visibility = System.Windows.Visibility.Visible;
                if (Txt_ReferenceClient.Text.Length == SessionObject.Enumere.TailleClient)
                    ChargerClientFromReference(this.Txt_ReferenceClient.Text);
                else
                {
                    Message.Show("La reference saisie n'est pas correcte", "Infomation");
                }
            }
            catch (Exception ex)
            {
                Message.ShowInformation(ex.Message, "Demande");
            }
        }
        private void VerifieExisteDemande(CsClient leClient)
        {

            try
            {
                if (!string.IsNullOrEmpty(Txt_ReferenceClient.Text) && Txt_ReferenceClient.Text.Length == SessionObject.Enumere.TailleClient)
                {
                    string OrdreMax = string.Empty;
                    AcceuilServiceClient service = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                    service.RetourneDemandeClientTypeCompleted += (s, args) =>
                    {
                        if (args != null && args.Cancelled)
                            return;
                        if (args.Result != null)
                        {
                            if (args.Result.DATEFIN == null && args.Result.ISSUPPRIME != true)
                            {
                                Message.ShowInformation("Il existe une demande numero " + args.Result.NUMDEM + " sur ce client", "Accueil");
                                prgBar.Visibility = System.Windows.Visibility.Collapsed;
                                return;
                            }
                        }
                        CsClientRechercher leclientRech = new CsClientRechercher()
                        {
                            CENTRE = leClient.CENTRE,
                            CLIENT = leClient.REFCLIENT,
                            ORDRE = leClient.ORDRE,
                            FK_IDCENTRE = leClient.FK_IDCENTRE.Value,
                        };
                        RetourneLeCompteClient(leclientRech);
                        ChargeDetailDEvis(leClient);
                    };
                    service.RetourneDemandeClientTypeAsync(leClient);
                    service.CloseAsync();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        private void ChargerClientFromReference(string ReferenceClient)
        {
            try
            {
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.RetourneClientByReferenceCompleted += (s, args) =>
                {
                    if ((args != null && args.Cancelled) || (args.Error != null))
                        return;
                    if (args.Result != null && args.Result.Count > 1)
                    {
                        List<object> _Listgen = ClasseMEthodeGenerique.RetourneListeObjet(args.Result);
                        Galatee.Silverlight.MainView.UcListeGenerique ctr = new Galatee.Silverlight.MainView.UcListeGenerique(_Listgen, "CENTRE", "LIBELLESITE", "Liste des site");
                        ctr.Show();
                        ctr.Closed += new EventHandler(galatee_OkClickedChoixClient);
                    }
                    else
                    {
                        CsClient leClient = args.Result.First();
                        leClient.TYPEDEMANDE = Tdem;
                        VerifieExisteDemande(leClient);
                    }
                };
                service.RetourneClientByReferenceAsync(ReferenceClient, lstIdCentre);
                service.CloseAsync();

            }
            catch (Exception)
            {
                prgBar.Visibility = System.Windows.Visibility.Collapsed;
                Message.ShowError("Erreur au chargement des données", "Demande");
            }
        }

        private void galatee_OkClickedChoixClient(object sender, EventArgs e)
        {
            Galatee.Silverlight.MainView.UcListeGenerique ctrs = sender as Galatee.Silverlight.MainView.UcListeGenerique;
            if (ctrs.isOkClick)
            {
                CsClient _UnClient = (CsClient)ctrs.MyObject;
                _UnClient.TYPEDEMANDE = Tdem;
                VerifieExisteDemande(_UnClient);
            }
        }

        private List<CsClient> DistinctSiteClient(List<CsClient> lstClient)
        {
            try
            {
                List<CsClient> lstCentreDistClientOrdreProduit = new List<CsClient>();
                var lstCentreDistnct = lstClient.Select(t => new { t.LIBELLESITE, t.FK_IDCENTRE, t.CENTRE, t.REFCLIENT, t.PRODUIT }).Distinct().ToList();
                foreach (var item in lstCentreDistnct)
                {
                    CsClient leClient = new CsClient()
                    {
                        FK_IDCENTRE = item.FK_IDCENTRE,
                        CENTRE = item.CENTRE,
                        REFCLIENT = item.REFCLIENT,
                        PRODUIT = item.PRODUIT
                    };
                    lstCentreDistClientOrdreProduit.Add(leClient);
                }
                return lstCentreDistClientOrdreProduit;
            }
            catch (Exception)
            {

                throw;
            }
        }


    
    }
}

