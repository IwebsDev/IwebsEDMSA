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
//using Galatee.Silverlight.serviceWeb;
using Galatee.Silverlight.ServiceAuthenInitialize;
using Galatee.Silverlight.Caisse;
using Galatee.Silverlight.MainView;
using System.Windows.Interop;
using System.Windows.Browser;
using System.Reflection;
using Galatee.Silverlight.ServiceRecouvrement;
using System.Windows.Media.Imaging;
using System.IO;
using System.Xml.Linq;
using System.Reflection.Emit;
using System.Threading;
using System.IO.IsolatedStorage;
using System.Xml.Serialization;
using System.Windows.Messaging;
using System.ComponentModel;
using System.Collections.ObjectModel;
using Galatee.Silverlight.Resources.Administration;
using Galatee.Silverlight.ServiceParametrage;
using Galatee.Silverlight.Resources.Parametrage;


namespace Galatee.Silverlight
{
    [ScriptableType()]
    public partial class FrmMain : Page
    {
        XmlSerializer serializerDesktopGroup = new XmlSerializer(typeof(ServiceAuthenInitialize.CsDesktopGroup));
        XmlSerializer serializerConnectedUser = new XmlSerializer(typeof(ServiceAuthenInitialize.CsUserConnecte));
        XmlSerializer serializerSessionEnumereWrap = new XmlSerializer(typeof(ServiceAuthenInitialize.EnumereWrap));
        XmlSerializer serializerSessionEnumereProc = new XmlSerializer(typeof(ServiceAuthenInitialize.EnumProcedureStockee));
        XmlSerializer serializerSessionEnumereSecurite = new XmlSerializer(typeof(ServiceAuthenInitialize.CsStrategieSecurite));
        //XmlSerializer serializerOpenDay = new XmlSerializer(typeof(ServiceCaisse.CsOpenningDay));
        public static FrmMain MainView = new FrmMain(true);
        static int index = 20;
        string streamOfDiv = string.Empty;
        bool passage = false;
        string moduleSend = string.Empty;
        static string currentModule = string.Empty;
        int passages = 0;
        bool FromModule = false;
        private static ObservableCollection<BusyIndicator> listeDeChargement = new ObservableCollection<BusyIndicator>();




        string EtatLicenceExpire = "Expire";
        string EtatLicenceValide = "Valide";
        string EtatLicenceNonTrouve = "NonTrouve";
        string EtatConnexionGaladbNonTrouve = "GALADBTrouve";
        string EtatConnexionAbo07NonTrouve = "ABO07Trouve";
        string EtatConnexionAucunTrouve = "AucunTrouve";
  
        

        public static ObservableCollection<BusyIndicator> ListeDeChargement
        {
            get { return FrmMain.listeDeChargement; }
            set { FrmMain.listeDeChargement = value;  }
        }

        public FrmMain()
        {
            try
            {
                InitializeComponent();
                this.Matricule.Visibility = System.Windows.Visibility.Collapsed;
                this.Nom.Visibility = System.Windows.Visibility.Collapsed;
                //this.Fonction.Visibility = System.Windows.Visibility.Collapsed;
                this.Centre.Visibility = System.Windows.Visibility.Collapsed;
                //hyperlinkButton1.Visibility = System.Windows.Visibility.Collapsed ;

                //this.NumeroCaisse.Visibility = System.Windows.Visibility.Collapsed;
                HtmlPage.RegisterScriptableObject("Page", this);
                Random r = new Random(90899);
                LocalMessageReceiver messageReceiver = 
                   new LocalMessageReceiver("receiver",
                   ReceiverNameScope.Global, LocalMessageReceiver.AnyDomain);
                messageReceiver.MessageReceived += messageReceiver_MessageReceived;
                messageReceiver.Listen();

                // Initialisation du panel d'indicateur de chargement                
                //this.LoadingPanel.ItemsSource = null;
                this.LoadingPanel.ItemsSource = ListeDeChargement;
                Classes.IsolatedStorage.DeleteIsWorkfloGp();


            }
            catch (ListenFailedException ex)
            {
                //Message.Show(ex, Galatee.Silverlight.Resources.Langue.errorTitle + "FrmMain");
                Nom.Content = ex.Message;
                string error = "Cannot receive messages." + Environment.NewLine +
                    "There is already a receiver with the name 'receiver'.";
            }
        }

        public FrmMain(bool others)
        {
            try
            {
                InitializeComponent();
                this.Matricule.Visibility = System.Windows.Visibility.Collapsed;
                this.Nom.Visibility = System.Windows.Visibility.Collapsed;
                //hyperlinkButton1.Visibility = System.Windows.Visibility.Collapsed ;

                //this.Fonction.Visibility = System.Windows.Visibility.Collapsed;
                this.Centre.Visibility = System.Windows.Visibility.Collapsed;
                //this.NumeroCaisse.Visibility = System.Windows.Visibility.Collapsed;
                // Initialisation du panel d'indicateur de chargement                
                //this.LoadingPanel.ItemsSource = null;
                this.LoadingPanel.ItemsSource = ListeDeChargement;
            }
            catch (ListenFailedException ex)
            {
                //Message.Show(ex, Galatee.Silverlight.Resources.Langue.errorTitle + "FrmMain");
                Nom.Content = ex.Message;
                string error = "Cannot receive messages." + Environment.NewLine +
                    "There is already a receiver with the name 'receiver'.";
            }
        }

        private void messageReceiver_MessageReceived(
          object sender, MessageReceivedEventArgs e)
        {

            try
            {
                e.Response = "response to " + e.Message;
                string eror =
                    "Message: " + e.Message + Environment.NewLine +
                    "NameScope: " + e.NameScope + Environment.NewLine +
                    "ReceiverName: " + e.ReceiverName + Environment.NewLine +
                    "SenderDomain: " + e.SenderDomain + Environment.NewLine +
                    "Response: " + e.Response;


                LaunchAppFromModule(true);
                SessionObject.OpenedModules = GetOpenedModules();
                if(!SessionObject.OpenedModules.Contains(e.Message))
                    CreateModuleFrame(e.Message);
                else
                    HiddenModules(e.Message);

            }
            catch (Exception ex)
            {
                Message.Show(ex, Galatee.Silverlight.Resources.Langue.errorTitle + "messageReceiver_MessageReceived");
            }

        }

        /// <summary>
        /// Permet de rendre visible le module passe en parametre et rend invisible tout autre module
        /// </summary>
        /// <param name="modulecourant">module qui sera affché</param>
        /// <author>HGB</author>
        /// <date>18/03/2013</date>
        void HiddenModules(string modulecourant)
        {
            try
            {
                // modification apportée par HGB 05/04/2013 pour le problème d'affichage des plugings
                HtmlElement div4 = HtmlPage.Document.GetElementById("div4");
                div4.SetStyleAttribute("visibility", "hidden");

                // modif effectuée  par HGB le 24/10/2013 pour améliorer  le précédent commenté
                //HtmlElement div4 = HtmlPage.Document.GetElementById("div4");
                //div4.SetStyleAttribute("display", "none");
                SessionObject.OpenedModules = GetOpenedModules();
                //if (SessionObject.OpenedModules.Count == 0)
                //    MessageBox.Show("Auncun élément trouvé");

                foreach (string module in SessionObject.OpenedModules)
                {
                    HtmlElement moduleDiv = HtmlPage.Document.GetElementById(module);

                    if (module == modulecourant)
                        moduleDiv.SetStyleAttribute("visibility", "visible");
                    else
                        moduleDiv.SetStyleAttribute("visibility", "hidden");

                    // modif effectuée  par HGB le 24/10/2013 pour améliorer  le précédent commenté
                    //if (module == modulecourant)
                    //    moduleDiv.SetStyleAttribute("display", "block");
                    //else
                    //    moduleDiv.SetStyleAttribute("display", "none");
                 
                }
            }
            catch (Exception ex)
            {
                throw ex;
                //Message.ShowError(ex, "HiddenModules");
            }
            
        }

