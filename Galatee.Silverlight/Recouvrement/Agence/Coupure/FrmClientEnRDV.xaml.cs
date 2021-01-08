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
    public partial class FrmClientEnRDV : ChildWindow
    {
        List<CsCentre> lstCentreCampagne = new List<CsCentre>();
        List<CsCAMPAGNE> lesCampagne = new List<CsCAMPAGNE>();
        ObservableCollection<CsDetailCampagne> lesClientCampagne = new ObservableCollection<CsDetailCampagne>();
        List<CsSite> lstSiteCampagne = new List<CsSite>();
        List<CsSite> lstAgentCampagne = new List<CsSite>();
        public FrmClientEnRDV()
        {
           try 
	        {	        
		         InitializeComponent();
                 InitialiserControle();
	        }
	        catch (Exception ex)
	        {
	        Message.ShowError(ex,Galatee.Silverlight.Resources.Langue.errorTitle);
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
                    }
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, "Facturation");

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

        private void InitialiserControle()
        {
            try
            {
                ChargerDonneeDuSite();
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
            if (this.Closed!=null)
	        {
                this.Closed(this, null);
	        }
        }

        public CsCAMPAGNE CampagneSelect;
        public CsDetailCampagne  ClientSelect;
        public CsClient ClientRechercheSelect;
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (lvwResultat.ItemsSource != null)
                {
                    List<CsDetailCampagne> ligne = ((ObservableCollection<CsDetailCampagne>)lvwResultat.ItemsSource).ToList();
                    Utility.ActionDirectOrientation<ServicePrintings.CsDetailCampagne, ServiceRecouvrement.CsDetailCampagne>(ligne, null, SessionObject.CheminImpression, "ClientEnRendezVous", "Recouvrement", true);
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, Galatee.Silverlight.Resources.Langue.errorTitle);
            }
        }
        private void btnReinitialiser_Click(object sender, EventArgs e)
        {
            try
            {
                this.lvwResultat.ItemsSource = null;
                //this.cmbCentre.SelectedItem = lCentre.First();
                this.dtpDateDebut.Text = null;
                this.dtpDateFin.Text = null;
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, Galatee.Silverlight.Resources.Langue.errorTitle);
            }
        }
         private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Shared.ClasseMEthodeGenerique.FermetureEcran(this);
        }

         List<CsDetailCampagne> detailcampagnes = new List<CsDetailCampagne>();
         void Recherche (string SiteSelect , DateTime? datedebut,DateTime? datefin )
         {
             try
             {
                 RecouvrementServiceClient client = new RecouvrementServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Recouvrement"));
                 client.ListeDesClientEnRDVCompleted += (ss, args) =>
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
                             detailcampagnes = args.Result;
                             lvwResultat.ItemsSource = null;
                             lvwResultat.ItemsSource = detailcampagnes;
                         }
                         catch (Exception ex)
                         {
                             Message.ShowError(ex, Galatee.Silverlight.Resources.Langue.errorTitle);
                         }
                     };
                 client.ListeDesClientEnRDVAsync(SiteSelect,datedebut, datefin);
             }
             catch (Exception ex)
             {
                 throw ex;
             }
         }

 

   

         private void btnsearch_Click(object sender, RoutedEventArgs e)
         {
             try
             {
                 if (this.dtpDateDebut.SelectedDate != null && this.dtpDateFin.SelectedDate != null && !string.IsNullOrEmpty(this.Txt_CodeSite.Text))
                     Recherche(this.Txt_CodeSite.Text,this.dtpDateDebut.SelectedDate, this.dtpDateFin.SelectedDate);
             }
             catch (Exception ex)
             {
                 Message.ShowError(ex, Galatee.Silverlight.Resources.Langue.errorTitle);
             }
         }

         private void lvwResultat_SelectionChanged(object sender, SelectionChangedEventArgs e)
         {
             try
             {
                 ClientSelect = lvwResultat.SelectedItem as CsDetailCampagne;
                 if (ClientSelect != null)
                 {
                     this.dtpDateDebut.Text = ClientSelect.DATECREATION.Value .ToShortDateString();
                 }
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
                 if (Closed != null)
                 {
                     campaign = CampagneSelect.IDCOUPURE ;
                     Closed(this, new EventArgs());
                     this.DialogResult = true;
                 }
             }
             catch (Exception ex)
             {
              Message.ShowError(ex, Galatee.Silverlight.Resources.Langue.errorTitle);
             }
         }

         private void btnreset_Click(object sender, RoutedEventArgs e)
         {

         }

         private void cmbCampagne_SelectionChanged(object sender, SelectionChangedEventArgs e)
         {
        
         }

         private void dgMyDataGrid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
         {
             DataGrid dg = (sender as DataGrid);
             var allObjects = dg.ItemsSource as List<CsLclient>;
             if (dg.SelectedItem != null)
             {
                 CsDetailCampagne SelectedObject = (CsDetailCampagne)dg.SelectedItem;

                 if (SelectedObject.IsSelect == false)
                 {
                     SelectedObject.IsSelect = true;
                 }
                 else
                     SelectedObject.IsSelect = false;
             }
         }

         private void btnExporter_Click_1(object sender, RoutedEventArgs e)
         {
             List<CsDetailCampagne> ligne = ((ObservableCollection<CsDetailCampagne>)lvwResultat.ItemsSource).ToList();
             Utility.ActionExportation<ServicePrintings.CsDetailCampagne, ServiceRecouvrement.CsDetailCampagne>(ligne, null, string.Empty, SessionObject.CheminImpression, "Campagnes", "Recouvrement", true, "xlsx");
         }
    }
}

