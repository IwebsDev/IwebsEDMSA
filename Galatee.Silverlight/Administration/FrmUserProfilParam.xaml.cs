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
    public partial class FrmUserProfilParam : ChildWindow
    {
        public FrmUserProfilParam()
        {
            InitializeComponent();
       
        }
        public CsUtilisateur userselect = new CsUtilisateur();
        List<CsProfil> ListProfilUtilisateur;
        List<CsProfil> ListProfilUtilisateuraSuprimer=new List<CsProfil>();

        public bool isOkClick = false;
        public FrmUserProfilParam(CsSite pSite, CsCentre pCentre, List<CsCentre> lstcentre, List<CsSite> lstsite, List<CsProfil> lstprofil, List<CsFonction> lstfonction, List<CsCentreDuProfil> lstcentreprofil,CsUtilisateur _userselect)
        {
            
            this.ListeFonction = lstfonction;
            this.ListeCentre = lstcentre;
            this.ListeProfil = lstprofil;
            this.ListeSite = lstsite;
            this.chxSite = pSite;
            this.chxCentre = pCentre;
            this.pParamProfil = lstcentreprofil;
            userselect = _userselect;
            if (_userselect.LESPROFILSUTILISATEUR != null && _userselect.LESPROFILSUTILISATEUR.Count != 0)
               ListProfilUtilisateur = new List<CsProfil>(_userselect.LESPROFILSUTILISATEUR);   

            InitializeComponent();
            this.Btn_Supprimer_Param_.IsEnabled = false;
            DataContext = ForView.Wrap(new DesignTimeData().Root);
            this.dtpkFromValid.SelectedDate = System.DateTime.Today.Date ;
            GetData();
            if (ListProfilUtilisateur != null && ListProfilUtilisateur.Count > 0)
                dataGrid_ParamProfil.ItemsSource = ListProfilUtilisateur;

            if (ListProfilUtilisateur!= null && ListProfilUtilisateur.Count > 0)
            {
                if (Btn_Supprimer_Param_.IsEnabled == false)
                    Btn_Supprimer_Param_.IsEnabled = true;
            }
        }

        #region chnage new
     

        List<CsFonction> DonnnesProfils = new List<CsFonction>();



        public List<CsCentre> ListeCentre = new List<CsCentre>();
        public List<CsSite> ListeSite = new List<CsSite>();
        public List<CsProfil> ListeProfil = new List<CsProfil>();
        public List<CsFonction> ListeFonction = new List<CsFonction>();

        public List<CsCentreDuProfil> pParamProfil = new List<CsCentreDuProfil>();

        public CsSite chxSite = new CsSite();
        public CsCentre chxCentre = new CsCentre();


        public List<CsProfil> ListeProfilUser = new List<CsProfil>();

        List<string> idMenuRecurv = new List<string>();
        List<string> idMenu = new List<string>();
        List<string> idProgram = new List<string>();
        List<string> idModule = new List<string>();
        List<CsProgramMenu> ListeProgramMenu = new List<CsProgramMenu>();
        List<CsProfil> CurrentProfilProgramMenu = new List<CsProfil>();
        ObservableCollection<Module> modules = null;
        CsFonction ta = null;// profil selectionne
        OptionViewModel viewModel = null;
        OptionViewModel viewModelSiteCentre = null;

        

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
                    CsProgramMenu main = ListeProgramMenu.Where(p => p.PK_MENUID == hbp.FK_IDMENU).FirstOrDefault();
                    if (!string.IsNullOrEmpty(main.MAINMENUID.ToString()))
                        if (!saveMainMenu.Contains(main.MAINMENUID.ToString()))
                        {
                            saveMainMenu.Add(main.MAINMENUID.ToString());
                            ////_habilitationProfil.Add(new CsHabilitationProgram() { CODEFONCTION = ta.CODE, FK_IDGROUPPROGRAM = hbp.FK_IDGROUPPROGRAM, FK_IDPROGRAM = hbp.FK_IDPROGRAM, FK_IDMENU = main.MAINMENUID, USERCREATION = UserConnecte.matricule, DATECREATION = DateTime.Today.Date/*, PK_ID= main.PK_ID */});
                            RecursiveOtenirMainMenu(main.MAINMENUID.ToString(), _habilitationProfil, saveMainMenu);
                        }
                }
                return _habilitationProfil;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        void RecursiveOtenirMainMenu(string pMenu, List<CsProgramMenu> habilitationProfil, List<string> saveMainMenu)
        {
            try
            {
                CsProgramMenu main = ListeProgramMenu.Where(p => p.PK_MENUID.ToString() == pMenu).FirstOrDefault();
                if (!string.IsNullOrEmpty(main.MAINMENUID.ToString()))
                    if (!saveMainMenu.Contains(main.MAINMENUID.ToString()))
                    {
                        saveMainMenu.Add(main.MAINMENUID.ToString());
                        ////habilitationProfil.Add(new CsHabilitationProgram() { CODEFONCTION = ta.CODE, FK_IDGROUPPROGRAM = main.FK_IDGROUPPROGRAM, FK_IDPROGRAM = main.ID, FK_IDMENU = main.MAINMENUID, USERCREATION = UserConnecte.matricule, DATECREATION = DateTime.Today.Date });
                        RecursiveOtenirMainMenu(main.MAINMENUID.ToString(), habilitationProfil, saveMainMenu);
                    }
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
                if (!string.IsNullOrEmpty(main.MAINMENUID.ToString()))
                    if (!saveMainMenu.Contains(main.MAINMENUID.ToString()))
                    {
                        saveMainMenu.Add(main.MAINMENUID.ToString());
                        ////habilitationProfil.Add(new CsHabilitationProgram() { CODEFONCTION = ta.CODE, FK_IDGROUPPROGRAM = main.FK_IDGROUPPROGRAM, FK_IDPROGRAM = main.ID, FK_IDMENU = main.MAINMENUID, USERCREATION = UserConnecte.matricule, DATECREATION = DateTime.Today.Date });
                        RecursiveOtenirMainMenu(main.MAINMENUID.ToString(), habilitationProfil, saveMainMenu);
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

        void RecursiveHabilitation2(OptionViewModel module, Option program, IEnumerable<Option> menu, List<CsProgramMenu> listeH)
        {
            try
            {
                if (menu != null && menu.Count() > 0)
                {
                    foreach (Option men in menu)
                    {

                        if (men.Children != null && men.Children.Count() > 0)
                            RecursiveHabilitation2(module, program, men.Children, listeH);
                        else
                        {
                            OptionLeaf _men = (OptionLeaf)men;
                            if (_men.Selected)
                            {
                                //listeH.Add(new CsProgramMenu() { CODEFONCTION = ta.CODE, FK_IDGROUPPROGRAM = program.IdGroupProgram, /*FK_IDPROGRAM = program.PrgramID,*/ FK_IDMENU = men.MenuID,/* PK_ID = program.PkId,*/ USERCREATION = UserConnecte.matricule, DATECREATION = DateTime.Today.Date });
                            }
                        }
                    }
                }
                else
                {
                    OptionLeaf _men = (OptionLeaf)program;
                    if (_men.Selected)
                    {
                        //listeH.Add(new CsProgramMenu() {ID = program.Id,  PK_MENUID = , FK_IDGROUPPROGRAM = program.IdGroupProgram, /*FK_IDPROGRAM = program.PrgramID,*/ FK_IDMENU = program.MenuID,/*PK_ID = program.PkId,*/  USERCREATION = UserConnecte.matricule, DATECREATION = DateTime.Today.Date });
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

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

        void GetDataCsProfilFunction(List<CsProfil> listeMenu, List<CsFonction> pFonction)
        {
            List<CsProgramMenu> program = new List<CsProgramMenu>();
            try
            {

                //iteration des profils
                foreach (var item in listeMenu)
                {

                    CsProgramMenu menu = new CsProgramMenu()
                    {
                        ID = item.PK_ID,
                        FK_IDGROUPPROGRAM = item.FK_IDFONCTION,
                        FK_IDPROGRAM = item.FK_IDFONCTION,
                        PK_MENUID = item.PK_ID,
                        MAINMENUID = item.FK_IDFONCTION,
                        MENUTEXT = item.LIBELLE,
                        LIBELLE = item.LIBELLE,
                    };
                    program.Add(menu);
                }

                //iteration des fonctions
                foreach (var item in pFonction)
                {

                    CsProgramMenu menu = new CsProgramMenu()
                    {
                        ID = item.PK_ID,
                        FK_IDGROUPPROGRAM = item.PK_ID,
                        FK_IDPROGRAM = item.PK_ID,
                        PK_MENUID = item.PK_ID,
                        MENUTEXT = item.ROLENAME,
                        LIBELLE = item.ROLENAME,
                    };
                    program.Add(menu);
                }

                ListeProgramMenu = program;
                //Creation du menu treeview de l'habilitaion 
                modules = new ObservableCollection<Module>();
                OptionGroup MainGrp = CreationHabilitationTrees(ListeProgramMenu);
                viewModel = new Library.ViewModels.OptionViewModel(MainGrp);
                DataContext = ForView.Wrap(viewModel);
                Main_Copy.ItemsSource = viewModel.Children.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        void GetDataCsSiteCentre(List<CsSite> listeMenu, List<CsCentre> pFonction)
        {
            List<CsProgramMenu> program = new List<CsProgramMenu>();
            try
            {
                //iteration des profils
                foreach (var item in pFonction)
                {
                    CsProgramMenu menu = new CsProgramMenu()
                    {
                        ID = item.PK_ID,
                        FK_IDGROUPPROGRAM = item.FK_IDCODESITE,
                        FK_IDPROGRAM = item.FK_IDCODESITE,
                        PK_MENUID = item.PK_ID,
                        MAINMENUID = item.FK_IDCODESITE,
                        MENUTEXT = item.LIBELLE,
                        LIBELLE = item.LIBELLE,
                    };
                    program.Add(menu);
                }

                //iteration des fonctions
                foreach (var item in listeMenu)
                {

                    CsProgramMenu menu = new CsProgramMenu()
                    {
                        ID = item.PK_ID,
                        FK_IDGROUPPROGRAM = item.PK_ID,
                        FK_IDPROGRAM = item.PK_ID,
                        PK_MENUID = item.PK_ID,
                        MENUTEXT = item.LIBELLE,
                        LIBELLE = item.LIBELLE,
                    };
                    program.Add(menu);
                }

                ListeProgramMenu = program;
                //Creation du menu treeview de l'habilitaion 
                modules = new ObservableCollection<Module>();
                OptionGroup MainGrpSiteCentre = CreationHabilitationTrees(ListeProgramMenu);
                viewModelSiteCentre = new Library.ViewModels.OptionViewModel(MainGrpSiteCentre);
                DataContext = ForView.Wrap(viewModelSiteCentre);
                Main.ItemsSource = viewModelSiteCentre.Children.ToList();

                if (!rdbCentre.IsChecked == true && !rdbSite.IsChecked == true && !rdbGlobal.IsChecked == true)
                {
                    Main.IsEnabled = false;
                }

                if ((rdbCentre.IsChecked == true || rdbSite.IsChecked == true || rdbGlobal.IsChecked == true) && Main.IsEnabled == false)
                {
                    Main.IsEnabled = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        void GetDataCsSiteCentreChoice(List<CsSite> listeMenu, List<CsCentre> pFonction, List<CsCentre> pChoiceCentre)
        {
            List<CsProgramMenu> program = new List<CsProgramMenu>();
            try
            {
                //iteration des profils
                foreach (var item in pFonction)
                {
                    CsProgramMenu menu = new CsProgramMenu()
                    {
                        ID = item.PK_ID,
                        FK_IDGROUPPROGRAM = item.FK_IDCODESITE,
                        FK_IDPROGRAM = item.FK_IDCODESITE,
                        PK_MENUID = item.PK_ID,
                        MAINMENUID = item.FK_IDCODESITE,
                        MENUTEXT = item.LIBELLE,
                        LIBELLE = item.LIBELLE,
                        
                    };
                    program.Add(menu);
                }

                //iteration des fonctions
                foreach (var item in listeMenu)
                {

                    CsProgramMenu menu = new CsProgramMenu()
                    {
                        ID = item.PK_ID,
                        FK_IDGROUPPROGRAM = item.PK_ID,
                        FK_IDPROGRAM = item.PK_ID,
                        PK_MENUID = item.PK_ID,
                        MENUTEXT = item.LIBELLE,
                        LIBELLE = item.LIBELLE,
                    };
                    program.Add(menu);
                }

                ListeProgramMenu = program;
                //Creation du menu treeview de l'habilitaion 
                
                List<string> listchoice = new List<string>();

                foreach (CsCentre item in pChoiceCentre)
                {
                    listchoice.Add(item.PK_ID.ToString());
                }

                listchoice.Add(pFonction.FirstOrDefault().PK_ID.ToString());
                modules = new ObservableCollection<Module>();
                OptionGroup MainGrpSiteCentre = CreationHabilitationTreesChoice(ListeProgramMenu, listchoice);
                viewModelSiteCentre = new Library.ViewModels.OptionViewModel(MainGrpSiteCentre);
                DataContext = ForView.Wrap(viewModelSiteCentre);
                Main.ItemsSource = viewModelSiteCentre.Children.ToList();
                //Main.ExpandAll();
                //Main.ExpandToDepth(3);
                //Main.ExpandSelectedPath();
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
                    if (grpParent != null && leaf != null)
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

        void CreateProgramTree(List<CsProgramMenu> _profils, OptionGroup Fonction)
        {
            try
            {
                List<Program> Programs = new List<Program>();

                foreach (CsProgramMenu prog in _profils) // entrée des menu principaux
                {
                    if (!idMenu.Contains(prog.PK_MENUID.ToString()))
                    {
                        idMenu.Add(prog.PK_MENUID.ToString());

                        int? mainM = string.IsNullOrEmpty(prog.MAINMENUID.ToString()) ? null : prog.MAINMENUID;
                        OptionLeaf _menuLeaf = new OptionLeaf(prog.MENUTEXT, prog.FK_IDGROUPPROGRAM, null, mainM, prog.PK_MENUID, prog.ID, prog.USERCREATION, prog.DATECREATION); // instance du menu principal courant ( ex Operation)
                        OptionGroup _menuGroup = new OptionGroup(prog.MENUTEXT, prog.FK_IDGROUPPROGRAM, null, mainM, prog.PK_MENUID, prog.ID, prog.USERCREATION, prog.DATECREATION); // instance du menu principal courant
                        RecursiveMenuTree(Fonction, _menuGroup, _menuLeaf, _profils);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        void CocherTreeviewDuProfil(IEnumerable<OptionViewModel> grp, List<CsProgramMenu> habilitation,List<string> lstCentreSelect)
        {
            try
            {
                foreach (OptionViewModel view in grp)
                {
                    foreach (CsProgramMenu habil in habilitation)
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
        void TraiterMainMenu(IEnumerable<Option> observableMenu, CsProgramMenu habilitation, Option module, Option programm)
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

        void RecursiveProfil(IEnumerable<Option> observableMenu, CsProgramMenu habilitation, Option module, Option programm, Option main)
        {
            try
            {
                if (observableMenu.Count() > 0)
                {
                    foreach (Option m in observableMenu)
                    {

                        RecursiveProfil(m.Children, habilitation, module, programm, m);
                        if (m.MenuID == habilitation.ID  && m.Children.Count() == 0)
                        {
                            OptionLeaf _m = (OptionLeaf)m;
                            _m.Selected = true;
                        }
                    }
                }
                else
                {
                    if (main.MenuID == habilitation.ID )
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

        void RecursiveProfil(ObservableCollection<Menu> observableMenu, CsProgramMenu habilitation, Module module, Program programm, Menu main)
        {
            try
            {
                if (observableMenu.Count > 0)
                {
                    foreach (Menu m in observableMenu)
                    {
                        m.CheckIHM = false;

                        if (m.MenuID == habilitation.ID )
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

                List<CsProgramMenu> fonctions = habilitation.Where(f => string.IsNullOrEmpty(f.MAINMENUID.ToString())).ToList();


                foreach (CsProgramMenu currentModule in fonctions) // entrée des modules
                {
                    if (!idModule.Contains(currentModule.FK_IDGROUPPROGRAM.ToString()))
                    {
                        idModule.Add(currentModule.FK_IDGROUPPROGRAM.ToString());
                        OptionGroup modul = new OptionGroup(currentModule.LIBELLE, currentModule.FK_IDGROUPPROGRAM, null, null, null, currentModule.ID, currentModule.USERCREATION, currentModule.DATECREATION); // instance du module courant

                        // liste des profils de la fonction courant
                        List<CsProgramMenu> _profilsFonction = habilitation.Where(p => p.MAINMENUID == currentModule.ID).ToList();

                        if (_profilsFonction != null && _profilsFonction.Count > 0) // il y a au moins un program dans le module courant
                        {
                            // _modules.Add(modul);
                            foreach (CsProgramMenu men in _profilsFonction) // entrée des enfants du sous menu courant
                            {
                                if (!idMenuRecurv.Contains(men.PK_MENUID.ToString()))
                                {
                                    idMenuRecurv.Add(men.PK_MENUID.ToString());
                                    OptionLeaf mLeaf = new OptionLeaf(men.MENUTEXT, null, men.ID, men.MAINMENUID, men.PK_MENUID, men.ID, men.USERCREATION, men.DATECREATION); // instance du premier enfant de la sous collection 

                                    modul.Add(mLeaf);
                                }
                            }

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

        OptionGroup CreationHabilitationTreesChoice(List<CsProgramMenu> habilitation, List<string> listChoice)
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

                List<CsProgramMenu> fonctions = habilitation.Where(f => string.IsNullOrEmpty(f.MAINMENUID.ToString())).ToList();


                foreach (CsProgramMenu currentModule in fonctions) // entrée des modules
                {
                    if (!idModule.Contains(currentModule.FK_IDGROUPPROGRAM.ToString()))
                    {
                        idModule.Add(currentModule.FK_IDGROUPPROGRAM.ToString());
                        OptionGroup modul = new OptionGroup(currentModule.LIBELLE, currentModule.FK_IDGROUPPROGRAM, null, null, null, currentModule.ID, currentModule.USERCREATION, currentModule.DATECREATION); // instance du module courant

                        // liste des profils de la fonction courant
                        List<CsProgramMenu> _profilsFonction = habilitation.Where(p => p.MAINMENUID == currentModule.ID).ToList();

                        if (_profilsFonction != null && _profilsFonction.Count > 0) // il y a au moins un program dans le module courant
                        {
                            // _modules.Add(modul);


                            foreach (CsProgramMenu men in _profilsFonction) // entrée des enfants du sous menu courant
                            {
                                if (!idMenuRecurv.Contains(men.PK_MENUID.ToString()))
                                {
                                    idMenuRecurv.Add(men.PK_MENUID.ToString());
                                    OptionLeaf mLeaf = new OptionLeaf(men.MENUTEXT, null, men.ID, men.MAINMENUID, men.PK_MENUID, men.ID, men.USERCREATION, men.DATECREATION); // instance du premier enfant de la sous collection 
                                    if (listChoice != null && listChoice.Contains(men.ID.ToString()))
                                    {
                                        mLeaf.Selected = true;
                                    }
                                    modul.Add(mLeaf);
                                }
                            }

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
        void RefreshTreeView()
        {
            Main.Items.Clear();
            modules = CreationHabilitationTree(ListeProgramMenu);
            createTreeItem(modules);
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

        void RecursiveProfil(IEnumerable<Option> observableMenu, CsProfil habilitation, Option module, Option programm, Option main)
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

        void RecursiveProfil(ObservableCollection<Menu> observableMenu, CsProfil habilitation, Module module, Program programm, Menu main)
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
                        //RecursiveMenuTreeCheck(prg.Menus, check);
                    }
                }
                else
                    if (item.Tag.GetType() == typeof(Program))
                    {
                        Program prgramCheck = item.Tag as Program;
                        CheckBox checkbPrg = AllInOne.FindControl<CheckBox>(Main, typeof(CheckBox), prgramCheck.Name);
                        checkbPrg.IsChecked = check.Value;
                        //RecursiveMenuTreeCheck(prgramCheck.Menus, check);
                    }
                    else
                        if (item.Tag.GetType() == typeof(Menu))
                        {
                            Menu menuCheck = item.Tag as Menu;
                            CheckBox checkMEnu = AllInOne.FindControl<CheckBox>(Main, typeof(CheckBox), menuCheck.Name);
                            checkMEnu.IsChecked = check.Value;
                            //RecursiveMenuTreeCheck(menuCheck.MenuItem, check);
                        }

            }
            catch (Exception ex)
            {

                throw ex;
            }


        }


        /* New TreeView*/
        /**/

  

        private void FrmUserProfilParam_Closed(object sender, EventArgs e)
        {
            try
            {
             
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void Btn_Param_Profil(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dtpkFromValid.SelectedDate>dtpkToValid.SelectedDate)
                {
                    Message.ShowWarning("La date de debut doit etre inférieur à la date de fin", "Information");
                    return;
                }
                var FrmUserProfilParam = new FrmUserProfilParam();
                FrmUserProfilParam.Closed += new EventHandler(FrmUserProfilParam_Closed);
                FrmUserProfilParam.Show();

            }
            catch (Exception ex)
            {
                //Message.ShowError(ex.Message, Languages.txtDevis);
            }
        }

        private void dataGrid_Profil_SelectionChanged(object sender, RoutedEventArgs e)
        {

        }

        #endregion chnage new

        #region  new
        private void Btn_Supprimer_Param(object sender, RoutedEventArgs e)
        {
            if (this.dataGrid_ParamProfil.SelectedItem != null)
            {
                foreach (var leProfilSelect in ListProfilUtilisateuraSuprimer)
                {
                    ListProfilUtilisateur.Remove(leProfilSelect); 
                }
                this.dataGrid_ParamProfil.ItemsSource = null;
                this.dataGrid_ParamProfil.ItemsSource = ListProfilUtilisateur;
                OKButton.IsEnabled = true;
            }
            else
            {
                Message.Show(" Veuillez sélectionner le profil à supprimer de la liste! ", Galatee.Silverlight.Resources.Langue.Error_Title);
                return;
            }
            GetData();
        }

        private void Btn_Ajouter_Param(object sender, RoutedEventArgs e)
        {
                if ((rdbCentre.IsChecked == false && rdbGlobal.IsChecked == false && rdbSite.IsChecked == false) && !IsModifProfil)
                {
                    Message.Show("Veuillez sélectionner un périmètre d'action", "Information");
                    return;
                }
                List<CsProgramMenu> ObtenirProfil = new List<CsProgramMenu>();
                List<CsProfil> ProfilSelect = new List<CsProfil>();
                OptionViewModel module = viewModel;
                DateTime dateFivalidite = string.IsNullOrEmpty(dtpkFromValid.Text) ? DateTime.Today.Date : dtpkFromValid.SelectedDate.Value;
                DateTime? dateFinvalidite = string.IsNullOrEmpty(dtpkToValid.Text) ? null : dtpkToValid.SelectedDate;
                //Profils
                foreach (Option MainProgram in module.Option.Children)
                {
                    foreach (Option program in MainProgram.Children)
                    {
                        OptionLeaf _men = (OptionLeaf)program;

                        if (_men.Selected)
                        {
                            ProfilSelect.Add(new CsProfil()
                            {
                                FK_IDPROFIL = (int)program.PkId,
                                FK_IDADMUTILISATEUR = userselect.PK_ID,
                                LIBELLE = program.Name,
                                DATEDEBUT = dateFivalidite,
                                DATEFIN = dateFinvalidite,
                                FK_IDFONCTION = MainProgram.IdGroupProgram.Value  
                            });
                            IsModifProfil = false;
                        }
                    }
                }
                CsProfil leProfilSel = IsModifProfil ? (CsProfil)dataGrid_ParamProfil.SelectedItem : ProfilSelect.First();
                //if (ListProfilUtilisateur != null && !IsModifProfil)
                //{
                //    CsProfil VerifieProfil = ListProfilUtilisateur.FirstOrDefault(o => o.FK_IDFONCTION == leProfilSel.FK_IDFONCTION);
                //    if (VerifieProfil != null)
                //    {
                //        Message.ShowInformation("Vous ne pouvez pas associer ce profil au profil " + VerifieProfil.LIBELLE, "Administration");
                //        return;
                //    }
                //}
                List<CsCentreProfil> lstCentreDuProfil = new List<CsCentreProfil>();

                if (rdbCentre.IsChecked == true)
                {
                    lstCentreDuProfil.Add(new CsCentreProfil()
                     {
                         FK_IDCENTRE = chxCentre.PK_ID,
                         FK_IDPROFIL = leProfilSel.FK_IDPROFIL,
                         FK_IDADMUTILISATEUR = userselect.PK_ID,
                         LIBELLECENTRE = chxCentre.LIBELLE,
                         DATEDEBUTVALIDITE = dateFivalidite,
                         DATEFINVALIDITE = dateFinvalidite
                     });
                    userselect.PERIMETREACTION = SessionObject.Enumere.ActionSurCentre;
                }
                else
                {
                    if (rdbSite.IsChecked == true)
                        userselect.PERIMETREACTION = SessionObject.Enumere.ActionSurSite;
                    else
                        userselect.PERIMETREACTION = SessionObject.Enumere.ActionSurTousSite;

                    List<CsProgramMenu> ObtenirCentre = new List<CsProgramMenu>();
                    List<CsCentre> UserCentre = new List<CsCentre>();
                    CsCentre leCentre = new CsCentre();
                    OptionViewModel moduleCentre = viewModelSiteCentre;
                    foreach (Option MainProgramCentre in moduleCentre.Option.Children)
                    {
                        foreach (Option program in MainProgramCentre.Children)
                        {
                            OptionLeaf _men = (OptionLeaf)program;
                            if (_men.Selected)
                            {
                                lstCentreDuProfil.Add(new CsCentreProfil()
                                {
                                    FK_IDCENTRE = (int)program.PkId,
                                    FK_IDPROFIL = leProfilSel.FK_IDPROFIL,
                                    FK_IDADMUTILISATEUR = userselect.PK_ID,
                                    LIBELLECENTRE = program.Name,
                                    DATEDEBUTVALIDITE = dateFivalidite,
                                    DATEFINVALIDITE = dateFinvalidite
                                });
                            }
                        }
                    }
                }

                CsProfil leProfilAjouter = new CsProfil();
                leProfilAjouter = leProfilSel;
                leProfilAjouter.LESCENTRESPROFIL = lstCentreDuProfil;
                if (ListProfilUtilisateur != null && ListProfilUtilisateur.FirstOrDefault(t => (t.FK_IDPROFIL == leProfilSel.FK_IDPROFIL)) != null)
                    ListProfilUtilisateur.Remove(ListProfilUtilisateur.FirstOrDefault(t => (t.FK_IDPROFIL == leProfilSel.FK_IDPROFIL)));

                if (ListProfilUtilisateur == null) ListProfilUtilisateur = new List<CsProfil>();
                ListProfilUtilisateur.Add(leProfilAjouter);

                dataGrid_ParamProfil.ItemsSource = null;
                dataGrid_ParamProfil.ItemsSource = ListProfilUtilisateur;

                if (rdbCentre.IsChecked == true)
                    rdbCentre.IsChecked = false;
                if (rdbSite.IsChecked == true)
                    rdbSite.IsChecked = false;
                if (rdbGlobal.IsChecked == true)
                    rdbGlobal.IsChecked = false;
                if (ListProfilUtilisateur != null && ListProfilUtilisateur.Count > 0)
                {
                    if (OKButton.IsEnabled == false)
                        OKButton.IsEnabled = true;
                }
                if (Btn_Supprimer_Param_.IsEnabled == false)
                    Btn_Supprimer_Param_.IsEnabled = true;

                //if (Btn_Ajouter_Param_.IsEnabled == true)
                //    Btn_Ajouter_Param_.IsEnabled = false;
                GetData();
            
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            isOkClick = true;
            if (userselect.LESPROFILSUTILISATEUR == null) userselect.LESPROFILSUTILISATEUR = new List<CsProfil>();
            userselect.LESPROFILSUTILISATEUR = ListProfilUtilisateur;
            this.Close();
        }
        bool IsModifProfil = false;
        private void dataGrid_ParamProfil_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dataGrid_ParamProfil.SelectedItem != null)
            {
                CsProfil leProfilSelect = (CsProfil)dataGrid_ParamProfil.SelectedItem;
                leProfilSelect.IsSelect = true;

                foreach (var item in ListProfilUtilisateur.Where(t => t.PK_ID != leProfilSelect.PK_ID).ToList())
                    item.IsSelect = false;
                
                this.Btn_Supprimer_Param_.IsEnabled = true;
                if (leProfilSelect.LESCENTRESPROFIL == null) return;
                List<CsCentreProfil> lstCentreProfil = leProfilSelect.LESCENTRESPROFIL ;
                List<string> lstCentreSelect =new List<string>();
                foreach ( CsCentreProfil itemss  in leProfilSelect.LESCENTRESPROFIL)
		              lstCentreSelect.Add(itemss.FK_IDCENTRE.ToString());

                List<CsProgramMenu> program = new List<CsProgramMenu>();
                 //iteration des profils
                foreach (var item in ListeCentre)
                {
                    CsProgramMenu menu = new CsProgramMenu()
                    {
                        ID = item.PK_ID,
                        FK_IDGROUPPROGRAM = item.FK_IDCODESITE,
                        FK_IDPROGRAM = item.FK_IDCODESITE,
                        PK_MENUID = item.PK_ID,
                        MAINMENUID = item.FK_IDCODESITE,
                        MENUTEXT = item.LIBELLE,
                        LIBELLE = item.LIBELLE,
                    };
                    program.Add(menu);
                }

                //iteration des fonctions
                foreach (var item in ListeSite)
                {

                    CsProgramMenu menu = new CsProgramMenu()
                    {
                        ID = item.PK_ID,
                        FK_IDGROUPPROGRAM = item.PK_ID,
                        FK_IDPROGRAM = item.PK_ID,
                        PK_MENUID = item.PK_ID,
                        MENUTEXT = item.LIBELLE,
                        LIBELLE = item.LIBELLE,
                    };
                    program.Add(menu);
                }
                OptionGroup MainGrpSiteCentre = CreationHabilitationTreesChoice(program, lstCentreSelect);
                viewModelSiteCentre = new Library.ViewModels.OptionViewModel(MainGrpSiteCentre);
                DataContext = ForView.Wrap(viewModelSiteCentre);
                Main.ItemsSource = viewModelSiteCentre.Children.ToList();
                IsModifProfil = true;
                Main.ExpandAll();
                if (Main.IsEnabled == false)
                    Main.IsEnabled = true;
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
        public void GetData()
        {
            GetDataCsProfilFunction(ListeProfil,ListeFonction);
            GetDataCsSiteCentre( ListeSite , ListeCentre);
        }
        #endregion  new
        private void rdbCentre_Checked(object sender, RoutedEventArgs e)
        {
            CsProfil leProfil = new CsProfil();
            List<CsProfil> ProfilSelect = new List<CsProfil>();
            OptionViewModel module = viewModel;
            //Profils
            foreach (Option MainProgram in module.Option.Children)
            {
                foreach (Option program in MainProgram.Children)
                {
                    OptionLeaf _men = (OptionLeaf)program;
                    if (_men.Selected)
                    {
                        leProfil = (new CsProfil()
                        {
                            PK_ID = (int)program.PkId,
                            LIBELLE = program.Name
                        });
                        ProfilSelect.Add(leProfil);
                    }
                }
            }

            if (ProfilSelect != null)
            {
                if (ProfilSelect.Count > 1)
                {
                    Message.Show(" Veuillez choisir un seul profil . ", Galatee.Silverlight.Resources.Langue.Error_Title);

                    rdbCentre.IsChecked = false;
                    rdbSite.IsChecked = false;
                    rdbGlobal.IsChecked = false;
                    GetData();
                }
                if (ProfilSelect.Count == 1)
                {
                    if (Btn_Ajouter_Param_.IsEnabled == false)
                        Btn_Ajouter_Param_.IsEnabled = true;
                    List<CsSite> ssite = new List<CsSite>();
                    ssite.Add(chxSite);
                    List<CsCentre> scentre = new List<CsCentre>();
                    scentre.Add(chxCentre);
                    GetDataCsSiteCentreChoice(ssite, ListeCentre, scentre);
                    Main.ExpandAll();
                    if (Main.IsEnabled == true)
                        Main.IsEnabled = false;
                }
                else
                    if (ProfilSelect.Count == 0)
                    {
                        Message.Show(" Veuillez choisir le profil . ", Galatee.Silverlight.Resources.Langue.Error_Title);
                        rdbCentre.IsChecked = false;
                        rdbSite.IsChecked = false;
                        rdbGlobal.IsChecked = false;
                        GetData();
                    }
            }
            
            
        }

        private void rdbSite_Checked(object sender, RoutedEventArgs e)
        {
             CsProfil leProfil = new CsProfil();
            List<CsProfil> ProfilSelect = new List<CsProfil>();
            OptionViewModel module = viewModel;
            //Profils
            foreach (Option MainProgram in module.Option.Children)
            {
                foreach (Option program in MainProgram.Children)
                {
                    OptionLeaf _men = (OptionLeaf)program;

                    if (_men.Selected)
                    {
                        leProfil = (new CsProfil()
                        {
                            PK_ID = (int)program.PkId,
                            LIBELLE = program.Name
                        });
                        ProfilSelect.Add(leProfil);
                    }
                }
            }

            if (ProfilSelect != null)
            {
                if (ProfilSelect.Count > 1)
                {
                    Message.Show(" Veuillez choisir un seul profil . ", Galatee.Silverlight.Resources.Langue.Error_Title);
                    rdbCentre.IsChecked = false;
                    rdbSite.IsChecked = false;
                    rdbGlobal.IsChecked = false;
                    GetData();
                }
                if (ProfilSelect.Count == 1)
                {
                    if (Btn_Ajouter_Param_.IsEnabled == false)
                        Btn_Ajouter_Param_.IsEnabled = true;

                    List<CsSite> ssite = new List<CsSite>();
                    ssite.Add(chxSite);
                    List<CsCentre> scentre = new List<CsCentre>();
                    List<CsCentre> _centres = new List<CsCentre>();
                    _centres = null;
                    _centres = ListeCentre.Where(c => c.FK_IDCODESITE == chxSite.PK_ID).ToList();
                    if (_centres != null && _centres.Count > 0)
                        GetDataCsSiteCentreChoice(ssite, ListeCentre, _centres);

                    Main.ExpandAll();
                    if (Main.IsEnabled == false)
                        Main.IsEnabled = true;
                }
                else
                    if (ProfilSelect.Count == 0)
                        {
                            Message.Show(" Veuillez choisir le profil . ", Galatee.Silverlight.Resources.Langue.Error_Title);
                            rdbCentre.IsChecked = false;
                            rdbSite.IsChecked = false;
                            rdbGlobal.IsChecked = false;

                            GetData();
                        }
                }
        }

        private void rdbGlobal_Checked(object sender, RoutedEventArgs e)
        {
             CsProfil leProfil = new CsProfil();
            List<CsProfil> ProfilSelect = new List<CsProfil>();
            OptionViewModel module = viewModel;
            //Profils
            foreach (Option MainProgram in module.Option.Children)
            {
                foreach (Option program in MainProgram.Children)
                {
                    OptionLeaf _men = (OptionLeaf)program;

                    if (_men.Selected)
                    {
                        leProfil = (new CsProfil()
                        {
                            PK_ID = (int)program.PkId,
                            LIBELLE = program.Name
                        });
                        ProfilSelect.Add(leProfil);
                    }
                }
            }
            if (ProfilSelect != null)
            {
                if (ProfilSelect.Count > 1)
                {
                    Message.Show(" Veuillez choisir un seul profil . ", Galatee.Silverlight.Resources.Langue.Error_Title);
                    rdbCentre.IsChecked = false;
                    rdbSite.IsChecked = false;
                    rdbGlobal.IsChecked = false;
                    GetData();
                }
                else
                    if (ProfilSelect.Count == 1)
                    {
                        if (Btn_Ajouter_Param_.IsEnabled == false)
                            Btn_Ajouter_Param_.IsEnabled = true;


                        List<CsCentre> _centres = new List<CsCentre>();
                        _centres = null;
                        _centres = ListeCentre;
                        GetDataCsSiteCentreChoice(ListeSite, ListeCentre, _centres);
                        if (Main.IsEnabled == false)
                            Main.IsEnabled = true;
                    }
                if (ProfilSelect.Count == 0)
                {
                    Message.Show(" Veuillez choisir le profil . ", Galatee.Silverlight.Resources.Langue.Error_Title);
                    rdbCentre.IsChecked = false;
                    rdbSite.IsChecked = false;
                    rdbGlobal.IsChecked = false;
                    GetData();
                }
            }
        }

        private void dtpkFromValid_CalendarClosed(object sender, RoutedEventArgs e)
        {
            if (this.dtpkFromValid.SelectedDate < System.DateTime.Today.Date)
            {
                Message.ShowInformation("Veuillez selectionner une date supperieur a la date en cour", "Administration");
                this.dtpkFromValid.SelectedDate = System.DateTime.Today.Date;
                return;
            }
        }

        private void chk_Critere_Checked_1(object sender, RoutedEventArgs e)
        {
            var idprofil=int.Parse( ((CheckBox)sender).Tag.ToString());
            var monProfiltASupp=ListProfilUtilisateur.FirstOrDefault(p=>p.PK_ID==idprofil);
            ListProfilUtilisateuraSuprimer.Add(monProfiltASupp);

        }

        private void chk_Critere_Unchecked_1(object sender, RoutedEventArgs e)
        {
            var idprofil = int.Parse(((CheckBox)sender).Tag.ToString());
            var monProfiltASupp = ListProfilUtilisateur.FirstOrDefault(p => p.PK_ID == idprofil);
            ListProfilUtilisateuraSuprimer.Remove(monProfiltASupp);
        }
    }
}

