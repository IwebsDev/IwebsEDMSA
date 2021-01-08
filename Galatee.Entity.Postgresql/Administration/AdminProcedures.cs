using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Transactions;

namespace Galatee.Entity.Model
{
    public static class AdminProcedures
    {
      public static DataTable RetourneInfoUtilisateurConnecte(string pLogin)
        {
         try 
	        {	        
		     using (galadbEntities context = new galadbEntities()) 
                { 
                    var AdmUtilisateur = context.ADMUTILISATEUR ;
                    var AdmStatusCompte = context.ADMSTATUSCOMPTE;
                    var Fonction = context.FONCTION;

                        IEnumerable<object> query =

                        from admUtilisateur in AdmUtilisateur
                        from pu in admUtilisateur.PROFILSUTILISATEUR
                        where admUtilisateur.LOGINNAME.ToUpper() == pLogin.ToUpper()
                        select new
                        {
                            PK_ID = admUtilisateur.PK_ID,
                            matricule = admUtilisateur.MATRICULE,
                            LIBELLE = admUtilisateur.LIBELLE,
                            codefontion = pu.PROFIL.FONCTION.CODE,// admUtilisateur.FONCTION1.CODE  ,
                            LibelleFontion = pu.PROFIL.FONCTION.ROLENAME,
                            isAdmin = pu.PROFIL.FONCTION.ESTADMIN,
                            CENTRE = admUtilisateur.CENTRE1.CODE,
                            PerimetreAction = admUtilisateur.PERIMETREACTION,
                            LibelleCentre = admUtilisateur.CENTRE1.LIBELLE ,
                            CODESITE = admUtilisateur.CENTRE1.SITE .CODE,
                            LIBELLESITE = admUtilisateur.CENTRE1.SITE.LIBELLE ,
                            FK_IDFONCTION = pu.PROFIL.FONCTION.PK_ID
                        };

                        return Galatee.Tools.Utility.ListToDataTable(query);
                }
	        }
	        catch (Exception ex)
	        {
		        throw ex;
	        }
         }

      public static DataTable RetourneListeToutUtilisateur()
      {
          try
          {
              using (galadbEntities context = new galadbEntities())
              {
                  var AdmUtilisateur = context.ADMUTILISATEUR;
                      //.Where(u=> u.ESTSUPPRIMER == false);
                  IEnumerable<object> query =
                  from admUtilisateur in AdmUtilisateur
                  //from pu in admUtilisateur.PROFILSUTILISATEUR
                  select new
                  {
                      PK_ID = admUtilisateur.PK_ID,
                      LoginName = admUtilisateur.LOGINNAME,
                      DateDerniereModification = admUtilisateur.DATEDERNIEREMODIFICATION,
                      DateDebutValidite = admUtilisateur.DATEDEBUTVALIDITE,
                      DateFinValidite = admUtilisateur.DATEFINVALIDITE,
                      admUtilisateur.FK_IDSTATUS,
                      PASSE = admUtilisateur.PASSE,
                      DateDerniereModificationPassword = admUtilisateur.DATEDERNIEREMODIFICATIONPASSWORD,
                      InitUserPassword = admUtilisateur.INITUSERPASSWORD,
                      NombreEchecsOuvertureSession = admUtilisateur.NOMBREECHECSOUVERTURESESSION,
                      DateDerniereConnexion = admUtilisateur.DATEDERNIERECONNEXION,
                      DerniereConnexionReussie = admUtilisateur.DERNIERECONNEXIONREUSSIE,
                      DateDernierVerrouillage = admUtilisateur.DATEDERNIERVERROUILLAGE,
                      LIBELLE = admUtilisateur.LIBELLE,
                      LIBELLESTATUSCOMPTE = admUtilisateur.ADMSTATUSCOMPTE.LIBELLE,
                      CENTRE = admUtilisateur.CENTRE1.CODE,
                      Matricule = admUtilisateur.MATRICULE,
                      //FONCTION = pu.PROFIL.FONCTION.CODE,
                      //LibelleFonction = pu.PROFIL.FONCTION.ROLENAME ,
                      E_Mail = admUtilisateur.E_MAIL,
                      //EstAdmin = pu.PROFIL.FONCTION.ESTADMIN,
                      Branche = admUtilisateur.BRANCHE,
                      PerimetreAction = admUtilisateur.PERIMETREACTION,
                      //RoleDisplayName = pu.PROFIL.FONCTION.ROLENAME,
                      EstSupprimer = admUtilisateur.ESTSUPPRIMER,
                      USERCREATION = admUtilisateur.USERCREATION,
                      admUtilisateur.DATECREATION ,
                      LibelleCentre = admUtilisateur.CENTRE1.LIBELLE,
                      admUtilisateur.FK_IDCENTRE,
                      //admUtilisateur.PROFILSUTILISATEUR ,
                      //admUtilisateur.FK_IDFONCTION,

                      
                  };
                  return Galatee.Tools.Utility.ListToDataTable(query);
              }
          }
          catch (Exception ex)
          {
              throw ex;
          }
      }
      public static DataTable RetourneHistoriqueConnectionFromListUser(List<int?> idUser)
      {
          try
          {
              using (galadbEntities context = new galadbEntities())
              {
                  var histo = context.ADMHISTORIQUECONNECTION;
                  IEnumerable<object> query =
                      from x in histo
                      where idUser.Contains(x.FK_IDADMUTILISATEUR)
                      select new
                      {
                          IDUSER = x.Login,
                          DATECREATION = x.DateConnection,
                          NOMUSER = x.ADMUTILISATEUR.LIBELLE,
                          x.POSTE 
                      };
                  return Galatee.Tools.Utility.ListToDataTable(query);
              }
          }
          catch (Exception ex)
          {
              throw ex;
          }
      }
      public static DataTable RetourneHistoriquePasswordFromListUser(List<int?> idUser)
      {
          try
          {
              using (galadbEntities context = new galadbEntities())
              {
                  var histo = context.ADMHISTORIQUEPASSWORD;
                  IEnumerable<object> query =
                      from x in histo
                      where idUser.Contains(x.FK_IDADMUTILISATEUR)
                      select new
                      {
                          x.IDUSER,
                          x.USERCREATION,
                          x.DATECREATION,
                          NOMUSER = x.ADMUTILISATEUR.LIBELLE,
                          USERMODIFICATION = (context.ADMUTILISATEUR.FirstOrDefault(t => t.MATRICULE == x.USERCREATION) != null) ? context.ADMUTILISATEUR.FirstOrDefault(t => t.MATRICULE == x.USERCREATION).LIBELLE : string.Empty
                      };
                  return Galatee.Tools.Utility.ListToDataTable(query);
              }
          }
          catch (Exception ex)
          {
              throw ex;
          }
      }

