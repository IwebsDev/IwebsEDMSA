using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Galatee.Structure;
using System.Data;
using Galatee.DataAccess;
using System.Reflection;
using System.Drawing.Printing;
using System.Security.Cryptography;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Formatters;
using System.Web.Hosting;
using System.Timers;

using System.ServiceModel.Activation;
using System.Web;
using System.Management;
using System.Runtime.InteropServices;
using System.Drawing.Imaging;
using System.Configuration;
using System.Data.EntityClient;
using System.Data.SqlClient;
using System.Xml;

namespace WcfService
{
    // REMARQUE : vous pouvez utiliser la commande Renommer du menu Refactoriser pour changer le nom de classe "AuthentInitializeService" à la fois dans le code, le fichier svc et le fichier de configuration.
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class AuthentInitializeService : IAuthentInitializeService
    {
        Dictionary<string, string> ProcessList = new Dictionary<string, string>();
        int compteurIL;
        List<string> il;
        string filename = HostingEnvironment.MapPath("~/App_Data/license.lic");
        string fileConnexion = HostingEnvironment.MapPath("~/Connexion.config");
        License licences;
        Timer TimerLicense = null;
        bool AvertissementLicense = false;
        bool FermetureLicense = false;
        List<Stream> m_streams = new List<Stream>();
        private int m_currentPageIndex;

        //[DllImport("winspool.drv", CharSet = CharSet.Auto, SetLastError = true)]
        //public static extern bool GetDefaultPrinter(StringBuilder pszBuffer, ref int pcchBuffer);

        //private const int ERROR_FILE_NOT_FOUND = 2;
        //const int ERROR_INSUFFICIENT_BUFFER = 122;

        //public String getDefaultPrinter()
        //{

        //    int pcchBuffer = 0;
        //    if (GetDefaultPrinter(null, ref pcchBuffer))
        //    {
        //        return null;
        //    }
        //    int lastWin32Error = Marshal.GetLastWin32Error();
        //    if (lastWin32Error == ERROR_INSUFFICIENT_BUFFER)
        //    {
        //        StringBuilder pszBuffer = new StringBuilder(pcchBuffer);
        //        if (GetDefaultPrinter(pszBuffer, ref pcchBuffer))
        //        {
        //            return pszBuffer.ToString();
        //        }
        //        lastWin32Error = Marshal.GetLastWin32Error();
        //    }
        //    if (lastWin32Error == ERROR_FILE_NOT_FOUND)
        //    {
        //        return null;
        //    }
        //    return null;
        //}

        //public string GetLocalMachine()
        //{
        //    try
        //    {
        //        return Environment.MachineName;
        //    }
        //    catch (Exception)
        //    {
        //        return null;
        //    }
        //}

        #region   Building Module && Menu node for profile

        public string LicenseInferieurPeriodeAvertissemnt()
        {
            try
            {
                // verifier si le timer est en marche   
                // cela peut etre du à un arrêt du serveur 
                // date : 27/03/2012 
                // auteur : HGB

                //HttpContext.Current.User 
                if (TimerLicense == null)
                {
                    string[] license = GetLicenseInfo().Split(';');
                    ValiditePeriodeLicense(license);

                    // activer le timer pour la prise en compte de la license
                    TimerLicense = new Timer(10000);
                    TimerLicense.Interval = 36000000;
                    TimerLicense.AutoReset = true;
                    TimerLicense.Elapsed += new ElapsedEventHandler(TimerLicense_Elapsed);
                    TimerLicense.Enabled = true;
                }


                if (AvertissementLicense)
                {
                    string[] pLicense = GetLicenseInfo().Split(';');
                    double periodeValidite = Convert.ToDouble(Decrypt(pLicense[4]));
                    return TimeSpan.FromMilliseconds(periodeValidite).TotalDays.ToString();
                }
                return null;
                //return AvertissementLicense;
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }
        public bool LicenseInferieurPeriodeValidite()
        {
            try
            {
                return FermetureLicense;
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        
        public CsUserConnecte RetourneInfoMatriculeConnecte(string matricule, string pwd)
        {
            try
            {
                DBAuthentification db = new DBAuthentification();
                CsUserConnecte userconnecte = db.RetourneInfoMatriculeConnecte(matricule);

                //// verifier si le timer est en marche   
                //// cela peut etre du à un arrêt du serveur 
                //// date : 27/03/2012 
                //// auteur : HGB
                //if (TimerLicense == null)
                //{
                //    TimerLicense = new Timer(10000);
                //    TimerLicense.Interval = 60000;
                //    TimerLicense.AutoReset = true;
                //    TimerLicense.Elapsed += new ElapsedEventHandler(TimerLicense_Elapsed);
                //    TimerLicense.Enabled = true;
                //}
                //Galatee.Tools.Utility.User = userconnecte;

                return userconnecte;
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }

        }

        public List<CsDesktopGroup> GetListeModule(CsUtilisateur agent)
        {
            try
            {
                List<CsHabilitationProgram> lineTotal = new DBHabilitationModule().SelectHabilitationByUser(agent.PK_ID );
                List<CsHabilitationProgram> lines = lineTotal.Where(t => t.ModuleName != "Grec").ToList();

                if ((lines == null) || (lines.Count == 0))
                    return null;// throw new Exception(MainUI.Common.ManageLanguage.RM.GetString("msgAucuneHabilitation"));
                List<CsDesktopGroup> Result = new List<CsDesktopGroup>();
                CsDesktopGroup grp = null;
                CsDesktopItem Itm;
                List<int?> capteur = new List<int?>();
                int increment = 0;
                foreach (var item in lines)
                {
                    CsProfil profilLine = new CsProfil();
                    profilLine = new DBProfils().RetourneProfilByID(item.FK_IDPROFIL).FirstOrDefault();

                    List<CsDesktopItem> items = new List<CsDesktopItem>();
                    List<CsHabilitationProgram> lstMenuModule = lines.Where(t => t.FK_IDMODULE == item.FK_IDMODULE).ToList();
                    foreach (var eltMenu in lstMenuModule.OrderBy(t=>t.FK_IDMENU ))
                    {
                        CsDesktopItem unsubitems = new CsDesktopItem();
                        if (items.FirstOrDefault(p => p.ID == item.FK_IDGROUPPROGRAM) == null)
                        {
                            unsubitems.ID = eltMenu.FK_IDMODULE ; // EX PRGM
                            unsubitems.NOM = item.ModuleName;
                            unsubitems.Process  = item.Code ;
                            unsubitems.LIBELLE_FONCTION = item.ModuleName;
                            unsubitems.IdGroupProgram = eltMenu.FK_IDGROUPPROGRAM;
                            unsubitems.ProfilDesktopItem = profilLine;
                        }
                        items.Add(unsubitems);

                        grp = new CsDesktopGroup();
                        grp.ID = item.FK_IDGROUPPROGRAM;
                        grp.NOM = item.ModuleName;
                        grp.CodeModule = item.Code;
                        grp.SubItems = new List<CsDesktopItem>();
                        grp.ProfilDesktopItem = profilLine;
                        //if (!capteur.Contains(grp.ID))
                        if (!capteur.Contains(grp.ProfilDesktopItem.PK_ID))
                        {
                            //capteur.Add(grp.ID);
                            capteur.Add(grp.ProfilDesktopItem.PK_ID);
                            grp.SubItems = items;
                            Result.Add(grp);
                            increment++;
                        }

                    }
                }
                return Result;
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }
        public bool UpdateUser(CsUtilisateur admUsers)
        {
            try
            {
                return new DBAdmUsers().Update(admUsers,false );
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        //public Dictionary<CSMenuGalatee, List<TreeNodes>> GetMenuDuProfil(string Module, int IdProfil)
        public Dictionary<CSMenuGalatee, List<TreeNodes>> GetMenuDuProfil(string Module, List<int> IdProfil)
        {


            Dictionary<CSMenuGalatee, List<TreeNodes>> treeMenu = new Dictionary<CSMenuGalatee, List<TreeNodes>>();
            List<CSMenuGalatee> ListeDeMenusProfile = null;
            List<CSMenuGalatee> ListeDeMenuProfilAuth = null;
            List<CSMenuGalatee> ListeEntreesDeMenus;
            List<CSMenuGalatee> ListeEntreesDeMenusGlobal = new List<CSMenuGalatee>();
            List<CSMenuGalatee> lesLignesMenuPrincipal = null;

            foreach (int st in IdProfil)
            {
                ListeDeMenusProfile = ChargerMenuEtSousMenu(Module, st).ToList();
                ListeDeMenuProfilAuth = ListeDeMenusProfile != null && ListeDeMenusProfile.Count > 0 ? ChargerMenuEtSousMenuDuProfil(Module) : null;
                ListeEntreesDeMenus = new List<CSMenuGalatee>();
                lesLignesMenuPrincipal = null;

                if (ListeDeMenuProfilAuth != null)
                {
                    foreach (CSMenuGalatee menu in ListeDeMenuProfilAuth)
                    {
                        if ((ListeDeMenusProfile.FirstOrDefault(p => p.MenuID == menu.MenuID) != null) && (!ListeEntreesDeMenusGlobal.Exists(t => t.MenuID == menu.MenuID)))
                        {
                            ListeEntreesDeMenus.Add(ListeDeMenusProfile.FirstOrDefault(p => p.MenuID == menu.MenuID));
                            ListeEntreesDeMenusGlobal.Add(ListeDeMenusProfile.FirstOrDefault(p => p.MenuID == menu.MenuID));
                        }
                    }
                }
                if (ListeEntreesDeMenus != null && ListeEntreesDeMenus.Count > 0)
                {

                    var module = ListeEntreesDeMenus.FirstOrDefault();
                    if (module != null && module.IdGroupProgram == Enumere.moduleParametrageId)
                    {
                        lesLignesMenuPrincipal =
                            ListeEntreesDeMenus.Where(m => m.MainMenuID == 0 || m.MainMenuID == null).OrderBy(m => m.MenuID).ToList();
                    }

                    else
                        lesLignesMenuPrincipal = ListeEntreesDeMenus.Where(m => m.MainMenuID == 0).ToList();
                    List<CSMenuGalatee> lesLignesSousMenusDuRole = ListeEntreesDeMenus.Where(m => m.MainMenuID != 0 && m.MainMenuID != null).ToList();

                    List<TreeNodes> sousMenu = new List<TreeNodes>();
                    compteurIL = 0;
                    foreach (CSMenuGalatee curMenuPrincipal in lesLignesMenuPrincipal)
                    {
                        if (curMenuPrincipal.IconName != null)
                        {
                            compteurIL++;
                        }

                        //- Récupération du menu courant
                        if (curMenuPrincipal != null)
                        {
                            //- Génération de la liste de sous-menus pour le menu récupéré
                            sousMenu = GetMenuNodes(curMenuPrincipal.MenuID, ListeEntreesDeMenus);

                            //- Si le menu est non null et contient une arborescence, on insère le menu dans le dictionnaire des treeNodes
                            if ((curMenuPrincipal != null) && (sousMenu != null))
                                treeMenu.Add(curMenuPrincipal, sousMenu);
                            else if (curMenuPrincipal != null)
                                treeMenu.Add(curMenuPrincipal, sousMenu);
                        }
                    }
                }
            }

            return treeMenu;
        }

        public Dictionary<CSMenuGalatee, List<TreeNodes>> GetMenuDuRole(int idCodeFonction, string Module)
        {
            string projetName = string.Empty;// Assembly.GetExecutingAssembly().GetName().Name;
            projetName = Module;
            Dictionary<CSMenuGalatee, List<TreeNodes>> treeMenu = new Dictionary<CSMenuGalatee, List<TreeNodes>>();
            List<CSMenuGalatee> ListeDeMenusProfile = ChargerMenuEtSousMenu(projetName,0).ToList();
            //List<CSMenuGalatee> ListeDeMenuProfilAuth =  ChargerMenuEtSousMenuDuProfil(CodeFonction, ListeDeMenusProfile[0].IdGroupProgram).ToList(); ATO le 17/07/2013
            List<CSMenuGalatee> ListeDeMenuProfilAuth = ListeDeMenusProfile != null && ListeDeMenusProfile.Count > 0 ? ChargerMenuEtSousMenuDuProfil(idCodeFonction.ToString()) : null; //, ListeDeMenusProfile[0].IdGroupProgram).ToList() : null;
            List<CSMenuGalatee> ListeEntreesDeMenus = new List<CSMenuGalatee>();
            List<CSMenuGalatee> lesLignesMenuPrincipal = null;

            if (ListeDeMenuProfilAuth != null)
            {
                foreach (CSMenuGalatee menu in ListeDeMenuProfilAuth)
                {
                    if (ListeDeMenusProfile.FirstOrDefault(p => p.MenuID == menu.MenuID) != null)
                        ListeEntreesDeMenus.Add(ListeDeMenusProfile.FirstOrDefault(p => p.MenuID == menu.MenuID));
                }
            }
            if (ListeEntreesDeMenus != null && ListeEntreesDeMenus.Count > 0)
            {

                var module = ListeEntreesDeMenus.FirstOrDefault();
                if (module != null && module.IdGroupProgram == Enumere.moduleParametrageId)
                {
                    lesLignesMenuPrincipal =
                        ListeEntreesDeMenus.Where(m => m.MainMenuID == 0).OrderBy(m => m.MenuID).ToList();
                }

                else
                    lesLignesMenuPrincipal = ListeEntreesDeMenus.Where(m => m.MainMenuID == 0).ToList();
                List<CSMenuGalatee> lesLignesSousMenusDuRole = ListeEntreesDeMenus.Where(m => m.MainMenuID != 0).ToList();

                List<TreeNodes> sousMenu = new List<TreeNodes>();
                compteurIL = 0;
                foreach (CSMenuGalatee curMenuPrincipal in lesLignesMenuPrincipal)
                {
                    if (curMenuPrincipal.IconName != null)
                    {
                        compteurIL++;
                    }

                    //- Récupération du menu courant
                    if (curMenuPrincipal != null)
                    {
                        //- Génération de la liste de sous-menus pour le menu récupéré
                        sousMenu = GetMenuNodes(curMenuPrincipal.MenuID, ListeEntreesDeMenus);

                        //- Si le menu est non null et contient une arborescence, on insère le menu dans le dictionnaire des treeNodes
                        if ((curMenuPrincipal != null) && (sousMenu != null))
                            treeMenu.Add(curMenuPrincipal, sousMenu);
                        else if (curMenuPrincipal != null)
                            treeMenu.Add(curMenuPrincipal, sousMenu);
                    }
                }
            }

            return treeMenu;
        }

        List<TreeNodes> GetMenuNodes(int MainMenuID, List<CSMenuGalatee> pListeDesSousMenusDuRole)
        {
            List<CSMenuGalatee> permissions = null;
            try
            {
                if (pListeDesSousMenusDuRole == null || pListeDesSousMenusDuRole.Count == 0)
                    return null;

                //- Récuparation des sous-menu du menu principal fourni en paramètre
                var module = pListeDesSousMenusDuRole.FirstOrDefault();
                if (module != null && module.IdGroupProgram == Enumere.moduleParametrageId)
                {
                    permissions = pListeDesSousMenusDuRole.Where(perm => perm.MainMenuID == MainMenuID).OrderBy(q => q.MenuID ).ToList(); // Ranger les menus du module Parametrage en fonction du MenuText
                }
                else if (module != null && (module.IdGroupProgram == Enumere.moduleDevisId || module.Module.ToUpper() == Enumere.LibellemoduleDevis.ToUpper()))
                {
                    permissions = pListeDesSousMenusDuRole.Where(perm => perm.MainMenuID == MainMenuID).OrderBy(q => q.MenuID).ToList(); // Ranger les menus du module Devis en fonction du MenuId
                }
                else
                {
                    permissions = pListeDesSousMenusDuRole.Where(perm => perm.MainMenuID == MainMenuID).OrderBy(q => q.MenuID ).ToList();
                }

                //- On sort si le menu n'a aucun sous-menu
                if (permissions == null || permissions.Count == 0)
                    return null;

                //- Si cette entrée à des sous-menus
                List<TreeNodes> resultat = new List<TreeNodes>();
                foreach (CSMenuGalatee perm in permissions)
                {
                    //- Création d'un noeud pour chacune des entrées du sous-menu
                    TreeNodes tn = new TreeNodes(perm.MenuText);
                    if (perm.IconName != null)
                    {
                        //- ajout de l'icône dans la liste, tout en conservant l'index
                        tn.ImageIndex = compteurIL++;
                        //pListeImages.Add(perm.IconName);
                    }

                    //- Récupération du sous-menu et appel récursif pour les noeuds enfants
                    tn.Tag = perm;
                    List<TreeNodes> enfants = GetMenuNodes(perm.MenuID, pListeDesSousMenusDuRole);
                    if (enfants != null && enfants.Count > 0)
                    {
                        //- Ajout des noeuds enfant retournés
                        foreach (TreeNodes tne in enfants)
                            tn.Add(tne);
                    }
                    resultat.Add(tn);
                }
                return resultat;
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }


        public List<CSMenuGalatee> ChargerMenuEtSousMenu(string idprojetName, int idprofil)
        {
            try
            {
                DBHabilitationMenuModule db = new DBHabilitationMenuModule();
                //return db.ChargerMenuEtSousMenu(idprojetName);
                return db.ChargerMenuEtSousMenu(idprojetName, idprofil);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }

        }

        List<CSMenuGalatee> ChargerMenuEtSousMenuDuProfil(string module)
        {
            try
            {
                DBHabilitationMenuModule db = new DBHabilitationMenuModule();
                return db.ChargerMenuEtSousMenuDuProfil(module);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }


        #endregion

        # region PARAMETRAGE


        public EnumProcedureStockee returnEnumProcedureStockee()
        {
            try
            {
                return new EnumProcedureStockee();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        List<CParametre> IAuthentInitializeService.RetourneListImprante()
        {
            try
            {
                List<CParametre> ListeImprimanteRetourne = new List<CParametre>();

                ManagementScope scope = new ManagementScope(@"\root\cimv2");
                scope.Connect();

                // Select Printers from WMI Object Collections
                ManagementObjectSearcher searcher = new
                ManagementObjectSearcher("SELECT * FROM Win32_Printer");

                string printerName = "";
                string deviceId = "";
                foreach (ManagementObject printer in searcher.Get())
                {
                    deviceId = printer["DeviceID"].ToString();
                    printerName = printer["Name"].ToString().ToLower();
                    //ListeImprimanteRetourne.Add(new CParametre() { LIBELLE = deviceId, VALEUR = printerName });
                }

                //List<CParametre> tempon = new List<CParametre>();
                //PrinterSettings.StringCollection Imprimante = null;
                //PrinterSettings defaultPrinter = new PrinterSettings();
                //string defaultNmae = defaultPrinter.PrinterName;
                //Imprimante = PrinterSettings.InstalledPrinters;
                //for (int i = 0; i < Imprimante.Count; i++)
                //{
                //    CParametre UneImprimante = new CParametre();
                //    UneImprimante.LIBELLE = Imprimante[i];
                //    tempon.Add(UneImprimante);
                //}

                //foreach (CParametre impr in tempon)
                //    if (impr.LIBELLE != defaultNmae)
                //        ListeImprimanteRetourne.Add(impr);
                //CParametre def = new CParametre();
                //def.LIBELLE = defaultNmae;
                //ListeImprimanteRetourne.Insert(0, def);

                return ListeImprimanteRetourne;
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public CsStrategieSecurite GetStrategieSecuriteActif()
        {
            try
            {
                return new DBAuthentification().GetActif();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        # endregion

        #region ENUMERAIRE
        public EnumereWrap returnEnumereWrapper()
        {
            try
            {

                List<CsParametresGeneraux> param = new DB_ParametresGeneraux().SelectAllParametresGeneraux();

                //initialisation des enumeraires à partir des données de la table parametregeneraux
                Enumere.CodeFonctionCaisse = param.FirstOrDefault(p => p.CODE == "000215").LIBELLE;
                Enumere.IsDevisPrisEnCompteAuGuichet = (param.FirstOrDefault(p => p.CODE == "000232").LIBELLE == "1" ? true : false);
                Enumere.IsReglementDevisPrisEnCompteAuGuichet = (param.FirstOrDefault(p => p.CODE == "000233").LIBELLE == "1" ? true : false);
                Enumere.IsPaiementFraixDevis = (param.FirstOrDefault(p => p.CODE == "000402").LIBELLE == "1" ? true : false);
                Enumere.IsModificationAutoriserEnFacturation = (param.FirstOrDefault(p => p.CODE == "000403").LIBELLE == "1" ? true : false);
                string lib = param.FirstOrDefault(p => p.CODE.Trim() == "000300").LIBELLE;                
                //string tailleMat= param.FirstOrDefault(p => p.CODE.Trim() == "000302").LIBELLE;
                //Enumere.TailleMatricule = int.Parse(tailleMat); 
                Enumere.IsGestionGlobaleCoutFournitureDevis = lib.Trim() == "1" ? true : false;
                Enumere.IsCompteurAttribuerAuto = (param.FirstOrDefault(p => p.CODE == "000404").LIBELLE == "1" ? true : false);
                Enumere.IsUtilisateurCreeParAgent = (param.FirstOrDefault(p => p.CODE == "000405").LIBELLE == "1" ? true : false);
                if (param.FirstOrDefault(p => p.CODE == "000406") != null)
                    Enumere.FournitureCable = int.Parse(param.FirstOrDefault(p => p.CODE == "000406").LIBELLE);

                if (param.FirstOrDefault(p => p.CODE == "000407") != null)
                    Enumere.TailleNumeroCheque = int.Parse(param.FirstOrDefault(p => p.CODE == "000407").LIBELLE);

                if (param.FirstOrDefault(p => p.CODE == "000408") != null)
                    Enumere.IsResilierPriseEcompte = lib.Trim() == "1" ? true : false;

                 if (param.FirstOrDefault(p => p.CODE == "000303") != null)
                    Enumere.CheminImpressionServeur =  param.FirstOrDefault(p => p.CODE == "000303").LIBELLE;

                 if (param.FirstOrDefault(p => p.CODE == "000304") != null)
                    Enumere.CheminImpressionClient =  param.FirstOrDefault(p => p.CODE == "000304").LIBELLE;

                 
                EnumereWrap wrapEnumere = new EnumereWrap()
                {
                    LotriChangementCompteur=Enumere.LotriChangementCompteur,
                    Eau = Enumere.Eau,
                    Electricite = Enumere.Electricite,
                    ElectriciteMT = Enumere.ElectriciteMT,
                    Prepaye = Enumere.Prepaye,
                    Dial = Enumere.EnumCompteur.Dial.ToString(),
                    Monophase  = Enumere.EnumCompteur.Monophase.ToString() ,
                    Triphase = Enumere.EnumCompteur.Triphase.ToString(),
                    Four = Enumere.EnumCompteur.Four.ToString(),
                    Caisse = Enumere.Caisse,
                    Accueil = Enumere.Accueil,
                    NombrePointFournitureBT = Enumere.NombrePointFournitureBT,
                    NombrePointFournitureMT = Enumere.NombrePointFournitureMT,
                    Generale = Enumere.Generale,
                    IDGenerale = Enumere.IDGenerale,
                    CodeSiteScaBT = Enumere.CodeSiteScaBT,
                    CodeSiteScaMT = Enumere.CodeSiteScaMT,
                    StatusScelleAbîmé = Enumere.StatusScelleAbîmé ,
                    StatusScelleRompu = Enumere.StatusScelleRompu ,
                    StatusScelleRemis = Enumere.StatusScelleRemis ,
                    StatusScelleDisponible = Enumere.StatusScelleDisponible ,
                    StatusScelleTransféré = Enumere.StatusScelleTransféré ,
                    StatusScellePosé = Enumere.StatusScellePosé ,
                    StatusScelleEgaré = Enumere.StatusScelleEgaré ,

                    ActionRecuAnnulation = Enumere.ActionRecuAnnulation,
                    ActionRecuDuplicat = Enumere.ActionRecuDuplicat,
                    ActionRecuEditionNormale = Enumere.ActionRecuEditionNormale,

                    CaisseStatAnnulation = Enumere.CaisseStatAnnulation,
                    CaisseStatFONDCAISSE = Enumere.CaisseStatFONDCAISSE,
                    ConnexionABO07 = Enumere.ConnexionABO07,
                    ConnexionGALADB = Enumere.ConnexionGALADB,
                    CoperOdDFA = Enumere.CoperOdDFA,
                    CoperFact = Enumere.CoperFact,
                    CoperMOR = Enumere.CoperMOR,
                    CoperMorSolde = Enumere.CoperMorSolde,
                    CoperNAF = Enumere.CoperNAF,
                    CoperRGT = Enumere.CoperRGT,
                    CoperRCD = Enumere.CoperRCD,
                    CoperRNA = Enumere.CoperRNA,
                    CoperOdQPA = Enumere.CoperOdQPA,
                    CoperCAU = Enumere.CoperCAU,
                    CoperTRV = Enumere.CoperTRV,
                    //FK_IDCOPER = Enumere.FK_IDCOPER,
                    CoperFAB = Enumere.CoperFAB,
                    CoperFDO = Enumere.CoperFDO,
                    CoperFPO = Enumere.CoperFPO,
                    CoperFPS = Enumere.CoperFPS,
                    CoperFRP = Enumere.CoperFRP,
                    CoperEXO = Enumere.CoperEXO ,
                    CoperFPA = Enumere.CoperFPA ,
                    CoperACT = Enumere.CoperACT,
                    CoperRembAvance = Enumere.CoperRembAvance,
                    CoperRembTrvx = Enumere.CoperRembTrvx,
                    CoperRAC = Enumere.CoperRAC,
                    CoperRAD = Enumere.CoperRAD,
                    CoperUDC = Enumere.CoperUDC,
                    CoperDSD = Enumere.CoperDSD,
                    CoperRDD = Enumere.CoperRDD,
                    CoperFactureTrvxEtDivers = Enumere.CoperFactureTrvxEtDivers,
                    CoperEtudeEtSurveillance = Enumere.CoperEtudeEtSurveillance,
                    CodeSansTaxe = Enumere.CodeSansTaxe,

                    Credit = Enumere.Credit,
                    Debit = Enumere.Debit,

                    StatusDemandeInitier = Enumere.StatusDemandeInitier,
                    StatusDemandeValider = Enumere.StatusDemandeValider,
                    StatusDemandeRejeter = Enumere.StatusDemandeRejeter,
                    StatusDemandeRetirer = Enumere.StatusDemandeRetirer,

                    CoperFondCaisse = Enumere.CoperFondCaisse,
                    CoperTransfertDebit= Enumere.CoperTransfertDebit,
                    CoperTransfertDette= Enumere.CoperTransfertDette,
                    //CoperTransfertSolde = Enumere.CoperTransfertSolde,
                    //CoperTransfertRemboursement = Enumere.CoperTransfertRemboursement,

                    //MessageDevisRegler = Enumere.MessageDevisRegler,
                    EtatChequeChequeHorsPlace = Enumere.EtatChequeChequeHorsPlace,
                    EtatChequeChequeSurPlace = Enumere.EtatChequeChequeSurPlace,
                    EtatDeCaisseDejaCloture = Enumere.EtatDeCaisseDejaCloture,
                    EtatDeCaisseInexistante = Enumere.EtatDeCaisseInexistante,
                    EtatDeCaisseNonCloture = Enumere.EtatDeCaisseNonCloture,
                    EtatDeCaissePasCassier = Enumere.EtatDeCaissePasCassier,
                    EtatDeCaissePasDeNumCaiss = Enumere.EtatDeCaissePasDeNumCaiss,
                    EtatDeCaisseValider = Enumere.EtatDeCaisseValider,
                    EtatDeCaisseOuverteALaDemande = Enumere.EtatDeCaisseOuverteALaDemande,
                    EtatDeCaisseAutreSessionOuvert = Enumere.EtatDeCaisseAutreSessionOuvert,
                    MontantFraisTravaux = Enumere.MontantFraisTravaux,
                    ModePayementCheque = Enumere.ModePayementCheque,
                    ModePayementEspece = Enumere.ModePayementEspece,
                    ModePayementAjustement = Enumere.ModePayementAjustement,

                    OperationDeCaisseEncaissementDevis = Enumere.OperationDeCaisseEncaissementDevis,
                    OperationDeCaisseEncaissementFacture = Enumere.OperationDeCaisseEncaissementFacture,
                    OperationDeCaisseEncaissementFactureManuel = Enumere.OperationDeCaisseEncaissementFactureManuel,
                    OperationDeCaisseEncaissementMoratoire = Enumere.OperationDeCaisseEncaissementMoratoire,

                    TailleCentre = Enumere.TailleCentre,
                    TailleClient = int.Parse(param.FirstOrDefault(p => p.CODE == "000001").LIBELLE),// Enumere.TailleClient,
                    TailleOrdre = Enumere.TailleOrdre,
                    TailleDesReferenceClient = Enumere.TailleDesReferenceClient,
                    TailleNumCaisse = Enumere.TailleNumCaisse,
                    TailleNumDevis = Enumere.TailleNumDevis,
                    TailleMatricule = Enumere.TailleMatricule,
                    TaillePeriode = Enumere.TaillePeriode,
                    TailleFacture = Enumere.TailleFacture,
                    TailleCodeCategorie = Enumere.TailleCodeCategorie,
                    TailleCodeConso = Enumere.TailleCodeConso,
                    TailleCodeTypeClient = Enumere.TailleCodeTypeClient,
                    TailleCodeNationalite = Enumere.TailleCodeNationalite,
                    TailleCodeRelance = Enumere.TailleCodeRelance,
                    TailleDiametre = Enumere.TailleDiametre,
                    TailleUsage = Enumere.TailleUsage,
                    TailleCommune = Enumere.TailleCommune,
                    TailleQuartier = Enumere.TailleQuartier,
                    TailleSecteur = Enumere.TailleSecteur,
                    TailleRue = Enumere.TailleRue,

                    TailleCodeMateriel = Enumere.TailleCodeMateriel,
                    TailleDiametreBranchement = Enumere.TailleDiametreBranchement,
                    TailleCodeTypeCompteur = Enumere.TailleCodeTypeCompteur,
                    TailleMoisDeFacturation = Enumere.TailleMoisDeFacturation,
                    TaillePeriodicite = Enumere.TaillePeriodicite,
                    TailleCodeCivilite = Enumere.TailleCodeCivilite,
                    TailleCodeMarqueCompteur = Enumere.TailleCodeMarqueCompteur,
                    TailleCodeQuartier = Enumere.TailleCodeQuartier,
                    TailleDigitCompteur = Enumere.TailleDigitCompteur,
                    TailleTarif = Enumere.TailleTarif,
                    TailleForfait = Enumere.TailleForfait,
                    TailleCodeProduit = Enumere.TailleCodeProduit,
                    TailleNumeroLigneBatch = Enumere.TailleNumeroLigneBatch,
                    TailleNumeroDemande = Enumere.TailleNumeroDemande,
                    TailleCodeTournee = Enumere.TailleCodeTournee,
                    TailleNumeroBatch = Enumere.TailleNumeroBatch,
                    TailleCas = Enumere.TailleCas,
                    TailleDate = Enumere.TailleDate,
                    TailleNoeudbrt = Enumere.TailleNoeudbrt,
                    TailleNumeroPost = Enumere.TailleNumeroPost,
                    TailleSequencePost = Enumere.TailleSequencePost,
                    TailleCodePoste = Enumere.TailleCodePoste,
                    TailleTypeBranchement = Enumere.TailleTypeBranchement,
                    TailleCodeRegroupement = Enumere.TailleCodeRegroupement,
                    TailleTypeComptage = Enumere.TailleTypeComptage,
                    TailleNumeroCheque = Enumere.TailleNumeroCheque,

                    NombreDejour = Enumere.NombreDejour,
                    NombreMois = Enumere.NombreDejour,

                    PROPRIETRAIRE = Enumere.PROPRIETRAIRE ,
                    LOCATAIRE = Enumere.LOCATAIRE ,

                    CompteurActif = Enumere.CompteurActif,
                    CompteurInactif = Enumere.CompteurInactif,
                    CompteurActifValeur = Enumere.CompteurActifValeur,
                    CompteurInactifValeur = Enumere.CompteurInactifValeur,
                    CodeFonctionCaisse = Enumere.CodeFonctionCaisse,
                    CodeFonctionMetreur = Enumere.CodeFonctionMetreur,
                    CodeFonctionPIA_E = Enumere.CodeFonctionPIA_E,
                    CodeFonctionPIA_O = Enumere.CodeFonctionPIA_O,
                    IsCompteurAttribuerAuto = Enumere.IsCompteurAttribuerAuto ,
                    IsUtilisateurCreeParAgent = Enumere.IsUtilisateurCreeParAgent,
                    IsDistanceSupplementaireFacture = Enumere.IsDistanceSupplementaireFacture,
                    BranchementSimple = Enumere.BranchementSimple,
                    BranchementAbonement = Enumere.BranchementAbonement,
                    BranchementAbonementExtention = Enumere.BranchementAbonementExtention,
                    AbonnementSeul = Enumere.AbonnementSeul,
                    Reabonnement = Enumere.Reabonnement,
                    Resiliation = Enumere.Resiliation,
                    AchatTimbre = Enumere.AchatTimbre,
                    ReprisIndex = Enumere.ReprisIndex,
                    FermetureBrt = Enumere.FermetureBrt,
                    RemboursementTrvxNonRealise = Enumere.RemboursementTrvxNonRealise,
                    //ReouvertureBrt = Enumere.ReouvertureBrt,
                    RemboursementAvance = Enumere.RemboursementAvance,
                    ChangementCompteur = Enumere.ChangementCompteur,
                    FactureManuelle = Enumere.FactureManuelle,
                    VerificationCompteur = Enumere.VerificationCompteur,
                    RegularisationAvance = Enumere.RegularisationAvance,
                    AvoirConsomation = Enumere.AvoirConsomation,
                    AutorisationAvancementTravaux = Enumere.AutorisationAvancementTravaux ,
                    PoseCompteur = Enumere.PoseCompteur,
                    DeposeCompteur = Enumere.DeposeCompteur,
                    Etalonage = Enumere.Etalonage,
                    ModificationAbonnement = Enumere.ModificationAbonnement,
                    ModificationBranchement = Enumere.ModificationBranchement,
                    ModificationAdresse = Enumere.ModificationAdresse,
                    ModificationClient = Enumere.ModificationClient,
                    ModificationCompteur = Enumere.ModificationCompteur,
                    CorrectionDeDonnes = Enumere.CorrectionDeDonnes,
                    DimunitionPuissance = Enumere.DimunitionPuissance,
                    AugmentationPuissance = Enumere.AugmentationPuissance,
                    DemandeScelle = Enumere.DemandeScelle,
                    TransfertAbonnement = Enumere.TransfertAbonnement,
                    BranchementAbonnementMt = Enumere.BranchementAbonnementMt,
                    BranchementAbonnementEp = Enumere.BranchementAbonnementEp,
                    RemboursementParticipation = Enumere.RemboursementParticipation,
                    DepannageEp = Enumere.DepannageEp,
                    DemandeFraude = Enumere.DemandeFraude,
                    DemandeReclamation = Enumere.DemandeReclamation,

                    DepannageMT = Enumere.DepannageMT,
                    DepannagePrepayer = Enumere.DepannagePrepayer,
                    DepannageClient = Enumere.DepannageClient,
                    DepannageMaintenance = Enumere.DepannageMaintenance,
                    TransfertSiteNonMigre = Enumere.TransfertSiteNonMigre,
                    ChangementProduit = Enumere.ChangementProduit,


                    RechercheDemandeParNum = Enumere.RechercheDemandeParNum,
                    RechercheDemandeDebit = Enumere.RechercheDemandeDebit,
                    RechercheDemandeCredit = Enumere.RechercheDemandeCredit,
                    //RechercheDemandeSolde = Enumere.RechercheDemandeSolde,
                    //RechercheDemandeRemboursement = Enumere.RechercheDemandeRemboursement,
                    IsBranchementNonSubventione = Enumere.IsBranchementNonSubventione,
                    IsBranchementSubventione = Enumere.IsBranchementSubventione,
                    //MessageNonAuthorise = Enumere.MessageNonAuthorise,
                 
                    IsGestionGlobaleCoutFournitureDevis = Enumere.IsGestionGlobaleCoutFournitureDevis,

                    //Status evenement
                    EvenementCree = Enumere.EvenementCree,
                    EvenementReleve = Enumere.EvenementReleve,
                    EvenementFacture = Enumere.EvenementFacture,
                    EvenementMisAJour = Enumere.EvenementMisAJour,
                    EvenementDefacture = Enumere.EvenementDefacture,
                    EvenementRejeter = Enumere.EvenementRejeter,
                    EvenementAnnule = Enumere.EvenementAnnule,
                    EvenementSupprimer = Enumere.EvenementSupprimer,
                    EvenementPurger = Enumere.EvenementPurger,
                    //Status
                    PagerieEnquetable = Enumere.PagerieEnquetable,
                    PagerieNonEnquetable = Enumere.PagerieNonEnquetable,
                    PagerieConfirme = Enumere.PagerieConfirme,
                    //Mode application de tarif
                    ModeApplicationTarifDate = Enumere.ModeApplicationTarifDate,
                    ModeApplicationTarifPeriode = Enumere.ModeApplicationTarifPeriode,
                    //Code evenement 
                    EvenementCodeDeposeCpt = Enumere.EvenementCodeDeposeCpt,
                    EvenementCodePoseCpt = Enumere.EvenementCodePoseCpt,
                    EvenementCodeFermetureBrt = Enumere.EvenementCodeFermetureBrt,
                    EvenementCodeOuvertureBrt = Enumere.EvenementCodeOuvertureBrt,
                    EvenementFermetureBrtAvecDepose = Enumere.EvenementFermetureBrtAvecDepose,
                    EvenementCodeForfait = Enumere.EvenementCodeForfait,
                    EvenementCodeSuspensionAbon = Enumere.EvenementCodeSuspensionAbon,
                    EvenementCodeCreationLot = Enumere.EvenementCodeCreationLot,
                    EvenementCodeFactureIsole = Enumere.EvenementCodeFactureIsole,
                    EvenementCodeResiliation = Enumere.EvenementCodeResiliation,
                    EvenementCodeAvoirConso = Enumere.EvenementCodeAvoirConso,
                    EvenementCodeRejetFacture = Enumere.EvenementCodeRejetFacture,
                    EvenementCodeDefacturationLot = Enumere.EvenementCodeDefacturationLot,
                    EvenementCodeNormale = Enumere.EvenementCodeNormale,
                    EvenementCodeFactureAjustementNeg = Enumere.EvenementCodeFactureAjustementNeg,
                    EvenementCodeFactureAjustementPos = Enumere.EvenementCodeFactureAjustementPos,
                    //lotri evenement
                    LotriTermination = Enumere.LotriTermination,
                    LotriManuel = Enumere.LotriManuel,
                    LotriAjustement = Enumere.LotriAjustement,
                    //Cas de saisi
                    CasPoseCompteur = Enumere.CasPoseCompteur,
                    CasDeposeCompteur = Enumere.CasDeposeCompteur,
                    CasCreation = Enumere.CasCreation,
                    CasNindesSup = Enumere.CasNindesSup,
                    CasPassageZero = Enumere.CasPassageZero,
                    //recouvrement

                    TraitementImpaye = Enumere.TraitementImpaye,
                    CoperChqImp = Enumere.CoperChqImp,
                    TopCheqImpaye = Enumere.TopCheqImpaye,
                    LibNatureCheqImpaye = Enumere.LibNatureCheqImpaye,
                    MontantFraisRDC = Enumere.MontantFraisRDC,
                    CoperFraisChqImp = Enumere.CoperFraisChqImp,

                    IsDelete = Enumere.IsDelete,
                    IsInsertion = Enumere.IsInsertion,
                    IsUpdate = Enumere.IsUpdate,

                    /* top 1*/
                    TopGuichet = Enumere.TopGuichet,
                    TopCaisse = Enumere.TopCaisse,
                    TopFacturation = Enumere.TopFacturation,
                    TopSaisieDeMasse = Enumere.TopSaisieDeMasse,
                    TopPortables = Enumere.TopPortables,
                    TopPaiementsDeplaces = Enumere.TopPaiementsDeplaces,
                    TopMoratoire = Enumere.TopMoratoire,


                    LibFacultatif = Enumere.LibFacultatif,
                    CodeFacultatif = Enumere.CodeFacultatif,
                    CodeInterdit = Enumere.CodeInterdit,
                    LibInterdit = Enumere.LibInterdit,
                    CodeObligatoire = Enumere.CodeObligatoire,
                    LibObligatoire = Enumere.LibObligatoire,
                    CodeNormale = Enumere.CodeNormale,
                    LibNormale = Enumere.LibNormale,
                    CodeForfait = Enumere.CodeForfait,
                    LibForfait = Enumere.LibForfait,
                    CodeBloqueeSansRegularisation = Enumere.CodeBloqueeSansRegularisation,
                    LibBloqueeSansRegularisation = Enumere.LibBloqueeSansRegularisation,
                    CodeSansTarifUnitaire = Enumere.CodeSansTarifUnitaire,
                    LibSansTarifUnitaire = Enumere.LibSansTarifUnitaire,
                    CodeSansTarifAnnuel = Enumere.CodeSansTarifAnnuel,
                    LibSansTarifAnnuel = Enumere.LibSansTarifAnnuel,
                    CodeForfaitSansRegularisation = Enumere.CodeForfaitSansRegularisation,
                    LibForfaitSansRegularisation = Enumere.LibForfaitSansRegularisation,
                    CodeBloqueeAvecRegularisation = Enumere.CodeBloqueeAvecRegularisation,
                    LibBloqueeAvecRegularisation = Enumere.LibBloqueeAvecRegularisation,
                    CodeEstimeeAvecRegularisation = Enumere.CodeEstimeeAvecRegularisation,
                    LibEstimeeAvecRegularisation = Enumere.LibEstimeeAvecRegularisation,
                    CodeEstimeeSansRegularisation = Enumere.CodeEstimeeSansRegularisation,
                    LibEstimeeSansRegularisation = Enumere.LibEstimeeSansRegularisation,
                    EvenementsValeurParDefaut = Enumere.EvenementsValeurParDefaut,

                    // CHK - 28/02/2013 - Report module
                    FactureGeneraleCoper = Enumere.FactureGeneraleCoper,
                    FactureManuelleCoper = Enumere.FactureManuelleCoper,
                    FournitureCable = Enumere.FournitureCable,
                    //HGB 01-03-2013 - Administration module
                    UserAcitveAccount = Enumere.UserAcitveAccount,
                    UserInAcitveAccount = Enumere.UserInAcitveAccount,
                    UserLockeAcitveAccount = Enumere.UserLockeAcitveAccount,
                    ActionSurCentre = Enumere.ActionSurCentre,
                    ActionSurSite = Enumere.ActionSurSite,
                    ActionSurTousSite = Enumere.ActionSurTousSite,
                    
                    // AKO le 07/03/2013 
                    DemandeStatusEnAttente = Enumere.DemandeStatusEnAttente,
                    DemandeStatusPasseeEncaisse = Enumere.DemandeStatusPasseeEncaisse,
                    DemandeStatusPriseEnCompte = Enumere.DemandeStatusPriseEnCompte,
                    DemandeStatusEnCaisse = Enumere.DemandeStatusEnCaisse ,
                    NOLIEN = Enumere.NOLIEN ,
                
                    UsageEp = Enumere.UsageEp,
                    CodeConsomateurEp = Enumere.CodeConsomateurEp,
                    // Action facturation
                    Simulation = Enumere.Simulation,
                    Defacturation = Enumere.Defacturation,
                    DestructionSimulation = Enumere.DestructionSimulation,
                    Normal = Enumere.Normal,
                    DemandeDefacturation = Enumere.DemandeDefacturation ,

                    MiseAJours = Enumere.MiseAJours,
                    NonMiseAJours = Enumere.NonMiseAJours,
                    NonMiseAbonne = Enumere.NonMiseAbonne,
                    SimulationFacture = Enumere.SimulationFacture,
                    TypeComptageMaximetre = Enumere.TypeComptageMaximetre,
                    TypeComptageHoraire = Enumere.TypeComptageHoraire,
                    TypeComptageReactif = Enumere.TypeComptageReactif,
                    TypeComptagePoint = Enumere.TypeComptagePoint,
                    TypeComptagePleinne = Enumere.TypeComptagePleinne,
                    TypeComptageCreuse = Enumere.TypeComptageCreuse,
                    BorneConsohoraire = Enumere.BorneConsohoraire,

                    CategorieAgentEdm = Enumere.CategorieAgentEdm ,
                    CategorieEp = Enumere.CategorieEp ,
                    CategorieConsoInterne = Enumere.CategorieConsoInterne,
                    CodeConsomateurIndeterminer = Enumere.CodeConsomateurIndeterminer,

                    Preuve = Enumere.Preuve,
                    Schema = Enumere.Schema,
                    Manuscrit = Enumere.Manuscrit,
                    PieceIdentite = Enumere.PieceIdentite,
                    Contrat = Enumere.Contrat,
                    TitrePropriete = Enumere.TitrePropriete,
                    DossierPromoteur = Enumere.DossierPromoteur,
                    AutorisationMairie = Enumere.AutorisationMairie,
                    InstructionDG = Enumere.InstructionDG,
                    DemandePrestation = Enumere.DemandePrestation,
                    FicheDexoneration = Enumere.FicheDexoneration ,

                    LIGNEHTA = Enumere.LIGNEHTA,
                    POSTEHTABT = Enumere.POSTEHTABT,
                    LIGNEBT = Enumere.LIGNEBT,
                    ENSEMBLECOMPTAGE = Enumere.ENSEMBLECOMPTAGE,
                    DEVISBRANCHEMENT = Enumere.DEVISBRANCHEMENT,
                    DEVISDEXTENSION = Enumere.DEVISDEXTENSION,

                    TRANSMETTRE = Enumere.TRANSMETTRE,
                    REJETER = Enumere.REJETER,
                    ANNULER = Enumere.ANNULER,
                    SUSPENDRE = Enumere.SUSPENDRE,
                    NationnaliteMali = Enumere.NationnaliteMali,

                    CompteurAffecte = Enumere.CompteurAffecte,
                    CheminImpressionClient = Enumere.CheminImpressionClient,
                    CheminImpressionServeur =Enumere.CheminImpressionServeur 

                   
                };
                return wrapEnumere;
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        //public Enumere returnEnumere()
        //{
        //    try
        //    {
        //        return null;// return new Enumere();
        //    }
        //    catch (Exception ex)
        //    {
        //         ErrorManager.LogException(this, ex);
        //         return null;;
        //    }
        //}
        #endregion

        #region GESTION DE LA LICENSE

        string Decrypt(string input)
        {

            try
            {
                //byte[] _SaltByte = new byte[15] { 9, 244, 48, 109, 95, 45, 208, 63, 58, 174, 243, 66, 82, 207, 159 }; // La clé est codée sur 192 bits
                byte[] _SaltByte = new byte[30] { 2, 32, 123, 70, 80, 21, 9, 244, 48, 109, 95, 77, 45, 87, 208, 49, 98, 63, 58, 23, 174, 248, 121, 243, 66, 82, 93, 207, 159, 76 }; // La clé est codée sur 192 bits
                string Salt = Convert.ToBase64String(_SaltByte);
                byte[] encryptedBytes = Convert.FromBase64String(input);
                byte[] saltBytes = Encoding.UTF8.GetBytes(Salt);
                string decryptedString = string.Empty;
                using (var aes = new AesManaged())
                {
                    Rfc2898DeriveBytes rfc = new Rfc2898DeriveBytes(Salt, saltBytes);
                    aes.BlockSize = aes.LegalBlockSizes[0].MaxSize;
                    aes.KeySize = aes.LegalKeySizes[0].MaxSize;
                    aes.Key = rfc.GetBytes(aes.KeySize / 8);
                    aes.IV = rfc.GetBytes(aes.BlockSize / 8);

                    using (ICryptoTransform decryptTransform = aes.CreateDecryptor())
                    {
                        using (MemoryStream decryptedStream = new MemoryStream())
                        {
                            CryptoStream decryptor =
                                new CryptoStream(decryptedStream, decryptTransform, CryptoStreamMode.Write);
                            decryptor.Write(encryptedBytes, 0, encryptedBytes.Length);
                            decryptor.Flush();
                            decryptor.Close();

                            byte[] decryptBytes = decryptedStream.ToArray();
                            decryptedString =
                                UTF8Encoding.UTF8.GetString(decryptBytes, 0, decryptBytes.Length);
                        }
                    }
                }
                //byte[] _SaltByte = new byte[21] { 9, 244, 48, 109, 95, 45, 208, 63, 58, 174, 243, 66, 82, 207, 159, 243, 102, 244, 48, 109, 95 }; // La clé est codée su 192 bits.//new byte[214] { 9, 244, 48, 109, 95, 77, 45, 87, 208, 49, 98, 63, 58, 23, 174, 248, 121, 243, 66, 82, 93, 207, 159, 76 }; // La clé est codée sur 192 bits
                //string Salt = Convert.ToBase64String(_SaltByte);
                //byte[] encryptedBytes = Convert.FromBase64String(input);
                //byte[] saltBytes = Encoding.UTF8.GetBytes(Salt);
                //string decryptedString = string.Empty;
                //using (var aes = new AesManaged())
                //{
                //    Rfc2898DeriveBytes rfc = new Rfc2898DeriveBytes(Salt, saltBytes);
                //    aes.BlockSize = aes.LegalBlockSizes[0].MaxSize;
                //    aes.KeySize = aes.LegalKeySizes[0].MaxSize;
                //    aes.Key = rfc.GetBytes(aes.KeySize / 8);
                //    aes.IV = rfc.GetBytes(aes.BlockSize / 8);

                //    using (ICryptoTransform decryptTransform = aes.CreateDecryptor())
                //    {
                //        using (MemoryStream decryptedStream = new MemoryStream())
                //        {
                //            CryptoStream decryptor =
                //                new CryptoStream(decryptedStream, decryptTransform, CryptoStreamMode.Write);
                //            decryptor.Write(encryptedBytes, 0, encryptedBytes.Length);
                //            decryptor.Flush();
                //            decryptor.Close();

                //            byte[] decryptBytes = decryptedStream.ToArray();
                //            decryptedString =
                //                UTF8Encoding.UTF8.GetString(decryptBytes, 0, decryptBytes.Length);
                //        }
                //    }
                //}

                return decryptedString;
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }
        string Encrypt(string input)
        {
            try
            {
                byte[] _SaltByte = new byte[30] {2,32,123,70,80,21, 9, 244, 48, 109, 95, 77, 45, 87, 208, 49, 98, 63, 58, 23, 174, 248, 121, 243, 66, 82, 93, 207, 159, 76 }; // La clé est codée sur 192 bits
                //byte[] _SaltByte = new byte[21] { 9, 244, 48, 109, 95, 45, 208, 63, 58, 174, 243, 66, 82, 207, 159, 243, 102, 244, 48, 109, 95 };
                string Salt = Convert.ToBase64String(_SaltByte);
                byte[] utfData = UTF8Encoding.UTF8.GetBytes(input);
                byte[] saltBytes = Encoding.UTF8.GetBytes(Salt);
                string encryptedString = string.Empty;
                using (AesManaged aes = new AesManaged())
                {
                    Rfc2898DeriveBytes rfc = new Rfc2898DeriveBytes(Salt, saltBytes);

                    aes.BlockSize = aes.LegalBlockSizes[0].MaxSize;
                    aes.KeySize = aes.LegalKeySizes[0].MaxSize;
                    aes.Key = rfc.GetBytes(aes.KeySize / 8);
                    aes.IV = rfc.GetBytes(aes.BlockSize / 8);

                    using (ICryptoTransform encryptTransform = aes.CreateEncryptor())
                    {
                        using (MemoryStream encryptedStream = new MemoryStream())
                        {
                            using (CryptoStream encryptor =
                                new CryptoStream(encryptedStream, encryptTransform, CryptoStreamMode.Write))
                            {
                                encryptor.Write(utfData, 0, utfData.Length);
                                encryptor.Flush();
                                encryptor.Close();

                                byte[] encryptBytes = encryptedStream.ToArray();
                                encryptedString = Convert.ToBase64String(encryptBytes);
                            }
                        }
                    }
                }
                Console.WriteLine("");
                Console.WriteLine("Encryption successfully executed *******************");
                return encryptedString;
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;

            }
        }
        string DecryptLicense(string input)
        {

            try
            {
                byte[] _SaltByte = new byte[15] { 9, 244, 48, 109, 95, 45, 208, 63, 58, 174, 243, 66, 82, 207, 159 }; // La clé est codée sur 192 bits
                string Salt = Convert.ToBase64String(_SaltByte);
                byte[] encryptedBytes = Convert.FromBase64String(input);
                byte[] saltBytes = Encoding.UTF8.GetBytes(Salt);
                string decryptedString = string.Empty;
                using (var aes = new AesManaged())
                {
                    Rfc2898DeriveBytes rfc = new Rfc2898DeriveBytes(Salt, saltBytes);
                    aes.BlockSize = aes.LegalBlockSizes[0].MaxSize;
                    aes.KeySize = aes.LegalKeySizes[0].MaxSize;
                    aes.Key = rfc.GetBytes(aes.KeySize / 8);
                    aes.IV = rfc.GetBytes(aes.BlockSize / 8);

                    using (ICryptoTransform decryptTransform = aes.CreateDecryptor())
                    {
                        using (MemoryStream decryptedStream = new MemoryStream())
                        {
                            CryptoStream decryptor =
                                new CryptoStream(decryptedStream, decryptTransform, CryptoStreamMode.Write);
                            decryptor.Write(encryptedBytes, 0, encryptedBytes.Length);
                            decryptor.Flush();
                            decryptor.Close();

                            byte[] decryptBytes = decryptedStream.ToArray();
                            decryptedString =
                                UTF8Encoding.UTF8.GetString(decryptBytes, 0, decryptBytes.Length);
                        }
                    }
                }
                return decryptedString;
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }
        string GetLicenseInfo()
        {
            try
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.AssemblyFormat = FormatterAssemblyStyle.Simple;
                formatter.Binder = new MyBinder(); // MyBinder class code given below
                FileStream stream = File.Open(filename, FileMode.Open);
                string data = (string)formatter.Deserialize(stream);
                stream.Close();
                stream.Dispose();
                return data;
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }
        /// <summary>
        /// Créé le fichier de license sur le serveur 
        /// Ce fichier est un fichier binaire qui contient les informatios cryptées
        /// </summary>
        /// <param name="File"></param>
        /// <returns></returns>
        public string[] SaveFile(byte[] File)
        {
            try
            {
                FileStream FileStream = new FileStream(filename, FileMode.Create);
                FileStream.Write(File, 0, File.Length);
                FileStream.Close();
                FileStream.Dispose();

                AvertissementLicense = false;
                FermetureLicense = false;

                if (TimerLicense == null) // Timer permettant de mettre à jour le temps d'utilisation du logiciel depuis l'activation 
                // de la license 
                {
                    TimerLicense = new Timer(10000);
                    TimerLicense.Interval = 36000000;
                    TimerLicense.AutoReset = true;
                    TimerLicense.Elapsed += new ElapsedEventHandler(TimerLicense_Elapsed);
                    TimerLicense.Enabled = true;
                }

                string[] license = GetLicenseInfo().Split(';');

                return license;
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }
        void TimerLicense_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                // la license générée est dans le format : longueurclient;datedebut;dateExpiration;dateAvertissement;periodValidite(ms);periodeAvertissement(ms)
                string[] license = GetLicenseInfo().Split(';');
                ValiditePeriodeLicense(license);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
            }
        }
        void ValiditePeriodeLicense(string[] pLicense)
        {
            try
            {
                // la license générée est dans le format : longueurclient;datedebut;dateExpiration;dateAvertissement;periodValidite(ms);periodeAvertissement(ms)

                //verifier que la periode d'utilisation atteint la période d'avertissement
                double periodeValidite = Convert.ToDouble(Decrypt(pLicense[4]));
                double periodeAvertissement = Convert.ToDouble(Decrypt(pLicense[5]));

                if (periodeAvertissement >= periodeValidite && periodeValidite > 0)
                {
                    AvertissementLicense = true;
                    FermetureLicense = false;
                }
                else
                    if (periodeValidite <= 0 && periodeAvertissement >= periodeValidite)
                    {
                        FermetureLicense = true;
                        AvertissementLicense = true;
                    }
                    else
                        AvertissementLicense = false;

                double _periodeValide = periodeValidite;
                _periodeValide -= 86400000;
                //_periodeValide -= 21600000;
                //_periodeValide -= TimerLicense.Interval;
                string _license = pLicense[0] + ";" + pLicense[1] + ";" + pLicense[2] + ";" + pLicense[3] + ";" + Encrypt(_periodeValide.ToString()) + ";" + pLicense[5];
                File.Delete(filename);

                CreateBinaryFile(_license);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);

            }
        }
        /// <summary>
        /// Mise à jour des informations dans le fichier license
        /// </summary>
        /// <param name="pString"></param>
        void CreateBinaryFile(string pString)
        {
            try
            {
                Stream stream = File.Open(filename, FileMode.Create);
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.AssemblyFormat = FormatterAssemblyStyle.Simple;
                // On sérialise
                formatter.Serialize(stream, pString);
                stream.Close();
                stream.Dispose();
                Console.WriteLine("");
                Console.WriteLine("Binary file successfully created  *******************");
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);

            }
        }


        string recupLicenseInfo()
        {
            try
            {
                string LaLicense = string.Empty;
                if (File.Exists(filename))
                {
                    using (StreamReader sr = new StreamReader(filename))
                    {
                        if (sr.Peek() >= 0)
                        {
                            //string LaLicenseCrypt = sr.Read().ToString();
                            string LaLicenseCrypt = (sr.ReadLine()).ToString();
                            if (!string.IsNullOrEmpty(LaLicenseCrypt))
                                LaLicense = DecryptLicense(LaLicenseCrypt);
                        }
                    }
                }
                return LaLicense;
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public bool GenereLicence(string licence)
        {
            try
            {
                EcrireFichier(licence, filename);
                return true;
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }
        private void EcrireFichier(string Message, string CheminLog)
        {
            FileInfo Fichier = new FileInfo(CheminLog);
            if (Fichier.Exists) // on verifie ke le fichier existe
            {
                Fichier.Delete();
                StreamWriter monstream = new StreamWriter(CheminLog, true);
                monstream.WriteLine(Message);
                monstream.Close();
                monstream.Dispose();
            }
        }

        public string LireLicence()
        {
            try
            {
                //string Resultat = VerifieConnexionGaladbAbo07();
                //if (Resultat != "TrouveGaladb" && Resultat != "TrouveAbo07")
                //    return Resultat;

                //string[] InfoLicence = recupLicenseInfo().Split(',');
                //string DateLicence = InfoLicence[0].ToString();
                //string NbreJours = InfoLicence[1].ToString();


                //if (!string.IsNullOrEmpty(DateLicence))
                //{
                //    DateTime dt1 = Convert.ToDateTime(DateLicence);
                //    TimeSpan ts = dt1 - System.DateTime.Today.Date;
                //    int days = ts.Days;
                //    if (days < 0)
                //        return "Expire";
                //    else if (days <= int.Parse(NbreJours))
                //        return days.ToString();
                //    else return "Valide";
                //}
                //else
                //    return "NonTrouve";

                return "valide";
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public bool EcrireFichierDeGaladbConnexion(string InstanceBd, string Catalogue,string userid, string MotPasse)
        {
            try
            {
                // Specify the provider name, server and database.
                string providerName = "System.Data.SqlClient";

                // Initialize the connection string builder for the
                SqlConnectionStringBuilder sqlBuilder =
                    new SqlConnectionStringBuilder();

                // Set the properties for the data source.
                sqlBuilder.DataSource = InstanceBd;
                sqlBuilder.InitialCatalog = Catalogue;
                sqlBuilder.Password = MotPasse;
                sqlBuilder.UserID = userid;
                sqlBuilder.IntegratedSecurity = true;

                // Build the SqlConnection connection string.
                string providerString = sqlBuilder.ToString();

                // Initialize the EntityConnectionStringBuilder.
                EntityConnectionStringBuilder entityBuilder =
                    new EntityConnectionStringBuilder();

                //Set the provider name.
                entityBuilder.Provider = providerName;

                // Set the provider-specific connection string.
                entityBuilder.ProviderConnectionString = providerString;

                // Set the Metadata location.
                entityBuilder.Metadata = @"res://*/GALADB.csdl|res://*/GALADB.ssdl|res://*/GALADB.msl";

                using (EntityConnection conn = new EntityConnection(entityBuilder.ToString()))
                {
                    // Test de connexion
                    conn.Open();
                    conn.Close();
                    //

                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(fileConnexion);
                    string ConectCrypt = Encrypt(entityBuilder.ToString());

                    // Modification des noeuds XML
                    XmlNode DBNode = xmlDoc.SelectSingleNode("//connectionStrings/add[@name='" + Enumere.ConnexionGALADB + "']");

                    // Sauvegarde des modifications
                    if (DBNode == null) //si noeud inexistant alors créer
                    {
                        //throw new Exception("La chaîne de connexion '" + (cmbDataBase.SelectedItem as GalaBase).ConnectionStringName + "' inexistante.");
                        XmlNode CStrNode = xmlDoc.GetElementsByTagName("connectionStrings").Item(0);
                        if (CStrNode == null)
                            CStrNode = xmlDoc.CreateNode(System.Xml.XmlNodeType.Element, "connectionStrings", "");

                        DBNode = xmlDoc.CreateNode(System.Xml.XmlNodeType.Element, "add", "");
                        XmlAttribute attName = xmlDoc.CreateAttribute("name");

                        attName.Value = Enumere.ConnexionGALADB;

                        XmlAttribute attConnectStr = xmlDoc.CreateAttribute("connectionString");

                        DBNode.Attributes.Append(attName);
                        DBNode.Attributes.Append(attConnectStr);

                        CStrNode.AppendChild(DBNode);
                    }

                    DBNode.Attributes["connectionString"].InnerText = ConectCrypt;
                    xmlDoc.Save(fileConnexion);
                    return true;
                }
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }
        public bool EcrireFichierDeAbo07Connexion(string InstanceBd, string Catalogue, string userid, string MotPasse)
        {
            try
            {
                // Specify the provider name, server and database.
                string providerName = "System.Data.SqlClient";

                // Initialize the connection string builder for the
                SqlConnectionStringBuilder sqlBuilder =
                    new SqlConnectionStringBuilder();

                // Set the properties for the data source.
                sqlBuilder.DataSource = InstanceBd;
                sqlBuilder.InitialCatalog = Catalogue;
                sqlBuilder.Password = MotPasse;
                sqlBuilder.IntegratedSecurity = true;

                // Build the SqlConnection connection string.
                string providerString = sqlBuilder.ToString();

                // Initialize the EntityConnectionStringBuilder.
                EntityConnectionStringBuilder entityBuilder =
                    new EntityConnectionStringBuilder();

                //Set the provider name.
                entityBuilder.Provider = providerName;

                // Set the provider-specific connection string.
                entityBuilder.ProviderConnectionString = providerString;

                // Set the Metadata location.
                entityBuilder.Metadata = @"res://*/ABO07.csdl|
                            res://*/ABO07.ssdl|
                            res://*/ABO07.msl";

                using (EntityConnection conn = new EntityConnection(entityBuilder.ToString()))
                {
                    // Test de connexion
                    conn.Open();
                    conn.Close();
                    //

                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(fileConnexion);
                    string ConectCrypt = Encrypt(entityBuilder.ToString());

                    // Modification des noeuds XML
                    XmlNode DBNode = xmlDoc.SelectSingleNode("//connectionStrings/add[@name='" + Enumere.ConnexionABO07  + "']");

                    // Sauvegarde des modifications
                    if (DBNode == null) //si noeud inexistant alors créer
                    {
                        //throw new Exception("La chaîne de connexion '" + (cmbDataBase.SelectedItem as GalaBase).ConnectionStringName + "' inexistante.");
                        XmlNode CStrNode = xmlDoc.GetElementsByTagName("connectionStrings").Item(0);
                        if (CStrNode == null)
                            CStrNode = xmlDoc.CreateNode(System.Xml.XmlNodeType.Element, "connectionStrings", "");

                        DBNode = xmlDoc.CreateNode(System.Xml.XmlNodeType.Element, "add", "");
                        XmlAttribute attName = xmlDoc.CreateAttribute("name");

                        attName.Value = Enumere.ConnexionABO07;

                        XmlAttribute attConnectStr = xmlDoc.CreateAttribute("connectionString");
                        XmlAttribute attProvider = xmlDoc.CreateAttribute("providerName");
                        attProvider.Value = "System.Data.SqlClient";

                        DBNode.Attributes.Append(attName);
                        DBNode.Attributes.Append(attConnectStr);
                        DBNode.Attributes.Append(attProvider);

                        CStrNode.AppendChild(DBNode);
                    }

                    DBNode.Attributes["connectionString"].InnerText = ConectCrypt;
                    xmlDoc.Save(fileConnexion);
                    return true;
                }
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }
        public string  VerifieConnexionGaladbAbo07()
        {
            string connexionname = string.Empty;
            try
            {
                string resultat ="Trouve";
                Dictionary<string,string> lstConnexion = new Dictionary<string,string>();
                foreach (ConnectionStringSettings aValue in ConfigurationManager.ConnectionStrings)
                {
                    if (aValue.Name == Enumere.ConnexionGALADB || aValue.Name == Enumere.ConnexionABO07)
                    {
                        string DecryptValue = Decrypt(aValue.ConnectionString);
                        lstConnexion.Add(aValue.Name, DecryptValue);
                    }
                }
                if (lstConnexion.Count == 0)
                    return "AucunTrouve";
                foreach (KeyValuePair<string, string> item in lstConnexion)
                {
                    connexionname = item.Key;
                  if(!TestChaineConnection(item.Value))
                      return item.Key + "NonTrouve";
                }
                return resultat;
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return  connexionname + "NonTrouve2";

            }
        }
        public bool  TestChaineConnection(string connexion)
        {
            try
            {
                using (EntityConnection conn = new EntityConnection(connexion))
                {
                    conn.Open();
                    conn.Close();
                    return true;
                }
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }
        #endregion

        #region 07/06/2016 SYLLA

        public void TraceDeConnection(int IdUser, string Post)
        {
            try
            {
                new DBAuthentification().TraceDeConnection(IdUser, Post);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return;
            }
        }


        #endregion

        public CsUtilisateur GetByLoginName(string LoginName)
        {
            try
            {
                return new DBAuthentification().GetByLoginName(LoginName);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        [Serializable()]
        public class License
        {


            public License(string pLongueurClient, string pDatedebut, string pDatefin)
            {
                this.LongueurClient = pLongueurClient;
                this.Datedebut = pDatedebut;
                this.Datefin = pDatefin;
            }

            #region Properties
            private string _LongueurClient;
            private string _datedebut;
            private string _datefin;
            #endregion

            #region Getters & Setters
            public string LongueurClient
            {
                get { return _LongueurClient; }
                set { _LongueurClient = value; }
            }
            public string Datedebut
            {
                get { return _datedebut; }
                set { _datedebut = value; }
            }
            public string Datefin
            {
                get { return _datefin; }
                set { _datefin = value; }
            }
            #endregion
        }

        #region  IMPRESSSION
        public String getDefaultPrinter()
        {
            PrinterSettings ps = new PrinterSettings();
            return ps.PrinterName;
        }
        public bool? PrintReceipt(byte[] pRenderStream)
        {
            string printer = string.Empty;
            try
            {
                Stream stream = new MemoryStream(pRenderStream);
                m_streams.Add(stream);
                ErrorManager.WriteInLogFile(this,"impression");
                //printer = getDefaultPrinter();
                Print();
                return true;
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message + ":::" + printer);
                return false;
            }
        }
        void Print()
        {
            try
            {
                if (m_streams == null || m_streams.Count == 0)
                    return;

                PrintDocument printDoc = new PrintDocument();
                PrinterSettings ps = new PrinterSettings();
                printDoc.PrinterSettings = ps;
                printDoc.PrintPage += new PrintPageEventHandler(PrintPage);
                printDoc.Print();

                foreach (Stream stream in m_streams)
                {
                    stream.Flush();
                    stream.Close();
                }
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                throw ex;
            }

        }
        void PrintPage(object sender, PrintPageEventArgs ev)
        {
            try
            {
                Metafile pageImage = new Metafile(m_streams[m_currentPageIndex]);
                ev.Graphics.DrawImage(pageImage, ev.PageBounds);
                m_currentPageIndex++;
                ev.HasMorePages = (m_currentPageIndex < m_streams.Count);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }

    internal sealed class MyBinder : SerializationBinder
    {
        public override Type BindToType(string assemblyName, string typeName)
        {
            Type ttd = null;
            try
            {
                string toassname = assemblyName.Split(',')[0];
                Assembly[] asmblies = AppDomain.CurrentDomain.GetAssemblies();
                foreach (Assembly ass in asmblies)
                {
                    if (ass.FullName.Split(',')[0] == toassname)
                    {
                        ttd = ass.GetType(typeName);
                        break;
                    }
                }
            }
            catch (System.Exception e)
            {
            }
            return ttd;
        }
    }


    

}
