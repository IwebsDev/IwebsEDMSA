using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
//using Galatee.Silverlight.Devis;
using Galatee.Silverlight.ServiceAccueil ;
using Galatee.Silverlight.ServiceCaisse;
using Galatee.Silverlight.ServiceAuthenInitialize;
using System.Collections.Generic;
using Galatee.Silverlight.Caisse;
using System.Windows.Browser;
using System.Linq;

using Galatee.Silverlight.ServiceAdministration;
using Galatee.Silverlight.Resources.Caisse;
using Galatee.Silverlight.ServiceFacturation;
using Galatee.Silverlight.ServiceRecouvrement;
using Galatee.Silverlight.ServiceTarification;

namespace Galatee.Silverlight.MainView
{
    public class TriggerMenuView
    {
        Dictionary<ServiceAuthenInitialize.CSMenuGalatee, List<ServiceAuthenInitialize.TreeNodes>> dico = new Dictionary<ServiceAuthenInitialize.CSMenuGalatee, List<ServiceAuthenInitialize.TreeNodes>>();
        static MenuViewModel viewModelG = new MenuViewModel();
        string Module = string.Empty;
        string EtatCaisse = string.Empty;
        static bool IsRefreh = false;
        string ServerMode = SessionObject.ServerEtatInit;
        Galatee.Silverlight.ServiceCaisse.CParametre CaisseSelection = new Galatee.Silverlight.ServiceCaisse.CParametre();
        //ServiceCaisse.CsOpenningDay CaisseOverte = null;
        List<CsCaisse> _ListeCaisse = new List<CsCaisse>();
        Page _mainPage;
        static Library.Menu _menu;
        Library.Menu _menu2;

        public TriggerMenuView(Page mainPage, Library.Menu m)
        {
            try
            {
                _mainPage = mainPage;
                _menu = m;
                _menu2 = m;
                RetournePosteConnecter();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "TriggerMenuView)");
                throw ex;
            }
        }
        public TriggerMenuView()
        {
            
        }
        void ConnexionSuccess(object sender, EventArgs e)
        {
            FrmCreeCaisse frm = sender as FrmCreeCaisse;
            UserConnecte.CaisseSelect = frm.CaisseSelectionee;
            UserConnecte.MatriculeSelect = frm.MatriculeSelectionee;

            RefreshMenuBar();
            MenuViewModel viewModel = new MenuViewModel(_mainPage, dico, "Galatee.Silverlight." + Module, Module);
            viewModelG = viewModel;

            if (!IsRefreh)
            {
                _mainPage.DataContext = viewModelG;

                IsRefreh = true;
            }
        }

