using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Galatee.Structure;

namespace Galatee.Entity.Model
{
    public static partial  class ParamProcedure
    {
        //public static IEnumerable<object> NatureByLibcourt(string Libcourt)
        //{
        // try 
        //    {	        
        //     using (galadbEntities context = new galadbEntities()) 
        //        {
        //            //var NATURE =  context.NATURE;

        //            //    IEnumerable<object> query =

        //            //    from _NATURE in NATURE
        //            //    where _NATURE.LIBCOURT.ToUpper() == Libcourt.ToUpper()
        //            //    select new
        //            //    {
        //            //        NATURE = _NATURE.CODE
        //            //    };

        //                return new IEnumerable<object>();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        // }

        #region APPAREILS

        public static DataTable PARAM_APPAREILS_RETOURNEByCodeAppareil(int pIdAppareil)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                   from appareil in context.APPAREILS
                   where
                     appareil.PK_ID == pIdAppareil
                   select new
                   {
                       appareil.PK_ID,
                       appareil.CODEAPPAREIL,
                       appareil.DESIGNATION,
                       appareil.DETAILS,
                       appareil.USERCREATION,

                       appareil.USERMODIFICATION,
                       appareil.DATECREATION,
                       appareil.DATEMODIFICATION
                   };

                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable PARAM_APPAREILS_RETOURNE()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                   from appareil in context.APPAREILS
                   select new
                   {
                       appareil.PK_ID,
                       appareil.CODEAPPAREIL,
                       appareil.DESIGNATION,
                       appareil.DETAILS,
                       appareil.PUISSANCE,
                       appareil.TEMPSUTILISATION,
                       appareil.USERCREATION,
                       appareil.USERMODIFICATION,
                       appareil.DATECREATION,
                       appareil.DATEMODIFICATION
                   };

                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable PARAM_APPAREILS_RETOURNE_BYDEVIS(string NumDevis)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                   from appareil in context.APPAREILSDEVIS
                   where appareil.NUMDEM == NumDevis
                   select new
                   {
                       appareil.NUMDEM,
                       appareil.CODEAPPAREIL,
                       appareil.NBRE,
                       appareil.PUISSANCE,
                       appareil.DATECREATION,
                       appareil.DATEMODIFICATION,
                       appareil.USERCREATION,
                       appareil.USERMODIFICATION,
                       appareil.PK_ID,
                       appareil.FK_IDDEMANDE,
                       appareil.FK_IDCODEAPPAREIL
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

        #region ETAPEDEVIS

        public static DataTable PARAM_ETAPEDEVIS_RETOURNE()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    return new DataTable();

                   // IEnumerable<object> query =
                   //from e in context.ETAPEDEVIS
                   //orderby e.FK_IDTYPEDEVIS, e.PRODUIT, e.NUMETAPE
                   //select new
                   //{
                   //    e.PK_ID,
                   //    e.FK_IDTYPEDEVIS,
                   //    e.PRODUIT,
                   //    e.NUMETAPE,
                   //    e.FK_IDTACHEDEVIS,
                   //    e.IDTACHESUIVANTE,
                   //    e.IDTACHEINTERMEDIAIRE,
                   //    e.IDTACHEREJET,
                   //    e.IDTACHESAUT,
                   //    e.DELAIEXECUTIONETAPE,
                   //    e.MENUID,
                   //    e.DATECREATION,
                   //    e.DATEMODIFICATION,
                   //    e.USERCREATION,
                   //    e.USERMODIFICATION
                   //};

                   // return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable PARAM_ETAPEDEVIS_RETOURNE_POUR_AFFICHAGE()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    return new DataTable();

                   // IEnumerable<object> query =
                   //from E in context.ETAPEDEVIS
                   //join P in context.PRODUIT on  E.FK_IDPRODUIT equals P.PK_ID  
                   //join TAI in context.TACHEDEVIS on  E.IDTACHEINTERMEDIAIRE  equals  TAI.PK_ID  into TAI_join
                   //from TAI in TAI_join.DefaultIfEmpty()
                   //join TAR in context.TACHEDEVIS on  E.IDTACHEREJET  equals TAR.PK_ID  into TAR_join
                   //from TAR in TAR_join.DefaultIfEmpty()
                   //join TAS in context.TACHEDEVIS on new { IdTacheSuivante = E.IDTACHESUIVANTE } equals new { IdTacheSuivante = TAS.PK_ID }
                   //join TASA in context.TACHEDEVIS on new { IdTacheSaut = (int)E.IDTACHESAUT } equals new { IdTacheSaut = TASA.PK_ID } into TASA_join
                   //from TASA in TASA_join.DefaultIfEmpty()
                   //orderby E.FK_IDTYPEDEVIS, E.PRODUIT, E.NUMETAPE
                   //select new
                   //{
                   //    E.PK_ID,
                   //    E.FK_IDTYPEDEVIS,
                   //    E.FK_IDPRODUIT,
                   //    E.PRODUIT,
                   //    E.NUMETAPE,
                   //    E.FK_IDTACHEDEVIS,
                   //    E.IDTACHESUIVANTE,
                   //    E.IDTACHEINTERMEDIAIRE,
                   //    E.IDTACHEREJET,
                   //    E.IDTACHESAUT,
                   //    E.DELAIEXECUTIONETAPE,
                   //    E.MENUID,
                   //    E.DATECREATION,
                   //    E.DATEMODIFICATION,
                   //    E.USERCREATION,
                   //    E.USERMODIFICATION,
                   //    LIBELLETYPEDEMANDE = E.TYPEDEVIS.LIBELLE,
                   //    LIBELLETACHE = E.TACHEDEVIS.LIBELLE,
                   //    LIBELLETACHEINTERMEDIARE = TAI.LIBELLE,
                   //    LIBELLETACHEREJET = TAR.LIBELLE,
                   //    LIBELLETACHESUIVANTE = TAS.LIBELLE,
                   //    LIBELLETACHESAUT = TASA.LIBELLE,
                   //    LIBELLEPRODUIT = P.LIBELLE
                   //};

                   // return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable PARAM_ETAPEDEVIS_RETOURNEById(int pId)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    return new DataTable();

                   // IEnumerable<object> query =
                   //from e in context.ETAPEDEVIS
                   //where
                   // e.PK_ID == pId
                   //select new
                   //{
                   //    e.PK_ID,
                   //    e.FK_IDPRODUIT,
                   //    e.FK_IDTYPEDEVIS,
                   //    e.MENUID,
                   //    e.PRODUIT,
                   //    e.NUMETAPE,
                   //    e.FK_IDTACHEDEVIS,
                   //    e.IDTACHESUIVANTE,
                   //    e.IDTACHEINTERMEDIAIRE,
                   //    e.IDTACHEREJET,
                   //    e.IDTACHESAUT,
                   //    e.DELAIEXECUTIONETAPE,
                   //    e.DATECREATION,
                   //    e.DATEMODIFICATION,
                   //    e.USERCREATION,
                   //    e.USERMODIFICATION
                   //};

                   // return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public static DataTable PARAM_ETAPEDEVIS_RETOURNEById(ObjDEVIS pdevis)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    return new DataTable();
                   // IEnumerable<object> query =
                   //from e in context.ETAPEDEVIS
                   //where
                   // e.FK_IDTACHEDEVIS == pdevis.FK_IDTACHEDEVIS_CURRENT && e.FK_IDPRODUIT==pdevis.FK_IDPRODUIT
                   //select new
                   //{
                   //    e.PK_ID,
                   //    e.FK_IDPRODUIT,
                   //    e.FK_IDTYPEDEVIS,
                   //    e.MENUID,
                   //    e.PRODUIT,
                   //    e.NUMETAPE,
                   //    e.FK_IDTACHEDEVIS,
                   //    e.IDTACHESUIVANTE,
                   //    e.IDTACHEINTERMEDIAIRE,
                   //    e.IDTACHEREJET,
                   //    e.IDTACHESAUT,
                   //    e.DELAIEXECUTIONETAPE,
                   //    e.DATECREATION,
                   //    e.DATEMODIFICATION,
                   //    e.USERCREATION,
                   //    e.USERMODIFICATION
                   //};

                   // return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public static DataTable PARAM_ETAPEDEVIS_RETOURNEByCodeProduit(int pIdProduit)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    return new DataTable();
                   // IEnumerable<object> query =
                   //from e in context.ETAPEDEVIS
                   //where
                   // e.FK_IDPRODUIT == pIdProduit
                   //select new
                   //{
                   //    e.PK_ID,
                   //    e.FK_IDTYPEDEVIS,
                   //    e.FK_IDPRODUIT,
                   //    e.PRODUIT,
                   //    e.NUMETAPE,
                   //    e.FK_IDTACHEDEVIS,
                   //    e.IDTACHESUIVANTE,
                   //    e.IDTACHEINTERMEDIAIRE,
                   //    e.IDTACHEREJET,
                   //    e.IDTACHESAUT,
                   //    e.DELAIEXECUTIONETAPE,
                   //    e.MENUID,
                   //    e.DATECREATION,
                   //    e.DATEMODIFICATION,
                   //    e.USERCREATION,
                   //    e.USERMODIFICATION
                   //};

                   // return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable PARAM_ETAPEDEVIS_RETOURNEByIdTacheDevis(int pIdTacheDevis)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    return new DataTable();

                   // IEnumerable<object> query =
                   //from e in context.ETAPEDEVIS
                   //where
                   // e.FK_IDTACHEDEVIS == pIdTacheDevis
                   //select new
                   //{
                   //    e.PK_ID,
                   //    e.FK_IDTYPEDEVIS,
                   //    e.PRODUIT,
                   //    e.NUMETAPE,
                   //    e.FK_IDTACHEDEVIS,
                   //    e.IDTACHESUIVANTE,
                   //    e.IDTACHEINTERMEDIAIRE,
                   //    e.IDTACHEREJET,
                   //    e.IDTACHESAUT,
                   //    e.DELAIEXECUTIONETAPE,
                   //    e.MENUID,
                   //    e.DATECREATION,
                   //    e.DATEMODIFICATION,
                   //    e.USERCREATION,
                   //    e.USERMODIFICATION
                   //};
                   // return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable PARAM_ETAPEDEVIS_RETOURNEByIdTypeDevis(int pIdTypeDevis)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    return new DataTable();

                  //  IEnumerable<object> query =
                  //from e in context.ETAPEDEVIS
                  //where
                  //  e.FK_IDTYPEDEVIS == pIdTypeDevis
                  //select new
                  //{
                  //    e.PK_ID,
                  //     e.FK_IDTYPEDEVIS,
                  //     e.PRODUIT,
                  //     e.NUMETAPE,
                  //     e.FK_IDTACHEDEVIS,
                  //     e.IDTACHESUIVANTE,
                  //     e.IDTACHEINTERMEDIAIRE,
                  //     e.IDTACHEREJET,
                  //     e.IDTACHESAUT,
                  //     e.DELAIEXECUTIONETAPE,
                  //     e.MENUID,
                  //     e.DATECREATION,
                  //     e.DATEMODIFICATION,
                  //     e.USERCREATION,
                  //     e.USERMODIFICATION,
                  //     LIBELLETACHE = e.TACHEDEVIS.LIBELLE
                  //};

                  //  return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable PARAM_ETAPEDEVIS_RETOURNEByIdTacheRejet(int pIdTacheRejet)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    return new DataTable();

                  //  IEnumerable<object> query =
                  //from e in context.ETAPEDEVIS
                  //where
                  //  e.IDTACHEREJET == pIdTacheRejet
                  //select new
                  //{
                  //    e.PK_ID,
                  //    e.FK_IDTYPEDEVIS,
                  //    e.PRODUIT,
                  //    e.NUMETAPE,
                  //    e.FK_IDTACHEDEVIS,
                  //    e.IDTACHESUIVANTE,
                  //    e.IDTACHEINTERMEDIAIRE,
                  //    e.IDTACHEREJET,
                  //    e.IDTACHESAUT,
                  //    e.DELAIEXECUTIONETAPE,
                  //    e.MENUID,
                  //    e.DATECREATION,
                  //    e.DATEMODIFICATION,
                  //    e.USERCREATION,
                  //    e.USERMODIFICATION,
                  //    LIBELLETACHE = e.TACHEDEVIS.LIBELLE
                  //};
                  //  return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable PARAM_ETAPEDEVIS_RETOURNEByIdTacheSaut(int pIdTacheSaut)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    return new DataTable();
                  //  IEnumerable<object> query =
                  //from e in context.ETAPEDEVIS
                  //where
                  //  e.IDTACHESAUT == pIdTacheSaut
                  //select new
                  //{
                  //    e.PK_ID,
                  //    e.FK_IDTYPEDEVIS,
                  //    e.PRODUIT,
                  //    e.NUMETAPE,
                  //    e.FK_IDTACHEDEVIS,
                  //    e.IDTACHESUIVANTE,
                  //    e.IDTACHEINTERMEDIAIRE,
                  //    e.IDTACHEREJET,
                  //    e.IDTACHESAUT,
                  //    e.DELAIEXECUTIONETAPE,
                  //    e.MENUID,
                  //    e.DATECREATION,
                  //    e.DATEMODIFICATION,
                  //    e.USERCREATION,
                  //    e.USERMODIFICATION,
                  //    LIBELLETACHE = e.TACHEDEVIS.LIBELLE
                  //};

                  //  return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable PARAM_ETAPEDEVIS_RETOURNEByIdTacheIntermediaire(int pIdTacheIntermediaire)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    return new DataTable();
                  //  IEnumerable<object> query =
                  //from e in context.ETAPEDEVIS
                  //where
                  //  e.IDTACHEINTERMEDIAIRE == pIdTacheIntermediaire
                  //select new
                  //{
                  //    e.PK_ID,
                  //    e.FK_IDTYPEDEVIS,
                  //    e.PRODUIT,
                  //    e.NUMETAPE,
                  //    e.FK_IDTACHEDEVIS,
                  //    e.IDTACHESUIVANTE,
                  //    e.IDTACHEINTERMEDIAIRE,
                  //    e.IDTACHEREJET,
                  //    e.IDTACHESAUT,
                  //    e.DELAIEXECUTIONETAPE,
                  //    e.MENUID,
                  //    e.DATECREATION,
                  //    e.DATEMODIFICATION,
                  //    e.USERCREATION,
                  //    e.USERMODIFICATION,
                  //    LIBELLETACHE = e.TACHEDEVIS.LIBELLE
                  //};
                  //  return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable PARAM_ETAPEDEVIS_RETOURNEByIdTypeDevisIdProduitIdTache(int pIdTypeDevis, int pIdProduit, int pIdTacheCourante)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    return new DataTable();

                  //  IEnumerable<object> query =
                  //from e in context.ETAPEDEVIS
                  //where
                  //  e.FK_IDTYPEDEVIS == pIdTypeDevis &&
                  //  e.FK_IDPRODUIT == pIdProduit &&
                  //  e.FK_IDTACHEDEVIS== pIdTacheCourante
                  //select new
                  //{
                  //    e.PK_ID,
                  //    e.FK_IDTYPEDEVIS,
                  //    e.PRODUIT,
                  //    e.NUMETAPE,
                  //    e.FK_IDTACHEDEVIS,
                  //    e.IDTACHESUIVANTE,
                  //    e.IDTACHEINTERMEDIAIRE,
                  //    e.IDTACHEREJET,
                  //    e.IDTACHESAUT,
                  //    e.DELAIEXECUTIONETAPE,
                  //    e.MENUID,
                  //    e.DATECREATION,
                  //    e.DATEMODIFICATION,
                  //    e.USERCREATION,
                  //    e.USERMODIFICATION,
                  //    LIBELLETACHE = e.TACHEDEVIS.LIBELLE
                  //};

                  //  return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable PARAM_ETAPEDEVIS_RETOURNEByIdTypeDevisNumEtape(int pIdTypeDevis, int pNumEtape)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    return new DataTable();

                  //  IEnumerable<object> query =
                  //from e in context.ETAPEDEVIS
                  //where
                  //  e.FK_IDTYPEDEVIS == pIdTypeDevis &&
                  //  e.NUMETAPE == pNumEtape 
                  //select new
                  //{
                  //    e.PK_ID,
                  //    e.FK_IDTYPEDEVIS,
                  //    e.PRODUIT,
                  //    e.NUMETAPE,
                  //    e.FK_IDTACHEDEVIS,
                  //    e.IDTACHESUIVANTE,
                  //    e.IDTACHEINTERMEDIAIRE,
                  //    e.IDTACHEREJET,
                  //    e.IDTACHESAUT,
                  //    e.DELAIEXECUTIONETAPE,
                  //    e.MENUID,
                  //    e.DATECREATION,
                  //    e.DATEMODIFICATION,
                  //    e.USERCREATION,
                  //    e.USERMODIFICATION,
                  //    LIBELLETACHE = e.TACHEDEVIS.LIBELLE
                  //};

                  //  return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable PARAM_ETAPEDEVIS_RETOURNEByMenuId(int pMenuId)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    return new DataTable();

                  //  IEnumerable<object> query =
                  //from e in context.ETAPEDEVIS
                  //where
                  //  e.MENUID == pMenuId
                  //select new
                  //{
                  //    e.PK_ID,
                  //    e.FK_IDTYPEDEVIS,
                  //    e.PRODUIT,
                  //    e.NUMETAPE,
                  //    e.FK_IDTACHEDEVIS,
                  //    e.IDTACHESUIVANTE,
                  //    e.IDTACHEINTERMEDIAIRE,
                  //    e.IDTACHEREJET,
                  //    e.IDTACHESAUT,
                  //    e.DELAIEXECUTIONETAPE,
                  //    e.MENUID,
                  //    e.DATECREATION,
                  //    e.DATEMODIFICATION,
                  //    e.USERCREATION,
                  //    e.USERMODIFICATION,
                  //    LIBELLETACHE = e.TACHEDEVIS.LIBELLE
                  //};

                  //  return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable PARAM_ETAPEDEVIS_RETOURNEByIdTypeDevisCodeProduitIdTacheDevisIdTacheSuivante(int pIdTypeDevis, string pCodeProduit, int pIdTacheDevis, int pIdTacheSuivante)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    return new DataTable();

                  //  IEnumerable<object> query =
                  //from e in context.ETAPEDEVIS
                  //where
                  //    e.FK_IDTYPEDEVIS == pIdTypeDevis &&
                  //    e.PRODUIT == pCodeProduit &&
                  //    e.FK_IDTACHEDEVIS == pIdTacheDevis &&
                  //    e.IDTACHESUIVANTE == pIdTacheSuivante
                  //select new
                  //{
                  //    e.PK_ID,
                  //    e.FK_IDTYPEDEVIS,
                  //    e.PRODUIT,
                  //    e.NUMETAPE,
                  //    e.FK_IDTACHEDEVIS,
                  //    e.IDTACHESUIVANTE,
                  //    e.IDTACHEINTERMEDIAIRE,
                  //    e.IDTACHEREJET,
                  //    e.IDTACHESAUT,
                  //    e.DELAIEXECUTIONETAPE,
                  //    e.MENUID,
                  //    e.DATECREATION,
                  //    e.DATEMODIFICATION,
                  //    e.USERCREATION,
                  //    e.USERMODIFICATION,
                  //    LIBELLETACHE = e.TACHEDEVIS.LIBELLE
                  //};

                  //  return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region CENTRE

        public static DataTable PARAM_CENTRE_RETOURNEByID(int pIdCentre)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                   from CENTREs in context.CENTRE
                   where
                     CENTREs.PK_ID == pIdCentre
                   select new
                   {
                       CENTREs.PK_ID,
                       CENTREs.FK_IDCODESITE,
                       CENTREs.FK_IDTYPECENTRE,
                       CENTREs.CODE ,
                       CENTREs.LIBELLE,
                       CENTREs.TYPECENTRE ,
                       CENTREs.CODESITE ,
                       CENTREs.DATECREATION,
                       CENTREs.DATEMODIFICATION,
                       CENTREs.USERCREATION,
                       CENTREs.USERMODIFICATION,
                       OriginalCodeCentre = CENTREs.CODE 
                   };

                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable PARAM_CENTRE_RETOURNEByIdSiteIdCentre(int pIdSite, int pIdCentre)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                   from CENTREs in context.CENTRE
                   where
                     CENTREs.FK_IDCODESITE == pIdSite &&
                     CENTREs.PK_ID == pIdCentre
                   select new
                   {
                       CENTREs.PK_ID,
                       CENTREs.FK_IDCODESITE,
                       CENTREs.FK_IDTYPECENTRE,
                       CENTREs.CODE,
                       CENTREs.LIBELLE,
                       CENTREs.TYPECENTRE ,
                       CENTREs.CODESITE ,
                       CENTREs.DATECREATION,
                       CENTREs.DATEMODIFICATION,
                       CENTREs.USERCREATION,
                       CENTREs.USERMODIFICATION,
                       OriginalCodeCentre = CENTREs.CODE ,
                       LIBELLENIVEAUTARIF = CENTREs.NIVEAUTARIF.LIBELLE 

                   };

                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable PARAM_CENTRE_RETOURNEByCodeType(int pIdTypeCentre)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                   from CENTREs in context.CENTRE
                   where
                     CENTREs.FK_IDCODESITE == pIdTypeCentre
                   select new
                   {
                       CENTREs.PK_ID,
                       CENTREs.FK_IDCODESITE,
                       CENTREs.FK_IDTYPECENTRE,
                       CENTREs.CODE,
                       CENTREs.LIBELLE,
                       CENTREs.TYPECENTRE ,
                       CENTREs.CODESITE ,
                       CENTREs.DATECREATION,
                       CENTREs.DATEMODIFICATION,
                       CENTREs.USERCREATION,
                       CENTREs.USERMODIFICATION,
                       OriginalCodeCentre = CENTREs.CODE,
                       LIBELLENIVEAUTARIF = CENTREs.NIVEAUTARIF.LIBELLE 

                   };

                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable PARAM_CENTRE_RETOURNEBySiteId(int pIdSite)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                   from CENTREs in context.CENTRE
                   where
                     CENTREs.SITE.PK_ID == pIdSite
                   select new
                   {
                       CENTREs.PK_ID,
                       CENTREs.FK_IDCODESITE,
                       CENTREs.FK_IDTYPECENTRE,
                       CENTREs.FK_IDNIVEAUTARIF,
                       CENTREs.CODE,
                       CENTREs.LIBELLE,
                       CENTREs.TYPECENTRE,
                       CENTREs.CODESITE,
                       CENTREs.DATECREATION,
                       CENTREs.DATEMODIFICATION,
                       CENTREs.USERCREATION,
                       CENTREs.USERMODIFICATION,
                       OriginalCodeCentre = CENTREs.CODE,
                       LIBELLENIVEAUTARIF = CENTREs.NIVEAUTARIF.LIBELLE 
                   };

                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable PARAM_CENTRE_RETOURNE()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                   from x in context.CENTRE
                   where x.DATEFIN == null 
                   select new
                   {
                        x. CODE ,
                        x. LIBELLE ,
                        x. TYPECENTRE ,
                        x. CODESITE ,
                        x. ADRESSE ,
                        x. TELRENSEIGNEMENT ,
                        x. TELDEPANNAGE ,
                        x.  NUMEROAUTOCLIENT ,
                        x.  GESTIONAUTOAVANCECONSO ,
                        x.  GESTIONAUTOFRAIS ,
                        x.  NUMEROFACTUREPARCLIENT ,
                        x. DATECREATION ,
                        x. DATEMODIFICATION ,
                        x. USERCREATION ,
                        x. USERMODIFICATION ,
                        x. PK_ID ,
                        x. FK_IDCODESITE ,
                        x. FK_IDTYPECENTRE ,
                        x. FK_IDNIVEAUTARIF ,
                        NIVEAUTARIF=  x.NIVEAUTARIF.CODE ,
                        //x. NUMERODEMANDE ,
                        x. NUMEROFACTURE ,
                        LIBELLETYPECENTRE = x.TYPECENTRE1 .LIBELLE,
                        LIBELLESITE = x.SITE.LIBELLE,
                        OriginalCodeCentre = x.CODE 
                   };
                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static DataTable PARAM_PRODUIT_CENTRE()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                   from x in context.PRODUITCENTRE  
                   where x.DATEFIN == null 
                   select new
                   {
                       x.PK_ID ,
                       x.FK_IDCENTRE,
                       x.FK_IDPRODUIT,
                       x.PRODUIT.LIBELLE,
                       x.PRODUIT.CODE ,
                       x.DATECREATION,
                       x.DATEMODIFICATION,
                       x.USERCREATION,
                       x.USERMODIFICATION,
                   };

                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static DataTable PARAM_PRODUIT_CENTRE(int idcentre)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                   from x in context.PRODUITCENTRE
                   where x.FK_IDCENTRE == idcentre && x.DATEFIN == null 
                   select new
                   {
                       x.PK_ID,
                       x.FK_IDCENTRE,
                       x.FK_IDPRODUIT,
                       x.PRODUIT.LIBELLE,
                       x.PRODUIT.CODE,
                       x.DATECREATION,
                       x.DATEMODIFICATION,
                       x.USERCREATION,
                       x.USERMODIFICATION,
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

        #region SITE

        public static DataTable PARAM_SITE_RETOURNEByIdSite(int pIdSite)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                   from SITEs in context.SITE
                   where
                     SITEs.PK_ID == pIdSite
                   select new
                   {
                       SITEs.PK_ID,
                        SITEs.CODE,
                       SITEs.LIBELLE,
                       SITEs.SERVEUR,
                       SITEs.USERID,
                       SITEs.PWD,
                       SITEs.CATALOGUE,
                       SITEs.DATECREATION,
                       SITEs.DATEMODIFICATION,
                       SITEs.USERCREATION,
                       SITEs.USERMODIFICATION,
                       OriginalSITE = SITEs.CODE 
                   };

                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable PARAM_SITE_RETOURNE()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                   from SITEs in context.SITE
                   select new
                   {
                       SITEs.PK_ID,
                        SITEs.CODE,
                       SITEs.LIBELLE,
                       SITEs.SERVEUR,
                       SITEs.USERID,
                       SITEs.PWD,
                       SITEs.CATALOGUE,
                       SITEs.DATECREATION,
                       SITEs.DATEMODIFICATION,
                       SITEs.USERCREATION,
                       SITEs.USERMODIFICATION,
                       OriginalSITE = SITEs.CODE
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
        #region TYPETAXE
       public static DataTable PARAM_TYPETAXE_RETOURNE()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                   from x in context.TYPETAXE 
                   select new
                   {
                        x. CODE ,
                        x. LIBELLE ,
                        x. DATECREATION ,
                        x. DATEMODIFICATION ,
                        x. USERCREATION ,
                        x. USERMODIFICATION ,
                        x. PK_ID 
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
        #region TYPEDEVIS

        public static DataTable PARAM_TYPEDEVIS_RETOURNE()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    return new DataTable();

                   // IEnumerable<object> query =
                   //from typedevis in context.TYPEDEVIS
                   //join produit in context.PRODUIT on typedevis.FK_IDPRODUIT equals produit.PK_ID
                   //orderby typedevis.PK_ID
                   //select new
                   //{
                   //    typedevis.PK_ID,
                   //    typedevis.LIBELLE,
                   //    typedevis.FK_IDPRODUIT,
                   //    typedevis.PRODUIT,
                   //    LIBELLEPRODUIT = produit.LIBELLE,
                   //    typedevis.FK_IDTDEM,
                   //    typedevis.TDEM,
                   //    LIBELLETDEM = typedevis.TDEM1.LIBELLE,
                   //    typedevis.DATECREATION,
                   //    typedevis.DATEMODIFICATION,
                   //    typedevis.USERCREATION,
                   //    typedevis.USERMODIFICATION
                   //};

                   // return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable PARAM_TYPEDEVIS_RETOURNEByProduitId(int pProduitId)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    return new DataTable();
                   // IEnumerable<object> query =
                   //from typedevis in context.TYPEDEVIS
                   //join produit in context.PRODUIT on typedevis.FK_IDPRODUIT equals produit.PK_ID
                   //orderby typedevis.PK_ID
                   //where
                   //  typedevis.FK_IDPRODUIT == pProduitId
                   //select new
                   //{
                   //    typedevis.PK_ID,
                   //    typedevis.LIBELLE,
                   //    typedevis.FK_IDPRODUIT,
                   //    typedevis.PRODUIT,
                   //    LIBELLEPRODUIT = produit.LIBELLE,
                   //    typedevis.FK_IDTDEM,
                   //    typedevis.TDEM,
                   //    LIBELLETDEM = typedevis.TDEM1.LIBELLE,
                   //    typedevis.DATECREATION,
                   //    typedevis.DATEMODIFICATION,
                   //    typedevis.USERCREATION,
                   //    typedevis.USERMODIFICATION
                   //};

                   // return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable PARAM_TYPEDEVIS_RETOURNEById(int pId)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    return new DataTable();

                   // IEnumerable<object> query =
                   //from typedevis in context.TYPEDEVIS
                   //join produit in context.PRODUIT on typedevis.FK_IDPRODUIT equals produit.PK_ID
                   //orderby typedevis.PK_ID
                   //where
                   //  typedevis.PK_ID == pId
                   //select new
                   //{
                   //    typedevis.PK_ID,
                   //    typedevis.LIBELLE,
                   //    typedevis.FK_IDPRODUIT,
                   //    typedevis.PRODUIT,
                   //    LIBELLEPRODUIT = produit.LIBELLE,
                   //    typedevis.FK_IDTDEM,
                   //    typedevis.TDEM,
                   //    LIBELLETDEM = typedevis.TDEM1.LIBELLE,
                   //    typedevis.DATECREATION,
                   //    typedevis.DATEMODIFICATION,
                   //    typedevis.USERCREATION,
                   //    typedevis.USERMODIFICATION
                   //};

                   // return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region PIECEIDENTITE

        public static DataTable PARAM_PIECEIDENTITE_RETOURNE()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                   from PIECEIDENTITEs in context.PIECEIDENTITE
                   select new
                   {
                       PIECEIDENTITEs.PK_ID,
                       PIECEIDENTITEs.LIBELLE,
                       PIECEIDENTITEs.DATECREATION,
                       PIECEIDENTITEs.DATEMODIFICATION,
                       PIECEIDENTITEs.USERCREATION,
                       PIECEIDENTITEs.USERMODIFICATION
                   };

                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable PARAM_PIECEIDENTITE_RETOURNEById(int pId)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                   from PIECEIDENTITEs in context.PIECEIDENTITE
                   where
                     PIECEIDENTITEs.PK_ID == pId
                   select new
                   {
                       PIECEIDENTITEs.PK_ID,
                       PIECEIDENTITEs.LIBELLE,
                       PIECEIDENTITEs.DATECREATION,
                       PIECEIDENTITEs.DATEMODIFICATION,
                       PIECEIDENTITEs.USERCREATION,
                       PIECEIDENTITEs.USERMODIFICATION
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

        #region PRODUIT

        public static DataTable PARAM_PRODUIT_RETOURNE()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                  from PRODUITs in context.PRODUIT
                  select new
                  {
                      PRODUITs.PK_ID,
                      PRODUITs.CODE ,
                      PRODUITs.LIBELLE,
                      PRODUITs.DATECREATION,
                      PRODUITs.DATEMODIFICATION,
                      PRODUITs.USERCREATION,
                      PRODUITs.USERMODIFICATION
                  };

                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable PARAM_PRODUIT_RETOURNEById(int pIdProduit)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                   from PRODUITs in context.PRODUIT
                   where
                     PRODUITs.PK_ID == pIdProduit
                   select new
                   {
                       PRODUITs.PK_ID,
                       PRODUITs.CODE,
                       PRODUITs.LIBELLE,
                       PRODUITs.DATECREATION,
                       PRODUITs.DATEMODIFICATION,
                       PRODUITs.USERCREATION,
                       PRODUITs.USERMODIFICATION
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

        #region CATEGORIECLIENT

        public static DataTable PARAM_CATEGORIECLIENT_RETOURNE()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                  from CATEGORIECLIENTs in context.CATEGORIECLIENT
                  select new
                  {
                      CATEGORIECLIENTs.PK_ID,
                      CATEGORIECLIENTs.CODE,
                      CATEGORIECLIENTs.LIBELLE,
                      CATEGORIECLIENTs.DATECREATION,
                      CATEGORIECLIENTs.DATEMODIFICATION,
                      CATEGORIECLIENTs.USERCREATION,
                      CATEGORIECLIENTs.USERMODIFICATION
                  };

                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable PARAM_CATEGORIECLIENT_RETOURNEById(int pIdCategorieClient)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                   from CATEGORIECLIENTs in context.CATEGORIECLIENT
                   where
                     CATEGORIECLIENTs.PK_ID == pIdCategorieClient
                   select new
                   {
                       CATEGORIECLIENTs.PK_ID,
                       CATEGORIECLIENTs.CODE,
                       CATEGORIECLIENTs.LIBELLE,
                       CATEGORIECLIENTs.DATECREATION,
                       CATEGORIECLIENTs.DATEMODIFICATION,
                       CATEGORIECLIENTs.USERCREATION,
                       CATEGORIECLIENTs.USERMODIFICATION
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

        #region COMMUNE

        public static DataTable PARAM_COMMUNE_RETOURNE()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                  (from COMMUNEs in context.COMMUNE
                  join centre in context.CENTRE on COMMUNEs.CENTRE equals centre.CODE 
                  orderby COMMUNEs.CODE 
                  select   new
                  {
                      COMMUNEs.PK_ID,
                      COMMUNEs.FK_IDCENTRE,
                      COMMUNEs.CODE,
                      COMMUNEs.CENTRE,
                      COMMUNEs.LIBELLE,
                      COMMUNEs.DATECREATION,
                      COMMUNEs.DATEMODIFICATION,
                      COMMUNEs.USERCREATION,
                      COMMUNEs.USERMODIFICATION,
                      LIBELLECENTRE = centre.LIBELLE
                  }).Distinct().ToList();

                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable PARAM_COMMUNE_RETOURNEById(int pId)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                  ( from COMMUNEs in context.COMMUNE
                   join centre in context.CENTRE on COMMUNEs.CENTRE equals centre.CODE
                   where
                     COMMUNEs.PK_ID == pId                      
                   orderby  COMMUNEs.CODE 
                   select   new
                   {
                       COMMUNEs.PK_ID,
                       COMMUNEs.CODE,
                       COMMUNEs.CENTRE,
                       COMMUNEs.LIBELLE,
                       COMMUNEs.DATECREATION,
                       COMMUNEs.DATEMODIFICATION,
                       COMMUNEs.USERCREATION,
                       COMMUNEs.USERMODIFICATION,
                       LIBELLECENTRE = centre.LIBELLE
                   });

                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable PARAM_COMMUNE_RETOURNEByIdCentre(int pIdCentre)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                   from COMMUNEs in context.COMMUNE
                   join centre in context.CENTRE on COMMUNEs.CENTRE equals centre.CODE
                   where
                     COMMUNEs.FK_IDCENTRE == pIdCentre
                   orderby COMMUNEs.CODE
                   select new
                   {
                       COMMUNEs.PK_ID,
                       COMMUNEs.CODE,
                       COMMUNEs.CENTRE,
                       COMMUNEs.LIBELLE,
                       COMMUNEs.DATECREATION,
                       COMMUNEs.DATEMODIFICATION,
                       COMMUNEs.USERCREATION,
                       COMMUNEs.USERMODIFICATION,
                       LIBELLECENTRE = centre.LIBELLE
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

        #region QUARTIER

        public static DataTable PARAM_QUARTIER_RETOURNE()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                  from QUARTIERs in context.QUARTIER
                  orderby QUARTIERs.COMMUNE,QUARTIERs.CODE 
                  select new
                  {
                      QUARTIERs.PK_ID,
                      QUARTIERs.FK_IDCOMMUNE,
                      QUARTIERs.COMMUNE,
                      QUARTIERs.CODE ,
                      QUARTIERs.LIBELLE,
                      QUARTIERs.TRANS,
                      QUARTIERs.DATECREATION,
                      QUARTIERs.DATEMODIFICATION,
                      QUARTIERs.USERCREATION,
                      QUARTIERs.USERMODIFICATION,
                      LIBELLECENTRE = QUARTIERs.COMMUNE1.CENTRE1.LIBELLE,
                      LIBELLECOMMUNE = QUARTIERs.COMMUNE1.LIBELLE,
                      CENTRE = QUARTIERs.COMMUNE1.CENTRE1.CODE
                  };

                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable PARAM_QUARTIER_RETOURNEByCentre(int pId)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                  from QUARTIERs in context.QUARTIER
                  where
                    QUARTIERs.PK_ID == pId
                  orderby QUARTIERs.COMMUNE, QUARTIERs.CODE  
                  select new
                  {
                      QUARTIERs.PK_ID,
                      QUARTIERs.FK_IDCOMMUNE,
                      QUARTIERs.COMMUNE,
                      QUARTIERs.CODE ,
                      QUARTIERs.LIBELLE,
                      QUARTIERs.TRANS,
                      QUARTIERs.DATECREATION,
                      QUARTIERs.DATEMODIFICATION,
                      QUARTIERs.USERCREATION,
                      QUARTIERs.USERMODIFICATION,
                      LIBELLECENTRE = QUARTIERs.COMMUNE1.CENTRE1.LIBELLE,
                      LIBELLECOMMUNE = QUARTIERs.COMMUNE1.LIBELLE,
                      CENTRE = QUARTIERs.COMMUNE1.CENTRE1.CODE
                  };

                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable PARAM_QUARTIER_RETOURNEByCommune(int pIdCommune)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                  from QUARTIERs in context.QUARTIER
                  where
                    QUARTIERs.COMMUNE1.PK_ID == pIdCommune 
                  orderby  QUARTIERs.COMMUNE , QUARTIERs.CODE 
                  select new
                  {
                      QUARTIERs.PK_ID ,
                      QUARTIERs.FK_IDCOMMUNE,
                      QUARTIERs.COMMUNE,
                      QUARTIERs.CODE ,
                      QUARTIERs.LIBELLE,
                      QUARTIERs.TRANS,
                      QUARTIERs.DATECREATION,
                      QUARTIERs.DATEMODIFICATION,
                      QUARTIERs.USERCREATION,
                      QUARTIERs.USERMODIFICATION,
                      LIBELLECENTRE = QUARTIERs.COMMUNE1.CENTRE1.LIBELLE,
                      LIBELLECOMMUNE = QUARTIERs.COMMUNE1.LIBELLE,
                      CENTRE = QUARTIERs.COMMUNE1.CENTRE1.CODE 
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

        #region REGLAGECOMPTEUR

        public static DataTable PARAM_REGLAGECOMPTEUR_RETOURNE()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                  from x in context.REGLAGECOMPTEUR   
                  select new
                  {
                      x.CODE,
                      x.LIBELLE,
                      x.REGLAGEMINI,
                      x.REGLAGEMAXI,
                      x.REGLAGE,
                      x.DATECREATION,
                      x.DATEMODIFICATION,
                      x.USERCREATION,
                      x.USERMODIFICATION,
                      x.PK_ID,
                      x.FK_IDPRODUIT,
                      x.FK_IDPHASE,
                      x.FK_IDCALIBRECOMPTEUR,
                      CODEPRODUIT= x.PRODUIT.CODE ,
                      LIBELLEPRODUIT = x.PRODUIT.LIBELLE 
                  };
                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable PARAM_REGLAGECOMPTEUR_RETOURNEById(int pId)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                  from DIACOMPs in context.REGLAGECOMPTEUR  
                  where
                    DIACOMPs.PK_ID == pId
                  select new
                  {
                      DIACOMPs.PK_ID,
                      DIACOMPs.FK_IDPRODUIT,
                      DIACOMPs.PRODUIT,
                      DIACOMPs.CODE ,
                      DIACOMPs.LIBELLE,
                      DIACOMPs.DATECREATION,
                      DIACOMPs.DATEMODIFICATION,
                      DIACOMPs.USERCREATION,
                      DIACOMPs.USERMODIFICATION
                  };

                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable PARAM_REGLAGECOMPTEUR_RETOURNEByCentre(int pIdCentre)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                  from DIACOMPs in context.REGLAGECOMPTEUR  
                  //where
                  //  DIACOMPs.FK_IDCENTRE == pIdCentre
                  select new
                  {
                      DIACOMPs.PK_ID,
                      DIACOMPs.FK_IDPRODUIT,
                      DIACOMPs.PRODUIT,
                      DIACOMPs.CODE ,
                      DIACOMPs.LIBELLE,
                      DIACOMPs.DATECREATION,
                      DIACOMPs.DATEMODIFICATION,
                      DIACOMPs.USERCREATION,
                      DIACOMPs.USERMODIFICATION
                  };

                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable PARAM_REGLAGECOMPTEUR_RETOURNEByIdProduit(int pIdProduit)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                  from DIACOMPs in context.REGLAGECOMPTEUR  
                  where
                    DIACOMPs.FK_IDPRODUIT == pIdProduit 
                  select new
                  {
                      DIACOMPs.PK_ID,
                      DIACOMPs.FK_IDPRODUIT,
                      DIACOMPs.PRODUIT,
                      DIACOMPs.CODE,
                      DIACOMPs.LIBELLE,
                      DIACOMPs.DATECREATION,
                      DIACOMPs.DATEMODIFICATION,
                      DIACOMPs.USERCREATION,
                      DIACOMPs.USERMODIFICATION
                  };

                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable PARAM_REGLAGECOMPTEUR_RETOURNEByProduitIdCentreId(int pId, int pCentreId, int pProduitId)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                   from DIACOMPs in context.REGLAGECOMPTEUR  
                   where
                     DIACOMPs.PK_ID == pId &&
                     DIACOMPs.FK_IDPRODUIT == pProduitId 
                   select new
                   {
                       DIACOMPs.PK_ID,
                       DIACOMPs.FK_IDPRODUIT,
                       DIACOMPs.PRODUIT,
                       DIACOMPs.CODE,
                       DIACOMPs.LIBELLE,
                       DIACOMPs.DATECREATION,
                       DIACOMPs.DATEMODIFICATION,
                       DIACOMPs.USERCREATION,
                       DIACOMPs.USERMODIFICATION
                   };
                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable PARAM_REGLAGECOMPTEUR_RETOURNEByIdProduitIdCentreId(int pCentreId, int pProduitId)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                   from DIACOMPs in context.REGLAGECOMPTEUR  
                   where
                     DIACOMPs.FK_IDPRODUIT == pProduitId
                   select new
                   {
                       DIACOMPs.PK_ID,
                       DIACOMPs.FK_IDPRODUIT,
                       DIACOMPs.PRODUIT,
                       DIACOMPs.CODE ,
                       DIACOMPs.LIBELLE,
                       DIACOMPs.DATECREATION,
                       DIACOMPs.DATEMODIFICATION,
                       DIACOMPs.USERCREATION,
                       DIACOMPs.USERMODIFICATION
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

        #region CALIBRECOMPTEUR

        public static DataTable PARAM_CALIBRECOMPTEUR_RETOURNE()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                  from x in context.CALIBRECOMPTEUR
                  select new
                  {
                      x.LIBELLE,
                      x.REGLAGEMINI,
                      x.REGLAGEMAXI,
                      x.DATECREATION,
                      x.DATEMODIFICATION,
                      x.USERCREATION,
                      x.USERMODIFICATION,
                      x.PK_ID,
                      x.FK_IDPRODUIT,
                      x.FK_IDPHASE,
                      LIBELLEPRODUIT =   x.PRODUIT.LIBELLE ,
                      CODEPHASE = x.PHASECOMPTEUR.CODE ,
                      PRODUIT = x.PRODUIT.CODE  
                  };
                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable PARAM_CALIBRECOMPTEUR_RETOURNEById(int pId)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                  from x in context.CALIBRECOMPTEUR 
                  where
                    x.PK_ID == pId
                  select new
                  {
                      x.LIBELLE,
                      x.REGLAGEMINI,
                      x.REGLAGEMAXI,
                      x.DATECREATION,
                      x.DATEMODIFICATION,
                      x.USERCREATION,
                      x.USERMODIFICATION,
                      x.PK_ID,
                      x.FK_IDPRODUIT,
                      x.FK_IDPHASE,
                      LIBELLEPRODUIT = x.PRODUIT.LIBELLE 
                  };

                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable PARAM_CALIBRECOMPTEUR_RETOURNEByCentre(int pIdCentre)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                  from x in context.CALIBRECOMPTEUR 
         
                  select new
                  {
                      x.LIBELLE,
                      x.REGLAGEMINI,
                      x.REGLAGEMAXI,
                      x.DATECREATION,
                      x.DATEMODIFICATION,
                      x.USERCREATION,
                      x.USERMODIFICATION,
                      x.PK_ID,
                      x.FK_IDPRODUIT,
                      x.FK_IDPHASE,
                      LIBELLEPRODUIT = x.PRODUIT.LIBELLE 
                  };

                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable PARAM_CALIBRECOMPTEUR_RETOURNEByIdProduit(int pIdProduit)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                  from x in context.CALIBRECOMPTEUR 
                  where
                    x.FK_IDPRODUIT == pIdProduit
                  select new
                  {
                      x.LIBELLE,
                      x.REGLAGEMINI,
                      x.REGLAGEMAXI,
                      x.DATECREATION,
                      x.DATEMODIFICATION,
                      x.USERCREATION,
                      x.USERMODIFICATION,
                      x.PK_ID,
                      x.FK_IDPRODUIT,
                      x.FK_IDPHASE,
                      LIBELLEPRODUIT = x.PRODUIT.LIBELLE 
                  };

                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable PARAM_CALIBRECOMPTEUR_RETOURNEByProduitIdCentreId(int pId, int pCentreId, int pProduitId)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                   from x in context.CALIBRECOMPTEUR 
                   where
                     x.PK_ID == pId &&
                     x.FK_IDPRODUIT == pProduitId
                   select new
                   {
                       x.LIBELLE,
                       x.REGLAGEMINI,
                       x.REGLAGEMAXI,
                       x.DATECREATION,
                       x.DATEMODIFICATION,
                       x.USERCREATION,
                       x.USERMODIFICATION,
                       x.PK_ID,
                       x.FK_IDPRODUIT,
                       x.FK_IDPHASE,
                       LIBELLEPRODUIT = x.PRODUIT.LIBELLE 
                   };
                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable PARAM_CALIBRECOMPTEUR_RETOURNEByIdProduitIdCentreId(int pCentreId, int pProduitId)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                   from x in context.CALIBRECOMPTEUR
                   where
                     x.FK_IDPRODUIT == pProduitId
                   select new
                   {
                       x.LIBELLE,
                       x.REGLAGEMINI,
                       x.REGLAGEMAXI,
                       x.DATECREATION,
                       x.DATEMODIFICATION,
                       x.USERCREATION,
                       x.USERMODIFICATION,
                       x.PK_ID,
                       x.FK_IDPRODUIT,
                       x.FK_IDPHASE,
                       LIBELLEPRODUIT = x.PRODUIT.LIBELLE 
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

        #region MARQUECOMPTEUR

        public static DataTable PARAM_MARQUECOMPTEUR_RETOURNE()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                 from MARQUECOMPTEURs in context.MARQUECOMPTEUR
                 select new
                 {
                     MARQUECOMPTEURs.PK_ID,
                     MARQUECOMPTEURs.CODE,
                     MARQUECOMPTEURs.LIBELLE,
                     MARQUECOMPTEURs.DATECREATION,
                     MARQUECOMPTEURs.DATEMODIFICATION,
                     MARQUECOMPTEURs.USERCREATION,
                     MARQUECOMPTEURs.USERMODIFICATION,
                     MARQUECOMPTEURs.COEFFICIENTDEMULTIPLICATION
                 };
                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable PARAM_MARQUECOMPTEUR_RETOURNEByCode(string pCode)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                 from MARQUECOMPTEURs in context.MARQUECOMPTEUR
                 where
                   MARQUECOMPTEURs.CODE == pCode
                 select new
                 {
                     MARQUECOMPTEURs.PK_ID,
                     MARQUECOMPTEURs.CODE,
                     MARQUECOMPTEURs.LIBELLE,
                     MARQUECOMPTEURs.DATECREATION,
                     MARQUECOMPTEURs.DATEMODIFICATION,
                     MARQUECOMPTEURs.USERCREATION,
                     MARQUECOMPTEURs.USERMODIFICATION
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

        #region TCOMPT

        public static DataTable PARAM_TCOMPT_RETOURNE()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                from TCOMPTs in context.TYPECOMPTEUR  
                select new
                {
                    TCOMPTs.PK_ID,
                    TCOMPTs.FK_IDPRODUIT,
                    TCOMPTs.PRODUIT ,
                    TCOMPTs.CODE ,
                    TCOMPTs.LIBELLE,
                    TCOMPTs.DATECREATION,
                    TCOMPTs.DATEMODIFICATION,
                    TCOMPTs.USERCREATION,
                    TCOMPTs.USERMODIFICATION
                };
                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable PARAM_MTCOMPT_RETOURNEByIdCentreIdProduitId(int pCentreId, int pProduitId, int pTypeId)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                 from TCOMPTs in context.TYPECOMPTEUR  
                 where
                   TCOMPTs.FK_IDPRODUIT == pProduitId &&
                   TCOMPTs.PK_ID == pTypeId
                 select new
                 {
                     TCOMPTs.PK_ID,
                     TCOMPTs.FK_IDPRODUIT,
                     TCOMPTs.PRODUIT ,
                     TCOMPTs.CODE ,
                     TCOMPTs.LIBELLE,
                     TCOMPTs.DATECREATION,
                     TCOMPTs.DATEMODIFICATION,
                     TCOMPTs.USERCREATION,
                     TCOMPTs.USERMODIFICATION
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

        #region RUES

        public static DataTable PARAM_RUES_RETOURNE()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                     from rue in context.RUES
                     select new
                     {
                         rue.PK_ID,
                         rue.FK_IDSECTEUR,
                         rue.CODE,
                         rue.NUMRUE,
                         rue.LIBELLE,
                         rue.TRANS,
                         rue.DATECREATION,
                         rue.DATEMODIFICATION,
                         rue.USERCREATION,
                         rue.USERMODIFICATION,
                         LIBELLECENTRE = rue.SECTEUR.QUARTIER.COMMUNE1.CENTRE1.LIBELLE,
                         LIBELLECOMMUNE = rue.SECTEUR.QUARTIER.COMMUNE1.LIBELLE,
                         LIBELLESECTEUR = rue.SECTEUR.LIBELLE,
                         FK_IDCOMMUNE = rue.SECTEUR.QUARTIER.COMMUNE1.PK_ID,
                         COMMUNE = rue.SECTEUR.QUARTIER.COMMUNE1.CODE ,
                         CENTRE = rue.SECTEUR.QUARTIER.COMMUNE1.CENTRE1.CODE
                     };
                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable PARAM_RUES_RETOURNEByCommune(int pQuartierId)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                     from rue in context.RUES
                     where rue.SECTEUR.QUARTIER.PK_ID == pQuartierId
                     select new
                     {
                         rue.PK_ID,
                         rue.FK_IDSECTEUR,
                         rue.CODE ,
                         rue.NUMRUE,
                         rue.LIBELLE,
                         rue.TRANS,
                         rue.DATECREATION,
                         rue.DATEMODIFICATION,
                         rue.USERCREATION,
                         rue.USERMODIFICATION,
                         LIBELLECENTRE = rue.SECTEUR.QUARTIER.COMMUNE1.CENTRE1.LIBELLE,
                         LIBELLECOMMUNE = rue.SECTEUR.QUARTIER.COMMUNE1.LIBELLE,
                         LIBELLESECTEUR = rue.SECTEUR.LIBELLE,
                         FK_IDCOMMUNE = rue.SECTEUR.QUARTIER.COMMUNE1.PK_ID,
                         COMMUNE = rue.SECTEUR.QUARTIER.COMMUNE1.CODE,
                         CENTRE = rue.SECTEUR.QUARTIER.COMMUNE1.CENTRE1.CODE
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

        #region FONCTION

        public static DataTable PARAM_FONCTION_RETOURNEById(int pId)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                     from fonction in context.FONCTION
                     where fonction.PK_ID == pId
                     select fonction;
                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable PARAM_FONCTION_RETOURNE()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                     from fonction in context.FONCTION
                     orderby fonction.CODE
                     select new
                     {
                         fonction.PK_ID,
                         fonction.DATECREATION,
                         fonction.DATEMODIFICATION,
                         fonction.ESTADMIN,
                         fonction.CODE,
                         fonction.ROLEDISPLAYNAME,
                         fonction.ROLENAME,
                         fonction.USERCREATION,
                         fonction.USERMODIFICATION
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

        #region PARAMETRESGENERAUX

        public static DataTable PARAM_PARAMETRESGENERAUX_RETOURNE()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                  from Ps in context.PARAMETRESGENERAUX
                  select new
                  {
                      Ps.PK_ID,
                      Ps.CODE,
                      Ps.LIBELLE,
                      Ps.DESCRIPTION,
                      Ps.DATECREATION,
                      Ps.DATEMODIFICATION,
                      Ps.USERCREATION,
                      Ps.USERMODIFICATION
                  };
                    DataTable obj=Galatee.Tools.Utility.ListToDataTable(query);
                    return obj;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable PARAM_PARAMETRESGENERAUX_RETOURNEById(int pIdParametre)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                   from Ps in context.PARAMETRESGENERAUX
                   where
                     Ps.PK_ID == pIdParametre
                   select new
                   {
                       Ps.PK_ID,
                       Ps.CODE,
                       Ps.LIBELLE,
                       Ps.DESCRIPTION,
                       Ps.DATECREATION,
                       Ps.DATEMODIFICATION,
                       Ps.USERCREATION,
                       Ps.USERMODIFICATION
                   };

                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable PARAM_PARAMETRESGENERAUX_RETOURNEByCode(string pCodeParametre)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                   from Ps in context.PARAMETRESGENERAUX
                   where
                     Ps.CODE == pCodeParametre
                   select new
                   {
                       Ps.PK_ID,
                       Ps.CODE,
                       Ps.LIBELLE,
                       Ps.DESCRIPTION,
                       Ps.DATECREATION,
                       Ps.DATEMODIFICATION,
                       Ps.USERCREATION,
                       Ps.USERMODIFICATION
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

        #region CTAX

        public static DataTable PARAM_CTAX_RETOURNE()
        {
            try
            {
                IEnumerable<object> query = null;
                using (galadbEntities context = new galadbEntities())
                {
                    var _CTAX = context.TAXE ;
                    query =
                    from _LaTaxe in _CTAX
                    //where _LaTaxe.FINAPPLICATION >= System.DateTime.Today.Date
                    where (_LaTaxe.FINAPPLICATION >= System.DateTime.Today.Date && _LaTaxe.DEBUTAPPLICATION <= System.DateTime.Today.Date)
                    select new
                    {
                        _LaTaxe.CODE,
                        _LaTaxe.LIBELLE,
                        LIBELLETYPETAXE = _LaTaxe.TYPETAXE1.LIBELLE ,
                        _LaTaxe.TAUX,
                        _LaTaxe.DEBUTAPPLICATION,
                        _LaTaxe.FINAPPLICATION,
                        _LaTaxe.TYPETAXE,
                        _LaTaxe.DATECREATION,
                        _LaTaxe.DATEMODIFICATION,
                        _LaTaxe.USERCREATION,
                        _LaTaxe.USERMODIFICATION,
                        _LaTaxe.FK_IDTYPETAXE ,
                        _LaTaxe.PK_ID

                    };
                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable PARAM_CTAX_RETOURNEById(Galatee.Structure.CsCtax pCtax)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                    from ctax in context.TAXE 
                    where ctax.PK_ID == pCtax.PK_ID
                    select ctax;
                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region TYPECOMPTAGE

        public static DataTable PARAM_TYPECOMPTAGE_RETOURNE()
        {
            try
            {
                IEnumerable<object> query = null;
                using (galadbEntities context = new galadbEntities())
                {
                    var _CTAX = context.TYPECOMPTAGE ;
                    query =
                    from x in _CTAX
                    select new
                    {
                        x.CODE,
                        x.LIBELLE,
                        x.USERCREATION,
                        x.DATECREATION,
                        x.DATEMODIFICATION,
                        x.USERMODIFICATION,
                        x.PK_ID

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

        #region BANQUE

        public static DataTable PARAM_BANQUE_RETOURNE()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                    from banq in context.BANQUE
                    select new { 
                    banq.PK_ID,
                    banq.CODE,
                    banq.LIBELLE,
                    banq.DATECREATION,
                    banq.USERCREATION,
                    banq.USERMODIFICATION,
                  
                    };
                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable PARAM_BANQUE_RETOURNEById(Galatee.Structure.aBanque pBanque)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                    from banque in context.BANQUE
                    where banque.PK_ID == pBanque.PK_ID

                    select new
                    {
                        banque.PK_ID,
                        banque.CODE,
                        banque.LIBELLE,
                        banque.DATECREATION,
                        banque.USERCREATION,
                        banque.USERMODIFICATION,

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

        #region ENTREPRISE

        public static DataTable PARAM_ENTREPRISE_RETOURNEById(int pIdEntreprise)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                    from entreprise in context.ENTREPRISE
                    where entreprise.PK_ID == pIdEntreprise
                    select entreprise;
                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region DENOMINATION

        public static DataTable PARAM_DENOMINATION_RETOURNE()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                   from DENOMINATIONs in context.DENOMINATION
                   select DENOMINATIONs;

                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable PARAM_DENOMINATION_RETOURNEById(int pId)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                    from DENOMINATIONs in context.DENOMINATION
                    where
                      DENOMINATIONs.PK_ID == pId
                    select DENOMINATIONs;
                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region PROPRIETAIRE

        public static DataTable PARAM_PROPRIETAIRE_RETOURNE()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                   from PROPRIETAIREs in context.PROPRIETAIRE
                   select new {
                       PROPRIETAIREs.PK_ID,
                       PROPRIETAIREs.CODE ,
                       PROPRIETAIREs.LIBELLE,
                       PROPRIETAIREs.DATECREATION,
                       PROPRIETAIREs.DATEMODIFICATION,
                       PROPRIETAIREs.USERCREATION,
                       PROPRIETAIREs.USERMODIFICATION,
                   };

                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable PARAM_PROPRIETAIRE_RETOURNEByCode(int pId)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                    from PROPRIETAIREs in context.PROPRIETAIRE
                    where
                      PROPRIETAIREs.PK_ID == pId
                    select  new {
                        PROPRIETAIREs.PK_ID,
                       PROPRIETAIREs.CODE ,
                       PROPRIETAIREs.LIBELLE,
                       PROPRIETAIREs.DATECREATION,
                       PROPRIETAIREs.DATEMODIFICATION,
                       PROPRIETAIREs.USERCREATION,
                       PROPRIETAIREs.USERMODIFICATION,
                   };
;
                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region CODECONSOMMATEUR

        public static DataTable PARAM_CODECONSOMMATEUR_RETOURNE()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                   from data in context.CODECONSOMMATEUR
                   select new
                   {
                       data.PK_ID,
                       data.CODE,
                       data.LIBELLE,
                       data.DATECREATION,
                       data.DATEMODIFICATION,
                       data.USERCREATION,
                       data.USERMODIFICATION,

                   };

                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable PARAM_CODECONSOMMATEUR_RETOURNEByCode(int pId)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                    from data in context.CODECONSOMMATEUR
                    where
                      data.PK_ID == pId
                    select  new { 
                   data.PK_ID,
                   data.CODE,
                   data.LIBELLE,
                   data.DATECREATION,
                   data.DATEMODIFICATION,
                   data.USERCREATION,
                   data.USERMODIFICATION,
 
                   };
;
                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region CATEGORIEBRANCHEMENT

        public static DataTable PARAM_TYPEBRANCHEMENT_RETOURNE()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                   from data in context.TYPEBRANCHEMENT 
                   where (data.ESTSUPPRIMER == null || data.ESTSUPPRIMER == false ) 
                   select data;

                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable PARAM_CATEGORIEBRANCHEMENT_RETOURNEByCode(int pId)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                    from data in context.TYPEBRANCHEMENT 
                    where
                      data.PK_ID == pId
                    select data;
                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region PARAMETREEVENEMENT

        public static DataTable PARAM_PARAMETREEVENEMENT_RETOURNE()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                  from Ps in context.PARAMETREEVENEMENT
                  select new
                  {
                      Ps.PK_ID,
                      Ps.CODE,
                      Ps.LIBELLE,
                      Ps.DATECREATION,
                      Ps.DATEMODIFICATION,
                      Ps.USERCREATION,
                      Ps.USERMODIFICATION,
                      OriginalPK_CODE = Ps.CODE

                  };

                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable PARAM_PARAMETREEVENEMENT_RETOURNEByCode(int pId)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                   from Ps in context.PARAMETREEVENEMENT
                   where
                     Ps.PK_ID == pId
                   select new
                   {
                       Ps.PK_ID,
                       Ps.CODE,
                       Ps.LIBELLE,
                       Ps.DATECREATION,
                       Ps.DATEMODIFICATION,
                       Ps.USERCREATION,
                       Ps.USERMODIFICATION,
                       OriginalPK_CODE = Ps.CODE
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

        #region NUMEROINSTALLATION

        public static DataTable PARAM_NUMEROINSTALLATION_RETOURNE()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                  from data in context.NUMEROINSTALLATION
                  select new
                  {
                      data.PK_ID,
                      data.DATECREATION,
                      data.DATEMODIFICATION,
                      data.LIBELLE,
                      data.CODE,
                      data.USERCREATION,
                      data.USERMODIFICATION
                  };
                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable PARAM_NUMEROINSTALLATION_RETOURNEByCode(int pId)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                   from data in context.NUMEROINSTALLATION
                   where data.PK_ID == pId
                   select new
                   {
                       data.PK_ID,
                       data.DATECREATION,
                       data.DATEMODIFICATION,
                       data.LIBELLE,
                       data.CODE,
                       data.USERCREATION,
                       data.USERMODIFICATION
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

        #region CELLULEDUSIEGE

        public static DataTable PARAM_CELLULEDUSIEGE_RETOURNE()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                  from data in context.CELLULEDUSIEGE
                  select new
                  {
                      data.PK_ID,
                      data.DATECREATION,
                      data.DATEMODIFICATION,
                      data.LIBELLE,
                      data.CODE,
                      data.USERCREATION,
                      data.USERMODIFICATION,
                  };
                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable PARAM_CELLULEDUSIEGE_RETOURNEByCode(int pId)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                   from data in context.CELLULEDUSIEGE
                   where data.PK_ID == pId
                   select new
                   {
                       data.PK_ID,
                       data.DATECREATION,
                       data.DATEMODIFICATION,
                       data.LIBELLE,
                       data.CODE,
                       data.USERCREATION,
                       data.USERMODIFICATION,
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

        #region LIBELLETOP

        public static DataTable PARAM_LIBELLETOP_RETOURNE()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                  from data in context.LIBELLETOP
                  select new
                  {
                      data.PK_ID,
                      data.DATECREATION,
                      data.DATEMODIFICATION,
                      data.LIBELLE,
                      data.CODE,
                      data.USERCREATION,
                      data.USERMODIFICATION,
                  };
                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable PARAM_LIBELLETOP_RETOURNEByCode(int pId)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                   from data in context.LIBELLETOP
                   where data.PK_ID == pId
                   select new
                   {
                       data.PK_ID,
                       data.DATECREATION,
                       data.DATEMODIFICATION,
                       data.LIBELLE,
                       data.CODE,
                       data.USERCREATION,
                       data.USERMODIFICATION,
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

        #region REGCLI

        public static DataTable PARAM_REGCLI_RETOURNE()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                  from data in context.REGROUPEMENT
                  select new
                  {
                      data.PK_ID,
                      data.CODE 
                      ,
                      data.NOM
                      ,
                      data.ADRESSE
                      ,
                      data.DATECREATION
                      ,
                      data.DATEMODIFICATION
                      ,
                      data.USERCREATION
                      ,
                      data.USERMODIFICATION
                
                  };
                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable PARAM_REGCLI_RETOURNEByIdRegCli(int pIdRegCli)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                  from data in context.REGROUPEMENT
                  where data.PK_ID == pIdRegCli
                  select new
                  {
                      data.PK_ID,

                      data.CODE 
                      ,
                      data.NOM
                      ,
                      data.ADRESSE
                      ,
                      data.DATECREATION
                      ,
                      data.DATEMODIFICATION
                      ,
                      data.USERCREATION
                      ,
                      data.USERMODIFICATION
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

        #region REGEXO

        public static DataTable PARAM_REGEXO_RETOURNE()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                  from data in context.REGEXO
                  select new
                  {
                      data.PK_ID,
                      data.FK_IDCENTRE,
                      data.FK_IDPRODUIT,
                      data.FK_IDREGROUPEMENT,
                      data.CENTRE,
                      data.PRODUIT,
                      data.REGROUPEMENT,
                      data.EXFAV,
                      data.EXFDOS,
                      data.EXFPOL,
                      data.TRAITFAC,
                      data.TRANS,
                      data.REFERENCEPUPITRE,
                      data.DATECREATION,
                      data.DATEMODIFICATION,
                      data.USERCREATION,
                      data.USERMODIFICATION,
                      LIBELLECENTRE = data.CENTRE1.LIBELLE,
                      LIBELLEPRODUIT = data.PRODUIT1.LIBELLE,
                      LIBELLEREGCLI = data.REGROUPEMENT1.NOM
                  };
                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable PARAM_REGEXO_RETOURNEById(int pRegExoId)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                    from data in context.REGEXO
                    where data.PK_ID == pRegExoId
                    select new
                    {
                        data.PK_ID,
                        data.FK_IDCENTRE,
                        data.FK_IDPRODUIT,
                        data.FK_IDREGROUPEMENT,
                        data.CENTRE,
                        data.PRODUIT,
                        data.REGROUPEMENT,
                        data.EXFAV,
                        data.EXFDOS,
                        data.EXFPOL,
                        data.TRAITFAC,
                        data.TRANS,
                        data.REFERENCEPUPITRE,
                        data.DATECREATION,
                        data.DATEMODIFICATION,
                        data.USERCREATION,
                        data.USERMODIFICATION,
                        LIBELLECENTRE = data.CENTRE1.LIBELLE,
                        LIBELLEPRODUIT = data.PRODUIT1.LIBELLE,
                        LIBELLEREGCLI = data.REGROUPEMENT1.NOM
                    };
                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable PARAM_REGEXO_RETOURNEById(int pRegExoId, int pProduit, int pCentreId, int pRegCli)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                    from data in context.REGEXO
                    where data.PK_ID == pRegExoId
                    && data.PK_ID == pProduit
                    && data.PK_ID == pCentreId
                    && data.PK_ID == pRegCli
                    select new
                    {
                        data.PK_ID,
                        data.FK_IDCENTRE,
                        data.FK_IDPRODUIT,
                        data.FK_IDREGROUPEMENT,
                        data.CENTRE,
                        data.PRODUIT,
                        data.REGROUPEMENT,
                        data.EXFAV,
                        data.EXFDOS,
                        data.EXFPOL,
                        data.TRAITFAC,
                        data.TRANS,
                        data.REFERENCEPUPITRE,
                        data.DATECREATION,
                        data.DATEMODIFICATION,
                        data.USERCREATION,
                        data.USERMODIFICATION,
                        LIBELLECENTRE = data.CENTRE1.LIBELLE,
                        LIBELLEPRODUIT = data.PRODUIT1.LIBELLE,
                        LIBELLEREGCLI = data.REGROUPEMENT1.NOM
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

        #region DOMBANC

        public static DataTable PARAM_DOMBANC_RETOURNE()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                  from data in context.DOMBANC
                  select new 
                  {
                  data.PK_ID,
                  data.FK_IDBANQUE,
                  data.BANQUE,
                  data.GUICHET,
                  data.COMPTE,
                  data.COMPTA,
                  data.LIBELLE,
                  data.DATECREATION,
                  data.DATEMODIFICATION,
                  data.USERCREATION,
                  data.USERMODIFICATION,
             
                  };
                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable PARAM_REGEXO_DOMBANCById(int pId)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                   from data in context.DOMBANC
                   where data.PK_ID == pId
                   select  new 
                  {
                  data.PK_ID,
                  data.FK_IDBANQUE,
                  data.BANQUE,
                  data.GUICHET,
                  data.COMPTE,
                  data.COMPTA,
                  data.LIBELLE,
                  data.DATECREATION,
                  data.DATEMODIFICATION,
                  data.USERCREATION,
                  data.USERMODIFICATION,
             
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

        #region PARAMABONUTILISE

        public static DataTable PARAM_PARAMABONUTILISE_RETOURNE()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                  from data in context.PARAMABONUTILISE
                  select data;
                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable PARAM_PARAMABONUTILISE_ByCentreClecalProduitParam(int pId)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                   from data in context.PARAMABONUTILISE
                   where data.PK_ID == pId
                   select data;

                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region CODEPOSTE

        public static DataTable PARAM_CODEPOSTE_RETOURNE()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                  from data in context.POSTESOURCE 
                  select new
                  {
                      data.PK_ID,
                      data.DATECREATION,
                      data.DATEMODIFICATION,
                      data.LIBELLE,
                      data.CODE,
                      data.USERCREATION,
                      data.USERMODIFICATION
                  };
                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable PARAM_CODEPOSTE_RETOURNEByCode(int pId)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                   from data in context.POSTESOURCE 
                   where data.PK_ID == pId
                   select new
                   {
                       data.PK_ID,
                       data.DATECREATION,
                       data.DATEMODIFICATION,
                       data.LIBELLE,
                       data.CODE,
                       data.USERCREATION,
                       data.USERMODIFICATION
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

        #region CODEDEPART



        #endregion

        #region CASIND

        public static DataTable PARAM_CASIND_RETOURNE()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                  from p in context.CASIND
                  orderby p.CODE 
                  select new
                  {
                      p.CODE,
                      p.LIBELLE,
                      p.CASGEN1,
                      p.CASGEN2,
                      p.CASGEN3,
                      p.CASGEN4,
                      p.CASGEN5,
                      p.CASGEN6,
                      p.CASGEN7,
                      p.CASGEN8,
                      p.CASGEN9,
                      p.CASGEN10,
                      p.SAISIEINDEX,
                      p.SAISIECOMPTEUR,
                      p.SAISIECONSO,
                      p.ENQUETABLE,
                      p.DATECREATION,
                      p.DATEMODIFICATION,
                      p.USERCREATION,
                      p.USERMODIFICATION,
                      p.FK_IDTYPEFACTURATIONAPRESENQUETE ,
                      p.FK_IDTYPEFACTURATIONSANSENQUETE ,
                      SANSENQUETE = p.TYPEFACTURATION.LIBELLE,
                      APRESENQUETE = p.TYPEFACTURATION1.LIBELLE,
                      p.PK_ID,
                  };
                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable PARAM_CASIND_RETOURNEByCas(int pCasindId)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                  from p in context.CASIND
                  where p.PK_ID == pCasindId
                  orderby p.CODE 
                  select new
                  {
                      p.CODE,
                      p.LIBELLE,
                      p.CASGEN1,
                      p.CASGEN2,
                      p.CASGEN3,
                      p.CASGEN4,
                      p.CASGEN5,
                      p.CASGEN6,
                      p.CASGEN7,
                      p.CASGEN8,
                      p.CASGEN9,
                      p.CASGEN10,
                      p.SAISIEINDEX,
                      p.SAISIECOMPTEUR,
                      p.SAISIECONSO,
                      p.DATECREATION,
                      p.ENQUETABLE ,
                      p.DATEMODIFICATION,
                      p.USERCREATION,
                      p.USERMODIFICATION,
                      p.PK_ID,
                      p.FK_IDTYPEFACTURATIONAPRESENQUETE ,
                      p.FK_IDTYPEFACTURATIONSANSENQUETE 
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

        #region TYPECENTRE

        public static DataTable PARAM_TYPECENTRE_RETOURNE()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                   from typeCentre in context.TYPECENTRE
                   select new
                   {
                       typeCentre.PK_ID,
                       typeCentre.CODE,
                       typeCentre.LIBELLE,
                       typeCentre.USERCREATION,
                       typeCentre.USERMODIFICATION,
                       typeCentre.DATECREATION,
                       typeCentre.DATEMODIFICATION
                   };

                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable PARAM_TYPECENTRE_RETOURNEById(int pId)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                   from typeCentre in context.TYPECENTRE
                   where typeCentre.PK_ID == pId
                   select new
                   {
                       typeCentre.PK_ID,
                       typeCentre.CODE,
                       typeCentre.LIBELLE,
                       typeCentre.USERCREATION,
                       typeCentre.USERMODIFICATION,
                       typeCentre.DATECREATION,
                       typeCentre.DATEMODIFICATION
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

        #region TACHEDEVIS

        public static DataTable PARAM_TACHEDEVIS_RETOURNE()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    return new DataTable();
                   // IEnumerable<object> query =
                   //from tacheDevis in context.TACHEDEVIS
                   //select new
                   //{
                   //    tacheDevis.PK_ID,
                   //    tacheDevis.LIBELLE,
                   //    tacheDevis.USERCREATION,
                   //    tacheDevis.USERMODIFICATION,
                   //    tacheDevis.DATECREATION,
                   //    tacheDevis.DATEMODIFICATION
                   //};

                   // return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable PARAM_TACHEDEVIS_RETOURNEById(int pId)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    return new DataTable();

                   // IEnumerable<object> query =
                   //from tacheDevis in context.TACHEDEVIS
                   //where tacheDevis.PK_ID == pId
                   //select new
                   //{
                   //    tacheDevis.PK_ID,
                   //    tacheDevis.LIBELLE,
                   //    tacheDevis.USERCREATION,
                   //    tacheDevis.USERMODIFICATION,
                   //    tacheDevis.DATECREATION,
                   //    tacheDevis.DATEMODIFICATION
                   //};
                   // return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region PERIODICITEFACTURATION

        public static DataTable PARAM_PERIODICITEFACTURATION_RETOURNE()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                  from data in context.PERIODICITE
                  orderby data.PK_ID
                  select new
                  {
                      data.PK_ID,
                      data.CODE ,
                      data.LIBELLE,
                      data.DATECREATION,
                      data.DATEMODIFICATION,
                      data.USERCREATION,
                      data.USERMODIFICATION
                  };

                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable PARAM_PERIODICITEFACTURATION_RETOURNEById(int pId)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                   from datat in context.PERIODICITE
                   orderby datat.CODE 
                   where
                     datat.PK_ID == pId
                   select new
                   {
                       datat.PK_ID,
                       datat.CODE ,
                       datat.LIBELLE,
                       datat.DATECREATION,
                       datat.DATEMODIFICATION,
                       datat.USERCREATION,
                       datat.USERMODIFICATION
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

        #region RECHERCHETARIF

        public static DataTable PARAM_RECHERCHETARIF_RETOURNE()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                  from data in context.RECHERCHETARIF
                  orderby data.CODE
                  select new
                  {
                      data.PK_ID,
                      data.CODE,
                      data.LIBELLE,
                      data.DATECREATION,
                      data.DATEMODIFICATION,
                      data.USERCREATION,
                      data.USERMODIFICATION
                  };

                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable PARAM_RECHERCHETARIF_RETOURNEByCODE(int pId)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                   from datat in context.RECHERCHETARIF
                   orderby datat.CODE 
                   where
                     datat.PK_ID == pId
                   select new
                   {
                       datat.PK_ID,
                       datat.CODE,
                       datat.LIBELLE,
                       datat.DATECREATION,
                       datat.DATEMODIFICATION,
                       datat.USERCREATION,
                       datat.USERMODIFICATION
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

        #region MODECALCUL

        public static DataTable PARAM_MODECALCUL_RETOURNE()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                  from data in context.MODECALCUL
                  orderby data.CODE
                  select new
                  {
                      data.PK_ID,
                      data.CODE ,
                      data.LIBELLE,
                      data.DATECREATION,
                      data.DATEMODIFICATION,
                      data.USERCREATION,
                      data.USERMODIFICATION
                  };

                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable PARAM_MODECALCUL_RETOURNEByCODE(int pId)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                   from datat in context.MODECALCUL
                   orderby datat.CODE
                   where
                     datat.PK_ID == pId
                   select new
                   {
                       datat.PK_ID,
                       datat.CODE ,
                       datat.LIBELLE,
                       datat.DATECREATION,
                       datat.DATEMODIFICATION,
                       datat.USERCREATION,
                       datat.USERMODIFICATION
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

        #region NIVEAUTARIF

        public static DataTable PARAM_NIVEAUTARIF_RETOURNE()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                  from data in context.NIVEAUTARIF
                  orderby data.CODE
                  select new
                  {
                      data.PK_ID,
                      data.CODE ,
                      data.LIBELLE,
                      data.DATECREATION,
                      data.DATEMODIFICATION,
                      data.USERCREATION,
                      data.USERMODIFICATION
                  };

                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable PARAM_NIVEAUTARIF_RETOURNEByCODE(int pId)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                   from datat in context.NIVEAUTARIF
                   orderby datat.CODE 
                   where
                     datat.PK_ID == pId
                   select new
                   {
                       datat.PK_ID,
                       datat.CODE ,
                       datat.LIBELLE,
                       datat.DATECREATION,
                       datat.DATEMODIFICATION,
                       datat.USERCREATION,
                       datat.USERMODIFICATION
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

        #region UNITECOMPTAGE

        public static DataTable PARAM_UNITECOMPTAGE_RETOURNE()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                  from data in context.UNITECOMPTAGE
                  orderby data.CODE 
                  select new
                  {
                      data.PK_ID,
                      data.CODE,
                      data.LIBELLE,
                      data.DATECREATION,
                      data.DATEMODIFICATION,
                      data.USERCREATION,
                      data.USERMODIFICATION
                  };

                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable PARAM_UNITECOMPTAGE_RETOURNEByCODE(int pId)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                   from datat in context.UNITECOMPTAGE
                   orderby datat.CODE
                   where
                     datat.PK_ID == pId
                   select new
                   {
                       datat.PK_ID,
                       datat.CODE,
                       datat.LIBELLE,
                       datat.DATECREATION,
                       datat.DATEMODIFICATION,
                       datat.USERCREATION,
                       datat.USERMODIFICATION
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

        #region MOIS

        public static DataTable PARAM_MOIS_RETOURNE()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                  from data in context.MOIS
                  orderby data.CODE
                  select new
                  {
                      data.PK_ID,
                      data.CODE,
                      data.LIBELLE,
                      data.DATECREATION,
                      data.DATEMODIFICATION,
                      data.USERCREATION,
                      data.USERMODIFICATION
                  };

                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable PARAM_MOIS_RETOURNEByCODE(int pId)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                   from datat in context.MOIS
                   orderby datat.CODE
                   where
                     datat.PK_ID == pId
                   select new
                   {
                       datat.PK_ID,
                       datat.CODE,
                       datat.LIBELLE,
                       datat.DATECREATION,
                       datat.DATEMODIFICATION,
                       datat.USERCREATION,
                       datat.USERMODIFICATION
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

        #region ETATCOMPTEUR

        public static DataTable PARAM_ETATCOMPTEUR_RETOURNE()
        {
            try
            {
                return new DataTable();

                //using (galadbEntities context = new galadbEntities())
                //{
                  //  IEnumerable<object> query =
                  //from data in context.ETATCOMPTEUR
                  //orderby data.PK_ID
                  //select new
                  //{
                  //    data.PK_ID,
                  //    data.CODE,
                  //    data.LIBELLE,
                  //    data.DATECREATION,
                  //    data.DATEMODIFICATION,
                  //    data.USERCREATION,
                  //    data.USERMODIFICATION
                  //};

                  //  return Galatee.Tools.Utility.ListToDataTable(query);
                   
                //}
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable PARAM_ETATCOMPTEUR_RETOURNEById(int pId)
        {
            try
            {
                //using (galadbEntities context = new galadbEntities())
                //{
                //    IEnumerable<object> query =
                //   from datat in context.ETATCOMPTEUR
                //   orderby datat.CODE
                //   where
                //     datat.PK_ID == pId
                //   select new
                //   {
                //       datat.PK_ID,
                //       datat.CODE,
                //       datat.LIBELLE,
                //       datat.DATECREATION,
                //       datat.DATEMODIFICATION,
                //       datat.USERCREATION,
                //       datat.USERMODIFICATION
                //   };

                //    return Galatee.Tools.Utility.ListToDataTable(query);
                   
                //}
                return new DataTable();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region APPLICATIONTAXE

        public static DataTable PARAM_APPLICATIONTAXE_RETOURNE()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                  from data in context.APPLICATIONTAXE
                  orderby data.CODEAPPLICATIONTAXE
                  select new
                  {
                      data.PK_ID,
                      data.CODEAPPLICATIONTAXE,
                      data.LIBELLE,
                      data.DATECREATION,
                      data.DATEMODIFICATION,
                      data.USERCREATION,
                      data.USERMODIFICATION
                  };

                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable PARAM_APPLICATIONTAXE_RETOURNEById(int pId)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                   from datat in context.APPLICATIONTAXE
                   orderby datat.CODEAPPLICATIONTAXE
                   where
                     datat.PK_ID == pId
                   select new
                   {
                       datat.PK_ID,
                       datat.CODEAPPLICATIONTAXE,
                       datat.LIBELLE,
                       datat.DATECREATION,
                       datat.DATEMODIFICATION,
                       datat.USERCREATION,
                       datat.USERMODIFICATION
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

        #region TYPEMESSAGE

        public static DataTable PARAM_TYPEMESSAGE_RETOURNE()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                  from data in context.TYPEMESSAGE
                  orderby data.IDTYPEMESSAGE
                  select new
                  {
                      data.PK_ID,
                      data.IDTYPEMESSAGE,
                      data.LIBELLE,
                      data.DATECREATION,
                      data.DATEMODIFICATION,
                      data.USERCREATION,
                      data.USERMODIFICATION
                  };

                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable PARAM_TYPEMESSAGE_RETOURNEByCODE(int pId)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                   from datat in context.TYPEMESSAGE
                   orderby datat.IDTYPEMESSAGE
                   where
                     datat.PK_ID == pId
                   select new
                   {
                       datat.PK_ID,
                       datat.IDTYPEMESSAGE,
                       datat.LIBELLE,
                       datat.DATECREATION,
                       datat.DATEMODIFICATION,
                       datat.USERCREATION,
                       datat.USERMODIFICATION
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

        #region TARIF

        public static DataTable PARAM_TARIF_RETOURNE()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                  from data in context.TYPETARIF 
                  orderby  data.PRODUIT, data.CODE 
                  select new
                  {
                      data.PK_ID,
                      data.FK_IDPRODUIT,
                      data.CODE ,
                      data.PRODUIT,
                      data.LIBELLE,
                      data.DATECREATION,
                      data.DATEMODIFICATION,
                      data.USERCREATION,
                      data.USERMODIFICATION,
                      LIBELLEPRODUIT = data.PRODUIT1.LIBELLE,
                  };

                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable PARAM_TARIFRETOURNEById(int pTarifId)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                   from data in context.TYPETARIF
                   orderby  data.PRODUIT, data.CODE 
                   where
                     data.PK_ID == pTarifId
                   select new
                   {
                       data.PK_ID,
                       data.FK_IDPRODUIT,
                       data.CODE ,
                       data.PRODUIT,
                       data.LIBELLE,
                       data.DATECREATION,
                       data.DATEMODIFICATION,
                       data.USERCREATION,
                       data.USERMODIFICATION,
                       //LIBELLECENTRE = data.CENTRE1.LIBELLE,
                       LIBELLEPRODUIT = data.PRODUIT1.LIBELLE,
                   };

                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable PARAM_TARIFRETOURNEById(int pTarifId, int pCentreId, int pProduitId)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                   from data in context.TYPETARIF
                   orderby  data.PRODUIT, data.CODE 
                   where
                     data.PK_ID == pTarifId
                     && data.FK_IDPRODUIT == pProduitId
                   select new
                   {
                       data.PK_ID,
                       data.FK_IDPRODUIT,
                       data.CODE ,
                       data.PRODUIT,
                       data.LIBELLE,
                       data.DATECREATION,
                       data.DATEMODIFICATION,
                       data.USERCREATION,
                       data.USERMODIFICATION,
                       LIBELLEPRODUIT = data.PRODUIT1.LIBELLE,
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

        #region FORFAIT

        public static DataTable PARAM_FORFAIT_RETOURNE()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                  from data in context.FORFAIT
                  orderby data.CENTRE, data.PRODUIT, data.CODE 
                  select new
                  {
                      data.PK_ID,
                      data.FK_IDCENTRE,
                      data.FK_IDPRODUIT,
                      data.CODE ,
                      data.PRODUIT,
                      data.CENTRE,
                      data.LIBELLE,
                      data.TRANS,
                      data.DATECREATION,
                      data.DATEMODIFICATION,
                      data.USERCREATION,
                      data.USERMODIFICATION,
                      LIBELLECENTRE = data.CENTRE1.LIBELLE,
                      LIBELLEPRODUIT = data.PRODUIT1.LIBELLE,
                  };

                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable PARAM_FORFAIT_RETOURNEById(int pId)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                  from data in context.FORFAIT
                  where data.PK_ID == pId
                  orderby data.CENTRE, data.PRODUIT, data.CODE
                  select new
                  {
                      data.PK_ID,
                      data.FK_IDCENTRE,
                      data.FK_IDPRODUIT,
                      data.CODE,
                      data.PRODUIT,
                      data.CENTRE,
                      data.LIBELLE,
                      data.TRANS,
                      data.DATECREATION,
                      data.DATEMODIFICATION,
                      data.USERCREATION,
                      data.USERMODIFICATION,
                      LIBELLECENTRE = data.CENTRE1.LIBELLE,
                      LIBELLEPRODUIT = data.PRODUIT1.LIBELLE,
                  };

                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable PARAM_FORFAITRETOURNEByIdProduitIdForfaitId(int pForfaitId, int pCentreId, int pProduitId)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                   from data in context.FORFAIT
                   orderby data.CENTRE, data.PRODUIT, data.CODE
                   where
                     data.PK_ID == pForfaitId && data.FK_IDCENTRE == pCentreId && data.FK_IDPRODUIT == pProduitId
                   select new
                   {
                       data.PK_ID,
                       data.FK_IDCENTRE,
                       data.FK_IDPRODUIT,
                       data.CODE,
                       data.PRODUIT,
                       data.CENTRE,
                       data.LIBELLE,
                       data.TRANS,
                       data.DATECREATION,
                       data.DATEMODIFICATION,
                       data.USERCREATION,
                       data.USERMODIFICATION,
                       LIBELLECENTRE = data.CENTRE1.LIBELLE,
                       LIBELLEPRODUIT = data.PRODUIT1.LIBELLE,
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

        #region REDEVANCE

        public static DataTable PARAM_REDEVANCE_RETOURNE()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                      from data in context.REDEVANCE
                      select new
                      {
                          data.PRODUIT,
                          data.CODE,
                          data.LIBELLE,
                          data.DATECREATION,
                          data.DATEMODIFICATION,
                          data.USERCREATION,
                          data.USERMODIFICATION,
                          data.FK_IDPRODUIT,
                          data.FK_IDTYPELIENREDEVANCE,
                          data.FK_IDTYPEREDEVANCE,
                          data.PK_ID
                      };
                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable PARAM_REDEVANCERETOURNEByPkId(int pId)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    //IEnumerable<object> query =
                    //from data in context.REDEVANCE
                    //join natureClient in context.NATURECLIENT on data.NATURECLI equals natureClient.CODE  into tempTable
                    //from natureClient in tempTable.DefaultIfEmpty()
                    //orderby data.CENTRE, data.PRODUIT
                    //where
                    //  data.PK_ID == pId
                    //select new
                    //{
                    //    data.CENTRE,
                    //    data.PRODUIT,
                    //    data.CODE ,
                    //    data.TRANCHE,
                    //    data.LIBELLE,
                    //    data.TRANS,
                    //    data.NATURECLI,
                    //    data.GRATUIT,
                    //    data.EXONERATION,
                    //    data.TYPELIEN,
                    //    data.PARAM1,
                    //    data.PARAM2,
                    //    data.PARAM3,
                    //    data.PARAM4,
                    //    data.PARAM5,
                    //    data.PARAM6,
                    //    data.EDITEE,
                    //    data.PK_ID,
                    //    data.FK_IDCENTRE,
                    //    data.FK_IDPRODUIT,
                    //    data.DATECREATION,
                    //    data.DATEMODIFICATION,
                    //    data.USERCREATION,
                    //    data.USERMODIFICATION,
                    //    LIBELLECENTRE = data.CENTRE1.LIBELLE,
                    //    LIBELLEPRODUIT = data.PRODUIT1.LIBELLE,
                    //    LIBELLENATURECLIENT = natureClient.LIBELLE
                    //};

                    //return Galatee.Tools.Utility.ListToDataTable(query);
                    return new DataTable();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region AJUFIN

        public static DataTable PARAM_AJUFIN_RETOURNE()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                  from data in context.AJUFIN
                  orderby data.CENTRE, data.CLE, data.DAPP
                  select new
                  {
                      data.CENTRE
                      ,
                      data.CLE
                      ,
                      data.DAPP
                      ,
                      data.POURCENT
                      ,
                      data.DELAI
                      ,
                      data.MINIMUM
                      ,
                      data.MAXIMUM
                      ,
                      data.COPER
                      ,
                      data.TRANS,
                      data.DATECREATION,
                      data.DATEMODIFICATION,
                      data.USERCREATION,
                      data.USERMODIFICATION,
                      LIBELLECENTRE = data.CENTRE1.LIBELLE,
                  };

                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable PARAM_AJUFIN_RETOURNEById(int pId)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                   from data in context.AJUFIN
                   orderby data.CENTRE, data.CLE, data.DAPP
                   where
                     data.PK_ID == pId
                   select new
                   {
                       data.CENTRE
                       ,
                       data.CLE
                       ,
                       data.DAPP
                       ,
                       data.POURCENT
                       ,
                       data.DELAI
                       ,
                       data.MINIMUM
                       ,
                       data.MAXIMUM
                       ,
                       data.COPER
                       ,
                       data.TRANS,
                       data.DATECREATION,
                       data.DATEMODIFICATION,
                       data.USERCREATION,
                       data.USERMODIFICATION,
                       LIBELLECENTRE = data.CENTRE1.LIBELLE,
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

        #region MONNAIE

        public static DataTable PARAM_MONNAIE_RETOURNE()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                  from data in context.MONNAIE
                  orderby data.CENTRE, data.SUPPORT, data.VALEUR
                  select new
                  {
                      data.PK_ID,
                      data.FK_IDCENTRE,
                      data.CENTRE,
                      data.SUPPORT,
                      data.VALEUR,
                      data.LIBELLE,
                      data.DATECREATION,
                      data.DATEMODIFICATION,
                      data.USERCREATION,
                      data.USERMODIFICATION,
                      LIBELLECENTRE = data.CENTRE1.LIBELLE,
                  };

                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable PARAM_MONNAIE_RETOURNEById(int pId)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                   from data in context.MONNAIE
                   orderby data.CENTRE, data.SUPPORT, data.VALEUR
                   where
                     data.PK_ID == pId
                   select new
                   {
                       data.PK_ID,
                       data.CENTRE,
                       data.SUPPORT,
                       data.VALEUR,
                       data.LIBELLE,
                       data.DATECREATION,
                       data.DATEMODIFICATION,
                       data.USERCREATION,
                       data.USERMODIFICATION,
                       LIBELLECENTRE = data.CENTRE1.LIBELLE,
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

        #region NATURECLIENT

        public static DataTable PARAM_NATURECLIENT_RETOURNE()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                  //  IEnumerable<object> query =
                  //from data in context.NATURECLIENT
                  //orderby data.CODE
                  //select new
                  //{
                  //    data.PK_ID,
                  //    data.CODE,
                  //    data.LIBELLE,
                  //    data.DATECREATION,
                  //    data.DATEMODIFICATION,
                  //    data.USERCREATION,
                  //    data.USERMODIFICATION
                  //};

                  //  return Galatee.Tools.Utility.ListToDataTable(query);
                    return new DataTable();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable PARAM_NATURECLIENT_RETOURNEByCODE(int pId)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                 //   IEnumerable<object> query =
                 //from data in context.NATURECLIENT
                 //where data.PK_ID == pId
                 //orderby data.CODE
                 //select new
                 //{
                 //    data.PK_ID,
                 //    data.CODE,
                 //    data.LIBELLE,
                 //    data.DATECREATION,
                 //    data.DATEMODIFICATION,
                 //    data.USERCREATION,
                 //    data.USERMODIFICATION
                 //};

                 //   return Galatee.Tools.Utility.ListToDataTable(query);
                    return new DataTable();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region ACTIONFACTURATION

        public static DataTable PARAM_ACTIONFACTURATION_RETOURNE()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                  from data in context.ACTIONFACTURATION
                  orderby data.CODE 
                  select new
                  {
                      data.PK_ID,
                      data.CODE ,
                      data.LIBELLE,
                      data.DATECREATION,
                      data.DATEMODIFICATION,
                      data.USERCREATION,
                      data.USERMODIFICATION
                  };

                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable PARAM_ACTIONFACTURATION_RETOURNEByCODE(int pId)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                 from data in context.ACTIONFACTURATION
                 where data.PK_ID == pId
                 orderby data.CODE
                 select new
                 {
                     data.PK_ID,
                     data.CODE,
                     data.LIBELLE,
                     data.DATECREATION,
                     data.DATEMODIFICATION,
                     data.USERCREATION,
                     data.USERMODIFICATION
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

        #region DEFPARAMABON

        public static DataTable PARAM_DEFPARAMABON_RETOURNE()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                  from data in context.DEFPARAMABON
                  orderby data.CODE, data.CENTRE, data.PRODUIT, data.PARAM
                  select new
                  {
                      data.PK_ID,
                      data.FK_IDCENTRE,
                      data.FK_IDPRODUIT,
                      data.CODE,
                      data.CENTRE,
                      data.PRODUIT,
                      data.PARAM,
                      data.LIBELLE,
                      data.GROUPE,
                      data.FORMAT,
                      data.SOUSGROUPE,
                      data.MODESAISIE,
                      data.CODERECHERCHE,
                      data.TRAITEMENT,
                      data.DATECREATION,
                      data.DATEMODIFICATION,
                      data.USERCREATION,
                      data.USERMODIFICATION,
                      LIBELLECENTRE = data.CENTRE1.LIBELLE,
                      LIBELLEPRODUIT = data.PRODUIT1.LIBELLE
                  };

                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable PARAM_DEFPARAMABON_RETOURNEByCodeCentreProduitParam(string pCode, string pCentre, string pProduit, string pParam)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                  from data in context.DEFPARAMABON
                  orderby data.CODE, data.CENTRE, data.PRODUIT, data.PARAM
                  where data.CODE == pCode && data.CENTRE == pCentre && data.PRODUIT == pProduit && data.PARAM == pParam
                  select new
                  {
                      data.PK_ID,
                      data.FK_IDCENTRE,
                      data.FK_IDPRODUIT,
                      data.CODE,
                      data.CENTRE,
                      data.PRODUIT,
                      data.PARAM,
                      data.LIBELLE,
                      data.GROUPE,
                      data.FORMAT,
                      data.SOUSGROUPE,
                      data.MODESAISIE,
                      data.CODERECHERCHE,
                      data.TRAITEMENT,
                      data.DATECREATION,
                      data.DATEMODIFICATION,
                      data.USERCREATION,
                      data.USERMODIFICATION,
                      LIBELLECENTRE = data.CENTRE1.LIBELLE,
                      LIBELLEPRODUIT = data.PRODUIT1.LIBELLE
                  };

                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable PARAM_DEFPARAMABON_RETOURNEByCodeCentreProduitParam(int pId, int pIdCentre, int pProduitId)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                  from data in context.DEFPARAMABON
                  orderby data.CODE, data.CENTRE, data.PRODUIT, data.PARAM
                  where data.PK_ID == pId && data.FK_IDCENTRE == pIdCentre && data.FK_IDPRODUIT == pProduitId
                  select new
                  {
                      data.PK_ID,
                      data.FK_IDCENTRE,
                      data.FK_IDPRODUIT,
                      data.CODE,
                      data.CENTRE,
                      data.PRODUIT,
                      data.PARAM,
                      data.LIBELLE,
                      data.GROUPE,
                      data.FORMAT,
                      data.SOUSGROUPE,
                      data.MODESAISIE,
                      data.CODERECHERCHE,
                      data.TRAITEMENT,
                      data.DATECREATION,
                      data.DATEMODIFICATION,
                      data.USERCREATION,
                      data.USERMODIFICATION,
                      LIBELLECENTRE = data.CENTRE1.LIBELLE,
                      LIBELLEPRODUIT = data.PRODUIT1.LIBELLE
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

        #region LIBRUBACTION

        public static DataTable PARAM_LIBRUBACTION_RETOURNE()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                  from data in context.LIBELLEACTIONFACTURATION
                  orderby data.CENTRE, data.CENTRE
                  select new
                  {
                      data.PK_ID,
                      data.FK_IDCENTRE,
                      data.ACTION,
                      data.CENTRE,
                      data.TRANS,
                      data.LNOMBRE1,
                      data.LIBELLE,
                      data.LMONTANT1,
                      data.LNOMBRE2,
                      data.LMONTANT2,
                      data.LNOMBRE3,
                      data.LMONTANT3,
                      data.DATECREATION,
                      data.DATEMODIFICATION,
                      data.USERCREATION,
                      data.USERMODIFICATION,
                      LIBELLEPRODUIT = data.CENTRE1.LIBELLE
                  };

                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable PARAM_LIBRUBACTION_RETOURNEById(int pId)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                  from data in context.LIBELLEACTIONFACTURATION
                  orderby data.ACTION, data.CENTRE
                  where data.PK_ID == pId
                  select new
                  {
                      data.PK_ID,
                      data.FK_IDCENTRE,
                      data.ACTION,
                      data.CENTRE,
                      data.TRANS,
                      data.LNOMBRE1,
                      data.LIBELLE,
                      data.LMONTANT1,
                      data.LNOMBRE2,
                      data.LMONTANT2,
                      data.LNOMBRE3,
                      data.LMONTANT3,
                      data.DATECREATION,
                      data.DATEMODIFICATION,
                      data.USERCREATION,
                      data.USERMODIFICATION,
                      LIBELLEPRODUIT = data.CENTRE1.LIBELLE
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

        #region SECTEUR

        public static DataTable PARAM_SECTEUR_RETOURNE()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                  from data in context.SECTEUR
                  orderby data.CODE, data.CODEQUARTIER
                  select new
                  {
                      data.PK_ID,
                      data.FK_IDQUARTIER,
                      data.CODEQUARTIER,
                      data.CODE,
                      data.TRANS,
                      data.LIBELLE,
                      data.DATECREATION,
                      data.DATEMODIFICATION,
                      data.USERCREATION,
                      data.USERMODIFICATION,
                      LIBELLEQUARTIER = data.QUARTIER.LIBELLE,
                      LIBELLECOMMUNE = data.QUARTIER.COMMUNE1.LIBELLE,
                      LIBELLECENTRE = data.QUARTIER.COMMUNE1.CENTRE1.LIBELLE,
                      CENTRE = data.QUARTIER.COMMUNE1.CENTRE1.CODE
                  };

                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable PARAM_SECTEUR_RETOURNEById(int pId)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                  from data in context.SECTEUR
                  where data.PK_ID == pId
                  orderby data.CODE, data.CODEQUARTIER
                  select new
                  {
                      data.PK_ID,
                      data.FK_IDQUARTIER,
                      data.CODEQUARTIER,
                      data.CODE,
                      data.TRANS,
                      data.LIBELLE,
                      data.DATECREATION,
                      data.DATEMODIFICATION,
                      data.USERCREATION,
                      data.USERMODIFICATION,
                      LIBELLEQUARTIER = data.QUARTIER.LIBELLE,
                      LIBELLECOMMUNE = data.QUARTIER.COMMUNE1.LIBELLE,
                      LIBELLECENTRE = data.QUARTIER.COMMUNE1.CENTRE1.LIBELLE,
                      CENTRE = data.QUARTIER.COMMUNE1.CENTRE1.CODE
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

        #region NATIONALITE

        public static DataTable RetourneNationnalite(string pNationnalite)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = null;
                    if (!string.IsNullOrEmpty(pNationnalite))
                    {
                        var _NATIONALITE = context.NATIONALITE;
                        query =
                        from _LaNation in _NATIONALITE
                        where _LaNation.CODE == pNationnalite
                        select new
                        {
                            _LaNation.CODE,
                            _LaNation.LIBELLE,
                            _LaNation.USERCREATION,
                            _LaNation.USERMODIFICATION,
                            _LaNation.DATECREATION,
                            _LaNation.DATEMODIFICATION,
                            _LaNation.PK_ID
                        };
                    }
                    else
                    {
                        var _NATIONALITE = context.NATIONALITE;
                        query =
                        from _LaNation in _NATIONALITE
                        select new
                        {
                            _LaNation.CODE,
                            _LaNation.LIBELLE,
                            _LaNation.USERCREATION,
                            _LaNation.USERMODIFICATION,
                            _LaNation.DATECREATION,
                            _LaNation.DATEMODIFICATION,
                            _LaNation.PK_ID
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

        #endregion

        #region CAISSE
       public static DataTable PARAM_CAISSE_RETOURNE()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                  from Caisse in context.CAISSE
                  select new
                  {
                      Caisse.PK_ID,
                      Caisse.FK_IDCENTRE,
                      Caisse.ESTATTRIBUEE,
                      Caisse.NUMCAISSE,
                      Caisse.ACQUIT,
                      Caisse.BORDEREAU,
                      Caisse.LIBELLE,
                      Caisse.FONDCAISSE,
                      Caisse.TYPECAISSE,
                      Caisse.CENTRE,
                      Caisse.USERCREATION,
                      Caisse.USERMODIFICATION,
                      Caisse.DATEMODIFICATION
                   


                  };

                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable PARAM_CAISSE_RETOURNEById(int pIdCaisse)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                   from Caisse in context.CAISSE
                   where
                     Caisse.PK_ID == pIdCaisse
                   select new
                   {
                       Caisse.PK_ID,
                       Caisse.FK_IDCENTRE,
                       Caisse.ESTATTRIBUEE,
                       Caisse.NUMCAISSE,
                       Caisse.ACQUIT,
                       Caisse.BORDEREAU,
                       Caisse.LIBELLE,
                       Caisse.FONDCAISSE,
                       Caisse.TYPECAISSE,
                       Caisse.CENTRE,
                       Caisse.USERCREATION,
                       Caisse.USERMODIFICATION,
                       Caisse.DATEMODIFICATION
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

        #region COPER
        public static DataTable PARAM_COPER_RETOURNE()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                  from coper in context.COPER
                  select new
                  {
                      coper.PK_ID,
                      coper.CODE,
                      coper.LIBCOURT,
                      coper.LIBELLE,
                      coper.COMPTGENE,
                      coper.DC,
                      coper.CTRAIT,
                      coper.DMAJ,
                      coper.TRANS,
                      coper.COMPTEANNEXE1,
                      coper.ISOD,
                      coper.USERCREATION,
                      coper.USERMODIFICATION,
                      coper.DATECREATION,
                      coper.DATEMODIFICATION
                     
                  };

                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable PARAM_COPER_RETOURNEById(int pIdCoper)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                   from coper in context.COPER
                   where
                     coper.PK_ID == pIdCoper
                   select new
                   {
                       coper.PK_ID,
                       coper.CODE,
                       coper.LIBCOURT,
                       coper.LIBELLE,
                       coper.COMPTGENE,
                       coper.DC,
                       coper.CTRAIT,
                       coper.DMAJ,
                       coper.TRANS,
                       coper.COMPTEANNEXE1,
                       coper.ISOD,
                       coper.USERCREATION,
                       coper.USERMODIFICATION,
                       coper.DATECREATION,
                       coper.DATEMODIFICATION
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

        #region COPERDEMANDE
        public static DataTable PARAM_COUTDEMANDE_RETOURNE()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                  from x in context.COUTDEMANDE 
                  select new
                  {
                      x.PK_ID,
                      x.CENTRE,
                      x.PRODUIT,
                      x.TYPEDEMANDE,
                      x.COPER,
                      x.CATEGORIE,
                      x.REGLAGECOMPTEUR ,
                      x.PUISSANCE,
                      x.TYPETARIF,
                      x.OBLIGATOIRE,
                      x.AUTOMATIQUE,
                      x.SUBVENTIONNEE,
                      x.MONTANT,
                      x.TAXE,
                      x.DATECREATION,
                      x.DATEMODIFICATION,
                      x.USERCREATION,
                      x.USERMODIFICATION,
                      x.FK_IDPRODUIT,
                      x.FK_IDCOPER,
                      x.FK_IDTYPEDEMANDE,
                      x.FK_IDCENTRE,
                      x.FK_IDTAXE,
                      x.FK_IDTYPETARIF,
                      x.FK_IDREGLAGECOMPTEUR ,
                      x.FK_IDCATEGORIECLIENT,
                      x.FK_IDPUISSANCESOUSCRITE,
                      LIBELLEREGLAGECOMPTEUR = x.REGLAGECOMPTEUR1 != null ? x.REGLAGECOMPTEUR1.LIBELLE : string.Empty,
                      LIBELLECOPER = x.COPER1.LIBELLE,
                      LIBELLETAXE = x.TAXE1.LIBELLE ,
                      LIBELLEPRODUIT = x.PRODUIT1.LIBELLE ,
                      LIBELLECENTRE = x.CENTRE1.LIBELLE ,
                      LIBELLETYPEDEMANDE = x.TYPEDEMANDE1.LIBELLE 
                  };

                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable PARAM_COUTDEMANDE_RETOURNEById(int pIdCoperDemande)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                     from x in context.COUTDEMANDE 
                     where
                       x.PK_ID == pIdCoperDemande
                     select new
                     {
                         x.PK_ID,
                         x.CENTRE,
                         x.PRODUIT,
                         x.TYPEDEMANDE,
                         x.COPER,
                         x.CATEGORIE,
                         x.REGLAGECOMPTEUR ,
                         x.PUISSANCE,
                         x.TYPETARIF,
                         x.OBLIGATOIRE,
                         x.AUTOMATIQUE,
                         x.SUBVENTIONNEE,
                         x.MONTANT,
                         x.TAXE,
                         x.DATECREATION,
                         x.DATEMODIFICATION,
                         x.USERCREATION,
                         x.USERMODIFICATION,
                         x.FK_IDPRODUIT,
                         x.FK_IDCOPER,
                         x.FK_IDTYPEDEMANDE,
                         x.FK_IDCENTRE,
                         x.FK_IDTAXE,
                         x.FK_IDTYPETARIF,
                         x.FK_IDREGLAGECOMPTEUR ,
                         x.FK_IDCATEGORIECLIENT,
                         x.FK_IDPUISSANCESOUSCRITE,
                         LIBELLEREGLAGECOMPTEUR = x.REGLAGECOMPTEUR1 != null ? x.REGLAGECOMPTEUR1.LIBELLE : string.Empty, 
                         LIBELLECOPER = x.COPER1.LIBELLE  ,
                         LIBELLETAXE = x.TAXE1.LIBELLE 
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

        #region FOURNITURE
        public static DataTable PARAM_FOURNITURE_RETOURNE()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                  from x in context.FOURNITURE
                     join  prod in context.PRODUIT on x.FK_IDPRODUIT equals prod.PK_ID
                  select new
                  {
                      //x.CODE,
                      //x.DESIGNATION,
                      x.PRODUIT,
                      //PRIX_UNITAIRE = x.COUTUNITAIRE_FOURNITURE,
                      prod.LIBELLE,
                      x.PK_ID,
                      x.REGLAGECOMPTEUR ,
                      x.QUANTITY,
                      x.ISSUMMARY,
                      x.ISADDITIONAL,
                      x.ISEXTENSION,
                      x.ISDEFAULT,
                      x.FK_IDPRODUIT,
                      x.FK_IDTYPEDEMANDE,
                      x.DATECREATION,
                      x.DATEMODIFICATION,
                      x.USERCREATION,
                      x.USERMODIFICATION
                  };

                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable PARAM_FOURNITURE_RETOURNEById(int pIdFOURNITURE)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                     from x in context.FOURNITURE
                     where
                       x.PK_ID == pIdFOURNITURE
                     select new
                     {
                         //x.CODE,
                         //x.DESIGNATION,
                         //x.PRODUIT,
                         //PRIX_UNITAIRE = x.COUTUNITAIRE_FOURNITURE ,
                         x.PK_ID,
                         x.REGLAGECOMPTEUR,
                         x.QUANTITY,
                         x.ISSUMMARY,
                         x.ISADDITIONAL,
                         x.ISEXTENSION,
                         x.ISDEFAULT,
                         x.FK_IDPRODUIT,
                         x.FK_IDTYPEDEMANDE,
                         x.DATECREATION,
                         x.DATEMODIFICATION,
                         x.USERCREATION,
                         x.USERMODIFICATION
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
        #region MATERIEL

        public static DataTable PARAM_MATERIEL_RETOURNE()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                  from x in context.MATERIELDEVIS 
                  select new
                  {
                      x.CODE,
                      x.LIBELLE,
                      x.COUTUNITAIRE_FOURNITURE,
                      x.COUTUNITAIRE_POSE,
                      x.DATECREATION,
                      x.DATEMODIFICATION,
                      x.USERCREATION,
                      x.USERMODIFICATION,
                      x.PK_ID,
                      FK_IDMATERIELDEVIS = x.PK_ID ,
                      x.FK_IDTYPEMATERIEL ,
                      x.ISDISTANCE,
                      COUTUNITAIRE = x.COUTUNITAIRE_FOURNITURE + x.COUTUNITAIRE_POSE,
                      x.ISGENERE ,
                      x.ISCOMPTEUR 
                  };

                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable PARAM_MATERIEL_RETOURNEById(int pIdMateriel)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                     from x in context.MATERIELDEVIS 
                     where
                       x.PK_ID == pIdMateriel
                     select new
                     {
                         x.CODE,
                         x.LIBELLE,
                         x.COUTUNITAIRE_FOURNITURE,
                         x.COUTUNITAIRE_POSE,
                         x.DATECREATION,
                         x.DATEMODIFICATION,
                         x.USERCREATION,
                         x.USERMODIFICATION,
                         x.PK_ID,
                         x.ISDISTANCE ,
                         COUTUNITAIRE = x.COUTUNITAIRE_FOURNITURE + x.COUTUNITAIRE_POSE 
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

        #region TYPECLIENT

        public static DataTable RetourneTypeClient(string pTypeClien)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = null;
                    if (!string.IsNullOrEmpty(pTypeClien))
                    {
                        var _TYPECLIENT = context.TYPECLIENT;
                        query =
                        from _LaNation in _TYPECLIENT
                        where _LaNation.CODE == pTypeClien
                        select new
                        {
                            _LaNation.CODE,
                            _LaNation.LIBELLE,
                            _LaNation.PK_ID
                        };
                    }
                    else
                    {
                        var _TYPECLIENT = context.TYPECLIENT;
                        query =
                        from _LaNation in _TYPECLIENT
                        select new
                        {
                            _LaNation.CODE,
                            _LaNation.LIBELLE,
                            _LaNation.PK_ID
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

        #endregion

        #region USAGE

        public static DataTable RetourneUsage(string pTypeClien)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = null;
                    if (!string.IsNullOrEmpty(pTypeClien))
                    {
                        var _USAGE = context.USAGE;
                        query =
                        from _LaNation in _USAGE
                        where _LaNation.CODE == pTypeClien
                        select new
                        {
                            _LaNation.CODE,
                            _LaNation.LIBELLE,
                            _LaNation.PK_ID
                        };
                    }
                    else
                    {
                        var _USAGE = context.USAGE;
                        query =
                        from _LaNation in _USAGE
                        select new
                        {
                            _LaNation.CODE,
                            _LaNation.LIBELLE,
                            _LaNation.PK_ID
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

        #endregion

        #region STATUT JURIDIQUE

        public static DataTable RetourneStatutJuridique(string pTypeClien)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = null;
                    if (!string.IsNullOrEmpty(pTypeClien))
                    {
                        var _STATUTJURIQUE = context.STATUTJURIQUE;
                        query =
                        from _LaNation in _STATUTJURIQUE
                        where _LaNation.CODE == pTypeClien
                        select new
                        {
                            _LaNation.CODE,
                            _LaNation.LIBELLE,
                            _LaNation.PK_ID
                        };
                    }
                    else
                    {
                        var _STATUTJURIQUE = context.STATUTJURIQUE;
                        query =
                        from _LaNation in _STATUTJURIQUE
                        select new
                        {
                            _LaNation.CODE,
                            _LaNation.LIBELLE,
                            _LaNation.PK_ID
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

        #endregion

        public static DataTable PARAM_TDEM_RETOURNE()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    var _Tdem = context.TYPEDEMANDE;
                    IEnumerable<object> query = null; 
                    query =
                    from x in _Tdem
                    select new
                    {
                        x.CODE,
                        x.LIBELLE,
                        x.DEMOPTION1,
                        x.DEMOPTION2,
                        x.DEMOPTION3,
                        x.DEMOPTION4,
                        x.DEMOPTION5,
                        x.DEMOPTION6,
                        x.DEMOPTION7,
                        x.DEMOPTION8,
                        x.DEMOPTION9,
                        x.DEMOPTION10,
                        x.DEMOPTION11,
                        x.DEMOPTION12,
                        x.DEMOPTION13,
                        x.DEMOPTION14,
                        x.DEMOPTION15,
                        x.DEMOPTION16,
                        x.DEMOPTION17,
                        x.DEMOPTION18,
                        x.DEMOPTION19,
                        x.DEMOPTION20,
                        x.EVT1,
                        x.EVT2,
                        x.EVT3,
                        x.EVT4,
                        x.EVT5,
                        x.USERCREATION,
                        x.DATECREATION,
                        x.DATEMODIFICATION,
                        x.USERMODIFICATION,
                        x.PK_ID,
                        x.FK_IDACTIVITE_TERRAIN
                    };

                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        #region NOTIFICATION
        public static DataTable RetourneTypenOTIFICATION()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = null;
                        var _TYPENOTIFICATION = context.TYPENOTIFICATION ;
                        query =
                        from _LaTnotif in _TYPENOTIFICATION
                        select new
                        {
                            _LaTnotif.CODE,
                            _LaTnotif.LIBELLE,
                            _LaTnotif.PK_ID
                        };
                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static DataTable RetourneParametreSMTP()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = null;
                    var _Smtp = context.PARAMETRESMTP ;
                    query =
                    from x in _Smtp
                    select new
                    {
                        x.LOGIN ,
                        x.PASSWORD,
                        x.PORT ,
                        x.SERVEURSMTP ,
                        x.SSL ,
                        x.PK_ID
                    };
                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static DataTable RetourneNotification()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = null;
                    var _Notification = context.NOTIFICATION ;
                    query =
                    from x in _Notification
                    select new
                    {
                        x.FK_IDTYPENOTIFICATION ,
                        x.MENU ,
                        x.MESSAGE ,
                    };
                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static DataTable SelectNotificationByTypeMail(string CodeTypeMail)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = null;
                    var _Notification = context.NOTIFICATION  ;
                    query =
                    from x in _Notification
                    where x.TYPENOTIFICATION.CODE == CodeTypeMail 
                    select new
                    {
                        x.FK_IDTYPENOTIFICATION,
                        x.MENU,
                        x.MESSAGE,
                        OBJET=  x.TYPENOTIFICATION.LIBELLE 
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

        #region MARQUEMODELE
        public static DataTable PARAM_MARQUEMODELE_RETOURNE()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                  from x in context.MARQUE_MODELE 
                  select new
                  {
                      x.PK_ID,
                      x.MODELE_ID,
                      Libelle_Modele = x.MODELE.Libelle_Modele ,
                      x.MARQUE_ID,
                      Libelle_MArque = x.MARQUECOMPTEUR.LIBELLE ,
                      x.Nbre_scel_capot,
                      x.Nbre_scel_cache,
                      x.Produit_ID,
                      Libelle_Produit = x.PRODUIT.LIBELLE 
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

        #region MODELE
        public static DataTable PARAM_MODELE_RETOURNE()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                  from x in context.MODELE
                  select new
                  {
                      x.MODELE_ID,
                      x.Libelle_Modele 
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
        #region ACTIVITER
        public static DataTable PARAM_ACTIVITE_RETOURNE()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                  from x in context.RefActivite 
                  select new
                  {
                      x.Activite_ID ,
                      x.Activite_Libelle
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
        #region COULEUR
        public static DataTable PARAM_COULEUR_RETOURNE()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                  IEnumerable<object> query =
                  from x in context.RefCouleurlot 
                  select new
                  {
                      x.Couleur_ID ,
                      x.Couleur_libelle 
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
        #region ACTIVITERCOULEUR
        public static DataTable PARAM_ACTIVITECOULEUR_RETOURNE()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                  from x in context.ActiviteCouleur 
                  select new
                  {
                      x.Activite_ID ,
                      x.Couleur_ID ,
                      x.RefActivite.Activite_Libelle ,
                      x.RefCouleurlot.Couleur_libelle ,
                      x.PK_ID 
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


        #region PARAMETRECOUPURESGC

        public static DataTable RetourneParamatreSCGC()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = null;
                    var _PARAMETRECOUPURESGC = context.PARAMETRECOUPURESGC;
                        query =
                        from _LaPARAMETRECOUPURESGC in _PARAMETRECOUPURESGC
                        select new
                        {
                            _LaPARAMETRECOUPURESGC.NOMCHEFSERVICE,
                            _LaPARAMETRECOUPURESGC.NOM_DONNEURORDRE,
                            _LaPARAMETRECOUPURESGC.TITRE_DONNEURORDRE,
                            _LaPARAMETRECOUPURESGC.CONTACT_DONNEURORDRE,
                            _LaPARAMETRECOUPURESGC.STRUCTURE_EXECUTION,
                            _LaPARAMETRECOUPURESGC.AGENT_EXECUTION,
                            _LaPARAMETRECOUPURESGC.MATRICULE_EXECUTION,
                            _LaPARAMETRECOUPURESGC.PK_ID
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
    }
}
