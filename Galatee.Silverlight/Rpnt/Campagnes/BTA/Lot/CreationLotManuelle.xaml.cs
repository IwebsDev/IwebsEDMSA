using Galatee.Silverlight.Rpnt.Helper;
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
    public partial class CreationLotManuelle : ChildWindow
    {
        #region Variables

        string CodeTypeClient = "";
        string CodeTypeTarif = "";
        string CodeTypeCompteur = "";
        string CodeGroupe = "";
        string CodeTournee = "";


        private Galatee.Silverlight.ServiceRecouvrement.CsCampagnesBTAAccessiblesParLUO camp_select;
        private List<Galatee.Silverlight.ServiceRecouvrement.CsElementsDeCampagneBTA> LISTEBRANCHEMENT_Temp = new List<Galatee.Silverlight.ServiceRecouvrement.CsElementsDeCampagneBTA>();
        Galatee.Silverlight.ServiceRecouvrement.CstbLotsDeControleBTA LotNew = new ServiceRecouvrement.CstbLotsDeControleBTA();
        public List<ServiceRecouvrement.CstbElementsLotDeControleBTA> ListElementLot = new List<ServiceRecouvrement.CstbElementsLotDeControleBTA>();

        #endregion


        #region Construction
        public CreationLotManuelle()
        {
            InitializeComponent();
        }
        public CreationLotManuelle(Galatee.Silverlight.ServiceRecouvrement.CsCampagnesBTAAccessiblesParLUO camp_select)
        {
            InitializeComponent();
            this.btn_load_brt_in_lot.Content = "<<";
            LoadTypeClient();
            LoadTypeTarif();
            LoadGroupeFacture();
            LoadTypeCompteur();
            LoadTournee();
            ChargeListeUser();

            this.camp_select = camp_select;
            LISTEBRANCHEMENT_Temp = this.camp_select.ListElementsCamp.ToList();

            LayoutRoot.DataContext = this.camp_select;
            tbx_statu.Text=this.camp_select.Statut_ID!=0?"Actif":"Inactif";
            dg_pop_non_affecte.ItemsSource = this.camp_select.ListElementsCamp;
        }
        #endregion
        List<Galatee.Silverlight.ServiceAccueil.CsUtilisateur> lstAllUser = new List<Galatee.Silverlight.ServiceAccueil.CsUtilisateur>();

        private void ChargeListeUser()
        {
            try
            {

                //Lancer la transaction de mise a jour en base
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service1 = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service1.RetourneListeAllUserCompleted += (sr, res) =>
                {

                    if (res != null && res.Cancelled)
                        return;
                    lstAllUser = res.Result;

                };
                service1.RetourneListeAllUserAsync();
                service1.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;

            }
        }
        List<ServiceFraude.CsSourceControle> lsSource = new List<ServiceFraude.CsSourceControle>();
        private void ChargerSourceControle()
        {
            try
            {
                Galatee.Silverlight.ServiceFraude.FraudeServiceClient client = new Galatee.Silverlight.ServiceFraude.FraudeServiceClient(Utility.Protocole(), Utility.EndPoint("Fraude"));
                client.SelectAllSourceControleCompleted += (ssender, args) =>
                {
                    if (args.Cancelled || args.Error != null)
                    {
                        string error = args.Error.Message;
                        Message.ShowError(error, "");
                        return;
                    }
                    if (args.Result == null)
                    {
                        //Message.ShowError(Languages.msgErreurChargementDonnees, Languages.Parametrage);
                        Message.ShowError(Galatee.Silverlight.Resources.Devis.Languages.msgErreurChargementDonnees, "");
                        return;
                    }
                    if (args.Result != null)
                        lsSource = args.Result;
                };
                client.SelectAllSourceControleAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        void galatee_OkClickedbtn_AgtRecepteur(object sender, EventArgs e)
        {

            Galatee.Silverlight.MainView.UcListeGenerique ctrs = sender as Galatee.Silverlight.MainView.UcListeGenerique;
            if (ctrs.isOkClick)
            {
                ServiceAccueil.CsUtilisateur utilisateur = (ServiceAccueil.CsUtilisateur)ctrs.MyObject;
                this.txtAgt_Recepteur.Text = utilisateur.MATRICULE;
                this.txtAgt_Recepteur.Tag = utilisateur.PK_ID;

            }

        }
        private void btn_AgtRecepteur_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (lstAllUser != null && lstAllUser.Count() > 0)
                {
                    List<object> _LstObj = Galatee.Silverlight.Shared.ClasseMEthodeGenerique.RetourneListeObjet(lstAllUser.Where(c => c.CENTRE == UserConnecte.Centre).ToList());
                    Galatee.Silverlight.MainView.UcListeGenerique ctr = new Galatee.Silverlight.MainView.UcListeGenerique(_LstObj, "MATRICULE", "LIBELLE", "");
                    ctr.Closed += new EventHandler(galatee_OkClickedbtn_AgtRecepteur);
                    ctr.Show();
                }
                else
                {
                    Message.ShowInformation("Aucun utilisareur trouvée", "Information");
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        private void txtAgt_Recepteur_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (this.txtAgt_Recepteur.Text.Length == SessionObject.Enumere.TailleMatricule)
            {
                if (lstAllUser != null && lstAllUser.Count() > 0)
                {
                    ServiceAccueil.CsUtilisateur leuser = lstAllUser.FirstOrDefault(t => t.MATRICULE == this.txtAgt_Recepteur .Text);
                    if (leuser != null)
                    {
                        this.txt_LibelleAgentRecepteur.Text = leuser.LIBELLE;
                        txtAgt_Recepteur.Tag = leuser.PK_ID;
                    }
                    else
                    {
                        Message.ShowInformation("Aucun utilisateur n'existe ", "Information");
                        this.txtAgt_Recepteur.Focus();
                    }
                }
            }
        }
   
        //public void LoadReleveur()
        //{
        //    //RpntServiceClient Service = new RpntServiceClient();
        //    RpntServiceClient service = new RpntServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Rpnt"));
        //    int handler = LoadingManager.BeginLoading("Recuperation des factures ...");
        //    service.GetAgentZontAsync();
        //    service.GetAgentZontCompleted += (er, res) =>
        //    {
        //        try
        //        {
        //            if (res.Error != null || res.Cancelled)
        //                Message.Show("Erreur dans le traitement des méthode de dectection : " + res.Error.InnerException.ToString(), "Erreur");
        //            else
        //                if (res.Result != null)
        //                {
        //                    cbx_releveur.DisplayMemberPath = "MATRICULE";
        //                    cbx_releveur.ItemsSource = res.Result;
        //                }
        //                else
        //                    Message.Show("Une erreur s'est produite, veuillez consultez le journal des erreurs",
        //                        "Erreur");

        //            LoadingManager.EndLoading(handler);
        //        }
        //        catch (Exception ex)
        //        {

        //            throw ex;
        //        }
        //    };
        //}
        public void LoadTypeClient()
        {
            List<Galatee.Silverlight.ServiceAccueil.CsCategorieClient> ListeCategClient = new List<Galatee.Silverlight.ServiceAccueil.CsCategorieClient>();
            if (SessionObject.LstCategorie.Count > 0)
            {
                ListeCategClient = SessionObject.LstCategorie;
                cbxtypeclient.DisplayMemberPath = "LIBELLE";
                cbxtypeclient.ItemsSource = ListeCategClient;
            }
            else
            {
                //RpntServiceClient Service = new RpntServiceClient();
                Galatee.Silverlight.ServiceRecouvrement.RecouvrementServiceClient service = new Galatee.Silverlight.ServiceRecouvrement.RecouvrementServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Recouvrement"));
                int handler = LoadingManager.BeginLoading("Recuperation des type de client ...");
                service.GetTypeClientAsync();
                service.GetTypeClientCompleted += (er, res) =>
                {
                    try
                    {
                        if (res.Error != null || res.Cancelled)
                            Message.Show("Erreur dans le traitement des méthode de dectection : " + res.Error.InnerException.ToString(), "Erreur");
                        else
                            if (res.Result != null)
                            {

                                foreach (var item in res.Result)
                                {
                                    SessionObject.LstCategorie.Add(new Galatee.Silverlight.ServiceAccueil.CsCategorieClient
                                    {
                                        DATECREATION = item.DATECREATION,
                                        DATEMODIFICATION = item.DATEMODIFICATION,
                                        LIBELLE = item.LIBELLE,
                                        CODE = item.CODE,
                                        PK_ID = item.PK_ID,
                                        USERCREATION = item.USERCREATION,
                                        USERMODIFICATION = item.USERMODIFICATION
                                    });

                                }
                                ListeCategClient = SessionObject.LstCategorie;
                                cbxtypeclient.DisplayMemberPath = "LIBELLE";
                                cbxtypeclient.ItemsSource = ListeCategClient;
                            }
                            else
                                Message.Show("Une erreur s'est produite, veuillez consultez le journal des erreurs",
                                    "Erreur");

                        LoadingManager.EndLoading(handler);
                    }
                    catch (Exception ex)
                    {

                        throw ex;
                    }
                };
            }
            cbxtypeclient.DisplayMemberPath = "LIBELLE";
            cbxtypeclient.ItemsSource = ListeCategClient;

        }
        public void LoadTypeTarif()
        {
            List<Galatee.Silverlight.ServiceRecouvrement.CsTypeTarif> ListeTarif = new List<Galatee.Silverlight.ServiceRecouvrement.CsTypeTarif>();

            //RpntServiceClient Service = new RpntServiceClient();
            Galatee.Silverlight.ServiceRecouvrement.RecouvrementServiceClient service = new Galatee.Silverlight.ServiceRecouvrement.RecouvrementServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Recouvrement"));
            int handler = LoadingManager.BeginLoading("Recuperation des Types Tarif ...");
            service.GetTypeTarifAsync();
            service.GetTypeTarifCompleted += (er, res) =>
            {
                try
                {
                    if (res.Error != null || res.Cancelled)
                        Message.Show("Erreur dans le traitement des méthode de dectection : " + res.Error.InnerException.ToString(), "Erreur");
                    else
                    {
                        if (res.Result != null)
                        {
                            ListeTarif = res.Result;
                            cbxtarif.DisplayMemberPath = "LIBELLE";
                            cbxtarif.ItemsSource = ListeTarif;
                        }
                        else
                            Message.Show("Une erreur s'est produite, veuillez consultez le journal des erreurs",
                                "Erreur");

                        LoadingManager.EndLoading(handler);
                    }
                }
                catch (Exception)
                {

                    throw;
                }
            };

            cbxtarif.DisplayMemberPath = "LIBELLE";
            cbxtarif.ItemsSource = ListeTarif;
         
        }
        public void LoadGroupeFacture()
        {
            List<Galatee.Silverlight.ServiceRecouvrement.CsGroupeDeFacturation> ListeGroupefacturation = new List<Galatee.Silverlight.ServiceRecouvrement.CsGroupeDeFacturation>();

            //RpntServiceClient Service = new RpntServiceClient();
            Galatee.Silverlight.ServiceRecouvrement.RecouvrementServiceClient service = new Galatee.Silverlight.ServiceRecouvrement.RecouvrementServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Recouvrement"));
            int handler = LoadingManager.BeginLoading("Recuperation des Groupes Facture ...");
            service.GetGroupeFactureAsync();
            service.GetGroupeFactureCompleted += (er, res) =>
            {
                try
                {
                    if (res.Error != null || res.Cancelled)
                        Message.Show("Erreur dans le traitement des méthode de dectection : " + res.Error.InnerException.ToString(), "Erreur");
                    else
                        if (res.Result != null)
                        {
                            ListeGroupefacturation = res.Result;
                            cbxgropfacture.DisplayMemberPath = "Libelle";
                            cbxgropfacture.ItemsSource = ListeGroupefacturation;
                        }
                        else
                            Message.Show("Une erreur s'est produite, veuillez consultez le journal des erreurs",
                                "Erreur");

                    LoadingManager.EndLoading(handler);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            };
            cbxgropfacture.DisplayMemberPath = "Libelle";
            cbxgropfacture.ItemsSource = ListeGroupefacturation;

        }
        public void LoadTypeCompteur()
        {
            List<Galatee.Silverlight.ServiceRecouvrement.CsTcompteur> ListeTcompt = new List<Galatee.Silverlight.ServiceRecouvrement.CsTcompteur>();


            //RpntServiceClient Service = new RpntServiceClient();
            Galatee.Silverlight.ServiceRecouvrement.RecouvrementServiceClient service = new Galatee.Silverlight.ServiceRecouvrement.RecouvrementServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Recouvrement"));
            int handler = LoadingManager.BeginLoading("Recuperation des Types Compteur ...");
            service.GetTypeCompteurAsync();
            service.GetTypeCompteurCompleted += (er, res) =>
            {
                try
                {
                    if (res.Error != null || res.Cancelled)
                        Message.Show("Erreur dans le traitement des méthode de dectection : " + res.Error.InnerException.ToString(), "Erreur");
                    else
                        if (res.Result != null)
                        {
                            ListeTcompt = res.Result;
                            cbxcompteur.DisplayMemberPath = "LIBELLE";
                            cbxcompteur.ItemsSource = ListeTcompt;
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
            cbxcompteur.DisplayMemberPath = "LIBELLE";
            cbxcompteur.ItemsSource = ListeTcompt;

        }
        public void LoadTournee()
        {
            List<Galatee.Silverlight.ServiceAccueil.CsTournee> ListeTournee = new List<Galatee.Silverlight.ServiceAccueil.CsTournee>();
            if (SessionObject.LstZone.Count() > 0)
            {
                ListeTournee = SessionObject.LstZone;
                cbxtournee.DisplayMemberPath = "CODE";
                cbxtournee.ItemsSource = ListeTournee;
            }
            //else
            //{
            //    //RpntServiceClient Service = new RpntServiceClient();
            //    RpntServiceClient service = new RpntServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Rpnt"));
            //    int handler = LoadingManager.BeginLoading("Recuperation des tournées ...");
            //    service.GetTourneeAsync();
            //    service.GetTourneeCompleted += (er, res) =>
            //    {
            //        try
            //        {
            //            if (res.Error != null || res.Cancelled)
            //                Message.Show("Erreur dans le traitement des méthode de dectection : " + res.Error.InnerException.ToString(), "Erreur");
            //            else
            //                if (res.Result != null)
            //                {
            //                    SessionObject.tournee = res.Result;
            //                    ListeTournee = SessionObject.tournee;
            //                    cbxtournee.DisplayMemberPath = "LIBELLE";
            //                    cbxtournee.ItemsSource = ListeTournee;
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
        public void LoadClientEligible()
        {
            try
            {
                if (cbxtypeclient.SelectedItem != null)
                {
                    CodeTypeClient = ((Galatee.Silverlight.ServiceAccueil.CsCategorieClient)cbxtypeclient.SelectedItem).CODE.ToString();
                }
                if (cbxtarif.SelectedItem != null)
                {
                    CodeTypeTarif = ((Galatee.Silverlight.ServiceRecouvrement.CsTypeTarif)cbxtarif.SelectedItem).PK_ID.ToString();
                }
                if (cbxgropfacture.SelectedItem != null)
                {
                    CodeGroupe = ((Galatee.Silverlight.ServiceRecouvrement.CsGroupeDeFacturation)cbxgropfacture.SelectedItem).GroupeDeFacturation.ToString();
                }
                if (cbxcompteur.SelectedItem != null)
                {
                    CodeTypeCompteur = ((Galatee.Silverlight.ServiceRecouvrement.CsTcompteur)cbxcompteur.SelectedItem).PK_ID.ToString();
                }
                if (cbxtournee.SelectedItem != null)
                {
                    CodeTournee = ((Galatee.Silverlight.ServiceAccueil.CsTournee)cbxtournee.SelectedItem).PK_ID.ToString();
                }

                var ElementCampFiltrer = GetClientEligibleLot(CodeTypeClient, CodeTypeTarif, CodeTypeCompteur, CodeGroupe, CodeTournee);

                dg_pop_non_affecte.ItemsSource = ElementCampFiltrer;
            }
            catch (Exception ex)
            {
                Message.Show(ex.Message, "Erreur");
            }
        }

        Galatee.Silverlight.ServiceFraude.CsClientFraude Clientfraude = new Galatee.Silverlight.ServiceFraude.CsClientFraude();
        List<Galatee.Silverlight.ServiceFraude.CsFraude> listForInsertOrUpdate = new List<Galatee.Silverlight.ServiceFraude.CsFraude>();
        List<Galatee.Silverlight.ServiceFraude.CsDenonciateur> listForInsertOrUpdate1 = new List<Galatee.Silverlight.ServiceFraude.CsDenonciateur>();
        Galatee.Silverlight.ServiceFraude.CsDemandeFraude LaDemande = new Galatee.Silverlight.ServiceFraude.CsDemandeFraude();

        private void SaveCampagne()
        {
            ////Association de campagne à liste de branchement selection
            Galatee.Silverlight.ServiceRecouvrement.RecouvrementServiceClient service = new Galatee.Silverlight.ServiceRecouvrement.RecouvrementServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Recouvrement"));
            int handler = LoadingManager.BeginLoading("Traitement de données ...");
            //List<Galatee.Silverlight.ServiceRecouvrement.CsCampagnesBTAAccessiblesParLUO> listecampageneTosave = (List<Galatee.Silverlight.ServiceRecouvrement.CsCampagnesBTAAccessiblesParLUO>)dgCampagne.ItemsSource;
            List<Galatee.Silverlight.ServiceRecouvrement.CsCampagnesBTAAccessiblesParLUO> listecampageneTosave = new List<Galatee.Silverlight.ServiceRecouvrement.CsCampagnesBTAAccessiblesParLUO>();
            listecampageneTosave.Add(this.camp_select);
            //service.SaveCampagneElementAsync(listecampageneTosave, new CsREFMETHODEDEDETECTIONCLIENTSBTA(), "");
            service.SaveCampagneElementAsync(listecampageneTosave);
            service.SaveCampagneElementCompleted += (er, res) =>
            {
                try
                {
                    if (res.Error != null || res.Cancelled)
                        Message.Show("Erreur dans le traitement des méthode de dectection : " + res.Error.InnerException.ToString(), "Erreur");
                    else
                        if (res.Result != false)
                        {
                            Message.Show("Modification enrégistrer avec succès",
                                "Erreur");

                            InitialisationFraude();
                            //SessionObject.campagne.Clear();
                            //SessionObject.campagne = listecampageneTosave;
                            //CallServiceToLoadCamp();
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

        private void InitialisationFraude()
        {
            ////Association de campagne à liste de branchement selection
            Galatee.Silverlight.ServiceRecouvrement.RecouvrementServiceClient service = new Galatee.Silverlight.ServiceRecouvrement.RecouvrementServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Recouvrement"));
            int handler = LoadingManager.BeginLoading("Traitement de données ...");
     
            //service.SaveCampagneElementAsync(listecampageneTosave, new CsREFMETHODEDEDETECTIONCLIENTSBTA(), "");
            service.InitialisationFraudAsync(ListElementLot);
            service.InitialisationFraudCompleted += (er, res) =>
            {
                try
                {
                    if (res.Error != null || res.Cancelled)
                        Message.Show("Erreur dans le traitement des méthode de dectection : " + res.Error.InnerException.ToString(), "Erreur");
                    else
                        if (res.Result != null)
                        {
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

        public ObservableCollection<Galatee.Silverlight.ServiceRecouvrement.CsElementsDeCampagneBTA> GetClientEligibleLot(string CodeTypeClient = "", string CodeTypeTarif = "", string CodeTypeCompteur = "", string CodeGroupe = "", string CodeTournee = "")
        {
            try
            {
                ObservableCollection<Galatee.Silverlight.ServiceRecouvrement.CsElementsDeCampagneBTA> list_brt_to_return = new ObservableCollection<Galatee.Silverlight.ServiceRecouvrement.CsElementsDeCampagneBTA>();
                if (camp_select.ListElementsCamp != null)
                {
                    List<Galatee.Silverlight.ServiceRecouvrement.CsElementsDeCampagneBTA> list_brt = new List<Galatee.Silverlight.ServiceRecouvrement.CsElementsDeCampagneBTA>();
                    list_brt = LISTEBRANCHEMENT_Temp.ToList();

                    if (CodeTypeClient != "")
                    {
                        //list_brt = list_brt.Where(cl => cl.CodeTypeClient == int.Parse(CodeTypeClient)).Select(t => t).ToList();
                    }
                    if (CodeTypeTarif != "")
                    {
                        //list_brt = list_brt.Where(cl => cl.CodeTypeTarif == int.Parse(CodeTypeTarif)).Select(t => t).ToList();
                    }
                    if (CodeTypeCompteur != "")
                    {
                        //list_brt = list_brt.Where(cl => cl.CodeTypeCompteur.Select(c => c.FK_IDTCOMPT).Contains(int.Parse(CodeTypeTarif))).Select(t => t).ToList();
                    }
                    if (CodeGroupe != "")
                    {
                        //list_brt = list_brt.Where(cl => cl.CodeGroupe == int.Parse(CodeGroupe)).Select(t => t).ToList();
                    }
                    if (CodeTournee != "")
                    {
                        //list_brt = list_brt.Where(cl => cl.FK_IDTOURNEE == int.Parse(CodeTournee)).Select(t => t).ToList();
                    }
                    foreach (var item in list_brt)
                    {
                        list_brt_to_return.Add(item);
                    }

                }
                return list_brt_to_return;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private Galatee.Silverlight.ServiceRecouvrement.CstbLotsDeControleBTA Load_Info_Lot()
        {
            Galatee.Silverlight.ServiceRecouvrement.CstbLotsDeControleBTA Lot_To_Save = new Galatee.Silverlight.ServiceRecouvrement.CstbLotsDeControleBTA();
            Lot_To_Save.Campagne_ID = camp_select.Campagne_ID;
            Lot_To_Save.Critere_GroupeDeFacturation =CodeGroupe!=""? int.Parse(CodeGroupe):int.MinValue;
            if (CodeTournee!="")
            {
                Lot_To_Save.Critere_IdTournee = int.Parse(CodeTournee);
            }

            Lot_To_Save.Critere_TypeClient = CodeTypeClient != "" ? int.Parse(CodeTypeClient) : int.MinValue;
            Lot_To_Save.Critere_TypeCompteur = CodeTypeCompteur != "" ? int.Parse(CodeTypeCompteur) : int.MinValue;
            Lot_To_Save.Critere_TypeTarif = CodeTypeTarif;
            Lot_To_Save.DateCreation = camp_select.DateCreation;
            Lot_To_Save.Libelle_Lot = tbx_lib_lot.Text;
            Lot_To_Save.Lot_ID = Guid.NewGuid();
            Lot_To_Save.MatriculeAgentControleur = this.txtAgt_Recepteur.Tag != null ? this.txtAgt_Recepteur.Text :string.Empty ;
            Lot_To_Save.MatriculeAgentCreation = tbx_createur.Text;
            Lot_To_Save.NbreElementsDuLot = dg_population_lot.ItemsSource != null ? ((List<Galatee.Silverlight.ServiceRecouvrement.CsElementsDeCampagneBTA>)dg_population_lot.ItemsSource).Count : 0;
            Lot_To_Save.StatutLot_ID = !string.IsNullOrWhiteSpace(tbx_statu.Text) ? tbx_statu.Text=="Actif"?1:0 : int.MinValue;

            Lot_To_Save.ListElementLot = new List<Galatee.Silverlight.ServiceRecouvrement.CstbElementsLotDeControleBTA>();
            foreach (var item in (List<Galatee.Silverlight.ServiceRecouvrement.CsElementsDeCampagneBTA>)dg_population_lot.ItemsSource)
            {
                Lot_To_Save.ListElementLot.Add(new Galatee.Silverlight.ServiceRecouvrement.CstbElementsLotDeControleBTA
                {
                    Contrat_ID = item.Contrat_ID.ToString(),
                    DateAffectationLot = DateTime.Now,
                    DateSelection = DateTime.Now,
                    Debut_PerAA = camp_select.DateDebutControles.Year,
                    Debut_PerMM = camp_select.DateDebutControles.Month,
                    Lot_ID = Lot_To_Save.Lot_ID
                    //NOMABON = item.Nom
                });
            }
            this.ListElementLot = Lot_To_Save.ListElementLot;
            return Lot_To_Save;
        }
        private void Update_Camp_Info()
        {
            LotNew = Load_Info_Lot();
            camp_select.ListLot.Add(LotNew);

            ObservableCollection<Galatee.Silverlight.ServiceRecouvrement.CsElementsDeCampagneBTA> brt_non_affecte = new ObservableCollection<Galatee.Silverlight.ServiceRecouvrement.CsElementsDeCampagneBTA>();
            foreach (var item in (List<Galatee.Silverlight.ServiceRecouvrement.CsElementsDeCampagneBTA>)dg_pop_non_affecte.ItemsSource)
            {
                brt_non_affecte.Add(item);
            }
            camp_select.ListElementsCamp = brt_non_affecte.ToList();
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (dg_population_lot.ItemsSource != null)
            {
                if (camp_select.ListLot != null)
                {
                    if (this.txtAgt_Recepteur.Tag != null)
                    {
                        Update_Camp_Info();
                        SaveCampagne();
                    }
                    else
                    {
                        Message.ShowWarning("Veuillez choisir un Agent controlleur", "Information");
                    }

                }
                else
                {
                    camp_select.ListLot = new List<Galatee.Silverlight.ServiceRecouvrement.CstbLotsDeControleBTA>();
                    if (this.txtAgt_Recepteur.Tag != null)
                    {
                        Update_Camp_Info();
                        SaveCampagne();
                    }
                    else
                    {
                        Message.ShowWarning("Veuillez choisir un Agent controlleur", "Information");
                    }
                }
            }
            
            this.DialogResult = true;
        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            //if (camp_select.LISTEBRANCHEMENT!=null)
            //{
            //    if (camp_select.LISTEBRANCHEMENT.Count <= 0)
            //    {
            //        foreach (var item in LISTEBRANCHEMENT_Temp)
            //        {
            //            camp_select.LISTEBRANCHEMENT.Add(item);
            //        }

            //    }
            //}
            
            this.DialogResult = false;
        }
       
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            LoadClientEligible();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            new CommonMethode().Transfertclient<Galatee.Silverlight.ServiceRecouvrement.CsElementsDeCampagneBTA>(dg_pop_non_affecte, dg_population_lot);
        }
        private void btn_load_brt_in_lot_Click_1(object sender, RoutedEventArgs e)
        {
            new CommonMethode().Transfertclient<Galatee.Silverlight.ServiceRecouvrement.CsElementsDeCampagneBTA>(dg_population_lot, dg_pop_non_affecte);

        }

    }
}