        void RefreshMenuBar()
        {

            if (IsRefreh)
            {

                try
                {
                    IsRefreh = false;
                    _menu.RefleshMenu();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        



     void BuildDashbord()
        {
            //#region Affichage du tableau de bord //Ajouté par WCO le 06/09/2015
            //1- On recupère les groupes de validation de l'utilisateur
            int back = LoadingManager.BeginLoading("Chargement du Tableau de bord");
            Galatee.Silverlight.ServiceParametrage.ParametrageClient clientP = new Galatee.Silverlight.ServiceParametrage.ParametrageClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Parametrage"));
            clientP.VerifierAppartenanceGroupeValidationCompleted += (gsender, gargs) =>
            {
                LoadingManager.EndLoading(back);
                if (gargs.Cancelled || gargs.Error != null)
                {
                    string error = gargs.Error.Message;
                    Message.Show(error, "Main");
                    return;
                }
                if (gargs.Result == null)
                {
                    Message.ShowError(Galatee.Silverlight.Resources.Parametrage.Languages.msgErreurChargementDonnees, Galatee.Silverlight.Resources.Parametrage.Languages.Parametrage);
                    return;
                }
                if (gargs.Result)
                {
                    #region Affichage du tableau de bord
                    SessionObject.moduleCourant = "Workflow";
                    UserControl UcForm = new Galatee.Silverlight.Workflow.UcWKFDashBoard();
                    if (UcForm != null)
                        SessionObject.ViewBox.Child = new Galatee.Silverlight.Workflow.UcWKFDashBoard();
                    #endregion
                }
            };
            clientP.VerifierAppartenanceGroupeValidationAsync(UserConnecte.PK_ID); 
        }




        public void RaiseView(string module, List<int> idProfil, Grid pGrid)
        {
            try
            {
                Module = module;
                AuthentInitializeServiceClient client = new AuthentInitializeServiceClient(Utility.ProtocoleFacturation() , Utility.EndPoint("Initialisation"));
                ////client.GetMenuDuRoleCompleted += (senders, arg) =>
                client.GetMenuDuProfilCompleted += (senders, arg) =>
                {
                    if (arg.Cancelled || arg.Error != null)
                    {
                        Message.ShowInformation("Error occurs while processing request ! ", "GetMenuDuRole");
                        return;
                        // 
                    }
                    dico = arg.Result;
                    if (dico.Count == 0 || dico == null)
                    {
                        Message.ShowInformation("Aucun module trouvé" + module + "1" + idProfil + "2" , Galatee.Silverlight.Resources.Langue.errorTitle);
                        return;
                    }
                    else
                    {
                        SessionObject.IsChargerDashbord = false;
                        ChargerDonneeDuSite();
                        if (Module == "Caisse")
                            InitialisationDonnneesCaisse();
                        else if (Module == "Accueil")
                        {
                         
                            InitialiseDonneesGuichet();

                            RefreshMenuBar();
                            var conteneur = Library.AllInOne.FindControl<Viewbox>(pGrid, typeof(Viewbox), "Body");
                            conteneur.Margin = new Thickness(0, 30, 0, 0);
                            SessionObject.ViewBox = conteneur;
                            MenuViewModel viewModel = new MenuViewModel(_mainPage, dico, "Galatee.Silverlight." + Module, Module);
                            viewModelG = viewModel;

                            if (!IsRefreh)
                            {
                                _mainPage.DataContext = viewModelG;

                                IsRefreh = true;
                            }
                            string EstActeurWkf =Classes.IsolatedStorage.getIsWorkfloGp();
                            if (EstActeurWkf == "OUI")
                            {
                                SessionObject.IsChargerDashbord = true;
                                #region Affichage du tableau de bord
                                UserControl UcForm = null;
                                UcForm = new Galatee.Silverlight.Workflow.UcWKFDashBoard();
                                if (UcForm != null)
                                    SessionObject.ViewBox.Child = UcForm;
                                #endregion
                            }
                            
                        }
                        else if (module == "Index" || module == "Facturation")
                        {
                            InitialiseDonneesFacturation();
                            RefreshMenuBar();
                            MenuViewModel viewModel = new MenuViewModel(_mainPage, dico, "Galatee.Silverlight." + Module, Module);
                            viewModelG = viewModel;

                            if (!IsRefreh)
                            {
                                _mainPage.DataContext = viewModelG;

                                IsRefreh = true;
                            }


                        }
                        else if (module == "Scelles")
                        {
                            InitDonneeScelle();
                            RefreshMenuBar();
                            MenuViewModel viewModel = new MenuViewModel(_mainPage, dico, "Galatee.Silverlight." + Module, Module);
                            viewModelG = viewModel;

                            if (!IsRefreh)
                            {
                                _mainPage.DataContext = viewModelG;

                                IsRefreh = true;
                            }


                        }
                        else if (module == "Administration")
                        {
                            InitialisationDonnesAdministration();
                            RefreshMenuBar();
                            MenuViewModel viewModel = new MenuViewModel(_mainPage, dico, "Galatee.Silverlight." + Module, Module);
                            viewModelG = viewModel;

                            if (!IsRefreh)
                            {
                                _mainPage.DataContext = viewModelG;

                                IsRefreh = true;
                            }


                        }
                        else if (module == "SIG")
                        {
                            InitialisationDonnesCampagnes();
                            RefreshMenuBar();
                            MenuViewModel viewModel = new MenuViewModel(_mainPage, dico, "Galatee.Silverlight." + Module, Module);
                            viewModelG = viewModel;

                            if (!IsRefreh)
                            {
                                _mainPage.DataContext = viewModelG;

                                IsRefreh = true;
                            }


                        }
                        else if (Module == "Recouvrement" || Module == "Precontentieux" || Module == "Depannage")
                        {
                            InitialiseDonneesGuichet();
                            RefreshMenuBar();

                            MenuViewModel viewModel = new MenuViewModel(_mainPage, dico, "Galatee.Silverlight." + Module, Module);
                            viewModelG = viewModel;

                            if (!IsRefreh)
                            {
                                _mainPage.DataContext = viewModelG;

                                IsRefreh = true;
                            }
                        }
                        else if (module == "Tarification")
                        {
                            InitialiseDonneesTarification();
                            RefreshMenuBar();
                            MenuViewModel viewModel = new MenuViewModel(_mainPage, dico, "Galatee.Silverlight." + Module, Module);
                            viewModelG = viewModel;

                            if (!IsRefreh)
                            {
                                _mainPage.DataContext = viewModelG;

                                IsRefreh = true;
                            }
                        }
                        else // si le matricule ne correspond pas à celui d'une caissiere
                        {
                            /// Peupler les menus du module select 
                            try
                            {
                                RefreshMenuBar();
                                MenuViewModel viewModel = new MenuViewModel(_mainPage, dico, "Galatee.Silverlight." + Module, Module);
                                viewModelG = viewModel;
                                if (!IsRefreh)
                                {
                                    _mainPage.DataContext = viewModelG;

                                    IsRefreh = true;
                                }
                            }
                            catch (Exception ex)
                            {

                                string error = ex.Message;
                            }

                        }

                    }

                };
                client.GetMenuDuProfilAsync(module, idProfil);

            }
            catch (Exception ex)
            {
                Message.ShowInformation("Erreur au chargement de tableau de bord", "DashBord");
            }
        }


        //public void RaiseView(string module, List<int> _lProfil, Grid pGrid)
        //{
        //    try
        //    {
        //        Module = module;
        //        AuthentInitializeServiceClient client = new AuthentInitializeServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Initialisation"));
        //        ////client.GetMenuDuRoleCompleted += (senders, arg) =>

        //        foreach (int item in _lProfil)
        //        {
        //            client.GetMenuDuProfilCompleted += (senders, arg) =>
        //            {
        //                if (arg.Cancelled || arg.Error != null)
        //                {
        //                    Message.ShowInformation("Error occurs while processing request ! ", "GetMenuDuRole");
        //                    return;
        //                    // 
        //                }

        //                if (arg.Result != null && arg.Result.Count > 0)
        //                {

        //                    foreach (KeyValuePair<ServiceAuthenInitialize.CSMenuGalatee, List<ServiceAuthenInitialize.TreeNodes>> menu in arg.Result)
        //                    {

        //                        if ((menu.Value != null) && (menu.Value.Count != 0))
        //                        {
        //                            dico.Add(menu.Key, menu.Value);
        //                        }
        //                    }

        //                }

        //            };
        //            client.GetMenuDuProfilAsync(module, item);
        //        }

        //        TraiterMenus(module, pGrid);

        //    }
        //    catch (Exception ex)
        //    {
        //        Message.ShowInformation("Erreur au chargement de tableau de bord", "DashBord");
        //    }
        //}


        private void TraiterMenus(string module, Grid pGrid)
        {
                    if (dico.Count == 0 || dico == null)
                    {
                        Message.ShowInformation("Aucun menu trouvé" + module + "1", Galatee.Silverlight.Resources.Langue.errorTitle);
                        return;
                    }
                    else
                    {
                        SessionObject.IsChargerDashbord = false;
                        ChargerDonneeDuSite();
                        if (Module == "Caisse")
                            InitialisationDonnneesCaisse();
                        else if (Module == "Accueil")
                        {

                            InitialiseDonneesGuichet();

                            RefreshMenuBar();
                            var conteneur = Library.AllInOne.FindControl<Viewbox>(pGrid, typeof(Viewbox), "Body");
                            conteneur.Margin = new Thickness(0, 30, 0, 0);
                            SessionObject.ViewBox = conteneur;
                            MenuViewModel viewModel = new MenuViewModel(_mainPage, dico, "Galatee.Silverlight." + Module, Module);
                            viewModelG = viewModel;

                            if (!IsRefreh)
                            {
                                _mainPage.DataContext = viewModelG;

                                IsRefreh = true;
                            }
                            string EstActeurWkf = Classes.IsolatedStorage.getIsWorkfloGp();
                            if (EstActeurWkf == "OUI")
                            {
                                SessionObject.IsChargerDashbord = true;
                                #region Affichage du tableau de bord
                                UserControl UcForm = null;
                                UcForm = new Galatee.Silverlight.Workflow.UcWKFDashBoard();
                                if (UcForm != null)
                                    SessionObject.ViewBox.Child = UcForm;
                                #endregion
                            }

                        }
                        else if (module == "Index" || module == "Facturation")
                        {
                            InitialiseDonneesFacturation();
                            RefreshMenuBar();
                            MenuViewModel viewModel = new MenuViewModel(_mainPage, dico, "Galatee.Silverlight." + Module, Module);
                            viewModelG = viewModel;

                            if (!IsRefreh)
                            {
                                _mainPage.DataContext = viewModelG;

                                IsRefreh = true;
                            }


                        }
                        else if (module == "Scelles")
                        {
                            InitDonneeScelle();
                            RefreshMenuBar();
                            MenuViewModel viewModel = new MenuViewModel(_mainPage, dico, "Galatee.Silverlight." + Module, Module);
                            viewModelG = viewModel;

                            if (!IsRefreh)
                            {
                                _mainPage.DataContext = viewModelG;

                                IsRefreh = true;
                            }


                        }
                        else if (module == "Administration")
                        {
                            InitialisationDonnesAdministration();
                            RefreshMenuBar();
                            MenuViewModel viewModel = new MenuViewModel(_mainPage, dico, "Galatee.Silverlight." + Module, Module);
                            viewModelG = viewModel;

                            if (!IsRefreh)
                            {
                                _mainPage.DataContext = viewModelG;

                                IsRefreh = true;
                            }


                        }
                        else if (module == "SIG")
                        {
                            InitialisationDonnesCampagnes();
                            RefreshMenuBar();
                            MenuViewModel viewModel = new MenuViewModel(_mainPage, dico, "Galatee.Silverlight." + Module, Module);
                            viewModelG = viewModel;

                            if (!IsRefreh)
                            {
                                _mainPage.DataContext = viewModelG;

                                IsRefreh = true;
                            }


                        }
                        else if (Module == "Recouvrement" || Module == "Precontentieux" || Module == "Depannage")
                        {
                            InitialiseDonneesGuichet();
                            RefreshMenuBar();

                            MenuViewModel viewModel = new MenuViewModel(_mainPage, dico, "Galatee.Silverlight." + Module, Module);
                            viewModelG = viewModel;

                            if (!IsRefreh)
                            {
                                _mainPage.DataContext = viewModelG;

                                IsRefreh = true;
                            }
                        }
                        else if (module == "Tarification")
                        {
                            InitialiseDonneesTarification();
                            RefreshMenuBar();
                            MenuViewModel viewModel = new MenuViewModel(_mainPage, dico, "Galatee.Silverlight." + Module, Module);
                            viewModelG = viewModel;

                            if (!IsRefreh)
                            {
                                _mainPage.DataContext = viewModelG;

                                IsRefreh = true;
                            }
                        }
                        else // si le matricule ne correspond pas à celui d'une caissiere
                        {
                            /// Peupler les menus du module select 
                            try
                            {
                                RefreshMenuBar();
                                MenuViewModel viewModel = new MenuViewModel(_mainPage, dico, "Galatee.Silverlight." + Module, Module);
                                viewModelG = viewModel;
                                if (!IsRefreh)
                                {
                                    _mainPage.DataContext = viewModelG;

                                    IsRefreh = true;
                                }
                            }
                            catch (Exception ex)
                            {

                                string error = ex.Message;
                            }

                        }

                    }

        }


        private void InitialisationDonnesCampagnes()
        {
            //LoadCampagne();
            //LoadCampagneCoupure();
        }
        void diag_Closed(object sender, EventArgs e)
        {
            try
            {
                HtmlPage.Document.Submit();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
       

        void ChargerFonction()
        {
            // desactivation du treeview des modules
            try
            {
                if (SessionObject.ListeFonction != null && SessionObject.ListeFonction.Count != 0)
                    return;
                AdministrationServiceClient client = new AdministrationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Administration"));
                client.SELECT_All_FonctionCompleted += (ss, res) =>
                {
                    try
                    {
                        if (res.Cancelled || res.Error != null)
                        {
                            string error = res.Error.Message;
                            Message.ShowError(error, Langue.errorTitle);
                            return;
                        }

                        if (res.Result == null || res.Result.Count == 0)
                        {
                            Message.ShowInformation(Galatee.Silverlight.Resources.Langue.msgNodata, Langue.informationTitle);
                            return;
                        }
                        SessionObject.ListeFonction = res.Result ;
                    }
                    catch (Exception ex)
                    {
                        Message.ShowError(ex.Message, Langue.informationTitle);
                    }

                };
                client.SELECT_All_FonctionAsync();

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        void ChargerProgramme()
        {
            if (SessionObject.ListeProgramMenu != null && SessionObject.ListeProgramMenu.Count != 0)
                return;
            AdministrationServiceClient prgram = new AdministrationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Administration"));
            prgram.RetourneAllModuleProgramCompleted += (sprog, resprog) =>
            {
                try
                {
               
                    if (resprog.Result == null || resprog.Result.Count == 0)
                    {
                        Message.ShowInformation(Galatee.Silverlight.Resources.Langue.msgNodata, Langue.informationTitle);
                        return;
                    }
                    SessionObject.ListeProgramMenu = resprog.Result;
                }
                catch (Exception ex)
                {
                    Message.ShowError(ex.Message, Langue.errorTitle);
                }

            };
            prgram.RetourneAllModuleProgramAsync();
        
        }


   
        void InitialisationDonnesAdministration()
        {
            ChargerDonneeDuSite ();
            RetourneTousUtlisateur();
            ChargerFonction();
            ChargerProgramme();
            RetourneListeDesAgents();
            ChargerListeProfil();
      

        }
        void InitialisationDonnneesCaisse()
        {
            try
            {
                //LoadingManager.EndLoading(loaderHandler);
                RetourneTousUtlisateur();
                RetourneListeDesModReglement();
                RecuperationListBanque();
                ChargerDonneeDuSite();
                RetourneLstFraixTimbre();
                RetourneListeDesLibelleTop();
                RetourneListeDesNature();
                ChargerCoper();
                ChargerCodeRegroupement();
                ChargerTauxMini("000231");
                VerifierEtatCaisse();
                ChargerTypeDemande();
                ChargerListeDeProduit();

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        void InitialiseDonneesGuichet()
        {
            ChargerTypeReclamation();
            ChargerPuissance();
            ChargerTypeComptage();
            ChargerForfait();
            ChargerTarif();
            ChargerTarifPuissance();
            ChargerFrequence();
            ChargerMois();
            ChargerApplicationTaxe();
            ChargerLaListeDesCommunes();
            ChargerListeDesPostesSource();
            RetourneListeDesDepartHta();
            ChargerListeDesPostesTransformation();
            //RetourneListeDesDepartBT();
            ChargeQuartier();
            ChargeSecteur();
            ChargeRue();
            ChargerTournee();
            ChargerEtapeDemande();
            RetourneListeDenominationAll();
            ChargerTypeDemande();
            ChargerTypeBranchement();
            ChargerRubriqueDevis();
            ChargerListeDeProduit();
            ChargerCategorie();
            ChargerFermable();
            ChargerPuissanceInstalle();
            ChargerNatureClient();
            RetourneListeDesNature();
            ChargerCivilite();
            ChargerNationnalite();
            ChargerCodeRegroupement();
            ChargerCodeConsomateur();
            ChargerCalibreCompteur();
            ChargerReglageCompteur();
            ChargerCadran();
            ChargerMarque();
            ChargerTypeCompteur();
            RetouneListeDesTaxes();
            RetourneListeDesLibelleTop();
            ChargerCoutDemande();         
            RetourneListeDesCas();
            ChargerTypeLot();
            ChargerOrigine();
            ChargerCoper();
            ChargerTarifReglageCompteur();
            ChargerTarifParCategorie();
            ChargerPuissanceReglageCompteur();
            RetouneListeDesTaxes();
            RemplirTypeClient();
        }
        private void RemplirTypeClient()
        {
            try
            {
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.GetAllTypeClientAsync();
                service.GetAllTypeClientCompleted += (ssender, args) =>
                {
                    if (args.Cancelled || args.Error != null)
                    {
                        string error = args.Error.Message;
                        Message.ShowError(error, Galatee.Silverlight.Resources.Devis.Languages.txtDevis);
                        return;
                    }
                    SessionObject.LstTypeClient = args.Result;
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        void InitialiseDonneesRecouvrement()
        {

            ChargerCategorie();
            ChargerNatureClient();
            RetourneListeDesNature();
            RetourneListeDesLibelleTop();
            ChargerCoper();
            ChargerMotifRejetCheque();
            ChargerDonneeDuSite();


        }
        void InitDonneeScelle()
        {
            RemplirListeActivitesExistant();
            RemplirListFournisseursExistant();
            RemplirListeOrigineScellesExistant();
            RemplirListeCouleurExistant();
            ChargerCalibreCompteur();
            ChargerTypeCompteur();
            ChargerListeDeProduit();
            RemplirListeDesModelesMarqueExistant();
            RemplirListeDesEtatCompteursExistant();

        }
        void InitialiseDonneesFacturation()
        {
            ChargerFrequence();
            ChargerCategorie();
            ChargerListeDeProduit();
            ChargerDonneeDuSite();
            ChargerTournee();
            ChargerCodeRegroupement();

        }

        void InitialiseDonneesIndex()
        {
            ChargerFrequence();
            ChargerCategorie();
            ChargerListeDeProduit();
            ChargerDonneeDuSite();
            ChargerTournee();

        }
        void InitialiseDonneesTarification()
        {
            LoadAllRedevance();
            LoadAllRechercheTarif();
            //LoadAllTarifFacturation();
            LoadAllVariableTarif();


            LoadAllModeCalcule();
            LoadAllModeApplicationTarif();
            ChargerLaListeDesCommunes();
            ChargerDonneeDuSite();
        }

        public void LoadAllRedevance()
        {

            try
            {
                if (!(SessionObject.ListeRedevence.Count > 0))
                {
                    TarificationServiceClient service = new TarificationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Tarification"));
                    //int handler = LoadingManager.BeginLoading("Chargement des donnée ...");
                    service.LoadAllRedevanceAsync();
                    service.LoadAllRedevanceCompleted += (er, res) =>
                    {
                        try
                        {
                            if (res.Error != null || res.Cancelled)
                                Message.Show("Erreur dans le traitement des méthode de dectection : " + res.Error.InnerException.ToString(), "Erreur");
                            else
                                if (res.Result != null)
                                {
                                    SessionObject.ListeRedevence = res.Result;
                                }
                                else
                                    Message.Show("Une erreur s'est produite, veuillez consultez le journal des erreurs",
                                        "Erreur");
                            //LoadingManager.EndLoading(handler);
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    };

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void LoadAllRechercheTarif()
        {

            try
            {
                if (!(SessionObject.ListeRechercheTarif.Count > 0))
                {
                    TarificationServiceClient service = new TarificationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Tarification"));
                    //int handler = LoadingManager.BeginLoading("Chargement des donnée ...");
                    service.LoadAllRechercheTarifAsync();
                    service.LoadAllRechercheTarifCompleted += (er, res) =>
                    {
                        try
                        {
                            if (res.Error != null || res.Cancelled)
                                Message.Show("Erreur dans le traitement : " + res.Error.InnerException.ToString(), "Erreur");
                            else
                                if (res.Result != null)
                                {
                                    SessionObject.ListeRechercheTarif = res.Result;
                                }
                                else
                                    Message.Show("Une erreur s'est produite, veuillez consultez le journal des erreurs",
                                        "Erreur");
                            //LoadingManager.EndLoading(handler);
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    };

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void LoadAllVariableTarif()
        {

            try
            {
                if (!(SessionObject.ListeVariableTarif.Count()>0))
                {
                    TarificationServiceClient service = new TarificationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Tarification"));
                    //int handler = LoadingManager.BeginLoading("Chargement des donnée ...");
                    service.LoadAllVariableTarifAsync();
                    service.LoadAllVariableTarifCompleted += (er, res) =>
                    {
                        try
                        {
                            if (res.Error != null || res.Cancelled)
                                Message.Show("Erreur dans le traitement : " + res.Error.InnerException.ToString(), "Erreur");
                            else
                                if (res.Result != null)
                                {
                                    SessionObject.ListeVariableTarif = res.Result;

                                }
                                else
                                    Message.Show("Une erreur s'est produite, veuillez consultez le journal des erreurs",
                                        "Erreur");
                            //LoadingManager.EndLoading(handler);
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    };
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        //public void LoadAllRechercheTarif()
        //{
        //    try
        //    {
        //        if (SessionObject.ListeRechercheTarif.Count <= 0)
        //        {
                    
        //        TarificationServiceClient service = new TarificationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Tarification"));
        //        int handler = LoadingManager.BeginLoading("Chargement des donnée ...");
        //        service.LoadAllRechercheTarifAsync();
        //        service.LoadAllRechercheTarifCompleted += (er, res) =>
        //        {
        //            try
        //            {
        //                if (res.Error != null || res.Cancelled)
        //                    Message.Show("Erreur dans le traitement : " + res.Error.InnerException.ToString(), "Erreur");
        //                else
        //                    if (res.Result != null)
        //                    {
                                
        //                        SessionObject.ListeRechercheTarif = res.Result;
        //                        //InitCentre_Redev_RechTarif_ModeCalc_ModeApp();
        //                        //System.Windows.Data.PagedCollectionView view = new System.Windows.Data.PagedCollectionView(ListeRechercheTarif);
        //                        //dgListeRechercheTarif.ItemsSource = view;
        //                        //datapager.Source = view;
        //                    }
        //                    else
        //                        Message.Show("Une erreur s'est produite, veuillez consultez le journal des erreurs",
        //                            "Erreur");
        //                LoadingManager.EndLoading(handler);
        //            }
        //            catch (Exception ex)
        //            {
        //                throw ex;
        //            }
        //        };

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        private void LoadAllModeCalcule()
        {
            try
            {
                if (SessionObject.ListeModeCalcule.Count <= 0)
                {
                   
                TarificationServiceClient service = new TarificationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Tarification"));
                //int handler = LoadingManager.BeginLoading("Chargement des donnée ...");
                service.LoadAllModeCalculeAsync();
                service.LoadAllModeCalculeCompleted += (er, res) =>
                {
                    try
                    {
                        if (res.Error != null || res.Cancelled)
                            Message.Show("Erreur dans le traitement : " + res.Error.InnerException.ToString(), "Erreur");
                        else
                            if (res.Result != null)
                            {
                                
                                SessionObject.ListeModeCalcule = res.Result;
                                //InitCentre_Redev_RechTarif_ModeCalc_ModeApp();
                                //System.Windows.Data.PagedCollectionView view = new System.Windows.Data.PagedCollectionView(ListeModeCalcule);
                                //dgListeRechercheTarif.ItemsSource = view;
                                //datapager.Source = view;
                            }
                            else
                                Message.Show("Une erreur s'est produite, veuillez consultez le journal des erreurs",
                                    "Erreur");
                        //LoadingManager.EndLoading(handler);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                };

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void LoadAllModeApplicationTarif()
        {
            try
            {
                if (SessionObject.ListeModeApplicationTarif.Count <= 0)
                {
                  
                TarificationServiceClient service = new TarificationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Tarification"));
                //int handler = LoadingManager.BeginLoading("Chargement des donnée ...");
                service.LoadAllModeApplicationTarifAsync();
                service.LoadAllModeApplicationTarifCompleted += (er, res) =>
                {
                    try
                    {
                        if (res.Error != null || res.Cancelled)
                            Message.Show("Erreur dans le traitement : " + res.Error.InnerException.ToString(), "Erreur");
                        else
                            if (res.Result != null)
                            {

                                SessionObject.ListeModeApplicationTarif = res.Result;
                                //InitCentre_Redev_RechTarif_ModeCalc_ModeApp();
                                //System.Windows.Data.PagedCollectionView view = new System.Windows.Data.PagedCollectionView(ListeModeApplicationTarif);
                                //dgListeRechercheTarif.ItemsSource = view;
                                //datapager.Source = view;
                            }
                            else
                                Message.Show("Une erreur s'est produite, veuillez consultez le journal des erreurs",
                                    "Erreur");
                        //LoadingManager.EndLoading(handler);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                };

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void ChargerTauxMini(string code)
        {
            try
            {
                CaisseServiceClient service = new CaisseServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Caisse"));
                service.RetourneListeTa58Completed += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    SessionObject.TauxMinimunDemande = args.Result;
                };
                service.RetourneListeTa58Async(code);
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        private void ChargerTypeLot()
        {
            try
            {
                if (SessionObject.ListeTypeLot != null && SessionObject.ListeTypeLot.Count != 0)
                    return;
                AcceuilServiceClient service1 = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service1.RetourneTypeLotCompleted += (es, argss) =>
                {
                    if (argss != null && argss.Cancelled)
                        return;
                    SessionObject.ListeTypeLot = argss.Result;
                };
                service1.RetourneTypeLotAsync();
                service1.CloseAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        private void ChargerOrigine()
        {
            if (SessionObject.ListeOrigine != null && SessionObject.ListeOrigine.Count != 0)
                return;
            AcceuilServiceClient service = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
            service.RetourneOrigineCompleted += (es, args) =>
            {
                List<CsOrigineLot> LstOrigine = new List<CsOrigineLot>();
                if (args != null && args.Cancelled)
                    return;
                SessionObject.ListeOrigine = args.Result;
            };
            service.RetourneOrigineAsync();
            service.CloseAsync();
        }
        private void ChargerRubriqueDevis()
        {
            try
            {
                if (SessionObject.LstRubriqueDevis != null && SessionObject.LstRubriqueDevis.Count != 0)
                    return;
                AcceuilServiceClient service = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.ChargerRubriqueCompleted  += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    SessionObject.LstRubriqueDevis = args.Result;
                };
                service.ChargerRubriqueAsync ();
                service.CloseAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        private void ChargerPuissance()
        {
            try
            {
                if (SessionObject.LstPuissance != null && SessionObject.LstPuissance.Count != 0)
                    return;
                AcceuilServiceClient service = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.ChargerPuissanceSouscriteCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                   SessionObject. LstPuissance=args.Result;
                };
                service.ChargerPuissanceSouscriteAsync();
                service.CloseAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        private void ChargerTypeComptage()
        {
            try
            {
                if (SessionObject.LstTypeComptage != null && SessionObject.LstTypeComptage.Count != 0)
                    return;
                AcceuilServiceClient service = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.ChargerTypeComptageCompleted  += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    SessionObject.LstTypeComptage = args.Result;
                };
                service.ChargerTypeComptageAsync();
                service.CloseAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void ChargerForfait()
        {
            try
            {
                if (SessionObject.LstForfait != null && SessionObject.LstForfait.Count != 0)
                    return;
                AcceuilServiceClient service = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.ChargerForfaitCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;

                   SessionObject.LstForfait= args.Result;
                };
                service.ChargerForfaitAsync();
                service.CloseAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        private void ChargerTarif()
        {
            try
            {
                if (SessionObject.LstTarif != null && SessionObject.LstTarif.Count != 0)
                    return;
                AcceuilServiceClient service = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.ChargerTarifCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    SessionObject.LstTarif=args.Result;
                };
                service.ChargerTarifAsync();
                service.CloseAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void ChargerTarifPuissance()
        {
            try
            {
                if (SessionObject.LstTarifPuissance != null && SessionObject.LstTarifPuissance.Count != 0)
                    return;
                AcceuilServiceClient service = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.ChargerTarifPuissanceCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    SessionObject.LstTarifPuissance = args.Result;
                };
                service.ChargerTarifPuissanceAsync();
                service.CloseAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        private void RemplirListeActivitesExistant()
        {
            try
            {
                Galatee.Silverlight.ServiceScelles.IScelleServiceClient client = new Galatee.Silverlight.ServiceScelles.IScelleServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Scelles"));
                client.RetourneListeActiviteCompleted += (ssender, args) =>
                {
                    if (args.Cancelled || args.Error != null)
                    {
                        string error = args.Error.Message;
                        Message.ShowError(error, Galatee.Silverlight.Resources.Scelles.Languages.Quartier);
                        return;
                    }
                    if (args.Result == null)
                    {
                        Message.ShowError(Galatee.Silverlight.Resources.Scelles.Languages.msgErreurChargementDonnees, Galatee.Silverlight.Resources.Scelles.Languages.Scelles);
                        return;
                    }
                    SessionObject.LstDesActivitee  = args.Result;
                };
                client.RetourneListeActiviteAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void RemplirListFournisseursExistant()
        {
            try
            {
                Galatee.Silverlight.ServiceScelles.IScelleServiceClient client = new Galatee.Silverlight.ServiceScelles.IScelleServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Scelles"));
                client.RetourneListeFournisseursCompleted += (ssender, args) =>
                {
                    if (args.Cancelled || args.Error != null)
                    {
                        string error = args.Error.Message;
                        Message.ShowError(error, Galatee.Silverlight.Resources.Scelles.Languages.Scelles);
                        return;
                    }
                    if (args.Result == null)
                    {
                        Message.ShowError(Galatee.Silverlight.Resources.Scelles.Languages.msgErreurChargementDonnees, Galatee.Silverlight.Resources.Scelles.Languages.Scelles);
                        return;
                    }
                    SessionObject.LstDesFournisseur  = args.Result;
                };
                client.RetourneListeFournisseursAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void RemplirListeOrigineScellesExistant()
        {
            try
            {
                Galatee.Silverlight.ServiceScelles.IScelleServiceClient client = new Galatee.Silverlight.ServiceScelles.IScelleServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Scelles"));
                client.RetourneListeOrigineScelleCompleted += (ssender, args) =>
                {
                    if (args.Cancelled || args.Error != null)
                    {
                        string error = args.Error.Message;
                        Message.ShowError(error, Galatee.Silverlight.Resources.Scelles.Languages.Scelles);
                        return;
                    }
                    if (args.Result == null)
                    {
                        Message.ShowError(Galatee.Silverlight.Resources.Scelles.Languages.msgErreurChargementDonnees, Galatee.Silverlight.Resources.Scelles.Languages.Scelles);
                        return;
                    }
                    SessionObject.LstDesOrigineScelle = args.Result;
                };
                client.RetourneListeOrigineScelleAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void RemplirListeCouleurExistant()
        {
            try
            {
                Galatee.Silverlight.ServiceScelles.IScelleServiceClient client = new Galatee.Silverlight.ServiceScelles.IScelleServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Scelles"));
                client.RetourneListeAllCouleurScelleCompleted += (ssender, args) =>
                {
                    if (args.Cancelled || args.Error != null)
                    {
                        string error = args.Error.Message;
                        Message.ShowError(error, Galatee.Silverlight.Resources.Scelles.Languages.Scelles);
                        return;
                    }
                    if (args.Result == null)
                    {
                        Message.ShowError(Galatee.Silverlight.Resources.Scelles.Languages.msgErreurChargementDonnees, Galatee.Silverlight.Resources.Scelles.Languages.Scelles);
                        return;
                    }
                    SessionObject.LstDesCouleur  = args.Result;
                };
                client.RetourneListeAllCouleurScelleAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private void RemplirListeDesModelesMarqueExistant()
        {
            try
            {
                if (SessionObject.LstMarqueModele.Count != 0) return;
                Galatee.Silverlight.ServiceScelles.IScelleServiceClient client = new Galatee.Silverlight.ServiceScelles.IScelleServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Scelles"));
                client.RetourneListMarque_ModeleCompleted += (ssender, args) =>
                {
                    if (args.Cancelled || args.Error != null)
                    {
                        string error = args.Error.Message;
                        return;
                    }
                    if (args.Result == null)
                    {
                        return;
                    }
                    else
                        SessionObject.LstMarqueModele = args.Result;
                };
                client.RetourneListMarque_ModeleAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void RemplirListeDesEtatCompteursExistant()
        {
            try
            {
                if (SessionObject.LstEtatCompteur.Count != 0) return;
                Galatee.Silverlight.ServiceScelles.IScelleServiceClient client = new Galatee.Silverlight.ServiceScelles.IScelleServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Scelles"));
                client.RetourneEtatCompteurCompleted += (ssender, args) =>
                {
                    if (args.Cancelled || args.Error != null)
                    {
                        string error = args.Error.Message;
                        return;
                    }
                    if (args.Result == null)
                    {
                        return;
                    }
                    else
                        SessionObject.LstEtatCompteur = args.Result;
                };
                client.RetourneEtatCompteurAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void ChargerFrequence()
        {
            AcceuilServiceClient service = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
            service.ChargerTousFrequenceCompleted += (s, args) =>
            {
                if (args != null && args.Cancelled)
                    return;
                SessionObject.LstFrequence =args.Result;
            };
            service.ChargerTousFrequenceAsync();
            service.CloseAsync();
        }
 
        private void ChargerMois()
        {
            try
            {
                AcceuilServiceClient service = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.ChargerTousMoisCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    SessionObject.LstMois=args.Result;
                };
                service.ChargerTousMoisAsync();
                service.CloseAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        private void ChargerApplicationTaxe()
        {
            try
            {
                AcceuilServiceClient service = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.RetourneTousApplicationTaxeCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    SessionObject.LstCodeApplicationTaxe = args.Result;
                };
                service.RetourneTousApplicationTaxeAsync();
                service.CloseAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void ChargerLaListeDesCommunes()
        {
            try
            {
                AcceuilServiceClient service = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.ChargerCommuneCompleted += (s, args) =>
                {
                    if (args.Error != null && args.Cancelled)
                        return;
                    SessionObject.LstCommune=args.Result;
                };
                service.ChargerCommuneAsync ();
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        private void ChargeQuartier()
        {
            try
            {
                AcceuilServiceClient service = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.ChargerLesQartiersCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                   SessionObject. LstQuartier=args.Result;
                };
                service.ChargerLesQartiersAsync();
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        private void ChargeSecteur()
        {
            try
            {
                AcceuilServiceClient service = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.ChargerLesSecteursCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                       SessionObject.  LstSecteur= args.Result;
                };
                service.ChargerLesSecteursAsync();
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void ChargeRue()
        {
            try
            {
                AcceuilServiceClient service = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.ChargerLesRueDesSecteurCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                   SessionObject.LstRues=args.Result;
                };
                service.ChargerLesRueDesSecteurAsync();
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void ChargerTournee()
        {
            AcceuilServiceClient service = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
            service.ChargerLesTourneesCompleted += (s, args) =>
            {
                if (args != null && args.Cancelled)
                    return;
                SessionObject.LstZone = args.Result;
            };
            service.ChargerLesTourneesAsync();
            service.CloseAsync();
        }

        private void ChargerDonneeDuSite()
        {
            try
            {
                if (SessionObject.LstCentre != null && SessionObject.LstCentre.Count != 0)
                    return;
                AcceuilServiceClient service = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.ListeDesDonneesDesSiteCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                   SessionObject.LstCentre= args.Result;
                };
                service.ListeDesDonneesDesSiteAsync(false );
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void ChargerEtapeDemande()
        {
            try
            {
                AcceuilServiceClient service1 = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service1.ChargerEtapeDemandeCompleted += (sr, res) =>
                {
                    try
                    {
                        if (res != null && res.Cancelled)
                            return;
                         SessionObject. LstEtapeDemande=res.Result;
                    }
                    catch (Exception)
                    {
                    }
                };
                service1.ChargerEtapeDemandeAsync();
                service1.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;

            }

        }
        private void ChargerTypeReclamation()
        {
            try
            {
                if (SessionObject.ListeDesReclamation != null || SessionObject.ListeDesReclamation.Count()>0)
                {
                    return;
                }
                else
                {
                    Galatee.Silverlight.ServiceReclamation.ReclamationsServiceClient client = new Galatee.Silverlight.ServiceReclamation.ReclamationsServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Reclamation"));
                    client.SelectAllTypeReclamationRclCompleted += (ssender, args) =>
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
                        {
                            SessionObject.ListeDesReclamation = args.Result;
                        }
                    };
                    client.SelectAllTypeReclamationRclAsync(); 
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void ChargerTypeDemande()
        {
            try
            {
                if (SessionObject.LstTypeDemande != null && SessionObject.LstTypeDemande.Count != 0)
                    return;

                AcceuilServiceClient service1 = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service1.RetourneOptionDemandeCompleted += (sr, res) =>
                {
                    if (res != null && res.Cancelled)
                        return;
                    SessionObject.LstTypeDemande = res.Result;
                };
                service1.RetourneOptionDemandeAsync();
                service1.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;

            }


        }
        private void ChargerTypeBranchement()
        {
            try
            {
                if (SessionObject.LstTypeBranchement != null && SessionObject.LstTypeBranchement.Count != 0)
                    return;

                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.ChargerTypeBranchementCompleted += (s, args) =>
                {
                    if ((args != null && args.Cancelled) || (args.Error != null))
                        return;
                    SessionObject.LstTypeBranchement = args.Result;
                };
                service.ChargerTypeBranchementAsync();
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void ChargerListeDeProduit()
        {
            try
            {
                AcceuilServiceClient service1 = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service1.ListeDesProduitCompleted += (sr, res) =>
                {
                    if (res != null && res.Cancelled)
                        return;
                     SessionObject. ListeDesProduit=res.Result;
                };
                service1.ListeDesProduitAsync();
                service1.CloseAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void ChargerPuissanceReglageCompteur()
        {
            try
            {
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.ChargerPuissanceReglageCompteurCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    SessionObject.LstPuissanceParReglageCompteur = args.Result;
                };
                service.ChargerPuissanceReglageCompteurAsync ();
                service.CloseAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void ChargerMotifRejetCheque()
        {
            try
            {
                RecouvrementServiceClient service = new RecouvrementServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Recouvrement"));
                service.RetourneMotifChequeImpayeCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    SessionObject.LstMotifRejetsCheque = args.Result;
                };
                service.RetourneMotifChequeImpayeAsync();
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void ChargerCategorie()
        {
            try
            {
                AcceuilServiceClient service = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.RetourneCategorieCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                   SessionObject. LstCategorie=args.Result;
                };
                service.RetourneCategorieAsync();
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void ChargerFermable()
        {
            try
            {
                AcceuilServiceClient service = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.RetourneFermableCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                   SessionObject. LstFermable=args.Result;
                };
                service.RetourneFermableAsync();
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void ChargerNatureClient()
        {
            try
            {
                AcceuilServiceClient service = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.RetourneNatureCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                   SessionObject. LstNatureClient=args.Result;
                };
                service.RetourneNatureAsync();
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void ChargerTarifReglageCompteur()
        {
            try
            {
                AcceuilServiceClient service = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.ChargerTarifParReglageCompteurCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    SessionObject.LstTarifParReglageCompteur = args.Result;

                };
                service.ChargerTarifParReglageCompteurAsync();
                service.CloseAsync();
                
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        private void ChargerTarifParCategorie()
        {
            try
            {
                AcceuilServiceClient service = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.ChargerTarifParCategorieCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    SessionObject.LstTarifCategorie = args.Result;

                };
                service.ChargerTarifParCategorieAsync();
                service.CloseAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        private void ChargerCivilite()
        {
            try
            {
                AcceuilServiceClient service = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.RetourneListeDenominationAllCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                   SessionObject. LstCivilite = args.Result;
                };
                service.RetourneListeDenominationAllAsync();
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void ChargerNationnalite()
        {
            try
            {
                AcceuilServiceClient service = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.RetourneNationnaliteCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                   SessionObject. LstDesNationalites=args.Result;
                };
                service.RetourneNationnaliteAsync();
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void ChargerPuissanceInstalle()
        {
            try
            {
                if (SessionObject.LstPuissanceInstalle != null && SessionObject.LstPuissanceInstalle.Count != 0)
                    return;
                AcceuilServiceClient service = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.ChargerPuissanceInstalleCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    SessionObject.LstPuissanceInstalle = args.Result;
                };
                service.ChargerPuissanceInstalleAsync();
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        private void ChargerCodeRegroupement()
        {
            try
            {
                if (SessionObject.LstCodeRegroupement != null && SessionObject.LstCodeRegroupement.Count != 0)
                    return;
                AcceuilServiceClient service = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.RetourneCodeRegroupementCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                         return;
                    SessionObject.LstCodeRegroupement=args.Result;
                };
                service.RetourneCodeRegroupementAsync();
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        private void ChargerCodeConsomateur()
        {
            try
            {
                if (SessionObject.LstCodeConsomateur != null && SessionObject.LstCodeConsomateur.Count != 0) return;
                AcceuilServiceClient service = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.RetourneCodeConsomateurCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    SessionObject. LstCodeConsomateur=args.Result;
                };
                service.RetourneCodeConsomateurAsync();
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        

        private void ChargerReglageCompteur()
        {
            try
            {
                AcceuilServiceClient service = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.ChargerReglageCompteurCompleted  += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    SessionObject.LstReglageCompteur  = args.Result;
                };
                service.ChargerReglageCompteurAsync();
                service.CloseAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
  
        private void ChargerCalibreCompteur()
        {
            try
            {
                AcceuilServiceClient service = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.ChargerCalibreCompteurCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    SessionObject.LstCalibreCompteur  = args.Result;
                };
                service.ChargerCalibreCompteurAsync();
                service.CloseAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        private void ChargerCadran()
        {
            try
            {
                AcceuilServiceClient service = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.RetourneToutCadranCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                  SessionObject.LstCadran = args.Result;
                };
                service.RetourneToutCadranAsync();
                service.CloseAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        private void ChargerMarque()
        {
            try
            {
                AcceuilServiceClient service = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.RetourneToutMarqueCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                   SessionObject. LstMarque = args.Result;
                };
                service.RetourneToutMarqueAsync();
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void ChargerTypeCompteur()
        {
            try
            {
                AcceuilServiceClient service = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.ChargerTypeCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                   SessionObject. LstTypeCompteur = args.Result;
                };
                service.ChargerTypeAsync();
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void ChargerListeProfil()
        {
            try
            {
                if (SessionObject.ListeDesProfils != null && SessionObject.ListeDesProfils.Count != 0)
                    return;
                AdministrationServiceClient admClient = new AdministrationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Administration"));
                admClient.RetourneListeAllProfilUserCompleted += (sen, result) =>
                {
                    if (result.Cancelled || result.Error != null)
                    {
                        string error = result.Error.Message;
                        Message.Show(Galatee.Silverlight.Resources.Langue.wcf_error, "GetAllRole");
                        return;
                    }

                    if (result.Result != null && result.Result.Count > 0)
                        SessionObject.ListeDesProfils = result.Result;
                };
                admClient.RetourneListeAllProfilUserAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        private void RetouneListeDesTaxes()
        {
            AcceuilServiceClient service = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
            service.RetourneListeTaxeCompleted += (s, args) =>
            {
                if (args != null && args.Cancelled)
                    return;
               SessionObject. LstDesTaxe = args.Result;
            };
            service.RetourneListeTaxeAsync();
            service.CloseAsync();
        }

        private void RetourneParametreDistanceElectricite()
        {
            try
            {
                AcceuilServiceClient clientDevis = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                clientDevis.GetParametresDistanceCompleted += (ssender, args) =>
                {
                    try
                    {
                        if (args.Cancelled || args.Error != null)
                        {
                            string error = args.Error.Message;

                            Message.ShowError(error, Galatee.Silverlight.Resources.Devis.Languages.txtDevis);
                            return;
                        }
                        if (args.Result != null)
                        {
                            SessionObject.distanceMaxiElectricite = (decimal)args.Result.Maxi;
                            SessionObject.seuilDistanceElectricite = (decimal)args.Result.Seuil;

                            if (args.Result.MaxiSubvention != null)
                                SessionObject.distanceMaxiSubventionElectricite = (decimal)args.Result.MaxiSubvention;
                        }
                    }
                    catch (Exception ex)
                    {
                        Message.ShowError(ex.Message, Galatee.Silverlight.Resources.Devis.Languages.txtDevis);
                    }
                };
                clientDevis.GetParametresDistanceAsync(DataReferenceManager.ParametreDistanceMaxiElectricite, DataReferenceManager.ParametreSeuilDistanceAdditionnelleElectricite, string.Empty);
            }
            catch (Exception ex)
            {
                throw;
            }
        
        }

       private void RetourneParametreDistanceEau()
        {
            try
            {
                AcceuilServiceClient clientDevis = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                clientDevis.GetParametresDistanceCompleted += (ssender, args) =>
                {
                    try
                    {
                        if (args.Cancelled || args.Error != null)
                        {
                            string error = args.Error.Message;
                            Message.ShowError(error, Galatee.Silverlight.Resources.Devis.Languages.txtDevis);
                            return;
                        }
                        if (args.Result != null)
                        {
                            SessionObject.distanceMaxiEau = (decimal)args.Result.Maxi;
                            SessionObject.seuilDistanceEau = (decimal)args.Result.Seuil;
                            if (args.Result.MaxiSubvention != null)
                                SessionObject.distanceMaxiSubventionElectricite = (decimal)args.Result.MaxiSubvention;
                        }
                    }
                    catch (Exception ex)
                    {
                        Message.ShowError(ex.Message, Galatee.Silverlight.Resources.Devis.Languages.txtDevis);
                    }
                };
                clientDevis.GetParametresDistanceAsync(DataReferenceManager.ParametreDistanceMaxiEau, DataReferenceManager.ParametreSeuilDistanceAdditionnelleEau, DataReferenceManager.ParametreDistanceMaxiEauSubvention);
            }
            catch (Exception ex)
            {
                
                throw;
            }
        
        }


        private void ChargerCoutDemande()
        {
            AcceuilServiceClient service = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
            service.ChargerCoutDemandeCompleted += (s, args) =>
            {
                if (args != null && args.Cancelled)
                    return;
                SessionObject.LstDesCoutDemande = args.Result;
            };
            service.ChargerCoutDemandeAsync();
            service.CloseAsync();
        }
        private void RetourneListeDesCas()
        {
            AcceuilServiceClient service = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
            service.RetourneListeDesCasCompleted += (s, args) =>
            {
                if (args != null && args.Cancelled)
                    return;
                SessionObject. LstDesCas = args.Result;
            };
            service.RetourneListeDesCasAsync ();
            service.CloseAsync();
        }
        private void ChargerListeDesPostesSource()
        {
            if (SessionObject.LsDesPosteElectriquesSource != null && SessionObject.LsDesPosteElectriquesSource.Count != 0)
            {
                return;
            }
            AcceuilServiceClient service = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
            service.ChargerPosteSourceCompleted += (s, args) =>
            {
                if (args != null && args.Cancelled)
                    return;

                if (args.Result != null)
                {
                    SessionObject.LsDesPosteElectriquesSource = args.Result;
                }
            };
            service.ChargerPosteSourceAsync();
            service.CloseAsync();
        }
        private void RetourneListeDesDepartHta()
        {
            if (SessionObject.LsDesDepartHTA != null && SessionObject.LsDesDepartHTA.Count != 0)
            {
                return;
            }
            AcceuilServiceClient service = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
            service.ChargerDepartHTACompleted  += (s, args) =>
            {
                if (args != null && args.Cancelled)
                    return;
                if (args.Result != null)
                {
                    SessionObject.LsDesDepartHTA = args.Result;
                }
            };
            service.ChargerDepartHTAAsync();
            service.CloseAsync();
        }
        private void ChargerListeDesPostesTransformation()
        {
            if (SessionObject.LsDesPosteElectriquesTransformateur != null && SessionObject.LsDesPosteElectriquesTransformateur.Count != 0)
            {
                return;
            }
            Galatee.Silverlight.ServiceParametrage.ParametrageClient service = new ServiceParametrage.ParametrageClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Parametrage"));
            service.SelectAllPosteTransformationCompleted += (s, args) =>
            {
                if (args != null && args.Cancelled)
                    return;
                if (args.Result != null)
                {
                    CsPosteTransformation poste;
                    SessionObject.LsDesPosteElectriquesTransformateur.Clear();
                    foreach (var item in args.Result)
                    {
                        poste = new CsPosteTransformation();
                        poste.CODE = item.CODE;
                        poste.CODEDEPARTHTA = item.CODEDEPARTHTA;
                        poste.DATECREATION = item.DATECREATION;
                        poste.DATEMODIFICATION = item.DATEMODIFICATION;
                        poste.FK_IDDEPARTHTA = item.FK_IDDEPARTHTA;
                        poste.LIBELLE = item.LIBELLE;
                        poste.LIBELLEDEPARTHTA = item.LIBELLEDEPARTHTA;
                        poste.OriginalCODE = item.OriginalCODE;
                        poste.PK_ID = item.PK_ID;
                        poste.USERCREATION = item.USERCREATION;
                        poste.USERMODIFICATION = item.USERMODIFICATION;
                        SessionObject.LsDesPosteElectriquesTransformateur.Add(poste);
                    }
                }
            };
            service.SelectAllPosteTransformationAsync();
            service.CloseAsync();
        }
        //private void RetourneListeDesDepartBT()
        //{
        //    AcceuilServiceClient service = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
        //    service.ChargerDepartBTCompleted += (s, args) =>
        //    {
        //        if (args != null && args.Cancelled)
        //            return;
        //        SessionObject.LsDesDepartBT = args.Result;
        //    };
        //    service.ChargerDepartBTAsync();
        //    service.CloseAsync();
        //}
        private void RecuperationListBanque()
        {
            if (SessionObject.ListeBanques == null)
            {
                int loaderHandler = LoadingManager.BeginLoading(Langue.Data_Loading);
                CaisseServiceClient srv = new CaisseServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Caisse"));
                srv.RetourneListeDesBanquesCompleted += (ss, ee) =>
                {
                    try
                    {
                        if (ee.Cancelled || ee.Error != null || ee.Result == null)
                        {
                            string error = ee.Error.InnerException.ToString();
                            return;
                        }

                        //Assignation de la variable de session contenant la liste des banques
                        SessionObject.ListeBanques = ee.Result;
                        if (SessionObject.ListeBanques == null || SessionObject.ListeBanques.Count == 0)
                        {
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        Message.ShowError(ex, Galatee.Silverlight.Resources.Caisse.Langue.errorTitle);
                    }
                    finally
                    {
                        LoadingManager.EndLoading(loaderHandler);
                    }
                };
                srv.RetourneListeDesBanquesAsync();
            }
        }
        private void RetourneImprimante()
        {
            int loaderHandler = LoadingManager.BeginLoading(Langue.Data_Loading);
            // obtenir imprimante par defaut de la caissiere
            AuthentInitializeServiceClient auth = new AuthentInitializeServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Initialisation"));
            //AuthentInitializeServiceClient auth = new AuthentInitializeServiceClient(Utility.Protocole(), Utility.EndPointCaisse());

            auth.getDefaultPrinterCompleted += (_, print) =>
            {
                try
                {
                    if (print.Cancelled || print.Error != null)
                    {
                        string error = print.Error.Message;
                        Message.Show(error,"getDefaultPrinter=>" + Galatee.Silverlight.Resources.Langue.informationTitle);
                        return;
                    }

                    if (print.Result == null)
                    {
                        Message.Show(print.Error.Message, "getDefaultPrinter=>" + Galatee.Silverlight.Resources.Langue.errorTitle);
                        return;
                    }

                    //SessionObject.DefaultPrinter = "2013";
                    SessionObject.DefaultPrinter = print.Result;
                }
                catch (Exception ex)
                {
                    Message.Show(ex.Message, Galatee.Silverlight.Resources.Langue.errorTitle + "getDefaultPrinter");
                }
                finally
                {
                    LoadingManager.EndLoading(loaderHandler);
                }
            };
            auth.getDefaultPrinterAsync();
        
        }
        private void RetourneLstFraixTimbre()
        {
            int loaderHandler = LoadingManager.BeginLoading(Langue.Data_Loading);
            System.ServiceModel.EndpointAddress ServerUrl = null;
            //if (ServerMode == SessionObject.Enumere.EtatServerEnLigne)
            if (ServerMode =="Online")
                ServerUrl = Utility.EndPoint("Caisse");
            else ServerUrl = SessionObject.LocalServiceCaisse;
                CaisseServiceClient cais = new CaisseServiceClient(Utility.Protocole(), ServerUrl);
                cais.RetourneListeTimbreCompleted += (send, aa) =>
                {
                    try
                    {
                        if (aa.Cancelled || aa.Error != null)
                        {
                            Message.Show(Galatee.Silverlight.Resources.Langue.wcf_error, Galatee.Silverlight.Resources.Langue.errorTitle);
                            return;
                        }
                        if (aa.Result == null || aa.Result.Count == 0)
                        {
                            Message.Show(Galatee.Silverlight.Resources.Langue.msgNodata,"RetourneListeTimbre =>" + Galatee.Silverlight.Resources.Langue.informationTitle);
                            return;
                        }
                        SessionObject.LstFraisTimbre = aa.Result;
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }        
                    finally
                    {
                        LoadingManager.EndLoading(loaderHandler);
                    }
                    
                };
                cais.RetourneListeTimbreAsync(ServerMode );

        }
        private void ChargerCoper()
        {
            AcceuilServiceClient service = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
            service.RetourneTousCoperCompleted   += (s, args) =>
            {
                if (args != null && args.Cancelled)
                    return;
                SessionObject.LstDesCopers = args.Result;
            };
            service.RetourneTousCoperAsync ();
            service.CloseAsync();
        }
        private void RecupereNumeroCaisse()
        {
            if (SessionObject.LePosteCourant.FK_IDCAISSE == null)
            {
                Message.ShowInformation(Silverlight.Resources.Administration.Langue.MsgNonPosteCaisse, Silverlight.Resources.Administration.Langue.libModule);
                return;
            }
            //UserConnecte.Centre = Silverlight.Classes.IsolatedStorage.getCentre ();
            CaisseServiceClient srv = new CaisseServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Caisse"));
            srv.RetourneCaisseEnCoursAsync(SessionObject.LePosteCourant.FK_IDCAISSE.Value, UserConnecte.PK_ID, System.DateTime.Today.Date);
            srv.RetourneCaisseEnCoursCompleted += (ss, ee) =>
            {
                try
                {
                    if (ee.Cancelled || ee.Error != null)
                    {
                        string error = ee.Error.InnerException.ToString();
                        return;
                    }
                    //if (ee.Result == null)
                    if (ee.Result == null && SessionObject.EtatCaisse == SessionObject.Enumere.EtatDeCaisseValider)
                        HabiliterCaisse();
                    else if (ee.Result != null)
                    {
                        if (ee.Result.NUMCAISSE != SessionObject.LePosteCourant.NUMCAISSE)
                        {
                            Message.ShowWarning("Vous avez ouvert une caisse sur le poste " + ee.Result.POSTE + "\n\r" + "Veuillez la clôturer", "Caisse");
                            return;
                        }
                        //Assignation de la variable de session contenant la liste des banques
                        SessionObject.LaCaisseCourante = ee.Result;
                        SessionObject.DernierNumeroDeRecu = Convert.ToDecimal(SessionObject.LaCaisseCourante.ACQUIT);
                    }
                }
                catch (Exception ex)
                {
                    Message.ShowError(ex, Galatee.Silverlight.Resources.Caisse.Langue.errorTitle);
                }
            };
        }
        private void AttributionCaisse()
        {
            if (SessionObject.LePosteCourant.FK_IDCAISSE == null)
            {
                Message.ShowInformation(Silverlight.Resources.Administration.Langue.MsgNonPosteCaisse, Silverlight.Resources.Administration.Langue.libModule);
                return;
            }
            //UserConnecte.Centre = Silverlight.Classes.IsolatedStorage.getCentre ();
            CaisseServiceClient srv = new CaisseServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Caisse"));
            srv.RetourneCaisseEnCoursAsync(SessionObject.LePosteCourant.FK_IDCAISSE.Value, UserConnecte.PK_ID, System.DateTime.Today.Date );
            srv.RetourneCaisseEnCoursCompleted += (ss, ee) =>
            {
                try
                {
                    if (ee.Cancelled || ee.Error != null)
                    {
                        string error = ee.Error.InnerException.ToString();
                        return;
                    }
                    if (ee.Result == null || ee.Result .PK_ID ==0)
                        HabiliterCaisse();
                    else
                    {
                        if (ee.Result.NUMCAISSE != SessionObject.LePosteCourant.NUMCAISSE)
                        {
                            Message.ShowWarning("Vous avez ouvert une caisse sur le poste " + ee.Result.POSTE + "\n\r" + "Veuillez la clôturer", "Caisse");
                            return;
                        }
                        //Assignation de la variable de session contenant la liste des banques
                        SessionObject.LaCaisseCourante = ee.Result;
                        SessionObject.DernierNumeroDeRecu = Convert.ToDecimal(SessionObject.LaCaisseCourante.ACQUIT);
                    }
                    MenuViewModel viewModel = new MenuViewModel(_mainPage, dico, "Galatee.Silverlight." + Module, Module);
                    viewModelG = viewModel;

                    if (!IsRefreh)
                    {
                        _mainPage.DataContext = viewModelG;

                        IsRefreh = true;
                    }
                }
                catch (Exception ex)
                {
                    Message.ShowError(ex, Galatee.Silverlight.Resources.Caisse.Langue.errorTitle);
                }
            };
        }
        private void HabiliterCaisse()
        {
            try
            {
                CsHabilitationCaisse habilitationCaisse = new CsHabilitationCaisse
                {
                    CENTRE = SessionObject.LePosteCourant.CODECENTRE,
                    DATE_DEBUT = DateTime.Now,
                    DATECREATION  = DateTime.Now ,
                    FK_IDCENTRE = SessionObject.LePosteCourant.FK_IDCENTRE.Value,
                    FK_IDCAISSE = SessionObject.LePosteCourant.FK_IDCAISSE.Value,
                    MATRICULE = UserConnecte.matricule,
                    NUMCAISSE = SessionObject.LePosteCourant.NUMCAISSE,
                    POSTE = SessionObject.LePosteCourant.NOMPOSTE,
                    FK_IDCAISSIERE = UserConnecte.PK_ID
                };

                Galatee.Silverlight.ServiceCaisse.CaisseServiceClient service = new Galatee.Silverlight.ServiceCaisse.CaisseServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Caisse"));
                service.HabiliterCaisseCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;

                    if (args.Result == null)
                        Message.ShowError("Impossible d'identifier le poste", Galatee.Silverlight.Resources.Caisse.Langue.errorTitle);

                    if (args.Result != null)
                        SessionObject.LaCaisseCourante = args.Result;
                };
                service.HabiliterCaisseAsync(habilitationCaisse);
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void RetournePosteConnecter()
        {

            //Obtenir les donnees de l'arborescence des modules , programmes et des menus relatifs
            Galatee.Silverlight.ServiceAdministration.AdministrationServiceClient prgram = new Galatee.Silverlight.ServiceAdministration.AdministrationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Administration"));
            prgram.RetourneListePosteCompleted += (sprog, resprog) =>
            {
                try
                {
                    if (resprog.Cancelled || resprog.Error != null)
                    {
                        string error = resprog.Error.Message;
                    }
                    if (resprog.Result == null || resprog.Result.Count == 0)
                    {
                        Message.ShowError("Aucun poste déclaré dans la base", "RetournePoste");
                        return;
                    }
                    else
                    {
                        SessionObject.ListePoste = resprog.Result;
                        CsInfoServiceLocal objectLocal = new CsInfoServiceLocal();
                        objectLocal.AdresseMachineLocal = Silverlight.Classes.IsolatedStorage.getMachine();
                        if (!string.IsNullOrEmpty(objectLocal.AdresseMachineLocal))
                        {
                            SessionObject.LePosteCourant = SessionObject.ListePoste.FirstOrDefault(p => p.NOMPOSTE == objectLocal.AdresseMachineLocal);
                            if (SessionObject.LePosteCourant.FK_IDCAISSE == null || SessionObject.LePosteCourant.FK_IDCAISSE == 0)
                                SessionObject.PosteNonCaisse = false;
                        }
                        else /** ZEG 29/08/2017 **/
                        {
                            Message.ShowError("Impossible d'identifier le poste. Veuillez (re)déclarer ce poste", "Identification poste");
                            return;
                        }
                        /** **/

                        objectLocal.AdresseMachineLocal = SessionObject.LePosteCourant.NOMPOSTE;
                        SessionObject.MachineName = SessionObject.LePosteCourant.NOMPOSTE;
                        //SessionObject.CheminImpression = "[[" + SessionObject.LePosteCourant.NOMPOSTE + "[" + "Impression";
                        SessionObject.CheminImpression = "[[" + SessionObject.LePosteCourant.NOMPOSTE + "[" + "Impression";
                        SessionObject.CheminDocumentScanne = "\\\\" + SessionObject.LePosteCourant.NOMPOSTE + "\\" + "DocumentScane";

                        
                    }
                }
                catch (Exception ex)
                {
                    Message.ShowError(ex.Message, "RetournePoste");
                }

            };
            prgram.RetourneListePosteAsync();
        }
        private void RetourneListeDesModReglement()
        {
            CaisseServiceClient service = new CaisseServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Caisse"));
            service.RetourneModesReglementCompleted += (s, args) =>
            {
                if (args != null && args.Cancelled)
                    return;
                SessionObject.ListeModesReglement = args.Result;
            };
            service.RetourneModesReglementAsync();
            service.CloseAsync();
        }
        private void RetourneListeDesLibelleTop()
        {
            CaisseServiceClient service = new CaisseServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Caisse"));
            service.RetourneTousLibelleTopCompleted += (s, args) =>
            {
                if (args != null && args.Cancelled)
                    return;
                SessionObject.LstDesLibelleTop  = args.Result;
            };
            service.RetourneTousLibelleTopAsync ();
            service.CloseAsync();
        }

        private void RetourneListeDenominationAll()
        {
            Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
            service.RetourneListeDenominationAllCompleted += (s, args) =>
            {
                if (args != null && args.Cancelled)
                    return;
                SessionObject.LstCivilite = args.Result;
            };
            service.RetourneListeDenominationAllAsync();
            service.CloseAsync();
        }
        private void RetourneListeDesNature()
        {
            CaisseServiceClient service = new CaisseServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Caisse"));
            service.RetourneNatureCompleted  += (s, args) =>
            {
                if (args != null && args.Cancelled)
                    return;
                //SessionObject.LstDesNature  = args.Result;
            };
            service.RetourneNatureAsync();
            service.CloseAsync();
        }
        private void RetourneTousUtlisateur()
        {
            try
            {

       

                Galatee.Silverlight.ServiceAdministration.AdministrationServiceClient client = new Galatee.Silverlight.ServiceAdministration.AdministrationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Administration"));
                client.RetourneListeAllUserCompleted += (ss, res) =>
                {
                    if (res.Cancelled || res.Error != null)
                    {
                        string error = res.Error.Message;
                        Message.ShowError(error, Galatee.Silverlight.Resources.Langue.errorTitle);
                        return;
                    }

                    if (res.Result == null || res.Result.Count == 0)
                    {
                        Message.ShowInformation(Galatee.Silverlight.Resources.Langue.msgNodata, Galatee.Silverlight.Resources.Langue.informationTitle + "  RetourneTousUtlisateur");
                        return;
                    }
                    SessionObject.ListeDesUtilisateurs = res.Result;
                };
                client.RetourneListeAllUserAsync();

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        private void RetourneListeDesAgents()
        {
            AdministrationServiceClient SiteClient = new AdministrationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Administration"));
            SiteClient.ChargeListeDesAgentsCompleted += (scentre, rcentre) =>
            {
                try
                {
                    if (rcentre.Cancelled || rcentre.Error != null)
                    {
                        string error = rcentre.Error.Message;
                        Message.Show(Galatee.Silverlight.Resources.Langue.wcf_error, "ChargeListeDesAgents");
                        return;
                    }
                    SessionObject.ListeDesAgents = rcentre.Result;
                    //List<CsAgent> lstAgent = rcentre.Result;
                    //this.agentListe = lstAgent;
                    //this.Cbo_ListeAgent.ItemsSource = null;
                    //this.Cbo_ListeAgent.ItemsSource = lstAgent;
                    //this.Cbo_ListeAgent.Tag = lstAgent;
                    //this.Cbo_ListeAgent.DisplayMemberPath  = "NOM";
                }
                catch (Exception ex)
                {
                    Message.ShowError(ex.Message, Galatee.Silverlight.Resources.Langue.errorTitle);
                }
            };
            SiteClient.ChargeListeDesAgentsAsync();
        }

        private void galatee_CaisseOK(object sender, EventArgs e)
        {

                            // ouverture de la fenetre d'authentification
                Galatee.Silverlight.Caisse.FrmCreeCaisse ctrl = sender as Galatee.Silverlight.Caisse.FrmCreeCaisse;
                if (ctrl.Cbo_ListeCaisse.SelectedItem != null)
                {
                    try
                    {
                        CsCaisse laCaisse = (CsCaisse)ctrl.Cbo_ListeCaisse.SelectedItem;
                        UserConnecte.numcaisse = laCaisse.NUMCAISSE;
                        SessionObject.DernierNumeroDeRecu = Convert.ToDecimal(laCaisse.ACQUIT);
                        RefreshMenuBar();
                        MenuViewModel viewModel = new MenuViewModel(_mainPage, dico, "Galatee.Silverlight." + Module, Module);
                        viewModelG = viewModel;

                        if (!IsRefreh)
                        {
                            _mainPage.DataContext = viewModelG;

                            IsRefreh = true;
                        }
                    }
                    catch (Exception ex)
                    {

                        string error = ex.Message;
                    }
                }
                else
                {
                    Galatee.Silverlight.Caisse.FrmCreeCaisse ctrls = new Galatee.Silverlight.Caisse.FrmCreeCaisse();
                    ctrls.Closed += new EventHandler(galatee_CaisseOK);
                    ctrls.Show();
                }

        }

        private void VerifierEtatCaisse()
        {
            int loaderHandler = LoadingManager.BeginLoading(Langue.Data_Loading);
            if (SessionObject.LePosteCourant.FK_IDCAISSE == null || SessionObject.LePosteCourant.FK_IDCAISSE == 0)
            {
                SessionObject.PosteNonCaisse = true;
                RefreshMenuBar();
                MenuViewModel viewModel = new MenuViewModel(_mainPage, dico, "Galatee.Silverlight." + Module, Module);
                viewModelG = viewModel;

                if (!IsRefreh)
                {
                    _mainPage.DataContext = viewModelG;

                    IsRefreh = true;
                }
                LoadingManager.EndLoading(loaderHandler);
                return;
            }
            if (SessionObject.LePosteCourant.FK_IDCAISSE != null)
            {
                CaisseServiceClient clt = new CaisseServiceClient(Utility.Protocole(), Utility.EndPoint("Caisse"));
                clt.VerifieEtatCaisseCompleted += (xx, args) =>
                {
                    try
                    {
                        if (args.Cancelled || args.Error != null)
                        {
                            Message.ShowError("Error occurs while processing request !", "VerifieEtatCaisse");
                            return;
                        }
                        EtatCaisse = args.Result;
                        SessionObject.EtatCaisse = EtatCaisse;

                        if (EtatCaisse == SessionObject.Enumere.EtatDeCaisseDejaCloture)
                        {
                            Message.ShowInformation(Galatee.Silverlight.Resources.Caisse.Langue.MsgCaisseDejaFerme, Galatee.Silverlight.Resources.Caisse.Langue.LibelleModule);
                            return;
                        }
                        else if (EtatCaisse == SessionObject.Enumere.EtatDeCaisseNonCloture)
                        {
                            RecupereNumeroCaisse();
                            Message.ShowInformation(Galatee.Silverlight.Resources.Caisse.Langue.MsgCaisseNonCloture, Galatee.Silverlight.Resources.Caisse.Langue.LibelleModule);
                            MenuViewModel viewModel = new MenuViewModel(_mainPage, dico, "Galatee.Silverlight." + Module, Module);
                            viewModelG = viewModel;

                            if (!IsRefreh)
                            {
                                _mainPage.DataContext = viewModelG;

                                IsRefreh = true;
                            }

                            return;
                        }
                        else if (EtatCaisse == SessionObject.Enumere.EtatDeCaisseAutreSessionOuvert)
                        {
                            Message.ShowWarning("Vous avez ouvert une caisse sur un autre poste " + "\n\r" + "Veuillez la clôturer", "Caisse");
                            return;
                        }
                        else if (EtatCaisse == SessionObject.Enumere.EtatDeCaisseValider)
                            AttributionCaisse();
                    }
                    catch (Exception ex)
                    {
                        Message.ShowError(ex.Message, Galatee.Silverlight.Resources.Caisse.Langue.LibelleModule + "VerifieEtatCaisse");
                    }
                    finally
                    {
                        LoadingManager.EndLoading(loaderHandler);
                    }
                };
                clt.VerifieEtatCaisseAsync(UserConnecte.matricule, SessionObject.LePosteCourant.FK_IDCAISSE.Value);
            }
        }
        private void ChargerDiametreCompteurDevis()
        {
            try
            {
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.ChargerDiamentreBranchementCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    SessionObject.LstDiametreDevis = args.Result;
                };
                service.ChargerDiamentreBranchementAsync();
                service.CloseAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
 
        private void ChargerMarqueDevis()
        {
            try
            {
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.RetourneToutMarqueCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    SessionObject.LstMarque = args.Result;
                };
                service.RetourneToutMarqueAsync();
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void ChargerTypeCompteurDevis()
        {
            try
            {
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.ChargerTypeCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    SessionObject.LstTypeCompteur = args.Result;
                };
                service.ChargerTypeAsync();
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
