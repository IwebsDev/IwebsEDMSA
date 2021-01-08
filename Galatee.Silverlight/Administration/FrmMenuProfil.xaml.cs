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
using Galatee.Silverlight.ServiceAdministration;
using System.Collections.ObjectModel;
using Galatee.Silverlight.Classes;
using Galatee.Silverlight.Library;
using Galatee.Silverlight.Resources;
using UpdateControls.XAML;
using Galatee.Silverlight.Library.Models;
using Galatee.Silverlight.Library.ViewModels;

namespace Galatee.Silverlight.Administration
{
    public partial class FrmMenuProfil : ChildWindow
    {
        public FrmMenuProfil()
        {
            InitializeComponent();
            DataContext = ForView.Wrap(new DesignTimeData().Root);
            GetAllFonction();
            GetAllProfil();
            this.txtCode.Focus();
        }

        public FrmMenuProfil(CsProfil profil, string exec)
        {
            InitializeComponent();
            DataContext = ForView.Wrap(new DesignTimeData().Root);
            //GetAllFonction();
            GetProfilData( profil,  exec);
        }

        void GetProfilData(CsProfil profil, string exec)
        {
            

            if (exec == "update")
            {
                //load fonctions
                GetAllFonction();

                //enable controls
                //save
            }
            else
            {
                if (exec == "display")
                {
                    //point to its fonction
                    //disable controls
                    // close without save
                }
            }

        }

        bool isProfilSelected = false;

        List<CsFonction> DonneesFonctions = new List<CsFonction>(); 
        List<CsProfil> DonneesProfils = new List<CsProfil>();

        List<string> idMenuRecurv = new List<string>();
        List<string> idMenu = new List<string>();
        List<string> idProgram = new List<string>();
        List<string> idModule = new List<string>();
        List<CsProgramMenu> ListeProgramMenu = new List<CsProgramMenu>();
        List<CsAdmMenu> CurrentProfilProgramMenu = new List<CsAdmMenu>();
        ObservableCollection<Module> modules = null;
        CsFonction ta = null;// profil selectionne
        OptionViewModel viewModel = null;
        CsProfil leprofil = new CsProfil();

        CsProfil leprofilSelect = new CsProfil();
        List<CsAdmMenu> lesMenusDuProfilSelect = new List<CsAdmMenu>();
        List<CsAdmMenu> listeAllMenuFonctionSelect = new List<CsAdmMenu>();

        void DesactiverControle(bool status)
        {
            cbo_profile.IsEnabled = status;
            OKButton.IsEnabled = status;
            Main.IsEnabled = status;
        }

        private void ActiverOK()
        {
            List<CsHabilitationProgram> ObtenirHabProf = ObtenirHabilitationProfil(viewModel.Children);

            if (txtCode.Text != "" && txtLibelle.Text != "" && ObtenirHabProf.Count > 0)
            {
                OKButton.IsEnabled = true;
            }
        }

        void ValidaterAttributionProfil()
        {
            //leprofilSelect = dataGrid_ListeProfil.SelectedItem as CsProfil;

            List<CsHabilitationProgram> ObtenirHabilProfil = ObtenirHabilitationProfil(viewModel.Children);

            if (ObtenirHabilProfil.Count > 0)
            {
                CsProfil _currentProfil = new CsProfil()
                {
                    FK_IDFONCTION = ta.PK_ID,
                    LIBELLE = txtLibelle.Text,
                    CODE = txtCode.Text
                };

                bool trouver = false;

                foreach (CsProfil item in DonneesProfils)
                {
                    if (item.CODE == _currentProfil.CODE)
                    {
                        trouver = true;
                    }
                    //break;
                }

                if (leprofilSelect != null)
                {
                    if (leprofilSelect.PK_ID.ToString() != "")
                    {
                        _currentProfil.PK_ID = leprofilSelect.PK_ID;
                    }
                }

                leprofil = _currentProfil;

                if (cbo_profile.SelectedItem == null)
                {
                    Message.ShowInformation(Galatee.Silverlight.Resources.Administration.Langue.MsgSelectProfilBeforeCompleteTask, Langue.informationTitle);
                    //OKButton.IsEnabled = true;
                    return;
                }


                    if (!trouver)
                    {

                        AdministrationServiceClient insertHabil = new AdministrationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Administration"));
                        //DesactiverControle(false);

                        insertHabil.saveProfilHabilitationCompleted += (insers, resultins) =>
                        {
                            try
                            {
                                if (resultins.Cancelled || resultins.Error != null)
                                {
                                    string error = resultins.Error.Message;
                                    Message.ShowInformation(error, Langue.errorTitle);
                                    //DesactiverControle(true);
                                    //OKButton.IsEnabled = true;
                                    return;
                                }

                                if (resultins.Result == false)
                                {
                                    Message.ShowInformation(Galatee.Silverlight.Resources.Administration.Langue.MsgSettingProfilFailed, Langue.informationTitle);
                                    //DesactiverControle(true);
                                    return;
                                }
                                else
                                    if (resultins.Result == true)
                                    {
                                        Message.ShowInformation(Galatee.Silverlight.Resources.Administration.Langue.MsgSettingProfilSuccess, Langue.informationTitle);


                                        //DesactiverControle(true);
                                        txtCode.Text = string.Empty;
                                        txtLibelle.Text = string.Empty;
                                        CheckTreeviewParProfils();
                                        return;
                                    }
                            }
                            catch (Exception ex)
                            {
                                //OKButton.IsEnabled = true;
                                Message.ShowError(ex.Message, Langue.informationTitle);
                            }
                        };
                        insertHabil.saveProfilHabilitationAsync(_currentProfil, ObtenirHabilProfil);
                    }
                    else
                    {
                        // nouvelle creation avec meme code
                        if (leprofilSelect == null)
                        {
                            Message.Show("Un profil de code " + _currentProfil.CODE + " existe déjà. Veuillez saisir un autre code.", Langue.errorTitle);
                        }

                            OKButton.IsEnabled = false;
                            var w = new MessageBoxControl.MessageBoxChildWindow(Galatee.Silverlight.Resources.Langue.ConfirmationTitle, Galatee.Silverlight.Resources.Administration.Langue.confirmHabilitationMsg, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Information);
                            w.OnMessageBoxClosed += (_, result) =>
                            {
                                if (w.Result == MessageBoxResult.OK)
                                {
                        // modification avec meme code
                        if (leprofilSelect != null)
                        {

                            AdministrationServiceClient insertHabil = new AdministrationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Administration"));
                            //DesactiverControle(false);

                            insertHabil.saveProfilHabilitationCompleted += (insers, resultins) =>
                            {
                                try
                                {
                                    if (resultins.Cancelled || resultins.Error != null)
                                    {
                                        string error = resultins.Error.Message;
                                        Message.ShowInformation(error, Langue.errorTitle);
                                        //DesactiverControle(true);
                                        //OKButton.IsEnabled = true;
                                        return;
                                    }

                                    if (resultins.Result == false)
                                    {
                                        Message.ShowInformation(Galatee.Silverlight.Resources.Administration.Langue.MsgSettingProfilFailed, Langue.informationTitle);
                                        //DesactiverControle(true);
                                        return;
                                    }
                                    else
                                        if (resultins.Result == true)
                                        {
                                            Message.ShowInformation(Galatee.Silverlight.Resources.Administration.Langue.MsgSettingProfilSuccess, Langue.informationTitle);
                                            //DesactiverControle(true);
                                            txtCode.Text = string.Empty;
                                            txtLibelle.Text = string.Empty;
                                            CheckTreeviewParProfils();
                                            return;
                                        }
                                }
                                catch (Exception ex)
                                {
                                    //OKButton.IsEnabled = true;
                                    Message.ShowError(ex.Message, Langue.informationTitle);
                                }
                            };
                            insertHabil.saveProfilHabilitationAsync(_currentProfil, ObtenirHabilProfil);
                        }

                        //end
                                }
                                else
                                    OKButton.IsEnabled = true;
                            };
                            w.Show();  
                    }

            }
            else
                Message.Show("Veuillez Habilliter le profil .", Langue.errorTitle);
        }

