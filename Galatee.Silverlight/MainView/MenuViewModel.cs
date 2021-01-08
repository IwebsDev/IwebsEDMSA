using System;
using System.Net;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using System.Windows.Threading;
using System.ComponentModel;
using System.Windows.Navigation;
using System.Text;
//using Galatee.Silverlight.Devis;
using Galatee.Silverlight.Library;
using SilverlightCommands;
using System.Windows.Controls;
using System.IO;
//using Galatee.Silverlight.serviceWeb;
using Galatee.Silverlight.ServiceAuthenInitialize;
using System.Collections.Generic;
using Galatee.Silverlight.Caisse ;
using Galatee.Silverlight.ServiceCaisse;
using System.Windows.Browser;
using Galatee.Silverlight.ServiceAccueil;
using Galatee.Silverlight.Resources.Devis;


namespace Galatee.Silverlight.MainView
{
    public class MenuViewModel : INotifyPropertyChanged
    {
        Galatee.Silverlight.Library.MenuItem mvvmMenuItem;
        Galatee.Silverlight.Library.MenuItem subItem;
        Page innerpage;
        string imagesPath = "../Image/";
        string module = string.Empty;
        public MenuViewModel()
        {
            try
            {
                mvvmMenuItem = new Galatee.Silverlight.Library.MenuItem()
                   {
                       Name = "Root"
                   };

                var mnuFile = new Galatee.Silverlight.Library.MenuItem() { Name = "foperation", Text = "Operations" };
                var mnuEdit = new Galatee.Silverlight.Library.MenuItem() { Name = "mnuEdit", Text = "Edit" };
                var mnuWindow = new Galatee.Silverlight.Library.MenuItem() { Name = "mnuWindow", Text = "Window" };
                var mnuHelp = new Galatee.Silverlight.Library.MenuItem() { Name = "mnuHelp", Text = "Help" };

                var mnuNew = new Galatee.Silverlight.Library.MenuItem() { Name = "Galatee.Silverlight.Caisse.FrmEncaissement", Text = "Collection" };
                var mnuSeparator1 = new Galatee.Silverlight.Library.MenuItem() { Name = "mnuSeparator1", Text = "-" };
                var mnuOpenFile = new Galatee.Silverlight.Library.MenuItem() { Name = "Galatee.Silverlight.Caisse.FrmEncaissementManuel", Text = "Input Manuel Payment" };
                var mnuSaveFile = new Galatee.Silverlight.Library.MenuItem() { Name = "Galatee.Silverlight.Caisse.UCListeCodeRegroupement", Text = "@@@@@" };
                var mnuCloseFile = new Galatee.Silverlight.Library.MenuItem() { Name = "mnuClose", Text = "Close File", IsEnabled = false };
                var mnuSeparator2 = new Galatee.Silverlight.Library.MenuItem() { Name = "mnuSeparator2", Text = "-" };
                var mnuExit = new Galatee.Silverlight.Library.MenuItem() { Name = "mnuExit", Text = "Exit" };

                //var mnuNewFile = new Galatee.Silverlight.Library.MenuItem() { Name = "mnuNewFile", Text = "New File" };
                //var mnuNewProject = new Galatee.Silverlight.Library.MenuItem() { Name = "mnuNewProject", Text = "New Project" };
                //var mnuNewSolution = new Galatee.Silverlight.Library.MenuItem() { Name = "mnuNewSolution", Text = "New Solution" };

                var mnuCut = new Galatee.Silverlight.Library.MenuItem() { Name = "mnuCut", Text = "Cut" };
                var mnuCopy = new Galatee.Silverlight.Library.MenuItem() { Name = "mnuCopy", Text = "Copy" };
                var mnuPaste = new Galatee.Silverlight.Library.MenuItem() { Name = "mnuPaste", Text = "Paste" };
                var mnuDelete = new Galatee.Silverlight.Library.MenuItem() { Name = "mnuDelete", Text = "Delete" };

                var mnuWindow1 = new Galatee.Silverlight.Library.MenuItem() { Name = "mnuWindow1", Text = "Window 1", IsChecked = true, IsCheckable = true };
                var mnuWindow2 = new Galatee.Silverlight.Library.MenuItem() { Name = "mnuWindow2", Text = "Window 2", IsChecked = false, IsCheckable = true };
                var mnuWindow3 = new Galatee.Silverlight.Library.MenuItem() { Name = "mnuWindow3", Text = "Window 3", IsChecked = false, IsCheckable = true };

                var mnuAbout = new Galatee.Silverlight.Library.MenuItem() { Name = "mnuViewHelp", Text = "About Silverlight Menu" };

                //mnuNew.Add(mnuNewFile);
                //mnuNew.Add(mnuNewProject);
                //mnuNew.Add(mnuNewSolution);

                mnuFile.Add(mnuNew);
                mnuFile.Add(mnuSeparator1);
                mnuFile.Add(mnuOpenFile);
                mnuFile.Add(mnuSaveFile);
                mnuFile.Add(mnuCloseFile);
                mnuFile.Add(mnuSeparator2);
                mnuFile.Add(mnuExit);

                mnuEdit.Add(mnuCut);
                mnuEdit.Add(mnuCopy);
                mnuEdit.Add(mnuPaste);
                mnuEdit.Add(mnuDelete);

                mnuWindow.Add(mnuWindow1);
                mnuWindow.Add(mnuWindow2);
                mnuWindow.Add(mnuWindow3);

                mnuHelp.Add(mnuAbout);

                mvvmMenuItem.Add(mnuFile);
                mvvmMenuItem.Add(mnuEdit);
                mvvmMenuItem.Add(mnuWindow);
                mvvmMenuItem.Add(mnuHelp);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        ///  Get xml file tree for 
        ///  constructing menu Items node
        /// </summary>
        /// <param name="path"></param>
        //public MenuViewModel(Dictionary<CSMenuGalatee, List<TreeNodes>> dico, string assemblyName)
        //{
        //    mvvmMenuItem = new Galatee.Silverlight.Library.MenuItem()
        //    {
        //        Name = "Root"
        //    };

        //    // Construct menu tree view  from dictionnary data 

        //    foreach (KeyValuePair<CSMenuGalatee, List<TreeNodes>> menu in dico)
        //    {
        //        var mnuFile = new Galatee.Silverlight.Library.MenuItem() { Name = assemblyName + "." + menu.Key.MenuID.ToString(), Text = menu.Key.MenuText };
        //        if (menu.Value.Count != 0 && menu.Value != null)
        //            foreach (TreeNodes sousmenu in menu.Value) // pour chaque sous menu
        //                //if (sousmenu.Enfants != null && sousmenu.Enfants.Count != 0)
        //                mnuFile.Add(new Galatee.Silverlight.Library.MenuItem() { Name = assemblyName + "." + sousmenu.Tag.FormName, Text = sousmenu.Tag.MenuText });
        //        mvvmMenuItem.Add(mnuFile);
        //    }
        //}


        public MenuViewModel(Page main, Dictionary<ServiceAuthenInitialize.CSMenuGalatee, List<ServiceAuthenInitialize.TreeNodes>> dico, string assemblyName,string _module)
        {
            try
            {
                innerpage = main;
                SessionObject.moduleCourant = _module;
                mvvmMenuItem = new Galatee.Silverlight.Library.MenuItem()
                {
                    Name = "Root"
                };

                // Construct menu tree view  from dictionnary data 

                foreach (KeyValuePair<ServiceAuthenInitialize.CSMenuGalatee, List<ServiceAuthenInitialize.TreeNodes>> menu in dico)
                {
                    //Recupération du tdem
                    int? Tdem=null;

                    if ((menu.Value != null) && (menu.Value.Count != 0))
                    {
                        foreach (var item in menu.Value)
                        {
                            if (item.Tag.FormName == "UcInitialisation")
                            {
                                Tdem = item.Tag.Tdem;
                            }
                        }

                        var mnuHeader = new Galatee.Silverlight.Library.MenuItem() { Name = assemblyName + "." + menu.Key.MenuID.ToString(), Text = menu.Key.MenuText, Title = menu.Key.MenuID.ToString(), Module = menu.Key.Module, MenuID = menu.Key.MenuID, FormName = menu.Key.FormName, TypeReedition = menu.Key.TypeReedition, Tdem = Tdem };
                        if (menu.Value != null && menu.Value.Count != 0)
                            RecursifFunct(mnuHeader, menu.Value, assemblyName);
                        mvvmMenuItem.Add(mnuHeader);
                    }
                }
                //MessageBox.Show("fin MenuViewModel");
                // if(UserConnecte.codefontion == "001")
                //CreateOptionsMenu();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Galatee.Silverlight.Resources.Caisse.Langue.LibelleModule + "MenuViewModel");
                throw ex;
            }
        }

        void CreateOptionsMenu()
        {
            try
            {
                var mnuFile = new Galatee.Silverlight.Library.MenuItem() { Name = "options", Text = "Options" };

                var mnulangue = new Galatee.Silverlight.Library.MenuItem() { Name = "langue", Text = "Change Language" };
                var mnuSeparator1 = new Galatee.Silverlight.Library.MenuItem() { Name = "mnuSeparator1", Text = "-" };
                var mnuANG = new Galatee.Silverlight.Library.MenuItem() { Name = "anglais", Text = "English-US", IsCheckable = true };
                var mnuFRANC = new Galatee.Silverlight.Library.MenuItem() { Name = "francais", Text = "French-FR", IsCheckable = true };
                var mnuSeparator2 = new Galatee.Silverlight.Library.MenuItem() { Name = "mnuSeparator1", Text = "-" };
                var config = new Galatee.Silverlight.Library.MenuItem() { Name = "Galatee.Silverlight.MainView.Setting", Text = "Configure Urls" };

                mnuFile.Add(mnulangue);
                mnulangue.Add(mnuANG);
                mnulangue.Add(mnuSeparator1);
                mnulangue.Add(mnuFRANC);
                mnulangue.Add(config);

                mvvmMenuItem.Add(mnuFile);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Fonction recursive permet de construit un menu en profondeur 
        /// Conçue ,Testée et éprouvée par Hermann GBOGUE 
        /// Date 17/11/2012 Galatee
        /// </summary>
        /// <param name="mnuHeader"></param>
        /// <param name="headerItems"></param>
        void RecursifFunct(Galatee.Silverlight.Library.MenuItem mnuHeader, List<ServiceAuthenInitialize.TreeNodes> headerItems, string assemblyName)
        {
            try
            {
                
                if (headerItems != null && headerItems.Count > 0) // creation des items du menu principal
                {
                    mnuHeader.Iframe = HtmlPage.Document.GetElementById(SessionObject.ModuleEnCours + "_IFRAME");
                    mnuHeader.CloseButton = HtmlPage.Document.GetElementById(SessionObject.ModuleEnCours + "_IFRAME_INPUT");
                    int i = 0;
                    foreach (ServiceAuthenInitialize.TreeNodes headerItem in headerItems)
                    {
                        //MessageBox.Show(headerItem.Tag.FormName + " "+(++i).ToString());
                        subItem = new Galatee.Silverlight.Library.MenuItem()
                        {
                            Name = !string.IsNullOrEmpty(headerItem.Tag.FormName) ? assemblyName + "." + headerItem.Tag.FormName : assemblyName + "." + headerItem.Tag.MenuID,
                            Text = headerItem.Tag.MenuText,
                            Title = headerItem.Tag.MenuID.ToString(),
                            IsControl = headerItem.Tag.IsControl,
                            FormName = headerItem.Tag.FormName,
                            MenuID = headerItem.Tag.MenuID,
                            Tdem=headerItem.Tag.Tdem
                        };

                        if (headerItem.Enfants != null && headerItem.Enfants.Count > 0)
                        {
                            mnuHeader.Add(subItem);
                            RecursifFunct(subItem, headerItem.Enfants, assemblyName);// fonctin récursive pour la construction du menu avec sous menu
                        }
                        else
                            mnuHeader.Add(subItem);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public MenuViewModel(StringBuilder s)
        {
            try
            {
                mvvmMenuItem = new Galatee.Silverlight.Library.MenuItem()
                   {
                       Name = "Root"
                   };

                var mnuFile = new Galatee.Silverlight.Library.MenuItem() { Name = "foperation", Text = "Operpations" };
                var mnuEdit = new Galatee.Silverlight.Library.MenuItem() { Name = "mnuEdit", Text = "Edit" };
                var mnuWindow = new Galatee.Silverlight.Library.MenuItem() { Name = "mnuWindow", Text = "Window" };
                var mnuHelp = new Galatee.Silverlight.Library.MenuItem() { Name = "mnuHelp", Text = "Help" };

                var mnuNew = new Galatee.Silverlight.Library.MenuItem() { Name = "Galatee.Silverlight.Caisse.FrmEncaissement", Text = "Collection" };
                var mnuSeparator1 = new Galatee.Silverlight.Library.MenuItem() { Name = "mnuSeparator1", Text = "-" };
                var mnuOpenFile = new Galatee.Silverlight.Library.MenuItem() { Name = "Galatee.Silverlight.Caisse.UCListeCodeRegroupement", Text = "Input Payment" };
                var mnuSaveFile = new Galatee.Silverlight.Library.MenuItem() { Name = "mnuSave", Text = "Save File", IsEnabled = false };
                var mnuCloseFile = new Galatee.Silverlight.Library.MenuItem() { Name = "mnuClose", Text = "Close File", IsEnabled = false };
                var mnuSeparator2 = new Galatee.Silverlight.Library.MenuItem() { Name = "mnuSeparator2", Text = "-" };
                var mnuExit = new Galatee.Silverlight.Library.MenuItem() { Name = "mnuExit", Text = "Exit" };

                //var mnuNewFile = new Galatee.Silverlight.Library.MenuItem() { Name = "mnuNewFile", Text = "New File" };
                //var mnuNewProject = new Galatee.Silverlight.Library.MenuItem() { Name = "mnuNewProject", Text = "New Project" };
                //var mnuNewSolution = new Galatee.Silverlight.Library.MenuItem() { Name = "mnuNewSolution", Text = "New Solution" };

                var mnuCut = new Galatee.Silverlight.Library.MenuItem() { Name = "mnuCut", Text = "Cut" };
                var mnuCopy = new Galatee.Silverlight.Library.MenuItem() { Name = "mnuCopy", Text = "Copy" };
                var mnuPaste = new Galatee.Silverlight.Library.MenuItem() { Name = "mnuPaste", Text = "Paste" };
                var mnuDelete = new Galatee.Silverlight.Library.MenuItem() { Name = "mnuDelete", Text = "Delete" };

                var mnuWindow1 = new Galatee.Silverlight.Library.MenuItem() { Name = "mnuWindow1", Text = "Window 1", IsChecked = true, IsCheckable = true };
                var mnuWindow2 = new Galatee.Silverlight.Library.MenuItem() { Name = "mnuWindow2", Text = "Window 2", IsChecked = false, IsCheckable = true };
                var mnuWindow3 = new Galatee.Silverlight.Library.MenuItem() { Name = "mnuWindow3", Text = "Window 3", IsChecked = false, IsCheckable = true };

                var mnuAbout = new Galatee.Silverlight.Library.MenuItem() { Name = "mnuViewHelp", Text = "About Silverlight Menu" };

                //mnuNew.Add(mnuNewFile);
                //mnuNew.Add(mnuNewProject);
                //mnuNew.Add(mnuNewSolution);

                mnuFile.Add(mnuNew);
                mnuFile.Add(mnuSeparator1);
                mnuFile.Add(mnuOpenFile);
                mnuFile.Add(mnuSaveFile);
                mnuFile.Add(mnuCloseFile);
                mnuFile.Add(mnuSeparator2);
                mnuFile.Add(mnuExit);

                mnuEdit.Add(mnuCut);
                mnuEdit.Add(mnuCopy);
                mnuEdit.Add(mnuPaste);
                mnuEdit.Add(mnuDelete);

                mnuWindow.Add(mnuWindow1);
                mnuWindow.Add(mnuWindow2);
                mnuWindow.Add(mnuWindow3);

                mnuHelp.Add(mnuAbout);

                mvvmMenuItem.Add(mnuFile);
                mvvmMenuItem.Add(mnuEdit);
                mvvmMenuItem.Add(mnuWindow);
                mvvmMenuItem.Add(mnuHelp);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public Galatee.Silverlight.Library.MenuItem MVVMMenuItem
        {
            get
            {
                return mvvmMenuItem;
            }
            set
            {
                mvvmMenuItem = value;
                OnPropertyChanged("MVVMMenuItem");
            }
        }

        public string ImagesPath
        {
            get
            {
                return imagesPath;
            }
            set
            {
                imagesPath = value;
                OnPropertyChanged("ImagesPath");
            }
        }

        public ICommand MenuCommand
        {
            get { return new RelayCommand(p => DoMenuCommand(p)); }
        }

        public void DoMenuCommand(object param)
        {
            var menuItem = (Galatee.Silverlight.Library.MenuItem)param;
            try
            {
                // pour les item s concernant le changment de langue 

                if (menuItem.IsCheckable)
                { 
                  //innerpage.Resources[
                }
                if (menuItem.Count == 0 && !string.IsNullOrEmpty(menuItem.Name) && menuItem.IsEnabled)
                {
                    Type cwType = Type.GetType(menuItem.Name);
                    object cw = null;

                    if (!menuItem.IsControl)
                    {
                        if (((SessionObject.EtatCaisse == SessionObject.Enumere.EtatDeCaissePasCassier)||
                            (SessionObject.EtatCaisse == SessionObject.Enumere.EtatDeCaisseNonCloture &&
                            !SessionObject.IsCaisseOuverte)) &&
                            ((menuItem.Name == "Galatee.Silverlight.Caisse.FrmEncaissement") ||
                             (menuItem.Name == "Galatee.Silverlight.Caisse.FrmPaiementDemande") ||
                             (menuItem.Name == "Galatee.Silverlight.Caisse.FrmEncaissementRegroupement") ||
                             (menuItem.Name == "Galatee.Silverlight.Caisse.FrmDemandeTimbre") ||
                             (menuItem.Name == "Galatee.Silverlight.Caisse.FrmAnnulationEncaissement"))
                            )
                        {
                            if (SessionObject.EtatCaisse == SessionObject.Enumere.EtatDeCaissePasCassier)
                            {
                                Message.ShowInformation(Galatee.Silverlight.Resources.Caisse.Langue.MsgPasCaissier, Galatee.Silverlight.Resources.Caisse.Langue.LibelleModule);
                                return;
                            }
                            if (SessionObject.PosteNonCaisse)
                            {
                                Message.ShowInformation("Ce poste n'est pas un poste caisse", Galatee.Silverlight.Resources.Caisse.Langue.LibelleModule);
                                return;
                            }
                                //, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            else if (menuItem.Name != "Galatee.Silverlight.Caisse.UcCashClosure")
                            {
                                Message.ShowInformation(Galatee.Silverlight.Resources.Caisse.Langue.MsgCaisseNonCloture, Galatee.Silverlight.Resources.Caisse.Langue.LibelleModule);//, MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return;
                            }

                            if (SessionObject.EtatCaisse == SessionObject.Enumere.EtatDeCaisseNonCloture)
                            {
                                Message.ShowInformation(Galatee.Silverlight.Resources.Caisse.Langue.MsgCaisseNonCloture, Galatee.Silverlight.Resources.Caisse.Langue.LibelleModule);
                                return;
                            }
                        }
                        string caisse = UserConnecte.numcaisse;
                        string MatriculeCaisse = UserConnecte.matricule;
                        if (!string.IsNullOrEmpty(UserConnecte.CaisseSelect)) caisse = UserConnecte.CaisseSelect;
                        if (!string.IsNullOrEmpty(UserConnecte.MatriculeSelect)) MatriculeCaisse = UserConnecte.MatriculeSelect;

                        if (menuItem.Name == "Galatee.Silverlight.Caisse.frmEtatCaisse" ||
                            menuItem.Name == "Galatee.Silverlight.Caisse.ListeEncaissementJournalière")
                        {
                            List<string> lstDesProfilUser = new List<string>();
                            foreach (Galatee.Silverlight.ServiceAuthenInitialize.CsProfil item in UserConnecte.listeProfilUser)
                                lstDesProfilUser.Add(item.CODEFONCTION);
                            if (!lstDesProfilUser.Contains(SessionObject.Enumere.CodeFonctionCaisse))
                            {

                                FrmCreeCaisse frm = new FrmCreeCaisse("NON");
                                frm.Show();
                                return;
                            }
                            else
                            {
                                if (menuItem.Name == "Galatee.Silverlight.Caisse.ListeEncaissementJournalière")
                                {
                                    ListeDesTransactions(SessionObject.LaCaisseCourante);
                                    return;
                                }
                            }
                        }
                        else if (menuItem.Name == "Galatee.Silverlight.Caisse.FrmPaiementDemande" && menuItem.Title == "4002")
                            cw = Activator.CreateInstance(cwType, new string[] {"Oui"});

                        else if (menuItem.Name == "Galatee.Silverlight.Recouvrement.FrmAvisDeCoupureSGC" && menuItem.Title == "1300301")
                            cw = Activator.CreateInstance(cwType, new string[] { "OUI" });

                        else if (menuItem.Name == "Galatee.Silverlight.Caisse.FrmDuplicatEncaissement"
                           || menuItem.Name == "Galatee.Silverlight.Caisse.FrmAnnulationEncaissementManuel"
                           || menuItem.Name == "Galatee.Silverlight.Caisse.FrmAnnulationEncaissement")
                            cw = Activator.CreateInstance(cwType, new string[] { caisse });
                        else if (menuItem.Name == "Galatee.Silverlight.Accueil.UcInitialisation" ||
                                 menuItem.Name == "Galatee.Silverlight.Accueil.FrmAutreDemande" ||
                                 menuItem.Name == "Galatee.Silverlight.Accueil.FrmAbonReabonnement" ||
                                 menuItem.Name == "Galatee.Silverlight.Accueil.UcInitialisationEP" ||
                                 menuItem.Name == "Galatee.Silverlight.Accueil.UcInitialisationDepannage" ||
                                 menuItem.Name == "Galatee.Silverlight.Accueil.FrmInitRemboursementAvance" ||
                                 menuItem.Name == "Galatee.Silverlight.Accueil.FrmInitRemboursementTravaux" ||
                                 menuItem.Name == "Galatee.Silverlight.Accueil.FrmModificationAbonnement" ||
                                 menuItem.Name == "Galatee.Silverlight.Accueil.FrmModificationCompteur" ||
                                 menuItem.Name == "Galatee.Silverlight.Accueil.FrmModificationBranchement" ||
                                 menuItem.Name == "Galatee.Silverlight.Accueil.FrmModificationAdresse" ||
                                 menuItem.Name == "Galatee.Silverlight.Accueil.UcInformationsReclamation" ||
                                 menuItem.Name == "Galatee.Silverlight.Accueil.FrmModicationClient" ||
                                 menuItem.Name == "Galatee.Silverlight.Accueil.FrmCorrectionDeDonnees" ||
                                 menuItem.Name == "Galatee.Silverlight.Accueil.UcInitialisationTransition")
                            cw = Activator.CreateInstance(cwType, new string[] { retourneTypeDemandeSaisi(menuItem.Title),"OUI" });


                        else if (menuItem.Name == "Galatee.Silverlight.Facturation.FrmCalculFacturation")
                            cw = Activator.CreateInstance(cwType, new string[] { retourneTypeActionFacturation(menuItem.Title) });

                        else if (menuItem.Name == "Galatee.Silverlight.Report.FrmAvisEmisParPIA" ||
                                 menuItem.Name == "Galatee.Silverlight.Report.FrmAvisEmisParControleur" ||
                                 menuItem.Name == "Galatee.Silverlight.Report.FrmAvisEmisParRegroupement" ||
                                 menuItem.Name == "Galatee.Silverlight.Accueil.FrmListeDeAction" ||
                                 menuItem.Name == "Galatee.Silverlight.Report.FrmEtatCompteur" ||
                                 menuItem.Name == "Galatee.Silverlight.Report.FrmDevisValide" ||
                                 menuItem.Name == "Galatee.Silverlight.Report.FrmTauxDeRecouvrement" ||
                                 menuItem.Name == "Galatee.Silverlight.Facturation.FrmEtatFaturation" ||
                                 menuItem.Name == "Galatee.Silverlight.Accueil.FrmReeditionEtats" ||
                                 menuItem.Name == "Galatee.Silverlight.Recouvrement.FrmReediterEtat"||
                                 menuItem.Name == "Galatee.Silverlight.Report.FrmComptabilisation" ||
                                 menuItem.Name == "Galatee.Silverlight.Report.UcStatistiqueReclamation" ||
                                 menuItem.Name == "Galatee.Silverlight.Accueil.FrmReeditonProgramme" ||
                                 menuItem.Name == "Galatee.Silverlight.Report.FrmListeFacture" ||
                                 menuItem.Name == "Galatee.Silverlight.Accueil.FrmReliaisonCompteur" ||
                                 menuItem.Name == "Galatee.Silverlight.InterfaceComptable.FrmEtatComptat")
                            cw = Activator.CreateInstance(cwType, new string[] { retourneTypeReport(menuItem.Title) });


                        // Menu impayés
                        // 1 pour les impayes par categorie
                        // 2 pour les impayes par secteur
                        else if (menuItem.Name == "Galatee.Silverlight.Report.FrmImpayeCategory")
                        {
                            menuItem.Name = "Galatee.Silverlight.Report.FrmEditionImpayes";
                            cwType = Type.GetType(menuItem.Name);
                            cw = Activator.CreateInstance(cwType, new string[] { "1" });
                        }
                        else if (menuItem.Name == "Galatee.Silverlight.Report.FrmImpayeSector")
                        {
                            menuItem.Name = "Galatee.Silverlight.Report.FrmEditionImpayes";
                            cwType = Type.GetType(menuItem.Name);
                            cw = Activator.CreateInstance(cwType, new string[] { "2" });
                        }

                        else if ((menuItem.Name == "Galatee.Silverlight.Administration.FrmStrategiesSecurite"))
                        {
                            List<string> lstDesProfilUser = new List<string>();
                            foreach (Galatee.Silverlight.ServiceAuthenInitialize.CsProfil item in UserConnecte.listeProfilUser)
                                lstDesProfilUser.Add(item.CODEFONCTION);
                            if (!lstDesProfilUser.Contains("001"))
                            {
                                Message.ShowInformation("Cette action n'est possible que par l'administrateur", "Administration");
                                return;
                            }
                        }
                        if ((menuItem.Tdem != null ))
                        {
                            int index=menuItem.Name.Split('.').Length-1;
                            string UcName = menuItem.Name.Split('.')[index];
                            if (!string.IsNullOrWhiteSpace(UcName))
                            {
                                menuItem.Name = "Galatee.Silverlight.Devis.UcInitialisation";
                                cwType = Type.GetType(menuItem.Name);
                                cw = Activator.CreateInstance(cwType, new string[] { menuItem.Tdem.ToString() });
                            }
                            
                        }
                        if (cw == null)
                            cw = Activator.CreateInstance(cwType);
                        ChildWindow form = (ChildWindow)cw;
                        form.Title = menuItem.Text;
                        form.Tag = menuItem.Name.Split('.')[2];
                        SessionObject.ModuleEnCours = menuItem.Name.Split('.')[2];
                        SessionObject.IsChargerDashbord = false;
                        form.Show();
                    }
                    else
                    {
                        string[] namespaces = menuItem.Name.Split('.');
                        int longueur = namespaces.Length;
                        Utility.ShowPreviewFrame(namespaces[longueur - 1], namespaces[longueur - 2]);
                    }
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "MenuViewMode");
                //throw ex;
            }
        }
        private void ListeDesTransactions(CsHabilitationCaisse laCaisse)
        {
            int handler = LoadingManager.BeginLoading("Traitement en cours ...");
            try
            {

                 
                    DateTime debut = DateTime.Today;
                    DateTime fin = (DateTime.Today.AddDays(1));
                    CaisseServiceClient proxy = new CaisseServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Caisse"));
                    proxy.LitseDesTransactionAsync(laCaisse);
                    proxy.LitseDesTransactionCompleted += (senders, results) =>
                    {
                        LoadingManager.EndLoading(handler);
                        if (results.Cancelled || results.Error != null)
                        {
                            string error = results.Error.Message;
                            MessageBox.Show("errror occurs while calling remote method", "ReportListeEncaissements", MessageBoxButton.OK);
                            return;
                        }
                        if (results.Result == null || results.Result.Count == 0)
                        {
                            Message.ShowInformation("Aucune donnée trouvée", "Caisse");
                            return;
                        }

                        List<ServiceCaisse.CsLclient> dataTable = new List<ServiceCaisse.CsLclient>();
                        dataTable.AddRange(results.Result);

                        Dictionary<string, string> param = new Dictionary<string, string>();
                        param.Add("pUser", !string.IsNullOrWhiteSpace(SessionObject.LaCaisseCourante.MATRICULE) ? "Matricule : " + SessionObject.LaCaisseCourante.NOMCAISSE : "Matricule :Aucun");
                        param.Add("pDateDebut", "Date debut : " + debut.ToShortDateString());
                        param.Add("pDateFin", "Date fin : " + debut.ToShortDateString());

                        string key = Utility.getKey();
                        Utility.ActionDirectOrientation<ServicePrintings.CsLclient, ServiceCaisse.CsLclient>(dataTable, param, SessionObject.CheminImpression, "ListeDesTransactions".Trim(), "Caisse".Trim(),true );
                    };
            }
            catch (Exception ex)
            {
                LoadingManager.EndLoading(handler);

            }
            finally
            {

            }

        }
        void FrmReEditionDevis_Closed(object sender, EventArgs e)
        {
            //On test si le viewbox est non null et que c'est le Dashboard
            if (null != SessionObject.ViewBox)
            {
                if (SessionObject.ViewBox.Child is Galatee.Silverlight.Workflow.UcWKFDashBoard)
                {
                    ((Galatee.Silverlight.Workflow.UcWKFDashBoard)SessionObject.ViewBox.Child)
                        .RechargerDashboard();
                }
            }
        }

        /// <summary>
        /// Retourne le type de réedition pour la gestion du devis
        /// </summary>
        /// <param name="MenuID"></param>
        /// <returns></returns>
        private int retourneTypeReedition(int MenuID)
        {
            try
            {
                switch (MenuID)
                {
                    case 1800101:
                        return (int)DataReferenceManager.Reedition.Devis;
                    case 1800102:
                        return (int)DataReferenceManager.Reedition.BonDeSortie;
                    case 1800103:
                        return (int)DataReferenceManager.Reedition.BonTravaux;
                    case 1800104:
                        return (int)DataReferenceManager.Reedition.ProcesVerbal;
                    case 1800105:
                        return (int)DataReferenceManager.Reedition.BonEntreeStock;
                    case 1800106:
                        return (int)DataReferenceManager.Reedition.BonControle;
                    case 1800107:
                        return (int)DataReferenceManager.Reedition.RapportControle;
                    case 1800108:
                        return (int)DataReferenceManager.Reedition.Bilan;
                    default:
                        return 0;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private  string retourneTypeDemandeSaisi(string MenuID)
        {
            try
            {
                string _typeDemande = string.Empty;
                string caseSwitch = MenuID;
                switch (caseSwitch)
                {
                    case "800101":
                        _typeDemande = SessionObject.Enumere.BranchementSimple;
                        break;
                    case "800102":
                        _typeDemande = SessionObject.Enumere.BranchementAbonement;
                        break;
                    case "800103":
                        _typeDemande = SessionObject.Enumere.AbonnementSeul ;
                        break;
                    case "800104":
                        _typeDemande = SessionObject.Enumere.Reabonnement ;
                        break;
                    case "800105":
                        _typeDemande = SessionObject.Enumere.Resiliation ;
                        break;
                    case "800106":
                        _typeDemande = SessionObject.Enumere.RemboursementAvance ;
                        break;
                    case "800107":
                        _typeDemande = SessionObject.Enumere.RemboursementTrvxNonRealise  ;
                        break;
                    case "80010801":
                        _typeDemande = SessionObject.Enumere.ModificationAbonnement ;
                        break;
                    case "80010802":
                        _typeDemande = SessionObject.Enumere.ModificationCompteur ;
                        break;
                    case "80010803":
                        _typeDemande = SessionObject.Enumere.ModificationBranchement ;
                        break;
                    case "80010804":
                        _typeDemande = SessionObject.Enumere.ModificationClient ;
                        break;
                    case "80010805":
                        _typeDemande = SessionObject.Enumere.ModificationAdresse ;
                        break;
                    case "800109":
                        _typeDemande = SessionObject.Enumere.ReprisIndex ;
                        break;
                    case "800110":
                        _typeDemande = SessionObject.Enumere.ChangementCompteur ;
                        break;
                    case "800111":
                        _typeDemande = SessionObject.Enumere.Etalonage  ;
                        break;
                    case "800112":
                        _typeDemande = SessionObject.Enumere.AutorisationAvancementTravaux   ;
                        break;
                    case "800113":
                        _typeDemande = SessionObject.Enumere.RemboursementParticipation;
                        break;
                    case "800114":
                        _typeDemande = SessionObject.Enumere.VerificationCompteur   ;
                        break;
                    case "800115":
                        _typeDemande = SessionObject.Enumere.BranchementAbonnementEp;
                        break;
                    case "80011601":
                        _typeDemande = SessionObject.Enumere.DeposeCompteur ;
                        break;
                    case "800120":
                        _typeDemande = SessionObject.Enumere.AugmentationPuissance  ;
                        break;
                    case "800121":
                        _typeDemande = SessionObject.Enumere.DimunitionPuissance ;
                        break;
                    case "800122":
                        _typeDemande = SessionObject.Enumere.BranchementAbonementExtention ;
                        break;
                    case "800124":
                        _typeDemande = SessionObject.Enumere.DepannageEp;
                        break;
                    case "800125":
                        _typeDemande = SessionObject.Enumere.DemandeReclamation ;
                        break;
                case "800119":
                        _typeDemande = SessionObject.Enumere.TransfertSiteNonMigre;
                        break;
                case "800108":
                        _typeDemande = SessionObject.Enumere.CorrectionDeDonnes ;
                        break;
                case "800126":
                        _typeDemande = SessionObject.Enumere.ChangementProduit ;
                        break;
         
                }
                return _typeDemande;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private string retourneTypeActionFacturation(string MenuID)
        {
            try
            {
                string _typeFacturation = string.Empty;
                string caseSwitch = MenuID;
                switch (caseSwitch)
                {
                    case "30001":
                        _typeFacturation = SessionObject.Enumere.Normal  ;
                        break;
                    case "30002":
                        _typeFacturation = SessionObject.Enumere.Simulation  ;
                        break;
                    case "30003":
                        _typeFacturation = SessionObject.Enumere.Defacturation ;
                        break;
                    case "30004":
                        _typeFacturation = SessionObject.Enumere.DestructionSimulation ;
                        break;
                }
                return _typeFacturation;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private string retourneTypeReport(string MenuID)
        {
            try
            {
                string _typeDemande = string.Empty;
                string caseSwitch = MenuID;
                switch (caseSwitch)
                {
                    case "21":
                        _typeDemande = SessionObject.DevisValiderDelais;
                        break;
                    case "22":
                        _typeDemande = SessionObject.DevisValiderHorsDelais;
                        break;
                    case "31":
                        _typeDemande = SessionObject.TravauxValiderDelais;
                        break;
                    case "32":
                        _typeDemande = SessionObject.TravauxValiderHorsDelais;
                        break;
                    case "33":
                        _typeDemande = SessionObject.TravauxRealiser;
                        break;
                    case "35":
                        _typeDemande = SessionObject.TravauxNonRealiser;
                        break;
                    case "5":
                        _typeDemande = SessionObject.DemandeEnAttenteLiaison;
                        break;
                    case "400011":
                        _typeDemande = SessionObject.AvisEmis ;
                        break;
                    case "400012":
                        _typeDemande = SessionObject.AvisCoupe ;
                        break;
                    case "400015":
                        _typeDemande = SessionObject.AvisRecouvre  ;
                        break;
                     case "400013":
                        _typeDemande = SessionObject.AvisRepose;
                        break;
                     case "400016":
                        _typeDemande = SessionObject.TauxRecouvrement;
                        break;
                     case "400017":
                        _typeDemande = SessionObject.TauxEncaissement;
                        break;
                     case "900010":
                        _typeDemande = SessionObject.ListePreavis;
                        break;
                     case "900011":
                        _typeDemande = SessionObject.MontantEncaisseControleur;
                        break;
                     case "900012":
                        _typeDemande = SessionObject.ListeMandatement;
                        break;
                     case "900013":
                        _typeDemande = SessionObject.ListePaiementMandat;
                        break;
                     case "900014":
                        _typeDemande = SessionObject.TauxMandatemant;
                        break;
                     case "900015":
                        _typeDemande = SessionObject.TauxPaiementMandat ;
                        break;
                     case "900016":
                        _typeDemande = SessionObject.EmissionRegroupement ;
                        break;
                     case "700011":
                        _typeDemande = SessionObject.AvanceSurConsomation ;
                        break;
                     case "700012":
                        _typeDemande = SessionObject.EncaissementReversement  ;
                        break;
                     case "700013":
                        _typeDemande = SessionObject.EncaissementModeRegement   ;
                        break;
                      case "700014":
                        _typeDemande = SessionObject.Vente   ;
                        break;
                      case "300001":
                        _typeDemande = SessionObject.PrepaidSansAchatPeriode;
                        break;
                      case "300002":
                        _typeDemande = SessionObject.PrepaidSansJamaisAchat;
                        break;
                      case "400010101":
                        _typeDemande = SessionObject.ReeditionAccuser ;
                        break;
                      case "400010102":
                        _typeDemande = SessionObject.ReeditionContrat  ;
                        break;
                      case "1510":
                        _typeDemande = SessionObject.ReeditionCampagne;
                        break;
                      case "1511":
                        _typeDemande = SessionObject.ReeditionMandatement;
                        break;
                      case "1512":
                        _typeDemande = SessionObject.ReeditionPaiement;
                        break;
                      case "1513":
                        _typeDemande = SessionObject.ReeditionMiseAJour;
                        break;
                      case "50010":
                        _typeDemande = SessionObject.ComptaFacturation;
                        break;
                      case "50011":
                        _typeDemande = SessionObject.RecapComptaFacturation ;
                        break;
                      case "50012":
                        _typeDemande = SessionObject.StatfacturationStat;
                        break;
                      case "50001":
                        _typeDemande = SessionObject.Statfacturation;
                        break;
                      case "50013":
                        _typeDemande = SessionObject.StatVenteCummuler;
                        break;
                      case "30502":
                        _typeDemande = SessionObject.AbonneNonConstituer;
                        break;
                      case "30503":
                        _typeDemande = SessionObject.AbonneNonSaisie;
                        break;
                      case "30504":
                        _typeDemande = SessionObject.AbonneSaisieNonFact ;
                        break;
                      case "30505":
                        _typeDemande = SessionObject.AbonneFactureNonMaj;
                        break;
                      case "50007":
                        _typeDemande = SessionObject.CompteurFacturePeriode;
                        break;
                      case "400010104":
                        _typeDemande = SessionObject.ReeditionProgramme ;
                        break;
                      case "400010105":
                        _typeDemande = SessionObject.ReeditionSortieCompteur ;
                        break;
                      case "400010106":
                        _typeDemande = SessionObject.ReeditionSortieMateriel ;
                        break;
                      case "50009":
                        _typeDemande = SessionObject.FactureAnnuler;
                        break;
                      case "50008":
                        _typeDemande = SessionObject.FactureIsole;
                        break;
                      case "50015":
                        _typeDemande = SessionObject.EncaissementCumule ;
                        break;
                      case "400018":
                        _typeDemande = SessionObject.TourneePIA;
                        break;
                      case "10000101":
                        _typeDemande = SessionObject.StatReclamation;
                        break;
                      case "10000102":
                        _typeDemande = SessionObject.ReclamationAgent;
                        break;
                      case "10000103":
                        _typeDemande = SessionObject.ReclamationListe;
                        break;
                      case "10000104":
                        _typeDemande = SessionObject.TauxReclamation;
                        break;
                      case "150001":
                        _typeDemande = SessionObject.ReliaisonCompteur ;
                        break;
                      case "150002":
                        _typeDemande = SessionObject.DeliaisonCompteur;
                        break;
                      case "150003":
                        _typeDemande = SessionObject.CorrectionNumCompteur;
                        break;
                      case "20011":
                        _typeDemande = SessionObject.AvanceSurConso;
                        break;
                      case "20012":
                        _typeDemande = SessionObject.ExtractionImpayes;
                        break;
                      case "7":
                        _typeDemande = SessionObject.RegistreDemande;
                        break;
                      case "8":
                        _typeDemande = SessionObject.DemandeEnAttenteDeRealisation;
                        break;
                        
                }
                return _typeDemande;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
 
