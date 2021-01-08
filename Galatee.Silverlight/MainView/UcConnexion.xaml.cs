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
using Galatee.Silverlight.ServiceAuthenInitialize;
//using Galatee.Silverlight.serviceWeb;
using System.Collections.ObjectModel;
using System.Windows.Printing;
using Galatee.Silverlight.Security;
using Galatee.Silverlight.Resources;
using System.Windows.Browser;
using Galatee.Silverlight.Administration;

namespace Galatee.Silverlight.Caisse
{
    public partial class UcConnexion : ChildWindow
    {
        public event EventHandler success;
        public string Module = "Accueil";
        bool firstCall = true;
        bool completedAllRequest = false;
        private Page _page;
        int passage = 0;
        Dictionary<ServiceAuthenInitialize.CSMenuGalatee, List<ServiceAuthenInitialize.TreeNodes>> dico = new Dictionary<ServiceAuthenInitialize.CSMenuGalatee, List<ServiceAuthenInitialize.TreeNodes>>();
        List<ServiceAuthenInitialize.CsDesktopGroup> _modules = new List<ServiceAuthenInitialize.CsDesktopGroup>();
        //Galatee.Silverlight.ServiceCaisse.CsOpenningDay CaisseOverte = new Galatee.Silverlight.ServiceCaisse.CsOpenningDay();

        bool passwordExpireJamais = false;
        string EtatCaisse = string.Empty;
        ServiceAuthenInitialize.CParametre CaisseSelection = new ServiceAuthenInitialize.CParametre();
        Galatee.Silverlight.ServiceAuthenInitialize.CsUtilisateur user = new Galatee.Silverlight.ServiceAuthenInitialize.CsUtilisateur();

        public Dictionary<ServiceAuthenInitialize.CSMenuGalatee, List<ServiceAuthenInitialize.TreeNodes>> Dico
        {
            get { return dico; }
            set { dico = value; }
        }

        public List<ServiceAuthenInitialize.CsDesktopGroup> modules
        {
            get { return _modules; }
            set { _modules = value; }
        }
  
        public UcConnexion(UserControl m)
        {
            InitializeComponent();
            System.Windows.Browser.HtmlPage.Plugin.Focus();
            this.Focus();
            this.GotFocus += new RoutedEventHandler(ConnexionGotFocus);
        }