      public static DataTable RetourneListePerimetreUtilisateur(List<int> lstCentrePerimetreAction)
      {
          try
          {
              using (galadbEntities context = new galadbEntities())
              {
                  var AdmUtilisateur = context.ADMUTILISATEUR
                      .Where(u => u.ESTSUPPRIMER == false);
                  IEnumerable<object> query =
                  from admUtilisateur in AdmUtilisateur
                  //from prof in admUtilisateur.PROFILSUTILISATEUR 
                  where lstCentrePerimetreAction.Contains(admUtilisateur.FK_IDCENTRE)
                  //&&
                   //prof.DATEFIN == null 
                  select new
                  {
                      PK_ID = admUtilisateur.PK_ID,
                      LoginName = admUtilisateur.LOGINNAME,
                      DateDerniereModification = admUtilisateur.DATEDERNIEREMODIFICATION,
                      DateDebutValidite = admUtilisateur.DATEDEBUTVALIDITE,
                      DateFinValidite = admUtilisateur.DATEFINVALIDITE,
                      admUtilisateur.FK_IDSTATUS,
                      PASSE = admUtilisateur.PASSE,
                      DateDerniereModificationPassword = admUtilisateur.DATEDERNIEREMODIFICATIONPASSWORD,
                      InitUserPassword = admUtilisateur.INITUSERPASSWORD,
                      NombreEchecsOuvertureSession = admUtilisateur.NOMBREECHECSOUVERTURESESSION,
                      DateDerniereConnexion = admUtilisateur.DATEDERNIERECONNEXION,
                      DerniereConnexionReussie = admUtilisateur.DERNIERECONNEXIONREUSSIE,
                      DateDernierVerrouillage = admUtilisateur.DATEDERNIERVERROUILLAGE,
                      LIBELLE = admUtilisateur.LIBELLE,
                      LIBELLESTATUSCOMPTE = admUtilisateur.ADMSTATUSCOMPTE.LIBELLE,
                      CENTRE = admUtilisateur.CENTRE1.CODE,
                      Matricule = admUtilisateur.MATRICULE,
                      //FONCTION = pu.PROFIL.FONCTION.CODE,
                      //LibelleFonction = pu.PROFIL.FONCTION.ROLENAME ,
                      E_Mail = admUtilisateur.E_MAIL,
                      //EstAdmin = pu.PROFIL.FONCTION.ESTADMIN,
                      Branche = admUtilisateur.BRANCHE,
                      PerimetreAction = admUtilisateur.PERIMETREACTION,
                      //RoleDisplayName = pu.PROFIL.FONCTION.ROLENAME,
                      EstSupprimer = admUtilisateur.ESTSUPPRIMER,
                      USERCREATION = admUtilisateur.USERCREATION,
                      admUtilisateur.DATECREATION,
                      LibelleCentre = admUtilisateur.CENTRE1.LIBELLE,
                      admUtilisateur.FK_IDCENTRE,
                      //admUtilisateur.PROFILSUTILISATEUR ,
                      //admUtilisateur.FK_IDFONCTION,


                  };
                  return Galatee.Tools.Utility.ListToDataTable(query);
              }
          }
          catch (Exception ex)
          {
              throw ex;
          }
      }

        
      public static DataTable RetourneProfilByFonction(string fonction)
      {

          try
          {
              using (galadbEntities context = new galadbEntities())
              {

                  var Module = context.PROFIL;

                  IEnumerable<object> query =

                      from x in Module.Where( p => p.FK_IDFONCTION.ToString() == fonction)
                      select new
                      {
                          
                          PK_ID = x.PK_ID,
                          FK_IDFONCTION = x.FK_IDFONCTION,
                          LIBELLE = x.LIBELLE,
                          CODE = x.CODE 

                      };
                  return Galatee.Tools.Utility.ListToDataTable(query);
              }
          }
          catch (Exception ex)
          {
              throw ex;
          }
      }

      public static DataTable RetourneProfilByID(int fonction)
      {

          try
          {
              using (galadbEntities context = new galadbEntities())
              {

                  var Module = context.PROFIL;

                  IEnumerable<object> query =

                      from x in Module.Where( p => p.PK_ID == fonction)
                      select new
                      {                          
                          PK_ID = x.PK_ID,
                          FK_IDFONCTION = x.FK_IDFONCTION,
                          LIBELLE = x.LIBELLE,
                          CODE = x.CODE ,                         
                      };
                  return Galatee.Tools.Utility.ListToDataTable(query);
              }
          }
          catch (Exception ex)
          {
              throw ex;
          }
      }
        

      public static DataTable RetourneListeToutProfilUtilisateur()
      {

          try
          {
              using (galadbEntities context = new galadbEntities())
              {
                  var AdmProfil = context.PROFIL;
                  IEnumerable<object> query =
                  from _Profil in AdmProfil

                  select new
                  {
                      PK_ID = _Profil.PK_ID,
                      FK_IDFONCTION = _Profil.FK_IDFONCTION,
                      LIBELLE = _Profil.LIBELLE,
                      CODE = _Profil.CODE,
                  };
                  return Galatee.Tools.Utility.ListToDataTable(query);
              }
          }
          catch (Exception ex)
          {
              throw ex;
          }
      }

      public static DataTable RetourneListeToutStrategieSecurite()
      {
          try
          {
              using (galadbEntities context = new galadbEntities())
              {
                  var AdmStrategieSecurite = context.ADMSTRATEGIESECURITE;

                  IEnumerable<object> query =
                  from _AdmStrategieSecurite in AdmStrategieSecurite
                  select _AdmStrategieSecurite;

                  return Galatee.Tools.Utility.ListToDataTable(query);
              }
          }
          catch (Exception ex)
          {
              throw ex;
          }
      }

