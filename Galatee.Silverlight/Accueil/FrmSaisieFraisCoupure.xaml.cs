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
using System.ComponentModel;
using System.Collections.ObjectModel;
using Galatee.Silverlight.ServiceAccueil;
using Galatee.Silverlight.Resources.Accueil;

namespace Galatee.Silverlight.Accueil
{
    public partial class FrmSaisieFraisCoupure : ChildWindow
    {
        public FrmSaisieFraisCoupure()
        {
            InitializeComponent();
            ChargerDonneeDuSite();
            ChargeTypeCoupure();
            
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dtailCampagneSelect != null && !string.IsNullOrEmpty(dtailCampagneSelect.CENTRE) && typeCoupure != null && typeCoupure.COUT != 0)
                {
                    CsLclient leFrais = GetElementDeFrais(dtailCampagneSelect, typeCoupure.COUT);
                    if (leFrais != null)
                        InsererCompteClient(leFrais);
                    
                }
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
        }
        List<CsSite> lstSite = new List<CsSite>();
        List<CsCentre> LstCentre = new List<CsCentre>();
        List<CsCentre> lstCentreCampagne = new List<CsCentre>();
        List<CsCAMPAGNE> lesCampagne = new List<CsCAMPAGNE>();
        List<CsTypeCoupure> lesTypeCoupure = new List<CsTypeCoupure>();
        List<CsSite> lstSiteCampagne = new List<CsSite>();
        List<CsSite> lstAgentCampagne = new List<CsSite>();
        private void ChargerDonneeDuSite()
        {
            try
            {
                AcceuilServiceClient service = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                service.ListeDesDonneesDesSiteCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    SessionObject.LstCentre = args.Result;
                    this.btn_Centre.IsEnabled = true;
                    List<ServiceAccueil.CsCentre> lesCentre = Shared.ClasseMEthodeGenerique.RetourCentreByPerimetre(SessionObject.LstCentre, UserConnecte.listeProfilUser);
                    lstSite = Galatee.Silverlight.Shared.ClasseMEthodeGenerique.RetourneSiteByCentre(lesCentre);
                    if (lstSite != null)
                    {
                        List<CsSite> _LstSite = lstSite.Where(p => p.CODE != SessionObject.Enumere.Generale).ToList();
                    }
                    if (LstCentre != null)
                    {
                        List<CsCentre> _LstCentre = LstCentre.Where(p => p.CODE != SessionObject.Enumere.Generale).ToList();
                        if (_LstCentre.Count == 1 )
                        {
                            this.Txt_CodeCentre.Text = _LstCentre[0].CODE;
                            this.Txt_LibelleCentre.Text = _LstCentre[0].LIBELLE;
                        }
                       
                    }
                };
                service.ListeDesDonneesDesSiteAsync(false );
                service.CloseAsync();
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
                                      group new { p } by new { p.CENTRE, p.FK_IDCENTRE, p.LIBELLECENTRE, p.FK_IDSITE, p.CODESITE, p.LIBELLESITE } into pResult
                                      select new
                                      {
                                          pResult.Key.FK_IDCENTRE,
                                          pResult.Key.CENTRE,
                                          pResult.Key.LIBELLECENTRE,
                                          pResult.Key.FK_IDSITE,
                                          pResult.Key.CODESITE,
                                          pResult.Key.LIBELLESITE,
                                      });
                foreach (var item in ListCentreTemp)
                {
                    CsCentre leCentre = new CsCentre()
                    {
                        CODESITE = item.CODESITE,
                        FK_IDCODESITE = item.FK_IDSITE,
                        LIBELLESITE = item.LIBELLESITE,
                        PK_ID = item.FK_IDCENTRE,
                        CODE = item.CENTRE,
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
                    CsSite leSite = new CsSite()
                    {
                        CODE = item.CODESITE,
                        PK_ID = item.FK_IDSITE,
                        LIBELLE = item.LIBELLESITE
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
                                    group new { p } by new { p.MATRICULEPIA, p.AGENTPIA } into pResult
                                    select new
                                    {
                                        pResult.Key.MATRICULEPIA,
                                        pResult.Key.AGENTPIA
                                    });
                foreach (var item in ListSiteTemp)
                {
                    CsSite leSite = new CsSite()
                    {
                        CODE = item.MATRICULEPIA,
                        LIBELLE = item.AGENTPIA
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

        private void ChargeTypeCoupure()
        {
            try
            {
                AcceuilServiceClient client = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
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
                        
                        lesTypeCoupure = result.Result;
                        lesTypeCoupure.ForEach(t => t.LIBELLE = t.LIBELLE + " " + t.COUT);
                        cbo_TypeCoupure.ItemsSource = null;
                        cbo_TypeCoupure.ItemsSource = lesTypeCoupure;
                        cbo_TypeCoupure.DisplayMemberPath = "LIBELLE";
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
        CsDetailCampagne dtailCampagneSelect = new CsDetailCampagne();
        CsTypeCoupure typeCoupure = new CsTypeCoupure();
        private void ChargeDetailCampagne(string IdCampagen, string centre, string client, string ordre)
        {
            try
            {
                AcceuilServiceClient clients = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                clients.RetourneFactureCampagneAsync(IdCampagen, centre, client, ordre);
                clients.RetourneFactureCampagneCompleted += (es, result) =>
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
                            Message.ShowInformation("Donnée non trouvé ", "SelectCentreCampagne");
                            return;
                        }
                        List<CsDetailCampagne>  leDetailCampagne = result.Result;
                        lvwResultat.ItemsSource = null;
                        lvwResultat.ItemsSource = leDetailCampagne;
                        lvwResultat.SelectedItem = leDetailCampagne[0];
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

        private void InsererCompteClient(CsLclient leFrais)
        {
            try
            {
                AcceuilServiceClient clients = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                clients.InsererFraisPoseAsync(leFrais);
                clients.InsererFraisPoseCompleted += (es, result) =>
                {
                    try
                    {
                        if (result.Cancelled || result.Error != null)
                        {
                            string error = result.Error.Message;
                            Message.ShowError("Erreur à l'exécution du service", "InsererCompteClient");
                            return;
                        }
                        if (result.Result == null)
                        {
                            Message.ShowInformation("Donnée non trouvé ", "InsererCompteClient");
                            return;
                        }
                        Message.ShowInformation(Langue.MsgOperationTerminee , Langue.lbl_Menu );
                      
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
        void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        void btnOk_Click(object sender, RoutedEventArgs e)
         {
             try
             {
             }
             catch (Exception ex)
             {
             } 
         }

        private void btn_Centre_Click(object sender, RoutedEventArgs e)
        {
            try
            {
               if (LstCentre.Count > 0)
                {
                    this.btn_Centre.IsEnabled = false;
                    List<object> _Listgen = Galatee.Silverlight.Shared.ClasseMEthodeGenerique.RetourneListeObjet(LstCentre);
                    Galatee.Silverlight.MainView.UcListeGenerique ctr = new Galatee.Silverlight.MainView.UcListeGenerique(_Listgen, "CODE", "LIBELLE", Langue.lbl_ListeCentre);
                    ctr.Closed += new EventHandler(galatee_OkClickedCentre);
                    ctr.Show();
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Langue.lbl_Menu);
            }
        }
        void galatee_OkClickedCentre(object sender, EventArgs e)
        {
            Galatee.Silverlight.MainView.UcListeGenerique ctrs = sender as Galatee.Silverlight.MainView.UcListeGenerique;
            if (ctrs.isOkClick)
            {
                this.btn_Centre.IsEnabled = true;
                CsCentre leCentre = (CsCentre)ctrs.MyObject;
                this.Txt_CodeCentre.Text = leCentre.CODE;
                this.Txt_LibelleCentre.Text = leCentre.LIBELLE;
            }
            else
                this.btn_Centre.IsEnabled = true;


        }

        private void btnRechercheCampagne_Click_1(object sender, RoutedEventArgs e)
        {

            try
            {
                if (lesCampagne.Count > 0)
                {
                    this.btnRechercheCampagne .IsEnabled = false;
                    Dictionary<string, string> _LstColonneAffich = new Dictionary<string, string>();
                    _LstColonneAffich.Add("IDCOUPURE", "NUMERO DE CAMPAGNE");
                    List<object> obj = Shared.ClasseMEthodeGenerique.RetourneListeObjet(lesCampagne);
                    MainView.UcListeGenerique ctrl = new MainView.UcListeGenerique(obj, _LstColonneAffich, false, "Campagne");
                    ctrl.Closed += new EventHandler(galatee_OkClickedCampagne);
                    ctrl.Show();


                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Langue.lbl_Menu);
            }
        }
        void galatee_OkClickedCampagne(object sender, EventArgs e)
        {
            Galatee.Silverlight.MainView.UcListeGenerique ctrs = sender as Galatee.Silverlight.MainView.UcListeGenerique;
            if (ctrs.isOkClick)
            {
                this.btnRechercheCampagne.IsEnabled = true;
                CsCAMPAGNE leCampagne = (CsCAMPAGNE)ctrs.MyObject;
                this.txtCampagne.Text = leCampagne.IDCOUPURE ;
            }
            else
                this.btnRechercheCampagne.IsEnabled = true;


        }

        private void button1_Copy_Click(object sender, RoutedEventArgs e)
        {
            string idCampagne = string.IsNullOrEmpty(this.txtCampagne.Text) ? string.Empty : this.txtCampagne.Text ;
            string Centre = string.IsNullOrEmpty(this.Txt_CodeCentre .Text) ? string.Empty : this.Txt_CodeCentre.Text ;
            string Client = string.IsNullOrEmpty(this.txt_Client .Text) ? string.Empty : this.txt_Client.Text ;
            string Ordre = string.IsNullOrEmpty(this.txt_Ordre .Text) ? string.Empty : this.txt_Ordre.Text ;
            ChargeDetailCampagne(idCampagne, Centre, Client, Ordre);
        }

        private CsLclient  GetElementDeFrais(CsDetailCampagne Campagne, decimal? montantFrais)
        {
            CsLclient Frais = new CsLclient();
            try
            {
                Frais.CENTRE = Campagne.CENTRE;
                Frais.CLIENT = Campagne.CLIENT;
                Frais.ORDRE = Campagne.ORDRE;
                Frais.REFEM = DateTime.Today.Date.Year.ToString() + DateTime.Today.Date.Month.ToString();
                Frais.IDCOUPURE = Campagne.IDCOUPURE;
                Frais.COPER = SessionObject.Enumere.CoperFRP;
                Frais.DENR = DateTime.Today.Date;
                Frais.EXIGIBILITE = DateTime.Today.Date;
                Frais.DATECREATION = DateTime.Today.Date;
                Frais.DATEMODIFICATION = DateTime.Today.Date;
                Frais.DC = SessionObject.Enumere.Debit;
                Frais.FK_IDCENTRE =Campagne.FK_IDCENTRE;
                Frais.FK_IDCLIENT = Campagne.FK_IDCLIENT;
                Frais.MATRICULE = UserConnecte.matricule;
                Frais.MOISCOMPT = DateTime.Today.Date.Year.ToString() + DateTime.Today.Date.Month.ToString();
                Frais.MONTANT = montantFrais;
                Frais.NATURE = SessionObject.Enumere.NatureFRP;
                Frais.TOP1 = "0";
                

                return Frais;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        private void lvwResultat_SizeChanged(object sender, SizeChangedEventArgs e)
        {
        }

        private void cbo_TypeCoupure_SizeChanged(object sender, SizeChangedEventArgs e)
        {
        }

        private void lvwResultat_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            dtailCampagneSelect = (CsDetailCampagne)lvwResultat.SelectedItem;

        }

        private void cbo_TypeCoupure_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            typeCoupure = (CsTypeCoupure)cbo_TypeCoupure.SelectedItem;

        }

    }
}


