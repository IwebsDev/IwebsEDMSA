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

namespace Galatee.Silverlight.Recouvrement
{
    public partial class FrmReEditionAvisCoupureSGC : ChildWindow
    {
        List<CsCentre> lstCentreCampagne = new List<CsCentre>();
        List<CsCAMPAGNE> lesCampagne = new List<CsCAMPAGNE>();
        List<CsSite> lstSiteCampagne = new List<CsSite>();
        List<CsSite> lstAgentCampagne = new List<CsSite>();
        public FrmReEditionAvisCoupureSGC()
        {
           try 
	        {	        
		         InitializeComponent();
                 InitialiserControle();
                 this.Rdb_Detail.IsChecked = true;
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
        aParam client;
        public bool IsGettingIdCoupure = false;
        public void ClosedEnventHandler()
        {
            if (this.Closed!=null)
	        {
                this.Closed(this, null);
	        }
        }

        public CsCAMPAGNE CampagneSelect;
 

        public string Campaign
        {
            get { return campaign; }
            set { campaign = value; }
        }

        bool _desbaleprint = false;
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
                bool IsListe = false;
                if (this.Rdb_Liste .IsChecked == true)
                    IsListe = true;
                else
                    IsListe = false;
               
                if (this.lvwResultat.SelectedItem != null )
                {
                    prgBar.Visibility = System.Windows.Visibility.Visible;
                    LayoutRoot.Cursor = Cursors.Wait;
                    List<ServiceRecouvrement.CsAvisDeCoupureClient> ligne = new List<ServiceRecouvrement.CsAvisDeCoupureClient>();
                    RecouvrementServiceClient client = new RecouvrementServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Recouvrement"));
                    client.returnAvisReedtionCoupureAsync(CampagneSelect, IsListe);
                    client.returnAvisReedtionCoupureCompleted += (ss, ee) =>
                        {
                            try
                            {
                                if (ee.Cancelled || ee.Error != null)
                                {
                                    string error = ee.Error.Message;
                                    Message.ShowError("error occurs while invoking remote procedure", "returnAvisReedtionCoupure");
                                    prgBar.Visibility = System.Windows.Visibility.Collapsed ;
                                    LayoutRoot.Cursor = Cursors.Arrow ;
                                    return;
                                }

                                if (ee.Result == null || ee.Result.Count == 0)
                                {
                                    Message.ShowInformation("no data found", Galatee.Silverlight.Resources.Langue.wcf_error);
                                    prgBar.Visibility = System.Windows.Visibility.Collapsed;
                                    LayoutRoot.Cursor = Cursors.Arrow;
                                    return;
                                }

                                List<ServiceRecouvrement.CsAvisDeCoupureClient> dataTable = new List<ServiceRecouvrement.CsAvisDeCoupureClient>();
                                dataTable = ee.Result;
                                ligne.AddRange(dataTable);


                                //impression du recu de la liste of cut-off

                                Dictionary<string, string> param = new Dictionary<string, string>();
                                param.Add("pfirstzone", CampagneSelect.PREMIERE_TOURNEE );
                                param.Add("plastzone", CampagneSelect.DERNIERE_TOURNEE );
                                param.Add("pminiAmount", CampagneSelect.MONTANT_RELANCABLE  .ToString());
                                param.Add("pduedate", CampagneSelect.DATE_EXIGIBILITE.ToString() );
                                param.Add("pnombreCustomer", CampagneSelect.NOMBRE_CLIENT );
                                param.Add("pcampainNumber", CampagneSelect.IDCOUPURE );
                                param.Add("pperioddue", CampagneSelect.PERIODE_RELANCABLE );
                                param.Add("pnumberbill", CampagneSelect.NOMBRE_CLIENT  );
                                param.Add("pfirstroute", string.Empty );
                                param.Add("plastroute", string.Empty );
                                param.Add("pfirstcategorie", CampagneSelect.DEBUT_CATEGORIE );
                                param.Add("plastcategorie", CampagneSelect.FIN_CATEGORIE );
                                param.Add("pnamecontroller", CampagneSelect.AGENTPIA );
                                string key = Utility.getKey();

                                if (IsListe == false)
                                    Utility.ActionCaisse<ServicePrintings.CsAvisDeCoupureClient, ServiceRecouvrement.CsAvisDeCoupureClient>(ligne, key, new Dictionary<string, string>(), "AvisDeCoupureDetail", "Recouvrement");
                                else
                                    Utility.ActionDirectOrientation<ServicePrintings.CsAvisDeCoupureClient, ServiceRecouvrement.CsAvisDeCoupureClient>(ligne, param, SessionObject.DefaultPrinter, "AvisDeCoupureListe", "Recouvrement", true);

                                prgBar.Visibility = System.Windows.Visibility.Collapsed;
                                LayoutRoot.Cursor = Cursors.Arrow;

                                //string CheminImpression = "55" + SessionObject.LePosteCourant.NOMPOSTE + "5" + "Impression";
                                //if (IsListe == false)
                                //    Utility.ActionDirectOrientation<ServicePrintings.aDisconnection, ServiceRecouvrement.aDisconnection>(ligne, new Dictionary<string, string>(), CheminImpression, "AvisDeCoupureDetail", "Recouvrement", false);
                                //else
                                //    Utility.ActionDirectOrientation<ServicePrintings.aDisconnection, ServiceRecouvrement.aDisconnection>(ligne, param, CheminImpression, "AvisDeCoupureListe", "Recouvrement", true);

                            }
                            catch (Exception ex)
                            {
                                Message.ShowError(ex, Galatee.Silverlight.Resources.Langue.errorTitle);
                            }

                        };
                }
                else
                {
                    //ClosedEnventHandler();
                    ClosedEnventHandler();
                    this.DialogResult = false;
                }
                   
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, Galatee.Silverlight.Resources.Langue.errorTitle);
                prgBar.Visibility = System.Windows.Visibility.Collapsed;
                LayoutRoot.Cursor = Cursors.Arrow;
            }
            //this.DialogResult = true;
        }

