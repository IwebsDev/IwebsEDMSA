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
    public class CommonProcedures
    {
        public static DataTable RetourneTousFourniture()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    var fourniture = context.FOURNITURE;

                    var Produit = context.PRODUIT;

                    IEnumerable<object> query = from x in fourniture
                                                select new
                                                {

                                                    //x.CODE,
                                                    //x.DESIGNATION,
                                                    x.PRODUIT,
                                                    // LIBELLEPRODUIT= x.PRODUIT1.LIBELLE,
                                                    //x.COUTUNITAIRE_FOURNITURE,
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

        public static DataTable RetourneTousAppareils()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    var Appareils = context.APPAREILS;

                    IEnumerable<object> query = from x in Appareils
                                                select new
                                                {

                                                    x.CODEAPPAREIL,
                                                    x.DESIGNATION,
                                                    x.DETAILS,
                                                    x.PK_ID,
                                                    x.TEMPSUTILISATION,
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

        public static DataTable RetourneTousTDEM()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    var tdem = context.TYPEDEMANDE;

                    IEnumerable<object> query = from x in tdem
                                                select new
                                                {

                                                    x.LIBELLE,
                                                    x.CODE,
                                                    x.PK_ID,
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


        //public static DataTable RetourneTousCoper()
        //{
        //    try
        //    {
        //        using (galadbEntities context = new galadbEntities())
        //        {
        //            var Appareils = context.COPER;

        //            IEnumerable<object> query = from x in Appareils
        //                                        select new
        //                                        {

        //                                            x.CODE,
        //                                            x.LIBCOURT,
        //                                            x.LIBELLE,
        //                                            x.COMPTGENE,
        //                                            x.DC,
        //                                            x.PK_ID,
        //                                            x.DATECREATION,
        //                                            x.DATEMODIFICATION,
        //                                            x.USERCREATION,
        //                                            x.USERMODIFICATION
        //                                        };
        //            return Galatee.Tools.Utility.ListToDataTable(query);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public static DataTable RetourneTousCoutCoper()
        //{
        //    try
        //    {
        //        using (galadbEntities context = new galadbEntities())
        //        {
        //            var Appareils = context.COUTCOPER;
                   

        //            IEnumerable<object> query = from x in Appareils
        //                                        select new
        //                                        {

        //                                            x.CENTRE,
        //                                  LibelleCentre=x.CENTRE1.LIBELLE,
        //                                            x.PRODUIT,
        //                                  LibelleProduit=x.PRODUIT1.LIBELLE,
        //                                            x.PUISSANCE,
        //                                            x.TYPETARIF,
        //                                  LibelleTYPETARIF=x.TYPETARIF1.LIBELLE,
        //                                            x.PK_ID,
        //                                            x.COPER,
        //                                  LibelleCoper=x.COPER1.LIBELLE,
        //                                            x.MONTANT,
        //                                            x.DIAMETRE,
        //                                  LibelleDiametre=x.DIAMETRECOMPTEUR.LIBELLE,
        //                                            x.SUBVENTIONNEE,
        //                                            x.DMAJ,
        //                                            x.FK_IDCENTRE,
        //                                            x.FK_IDCOPER,
        //                                            x.FK_IDDIAMETRECOMPTEUR,
        //                                            x.FK_IDPRODUIT,
        //                                            x.FK_IDTYPETARIF,
        //                                            x.DATECREATION,
        //                                            x.DATEMODIFICATION,
        //                                            x.USERCREATION,
        //                                            x.USERMODIFICATION
        //                                        };
        //            return Galatee.Tools.Utility.ListToDataTable(query);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public static DataTable RetourneTousCoperDemande()
        //{
        //    try
        //    {
        //        using (galadbEntities context = new galadbEntities())
        //        {
        //            var Appareils = context.COPERDEMANDE;

        //            IEnumerable<object> query = from x in Appareils
        //                                        select new
        //                                        {

        //                                            x.CENTRE,
        //                                            x.PRODUIT,
        //                           libelleProduit = x.PRODUIT1.LIBELLE, 
        //                           libelleCentre=   x.CENTRE1.LIBELLE,
        //                                            x.PK_ID,
        //                                            x.TAXE,
        //                                libelleTaxe=x.TAXE1.LIBELLE,
        //                                            x.COPER,
        //                        libelleCoper=x.COPER1.LIBELLE,
        //                                            x.MONTANT,
        //                                            x.TDEM,
        //                                            x.DMAJ,
        //                                            x.AUTO,
        //                                            x.OBLIG,
        //                                            x.FK_IDCENTRE,
        //                                            x.FK_IDCOPER,
        //                                            x.FK_IDPRODUIT,
        //                                            x.FK_IDTDEM,
        //                                            x.FK_IDTAXE,
        //                                            x.DATECREATION,
        //                                            x.DATEMODIFICATION,
        //                                            x.USERCREATION,
        //                                            x.USERMODIFICATION
        //                                        };
        //            return Galatee.Tools.Utility.ListToDataTable(query);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public static DataTable RetourneTousCentres()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    var centres = context.CENTRE;

                    IEnumerable<object> query = from x in centres
                                                select new {

                                                    x.CODESITE ,
                                                    x.LIBELLE,
                                                    x.CODE,
                                                    x.PK_ID,
                                                    x.FK_IDCODESITE 
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
                                                    x. NUMCAISSE ,
                                                    x. ACQUIT ,
                                                    x. BORDEREAU ,
                                                    x. FONDCAISSE ,
                                                    x. COMPTE ,
                                                    x. LIBELLE ,
                                                    x. TYPECAISSE ,
                                                    x. USERCREATION ,
                                                    x. DATECREATION ,
                                                    x. DATEMODIFICATION ,
                                                    x. USERMODIFICATION ,
                                                    x. PK_ID ,
                                                    x. CENTRE ,
                                                    x. FK_IDCENTRE ,
                                                    x. ESTATTRIBUEE,
                                                    LIBELLECENTRE = x.CENTRE1.LIBELLE,
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
                                                    x.FRAISDERETOUR ,
                                                    x.DATECREATION,
                                                    x.DATEMODIFICATION,
                                                    x.USERCREATION,
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

        public static DataTable RetourneTousCodeRegroupement()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    var REGCLI = context.REGROUPEMENT;

                    IEnumerable<object> query = from x in REGCLI
                                                select new {
                                                    x.CODE ,
                                                    x.NOM,
                                                    x.ADRESSE,
                                                    x.DATECREATION,
                                                    x.DATEMODIFICATION,
                                                    x.USERCREATION,
                                                    x.USERMODIFICATION,
                                                    x.PK_ID,
                                                    LIBELLE= x.NOM 
                                                };
                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable RetourneCodeRegroupementByCampagne(int IdCampagne)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    var REGCLI = context.REGROUPEMENT;
                    var CAMPAGNEGC = context.CAMPAGNEGC;

                    IEnumerable<object> query = from x in REGCLI
                                                join c in CAMPAGNEGC
                                                on x.PK_ID equals c.FK_IDREGROUPEMENT

                                                where c.PK_ID == IdCampagne
                                                select new
                                                {
                                                    x.CODE,
                                                    x.NOM,
                                                    x.ADRESSE,
                                                    x.DATECREATION,
                                                    x.DATEMODIFICATION,
                                                    x.USERCREATION,
                                                    x.USERMODIFICATION,
                                                    x.PK_ID,
                                                };
                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable RetourneTousPayeur()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    var Payeur = context.PAYEUR ;
                    IEnumerable<object> query = from x in Payeur
                                                select new {
                                                    x.PAYEUR1,
                                                    x.NOM,
                                                    x.TELEPHONE,
                                                    x.FAX,
                                                    x.EMAIL,
                                                    x.ACCEPTFACTURE,
                                                    x.ACCEPTPENALITE,
                                                    x.REGISTRE,
                                                    x.CODENATIONAL,
                                                    x.ACTIVITE,
                                                    x.DISTRIBUTION,
                                                    x.CENTRE,
                                                    x.COMMUNE,
                                                    x.QUARTIER,
                                                    x.CODERUE,
                                                    x.NUMRUE,
                                                    x.NOMRUE,
                                                    x.COMPRUE,
                                                    x.CPOS,
                                                    x.CADR,
                                                    x.PK_ID, 
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
                    var Site= context.SITE;

                    IEnumerable<object> query = from x in Site
                                                select new { 
                                                    x.CODE ,
                                                    x.DATECREATION ,
                                                    x.DATEMODIFICATION ,
                                                    x.LIBELLE ,
                                                    x.PK_ID ,
                                                    x.PWD ,
                                                    x.SERVEUR ,
                                                    x.USERCREATION ,
                                                    x.USERID ,
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

        public static DataTable RetourneTousParametreGenerauxByCode(string code)
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
                    var fonction = context.FONCTION;

                    IEnumerable<object> query = from x in fonction
                                                select new
                                                { 
                                                    x.CODE ,
                                                    x.DATECREATION ,
                                                    x.DATEMODIFICATION ,
                                                    x.ESTADMIN ,
                                                    x.PK_ID ,
                                                    x.ROLEDISPLAYNAME ,
                                                    x.ROLENAME ,
                                                    x.USERCREATION ,
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
                                                 x.CODE,
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
                                                select new {
                                                    x.CODE,
                                                    x.LIBCOURT,
                                                    x.LIBELLE,
                                                    x.COMPTGENE,
                                                    x.DC,
                                                    x.CTRAIT,
                                                    x.DMAJ,
                                                    x.TRANS,
                                                    x.COMPTEANNEXE1,
                                                    x.USERCREATION,
                                                    x.DATECREATION,
                                                    x.DATEMODIFICATION,
                                                    x.USERMODIFICATION,
                                                    x.PK_ID,
                                                    x.ISOD
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
                                                    MenuID = _admMenu.PK_ID ,
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
                        produit.PK_ID,
                        produit.CODE ,
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
                using (galadbEntities context = new galadbEntities())
                {
                    //IEnumerable<object> query = null;
                    //var _SpSite = context.SPESITE;

                    //if ((!string.IsNullOrEmpty(pSite)))
                    //{
                    //    query =
                    //    from LaSpSite in _SpSite
                    //    where LaSpSite.SITE == pSite
                    //    select _SpSite;
                    //}
                    //return Galatee.Tools.Utility.ListToDataTable(query);
                    return new DataTable();
                };
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
                    //var Directeur = context.DIRECTEUR;
                    //IEnumerable<CsCentre> query =
                    //from directeur in Directeur
                    //select new CsCentre
                    //{
                    //    CODECENTRE = directeur.CENTRE,
                    //    Libelle = directeur.LIBELLE
                    //};

                    //return query.ToList();

                    return new List<CsCentre>();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable RetourneTaxe()
        {
            try
            {
                IEnumerable<object> query = null;
                using (galadbEntities context = new galadbEntities())
                {
                    var _CTAX = context.TAXE;
                    query =
                    from _LaTaxe in _CTAX
                    where _LaTaxe.FINAPPLICATION >= System.DateTime.Today.Date 
                    select new
                    {
                        _LaTaxe.CODE,
                        _LaTaxe.LIBELLE,
                        _LaTaxe.TAUX,
                        _LaTaxe.TYPETAXE,
                        _LaTaxe.COMPTE,
                        _LaTaxe.DEBUTAPPLICATION,
                        _LaTaxe.FINAPPLICATION,
                        _LaTaxe.DATECREATION,
                        _LaTaxe.DATEMODIFICATION,
                        _LaTaxe.USERCREATION,
                        _LaTaxe.USERMODIFICATION,
                        _LaTaxe.FK_IDTYPETAXE,
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

        public static DataTable RetourneCodeConsomateur()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = null;
                    var _CONSOMMATEUR = context.CODECONSOMMATEUR;
                        query =
                        from _LaConso in _CONSOMMATEUR
                        select new
                        {
                            _LaConso.CODE ,
                            _LaConso.LIBELLE,
                            _LaConso.DATECREATION,
                            _LaConso.DATEMODIFICATION,
                            _LaConso.USERCREATION,
                            _LaConso.USERMODIFICATION,
                            _LaConso.PK_ID
                        };
                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable RetourneNatureClient()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    //IEnumerable<object> query = null;
                    //var _NATURECLIENT = context.NATURECLIENT;
                    //    query =
                    //    from _LaNature in _NATURECLIENT
                    //    select new
                    //    {
                    //        _LaNature.CODE,
                    //        _LaNature.LIBELLE,
                    //        _LaNature.USERCREATION,
                    //        _LaNature.USERMODIFICATION,
                    //        _LaNature.DATECREATION,
                    //        _LaNature.DATEMODIFICATION,
                    //        _LaNature.PK_ID 
                    //    };
                    //return Galatee.Tools.Utility.ListToDataTable(query);

                    return new DataTable();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable RetourneFermable()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = null;
                    var _RELANCE = context.RELANCE;
                    query =
                    from _LaRel in _RELANCE
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

        public static DataTable RetourneNationnalite()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = null;
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
                        return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public static DataTable RetourneTypeDeBranchement()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = null;
                    var _DIAMETREBRANCHEMENT = context.TYPEBRANCHEMENTPARPRODUIT  ;
                        query =
                        from x in _DIAMETREBRANCHEMENT
                        select new
                        {
                            PRODUIT = x.PRODUIT.CODE,
                            LIBELLEPRODUIT = x.PRODUIT.LIBELLE ,
                            x.FK_IDPRODUIT ,
                            CODE = x.TYPEBRANCHEMENT.CODE,
                            x.TYPEBRANCHEMENT.LIBELLE,
                            x.TYPEBRANCHEMENT.PK_ID 
                        };
                        return Galatee.Tools.Utility.ListToDataTable(query);
                  }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static DataTable ChargerTarifParReglageCompteur()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = null;
                    var _TarifDiametre = context.TYPETARIFPARREGLAGECOMPTEUR ;
                    query =
                    from x in _TarifDiametre
                    select new
                    {
                        x.TYPETARIF.CODE,
                        x.TYPETARIF.LIBELLE ,
                        x.TYPETARIF.PRODUIT,
                        x.TYPETARIF.FK_IDPRODUIT ,
                        REGLAGECOMPTEUR =x.REGLAGECOMPTEUR.CODE,
                        LIBELLEREGLAGECOMPTEUR = x.REGLAGECOMPTEUR.LIBELLE,
                        x.FK_IDREGLAGECOMPTEUR ,
                        x.FK_IDTYPETARIF ,
                        PK_ID = x.FK_IDTYPETARIF 
                    };
                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static DataTable ChargerTarifParCategorie()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = null;
                    var _TarifCategorie= context.TYPETARIFPARCATEGORIE;
                    query =
                    from x in _TarifCategorie
                    select new
                    {
                        x.TYPETARIF.CODE,
                        x.TYPETARIF.PRODUIT,
                        x.TYPETARIF.FK_IDPRODUIT,

                        LIBELLE = x.TYPETARIF.LIBELLE ,

                        CATEGORIE = x.CATEGORIECLIENT.CODE,
                        LIBELLECATEGORIE = x.CATEGORIECLIENT.LIBELLE,

                        x.FK_IDCATEGORIE ,
                        x.FK_IDTYPETARIF,
                    };
                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static DataTable RetournePuissanceReglageCompteur()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = null;
                    var _LstPuissance = context.PUISSANCEPARREGLAGECOMPTEUR;
                    query =
                    (from _LaPuissance in _LstPuissance
                     select new
                     {
                         FK_IDPUISSANCE = _LaPuissance.FK_IDPUISSANCE,
                         _LaPuissance.PUISSANCESOUSCRITE.PRODUIT,
                         _LaPuissance.PUISSANCESOUSCRITE.CODE,
                         _LaPuissance.PUISSANCESOUSCRITE.FK_IDPRODUIT,
                         _LaPuissance.PUISSANCESOUSCRITE.VALEUR ,
                         FK_IDREGLAGECOMPTEUR = _LaPuissance.REGLAGECOMPTEUR.PK_ID,
                         LIBELLEREGLAGECOMPTEUR = _LaPuissance.REGLAGECOMPTEUR.LIBELLE,
                         REGLAGECOMPTEUR = _LaPuissance.REGLAGECOMPTEUR.CODE
                     }).Distinct();
                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable RetournePuissanceInstalle()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = null;
                    var _PUISSANCEINSTALLEE = context.PUISSANCEINSTALLEE;
                    query =
                    from x in _PUISSANCEINSTALLEE
                    select new
                    {
                        x.PK_ID,
                        x.PRODUIT,
                        x.CODE,
                        x.VALEUR,
                        x.KPERTEACTIVE1,
                        x.KPERTEACTIVE2,
                        x.KPERTEREACTIVE1,
                        x.KPERTEREACTIVE2,
                        x.DATECREATION,
                        x.USERCREATION,
                        x.DATEMODIFICATION,
                        x.USERMODIFICATION,
                        x.FK_IDPRODUIT,
                        LIBELLEPRODUIT= x.PRODUIT1.LIBELLE 
                    };
                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable RetourneDiametreBranchement()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = null;
                    var _DIAMETREBRANCHEMENT = context.TYPEBRANCHEMENT;
                    query =
                    from x in _DIAMETREBRANCHEMENT
                    select new
                    {
                        x.CODE,
                        x.LIBELLE,
                        x.DATECREATION,
                        x.DATEMODIFICATION,
                        x.USERCREATION,
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

        public static DataTable RetourneMaterielBranchement()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = null;
                    var _MATERIELBRANCHEMENT = context.MATERIELBRANCHEMENT;
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
                            x.USERMODIFICATION,
                            x.PK_ID ,
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
        public static DataTable RetourneListeCentre()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = null;
                    var _LstCentre= context.CENTRE ;

                    query =
                    from x in _LstCentre
                    select new
                    {
                        x.  CODE ,
                        x.  LIBELLE ,
                        x.  TYPECENTRE ,
                        x.  CODESITE ,
                        x.  ADRESSE ,
                        x.  TELRENSEIGNEMENT ,
                        x.  TELDEPANNAGE ,
                        x. NUMEROAUTOCLIENT ,
                        x. GESTIONAUTOAVANCECONSO ,
                        x. GESTIONAUTOFRAIS ,
                        x. NUMEROFACTUREPARCLIENT ,
                        x.  DATECREATION ,
                        x. DATEMODIFICATION ,
                        x.  USERCREATION ,
                        x.  USERMODIFICATION ,
                        x. NUMERODEMANDE ,
                        x. PK_ID ,
                        x. FK_IDCODESITE ,
                        x. FK_IDTYPECENTRE ,
                        x. FK_IDNIVEAUTARIF ,
                        x. COMPTEECLAIRAGEPUBLIC ,
                        x. NUMEROFACTURE ,
                        x. DATEFIN,
                        CODENIVEAUTARIF = x.NIVEAUTARIF.CODE 
                    };

                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static DataTable RetourneListeCommune()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = null;
                    var _LstCommuneInit = context.COMMUNE;

                    query =
                    from _LaCommune in _LstCommuneInit
                    select new
                    {
                        _LaCommune.CENTRE,
                        _LaCommune.CODE,
                        _LaCommune.LIBELLE,
                        _LaCommune.PK_ID,
                        _LaCommune.FK_IDCENTRE,
                        _LaCommune.DATECREATION ,
                        _LaCommune.DATEMODIFICATION ,
                        _LaCommune.USERCREATION ,
                        _LaCommune.USERMODIFICATION 
                    };

                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable RetourneListeQuartier()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = null;
                    var _LstQuartierInit = context.QUARTIER;
                    query =
                    from _LeQuartier in _LstQuartierInit
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

        public static DataTable RetourneListeRue()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = null;
                    var _LstRue = context.RUES;
                    query =
                    from _LaRue in _LstRue
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

        public static DataTable RetourneListeTournee( )
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = null;
                    var _LstReleveur = context.RELEVEUR;
                    var _LstTournee = context.TOURNEE.Where(r => r.SUPPRIMER == false || r.SUPPRIMER == null);
                    var _LstTourneeReleve = context.TOURNEERELEVEUR;
                        query =
                        (from x in _LstTournee
                         join n in _LstTourneeReleve on new { x.PK_ID  }
                                                   equals new { PK_ID = n.FK_IDTOURNEE   } into _RecuCaisse
                         from p in _RecuCaisse.DefaultIfEmpty()
                         select new
                         {
                             x.PK_ID,
                             x.CODE,
                             x.CENTRE,
                             x.LOCALISATION,
                             x.LIBELLE,
                             x.PRIORITE,
                             x.USERMODIFICATION,
                             x.DATEMODIFICATION,
                             x.DATECREATION,
                             x.USERCREATION,
                             x.FK_IDCENTRE,
                             x.FK_IDLOCALISATION ,
                             //NOMRELEVEUR = p.RELEVEUR.ADMUTILISATEUR.LIBELLE,
                             //p.FK_IDRELEVEUR ,
                             DISPLAYLABEL = x.CODE + "-" + x.LIBELLE 
                        
                         }).OrderBy(o => o.CODE).Distinct();
                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable RetourneListeTourneePia(List<int> lstIdCentre)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = null;
                    var _LstTournee = context.TOURNEE.Where(r => r.SUPPRIMER == false || r.SUPPRIMER == null);
                    var _LstTourneeReleve = context.TOURNEEPIA ;
                    query =
                    (from x in _LstTournee
                     join n in _LstTourneeReleve on new { x.PK_ID }
                                               equals new { PK_ID = n.FK_IDTOURNEE  } into _RecuCaisse
                     from p in _RecuCaisse.DefaultIfEmpty()
                     where lstIdCentre.Contains(x.FK_IDCENTRE)
                     select new
                     {
                         x.PK_ID,
                         x.CODE,
                         x.CENTRE,
                         x.LOCALISATION,
                         x.LIBELLE,
                         x.PRIORITE,
                         x.USERMODIFICATION,
                         x.DATEMODIFICATION,
                         x.DATECREATION,
                         x.USERCREATION,
                         x.FK_IDCENTRE,
                         x.FK_IDLOCALISATION,
                         NOMAGENTPIA = p.ADMUTILISATEUR.LIBELLE,
                         //FK_IDADMUTILSATEUR = p.FK_IDADMUTILSATEUR,
                         MATRICULEPIA = p.ADMUTILISATEUR.MATRICULE ,
                     }).OrderBy(o => o.CODE ).Distinct();
                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable RetourneListeUtiliasteurPia(int IdCentre,string CodeFonction)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    //int idProfil = context.PROFIL.FirstOrDefault(t => t.FONCTION.CODE == CodeFonction).PK_ID;
                    int idProfil = context.PROFIL.FirstOrDefault(t => t.FONCTION.CODE == Enumere.CodeFonctionPIA_E || t.FONCTION.CODE == Enumere.CodeFonctionPIA_O).PK_ID;

                    IEnumerable<object> query = null;
                    var _LstUser = context.ADMUTILISATEUR;
                    query =
                    from x in _LstUser
                    from h in x.CENTREDUPROFIL
                    where h.FK_IDPROFIL == idProfil && h.FK_IDCENTRE == IdCentre 
                     select new
                     {
                         x.PK_ID,
                         x.LIBELLE ,
                         x.MATRICULE ,
                         h.FK_IDCENTRE 
                     };
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
                                                    p.ENQUETABLE ,
                                                    p.SAISIECONSO,
                                                    p.DATECREATION,
                                                    p.DATEMODIFICATION,
                                                    p.USERCREATION,
                                                    p.USERMODIFICATION,
                                                    p.PK_ID,
                                                    p.FK_IDTYPEFACTURATIONAPRESENQUETE,
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
                    from x in _LstDenon
                    select new
                    {
                        x.CODE,
                        x.LIBELLE,
                        x.DATECREATION,
                        x.DATEMODIFICATION,
                        x.USERCREATION,
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
        public static DataTable RetourneCategorieClient()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = null;
                    var _CATEGORIECLIENT = context.CATEGORIECLIENT;
                        query =
                        from x in _CATEGORIECLIENT
                        select new
                        {
                            x.CODE ,
                            x.LIBELLE ,
                            x.DATECREATION ,
                            x.DATEMODIFICATION ,
                            x.USERCREATION ,
                            x.USERMODIFICATION ,
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

        public static DataTable RetourneNature()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    //IEnumerable<object> query = null;
                    //var _NATURE = context.NATURE;
                    //query =
                    //from x in _NATURE
                    //select new
                    //{
                    //    x.CODE,
                    //    x.LIBCOURT,
                    //    x.LIBELLE,
                    //    x.COMPTGENE,
                    //    x.DC,
                    //    x.CTRAIT,
                    //    x.TRANS,
                    //    x.COPER,
                    //    x.USERCREATION,
                    //    x.DATECREATION,
                    //    x.DATEMODIFICATION,
                    //    x.USERMODIFICATION,
                    //    x.PK_ID
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
                           MARQUECOMPTEURs.USERMODIFICATION,
                           MARQUECOMPTEURs.PK_ID 

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
                           MARQUECOMPTEURs.USERMODIFICATION,
                           MARQUECOMPTEURs.PK_ID 
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
                       where TCOMPTs.PRODUIT == pProduit || string.IsNullOrEmpty(pProduit)
                       select new
                       {
                           TCOMPTs.CODE,
                           TCOMPTs.PRODUIT,
                           TCOMPTs.LIBELLE,
                           TCOMPTs.DATECREATION,
                           TCOMPTs.DATEMODIFICATION,
                           TCOMPTs.USERCREATION,
                           TCOMPTs.USERMODIFICATION,
                           TCOMPTs.PK_ID,
                           TCOMPTs.FK_IDPRODUIT
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
                    var _FREQUENCE = context.PERIODICITE ;
                    IEnumerable<object> query =
                    from f in _FREQUENCE
                    select new
                    {   f.PK_ID ,
                        f.CODE ,
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
                    {   x.PK_ID ,
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
                    select new 
                    { 
                        x.DATECREATION ,
                        x.DATEMODIFICATION ,
                        x.LIBELLE ,
                        x.PK_ID ,
                        x.USERCREATION ,
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
                                                  //on new { client.PK_ID }
                                                  //equals new { canalisation.CENTRE, canalisation.CLIENT }

                                                  where (numeroClient.Equals(vide) || client.REFCLIENT.ToLower().Equals(numeroClient)) &&
                                                        (nomClient.Equals(vide) || client.NOMABON.ToLower().Contains(nomClient)) &&
                                                        (centre.Equals(vide) || client.CENTRE.ToLower().Equals(centre)) &&
                                                        (compteur.Equals(vide) 
                                                        //|| 
                                                        //canalisation.COMPTEUR.ToLower().Equals(compteur)
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

        public static List<CsTournee> RetourneToutesTournee()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    var tournees = context.TOURNEE;

                    IEnumerable<CsTournee> query = from tournee in tournees
                                                orderby tournee.CODE
                                                select new CsTournee 
                                                {
                                                    CODE  = tournee.CODE,
                                                    LIBELLE  = tournee.LIBELLE,
                                                    //Libelle = "(" + tournee.CODE + ")" + " " + tournee.LIBELLE
                                                };

                    return query.ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public static DataTable RetourneToutesBranches()
        //{
        //    try
        //    {
        //        using (galadbEntities context = new galadbEntities())
        //        {
        //            var _MOIS = context.ta;
        //            IEnumerable<object> query =
        //            from x in _MOIS
        //            select new
        //            {
        //                x.PK_MOIS,
        //                x.LIBELLE,
        //                x.USERCREATION,
        //                x.USERMODIFICATION,
        //                x.DATECREATION,
        //                x.DATEMODIFICATION
        //            };
        //            return Galatee.Tools.Utility.ListToDataTable(query);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public static List<CsCanalisation> RetourneCanalisations(string client, string centre)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    var canalisations = context.CANALISATION;
                    var ags = context.AG;


                    //IEnumerable<CsCanalisation> query = from canalisation in canalisations
                    //                                    join ag in ags
                    //                                    on new { canalisation.CLIENT, canalisation.CENTRE }
                    //                                    equals new { ag.CLIENT, ag.CENTRE }
                    //                                    where canalisation.CENTRE.Equals(centre) && canalisation.CLIENT.Equals(client)
                    //                                    orderby canalisation.PRODUIT
                    //                                    select new CsCanalisation
                    //                                    {
                    //                                        COMPTEUR = canalisation.COMPTEUR,
                    //                                        PRODUIT = canalisation.PRODUIT,
                    //                                        DIAMETRE = canalisation.DIAMETRE,
                    //                                        TOURNEE = ag.TOURNEE
                    //                                    };
                    return new List<CsCanalisation>();


                    //return query.ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<CsUtilisateur> RechercheAgent(string MatriculeAgent, string NomAgent, string Idfonction)
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

                    IEnumerable<CsUtilisateur > query = from utilisateur in utilisateurs
                                                      where 
                                                      //(Idfonction.Equals(vide) || utilisateur.FONCTION1.CODE.ToLower().Equals(Idfonction)) &&
                                                            (MatriculeAgent.Equals(vide) || utilisateur.MATRICULE.ToLower().StartsWith(MatriculeAgent)) &&
                                                            (NomAgent.Equals(vide) || utilisateur.LIBELLE.ToLower().Contains(NomAgent))

                                                        select new CsUtilisateur
                                                      {
                                                          //CodeFonction = utilisateur.FONCTION1.CODE ,
                                                          //Fonction = utilisateur.FONCTION1.ROLEDISPLAYNAME,
                                                          //Matricule = utilisateur.MATRICULE,
                                                          PASSE = utilisateur.PASSE,
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

        public static DataTable RetourneReleveurCentre(List<int> Liscentre)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                        //var releveur = context.RELEVEUR.Where(r => (r.SUPPRIMER == false || r.SUPPRIMER == null) && Liscentre.Contains(r.FK_IDCENTRE));----Ancien code
                    ////On récupere tous les réléveurs
                    //var releveur = context.RELEVEUR;

                    #region Recupération de sréléveur par centre du profil
                    //On récupére tous les réléveur du périmetre d'action de l'utilisateur connecté
                    IEnumerable<object> query = from  r in context.RELEVEUR 
                                        join u in context.ADMUTILISATEUR on   r.MATRICULE equals u.MATRICULE
                                        join pu in context.PROFILSUTILISATEUR on   u.PK_ID equals pu.FK_IDADMUTILISATEUR
                                        join p in context.PROFIL on  pu.FK_IDPROFIL equals p.PK_ID 
                                        join cp in context.CENTREDUPROFIL on   p.PK_ID equals  cp.FK_IDPROFIL
                                        //join c in context.CENTRE on cp.FK_IDCENTRE equals c.PK_ID
                                                where Liscentre.Contains(cp.FK_IDCENTRE) && p.CODE == "00110"
                                        select  new
                                                {
                                                    CENTRE = cp.CENTRE.CODE,
                                                    r.CODE,
                                                    r.MATRICULE,
                                                    r.FERMEQUOT,
                                                    r.FERMEREAL,
                                                    r.PORTABLE,
                                                    r.USERCREATION,
                                                    r.USERMODIFICATION,
                                                    r.DATECREATION,
                                                    r.DATEMODIFICATION,
                                                    FK_IDCENTRE=cp.FK_IDCENTRE,
                                                    FK_IDUSER = u.PK_ID,
                                                    r.SUPPRIMER,
                                                    NOMRELEVEUR = u.LIBELLE,
                                                    r.PK_ID
                                                };
                    #endregion

                    //IEnumerable<object> query = from x in releveur
                    //                            join c in centreduprofil
                    //                                   on x.MATRICULE equals c.ADMUTILISATEUR.MATRICULE
                    //                            select new
                    //                            {
                    //                                CENTRE = c.CENTRE.CODE,
                    //                                x.CODE,
                    //                                x.MATRICULE,
                    //                                x.FERMEQUOT,
                    //                                x.FERMEREAL,
                    //                                x.PORTABLE,
                    //                                x.USERCREATION,
                    //                                x.USERMODIFICATION,
                    //                                x.DATECREATION,
                    //                                x.DATEMODIFICATION,
                    //                                c.FK_IDCENTRE,
                    //                                FK_IDUSER = c.ADMUTILISATEUR.PK_ID,
                    //                                x.SUPPRIMER,
                    //                                NOMRELEVEUR = c.ADMUTILISATEUR.LIBELLE,
                    //                                x.PK_ID
                    //                            };

                    return Galatee.Tools.Utility.ListToDataTable(query.Distinct());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //public static DataTable RetourneReleveurCentre(List<int> Liscentre)
        //{
        //    try
        //    {
        //        using (galadbEntities context = new galadbEntities())
        //        {
        //            var releveur = context.RELEVEUR.Where(r => (r.SUPPRIMER == false || r.SUPPRIMER == null) && Liscentre.Contains(r.FK_IDCENTRE ));

        //            IEnumerable<object> query = from x in releveur
        //                                        select new
        //                                        {
        //                                            x.CENTRE,
        //                                            x.CODE ,
        //                                            x.MATRICULE ,
        //                                            x.FERMEQUOT ,
        //                                            x.FERMEREAL ,
        //                                            x.PORTABLE ,
        //                                            x.USERCREATION ,
        //                                            x.USERMODIFICATION ,
        //                                            x.DATECREATION ,
        //                                            x.DATEMODIFICATION ,
        //                                            x.FK_IDCENTRE ,
        //                                            x.FK_IDUSER ,
        //                                            x.SUPPRIMER ,
        //                                      NOMRELEVEUR= x.ADMUTILISATEUR.LIBELLE ,
        //                                            x.PK_ID 
        //                                        };
        //            return Galatee.Tools.Utility.ListToDataTable(query);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public static DataTable RetourneTousCoperOD()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    var COPEROD = context.COPER.Where(p=>p.ISOD== true ) ;

                    IEnumerable<object> query = from c in COPEROD
                                                select new
                                                {
                                               PK_COPER= c.CODE ,
                                                    c.LIBELLE,
                                                    c.LIBCOURT,
                                                    c.COMPTEANNEXE1,
                                                    c.COMPTGENE,
                                                    c.CTRAIT,
                                                    c.DC,
                                                    c.PK_ID 
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

        public static DataTable RetourneCompteurDispo(string pDiametre, string pProduit, string pNumeroCompteur, string pAnneeFabrication)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    var Canalisation = context.CANALISATION;
                    IEnumerable<object> query1 =
                    from canalisation in Canalisation
                    select new 
                    { 
                       canalisation.FK_IDCOMPTEUR 
                    };

                    IEnumerable<object> query =
                    from compt in context.COMPTEUR
                    where
                    //(string.IsNullOrEmpty(pDiametre) || compt.RE == pDiametre)
                    //&& 
                    (string.IsNullOrEmpty(pProduit) || compt.PRODUIT == pProduit)
                    && (string.IsNullOrEmpty(pNumeroCompteur) || compt.NUMERO == pNumeroCompteur)
                    && (string.IsNullOrEmpty(pAnneeFabrication) || compt.ANNEEFAB == pAnneeFabrication)
                    && !query1.Contains(compt.PK_ID)


                    select new
                    {
                        compt.PRODUIT,
                        compt.NUMERO,
                        compt.TYPECOMPTEUR,
                        //compt.TYPECOMPTAGE,
                        //compt.FK_IDCALIBRE ,
                        compt.MARQUE,
                        compt.COEFLECT,
                        compt.COEFCOMPTAGE,
                        compt.CADRAN,
                        compt.ANNEEFAB,
                        compt.FONCTIONNEMENT,
                        compt.PLOMBAGE,
                        compt.USERCREATION,
                        compt.DATECREATION,
                        compt.DATEMODIFICATION,
                        compt.USERMODIFICATION,
                        //compt.FK_IDTYPECOMPTAGE,
                        compt.FK_IDTYPECOMPTEUR,
                        compt.FK_IDPRODUIT,
                        compt.FK_IDMARQUECOMPTEUR,
                        compt.FK_IDCALIBRE ,
                        compt.PK_ID
                    };

                    return Galatee.Tools.Utility.ListToDataTable(query);
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

        public static string RetourneIdCampagneCoupure(CsAvisCoupureEdition leAvis)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    

                    DateTime date = DateTime.Now.Date;
                    string suffixe;
                    string[] datearray = date.ToShortDateString().Split('/');
                    string debutdate = datearray[2] + datearray[1] + datearray[0];
                    debutdate = leAvis.Site + leAvis.Centre + debutdate;
                    var exist = context.LCLIENT.Where(l => l.CENTRE == leAvis.Centre  && l.IDCOUPURE.Contains(debutdate));

                    if (exist == null || exist.Count() == 0)
                        suffixe = "01";
                    else
                    {
                        Int64? maxsuffixe = Int64.Parse(exist.Max(l => l.IDCOUPURE).PadRight(2));

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

        public static DataTable  RetouneDevisParNum(string NumDevis)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    //DEVIS devis = context.DEVIS.First(d => d.NUMDEVIS == NumDevis);

                    //if (devis.ETAPEDEVIS.NUMETAPE  != Enumere.EtapeEncaissementDevis)
                    //    return null;

                    //var demandedevis = context.DEMANDEDEVIS.Where(dd => dd.FK_IDDEVIS == devis.PK_ID);
                    ////var deposit = context.DEPOSIT;
                    //var query = from dd in demandedevis
                    //            //join dp in deposit on dd.NUMDEVIS equals dp.NUMDEVIS
                    //            select new
                    //            {
                    //                dd.NOM,
                    //                dd.CLIENT,
                    //                DENR = dd.DEVIS.DATEREGLEMENT,
                    //                CENTRE=dd.DEVIS.CODECENTRE,
                    //                ORDRE =dd.ORDRECLIENT,
                    //                dd.DEVIS.NUMDEVIS,
                    //                dd.DEVIS.ETAPEDEVIS.NUMETAPE,
                    //                devis.FK_IDETAPEDEVIS,
                    //                devis.FK_IDPRODUIT,
                    //                devis.FK_IDTYPEDEVIS,
                    //                devis.FK_IDCENTRE ,
                    //                MONTANT = dd.DEVIS.MONTANTTTC,

                    //             };
                    //return Galatee.Tools.Utility.ListToDataTable<object>(query);
                    return new DataTable();
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
        public static DataTable RetourneCaisseDisponible(string Centre,string matricule)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = null;
                    galadbEntities context1 = new galadbEntities();

                    var LstCaisse = context.CAISSE.Where(p => p.CENTRE == Centre && (p.ESTATTRIBUEE == false || p.ESTATTRIBUEE == null )).ToList();
                    TRANSCAISSE CaisseUtil = context.TRANSCAISSE.FirstOrDefault(p => p.MATRICULE == matricule && p.COPER!=Enumere.CoperFondCaisse && p.COPER!=Enumere.CoperAjsutemenFondCaisse);

                    if (CaisseUtil != null)
                    {
                        int fk_idhabilCaisse = 0;
                        decimal ? FonDcaisse = 0;
                        DateTime DateCaisse = System.DateTime.Today; ;
                        HABILITATIONCAISSE CaisseHabil = context.HABILITATIONCAISSE.FirstOrDefault(p => p.MATRICULE == matricule &&  p.DATE_FIN == null);
                        if (CaisseHabil != null && CaisseUtil != null)
                        {
                            if (Convert.ToDateTime(CaisseHabil.DATE_DEBUT).ToShortDateString() == Convert.ToDateTime(CaisseUtil.DTRANS).ToShortDateString())
                            {
                                fk_idhabilCaisse = CaisseHabil.PK_ID;
                                DateCaisse = Convert.ToDateTime(CaisseHabil.DATE_DEBUT.Value.ToShortDateString());
                            }
                        }
                        var lstCaisse = context.CAISSE.Where (p => p.NUMCAISSE == CaisseUtil.CAISSE && p.CENTRE == Centre && p.PK_ID == CaisseUtil.HABILITATIONCAISSE.FK_IDCAISSE ).ToList();
                        query = (from x in lstCaisse
                                 select new
                                 {
                                     x.  NUMCAISSE ,
                                     x.  ACQUIT ,
                                     x.  BORDEREAU ,
                                     x.  FONDCAISSE ,
                                     x.  COMPTE ,
                                     x.  LIBELLE ,
                                     x.  TYPECAISSE ,
                                     x.  USERCREATION ,
                                     x.  DATECREATION ,
                                     x.  DATEMODIFICATION ,
                                     x.  USERMODIFICATION ,
                                     x.  PK_ID ,
                                     x.  CENTRE ,
                                     x.  FK_IDCENTRE ,
                                     x.  ESTATTRIBUEE,
                                     FK_HABILITATIONCAISSE = fk_idhabilCaisse,
                                     DATECAISSE = DateCaisse,
                                     FK_IDADMUTILISATEUR=  CaisseHabil.FK_IDCAISSIERE  ,
                                 });

                    }
                    else
                    {
                        List<CAISSE> lstCaisse=new List<CAISSE>();
                        HABILITATIONCAISSE CaisseHabil = context.HABILITATIONCAISSE.FirstOrDefault(p => p.MATRICULE == matricule && p.DATE_FIN == null);
                        
                        if (CaisseHabil!= null && CaisseHabil.CAISSE.ESTATTRIBUEE  == true )
                        {
                            CAISSE _LaCaisse = CaisseHabil.CAISSE;
                            lstCaisse.Add(_LaCaisse);
                            query = (from x in lstCaisse
                                     select new
                                     {
                                         x.NUMCAISSE,
                                         x.ACQUIT,
                                         x.BORDEREAU,
                                         x.FONDCAISSE,
                                         x.COMPTE,
                                         x.LIBELLE,
                                         x.TYPECAISSE,
                                         x.USERCREATION,
                                         x.DATECREATION,
                                         x.DATEMODIFICATION,
                                         x.USERMODIFICATION,
                                         x.PK_ID,
                                         x.CENTRE,
                                         x.FK_IDCENTRE,
                                         x.ESTATTRIBUEE,
                                         FK_HABILITATIONCAISSE = CaisseHabil.PK_ID,
                                         DATECAISSE = CaisseHabil.DATE_DEBUT,
                                         DATE_DEBUT = CaisseHabil.DATE_DEBUT,
                                         DATE_FIN = CaisseHabil.DATE_FIN ,
                                         FK_IDADMUTILISATEUR= CaisseHabil.FK_IDCAISSIERE 

                                     });
                        }
                        else
                        {
                            query = (from x in LstCaisse
                                     select new
                                     {
                                         x.NUMCAISSE,
                                         x.ACQUIT,
                                         x.BORDEREAU,
                                         x. FONDCAISSE ,
                                         x.COMPTE,
                                         x.LIBELLE,
                                         x.TYPECAISSE,
                                         x.USERCREATION,
                                         x.DATECREATION,
                                         x.DATEMODIFICATION,
                                         x.USERMODIFICATION,
                                         x.PK_ID,
                                         x.CENTRE,
                                         x.FK_IDCENTRE,
                                         x.ESTATTRIBUEE ,
                                     });
                        }
                      
                    }
                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable RetourneListeTimbre()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    var bank = context.FRAISTIMBRE;

                    IEnumerable<object> query = from x in bank
                                                select new
                                                {
                                                    x.BORNEINF,
                                                    x.BORNESUP,
                                                    x.FRAIS,
                                                    x.PK_ID,
                                                };
                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public static DataTable RetourneClientsParReference(string route, string sens )
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    var client = context.CLIENT;

                    IEnumerable<object> query = from x in client
                                                where x.REFCLIENT == route  
                                                select new 
                                                {
                                                    x.CENTRE,
                                                    x.REFCLIENT,
                                                    x.ORDRE,
                                                    x.DENABON,
                                                    x.NOMABON,
                                                    x.DENMAND,
                                                    x.NOMMAND,
                                                    x.ADRMAND1,
                                                    x.ADRMAND2,
                                                    x.CPOS,
                                                    x.BUREAU,
                                                    x.DINC,
                                                    x.MODEPAIEMENT,
                                                    x.NOMTIT,
                                                    x.BANQUE,
                                                    x.GUICHET,
                                                    x.COMPTE,
                                                    x.RIB,
                                                    x.PROPRIO,
                                                    x.CODECONSO,
                                                    x.CATEGORIE,
                                                    x.CODERELANCE,
                                                    x.NOMCOD,
                                                    x.MOISNAIS,
                                                    x.ANNAIS,
                                                    x.NOMPERE,
                                                    x.NOMMERE,
                                                    x.NATIONNALITE,
                                                    x.CNI,
                                                    x.TELEPHONE,
                                                    x.MATRICULE,
                                                    x.REGLEMENT,
                                                    x.REGEDIT,
                                                    x.FACTURE,
                                                    x.DMAJ,
                                                    x.REFERENCEPUPITRE,
                                                    x.PAYEUR,
                                                    x.SOUSACTIVITE,
                                                    x.AGENTFACTURE,
                                                    x.AGENTRECOUVR,
                                                    x.AGENTASSAINI,
                                                    x.REGROUCONTRAT,
                                                    x.INSPECTION,
                                                    x.DECRET,
                                                    x.CONVENTION,
                                                    x.REFERENCEATM,
                                                    x.PK_ID,
                                                    x.DATECREATION,
                                                    x.DATEMODIFICATION,
                                                    x.USERCREATION,
                                                    x.USERMODIFICATION,
                                                    x.FK_IDMODEPAIEMENT,
                                                    x.FK_IDCODECONSO,
                                                    x.FK_IDCATEGORIE,
                                                    x.FK_IDRELANCE,
                                                    x.FK_IDNATIONALITE,
                                                    x.FK_IDREGROUPEMENT,
                                                    x.FK_IDCENTRE,
                                                    x.CODEIDENTIFICATIONNATIONALE,
                                                    x.EMAIL,
                                                    x.ISFACTUREEMAIL,
                                                    x.ISFACTURESMS,
                                                    x.FK_IDPAYEUR
                                                };
                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable RetourneClientsParAmount(decimal amount, string sens,List<int> lstIdCentre)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    var lclient = context.LCLIENT.Where(x=>lstIdCentre.Contains(x.FK_IDCENTRE));

                    IEnumerable<object> query = from x in lclient
                                                 where x.MONTANT == amount
                                                 select new  {  x.CLIENT1.ADRMAND1,
                                                                x.CLIENT1.ADRMAND2,
                                                                x.CLIENT1.AGENTASSAINI,
                                                                x.CLIENT1.AGENTFACTURE,
                                                                x.CLIENT1.AGENTRECOUVR,
                                                                x.CLIENT1.ANNAIS,
                                                                x.CLIENT1.BANQUE,
                                                                x.CLIENT1.BUREAU,
                                                                x.CLIENT1.CATEGORIE,
                                                                x.CLIENT1.CENTRE,
                                                                x.CLIENT1.CNI,
                                                                x.CLIENT1.CODECONSO,
                                                                x.CLIENT1.CODERELANCE,
                                                                x.CLIENT1.COMPTE,
                                                                x.CLIENT1.CONVENTION,
                                                                x.CLIENT1.CPOS,
                                                                x.CLIENT1.DATECREATION,
                                                                x.CLIENT1.DATEMODIFICATION,
                                                                x.CLIENT1.DECRET,
                                                                x.CLIENT1.DENABON,
                                                                x.CLIENT1.DENMAND,
                                                                x.CLIENT1.DINC,
                                                                x.CLIENT1.DMAJ,
                                                                x.CLIENT1.FACTURE,
                                                                x.CLIENT1.FK_IDCATEGORIE,
                                                                x.CLIENT1.FK_IDCENTRE,
                                                                x.CLIENT1.FK_IDCODECONSO,
                                                                x.CLIENT1.FK_IDMODEPAIEMENT,
                                                                x.CLIENT1.FK_IDNATIONALITE,
                                                                x.CLIENT1.FK_IDREGROUPEMENT,
                                                                x.CLIENT1.FK_IDRELANCE,
                                                                x.CLIENT1.GUICHET,
                                                                x.CLIENT1.INSPECTION,
                                                                x.CLIENT1.MATRICULE,
                                                                x.CLIENT1.MODEPAIEMENT,
                                                                x.CLIENT1.MOISNAIS,
                                                                x.CLIENT1.NATIONNALITE,
                                                                x.CLIENT1.NOMABON,
                                                                x.CLIENT1.NOMCOD,
                                                                x.CLIENT1.NOMMAND,
                                                                x.CLIENT1.NOMMERE,
                                                                x.CLIENT1.NOMPERE,
                                                                x.CLIENT1.NOMTIT,
                                                                x.CLIENT1.ORDRE,
                                                                x.CLIENT1.PAYEUR,
                                                                x.CLIENT1.PK_ID,
                                                                x.CLIENT1.PROPRIO,
                                                                x.CLIENT1.REFCLIENT,
                                                                x.CLIENT1.REFERENCEATM,
                                                                x.CLIENT1.REFERENCEPUPITRE,
                                                                x.CLIENT1.REGROUPEMENT,
                                                                x.CLIENT1.REGEDIT,
                                                                x.CLIENT1.REGLEMENT,
                                                                x.CLIENT1.REGROUCONTRAT,
                                                                x.CLIENT1.RIB,
                                                                x.CLIENT1.SOUSACTIVITE,
                                                                x.CLIENT1.TELEPHONE,
                                                                x.CLIENT1.USERCREATION,
                                                                x.CLIENT1.USERMODIFICATION,
                                                                MONTANT=x.MONTANT };
                    if (sens=="1")
                    {
                        query = from x in lclient
                                where x.MONTANT >= amount
                               select new  {  x.CLIENT1.ADRMAND1,
                                                                x.CLIENT1.ADRMAND2,
                                                                x.CLIENT1.AGENTASSAINI,
                                                                x.CLIENT1.AGENTFACTURE,
                                                                x.CLIENT1.AGENTRECOUVR,
                                                                x.CLIENT1.ANNAIS,
                                                                x.CLIENT1.BANQUE,
                                                                x.CLIENT1.BUREAU,
                                                                x.CLIENT1.CATEGORIE,
                                                                x.CLIENT1.CENTRE,
                                                                x.CLIENT1.CNI,
                                                                x.CLIENT1.CODECONSO,
                                                                x.CLIENT1.CODERELANCE,
                                                                x.CLIENT1.COMPTE,
                                                                x.CLIENT1.CONVENTION,
                                                                x.CLIENT1.CPOS,
                                                                x.CLIENT1.DATECREATION,
                                                                x.CLIENT1.DATEMODIFICATION,
                                                                x.CLIENT1.DECRET,
                                                                x.CLIENT1.DENABON,
                                                                x.CLIENT1.DENMAND,
                                                                x.CLIENT1.DINC,
                                                                x.CLIENT1.DMAJ,
                                                                x.CLIENT1.FACTURE,
                                                                x.CLIENT1.FK_IDCATEGORIE,
                                                                x.CLIENT1.FK_IDCENTRE,
                                                                x.CLIENT1.FK_IDCODECONSO,
                                                                x.CLIENT1.FK_IDMODEPAIEMENT,
                                                                x.CLIENT1.FK_IDNATIONALITE,
                                                                x.CLIENT1.FK_IDREGROUPEMENT,
                                                                x.CLIENT1.FK_IDRELANCE,
                                                                x.CLIENT1.GUICHET,
                                                                x.CLIENT1.INSPECTION,
                                                                x.CLIENT1.MATRICULE,
                                                                x.CLIENT1.MODEPAIEMENT,
                                                                x.CLIENT1.MOISNAIS,
                                                                x.CLIENT1.NATIONNALITE,
                                                                x.CLIENT1.NOMABON,
                                                                x.CLIENT1.NOMCOD,
                                                                x.CLIENT1.NOMMAND,
                                                                x.CLIENT1.NOMMERE,
                                                                x.CLIENT1.NOMPERE,
                                                                x.CLIENT1.NOMTIT,
                                                                x.CLIENT1.ORDRE,
                                                                x.CLIENT1.PAYEUR,
                                                                x.CLIENT1.PK_ID,
                                                                x.CLIENT1.PROPRIO,
                                                                x.CLIENT1.REFCLIENT,
                                                                x.CLIENT1.REFERENCEATM,
                                                                x.CLIENT1.REFERENCEPUPITRE,
                                                                x.CLIENT1.REGROUPEMENT,
                                                                x.CLIENT1.REGEDIT,
                                                                x.CLIENT1.REGLEMENT,
                                                                x.CLIENT1.REGROUCONTRAT,
                                                                x.CLIENT1.RIB,
                                                                x.CLIENT1.SOUSACTIVITE,
                                                                x.CLIENT1.TELEPHONE,
                                                                x.CLIENT1.USERCREATION,
                                                                x.CLIENT1.USERMODIFICATION,
                                                                MONTANT=x.MONTANT };
                    }
                    if (sens == "-1")
                    {
                        query = from x in lclient
                                
                                where x.MONTANT <= amount
                                select new  {  x.CLIENT1.ADRMAND1,
                                                                x.CLIENT1.ADRMAND2,
                                                                x.CLIENT1.AGENTASSAINI,
                                                                x.CLIENT1.AGENTFACTURE,
                                                                x.CLIENT1.AGENTRECOUVR,
                                                                x.CLIENT1.ANNAIS,
                                                                x.CLIENT1.BANQUE,
                                                                x.CLIENT1.BUREAU,
                                                                x.CLIENT1.CATEGORIE,
                                                                x.CLIENT1.CENTRE,
                                                                x.CLIENT1.CNI,
                                                                x.CLIENT1.CODECONSO,
                                                                x.CLIENT1.CODERELANCE,
                                                                x.CLIENT1.COMPTE,
                                                                x.CLIENT1.CONVENTION,
                                                                x.CLIENT1.CPOS,
                                                                x.CLIENT1.DATECREATION,
                                                                x.CLIENT1.DATEMODIFICATION,
                                                                x.CLIENT1.DECRET,
                                                                x.CLIENT1.DENABON,
                                                                x.CLIENT1.DENMAND,
                                                                x.CLIENT1.DINC,
                                                                x.CLIENT1.DMAJ,
                                                                x.CLIENT1.FACTURE,
                                                                x.CLIENT1.FK_IDCATEGORIE,
                                                                x.CLIENT1.FK_IDCENTRE,
                                                                x.CLIENT1.FK_IDCODECONSO,
                                                                x.CLIENT1.FK_IDMODEPAIEMENT,
                                                                x.CLIENT1.FK_IDNATIONALITE,
                                                                x.CLIENT1.FK_IDREGROUPEMENT,
                                                                x.CLIENT1.FK_IDRELANCE,
                                                                x.CLIENT1.GUICHET,
                                                                x.CLIENT1.INSPECTION,
                                                                x.CLIENT1.MATRICULE,
                                                                x.CLIENT1.MODEPAIEMENT,
                                                                x.CLIENT1.MOISNAIS,
                                                                x.CLIENT1.NATIONNALITE,
                                                                x.CLIENT1.NOMABON,
                                                                x.CLIENT1.NOMCOD,
                                                                x.CLIENT1.NOMMAND,
                                                                x.CLIENT1.NOMMERE,
                                                                x.CLIENT1.NOMPERE,
                                                                x.CLIENT1.NOMTIT,
                                                                x.CLIENT1.ORDRE,
                                                                x.CLIENT1.PAYEUR,
                                                                x.CLIENT1.PK_ID,
                                                                x.CLIENT1.PROPRIO,
                                                                x.CLIENT1.REFCLIENT,
                                                                x.CLIENT1.REFERENCEATM,
                                                                x.CLIENT1.REFERENCEPUPITRE,
                                                                x.CLIENT1.REGROUPEMENT,
                                                                x.CLIENT1.REGEDIT,
                                                                x.CLIENT1.REGLEMENT,
                                                                x.CLIENT1.REGROUCONTRAT,
                                                                x.CLIENT1.RIB,
                                                                x.CLIENT1.SOUSACTIVITE,
                                                                x.CLIENT1.TELEPHONE,
                                                                x.CLIENT1.USERCREATION,
                                                                x.CLIENT1.USERMODIFICATION,
                                                                MONTANT=x.MONTANT };
                    }

                    
                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable RetourneClientsParNoms(string names)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    var client = context.CLIENT;

                    IEnumerable<object> query = from x in client
                                                where x.NOMABON.Contains(names)
                                                select new {
                                                    x.CENTRE ,
                                                    x.REFCLIENT ,
                                                    x.ORDRE ,
                                                    x.NOMABON 
                                                };
                    return Galatee.Tools.Utility.ListToDataTable(query.Take(100));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public static DataTable ChargerCaisseDisponible()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    var _CAISSE = context.CAISSE ;

                    IEnumerable<object> query = from x in _CAISSE
                                                where (x.ESTATTRIBUEE == false || x.ESTATTRIBUEE == null ) 
                                                select new
                                                {
                                                    x.NUMCAISSE,
                                                    x.ACQUIT,
                                                    x.BORDEREAU,
                                                    x.FONDCAISSE,
                                                    x.COMPTE,
                                                    x.LIBELLE,
                                                    x.TYPECAISSE,
                                                    x.USERCREATION,
                                                    x.DATECREATION,
                                                    x.DATEMODIFICATION,
                                                    x.USERMODIFICATION,
                                                    x.PK_ID,
                                                    x.CENTRE,
                                                    x.FK_IDCENTRE,
                                                    x.ESTATTRIBUEE
                                                };
                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public static DataTable ListeDesTypeTimbre()
        {
            try
            {
                using (galadbEntities ctontext = new galadbEntities())
                {
                    var typeTimbre = ctontext.TYPETIMBRE;
                    IEnumerable<object> query = (from t in typeTimbre
                                                 select new
                                                 {
                                                     t.CODE,
                                                     t.LIBELLE,
                                                     t.MONTANT,
                                                     t.PK_ID
                                                 });
                    return Galatee.Tools.Utility.ListToDataTable<object>(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable ListeDesMotifRejetCheque()
        {
            try
            {
                using (galadbEntities ctontext = new galadbEntities())
                {
                    var typeMotif = ctontext.MOTIFCHEQUEIMPAYE ;
                    IEnumerable<object> query = (from t in typeMotif
                                                 select new
                                                 {
                                                     t.CODE,
                                                     t.LIBELLE,
                                                     t.POIDS ,
                                                     t.PK_ID
                                                 });
                    return Galatee.Tools.Utility.ListToDataTable<object>(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static bool IsLotIsole(string leLot)
        {
            string FactureIsoleIndex = "00002";
            string FactureResiliationIndex = "00001";
            string FactureAnnulatinIndex = "00003";
            if (new string[] { FactureIsoleIndex, FactureResiliationIndex, FactureAnnulatinIndex }.Contains(leLot.Substring(Enumere.TailleCentre, (Enumere.TailleNumeroBatch - Enumere.TailleCentre))))
                return true;
            else
                return false;
        }
    }
}
