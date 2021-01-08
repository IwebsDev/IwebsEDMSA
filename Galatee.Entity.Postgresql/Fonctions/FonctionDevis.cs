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
    public class FonctionDevis
    {
        public static decimal RetourneMontantRemboursableHT(string pNumDevis)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    decimal MontantRemboursableHorsTaxe = 0;

                    var r = from d in context.DEMANDE
                            join el in context.ELEMENTDEVIS on d.NUMDEM equals el.NUMDEM
                            //join f in context.FOURNITURE on el.NUMFOURNITURE equals f.CODE 
                            where
                              el.QUANTITEREMISENSTOCK > 0 &&
                              el.NUMDEM == pNumDevis
                            select d;
                    if (r != null && r.FirstOrDefault() != null)
                    {
                        CsEtatBonDeSortie q = (from e in context.ELEMENTDEVIS
                                               where
                                                 e.QUANTITEREMISENSTOCK > 0 &&
                                                 e.NUMDEM == pNumDevis
                                               group new { e, e.NUMFOURNITURE } by new
                                               {
                                                   e.NUMDEM
                                               } into g
                                               select new CsEtatBonDeSortie { TotalRemboursableHT = (decimal)g.Sum(p => p.e.QUANTITEREMISENSTOCK * 1  /* p.e.FOURNITURE.COUTUNITAIRE_FOURNITURE*/ ) }).FirstOrDefault();
                        if (q != null && q.TotalRemboursableHT != null)
                            MontantRemboursableHorsTaxe = (decimal)q.TotalRemboursableHT;
                    }
                    return MontantRemboursableHorsTaxe;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

      public static decimal RetourneMontantTotalDevis(string pNumDevis)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    decimal MontantTotalDevis = 0;
                    DEMANDE devis = context.DEMANDE.FirstOrDefault(t => t.NUMDEM == pNumDevis);
                    //if (devis != null && !string.IsNullOrEmpty(devis.NUMDEM))
                    //    MontantTotalDevis = devis.MONTANTTTC.Value ;
                    return MontantTotalDevis;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static decimal RetourneMontantDevis(string pNumDevis, bool pIsSummary)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    decimal MontantTotalDevis = 0;
                    var query = from e in context.ELEMENTDEVIS
                                join f in context.FOURNITURE on e.FK_IDFOURNITURE  equals f.PK_ID into temp
                                where e.NUMDEM  == pNumDevis
                                from tp in temp
                                where tp.ISSUMMARY == pIsSummary 
                                //&& (tp.ISDEFAULT != true  )
                                select new
                                {
                                    e.NUMDEM ,
                                    //Montant = (e.QUANTITE * e.FOURNITURE.COUTUNITAIRE_FOURNITURE + (e.QUANTITE * e.FOURNITURE.COUTUNITAIRE_FOURNITURE) * e.TAXE)
                                };
                    if (query != null && query.FirstOrDefault() != null)
                    {
                        var devis = from t in query
                                    group t by t.NUMDEM  into total
                                    select new CsEtatBonDeSortie
                                    {
                                        //montantTTC = total.Sum(p => p.Montant)
                                    };
                        if (devis != null && devis.FirstOrDefault() !=null)
                        {
                            var r = devis.First();
                            if (r != null && r.montantTTC != null)
                                MontantTotalDevis = (decimal)r.montantTTC;
                        }
                    }
                    return MontantTotalDevis;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static decimal RetourneMontantDevisPaye(ObjDEVIS ODevis)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    decimal MontantDeposit = 0;
                    decimal MontantTranscaisse = 0;
                    decimal MontantTranscaisb = 0;
                    //Montant du Deposit
                    var queryDeposit = from d in context.DEPOSIT where d.CENTRE == ODevis.CODECENTRE && d.NUMDEM == ODevis.NUMDEVIS  && d.FK_IDCENTRE == ODevis.FK_IDCENTRE  
                                       select d.MONTANTDEPOSIT;
                    var resultDepodit = queryDeposit.FirstOrDefault();
                    if (resultDepodit != null)
                        MontantDeposit = resultDepodit.Value;
                    // Montant de Transcaisse
                    var queryTranscaisse = from t in context.TRANSCAISSE
                                           where t.CENTRE == ODevis.CODECENTRE && t.NUMDEM == ODevis.NUMDEVIS && t.FK_IDCENTRE == ODevis.FK_IDCENTRE  
                                           select t.MONTANT;
                    var resultTranscaisse = queryTranscaisse.FirstOrDefault();
                    if (resultTranscaisse != null)
                        MontantTranscaisse = resultTranscaisse.Value;
                    // Montant de Transcaisb
                    var queryTranscaisb = from tb in context.TRANSCAISB
                                          where tb.CENTRE == ODevis.CODECENTRE && tb.NUMDEM == ODevis.NUMDEVIS && tb.FK_IDCENTRE == ODevis.FK_IDCENTRE  
                                          select tb.MONTANT;
                    var resultTranscaisb = queryTranscaisb.FirstOrDefault();
                    if (resultTranscaisb != null)
                        MontantTranscaisb = resultTranscaisb.Value;

                    return MontantDeposit + MontantTranscaisse + MontantTranscaisse;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static decimal RetourneMontantRemboursableHT(int pDevisId)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    decimal MontantRemboursableHorsTaxe = 0;

                    var r = from d in context.DEMANDE
                            join el in context.ELEMENTDEVIS on d.PK_ID equals el.FK_IDDEMANDE
                            where
                              el.QUANTITEREMISENSTOCK > 0 &&
                              el.PK_ID == pDevisId
                            select d;
                    if (r != null && r.FirstOrDefault() != null)
                    {
                        CsEtatBonDeSortie q = (from e in context.ELEMENTDEVIS
                                               where
                                                 e.QUANTITEREMISENSTOCK > 0 &&
                                                 e.FK_IDDEMANDE == pDevisId
                                               group new { e, e.FK_IDFOURNITURE } by new
                                               {
                                                   e.NUMDEM
                                               } into g
                                               select new CsEtatBonDeSortie { TotalRemboursableHT = (decimal)g.Sum(p => p.e.QUANTITEREMISENSTOCK * 1 /*p.e.FOURNITURE.COUTUNITAIRE_FOURNITURE*/ ) }).FirstOrDefault();
                        if (q != null && q.TotalRemboursableHT != null)
                            MontantRemboursableHorsTaxe = (decimal)q.TotalRemboursableHT;
                    }
                    return MontantRemboursableHorsTaxe;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static decimal RetourneMontantDevis(int pDevisId, bool pIsSummary)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    decimal MontantTotalDevis = 0;
                    var query = from e in context.ELEMENTDEVIS
                                join f in context.FOURNITURE on e.FK_IDFOURNITURE equals f.PK_ID into temp
                                where e.PK_ID == pDevisId
                                from tp in temp
                                where tp.ISSUMMARY == pIsSummary
                                select new
                                {
                                    e.FK_IDDEMANDE,
                                    //Montant = (e.QUANTITE * e.FOURNITURE.COUTUNITAIRE_FOURNITURE + (e.QUANTITE * e.FOURNITURE.COUTUNITAIRE_FOURNITURE) * e.TAXE)
                                };
                    if (query != null && query.FirstOrDefault() != null)
                    {
                        var devis = from t in query
                                    group t by t.FK_IDDEMANDE into total
                                    select new CsEtatBonDeSortie
                                    {
                                        //montantTTC = total.Sum(p => p.Montant)
                                    };
                        if (devis != null && devis.FirstOrDefault() != null)
                        {
                            var r = devis.First();
                            if (r != null && r.montantTTC != null)
                                MontantTotalDevis = (decimal)r.montantTTC;
                        }
                    }
                    return MontantTotalDevis;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public static float RetourneMontantDevis(string pNumDevis)
        //{
        //    try
        //    {
        //        using (galadbEntities context = new galadbEntities())
        //        {
        //            float MontantTotalDevis = 0;
        //            var query = from e in context.ELEMENTDEVIS
        //                        join f in context.FOURNITURE on e.PK_FK_NUMFOURNITURE equals f.PK_FK_NUMFOURNITURE into temp
        //                        where e.PK_FK_NUMDEVIS == pNumDevis
        //                        from tp in temp
        //                        where tp.ISSUMMARY == true
        //                        select new
        //                        {
        //                            e.PK_FK_NUMDEVIS,
        //                            Montant = (e.QUANTITE * e.FOURNITURE.PRIX_UNITAIRE + (e.QUANTITE * e.FOURNITURE.PRIX_UNITAIRE) * e.TAXE)
        //                        };
        //            if (query != null)
        //            {
        //                var devis = from t in query
        //                            group t by t.PK_FK_NUMDEVIS into total
        //                            select new CsEtatBonDeSortie
        //                            {
        //                                montantTTC = total.Sum(p => p.Montant)
        //                            };
        //                if (devis != null)
        //                {
        //                    MontantTotalDevis = (float)devis.First().montantTTC;
        //                }
        //            }
        //            return MontantTotalDevis;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
    }
}
