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
using Galatee.Silverlight.ServiceRecouvrement;
//using Galatee.Silverlight.ServicePrintings;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace Galatee.Silverlight.Recouvrement
{
    public partial class FrmAutorisationPaiementPose : ChildWindow
    {
        List<CsCentre> lstCentreCampagne = new List<CsCentre>();
        ObservableCollection<CsDetailCampagne> lesClientCampagne = new ObservableCollection<CsDetailCampagne>();
        List<CsSite> lstSiteCampagne = new List<CsSite>();
        List<CsSite> lstAgentCampagne = new List<CsSite>();
        public FrmAutorisationPaiementPose()
        {
            try
            {
                InitializeComponent();
                InitialiserControle();
                ChargeTypeCoupure();
                ChargerDonneeDuSite();
                prgBar.Visibility = Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, Galatee.Silverlight.Resources.Langue.errorTitle);
            }
        }
        private void InitialiserControle()
        {
            try
            {
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        List<int> lesCentreCaisse = new List<int>();
        List<Galatee.Silverlight.ServiceAccueil.CsCentre> LstCentrePerimetre = new List<ServiceAccueil.CsCentre>();
        List<Galatee.Silverlight.ServiceAccueil.CsSite> lstSite = new List<ServiceAccueil.CsSite>();
        private void ChargerDonneeDuSite()
        {
            try
            {
                List<int> lstCentreSelect = new List<int>();

                if (SessionObject.LstCentre != null && SessionObject.LstCentre.Count > 0)
                {
                    LstCentrePerimetre = Shared.ClasseMEthodeGenerique.RetourCentreByPerimetre(SessionObject.LstCentre.Where(p => p.CODE != SessionObject.Enumere.Generale).ToList(), UserConnecte.listeProfilUser);
                    lstSite = Shared.ClasseMEthodeGenerique.RetourneSiteByCentre(LstCentrePerimetre);
                    foreach (Galatee.Silverlight.ServiceAccueil.CsCentre item in LstCentrePerimetre)
                        lesCentreCaisse.Add(item.PK_ID);
                    if (lstSite.Count == 1)
                    {
                        this.Txt_CodeSite.Text = lstSite.First().CODE;
                        this.Txt_LibelleSite.Text = lstSite.First().LIBELLE;
                        this.Txt_CodeSite.Tag = lstSite.First().PK_ID;
                        ChargerPiaAgence(lstSite.First().CODE);
                        this.btn_Site.IsEnabled = false;
                    }
                    return;
                }
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.ListeDesDonneesDesSiteCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    SessionObject.LstCentre = args.Result;
                    LstCentrePerimetre = Shared.ClasseMEthodeGenerique.RetourCentreByPerimetre(SessionObject.LstCentre.Where(p => p.CODE != SessionObject.Enumere.Generale).ToList(), UserConnecte.listeProfilUser);
                    lstSite = Shared.ClasseMEthodeGenerique.RetourneSiteByCentre(LstCentrePerimetre);
                    foreach (Galatee.Silverlight.ServiceAccueil.CsCentre item in LstCentrePerimetre)
                        lesCentreCaisse.Add(item.PK_ID);
                    if (lstSite.Count == 1)
                    {
                        this.Txt_CodeSite.Text = lstSite.First().CODE;
                        this.Txt_LibelleSite.Text = lstSite.First().LIBELLE;
                        this.Txt_CodeSite.Tag = lstSite.First().PK_ID;
                        ChargerPiaAgence(lstSite.First().CODE);
                        this.btn_Site.IsEnabled = false;
                    }
                    return;
                };
                service.ListeDesDonneesDesSiteAsync(false);
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void btn_Site_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (lstSite.Count > 0)
                {
                    this.btn_Site.IsEnabled = false;
                    Dictionary<string, string> _LstColonneAffich = new Dictionary<string, string>();
                    _LstColonneAffich.Add("CODE", "SITE");
                    _LstColonneAffich.Add("LIBELLE", "LIBELLE");

                    List<object> obj = Shared.ClasseMEthodeGenerique.RetourneListeObjet(lstSite);
                    MainView.UcListeGenerique ctrl = new MainView.UcListeGenerique(obj, _LstColonneAffich, false, "Lots");
                    ctrl.Closed += new EventHandler(galatee_OkClickedSite);
                    ctrl.Show();
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, "Recouvrement");
            }
        }
        private void Txt_CodeSite_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(Txt_CodeSite.Text) && Txt_CodeSite.Text.Length == SessionObject.Enumere.TailleCentre)
                {
                    Galatee.Silverlight.ServiceAccueil.CsSite _LeSiteClient = Shared.ClasseMEthodeGenerique.RetourneObjectFromList(lstSite, this.Txt_CodeSite.Text, "CODE");
                    if (!string.IsNullOrEmpty(_LeSiteClient.LIBELLE))
                    {
                        this.Txt_LibelleSite.Text = _LeSiteClient.LIBELLE;
                        this.Txt_CodeSite.Text = _LeSiteClient.CODE;
                        this.Txt_CodeSite.Tag = _LeSiteClient.PK_ID;
                        ChargerPiaAgence(lstSite.First().CODE);
                    }
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, "Facturation");

            }
        }

        private void ChargerPiaAgence(string CodeSite)
        {
            try
            {

                Galatee.Silverlight.ServiceRecouvrement.RecouvrementServiceClient service = new Galatee.Silverlight.ServiceRecouvrement.RecouvrementServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Recouvrement"));
                service.RetournePIAAgenceCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    List<CsUtilisateur> lstPia = new List<CsUtilisateur>();
                    lstPia.Add(new CsUtilisateur() { LIBELLE = "Aucun" });
                    lstPia.AddRange(args.Result);

                    this.cmbAgent.ItemsSource = null;
                    this.cmbAgent.ItemsSource = lstPia;
                    this.cmbAgent.DisplayMemberPath = "LIBELLE";
                    return;
                };
                service.RetournePIAAgenceAsync(CodeSite);
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        void galatee_OkClickedSite(object sender, EventArgs e)
        {
            Galatee.Silverlight.MainView.UcListeGenerique ctrs = sender as Galatee.Silverlight.MainView.UcListeGenerique;
            if (ctrs.isOkClick)
            {
                this.btn_Site.IsEnabled = true;
                Galatee.Silverlight.ServiceAccueil.CsSite leSite = (Galatee.Silverlight.ServiceAccueil.CsSite)ctrs.MyObject;
                this.Txt_CodeSite.Text = leSite.CODE;
                this.Txt_CodeSite.Tag = leSite.PK_ID;
                this.Txt_LibelleSite.Text = leSite.LIBELLE;
            }
            this.btn_Site.IsEnabled = true;
        }
        List<Galatee.Silverlight.ServiceAccueil.CsTypeCoupure> lstTypeCoupure = new List<Galatee.Silverlight.ServiceAccueil.CsTypeCoupure>();
        private void ChargeTypeCoupure()
        {
            try
            {
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient client = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                client.RetourneTypeCoupureAsync();
                client.RetourneTypeCoupureCompleted += (es, result) =>
                {
                    try
                    {
                        if (result.Cancelled || result.Error != null)
                        {
                            string error = result.Error.Message;
                            Message.ShowError("Erreur à l'exécution du service", "SelectCentreCampagne");
                            return;
                        }

                        if (result.Result == null)
                        {
                            Message.ShowInformation("Aucune donnée trouvée", "SelectCentreCampagne");
                            return;
                        }
                        lstTypeCoupure = result.Result;
                    }
                    catch (Exception ex)
                    {
                        Message.ShowError(ex, Galatee.Silverlight.Resources.Langue.errorTitle);
                    }
                };

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public event EventHandler Closed;

        string campaign = string.Empty;
        public bool IsGettingIdCoupure = false;
        public void ClosedEnventHandler()
        {
            if (this.Closed != null)
            {
                this.Closed(this, null);
            }
        }

        public CsDetailCampagne ClientSelect;

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                Message.ShowError(ex, Galatee.Silverlight.Resources.Langue.errorTitle);
            }
            //this.DialogResult = true;
        }
        private Galatee.Silverlight.ServiceAccueil.CsLclient GetElementDeFrais(CsDetailCampagne Campagne)
        {
            Galatee.Silverlight.ServiceAccueil.CsLclient Frais = new Galatee.Silverlight.ServiceAccueil.CsLclient();
            try
            {
                Frais.CENTRE = Campagne.CENTRE;
                Frais.CLIENT = Campagne.CLIENT;
                Frais.ORDRE = Campagne.ORDRE;
                Frais.REFEM = DateTime.Today.Date.Year.ToString() + DateTime.Today.Date.Month.ToString("00");
                Frais.IDCOUPURE = Campagne.IDCOUPURE;
                Frais.COPER = SessionObject.Enumere.CoperFRP;
                Frais.DENR = DateTime.Today.Date;
                Frais.EXIGIBILITE = DateTime.Today.Date;
                Frais.DATECREATION = DateTime.Today.Date;
                Frais.DATEMODIFICATION = DateTime.Today.Date;
                Frais.DC = SessionObject.Enumere.Debit;
                Frais.FK_IDCENTRE = Campagne.FK_IDCENTRE;
                Frais.FK_IDCLIENT = Campagne.FK_IDCLIENT;
                Frais.MATRICULE = UserConnecte.matricule;
                Frais.MOISCOMPT = DateTime.Today.Date.Year.ToString() + DateTime.Today.Date.Month.ToString("00");
                Frais.MONTANT = Campagne.MONTANTFRAIS;
                Frais.TOP1 = "0";


                return Frais;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        private void btnReinitialiser_Click(object sender, EventArgs e)
        {
            try
            {
                this.lvwResultat.ItemsSource = null;
                //this.cmbCentre.SelectedItem = lCentre.First();
                this.dtpDate.Text = null;
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, Galatee.Silverlight.Resources.Langue.errorTitle);
            }
        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            ClosedEnventHandler();
            this.DialogResult = false;
        }
        void Recherche(CsCAMPAGNE laCampagneSelect, CsClient LeClientRechercheSelect)
        {
            try
            {
                RecouvrementServiceClient client = new RecouvrementServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Recouvrement"));
                client.RetourneDonneeAnnulationFraisAsync(laCampagneSelect, LeClientRechercheSelect);
                client.RetourneDonneeAnnulationFraisCompleted += (ss, args) =>
                {
                    try
                    {
                        if (args.Cancelled || args.Error != null)
                        {
                            string error = args.Error.Message;
                            Message.ShowError("Erreur à l'exécution du service", "SearchCampagne");
                            return;
                        }

                        if (args.Result == null || args.Result.Count == 0)
                        {
                            Message.ShowInformation("Aucune donnée trouvée", "SearchCampagne");
                            return;
                        }

                        List<CsDetailCampagne> detailcampagnes = new List<CsDetailCampagne>();
                        detailcampagnes = args.Result;
                        lesClientCampagne.Clear();
                        foreach (CsDetailCampagne item in detailcampagnes)
                            lesClientCampagne.Add(item);

                        this.lvwResultat.ItemsSource = null;
                        this.lvwResultat.ItemsSource = lesClientCampagne;

                    }
                    catch (Exception ex)
                    {
                        Message.ShowError(ex, Galatee.Silverlight.Resources.Langue.errorTitle);
                    }
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void cmbAgent_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (cmbAgent.SelectedItem != null)
                {
                    this.txtAgent.Tag = null;
                    CsUtilisateur leUser = ((CsUtilisateur)cmbAgent.SelectedItem);
                    if (leUser.LIBELLE != "Aucun")
                    {
                        this.txtAgent.Text = leUser.MATRICULE;
                        this.txtAgent.Tag = leUser.PK_ID;
                    }
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, Galatee.Silverlight.Resources.Langue.errorTitle);
            }
        }
        CsClient ClientRechercheSelect =new CsClient();
        private void btnsearch_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string CodeSite = string.Empty; string IdCampagne = string.Empty; int IdPia = 0; DateTime? DateDebut = null; DateTime? DateFin = null; string Centre = null; string Client = null; string Ordre = null;
                if (!string.IsNullOrEmpty(this.Txt_CodeSite.Text)) CodeSite = this.Txt_CodeSite.Text;
                if (!string.IsNullOrEmpty(this.Txt_NumCampagne.Text)) IdCampagne = this.Txt_NumCampagne.Text;
                if (cmbAgent.SelectedItem != null) IdPia = ((CsUtilisateur)cmbAgent.SelectedItem).PK_ID;
                if (this.dtpDate.SelectedDate != null) DateDebut = this.dtpDate.SelectedDate.Value;
                if (this.dtpDateFin.SelectedDate != null) DateFin = this.dtpDateFin.SelectedDate.Value;
                if (!string.IsNullOrEmpty(this.txtReferenceClient.Text)) Client = this.txtReferenceClient.Text;
                if (!string.IsNullOrEmpty(this.txtOrdeClient.Text)) Ordre = this.txtOrdeClient.Text;
                Recherche(CodeSite, IdCampagne, IdPia, DateDebut, DateFin, Centre, Client, Ordre);  
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, Galatee.Silverlight.Resources.Langue.errorTitle);
            }
        }
        void Recherche(string CodeSite, string IdCampagne, int IdPia, DateTime? DateDebut, DateTime? DateFin, string Centre, string Client, string Ordre)
        {
            try
            {
                prgBar.Visibility = Visibility.Visible ;

                RecouvrementServiceClient client = new RecouvrementServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Recouvrement"));
                client.RechercheClientCampagneCompleted += (ss, args) =>
                {
                    try
                    {
                        prgBar.Visibility = Visibility.Collapsed ;
                        if (args.Cancelled || args.Error != null)
                        {
                            string error = args.Error.Message;
                            Message.ShowError("Erreur à l'exécution du service", "SearchCampagne");
                            return;
                        }

                        if (args.Result == null || args.Result.Count == 0)
                        {
                            Message.ShowInformation("Aucune donnée trouvée", "SearchCampagne");
                            return;
                        }

                        List<CsDetailCampagne> detailcampagnes = new List<CsDetailCampagne>();
                        detailcampagnes = args.Result;
                        lesClientCampagne.Clear();
                        foreach (CsDetailCampagne item in detailcampagnes)
                            lesClientCampagne.Add(item);

                        this.lvwResultat.ItemsSource = null;
                        this.lvwResultat.ItemsSource = lesClientCampagne;


                    }
                    catch (Exception ex)
                    {
                        Message.ShowError(ex, Galatee.Silverlight.Resources.Langue.errorTitle);
                    }
                };
                client.RechercheClientCampagneAsync(CodeSite, IdCampagne, IdPia, DateDebut, DateFin, Centre, Client, Ordre,7);


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        private void lvwResultat_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                ClientSelect = lvwResultat.SelectedItem as CsDetailCampagne;
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, Galatee.Silverlight.Resources.Langue.errorTitle);
            }
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (lvwResultat.ItemsSource != null)
                {
                    List<CsDetailCampagne> lstClientSelect = new List<CsDetailCampagne>();
                    lstClientSelect.Add(lvwResultat.SelectedItem as CsDetailCampagne);
                    lstClientSelect.ForEach(o => o.ISNONENCAISSABLE = false);
                    ValiderAutorisation(lstClientSelect);
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, Galatee.Silverlight.Resources.Langue.errorTitle);
            }
        }
        private void ValiderAutorisation(List<CsDetailCampagne> Lst)
        {
            try
            {
                CsDetailCampagne leClient = (CsDetailCampagne)lvwResultat.SelectedItem;
                RecouvrementServiceClient client = new RecouvrementServiceClient(Utility.ProtocoleIndex(), Utility.EndPoint("Recouvrement"));
                client.ValidationAutorisationFraisCompleted += (ss, ress) =>
                {
                    try
                    {
                        if (ress.Cancelled || ress.Error != null)
                        {
                            Message.ShowError("Erreur survenue lors de l'appel service", "Informations");
                            return;
                        }

                        if (ress.Result == false)
                        {
                            Message.ShowInformation("Erreur lors de l'insertion des index de campagne! Veuillez réessayer svp ", "Informations");
                            return;
                        }
                        if (ress.Result == true)
                        {
                            Message.ShowInformation("Mise à jour validée ", "Informations");
                            ReloadDatagraid();
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        Message.ShowError(ex, "Erreur");
                    }
                };
                client.ValidationAutorisationFraisAsync(Lst);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void ReloadDatagraid()
        {
            try
            {
                lvwResultat.IsReadOnly = false;
                ObservableCollection<CsDetailCampagne> listCouranteDansLaGridTemp = new ObservableCollection<CsDetailCampagne>();

                List<CsDetailCampagne> lstNonSaisie = lesClientCampagne.Where(f => f.ISNONENCAISSABLE == null).ToList();
                foreach (CsDetailCampagne item in lstNonSaisie)
                    listCouranteDansLaGridTemp.Add(item);
                if (listCouranteDansLaGridTemp.Count != 0)
                    lvwResultat.SelectedItem = listCouranteDansLaGridTemp.First();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void btnreset_Click(object sender, RoutedEventArgs e)
        {

        }
        private void galatee_OkClickedClient(object sender, EventArgs e)
        {
            FrmMotifAnnulation ctrs = sender as FrmMotifAnnulation;
            if (!string.IsNullOrEmpty( ctrs.txt_MotifAnnulation.Text ))
            {
                CsDetailCampagne LeEvtSelect = (CsDetailCampagne)this.lvwResultat.SelectedItem;
                LeEvtSelect.MOTIFANNULATION = ctrs.txt_MotifAnnulation.Text;
                LeEvtSelect.ISANNULATIONFRAIS = true;
                LeEvtSelect.USERMODIFICATION   = UserConnecte.matricule ;
              
            }
        }
        List<DataGridRow> Rows = new List<DataGridRow>();
        public static FrameworkElement SearchFrameworkElement(FrameworkElement parentFrameworkElement, string childFrameworkElementNameToSearch)
        {
            FrameworkElement childFrameworkElementFound = null;
            SearchFrameworkElement(parentFrameworkElement, ref childFrameworkElementFound, childFrameworkElementNameToSearch);
            return childFrameworkElementFound;
        }
        private static void SearchFrameworkElement(FrameworkElement parentFrameworkElement, ref FrameworkElement childFrameworkElementToFind, string childFrameworkElementName)
        {
            int childrenCount = VisualTreeHelper.GetChildrenCount(parentFrameworkElement);
            if (childrenCount > 0)
            {
                FrameworkElement childFrameworkElement = null;
                for (int i = 0; i < childrenCount; i++)
                {
                    childFrameworkElement = (FrameworkElement)VisualTreeHelper.GetChild(parentFrameworkElement, i);
                    if (childFrameworkElement != null && childFrameworkElement.Name.Equals(childFrameworkElementName))
                    {
                        childFrameworkElementToFind = childFrameworkElement;
                        return;
                    }
                    SearchFrameworkElement(childFrameworkElement, ref childFrameworkElementToFind, childFrameworkElementName);
                }
            }
        }
        private void ChangeSelectedItemColor()
        {
            try
            {
                //to get the current row binding value
                CsDetailCampagne currentRow = (CsDetailCampagne)lvwResultat.SelectedItem;

                //to read the currentRow
                DataGridRow selectedRow = Rows[lvwResultat.SelectedIndex];
                //color row
                var backgroundRectangle = SearchFrameworkElement(selectedRow, "BackgroundRectangle") as Rectangle;
                if (backgroundRectangle != null)
                {
                    backgroundRectangle.Fill = new SolidColorBrush(Colors.Cyan);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void dgMyDataGrid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            DataGrid dg = (sender as DataGrid);
            var allObjects = ((ObservableCollection<CsDetailCampagne>)dg.ItemsSource).ToList();
            foreach (var item in allObjects)
                item.IsSelect = false;
            if (dg.SelectedItem != null)
            {
                CsDetailCampagne SelectedObject = (CsDetailCampagne)dg.SelectedItem;
                if (SelectedObject.IsSelect == false)
                    SelectedObject.IsSelect = true;
                else
                    SelectedObject.IsSelect = false;
            }
        }
        private void chk_Unchecked_1(object sender, RoutedEventArgs e)
        {
            var lst = ((ObservableCollection<CsDetailCampagne>)lvwResultat.ItemsSource).ToList();
            if (lst != null && this.lvwResultat.SelectedItem != null)
            {
                CsDetailCampagne laSelect = (CsDetailCampagne)this.lvwResultat.SelectedItem;
                if (laSelect == null)
                {
                    Message.ShowInformation("Sélectionner la catégorie", "Index");
                    return;
                }
                laSelect.IsSelect = false;
            }
        }

        private void chk_Checked_1(object sender, RoutedEventArgs e)
        {
            try
            {
                if (lvwResultat.ItemsSource != null)
                {
                    var lst = ((ObservableCollection<CsDetailCampagne>)lvwResultat.ItemsSource).ToList();
                    if (lst != null && this.lvwResultat.SelectedItem != null)
                    {
                        CsDetailCampagne laSelect = (CsDetailCampagne)this.lvwResultat.SelectedItem;
                        if (laSelect == null)
                        {
                            Message.ShowInformation("Sélectionner la catégorie", "Index");
                            return;
                        }
                        laSelect.IsSelect = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, "Erreur");
            }
        }

        private void Txt_NumCampagne_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}

