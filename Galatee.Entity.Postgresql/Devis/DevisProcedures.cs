using Galatee.Structure;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galatee.Entity.Model
{
    public class DevisProcedures
    {
        #region APPAREILSDEVIS

        public static DataTable DEVIS_APPAREILSDEVIS_RETOURNEByDevisId(int pIddevis)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {

                    IEnumerable<object> query =
                   from AD in context.APPAREILSDEVIS
                   where
                     AD.FK_IDDEMANDE == pIddevis
                   select new
                   {
                       AD.PK_ID,
                       AD.FK_IDCODEAPPAREIL,
                       AD.FK_IDDEMANDE,
                       AD.NUMDEM,
                       AD.CODEAPPAREIL,
                       AD.NBRE,
                       AD.PUISSANCE,
                       AD.APPAREILS.DESIGNATION,
                       AD.APPAREILS.TEMPSUTILISATION,
                       AD.USERCREATION,
                       AD.USERMODIFICATION,
                       AD.DATECREATION,
                       AD.DATEMODIFICATION
                   };

                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable DEVIS_APPAREILSDEVIS_RETOURNE()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                   from APPAREILSDEVIS in context.APPAREILSDEVIS
                   select new
                   {
                       APPAREILSDEVIS.PK_ID,
                       APPAREILSDEVIS.FK_IDCODEAPPAREIL,
                       APPAREILSDEVIS.FK_IDDEMANDE,
                       APPAREILSDEVIS.NUMDEM,
                       APPAREILSDEVIS.CODEAPPAREIL,
                       APPAREILSDEVIS.NBRE,
                       APPAREILSDEVIS.PUISSANCE,
                       APPAREILSDEVIS.APPAREILS.DESIGNATION,
                       APPAREILSDEVIS.USERCREATION,
                       APPAREILSDEVIS.USERMODIFICATION,
                       APPAREILSDEVIS.DATECREATION,
                       APPAREILSDEVIS.DATEMODIFICATION
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

        #region CLIENT
        public static DataTable DEVIS_CLIENT_RETOURNE_ListByCentreClientNomProduit(string pProduit, string pCentre, string pClient, string pNom)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = from a in context.AG
                                                from c in a.CLIENT1
                                                from b in a.BRT
                                                from t in c.ABON
                                                join d in context.CANALISATION
                                                      on t.PK_ID
                                                  equals d.FK_IDABON into d_join
                                                from d in d_join.DefaultIfEmpty()
                                                where
                                                (pCentre == null || c.CENTRE == pCentre)
                                                && (pClient == null || c.REFCLIENT.Contains(pClient))
                                                && (pClient == null || c.NOMABON.Contains(pNom))
                                                orderby c.CENTRE, c.REFCLIENT, c.ORDRE
                                                select new
                                                {
                                                    c.PK_ID,
                                                    c.CENTRE,
                                                    c.REFCLIENT,
                                                    c.ORDRE,
                                                    c.NOMABON,
                                                    a.COMMUNE,
                                                    a.QUARTIER,
                                                    RUE = a.RUES.CODE,
                                                    a.PORTE,
                                                    a.TOURNEE,
                                                    d.COMPTEUR,
                                                    c.TELEPHONE,
                                                    c.CNI,
                                                    c.CATEGORIECLIENT
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

        #region CONTROLETRAVAUX
        public static DataTable DEVIS_CONTROLETRAVAUX_RETOURNEByDevisId(int pIdDevis, int pOrdre)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                    from CONTROLETRAVAUXes in context.CONTROLETRAVAUX
                    where
                      CONTROLETRAVAUXes.FK_IDDEMANDE == pIdDevis &&
                      CONTROLETRAVAUXes.ORDRE == pOrdre
                    select new
                    {
                        CONTROLETRAVAUXes.PK_ID,
                        CONTROLETRAVAUXes.FK_IDDEMANDE,
                        CONTROLETRAVAUXes.FK_IDMATRICULE,
                        CONTROLETRAVAUXes.NUMDEM,
                        CONTROLETRAVAUXes.ORDRE,
                        CONTROLETRAVAUXes.MATRICULECHEFEQUIPE,
                        CONTROLETRAVAUXes.NOMCHEFEQUIPE,
                        CONTROLETRAVAUXes.METHODE_MOYEN_CONTROLE,
                        CONTROLETRAVAUXes.DATECONTROLE,
                        CONTROLETRAVAUXes.VOLUMETERTRVX,
                        CONTROLETRAVAUXes.DEGRADATIONVOIE,
                        CONTROLETRAVAUXes.NOTE,
                        CONTROLETRAVAUXes.USERCREATION,
                        CONTROLETRAVAUXes.USERMODIFICATION,
                        CONTROLETRAVAUXes.DATECREATION,
                        CONTROLETRAVAUXes.DATEMODIFICATION
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

        #region DEMANDEDEVIS

        public static DataTable DEVIS_DEMANDEDEVIS_RETOURNEById(int pIdDemandeDevis)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    //IEnumerable<object> query =
                    //from D in context.DEMANDE
                    //join DIA in context.DIAMETRECOMPTEUR  on new { DIAMETRE = D.FK_IDDIAMETRECOMPTEUR  } equals new { DIAMETRE = DIA.PK_ID  } into DIA_join
                    //from DIA in DIA_join.DefaultIfEmpty()
                    //where
                    //  D.PK_ID == pIdDemandeDevis
                    //select new
                    //{
                    //    D.PK_ID,
                    //    D.FK_IDCATEGORIECLIENT,
                    //    D.FK_IDCENTRE,
                    //    D.FK_IDCOMMUNE,
                    //    D.FK_IDDEVIS,
                    //    D.FK_IDDIAMETRECOMPTEUR,
                    //    D.FK_IDQUARTIER,
                    //    D.FK_IDRUE,
                    //    D.FK_IDTOURNEE,
                    //    D.NUMDEVIS ,
                    //    OriginalNUMDEVIS = D.NUMDEVIS ,
                    //    D.CENTRE,
                    //    D.CLIENT,
                    //    D.ORDRECLIENT,
                    //    D.DIAMETRE,
                    //    D.NOM,
                    //    D.TOURNEE ,
                    //    D.PROFESSION,
                    //    D.NUMLOT,
                    //    D.PARCELLE,
                    //    D.SECTION_PAR,
                    //    D.QUARTIER ,
                    //    D.SECTEUR,
                    //    D.NUMTEL,
                    //    D.RUE ,
                    //    D.NUMPORTE,
                    //    D.NUMPOTEAUPROCHE,
                    //    D.CATEGORIECLIENT ,
                    //    D.DATEDEMANDE,
                    //    D.ADRESSE,
                    //    D.COMMUNE ,
                    //    D.REPEREPROCHE,
                    //    D.ORDTOUR,
                    //    LONGITUDE= D.LONGITUDE,
                    //    LATITUDE= D.LATITUDE,
                    //    LIBELLETOURNEE = D.TOURNEE1.LIBELLE,
                    //    LIBELLECOMMUNE = D.COMMUNE1.LIBELLE,
                    //    LIBELLEQUARTIER = D.QUARTIER1.LIBELLE,
                    //    LIBELLERUE = D.RUES.LIBELLE ,
                    //    LIBELLEDIAMETRE = DIA.LIBELLE,
                    //    LIBELLECATEGORIE =D.CATEGORIECLIENT.LIBELLE ,
                    //    D.USERCREATION,
                    //    D.USERMODIFICATION,
                    //    D.DATECREATION,
                    //    D.DATEMODIFICATION
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

        public static DataTable DEVIS_DEMANDEDEVIS_RETOURNEByDevisId(int pIdDevis)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    // IEnumerable<object> query =
                    //from D in context.DEMANDEDEVIS
                    //join DIA in context.DIAMETRECOMPTEUR  on new { DIAMETRE = D.FK_IDDIAMETRECOMPTEUR } equals new { DIAMETRE = DIA.PK_ID } into DIA_join
                    //from DIA in DIA_join.DefaultIfEmpty()
                    // where
                    //   D.FK_IDDEVIS == pIdDevis
                    // select new
                    // {
                    //     D.PK_ID,
                    //     D.FK_IDCATEGORIECLIENT,
                    //     D.FK_IDCENTRE,
                    //     D.FK_IDCOMMUNE,
                    //     D.FK_IDDEVIS,
                    //     D.FK_IDDIAMETRECOMPTEUR,
                    //     D.FK_IDQUARTIER,
                    //     D.FK_IDRUE,
                    //     D.FK_IDTOURNEE,
                    //     D.NUMDEVIS,
                    //     OriginalNUMDEVIS = D.NUMDEVIS,
                    //     D.CENTRE,
                    //     D.CLIENT,
                    //     D.ORDRECLIENT,
                    //     D.DIAMETRE,
                    //     D.NOM,
                    //     D.TOURNEE,
                    //     D.PROFESSION,
                    //     D.NUMLOT,
                    //     D.PARCELLE,
                    //     D.SECTION_PAR,
                    //     D.QUARTIER,
                    //     D.SECTEUR,
                    //     D.NUMTEL,
                    //     D.RUE,
                    //     D.NUMPORTE,
                    //     D.NUMPOTEAUPROCHE,
                    //     D.CATEGORIECLIENT,
                    //     D.DATEDEMANDE,
                    //     D.ADRESSE,
                    //     D.COMMUNE,
                    //     D.REPEREPROCHE,
                    //     D.ORDTOUR,
                    //     LONGITUDE = D.LONGITUDE,
                    //     LATITUDE = D.LATITUDE,
                    //     LIBELLETOURNEE = D.TOURNEE1.LIBELLE,
                    //     LIBELLECOMMUNE = D.COMMUNE1.LIBELLE,
                    //     LIBELLEQUARTIER = D.QUARTIER1.LIBELLE,
                    //     LIBELLERUE = D.RUES.LIBELLE ,
                    //     LIBELLEDIAMETRE = DIA.LIBELLE,
                    //     LIBELLECATEGORIE = D.CATEGORIECLIENT.LIBELLE,
                    //     D.USERCREATION,
                    //     D.USERMODIFICATION,
                    //     D.DATECREATION,
                    //     D.DATEMODIFICATION
                    // };

                    // return Galatee.Tools.Utility.ListToDataTable(query);
                    return new DataTable();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable DEVIS_DEMANDEDEVIS_RETOURNE()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    //IEnumerable<object> query =
                    // from D in context.DEMANDEDEVIS
                    // join DIA in context.DIAMETRECOMPTEUR  on new { DIAMETRE = D.FK_IDDIAMETRECOMPTEUR } equals new { DIAMETRE = DIA.PK_ID } into DIA_join
                    // from DIA in DIA_join.DefaultIfEmpty()
                    //select new
                    //{
                    //    D.PK_ID,
                    //    D.FK_IDCATEGORIECLIENT,
                    //    D.FK_IDCENTRE,
                    //    D.FK_IDCOMMUNE,
                    //    D.FK_IDDEVIS,
                    //    D.FK_IDDIAMETRECOMPTEUR,
                    //    D.FK_IDQUARTIER,
                    //    D.FK_IDRUE,
                    //    D.FK_IDTOURNEE,
                    //    D.NUMDEVIS,
                    //    OriginalNUMDEVIS = D.NUMDEVIS,
                    //    D.CENTRE,
                    //    D.CLIENT,
                    //    D.ORDRECLIENT,
                    //    D.DIAMETRE,
                    //    D.NOM,
                    //    D.TOURNEE,
                    //    D.PROFESSION,
                    //    D.NUMLOT,
                    //    D.PARCELLE,
                    //    D.SECTION_PAR,
                    //    D.QUARTIER,
                    //    D.SECTEUR,
                    //    D.NUMTEL,
                    //    D.RUE,
                    //    D.NUMPORTE,
                    //    D.NUMPOTEAUPROCHE,
                    //    D.CATEGORIECLIENT,
                    //    D.DATEDEMANDE,
                    //    D.ADRESSE,
                    //    D.COMMUNE,
                    //    D.REPEREPROCHE,
                    //    D.ORDTOUR,
                    //    LONGITUDE = D.LONGITUDE,
                    //    LATITUDE = D.LATITUDE,
                    //    LIBELLETOURNEE = D.TOURNEE1.LIBELLE,
                    //    LIBELLECOMMUNE = D.COMMUNE1.LIBELLE,
                    //    LIBELLEQUARTIER = D.QUARTIER1.LIBELLE,
                    //    LIBELLERUE = D.RUES.LIBELLE ,
                    //    LIBELLEDIAMETRE = DIA.LIBELLE,
                    //    LIBELLECATEGORIE = D.CATEGORIECLIENT.LIBELLE,
                    //    D.USERCREATION,
                    //    D.USERMODIFICATION,
                    //    D.DATECREATION,
                    //    D.DATEMODIFICATION
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

        #region DEPOSIT

        public static DataTable DEVIS_DEPOSIT_RETOURNE()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                    from DEPOSIT in context.DEPOSIT
                    where
                      (String)(DEPOSIT.NUMDEM ?? "") == "" &&
                      (String)(DEPOSIT.TOPANNUL ?? "") == "" &&
                      (String)(DEPOSIT.RECEIPT ?? "") != ""
                    select new
                    {
                        DEPOSIT.PK_ID,
                        DEPOSIT.FK_IDCENTRE,
                        DEPOSIT.CENTRE,
                        DEPOSIT.CLIENT,
                        DEPOSIT.ORDRE,
                        DEPOSIT = DEPOSIT.MONTANTDEPOSIT,
                        DEPOSIT.RECEIPT,
                        DEPOSIT.DATEENC,
                        DEPOSIT.NUMDEM,
                        DEPOSIT.NOM,
                        DEPOSIT.TOTAL,
                        DEPOSIT.IDENTITE,
                        DEPOSIT.TOPANNUL,
                        DEPOSIT.MONTANTTVA,
                        DEPOSIT.BANQUE,
                        DEPOSIT.COMPTE,
                        DEPOSIT.IDLETTER,
                        DEPOSIT.ISCREATED,
                        DEPOSIT.USERCREATION,
                        DEPOSIT.USERMODIFICATION,
                        DEPOSIT.DATECREATION,
                        DEPOSIT.DATEMODIFICATION
                    };

                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable DEVIS_DEPOSIT_RETOURNEByNumdevis(string pNumdevis)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                    from DEPOSIT in context.DEPOSIT
                    where
                      DEPOSIT.NUMDEM == pNumdevis &&
                      (String)(DEPOSIT.TOPANNUL ?? "") == ""
                    select new
                    {
                        DEPOSIT.PK_ID,
                        DEPOSIT.FK_IDCENTRE,
                        DEPOSIT.CENTRE,
                        DEPOSIT.CLIENT,
                        DEPOSIT.ORDRE,
                        DEPOSIT = DEPOSIT.MONTANTDEPOSIT,
                        DEPOSIT.RECEIPT,
                        DEPOSIT.DATEENC,
                        DEPOSIT.NUMDEM,
                        DEPOSIT.NOM,
                        DEPOSIT.TOTAL,
                        DEPOSIT.IDENTITE,
                        DEPOSIT.TOPANNUL,
                        DEPOSIT.MONTANTTVA,
                        DEPOSIT.BANQUE,
                        DEPOSIT.COMPTE,
                        DEPOSIT.IDLETTER,
                        DEPOSIT.ISCREATED,
                        DEPOSIT.USERCREATION,
                        DEPOSIT.USERMODIFICATION,
                        DEPOSIT.DATECREATION,
                        DEPOSIT.DATEMODIFICATION
                    };

                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable DEVIS_DEPOSIT_RETOURNEByReceipt(string pReceipt)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                    from DEPOSIT in context.DEPOSIT
                    where
                      DEPOSIT.RECEIPT == pReceipt &&
                      (String)(DEPOSIT.TOPANNUL ?? "") == ""
                    select new
                    {
                        DEPOSIT.PK_ID,
                        DEPOSIT.FK_IDCENTRE,
                        DEPOSIT.CENTRE,
                        DEPOSIT.CLIENT,
                        DEPOSIT.ORDRE,
                        DEPOSIT = DEPOSIT.MONTANTDEPOSIT,
                        DEPOSIT.RECEIPT,
                        DEPOSIT.DATEENC,
                        DEPOSIT.NUMDEM,
                        DEPOSIT.NOM,
                        DEPOSIT.TOTAL,
                        DEPOSIT.IDENTITE,
                        DEPOSIT.TOPANNUL,
                        DEPOSIT.MONTANTTVA,
                        DEPOSIT.BANQUE,
                        DEPOSIT.COMPTE,
                        DEPOSIT.IDLETTER,
                        DEPOSIT.ISCREATED,
                        DEPOSIT.USERCREATION,
                        DEPOSIT.USERMODIFICATION,
                        DEPOSIT.DATECREATION,
                        DEPOSIT.DATEMODIFICATION
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

        #region DEVIS

        public static DataTable DEVIS_DEVIS_RETOURNEByCodeAppareilFromAPPAREILSDEVIS(int pIdAppareil)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                    from DEVIS in context.DEMANDE
                    where
                        (from AP in context.APPAREILSDEVIS
                         where
                           AP.PK_ID == pIdAppareil &&
                           AP.NUMDEM == AP.NUMDEM
                         select new
                         {
                             Column1 = 1
                         }).FirstOrDefault().Column1 != null
                    select new
                    {
                        DEVIS.PK_ID,
                        //DEVIS.CODETAXE ,
                        //DEVIS.FK_IDETAPEDEVIS,
                        //DEVIS.FK_IDCENTRE,
                        //DEVIS.FK_IDPIECEIDENTITE,
                        //DEVIS.FK_IDPRODUIT,
                        //DEVIS.FK_IDSITE,
                        //DEVIS.FK_IDTYPEDEVIS,
                        //DEVIS.NUMDEVIS,
                        //DEVIS.CODECENTRE,
                        //DEVIS.CODESITE,
                        //DEVIS.CODEPRODUIT,
                        //DEVIS.DATEDECREATION,
                        //DEVIS.DATEETAPE,
                        //DEVIS.MONTANTHT,
                        //DEVIS.MONTANTTTC,
                        //DEVIS.MONTANTTOUTORDRE,
                        //DEVIS.NUMEROCTR,
                        //DEVIS.MOTIFREJET,
                        //DEVIS.DATEREGLEMENT,
                        //DEVIS.MATRICULECAISSE ,
                        //DEVIS.ORDRE,
                        //DEVIS.IDSCHEMA ,
                        //DEVIS.ISFOURNITURE,
                        //DEVIS.ISPOSE,
                        //DEVIS.ISANALYSED,
                        //DEVIS.PUISSANCESOUSCRITE,
                        //DEVIS.IDTYPECTR,
                        //DEVIS.IDMARQUECTR,
                        //DEVIS.IDOWNERSHIP,
                        //DEVIS.DATEFABRICATIONCTR,
                        //DEVIS.DATEPOSECTR,
                        //DEVIS.INDEXPOSECTR,
                        //DEVIS.DISTANCE,
                        //DEVIS.OWNERSHIPPROOFID ,
                        //DEVIS.NUMEROPIECEIDENTITE,
                        //DEVIS.NUMEROGPS,
                        //DEVIS.NEARESTROUTE,
                        //DEVIS.ISBRACKET,
                        //DEVIS.ISSERVICEPOLE,
                        //DEVIS.ESTSIMPLIFIE,
                        //DEVIS.ESTCOMPLET,
                        //DEVIS.ISSUBVENTION,
                        //DEVIS.ISEXTENSION,
                        //DEVIS.IDMANUSCRIT,
                        //DEVIS.EMPLACEMENTCOMPTEUR,
                        //DEVIS.MONTANTCOMPLEMENTAIRE,
                        //DEVIS.MONTANTPARTICIPATION,
                        //DEVIS.FRAISPOSE,
                        //DEVIS.ISPARTICIPATION,
                        DEVIS.USERCREATION,
                        DEVIS.USERMODIFICATION,
                        DEVIS.DATECREATION,
                        DEVIS.DATEMODIFICATION
                    };

                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public static DataTable _DEVIS_DEVIS_RETOURNE()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    return new DataTable();
                    //IEnumerable<object> query =
                    // from D in context.DEVIS
                    // join C in context.CENTRE on D.CODECENTRE equals C.CODECENTRE 
                    // join TY in context.TYPEDEVIS on D.FK_IDTYPEDEVIS equals TY.PK_ID
                    // join P in context.PRODUIT on D.CODEPRODUIT equals P.CODE  
                    // join PIE in context.PIECEIDENTITE on D.FK_IDPIECEIDENTITE equals PIE.PK_ID
                    // join ET in context.ETAPEDEVIS on new { D.FK_IDETAPEDEVIS}
                    //   equals new { FK_IDETAPEDEVIS = ET.PK_ID}
                    // join S in context.SITE on new { Site = D.CODESITE  } equals new { Site = S.CODESITE  }
                    // join DEP in context.DEVISPIA on D.NUMDEVIS  equals DEP.NUMDEVIS  into DEP_join
                    // from DEP in DEP_join.DefaultIfEmpty()
                    // orderby
                    //   D.NUMDEVIS  descending
                    // select new
                    // {
                    //     D.PK_ID,
                    //     D.CODETAXE ,
                    //     D.FK_IDETAPEDEVIS,
                    //     D.FK_IDCENTRE,
                    //     D.FK_IDPIECEIDENTITE,
                    //     D.FK_IDPRODUIT,
                    //     D.FK_IDSITE,
                    //     D.FK_IDTYPEDEVIS,
                    //     D.NUMDEVIS,
                    //     D.CODECENTRE,
                    //     D.CODESITE,
                    //     D.CODEPRODUIT,
                    //     D.DATEDECREATION,
                    //     D.DATEETAPE,
                    //     D.MONTANTHT,
                    //     D.MONTANTTTC,
                    //     D.MONTANTTOUTORDRE,
                    //     D.NUMEROCTR,
                    //     D.MOTIFREJET,
                    //     D.DATEREGLEMENT,
                    //     D.MATRICULECAISSE ,
                    //     D.ORDRE,
                    //     D.IDSCHEMA ,
                    //     D.ISFOURNITURE,
                    //     D.ISPOSE,
                    //     D.ISANALYSED,
                    //     D.PUISSANCESOUSCRITE,
                    //     D.IDTYPECTR,
                    //     D.IDMARQUECTR,
                    //     D.IDOWNERSHIP,
                    //     D.DATEFABRICATIONCTR,
                    //     D.DATEPOSECTR,
                    //     D.INDEXPOSECTR,
                    //     D.DISTANCE,
                    //     D.OWNERSHIPPROOFID ,
                    //     D.NUMEROPIECEIDENTITE,
                    //     D.NUMEROGPS,
                    //     D.NEARESTROUTE,
                    //     D.ISBRACKET,
                    //     D.ISSERVICEPOLE,
                    //     D.ESTSIMPLIFIE,
                    //     D.ESTCOMPLET,
                    //     D.ISSUBVENTION,
                    //     D.ISEXTENSION,
                    //     D.IDMANUSCRIT,
                    //     D.EMPLACEMENTCOMPTEUR,
                    //     D.MONTANTCOMPLEMENTAIRE,
                    //     D.MONTANTPARTICIPATION,
                    //     D.FRAISPOSE,
                    //     D.ISPARTICIPATION,
                    //     LIBELLECENTRE = C.LIBELLE,
                    //     LIBELLETYPEDEMANDE = TY.LIBELLE,
                    //     LIBELLEPRODUIT = P.LIBELLE,
                    //     LIBELLETYPEPIECE = PIE.LIBELLE,
                    //     LIBELLESITE = S.LIBELLE,
                    //     LIBELLETACHE = D.ETAPEDEVIS.TACHEDEVIS.LIBELLE,
                    //     MATRICULEPIA = DEP.MATRICULEPIA ,
                    //     DELAIEXECUTIONETAPE = D.ETAPEDEVIS.DELAIEXECUTIONETAPE,
                    //     D.USERCREATION,
                    //     D.USERMODIFICATION,
                    //     D.DATECREATION,
                    //     D.DATEMODIFICATION
                    // };

                    //return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public static DataTable DEVIS_DEVIS_RETOURNEByCentre(String pCentre)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                    from demande in context.DEMANDE
                    where
                      demande.CENTRE == pCentre
                    select new
                    {
                        demande.PK_ID,
                        demande.FK_IDTYPEDEMANDE,
                        demande.FK_IDCENTRE,
                        //DEVIS.FK_IDPIECEIDENTITE,
                        //DEVIS.FK_IDPRODUIT,
                        //DEVIS.FK_IDSITE,
                        //DEVIS.FK_IDTYPEDEVIS,
                        //DEVIS.NUMDEVIS,
                        //DEVIS.CODECENTRE,
                        //DEVIS.CODESITE,
                        //DEVIS.CODEPRODUIT,
                        //DEVIS.DATEDECREATION,
                        //DEVIS.DATEETAPE,
                        //DEVIS.MONTANTHT,
                        //DEVIS.MONTANTTTC,
                        //DEVIS.MONTANTTOUTORDRE,
                        //DEVIS.NUMEROCTR,
                        //DEVIS.MOTIFREJET,
                        //DEVIS.DATEREGLEMENT,
                        //DEVIS.MATRICULECAISSE ,
                        //DEVIS.ORDRE,
                        //DEVIS.IDSCHEMA ,
                        //DEVIS.ISFOURNITURE,
                        //DEVIS.ISPOSE,
                        //DEVIS.ISANALYSED,
                        //DEVIS.PUISSANCESOUSCRITE,
                        //DEVIS.IDTYPECTR,
                        //DEVIS.IDMARQUECTR,
                        //DEVIS.IDOWNERSHIP,
                        //DEVIS.DATEFABRICATIONCTR,
                        //DEVIS.DATEPOSECTR,
                        //DEVIS.INDEXPOSECTR,
                        //DEVIS.DISTANCE,
                        //DEVIS.OWNERSHIPPROOFID,
                        //DEVIS.NUMEROPIECEIDENTITE,
                        //DEVIS.NUMEROGPS,
                        //DEVIS.NEARESTROUTE,
                        //DEVIS.ISBRACKET,
                        //DEVIS.ISSERVICEPOLE,
                        //DEVIS.ESTSIMPLIFIE,
                        //DEVIS.ESTCOMPLET,
                        //DEVIS.ISSUBVENTION,
                        //DEVIS.ISEXTENSION,
                        //DEVIS.IDMANUSCRIT,
                        //DEVIS.EMPLACEMENTCOMPTEUR,
                        //DEVIS.MONTANTCOMPLEMENTAIRE,
                        //DEVIS.MONTANTPARTICIPATION,
                        //DEVIS.FRAISPOSE,
                        //DEVIS.ISPARTICIPATION,
                        //DEVIS.USERCREATION,
                        //DEVIS.USERMODIFICATION,
                        //DEVIS.DATECREATION,
                        //DEVIS.DATEMODIFICATION

                    };
                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public static DataTable DEVIS_DEVIS_RETOURNE()
        {
            try
            {

                using (galadbEntities context = new galadbEntities())
                {
                    return new DataTable();
                    //    IEnumerable<object> query =
                    //    from D in context.DEVIS
                    //    join DEP in context.DEVISPIA on D.NUMDEVIS equals DEP.NUMDEVIS into DEP_join
                    //    from DEP in DEP_join.DefaultIfEmpty()
                    //    orderby
                    //      D.NUMDEVIS descending
                    //    select new
                    //    {
                    //        D.PK_ID,
                    //        D.CODETAXE,
                    //        D.FK_IDETAPEDEVIS,
                    //        D.FK_IDCENTRE,
                    //        D.FK_IDPIECEIDENTITE,
                    //        D.FK_IDPRODUIT,
                    //        D.FK_IDSITE,
                    //        D.FK_IDTYPEDEVIS,
                    //        D.NUMDEVIS,
                    //        D.CODECENTRE,
                    //        D.CODESITE,
                    //        D.CODEPRODUIT,
                    //        D.DATEDECREATION,
                    //        D.DATEETAPE,
                    //        D.MONTANTHT,
                    //        D.MONTANTTTC,
                    //        D.MONTANTTOUTORDRE,
                    //        D.NUMEROCTR,
                    //        D.MOTIFREJET,
                    //        D.DATEREGLEMENT,
                    //        D.MATRICULECAISSE,
                    //        D.ORDRE,
                    //        D.IDSCHEMA,
                    //        D.ISFOURNITURE,
                    //        D.ISPOSE,
                    //        D.ISANALYSED,
                    //        D.PUISSANCESOUSCRITE,
                    //        D.IDTYPECTR,
                    //        D.IDMARQUECTR,
                    //        D.IDOWNERSHIP,
                    //        D.DATEFABRICATIONCTR,
                    //        D.DATEPOSECTR,
                    //        D.INDEXPOSECTR,
                    //        D.DISTANCE,
                    //        D.OWNERSHIPPROOFID,
                    //        D.NUMEROPIECEIDENTITE,
                    //        D.NUMEROGPS,
                    //        D.NEARESTROUTE,
                    //        D.ISBRACKET,
                    //        D.ISSERVICEPOLE,
                    //        D.ESTSIMPLIFIE,
                    //        D.ESTCOMPLET,
                    //        D.ISSUBVENTION,
                    //        D.ISEXTENSION,
                    //        D.IDMANUSCRIT,
                    //        D.EMPLACEMENTCOMPTEUR,
                    //        D.MONTANTCOMPLEMENTAIRE,
                    //        D.MONTANTPARTICIPATION,
                    //        D.FRAISPOSE,
                    //        D.ISPARTICIPATION,
                    //        LIBELLECENTRE = D.CENTRE.LIBELLE,
                    //        LIBELLETYPEDEMANDE = D.TYPEDEVIS.LIBELLE,
                    //        LIBELLEPRODUIT = D.PRODUIT.LIBELLE,
                    //        LIBELLETYPEPIECE = D.PIECEIDENTITE.LIBELLE,
                    //        LIBELLESITE = D.SITE.LIBELLE,
                    //        LIBELLETACHE = D.ETAPEDEVIS.TACHEDEVIS.LIBELLE,
                    //        MATRICULEPIA = DEP.MATRICULEPIA,
                    //        DELAIEXECUTIONETAPE = D.ETAPEDEVIS.DELAIEXECUTIONETAPE,
                    //        D.USERCREATION,
                    //        D.USERMODIFICATION,
                    //        D.DATECREATION,
                    //        D.DATEMODIFICATION
                    //    };
                    //    return Galatee.Tools.Utility.ListToDataTable(query);
                    ////using (galadbEntities context = new galadbEntities())
                    ////{
                    ////    IEnumerable<object> query =
                    ////     from D in context.DEVIS
                    ////     join C in context.CENTRE on D.CODECENTRE equals C.CODECENTRE
                    ////     join TY in context.TYPEDEVIS on D.FK_IDTYPEDEVIS equals TY.PK_ID
                    ////     join P in context.PRODUIT on D.CODEPRODUIT  equals P.CODE 
                    ////     join PIE in context.PIECEIDENTITE on D.FK_IDPIECEIDENTITE equals PIE.PK_ID
                    ////     join ET in context.ETAPEDEVIS on new { D.FK_IDETAPEDEVIS }
                    ////       equals new { FK_IDETAPEDEVIS = ET.PK_ID }
                    ////     join S in context.SITE on new { Site = D.CODESITE  } equals new { Site = S.CODESITE  }
                    ////     join DEP in context.DEVISPIA on D.NUMDEVIS  equals DEP.NUMDEVIS  into DEP_join
                    ////     from DEP in DEP_join.DefaultIfEmpty()
                    ////     orderby
                    ////       D.NUMDEVIS  descending
                    ////     select new
                    ////     {
                    ////         D.PK_ID,
                    ////         D.CODETAXE ,
                    ////         D.FK_IDETAPEDEVIS,
                    ////         D.FK_IDCENTRE,
                    ////         D.FK_IDPIECEIDENTITE,
                    ////         D.FK_IDPRODUIT,
                    ////         D.FK_IDSITE,
                    ////         D.FK_IDTYPEDEVIS,
                    ////         D.NUMDEVIS,
                    ////         D.CODECENTRE,
                    ////         D.CODESITE,
                    ////         D.CODEPRODUIT,
                    ////         D.DATEDECREATION,
                    ////         D.DATEETAPE,
                    ////         D.MONTANTHT,
                    ////         D.MONTANTTTC,
                    ////         D.MONTANTTOUTORDRE,
                    ////         D.NUMEROCTR,
                    ////         D.MOTIFREJET,
                    ////         D.DATEREGLEMENT,
                    ////         D.MATRICULECAISSE ,
                    ////         D.ORDRE,
                    ////         D.IDSCHEMA ,
                    ////         D.ISFOURNITURE,
                    ////         D.ISPOSE,
                    ////         D.ISANALYSED,
                    ////         D.PUISSANCESOUSCRITE,
                    ////         D.IDTYPECTR,
                    ////         D.IDMARQUECTR,
                    ////         D.IDOWNERSHIP ,
                    ////         D.DATEFABRICATIONCTR,
                    ////         D.DATEPOSECTR,
                    ////         D.INDEXPOSECTR,
                    ////         D.DISTANCE,
                    ////         D.OWNERSHIPPROOFID,
                    ////         D.NUMEROPIECEIDENTITE,
                    ////         D.NUMEROGPS,
                    ////         D.NEARESTROUTE,
                    ////         D.ISBRACKET,
                    ////         D.ISSERVICEPOLE,
                    ////         D.ESTSIMPLIFIE,
                    ////         D.ESTCOMPLET,
                    ////         D.ISSUBVENTION,
                    ////         D.ISEXTENSION,
                    ////         D.IDMANUSCRIT,
                    ////         D.EMPLACEMENTCOMPTEUR,
                    ////         D.MONTANTCOMPLEMENTAIRE,
                    ////         D.MONTANTPARTICIPATION,
                    ////         D.FRAISPOSE,
                    ////         D.ISPARTICIPATION,
                    ////         LIBELLECENTRE = C.LIBELLE,
                    ////         LIBELLETYPEDEMANDE = TY.LIBELLE,
                    ////         LIBELLEPRODUIT = P.LIBELLE,
                    ////         LIBELLETYPEPIECE = PIE.LIBELLE,
                    ////         LIBELLESITE = S.LIBELLE,
                    ////         LIBELLETACHE = D.ETAPEDEVIS.TACHEDEVIS.LIBELLE,
                    ////         MATRICULEPIA = DEP.MATRICULEPIA,
                    ////         DELAIEXECUTIONETAPE = D.ETAPEDEVIS.DELAIEXECUTIONETAPE,
                    ////         D.USERCREATION,
                    ////         D.USERMODIFICATION,
                    ////         D.DATECREATION,
                    ////         D.DATEMODIFICATION
                    ////     };
                    ////    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public static DataTable DEVIS_DEVIS_RETOURNEByIdSchema(Guid pIdSchema)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    return new DataTable();
                    //IEnumerable<object> query =
                    //    from DEVIS in context.DEVIS
                    //    where
                    //      DEVIS.IDSCHEMA == pIdSchema
                    //    select new
                    //    {
                    //        DEVIS.PK_ID,
                    //        DEVIS.CODETAXE ,
                    //        DEVIS.FK_IDETAPEDEVIS,
                    //        DEVIS.FK_IDCENTRE,
                    //        DEVIS.FK_IDPIECEIDENTITE,
                    //        DEVIS.FK_IDPRODUIT,
                    //        DEVIS.FK_IDSITE,
                    //        DEVIS.FK_IDTYPEDEVIS,
                    //        DEVIS.NUMDEVIS,
                    //        DEVIS.CODECENTRE,
                    //        DEVIS.CODESITE,
                    //        DEVIS.CODEPRODUIT,
                    //        DEVIS.DATEDECREATION,
                    //        DEVIS.DATEETAPE,
                    //        DEVIS.MONTANTHT,
                    //        DEVIS.MONTANTTTC,
                    //        DEVIS.MONTANTTOUTORDRE,
                    //        DEVIS.NUMEROCTR,
                    //        DEVIS.MOTIFREJET,
                    //        DEVIS.DATEREGLEMENT,
                    //        DEVIS.MATRICULECAISSE,
                    //        DEVIS.ORDRE,
                    //        DEVIS.IDSCHEMA,
                    //        DEVIS.ISFOURNITURE,
                    //        DEVIS.ISPOSE,
                    //        DEVIS.ISANALYSED,
                    //        DEVIS.PUISSANCESOUSCRITE,
                    //        DEVIS.IDTYPECTR,
                    //        DEVIS.IDMARQUECTR,
                    //        DEVIS.IDOWNERSHIP,
                    //        DEVIS.DATEFABRICATIONCTR,
                    //        DEVIS.DATEPOSECTR,
                    //        DEVIS.INDEXPOSECTR,
                    //        DEVIS.DISTANCE,
                    //        DEVIS.OWNERSHIPPROOFID,
                    //        DEVIS.NUMEROPIECEIDENTITE,
                    //        DEVIS.NUMEROGPS,
                    //        DEVIS.NEARESTROUTE,
                    //        DEVIS.ISBRACKET,
                    //        DEVIS.ISSERVICEPOLE,
                    //        DEVIS.ESTSIMPLIFIE,
                    //        DEVIS.ESTCOMPLET,
                    //        DEVIS.ISSUBVENTION,
                    //        DEVIS.ISEXTENSION,
                    //        DEVIS.IDMANUSCRIT,
                    //        DEVIS.EMPLACEMENTCOMPTEUR,
                    //        DEVIS.MONTANTCOMPLEMENTAIRE,
                    //        DEVIS.MONTANTPARTICIPATION,
                    //        DEVIS.FRAISPOSE,
                    //        DEVIS.ISPARTICIPATION,
                    //        DEVIS.USERCREATION,
                    //        DEVIS.USERMODIFICATION,
                    //        DEVIS.DATECREATION,
                    //        DEVIS.DATEMODIFICATION
                    //    };
                    //return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public static DataTable DEVIS_DEVIS_RETOURNEByIdOwnerShip(Guid pIdOwnerShip)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    return new DataTable();
                    //IEnumerable<object> query =
                    //    from DEVIS in context.DEVIS
                    //    where
                    //      DEVIS.IDOWNERSHIP == pIdOwnerShip
                    //    select new
                    //    {
                    //        DEVIS.PK_ID,
                    //        DEVIS.CODETAXE ,
                    //        DEVIS.FK_IDETAPEDEVIS,
                    //        DEVIS.FK_IDCENTRE,
                    //        DEVIS.FK_IDPIECEIDENTITE,
                    //        DEVIS.FK_IDPRODUIT,
                    //        DEVIS.FK_IDSITE,
                    //        DEVIS.FK_IDTYPEDEVIS,
                    //        DEVIS.NUMDEVIS,
                    //        DEVIS.CODECENTRE,
                    //        DEVIS.CODESITE,
                    //        DEVIS.CODEPRODUIT,
                    //        DEVIS.DATEDECREATION,
                    //        DEVIS.DATEETAPE,
                    //        DEVIS.MONTANTHT,
                    //        DEVIS.MONTANTTTC,
                    //        DEVIS.MONTANTTOUTORDRE,
                    //        DEVIS.NUMEROCTR,
                    //        DEVIS.MOTIFREJET,
                    //        DEVIS.DATEREGLEMENT,
                    //        DEVIS.MATRICULECAISSE,
                    //        DEVIS.ORDRE,
                    //        DEVIS.IDSCHEMA,
                    //        DEVIS.ISFOURNITURE,
                    //        DEVIS.ISPOSE,
                    //        DEVIS.ISANALYSED,
                    //        DEVIS.PUISSANCESOUSCRITE,
                    //        DEVIS.IDTYPECTR,
                    //        DEVIS.IDMARQUECTR,
                    //        DEVIS.IDOWNERSHIP,
                    //        DEVIS.DATEFABRICATIONCTR,
                    //        DEVIS.DATEPOSECTR,
                    //        DEVIS.INDEXPOSECTR,
                    //        DEVIS.DISTANCE,
                    //        DEVIS.OWNERSHIPPROOFID,
                    //        DEVIS.NUMEROPIECEIDENTITE,
                    //        DEVIS.NUMEROGPS,
                    //        DEVIS.NEARESTROUTE,
                    //        DEVIS.ISBRACKET,
                    //        DEVIS.ISSERVICEPOLE,
                    //        DEVIS.ESTSIMPLIFIE,
                    //        DEVIS.ESTCOMPLET,
                    //        DEVIS.ISSUBVENTION,
                    //        DEVIS.ISEXTENSION,
                    //        DEVIS.IDMANUSCRIT,
                    //        DEVIS.EMPLACEMENTCOMPTEUR,
                    //        DEVIS.MONTANTCOMPLEMENTAIRE,
                    //        DEVIS.MONTANTPARTICIPATION,
                    //        DEVIS.FRAISPOSE,
                    //        DEVIS.ISPARTICIPATION,
                    //        DEVIS.USERCREATION,
                    //        DEVIS.USERMODIFICATION,
                    //        DEVIS.DATECREATION,
                    //        DEVIS.DATEMODIFICATION
                    //    };
                    //return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public static DataTable DEVIS_DEVIS_RETOURNEByIdManuscrit(Guid pIdManuscrit)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    return new DataTable();
                    //IEnumerable<object> query =
                    //    from DEVIS in context.DEVIS
                    //    where
                    //      DEVIS.IDMANUSCRIT == pIdManuscrit
                    //    select new
                    //    {
                    //        DEVIS.PK_ID,
                    //        DEVIS.CODETAXE ,
                    //        DEVIS.FK_IDETAPEDEVIS,
                    //        DEVIS.FK_IDCENTRE,
                    //        DEVIS.FK_IDPIECEIDENTITE,
                    //        DEVIS.FK_IDPRODUIT,
                    //        DEVIS.FK_IDSITE,
                    //        DEVIS.FK_IDTYPEDEVIS,
                    //        DEVIS.CODECENTRE,
                    //        DEVIS.CODESITE,
                    //        DEVIS.CODEPRODUIT,
                    //        DEVIS.PRODUIT,
                    //        DEVIS.DATEDECREATION,
                    //        DEVIS.DATEETAPE,
                    //        DEVIS.MONTANTHT,
                    //        DEVIS.MONTANTTTC,
                    //        DEVIS.MONTANTTOUTORDRE,
                    //        DEVIS.NUMEROCTR,
                    //        DEVIS.MOTIFREJET,
                    //        DEVIS.DATEREGLEMENT,
                    //        DEVIS.MATRICULECAISSE,
                    //        DEVIS.ORDRE,
                    //        DEVIS.IDSCHEMA,
                    //        DEVIS.ISFOURNITURE,
                    //        DEVIS.ISPOSE,
                    //        DEVIS.ISANALYSED,
                    //        DEVIS.PUISSANCESOUSCRITE,
                    //        DEVIS.IDTYPECTR,
                    //        DEVIS.IDMARQUECTR,
                    //        DEVIS.IDOWNERSHIP,
                    //        DEVIS.DATEFABRICATIONCTR,
                    //        DEVIS.DATEPOSECTR,
                    //        DEVIS.INDEXPOSECTR,
                    //        DEVIS.DISTANCE,
                    //        DEVIS.OWNERSHIPPROOFID,
                    //        DEVIS.NUMEROPIECEIDENTITE,
                    //        DEVIS.NUMEROGPS,
                    //        DEVIS.NEARESTROUTE,
                    //        DEVIS.ISBRACKET,
                    //        DEVIS.ISSERVICEPOLE,
                    //        DEVIS.ESTSIMPLIFIE,
                    //        DEVIS.ESTCOMPLET,
                    //        DEVIS.ISSUBVENTION,
                    //        DEVIS.ISEXTENSION,
                    //        DEVIS.IDMANUSCRIT,
                    //        DEVIS.EMPLACEMENTCOMPTEUR,
                    //        DEVIS.MONTANTCOMPLEMENTAIRE,
                    //        DEVIS.MONTANTPARTICIPATION,
                    //        DEVIS.FRAISPOSE,
                    //        DEVIS.ISPARTICIPATION,
                    //        DEVIS.USERCREATION,
                    //        DEVIS.USERMODIFICATION,
                    //        DEVIS.DATECREATION,
                    //        DEVIS.DATEMODIFICATION
                    //    };
                    //return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public static DataTable DEVIS_DEVIS_RETOURNEByNumEtape(int pNumEtape)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                    from D in context.DEMANDE
                    orderby
                      D.NUMDEM descending
                    where D.ETAPEDEMANDE == pNumEtape
                    select new
                    {
                        D.PK_ID,
                        D.FK_IDCENTRE,
                        D.FK_IDPRODUIT,
                        D.NUMDEM,
                        D.CENTRE,
                        D.CENTRE1.SITE.CODE,
                        D.PRODUIT,
                        D.ORDRE,
                        LIBELLECENTRE = D.CENTRE1.LIBELLE,
                        LIBELLEPRODUIT = D.PRODUIT1.LIBELLE,
                        LIBELLESITE = D.CENTRE1.SITE.LIBELLE,
                        MATRICULEPIA = D.MATRICULE,
                        D.USERCREATION,
                        D.DATECREATION,
                        D.USERMODIFICATION,
                        D.DATEMODIFICATION
                    };
                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        //public static DataTable DEVIS_DEVIS_RETOURNEByNumEtape(int pNumEtape)
        //{
        //    try
        //    {
        //        using (galadbEntities context = new galadbEntities())
        //        {
        //            IEnumerable<object> query =
        //            from D in context.DEVIS
        //            join DEP in context.DEVISPIA on D.NUMDEVIS equals DEP.NUMDEVIS into DEP_join
        //            from DEP in DEP_join.DefaultIfEmpty()
        //            orderby
        //              D.NUMDEVIS descending
        //            where D.ETAPEDEVIS.NUMETAPE == pNumEtape 
        //            select new
        //            {
        //                D.PK_ID,
        //                D.CODETAXE ,
        //                D.FK_IDETAPEDEVIS,
        //                D.FK_IDCENTRE,
        //                D.FK_IDPIECEIDENTITE,
        //                D.FK_IDPRODUIT,
        //                D.FK_IDSITE,
        //                D.FK_IDTYPEDEVIS,
        //                D.NUMDEVIS,
        //                D.CODECENTRE,
        //                D.CODESITE,
        //                D.CODEPRODUIT ,
        //                D.DATEDECREATION,
        //                D.DATEETAPE,
        //                D.MONTANTHT,
        //                D.MONTANTTTC,
        //                D.MONTANTTOUTORDRE,
        //                D.NUMEROCTR,
        //                D.MOTIFREJET,
        //                D.DATEREGLEMENT,
        //                D.MATRICULECAISSE ,
        //                D.ORDRE,
        //                D.IDSCHEMA ,
        //                D.ISFOURNITURE,
        //                D.ISPOSE,
        //                D.ISANALYSED,
        //                D.PUISSANCESOUSCRITE,
        //                D.IDTYPECTR,
        //                D.IDMARQUECTR,
        //                D.IDOWNERSHIP ,
        //                D.DATEFABRICATIONCTR,
        //                D.DATEPOSECTR,
        //                D.INDEXPOSECTR,
        //                D.DISTANCE,
        //                D.OWNERSHIPPROOFID ,
        //                D.NUMEROPIECEIDENTITE,
        //                D.NUMEROGPS,
        //                D.NEARESTROUTE,
        //                D.ISBRACKET,
        //                D.ISSERVICEPOLE,
        //                D.ESTSIMPLIFIE,
        //                D.ESTCOMPLET,
        //                D.ISSUBVENTION,
        //                D.ISEXTENSION,
        //                D.IDMANUSCRIT,
        //                D.EMPLACEMENTCOMPTEUR,
        //                D.MONTANTCOMPLEMENTAIRE,
        //                D.MONTANTPARTICIPATION,
        //                D.FRAISPOSE,
        //                D.ISPARTICIPATION,
        //                LIBELLECENTRE = D.CENTRE.LIBELLE,
        //                LIBELLETYPEDEMANDE = D.TYPEDEVIS.LIBELLE,
        //                LIBELLEPRODUIT = D.PRODUIT.LIBELLE,
        //                LIBELLETYPEPIECE = D.PIECEIDENTITE.LIBELLE,
        //                LIBELLESITE = D.SITE .LIBELLE,
        //                LIBELLETACHE = D.ETAPEDEVIS.TACHEDEVIS.LIBELLE,
        //                MATRICULEPIA = DEP.MATRICULEPIA ,
        //                DELAIEXECUTIONETAPE = D.ETAPEDEVIS.DELAIEXECUTIONETAPE,
        //                D.USERCREATION,
        //                D.USERMODIFICATION,
        //                D.DATECREATION,
        //                D.DATEMODIFICATION
        //            };
        //            return Galatee.Tools.Utility.ListToDataTable(query);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //}

        public static DataTable DEVIS_DEVIS_RETOURNEByIdEtapeDevis(int pIdEtapeDevis)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    return new DataTable();
                    //IEnumerable<object> query =
                    //    from DEVIS in context.DEVIS
                    //    where
                    //      DEVIS.FK_IDETAPEDEVIS == pIdEtapeDevis
                    //    select new
                    //    {
                    //        DEVIS.PK_ID,
                    //        DEVIS.CODETAXE,
                    //        DEVIS.FK_IDETAPEDEVIS,
                    //        DEVIS.FK_IDCENTRE,
                    //        DEVIS.FK_IDPIECEIDENTITE,
                    //        DEVIS.FK_IDPRODUIT,
                    //        DEVIS.FK_IDSITE,
                    //        DEVIS.FK_IDTYPEDEVIS,
                    //        DEVIS.NUMDEVIS,
                    //        DEVIS.CODECENTRE,
                    //        DEVIS.CODESITE,
                    //        DEVIS.CODEPRODUIT,
                    //        DEVIS.DATEDECREATION,
                    //        DEVIS.DATEETAPE,
                    //        DEVIS.MONTANTHT,
                    //        DEVIS.MONTANTTTC,
                    //        DEVIS.MONTANTTOUTORDRE,
                    //        DEVIS.NUMEROCTR,
                    //        DEVIS.MOTIFREJET,
                    //        DEVIS.DATEREGLEMENT,
                    //        DEVIS.MATRICULECAISSE,
                    //        DEVIS.ORDRE,
                    //        DEVIS.IDSCHEMA,
                    //        DEVIS.ISFOURNITURE,
                    //        DEVIS.ISPOSE,
                    //        DEVIS.ISANALYSED,
                    //        DEVIS.PUISSANCESOUSCRITE,
                    //        DEVIS.IDTYPECTR,
                    //        DEVIS.IDMARQUECTR,
                    //        DEVIS.IDOWNERSHIP,
                    //        DEVIS.DATEFABRICATIONCTR,
                    //        DEVIS.DATEPOSECTR,
                    //        DEVIS.INDEXPOSECTR,
                    //        DEVIS.DISTANCE,
                    //        DEVIS.OWNERSHIPPROOFID,
                    //        DEVIS.NUMEROPIECEIDENTITE,
                    //        DEVIS.NUMEROGPS,
                    //        DEVIS.NEARESTROUTE,
                    //        DEVIS.ISBRACKET,
                    //        DEVIS.ISSERVICEPOLE,
                    //        DEVIS.ESTSIMPLIFIE,
                    //        DEVIS.ESTCOMPLET,
                    //        DEVIS.ISSUBVENTION,
                    //        DEVIS.ISEXTENSION,
                    //        DEVIS.IDMANUSCRIT,
                    //        DEVIS.EMPLACEMENTCOMPTEUR,
                    //        DEVIS.MONTANTCOMPLEMENTAIRE,
                    //        DEVIS.MONTANTPARTICIPATION,
                    //        DEVIS.FRAISPOSE,
                    //        DEVIS.ISPARTICIPATION,
                    //        DEVIS.USERCREATION,
                    //        DEVIS.USERMODIFICATION,
                    //        DEVIS.DATECREATION,
                    //        DEVIS.DATEMODIFICATION
                    //    };
                    //return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable DEVIS_DEVIS_RETOURNEByFK_IDPIECEIDENTITE(int pIdPieceDidentite)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    return new DataTable();
                    //IEnumerable<object> query =
                    //    from DEVIS in context.DEVIS
                    //    where
                    //      DEVIS.FK_IDPIECEIDENTITE == pIdPieceDidentite
                    //    select new
                    //    {
                    //        DEVIS.PK_ID,
                    //        DEVIS.CODETAXE,
                    //        DEVIS.FK_IDETAPEDEVIS,
                    //        DEVIS.FK_IDCENTRE,
                    //        DEVIS.FK_IDPIECEIDENTITE,
                    //        DEVIS.FK_IDPRODUIT,
                    //        DEVIS.FK_IDSITE,
                    //        DEVIS.FK_IDTYPEDEVIS,
                    //        DEVIS.NUMDEVIS,
                    //        DEVIS.CODECENTRE,
                    //        DEVIS.CODESITE,
                    //        DEVIS.CODEPRODUIT,
                    //        DEVIS.DATEDECREATION,
                    //        DEVIS.DATEETAPE,
                    //        DEVIS.MONTANTHT,
                    //        DEVIS.MONTANTTTC,
                    //        DEVIS.MONTANTTOUTORDRE,
                    //        DEVIS.NUMEROCTR,
                    //        DEVIS.MOTIFREJET,
                    //        DEVIS.DATEREGLEMENT,
                    //        DEVIS.MATRICULECAISSE,
                    //        DEVIS.ORDRE,
                    //        DEVIS.IDSCHEMA,
                    //        DEVIS.ISFOURNITURE,
                    //        DEVIS.ISPOSE,
                    //        DEVIS.ISANALYSED,
                    //        DEVIS.PUISSANCESOUSCRITE,
                    //        DEVIS.IDTYPECTR,
                    //        DEVIS.IDMARQUECTR,
                    //        DEVIS.IDOWNERSHIP,
                    //        DEVIS.DATEFABRICATIONCTR,
                    //        DEVIS.DATEPOSECTR,
                    //        DEVIS.INDEXPOSECTR,
                    //        DEVIS.DISTANCE,
                    //        DEVIS.OWNERSHIPPROOFID,
                    //        DEVIS.NUMEROPIECEIDENTITE,
                    //        DEVIS.NUMEROGPS,
                    //        DEVIS.NEARESTROUTE,
                    //        DEVIS.ISBRACKET,
                    //        DEVIS.ISSERVICEPOLE,
                    //        DEVIS.ESTSIMPLIFIE,
                    //        DEVIS.ESTCOMPLET,
                    //        DEVIS.ISSUBVENTION,
                    //        DEVIS.ISEXTENSION,
                    //        DEVIS.IDMANUSCRIT,
                    //        DEVIS.EMPLACEMENTCOMPTEUR,
                    //        DEVIS.MONTANTCOMPLEMENTAIRE,
                    //        DEVIS.MONTANTPARTICIPATION,
                    //        DEVIS.FRAISPOSE,
                    //        DEVIS.ISPARTICIPATION,
                    //        DEVIS.USERCREATION,
                    //        DEVIS.USERMODIFICATION,
                    //        DEVIS.DATECREATION,
                    //        DEVIS.DATEMODIFICATION
                    //    };
                    //return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable DEVIS_DEVIS_RETOURNEByCodeProduit(int pIdProduit)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    return new DataTable();
                    //IEnumerable<object> query =
                    //    from DEVIS in context.DEVIS
                    //    where
                    //      DEVIS.FK_IDPRODUIT == pIdProduit
                    //    select new
                    //    {
                    //        DEVIS.PK_ID,
                    //        DEVIS.CODETAXE,
                    //        DEVIS.FK_IDETAPEDEVIS,
                    //        DEVIS.FK_IDCENTRE,
                    //        DEVIS.FK_IDPIECEIDENTITE,
                    //        DEVIS.FK_IDPRODUIT,
                    //        DEVIS.FK_IDSITE,
                    //        DEVIS.FK_IDTYPEDEVIS,
                    //        DEVIS.NUMDEVIS,
                    //        DEVIS.CODECENTRE,
                    //        DEVIS.CODESITE,
                    //        DEVIS.CODEPRODUIT,
                    //        DEVIS.DATEDECREATION,
                    //        DEVIS.DATEETAPE,
                    //        DEVIS.MONTANTHT,
                    //        DEVIS.MONTANTTTC,
                    //        DEVIS.MONTANTTOUTORDRE,
                    //        DEVIS.NUMEROCTR,
                    //        DEVIS.MOTIFREJET,
                    //        DEVIS.DATEREGLEMENT,
                    //        DEVIS.MATRICULECAISSE,
                    //        DEVIS.ORDRE,
                    //        DEVIS.IDSCHEMA,
                    //        DEVIS.ISFOURNITURE,
                    //        DEVIS.ISPOSE,
                    //        DEVIS.ISANALYSED,
                    //        DEVIS.PUISSANCESOUSCRITE,
                    //        DEVIS.IDTYPECTR,
                    //        DEVIS.IDMARQUECTR,
                    //        DEVIS.IDOWNERSHIP,
                    //        DEVIS.DATEFABRICATIONCTR,
                    //        DEVIS.DATEPOSECTR,
                    //        DEVIS.INDEXPOSECTR,
                    //        DEVIS.DISTANCE,
                    //        DEVIS.OWNERSHIPPROOFID,
                    //        DEVIS.NUMEROPIECEIDENTITE,
                    //        DEVIS.NUMEROGPS,
                    //        DEVIS.NEARESTROUTE,
                    //        DEVIS.ISBRACKET,
                    //        DEVIS.ISSERVICEPOLE,
                    //        DEVIS.ESTSIMPLIFIE,
                    //        DEVIS.ESTCOMPLET,
                    //        DEVIS.ISSUBVENTION,
                    //        DEVIS.ISEXTENSION,
                    //        DEVIS.IDMANUSCRIT,
                    //        DEVIS.EMPLACEMENTCOMPTEUR,
                    //        DEVIS.MONTANTCOMPLEMENTAIRE,
                    //        DEVIS.MONTANTPARTICIPATION,
                    //        DEVIS.FRAISPOSE,
                    //        DEVIS.ISPARTICIPATION,
                    //        DEVIS.USERCREATION,
                    //        DEVIS.USERMODIFICATION,
                    //        DEVIS.DATECREATION,
                    //        DEVIS.DATEMODIFICATION
                    //    };
                    //return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable DEVIS_DEVIS_RETOURNEBySite(int pSiteId)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    return new DataTable();
                    //IEnumerable<object> query =
                    //    from DEVIS in context.DEVIS
                    //    where
                    //      DEVIS.FK_IDSITE == pSiteId
                    //    select new
                    //    {
                    //        DEVIS.PK_ID,
                    //        DEVIS.CODETAXE,
                    //        DEVIS.FK_IDETAPEDEVIS,
                    //        DEVIS.FK_IDCENTRE,
                    //        DEVIS.FK_IDPIECEIDENTITE,
                    //        DEVIS.FK_IDPRODUIT,
                    //        DEVIS.FK_IDSITE,
                    //        DEVIS.FK_IDTYPEDEVIS,
                    //        DEVIS.NUMDEVIS,
                    //        DEVIS.CODECENTRE,
                    //        DEVIS.CODESITE,
                    //        DEVIS.CODEPRODUIT,
                    //        DEVIS.DATEDECREATION,
                    //        DEVIS.DATEETAPE,
                    //        DEVIS.MONTANTHT,
                    //        DEVIS.MONTANTTTC,
                    //        DEVIS.MONTANTTOUTORDRE,
                    //        DEVIS.NUMEROCTR,
                    //        DEVIS.MOTIFREJET,
                    //        DEVIS.DATEREGLEMENT,
                    //        DEVIS.MATRICULECAISSE,
                    //        DEVIS.ORDRE,
                    //        DEVIS.IDSCHEMA,
                    //        DEVIS.ISFOURNITURE,
                    //        DEVIS.ISPOSE,
                    //        DEVIS.ISANALYSED,
                    //        DEVIS.PUISSANCESOUSCRITE,
                    //        DEVIS.IDTYPECTR,
                    //        DEVIS.IDMARQUECTR,
                    //        DEVIS.IDOWNERSHIP,
                    //        DEVIS.DATEFABRICATIONCTR,
                    //        DEVIS.DATEPOSECTR,
                    //        DEVIS.INDEXPOSECTR,
                    //        DEVIS.DISTANCE,
                    //        DEVIS.OWNERSHIPPROOFID,
                    //        DEVIS.NUMEROPIECEIDENTITE,
                    //        DEVIS.NUMEROGPS,
                    //        DEVIS.NEARESTROUTE,
                    //        DEVIS.ISBRACKET,
                    //        DEVIS.ISSERVICEPOLE,
                    //        DEVIS.ESTSIMPLIFIE,
                    //        DEVIS.ESTCOMPLET,
                    //        DEVIS.ISSUBVENTION,
                    //        DEVIS.ISEXTENSION,
                    //        DEVIS.IDMANUSCRIT,
                    //        DEVIS.EMPLACEMENTCOMPTEUR,
                    //        DEVIS.MONTANTCOMPLEMENTAIRE,
                    //        DEVIS.MONTANTPARTICIPATION,
                    //        DEVIS.FRAISPOSE,
                    //        DEVIS.ISPARTICIPATION,
                    //        DEVIS.USERCREATION,
                    //        DEVIS.USERMODIFICATION,
                    //        DEVIS.DATECREATION,
                    //        DEVIS.DATEMODIFICATION
                    //    };
                    //return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable DEVIS_DEVIS_RETOURNEByIdTypeDevis(int pIdTypeDevis)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    return new DataTable();
                    //IEnumerable<object> query =
                    //    from DEVIS in context.DEVIS
                    //    where
                    //      DEVIS.FK_IDTYPEDEVIS == pIdTypeDevis
                    //    select new
                    //    {
                    //        DEVIS.PK_ID,
                    //        DEVIS.CODETAXE,
                    //        DEVIS.FK_IDETAPEDEVIS,
                    //        DEVIS.FK_IDCENTRE,
                    //        DEVIS.FK_IDPIECEIDENTITE,
                    //        DEVIS.FK_IDPRODUIT,
                    //        DEVIS.FK_IDSITE,
                    //        DEVIS.FK_IDTYPEDEVIS,
                    //        DEVIS.NUMDEVIS,
                    //        DEVIS.CODECENTRE,
                    //        DEVIS.CODESITE,
                    //        DEVIS.CODEPRODUIT,
                    //        DEVIS.DATEDECREATION,
                    //        DEVIS.DATEETAPE,
                    //        DEVIS.MONTANTHT,
                    //        DEVIS.MONTANTTTC,
                    //        DEVIS.MONTANTTOUTORDRE,
                    //        DEVIS.NUMEROCTR,
                    //        DEVIS.MOTIFREJET,
                    //        DEVIS.DATEREGLEMENT,
                    //        DEVIS.MATRICULECAISSE,
                    //        DEVIS.ORDRE,
                    //        DEVIS.IDSCHEMA,
                    //        DEVIS.ISFOURNITURE,
                    //        DEVIS.ISPOSE,
                    //        DEVIS.ISANALYSED,
                    //        DEVIS.PUISSANCESOUSCRITE,
                    //        DEVIS.IDTYPECTR,
                    //        DEVIS.IDMARQUECTR,
                    //        DEVIS.IDOWNERSHIP,
                    //        DEVIS.DATEFABRICATIONCTR,
                    //        DEVIS.DATEPOSECTR,
                    //        DEVIS.INDEXPOSECTR,
                    //        DEVIS.DISTANCE,
                    //        DEVIS.OWNERSHIPPROOFID,
                    //        DEVIS.NUMEROPIECEIDENTITE,
                    //        DEVIS.NUMEROGPS,
                    //        DEVIS.NEARESTROUTE,
                    //        DEVIS.ISBRACKET,
                    //        DEVIS.ISSERVICEPOLE,
                    //        DEVIS.ESTSIMPLIFIE,
                    //        DEVIS.ESTCOMPLET,
                    //        DEVIS.ISSUBVENTION,
                    //        DEVIS.ISEXTENSION,
                    //        DEVIS.IDMANUSCRIT,
                    //        DEVIS.EMPLACEMENTCOMPTEUR,
                    //        DEVIS.MONTANTCOMPLEMENTAIRE,
                    //        DEVIS.MONTANTPARTICIPATION,
                    //        DEVIS.FRAISPOSE,
                    //        DEVIS.ISPARTICIPATION,
                    //        DEVIS.USERCREATION,
                    //        DEVIS.USERMODIFICATION,
                    //        DEVIS.DATECREATION,
                    //        DEVIS.DATEMODIFICATION
                    //    };
                    //return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable DEVIS_DEVIS_RETOURNEByIsAnalysed(bool isAnalysed)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    return new DataTable();
                    //IEnumerable<object> query =
                    //    from DEVIS in context.DEVIS
                    //    where
                    //      DEVIS.ISANALYSED == isAnalysed
                    //    select new
                    //    {
                    //        DEVIS.PK_ID,
                    //        DEVIS.CODETAXE,
                    //        DEVIS.FK_IDETAPEDEVIS,
                    //        DEVIS.FK_IDCENTRE,
                    //        DEVIS.FK_IDPIECEIDENTITE,
                    //        DEVIS.FK_IDPRODUIT,
                    //        DEVIS.FK_IDSITE,
                    //        DEVIS.FK_IDTYPEDEVIS,
                    //        DEVIS.NUMDEVIS,
                    //        DEVIS.CODECENTRE,
                    //        DEVIS.CODESITE,
                    //        DEVIS.CODEPRODUIT,
                    //        DEVIS.DATEDECREATION,
                    //        DEVIS.DATEETAPE,
                    //        DEVIS.MONTANTHT,
                    //        DEVIS.MONTANTTTC,
                    //        DEVIS.MONTANTTOUTORDRE,
                    //        DEVIS.NUMEROCTR,
                    //        DEVIS.MOTIFREJET,
                    //        DEVIS.DATEREGLEMENT,
                    //        DEVIS.MATRICULECAISSE,
                    //        DEVIS.ORDRE,
                    //        DEVIS.IDSCHEMA,
                    //        DEVIS.ISFOURNITURE,
                    //        DEVIS.ISPOSE,
                    //        DEVIS.ISANALYSED,
                    //        DEVIS.PUISSANCESOUSCRITE,
                    //        DEVIS.IDTYPECTR,
                    //        DEVIS.IDMARQUECTR,
                    //        DEVIS.IDOWNERSHIP,
                    //        DEVIS.DATEFABRICATIONCTR,
                    //        DEVIS.DATEPOSECTR,
                    //        DEVIS.INDEXPOSECTR,
                    //        DEVIS.DISTANCE,
                    //        DEVIS.OWNERSHIPPROOFID,
                    //        DEVIS.NUMEROPIECEIDENTITE,
                    //        DEVIS.NUMEROGPS,
                    //        DEVIS.NEARESTROUTE,
                    //        DEVIS.ISBRACKET,
                    //        DEVIS.ISSERVICEPOLE,
                    //        DEVIS.ESTSIMPLIFIE,
                    //        DEVIS.ESTCOMPLET,
                    //        DEVIS.ISSUBVENTION,
                    //        DEVIS.ISEXTENSION,
                    //        DEVIS.IDMANUSCRIT,
                    //        DEVIS.EMPLACEMENTCOMPTEUR,
                    //        DEVIS.MONTANTCOMPLEMENTAIRE,
                    //        DEVIS.MONTANTPARTICIPATION,
                    //        DEVIS.FRAISPOSE,
                    //        DEVIS.ISPARTICIPATION,
                    //        DEVIS.USERCREATION,
                    //        DEVIS.USERMODIFICATION,
                    //        DEVIS.DATECREATION,
                    //        DEVIS.DATEMODIFICATION
                    //    };
                    //return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable DEVIS_DEVIS_RETOURNEByCodeProduitIdTypeDevis(int pIdProduit, int pIdTypeDevis)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    return new DataTable();
                    //IEnumerable<object> query =
                    //    from DEVIS in context.DEVIS
                    //    where
                    //      DEVIS.FK_IDPRODUIT == pIdProduit &&
                    //      DEVIS.FK_IDTYPEDEVIS == pIdTypeDevis
                    //    select new
                    //    {
                    //        DEVIS.PK_ID,
                    //        DEVIS.CODETAXE,
                    //        DEVIS.FK_IDETAPEDEVIS,
                    //        DEVIS.FK_IDCENTRE,
                    //        DEVIS.FK_IDPIECEIDENTITE,
                    //        DEVIS.FK_IDPRODUIT,
                    //        DEVIS.FK_IDSITE,
                    //        DEVIS.FK_IDTYPEDEVIS,
                    //        DEVIS.NUMDEVIS,
                    //        DEVIS.CODECENTRE,
                    //        DEVIS.CODESITE,
                    //        DEVIS.CODEPRODUIT,
                    //        DEVIS.DATEDECREATION,
                    //        DEVIS.DATEETAPE,
                    //        DEVIS.MONTANTHT,
                    //        DEVIS.MONTANTTTC,
                    //        DEVIS.MONTANTTOUTORDRE,
                    //        DEVIS.NUMEROCTR,
                    //        DEVIS.MOTIFREJET,
                    //        DEVIS.DATEREGLEMENT,
                    //        DEVIS.MATRICULECAISSE,
                    //        DEVIS.ORDRE,
                    //        DEVIS.IDSCHEMA,
                    //        DEVIS.ISFOURNITURE,
                    //        DEVIS.ISPOSE,
                    //        DEVIS.ISANALYSED,
                    //        DEVIS.PUISSANCESOUSCRITE,
                    //        DEVIS.IDTYPECTR,
                    //        DEVIS.IDMARQUECTR,
                    //        DEVIS.IDOWNERSHIP,
                    //        DEVIS.DATEFABRICATIONCTR,
                    //        DEVIS.DATEPOSECTR,
                    //        DEVIS.INDEXPOSECTR,
                    //        DEVIS.DISTANCE,
                    //        DEVIS.OWNERSHIPPROOFID,
                    //        DEVIS.NUMEROPIECEIDENTITE,
                    //        DEVIS.NUMEROGPS,
                    //        DEVIS.NEARESTROUTE,
                    //        DEVIS.ISBRACKET,
                    //        DEVIS.ISSERVICEPOLE,
                    //        DEVIS.ESTSIMPLIFIE,
                    //        DEVIS.ESTCOMPLET,
                    //        DEVIS.ISSUBVENTION,
                    //        DEVIS.ISEXTENSION,
                    //        DEVIS.IDMANUSCRIT,
                    //        DEVIS.EMPLACEMENTCOMPTEUR,
                    //        DEVIS.MONTANTCOMPLEMENTAIRE,
                    //        DEVIS.MONTANTPARTICIPATION,
                    //        DEVIS.FRAISPOSE,
                    //        DEVIS.ISPARTICIPATION,
                    //        DEVIS.USERCREATION,
                    //        DEVIS.USERMODIFICATION,
                    //        DEVIS.DATECREATION,
                    //        DEVIS.DATEMODIFICATION
                    //    };
                    //return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable DEVIS_DEVIS_RETOURNEById(int pIdDevis)
        {
            try
            {
                return new DataTable();
                //using (galadbEntities context = new galadbEntities())
                //{
                //        IEnumerable<object> query =
                //        from DEVIS in context.DEVIS
                //        join PIE in context.PIECEIDENTITE on DEVIS.FK_IDPIECEIDENTITE equals PIE.PK_ID
                //        join Marque in context.MARQUECOMPTEUR on DEVIS.IDMARQUECTR equals Marque.CODE into Marque_join
                //        from Marque in Marque_join.DefaultIfEmpty()
                //        join TypeCompteur in context.TYPECOMPTEUR   on DEVIS.IDTYPECTR equals TypeCompteur.CODE  into TypeCompteur_join
                //        from TypeCompteur in TypeCompteur_join.DefaultIfEmpty()
                //        join DEP in context.DEVISPIA on DEVIS.PK_ID equals DEP.FK_IDDEVIS into DEP_join
                //        from DEP in DEP_join.DefaultIfEmpty()
                //        where
                //          DEVIS.PK_ID == pIdDevis
                //        select new
                //        {
                //            DEVIS.PK_ID,
                //            DEVIS.CODETAXE,
                //            DEVIS.FK_IDETAPEDEVIS,
                //            DEVIS.FK_IDCENTRE,
                //            DEVIS.FK_IDPIECEIDENTITE,
                //            DEVIS.FK_IDPRODUIT,
                //            DEVIS.FK_IDSITE,
                //            DEVIS.FK_IDTYPEDEVIS,
                //            DEVIS.NUMDEVIS,
                //            DEVIS.CODECENTRE,
                //            DEVIS.CODESITE,
                //            DEVIS.CODEPRODUIT,
                //            DEVIS.DATEDECREATION,
                //            DEVIS.DATEETAPE,
                //            DEVIS.MONTANTHT,
                //            DEVIS.MONTANTTTC,
                //            DEVIS.MONTANTTOUTORDRE,
                //            DEVIS.NUMEROCTR,
                //            DEVIS.MOTIFREJET,
                //            DEVIS.DATEREGLEMENT,
                //            DEVIS.MATRICULECAISSE,
                //            DEVIS.ORDRE,
                //            DEVIS.IDSCHEMA,
                //            DEVIS.ISFOURNITURE,
                //            DEVIS.ISPOSE,
                //            DEVIS.ISANALYSED,
                //            DEVIS.PUISSANCESOUSCRITE,
                //            DEVIS.IDTYPECTR,
                //            DEVIS.IDMARQUECTR,
                //            DEVIS.IDOWNERSHIP,
                //            DEVIS.DATEFABRICATIONCTR,
                //            DEVIS.DATEPOSECTR,
                //            DEVIS.INDEXPOSECTR,
                //            DEVIS.DISTANCE,
                //            DEVIS.OWNERSHIPPROOFID,
                //            DEVIS.NUMEROPIECEIDENTITE,
                //            DEVIS.NUMEROGPS,
                //            DEVIS.NEARESTROUTE,
                //            DEVIS.ISBRACKET,
                //            DEVIS.ISSERVICEPOLE,
                //            DEVIS.ESTSIMPLIFIE,
                //            DEVIS.ESTCOMPLET,
                //            DEVIS.ISSUBVENTION,
                //            DEVIS.ISEXTENSION,
                //            DEVIS.IDMANUSCRIT,
                //            DEVIS.EMPLACEMENTCOMPTEUR,
                //            DEVIS.MONTANTCOMPLEMENTAIRE,
                //            DEVIS.MONTANTPARTICIPATION,
                //            DEVIS.FRAISPOSE,
                //            DEVIS.ISPARTICIPATION,
                //            DEVIS.USERCREATION,
                //            DEVIS.USERMODIFICATION,
                //            DEVIS.DATECREATION,
                //            DEVIS.DATEMODIFICATION,
                //            LIBELLECENTRE = DEVIS.CENTRE.LIBELLE,
                //            LIBELLETYPEDEMANDE = DEVIS.TYPEDEVIS.LIBELLE,
                //            LIBELLEPRODUIT = DEVIS.PRODUIT.LIBELLE,
                //            LIBELLETYPEPIECE = PIE.LIBELLE,
                //            LIBELLESITE = DEVIS.SITE.LIBELLE,
                //            LIBELLETACHE = DEVIS.ETAPEDEVIS.TACHEDEVIS.LIBELLE,
                //            MATRICULEPIA = DEP.MATRICULEPIA,
                //            LIBELLEMARQUECOMPTEUR = Marque.LIBELLE,
                //            LIBELLETYPECOMPTEUR = TypeCompteur.LIBELLE
                //        };
                //    return Galatee.Tools.Utility.ListToDataTable(query);
                //}
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable DEVIS_DEVIS_RETOURNEByNumDevis(string pNumDevis)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    return new DataTable();
                    //IEnumerable<object> query =
                    //    from DEVIS in context.DEVIS
                    //    join PIE in context.PIECEIDENTITE on DEVIS.FK_IDPIECEIDENTITE equals PIE.PK_ID
                    //    join Marque in context.MARQUECOMPTEUR on DEVIS.IDMARQUECTR equals Marque.CODE into Marque_join
                    //    from Marque in Marque_join.DefaultIfEmpty()
                    //    join TypeCompteur in context.TYPECOMPTEUR   on DEVIS.IDTYPECTR equals TypeCompteur.CODE  into TypeCompteur_join
                    //    from TypeCompteur in TypeCompteur_join.DefaultIfEmpty()
                    //    join DEP in context.DEVISPIA on DEVIS.PK_ID equals DEP.FK_IDDEVIS into DEP_join
                    //    from DEP in DEP_join.DefaultIfEmpty()
                    //    where
                    //      DEVIS.NUMDEVIS == pNumDevis
                    //    select new
                    //    {
                    //        DEVIS.PK_ID,
                    //        DEVIS.CODETAXE,
                    //        DEVIS.FK_IDETAPEDEVIS,
                    //        DEVIS.FK_IDCENTRE,
                    //        DEVIS.FK_IDPIECEIDENTITE,
                    //        DEVIS.FK_IDPRODUIT,
                    //        DEVIS.FK_IDSITE,
                    //        DEVIS.FK_IDTYPEDEVIS,
                    //        DEVIS.NUMDEVIS,
                    //        DEVIS.CODECENTRE,
                    //        DEVIS.CODESITE,
                    //        DEVIS.CODEPRODUIT,
                    //        DEVIS.DATEDECREATION,
                    //        DEVIS.DATEETAPE,
                    //        DEVIS.MONTANTHT,
                    //        DEVIS.MONTANTTTC,
                    //        DEVIS.MONTANTTOUTORDRE,
                    //        DEVIS.NUMEROCTR,
                    //        DEVIS.MOTIFREJET,
                    //        DEVIS.DATEREGLEMENT,
                    //        DEVIS.MATRICULECAISSE,
                    //        DEVIS.ORDRE,
                    //        DEVIS.IDSCHEMA,
                    //        DEVIS.ISFOURNITURE,
                    //        DEVIS.ISPOSE,
                    //        DEVIS.ISANALYSED,
                    //        DEVIS.PUISSANCESOUSCRITE,
                    //        DEVIS.IDTYPECTR,
                    //        DEVIS.IDMARQUECTR,
                    //        DEVIS.IDOWNERSHIP,
                    //        DEVIS.DATEFABRICATIONCTR,
                    //        DEVIS.DATEPOSECTR,
                    //        DEVIS.INDEXPOSECTR,
                    //        DEVIS.DISTANCE,
                    //        DEVIS.OWNERSHIPPROOFID,
                    //        DEVIS.NUMEROPIECEIDENTITE,
                    //        DEVIS.NUMEROGPS,
                    //        DEVIS.NEARESTROUTE,
                    //        DEVIS.ISBRACKET,
                    //        DEVIS.ISSERVICEPOLE,
                    //        DEVIS.ESTSIMPLIFIE,
                    //        DEVIS.ESTCOMPLET,
                    //        DEVIS.ISSUBVENTION,
                    //        DEVIS.ISEXTENSION,
                    //        DEVIS.IDMANUSCRIT,
                    //        DEVIS.EMPLACEMENTCOMPTEUR,
                    //        DEVIS.MONTANTCOMPLEMENTAIRE,
                    //        DEVIS.MONTANTPARTICIPATION,
                    //        DEVIS.FRAISPOSE,
                    //        DEVIS.ISPARTICIPATION,
                    //        DEVIS.USERCREATION,
                    //        DEVIS.USERMODIFICATION,
                    //        DEVIS.DATECREATION,
                    //        DEVIS.DATEMODIFICATION,
                    //        LIBELLECENTRE = DEVIS.CENTRE.LIBELLE,
                    //        LIBELLETYPEDEMANDE = DEVIS.TYPEDEVIS.LIBELLE,
                    //        LIBELLEPRODUIT = DEVIS.PRODUIT.LIBELLE,
                    //        LIBELLETYPEPIECE = PIE.LIBELLE,
                    //        LIBELLESITE = DEVIS.SITE.LIBELLE,
                    //        LIBELLETACHE =DEVIS.ETAPEDEVIS.TACHEDEVIS.LIBELLE,
                    //        MATRICULEPIA =DEP.MATRICULEPIA,
                    //        LIBELLEMARQUECOMPTEUR = Marque.LIBELLE,
                    //        LIBELLETYPECOMPTEUR = TypeCompteur.LIBELLE
                    //    };
                    //return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable DEVIS_DEVIS_RETOURNE_DATECREATION()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    return new DataTable();
                    //IEnumerable<object> query =
                    // (from D in context.DEVIS
                    //  orderby
                    //    D.DATEDECREATION
                    //  select new
                    //  {
                    //      Code = D.DATEDECREATION.Value.Year,
                    //  }).Distinct();

                    //return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable DEVIS_DEVIS_EditionDevisAEncCaisser()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    return new DataTable();
                    //IEnumerable<object> query =
                    //    from DEVIS in context.DEVIS
                    //    join etapeDevis in context.ETAPEDEVIS on DEVIS.FK_IDETAPEDEVIS equals etapeDevis.PK_ID
                    //    join typeDevis in context.TYPEDEVIS on DEVIS.FK_IDTYPEDEVIS equals typeDevis.PK_ID
                    //    join site in context.SITE on DEVIS.FK_IDSITE equals site.PK_ID 
                    //    join centre in context.CENTRE on DEVIS.FK_IDCENTRE equals centre.PK_ID
                    //    join produit in context.PRODUIT on DEVIS.FK_IDPRODUIT equals produit.PK_ID 
                    //    where
                    //      DEVIS.ETAPEDEVIS.NUMETAPE == (int)Enumere.EtapeDevis.Encaissement
                    //    select new
                    //    {
                    //        DEVIS.PK_ID,
                    //        DEVIS.CODETAXE,
                    //        DEVIS.FK_IDETAPEDEVIS,
                    //        DEVIS.FK_IDCENTRE,
                    //        DEVIS.FK_IDPIECEIDENTITE,
                    //        DEVIS.FK_IDPRODUIT,
                    //        DEVIS.FK_IDSITE,
                    //        DEVIS.FK_IDTYPEDEVIS,
                    //        DEVIS.NUMDEVIS,
                    //        DEVIS.CODECENTRE,
                    //        DEVIS.CODESITE,
                    //        DEVIS.CODEPRODUIT,
                    //        DEVIS.DATEDECREATION,
                    //        DEVIS.DATEETAPE,
                    //        DEVIS.MONTANTHT,
                    //        DEVIS.MONTANTTTC,
                    //        DEVIS.MONTANTTOUTORDRE,
                    //        DEVIS.NUMEROCTR,
                    //        DEVIS.MOTIFREJET,
                    //        DEVIS.DATEREGLEMENT,
                    //        DEVIS.MATRICULECAISSE,
                    //        DEVIS.ORDRE,
                    //        DEVIS.IDSCHEMA,
                    //        DEVIS.ISFOURNITURE,
                    //        DEVIS.ISPOSE,
                    //        DEVIS.ISANALYSED,
                    //        DEVIS.PUISSANCESOUSCRITE,
                    //        DEVIS.IDTYPECTR,
                    //        DEVIS.IDMARQUECTR,
                    //        DEVIS.IDOWNERSHIP,
                    //        DEVIS.DATEFABRICATIONCTR,
                    //        DEVIS.DATEPOSECTR,
                    //        DEVIS.INDEXPOSECTR,
                    //        DEVIS.DISTANCE,
                    //        DEVIS.OWNERSHIPPROOFID,
                    //        DEVIS.NUMEROPIECEIDENTITE,
                    //        DEVIS.NUMEROGPS,
                    //        DEVIS.NEARESTROUTE,
                    //        DEVIS.ISBRACKET,
                    //        DEVIS.ISSERVICEPOLE,
                    //        DEVIS.ESTSIMPLIFIE,
                    //        DEVIS.ESTCOMPLET,
                    //        DEVIS.ISSUBVENTION,
                    //        DEVIS.ISEXTENSION,
                    //        DEVIS.IDMANUSCRIT,
                    //        DEVIS.EMPLACEMENTCOMPTEUR,
                    //        DEVIS.MONTANTCOMPLEMENTAIRE,
                    //        DEVIS.MONTANTPARTICIPATION,
                    //        DEVIS.FRAISPOSE,
                    //        DEVIS.ISPARTICIPATION,
                    //        DEVIS.USERCREATION,
                    //        DEVIS.USERMODIFICATION,
                    //        DEVIS.DATECREATION,
                    //        DEVIS.DATEMODIFICATION,
                    //        LIBELLECENTRE = centre.LIBELLE,
                    //        LIBELLETYPEDEMANDE = typeDevis.LIBELLE,
                    //        LIBELLEPRODUIT = produit.LIBELLE,
                    //        LIBELLESITE = site.LIBELLE,
                    //        LIBELLETACHE = DEVIS.ETAPEDEVIS.TACHEDEVIS.LIBELLE,
                    //        DELAIEXECUTIONETAPE = DEVIS.ETAPEDEVIS.DELAIEXECUTIONETAPE
                    //    };
                    //return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable DEVIS_DEVIS_EditionDevisBanchementAFinaliser()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    return new DataTable();

                    //IEnumerable<object> query =
                    //    from demande in context.DEMANDE
                    //    join devis in context.DEVIS on new { Centre = demande.CENTRE , NumeroDevis = demande.NUMDEVIS } equals new { Centre = devis.CODECENTRE, NumeroDevis = devis.NUMDEVIS}
                    //    join deposit in context.DEPOSIT on devis.NUMDEVIS equals deposit.NUMDEVIS
                    //    join site in context.SITE on devis.FK_IDSITE equals site.PK_ID 
                    //    join centre in context.CENTRE on demande.FK_IDCENTRE  equals centre.PK_ID
                    //    join produit in context.PRODUIT on devis.FK_IDPRODUIT equals produit.PK_ID 
                    //    where
                    //      devis.ETAPEDEVIS.TACHEDEVIS.PK_ID == (int)Enumere.BranchementAFinaliser
                    //      && deposit.ISCREATED == false
                    //    select new
                    //    {
                    //        devis.PK_ID,
                    //        devis.CODETAXE,
                    //        devis.FK_IDETAPEDEVIS,
                    //        devis.FK_IDCENTRE,
                    //        devis.FK_IDPIECEIDENTITE,
                    //        devis.FK_IDPRODUIT,
                    //        devis.FK_IDSITE,
                    //        devis.FK_IDTYPEDEVIS,
                    //        devis.NUMDEVIS,
                    //        devis.CODECENTRE,
                    //        devis.CODESITE,
                    //        devis.CODEPRODUIT,
                    //        devis.DATEDECREATION,
                    //        devis.DATEETAPE,
                    //        devis.MONTANTHT,
                    //        devis.MONTANTTTC,
                    //        devis.MONTANTTOUTORDRE,
                    //        devis.NUMEROCTR,
                    //        devis.MOTIFREJET,
                    //        devis.DATEREGLEMENT,
                    //        devis.MATRICULECAISSE,
                    //        devis.ORDRE,
                    //        devis.IDSCHEMA,
                    //        devis.ISFOURNITURE,
                    //        devis.ISPOSE,
                    //        devis.ISANALYSED,
                    //        devis.PUISSANCESOUSCRITE,
                    //        devis.IDTYPECTR,
                    //        devis.IDMARQUECTR,
                    //        devis.IDOWNERSHIP,
                    //        devis.DATEFABRICATIONCTR,
                    //        devis.DATEPOSECTR,
                    //        devis.INDEXPOSECTR,
                    //        devis.DISTANCE,
                    //        devis.OWNERSHIPPROOFID,
                    //        devis.NUMEROPIECEIDENTITE,
                    //        devis.NUMEROGPS,
                    //        devis.NEARESTROUTE,
                    //        devis.ISBRACKET,
                    //        devis.ISSERVICEPOLE,
                    //        devis.ESTSIMPLIFIE,
                    //        devis.ESTCOMPLET,
                    //        devis.ISSUBVENTION,
                    //        devis.ISEXTENSION,
                    //        devis.IDMANUSCRIT,
                    //        devis.EMPLACEMENTCOMPTEUR,
                    //        devis.MONTANTCOMPLEMENTAIRE,
                    //        devis.MONTANTPARTICIPATION,
                    //        devis.FRAISPOSE,
                    //        devis.ISPARTICIPATION,
                    //        devis.USERCREATION,
                    //        devis.USERMODIFICATION,
                    //        devis.DATECREATION,
                    //        devis.DATEMODIFICATION,
                    //        LIBELLECENTRE =  centre.LIBELLE,
                    //        LIBELLETYPEDEMANDE = devis.TYPEDEVIS.LIBELLE,
                    //        LIBELLEPRODUIT = produit.LIBELLE,
                    //        LIBELLESITE = site.LIBELLE,
                    //        LIBELLETACHE = devis.ETAPEDEVIS.TACHEDEVIS.LIBELLE,
                    //        DELAIEXECUTIONETAPE = devis.ETAPEDEVIS.DELAIEXECUTIONETAPE,
                    //        //NOM = devis.DEMANDEDEVIS.NOM, 
                    //        DCAISSE = demande.DCAISSE,
                    //        DEMANDE = demande.NUMDEM,
                    //        NUMEROCLIENT = demande.CLIENT
                    //    };
                    //return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        #endregion

        #region DEVISPIA
        public static DataTable DEVIS_DEVISPIA_RETOURNEByDevisIdOrdre(int pIdDevis, int pOrdre)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    return new DataTable();

                    // IEnumerable<object> query =
                    //from D in context.DEVISPIA
                    //where
                    //  D.FK_IDDEVIS == pIdDevis &&
                    //  D.ORDRE  == pOrdre
                    //select new
                    //{
                    //    D.PK_ID,
                    //    D.FK_IDDEVIS,
                    //    D.FK_IDUSER,
                    //    D.NUMDEVIS,
                    //    D.ORDRE,
                    //    D.MATRICULEPIA,
                    //    D.DATEPIA,
                    //    NomMetreur = D.ADMUTILISATEUR.LIBELLE ,
                    //    D.USERCREATION,
                    //    D.USERMODIFICATION,
                    //    D.DATECREATION,
                    //    D.DATEMODIFICATION
                    //};

                    // return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable DEVIS_DEVISPIA_RETOURNEById(int pId)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    return new DataTable();
                    // IEnumerable<object> query =
                    //from D in context.DEVISPIA
                    //where
                    //  D.PK_ID == pId 
                    //select new
                    //{
                    //    D.PK_ID,
                    //    D.FK_IDDEVIS,
                    //    D.FK_IDUSER,
                    //    D.NUMDEVIS,
                    //    D.ORDRE,
                    //    D.MATRICULEPIA,
                    //    D.DATEPIA,
                    //    NomMetreur = D.ADMUTILISATEUR.LIBELLE,
                    //    D.USERCREATION,
                    //    D.USERMODIFICATION,
                    //    D.DATECREATION,
                    //    D.DATEMODIFICATION
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

        #region DOCUMENTSCANNE
        public static DataTable DEVIS_DOCUMENTSCANNE_RETOURNEById(Guid pId)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                  from DOCUMENTSCANNE in context.DOCUMENTSCANNE
                  where
                    DOCUMENTSCANNE.PK_ID == pId
                  select new
                  {
                      DOCUMENTSCANNE.PK_ID,
                      DOCUMENTSCANNE.NOMDOCUMENT,
                      DOCUMENTSCANNE.CONTENU,
                      DOCUMENTSCANNE.USERCREATION,
                      DOCUMENTSCANNE.USERMODIFICATION,
                      DOCUMENTSCANNE.DATECREATION,
                      DOCUMENTSCANNE.DATEMODIFICATION
                  };
                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static DataTable DEVIS_DOCUMENTSCANNE_RETOURNEByIdDemande(int fk_iddemande)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                  from DOCUMENTSCANNE in context.DOCUMENTSCANNE
                  where
                    DOCUMENTSCANNE.FK_IDDEMANDE == fk_iddemande
                  select new
                  {
                      DOCUMENTSCANNE.PK_ID,
                      DOCUMENTSCANNE.NOMDOCUMENT,
                      DOCUMENTSCANNE.FK_IDTYPEDOCUMENT,
                      DOCUMENTSCANNE.CONTENU,
                      DOCUMENTSCANNE.USERCREATION,
                      DOCUMENTSCANNE.USERMODIFICATION,
                      DOCUMENTSCANNE.DATECREATION,
                      DOCUMENTSCANNE.DATEMODIFICATION
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

        #region ELEMENTDEVIS
        public static DataTable DEVIS_ELEMENTDEVIS_SelByDevisId(int pIdDevis, int pOrdre, bool pIsSummary)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                  from e in context.ELEMENTDEVIS
                  where
                    e.FK_IDDEMANDE == pIdDevis &&
                    e.ORDRE == pOrdre
                  //&&
                  //e.FOURNITURE.ISSUMMARY != pIsSummary
                  select new
                  {
                      e.PK_ID,
                      e.NUMDEM,
                      e.FK_IDDEMANDE,
                      e.FK_IDFOURNITURE,
                      e.ORDRE,
                      e.QUANTITE,
                      e.QUANTITECONSOMMEE,
                      e.QUANTITEREMISENSTOCK,
                      e.TAXE,
                      REMISE = (System.Int64)((Int32?)e.QUANTITEREMISENSTOCK ?? (Int32?)0) == 0 ? 0 : 1,
                      e.ISEXTENSION,
                      e.ISFOURNITURE,
                      e.ISPOSE,
                      e.USERCREATION,
                      e.USERMODIFICATION,
                      e.DATECREATION,
                      e.DATEMODIFICATION,
                  };
                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable DEVIS_ELEMENTDEVIS_ValidationRemiseStock_SelByDevisId(int pIdDevis, int pOrdre, bool pIsSummary)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                  from e in context.ELEMENTDEVIS
                  where
                    e.FK_IDDEMANDE == pIdDevis &&
                    e.ORDRE == pOrdre
                  //&&
                  // e.FOURNITURE.ISSUMMARY == pIsSummary
                  select new
                  {
                      e.PK_ID,
                      e.NUMDEM,
                      e.FK_IDDEMANDE,
                      e.FK_IDFOURNITURE,
                      e.ORDRE,
                      //NUMFOURNITURE = e.FOURNITURE.CODE ,
                      //e.FOURNITURE.DESIGNATION,
                      //PRIX_UNITAIRE = (decimal?)e.FOURNITURE.COUTUNITAIRE_FOURNITURE,
                      e.QUANTITE,
                      e.QUANTITECONSOMMEE,
                      e.QUANTITEREMISENSTOCK,
                      //e.QUANTITEALIVRET,
                      //e.QUANTITELIVRET,
                      e.TAXE,
                      //MONTANT = (decimal?)(e.QUANTITE * e.FOURNITURE.COUTUNITAIRE_FOURNITURE ),
                      // MONTANTQTECONSOMMEE = (decimal?)(e.QUANTITECONSOMMEE * e.FOURNITURE.COUTUNITAIRE_FOURNITURE),
                      CONSOMME = e.QUANTITECONSOMMEE == 0 ? 0 : 1,
                      e.USERCREATION,
                      e.USERMODIFICATION,
                      e.DATECREATION,
                      e.DATEMODIFICATION
                  };
                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable DEVIS_ELEMENTDEVIS_Consomme_SelByDevisId(int pIdDevis, int pOrdre)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                     from e in context.ELEMENTDEVIS
                     where
                       e.FK_IDDEMANDE == pIdDevis &&
                       e.ORDRE == pOrdre
                     select new
                     {
                         e.PK_ID,
                         e.NUMDEM,
                         e.FK_IDDEMANDE,
                         e.FK_IDFOURNITURE,
                         e.ORDRE,
                         e.QUANTITE,
                         e.QUANTITECONSOMMEE,
                         e.QUANTITEREMISENSTOCK,
                         e.TAXE,
                         CONSOMME = e.QUANTITECONSOMMEE == 0 ? 0 : 1,
                         e.ISEXTENSION,
                         e.ISFOURNITURE,
                         e.ISPOSE,
                         e.USERCREATION,
                         e.USERMODIFICATION,
                         e.DATECREATION,
                         e.DATEMODIFICATION
                     };
                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable DEVIS_ELEMENTDEVIS_RetourneByDevisIdOrdreNumFourniture(int pIdDevis, int pOrdre, int pNumFourniture)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                     from e in context.ELEMENTDEVIS
                     where
                       e.FK_IDDEMANDE == pIdDevis &&
                       e.ORDRE == pOrdre
                     //&& e.NUMFOURNITURE  == pNumFourniture
                     select new
                     {
                         e.PK_ID,
                         e.NUMDEM,
                         e.FK_IDDEMANDE,
                         e.FK_IDFOURNITURE,
                         e.ORDRE,
                         //NUMFOURNITURE = e.FOURNITURE.CODE ,
                         //e.FOURNITURE.DESIGNATION,
                         //PRIX_UNITAIRE = (decimal?)e.FOURNITURE.COUTUNITAIRE_FOURNITURE,
                         e.QUANTITE,
                         e.QUANTITECONSOMMEE,
                         e.QUANTITEREMISENSTOCK,
                         //e.QUANTITEALIVRET,
                         //e.QUANTITELIVRET,
                         e.TAXE,
                         //MONTANT = (decimal?)(e.QUANTITE * e.FOURNITURE.COUTUNITAIRE_FOURNITURE),
                         //MONTANTQTECONSOMMEE = (decimal?)(e.QUANTITECONSOMMEE * e.FOURNITURE.COUTUNITAIRE_FOURNITURE),
                         CONSOMME = e.QUANTITECONSOMMEE == 0 ? 0 : 1,
                         e.USERCREATION,
                         e.USERMODIFICATION,
                         e.DATECREATION,
                         e.DATEMODIFICATION
                     };
                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable DEVIS_ELEMENTDEVIS_RetourneByDevisIdOrdreFournitureId(int pIdDevis, int pOrdre, int pFournitureId)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                     from e in context.ELEMENTDEVIS
                     where
                       e.FK_IDDEMANDE == pIdDevis &&
                       e.ORDRE == pOrdre
                       && e.FK_IDFOURNITURE == pFournitureId
                     select new
                     {
                         e.PK_ID,
                         e.NUMDEM,
                         e.FK_IDDEMANDE,
                         e.FK_IDFOURNITURE,
                         e.ORDRE,
                         //NUMFOURNITURE = e.FOURNITURE.CODE ,
                         //e.FOURNITURE.DESIGNATION,
                         //PRIX_UNITAIRE = (decimal?)e.FOURNITURE.COUTUNITAIRE_FOURNITURE,
                         e.QUANTITE,
                         e.QUANTITECONSOMMEE,
                         e.QUANTITEREMISENSTOCK,
                         //e.QUANTITEALIVRET,
                         //e.QUANTITELIVRET,
                         e.TAXE,
                         //MONTANT = (decimal?)(e.QUANTITE * e.FOURNITURE.COUTUNITAIRE_FOURNITURE),
                         //MONTANTQTECONSOMMEE = (decimal?)(e.QUANTITECONSOMMEE * e.FOURNITURE.COUTUNITAIRE_FOURNITURE),
                         CONSOMME = e.QUANTITECONSOMMEE == 0 ? 0 : 1,
                         e.USERCREATION,
                         e.USERMODIFICATION,
                         e.DATECREATION,
                         e.DATEMODIFICATION
                     };
                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static DataTable DEVIS_ELEMENTDEVIS_MaterielByDevisById(int pIdDevis)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                                  from e in context.ELEMENTDEVIS
                                  where
                                    e.FK_IDDEMANDE == pIdDevis && e.COPER.CODE == Enumere.CoperTRV
                                  select new
                                  {
                                      e.PK_ID,
                                      e.NUMDEM,
                                      e.FK_IDTYPEDEMANDE,
                                      e.FK_IDDEMANDE,
                                      e.FK_IDMATERIELDEVIS,
                                      e.ORDRE,
                                      MATERIELDEVIS = e.MATERIELDEVIS.CODE,
                                      DESIGNATION = e.MATERIELDEVIS.LIBELLE,
                                      e.MATERIELDEVIS.LIBELLE,
                                      PRIX_UNITAIRE = (decimal?)((e.COUTUNITAIRE_FOURNITURE != null ? e.COUTUNITAIRE_FOURNITURE : 0) +
                                                                 (e.COUTUNITAIRE_POSE != null ? e.COUTUNITAIRE_POSE : 0)),
                                      e.QUANTITE,
                                      e.TAXE,
                                      MONTANT = e.MONTANTTTC,
                                      e.ISEXTENSION,
                                      e.ISPOSE,
                                      e.ISFOURNITURE,
                                      e.USERCREATION,
                                      e.USERMODIFICATION,
                                      e.DATECREATION,
                                      e.DATEMODIFICATION,
                                      COUT = e.MONTANTTTC,
                                      MONTANTTTC = e.MONTANTTTC,
                                      MONTANTHT = e.MONTANTHT,
                                      MONTANTTAXE = e.MONTANTTAXE,
                                      e.FK_IDTAXE,
                                      e.FK_IDCOPER,
                                      e.ISPRESTATION,
                                      e.ISPM,
                                      e.NOM,
                                      e.COUTUNITAIRE_FOURNITURE,
                                      e.COUTUNITAIRE_POSE,
                                      CODECOPER = e.COPER.CODE,
                                      TAUXTAXE = e.TAXE1.TAUX,
                                      e.RUBRIQUE,
                                      e.FK_IDRUBRIQUEDEVIS,
                                      e.MATERIELDEVIS.FK_IDTYPEMATERIEL
                                  };
                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static DataTable DEVIS_ELEMENTDEVISCoutDemande_SelByDevisById(int pIdDevis)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query1 =
                    from e in context.ELEMENTDEVIS
                    where
                      e.FK_IDDEMANDE == pIdDevis && e.COPER.CODE != Enumere.CoperTRV
                    select new
                    {
                        e.PK_ID,
                        e.NUMDEM,
                        e.FK_IDDEMANDE,
                        e.FK_IDFOURNITURE,
                        e.ORDRE,
                        e.NUMFOURNITURE,
                        DESIGNATION = e.COPER.LIBELLE,
                        LIBELLE = e.COPER.LIBELLE,
                        PRIX_UNITAIRE = (decimal?)((e.COUTDEMANDE.MONTANT != null ? e.COUTDEMANDE.MONTANT : 0)),
                        e.QUANTITE,
                        e.QUANTITECONSOMMEE,
                        e.QUANTITEREMISENSTOCK,
                        e.TAXE,
                        ISSUMMARY = false,
                        ISADDITIONAL = false,
                        ISEXTENSION = false,
                        ISDEFAULT = true,
                        e.USERCREATION,
                        e.USERMODIFICATION,
                        e.DATECREATION,
                        e.DATEMODIFICATION,
                        PRIX = e.MONTANTHT,
                        COUT = e.MONTANTHT,
                        e.MONTANTTTC,
                        e.MONTANTHT,
                        e.MONTANTTAXE,
                        e.ISFOURNITURE,
                        e.ISPOSE,
                        e.ISPRESTATION,
                        e.FK_IDTAXE,
                        e.FK_IDCOPER,
                        CODECOPER = e.COPER.CODE,
                        TAUXTAXE = e.TAXE1.TAUX
                    };
                    return Galatee.Tools.Utility.ListToDataTable(query1);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static DataTable DEVIS_ELEMENTDEVIS_SelByDevisById(int pIdDevis)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query1 =
                   from e in context.ELEMENTDEVIS
                   where
                     e.FK_IDDEMANDE == pIdDevis && e.COPER.CODE != Enumere.CoperTRV
                   select new
                   {
                       e.PK_ID,
                       e.NUMDEM,
                       e.FK_IDDEMANDE,
                       e.FK_IDFOURNITURE,
                       e.ORDRE,
                       e.NUMFOURNITURE,
                       DESIGNATION = e.COPER.LIBELLE,
                       PRIX_UNITAIRE = e.MONTANT,
                       e.QUANTITE,
                       e.QUANTITECONSOMMEE,
                       e.QUANTITEREMISENSTOCK,
                       //e.QUANTITEALIVRET,
                       //e.QUANTITELIVRET,
                       e.TAXE,
                       e.MONTANT,
                       MONTANTAREMBOURSER = 0,
                       ISSUMMARY = true,
                       ISADDITIONAL = true,
                       ISEXTENSION = false,
                       ISDEFAULT = false,
                       e.USERCREATION,
                       e.USERMODIFICATION,
                       e.DATECREATION,
                       e.DATEMODIFICATION,
                       PRIX = e.MONTANT,
                       COUT = e.MONTANT,
                       e.FK_IDTAXE,
                       e.FK_IDCOPER
                   };
                    return Galatee.Tools.Utility.ListToDataTable(query1);
                    //return Galatee.Tools.Utility.ListToDataTable(query.Union(query1));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region FOURNITURE

        public static DataTable DEVIS_FOURNITURE_RETOURNEByCodeProduitByIdTypeDevisDiametre(int pIdTypeDevis, int pProduitId, string pDiametre)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = null;
                    if (pDiametre == null)
                    {
                        query = from x in context.FOURNITURE
                                where
                                  x.FK_IDTYPEDEMANDE == pIdTypeDevis &&
                                  x.FK_IDPRODUIT == pProduitId
                                select new
                                {
                                    x.MATERIELDEVIS.CODE,
                                    x.FK_IDTYPEDEMANDE,
                                    CODEPRODUIT = x.PRODUIT.CODE,
                                    x.MATERIELDEVIS.COUTUNITAIRE_FOURNITURE,
                                    x.MATERIELDEVIS.COUTUNITAIRE_POSE,
                                    DESIGNATION = x.MATERIELDEVIS.LIBELLE,
                                    x.REGLAGECOMPTEUR,
                                    x.QUANTITY,
                                    x.ISSUMMARY,
                                    x.ISADDITIONAL,
                                    x.ISEXTENSION,
                                    x.ISDEFAULT,
                                    x.DATECREATION,
                                    x.DATEMODIFICATION,
                                    x.USERCREATION,
                                    x.USERMODIFICATION,
                                    x.PK_ID,
                                    x.FK_IDPRODUIT,
                                    x.MATERIELDEVIS.ISDISTANCE,
                                    LIBELLEPRODUIT = x.PRODUIT.LIBELLE,
                                    //LIBELLETYPEDEMANDE = x.TYPEDEMANDE.LIBELLE 
                                };
                    }
                    else
                    {
                        query = from x in context.FOURNITURE
                                where
                                  x.FK_IDTYPEDEMANDE == pIdTypeDevis &&
                                  x.FK_IDPRODUIT == pProduitId ||
                                  x.REGLAGECOMPTEUR == pDiametre
                                select new
                                {
                                    x.MATERIELDEVIS.CODE,
                                    x.FK_IDTYPEDEMANDE,
                                    CODEPRODUIT = x.PRODUIT.CODE,
                                    x.MATERIELDEVIS.COUTUNITAIRE_FOURNITURE,
                                    x.MATERIELDEVIS.COUTUNITAIRE_POSE,
                                    DESIGNATION = x.MATERIELDEVIS.LIBELLE,
                                    x.REGLAGECOMPTEUR,
                                    x.QUANTITY,
                                    x.ISSUMMARY,
                                    x.ISADDITIONAL,
                                    x.ISEXTENSION,
                                    x.ISDEFAULT,
                                    x.DATECREATION,
                                    x.DATEMODIFICATION,
                                    x.USERCREATION,
                                    x.USERMODIFICATION,
                                    x.PK_ID,
                                    x.FK_IDPRODUIT,
                                    x.MATERIELDEVIS.ISDISTANCE,
                                    LIBELLEPRODUIT = x.PRODUIT.LIBELLE,
                                    //LIBELLETYPEDEMANDE = x.TYPEDEMANDE.LIBELLE 
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

        public static DataTable DEVIS_FOURNITURE_RETOURNE()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                  from x in context.FOURNITURE
                  select new
                  {
                      x.MATERIELDEVIS.CODE,
                      x.FK_IDTYPEDEMANDE,
                      x.FK_IDMATERIELDEVIS,
                      CODEPRODUIT = x.PRODUIT.CODE,
                      x.MATERIELDEVIS.COUTUNITAIRE_FOURNITURE,
                      x.MATERIELDEVIS.COUTUNITAIRE_POSE,
                      x.MATERIELDEVIS.LIBELLE,
                      x.REGLAGECOMPTEUR,
                      x.QUANTITY,
                      x.ISSUMMARY,
                      x.ISADDITIONAL,
                      x.ISEXTENSION,
                      x.ISDEFAULT,
                      x.DATECREATION,
                      x.DATEMODIFICATION,
                      x.USERCREATION,
                      x.USERMODIFICATION,
                      x.PK_ID,
                      x.FK_IDPRODUIT,
                      x.MATERIELDEVIS.ISDISTANCE,
                      LIBELLEPRODUIT = x.PRODUIT.LIBELLE,
                      //LIBELLETYPEDEMANDE = x.TYPEDEMANDE.LIBELLE 
                  };
                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable DEVIS_FOURNITURE_RETOURNEByNumFourniture(int pNumFourniture)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                  from x in context.FOURNITURE
                  //where
                  //  x.CODE  == pNumFourniture
                  select new
                  {
                      //x.CODE,
                      x.FK_IDTYPEDEMANDE,
                      x.PRODUIT,
                      //x.COUTUNITAIRE_FOURNITURE,
                      //x.COUTUNITAIRE_POSE,
                      //x.DESIGNATION,
                      x.REGLAGECOMPTEUR,
                      x.QUANTITY,
                      x.ISSUMMARY,
                      x.ISADDITIONAL,
                      x.ISEXTENSION,
                      x.ISDEFAULT,
                      x.DATECREATION,
                      x.DATEMODIFICATION,
                      x.USERCREATION,
                      x.USERMODIFICATION,
                      x.PK_ID,
                      x.FK_IDPRODUIT
                  };
                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable DEVIS_FOURNITURE_RETOURNEByIdTypeDevis(int pIdTypeDevis)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                  from x in context.FOURNITURE
                  where
                    x.FK_IDTYPEDEMANDE == pIdTypeDevis
                  select new
                  {
                      //x.CODE,
                      x.FK_IDTYPEDEMANDE,
                      x.PRODUIT,
                      //x.COUTUNITAIRE_FOURNITURE,
                      //x.COUTUNITAIRE_POSE,
                      //x.DESIGNATION,
                      x.REGLAGECOMPTEUR,
                      x.QUANTITY,
                      x.ISSUMMARY,
                      x.ISADDITIONAL,
                      x.ISEXTENSION,
                      x.ISDEFAULT,
                      x.DATECREATION,
                      x.DATEMODIFICATION,
                      x.USERCREATION,
                      x.USERMODIFICATION,
                      x.PK_ID,
                      x.FK_IDPRODUIT
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

        #region INTERVENTIONPLANNIFIEE

        public static DataTable DEVIS_INTERVENTIONPLANNIFIEE_RETOURNE()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                  from f in context.INTERVENTIONPLANNIFIEE
                  select new
                  {
                      f.PK_ID,
                      f.DATERENDEZVOUS,
                      f.INTERVENTION,
                      f.MATRICULERESPONSABLE,
                      f.USERCREATION,
                      f.USERMODIFICATION,
                      f.DATECREATION,
                      f.DATEMODIFICATION
                  };
                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable INTERVENTIONPLANNIFIEE_GetByResponsable(string pResponsable)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                  from f in context.INTERVENTIONPLANNIFIEE
                  where
                    f.MATRICULERESPONSABLE == pResponsable
                  select new
                  {
                      f.PK_ID,
                      f.DATERENDEZVOUS,
                      f.INTERVENTION,
                      f.MATRICULERESPONSABLE,
                      f.USERCREATION,
                      f.USERMODIFICATION,
                      f.DATECREATION,
                      f.DATEMODIFICATION
                  };
                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable DEVIS_INTERVENTIONPLANNIFIEE_RETOURNEById(int pId)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                  from f in context.INTERVENTIONPLANNIFIEE
                  where
                    f.PK_ID == pId
                  select new
                  {
                      f.PK_ID,
                      f.DATERENDEZVOUS,
                      f.INTERVENTION,
                      f.MATRICULERESPONSABLE,
                      f.USERCREATION,
                      f.USERMODIFICATION,
                      f.DATECREATION,
                      f.DATEMODIFICATION
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

        #region SUIVIDEVIS
        public static DataTable DEVIS_SUIVIDEVIS_RETOURNEByDevisIdIdEtape(int pIdDevis, int pIdEtape)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    return new DataTable();

                    //  IEnumerable<object> query =
                    //from S in context.SUIVIDEVIS
                    //join M in context.ADMUTILISATEUR on new { Agent = S.MATRICULEAGENT  } equals new { Agent = M.MATRICULE  } into M_join
                    //from M in M_join.DefaultIfEmpty()
                    //where
                    //  S.FK_IDDEVIS == pIdDevis &&
                    //  S.FK_IDETAPEDEVIS  == pIdEtape
                    //select new
                    //{
                    //    S.PK_ID,
                    //    S.FK_IDDEVIS,
                    //    S.NUMDEVIS ,
                    //    S.FK_IDETAPEDEVIS,
                    //    S.DUREE,
                    //    S.MATRICULEAGENT ,
                    //    S.COMMENTAIRE,
                    //    NOMAGENT = M.LIBELLE,
                    //    LIBELLETACHE = S.ETAPEDEVIS.TACHEDEVIS.LIBELLE,
                    //    S.USERCREATION,
                    //    S.USERMODIFICATION,
                    //    S.DATECREATION,
                    //   S.DATEMODIFICATION
                    //};
                    //  return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable DEVIS_SUIVIDEVIS_RETOURNEByDevisIdAgent(int pIdDevis, string pMatriculeAgent)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    return new DataTable();

                    //  IEnumerable<object> query =
                    //from S in context.SUIVIDEVIS
                    //join M in context.ADMUTILISATEUR on new { Agent = S.MATRICULEAGENT  } equals new { Agent = M.MATRICULE  } into M_join
                    //from M in M_join.DefaultIfEmpty()
                    //where
                    //  S.FK_IDDEVIS == pIdDevis &&
                    //  S.MATRICULEAGENT  == pMatriculeAgent
                    //select new
                    //{
                    //    S.PK_ID,
                    //    S.FK_IDDEVIS,
                    //    S.NUMDEVIS,
                    //    S.FK_IDETAPEDEVIS,
                    //    S.DUREE,
                    //    S.MATRICULEAGENT ,
                    //    S.COMMENTAIRE,
                    //    NOMAGENT = M.LIBELLE,
                    //    LIBELLETACHE = S.ETAPEDEVIS.TACHEDEVIS.LIBELLE,
                    //        S.USERCREATION,
                    //    S.USERMODIFICATION,
                    //    S.DATECREATION,
                    //   S.DATEMODIFICATION
                    //};
                    //  return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable DEVIS_SUIVIDEVIS_RETOURNEByDevisId(CsCriteresDevis pCritereDevis)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    return new DataTable();

                    //  IEnumerable<object> query =
                    //from S in context.SUIVIDEVIS
                    //join M in context.ADMUTILISATEUR on new { Agent = S.MATRICULEAGENT  } equals new { Agent = M.MATRICULE  } into M_join
                    //from M in M_join.DefaultIfEmpty()
                    //where
                    //  S.FK_IDDEVIS  == pCritereDevis.IdDevis
                    //select new
                    //{
                    //    S.PK_ID,
                    //    S.FK_IDDEVIS,
                    //    S.NUMDEVIS,
                    //    S.FK_IDETAPEDEVIS,
                    //    S.DUREE,
                    //    S.MATRICULEAGENT ,
                    //    S.COMMENTAIRE,
                    //    NOMAGENT = M.LIBELLE,
                    //    LIBELLETACHE = S.ETAPEDEVIS.TACHEDEVIS.LIBELLE,
                    //    S.USERCREATION,
                    //    S.USERMODIFICATION,
                    //    S.DATECREATION,
                    //    S.DATEMODIFICATION
                    //};
                    //  return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //private static double CalculerDelai(CsCriteresDevis pCritereDevis)
        //{
        //    DateTime date, currentDate;
        //    currentDate = DateTime.Now.Date;
        //    TimeSpan difference;
        //    try
        //    {
        //        if (pCritereDevis != null)
        //        {
        //            date = (DateTime)pCritereDevis.DateEtape;
        //            difference = (currentDate.Date - date.Date);
        //           return difference.TotalDays;
        //        }
        //        return 0;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        #endregion

        #region TRAVAUXDEVIS
        public static DataTable DEVIS_TRAVAUXDEVIS_RETOURNEByDevisIdOrdre(int pDevisId, int pOrdre)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                 from E in context.TRAVAUXDEVIS
                 where
                   E.FK_IDDEMANDE == pDevisId &&
                   E.ORDRE == pOrdre
                 select new
                 {
                     E.PK_ID,
                     E.FK_IDDEMANDE,
                     E.FK_IDPRESTATAIRE,
                     E.NUMDEM,
                     E.ORDRE,
                     E.MATRICULECHEFEQUIPE,
                     E.NOMCHEFEQUIPE,
                     E.PROCESVERBAL,
                     E.MONTANTREGLE,
                     E.MONTANTREMBOURSEMENT,
                     E.DATEPREVISIONNELLE,
                     E.DATEDEBUTTRVX,
                     E.DATEFINTRVX,
                     E.MATRICULEREGLEMENT,
                     E.DATEREGLEMENT,
                     E.ISUSEDFORBILAN,
                     E.USERCREATION,
                     E.USERMODIFICATION,
                     E.DATECREATION,
                     E.DATEMODIFICATION
                 };
                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable DEVIS_TRAVAUXDEVIS_RETOURNEByDevisId(int pDevisId)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                 from E in context.TRAVAUXDEVIS
                 where
                   E.FK_IDDEMANDE == pDevisId
                 select new
                 {
                     E.PK_ID,
                     E.FK_IDDEMANDE,
                     E.FK_IDPRESTATAIRE,
                     E.NUMDEM,
                     E.ORDRE,
                     E.MATRICULECHEFEQUIPE,
                     E.NOMCHEFEQUIPE,
                     E.PROCESVERBAL,
                     E.MONTANTREGLE,
                     E.MONTANTREMBOURSEMENT,
                     E.DATEPREVISIONNELLE,
                     E.DATEDEBUTTRVX,
                     E.DATEFINTRVX,
                     E.MATRICULEREGLEMENT,
                     E.DATEREGLEMENT,
                     E.ISUSEDFORBILAN,
                     E.USERCREATION,
                     E.USERMODIFICATION,
                     E.DATECREATION,
                     E.DATEMODIFICATION,
                     E.NBRCABLESECTION
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

        #region ETATSDEVIS

        public static DataTable DEVIS_DEVIS_EditionDEvis(int pIdDevis, int pOrdre, bool pIsSummary, string pMatricule)
        {
            //todo Mettre les messaga dans des fichiers ressources
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    return new DataTable();
                    //string message = string.Empty;
                    //string produit = string.Empty;
                    ////bool bracket, @pole;

                    //var InfoEntreprise = (from entreprise in context.ENTREPRISE
                    //                      select entreprise).FirstOrDefault();

                    //var InfoMatricule = from user in context.ADMUTILISATEUR
                    //                    where user.MATRICULE == pMatricule
                    //                    select user;

                    //var devis = (from d in context.DEVIS
                    //             where
                    //               d.PK_ID == pIdDevis &&
                    //               d.ORDRE == pOrdre
                    //             select d).First();

                    //IEnumerable<object > query = (from d in context.DEVIS
                    //                                        join dem in context.DEMANDEDEVIS on d.PK_ID equals dem.FK_IDDEVIS
                    //                                        join e in context.ELEMENTDEVIS on d.PK_ID equals e.FK_IDDEVIS
                    //                                        join f in context.FOURNITURE on e.FK_IDFOURNITURE equals f.PK_ID
                    //                                        join p in context.PRODUIT on d.FK_IDPRODUIT equals p.PK_ID
                    //                                        join dep in context.DEPOSIT on new { NumDevis = d.NUMDEVIS } equals new { NumDevis = dep.NUMDEVIS } into dep_join
                    //                                        from dep in dep_join.DefaultIfEmpty()
                    //                                        join diam in context.DIAMETRECOMPTEUR  on dem.FK_IDDIAMETRECOMPTEUR equals diam.PK_ID into diam_join
                    //                                        from diam in diam_join.DefaultIfEmpty()
                    //                                        join tc in context.TYPECOMPTEUR   on new { IDTYPECTR = d.IDTYPECTR } equals new { IDTYPECTR = tc.CODE  } into tc_join
                    //                                        from tc in tc_join.DefaultIfEmpty()
                    //                                        join bk in context.BANQUE on new { BANQUE = dep.BANQUE } equals new { BANQUE = (string)bk.CODE } into bk_join
                    //                                        from bk in bk_join.DefaultIfEmpty()
                    //                                        join com in context.COMMUNE on dem.FK_IDCOMMUNE equals com.PK_ID into com_join
                    //                                        from com in com_join.DefaultIfEmpty()
                    //                                        join quart in context.QUARTIER on dem.FK_IDQUARTIER equals quart.PK_ID into quart_join
                    //                                        from quart in quart_join.DefaultIfEmpty()
                    //                                        where
                    //                                          e.FK_IDDEVIS == pIdDevis &&
                    //                                          e.ORDRE == pOrdre
                    //                                          //&&
                    //                                          //f.ISSUMMARY == pIsSummary
                    //                                        select new 
                    //                                        {
                    //                                            DevisId = d.PK_ID,
                    //                                            DateReglement = d.DATEREGLEMENT,
                    //                                            TypeDevis = d.ISPOSE == false ? (d.TYPEDEVIS.LIBELLE + " " + Enumere.Simplifie) : (d.TYPEDEVIS.LIBELLE + " " + Enumere.Complete),
                    //                                            Produit = p.LIBELLE,
                    //                                            Centre = d.CENTRE.LIBELLE,
                    //                                            Designation = f.DESIGNATION,
                    //                                            Quantite = e.QUANTITE,
                    //                                            Prix_Unitaire = f.PRIX_UNITAIRE,
                    //                                            montantHT = (decimal?)(e.QUANTITE * f.PRIX_UNITAIRE),
                    //                                            montantTTC = (decimal?)((e.QUANTITE * f.PRIX_UNITAIRE) + (Decimal)e.QUANTITE * f.PRIX_UNITAIRE * e.TAXE),
                    //                                            numdevis = e.NUMDEVIS,
                    //                                            ordre = e.ORDRE,
                    //                                            Nom = dem.NOM,
                    //                                            PoteauProche = dem.NUMPOTEAUPROCHE,
                    //                                            Telephone = dem.NUMTEL,
                    //                                            Latitude = dem.LATITUDE,
                    //                                            Longitude = dem.LONGITUDE,
                    //                                            Commune = com.LIBELLE,
                    //                                            Quartier = quart.LIBELLE,
                    //                                            QuantiteRemisEnStock = e.QUANTITEREMISENSTOCK,
                    //                                            RembourserHT = (decimal?)(e.QUANTITEREMISENSTOCK * f.PRIX_UNITAIRE),
                    //                                            RembourserTTC = (decimal?)((e.QUANTITEREMISENSTOCK * f.PRIX_UNITAIRE) + (Decimal)e.QUANTITEREMISENSTOCK * f.PRIX_UNITAIRE * e.TAXE),
                    //                                            //Payment = (dep.BANQUE ?? "") == "" ? Enumere.Agence + " " + InfoEntreprise.SIGLE : (bk.LIBELLE + ", " + Enumere.NumeroCompte + " " + dep.COMPTE),
                    //                                            NumeroCTR = d.NUMEROCTR,
                    //                                            MeterSize = (System.String)d.CODEPRODUIT == Enumere.Eau ? (Enumere.Diamete + ": " + diam.LIBELLE) : (Enumere.TypeCompteur + ": " + diam.LIBELLE),
                    //                                            TYPECTR = tc.LIBELLE,
                    //                                            Message = message,
                    //                                            DEPOSIT = ((Decimal?)dep.MONTANTDEPOSIT ?? (Decimal?)0),
                    //                                            MatriculeAgent = InfoMatricule.FirstOrDefault().MATRICULE,
                    //                                            EmplacementCRT = d.EMPLACEMENTCOMPTEUR,
                    //                                            NomAgent = InfoMatricule.FirstOrDefault().LIBELLE,
                    //                                            Taxe = e.TAXE,
                    //                                            f.ISDEFAULT 

                    //                                            //       ,
                    //                                            //NOMENTREPRISE = InfoEntreprise.NOM.ToUpper()
                    //                                            //  ,
                    //                                            //SIGLE = InfoEntreprise.SIGLE
                    //                                            //  ,
                    //                                            //SLOGAN = InfoEntreprise.SLOGAN
                    //                                            //  ,
                    //                                            //ADRESSEPRINCIPALE = InfoEntreprise.ADRESSEPRINCIPALE
                    //                                            //  ,
                    //                                            //ADRESSESECONDAIRE = InfoEntreprise.ADRESSESECONDAIRE
                    //                                            //  ,
                    //                                            //TELEPHONEPRINCIPAL = InfoEntreprise.TELEPHONEPRINCIPAL
                    //                                            //  ,
                    //                                            //TELEPHONESECONDAIRE = InfoEntreprise.TELEPHONESECONDAIRE
                    //                                            //  ,
                    //                                            //FAXPRINCIPALE = InfoEntreprise.FAXPRINCIPALE
                    //                                            //  ,
                    //                                            //FAXSECONDAIRE = InfoEntreprise.FAXSECONDAIRE
                    //                                            //  ,
                    //                                            //EMAILPRINCIPALE = InfoEntreprise.EMAILPRINCIPALE
                    //                                            //  ,
                    //                                            //EMAILSECONDAIRE = InfoEntreprise.EMAILSECONDAIRE
                    //                                            //  ,
                    //                                            //ACTIVITE = InfoEntreprise.ACTIVITE
                    //                                            //  ,
                    //                                            //PAYS = InfoEntreprise.PAYS
                    //                                            //  ,
                    //                                            //SITEINTERNET = InfoEntreprise.SITEINTERNET
                    //                                            //  ,
                    //                                            //LOGO = InfoEntreprise.LOGO

                    //                                        }).Distinct();


                    //return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable DEVIS_DEVIS_EditionBonSortie(int pIdDevis, int pOrdre, bool pIsSummary, string pMatricule)
        {
            //todo Mettre les messaga dans des fichiers ressources
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    return new DataTable();
                    //string message = string.Empty;
                    //string produit = string.Empty;
                    ////bool bracket, @pole;

                    //var InfoEntreprise = (from entreprise in context.ENTREPRISE
                    //                      select entreprise).FirstOrDefault();

                    //var InfoMatricule = from user in context.ADMUTILISATEUR
                    //                    where user.MATRICULE == pMatricule
                    //                    select user;

                    //var devis = (from d in context.DEVIS
                    //             where
                    //               d.PK_ID == pIdDevis &&
                    //               d.ORDRE == pOrdre
                    //             select d).First();

                    //produit = devis.CODEPRODUIT;
                    //bracket = Convert.ToBoolean(devis.ISFOURNITURE);
                    //pole = Convert.ToBoolean(devis.ISPOSE);

                    //if (produit == Enumere.Electricite && bracket == true && pole == true)
                    //    message = Enumere.Message1;
                    //else
                    //{
                    //    if (produit == Enumere.Electricite && bracket == true && pole == false)
                    //        message = Enumere.Message2;
                    //    else
                    //    {
                    //        if (produit == Enumere.Electricite && bracket == false && pole == true)
                    //            message = Enumere.Message3;
                    //        else
                    //        {
                    //            if (produit == Enumere.Eau && bracket == true)
                    //                message = Enumere.Message4;
                    //        }
                    //    }
                    //}

                    //#region
                    // IEnumerable<object > query = (from e in context.ELEMENTDEVIS 
                    //                               from dem in e.DEVIS.DEMANDEDEVIS 
                    //                               where e.FK_IDDEVIS == pIdDevis && e.DEVIS.ORDRE == pOrdre
                    //                               select new 
                    //                               {
                    //                                            DevisId = e.DEVIS.PK_ID ,
                    //                                            DateReglement = e.DEVIS.DATEREGLEMENT ,
                    //                                            TypeDevis = e.DEVIS.ISPOSE == false ? (e.DEVIS.TYPEDEVIS.LIBELLE + " " + Enumere.Simplifie) : (e.DEVIS.TYPEDEVIS.LIBELLE + " " + Enumere.Complete),
                    //                                            Produit = e.DEVIS.PRODUIT.LIBELLE,
                    //                                            Centre = e.DEVIS.CENTRE.LIBELLE,
                    //                                            Designation = e.FOURNITURE.DESIGNATION,
                    //                                            Quantite = e.QUANTITE,
                    //                                            Prix_Unitaire = e.FOURNITURE.PRIX_UNITAIRE,
                    //                                            montantHT = (decimal?)(e.QUANTITE * e.FOURNITURE.PRIX_UNITAIRE),
                    //                                            montantTTC = (decimal?)((e.QUANTITE * e.FOURNITURE.PRIX_UNITAIRE) + (Decimal)e.QUANTITE * e.FOURNITURE.PRIX_UNITAIRE * e.TAXE),
                    //                                            numdevis = e.NUMDEVIS,
                    //                                            ordre = e.ORDRE,
                    //                                            Nom = dem.NOM,
                    //                                            PoteauProche = dem.NUMPOTEAUPROCHE,
                    //                                            Telephone = dem.NUMTEL,
                    //                                            Latitude = dem.LATITUDE,
                    //                                            Longitude = dem.LONGITUDE,
                    //                                            Commune = dem.COMMUNE1.LIBELLE,
                    //                                            Quartier = dem.QUARTIER1.LIBELLE,
                    //                                            QuantiteRemisEnStock = e.QUANTITEREMISENSTOCK,
                    //                                            RembourserHT = (decimal?)(e.QUANTITEREMISENSTOCK * e.FOURNITURE.PRIX_UNITAIRE),
                    //                                            RembourserTTC = (decimal?)((e.QUANTITEREMISENSTOCK * e.FOURNITURE.PRIX_UNITAIRE) + (Decimal)e.QUANTITEREMISENSTOCK * e.FOURNITURE.PRIX_UNITAIRE * e.TAXE),
                    //                                            NumeroCTR = e.DEVIS.NUMEROCTR,
                    //                                            //MeterSize = (e.DEVIS.CODEPRODUIT == Enumere.Eau || Enumere.Electricite) ? (dem.DIAMETRECOMPTEUR.
                    //                                            //MeterSize = (System.String)d.CODEPRODUIT == Enumere.Eau ? (Enumere.Diamete + ": " + diam.LIBELLE) : (Enumere.TypeCompteur + ": " + diam.LIBELLE),
                    //                                            //TYPECTR = tc.LIBELLE,
                    //                                            Message = message,
                    //                                            DEPOSIT = ((Decimal?)dep.MONTANTDEPOSIT ?? (Decimal?)0),
                    //                                            MatriculeAgent = InfoMatricule.FirstOrDefault().MATRICULE,
                    //                                            EmplacementCRT = d.EMPLACEMENTCOMPTEUR,
                    //                                            NomAgent = InfoMatricule.FirstOrDefault().LIBELLE,
                    //                                            Taxe = e.TAXE ,
                    //                                            NOMENTREPRISE = InfoEntreprise.NOM.ToUpper(),
                    //                                            SIGLE = InfoEntreprise.SIGLE,
                    //                                            SLOGAN = InfoEntreprise.SLOGAN,
                    //                                            ADRESSEPRINCIPALE = InfoEntreprise.ADRESSEPRINCIPALE,
                    //                                            ADRESSESECONDAIRE = InfoEntreprise.ADRESSESECONDAIRE,
                    //                                            TELEPHONEPRINCIPAL = InfoEntreprise.TELEPHONEPRINCIPAL,
                    //                                            TELEPHONESECONDAIRE = InfoEntreprise.TELEPHONESECONDAIRE,
                    //                                            FAXPRINCIPALE = InfoEntreprise.FAXPRINCIPALE ,
                    //                                            FAXSECONDAIRE = InfoEntreprise.FAXSECONDAIRE ,
                    //                                            EMAILPRINCIPALE = InfoEntreprise.EMAILPRINCIPALE ,
                    //                                            EMAILSECONDAIRE = InfoEntreprise.EMAILSECONDAIRE ,
                    //                                            ACTIVITE = InfoEntreprise.ACTIVITE ,
                    //                                            PAYS = InfoEntreprise.PAYS ,
                    //                                            SITEINTERNET = InfoEntreprise.SITEINTERNET ,
                    //                                            LOGO = InfoEntreprise.LOGO
                    //                               };





                    //IEnumerable<object > query = (from d in context.DEVIS
                    //                                        join dem in context.DEMANDEDEVIS on d.PK_ID equals dem.FK_IDDEVIS
                    //                                        join e in context.ELEMENTDEVIS on d.PK_ID equals e.FK_IDDEVIS
                    //                                        join f in context.FOURNITURE on e.FK_IDFOURNITURE equals f.PK_ID
                    //                                        join p in context.PRODUIT on d.FK_IDPRODUIT equals p.PK_ID
                    //                                        join dep in context.DEPOSIT on new { NumDevis = d.NUMDEVIS } equals new { NumDevis = dep.NUMDEVIS } into dep_join
                    //                                        from dep in dep_join.DefaultIfEmpty()
                    //                                        join diam in context.DIAMETRECOMPTEUR  on dem.FK_IDDIAMETRECOMPTEUR equals diam.PK_ID into diam_join
                    //                                        from diam in diam_join.DefaultIfEmpty()
                    //                                        join tc in context.TYPECOMPTEUR   on new { IDTYPECTR = d.IDTYPECTR } equals new { IDTYPECTR = tc.CODE  } into tc_join
                    //                                        from tc in tc_join.DefaultIfEmpty()
                    //                                        join bk in context.BANQUE on new { BANQUE = dep.BANQUE } equals new { BANQUE = (string)bk.CODE } into bk_join
                    //                                        from bk in bk_join.DefaultIfEmpty()
                    //                                        join com in context.COMMUNE on dem.FK_IDCOMMUNE equals com.PK_ID into com_join
                    //                                        from com in com_join.DefaultIfEmpty()
                    //                                        join quart in context.QUARTIER on dem.FK_IDQUARTIER equals quart.PK_ID into quart_join
                    //                                        from quart in quart_join.DefaultIfEmpty()
                    //                                        where
                    //                                          e.FK_IDDEVIS == pIdDevis &&
                    //                                          e.ORDRE == pOrdre
                    //                                          && f.ISDEFAULT  == true 
                    //                                        select new 
                    //                                        {
                    //                                            DevisId = d.PK_ID,
                    //                                            DateReglement = d.DATEREGLEMENT,
                    //                                            TypeDevis = d.ISPOSE == false ? (d.TYPEDEVIS.LIBELLE + " " + Enumere.Simplifie) : (d.TYPEDEVIS.LIBELLE + " " + Enumere.Complete),
                    //                                            Produit = p.LIBELLE,
                    //                                            Centre = d.CENTRE.LIBELLE,
                    //                                            Designation = f.DESIGNATION,
                    //                                            Quantite = e.QUANTITE,
                    //                                            Prix_Unitaire = f.PRIX_UNITAIRE,
                    //                                            montantHT = (decimal?)(e.QUANTITE * f.PRIX_UNITAIRE),
                    //                                            montantTTC = (decimal?)((e.QUANTITE * f.PRIX_UNITAIRE) + (Decimal)e.QUANTITE * f.PRIX_UNITAIRE * e.TAXE),
                    //                                            numdevis = e.NUMDEVIS,
                    //                                            ordre = e.ORDRE,
                    //                                            Nom = dem.NOM,
                    //                                            PoteauProche = dem.NUMPOTEAUPROCHE,
                    //                                            Telephone = dem.NUMTEL,
                    //                                            Latitude = dem.LATITUDE,
                    //                                            Longitude = dem.LONGITUDE,
                    //                                            Commune = com.LIBELLE,
                    //                                            Quartier = quart.LIBELLE,
                    //                                            QuantiteRemisEnStock = e.QUANTITEREMISENSTOCK,
                    //                                            RembourserHT = (decimal?)(e.QUANTITEREMISENSTOCK * f.PRIX_UNITAIRE),
                    //                                            RembourserTTC = (decimal?)((e.QUANTITEREMISENSTOCK * f.PRIX_UNITAIRE) + (Decimal)e.QUANTITEREMISENSTOCK * f.PRIX_UNITAIRE * e.TAXE),
                    //                                            //Payment = (dep.BANQUE ?? "") == "" ? Enumere.Agence + " " + InfoEntreprise.SIGLE : (bk.LIBELLE + ", " + Enumere.NumeroCompte + " " + dep.COMPTE),
                    //                                            NumeroCTR = d.NUMEROCTR,
                    //                                            MeterSize = (System.String)d.CODEPRODUIT == Enumere.Eau ? (Enumere.Diamete + ": " + diam.LIBELLE) : (Enumere.TypeCompteur + ": " + diam.LIBELLE),
                    //                                            TYPECTR = tc.LIBELLE,
                    //                                            Message = message,
                    //                                            DEPOSIT = ((Decimal?)dep.MONTANTDEPOSIT ?? (Decimal?)0),
                    //                                            MatriculeAgent = InfoMatricule.FirstOrDefault().MATRICULE,
                    //                                            EmplacementCRT = d.EMPLACEMENTCOMPTEUR,
                    //                                            NomAgent = InfoMatricule.FirstOrDefault().LIBELLE,
                    //                                            Taxe = e.TAXE,
                    //                                            f.ISDEFAULT 

                    //                                            //       ,
                    //                                            //NOMENTREPRISE = InfoEntreprise.NOM.ToUpper()
                    //                                            //  ,
                    //                                            //SIGLE = InfoEntreprise.SIGLE
                    //                                            //  ,
                    //                                            //SLOGAN = InfoEntreprise.SLOGAN
                    //                                            //  ,
                    //                                            //ADRESSEPRINCIPALE = InfoEntreprise.ADRESSEPRINCIPALE
                    //                                            //  ,
                    //                                            //ADRESSESECONDAIRE = InfoEntreprise.ADRESSESECONDAIRE
                    //                                            //  ,
                    //                                            //TELEPHONEPRINCIPAL = InfoEntreprise.TELEPHONEPRINCIPAL
                    //                                            //  ,
                    //                                            //TELEPHONESECONDAIRE = InfoEntreprise.TELEPHONESECONDAIRE
                    //                                            //  ,
                    //                                            //FAXPRINCIPALE = InfoEntreprise.FAXPRINCIPALE
                    //                                            //  ,
                    //                                            //FAXSECONDAIRE = InfoEntreprise.FAXSECONDAIRE
                    //                                            //  ,
                    //                                            //EMAILPRINCIPALE = InfoEntreprise.EMAILPRINCIPALE
                    //                                            //  ,
                    //                                            //EMAILSECONDAIRE = InfoEntreprise.EMAILSECONDAIRE
                    //                                            //  ,
                    //                                            //ACTIVITE = InfoEntreprise.ACTIVITE
                    //                                            //  ,
                    //                                            //PAYS = InfoEntreprise.PAYS
                    //                                            //  ,
                    //                                            //SITEINTERNET = InfoEntreprise.SITEINTERNET
                    //                                            //  ,
                    //                                            //LOGO = InfoEntreprise.LOGO

                    //                                        }).Distinct();


                    //return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable DEVIS_DEVIS_EditionBonControle(int pIdDevis, int pOrdre)
        {
            try
            {
                using (galadbEntities db = new galadbEntities())
                {
                    return new DataTable();
                    //IEnumerable<object> query = (from d in db.DEMANDE
                    //                             join p in db.PRODUIT on d.FK_IDPRODUIT equals p.PK_ID
                    //                             join c in db.TRAVAUXDEVIS on d.NUMDEM equals c.NUMDEM into c_join
                    //                             from c in c_join.DefaultIfEmpty()
                    //                             join centre in db.CENTRE on d.FK_IDCENTRE equals centre.PK_ID
                    //                             join t in db.COMMUNE on dem.FK_IDCOMMUNE equals t.PK_ID into t_join
                    //                             from t in t_join.DefaultIfEmpty()
                    //                             join q in db.QUARTIER on dem.FK_IDQUARTIER equals q.PK_ID into q_join
                    //                             from q in q_join.DefaultIfEmpty()
                    //                             join r in db.RUES on dem.FK_IDRUE equals r.PK_ID into r_join
                    //                             from r in r_join.DefaultIfEmpty()
                    //                             join car in db.DIAMETRECOMPTEUR  on dem.FK_IDDIAMETRECOMPTEUR equals car.PK_ID into car_join
                    //                             from car in car_join.DefaultIfEmpty()
                    //                             join tc in db.TYPECOMPTEUR   on new { IDTYPECTR = d.IDTYPECTR } equals new { IDTYPECTR = tc.CODE  } into tc_join
                    //                             from tc in tc_join.DefaultIfEmpty()
                    //                             join t1 in db.MARQUECOMPTEUR on new { IDMARQUECTR = d.IDMARQUECTR } equals new { IDMARQUECTR = t1.CODE } into t1_join
                    //                             from t1 in t1_join.DefaultIfEmpty()
                    //                             join ctrvx in db.CONTROLETRAVAUX on d.PK_ID equals ctrvx.FK_IDDEVIS
                    //                             where
                    //                               d.PK_ID == pIdDevis &&
                    //                               c.ORDRE == pOrdre
                    //                             select new CsEtatProcesVerbal
                    //                             {
                    //                                 DevisId = d.PK_ID,
                    //                                 Route = dem.CLIENT,
                    //                                 Nom = dem.NOM,
                    //                                 Commune = t.LIBELLE,
                    //                                 NumLot = dem.NUMLOT,
                    //                                 Quartier = q.LIBELLE,
                    //                                 NumTel = dem.NUMTEL,
                    //                                 Rue = r.LIBELLE,
                    //                                 NumPoteauProche = dem.NUMPOTEAUPROCHE,
                    //                                 DateValidation = d.DATEETAPE,
                    //                                 TypeDevis = d.ISPOSE == false ? (d.TYPEDEVIS.LIBELLE + " " + Enumere.Simplifie) : (d.TYPEDEVIS.LIBELLE + " " + Enumere.Simplifie),
                    //                                 Produit = p.LIBELLE,
                    //                                 Centre = centre.LIBELLE,
                    //                                 TotalTTC = d.MONTANTTTC,
                    //                                 NumDevis = d.NUMDEVIS,
                    //                                 Ordre = c.ORDRE,
                    //                                 DateDebutTrvx = c.DATEDEBUTTRVX,
                    //                                 DateFinTrvx = c.DATEFINTRVX,
                    //                                 Matricule = c.MATRICULECHEFEQUIPE,
                    //                                 Chefequipe = c.NOMCHEFEQUIPE,
                    //                                 ProcesVerbal = c.PROCESVERBAL,
                    //                                 NumeroCTR = d.NUMEROCTR,
                    //                                 MeterSize = d.CODEPRODUIT == Enumere.Eau ? (Enumere.Diamete + ": " + car.LIBELLE) : (Enumere.TypeCompteur + ": " + car.LIBELLE),
                    //                                 TYPECTR = tc.LIBELLE,
                    //                                 MarqueCTR = t1.LIBELLE,
                    //                                 Adresse = dem.ADRESSE,
                    //                                 IndexPoseCTR = d.INDEXPOSECTR,
                    //                                 AnneeFabricationCTR = d.DATEFABRICATIONCTR.Value.Year,
                    //                                 NearestRoute = d.NEARESTROUTE,
                    //                                 NumeroGPS = d.NUMEROGPS,
                    //                                 DATECONTROLE = ctrvx.DATECONTROLE,
                    //                                 DEGRADATIONVOIE = ctrvx.DEGRADATIONVOIE,
                    //                                 METMOYCONTROLE = ctrvx.METMOYCONTROLE,
                    //                                 VOLUMETERTRVX = ctrvx.VOLUMETERTRVX,
                    //                                 NOTE = ctrvx.NOTE,
                    //                                 EmplacementCRT = d.EMPLACEMENTCOMPTEUR
                    //                             }).Distinct();

                    //return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion


        #region PUISSANCE

        public static DataTable DEVIS_PUISSANCE_GetByProduit(string pProduit)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                 (from PUISSANCEs in context.PUISSANCEINSTALLEE
                  where
                    PUISSANCEs.PRODUIT == pProduit
                  orderby
                    PUISSANCEs.CODE
                  select new
                  {
                      IDPUISSANCE = PUISSANCEs.CODE
                  }).Distinct();
                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable DEVIS_PUISSANCE_GetByProduitId(int pProduitId)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                 (from PUISSANCEs in context.PUISSANCEINSTALLEE
                  where
                    PUISSANCEs.FK_IDPRODUIT == pProduitId
                  orderby
                    PUISSANCEs.CODE
                  select new
                  {
                      IDPUISSANCE = PUISSANCEs.CODE
                  }).Distinct();
                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable DEVIS_PUISSANCE_RETOURNE()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                 from PUISSANCEs in context.PUISSANCEINSTALLEE
                 orderby
                   PUISSANCEs.CODE
                 select PUISSANCEs;
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
        public static DataTable DEVIS_TYPEDEVIS_SelByDevisById(int pIdtypedevis)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    return new DataTable();
                    //  IEnumerable<object> query =
                    //from e in context.TYPEDEVIS 
                    //where
                    //  e.PK_ID == pIdtypedevis
                    //select new
                    //{
                    //    e.PK_ID,
                    //    e.LIBELLE,
                    //    e.PRODUIT,
                    //    e.DATECREATION,
                    //    e.DATEMODIFICATION,
                    //    e.USERCREATION,
                    //    e.USERMODIFICATION,
                    //    e.FK_IDPRODUIT,
                    //    e.TDEM,

                    //    e.FK_IDTDEM
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

        #region Sylla

        public static DataTable ChargerTypeDocument()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                  from e in context.TYPEDOCUMENT
                  select new
                  {
                      e.PK_ID,
                      e.LIBELLE,
                      e.CODE
                  };
                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable ChargerCategorieClient_TypeClient()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                  from e in context.CATEGORIECLIENT_TYPECLIENT
                  select new
                  {
                      e.PK_ID,
                      e.FK_IDCATEGORIECLIENT,
                      e.FK_IDTYPECLIENT
                  };
                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static DataTable ChargerCategorieClient_Usage()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                  from e in context.CATEGORIECLIENT_USAGE
                  select new
                  {
                      e.PK_ID,
                      e.FK_IDCATEGORIECLIENT,
                      e.FK_IDUSAGE
                  };
                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static DataTable ChargerNatureClient_TypeClient()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    //  IEnumerable<object> query =
                    //from e in context.NATURECLIENT_TYPECLIENT
                    //select new
                    //{
                    //    e.PK_ID,
                    //    e.FK_IDNATURECLIENT,
                    //    e.FK_IDTYPECLIENT
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
        public static DataTable ChargerUsage_NatureClient()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    //  IEnumerable<object> query =
                    //from e in context.USAGE_NATURECLIENT
                    //select new
                    //{
                    //    e.PK_ID,
                    //    e.FK_IDNATURECLIENT,
                    //    e.FK_IDUSAGE
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

        #endregion


        #region CR TRAVAUX

        public static DataTable LoadListeOrganeScellable(int FK_IDTDEM, int FK_IDPRODUIT)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                    from t in context.RefActivite
                    from a in t.TYPEDEMANDE
                    from ao in t.ACTIVITEE_ORGANE
                    where a.PK_ID == FK_IDTDEM && ao.FK_IDPRODUIT == FK_IDPRODUIT
                    select new
                    {
                        ao.ORGANE_SCELLABLE.PK_ID,
                        ao.ORGANE_SCELLABLE.LIBELLE,
                        ao.ORGANE_SCELLABLE.FK_IDACTIVITE_TERRAIN
                    };
                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static DataTable LoadListeScelle()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                  from a in context.SCELLE
                  select new
                  {
                      a.NUM_SCELLE,
                      a.PK_ID,
                  };
                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static DataTable LoadListeScelle(int idAgentRecever, int fk_TypeDemande)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    var Activite_ID = context.TYPEDEMANDE.FirstOrDefault(t => t.PK_ID == fk_TypeDemande).RefActivite.Activite_ID;

                    DataTable dt = Galatee.Entity.Model.ScelleProcedures.RetourneListeCouleurScelle(Activite_ID);
                    List<CsCouleurActivite> _lstCouleur = Entities.GetEntityListFromQuery<CsCouleurActivite>(dt);
                    var lstCouleur = _lstCouleur.Select(c => c.Couleur_libelle).ToList();

                    //int matricule = int.Parse(Matricule);

                    IEnumerable<object> query =
                  from a in context.tbDetailRemiseScelles
                  join s in context.Scelles on a.Id_Scelle equals s.Id_Scelle
                  join c in context.RefCouleurlot on s.Couleur_Scelle equals c.Couleur_ID
                  where a.tbRemiseScelles.Matricule_Receiver == idAgentRecever
                  && lstCouleur.Contains(c.Couleur_libelle) && s.Status_ID != 5
                  select new
                  {
                      a.Scelles.Numero_Scelle,
                      a.Scelles.Id_Scelle
                  };
                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public static DataTable LoadListeScelle(int idAgentRecever, int fk_TypeDemande, int Activite_ID)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    DataTable dt = Galatee.Entity.Model.ScelleProcedures.RetourneListeCouleurScelle(Activite_ID);
                    List<CsCouleurActivite> _lstCouleur = Entities.GetEntityListFromQuery<CsCouleurActivite>(dt);
                    var lstCouleur = _lstCouleur.Select(c => c.Couleur_libelle).ToList();

                    //int matricule = int.Parse(Matricule);

                    IEnumerable<object> query =
                  from a in context.tbDetailRemiseScelles
                  join s in context.Scelles on a.Id_Scelle equals s.Id_Scelle
                  join c in context.RefCouleurlot on s.Couleur_Scelle equals c.Couleur_ID
                  where a.tbRemiseScelles.Matricule_Receiver == idAgentRecever
                  && lstCouleur.Contains(c.Couleur_libelle) && s.Status_ID != 5
                  select new
                  {
                      a.Scelles.Numero_Scelle,
                      a.Scelles.Id_Scelle
                  };
                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static DataTable DEVIS_ELEMENTDEVIS_MaterielByDevisById(List<int> pIdDevis)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                                  from d in context.DEMANDE
                                  from e in d.ELEMENTDEVIS
                                  from cli in d.DCLIENT
                                  from ag in d.DAG
                                  from can in d.DCANALISATION
                                  where
                                   pIdDevis.Contains(e.FK_IDDEMANDE) && e.COPER.CODE == Enumere.CoperTRV
                                  select new
                                  {
                                      CLIENT = d.CENTRE + " " + d.CLIENT + " " + d.ORDRE,
                                      e.NUMDEM,
                                      DESIGNATION = e.MATERIELDEVIS.LIBELLE,
                                      e.MATERIELDEVIS.LIBELLE,
                                      e.QUANTITE,
                                      COMMUNE = ag.COMMUNE1.LIBELLE,
                                      QUARTIER = ag.QUARTIER1.LIBELLE,
                                      ag.RUE,
                                      ag.PORTE,
                                      NOM = cli.NOMABON,
                                      COMPTEUR = can.MAGASINVIRTUEL.NUMERO,
                                      cli.TELEPHONE,
                                      can.REGLAGECOMPTEUR1.REGLAGE

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

        #region PARTICIPATION
        public static DataTable DEVIS_PARTICIPATIONDevisId(int pIddevis)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {

                    IEnumerable<object> query =
                   from x in context.FRAIXPARICIPATIONDEVIS
                   where
                     x.FK_IDDEMANDE == pIddevis
                   select new
                   {
                       x.PK_ID,
                       x.CENTRE,
                       x.REF_CLIENT,
                       x.ORDRE,
                       x.MONTANT,
                       x.ESTEXONERE,
                       x.PREUVE,
                       x.FK_IDDEMANDE
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
