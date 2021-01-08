using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Collections ;
using Galatee.Structure;
//using Galatee.DataAccess;
using System.Data.Objects;




namespace Galatee.Entity.Model
{
    public static class EserviceProcedure
    {
        #region Envoi Facture


        //Pioche dans ABO07
        public static DataTable ListeDesClientPourEnvoieMailRegroupement(int IdClient, List<string> lstPeriode, List<string> lstRegroupement, bool sms, bool email)
        {
            try
            {
                using (ABO07Entities context = new ABO07Entities())
                {
                    var _LeCENTFAC = context.CENTFAC;
                    var query = (
                                             from x in _LeCENTFAC
                                             where
                                             (lstPeriode == null || lstPeriode.Contains(x.PERIODE)) &&
                                             (IdClient == 0 || x.FK_IDCLIENT == IdClient) &&
                                             (lstRegroupement != null && lstRegroupement.Contains(x.REGROUPEMENT )) && 
                                             (x.ISFACTUREEMAIL != null || x.ISFACTURESMS != null) && 
                                             (x.EMAIL != null || x.TELEPHONE != null) 
                                             select new
                                             {
                                                 x.CENTRE,
                                                 x.CLIENT,
                                                 x.ORDRE,
                                                 NomClient = x.NOMABON,
                                                 x.PERIODE,
                                                 montant = x.TOTFTTC,
                                                 numfact = x.FACTURE,
                                                 ISFACTURE = x.ISFACTUREEMAIL.Value,
                                                 ISSMS = x.ISFACTURESMS.Value ,
                                                 x.EMAIL ,
                                                 x.TELEPHONE
                                             });
                    if (!sms && !email && query != null)
                    {
                        query = (from x in query
                                 where x.ISFACTURE || x.ISSMS
                                 select x);
                    }
                    else if (sms && query != null)
                    {
                        query = (from x in query
                                 where x.ISSMS
                                 select x);
                    }
                    else if (email && query != null)
                    {
                        query = (from x in query
                                 where x.ISFACTURE
                                 select x);
                    }
                    return Galatee.Tools.Utility.ListToDataTable(query.ToList());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }
        public static DataTable  ListeDesClientPourEnvoieMail(List<int> CentreClient, List<string> lstPeriode, bool sms, bool email)
        {
            try
            {
                using (ABO07Entities context = new ABO07Entities())
                {

                    var _LeCENTFAC = context.CENTFAC;
                    IEnumerable<object> query =
                    from x in _LeCENTFAC
                    where CentreClient.Contains(x.FK_IDCENTRE) && lstPeriode.Contains(x.PERIODE)
                    && ((x.ISFACTUREEMAIL != null && x.ISFACTUREEMAIL != false ) || (x.ISFACTURESMS != null && x.ISFACTURESMS != false ))
                    select new
                    {
                        x.CENTRE,
                        x.CLIENT,
                        x.ORDRE,
                        x.FACTURE,
                        x.PERIODE,
                        x.REGROUPEMENT,
                        x.CODCONSO,
                        x.CATEGORIECLIENT,
                        x.DFAC,
                        x.TOTFTAX,
                        x.TOTFHT,
                        x.TOURNEE,
                        //x.MOISCOMPTA,
                        x.COMMUNE,
                        //x.DATEENC,
                        //x.TYPEENC,
                        x.LOTRI,
                        x.COPER,
                        //x.MODEP,
                        x.ANCIENREPORT,
                        x.TOTFTTC,
                        x.LIENFAC,
                        x.SECTEUR,
                        x.QUARTIER,
                        x.ORDTOUR,
                        x.NOMABON,
                        x.RUE ,
                        x.NOMRUE,
                        x.DRESABON,
                        //x.DATEFLAG,
                        x.USERCREATION,
                        x.DATECREATION,
                        x.USERMODIFICATION,
                        x.DATEMODIFICATION,
                        x.PK_ID,
                        x.FK_IDCENTRE,
                        x.FK_IDREGROUPEMENT ,
                        x.FK_IDCODECONSOMMATEUR,
                        x.FK_IDCATEGORIECLIENT,
                        x.FK_IDMODEPAIEMENT,
                        x.FK_IDTOURNEE,
                        x.FK_IDCOMMUNE,
                        x.FK_IDCOPER,
                        x.FK_IDSECTEUR,
                        x.FK_IDQUARTIER,
                        x.FK_IDRUE,
                        x.ISFACTUREEMAIL,
                        x.EMAIL,
                        x.ISFACTURESMS,
                        x.TELEPHONE,
                        x.FK_IDCLIENT
                    };
                    return Galatee.Tools.Utility.ListToDataTable(query);


                    //var _LeCENTFAC = context.CENTFAC;
                    //   IEnumerable<object> query =(
                    //                         from x in _LeCENTFAC
                    //                         where
                    //                         (lstPeriode == null || lstPeriode.Contains(x.PERIODE)) &&
                    //                         (CentreClient == null || CentreClient.Contains(x.FK_IDCENTRE )) &&
                    //                         (x.ISFACTUREEMAIL != null || x.ISFACTURESMS != null) && 
                    //                         ( x.EMAIL != null || x.TELEPHONE!=null)
                    //                         select new
                    //                         {
                    //                             x.CENTRE,
                    //                             x.CLIENT,
                    //                             x.ORDRE,
                    //                             NomClient = x.NOMABON,
                    //                             x.PERIODE,
                    //                             montant = x.TOTFTTC,
                    //                             numfact = x.FACTURE,
                    //                             ISFACTURE=x.ISFACTUREEMAIL ,
                    //                             ISSMS=    x.ISFACTURESMS 
                    //                             ,x.EMAIL
                    //                             ,x.TELEPHONE
                    //                         });
                    ////if (!sms && !email && query!=null)
                    ////{
                    ////    query = (from x in query
                    ////            where  x.ISFACTURE || x.ISSMS 
                    ////            select x);
                    ////}
                    ////else if (sms && query != null)
                    ////{
                    ////    query = (from x in query
                    ////             where x.ISSMS
                    ////             select x);
                    ////}
                    ////else if (email && query != null)
                    ////{
                    ////    query = (from x in query
                    ////             where x.ISFACTURE
                    ////             select x);
                    ////}
                    //return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }


     


        public static DataTable ListeDesClientPourEnvoieMailDiffusion(int pidCentre, int pidSite, int pidCategorie, int pidTourne, bool sms, bool email)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    var _Ag = context.AG ;
                    var query = (
                                             from t in _Ag
                                             from x in t.CLIENT1 
                                             from br in t.BRT 
                                             where
                                             (t.FK_IDTOURNEE  == pidTourne || pidTourne  == 0) &&
                                             (x.FK_IDCENTRE  == pidCentre || pidCentre  == 0)
                                                 && (x.CENTRE1.SITE.PK_ID  == pidSite || pidSite  == 0) &&
                                             (x.FK_IDCATEGORIE  == pidCategorie || pidCategorie == 0)
                                             && (x.EMAIL != null || x.TELEPHONE != null)
                                             select new
                                             {
                                                 x.CENTRE,
                                                 client=x.REFCLIENT,
                                                 numfact="",
                                                 periode="",
                                                 x.ORDRE,
                                                 NomClient = x.NOMABON,
                                                 x.EMAIL
                                                 ,
                                                 x.ISFACTUREEMAIL,
                                                 x.ISFACTURESMS,
                                                 x.TELEPHONE
                                             });
                    var lst = query.ToList();
                    if (!sms && !email && query != null)
                    {
                        query = (from x in query
                                 where x.ISFACTUREEMAIL.Value || x.ISFACTURESMS.Value
                                 select x);
                    }
                    else if (sms && query != null)
                    {
                        query = (from x in query
                                 where x.ISFACTURESMS.Value
                                 select x);
                    }
                    else if (email && query != null)
                    {
                        query = (from x in query
                                 where x.ISFACTUREEMAIL.Value
                                 select x);
                    }
                    return Galatee.Tools.Utility.ListToDataTable(query.ToList());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }

        //Pioche dans ABO07
        public static DataTable ListeDesFactures(string centre, string client, string ordre, string periode, string numFacture)
        {


            try
            {
                using (ABO07Entities context = new ABO07Entities())
                {
                    List<object> query = null;
                    using (galadbEntities context1 = new galadbEntities())
                    {
                        CENTFAC MyCENFAC = new CENTFAC();

                        var _LeCENTFAC = context.CENTFAC;
                        var _LCPROFAC = context.CPROFAC;
                        var _leCREDFAC = context.CREDFAC;
                        var _laREDVANCE = context1.REDEVANCE;
                        var _lEVENEMENT = context1.EVENEMENT;
                        //var _ = context1.ENTFAC;
                        var _REDFACT = context1.REDFAC;
                        var _ENTFAC = context1.ENTFAC;
                        var _PROFAC = context1.PROFAC;
                        var _AG = context1.AG;
                        #region
                        if (_leCREDFAC.FirstOrDefault(d => d.CENTRE == centre && d.CLIENT == client && d.FACTURE == numFacture && d.PERIODE == periode) != null)
                        {
                            query.Add(ListeFactureABO07(centre, client, ordre, periode, numFacture));
                        }
                        #endregion

                        #region
                        else
                        {
                            //query = ListeFacturegaladb(centre, client, ordre, periode, numFacture, query, _LeCENTFAC);
                        }
                        #endregion

                    }
                    return Galatee.Tools.Utility.ListToDataTable(query);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }



        }

        public static List<CENTFAC> ListeFactureABO07(string centre, string client, string ordre, string periode, string numFacture)
        {
           


            //RECUPE DE L'ENTETE DE FACTURE
            #region RECUPE DE L'ENTETE DE FACTURE

            ABO07Entities c = new ABO07Entities();
            var query_ = (from d in c.CENTFAC
                          where (d.CENTRE == centre &&
                                 d.CLIENT == client &&
                                 d.ORDRE == ordre &&
                                 (d.FACTURE == numFacture || string.IsNullOrEmpty(numFacture)) &&
                                 (d.PERIODE == periode || string.IsNullOrEmpty(periode)))
                          select d);


            return (List<CENTFAC>)query_.ToList();

            #endregion

            ////UPDATE DE LA STRUCTURE CsLafactureClient
            //#region UPDATE DE LA STRUCTURE CsLafactureClient
            //     CsLafactureClient _tt = new CsLafactureClient();
            //     //_tt = Galatee.Entity.Model.EserviceProcedure.ListeFactureABO07(centre, client, ordre, periode, numFacture);
            //    //Galatee.Entity.Model.ENTFAC dt_ = Galatee.Entity.Model.EserviceProcedure.ListeFacturegaladb(centre, client, ordre, periode, numFacture);

            //     _tt._LeEntatfac = Entities.ConvertObject<CsEnteteFacture, Galatee.Entity.Model.CENTFAC>(dt);
            //     _tt._LstRedFact = Entities.ConvertObject<List<CsRedevanceFacture>, IEnumerable<Galatee.Entity.Model.CREDFAC>>(dt.CREDFAC.ToList());

            //     _tt._LstProfact = Entities.ConvertObject<List< CsProduitFacture>,IEnumerable<Galatee.Entity.Model.CPROFAC>>(dt.CPROFAC.ToList());


            //                List<CsRedevance> lestranche =ListeDesREdevance();
            //    if (_tt._Ls
            //        foreach (CsRedevanceFacture item in _tt._LstRedFact)
            //        {
            //            CsRedevance laTranchRech = lestranche.FirstOrDefault(p => p.NUMREDEVANCE == item.Redevance && item.Produit == p.FK_PRODUIT && item.Tranche == p.TRANCHE);
            //            if (laTranchRech != null)
            //            {
            //                item.Produit = laTranchRech.FK_PRODUIT;
            //                item.LibelleRedevance = laTranchRech.LIBELLE;
            //            }
            //        }
            //        List<CsProduitFacture> lesProduitFacture = ListeDesProduit();
            //        foreach (var item in _tt._LstProfact)
            //        {
            //            CsProduitFacture leProduit = lesProduitFacture.FirstOrDefault(p => p.Pk_Id == item.Pk_Id && item.Compteur == p.Compteur && item.Centre == p.Centre);
            //            if tRedFact != null && _tt._LstRedFact.Count > 0)
            //    {(leProduit != null)
            //            {
            //                item.Produit = leProduit.Produit;
                           
            //            }
            //        }

            //        List<CsCategorieClient> lescategorieclient = ListeDesCategorieClient();

            //        CsCategorieClient lacategorieclient = lescategorieclient.FirstOrDefault(p => p.PK_CATEGORIECLIENT == _tt._LeEntatfac.Catcli);
            //        if (lacategorieclient != null)
            //            {
            //                _tt._LeEntatfac.Catcli = lacategorieclient.LIBELLE;

            //            }
                   
            //    }
            //    #endregion
            //    return _tt;
            //};
        }

        public static List<CENTFAC> ListeFactureDuplicata(string centre, string client, string ordre, string periode, string numFacture)
        {
            //using (ABO07Entities c = new ABO07Entities())
            //{


            //RECUPE DE L'ENTETE DE FACTURE
            #region RECUPE DE L'ENTETE DE FACTURE
            string Pperiode = string.IsNullOrEmpty(periode) ? "" : periode;
            string PnumFacture = string.IsNullOrEmpty(numFacture) ? "" : numFacture;
            string vide = "";

            ABO07Entities c = new ABO07Entities();
            var query_ = (from d in c.CENTFAC
                          where (d.CENTRE == centre &&
                                 d.CLIENT == client &&
                                 d.ORDRE == ordre &&
                                 (d.FACTURE == numFacture || PnumFacture == vide) &&
                                 (d.PERIODE == periode || Pperiode == vide))
                        

                          select d).ToList();


            return (List<CENTFAC>)query_;

            #endregion

            ////UPDATE DE LA STRUCTURE CsLafactureClient
            //#region UPDATE DE LA STRUCTURE CsLafactureClient
            //     CsLafactureClient _tt = new CsLafactureClient();
            //     //_tt = Galatee.Entity.Model.EserviceProcedure.ListeFactureABO07(centre, client, ordre, periode, numFacture);
            //    //Galatee.Entity.Model.ENTFAC dt_ = Galatee.Entity.Model.EserviceProcedure.ListeFacturegaladb(centre, client, ordre, periode, numFacture);

            //     _tt._LeEntatfac = Entities.ConvertObject<CsEnteteFacture, Galatee.Entity.Model.CENTFAC>(dt);
            //     _tt._LstRedFact = Entities.ConvertObject<List<CsRedevanceFacture>, IEnumerable<Galatee.Entity.Model.CREDFAC>>(dt.CREDFAC.ToList());

            //     _tt._LstProfact = Entities.ConvertObject<List< CsProduitFacture>,IEnumerable<Galatee.Entity.Model.CPROFAC>>(dt.CPROFAC.ToList());


            //                List<CsRedevance> lestranche =ListeDesREdevance();
            //    if (_tt._Ls
            //        foreach (CsRedevanceFacture item in _tt._LstRedFact)
            //        {
            //            CsRedevance laTranchRech = lestranche.FirstOrDefault(p => p.NUMREDEVANCE == item.Redevance && item.Produit == p.FK_PRODUIT && item.Tranche == p.TRANCHE);
            //            if (laTranchRech != null)
            //            {
            //                item.Produit = laTranchRech.FK_PRODUIT;
            //                item.LibelleRedevance = laTranchRech.LIBELLE;
            //            }
            //        }
            //        List<CsProduitFacture> lesProduitFacture = ListeDesProduit();
            //        foreach (var item in _tt._LstProfact)
            //        {
            //            CsProduitFacture leProduit = lesProduitFacture.FirstOrDefault(p => p.Pk_Id == item.Pk_Id && item.Compteur == p.Compteur && item.Centre == p.Centre);
            //            if tRedFact != null && _tt._LstRedFact.Count > 0)
            //    {(leProduit != null)
            //            {
            //                item.Produit = leProduit.Produit;

            //            }
            //        }

            //        List<CsCategorieClient> lescategorieclient = ListeDesCategorieClient();

            //        CsCategorieClient lacategorieclient = lescategorieclient.FirstOrDefault(p => p.PK_CATEGORIECLIENT == _tt._LeEntatfac.Catcli);
            //        if (lacategorieclient != null)
            //            {
            //                _tt._LeEntatfac.Catcli = lacategorieclient.LIBELLE;

            //            }

            //    }
            //    #endregion
            //    return _tt;
            //};
        }
        //Pioche dans galadb
        public static ENTFAC ListeFacturegaladb(string centre, string client, string ordre, string periode, string numFacture)
        {
            //using (ABO07Entities c = new ABO07Entities())
            //{
            galadbEntities c = new galadbEntities();
            var query_ = (from d in c.ENTFAC
                          where (d.CENTRE == centre && d.CLIENT    == client && d.ORDRE  == ordre && d.FACTURE  == numFacture && d.PERIODE  == periode)
                          //||
                          //        (d.CENTRE == centre && d.CLIENT == client && d.ORDRE == ordre && int.Parse(d.PERIODE) <= int.Parse(periode))

                          select d).FirstOrDefault();
            return (ENTFAC)query_;
            //};
        }
        //private static IEnumerable<CsFactureClient> ListeFactureABO07(string centre, string client, string ordre, string periode, string numFacture, IEnumerable<object> query, System.Data.Entity.DbSet<CENTFAC> _LeCENTFAC)
        //{
        //    query = from d in _LeCENTFAC

        //            where (d.CENTRE == centre && d.CLIENT == client && d.ORDRE == ordre && d.FACTURE == numFacture && d.PERIODE == periode) ||
        //                    (d.CENTRE == centre && d.CLIENT == client && d.ORDRE == ordre && int.Parse(d.PERIODE) <= int.Parse(periode))

        //            select new
        //            {
        //                d.CREDFAC,
        //                d.CPROFAC
        //            };
        //    return null;
        //    //join a in _LeCENTFAC on new { d.CENTRE, d.CLIENT, d.ORDRE }
        //    //equals new { CENTRE = a.CENTRE, a.CLIENT, a.ORDRE } into pTemp1
        //    //from t in pTemp1.DefaultIfEmpty()

        //    //join k in _LCPROFAC on new { d.CENTRE, d.CLIENT, d.ORDRE, d.PRODUIT }
        //    //equals new { CENTRE = k.CENTRE, CLIENT = k.CLIENT, ORDRE = k.ORDRE, PRODUIT = k.PRODUIT }

        //    //join b in _laREDVANCE on new { d.REDEVANCE, d.PRODUIT }
        //    //equals new { REDEVANCE = b.REDEVANCE1,  PRODUIT = b.FK_PRODUIT } into pTemp2
        //    //from y in pTemp2.DefaultIfEmpty()
        //    ////where (new List<int> { 3, 4, 99 }.FirstOrDefault(i => i.ToString() == b.STATUS.ToString()) != null)

        //    //where (d.CENTRE == centre && d.CLIENT == client && d.ORDRE == ordre && d.FACTURE == numFacture && d.PERIODE == periode) ||
        //    //(d.CENTRE == centre && d.CLIENT == client && d.ORDRE == ordre && int.Parse(d.PERIODE) <= int.Parse(periode))

        //    //select new
        //    //{
        //    //    d.CENTRE,
        //    //    d.CLIENT,
        //    //    d.ORDRE,
        //    //    d.PRODUIT,
        //    //    d.FACTURE,
        //    //    d.PERIODE,
        //    //    d.TRANCHE,
        //    //    d.REDEVANCE,
        //    //    d.QUANTITE,
        //    //    d.BARPRIX,
        //    //    d.REDHT,
        //    //    d.REDTAXE,
        //    //    t.NOMABON,
        //    //    k.AINDEX,
        //    //    k.NINDEX,
        //    //    k.CONSOFAC,
        //    //    k.DIAMETRE,
        //    //    k.COMPTEUR,
        //    //    y.LIBELLE
        //    //};
        //    //return query;
        //}

        //Pioche dans galadb
        
        public static DataTable ListeDesREdevance()
        {
            try
            {
                using (galadbEntities context1 = new galadbEntities())
                {
                    string vide = "";

                    //var _REDEVANCE = context1.REDEVANCE;
                    //IEnumerable<object> query = from r in _REDEVANCE
                    //                            select new {                                              
                    //                                       r.CENTRE
                    //                                      ,r.PRODUIT 
                    //                                      ,r.CODE  
                    //                                      ,r.TRANCHE
                    //                                      ,r.LIBELLE                                                          
                    //                                      ,r.TRANS 
                    //                                      ,r.NATURECLI 
                    //                                      ,r.GRATUIT 
                    //                                      ,r.EXONERATION
                    //                                      ,r.TYPELIEN
                    //                                      ,r.PARAM1 
                    //                                      ,r.PARAM2 
                    //                                      ,r.PARAM3 
                    //                                      ,r.PARAM4 
                    //                                      ,r.PARAM5 
                    //                                      ,r.PARAM6 
                    //                                      ,r.EDITEE 
                    //                                      ,r.PK_ID 
                    //                                      ,r.DATECREATION 
                    //                                      ,r.DATEMODIFICATION 
                    //                                      ,r.USERCREATION 
                    //                                      ,r.USERMODIFICATION 

                    //                            };

                    //return Galatee.Tools.Utility.ListToDataTable(query);
                    return new DataTable();
                }
            }
            catch (Exception ex)
            {

                throw;
            }

        #endregion
        }

        //public static List<CsCategorieClient> ListeDesCategorieClient()
        //{
        //    try
        //    {
        //        using (galadbEntities context1 = new galadbEntities())
        //        {
        //            string vide = "";

        //            var _CATEGORIECLIENT = context1.CATEGORIECLIENT;
        //            IEnumerable<object> query = from r in _CATEGORIECLIENT
        //                                        select r; 

                                                

        //            return (List<CsCategorieClient>)query;
        //        }
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}

        //public static List<CsProduitFacture> ListeDesProduit()
        //{
        //    try
        //    {
        //        using (galadbEntities context1 = new galadbEntities())
        //        {
        //            string vide = "";

        //            var _PROFAC = context1.PRODUIT;
        //            IEnumerable<object> query = from p in _PROFAC
        //                                        select p; 

                                                

        //            return (List<CsProduitFacture>)query;
        //        }
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }




        //}

        #region SMS

        public static DataTable SMS_RETOURNEByStatutEnvoiNombreEnvoi(int pStatutEnvoi, int pNombreEnvoi)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {

                    IEnumerable<object> query = from sms in context.SMS
                                                where sms.STATUTENVOI == pStatutEnvoi && sms.NOMBREENVOI <= pNombreEnvoi
                                                select sms;
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
