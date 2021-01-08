using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Transactions;
using Galatee.Structure;
using System.Threading.Tasks;
using System.Data.Entity;
using Galatee.Entity.Model.Fraude;

namespace Galatee.Entity.Model
{
    public static class CaisseProcedures
    {
        public static List<TRANSCAISSE> ObtenirTransCaiss(string pCaisse, string pMatricule)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<TRANSCAISSE> transc = context.TRANSCAISSE.Where(t => 
                        //t.CAISSE == pCaisse && 
                        t.MATRICULE == pMatricule && t.COPER != Enumere.CoperAjsutemenFondCaisse &&
                                                           t.COPER != Enumere.CoperFondCaisse);
                    return transc.ToList();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<OPENINGDAY> ObtenirOpenDay(string pMatricule)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    // confusion a lever LUDOVIC
                    List<OPENINGDAY> openDayDemande = context.OPENINGDAY.Where(o => (o.KEYEDBY == pMatricule || o.CASHIER == pMatricule) && o.OPEN == Enumere.TopOuvertureCaisse).ToList();

                    /* Mettre a jour openingday en cas d'ouverture de caisse a la demande*/
                    if (openDayDemande.Count() > 0)
                        openDayDemande.ForEach(o => o.OPEN = null);

                    return openDayDemande;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void ValuerForeignKey(TRANSCAISB transB)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    transB.FK_IDCAISSIERE  = context.CAISSE.First(c => c.NUMCAISSE == transB.CAISSE).PK_ID;
                    transB.FK_IDCENTRE = context.CENTRE.First(c => c.CODE == transB.CENTRE).PK_ID;
                    transB.FK_IDCOPER = context.COPER.First(c => c.CODE == transB.COPER).PK_ID;
                    transB.FK_IDLIBELLETOP = context.LIBELLETOP.First(l => l.CODE == transB.TOP1).PK_ID;
                    transB.FK_IDHABILITATIONCAISSE  = context.ADMUTILISATEUR.First(a => a.MATRICULE == transB.MATRICULE).PK_ID;
                    transB.FK_IDMODEREG = context.MODEREG.First(m => m.CODE  == transB.MODEREG).PK_ID;
                }
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        public static DataTable RetourneListeDesClientsParCodeRegroupemnt(int idCodeRegroupement)
        {
            try
            {
                //cmd.CommandText = "SPX_ENC_LISTECLIENTREGROUPE";
                using (galadbEntities ctontext = new galadbEntities())
                {
                    IEnumerable<CLIENT> clientGrouper = ctontext.CLIENT.Where(cl => cl.FK_IDREGROUPEMENT == idCodeRegroupement);
                    List<CLIENT> clientSoldeSupZero = new List<CLIENT>();
                    foreach (CLIENT cl in clientGrouper)
                    {
                        decimal? SoldeClient = FonctionCaisse.RetourneSoldeClient(cl.FK_IDCENTRE ,cl.CENTRE ,cl.REFCLIENT ,cl.ORDRE );
                        if (SoldeClient > 0)
                            clientSoldeSupZero.Add(cl); 
                    }


                    IEnumerable<object> query = from _clientGrouper in clientSoldeSupZero
                                                select new
                                                {
                                                    _clientGrouper.CENTRE,
                                                    _clientGrouper.REFCLIENT,
                                                    _clientGrouper.ORDRE,
                                                    _clientGrouper.PK_ID
                                                };
                    return Galatee.Tools.Utility.ListToDataTable<object>(query);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static DataTable RetourneDecaissementCaisse(CsHabilitationCaisse laCaisse)
        {
            try
            {

                List<CsLclient> LstEncaissement = new List<CsLclient>();
                galadbEntities context = new galadbEntities();
                IEnumerable<object> query = null;
                var _TRANSCAISSE = context.TRANSCAISSE;
                query =
                from _LeTRANSCAISSE in _TRANSCAISSE
                where _LeTRANSCAISSE.FK_IDHABILITATIONCAISSE == laCaisse.PK_ID && _LeTRANSCAISSE.DC == Enumere.Credit 
                orderby _LeTRANSCAISSE.DTRANS, _LeTRANSCAISSE.DATEENCAISSEMENT
                group _LeTRANSCAISSE by new
                {
                    CENTRE = _LeTRANSCAISSE.CENTRE,
                    CLIENT = _LeTRANSCAISSE.CLIENT,
                    ORDRE = _LeTRANSCAISSE.ORDRE,
                    CAISSE = _LeTRANSCAISSE.CAISSE,
                    ACQUIT = _LeTRANSCAISSE.ACQUIT,
                    MATRICULE = _LeTRANSCAISSE.MATRICULE,
                    MODEREG = _LeTRANSCAISSE.MODEREG,
                    NUMCHEQ = _LeTRANSCAISSE.NUMCHEQ,
                    BANQUE = _LeTRANSCAISSE.BANQUE,
                    NDOC = _LeTRANSCAISSE.NDOC,
                    REFEM = _LeTRANSCAISSE.REFEM,
                    TOPANNUL = _LeTRANSCAISSE.TOPANNUL,
                    DTRANS = _LeTRANSCAISSE.DTRANS,
                    DATEENCAISSEMENT = _LeTRANSCAISSE.DATEENCAISSEMENT,
                    DENR = _LeTRANSCAISSE.DTRANS,
                    FK_IDCENTRE = _LeTRANSCAISSE.FK_IDCENTRE,
                    FK_IDMODEREG = _LeTRANSCAISSE.FK_IDMODEREG,
                    FK_IDCAISSIERE = _LeTRANSCAISSE.FK_IDCAISSIERE,
                    FK_IDHABILITATIONCAISSE = _LeTRANSCAISSE.FK_IDHABILITATIONCAISSE,
                    SAISIPAR = _LeTRANSCAISSE.SAISIPAR,
                    NUMDEM = _LeTRANSCAISSE.NUMDEM,
                    LIBELLECOPER = _LeTRANSCAISSE.COPER1.LIBCOURT,
                    LIBELLEMODREG = _LeTRANSCAISSE.MODEREG1.LIBELLE,
                    NOMCAISSIERE = _LeTRANSCAISSE.HABILITATIONCAISSE == null ? _LeTRANSCAISSE.ADMUTILISATEUR1.LIBELLE : _LeTRANSCAISSE.HABILITATIONCAISSE.ADMUTILISATEUR.LIBELLE,
                    _LeTRANSCAISSE.DC 

                } into pTemp
                let MONTANT = pTemp.Sum(x => x.MONTANT)
                select new
                {
                    pTemp.Key.CENTRE,
                    pTemp.Key.CLIENT,
                    pTemp.Key.ORDRE,
                    pTemp.Key.CAISSE,
                    pTemp.Key.ACQUIT,
                    pTemp.Key.MATRICULE,
                    pTemp.Key.MODEREG,
                    pTemp.Key.DTRANS,
                    pTemp.Key.BANQUE,
                    pTemp.Key.NDOC,
                    pTemp.Key.REFEM,
                    pTemp.Key.TOPANNUL,
                    pTemp.Key.NUMDEM,
                    pTemp.Key.NUMCHEQ,
                    pTemp.Key.SAISIPAR,
                    MONTANT,
                    pTemp.Key.DATEENCAISSEMENT,
                    pTemp.Key.DENR,
                    pTemp.Key.FK_IDCENTRE,
                    pTemp.Key.LIBELLECOPER,
                    pTemp.Key.LIBELLEMODREG,
                    pTemp.Key.NOMCAISSIERE,
                    pTemp.Key.DC 
                };


                IEnumerable<object> query1 = null;
                var _TRANSCAISB = context.TRANSCAISB;
                query1 =
                from _LeTRANSCAISB in _TRANSCAISB
                where _LeTRANSCAISB.FK_IDHABILITATIONCAISSE == laCaisse.PK_ID && _LeTRANSCAISB.DC == Enumere.Credit  
                orderby _LeTRANSCAISB.DTRANS, _LeTRANSCAISB.DATEENCAISSEMENT
                group _LeTRANSCAISB by new
                {
                    CENTRE = _LeTRANSCAISB.CENTRE,
                    CLIENT = _LeTRANSCAISB.CLIENT,
                    ORDRE = _LeTRANSCAISB.ORDRE,
                    CAISSE = _LeTRANSCAISB.CAISSE,
                    ACQUIT = _LeTRANSCAISB.ACQUIT,
                    MATRICULE = _LeTRANSCAISB.MATRICULE,
                    MODEREG = _LeTRANSCAISB.MODEREG,
                    NUMCHEQ = _LeTRANSCAISB.NUMCHEQ,
                    BANQUE = _LeTRANSCAISB.BANQUE,
                    NDOC = _LeTRANSCAISB.NDOC,
                    REFEM = _LeTRANSCAISB.REFEM,
                    TOPANNUL = _LeTRANSCAISB.TOPANNUL,
                    DTRANS = _LeTRANSCAISB.DTRANS,
                    DENR = _LeTRANSCAISB.DTRANS,
                    DATEENCAISSEMENT = _LeTRANSCAISB.DTRANS,
                    FK_IDCENTRE = _LeTRANSCAISB.FK_IDCENTRE,
                    FK_IDMODEREG = _LeTRANSCAISB.FK_IDMODEREG,
                    FK_IDCAISSIERE = _LeTRANSCAISB.FK_IDCAISSIERE,
                    FK_IDHABILITATIONCAISSE = _LeTRANSCAISB.FK_IDHABILITATIONCAISSE,
                    SAISIPAR = _LeTRANSCAISB.SAISIPAR,
                    NUMDEM = _LeTRANSCAISB.NUMDEM,
                    LIBELLECOPER = _LeTRANSCAISB.COPER1.LIBCOURT,
                    LIBELLEMODREG = _LeTRANSCAISB.MODEREG1.LIBELLE,
                    NOMCAISSIERE = _LeTRANSCAISB.HABILITATIONCAISSE == null ? _LeTRANSCAISB.ADMUTILISATEUR1.LIBELLE : _LeTRANSCAISB.HABILITATIONCAISSE.ADMUTILISATEUR.LIBELLE,
                    _LeTRANSCAISB.DC 

                } into pTemp
                let MONTANT = pTemp.Sum(x => x.MONTANT)
                select new
                {
                    pTemp.Key.CENTRE,
                    pTemp.Key.CLIENT,
                    pTemp.Key.ORDRE,
                    pTemp.Key.CAISSE,
                    pTemp.Key.ACQUIT,
                    pTemp.Key.MATRICULE,
                    pTemp.Key.NDOC,
                    pTemp.Key.REFEM,
                    pTemp.Key.TOPANNUL,
                    pTemp.Key.DENR,
                    pTemp.Key.MODEREG,
                    pTemp.Key.DTRANS,
                    pTemp.Key.BANQUE,
                    pTemp.Key.NUMDEM,
                    pTemp.Key.NUMCHEQ,
                    pTemp.Key.SAISIPAR,
                    MONTANT,
                    pTemp.Key.DATEENCAISSEMENT,
                    pTemp.Key.FK_IDCENTRE,
                    pTemp.Key.LIBELLECOPER,
                    pTemp.Key.LIBELLEMODREG,
                    pTemp.Key.NOMCAISSIERE,
                    pTemp.Key.DC 
                };
                var result = query.Union(query1);
                return Galatee.Tools.Utility.ListToDataTable(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static DataTable RetourneEncaissementCaisse(CsHabilitationCaisse laCaisse)
        {
            try
            {

                List<CsLclient> LstEncaissement = new List<CsLclient>();
                galadbEntities context = new galadbEntities();
                IEnumerable<object> query = null;
                var _TRANSCAISSE = context.TRANSCAISSE;
                query =
                from _LeTRANSCAISSE in _TRANSCAISSE
                where _LeTRANSCAISSE.FK_IDHABILITATIONCAISSE == laCaisse.PK_ID  &&  _LeTRANSCAISSE.DC == Enumere.Debit  
                orderby _LeTRANSCAISSE.DTRANS, _LeTRANSCAISSE.DATEENCAISSEMENT
                group _LeTRANSCAISSE by new
                {
                    CENTRE = _LeTRANSCAISSE.CENTRE,
                    CLIENT = _LeTRANSCAISSE.CLIENT,
                    ORDRE = _LeTRANSCAISSE.ORDRE,
                    CAISSE = _LeTRANSCAISSE.CAISSE,
                    ACQUIT = _LeTRANSCAISSE.ACQUIT,
                    MATRICULE = _LeTRANSCAISSE.MATRICULE,
                    MODEREG = _LeTRANSCAISSE.MODEREG,
                    NUMCHEQ = _LeTRANSCAISSE.NUMCHEQ,
                    BANQUE = _LeTRANSCAISSE.BANQUE,
                    NDOC = _LeTRANSCAISSE.NDOC,
                    REFEM = _LeTRANSCAISSE.REFEM,
                    TOPANNUL = _LeTRANSCAISSE.TOPANNUL ,
                    DTRANS = _LeTRANSCAISSE.DTRANS,
                    DATEENCAISSEMENT = _LeTRANSCAISSE.DATEENCAISSEMENT,
                    DENR = _LeTRANSCAISSE.DTRANS,
                    FK_IDCENTRE = _LeTRANSCAISSE.FK_IDCENTRE,
                    FK_IDMODEREG = _LeTRANSCAISSE.FK_IDMODEREG,
                    FK_IDCAISSIERE = _LeTRANSCAISSE.FK_IDCAISSIERE,
                    FK_IDHABILITATIONCAISSE = _LeTRANSCAISSE.FK_IDHABILITATIONCAISSE,
                    SAISIPAR = _LeTRANSCAISSE.SAISIPAR,
                    NUMDEM = _LeTRANSCAISSE.NUMDEM,
                    LIBELLECOPER = _LeTRANSCAISSE.COPER1.LIBCOURT,
                    LIBELLEMODREG = _LeTRANSCAISSE.MODEREG1.LIBELLE,
                    NOMCAISSIERE = _LeTRANSCAISSE.HABILITATIONCAISSE == null ? _LeTRANSCAISSE.ADMUTILISATEUR1.LIBELLE : _LeTRANSCAISSE.HABILITATIONCAISSE.ADMUTILISATEUR.LIBELLE,
                    _LeTRANSCAISSE.DC 

                } into pTemp
                let MONTANT = pTemp.Sum(x => x.MONTANT)
                select new
                {
                    pTemp.Key.CENTRE,
                    pTemp.Key.CLIENT,
                    pTemp.Key.ORDRE,
                    pTemp.Key.CAISSE,
                    pTemp.Key.ACQUIT,
                    pTemp.Key.MATRICULE,
                    pTemp.Key.MODEREG,
                    pTemp.Key.DTRANS,
                    pTemp.Key.BANQUE,
                    pTemp.Key.NDOC,
                    pTemp.Key.REFEM,
                    pTemp.Key.TOPANNUL ,
                    pTemp.Key.NUMDEM,
                    pTemp.Key.NUMCHEQ,
                    pTemp.Key.SAISIPAR,
                    MONTANT,
                    pTemp.Key.DATEENCAISSEMENT,
                    pTemp.Key.DENR,
                    pTemp.Key.FK_IDCENTRE,
                    pTemp.Key.LIBELLECOPER,
                    pTemp.Key.LIBELLEMODREG,
                    pTemp.Key.NOMCAISSIERE,
                    pTemp.Key.DC
                };


                IEnumerable<object> query1 = null;
                var _TRANSCAISB = context.TRANSCAISB;
                query1 =
                from _LeTRANSCAISB in _TRANSCAISB
                where _LeTRANSCAISB.FK_IDHABILITATIONCAISSE == laCaisse.PK_ID && _LeTRANSCAISB.DC == Enumere.Debit 
                orderby _LeTRANSCAISB.DTRANS, _LeTRANSCAISB.DATEENCAISSEMENT
                group _LeTRANSCAISB by new
                {
                    CENTRE = _LeTRANSCAISB.CENTRE,
                    CLIENT = _LeTRANSCAISB.CLIENT,
                    ORDRE = _LeTRANSCAISB.ORDRE,
                    CAISSE = _LeTRANSCAISB.CAISSE,
                    ACQUIT = _LeTRANSCAISB.ACQUIT,
                    MATRICULE = _LeTRANSCAISB.MATRICULE,
                    MODEREG = _LeTRANSCAISB.MODEREG,
                    NUMCHEQ = _LeTRANSCAISB.NUMCHEQ,
                    BANQUE = _LeTRANSCAISB.BANQUE,
                    NDOC = _LeTRANSCAISB.NDOC,
                    REFEM = _LeTRANSCAISB.REFEM,
                    TOPANNUL = _LeTRANSCAISB.TOPANNUL,
                    DTRANS = _LeTRANSCAISB.DTRANS,
                    DENR = _LeTRANSCAISB.DTRANS,
                    DATEENCAISSEMENT = _LeTRANSCAISB.DTRANS,
                    FK_IDCENTRE = _LeTRANSCAISB.FK_IDCENTRE,
                    FK_IDMODEREG = _LeTRANSCAISB.FK_IDMODEREG,
                    FK_IDCAISSIERE = _LeTRANSCAISB.FK_IDCAISSIERE,
                    FK_IDHABILITATIONCAISSE = _LeTRANSCAISB.FK_IDHABILITATIONCAISSE,
                    SAISIPAR = _LeTRANSCAISB.SAISIPAR,
                    NUMDEM = _LeTRANSCAISB.NUMDEM,
                    LIBELLECOPER = _LeTRANSCAISB.COPER1.LIBCOURT,
                    LIBELLEMODREG = _LeTRANSCAISB.MODEREG1.LIBELLE,
                    NOMCAISSIERE = _LeTRANSCAISB.HABILITATIONCAISSE == null ? _LeTRANSCAISB.ADMUTILISATEUR1.LIBELLE : _LeTRANSCAISB.HABILITATIONCAISSE.ADMUTILISATEUR.LIBELLE,
                    _LeTRANSCAISB.DC 

                } into pTemp
                let MONTANT = pTemp.Sum(x => x.MONTANT)
                select new
                {
                    pTemp.Key.CENTRE,
                    pTemp.Key.CLIENT,
                    pTemp.Key.ORDRE,
                    pTemp.Key.CAISSE,
                    pTemp.Key.ACQUIT,
                    pTemp.Key.MATRICULE,
                    pTemp.Key.NDOC,
                    pTemp.Key.REFEM,
                    pTemp.Key.TOPANNUL ,
                    pTemp.Key.DENR,
                    pTemp.Key.MODEREG,
                    pTemp.Key.DTRANS,
                    pTemp.Key.BANQUE,
                    pTemp.Key.NUMDEM,
                    pTemp.Key.NUMCHEQ,
                    pTemp.Key.SAISIPAR,
                    MONTANT,
                    pTemp.Key.DATEENCAISSEMENT,
                    pTemp.Key.FK_IDCENTRE,
                    pTemp.Key.LIBELLECOPER,
                    pTemp.Key.LIBELLEMODREG,
                    pTemp.Key.NOMCAISSIERE,
                    pTemp.Key.DC  
                };
                var result = query.Union(query1);
                return Galatee.Tools.Utility.ListToDataTable(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static DataTable RetourneListeDemandeDevis(string pDevis)
        {
            try
            {
                using (galadbEntities ctontext = new galadbEntities())
                {

                    return null;// Galatee.Tools.Utility.ListToDataTable<object>(query);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static DataTable RetourneListeDesRecuNonAnnules(int pCaisse)
        {
            try
            {
                using (galadbEntities ctontext = new galadbEntities())
                {
                    var transc = ctontext.TRANSCAISSE.Where(t => t.FK_IDHABILITATIONCAISSE  == pCaisse).OrderBy(t => t.ACQUIT);
                    var Leclient= ctontext.CLIENT   ;
                    var LeDclient= ctontext.DCLIENT    ;
                    IEnumerable<object> query = (from t in transc
                                                 where t.TOPANNUL != Enumere.CaisseTopAnnulation || string.IsNullOrEmpty(t.TOPANNUL)
                                                 select new
                                                 {
                                                        t. CENTRE ,
                                                        t. CLIENT ,
                                                        t. ORDRE ,
                                                        t. CAISSE ,
                                                        t. ACQUIT ,
                                                        t. MATRICULE ,
                                                        t. NDOC ,
                                                        t. REFEM ,
                                                        t. MONTANT ,
                                                        t. DC ,
                                                        t. COPER ,
                                                        t. PERCU ,
                                                        t. RENDU ,
                                                        t. MODEREG ,
                                                        t. PLACE ,
                                                        t. DTRANS ,
                                                        t. DEXIG ,
                                                        t. BANQUE ,
                                                        t. GUICHET ,
                                                        t. ORIGINE ,
                                                        t. ECART ,
                                                        t. TOPANNUL ,
                                                        t. MOTIFANNULATION ,
                                                        t. ISDEMANDEANNULATION ,
                                                        t. CRET ,
                                                        t. MOISCOMPT ,
                                                        t. TOP1 ,
                                                        t. TOURNEE ,
                                                        t. NUMDEM ,
                                                        t. DATEVALEUR ,
                                                        t. DATEFLAG ,
                                                        t. NUMCHEQ ,
                                                        t. SAISIPAR ,
                                                        t. DATEENCAISSEMENT ,
                                                        t. CANCELLATION ,
                                                        t. USERCREATION ,
                                                        t. DATECREATION ,
                                                        t. DATEMODIFICATION ,
                                                        t. USERMODIFICATION ,
                                                        t. PK_ID ,
                                                        t. FK_IDCENTRE ,
                                                        t. FK_IDLCLIENT ,
                                                        t. FK_IDHABILITATIONCAISSE ,
                                                        t. FK_IDMODEREG ,
                                                        t. FK_IDLIBELLETOP ,
                                                        t. FK_IDCAISSIERE ,
                                                        t. FK_IDAGENTSAISIE ,
                                                        t. FK_IDCOPER ,
                                                        t. FK_IDPOSTECLIENT ,
                                                        t. FK_IDNAF ,
                                                        t. POSTE ,
                                                        t. DATETRANS ,
                                                        t. BANQUECAISSE ,
                                                        t. AGENCEBANQUE,
                                                        REFFERENCEACQUIT = t.CAISSE + t.ACQUIT + t.MATRICULE,
                                                        MONTANTPAYE =  t.MONTANT,
                                                        LIBELLEAGENCE = t.HABILITATIONCAISSE.CENTRE1.LIBELLE,
                                                        LIBELLESITE = t.HABILITATIONCAISSE.CENTRE1.SITE.LIBELLE,
                                                        LIBELLECOPER = t.COPER1.LIBELLE,
                                                        NOM = ctontext.CLIENT.FirstOrDefault(c => c.REFCLIENT == t.CLIENT && c.CENTRE == t.CENTRE && c.ORDRE == t.ORDRE).NOMABON,
                                                        IsDEMANDEANNULATION = t.ISDEMANDEANNULATION ,
                                                 });


                    return Galatee.Tools.Utility.ListToDataTable<object>(query);

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static DataTable RetourneListeRecuDeCaisse(int pCaisse)
        {
            try
            {
                using (galadbEntities ctontext = new galadbEntities())
                {
                    var transc = ctontext.TRANSCAISSE.Where(t => t.FK_IDHABILITATIONCAISSE == pCaisse).OrderBy(t => t.ACQUIT);
                    var demAnnul = ctontext.DEMANDEANNULATION.Where(t => t.FK_IDHABILITATIONCAISSE == pCaisse).OrderBy(t => t.ACQUIT);
                    IEnumerable<object> query = (from t in transc
                                                 join n in demAnnul on new { t.ACQUIT  }
                                                 equals new { ACQUIT = n.ACQUIT  } into _RecuCaisse
                                                 from p in _RecuCaisse.DefaultIfEmpty()
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
                                                     t.ISDEMANDEANNULATION,
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
                                                     MONTANTPAYE = t.MONTANT,
                                                     LIBELLEAGENCE = t.HABILITATIONCAISSE.CENTRE1.LIBELLE,
                                                     LIBELLESITE = t.HABILITATIONCAISSE.CENTRE1.SITE.LIBELLE,
                                                     LIBELLECOPER = string.IsNullOrEmpty(t.NUMDEM) ? t.LCLIENT.COPER1.LIBELLE :
                                                                    ctontext.RUBRIQUEDEMANDE.FirstOrDefault(c => t.NUMDEM == c.NUMDEM && t.REFEM == c.REFEM && t.NDOC == c.NDOC).COPER1.LIBELLE,
                                                     NOM = string.IsNullOrEmpty(t.NUMDEM) ? ctontext.CLIENT.FirstOrDefault(c => c.REFCLIENT == t.CLIENT && c.CENTRE == t.CENTRE && c.ORDRE == t.ORDRE).NOMABON :
                                                           ctontext.DCLIENT.FirstOrDefault(c => t.NUMDEM == c.NUMDEM).NOMABON, 
                                                     IsDEMANDEANNULATION = t.ISDEMANDEANNULATION,
                                                     p.MOTIFDEMANDE ,
                                                     p.MOTIFREJET ,
                                                     p.STATUS 
                                                 });


                    return Galatee.Tools.Utility.ListToDataTable<object>(query);

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static DataTable RetourneListeRecuDeCaisse_EnCour(int pCaisse)
        {
            try
            {
                using (galadbEntities ctontext = new galadbEntities())
                {
                    var transc = ctontext.TRANSCAISSE.Where(t => t.FK_IDHABILITATIONCAISSE == pCaisse).OrderBy(t => t.ACQUIT);
                    //var demAnnul = ctontext.DEMANDEANNULATION.Where(t => t.FK_IDHABILITATIONCAISSE == pCaisse).OrderBy(t => t.ACQUIT);
                    IEnumerable<object> query = (from t in transc
                                                 //join n in demAnnul on new { t.ACQUIT  }
                                                 //equals new { ACQUIT = n.ACQUIT  } into _RecuCaisse
                                                 //from p in _RecuCaisse.DefaultIfEmpty()
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
                                                     t.ISDEMANDEANNULATION,
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
                                                     MONTANTPAYE = t.MONTANT,
                                                     LIBELLEAGENCE = t.HABILITATIONCAISSE.CENTRE1.LIBELLE,
                                                     LIBELLESITE = t.HABILITATIONCAISSE.CENTRE1.SITE.LIBELLE,
                                                     LIBELLECOPER = string.IsNullOrEmpty(t.NUMDEM) ? t.LCLIENT.COPER1.LIBELLE :
                                                                    ctontext.RUBRIQUEDEMANDE.FirstOrDefault(c => t.NUMDEM == c.NUMDEM && t.REFEM == c.REFEM && t.NDOC == c.NDOC).COPER1.LIBELLE,
                                                     NOM = string.IsNullOrEmpty(t.NUMDEM) ? ctontext.CLIENT.FirstOrDefault(c => c.REFCLIENT == t.CLIENT && c.CENTRE == t.CENTRE && c.ORDRE == t.ORDRE).NOMABON :
                                                           ctontext.DCLIENT.FirstOrDefault(c => t.NUMDEM == c.NUMDEM).NOMABON, 
                                                     IsDEMANDEANNULATION = t.ISDEMANDEANNULATION
                                                     //,
                                                     //p.MOTIFDEMANDE ,
                                                     //p.MOTIFREJET ,
                                                     //p.STATUS 
                                                 });


                    return Galatee.Tools.Utility.ListToDataTable<object>(query);

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable RetourneListeDesRecuManuelNonAnnules(string pCaisse)
        {
            try
            {
                using (galadbEntities ctontext = new galadbEntities())
                {
                    var transc = ctontext.TRANSCAISSE.Where(t => t.CAISSE == pCaisse).OrderBy(t => t.ACQUIT);

                    List<TRANSCAISSE> listeTrans = new List<TRANSCAISSE>();
                    foreach (var t in transc)
                    {
                        if ((t.TOPANNUL != Enumere.CaisseTopAnnulation || string.IsNullOrEmpty(t.TOPANNUL)) &&
                            t.COPER != Enumere.CoperAjsutemenFondCaisse && t.COPER != Enumere.CoperFondCaisse && !string.IsNullOrEmpty(t.SAISIPAR))
                            listeTrans.Add(t);
                    }

                    IEnumerable<object> query = (from t in listeTrans
                                                 select new
                                                 {
                                                     t.ACQUIT,
                                                     t.CAISSE,
                                                     t.MATRICULE,
                                                     REFFERENCEACQUIT = t.CAISSE + t.ACQUIT + t.MATRICULE,
                                                 }).Distinct();


                    return Galatee.Tools.Utility.ListToDataTable<object>(query);

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static DataTable RetourneSuppervisionCaisse(List<int> pCenrtre, DateTime? dateCaisse)
        {
            try
            {
                using (galadbEntities ctontext = new galadbEntities())
                {
                    var transc = ctontext.CAISSE.Where(t => pCenrtre.Contains(t.FK_IDCENTRE));
                    IEnumerable<object> query = (from f in transc
                                                 join n in ctontext.HABILITATIONCAISSE  on new { f.PK_ID }
                                                 equals new {PK_ID= n.FK_IDCAISSE } into _habilCaisse
                                                 from p in _habilCaisse.DefaultIfEmpty()
                                                 select new
                                                 {
                                                     SITECAISSE=f.CENTRE1.CODESITE  + " " +  f.CENTRE1.SITE.LIBELLE   ,
                                                     CENTRE =f.CENTRE + " " + f.CENTRE1.LIBELLE ,
                                                     f.NUMCAISSE ,
                                                     NOMCAISSE=  p.ADMUTILISATEUR.LIBELLE ,
                                                     ESTATRIBUER= f.ESTATTRIBUEE ,
                                                     PK_ID = p.PK_ID == null ? 0 : p.PK_ID,
                                                     p.DATE_DEBUT,
                                                     p.DATE_FIN 
                                                 }).Distinct();

                    return Galatee.Tools.Utility.ListToDataTable<object>(query);

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static DataTable LigneFactureClient( int pFforeignkey)
        {
            try
            {
  
               using (galadbEntities ctontext = new galadbEntities())
                {
                    var LAfACTURE= ctontext.LCLIENT   ;
                    IEnumerable<object> query = (from t in LAfACTURE
                                                 where t.FK_IDCLIENT  == pFforeignkey 
                                                 select new
                                                 {
                                                    t. PK_ID ,
                                                    t.  CENTRE ,
                                                    t.  CLIENT ,
                                                    t.  ORDRE ,
                                                    t.  REFEM ,
                                                    t.  NDOC ,
                                                    t.  COPER ,
                                                    t. DENR ,
                                                    t. EXIG ,
                                                    t. MONTANT ,
                                                    t.  CAPUR ,
                                                    t.  CRET ,
                                                    t.  MODEREG ,
                                                    t.  DC ,
                                                    t.  ORIGINE ,
                                                    t.  CAISSE ,
                                                    t. ECART ,
                                                    t.  MOISCOMPT ,
                                                    t.  TOP1 ,
                                                    t.  EXIGIBILITE ,
                                                    t. FRAISDERETARD ,
                                                    t. REFERENCEPUPITRE ,
                                                    t. IDLOT ,
                                                    t.  DATEVALEUR ,
                                                    t.  REFERENCE ,
                                                    t.  REFEMNDOC ,
                                                    t.  ACQUIT ,
                                                    t.  MATRICULE ,
                                                    t. TAXESADEDUIRE ,
                                                    t.  DATEFLAG ,
                                                    t. MONTANTTVA ,
                                                    t.  IDCOUPURE ,
                                                    t.  AGENT_COUPURE ,
                                                    t. RDV_COUPURE ,
                                                    t. NUMCHEQ ,
                                                    t. OBSERVATION_COUPURE ,
                                                    t. USERCREATION ,
                                                    t. DATECREATION ,
                                                    t. DATEMODIFICATION ,
                                                    t. USERMODIFICATION ,
                                                    t. BANQUE ,
                                                    t. GUICHET ,
                                                    t. FK_IDCENTRE ,
                                                    t. FK_IDADMUTILISATEUR ,
                                                    t. FK_IDCOPER ,
                                                    t. FK_IDLIBELLETOP ,
                                                    t. FK_IDCLIENT ,
                                                    t. FK_IDPOSTE ,
                                                    t.  POSTE ,
                                                    t.  DATETRANS ,
                                                    //t. IDMORATOIRE ,
                                                    t. FK_IDMORATOIRE ,
                                                    LIBELLECOPER = t.COPER1.LIBELLE,
                                                    LIBELLENATURE = t.COPER1.LIBCOURT,
                                                NOM=t.CLIENT1.NOMABON ,
                                                ADRESSE = t.CLIENT1.ADRMAND1,
                                                t.ISNONENCAISSABLE 
                                                 });

                    return Galatee.Tools.Utility.ListToDataTable<object>(query);

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable LigneFactureClient(List<int> idregroupement,List<string> LstPeriode)
        {
            try
            {

                using (galadbEntities ctontext = new galadbEntities())
                {
                    var LAfACTURE = ctontext.LCLIENT;
                    IEnumerable<object> query = (from t in LAfACTURE
                                                 where idregroupement.Contains(t.CLIENT1.FK_IDREGROUPEMENT.Value) && LstPeriode.Contains(t.REFEM)
                                                 select new
                                                 {
                                                     t.PK_ID,
                                                     t.CENTRE,
                                                     t.CLIENT,
                                                     t.ORDRE,
                                                     t.REFEM,
                                                     t.NDOC,
                                                     t.COPER,
                                                     t.DENR,
                                                     t.EXIG,
                                                     t.MONTANT,
                                                     t.CAPUR,
                                                     t.CRET,
                                                     t.MODEREG,
                                                     t.DC,
                                                     t.ORIGINE,
                                                     t.CAISSE,
                                                     t.ECART,
                                                     t.MOISCOMPT,
                                                     t.TOP1,
                                                     t.EXIGIBILITE,
                                                     t.FRAISDERETARD,
                                                     t.REFERENCEPUPITRE,
                                                     t.IDLOT,
                                                     t.DATEVALEUR,
                                                     t.REFERENCE,
                                                     t.REFEMNDOC,
                                                     t.ACQUIT,
                                                     t.MATRICULE,
                                                     t.TAXESADEDUIRE,
                                                     t.DATEFLAG,
                                                     t.MONTANTTVA,
                                                     t.IDCOUPURE,
                                                     t.AGENT_COUPURE,
                                                     t.RDV_COUPURE,
                                                     t.NUMCHEQ,
                                                     t.OBSERVATION_COUPURE,
                                                     t.USERCREATION,
                                                     t.DATECREATION,
                                                     t.DATEMODIFICATION,
                                                     t.USERMODIFICATION,
                                                     t.BANQUE,
                                                     t.GUICHET,
                                                     t.FK_IDCENTRE,
                                                     t.FK_IDADMUTILISATEUR,
                                                     t.FK_IDCOPER,
                                                     t.FK_IDLIBELLETOP,
                                                     t.FK_IDCLIENT,
                                                     t.FK_IDPOSTE,
                                                     t.POSTE,
                                                     t.DATETRANS,
                                                     //t.IDMORATOIRE,
                                                     t.FK_IDMORATOIRE,
                                                     LIBELLECOPER = t.COPER1.LIBELLE,
                                                     ADRESSE = t.CLIENT1.ADRMAND1,
                                                     NOM = t.CLIENT1.NOMABON
                                                 });

                    return Galatee.Tools.Utility.ListToDataTable<object>(query);

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable LigneFactureClient(List<int> idregroupement )
        {
            try
            {

                using (galadbEntities ctontext = new galadbEntities())
                {
                    var LAfACTURE = ctontext.LCLIENT;
                    IEnumerable<object> query = (from t in LAfACTURE
                                                 where idregroupement.Contains(t.CLIENT1.FK_IDREGROUPEMENT.Value)  
                                                 select new
                                                 {
                                                     t.PK_ID,
                                                     t.CENTRE,
                                                     t.CLIENT,
                                                     t.ORDRE,
                                                     t.REFEM,
                                                     t.NDOC,
                                                     t.COPER,
                                                     t.DENR,
                                                     t.EXIG,
                                                     t.MONTANT,
                                                     t.CAPUR,
                                                     t.CRET,
                                                     t.MODEREG,
                                                     t.DC,
                                                     t.ORIGINE,
                                                     t.CAISSE,
                                                     t.ECART,
                                                     t.MOISCOMPT,
                                                     t.TOP1,
                                                     t.EXIGIBILITE,
                                                     t.FRAISDERETARD,
                                                     t.REFERENCEPUPITRE,
                                                     t.IDLOT,
                                                     t.DATEVALEUR,
                                                     t.REFERENCE,
                                                     t.REFEMNDOC,
                                                     t.ACQUIT,
                                                     t.MATRICULE,
                                                     t.TAXESADEDUIRE,
                                                     t.DATEFLAG,
                                                     t.MONTANTTVA,
                                                     t.IDCOUPURE,
                                                     t.AGENT_COUPURE,
                                                     t.RDV_COUPURE,
                                                     t.NUMCHEQ,
                                                     t.OBSERVATION_COUPURE,
                                                     t.USERCREATION,
                                                     t.DATECREATION,
                                                     t.DATEMODIFICATION,
                                                     t.USERMODIFICATION,
                                                     t.BANQUE,
                                                     t.GUICHET,
                                                     t.FK_IDCENTRE,
                                                     t.FK_IDADMUTILISATEUR,
                                                     t.FK_IDCOPER,
                                                     t.FK_IDLIBELLETOP,
                                                     t.FK_IDCLIENT,
                                                     t.FK_IDPOSTE,
                                                     t.POSTE,
                                                     t.DATETRANS,
                                                     //t.IDMORATOIRE,
                                                     t.FK_IDMORATOIRE,
                                                     LIBELLECOPER = t.COPER1.LIBELLE,
                                                     ADRESSE = t.CLIENT1.ADRMAND1,
                                                     NOM = t.CLIENT1.NOMABON
                                                 });

                    return Galatee.Tools.Utility.ListToDataTable<object>(query);

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
      //public static List<LCLIENT> RetourneLigneDesFactureDues(string pCentre, string pClient, string pOrdre, int pForeingkey,galadbEntities pcontext)
      //  {
      //      try
      //      {
      //          List<LCLIENT> factureClient = new List<LCLIENT>();
      //          var ligne = LigneFactureClient(pForeingkey, pcontext);

      //          List<LCLIENT> client = new List<LCLIENT>();
      //          List<LCLIENT> tempon = new List<LCLIENT>();

      //          foreach (LCLIENT cl in ligne)
      //          {
      //              if (tempon.First(t => t.FK_IDCENTRE ==cl.FK_IDCENTRE && t.CENTRE == cl.CENTRE && t.CLIENT == cl.CLIENT && t.ORDRE == cl.ORDRE && t.NDOC == cl.NDOC) == null)
      //              {
      //                  decimal? solde = FonctionCaisse.RetourneSoldeDocument(cl.FK_IDCENTRE , cl.CENTRE, cl.CLIENT, cl.ORDRE, cl.NDOC,cl.REFEM );
      //                  if (solde > 0)
      //                  {
      //                      cl.MONTANTTVA = solde;
      //                      client.Add(cl);
      //                  }
      //                  tempon.Add(cl);
      //              }
                   
      //          }

      //          return client;
      //      }
      //      catch (Exception ex)
      //      {
      //          throw ex;
      //      }
      //  }

        //public static DataTable RetourneLigneFactureClients(string pCentre, string pClient, string pOrdre,int pForeingkey)
        //{
        //    try
        //    {
        //        galadbEntities context = new galadbEntities();
        //        var client = RetourneLigneDesFactureDues(pCentre, pClient, pOrdre, pForeingkey, context);
        //        using (galadbEntities ctontext = new galadbEntities())
        //        {
        //            IEnumerable<object> query = (from f in client
        //                                         select new
        //                                         {
        //                                             f.CENTRE,
        //                                             f.CLIENT,
        //                                             f.ORDRE,
        //                                             f.NDOC,
        //                                             f.REFEM,
        //                                             f.NATURE,
        //                                             f.COPER,
        //                                             f.DENR,
        //                                             f.DC,
        //                                             LIBELLENATURE=  f.NATURE1.LIBCOURT ,
        //                                             f.EXIGIBILITE,
        //                                             f.MONTANT,
        //                                             f.ECART,
        //                                             SOLDEFACTURE = f.MONTANTTVA,
        //                                             MONTANTFACTURE = f.MONTANT,
        //                                             f.CRET,
        //                                             ACQUITANTERIEUR = f.ACQUIT,
        //                                             f.MOISCOMPT,
        //                                             PK_IDCLIENT = f.PK_ID
        //                                         }).Distinct();

        //            context.Dispose();

        //            return Galatee.Tools.Utility.ListToDataTable<object>(query);

        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}


         public static DataTable RetourneLigneFactureClients(string pCentre, string pClient, string pOrdre)
        {
            try
            {
                //cmd.CommandText = "SPX_ENC_RETOURNELIGNEFACTURE";
                using (galadbEntities ctontext = new galadbEntities())
                {
                    var factureClient = ctontext.LCLIENT.Where(cl => cl.CENTRE == pCentre && cl.CLIENT== pClient && cl.ORDRE == pOrdre &&
                                                                 cl.TOP1 != Enumere.CaisseModule && cl.DC == Enumere.Debit && cl.MONTANT > 0);

                    List<LCLIENT> client = new List<LCLIENT>();
                    foreach (LCLIENT cl in factureClient)
                    {
                        decimal? solde = FonctionCaisse.RetourneSoldeDocument(cl.FK_IDCENTRE, cl.CENTRE, cl.CLIENT, cl.ORDRE, cl.NDOC,cl.REFEM);
                        if (solde > 0)
                        {
                            cl.MONTANTTVA = solde;
                            client.Add(cl);
                        }
                    }

                    IEnumerable<object> query = (from f in client
                                                 select new
                                                 {
                                                     f.CENTRE,
                                                     f.CLIENT,
                                                     f.ORDRE,
                                                     f.NDOC,
                                                     f.REFEM,
                                                     f.COPER,
                                                     f.DENR,
                                                     f.DC,
                                                     f.EXIGIBILITE,
                                                     f.MONTANT,
                                                     f.ECART,// il s'agit du montant d'une pénalité éventuelle
                                                     SOLDEFACTURE = f.MONTANTTVA,
                                                     MONTANTFACTURE = f.MONTANT,
                                                     f.CRET,
                                                     ACQUITANTERIEUR = f.ACQUIT,
                                                     f.MOISCOMPT,
                                                     f.PK_ID
                                                 }).Distinct();


                    return Galatee.Tools.Utility.ListToDataTable<object>(query);

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable RetourneListePaiementDuRecuDuReglementNormale(string caisse, string acquit, string MatriculeConect)
        {
            try
            {
                //cmd.CommandText = "SPX_ENC_PAIEMENTDURECU";
                // confusion LUDOVIC
                using (galadbEntities ctontext = new galadbEntities())
                {
                    var Transcaisse = ctontext.TRANSCAISSE.Where(t => t.CAISSE == caisse && t.ACQUIT == acquit
                                                                 && t.MATRICULE == MatriculeConect && (t.COPER == Enumere.CoperNAF ||
                                                                 t.COPER == Enumere.CoperRGT));


                    //TRANSCAISSE transcSingle = Transcaisse.First();

                    var modreg = ctontext.MODEREG;
                    var _client = ctontext.CLIENT;

                    IEnumerable<object> query = (from pTrans in Transcaisse
                                                 join m in modreg on pTrans.FK_IDMODEREG equals m.PK_ID
                                                 join c in _client on new { pTrans.CENTRE, pTrans.CLIENT, pTrans.ORDRE }
                                                 equals new { CENTRE = c.CENTRE, CLIENT = c.REFCLIENT, ORDRE = c.ORDRE }
                                                 group new { pTrans } by
                                                       new
                                                       {
                                                           pTrans.CENTRE,
                                                           pTrans.CLIENT,
                                                           pTrans.ORDRE,
                                                           pTrans.REFEM,
                                                           pTrans.NDOC,
                                                           pTrans.DC,
                                                           pTrans.PERCU,
                                                           pTrans.RENDU,
                                                           pTrans.ECART,
                                                           pTrans.FK_IDMODEREG,
                                                           pTrans.MODEREG,
                                                           pTrans.TOPANNUL,
                                                           pTrans.MOISCOMPT,
                                                           pTrans.FK_IDCOPER,
                                                           pTrans.COPER,
                                                           pTrans.NUMDEM,
                                                           pTrans.DTRANS,
                                                           pTrans.DATEENCAISSEMENT,
                                                           pTrans.MATRICULE,
                                                           //pTrans.FK_IDUTILISATEUR,
                                                           pTrans.CAISSE,
                                                           pTrans.ACQUIT,
                                                           c.NOMABON,
                                                           m.LIBELLE,
                                                           pTrans.MONTANT
                                                       } into p
                                                 select new
                                                 {
                                                     p.Key.CENTRE,
                                                     p.Key.CLIENT,
                                                     p.Key.ORDRE,
                                                     p.Key.REFEM,
                                                     p.Key.NDOC,
                                                     p.Key.DC,
                                                     p.Key.PERCU,
                                                     p.Key.RENDU,
                                                     p.Key.ECART,
                                                     LIBELLEMODREG = p.Key.LIBELLE,
                                                     p.Key.TOPANNUL,
                                                     MONTANTNAF = p.Key.MONTANT,
                                                     p.Key.MOISCOMPT,
                                                     p.Key.FK_IDCOPER,
                                                     p.Key.NUMDEM,
                                                     p.Key.DTRANS,
                                                     p.Key.DATEENCAISSEMENT,
                                                     //p.Key.NATURE ,
                                                     p.Key.MATRICULE,
                                                     p.Key.CAISSE,
                                                     p.Key.ACQUIT,
                                                     NOMCLIENT = p.Key.NOMABON,
                                                     p.Key.FK_IDMODEREG,
                                                     MONTANTPAYE = (decimal?)p.Sum(o => o.pTrans.MONTANT)
                                                 });

                    return Galatee.Tools.Utility.ListToDataTable<object>(query);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable RetourneLigneNafClients(string pCentre, string pClient, string pOrdre)
        {
            try
            {
                //cmd.CommandText = "SPX_ENC_RETOURNELIGNEFACTURE";
                using (galadbEntities ctontext = new galadbEntities())
                {
                    var factureClient = ctontext.LCLIENT.Where(cl => cl.CENTRE == pCentre && cl.CLIENT == pClient && cl.ORDRE == pOrdre && cl.COPER == Enumere.CoperNAF);
                    List<LCLIENT> lclient = new List<LCLIENT>();
                    foreach (LCLIENT cl in factureClient)
                    {
                        Decimal? soldenaf = FonctionCaisse.RetourneSoldeNAF(cl.CENTRE, cl.CLIENT, cl.ORDRE, cl.NDOC);
                        if (soldenaf < 0)
                        {
                            cl.TAXESADEDUIRE = soldenaf;
                            lclient.Add(cl);
                        }
                    }

                    IEnumerable<object> query = (from f in lclient
                                                 select new
                                                 {
                                                     f.CENTRE,
                                                     f.CLIENT,
                                                     f.ORDRE,
                                                     f.NDOC,
                                                     f.REFEM,
                                                     f.COPER,
                                                     f.DENR,
                                                     f.DC,
                                                     f.EXIGIBILITE,
                                                     MONTANTFACTURE = f.MONTANT,
                                                     MONTANTNAF = f.MONTANT,
                                                     f.ECART,// il s'agit du montant d'une pénalité éventuelle
                                                     SOLDEFACTURE = f.MONTANTTVA,
                                                     f.CRET,
                                                     f.MOISCOMPT,
                                                     PK_ID = f.PK_ID,
                                                     ACQUITANTERIEUR = f.ACQUIT,
                                                     PK_IDCLIENT = f.FK_IDCLIENT

                                                 }).Distinct();
                    return Galatee.Tools.Utility.ListToDataTable<object>(query);

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool OuvertureCaisseEnLigne(DateTime? date, string matriculeCaissier, string matriculeOperateurOuverture, string numcaisse, string raison)
        {
            try
            {
                using (galadbEntities ctontext = new galadbEntities())
                {
                    var transcB = ctontext.TRANSCAISB.Where(t => t.MATRICULE == matriculeCaissier && t.DTRANS == date);

                    List<TRANSCAISSE> transcBList = new List<TRANSCAISSE>();


                    foreach (var t in transcB)
                    {
                        if (!string.IsNullOrEmpty(t.SAISIPAR))
                            continue;

                        TRANSCAISSE transB = new TRANSCAISSE();
                        transB.ACQUIT = t.ACQUIT;
                        transB.CENTRE = t.CENTRE;
                        transB.CAISSE = t.CAISSE;
                        transB.CLIENT = t.CLIENT;
                        transB.ORDRE = t.ORDRE;
                        transB.MATRICULE = t.MATRICULE;
                        transB.NDOC = t.NDOC;
                        transB.REFEM = t.REFEM;
                        transB.MONTANT = t.MONTANT;
                        transB.DC = t.DC;
                        transB.COPER = t.COPER;
                        transB.PERCU = t.PERCU;
                        transB.RENDU = t.RENDU;
                        transB.MODEREG = t.MODEREG;
                        transB.PLACE = t.PLACE;
                        transB.DTRANS = t.DTRANS;
                        transB.DEXIG = t.DEXIG;
                        transB.BANQUE = t.BANQUE;
                        transB.GUICHET = t.BANQUE;
                        transB.ORIGINE = t.ORIGINE;
                        transB.ECART = t.ECART;
                        transB.TOPANNUL = t.TOPANNUL;
                        transB.MOISCOMPT = t.MOISCOMPT;
                        transB.TOP1 = t.TOP1;
                        transB.TOURNEE = t.TOURNEE;
                        transB.NUMDEM = t.NUMDEM;
                        transB.NUMCHEQ = t.NUMCHEQ;
                        transB.SAISIPAR = t.SAISIPAR;
                        transB.DATEENCAISSEMENT = t.DATEENCAISSEMENT;
                        transB.DATECREATION = t.DATECREATION;
                        transB.USERCREATION = t.USERCREATION;
                        transB.DATEVALEUR = t.DATEVALEUR;
                        transB.CANCELLATION = t.CANCELLATION;
                        
                        // valorisation des foreign key

                        transB.FK_IDCAISSIERE = t.FK_IDCAISSIERE ;
                        transB.FK_IDCENTRE = t.FK_IDCENTRE;
                        transB.FK_IDCOPER = t.FK_IDCOPER;
                        transB.FK_IDLIBELLETOP = t.FK_IDLIBELLETOP;
                        transB.FK_IDMODEREG = t.FK_IDMODEREG.Value ;
                        transB.FK_IDHABILITATIONCAISSE = t.FK_IDHABILITATIONCAISSE.Value  ;

                        transcBList.Add(transB);
                    }

                    // insertion des encaissement dans transcaissb
                    foreach (TRANSCAISSE _entity in transcBList)
                    {
                        if (ctontext.Entry(_entity).State == EntityState.Detached)
                            ctontext.Set<TRANSCAISSE>().Add(_entity);
                    }

                    // suppression des encaissments de la caisse dans la table TranscaisseB
                    foreach (TRANSCAISB _entity in transcB)
                    {
                        if (ctontext.Entry(_entity).State == EntityState.Detached)
                            ctontext.Set<TRANSCAISB>().Add(_entity);
                        ctontext.Set<TRANSCAISB>().Remove(_entity);
                    }

                    // insertion dans la table OPENINGDAY 

                    OPENINGDAY open = new OPENINGDAY();
                    open.WHEN = DateTime.Now.Date;
                    open.WHO = matriculeOperateurOuverture;
                    open.CASHIER = matriculeCaissier;
                    open.DAY = date.Value;
                    open.OPEN = Enumere.TopOuvertureCaisse;
                    open.WHY = raison;

                    open.USERCREATION = matriculeOperateurOuverture;
                    open.DATECREATION = DateTime.Now.Date;

                    if (ctontext.Entry(open).State == EntityState.Detached)
                        ctontext.Set<OPENINGDAY>().Add(open);

                    ctontext.SaveChanges();
                }

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool OuvertureCaisseSaisiManuel(DateTime? date, string matriculeCaissier, string matriculeClerk, string matriculeOperateurOuverture, string numcaisse, string raison)
        {
            try
            {
                using (galadbEntities ctontext = new galadbEntities())
                {
                    var transcB = ctontext.TRANSCAISB.Where(t => t.MATRICULE == matriculeCaissier && t.DTRANS == date);

                    List<TRANSCAISSE> transcBList = new List<TRANSCAISSE>();


                    foreach (var t in transcB)
                    {
                        if (string.IsNullOrEmpty(t.SAISIPAR))
                            continue;

                        TRANSCAISSE transB = new TRANSCAISSE();
                        transB.ACQUIT = t.ACQUIT;
                        transB.CENTRE = t.CENTRE;
                        transB.CAISSE = t.CAISSE;
                        transB.CLIENT = t.CLIENT;
                        transB.ORDRE = t.ORDRE;
                        transB.MATRICULE = t.MATRICULE;
                        transB.NDOC = t.NDOC;
                        transB.REFEM = t.REFEM;
                        transB.MONTANT = t.MONTANT;
                        transB.DC = t.DC;
                        transB.COPER = t.COPER;
                        transB.PERCU = t.PERCU;
                        transB.RENDU = t.RENDU;
                        transB.MODEREG = t.MODEREG;
                        transB.PLACE = t.PLACE;
                        transB.DTRANS = t.DTRANS;
                        transB.DEXIG = t.DEXIG;
                        transB.BANQUE = t.BANQUE;
                        transB.GUICHET = t.GUICHET;
                        transB.ORIGINE = t.ORIGINE;
                        transB.ECART = t.ECART;
                        transB.TOPANNUL = t.TOPANNUL;
                        transB.MOISCOMPT = t.MOISCOMPT;
                        transB.TOP1 = t.TOP1;
                        transB.TOURNEE = t.TOURNEE;
                        transB.NUMDEM = t.NUMDEM;
                        transB.NUMCHEQ = t.NUMCHEQ;
                        transB.SAISIPAR = t.SAISIPAR;
                        transB.DATEENCAISSEMENT = t.DATEENCAISSEMENT;
                        transB.DATECREATION = t.DATECREATION;
                        transB.USERCREATION = t.USERCREATION;
                        transB.DATEVALEUR = t.DATEVALEUR;
                        transB.CANCELLATION = t.CANCELLATION;

                        // valorisation des foreign key

                        transB.FK_IDCAISSIERE = t.FK_IDCAISSIERE ;
                        transB.FK_IDCENTRE = t.FK_IDCENTRE;
                        transB.FK_IDCOPER = t.FK_IDCOPER;
                        transB.FK_IDLIBELLETOP = t.FK_IDLIBELLETOP;
                        transB.FK_IDMODEREG = t.FK_IDMODEREG.Value ;
                        transB.FK_IDHABILITATIONCAISSE = t.FK_IDHABILITATIONCAISSE.Value;

                        transcBList.Add(transB);
                    }

                    // insertion des encaissement dans transcaissb
                    foreach (TRANSCAISSE _entity in transcBList)
                    {
                        if (ctontext.Entry(_entity).State == EntityState.Detached)
                            ctontext.Set<TRANSCAISSE>().Add(_entity);
                    }

                    // suppression des encaissments de la caisse dans la table TranscaisseB
                    foreach (TRANSCAISB _entity in transcB)
                    {
                        if (ctontext.Entry(_entity).State == EntityState.Detached)
                            ctontext.Set<TRANSCAISB>().Add(_entity);
                        ctontext.Set<TRANSCAISB>().Remove(_entity);
                    }

                    // insertion dans la table OPENINGDAY 

                    OPENINGDAY open = new OPENINGDAY();
                    open.WHEN = DateTime.Now.Date;
                    open.WHO = matriculeOperateurOuverture;
                    open.CASHIER = matriculeCaissier;
                    open.DAY = date.Value;
                    open.OPEN = Enumere.TopOuvertureCaisse;
                    open.WHY = raison;
                    open.KEYEDBY = matriculeClerk;

                    open.USERCREATION = matriculeOperateurOuverture;
                    open.DATECREATION = DateTime.Now.Date;

                    if (ctontext.Entry(open).State == EntityState.Detached)
                        ctontext.Set<OPENINGDAY>().Add(open);

                    ctontext.SaveChanges();
                }

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable RetourneClient(string pCentre, string pClient, string pOrdre)
        {
            try
            {
                //cmd.CommandText = "SPX_ENC_RETOURNELIGNEFACTURE";
                using (galadbEntities ctontext = new galadbEntities())
                {
                    var Client = ctontext.CLIENT;
                    //.Where(cl => cl.CENTRE == pCentre && cl.REFCLIENT == pClient && cl.ORDRE == pOrdre);
                    List<CLIENT> lclient = new List<CLIENT>();

                    IEnumerable<object> query = (from cl in Client
                                                 from abon in cl.ABON 
                                                 where (cl.CENTRE == pCentre && cl.REFCLIENT == pClient && cl.ORDRE == pOrdre)
                                                 select new
                                                 {
                                                    cl.CENTRE ,
                                                    cl.REFCLIENT ,
                                                    cl.ORDRE ,
                                                    cl.CODEIDENTIFICATIONNATIONALE ,
                                                    cl.DENABON ,
                                                    cl.NOMABON ,
                                                    cl.DENMAND ,
                                                    cl.NOMMAND ,
                                                    cl.ADRMAND1 ,
                                                    cl.ADRMAND2 ,
                                                    cl.CPOS ,
                                                    cl.BUREAU ,
                                                    cl.DINC ,
                                                    cl.MODEPAIEMENT ,
                                                    cl.NOMTIT ,
                                                    cl.BANQUE ,
                                                    cl.GUICHET ,
                                                    cl.COMPTE ,
                                                    cl.RIB ,
                                                    cl.PROPRIO ,
                                                    cl.CODECONSO ,
                                                    cl.CATEGORIE ,
                                                    cl.CODERELANCE ,
                                                    cl.NOMCOD ,
                                                    cl.MOISNAIS ,
                                                    cl.ANNAIS ,
                                                    cl.NOMPERE ,
                                                    cl.NOMMERE ,
                                                    cl.NATIONNALITE ,
                                                    cl.CNI ,
                                                    cl.TELEPHONE ,
                                                    cl.MATRICULE ,
                                                    cl.REGROUPEMENT ,
                                                    cl.REGEDIT ,
                                                    cl.FACTURE ,
                                                    cl.DMAJ ,
                                                    cl.REFERENCEPUPITRE ,
                                                    cl.PAYEUR ,
                                                    cl.SOUSACTIVITE ,
                                                    cl.AGENTFACTURE ,
                                                    cl.AGENTRECOUVR ,
                                                    cl.AGENTASSAINI ,
                                                    cl.REGROUCONTRAT ,
                                                    cl.INSPECTION ,
                                                    cl.REGLEMENT ,
                                                    cl.DECRET ,
                                                    cl.CONVENTION ,
                                                    cl.REFERENCEATM ,
                                                    cl.PK_ID ,
                                                    cl.DATECREATION ,
                                                    cl.DATEMODIFICATION ,
                                                    cl.USERCREATION ,
                                                    cl.USERMODIFICATION ,
                                                    cl.FK_IDMODEPAIEMENT ,
                                                    cl.FK_IDCODECONSO ,
                                                    cl.FK_IDCATEGORIE ,
                                                    cl.FK_IDRELANCE ,
                                                    cl.FK_IDNATIONALITE ,
                                                    cl.FK_IDCENTRE ,
                                                    cl.EMAIL ,
                                                    cl.ISFACTUREEMAIL ,
                                                    cl.ISFACTURESMS ,
                                                    cl.FK_IDPAYEUR ,
                                                    cl.FK_IDREGROUPEMENT ,
                                                    cl.FK_IDAG,
                                                    CODESITE = cl.CENTRE1.SITE.CODE,
                                                    LIBELLESITE = cl.CENTRE1.SITE.LIBELLE,
                                                    FK_IDSITE = cl.CENTRE1.SITE.PK_ID
                                                    //,
                                                    //abon.DRES 
                                                   
                                                 }).Distinct();
                    return Galatee.Tools.Utility.ListToDataTable<object>(query);

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable RetourneClientByIdReg(List<int> ListIdReg)
        {
            try
            {
                using (galadbEntities ctontext = new galadbEntities())
                {
                    var Client = ctontext.CLIENT;
                    List<CLIENT> lclient = new List<CLIENT>();
                    IEnumerable<object> query = (from cl in Client
                                                 where (ListIdReg.Contains(cl.FK_IDREGROUPEMENT.Value ))
                                                 select new
                                                 {
                                                     cl.CENTRE,
                                                     cl.REFCLIENT,
                                                     cl.ORDRE,
                                                     cl.CODEIDENTIFICATIONNATIONALE,
                                                     cl.DENABON,
                                                     cl.NOMABON,
                                                     cl.DENMAND,
                                                     cl.NOMMAND,
                                                     cl.ADRMAND1,
                                                     cl.ADRMAND2,
                                                     cl.CPOS,
                                                     cl.BUREAU,
                                                     cl.DINC,
                                                     cl.MODEPAIEMENT,
                                                     cl.NOMTIT,
                                                     cl.BANQUE,
                                                     cl.GUICHET,
                                                     cl.COMPTE,
                                                     cl.RIB,
                                                     cl.PROPRIO,
                                                     cl.CODECONSO,
                                                     cl.CATEGORIE,
                                                     cl.CODERELANCE,
                                                     cl.NOMCOD,
                                                     cl.MOISNAIS,
                                                     cl.ANNAIS,
                                                     cl.NOMPERE,
                                                     cl.NOMMERE,
                                                     cl.NATIONNALITE,
                                                     cl.CNI,
                                                     cl.TELEPHONE,
                                                     cl.MATRICULE,
                                                     cl.REGROUPEMENT,
                                                     cl.REGEDIT,
                                                     cl.FACTURE,
                                                     cl.DMAJ,
                                                     cl.REFERENCEPUPITRE,
                                                     cl.PAYEUR,
                                                     cl.SOUSACTIVITE,
                                                     cl.AGENTFACTURE,
                                                     cl.AGENTRECOUVR,
                                                     cl.AGENTASSAINI,
                                                     cl.REGROUCONTRAT,
                                                     cl.INSPECTION,
                                                     cl.REGLEMENT,
                                                     cl.DECRET,
                                                     cl.CONVENTION,
                                                     cl.REFERENCEATM,
                                                     cl.PK_ID,
                                                     cl.DATECREATION,
                                                     cl.DATEMODIFICATION,
                                                     cl.USERCREATION,
                                                     cl.USERMODIFICATION,
                                                     cl.FK_IDMODEPAIEMENT,
                                                     cl.FK_IDCODECONSO,
                                                     cl.FK_IDCATEGORIE,
                                                     cl.FK_IDRELANCE,
                                                     cl.FK_IDNATIONALITE,
                                                     cl.FK_IDCENTRE,
                                                     cl.EMAIL,
                                                     cl.ISFACTUREEMAIL,
                                                     cl.ISFACTURESMS,
                                                     cl.FK_IDPAYEUR,
                                                     cl.FK_IDREGROUPEMENT,
                                                     cl.FK_IDAG,
                                                     CODESITE = cl.CENTRE1.SITE.CODE,
                                                     LIBELLESITE = cl.CENTRE1.SITE.LIBELLE,
                                                     FK_IDSITE = cl.CENTRE1.SITE.PK_ID
                                                 });
                    return Galatee.Tools.Utility.ListToDataTable<object>(query);

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable RetourneListeDemandesPourCaisse(string NumDemande)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    var demande = context.DEMANDE.Where(d => d.NUMDEM == NumDemande && d.DCAISSE == null);
                    var detaildemande = context.RUBRIQUEDEMANDE.Where(d => d.NUMDEM == NumDemande && d.DATECAISSE == null);
                    var demandeworkflow = context.DEMANDE_WORFKLOW.Where(d => d.CODE_DEMANDE_TABLETRAVAIL == NumDemande);
                    var etapeWkf = context.ETAPE;
                    DEMANDE Lademande = context.DEMANDE.FirstOrDefault(d => d.NUMDEM == NumDemande);
                    string NomClient = string.Empty;
                    int IdClient = 1;
                    if ((Lademande != null && !string.IsNullOrEmpty(Lademande.NUMDEM)) &&
                        (Lademande.TYPEDEMANDE == Enumere.BranchementAbonement ||
                        Lademande.TYPEDEMANDE == Enumere.BranchementAbonnementMt ||
                        Lademande.TYPEDEMANDE == Enumere.AbonnementSeul ||
                        Lademande.TYPEDEMANDE == Enumere.TransfertSiteNonMigre ||
                        Lademande.TYPEDEMANDE == Enumere.TransfertAbonnement ||  /* Affciher le nom du nouveau client a la caisse*/
                        Lademande.TYPEDEMANDE == Enumere.BranchementAbonementExtention ||
                        Lademande.TYPEDEMANDE == Enumere.RemboursementTrvxNonRealise ||
                        Lademande.TYPEDEMANDE == Enumere.BranchementSimple))
                    {
                        DCLIENT dclient = context.DCLIENT.FirstOrDefault(t => t.NUMDEM == NumDemande);
                        if (dclient != null && !string.IsNullOrEmpty(dclient.NOMABON))
                            NomClient = dclient.NOMABON;
                    }
                    else
                    {
                        CLIENT client = context.CLIENT.FirstOrDefault(t => t.CENTRE == Lademande.CENTRE && t.REFCLIENT == Lademande.CLIENT && t.ORDRE == Lademande.ORDRE);
                        if (client != null)
                        {
                            NomClient = client.NOMABON;
                            IdClient = client.PK_ID;
                        }
                    }
                    IEnumerable<object> query = from d in demande
                                                join dt in detaildemande on d.NUMDEM equals dt.NUMDEM
                                                join dmd in demandeworkflow on d.NUMDEM equals dmd.CODE_DEMANDE_TABLETRAVAIL
                                                join wkfEtap in etapeWkf on dmd.FK_IDETAPEACTUELLE  equals wkfEtap.PK_ID
                                                select new
                                                {
                                                    dt.CENTRE,
                                                    dt.CLIENT,
                                                    DENR = d.DCAISSE,
                                                    dt.ORDRE,
                                                    dt.COPER,
                                                    dt.TAXE,
                                                    MONTANTTVA = dt.MONTANTTAXE,
                                                    MONTANT = dt.MONTANTHT,
                                                    SOLDEFACTURE = dt.MONTANTTAXE + dt.MONTANTHT,
                                                    dt.NDOC,
                                                    dt.REFEM,
                                                    dt.USERCREATION,
                                                    dt.DATECREATION,
                                                    dt.NUMDEM,
                                                    d.STATUT,
                                                    NOM = NomClient,
                                                    dt.FK_IDCENTRE,
                                                    Li = dt.COPER1.LIBELLE,
                                                    LIBELLECOPER = dt.COPER1.LIBELLE,
                                                    FK_IDCLIENT = IdClient,
                                                    CODE_WKF = dmd.CODE,
                                                    d.TYPEDEMANDE,
                                                    ISPRESTATIONSEULEMENT = dt.ISEXTENSION,
                                                    NUMEROLOT = wkfEtap.CODE  /* Etape de la demande */
                                                };
                    return Galatee.Tools.Utility.ListToDataTable<object>(query.Distinct());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        //public static DataTable RetourneListeDemandesPourCaisse(string NumDemande)
        //{
        //    try
        //    {
        //        using (galadbEntities context = new galadbEntities())
        //        {
        //            var demande         = context.DEMANDE.Where(d => d.NUMDEM == NumDemande && d.DCAISSE == null);
        //            var detaildemande   = context.RUBRIQUEDEMANDE.Where(d => d.NUMDEM == NumDemande && d.DATECAISSE == null);
        //            var demandeworkflow = context.DEMANDE_WORFKLOW.Where(d => d.CODE_DEMANDE_TABLETRAVAIL == NumDemande);
        //            //var etapeWkf = context.ETAPE.Where(o => o.CODE == "ENCAI");
        //            DEMANDE Lademande   = context.DEMANDE.FirstOrDefault(d => d.NUMDEM == NumDemande);
        //            string NomClient = string.Empty;
        //            int IdClient = 1;
        //            if ((Lademande != null && !string.IsNullOrEmpty(Lademande.NUMDEM)) &&
        //                (Lademande.TYPEDEMANDE == Enumere.BranchementAbonement ||
        //                Lademande.TYPEDEMANDE == Enumere.BranchementAbonnementMt ||
        //                Lademande.TYPEDEMANDE == Enumere.AbonnementSeul ||
        //                Lademande.TYPEDEMANDE == Enumere.TransfertSiteNonMigre  ||
        //                Lademande.TYPEDEMANDE == Enumere.BranchementAbonementExtention ||
        //                Lademande.TYPEDEMANDE == Enumere.RemboursementTrvxNonRealise ||
        //                Lademande.TYPEDEMANDE == Enumere.BranchementSimple))
        //            {
        //                DCLIENT dclient = context.DCLIENT.FirstOrDefault(t => t.NUMDEM == NumDemande);
        //                if (dclient != null && !string.IsNullOrEmpty(dclient.NOMABON))
        //                    NomClient = dclient.NOMABON;
        //            }
        //            else
        //            {
        //                CLIENT client = context.CLIENT.FirstOrDefault(t => t.CENTRE == Lademande.CENTRE && t.REFCLIENT == Lademande.CLIENT && t.ORDRE == Lademande.ORDRE);
        //                if (client != null)
        //                {
        //                    NomClient = client.NOMABON;
        //                    IdClient = client.PK_ID;
        //                }
        //            }
        //            IEnumerable<object> query = from d in demande
        //                                        join dt in detaildemande on d.NUMDEM equals dt.NUMDEM
        //                                        join dmd in demandeworkflow on d.NUMDEM equals dmd.CODE_DEMANDE_TABLETRAVAIL
        //                                        //join wkfEtap in etapeWkf on dmd.FK_IDETAPEACTUELLE  equals wkfEtap.PK_ID
        //                                        select new
        //                                        {
        //                                            dt.CENTRE,
        //                                            dt.CLIENT,
        //                                            DENR = d.DCAISSE,
        //                                            dt.ORDRE,
        //                                            dt.COPER,
        //                                            dt.TAXE,
        //                                            MONTANTTVA = dt.MONTANTTAXE,
        //                                            MONTANT = dt.MONTANTHT,
        //                                            SOLDEFACTURE = dt.MONTANTTAXE + dt.MONTANTHT,
        //                                            dt.NDOC,
        //                                            dt.REFEM,
        //                                            dt.USERCREATION,
        //                                            dt.DATECREATION,
        //                                            dt.NUMDEM,
        //                                            d.STATUT,
        //                                            NOM = NomClient,
        //                                            d.FK_IDCENTRE,
        //                                            Li = dt.COPER1.LIBELLE,
        //                                            LIBELLECOPER = dt.COPER1.LIBELLE,
        //                                            FK_IDCLIENT = IdClient,
        //                                            CODE_WKF = dmd.CODE,
        //                                            d.TYPEDEMANDE,
        //                                            ISPRESTATIONSEULEMENT= dt.ISEXTENSION 
        //                                        };
        //            return Galatee.Tools.Utility.ListToDataTable<object>(query.Distinct());
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        public static DataTable RetourneListeDemandesExtensionPourCaisse(string NumDemande)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    var demande = context.DEMANDE.Where(d => d.NUMDEM == NumDemande && d.DCAISSE == null);
                    var detaildemande = context.RUBRIQUEDEMANDE.Where(d => d.NUMDEM == NumDemande && d.ISEXTENSION == true  && d.DATECAISSE == null );
                    var demandeworkflow = context.DEMANDE_WORFKLOW.Where(d => d.CODE_DEMANDE_TABLETRAVAIL == NumDemande);
                    DEMANDE  Lademande = context.DEMANDE.FirstOrDefault (d => d.NUMDEM == NumDemande);
                    string NomClient = string.Empty;
                    int IdClient = 1;
                    if ((Lademande != null && !string.IsNullOrEmpty(Lademande.NUMDEM)) &&
                        (Lademande.TYPEDEMANDE == Enumere.BranchementAbonement ||
                        Lademande.TYPEDEMANDE == Enumere.BranchementAbonnementMt ||
                        Lademande.TYPEDEMANDE == Enumere.BranchementAbonementExtention ||

                        Lademande.TYPEDEMANDE == Enumere.RemboursementTrvxNonRealise  ||
                        Lademande.TYPEDEMANDE == Enumere.BranchementSimple ))
                    {
                        DCLIENT dclient = context.DCLIENT.FirstOrDefault(t => t.NUMDEM == NumDemande);
                        if (dclient != null && !string.IsNullOrEmpty(dclient.NOMABON))
                            NomClient = dclient.NOMABON; 
                    }
                    else
                    {
                      CLIENT client = context.CLIENT.FirstOrDefault(t => t.CENTRE == Lademande.CENTRE && t.REFCLIENT == Lademande.CLIENT && t.ORDRE == Lademande.ORDRE);
                            NomClient = client.NOMABON;
                            IdClient = client.PK_ID;
                    }
                    IEnumerable<object> query = from d in demande
                                                join dt in detaildemande on d.NUMDEM equals dt.NUMDEM
                                                join dmd in demandeworkflow on  d.NUMDEM equals dmd.CODE_DEMANDE_TABLETRAVAIL
                                                select new
                                                {
                                                    dt.CENTRE,
                                                    dt.CLIENT,
                                                    DENR=  d.DCAISSE,
                                                    d.ORDRE,
                                                    dt.COPER,
                                                    dt.TAXE,
                                                    MONTANTTVA =    dt.MONTANTTAXE,
                                                    MONTANT=  dt.MONTANTHT,
                                                    SOLDEFACTURE = dt.MONTANTTAXE + dt.MONTANTHT,
                                                    dt.NDOC,
                                                    dt.REFEM,
                                                    dt.USERCREATION,
                                                    dt.DATECREATION,
                                                    dt.NUMDEM,
                                                    d.STATUT,
                                                    NOM = NomClient,
                                                    d.FK_IDCENTRE ,
                                                    Li = dt.COPER1.LIBELLE,
                                                    LIBELLECOPER = dt.COPER1.LIBELLE, 
                                                    FK_IDCLIENT = IdClient,
                                                    CODE_WKF = dmd.CODE,
                                                    d.TYPEDEMANDE 
                                                };

                    return Galatee.Tools.Utility.ListToDataTable<object>(query.Distinct());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable RetourneListePaiementDuRecuDuReglementNormaleOD(string caisse, string acquit, string MatriculeConect)
        {
            try
            {
                //cmd.CommandText = "SPX_ENC_PAIEMENTDURECU";
                // confusion LUDOVIC
                using (galadbEntities ctontext = new galadbEntities())
                {
                    return new DataTable();
                    //var Transcaisse = ctontext.TRANSCAISSE.Where(t => t.CAISSE == caisse && t.ACQUIT == acquit
                    //                                             && t.MATRICULE == MatriculeConect &&  t.COPER == Enumere.CoperOdQPA);


                    ////TRANSCAISSE transcSingle = Transcaisse.First();

                    //var modreg = ctontext.MODEREG;
                    //var _client = ctontext.DEMANDEDEVIS;

                    //IEnumerable<object> query = (from pTrans in Transcaisse
                    //                             join m in modreg on pTrans.FK_IDMODEREG equals m.PK_ID
                    //                             //join c in _client on pTrans.NUMDEVIS equals c.NUMDEVIS
                    //                             group new { pTrans } by
                    //                                   new
                    //                                   {
                    //                                       pTrans.CENTRE,
                    //                                       pTrans.CLIENT,
                    //                                       pTrans.ORDRE,
                    //                                       pTrans.REFEM,
                    //                                       pTrans.NDOC,
                    //                                       pTrans.NATURE,
                    //                                       pTrans.DC,
                    //                                       pTrans.PERCU,
                    //                                       pTrans.RENDU,
                    //                                       pTrans.ECART,
                    //                                       pTrans.FK_IDMODEREG,
                    //                                       pTrans.MODEREG,
                    //                                       pTrans.TOPANNUL,
                    //                                       pTrans.MOISCOMPT,
                    //                                       pTrans.FK_IDCOPER,
                    //                                       pTrans.COPER,
                    //                                       pTrans.NUMDEM,
                    //                                       pTrans.DTRANS,
                    //                                       pTrans.DATEENCAISSEMENT,
                    //                                       pTrans.MATRICULE,
                    //                                       pTrans.FK_IDHABILITATIONCAISSE,
                    //                                       pTrans.NUMDEVIS,
                    //                                       pTrans.FK_IDCAISSIERE,
                    //                                       pTrans.CAISSE,
                    //                                       pTrans.ACQUIT,
                    //                                       c.NOM,
                    //                                       m.LIBELLE,
                    //                                       pTrans.MONTANT
                    //                                   } into p
                    //                             select new
                    //                             {
                    //                                 p.Key.CENTRE,
                    //                                 p.Key.CLIENT,
                    //                                 p.Key.ORDRE,
                    //                                 p.Key.REFEM,
                    //                                 p.Key.NDOC,
                    //                                 p.Key.NATURE,
                    //                                 p.Key.DC,
                    //                                 p.Key.PERCU,
                    //                                 p.Key.RENDU,
                    //                                 p.Key.ECART,
                    //                                 LIBELLEMODREG = p.Key.LIBELLE,
                    //                                 p.Key.TOPANNUL,
                    //                                 MONTANTNAF = p.Key.MONTANT,
                    //                                 p.Key.MOISCOMPT,
                    //                                 p.Key.FK_IDCOPER,
                    //                                 p.Key.NUMDEM,
                    //                                 p.Key.DTRANS,
                    //                                 p.Key.DATEENCAISSEMENT,
                    //                                 //p.Key.NATURE ,
                    //                                 p.Key.MATRICULE,
                    //                                 p.Key.NUMDEVIS,
                    //                                 p.Key.CAISSE,
                    //                                 p.Key.ACQUIT,
                    //                                 NOMCLIENT = p.Key.NOM,
                    //                                 p.Key.FK_IDMODEREG,
                    //                                 MONTANTPAYE = (decimal?)p.Sum(o => o.pTrans.MONTANT)
                    //                             });

                    //return Galatee.Tools.Utility.ListToDataTable<object>(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable RetourneListePaiementDuRecuDuReglementDemande(string caisse, string acquit, string MatriculeConect)
        {
            try
            {
                //cmd.CommandText = "SPX_ENC_PAIEMENTDURECU";

                // Verifier si le client du recu n'est pas encore créé , dans l'optique de fait la jointure avec  DCLIENT ds le cas échéant
                using (galadbEntities ctontext = new galadbEntities())
                {
                    var Transcaisse = ctontext.TRANSCAISSE.Where(t => t.CAISSE == caisse && t.ACQUIT == acquit
                                                                 && t.MATRICULE == MatriculeConect);


                    //TRANSCAISSE transcSingle = Transcaisse.First();

                    var modreg = ctontext.MODEREG;
                    var _client = ctontext.DCLIENT;

                    IEnumerable<object> query = (from pTrans in Transcaisse
                                                 join m in modreg on pTrans.FK_IDMODEREG equals m.PK_ID
                                                 join c in _client on new { pTrans.CENTRE, pTrans.CLIENT, pTrans.ORDRE }
                                                                   equals new { c.CENTRE, CLIENT=c.REFCLIENT, c.ORDRE }
                                                 group new { pTrans } by
                                                       new
                                                       {
                                                           pTrans.CENTRE,
                                                           pTrans.CLIENT,
                                                           pTrans.ORDRE,
                                                           pTrans.REFEM,
                                                           pTrans.NDOC,
                                                           pTrans.DC,
                                                           pTrans.PERCU,
                                                           pTrans.RENDU,
                                                           pTrans.ECART,
                                                           pTrans.FK_IDMODEREG,
                                                           pTrans.MODEREG,
                                                           pTrans.TOPANNUL,
                                                           pTrans.MOISCOMPT,
                                                           pTrans.FK_IDCOPER,
                                                           pTrans.COPER,
                                                           pTrans.NUMDEM,
                                                           pTrans.DTRANS,
                                                           pTrans.DATEENCAISSEMENT,
                                                           pTrans.MATRICULE,
                                                           //pTrans.FK_IDUTILISATEUR,
                                                           pTrans.FK_IDCAISSIERE,
                                                           pTrans.CAISSE,
                                                           pTrans.ACQUIT,
                                                           c.NOMABON,
                                                           m.LIBELLE,
                                                           pTrans.MONTANT
                                                       } into p
                                                 select new
                                                 {
                                                     p.Key.CENTRE,
                                                     p.Key.CLIENT,
                                                     p.Key.ORDRE,
                                                     p.Key.REFEM,
                                                     p.Key.NDOC,
                                                     p.Key.DC,
                                                     p.Key.PERCU,
                                                     p.Key.RENDU,
                                                     p.Key.ECART,
                                                     LIBELLEMODREG = p.Key.LIBELLE,
                                                     p.Key.TOPANNUL,
                                                     MONTANTNAF = p.Key.MONTANT,
                                                     p.Key.MOISCOMPT,
                                                     p.Key.FK_IDCOPER,
                                                     p.Key.NUMDEM,
                                                     p.Key.DTRANS,
                                                     p.Key.DATEENCAISSEMENT,
                                                     //p.Key.NATURE ,
                                                     p.Key.MATRICULE,
                                                     p.Key.CAISSE,
                                                     p.Key.ACQUIT,
                                                     NOMCLIENT = p.Key.NOMABON,
                                                     p.Key.FK_IDMODEREG,
                                                     MONTANTPAYE = (decimal?)p.Sum(o => o.pTrans.MONTANT)
                                                 });

                    return Galatee.Tools.Utility.ListToDataTable<object>(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
       public static bool  MiseAjourDemandeAnnulation(CsReglement lereglement)
        {
            try
            {
             TRANSCAISSE transcaisse  = Entities.ConvertObject<TRANSCAISSE, CsReglement>(lereglement);
             return   Entities.UpdateEntity<TRANSCAISSE>(transcaisse);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

       public static CsHabilitationCaisse HabiliterCaisse(CsHabilitationCaisse laCaisseHAbil)
       {
           try
           {
               decimal? EcartDerniereCaisse = 0;
               CsHabilitationCaisse laDernierLigneCaisseHabil = RetourneDerniereCaisse(laCaisseHAbil);
               if (laDernierLigneCaisseHabil != null && !string.IsNullOrEmpty(laDernierLigneCaisseHabil.CENTRE))
                   EcartDerniereCaisse = laDernierLigneCaisseHabil.ECART;

               using (galadbEntities context = new galadbEntities())
               {
                   ADMUTILISATEUR user = context.ADMUTILISATEUR.FirstOrDefault(u => u.MATRICULE == laCaisseHAbil.MATRICULE);
                   CAISSE Caisse = context.CAISSE.FirstOrDefault(c => c.PK_ID  == laCaisseHAbil.FK_IDCAISSE);
                   HABILITATIONCAISSE habilitationCaisse = new HABILITATIONCAISSE
                   {
                        CENTRE = laCaisseHAbil.CENTRE, 
                        DATE_DEBUT = DateTime.Today.Date ,
                        FK_IDCENTRE = laCaisseHAbil.FK_IDCENTRE,
                        FK_IDCAISSE  = laCaisseHAbil.FK_IDCAISSE  ,
                        MATRICULE = laCaisseHAbil.MATRICULE ,
                        NUMCAISSE = laCaisseHAbil.NUMCAISSE,
                        ECART = EcartDerniereCaisse,
                        POSTE = laCaisseHAbil.POSTE,
                        FONDCAISSE = Caisse.FONDCAISSE,
                        FK_IDCAISSIERE  = laCaisseHAbil.FK_IDCAISSIERE  
                   };
                   Caisse.ESTATTRIBUEE  = true;
                   context.HABILITATIONCAISSE.Add( habilitationCaisse);
                   context.SaveChanges();
                   CsHabilitationCaisse laCaisse = Entities.ConvertObject<CsHabilitationCaisse, HABILITATIONCAISSE>(habilitationCaisse);
                   laCaisse.ACQUIT = Caisse.ACQUIT;
                   return laCaisse;
               }

           }
           catch (Exception ex)
           {
               throw ex;
           }
       }

       public static bool ReverserCaisse(List<CsReversementCaisse> laCaisseHAbil)
       {
           try
           {
               using (galadbEntities context = new galadbEntities())
               {
                   List<int> lstIdHabilCaisse = new List<int>();
                   foreach (var item in laCaisseHAbil)
                       lstIdHabilCaisse.Add(item.FK_IDHABILITATIONCAISSE);

                   List<HABILITATIONCAISSE> lst = context.HABILITATIONCAISSE.Where(t => lstIdHabilCaisse.Contains(t.PK_ID)).ToList();
                   lst.ForEach(t => t.ECART = 0);

                   Entities.InsertEntity<REVERSEMENTCAISSE>(Entities.ConvertObject<REVERSEMENTCAISSE, CsReversementCaisse >(laCaisseHAbil),context );
                   context.SaveChanges();
               }
               return true ;
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }

       public static decimal?   RetourneEcartDerniereCaisse(ref decimal? MontantEncaisse,  CsHabilitationCaisse laCaissehabil)
       {
           try
           {
               
               decimal? MontantReverser = 0;
               using (galadbEntities context = new galadbEntities())
               {
                   List<REVERSEMENTCAISSE> ListDesReversement = context.REVERSEMENTCAISSE.Where(u => u.FK_IDHABILITATIONCAISSE  == laCaissehabil.PK_ID).ToList();
                   if (ListDesReversement != null && ListDesReversement.Count != 0)
                       MontantReverser = ListDesReversement.Sum(p => p.MONTANT);
               } 
               if (laCaissehabil != null)
                   MontantEncaisse = RetourneEncaissementDate(laCaissehabil);

               return (MontantEncaisse - MontantReverser);
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }

       public static CsHabilitationCaisse  RetourneDerniereCaisse(CsHabilitationCaisse laCaissehabil)
       {
           try
           {

               CsHabilitationCaisse laDerniereCaisse = new CsHabilitationCaisse();
               using (galadbEntities context = new galadbEntities())
               {
                   var DernierCaisse = context.HABILITATIONCAISSE.Where(u => u.MATRICULE == laCaissehabil.MATRICULE && u.FK_IDCENTRE   == laCaissehabil.FK_IDCENTRE ).ToList();
                   if (DernierCaisse != null && DernierCaisse.Count != 0)
                   {
                       int maxCaisse = DernierCaisse.Max(t => t.PK_ID);

                       IEnumerable<object> query =
                          from x in DernierCaisse
                          where x.PK_ID == maxCaisse 
                          select new
                          {
                              x.NUMCAISSE,
                              x.MATRICULE,
                              x.DATE_DEBUT,
                              x.DATE_FIN,
                              x.PK_ID,
                              x.POSTE,
                              x.CENTRE,
                              x.ECART,
                              x.FONDCAISSE,
                              x.FK_IDCENTRE,
                              x.FK_IDCAISSE,
                              x.FK_IDCAISSIERE,
                              x.MONTANTENCAISSE
                          };
                         DataTable dt = Galatee.Tools.Utility.ListToDataTable(query);
                         laDerniereCaisse = Entities.GetEntityFromQuery<CsHabilitationCaisse>(dt);
                   }
               }
               return laDerniereCaisse;
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }
       public static DataTable  RetourneCaisseHabiliterCentre(CsCentre leCentre)
       {
           try
           {
               using (galadbEntities context = new galadbEntities())
               {
                   var habilCaisse = context.HABILITATIONCAISSE;
                   IEnumerable<object> query = (from x in habilCaisse 
                                                where x.FK_IDCENTRE == leCentre.PK_ID 
                                                select new {
                                                x.CENTRE ,
                                                x.DATE_DEBUT ,
                                                x.DATE_FIN ,
                                                x.FK_IDCENTRE ,
                                                x.FK_IDCAISSE ,
                                                x.MATRICULE ,
                                                x.NUMCAISSE ,
                                             NOMCAISSE=   x.ADMUTILISATEUR.LIBELLE, 
                                                x.POSTE ,
                                                x.FK_IDCAISSIERE ,
                                                x.PK_ID,
                                                x.FONDCAISSE 
                                                }).Distinct();
                         return Galatee.Tools.Utility.ListToDataTable(query);
               }

           }
           catch (Exception ex)
           {
               throw ex;
           }
       }

       public static bool AjustementDeFondCaisse(string NumCaisse, string NouveauFond, string MoisCompt, string matricule)
       {
           try
           {
               using (galadbEntities context = new galadbEntities())
               {
                   //CAISSE Caisse = context.CAISSE.FirstOrDefault(c => c.NUMCAISSE == NumCaisse);
                   HABILITATIONCAISSE Caisse = context.HABILITATIONCAISSE.FirstOrDefault(c => c.NUMCAISSE   == NumCaisse);
                   //COPER Coper = context.COPER.FirstOrDefault(c => c.CODE == "102");
                   // ADMUTILISATEUR user = context.ADMUTILISATEUR.FirstOrDefault(u => u.MATRICULE == matricule);

                   //TRANSCAISSE transcaisse = new TRANSCAISSE
                   //{
                   //    ACQUIT = "//////",
                   //    CAISSE = Caisse.NUMCAISSE,
                   //    CENTRE = Caisse.CENTRE,
                   //    CLIENT = "///////////",
                   //    COPER = "102",
                   //    DATECREATION = DateTime.Now,
                   //    DATEMODIFICATION = DateTime.Now,
                   //    DC =decimal.Parse(NouveauFond)>0? "D":"C",
                   //    DTRANS = DateTime.Now,
                   //    FK_IDCAISSIERE = Caisse.PK_ID,
                   //    FK_IDCENTRE = Caisse.FK_IDCENTRE,
                   //    FK_IDCOPER = Coper.PK_ID,
                   //    MATRICULE = user.MATRICULE,
                   //    FK_IDHABILITATIONCAISSE = user.PK_ID,
                   //    MOISCOMPT = MoisCompt,
                   //    MONTANT = decimal.Parse(NouveauFond),
                   //    //POSTE = user.FONCTION1.CODE ,
                   //    USERCREATION = user.LOGINNAME,
                   //    NATURE = "",
                   //    MODEREG = "",
                   //    TOP1 = "",
                   //    FK_IDLIBELLETOP = 3,
                   //    FK_IDMODEREG = 3,
                   //    FK_IDNATURE = 20
                   //};

                   Caisse.FONDCAISSE = decimal.Parse(NouveauFond);
                   //context.TRANSCAISSE.Add(transcaisse);
                   context.SaveChanges();

                   return true;
               }

           }
           catch (Exception ex)
           {
               throw ex;
           }
       }

       public static decimal ? RetourneEncaissementDate(CsHabilitationCaisse laCaisse)
       {
           try
           {
               using (galadbEntities context = new galadbEntities())
               {
                   decimal? MontantEncaisseTrcaisse = 0;
                   decimal? MontantDencaisseTrcaisse = 0;
                   decimal? MontantEncaisseTrcaisb = 0;
                   decimal? MontantDencaisseTrcaisb = 0;
                   DateTime? laDateCaisse = Convert.ToDateTime(laCaisse.DATE_DEBUT.Value.ToShortDateString());

                   List<TRANSCAISSE> transcaisse = context.TRANSCAISSE.Where(p => p.FK_IDHABILITATIONCAISSE == laCaisse.PK_ID && 
                       p.MODEREG == Enumere.ModePayementEspece &&  (p.ISDEMANDEANNULATION == false || p.ISDEMANDEANNULATION == null) && p.DC == Enumere.Debit  ).ToList();
                   MontantEncaisseTrcaisse = transcaisse.Where(p => p.TOPANNUL != "O").Sum(t => t.MONTANT);

                   List<TRANSCAISSE> transcaisseDecais = context.TRANSCAISSE.Where(p => p.FK_IDHABILITATIONCAISSE == laCaisse.PK_ID && p.MODEREG == Enumere.ModePayementEspece && (p.ISDEMANDEANNULATION == false || p.ISDEMANDEANNULATION == null) && p.DC == Enumere.Credit).ToList();
                   MontantDencaisseTrcaisse = transcaisseDecais.Where(p => p.TOPANNUL != "O").Sum(t => t.MONTANT);

                   List<TRANSCAISB> transcaisb = context.TRANSCAISB.Where(p => p.FK_IDHABILITATIONCAISSE == laCaisse.PK_ID && p.MODEREG == Enumere.ModePayementEspece  && p.DC == Enumere.Debit).ToList();
                   MontantEncaisseTrcaisb = transcaisb.Where(p => p.TOPANNUL != "O").Sum(t => t.MONTANT);

                   List<TRANSCAISB> transcaisbDec = context.TRANSCAISB.Where(p => p.FK_IDHABILITATIONCAISSE == laCaisse.PK_ID && p.MODEREG == Enumere.ModePayementEspece && p.DC ==Enumere.Credit  ).ToList();
                   MontantDencaisseTrcaisb = transcaisbDec.Where(p => p.TOPANNUL != "O").Sum(t => t.MONTANT);

                   MontantEncaisseTrcaisse = MontantEncaisseTrcaisse == null ? 0 : MontantEncaisseTrcaisse;
                   MontantEncaisseTrcaisb = MontantEncaisseTrcaisb == null ? 0 : MontantEncaisseTrcaisb;
                   MontantDencaisseTrcaisse = MontantDencaisseTrcaisse == null ? 0 : MontantDencaisseTrcaisse * (-1);
                   MontantDencaisseTrcaisb = MontantDencaisseTrcaisb == null ? 0 : MontantDencaisseTrcaisb * (-1);

                   return (MontantEncaisseTrcaisse + MontantEncaisseTrcaisb) - (MontantDencaisseTrcaisse + MontantDencaisseTrcaisb);
               }

           }
           catch (Exception ex)
           {
               throw ex;
           }
       }

       public static decimal? RetourneEncaissementEspeceDate(CsHabilitationCaisse laCaisse)
       {
           try
           {
               using (galadbEntities context = new galadbEntities())
               {
                   decimal? MontantEncaisseTrcaisse = 0;
                   decimal? MontantEncaisseTrcaisb = 0;
                   DateTime? laDateCaisse = Convert.ToDateTime(laCaisse.DATE_DEBUT.Value.ToShortDateString());

                   List<TRANSCAISSE> transcaisse = context.TRANSCAISSE.Where(p => p.FK_IDHABILITATIONCAISSE == laCaisse.PK_ID && (p.ISDEMANDEANNULATION == false || p.ISDEMANDEANNULATION == null) && p.MODEREG == Enumere.ModePayementEspece).ToList();
                   MontantEncaisseTrcaisse = transcaisse.Where(p => p.TOPANNUL != "O").Sum(t => t.MONTANT);

                   List<TRANSCAISB> transcaisb = context.TRANSCAISB.Where(p => p.FK_IDHABILITATIONCAISSE == laCaisse.PK_ID &&  p.MODEREG == Enumere.ModePayementEspece).ToList();
                   MontantEncaisseTrcaisb = transcaisb.Where(p => p.TOPANNUL != "O").Sum(t => t.MONTANT);

                   MontantEncaisseTrcaisse = MontantEncaisseTrcaisse == null ? 0 : MontantEncaisseTrcaisse;
                   MontantEncaisseTrcaisb = MontantEncaisseTrcaisb == null ? 0 : MontantEncaisseTrcaisb;

                   return MontantEncaisseTrcaisse + MontantEncaisseTrcaisb;
               }

           }
           catch (Exception ex)
           {
               throw ex;
           }
       }
       public static DataTable RetourneHabileCaisseNonReversement(CsHabilitationCaisse laCaisse)
       {
           try
           {
               using (galadbEntities context = new galadbEntities())
               {
                   var lstHabilCaisse = context.HABILITATIONCAISSE.Where(t => t.MATRICULE == laCaisse.MATRICULE &&
                                                                       t.NUMCAISSE == laCaisse.NUMCAISSE &&
                                                                       t.DATE_FIN != null 
                                                                       //&&
                                                                       //t.ECART != 0 
                                                                       );

                   var lstReversement = context.REVERSEMENTCAISSE;
                   var query1 = (from x in lstHabilCaisse
                                 join y in lstReversement on new { x.PK_ID }
                                 equals new { PK_ID = y.FK_IDHABILITATIONCAISSE }
                                 group y by new
                                    {
                                        y.FK_IDHABILITATIONCAISSE
                                    } into pTemp3
                                    let MONTANT = pTemp3.Sum(x => x.MONTANT)
                                    select new
                                    {
                                        pTemp3.Key.FK_IDHABILITATIONCAISSE ,
                                        MONTANT
                                    });
             
                   IEnumerable<object> query = (from x in lstHabilCaisse
                                                join y in query1 on new { x.PK_ID }
                                               equals new { PK_ID = y.FK_IDHABILITATIONCAISSE  } into _revervement
                                               from p in _revervement.DefaultIfEmpty()
                                               select new
                                               {
                                                   x.NUMCAISSE,
                                                   x.MATRICULE,
                                                   x.DATE_DEBUT,
                                                   x.DATE_FIN,
                                                   x.FK_IDCAISSE,
                                                   x.PK_ID,
                                                   x.POSTE,
                                                   x.CENTRE,
                                                   x.ECART,
                                                   x.FONDCAISSE,
                                                   x.FK_IDCENTRE,
                                                   x.MONTANTENCAISSE,
                                                   MONTANTREVERSER = p.MONTANT ,
                                                   NOMCAISSE = x.ADMUTILISATEUR.LIBELLE 
                                               });

                   return Galatee.Tools.Utility.ListToDataTable(query);
               }
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }

       public static DataTable   RetourneHabileCaisseReversement(CsHabilitationCaisse laCaisse)
       {
           try
           {
               using (galadbEntities context = new galadbEntities())
               {
                   var habilcaisseReversement = context.REVERSEMENTCAISSE ;
                   IEnumerable<object> query = from x in habilcaisseReversement
                                               where x.FK_IDHABILITATIONCAISSE   == laCaisse.PK_ID 
                                               select new
                                               {
                                                   x.FK_IDHABILITATIONCAISSE ,
                                                   x.DATE ,
                                                   x.MONTANT ,
                                                   x.RESTE  
                                               };
                   return Galatee.Tools.Utility.ListToDataTable(query);
               }

           }
           catch (Exception ex)
           {
               throw ex;
           }
       }

       public static DataTable RetourneListeCaisseHabilite(List<int> LsiteCentreCaisse)
        {
            try
            {
                using (galadbEntities ctontext = new galadbEntities())
                {
                    var habilcaisse = ctontext.HABILITATIONCAISSE.Where(t => LsiteCentreCaisse.Contains(t.FK_IDCENTRE ));
                    IEnumerable<object> query = (from t in habilcaisse 
                                                 select new
                                                 {
                                                     t.NUMCAISSE,
                                                     t.MATRICULE,
                                                     t.DATE_DEBUT,
                                                     t.DATE_FIN,
                                                     t.FK_IDCAISSE,
                                                     t.PK_ID,
                                                     t.POSTE,
                                                     t.CENTRE,
                                                     t.ECART,
                                                     t.FONDCAISSE,
                                                     t.FK_IDCENTRE,
                                                     t.MONTANTENCAISSE,
                                                     NOMCAISSE = t.ADMUTILISATEUR.LIBELLE 
                                                 }).Distinct();
                    return Galatee.Tools.Utility.ListToDataTable<object>(query);

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
       public static DataTable RetourneCaisseNonCloture(List<int> LsiteCentreCaisse)
        {
            try
            {
                using (galadbEntities ctontext = new galadbEntities())
                {
                    var habilcaisse = ctontext.HABILITATIONCAISSE.Where(t => LsiteCentreCaisse.Contains(t.FK_IDCENTRE ) && t.DATE_FIN  == null );
                    IEnumerable<object> query = (from t in habilcaisse 
                                                 select new
                                                 {
                                                     t.NUMCAISSE,
                                                     t.MATRICULE,
                                                     t.DATE_DEBUT,
                                                     t.DATE_FIN,
                                                     t.FK_IDCAISSE ,
                                                     t.PK_ID,
                                                     t.POSTE,
                                                     t.CENTRE,
                                                     t.ECART,
                                                     t.FONDCAISSE,
                                                     t.FK_IDCENTRE,
                                                     t.MONTANTENCAISSE,
                                                     //t.MONTANTREVERSE,
                                                     NOMCAISSE = ctontext.ADMUTILISATEUR.FirstOrDefault(p=>p.MATRICULE == t.MATRICULE).LIBELLE 
                                                 });
                    return Galatee.Tools.Utility.ListToDataTable<object>(query);

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
       public static DataTable RetourneCaisseEnCours(int IdNumCaisse, int IdCaissier, DateTime DateDebut)
       {
           try
           {
               using (galadbEntities ctontext = new galadbEntities())
               {
                   var habilcaisse = ctontext.HABILITATIONCAISSE.Where(t => t.FK_IDCAISSE == IdNumCaisse && t.FK_IDCAISSIERE == IdCaissier && t.DATE_FIN == null );
                   IEnumerable<object> query = (from t in habilcaisse
                                                select new
                                                {
                                                    t.NUMCAISSE,
                                                    t.MATRICULE,
                                                    t.DATE_DEBUT,
                                                    t.DATE_FIN,
                                                    t.FK_IDCAISSE,
                                                    t.FK_IDCAISSIERE ,
                                                    t.PK_ID,
                                                    t.POSTE,
                                                    t.CENTRE,
                                                    t.ECART,
                                                    t.FONDCAISSE,
                                                    t.FK_IDCENTRE,
                                                    t.MONTANTENCAISSE,
                                                    NOMCAISSE =t.ADMUTILISATEUR.LIBELLE 
                                                });
                   return Galatee.Tools.Utility.ListToDataTable<object>(query);

               }
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }
       public static DataTable RetourneCaisseCloture(List<int> LsiteCentreCaisse)
       {
           try
           {
               using (galadbEntities ctontext = new galadbEntities())
               {
                   var habilcaisse = ctontext.HABILITATIONCAISSE.Where(t => LsiteCentreCaisse.Contains(t.FK_IDCENTRE) && t.DATE_FIN != null);
                   IEnumerable<object> query = (from t in habilcaisse
                                                select new
                                                {
                                                    t.NUMCAISSE,
                                                    t.MATRICULE,
                                                    t.DATE_DEBUT,
                                                    t.DATE_FIN,
                                                    t.FK_IDCAISSE,
                                                    t.PK_ID,
                                                    t.POSTE,
                                                    t.CENTRE,
                                                    t.ECART,
                                                    t.FONDCAISSE,
                                                    t.FK_IDCENTRE,
                                                    t.MONTANTENCAISSE,
                                                    NOMCAISSE =t.ADMUTILISATEUR.LIBELLE 
                                                });
                   return Galatee.Tools.Utility.ListToDataTable<object>(query);

               }
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }
       public static DataTable RetourneEncaissementPourValidationAnnulation(List<int> LsiteCentreCaisse)
       {
           try
           {
               using (galadbEntities ctontext = new galadbEntities())
               {
                   var transc = ctontext.TRANSCAISSE.Where(t => LsiteCentreCaisse.Contains(t.HABILITATIONCAISSE.FK_IDCENTRE ));
                   var demAnnul = ctontext.DEMANDEANNULATION.Where(t => LsiteCentreCaisse.Contains(t.HABILITATIONCAISSE.FK_IDCENTRE)).OrderBy(t => t.ACQUIT);
                   IEnumerable<object> query = (from t in transc
                                                join n in demAnnul on new { t.ACQUIT,t.FK_IDHABILITATIONCAISSE}
                                                equals new { ACQUIT = n.ACQUIT,FK_IDHABILITATIONCAISSE = n.FK_IDHABILITATIONCAISSE  } 
                                                where n.STATUS  == Enumere.StatusDemandeInitier 
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
                                                    t.ISDEMANDEANNULATION,
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
                                                    MONTANTPAYE = t.MONTANT,
                                                    LIBELLEAGENCE = t.HABILITATIONCAISSE.CENTRE1.LIBELLE,
                                                    LIBELLESITE = t.HABILITATIONCAISSE.CENTRE1.SITE.LIBELLE,
                                                    LIBELLECOPER = string.IsNullOrEmpty(t.NUMDEM) ? t.LCLIENT.COPER1.LIBELLE :
                                                                   ctontext.RUBRIQUEDEMANDE.FirstOrDefault(c => t.NUMDEM == c.NUMDEM && t.REFEM == c.REFEM && t.NDOC == c.NDOC).COPER1.LIBELLE,
                                                    NOM = string.IsNullOrEmpty(t.NUMDEM) ? ctontext.CLIENT.FirstOrDefault(c => c.REFCLIENT == t.CLIENT && c.CENTRE == t.CENTRE && c.ORDRE == t.ORDRE).NOMABON :
                                                          ctontext.DCLIENT.FirstOrDefault(c => t.NUMDEM == c.NUMDEM).NOMABON, 

                                                    FK_IDCLIENT = t.LCLIENT.FK_IDCLIENT ,
                                                    IsDEMANDEANNULATION = t.ISDEMANDEANNULATION,
                                                    REFFERENCECLIENT = t.CENTRE + "." + t.CLIENT + "." + t.ORDRE ,
                                                    n.MOTIFDEMANDE,
                                                    n.MOTIFREJET,
                                                    n.STATUS
                                                });

                   return Galatee.Tools.Utility.ListToDataTable<object>(query);

               }
           }
           catch (Exception ex)
           {
               throw ex;
           }

       }

       public static DataTable ListeDesReversementCaisse(List<CsHabilitationCaisse> LstHabilCaisse)
       {
           try
           {
               using (galadbEntities context=new galadbEntities())
               {
                   List<int> lstIdCentre =new List<int>();
                   foreach (CsHabilitationCaisse  item in LstHabilCaisse)
		                    lstIdCentre.Add(item.PK_ID);

                   List<CsHabilitationCaisse> habilitationtoreturn = new List<CsHabilitationCaisse>();
                   var habilitation = context.HABILITATIONCAISSE.Where(t => lstIdCentre.Contains(t.PK_ID));
                   var lstReversement = context.REVERSEMENTCAISSE;
                   var query = (from h in habilitation
                                join y in lstReversement on new { h.PK_ID  }
                                                 equals new { PK_ID = y.FK_IDHABILITATIONCAISSE } into _revervement
                                from p in _revervement.DefaultIfEmpty()
                                select new
                                {
                                    CODECENTRE = h.CENTRE,
                                    DATE_DEBUT = h.DATE_DEBUT,
                                    DATE_FIN =p.DATE   ,
                                    ECART = h.ECART,
                                    FK_IDCENTRE = h.FK_IDCENTRE,
                                    FK_NUMCAISSE = h.FK_IDCAISSE,
                                    FONDCAIS = h.FONDCAISSE,
                                    MATRICULE = h.MATRICULE,
                                    MONTANTENCAISSE = h.MONTANTENCAISSE ,
                                    MONTANTREVERSER = p.MONTANT,
                                    NOMCAISSE = h.ADMUTILISATEUR.LIBELLE ,
                                    NUMCAISSE = h.NUMCAISSE,
                                    POSTE = h.POSTE,
                                    AGENCECAISSE = h.CENTRE1.LIBELLE,
                                    SITECAISSE  = h.CENTRE1.SITE.LIBELLE ,
                                    h.PK_ID 
                                    
                                }).ToList();
                   return Galatee.Tools.Utility.ListToDataTable<object>(query);
                 
               }
           }
           catch (Exception ex )
           {
               throw ex;
           }
       }

       public static DataTable ListeDesEtatCaisse(int fk_idcaisse, string centre, string matrice, DateTime datedebut, DateTime datefin)
       {
           try
           {
               using (galadbEntities context = new galadbEntities())
               {
                   List<CsHabilitationCaisse> habilitationtoreturn = new List<CsHabilitationCaisse>();
                   List<HABILITATIONCAISSE> habilitation = context.HABILITATIONCAISSE.Where(t =>
                                                                   (t.FK_IDCENTRE == fk_idcaisse) &&
                                                                   (t.CENTRE == centre || string.IsNullOrEmpty(centre)) &&
                                                                   (t.MATRICULE == matrice || string.IsNullOrEmpty(matrice)) &&
                                                                   (t.DATE_DEBUT >= datedebut)).ToList();
                   if (datedebut.ToString() != new DateTime().ToString())
                       habilitation = habilitation.Where(h => h.DATE_DEBUT >= datedebut).ToList();

                   if ((datefin.ToString() != new DateTime().ToString()))
                       habilitation = habilitation.Where(h => h.DATE_DEBUT <= datefin || h.DATE_DEBUT == null).ToList();

                   var query = (from h in habilitation
                                select new
                                {
                                    CODECENTRE = h.CENTRE,
                                    DATE_DEBUT = h.DATE_DEBUT,
                                    DATE_FIN = h.DATE_FIN,
                                    ECART = h.ECART,
                                    FK_IDCENTRE = h.FK_IDCENTRE,
                                    FK_NUMCAISSE = h.FK_IDCAISSE,
                                    FONDCAIS = h.FONDCAISSE,
                                    MATRICULE = h.MATRICULE,
                                    MONTANTENCAISSE = h.MONTANTENCAISSE,
                                    NOMCAISSE = h.ADMUTILISATEUR.LIBELLE,
                                    NUMCAISSE = h.NUMCAISSE,
                                    POSTE = h.POSTE,
                                    AGENCECAISSE = h.CENTRE1.LIBELLE,
                                    SITECAISSE = h.CENTRE1.SITE.LIBELLE,
                                    h.DATECREATION 
                                }).ToList();
                   return Galatee.Tools.Utility.ListToDataTable<object>(query);

               }
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }
       public static DataTable  LitseDesTransaction(CsHabilitationCaisse laCaisse)
       {
           try
           {
                   using (galadbEntities ctontext = new galadbEntities())
                   {
                       var transc = ctontext.TRANSCAISSE.Where(t => t.FK_IDHABILITATIONCAISSE == laCaisse.PK_ID );
                       IEnumerable<object> query = (from t in transc
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
                                                        t.ISDEMANDEANNULATION,
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
                                                        MONTANTPAYE = t.MONTANT,
                                                        NOMCAISSIERE = t.ADMUTILISATEUR1.LIBELLE ,
                                                        LIBELLEMODREG = t.MODEREG1.LIBELLE,
                                                        LIBELLEBANQUE =(t.MODEREG == Enumere.ModePayementCheque)?ctontext.BANQUE.FirstOrDefault(c => c.CODE  == t.BANQUE).LIBELLE : string.Empty ,
                                                        LIBELLEAGENCE = t.HABILITATIONCAISSE.CENTRE1.LIBELLE,
                                                        LIBELLESITE = t.HABILITATIONCAISSE.CENTRE1.SITE.LIBELLE,
                                                        LIBELLECOPER = t.COPER1.LIBELLE,
                                                        //NOM = ctontext.CLIENT.FirstOrDefault(c => c.REFCLIENT == t.CLIENT && c.CENTRE == t.CENTRE && c.ORDRE == t.ORDRE).NOMABON,
                                                        NOM = string.IsNullOrEmpty(t.NUMDEM) ? ctontext.CLIENT.FirstOrDefault(c => c.REFCLIENT == t.CLIENT && c.CENTRE == t.CENTRE && c.ORDRE == t.ORDRE).NOMABON :
                                                              ctontext.DCLIENT.FirstOrDefault(c => t.NUMDEM == c.NUMDEM).NOMABON,
                                                        IsDEMANDEANNULATION = t.ISDEMANDEANNULATION,
                                                    });
                       return Galatee.Tools.Utility.ListToDataTable<object>(query);
               }
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }

       public static  DataTable HistoriqueListeEncaissements(List<CsHabilitationCaisse>  laCaisse)
       {
           try
           {
               using (galadbEntities ctontext = new galadbEntities())
               {
                   List<int> lstIdCaisse = new List<int>();
                   foreach (CsHabilitationCaisse item in laCaisse)
                       lstIdCaisse.Add(item.PK_ID );

                   var transc = ctontext.TRANSCAISB .Where(t =>lstIdCaisse.Contains(t.FK_IDHABILITATIONCAISSE.Value  ));
                   IEnumerable<object> query = (from t in transc
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
                                                    MONTANTPAYE = t.MONTANT,
                                                    NOMCAISSIERE = t.ADMUTILISATEUR1.LIBELLE,
                                                    LIBELLEMODREG = t.MODEREG1.LIBELLE,
                                                    LIBELLEBANQUE = (t.MODEREG == Enumere.ModePayementCheque) ? ctontext.BANQUE.FirstOrDefault(c => c.CODE == t.BANQUE).LIBELLE : string.Empty,
                                                    LIBELLEAGENCE = t.HABILITATIONCAISSE.CENTRE1.LIBELLE,
                                                    LIBELLESITE = t.HABILITATIONCAISSE.CENTRE1.SITE.LIBELLE,
                                                    LIBELLECOPER = t.COPER1.LIBELLE,
                                                    NOM = string.IsNullOrEmpty(t.NUMDEM) ? ctontext.CLIENT.FirstOrDefault(c => c.REFCLIENT == t.CLIENT && c.CENTRE == t.CENTRE && c.ORDRE == t.ORDRE).NOMABON :
                                                      ctontext.DCLIENT.FirstOrDefault(c => t.NUMDEM == c.NUMDEM).NOMABON, 
                                                    //NOM = ctontext.CLIENT.FirstOrDefault(c => c.REFCLIENT == t.CLIENT && c.CENTRE == t.CENTRE && c.ORDRE == t.ORDRE).NOMABON,
                                                });
                   return Galatee.Tools.Utility.ListToDataTable<object>(query);
               }
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }

       public static List<LCLIENT> ChargerListeFacturePayeur(int? leIdPayeur)
       {
           try
           {
               using (galadbEntities context = new galadbEntities())
               {
                  return  context.LCLIENT.Where(t => t.CLIENT1.FK_IDPAYEUR == leIdPayeur).ToList(); ;
               }
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }

       public static DataTable  ChargerListeFacturePeriode(string periodeDebut,string periodeFin,string FactureDeb,string FactureFin)
       {
           try
           {
               using (galadbEntities ctontext = new galadbEntities())
               {
                   var lesFacture = ctontext.LCLIENT;
                   IEnumerable<object> query = (from t in lesFacture
                                                //where (t.REFEM >= periodeDebut && (t.REFEM <= periodeFin || string.IsNullOrEmpty( periodeFin)))  
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
                                                   

                                                });

                   return Galatee.Tools.Utility.ListToDataTable<object>(query);

               }
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }
       public static DataTable ListeDesCaisse(string centre, string matrice, DateTime datedebut, DateTime datefin)
       {
           try
           {
               using (galadbEntities context = new galadbEntities())
               {
                   List<CsHabilitationCaisse> habilitationtoreturn = new List<CsHabilitationCaisse>();
                   List<HABILITATIONCAISSE> habilitation = context.HABILITATIONCAISSE.Where(t =>
                                                                   (t.CENTRE == centre || string.IsNullOrEmpty(centre)) &&
                                                                   (t.MATRICULE == matrice || string.IsNullOrEmpty(matrice)) &&
                                                                   (t.DATE_DEBUT >= datedebut)).ToList();

                 
                   var query = (from h in habilitation
                                select new
                                {
                                    CODECENTRE = h.CENTRE,
                                    DATE_DEBUT = h.DATE_DEBUT,
                                    DATE_FIN = h.DATE_FIN,
                                    ECART = h.ECART,
                                    FK_IDCENTRE = h.FK_IDCENTRE,
                                    FK_NUMCAISSE = h.FK_IDCAISSE,
                                    FONDCAIS = h.FONDCAISSE,
                                    MATRICULE = h.MATRICULE,
                                    MONTANTENCAISSE = h.MONTANTENCAISSE,
                                    //MONTANTREVERSE = h.MONTANTREVERSE,
                                    NOMCAISSE = context.ADMUTILISATEUR.FirstOrDefault(t => t.MATRICULE == h.MATRICULE).LIBELLE,
                                    NUMCAISSE = h.NUMCAISSE,
                                    POSTE = h.POSTE,
                                    AGENCECAISSE = h.CENTRE1.LIBELLE,
                                    SITECAISSE = h.CENTRE1.SITE.LIBELLE
                                }).ToList();
                   return Galatee.Tools.Utility.ListToDataTable<object>(query);

               }
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }




       #region Sylla

       public static DataTable LigneFactureClient(string centre, string client, string ordre, string REFEM)
       {
           try
           {

               using (galadbEntities ctontext = new galadbEntities())
               {
                   var LAfACTURE = ctontext.LCLIENT;
                   IEnumerable<object> query = (from t in LAfACTURE
                                                where t.CENTRE == centre && t.CLIENT == client && t.ORDRE == ordre && t.REFEM == REFEM
                                                select new
                                                {
                                                    t.PK_ID,
                                                    t.CENTRE,
                                                    t.CLIENT,
                                                    t.ORDRE,
                                                    t.REFEM,
                                                    t.NDOC,
                                                    t.COPER,
                                                    t.DENR,
                                                    t.EXIG,
                                                    t.MONTANT,
                                                    t.CAPUR,
                                                    t.CRET,
                                                    t.MODEREG,
                                                    t.DC,
                                                    t.ORIGINE,
                                                    t.CAISSE,
                                                    t.ECART,
                                                    t.MOISCOMPT,
                                                    t.TOP1,
                                                    t.EXIGIBILITE,
                                                    t.FRAISDERETARD,
                                                    t.REFERENCEPUPITRE,
                                                    t.IDLOT,
                                                    t.DATEVALEUR,
                                                    t.REFERENCE,
                                                    t.REFEMNDOC,
                                                    t.ACQUIT,
                                                    t.MATRICULE,
                                                    t.TAXESADEDUIRE,
                                                    t.DATEFLAG,
                                                    t.MONTANTTVA,
                                                    t.IDCOUPURE,
                                                    t.AGENT_COUPURE,
                                                    t.RDV_COUPURE,
                                                    t.NUMCHEQ,
                                                    t.OBSERVATION_COUPURE,
                                                    t.USERCREATION,
                                                    t.DATECREATION,
                                                    t.DATEMODIFICATION,
                                                    t.USERMODIFICATION,
                                                    t.BANQUE,
                                                    t.GUICHET,
                                                    t.FK_IDCENTRE,
                                                    t.FK_IDADMUTILISATEUR,
                                                    t.FK_IDCOPER,
                                                    t.FK_IDLIBELLETOP,
                                                    t.FK_IDCLIENT,
                                                    t.FK_IDPOSTE,
                                                    t.POSTE,
                                                    t.DATETRANS,
                                                    //t.IDMORATOIRE,
                                                    t.FK_IDMORATOIRE,
                                                    LIBELLECOPER = t.COPER1.LIBELLE,
                                                    NOM = t.CLIENT1.NOMABON
                                                });

                   return Galatee.Tools.Utility.ListToDataTable<object>(query);

               }
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }









       public static DataTable RetourneListeDesClientsParCodeRegroupemnt(int idCodeRegroupement, string REFEM)
       {
           try
           {
               //cmd.CommandText = "SPX_ENC_LISTECLIENTREGROUPE";
               using (galadbEntities ctontext = new galadbEntities())
               {
                   IEnumerable<CLIENT> clientGrouper = ctontext.CLIENT.Where(cl => cl.FK_IDREGROUPEMENT == idCodeRegroupement);
                   List<CLIENT> clientSoldeSupZero = new List<CLIENT>();
                   foreach (CLIENT cl in clientGrouper)
                   {
                       decimal? SoldeClient = FonctionCaisse.RetourneSoldeClient(cl.FK_IDCENTRE, cl.CENTRE, cl.REFCLIENT, cl.ORDRE, REFEM);
                       if (SoldeClient > 0)
                           clientSoldeSupZero.Add(cl);
                   }


                   IEnumerable<object> query = from _clientGrouper in clientSoldeSupZero
                                               select new
                                               {
                                                   _clientGrouper.CENTRE,
                                                   _clientGrouper.REFCLIENT,
                                                   _clientGrouper.ORDRE,
                                                   _clientGrouper.PK_ID
                                               };
                   return Galatee.Tools.Utility.ListToDataTable<object>(query);
               }

           }
           catch (Exception ex)
           {
               throw ex;
           }
       }
       public static DataTable RetouneLaCaisseCourante(string Matricule)
       {
           try
           {
               using (galadbEntities ctontext = new galadbEntities())
               {
                   var laCaisse = ctontext.HABILITATIONCAISSE;
                   IEnumerable<object> query = (from x in laCaisse
                                                where x.DATE_FIN == null && x.MATRICULE == Matricule 
                                                select new
                                                {
                                                    x.NUMCAISSE,
                                                    x.MATRICULE,
                                                    x.DATE_DEBUT,
                                                    x.DATE_FIN,
                                                    x.PK_ID,
                                                    x.POSTE,
                                                    x.CENTRE,
                                                    x.ECART,
                                                    x.FONDCAISSE,
                                                    x.FK_IDCENTRE,
                                                    x.FK_IDCAISSE,
                                                    x.FK_IDCAISSIERE,
                                                    x.MONTANTENCAISSE,
                                                    x.DATECREATION
                                                });
                   return Galatee.Tools.Utility.ListToDataTable<object>(query);
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
