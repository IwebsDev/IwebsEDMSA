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

namespace Galatee.Silverlight.Precontentieux
{
    public partial class FrmSaisieRendezVous : ChildWindow
    {
        List<CsCentre> lstCentreCampagne = new List<CsCentre>();
        List<CsCAMPAGNE> lesCampagne = new List<CsCAMPAGNE>();
        ObservableCollection<CsDetailCampagnePrecontentieux> lesClientCampagne = new ObservableCollection<CsDetailCampagnePrecontentieux>();
        List<CsSite> lstSiteCampagne = new List<CsSite>();
        List<CsSite> lstAgentCampagne = new List<CsSite>();
        public FrmSaisieRendezVous()
        {
            try
            {
                InitializeComponent();
                InitialiserControle();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, Galatee.Silverlight.Resources.Langue.errorTitle);
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
                                    group new { p } by new { p.MATRICULEPIA, p.AGENTPIA, p.FK_IDMATRICULE } into pResult
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
                //Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new AccesServiceWCF().GetAcceuilClient();
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
                client.RetourneCampagnePrecontentieuxCompleted += (es, result) =>
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
                        cmbCampagne.ItemsSource = null;
                        cmbCampagne.ItemsSource = result.Result;
                        cmbCampagne.DisplayMemberPath  = "IDCOUPURE";
                    }
                    catch (Exception ex)
                    {
                        Message.ShowError(ex, Galatee.Silverlight.Resources.Langue.errorTitle);
                    }
                };
                client.RetourneCampagnePrecontentieuxAsync(lstIdCentre);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public event EventHandler Closed;

        public CsCAMPAGNE CampagneSelect;
        public CsClient  ClientRechercheSelect;
        public CsDetailCampagnePrecontentieux  ClientSelect;

     
        private void InsererCompteClient(Galatee.Silverlight.ServiceAccueil.CsLclient leFrais)
        {
            try
            {
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient clients = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
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
                        Message.ShowInformation(Galatee.Silverlight.Resources.Accueil.Langue.MsgOperationTerminee, Galatee.Silverlight.Resources.Accueil.Langue.lbl_Menu);

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
   
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (lvwResultat.ItemsSource != null)
                {
                    List<CsDetailCampagnePrecontentieux> lstClientImprimer = ((List<CsDetailCampagnePrecontentieux>)this.lvwResultat.ItemsSource).Where(t => t.DATERDV != null).ToList();
                    if (lstClientImprimer != null && lstClientImprimer.Count != 0)
                    {
                        MiseAJoursDetailCampagne(lstClientImprimer);
                    }
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
        private void dgMyDataGrid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            DataGrid dg = (sender as DataGrid);
            var allObjects = dg.ItemsSource as ObservableCollection<CsDetailCampagnePrecontentieux>;
            if (dg.SelectedItem != null)
            {
                CsDetailCampagnePrecontentieux SelectedObject = (CsDetailCampagnePrecontentieux)dg.SelectedItem;
                if (SelectedObject.IsSelect == false)
                    SelectedObject.IsSelect = true;
                else
                    SelectedObject.IsSelect  = false;
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
          
        }

        private void galatee_OkClickedClient(object sender, EventArgs e)
        {
          
        }
        private void cmbCampagne_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CampagneSelect = (CsCAMPAGNE)this.cmbCampagne.SelectedItem;
            this.txtCampagne.Text = CampagneSelect.IDCOUPURE;
            this.txtCampagne.Tag = CampagneSelect.PK_ID;
        }

        private void btnReinitialiser_Click(object sender, RoutedEventArgs e)
        {
            this.lvwResultat.ItemsSource = null;
            this.cmbCampagne.SelectedItem = null;
        }

        private void btnsearch_Click(object sender, RoutedEventArgs e)
        {
            if (this.txtCampagne.Tag != null )
            {
                int idcampagne = (int)this.txtCampagne.Tag;
                RechercheDetailCampagne(idcampagne);
            }
        }
        private void RechercheDetailCampagne(int IdCampagne)
        {
            try
            {
                RecouvrementServiceClient client = new RecouvrementServiceClient(Utility.ProtocoleIndex(), Utility.EndPoint("Recouvrement"));
                client.RetourneDetailPrecontentieuxCompleted += (ss, ress) =>
                {
                    try
                    {
                        if (ress.Cancelled || ress.Error != null)
                        {
                            Message.ShowError("Erreur survenue lors de l'appel service", "Informations");
                            return;
                        }
                        if (ress.Result != null && ress.Result.Count != 0)
                        {
                            List<CsDetailCampagnePrecontentieux> lstDetail = ress.Result.Where(t=>t.ISINVITATIONEDITER).ToList();
                            this.lvwResultat.ItemsSource = null;
                            this.lvwResultat.ItemsSource = lstDetail;
                        }
                    }
                    catch (Exception ex)
                    {
                        Message.ShowError(ex, "Erreur");
                    }
                };
                client.RetourneDetailPrecontentieuxAsync(IdCampagne);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void MiseAJoursDetailCampagne(List<CsDetailCampagnePrecontentieux> Detail)
        {
            try
            {
                RecouvrementServiceClient client = new RecouvrementServiceClient(Utility.ProtocoleIndex(), Utility.EndPoint("Recouvrement"));
                client.MajDetailPrecontentieuxCompleted += (ss, ress) =>
                {
                    try
                    {
                        if (ress.Cancelled || ress.Error != null)
                        {
                            Message.ShowError("Erreur survenue lors de l'appel service", "Informations");
                            return;
                        }
                        else
                        {
                            if (ress.Result == true)
                            {
                                Message.ShowInformation("Rendez-vous validé ", "Informations");
                                this.DialogResult = true;
                                return;
                            }
                            else if (ress.Result == false)
                            {
                                Message.ShowInformation("Erreur à la validation du rendez-vous ", "Informations");
                                return;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Message.ShowError(ex, "Erreur");
                    }
                };
                client.MajDetailPrecontentieuxAsync(Detail);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void DtpDePose_SelectedDateChanged_1(object sender, SelectionChangedEventArgs e)
        {
            if (((DatePicker)sender).SelectedDate.Value != null)
            {
                CsDetailCampagnePrecontentieux leDetail = (CsDetailCampagnePrecontentieux)lvwResultat.SelectedItem;
                leDetail.DATERDV = ((DatePicker)sender).SelectedDate.Value;
            }
        }
    }
}

