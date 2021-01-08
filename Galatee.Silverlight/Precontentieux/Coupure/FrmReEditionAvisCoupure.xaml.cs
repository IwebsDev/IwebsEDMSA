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
    public partial class FrmReEditionAvisCoupure : ChildWindow
    {
        List<CsCentre> lstCentreCampagne = new List<CsCentre>();
        List<CsCAMPAGNE> lesCampagne = new List<CsCAMPAGNE>();
        List<CsSite> lstSiteCampagne = new List<CsSite>();
        List<CsSite> lstAgentCampagne = new List<CsSite>();
        public FrmReEditionAvisCoupure()
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
        public FrmReEditionAvisCoupure(bool returnbackvalue)
        {
            try
            {
                InitializeComponent();
                _returnvalue = returnbackvalue;
                btnOk.Visibility = Visibility.Visible;
                OKButton.Visibility = Visibility.Collapsed;
                prgBar.Visibility = System.Windows.Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, Galatee.Silverlight.Resources.Langue.errorTitle);
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
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
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
                RecouvrementServiceClient client = new RecouvrementServiceClient(Utility.Protocole(), Utility.EndPoint("Recouvrement"));
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
                                    Utility.ActionDirectOrientation<ServicePrintings.CsAvisDeCoupureClient, ServiceRecouvrement.CsAvisDeCoupureClient>(ligne, null, SessionObject.CheminImpression, "AvisDeCoupureDetail", "Recouvrement", true);
                                else
                                    Utility.ActionDirectOrientation<ServicePrintings.CsAvisDeCoupureClient, ServiceRecouvrement.CsAvisDeCoupureClient>(ligne, param, SessionObject.CheminImpression, "AvisDeCoupureListe", "Recouvrement", true);

                                prgBar.Visibility = System.Windows.Visibility.Collapsed;
                                LayoutRoot.Cursor = Cursors.Arrow;
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

 
       private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Galatee.Silverlight.Shared.ClasseMEthodeGenerique.FermetureEcran(this);
        }
         void Recherche()
         {
             try
             {
                 aParam critere = new aParam();
                 critere.Centre = (string.IsNullOrEmpty(this.txtCentre.Text)) ? null : this.txtCentre.Text;
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

                     this.txtSite.Text = CampagneSelect.CODESITE;
                     if (lstSiteCampagne.FirstOrDefault(t => t.CODE == CampagneSelect.CODESITE) != null)
                         this.cmbSite.SelectedItem = lstSiteCampagne.FirstOrDefault(t => t.CODE == CampagneSelect.CODESITE);

                     this.txtCentre.Text = CampagneSelect.CENTRE;
                     if (lstCentreCampagne.FirstOrDefault(t => t.CODE  == CampagneSelect.CENTRE ) != null)
                         this.cmbCentre.SelectedItem = lstCentreCampagne.FirstOrDefault(t => t.CODE  == CampagneSelect.CENTRE );

                     this.txtAgent.Text = CampagneSelect.MATRICULEPIA ;
                     if (lstSiteCampagne.FirstOrDefault(t => t.CODE == CampagneSelect.CODESITE) != null)
                         this.cmbSite.SelectedItem = lstSiteCampagne.FirstOrDefault(t => t.CODE == CampagneSelect.CODESITE);


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
                 }
             }
             catch (Exception ex)
             {
                 Message.ShowError(ex, Galatee.Silverlight.Resources.Langue.errorTitle);
             }
         }

         private void btnReinitialiser_Click(object sender, RoutedEventArgs e)
         {
             this.cmbAgent.SelectedItem = null;
             this.lvwResultat.ItemsSource = null;
             this.txtAgent.Text = string.Empty;
             this.txtCampagne .Text = string.Empty;
         }
    }
}

