using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Galatee.Structure;
using System.Transactions;

namespace Galatee.Entity.Model
{
    public class MobileProcedures
    {
        public static DataTable RetourneDonneesReleveurEtTournee(string adressMac)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    var releves = context.RELEVEUR.Where(r => r.PORTABLE == adressMac);
                    
                    IEnumerable<object> query = from x in releves
                                                from y in x.TOURNEERELEVEUR 
                                                select new {

                                                    // table releveur mobile
                                                    x.CENTRE ,
                                                    NUMRELEVEUR = x.CODE ,
                                                    MATRICULE = x.MATRICULE,
                                                    FERMEQUOT = x.FERMEQUOT,
                                                    FERMEREAL = x.FERMEREAL,
                                                    PORTABLE = x.PORTABLE

                                                    //table tournee mobile
                                                    //t.RELEVEUR ,
                                                    //NUMTOURNEE = t.CODE,
                                                    //t.LIBELLE,
                                                    //t.LOCALISATION,
                                                    //t.PRIORITE,
                                                    //MATRICULEPIA = t.MATRICULEPIA,
                                                    //SITE = t.CENTRE1.SITE.CODESITE
                                                };
                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable RetourneDonneesReleveur(string adressMac)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    var releves = context.RELEVEUR.Where(r => r.PORTABLE == adressMac);
                    IEnumerable<object> query = from x in releves 
                                                join a in context.ADMUTILISATEUR on x.MATRICULE equals a.MATRICULE
                                                select new
                                                {
                                                    x.CENTRE,
                                                    NUMRELEVEUR = x.CODE ,
                                                    MATRICULE = x.MATRICULE,
                                                    FERMEQUOT = x.FERMEQUOT,
                                                    FERMEREAL = x.FERMEREAL,
                                                    PORTABLE = x.PORTABLE,
                                                    //a.FONCTION
                                                };
                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

     
        public static DataTable RetourneTousClients()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    var moratoire = context.CLIENT;

