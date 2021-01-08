using Galatee.Silverlight.Rpnt.Common;
//using Galatee.Silverlight.ServiceRpnt;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Galatee.Silverlight.Recouvrement
{
    public partial class CreationLotBTA : ChildWindow
    {
        #region Variables
            //private ViewsModels.CsTBCAMPAGNECONTROLEBTA C_BTA;
        private List<Galatee.Silverlight.ServiceRecouvrement.CsCampagnesBTAAccessiblesParLUO> ListeCampagne_;

        Galatee.Silverlight.Rpnt.ViewsModels.CsTBCAMPAGNECONTROLEBTA C_BTA = new Galatee.Silverlight.Rpnt.ViewsModels.CsTBCAMPAGNECONTROLEBTA();

            ObservableCollection<Galatee.Silverlight.ServiceAccueil.CsCentre> ListExploitation = new ObservableCollection<Galatee.Silverlight.ServiceAccueil.CsCentre>();
            List<Galatee.Silverlight.ServiceRecouvrement.CsCampagnesBTAAccessiblesParLUO> listeCampagne = new List<Galatee.Silverlight.ServiceRecouvrement.CsCampagnesBTAAccessiblesParLUO>();
            //CsREFMETHODEDEDETECTIONCLIENTSBTA Mth_Detection = new CsREFMETHODEDEDETECTIONCLIENTSBTA();

        #endregion

        public CreationLotBTA()
        {
            InitializeComponent();
            LoadExploitation();
            LoadCampagne();
        }

        public CreationLotBTA(List<Galatee.Silverlight.ServiceRecouvrement.CsCampagnesBTAAccessiblesParLUO> ListeCampagne_)
        {
            InitializeComponent();
            LoadCampagne();
        }

        //private void UpdateInfocampagnes(CsREFMETHODEDEDETECTIONCLIENTSBTA Mth_Detection_, string PeriodeDepart_)
        //{
        //    //Association de campagne à liste de branchement selection
        //    RpntServiceClient service = new RpntServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Rpnt"));
        //    int handler = LoadingManager.BeginLoading("Traitement des données ...");
        //    List<CsTBCAMPAGNECONTROLEBTA> listecampageneTosave = new List<CsTBCAMPAGNECONTROLEBTA>();
        //    foreach (var item in (ObservableCollection<ViewsModels.CsTBCAMPAGNECONTROLEBTA>)dgCampagne.ItemsSource)
        //    {
        //        CsTBCAMPAGNECONTROLEBTA camp = new CsTBCAMPAGNECONTROLEBTA
        //        {
        //            CAMPAGNE_ID = item.CAMPAGNE_ID,
        //            CODECENTRE = int.Parse(item.CODECENTRE),
        //            LIBELLECENTRE = item.LIBELLECENTRE,
        //            DATECREATION = item.DATECREATION,
        //            DATEDEBUTCONTROLES = item.DATEDEBUTCONTROLES,
        //            DATEFINPREVUE = item.DATEFINPREVUE,
        //            DATEMODIFICATION = item.DATEMODIFICATION,
        //            LIBELLE_CAMPAGNE = item.LIBELLE_CAMPAGNE,
        //            LIBELLEEXPLOITATION = item.LIBELLEEXPLOITATION,
        //            MATRICULEAGENTCREATION = item.MATRICULEAGENTCREATION,
        //            MATRICULEAGENTDERNIEREMODIFICATION = item.MATRICULEAGENTDERNIEREMODIFICATION,
        //            NBREELEMENTS = item.NBREELEMENTS,
        //            //STATUT = item.STATUT_ID,
        //            STATUT_ID = item.STATUT_ID,
        //            NBRLOTS = item.NBRLOTS,
        //            POULATIONNONAFFECTES = item.POULATIONNONAFFECTES,
        //            //LISTEBRANCHEMENT = item.LISTEBRANCHEMENT.ToList()
        //        };
        //        camp.LISTEBRANCHEMENT = new List<CsBrt>();
        //        if (item.LISTEBRANCHEMENT != null)
        //        {
        //            foreach (var _item in item.LISTEBRANCHEMENT)
        //            {
        //                camp.LISTEBRANCHEMENT.Add(_item);
        //            }
        //        }
        //        camp.LISTELOT = new List<CsTBLOTDECONTROLEBTA>();
        //        if (item.LISTELOT != null)
        //        {
        //            foreach (var _item in item.LISTELOT)
        //            {
        //                camp.LISTELOT.Add(_item);
        //            }
        //        }
        //        listecampageneTosave.Add(camp);
        //    }

        //    service.SaveCampagneElementAsync(listecampageneTosave, SessionObject.Mth_Detection, PeriodeDepart_);
        //    service.SaveCampagneElementCompleted += (er, res) =>
        //    {
        //        try
        //        {
        //            if (res.Error != null || res.Cancelled)
        //                Message.Show("Erreur dans le traitement des méthode de dectection : " + res.Error.InnerException.ToString(), "Erreur");
        //            else
        //                if (res.Result != null)
        //                {
        //                    Message.Show("Enrégistrer avec succès",
        //                        "Erreur");
        //                }
        //                else
        //                    Message.Show("Une erreur s'est produite, veuillez consultez le journal des erreurs",
        //                        "Erreur");

        //            LoadingManager.EndLoading(handler);
        //        }
        //        catch (Exception)
        //        {
        //            throw;
        //        }
        //    };
        //}

        public void LoadExploitation()
        {
            //if (SessionObject.centre.Count() > 0)
            //{
            //    ListExploitation.Clear();
            //    foreach (var item in SessionObject.centre)
            //    {
            //        ListExploitation.Add(item);
            //    }
            //    LoadCampagne();
            //}
            //else
            //{
            //    RpntServiceClient service = new RpntServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Rpnt"));
            //    int handler = LoadingManager.BeginLoading("Recuperation des centres ...");
            //    service.GetExploitationByUOAsync(new CsREFUNITEORGANISATIONNELLE());
            //    service.GetExploitationByUOCompleted += (er, res) =>
            //    {
            //        try
            //        {
            //            if (res.Error != null || res.Cancelled)
            //                Message.Show("Erreur dans le traitement des méthode de dectection : " + res.Error.InnerException.ToString(), "Erreur");
            //            else
            //                if (res.Result != null)
            //                {
            //                    SessionObject.centre = res.Result;
            //                    ListExploitation.Clear();
            //                    foreach (var item in SessionObject.centre)
            //                    {
            //                        ListExploitation.Add(item);
            //                    }
            //                    LoadCampagne();
            //                }
            //                else
            //                    Message.Show("Une erreur s'est produite, veuillez consultez le journal des erreurs",
            //                        "Erreur");

            //            LoadingManager.EndLoading(handler);
            //        }
            //        catch (Exception)
            //        {

            //            throw;
            //        }
            //    };
            //}

        }
        public void LoadCampagne()
        {
            //if (SessionObject.campagne.Count() > 0)
            //{
            //    Load_Campagne();
            //}
            //else
            //{
            Galatee.Silverlight.ServiceRecouvrement.RecouvrementServiceClient service = new Galatee.Silverlight.ServiceRecouvrement.RecouvrementServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Recouvrement"));
            int handler = LoadingManager.BeginLoading("Recuperation des factures ...");
            service.GetCampagneBTAControleAsync();
            service.GetCampagneBTAControleCompleted += (er, res) =>
            {
                try
                {
                    if (res.Error != null || res.Cancelled)
                        Message.Show("Erreur dans le traitement des méthode de dectection : " + res.Error.InnerException.ToString(), "Erreur");
                    else
                        if (res.Result != null)
                        {
                            this.ListeCampagne_ = res.Result;
                            dgCampagne.ItemsSource = this.ListeCampagne_;
                        }
                        else
                            Message.Show("Une erreur s'est produite, veuillez consultez le journal des erreurs",
                                "Erreur");

                    LoadingManager.EndLoading(handler);
                }
                catch (Exception)
                {

                    throw;
                }
            };
            //}

        }


        //private void CallServiceToLoadCamp()
        //{
        //    RpntServiceClient service = new RpntServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Rpnt"));
        //    int handler = LoadingManager.BeginLoading("Recuperation des données ...");
        //    service.GetCampagneBTAControleAsync();
        //    service.GetCampagneBTAControleCompleted += (er, res) =>
        //    {
        //        try
        //        {
        //            if (res.Error != null || res.Cancelled)
        //                Message.Show("Erreur dans le traitement des méthode de dectection : " + res.Error.InnerException.ToString(), "Erreur");
        //            else
        //                if (res.Result != null)
        //                {
        //                    SessionObject.campagne = res.Result;
        //                    Load_Campagne();
        //                }
        //                else
        //                    Message.Show("Une erreur s'est produite, veuillez consultez le journal des erreurs",
        //                        "Erreur");

        //            LoadingManager.EndLoading(handler);
        //        }
        //        catch (Exception)
        //        {

        //            throw;
        //        }
        //    };
        //}

        private void Load_Campagne()
        {
            //listeCampagne.Clear();
            //foreach (var item in SessionObject.campagne)
            //{
            //    C_BTA = new ViewsModels.CsTBCAMPAGNECONTROLEBTA();
            //    string code_centre = item.CODECENTRE != null ? item.CODECENTRE.ToString() : "";
            //    C_BTA.CAMPAGNE_ID = item.CAMPAGNE_ID != null ? item.CAMPAGNE_ID : Guid.NewGuid();
            //    C_BTA.CODECENTRE = code_centre;
            //    if (code_centre != "")
            //    {
            //        C_BTA.LIBELLECENTRE = ListExploitation.First(c => Convert.ToInt16(c.CODECENTRE) == Convert.ToInt16(code_centre)).LIBELLE != null ? ListExploitation.First(c => Convert.ToInt16(c.CODECENTRE) == Convert.ToInt16(code_centre)).LIBELLE : "";
            //    }
            //    else
            //    {
            //        C_BTA.LIBELLECENTRE = "";
            //    }
            //    C_BTA.METHODE = new CsREFMETHODEDEDETECTIONCLIENTSBTA();
            //    C_BTA.METHODE.METHODE_ID = item.METHODE != null ? (item.METHODE.METHODE_ID != null ? item.METHODE.METHODE_ID : int.MinValue) : int.MinValue;
            //    SessionObject.Mth_Detection = C_BTA.METHODE;
            //    C_BTA.DATECREATION = item.DATECREATION != null ? item.DATECREATION : DateTime.MinValue;
            //    C_BTA.DATEDEBUTCONTROLES = item.DATEDEBUTCONTROLES != null ? item.DATEDEBUTCONTROLES : DateTime.MinValue;
            //    C_BTA.DATEFINPREVUE = item.DATEFINPREVUE != null ? item.DATEFINPREVUE : DateTime.MinValue;
            //    C_BTA.DATEMODIFICATION = item.DATEMODIFICATION != null ? item.DATEMODIFICATION : DateTime.MinValue;
            //    C_BTA.LIBELLE_CAMPAGNE = item.LIBELLE_CAMPAGNE != null ? item.LIBELLE_CAMPAGNE : "";
            //    C_BTA.LIBELLEEXPLOITATION = item.LIBELLEEXPLOITATION != null ? item.LIBELLEEXPLOITATION : "";
            //    C_BTA.MATRICULEAGENTCREATION = item.MATRICULEAGENTCREATION != null ? item.MATRICULEAGENTCREATION : "";
            //    C_BTA.MATRICULEAGENTDERNIEREMODIFICATION = item.MATRICULEAGENTDERNIEREMODIFICATION != null ? item.MATRICULEAGENTDERNIEREMODIFICATION : "";
            //    C_BTA.NBREELEMENTS = item.NBREELEMENTS != null ? item.NBREELEMENTS : 0;
            //    C_BTA.STATUT = item.STATUT_ID == 1 ? Enumere.LibelleActif : Enumere.LibelleInactif;
            //    C_BTA.STATUT_ID = item.STATUT_ID != null ? item.STATUT_ID : 0;
            //    C_BTA.NBRLOTS = item.NBRLOTS != null ? item.NBRLOTS : 0;
            //    C_BTA.POULATIONNONAFFECTES = item.POULATIONNONAFFECTES != null ? item.POULATIONNONAFFECTES : 0;
            //    C_BTA.LISTEBRANCHEMENT = new ObservableCollection<CsBrt>();
            //    C_BTA.LISTELOT = new ObservableCollection<CsTBLOTDECONTROLEBTA>();
            //    if (item.LISTEBRANCHEMENT != null)
            //    {
            //        foreach (var item_ in item.LISTEBRANCHEMENT)
            //        {
            //            C_BTA.LISTEBRANCHEMENT.Add(item_);
            //        }
            //    }
            //    if (item.LISTELOT != null)
            //    {
            //        foreach (var item_ in item.LISTELOT)
            //        {
            //            C_BTA.LISTELOT.Add(item_);
            //        }
            //    }
            //    C_BTA.METHODE = item.METHODE;
            //    listeCampagne.Add(C_BTA);
            //    //txtnbrbranch.Text = ListeBrtCampagne.Count().ToString();
            //}
        }

        //private void SaveCampagne()
        //{
        //    ////Association de campagne à liste de branchement selection
        //    Galatee.Silverlight.ServiceRecouvrement.RecouvrementServiceClient service = new Galatee.Silverlight.ServiceRecouvrement.RecouvrementServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Recouvrement"));
        //    int handler = LoadingManager.BeginLoading("Traitement de données ...");
        //    List<Galatee.Silverlight.ServiceRecouvrement.CsCampagnesBTAAccessiblesParLUO> listecampageneTosave = (List<Galatee.Silverlight.ServiceRecouvrement.CsCampagnesBTAAccessiblesParLUO>)dgCampagne.ItemsSource;

        //    //service.SaveCampagneElementAsync(listecampageneTosave, new CsREFMETHODEDEDETECTIONCLIENTSBTA(), "");
        //    service.SaveCampagneElementAsync(listecampageneTosave);
        //    service.SaveCampagneElementCompleted += (er, res) =>
        //    {
        //        try
        //        {
        //            if (res.Error != null || res.Cancelled)
        //                Message.Show("Erreur dans le traitement des méthode de dectection : " + res.Error.InnerException.ToString(), "Erreur");
        //            else
        //                if (res.Result != null)
        //                {
        //                    Message.Show("Modification enrégistrer avec succès",
        //                        "Erreur");

        //                    //SessionObject.campagne.Clear();
        //                    //SessionObject.campagne = listecampageneTosave;
        //                    //CallServiceToLoadCamp();
        //                }
        //                else
        //                    Message.Show("Une erreur s'est produite, veuillez consultez le journal des erreurs",
        //                        "Erreur");

        //            LoadingManager.EndLoading(handler);
        //        }
        //        catch (Exception)
        //        {
        //            throw;
        //        }
        //    };

        //}

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            //SaveCampagne();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

       

        private void RadioButton_Checked_1(object sender, RoutedEventArgs e)
        {
           
        }

        private void RadioButton_Checked_2(object sender, RoutedEventArgs e)
        {
            
        }

        private void btn_creerlot_Click(object sender, RoutedEventArgs e)
        {
            if (true)
	        {
                var camp_select = (Galatee.Silverlight.ServiceRecouvrement.CsCampagnesBTAAccessiblesParLUO)dgCampagne.SelectedItem;
                Galatee.Silverlight.Recouvrement.CreationLotManuelle frm = new Galatee.Silverlight.Recouvrement.CreationLotManuelle(camp_select);
                 frm.Closing += frm_Closing;
                 frm.Show();
           
	        }
         
        }

        void frm_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (dgCampagne.SelectedItem!=null)
            {
                dg_lot.ItemsSource = ((Galatee.Silverlight.ServiceRecouvrement.CsCampagnesBTAAccessiblesParLUO)dgCampagne.SelectedItem).ListLot;
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //ViewsModels.CsTBCAMPAGNECONTROLEBTA camp_select = (ViewsModels.CsTBCAMPAGNECONTROLEBTA)dgCampagne.SelectedItem;
            //new AffceterAgentControleALot(camp_select).Show();
           
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            //if ((CsTBLOTDECONTROLEBTA)dg_lot.SelectedItem!=null)
            //{
            //    new ConsultationLotControle((CsTBLOTDECONTROLEBTA)dg_lot.SelectedItem,true).Show();
            //}
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //if ((CsTBLOTDECONTROLEBTA)dg_lot.SelectedItem != null)
            //{
            //    new ConsultationLotControle((CsTBLOTDECONTROLEBTA)dg_lot.SelectedItem, false).Show();
            //}
        }

        private void dgCampagne_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgCampagne.SelectedItem != null)
            {
                var SelectedCampagne = (Galatee.Silverlight.ServiceRecouvrement.CsCampagnesBTAAccessiblesParLUO)dgCampagne.SelectedItem;

                var AgentCreateur = SessionObject.ListeDesAgents.FirstOrDefault(a => a.MATRICULE == SelectedCampagne.MatriculeAgentCreation);
                if (AgentCreateur != null)
                    txtagcrea.Text = AgentCreateur.NOM + " " + AgentCreateur.PRENOM + " ( " + AgentCreateur.MATRICULE + " )";

                txtdateCrea.Text = SelectedCampagne.DateCreation.Date.ToString();

                var CentreCampagne = SessionObject.LstCentre.FirstOrDefault(c => c.PK_ID == SelectedCampagne.fk_idCentre);
                if (CentreCampagne != null)
                    txtExploitaion.Text = CentreCampagne.LIBELLE + " ( " + SelectedCampagne.CodeCentre + " )";

                txtlibcamp.Text = SelectedCampagne.Libelle_Campagne;

                txtpopulation.Text = SelectedCampagne.NbreElements.ToString();

                txtstatut.Text = SelectedCampagne.Statut_ID == 0 ? "Inactif" : "Actif";


                dg_lot.ItemsSource = ((Galatee.Silverlight.ServiceRecouvrement.CsCampagnesBTAAccessiblesParLUO)dgCampagne.SelectedItem).ListLot;

            }
        }

        private void btn_Editer_Lot_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dg_lot.SelectedItem != null)
                {
                    Galatee.Silverlight.ServiceRecouvrement.CstbLotsDeControleBTA Lot = (Galatee.Silverlight.ServiceRecouvrement.CstbLotsDeControleBTA)dg_lot.SelectedItem;

                    Galatee.Silverlight.ServiceRecouvrement.RecouvrementServiceClient service = new Galatee.Silverlight.ServiceRecouvrement.RecouvrementServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Recouvrement"));
                    int handler = LoadingManager.BeginLoading("Recuperation des factures ...");
                    service.GetClienteBTADuLotControleAsync(Lot);
                    service.GetClienteBTADuLotControleCompleted += (er, res) =>
                    {
                        try
                        {
                            if (res.Error != null || res.Cancelled)
                                Message.Show("Erreur dans le traitement des méthode de dectection : " + res.Error.InnerException.ToString(), "Erreur");
                            else
                                if (res.Result != null)
                                {
                                    Utility.ActionDirectOrientation<ServicePrintings.CsClient, ServiceRecouvrement.CsClient>(res.Result, null, SessionObject.CheminImpression, "ClientAControler", "Recouvrement", true);
                                }
                                else
                                    Message.Show("Une erreur s'est produite, veuillez consultez le journal des erreurs",
                                        "Erreur");

                            LoadingManager.EndLoading(handler);
                        }
                        catch (Exception)
                        {
                            throw;
                        }
                    };
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, Galatee.Silverlight.Resources.Langue.errorTitle);
            }
        }

       

        
    }
}

