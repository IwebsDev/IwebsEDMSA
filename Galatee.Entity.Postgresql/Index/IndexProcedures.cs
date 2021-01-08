using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Transactions;
using Galatee.Structure;

namespace Galatee.Entity.Model
{
    public class IndexProcedures
    {

        private static bool IsLotIsole(string Numlot)
        {
            if (Numlot == null)
                return false;
            string FactureIsoleIndex = "00002";
            string FactureResiliationIndex = "00001";
            string FactureAnnulatinIndex = "00003";
            if (new string[] { FactureIsoleIndex, FactureAnnulatinIndex, FactureResiliationIndex }.Contains(Numlot.Substring(Enumere.TailleCentre, (Enumere.TailleNumeroBatch - Enumere.TailleCentre))))
                return true;
            else
                return false;
        }
        public static DataTable ListeDesLotCandidatLori(int fk_idcentre,int? fk_idProduit, int? fkidcategorie, int? fkidperiodicite,int? fkidtournee,string PeriodeEnCours)
        {
            try
            {
                string FactureIsoleIndex = "00002";
                string FactureResiliationIndex = "00001";
                string FactureAnnulatinIndex = "00003";

                using (galadbEntities context = new galadbEntities())
                {
                    List<int?> StatusIndex = new List<int?>() { 3, 4, 99 };

                    var Ag = context.AG ;
                    var query = (from _ag in Ag
                                 from _client in _ag.CLIENT1
                                 from _brt in _ag.BRT 
                                 from _abon in _client.ABON
                                 from cpt in _abon.CANALISATION 
                                 from tourrel in _ag.TOURNEE1.TOURNEERELEVEUR
                                 where
                                 _client.FK_IDCATEGORIE == fkidcategorie &&
                                 _abon.FK_IDPERIODICITEFACTURE == fkidperiodicite &&
                                 _ag.FK_IDTOURNEE == fkidtournee &&
                                 _abon.FK_IDPRODUIT == fk_idProduit &&
                                 _ag.FK_IDCENTRE == fk_idcentre &&
                                 _brt.DRES == null 
                                 && !context.EVENEMENT.Any(t => t.FK_IDABON == _abon.PK_ID && t.PERIODE == PeriodeEnCours &&
                                                            ((t.STATUS != Enumere.EvenementSupprimer) || (t.STATUS != Enumere.EvenementAnnule)) &&
                                                             !new string[] { FactureIsoleIndex, FactureAnnulatinIndex, FactureResiliationIndex }.Contains(t.LOTRI.Substring(Enumere.TailleCentre, (Enumere.TailleNumeroBatch - Enumere.TailleCentre))))
                                 select new
                                 {
                                     _abon.CENTRE,
                                     _abon.CLIENT,
                                     _abon.ORDRE,
                                     _ag.TOURNEE,
                                     _ag.ORDTOUR,
                                     _abon.PRODUIT,
                                     _abon.PUISSANCE,
                                     _abon.DRES,
                                     _abon.PERFAC,
                                     idabon = _abon.PK_ID,
                                     cpt.COMPTEUR.NUMERO,
                                     DIAMETRE = cpt.REGLAGECOMPTEUR  ,
                                     //cpt.COMPTEUR.ETAT,
                                     cpt.COMPTEUR.COEFLECT,
                                     //_brt.TYPECOMPTAGE,
                                     cpt.COMPTEUR.COEFCOMPTAGE,
                                     cpt.COMPTEUR.TYPECOMPTEUR,
                                     PROPRIETAIRE = cpt.PROPRIETAIRE.CODE,
                                     cpt.POINT,
                                     _client.CATEGORIE,
                                     _abon.TYPETARIF,
                                     _abon.FORFAIT,
                                     _client.CODECONSO,
                                     _brt.PUISSANCEINSTALLEE,
                                     
                                     FK_IDCLIENT = _client.PK_ID  ,
                                     FK_IDCANALISATION = cpt.PK_ID ,
                                     FK_IDABON = _abon.PK_ID,
                                     FK_IDCENTRE = _abon.FK_IDCENTRE,
                                     FK_IDPRODUIT = _abon.FK_IDPRODUIT,
                                     FK_IDTOURNEE = _ag.FK_IDTOURNEE,
                                     FK_IDCOMPTEUR = cpt.COMPTEUR.PK_ID,
                                     FK_IDCATEGORIE = _client.FK_IDCATEGORIE,
                                     FK_IDRELEVEUR =tourrel.FK_IDRELEVEUR 
                                 });


                    var distinctClient = (from d in query select new {d.FK_IDCENTRE , d.CENTRE, d.CLIENT, d.ORDRE, d.POINT, d.PRODUIT }).Distinct();

                    var MaxEvenemt = from d in distinctClient
                                            join ev in context.EVENEMENT on new {d.FK_IDCENTRE , d.CENTRE, d.CLIENT, d.ORDRE, d.POINT, d.PRODUIT }
                                                                         equals new {ev.FK_IDCENTRE , ev.CENTRE, ev.CLIENT, ev.ORDRE, ev.POINT, ev.PRODUIT }
                                            group new { d, ev } by new {d.FK_IDCENTRE , d.CENTRE, d.CLIENT, d.ORDRE, d.POINT, d.PRODUIT } into pJoin
                                            select new
                                            {
                                                pJoin.Key.CENTRE,
                                                pJoin.Key.CLIENT,
                                                pJoin.Key.ORDRE,
                                                pJoin.Key.PRODUIT,
                                                pJoin.Key.POINT,
                                                pJoin.Key.FK_IDCENTRE ,
                                                MaxEvenement = pJoin.Max(o => o.ev.NUMEVENEMENT)
                                            };

                    var historiqueFacture = (from d in distinctClient
                                            join ev in context.HISTORIQUE on new {d.FK_IDCENTRE , d.CENTRE, d.CLIENT, d.ORDRE, d.POINT, d.PRODUIT }
                                                                         equals new { ev.FK_IDCENTRE, ev.CENTRE, ev.CLIENT, ev.ORDRE, ev.POINT, ev.PRODUIT }
                                            group new { d, ev } by new {d.FK_IDCENTRE , d.CENTRE, d.CLIENT, d.ORDRE, d.POINT, d.PRODUIT } into pJoin
                                            select new
                                            {
                                                pJoin.Key.FK_IDCENTRE ,
                                                pJoin.Key.CENTRE,
                                                pJoin.Key.CLIENT,
                                                pJoin.Key.ORDRE,
                                                pJoin.Key.PRODUIT,
                                                pJoin.Key.POINT,
                                                CumConso = (pJoin.Sum(o => o.ev.CONSOFAC)/pJoin.Sum(o => o.ev.NBREJOURFACTURE)) * Enumere.NombreDejour
                                            }).Take(12);

                    var MaxEvenemtFacture = from d in distinctClient
                                            join ev in context.EVENEMENT on new {d.FK_IDCENTRE , d.CENTRE, d.CLIENT, d.ORDRE, d.POINT, d.PRODUIT }
                                                                         equals new {ev.FK_IDCENTRE , ev.CENTRE, ev.CLIENT, ev.ORDRE, ev.POINT, ev.PRODUIT }
                                            where StatusIndex.Contains(ev.STATUS) 
                                            group new { d, ev } by new {d.FK_IDCENTRE , d.CENTRE, d.CLIENT, d.ORDRE, d.POINT, d.PRODUIT } into pJoin
                                            select new
                                            {
                                                pJoin.Key.FK_IDCENTRE ,
                                                pJoin.Key.CENTRE,
                                                pJoin.Key.CLIENT,
                                                pJoin.Key.ORDRE,
                                                pJoin.Key.PRODUIT,
                                                pJoin.Key.POINT,
                                                MaxEvenement = pJoin.Max(o => o.ev.NUMEVENEMENT)
                                            };

                    var AnclientMax = from a in MaxEvenemt
                                   join ev in context.EVENEMENT on new {a.FK_IDCENTRE , a.CENTRE, a.CLIENT, a.ORDRE, a.POINT, a.PRODUIT }
                                                                  equals new {ev.FK_IDCENTRE , ev.CENTRE, ev.CLIENT, ev.ORDRE, ev.POINT, ev.PRODUIT }
                                   where ev.NUMEVENEMENT == a.MaxEvenement
                                   select new
                                   {
                                       a.FK_IDCENTRE ,
                                       a.CENTRE,
                                       a.CLIENT,
                                       a.ORDRE,
                                       a.PRODUIT,
                                       a.POINT,
                                       a.MaxEvenement
                                   };
                    var Anclient = from a in MaxEvenemtFacture
                                   join ev in context.EVENEMENT on new {a.FK_IDCENTRE , a.CENTRE, a.CLIENT, a.ORDRE, a.POINT, a.PRODUIT }
                                                                  equals new {ev.FK_IDCENTRE , ev.CENTRE, ev.CLIENT, ev.ORDRE, ev.POINT, ev.PRODUIT }
                                   where ev.NUMEVENEMENT == a.MaxEvenement
                                   select new
                                   {
                                       a.FK_IDCENTRE ,
                                       a.CENTRE,
                                       a.CLIENT,
                                       a.ORDRE,
                                       a.PRODUIT,
                                       a.POINT,
                                       a.MaxEvenement,
                                       DANCIENINDEX = ev.INDEXEVT,
                                       ANCIENCAS = ev.CAS,
                                       ev.DATEEVT,
                                       ev.QTEAREG
                                   };


                    var query2 = from q1 in query
                                 join an in Anclient on new {q1.FK_IDCENTRE , q1.CENTRE, q1.CLIENT, q1.ORDRE, q1.POINT, q1.PRODUIT }
                                                     equals new {an.FK_IDCENTRE , an.CENTRE, an.CLIENT, an.ORDRE, an.POINT, an.PRODUIT }
                                 select new
                                 {
                                     q1.CENTRE,
                                     q1.CLIENT,
                                     q1.ORDRE,
                                     q1.TOURNEE,
                                     q1.ORDTOUR,
                                     q1.PRODUIT,
                                     q1.PUISSANCE,
                                     q1.DRES,
                                     q1.PERFAC,
                                     COMPTEUR=   q1.NUMERO,
                                     q1.DIAMETRE,
                                     q1.COEFLECT,
                                     q1.COEFCOMPTAGE,
                                     q1.TYPECOMPTEUR,
                                     q1.PROPRIETAIRE,
                                     q1.POINT,
                                     q1.CATEGORIE,
                                     q1.TYPETARIF,
                                     q1.FORFAIT,
                                     q1.CODECONSO,
                                     q1.PUISSANCEINSTALLEE,

                                     q1.FK_IDCLIENT  ,
                                     q1.FK_IDCANALISATION  ,
                                     q1.FK_IDABON  ,
                                     q1.FK_IDCENTRE ,
                                     q1.FK_IDPRODUIT ,
                                     q1.FK_IDTOURNEE ,
                                     q1.FK_IDCOMPTEUR ,
                                     q1.FK_IDCATEGORIE ,
                                     q1.FK_IDRELEVEUR ,
                                     //NUMEVENEMENT = anMax.MaxEvenement,
                                     //an.QTEAREG,
                                     //an.DANCIENINDEX,
                                     //an.ANCIENCAS,
                                     //an.DATEEVT
                                     //,
                                     //CONSOMOYENNE = hs.CumConso
                                 };
                    return Galatee.Tools.Utility.ListToDataTable<object>(query2);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable ListeDesLotCandidatLoriAvecEdition(int fk_idcentre, int? fk_idProduit, int? fkidcategorie, int? fkidperiodicite, int? fkidtournee, string PeriodeEnCours)
        {
            try
            {
                string FactureIsoleIndex = "00002";
                string FactureResiliationIndex = "00001";
                string FactureAnnulatinIndex = "00003";

                using (galadbEntities context = new galadbEntities())
                {

                    List<int?> StatusIndex = new List<int?>() { 3, 4, 99 };

                    var Ag = context.AG;
                    var query = (from _ag in Ag
                                 from _client in _ag.CLIENT1
                                 from _brt in _ag.BRT
                                 from _abon in _client.ABON
                                 from cpt in _abon.CANALISATION
                                 from tourrel in _ag.TOURNEE1.TOURNEERELEVEUR
                                 where
                                 _client.FK_IDCATEGORIE == fkidcategorie &&
                                 _abon.FK_IDPERIODICITEFACTURE == fkidperiodicite &&
                                 _ag.FK_IDTOURNEE == fkidtournee &&
                                 _abon.FK_IDPRODUIT == fk_idProduit &&
                                 _ag.FK_IDCENTRE == fk_idcentre &&
                                 _brt.DRES == null &&
                                 _abon.DRES == null 
                                 && !context.EVENEMENT.Any(t => t.FK_IDABON == _abon.PK_ID && t.PERIODE == PeriodeEnCours &&
                                                            ((t.STATUS != Enumere.EvenementSupprimer) || (t.STATUS != Enumere.EvenementAnnule)) &&
                                                             !new string[] { FactureIsoleIndex, FactureAnnulatinIndex, FactureResiliationIndex }.Contains(t.LOTRI.Substring(Enumere.TailleCentre, (Enumere.TailleNumeroBatch - Enumere.TailleCentre))))
                                 select new
                                 {
                                     _abon.CENTRE,
                                     _abon.CLIENT,
                                     _abon.ORDRE,
                                     _ag.TOURNEE,
                                     _ag.ORDTOUR,
                                     _abon.PRODUIT,
                                     _abon.PUISSANCE,
                                     _abon.DRES,
                                     _abon.PERFAC,
                                     idabon = _abon.PK_ID,
                                     cpt.COMPTEUR.NUMERO,
                                      cpt.REGLAGECOMPTEUR ,
                                     cpt.COMPTEUR.COEFLECT,
                                     cpt.COMPTEUR.COEFCOMPTAGE,
                                     cpt.COMPTEUR.TYPECOMPTEUR,
                                     PROPRIETAIRE = cpt.PROPRIETAIRE.CODE,
                                     cpt.POINT,
                                     _client.CATEGORIE,
                                     _abon.TYPETARIF,
                                     _abon.FORFAIT,
                                     _client.CODECONSO,
                                     _brt.PUISSANCEINSTALLEE,

                                     FK_IDCLIENT = _client.PK_ID,
                                     FK_IDCANALISATION = cpt.PK_ID,
                                     FK_IDABON = _abon.PK_ID,
                                     FK_IDCENTRE = _abon.FK_IDCENTRE,
                                     FK_IDPRODUIT = _abon.FK_IDPRODUIT,
                                     FK_IDTOURNEE = _ag.FK_IDTOURNEE,
                                     FK_IDCOMPTEUR = cpt.COMPTEUR.PK_ID,
                                     FK_IDCATEGORIE = _client.FK_IDCATEGORIE,
                                     FK_IDRELEVEUR = tourrel.FK_IDRELEVEUR
                                 });


                    var distinctClient = (from d in query select new { d.CENTRE, d.CLIENT, d.ORDRE, d.POINT, d.PRODUIT }).Distinct();

                    var MaxEvenemt = from d in distinctClient
                                     join ev in context.EVENEMENT on new { d.CENTRE, d.CLIENT, d.ORDRE, d.POINT, d.PRODUIT }
                                                                  equals new { ev.CENTRE, ev.CLIENT, ev.ORDRE, ev.POINT, ev.PRODUIT }
                                     group new { d, ev } by new { d.CENTRE, d.CLIENT, d.ORDRE, d.POINT, d.PRODUIT } into pJoin
                                     select new
                                     {
                                         pJoin.Key.CENTRE,
                                         pJoin.Key.CLIENT,
                                         pJoin.Key.ORDRE,
                                         pJoin.Key.PRODUIT,
                                         pJoin.Key.POINT,
                                         MaxEvenement = pJoin.Max(o => o.ev.NUMEVENEMENT)
                                     };

                    var MaxEvenemtFacture = from d in distinctClient
                                            join ev in context.EVENEMENT on new { d.CENTRE, d.CLIENT, d.ORDRE, d.POINT, d.PRODUIT }
                                                                         equals new { ev.CENTRE, ev.CLIENT, ev.ORDRE, ev.POINT, ev.PRODUIT }
                                            where StatusIndex.Contains(ev.STATUS) // confusion avec la procédure stockée corresp
                                            group new { d, ev } by new { d.CENTRE, d.CLIENT, d.ORDRE, d.POINT, d.PRODUIT } into pJoin
                                            select new
                                            {
                                                pJoin.Key.CENTRE,
                                                pJoin.Key.CLIENT,
                                                pJoin.Key.ORDRE,
                                                pJoin.Key.PRODUIT,
                                                pJoin.Key.POINT,
                                                MaxEvenement = pJoin.Max(o => o.ev.NUMEVENEMENT)
                                            };

                    var AnclientMax = from a in MaxEvenemt
                                      join ev in context.EVENEMENT on new { a.CENTRE, a.CLIENT, a.ORDRE, a.POINT, a.PRODUIT }
                                                                     equals new { ev.CENTRE, ev.CLIENT, ev.ORDRE, ev.POINT, ev.PRODUIT }
                                      where ev.NUMEVENEMENT == a.MaxEvenement
                                      select new
                                      {
                                          a.CENTRE,
                                          a.CLIENT,
                                          a.ORDRE,
                                          a.PRODUIT,
                                          a.POINT,
                                          a.MaxEvenement
                                      };
                    var Anclient = from a in MaxEvenemtFacture
                                   join ev in context.EVENEMENT on new { a.CENTRE, a.CLIENT, a.ORDRE, a.POINT, a.PRODUIT }
                                                                  equals new { ev.CENTRE, ev.CLIENT, ev.ORDRE, ev.POINT, ev.PRODUIT }
                                   where ev.NUMEVENEMENT == a.MaxEvenement
                                   select new
                                   {
                                       a.CENTRE,
                                       a.CLIENT,
                                       a.ORDRE,
                                       a.PRODUIT,
                                       a.POINT,
                                       a.MaxEvenement,
                                       DANCIENINDEX = ev.INDEXEVT,
                                       ANCIENCAS = ev.CAS,
                                       ev.DATEEVT,
                                       ev.QTEAREG
                                   };

                    var query2 = from q1 in query
                                 join an in Anclient on new { q1.CENTRE, q1.CLIENT, q1.ORDRE, q1.POINT, q1.PRODUIT }
                                                     equals new { an.CENTRE, an.CLIENT, an.ORDRE, an.POINT, an.PRODUIT }
                                 join anMax in AnclientMax on new { q1.CENTRE, q1.CLIENT, q1.ORDRE, q1.POINT, q1.PRODUIT }
                                                     equals new { anMax.CENTRE, anMax.CLIENT, anMax.ORDRE, anMax.POINT, anMax.PRODUIT }
                                 select new
                                 {
                                     q1.CENTRE,
                                     q1.CLIENT,
                                     q1.ORDRE,
                                     q1.TOURNEE,
                                     q1.ORDTOUR,
                                     q1.PRODUIT,
                                     q1.PUISSANCE,
                                     q1.DRES,
                                     q1.PERFAC,
                                     COMPTEUR = q1.NUMERO,
                                     q1.REGLAGECOMPTEUR ,
                                     q1.COEFLECT,
                                     q1.COEFCOMPTAGE,
                                     q1.TYPECOMPTEUR,
                                     q1.PROPRIETAIRE,
                                     q1.POINT,
                                     q1.CATEGORIE,
                                     q1.TYPETARIF,
                                     q1.FORFAIT,
                                     q1.CODECONSO,
                                     q1.PUISSANCEINSTALLEE,

                                     q1.FK_IDCLIENT,
                                     q1.FK_IDCANALISATION,
                                     q1.FK_IDABON,
                                     q1.FK_IDCENTRE,
                                     q1.FK_IDPRODUIT,
                                     q1.FK_IDTOURNEE,
                                     q1.FK_IDCOMPTEUR,
                                     q1.FK_IDCATEGORIE,
                                     q1.FK_IDRELEVEUR,
                                     NUMEVENEMENT = anMax.MaxEvenement,
                                     an.QTEAREG,
                                     an.DANCIENINDEX,
                                     an.ANCIENCAS,
                                     an.DATEEVT,

                                 };
                    return Galatee.Tools.Utility.ListToDataTable<object>(query2);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable  RetourneMaxEvenement(int fk_idcentre, string centre, string client, string ordre,string produit, int FK_POINT)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    List<EVENEMENT> _lstEvt = new List<EVENEMENT>();
                    List<EVENEMENT> _lstEvtInit =context.EVENEMENT.Where(e =>e.FK_IDCENTRE ==fk_idcentre && e.CENTRE == centre && e.CLIENT == client && e.ORDRE == ordre && e.POINT == FK_POINT && e.PRODUIT == produit).ToList();
                    if (_lstEvtInit != null && _lstEvtInit.Count != 0)
                    {
                        int max = context.EVENEMENT.Where(p => p.FK_IDCENTRE == fk_idcentre && p.CENTRE == centre && p.CLIENT == client && p.ORDRE == ordre && p.POINT == FK_POINT && p.PRODUIT == produit).Max(p => p.NUMEVENEMENT);
                        EVENEMENT _evenement = context.EVENEMENT.FirstOrDefault(p => p.FK_IDCENTRE == fk_idcentre && p.CENTRE == centre && p.CLIENT == client && p.ORDRE == ordre && p.POINT == FK_POINT && p.PRODUIT == produit && p.NUMEVENEMENT == max);
                   
                        _lstEvt.Add(_evenement);
                        IEnumerable<object> query = from x in _lstEvt
                                                    select new
                                                    {
                                                        x.CENTRE,
                                                        x.CLIENT,
                                                        x.PRODUIT,
                                                        x.POINT,
                                                        x.NUMEVENEMENT,
                                                        x.ORDRE,
                                                        x.COMPTEUR,
                                                        x.DATEEVT,
                                                        x.PERIODE,
                                                        x.CODEEVT,
                                                        x.INDEXEVT,
                                                        x.CAS,
                                                        x.ENQUETE,
                                                        x.CONSO,
                                                        x.CONSONONFACTUREE,
                                                        x.LOTRI,
                                                        x.FACTURE,
                                                        x.SURFACTURATION,
                                                        x.STATUS,
                                                        x.TYPECONSO,
                                                        x.REGLAGECOMPTEUR,
                                                        x.MATRICULE,
                                                        x.FACPER,
                                                        x.QTEAREG,
                                                        x.DERPERF,
                                                        x.DERPERFN,
                                                        x.CONSOFAC,
                                                        x.REGIMPUTE,
                                                        x.REGCONSO,
                                                        x.COEFLECT,
                                                        x.COEFCOMPTAGE,
                                                        x.PUISSANCE,
                                                        x.TYPECOMPTAGE,
                                                        x.TYPECOMPTEUR,
                                                        x.COEFK1,
                                                        x.COEFK2,
                                                        x.COEFFAC,
                                                        x.USERCREATION,
                                                        x.DATECREATION,
                                                        x.DATEMODIFICATION,
                                                        x.USERMODIFICATION,
                                                        x.PK_ID,
                                                        //x.FK_IDCAS,
                                                        x.FK_IDCENTRE,
                                                        x.FK_IDPRODUIT,
                                                        x.FK_IDCANALISATION
                                                    };

                        return Galatee.Tools.Utility.ListToDataTable(query);
                    }
                    else
                        return null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static int[] StatusEvenementFacture()
        {
            int[] strArray =
                {
                  Enumere.EvenementFacture, Enumere.EvenementMisAJour, Enumere.EtatEvenementPurge,
                };
            return strArray;

        }
        public static int[] StatusEvenementSaisie()
        {
            int[] strArray =
                {
                  Enumere.EvenementReleve,Enumere.EvenementFacture, Enumere.EvenementMisAJour, Enumere.EtatEvenementPurge,Enumere.EvenementDefacture,Enumere.EvenementRejeter 
                };
            return strArray;

        }
        public static int[] StatusEvenementNonFacture()
        {
                int[] strArray =
                {
                  Enumere.EvenementCree ,Enumere.EvenementReleve ,Enumere.EvenementDefacture,Enumere.EvenementRejeter 
                };
                return strArray;
           
        }
        public static CsEvenement    VerifieEvtNonFacture(int fk_Idcentre, string centre, string client, string ordre, string produit,string Periode, int point)
        {
            try
            {
                int[] LstStatut = StatusEvenementNonFacture();
                List<EVENEMENT> _lstEvt = new List<EVENEMENT>();
                CsEvenement ReturneValu = new CsEvenement();
                using (galadbEntities context = new galadbEntities())
                {
                    int MaxEvt = 0;
                    _lstEvt = context.EVENEMENT.Where(e =>e.FK_IDCENTRE == fk_Idcentre && 
                                                          e.CENTRE == centre && 
                                                          e.CLIENT == client && 
                                                          e.ORDRE == ordre && 
                                                          e.POINT == point &&
                                                          e.PRODUIT == produit &&
                                                          e.PERIODE  == Periode &&
                                                          LstStatut.Contains(e.STATUS.Value)).ToList();

                    //List<EVENEMENT> LMaxEvt = context.EVENEMENT.Where(e =>e.FK_IDCENTRE == fk_Idcentre && 
                    //                                      e.CENTRE == centre && 
                    //                                      e.CLIENT == client && 
                    //                                      e.ORDRE == ordre && 
                    //                                      e.POINT == point && 
                    //                                      e.PRODUIT == produit).OrderByDescending(o=>o.DATEEVT ).ToList();

                    List<EVENEMENT> LMaxEvt = _lstEvt.OrderByDescending(o => o.DATEEVT).ToList();

                    if (LMaxEvt != null && LMaxEvt.Count != 0)
                        MaxEvt = LMaxEvt.FirstOrDefault().PK_ID;

                    if (_lstEvt.Count != 0)
                    {
                        ReturneValu = Galatee.Tools.Utility.ConvertEntity<CsEvenement, EVENEMENT>(_lstEvt.FirstOrDefault(t => t.PK_ID == MaxEvt));
                    }
                }
                return ReturneValu;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static EVENEMENT  SupprimerEvtNonFacture(int fk_Idcentre, string centre, string client, string ordre, string produit, int point)
        {
            try
            {
                int[] LstStatut = StatusEvenementNonFacture();
                List<EVENEMENT> _lstEvt = new List<EVENEMENT>();
                EVENEMENT le = new EVENEMENT();
                using (galadbEntities context = new galadbEntities())
                {
                    int MaxEvt = 0;
                    _lstEvt = context.EVENEMENT.Where(e => e.FK_IDCENTRE == fk_Idcentre &&
                                                          e.CENTRE == centre &&
                                                          e.CLIENT == client &&
                                                          e.ORDRE == ordre &&
                                                          e.POINT == point &&
                                                          e.PRODUIT == produit &&
                                                          LstStatut.Contains(e.STATUS.Value)).OrderByDescending(t => t.NUMEVENEMENT).ToList();

                    List<EVENEMENT> LMaxEvt = context.EVENEMENT.Where(e => e.FK_IDCENTRE == fk_Idcentre &&
                                                          e.CENTRE == centre &&
                                                          e.CLIENT == client &&
                                                          e.ORDRE == ordre &&
                                                          e.POINT == point &&
                                                          e.PRODUIT == produit).ToList();
                    if (LMaxEvt != null && LMaxEvt.Count != 0)
                        MaxEvt = LMaxEvt.Max(t => t.NUMEVENEMENT);

                    if (_lstEvt.Count != 0)
                    {
                        if (_lstEvt.First().NUMEVENEMENT == MaxEvt)
                        {
                            le.STATUS = Enumere.EvenementSupprimer;
                            le.DATEMODIFICATION = System.DateTime.Now;
                            le = _lstEvt.First();
                        }
                    }
                }
                return le;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable RetourneEvenementPrecedentPeriode(int fk_idcentre, string centre, string client, string ordre, string produit, int point,string Periode)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    int[] LstStatut = StatusEvenementSaisie();
                        IEnumerable<object> query = from x in context.EVENEMENT 
                                                    where x.FK_IDCENTRE == fk_idcentre && x.CENTRE == centre &&
                                                        x.CLIENT == client && x.ORDRE == ordre && x.POINT == point &&
                                                        x.PRODUIT == produit && LstStatut.Contains(x.STATUS.Value) && x.PERIODE == Periode
                                                    select new
                                                    {
                                                        x. CENTRE ,
                                                                x. CLIENT ,
                                                                x. PRODUIT ,
                                                                x.POINT ,
                                                                x. NUMEVENEMENT ,
                                                                x. ORDRE ,
                                                                x. COMPTEUR ,
                                                                x. DATEEVT ,
                                                                x. PERIODE ,
                                                                x. CODEEVT ,
                                                                x. INDEXEVT ,
                                                                x. CAS ,
                                                                x. ENQUETE ,
                                                                x. CONSO ,
                                                                x. CONSONONFACTUREE ,
                                                                x. LOTRI ,
                                                                x. FACTURE ,
                                                                x. SURFACTURATION ,
                                                                x. STATUS ,
                                                                x. TYPECONSO ,
                                                                x. REGLAGECOMPTEUR ,
                                                                x. TYPETARIF ,
                                                                x. FORFAIT ,
                                                                x. CATEGORIE ,
                                                                x. CODECONSO ,
                                                                x. PROPRIO ,
                                                                x. STATUTCOMPTEUR ,
                                                                x. MODEPAIEMENT ,
                                                                x. MATRICULE ,
                                                                x. FACPER ,
                                                                x. QTEAREG ,
                                                                x. DERPERF ,
                                                                x. DERPERFN ,
                                                                x. CONSOFAC ,
                                                                x. REGIMPUTE ,
                                                                x. REGCONSO ,
                                                                x. COEFLECT ,
                                                                x. COEFCOMPTAGE ,
                                                                x. PUISSANCE ,
                                                                x. TYPECOMPTAGE ,
                                                                x. TYPECOMPTEUR ,
                                                                x. COEFK1 ,
                                                                x. COEFK2 ,
                                                                x. COEFKR1 ,
                                                                x. COEFKR2 ,
                                                                x. COEFFAC ,
                                                                x. USERCREATION ,
                                                                x.DATECREATION ,
                                                                x. DATEMODIFICATION ,
                                                                x. USERMODIFICATION ,
                                                                x. PK_ID ,
                                                                x. FK_IDCANALISATION ,
                                                                x. FK_IDABON ,
                                                                x. FK_IDCOMPTEUR ,
                                                                x. FK_IDCENTRE ,
                                                                x. FK_IDPRODUIT ,
                                                                x. ESTCONSORELEVEE ,
                                                                x. FK_IDTOURNEE ,
                                                                x. TOURNEE ,
                                                                x. ORDTOUR ,
                                                                x. PERFAC ,
                                                                x. CONSOMOYENNEPRECEDENTEFACTURE ,
                                                                x. DATERELEVEPRECEDENTEFACTURE ,
                                                                x. CASPRECEDENTEFACTURE ,
                                                                x. INDEXPRECEDENTEFACTURE ,
                                                                x. PERIODEPRECEDENTEFACTURE ,
                                                                x. ORDREAFFICHAGE ,
                                                                x. NOUVEAUCOMPTEUR ,
                                                                x. PUISSANCEINSTALLEE ,
                                                                x. QTEAREGPRECEDENT ,
                                                                x. ISCONSOSEULE ,
                                                                x. ISEXONERETVA ,
                                                                x. DEBUTEXONERATIONTVA ,
                                                                x. FINEXONERATIONTVA ,
                                                                x. COMMENTAIRE
                                                    };
                        return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static DataTable RetourneEvenementPrecedent(int fk_idcentre, string centre, string client, string ordre, string produit, int point)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    int[] LstStatut = StatusEvenementFacture();
                    List<EVENEMENT> _lstEvt = new List<EVENEMENT>();
                    List<EVENEMENT> _lstEvtInit = context.EVENEMENT.Where(e => e.FK_IDCENTRE ==fk_idcentre && e.CENTRE == centre && 
                                                                               e.CLIENT == client && e.ORDRE == ordre && e.POINT == point &&
                                                                               e.PRODUIT == produit && LstStatut.Contains(e.STATUS.Value)).OrderByDescending(p=>p.DATEEVT).ToList();
                    if (_lstEvtInit != null && _lstEvtInit.Count != 0)
                    {
                        _lstEvt.Add(_lstEvtInit.First());
                        IEnumerable<object> query = from x in _lstEvt
                                                    select new
                                                     {
                                            x. CENTRE ,
                                                                x. CLIENT ,
                                                                x. PRODUIT ,
                                                                x.POINT ,
                                                                x. NUMEVENEMENT ,
                                                                x. ORDRE ,
                                                                x. COMPTEUR ,
                                                                x. DATEEVT ,
                                                                x. PERIODE ,
                                                                x. CODEEVT ,
                                                                x. INDEXEVT ,
                                                                x. CAS ,
                                                                x. ENQUETE ,
                                                                x. CONSO ,
                                                                x. CONSONONFACTUREE ,
                                                                x. LOTRI ,
                                                                x. FACTURE ,
                                                                x. SURFACTURATION ,
                                                                x. STATUS ,
                                                                x. TYPECONSO ,
                                                                x. REGLAGECOMPTEUR ,
                                                                x. TYPETARIF ,
                                                                x. FORFAIT ,
                                                                x. CATEGORIE ,
                                                                x. CODECONSO ,
                                                                x. PROPRIO ,
                                                                x. MODEPAIEMENT ,
                                                                x. MATRICULE ,
                                                                x. FACPER ,
                                                                x. QTEAREG ,
                                                                x. DERPERF ,
                                                                x. DERPERFN ,
                                                                x. CONSOFAC ,
                                                                x. REGIMPUTE ,
                                                                x. REGCONSO ,
                                                                x. COEFLECT ,
                                                                x. COEFCOMPTAGE ,
                                                                x. PUISSANCE ,
                                                                x. TYPECOMPTAGE ,
                                                                x. TYPECOMPTEUR ,
                                                                x. COEFK1 ,
                                                                x. COEFK2 ,
                                                                x. COEFKR1 ,
                                                                x. COEFKR2 ,
                                                                x. COEFFAC ,
                                                                x. USERCREATION ,
                                                                x.DATECREATION ,
                                                                x. DATEMODIFICATION ,
                                                                x. USERMODIFICATION ,
                                                                x. PK_ID ,
                                                                x. FK_IDCANALISATION ,
                                                                x. FK_IDABON ,
                                                                x. FK_IDCOMPTEUR ,
                                                                x. FK_IDCENTRE ,
                                                                x. FK_IDPRODUIT ,
                                                                x. ESTCONSORELEVEE ,
                                                                x. FK_IDTOURNEE ,
                                                                x. TOURNEE ,
                                                                x. ORDTOUR ,
                                                                x. PERFAC ,
                                                                x. CONSOMOYENNEPRECEDENTEFACTURE ,
                                                                x. DATERELEVEPRECEDENTEFACTURE ,
                                                                x. CASPRECEDENTEFACTURE ,
                                                                x. INDEXPRECEDENTEFACTURE ,
                                                                x. PERIODEPRECEDENTEFACTURE ,
                                                                x. ORDREAFFICHAGE ,
                                                                x. NOUVEAUCOMPTEUR ,
                                                                x. PUISSANCEINSTALLEE ,
                                                                x. QTEAREGPRECEDENT ,
                                                                x. ISCONSOSEULE ,
                                                                x. ISEXONERETVA ,
                                                                x. DEBUTEXONERATIONTVA ,
                                                                x. FINEXONERATIONTVA ,
                                                                x. COMMENTAIRE,
                                                                NOMABON = x.CANALISATION.ABON.CLIENT1.NOMABON,
                                                                CADRAN = x.CANALISATION.COMPTEUR.CADRAN,
                                                                RUE = x.CANALISATION.ABON.CLIENT1.AG.RUE,
                                                                PORTE = x.CANALISATION.ABON.CLIENT1.AG.PORTE,
                                                                COMPTEURAFFICHER = x.COMPTEUR ,
                                                                LIBELLETYPECOMPTEUR = x.COMPTEUR1.TYPECOMPTEUR1.LIBELLE,
                                                                STATUTCOMPTEUR= x.COMPTEUR1.STATUTCOMPTEUR.CODE  
                                   };
                        return Galatee.Tools.Utility.ListToDataTable(query);
                    }
                    else
                        return null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        //public static DataTable RetourneListePointsDesProduitsDuClient(string Centre, string client)
        //{
        //    try
        //    {
        //        using (galadbEntities context = new galadbEntities())
        //        {
        //            var Canal = context.CANALISATION;
        //            var Event = context.EVENEMENT;   // on utilise la table EVENEMENT pour ramener en meme temps le dernier evenemnt sur le point du produit
        //            IEnumerable<object> query = from _canal in Canal
        //                                        join _event in Event on new
        //                                        {
        //                                            _canal.CENTRE,
        //                                            //_canal.CLIENT,
        //                                            _canal.PRODUIT,
        //                                            _canal.POINT
        //                                        }
        //                                                             equals new
        //                                                             {
        //                                                                 CENTRE = _event.CENTRE,
        //                                                                 CLIENT = _event.CLIENT,
        //                                                                 PRODUIT = _event.PRODUIT,
        //                                                                 POINT = _event.POINT
        //                                                             }
        //                                        where _canal.CENTRE == Centre && _canal.CLIENT == client
        //                                        select new
        //                                        {
        //                                            _event.DATEEVT,
        //                                            _event.PERIODE,
        //                                            _event.LOTRI,
        //                                            _event.STATUS,
        //                                            _event.COMPTEUR,
        //                                            _event.CAS,
        //                                            PK_EVENEMENT = (int)Event.Where(e => e.CENTRE == _event.CLIENT &&
        //                                                                   e.CLIENT == _event.CLIENT && e.ORDRE == _event.ORDRE &&
        //                                                                   e.PRODUIT == _canal.PRODUIT && e.POINT == _canal.POINT).Max(e => e.NUMEVENEMENT)
        //                                        };

        //            return Galatee.Tools.Utility.ListToDataTable<object>(query);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public static DataTable RetournerListeLotNonSaisi(List<int> lsCentrePerimetre)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    //var tournee = context.LOTRI.Where(l => (l.ETATFAC5 == Enumere.NonMiseAJours || string.IsNullOrEmpty(l.ETATFAC5)) && lsCentrePerimetre.Contains(l.FK_IDCENTRE));
                    var lotri = context.LOTRI;
                    IEnumerable<object> query = from t in lotri
                                                where lsCentrePerimetre.Contains(t.FK_IDCENTRE)
                                                select new
                                                {
                                                    t.NUMLOTRI,
                                                    t.JET,
                                                    t.PERIODE,
                                                    t.CENTRE,
                                                    t.PRODUIT,
                                                    t.CATEGORIECLIENT,
                                                    t.PERIODICITE,
                                                    t.EXIG,
                                                    t.DFAC,
                                                    t.ETATFAC1,
                                                    t.ETATFAC2,
                                                    t.ETATFAC3,
                                                    t.ETATFAC4,
                                                    t.ETATFAC5,
                                                    t.ETATFAC6,
                                                    t.ETATFAC7,
                                                    t.ETATFAC8,
                                                    t.ETATFAC9,
                                                    t.ETATFAC10,
                                                    t.TOURNEE,
                                                    t.RELEVEUR,
                                                    t.BASE,
                                                    t.PK_ID,
                                                    t.DATECREATION,
                                                    t.DATEMODIFICATION,
                                                    t.USERCREATION,
                                                    t.USERMODIFICATION,
                                                    t.FK_IDPRODUIT,
                                                    t.FK_IDCATEGORIECLIENT,
                                                    t.FK_IDRELEVEUR,
                                                    t.FK_IDCENTRE,
                                                    t.FK_IDTOURNEE ,
                                                    LIBELLECENTRE=   t.CENTRE1.LIBELLE 
                                                    //NOMUSER = t.RELEVEUR1.ADMUTILISATEUR .LIBELLE 
                                                };
                    return Galatee.Tools.Utility.ListToDataTable<object>(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        static string PeriodeFacturationEnCours(string lotri, string centre)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    if(!string.IsNullOrEmpty(centre))
                       return context.EVENEMENT.FirstOrDefault(e => e.LOTRI == lotri && e.CENTRE == centre).PERIODE;
                    else
                        return context.EVENEMENT.FirstOrDefault(e => e.LOTRI == lotri).PERIODE;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static DataTable RetourneListeEvenementDuLotPourSynchro(CsLotri leLotri, string sequence)
        {
            try
            {
                //string periodeEncours = PeriodeFacturationEnCours(lotri, centre);
                using (galadbEntities context = new galadbEntities())
                {
                    //var client = context.CLIENT;
                    var Evt = context.EVENEMENT;
                    var Transfert = context.TRANSFERT ;

                    var trans = (from p in Transfert 
                                 join t in Evt on p.PK_ID equals t.PK_ID 
                               
                                  where
                                      p.LOTRI == leLotri.NUMLOTRI && p.FK_IDCENTRE == leLotri.FK_IDCENTRE 
                                   select new
                                   {
                                       p.CENTRE,
                                       p.CLIENT,
                                       p.PRODUIT,
                                       p.POINT,
                                       p.NUMEVENEMENT,
                                       p.ORDRE,
                                       p.COMPTEUR,
                                       p.DATEEVT,
                                       p.PERIODE,
                                       p.CODEEVT,
                                       p.INDEXEVT,
                                       p.CAS,
                                       p.ENQUETE,
                                       p.CONSO,
                                       p.LOTRI,
                                       p.FACTURE,
                                       p.STATUS,
                                       p.REGLAGECOMPTEUR,
                                       p.TYPETARIF,
                                       p.FORFAIT,
                                       p.CATEGORIE,
                                       p.CODECONSO,
                                       p.MATRICULE,
                                       p.QTEAREG,
                                       p.CONSOFAC,
                                       p.COEFLECT,
                                       p.COEFCOMPTAGE,
                                       p.PUISSANCE,
                                       p.TYPECOMPTAGE,
                                       p.TYPECOMPTEUR,
                                       p.COEFK1,
                                       p.COEFK2,
                                       p.COEFFAC,
                                       p.USERCREATION,
                                       p.DATECREATION,
                                       p.DATEMODIFICATION,
                                       p.USERMODIFICATION,
                                       p.PK_ID,
                                       p.FK_IDCENTRE,
                                       p.FK_IDPRODUIT,
                                       p.FK_IDSTATUTTRANSFERT,
                                       p.ESTCONSORELEVEE,
                                       p.COMMENTAIRE
                                      
                                   });
                    return Galatee.Tools.Utility.ListToDataTable<object>(trans);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public static DataTable RetourneListeEvenementDuLotPourSaisiIndex(CsLotri leLotri, string sequence)
        //{
        //    try
        //    {
        //        using (galadbEntities context = new galadbEntities())
        //        {
        //                              var client = context.CLIENT;
        //            var abonNonRes = context.ABON.Where(a => a.DRES == null);
        //            //IEnumerable<EVENEMENT> events;
        //            IEnumerable<PAGERI> pageri;
        //            var eventValide = context.EVENEMENT.Where(e => (new int[] { Enumere.EvenementFacture, Enumere.EvenementMisAJour, Enumere.EtatEvenementPurge, Enumere.EvenementReleve }.Contains(e.STATUS.Value)) &&
        //                                                           !(new string[] { Enumere.CasDeposeCompteur, Enumere.CasPoseCompteur }.Contains(e.CAS)) 
        //                                                           && e.FK_IDCENTRE ==leLotri.FK_IDCENTRE && e.ABON.CLIENT1.AG.TOURNEE  == leLotri.TOURNEE );

        //            // ramener les clients du lot qui se trouvent dans la table pageri dont les index n'ont pas Ã©tÃ© saisis 
        //            if (!string.IsNullOrEmpty(leLotri.TOURNEE ) && !string.IsNullOrEmpty(sequence))
        //                pageri = context.PAGERI.Where(p => p.CENTRE == leLotri.CENTRE && p.LOTRI == leLotri.NUMLOTRI && p.TOURNEE == leLotri.TOURNEE  && p.ORDTOUR == sequence &&
        //                                             (p.CAS == Enumere.CompteurNonLu || p.CAS == Enumere.CompteurChanged));
        //            else
        //            {
        //                if (string.IsNullOrEmpty(leLotri.TOURNEE ))
        //                {
        //                    if (string.IsNullOrEmpty(sequence))
        //                    {
        //                        if (string.IsNullOrEmpty(leLotri.CENTRE ))
        //                            pageri = context.PAGERI.Where(p => p.LOTRI == leLotri.NUMLOTRI  &&
        //                                                (p.CAS == Enumere.CompteurNonLu || p.CAS == Enumere.CompteurChanged));
        //                        else
        //                            pageri = context.PAGERI.Where(p => p.LOTRI == leLotri.NUMLOTRI && p.CENTRE == leLotri.CENTRE  &&
        //                                               (p.CAS == Enumere.CompteurNonLu || p.CAS == Enumere.CompteurChanged));
        //                    }
        //                    else
        //                    {
        //                        pageri = context.PAGERI.Where(p => p.CENTRE == leLotri.CENTRE && p.LOTRI == leLotri.NUMLOTRI && p.ORDTOUR == sequence &&
        //                                                (p.CAS == Enumere.CompteurNonLu || p.CAS == Enumere.CompteurChanged));
        //                    }
        //                }
        //                else
        //                {
        //                    if (string.IsNullOrEmpty(sequence))
        //                        pageri = context.PAGERI.Where(p => p.CENTRE == leLotri.CENTRE && p.LOTRI == leLotri.NUMLOTRI && p.TOURNEE == leLotri.TOURNEE  &&
        //                                                (p.CAS == Enumere.CompteurNonLu || p.CAS == Enumere.CompteurChanged));
        //                    else
        //                        pageri = context.PAGERI.Where(p => p.CENTRE == leLotri.CENTRE && p.LOTRI == leLotri.NUMLOTRI && p.TOURNEE == leLotri.TOURNEE  && p.ORDTOUR == sequence &&
        //                                                (p.CAS == Enumere.CompteurNonLu || p.CAS == Enumere.CompteurChanged));
        //                }
        //            }
        //            var maxevent = from evnt in eventValide
        //                           join pg in pageri on new {evnt.FK_IDCENTRE , evnt.CENTRE, evnt.CLIENT, evnt.PRODUIT, evnt.POINT }
        //                                             equals new {pg.FK_IDCENTRE , pg.CENTRE, pg.CLIENT, pg.PRODUIT, pg.POINT }
        //                           group new { evnt, pg } by new {pg.FK_IDCENTRE ,  pg.CENTRE, pg.CLIENT, pg.POINT, pg.PRODUIT } into pJ
        //                           select new
        //                           {
        //                               pJ.Key.CENTRE,
        //                               pJ.Key.CLIENT,
        //                               pJ.Key.PRODUIT,
        //                               pJ.Key.POINT,
        //                               pJ.Key.FK_IDCENTRE ,
        //                               MAXEVENMENT = pJ.Max(o => o.evnt.NUMEVENEMENT)
        //                           };



        //            var query2 = (from q in maxevent
        //                          join ev in context.EVENEMENT on new {q.FK_IDCENTRE , q.CENTRE, q.CLIENT, q.PRODUIT, q.POINT, NUMEVENEMENT = q.MAXEVENMENT }
        //                                                 equals new {ev.FK_IDCENTRE , ev.CENTRE, ev.CLIENT, ev.PRODUIT, ev.POINT, ev.NUMEVENEMENT }
        //                          join pg in pageri on new {q.FK_IDCENTRE , q.CENTRE, q.CLIENT, q.PRODUIT, q.POINT }
        //                                            equals new {pg.FK_IDCENTRE , pg.CENTRE, pg.CLIENT, pg.PRODUIT, pg.POINT }

        //                          join pgs in context.TOURNEERELEVEUR  on new { pg.FK_IDTOURNEE  }
        //                                            equals new { pgs.FK_IDTOURNEE}
                                  
        //                          select new
        //                          {
        //                              ev.CENTRE,
        //                              ev.CLIENT,
        //                              ev.POINT,
        //                              ev.ABON.CLIENT1.NOMABON,
        //                              ev.NUMEVENEMENT,
        //                              ev.COMPTEUR,
        //                              pg.TOURNEE,
        //                              //NOMPIA = m.LIBELLE,***
        //                              ev.PERIODE,
        //                              ev.PRODUIT,
        //                              ev.LOTRI,
        //                              ev.FACTURE,
        //                              ev.SURFACTURATION,
        //                              ev.STATUS,
        //                              ev.TYPECONSO,
        //                              ev.DIAMETRE,
        //                              ev.MATRICULE,
        //                              ev.FACPER,
        //                              ev.QTEAREG,
        //                              ev.DERPERF,
        //                              ev.DERPERFN,
        //                              ev.CONSOFAC,
        //                              ev.REGIMPUTE,
        //                              ev.REGCONSO,
        //                              ev.COEFLECT,
        //                              ev.COEFCOMPTAGE,
        //                              ev.PUISSANCE,
        //                              ev.TYPECOMPTAGE,
        //                              ev.TYPECOMPTEUR,
        //                              ev.COEFK1,
        //                              ev.COEFK2,
        //                              ev.COEFFAC,
        //                              ev.ORDRE,
        //                              INDEXEVTPRECEDENT = q.,
        //                              CASPRECEDENT = q.ANCIENCAS,
        //                              CONSOPRECEDENT = q.CONSO,
        //                              CONSOFACPRECEDENT = q.CONSOFAC,
        //                              pg.ORDTOUR,

        //                              RELEVEUR=  pgs.RELEVEUR.ADMUTILISATEUR.LIBELLE ,
        //                              CADRAN =ev.CANALISATION.COMPTEUR.CADRAN,
        //                              ev.CANALISATION.ABON.CLIENT1.AG.RUE ,
        //                              ev.CANALISATION.ABON.CLIENT1.AG.PORTE  ,

        //                              FK_IDPAGERI=  pg.PK_ID ,
        //                              FK_IDEVENEMENT= ev.PK_ID,
        //                              ev.FK_IDCANALISATION,
        //                              ev.FK_IDCENTRE,
        //                              ev.FK_IDPRODUIT,
        //                              pgs.FK_IDRELEVEUR 
        //                          }).Distinct();
        //            return Galatee.Tools.Utility.ListToDataTable<object>(query2);  
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public static DataTable ChargerListeDesEvenementsNonFacture(List<int> idCanalisation)
        {
            try
            {
                List<int?> StatusIndex = new List<int?>() { 3, 4, 99 };
                using (galadbEntities context = new galadbEntities())
                {
                    var EvenementClient = context.EVENEMENT ;
                    var pagerie = (from p in EvenementClient
                                   where
                                   new int[] { Enumere.EvenementCree }.Contains(p.STATUS.Value) &&
                                   !(new string[] { Enumere.CasDeposeCompteur, Enumere.CasPoseCompteur }.Contains(p.CAS)) && 
                                   idCanalisation.Contains(  p.FK_IDCOMPTEUR)
                                   select new
                                   {
                                       p.CENTRE,
                                       p.CLIENT,
                                       p.POINT,
                                       NOMABON = p.CANALISATION.ABON.CLIENT1.NOMABON,
                                       p.NUMEVENEMENT,
                                       p.COMPTEUR,
                                       PERIODE = p.PERIODE,
                                       p.PRODUIT,
                                       LOTRI = p.LOTRI,
                                       p.FACTURE,
                                       p.SURFACTURATION,
                                       p.STATUS,
                                       p.TYPECONSO,
                                       p.REGLAGECOMPTEUR,
                                       p.MATRICULE,
                                       p.FACPER,
                                       p.QTEAREG,
                                       p.DERPERF,
                                       p.DERPERFN,
                                       p.CONSOFAC,
                                       p.REGIMPUTE,
                                       p.REGCONSO,
                                       p.COEFLECT,
                                       p.COEFCOMPTAGE,
                                       p.PUISSANCE,
                                       p.TYPECOMPTAGE,
                                       p.TYPECOMPTEUR,
                                       p.COEFK1,
                                       p.COEFK2,
                                       p.COEFFAC,
                                       p.ORDRE,
                                       p.PK_ID ,
                                       p.FK_IDCANALISATION,
                                       p.FK_IDCENTRE,
                                       p.FK_IDPRODUIT,
                                       p.FK_IDABON,
                                       p.FK_IDCOMPTEUR ,

                                       CADRAN = p.CANALISATION.COMPTEUR.CADRAN,
                                       RUE = p.CANALISATION.ABON.CLIENT1.AG.RUE,
                                       PORTE = p.CANALISATION.ABON.CLIENT1.AG.PORTE,
                                   });
                    var distinctClient = (from d in pagerie select new { d.FK_IDCENTRE, d.CENTRE, d.CLIENT, d.ORDRE, d.POINT, d.PRODUIT }).Distinct();
                    var MaxEvenemtFacture = from d in distinctClient
                                            join ev in context.EVENEMENT on new { d.FK_IDCENTRE, d.CENTRE, d.CLIENT, d.ORDRE, d.POINT, d.PRODUIT }
                                                                         equals new { ev.FK_IDCENTRE, ev.CENTRE, ev.CLIENT, ev.ORDRE, ev.POINT, ev.PRODUIT }
                                            where StatusIndex.Contains(ev.STATUS) 
                                            group new { d, ev } by new { ev.FK_IDCENTRE, d.CENTRE, d.CLIENT, d.ORDRE, d.POINT, d.PRODUIT } into pJoin
                                            select new
                                            {
                                                pJoin.Key.CENTRE,
                                                pJoin.Key.CLIENT,
                                                pJoin.Key.ORDRE,
                                                pJoin.Key.PRODUIT,
                                                pJoin.Key.POINT,
                                                pJoin.Key.FK_IDCENTRE,
                                                MaxEvenement = pJoin.Max(o => o.ev.NUMEVENEMENT)
                                            };

                    var Anclient = from a in MaxEvenemtFacture
                                   join ev in context.EVENEMENT on new { a.FK_IDCENTRE, a.CENTRE, a.CLIENT, a.ORDRE, a.POINT, a.PRODUIT }
                                                                  equals new { ev.FK_IDCENTRE, ev.CENTRE, ev.CLIENT, ev.ORDRE, ev.POINT, ev.PRODUIT }
                                   where ev.NUMEVENEMENT == a.MaxEvenement
                                   select new
                                   {
                                       a.CENTRE,
                                       a.CLIENT,
                                       a.ORDRE,
                                       a.PRODUIT,
                                       a.POINT,
                                       a.MaxEvenement,
                                       DANCIENINDEX = ev.INDEXEVT,
                                       ANCIENCAS = ev.CAS,
                                       ev.CONSO,
                                       ev.CONSOFAC,
                                       ev.DATEEVT,
                                       ev.QTEAREG,
                                       ev.FK_IDCENTRE
                                   };

                    var query2 = from p in pagerie
                                 join an in Anclient on new { p.FK_IDCENTRE, p.CENTRE, p.CLIENT, p.ORDRE, p.POINT, p.PRODUIT }
                                                     equals new { an.FK_IDCENTRE, an.CENTRE, an.CLIENT, an.ORDRE, an.POINT, an.PRODUIT }
                                 select new
                                 {
                                     p.CENTRE,
                                     p.CLIENT,
                                     p.POINT,
                                     p.NOMABON,
                                     p.NUMEVENEMENT,
                                     p.COMPTEUR,
                                     p.PERIODE,
                                     p.PRODUIT,
                                     p.LOTRI,
                                     p.FACTURE,
                                     p.SURFACTURATION,
                                     p.STATUS,
                                     p.TYPECONSO,
                                     p.REGLAGECOMPTEUR ,
                                     p.MATRICULE,
                                     p.FACPER,
                                     p.QTEAREG,
                                     p.DERPERF,
                                     p.DERPERFN,
                                     p.CONSOFAC,
                                     p.REGIMPUTE,
                                     p.REGCONSO,
                                     p.COEFLECT,
                                     p.COEFCOMPTAGE,
                                     p.PUISSANCE,
                                     p.TYPECOMPTAGE,
                                     p.TYPECOMPTEUR,
                                     p.COEFK1,
                                     p.COEFK2,
                                     p.COEFFAC,
                                     p.ORDRE,
                                     INDEXEVTPRECEDENT = an.DANCIENINDEX,
                                     CASPRECEDENT = an.ANCIENCAS,
                                     CONSOPRECEDENT = an.CONSO,
                                     CONSOFACPRECEDENT = an.CONSOFAC,
                                     p.PK_ID,
                                     p.FK_IDCANALISATION,
                                     p.FK_IDCENTRE,
                                     p.FK_IDPRODUIT,
                                     p.CADRAN,
                                     p.RUE,
                                     p.PORTE,
                                     p.FK_IDABON,
                                     p.FK_IDCOMPTEUR,
                                 };
                    return Galatee.Tools.Utility.ListToDataTable<object>(query2);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable  RetourneNbrClientLot(CsLotri leLotri, string sequence)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    var EvenementClient = context.EVENEMENT;
                    var page = (from p in EvenementClient
                                from t in p.TOURNEE1.TOURNEERELEVEUR
                                where
                                !(new string[] { Enumere.CasDeposeCompteur, Enumere.CasPoseCompteur }.Contains(p.CAS))
                                 && p.LOTRI == leLotri.NUMLOTRI
                                 && (p.FK_IDTOURNEE == leLotri.FK_IDTOURNEE)
                                 && (p.CATEGORIE == leLotri.CATEGORIECLIENT)
                                 && (p.PERFAC == leLotri.PERIODICITE)
                                 && (p.PRODUIT == leLotri.PRODUIT)
                                 && (p.ORDTOUR == sequence || string.IsNullOrEmpty(sequence))
                                 && (p.FK_IDCENTRE == leLotri.FK_IDCENTRE)
                                select new
                                {
                                    p.INDEXPRECEDENTEFACTURE ,
                                    p.CAS 
                                });
                    return Galatee.Tools.Utility.ListToDataTable<object>(page);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable RetourneListeEvenementDuLotPourSaisiIndex(CsLotri leLotri, string sequence)
        {
            try
            {
                List<int?> StatusIndex = new List<int?>() { 3, 4, 99, 8, 7 };
                using (galadbEntities context = new galadbEntities())
                {
                    var EvenementClient = context.EVENEMENT ;
                    var page  = (from p in EvenementClient
                                 //from t in p.TOURNEE1.TOURNEERELEVEUR
                                   where
                                   !(new string[] { Enumere.CasDeposeCompteur, Enumere.CasPoseCompteur }.Contains(p.CAS))
                                    && p.LOTRI == leLotri.NUMLOTRI
                                    && (p.FK_IDTOURNEE == leLotri.FK_IDTOURNEE)
                                    && (p.CATEGORIE == leLotri.CATEGORIECLIENT)
                                    && (p.PERFAC == leLotri.PERIODICITE)
                                    && (p.PRODUIT  == leLotri.PRODUIT )
                                    && (p.ORDTOUR == sequence || string.IsNullOrEmpty(sequence))
                                    && (p.FK_IDCENTRE == leLotri.FK_IDCENTRE)
                                    && (!StatusIndex.Contains(p.STATUS) || (p.STATUS == 3 && p.FACTURE.Contains("*")))
                                   select  new
                                   {
                                            p. CENTRE ,
                                            p. CLIENT ,
                                            p. PRODUIT ,
                                            p.POINT ,
                                            p. NUMEVENEMENT ,
                                            p. ORDRE ,
                                            p. COMPTEUR ,
                                            p. DATEEVT ,
                                            p. PERIODE ,
                                            p. CODEEVT ,
                                            p. INDEXEVT ,
                                            p. CAS ,
                                            p. ENQUETE ,
                                            p. CONSO ,
                                            p. CONSONONFACTUREE ,
                                            p. LOTRI ,
                                            p. FACTURE ,
                                            p. SURFACTURATION ,
                                            p. STATUS ,
                                            p. TYPECONSO ,
                                            p. REGLAGECOMPTEUR ,
                                            p. TYPETARIF ,
                                            p. FORFAIT ,
                                            p. CATEGORIE ,
                                            p. CODECONSO ,
                                            p. PROPRIO ,
                                            p. STATUTCOMPTEUR ,
                                            p. MODEPAIEMENT ,
                                            p. MATRICULE ,
                                            p. FACPER ,
                                            p. QTEAREG ,
                                            p. DERPERF ,
                                            p. DERPERFN ,
                                            p. CONSOFAC ,
                                            p. REGIMPUTE ,
                                            p. REGCONSO ,
                                            p. COEFLECT ,
                                            p. COEFCOMPTAGE ,
                                            p. PUISSANCE ,
                                            p. TYPECOMPTAGE ,
                                            p. TYPECOMPTEUR ,
                                            p. COEFK1 ,
                                            p. COEFK2 ,
                                            p. COEFKR1 ,
                                            p. COEFKR2 ,
                                            p. COEFFAC ,
                                            p. USERCREATION ,
                                            p.DATECREATION ,
                                            p. DATEMODIFICATION ,
                                            p. USERMODIFICATION ,
                                            p. PK_ID ,
                                            p. FK_IDCANALISATION ,
                                            p. FK_IDABON ,
                                            p. FK_IDCOMPTEUR ,
                                            p. FK_IDCENTRE ,
                                            p. FK_IDPRODUIT ,
                                            p. ESTCONSORELEVEE ,
                                            p. FK_IDTOURNEE ,
                                            p. TOURNEE ,
                                            p. ORDTOUR ,
                                            p. PERFAC ,
                                            p. CONSOMOYENNEPRECEDENTEFACTURE ,
                                            p. DATERELEVEPRECEDENTEFACTURE ,
                                            p. CASPRECEDENTEFACTURE ,
                                            p. INDEXPRECEDENTEFACTURE ,
                                            p. PERIODEPRECEDENTEFACTURE ,
                                            p. ORDREAFFICHAGE ,
                                            p. NOUVEAUCOMPTEUR ,
                                            p. PUISSANCEINSTALLEE ,
                                            p. QTEAREGPRECEDENT ,
                                            p. ISCONSOSEULE ,
                                            p. ISEXONERETVA ,
                                            p. DEBUTEXONERATIONTVA ,
                                            p. FINEXONERATIONTVA ,
                                            p. COMMENTAIRE,
                                            NOMABON = p.CANALISATION.ABON.CLIENT1.NOMABON,
                                            //RELEVEUR = t.RELEVEUR.ADMUTILISATEUR.LIBELLE,
                                            LIBELLETYPECOMPTEUR = p.COMPTEUR1.TYPECOMPTEUR1.LIBELLE,
                                            CADRAN = p.CANALISATION.COMPTEUR.CADRAN,
                                            RUE = p.CANALISATION.ABON.CLIENT1.AG.RUE,
                                            PORTE = p.CANALISATION.ABON.CLIENT1.AG.PORTE,
                                            COMPTEURAFFICHER = p.COMPTEUR 
                                   });
                    return Galatee.Tools.Utility.ListToDataTable<object>(page);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable RetourneListeEvenementDuLotIsolePourSaisiIndex(CsLotri leLotri, string sequence)
        {
            try
            {
                List<int?> StatusIndex = new List<int?>() { 3, 4, 99, 8, 7 };
                using (galadbEntities context = new galadbEntities())
                {
                    var EvenementClient = context.EVENEMENT;
                    var page = (from p in EvenementClient
                                where
                                !(new string[] { Enumere.CasDeposeCompteur, Enumere.CasPoseCompteur }.Contains(p.CAS))
                                 && p.LOTRI == leLotri.NUMLOTRI
                                 && (p.ORDTOUR == sequence || string.IsNullOrEmpty(sequence))
                                 && (p.FK_IDCENTRE == leLotri.FK_IDCENTRE)
                                 && (p.USERCREATION == leLotri.USERCREATION  )
                                 && (!StatusIndex.Contains(p.STATUS) || (p.STATUS == 3 && p.FACTURE.Contains("*")))
                                select new
                                {
                                    p.CENTRE,
                                    p.CLIENT,
                                    p.PRODUIT,
                                    p.POINT,
                                    p.NUMEVENEMENT,
                                    p.ORDRE,
                                    p.COMPTEUR,
                                    p.DATEEVT,
                                    p.PERIODE,
                                    p.CODEEVT,
                                    p.INDEXEVT,
                                    p.CAS,
                                    p.ENQUETE,
                                    p.CONSO,
                                    p.CONSONONFACTUREE,
                                    p.LOTRI,
                                    p.FACTURE,
                                    p.SURFACTURATION,
                                    p.STATUS,
                                    p.TYPECONSO,
                                    p.REGLAGECOMPTEUR,
                                    p.TYPETARIF,
                                    p.FORFAIT,
                                    p.CATEGORIE,
                                    p.CODECONSO,
                                    p.PROPRIO,
                                    p.STATUTCOMPTEUR,
                                    p.MODEPAIEMENT,
                                    p.MATRICULE,
                                    p.FACPER,
                                    p.QTEAREG,
                                    p.DERPERF,
                                    p.DERPERFN,
                                    p.CONSOFAC,
                                    p.REGIMPUTE,
                                    p.REGCONSO,
                                    p.COEFLECT,
                                    p.COEFCOMPTAGE,
                                    p.PUISSANCE,
                                    p.TYPECOMPTAGE,
                                    p.TYPECOMPTEUR,
                                    p.COEFK1,
                                    p.COEFK2,
                                    p.COEFKR1,
                                    p.COEFKR2,
                                    p.COEFFAC,
                                    p.USERCREATION,
                                    p.DATECREATION,
                                    p.DATEMODIFICATION,
                                    p.USERMODIFICATION,
                                    p.PK_ID,
                                    p.FK_IDCANALISATION,
                                    p.FK_IDABON,
                                    p.FK_IDCOMPTEUR,
                                    p.FK_IDCENTRE,
                                    p.FK_IDPRODUIT,
                                    p.ESTCONSORELEVEE,
                                    p.FK_IDTOURNEE,
                                    p.TOURNEE,
                                    p.ORDTOUR,
                                    p.PERFAC,
                                    p.CONSOMOYENNEPRECEDENTEFACTURE,
                                    p.DATERELEVEPRECEDENTEFACTURE,
                                    p.CASPRECEDENTEFACTURE,
                                    p.INDEXPRECEDENTEFACTURE,
                                    p.PERIODEPRECEDENTEFACTURE,
                                    p.ORDREAFFICHAGE,
                                    p.NOUVEAUCOMPTEUR,
                                    p.PUISSANCEINSTALLEE,
                                    p.QTEAREGPRECEDENT,
                                    p.ISCONSOSEULE,
                                    p.ISEXONERETVA,
                                    p.DEBUTEXONERATIONTVA,
                                    p.FINEXONERATIONTVA,
                                    p.COMMENTAIRE,
                                    NOMABON = p.CANALISATION.ABON.CLIENT1.NOMABON,
                                    //RELEVEUR = t.RELEVEUR.ADMUTILISATEUR.LIBELLE,
                                    LIBELLETYPECOMPTEUR = p.COMPTEUR1.TYPECOMPTEUR1.LIBELLE,
                                    CADRAN = p.CANALISATION.COMPTEUR.CADRAN,
                                    RUE = p.CANALISATION.ABON.CLIENT1.AG.RUE,
                                    PORTE = p.CANALISATION.ABON.CLIENT1.AG.PORTE,
                                    COMPTEURAFFICHER = p.COMPTEUR
                                });
                    return Galatee.Tools.Utility.ListToDataTable<object>(page);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public static DataTable RetourneListeEvenementSaisiIndexClient(CsEvenement leClient, byte? OrdreAffichage)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    var EvenementClient = context.EVENEMENT;
                    var page = (from p in EvenementClient
                                from t in p.TOURNEE1.TOURNEERELEVEUR
                                where
                                !(new string[] { Enumere.CasDeposeCompteur, Enumere.CasPoseCompteur }.Contains(p.CAS))
                                 && p.LOTRI == leClient.LOTRI
                                 && (p.ORDREAFFICHAGE < OrdreAffichage)
                                 && (p.FK_IDABON == leClient.FK_IDABON)
                                select new
                                {
                                    p.CENTRE,
                                    p.CLIENT,
                                    p.PRODUIT,
                                    p.POINT,
                                    p.NUMEVENEMENT,
                                    p.ORDRE,
                                    p.COMPTEUR,
                                    p.DATEEVT,
                                    p.PERIODE,
                                    p.CODEEVT,
                                    p.INDEXEVT,
                                    p.CAS,
                                    p.ENQUETE,
                                    p.CONSO,
                                    p.CONSONONFACTUREE,
                                    p.LOTRI,
                                    p.FACTURE,
                                    p.SURFACTURATION,
                                    p.STATUS,
                                    p.TYPECONSO,
                                    p.REGLAGECOMPTEUR,
                                    p.TYPETARIF,
                                    p.FORFAIT,
                                    p.CATEGORIE,
                                    p.CODECONSO,
                                    p.PROPRIO,
                                    p.MODEPAIEMENT,
                                    p.MATRICULE,
                                    p.FACPER,
                                    p.QTEAREG,
                                    p.DERPERF,
                                    p.DERPERFN,
                                    p.CONSOFAC,
                                    p.REGIMPUTE,
                                    p.REGCONSO,
                                    p.COEFLECT,
                                    p.COEFCOMPTAGE,
                                    p.PUISSANCE,
                                    p.TYPECOMPTAGE,
                                    p.TYPECOMPTEUR,
                                    LIBELLETYPECOMPTEUR = p.COMPTEUR1.TYPECOMPTEUR1.LIBELLE,
                                    p.COEFK1,
                                    p.COEFK2,
                                    p.COEFFAC,
                                    p.USERCREATION,
                                    p.DATECREATION,
                                    p.DATEMODIFICATION,
                                    p.USERMODIFICATION,
                                    p.PK_ID,
                                    p.FK_IDCANALISATION,
                                    p.FK_IDABON,
                                    p.FK_IDCOMPTEUR,
                                    p.FK_IDCENTRE,
                                    p.FK_IDPRODUIT,
                                    p.ESTCONSORELEVEE,
                                    p.COMMENTAIRE,
                                    p.FK_IDTOURNEE,
                                    p.TOURNEE,
                                    p.ORDTOUR,
                                    p.PERFAC,
                                    p.CONSOMOYENNEPRECEDENTEFACTURE,
                                    p.DATERELEVEPRECEDENTEFACTURE,
                                    p.CASPRECEDENTEFACTURE,
                                    p.INDEXPRECEDENTEFACTURE,
                                    p.PERIODEPRECEDENTEFACTURE,
                                    p.ORDREAFFICHAGE,
                                    NOMABON = p.CANALISATION.ABON.CLIENT1.NOMABON,
                                    RELEVEUR = t.RELEVEUR.ADMUTILISATEUR.LIBELLE,
                                    CADRAN = p.CANALISATION.COMPTEUR.CADRAN,
                                    RUE = p.CANALISATION.ABON.CLIENT1.AG.RUE,
                                    PORTE = p.CANALISATION.ABON.CLIENT1.AG.PORTE,
                                    COMPTEURAFFICHER = p.COMPTEUR
                                });
                    return Galatee.Tools.Utility.ListToDataTable<object>(page);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable ChargerListeDesTransfertsp(CsLotri _lstLori)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    var Transfer = context.TRANSFERT ;
                    var trans = (from p in Transfer
                                   where
                                   p.LOTRI == _lstLori.NUMLOTRI && p.FK_IDCENTRE == _lstLori.FK_IDCENTRE 
                                   select new
                                   {
                                       p.EVENEMENT.CENTRE,
                                       p.EVENEMENT.CLIENT,
                                       p.EVENEMENT.PRODUIT,
                                       p.EVENEMENT.POINT,
                                       p.EVENEMENT.ABON.CLIENT1.NOMABON ,
                                       p.EVENEMENT.NUMEVENEMENT,
                                       p.EVENEMENT.ORDRE,
                                       p.COMPTEUR,
                                       p.DATEEVT,
                                       p.EVENEMENT.PERIODE,
                                       p.EVENEMENT.CODEEVT,
                                       p.INDEXEVT,
                                       p.CAS,
                                       p.ENQUETE,
                                       p.CONSO,
                                       p.EVENEMENT.CONSONONFACTUREE,
                                       p.EVENEMENT.LOTRI,
                                       p.EVENEMENT.FACTURE,
                                       p.EVENEMENT.SURFACTURATION,
                                       p.STATUS,
                                       p.EVENEMENT.TYPECONSO,
                                       p.EVENEMENT.REGLAGECOMPTEUR,
                                       p.EVENEMENT.TYPETARIF,
                                       p.EVENEMENT.FORFAIT,
                                       p.EVENEMENT.CATEGORIE,
                                       p.EVENEMENT.CODECONSO,
                                       p.EVENEMENT.PROPRIO,
                                       p.EVENEMENT.MODEPAIEMENT,
                                       p.EVENEMENT.MATRICULE,
                                       p.EVENEMENT.FACPER,
                                       p.EVENEMENT.QTEAREG,
                                       p.EVENEMENT.DERPERF,
                                       p.EVENEMENT.DERPERFN,
                                       p.EVENEMENT.CONSOFAC,
                                       p.EVENEMENT.REGIMPUTE,
                                       p.EVENEMENT.REGCONSO,
                                       p.EVENEMENT.COEFLECT,
                                       p.EVENEMENT.COEFCOMPTAGE,
                                       p.EVENEMENT.PUISSANCE,
                                       p.EVENEMENT.TYPECOMPTAGE,
                                       p.EVENEMENT.TYPECOMPTEUR,
                                       p.EVENEMENT.COEFK1,
                                       p.EVENEMENT.COEFK2,
                                       p.EVENEMENT.COEFFAC,
                                       p.EVENEMENT.USERCREATION,
                                       p.EVENEMENT.DATECREATION,
                                       p.EVENEMENT.DATEMODIFICATION,
                                       p.EVENEMENT.USERMODIFICATION,
                                       p.EVENEMENT.PK_ID,
                                       p.EVENEMENT.FK_IDCANALISATION,
                                       p.EVENEMENT.FK_IDABON,
                                       p.EVENEMENT.FK_IDCOMPTEUR,
                                       p.EVENEMENT.FK_IDCENTRE,
                                       p.EVENEMENT.FK_IDPRODUIT,
                                       p.EVENEMENT.ESTCONSORELEVEE,
                                       p.EVENEMENT.COMMENTAIRE,
                                       p.EVENEMENT.NOUVEAUCOMPTEUR,
                                       p.EVENEMENT.NOUVEAUCADRAN ,
                                       REFERENCE = p.EVENEMENT.CENTRE + " " + p.EVENEMENT.CLIENT + " " + p.EVENEMENT .ORDRE 
                                   });
                    return Galatee.Tools.Utility.ListToDataTable<object>(trans);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static DataTable ChargerDistinctLotTsp(List<int> isCentre)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    var Transfer = context.TRANSFERT ;
                    var trans = (from p in Transfer
                                   where
                                 isCentre.Contains(p.FK_IDCENTRE)
                                   select new
                                   {
                                       p.CENTRE ,
                                       p.FK_IDCENTRE ,
                                       NUMLOTRI=   p.LOTRI 
                                   }).Distinct();
                    return Galatee.Tools.Utility.ListToDataTable<object>(trans);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static DataTable RetourneListeEvenementDuLotPourSaisiIndexTSP(CsLotri leLotri, string sequence)
        {
            try
            {
                List<int?> StatusIndex = new List<int?>() { 3, 4, 99 };

                using (galadbEntities context = new galadbEntities())
                {
                    var PagerieClient = context.EVENEMENT ;
                    var pagerie = (from p in PagerieClient
                                   from y in p.TOURNEE1.TOURNEERELEVEUR
                                   where
                                   new int[] { Enumere.EvenementCree }.Contains(p.STATUS.Value) &&
                                   !(new string[] { Enumere.CasDeposeCompteur, Enumere.CasPoseCompteur }.Contains(p.CAS))
                                    && p.LOTRI == leLotri.NUMLOTRI
                                    && (p.TOURNEE == leLotri.TOURNEE || string.IsNullOrEmpty(leLotri.TOURNEE))
                                    && (p.ORDTOUR == sequence || string.IsNullOrEmpty(sequence))
                                    && !context.TRANSFERT.Any(t => t.PK_ID  == p.PK_ID)
                                   select new
                                   {
                                       p.CENTRE,
                                       p.CLIENT,
                                       p.POINT,
                                       NOMABON = p.CANALISATION.ABON.CLIENT1.NOMABON,
                                       p.NUMEVENEMENT,
                                       p.COMPTEUR,
                                       p.TOURNEE,
                                       //NOMPIA = m.LIBELLE,***
                                       PERIODE = leLotri.PERIODE,
                                       p.PRODUIT,
                                       LOTRI = p.LOTRI,
                                       p.FACTURE,
                                       p.SURFACTURATION,
                                       p.STATUS,
                                       p.TYPECONSO,
                                       p.REGLAGECOMPTEUR,
                                       p.FACPER,
                                       p.QTEAREG,
                                       p.DERPERF,
                                       p.DERPERFN,
                                       p.CONSOFAC,
                                       p.REGIMPUTE,
                                       p.REGCONSO,
                                       p.COEFLECT,
                                       p.COEFCOMPTAGE,
                                       p.PUISSANCE,
                                       p.TYPECOMPTAGE,
                                       p.TYPECOMPTEUR,
                                       p.COEFK1,
                                       p.COEFK2,
                                       p.COEFFAC,
                                       p.ORDRE,
                                       //INDEXEVTPRECEDENT = ev.INDEXEVT,
                                       //CASPRECEDENT = ev.CAS,
                                       //CONSOPRECEDENT = ev.CONSO,
                                       //CONSOFACPRECEDENT = ev.CONSOFAC,
                                       p.ORDTOUR,
                                       //_abon.DRES,****
                                       //cl.NOMABON,***
                                       p.PK_ID  ,
                                       p.FK_IDCANALISATION,
                                       p.FK_IDCENTRE,
                                       p.FK_IDPRODUIT,
                                       RELEVEUR = y.RELEVEUR.ADMUTILISATEUR.LIBELLE,
                                       MATRICULE = y.RELEVEUR.ADMUTILISATEUR.MATRICULE ,
                                       CADRAN = p.CANALISATION.COMPTEUR.CADRAN,
                                       RUE = p.CANALISATION.ABON.CLIENT1.AG.RUE,
                                       PORTE = p.CANALISATION.ABON.CLIENT1.AG.PORTE,
                                   });
                    var distinctClient = (from d in pagerie select new { d.CENTRE, d.CLIENT, d.ORDRE, d.POINT, d.PRODUIT }).Distinct();


                    //var historiqueFacture = from d in distinctClient
                    //                        join ev in context.HISTORIQUE  on new { d.CENTRE, d.CLIENT, d.ORDRE, d.POINT, d.PRODUIT }
                    //                                                     equals new { ev.CENTRE, ev.CLIENT, ev.ORDRE, ev.POINT, ev.PRODUIT }
                    //                        group new { d, ev } by new { d.CENTRE, d.CLIENT, d.ORDRE, d.POINT, d.PRODUIT } into pJoin
                    //                        select new
                    //                        {
                    //                            pJoin.Key.CENTRE,
                    //                            pJoin.Key.CLIENT,
                    //                            pJoin.Key.ORDRE,
                    //                            pJoin.Key.PRODUIT,
                    //                            pJoin.Key.POINT,
                    //                            CumConso =  (pJoin.Sum(o => o.ev.CONSOFAC * o.ev.NBREJOURFACTURE) / Enumere.NombreDejour),
                    //                            nombrePeriode = pJoin.Count()

                    //                        };



                    var MaxEvenemtFacture = from d in distinctClient
                                            join ev in context.EVENEMENT on new { d.CENTRE, d.CLIENT, d.ORDRE, d.POINT, d.PRODUIT }
                                                                         equals new { ev.CENTRE, ev.CLIENT, ev.ORDRE, ev.POINT, ev.PRODUIT }
                                            where StatusIndex.Contains(ev.STATUS) // confusion avec la procédure stockée corresp
                                            group new { d, ev } by new { d.CENTRE, d.CLIENT, d.ORDRE, d.POINT, d.PRODUIT } into pJoin
                                            select new
                                            {
                                                pJoin.Key.CENTRE,
                                                pJoin.Key.CLIENT,
                                                pJoin.Key.ORDRE,
                                                pJoin.Key.PRODUIT,
                                                pJoin.Key.POINT,
                                                MaxEvenement = pJoin.Max(o => o.ev.NUMEVENEMENT)
                                            };

                    var Anclient = from a in MaxEvenemtFacture
                                   join ev in context.EVENEMENT on new { a.CENTRE, a.CLIENT, a.ORDRE, a.POINT, a.PRODUIT }
                                                                  equals new { ev.CENTRE, ev.CLIENT, ev.ORDRE, ev.POINT, ev.PRODUIT }
                                   where ev.NUMEVENEMENT == a.MaxEvenement
                                   select new
                                   {
                                       a.CENTRE,
                                       a.CLIENT,
                                       a.ORDRE,
                                       a.PRODUIT,
                                       a.POINT,
                                       a.MaxEvenement,
                                       DANCIENINDEX = ev.INDEXEVT,
                                       ANCIENCAS = ev.CAS,
                                       ev.CONSO,
                                       ev.CONSOFAC,
                                       ev.DATEEVT,
                                       ev.QTEAREG
                                   };

                    var query2 = from p in pagerie
                                 join an in Anclient on new { p.CENTRE, p.CLIENT, p.ORDRE, p.POINT, p.PRODUIT }
                                                     equals new { an.CENTRE, an.CLIENT, an.ORDRE, an.POINT, an.PRODUIT }
                                 //join hs in historiqueFacture on new { p.CENTRE, p.CLIENT, p.ORDRE, p.POINT, p.PRODUIT }
                                 //                    equals new { hs.CENTRE, hs.CLIENT, hs.ORDRE, hs.POINT, hs.PRODUIT }
                                 select new
                                 {
                                     p.CENTRE,
                                     p.CLIENT,
                                     p.POINT,
                                     p.NOMABON,
                                     p.NUMEVENEMENT,
                                     p.COMPTEUR,
                                     p.TOURNEE,
                                     //NOMPIA = m.LIBELLE,***
                                     PERIODE = leLotri.PERIODE,
                                     p.PRODUIT,
                                     p.LOTRI,
                                     p.FACTURE,
                                     p.SURFACTURATION,
                                     p.STATUS,
                                     p.TYPECONSO,
                                     p.REGLAGECOMPTEUR ,
                                     p.MATRICULE,
                                     p.FACPER,
                                     p.QTEAREG,
                                     p.DERPERF,
                                     p.DERPERFN,
                                     p.CONSOFAC,
                                     p.REGIMPUTE,
                                     p.REGCONSO,
                                     p.COEFLECT,
                                     p.COEFCOMPTAGE,
                                     p.PUISSANCE,
                                     p.TYPECOMPTAGE,
                                     p.TYPECOMPTEUR,
                                     p.COEFK1,
                                     p.COEFK2,
                                     p.COEFFAC,
                                     p.ORDRE,
                                     INDEXEVTPRECEDENT = an.DANCIENINDEX,
                                     CASPRECEDENT = an.ANCIENCAS,
                                     CONSOPRECEDENT = an.CONSO,
                                     CONSOFACPRECEDENT = an.CONSOFAC,
                                     p.ORDTOUR,
                                     p.PK_ID,
                                     p.FK_IDCANALISATION,
                                     p.FK_IDCENTRE,
                                     p.FK_IDPRODUIT,
                                     p.RELEVEUR,
                                     p.CADRAN,
                                     p.RUE,
                                     p.PORTE,
                                     //CONSOMOYENNE = hs.CumConso/hs.nombrePeriode  
                                 };
                    return Galatee.Tools.Utility.ListToDataTable<object>(query2);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //public static DataTable RetourneListeEvenementDuClientPourSaisiIndex(string lotri, string centre, string pclient, string ordre, string produit, int point)
        //{
        //    try
        //    {
        //        using (galadbEntities context = new galadbEntities())
        //        {
        //            IEnumerable<CLIENT> client;
        //            if (string.IsNullOrEmpty(ordre))
        //                client = context.CLIENT.Where(c => c.CENTRE == centre && c.REFCLIENT == pclient);
        //            else
        //                client = context.CLIENT.Where(c => c.CENTRE == centre && c.REFCLIENT == pclient && c.ORDRE == ordre);

        //            var abonNonRes = context.ABON.Where(a => a.DRES == null);
        //            IEnumerable<EVENEMENT> eventValide;
        //            IEnumerable<EVENEMENT> eventNonValide;
        //            if (string.IsNullOrEmpty(lotri))
        //            {
        //                eventValide = context.EVENEMENT.Where(e => (new int[] { Enumere.EvenementFacture, Enumere.EvenementMisAJour, Enumere.EtatEvenementPurge, Enumere.EvenementReleve }.Contains(e.STATUS.Value)) &&
        //                                                            !(new string[] { Enumere.CasDeposeCompteur, Enumere.CasPoseCompteur }.Contains(e.CAS)));

        //                eventNonValide = context.EVENEMENT.Where(e => (e.STATUS < 3 || e.STATUS == Enumere.EvenementDefacture) && // < 3 sont les evenemnts non encore factures ou ceux defactures
        //                                                               !(new string[] { Enumere.CasDeposeCompteur, Enumere.CasPoseCompteur }.Contains(e.CAS)) && e.CENTRE == centre &&
        //                                                               e.CLIENT == pclient && e.PRODUIT == produit && e.POINT == point);

        //            }
        //            else
        //            {
        //                eventValide = context.EVENEMENT.Where(e => (new int[] { Enumere.EvenementFacture, Enumere.EvenementMisAJour, Enumere.EtatEvenementPurge, Enumere.EvenementReleve }.Contains(e.STATUS.Value)) &&
        //                                                          !(new string[] { Enumere.CasDeposeCompteur, Enumere.CasPoseCompteur }.Contains(e.CAS)) && e.LOTRI == lotri);

        //                eventNonValide = context.EVENEMENT.Where(e => (e.STATUS < 3 || e.STATUS == Enumere.EvenementDefacture) && // < 3 sont les evenemnts non encore factures ou ceux defactures
        //                                                               !(new string[] { Enumere.CasDeposeCompteur, Enumere.CasPoseCompteur }.Contains(e.CAS)) && e.CENTRE == centre &&
        //                                                               e.CLIENT == pclient && e.PRODUIT == produit && e.POINT == point && e.LOTRI == lotri);
        //            }

        //            // ramener les lignes des clients par point de fourniture dont le numevenement est le max et dont la facturation est MAJ,PURGE,ou FACTURE
        //            var maxevent = from pg in eventValide
        //                           where pg.CENTRE == centre && pg.CLIENT == pclient && pg.PRODUIT == produit && pg.POINT == point
        //                           group new { pg } by new { pg.CENTRE, pg.CLIENT, pg.POINT, pg.PRODUIT } into pJ
        //                           select new
        //                           {
        //                               pJ.Key.CENTRE,
        //                               pJ.Key.CLIENT,
        //                               pJ.Key.PRODUIT,
        //                               pJ.Key.POINT,
        //                               MAXEVENMENT = pJ.Max(o => o.pg.NUMEVENEMENT)
        //                           };

        //            var query2 = (from q in maxevent
        //                          join ev in context.EVENEMENT on new { q.CENTRE, q.CLIENT, q.PRODUIT, q.POINT, NUMEVENEMENT = q.MAXEVENMENT }
        //                                                       equals new { ev.CENTRE, ev.CLIENT, ev.PRODUIT, ev.POINT, ev.NUMEVENEMENT }
        //                          join pg in context.PAGERI on new { q.CENTRE, q.CLIENT, q.PRODUIT, q.POINT }
        //                                                    equals new { pg.CENTRE, pg.CLIENT, pg.PRODUIT, pg.POINT }
        //                          join evn in eventNonValide on new { pg.CENTRE, pg.CLIENT, pg.PRODUIT, pg.POINT, pg.LOTRI }
        //                                                     equals new { evn.CENTRE, evn.CLIENT, evn.PRODUIT, evn.POINT, evn.LOTRI }
        //                          join cl in client on new { q.CLIENT, q.CENTRE }
        //                                            equals new { CLIENT = cl.REFCLIENT, cl.CENTRE }
        //                          join _abon in abonNonRes on cl.PK_ID equals _abon.FK_IDCLIENT
        //                          join t in context.TOURNEE on new { pg.TOURNEE, pg.CENTRE }
        //                                                    equals new { TOURNEE = t.CODE , t.CENTRE }
        //                          join m in context.ADMUTILISATEUR on t.MATRICULEPIA equals m.MATRICULE
        //                          select new
        //                          {
        //                              q.CENTRE,
        //                              q.CLIENT,
        //                              q.POINT,
        //                              NOMCLIENT = cl.NOMABON,
        //                              //ev.NUMEVENEMENT,
        //                              evn.COMPTEUR,
        //                              pg.TOURNEE,
        //                              NOMPIA = m.LIBELLE,
        //                              evn.DATEEVT,
        //                              PERIODEPRECEDENT = ev.PERIODE,
        //                              evn.PERIODE,
        //                              evn.CODEEVT,
        //                              evn.INDEXEVT,
        //                              evn.CAS,
        //                              evn.ENQUETE,
        //                              evn.PRODUIT,
        //                              evn.NUMEVENEMENT,
        //                              evn.LOTRI,
        //                              evn.FACTURE,
        //                              evn.SURFACTURATION,
        //                              evn.STATUS,
        //                              evn.TYPECONSO,
        //                              evn.DIAMETRE,
        //                              evn.MATRICULE,
        //                              evn.FACPER,
        //                              evn.QTEAREG,
        //                              evn.DERPERF,
        //                              evn.DERPERFN,
        //                              evn.CONSOFAC,
        //                              evn.REGIMPUTE,
        //                              evn.REGCONSO,
        //                              evn.COEFLECT,
        //                              evn.COEFCOMPTAGE,
        //                              evn.PUISSANCE,
        //                              evn.TYPECOMPTAGE,
        //                              evn.TYPECOMPTEUR,
        //                              evn.COEFK1,
        //                              evn.COEFK2,
        //                              evn.COEFFAC,
        //                              evn.ORDRE,
        //                              INDEXEVTPRECEDENT = ev.INDEXEVT,
        //                              CASPRECEDENT = ev.CAS,
        //                              CONSOPRECEDENT = ev.CONSO,
        //                              CONSOFACPRECEDENT = ev.CONSOFAC,
        //                              //pg.TOURNEE,
        //                              pg.ORDTOUR,
        //                              _abon.DRES,
        //                              cl.NOMABON,
        //                              FK_IDPAGERI = pg.PK_ID,
        //                              evn.PK_ID
        //                          }).Distinct();

        //            return Galatee.Tools.Utility.ListToDataTable<object>(query2);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public static CsSaisiIndexIndividuel RetourneListeEvenementDuClientPourSaisiIndex2(string lotri, int Fk_Idcentre, string centre, string pclient, string ordre, string produit, int point)
        {
            try
            {
                DataTable dt = RetourneTousEvenemntsDuClient(Fk_Idcentre, centre, pclient, ordre, produit, point);
                List<CsEvenement> EventsClient = Entities.GetEntityListFromQuery<CsEvenement>(dt);
                List<CsEvenement> EventNonFactureClient = RetourneEvenementNonFacturesClient(lotri, EventsClient);

                if (EventNonFactureClient == null || EventNonFactureClient.Count == 0)
                    return null;

                List<CsEvenement> EvenementAvecLotriNull = EventNonFactureClient.Where(e => string.IsNullOrEmpty(e.LOTRI)).ToList();
                List<CsEvenement> EvenementFactureIsole = EventNonFactureClient.Where(e => new string[] { Enumere.FactureAnnulatinIndex, Enumere.FactureIsoleIndex, Enumere.FactureResiliationIndex }.Contains(e.LOTRI.Substring(Enumere.TailleCentre))).ToList();
                CsEvenement InfoConsommationDerniereFacturation = new CsEvenement();

                List<CsEvenement> LstEvenementDernEvt = EventsClient.Where(p => p.POINT == point &&
                                                                    (p.STATUS == Enumere.EvenementFacture ||
                                                                     p.STATUS == Enumere.EvenementMisAJour ||
                                                                     p.STATUS == Enumere.EvenementPurger)).ToList();
                if (LstEvenementDernEvt != null && LstEvenementDernEvt.Count != 0)
                {
                    InfoConsommationDerniereFacturation = LstEvenementDernEvt.FirstOrDefault(p => p.NUMEVENEMENT == LstEvenementDernEvt.Max(c => c.NUMEVENEMENT));
                }
                else
                    InfoConsommationDerniereFacturation = EventsClient.FirstOrDefault(p => p.CAS == Enumere.CasPoseCompteur);


                // récupérer les events qui ont une ligne dans pageri
                List<CsEvenement> EvenementFacturePageri = new List<CsEvenement>();
                foreach (var item in EventNonFactureClient)
                {
                    if (!EvenementAvecLotriNull.Contains(item) && !EvenementFactureIsole.Contains(item))
                        EvenementFacturePageri.Add(item);
                }

                List<CsEvenement> ElementPageri = EvenementFacturePageri;
                List<CsEvenement> ElementPagisol = EvenementFactureIsole;
                CsEvenement ElementDernierEvtFac = InfoConsommationDerniereFacturation;
                List<CsEvenement> EventLotriNull = EvenementAvecLotriNull;

                CsSaisiIndexIndividuel saisi = new CsSaisiIndexIndividuel();
                saisi.ConsoPrecedent.Add(ElementDernierEvtFac);
                saisi.EventLotriNull = EventLotriNull;
                saisi.EventPageri = ElementPageri;
                saisi.EventPagisol = ElementPagisol;

                return saisi;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static CsEvenement  RetourneListeEvenementDuClientFacture(int Fk_idcentre,string centre, string pclient, string ordre, string produit, int point)
        {
            try
            {
                DataTable dt = RetourneTousEvenemntsDuClient(Fk_idcentre, centre, pclient, ordre, produit, point);
                List<CsEvenement> EventsClient = Entities.GetEntityListFromQuery<CsEvenement>(dt);

                if (EventsClient == null || EventsClient.Count == 0)
                    return null ;

                CsEvenement InfoConsommationDerniereFacturation = new CsEvenement();
                List<CsEvenement> LstEvenementDernEvt = EventsClient.Where(p => p.POINT == point &&
                                                                    (p.STATUS == Enumere.EvenementFacture ||
                                                                     p.STATUS == Enumere.EvenementMisAJour ||
                                                                     p.STATUS == Enumere.EvenementPurger)).ToList();
                if (LstEvenementDernEvt != null && LstEvenementDernEvt.Count != 0)
                {
                    InfoConsommationDerniereFacturation = LstEvenementDernEvt.FirstOrDefault(p => p.NUMEVENEMENT == LstEvenementDernEvt.Max(c => c.NUMEVENEMENT));
                }
                else
                    InfoConsommationDerniereFacturation = EventsClient.FirstOrDefault(p => p.CAS == Enumere.CasPoseCompteur);

                if (InfoConsommationDerniereFacturation != null )
                {
                    CsEvenement dernEvt = new CsEvenement()
                    {
                        CASPRECEDENTEFACTURE = InfoConsommationDerniereFacturation.CAS,
                        CONSOFACPRECEDENT = InfoConsommationDerniereFacturation.CONSOFAC,
                        INDEXPRECEDENTEFACTURE = InfoConsommationDerniereFacturation.INDEXEVT
                    };
                    return dernEvt;
                }
                else
                    return null ; ;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        ////private static List<CsEvenement> ConvertionEvenementNull(List<EVENEMENT> EvenementAvecLotriNull)
        ////{
        ////    try
        ////    {
        ////        List<CsEvenement> events = new List<CsEvenement>();
        ////        foreach (var item in EvenementAvecLotriNull)
        ////            events.Add(new CsEvenement()
        ////            {

        ////                CONSO = item.CONSO,
        ////                CONSOFAC = item.CONSOFAC,
        ////                INDEXEVT = item.INDEXEVT,
        ////                DATEEVT = item.DATEEVT,
        ////                COMPTEUR = item.COMPTEUR,
        ////                ENQUETE = item.ENQUETE,
        ////                CASEVENEMENT = item.CAS,
        ////                CLIENT = item.CLIENT,
        ////                CENTRE = item.CENTRE,
        ////                PRODUIT = item.PRODUIT,
        ////                POINT = item.POINT,
        ////                LOTRI = item.LOTRI,
        ////                PERIODE = item.PERIODE,
        ////                PK_ID  = item.PK_ID,
        ////                NUMEVENEMENT = item.NUMEVENEMENT,
        ////                IsFromEvennt = true
        ////            });

        ////        return events;
        ////    }
        ////    catch (Exception ex)
        ////    {
        ////        throw ex;
        ////    }
        ////}

        private static List<CsEvenement> ConvertionConsommation(List<EVENEMENT> InfoConsommationDerniereFacturation)
        {
            try
            {
                List<CsEvenement> events = new List<CsEvenement>();
                foreach (var item in InfoConsommationDerniereFacturation)
                    events.Add(new CsEvenement()
                    {

                        CONSOFACPRECEDENT = item.CONSOFAC,
                        CONSOPRECEDENT = item.CONSO,
                        INDEXPRECEDENTEFACTURE = item.INDEXEVT,
                        CASPRECEDENTEFACTURE = item.CAS,
                        CLIENT = item.CLIENT,
                        CENTRE = item.CENTRE,
                        ORDRE = item.ORDRE,
                        PRODUIT = item.PRODUIT,
                        POINT = item.POINT,
                        LOTRI = item.LOTRI,
                        PERIODEPRECEDENT = item.PERIODE,
                        //FK_IDCAS = item.FK_IDCAS,
                        COEFLECT = item.COEFLECT,
                        COMPTEUR = item.COMPTEUR

                    });

                return events;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        static List<CsEvenement> RetourneNumeroDernierEvenemntFactureDuClient(string lotri, string centre, string pclient, string ordre, string produit, int point)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<EVENEMENT> eventValide;
                    if (string.IsNullOrEmpty(lotri))
                        eventValide = context.EVENEMENT.Where(e => (new int[] { Enumere.EvenementFacture, Enumere.EvenementMisAJour, Enumere.EtatEvenementPurge, Enumere.EvenementReleve }.Contains(e.STATUS.Value)) &&
                                                                    !(new string[] { Enumere.CasDeposeCompteur, Enumere.CasPoseCompteur }.Contains(e.CAS)) && e.CLIENT == pclient &&
                                                                    e.CENTRE == centre && e.PRODUIT == produit && e.POINT == point);
                    else
                        eventValide = context.EVENEMENT.Where(e => (new int[] { Enumere.EvenementFacture, Enumere.EvenementMisAJour, Enumere.EtatEvenementPurge, Enumere.EvenementReleve }.Contains(e.STATUS.Value)) &&
                                                                  !(new string[] { Enumere.CasDeposeCompteur, Enumere.CasPoseCompteur }.Contains(e.CAS)) && e.LOTRI == lotri && e.CLIENT == pclient &&
                                                                    e.CENTRE == centre && e.PRODUIT == produit && e.POINT == point);

                    // ramener les lignes des clients par point de fourniture dont le numevenement est le max et dont la facturation est MAJ,PURGE,ou FACTURE
                    IEnumerable<CsEvenement> maxevent = from pg in eventValide
                                                        group new { pg } by new { pg.CENTRE, pg.CLIENT, pg.POINT, pg.PRODUIT } into pJ
                                                        select new CsEvenement
                                                        {
                                                            CENTRE = pJ.Key.CENTRE,
                                                            CLIENT = pJ.Key.CLIENT,
                                                            PRODUIT = pJ.Key.PRODUIT,
                                                            POINT = pJ.Key.POINT,
                                                            NUMEVENEMENT = pJ.Max(o => o.pg.NUMEVENEMENT)
                                                        };

                    return maxevent.ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        static DataTable RetourneEvenementNonFacturesDuClient(string lotri, List<EVENEMENT> _events)
        {
            try
            {
                // ramener la conso ,index de la dernier evenemnt facturé
                IEnumerable<CsEvenement> eventFactureInfo = Tools.Utility.GetEntityListFromQuery<CsEvenement>(RetourneInfoElemensDejaFacture(_events, lotri));

                // ramener la liste ds évenemnets non facturés ou qui satisfont à certains critères
                IEnumerable<EVENEMENT> eventNonFacture;
                if (string.IsNullOrEmpty(lotri))
                    eventNonFacture = _events.Where(e => (e.STATUS < 3 || e.STATUS == Enumere.EvenementDefacture) && // < 3 sont les evenemnts non encore factures ou ceux defactures
                                                                  !(new string[] { Enumere.CasDeposeCompteur, Enumere.CasPoseCompteur }.Contains(e.CAS)));
                else
                    eventNonFacture = _events.Where(e => (e.STATUS < 3 || e.STATUS == Enumere.EvenementDefacture) && // < 3 sont les evenemnts non encore factures ou ceux defactures
                                                                  !(new string[] { Enumere.CasDeposeCompteur, Enumere.CasPoseCompteur }.Contains(e.CAS)) && e.LOTRI == lotri);

                // ramener les infos de la pageri correspondant au info des évenements non facturés ci dessus
                List<CsEvenement> pageriEvent = Tools.Utility.GetEntityListFromQuery<CsEvenement>(RetourneInfoPageriElementNonFacture(eventNonFacture));

                // enfin ramenre les elements du précédent évenemnt facture et les evements non facture du client
                var query2 = from infofac in eventFactureInfo
                             join infopageri in pageriEvent on new { infofac.CENTRE, infofac.CLIENT, infofac.PRODUIT, infofac.POINT }
                                                            equals new { infopageri.CENTRE, infopageri.CLIENT, infopageri.PRODUIT, infopageri.POINT }

                             select new
                             {

                                 infofac.CENTRE,
                                 infofac.CLIENT,
                                 infofac.POINT,
                                 //infofac.PRODUIT,
                                 //infofac.DATEEVT,
                                 infofac.PERIODEPRECEDENT,
                                 infofac.INDEXPRECEDENTEFACTURE,
                                 //infofac.CASPRECEDENT,
                                 infofac.CONSOPRECEDENT,
                                 infofac.CONSOFACPRECEDENT,

                                 infopageri.COMPTEUR,
                                 infopageri.LOTRI,
                                 infopageri.NOMABON,
                                 //infopageri.TOURNEE,
                                 infopageri.NOMPIA,
                                 infopageri.DATEEVT,
                                 //PERIODEPRECEDENT = ev.PERIODE,
                                 infopageri.PERIODE,
                                 infopageri.CODEEVT,
                                 infopageri.INDEXEVT,
                                 infopageri.CAS,
                                 infopageri.ENQUETE,
                                 infopageri.PRODUIT,
                                 infopageri.NUMEVENEMENT,
                                 infopageri.FACTURE,
                                 infopageri.SURFACTURATION,
                                 infopageri.STATUS,
                                 infopageri.TYPECONSO,
                                 infopageri.REGLAGECOMPTEUR,
                                 infopageri.MATRICULE,
                                 infopageri.FACPER,
                                 //evn.QTEAREG,
                                 //evn.DERPERF,
                                 //evn.DERPERFN,
                                 infopageri.CONSOFAC,
                                 //evn.REGIMPUTE,
                                 //evn.REGCONSO,
                                 //evn.COEFLECT,
                                 //evn.COEFCOMPTAGE,
                                 //evn.PUISSANCE,
                                 //evn.TYPECOMPTAGE,
                                 //evn.TCOMPT,
                                 //evn.COEFK1,
                                 //evn.COEFK2,
                                 //evn.COEFFAC,
                                 //evn.ORDRE,
                                 //INDEXEVTPRECEDENT = ev.INDEXEVT,
                                 //CASPRECEDENT = ev.CAS,
                                 //CONSOPRECEDENT = ev.CONSO,
                                 //CONSOFACPRECEDENT = ev.CONSOFAC,
                                 //pg.TOURNEE,
                                 infopageri.ORDTOUR,
                                 infopageri.FK_IDPAGERI,
                                 infopageri.PK_ID 
                             };
                return Tools.Utility.ListToDataTable<object>(query2);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        static List<CsEvenement> RetourneEvenementNonFacturesClient(string lotri, List<CsEvenement> _events)
        {
            try
            {
                // ramener la liste ds évenemnets non facturés ou qui satisfont à certains critères
                List<CsEvenement> eventNonFacture;
                if (string.IsNullOrEmpty(lotri))
                    eventNonFacture = _events.Where(e => (e.STATUS < 3 || e.STATUS == Enumere.EvenementDefacture) && // < 3 sont les evenemnts non encore factures ou ceux defactures
                                                                  !(new string[] { Enumere.CasDeposeCompteur, Enumere.CasPoseCompteur }.Contains(e.CAS))).ToList();
                else
                    eventNonFacture = _events.Where(e => (e.STATUS < 3 || e.STATUS == Enumere.EvenementDefacture) && // < 3 sont les evenemnts non encore factures ou ceux defactures
                                                                  !(new string[] { Enumere.CasDeposeCompteur, Enumere.CasPoseCompteur }.Contains(e.CAS)) && e.LOTRI == lotri).ToList();

                return eventNonFacture.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        static List<EVENEMENT> RetourneEvenementFacturesClient( List<EVENEMENT> _events)
        {
            try
            {
                List<int?> StatusIndex = new List<int?>() { 3, 4, 99 };
                // ramener la liste ds évenemnets non facturés ou qui satisfont à certains critères
                IEnumerable<EVENEMENT> eventFacture;
                eventFacture = _events.Where(p => !StatusIndex.Contains(p.STATUS));
                return eventFacture.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        static DataTable RetourneInfoElemensDejaFacture(IEnumerable<EVENEMENT> _events, string lotri)
        {
            try
            {

                IEnumerable<EVENEMENT> eventValide;
                if (string.IsNullOrEmpty(lotri))
                    eventValide = _events.Where(e => (new int[] { Enumere.EvenementFacture, Enumere.EvenementMisAJour, Enumere.EtatEvenementPurge, Enumere.EvenementReleve }.Contains(e.STATUS.Value)) &&
                                                                !(new string[] { Enumere.CasDeposeCompteur, Enumere.CasPoseCompteur }.Contains(e.CAS)));
                else
                    eventValide = _events.Where(e => (new int[] { Enumere.EvenementFacture, Enumere.EvenementMisAJour, Enumere.EtatEvenementPurge, Enumere.EvenementReleve }.Contains(e.STATUS.Value)) &&
                                                              !(new string[] { Enumere.CasDeposeCompteur, Enumere.CasPoseCompteur }.Contains(e.CAS)) && e.LOTRI == lotri);

                // ramener les lignes des clients par point de fourniture dont le numevenement est le max et dont la facturation est MAJ,PURGE,ou FACTURE
                IEnumerable<CsEvenement> maxevent = from pg in eventValide
                                                    group new { pg } by new { pg.CENTRE, pg.CLIENT, pg.POINT, pg.PRODUIT } into pJ
                                                    select new CsEvenement
                                                    {
                                                        CENTRE = pJ.Key.CENTRE,
                                                        CLIENT = pJ.Key.CLIENT,
                                                        PRODUIT = pJ.Key.PRODUIT,
                                                        POINT = pJ.Key.POINT,
                                                        NUMEVENEMENT = pJ.Max(o => o.pg.NUMEVENEMENT)
                                                    };

                var query1 = from ev in maxevent      // ramener les éléments constituants le dernier évenemnt facturé
                             join
                                 e in _events on new { ev.CENTRE, ev.CLIENT, ev.PRODUIT, ev.POINT, ev.NUMEVENEMENT }
                                 equals new { e.CENTRE, e.CLIENT, e.PRODUIT, e.POINT, e.NUMEVENEMENT }
                             select new
                             {
                                 e.CENTRE,
                                 e.CLIENT,
                                 e.POINT,
                                 e.PRODUIT,
                                 e.COMPTEUR,
                                 e.DATEEVT,
                                 PERIODEPRECEDENT = e.PERIODE,
                                 e.LOTRI,
                                 INDEXEVTPRECEDENT = e.INDEXEVT,
                                 CASPRECEDENT = e.CAS,
                                 CONSOPRECEDENT = e.CONSO,
                                 CONSOFACPRECEDENT = e.CONSOFAC,
                             };
                return Tools.Utility.ListToDataTable<object>(query1);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        static List<EVENEMENT> RetourneInfoDernierElemensFacture(IEnumerable<EVENEMENT> _events, string lotri)
        {
            try
            {

                IEnumerable<EVENEMENT> eventValide;
                if (string.IsNullOrEmpty(lotri))
                    eventValide = _events.Where(e => (new int[] { Enumere.EvenementFacture, Enumere.EvenementMisAJour, Enumere.EtatEvenementPurge }.Contains(e.STATUS.Value)) &&
                                                                !(new string[] { Enumere.CasDeposeCompteur, Enumere.CasPoseCompteur }.Contains(e.CAS)));
                else
                    eventValide = _events.Where(e => (new int[] { Enumere.EvenementFacture, Enumere.EvenementMisAJour, Enumere.EtatEvenementPurge }.Contains(e.STATUS.Value)) &&
                                                              !(new string[] { Enumere.CasDeposeCompteur, Enumere.CasPoseCompteur }.Contains(e.CAS)) && e.LOTRI == lotri);

                // ramener les lignes des clients par point de fourniture dont le numevenement est le max et dont la facturation est MAJ,PURGE,ou FACTURE
                IEnumerable<EVENEMENT> maxevent = from pg in eventValide
                                                  group new { pg } by new { pg.CENTRE, pg.CLIENT, pg.POINT, pg.PRODUIT } into pJ
                                                  select new EVENEMENT
                                                  {
                                                      CENTRE = pJ.Key.CENTRE,
                                                      CLIENT = pJ.Key.CLIENT,
                                                      PRODUIT = pJ.Key.PRODUIT,
                                                      POINT = pJ.Key.POINT,
                                                      NUMEVENEMENT = pJ.Max(o => o.pg.NUMEVENEMENT)
                                                  };

                IEnumerable<EVENEMENT> query1 = from ev in maxevent      // ramener les éléments constituants le dernier évenemnt facturé
                                                join
                                                    e in _events on new { ev.CENTRE, ev.CLIENT, ev.PRODUIT, ev.POINT, ev.NUMEVENEMENT }
                                                    equals new { e.CENTRE, e.CLIENT, e.PRODUIT, e.POINT, e.NUMEVENEMENT }
                                                select new EVENEMENT
                                                {
                                                    CENTRE = e.CENTRE,
                                                    CLIENT = e.CLIENT,
                                                    POINT = e.POINT,
                                                    PRODUIT = e.PRODUIT,
                                                    COMPTEUR = e.COMPTEUR,
                                                    DATEEVT = e.DATEEVT,
                                                    PERIODE = e.PERIODE,
                                                    LOTRI = e.LOTRI,
                                                    INDEXEVT = e.INDEXEVT,
                                                    CAS = e.CAS,
                                                    CONSO = e.CONSO,
                                                    CONSOFAC = e.CONSOFAC,
                                                    //FK_IDCAS = e.FK_IDCAS,
                                                    COEFLECT = e.COEFLECT,
                                                    ORDRE = e.ORDRE,
                                                    NUMEVENEMENT = ev.NUMEVENEMENT
                                                };
                return query1.ToList();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        static DataTable PageriNonFacture(string centre, string client, string produit, int point, string lotri, string periode)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    var query2 = //(from evn in eventNonFacture
                                  from pg in context.EVENEMENT 
                                  where pg.CENTRE == centre && pg.CLIENT == client && pg.PRODUIT == produit && pg.POINT == point
                                        && pg.LOTRI == lotri

                                  select new
                                  {
                                      pg.CENTRE,
                                      pg.CLIENT,
                                      pg.POINT,
                                      pg.PRODUIT,
                                      pg.LOTRI,
                                      PERIODE = periode,
                                      CASPAGERI = pg.CAS,
                                      //a.CLIENT1.NOMABON,
                                      //evn.COMPTEUR,
                                      pg.TOURNEE,
                                      //NOMPIA = m.LIBELLE,
                                      //evn.DATEEVT,
                                      //PERIODEPRECEDENT = ev.PERIODE,
                                      //evn.PERIODE,
                                      //evn.CODEEVT,
                                      //evn.INDEXEVT,
                                      //evn.CAS,
                                      //evn.ENQUETE,
                                      //evn.PRODUIT,
                                      //evn.NUMEVENEMENT,
                                      //evn.LOTRI,
                                      //evn.FACTURE,
                                      //evn.SURFACTURATION,
                                      //evn.STATUS,
                                      //evn.TYPECONSO,
                                      //evn.DIAMETRE,
                                      //evn.DMAJ,
                                      //evn.MATRICULE,
                                      //evn.FACPER,
                                      //evn.QTEAREG,
                                      //evn.DERPERF,
                                      //evn.DERPERFN,
                                      //evn.CONSOFAC,
                                      //evn.REGIMPUTE,
                                      //evn.REGCONSO,
                                      //evn.COEFLECT,
                                      //evn.COEFCOMPTAGE,
                                      //evn.PUISSANCE,
                                      //evn.TYPECOMPTAGE,
                                      //evn.TCOMPT,
                                      //evn.COEFK1,
                                      //evn.COEFK2,
                                      //evn.COEFFAC,
                                      //evn.ORDRE,
                                      //INDEXEVTPRECEDENT = ev.INDEXEVT,
                                      //CASPRECEDENT = ev.CAS,
                                      //CONSOPRECEDENT = ev.CONSO,
                                      //CONSOFACPRECEDENT = ev.CONSOFAC,
                                      //pg.TOURNEE,
                                      pg.ORDTOUR,
                                      FK_IDPAGERI = pg.PK_ID
                                  };

                    return Galatee.Tools.Utility.ListToDataTable<object>(query2);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //static DataTable PagisolNonFacture(string centre, string client, string produit, int point, string lotri, string periodefac)
        //{
        //    try
        //    {
        //        using (galadbEntities context = new galadbEntities())
        //        {
        //            var query2 =
        //                          from pg in context.PAGISOL
        //                          where pg.CENTRE == centre && pg.CLIENT == client && pg.PRODUIT == produit && pg.POINT == point
        //                                && pg.LOTRI == lotri && pg.PERFAC == periodefac

        //                          select new
        //                          {
        //                              pg.CENTRE,
        //                              pg.CLIENT,
        //                              pg.POINT,
        //                              pg.PRODUIT,
        //                              pg.TOURNEE,
        //                              pg.ORDTOUR,
        //                              pg.LOTRI,
        //                              CASPAGISOL = pg.CAS,
        //                              PERIODE = pg.PERFAC,
        //                              FK_IDPAGISOL = pg.PK_ID,
        //                              pg.FK_IDEVENEMENT
        //                          };
        //            return Galatee.Tools.Utility.ListToDataTable<object>(query2);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        static DataTable RetourneInfoPageriElementNonFacture(IEnumerable<EVENEMENT> eventNonFacture)
        {
            try
            {
                List<CsEvenement> listePageri = new List<CsEvenement>();
                foreach (var item in eventNonFacture)
                    listePageri.AddRange(Tools.Utility.GetEntityListFromQuery<CsEvenement>(PageriNonFacture(item.CENTRE, item.CLIENT, item.PRODUIT, item.POINT, item.LOTRI, item.PERIODE)));

                return Galatee.Tools.Utility.ListToDataTable<CsEvenement>(listePageri);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable RetourneTousEvenemntsFactureDuClient(int Fk_Idcentre, string centre, string pclient, string ordre, string produit, int point)
        {
            try
            {
                List<int?> StatusIndex = new List<int?>() { 3, 4, 99 };
                List<int?> StatusIndexFact = new List<int?>() { 0, 1 };
                using (galadbEntities context = new galadbEntities())
                {
                    var query2 =
                                  from ev in context.EVENEMENT
                                  from rel in ev.ABON.CLIENT1.AG.TOURNEE1.TOURNEERELEVEUR
                                  where ev.CENTRE == centre && ev.CLIENT == pclient && ev.PRODUIT == produit
                                                                    && ev.POINT == point && ev.FK_IDCENTRE == Fk_Idcentre &&
                                                                    StatusIndexFact.Contains(ev.STATUS)
                                  select new
                                  {
                                      ev.CENTRE,
                                      ev.CLIENT,
                                      ev.PRODUIT,
                                      ev.POINT,
                                      ev.NUMEVENEMENT,
                                      ev.ORDRE,
                                      ev.COMPTEUR,
                                      ev.DATEEVT,
                                      ev.PERIODE,
                                      ev.CODEEVT,
                                      ev.INDEXEVT,
                                      ev.CAS,
                                      ev.ENQUETE,
                                      ev.CONSO,
                                      ev.CONSONONFACTUREE,
                                      ev.LOTRI,
                                      ev.FACTURE,
                                      ev.SURFACTURATION,
                                      ev.STATUS,
                                      ev.TYPECONSO,
                                      ev.REGLAGECOMPTEUR,
                                      ev.TYPETARIF,
                                      ev.FORFAIT,
                                      ev.CATEGORIE,
                                      ev.CODECONSO,
                                      ev.PROPRIO,
                                      ev.MODEPAIEMENT,
                                      ev.MATRICULE,
                                      ev.FACPER,
                                      ev.QTEAREG,
                                      ev.DERPERF,
                                      ev.DERPERFN,
                                      ev.CONSOFAC,
                                      ev.REGIMPUTE,
                                      ev.REGCONSO,
                                      ev.COEFLECT,
                                      ev.COEFCOMPTAGE,
                                      ev.PUISSANCE,
                                      ev.TYPECOMPTAGE,
                                      ev.TYPECOMPTEUR,
                                      ev.COEFK1,
                                      ev.COEFK2,
                                      ev.COEFFAC,
                                      ev.USERCREATION,
                                      ev.DATECREATION,
                                      ev.DATEMODIFICATION,
                                      ev.USERMODIFICATION,
                                      ev.PK_ID,
                                      ev.FK_IDCANALISATION,
                                      ev.FK_IDABON,
                                      ev.FK_IDCOMPTEUR,
                                      ev.FK_IDCENTRE,
                                      ev.FK_IDPRODUIT,
                                      ev.ESTCONSORELEVEE,
                                      RELEVEUR = rel.RELEVEUR.ADMUTILISATEUR.LIBELLE
                                  };

                    int MaxEvenemtFacture = context.EVENEMENT.Where(ev=> StatusIndex.Contains(ev.STATUS) &&  
                                                                                     ev.FK_IDCENTRE == Fk_Idcentre && 
                                                                                     ev.CENTRE == centre && ev.CLIENT == pclient && 
                                                                                     ev.ORDRE == ordre && ev.PRODUIT ==produit && 
                                                                                     ev.POINT == point).Max(y=>y.NUMEVENEMENT );



                                          
                    return Galatee.Tools.Utility.ListToDataTable<object>(query2);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public static DataTable  RetourneTousEvenemntsDuClient(int Fk_Idcentre, string centre, string pclient, string ordre, string produit, int point)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    var query2 =
                                  from ev in context.EVENEMENT
                                  from rel in ev.ABON.CLIENT1.AG.TOURNEE1.TOURNEERELEVEUR
                                  where ev.CENTRE == centre && ev.CLIENT == pclient && ev.PRODUIT == produit
                                                                    && ev.POINT == point && ev.FK_IDCENTRE == Fk_Idcentre
                                  select new
                                  {
                                        ev. CENTRE ,
                                        ev. CLIENT ,
                                        ev. PRODUIT ,
                                        ev. POINT ,
                                        ev. NUMEVENEMENT ,
                                        ev. ORDRE ,
                                        ev. COMPTEUR ,
                                        ev. DATEEVT ,
                                        ev. PERIODE ,
                                        ev. CODEEVT ,
                                        ev. INDEXEVT ,
                                        ev. CAS ,
                                        ev. ENQUETE ,
                                        ev. CONSO ,
                                        ev. CONSONONFACTUREE ,
                                        ev. LOTRI ,
                                        ev. FACTURE ,
                                        ev. SURFACTURATION ,
                                        ev. STATUS ,
                                        ev. TYPECONSO ,
                                        ev.REGLAGECOMPTEUR,
                                        ev. TYPETARIF ,
                                        ev. FORFAIT ,
                                        ev. CATEGORIE ,
                                        ev. CODECONSO ,
                                        ev. PROPRIO ,
                                        ev. MODEPAIEMENT ,
                                        ev. MATRICULE ,
                                        ev. FACPER ,
                                        ev. QTEAREG ,
                                        ev. DERPERF ,
                                        ev. DERPERFN ,
                                        ev. CONSOFAC ,
                                        ev. REGIMPUTE ,
                                        ev. REGCONSO ,
                                        ev. COEFLECT ,
                                        ev. COEFCOMPTAGE ,
                                        ev. PUISSANCE ,
                                        ev. TYPECOMPTAGE ,
                                        ev. TYPECOMPTEUR ,
                                        ev. COEFK1 ,
                                        ev. COEFK2 ,
                                        ev. COEFFAC ,
                                        ev. USERCREATION ,
                                        ev. DATECREATION ,
                                        ev. DATEMODIFICATION ,
                                        ev. USERMODIFICATION ,
                                        ev. PK_ID ,
                                        ev. FK_IDCANALISATION ,
                                        ev. FK_IDABON ,
                                        ev. FK_IDCOMPTEUR ,
                                        ev. FK_IDCENTRE ,
                                        ev. FK_IDPRODUIT ,
                                        ev. ESTCONSORELEVEE,
                                         RELEVEUR=     rel.RELEVEUR.ADMUTILISATEUR.LIBELLE 
                                        

                                  };
                    return Galatee.Tools.Utility.ListToDataTable<object>(query2);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //static DataTable RetourneInfoDernierEvenemntFacture(string lotri,string centre, string pclient, string ordre, string produit,int point)
        //{
        //    try
        //    {
        //        List<EVENEMENT> listeEvenemntClient = RetourneTousEvenemntsDuClient(lotri,centre,pclient,ordre,produit,point);
        //        List<CsEvenement> maxevent = RetourneNumeroDernierEvenemntFactureDuClient(lotri, listeEvenemntClient);

        //        using (galadbEntities context = new galadbEntities())
        //        {
        //            var query2 = (from ev in context.EVENEMENT where ev.CENTRE == centre && ev.CLIENT == pclient && ev.PRODUIT == produit 
        //                                                            && ev.POINT == point
        //                          //                             var query2 = (from q in maxevent
        //                          //join ev in context.EVENEMENT on new { q.CENTRE, q.CLIENT, q.PRODUIT, q.POINT, q.NUMEVENEMENT}
        //                          //                             equals new { ev.CENTRE, ev.CLIENT, ev.PRODUIT, ev.POINT, ev.NUMEVENEMENT }

        //                          //join pg in context.PAGERI on new { q.CENTRE, q.CLIENT, q.PRODUIT, q.POINT }
        //                          //                          equals new { pg.CENTRE, pg.CLIENT, pg.PRODUIT, pg.POINT }
        //                          //join cl in client on new { q.CLIENT, q.CENTRE }
        //                          //                  equals new { CLIENT = cl.REFCLIENT, cl.CENTRE }
        //                          //join _abon in abonNonRes on cl.PK_ID equals _abon.FK_IDCLIENT
        //                          //join t in context.TOURNEE on new { pg.TOURNEE, pg.CENTRE }
        //                          //                          equals new { TOURNEE = t.CODE, t.CENTRE }
        //                          //join m in context.ADMUTILISATEUR on t.MATRICULEPIA equals m.MATRICULE
        //                          select new
        //                          {
        //                              CENTRE = ev.CENTRE,
        //                              CLIENT = ev.CLIENT,
        //                              POINT = ev.POINT,
        //                              //NOMABON = cl.NOMABON,
        //                              //TOURNEE = pg.TOURNEE,
        //                              //NOMPIA = m.LIBELLE,
        //                              PERIODEPRECEDENT = ev.PERIODE,
        //                              PRODUIT = ev.PRODUIT,
        //                              NUMEVENEMENT = ev.NUMEVENEMENT,
        //                              LOTRI = ev.LOTRI,
        //                              INDEXEVTPRECEDENT = ev.INDEXEVT,
        //                              CASPRECEDENT = ev.CAS,
        //                              CONSOPRECEDENT = ev.CONSO,
        //                              CONSOFACPRECEDENT = ev.CONSOFAC
        //                              //ORDTOUR= pg.ORDTOUR,
        //                              //FK_IDPAGERI = pg.PK_ID,
        //                          }).Distinct();
        //            return Galatee.Tools.Utility.ListToDataTable<object>(query2);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public static DataTable RetourneEvenementClientCanalisation(string Centre, string Client, string Produit, int Point, string Compteur)
        {
            try
            {
                string vide = "";

                using (galadbEntities context = new galadbEntities())
                {
                    var client = context.CLIENT;
                    var events = context.EVENEMENT.Where(e => e.CENTRE == Centre && e.CLIENT == Client && e.PRODUIT == Produit &&
                                                         e.POINT == Point);
                    var pageri = context.EVENEMENT .Where(p => p.CENTRE == Centre && p.CLIENT == Client && p.PRODUIT == Produit &&
                                                            p.POINT == Point && (p.CAS == Enumere.CompteurNonLu || p.CAS == Enumere.CompteurChanged));
                    //(p.TOURNEE == tournee || p.TOURNEE.Equals(vide)) && 
                    //(p.ORDTOUR == sequence || p.ORDTOUR.Equals(vide)) &&

                    // retourner les historique de consommation des clients

                    //var historiqe = context.HISTORIQUE;
                    //if(!string.IsNullOrEmpty(centre))
                    //    events = context.EVENEMENT.Where(e => e.CENTRE == centre && e.LOTRI == lotri);
                    //else
                    //    events = context.EVENEMENT.Where(e => e.LOTRI == lotri);

                    //if (!string.IsNullOrEmpty(tournee) && !string.IsNullOrEmpty(sequence))
                    //    pageri = context.PAGERI.Where(p => p.CENTRE == centre && p.LOTRI == lotri && p.TOURNEE == tournee && p.ORDTOUR == sequence &&
                    //                                 (p.CASREL == Enumere.CompteurNonLu || p.CASREL == Enumere.CompteurChanged));
                    //else
                    //{
                    //    if (string.IsNullOrEmpty(tournee))
                    //    {
                    //        if (string.IsNullOrEmpty(sequence))
                    //            pageri = context.PAGERI.Where(p => p.CENTRE == centre && p.LOTRI == lotri &&
                    //                                    (p.CASREL == Enumere.CompteurNonLu || p.CASREL == Enumere.CompteurChanged));
                    //        else
                    //            pageri = context.PAGERI.Where(p => p.CENTRE == centre && p.LOTRI == lotri && p.ORDTOUR == sequence &&
                    //                                    (p.CASREL == Enumere.CompteurNonLu || p.CASREL == Enumere.CompteurChanged));
                    //    }
                    //    else
                    //    { 
                    //     if(string.IsNullOrEmpty(sequence))
                    //         pageri = context.PAGERI.Where(p => p.CENTRE == centre && p.LOTRI == lotri && p.TOURNEE == tournee &&
                    //                                 (p.CASREL == Enumere.CompteurNonLu || p.CASREL == Enumere.CompteurChanged));
                    //        else
                    //         pageri = context.PAGERI.Where(p => p.CENTRE == centre && p.LOTRI == lotri && p.TOURNEE == tournee && p.ORDTOUR == sequence &&
                    //                                 (p.CASREL == Enumere.CompteurNonLu || p.CASREL == Enumere.CompteurChanged));
                    //    }
                    // }

                    var query1 = (from _events in events
                                  join _pageri in pageri on new { LOTRI = _events.LOTRI, _events.CENTRE, _events.CLIENT, _events.PRODUIT, POINT = _events.POINT }
                                                         equals new { _pageri.LOTRI, _pageri.CENTRE, _pageri.CLIENT, _pageri.PRODUIT, _pageri.POINT }
                                  select new
                                  {
                                      _events.CENTRE,
                                      _events.CLIENT,
                                      _events.POINT,
                                      _events.ORDRE,
                                      _events.NUMEVENEMENT,
                                      _events.COMPTEUR,
                                      _events.DATEEVT,
                                      _events.PERIODE,
                                      _events.CODEEVT,
                                      _events.PRODUIT,
                                      _events.INDEXEVT,
                                      _events.CAS,
                                      _events.ENQUETE,
                                      _events.CONSO,
                                      _events.CONSONONFACTUREE,
                                      _events.LOTRI,
                                      _events.FACTURE,
                                      _events.SURFACTURATION,
                                      _events.STATUS,
                                      _events.TYPECONSO,
                                      _events.REGLAGECOMPTEUR,
                                      _events.MATRICULE,
                                      _events.FACPER,
                                      _events.QTEAREG,
                                      _events.DERPERF,
                                      _events.DERPERFN,
                                      _events.CONSOFAC,
                                      _events.REGIMPUTE,
                                      _events.REGCONSO,
                                      _events.COEFLECT,
                                      _events.COEFCOMPTAGE,
                                      _events.PUISSANCE,
                                      _events.TYPECOMPTAGE,
                                      _events.TYPECOMPTEUR,
                                      _events.COEFK1,
                                      _events.COEFK2,
                                      _events.COEFFAC,
                                      _pageri.TOURNEE,
                                      _pageri.ORDTOUR
                                  }).OrderBy(e => new { e.CENTRE, e.CLIENT, e.ORDRE, e.PRODUIT, e.POINT }).Distinct();

                    // historique de consommation du client sur les points pr différents clients
                    //var query3 = 
                    return Galatee.Tools.Utility.ListToDataTable<object>(query1);
                }

                //using (galadbEntities context = new galadbEntities())
                //{

                //    var events = context.EVENEMENT.Where(e => e.CENTRE == Centre && e.CLIENT == Client  && e.PRODUIT == Produit &&
                //                                         e.POINT == Point && e.COMPTEUR == Compteur);

                //    IEnumerable<object> query = (from _events in events
                //                                select new
                //                                {
                //                                   _events.CENTRE,
                //                                   _events.CLIENT,
                //                                   POINT= _events.POINT,
                //                                   EVENEMENT=_events.EVENEMENT1,
                //                                   _events.COMPTEUR,
                //                                   _events.DATEEVT,
                //                                   _events.PERIODE,
                //                                   _events.CODEEVT,
                //                                   _events.INDEXEVT,
                //                                   _events.CAS,
                //                                   _events.ENQUETE,
                //                                   _events.CONSO,
                //                                   _events.CONSONONFACTUREE,
                //                                   _events.LOTRI,
                //                                   _events.FACTURE,
                //                                   _events.SURFACTURATION,
                //                                   _events.STATUS,
                //                                   _events.TYPECONSO,
                //                                   _events.DIAMETRE,
                //                                   _events.DMAJ,
                //                                   _events.FK_MATRICULE,
                //                                   _events.FACPER,
                //                                   _events.QTEAREG,
                //                                   _events.DERPERF,
                //                                   _events.DERPERFN,
                //                                   _events.CONSOFAC,
                //                                   _events.REGIMPUTE,
                //                                   _events.REGCONSO,
                //                                   _events.COEFLECT,
                //                                   _events.COEFCOMPTAGE,
                //                                   _events.PUISSANCE,
                //                                   _events.TYPECOMPTAGE,
                //                                   _events.TCOMPT,
                //                                   _events.COEFK1,
                //                                   _events.COEFK2,
                //                                   _events.COEFFAC,
                //                                   _events.ORDRE,
                //                                   _events.PRODUIT
                //                                }).Distinct();
                //    return Galatee.Tools.Utility.ListToDataTable<object>(query);
                //}
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable RetourneHistoriqueConsoPoint(int fk_idcentre,string Centre, string Client, string Ordre, string Produit, int Point)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    var historik = context.HISTORIQUE.Where(h => h.FK_IDCENTRE == fk_idcentre && h.CENTRE == Centre && h.CLIENT == Client &&
                                                                h.ORDRE == Ordre && h.PRODUIT == Produit && h.POINT == Point).OrderByDescending (h => h.PERIODE).Take(Enumere.DernierHistoriqueEvent);

                    IEnumerable<object> query = (from h in historik
                                                select new
                                                {
                                                    h.PK_ID ,
                                                    h.PERIODE,
                                                    h.FK_IDCENTRE,
                                                    h.CENTRE,
                                                    h.CLIENT,
                                                    h.ORDRE,
                                                    h.POINT,
                                                    h.CONSO,
                                                    h.CONSOFAC,
                                                    h.FK_IDPRODUIT,
                                                    h.PRODUIT,
                                                    h.NBREJOURFACTURE 
                                                }).OrderByDescending (t=>t.PERIODE).Take(12);
                    return Galatee.Tools.Utility.ListToDataTable<object>(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static DataTable retourneDonneeDuCasIndex(string Lotri, List<int> LesCentre, List<int?> Latournee, string Cas)
        {
            List<int?> StatusIndex = new List<int?>() { 3, 4, 99 };

            using (galadbEntities context = new galadbEntities())
            {
                var evenemnt = (from p in context.EVENEMENT
                               from t in p.TOURNEE1.TOURNEERELEVEUR
                             where p.LOTRI == Lotri &&
                                   LesCentre.Contains( p.FK_IDCENTRE ) &&
                                   Latournee.Contains(p.FK_IDTOURNEE ) &&
                                   p.CAS == Cas &&
                                     !StatusIndex.Contains(p.STATUS)

                             select new
                             {
                                 p.CENTRE,
                                 p.CLIENT,
                                 p.ORDRE,
                                 p.CAS,
                                 p.PERIODE,
                                 p.INDEXEVT,
                                 p.CONSO,
                                 p.TOURNEE,
                                 p.PRODUIT,
                                 LIBELLEPRODUIT = p.PRODUIT1.LIBELLE,
                                 p.LOTRI,
                                 p.NUMEVENEMENT,
                                 p.ORDTOUR,
                                 p.POINT,
                                 p.ABON.CLIENT1.NOMABON,
                                 ADRESSE = p.ABON.CLIENT1.ADRMAND1,
                                 p.ORDREAFFICHAGE,
                                 p.ABON.CLIENT1.AG.RUE,
                                 p.ABON.PUISSANCE ,
                                 p.ABON.CLIENT1.AG.PORTE,
                                 p.TYPECOMPTAGE,
                                 p.COMPTEUR,
                                 p.REGLAGECOMPTEUR,
                                 p.DATEEVT,
                                 LIBELLERELEVEUR = t.RELEVEUR.ADMUTILISATEUR.LIBELLE,
                                 p.INDEXPRECEDENTEFACTURE,
                                 p.CONSOMOYENNEPRECEDENTEFACTURE,
                                 p.DATERELEVEPRECEDENTEFACTURE
                             }).Distinct();

                return Galatee.Tools.Utility.ListToDataTable<object>(evenemnt);
            }
        }

        public static DataTable retourneDonneeDuCasIndexConfirmer(string Lotri, List<int> LesCentre, List<int?> Latournee, string Cas)
        {
            string vide = "";
            List<int?> StatusIndex = new List<int?>() { 3, 4, 99 };

            using (galadbEntities context = new galadbEntities())
            {
                var evenemnt = from p in context.EVENEMENT
                               from t in p.TOURNEE1.TOURNEERELEVEUR
                               where p.LOTRI == Lotri &&
                                     LesCentre.Contains(p.FK_IDCENTRE) &&
                                     Latournee.Contains(p.FK_IDTOURNEE) &&
                                     p.CAS == Cas &&  p.ENQUETE =="C" &&
                                     !StatusIndex.Contains(p.STATUS)
                               select new
                               {

                                   p.CENTRE,
                                   p.CLIENT,
                                   p.ORDRE,
                                   p.CAS,
                                   p.PERIODE,
                                   p.INDEXEVT,
                                   p.CONSO,
                                   p.TOURNEE,
                                   p.PRODUIT,
                                   LIBELLEPRODUIT = p.PRODUIT1.LIBELLE,
                                   p.LOTRI,
                                   p.NUMEVENEMENT,
                                   p.ORDTOUR,
                                   p.POINT,
                                   p.ABON.CLIENT1.NOMABON,
                                   ADRESSE = p.ABON.CLIENT1.ADRMAND1,
                                   p.ORDREAFFICHAGE ,
                                   p.ABON.CLIENT1.AG.RUE,
                                   p.ABON.CLIENT1.AG.PORTE ,
                                   p.TYPECOMPTAGE ,
                                   p.COMPTEUR,
                                   p.REGLAGECOMPTEUR,
                                   p.DATEEVT,
                                   LIBELLERELEVEUR = t.RELEVEUR.ADMUTILISATEUR.LIBELLE,
                                   p.INDEXPRECEDENTEFACTURE,
                                   p.CONSOMOYENNEPRECEDENTEFACTURE,
                                   p.DATERELEVEPRECEDENTEFACTURE
                               };

                return Galatee.Tools.Utility.ListToDataTable<object>(evenemnt);
            }
        }
        //public static DataTable EditionDesRi(string Lotri, string Centre, string Tournee)
        //{
        //    try
        //    {
        //        using (galadbEntities context = new galadbEntities())
        //        {

        //            List<int?> StatusIndex = new List<int?>() { 3, 4, 99 };

        //            var page = context.PAGERI ;
        //            var query = (from p in page 
        //                         from ev in page.
        //                         from _brt in _ag.BRT
        //                         from _abon in _client.ABON
        //                         from cpt in _abon.CANALISATION
        //                         from tourrel in _ag.TOURNEE1.TOURNEERELEVEUR
        //                         where
        //                         _client.FK_IDCATEGORIE == fkidcategorie &&
        //                         _abon.FK_IDPERIODICITEFACTURE == fkidperiodicite &&
        //                         _ag.FK_IDTOURNEE == fkidtournee &&
        //                         _abon.FK_IDPRODUIT == fk_idProduit &&
        //                         _ag.FK_IDCENTRE == fk_idcentre &&
        //                         _brt.DRES == null
        //                         && !context.EVENEMENT.Any(t => t.FK_IDABON == _abon.PK_ID && t.PERIODE == PeriodeEnCours &&
        //                                                    ((t.STATUS != Enumere.EtatEvenementSupprime) || (t.STATUS != Enumere.EtatEvenementAnnule)) &&
        //                                                     !new string[] { FactureIsoleIndex, FactureAnnulatinIndex, FactureResiliationIndex }.Contains(t.LOTRI.Substring(Enumere.TailleCentre, (Enumere.TailleNumeroBatch - Enumere.TailleCentre))))
        //                         select new
        //                         {
        //                             _abon.CENTRE,
        //                             _abon.CLIENT,
        //                             _abon.ORDRE,
        //                             _ag.TOURNEE,
        //                             _ag.ORDTOUR,
        //                             _abon.PRODUIT,
        //                             _abon.PUISSANCE,
        //                             _abon.DRES,
        //                             _abon.PERFAC,
        //                             idabon = _abon.PK_ID,
        //                             cpt.COMPTEUR.NUMERO,
        //                             DIAMETRE = cpt.COMPTEUR.DIAMETRECOMPTEUR.CODE,
        //                             cpt.COMPTEUR.ETAT,
        //                             cpt.COMPTEUR.COEFLECT,
        //                             _brt.TYPECOMPTAGE,
        //                             cpt.COMPTEUR.COEFCOMPTAGE,
        //                             cpt.COMPTEUR.TYPECOMPTEUR,
        //                             PROPRIETAIRE = cpt.PROPRIETAIRE.CODE,
        //                             cpt.POINT,
        //                             _client.CATEGORIE,
        //                             _abon.TYPETARIF,
        //                             _abon.FORFAIT,
        //                             _client.CODECONSO,
        //                             _brt.PUISSANCEINSTALLEE,

        //                             FK_IDCLIENT = _client.PK_ID,
        //                             FK_IDCANALISATION = cpt.PK_ID,
        //                             FK_IDABON = _abon.PK_ID,
        //                             FK_IDCENTRE = _abon.FK_IDCENTRE,
        //                             FK_IDPRODUIT = _abon.FK_IDPRODUIT,
        //                             FK_IDTOURNEE = _ag.FK_IDTOURNEE,
        //                             FK_IDCOMPTEUR = cpt.COMPTEUR.PK_ID,
        //                             FK_IDCATEGORIE = _client.FK_IDCATEGORIE,
        //                             FK_IDRELEVEUR = tourrel.FK_IDRELEVEUR
        //                         });


        //            var distinctClient = (from d in query select new { d.CENTRE, d.CLIENT, d.ORDRE, d.POINT, d.PRODUIT }).Distinct();

        //            var MaxEvenemt = from d in distinctClient
        //                             join ev in context.EVENEMENT on new { d.CENTRE, d.CLIENT, d.ORDRE, d.POINT, d.PRODUIT }
        //                                                          equals new { ev.CENTRE, ev.CLIENT, ev.ORDRE, ev.POINT, ev.PRODUIT }
        //                             group new { d, ev } by new { d.CENTRE, d.CLIENT, d.ORDRE, d.POINT, d.PRODUIT } into pJoin
        //                             select new
        //                             {
        //                                 pJoin.Key.CENTRE,
        //                                 pJoin.Key.CLIENT,
        //                                 pJoin.Key.ORDRE,
        //                                 pJoin.Key.PRODUIT,
        //                                 pJoin.Key.POINT,
        //                                 MaxEvenement = pJoin.Max(o => o.ev.NUMEVENEMENT)
        //                             };

        //            //var MaxEvenemtFacture = from d in distinctClient
        //            //                 join ev in context.EVENEMENT on new { d.CENTRE, d.CLIENT, d.ORDRE, d.POINT, d.PRODUIT }
        //            //                                              equals new { ev.CENTRE, ev.CLIENT, ev.ORDRE, ev.POINT, ev.PRODUIT }
        //            //                 where StatusIndex.Contains(ev.STATUS) // confusion avec la procédure stockée corresp
        //            //                 group new { d, ev } by new { d.CENTRE, d.CLIENT, d.ORDRE, d.POINT, d.PRODUIT } into pJoin
        //            //                 select new
        //            //                 {
        //            //                     pJoin.Key.CENTRE,
        //            //                     pJoin.Key.CLIENT,
        //            //                     pJoin.Key.ORDRE,
        //            //                     pJoin.Key.PRODUIT,
        //            //                     pJoin.Key.POINT,
        //            //                     MaxEvenement = pJoin.Max(o => o.ev.NUMEVENEMENT)
        //            //                 };

        //            var AnclientMax = from a in MaxEvenemt
        //                              join ev in context.EVENEMENT on new { a.CENTRE, a.CLIENT, a.ORDRE, a.POINT, a.PRODUIT }
        //                                                             equals new { ev.CENTRE, ev.CLIENT, ev.ORDRE, ev.POINT, ev.PRODUIT }
        //                              where ev.NUMEVENEMENT == a.MaxEvenement
        //                              select new
        //                              {
        //                                  a.CENTRE,
        //                                  a.CLIENT,
        //                                  a.ORDRE,
        //                                  a.PRODUIT,
        //                                  a.POINT,
        //                                  a.MaxEvenement
        //                              };
        //            //    var Anclient = from a in MaxEvenemtFacture
        //            //join ev in context.EVENEMENT on new { a.CENTRE, a.CLIENT, a.ORDRE, a.POINT, a.PRODUIT }
        //            //                               equals new { ev.CENTRE, ev.CLIENT, ev.ORDRE, ev.POINT, ev.PRODUIT }
        //            //where ev.NUMEVENEMENT == a.MaxEvenement
        //            //select new
        //            //{
        //            //    a.CENTRE,
        //            //    a.CLIENT,
        //            //    a.ORDRE,
        //            //    a.PRODUIT,
        //            //    a.POINT,
        //            //    a.MaxEvenement,
        //            //    DANCIENINDEX = ev.INDEXEVT,
        //            //    ANCIENCAS = ev.CAS,
        //            //    ev.DATEEVT,
        //            //    ev.QTEAREG 
        //            //};

        //            var query2 = from q1 in query
        //                         //join an in Anclient on new { q1.CENTRE, q1.CLIENT, q1.ORDRE, q1.POINT, q1.PRODUIT }
        //                         //                    equals new { an.CENTRE, an.CLIENT, an.ORDRE, an.POINT, an.PRODUIT }
        //                         join anMax in AnclientMax on new { q1.CENTRE, q1.CLIENT, q1.ORDRE, q1.POINT, q1.PRODUIT }
        //                                             equals new { anMax.CENTRE, anMax.CLIENT, anMax.ORDRE, anMax.POINT, anMax.PRODUIT }
        //                         select new
        //                         {
        //                             q1.CENTRE,
        //                             q1.CLIENT,
        //                             q1.ORDRE,
        //                             q1.TOURNEE,
        //                             q1.ORDTOUR,
        //                             q1.PRODUIT,
        //                             q1.PUISSANCE,
        //                             q1.DRES,
        //                             q1.PERFAC,
        //                             COMPTEUR = q1.NUMERO,
        //                             q1.DIAMETRE,
        //                             ETATCOMPTEUR = q1.ETAT,
        //                             q1.COEFLECT,
        //                             q1.TYPECOMPTAGE,
        //                             q1.COEFCOMPTAGE,
        //                             q1.TYPECOMPTEUR,
        //                             q1.PROPRIETAIRE,
        //                             q1.POINT,
        //                             q1.CATEGORIE,
        //                             q1.TYPETARIF,
        //                             q1.FORFAIT,
        //                             q1.CODECONSO,
        //                             q1.PUISSANCEINSTALLEE,

        //                             q1.FK_IDCLIENT,
        //                             q1.FK_IDCANALISATION,
        //                             q1.FK_IDABON,
        //                             q1.FK_IDCENTRE,
        //                             q1.FK_IDPRODUIT,
        //                             q1.FK_IDTOURNEE,
        //                             q1.FK_IDCOMPTEUR,
        //                             q1.FK_IDCATEGORIE,
        //                             q1.FK_IDRELEVEUR,
        //                             NUMEVENEMENT = anMax.MaxEvenement,
        //                             //an.QTEAREG,
        //                             //an.DANCIENINDEX,
        //                             //an.ANCIENCAS,
        //                             //an.DATEEVT,

        //                         };
        //            return Galatee.Tools.Utility.ListToDataTable<object>(query2);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        public static DataTable EditionDesRi(CsLotri  leLot)
        {
            string vide = "";
            List<int?> StatusIndex = new List<int?>() { 3, 4, 99 };

            using (galadbEntities context = new galadbEntities())
            {
                var evenemnt =from p in context.EVENEMENT
                               //join y in context.TOURNEERELEVEUR  on p.FK_IDTOURNEE equals y.FK_IDTOURNEE into pTemp
                               //join u in context.ADMUTILISATEUR on pTemp.
                               where p.LOTRI == leLot.NUMLOTRI  &&
                                  p.CENTRE == leLot.CENTRE &&
                                  p.FK_IDCENTRE == leLot.FK_IDCENTRE &&
                                  p.FK_IDTOURNEE == leLot.FK_IDTOURNEE &&
                                  p.CATEGORIE == leLot.CATEGORIECLIENT &&
                                  p.PRODUIT  == leLot.PRODUIT  &&
                                  p.PERFAC == leLot.PERIODICITE &&
                                   (p.STATUS  == 0) &&
                                  !(new string[] { Enumere.CasDeposeCompteur, Enumere.CasPoseCompteur }.Contains(p.CAS))
                               select new
                               {

                                   p.CENTRE,
                                   p.CLIENT,
                                   p.ORDRE,
                                   p.PERIODE,
                                   p.INDEXEVT,
                                   p.CONSO,
                                   p.TOURNEE,
                                   p.PRODUIT,
                                   LIBELLEPRODUIT = p.PRODUIT1.LIBELLE,
                                   p.LOTRI,
                                   p.NUMEVENEMENT,
                                   p.ORDTOUR,
                                   p.POINT,
                                   p.ABON.CLIENT1.NOMABON,
                                   p.ABON.PUISSANCE ,
                                   ADRESSE = p.ABON.CLIENT1.ADRMAND1 ,
                                   RUE = p.ABON.CLIENT1.AG.RUE,
                                   PORTE = p.ABON.CLIENT1.AG.PORTE ,
                                   p.COMPTEUR,
                                   p.ORDREAFFICHAGE ,
                                   p.REGLAGECOMPTEUR,
                                   p.TYPECOMPTAGE ,
                                   p.DATEEVT,
                                   p.CASPRECEDENTEFACTURE,
                                   p.INDEXPRECEDENTEFACTURE, 
                                   p.DATERELEVEPRECEDENTEFACTURE ,
                                   //LIBELLERELEVEUR = pTemp.
                               } ;

                return Galatee.Tools.Utility.ListToDataTable<object>(evenemnt);
            }
        }

        public static DataTable EditionDesRiAlaCreation(string numLotri,string usercreation,string periode)
        {
            List<int?> StatusIndex = new List<int?>() { 3, 4, 99 };

            using (galadbEntities context = new galadbEntities())
            {
                var evenemnt = from p in context.EVENEMENT
                               from y in p.TOURNEE1.TOURNEERELEVEUR
                               where p.LOTRI == numLotri &&
                                  p.MATRICULE == usercreation 
                               select new
                               {

                                   p.CENTRE,
                                   p.CLIENT,
                                   p.ORDRE,
                                   p.PERIODE,
                                   p.INDEXEVT,
                                   p.CONSO,
                                   p.TOURNEE,
                                   p.PRODUIT,
                                   LIBELLEPRODUIT = p.PRODUIT1.LIBELLE,
                                   p.LOTRI,
                                   p.NUMEVENEMENT,
                                   p.ORDTOUR,
                                   p.POINT,
                                   p.ABON.CLIENT1.NOMABON,
                                   ADRESSE = p.ABON.CLIENT1.ADRMAND1,
                                   p.COMPTEUR,
                                   p.REGLAGECOMPTEUR,
                                   p.DATEEVT,
                                   p.CASPRECEDENTEFACTURE,
                                   p.INDEXPRECEDENTEFACTURE,
                                   p.DATERELEVEPRECEDENTEFACTURE,
                                   LIBELLERELEVEUR = y.RELEVEUR.ADMUTILISATEUR.LIBELLE
                               };

                return Galatee.Tools.Utility.ListToDataTable<object>(evenemnt);
            }
        }

        //public static DataTable EditionDesRi(string Lotri, string Centre, string Tournee)
        //{
        //    try
        //    {
        //        using (galadbEntities context = new galadbEntities())
        //        {

        //            IEnumerable<PAGERI> pageri;
        //            var eventValide = context.EVENEMENT.Where(e => (new int[] { Enumere.EvenementFacture, Enumere.EvenementMisAJour, Enumere.EtatEvenementPurge, Enumere.EvenementReleve }.Contains(e.STATUS.Value)) &&
        //                                                           !(new string[] { Enumere.CasDeposeCompteur, Enumere.CasPoseCompteur }.Contains(e.CAS)));

        //            // ramener les clients du lot qui se trouvent dans la table pageri dont les index n'ont pas Ã©tÃ© saisis 
        //            pageri = context.PAGERI.Where(p => p.CENTRE == Centre && p.LOTRI == Lotri && p.TOURNEE == Tournee &&
        //                                             (p.CAS == Enumere.CompteurNonLu || p.CAS == Enumere.CompteurChanged));
        //            var maxevent = from evnt in eventValide
        //                           join pg in pageri on new { evnt.CENTRE, evnt.CLIENT, evnt.PRODUIT, evnt.POINT }
        //                                             equals new { pg.CENTRE, pg.CLIENT, pg.PRODUIT, pg.POINT }
        //                           group new { evnt, pg } by new { pg.CENTRE, pg.CLIENT, pg.POINT, pg.PRODUIT } into pJ
        //                           select new
        //                           {
        //                               pJ.Key.CENTRE,
        //                               pJ.Key.CLIENT,
        //                               pJ.Key.PRODUIT,
        //                               pJ.Key.POINT,
        //                               MAXEVENMENT = pJ.Max(o => o.evnt.NUMEVENEMENT)
        //                           };



        //            var query2 = (from q in maxevent
        //                          join ev in context.EVENEMENT on new { q.CENTRE, q.CLIENT, q.PRODUIT, q.POINT, NUMEVENEMENT = q.MAXEVENMENT }
        //                                                 equals new { ev.CENTRE, ev.CLIENT, ev.PRODUIT, ev.POINT, ev.NUMEVENEMENT }
        //                          join pg in pageri on new { q.CENTRE, q.CLIENT, q.PRODUIT, q.POINT }
        //                                            equals new { pg.CENTRE, pg.CLIENT, pg.PRODUIT, pg.POINT }
        //                          select new
        //                          {
        //                              q.CENTRE,
        //                              q.CLIENT,
        //                              q.POINT,
        //                              NOMABON = ev.CANALISATION.ABON.CLIENT1.NOMABON,
        //                              ev.NUMEVENEMENT,
        //                              ev.COMPTEUR,
        //                              pg.TOURNEE,
        //                              //NOMPIA = m.LIBELLE,***
        //                              ev.PRODUIT,
        //                              pg.LOTRI,
        //                              ev.FACTURE,
        //                              ev.SURFACTURATION,
        //                              ev.STATUS,
        //                              ev.TYPECONSO,
        //                              ev.DIAMETRE,
        //                              ev.MATRICULE,
        //                              ev.FACPER,
        //                              ev.QTEAREG,
        //                              ev.DERPERF,
        //                              ev.DERPERFN,
        //                              ev.CONSOFAC,
        //                              ev.REGIMPUTE,
        //                              ev.REGCONSO,
        //                              ev.COEFLECT,
        //                              ev.COEFCOMPTAGE,
        //                              ev.PUISSANCE,
        //                              ev.TYPECOMPTAGE,
        //                              ev.TYPECOMPTEUR ,
        //                              ev.COEFK1,
        //                              ev.COEFK2,
        //                              ev.COEFFAC,
        //                              ev.ORDRE,
        //                              INDEXEVTPRECEDENT = ev.INDEXEVT,
        //                              CASPRECEDENT = ev.CAS,
        //                              CONSOPRECEDENT = ev.CONSO,
        //                              CONSOFACPRECEDENT = ev.CONSOFAC,
        //                              pg.ORDTOUR,
        //                              FK_IDPAGERI = pg.PK_ID,
        //                              pg.FK_IDEVENEMENT,
        //                          }).Distinct();
        //            return Galatee.Tools.Utility.ListToDataTable<object>(query2);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //}

        public static DataTable RetourneCanalisation(int Fk_IDcentre, string pCentre, string pClient, string pProduit, int? pPoint)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    string produit = string.IsNullOrEmpty(pProduit) ? "" : pProduit.ToLower();
                    string point = pPoint == null ? "" : pPoint.ToString();
                    string vide = "";

                    IEnumerable<object> query = null;
                    var _CANALISATION = context.CANALISATION;

                    query =
                    from _LeCANALISATION in _CANALISATION
                    where
                    _LeCANALISATION.FK_IDCENTRE == Fk_IDcentre &&
                    _LeCANALISATION.CENTRE == pCentre &&
                          _LeCANALISATION.CLIENT == pClient &&
                    (produit.Equals(vide) || _LeCANALISATION.PRODUIT == pProduit) &&
                    (point.Equals(vide) || _LeCANALISATION.POINT == pPoint)
                    select new
                    {
                        _LeCANALISATION.CENTRE,
                        _LeCANALISATION.CLIENT,
                        _LeCANALISATION.PRODUIT,
                        _LeCANALISATION.POINT,
                        _LeCANALISATION.BRANCHEMENT,
                        _LeCANALISATION.COMPTEUR.NUMERO,
                        //_LeCANALISATION.PROPRIO,
                        _LeCANALISATION.COMPTEUR.TYPECOMPTEUR ,
                        _LeCANALISATION.COMPTEUR.MARQUE ,
                         REGLAGECOMPTEUR =    _LeCANALISATION.REGLAGECOMPTEUR  ,
                        _LeCANALISATION.COMPTEUR.COEFLECT,
                        //_LeCANALISATION.CADCOMPT,
                        _LeCANALISATION.COMPTEUR.ANNEEFAB,
                        _LeCANALISATION.SURFACTURATION,
                        _LeCANALISATION.DEBITANNUEL,
                        _LeCANALISATION.DEPOSE,
                        _LeCANALISATION.POSE,
                        //_LeCANALISATION.FONCTIONNEMENT,
                        //_LeCANALISATION.PLOMBAGE,
                        _LeCANALISATION.COMPTEUR.COEFCOMPTAGE,
                        //_LeCANALISATION.COMPTEUR.TYPECOMPTAGE,
                         LIBELLETCOMPT = _LeCANALISATION.COMPTEUR.TYPECOMPTEUR1.LIBELLE,
                         LIBELLETYPECOMPTEUR = _LeCANALISATION.COMPTEUR.TYPECOMPTEUR1.LIBELLE,
                        _LeCANALISATION.USERCREATION,
                        _LeCANALISATION.DATECREATION,
                        _LeCANALISATION.DATEMODIFICATION,
                        _LeCANALISATION.USERMODIFICATION,
                        _LeCANALISATION.PK_ID,
                        _LeCANALISATION.FK_IDCENTRE,
                        _LeCANALISATION.FK_IDREGLAGECOMPTEUR ,
                        _LeCANALISATION.COMPTEUR.FK_IDTYPECOMPTEUR,
                        _LeCANALISATION.FK_IDPRODUIT,
                        _LeCANALISATION.COMPTEUR.FK_IDMARQUECOMPTEUR
                    };
                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable RetourneEvenementPose(CsEvenement   leEvt)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
            
                    IEnumerable<object> query = null;
                    var _Evenement = context.EVENEMENT.Where(t => t.FK_IDABON == leEvt.FK_IDABON &&
                                                                  t.COMPTEUR == leEvt.NOUVCOMPTEUR && 
                                                                  t.POINT == leEvt.POINT &&
                                                                  t.CAS == Enumere.CasPoseCompteur);
                    query =
                    from leEvement in _Evenement
                    select new
                    {
                        leEvement.POINT,
                        leEvement.COMPTEUR,
                        leEvement.INDEXEVT,
                        leEvement.FK_IDCANALISATION,
                        leEvement.FK_IDCOMPTEUR 
                    };
                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static DataTable VerificationNumeroCompteur(CsEvenement leEvt)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
            
                    IEnumerable<object> query = null;
                    var _Canalisation = context.CANALISATION .Where(t => t.FK_IDABON == leEvt.FK_IDABON &&  
                                                                  t.POINT == leEvt.POINT &&
                                                                  t.DEPOSE    == null );
                    query =
                    from leEvement in _Canalisation
                    select new
                    {
                        leEvement.POINT,
                        leEvement.COMPTEUR,
                        leEvement.PK_ID ,
                        leEvement.FK_IDCOMPTEUR 
                    };
                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        


        public static DataTable RetourneCanalisationbyIdEvenement(int? Fk_IdCompteur)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
            
                    IEnumerable<object> query = null;
                    var _CANALISATION = context.CANALISATION.Where (t=>t.PK_ID == Fk_IdCompteur );

                    query =
                    from _LeCANALISATION in _CANALISATION
                    select new
                    {
                        _LeCANALISATION.CENTRE,
                        //_LeCANALISATION.CLIENT,
                        _LeCANALISATION.PRODUIT,
                        _LeCANALISATION.POINT,
                        _LeCANALISATION.BRANCHEMENT,
                        _LeCANALISATION.COMPTEUR,
                        //_LeCANALISATION.PROPRIO,
                        _LeCANALISATION.COMPTEUR.TYPECOMPTEUR ,
                        _LeCANALISATION.COMPTEUR.MARQUE ,
                         REGLAGECOMPTEUR =   _LeCANALISATION.REGLAGECOMPTEUR   ,
                        _LeCANALISATION.COMPTEUR.COEFLECT,
                        //_LeCANALISATION.CADCOMPT,
                        _LeCANALISATION.COMPTEUR.ANNEEFAB,
                        _LeCANALISATION.SURFACTURATION,
                        _LeCANALISATION.DEBITANNUEL,
                        //_LeCANALISATION.DEPOSE ,
                        //_LeCANALISATION.POSE ,
                        //_LeCANALISATION.FONCTIONNEMENT,
                        //_LeCANALISATION.PLOMBAGE,
                        _LeCANALISATION.COMPTEUR.COEFCOMPTAGE,
                        //_LeCANALISATION.COMPTEUR.TYPECOMPTAGE,
                        LIBELLETCOMPT = _LeCANALISATION.COMPTEUR.TYPECOMPTEUR1 .LIBELLE,
                        _LeCANALISATION.USERCREATION,
                        _LeCANALISATION.DATECREATION,
                        _LeCANALISATION.DATEMODIFICATION,
                        _LeCANALISATION.USERMODIFICATION,
                        _LeCANALISATION.PK_ID,
                        _LeCANALISATION.FK_IDCENTRE,
                        _LeCANALISATION.FK_IDREGLAGECOMPTEUR ,
                        _LeCANALISATION.COMPTEUR.FK_IDTYPECOMPTEUR ,
                        _LeCANALISATION.FK_IDPRODUIT,
                        _LeCANALISATION.COMPTEUR.FK_IDMARQUECOMPTEUR, 
                        _LeCANALISATION.COMPTEUR.CADRAN
                    };
                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable EditionDesRiNonSaisi(string Lotri, int idCentre, int idtourne)
        {
            try
            {
                return new DataTable();

                //using (galadbEntities context = new galadbEntities())
                //{
                //    string vide = "";
                //    List<string> CasIndex = new List<string>() { "##", "++" };
                //    List<int?> StatusIndex = new List<int?>() { 3, 4, 99 };

                //    string periodeFac = context.LOTRI.FirstOrDefault(l => l.NUMLOTRI == Lotri).PERIODE;
                //    var can = context.CANALISATION;
                //    var pageri = from pg in context.EVENEMENT 
                //                 where (pg.FK_IDTOURNEE== idtourne  || idtourne == 0  &&
                //                       pg.FK_IDCENTRE == idCentre   || idCentre == 0 &&
                //                       pg.LOTRI.Equals(Lotri))
                //                 select pg;

                //    var query1 = from ev in pageri
                //                 where ev.PERIODE == periodeFac && CasIndex.Contains(ev.CAS) && !StatusIndex.Contains(ev.STATUS)
                //                 select new
                //                 {
                //                     ev.CANALISATION.ABON.CLIENT1.CENTRE  ,
                //                     ev.CANALISATION.ABON.CLIENT1.REFCLIENT ,
                //                     ev.CANALISATION.ABON.CLIENT1.ORDRE,
                //                     ev.CANALISATION.ABON.CLIENT1.NOMABON,
                //                     ev.CANALISATION.ABON.CLIENT1.ADRMAND1 ,
                //                     ev.PERIODE,
                //                     DNOUVELINDEX = ev.INDEXEVT,
                //                     ev.CONSO,
                //                     ev.PRODUIT,
                //                     ev.TOURNEE,
                //                     ev.LOTRI,
                //                     ev.NUMEVENEMENT,
                //                     ev.FK_IDCANALISATION,
                //                     SEQUENCE = p.ORDTOUR,
                //                     ev.POINT,
                //                     ev.STATUS
                //                 };

                //    var distinctClient = (from d in query1 select new { d.CENTRE, d.REFCLIENT, d.ORDRE, d.POINT, d.PRODUIT, d.FK_IDCANALISATION }).Distinct();

                //    var MaxEvenemt = from d in distinctClient
                //                     join ev in context.EVENEMENT on d.FK_IDCANALISATION equals ev.FK_IDCANALISATION
                //                     where StatusIndex.Contains(ev.STATUS)
                //                     group new { d, ev } by new { d.CENTRE, d.REFCLIENT , d.ORDRE, d.POINT, d.PRODUIT, d.FK_IDCANALISATION } into pJoin
                //                     select new
                //                     {
                //                         pJoin.Key.CENTRE,
                //                         pJoin.Key.REFCLIENT ,
                //                         pJoin.Key.ORDRE,
                //                         pJoin.Key.PRODUIT,
                //                         pJoin.Key.POINT,
                //                         pJoin.Key.FK_IDCANALISATION,
                //                         MaxEvenement = pJoin.Max(o => o.ev.NUMEVENEMENT)
                //                     };

                //    var Anclient = from a in MaxEvenemt
                //                   join ev in context.EVENEMENT on a.FK_IDCANALISATION equals ev.FK_IDCANALISATION
                //                   where ev.NUMEVENEMENT == (a.MaxEvenement - 1)
                //                   select new
                //                   {
                //                       a.CENTRE,
                //                       a.REFCLIENT,
                //                       a.ORDRE,
                //                       a.PRODUIT,
                //                       a.POINT,
                //                       a.MaxEvenement,
                //                       DANCIENINDEX = ev.INDEXEVT,
                //                       ANCIENCAS = ev.CAS,
                //                       a.FK_IDCANALISATION,
                //                       //ev.CANALISATION.COMPTEUR,
                //                       //DIAMBRT = (ev.CANALISATION.COMPTEUR.FK_IDDIAMETRECOMPTEUR != null ? context.DIAMETRECOMPTEUR.FirstOrDefault(g => g.PK_ID == ev.CANALISATION.COMPTEUR.FK_IDDIAMETRECOMPTEUR).CODE :
                //                       //         context.COMPTAGE.FirstOrDefault(g => g.PK_ID == ev.CANALISATION.COMPTEUR.FK_IDTYPECOMPTAGE).TYPECOMPTAGE),
                //                       ev.DATEEVT,
                //                    idclient =   ev.CANALISATION.ABON.CLIENT1.PK_ID  
                //                   };

                //    var query2 = from an in Anclient
                //                 join q1 in query1 on an.FK_IDCANALISATION equals q1.FK_IDCANALISATION
                //                 select new
                //                 {
                //                     q1.CENTRE,
                //                     q1.REFCLIENT ,
                //                     q1.ORDRE,
                //                     q1.PERIODE,
                //                     q1.DNOUVELINDEX,
                //                     q1.CONSO,
                //                     q1.PRODUIT,
                //                     q1.TOURNEE,
                //                     LOTRI = q1.LOTRI,
                //                     q1.NUMEVENEMENT,
                //                     q1.SEQUENCE,
                //                     q1.POINT,
                //                     q1.NOMABON,
                //                     q1.ADRMAND1,
                //                     //an.COMPTEUR,
                //                     //DIAMETRE = an.DIAMBRT,
                //                     an.DANCIENINDEX,
                //                     an.ANCIENCAS,
                //                     DATERELEVE = an.DATEEVT
                //                 };
                //    return Galatee.Tools.Utility.ListToDataTable<object>(query2);
                //}
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        static List<CsEnregRI> PageriEvenmnentDesRiNonSaisi(string Lotri, string Centre, string Tournee)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    string vide = "";
                    List<string> CasIndex = new List<string>() { "##", "++" };
                    //List<int?> StatusIndex = new List<int?>() { 3, 4, 99 };

                    string periodeFac = context.LOTRI.FirstOrDefault(l => l.NUMLOTRI == Lotri).PERIODE;

                    IEnumerable<CsEnregRI> pageri = from p in context.EVENEMENT 
                                                    where (p.TOURNEE.Equals(Tournee) || Tournee.Equals(vide) &&
                                                           p.CENTRE.Equals(Centre) || Centre.Equals(vide) &&
                                                           p.LOTRI.Equals(Lotri) && CasIndex.Contains(p.CAS))
                                                    select new CsEnregRI
                                                    {
                                                        CENTRE = p.CENTRE,
                                                        CLIENT = p.CLIENT,
                                                        ORDRE = p.ORDRE,
                                                        PERIODE = periodeFac,
                                                        INDEXEVT = p.INDEXEVT,
                                                        CONSO = p.CONSO,
                                                        PRODUIT = p.PRODUIT,
                                                        TOURNEE = p.TOURNEE,
                                                        LOTRI = p.LOTRI,
                                                        NUMEVENEMENT = p.NUMEVENEMENT,
                                                        FK_IDCANALISATION = p.FK_IDCANALISATION,
                                                        //or p.ORDTOUR,
                                                        POINT = p.POINT,
                                                        STATUS = p.STATUS
                                                    };

                    return pageri.ToList();
                    //return Galatee.Tools.Utility.ListToDataTable<object>(pageri);NUMEVENEMENT=FK_IDCANALISATION=INDEXEVT
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        static List<EVENEMENT> EvenemntsDesRiNonSaisi(string Lotri, string Centre, string Tournee)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    List<string> CasIndex = new List<string>() { "##", "++" };
                    List<int?> StatusIndex = new List<int?>() { 3, 4, 99 };

                    string periodeFac = context.LOTRI.FirstOrDefault(l => l.NUMLOTRI == Lotri).PERIODE;

                    var query1 = from ev in context.EVENEMENT
                                 where ev.PERIODE == periodeFac && !StatusIndex.Contains(ev.STATUS)
                                 select new
                                 {
                                     ev.CENTRE,
                                     ev.CLIENT,
                                     ev.ORDRE,
                                     ev.PERIODE,
                                     DNOUVELINDEX = ev.INDEXEVT,
                                     ev.CONSO,
                                     ev.PRODUIT,
                                     //p.TOURNEE,
                                     ev.LOTRI,
                                     ev.NUMEVENEMENT,
                                     ev.FK_IDCANALISATION,
                                     //SEQUENCE = p.ORDTOUR,
                                     ev.POINT,
                                     ev.STATUS
                                 };

                    return Galatee.Tools.Utility.GetEntityListFromQuery<EVENEMENT>(Galatee.Tools.Utility.ListToDataTable<object>(query1)).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        static string PeriodeDesRiNonSaisi(string Lotri)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    return context.LOTRI.FirstOrDefault(l => l.NUMLOTRI == Lotri).PERIODE;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<CsEnregRI> EditionDesRiNonSaisi2(string Lotri, string Centre, string Tournee)
        {
            try
            {
                List<CsEnregRI> PageriEvenmnent = PageriEvenmnentDesRiNonSaisi(Lotri, Centre, Tournee);
                List<int?> StatusIndex = new List<int?>() { 3, 4, 99 };


                List<CsEnregRI> events = (from ev in PageriEvenmnent
                                          where !StatusIndex.Contains(ev.STATUS)
                                          select ev).ToList();

                string periode = PeriodeDesRiNonSaisi(Lotri);
                //List<EVENEMENT> events = EvenemntsDesRiNonSaisi(Lotri, Centre, Tournee);

                //using (galadbEntities context = new galadbEntities())
                //{
                List<string> CasIndex = new List<string>() { "##", "++" };

                string periodeFac = periode;// context.LOTRI.FirstOrDefault(l => l.NUMLOTRI == Lotri).PERIODE;

                IEnumerable<CsEnregRI> query1 = from p in PageriEvenmnent
                                                join
                                                    ev in events on new { p.LOTRI, p.CENTRE, p.CLIENT, p.PRODUIT }
                                                                        equals new { ev.LOTRI, ev.CENTRE, ev.CLIENT, ev.PRODUIT }
                                                //where ev.PERIODE == periodeFac
                                                select new CsEnregRI
                                                {
                                                    CENTRE = p.CENTRE,
                                                    CLIENT = p.CLIENT,
                                                    ORDRE = ev.ORDRE,
                                                    PERIODE = ev.PERIODE,
                                                    INDEXEVT = ev.INDEXEVT,
                                                    CONSO = ev.CONSO,
                                                    PRODUIT = ev.PRODUIT,
                                                    TOURNEE = p.TOURNEE,
                                                    LOTRI = p.LOTRI,
                                                    NUMEVENEMENT = ev.NUMEVENEMENT,
                                                    FK_IDCANALISATION = ev.FK_IDCANALISATION,
                                                    //SEQUENCE = p.ORDTOUR,
                                                    POINT = ev.POINT,
                                                    STATUS = ev.STATUS
                                                };

                //var distinctClient = (from d in query1 select new { d.FK_IDCANALISATION}).Distinct();
                IEnumerable<CsEnregRI> distinctClient = (from d in query1
                                                         select new CsEnregRI
                                                                     {
                                                                         CENTRE = d.CENTRE,
                                                                         CLIENT = d.CLIENT,
                                                                         ORDRE = d.ORDRE,
                                                                         POINT = d.POINT,
                                                                         PRODUIT = d.PRODUIT
                                                                         //NUMEVENEMENT=d.NUMEVENEMENT,
                                                                         //FK_IDCANALISATION=d.FK_IDCANALISATION
                                                                     }).Distinct();

                IEnumerable<CsEnregRI> MaxEvenemt = from d in distinctClient
                                                    join ev in events
                                                    on new { d.CENTRE, d.CLIENT, d.ORDRE, d.POINT, d.PRODUIT }
                                                                                 equals new { ev.CENTRE, ev.CLIENT, ev.ORDRE, ev.POINT, ev.PRODUIT }
                                                    where StatusIndex.Contains(ev.STATUS)
                                                    group new { d, ev } by new { d.CENTRE, d.CLIENT, d.ORDRE, d.POINT, d.PRODUIT } into pJoin
                                                    //group new { d, ev } by new { d.CENTRE, d.CLIENT, d.ORDRE, d.POINT, d.PRODUIT } into pJoin
                                                    select new CsEnregRI
                                                    {
                                                        CENTRE = pJoin.Key.CENTRE,
                                                        CLIENT = pJoin.Key.CLIENT,
                                                        ORDRE = pJoin.Key.ORDRE,
                                                        PRODUIT = pJoin.Key.PRODUIT,
                                                        POINT = pJoin.Key.POINT,
                                                        //FK_IDCANALISATION= pJoin.Key.FK_IDCANALISATION,
                                                        NUMEVENEMENT = pJoin.Max(o => o.ev.NUMEVENEMENT)
                                                    };

                IEnumerable<CsEnregRI> Anclient = from a in MaxEvenemt
                                                  join ev in events //on a.FK_IDCANALISATION equals ev.FK_IDCANALISATION
                                                  on new { a.CENTRE, a.CLIENT, a.ORDRE, a.POINT, a.PRODUIT }
                                                                                 equals new { ev.CENTRE, ev.CLIENT, ev.ORDRE, ev.POINT, ev.PRODUIT }
                                                  where ev.NUMEVENEMENT == (a.NUMEVENEMENT - 1)
                                                  select new CsEnregRI
                                                {
                                                    CENTRE = a.CENTRE,
                                                    CLIENT = a.CLIENT,
                                                    ORDRE = a.ORDRE,
                                                    PRODUIT = a.PRODUIT,
                                                    POINT = a.POINT,
                                                    NUMEVENEMENT = a.NUMEVENEMENT,
                                                    INDEXEVT = ev.INDEXEVT,
                                                    //ANCIENCAS = ev.CAS,
                                                    //FK_IDCANALISATION= a.FK_IDCANALISATION
                                                    //ev.CANALISATION.COMPTEUR,
                                                    //ev.CANALISATION.BRT.DIAMBRT,
                                                    //ev.DATEEVT
                                                };

                IEnumerable<CsEnregRI> query2 = from an in Anclient //context.CLIENT Anclient

                                                //             join an in Anclient on new { cl.CENTRE, CLIENT = cl.REFCLIENT, cl.ORDRE }
                                                //                               equals new { an.CENTRE, an.CLIENT, an.ORDRE }
                                                //join q1 in query1 on new { cl.CENTRE, CLIENT = cl.REFCLIENT, ORDRE = cl.ORDRE }
                                                //                 equals new { q1.CENTRE, q1.CLIENT, q1.ORDRE }
                                                //join cnl in context.CANALISATION  on q1.FK_IDCANALISATION equals cnl.PK_ID
                                                ////on new { q1.PRODUIT, q1.CLIENT }
                                                //                                 equals new { cnl.PRODUIT, cnl.CLIENT }
                                                //join br in context.BRT on new { q1.PRODUIT, q1.CENTRE, q1.CLIENT }
                                                //                       equals new { br.PRODUIT, br.CENTRE, br.CLIENT }
                                                join q1 in query1 on new { an.CENTRE, an.CLIENT, an.ORDRE, an.POINT, an.PRODUIT }
                                                                    equals new { q1.CENTRE, q1.CLIENT, q1.ORDRE, q1.POINT, q1.PRODUIT }
                                                select new CsEnregRI
                                                {
                                                    CENTRE = q1.CENTRE,
                                                    CLIENT = q1.CLIENT,
                                                    ORDRE = q1.ORDRE,
                                                    PERIODE = q1.PERIODE,
                                                    INDEXEVT = q1.INDEXEVT,
                                                    CONSO = q1.CONSO,
                                                    PRODUIT = q1.PRODUIT,
                                                    TOURNEE = q1.TOURNEE,
                                                    LOTRI = q1.LOTRI,
                                                    NUMEVENEMENT = q1.NUMEVENEMENT,
                                                    //q1.SEQUENCE,
                                                    POINT = q1.POINT,
                                                    //NOMABON = cl.NOMABON,
                                                    //ADRMAND1 = cl.ADRMAND1,
                                                    //an.COMPTEUR,
                                                    //cnl.COMPTEUR,
                                                    //DIAMETRE = an.DIAMBRT,
                                                    //DIAMETRE = br.DIAMBRT,
                                                    //an.DANCIENINDEX
                                                    //an.ANCIENCAS,
                                                    //DATERELEVE = an.DATEEVT
                                                };
                return distinctClient.ToList();
                //return Galatee.Tools.Utility.ListToDataTable<CsEnregRI>(query2);
                //}
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable EditionDesRiSaisi(string Lotri, string Centre, string Tournee)
        {
            return new DataTable();
            //using (galadbEntities context = new galadbEntities())
            //{
            //    string vide = "";
            //    List<string> CasIndex = new List<string>() { "##", "++" };
            //    List<int?> StatusIndex = new List<int?>() { 3, 4, 99 };

            //    string periodeFac = context.LOTRI.FirstOrDefault(l => l.NUMLOTRI == Lotri).PERIODE;

            //    var pageri = from pg in context.PAGERI
            //                 where (pg.TOURNEE.Equals(Tournee) || Tournee.Equals(vide)) &&
            //                       (pg.CENTRE.Equals(Centre) || Centre.Equals(vide)) &&
            //                       pg.LOTRI.Equals(Lotri)
            //                 select pg;
            //    var query1 = from p in pageri
            //                 join
            //                     ev in context.EVENEMENT on new { p.LOTRI, p.CENTRE, p.CLIENT, p.PRODUIT }
            //                                         equals new { LOTRI = ev.LOTRI, ev.CENTRE, ev.CLIENT, ev.PRODUIT }
            //                 where ev.PERIODE == periodeFac && !CasIndex.Contains(p.CAS) && !StatusIndex.Contains(ev.STATUS)
            //                 select new
            //                 {

            //                     p.CENTRE,
            //                     p.CLIENT,
            //                     ev.ORDRE,
            //                     ev.PERIODE,
            //                     DNOUVELINDEX = ev.INDEXEVT,
            //                     ev.CONSO,
            //                     ev.PRODUIT,
            //                     p.TOURNEE,
            //                     p.LOTRI,
            //                     ev.NUMEVENEMENT,
            //                     SEQUENCE = p.ORDTOUR,
            //                     ev.POINT,
            //                     ev.STATUS
            //                 };

            //    var distinctClient = (from d in query1 select new { d.CENTRE, d.CLIENT, d.ORDRE, d.POINT, d.PRODUIT }).Distinct();

            //    var MaxEvenemt = from d in distinctClient
            //                     join ev in context.EVENEMENT on new { d.CENTRE, d.CLIENT, d.ORDRE, d.POINT, d.PRODUIT }
            //                                                  equals new { ev.CENTRE, ev.CLIENT, ev.ORDRE, ev.POINT, ev.PRODUIT }
            //                     where !StatusIndex.Contains(ev.STATUS) // confusion avec la procédure stockée corresp
            //                     group new { d, ev } by new { d.CENTRE, d.CLIENT, d.ORDRE, d.POINT, d.PRODUIT } into pJoin
            //                     select new
            //                     {
            //                         pJoin.Key.CENTRE,
            //                         pJoin.Key.CLIENT,
            //                         pJoin.Key.ORDRE,
            //                         pJoin.Key.PRODUIT,
            //                         pJoin.Key.POINT,
            //                         MaxEvenement = pJoin.Max(o => o.ev.NUMEVENEMENT)
            //                     };

            //    var Anclient = from a in MaxEvenemt
            //                   join ev in context.EVENEMENT on new { a.CENTRE, a.CLIENT, a.ORDRE, a.POINT, a.PRODUIT }
            //                                                  equals new { ev.CENTRE, ev.CLIENT, ev.ORDRE, ev.POINT, ev.PRODUIT }
            //                   where ev.NUMEVENEMENT == (a.MaxEvenement - 1)
            //                   select new
            //                   {

            //                       a.CENTRE,
            //                       a.CLIENT,
            //                       a.ORDRE,
            //                       a.PRODUIT,
            //                       a.POINT,
            //                       a.MaxEvenement,
            //                       DANCIENINDEX = ev.INDEXEVT,
            //                       ANCIENCAS = ev.CAS,
            //                       ev.DATEEVT
            //                   };

            //    var query2 = from cl in context.CLIENT

            //                 //join q1 in query1 on new { cl.CENTRE, CLIENT = cl.REFCLIENT, ORDRE = cl.ORDRE }
            //                 //                  equals new { q1.CENTRE, q1.CLIENT, q1.ORDRE }
            //                 //join cnl in context.CANALISATION on new { q1.PRODUIT, q1.CLIENT }
            //                 //                                 equals new { cnl.PRODUIT, cnl.CLIENT }
            //                 //join br in context.BRT on new { q1.PRODUIT, q1.CENTRE, q1.CLIENT }
            //                 //                        equals new { br.PRODUIT, br.CENTRE, br.CLIENT }
            //                 //join an in Anclient on new { q1.CENTRE, q1.CLIENT, q1.ORDRE, q1.POINT, q1.PRODUIT }
            //                 //                    equals new { an.CENTRE, an.CLIENT, an.ORDRE, an.POINT, an.PRODUIT }
            //                 select new
            //                 {
            //                     //CENTRE = q1.CENTRE,
            //                     //CLIENT = q1.CLIENT,
            //                     //ORDRE = q1.ORDRE,
            //                     //q1.PERIODE,
            //                     //q1.DNOUVELINDEX,
            //                     //q1.CONSO,
            //                     //PRODUIT = q1.PRODUIT,
            //                     //TOURNEE = q1.TOURNEE,
            //                     //LOTRI = q1.LOTRI,
            //                     //q1.NUMEVENEMENT,
            //                     //q1.SEQUENCE,
            //                     //POINT = q1.POINT,
            //                     cl.NOMABON,
            //                     ADRESSE = cl.ADRMAND1,
            //                     //cnl.COMPTEUR,
            //                     //DIAMETRE = br.DIAMBRT,
            //                     //an.DANCIENINDEX,
            //                     //an.ANCIENCAS,
            //                     //DATERELEVE = an.DATEEVT
            //                 };
            //    return Galatee.Tools.Utility.ListToDataTable<object>(query2);
            //}
        }

        public static DataTable RetourneCasEvement(string pLotri, List<int> LesCentre, List<int?>  lesTournee)
        {
            List<int?> StatusIndex = new List<int?>() { 3, 4, 99 };

            using (galadbEntities context = new galadbEntities())
            {
                var pagerie = (from p in context.EVENEMENT 
                               join c in context.CASIND on p.CAS equals c.CODE
                               where (LesCentre.Contains(p.FK_IDCENTRE)  && p.LOTRI == pLotri &&
                                      lesTournee.Contains(p.FK_IDTOURNEE)) && 
                                    ! StatusIndex.Contains( p.STATUS )
                               select new
                               {
                                 CODE =p.CAS,
                                 c.LIBELLE,
                                 c.ENQUETABLE
                                 
                               }).Distinct();
                return Galatee.Tools.Utility.ListToDataTable<object>(pagerie);
            }
        }


        public static DataTable ListeDesEnqueteConfirmer(string pLotri, int pCentre, int pTournee)
        {
            List<int?> StatusIndex = new List<int?>() { 3, 4, 99 };
            using (galadbEntities context = new galadbEntities())
            {
                var pagerie = (from p in context.EVENEMENT
                               join c in context.CASIND on p.CAS equals c.CODE
                               where (p.FK_IDCENTRE == pCentre || pCentre == 0) && p.LOTRI == pLotri &&
                                     (p.FK_IDTOURNEE == pTournee || pTournee == 0)  &&
                                     (p.ENQUETE == "C") &&
                                    !StatusIndex.Contains(p.STATUS)

                               select new
                               {
                                   CODE = p.CAS,
                                   c.LIBELLE,
                                   c.ENQUETABLE

                               }).Distinct();
                return Galatee.Tools.Utility.ListToDataTable<object>(pagerie);
            }
        }

        public static DataTable ListeDesEnquete(CsLotri LeLotri)
        {
            List<int?> StatusIndex = new List<int?>() { 3, 4, 99 };
            using (galadbEntities context = new galadbEntities())
            {
                var pagerie = (from p in context.EVENEMENT 
                               where p.CENTRE == LeLotri.CENTRE && p.FK_IDCENTRE == LeLotri.FK_IDCENTRE && p.LOTRI == LeLotri.NUMLOTRI &&
                                     (LeLotri.TOURNEE == p.TOURNEE) &&
                                     (p.ENQUETE == "E") &&
                                    !StatusIndex.Contains(p.STATUS)
                               select new
                               {
                                   p.CENTRE,
                                   p.CLIENT,
                                   p.PRODUIT,
                                   p.POINT,
                                   p.NUMEVENEMENT,
                                   p.ORDRE,
                                   p.COMPTEUR,
                                   p.DATEEVT,
                                   p.PERIODE,
                                   p.CODEEVT,
                                   p.INDEXEVT,
                                   p.CAS,
                                   p.ENQUETE,
                                   p.CONSO,
                                   p.CONSONONFACTUREE,
                                   p.LOTRI,
                                   p.FACTURE,
                                   p.SURFACTURATION,
                                   p.STATUS,
                                   p.TYPECONSO,
                                   p.REGLAGECOMPTEUR,
                                   p.TYPETARIF,
                                   p.FORFAIT,
                                   p.CATEGORIE,
                                   p.CODECONSO,
                                   p.PROPRIO,
                                   p.MODEPAIEMENT,
                                   p.MATRICULE,
                                   p.FACPER,
                                   p.QTEAREG,
                                   p.DERPERF,
                                   p.DERPERFN,
                                   p.CONSOFAC,
                                   p.REGIMPUTE,
                                   p.REGCONSO,
                                   p.COEFLECT,
                                   p.COEFCOMPTAGE,
                                   p.PUISSANCE,
                                   p.TYPECOMPTAGE,
                                   p.TYPECOMPTEUR,
                                   p.COEFK1,
                                   p.COEFK2,
                                   p.COEFFAC,
                                   p.USERCREATION,
                                   p.DATECREATION,
                                   p.DATEMODIFICATION,
                                   p.USERMODIFICATION,
                                   p.PK_ID,
                                   p.FK_IDCANALISATION,
                                   p.FK_IDABON,
                                   p.FK_IDCOMPTEUR,
                                   p.FK_IDCENTRE,
                                   p.FK_IDPRODUIT,
                                   p.ESTCONSORELEVEE
                               }).Distinct();
                return Galatee.Tools.Utility.ListToDataTable<object>(pagerie);
            }
        }

        public static DataTable ListeDesEnqueteCasOnze(CsLotri LeLotri)
        {
            List<int?> StatusIndex = new List<int?>() { 3, 4, 99 };
            using (galadbEntities context = new galadbEntities())
            {
                var pagerie = (from p in context.EVENEMENT 
                               where p.CENTRE == LeLotri.CENTRE && p.FK_IDCENTRE == LeLotri.FK_IDCENTRE && p.LOTRI == LeLotri.NUMLOTRI &&
                                     (LeLotri.TOURNEE == p.TOURNEE) &&
                                     (p.CAS == "11") &&
                                     (p.ENQUETE == "E") &&
                                    !StatusIndex.Contains(p.STATUS)
                               select new
                               {
                                   p.CENTRE,
                                   p.CLIENT,
                                   p.PRODUIT,
                                   p.POINT,
                                   p.NUMEVENEMENT,
                                   p.ORDRE,
                                   p.COMPTEUR,
                                   p.DATEEVT,
                                   p.PERIODE,
                                   p.CODEEVT,
                                   p.INDEXEVT,
                                   p.CAS,
                                   p.ENQUETE,
                                   p.CONSO,
                                   p.CONSONONFACTUREE,
                                   p.LOTRI,
                                   p.FACTURE,
                                   p.SURFACTURATION,
                                   p.STATUS,
                                   p.TYPECONSO,
                                   p.REGLAGECOMPTEUR,
                                   p.TYPETARIF,
                                   p.FORFAIT,
                                   p.CATEGORIE,
                                   p.CODECONSO,
                                   p.PROPRIO,
                                   p.MODEPAIEMENT,
                                   p.MATRICULE,
                                   p.FACPER,
                                   p.QTEAREG,
                                   p.DERPERF,
                                   p.DERPERFN,
                                   p.CONSOFAC,
                                   p.REGIMPUTE,
                                   p.REGCONSO,
                                   p.COEFLECT,
                                   p.COEFCOMPTAGE,
                                   p.PUISSANCE,
                                   p.TYPECOMPTAGE,
                                   p.TYPECOMPTEUR,
                                   p.COEFK1,
                                   p.COEFK2,
                                   p.COEFFAC,
                                   p.USERCREATION,
                                   p.DATECREATION,
                                   p.DATEMODIFICATION,
                                   p.USERMODIFICATION,
                                   p.PK_ID,
                                   p.FK_IDCANALISATION,
                                   p.FK_IDABON,
                                   p.FK_IDCOMPTEUR,
                                   p.FK_IDCENTRE,
                                   p.FK_IDPRODUIT,
                                   p.ESTCONSORELEVEE
                               }).Distinct();
                return Galatee.Tools.Utility.ListToDataTable<object>(pagerie);
            }
        }
        

        public static DataTable TestClientExist(int PK_IDCLIENT)
        {
            try
            {
                //cmd.CommandText = "SPX_ENC_RETOURNELIGNEFACTURE";
                using (galadbEntities ctontext = new galadbEntities())
                {
                    var Abon = ctontext.ABON;
                    List<CLIENT> lclient = new List<CLIENT>();

                    var abonne = ctontext.ABON;
                    IEnumerable<object> query = (from f in Abon
                                                 where f.FK_IDCLIENT == PK_IDCLIENT
                                                 select new
                                                 {
                                                     f.CLIENT1.NOMABON,
                                                     f.CLIENT1.CENTRE1.CODE ,
                                                     f.CLIENT1.ORDRE,
                                                     f.CLIENT1.REFCLIENT,
                                                     f.CLIENT1.CATEGORIE,
                                                     f.CLIENT1.ADRMAND1,
                                                     f.AVANCE,
                                                     f.TYPETARIF ,
                                                     f.DRES,
                                                     f.CLIENT1.PK_ID ,
                                                     CODESITE = f.CENTRE1.SITE.CODE ,
                                                     LIBELLESITE = f.CENTRE1.SITE.LIBELLE,
                                                     FK_IDSITE = f.CENTRE1.SITE.PK_ID
                                                 });
                    return Galatee.Tools.Utility.ListToDataTable<object>(query);

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static DataTable IsBatchExistDansPagerie(string _NumLot)
        {
            try
            {
                List<int?> StatusIndex = new List<int?>() { Enumere.EvenementSupprimer  ,Enumere.EvenementDetruit };

                using (galadbEntities context = new galadbEntities())
                {
                    var PagerieClient = context.EVENEMENT ;
                    var pagerie = (from p in PagerieClient
                                   where p.LOTRI == _NumLot && !StatusIndex.Contains(p.STATUS)
                                   select new
                                   {
                                       p.CENTRE ,
                                       p.LOTRI ,
                                       p.FK_IDCENTRE 
                                   });
                    return Galatee.Tools.Utility.ListToDataTable<object>(pagerie);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable RetourneListeLotSelonCritere(string NumLot, string Centre, string Tournee)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    var LOTRI = context.LOTRI;
                    var result = (from p in LOTRI
                                  where p.NUMLOTRI == NumLot && p.CENTRE == Centre && p.TOURNEE == Tournee
                                  select p);
                    return Galatee.Tools.Utility.ListToDataTable<object>(result);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable GetEvenement(string PK_ID)
        {
            int id = int.Parse(PK_ID);
            using (galadbEntities context = new galadbEntities())
            {
                var client = context.CLIENT;
                var PagerieClient = context.EVENEMENT ;
                var pagerie = (from p in context.EVENEMENT
                               join l in context.LOTRI on new { NUMLOTRI = p.LOTRI } equals new { l.NUMLOTRI }
                               where p.PK_ID == id

                               select new
                               {
                                   p.CENTRE,
                                   p.CLIENT,
                                   p.POINT,
                                   NOMABON = p.CANALISATION.ABON.CLIENT1.NOMABON,
                                   p.NUMEVENEMENT,
                                   p.COMPTEUR,
                                   l.TOURNEE,
                                   //NOMPIA = m.LIBELLE,***
                                   PERIODE = p.PERIODE,
                                   p.PRODUIT,
                                   LOTRI = p.LOTRI,
                                   p.FACTURE,
                                   p.SURFACTURATION,
                                   p.STATUS,
                                   p.TYPECONSO,
                                   p.REGLAGECOMPTEUR,
                                   p.MATRICULE,
                                   p.FACPER,
                                   p.QTEAREG,
                                   p.DERPERF,
                                   p.DERPERFN,
                                   p.CONSOFAC,
                                   p.REGIMPUTE,
                                   p.REGCONSO,
                                   p.COEFLECT,
                                   p.COEFCOMPTAGE,
                                   p.PUISSANCE,
                                   p.TYPECOMPTAGE,
                                   p.TYPECOMPTEUR,
                                   p.COEFK1,
                                   p.COEFK2,
                                   p.COEFFAC,
                                   p.ORDRE,
                                   //p.ORDTOUR,
                                   FK_IDPAGERI = p.PK_ID,
                                   PK_ID = p.PK_ID,
                                   PK_IDEVENT = p.PK_ID,
                                   p.FK_IDCANALISATION,
                                   p.FK_IDCENTRE,
                                   p.FK_IDPRODUIT,
                                   RELEVEUR = l.RELEVEUR1.ADMUTILISATEUR.LIBELLE
                               });
                return Galatee.Tools.Utility.ListToDataTable<object>(pagerie);
            }
        }

        public static DataTable RetourneListeLotNonTraiteParReleveur(string NumPortable)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    var tournee = context.LOTRI.Where(l => l.ETATFAC5 == Enumere.NonMiseAJours || string.IsNullOrEmpty(l.ETATFAC5));
                    var releveur = context.RELEVEUR.FirstOrDefault(r => r.PORTABLE == NumPortable);
                    if (releveur != null)
                    {
                        List<LOTRI> lotri = new List<LOTRI>();

                        foreach (var t in tournee)
                            lotri.Add(t);

                        IEnumerable<object> query = from t in lotri
                                                    where t.FK_IDRELEVEUR == releveur.PK_ID  
                                                    select new
                                                    {
                                                        t.NUMLOTRI,
                                                        t.JET,
                                                        t.PERIODE,
                                                        t.CENTRE,
                                                        t.PRODUIT,
                                                        t.CATEGORIECLIENT,
                                                        t.PERIODICITE,
                                                        t.EXIG,
                                                        t.DFAC,
                                                        t.ETATFAC1,
                                                        t.ETATFAC2,
                                                        t.ETATFAC3,
                                                        t.ETATFAC4,
                                                        t.ETATFAC5,
                                                        t.ETATFAC6,
                                                        t.ETATFAC7,
                                                        t.ETATFAC8,
                                                        t.ETATFAC9,
                                                        t.ETATFAC10,
                                                        t.TOURNEE,
                                                        t.RELEVEUR,
                                                        t.BASE,
                                                        t.PK_ID,
                                                        t.DATECREATION,
                                                        t.DATEMODIFICATION,
                                                        t.USERCREATION,
                                                        t.USERMODIFICATION,
                                                        t.FK_IDPRODUIT,
                                                        t.FK_IDCATEGORIECLIENT,
                                                        t.FK_IDRELEVEUR
                                                    };


                        return Galatee.Tools.Utility.ListToDataTable<object>(query);
                    }
                    else
                    {
                        return new DataTable();
                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public static DataTable ChargerTourneeReleveur(int Fk_IdReleveur)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                      {
                        var x = context.TOURNEE  ;
                        
                        var pagerie = (from p in x
                                       from y in p.TOURNEERELEVEUR
                                       where y.FK_IDRELEVEUR  == Fk_IdReleveur && y.DATEFIN == null 
                                       select new
                                       {
                                           p.CENTRE,
                                           p.CODE,
                                           p.LIBELLE,
                                           p.LOCALISATION,
                                           p.PRIORITE,
                                           p.USERCREATION,
                                           p.DATECREATION,
                                           p.DATEMODIFICATION,
                                           p.USERMODIFICATION,
                                           p.PK_ID,
                                           p.SUPPRIMER,
                                           p.FK_IDCENTRE,
                                           p.FK_IDLOCALISATION ,
                                           NOMRELEVEUR = y.RELEVEUR.ADMUTILISATEUR.LIBELLE 
                                       });
                        return Galatee.Tools.Utility.ListToDataTable<object>(pagerie);

                        }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable DeleteLot(CsLotri leLot)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    var x = context.EVENEMENT;
                    var pagerie = (from p in x
                                   where p.LOTRI == leLot.NUMLOTRI 
                                   select new
                                   {
                                       p
                                   });
                    return Galatee.Tools.Utility.ListToDataTable<object>(pagerie);

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static DataTable   VerifierSaisie(CsLotri leLot)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    var x = context.EVENEMENT ;
                    var pagerie = (from p in x
                                   where p.LOTRI  == leLot.NUMLOTRI && 
                                         p.STATUS != 0 
                                   select new
                                   {
                                     p.LOTRI  
                                   });
                    return Galatee.Tools.Utility.ListToDataTable<object>(pagerie);

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

       public static void SupprimeEvtLotri (CsLotri leLot, galadbEntities context)
        {
            try
            {
                    List<EVENEMENT> x = context.EVENEMENT.Where(t=>t.LOTRI == leLot.NUMLOTRI && t.FK_IDTOURNEE == leLot.FK_IDTOURNEE ).ToList() ;
                    foreach (EVENEMENT item in x)
                        item.STATUS = 8;

                    LOTRI le = context.LOTRI.FirstOrDefault(t => t.NUMLOTRI == leLot.NUMLOTRI && t.FK_IDTOURNEE == leLot.FK_IDTOURNEE);
                        Entities.DeleteEntity<LOTRI>(le,context);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //public static DataTable ChargerLotriPourDelete(string matricule, string DebutLot, string Finlot)
        //{
        //    try
        //    {
        //        using (galadbEntities context = new galadbEntities())
        //        {
        //            var x = context.LOTRI ;
        //            var e = context.EVENEMENT  ;
        //            var DistinctLot = (from p in x
        //                               join ev in e on new {p.NUMLOTRI ,p.USERCREATION } 
        //                               equals new {NUMLOTRI =ev.LOTRI,ev.USERCREATION } 
        //                           where p.USERCREATION == matricule && ev.CAS != "##" && 
        //                           (string.IsNullOrEmpty(DebutLot) || p.NUMLOTRI == DebutLot)
        //                           select new
        //                           {
        //                               p.NUMLOTRI,
        //                               p.PERIODE,
        //                               p.USERCREATION 
        //                           });

        //            var pagerie = (from p in x
        //                           where p.USERCREATION == matricule &&
        //                           (string.IsNullOrEmpty( DebutLot) || p.NUMLOTRI == DebutLot ) &&
        //                           !DistinctLot.Any(u=>u.NUMLOTRI == p.NUMLOTRI && u.PERIODE == p.PERIODE && p.USERCREATION ==u.USERCREATION )
        //                           select new
        //                           {
        //                               p.NUMLOTRI,
        //                               p.JET,
        //                               p.PERIODE,
        //                               p.CENTRE,
        //                               p.PRODUIT,
        //                               p.CATEGORIECLIENT,
        //                               p.PERIODICITE,
        //                               p.EXIG,
        //                               p.DFAC,
        //                               p.ETATFAC1,
        //                               p.ETATFAC2,
        //                               p.ETATFAC3,
        //                               p.ETATFAC4,
        //                               p.ETATFAC5,
        //                               p.ETATFAC6,
        //                               p.ETATFAC7,
        //                               p.ETATFAC8,
        //                               p.ETATFAC9,
        //                               p.ETATFAC10,
        //                               p.TOURNEE,
        //                               p.RELEVEUR,
        //                               p.BASE,
        //                               p.PK_ID,
        //                               p.DATECREATION,
        //                               p.DATEMODIFICATION,
        //                               p.USERCREATION,
        //                               p.USERMODIFICATION,
        //                               p.FK_IDPRODUIT,
        //                               p.FK_IDCATEGORIECLIENT,
        //                               p.FK_IDRELEVEUR,
        //                               p.FK_IDTOURNEE,
        //                               p.FK_IDCENTRE
        //                           });
        //            return Galatee.Tools.Utility.ListToDataTable<object>(pagerie);

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public static DataTable ChargerTourneeReleveurbyid(int Fk_idTournee)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    var x = context.TOURNEE;

                    var pagerie = (from p in x
                                   from y in p.TOURNEERELEVEUR
                                   where y.FK_IDTOURNEE == Fk_idTournee && y.DATEFIN != null 
                                   select new
                                   {
                                       p.CENTRE,
                                       p.CODE,
                                       p.LIBELLE,
                                       p.LOCALISATION,
                                       p.PRIORITE,
                                       p.USERCREATION,
                                       p.DATECREATION,
                                       p.DATEMODIFICATION,
                                       p.USERMODIFICATION,
                                       p.PK_ID,
                                       p.SUPPRIMER,
                                       p.FK_IDCENTRE,
                                       p.FK_IDLOCALISATION,
                                       NOMRELEVEUR = y.RELEVEUR.ADMUTILISATEUR.LIBELLE
                                   });
                    return Galatee.Tools.Utility.ListToDataTable<object>(pagerie);

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static bool VerifiPeriodeExist(CsClientLotri leClient)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    var evenement = context.EVENEMENT.FirstOrDefault(t => t.FK_IDCENTRE == leClient.FK_IDCENTRE && 
                                                                          t.CENTRE == leClient.CENTRE && 
                                                                          t.CLIENT == leClient.CLIENT && 
                                                                          t.ORDRE == leClient.ORDRE && 
                                                                          t.PRODUIT == leClient.PRODUIT && 
                                                                          t.POINT == leClient.POINT &&
                                                                          t.PERIODE == leClient.PERIODE );
                    if (evenement != null)
                        return true;
                    else
                        return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