      public static DataTable RetourneListeToutModuleEtSousMenu()
      {
          try
          {
              using (galadbEntities context = new galadbEntities())
              {
                  var AdmMenus = context.ADMMENU.Distinct();
                  var Program = context.MODULE;

                  IEnumerable<object> query =
                    
                      from _AdmMenus in AdmMenus
                      join _Program in Program
                      on _AdmMenus.MODULE.Trim()
                      equals _Program.CODE.Trim()
                      select new
                      {
                          FK_IDGROUPPROGRAM = _Program.FK_IDGROUPPROGRAM,
                          ID = _Program.PK_ID,
                          CODE = _Program.CODE,
                          MAINMENUID = _AdmMenus.MAINMENUID,
                          PK_MENUID = _AdmMenus.PK_ID,
                          LIBELLE = _Program.LIBELLE,
                          MENUTEXT = _AdmMenus.MENUTEXT,
                      };
                      return Galatee.Tools.Utility.ListToDataTable(query);
              }
          }
          catch (Exception ex)
          {
              throw ex;
          }
      }

      public static DataTable RetourneListeToutModuleFonction()
      {
          try
          {
              using (galadbEntities context = new galadbEntities())
              {
                  var modules = context.MODULE.Distinct();

                  IEnumerable<object> query =

                      from _module in modules

                      select new
                      {
                            PK_ID = _module.PK_ID ,
                            FK_IDGROUPPROGRAM = _module.FK_IDGROUPPROGRAM,
                            CODE = _module.CODE,
                            LIBELLE = _module.LIBELLE
                      };
                  return Galatee.Tools.Utility.ListToDataTable(query);
              }
          }
          catch (Exception ex)
          {
              throw ex;
          }
      }

      public static List<MODULE> RetourneListeToutModuleFonctionTotal(galadbEntities context)
      {
          try
          {
                 

                    var modules = context.MODULE;
                    var query_ = (from d in modules
                                  select d);
                    return (List<MODULE>)query_.ToList();
              
          }
          catch (Exception ex)
          {
              throw ex;
          }
      }

      public static List<ADMMENU> RetourneListeToutMenuProfilTotal(galadbEntities context)
      {
          try
          {


              var modules = context.ADMMENU;
              var query_ = (from d in modules
                            select d);
              return (List<ADMMENU>)query_.ToList();

          }
          catch (Exception ex)
          {
              throw ex;
          }
      }

      public static DataTable RetourneMenusParFonction(int pProfil)
      {
          try
          {
              using (galadbEntities context = new galadbEntities())
              {
                  //var HabilitationProgram = context.HABILITATIONPROGRAM.Where(h => h.FONCTION.ROLENAME.ToUpper() == pProfil.ToUpper());

                  var Module = context.MODULE ;

                  IEnumerable<object> query =

                      from x in Module
                      from y in x.MODULESDEFONCTION 
                      from z in x.ADMMENU 
                      where y.FK_IDFONCTION == pProfil 
                      select new
                      {
                          MODULE = x.LIBELLE ,
                          PK_ID = z.PK_ID ,
                          MENUTEXT = z.MENUTEXT,
                          MAINMENUID = z.MAINMENUID,
                          FK_IDMODULE = x.PK_ID ,
                          FK_IDGROUPPROGRAM = x.FK_IDGROUPPROGRAM 

                      };
                  return Galatee.Tools.Utility.ListToDataTable(query);
              }
          }
          catch (Exception ex)
          {
              throw ex;
          }
      }
      public static DataTable RetourneHabilitationProgramParModule(string pProfil)
      {
          try
          {
              using (galadbEntities context = new galadbEntities())
              {
                  var module = context.MODULE;
                  //var HabilitationProgram = context.MODULE.Where(h => h.FONCTION.CODE.ToUpper() == pProfil.ToUpper());
                  IEnumerable<object> query =

                      from _module in module
                      from _modulefonction in _module.MODULESDEFONCTION
                      where _modulefonction.FONCTION.CODE.ToUpper() == pProfil.ToUpper()
                      select new
                      {
                          PK_ID = _module.PK_ID,
                          FK_IDGROUPPROGRAM = _module.FK_IDGROUPPROGRAM,
                          CODE = _module.CODE,
                          USERCREATION = _module.USERCREATION,
                          DATECREATION = _module.DATECREATION,
                          DATEMODIFICATION = _module.DATEMODIFICATION,
                          USERMODIFICATION = _module.USERMODIFICATION,
                          Libelle = _module.LIBELLE

                      };
                  return Galatee.Tools.Utility.ListToDataTable(query);
              }
          }
          catch (Exception ex)
          {
              throw ex;
          }
      }
      public static bool InsertionModuleDeFonction( List<MODULESDEFONCTION > pModuleListe) 
      {
          try
          {
              galadbEntities context = new galadbEntities();
              int laFonction = pModuleListe[0].FK_IDFONCTION;
              List<MODULESDEFONCTION> lstAncienneHabile = context.MODULESDEFONCTION.Where(t=>t.FK_IDFONCTION == laFonction).ToList();
              Entities.DeleteEntity<MODULESDEFONCTION>(lstAncienneHabile, context);

              Entities.InsertEntity<MODULESDEFONCTION>(pModuleListe, context);

              context.SaveChanges();
              return true;
          }
          catch (Exception ex)
          {
              throw ex;
          }
      }

      public static bool InsertionHabilitationProfil(List<PROFIL> pModuleListe)
      {
          try
          {
              galadbEntities context = new galadbEntities();
              //int laFonction = pModuleListe[0].FK_IDFONCTION;
              //List<PROFIL> lstAncienneHabile = context.MODULESDEFONCTION.Where(t => t.FK_IDFONCTION == laFonction).ToList();
              //Entities.DeleteEntity<PROFIL>(lstAncienneHabile, context);

              Entities.InsertEntity<PROFIL>(pModuleListe, context);

              context.SaveChanges();
              return true;
          }
          catch (Exception ex)
          {
              throw ex;
          }
      }

