using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace Galatee.Entity.Model
{
    public static class AuthentProcedures
    {
        #region 07/06/2016 SYLLA
        public static void TraceDeConnection(int IdUser, string Post)
        {
            try
            {
                using (galadbEntities Context = new galadbEntities())
                {
                    var ADMUTILISATEUR = Context.ADMUTILISATEUR.FirstOrDefault(a => a.PK_ID == IdUser);
                    ADMHISTORIQUECONNECTION ADMHISTORIQUECONNECTION = new ADMHISTORIQUECONNECTION();
                    ADMHISTORIQUECONNECTION.DateConnection = DateTime.Now;
                    ADMHISTORIQUECONNECTION.FK_IDADMUTILISATEUR = ADMUTILISATEUR.PK_ID;
                    ADMHISTORIQUECONNECTION.Login = ADMUTILISATEUR.LOGINNAME;
                    ADMHISTORIQUECONNECTION.Matricule = ADMUTILISATEUR.MATRICULE;
                    ADMHISTORIQUECONNECTION.POSTE = Post;


                    Entities.InsertEntity<ADMHISTORIQUECONNECTION>(ADMHISTORIQUECONNECTION);

                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        #endregion

        public static DataTable SelectHabilitationByUser(int pIduser)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    List<PROFILSUTILISATEUR> profiluser = context.PROFILSUTILISATEUR.Where(t=>t.FK_IDADMUTILISATEUR == pIduser && t.DATEFIN == null ).ToList() ;
                    
                    IEnumerable<object> query =
                    (from _profiluser in profiluser
                     from _menuPro in _profiluser.PROFIL.FONCTION.MODULESDEFONCTION
                    select new
                    {
                        FK_IDPROFIL = _profiluser.PROFIL.PK_ID,
                        FK_CODEFONCTION = _profiluser.PROFIL.FONCTION.PK_ID ,
                        FK_IDMODULE = _menuPro.FK_IDMODULE,
                        FK_IDGROUPPROGRAM = _menuPro.MODULE.FK_IDGROUPPROGRAM,
                        ProgramName = _menuPro.MODULE.LIBELLE,
                        ModuleName = _menuPro.MODULE.LIBELLE,
                        Code = _menuPro.MODULE.CODE,
                        //PK_ID = _menuPro.ADMMENU.PK_ID,
                        //FK_IDMENU = _menuPro.ADMMENU.PK_ID,
                        
                    }).Distinct();
                    
                    //IEnumerable<object> query =
                    //(from _profiluser in profiluser
                    //from _menuPro in context.MENUSDUPROFIL 
                    //select new
                    //{
                    //    FK_CODEFONCTION = _profiluser.PROFIL.FONCTION.PK_ID ,
                    //    FK_IDMODULE = _menuPro.ADMMENU.MODULE1.PK_ID,
                    //    FK_IDGROUPPROGRAM = _menuPro.ADMMENU.MODULE1.GROUPPROGRAM.PK_ID,
                    //    ProgramName = _menuPro.ADMMENU.MODULE1.LIBELLE,
                    //    ModuleName = _menuPro.ADMMENU.MODULE1.LIBELLE,
                    //    Code = _menuPro.ADMMENU.MODULE1.CODE,
                    //    //PK_ID = _menuPro.ADMMENU.PK_ID,
                    //    //FK_IDMENU = _menuPro.ADMMENU.PK_ID,
                        
                    //}).Distinct();

                    return Galatee.Tools.Utility.ListToDataTable(query.Distinct());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable  ListeMenuSousMenu(string pModuleName,int Fk_idProfil)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    var admMenu = context.ADMMENU;
                    var MenuProfil = context.MENUSDUPROFIL ;

                    IEnumerable<object> query =

                    from _MenuProfil in MenuProfil
                    where _MenuProfil.ADMMENU.MODULE1.CODE == pModuleName && _MenuProfil.FK_IDPROFIL ==Fk_idProfil 
                    select new
                    {
                        Module = _MenuProfil.ADMMENU.MODULE,
                        MenuID = _MenuProfil.ADMMENU.PK_ID, //PK_MENUID,
                        MenuText = _MenuProfil.ADMMENU.MENUTEXT,
                        MainMenuID = _MenuProfil.ADMMENU.MAINMENUID,
                        FormName = _MenuProfil.ADMMENU.FORMENAME,
                        IsDock = _MenuProfil.ADMMENU.ISDOCK,
                        IsControl = _MenuProfil.ADMMENU.ISCONTROLE,
                        IdGroupProgram = _MenuProfil.ADMMENU.MODULE1.FK_IDGROUPPROGRAM,
                        MenuOrder = _MenuProfil.ADMMENU.MENUORDER,
                    };
                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public static DataTable ListeMenuParHabilitationProfil( string module)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    var admMenu = context.ADMMENU;
                    var Program = context.MODULE;

                    IEnumerable<object> query =

                    from _admMenu in admMenu
                    join _Program in Program on _admMenu.MODULE.ToLower() equals _Program.CODE.ToLower()
                    where _admMenu.MODULE.ToUpper() == module.ToUpper()
                    select new
                    {
                        Module = _admMenu.MODULE,
                        MenuID = _admMenu.PK_ID, //PK_MENUID,
                        MenuText = _admMenu.MENUTEXT,
                        MainMenuID = _admMenu.MAINMENUID,
                        FormName = _admMenu.FORMENAME,
                        IsDock = _admMenu.ISDOCK,
                        IsControl = _admMenu.ISCONTROLE,
                        IdGroupProgram = _Program.FK_IDGROUPPROGRAM,
                        MenuOrder = _admMenu.MENUORDER,
                    };

                    return Galatee.Tools.Utility.ListToDataTable(query);

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable AdmStrategieSecuriteActive()
        {
            try
            {

                // load sequence of scalars.
                //IEnumerable<DataRow> query = null;
                //IEnumerable<object> query = null;

                //using (galadbEntities context = new galadbEntities(Entities.GetGaladbConnectionString()))
                using (galadbEntities context = new galadbEntities())
                {
                    var AdmStrategieSecurite = context.ADMSTRATEGIESECURITE;//.AsEnumerable();
                    IEnumerable<object> query =
                    from s in AdmStrategieSecurite
                    where s.ACTIF == true
                    select new { 
                     
                         s.ACTIF,
                         s.CHIFFREMENTREVERSIBLEPASSWORD,
                         s.DATECREATION,
                         s.DATEMODIFICATION,
                         s.DUREEMAXIMALEPASSWORD,
                         s.DUREEMINIMALEPASSWORD,
                         s.DUREEVERROUILLAGECOMPTE,
                         s.DUREEVEUILLESESSION,
                         s.HISTORIQUENOMBREPASSWORD,
                         s.LIBELLE,
                         s.LONGUEURMINIMALEPASSWORD,
                         s.NEPASCONTENIRNOMCOMPTE,
                         s.NOMBREMAXIMALECHECSOUVERTURESESSION,
                         s.NOMBREMINIMALCARACTERENONALPHABETIQUES,
                         s.NOMBREMINIMALCARACTERESCHIFFRES,
                         s.NOMBREMINIMALCARACTERESMAJUSCULES,
                         s.NOMBREMINIMALCARACTERESMINISCULES,
                         s.REINITIALISERCOMPTEURVERROUILLAGECOMPTEAPRES,
                         s.TOUCHEVERROUILLAGESESSION,
                         s.USERCREATION,
                         s.USERMODIFICATION
                    } ;

                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable GetUserByLoginName(string pLogin)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    var AdmUtilisateur = context.ADMUTILISATEUR;
                    IEnumerable<object> query =
                    from admUtilisateur in AdmUtilisateur
                    where admUtilisateur.LOGINNAME.ToUpper() == pLogin.ToUpper()
                    select new
                    {
                        CENTRE = admUtilisateur.CENTRE ,
                        admUtilisateur.MATRICULE,
                        admUtilisateur.LIBELLE,
                        admUtilisateur.PASSE,
                        admUtilisateur.DMAJ,
                        admUtilisateur.TRANS,
                        admUtilisateur.LOGINNAME,
                        admUtilisateur.E_MAIL,
                        admUtilisateur.DATEDERNIEREMODIFICATION,
                        admUtilisateur.DATEDEBUTVALIDITE,
                        admUtilisateur.DATEFINVALIDITE,
                        admUtilisateur.STATUSCOMPTE,
                        admUtilisateur.DATEDERNIEREMODIFICATIONPASSWORD,
                        admUtilisateur.INITUSERPASSWORD,
                        admUtilisateur.NOMBREECHECSOUVERTURESESSION,
                        admUtilisateur.DATEDERNIERECONNEXION,
                        admUtilisateur.DERNIERECONNEXIONREUSSIE,
                        admUtilisateur.DATEDERNIERVERROUILLAGE,
                        admUtilisateur.BRANCHE,
                        admUtilisateur.PERIMETREACTION,
                        admUtilisateur.ESTSUPPRIMER,
                        admUtilisateur.USERCREATION,
                        admUtilisateur.DATEMODIFICATION,
                        admUtilisateur.USERMODIFICATION,
                        admUtilisateur.DATECREATION,
                        ////admUtilisateur.FK_IDFONCTION,
                        admUtilisateur.FK_IDCENTRE,
                        FK_IDANCIENCENTRE=  admUtilisateur.FK_IDCENTRE,
                        PK_ID = admUtilisateur.PK_ID,
                        admUtilisateur.FK_IDSTATUS,
                        admUtilisateur.COMPTEWINDOW,
                        NOM = admUtilisateur.LIBELLE,
                        admUtilisateur.TELEPHONE ,
                        LIBELLESTATUSCOMPTE = admUtilisateur.ADMSTATUSCOMPTE.LIBELLE,
                        LIBELLECENTRE = admUtilisateur.CENTRE1.LIBELLE,
                        ////LIBELLEFONCTION = admUtilisateur.FONCTION1.ROLENAME,
                    };
                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable GetCentreAffectation(int pIdUSer)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    var CentreAffectation = context.CENTREAFFECTATION ;
                    IEnumerable<object> query =
                    from c in CentreAffectation
                    where c.FK_IDADMUTILISATEUR == pIdUSer 
                    select new
                    {
                        c.PK_ID ,
                        c.FK_IDCENTRE ,
                        c.FK_IDADMUTILISATEUR ,
                        c.DATEFINVALIDITE ,
                        c.DATEDEBUTVALIDITE 
                    };
                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable GetProfilUserByLoginName(int  pIdUSer)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    var profiluser = context.PROFILSUTILISATEUR ;
                    IEnumerable<object> query =
                    from c in profiluser 
                    join a in context.MODULESDEFONCTION on c.PROFIL.FK_IDFONCTION equals a.FK_IDFONCTION
                    where c.FK_IDADMUTILISATEUR == pIdUSer 
                    //&& c.DATEDEBUT.Value <= System.DateTime.Today.Date
                    // && (c.DATEFIN >= System.DateTime.Today || c.DATEFIN == null)
                    select new
                    {
                        c.PROFIL.CODE,
                        c.PROFIL.FK_IDFONCTION,
                        c.PROFIL.LIBELLE,
                        c.PK_ID ,
                        c.DATEDEBUT ,
                        c.DATEFIN ,
                        CODEFONCTION = c.PROFIL.FONCTION.CODE,
                        c.PROFIL.FONCTION.ROLENAME,
                        MODULE =   a.MODULE.CODE ,
                        c.FK_IDPROFIL ,
                        c.FK_IDADMUTILISATEUR
                    };
                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable GetCentreDesProfilsUserByLoginName(int pIdUSer,int pIdrofil)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    var Centreuser = context.CENTREDUPROFIL ;
                    IEnumerable<object> query =
                    from c in Centreuser
                    where c.FK_IDADMUTILISATEUR == pIdUSer && c.FK_IDPROFIL == pIdrofil  && c.DATEFINVALIDITE == null 
                    select new
                    {
                        c.PK_ID,
                        c.FK_IDADMUTILISATEUR,
                        c.FK_IDPROFIL,
                        c.FK_IDCENTRE,
                        c.DATEDEBUTVALIDITE,
                        c.DATEFINVALIDITE,
                        FK_IDSITE= c.CENTRE1.SITE.PK_ID 
                    };
                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public static DataTable GetCentreDesProfilsUserByLoginName(string pLogin)
        //{
        //    try
        //    {
        //        using (galadbEntities context = new galadbEntities())
        //        {
        //            var AdmUtilisateur = context.ADMUTILISATEUR;
        //            IEnumerable<object> query =
        //            from admUtilisateur in AdmUtilisateur
        //            from c in admUtilisateur.CENTREDUPROFIL
        //            where admUtilisateur.LOGINNAME.ToUpper() == pLogin.ToUpper()
        //            select new
        //            {
        //                c.CENTRE.PK_ID,
        //                c.CENTRE.LIBELLE,
        //                c.CENTRE.FK_IDCODESITE,
                        
        //                //c.PROFIL.pk
        //                //c.CENTRE.PK_ID,
        //                //CODEFONCTION = c.PROFIL.FONCTION.CODE,
        //                //c.CENTRE.FONCTION.ROLENAME,
        //                //c.CENTRE.FONCTION

        //            };
        //            return Galatee.Tools.Utility.ListToDataTable(query);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

    }
}
