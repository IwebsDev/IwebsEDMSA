using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Transactions;
using Galatee.Structure;
using System.Data.Entity.Validation;


namespace Galatee.Entity.Model
{
    public class RecouvProcedures
    {
        public static DataTable RetourneMoratoireClient(string pcentre, string pclient, string pordre)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    var moratoire = context.MORATOIRE.Where(m => m.CENTRE == pcentre && m.CLIENT == pclient && m.ORDRE == pordre
                                                                && m.STATUS != Enumere.StatusSupprimer);
                    var query = from c in moratoire
                                                from d in c.DETAILMORATOIRE
                                                select new
                                                {
                                                    d.NDOC,
                                                    d.REFEM,
                                                    d.CRET,
                                                    d.MONTANT,
                                                    d.FRAISDERETARD,
                                                    d.EXIGIBILITE,
                                                    IDMORATOIRE = d.FK_IDMORATOIRE,
                                                    c.CENTRE ,
                                                    c.CLIENT ,
                                                    c.ORDRE ,
                                                    c.FK_IDCENTRE ,
                                                    d.DATECAISSE
                                                };
                    return Galatee.Tools.Utility.ListToDataTable<object>(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public static DataTable VerifiePaiementMoratoire(int fk_idclient,List<CsDetailMoratoire> lstMor)
        {
            try
            {

                List<CsLclient> LstEncaissement = new List<CsLclient>();
              
                    using (galadbEntities context = new galadbEntities())
                    {
                        List<int> lstMoratoire = new List<int>();
                        foreach (CsDetailMoratoire item in lstMor)
                            lstMoratoire.Add(item.PK_ID);

                        IEnumerable<object> query = (
                            from camp in context.LCLIENT
                            from trans in camp.TRANSCAISB
                            where
                              (lstMoratoire.Contains(camp.FK_IDMORATOIRE.Value)) && camp.FK_IDCLIENT == fk_idclient &&
                                string.IsNullOrEmpty(trans.TOPANNUL) && trans.NDOC != "TIMBRE"
                            select new 
                            {
                                camp.PK_ID ,
                                FK_IDCLIENT = camp.FK_IDCLIENT,
                                MONTANT = camp.MONTANT,
                            });

                        IEnumerable<object> query1 = (
                                       from camp in context.LCLIENT
                                       from trans in camp.TRANSCAISSE 
                                       where
                                             (lstMoratoire.Contains(camp.FK_IDMORATOIRE.Value)) && camp.FK_IDCLIENT == fk_idclient &&
                                             string.IsNullOrEmpty(trans.TOPANNUL) && trans.NDOC != "TIMBRE"
                                       select new 
                                       {
                                           FK_IDCLIENT = camp.FK_IDCLIENT,
                                           MONTANT = camp.MONTANT,
                                           camp.PK_ID,
                                       });
                        return Galatee.Tools.Utility.ListToDataTable<object>(query.Union(query1).ToList());
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
        }

         public static DataTable RetourneReglementMoratoireClient(string pcentre, string pclient, string pordre)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    var moratoire = context.MORATOIRE.Where(m =>m.CENTRE == pcentre && m.CLIENT == pclient && m.ORDRE == pordre
                                                                && m.STATUS != Enumere.StatusSupprimer);
                                   var query = from c in moratoire
                                                from d in c.TRANSCAISB  
                                                select new
                                                {
                                                    d.NDOC,
                                                    d.REFEM,
                                                    d.CRET,
                                                    d.FK_IDMORATOIRE,
                                                };
                    return Galatee.Tools.Utility.ListToDataTable<object>(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable RetourneClientDuChecque(string numChq, string banque, string guichet)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    List<TRANSCAISB> transB = new List<TRANSCAISB>();
                    var transcaissB = context.TRANSCAISB.Where(t => t.NUMCHEQ == numChq && t.BANQUE == banque
                        //&& t.GUICHET == guichet
                        );
                    IEnumerable<object> query = from t in transcaissB
                                                join b in context.BANQUE on t.BANQUE equals b.CODE
                                                select new
                                                {

                                                    t.CENTRE,
                                                    t.CLIENT,
                                                    t.ORDRE,
                                                    t.CAISSE,
                                                    t.ACQUIT,
                                                    t.MATRICULE,
                                                    t.NDOC,
                                                    t.REFEM,
                                                    t.MONTANT,
                                                    t.DC,
                                                    t.COPER,
                                                    t.PERCU,
                                                    t.RENDU,
                                                    t.MODEREG,
                                                    t.PLACE,
                                                    t.DTRANS,
                                                    t.DEXIG,
                                                    t.BANQUE,
                                                    t.GUICHET,
                                                    t.ORIGINE,
                                                    t.ECART,
                                                    t.TOPANNUL,
                                                    t.MOTIFANNULATION,
                                                    t.CRET,
                                                    t.MOISCOMPT,
                                                    t.TOP1,
                                                    t.TOURNEE,
                                                    t.NUMDEM,
                                                    t.DATEVALEUR,
                                                    t.DATEFLAG,
                                                    t.NUMCHEQ,
                                                    t.SAISIPAR,
                                                    t.DATEENCAISSEMENT,
                                                    t.CANCELLATION,
                                                    t.USERCREATION,
                                                    t.DATECREATION,
                                                    t.DATEMODIFICATION,
                                                    t.USERMODIFICATION,
                                                    t.PK_ID,
                                                    t.FK_IDCENTRE,
                                                    t.FK_IDLCLIENT,
                                                    FK_IDCLIENT = t.LCLIENT.FK_IDCLIENT,
                                                    t.FK_IDHABILITATIONCAISSE,
                                                    t.FK_IDMODEREG,
                                                    t.FK_IDLIBELLETOP,
                                                    t.FK_IDCAISSIERE,
                                                    t.FK_IDAGENTSAISIE,
                                                    t.FK_IDCOPER,
                                                    t.FK_IDPOSTECLIENT,
                                                    t.FK_IDNAF,
                                                    t.POSTE,
                                                    t.DATETRANS,
                                                    t.BANQUECAISSE,
                                                    t.AGENCEBANQUE,
                                                    b.LIBELLE
                                                };

                    return Galatee.Tools.Utility.ListToDataTable<object>(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static DataTable RetourneFactureImplique(int pk_IdClient, CsAvisCoupureEdition aviscoupure)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    var _lstLclient = context.LCLIENT;
                    IEnumerable<object> query = from l in _lstLclient
                                                where
                                                      aviscoupure.Exigible >= l.EXIGIBILITE &&
                                                      l.DC == Enumere.Debit &&
                                                      l.COPER != Enumere.CoperRNA &&
                                                      l.FK_IDCLIENT == pk_IdClient
                                                //&& FonctionCaisse.RetourneSoldeDocument(l.FK_IDCLIENT,l.NDOC,l.CRET)> 0

                                                select new
                                                {
                                                    l.PK_ID,
                                                    l.CENTRE,
                                                    l.CLIENT,
                                                    l.ORDRE,
                                                    l.REFEM,
                                                    l.NDOC,
                                                    l.COPER,
                                                    l.DENR,
                                                    l.EXIG,
                                                    l.MONTANT,
                                                    l.CAPUR,
                                                    l.CRET,
                                                    l.MODEREG,
                                                    l.DC,
                                                    l.ORIGINE,
                                                    l.CAISSE,
                                                    l.ECART,
                                                    l.MOISCOMPT,
                                                    l.TOP1,
                                                    l.EXIGIBILITE,
                                                    l.FRAISDERETARD,
                                                    l.REFERENCEPUPITRE,
                                                    l.IDLOT,
                                                    l.DATEVALEUR,
                                                    l.REFERENCE,
                                                    l.REFEMNDOC,
                                                    l.ACQUIT,
                                                    l.MATRICULE,
                                                    l.TAXESADEDUIRE,
                                                    l.DATEFLAG,
                                                    l.MONTANTTVA,
                                                    l.IDCOUPURE,
                                                    l.AGENT_COUPURE,
                                                    l.RDV_COUPURE,
                                                    l.NUMCHEQ,
                                                    l.OBSERVATION_COUPURE,
                                                    l.USERCREATION,
                                                    l.DATECREATION,
                                                    l.DATEMODIFICATION,
                                                    l.USERMODIFICATION,
                                                    l.BANQUE,
                                                    l.GUICHET,
                                                    l.FK_IDCENTRE,
                                                    l.FK_IDADMUTILISATEUR,
                                                    l.FK_IDCOPER,
                                                    l.FK_IDLIBELLETOP,
                                                    l.FK_IDCLIENT,
                                                    l.POSTE,
                                                    l.FK_IDPOSTE

                                                };
                    return Galatee.Tools.Utility.ListToDataTable<object>(query);
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static DataTable RetourneElementAvisCoupureGC(CsAvisCoupureEdition aviscoupure)
        {
            try
            {
                List<int> lstIDreglement = new List<int>();
                foreach (CsRegCli item in aviscoupure.ListeRegroupement)
                    lstIDreglement.Add(item.PK_ID);

                using (galadbEntities context = new galadbEntities())
                {
                    var _AG = context.AG;
                    IEnumerable<object> query = (
                                                    from a in _AG
                                                    from cl in a.CLIENT1
                                                    from t in cl.ABON
                                                    where lstIDreglement.Contains(cl.FK_IDREGROUPEMENT.Value)
                                                    select new
                                                    {
                                                        cl.CENTRE,
                                                        cl.REFCLIENT,
                                                        cl.ORDRE,
                                                        cl.NOMABON,
                                                        cl.ADRMAND1,
                                                        LIBELLECATEGORIE = cl.CATEGORIECLIENT.LIBELLE,
                                                        REGROUPEMENT = cl.REGROUPEMENT1.CODE,
                                                        cl.FK_IDREGROUPEMENT ,
                                                        cl.PK_ID,
                                                        t.PRODUIT,
                                                        TOURNEE = a.TOURNEE1.CODE,
                                                        cl.FK_IDCATEGORIE,
                                                        cl.FK_IDCENTRE,
                                                        a.ORDTOUR,
                                                        FK_IDTOURNEE = a.TOURNEE1.PK_ID,
                                                        t.DRES,
                                                        //a.RUE,
                                                        //a.PORTE 
                                                    }).Distinct();
                    return Galatee.Tools.Utility.ListToDataTable<object>(query);
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable RetourneElementAvisCoupure(CsAvisCoupureEdition aviscoupure)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    var _AG = context.AG;
                    IEnumerable<object> query = (
                                                    from a in _AG
                                                    from cl in a.CLIENT1
                                                    from t in cl.ABON
                                                    from can in t.CANALISATION
                                                    where
                                                         aviscoupure.Tournees.Contains(a.FK_IDTOURNEE) &&
                                                         aviscoupure.Categories.Contains(cl.FK_IDCATEGORIE) &&
                                                        (aviscoupure.Consomateur.Count == 0 || aviscoupure.Consomateur.Contains(cl.FK_IDCENTRE )) &&
                                                         aviscoupure.idCentre == a.FK_IDCENTRE
                                                    select new
                                                    {
                                                        cl.CENTRE,
                                                        cl.REFCLIENT,
                                                        cl.ORDRE,
                                                        cl.NOMABON,
                                                        cl.ADRMAND1,
                                                        LIBELLECATEGORIE = cl.CATEGORIECLIENT.LIBELLE,
                                                        REGROUPEMENT = cl.REGROUPEMENT1.CODE,
                                                        cl.PK_ID,
                                                        COMPTEUR = can.COMPTEUR.NUMERO,
                                                        can.PRODUIT,
                                                        TOURNEE = a.TOURNEE1.CODE,
                                                        cl.FK_IDCATEGORIE,
                                                        cl.FK_IDCENTRE,
                                                        a.ORDTOUR,
                                                        FK_IDTOURNEE = a.TOURNEE1.PK_ID,
                                                        t.DRES ,
                                                        a.RUE,
                                                        a.PORTE ,
                                                    }).Distinct();
                    return Galatee.Tools.Utility.ListToDataTable<object>(query);
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable RetourneCampagneReglementCoupure(List<int> lstCentre, string idCampagne)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    var _lstCampagne = context.LCLIENT;
                    IEnumerable<object> query = (from x in _lstCampagne
                                                 from y in x.TRANSCAISB
                                                 from z in x.TRANSCAISSE
                                                 where x.IDCOUPURE == idCampagne
                                                 //lstCentre.Contains(x.FK_IDCENTRE )
                                                 select new
                                                 {
                                                     x.IDCOUPURE,
                                                     x.CENTRE,
                                                     x.CLIENT,
                                                     x.ORDRE,
                                                     x.CLIENT1.NOMABON,
                                                     x.RDV_COUPURE,
                                                     MONTANTFRAIS = z.MONTANT,
                                                     z.DATETRANS
                                                 }).Distinct();
                    return Galatee.Tools.Utility.ListToDataTable<object>(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable RetourneCampagneRendezVousCoupure(List<int> lstCentre, string idCampagne)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    var _lstCampagne = context.LCLIENT;
                    IEnumerable<object> query = (from x in _lstCampagne
                                                 where x.IDCOUPURE == idCampagne && x.RDV_COUPURE != null
                                                 //lstCentre.Contains(x.FK_IDCENTRE )
                                                 select new
                                                 {
                                                     x.IDCOUPURE,
                                                     x.CENTRE,
                                                     x.CLIENT,
                                                     x.ORDRE,
                                                     x.CLIENT1.NOMABON,
                                                     x.RDV_COUPURE
                                                 }).Distinct();
                    return Galatee.Tools.Utility.ListToDataTable<object>(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static DataTable RetourneElementAvisCoupure(List<int> lstCentre)
        {
            try
            {
                List<CsAvisCoupureEdition> coupure = new List<CsAvisCoupureEdition>();
                using (galadbEntities context = new galadbEntities())
                {
                    var _lstCampagne = context.CAMPAGNE;
                    List<string> lstIdCampagne = new List<string>();
                    IEnumerable<object> query = (from x in _lstCampagne
                                                 from y in x.DETAILCAMPAGNE
                                                 where lstCentre.Contains(x.FK_IDCENTRE )
                                                 select new
                                                 {
                                                     x.IDCOUPURE,
                                                     x.CENTRE,
                                                     x.MONTANT,
                                                     x.MATRICULEPIA,
                                                     x.PERIODE_RELANCABLE,
                                                     x.DATE_EXIGIBILITE,
                                                     x.PREMIERE_TOURNEE,
                                                     x.DERNIERE_TOURNEE,
                                                     x.DEBUT_ORDTOUR,
                                                     x.FIN_ORDTOUR,
                                                     x.MONTANT_RELANCABLE,
                                                     x.DEBUT_CATEGORIE,
                                                     x.FIN_CATEGORIE,
                                                     x.NOMBRE_CLIENT,
                                                     x.NOMBRE_FACTURE,
                                                     x.DEBUT_AG,
                                                     x.FIN_AG,
                                                     x.PK_ID,
                                                     x.DATECREATION,
                                                     x.DATEMODIFICATION,
                                                     x.USERCREATION,
                                                     x.USERMODIFICATION,
                                                     x.FK_IDCENTRE,
                                                     x.FK_IDMATRICULE,
                                                     CODESITE = x.CENTRE1.SITE.CODE,
                                                     FK_IDSITE = x.CENTRE1.FK_IDCODESITE,
                                                     LIBELLECENTRE = x.CENTRE1.LIBELLE,
                                                     LIBELLESITE = x.CENTRE1.SITE.LIBELLE,
                                                     AGENTPIA = (context.ADMUTILISATEUR.FirstOrDefault(t => t.MATRICULE == x.MATRICULEPIA) != null) ? context.ADMUTILISATEUR.FirstOrDefault(t => t.MATRICULE == x.MATRICULEPIA).LIBELLE : string.Empty

                                                 }).Distinct();
                    return Galatee.Tools.Utility.ListToDataTable<object>(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static DataTable RetourneTourneeFromCampagne(List<CsCAMPAGNE> lstCampagne)
        {
            try
            {
                List<int > lstIdCampagne = new List<int>();
                foreach (CsCAMPAGNE item in lstCampagne)
                    lstIdCampagne.Add(item.PK_ID);

                using (galadbEntities context = new galadbEntities())
                {
                    var _lstCampagne = context.CAMPAGNE;
                    IEnumerable<object> query = (from x in _lstCampagne
                                                 from y in x.DETAILCAMPAGNE
                                                 where lstIdCampagne.Contains(x.PK_ID )
                                                 select new
                                                 {
                                                     y.TOURNEE1.PK_ID,
                                                     CODE=  y.TOURNEE ,
                                                     x.IDCOUPURE 
                                                 }).Distinct();
                    return Galatee.Tools.Utility.ListToDataTable<object>(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable RetourneDonneesSaisieIndexAvisCoupure(List<int> lstCentre)
        {
            try
            {
                List<CsAvisCoupureEdition> coupure = new List<CsAvisCoupureEdition>();
                using (galadbEntities context = new galadbEntities())
                {
                    var _lstCampagne = context.CAMPAGNE;
                    List<string> lstIdCampagne = new List<string>();
                    IEnumerable<object> query = (from x in _lstCampagne
                                                 from y in x.DETAILCAMPAGNE
                                                 join idxCampagne in context.INDEXCAMPAGNE on new { x.IDCOUPURE } equals
                                                                                              new { IDCOUPURE = idxCampagne.IDCOUPURE } into _idxSaisie
                                                 from p in _idxSaisie.DefaultIfEmpty()
                                                 where lstCentre.Contains(x.FK_IDCENTRE )
                                                 select new
                                                 {
                                                     x.IDCOUPURE,
                                                     x.CENTRE,
                                                     x.MONTANT,
                                                     x.MATRICULEPIA,
                                                     x.PERIODE_RELANCABLE,
                                                     x.DATE_EXIGIBILITE,
                                                     x.PREMIERE_TOURNEE,
                                                     x.DERNIERE_TOURNEE,
                                                     x.DEBUT_ORDTOUR,
                                                     x.FIN_ORDTOUR,
                                                     x.MONTANT_RELANCABLE,
                                                     x.DEBUT_CATEGORIE,
                                                     x.FIN_CATEGORIE,
                                                     x.NOMBRE_CLIENT,
                                                     x.NOMBRE_FACTURE,
                                                     x.DEBUT_AG,
                                                     x.FIN_AG,
                                                     x.PK_ID,
                                                     x.DATECREATION,
                                                     x.DATEMODIFICATION,
                                                     x.USERCREATION,
                                                     x.USERMODIFICATION,
                                                     x.FK_IDCENTRE,
                                                     x.FK_IDMATRICULE,
                                                     CODESITE = x.CENTRE1.SITE.CODE,
                                                     FK_IDSITE = x.CENTRE1.FK_IDCODESITE,
                                                     LIBELLECENTRE = x.CENTRE1.LIBELLE,
                                                     LIBELLESITE = x.CENTRE1.SITE.LIBELLE,
                                                     AGENTPIA = (context.ADMUTILISATEUR.FirstOrDefault(t => t.MATRICULE == x.MATRICULEPIA) != null) ? context.ADMUTILISATEUR.FirstOrDefault(t => t.MATRICULE == x.MATRICULEPIA).LIBELLE : string.Empty,

                                                     INDEX = p.INDEXE.Value,
                                                     DATESAISIE = p.DATECOUPURE 

                                                 }).Distinct();
                    return Galatee.Tools.Utility.ListToDataTable<object>(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static DataTable RetourneDonneeAnnulationFrais(CsCAMPAGNE campagne, CsClient leClientRech)
        {

            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = (
                    from cp in context.LCLIENT
                    join cnl in context.DETAILCAMPAGNE on new { cp.CENTRE, cp.CLIENT, cp.ORDRE, cp.FK_IDCENTRE }
                                   equals new { cnl.CENTRE, cnl.CLIENT, cnl.ORDRE, cnl.FK_IDCENTRE }
                    where cp.IDCOUPURE == campagne.IDCOUPURE && cp.FK_IDCENTRE == campagne.FK_IDCENTRE && 
                          cp.COPER == Enumere.CoperFRP && cnl.FRAIS != 0  &&
                          !context.TRANSCAISB.Any(es => es.FK_IDCENTRE == cp.FK_IDCENTRE && es.CENTRE == cp.CENTRE && es.CLIENT == cp.CLIENT && es.ORDRE == cp.ORDRE)
                    select new
                    {

                        cnl. IDCOUPURE,
                        cnl. CENTRE,
                        cnl. CLIENT,
                        cnl. ORDRE,
                        cnl.CLIENT1.NOMABON,
                        MONTANTFRAIS = cp.MONTANT,
                        cnl.TOURNEE,
                        cnl.ORDTOUR,
                        cnl.CATEGORIECLIENT,
                        cnl.SOLDEDUE,
                        cnl.NOMBREFACTURE ,
                        cnl. DATECREATION,
                        cnl. DATEMODIFICATION,
                        cnl. USERCREATION,
                        cnl. USERMODIFICATION,
                        cnl. FK_IDCENTRE,
                        cnl.FK_IDTOURNEE,
                        cnl.FK_IDCATEGORIECLIENT,
                        cnl. FK_IDCAMPAGNE,
                        cnl. FK_IDCLIENT,
                        cnl.SOLDECLIENT,
                        cnl.COMPTEUR,
                        cnl.ISAUTORISER,
                        cnl.MOTIFAUTORISATION,
                        cnl.FRAIS,
                        cnl.ISANNULATIONFRAIS,
                        cnl.MOTIFANNULATION,
                        cp.ISNONENCAISSABLE 

                    }).Distinct();

                    IEnumerable<object> query1 = (
                                 from cp in context.LCLIENT
                                 join cnl in context.DETAILCAMPAGNE on new { cp.CENTRE, cp.CLIENT, cp.ORDRE, cp.FK_IDCENTRE }
                                                equals new { cnl.CENTRE, cnl.CLIENT, cnl.ORDRE, cnl.FK_IDCENTRE }
                                 where cp.IDCOUPURE == campagne.IDCOUPURE &&
                                       cp.COPER == Enumere.CoperFRP && cnl.FRAIS != 0 &&
                                       !context.TRANSCAISSE.Any(es => es.FK_IDCENTRE == cp.FK_IDCENTRE && es.CENTRE == cp.CENTRE && es.CLIENT == cp.CLIENT && es.ORDRE == cp.ORDRE)

                                 select new
                                 {
                                     
                                        cnl. IDCOUPURE,
                                        cnl. CENTRE,
                                        cnl. CLIENT,
                                        cnl. ORDRE,
                                        cnl.CLIENT1.NOMABON,
                                        MONTANTFRAIS = cp.MONTANT,
                                        cnl.TOURNEE,
                                        cnl.ORDTOUR,
                                        cnl.CATEGORIECLIENT,
                                        cnl.SOLDEDUE,
                                        cnl.NOMBREFACTURE,
                                        cnl. DATECREATION,
                                        cnl. DATEMODIFICATION,
                                        cnl. USERCREATION,
                                        cnl. USERMODIFICATION,
                                        cnl. FK_IDCENTRE,
                                        cnl.FK_IDTOURNEE,
                                        cnl.FK_IDCATEGORIECLIENT,
                                        cnl. FK_IDCAMPAGNE,
                                        cnl. FK_IDCLIENT,
                                        cnl.SOLDECLIENT,
                                        cnl.COMPTEUR,
                                        cnl.ISAUTORISER,
                                        cnl.MOTIFAUTORISATION,
                                        cnl.FRAIS,
                                        cnl.ISANNULATIONFRAIS,
                                        cnl.MOTIFANNULATION,
                                        cp.ISNONENCAISSABLE 
                                 }).Distinct();

                    IEnumerable<object> queryf = query.Union(query1);
                    return Galatee.Tools.Utility.ListToDataTable<object>(queryf);
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public static bool ValidationAnnulationFrais(List<CsDetailCampagne> campagne)
        {

            try
            {
                List<LCLIENT> leFrais = new List<LCLIENT>();
                List<int> lstIdClient = new List<int>();
                List<int> lstIdDetail = new List<int>();
                int res = -1;
                foreach (CsDetailCampagne item in campagne)
                    lstIdClient.Add(item.FK_IDCLIENT);

                //foreach (CsDetailCampagne item in campagne)
                //    lstIdDetail.Add(item.PK_ID );

                string idCoupure = campagne.First().IDCOUPURE;
                List<DETAILCAMPAGNE> lstDetail = new List<DETAILCAMPAGNE>();

                using (galadbEntities context = new galadbEntities())
                {
                    leFrais = context.LCLIENT.Where(cp => cp.IDCOUPURE == idCoupure &&
                           cp.COPER == Enumere.CoperFRP && lstIdClient.Contains(cp.FK_IDCLIENT)).ToList();
                };

                if (leFrais != null && leFrais.Count != 0)
                {
                    using (galadbEntities context = new galadbEntities())
                    {
                        foreach (int item in lstIdClient)
                            lstDetail.AddRange(context.DETAILCAMPAGNE.Where(t=>t.FK_IDCLIENT == item && t.IDCOUPURE == idCoupure ).ToList());

                        foreach (DETAILCAMPAGNE item in lstDetail)
                        {
                            item.ISANNULATIONFRAIS = true;
                            item.MOTIFANNULATION = campagne.FirstOrDefault(t => t.FK_IDCLIENT == item.FK_IDCLIENT).MOTIFAUTORISATION;
                        }
                        if (lstDetail != null && lstDetail.Count != 0)
                        {
                            Entities.DeleteEntity<LCLIENT>(leFrais, context);
                            res = context.SaveChanges();
                        }
                    }

                }
                return res == -1 ? false : true;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public static bool ValidationAutorisationFrais(List<CsDetailCampagne> campagne)
        {

            try
            {
                List<LCLIENT> leFrais = new List< LCLIENT>();
                List<int> lstIdClient = new List<int>();
                List<int> lstIdDetail = new List<int>();
                int res = -1 ;
                foreach (CsDetailCampagne item in campagne)
                    lstIdClient.Add(item.FK_IDCLIENT);

                string idCoupure = campagne.First().IDCOUPURE;
                List<DETAILCAMPAGNE> lstDetail = new List<DETAILCAMPAGNE>();

                using (galadbEntities context = new galadbEntities())
                {
                    leFrais = context.LCLIENT.Where(cp => cp.IDCOUPURE == idCoupure &&
                           cp.COPER == Enumere.CoperFRP && lstIdClient.Contains(cp.FK_IDCLIENT)).ToList();
                };

                if (leFrais != null && leFrais.Count != 0)
                {
                    using(galadbEntities context = new galadbEntities())
	                {
                        leFrais.ForEach(t => t.ISNONENCAISSABLE = null);
                            Entities.UpdateEntity <LCLIENT>(leFrais, context);
                            res = context.SaveChanges();
	                }
                }
                return res == -1 ?false :true ;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public static bool ValidationReposeCompteur(CsDetailCampagne campagne)
        {

            try
            {
                List<LCLIENT> leFrais = new List<LCLIENT>();

                int res = -1;
               using (galadbEntities context = new galadbEntities())
               {
                   List<DETAILCAMPAGNE> lstDetail = context.DETAILCAMPAGNE.Where(t => t.FK_IDCLIENT == campagne.FK_IDCLIENT && t.IDCOUPURE == campagne.IDCOUPURE).ToList();
                   leFrais = context.LCLIENT.Where(cp => cp.IDCOUPURE == campagne.IDCOUPURE  &&
                          cp.COPER == Enumere.CoperFRP && campagne.FK_IDCLIENT  ==cp.FK_IDCLIENT).ToList();
                   foreach (DETAILCAMPAGNE item in lstDetail)
                   {
                       item.ISAUTORISER = true;
                       item.MOTIFAUTORISATION = campagne.MOTIFAUTORISATION ;
                   }
                   leFrais.ForEach(t => t.ISNONENCAISSABLE = null);
                   Entities.UpdateEntity<DETAILCAMPAGNE>(lstDetail, context);
                   res = context.SaveChanges();
               };
               return res == -1 ? false : true;

            }
            catch (Exception ex)
            {
                return false;
            }

        }
        
        public static DataTable RetourneCampagneCoupureIdCoupureNonSaisi(CsCAMPAGNE Campagne, CsClient leClientRech)
        {

            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    var query1 = (from x in context.DETAILCAMPAGNE
                                  where x.IDCOUPURE == Campagne.IDCOUPURE &&                         
                                    !context.INDEXCAMPAGNE.Any(es => es.FK_IDCLIENT == x.FK_IDCLIENT && es.IDCOUPURE == x.IDCOUPURE )
                                  select new
                                  {
                                      x.IDCOUPURE,
                                      x.CENTRE,
                                      x.CLIENT,
                                      x.ORDRE,
                                      x.TOURNEE,
                                      x.ORDTOUR,
                                      x.CATEGORIECLIENT,
                                      x.SOLDEDUE,
                                      x.NOMBREFACTURE,
                                      x.DATECREATION,
                                      x.DATEMODIFICATION,
                                      x.USERCREATION,
                                      x.USERMODIFICATION,
                                      x.FK_IDCENTRE,
                                      x.FK_IDTOURNEE,
                                      x.FK_IDCATEGORIECLIENT,
                                      x.FK_IDCAMPAGNE,
                                      x.FK_IDCLIENT,
                                      x.SOLDECLIENT,
                                      x.COMPTEUR,
                                      x.ISAUTORISER,
                                      x.MOTIFAUTORISATION,
                                      x.FRAIS,
                                      x.ISANNULATIONFRAIS,
                                      x.MOTIFANNULATION,
                                      x.DATERDV
                                  }).Distinct();
                    return Galatee.Tools.Utility.ListToDataTable<object>(query1);
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public static DataTable RetourneDonneeFraisCoupure(string idcoupure)
        {

            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    var query1 = (from cp in context.DETAILCAMPAGNE                                 
                                  where cp.IDCOUPURE == idcoupure && !context.LCLIENT.Any(es =>es.FK_IDCLIENT  ==cp.FK_IDCLIENT && es.IDCOUPURE == idcoupure && es.COPER == Enumere.CoperFRP)
                                  select new
                                  {
                                      IdCoupure = cp.IDCOUPURE,
                                      Centre = cp.CENTRE,
                                      Client = cp.CLIENT,
                                      Ordre = cp.ORDRE,
                                      MontantRelance = cp.CAMPAGNE.MONTANT,
                                      MatrAgent = cp.CAMPAGNE.MATRICULEPIA,
                                      cp.CLIENT1.NOMABON,
                                      cp.FK_IDCLIENT,
                                      cp.FK_IDCENTRE,
                                      FK_IDCAMPAGNE = cp.CAMPAGNE.PK_ID
                                  }).OrderBy(o => new { o.Centre, o.Client, o.Ordre, o.FK_IDCENTRE }).Distinct();
                    return Galatee.Tools.Utility.ListToDataTable<object>(query1);
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public static DataTable RetourneCampagneCoupureIdCoupureSaisi(CsCAMPAGNE Campagne, CsClient leClientRech)
        {

            try
            {
                using (galadbEntities context = new galadbEntities())
                {

                    IEnumerable<object> query1 = (
                    from camp in context.INDEXCAMPAGNE
                    join cnl in context.DETAILCAMPAGNE on new {camp.FK_IDCLIENT } equals new {FK_IDCLIENT = cnl.FK_IDCLIENT}
                    where cnl.IDCOUPURE == Campagne.IDCOUPURE && string.IsNullOrEmpty( cnl.MOTIFAUTORISATION) 
                    //&&
                    //      (leClientRech == null || 
                    //      (leClientRech != null && leClientRech.FK_IDCENTRE == cnl.FK_IDCLIENT && leClientRech.CENTRE == cnl.CENTRE && leClientRech.REFCLIENT == cnl.CLIENT)) 
                    select new
                    {
                        camp. IDCOUPURE,
                        camp. CENTRE,
                        camp. CLIENT,
                        camp. ORDRE,
                        camp.CLIENT1.NOMABON,
                        cnl.TOURNEE,
                        cnl.ORDTOUR,
                        cnl.CATEGORIECLIENT,
                        cnl.SOLDEDUE,
                        cnl.NOMBREFACTURE,
                        camp. DATECREATION,
                        camp. DATEMODIFICATION,
                        camp. USERCREATION,
                        camp. USERMODIFICATION,
                        camp. FK_IDCENTRE,
                        cnl.FK_IDTOURNEE,
                        cnl.FK_IDCATEGORIECLIENT,
                        camp. FK_IDCAMPAGNE,
                        camp. FK_IDCLIENT,
                        cnl.SOLDECLIENT,
                        cnl.COMPTEUR,
                        cnl.ISAUTORISER,
                        cnl.MOTIFAUTORISATION,
                        cnl.FRAIS,
                        cnl.ISANNULATIONFRAIS,
                        cnl.MOTIFANNULATION
                    }).Distinct();
                    return Galatee.Tools.Utility.ListToDataTable<object>(query1);



                   
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public static DataTable RetourneCampagneCoupureParDate(DateTime debut, DateTime pfin)
        {
            try
            {
                string vide = "";
                string[] datearraydeb = debut.ToShortDateString().Split('/');
                string debutdate = datearraydeb[2] + datearraydeb[1] + datearraydeb[0] + "00";

                int deb = int.Parse(debutdate);
                string[] datearrayfin = pfin.ToShortDateString().Split('/');
                string findate = datearrayfin[2] + datearrayfin[1] + datearrayfin[0] + "99";

                int fin = int.Parse(findate);

                List<string> Cas = new List<string>() { Enumere.CasCreation, Enumere.CompteurChanged };
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<LCLIENT> clientCamp = from ct in context.LCLIENT
                                                      where !ct.IDCOUPURE.Equals(vide)
                                                      select ct;

                    List<LCLIENT> compteclient = new List<LCLIENT>();
                    foreach (var c in clientCamp)
                    {
                        if (Convert.ToDecimal(c.IDCOUPURE) < fin && Convert.ToDecimal(c.IDCOUPURE) > deb)
                            compteclient.Add(c);
                    }

                    var query1 = (

                                  from ag in context.AG
                                  from cl in ag.CLIENT1
                                  from t in cl.ABON
                                  from cnl in t.CANALISATION
                                  join cp in compteclient on cl.PK_ID equals cp.FK_IDCLIENT
                                  //join q in context.QUARTIER on ag.FK_IDQUARTIER equals  q.PK_ID 
                                  //join d in context.DIRECTEUR on cl.FK_IDCENTRE  equals d.FK_IDCENTRE 
                                  //join m in context.ADMUTILISATEUR on cp.AGENT_COUPURE equals m.MATRICULE 
                                  select new
                                  {
                                      Centre = cp.CENTRE,
                                      IdCoupure = cp.IDCOUPURE,
                                      Client = cp.CLIENT,
                                      Ordre = cp.ORDRE,
                                      NOM = cl.NOMABON,
                                      DateRDV = cp.RDV_COUPURE,
                                      MatrAgent = cp.AGENT_COUPURE,
                                      cp.OBSERVATION_COUPURE,
                                      ag.PORTE,
                                      TOURNEE = ag.TOURNEE1.CODE,
                                      RUES = ag.RUES.CODE,
                                      QUARTIER = ag.QUARTIER1.CODE,
                                      NomQuartier = ag.QUARTIER1.LIBELLE,
                                      NomAgence = ag.CENTRE1.LIBELLE,
                                      //NomAgent = m.LIBELLE,
                                  }).Distinct();
                    return Galatee.Tools.Utility.ListToDataTable<object>(query1);
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable VerifieFraisDejaSaisi(CsDetailCampagne leClient)
        {
            using (galadbEntities context = new galadbEntities())
            {
                var indexcoupure = (from p in context.LCLIENT
                                    where p.FK_IDCLIENT == leClient.FK_IDCLIENT && 
                                          p.COPER == Enumere.CoperFRP &&
                                          p.IDCOUPURE == leClient.IDCOUPURE 
                                    select new
                                    {
                                        p.CENTRE
                                    });

                return Galatee.Tools.Utility.ListToDataTable<object>(indexcoupure);

            }
        }
        public static DataTable EditionClientAReposer(List<CsCAMPAGNE> lstCampagne)
        {
            using (galadbEntities context = new galadbEntities())
            {
                List<string > lstidCampagne = new List<string >();
                foreach (CsCAMPAGNE item in lstCampagne)
                    lstidCampagne.Add(item.IDCOUPURE);

                   IEnumerable<object> query = (
                    from camp in context.LCLIENT
                    from trans in camp.TRANSCAISB
                    join cnl in context.INDEXCAMPAGNE on new { trans.CENTRE, trans.CLIENT, trans.ORDRE, trans.FK_IDCENTRE }
                                   equals new { cnl.CENTRE, cnl.CLIENT, cnl.ORDRE, cnl.FK_IDCENTRE }
                    join detc in context.DETAILCAMPAGNE  on new { trans.CENTRE, trans.CLIENT, trans.ORDRE, trans.FK_IDCENTRE }
                                   equals new { detc.CENTRE, detc.CLIENT, detc.ORDRE, detc.FK_IDCENTRE }
                    where
                    (lstidCampagne.Contains(camp.IDCOUPURE)) && camp.COPER == Enumere.CoperFRP &&
                        string.IsNullOrEmpty(trans.TOPANNUL) && trans.NDOC != "TIMBRE"
                    select new 
                    {
                        camp.IDCOUPURE,
                        camp.CENTRE,
                        camp.CLIENT,
                        camp.ORDRE,
                        camp.MONTANT,
                        INDEX= cnl.INDEXE ,
                        cnl.DATECOUPURE,
                        cnl.CLIENT1.NOMABON,
                        cnl.CLIENT1.AG.RUE,
                        cnl.CLIENT1.AG.PORTE ,
                        detc.COMPTEUR 
                    });


                   IEnumerable<object> query1 = (
                               from camp in context.LCLIENT
                               from trans in camp.TRANSCAISSE
                               join cnl in context.INDEXCAMPAGNE on new { trans.CENTRE, trans.CLIENT, trans.ORDRE, trans.FK_IDCENTRE }
                                equals new { cnl.CENTRE, cnl.CLIENT, cnl.ORDRE, cnl.FK_IDCENTRE }
                               join detc in context.DETAILCAMPAGNE on new { trans.CENTRE, trans.CLIENT, trans.ORDRE, trans.FK_IDCENTRE }
                                              equals new { detc.CENTRE, detc.CLIENT, detc.ORDRE, detc.FK_IDCENTRE }
                               where
                                 (lstidCampagne.Contains(camp.IDCOUPURE)) && camp.COPER == Enumere.CoperFRP &&
                                  string.IsNullOrEmpty(trans.TOPANNUL) && trans.NDOC != "TIMBRE"
                               select new  
                               {
                                   camp.IDCOUPURE,
                                   camp.CENTRE,
                                   camp.CLIENT,
                                   camp.ORDRE,
                                   camp.MONTANT,
                                   INDEX = cnl.INDEXE,
                                   cnl.DATECOUPURE,
                                   cnl.CLIENT1.NOMABON,
                                   cnl.CLIENT1.AG.RUE,
                                   cnl.CLIENT1.AG.PORTE,
                                   detc.COMPTEUR 
                               });

                IEnumerable<object> autorisation = (from p in context.DETAILCAMPAGNE
                                    join idxCampagne in context.INDEXCAMPAGNE on new { p.IDCOUPURE } equals
                                                                                    new { IDCOUPURE = idxCampagne.IDCOUPURE } into _idxSaisie
                                     from t in _idxSaisie.DefaultIfEmpty()
                                                    where p.ISAUTORISER == true && !string.IsNullOrEmpty(p.MOTIFAUTORISATION) && lstidCampagne.Contains(p.IDCOUPURE)

                                    select new
                                    {
                                        p.IDCOUPURE,
                                        p.CENTRE,
                                        p.CLIENT,
                                        p.ORDRE,
                                        MONTANT = 0,
                                        INDEX = t.INDEXE,
                                        t.DATECOUPURE,
                                        p.CLIENT1.NOMABON,
                                        p.CLIENT1.AG.RUE,
                                        p.CLIENT1.AG.PORTE,
                                        p.COMPTEUR 
                                    }).OrderBy(c => new { c.CLIENT, c.ORDRE });
                IEnumerable<object> queryf = query.Union(autorisation).Union(query1);
                return Galatee.Tools.Utility.ListToDataTable<object>(queryf);
            }
        }
        public static DataTable RechercherIndexParCampagne(string idCoupure)
        {
            using (galadbEntities context = new galadbEntities())
            {
                var indexcoupure = (from p in context.INDEXCAMPAGNE
                                    //join cnl in context.CANALISATION on new { p.CLIENT  , p.FK_IDCENTRE}
                                    //                                   equals new { cnl. , cnl.FK_IDCENTRE }
                                    //join cmp in context.CAMPAGNE on p.FK_IDCAMPAGNE equals cmp.PK_ID
                                    where p.IDCOUPURE == idCoupure
                                    select new
                                    {
                                        p.IDCOUPURE,
                                        p.CENTRE,
                                        p.CLIENT,
                                        p.ORDRE,
                                        p.MONTANT,
                                        p.INDEXE,
                                        //cnl.PRODUIT ,
                                        p.INDEXO,
                                        p.CODEOBSERVATION,
                                        p.DATECOUPURE,
                                        p.DATERDV,
                                        p.FK_IDCAMPAGNE
                                    }).OrderBy(c => new { c.CLIENT, c.ORDRE });

                return Galatee.Tools.Utility.ListToDataTable<object>(indexcoupure);

            }
        }

        public static DataTable RechercherIndexCampagneParClient(string idCoupure, string centre, string client, string ordre)
        {
            using (galadbEntities context = new galadbEntities())
            {
                var indexcamp = from i in context.INDEXCAMPAGNE
                                where i.IDCOUPURE == idCoupure &&
                                      i.CENTRE == centre &&
                                      i.CLIENT == client &&
                                      i.ORDRE == ordre
                                select new
                                {

                                    CENTRE = i.CENTRE,
                                    CLIENT = i.CLIENT,
                                    ORDRE = i.ORDRE,
                                    SoldeInitial = i.MONTANT,
                                    i.INDEXO,
                                    i.INDEXE,
                                    Observation = i.CODEOBSERVATION,
                                    i.DATERDV,
                                    i.DATECOUPURE
                                };

                return Galatee.Tools.Utility.ListToDataTable<object>(indexcamp);
            }
        }

        public static DataTable RechercherImpayeCampagne(string centre, string client, string ordre, string campagne)
        {
            // convertir id campagne est date
            string year = campagne.Substring(0, 4);
            string month = campagne.Substring(4, 2);
            string day = campagne.Substring(6, 2);

            DateTime datecamp = new DateTime(int.Parse(year), int.Parse(month), int.Parse(day));
            using (galadbEntities context = new galadbEntities())
            {
                List<LCLIENT> compte = context.LCLIENT.Where(c => c.CENTRE == centre && c.CLIENT == client &&
                                                                 c.ORDRE == ordre && c.DC == Enumere.Debit && c.IDCOUPURE == campagne).ToList();

                List<LCLIENT> clientDebiteur = new List<LCLIENT>();
                foreach (var c in compte)
                {
                    decimal? solde = FonctionCaisse.RetourneSoldeInitialDocumentCampagne(c.CENTRE, c.CLIENT, c.ORDRE, c.NDOC, campagne);
                    if (solde > 0)
                    {
                        c.MONTANTTVA = solde;
                        clientDebiteur.Add(c);
                    }
                }

                // recupérer le nombre de factures réglées
                int? NombreNdocRegler = 0;
                foreach (var c in clientDebiteur)
                {
                    decimal? soldedoc = FonctionCaisse.RetourneSoldeDocument(c.FK_IDCENTRE, c.CENTRE, c.CLIENT, c.ORDRE, c.NDOC, c.REFEM);
                    c.MONTANT = soldedoc;
                    if (soldedoc < 0)
                        NombreNdocRegler++;
                }

                // recupérer les frais de remise
                decimal? Frais = 0;
                DateTime? Datefrais = null;

                LCLIENT cmpt = context.LCLIENT.FirstOrDefault(c => c.CENTRE == centre && c.CLIENT == client && 
                                                                 c.ORDRE == ordre && c.DC == Enumere.Credit && c.DENR > datecamp);
                if (cmpt != null)
                {
                    Frais = cmpt.MONTANT;
                    Datefrais = cmpt.DENR;
                }

                var query2 = from i in clientDebiteur
                             select new
                             {
                                 i.NDOC,
                                 SoldeInitial = i.MONTANTTVA,
                                 SoldeCourant = i.MONTANT,
                                 NbreFactRegle = NombreNdocRegler,
                                 Frais = Frais,
                                 DateRemise = Datefrais
                             };
                return Galatee.Tools.Utility.ListToDataTable<object>(query2);
            }
        }

        public static DataTable RetourneCritereCampagne(string campagne)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    var campagn = from c in context.CAMPAGNE
                                  where c.IDCOUPURE == campagne
                                  select new
                                  {

                                      c.IDCOUPURE,
                                      c.MONTANT,
                                      Agent = c.MATRICULEPIA,
                                      PeriodeRelance = c.PERIODE_RELANCABLE,
                                      DateExigibilite = c.DATE_EXIGIBILITE,
                                      DebutTournee = c.PREMIERE_TOURNEE,
                                      FinTournee = c.DERNIERE_TOURNEE,
                                      DebutOrdTournee = c.DEBUT_ORDTOUR,
                                      FinOrdTournee = c.FIN_ORDTOUR,
                                      MontantRelance = c.MONTANT_RELANCABLE
                                  };
                    return Galatee.Tools.Utility.ListToDataTable<object>(campagn);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public static DataTable GetTournee(string centre, int Fk_idcentre)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    var campagn = from c in context.TOURNEE
                                  where c.CENTRE == centre && c.FK_IDCENTRE == Fk_idcentre
                                  select new
                                  {
                                      c.CENTRE,
                                      //c.RELEVEUR,
                                      c.CODE,
                                      c.LIBELLE,
                                      c.LOCALISATION,
                                      c.PRIORITE,
                                      //c.MATRICULEPIA,
                                      //NOMAGENTPIA = c.ADMUTILISATEUR.LIBELLE,
                                      c.USERCREATION,
                                      c.DATECREATION,
                                      c.DATEMODIFICATION,
                                      c.USERMODIFICATION,
                                      c.PK_ID,
                                      c.SUPPRIMER,
                                      c.FK_IDCENTRE,
                                      //c.FK_IDRELEVEUR,
                                      //c.FK_IDADMUTILISATEUR,
                                      c.FK_IDLOCALISATION
                                  };
                    return Galatee.Tools.Utility.ListToDataTable<object>(campagn);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool SaveAffectationTourne( List<CsTournee> ListeDesTourneAAffecter)
        {
            try
            {


                List<int?> lstIdTourne = ListeDesTourneAAffecter.Select(u => u.FK_IDTOURNEE).ToList();
                int? IdPia = ListeDesTourneAAffecter.First().FK_IDADMUTILISATEUR ;

                List<TOURNEEPIA> TournePiaA = new List<TOURNEEPIA>();
                List<TOURNEEPIA> TournePiaAutreAfermer = new List<TOURNEEPIA>();
                using (galadbEntities ctx = new galadbEntities())
                {
                    TournePiaA = ctx.TOURNEEPIA.Where(t => t.FK_IDADMUTILSATEUR == IdPia && t.DATEFIN == null ).ToList();
                    TournePiaAutreAfermer =ctx.TOURNEEPIA.Where(t => t.FK_IDADMUTILSATEUR != IdPia && lstIdTourne.Contains(t.FK_IDTOURNEE)).ToList();
                }
                List<TOURNEEPIA> TournePiaAFermer = TournePiaA.Where(p => !lstIdTourne.Contains(p.FK_IDTOURNEE)).ToList();
                TournePiaAFermer.AddRange(TournePiaAutreAfermer);
                TournePiaAFermer.ForEach(t => t.DATEFIN = System.DateTime.Today);

                List<int> lstIdTourneAnc = TournePiaA.Select(t=>t.FK_IDTOURNEE).ToList();
                List<TOURNEEPIA> TourneParPIAAjouter = Entities.ConvertObject<TOURNEEPIA,CsTournee>(ListeDesTourneAAffecter.Where(p => !lstIdTourneAnc.Contains(p.FK_IDTOURNEE.Value)).ToList());
                TourneParPIAAjouter.ForEach(o => o.FK_IDADMUTILSATEUR = IdPia.Value );

                int res = -1;
                using (galadbEntities context = new galadbEntities())
                {
                    if (TournePiaAFermer != null && TournePiaAFermer.Count != 0)
                    Entities.UpdateEntity<Galatee.Entity.Model.TOURNEEPIA>(TournePiaAFermer, context);
                    Entities.InsertEntity<Galatee.Entity.Model.TOURNEEPIA>(TourneParPIAAjouter, context);
                   res= context.SaveChanges();
                }
                return res == -1 ? false : true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
       public static List<TOURNEEPIA> RetourneListTourneePia(List<CsTournee> lstTournee)
        {
            try
            {
                List<TOURNEEPIA> lstT = new List<TOURNEEPIA>();
                foreach (CsTournee item in lstTournee)
                {
                    lstT.Add(new TOURNEEPIA()
                    {
                        FK_IDADMUTILSATEUR = item.FK_IDADMUTILISATEUR.Value ,
                        FK_IDTOURNEE = item.FK_IDTOURNEE.Value ,
                        DATEDEBUT = item.DATEDEBUT.Value ,
                        DATEFIN = item.DATEFIN
                    });
                }
                return lstT;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private static bool ConvertAndCompar_Sup(int periode, string ev)
        {
            return Convert.ToInt32(ev) >= periode;
        }
        private static bool ConvertAndCompar_Inf(int periode, string ev)
        {
            return Convert.ToInt32(ev) <= periode;
        }
        public static DataTable liste_impaye(CsCritere critere)
        {
            try
            {

                using (galadbEntities context = new galadbEntities())
                {
                    var _lclient = context.LCLIENT;
                    IEnumerable<object> query =
                    from fact in _lclient
                    where fact.COPER == Enumere.CoperChqImp &&
                     ((fact.FK_IDCENTRE == critere.FK_IDCENTRE || critere.FK_IDCENTRE ==0) &&
                      (fact.DENR >= critere.DATEDEBUT || critere.DATEDEBUT == new DateTime()) && 
                      (fact.DENR <= critere.DATEFIN || critere.DATEFIN == new DateTime()))
                    select new
                    {
                        fact.CENTRE,
                        fact.CLIENT,
                        fact.ORDRE,
                        fact.MOISCOMPT,
                        MONTANTFRAIS = fact.MONTANT.Value,
                        fact.CLIENT1.NOMABON,
                        LIBELLECENTRE = fact.CENTRE1.LIBELLE,
                        LIBELLEMOTIF =fact.MOTIFCHEQUEIMPAYE.LIBELLE ,
                        LIBELLEBANQUE = context.BANQUE.FirstOrDefault(t => t.CODE == fact.BANQUE) == null ? string.Empty :
                                        context.BANQUE.FirstOrDefault(t => t.CODE == fact.BANQUE).LIBELLE ,
                        NUMCHEQ = fact.NUMCHEQ 

                    };
                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public static bool SaveRDVCoupure(int FK_IDclient, string idcoupure, DateTime dateRdv)
        {
            try
            {
                int retourne = -1;
                using (galadbEntities context = new galadbEntities())
                {
                    List<DETAILCAMPAGNE> LACAMPAGNE = context.DETAILCAMPAGNE.Where(l => l.FK_IDCLIENT == FK_IDclient && l.IDCOUPURE == idcoupure).ToList();
                    List<LCLIENT> LCLIENT = context.LCLIENT.Where(l => l.FK_IDCLIENT == FK_IDclient && l.IDCOUPURE == idcoupure).ToList();
                    if (LCLIENT != null && LCLIENT.Count != 0)
                    {
                        foreach (LCLIENT item in LCLIENT)
                            item.RDV_COUPURE = dateRdv;
                    }
                    if (LACAMPAGNE != null && LACAMPAGNE.Count != 0)
                    {
                        foreach (DETAILCAMPAGNE item in LACAMPAGNE)
                            item.DATERDV = dateRdv;
                    }
                    retourne = context.SaveChanges();
                    return retourne < 0 ? false : true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool SaveRDVCoupureHorsCampagne(CsClient leClient, DateTime dateRdv)
        {
            try
            {
                int retourne = -1;
                using (galadbEntities context = new galadbEntities())
                {
                    List<LCLIENT> LCLIENT = context.LCLIENT.Where(l => l.FK_IDCLIENT == leClient.PK_ID ).ToList();
                    if (LCLIENT != null && LCLIENT.Count != 0)
                    {
                        foreach (LCLIENT item in LCLIENT)
                            item.RDV_COUPURE = dateRdv;
                    }
                    retourne = context.SaveChanges();
                    return retourne < 0 ? false : true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool SaveCampagne(CAMPAGNE camp, List<CsAvisDeCoupureClient> Comptes)
        {
            try
            {
                int Resultat = -1;
                using (galadbEntities context = new galadbEntities())
                {

                    if (Comptes.Count > 0)
                    {
                        foreach (CsAvisDeCoupureClient item in Comptes)
                        {
                            LCLIENT leCompte = context.LCLIENT.FirstOrDefault(t => t.PK_ID == item.FK_IDLCLIENT  );
                            if (leCompte != null)
                            {
                                leCompte.IDCOUPURE = camp.IDCOUPURE;
                                leCompte.DATEMODIFICATION = System.DateTime.Today;
                                leCompte.USERCREATION = camp.USERMODIFICATION;
                            }

                        }
                        camp.MONTANT = camp.DETAILCAMPAGNE.Select(t => new { t.FK_IDCLIENT, t.SOLDEDUE }).Distinct().ToList().Sum(y => y.SOLDEDUE);
                        Entities.InsertEntity<CAMPAGNE>(camp, context);
                        Resultat = context.SaveChanges();
                    }
                }
                return (Resultat == -1) ? false : true;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public static DataTable ListeDesClientEnRDC(List<CsCAMPAGNE> lstCampagne)
        {
            using (galadbEntities context = new galadbEntities())
            {
                List<string> lstidCampagne = new List<string>();
                foreach (CsCAMPAGNE item in lstCampagne)
                    lstidCampagne.Add(item.IDCOUPURE);

                IEnumerable<object> query = (
                 from camp in context.DETAILCAMPAGNE
                 where
                 lstidCampagne.Contains(camp.IDCOUPURE) && camp.DATERDV != null  
                 select new
                 {
                     camp.IDCOUPURE,
                     camp.CENTRE,
                     camp.CLIENT,
                     camp.ORDRE,
                     camp.CLIENT1.NOMABON ,
                     camp.SOLDECLIENT,
                     camp.SOLDEDUE,
                     camp.NOMBREFACTURE,
                     DATERENDEZVOUS=camp.DATERDV 
                 }).Distinct();
                return Galatee.Tools.Utility.ListToDataTable<object>(query);
            }
        }

        public static DataTable ListeDesClientRelance(List<CsCAMPAGNE> lstCampagne)
        {
            using (galadbEntities context = new galadbEntities())
            {
                List<string> lstidCampagne = new List<string>();
                foreach (CsCAMPAGNE item in lstCampagne)
                    lstidCampagne.Add(item.IDCOUPURE);

                IEnumerable<object> query = (
                 from camp in context.INDEXCAMPAGNE 
                 join det in context.DETAILCAMPAGNE on camp.FK_IDCLIENT  equals det.FK_IDCLIENT  
                 where
                 lstidCampagne.Contains(camp.IDCOUPURE)  && det.RELANCE != null  
                 select new
                 {
                     camp.IDCOUPURE,
                     camp.FK_IDCENTRE,
                     camp.FK_IDCLIENT ,
                     camp.CENTRE,
                     camp.CLIENT,
                     camp.ORDRE,
                     camp.CLIENT1.NOMABON,
                     camp.DATECOUPURE ,
                     det.COMPTEUR ,
                     LIBELLECOUPURE = camp.TYPECOUPURE.LIBELLE,
                     INDEX=  camp.INDEXO ,
                     det.TOURNEE ,
                     det.FK_IDTOURNEE ,
                     det.FRAIS ,
                     det.ISANNULATIONFRAIS,
                     det.RELANCE 
                 }).Distinct();
                return Galatee.Tools.Utility.ListToDataTable<object>(query);
            }
        }

        public static DataTable ListeDesClientIndexSaisi(CsCAMPAGNE lCampagne)
        {
            using (galadbEntities context = new galadbEntities())
            {


                IEnumerable<object> query = (
                 from camp in context.INDEXCAMPAGNE
                 join det in context.DETAILCAMPAGNE on new { camp.FK_IDCLIENT, camp.IDCOUPURE } equals
                                                       new { det.FK_IDCLIENT, det.IDCOUPURE }
                 where
                  camp.IDCOUPURE == lCampagne.IDCOUPURE 
                 select new
                 {
                     camp.IDCOUPURE,
                     camp.FK_IDCENTRE,
                     camp.FK_IDCLIENT,
                     camp.CENTRE,
                     camp.CLIENT,
                     camp.ORDRE,
                     camp.CLIENT1.NOMABON,
                     camp.DATECOUPURE,
                     det.COMPTEUR,
                     LIBELLECOUPURE = camp.TYPECOUPURE.LIBELLE,
                     INDEX = camp.INDEXO,
                     det.TOURNEE,
                     det.FK_IDTOURNEE,
                     det.FRAIS,
                     det.ISANNULATIONFRAIS,
                     det.RELANCE
                 }).Distinct();
                return Galatee.Tools.Utility.ListToDataTable<object>(query);
            }
        }
        public static DataTable ListeDesClientIndexSaisi(List<CsCAMPAGNE> lstCampagne)
        {
            using (galadbEntities context = new galadbEntities())
            {
                List<string> lstidCampagne = new List<string>();
                foreach (CsCAMPAGNE item in lstCampagne)
                    lstidCampagne.Add(item.IDCOUPURE);

                IEnumerable<object> query = (
                 from camp in context.INDEXCAMPAGNE 
                 join det in context.DETAILCAMPAGNE on camp.FK_IDCLIENT  equals det.FK_IDCLIENT  
                 where
                 lstidCampagne.Contains(camp.IDCOUPURE)  
                 select new
                 {
                     camp.IDCOUPURE,
                     camp.FK_IDCENTRE,
                     camp.FK_IDCLIENT ,
                     camp.CENTRE,
                     camp.CLIENT,
                     camp.ORDRE,
                     camp.CLIENT1.NOMABON,
                     camp.DATECOUPURE ,
                     det.COMPTEUR ,
                     LIBELLECOUPURE = camp.TYPECOUPURE.LIBELLE,
                     INDEX=  camp.INDEXO ,
                     det.TOURNEE ,
                     det.FK_IDTOURNEE ,
                     det.FRAIS ,
                     det.ISANNULATIONFRAIS,
                     det.RELANCE 
                 }).Distinct();
                return Galatee.Tools.Utility.ListToDataTable<object>(query);
            }
        }
        public static DataTable ListeDesClientFraisSaisi(List<CsCAMPAGNE> lstCampagne)
        {
            using (galadbEntities context = new galadbEntities())
            {
                List<string> lstidCampagne = new List<string>();
                foreach (CsCAMPAGNE item in lstCampagne)
                    lstidCampagne.Add(item.IDCOUPURE);

                IEnumerable<object> query = (
                 from det in context.DETAILCAMPAGNE
                 where
                 lstidCampagne.Contains(det.IDCOUPURE) && (det.FRAIS != 0 && det.FRAIS != null)
                 select new
                 {
                     det.IDCOUPURE,
                     det.CENTRE,
                     det.CLIENT,
                     det.ORDRE,
                     det.CLIENT1.NOMABON ,
                     det.SOLDEDUE,
                     det.NOMBREFACTURE,
                     det.FRAIS,
                     det.COMPTEUR ,
                     LIBELLECOUPURE= det.TYPECOUPURE.LIBELLE 
                 }).Distinct();
                return Galatee.Tools.Utility.ListToDataTable<object>(query);
            }
        }

        public static DataTable ListeDesMauvaisPayer(List<int> lstIdCentre, DateTime? Datedebut, DateTime? Datefin)
        {
            using (galadbEntities context = new galadbEntities())
            {
                IEnumerable<object> query = (
                 from x in context.MAUVAISPAYEUR 
                 where
                 lstIdCentre.Contains(x.CLIENT.FK_IDCENTRE  ) && x.DATECREATION >= Datedebut  && x.DATECREATION <= Datefin 
                 select new
                 {
                     x.CLIENT.CENTRE ,
                   CLIENT=  x.CLIENT.REFCLIENT ,
                     x.CLIENT.ORDRE ,
                     x.CLIENT.NOMABON 
                 });
                return Galatee.Tools.Utility.ListToDataTable<object>(query);
            }
        }
        public static DataTable ListeDesMoratoiresEmis(List<int> lstIdCentre, DateTime? Datedebut, DateTime? Datefin)
        {
            using (galadbEntities context = new galadbEntities())
            {

                IEnumerable<object> query = (
                 from x in context.MORATOIRE
                 from y in x.LCLIENT
                 join cnl in context.CLIENT on new { CENTRE = x.CENTRE, CLIENT = x.CLIENT, ORDRE = x.ORDRE }
                                                 equals new { cnl.CENTRE, CLIENT = cnl.REFCLIENT, ORDRE = cnl.ORDRE }
                 where
                 lstIdCentre.Contains(x.FK_IDCENTRE.Value) && x.DATECREATION >= Datedebut && x.DATECREATION <= Datefin
                 select new
                 {
                     x.CENTRE,
                     x.CLIENT,
                     x.ORDRE,
                     y.NDOC,
                     y.REFEM,
                     y.CRET,
                     y.MONTANT,
                     FK_IDLCLIENT = y.PK_ID,
                     FK_IDCLIENT = cnl.PK_ID, 
                     cnl.NOMABON,
                     y.EXIGIBILITE,
                     x.STATUS
                 });
                return Galatee.Tools.Utility.ListToDataTable<object>(query);
            }
        }

        public static DataTable ListeDesMoratoiresEmisPrecontentieux(List<int> lstIdCentre, DateTime? Datedebut, DateTime? Datefin)
        {
            using (galadbEntities context = new galadbEntities())
            {

                IEnumerable<object> query = (
                 from x in context.MORATOIRE
                 from y in x.LCLIENT
                 join cnl in context.CLIENT on new { CENTRE = x.CENTRE, CLIENT = x.CLIENT, ORDRE = x.ORDRE }
                                                 equals new { cnl.CENTRE, CLIENT = cnl.REFCLIENT, ORDRE = cnl.ORDRE }
                 where
                 lstIdCentre.Contains(x.FK_IDCENTRE.Value) && x.DATECREATION >= Datedebut && x.DATECREATION <= Datefin && x.ISPRECONTENTIEUX == true 
                 select new
                 {
                     x.CENTRE,
                     x.CLIENT,
                     x.ORDRE,
                     y.NDOC,
                     y.REFEM,
                     y.CRET,
                     y.MONTANT,
                     FK_IDLCLIENT = y.PK_ID,
                     cnl.NOMABON,
                     FK_IDCLIENT =  cnl.PK_ID, 
                     y.EXIGIBILITE,
                     x.STATUS
                 });
                return Galatee.Tools.Utility.ListToDataTable<object>(query);
            }
        }

        public static DataTable ListeDesMoratoiresNonRespecte(List<int> lstIdCentre, DateTime? Datedebut, DateTime? Datefin)
        {
            using (galadbEntities context = new galadbEntities())
            {
                IEnumerable<object> query = (
                 from x in context.DETAILMORATOIRE
                 from y in x.MORATOIRE.LCLIENT 
                 where
                 !context.TRANSCAISB.Any(es => es.FK_IDLCLIENT  == y.PK_ID ) &&
                 lstIdCentre.Contains(x.MORATOIRE.FK_IDCENTRE.Value) && x.DATECREATION >= Datedebut && x.DATECREATION <= Datefin
                 select new
                 {
                     x.MORATOIRE.CENTRE,
                     x.MORATOIRE.CLIENT,
                     x.MORATOIRE.ORDRE,
                     x.REFEM,
                     x.NDOC,
                     x.CRET,
                     x.FK_IDMORATOIRE
                 });
                return Galatee.Tools.Utility.ListToDataTable<object>(query);
            }
        }
        public static DataTable ListeDesMoratoiresRespecte(List<int> lstIdCentre, DateTime? Datedebut, DateTime? Datefin)
        {
            using (galadbEntities context = new galadbEntities())
            {
                IEnumerable<object> query = (
                 from x in context.DETAILMORATOIRE
                 from y in x.MORATOIRE.LCLIENT
                 where
                 context.TRANSCAISB.Any(es => es.FK_IDLCLIENT == y.PK_ID) &&
                 lstIdCentre.Contains(x.MORATOIRE.FK_IDCENTRE.Value) && x.DATECREATION >= Datedebut && x.DATECREATION <= Datefin
                 select new
                 {
                     x.MORATOIRE.CENTRE,
                     x.MORATOIRE.CLIENT,
                     x.MORATOIRE.ORDRE,
                     x.REFEM,
                     x.NDOC,
                     x.CRET,
                     x.FK_IDMORATOIRE
                 });
                return Galatee.Tools.Utility.ListToDataTable<object>(query);
            }
        }


        public static DataTable ListeDesReglementMoratoires(int idfacture)
        {
            using (galadbEntities context = new galadbEntities())
            {
                IEnumerable<object> query = (
                 from x in context.TRANSCAISSE
                 where
                    x.FK_IDLCLIENT == idfacture && string.IsNullOrEmpty(x.TOPANNUL) && x.NDOC != "TIMBRE"
                 select new
                 {
                     x.CENTRE,
                     x.CLIENT,
                     x.ORDRE,
                     x.REFEM,
                     x.NDOC,
                     x.CRET,
                     x.MONTANT,
                 });
                IEnumerable<object> query1 = (
                from x in context.TRANSCAISB
                where
                   x.FK_IDLCLIENT == idfacture && string.IsNullOrEmpty(x.TOPANNUL) && x.NDOC != "TIMBRE"
                select new
                {
                    x.CENTRE,
                    x.CLIENT,
                    x.ORDRE,
                    x.REFEM,
                    x.NDOC,
                    x.CRET,
                    x.MONTANT,
                });
                return Galatee.Tools.Utility.ListToDataTable<object>(query.Union(query1));
            }
        }
        public static DataTable RemplirObservation()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    var observation = from c in context.OBSERVATION 
                                      select c;
                    return Galatee.Tools.Utility.ListToDataTable<object>(observation);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        #region Sylla

        public static DataTable RemplirAffectation( )
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    var Affectation = (from c in context.AFFECTATIONGESTIONAIRE
                            select new
                            {
                              c.FK_IDADMUTILISATEUR ,
                              c.FK_IDREGROUPEMENT ,
                              c.ISACTIVE ,
                              LIBELLEREGROUPEMENT = c.REGROUPEMENT.NOM  ,
                              c.CODECENTRE 
                            });
                    return Galatee.Tools.Utility.ListToDataTable<object>(Affectation);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static bool? SaveAffection(List<CsRegCli> ListRegCliAffecter, int? ID_USER)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    List<AFFECTATIONGESTIONAIRE> LISTAFFECTATIONGESTIONAIRETOINSERT = new List<AFFECTATIONGESTIONAIRE>();
                    List<AFFECTATIONGESTIONAIRE> LISTAFFECTATIONGESTIONAIRETOUPDATE = new List<AFFECTATIONGESTIONAIRE>();
                    List<AFFECTATIONGESTIONAIRE> LISTAFFECTATIONGESTIONAIRETONOUPDATE = new List<AFFECTATIONGESTIONAIRE>();
                    LISTAFFECTATIONGESTIONAIRETOUPDATE = context.AFFECTATIONGESTIONAIRE.Where(a => a.FK_IDADMUTILISATEUR == ID_USER && a.ISACTIVE == true) != null ? context.AFFECTATIONGESTIONAIRE.Where(a => a.FK_IDADMUTILISATEUR == ID_USER && a.ISACTIVE == true).ToList() : null;
                    foreach (var item in ListRegCliAffecter)
                    {
                        if (!LISTAFFECTATIONGESTIONAIRETOUPDATE.Select(a => a.FK_IDREGROUPEMENT).Contains(item.PK_ID))
                        {
                            if (item.LstCentre != null && item.LstCentre.Count != 0)
                            {
                                foreach (string items in item.LstCentre)
                                {
                                    AFFECTATIONGESTIONAIRE AFFECTATIONGESTIONAIRE = new AFFECTATIONGESTIONAIRE();
                                    AFFECTATIONGESTIONAIRE.FK_IDADMUTILISATEUR = ID_USER;
                                    AFFECTATIONGESTIONAIRE.FK_IDREGROUPEMENT = item.PK_ID;
                                    AFFECTATIONGESTIONAIRE.DATECREATION = DateTime.Now;
                                    AFFECTATIONGESTIONAIRE.ISACTIVE = true;
                                    AFFECTATIONGESTIONAIRE.CODECENTRE = items;
                                    LISTAFFECTATIONGESTIONAIRETOINSERT.Add(AFFECTATIONGESTIONAIRE);
                                }
                            }
                            else
                            {
                                AFFECTATIONGESTIONAIRE AFFECTATIONGESTIONAIRE = new AFFECTATIONGESTIONAIRE();
                                AFFECTATIONGESTIONAIRE.FK_IDADMUTILISATEUR = ID_USER;
                                AFFECTATIONGESTIONAIRE.FK_IDREGROUPEMENT = item.PK_ID;
                                AFFECTATIONGESTIONAIRE.DATECREATION = DateTime.Now;
                                AFFECTATIONGESTIONAIRE.ISACTIVE = true;
                                //AFFECTATIONGESTIONAIRE.CODECENTRE = items;
                                LISTAFFECTATIONGESTIONAIRETOINSERT.Add(AFFECTATIONGESTIONAIRE);
                            }
                            
                        }
                        else
                        {
                            var AFFECTATIONGESTIONAIRETONOUPDATE = LISTAFFECTATIONGESTIONAIRETOUPDATE.FirstOrDefault(a => a.FK_IDREGROUPEMENT == item.PK_ID);
                            LISTAFFECTATIONGESTIONAIRETOUPDATE.Remove(AFFECTATIONGESTIONAIRETONOUPDATE);
                        }
                    }

                    foreach (var item in LISTAFFECTATIONGESTIONAIRETOUPDATE)
                    {
                        item.ISACTIVE = false;
                    }
                    Entities.UpdateEntity<AFFECTATIONGESTIONAIRE>(LISTAFFECTATIONGESTIONAIRETOUPDATE, context);
                    Entities.InsertEntity<AFFECTATIONGESTIONAIRE>(LISTAFFECTATIONGESTIONAIRETOINSERT, context);

                    context.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
                return false;
            }
        }
        public static string SaveCampagne(List<CsLclient> ListFacturation, CsRegCli csRegCli, int? ID_USER)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    CAMPAGNESGC camp = GetCampagneToInsertSGC(ListFacturation, csRegCli, ID_USER, context);
                    Entities.InsertEntity<CAMPAGNESGC>(camp, context);
                    context.SaveChanges();
                    return camp.PK_ID.ToString() + "." + camp.NUMEROCAMPAGNE;
                }

            }
            catch (DbEntityValidationException ex)
            {
                GenericEntityExeptionandler(ex);
                return null;
            }

        }


        public static string SaveCampagnePrecontentieux(List<CsDetailCampagnePrecontentieux > ListClient,int? ID_USER,string matricule)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {



                    CAMPAGNEPRECONTENTIEUX camp = new CAMPAGNEPRECONTENTIEUX();
                    camp.DATECREATION = DateTime.Now;
                    camp.DATEMODIFICATION = DateTime.Now;
                    camp.FK_IDMATRICULE = ID_USER.Value;
                    camp.FK_IDCENTRE = ListClient.First().FK_IDCENTRE;
                    camp.CENTRE = ListClient.First().CENTRE;
                    camp.MONTANT = ListClient.Sum(a => a.SOLDEDUE );
                    camp.IDCOUPURE = RetourneIdCampagnePrecontantieux(camp.CENTRE,System.DateTime.Today.Year.ToString() );
                    camp.USERCREATION = matricule ;
                    camp.USERMODIFICATION = matricule;

                    List<DETAILCAMPAGNEPRENCONTENTIEUX> lstDetail = Galatee.Tools.Utility.ConvertListType<DETAILCAMPAGNEPRENCONTENTIEUX, CsDetailCampagnePrecontentieux>(ListClient);
                    lstDetail.ForEach(t => t.USERCREATION = matricule);
                    lstDetail.ForEach(t => t.DATECREATION = System.DateTime.Now);
                    lstDetail.ForEach(t => t.IDCAMPAGNE = camp.IDCOUPURE);
                    camp.DETAILCAMPAGNEPRENCONTENTIEUX = lstDetail;
                    Entities.InsertEntity<CAMPAGNEPRECONTENTIEUX>(camp, context);
                    context.SaveChanges();
                    return camp.PK_ID.ToString() + "." + camp.IDCOUPURE;
                }

            }
            catch (DbEntityValidationException ex)
            {
                GenericEntityExeptionandler(ex);
                return null;
            }

        }
        public static string RetourneIdCampagnePrecontantieux(string Centre,string Prefixe)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    var exist = context.CAMPAGNEPRECONTENTIEUX.FirstOrDefault(l => l.CENTRE == Centre && l.IDCOUPURE.Substring(0, 4) == Prefixe);

                    string suffixe = string.Empty;
                    if (exist == null)
                        suffixe = "0000001";
                    else
                    {
                        Int64? maxsuffixe = Int64.Parse(exist.IDCOUPURE.Substring(8,7));

                        if (maxsuffixe.ToString().Length == 1)
                        {
                            suffixe = (maxsuffixe + 1).Value.ToString("0000000");
                        }
                        else
                            return suffixe = (maxsuffixe + 1).Value.ToString("0000000");

                    }

                    return System.DateTime.Today.Year.ToString() + System.DateTime.Today.Month.ToString("00") + Centre  + suffixe;


                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public static List<Dico> SaveCampane(List<CsLclient> ListFacturation, CsRegCli csRegCli, int? ID_USER)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    CAMPAGNEGC camp = GetCampagneToInsert(ListFacturation, csRegCli, ID_USER, context);

                    List<CAMPAGNEGC> ListCampagneToUpdate = context.CAMPAGNEGC.Where(c => c.REGROUPEMENT == csRegCli.CODE && c.STATUS == "1") != null ? context.CAMPAGNEGC.Where(c => c.REGROUPEMENT == csRegCli.CODE && c.STATUS == "1").ToList() : null;

                    foreach (var item in ListCampagneToUpdate)
                    {
                        item.STATUS = "0";
                    }
                    Entities.UpdateEntity<CAMPAGNEGC>(ListCampagneToUpdate, context);

                    Entities.InsertEntity<CAMPAGNEGC>(camp, context);

                    context.SaveChanges();
                    List<Dico> Result = new List<Dico>();

                    List<CsDetailCampagneGc> DETAILCAMPAGNEGC_ = new List<CsDetailCampagneGc>();
                    foreach (var item in ListFacturation)
                    {
                        CsDetailCampagneGc detailCamp = new CsDetailCampagneGc();
                        detailCamp.FK_IDCLIENT = item.FK_IDCLIENT;
                        detailCamp.FK_IDLCLIENT = item.PK_ID;
                        detailCamp.CENTRE = item.CENTRE;
                        detailCamp.CLIENT = item.CLIENT;
                        detailCamp.DATECREATION = DateTime.Now;
                        detailCamp.DATEMODIFICATION = DateTime.Now;
                        detailCamp.MONTANT = item.SOLDEFACTURE;
                        detailCamp.NDOC = item.NDOC;
                        detailCamp.ORDRE = item.ORDRE;
                        detailCamp.PERIODE = item.REFEM;
                        detailCamp.STATUS = "1";
                        detailCamp.USERCREATION = context.ADMUTILISATEUR.FirstOrDefault(a => a.PK_ID == ID_USER).MATRICULE;
                        detailCamp.USERMODIFICATION = context.ADMUTILISATEUR.FirstOrDefault(a => a.PK_ID == ID_USER).MATRICULE;
                        detailCamp.IDCAMPAGNEGC = camp.PK_ID;
                        DETAILCAMPAGNEGC_.Add(detailCamp);
                    }
                    Result.Add(new Dico { Key = camp.PK_ID.ToString() + "." + camp.NUMEROCAMPAGNE, Valeur = DETAILCAMPAGNEGC_ });
                    return Result;
                }

            }
            catch (DbEntityValidationException ex)
            {
                GenericEntityExeptionandler(ex);
                return null;
            }

        }

        private static CAMPAGNESGC GetCampagneToInsertSGC(List<CsLclient> ListFacturation, CsRegCli csRegCli, int? ID_USER, galadbEntities context)
        {
            string client = string.Empty;
            try
            {



                string IdCoupure = System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString("00") +
                                  System.DateTime.Now.Year.ToString() + csRegCli.CODE;


                CAMPAGNESGC camp = new CAMPAGNESGC();
                camp.DATECREATION = DateTime.Now;
                camp.DATEMODIFICATION = DateTime.Now;
                camp.FK_IDMATRICULE = ID_USER.Value;
                camp.FK_IDREGROUPEMENT = csRegCli.PK_ID;
                camp.MATRICULECREATEURCAMPAGNE = context.ADMUTILISATEUR.FirstOrDefault(a => a.PK_ID == ID_USER).MATRICULE;
                camp.MATRICULEGESTIONNAIRE = context.ADMUTILISATEUR.FirstOrDefault(a => a.PK_ID == ID_USER).MATRICULE;
                camp.MONTANT = ListFacturation.Sum(a => a.SOLDEFACTURE);
                camp.STATUS = "1";
                camp.NUMEROCAMPAGNE = DateTime.Now.Day.ToString("00") + DateTime.Now.Month.ToString("00") + DateTime.Now.Year.ToString() + DateTime.Now.Minute.ToString("00") + DateTime.Now.Second.ToString("00");
                camp.USERCREATION = context.ADMUTILISATEUR.FirstOrDefault(a => a.PK_ID == ID_USER).MATRICULE;
                camp.USERMODIFICATION = context.ADMUTILISATEUR.FirstOrDefault(a => a.PK_ID == ID_USER).MATRICULE;
                camp.REGROUPEMENT = csRegCli.CODE;
                
                foreach (var item in ListFacturation)
                {
                    client = item.CLIENT;
                    DETAILCAMPAGNESGC detailCamp = new DETAILCAMPAGNESGC();
                    detailCamp.CENTRE = item.CENTRE;
                    detailCamp.CLIENT = item.CLIENT;
                    detailCamp.CLIENT = item.CLIENT;
                    detailCamp.FK_IDLCLIENT = item.FK_IDLCLIENT.Value   ;
                    detailCamp.FK_IDCLIENT = item.FK_IDCLIENT;
                    detailCamp.DATECREATION = DateTime.Now;
                    detailCamp.DATEMODIFICATION = DateTime.Now;
                    detailCamp.MONTANT = item.SOLDEFACTURE;
                    detailCamp.NDOC = item.NDOC;
                    detailCamp.ORDRE = item.ORDRE;
                    detailCamp.PERIODE = item.REFEM;
                    detailCamp.STATUS = "1";
                    detailCamp.USERCREATION = context.ADMUTILISATEUR.FirstOrDefault(a => a.PK_ID == ID_USER).MATRICULE;
                    detailCamp.USERMODIFICATION = context.ADMUTILISATEUR.FirstOrDefault(a => a.PK_ID == ID_USER).MATRICULE;
                    detailCamp.CAMPAGNESGC = camp;
                    detailCamp.IDCAMPAGNEGC = camp.PK_ID;

                    camp.DETAILCAMPAGNESGC.Add(detailCamp);
                }
                return camp;
            }
            catch (Exception ex)
            {
                string ll = client;
                throw;
            }
        }
        private static CAMPAGNEGC GetCampagneToInsert(List<CsLclient> ListFacturation, CsRegCli csRegCli, int? ID_USER, galadbEntities context)
        {
            CAMPAGNEGC camp = new CAMPAGNEGC();
            camp.DATECREATION = DateTime.Now;
            camp.DATEMODIFICATION = DateTime.Now;
            camp.FK_IDMATRICULE = ID_USER.Value;
            camp.FK_IDREGROUPEMENT = csRegCli.PK_ID;
            camp.MATRICULECREATEURCAMPAGNE = context.ADMUTILISATEUR.FirstOrDefault(a => a.PK_ID == ID_USER).MATRICULE;
            camp.MATRICULEGESTIONNAIRE = context.ADMUTILISATEUR.FirstOrDefault(a => a.PK_ID == ID_USER).MATRICULE;
            camp.MONTANT = ListFacturation.Sum(a => a.SOLDEFACTURE );
            camp.STATUS = "1";
            camp.NUMEROCAMPAGNE = DateTime.Now.Day.ToString("00") + DateTime.Now.Month.ToString("00") + DateTime.Now.Year.ToString() + DateTime.Now.Minute.ToString("00") + DateTime.Now.Second.ToString("00");
            camp.USERCREATION = context.ADMUTILISATEUR.FirstOrDefault(a => a.PK_ID == ID_USER).MATRICULE;
            camp.USERMODIFICATION = context.ADMUTILISATEUR.FirstOrDefault(a => a.PK_ID == ID_USER).MATRICULE;
            camp.REGROUPEMENT = csRegCli.CODE;

            foreach (var item in ListFacturation)
            {
                DETAILCAMPAGNEGC detailCamp = new DETAILCAMPAGNEGC();
                detailCamp.FK_IDCLIENT = item.FK_IDCLIENT;
                detailCamp.FK_IDLCLIENT  = item.PK_ID ;
                detailCamp.CENTRE = item.CENTRE;
                detailCamp.CLIENT = item.CLIENT;
                detailCamp.DATECREATION = DateTime.Now;
                detailCamp.DATEMODIFICATION = DateTime.Now;
                detailCamp.MONTANT = item.SOLDEFACTURE ;
                detailCamp.NDOC = item.NDOC;
                detailCamp.ORDRE = item.ORDRE;
                detailCamp.PERIODE = item.REFEM;
                detailCamp.STATUS = "1";
                detailCamp.USERCREATION = context.ADMUTILISATEUR.FirstOrDefault(a => a.PK_ID == ID_USER).MATRICULE;
                detailCamp.USERMODIFICATION = context.ADMUTILISATEUR.FirstOrDefault(a => a.PK_ID == ID_USER).MATRICULE;
                detailCamp.CAMPAGNEGC = camp;
                detailCamp.IDCAMPAGNEGC = camp.PK_ID;
                camp.DETAILCAMPAGNEGC.Add(detailCamp);
            }
            return camp;
        }
        public static void GenericEntityExeptionandler(DbEntityValidationException ex)
        {
            // Retrieve the error messages as a list of strings.
            var errorMessages = ex.EntityValidationErrors
                    .SelectMany(x => x.ValidationErrors)
                    .Select(x => x.ErrorMessage);

            // Join the list to a single string.
            var fullErrorMessage = string.Join("; ", errorMessages);

            // Combine the original exception message with the new one.
            var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

            // Throw a new DbEntityValidationException with the improved exception message.
            throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
        }

        public static CsCampagneGc VerifierCampagneExiste(CsRegCli csRegCli, string periode)
        {

            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    var detailcamp = context.DETAILCAMPAGNEGC.FirstOrDefault(c => c.CAMPAGNEGC.REGROUPEMENT == csRegCli.CODE && c.CAMPAGNEGC.STATUS == "1" && c.PERIODE == periode);
                    CsCampagneGc MyCamp = null;
                    if (detailcamp != null)
                    {
                        MyCamp = new CsCampagneGc();
                        var camp = detailcamp.CAMPAGNEGC;

                        MyCamp.DATECREATION = camp.DATECREATION;
                        MyCamp.DATEMODIFICATION = camp.DATEMODIFICATION;
                        MyCamp.FK_IDMATRICULE = camp.FK_IDMATRICULE;
                        MyCamp.FK_IDREGROUPEMENT = camp.FK_IDREGROUPEMENT;
                        MyCamp.MATRICULECREATEURCAMPAGNE = camp.MATRICULECREATEURCAMPAGNE;
                        MyCamp.MATRICULEGESTIONNAIRE = camp.MATRICULEGESTIONNAIRE;
                        MyCamp.MONTANT = camp.MONTANT;
                        MyCamp.STATUS = camp.STATUS;
                        MyCamp.NUMEROCAMPAGNE = camp.NUMEROCAMPAGNE;
                        MyCamp.USERCREATION = camp.USERCREATION;
                        MyCamp.USERMODIFICATION = camp.USERMODIFICATION;
                        MyCamp.REGROUPEMENT = camp.REGROUPEMENT;

                        MyCamp.DETAILCAMPAGNEGC_ = new List<CsDetailCampagneGc>();
                        foreach (var item in camp.DETAILCAMPAGNEGC)
                        {
                            CsDetailCampagneGc detailCamp = new CsDetailCampagneGc();
                            detailCamp.CENTRE = item.CENTRE;
                            detailCamp.CLIENT = item.CLIENT;
                            detailCamp.DATECREATION = DateTime.Now;
                            detailCamp.DATEMODIFICATION = DateTime.Now;
                            detailCamp.MONTANT = item.MONTANT;
                            detailCamp.NDOC = item.NDOC;
                            detailCamp.ORDRE = item.ORDRE;
                            detailCamp.PERIODE = item.PERIODE;
                            detailCamp.STATUS = "1";
                            detailCamp.USERCREATION = item.USERCREATION;
                            detailCamp.USERMODIFICATION = item.USERMODIFICATION;
                            detailCamp.IDCAMPAGNEGC = camp.PK_ID;
                            var CLIENT = context.CLIENT.FirstOrDefault(a => a.CENTRE == item.CENTRE && a.REFCLIENT == item.CLIENT && a.ORDRE == item.ORDRE);
                            detailCamp.NOM = CLIENT != null ? CLIENT.NOMABON : string.Empty;

                            MyCamp.DETAILCAMPAGNEGC_.Add(detailCamp);
                        }
                    }
                    return MyCamp;
                }
            }
            catch (DbEntityValidationException ex)
            {
                GenericEntityExeptionandler(ex);
                return null;
            }
        }
        public static List<CsCampagneGc> RetournCampagneByRegcli(CsRegCli csRegCli, string periode)
        {

            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    var Listdetailcamp = context.DETAILCAMPAGNEGC.Where(c => c.CAMPAGNEGC.REGROUPEMENT == csRegCli.CODE && c.CAMPAGNEGC.STATUS == "1" && c.PERIODE == periode).Distinct();
                    List<CsCampagneGc> ListCampagne = new List<CsCampagneGc>();

                    foreach (var detailcamp in Listdetailcamp)
                    {
                        CsCampagneGc MyCamp = null;
                        if (detailcamp != null && detailcamp.CAMPAGNEGC.STATUS != "0")
                        {
                            MyCamp = new CsCampagneGc();
                            var camp = detailcamp.CAMPAGNEGC;

                            MyCamp.DATECREATION = camp.DATECREATION;
                            MyCamp.LIBELLEREGROUPEMENT = context.REGROUPEMENT.FirstOrDefault(r => r.PK_ID == camp.FK_IDREGROUPEMENT).NOM;
                            MyCamp.DATEMODIFICATION = camp.DATEMODIFICATION;
                            MyCamp.FK_IDMATRICULE = camp.FK_IDMATRICULE;
                            MyCamp.FK_IDREGROUPEMENT = camp.FK_IDREGROUPEMENT;
                            MyCamp.MATRICULECREATEURCAMPAGNE = camp.MATRICULECREATEURCAMPAGNE;
                            MyCamp.MATRICULEGESTIONNAIRE = camp.MATRICULEGESTIONNAIRE;
                            MyCamp.MONTANT = camp.MONTANT;
                            MyCamp.STATUS = camp.STATUS;
                            MyCamp.NUMEROCAMPAGNE = camp.NUMEROCAMPAGNE;
                            MyCamp.USERCREATION = camp.USERCREATION;
                            MyCamp.USERMODIFICATION = camp.USERMODIFICATION;
                            MyCamp.REGROUPEMENT = camp.REGROUPEMENT;
                            MyCamp.PK_ID = camp.PK_ID;

                            MyCamp.DETAILCAMPAGNEGC_ = new List<CsDetailCampagneGc>();
                            foreach (var item in camp.DETAILCAMPAGNEGC)
                            {
                                CsDetailCampagneGc detailCamp = new CsDetailCampagneGc();
                                detailCamp.CENTRE = item.CENTRE;
                                detailCamp.CLIENT = item.CLIENT;
                                detailCamp.DATECREATION = DateTime.Now;
                                detailCamp.DATEMODIFICATION = DateTime.Now;
                                detailCamp.MONTANT = item.MONTANT;
                                detailCamp.NDOC = item.NDOC;
                                detailCamp.ORDRE = item.ORDRE;
                                detailCamp.PERIODE = item.PERIODE;
                                detailCamp.STATUS = "1";
                                detailCamp.USERCREATION = item.USERCREATION;
                                detailCamp.USERMODIFICATION = item.USERMODIFICATION;
                                detailCamp.IDCAMPAGNEGC = camp.PK_ID;
                                var CLIENT = context.CLIENT.FirstOrDefault(a => a.CENTRE == item.CENTRE && a.REFCLIENT == item.CLIENT && a.ORDRE == item.ORDRE);
                                detailCamp.NOM = CLIENT != null ? CLIENT.NOMABON : string.Empty;

                                MyCamp.DETAILCAMPAGNEGC_.Add(detailCamp);
                            }
                            if (!ListCampagne.Select(c => c.PK_ID).Contains(MyCamp.PK_ID))
                            {
                                ListCampagne.Add(MyCamp);
                            }
                        }
                    }
                    return ListCampagne;
                }
            }
            catch (DbEntityValidationException ex)
            {
                GenericEntityExeptionandler(ex);
                return null;
            }
        }

        public static List<CsCampagneGc> RemplirCampagne(string Matricule)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    List<CsCampagneGc> ListeCampagne = new List<CsCampagneGc>();
                    var CampagneGc = from c in context.CAMPAGNEGC
                                     where c.MATRICULEGESTIONNAIRE == Matricule && c.STATUS == "1".Trim()
                                     select c;
                    if (CampagneGc != null)
                    {
                        foreach (var camp in CampagneGc)
                        {
                            CsCampagneGc MyCamp = new CsCampagneGc();

                            MyCamp.DATECREATION = camp.DATECREATION;
                            MyCamp.DATEMODIFICATION = camp.DATEMODIFICATION;
                            MyCamp.FK_IDMATRICULE = camp.FK_IDMATRICULE;
                            MyCamp.FK_IDREGROUPEMENT = camp.FK_IDREGROUPEMENT;
                            MyCamp.MATRICULECREATEURCAMPAGNE = camp.MATRICULECREATEURCAMPAGNE;
                            MyCamp.MATRICULEGESTIONNAIRE = camp.MATRICULEGESTIONNAIRE;
                            MyCamp.MONTANT = camp.MONTANT;
                            MyCamp.STATUS = camp.STATUS;
                            MyCamp.NUMEROCAMPAGNE = camp.NUMEROCAMPAGNE;
                            MyCamp.USERCREATION = camp.USERCREATION;
                            MyCamp.USERMODIFICATION = camp.USERMODIFICATION;
                            MyCamp.REGROUPEMENT = camp.REGROUPEMENT;
                            MyCamp.LIBELLEREGROUPEMENT = camp.REGROUPEMENT1.NOM;
                            MyCamp.PK_ID = camp.PK_ID;

                            MyCamp.DETAILCAMPAGNEGC_ = new List<CsDetailCampagneGc>();
                            foreach (var item in camp.DETAILCAMPAGNEGC)
                            {
                                CsDetailCampagneGc detailCamp = new CsDetailCampagneGc();
                                detailCamp.CENTRE = item.CENTRE;
                                detailCamp.CLIENT = item.CLIENT;
                                detailCamp.DATECREATION = DateTime.Now;
                                detailCamp.DATEMODIFICATION = DateTime.Now;
                                detailCamp.MONTANT = item.MONTANT;
                                detailCamp.NDOC = item.NDOC;
                                detailCamp.ORDRE = item.ORDRE;
                                detailCamp.PERIODE = item.PERIODE;
                                detailCamp.STATUS = "1";
                                detailCamp.USERCREATION = item.USERCREATION;
                                detailCamp.USERMODIFICATION = item.USERMODIFICATION;
                                detailCamp.IDCAMPAGNEGC = camp.PK_ID;
                                var CLIENT = context.CLIENT.FirstOrDefault(a => a.CENTRE == item.CENTRE && a.REFCLIENT == item.CLIENT && a.ORDRE == item.ORDRE);
                                detailCamp.NOM = CLIENT != null ? CLIENT.NOMABON : string.Empty;
                                detailCamp.PK_ID = item.PK_ID;

                                MyCamp.DETAILCAMPAGNEGC_.Add(detailCamp);
                            }

                            MyCamp.MANDATEMENTGC_ = new List<CsMandatementGc>();

                            foreach (var Mand in camp.MANDATEMENTGC)
                            {
                                CsMandatementGc MandatementCamp = new CsMandatementGc();

                                MandatementCamp.DATECREATION = DateTime.Now;
                                MandatementCamp.DATEMODIFICATION = DateTime.Now;
                                MandatementCamp.MONTANT = Mand.MONTANT;
                                MandatementCamp.USERCREATION = Mand.USERCREATION;
                                MandatementCamp.USERMODIFICATION = Mand.USERMODIFICATION;
                                MandatementCamp.FK_IDCAMPAGNA = camp.PK_ID;
                                MandatementCamp.PK_ID = Mand.PK_ID;
                                MandatementCamp.NUMEROMANDATEMENT = Mand.NUMEROMANDATEMENT;

                                MandatementCamp.DETAILMANDATEMENTGC_ = new List<CsDetailMandatementGc>();
                                foreach (var item in Mand.DETAILMANDATEMENTGC)
                                {
                                    CsDetailMandatementGc detailmand = new CsDetailMandatementGc();
                                    detailmand.CENTRE = item.CENTRE;
                                    detailmand.CLIENT = item.CLIENT;
                                    detailmand.DATECREATION = DateTime.Now;
                                    detailmand.DATEMODIFICATION = DateTime.Now;
                                    detailmand.MONTANT = item.MONTANT;
                                    detailmand.NDOC = item.NDOC;
                                    detailmand.ORDRE = item.ORDRE;
                                    detailmand.PERIODE = item.PERIODE;
                                    detailmand.STATUS = "1";
                                    detailmand.USERCREATION = item.USERCREATION;
                                    detailmand.USERMODIFICATION = item.USERMODIFICATION;
                                    detailmand.FK_IDMANDATEMENT = Mand.PK_ID;

                                    MandatementCamp.DETAILMANDATEMENTGC_.Add(detailmand);
                                }



                                MandatementCamp.PAIEMENTGC_ = new List<CsPaiementGc>();
                                foreach (var ItemMand in Mand.PAIEMENTCAMPAGNEGC)
                                {
                                    CsPaiementGc PaiementMand = new CsPaiementGc();

                                    PaiementMand.DATECREATION = DateTime.Now;
                                    PaiementMand.DATEMODIFICATION = DateTime.Now;
                                    PaiementMand.MONTANT = ItemMand.MONTANT;
                                    PaiementMand.USERCREATION = ItemMand.USERCREATION;
                                    PaiementMand.USERMODIFICATION = ItemMand.USERMODIFICATION;
                                    PaiementMand.FK_IDMANDATEMANT = camp.PK_ID;
                                    PaiementMand.PK_ID = ItemMand.PK_ID;


                                    PaiementMand.DETAILCAMPAGNEGC_ = new List<CsDetailPaiementGc>();
                                    foreach (var item in ItemMand.DETAILPAIEMENTCAMPAGNEGC)
                                    {
                                        CsDetailPaiementGc detailmand = new CsDetailPaiementGc();
                                        detailmand.CENTRE = item.CENTRE;
                                        detailmand.CLIENT = item.CLIENT;
                                        detailmand.DATECREATION = DateTime.Now;
                                        detailmand.DATEMODIFICATION = DateTime.Now;
                                        detailmand.MONTANT = item.MONTANT;
                                        detailmand.NDOC = item.NDOC;
                                        detailmand.ORDRE = item.ORDRE;
                                        detailmand.PERIODE = item.PERIODE;
                                        detailmand.STATUS = "1";
                                        detailmand.USERCREATION = item.USERCREATION;
                                        detailmand.USERMODIFICATION = item.USERMODIFICATION;
                                        detailmand.FK_IDPAIEMENTCAMPAGNEGC = Mand.PK_ID;


                                        PaiementMand.DETAILCAMPAGNEGC_.Add(detailmand);
                                    }



                                    MandatementCamp.PAIEMENTGC_.Add(PaiementMand);
                                }



                                MandatementCamp.PAIEMENTGC_ = new List<CsPaiementGc>();
                                foreach (var ItemMand in Mand.PAIEMENTCAMPAGNEGC)
                                {
                                    CsPaiementGc PaiementMand = new CsPaiementGc();

                                    PaiementMand.DATECREATION = DateTime.Now;
                                    PaiementMand.DATEMODIFICATION = DateTime.Now;
                                    PaiementMand.MONTANT = ItemMand.MONTANT;
                                    PaiementMand.USERCREATION = ItemMand.USERCREATION;
                                    PaiementMand.USERMODIFICATION = ItemMand.USERMODIFICATION;
                                    PaiementMand.FK_IDMANDATEMANT = camp.PK_ID;
                                    PaiementMand.PK_ID = ItemMand.PK_ID;


                                    PaiementMand.DETAILCAMPAGNEGC_ = new List<CsDetailPaiementGc>();
                                    foreach (var item in ItemMand.DETAILPAIEMENTCAMPAGNEGC)
                                    {
                                        CsDetailPaiementGc detailmand = new CsDetailPaiementGc();
                                        detailmand.CENTRE = item.CENTRE;
                                        detailmand.CLIENT = item.CLIENT;
                                        detailmand.DATECREATION = DateTime.Now;
                                        detailmand.DATEMODIFICATION = DateTime.Now;
                                        detailmand.MONTANT = item.MONTANT;
                                        detailmand.NDOC = item.NDOC;
                                        detailmand.ORDRE = item.ORDRE;
                                        detailmand.PERIODE = item.PERIODE;
                                        detailmand.STATUS = "1";
                                        detailmand.USERCREATION = item.USERCREATION;
                                        detailmand.USERMODIFICATION = item.USERMODIFICATION;
                                        detailmand.FK_IDPAIEMENTCAMPAGNEGC = Mand.PK_ID;


                                        PaiementMand.DETAILCAMPAGNEGC_.Add(detailmand);
                                    }



                                    MandatementCamp.PAIEMENTGC_.Add(PaiementMand);
                                }



                                MyCamp.MANDATEMENTGC_.Add(MandatementCamp);
                            }



                            MyCamp.DETAILCAMPAGNEGC_ = MyCamp.DETAILCAMPAGNEGC_.OrderBy(d => d.NOM).ToList();
                            ListeCampagne.Add(MyCamp);
                        }
                    }
                    return ListeCampagne;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static CsCampagneGc RemplirCampagneById(int IdCampagne, string Statut)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    CsCampagneGc LaCampagne = new CsCampagneGc();
                    List<CsDetailCampagneGc> leDetail = new List<CsDetailCampagneGc>();
                    List<CsMandatementGc> leMandatement = new List<CsMandatementGc>();
                    CAMPAGNEGC CampagneGc = context.CAMPAGNEGC.FirstOrDefault(t => t.PK_ID == IdCampagne && t.STATUS == Statut);
                    //var NDOCS = CampagneGc.DETAILCAMPAGNEGC.Select(t => t.NDOC);
                    //var LCLIENTS = context.LCLIENT.Where(t => NDOCS.Contains(t.NDOC));
                    if (CampagneGc != null && CampagneGc.FK_IDREGROUPEMENT != 0)
                    {
                        LaCampagne = Galatee.Tools.Utility.ConvertEntity<CsCampagneGc, CAMPAGNEGC>(CampagneGc);
                        LaCampagne.LIBELLEREGROUPEMENT = context.REGROUPEMENT.FirstOrDefault(t => t.PK_ID == LaCampagne.FK_IDREGROUPEMENT).NOM;

                        if (CampagneGc.DETAILCAMPAGNEGC != null && CampagneGc.DETAILCAMPAGNEGC.Count != 0)
                        {
                            leDetail = Galatee.Tools.Utility.ConvertListType<CsDetailCampagneGc, DETAILCAMPAGNEGC>(CampagneGc.DETAILCAMPAGNEGC.ToList());
                            LaCampagne.DETAILCAMPAGNEGC_ = leDetail;
                            foreach (var item in leDetail)
                            {
                                item.NOM = context.CLIENT.FirstOrDefault(t => t.PK_ID == item.FK_IDCLIENT).NOMABON;
                                //var MONTANTTVA = LCLIENTS.FirstOrDefault(t => t.NDOC == item.NDOC && t.CENTRE == item.CENTRE && t.CLIENT == item.CLIENT && t.ORDRE == item.ORDRE).MONTANTTVA;
                                var MONTANTTVA = context.LCLIENT.FirstOrDefault(t => t.PK_ID == item.FK_IDLCLIENT).MONTANTTVA;
                                item.MONTANTTVA = MONTANTTVA != null ? MONTANTTVA : 0;
                                item.MONTANTHT = item.MONTANT - item.MONTANTTVA;
                            }
                        }
                        if (CampagneGc.MANDATEMENTGC != null && CampagneGc.DETAILCAMPAGNEGC.Count != 0)
                        {
                            List<CsMandatementGc> lstMandat = new List<CsMandatementGc>();
                            foreach (MANDATEMENTGC item in CampagneGc.MANDATEMENTGC)
                            {
                                CsMandatementGc leMandat = Galatee.Tools.Utility.ConvertEntity<CsMandatementGc, MANDATEMENTGC>(item);
                                leMandat.DETAILMANDATEMENTGC_ = Galatee.Tools.Utility.ConvertListType<CsDetailMandatementGc, DETAILMANDATEMENTGC>(item.DETAILMANDATEMENTGC.ToList());

                                List<CsPaiementGc> lstPAiement = new List<CsPaiementGc>();

                                foreach (PAIEMENTCAMPAGNEGC paiemnt in item.PAIEMENTCAMPAGNEGC)
                                {
                                    CsPaiementGc lePAiemnet = Galatee.Tools.Utility.ConvertEntity<CsPaiementGc, PAIEMENTCAMPAGNEGC>(paiemnt);
                                    lePAiemnet.DETAILCAMPAGNEGC_ = Galatee.Tools.Utility.ConvertListType<CsDetailPaiementGc, DETAILPAIEMENTCAMPAGNEGC>(paiemnt.DETAILPAIEMENTCAMPAGNEGC.ToList());
                                    lstPAiement.Add(lePAiemnet);
                                }
                                leMandat.PAIEMENTGC_ = lstPAiement;
                                lstMandat.Add(leMandat);
                            }
                            LaCampagne.MANDATEMENTGC_ = lstMandat;
                        }
                    }
                    return LaCampagne;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        
        public static DataTable RechercheDetaileCampane(string numerocamp)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    var _lstDetPaiement = context.CAMPAGNEGC ;
                    IEnumerable<object> query = (from y in _lstDetPaiement 
                                                 from x in y.DETAILCAMPAGNEGC 
                                                 where y.NUMEROCAMPAGNE == numerocamp
                                                 select new
                                                 {
                                                     x.CENTRE,
                                                     x.CLIENT,
                                                     x.ORDRE,
                                                     REFEM = x.PERIODE,
                                                     NOM=  x.CLIENT1.NOMABON ,
                                                     x.NDOC,
                                                     x.STATUS,
                                                     x.MONTANT,
                                                     x.PK_ID,
                                                     x.DATECREATION,
                                                     x.DATEMODIFICATION,
                                                     x.USERCREATION,
                                                     x.USERMODIFICATION
                                                 });
                    return Galatee.Tools.Utility.ListToDataTable<object>(query);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static DataTable RechercheCampane(string numerocamp)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    var _lstCampagne = context.CAMPAGNEGC.ToList();

                    if (!string.IsNullOrWhiteSpace(numerocamp))
                    {
                        _lstCampagne = _lstCampagne.Where(c => c.NUMEROCAMPAGNE == numerocamp) != null ? _lstCampagne.Where(c => c.NUMEROCAMPAGNE == numerocamp).ToList() : null;
                    }
                    IEnumerable<object> query = (from x in _lstCampagne
                                                 //where ( x.NUMEROCAMPAGNE == numerocamp)
                                                 select new
                                                 {
                                                     x.FK_IDREGROUPEMENT,
                                                     x.PK_ID,
                                                     x.FK_IDMATRICULE,
                                                     x.NUMEROCAMPAGNE,
                                                     x.MATRICULECREATEURCAMPAGNE
                                                 }).Distinct();
                    return Galatee.Tools.Utility.ListToDataTable<object>(query);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public static DataTable RechercheMandemande(string numeroMandatement)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    var _lstDetMandatement = context.MANDATEMENTGC .ToList();
                    IEnumerable<object> query = (from y in _lstDetMandatement
                                                 from x in y.DETAILMANDATEMENTGC
                                                 from z in y.CAMPAGNEGC.DETAILCAMPAGNEGC 
                                                 where y.NUMEROMANDATEMENT   == numeroMandatement 
                                                 select new
                                                 {
                                                     x.CENTRE,
                                                     x.CLIENT,
                                                     x.ORDRE,
                                                     REFEM=  x.PERIODE,
                                                     NOM = z.CLIENT1.NOMABON ,
                                                     x.NDOC,
                                                     x.STATUS,
                                                     x.MONTANT,
                                                     x.PK_ID,
                                                     x.DATECREATION,
                                                     x.DATEMODIFICATION,
                                                     x.USERCREATION,
                                                     x.USERMODIFICATION,
                                                     x.MONTANTTVA
                                                 }).Distinct();
                    return Galatee.Tools.Utility.ListToDataTable<object>(query);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static DataTable RecherchePaiement(string AvisDeCrit)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    var _lstDetMandatement = context.PAIEMENTCAMPAGNEGC .ToList();
                    IEnumerable<object> query = (from y in _lstDetMandatement
                                                 from x in y.DETAILPAIEMENTCAMPAGNEGC 
                                                 from z in y.MANDATEMENTGC.CAMPAGNEGC.DETAILCAMPAGNEGC
                                                 where y.NumAvisCredit == AvisDeCrit
                                                 select new
                                                 {
                                                     x.CENTRE,
                                                     x.CLIENT,
                                                     x.ORDRE,
                                                     REFEM = x.PERIODE,
                                                     NOM = z.CLIENT1.NOMABON,
                                                     x.NDOC,
                                                     x.MONTANT,
                                                     x.DATECREATION,
                                                     x.USERCREATION,
                                                 }).Distinct();
                    return Galatee.Tools.Utility.ListToDataTable<object>(query);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static DataTable RechercheMiseAJour(string AvisDeCrit)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    var _lstDetPaiement = context.PAIEMENTCAMPAGNEGC .ToList();
                    var _lstTranscaisse = context.TRANSCAISB.ToList();
                    IEnumerable<object> query = (from y in _lstDetPaiement
                                                 from x in y.DETAILPAIEMENTCAMPAGNEGC
                                                 from z in x.PAIEMENTCAMPAGNEGC.MANDATEMENTGC.CAMPAGNEGC.DETAILCAMPAGNEGC
                                                 join t in _lstTranscaisse on new {x.CENTRE ,x.CLIENT ,x.ORDRE ,x.PERIODE ,x.NDOC } equals
                                                 new {t.CENTRE ,t.CLIENT ,t.ORDRE ,PERIODE =t.REFEM,t.NDOC } 
                                                 where x.PAIEMENTCAMPAGNEGC.NumAvisCredit == AvisDeCrit 
                                                 select new
                                                 {
                                                     x.FK_IDPAIEMENTCAMPAGNEGC,
                                                     x.CENTRE,
                                                     x.CLIENT,
                                                     x.ORDRE,
                                                     REFEM=   x.PERIODE,
                                                     NOM=z.CLIENT1.NOMABON ,
                                                     x.NDOC,
                                                     x.STATUS,
                                                     y.MONTANT,
                                                     x.PK_ID,
                                                     x.DATECREATION,
                                                     x.DATEMODIFICATION,
                                                     x.USERCREATION,
                                                     x.USERMODIFICATION
                                                 });
                    return Galatee.Tools.Utility.ListToDataTable<object>(query);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static bool? SavePaiement(List<CsPaiementGc> ListMandatementGc, CsDetailMandatementGc Facture_Payer_Partiellement)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    List<PAIEMENTCAMPAGNEGC> ListMand = new List<PAIEMENTCAMPAGNEGC>();

                    foreach (var item in ListMandatementGc)
                    {
                        PAIEMENTCAMPAGNEGC Mand = new PAIEMENTCAMPAGNEGC();

                        Mand.PK_ID = item.PK_ID;
                        Mand.DATECREATION = item.DATECREATION;
                        Mand.DATEMODIFICATION = item.DATEMODIFICATION;
                        Mand.FK_IDMANDATEMANT = item.FK_IDMANDATEMANT;
                        Mand.MONTANT = item.MONTANT;
                        Mand.NUMEROMANDATEMENT = item.NUMEROMANDATEMENT;
                        Mand.USERCREATION = item.USERCREATION;
                        Mand.USERMODIFICATION = item.USERMODIFICATION;
                        Mand.NumAvisCredit = item.NumAvisCredit;
                        Mand.TYPE_PAIEMENT = item.TYPE_PAIEMENT;
                        Mand.EST_MIS_A_JOUR = item.EST_MIS_A_JOUR;
                        foreach (var item_ in item.DETAILCAMPAGNEGC_)
                        {
                            DETAILPAIEMENTCAMPAGNEGC DetailMand = new DETAILPAIEMENTCAMPAGNEGC();

                            DetailMand.CENTRE = item_.CENTRE;
                            DetailMand.CLIENT = item_.CLIENT;
                            DetailMand.DATECREATION = item_.DATECREATION;
                            DetailMand.DATEMODIFICATION = item_.DATEMODIFICATION;
                            DetailMand.FK_IDPAIEMENTCAMPAGNEGC = item_.FK_IDPAIEMENTCAMPAGNEGC;
                            DetailMand.MONTANT = item_.MONTANT;
                            DetailMand.NDOC = item_.NDOC;
                            DetailMand.ORDRE = item_.ORDRE;
                            DetailMand.PERIODE = item_.PERIODE;
                            DetailMand.PK_ID = item_.PK_ID;
                            DetailMand.FK_IDCLIENT  = item_.FK_IDCLIENT ;
                            DetailMand.FK_IDLCLIENT = item_.FK_IDLCLIENT ;
                            DetailMand.STATUS = item_.STATUS;
                            DetailMand.USERCREATION = item_.USERCREATION;
                            DetailMand.USERMODIFICATION = item_.USERMODIFICATION;

                            Mand.DETAILPAIEMENTCAMPAGNEGC.Add(DetailMand);
                        }

                        ListMand.Add(Mand);
                    }
                    Entities.InsertEntity<PAIEMENTCAMPAGNEGC>(ListMand, context);
                    //if (Facture_Payer_Partiellement!=null && Facture_Payer_Partiellement .MONTANT_REGLER != 0)
                    //{
                    //    MANDATEMENTGC MANDATEMENTGC = context.MANDATEMENTGC.FirstOrDefault(d => d.PK_ID == Facture_Payer_Partiellement.FK_IDMANDATEMENT);
                    //    DETAILMANDATEMENTGC DETAILMANDATEMENTGC = MANDATEMENTGC.DETAILMANDATEMENTGC.FirstOrDefault(d => d.NDOC == Facture_Payer_Partiellement.NDOC);
                    //    DETAILMANDATEMENTGC.MONTANT = Facture_Payer_Partiellement.MONTANT;
                    //    Entities.UpdateEntity<DETAILMANDATEMENTGC>(DETAILMANDATEMENTGC, context); 
                    //}
                    context.SaveChanges();
                    return true;
                }

            }
            catch (DbEntityValidationException ex)
            {
                GenericEntityExeptionandler(ex);
                return null;
            }

        }


        public static bool? SaisiPaiement(decimal? Montant, int Id)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    var camp = context.CAMPAGNEGC.FirstOrDefault(c => c.PK_ID == Id);
                    camp.MONTANTPAYER = Montant;

                    Entities.InsertEntity<CAMPAGNEGC>(camp, context);
                    context.SaveChanges();
                    return true;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
 

        public static DataTable ChargerClientPourSaisiIndex(int idCampagne)
        {

            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    var query1 = (from x in context.DETAILCAMPAGNESGC
                                  where x.CAMPAGNESGC.PK_ID   == idCampagne  &&
                                    !context.INDEXCAMPAGNE.Any(es => es.CENTRE    == x.CENTRE   && es.CLIENT  == x.CLIENT && x.ORDRE ==es.ORDRE )
                                  select new
                                  {
                                      x.IDCAMPAGNEGC ,
                                      x.CENTRE,
                                      x.CLIENT,
                                      x.ORDRE,
                                      IDCOUPURE = x.CAMPAGNESGC.NUMEROCAMPAGNE 
                                  }).Distinct();
                    return Galatee.Tools.Utility.ListToDataTable<object>(query1);
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public static DataTable  ChargerCampagne( int IdRegroupement)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    var _lstCampagne = context.CAMPAGNESGC ;
                    IEnumerable<object> query = (from x in _lstCampagne
                                                 where x.FK_IDREGROUPEMENT == IdRegroupement
                                                 select new
                                                 {
                                                   x.FK_IDREGROUPEMENT ,
                                                   x.PK_ID ,
                                                   x.FK_IDMATRICULE,
                                                   x.NUMEROCAMPAGNE 
                                                 }).Distinct();
                    return Galatee.Tools.Utility.ListToDataTable<object>(query);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static DataTable AutorisationDePaiement(string centre,string client,string ordre)
        {

            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    var query1 = (from x in context.LCLIENT 
                                  where x.COPER == Enumere.CoperFRP && x.ISNONENCAISSABLE == true && 
                                  x.CENTRE == centre && x.CLIENT == client && x.ORDRE == ordre                                
                                  select new
                                  {
                                      x.PK_ID ,
                                      x.CENTRE,
                                      x.FK_IDCENTRE ,
                                      x.CLIENT,
                                      x.ORDRE,
                                      x.MONTANT ,
                                   NOM =  x.CLIENT1.NOMABON 
                                  }).Distinct();
                    return Galatee.Tools.Utility.ListToDataTable<object>(query1);
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public static bool ValidateAutorisation(CsLclient laFacture)
        {

            try
            {
                int res = -1;
                using (galadbEntities context = new galadbEntities())
                {
                    LCLIENT laFactureFrais = context.LCLIENT.FirstOrDefault(t => t.PK_ID == laFacture.PK_ID);
                    laFactureFrais.ISNONENCAISSABLE = false;
                   res= context.SaveChanges();
                };
                return res == -1 ? false : true;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        #endregion


        public static DataTable RetourneCampagnePrecontentieux(List<int> lstCentre)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    var _lstCampagne = context.CAMPAGNEPRECONTENTIEUX ;
                    List<string> lstIdCampagne = new List<string>();
                    IEnumerable<object> query = (from x in _lstCampagne
                                                 where lstCentre.Contains(x.FK_IDCENTRE)
                                                 select new
                                                 {
                                                     x.IDCOUPURE,
                                                     x.CENTRE,
                                                     x.MONTANT,
                                                     x.PK_ID,
                                                     x.DATECREATION,
                                                     x.DATEMODIFICATION,
                                                     x.USERCREATION,
                                                     x.USERMODIFICATION,
                                                     x.FK_IDCENTRE,
                                                     x.FK_IDMATRICULE,
                                                     CODESITE = x.CENTRE1.SITE.CODE,
                                                     FK_IDSITE = x.CENTRE1.FK_IDCODESITE,
                                                     LIBELLECENTRE = x.CENTRE1.LIBELLE,
                                                     LIBELLESITE = x.CENTRE1.SITE.LIBELLE,
                                                     AGENTPIA = (context.ADMUTILISATEUR.FirstOrDefault(t => t.PK_ID  == x.FK_IDMATRICULE ) != null) ? context.ADMUTILISATEUR.FirstOrDefault(t => t.PK_ID  == x.FK_IDMATRICULE ).LIBELLE : string.Empty
                                                 }).Distinct();
                    return Galatee.Tools.Utility.ListToDataTable<object>(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable RetourneDetailCampagnePrecontentieux(int idCampagne)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    var _lstCampagne = context.DETAILCAMPAGNEPRENCONTENTIEUX ;
                    List<string> lstIdCampagne = new List<string>();
                    IEnumerable<object> query = (from x in _lstCampagne
                                                 where x.FK_IDCAMPAGNE == idCampagne 
                                                 select new
                                                 {
                                                            x.  IDCAMPAGNE ,
                                                                x. PK_ID ,
                                                                x.  CENTRE ,
                                                                x.  CLIENT ,
                                                                x.  ORDRE ,
                                                                x.  TOURNEE ,
                                                                x.  ORDTOUR ,
                                                                x.  CATEGORIE ,
                                                                x. SOLDEDUE ,
                                                                x. SOLDECLIENT ,
                                                                x.  USERCREATION ,
                                                                x. DATECREATION ,
                                                                x. DATEMODIFICATION ,
                                                                x.  USERMODIFICATION ,
                                                                x. FK_IDCENTRE ,
                                                                x. FK_IDCLIENT ,
                                                                x. FK_IDTOURNEE ,
                                                                x. FK_IDCATEGORIE ,
                                                                x. FK_IDCAMPAGNE,
                                                            x.CLIENT1.NOMABON,
                                                            x.ISINVITATIONEDITER ,
                                                            x.DATERDV,
                                                            x.DATERESILIATION ,
                                                            ADRESSE = x.CLIENT1.ADRMAND1,
                                                              x.CLIENT1.AG.RUE ,
                                                            x.CLIENT1.AG.PORTE ,
                                                     CODESITE = x.CENTRE1.SITE.CODE,
                                                     FK_IDSITE = x.CENTRE1.FK_IDCODESITE,
                                                     LIBELLECENTRE = x.CENTRE1.LIBELLE,
                                                     LIBELLESITE = x.CENTRE1.SITE.LIBELLE
                                                 }).Distinct();
                    return Galatee.Tools.Utility.ListToDataTable<object>(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        #region Sylla 20/06/2016

        public static List<string> RecupererPeriodeDePlage(string PeriodeDebut, string PeriodeFin)
        {
            try
            {
                using (galadbEntities Context = new galadbEntities())
                {
                    int outint;

                    var ListeDesPeriodeFacturer = Context.LCLIENT.Take(30000).Select(l => l.REFEM).Distinct().ToList();
                    List<string> lstper=new List<string>();
                    foreach (var item in ListeDesPeriodeFacturer )
                    {
                        if (int.TryParse(item, out outint) == true)
                            lstper.Add(item);
                    }
                    var ListeDesPeriodeDePlage = lstper.Where(l => int.Parse(l) > int.Parse(PeriodeDebut) && int.Parse(l) < int.Parse(PeriodeFin));
                    if (ListeDesPeriodeDePlage != null)
                        return ListeDesPeriodeDePlage.ToList();
                    return new List<string>();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public static bool AnnulerCampagne(string NumeroCampagne)
        {
            try
            {
                using (galadbEntities Context = new galadbEntities())
                {
                    var CAMPAGNEGC = Context.CAMPAGNEGC.FirstOrDefault(c => c.NUMEROCAMPAGNE == NumeroCampagne);
                    var demandeWKF = Context.DEMANDE_WORFKLOW .FirstOrDefault(c => c.CODE_DEMANDE_TABLETRAVAIL  == NumeroCampagne);
                    if (CAMPAGNEGC != null)
                    {
                        CAMPAGNEGC.STATUS = "0";
                        demandeWKF.FK_IDSTATUS =(int)Enumere.STATUSDEMANDE.Annulee;
                        Entities.UpdateEntity<CAMPAGNEGC>(CAMPAGNEGC, Context);
                        Entities.UpdateEntity<DEMANDE_WORFKLOW>(demandeWKF, Context);
                        Context.SaveChanges();
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region sylla 11/05/2017
        public static List<CsLotComptClient> LoadAllAjustement()
        {
            List<CsLotComptClient> ListeAjustement = new List<CsLotComptClient>();
            try
            {
                using (galadbEntities Context = new galadbEntities())
                {
                    var lot = Context.LOTCOMPTECLIENT.Where(l => l.STATUS != "0" && l.STATUS != "2") != null ? Context.LOTCOMPTECLIENT.Where(l => l.STATUS != "0" && l.STATUS != "2").ToList() : new List<LOTCOMPTECLIENT>();
                    foreach (var item in lot.ToList())
                    {
                        CsLotComptClient Ajustement = new CsLotComptClient();
                        Ajustement.PK_ID = item.PK_ID;
                        Ajustement.DATECREATION = item.DATECREATION;
                        Ajustement.DATEMODIFICATION = item.DATEMODIFICATION;
                        Ajustement.DC = item.DC;
                        Ajustement.IDLOT = item.IDLOT;
                        Ajustement.MOISCOMPTABLE = item.MOISCOMPTABLE;
                        Ajustement.MONTANT = item.MONTANT;
                        Ajustement.NUMEROLOT = item.NUMEROLOT;
                        Ajustement.STATUS = item.STATUS;
                        Ajustement.USERCREATION = item.USERCREATION;
                        Ajustement.USERMODIFICATION = item.USERMODIFICATION;

                        Ajustement.DetaiLot = new List<CsDetailLot>();
                        foreach (var item_ in item.DETAILLOT)
                        {
                            CsDetailLot DetailLot = new CsDetailLot();
                            DetailLot.PK_ID = item_.PK_ID;
                            DetailLot.ACQUIT = item_.ACQUIT;
                            DetailLot.CENTRE = item_.CENTRE;
                            DetailLot.CLIENT = item_.CLIENT;
                            DetailLot.CODEERR = item_.CODEERR;
                            DetailLot.COPER = item_.COPER;
                            DetailLot.DATECREATION = item_.DATECREATION;
                            DetailLot.DATEMODIFICATION = item_.DATEMODIFICATION;
                            DetailLot.DATEPIECE = item_.DATEPIECE;
                            DetailLot.DATESAISIE = item_.DATESAISIE;
                            DetailLot.DATETRAIT = item_.DATETRAIT;
                            DetailLot.ECART = item_.ECART;
                            DetailLot.EXIGIBILITE = item_.EXIGIBILITE;
                            DetailLot.FK_IDCENTRE = item_.FK_IDCENTRE;
                            DetailLot.FK_IDCLIENT = item_.FK_IDCLIENT;
                            DetailLot.FK_IDCOPER = item_.FK_IDCOPER;
                            DetailLot.FK_IDLCLIENT = item_.FK_IDLCLIENT;
                            DetailLot.FK_IDLOTCOMPECLIENT = item_.FK_IDLOTCOMPECLIENT;
                            DetailLot.FK_IDMATRICULE = item_.FK_IDMATRICULE;
                            DetailLot.IDLOT = item_.IDLOT;
                            DetailLot.MATRICULE = item_.MATRICULE;
                            DetailLot.MODEREG = item_.MODEREG;
                            DetailLot.MONTANT = item_.MONTANT;
                            DetailLot.NDOC = item_.NDOC;
                            DetailLot.NUMEROLIGNE = item_.NUMEROLIGNE;
                            DetailLot.ORDRE = item_.ORDRE;
                            DetailLot.REFEM = item_.REFEM;
                            DetailLot.REFEMNDOC = item_.REFEMNDOC;
                            DetailLot.REFERENCE = item_.REFERENCE;
                            DetailLot.SENS = item_.SENS;
                            DetailLot.USERCREATION = item_.USERCREATION;
                            DetailLot.USERMODIFICATION = item_.USERMODIFICATION;
                            DetailLot.STATUT = item_.STATUT;
                            DetailLot.MONTANT_AJUSTEMENT = item_.MONTANT_AJUSTEMENT != null ? item_.MONTANT_AJUSTEMENT : 0;

                            Ajustement.DetaiLot.Add(DetailLot);
                        }
                        ListeAjustement.Add(Ajustement);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return ListeAjustement;
        }

        public static int SaveAjustement(List<CsLotComptClient> ListeAjustementScelleToUpdate, List<CsLotComptClient> ListeAjustementScelleToInserte, List<CsLotComptClient> ListeAjustementScelleToDelete)
        {
            //bool IsSaved = false;
            int PK_ID = 0;
            List<LOTCOMPTECLIENT> ListeAjustementScelleToUpdate_ = new List<LOTCOMPTECLIENT>();
            List<LOTCOMPTECLIENT> ListeAjustementScelleToInserte_ = new List<LOTCOMPTECLIENT>();
            List<LOTCOMPTECLIENT> ListeAjustementScelleToDelete_ = new List<LOTCOMPTECLIENT>();

            List<int> LISTDETAILLOT = new List<int>();

            try
            {

                using (galadbEntities Context = new galadbEntities())
                {
                    convertAjustement(ListeAjustementScelleToInserte, ListeAjustementScelleToInserte_, Context);
                    convertAjustement(ListeAjustementScelleToDelete, ListeAjustementScelleToDelete_, Context);
                    convertAjustement(ListeAjustementScelleToUpdate, ListeAjustementScelleToUpdate_, Context);

                    //Entities.UpdateEntity<LOTCOMPTECLIENT>(ListeAjustementScelleToUpdate_, Context);
                    if (ListeAjustementScelleToUpdate != null && ListeAjustementScelleToUpdate.Count() > 0)
                    {
                        CsLotComptClient idlotcmptcl = ListeAjustementScelleToUpdate.First();
                        LOTCOMPTECLIENT lstupdate = Context.LOTCOMPTECLIENT.FirstOrDefault(c => c.PK_ID == idlotcmptcl.PK_ID);
                        if (lstupdate != null)
                            lstupdate.MONTANT = ListeAjustementScelleToUpdate.First().MONTANT;

                        List<DETAILLOT> dl = Entities.ConvertObject<DETAILLOT, CsDetailLot>(idlotcmptcl.DetaiLot.Where(c => c.PK_ID == 0).ToList());
                        dl.ForEach(c => c.FK_IDMATRICULE = Context.ADMUTILISATEUR.FirstOrDefault(d => d.MATRICULE == idlotcmptcl.USERCREATION).PK_ID);
                        dl.ForEach(c => c.FK_IDLOTCOMPECLIENT = idlotcmptcl.PK_ID);
                        Entities.InsertEntity<DETAILLOT>(dl, Context);

                        foreach (var item in idlotcmptcl.DetaiLot.Where(c => c.PK_ID != 0).ToList())
                        {
                            DETAILLOT dtl__ = Context.DETAILLOT.FirstOrDefault(c => c.PK_ID == item.PK_ID);
                            if (dtl__ != null)
                            {
                                dtl__.STATUT = item.STATUT;
                                dtl__.MONTANT = item.MONTANT;
                                dtl__.MONTANT_AJUSTEMENT = item.MONTANT_AJUSTEMENT;
                            }

                        }
                    }
                    //foreach (var item in ListeAjustementScelleToUpdate_)
                    //{

                    //    foreach (var item_ in item.DETAILLOT)
                    //    {
                    //        if (item_.PK_ID == 0)
                    //        {

                    //            Context.DETAILLOT.Add(item_);
                    //        }
                    //        else
                    //        {
                    //            Entities.UpdateEntity<DETAILLOT>(item_, Context);
                    //        }
                    //    }
                    //    //List<DETAILLOT> DETAILLOT_OLD = Context.DETAILLOT.Where(d => d.FK_IDLOTCOMPECLIENT == item.PK_ID) != null ? Context.DETAILLOT.Where(d => d.FK_IDLOTCOMPECLIENT == item.PK_ID).ToList() : new List<DETAILLOT>();

                    //    //var DETAILLOT_REMOVE = DETAILLOT_OLD.Where(d => !item.DETAILLOT.Select(c => c.PK_ID).Contains(d.PK_ID)) != null ? DETAILLOT_OLD.Where(d => !item.DETAILLOT.Select(c => c.PK_ID).Contains(d.PK_ID)).ToList() : new List<DETAILLOT>();
                    //    //Entities.DeleteEntity<DETAILLOT>(LISTDETAILLOT, Context);

                    //}

                    Entities.InsertEntity<LOTCOMPTECLIENT>(ListeAjustementScelleToInserte_, Context);

                    Entities.DeleteEntity<LOTCOMPTECLIENT>(ListeAjustementScelleToDelete_, Context);
                    foreach (var item in ListeAjustementScelleToDelete_)
                    {
                        Entities.DeleteEntity<DETAILLOT>(item.DETAILLOT.ToList(), Context);
                    }
                    Context.SaveChanges();

                    if (ListeAjustementScelleToUpdate_.Count > 0)
                    {
                        PK_ID = ListeAjustementScelleToUpdate_.First().PK_ID;
                    }
                    if (ListeAjustementScelleToInserte_.Count > 0)
                    {
                        PK_ID = ListeAjustementScelleToInserte_.First().PK_ID;
                    }
                    if (ListeAjustementScelleToDelete_.Count > 0)
                    {
                        PK_ID = ListeAjustementScelleToDelete_.First().PK_ID;
                    }
                    //IsSaved = true;

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return PK_ID;
        }

        //Convertir une liste de CsAjustement en une liste de LOTCOMPTECLIENT
        private static void convertAjustement(List<CsLotComptClient> ListeAjustementScelle, List<LOTCOMPTECLIENT> ListeAjustementScelle_, galadbEntities Context)
        {
            foreach (var item in ListeAjustementScelle)
            {
                LOTCOMPTECLIENT Ajustement = new LOTCOMPTECLIENT();

                Ajustement.PK_ID = item.PK_ID;
                Ajustement.DATECREATION = item.DATECREATION;
                Ajustement.DATEMODIFICATION = item.DATEMODIFICATION;
                Ajustement.DC = item.DC;
                Ajustement.IDLOT = item.IDLOT;
                Ajustement.MOISCOMPTABLE = item.MOISCOMPTABLE;
                Ajustement.MONTANT = item.MONTANT;
                Ajustement.NUMEROLOT = item.NUMEROLOT;
                Ajustement.STATUS = item.STATUS;
                Ajustement.USERCREATION = item.USERCREATION;
                Ajustement.USERMODIFICATION = item.USERMODIFICATION;

                //if (ListeDetailLot!=null)
                //{
                //    var o = new galadbEntities().DETAILLOT.Where(d => d.FK_IDLOTCOMPECLIENT == item.PK_ID) != null ? Context.DETAILLOT.Where(d => d.FK_IDLOTCOMPECLIENT == item.PK_ID).ToList() : new List<DETAILLOT>();
                //    var s = o.Where(d => !item.DetaiLot.Select(c => c.PK_ID).Contains(d.PK_ID)) != null ? o.Where(d => !item.DetaiLot.Select(c => c.PK_ID).Contains(d.PK_ID)).ToList() : new List<DETAILLOT>();
                //    ListeDetailLot = s.Select(c=>c.PK_ID).ToList();
                //    //Context.Entry(o).State = EntityState.Detached;
                //}

                foreach (var item_ in item.DetaiLot)
                {
                    DETAILLOT DetailLot = new DETAILLOT();

                    DetailLot.PK_ID = item_.PK_ID;
                    DetailLot.ACQUIT = item_.ACQUIT;
                    DetailLot.CENTRE = item_.CENTRE;
                    DetailLot.CLIENT = item_.CLIENT;
                    DetailLot.CODEERR = item_.CODEERR;
                    DetailLot.COPER = item_.COPER;
                    DetailLot.DATECREATION = item_.DATECREATION;
                    DetailLot.DATEMODIFICATION = item_.DATEMODIFICATION;
                    DetailLot.DATEPIECE = item_.DATEPIECE;
                    DetailLot.DATESAISIE = item_.DATESAISIE;
                    DetailLot.DATETRAIT = item_.DATETRAIT;
                    DetailLot.ECART = item_.ECART;
                    DetailLot.EXIGIBILITE = item_.EXIGIBILITE;
                    DetailLot.FK_IDCENTRE = item_.FK_IDCENTRE;
                    DetailLot.FK_IDCLIENT = item_.FK_IDCLIENT;
                    DetailLot.FK_IDCOPER = item_.FK_IDCOPER;
                    DetailLot.FK_IDLCLIENT = item_.FK_IDLCLIENT;
                    DetailLot.FK_IDLOTCOMPECLIENT = item_.FK_IDLOTCOMPECLIENT != 0 ? item_.FK_IDLOTCOMPECLIENT : item.PK_ID;
                    DetailLot.FK_IDMATRICULE = Context.ADMUTILISATEUR.FirstOrDefault(u => u.MATRICULE == item.USERCREATION).PK_ID;
                    DetailLot.IDLOT = item_.IDLOT;
                    DetailLot.MATRICULE = item_.MATRICULE;
                    DetailLot.MODEREG = item_.MODEREG;
                    DetailLot.MONTANT = item_.MONTANT;
                    DetailLot.NDOC = item_.NDOC;
                    DetailLot.NUMEROLIGNE = item_.NUMEROLIGNE;
                    DetailLot.ORDRE = item_.ORDRE;
                    DetailLot.REFEM = item_.REFEM;
                    DetailLot.REFEMNDOC = item_.REFEMNDOC;
                    DetailLot.REFERENCE = item_.REFERENCE;
                    DetailLot.SENS = item_.SENS;
                    DetailLot.USERCREATION = item_.USERCREATION;
                    DetailLot.USERMODIFICATION = item_.USERMODIFICATION;
                    DetailLot.STATUT = item_.STATUT;
                    DetailLot.MONTANT_AJUSTEMENT = item_.MONTANT_AJUSTEMENT;
                    Ajustement.DETAILLOT.Add(DetailLot);

                }

                ListeAjustementScelle_.Add(Ajustement);
            }
        }

        #endregion
    }
}