      public static bool InsertionMenuDuProfil(List<MENUSDUPROFIL> pModuleListe)
      {
          try
          {
              galadbEntities context = new galadbEntities();
              int laFonction = pModuleListe[0].FK_IDPROFIL;
              List<MENUSDUPROFIL> lstAncienneHabile = context.MENUSDUPROFIL.Where(t => t.FK_IDPROFIL == laFonction).ToList();
              Entities.DeleteEntity<MENUSDUPROFIL>(lstAncienneHabile, context);

              Entities.InsertEntity<MENUSDUPROFIL>(pModuleListe, context);

              context.SaveChanges();
              return true;
          }
          catch (Exception ex)
          {
              throw ex;
          }
      }
      public static bool InsertionProfilFonction(List<PROFIL> pModuleListe)
      {
          try
          {
              galadbEntities context = new galadbEntities();
              //int laFonction = pModuleListe[0].FK_IDPROFIL;
              //List<PROFIL> lstAncienneHabile = context.MENUSDUPROFIL.Where(t => t.FK_IDPROFIL == laFonction).ToList();
             // Entities.DeleteEntity<PROFIL>(lstAncienneHabile, context);

              Entities.InsertEntity<PROFIL>(pModuleListe, context);

              context.SaveChanges();
              return true;
          }
          catch (Exception ex)
          {
              throw ex;
          }
      }
        

      #region ADMUTILISATEUR

      public static DataTable DEVIS_ADMUTILISATEUR_SEARCH_AGENTByIdFonctionMatriculeNom(string pIdFonction, string pMatriculeAgent, string pNomAgent)
      {
          string vide = "";
          try
          {
              using (galadbEntities context = new galadbEntities())
              {
                  IEnumerable<object> query =
                  from mat in context.ADMUTILISATEUR
                  from pu in mat.PROFILSUTILISATEUR
                  where (string.IsNullOrEmpty(pIdFonction) || pu.PROFIL.FONCTION.CODE  == pIdFonction)
                  && (string.IsNullOrEmpty(pMatriculeAgent) || mat.MATRICULE.ToUpper().StartsWith(pMatriculeAgent.ToUpper()))
                  && (string.IsNullOrEmpty(pNomAgent) || mat.LIBELLE.ToUpper().Contains(pNomAgent.ToUpper()))
                  select new
                  {
                      mat.PK_ID,
                      mat.LOGINNAME,
                      mat.DATEDERNIEREMODIFICATION,
                      mat.DATEDEBUTVALIDITE,
                      mat.DATEFINVALIDITE,
                      mat.FK_IDSTATUS,
                      mat.PASSE,
                      mat.DATEDERNIEREMODIFICATIONPASSWORD,
                      mat.INITUSERPASSWORD,
                      mat.NOMBREECHECSOUVERTURESESSION,
                      mat.DATEDERNIERECONNEXION,
                      mat.DERNIERECONNEXIONREUSSIE,
                      mat.DATEDERNIERVERROUILLAGE,
                      StatusCompteLibelle = mat.ADMSTATUSCOMPTE.LIBELLE,
                      CENTRE = mat.CENTRE1.CODE,
                      mat.MATRICULE,
                      mat.LIBELLE,
                      pu.PROFIL.FONCTION.ROLEDISPLAYNAME,
                      DisplayName = pu.PROFIL.FONCTION.ROLENAME,
                      LibelleFonction = pu.PROFIL.FONCTION.ROLENAME,
                      mat.E_MAIL,
                      ESTADMIN = (bool?)pu.PROFIL.FONCTION.ESTADMIN,
                      mat.BRANCHE,
                      mat.PERIMETREACTION
                  };
                  return Galatee.Tools.Utility.ListToDataTable(query);
              }
          }
          catch (Exception ex)
          {
              throw ex;
          }
      }

      public static DataTable DEVIS_ADMUTILISATEUR_SEARCH_AGENTByIdFonctionMatriculeNom()
      {
          try
          {
              using (galadbEntities context = new galadbEntities())
              {
                  IEnumerable<object> query =
              from mat in context.ADMUTILISATEUR
              from pu in mat.PROFILSUTILISATEUR
              select new
              {
                  mat.PK_ID,
                  mat.LOGINNAME,
                  mat.DATEDERNIEREMODIFICATION,
                  mat.DATEDEBUTVALIDITE,
                  mat.DATEFINVALIDITE,
                  mat.FK_IDSTATUS,
                  mat.PASSE,
                  mat.DATEDERNIEREMODIFICATIONPASSWORD,
                  mat.INITUSERPASSWORD,
                  mat.NOMBREECHECSOUVERTURESESSION,
                  mat.DATEDERNIERECONNEXION,
                  mat.DERNIERECONNEXIONREUSSIE,
                  mat.DATEDERNIERVERROUILLAGE,
                  StatusCompteLibelle = mat.ADMSTATUSCOMPTE.LIBELLE,
                  mat.CENTRE,
                  mat.MATRICULE,
                  mat.LIBELLE,
                  codeFonction = pu.PROFIL.FONCTION.CODE,
                  DisplayName = pu.PROFIL.FONCTION.ROLENAME,
                  LibelleFonction = pu.PROFIL.FONCTION.ROLENAME,
                  mat.E_MAIL,
                  ESTADMIN = (bool?)pu.PROFIL.FONCTION.ESTADMIN,
                  mat.BRANCHE,
                  mat.PERIMETREACTION
              };
                  return Galatee.Tools.Utility.ListToDataTable(query);
              }
          }
          catch (Exception ex)
          {
              throw ex;
          }
      }

