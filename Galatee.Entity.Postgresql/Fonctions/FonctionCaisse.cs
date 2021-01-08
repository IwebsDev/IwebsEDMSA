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
    public class FonctionCaisse
    {
     
        public static decimal? RetourneSoldeClient(int? Fk_idCentre , string centre, string client, string ordre)
        {
            try
            {
                decimal? sommeDebit = RetourneDebitClient(Fk_idCentre, centre, client, ordre);
                decimal? sommeCrebit = RetourneCrebitClient(Fk_idCentre, centre, client, ordre);
                decimal? difference = (sommeDebit - sommeCrebit);
                return difference;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static decimal? RetourneDebitClient(int? Fk_idCentre, string centre, string client, string ordre)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    decimal? TotTranscaisB = context.LCLIENT.Where(t => t.FK_IDCENTRE == Fk_idCentre && t.CENTRE == centre && t.CLIENT == client && t.ORDRE == ordre).Sum(g => g.MONTANT);
                    return (TotTranscaisB == null ? 0 : TotTranscaisB);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static decimal? RetourneCrebitClient(int? Fk_idCentre, string centre, string client, string ordre)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    decimal? TotTranscaisse = context.TRANSCAISSE.Where(t => t.FK_IDCENTRE == Fk_idCentre && t.CENTRE == centre &&
                                                                       t.CLIENT == client && t.ORDRE == ordre && t.COPER != Enumere.CoperTimbre && (t.TOPANNUL != "O" || t.TOPANNUL == null)).Sum(g => g.MONTANT);
                    decimal? TotTranscaisB = context.TRANSCAISB.Where(t => t.FK_IDCENTRE == Fk_idCentre && t.CENTRE == centre &&
                                                                      t.CLIENT == client && t.ORDRE == ordre && t.COPER != Enumere.CoperTimbre && (t.TOPANNUL != "O" || t.TOPANNUL == null)).Sum(g => g.MONTANT);

                    TotTranscaisse = TotTranscaisse == null ? 0 : TotTranscaisse;
                    TotTranscaisB = TotTranscaisB == null ? 0 : TotTranscaisB;
                    return (TotTranscaisse + TotTranscaisB);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static decimal? RetourneSoldeClientDate(int fk_idcentre, string centre, string client, string ordre,DateTime? date)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    decimal? sommeDebit = null;
                    decimal? sommeCredit = null;
                    decimal? paiement = null;

                    IEnumerable<LCLIENT> _lclientDebiteur = context.LCLIENT.Where(cl => cl.CENTRE == centre && cl.CLIENT == client && cl.ORDRE == ordre && cl.DC == Enumere.Debit && cl.DENR <= date && cl.TOP1 != "2");
                    if (_lclientDebiteur != null)
                        sommeDebit = _lclientDebiteur.GroupBy(cl => new { cl.CENTRE, cl.CLIENT, cl.ORDRE, cl.DC }).Select(cl => cl.Sum(x => x.MONTANT).Value).FirstOrDefault();
                    else
                        sommeDebit = 0;
                    
                    IEnumerable<LCLIENT> _lclientCrediteur = context.LCLIENT.Where(cl => cl.CENTRE  == centre && cl.CENTRE  == client && cl.ORDRE  == ordre && cl.DC == Enumere.Credit && cl.DENR <= date && cl.TOP1  != "2");
                    if (_lclientCrediteur != null)
                        sommeCredit = _lclientDebiteur.GroupBy(cl => new { cl.CENTRE , cl.CLIENT , cl.ORDRE , cl.DC }).Select(cl => cl.Sum(x => x.MONTANT).Value).FirstOrDefault();
                    else
                        sommeCredit = 0;

                    IEnumerable<TRANSCAISB> _paiementLclient = context.TRANSCAISB.Where(cl => cl.CLIENT  == centre && cl.CLIENT  == client && cl.ORDRE  == ordre && cl.TOPANNUL == null && cl.DTRANS <= date);
                    if (_paiementLclient != null)
                        paiement = _paiementLclient.GroupBy(p => new { p.CENTRE  , p.CLIENT , p.ORDRE  }).Select(p => p.Sum(x => (Char.Parse(x.DC) - 'D') * x.MONTANT - ('C' - Char.Parse(x.DC)) * x.MONTANT) +
                                                                                                              p.Sum(x => (Char.Parse(x.DC) - 'D') * x.ECART - ('C' - Char.Parse(x.DC)) * x.ECART)).FirstOrDefault();
                    else
                        paiement = 0;
                    
                    return (sommeDebit - sommeCredit - paiement );
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static decimal? RetourneSoldeClientDate(int ? Pk_Id, DateTime? date)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {

                    string centre = string.Empty;
                    string client  = string.Empty;
                    string ordre = string.Empty;

                    decimal? sommeDebit = null;
                    decimal? sommeCredit = null;
                    decimal? paiement = null;

                    List <LCLIENT> _lclientDebiteur = context.LCLIENT.Where(cl => cl.FK_IDCLIENT == Pk_Id  && cl.DC == Enumere.Debit && cl.DENR <= date && cl.TOP1 != "2").ToList();
                    if (_lclientDebiteur != null)
                    {
                        centre = _lclientDebiteur[0].CENTRE;
                        client = _lclientDebiteur[0].CLIENT;
                        ordre = _lclientDebiteur[0].ORDRE;
                        sommeDebit = _lclientDebiteur.GroupBy(cl => new { cl.FK_IDCLIENT, cl.DC }).Select(cl => cl.Sum(x => x.MONTANT).Value).FirstOrDefault();
                    }
                    else
                        sommeDebit = 0;

                    IEnumerable<LCLIENT> _lclientCrediteur = context.LCLIENT.Where(cl => cl.FK_IDCLIENT == Pk_Id && cl.DC == Enumere.Credit && cl.DENR <= date && cl.TOP1 != "2");
                    if (_lclientCrediteur != null)
                        sommeCredit = _lclientDebiteur.GroupBy(cl => new { cl.FK_IDCLIENT , cl.DC }).Select(cl => cl.Sum(x => x.MONTANT).Value).FirstOrDefault();
                    else
                        sommeCredit = 0;

                    IEnumerable<TRANSCAISB> _paiementLclient = context.TRANSCAISB.Where(cl => cl.CLIENT == centre && cl.CLIENT == client && cl.ORDRE == ordre && cl.TOPANNUL == null && cl.DTRANS <= date);
                    if (_paiementLclient != null)
                        paiement = _paiementLclient.GroupBy(p => new { p.CENTRE, p.CLIENT, p.ORDRE }).Select(p => p.Sum(x => (Char.Parse(x.DC) - 'D') * x.MONTANT - ('C' - Char.Parse(x.DC)) * x.MONTANT) +
                                                                                                              p.Sum(x => (Char.Parse(x.DC) - 'D') * x.ECART - ('C' - Char.Parse(x.DC)) * x.ECART)).FirstOrDefault();
                    else
                        paiement = 0;

                    return (sommeDebit - sommeCredit - paiement);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static decimal? RetourneSoldeDocument(int? Fk_idCentre, string centre, string client, string ordre, string ndoc, string refem)
        {
            try
            {
                decimal? sommeDebit = RetourneDebitFacture(Fk_idCentre, centre, client, ordre, ndoc, refem);
                decimal? sommeCrebit = RetourneCrebitFacture(Fk_idCentre, centre, client, ordre, ndoc, refem);
                decimal? sommeAnnulation = RetourneCrebitFactureNegative(Fk_idCentre, centre, client, ordre, ndoc, refem);
                
                decimal? difference = (sommeDebit - (sommeCrebit + sommeAnnulation));
                return difference;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static decimal? RetourneSoldeDocument(CsLclient lstFacture ,List<CsLclient> lstReglement)
        {
            try
            {
                decimal? sommeDebit = lstFacture.MONTANT ;
                decimal? sommeCrebit = lstReglement.Sum(t => t.MONTANT);
                decimal? difference = (sommeDebit - sommeCrebit);
                return difference;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static decimal? RetourneDebitFacture(int? Fk_idCentre, string centre, string client, string ordre, string ndoc, string refem)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    decimal? TotTranscaisB = context.LCLIENT .Where(t => t.FK_IDCENTRE == Fk_idCentre && t.CENTRE == centre && t.CLIENT == client && t.ORDRE == ordre && t.NDOC == ndoc && t.REFEM == refem).Sum(g => g.MONTANT);
                    return (TotTranscaisB == null ? 0 : TotTranscaisB);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static decimal? RetourneCrebitFacture(int? Fk_idCentre,string centre,string client,string ordre, string ndoc, string refem)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    decimal? TotTranscaisse = context.TRANSCAISSE.Where(t => t.FK_IDCENTRE == Fk_idCentre && t.CENTRE == centre && 
                                                                       t.CLIENT == client && t.ORDRE == ordre && t.NDOC == ndoc &&
                                                                       t.REFEM == refem && t.COPER != Enumere.CoperTimbre && (t.TOPANNUL != "O" || t.TOPANNUL == null)).Sum(g => g.MONTANT);
                    decimal? TotTranscaisB = context.TRANSCAISB.Where(t => t.FK_IDCENTRE == Fk_idCentre && t.CENTRE == centre && 
                                                                      t.CLIENT == client && t.ORDRE == ordre && t.NDOC == ndoc &&
                                                                      t.REFEM == refem && t.COPER != Enumere.CoperTimbre && (t.TOPANNUL != "O" || t.TOPANNUL == null)).Sum(g => g.MONTANT);

                    TotTranscaisse = TotTranscaisse == null ? 0 : TotTranscaisse;
                    TotTranscaisB = TotTranscaisB == null ? 0 : TotTranscaisB;
                    return (TotTranscaisse + TotTranscaisB);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static decimal? RetourneCrebitFactureNegative(int? Fk_idCentre, string centre, string client, string ordre, string ndoc, string refem)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    decimal? TotAnnulation= context.LCLIENT .Where(t => t.FK_IDCENTRE == Fk_idCentre && t.CENTRE == centre &&
                                                                       t.CLIENT == client && t.ORDRE == ordre && t.NDOC == ndoc &&
                                                                       t.REFEM == refem  && t.MONTANT <0 ).Sum(g => g.MONTANT);

                    TotAnnulation = TotAnnulation == null ? 0 : TotAnnulation;
                    return TotAnnulation * (-1);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static decimal? RetourneMontantPayeDuRecuClient(int idcentre, string centre, string client, string ordre, int? idcaisse, string acquit, string matricule)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    decimal? sommeCredit = null;
                    List<TRANSCAISB>  _ExisteInTrnscaissB = new List<TRANSCAISB>();
                     var existeInTranscaissB = context.TRANSCAISB.Where(tr =>tr.FK_IDCENTRE == idcentre && tr.CENTRE == centre && tr.CLIENT == client && tr.ORDRE == ordre && tr.ACQUIT ==acquit && tr.FK_IDCAISSIERE ==idcaisse);
                     if (existeInTranscaissB != null )
                         sommeCredit = existeInTranscaissB.Where(p => string.IsNullOrEmpty( p.TOPANNUL)).Sum(t => t.MONTANT);
                     return sommeCredit;
                    }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static decimal? RetourneMontantPayeAcquit(int idcaisse, string acquit, string matricule)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    decimal? sommeCredit = null;
                    List<TRANSCAISB> _ExisteInTrnscaissB = new List<TRANSCAISB>();
                    var existeInTranscaissB = context.TRANSCAISB.Where(tr => tr.ACQUIT == acquit && tr.FK_IDCAISSIERE == idcaisse);
                    if (existeInTranscaissB != null)
                        sommeCredit = existeInTranscaissB.Where(p => !string.IsNullOrEmpty(p.TOPANNUL)).Sum(t => t.MONTANT);
                    return sommeCredit;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static decimal? RetourneSoldeNAF(string centre, string client, string ordre, string ndoc)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    decimal? sommeDebit = null;
                    decimal? sommeCredit = null;

                    var _ndoc = context.LCLIENT.Where(cl => cl.CENTRE  == centre && cl.CLIENT  == client && cl.ORDRE  == ordre && cl.NDOC == ndoc && (cl.COPER  == Enumere.CoperNAF || (cl.DC == Enumere.Debit && cl.MONTANT <0)));
                    if (_ndoc == null)
                        return  0;

                    var _lclientDebiteur = context.LCLIENT.Where(cl => cl.CENTRE  == centre && cl.CLIENT  == client && cl.ORDRE  == ordre && cl.NDOC == ndoc && cl.DC == Enumere.Debit);
                    if (_lclientDebiteur != null)
                        sommeDebit = _lclientDebiteur.Sum(d=> d.MONTANT);
                    else
                        sommeDebit = 0;

                    var _lclientCrediteur = context.LCLIENT.Where(cl => cl.CENTRE  == centre && cl.CLIENT  == client && cl.ORDRE  == ordre && cl.NDOC == ndoc && cl.DC == Enumere.Credit);
                    if (_lclientCrediteur != null)
                        sommeCredit = _lclientCrediteur.Sum(cl => cl.MONTANT);
                    else
                        sommeCredit = 0;
                    sommeDebit = sommeDebit == null ? 0 : sommeDebit;
                    sommeCredit = sommeCredit == null ? 0 : sommeCredit;
                    decimal? difference = (sommeDebit - sommeCredit);
                    return difference;

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static decimal? RetourneSoldeNAF(int Ppid, string ndoc)
        {
            try
            {
                List<LCLIENT> facture = LigneFactureClient(Ppid);
                    decimal? sommeDebit = null;
                    decimal? sommeCredit = null;

                    var _ndoc = facture.Where(cl => cl.NDOC == ndoc  && (cl.COPER == Enumere.CoperNAF || (cl.DC == Enumere.Debit && cl.MONTANT < 0)));
                    if (_ndoc == null)
                        return 0;

                    var _lclientDebiteur = facture.Where(cl => cl.NDOC == ndoc && cl.DC == Enumere.Debit);
                    if (_lclientDebiteur != null)
                        sommeDebit = _lclientDebiteur.Sum(d => d.MONTANT);
                    else
                        sommeDebit = 0;

                    var _lclientCrediteur = facture.Where(cl =>  cl.NDOC == ndoc  && cl.DC == Enumere.Credit);
                    if (_lclientCrediteur != null)
                        sommeCredit = _lclientCrediteur.Sum(cl => cl.MONTANT);
                    else
                        sommeCredit = 0;
                    sommeDebit = sommeDebit == null ? 0 : sommeDebit;
                    sommeCredit = sommeCredit == null ? 0 : sommeCredit;
                    decimal? difference = (sommeDebit - sommeCredit);
                   
                return difference;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        static List<LCLIENT> LigneFactureClient(int? pFforeignkey)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    var ligne = context.LCLIENT.Where(f => f.FK_IDCLIENT == pFforeignkey);
                    return ligne.ToList();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        static List<TRANSCAISB> LigneReglementClotureClient(int? pfkIdCentre ,string Centre,string client,string Ordre)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    var ligne = context.TRANSCAISB .Where(f => f.CENTRE  == Centre && f.CLIENT == client && f.ORDRE == Ordre 
                        //&& f.FK_IDCENTRE == pfkIdCentre
                        );
                    return ligne.ToList();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        static List<LCLIENT> LigneFactureClientByNDOC(int? pFforeignkey ,string ndoc)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    var ligne = context.LCLIENT.Where(f => f.FK_IDCLIENT == pFforeignkey);
                    return ligne.ToList();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static string RetourneDernierNumeroRecu(int idCaisse )
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {

                    string _acquitC = context.CAISSE.Where(a => a.PK_ID == idCaisse).First().ACQUIT;
                    return _acquitC;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static int RetourneEtatCaisse(string  matricule, int? Fk_IdCaisse)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                        {
                            var clientT = context.TRANSCAISSE.Where(c => c.MATRICULE == matricule && c.DTRANS < DateTime.Today.Date  && c.SAISIPAR == null && c.COPER != Enumere.CoperAjsutemenFondCaisse && c.COPER != Enumere.CoperFondCaisse);
                            if (clientT != null && clientT.Count() > 0)
                                if (!string.IsNullOrEmpty(clientT.First().CLIENT))
                                    return 3;


                            var HabilCaisse = context.HABILITATIONCAISSE.Where(c => c.MATRICULE == matricule && c.DATE_DEBUT < DateTime.Today.Date  && c.DATE_FIN == null);
                            if (HabilCaisse != null && HabilCaisse.Count() > 0)
                                return 3;

                         /* CAISSE DEJA ARRETE*/
                            //IEnumerable<TRANSCAISB> clientB = context.TRANSCAISB.Where(c => c.MATRICULE == matricule && c.HABILITATIONCAISSE.FK_IDCAISSE == Fk_IdCaisse && c.DTRANS == DateTime.Today && c.SAISIPAR == null && c.FK_IDHABILITATIONCAISSE != null);
                            //c'etait fait pour gerer la mobilité de caisse donc on verifie la caisse du centre mais a cause des problem au demarage je commente 01/10/2016
                            IEnumerable<TRANSCAISB> clientB = context.TRANSCAISB.Where(c => c.MATRICULE == matricule &&  c.DTRANS == DateTime.Today.Date  && c.SAISIPAR == null && c.FK_IDHABILITATIONCAISSE != null);
                            if (clientB != null && clientB.Count() > 0 )
                                if (!string.IsNullOrEmpty(clientB.First().CLIENT))
                                    return 2;

                            /*Caisse ouverte a la demande*/
                            var clientO = context.OPENINGDAY.Where(c => c.KEYEDBY == matricule && c.OPEN == Enumere.CaisseStatOuverte);
                            if (clientO != null &&  clientO.Count() > 0)
                                if (!string.IsNullOrEmpty(clientO.First().KEYEDBY))
                                    return 4;

                            /*journée précédente non arretée*/


                         }

                return 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static CsHabilitationCaisse  RetourneCaisseEnCours(string matricule)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    var HabilCaisse = context.HABILITATIONCAISSE.Where(c => c.MATRICULE == matricule && c.DATE_DEBUT == DateTime.Today && c.DATE_FIN == null);
                    if (HabilCaisse != null && HabilCaisse.Count() > 0)
                        return Galatee.Tools.Utility.ConvertEntity < CsHabilitationCaisse,HABILITATIONCAISSE>(HabilCaisse.FirstOrDefault());
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static decimal? RetourneSoldeInitialDocumentCampagne(string centre, string client, string ordre, string ndoc, string campagne)
        {
            using (galadbEntities context = new galadbEntities())
            {
                decimal? sommeDebit = null;
                decimal? sommeCredit = null;

                var _lclientDebiteur = context.LCLIENT.Where(cl => cl.CENTRE  == centre && cl.CLIENT  == client && cl.ORDRE  == ordre && cl.NDOC == ndoc && cl.DC == Enumere.Debit && cl.IDCOUPURE  == campagne);
                if (_lclientDebiteur != null && _lclientDebiteur.Count() > 0)
                    sommeDebit = _lclientDebiteur.Sum(cl => cl.MONTANT);
                else
                    sommeDebit = 0;
                var _lclientCrediteur = context.LCLIENT.Where(cl => cl.CENTRE  == centre && cl.CLIENT  == client && cl.ORDRE  == ordre && cl.NDOC == ndoc && cl.DC == Enumere.Credit && cl.IDCOUPURE  == campagne);
                if (_lclientCrediteur != null && _lclientCrediteur.Count() > 0)
                    sommeCredit = _lclientCrediteur.Sum(cl => cl.MONTANT);
                else
                    sommeCredit = 0;

                sommeDebit = sommeDebit == null ? 0 : sommeDebit;
                sommeCredit = sommeCredit == null ? 0 : sommeCredit;

                decimal? difference = (sommeDebit - sommeCredit);
                return (difference < 0) ? 0 : difference;

            }
        }


        public static decimal? RetourneSoldeClient(int? Fk_idCentre, string centre, string client, string ordre, string REFEM)
        {
            try
            {
                decimal? sommeDebit = RetourneDebitClient(Fk_idCentre, centre, client, ordre, REFEM);
                decimal? sommeCrebit = RetourneCrebitClient(Fk_idCentre, centre, client, ordre, REFEM);
                decimal? difference = (sommeDebit - sommeCrebit);
                return difference;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }




        public static decimal? RetourneDebitClient(int? Fk_idCentre, string centre, string client, string ordre, string REFEM)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    decimal? TotTranscaisB = context.LCLIENT.Where(t => t.FK_IDCENTRE == Fk_idCentre && t.CENTRE == centre && t.CLIENT == client && t.ORDRE == ordre && t.REFEM == REFEM).Sum(g => g.MONTANT);
                    return (TotTranscaisB == null ? 0 : TotTranscaisB);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }




        public static decimal? RetourneCrebitClient(int? Fk_idCentre, string centre, string client, string ordre, string REFEM)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    decimal? TotTranscaisse = context.TRANSCAISSE.Where(t => t.FK_IDCENTRE == Fk_idCentre && t.CENTRE == centre &&
                                                                       t.CLIENT == client && t.ORDRE == ordre && t.COPER != Enumere.CoperTimbre && (t.TOPANNUL != "O" || t.TOPANNUL == null) && t.REFEM == REFEM).Sum(g => g.MONTANT);
                    decimal? TotTranscaisB = context.TRANSCAISB.Where(t => t.FK_IDCENTRE == Fk_idCentre && t.CENTRE == centre &&
                                                                      t.CLIENT == client && t.ORDRE == ordre && t.COPER != Enumere.CoperTimbre && (t.TOPANNUL != "O" || t.TOPANNUL == null) && t.REFEM == REFEM).Sum(g => g.MONTANT);

                    TotTranscaisse = TotTranscaisse == null ? 0 : TotTranscaisse;
                    TotTranscaisB = TotTranscaisB == null ? 0 : TotTranscaisB;
                    return (TotTranscaisse + TotTranscaisB);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }




        //public static decimal? RetourneSoldeDocument(int? Fk_idCentre, string centre, string client, string ordre, string ndoc, string refem)
        //{
        //    try
        //    {
        //        decimal? sommeDebit = RetourneDebitFacture(Fk_idCentre, centre, client, ordre, ndoc, refem);
        //        decimal? sommeCrebit = RetourneCrebitFacture(Fk_idCentre, centre, client, ordre, ndoc, refem);
        //        decimal? difference = (sommeDebit - sommeCrebit);
        //        return difference;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
    }
}
