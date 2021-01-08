using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Galatee.Structure;
using System.Data;

namespace Galatee.Entity.Model.Reports
{
    public static class ReportsProcedures
    {
        public static int ParseInt(string number)
        {
            return Int32.Parse(number);
        }

        /* En stand by, problement avec le calcul des DC
         * char.parse() n'est pas reconnu par linq to entities */


        #region :Encaissement
        public static List<TRANSCAISB> EtatEncaissement(string MatriculeAgentConnecte)
        {
            try
            {
                IEnumerable<TRANSCAISB> InofTransCaisse = GetEtatEncaissementByMatricule(MatriculeAgentConnecte);
                if (InofTransCaisse == null)
                {
                    return null;
                }
                //return InofTransCaisse.ToList();
                return InofTransCaisse.Where(tr =>
                                            (tr.ADMUTILISATEUR.MATRICULE.Equals(MatriculeAgentConnecte)) &&
                                            (tr.COPER != Enumere.CoperFondCaisse)).ToList();
                #region
                // (tr.ADMUTILISATEUR.MATRICULE.Equals(MatriculeAgentConnecte))
                //&& (tr.COPER != Enumere.CoperFondCaisse) &&
                //(!tr.DATEENCAISSEMENT.HasValue)).OrderBy(t => new { t.ACQUIT, t.DTRANS, t.MODEREG1.LIBELLE }).ToList(); 








                //using (galadbEntities context = new galadbEntities())
                //{
                //    var Trancaisse = context.TRANSCAISSE;
                //    var Client = context.CLIENT;
                //    var Utilisateur = context.ADMUTILISATEUR;
                //    var ModeReglement = context.MODEREG;
                //    var Banque = context.BANQUE;                   

                //    // ON recupere tous les enregistrement puis on les groupe dans une autre requete
                //    // ceci parce que certaines syntaxes ne sont pas disponibles dans une requete linq to entities
                //    var reglements =
                //        from tr in Trancaisse
                //        join cl in Client on tr.CLIENT equals cl.REFCLIENT  
                //        join ma in Utilisateur on tr.FK_MATRICULE equals ma.PK_MATRICULE
                //        join mr in ModeReglement on tr.FK_MODEREG equals mr.CODEMODEREG
                //        join b in Banque on tr.BANQUE equals b.BANQUE

                //        where
                //            (tr.FK_MATRICULE.Equals(MatriculeAgentConnecte)) &&
                //            (tr.FK_COPER != "103") &&
                //            (!tr.DATEENCAISSEMENT.HasValue)

                //        select new 
                //        {
                //            centre = tr.CENTRE,
                //            client = tr.CLIENT,
                //            ordre =  tr.ORDRE,
                //            nomClient = cl.NOMABON,
                //            acquit = ma.FK_NUMCAISSE + " " + tr.ACQUIT + " " + tr.FK_MATRICULE,
                //            nomAgentConnecte = ma.LIBELLE,
                //            matricule = tr.FK_MATRICULE,
                //            dtrans = tr.DTRANS,
                //            modeReg = mr.CODEMODEREG,
                //            topAnnul = tr.TOPANNUL,
                //            numeroCheque = tr.NUMCHEQ,
                //            banque = b.BANQUE,                            

                //            // Champs utilisé pour faire la somme du groupe par la suite
                //            DC = tr.DC,
                //            ecart = tr.ECART,
                //            Montant = tr.MONTANT,
                //            montantPaye = 0,
                //            soldeFacture = 0                            
                //        };



                //    IEnumerable<CsReglement> reglets = reglements.
                //        GroupBy(r => new
                //        {
                //            r.centre,
                //            r.client,
                //            r.ordre,
                //            r.acquit,
                //            r.matricule,
                //            r.dtrans,
                //            r.modeReg,
                //            r.topAnnul,
                //            r.numeroCheque,
                //        }).
                //        Select(t => new CsReglement
                //        {
                //            CENTRE = t.Key.centre,
                //            CLIENT = t.Key.client,
                //            ORDRE = t.Key.ordre,
                //            NOMCLIENT = t.Key.nomClient,
                //            ACQUIT = t.Key.acquit,
                //            NOMCAISSIERE = t.Key.nomAgentConnecte,
                //            FK_MATRICULE = t.Key.matricule,
                //            DTRANS = t.Key.dtrans,
                //            FK_MODEREG = t.Key.modeReg,
                //            TOPANNUL = t.Key.topAnnul,
                //            NUMCHEQ = t.Key.numeroCheque,
                //            BANQUE = t.Key.banque,

                //            MONTANTPAYE = t.Sum(x => (Char.Parse(x.DC) - 'D') * x.Montant - ('C' - Char.Parse(x.DC)) * x.Montant) +
                //                t.Sum(x => (Char.Parse(x.DC) - 'D') * x.ecart - ('C' - Char.Parse(x.DC)) * x.ecart),

                //            SOLDEFACTURE = FonctionCaisse.RetourneSoldeClient(t.Key.centre, t.Key.client, t.Key.ordre)
                //        }).
                //        OrderBy(t => new { t.ACQUIT, t.DTRANS, t.LIBELLEMODREG });                          


                //    return reglets.ToList();
                //}
                #endregion

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static List<TRANSCAISB> GetEtatEncaissementByMatricule(string MatriculeAgentConnecte)
        {
            using (galadbEntities context = new galadbEntities())
            {

                //var _util = context.ADMUTILISATEUR ;
                //IEnumerable<object> query =
                //from _Leutil in _util
                //where _Leutil.MATRICULE == MatriculeAgentConnecte
                //select
                //   _Leutil.TRANSCAISB;

                //return (List<TRANSCAISB>)query;





                ADMUTILISATEUR Utilisateur = context.ADMUTILISATEUR.FirstOrDefault(p => p.MATRICULE == MatriculeAgentConnecte);
                var query_ = (from d in Utilisateur.TRANSCAISB
                              select d).ToList();
                if (query_.Count > 0)
                {

                    return (List<TRANSCAISB>)query_;
                }


                //    //return (List<TRANSCAISB>)query_.GroupBy(r => new
                //    //{
                //    //    r.CENTRE,
                //    //    r.CLIENT,
                //    //    r.ORDRE,
                //    //    r.ACQUIT,
                //    //    r.MATRICULE,
                //    //    r.DTRANS,
                //    //    r.MODEREG,
                //    //    r.TOPANNUL,
                //    //    r.NUMCHEQ,
                //    //});



                //}
                else
                {
                    return null;
                }


            };
        }

        public static List<TRANSCAISB> EtatEncaissementParAgent(string MatriculeAgentConnecte, DateTime? collectionDate, int mois, int annee)
        {
            string vide = "";
            List<TRANSCAISB> ListeFinale = new List<TRANSCAISB>();
            try
            {
                using (galadbEntities context = new galadbEntities())
                {


                    //ADMUTILISATEUR Utilisateur = context.ADMUTILISATEUR.FirstOrDefault(p => p.MATRICULE == MatriculeAgentConnecte);
                    //var query_ = (from d in Utilisateur.TRANSCAISB
                    //              select d).ToList();

                    var trc = context.TRANSCAISB;
                    var query_ = (
                        from u in trc
                        where u.MATRICULE == MatriculeAgentConnecte && (u.DTRANS.Value == collectionDate ||
                                                                       (u.DTRANS.Value.Year == annee && u.DTRANS.Value.Month == mois))
                        select u).ToList();
                    if (query_.Count > 0)
                    {

                        return (List<TRANSCAISB>)query_;
                    }
                }

                return ListeFinale;


                #region

                //if (GetEtatEncaissementByMatricule(MatriculeAgentConnecte)!=null)
                //{
                //                  ListeFinale=  GetEtatEncaissementByMatricule(MatriculeAgentConnecte).Where(tr =>
                //            (tr.ADMUTILISATEUR.MATRICULE.Equals(MatriculeAgentConnecte)) &&
                //            (collectionDate.Equals(vide) || ((tr.DATEENCAISSEMENT == null && tr.DTRANS.ToString() == collectionDate) || (tr.DATEENCAISSEMENT == null && tr.DATEENCAISSEMENT.ToString() == collectionDate))) &&
                //            (tr.TOPANNUL == null || (tr.DATEENCAISSEMENT == null && tr.TOPANNUL != null)) &&

                //            (annee.Equals(vide) || ((tr.DATEENCAISSEMENT == null && tr.DTRANS.Value.Year.ToString() == annee) || (tr.DATEENCAISSEMENT != null && tr.DATEENCAISSEMENT.Value.Year.ToString() == annee))) &&
                //            ((tr.TOPANNUL == null || (tr.DATEENCAISSEMENT == null && tr.TOPANNUL != null))) &&

                //            (mois.Equals(vide) || ((tr.DATEENCAISSEMENT == null && tr.DTRANS.Value.Month.ToString() == mois) || (tr.DATEENCAISSEMENT != null && tr.DATEENCAISSEMENT.Value.Month.ToString() == mois))) &&
                //            ((tr.TOPANNUL == null || (tr.DATEENCAISSEMENT == null && tr.TOPANNUL != null)))).OrderBy(t => new { t.ACQUIT, t.DTRANS, t.MODEREG1.LIBELLE }).ToList();
                //}
                //else
                //{
                //    ListeFinale = null;
                //}




                //using (galadbEntities context = new galadbEntities())
                //{
                //    var TrancaisseB = context.TRANSCAISB;
                //    var Client = context.CLIENT;
                //    var Utilisateur = context.ADMUTILISATEUR;
                //    var ModeReglement = context.MODEREG;
                //    var Banque = context.BANQUE;

                //    MatriculeAgentConnecte = string.IsNullOrEmpty(MatriculeAgentConnecte) ? "" : MatriculeAgentConnecte;
                //    DateTime? _collectionDate = string.IsNullOrEmpty(collectionDate) ? null : (DateTime?)DateTime.Parse(collectionDate);
                //    int _mois = string.IsNullOrEmpty(mois) ? -1 : Int32.Parse(mois);
                //    int _annee = string.IsNullOrEmpty(annee) ? -1 : Int32.Parse(annee);
                //    string vide = "";

                //    // ON recupere tous les enregistrement puis on les groupe dans une autre requete
                //    // ceci parce que certaines syntaxes ne sont pas disponibles dans une requete linq to entities
                //    var reglements =
                //        from tr in TrancaisseB
                //        join cl in Client on tr.CLIENT equals cl.REFCLIENT
                //        join ma in Utilisateur on tr.FK_MATRICULE equals ma.PK_MATRICULE
                //        join mr in ModeReglement on tr.FK_MODEREG equals mr.PK_MODEREG
                //        join b in Banque on new {banque = tr.FK_BANQUE,guichet = tr.FK_GUICHET } equals new { banque = b.PK_BANQUE, guichet = b.PK_GUICHET }

                //        where
                //            (tr.FK_MATRICULE.Equals(MatriculeAgentConnecte)) &&
                //            (collectionDate.Equals(vide) || ((tr.DATEENCAISSEMENT == null && tr.DTRANS == _collectionDate) || (tr.DATEENCAISSEMENT == null && tr.DATEENCAISSEMENT == _collectionDate))) && 
                //            (tr.TOPANNUL == null || (tr.DATEENCAISSEMENT == null && tr.TOPANNUL != null)) &&

                //            (annee.Equals(vide) || ((tr.DATEENCAISSEMENT == null && tr.DTRANS.Value.Year == _annee) || (tr.DATEENCAISSEMENT != null && tr.DATEENCAISSEMENT.Value.Year == _annee))) &&
                //            ((tr.TOPANNUL == null || (tr.DATEENCAISSEMENT == null && tr.TOPANNUL != null))) && 

                //            (mois.Equals(vide) || ((tr.DATEENCAISSEMENT == null && tr.DTRANS.Value.Month == _mois) || (tr.DATEENCAISSEMENT != null && tr.DATEENCAISSEMENT.Value.Month == _mois))) &&
                //            ((tr.TOPANNUL == null || (tr.DATEENCAISSEMENT == null && tr.TOPANNUL != null)))

                //        select new
                //        {
                //            centre = tr.CENTRE,
                //            client = tr.CLIENT,
                //            ordre = tr.ORDRE,
                //            nomClient = cl.NOMABON,
                //            acquit = ma.FK_NUMCAISSE + " " + tr.ACQUIT + " " + tr.FK_MATRICULE,
                //            nomAgentConnecte = ma.LIBELLE,
                //            caisse = ma.FK_NUMCAISSE,
                //            matricule = tr.FK_MATRICULE,
                //            dtrans = tr.DTRANS,
                //            modeReg = mr.PK_MODEREG,
                //            libelleModeReg = mr.LIBELLE,
                //            topAnnul = tr.TOPANNUL,
                //            numeroCheque = tr.NUMCHEQ,
                //            banque = b.PK_BANQUE,
                //            banqueLibelle = b.LIBELLE,

                //            // Champs utilisé pour faire la somme du groupe par la suite
                //            DC = tr.DC,
                //            ecart = tr.ECART,
                //            Montant = tr.MONTANT,
                //            montantPaye = 0,
                //            soldeFacture = 0
                //        };

                //    IEnumerable<CsReglement> reglets = reglements.
                //        GroupBy(r => new
                //        {
                //            r.centre,
                //            r.client,
                //            r.ordre,
                //            r.nomClient,
                //            r.acquit,
                //            r.nomAgentConnecte,
                //            r.caisse,
                //            r.matricule,
                //            r.dtrans,
                //            r.libelleModeReg,
                //            r.topAnnul,
                //            r.numeroCheque,
                //            r.banqueLibelle
                //        }).
                //        Select(t => new CsReglement
                //        {
                //            CENTRE = t.Key.centre,
                //            CLIENT = t.Key.client,
                //            ORDRE = t.Key.ordre,
                //            NOMCLIENT = t.Key.nomClient,
                //            ACQUIT = t.Key.acquit,
                //            NOMCAISSIERE = t.Key.nomAgentConnecte,
                //            NumCaiss = t.Key.caisse,
                //            FK_MATRICULE = t.Key.matricule,
                //            ZONE = t.Key.dtrans.Value.ToShortDateString(),
                //            LIBELLEMODREG = t.Key.libelleModeReg,
                //            TOPANNUL = t.Key.topAnnul,
                //            NUMCHEQ = t.Key.numeroCheque,
                //            BANQUE = t.Key.banqueLibelle,

                //            MONTANTFACTURE = t.Key.topAnnul == null ? 0 : t.Sum(x => (Char.Parse(x.DC) - 'D') * x.Montant - ('C' - Char.Parse(x.DC)) * x.Montant) +
                //                t.Sum(x => (Char.Parse(x.DC) - 'D') * x.ecart - ('C' - Char.Parse(x.DC)) * x.ecart),

                //            SOLDEFACTURE = t.Key.topAnnul == null ? 0 : FonctionCaisse.RetourneSoldeClientDate(t.Key.centre, t.Key.client, t.Key.ordre, t.Key.dtrans)
                //        }).
                //        OrderBy(t => new { t.ACQUIT, t.DTRANS, t.LIBELLEMODREG });

                //    return reglets.ToList();
                //}
                //return null;
                #endregion
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public static List<TRANSCAISSE> EtatEncaissementParAgent_current(string MatriculeAgentConnecte, DateTime? collectionDate, int mois, int annee)
        {
            string vide = "";
            List<TRANSCAISSE> ListeFinale = new List<TRANSCAISSE>();
            try
            {
                using (galadbEntities context = new galadbEntities())
                {


                    //ADMUTILISATEUR Utilisateur = context.ADMUTILISATEUR.FirstOrDefault(p => p.MATRICULE == MatriculeAgentConnecte);
                    //var query_ = (from d in Utilisateur.TRANSCAISB
                    //              select d).ToList();




                    var trc = context.TRANSCAISSE;
                    var query_ = (
                        from u in trc
                        where u.MATRICULE == MatriculeAgentConnecte && (u.DTRANS.Value == collectionDate ||
                                                                       (u.DTRANS.Value.Year == annee && u.DTRANS.Value.Month == mois))
                        select u).ToList();
                    if (query_.Count > 0)
                    {

                        return (List<TRANSCAISSE>)query_;
                    }
                }




                //if (GetEtatEncaissementByMatricule(MatriculeAgentConnecte)!=null)
                //{
                //                  ListeFinale=  GetEtatEncaissementByMatricule(MatriculeAgentConnecte).Where(tr =>
                //            (tr.ADMUTILISATEUR.MATRICULE.Equals(MatriculeAgentConnecte)) &&
                //            (collectionDate.Equals(vide) || ((tr.DATEENCAISSEMENT == null && tr.DTRANS.ToString() == collectionDate) || (tr.DATEENCAISSEMENT == null && tr.DATEENCAISSEMENT.ToString() == collectionDate))) &&
                //            (tr.TOPANNUL == null || (tr.DATEENCAISSEMENT == null && tr.TOPANNUL != null)) &&

                //            (annee.Equals(vide) || ((tr.DATEENCAISSEMENT == null && tr.DTRANS.Value.Year.ToString() == annee) || (tr.DATEENCAISSEMENT != null && tr.DATEENCAISSEMENT.Value.Year.ToString() == annee))) &&
                //            ((tr.TOPANNUL == null || (tr.DATEENCAISSEMENT == null && tr.TOPANNUL != null))) &&

                //            (mois.Equals(vide) || ((tr.DATEENCAISSEMENT == null && tr.DTRANS.Value.Month.ToString() == mois) || (tr.DATEENCAISSEMENT != null && tr.DATEENCAISSEMENT.Value.Month.ToString() == mois))) &&
                //            ((tr.TOPANNUL == null || (tr.DATEENCAISSEMENT == null && tr.TOPANNUL != null)))).OrderBy(t => new { t.ACQUIT, t.DTRANS, t.MODEREG1.LIBELLE }).ToList();
                //}
                //else
                //{
                //    ListeFinale = null;
                //}


                return ListeFinale;

                //using (galadbEntities context = new galadbEntities())
                //{
                //    var TrancaisseB = context.TRANSCAISB;
                //    var Client = context.CLIENT;
                //    var Utilisateur = context.ADMUTILISATEUR;
                //    var ModeReglement = context.MODEREG;
                //    var Banque = context.BANQUE;

                //    MatriculeAgentConnecte = string.IsNullOrEmpty(MatriculeAgentConnecte) ? "" : MatriculeAgentConnecte;
                //    DateTime? _collectionDate = string.IsNullOrEmpty(collectionDate) ? null : (DateTime?)DateTime.Parse(collectionDate);
                //    int _mois = string.IsNullOrEmpty(mois) ? -1 : Int32.Parse(mois);
                //    int _annee = string.IsNullOrEmpty(annee) ? -1 : Int32.Parse(annee);
                //    string vide = "";

                //    // ON recupere tous les enregistrement puis on les groupe dans une autre requete
                //    // ceci parce que certaines syntaxes ne sont pas disponibles dans une requete linq to entities
                //    var reglements =
                //        from tr in TrancaisseB
                //        join cl in Client on tr.CLIENT equals cl.REFCLIENT
                //        join ma in Utilisateur on tr.FK_MATRICULE equals ma.PK_MATRICULE
                //        join mr in ModeReglement on tr.FK_MODEREG equals mr.PK_MODEREG
                //        join b in Banque on new {banque = tr.FK_BANQUE,guichet = tr.FK_GUICHET } equals new { banque = b.PK_BANQUE, guichet = b.PK_GUICHET }

                //        where
                //            (tr.FK_MATRICULE.Equals(MatriculeAgentConnecte)) &&
                //            (collectionDate.Equals(vide) || ((tr.DATEENCAISSEMENT == null && tr.DTRANS == _collectionDate) || (tr.DATEENCAISSEMENT == null && tr.DATEENCAISSEMENT == _collectionDate))) && 
                //            (tr.TOPANNUL == null || (tr.DATEENCAISSEMENT == null && tr.TOPANNUL != null)) &&

                //            (annee.Equals(vide) || ((tr.DATEENCAISSEMENT == null && tr.DTRANS.Value.Year == _annee) || (tr.DATEENCAISSEMENT != null && tr.DATEENCAISSEMENT.Value.Year == _annee))) &&
                //            ((tr.TOPANNUL == null || (tr.DATEENCAISSEMENT == null && tr.TOPANNUL != null))) && 

                //            (mois.Equals(vide) || ((tr.DATEENCAISSEMENT == null && tr.DTRANS.Value.Month == _mois) || (tr.DATEENCAISSEMENT != null && tr.DATEENCAISSEMENT.Value.Month == _mois))) &&
                //            ((tr.TOPANNUL == null || (tr.DATEENCAISSEMENT == null && tr.TOPANNUL != null)))

                //        select new
                //        {
                //            centre = tr.CENTRE,
                //            client = tr.CLIENT,
                //            ordre = tr.ORDRE,
                //            nomClient = cl.NOMABON,
                //            acquit = ma.FK_NUMCAISSE + " " + tr.ACQUIT + " " + tr.FK_MATRICULE,
                //            nomAgentConnecte = ma.LIBELLE,
                //            caisse = ma.FK_NUMCAISSE,
                //            matricule = tr.FK_MATRICULE,
                //            dtrans = tr.DTRANS,
                //            modeReg = mr.PK_MODEREG,
                //            libelleModeReg = mr.LIBELLE,
                //            topAnnul = tr.TOPANNUL,
                //            numeroCheque = tr.NUMCHEQ,
                //            banque = b.PK_BANQUE,
                //            banqueLibelle = b.LIBELLE,

                //            // Champs utilisé pour faire la somme du groupe par la suite
                //            DC = tr.DC,
                //            ecart = tr.ECART,
                //            Montant = tr.MONTANT,
                //            montantPaye = 0,
                //            soldeFacture = 0
                //        };

                //    IEnumerable<CsReglement> reglets = reglements.
                //        GroupBy(r => new
                //        {
                //            r.centre,
                //            r.client,
                //            r.ordre,
                //            r.nomClient,
                //            r.acquit,
                //            r.nomAgentConnecte,
                //            r.caisse,
                //            r.matricule,
                //            r.dtrans,
                //            r.libelleModeReg,
                //            r.topAnnul,
                //            r.numeroCheque,
                //            r.banqueLibelle
                //        }).
                //        Select(t => new CsReglement
                //        {
                //            CENTRE = t.Key.centre,
                //            CLIENT = t.Key.client,
                //            ORDRE = t.Key.ordre,
                //            NOMCLIENT = t.Key.nomClient,
                //            ACQUIT = t.Key.acquit,
                //            NOMCAISSIERE = t.Key.nomAgentConnecte,
                //            NumCaiss = t.Key.caisse,
                //            FK_MATRICULE = t.Key.matricule,
                //            ZONE = t.Key.dtrans.Value.ToShortDateString(),
                //            LIBELLEMODREG = t.Key.libelleModeReg,
                //            TOPANNUL = t.Key.topAnnul,
                //            NUMCHEQ = t.Key.numeroCheque,
                //            BANQUE = t.Key.banqueLibelle,

                //            MONTANTFACTURE = t.Key.topAnnul == null ? 0 : t.Sum(x => (Char.Parse(x.DC) - 'D') * x.Montant - ('C' - Char.Parse(x.DC)) * x.Montant) +
                //                t.Sum(x => (Char.Parse(x.DC) - 'D') * x.ecart - ('C' - Char.Parse(x.DC)) * x.ecart),

                //            SOLDEFACTURE = t.Key.topAnnul == null ? 0 : FonctionCaisse.RetourneSoldeClientDate(t.Key.centre, t.Key.client, t.Key.ordre, t.Key.dtrans)
                //        }).
                //        OrderBy(t => new { t.ACQUIT, t.DTRANS, t.LIBELLEMODREG });

                //    return reglets.ToList();
                //}
                //return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        #endregion


        #region

        public static DataTable RPT_DECLARATION_TVA(string moisComptable, string Annee)
        {

            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<LCLIENT> ClientListe = context.LCLIENT;
                    IEnumerable<PROFAC> ProfacListe = context.PROFAC;
                    IEnumerable<ENTFAC> EntFacListe = context.ENTFAC;


                    // SELECT CENTRE,CLIENT , ORDRE, REFEM,NDOC, CONVERT(char(35),'') as CATCLI, 
                    //  CONVERT(decimal(18,0),0) as MONTANTHT, CONVERT(decimal(18,0),0) as MONTANTTVA, 
                    //  CONVERT(decimal(18,0),0) as LOCATION, CONVERT(decimal(18,0),0) as ENTRETIEN, 
                    //  CONVERT(decimal(18,0),0) as REDEVANCE, CONVERT(decimal(18,0),0) as MONTANTCONSO INTO #tempA 
                    //  FROM LCLIENT where TOP1 = 2 and  COPER not in ('082','083') and refem > '200106'
                    //and  (@moisComptable is null or MOISCOMPT  = @moisComptable)
                    //and (@Annee is null or datepart(YYYY,DENR) = @Annee)



                    //Recup liste de d'info de facture depuit la table
                    //IEnumerable<object> DeclarationTVA = (from entfac in EntFacListe
                    //                                                from client in ClientListe
                    //                                                where client.TOP1 == "2" &&
                    //                                    !(new List<string> { "082", "083" }.Contains(client.COPER)) &&
                    //                                    int.Parse(client.REFEM) > 201106 &&
                    //                                    (string.IsNullOrWhiteSpace(moisComptable) || client.MOISCOMPT == Annee + moisComptable) &&
                    //                                    (string.IsNullOrWhiteSpace(Annee) || client.DENR.Year.ToString() == Annee) &&
                    //                                    entfac.CLIENT == client.CLIENT && entfac.CENTRE == client.CENTRE && entfac.ORDRE == client.ORDRE && entfac.FACTURE == client.NDOC
                    //                                                     && entfac.PERIODE == client.REFEM
                    //                                                select new CsDeclarationTVA() { Annee = Annee, Categorie = entfac.CATEGORIECLIENT.LIBELLE });


                    IEnumerable<object> DeclarationTVA = (from entfac in EntFacListe
                                                          from client in ClientListe
                                                          where client.TOP1 == "2" &&
                                              !(new List<string> { "082", "083" }.Contains(client.COPER)) &&
                                              int.Parse(client.REFEM) > 201106 &&
                                              (string.IsNullOrWhiteSpace(moisComptable) || client.MOISCOMPT == Annee + moisComptable) &&
                                              (string.IsNullOrWhiteSpace(Annee) || client.DENR.Year.ToString() == Annee) &&
                                              entfac.CLIENT == client.CLIENT && entfac.CENTRE == client.CENTRE && entfac.ORDRE == client.ORDRE && entfac.FACTURE == client.NDOC
                                                               && entfac.PERIODE == client.REFEM
                                                          select new
                                                          {
                                                              Annee = Annee,
                                                              //Categorie = entfac.CATEGORIECLIENT1.LIBELLE ,

                                                          }
                                                          );





                    //Filtrage de des info précédantes avec la fonction  [FnSoldeDocument]{Recupère la redévence du mois pour un client en retournant(debit-credit)}

                    //List<CsDeclarationTVA> listedeclTVA = DeclarationTVA.ToList();
                    var listedeclTVA = DeclarationTVA.ToList();
                    return Galatee.Tools.Utility.ListToDataTable(DeclarationTVA);


                    //Mise a jour finale
                }

            }
            catch (Exception)
            {

                throw;
            }






            //SqlCommand sqlCommand = new SqlCommand();
            //sqlCommand.Connection = sqlConnection;
            //sqlCommand.CommandTimeout = 360;
            //sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
            //sqlCommand.CommandText = "SPX_ETAT_DECLARATION_TVA";
            //sqlCommand.Parameters.Add("@moisComptable", SqlDbType.VarChar, 6).Value = (string.IsNullOrEmpty(moisComptable)) ? null : moisComptable;
            //sqlCommand.Parameters.Add("@Annee", SqlDbType.VarChar, 4).Value = (string.IsNullOrEmpty(Annee)) ? null : Annee;

            //DBBase.SetDBNullParametre(sqlCommand.Parameters);
            //try
            //{
            //    if (sqlConnection.State == ConnectionState.Closed)
            //        sqlConnection.Open();

            //    SqlDataReader reader = sqlCommand.ExecuteReader();

            //    List<CsDeclarationTVA> liste = new List<CsDeclarationTVA>();

            //    #region remplissage de la liste

            //    while (reader.Read())
            //    {
            //        CsDeclarationTVA c = new CsDeclarationTVA();
            //        c.Categorie = (Convert.IsDBNull(reader["CATCLI"])) ? String.Empty : (System.String)reader["CATCLI"];
            //        c.MontantFacture = (Convert.IsDBNull(reader["TOTALHT"])) ? 0 : (System.Decimal)reader["TOTALHT"];
            //        c.MontantTVA = (Convert.IsDBNull(reader["TVA"])) ? 0 : (System.Decimal)reader["TVA"];
            //        c.MontantConso = (Convert.IsDBNull(reader["MONTANTHTCONSO"])) ? 0 : (System.Decimal)reader["MONTANTHTCONSO"];
            //        c.MontantLocation = (Convert.IsDBNull(reader["LOCATIONHT"])) ? 0 : (System.Decimal)reader["LOCATIONHT"];
            //        c.MontantEntretien = (Convert.IsDBNull(reader["ENTRETIENHT"])) ? 0 : (System.Decimal)reader["ENTRETIENHT"];
            //        c.MontantTutelle = (Convert.IsDBNull(reader["REDEVANCEHT"])) ? 0 : (System.Decimal)reader["REDEVANCEHT"];
            //        c.Mois = (Convert.IsDBNull(reader["MOIS"])) ? string.Empty : (System.String)reader["MOIS"];
            //        c.Annee = (Convert.IsDBNull(reader["ANNEE"])) ? string.Empty : (System.String)reader["ANNEE"];

            //        //if (liste.Count < 1024)
            //        liste.Add(c);
            //    }

            //    #endregion
            //    return liste;
            //}
            //catch (Exception ex)
            //{
            //    throw new Exception(ex.Message);
            //}
            //finally
            //{
            //    if (sqlConnection.State == ConnectionState.Open)
            //        sqlConnection.Close(); // Fermeture de la connection 
            //    sqlCommand.Dispose();
            //}
        }




        #endregion


        /*             */

        public static List<CsDeclarationTVA> EtatDeclarationTVA(string moisComptable, string Annee)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    var Lclients = context.LCLIENT;

                    int refem = Int32.Parse("200106");
                    IEnumerable<CsDeclarationTVA> clients =
                        from l in Lclients
                        where l.TOP1 == "2" && (l.COPER != "082" && l.COPER != "083")// && ReportsProcedures.ParseInt(l.REFEM) > refem

                        select new CsDeclarationTVA { };


                    return clients.ToList();

                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static List<CsFacture> RetourneFacturesCategoryDetails(string Categorie, string TypeEdition, string jour, string mois, string annee)
        {
            try
            {
                Categorie = string.IsNullOrEmpty(Categorie) ? "" : Categorie;
                TypeEdition = string.IsNullOrEmpty(TypeEdition) ? "" : TypeEdition;
                int _jour = string.IsNullOrEmpty(jour) ? 0 : Int32.Parse(jour);
                int _mois = string.IsNullOrEmpty(mois) ? 0 : Int32.Parse(mois);
                int _annee = string.IsNullOrEmpty(annee) ? 0 : Int32.Parse(annee);

                string Typeetat = "C";

                string vide = "";
                using (galadbEntities context = new galadbEntities())
                {
                    var Lclients = context.LCLIENT;
                    var Clients = context.CLIENT;
                    var CategorieClient = context.CATEGORIECLIENT;

                    //IEnumerable<object> temp1 =

                    var data1 = from c in Clients
                                join l in Lclients on new { centre = c.CENTRE, client = c.REFCLIENT, ordre = c.ORDRE }
                                equals new { centre = l.CENTRE, client = l.CLIENT, ordre = l.ORDRE }
                                where (c.CATEGORIE.Equals(vide) || c.CATEGORIE.Equals(Categorie)) &&
                                  (_annee == 0 || l.DENR.Year <= _annee) &&
                                  (_jour == 0 || l.DENR.Day <= _jour) &&
                                  (_mois == 0 || l.DENR.Month <= _mois)

                                select new
                                {
                                    categorie = c.CATEGORIE,
                                    centre = c.CENTRE,
                                    client = c.REFCLIENT,
                                    ordre = c.ORDRE,
                                    dc = l.DC,
                                    nomabon = c.NOMABON,
                                    montant = l.MONTANT
                                };

                    var data2 = from temp in data1
                                where temp.dc == "D"
                                select new
                                {
                                    temp.centre,
                                    temp.client,
                                    temp.ordre,
                                    debit = data1.Sum(x => x.montant)
                                };

                    var data3 = from temp in data1
                                where temp.dc == "C"
                                select new
                                {
                                    temp.centre,
                                    temp.client,
                                    temp.ordre,
                                    credit = data1.Sum(x => x.montant)
                                };

                    var data4 = from temp in data2
                                join tmp in data3 on new { centre = temp.centre, client = temp.client, ordre = temp.ordre }
                                equals new { centre = tmp.centre, client = tmp.client, ordre = tmp.ordre }
                                select new
                                {
                                    centre = temp.centre,
                                    client = temp.client,
                                    ordre = temp.ordre,
                                    solde = temp.debit - tmp.credit
                                };

                    IEnumerable<CsFacture> fac = from temp in data4
                                                 join tmp in data1 on new { centre = temp.centre, client = temp.client, ordre = temp.ordre }
                                                 equals new { centre = tmp.centre, client = tmp.client, ordre = tmp.ordre }
                                                 join cat in CategorieClient on new { categorie = tmp.categorie }
                                                 equals new { categorie = cat.CODE }
                                                 orderby temp.centre, temp.client, temp.ordre, cat.CODE
                                                 select new CsFacture
                                                 {
                                                     CENTRE = temp.centre,
                                                     CLIENT = temp.client,
                                                     ORDRE = temp.ordre,
                                                     NOM = tmp.nomabon,
                                                     MONTANTFACTURE = (decimal)temp.solde,
                                                     Categorie = tmp.categorie + ' ' + cat.LIBELLE
                                                 };

                    return fac.ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static List<CsConnexion> ObtenirNouveauxBranchementsElectricite(string annee)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    var Abonnes = context.ABON;
                    var Produit = context.PRODUIT;
                    int _annee = Int32.Parse(annee);
                    //int _periode = Int32.Parse(annee);
                    //int _mois = Int32.Parse(annee);
                    //int _cumul = Int32.Parse(_cumul);

                    //int branchements = Int32.Parse("200106");
                    IEnumerable<CsConnexion> branchements =
                        from a in Abonnes
                        join p in Produit
                        on a.FK_IDPRODUIT equals p.PK_ID
                        where a.DMAJ.Value.Year == _annee
                        select new CsConnexion
                        {
                            Produit = p.LIBELLE,
                            NumMois = a.DMAJ.Value.Month
                        };


                    return branchements.ToList();

                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static DataTable  GetNewBranchementsByProduit(string annee, int IdProduit, bool Etat)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    var Abonnes = context.ABON;
                    annee = annee != string.Empty ? annee : "0";
                    int _annee = Int32.Parse(annee);
                    IEnumerable<object > branchements =
                        from a in Abonnes
                        where
                            (a.PK_ID == IdProduit || IdProduit == 0) &&
                            (a.DABONNEMENT.Value.Year == _annee || _annee == 0) &&
                            ((a.DRES == null) == Etat)
                        select new 
                        {
                            Produit = a.PRODUIT1.LIBELLE,
                            NumMois = a.DABONNEMENT .Value.Month,
                            //Mois = a.DABONNEMENT.Value.Month
                        };
                    return Galatee.Tools.Utility.ListToDataTable(branchements);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static string ToString(int p)
        {
            return p.ToString();
        }

        public static List<CsConnexion> ObtenirBranchementsElectricite(string annee, string mois)
        {
            try
            {

                #region MyRegion
                using (galadbEntities context = new galadbEntities())
                {
                    var Clients = context.CLIENT;
                    var Abonnes = context.ABON;
                    var Canalisations = context.CANALISATION;
                    var Diametres = context.REGLAGECOMPTEUR ;

                    int _annee = Int32.Parse(annee);
                    //int _periode = Int32.Parse(annee);
                    int _mois = Int32.Parse(mois);
                    //int _cumul = Int32.Parse(_cumul);

                    //IEnumerable<CsConnexion> branchements = GetBranchement(Clients, _annee,"03");


                    //.GroupBy(c_=> c_.SetPeriode, c_=>c_.Diametre,c_=>c_.MoisInt,c_=>c_.Libelle);
                    IEnumerable<CsConnexion> branchements =
                     from c in Clients.AsEnumerable()
                     join a in Abonnes.AsEnumerable() on new { centre = c.FK_IDCENTRE.ToString(), client = c.REFCLIENT, ordre = c.ORDRE }
                    equals new { centre = a.FK_IDCENTRE.ToString(), client = a.FK_IDCLIENT.ToString(), ordre = a.ORDRE }
                     join can in Canalisations.AsEnumerable() on new { centre = c.FK_IDCENTRE.ToString(), client = c.REFCLIENT }
                    equals new { centre = can.FK_IDCENTRE.ToString(), client = can.CLIENT.ToString() }
                     join d in Diametres.AsEnumerable() on can.FK_IDREGLAGECOMPTEUR  equals d.PK_ID

                     orderby a.DMAJ.Value.Year, a.DMAJ.Value.Month

                     where

                     //c.BRT.
                         //a.DRES != null &&
                         a.FK_IDPRODUIT.ToString() == "03" &&
                         a.DMAJ.Value.Year == _annee

                     group new { a, can }
                     by new { a.DMAJ.Value.Year, a.DMAJ.Value.Month, can.REGLAGECOMPTEUR , can.FK_IDREGLAGECOMPTEUR  } into abon

                     select new CsConnexion
                     {
                         SetPeriode = abon.Key.Year,
                         MoisInt = abon.Key.Month,
                         Mois = "",
                         Diametre = abon.Key.FK_IDREGLAGECOMPTEUR.ToString(),
                         Libelle = abon.Key.REGLAGECOMPTEUR,
                         Connection = abon.Count()
                     };

                    foreach (var c in branchements)
                    {
                        switch (c.MoisInt)
                        {
                            case 01:
                                c.Mois = "JANUARY";
                                break;
                            case 02:
                                c.Mois = "FEBRUARY";
                                break;
                            case 03:
                                c.Mois = "MARCH";
                                break;
                            case 04:
                                c.Mois = "APRIL";
                                break;
                            case 05:
                                c.Mois = "MAY";
                                break;
                            case 06:
                                c.Mois = "JUNE";
                                break;
                            case 07:
                                c.Mois = "JULY";
                                break;
                            case 08:
                                c.Mois = "AUGUST";
                                break;
                            case 09:
                                c.Mois = "SEPTEMBER";
                                break;
                            case 10:
                                c.Mois = "OCTOBER";
                                break;
                            case 11:
                                c.Mois = "NOVEMBER";
                                break;
                            case 12:
                                c.Mois = "DECEMBER";
                                break;
                        }
                    }
                #endregion
                    return branchements.ToList();

                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private static void GetBranchement(System.Data.Entity.DbSet<CLIENT> Clients, int _annee, string produit = "01")
        {
            //int branchements = Int32.Parse("200106");




            //IEnumerable<CsConnexion> branchements = from c in Clients
            //                                         from a in c.ABON
            //                                         from brt in c.BRT
            //                                         from can in brt.CANALISATION
            //                                         orderby a.DMAJ.Value.Year, a.DMAJ.Value.Month
            //                                         where
            //                                             //a.DRES != null &&
            //                                             a.FK_IDPRODUIT.ToString() == "01" &&
            //                                             a.DMAJ.Value.Year == _annee

            //group new { a, can }
            //by new { a.DMAJ.Value.Year, a.DMAJ.Value.Month, can.FK_IDDIAMETRE, can.DIAMETRE } into abon

            //select new CsConnexion
            //{
            //    SetPeriode = abon.Key.Year,
            //    MoisInt = abon.Key.Month,
            //    Mois = "",
            //    Diametre = abon.Key.FK_IDDIAMETRE.ToString(),
            //    Libelle = abon.Key.DIAMETRE,
            //    Connection = abon.Count()
            //};


            //IEnumerable<CsConnexion> branchements = (from conxion in
            //                                             (from c in Clients
            //                                              where c.ABON.FirstOrDefault(p => p.DABON.Value.Year == _annee) != null
            //                                              select new CsConnexion
            //                                              {
            //                                                  SetPeriode = c.ABON.FirstOrDefault(p => p.PRODUIT == produit).DABON.Value.Year,
            //                                                  Diametre = c.BRT.FirstOrDefault(p => p.PRODUIT == produit).CANALISATION.FirstOrDefault(can => can.PRODUIT == c.BRT.FirstOrDefault(p => p.PRODUIT == produit).PRODUIT).DIACOMPTEUR.DIAMETRE,

            //                                                  MoisInt = c.ABON.FirstOrDefault(p => p.PRODUIT == produit).DABON.Value.Month,
            //                                                  Libelle = c.BRT.FirstOrDefault(p => p.PRODUIT == produit).CANALISATION.FirstOrDefault(can => can.PRODUIT == c.BRT.FirstOrDefault(p => p.PRODUIT == produit).PRODUIT).DIACOMPTEUR.LIBELLE,
            //                                                  Connection = c.ABON.Count()
            //                                              })
            //                                         group new { conxion }
            //                                         by new { conxion.Diametre, conxion.MoisInt, conxion.Libelle } into cnx

            //                                         select new CsConnexion
            //                                         {
            //                                             //SetPeriode = cnx.Key.,
            //                                             MoisInt = cnx.Key.MoisInt,
            //                                             Mois = "",
            //                                             Diametre = cnx.Key.Diametre.ToString(),
            //                                             Libelle = cnx.Key.Libelle,
            //                                             Connection = cnx.Count()
            //                                         });
            //return branchements;

        }

        public static List<CsConnexion> ObtenirNouveauxBranchementsEau(string annee)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    //var Clients = context.CLIENT;
                    //var Abonnes = context.ABON.ToList();
                    //var Canalisations = context.CANALISATION.ToList();
                    //var Diametres = context.DIAMETRECOMPTEUR.ToList();

                    int _annee = Int32.Parse(annee);

                    //IEnumerable<CsConnexion> branchements = GetBranchement(Clients, _annee);






                    var branchements =
                        (from c in context.CLIENT
                         from a in c.ABON.AsEnumerable()
                         from can in a.CANALISATION.AsEnumerable()

                         orderby a.DMAJ.Value.Year, a.DMAJ.Value.Month

                         where
                             //a.DRES != null &&
                             //a.FK_IDPRODUIT.ToString() == "01" &&
                             a.DMAJ.Value.Year == _annee
                         //select a).ToList();

                         group new { a, can }
                         by new { a.DMAJ.Value.Year, a.DMAJ.Value.Month, can.REGLAGECOMPTEUR   } into abon


                         select new CsConnexion
                         {
                             SetPeriode = abon.Key.Year,
                             MoisInt = abon.Key.Month,
                             Mois = "",
                             //Diametre = abon.Key.FK_IDDIAMETRE.Value.ToString(),
                             Libelle = abon.Key.REGLAGECOMPTEUR  ,
                             Connection = abon.Count()
                         }).ToList();

                    RetourneLibelleMois(branchements);

                    return branchements;

                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private static void RetourneLibelleMois(IEnumerable<CsConnexion> branchements)
        {
            foreach (var c in branchements)
            {
                if (c.Mois.Length == 1)
                    c.Mois = '0' + c.Mois;

                switch (c.MoisInt)
                {
                    case 01:
                        c.Mois = "JANUARY";
                        break;
                    case 02:
                        c.Mois = "FEBRUARY";
                        break;
                    case 03:
                        c.Mois = "MARCH";
                        break;
                    case 04:
                        c.Mois = "APRIL";
                        break;
                    case 05:
                        c.Mois = "MAY";
                        break;
                    case 06:
                        c.Mois = "JUNE";
                        break;
                    case 07:
                        c.Mois = "JULY";
                        break;
                    case 08:
                        c.Mois = "AUGUST";
                        break;
                    case 09:
                        c.Mois = "SEPTEMBER";
                        break;
                    case 10:
                        c.Mois = "OCTOBER";
                        break;
                    case 11:
                        c.Mois = "NOVEMBER";
                        break;
                    case 12:
                        c.Mois = "DECEMBER";
                        break;
                }
            }
        }

        public static List<CsConnexion> ObtenirBranchementsEau(string annee, string mois)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    //var Clients = context.CLIENT;
                    //var Abonnes = context.ABON;
                    //var Canalisations = context.CANALISATION;
                    //var Diametres = context.DIAMETRECOMPTEUR;

                    int _annee = Int32.Parse(annee);
                    //int _periode = Int32.Parse(annee);
                    int _mois = Int32.Parse(mois);
                    //int _cumul = Int32.Parse(_cumul);





                    //IEnumerable<CsConnexion> branchements = GetBranchement(Clients, _annee).Where(b=>b.MoisInt.ToString()==mois);
                    //int branchements = Int32.Parse("200106");
                    var branchements =
                          (from c in context.CLIENT
                           from a in c.ABON.AsEnumerable()
                           from can in a.CANALISATION.AsEnumerable()

                           orderby a.DMAJ.Value.Year, a.DMAJ.Value.Month

                           where
                               //a.DRES != null &&
                               //a.FK_IDPRODUIT.ToString() == "01" &&
                               a.DMAJ.Value.Year == _annee &&
                               a.DMAJ.Value.Month == _mois

                           group new { a, can }
                           by new { a.DMAJ.Value.Year, a.DMAJ.Value.Month, can.FK_IDREGLAGECOMPTEUR  , can.REGLAGECOMPTEUR  } into abon

                           select new CsConnexion
                           {
                               SetPeriode = abon.Key.Year,
                               MoisInt = abon.Key.Month,
                               Mois = "",
                               //Diametre = abon.Key.FK_IDDIAMETRECOMPTEUR.ToString(),
                               Libelle = abon.Key.REGLAGECOMPTEUR,
                               Connection = abon.Count()
                           }).ToList();

                    foreach (var c in branchements)
                    {
                        if (c.MoisInt.ToString().Length == 1)
                            c.Mois = '0' + c.MoisInt.ToString();
                    }

                    return branchements.ToList();

                    //return new List<CsConnexion>();

                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        //public static List<CParametre> ObtenirListeCaissiere()
        //{
        //    try
        //    {
        //        using (galadbEntities context = new galadbEntities())
        //        {
        //            var Utilisateurs = context.ADMUTILISATEUR;

        //            IEnumerable<CParametre> caissieres =
        //                from u in Utilisateurs
        //                where u.FONCTION1.CODE == "430"
        //                select new CParametre
        //                {
        //                    //FK_NUMCAISSE = u.FK_IDNUMCAISSE.ToString(),
        //                    LIBELLE = u.LIBELLE,
        //                    VALEUR = u.MATRICULE
        //                };
        //            List<CParametre> listeResult = caissieres.ToList();
        //            return listeResult;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}


        public static List<CsReglement> ObtenirAgent()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    var Utilisateurs = context.ADMUTILISATEUR;
                    var Transcaisse = context.TRANSCAISSE;

                    IEnumerable<CsReglement> agent =
                         from u in Utilisateurs
                         from trsc in u.TRANSCAISSE
                         where (u.TRANSCAISSE.FirstOrDefault(
                                                             t => t.COPER != "102" && t.COPER != "103" &&
                                                                   t.DC == "D" &&
                                                                   t.TOPANNUL != null) != null && trsc.SAISIPAR != null
                                                            )
                         orderby trsc.SAISIPAR
                         group new { trsc, u } by new { trsc.SAISIPAR, u.LIBELLE } into g
                         select new CsReglement
                         {
                             NOMCAISSIERE = g.Key.LIBELLE,
                             MATRICULE = g.Key.SAISIPAR
                         };

                    return agent.Distinct().ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }



        public static List<CsReglement> ObtenirAgent(string MatriculeAgent)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    var Utilisateurs = context.ADMUTILISATEUR;
                    var Transcaisse = context.TRANSCAISSE;

                    IEnumerable<CsReglement> agent =
                         from u in Utilisateurs
                         from trsc in u.TRANSCAISSE
                         where (u.TRANSCAISSE.FirstOrDefault(t => t.SAISIPAR == MatriculeAgent &&
                               t.COPER != "102" && t.COPER != "103" &&
                               t.DC == "D" &&
                               t.TOPANNUL != null) != null)

                         orderby trsc.MATRICULE
                         group new { trsc, u } by new { trsc.MATRICULE, u.LIBELLE } into g
                         select new CsReglement
                         {
                             NOMCAISSIERE = g.Key.LIBELLE,
                             MATRICULE = g.Key.MATRICULE
                         };


                    //from t in Transcaisse
                    //join u in Utilisateurs on new {matricule = t.FK_IDUTILISATEUR.ToString(),centre = t.FK_IDCENTRE.ToString() } equals new {matricule = u.tr.MATRICULE,centre = u.FK_IDCENTRE.ToString() }
                    //where t.SAISIPAR == MatriculeAgent &&
                    //      t.COPER != "102" && t.COPER != "103" &&
                    //      t.DC == "D" && 
                    //      t.TOPANNUL != null

                    //orderby t.MATRICULE
                    //group new {t,u} by new { t.MATRICULE, u.LIBELLE} into g
                    //select new CsReglement
                    //{
                    //    NOMCAISSIERE = g.Key.LIBELLE,
                    //    MATRICULE = g.Key.MATRICULE 
                    //};

                    return agent.Distinct().ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static List<CsReglement> ObtenirDateEncaissement(string MatriculeAgent)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    var Transcaisse = context.TRANSCAISSE;

                    IEnumerable<CsReglement> date =
                        from t in Transcaisse
                        where t.SAISIPAR == MatriculeAgent &&
                              t.COPER != "102" && t.COPER != "103" &&
                              t.DC == "D" &&
                              t.TOPANNUL != null

                        orderby t.DATEENCAISSEMENT
                        group t by t.DATEENCAISSEMENT into g
                        select new CsReglement
                        {
                            DATEENCAISSEMENT = g.Key.Value,
                        };
                    List<CsReglement> listeReg = date.Distinct().ToList();
                    return listeReg;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<CsReglement> ObtenirPaiementsJournaliers(string MatriculeAgent, string date)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    var Transcaisse = context.TRANSCAISSE;
                    var Clients = context.CLIENT;
                    var Utilisateurs = context.ADMUTILISATEUR;
                    var ModesReg = context.MODEREG;

                    DateTime dateEncaissement = DateTime.Parse(date);

                    var data1 =
                        from t in Transcaisse
                        join cl in Clients on new { t.FK_IDCENTRE, client = t.CLIENT, ordre = t.ORDRE }
                            equals new { cl.FK_IDCENTRE, client = cl.REFCLIENT, ordre = cl.ORDRE }
                        join u in Utilisateurs on new { matricule = t.MATRICULE }
                            equals new { matricule = u.MATRICULE }
                        join n in Utilisateurs on new { saisiPar = t.SAISIPAR }
                            equals new { saisiPar = n.MATRICULE }
                        join m in ModesReg on t.FK_IDMODEREG equals m.PK_ID

                        where t.SAISIPAR == MatriculeAgent &&
                              t.DATEENCAISSEMENT == dateEncaissement &&
                              t.COPER != "102" && t.COPER != "103" &&
                              t.DC == "D" &&
                              t.TOPANNUL != null

                        orderby t.DATEENCAISSEMENT
                        select new
                        {
                            centre = t.CENTRE,
                            client = t.CLIENT,
                            ordre = t.ORDRE,
                            nomAbon = cl.NOMABON,
                            caisse = t.FK_IDCAISSIERE,
                            acquit = t.ACQUIT,
                            matricule = t.MATRICULE,
                            montant = t.MONTANT,
                            percu = t.PERCU,
                            rendu = t.RENDU,
                            modeReg = t.FK_IDMODEREG,
                            topAnnul = t.TOPANNUL,
                            saisiPar = t.SAISIPAR,
                            nomAgentSaisi = n.LIBELLE,
                            solde = 0,
                            nomAgentManuel = u.LIBELLE,
                            coper = t.FK_IDCOPER,
                            LibelleEncaissement = m.PK_ID,
                            Numcaiss = t.FK_IDCAISSIERE,

                        };

                    var data2 = from t in data1
                                where t.coper != 082
                                group t by new { client = t.client, acquit = t.acquit, modeReg = t.modeReg, montant = t.montant } into g
                                select new
                                {
                                    client = g.Key.client,
                                    acquit = g.Key.acquit,
                                    modeReg = g.Key.modeReg,
                                    montant1 = g.Sum(x => x.montant)
                                };

                    var data3 = from t in data1
                                group t by new { client = t.client, acquit = t.acquit, modeReg = t.modeReg, percu = t.percu, rendu = t.rendu } into g
                                select new
                                {
                                    client = g.Key.client,
                                    acquit = g.Key.acquit,
                                    modeReg = g.Key.modeReg,
                                    montant2 = g.Sum(x => x.montant) - g.Sum(x => x.rendu)
                                };

                    var data4 = from t in data3
                                group t by new { client = t.client, acquit = t.acquit } into g
                                where g.Count() > 1
                                select new
                                {
                                    client = g.Key.client,
                                    acquit = g.Key.acquit
                                };

                    var data5 = from d3 in data3
                                join d1 in data4 on new { client = d3.client, acquit = d3.acquit }
                                                 equals new { client = d1.client, acquit = d1.acquit }
                                select new
                                {
                                    client = d3.client,
                                    acquit = d3.acquit,
                                    modeReg = 0,
                                    montant2 = d3.montant2
                                };

                    IEnumerable<CsReglement> reglements = (from d1 in data1
                                                           join d3 in data5 on new { client = d1.client, acquit = d1.acquit }
                                                               equals new { client = d3.client, acquit = d3.acquit }
                                                           join d2 in data2 on new { client = d1.client, acquit = d1.acquit, modeReg = d1.modeReg }
                                                               equals new { client = d2.client, acquit = d2.acquit, modeReg = d2.modeReg }
                                                           orderby d1.client, d1.acquit, d1.modeReg
                                                           //let caisse = d1.caisse.ToString()
                                                           //let modeReg = d1.modeReg.ToString()
                                                           //let LibelleEncaissement = d1.LibelleEncaissement.ToString()
                                                           select new
                                                           {
                                                               d1.centre,
                                                               d1.client,
                                                               d1.ordre,
                                                               d1.caisse,
                                                               d1.acquit,
                                                               d1.matricule,
                                                               d1.modeReg,
                                                               d1.topAnnul,
                                                               d1.saisiPar,
                                                               d1.nomAgentSaisi,
                                                               d1.nomAgentManuel,
                                                               d1.LibelleEncaissement,
                                                               d1.solde,
                                                               d2.montant1,
                                                               d3.montant2,
                                                               d1.Numcaiss
                                                           }).AsEnumerable()
                                                            .Select(t => new CsReglement
                                                            {
                                                                CENTRE = t.centre,
                                                                CLIENT = t.client,
                                                                ORDRE = t.ordre,
                                                                CAISSE = t.caisse.ToString(),
                                                                ACQUIT = t.acquit,
                                                                MATRICULE = t.matricule,
                                                                MODEREG = t.modeReg.ToString(),
                                                                TOPANNUL = t.topAnnul,
                                                                SAISIPAR = t.saisiPar,
                                                                NOMCAISSIERE = t.nomAgentSaisi,
                                                                NOMAGENTMANUEL = t.nomAgentManuel,
                                                                LIBENCAISS = t.LibelleEncaissement.ToString(),
                                                                SOLDEFACTURE = t.solde,
                                                                MONTANTFACTURE = t.montant1,
                                                                MONTANTPAYE = t.modeReg == 0 ? t.montant1 : t.montant2,
                                                                NumCaiss = t.Numcaiss.ToString()
                                                            });





                    //,d3.modeReg //d3.modeReg == 0 ? d2.montant1 : d3.montant2
                    return reglements != null ? reglements.Distinct().ToList() : new List<CsReglement>(); ;
                    //return null ;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<CsFacture> ObtenirListeAbonnements(DateTime DateInit, DateTime DateFin, int TypeEdition)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    var Abonnes = context.ABON;
                    var Clients = context.CLIENT;
                    var Categories = context.CATEGORIECLIENT;
                    var AG = context.AG;

                    DateInit = DateInit == null ? new DateTime() : DateInit;
                    DateFin = DateFin == null ? new DateTime() : DateFin;
                    DateTime vide = new DateTime();

                    var dat1 = from c in Clients
                               from a in c.ABON
                               //from g in c.AG                            
                               where (DateInit.Equals(vide) || a.DABONNEMENT  >= DateInit) &&
                               (DateFin.Equals(vide) || a.DABONNEMENT <= DateFin)
                               orderby c.CENTRE, c.REFCLIENT, c.ORDRE
                               select new CsFacture
                               {
                                   CENTRE = c.CENTRE,
                                   CLIENT = c.REFCLIENT,
                                   ORDRE = c.ORDRE,
                                   MONTANTFACTURE = a.AVANCE != null ? (decimal)a.AVANCE : 0,
                                   DateAbonnement = a.DABONNEMENT,
                                   NOM = c.NOMABON,
                                   Categorie = c.CATEGORIECLIENT.LIBELLE,
                                   //TOURNEE = g.TOURNEE
                               };
                    return dat1.Distinct().ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static List<CsFacture> ObtenirListeResiliation(DateTime DateInit, DateTime DateFin, int TypeEdition)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    var Abonnes = context.ABON;
                    var Clients = context.CLIENT;
                    var Categories = context.CATEGORIECLIENT;
                    var AG = context.AG;

                    DateInit = DateInit == null ? new DateTime() : DateInit;
                    DateFin = DateFin == null ? new DateTime() : DateFin;
                    DateTime vide = new DateTime();

                    var dat1 = from c in Clients
                               from a in c.ABON
                               //from g in c.AG                            
                               where (DateInit.Equals(vide) || a.DRES  >= DateInit) &&
                               (DateFin.Equals(vide) || a.DRES <= DateFin)
                               orderby c.CENTRE, c.REFCLIENT, c.ORDRE
                               select new CsFacture
                               {
                                   CENTRE = c.CENTRE,
                                   CLIENT = c.REFCLIENT,
                                   ORDRE = c.ORDRE,
                                   MONTANTFACTURE = a.AVANCE != null ? (decimal)a.AVANCE : 0,
                                   DateAbonnement = a.DABONNEMENT,
                                   NOM = c.NOMABON,
                                   Categorie = c.CATEGORIECLIENT.LIBELLE,
                                   //TOURNEE = g.TOURNEE
                               };
                    return dat1.Distinct().ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        //Formate une date
        public static string FormatDate(string Date)
        {
            string DateFormater = string.Empty;
            DateFormater = Date.Substring(8, 2) + "/" + Date.Substring(5, 2) + "/" + Date.Substring(0, 4);
            return DateFormater;
        }

        public static List<CsArrete> GetALLMoisComptable()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    var ARRETE = context.ARRETE;
                    //var Transcaisse = context.TRANSCAISSE;

                    IEnumerable<CsArrete> agent = from t in ARRETE

                                                  //orderby t.ANNMOIS
                                                  select new CsArrete
                                                  {
                                                      ANNMOIS = t.ANNMOIS.Substring(4, 2) + "/" + t.ANNMOIS.Substring(0, 4),
                                                      CUMULETAPE = t.CUMULETAPE,
                                                      DATEARRETE = t.DATEARRETE,
                                                      DATEDEBUT = t.DATEDEBUT,
                                                      DATEFIN = t.DATEFIN,
                                                      ETAPE = t.ETAPE,
                                                      MOISCOMPTABLE = t.ANNMOIS.Substring(4, 2),
                                                      STATUT = t.STATUT
                                                  };
                    List<CsArrete> listeArret = agent.Distinct().ToList();
                    //return Galatee.Tools.Utility.ListToDataTable(listeArret);
                    return listeArret;

                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static List<CsMois> GetMois()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    var MOIS = context.MOIS;
                    //var Transcaisse = context.TRANSCAISSE;

                    IEnumerable<CsMois> mois = from m in MOIS

                                               //orderby t.ANNMOIS
                                               select new CsMois
                                               {
                                                   LIBELLE = m.LIBELLE,
                                                   CODE = m.CODE,
                                                   PK_ID = m.PK_ID
                                               };
                    List<CsMois> listemois = mois.ToList();
                    //return Galatee.Tools.Utility.ListToDataTable(listeArret);
                    return listemois;

                }
            }
            catch (Exception)
            {
                throw;
            }
        }


        public static List<CsReglement> ObtenirMoisComptable(string MatriculeAgent)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    var Utilisateurs = context.ADMUTILISATEUR;
                    var Transcaisse = context.TRANSCAISSE;

                    IEnumerable<CsReglement> agent =
                        from u in Utilisateurs
                        from t in u.TRANSCAISSE

                        where t.SAISIPAR == MatriculeAgent &&
                         t.COPER != "102" && t.COPER != "103" &&
                         t.DC == "D" &&
                         t.TOPANNUL != null

                        orderby t.MATRICULE
                        group new { t, u } by new { t.MATRICULE, u.LIBELLE } into g
                        select new CsReglement
                        {
                            NOMCAISSIERE = g.Key.LIBELLE,
                            MATRICULE = g.Key.MATRICULE
                        };


                    //from t in Transcaisse
                    //join u in Utilisateurs on new { matricule = t.MATRICULE , centre = t.CENTRE } equals new { matricule = u.MATRICULE , centre = u.CENTRE }
                    //where t.SAISIPAR == MatriculeAgent &&
                    //      t.COPER  != "102" && t.COPER  != "103" &&
                    //      t.DC == "D" &&
                    //      t.TOPANNUL != null

                    //orderby t.MATRICULE 
                    //group new { t, u } by new { t.MATRICULE , u.LIBELLE } into g
                    //select new CsReglement
                    //{
                    //    NOMCAISSIERE = g.Key.LIBELLE,
                    //    MATRICULE = g.Key.MATRICULE 
                    //};

                    return agent.Distinct().ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static DataTable ObtenirFactures(string Categorie, string TypeEdition, string jour, string mois, string annee)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    var Clients = context.CLIENT;
                    var Lclients = context.LCLIENT;
                    var Categories = context.CATEGORIECLIENT;

                    int Categorie_ = string.IsNullOrEmpty(Categorie) ? 0 : int.Parse(Categorie);
                    int _jour = string.IsNullOrEmpty(jour) ? -1 : Int32.Parse(jour.Substring(0, 2));
                    int _mois = string.IsNullOrEmpty(mois) ? -1 : Int32.Parse(mois);
                    int _annee = string.IsNullOrEmpty(annee) ? -1 : Int32.Parse(annee);
                    TypeEdition = string.IsNullOrEmpty(TypeEdition) ? "" : TypeEdition;
                    string vide = "";

                    IEnumerable<object> data =
                        from cl in Clients
                        join lcl in Lclients on new { centre = cl.CENTRE, client = cl.REFCLIENT, ordre = cl.ORDRE }
                                             equals new { centre = lcl.CENTRE, client = lcl.CLIENT, ordre = lcl.ORDRE }
                        join cat in Categories on cl.FK_IDCATEGORIE equals cat.PK_ID
                        where
                            (cl.FK_IDCATEGORIE == Categorie_) &&
                            (lcl.DENR.Year <= _annee) &&
                            (lcl.DENR.Day <= _jour) &&
                            (lcl.DENR.Month <= _mois) &&
                            (lcl.MONTANT > 100000)

                        select new
                        {
                            FK_IDCATEGORIE = cl.FK_IDCATEGORIE,
                            LibelleCategorie = cat.LIBELLE,
                            CENTRE = cl.CENTRE,
                            CLIENT = cl.REFCLIENT,
                            PK_IDCLIENT = cl.PK_ID,
                            ORDRE = cl.ORDRE,
                            DC = lcl.DC,
                            montant = lcl.MONTANT,
                            NOM = cl.NOMABON
                        };
                    DataTable dt = Galatee.Tools.Utility.ListToDataTable(data);
                    return dt;

                    //var Clients = context.CLIENT;
                    //var Lclients = context.LCLIENT;
                    //var Categories = context.CATEGORIECLIENT;

                    //int Categorie_ = string.IsNullOrEmpty(Categorie) ? 0 : int.Parse(Categorie);
                    //int _jour = string.IsNullOrEmpty(jour) ? -1 : Int32.Parse(jour.Substring(0, 2));
                    //int _mois = string.IsNullOrEmpty(mois) ? -1 : Int32.Parse(mois);
                    //int _annee = string.IsNullOrEmpty(annee) ? -1 : Int32.Parse(annee);
                    //TypeEdition = string.IsNullOrEmpty(TypeEdition) ? "" : TypeEdition;
                    //string vide = "";

                    //var data =
                    //    from cl in Clients
                    //    join lcl in Lclients on new { centre = cl.CENTRE, client = cl.REFCLIENT, ordre = cl.ORDRE }
                    //                         equals new { centre = lcl.CENTRE, client = lcl.CLIENT, ordre = lcl.ORDRE }
                    //    where
                    //        (Categorie.Equals(vide) || cl.FK_IDCATEGORIE == Categorie_) &&
                    //        (_annee.Equals(-1) || lcl.DENR.Year <= _annee) &&
                    //        (_jour.Equals(-1) || lcl.DENR.Day <= _jour) &&
                    //        (_mois.Equals(-1) || lcl.DENR.Month <= _mois)

                    //    select new
                    //    {
                    //        categorie = cl.FK_IDCATEGORIE,
                    //        centre = cl.CENTRE,
                    //        client = cl.REFCLIENT,
                    //        ordre = cl.ORDRE,
                    //        dc = lcl.DC,
                    //        montant = lcl.MONTANT
                    //    };

                    //var data1 = data.Where(x => x.dc == "D").
                    //            GroupBy(x => new { x.centre, x.client, x.ordre, x.montant }).
                    //            Select(x => new { centre = x.Key.centre, client = x.Key.client, ordre = x.Key.ordre, debit = x.Sum(y => y.montant) });

                    //var data2 = data.Where(x => x.dc == "C").
                    //            GroupBy(x => new { x.centre, x.client, x.ordre, x.montant }).
                    //            Select(x => new { centre = x.Key.centre, client = x.Key.client, ordre = x.Key.ordre, credit = x.Sum(y => y.montant) });

                    //var data3 = data1.Join(data2,
                    //                        d1 => new { d1.centre, d1.client, d1.ordre },
                    //                        d2 => new { d2.centre, d2.client, d2.ordre },
                    //                        (d1, d2) => new { centre = d1.centre, client = d1.client, ordre = d1.ordre, solde = d1.debit - d2.credit });

                    //var data4 = data3.Join(data,
                    //                        d3 => new { d3.centre, d3.client, d3.ordre },
                    //                        d => new { d.centre, d.client, d.ordre },
                    //                        (d3, d) => new { centre = d3.centre, client = d3.client, ordre = d3.ordre, categorie = d.categorie, solde = d3.solde }).Distinct();

                    //IEnumerable<CsFacture> factures = data4.AsEnumerable().Join(Categories,
                    //                        d4 => d4.categorie.ToString(),
                    //                        c => c.CODE,
                    //                        (d4, c) => new { categorie = d4.categorie, libelle = c.LIBELLE, solde = d4.solde }).
                    //                        OrderBy(x => x.categorie).
                    //                        GroupBy(x => new { x.categorie, x.libelle, x.solde }).
                    //                        Select(x => new CsFacture { Categorie = x.Key.categorie + " " + x.Key.libelle, SoldeClient = x.Sum(y => (decimal)y.solde) });

                    //return factures.ToList();

                }
                //return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable ObtenirFacturesDeTournee(int idTournee, string jour, string mois, string annee)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    var Clients = context.CLIENT;
                    var Lclients = context.LCLIENT;
                    var Ag = context.AG;

                    int _jour = string.IsNullOrEmpty(jour) ? -1 : Int32.Parse(jour.Substring(0, 2));
                    int _mois = string.IsNullOrEmpty(mois) ? -1 : Int32.Parse(mois);
                    int _annee = string.IsNullOrEmpty(annee) ? -1 : Int32.Parse(annee);
                    string vide = "";

                    IEnumerable<object> data =
                        from cl in Clients
                        //from a in cl.AG 
                        from lcl in cl.LCLIENT
                        where
                            //(a.FK_IDTOURNEE  == idTournee) &&
                            (lcl.DENR.Year <= _annee) &&
                            (lcl.DENR.Day <= _jour) &&
                            (lcl.DENR.Month <= _mois)
                            && (lcl.MONTANT > 1000000)

                        select new
                        {
                            FK_IDCATEGORIE = cl.FK_IDCATEGORIE,
                            //LibelleCategorie = a.TOURNEE,
                            CENTRE = cl.CENTRE,
                            CLIENT = cl.REFCLIENT,
                            PK_IDCLIENT = cl.PK_ID,
                            ORDRE = cl.ORDRE,
                            DC = lcl.DC,
                            montant = lcl.MONTANT,
                            NOM = cl.NOMABON,
                            //TOURNEE=a.TOURNEE
                        };
                    DataTable dt = Galatee.Tools.Utility.ListToDataTable(data);
                    return dt;



















                    //var Clients = context.CLIENT;
                    //var Lclients = context.LCLIENT;
                    //var Ag = context.AG;

                    //Tournee = string.IsNullOrEmpty(Tournee) ? "" : Tournee;
                    //int _jour = string.IsNullOrEmpty(jour) ? -1 : Int32.Parse(jour.Substring(0,2));
                    //int _mois = string.IsNullOrEmpty(mois) ? -1 : Int32.Parse(mois);
                    //int _annee = string.IsNullOrEmpty(annee) ? -1 : Int32.Parse(annee);
                    ////TypeEdition = string.IsNullOrEmpty(TypeEdition) ? "" : TypeEdition;
                    //string vide = "";

                    //var temp =
                    //    from a in Ag
                    //    join cl in Clients on new { centre = a.CENTRE, client = a.CLIENT }
                    //                       equals new { centre = cl.CENTRE, client = cl.REFCLIENT }
                    //    where
                    //        (Tournee.Equals(vide) || a.TOURNEE == Tournee)
                    //    select new
                    //    {
                    //        tournee = a.TOURNEE,
                    //        centre = cl.CENTRE,
                    //        client = cl.REFCLIENT,
                    //        ordre = cl.ORDRE,
                    //        nom = cl.NOMABON
                    //    };


                    //var data = temp.Join(Lclients,
                    //                        d => new { centre = d.centre, client = d.client, ordre = d.ordre },
                    //                        l => new { centre = l.CENTRE, client = l.CLIENT, ordre = l.ORDRE },
                    //                        (d, l) => new { tournee = d.tournee, centre = d.centre, client = d.client, ordre = d.ordre, nom = d.nom, dc = l.DC, montant = l.MONTANT, denr = l.DENR }).
                    //                        Where(x => (_annee == -1 || x.denr.Year <= _annee) &&
                    //                                    (_jour == -1 || x.denr.Day <= _jour) &&
                    //                                    (_mois == -1 || x.denr.Month <= _mois));

                    //var data1 = data.Where(d => d.dc == "D").
                    //                 GroupBy(d => new { centre = d.centre, client = d.client, ordre = d.ordre }).
                    //                 Select(x => new { centre = x.Key.centre, client = x.Key.client, ordre = x.Key.ordre, debit = x.Sum(y => y.montant) });

                    //var data2 = data.Where(d => d.dc == "C").
                    //                 GroupBy(d => new { centre = d.centre, client = d.client, ordre = d.ordre }).
                    //                 Select(x => new { centre = x.Key.centre, client = x.Key.client, ordre = x.Key.ordre, credit = x.Sum(y => y.montant) });

                    //var data3 = data1.Join(data2,
                    //                        d3 => new { centre = d3.centre, client = d3.centre, ordre = d3.ordre },
                    //                        d2 => new { centre = d2.centre, client = d2.centre, ordre = d2.ordre },
                    //                        (d3, d2) => new { centre = d3.centre, client = d3.client, ordre = d3.ordre, solde = d3.debit - d2.credit });

                    //IEnumerable<CsFacture> factures =
                    //                       data3.Join(data,
                    //                                  d3 => new { centre = d3.centre, client = d3.client, ordre = d3.ordre },
                    //                                  d => new { centre = d.centre, client = d.client, ordre = d.ordre },
                    //                                  (d3, d) => new CsFacture { CENTRE = d3.centre, CLIENT = d3.client, ORDRE = d3.ordre, NOM = d.nom, MONTANTFACTURE = (decimal)d.montant, TOURNEE = d.tournee }).
                    //                                  OrderBy(d => new { d.TOURNEE, d.CENTRE, d.CLIENT, d.ORDRE }).Distinct();

                    //return factures.ToList();
                }
                //return null;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public static List<CsCprofac> SPX_CPROFAC_SEL_ELEC_CONSO(string Annee, string Mois, string Categorie, string coper)
        {
            try
            {
                galadbEntities contextgala = new galadbEntities();
                ABO07Entities context = new ABO07Entities();


                var CATCLIENT = contextgala.CATEGORIECLIENT;
                var PRODUIT = contextgala.PRODUIT;
                var CENTFAC = context.CENTFAC;
                var CREDFAC = context.CREDFAC;
                var CPROFAC = context.CPROFAC;

                IEnumerable<CsCprofac> query_ = from centfac in CENTFAC
                                                join credfac in CREDFAC
                                                    on
                                                        new { centfac.CENTRE, centfac.CLIENT, centfac.ORDRE, centfac.PERIODE }
                                                            equals
                                                        new { credfac.CENTRE, credfac.CLIENT, credfac.ORDRE, credfac.PERIODE }
                                                join cprofac in CPROFAC
                                                    on
                                                        new { centfac.CENTRE, centfac.CLIENT, centfac.ORDRE, centfac.PERIODE }
                                                            equals
                                                        new { cprofac.CENTRE, cprofac.CLIENT, cprofac.ORDRE, cprofac.PERIODE }
                                                where
                                                        credfac.PERIODE.Substring(0, 4) == Annee &&
                                                        credfac.PERIODE.Substring(4, 2) == Mois &&
                                                        centfac.CATEGORIECLIENT == Categorie &&
                                                        credfac.REDEVANCE.Substring(credfac.REDEVANCE.Length - 1, 1) == "3" &&
                                                        centfac.COPER == coper
                                                select
                                                    new CsCprofac
                                                    {
                                                        MOIS = credfac.PERIODE.Substring(4, 2),
                                                        PERIODE = credfac.PERIODE,
                                                        CONSOFAC = credfac.QUANTITE,
                                                        TOTAL = credfac.TOTREDHT * credfac.TOTREDTAX ,
                                                        //PRODUCTLABEL = (PRODUIT.FirstOrDefault(p => p.PK_ID.ToString() == cprofac.PRODUIT)).LIBELLE ,
                                                        PRODUCTLABEL = "ELECTRICITY",
                                                        //CATCLILABEL = (CATCLIENT.FirstOrDefault(catcli=>catcli.PK_ID.ToString()==centfac.CATCLI).LIBELLE),
                                                        CATCLILABEL = centfac.CATEGORIECLIENT,
                                                        PRODUIT = cprofac.PRODUIT
                                                    };
                if (query_ == null)
                {
                    return null;
                }
                return query_.ToList();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<CsCprofac> SPX_CPROFAC_SEL_GEN_CONSO(string Annee, string Mois, string Categorie, string coper)
        {
            try
            {
                galadbEntities contextgala = new galadbEntities();
                ABO07Entities context = new ABO07Entities();


                var CATCLIENT = contextgala.CATEGORIECLIENT;
                var PRODUIT = contextgala.PRODUIT;
                var CENTFAC = context.CENTFAC;
                var CREDFAC = context.CREDFAC;
                var CPROFAC = context.CPROFAC;

                IEnumerable<CsCprofac> query_ = from centfac in CENTFAC
                                                join credfac in CREDFAC
                                                    on
                                                        new { centfac.CENTRE, centfac.CLIENT, centfac.ORDRE, centfac.PERIODE }
                                                            equals
                                                        new { credfac.CENTRE, credfac.CLIENT, credfac.ORDRE, credfac.PERIODE }
                                                join cprofac in CPROFAC
                                                    on
                                                        new { centfac.CENTRE, centfac.CLIENT, centfac.ORDRE, centfac.PERIODE }
                                                            equals
                                                        new { cprofac.CENTRE, cprofac.CLIENT, cprofac.ORDRE, cprofac.PERIODE }
                                                where
                                                        credfac.PERIODE.Substring(0, 4) == Annee &&
                                                        credfac.PERIODE.Substring(4, 2) == Mois &&
                                                        centfac.CATEGORIECLIENT == Categorie &&
                                                    //credfac.REDEVANCE.Substring(credfac.REDEVANCE.Length - 1, 1) == "3" &&
                                                        centfac.COPER == coper
                                                select
                                                    new CsCprofac
                                                    {
                                                        MOIS = credfac.PERIODE.Substring(4, 2),
                                                        PERIODE = credfac.PERIODE,
                                                        CONSOFAC = credfac.QUANTITE,
                                                        TOTAL = credfac.TOTREDHT  * credfac.TOTREDTAX ,

                                                        //PRODUCTLABEL = (PRODUIT.FirstOrDefault(p => p.PK_ID.ToString() == cprofac.PRODUIT)).LIBELLE ,
                                                        PRODUCTLABEL = "GENERALE",
                                                        //CATCLILABEL = (CATCLIENT.FirstOrDefault(catcli=>catcli.PK_ID.ToString()==centfac.CATCLI).LIBELLE),
                                                        CATCLILABEL = centfac.CATEGORIECLIENT,
                                                        PRODUIT = cprofac.PRODUIT
                                                    };
                if (query_ == null)
                {
                    return null;
                }
                return query_.ToList();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<CsSales> QUANTITY_SOLD_BY_CATEG(string Year, string coper)
        {
            try
            {
                galadbEntities contextgala = new galadbEntities();
                ABO07Entities context = new ABO07Entities();


                var CATCLIENT = contextgala.CATEGORIECLIENT;
                var PRODUIT = contextgala.PRODUIT;
                var CENTFAC = context.CENTFAC;
                var CREDFAC = context.CREDFAC;
                var CPROFAC = context.CPROFAC;

                IEnumerable<CsSales> query_ = from centfac in CENTFAC
                                              join credfac in CREDFAC
                                                  on
                                                      new { centfac.CENTRE, centfac.CLIENT, centfac.ORDRE, centfac.PERIODE }
                                                          equals
                                                      new { credfac.CENTRE, credfac.CLIENT, credfac.ORDRE, credfac.PERIODE }
                                              join cprofac in CPROFAC
                                                  on
                                                      new { centfac.CENTRE, centfac.CLIENT, centfac.ORDRE, centfac.PERIODE }
                                                          equals
                                                      new { cprofac.CENTRE, cprofac.CLIENT, cprofac.ORDRE, cprofac.PERIODE }
                                              where
                                                      credfac.PERIODE.Substring(0, 4) == Year &&
                                                      centfac.COPER == coper
                                              select
                                                  new CsSales
                                                  {
                                                      PERIODE = credfac.PERIODE,
                                                      CONSOFAC = credfac.QUANTITE.Value,
                                                      TOTAL = credfac.TOTREDHT.Value + credfac.TOTREDTAX.Value,
                                                      CATCLI = centfac.CATEGORIECLIENT,
                                                      PRODUIT = cprofac.PRODUIT,
                                                      TOURNEE = centfac.TOURNEE
                                                  };
                if (query_ == null)
                {
                    return null;
                }
                List<CsSales> var = query_.ToList();
                return var;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