      public static DataTable GetUserByIdGroupeValidationMatriculeNom(Guid pIdGroupeValidation, int IdCentreDemande, string CodeFonction, string Matricule, string NomAgent)
      {
          try
          {
              using (galadbEntities context = new galadbEntities())
              {

                  IEnumerable<object> query =
                  (from mat in context.RHABILITATIONGROUPEVALIDATION
                   //from ct in mat.ADMUTILISATEUR.CENTREDUPROFIL  
                  where mat.FK_IDGROUPE_VALIDATION == pIdGroupeValidation &&  
                  (mat.ADMUTILISATEUR.MATRICULE == Matricule || string.IsNullOrEmpty(Matricule)) &&
                  (mat.ADMUTILISATEUR.LIBELLE.Contains(NomAgent) || string.IsNullOrEmpty(NomAgent)) &&
                  mat.ADMUTILISATEUR.CENTREDUPROFIL.FirstOrDefault(t => t.FK_IDCENTRE == IdCentreDemande) != null
                  select new
                  {
                      mat.ADMUTILISATEUR.PK_ID,
                      mat.ADMUTILISATEUR.LOGINNAME,
                      mat.ADMUTILISATEUR.DATEDERNIEREMODIFICATION,
                      mat.ADMUTILISATEUR.DATEDEBUTVALIDITE,
                      mat.ADMUTILISATEUR.DATEFINVALIDITE,
                      mat.ADMUTILISATEUR.FK_IDSTATUS,
                      mat.ADMUTILISATEUR.PASSE,
                      mat.ADMUTILISATEUR.DATEDERNIEREMODIFICATIONPASSWORD,
                      mat.ADMUTILISATEUR.INITUSERPASSWORD,
                      mat.ADMUTILISATEUR.NOMBREECHECSOUVERTURESESSION,
                      mat.ADMUTILISATEUR.DATEDERNIERECONNEXION,
                      mat.ADMUTILISATEUR.DERNIERECONNEXIONREUSSIE,
                      mat.ADMUTILISATEUR.DATEDERNIERVERROUILLAGE,
                      StatusCompteLibelle = mat.ADMUTILISATEUR.ADMSTATUSCOMPTE.LIBELLE,
                      mat.ADMUTILISATEUR.CENTRE,
                      mat.ADMUTILISATEUR.STATUSCOMPTE ,
                      mat.ADMUTILISATEUR.MATRICULE,
                      mat.ADMUTILISATEUR.LIBELLE,
                      mat.ADMUTILISATEUR.E_MAIL,
                      mat.ADMUTILISATEUR.PERIMETREACTION,
                      mat.ADMUTILISATEUR.FK_IDCENTRE,
                      FK_CENTREAFFECTATION = IdCentreDemande 
                  }).Distinct();
                  return Galatee.Tools.Utility.ListToDataTable(query);
              }
          }
          catch (Exception ex)
          {
              throw ex;
          }
      }

      //public static DataTable GetUserByIdGroupeValidationMatriculeNom(Guid pIdGroupeValidation, int IdCentreDemande, string CodeFonction)
      //{
      //    try
      //    {
      //        using (galadbEntities context = new galadbEntities())
      //        {
      //            var query1 = (from x in context.RHABILITATIONGROUPEVALIDATION
      //                          where x.FK_IDGROUPE_VALIDATION == pIdGroupeValidation
      //                          select new
      //                                     {
      //                                         x.FK_IDADMUTILISATEUR, 
      //                                     });

      //            var query2 =
      //            (from mat in context.ADMUTILISATEUR 
      //             from y in mat.CENTREDUPROFIL 
      //             join x in query1 on mat.PK_ID equals x.FK_IDADMUTILISATEUR
      //             where y.FK_IDCENTRE == IdCentreDemande
      //            select new
      //            {
      //                mat.ADMUTILISATEUR.PK_ID,
      //                mat.ADMUTILISATEUR.LOGINNAME,
      //                mat.ADMUTILISATEUR.DATEDERNIEREMODIFICATION,
      //                mat.ADMUTILISATEUR.DATEDEBUTVALIDITE,
      //                mat.ADMUTILISATEUR.DATEFINVALIDITE,
      //                mat.ADMUTILISATEUR.FK_IDSTATUS,
      //                mat.ADMUTILISATEUR.PASSE,
      //                mat.ADMUTILISATEUR.DATEDERNIEREMODIFICATIONPASSWORD,
      //                mat.ADMUTILISATEUR.INITUSERPASSWORD,
      //                mat.ADMUTILISATEUR.NOMBREECHECSOUVERTURESESSION,
      //                mat.ADMUTILISATEUR.DATEDERNIERECONNEXION,
      //                mat.ADMUTILISATEUR.DERNIERECONNEXIONREUSSIE,
      //                mat.ADMUTILISATEUR.DATEDERNIERVERROUILLAGE,
      //                StatusCompteLibelle = mat.ADMUTILISATEUR.ADMSTATUSCOMPTE.LIBELLE,
      //                mat.ADMUTILISATEUR.CENTRE,
      //                mat.ADMUTILISATEUR.MATRICULE,
      //                mat.ADMUTILISATEUR.LIBELLE,
      //                mat.ADMUTILISATEUR.E_MAIL,
      //                mat.ADMUTILISATEUR.PERIMETREACTION
      //            });

                
      //        }
      //    }
      //    catch (Exception ex)
      //    {
      //        throw ex;
      //    }
      //}
        

      public static DataTable RetourneUtilisateurParMatricule(string pIdUtilisateur)
      {
          try
          {
              using (galadbEntities context = new galadbEntities())
              {
                  var AdmUtilisateur = context.ADMUTILISATEUR.Where(u => u.ESTSUPPRIMER == false);
                  var AdmStatusCompte = context.ADMSTATUSCOMPTE;
                  var Fonction = context.FONCTION;

                  IEnumerable<object> query =
                  from admUtilisateur in AdmUtilisateur
                  from pu in admUtilisateur.PROFILSUTILISATEUR
                  where admUtilisateur.MATRICULE == pIdUtilisateur
                  select new
                  {
                      admUtilisateur.PK_ID,

                      LoginName = admUtilisateur.LOGINNAME,
                      DateDerniereModification = admUtilisateur.DATEDERNIEREMODIFICATION,
                      DateDebutValidite = admUtilisateur.DATEDEBUTVALIDITE,
                      DateFinValidite = admUtilisateur.DATEFINVALIDITE,
                      FK_IdStatusCompte = admUtilisateur.FK_IDSTATUS,
                      PASSE = admUtilisateur.PASSE,
                      DateDerniereModificationPassword = admUtilisateur.DATEDERNIEREMODIFICATIONPASSWORD,
                      InitUserPassword = admUtilisateur.INITUSERPASSWORD,
                      NombreEchecsOuvertureSession = admUtilisateur.NOMBREECHECSOUVERTURESESSION,
                      DateDerniereConnexion = admUtilisateur.DATEDERNIERECONNEXION,
                      DerniereConnexionReussie = admUtilisateur.DERNIERECONNEXIONREUSSIE,
                      DateDernierVerrouillage = admUtilisateur.DATEDERNIERVERROUILLAGE,
                      LIBELLE = admUtilisateur.LIBELLE,
                      StatusCompteLibelle = admUtilisateur.ADMSTATUSCOMPTE.LIBELLE,
                      CENTRE = admUtilisateur.CENTRE1.CODE,
                      Matricule = admUtilisateur.MATRICULE,
                      Fonction = pu.PROFIL.FONCTION.CODE,
                      LibelleFonction = pu.PROFIL.FONCTION.ROLENAME,
                      E_Mail = admUtilisateur.E_MAIL,
                      EstAdmin = pu.PROFIL.FONCTION.ESTADMIN,
                      Branche = admUtilisateur.BRANCHE,
                      PerimetreAction = admUtilisateur.PERIMETREACTION,
                      RoleDisplayName = pu.PROFIL.FONCTION.ROLENAME,
                      EstSupprimer = admUtilisateur.ESTSUPPRIMER,
                      USERCREATION = admUtilisateur.USERCREATION,
                  };
                  return Galatee.Tools.Utility.ListToDataTable(query);
              }
          }
          catch (Exception ex)
          {
              throw ex;
          }
      }