       public  void DrawIframePresentation(string IframeId,string URI)
        {
            try
            {
                //MinimizeSilverPage();
                // iframe id :ifHtmlContent
                System.Windows.Browser.HtmlElement myFrame = System.Windows.Browser.HtmlPage.Document.GetElementById(IframeId);
                if (myFrame != null)
                {
                    myFrame.SetStyleAttribute("width", "100%");
                    myFrame.SetStyleAttribute("height", "100%");
                    myFrame.SetStyleAttribute("left", "0");
                    myFrame.SetStyleAttribute("top", "50");
                    myFrame.SetStyleAttribute("visibility", "visible");
                    myFrame.SetAttribute("src", URI);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void MinimizeSilverPage()
        {
            try
            {
                string currentmodule = getModuleSend();
                HtmlElement myFrame = HtmlPage.Document.GetElementById(currentmodule);
                if (myFrame != null)
                {
                    myFrame.SetStyleAttribute("width", "0%");
                    myFrame.SetStyleAttribute("height", "0%");
                    myFrame.SetStyleAttribute("top", "0");
                    myFrame.SetStyleAttribute("visibility", "hidden");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            //DrawIframePresentation("IFRAME", textBox1.Text);
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                FrmSelectDateCaisse ctr = new FrmSelectDateCaisse();
                ctr.Show();
            }
            catch (Exception ex)
            {
                Message.Show(ex, Galatee.Silverlight.Resources.Langue.errorTitle + " button2_Click");
            }
        }

        void InitialiserEnumereWrap()
        {
            AuthentInitializeServiceClient services = new AuthentInitializeServiceClient(Utility.ProtocoleFacturation (), Utility.EndPoint("Initialisation"));
            services.returnEnumereWrapperCompleted += (ss, ess) =>
            {
                try
                {
                    if (ess.Cancelled || ess.Error != null)
                    {
                        Message.ShowError(Galatee.Silverlight.Resources.Langue.wcf_error + "::" + "returnEnumereWrappe", Galatee.Silverlight.Resources.Langue.errorTitle);
                        return;
                    }

                    if (ess.Result == null)
                    {
                        Message.ShowInformation(Galatee.Silverlight.Resources.Langue.msgNodata + "::" + "returnEnumereWrappe", Galatee.Silverlight.Resources.Langue.informationTitle); //new DialogResult("Wrong parameters", false).Show(); //
                        return;
                    }

                    SessionObject.Enumere = ess.Result;
                }
                catch (Exception ex)
                {
                    Message.ShowError(ex + "::" + "returnEnumereWrappe", Galatee.Silverlight.Resources.Langue.errorTitle);
                }
            };
            services.returnEnumereWrapperAsync();
        }

        void InitialiserParametreSecurite()
        {
            AuthentInitializeServiceClient securite = new AuthentInitializeServiceClient(Utility.ProtocoleFacturation (), Utility.EndPoint("Initialisation"));
            securite.GetStrategieSecuriteActifCompleted += (ssecurite, resultsecurity) =>
            {
                try
                {
                    if (resultsecurity.Cancelled || resultsecurity.Error != null)
                    {
                        Message.ShowError(Galatee.Silverlight.Resources.Langue.wcf_error + "::" + "GetStrategieSecuriteActif", Galatee.Silverlight.Resources.Langue.errorTitle);
                        return;
                    }

                    if (resultsecurity.Result == null)
                    {
                        Message.ShowInformation("Aucune stratégie de sécurité active existe dans le système.Veuillez contacter l'administrateur", Galatee.Silverlight.Resources.Langue.informationTitle); //new DialogResult("Wrong parameters", false).Show(); //
                        return;
                    }

                    SessionObject.securiteActive = resultsecurity.Result;
                }
                catch (Exception ex)
                {
                    Message.ShowError(ex , Galatee.Silverlight.Resources.Langue.errorTitle);
                }
            };
            securite.GetStrategieSecuriteActifAsync();
        }
     

        private void galatee_PosteOK(object sender, EventArgs e)
        {
             //ouverture de la fenetre d'authentification
            Galatee.Silverlight.Administration.FrmParametragePoste ctrl = sender as Galatee.Silverlight.Administration.FrmParametragePoste;
                if (ctrl.Cbo_Centre .SelectedItem != null)
                    OuvertureFenetreAuthentification(false, "");
                else
                {
                    ctrl.Closed += new EventHandler(galatee_PosteOK);
                    ctrl.Show();
                }
        }
        void InitializationDonneesApp(bool IsMsgPrompt, string msgAvertissement)
        {
            try
            {

                // Initialisation des enumerewrap
                InitialiserEnumereWrap();
                // Initialisation des paramètres de sécurité
                InitialiserParametreSecurite();
                LicenceOK(IsMsgPrompt, msgAvertissement);
                //RetournePoste(IsMsgPrompt, msgAvertissement);

                

                //ChargerCoperDemande();
                //ChargerTaxeDevis();
            }
           catch (Exception ex)
            {
              Message.ShowError(ex , Galatee.Silverlight.Resources.Langue.errorTitle + " InitializationDonneesApp");
            }
        }
        void OuvertureFenetreAuthentification(bool IsMsgPrompt, string msgAvertissement)
        {

            try
            {
                if (IsMsgPrompt)
                {
                    //var w = new MessageBoxControl.MessageBoxChildWindow(Galatee.Silverlight.Resources.Langue.informationTitle, "La license va expirée dans :" + msgAvertissement + " Jours", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    var w = new MessageBoxControl.MessageBoxChildWindow(Galatee.Silverlight.Resources.Langue.informationTitle, msgAvertissement , MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    w.OnMessageBoxClosed += (_, result) =>
                    {
                        UcConnexion connex = new UcConnexion(this);
                        connex.success += new EventHandler(ConnexionSuccess);
                        connex.Show();
                    };
                    w.Show();
                }
                else
                {
                    //hyperlinkButton1.Visibility = System.Windows.Visibility.Collapsed;
                    UcConnexion connex = new UcConnexion(this);
                    connex.success += new EventHandler(ConnexionSuccess);
                    connex.Show();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        void VerifierLicenseInferiorPeriodAlert()
        {
            try
            {
                //AuthentInitializeServiceClient license = new AuthentInitializeServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Initialisation"));
                //license.LireLicenceCompleted += (sendlic, resultlic) =>
                //{
                //    try
                //    {

                //        if (resultlic.Cancelled || resultlic.Error != null)
                //        {
                //            Message.ShowInformation(resultlic.Error.Message + "::" + "LicenseInferieurPeriodeAvertissemnt", Galatee.Silverlight.Resources.Langue.errorTitle);
                //            return;
                //        }

                //        else if (resultlic.Result.Equals(EtatConnexionGaladbNonTrouve) ||
                //               resultlic.Result.Equals(EtatConnexionAbo07NonTrouve) ||
                //            resultlic.Result.Equals(EtatConnexionAucunTrouve))
                //        {
                //            string message = string.Empty;
                //            if (resultlic.Result.Equals(EtatConnexionGaladbNonTrouve))
                //                message = Langue.msgConnextionAbo07Nontrove;
                //            else
                //                message = Langue.msgConnextionGaladbNontrove;
                //            List<string> lstConnexion = new List<string>();
                //            lstConnexion.Add(SessionObject.ConnexionGaladb);
                //            lstConnexion.Add(SessionObject.ConnexionAbo07);

                //            var w = new MessageBoxControl.MessageBoxChildWindow(Galatee.Silverlight.Resources.Langue.errorTitle, message, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                //            w.OnMessageBoxClosed += (_, result) =>
                //            {
                //                if (w.Result == MessageBoxResult.OK)
                //                {
                //                    FrmGenereChaineConnexion ctrl = new FrmGenereChaineConnexion(lstConnexion);
                //                    ctrl.Show();
                //                }
                //            };
                //            w.Show();
                //        }
                //        else if (resultlic.Result.Equals(EtatConnexionAbo07NonTrouve))
                //        {
                //            Message.ShowInformation(Langue.msgConnextionGaladbNontrove, Galatee.Silverlight.Resources.Langue.errorTitle);
                //            return;
                //        }


                //        else if (resultlic.Result == EtatLicenceValide) // si la période d'avertissement  nest pas encore atteinte
                //            InitializationDonneesApp(false, "");//OuvertureFenetreAuthentification();// InitializationDonneesApp();
                //        else if (resultlic.Result == EtatLicenceExpire) //si la license n'existe pas de la configuration du serveur
                //        {
                //            Message.ShowInformation(Langue.MsgLicenceExpire, Galatee.Silverlight.Resources.Langue.errorTitle);
                //            return;
                //        }
                //        else if (!resultlic.Result.Equals(EtatLicenceValide) && !resultlic.Result.Equals(EtatLicenceExpire) && !resultlic.Result.Equals(EtatLicenceNonTrouve))
                //        {
                //            string message = Langue.MsgLicence + resultlic.Result + "  Jour(s)" + "\r\n" + Langue.MsgLicenceSuite;
                //            InitializationDonneesApp(true, message);
                //        }
                //        else if (resultlic.Result.Equals(EtatLicenceNonTrouve))
                //        {
                //            Message.ShowInformation(Langue.MsgLicenceNonTrouve, Galatee.Silverlight.Resources.Langue.errorTitle);
                //            return;
                //        }
                //    }
                //    catch (Exception ex)
                //    {
                //        Message.ShowError(ex, Galatee.Silverlight.Resources.Langue.errorTitle);
                //    }
                //};
                //license.LireLicenceAsync();

               InitializationDonneesApp(false, "");//OuvertureFenetreAuthentification();// InitializationDonneesApp();


            }
            catch (Exception ex)
            {
                Message.ShowError(ex, Galatee.Silverlight.Resources.Langue.errorTitle + "VerifierLicenseInferiorPeriodAlert");// throw;
            }
        }

        private void FrmMain_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                //light.Begin();
                ((App)Application.Current).FrmMain = this;

                // mettre in flag pour signifier que l'authentification est deja effectuée
                //if (!EstDejaConnecte())
                if (!EstDejaConnecte() || VerifierSendModule())
                    //VerifierLicenseInferiorPeriodAlert();
                    InitializationDonneesApp(false, "");//OuvertureFenetreAuthentification();// InitializationDonneesApp();
                else
                {
                    try
                    {
                        LaunchAppFromModule(false);
                        using (IsolatedStorageFile store = IsolatedStorageFile.GetUserStoreForSite())
                        {
                            if (store.FileExists("modulesend.txt"))
                            {
                                using (FileStream stream = store.OpenFile("modulesend.txt", FileMode.Open))

                                {
                                    StreamReader reader = new StreamReader(stream);
                                    moduleSend = reader.ReadLine();
                                    reader.Close();
                                }

                                Matricule.Content = moduleSend;
                                store.DeleteFile("modulesend.txt");
                                SessionObject.ModuleEnCours = moduleSend;
                                //MessageBox.Show("Module en cours d'exec :" + SessionObject.ModuleEnCours);
                            }
                        }

                        // obtenir les infos initialiser dans le store
                        GetUserConnecteValue();
                        GetSessionObjectStore(moduleSend);
                        //int IdProfil = GetDestopGroupSelectedValue(moduleSend);
                        List<int> _iProfil = GetDestopGroupSelectedValue(moduleSend);
                        TriggerMenuView menu = new TriggerMenuView(this, this.MenuGlobal);
                        //menu.RaiseView(moduleSend, LayoutRoot);

                         //menu.RaiseView(moduleSend, item, LayoutRoot);
                        menu.RaiseView(moduleSend, _iProfil, LayoutRoot);

                        ShowUserInformations();
                    }
                    catch (Exception ex)
                    {
                        Message.Show(ex, Galatee.Silverlight.Resources.Langue.errorTitle+"FrmMain_Loaded");
                    }
                }

            }
            catch (Exception ex)
            {
                Message.Show(ex, Galatee.Silverlight.Resources.Langue.errorTitle + "FrmMain_Loaded");
            }
        }

        void storeModuleSend(string modlue)
        {
            moduleSend = modlue;
           
            try
            {
                using (IsolatedStorageFile store =
                   IsolatedStorageFile.GetUserStoreForSite())
                {
                    // The CreateFile() method creates a new file or overwrites an existing one.
                    using (IsolatedStorageFileStream stream = store.CreateFile("modulesend.txt"))
                    {
                        // Store the person details in the file.
                        StreamWriter writer = new StreamWriter(stream);
                        writer.Write(modlue);
                        writer.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Message.Show(ex, Galatee.Silverlight.Resources.Langue.errorTitle + "storeModuleSend");// throw;
            }
        }

        string  getModuleSend()
        {
            string module = string.Empty;
            try
            {
                using (IsolatedStorageFile store =
                   IsolatedStorageFile.GetUserStoreForSite())
                {
                    if (store.FileExists("modulesend.txt"))
                    {
                        using (FileStream stream = store.OpenFile("modulesend.txt", FileMode.Open))
                        {
                            StreamReader reader = new StreamReader(stream);
                            module = reader.ReadLine();
                            reader.Close();
                        }
                    }
                }

                return module;
            }
            catch (Exception ex)
            {
                Message.Show(ex, Galatee.Silverlight.Resources.Langue.errorTitle + "getModuleSend");// throw;
                return null;
            }
        }

        void storeMachine(string MachineName)
        {
            try
            {
                using (IsolatedStorageFile store = IsolatedStorageFile.GetUserStoreForSite())
                {
                    using (IsolatedStorageFileStream stream = store.CreateFile("MachineNameStore.txt"))
                    {
                        // Store the person details in the file.
                        StreamWriter writer = new StreamWriter(stream);
                        writer.Write(MachineName);
                        writer.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Message.Show(ex, Galatee.Silverlight.Resources.Langue.errorTitle + "MachineNameStore");// throw;
            }
        }

        string getMachine()
        {
            string Machine = string.Empty;
            try
            {
                using (IsolatedStorageFile store = IsolatedStorageFile.GetUserStoreForSite())
                {
                    if (store.FileExists("MachineNameStore.txt"))
                    {
                        using (FileStream stream = store.OpenFile("MachineNameStore.txt", FileMode.Open))
                        {
                            StreamReader reader = new StreamReader(stream);
                            Machine = reader.ReadLine();
                            reader.Close();
                        }
                    }
                }
                return Machine;
            }
            catch (Exception ex)
            {
                Message.Show(ex, Galatee.Silverlight.Resources.Langue.errorTitle + "getModuleSend");// throw;
                return null;
            }
        }

        void GetUserConnecteValue()
        {
            try
            {
                using (IsolatedStorageFile store = IsolatedStorageFile.GetUserStoreForSite())
                {
                    if (store.FileExists("user.user"))
                    {
                        using (FileStream stream = store.OpenFile("user.user", FileMode.Open))
                        {
                            ServiceAuthenInitialize.CsUserConnecte userc = (ServiceAuthenInitialize.CsUserConnecte)serializerConnectedUser.Deserialize(stream);
                            List<ServiceAuthenInitialize.CsProfil> lstDeProfil = new List<ServiceAuthenInitialize.CsProfil>();
                            List<Galatee.Silverlight.ServiceAuthenInitialize.CsCentreProfil > lstDeCentreProfil = new List<Galatee.Silverlight.ServiceAuthenInitialize.CsCentreProfil>();
                            UserConnecte.PK_ID = userc.PK_ID;
                            UserConnecte.Centre = userc.Centre;
                            UserConnecte.codefontion = userc.codefontion;
                            UserConnecte.matricule = userc.matricule;
                            UserConnecte.nomUtilisateur = userc.nomUtilisateur;
                            UserConnecte.numcaisse = userc.numcaisse;
                            UserConnecte.LibelleFonction = userc.LibelleFontion;
                            UserConnecte.LibelleCentre  = userc.LibelleCentre ;
                            UserConnecte.PerimetreAction = userc.PerimetreAction;
                            UserConnecte.FK_IDFONCTION = userc.FK_IDFONCTION;
                            UserConnecte.ListeDesCentreProfil  = userc.ListeDesCentreProfil  ;
                          	UserConnecte.FK_IDCENTRE = userc.FK_IDCENTRE;
                            foreach (string  item in userc.ListeDesCentreProfil)
                            {
                                ServiceAuthenInitialize.CsProfil leProfil = new ServiceAuthenInitialize.CsProfil();
                                string[] decoupe = item.Split('.');
                                leProfil.PK_ID =int.Parse( decoupe[0]);
                                leProfil.MODULE = decoupe[1];
                                leProfil.CODEFONCTION   = decoupe[3];

                                ServiceAuthenInitialize.CsProfil leProfilRech = lstDeProfil.FirstOrDefault(t => t.PK_ID == leProfil.PK_ID);
                                if (leProfilRech == null)
                                    lstDeProfil.Add(leProfil);

                               
                                Galatee.Silverlight.ServiceAuthenInitialize.CsCentreProfil  leCentre = new ServiceAuthenInitialize.CsCentreProfil();
                                leCentre.FK_IDCENTRE  = int.Parse(decoupe[2]);
                                leCentre.FK_IDPROFIL  = leProfil.PK_ID;
                                lstDeCentreProfil.Add(leCentre);
                            }
                            foreach (ServiceAuthenInitialize.CsProfil item in lstDeProfil)
                                item.LESCENTRESPROFIL = lstDeCentreProfil.Where(t => t.FK_IDPROFIL == item.PK_ID).ToList();

                            UserConnecte.listeProfilUser = lstDeProfil;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Message.Show(ex, Galatee.Silverlight.Resources.Langue.errorTitle + "GetUserConnecteValue");// throw;
            }
        }

        //int  GetDestopGroupSelectedValue(string ModuleSelect )
        List<int>  GetDestopGroupSelectedValue(string ModuleSelect )
        {
            try
            {
                //int idProfilModulSelct = 0;
                List<int> idProfilModulSelct = new List<int>() ;
                using (IsolatedStorageFile store = IsolatedStorageFile.GetUserStoreForSite())
                {
                    CsDesktopGroup lesDstEnreg = new CsDesktopGroup();
                    if (store.FileExists("desktop.desktop"))
                    {
                        using (FileStream stream = store.OpenFile("desktop.desktop", FileMode.Open))
                        {
                            ServiceAuthenInitialize.CsDesktopGroup dstGpr = (ServiceAuthenInitialize.CsDesktopGroup)serializerDesktopGroup.Deserialize(stream);

                            lesDstEnreg.NOM  = dstGpr.NOM ;
                            lesDstEnreg.LstModuleIdProfil  = dstGpr.LstModuleIdProfil ;
                        };
                        foreach (string item in lesDstEnreg.LstModuleIdProfil)
	                    {
                            CsDesktopGroup l = new CsDesktopGroup();
		                        string[] dest = item.Split('.');
                                l.NOM = dest[0];
                                string IDProfil = dest[1].Replace(';',' ');
                                l.ID = int.Parse(IDProfil.TrimEnd());
                                if (l.NOM == ModuleSelect)
                                {
                                    //idProfilModulSelct = l.ID.Value;
                                    idProfilModulSelct.Add(l.ID.Value);
                                    //break;
                                }
	                    }
                    }
                }
                return idProfilModulSelct;
            }
            catch (Exception ex)
            {
                //return 0;
                return null;
            }
        }
        /// <summary>
        /// Permet de construire la vue dans laquelle le module sera visuel
        /// </summary>
        /// <param name="messagge">le libelle du module qui sera cree dans la vue</param>
        void CreateModuleFrame(string messagge)
        {
            try
            {
                //HiddenModules(messagge);
                //storeModuleSend(messagge);

                //HtmlElement divB = HtmlPage.Document.GetElementById("black");

                //// creation de la div conteneur de la div du  plugin du module en cours et de la div de l iframe
                //HtmlElement ParentDiv = HtmlPage.Document.CreateElement("div");
                //ParentDiv.Id = messagge + "_Parent";
                //ParentDiv.SetStyleAttribute("width", "100%");
                //ParentDiv.SetStyleAttribute("height", "100%");
                //ParentDiv.SetStyleAttribute("position", "absolute");
                //ParentDiv.SetStyleAttribute("z-index", "5");
                //ParentDiv.SetStyleAttribute("top", "0px");
                //ParentDiv.SetStyleAttribute("min-height", "400px");

                //divB.AppendChild(ParentDiv);
                //// fin de la creation de la div parente

                //HtmlElement divParent = HtmlPage.Document.GetElementById(messagge + "_Parent");

                //// creation de la div conteneur du plugin du  module à creer
                //HtmlElement divElement = HtmlPage.Document.CreateElement("div");
                //divElement.Id = messagge;
                //divElement.SetStyleAttribute("width", "100%");
                //divElement.SetStyleAttribute("height", "100%");
                //divElement.SetStyleAttribute("z-index", "-10");
                //divElement.SetStyleAttribute("top", "0px");
                //divElement.SetStyleAttribute("min-height", "400px");
                //// fin de la creation de la div du plugin silverlight

                //divParent.AppendChild(divElement);

                //// Invocation du javascript pour la creation du silverlight object dans le 
                //ScriptObject script = (ScriptObject)HtmlPage.Window.GetProperty("CreatePluginMainFrame");
                //script.InvokeSelf(messagge);

                //// creation de la div conteneur de l iframe
                //HtmlElement divFrame = HtmlPage.Document.CreateElement("div");
                //divFrame.Id = messagge + "DIV_IFRAME";
                //divFrame.SetStyleAttribute("width", "0%");
                //divFrame.SetStyleAttribute("height", "0%");
                //divFrame.SetStyleAttribute("position", "absolute");
                //divFrame.SetStyleAttribute("z-index", "20");
                //divFrame.SetStyleAttribute("background", "white");
                //divFrame.SetStyleAttribute("margin-top", "-59%");
                //divFrame.SetStyleAttribute("min-height", "400px");
                //// fin de la creation de la div de l iframe

                //divParent.AppendChild(divFrame);

                //HtmlElement FrameDiv = HtmlPage.Document.GetElementById(messagge + "DIV_IFRAME");

                //// Pour la construction du Iframe du plug en courant de creation
                //// Cet element Iframe servira à afficher les apercus avant impression

                //HtmlElement frame = HtmlPage.Document.CreateElement("iframe");
                //frame.Id = messagge + "_IFRAME";
                //frame.SetStyleAttribute("margin-bottom", "-100px");
                //frame.SetStyleAttribute("width", "50%");
                ////frame.SetStyleAttribute("height", "50%");
                //frame.SetStyleAttribute("background", "red");
                //frame.SetStyleAttribute("border-style", "solid");
                //frame.SetStyleAttribute("border-width", "2px");

                //FrameDiv.AppendChild(frame);

                //SessionObject.OpenedModules.Add(messagge);
                //currentModule = messagge;


                // ajout de la modification design par bamba
                HiddenModules(messagge);
                storeModuleSend(messagge);
                SetOpenModuleItem(messagge);

                HtmlElement divB = HtmlPage.Document.GetElementById("black");

                // creation de la div conteneur de la div du  plugin du module en cours et de la div de l iframe
                HtmlElement ParentDiv = HtmlPage.Document.CreateElement("div");
                ParentDiv.Id = messagge + "_Parent";
                ParentDiv.SetStyleAttribute("width", "100%");
                ParentDiv.SetStyleAttribute("height", "100%");
                ParentDiv.SetStyleAttribute("position", "absolute");
                ParentDiv.SetStyleAttribute("z-index", "5");
                ParentDiv.SetStyleAttribute("top", "0px");
                ParentDiv.SetStyleAttribute("min-height", "400px");

                divB.AppendChild(ParentDiv);
                // fin de la creation de la div parente

                HtmlElement divParent = HtmlPage.Document.GetElementById(messagge + "_Parent");

                // creation de la div conteneur du plugin du  module à creer
                HtmlElement divElement = HtmlPage.Document.CreateElement("div");
                divElement.Id = messagge;
                divElement.SetStyleAttribute("width", "100%");
                divElement.SetStyleAttribute("height", "100%");
                divElement.SetStyleAttribute("z-index", "-10");
                divElement.SetStyleAttribute("top", "0px");
                divElement.SetStyleAttribute("min-height", "300px");
                //divElement.SetStyleAttribute("background", "black");

                // fin de la creation de la div du plugin silverlight

                divParent.AppendChild(divElement);

                // Invocation du javascript pour la creation du silverlight object dans le 
                ScriptObject script = (ScriptObject)HtmlPage.Window.GetProperty("CreatePluginMainFrame");
                script.InvokeSelf(messagge);

                // creation de la div conteneur de l iframe
                HtmlElement divFrame = HtmlPage.Document.CreateElement("div");
                divFrame.Id = messagge + "DIV_IFRAME";
                divFrame.SetStyleAttribute("width", "0%");
                divFrame.SetStyleAttribute("height", "0%");
                divFrame.SetStyleAttribute("position", "absolute");
                divFrame.SetStyleAttribute("z-index", "20");
                //divFrame.SetStyleAttribute("background", "black");
                divFrame.SetStyleAttribute("top", "0px");
                //divFrame.SetStyleAttribute("top", "30px");
                //divFrame.SetStyleAttribute("margin-top", "-800px");
                divFrame.SetStyleAttribute("min-height", "100px");
                //divFrame.SetStyleAttribute("padding-top", "40px");
                // fin de la creation de la div de l iframe

                divParent.AppendChild(divFrame);

                HtmlElement FrameDiv = HtmlPage.Document.GetElementById(messagge + "DIV_IFRAME");

                // Pour la construction du Iframe du plug en courant de creation
                // Cet element Iframe servira à afficher les apercus avant impression

                HtmlElement frame = HtmlPage.Document.CreateElement("iframe");
                frame.Id = messagge + "_IFRAME";
                // frame.SetStyleAttribute("margin-bottom", "-100px");
                frame.SetStyleAttribute("width", "100%");
                frame.SetStyleAttribute("height", "100%");
                //frame.SetStyleAttribute("background", "black");
                frame.SetStyleAttribute("border-style", "solid");
                //frame.SetStyleAttribute("border-width", "2px");

                // CREATE BUTTON CLOSE IN IFRAME
                HtmlElement btnIframe = HtmlPage.Document.CreateElement("input");
                btnIframe.Id = messagge + "_IFRAME_INPUT";
                btnIframe.SetAttribute("type", "button");
                btnIframe.SetAttribute("value", "GAL CLOSE");
                btnIframe.SetStyleAttribute("width", "5px");
                btnIframe.SetStyleAttribute("height", "3px");
                // frame.SetStyleAttribute("margin-bottom", "-100px");
                frame.AppendChild(btnIframe);
                FrameDiv.AppendChild(frame);

                //SessionObject.OpenedModules.Add(messagge);
                currentModule = messagge;
            }
            catch (Exception ex)
            {
                Message.Show(ex, Galatee.Silverlight.Resources.Langue.errorTitle + "CreateModuleFrame");
                //Message.Show(ex, Galatee.Silverlight.Resources.Langue.errorTitle);// throw;
            }
        }

        public void CloseIframe(object sender, HtmlEventArgs e)
        {
            try
            {
                Message.Show("jkjdskfj", "");
                //HtmlElement element = (HtmlElement)sender;
                string currentmodule = SessionObject.moduleCourant;
                HtmlElement myFrame = HtmlPage.Document.GetElementById(currentmodule);
                HtmlElement pgl = HtmlPage.Document.GetElementById(currentmodule + "pgl");
                HtmlElement myFrame1 = HtmlPage.Document.GetElementById(currentmodule + "_IFRAME");
                if (myFrame != null)
                {
                    //myFrame.SetStyleAttribute("width", "0%");
                    //myFrame.SetStyleAttribute("height", "0%");
                    //myFrame.SetStyleAttribute("top", "0");
                    //myFrame.SetStyleAttribute("visibility", "hidden");

                    pgl.SetStyleAttribute("width", "100%");
                    pgl.SetStyleAttribute("height", "100%");
                    pgl.SetStyleAttribute("left", "0");
                    pgl.SetStyleAttribute("top", "50");
                    pgl.SetStyleAttribute("visibility", "visible");
                }

                // HtmlElement frame = HtmlPage.Document.GetElementById(currentmodule + "_IFRAME");
                if (myFrame1 != null)
                {
                    myFrame1.SetStyleAttribute("width", "0%");
                    myFrame1.SetStyleAttribute("height", "0%");
                    myFrame1.SetStyleAttribute("left", "0");
                    myFrame1.SetStyleAttribute("top", "50");
                    myFrame1.SetStyleAttribute("visibility", "hidden");
                    myFrame1.SetAttribute("src", "http://localhost:17207/Galatee.ModuleLoaderTestPage.aspx");

                }
            }
            catch (Exception ex)
            {
                Message.Show(ex, Galatee.Silverlight.Resources.Langue.errorTitle + "CloseIframe");// throw;
            }
        }

        void GetSessionObjectStore(string module)
        {
            try
            {

                // pour les enumere
                using (IsolatedStorageFile store = IsolatedStorageFile.GetUserStoreForSite())
                {
                    // The CreateFile() method creates a new file or overwrites an existing one.
                    if (store.FileExists("session.enumerewrap"))
                    {
                        using (FileStream stream = store.OpenFile("session.enumerewrap", FileMode.Open))
                        {
                            // Store the person details in the file.
                            SessionObject.Enumere = (ServiceAuthenInitialize.EnumereWrap)serializerSessionEnumereWrap.Deserialize(stream);
                        }
                    }
                    // The CreateFile() method creates a new file or overwrites an existing one.
                    if (store.FileExists("session.enumereproc"))
                    {
                        using (FileStream stream = store.OpenFile("session.enumereproc", FileMode.Open))
                        {
                            // Store the person details in the file.
                            SessionObject.EnumereProcedureStockee = (ServiceAuthenInitialize.EnumProcedureStockee)serializerSessionEnumereProc.Deserialize(stream);
                        }
                    }

                    // imprimante 
                    //if (store.FileExists("imprimante.txt"))
                    //{
                    //    using (FileStream stream = store.OpenFile("imprimante.txt", FileMode.Open))
                    //    {
                    //        StreamReader reader = new StreamReader(stream);
                    //        string imprimante = reader.ReadLine();
                    //        SessionObject.Imprimantes = imprimante.Split(';').ToList();
                    //        reader.Close();
                    //    }
                    //}
                    // pour l'objet securtie
                    if (store.FileExists("session.securite"))
                    {
                        using (FileStream stream = store.OpenFile("session.securite", FileMode.Open))
                        {
                            // Store the person details in the file.
                            SessionObject.securiteActive = (ServiceAuthenInitialize.CsStrategieSecurite)serializerSessionEnumereSecurite.Deserialize(stream);
                        }
                    }
                    //// imprimante par defaut de la machine locale
                    //using (FileStream stream = store.OpenFile("printer.default", FileMode.Open))
                    //{
                    //    StreamReader reader = new StreamReader(stream);
                    //    SessionObject.DefaultPrinter = reader.ReadLine();
                    //    reader.Close();
                    //}

                    //// nom de la machine de la caissiere 
                    //using (FileStream stream = store.OpenFile("machine.name", FileMode.Open))
                    //{
                    //    StreamReader reader = new StreamReader(stream);
                    //    SessionObject.MachineName = reader.ReadLine();
                    //    reader.Close();
                    //}
                }

            }
            catch (Exception ex)
            {
                Message.Show(ex, Galatee.Silverlight.Resources.Langue.errorTitle + "GetSessionObjectStore");// throw;
            }
        }

        void CreateSessionObjectStore()
        {
            try
            {
                
                // pour les enumere
                using (IsolatedStorageFile store = IsolatedStorageFile.GetUserStoreForSite())
                {
                    // The CreateFile() method creates a new file or overwrites an existing one.
                    using (FileStream stream = store.CreateFile("session.enumerewrap"))
                    {
                        // Store the person details in the file.
                        serializerSessionEnumereWrap.Serialize(stream, SessionObject.Enumere);
                    }
                }

                // pour les enumreprocedure
                using (IsolatedStorageFile store = IsolatedStorageFile.GetUserStoreForSite())
                {
                    // The CreateFile() method creates a new file or overwrites an existing one.
                    using (FileStream stream = store.CreateFile("session.enumereproc"))
                    {
                        // Store the person details in the file.
                        serializerSessionEnumereProc.Serialize(stream, SessionObject.EnumereProcedureStockee);
                    }
                }

                //pour l'object Opennigay
                using (IsolatedStorageFile store = IsolatedStorageFile.GetUserStoreForSite())
                {
                    // The CreateFile() method creates a new file or overwrites an existing one.
                    //using (FileStream stream = store.CreateFile("session.openday"))
                    //{
                    //    // Store the person details in the file.
                    //    serializerOpenDay.Serialize(stream, SessionObject.IsCaisseOuverte );
                    //}

                    // pour les imprimantes
                    // The CreateFile() method creates a new file or overwrites an existing one.
                    //using (IsolatedStorageFileStream stream = store.CreateFile("imprimante.txt"))
                    //{
                    //    string chaine = string.Empty;
                    //    string separateur = ";";
                    //    foreach(string impriamnte in SessionObject.Imprimantes)
                    //    {
                    //        chaine += impriamnte + separateur;
                    //    }
                    //    chaine = chaine.Substring(0, chaine.LastIndexOf(";"));
                    //    // Store the person details in the file.
                    //    StreamWriter writer = new StreamWriter(stream);
                    //    writer.Write(chaine);
                    //    writer.Close();
                    //}

                    // pour l'objet securite 
                    // The CreateFile() method creates a new file or overwrites an existing one.
                    using (FileStream stream = store.CreateFile("session.securite"))
                    {
                        // Store the person details in the file.
                        serializerSessionEnumereSecurite.Serialize(stream, SessionObject.securiteActive);
                    }

                    //// imprimante par defaut 
                    //using (FileStream stream = store.CreateFile("printer.default"))
                    //{
                    //    StreamWriter writer = new StreamWriter(stream);
                    //    writer.Write(SessionObject.DefaultPrinter);
                    //    writer.Close();
                    //}

                    //// nom de la machine de la caissiere 
                    //using (FileStream stream = store.CreateFile("machine.name"))
                    //{
                    //    StreamWriter writer = new StreamWriter(stream);
                    //    writer.Write(SessionObject.MachineName);
                    //    writer.Close();
                    //}
                }
            }
            catch (Exception ex)
            {
                Message.Show(ex, Galatee.Silverlight.Resources.Langue.errorTitle+"CreateSessionObjectStore");// throw;
            }

        }

        void SetOpenModuleItem(string newModule)
        {
            try
            {
                //pour l'object Opennigay
                //MessageBox.Show("calling : " + "GetOpenedModules()");
                List<string> openedM = GetOpenedModules();
                openedM.Add(newModule);
                //MessageBox.Show("saving opening module : " + newModule);
                using (IsolatedStorageFile store = IsolatedStorageFile.GetUserStoreForSite())
                {
                    if (store.FileExists("openmodule.txt"))
                        store.DeleteFile("openmodule.txt");
                    

                    using (IsolatedStorageFileStream stream = store.CreateFile("openmodule.txt"))
                    {
                        string chaine = string.Empty;
                        string separateur = ";";
                        //MessageBox.Show("saving opening module : " + newModule);

                        foreach (string module in openedM)
                        {
                            chaine += module + separateur;
                        }
                        chaine = chaine.Substring(0, chaine.LastIndexOf(";"));
                        //MessageBox.Show("all stored module : "+ chaine);
                        StreamWriter writer = new StreamWriter(stream);
                        writer.Write(chaine);
                        writer.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Message.Show(ex, Galatee.Silverlight.Resources.Langue.errorTitle + "SetOpenModuleItem");// throw;

            }
        }

        List<string> GetOpenedModules()
        {
            try
            {
                using (IsolatedStorageFile store = IsolatedStorageFile.GetUserStoreForSite())
                {
                    if (store.FileExists("openmodule.txt"))
                    {
                        using (FileStream stream = store.OpenFile("openmodule.txt", FileMode.Open))
                        {
                            StreamReader reader = new StreamReader(stream);
                            string modulesOpened = reader.ReadLine();
                            //MessageBox.Show(modulesOpened);
                            SessionObject.OpenedModules = modulesOpened.Split(';').ToList();
                            reader.Close();
                        }

                    }
                    return SessionObject.OpenedModules;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [ScriptableMember()]
        public void InvokeOnCloseNavigator()
        {
            using (IsolatedStorageFile store =IsolatedStorageFile.GetUserStoreForSite())
            {
                try
                {
                    if (store.FileExists("AuthFlag.txt")) store.DeleteFile("AuthFlag.txt");
                    if (store.FileExists("AppLaunch.txt")) store.DeleteFile("AppLaunch.txt");
                    if (store.FileExists("OpenApp.txt")) store.DeleteFile("OpenApp.txt");
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            string closed = "true";

            try
            {
                using (IsolatedStorageFile store =
                   IsolatedStorageFile.GetUserStoreForSite())
                {
                    // The CreateFile() method creates a new file or overwrites an existing one.
                    using (IsolatedStorageFileStream stream = store.CreateFile("close.txt"))
                    {
                        // Store the person details in the file.
                        StreamWriter writer = new StreamWriter(stream);
                        writer.Write(closed);
                        writer.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [ScriptableMember()]
        public void ReduceFrame()
        {
            try
            {
                //System.Windows.Browser.HtmlElement plugin = System.Windows.Browser.HtmlPage.Document.GetElementById("MainPlg");
                //if (plugin != null)
                //{
                //    plugin.SetStyleAttribute("width", "100%");
                //    plugin.SetStyleAttribute("height", "100%");
                //    plugin.SetStyleAttribute("visibility", "visible");
                //}

                //// iframe id :ifHtmlContent
                //System.Windows.Browser.HtmlElement myFrame = System.Windows.Browser.HtmlPage.Document.GetElementById("ifHtmlContent");
                //if (myFrame != null)
                //{
                //    myFrame.SetStyleAttribute("width", "Opx");
                //    myFrame.SetStyleAttribute("height", "0px");
                //    myFrame.SetStyleAttribute("visibility", "hidden");
                //}

                string currentmodule = SessionObject.moduleCourant;
                HtmlElement pgl = HtmlPage.Document.GetElementById(currentmodule + "pgl");
                HtmlElement PluginFrame = HtmlPage.Document.GetElementById(currentmodule + "_IFRAME");
                //HtmlElement BUTTONFRAME = HtmlPage.Document.GetElementById(currentmodule + "_IFRAME_BUTTON");

                if (pgl != null)
                {
                    pgl.SetStyleAttribute("width", "100%");
                    pgl.SetStyleAttribute("height", "100%");
                    pgl.SetStyleAttribute("visibility", "visible");
                }

                if (PluginFrame != null)
                {
                    PluginFrame.SetStyleAttribute("width", "0%");
                    PluginFrame.SetStyleAttribute("height", "0%");
                    PluginFrame.SetStyleAttribute("left", "0");
                    PluginFrame.SetStyleAttribute("top", "50");
                    PluginFrame.SetStyleAttribute("visibility", "hidden");
                    //PluginFrame.SetAttribute("src", URI);
                    //PluginFrame.SetAttribute("src", "http://localhost:17207/Galatee.ModuleLoaderTestPage.aspx");

                    //BUTTONFRAME.SetStyleAttribute("visibility", "visible");


                }
            }
            catch (Exception ex)
            {
                Message.Show(ex, Galatee.Silverlight.Resources.Langue.errorTitle+"ReduceFrame");// throw;
            }
        }

        void CreateUserConnectedStore()
          {
              try
              {
                  ServiceAuthenInitialize.CsUserConnecte u = new ServiceAuthenInitialize.CsUserConnecte()
                  {
                      PK_ID = UserConnecte.PK_ID,
                      Centre = UserConnecte.Centre,
                      codefontion = UserConnecte.codefontion,
                      matricule = UserConnecte.matricule,
                      nomUtilisateur = UserConnecte.nomUtilisateur,
                      numcaisse = UserConnecte.numcaisse,
                      LibelleFontion = UserConnecte.LibelleFonction,
                      LibelleCentre =  UserConnecte.LibelleCentre,
                      PerimetreAction=UserConnecte.PerimetreAction,
                      FK_IDFONCTION=   UserConnecte.FK_IDFONCTION ,
                      listeProfilUser = UserConnecte.listeProfilUser,
                      FK_IDCENTRE = UserConnecte.FK_IDCENTRE ,
                      
                  };
                  List<string> lst = new List<string>();
                  foreach ( Galatee.Silverlight.ServiceAuthenInitialize.CsProfil   item in UserConnecte.listeProfilUser )
                  {
                      lst.AddRange( item.LESCENTRESPROFIL.Where( t=>t.FK_IDPROFIL == item.FK_IDPROFIL  && (t.DATEFINVALIDITE == null || t.DATEFINVALIDITE <=System.DateTime.Today.Date)).
                          Select(o=>item.PK_ID + "." + item.MODULE + "." + o.FK_IDCENTRE + "."+ item.CODEFONCTION).ToList());
                      //foreach (Galatee.Silverlight.ServiceAuthenInitialize.CsCentreProfil   items in )
                      //    lst.Add(item.PK_ID + "." + item.MODULE + "." + items.FK_IDCENTRE + "."+ item.CODEFONCTION  );
                  }
                  u.ListeDesCentreProfil = new List<string>();
                  u.ListeDesCentreProfil = lst;

//                  public static bool ExtendQuota(long sizeInBytes) 
//{ 
//    using (IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication()) 
//    { 
//        if (sizeInBytes < storage.Quota) return false; 
//        return storage.IncreaseQuotaTo(sizeInBytes); 
//    } 
//}

                  using (IsolatedStorageFile store = IsolatedStorageFile.GetUserStoreForSite())
                  {
                      //if (sizeInBytes < storage.Quota) return false;
                      //return storage.IncreaseQuotaTo(sizeInBytes); 

                      //if (store.UsedSize 
                      //store.IncreaseQuotaTo(

                      // The CreateFile() method creates a new file or overwrites an existing one.
                      using (FileStream stream = store.CreateFile("user.user"))
                      {
                          // Store the person details in the file.
                          serializerConnectedUser.Serialize(stream, u);
                      }
                  }
              }
              catch (Exception ex)
              {
                  Message.ShowError(ex, Galatee.Silverlight.Resources.Langue.errorTitle+"CreateUserConnectedStore");// throw;
              }
          }

        void CreationSessionStore()
        {
            try
            {
                using (IsolatedStorageFile store = IsolatedStorageFile.GetUserStoreForSite())
                {
                    ServiceAuthenInitialize.CsUserConnecte u = new ServiceAuthenInitialize.CsUserConnecte()
                    {
                        Centre = UserConnecte.Centre,
                        codefontion = UserConnecte.codefontion,
                        matricule = UserConnecte.matricule,
                        nomUtilisateur = UserConnecte.nomUtilisateur,
                        numcaisse = UserConnecte.numcaisse
                    };
                    // The CreateFile() method creates a new file or overwrites an existing one.
                    using (FileStream stream = store.CreateFile("sessionobject.data"))
                    {
                        // Store the person details in the file.
                        serializerDesktopGroup.Serialize(stream, u);
                    }
                }
            }
            catch (Exception ex)
            {
                Message.Show(ex, Galatee.Silverlight.Resources.Langue.errorTitle+"CreationSessionStore");// throw;
            }
        }

        void LaunchAppFromModule(bool pValue)
        {
            try
            {
                using (IsolatedStorageFile store =
                  IsolatedStorageFile.GetUserStoreForSite())
                {
                    if (pValue)
                    {
                        if (!store.FileExists("OpenApp.txt"))
                        using (IsolatedStorageFileStream stream = store.CreateFile("OpenApp.txt"))
                        {
                            StreamWriter writer = new StreamWriter(stream);
                            writer.Write(pValue.ToString());
                            writer.Close();
                        }
                    }
                    else
                        if (store.FileExists("OpenApp.txt"))
                            store.DeleteFile("OpenApp.txt");
                }  
            }
            catch (Exception ex)
            {
                //Message.Show(ex, Galatee.Silverlight.Resources.Langue.errorTitle + "LaunchAppFromModule");
              throw ex;
            }
        }

        void CreateAuthExist()
        {
            bool flag = true;
            string Strems = "GALATEE V9";
            try
            {
                using (IsolatedStorageFile store =
                   IsolatedStorageFile.GetUserStoreForSite())
                {
                    // The CreateFile() method creates a new file or overwrites an existing one.
                    using (IsolatedStorageFileStream stream = store.CreateFile("AuthFlag.txt"))
                    {
                        // Store the person details in the file.
                        StreamWriter writer = new StreamWriter(stream);
                        writer.Write(flag.ToString());
                        writer.Close();
                    }

                    // The CreateFile() method creates a new file or overwrites an existing one.
                    bool flags = false;
                    using (IsolatedStorageFileStream stream = store.CreateFile("close.txt"))
                    {
                        // Store the person details in the file.
                        StreamWriter writer = new StreamWriter(stream);
                        writer.Write(flags.ToString());
                        writer.Close();
                    }

                    using (IsolatedStorageFileStream stream = store.CreateFile("AppLaunch.txt"))
                    {
                        // Store the person details in the file.
                        StreamWriter writer = new StreamWriter(stream);
                        writer.Write(Strems);
                        writer.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Message.Show(ex, Galatee.Silverlight.Resources.Langue.errorTitle + "CreateAuthExist");// throw;
            }
            
        }

        void CreateFlagEstDejaConnecte()
        {
            try
            {
                using (IsolatedStorageFile store =
                   IsolatedStorageFile.GetUserStoreForSite())
                {
                    if (store.FileExists("OpenApp.txt")) store.DeleteFile("OpenApp.txt");
                    using (IsolatedStorageFileStream stream = store.CreateFile("OpenApp.txt"))
                    {
                        bool flag = false;
                        StreamWriter writer = new StreamWriter(stream);
                        writer.Write(flag.ToString());
                        writer.Close();
                    }

                }
            }
            catch (Exception ex)
            {
              throw ex;
            }
        }

        bool EstDejaConnecte()
        {
            string respond = string.Empty;
            try
            {
                using (IsolatedStorageFile store = IsolatedStorageFile.GetUserStoreForSite())
                {
                  return store.FileExists("OpenApp.txt");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        bool VerifierAuthExistDeja()
        {
            string respond = string.Empty;
            try
            {
                using (IsolatedStorageFile store = IsolatedStorageFile.GetUserStoreForSite())
                {
                    if (!store.FileExists("AuthFlag.txt")) return false;
                    using (FileStream stream = store.OpenFile("AuthFlag.txt", FileMode.Open))
                    {
                        StreamReader reader = new StreamReader(stream);
                        respond = reader.ReadLine();
                        reader.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Message.Show(ex, Galatee.Silverlight.Resources.Langue.errorTitle +"VerifierAuthExistDeja");// throw;
                respond = "false"; 
            }
            
             return Convert.ToBoolean(respond);
        }

        bool VerifierSendModule()
        {
            string respond = string.Empty;
            try
            {
                using (IsolatedStorageFile store = IsolatedStorageFile.GetUserStoreForSite())
                {
                    if (!store.FileExists("modulesend.txt")) return false;
                    using (FileStream stream = store.OpenFile("modulesend.txt", FileMode.Open))
                    {
                        StreamReader reader = new StreamReader(stream);
                        respond = reader.ReadLine();
                        reader.Close();
                        return string.IsNullOrEmpty(respond);
                    }
                }
            }
            catch (Exception ex)
            {
                //string error = ex;
                Message.ShowError(ex, Galatee.Silverlight.Resources.Langue.errorTitle + "VerifierSendModule");// throw;
                respond = "false";
            }

            return true;
        }
           
        void ConnexionSuccess(object sender, EventArgs e)
        {
            try
            {
                if (passages == 0)
                {
                    passages++;
                    UcConnexion ctrs = sender as UcConnexion;

                    // signaler que l'authentification est effective
                    //CreateFlagEstDejaConnecte();
                    CreateAuthExist();

                    //suppression du store des modules view
                    DeleteModuleViewStore();

                    //creation du store du user connecte
                    CreateUserConnectedStore();

                    // creation du session object store
                    CreateSessionObjectStore();
                    foreach (ServiceAuthenInitialize.CsDesktopGroup d in ctrs.modules)
                    {
                        using (IsolatedStorageFile store =
                      IsolatedStorageFile.GetUserStoreForSite())
                        {
                            // The CreateFile() method creates a new file or overwrites an existing one.
                            using (FileStream stream = store.CreateFile(d.NOM + ".desktopgroup"))
                            {
                                // Store the person details in the file.
                                serializerDesktopGroup.Serialize(stream, d);
                            }
                        }

                    }
                    CreateModuleView();

                    //#region Affichage du tableau de bord //Ajouté par WCO le 06/09/2015
                    //1- On recupère les groupes de validation de l'utilisateur
                    int back = LoadingManager.BeginLoading("Chargement du Tableau de bord");
                    ParametrageClient client = new ParametrageClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Parametrage"));
                    client.VerifierAppartenanceGroupeValidationCompleted += (gsender, gargs) =>
                    {
                        LoadingManager.EndLoading(back);
                        if (gargs.Cancelled || gargs.Error != null)
                        {
                            string error = gargs.Error.Message;
                            Message.Show(error, Languages.ListeCodePoste);
                            return;
                        }
                        if (gargs.Result == null)
                        {
                            Message.ShowError(Languages.msgErreurChargementDonnees, Languages.Parametrage);
                            return;
                        }
                        if (gargs.Result)
                        {
                            SessionObject.IsChargerDashbord = true;
                            BuildDashbord();
                            Classes.IsolatedStorage.storeIsWorkfloGp("OUI");
                            UserConnecte.IsAppartienGroupeValidation = true;
                        }
                    };
                    client.VerifierAppartenanceGroupeValidationAsync(UserConnecte.PK_ID);

                    //#endregion

                    //Ajouter par ATO le 06/04/2013 pour afficher les information utilisateur
                    ShowUserInformations();
                    //Fin Ajout ATO le 06/04/2013 pour afficher les information utilisateur
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, Galatee.Silverlight.Resources.Langue.errorTitle +" ConnexionSuccess");// throw;
            }
        }

        private void ShowUserInformations()
        {
            try
            {
                this.Matricule.Visibility = System.Windows.Visibility.Visible ;
                this.Nom.Visibility = System.Windows.Visibility.Visible;
                //this.Fonction.Visibility = System.Windows.Visibility.Visible;
                this.Centre.Visibility = System.Windows.Visibility.Visible;
                //hyperlinkButton1.Visibility = System.Windows.Visibility.Visible;
                //this.NumeroCaisse.Visibility = System.Windows.Visibility.Visible;

                Matricule.Content = UserConnecte.matricule.ToUpper() ?? string.Empty;
                Nom.Content = UserConnecte.nomUtilisateur.ToUpper() ?? string.Empty;
                //Fonction.Content = UserConnecte.LibelleFonction.ToUpper() ?? string.Empty;
                Centre.Content = UserConnecte.LibelleCentre.ToUpper() ?? string.Empty;
                //NumeroCaisse.Content =!string.IsNullOrEmpty( UserConnecte.numcaisse) ? "Caisse :" + UserConnecte.numcaisse: string.Empty;
                
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        void DeleteModuleViewStore()
        {
            try
            {
                using (IsolatedStorageFile store = IsolatedStorageFile.GetUserStoreForSite())
                {
                    foreach (string file in store.GetFileNames("*.desktopgroup"))
                    {
                        try
                        {
                            if (store.FileExists(file))
                                store.DeleteFile(file);

                        }
                        catch (Exception ex)
                        {
                            Message.Show(ex, Galatee.Silverlight.Resources.Langue.errorTitle + "  DeleteModuleViewStore");// throw;
                        }
                    }

                    // delete moduleopend store
                    if(store.FileExists("openmodule.txt"))
                        store.DeleteFile("openmodule.txt");
                }
            }
            catch (Exception ex) 
            {
                throw ex;
            }
            
        }

        void CreateModuleView()
        {
            try
            {
                ScriptObject script = (ScriptObject)HtmlPage.Window.GetProperty("CreatePluginModule");
                script.InvokeSelf("ModuleDiv");


                HtmlElement divElement1 = HtmlPage.Document.GetElementById("ModuleDivTD");
                HtmlElement divElement2 = HtmlPage.Document.GetElementById("MainPlgTD");
                HtmlElement divElement3 = HtmlPage.Document.GetElementById("black");
                divElement1.SetStyleAttribute("width", "15%");
                divElement1.SetStyleAttribute("height", "100%");
                divElement2.SetStyleAttribute("width", "85%");
                divElement3.SetStyleAttribute("width", "84%");
                divElement2.SetStyleAttribute("height", "100%");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Uri SourceUri = new Uri(HtmlPage.Document.DocumentUri, Application.Current.Host.Source.ToString().Substring(0, Application.Current.Host.Source.ToString().IndexOf("Bin")) + "report.aspx"); // Report.aspx
                HtmlPage.Window.Navigate(SourceUri, "_blank"); // _blank => new Window 
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void Btn_LogOut_Click(object sender, RoutedEventArgs e)
        {
            //;
           
           // MenuGlobal.MenuItem.Clear();
            try
            {
                MenuGlobal.RefleshMenu();
            }
            catch (Exception ex)
            {

                Message.Show(ex, Galatee.Silverlight.Resources.Langue.errorTitle + " Btn_LogOut_Click");
            }
           
           // MenuGlobal.ClearReferences(MenuGlobal.MenuItem);
             //MenuGlobal.Children.Clear();

            //UcPNewPaymentPlan plan = new UcPNewPaymentPlan();
            //plan.Show();
            ////HtmlPage.Window.Navigate(HtmlPage.Document.DocumentUri);
            //invokeAsp asp = new invokeAsp();

            //List<Galatee.Silverlight.serviceWeb.CsReglement> listes = new List<Galatee.Silverlight.serviceWeb.CsReglement>()
            //{
            // new Galatee.Silverlight.serviceWeb.CsReglement(){ ACQUIT="000002", BANQUE="BIAO"},
            // new Galatee.Silverlight.serviceWeb.CsReglement(){ ACQUIT="054546", BANQUE="BIAO"},
            // new Galatee.Silverlight.serviceWeb.CsReglement(){ ACQUIT="00036", BANQUE="BIAO"},
            // new Galatee.Silverlight.serviceWeb.CsReglement(){ ACQUIT="085936", BANQUE="BIAO"},
            // new Galatee.Silverlight.serviceWeb.CsReglement(){ ACQUIT="00036", BANQUE="BIAO"},
            // new Galatee.Silverlight.serviceWeb.CsReglement(){ ACQUIT="00036", BANQUE="ECOBANK"},
            // new Galatee.Silverlight.serviceWeb.CsReglement(){ ACQUIT="5545454", BANQUE="ECOBANK"},
            // new Galatee.Silverlight.serviceWeb.CsReglement(){ ACQUIT="00036", BANQUE="ECOBANK"},
            // new Galatee.Silverlight.serviceWeb.CsReglement(){ ACQUIT="00036", BANQUE="ECOBANK"},
            // new Galatee.Silverlight.serviceWeb.CsReglement(){ ACQUIT="00036", BANQUE="ECOBANK"},
            // new Galatee.Silverlight.serviceWeb.CsReglement(){ ACQUIT="54545", BANQUE="UBA"},
            // new Galatee.Silverlight.serviceWeb.CsReglement(){ ACQUIT="00036", BANQUE="UBA"},
            // new Galatee.Silverlight.serviceWeb.CsReglement(){ ACQUIT="00036", BANQUE="UBA"},
            // new Galatee.Silverlight.serviceWeb.CsReglement(){ ACQUIT="4545445", BANQUE="UBA"},
            // new Galatee.Silverlight.serviceWeb.CsReglement(){ ACQUIT="00036", BANQUE="UBA"},
            // new Galatee.Silverlight.serviceWeb.CsReglement(){ ACQUIT="00036", BANQUE="UBA"},
            // new Galatee.Silverlight.serviceWeb.CsReglement(){ ACQUIT="54455", BANQUE="ORANGE"},
            // new Galatee.Silverlight.serviceWeb.CsReglement(){ ACQUIT="00036", BANQUE="ORANGE"},
            // new Galatee.Silverlight.serviceWeb.CsReglement(){ ACQUIT="00036", BANQUE="ORANGE"},
            // new Galatee.Silverlight.serviceWeb.CsReglement(){ ACQUIT="36552", BANQUE="ORANGE"},
            // new Galatee.Silverlight.serviceWeb.CsReglement(){ ACQUIT="99865", BANQUE="ORANGE"}
            //};
            //List<Person> liste = new List<Person>() { 
               
            //    new Person(){ Age=10,Description="csharper",Gender="M",Name="minno",Weight=75},
            //    new Person(){ Age=50,Description="vbdontet",Gender="F",Name="hermann",Weight=78}
            //}
            //    ;
            //Dictionary<string, string> param = new Dictionary<string, string>();
            //param.Add("minno_pTypeRecu", "OPERATION DE CAISSE");
            //Utility.Action(listes,param, "recu", "Caisse", "minno");
          //  asp.launchPrint("Caisse", "recu", "minno", listes);
            //asp.launchOPrint("Caisse", "recu", "minno", listes);
            //asp.launchPrint("Caisse", "recu", "minno");
            //asp.Invoke("Caisse", "recu", listes);launchPrint
            //asp.Invoke("report", 1, 1);
            //report.lauchAspPage("ReportAspPage/WebForm1.aspx");
        }

        private void Btn_LogIn_Click(object sender, RoutedEventArgs e)
        {
            //HtmlPage.Document.Submit();
        }

        private void button2_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                // On récupère l'url à laquelle se trouve le XAP Silverlight
                String uri = Application.Current.Host.Source.AbsoluteUri;
                // On retourne à la racine du dossier du projet Web
                uri = uri.Substring(0, uri.IndexOf("/ClientBin")) + "/Config" + "/ClientConfig.xml";
                //uri = uri.Substring(uri.LastIndexOf("/ClientBin"));
                //uri = uri.Substring(0, uri.IndexOf("/ClientBin")) + "/ClientConfig.xml";

                // On créé le Webclient
                WebClient objWebClient = new WebClient();
                // on gère l'évenement "J'ai fini de télécharger le fichier XML"
                objWebClient.DownloadStringCompleted += (sen, args) =>
                {
                    try
                    {
                        // Si le télécghargement c'est bien passé et n'as pas été annulé
                        if (!args.Cancelled && args.Error == null)
                        {
                            List<object> li = new List<object>();
                            // On convertis la String en objet XML
                            XElement myXml = XElement.Parse(args.Result);
                            // On parcours tout les neuds User
                            foreach (XElement el in (from el in myXml.Elements("ConfigClass") select el).ToList())
                            {
                                li.Add(CreateOurNewObject(el));
                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        Message.Show(ex, Galatee.Silverlight.Resources.Langue.errorTitle + "  button2_Click_1");
                    }
                };
                objWebClient.DownloadStringAsync((new Uri(uri, UriKind.Absolute))); ;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private object CreateOurNewObject(XElement monItem)
        {
            /// >>> MODIFICATION END

            // create a dynamic assembly and module
            try
            {
                AssemblyName assemblyName = new AssemblyName();
                assemblyName.Name = "tmpAssembly";
                AssemblyBuilder assemblyBuilder = Thread.GetDomain().DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
                ModuleBuilder module = assemblyBuilder.DefineDynamicModule("tmpModule");

                // create a new type builder
                TypeBuilder typeBuilder = module.DefineType("BindableRowCellCollection", TypeAttributes.Public | TypeAttributes.Class);

                // Loop over the attributes that will be used as the properties names in out new type
                /// >>> MODIFICATION START
                foreach (XElement node in monItem.Elements())
                {
                    string propertyName = node.Name.ToString();
                    /// >>> MODIFICATION END

                    // Generate a private field
                    FieldBuilder field = typeBuilder.DefineField("_" + propertyName, typeof(string), FieldAttributes.Private);
                    // Generate a public property
                    PropertyBuilder property =
                        typeBuilder.DefineProperty(propertyName,
                                         PropertyAttributes.None,
                                         typeof(string),
                                         new Type[] { typeof(string) });

                    // The property set and property get methods require a special set of attributes:

                    MethodAttributes GetSetAttr =
                        MethodAttributes.Public |
                        MethodAttributes.HideBySig;

                    // Define the "get" accessor method for current private field.
                    MethodBuilder currGetPropMthdBldr =
                        typeBuilder.DefineMethod("get_value",
                                                   GetSetAttr,
                                                   typeof(string),
                                                   Type.EmptyTypes);

                    // Intermediate Language stuff...
                    ILGenerator currGetIL = currGetPropMthdBldr.GetILGenerator();
                    currGetIL.Emit(OpCodes.Ldarg_0);
                    currGetIL.Emit(OpCodes.Ldfld, field);
                    currGetIL.Emit(OpCodes.Ret);

                    // Define the "set" accessor method for current private field.
                    MethodBuilder currSetPropMthdBldr =
                        typeBuilder.DefineMethod("set_value",
                                                   GetSetAttr,
                                                   null,
                                                   new Type[] { typeof(string) });

                    // Again some Intermediate Language stuff...
                    ILGenerator currSetIL = currSetPropMthdBldr.GetILGenerator();
                    currSetIL.Emit(OpCodes.Ldarg_0);
                    currSetIL.Emit(OpCodes.Ldarg_1);
                    currSetIL.Emit(OpCodes.Stfld, field);
                    currSetIL.Emit(OpCodes.Ret);

                    // Last, we must map the two methods created above to our PropertyBuilder to
                    // their corresponding behaviors, "get" and "set" respectively.
                    property.SetGetMethod(currGetPropMthdBldr);
                    property.SetSetMethod(currSetPropMthdBldr);
                }

                // Generate our type
                Type generetedType = typeBuilder.CreateType();

                // Now we have our type. Let's create an instance from it:
                object generetedObject = Activator.CreateInstance(generetedType);

                // Loop over all the generated properties, and assign the values from our XML:
                PropertyInfo[] properties = generetedType.GetProperties();

                int propertiesCounter = 0;

                // Loop over the values that we will assign to the properties
                /// >>> MODIFICATION START
                foreach (XElement node in monItem.Elements())
                {
                    string value = node.Value.ToString();
                    /// >>> MODIFICATION END
                    properties[propertiesCounter].SetValue(generetedObject, value, null);
                    propertiesCounter++;
                }

                //Yoopy ! Return our new genereted object.
                return generetedObject;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void Page_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                //if (e.Key.Equals(Key.Shift))
                //{
                //    new UcResetLicense().Show();
                //}
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void RetournePoste(bool IsMsgPrompt, string msgAvertissement)
        {
            //Obtenir les donnees de l'arborescence des modules , programmes et des menus relatifs
            Galatee.Silverlight.ServiceAdministration.AdministrationServiceClient prgram = new Galatee.Silverlight.ServiceAdministration.AdministrationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Administration"));
            prgram.RetourneListePosteAsync();
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
                        Galatee.Silverlight.Administration.FrmParametragePoste ctrl = new Administration.FrmParametragePoste();
                        ctrl.Closed += new EventHandler(galatee_PosteOK);
                        ctrl.Show();
                    }
                    else
                    {
                        SessionObject.ListePoste = resprog.Result;
                        string Machine = Silverlight.Classes.IsolatedStorage.getMachine();
                        if (!string.IsNullOrEmpty(Machine))
                        {
                            if (SessionObject.ListePoste.FirstOrDefault(p => p.NOMPOSTE.ToUpper().Trim() == Machine.ToUpper().Trim()) == null)
                            {
                                Galatee.Silverlight.Administration.FrmParametragePoste ctrl = new Administration.FrmParametragePoste();
                                ctrl.Closed += new EventHandler(galatee_PosteOK);
                                ctrl.Show();
                            }
                            else
                            {
                                SessionObject.LePosteCourant = SessionObject.ListePoste.FirstOrDefault(p => p.NOMPOSTE == Machine);
                                string CentreConnecte = SessionObject.ListePoste.FirstOrDefault(p => p.NOMPOSTE == Machine).CODECENTRE;
                                if (SessionObject.LePosteCourant.FK_IDCAISSE == null || SessionObject.LePosteCourant.FK_IDCAISSE == 0)
                                    SessionObject.PosteNonCaisse = false;

                                Silverlight.Classes.IsolatedStorage.storeCentre(CentreConnecte);
                                OuvertureFenetreAuthentification(IsMsgPrompt, msgAvertissement);
                            }
                        }
                        else
                        {
                            Galatee.Silverlight.Administration.FrmParametragePoste ctrl = new Administration.FrmParametragePoste();
                            ctrl.Closed += new EventHandler(galatee_PosteOK);
                            ctrl.Show();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Message.ShowError(ex.Message, "RetournePoste");
                }

            };
        }


        //Ajouté par WCO le 03/08/2015
        //Permet de construire le tableau de bord en appelant le control Galatee.Silverlight.Workflow.UcWKFDashBoard()        
        void BuildDashbord()
        {
            SessionObject.moduleCourant = "Workflow";
            UserControl UcForm = null;

            //Pas besoin de construire le menu pour le tableau de bord à l'accueil
            //TriggerMenuView menu = new TriggerMenuView(this, this.MenuGlobal);
            //menu.RaiseView(SessionObject.moduleCourant, LayoutRoot);
            var conteneur = Library.AllInOne.FindControl<Viewbox>(LayoutRoot, typeof(Viewbox), "Body");
            conteneur.Margin = new Thickness(0, 30, 0, 0);
            SessionObject.ViewBox = conteneur;
            UcForm = new Galatee.Silverlight.Workflow.UcWKFDashBoard();
            if (UcForm != null)
            {
                //  UcForm.Tag = menuItem;
                SessionObject.ViewBox.Child = UcForm;
            }
        }
        private void LicenseLooking()
        {
            Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient services = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
            services.LicenseLookingCompleted += (ss, ess) =>
            {
                try
                {
                    if (ess.Cancelled || ess.Error != null)
                    {
                        Message.ShowError(Galatee.Silverlight.Resources.Langue.wcf_error + "::" + "returnEnumereWrappe", Galatee.Silverlight.Resources.Langue.errorTitle);
                        return;
                    }

                    if (ess.Result == null)
                    {
                        Message.ShowInformation(Galatee.Silverlight.Resources.Langue.msgNodata + "::" + "returnEnumereWrappe", Galatee.Silverlight.Resources.Langue.informationTitle); //new DialogResult("Wrong parameters", false).Show(); //
                        return;
                    }

                    return;
                }
                catch (Exception ex)
                {
                    Message.ShowError(ex + "::" + "returnEnumereWrappe", Galatee.Silverlight.Resources.Langue.errorTitle);
                }
            };
            services.LicenseLookingAsync();
        }
        private void LicenceOK(bool IsMsgPrompt, string msgAvertissement)
        {
            Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient services = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
            services.LicenceOKCompleted += (ss, ess) =>
            {
                try
                {
                    if (ess.Cancelled || ess.Error != null)
                    {
                        Message.ShowError(Galatee.Silverlight.Resources.Langue.wcf_error + "::" + "returnEnumereWrappe", Galatee.Silverlight.Resources.Langue.errorTitle);
                        return;
                    }

                    if (ess.Result == null)
                    {
                        Message.ShowInformation(Galatee.Silverlight.Resources.Langue.msgNodata + "::" + "returnEnumereWrappe", Galatee.Silverlight.Resources.Langue.informationTitle); //new DialogResult("Wrong parameters", false).Show(); //
                        return;
                    }
                    if (ess.Result == true)
                    {
                        if (DateTime.Now > new DateTime(2070, 12, 23))
                        {
                            /** ZEG 29/08/2017 **/

                            /*
                             Message.ShowInformation("Veuillez contacter votre fournisseur logiciel afin de réactiver votre licence", "Licensing");
                             LicenseLooking();
                             return; */

                            CsCaisse dr = new CsCaisse();
                            string zozo = dr.LIBELLE.ToString();

                            /** ZEG **/

                        }

                        RetournePoste(IsMsgPrompt, msgAvertissement);

                    }
                    else
                    {
                        /** ZEG 29/08/2017 **/
                        /*
                        Message.ShowInformation("Veuillez contacter votre fournisseur logiciel afin de réactiver votre licence", "Licensing");
                        return;*/

                        CsCaisse dr = new CsCaisse();
                        string zozo = dr.LIBELLE.ToString();

                        /** ZEG **/

                    }
                }
                catch (Exception ex)
                {
                    Message.ShowError(ex + "::" + "returnEnumereWrappe", Galatee.Silverlight.Resources.Langue.errorTitle);
                }
            };
            services.LicenceOKAsync();
        }

        //private void hyperlinkButton1_Click_1(object sender, RoutedEventArgs e)
        //{
        //    HtmlPage.Document.Submit();
        //}
    }
}