        void OKButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
            //    OKButton.IsEnabled = false;
            //    var w = new MessageBoxControl.MessageBoxChildWindow(Galatee.Silverlight.Resources.Langue.ConfirmationTitle, Galatee.Silverlight.Resources.Administration.Langue.confirmHabilitationMsg, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Information);
            //    w.OnMessageBoxClosed += (_, result) =>
            //    {
            //        if (w.Result == MessageBoxResult.OK)
            //        {
                if (this.txtCode.Text != "" && this.txtLibelle.Text != "")
                {
                        ValidaterAttributionProfil();
                        GetAllProfil();
                        CheckTreeviewParProfils();
                }
                else
                    Message.Show("Veuillez saisir le code et/ou le libellé du profil .", Langue.errorTitle);

                //    }
                //    else
                //        OKButton.IsEnabled = true;
                //};
                //w.Show();
            }
            catch (Exception ex)
            {
                //OKButton.IsEnabled = true;
                Message.ShowError(ex, Langue.informationTitle);
            }

        }

        List<CsHabilitationProgram> ObtenirHabilitationProfil(IEnumerable<OptionViewModel> m)
        {
            List<CsHabilitationProgram> habilitationProfil = new List<CsHabilitationProgram>();
            try
            {
                foreach (OptionViewModel module in m)
                {
                    CsHabilitationProgram modules = new CsHabilitationProgram();

                    foreach (Option program in module.Option.Children)
                    {
                        if (program.Children != null && program.Children.Count() > 0)
                            RecursiveHabilitation(module, program, program.Children, habilitationProfil);
                    }
                }

                // seconde phase : elle consiste a trouver tous les menu parents des sous menu cocher de maniere recursive

                habilitationProfil.AddRange(ObtenirMainMenu(habilitationProfil));
                return habilitationProfil;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        List<CsHabilitationProgram> ObtenirMainMenu(List<CsHabilitationProgram> habilitationProfil)
        {
            List<string> saveMainMenu = new List<string>();
            List<CsHabilitationProgram> _habilitationProfil = new List<CsHabilitationProgram>();
            try
            {
                foreach (CsHabilitationProgram hbp in habilitationProfil)
                {
                    // get parent_direct
                    CsProgramMenu main = ListeProgramMenu.Where(p => p.PK_MENUID == hbp.FK_IDMENU).FirstOrDefault();

                    // si_parent_direct_est_un_fils
                    if (!string.IsNullOrEmpty(main.MAINMENUID.ToString()))
                    {
                        // si_parent_direct_nexiste_pas_dans_liste
                        if (!saveMainMenu.Contains(main.MAINMENUID.ToString()))
                        {
                            //l_ajouter : ne_rien_ajouter
                            //saveMainMenu.Add(main.MAINMENUID.ToString());
                            ////_habilitationProfil.Add(new CsHabilitationProgram() { CODEFONCTION = ta.CODE, FK_IDGROUPPROGRAM = hbp.FK_IDGROUPPROGRAM, FK_IDPROGRAM = hbp.FK_IDPROGRAM, FK_IDMENU = main.MAINMENUID, USERCREATION = UserConnecte.matricule, DATECREATION = DateTime.Today.Date/*, PK_ID= main.PK_ID */});
                            //remonter_si_parent_direct_a_parent
                            RecursiveOtenirMainMenu(main.MAINMENUID.ToString(), _habilitationProfil, saveMainMenu);
                        }
                        //si_parent_direct_existe_dans_liste:
                        //ne rien faire
                    }

                    // si est_un_pere
                    //else
                    //{

                    //}
                }
                return _habilitationProfil;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        void RecursiveOtenirMainMenu(string pMenu, List<CsHabilitationProgram> habilitationProfil, List<string> saveMainMenu)
        {
            try
            {
                CsProgramMenu main = ListeProgramMenu.Where(p => p.PK_MENUID.ToString() == pMenu).FirstOrDefault();

                if (main != null)
                {
                    if (!string.IsNullOrEmpty(main.PK_MENUID.ToString()))
                    {
                        if (!saveMainMenu.Contains(main.PK_MENUID.ToString()))
                        {
                            saveMainMenu.Add(main.PK_MENUID.ToString());

                        //if (!saveMainMenu.Contains(main.MAINMENUID.ToString()))
                        //{
                        //    saveMainMenu.Add(main.MAINMENUID.ToString());

                            // decommenter by HGB on 05/08/2015

                            habilitationProfil.Add(new CsHabilitationProgram() { CODEFONCTION = ta.CODE, FK_IDGROUPPROGRAM = main.FK_IDGROUPPROGRAM, FK_IDMENU = main.PK_MENUID, USERCREATION = UserConnecte.matricule, DATECREATION = DateTime.Today.Date });
                            if (!string.IsNullOrEmpty(main.MAINMENUID.ToString()))
                            {
                                RecursiveOtenirMainMenu(main.MAINMENUID.ToString(), habilitationProfil, saveMainMenu);
                            }
                        }
                    }
                }
               
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        List<CsHabilitationProgram> ObtenirHabilitationProfil(ObservableCollection<Module> m)
        {
            List<CsHabilitationProgram> habilitationProfil = new List<CsHabilitationProgram>();
            try
            {
                foreach (Module module in m)
                {
                    CsHabilitationProgram modules = new CsHabilitationProgram();

                    foreach (Program program in module.Programs)
                    {
                        if (program.Menus != null && program.Menus.Count > 0)
                            RecursiveHabilitation(module, program, program.Menus, habilitationProfil);
                    }
                }
                return habilitationProfil;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void RecursiveHabilitation(OptionViewModel module, Option program, IEnumerable<Option> menu, List<CsHabilitationProgram> listeH)
        {
            try
            {
                if (menu != null && menu.Count() > 0)
                {
                    foreach (Option men in menu)
                    {

                        if (men.Children != null && men.Children.Count() > 0)
                            RecursiveHabilitation(module, program, men.Children, listeH);
                        else
                        {
                            OptionLeaf _men = (OptionLeaf)men;
                            if (_men.Selected)
                            {
                                listeH.Add(new CsHabilitationProgram() { CODEFONCTION = ta.CODE, FK_IDGROUPPROGRAM = program.IdGroupProgram, /*FK_IDPROGRAM = program.PrgramID,*/ FK_IDMENU = men.MenuID,/* PK_ID = program.PkId,*/ USERCREATION = UserConnecte.matricule, DATECREATION = DateTime.Today.Date });
                            }
                        }
                    }
                }
                else
                {
                    OptionLeaf _men = (OptionLeaf)program;
                    if (_men.Selected)
                    {
                        listeH.Add(new CsHabilitationProgram() { CODEFONCTION = ta.CODE, FK_IDGROUPPROGRAM = program.IdGroupProgram, /*FK_IDPROGRAM = program.PrgramID,*/ FK_IDMENU = program.MenuID,/*PK_ID = program.PkId,*/  USERCREATION = UserConnecte.matricule, DATECREATION = DateTime.Today.Date });
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        //ajouter les menus peres /**/

        void RecursiveHabilitation(Module module, Program program, ObservableCollection<Menu> menu, List<CsHabilitationProgram> listeH)
        {
            if (menu != null && menu.Count > 0)
            {
                foreach (Menu men in menu)
                {
                    if (men.Check)
                    {
                        ////listeH.Add(new CsHabilitationProgram() { CODEFONCTION = ta.CODE, FK_IDGROUPPROGRAM = module.IdModule, FK_IDPROGRAM = program.prgramID, FK_IDMENU = men.MenuID });
                    }
                    if (men.MenuItem != null && men.MenuItem.Count > 0)
                        RecursiveHabilitation(module, program, men.MenuItem, listeH);
                }
            }
        }

        void RetourneProgramme()
        {
            //Obtenir les donnees de l'arborescence des modules , programmes et des menus relatifs
            AdministrationServiceClient prgram = new AdministrationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Administration"));
            prgram.RetourneAllModuleProgramCompleted += (sprog, resprog) =>
            {
                try
                {
                    if (resprog.Result == null || resprog.Result.Count == 0)
                    {
                        Message.ShowInformation(Langue.msgNodata, Langue.informationTitle);
                        return;
                    }
                    ListeProgramMenu.AddRange(resprog.Result);
                    SessionObject.ListeProgramMenu = resprog.Result;

                    //Creation du menu treeview de l'habilitaion 
                    modules = new ObservableCollection<Module>();
                    OptionGroup MainGrp = CreationHabilitationTrees(ListeProgramMenu);
                    viewModel = new Library.ViewModels.OptionViewModel(MainGrp);

                    DataContext = ForView.Wrap(viewModel);

                    // reactivation du button de validation
                    OKButton.IsEnabled = true;
                }
                catch (Exception ex)
                {
                    Message.ShowError(ex.Message, Langue.errorTitle);
                }

            };
            prgram.RetourneAllModuleProgramAsync();

        }

        void GetAllProfil()
        {
            // desactivation du treeview des modules
            //Main.IsEnabled = false;
            //OKButton.IsEnabled = false;

            try
            {
                DonneesProfils = new List<CsProfil>();
                    AdministrationServiceClient client = new AdministrationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Administration"));
                    client.RetourneListeAllProfilUserCompleted += (ss, res) =>
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
                                Message.ShowInformation(Langue.msgNodata, Langue.informationTitle);
                                return;
                            }

                            DonneesProfils.AddRange(res.Result);

                            GetDataListProfildeFonctionAll();

                        }
                        catch (Exception ex)
                        {
                            Message.ShowError(ex.Message, Langue.informationTitle);
                        }

                    };
                    client.RetourneListeAllProfilUserAsync();
                }
            //}
            catch (Exception ex)
            {
                throw ex;
            }

        }

        void GetData()
        {
            // desactivation du treeview des modules
            Main.IsEnabled = false;
            OKButton.IsEnabled = false;

            try
            {
                if (SessionObject.ListeFonction != null && SessionObject.ListeFonction.Count != 0)
                {
                    DonneesFonctions = SessionObject.ListeFonction;
                    cbo_profile.ItemsSource = DonneesFonctions.OrderBy(t=>t.ROLENAME).ToList();
                    cbo_profile.SelectedValuePath = "CODE";
                    cbo_profile.DisplayMemberPath = "ROLENAME";

                    if (SessionObject.ListeProgramMenu != null && SessionObject.ListeProgramMenu.Count != 0)
                    {
                        ListeProgramMenu = SessionObject.ListeProgramMenu;
                        //Creation du menu treeview de l'habilitaion 
                        modules = new ObservableCollection<Module>();
                        OptionGroup MainGrp = CreationHabilitationTrees(ListeProgramMenu);
                        viewModel = new Library.ViewModels.OptionViewModel(MainGrp);
                        DataContext = ForView.Wrap(viewModel);
                        // reactivation du button de validation
                        OKButton.IsEnabled = true;
                    }
                }
                else
                {
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
                                Message.ShowInformation(Langue.msgNodata, Langue.informationTitle);
                                return;
                            }
                            SessionObject.ListeFonction = res.Result;
                            DonneesFonctions.AddRange(res.Result);
                            cbo_profile.ItemsSource = res.Result;
                            cbo_profile.SelectedValuePath = "CODE";
                            cbo_profile.DisplayMemberPath = "ROLENAME";
                            RetourneProgramme();

                        }
                        catch (Exception ex)
                        {
                            Message.ShowError(ex.Message, Langue.informationTitle);
                        }

                    };
                    client.SELECT_All_FonctionAsync();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        void GetAllFonction()
        {
            // desactivation du treeview des modules
            Main.IsEnabled = false;
            OKButton.IsEnabled = false;

            try
            {
                if (SessionObject.ListeFonction != null && SessionObject.ListeFonction.Count > 0)
                {
                    DonneesFonctions = SessionObject.ListeFonction;

                    CsFonction premier = new CsFonction();
                    premier.CODE = "PREMIER";
                    premier.ROLENAME = "TOUTES LES FONCTIONS";
                    

                    bool estpremier = false;


                    foreach (var item in DonneesFonctions)
                    {
                        if (item.CODE == "PREMIER" && item.ROLENAME == "TOUTES LES FONCTIONS")
                        {
                            estpremier = true;
                        }

                    }

                    if (!estpremier)
                    {
                        DonneesFonctions.Add(premier);
                    }


                    cbo_profile.ItemsSource = DonneesFonctions;
                    cbo_profile.SelectedValuePath = "CODE";
                    cbo_profile.DisplayMemberPath = "ROLENAME";

                }
                else
                {
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
                                Message.ShowInformation(Langue.msgNodata, Langue.informationTitle);
                                return;
                            }
                            //SessionObject.ListeFonction = res.Result;

                            DonneesFonctions.AddRange(res.Result);

                            CsFonction premier = new CsFonction();
                            premier.CODE = "PREMIER";
                            premier.ROLENAME = "TOUTES LES FONCTIONS";

                            DonneesFonctions.Add(premier);

                            SessionObject.ListeFonction = DonneesFonctions;

                            //cbo_profile.ItemsSource = res.Result;
                            cbo_profile.ItemsSource = DonneesFonctions;
                            cbo_profile.SelectedValuePath = "CODE";
                            cbo_profile.DisplayMemberPath = "ROLENAME";
                            //RetourneProgramme();

                        }
                        catch (Exception ex)
                        {
                            Message.ShowError(ex.Message, Langue.informationTitle);
                        }

                    };
                    client.SELECT_All_FonctionAsync();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        void GetDataListProfildeFonctionAll()
        {
            List<CsProfil> lstp = new List<CsProfil>();
            if (DonneesProfils.Count > 0)
            {
                lstp.AddRange(DonneesProfils);
                List<CsProfil> vide = new List<CsProfil>();
                dataGrid_ListeProfil.ItemsSource = vide;
                dataGrid_ListeProfil.ItemsSource = lstp;

            }
            else
                Message.Show("Veuillez fermer et relancer la page", Langue.errorTitle);
        }

        void GetDataListProfildeFonction(CsFonction tat)
        {
            if(DonneesProfils.Count > 0)
            {

                List<CsProfil> lstp = new List<CsProfil>();

                if (tat.CODE == "PREMIER")
                {
                    lstp.AddRange(DonneesProfils);
                }
                else
                {
                    foreach (CsProfil item in DonneesProfils)
                    {
                        if (item.FK_IDFONCTION == tat.PK_ID)
                        {
                            lstp.Add(item);
                        }
                    }
                }
                List<CsProfil> vide = new List<CsProfil>();
                dataGrid_ListeProfil.ItemsSource = vide;

                if (lstp.Count>0)
                    dataGrid_ListeProfil.ItemsSource = lstp;
                //else
                //    Message.Show("La fonction " + tat.ROLENAME + " n'a aucun profil", Langue.errorTitle);

            }
        }

        void GetDataCsAdmMEnu(List<CsAdmMenu> listeMenu)
        {
            // desactivation du treeview des modules
            //Main.IsEnabled = false;
            OKButton.IsEnabled = false;
            List<CsProgramMenu> program = new List<CsProgramMenu>();
            try
            {
                foreach (var item in listeMenu)
                {
                    CsProgramMenu menu = new CsProgramMenu()
                    {

                        FK_IDPROGRAM = item.FK_IDMODULE,
                        PK_MENUID = item.PK_ID,
                        MAINMENUID = item.MAINMENUID,
                        MENUTEXT = item.MENUTEXT,
                        LIBELLE = item.MENUTEXT

                    };
                    program.Add(menu);
                }


                ListeProgramMenu = program;
                //Creation du menu treeview de l'habilitaion 
                modules = new ObservableCollection<Module>();
                OptionGroup MainGrp = CreationHabilitationTrees(ListeProgramMenu);
                viewModel = new Library.ViewModels.OptionViewModel(MainGrp);
                DataContext = ForView.Wrap(viewModel);
                // reactivation du button de validation
                OKButton.IsEnabled = true;
                // viewModel.IsChecked = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        void GetDataCsAdmMEnuCocher(List<CsAdmMenu> listeMenu, List<CsAdmMenu>  listecocher)
        {
            // desactivation du treeview des modules
            //Main.IsEnabled = false;
            OKButton.IsEnabled = false;
            List<CsProgramMenu> program = new List<CsProgramMenu>();
            try
            {
                foreach (var item in listeMenu)
                {
                    CsProgramMenu menu = new CsProgramMenu()
                    {

                        FK_IDPROGRAM = item.FK_IDMODULE,
                        PK_MENUID = item.PK_ID,
                        MAINMENUID = item.MAINMENUID,
                        MENUTEXT = item.MENUTEXT,
                        LIBELLE = item.MENUTEXT

                    };

                    program.Add(menu);
                }

                //foreach (CsProgramMenu item in program)
                //{
                //    foreach (CsProgramMenu itemcocher in listecocher)
                //    {
                //        if (item.PK_MENUID == itemcocher.PK_MENUID)
                //        {

                //        }
                //    }
                //}

                ListeProgramMenu = program;
                //Creation du menu treeview de l'habilitaion 
                modules = new ObservableCollection<Module>();
                OptionGroup MainGrp = CreationHabilitationTrees(ListeProgramMenu);

                //CocherTreeviewDuProfil(viewModel.Children, reshab.Result);

                viewModel = new Library.ViewModels.OptionViewModel(MainGrp);

                CocherTreeviewDuProfilSelect(viewModel.Children, listecocher);

                List<CsHabilitationProgram> ObtenirHabProf = ObtenirHabilitationProfil(viewModel.Children);

                DataContext = ForView.Wrap(viewModel);
                // reactivation du button de validation
                OKButton.IsEnabled = true;
                // viewModel.IsChecked = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        void RecursiveMenuTree(Menu menu, List<CsProgramMenu> menuList)
        {

            try
            {
                if (menuList != null && menuList.Count > 0)
                    foreach (CsProgramMenu men in menuList)
                    {
                        if (!idMenuRecurv.Contains(men.PK_MENUID.ToString()))
                        {
                            idMenuRecurv.Add(men.PK_MENUID.ToString());
                            Menu m = new Menu();
                            m.Name = men.MENUTEXT;
                            m.programID = men.ID;
                            m.MainMenuID = men.MAINMENUID;
                            m.MenuID = men.PK_MENUID;
                            List<CsProgramMenu> main = ListeProgramMenu.Where(p => p.MAINMENUID == men.PK_MENUID && p.ID == men.ID).ToList();
                            if (main != null && main.Count > 0)
                            {
                                menu.MenuItem.Add(m);
                                RecursiveMenuTree(m, main);
                            }
                            else
                                if (main.Count == 0)
                                    menu.MenuItem.Add(m);

                        }
                    }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        void RecursiveMenuTree(OptionGroup grpParent, OptionGroup grp, OptionLeaf leaf, List<CsProgramMenu> menuList)
        {

            try
            {
                if (menuList != null && menuList.Count > 0) // si sous menu courant contient des enfants ( ex Collection contient des sous collections)
                {
                    grpParent.Add(grp);

                    foreach (CsProgramMenu men in menuList) // entrée des enfants du sous menu courant
                    {
                        if (!idMenuRecurv.Contains(men.PK_MENUID.ToString()))
                        {
                            idMenuRecurv.Add(men.PK_MENUID.ToString());
                            OptionLeaf mLeaf = new OptionLeaf(men.MENUTEXT, null, men.ID, men.MAINMENUID, men.PK_MENUID, men.ID, men.USERCREATION, men.DATECREATION); // instance du premier enfant de la sous collection 
                            OptionGroup mGrp = new OptionGroup(men.MENUTEXT, null, men.ID, men.MAINMENUID, men.PK_MENUID, men.ID, men.USERCREATION, men.DATECREATION);

                            // liste des enfants du sous menu courant
                            List<CsProgramMenu> main = ListeProgramMenu.Where(p => p.MAINMENUID == men.PK_MENUID && p.ID == men.ID).ToList();
                            RecursiveMenuTree(grp, mGrp, mLeaf, main);
                        }
                    }
                }
                else
                    grpParent.Add(leaf);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        void CreateMenuTree(List<CsProgramMenu> MainMenu, Program program)
        {

            foreach (CsProgramMenu prog in MainMenu)
            {
                if (!idMenu.Contains(prog.PK_MENUID.ToString()))
                {
                    idMenu.Add(prog.PK_MENUID.ToString());

                    Menu menu = new Menu();
                    menu.moduleID = prog.FK_IDGROUPPROGRAM;
                    menu.Name = prog.MENUTEXT;
                    menu.MenuID = prog.PK_MENUID;
                    menu.MainMenuID = string.IsNullOrEmpty(prog.MAINMENUID.ToString()) ? null : prog.MAINMENUID;

                    program.Menus.Add(menu);
                    List<CsProgramMenu> Menu = ListeProgramMenu.Where(p => p.MAINMENUID == prog.PK_MENUID && p.ID == prog.ID).ToList();
                    RecursiveMenuTree(menu, Menu);
                }
            }
        }

        void CreateMenuTree(List<CsProgramMenu> MainMenu, OptionGroup program)
        {

            try
            {
                foreach (CsProgramMenu prog in MainMenu) // entrée des menu principaux
                {
                    if (!idMenu.Contains(prog.PK_MENUID.ToString()))
                    {
                        idMenu.Add(prog.PK_MENUID.ToString());

                        int? mainM = string.IsNullOrEmpty(prog.MAINMENUID.ToString()) ? null : prog.MAINMENUID;
                        OptionLeaf _menuLeaf = new OptionLeaf(prog.MENUTEXT, prog.FK_IDGROUPPROGRAM, null, mainM, prog.PK_MENUID, prog.ID, prog.USERCREATION, prog.DATECREATION); // instance du menu principal courant ( ex Operation)
                        OptionGroup _menuGroup = new OptionGroup(prog.MENUTEXT, prog.FK_IDGROUPPROGRAM, null, mainM, prog.PK_MENUID, prog.ID, prog.USERCREATION, prog.DATECREATION); // instance du menu principal courant
                        // liste des sous menus relatifs au menu principal courant ( ex  Collection,Input payment,Close of cash etc...)
                        List<CsProgramMenu> Menu = ListeProgramMenu.Where(p => p.MAINMENUID == prog.PK_MENUID && p.ID == prog.ID).ToList();
                        if (Menu != null && Menu.Count > 0)
                            RecursiveMenuTree(program, _menuGroup, _menuLeaf, Menu);
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        void CreateMenuTreeAdmMenu(List<CsAdmMenu> MainMenu, OptionGroup program)
        {

            try
            {
                foreach (CsAdmMenu prog in MainMenu) // entrée des menu principaux
                {
                    if (!idMenu.Contains(prog.PK_ID.ToString()))
                    {
                        idMenu.Add(prog.PK_ID.ToString());

                        int? mainM = string.IsNullOrEmpty(prog.MAINMENUID.ToString()) ? null : prog.MAINMENUID;
                        OptionLeaf _menuLeaf = new OptionLeaf(prog.MENUTEXT, prog.FK_IDMODULE, null, mainM, prog.PK_ID, prog.PK_ID, prog.USERCREATION, prog.DATECREATION); // instance du menu principal courant ( ex Operation)
                        OptionGroup _menuGroup = new OptionGroup(prog.MENUTEXT, prog.FK_IDMODULE, null, mainM, prog.PK_ID, prog.PK_ID, prog.USERCREATION, prog.DATECREATION); // instance du menu principal courant
                        // liste des sous menus relatifs au menu principal courant ( ex  Collection,Input payment,Close of cash etc...)
                        List<CsProgramMenu> Menu = ListeProgramMenu.Where(p => p.MAINMENUID == prog.PK_ID && p.ID == prog.PK_ID).ToList();
                        if (Menu != null && Menu.Count > 0)
                            RecursiveMenuTree(program, _menuGroup, _menuLeaf, Menu);
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        void CreateProgramTree(List<CsProgramMenu> programs, Module Modules)
        {
            try
            {
                List<Program> Programs = new List<Program>();

                foreach (CsProgramMenu prog in programs)
                {
                    if (!idProgram.Contains(prog.ID.ToString()))
                    {
                        idProgram.Add(prog.ID.ToString());

                        Program prgram = new Program();
                        prgram.moduleID = Modules.IdModule;
                        prgram.Name = prog.ProgName;
                        prgram.prgramID = prog.ID;
                        Modules.Programs.Add(prgram);

                        List<CsProgramMenu> MainMenu = programs.Where(p => string.IsNullOrEmpty(p.MAINMENUID.ToString())).ToList();
                        CreateMenuTree(MainMenu, prgram);
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        void CreateProgramTree(List<CsProgramMenu> programs, OptionGroup Modules)
        {
            try
            {
                List<Program> Programs = new List<Program>();
                foreach (CsProgramMenu prog in programs) // entrée des programs du module courant
                {
                    if (!idProgram.Contains(prog.ID.ToString()))
                    {
                        idProgram.Add(prog.ID.ToString());

                        OptionGroup _prgram = new OptionGroup(prog.LIBELLE, Modules.IdGroupProgram, prog.ID, null, null, prog.ID, prog.USERCREATION, prog.DATECREATION); // instance du programme courant

                        // liste des menu principaux du program courant
                        List<CsProgramMenu> MainMenu = programs.Where(p => string.IsNullOrEmpty(p.MAINMENUID.ToString())).ToList();
                        if (MainMenu != null && MainMenu.Count > 0)
                        {
                            CreateMenuTree(MainMenu, _prgram);
                            if (_prgram.Children.Count() > 0)
                                Modules.Add(_prgram);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void CreateProgramTreeAdmMenu(List<CsAdmMenu> programs, OptionGroup Modules)
        {
            try
            {
                List<Program> Programs = new List<Program>();
                foreach (CsAdmMenu prog in programs) // entrée des programs du module courant
                {
                    if (!idProgram.Contains(prog.PK_ID.ToString()))
                    {
                        idProgram.Add(prog.PK_ID.ToString());

                        OptionGroup _prgram = new OptionGroup(prog.MENUTEXT, Modules.IdGroupProgram, prog.PK_ID, null, null, prog.PK_ID, prog.USERCREATION, prog.DATECREATION); // instance du programme courant

                        // liste des menu principaux du program courant
                        List<CsAdmMenu> MainMenu = programs.Where(p => string.IsNullOrEmpty(p.MAINMENUID.ToString())).ToList();
                        if (MainMenu != null && MainMenu.Count > 0)
                        {
                            CreateMenuTreeAdmMenu(MainMenu, _prgram);
                            if (_prgram.Children.Count() > 0)
                                Modules.Add(_prgram);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        OptionGroup CreationHabilitationTrees(List<CsProgramMenu> habilitation)
        {
            // Initialiser les liste de id
            try
            {
                idModule.Clear();
                idMenuRecurv.Clear();
                idMenu.Clear();
                idProgram.Clear();
                ObservableCollection<Module> Modules = new ObservableCollection<Module>();
                OptionGroup _modules = new OptionGroup("Modules", null, null, null, null, null, null, null); // entrée du module principal
                foreach (CsProgramMenu currentModule in habilitation) // entrée des modules
                {
                    if (!idModule.Contains(currentModule.FK_IDGROUPPROGRAM.ToString()))
                    {
                        idModule.Add(currentModule.FK_IDGROUPPROGRAM.ToString());
                        OptionGroup modul = new OptionGroup(currentModule.LIBELLE, currentModule.FK_IDGROUPPROGRAM, null, null, null, currentModule.ID, currentModule.USERCREATION, currentModule.DATECREATION); // instance du module courant
                        // liste des programs du module courant
                        List<CsProgramMenu> programs = habilitation.Where(p => p.FK_IDGROUPPROGRAM == currentModule.FK_IDGROUPPROGRAM).ToList();
                        if (programs != null && programs.Count > 0) // il y a au moins un program dans le module courant
                        {
                            CreateProgramTree(programs, modul);
                            if (modul.Children.Count() > 0)
                                _modules.Add(modul);
                        }
                    }
                }
                return _modules;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }


        OptionGroup CreationHabilitationTreesCsAdmMenu(List<CsAdmMenu> habilitation)
        {
            // Initialiser les liste de id
            try
            {
                idModule.Clear();
                idMenuRecurv.Clear();
                idMenu.Clear();
                idProgram.Clear();
                ObservableCollection<Module> Modules = new ObservableCollection<Module>();
                OptionGroup _modules = new OptionGroup("Modules", null, null, null, null, null, null, null); // entrée du module principal
                foreach (CsAdmMenu currentModule in habilitation) // entrée des modules
                {
                    if (!idModule.Contains(currentModule.FK_IDMODULE.ToString()))
                    {
                        idModule.Add(currentModule.FK_IDMODULE.ToString());
                        OptionGroup modul = new OptionGroup(currentModule.MENUTEXT, currentModule.FK_IDMODULE, null, null, null, currentModule.PK_ID, currentModule.USERCREATION, currentModule.DATECREATION); // instance du module courant
                        // liste des programs du module courant
                        List<CsAdmMenu> programs = habilitation.Where(p => p.FK_IDMODULE == currentModule.FK_IDMODULE).ToList();
                        if (programs != null && programs.Count > 0) // il y a au moins un program dans le module courant
                        {
                            CreateProgramTreeAdmMenu(programs, modul);
                            if (modul.Children.Count() > 0)
                                _modules.Add(modul);
                        }
                    }
                }
                return _modules;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        ObservableCollection<Module> CreationHabilitationTree(List<CsProgramMenu> habilitation)
        {
            // Initialiser les liste de id
            try
            {
                idModule.Clear();
                idMenuRecurv.Clear();
                idMenu.Clear();
                idProgram.Clear();
                ObservableCollection<Module> Modules = new ObservableCollection<Module>();
                foreach (CsProgramMenu prog in habilitation)
                {
                    if (!idModule.Contains(prog.FK_IDGROUPPROGRAM.ToString()))
                    {
                        idModule.Add(prog.FK_IDGROUPPROGRAM.ToString());
                        Module module = new Module();
                        module.Name = prog.LIBELLE;
                        module.IdModule = prog.FK_IDGROUPPROGRAM;
                        Modules.Add(module);
                        List<CsProgramMenu> programs = habilitation.Where(p => p.FK_IDGROUPPROGRAM == prog.FK_IDGROUPPROGRAM).ToList();
                        CreateProgramTree(programs, module);
                    }
                }
                return Modules;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Galatee.Silverlight.Shared.ClasseMEthodeGenerique.FermetureEcran(this);

        }

        void cbo_profile_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (cbo_profile.SelectedItem != null)
                {
                    txtCode.Text = string.Empty;
                    txtLibelle.Text = string.Empty;
                    CheckTreeviewParProfils();
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Langue.errorTitle);
            }
        }

        void txtLibelle_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        void txtCode_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
        void GetListeMenuDuProfilByProfil( CsProfil leprof)
        {
            try
            {
                AdministrationServiceClient client = new AdministrationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Administration"));
                client.MenusSelectByProfilCompleted += (sshab, reshab) =>
                {
                    try
                    {
                        if (reshab.Cancelled || reshab.Error != null)
                        {
                            string error = reshab.Error.Message;
                            Message.ShowInformation(error, "HabilitationSelectByMetier");
                            return;
                        }

                        if (reshab.Result == null || reshab.Result.Count == 0)
                        {
                            return;

                        }
                        lesMenusDuProfilSelect.Clear();
                        lesMenusDuProfilSelect.AddRange(reshab.Result);
                        CheckTreeviewParFonction(leprof.FK_IDFONCTION);

                       
                    }
                    catch (Exception ex)
                    {
                        Message.ShowError(ex.Message, Langue.informationTitle);
                    }
                };
                client.MenusSelectByProfilAsync(leprof.PK_ID);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void CheckTreeviewParFonction( int idfonction)
        {
            try
            {
                AdministrationServiceClient client = new AdministrationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Administration"));
                client.MenusSelectByFonctionCompleted += (sshab, reshab) =>
                {
                    try
                    {
                        if (reshab.Cancelled || reshab.Error != null)
                        {
                            string error = reshab.Error.Message;
                            Message.ShowInformation(error, "HabilitationSelectByMetier");
                            InitializerTreeviewCheckbox();
                            return;
                        }

                        if (reshab.Result == null || reshab.Result.Count == 0)
                        {
                            
                        }
                        listeAllMenuFonctionSelect.Clear();
                        listeAllMenuFonctionSelect.AddRange(reshab.Result);
                        GetDataCsAdmMEnuCocher(listeAllMenuFonctionSelect, lesMenusDuProfilSelect);
                    }
                    catch (Exception ex)
                    {
                        Message.ShowError(ex.Message, Langue.informationTitle);
                    }
                };
                client.MenusSelectByFonctionAsync(idfonction);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void CheckTreeviewParProfils()
        {
            try
            {

                OKButton.IsEnabled = false;
                ta = new CsFonction();
                ta = cbo_profile.SelectedItem as CsFonction;

                AdministrationServiceClient client = new AdministrationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Administration"));
                client.MenusSelectByFonctionCompleted += (sshab, reshab) =>
                {
                    try
                    {
                        if (reshab.Cancelled || reshab.Error != null)
                        {
                            string error = reshab.Error.Message;
                            Message.ShowInformation(error, "HabilitationSelectByMetier");
                            InitializerTreeviewCheckbox();
                            return;
                        }

                        if (reshab.Result == null || reshab.Result.Count == 0)
                        {
                            InitializerTreeviewCheckbox();
                            Main.IsEnabled = true;
                            if (ta.CODE != "PREMIER")
                            {
                                Message.Show("La fonction "+ ta.ROLENAME + " n'est  associée à aucun module", Langue.errorTitle);
                                return;
                            }

                            //return;
                        }
                        Main.IsEnabled = true;

                        listeAllMenuFonctionSelect.AddRange(reshab.Result);

                        GetDataCsAdmMEnu(reshab.Result);
                        GetDataListProfildeFonction(ta);
                    }
                    catch (Exception ex)
                    {
                        Message.ShowError(ex.Message, Langue.informationTitle);
                    }
                };
                client.MenusSelectByFonctionAsync(ta.PK_ID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CsProgramMenu> ConvertProgramAdmmenu(List<CsAdmMenu> mList)
        {
            List<CsProgramMenu> plist = new List<CsProgramMenu>();

            foreach (var item in mList)
            {
                CsProgramMenu pMenu = new CsProgramMenu();

            }
            return plist;
        }

        void CocherTreeviewDuProfil(IEnumerable<OptionViewModel> grp, List<CsAdmMenu> habilitation)
        {
            try
            {
                foreach (OptionViewModel view in grp)
                {
                    foreach (CsAdmMenu habil in habilitation)
                    {
                        foreach (Option m in view.Option.Children)
                        {
                            IEnumerable<Option> program = m.Children;
                            foreach (Option prog in program)
                            {
                                IEnumerable<Option> menu = prog.Children;
                                TraiterMainMenu(menu, habil, m, prog);

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        void CocherTreeviewDuProfilSelect(IEnumerable<OptionViewModel> grp, List<CsAdmMenu> habilitation)
        {
            try
            {
                foreach (OptionViewModel view in grp)
                {
                    foreach (CsAdmMenu habil in habilitation)
                    {
                        foreach (Option m in view.Option.Children)
                        {
                            IEnumerable<Option> program = m.Children;
                            foreach (Option prog in program)
                            {
                                IEnumerable<Option> menu = prog.Children;
                                TraiterMainMenu(menu, habil, m, prog);

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        void cbo_profile_DropDownClosed(object sender, EventArgs e)
        {
            //CheckTreeviewParProfil();
        }

        void RefreshTreeView()
        {
            Main.Items.Clear();
            modules = CreationHabilitationTree(ListeProgramMenu);
            createTreeItem(modules);
        }

        #region Reinitialiser la treeview

        void InitializerTreeviewCheckbox()
        {
            if (viewModel != null)
                viewModel.IsChecked = false;
        }

        void InitializerTreeviewCheckboxProgram(ObservableCollection<Menu> observableMenu)
        {
            foreach (Menu mainM in observableMenu)
            {
                RecursiveInitializeTreeviewCheckMenu(mainM.MenuItem);
                mainM.CheckIHM = true;
                mainM.Check = true;
                mainM.Check = false;
            }
        }

        void RecursiveInitializeTreeviewCheckMenu(ObservableCollection<Menu> observableMenu)
        {
            if (observableMenu.Count > 0)
            {
                foreach (Menu m in observableMenu)
                {
                    RecursiveInitializeCheckMenu(m.MenuItem);
                    m.CheckIHM = true;
                    m.Check = true;
                    m.Check = false;
                }
            }

        }

        #endregion

        void InitializeCheckIhm()
        {
            foreach (Module m in modules)
            {
                m.CheckIHM = true;
                ObservableCollection<Program> program = m.Programs;
                foreach (Program prog in program)
                {
                    prog.CheckIHM = true;
                    ObservableCollection<Menu> menu = prog.Menus;
                    TraiterInitializeCheckProgram(menu);

                }
            }
        }

        void TraiterInitializeCheckProgram(ObservableCollection<Menu> observableMenu)
        {
            foreach (Menu mainM in observableMenu)
            {
                mainM.CheckIHM = true;
                RecursiveInitializeCheckMenu(mainM.MenuItem);
            }
        }

        void RecursiveInitializeCheckMenu(ObservableCollection<Menu> observableMenu)
        {
            if (observableMenu.Count > 0)
            {
                foreach (Menu m in observableMenu)
                {
                    m.CheckIHM = true;
                    RecursiveInitializeCheckMenu(m.MenuItem);
                }
            }

        }

        void TraiterMainMenu(ObservableCollection<Menu> observableMenu, CsAdmMenu habilitation, Module module, Program programm)
        {
            foreach (Menu mainM in observableMenu)
            {
                mainM.CheckIHM = false;
                RecursiveProfil(mainM.MenuItem, habilitation, module, programm, mainM);
            }
        }

        void TraiterMainMenu(IEnumerable<Option> observableMenu, CsAdmMenu habilitation, Option module, Option programm)
        {
            try
            {
                foreach (Option mainM in observableMenu)
                {
                    RecursiveProfil(mainM.Children, habilitation, module, programm, mainM);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        void RecursiveProfil(IEnumerable<Option> observableMenu, CsAdmMenu habilitation, Option module, Option programm, Option main)
        {
            try
            {
                if (observableMenu.Count() > 0)
                {
                    foreach (Option m in observableMenu)
                    {

                        RecursiveProfil(m.Children, habilitation, module, programm, m);
                        if (m.MenuID == habilitation.PK_ID && m.Children.Count() == 0)
                        {
                            OptionLeaf _m = (OptionLeaf)m;
                            _m.Selected = true;
                        }
                    }
                }
                else
                {
                    if (main.MenuID == habilitation.PK_ID)
                    {
                        OptionLeaf _m = (OptionLeaf)main;
                        _m.Selected = true;
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        void RecursiveProfil(ObservableCollection<Menu> observableMenu, CsAdmMenu habilitation, Module module, Program programm, Menu main)
        {
            try
            {
                if (observableMenu.Count > 0)
                {
                    foreach (Menu m in observableMenu)
                    {
                        m.CheckIHM = false;

                        if (m.MenuID == habilitation.PK_ID)
                        {
                            m.Check = true;
                            module.Check = true;
                            programm.Check = true;
                            main.Check = true;
                        }
                        RecursiveProfil(m.MenuItem, habilitation, module, programm, m);
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        void createTreeItem(ObservableCollection<Module> m)
        {
            foreach (Module module in m)
            {
                TreeViewItem moduleItem = new TreeViewItem();
                moduleItem.Header = module;
                moduleItem.Tag = module;
                moduleItem.HeaderTemplate = (DataTemplate)LayoutRoot.Resources["headerTemplate"];
                foreach (Program program in module.Programs)
                {
                    TreeViewItem programItem = new TreeViewItem();
                    programItem.Header = program;
                    programItem.Tag = program;
                    programItem.HeaderTemplate = (DataTemplate)LayoutRoot.Resources["headerTemplate"];
                    if (program.Menus != null && program.Menus.Count > 0)
                        CreateRecursiveItem(programItem, program.Menus);
                    moduleItem.Items.Add(programItem);
                }
                Main.Items.Add(moduleItem);

            }
        }

        void CreateRecursiveItem(TreeViewItem Parent, ObservableCollection<Menu> menu)
        {
            if (menu != null && menu.Count > 0)
            {
                foreach (Menu men in menu)
                {
                    TreeViewItem MenuItem = new TreeViewItem();
                    MenuItem.Header = men;
                    MenuItem.Tag = men;
                    MenuItem.HeaderTemplate = (DataTemplate)LayoutRoot.Resources["headerTemplate"];
                    if (men.MenuItem != null && men.MenuItem.Count > 0)
                    {
                        Parent.Items.Add(MenuItem);
                        CreateRecursiveItem(MenuItem, men.MenuItem);
                    }
                    else
                        Parent.Items.Add(MenuItem);
                }
            }
        }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            //CheckBox checkbox = sender as CheckBox;

            //TreeViewItem item = AllInOne.FindControl<TreeViewItem>(Main, typeof(TreeViewItem), checkbox.Name);
            //RecursiveCheck(item, checkbox.IsChecked);
        }

        private void RecursiveCheck(TreeViewItem item, bool? check)
        {
            try
            {
                CheckBox checkb = AllInOne.FindControl<CheckBox>(Main, typeof(CheckBox), item.Name);
                checkb.IsChecked = check;

                if (item.Tag.GetType() == typeof(Module))
                {
                    Module moduleCheck = item.Tag as Module;
                    foreach (Program prg in moduleCheck.Programs)
                    {
                        CheckBox checkbPrg = AllInOne.FindControl<CheckBox>(Main, typeof(CheckBox), prg.Name);
                        checkbPrg.IsChecked = check.Value;
                        RecursiveMenuTreeCheck(prg.Menus, check);
                        //RecursiveMenuTreeCheck(prg.Menus, check);
                    }
                }
                else
                    if (item.Tag.GetType() == typeof(Program))
                    {
                        Program prgramCheck = item.Tag as Program;
                        CheckBox checkbPrg = AllInOne.FindControl<CheckBox>(Main, typeof(CheckBox), prgramCheck.Name);
                        checkbPrg.IsChecked = check.Value;
                        RecursiveMenuTreeCheck(prgramCheck.Menus, check);
                        //RecursiveMenuTreeCheck(prgramCheck.Menus, check);
                    }
                    else
                        if (item.Tag.GetType() == typeof(Menu))
                        {
                            Menu menuCheck = item.Tag as Menu;
                            CheckBox checkMEnu = AllInOne.FindControl<CheckBox>(Main, typeof(CheckBox), menuCheck.Name);

                            checkMEnu.IsChecked = check.Value;
                            RecursiveMenuTreeCheck(menuCheck.MenuItem, check);
                            //RecursiveMenuTreeCheck(menuCheck.MenuItem, check);
                        }

            }
            catch (Exception ex)
            {

                throw ex;
            }


        }

        void RecursiveMenuTreeCheck(ObservableCollection<Menu> menus, bool? check)
        {
            try
            {
                if (menus != null && menus.Count > 0)
                    foreach (Menu men in menus)
                    {
                        CheckBox checkbMnu = AllInOne.FindControl<CheckBox>(Main, typeof(CheckBox), men.Name);
                        checkbMnu.IsChecked = check.Value;
                        RecursiveMenuTreeCheck(men.MenuItem, check);
                    }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void dataGrid_ParamProfil_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // charger données select

            CsProfil prof = new CsProfil();
            prof = dataGrid_ListeProfil.SelectedItem as CsProfil;

            if (prof != null && prof.PK_ID.ToString() != "")
            {
                CsFonction fct = new CsFonction();
                fct = DonneesFonctions.Where(s=> s.PK_ID == prof.FK_IDFONCTION).FirstOrDefault();
                int rang = DonneesFonctions.IndexOf(fct);
                cbo_profile.SelectedIndex = rang;

                txtCode.Text = prof.CODE;
                txtCode.IsEnabled = false;
                txtLibelle.Text = prof.LIBELLE;

                leprofilSelect = prof;
                GetListeMenuDuProfilByProfil(prof);
                //CheckTreeviewParProfils();
                //CheckTreeviewParFonction(fct.PK_ID);
                

            }
        }

        private void Btn_NvoProfil_Click(object sender, RoutedEventArgs e)
        {
            CsFonction tf = new CsFonction();
            tf = cbo_profile.SelectedItem as CsFonction;

            //OKButton.IsEnabled = true;

            if (tf != null && tf.CODE != "" && tf.PK_ID.ToString() != "" && tf.CODE != "PREMIER")
            {
                CheckTreeviewParProfils();

                txtCode.Text = string.Empty;
                txtCode.IsEnabled = true;
                txtCode.Focus();
                txtLibelle.Text = string.Empty;
            }
            else
            {
                Message.Show("Veuillez choisir une fonction .", Langue.errorTitle);
            }

        }

        

    }
}