      #endregion

      public static DataTable RetourneListePoste()
      {
          try
          {
              using (galadbEntities context = new galadbEntities())
              {
                  
                  var poste = context.POSTECLIENT   ;

                  IEnumerable<object> query =

                      from x in poste
                      select new
                      {
                          x.PK_ID,
                          x.CODECENTRE,
                          x.NOMPOSTE,
                          x.FK_IDCENTRE,
                          x.IPPOSTE,
                          x.CHEMINEDITION,
                          x.FK_IDCAISSE,
                          x.NUMCAISSE
                      };
                  return Galatee.Tools.Utility.ListToDataTable(query);
              }
          }
          catch (Exception ex)
          {
              throw ex;
          }
      }

      public static bool InsererPoste(Galatee.Structure.CsPoste   lePoste)
      {
          try
          {
              int res = -1;
              using (galadbEntities ctx = new galadbEntities())
              {
                  POSTECLIENT lePo = ctx.POSTECLIENT.FirstOrDefault(t => t.NOMPOSTE.ToUpper()  == lePoste.NOMPOSTE .ToUpper());
                  if (lePo != null && !string.IsNullOrEmpty(lePo.NOMPOSTE))
                  {
                      if (lePo.FK_IDCAISSE != lePoste.FK_IDCAISSE)
                      {
                          if (lePo.FK_IDCAISSE == null)
                          {
                              CAISSE laCaisse = ctx.CAISSE.FirstOrDefault(t => t.PK_ID == lePoste.FK_IDCAISSE);
                              if (laCaisse != null && !string.IsNullOrEmpty(laCaisse.CENTRE))
                                  laCaisse.ESTATTRIBUEE = true;

                          }
                          if (lePoste.FK_IDCAISSE == null)
                          {
                              CAISSE laCaisse = ctx.CAISSE.FirstOrDefault(t => t.PK_ID == lePo.FK_IDCAISSE);
                              if (laCaisse != null && !string.IsNullOrEmpty(laCaisse.CENTRE))
                                  laCaisse.ESTATTRIBUEE = false;
                          }
                          if (lePoste.FK_IDCAISSE != null && lePo.FK_IDCAISSE != null)
                          {
                              CAISSE laCaisse = ctx.CAISSE.FirstOrDefault(t => t.PK_ID == lePo.FK_IDCAISSE);
                              if (laCaisse != null && !string.IsNullOrEmpty(laCaisse.CENTRE))
                                  laCaisse.ESTATTRIBUEE = false;

                              CAISSE laNouvCaisse = ctx.CAISSE.FirstOrDefault(t => t.PK_ID == lePoste.FK_IDCAISSE);
                              if (laCaisse != null && !string.IsNullOrEmpty(laCaisse.CENTRE))
                                  laCaisse.ESTATTRIBUEE = true;
                          }

                      }
                      lePo.CODECENTRE = lePoste.CODECENTRE;
                      lePo.FK_IDCENTRE = lePoste.FK_IDCENTRE.Value;
                      lePo.NOMPOSTE = lePoste.NOMPOSTE;
                      lePo.NUMCAISSE = lePoste.NUMCAISSE;
                      lePo.FK_IDCAISSE = lePoste.FK_IDCAISSE;
                      res = ctx.SaveChanges();
                      return res == -1 ? false : true;

                  }
                  else
                  {
                      POSTECLIENT lePosteInsert = new POSTECLIENT()
                      {
                          CODECENTRE = lePoste.CODECENTRE,
                          FK_IDCENTRE = lePoste.FK_IDCENTRE.Value,
                          NOMPOSTE = lePoste.NOMPOSTE,
                          NUMCAISSE = lePoste.NUMCAISSE,
                          FK_IDCAISSE = lePoste.FK_IDCAISSE
                      };
                      return Entities.InsertEntity<POSTECLIENT>(lePosteInsert);
                  }
              }




              //POSTECLIENT lePosteInsert = new POSTECLIENT()
              //{
              //    CODECENTRE = lePoste.CODECENTRE ,
              //    FK_IDCENTRE = lePoste.FK_IDCENTRE.Value  ,
              //    NOMPOSTE = lePoste.NOMPOSTE ,
              //    NUMCAISSE = lePoste.NUMCAISSE ,
              //    FK_IDCAISSE = lePoste.FK_IDCAISSE 
              //};
              //galadbEntities ctx = new galadbEntities();
              //POSTECLIENT leClient = ctx.POSTECLIENT.FirstOrDefault(t=>t.NOMPOSTE == lePosteInsert.NOMPOSTE) ;
              //if (leClient == null ) 
              //    return Entities.InsertEntity<POSTECLIENT>(lePosteInsert);
              //else 
              //{
              //    lePosteInsert.PK_ID = leClient.PK_ID ;
              //    return Entities.UpdateEntity<POSTECLIENT>(lePosteInsert);
              // }
          }
          catch (Exception ex)
          {
              throw ex;
          }
      }
      public static bool UpdatePoste(Galatee.Structure.CsPoste lePoste)
      {
          int res = -1;
          try
          {
              using (galadbEntities ctx =new galadbEntities())
              {
                  POSTECLIENT lePo = ctx.POSTECLIENT.FirstOrDefault(t => t.PK_ID == lePoste.PK_ID);
                  if (lePo != null && !string.IsNullOrEmpty(lePo.NOMPOSTE))
                  {
                      if (lePo.FK_IDCAISSE != lePoste.FK_IDCAISSE)
                      {
                          if (lePo.FK_IDCAISSE == null)
                          {
                              CAISSE laCaisse = ctx.CAISSE.FirstOrDefault(t => t.PK_ID == lePoste.FK_IDCAISSE);
                              if (laCaisse != null && !string.IsNullOrEmpty(laCaisse.CENTRE))
                                  laCaisse.ESTATTRIBUEE = true;

                          }
                          if (lePoste.FK_IDCAISSE == null)
                          {
                              CAISSE laCaisse = ctx.CAISSE.FirstOrDefault(t => t.PK_ID == lePo.FK_IDCAISSE);
                              if (laCaisse != null && !string.IsNullOrEmpty(laCaisse.CENTRE))
                                  laCaisse.ESTATTRIBUEE = false;
                          }
                          if (lePoste.FK_IDCAISSE != null && lePo.FK_IDCAISSE != null)
                          {
                              CAISSE laCaisse = ctx.CAISSE.FirstOrDefault(t => t.PK_ID == lePo.FK_IDCAISSE);
                              if (laCaisse != null && !string.IsNullOrEmpty(laCaisse.CENTRE))
                                  laCaisse.ESTATTRIBUEE = false;

                              CAISSE laNouvCaisse = ctx.CAISSE.FirstOrDefault(t => t.PK_ID == lePoste.FK_IDCAISSE);
                              if (laCaisse != null && !string.IsNullOrEmpty(laCaisse.CENTRE))
                                  laCaisse.ESTATTRIBUEE = true;
                          }

                      }
                      lePo.CODECENTRE = lePoste.CODECENTRE;
                      lePo.FK_IDCENTRE = lePoste.FK_IDCENTRE.Value;
                      lePo.NOMPOSTE = lePoste.NOMPOSTE;
                      lePo.NUMCAISSE = lePoste.NUMCAISSE;
                      lePo.FK_IDCAISSE = lePoste.FK_IDCAISSE;
                      res = ctx.SaveChanges();
                  }
              }
              return res == -1 ? false : true;
          }
          catch (Exception ex)
          {
              return res == -1 ? false : true;
          }
      }
      public static DataTable RetourneHistoriqueConnection(int? idUser)
      {
          try
          {
              using (galadbEntities context = new galadbEntities())
              {
                  var histo = context.ADMHISTORIQUECONNECTION  ;
                  IEnumerable<object> query =
                      from x in histo
                      where x.FK_IDADMUTILISATEUR  == idUser 
                      select new
                      {
                        IDUSER = x.Login    ,
                        DATECREATION = x.DateConnection ,
                        NOMUSER = x.ADMUTILISATEUR.LIBELLE ,
                        USERCREATION = x.POSTE 
                      };
                  return Galatee.Tools.Utility.ListToDataTable(query);
              }   
          }
          catch (Exception ex)
          {
              throw ex;
          }
      }
      public static DataTable  RetourneHistoriquePassword(int? idUser)
      {
          try
          {
              using (galadbEntities context = new galadbEntities())
              {
                  var histo = context.ADMHISTORIQUEPASSWORD ;
                  IEnumerable<object> query =
                      from x in histo
                      where x.FK_IDADMUTILISATEUR  == idUser 
                      select new
                      {
                        x.IDUSER  ,
                        x.USERCREATION ,
                        x.DATECREATION ,
                        NOMUSER = x.ADMUTILISATEUR.LIBELLE ,
                        USERMODIFICATION = (context.ADMUTILISATEUR.FirstOrDefault(t=>t.MATRICULE == x.USERCREATION) != null ) ?context.ADMUTILISATEUR.FirstOrDefault(t=>t.MATRICULE == x.USERCREATION).LIBELLE : string.Empty   
                      };
                  return Galatee.Tools.Utility.ListToDataTable(query);
              }   
          }
          catch (Exception ex)
          {
              throw ex;
          }
      }
      public static DataTable ChargeListeDesAgents()
      {
          try
          {
              using (galadbEntities context = new galadbEntities())
              {
                  var Agent = context.AGENT ;
                  IEnumerable<object> query =
                      from x in Agent
                      select new
                      {
                          x.MATRICULE,
                          x.NOM,
                          x.PRENOM,
                          x.COMPTEWINDOWS,
                          x.PK_ID,
                          IDENTITE = x.NOM + "  "+ x.PRENOM 
                      };
                  return Galatee.Tools.Utility.ListToDataTable(query);
              }
          }
          catch (Exception ex)
          {
              throw ex;
          }
      }
      public static DataTable RetourneMenusParProfil(int pProfil)
      {
          try
          {
              using (galadbEntities context = new galadbEntities())
              {
                  var Module = context.ADMMENU;
                  IEnumerable<object> query =
                      from x in Module
                      from z in x.MENUSDUPROFIL
                      where z.FK_IDPROFIL == pProfil
                      select new
                      {
                          MODULE = x.MODULE  ,
                          PK_ID = x.PK_ID,
                          MENUTEXT = x.MENUTEXT,
                          MAINMENUID = x.MAINMENUID,
                          FormName = x.FORMENAME 
                      };
                  return Galatee.Tools.Utility.ListToDataTable(query);
              }
          }
          catch (Exception ex)
          {
              throw ex;
          }
      }
      public static DataTable RetourneFonctionByCode(string codefonction)
      {

          try
          {
              using (galadbEntities context = new galadbEntities())
              {

                  var Module = context.FONCTION;
                  IEnumerable<object> query;
                  if (!string.IsNullOrEmpty(codefonction))
                  {
                      query =

                         from x in Module.Where(p => p.CODE == codefonction)
                         select new
                         {
                             PK_ID = x.PK_ID,
                             CODE = x.CODE,
                             ROLEDISPLAYNAME = x.ROLEDISPLAYNAME

                         };
                  }
                  else
                  {
                      query =

                          from x in Module
                          select new
                          {
                              PK_ID = x.PK_ID,
                              CODE = x.CODE,
                              ROLEDISPLAYNAME = x.ROLEDISPLAYNAME

                          };
                  }

                  return Galatee.Tools.Utility.ListToDataTable(query);
              }
          }
          catch (Exception ex)
          {
              throw ex;
          }
      }
      public static DataTable RetourneCentreDuProfil(int id_profil)
      {

          try
          {
              using (galadbEntities context = new galadbEntities())
              {

                  var Module = context.CENTREDUPROFIL;

                  IEnumerable<object> query =

                      from x in Module.Where(p => p.FK_IDPROFIL == id_profil)
                      group new { x.FK_IDPROFIL } by new { x.FK_IDCENTRE, x.FK_IDPROFIL, x.PK_ID, x.DATEDEBUTVALIDITE, x.DATEFINVALIDITE } into profilcentre
                      select new
                      {
                          PK_ID = profilcentre.Key.PK_ID,
                          FK_IDPROFIL = profilcentre.Key.FK_IDPROFIL,
                          FK_IDCENTRE = profilcentre.Key.FK_IDCENTRE,
                          DATEDEBUTVALIDITE = profilcentre.Key.DATEDEBUTVALIDITE,
                          DATEFINVALIDITE = profilcentre.Key.DATEFINVALIDITE
                      };
                  return Galatee.Tools.Utility.ListToDataTable(query);
              }
          }
          catch (Exception ex)
          {
              throw ex;
          }
      }

