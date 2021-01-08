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
    public partial class FrmClientAFacturer : ChildWindow
    {
        List<CsCentre> lstCentreCampagne = new List<CsCentre>();
        List<CsCAMPAGNE> lesCampagne = new List<CsCAMPAGNE>();
        ObservableCollection<CsDetailCampagne> lesClientCampagne = new ObservableCollection<CsDetailCampagne>();
        List<CsSite> lstSiteCampagne = new List<CsSite>();
        List<CsSite> lstAgentCampagne = new List<CsSite>();
        public FrmClientAFacturer()
        {
           try 
	        {	        
		         InitializeComponent();
                 InitialiserControle();
                 prgBar.Visibility = System.Windows.Visibility.Collapsed;
	        }
	        catch (Exception ex)
	        {
	        Message.ShowError(ex,Galatee.Silverlight.Resources.Langue.errorTitle);
	        }
        }
 
        List<Galatee.Silverlight.ServiceAccueil.CsCentre> LstCentre = new List<Galatee.Silverlight.ServiceAccueil.CsCentre>();
        List<Galatee.Silverlight.ServiceAccueil.CsSite> lstSite = new List<Galatee.Silverlight.ServiceAccueil.CsSite>();
        private void ChargerDonneeCentre()
        {
            try
            {
                List<int> lstIdCentreClient = new List<int>();
                if (SessionObject.LstCentre.Count != 0)
                {
                    LstCentre = Shared.ClasseMEthodeGenerique.RetourCentreByPerimetre(SessionObject.LstCentre.Where(p => p.CODE != SessionObject.Enumere.Generale).ToList(), UserConnecte.listeProfilUser);
                    lstSite = Shared.ClasseMEthodeGenerique.RetourneSiteByCentre(LstCentre);
                    if (LstCentre != null)
                    {
                        foreach (Galatee.Silverlight.ServiceAccueil.CsCentre item in LstCentre)
                            lstIdCentreClient.Add(item.PK_ID);
                    }
                    ChargerCampagne(lstIdCentreClient);
                    return;
                }
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.ListeDesDonneesDesSiteCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    SessionObject.LstCentre = args.Result;
                    LstCentre = Shared.ClasseMEthodeGenerique.RetourCentreByPerimetre(SessionObject.LstCentre.Where(p => p.CODE != SessionObject.Enumere.Generale).ToList(), UserConnecte.listeProfilUser);
                    lstSite = Galatee.Silverlight.Shared.ClasseMEthodeGenerique.RetourneSiteByCentre(LstCentre);
                    if (LstCentre != null)
                    {
                        List<Galatee.Silverlight.ServiceAccueil.CsCentre> _LstCentre = LstCentre.Where(p => p.CODESITE != SessionObject.Enumere.Generale).ToList();
                        foreach (Galatee.Silverlight.ServiceAccueil.CsCentre item in _LstCentre)
                            lstIdCentreClient.Add(item.PK_ID);
                    }
                    ChargerCampagne(lstIdCentreClient);
                };
                service.ListeDesDonneesDesSiteAsync(false);
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, "LoadCentre");

            }
        }
        public void ChargerCampagne(List<int> lstIdCentre)
        {
            try
            {
                RecouvrementServiceClient client = new RecouvrementServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Recouvrement"));
                client.RetourneDonneesReeditionAvisCoupureCompleted += (es, result) =>
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
                        lesCampagne = result.Result;
                        lstSiteCampagne = RetourneSiteFromCampagne(result.Result);
                        lstCentreCampagne = RetourneCentreFromCampagne(result.Result);
                        lstAgentCampagne = RetourneAgentFromCampagne(result.Result);
                        if (lstSiteCampagne != null && lstSiteCampagne.Count != 0)
                        {
                            cmbSite.ItemsSource = null;
                            cmbSite.ItemsSource = lstSiteCampagne.Where(t => t.CODE != "000").ToList();
                            cmbSite.DisplayMemberPath = "LIBELLE";

                            if (lstSiteCampagne.Count == 1)
                                cmbSite.SelectedItem = lstSiteCampagne[0];
                        }
                        if (lstCentreCampagne != null && lstCentreCampagne.Count != 0)
                        {
                            cmbCentre.ItemsSource = null;
                            cmbCentre.ItemsSource = lstCentreCampagne;
                            cmbCentre.DisplayMemberPath = "LIBELLE";

                            if (lstCentreCampagne.Count == 1)
                                cmbCentre.SelectedItem = lstCentreCampagne[0];
                        }
                        if (lstAgentCampagne != null && lstAgentCampagne.Count != 0)
                        {
                            cmbAgent.ItemsSource = null;
                            cmbAgent.ItemsSource = lstAgentCampagne;
                            cmbAgent.DisplayMemberPath = "LIBELLE";
                        }
                    }
                    catch (Exception ex)
                    {
                        Message.ShowError(ex, Galatee.Silverlight.Resources.Langue.errorTitle);
                    }
                };
                client.RetourneDonneesReeditionAvisCoupureAsync(lstIdCentre);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public List<CsCentre> RetourneCentreFromCampagne(List<CsCAMPAGNE> _lstCampagne)
        {
            try
            {
                List<CsCentre> _lstCentreDistinct = new List<CsCentre>();
                var ListCentreTemp = (from p in _lstCampagne
                                     group new { p } by new { p.CENTRE , p.FK_IDCENTRE,p.LIBELLECENTRE , p.FK_IDSITE,p.CODESITE ,p.LIBELLESITE } into pResult
                                     select new
                                     {
                                         pResult.Key.FK_IDCENTRE ,
                                         pResult.Key.CENTRE ,
                                         pResult.Key.LIBELLECENTRE ,
                                         pResult.Key.FK_IDSITE,
                                         pResult.Key.CODESITE ,
                                         pResult.Key.LIBELLESITE ,
                                     });
                foreach (var item in ListCentreTemp)
                {
                    CsCentre leCentre = new CsCentre()
                    {
                        CODESITE = item.CODESITE ,
                        FK_IDCODESITE = item.FK_IDSITE  ,
                        LIBELLESITE = item.LIBELLESITE ,
                        PK_ID  = item.FK_IDCENTRE ,
                        CODE  = item.CENTRE ,
                        LIBELLE = item.LIBELLECENTRE 
                    };
                    _lstCentreDistinct.Add(leCentre);
                }
                return _lstCentreDistinct;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        public List<CsSite> RetourneSiteFromCampagne(List<CsCAMPAGNE> _lstCampagne)
        {
            try
            {
                List<CsSite> _lstSiteDistinct = new List<CsSite>();
                var ListSiteTemp = (from p in _lstCampagne
                                      group new { p } by new { p.FK_IDSITE, p.CODESITE, p.LIBELLESITE } into pResult
                                      select new
                                      {
                                          pResult.Key.FK_IDSITE,
                                          pResult.Key.CODESITE,
                                          pResult.Key.LIBELLESITE
                                      });
                foreach (var item in ListSiteTemp)
                {
                    CsSite leSite= new CsSite()
                    {
                        CODE = item.CODESITE,
                        PK_ID  = item.FK_IDSITE,
                        LIBELLE  = item.LIBELLESITE
                    };
                    _lstSiteDistinct.Add(leSite);
                }
                return _lstSiteDistinct;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        public List<CsSite> RetourneAgentFromCampagne(List<CsCAMPAGNE> _lstCampagne)
        {
            try
            {
                List<CsSite> _lstSiteDistinct = new List<CsSite>();
                var ListSiteTemp = (from p in _lstCampagne
                                    group new { p } by new { p.MATRICULEPIA, p.AGENTPIA,p.FK_IDMATRICULE } into pResult
                                    select new
                                    {
                                        pResult.Key.MATRICULEPIA,
                                        pResult.Key.AGENTPIA,
                                        pResult.Key.FK_IDMATRICULE 
                                    });
                foreach (var item in ListSiteTemp)
                {
                    CsSite leSite = new CsSite()
                    {
                        CODE = item.MATRICULEPIA,
                        LIBELLE = item.AGENTPIA,
                        PK_ID = item.FK_IDMATRICULE 
                    };
                    _lstSiteDistinct.Add(leSite);
                }
                return _lstSiteDistinct;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        private void InitialiserControle()
        {
            try
            {
                ChargerDonneeCentre();
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
 

        public string Campaign
        {
            get { return campaign; }
            set { campaign = value; }
        }

        bool _returnvalue = false;

 

        List<aParam> _campagnes = null;
        [Description("Retourne les campagnes issues de la recherche ou sélectionnées dans la liste")]
        public List<aParam> campagnes
        {
            get { return this._campagnes; }
        }


        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<CsDetailCampagne> lstClientEnRendezVous = lesClientCampagne.Where(t => t.IsSelect == true).ToList();
                if (lstClientEnRendezVous == null || lstClientEnRendezVous.Count == 0) return;
                Utility.ActionDirectOrientation<ServicePrintings.CsDetailCampagne, ServiceRecouvrement.CsDetailCampagne>(lstClientEnRendezVous, null, SessionObject.CheminImpression, "ClientFacture", "Recouvrement", true);
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, Galatee.Silverlight.Resources.Langue.errorTitle);
            }
        }
        private void ExportFile_Click_1(object sender, RoutedEventArgs e)
        {
            Dictionary<string, string> param = new Dictionary<string, string>();
            string key = Utility.getKey();

            List<CsDetailCampagne> lstClientEnRendezVous = lesClientCampagne.Where(t => t.IsSelect == true).ToList();
            if (lstClientEnRendezVous == null || lstClientEnRendezVous.Count == 0) return;
            Utility.ActionExportation<ServicePrintings.CsDetailCampagne, ServiceRecouvrement.CsDetailCampagne>(lstClientEnRendezVous, null, string.Empty, SessionObject.CheminImpression, "ClientFacture", "Recouvrement", true, "xlsx");


        }
        private void ReinitialiserGrid()
        {
            try
            {
                int indexElementSelected = this.lvwResultat.SelectedIndex + 1;
                if (indexElementSelected <= lesClientCampagne.Count() - 1)
                {
                    lvwResultat.IsReadOnly = true;
                    lvwResultat.SelectedIndex = indexElementSelected;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        List<DataGridRow> Rows = new List<DataGridRow>();
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
        public static FrameworkElement SearchFrameworkElement(FrameworkElement parentFrameworkElement, string childFrameworkElementNameToSearch)
        {
            try
            {
                FrameworkElement childFrameworkElementFound = null;
                SearchFrameworkElement(parentFrameworkElement, ref childFrameworkElementFound, childFrameworkElementNameToSearch);
                return childFrameworkElementFound;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static void SearchFrameworkElement(FrameworkElement parentFrameworkElement, ref FrameworkElement childFrameworkElementToFind, string childFrameworkElementName)
        {
            try
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

         void Recherche(CsCAMPAGNE laCampagneSelect)
         {
             try
             {
                 prgBar.Visibility = System.Windows.Visibility.Visible ;

                 List<CsCAMPAGNE> lesCampagne = new List<CsCAMPAGNE>();
                 lesCampagne.Add(laCampagneSelect);
                 RecouvrementServiceClient client = new RecouvrementServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Recouvrement"));
                 client.ClientAFactureCampagneAsync(lesCampagne);
                 client.ClientAFactureCampagneCompleted += (ss, args) =>
                     {
                         try
                         {
                             this.btnsearch.IsEnabled = true;
                             prgBar.Visibility = System.Windows.Visibility.Collapsed ;
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
                             detailcampagnes=args.Result;
                             foreach (CsDetailCampagne item in detailcampagnes)
                                 lesClientCampagne.Add(item); 

                             this.lvwResultat.ItemsSource = null;
                             this.lvwResultat.ItemsSource = lesClientCampagne;
                             this.lvwResultat.SelectedIndex   = 0;

                             OKButton.Visibility = System.Windows.Visibility.Visible;

                         }
                         catch (Exception ex)
                         {
                             this.btnsearch.IsEnabled = true ;

                             Message.ShowError(ex, Galatee.Silverlight.Resources.Langue.errorTitle);
                         }
                     };

                
             }
             catch (Exception ex)
             {
                 throw ex;
             }
         }

         private void cmbCentre_SelectionChanged(object sender, SelectionChangedEventArgs e)
         {
             try
             {
                 if ((this.cmbCentre.SelectedItem != null) &&
                         ((this.cmbCentre.SelectedItem as Galatee.Silverlight.ServiceRecouvrement.CsCentre).CODE != string.Empty))
                 {
                     CsCentre lstCentreSelect = this.cmbCentre.SelectedItem as Galatee.Silverlight.ServiceRecouvrement.CsCentre;
                     this.txtCentre.Text = lstCentreSelect.CODE;
                     this.txtCentre.Tag  = lstCentreSelect.PK_ID ;



                     List<CsCAMPAGNE> lstCampagne = lesCampagne.Where(t => t.CENTRE == lstCentreSelect.CODE && t.FK_IDCENTRE  == lstCentreSelect.PK_ID).ToList();
                     List<CsSite> lsteAgentCampagne = RetourneAgentFromCampagne(lstCampagne);
                     this.cmbAgent.ItemsSource = null;
                     this.cmbAgent.ItemsSource = lsteAgentCampagne;

                     this.cmbCampagne.ItemsSource = null;
                     this.cmbCampagne.DisplayMemberPath  = "IDCOUPURE";
                     this.cmbCampagne.ItemsSource = lstCampagne;
                 }
             }
             catch (Exception ex)
             {
                 Message.ShowError(ex, Galatee.Silverlight.Resources.Langue.errorTitle);
             }
         }

         private void cmbAgent_SelectionChanged(object sender, SelectionChangedEventArgs e)
         {
             try
             {
                 if ((this.cmbAgent.SelectedItem != null) &&
                     ((this.cmbAgent.SelectedItem as CsSite).LIBELLE != string.Empty))
                 {
                     CsSite leAgentSelect = this.cmbAgent.SelectedItem as CsSite;
                     this.txtAgent.Text = leAgentSelect.CODE;
                     this.txtAgent.Tag  = leAgentSelect.PK_ID ;



                     List<CsCAMPAGNE> lstCampagne = lesCampagne.Where(t => t.FK_IDMATRICULE == leAgentSelect.PK_ID ).ToList();
                     List<CsSite> lsteAgentCampagne = RetourneAgentFromCampagne(lstCampagne);
                     this.cmbCampagne .ItemsSource = null;
                     this.cmbCampagne.ItemsSource = lstCampagne;
                 }
             }
             catch (Exception ex)
             {
                 Message.ShowError(ex, Galatee.Silverlight.Resources.Langue.errorTitle);
             }
         }

         private void btnsearch_Click(object sender, RoutedEventArgs e)
         {
             try
             {
                 this.btnsearch.IsEnabled = false;
                 if (CampagneSelect != null)
                 {
                     if (this.lvwResultat.ItemsSource == null)
                         Recherche(CampagneSelect );
                     else
                     {
                         if (ClientRechercheSelect == null  || string.IsNullOrEmpty(ClientRechercheSelect.ORDRE )) return ;
                         ObservableCollection<CsDetailCampagne> lesClientCampagneClient = new ObservableCollection<CsDetailCampagne>();
                         List<CsDetailCampagne> lesClientCamp = lesClientCampagne.Where(t => t.CLIENT == ClientRechercheSelect.REFCLIENT && t.ORDRE == ClientRechercheSelect.ORDRE).ToList();
                         foreach (CsDetailCampagne item in lesClientCamp)
                             lesClientCampagneClient.Add(item);

                         this.lvwResultat.ItemsSource = null;
                         this.lvwResultat.ItemsSource = lesClientCampagneClient;
                         this.lvwResultat.SelectedItem = lesClientCampagne[0];
                     }

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

         private void cmbSite_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
         {
             try
             {
                 if ((this.cmbSite.SelectedItem != null) &&
                         ((this.cmbSite.SelectedItem as Galatee.Silverlight.ServiceRecouvrement.CsSite).CODE != string.Empty))
                 {
                     CsSite leSiteSelect = this.cmbSite.SelectedItem as Galatee.Silverlight.ServiceRecouvrement.CsSite;
                     this.txtSite.Text = leSiteSelect.CODE ;
                     this.txtSite.Tag = leSiteSelect.PK_ID ;
                     List<CsCAMPAGNE> lstCampagne = lesCampagne.Where(t => t.CODESITE == leSiteSelect.CODE && t.FK_IDSITE  == leSiteSelect.PK_ID).ToList();
                     lstCentreCampagne = RetourneCentreFromCampagne(lstCampagne);
                     this.cmbCentre.ItemsSource = null;
                     this.cmbCentre.ItemsSource = lstCentreCampagne;
                     lstAgentCampagne = RetourneAgentFromCampagne(lstCampagne);
                     this.cmbAgent.ItemsSource = null;
                     this.cmbAgent.ItemsSource = lstAgentCampagne;

                     this.cmbCampagne.ItemsSource = null;
                     this.cmbCampagne.ItemsSource = lstCampagne;
                 }
             }
             catch (Exception ex)
             {
                 Message.ShowError(ex, Galatee.Silverlight.Resources.Langue.errorTitle);
             }
         }

         private void cmbCampagne_SelectionChanged(object sender, SelectionChangedEventArgs e)
         {
             CampagneSelect = (CsCAMPAGNE)this.cmbCampagne.SelectedItem;
         }

         private void dgMyDataGrid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
         {
             DataGrid dg = (sender as DataGrid);
             //var allObjects = ((ObservableCollection<CsDetailCampagne>)dg.ItemsSource).ToList();
             //foreach (var item in allObjects)
             //    item.IsSelect = false;
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
     
    }
}