        private void btnReinitialiser_Click(object sender, EventArgs e)
        {
            try
            {
                this.lvwResultat.ItemsSource = null;
                //this.cmbCentre.SelectedItem = lCentre.First();
                this.txtCampagne.Text = string.Empty;
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
         void Recherche()
         {
             try
             {
                 aParam critere = new aParam();
                 critere.IdCoupure = (string.IsNullOrEmpty(this.txtCampagne.Text)) ? null : this.txtCampagne.Text;
                 critere.Agent = (string.IsNullOrEmpty(this.txtAgent.Text)) ? null : this.txtAgent.Text;
                 List<CsCAMPAGNE> _lstDesCampagne = lesCampagne.Where(t => (t.CENTRE == critere.Centre || string.IsNullOrEmpty(critere.Centre)) &&
                                                                          (t.IDCOUPURE == critere.IdCoupure || string.IsNullOrEmpty(critere.IdCoupure)) &&
                                                                          (t.MATRICULEPIA == critere.Agent || string.IsNullOrEmpty(critere.Agent))).ToList();

                 if (_lstDesCampagne != null && _lstDesCampagne.Count != 0)
                 {
                     this.lvwResultat.ItemsSource = null;
                     this.lvwResultat.ItemsSource = _lstDesCampagne;

                     OKButton.Visibility = System.Windows.Visibility.Visible;
                 }
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
                 Recherche();
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
                 OKButton.IsEnabled = true;
                 if (_returnvalue)
                     btnOk.IsEnabled = true;
                 CampagneSelect = lvwResultat.SelectedItem as CsCAMPAGNE;
                 if (CampagneSelect != null )
                 {
                     this.txtCampagne.Text = CampagneSelect.IDCOUPURE;
                     this.dtpDate.Text = CampagneSelect.DATECREATION.ToShortDateString();

                 }
                 //Campaign = (lvwResultat.SelectedItem as CsCAMPAGNE).IdCoupure;
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
                     //_campagnes = this.RecupererLesElementsSelectionnes();
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

         private void btn_Regroupement_Click(object sender, RoutedEventArgs e)
         {

         }
    }
}