      public static DataTable RetourneCentreDuProfilUser(int id_profil,int IdUSer)
      {

          try
          {
              using (galadbEntities context = new galadbEntities())
              {

                  var Module = context.CENTREDUPROFIL;

                  IEnumerable<object> query =

                      from x in Module.Where(p => p.FK_IDPROFIL == id_profil && p.FK_IDADMUTILISATEUR == IdUSer )
                      group new { x.FK_IDPROFIL } by new { x.FK_IDCENTRE, x.FK_IDPROFIL, x.PK_ID, x.DATEDEBUTVALIDITE, x.DATEFINVALIDITE } into profilcentre
                      select new
                      {
                          PK_ID = profilcentre.Key.PK_ID,
                          FK_IDPROFIL = profilcentre.Key.FK_IDPROFIL,
                          FK_IDCENTRE = profilcentre.Key.FK_IDCENTRE,
                          DATEDEBUTVALIDITE = profilcentre.Key.DATEDEBUTVALIDITE,
                          DATEFINVALIDITE = profilcentre.Key.DATEFINVALIDITE
                      };
                  return Galatee.Tools.Utility.ListToDataTable(query);
              }
          }
          catch (Exception ex)
          {
              throw ex;
          }
      }

      public static DataTable RetourneProfilByFonction(int fonction)
      {
          try
          {
              using (galadbEntities context = new galadbEntities())
              {

                  var Module = context.PROFIL;

                  IEnumerable<object> query =

                      from x in Module.Where(p => p.FK_IDFONCTION == fonction)
                      select new
                      {

                          PK_ID = x.PK_ID,
                          FK_IDFONCTION = x.FK_IDFONCTION,
                          LIBELLE = x.LIBELLE,
                          CODE = x.CODE

                      };
                  return Galatee.Tools.Utility.ListToDataTable(query);
              }
          }
          catch (Exception ex)
          {
              return null;
          }
      }