                    IEnumerable<object> query = from x in moratoire
                                                select new
                                                {
                                                    x.CENTRE,
                                                    x.REFCLIENT,
                                                    x.ORDRE,
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

        public static DataTable RetourneTousCaisse()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    var caisse = context.CAISSE;

                    IEnumerable<object> query = from x in caisse
                                                select new
                                                {
                                                    x.ACQUIT,
                                                    x.BORDEREAU,
                                                    x.COMPTE,
                                                    x.FONDCAISSE,
                                                    x.NUMCAISSE,
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

        public static DataTable RetourneTousBanques()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    var bank = context.BANQUE;

                    IEnumerable<object> query = from x in bank
                                                select new
                                                {
                                                    x.LIBELLE,
                                                    x.CODE ,
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

        public static DataTable RetourneTousCodeRegroupement()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    var REGCLI = context.REGROUPEMENT;

                    IEnumerable<object> query = from x in REGCLI
                                                select new
                                                {

                                                    x.CODE ,
                                                    x.NOM
                                                };
                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable RetourneTousSites()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    var centres = context.SITE;

                    IEnumerable<object> query = from x in centres
                                                select x;
                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable RetourneTousParametreGeneraux()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    var generaux = context.PARAMETRESGENERAUX;

                    IEnumerable<object> query = from x in generaux
                                                select new { 
                                                 x.LIBELLE,
                                                 x.CODE,
                                                 x.DESCRIPTION
                                                };
                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable RetourneTousParametreGeneraux(string code)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    var generaux = context.PARAMETRESGENERAUX.Where(p=> p.CODE == code);

                    IEnumerable<object> query = from x in generaux
                                                select new
                                                {
                                                    x.LIBELLE,
                                                    x.CODE,
                                                    x.DESCRIPTION
                                                };
                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable RetourneTousFonctions()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    var centres = context.FONCTION;

                    IEnumerable<object> query = from x in centres
                                                select x;
                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable RetourneTousModeReglement()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    var _MODEREG = context.MODEREG;

                    IEnumerable<object> query = from x in _MODEREG
                                                select new { 
                                                 x.COMPTE,
                                                 x.COMPTEANNEXE1,
                                                 x.COPER,
                                                 x.FK_IDCOPER,
                                                 x.DATECREATION,
                                                 x.ECARTNEG,
                                                 x.ECARTPOS,
                                                 x.LIBELLE,
                                                 x.CODE ,
                                                 x.USERCREATION,
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

        public static DataTable RetourneTousCoper()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {

                    IEnumerable<object> query = from x in context.COPER
                                                select new
                                                {

                                                    x.AJUFIN,
                                                    x.CODE,
                                                    x.COMPTEANNEXE1,
                                                    x.COMPTGENE,
                                                    //x.COPERDEMANDE,
                                                    //x.COUTCOPER,
                                                    x.CTRAIT,
                                                    x.DATECREATION,
                                                    x.DATEMODIFICATION,
                                                    x.DC,
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

        public static DataTable RetourneTousUtilisateur()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {

                    IEnumerable<object> query = from x in context.ADMUTILISATEUR
                                                select new
                                                {
                                                    x.PK_ID,
                                                    x.MATRICULE,
                                                    x.LIBELLE,
                                                    x.LOGINNAME
                                                };
                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable RetourneMenuParModule(string pModule)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = from _admMenu in context.ADMMENU
                                                where _admMenu.MODULE.ToUpper() == pModule.ToUpper()
                                                select new
                                                {
                                                    Module = _admMenu.MODULE,
                                                    MenuID = _admMenu.PK_ID,
                                                    MenuText = _admMenu.MENUTEXT,
                                                    MainMenuID = _admMenu.MAINMENUID,
                                                    FormName = _admMenu.FORMENAME,
                                                    IsDock = _admMenu.ISDOCK,
                                                    IsControl = _admMenu.ISCONTROLE,
                                                    MenuOrder = _admMenu.MENUORDER
                                                }; 
                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable RetourneTousProduit()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    var Produit = context.PRODUIT;
                    IEnumerable<object> query =
                    from produit in Produit
                    select new
                    {
                       PK_ID= produit.PK_ID,
                        produit.CODE ,
                       //LIBELLEPRODUIT = produit.LIBELLE,
                        produit.LIBELLE,
                        produit.USERCREATION,
                        produit.USERMODIFICATION,
                        produit.DATECREATION,
                        produit.DATEMODIFICATION
                    };
                    return Galatee.Tools.Utility.ListToDataTable(query);
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable RetourneSpecificiteSite(string pSite)
        {
            try
            {
                //using (galadbEntities context = new galadbEntities())
                //{
                //    IEnumerable<object> query = null;
                //    var _SpSite = context.SPESITE;

                //    if ((!string.IsNullOrEmpty(pSite)))
                //    {
                //        query =
                //        from LaSpSite in _SpSite
                //        where LaSpSite.SITE == pSite
                //        select _SpSite;
                //    }
                //    return Galatee.Tools.Utility.ListToDataTable(query);
                //};
                return new DataTable();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<CsCentre> RetourneTousDirecteur()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    var Centre = context.CENTRE ;
                    IEnumerable<CsCentre> query =
                    from c in Centre
                    select new CsCentre
                    {
                        CODE = c.CODE ,
                        LIBELLE = c.LIBELLE
                    };

                    return query.ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable RetourneTaxe(string pCentre, string pTaxe)
        {
            try
            {
                IEnumerable<object> query = null;
                using (galadbEntities context = new galadbEntities())
                {
                    string Centre = !string.IsNullOrEmpty(pCentre) ? pCentre : "";
                    string Taxe = !string.IsNullOrEmpty(pTaxe) ? pTaxe : "";
                    string vide = "";
                    var _CTAX = context.TAXE ;
                    query =
                    from _LaTaxe in _CTAX
                    where  
                          (_LaTaxe.CODE == pTaxe || Taxe.Equals(vide))
                    select new
                    {
                        _LaTaxe.CODE,
                        _LaTaxe.LIBELLE,
                        _LaTaxe.TAUX,
                        _LaTaxe.DEBUTAPPLICATION,
                        _LaTaxe.FINAPPLICATION,
                        _LaTaxe.TYPETAXE,
                        _LaTaxe.DATECREATION,
                        _LaTaxe.DATEMODIFICATION,
                        _LaTaxe.USERCREATION,
                        _LaTaxe.USERMODIFICATION

                    };
                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable RetourneTousLibelleTop()
        {
            try
            {
                IEnumerable<object> query = null;
                using (galadbEntities context = new galadbEntities())
                {
                    query =
                    from l in context.LIBELLETOP
                    select new
                    {
                        l.CODE,
                        l.DATECREATION,
                        l.LIBELLE,
                        l.PK_ID,
                        l.USERCREATION
                    };
                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable RetourneCodeConsomateur(string pConsomateur)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = null;
                    var _CONSOMMATEUR = context.CODECONSOMMATEUR;
                    if ((!string.IsNullOrEmpty(pConsomateur)))
                    {
                        query =
                        from _LaConso in _CONSOMMATEUR
                        where _LaConso.CODE   == pConsomateur
                        select new
                        {
                            _LaConso.CODE ,
                            _LaConso.LIBELLE,
                            _LaConso.PK_ID 
                        };
                    }
                    else
                    {
                        query =
                        from _LaConso in _CONSOMMATEUR
                        select new
                        {
                            _LaConso.CODE,
                            _LaConso.LIBELLE,
                            _LaConso.PK_ID 
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

        public static DataTable RetourneNatureClient(string pNature)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    //IEnumerable<object> query = null;
                    //var _NATURECLIENT = context.NATURECLIENT;
                    //if ((!string.IsNullOrEmpty(pNature)))
                    //{
                    //    query =
                    //    from _LaNature in _NATURECLIENT
                    //    where _LaNature.CODE == pNature
                    //    select new
                    //    {
                    //        _LaNature.CODE,
                    //        _LaNature.LIBELLE,
                    //        _LaNature.USERCREATION,
                    //        _LaNature.USERMODIFICATION,
                    //        _LaNature.DATECREATION,
                    //        _LaNature.DATEMODIFICATION
                    //    };
                    //}
                    //else
                    //{
                    //    query =
                    //    from _LaNature in _NATURECLIENT
                    //    select new
                    //    {
                    //        _LaNature.CODE,
                    //        _LaNature.LIBELLE,
                    //        _LaNature.USERCREATION,
                    //        _LaNature.USERMODIFICATION,
                    //        _LaNature.DATECREATION,
                    //        _LaNature.DATEMODIFICATION
                    //    };
                    //}
                    //return Galatee.Tools.Utility.ListToDataTable(query);

                    return new DataTable();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable RetourneNature()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                //    IEnumerable<object> query = null;
                //    query =
                //    from n in context.NATURE
                //    select new
                //    {
                //        n.CODE,
                //        n.COPER,
                //        n.PK_ID,
                //        n.LIBELLE,
                //        n.USERCREATION,
                //        n.USERMODIFICATION,
                //        n.DATECREATION,
                //        n.DATEMODIFICATION,
                //        n.DC,
                //        n.LIBCOURT
                //    };
                //    return Galatee.Tools.Utility.ListToDataTable(query);

                return new DataTable();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public static DataTable RetourneFermable(string pFermable)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = null;
                    var _RELANCE = context.RELANCE;
                    string Fermable = string.IsNullOrEmpty(pFermable) ? "" : pFermable;
                    string vide = "";
                    query =
                    from _LaRel in _RELANCE
                    where (_LaRel.CODE == pFermable || Fermable.Equals(vide))
                    select new
                    {
                        _LaRel.CODE,
                        _LaRel.LIBELLE,
                        _LaRel.USERCREATION,
                        _LaRel.USERMODIFICATION,
                        _LaRel.DATECREATION,
                        _LaRel.DATEMODIFICATION,
                        _LaRel.PK_ID

                    };
                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable RetourneNationnalite(string pNationnalite)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    string Nationnalite = string.IsNullOrEmpty(pNationnalite)?string.Empty :pNationnalite ;
                    string Vide = "";

                    IEnumerable<object> query = null;
                    var _NATIONALITE = context.NATIONALITE;
                        query =
                        from _LaNation in _NATIONALITE
                        where (_LaNation.CODE == pNationnalite || pNationnalite.Equals(Vide))
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
                        return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable RetourneDiametreCompteur(string pProduit)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = null;
                    var _LstDiametreCompteur = context.REGLAGECOMPTEUR ;
                  
                        query =
                        from _LeDiametreCompteur in _LstDiametreCompteur
                        where _LeDiametreCompteur.PRODUIT.CODE  == pProduit
                        select new
                        {
                            _LeDiametreCompteur.PRODUIT,
                            _LeDiametreCompteur.CODE ,
                            _LeDiametreCompteur.LIBELLE,
                            _LeDiametreCompteur.DATECREATION,
                            _LeDiametreCompteur.DATEMODIFICATION,
                            _LeDiametreCompteur.USERCREATION,
                            _LeDiametreCompteur.USERMODIFICATION
                        };
                   
                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable RetourneDiametreBranchement(string pProduit)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = null;
                    var _DIAMETREBRANCHEMENT = context.TYPEBRANCHEMENTPARPRODUIT   ;
                  
                        query =
                        from x in _DIAMETREBRANCHEMENT
                        select new
                        {
                           PRODUIT= x.PRODUIT.CODE ,
                           CODE= x.TYPEBRANCHEMENT.CODE  ,
                           x.TYPEBRANCHEMENT.LIBELLE ,
                        };
                  
                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable RetourneMaterielBranchement(string pProduit)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = null;
                    var _MATERIELBRANCHEMENT = context.MATERIELBRANCHEMENT;
                    if ((!string.IsNullOrEmpty(pProduit)))
                    {
                        query =
                        from x in _MATERIELBRANCHEMENT
                        where x.PRODUIT == pProduit
                        select new
                        {
                            x.CODE ,
                            x.PRODUIT,
                            x.LIBELLE,
                            x.DATECREATION,
                            x.DATEMODIFICATION,
                            x.USERCREATION,
                            x.USERMODIFICATION
                        };
                    }
                    else
                    {
                        query =
                          from x in _MATERIELBRANCHEMENT
                          select new
                          {
                              x.CODE ,
                              x.PRODUIT,
                              x.LIBELLE,
                              x.DATECREATION,
                              x.DATEMODIFICATION,
                              x.USERCREATION,
                              x.USERMODIFICATION
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

        //public static DataTable RetourneTousNonCaissiere()
        //{
        //    try
        //    {
        //        using (galadbEntities context = new galadbEntities())
        //        {
        //            var caissiere = context.ADMUTILISATEUR.Where(c => c.FONCTION1.CODE  != Enumere.CodeFonctionCaisse);

        //            //foreach(var c in caissiere)
        //            //{
        //            // if(int.Parse(c.NUMCAISSE)> 0)
        //            //}

        //            IEnumerable<object> query = from c in caissiere
        //                                        select new
        //                                        {
        //                                            c.MATRICULE,
        //                                            //c.NUMCAISSE,
        //                                            c.LIBELLE,
        //                                            c.CENTRE
        //                                        };
        //            return Galatee.Tools.Utility.ListToDataTable(query);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public static DataTable RetourneTousCaissiere()
        //{
        //    try
        //    {
        //        using (galadbEntities context = new galadbEntities())
        //        {
        //            var caissiere = context.ADMUTILISATEUR.Where(c => c.FONCTION1.CODE  == Enumere.CodeFonctionCaisse);

        //            IEnumerable<object> query = from c in caissiere
        //                                        select new
        //                                        {
        //                                            c.MATRICULE,
        //                                            //c.NUMCAISSE,
        //                                            c.LIBELLE,
        //                                            c.CENTRE
        //                                        };
        //            return Galatee.Tools.Utility.ListToDataTable(query);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public static DataTable RetourneListeCommune(string pCentre, string pCommune)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    string Commune = !string.IsNullOrEmpty(pCommune) ? pCommune : "";
                    string Centre = !string.IsNullOrEmpty(pCentre) ? pCentre : "";
                    string vide = "";
                    IEnumerable<object> query = null;
                    var _LstCommuneInit = context.COMMUNE;

                    query =
                    from _LaCommune in _LstCommuneInit
                    //where (_LaCommune.CENTRE == pCentre || Centre.Equals(vide)) &&
                    //      ((_LaCommune.CODE == pCommune || 
                    //        _LaCommune.CODE== Enumere.Generale || 
                    //        Commune.Equals(vide)))
                    select new
                    {
                        _LaCommune.CENTRE,
                        _LaCommune.CODE ,
                        _LaCommune.LIBELLE,
                        _LaCommune.PK_ID,
                        _LaCommune.FK_IDCENTRE
                    };

                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static DataTable RetourneListeQuartier(string pCommune, string pQuartier)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    string Commune = string.IsNullOrEmpty(pCommune) ? "" : pCommune;
                    string Quartier = string.IsNullOrEmpty(pQuartier) ? "" : pQuartier;
                    string Vide = "";

                    IEnumerable<object> query = null;
                    var _LstQuartierInit = context.QUARTIER;

                    query =
                    from _LeQuartier in _LstQuartierInit
                    //where (_LeQuartier.COMMUNE == pCommune || Commune.Equals(Vide)) &&
                    //      (_LeQuartier.CODEQUARTIER == pQuartier || Quartier.Equals(Vide))
                    select new
                    {
                        _LeQuartier.COMMUNE,
                        _LeQuartier.CODE ,
                        _LeQuartier.LIBELLE,
                        _LeQuartier.TRANS,
                        _LeQuartier.DATECREATION,
                        _LeQuartier.DATEMODIFICATION,
                        _LeQuartier.USERCREATION,
                        _LeQuartier.USERMODIFICATION,
                        _LeQuartier.PK_ID,
                        _LeQuartier.FK_IDCOMMUNE 
                    };
                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static DataTable RetourneListeRue(string pQuartier, string pRue)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    string Quartier = string.IsNullOrEmpty(pQuartier) ? "" : pQuartier;
                    string Rue = string.IsNullOrEmpty(pRue) ? "" : pRue;
                    string Vide = "";

                    IEnumerable<object> query = null;
                    var _LstRue = context.RUES;
                    query =
                    from _LaRue in _LstRue
                    where (_LaRue.CODE  == pRue || Rue.Equals(Vide))
                    select new
                    {
                        _LaRue.CODE ,
                        _LaRue.LIBELLE ,
                        _LaRue.COMPRUE,
                        _LaRue.NUMRUE,
                        _LaRue.TRANS,
                        _LaRue.DATECREATION,
                        _LaRue.DATEMODIFICATION,
                        _LaRue.USERCREATION,
                        _LaRue.USERMODIFICATION,
                        _LaRue.PK_ID,
                        _LaRue.FK_IDSECTEUR 
                    };
                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static DataTable RetourneListeTournee(string pCentre, string pTournee)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = null;
                    var _LstTournee = context.TOURNEE.Where(r => r.SUPPRIMER == false || r.SUPPRIMER == null);
                    if (!string.IsNullOrEmpty(pTournee))
                    {
                        query =
                        (from _LaTournee in _LstTournee
                         where (_LaTournee.CENTRE == pCentre || _LaTournee.CENTRE == Enumere.Generale)
                         && _LaTournee.CODE == pTournee
                         select new
                         {
                             _LaTournee.PK_ID,
                             _LaTournee.FK_IDCENTRE,
                             //_LaTournee.FK_IDRELEVEUR,
                             _LaTournee.CODE,
                             _LaTournee.CENTRE,
                             _LaTournee.LOCALISATION,
                             //_LaTournee.RELEVEUR,
                             _LaTournee.LIBELLE,
                             //_LaTournee.MATRICULEPIA,
                             _LaTournee.PRIORITE,
                             _LaTournee.USERMODIFICATION,
                             _LaTournee.DATEMODIFICATION,
                             _LaTournee.DATECREATION,
                             _LaTournee.USERCREATION,
                             DISPLAYLABEL = _LaTournee.CODE + "-" + _LaTournee.LIBELLE
                         }).OrderBy(o => o.CODE).Distinct();
                    }
                    else
                    {
                        query =
                        (from _LaTournee in _LstTournee
                         //where (_LaTournee.PK_CENTRE == pCentre ||_LaTournee.PK_CENTRE == Enumere.Generale )
                         select new
                         {
                             _LaTournee.PK_ID,
                             _LaTournee.FK_IDCENTRE,
                             //_LaTournee.FK_IDRELEVEUR,
                             _LaTournee.CODE,
                             _LaTournee.CENTRE,
                             _LaTournee.LOCALISATION,
                             //_LaTournee.RELEVEUR,
                             _LaTournee.LIBELLE,
                             //_LaTournee.MATRICULEPIA,
                             _LaTournee.PRIORITE,
                             _LaTournee.USERMODIFICATION,
                             _LaTournee.DATEMODIFICATION,
                             _LaTournee.DATECREATION,
                             _LaTournee.USERCREATION,
                             DISPLAYLABEL = _LaTournee.CODE + "-" + _LaTournee.LIBELLE
                         }).OrderBy(o => o.CODE).Distinct();
                    }
                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable RetourneToutCasIndex()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = from p in context.CASIND
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
                                                    p.ENQUETABLE ,
                                                    p.DATECREATION,
                                                    p.DATEMODIFICATION,
                                                    p.USERCREATION,
                                                    p.USERMODIFICATION,
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
        public static DataTable RetourneListeDenominationAll()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = null;
                    var _LstDenon = context.DENOMINATION;
                    query =
                    from _LaDeno in _LstDenon
                    select new
                    {
                        _LaDeno.PK_ID,
                        _LaDeno.CODE,
                        _LaDeno.LIBELLE
                    };
                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static DataTable RetourneCategorieClient(string pCategorieClient)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = null;
                    var _CATEGORIECLIENT = context.CATEGORIECLIENT;
                    if ((!string.IsNullOrEmpty(pCategorieClient)))
                    {
                        query =
                        from _LaCategorie in _CATEGORIECLIENT
                        where _LaCategorie.CODE == pCategorieClient
                        select new
                        {
                            _LaCategorie.CODE,
                            _LaCategorie.LIBELLE
                        };
                    }
                    else
                    {
                        query =
                        from _LaCategorie in _CATEGORIECLIENT
                        select new
                        {
                            _LaCategorie.CODE,
                            _LaCategorie.LIBELLE
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
        public static DataTable RetourneMarqueCompteur(string pMarqueCompteur)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = null;
                    if ((!string.IsNullOrEmpty(pMarqueCompteur)))
                    {
                        query =
                       from MARQUECOMPTEURs in context.MARQUECOMPTEUR
                       where MARQUECOMPTEURs.CODE == pMarqueCompteur
                       select new
                       {
                           MARQUECOMPTEURs.CODE,
                           MARQUECOMPTEURs.LIBELLE,
                           MARQUECOMPTEURs.DATECREATION,
                           MARQUECOMPTEURs.DATEMODIFICATION,
                           MARQUECOMPTEURs.USERCREATION,
                           MARQUECOMPTEURs.USERMODIFICATION
                       };
                    }
                    else
                    {
                        query =
                       from MARQUECOMPTEURs in context.MARQUECOMPTEUR
                       select new
                       {
                           MARQUECOMPTEURs.CODE,
                           MARQUECOMPTEURs.LIBELLE,
                           MARQUECOMPTEURs.DATECREATION,
                           MARQUECOMPTEURs.DATEMODIFICATION,
                           MARQUECOMPTEURs.USERCREATION,
                           MARQUECOMPTEURs.USERMODIFICATION
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

        public static DataTable RetourneTypeCompteurByProduit(string pProduit)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = null;
                  
                        query =
                       from TCOMPTs in context.TYPECOMPTEUR  
                       where TCOMPTs.PRODUIT == pProduit || string.IsNullOrEmpty(pProduit )
                       select new
                       {
                           TCOMPTs.PRODUIT,
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

        public static DataTable RetourneTousFrequence()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    var _FREQUENCE = context.FREQUENCE;
                    IEnumerable<object> query =
                    from f in _FREQUENCE
                    select new
                    {
                        f.CODE,
                        f.LIBELLE,
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

        public static DataTable RetourneTousMois()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    var _MOIS = context.MOIS;
                    IEnumerable<object> query =
                    from x in _MOIS
                    select new
                    {
                        x.CODE,
                        x.LIBELLE,
                        x.USERCREATION,
                        x.USERMODIFICATION,
                        x.DATECREATION,
                        x.DATEMODIFICATION
                    };
                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable RetourneTousModeRecepetion()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    var _MODERECEPETION = context.MODERECEPTION;
                    IEnumerable<object> query =
                    from x in _MODERECEPETION
                    select x;

                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable RechercheClient(string numeroClient, string nomClient, string centre, string compteur)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    var clients = context.CLIENT;
                    var canalisations = context.CANALISATION;
                    numeroClient = string.IsNullOrEmpty(numeroClient) ? "" : numeroClient.ToLower();
                    nomClient = string.IsNullOrEmpty(nomClient) ? "" : nomClient.ToLower();
                    centre = string.IsNullOrEmpty(centre) ? "" : centre.ToLower();
                    compteur = string.IsNullOrEmpty(compteur) ? "" : compteur.ToLower();

                    // ce hack est utilisé parce que le provider dotconnect n'arrive pas a effectuer 
                    // les autres methodes de comparaison de valeus nulles 
                    string vide = "";

                    IEnumerable<CsClient> query = from client in clients
                                                  //join canalisation in canalisations
                                                  //on new { client.CENTRE, CLIENT = client.REFCLIENT }
                                                  //equals new { canalisation.CENTRE, canalisation.CLIENT }

                                                  where (numeroClient.Equals(vide) || client.REFCLIENT.ToLower().Equals(numeroClient)) &&
                                                        (nomClient.Equals(vide) || client.NOMABON.ToLower().Contains(nomClient)) &&
                                                        (centre.Equals(vide) || client.CENTRE.ToLower().Equals(centre)) &&
                                                        (compteur.Equals(vide) 
                                                        //|| canalisation.COMPTEUR.ToLower().Equals(compteur)
                                                        )

                                                  select new CsClient
                                                  {
                                                      CENTRE = client.CENTRE,
                                                      //client.CLIENT,
                                                      ORDRE = client.ORDRE,
                                                      NOMABON = client.NOMABON,
                                                      TELEPHONE = client.TELEPHONE,
                                                      ADRMAND1 = client.ADRMAND1,
                                                      //COMPTEUR = canalisation.COMPTEUR
                                                  };

                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<CsUtilisateur > RechercheAgent(string MatriculeAgent, string NomAgent, string Idfonction)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    var utilisateurs = context.ADMUTILISATEUR;
                    var fonctions = context.FONCTION;

                    MatriculeAgent = string.IsNullOrEmpty(MatriculeAgent) ? "" : MatriculeAgent.ToLower();
                    NomAgent = string.IsNullOrEmpty(NomAgent) ? "" : NomAgent.ToLower();
                    Idfonction = string.IsNullOrEmpty(Idfonction) ? "" : Idfonction.ToLower();
                    //compteur = string.IsNullOrEmpty(compteur) ? "" : compteur.ToLower();

                    // ce hack est utilisé parce que le provider dotconnect n'arrive pas a effectuer 
                    // les autres methodes de comparaison de valeus nulles 
                    string vide = "";

                    IEnumerable<CsUtilisateur> query = from utilisateur in utilisateurs
                                                      where 
                                                      //(Idfonction.Equals(vide) || utilisateur.FONCTION1.CODE.ToLower().Equals(Idfonction)) &&
                                                            (MatriculeAgent.Equals(vide) || utilisateur.MATRICULE.ToLower().StartsWith(MatriculeAgent)) &&
                                                            (NomAgent.Equals(vide) || utilisateur.LIBELLE.ToLower().Contains(NomAgent))

                                                       select new CsUtilisateur
                                                      {
                                                          CENTRE  = utilisateur.CENTRE1.CODE ,
                                                          //CodeFonction = utilisateur.FONCTION1.CODE ,
                                                          //Fonction = utilisateur.FONCTION1.ROLEDISPLAYNAME,
                                                          MATRICULE = utilisateur.MATRICULE,
                                                          PASSE  = utilisateur.PASSE,
                                                          NOM  = utilisateur.LIBELLE
                                                      };

                    return query.ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<CsFonction> RetourneListeFonctions()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    var fonctions = context.FONCTION;

                    IEnumerable<CsFonction> query = from fonction in fonctions

                                                    select new CsFonction
                                                    {
                                                        CODE = fonction.CODE,
                                                        ROLEDISPLAYNAME = fonction.ROLEDISPLAYNAME,
                                                        ESTADMIN = (bool)fonction.ESTADMIN
                                                    };

                    return query.ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable RetourneInformationsEntreprise()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    var Entreprise = context.ENTREPRISE;
                    IEnumerable<object> query = from enteprise in Entreprise select enteprise;
                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<CsTa> SelectionneCategoriesClient()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    var categories = context.CATEGORIECLIENT;

                    IEnumerable<CsTa> ListeCategories = from cat in categories
                                                        select new CsTa
                                                        {
                                                            CODE = cat.CODE,
                                                            LIBELLE = cat.LIBELLE,
                                                            NUM=cat.PK_ID
                                                        };

                    return ListeCategories.ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable RetourneTousMoratoires()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    var moratoire = context.MORATOIRE.Where(p => p.STATUS.Value == Enumere.StatusActive);

                    IEnumerable<object> query = from x in moratoire
                                                select new
                                                {
                                                    x.CENTRE,
                                                    x.CLIENT,
                                                    x.ORDRE,
                                                    x.PK_ID,
                                                    x.STATUS
                                                };
                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable RetourneInfoReleveur(string centre, string matricule)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    var releveur = context.RELEVEUR.Where(r => r.SUPPRIMER == false || r.SUPPRIMER == null);//.Where(c => c.PK_CENTRE == centre);

                    if (releveur == null || releveur.Count() == 0)
                        return null;

                    List<RELEVEUR> _releveur = new List<RELEVEUR>();
                    foreach (var r in releveur)
                    {
                        if (r.MATRICULE == matricule || string.IsNullOrEmpty(matricule) && (r.CENTRE == centre || string.IsNullOrEmpty(centre)))
                            _releveur.Add(r);
                    }

                    IEnumerable<object> query = from x in _releveur
                                                select new
                                                {
                                                    CENTRE = x.CENTRE,
                                                    RELEVEUR = x.CODE ,
                                                    MATRICULE = x.MATRICULE,
                                                    x.FERMEQUOT,
                                                    x.FERMEREAL,
                                                    x.PORTABLE
                                                };
                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable RetourneTousCoperOD()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    var COPEROD = context.COPEROD;

                    IEnumerable<object> query = from c in COPEROD
                                                select new
                                                {
                                                    c.CODE,
                                                    c.LIBELLE,
                                                    c.LIBCOURT,
                                                    c.COMPTEANNEXE1,
                                                    c.COMPTGENE,
                                                    c.CTRAIT,
                                                    c.DC
                                                };
                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static int RetourneEvenementMax(string centre, string client, string Ordre, string produit, int point)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    int maxEvent = context.EVENEMENT.Where(e => e.CENTRE == centre && e.CLIENT == client && e.ORDRE == Ordre
                                                            && e.PRODUIT == produit && e.POINT == point).Max(e => e.NUMEVENEMENT);

                    return maxEvent;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable RetourneTousMatriculeDesTournee()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    var tournee = (from t in context.TOURNEEPIA  select new { t.ADMUTILISATEUR.MATRICULE  }).Distinct();
                    var matricule = context.ADMUTILISATEUR;

                    IEnumerable<object> query = from t in tournee
                                                join a in matricule on t.MATRICULE  equals a.MATRICULE
                                                select new
                                                {

                                                    Code = a.MATRICULE,
                                                    Libelle = a.LIBELLE
                                                };

                    return Galatee.Tools.Utility.ListToDataTable<object>(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable RetourneToutesFactureClient(string centre, string client, string ordre)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> lclient = from c in context.LCLIENT
                                                  where c.DC == Enumere.Debit && c.COPER == Enumere.CoperRNA
                                                  select new
                                                  {

                                                  };

                    return Galatee.Tools.Utility.ListToDataTable<object>(lclient);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string RetourneIdCampagneCoupure()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    DateTime date = DateTime.Now.Date;
                    string suffixe;
                    string[] datearray = date.ToShortDateString().Split('/');
                    string debutdate = datearray[2] + datearray[1] + datearray[0];
                    //string debutdate = date.Year.ToString() + date.Month.ToString() + date.Day.ToString();

                    var exist = context.LCLIENT.Where(l => l.IDCOUPURE.Contains(debutdate));


                    if (exist == null || exist.Count() == 0)
                        suffixe = "01";
                    else
                    {
                        int? maxsuffixe = int.Parse(exist.Max(l => l.IDCOUPURE).PadRight(2));

                        if (maxsuffixe.ToString().Length == 1)
                        {
                            suffixe = "0" + (maxsuffixe + 1).ToString();
                        }
                        else
                            return suffixe = (maxsuffixe + 1).ToString();

                    }

                    return debutdate + suffixe;


                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public static DataTable RetournInfoFectForCompletArchive(string centre, string client, string ordre, string periode, string numFacture)
        //{
        //    using(galadbEntities context=new galadbEntities())
        //    {
        //    IEnumerable<object> query=from entfac in context.ENTFAC
        //                              where 
        //    return Galatee.Tools.Utility.ListToDataTable<object>(query);
        //    }
        //}


        public static DataTable ChargerLotri()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = (from t in context.LOTRI
                                                 select new
                                                 {
                                                     t.CENTRE,
                                                     t.BASE,
                                                     t.PERIODE,
                                                     t.NUMLOTRI,
                                                     t.PK_ID
                                                 }).Distinct();

                    return Galatee.Tools.Utility.ListToDataTable<object>(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable retourneObservationCoupure()
        {
            using (galadbEntities context = new galadbEntities())
            {
                var observation = from o in context.OBSERVATION
                                  select new
                                  {
                                      o.CODE,
                                      o.LIBELLE
                                  };
                return Galatee.Tools.Utility.ListToDataTable<object>(observation);
            }
        }

        public static DataTable RetouneDevisParNum(string NumDevis)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    return new DataTable();
                    //DEMANDE devis = context.DEMANDE.First(d => d.NUMDEM == NumDevis);
                    //ETAPEDEVIS etape = devis.ETAPEDEVIS;

                    //if (etape.NUMETAPE != Enumere.EtapeEncaissementDevis)
                    //    return null;

                    //var demandedevis = context.DEMANDEDEVIS.Where(dd => dd.FK_IDDEVIS == devis.PK_ID);
                    //var deposit = context.DEPOSIT;
                    //var query = from dd in demandedevis
                    //            join dp in deposit on dd.NUMDEVIS equals dp.NUMDEVIS
                    //            select new
                    //            {
                    //                SOLDEFACTURED = dd.DEVIS.MONTANTTTC,
                    //                dd.NOM,
                    //                dd.CLIENT,
                    //                EXIGIBLE = dd.DEVIS.DATEREGLEMENT,
                    //                CENTRE=dd.DEVIS.CODECENTRE,
                    //                ORDRE =dd.ORDRECLIENT,
                    //                AVANCED = dp.MONTANTDEPOSIT,
                    //                MONTANTFACTURED = (dd.DEVIS.MONTANTTTC - dp.MONTANTDEPOSIT),
                    //                dd.DEVIS.ETAPEDEVIS.NUMETAPE,
                    //                dd.FK_IDCATEGORIECLIENT,
                    //                dd.DEVIS.NUMDEVIS,
                    //                devis.FK_IDETAPEDEVIS,
                    //                devis.FK_IDPRODUIT,
                    //                devis.FK_IDTYPEDEVIS
                    //                  };
                    //return Galatee.Tools.Utility.ListToDataTable<object>(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static object OpenTransaction(Enumere.DataBase pBase)
        {
            try
            {
                switch (pBase)
                {
                    case Enumere.DataBase.Galadb:
                      galadbEntities context= new galadbEntities();
                     
                        return new galadbEntities();
                    case Enumere.DataBase.Abo07:
                        return new ABO07Entities();;
                    default:
                        return new galadbEntities();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