        void ConnexionGotFocus(object sender, RoutedEventArgs e)
        {
            if (firstCall)
            {
                txtLogin.Focus();
                this.firstCall = false;
            }
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                LayoutRoot.Cursor = Cursors.Wait;
                if (passage == 0)
                {
                    isModification = false;
                    passage++;
                    this.OKButton.IsEnabled = false;
                    VerifierDonneesSaisies(); //ObtenirDonneesConnection();
                }
            }
            catch (Exception ex)
            {
              LayoutRoot.Cursor = Cursors.Wait;
              Message.ShowError(ex, Galatee.Silverlight.Resources.Langue.errorTitle);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        void InitializationDonneesApp()
        {
            try
            {
                //desactiver les boutons de connexions

                OKButton.IsEnabled = CancelButton.IsEnabled = false;
                // Initialisation des objec static ( Session ) 
                AuthentInitializeServiceClient services = new AuthentInitializeServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Initialisation"));
                services.returnEnumereWrapperCompleted += (ss, ess) =>
                {
                    SessionObject.Enumere = ess.Result;
                    AuthentInitializeServiceClient securite = new AuthentInitializeServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Initialisation"));
                    securite.GetStrategieSecuriteActifCompleted += (ssecurite, resultsecurity) =>
                    {
                        if (resultsecurity.Cancelled || resultsecurity.Error != null)
                        {
                            passage = 0;
                            CancelButton.IsEnabled = OKButton.IsEnabled = true;
                            Message.ShowInformation("Error occurs while processing request !", "GetStrategieSecuriteActif");
                            desableProgressBar();
                            return;
                        }

                        if (resultsecurity.Result == null)
                        {
                            Message.ShowInformation("Wrong parameters", Galatee.Silverlight.Resources.Langue.errorTitle); //new DialogResult("Wrong parameters", false).Show(); //
                            CancelButton.IsEnabled = OKButton.IsEnabled = true;
                            passage = 0;
                            desableProgressBar();
                            return;
                        }

                        OKButton.IsEnabled = CancelButton.IsEnabled = true;
                        SessionObject.securiteActive = resultsecurity.Result;
                    };
                    securite.GetStrategieSecuriteActifAsync();
                };
                services.returnEnumereWrapperAsync();
            }
            catch (Exception ex)
            {
                OKButton.IsEnabled = CancelButton.IsEnabled = true;
                Message.Show(ex.Message, Galatee.Silverlight.Resources.Langue.errorTitle);
            }
        }
        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
         
        }
        private void txthidden_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txthidden.Text))
            {
                string lechar_saisi = txthidden.Text.Last().ToString();
                if (System.Text.RegularExpressions.Regex.IsMatch(lechar_saisi, @"[A-Z]"))
                {
                    lblMessageMajusculeActive.Content = "La majuscule est activée";
                }
                else
                {
                    lblMessageMajusculeActive.Content = string.Empty;
                }
            }
            else
            {
                lblMessageMajusculeActive.Content = string.Empty;
            }
        }
        private void ChildWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key.Equals(Key.Enter))
            {
                this.OKButton.IsEnabled = false;
                //ObtenirDonneesConnection();
                VerifierDonneesSaisies();
            }
        }

        void allowProgressBar()
        {
            progressBar1.IsEnabled = true;
            progressBar1.Visibility = Visibility.Visible;
            progressBar1.IsIndeterminate = true;
        }

        void desableProgressBar()
        {
            progressBar1.IsIndeterminate = false;
            progressBar1.Visibility = Visibility.Collapsed;
        }

        void VerifierLicenseInferiorPeriodAlert()
        {
            try
            {
                AuthentInitializeServiceClient license = new AuthentInitializeServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Initialisation"));
                license.LicenseInferieurPeriodeAvertissemntCompleted += (sendlic, resultlic) =>
                    {
                        if (resultlic.Cancelled || resultlic.Error != null)
                        {
                            Message.Show(resultlic.Error.Message,Galatee.Silverlight.Resources.Langue.errorTitle);
                            return;
                        }

                        if (resultlic.Result == null) // si la période d'avertissement  nest pas encore atteinte
                            InitializationDonneesApp();
                        else if(resultlic.Result == string.Empty) //si la license n'existe pas de la configuration du serveur
                        {
                            Message.Show("Une mise à jour de la license est requise !",Galatee.Silverlight.Resources.Langue.errorTitle);
                            this.DialogResult = true;
                        }
                        else
                            if(!string.IsNullOrEmpty(resultlic.Result)){
                                VerifierLicenseInferiorPeriodValidite(resultlic.Result);
                        }
                    };
                license.LicenseInferieurPeriodeAvertissemntAsync();

            }
            catch (Exception ex )
            {
                Message.Show(ex.Message, Galatee.Silverlight.Resources.Langue.errorTitle);// throw;
            }
        }

        void VerifierLicenseInferiorPeriodValidite(string msgAvertissement)
        {
            try
            {
                AuthentInitializeServiceClient license = new AuthentInitializeServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Initialisation"));
                license.LicenseInferieurPeriodeValiditeCompleted += (sendlic, resultlic) =>
                {
                    if (resultlic.Cancelled || resultlic.Error != null)
                    {
                        Message.Show(resultlic.Error.Message, Galatee.Silverlight.Resources.Langue.errorTitle);
                        return;
                    }

                    if (resultlic.Result == false)
                    {
                        Message.Show("La license va expirée dans :" + msgAvertissement + " Jours", Galatee.Silverlight.Resources.Langue.informationTitle);
                        InitializationDonneesApp();// VerifierDonneesSaisies();
                    }
                    else
                    {
                        Message.Show("veuillez contacter INOVA Ci pour une nouvelle license", Galatee.Silverlight.Resources.Langue.informationTitle);
                        this.DialogResult = true;
                    }
                };
                license.LicenseInferieurPeriodeValiditeAsync();

            }
            catch (Exception ex)
            {
                Message.Show(ex.Message, Galatee.Silverlight.Resources.Langue.errorTitle);// throw;
            }
        }

        //void ObtenirDonneesConnection()
        //{

        //    try
        //    {

        //        if (!string.IsNullOrEmpty(txtLogin.Text) && !string.IsNullOrEmpty(txtpwd.Password))
        //        {
        //            allowProgressBar();
        //            CancelButton.IsEnabled = OKButton.IsEnabled = false;

        //            AuthentInitializeServiceClient service = new AuthentInitializeServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Initialisation"));
        //            service.RetourneInfoMatriculeConnecteCompleted += (s, es) =>
        //            {

        //                try
        //                {
        //                    if (es.Cancelled || es.Error != null)
        //                    {
        //                        passage = 0;
        //                        CancelButton.IsEnabled = OKButton.IsEnabled = true;
        //                        Message.ShowInformation(Galatee.Silverlight.Resources.Langue.wcf_error,Galatee.Silverlight.Resources.Langue.errorTitle);
        //                        desableProgressBar();
        //                        passage = 0;
        //                        LayoutRoot.Cursor = Cursors.Wait;
        //                        return;
        //                    }

        //                    if (es.Result == null)
        //                    {
        //                        Message.ShowInformation(Galatee.Silverlight.Resources.Langue.wcf_no_user_found, Galatee.Silverlight.Resources.Langue.errorTitle); //new DialogResult("Wrong parameters", false).Show(); //
        //                        CancelButton.IsEnabled = OKButton.IsEnabled = true;
        //                        passage = 0;
        //                        desableProgressBar();
        //                        passage = 0;
        //                        LayoutRoot.Cursor = Cursors.Wait;
        //                        return;
        //                    }
        //                    else
        //                    {
        //                        //UserConnecte.PK_ID = es.Result.PK_ID;
        //                        UserConnecte.matricule = es.Result.matricule;
        //                        UserConnecte.Centre = es.Result.Centre;
        //                        UserConnecte.numcaisse = es.Result.numcaisse;
        //                        UserConnecte.nomUtilisateur = es.Result.nomUtilisateur;
        //                        UserConnecte.codefontion = es.Result.codefontion;
        //                        UserConnecte.LibelleFonction = es.Result.LibelleFontion;
        //                        UserConnecte.PerimetreAction = es.Result.PerimetreAction;
        //                        UserConnecte.LibelleCentre = es.Result.LibelleCentre;


        //                        ServiceAuthenInitialize.CsUserConnecte user = new ServiceAuthenInitialize.CsUserConnecte();
        //                        user = es.Result;
        //                        // construction du menu des modules relatifs au profil du userconnecte

        //                        AuthentInitializeServiceClient proxy = new AuthentInitializeServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Initialisation"));
        //                        proxy.GetListeModuleCompleted += (sender, results) =>
        //                            {
        //                                if (results.Cancelled || results.Error != null)
        //                                {
        //                                    desableProgressBar();
        //                                    Message.Show(Galatee.Silverlight.Resources.Langue.wcf_error, Galatee.Silverlight.Resources.Langue.errorTitle);
        //                                    CancelButton.IsEnabled = OKButton.IsEnabled = true;
        //                                    passage = 0;
        //                                    LayoutRoot.Cursor = Cursors.Wait;
        //                                    return;
        //                                }

        //                                if (results.Result == null || results.Result.Count == 0)
        //                                {
        //                                    desableProgressBar();
        //                                    Message.Show(Galatee.Silverlight.Resources.Langue.wcf_no_profile_module,Galatee.Silverlight.Resources.Langue.informationTitle);
        //                                    CancelButton.IsEnabled = OKButton.IsEnabled = true;
        //                                    passage = 0;
        //                                    LayoutRoot.Cursor = Cursors.Wait;
        //                                    return;
        //                                }

        //                                /// call MVVM to create module treeNode
        //                                /// 
        //                                modules = results.Result;

        //                                AuthentInitializeServiceClient cls = new AuthentInitializeServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Initialisation"));
        //                                cls.RetourneListImpranteCompleted += (send, aa) =>
        //                                {
        //                                    if (aa.Cancelled || aa.Error != null)
        //                                    {
        //                                        CancelButton.IsEnabled = OKButton.IsEnabled = true;
        //                                        Message.Show(Galatee.Silverlight.Resources.Langue.wcf_error, Galatee.Silverlight.Resources.Langue.errorTitle);
        //                                        desableProgressBar();
        //                                        passage = 0;
        //                                        LayoutRoot.Cursor = Cursors.Wait;
        //                                        return;
        //                                        // 
        //                                    }

        //                                    if (aa.Result == null || aa.Result.Count == 0)
        //                                    {
        //                                        CancelButton.IsEnabled = OKButton.IsEnabled = true;
        //                                        Message.Show(Galatee.Silverlight.Resources.Langue.msgNodata, Galatee.Silverlight.Resources.Langue.informationTitle);
        //                                        desableProgressBar();
        //                                        passage = 0;
        //                                        LayoutRoot.Cursor = Cursors.Wait;
        //                                        return;
        //                                    }
        //                                    List<ServiceAuthenInitialize.CParametre> imprimante = new List<ServiceAuthenInitialize.CParametre>();
        //                                        imprimante = aa.Result;
        //                                        foreach (ServiceAuthenInitialize.CParametre impr in imprimante)
        //                                            SessionObject.Imprimantes.Add(impr.LIBELLE);


        //                                // Initialisation des libelle des frais de checques impayes et 
        //                                // de la nature du checque

        //                                //AuthentInitializeServiceClient client = new AuthentInitializeServiceClient();
        //                                //client.GetNatureByLibelleCourtCompleted += (sss, res) =>
        //                                //    {

        //                                //        if (res.Cancelled || res.Error != null)
        //                                //        {
        //                                //            string error = res.Error.Message;
        //                                //            MessageBox.Show("error occurs while invoking remote procedure", "GetNatureByLibelleCourt", MessageBoxButton.OK);
        //                                //            return;
        //                                //        }

        //                                //        if (res.Result == null)
        //                                //        {
        //                                //            MessageBox.Show("Parametrage de l'app impossible§ veuillez contacter l'admin");
        //                                //            return;
        //                                //        }

        //                                //        SessionObject.Enumere.LibNatureCheqImpaye = res.Result.NATURE;

        //                                        //AuthentInitializeServiceClient clis = new AuthentInitializeServiceClient();
        //                                        //clis.GetNatureByLibelleCourtCompleted += (env, ress) =>
        //                                        //  {

        //                                        //      if (ress.Cancelled || ress.Error != null)
        //                                        //        {
        //                                        //            string error = ress.Error.Message;
        //                                        //            MessageBox.Show("error occurs while invoking remote procedure", "GetNatureByLibelleCourtFraisChecImp", MessageBoxButton.OK);
        //                                        //            return;
        //                                        //        }

        //                                        //      if (ress.Result == null)
        //                                        //        {
        //                                        //            MessageBox.Show("Parametrage de l'app impossible! veuillez contacter l'admin");
        //                                        //            return;
        //                                        //        }

        //                                        //      SessionObject.Enumere.LibNatureFraisCheqImpaye = ress.Result.NATURE;
                                                    


        //                                        // enumeree de la procedure stockees 

        //                                        AuthentInitializeServiceClient prox = new AuthentInitializeServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Initialisation"));
        //                                        prox.returnEnumProcedureStockeeCompleted += (sa, resa) =>
        //                                            {

        //                                                if (resa.Cancelled || resa.Error != null)
        //                                                {
        //                                                    string error = resa.Error.Message;
        //                                                    Message.Show(Galatee.Silverlight.Resources.Langue.wcf_error, Galatee.Silverlight.Resources.Langue.errorTitle);
        //                                                    LayoutRoot.Cursor = Cursors.Wait;
        //                                                    return;
        //                                                }

        //                                                if (resa.Result == null)
        //                                                {
        //                                                    Message.Show(Galatee.Silverlight.Resources.Langue.msgNodata, Galatee.Silverlight.Resources.Langue.informationTitle);
        //                                                    LayoutRoot.Cursor = Cursors.Wait;
        //                                                    return;
        //                                                }

        //                                                SessionObject.EnumereProcedureStockee = resa.Result;

        //                                                completedAllRequest = true;
        //                                                desableProgressBar();
        //                                                LayoutRoot.Cursor = Cursors.Wait;
        //                                                success(this, new EventArgs());
        //                                                this.DialogResult = true;

        //                                            };
        //                                        prox.returnEnumProcedureStockeeAsync();  
        //                                        //  };
        //                                        //clis.GetNatureByLibelleCourtAsync(SessionObject.Enumere.LibNatureFraisCheqImpaye);
        //                                //    };
        //                                //client.GetNatureByLibelleCourtAsync(SessionObject.Enumere.LibNatureCheqImpaye);
        //                                 };
        //                                cls.RetourneListImpranteAsync();
        //                            };
        //                        proxy.GetListeModuleAsync(user);
                               
        //                    }

        //                }
        //                catch (Exception ex)
        //                {
        //                    Message.ShowError(ex.Message, Galatee.Silverlight.Resources.Langue.informationTitle);// throw;
        //                    CancelButton.IsEnabled = OKButton.IsEnabled = true;
        //                    passage = 0;
        //                    LayoutRoot.Cursor = Cursors.Wait;
        //                    desableProgressBar();
        //                }


        //            };
        //            service.RetourneInfoMatriculeConnecteAsync(txtLogin.Text, txtpwd.Password);
        //            //service.CloseAsync();
        //        }
              

        //    }
        //    catch (Exception ex)
        //    {

        //        Message.ShowError(ex.Message, Galatee.Silverlight.Resources.Langue.informationTitle);// throw;
        //        passage = 0;
        //    }
           
        //}

        void ObtenirDonneesConnection(ServiceAuthenInitialize.CsUtilisateur _leUser)
        {

            try
            {

                if (!string.IsNullOrEmpty(txtLogin.Text) && !string.IsNullOrEmpty(txtpwd.Password))
                {
                    allowProgressBar();


                    UserConnecte.PK_ID = _leUser.PK_ID;
                    UserConnecte.matricule = _leUser.MATRICULE;
                    UserConnecte.Centre = _leUser.CENTRE;
                    UserConnecte.nomUtilisateur = _leUser.LIBELLE;
                    UserConnecte.codefontion = _leUser.FONCTION;
                    UserConnecte.LibelleFonction = _leUser.LIBELLEFONCTION;
                    UserConnecte.PerimetreAction = _leUser.PERIMETREACTION;
                    UserConnecte.LibelleCentre = _leUser.LIBELLECENTRE;
                    UserConnecte.FK_IDFONCTION = _leUser.FK_IDFONCTION;
                    UserConnecte.listeProfilUser = _leUser.LESPROFILSUTILISATEUR;
                    UserConnecte.FK_IDCENTRE = _leUser.FK_IDCENTRE;
                    UserConnecte.numcaisse = SessionObject.LePosteCourant.NUMCAISSE;
                    // construction du menu des modules relatifs au profil du userconnecte

                    AuthentInitializeServiceClient proxy = new AuthentInitializeServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Initialisation"));
                    proxy.GetListeModuleCompleted += (sender, results) =>
                    {
                        if (results.Cancelled || results.Error != null)
                        {
                            desableProgressBar();
                            Message.Show(Galatee.Silverlight.Resources.Langue.wcf_error, Galatee.Silverlight.Resources.Langue.errorTitle);
                            CancelButton.IsEnabled = OKButton.IsEnabled = true;
                            passage = 0;
                            LayoutRoot.Cursor = Cursors.Wait;
                            return;
                        }

                        if (results.Result == null || results.Result.Count == 0)
                        {
                            desableProgressBar();
                            Message.Show(Galatee.Silverlight.Resources.Langue.wcf_no_profile_module, Galatee.Silverlight.Resources.Langue.informationTitle);
                            CancelButton.IsEnabled = OKButton.IsEnabled = true;
                            passage = 0;
                            LayoutRoot.Cursor = Cursors.Wait;
                            return;
                        }

                        /// call MVVM to create module treeNode
                        /// 
                        modules = results.Result;
                        CreateUserConnectedStore(modules);

                        completedAllRequest = true;
                        desableProgressBar();
                        LayoutRoot.Cursor = Cursors.Wait;
                        success(this, new EventArgs());
                        this.DialogResult = true;
                    };
                    proxy.GetListeModuleAsync(_leUser);

                }

            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Galatee.Silverlight.Resources.Langue.informationTitle);// throw;
                CancelButton.IsEnabled = OKButton.IsEnabled = true;
                passage = 0;
                LayoutRoot.Cursor = Cursors.Wait;
                desableProgressBar();
            }
        }
        System.Xml.Serialization.XmlSerializer serializerDstGroup = new System.Xml.Serialization.XmlSerializer(typeof(ServiceAuthenInitialize.CsDesktopGroup));
        void CreateUserConnectedStore(List<CsDesktopGroup> lstItem)
        {
            try
            {
                List<string> lst = new List<string>();
                foreach (CsDesktopGroup item in lstItem)
                {
                    string leMenu = string.Empty;
                    leMenu += item.CodeModule + "." + item.ProfilDesktopItem.PK_ID;
                    lst.Add(leMenu);
                };
                ServiceAuthenInitialize.CsDesktopGroup u = new ServiceAuthenInitialize.CsDesktopGroup()
                {
                    NOM = "UseurConnecr",
                    LstModuleIdProfil = lst
                };
                using (System.IO.IsolatedStorage.IsolatedStorageFile store = System.IO.IsolatedStorage.IsolatedStorageFile.GetUserStoreForSite())
                {
                    // The CreateFile() method creates a new file or overwrites an existing one.
                    using (System.IO.FileStream stream = store.CreateFile("desktop.desktop"))
                    {
                        // Store the person details in the file.
                        serializerDstGroup.Serialize(stream, u);
                    }
                };
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, Galatee.Silverlight.Resources.Langue.errorTitle + "CreateUserConnectedStore");// throw;
            }
        }
        void VerifierDonneesSaisiesModif()
        {
            // obtenir les informations de l'utilisateur connecté
            Galatee.Silverlight.ServiceAuthenInitialize.CsUtilisateur _utilisateur = new Galatee.Silverlight.ServiceAuthenInitialize.CsUtilisateur();
            Security.CsStrategieSecurite _security = new Security.CsStrategieSecurite();

            try
            {
                //client.GetNatureByLibelleCourtAsync(SessionObject.Enumere.LibNatureCheqImpaye);
                AuthentInitializeServiceClient getlogin = new AuthentInitializeServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Initialisation"));
                getlogin.GetByLoginNameCompleted += (send, result) =>
                {
                    try
                    {
                        if (result.Cancelled || result.Error != null)
                        {
                            string error = result.Error.Message;
                            Message.ShowError(error, Galatee.Silverlight.Resources.Langue.informationTitle);
                            passage = 0;
                            CancelButton.IsEnabled = OKButton.IsEnabled = true;

                            return;
                        }

                        if (result.Result == null)
                        {
                            Message.ShowInformation("Le login entré est inconnu du système.Veuillez réessayer svp!", Galatee.Silverlight.Resources.Langue.errorTitle);
                            passage = 0;
                            CancelButton.IsEnabled = OKButton.IsEnabled = true;

                            return;
                        }
                        else
                        {
                            // valorisation des objets csStrategie et csUtilisateur pour le controle de sécurité
                            user = result.Result;
                            Galatee.Silverlight.Security.CsStrategieSecurite strategie = new Security.CsStrategieSecurite();
                            strategie.ACTIF = SessionObject.securiteActive.ACTIF;
                            strategie.CHIFFREMENTREVERSIBLEPASSWORD = SessionObject.securiteActive.CHIFFREMENTREVERSIBLEPASSWORD;
                            strategie.DUREEMAXIMALEPASSWORD = SessionObject.securiteActive.DUREEMAXIMALEPASSWORD;
                            strategie.DUREEMINIMALEPASSWORD = SessionObject.securiteActive.DUREEMINIMALEPASSWORD;
                            strategie.DUREEVEUILLESESSION = SessionObject.securiteActive.DUREEVEUILLESESSION;
                            strategie.DUREEVERROUILLAGECOMPTE = SessionObject.securiteActive.DUREEVERROUILLAGECOMPTE;
                            strategie.HISTORIQUENOMBREPASSWORD = SessionObject.securiteActive.HISTORIQUENOMBREPASSWORD;
                            strategie.LIBELLE = SessionObject.securiteActive.LIBELLE;
                            strategie.LONGUEURMINIMALEPASSWORD = SessionObject.securiteActive.LONGUEURMINIMALEPASSWORD;
                            strategie.NEPASCONTENIRNOMCOMPTE = SessionObject.securiteActive.NEPASCONTENIRNOMCOMPTE;
                            strategie.NOMBREMAXIMALECHECSOUVERTURESESSION = SessionObject.securiteActive.NOMBREMAXIMALECHECSOUVERTURESESSION;
                            strategie.NOMBREMINIMALCARACTERESCHIFFRES = SessionObject.securiteActive.NOMBREMINIMALCARACTERESCHIFFRES;
                            strategie.NOMBREMINIMALCARACTERESMAJUSCULES = SessionObject.securiteActive.NOMBREMINIMALCARACTERESMAJUSCULES;
                            strategie.NOMBREMINIMALCARACTERENONALPHABETIQUES = SessionObject.securiteActive.NOMBREMINIMALCARACTERENONALPHABETIQUES;
                            strategie.PK_IDSTRATEGIESECURITE = SessionObject.securiteActive.PK_IDSTRATEGIESECURITE;
                            strategie.REINITIALISERCOMPTEURVERROUILLAGECOMPTEAPRES = SessionObject.securiteActive.REINITIALISERCOMPTEURVERROUILLAGECOMPTEAPRES;
                            strategie.TOUCHEVERROUILLAGESESSION = SessionObject.securiteActive.TOUCHEVERROUILLAGESESSION;
                            user.PK_ID = result.Result.PK_ID;
                            user.FK_IDCENTRE = result.Result.FK_IDCENTRE;
                            user.FK_IDFONCTION = result.Result.FK_IDFONCTION;

                            Galatee.Silverlight.Security.CsUtilisateur leUSer = ConvertionUtilisateur(user);
                            var resultat = Securities.VerifierDonneesConnexion(txtLogin.Text, txtpwd.Password, strategie, leUSer);
                            if (resultat != null)
                            {

                                user.DERNIERECONNEXIONREUSSIE = leUSer.DERNIERECONNEXIONREUSSIE;
                                user.NOMBREECHECSOUVERTURESESSION = leUSer.NOMBREECHECSOUVERTURESESSION;
                                user.FK_IDSTATUS = leUSer.FK_IDSTATUS;

                                if (resultat.Isupdated)
                                    UpdateUser(user, resultat.ex);
                                else
                                {
                                    if (resultat.ex == null) //pour les admins
                                    {
                                        if (user.INITUSERPASSWORD.Value == true)
                                        {
                                            ServiceAdministration.CsUtilisateur _user = ConvertionUtilisateurAdminSVC(user);
                                            _user.NOMBREECHECSOUVERTURESESSION = 0;
                                            _user.FK_IDSTATUS =SessionObject.Enumere.UserAcitveAccount;
                                            Administration.UcChangerPassword adm = new Administration.UcChangerPassword(_user, user, "");
                                            adm.Show();
                                        }
                                        else
                                        {
                                                ServiceAdministration.CsUtilisateur _user = ConvertionUtilisateurAdminSVC(user);
                                                _user.NOMBREECHECSOUVERTURESESSION  = 0;
                                                _user.FK_IDSTATUS = SessionObject.Enumere.UserAcitveAccount;
                                                Administration.UcChangerPassword adm = new Administration.UcChangerPassword(_user,user, "");
                                                adm.Show();
                                        }
                                    }
                                    else
                                    {
                                        if (user.INITUSERPASSWORD.Value == true)
                                        {
                                            ServiceAdministration.CsUtilisateur _user = ConvertionUtilisateurAdminSVC(user);
                                            _user.NOMBREECHECSOUVERTURESESSION = 0;
                                            _user.FK_IDSTATUS =SessionObject.Enumere.UserAcitveAccount;
                                            Administration.UcChangerPassword adm = new Administration.UcChangerPassword(_user,user, "", false);
                                            adm.Show();
                                        }
                                        else
                                            if (user.INITUSERPASSWORD .Value == true && resultat.ex.Message == null)
                                        {
                                            ServiceAdministration.CsUtilisateur _user = ConvertionUtilisateurAdminSVC(result.Result);
                                            _user.NOMBREECHECSOUVERTURESESSION = 0;
                                            _user.FK_IDSTATUS =SessionObject.Enumere.UserAcitveAccount;
                                            Administration.UcChangerPassword adm = new Administration.UcChangerPassword(_user,user, "", false);
                                            adm.Show();
                                        }
                                        else
                                            Message.ShowInformation(resultat.ex.Message, Galatee.Silverlight.Resources.Langue.informationTitle);
                                    }
                                   }
                                    passage = 0;
                            }
                            else
                                passage = 0;
                            // this.DialogResult = true;
                            LayoutRoot.Cursor = Cursors.Arrow;
                            CancelButton.IsEnabled = OKButton.IsEnabled = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        Message.ShowError(ex.Message, Galatee.Silverlight.Resources.Langue.errorTitle);
                        LayoutRoot.Cursor = Cursors.Arrow;
                        passage = 0;
                    }
                };
                getlogin.GetByLoginNameAsync(txtLogin.Text);


            }
            catch (Exception ex)
            {
                Message.Show(ex.Message, Galatee.Silverlight.Resources.Langue.errorTitle);
                LayoutRoot.Cursor = Cursors.Wait;
                passage = 0;
            }
        }
     
        void VerifierDonneesSaisies()
        { 
         // obtenir les informations de l'utilisateur connecté
             Galatee.Silverlight.ServiceAuthenInitialize.CsUtilisateur _utilisateur = new Galatee.Silverlight.ServiceAuthenInitialize.CsUtilisateur();
             Security.CsStrategieSecurite _security = new Security.CsStrategieSecurite();

             try
             {
                 //client.GetNatureByLibelleCourtAsync(SessionObject.Enumere.LibNatureCheqImpaye);
                 AuthentInitializeServiceClient getlogin = new AuthentInitializeServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Initialisation"));
                 getlogin.GetByLoginNameCompleted += (send, result) =>
                     {
                         try
                         {
                             if (result.Cancelled || result.Error != null)
                             {
                                 string error = result.Error.Message;
                                 Message.ShowError(error, Galatee.Silverlight.Resources.Langue.informationTitle);
                                 passage = 0;
                                 CancelButton.IsEnabled = OKButton.IsEnabled = true ;

                                 return;
                             }

                             if (result.Result == null)
                             {
                                 Message.ShowInformation("Le login entré est inconnu du système. Veuillez réessayer svp!", Galatee.Silverlight.Resources.Langue.errorTitle);
                                 passage = 0;
                                 CancelButton.IsEnabled = OKButton.IsEnabled = true ;

                                 return;
                             }
                             else
                             {
                                 user = result.Result;
                                 Galatee.Silverlight.Security.CsStrategieSecurite strategie = new Security.CsStrategieSecurite();
                                 strategie.ACTIF = SessionObject.securiteActive.ACTIF;
                                 strategie.CHIFFREMENTREVERSIBLEPASSWORD = SessionObject.securiteActive.CHIFFREMENTREVERSIBLEPASSWORD;
                                 strategie.DUREEMAXIMALEPASSWORD = SessionObject.securiteActive.DUREEMAXIMALEPASSWORD;
                                 strategie.DUREEMINIMALEPASSWORD = SessionObject.securiteActive.DUREEMINIMALEPASSWORD;
                                 strategie.DUREEVEUILLESESSION = SessionObject.securiteActive.DUREEVEUILLESESSION;
                                 strategie.DUREEVERROUILLAGECOMPTE = SessionObject.securiteActive.DUREEVERROUILLAGECOMPTE;
                                 strategie.HISTORIQUENOMBREPASSWORD = SessionObject.securiteActive.HISTORIQUENOMBREPASSWORD;
                                 strategie.LIBELLE = SessionObject.securiteActive.LIBELLE;
                                 strategie.LONGUEURMINIMALEPASSWORD = SessionObject.securiteActive.LONGUEURMINIMALEPASSWORD;
                                 strategie.NEPASCONTENIRNOMCOMPTE = SessionObject.securiteActive.NEPASCONTENIRNOMCOMPTE;
                                 strategie.NOMBREMAXIMALECHECSOUVERTURESESSION = SessionObject.securiteActive.NOMBREMAXIMALECHECSOUVERTURESESSION;
                                 strategie.NOMBREMINIMALCARACTERESCHIFFRES = SessionObject.securiteActive.NOMBREMINIMALCARACTERESCHIFFRES;
                                 strategie.NOMBREMINIMALCARACTERESMAJUSCULES = SessionObject.securiteActive.NOMBREMINIMALCARACTERESMAJUSCULES;
                                 strategie.NOMBREMINIMALCARACTERENONALPHABETIQUES = SessionObject.securiteActive.NOMBREMINIMALCARACTERENONALPHABETIQUES;
                                 strategie.PK_IDSTRATEGIESECURITE = SessionObject.securiteActive.PK_IDSTRATEGIESECURITE;
                                 strategie.REINITIALISERCOMPTEURVERROUILLAGECOMPTEAPRES = SessionObject.securiteActive.REINITIALISERCOMPTEURVERROUILLAGECOMPTEAPRES;
                                 strategie.TOUCHEVERROUILLAGESESSION = SessionObject.securiteActive.TOUCHEVERROUILLAGESESSION;
                                 user.PK_ID = result.Result.PK_ID;
                                 user.FK_IDCENTRE = result.Result.FK_IDCENTRE;
                                 user.FK_IDFONCTION  = result.Result.FK_IDFONCTION ;
                                 Galatee.Silverlight.Security.CsUtilisateur leUSer = ConvertionUtilisateur(user);

                                 Galatee.Silverlight.ServiceAdministration.CsUtilisateur leUtilisateurCourant = new Galatee.Silverlight.ServiceAdministration.CsUtilisateur();
                                 leUtilisateurCourant = ConvertionUtilisateurAdminSVC(result.Result);
                                 try
                                 {
                                     Cryptage.StrategieDeMotDePasse(strategie, txtLogin.Text, txtpwd.Password);
                                     
                                 }
                                 catch (Exception ex)
                                 {
                                     /** ZEG 15/09/2017 **/
                                         /*leUtilisateurCourant.USERCREATION = user.MATRICULE;
                                         leUtilisateurCourant.USERMODIFICATION = user.MATRICULE;


                                         var FrmGererUser = new Administration.UcChangerPassword(leUtilisateurCourant, user, "");
                                         FrmGererUser.Closed += new EventHandler(FrmGererUser_Closed);
                                         FrmGererUser.Show();*/
                                     
                                     Message.ShowInformation(ex.Message, "Connexion");
                                     CancelButton.IsEnabled = OKButton.IsEnabled = true;

                                     /** **/

                                     return;
                                 }
                                 var resultat = Securities.VerifierDonneesConnexion(txtLogin.Text, txtpwd.Password, strategie, leUSer);
                                 if (resultat != null)
                                 {
                                     user.DERNIERECONNEXIONREUSSIE = leUSer.DERNIERECONNEXIONREUSSIE;
                                     user.NOMBREECHECSOUVERTURESESSION = leUSer.NOMBREECHECSOUVERTURESESSION;
                                     user.FK_IDSTATUS = leUSer.FK_IDSTATUS;

                                     if (resultat.Isupdated)
                                         UpdateUser(user, resultat.ex);
                                     else
                                     {
                                         //FIFO
                                         if (resultat.ex == null) //pour les admins
                                             ObtenirDonneesConnection(user);
                                         else
                                             if (user.INITUSERPASSWORD.Value == true)
                                             {
                                                 ServiceAdministration.CsUtilisateur _user = ConvertionUtilisateurAdminSVC(user);
                                                 _user.NOMBREECHECSOUVERTURESESSION = 0;
                                                 _user.FK_IDSTATUS = SessionObject.Enumere.UserAcitveAccount;
                                                 Administration.UcChangerPassword adm = new Administration.UcChangerPassword(_user,user, "", false);
                                                 adm.Closed += new EventHandler(CloseUcChangePassword);
                                                 adm.Show();
                                             }
                                             else
                                             {
                                                 if (resultat.ex.Message == Galatee.Silverlight.Security.Langue.MsgPwdExpire)
                                                 {
                                                     var ws = new MessageBoxControl.MessageBoxChildWindow(Security.Langue.MsgChangePwd, resultat.ex.Message, MessageBoxControl.MessageBoxButtons.Ok , MessageBoxControl.MessageBoxIcon.Information);
                                                     ws.OnMessageBoxClosed += (l, results) =>
                                                     {
                                                         if (ws.Result == MessageBoxResult.OK)
                                                         {

                                                             ServiceAdministration.CsUtilisateur _user = ConvertionUtilisateurAdminSVC(user);
                                                             Administration.UcChangerPassword adm = new Administration.UcChangerPassword(_user, user, "");
                                                             adm.Closed += new EventHandler(CloseUcChangePassword);
                                                             adm.Show();
                                                         }
                                                     };
                                                     ws.Show();
                                                 }
                                                 else 
                                                     Message.ShowInformation(resultat.ex.Message, Galatee.Silverlight.Resources.Langue.errorTitle);
                                             }
                                         passage = 0;
                                     }
                                 }
                                 else
                                     passage = 0;
                                // this.DialogResult = true;
                                 LayoutRoot.Cursor = Cursors.Arrow;
                                 CancelButton.IsEnabled = OKButton.IsEnabled = true;
                                 
                                 #region 07/06/2016

                                 TraceDeConnection(user);

                                 #endregion
                             }
                         }
                         catch (Exception ex)
                         {
                             Message.ShowError(ex.Message, Galatee.Silverlight.Resources.Langue.errorTitle);
                             LayoutRoot.Cursor = Cursors.Arrow;
                             passage = 0;
                         }
                     };
                 getlogin.GetByLoginNameAsync(txtLogin.Text);

                
             }
             catch (Exception ex)
             {
               Message.Show(ex.Message, Galatee.Silverlight.Resources.Langue.errorTitle);
               LayoutRoot.Cursor = Cursors.Wait;
               passage = 0;
             }
        }

        private void FrmGererUser_Closed(object sender, EventArgs e)
        {
            //txtLogin.Text = ((UcReinitialisePwd)sender).userSelected.LOGINNAME;
            //txtpwd.Password = ((UcReinitialisePwd)sender).userSelected.PASSE;
        }
        #region 07/06/2016
        private void TraceDeConnection(ServiceAuthenInitialize.CsUtilisateur user)
        {
             
            AuthentInitializeServiceClient service = new AuthentInitializeServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Initialisation"));
            service.TraceDeConnectionCompleted += (send, result) =>
                {
                };
            service.TraceDeConnectionAsync(user.PK_ID,SessionObject.LePosteCourant.NOMPOSTE);
        }
        #endregion

        public static ServiceAdministration.CsUtilisateur ConvertionUtilisateurAdminSVC(Galatee.Silverlight.ServiceAuthenInitialize.CsUtilisateur user)
        {
            ServiceAdministration.CsUtilisateur _user = new ServiceAdministration.CsUtilisateur();
            if (user != null)
            {
                try
                {
                    _user.USERCREATION = user.USERCREATION;
                    _user.PK_ID = user.PK_ID;
                    _user.BRANCHE = user.BRANCHE;
                    _user.DATECREATION = user.DATECREATION;
                    _user.DATEDEBUTVALIDITE = user.DATEDEBUTVALIDITE;
                    _user.DATEDERNIERECONNEXION = DateTime.Now;
                    _user.DATEDERNIEREMODIFICATION = user.DATEDERNIEREMODIFICATION;
                    _user.DATEDERNIEREMODIFICATIONPASSWORD = user.DATEDERNIEREMODIFICATIONPASSWORD;
                    _user.DATEDERNIERVERROUILLAGE = user.DATEDERNIERVERROUILLAGE;
                    _user.DATEFINVALIDITE = user.DATEFINVALIDITE;
                    _user.DERNIERECONNEXIONREUSSIE = user.DERNIERECONNEXIONREUSSIE;
                    _user.LIBELLE = user.LIBELLE;
                    _user.ESTSUPPRIMER = user.ESTSUPPRIMER;
                    _user.E_MAIL = user.E_MAIL;
                    _user.EsADMIN = user.EsADMIN;
                    _user.CENTRE = user.CENTRE;
                    _user.FONCTION = user.FONCTION;
                    _user.INITUSERPASSWORD = user.INITUSERPASSWORD;
                    _user.LIBELLEFONCTION = user.LIBELLEFONCTION;
                    _user.LOGINNAME = user.LOGINNAME;
                    _user.NOMBREECHECSOUVERTURESESSION = user.NOMBREECHECSOUVERTURESESSION;
                    _user.PASSE = user.PASSE;
                    _user.PERIMETREACTION = user.PERIMETREACTION;
                    _user.MATRICULE = user.MATRICULE;
                    _user.PK_ID = user.PK_ID;
                    _user.LIBELLESTATUSCOMPTE = user.LIBELLESTATUSCOMPTE;
                    _user.FK_IDCENTRE = user.FK_IDCENTRE;
                    _user.FK_IDFONCTION = user.FK_IDFONCTION;
                    _user.FK_IDSTATUS = user.FK_IDSTATUS;
                    _user.PK_ID = user.PK_ID;
                    _user.TELEPHONE = user.TELEPHONE;
                    if (user.LESPROFILSUTILISATEUR != null)
                    {
                        foreach (Galatee.Silverlight.ServiceAuthenInitialize.CsProfil item in user.LESPROFILSUTILISATEUR)
                        {
                            if (_user.LESPROFILSUTILISATEUR == null) _user.LESPROFILSUTILISATEUR = new List<Galatee.Silverlight.ServiceAdministration.CsProfil>();
                            _user.LESPROFILSUTILISATEUR.Add(ConvertionProfilAdminSVC(item));
                        }
                    }

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return _user;
        }

        public static ServiceAdministration.CsProfil ConvertionProfilAdminSVC(Galatee.Silverlight.ServiceAuthenInitialize.CsProfil user)
        {
            ServiceAdministration.CsProfil _user = new ServiceAdministration.CsProfil();

            if (user != null)
            {
                try
                {
                    _user.PK_ID = user.PK_ID;
                    _user.CODE = user.CODE;
                    _user.CODEFONCTION = user.CODEFONCTION;
                    _user.DATECREATION = user.DATECREATION;
                    _user.DATEMODIFICATION = user.DATEMODIFICATION;
                    _user.FK_IDFONCTION = user.FK_IDFONCTION;
                    _user.LIBELLE = user.LIBELLE;
                    _user.MODULE = user.MODULE;
                    _user.USERCREATION = user.USERCREATION;
                    _user.USERMODIFICATION = user.USERMODIFICATION;
                    _user.FK_IDPROFIL = user.FK_IDPROFIL;
                    if (user.LESCENTRESPROFIL != null && user.LESCENTRESPROFIL.Count > 0)
                    {
                        foreach (Galatee.Silverlight.ServiceAuthenInitialize.CsCentreProfil item in user.LESCENTRESPROFIL)
                        {
                            if (_user.LESCENTRESPROFIL == null)
                                _user.LESCENTRESPROFIL = new List<ServiceAdministration.CsCentreProfil>();
                            _user.LESCENTRESPROFIL.Add(ConvertionCentreProfilAdminSVC(item));
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return _user;
        }

        public static ServiceAdministration.CsCentreProfil ConvertionCentreProfilAdminSVC(Galatee.Silverlight.ServiceAuthenInitialize.CsCentreProfil user)
        {
            ServiceAdministration.CsCentreProfil _user = new ServiceAdministration.CsCentreProfil();

            if (user != null)
            {
                try
                {
                    _user.PK_ID = user.PK_ID;
                    _user.DATEDEBUTVALIDITE = user.DATEDEBUTVALIDITE;
                    _user.DATEFINVALIDITE = user.DATEFINVALIDITE;
                    _user.FK_IDADMUTILISATEUR = user.FK_IDADMUTILISATEUR;
                    _user.FK_IDCENTRE = user.FK_IDCENTRE;
                    _user.FK_IDPROFIL = user.FK_IDPROFIL;
                }

                catch (Exception ex)
                {
                    throw ex;
                }

            }
            return _user;
        }

        public static ServiceAdministration.CsCentre ConvertionCentreAdminSVC(Galatee.Silverlight.ServiceAuthenInitialize.CsCentre user)
        {
            ServiceAdministration.CsCentre _user = new ServiceAdministration.CsCentre();

            if (user != null)
            {
                try
                {
                    _user.PK_ID = user.PK_ID;
                    _user.ADRESSE = user.ADRESSE;
                    _user.CODE = user.CODE;
                    _user.CODESITE = user.CODESITE;
                    _user.DATECREATION = user.DATECREATION;
                    _user.DATEMODIFICATION = user.DATEMODIFICATION;
                    _user.FK_IDCODESITE = user.FK_IDCODESITE;
                    _user.FK_IDNIVEAUTARIF = user.FK_IDNIVEAUTARIF;
                    _user.FK_IDTYPECENTRE = user.FK_IDTYPECENTRE;
                    _user.GESTIONAUTOAVANCECONSO = user.GESTIONAUTOAVANCECONSO;
                    _user.GESTIONAUTOFRAIS = user.GESTIONAUTOFRAIS;

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return _user;
        }

        private Galatee.Silverlight.Security.CsUtilisateur ConvertionUtilisateur(Galatee.Silverlight.ServiceAuthenInitialize.CsUtilisateur user)
        {
            try
            {
                Galatee.Silverlight.Security.CsUtilisateur _user = new Galatee.Silverlight.Security.CsUtilisateur();
                                         _user.USERCREATION = user.USERCREATION;
                                         _user.DATECREATION = user.DATECREATION ;
                                         //_user.NumCaisse = user.NumCaisse ;
                                         _user.PK_ID  = user.PK_ID ;
                                         _user.BRANCHE = user.BRANCHE ;
                                         _user.DATECREATION = user.DATECREATION;
                                         _user.DATEDEBUTVALIDITE = user.DATEDEBUTVALIDITE ;
                                         //_user.DATEDERNIERECONNEXION = DateTime.Now;
                                         _user.DATEDERNIERECONNEXION  = user.DATEDERNIERECONNEXION ;
                                         _user.DATEDERNIEREMODIFICATION = user.DATEDERNIEREMODIFICATION ;
                                         _user.DATEDERNIEREMODIFICATIONPASSWORD = user.DATEDERNIEREMODIFICATIONPASSWORD ;
                                         _user.DATEDERNIERVERROUILLAGE = user.DATEDERNIERVERROUILLAGE ;
                                         _user.DATEFINVALIDITE = user.DATEFINVALIDITE ;
                                         _user.DERNIERECONNEXIONREUSSIE = user.DERNIERECONNEXIONREUSSIE ;
                                         _user.LIBELLE = user.LIBELLE;
                                         _user.ESTSUPPRIMER = user.ESTSUPPRIMER;
                                         _user.E_MAIL = user.E_MAIL ;
                                         _user.EsADMIN = user.EsADMIN;
                                         _user.CENTRE = user.CENTRE  ;
                                         _user.FONCTION = user.FONCTION  ;
                                         _user.FK_IDSTATUS = user.FK_IDSTATUS;
                                         _user.INITUSERPASSWORD  = user.INITUSERPASSWORD ;
                                         _user.LIBELLEFONCTION = user.LIBELLEFONCTION ;
                                         _user.LOGINNAME = user.LOGINNAME;
                                         //_user.Nom = user.Nom;
                                         _user.NOMBREECHECSOUVERTURESESSION  = user.NOMBREECHECSOUVERTURESESSION ;
                                         //_user.NumCaisse = user.NumCaisse;
                                         _user.PASSE = user.PASSE;
                                         _user.PERIMETREACTION = user.PERIMETREACTION ;
                                         _user.MATRICULE = user.MATRICULE ;
                                         //_user.Prenoms = user.Prenoms;
                                         //_user.RoleDisplayName = user.RoleDisplayName;
                                         //_user.RoleID = user.FK_IDFONCTION ;
                                         _user.LIBELLESTATUSCOMPTE = user.LIBELLESTATUSCOMPTE;
                                         _user.PK_ID = user.PK_ID;
                                        //List<string> lst = new List<string>();
                                        //foreach (var item in user.LESPROFILSUTILISATEUR)
                                        //{
                                        //    lst.Add(item.CODEFONCTION);
                                        //}
                                        //_user.ListeDesIdProfil = new List<string>();
                                        //_user.ListeDesIdProfil.AddRange(lst);

                                         return _user;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void UpdateUser(Galatee.Silverlight.ServiceAuthenInitialize.CsUtilisateur pUser, Exception pException)
        {
            try
            {
                 Galatee.Silverlight.Security.CsUtilisateur _user = new Galatee.Silverlight.Security.CsUtilisateur();
                 Security.CsStrategieSecurite _security = new Security.CsStrategieSecurite();
                 ServiceAdministration.CsUtilisateur _admUser = new ServiceAdministration.CsUtilisateur();
                 bool isPremiereAuthent = false;
                 if (pUser.DATEDERNIERECONNEXION  == null)
                 {
                     pUser.DATEDERNIERECONNEXION = System.DateTime.Now ;
                     isPremiereAuthent = true;
                 }

                 AuthentInitializeServiceClient _updateUser = new AuthentInitializeServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Initialisation"));
                _updateUser.UpdateUserCompleted += (sendermaj, resultmaj) =>
                    {
                        try
                        {
                            if (resultmaj.Cancelled || resultmaj.Error != null)
                            {
                                string error = resultmaj.Error.Message;
                                Message.ShowError(error, Galatee.Silverlight.Resources.Langue.informationTitle);
                                passage = 0;
                                LayoutRoot.Cursor = Cursors.Wait;
                                return;
                            }

                            if (resultmaj.Result == false)
                            {
                                Message.ShowError(resultmaj.Error.Message, Galatee.Silverlight.Resources.Langue.errorTitle);
                                passage = 0;
                                LayoutRoot.Cursor = Cursors.Wait;
                                return;
                            }
                            else
                            {
                                if (pException != null)
                                {
                                    Message.ShowError(pException.Message, Galatee.Silverlight.Resources.Langue.errorTitle);
                                    passage = 0;
                                }
                                else
                                {
                                    passage = 0;

                                    _security = Utility.ParseObject(_security, SessionObject.securiteActive);
                                    _user = ConvertionUtilisateur(pUser);
                                    int nombreJour = Security.Securities.GetNombreJourAvantExpiration(_user, _security, passwordExpireJamais);


                                  if (!passwordExpireJamais)
                                  {
                                      //Si le mot de passe expire dans moins de dix jours
                                      //le changement de mot de passe est facultatif
                                   
                                      _admUser.BRANCHE  = _user.BRANCHE;
                                      _admUser.DATECREATION  = _user.DATECREATION ;
                                      _admUser.DATEDEBUTVALIDITE  = _user.DATEDEBUTVALIDITE;
                                      _admUser.DATEDERNIERECONNEXION = _user.DATEDERNIERECONNEXION ;
                                      _admUser.DATEDERNIEREMODIFICATION  = _user.DATEDERNIEREMODIFICATION;
                                      _admUser.DATEDERNIEREMODIFICATIONPASSWORD  = _user.DATEDERNIEREMODIFICATIONPASSWORD;
                                      _admUser.DATEDERNIERVERROUILLAGE  = _user.DATEDERNIERVERROUILLAGE;
                                      _admUser.DATEFINVALIDITE  = _user.DATEFINVALIDITE;
                                      _admUser.DERNIERECONNEXIONREUSSIE  = _user.DERNIERECONNEXIONREUSSIE;
                                      _admUser.LIBELLE = _user.LIBELLE;
                                      _admUser.E_MAIL  = _user.E_MAIL;
                                      _admUser.EsADMIN = _user.EsADMIN;
                                      _admUser.ESTSUPPRIMER = _user.ESTSUPPRIMER;
                                      _admUser.CENTRE     = _user.CENTRE;
                                      _admUser.FONCTION    = _user.FONCTION;
                                      _admUser.FK_IDSTATUS = _user.FK_IDSTATUS;
                                      _admUser.INITUSERPASSWORD = _user.INITUSERPASSWORD ;
                                      _admUser.LIBELLEFONCTION = _user.LIBELLEFONCTION;
                                      _admUser.LOGINNAME  = _user.LOGINNAME;
                                      _admUser.NOMBREECHECSOUVERTURESESSION = _user.NOMBREECHECSOUVERTURESESSION ;
                                      _admUser.USERCREATION = _user.USERCREATION;
                                      _admUser.PASSE = _user.PASSE;
                                      _admUser.PERIMETREACTION  = _user.PERIMETREACTION;
                                      _admUser.MATRICULE = _user.MATRICULE;
                                      _admUser.LIBELLESTATUSCOMPTE = _user.LIBELLESTATUSCOMPTE;
                                      _admUser.PK_ID = _user.PK_ID;
                                      _admUser.FK_IDCENTRE = pUser.FK_IDCENTRE;
                                      _admUser.FK_IDFONCTION = pUser.FK_IDFONCTION;
                                      _admUser.FK_IDSTATUS = pUser.FK_IDSTATUS;
                                      //_admUser.LESPROFILSUTILISATEUR = _user.ls;

                                      if (nombreJour <= 10 && nombreJour > 0)
                                      {
                                          var ws = new MessageBoxControl.MessageBoxChildWindow(Security.Langue.MsgChangePwd,string.Format(Security.Langue.MsgExpire,nombreJour.ToString()),MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Information);
                                          ws.OnMessageBoxClosed += (l, results) =>
                                          {
                                              if (ws.Result == MessageBoxResult.No)
                                              {
                                                  ObtenirDonneesConnection(pUser );
                                              }
                                              else if (ws.Result == MessageBoxResult.OK)
                                              {
                                                  Administration.UcChangerPassword adm = new Administration.UcChangerPassword(_admUser,user, "");
                                                  //FIFO
                                                  //adm.Closed += new EventHandler(CloseUcChangePassword);
                                                  adm.Show();
                                              }
                                          };
                                          ws.Show();

                                      }
                                      //Si le mot de passe a expiré
                                      //le changement de mot de passe est obligatoire
                                      else if (nombreJour <= 0 && _admUser.DATEDERNIERECONNEXION != null && !isPremiereAuthent  )
                                      {
                                          Administration.UcChangerPassword adm = new Administration.UcChangerPassword(_admUser,user, "");
                                          //FIFO
                                          //adm.Closed += new EventHandler(CloseUcChangePassword);
                                          adm.Show();
                                      }
                                      else
                                      {
                                          if (_admUser.INITUSERPASSWORD.Value)
                                          {
                                              Administration.UcChangerPassword adm = new Administration.UcChangerPassword(_admUser,user, "");
                                              //FIFO
                                              adm.Closed += new EventHandler(CloseUcChangePassword);
                                              adm.Show();
                                          }
                                          else if (!isModification)
                                              ObtenirDonneesConnection(pUser);
                                          else
                                          {
                                              Administration.UcChangerPassword adm = new Administration.UcChangerPassword(_admUser,user, "");
                                              //FIFO
                                              //adm.Closed += new EventHandler(CloseUcChangePassword);
                                              adm.Show();
                                          }

                                      }
                                  }

                                }
                                   // ObtenirDonneesConnection();
                                //this.DialogResult = true;

                            }
                        }
                        catch (Exception ex)
                        {
                            LayoutRoot.Cursor = Cursors.Wait;
                            Message.Show(ex.Message, Galatee.Silverlight.Resources.Langue.errorTitle);
                            passage = 0;
                        }
                    };
                _updateUser.UpdateUserAsync(pUser);
            }
            catch (Exception ex)
            {
                Message.Show(ex.Message, Galatee.Silverlight.Resources.Langue.errorTitle);
              passage = 0;
            }

        }


        void CloseUcChangePassword(object sender, EventArgs e)
        {
            try
            {
                Administration.UcChangerPassword adm = sender as Administration.UcChangerPassword;
                if (adm.IsPasswordChange)
                    //HtmlPage.Document.Submit();
                    ObtenirDonneesConnection(user);

                //if (adm.DialogResult.Value == true) // permet de tester si l'utilisateur a click sur Ok ou NON 
                //    throw new Exception(Galatee.Silverlight.Security.Langue.MsgChangePwd2);
            }
            catch (Exception ex)
            {
                Message.Show(ex.Message, Galatee.Silverlight.Resources.Langue.errorTitle);
            }
        }

        void ReinitialiserPassword(object sender, EventArgs e)
        {
            try
            {
                Administration.UcChangerPassword adm = sender as Administration.UcChangerPassword;
                if (adm.IsPasswordChange)
                    HtmlPage.Document.Submit();

                if (adm.DialogResult.Value == true) // permet de tester si l'utilisateur a click sur Ok ou NON 
                    throw new Exception(Galatee.Silverlight.Security.Langue.MsgChangePwd2);
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Galatee.Silverlight.Resources.Langue.errorTitle);
            }
        }
        void ConnexionSuccess(object sender, EventArgs e)
        {
            FrmCreeCaisse frm = sender as FrmCreeCaisse;
            //CaisseSelection.LIBELLE = frm.CaisseSelectionee;
            UserConnecte.CaisseSelect = frm.CaisseSelectionee;
            UserConnecte.MatriculeSelect = frm.MatriculeSelectionee;

       

        }

        private void ModifieButton_Click(object sender, RoutedEventArgs e)
        {

        }

        bool isModification = false;
        private void OKModifierPass_Click(object sender, RoutedEventArgs e)
        {
            LayoutRoot.Cursor = Cursors.Wait;
            if (passage == 0)
            {
                isModification = true;
                passage++;
                VerifierDonneesSaisiesModif(); //ObtenirDonneesConnection();
            }
        }
        

     //private void Content_FullScreenChanged(object sender, EventArgs e)
        //{

        //    if (Application.Current.Host.Content.IsFullScreen)
        //    {

        //        // Setto Screen resolution

        //        ScreenResolution.Height = Application.Current.Host.Content.ActualHeight;

        //        ScreenResolution.Width = Application.Current.Host.Content.ActualWidth;

        //    }

        //    else
        //    {

        //        // Resize controls to browser white window space

        //        // ScreenResolution.Height = double.Parse(HtmlPage.Document.Body.GetAttribute("clientHeight"));

        //        // ScreenResolution.Width = double.Parse(HtmlPage.Document.Body.GetAttribute("clientWidth"));

        //        ScreenResolution.Height = 300;

        //        ScreenResolution.Width = 640;

        //        chkFullScreen.IsChecked = false;

        //    }

        //}
    }

    public class ConfigClass
    {
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string Sexe { get; set; }
    }
}