      public static DataTable RetourneUserByUser(List<int> idUtilisateur)
      {
          try
          {
              using (galadbEntities context = new galadbEntities())
              {

                  var utilisateur = context.ADMUTILISATEUR ;

                  IEnumerable<object> query =
                      from x in utilisateur
                      where idUtilisateur.Contains(x.PK_ID)
                      select new
                      {
                        x.PK_ID ,
                        x.MATRICULE ,
                        x.LIBELLE  
                      };
                  return Galatee.Tools.Utility.ListToDataTable(query);
              }
          }
          catch (Exception ex)
          {
              return null;
          }
      }
      public static DataTable RetourneProfilByUser(List<int> idUtilisateur)
      {
          try
          {
              using (galadbEntities context = new galadbEntities())
              {

                  var profiluser = context.PROFILSUTILISATEUR  ;

                  IEnumerable<object> query =
                      from x in profiluser
                      where idUtilisateur.Contains(x.FK_IDADMUTILISATEUR )
                      select new
                      {
                        x.FK_IDADMUTILISATEUR ,
                        x.FK_IDPROFIL ,
                        x.PK_ID ,
                        x.PROFIL.LIBELLE ,
                        x.DATEDEBUT ,
                        x.DATEFIN 
                        
                      };
                  return Galatee.Tools.Utility.ListToDataTable(query);
              }
          }
          catch (Exception ex)
          {
              return null;
          }
      }
      public static DataTable RetourneCentreByUser(List<int> idUtilisateur)
      {
          try
          {
              using (galadbEntities context = new galadbEntities())
              {

                  var centreprofil = context.CENTREDUPROFIL ;

                  IEnumerable<object> query =
                      from x in centreprofil
                      where idUtilisateur.Contains(x.FK_IDADMUTILISATEUR)
                      select new
                      {
                          x.FK_IDADMUTILISATEUR,
                          x.FK_IDPROFIL,
                          x.PK_ID,
                          x.FK_IDCENTRE ,
                          LIBELLECENTRE=  x.CENTRE1.LIBELLE ,
                          x.DATEDEBUTVALIDITE ,
                          x.DATEFINVALIDITE 

                      };
                  return Galatee.Tools.Utility.ListToDataTable(query);
              }
          }
          catch (Exception ex)
          {
              return null;
          }
      }
      public static DataTable RetourneMenuByUser()
      {
          try
          {
              using (galadbEntities context = new galadbEntities())
              {

                  var centreprofil = context.MENUSDUPROFIL ;
                  IEnumerable<object> query =
                      from x in centreprofil
                      select new
                      {
                          x.FK_IDPROFIL ,
                          x.ADMMENU.MENUTEXT ,
                        FormName=  x.ADMMENU.FORMENAME 
                      };
                  return Galatee.Tools.Utility.ListToDataTable(query);
              }
          }
          catch (Exception ex)
          {
              return null;
          }
      }

    }
}
