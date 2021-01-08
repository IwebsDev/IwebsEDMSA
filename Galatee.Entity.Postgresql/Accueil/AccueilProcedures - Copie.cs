using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Collections ;
using Galatee.Structure;
using System.IO;
using System.Data.Entity.Validation;
//using Galatee.DataAccess;




namespace Galatee.Entity.Model
{
    public static class AccueilProcedures
    {

        public static DataTable RetourneEtapeDemande()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    var _Etape = context.ETAPEDEMANDE;
                    IEnumerable<object> query =
                    from _etape in _Etape
                    select new
                    {
                        _etape.TYPEDEMANDE ,
                        _etape.CONTROLEETAPE ,
                        _etape.LIBELLEETAPE ,
                        _etape.ORDRE ,
                        _etape.USERCREATION,
                        _etape.USERMODIFICATION,
                        _etape.DATECREATION,
                        _etape.DATEMODIFICATION
                    };
                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static DataTable RetourneDonneesSite()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    var _CENTRE = context.CENTRE;
                    IEnumerable<object> query =
                    from c in _CENTRE
                    select new
                    {
                        c.CODE,
                        c.LIBELLE,
                        c.TYPECENTRE,
                        c.CODESITE,
                        c.ADRESSE,
                        c.TELRENSEIGNEMENT,
                        c.TELDEPANNAGE,
                        c.NUMEROAUTOCLIENT,
                        c.GESTIONAUTOAVANCECONSO,
                        c.GESTIONAUTOFRAIS,
                        c.NUMEROFACTUREPARCLIENT,
                        c.DATECREATION,
                        c.DATEMODIFICATION,
                        c.USERCREATION,
                        c.USERMODIFICATION,
                        c.PK_ID,
                        c.FK_IDCODESITE,
                        c.FK_IDTYPECENTRE,
                        c.FK_IDNIVEAUTARIF,
                        c.COMPTEECLAIRAGEPUBLIC ,
                        c.NUMERODEMANDE,
                       LIBELLESITE = c.SITE.LIBELLE,
                        c.CODESOUSCOMPTE
                    };
                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static DataTable  NumeroDemande()
        {
            try
            {
                DataTable dt = new DataTable();
                //List<SPESITE> SpecSite = new List<SPESITE>();
                //using (galadbEntities ctontext = new galadbEntities())
                //{
                //    IEnumerable<object> queri = ctontext.SPESITE.ToList();
                //    dt = Galatee.Tools.Utility.ListToDataTable(queri);
                //}
                //using (galadbEntities ctontext1 = new galadbEntities())
                //{
                //    SpecSite = ctontext1.SPESITE.ToList();
                //    foreach (SPESITE item in SpecSite)
                //        item.NUMDEM++;
                //}
                //Entities.UpdateEntity<SPESITE>(SpecSite);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static DataTable RetourneProduitSite()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    var _CENTRE = context.CENTRE;
                    IEnumerable<object> query =
                    from c in _CENTRE
                    from p in c.PRODUITCENTRE
                    select new
                    {
                        p.FK_IDCENTRE,
                        p.PRODUIT.CODE,
                        p.PRODUIT.LIBELLE,
                        PK_ID = p.FK_IDPRODUIT
                    };
                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static string NumeroFacture(int? pIdCentre)
        {
            try
            {
                string NumRecu = string.Empty;
                CENTRE leCentre = new CENTRE();
                using (galadbEntities ctontext = new galadbEntities())
                {
                    leCentre = ctontext.CENTRE.FirstOrDefault(t => t.PK_ID == pIdCentre);

                    if (leCentre != null && !string.IsNullOrEmpty(leCentre.CODESITE))
                    {
                        NumRecu = leCentre.SITE.NUMEROFACTURE.ToString();
                        int DernN = int.Parse(NumRecu) + 1;
                        leCentre.SITE.NUMEROFACTURE = DernN.ToString();
                    }
                }
                if (leCentre.SITE != null && !string.IsNullOrEmpty(leCentre.SITE.CODE))
                    Entities.UpdateEntity<SITE>(leCentre.SITE);
                return NumRecu;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static string NumeroFacture()
        {
            try
            {
                string NumRecu = string.Empty;
                PARAMETRESGENERAUX leParam = new PARAMETRESGENERAUX();
                using (galadbEntities ctontext = new galadbEntities())
                {
                    leParam = ctontext.PARAMETRESGENERAUX.FirstOrDefault(t => t.CODE == "000410");

                    if (leParam != null && !string.IsNullOrEmpty(leParam.LIBELLE))
                    {
                        NumRecu = leParam.LIBELLE;
                        int DernN = int.Parse(NumRecu) + 1;
                        if (DernN == 1000000) leParam.LIBELLE = "0000001";
                        NumRecu = DernN.ToString("000000");
                    }
                    ctontext.SaveChanges();
                }
                return NumRecu;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static void IncrementNumDem(int pIdSite)
        {
            try
            {
                string NumRecu = string.Empty;
                SITE leSite = new SITE();
                using (galadbEntities ctontext = new galadbEntities())
                {
                    leSite = ctontext.SITE.FirstOrDefault(t => t.PK_ID == pIdSite);

                    if (leSite != null && !string.IsNullOrEmpty(leSite.CODE))
                    {
                        leSite.NUMERODEMANDE = leSite.NUMERODEMANDE + 1;
                        ctontext.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static List<DEMANDE> SuivieDemandeDeModif(string matricule, galadbEntities pcontext)
        {
            try
            {

                return pcontext.DEMANDE.Where(t => t.MATRICULE == matricule && (new string[] { "26", "27", "37" }.Contains(t.TYPEDEMANDE))).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static DataTable RetourneOptionDemande()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    var _Tdem = context.TYPEDEMANDE;
                    IEnumerable<object> query =
                    from _tdem in _Tdem
                    select new
                    {
                        _tdem.CODE,
                        _tdem.DEMOPTION1,
                        _tdem.DEMOPTION2,
                        _tdem.DEMOPTION3,
                        _tdem.DEMOPTION4,
                        _tdem.DEMOPTION5,
                        _tdem.DEMOPTION6,
                        _tdem.DEMOPTION7,
                        _tdem.DEMOPTION8,
                        _tdem.DEMOPTION9,
                        _tdem.DEMOPTION10,
                        _tdem.DEMOPTION11,
                        _tdem.DEMOPTION12,
                        _tdem.DEMOPTION13,
                        _tdem.DEMOPTION14,
                        _tdem.DEMOPTION15,
                        _tdem.DEMOPTION16,
                        _tdem.DEMOPTION17,
                        _tdem.DEMOPTION18,
                        _tdem.DEMOPTION19,
                        _tdem.DEMOPTION20,
                        _tdem.LIBELLE,
                        _tdem.EVT1,
                        _tdem.EVT2,
                        _tdem.EVT3,
                        _tdem.EVT4,
                        _tdem.EVT5,
                        _tdem.USERCREATION,
                        _tdem.DATECREATION,
                        _tdem.DATEMODIFICATION,
                        _tdem.USERMODIFICATION,
                        _tdem.PK_ID
                    };
                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static DataTable RetourneParametre()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = null;
                    var _LstParametrageProduit = context.PARAMETREBRANCHEMENT;
                    query =
                    from _LeParametrageProduit in _LstParametrageProduit
                    select new
                    {
                        _LeParametrageProduit.CENTRE,
                        _LeParametrageProduit.PRODUIT,
                        _LeParametrageProduit.NBPOINTSMAXI,
                        _LeParametrageProduit.GESTIONTRANSFO,
                        _LeParametrageProduit.MODESAISIEINDEX,
                        _LeParametrageProduit.USERCREATION,
                        _LeParametrageProduit.DATECREATION,
                        _LeParametrageProduit.DATEMODIFICATION,
                        _LeParametrageProduit.USERMODIFICATION,
                        _LeParametrageProduit.PK_ID,
                        _LeParametrageProduit.FK_IDPRODUIT,
                        _LeParametrageProduit.FK_IDCENTRE
                    };

                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static DataTable RetourneTarif()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = null;
                    var _LstTarif = context.TYPETARIF;
                    query =
                    from _LeTarif in _LstTarif
                    select new
                    {
                        _LeTarif.CODE,
                        _LeTarif.PRODUIT,
                        _LeTarif.LIBELLE,
                        _LeTarif.USERCREATION,
                        _LeTarif.DATECREATION,
                        _LeTarif.DATEMODIFICATION,
                        _LeTarif.USERMODIFICATION,
                        _LeTarif.PK_ID,
                        _LeTarif.FK_IDPRODUIT
                    };
                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //public static DataTable RetourneTarifPuissance()
        //{
        //    try
        //    {
        //        using (galadbEntities context = new galadbEntities())
        //        {
        //            IEnumerable<object> query = null;
        //            var _LstTarifPuissance = context.PUISSANCETYPETARIF ;
        //            query =
        //            from _LeTarifPuissance in _LstTarifPuissance
        //            select new
        //            {
        //                _LeTarifPuissance.PUISSANCE.PRODUIT ,
        //                 FK_IDPUISSANCE=  _LeTarifPuissance.FK_IDPUISSANCE ,
        //                _LeTarifPuissance.TYPETARIF.CODE ,
        //                _LeTarifPuissance.TYPETARIF.LIBELLE  ,
        //                CODEPUISSANCE = _LeTarifPuissance.PUISSANCE.CODE,
        //                PK_ID=  _LeTarifPuissance.FK_IDTYPETARIF  ,


        //            };
        //            return Galatee.Tools.Utility.ListToDataTable(query);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        public static DataTable RetourneCodeModePayement()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = null;
                    var _MODEPAIEMENT = context.MODEPAIEMENT;
                    query =
                    from _LeModePaiement in _MODEPAIEMENT
                    select new
                    {
                        _LeModePaiement.CODE,
                        _LeModePaiement.PK_ID,
                        _LeModePaiement.LIBELLE,
                        _LeModePaiement.USERCREATION,
                        _LeModePaiement.USERMODIFICATION,
                        _LeModePaiement.DATECREATION,
                        _LeModePaiement.DATEMODIFICATION
                    };
                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static DataTable RetournePosteSource()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = null;
                    var _LstPosteSource = context.POSTESOURCE;
                    query =
                    from x in _LstPosteSource
                    select new
                    {
                        x.CODE,
                        x.LIBELLE,
                        x.DATECREATION,
                        x.DATEMODIFICATION,
                        x.USERCREATION,
                        x.USERMODIFICATION,
                        x.PK_ID,
                        COMMUNE = x.COMMUNE.CODE,
                        FK_IDCOMMUNE = x.FK_IDCOMMUNE
                    };
                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static DataTable RetourneDepartHTA()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = null;
                    var _LstCodeDepartHTA = context.DEPARTHTA;
                    query =
                    from x in _LstCodeDepartHTA
                    select new
                    {
                        x.CODE,
                        x.LIBELLE,
                        x.DATECREATION,
                        x.DATEMODIFICATION,
                        x.USERCREATION,
                        x.USERMODIFICATION,
                        x.PK_ID,
                        x.FK_IDPOSTESOURCE,
                        POSTESOURCE = x.POSTESOURCE.CODE,
                        LIBELLEPOSTESOURCE = x.POSTESOURCE.LIBELLE
                    };
                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable RetournePosteTransformateur()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = null;
                    var _LstPosteSource = context.POSTETRANSFORMATION ;
                    query =
                    from x in _LstPosteSource
                    select new
                    {
                        x.CODE,
                        x.LIBELLE,
                        x.DATECREATION,
                        x.DATEMODIFICATION,
                        x.USERCREATION,
                        x.USERMODIFICATION,
                        x.PK_ID,
                        COMMUNE = x.COMMUNE.CODE,
                        FK_IDCOMMUNE = x.FK_IDCOMMUNE,
                        x.FK_IDDEPARTHTA ,
                        CODEDEPARTHTA = x.DEPARTHTA.CODE,
                        LIBELLEDEPARTHTA = x.DEPARTHTA.LIBELLE  
                    };
                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable ChargerRubrique()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = null;
                    var _LstRubrique = context.RUBRIQUEDEVIS  ;
                    query =
                    from x in _LstRubrique
                    select new
                    {
                        x.CODE,
                        x.LIBELLE,
                        CODEPRODUIT=  x.PRODUIT.CODE ,
                        x.FK_IDPRODUIT ,
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

        
        public static DataTable RetourneDepartBT()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = null;
                    var _LstCodeDepart = context.DEPARTBT;
                    query =
                    from x in _LstCodeDepart
                    select new
                    {
                        x.CODE,
                        x.LIBELLE,
                        x.DATECREATION,
                        x.DATEMODIFICATION,
                        x.USERCREATION,
                        x.USERMODIFICATION,
                        x.PK_ID,
                        x.FK_IDPOSTETRANSFORMATION ,
                        CODETRANSFORMATEUR = x.POSTETRANSFORMATION.CODE,
                        LIBELLETRANSFORMATEUR = x.POSTETRANSFORMATION.LIBELLE,
                    };
                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

 
        public static DataTable RetourneForfait()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = null;
                    var _LstForfait = context.FORFAIT;
                    query =
                    from _LeForfait in _LstForfait
                    select new
                    {
                        _LeForfait.CENTRE,
                        _LeForfait.PRODUIT,
                        _LeForfait.CODE,
                        _LeForfait.LIBELLE,
                        _LeForfait.TRANS,
                        _LeForfait.USERCREATION,
                        _LeForfait.DATECREATION,
                        _LeForfait.DATEMODIFICATION,
                        _LeForfait.USERMODIFICATION,
                        _LeForfait.PK_ID,
                        _LeForfait.FK_IDCENTRE,
                        _LeForfait.FK_IDPRODUIT 
                    };
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
                    var _LstPuissance = context.PUISSANCEINSTALLEE;
                        query =
                        from _LaPuissance in _LstPuissance
                        select new
                        {
                            _LaPuissance.PK_ID,
                            _LaPuissance.PRODUIT,
                            _LaPuissance.CODE,
                            _LaPuissance.VALEUR,
                            _LaPuissance.KPERTEACTIVE1,
                            _LaPuissance.KPERTEACTIVE2,
                            _LaPuissance.KPERTEREACTIVE1,
                            _LaPuissance.KPERTEREACTIVE2,
                            _LaPuissance.DATECREATION,
                            _LaPuissance.USERCREATION,
                            _LaPuissance.DATEMODIFICATION,
                            _LaPuissance.USERMODIFICATION,
                            _LaPuissance.FK_IDPRODUIT
                        };
                        return Galatee.Tools.Utility.ListToDataTable(query);
                    }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static DataTable RetournePuissanceSouscrite()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = null;
                    var _LstPuissance = context.PUISSANCESOUSCRITE ;
                    query =
                    from _LaPuissance in _LstPuissance
                    select new
                    {
                        _LaPuissance.PK_ID,
                        _LaPuissance.PRODUIT,
                        _LaPuissance.CODE,
                        _LaPuissance.VALEUR,
                        _LaPuissance.DATECREATION,
                        _LaPuissance.USERCREATION,
                        _LaPuissance.DATEMODIFICATION,
                        _LaPuissance.USERMODIFICATION,
                        _LaPuissance.FK_IDPRODUIT
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
                    var _LstPuissance = context.PUISSANCEPARREGLAGECOMPTEUR ;
                    query =
                    (from _LaPuissance in _LstPuissance
                    select new
                    {
                         FK_IDPUISSANCE = _LaPuissance.FK_IDPUISSANCE  ,
                        _LaPuissance.PUISSANCESOUSCRITE.PRODUIT,
                        _LaPuissance.PUISSANCESOUSCRITE.CODE,
                        _LaPuissance.PUISSANCESOUSCRITE.FK_IDPRODUIT ,
                         FK_IDREGLAGECOMPTEUR = _LaPuissance.REGLAGECOMPTEUR.PK_ID ,
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
        public static DataTable RetourneMateriel(string pProduit)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    var _MATERIELBRANCHEMENT = context.MATERIELBRANCHEMENT;
                    IEnumerable<object> query =
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
        public static DataTable RetourneListeSecteur()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = null;
                    var _LstSecteur = context.SECTEUR ;
                    query =
                    from _LeSecteur in _LstSecteur
                    select new
                    {
                        _LeSecteur.CODEQUARTIER,
                        _LeSecteur.CODE,
                        _LeSecteur.LIBELLE,
                        _LeSecteur.TRANS,
                        _LeSecteur.PK_ID,
                        _LeSecteur.FK_IDQUARTIER,
                        _LeSecteur.DATECREATION,
                        _LeSecteur.DATEMODIFICATION,
                        _LeSecteur.USERCREATION,
                        _LeSecteur.USERMODIFICATION
                    };
                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static DataTable RetourneTypeCompteur()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = null;
                    var _TCOMPT = context.TYPECOMPTEUR  ;
                        query =
                        from _LeTcompt in _TCOMPT
                        select new
                        {
                            _LeTcompt.PRODUIT,
                            _LeTcompt.CODE  ,
                            _LeTcompt.LIBELLE,
                            _LeTcompt.DATECREATION,
                            _LeTcompt.DATEMODIFICATION,
                            _LeTcompt.USERCREATION,
                            _LeTcompt.USERMODIFICATION,
                            _LeTcompt.PK_ID,
                            _LeTcompt.FK_IDPRODUIT
                        };
                        return Galatee.Tools.Utility.ListToDataTable(query);
                    }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static DataTable RetourneTypeCompteurMt(string pProduit, string pTypecomptage, string pCentre)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = null;
                    var _TCOMPT = context.TYPECOMPTEUR  ;
                    //var _COMPTAGE = context.COMPTAGE ;
                  
                        query =
                      from _LeTcompt in _TCOMPT
                      //join _LeComptage in _COMPTAGE
                      //on _LeTcompt.CODE 
                      //equals _LeComptage.TYPECOMPTEUR  
                      //where _LeTcompt.PRODUIT == pProduit &&
                      //     (_LeTcompt.CENTRE == pCentre || _LeComptage.CENTRE == Enumere.Generale) && 
                      //      _LeComptage.TYPECOMPTAGE == pTypecomptage 
                      select new
                      {
                          //_LeComptage.TYPECOMPTAGE,
                          _LeTcompt.LIBELLE,
                          _LeTcompt.CODE ,
                          //_LeTcompt.POINT 
                          //POINT =null ,
                          //SAISIE = "Non3
                      };
                    
                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static DataTable ChargerTypeComptage()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = null;
                    var _TCOMPTAGE = context.TYPECOMPTAGE;
                    query =
                  from x in _TCOMPTAGE
                  select new
                  {
                      x.CODE,
                      x.LIBELLE,
                      x.TRANSFO_UNIQUE,
                      x.PUISSANCEINSTALLEE_MINI,
                      x.PUISSANCEINSTALLEE_MAXI,
                      x.AVEC_PERTE,
                      x.AVEC_PRIMEFIXE,
                      x.USERCREATION,
                      x.DATECREATION,
                      x.DATEMODIFICATION,
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


        public static string RetourneOrdreMax(int fk_idcentre, string pCentre, string pClient, string pProduit)
        {
            try
            {
                string ordreMax = string.Empty;
                using (galadbEntities context = new galadbEntities())
                {
                    var _abon = context.ABON;
                    var query = (from x in _abon
                                 where x.CENTRE == pCentre && x.CLIENT  == pClient && x.PRODUIT  == pProduit && x.FK_IDCENTRE == fk_idcentre
                                 select new
                                 {
                                     x.ORDRE
                                 });
                    if (query != null)
                        ordreMax = query.Max(cl => cl.ORDRE);
                }
                return ordreMax;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }




        public static DataTable RetourneDemande(string pCentre, string pNumDemande)
        {
            try
            {

                string Centre =string.IsNullOrEmpty(pCentre )? "":pCentre ;
                string NumDemande = string.IsNullOrEmpty(pNumDemande) ? "" : pCentre;
                string Vide = "";

                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = null;
                    var _DEMANDE = context.DEMANDE;
                    query =
                  from _LaDemande in _DEMANDE
                  where
                  ((_LaDemande.CENTRE  == pCentre || Centre.Equals(Vide) ) && (_LaDemande.NUMDEM  == pNumDemande || NumDemande.Equals(Vide)))
                  select new
                  {
                      _LaDemande.NUMDEM,
                      _LaDemande.CENTRE,
                      _LaDemande.NUMPERE,
                      _LaDemande.TYPEDEMANDE,
                      _LaDemande.DPRRDV,
                      _LaDemande.DPRDEV,
                      _LaDemande.DPREX,
                      _LaDemande.DREARDV,
                      _LaDemande.DREADEV,
                      _LaDemande.DREAEX,
                      _LaDemande.HRDVPR,
                      _LaDemande.FDEM,
                      _LaDemande.FREP,
                      _LaDemande.NOMPERE,
                      _LaDemande.NOMMERE,
                      _LaDemande.MATRICULE,
                      _LaDemande.STATUT,
                      _LaDemande.DCAISSE,
                      _LaDemande.NCAISSE,
                      _LaDemande.EXDAG,
                      _LaDemande.EXDBRT,
                      _LaDemande.PRODUIT,
                      _LaDemande.EXCL,
                      _LaDemande.CLIENT,
                      _LaDemande.EXCOMPT,
                      _LaDemande.COMPTEUR,
                      _LaDemande.EXEVT,
                      _LaDemande.CTAXEG,
                      _LaDemande.DATED,
                      _LaDemande.REFEM,
                      _LaDemande.ORDRE,
                      _LaDemande.TOPEDIT,
                      _LaDemande.FACTURE,
                      _LaDemande.DATEFLAG,
                      _LaDemande.USERCREATION,
                      _LaDemande.DATECREATION,
                      _LaDemande.DATEMODIFICATION,
                      _LaDemande.USERMODIFICATION,
                      _LaDemande.ETAPEDEMANDE,
                      _LaDemande.PK_ID,
                      _LaDemande.FK_IDCENTRE,
                      _LaDemande.FK_IDADMUTILISATEUR,
                      _LaDemande.FK_IDTYPEDEMANDE,
                      _LaDemande.FK_IDPRODUIT
                  };
                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static bool MAJDemande(CsDemandeBase laDemande)
        {
            try
            {
                int res = -1;
                    DEMANDE _Demande = Entities.ConvertObject<DEMANDE, CsDemandeBase>(laDemande);
                    using (galadbEntities ct = new galadbEntities())
                    {
                        string  iddemande = laDemande.PK_ID.ToString();
                        DEMANDE_WORFKLOW dd = ct.DEMANDE_WORFKLOW.FirstOrDefault(t => t.FK_IDLIGNETABLETRAVAIL == iddemande);
                        if (dd != null)
                            dd.FK_IDSTATUS = 4;
                         Entities.UpdateEntity<DEMANDE>(_Demande,ct );
                        res= ct.SaveChanges();
                    }
                    return res == -1 ? false : true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static DataTable RetourneReglementTranscaisB(string pCentre, string pClient, string pOrdre)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = null;
                    var _TRANSCAISB = context.TRANSCAISB;
                    query =
                    from _LeTRANSCAISB in _TRANSCAISB
                    where _LeTRANSCAISB.CENTRE  == pCentre && _LeTRANSCAISB.CLIENT  == pClient &&
                          _LeTRANSCAISB.ORDRE  == pOrdre &&
                          _LeTRANSCAISB.TOPANNUL == null
                    select new
                    {
                        _LeTRANSCAISB.CENTRE,
                        _LeTRANSCAISB.CLIENT,
                        _LeTRANSCAISB.ORDRE,
                        _LeTRANSCAISB.CAISSE,
                        _LeTRANSCAISB.ACQUIT,
                        _LeTRANSCAISB.MATRICULE,
                        NOMCAISSIERE = _LeTRANSCAISB.ADMUTILISATEUR.LIBELLE,
                        _LeTRANSCAISB.NDOC,
                        _LeTRANSCAISB.REFEM,
                        MONTANTPAYE = _LeTRANSCAISB.MONTANT,
                        _LeTRANSCAISB.DC,
                        _LeTRANSCAISB.COPER,
                        _LeTRANSCAISB.PERCU,
                        _LeTRANSCAISB.RENDU,
                        LIBELLEMODREG = _LeTRANSCAISB.MODEREG1.LIBELLE,
                        _LeTRANSCAISB.MODEREG,
                        _LeTRANSCAISB.PLACE,
                        _LeTRANSCAISB.DTRANS,
                        _LeTRANSCAISB.DEXIG,
                        _LeTRANSCAISB.BANQUE,
                        _LeTRANSCAISB.GUICHET,
                        _LeTRANSCAISB.ORIGINE,
                        _LeTRANSCAISB.ECART,
                        _LeTRANSCAISB.TOPANNUL,
                        _LeTRANSCAISB.MOISCOMPT,
                        _LeTRANSCAISB.TOP1,
                        _LeTRANSCAISB.TOURNEE,
                        _LeTRANSCAISB.NUMDEM,
                        _LeTRANSCAISB.DATEVALEUR,
                        _LeTRANSCAISB.DATEFLAG,
                        _LeTRANSCAISB.NUMCHEQ,
                        //NOMOPERATEUR = _LeTRANSCAISB.ADMUTILISATEUR1.LIBELLE ,
                        _LeTRANSCAISB.SAISIPAR,
                        _LeTRANSCAISB.DATEENCAISSEMENT,
                        _LeTRANSCAISB.CANCELLATION,
                        _LeTRANSCAISB.USERCREATION,
                        _LeTRANSCAISB.DATECREATION,
                        _LeTRANSCAISB.DATEMODIFICATION,
                        _LeTRANSCAISB.USERMODIFICATION,
                        _LeTRANSCAISB.PK_ID,
                        _LeTRANSCAISB.FK_IDCENTRE,
                        _LeTRANSCAISB.FK_IDHABILITATIONCAISSE ,
                        _LeTRANSCAISB.FK_IDMODEREG,
                        _LeTRANSCAISB.FK_IDLIBELLETOP,
                        _LeTRANSCAISB.FK_IDCAISSIERE 
                        
                    };
                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static DataTable RetourneReglementTranscaisse(string pCentre, string pClient, string pOrdre)
        {
           try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = null;
                    var _TRANSCAISSE = context.TRANSCAISSE;
                    query =
                    from _LeTRANSCAISSE in _TRANSCAISSE
                    where _LeTRANSCAISSE.CENTRE == pCentre && _LeTRANSCAISSE.CLIENT == pClient &&
                          _LeTRANSCAISSE.ORDRE == pOrdre &&
                          _LeTRANSCAISSE.TOPANNUL == null
                    select new
                    {
                        _LeTRANSCAISSE.CENTRE,
                        _LeTRANSCAISSE.CLIENT,
                        _LeTRANSCAISSE.ORDRE,
                        _LeTRANSCAISSE.CAISSE,
                        _LeTRANSCAISSE.ACQUIT,
                        NOMCAISSIERE = _LeTRANSCAISSE.ADMUTILISATEUR.LIBELLE,
                        _LeTRANSCAISSE.MATRICULE,
                        _LeTRANSCAISSE.NDOC,
                        _LeTRANSCAISSE.REFEM,
                        MONTANTPAYE = _LeTRANSCAISSE.MONTANT,
                        _LeTRANSCAISSE.DC,
                        _LeTRANSCAISSE.COPER,
                        _LeTRANSCAISSE.PERCU,
                        _LeTRANSCAISSE.RENDU,
                        LIBELLEMODREG = _LeTRANSCAISSE.MODEREG1.LIBELLE,
                        _LeTRANSCAISSE.MODEREG,
                        _LeTRANSCAISSE.PLACE,
                        _LeTRANSCAISSE.DTRANS,
                        _LeTRANSCAISSE.DEXIG,
                        _LeTRANSCAISSE.BANQUE,
                        _LeTRANSCAISSE.GUICHET,
                        _LeTRANSCAISSE.ORIGINE,
                        _LeTRANSCAISSE.ECART,
                        _LeTRANSCAISSE.TOPANNUL,
                        _LeTRANSCAISSE.MOISCOMPT,
                        _LeTRANSCAISSE.TOP1,
                        _LeTRANSCAISSE.TOURNEE,
                        _LeTRANSCAISSE.NUMDEM,
                        _LeTRANSCAISSE.DATEVALEUR,
                        _LeTRANSCAISSE.NUMCHEQ,
                        //NOMOPERATEUR = _LeTRANSCAISSE.ADMUTILISATEUR1.LIBELLE ,
                        _LeTRANSCAISSE.SAISIPAR,
                        _LeTRANSCAISSE.DATEENCAISSEMENT,
                        _LeTRANSCAISSE.CANCELLATION,
                        _LeTRANSCAISSE.USERCREATION,
                        _LeTRANSCAISSE.DATECREATION,
                        _LeTRANSCAISSE.DATEMODIFICATION,
                        _LeTRANSCAISSE.USERMODIFICATION,
                        _LeTRANSCAISSE.PK_ID,
                        _LeTRANSCAISSE.FK_IDCENTRE,
                        _LeTRANSCAISSE.FK_IDCAISSIERE,
                        _LeTRANSCAISSE.FK_IDMODEREG,
                        _LeTRANSCAISSE.FK_IDLIBELLETOP,
                        _LeTRANSCAISSE.FK_IDHABILITATIONCAISSE 

                    };
                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }
        public static DataTable RetourneReglementAjustementCredit(string pCentre, string pClient, string pOrdre)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = null;
                    var _LCLIENT = context.LCLIENT;
                    var _MODREG = context.MODEREG;
                    query =
                    from _LeLCLIENT in _LCLIENT
                    join _LeMODREG in _MODREG on _LeLCLIENT.MODEREG  equals _LeMODREG.CODE 
                    where _LeLCLIENT.CENTRE  == pCentre &&
                          _LeLCLIENT.CLIENT  == pClient &&
                          _LeLCLIENT.ORDRE  == pOrdre &&
                          _LeLCLIENT.DC == Enumere.Credit &&
                          _LeLCLIENT.IDLOT  > 0
                    select new
                    {
                        _LeLCLIENT.PK_ID,
                        _LeLCLIENT.CENTRE ,
                        _LeLCLIENT.CLIENT ,
                        _LeLCLIENT.ORDRE ,
                        _LeLCLIENT.REFEM,
                        _LeLCLIENT.NDOC,
                        _LeLCLIENT.COPER ,
                        DATEFACTURE = _LeLCLIENT.DENR,
                        _LeLCLIENT.EXIG,
                        MONTANTFACTURE = _LeLCLIENT.MONTANT,
                        _LeLCLIENT.CAPUR,
                        _LeLCLIENT.CRET,
                        _LeLCLIENT.MODEREG ,
                        LIBELLEMODEREG = _LeMODREG.LIBELLE,
                        _LeLCLIENT.DC,
                        _LeLCLIENT.ORIGINE,
                        _LeLCLIENT.CAISSE ,
                        _LeLCLIENT.ECART,
                        _LeLCLIENT.MOISCOMPT,
                        _LeLCLIENT.TOP1 ,
                        DATEEXIGIBLE = _LeLCLIENT.EXIGIBILITE,
                        _LeLCLIENT.FRAISDERETARD,
                        _LeLCLIENT.REFERENCEPUPITRE,
                        _LeLCLIENT.IDLOT ,
                        _LeLCLIENT.DATEVALEUR,
                        _LeLCLIENT.REFERENCE,
                        _LeLCLIENT.REFEMNDOC,
                        _LeLCLIENT.ACQUIT,
                        _LeLCLIENT.MATRICULE ,
                         NOMCAISSIERE = _LeLCLIENT.ADMUTILISATEUR.LIBELLE ,
                        _LeLCLIENT.TAXESADEDUIRE,
                        _LeLCLIENT.DATEFLAG,
                        _LeLCLIENT.MONTANTTVA,
                        _LeLCLIENT.IDCOUPURE ,
                        _LeLCLIENT.AGENT_COUPURE ,
                        _LeLCLIENT.RDV_COUPURE,
                        _LeLCLIENT.NUMCHEQ,
                        _LeLCLIENT.OBSERVATION_COUPURE,
                        _LeLCLIENT.FK_IDADMUTILISATEUR ,
                        _LeLCLIENT.FK_IDCENTRE ,
                        _LeLCLIENT.FK_IDLIBELLETOP ,
                        _LeLCLIENT.USERCREATION,
                        _LeLCLIENT.DATECREATION,
                        _LeLCLIENT.DATEMODIFICATION,
                        _LeLCLIENT.USERMODIFICATION

                    };
                    return Galatee.Tools.Utility.ListToDataTable(query);
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static DataTable RetourneFactureCredit(string pCentre, string pClient, string pOrdre)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = null;
                    var _LCLIENT = context.LCLIENT;
                    query =
                    from _LeLCLIENT in _LCLIENT
                    where (_LeLCLIENT.CENTRE == pCentre && _LeLCLIENT.CLIENT == pClient &&
                          _LeLCLIENT.ORDRE == pOrdre && _LeLCLIENT.IDLOT > 0 && _LeLCLIENT.DC == Enumere.Credit && _LeLCLIENT.COPER != Enumere.CoperAjsutemenFondCaisse && _LeLCLIENT.COPER != Enumere.CoperFondCaisse)
                    orderby _LeLCLIENT.DENR   
                    group _LeLCLIENT by new
                    {
                        CENTRE = _LeLCLIENT.CENTRE,
                        CLIENT = _LeLCLIENT.CLIENT,
                        ORDRE = _LeLCLIENT.ORDRE,
                        CAISSE = _LeLCLIENT.CAISSE,
                        ACQUIT = _LeLCLIENT.ACQUIT,
                        MATRICULE = _LeLCLIENT.MATRICULE,
                        MODEREG = _LeLCLIENT.MODEREG,
                        NUMCHEQ = _LeLCLIENT.NUMCHEQ,
                        BANQUE = _LeLCLIENT.BANQUE,
                        DTRANS = _LeLCLIENT.DENR,
                        FK_IDCENTRE = _LeLCLIENT.FK_IDCENTRE,
                        FK_IDMATRICULE = _LeLCLIENT.FK_IDADMUTILISATEUR,
                       IDLOT = _LeLCLIENT.IDLOT

                    } into pTemp3
                    let MONTANT = pTemp3.Sum(x => x.MONTANT)
                    select new
                    {
                        pTemp3.Key.CENTRE,
                        pTemp3.Key.CLIENT,
                        pTemp3.Key.ORDRE,
                        pTemp3.Key.CAISSE,
                        pTemp3.Key.ACQUIT,
                        pTemp3.Key.MATRICULE,
                        pTemp3.Key.MODEREG,
                        pTemp3.Key.DTRANS,
                        pTemp3.Key.BANQUE,
                        pTemp3.Key.NUMCHEQ,
                        MONTANT,
                        pTemp3.Key.FK_IDCENTRE,
                        pTemp3.Key.FK_IDMATRICULE,
                        pTemp3.Key.IDLOT 
                    };
                    return Galatee.Tools.Utility.ListToDataTable(query);
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static List<LCLIENT> RetourneFacture(string pCentre, string pClient, string pOrdre,galadbEntities pcontext)
        {
            try
            {
                    IEnumerable<object> query = null;
                    var _LCLIENT = pcontext.LCLIENT;
                    query =
                    (from _LeLCLIENT in _LCLIENT
                     where (_LeLCLIENT.CENTRE == pCentre && _LeLCLIENT.CLIENT  == pClient &&
                           _LeLCLIENT.ORDRE == pOrdre
                           && _LeLCLIENT.DC == Enumere.Debit
                           && (_LeLCLIENT.TOP1 != Enumere.TopCaisse || _LeLCLIENT.IDLOT > 0)
                           )
                     select _LeLCLIENT).ToList();
                  return (List<LCLIENT >)query;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static DataTable RetourneListeFactureClient(int fk_idcentre, string pCentre, string pClient, string pOrdre)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = null;
                    var _LCLIENT = context.LCLIENT;
                    query =
                    from _LeLCLIENT in _LCLIENT
                    where _LeLCLIENT.FK_IDCENTRE == fk_idcentre &&
                          _LeLCLIENT.CENTRE == pCentre &&
                          _LeLCLIENT.CLIENT == pClient &&
                          _LeLCLIENT.ORDRE == pOrdre &&
                          _LeLCLIENT.IDLOT == null 
                    select new
                    {
                        _LeLCLIENT.PK_ID,
                        _LeLCLIENT.CENTRE,
                        _LeLCLIENT.CLIENT,
                        _LeLCLIENT.ORDRE,

                       NOM = _LeLCLIENT.CLIENT1.NOMABON ,
                        _LeLCLIENT.REFEM,
                        _LeLCLIENT.NDOC,
                        _LeLCLIENT.COPER,
                        _LeLCLIENT.DENR,
                        _LeLCLIENT.EXIG,
                        _LeLCLIENT.MONTANT,
                        _LeLCLIENT.CAPUR,
                        _LeLCLIENT.CRET,
                        _LeLCLIENT.MODEREG,
                        _LeLCLIENT.DC,
                        _LeLCLIENT.ORIGINE,
                        _LeLCLIENT.CAISSE,
                        _LeLCLIENT.ECART,
                        _LeLCLIENT.MOISCOMPT,
                        _LeLCLIENT.TOP1,
                        DATEEXIGIBLE = _LeLCLIENT.EXIGIBILITE,
                        _LeLCLIENT.FRAISDERETARD,
                        _LeLCLIENT.REFERENCEPUPITRE,
                        _LeLCLIENT.IDLOT,
                        _LeLCLIENT.DATEVALEUR,
                        _LeLCLIENT.REFERENCE,
                        _LeLCLIENT.REFEMNDOC,
                        _LeLCLIENT.ACQUIT,
                        _LeLCLIENT.MATRICULE,
                        _LeLCLIENT.TAXESADEDUIRE,
                        _LeLCLIENT.DATEFLAG,
                        _LeLCLIENT.MONTANTTVA,
                        _LeLCLIENT.IDCOUPURE,
                        _LeLCLIENT.AGENT_COUPURE,
                        _LeLCLIENT.RDV_COUPURE,
                        _LeLCLIENT.NUMCHEQ,
                        _LeLCLIENT.OBSERVATION_COUPURE,
                        _LeLCLIENT.FK_IDADMUTILISATEUR,
                        _LeLCLIENT.FK_IDCLIENT ,
                        _LeLCLIENT.FK_IDCENTRE,
                        _LeLCLIENT.FK_IDLIBELLETOP,
                        _LeLCLIENT.USERCREATION,
                        _LeLCLIENT.DATECREATION,
                        _LeLCLIENT.DATEMODIFICATION,
                        _LeLCLIENT.USERMODIFICATION

                    };
                    return Galatee.Tools.Utility.ListToDataTable(query);
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }

            //try
            //{
            //    galadbEntities context = new galadbEntities();
            //    IEnumerable<object> query = null;
            //    var _LCLIENT = context.LCLIENT;
            //    query =
            //    (from _LeLCLIENT in _LCLIENT
            //     where (_LeLCLIENT.FK_IDCENTRE  == fk_idcentre &&   _LeLCLIENT.CENTRE == pCentre && _LeLCLIENT.CLIENT == pClient &&
            //           _LeLCLIENT.ORDRE == pOrdre && _LeLCLIENT.DC == Enumere.Debit && _LeLCLIENT.COPER == Enumere.CoperFact)
            //     select _LeLCLIENT).ToList();
            //    return (List<LCLIENT>)query;
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
        }
        public static DataTable RetourneListeReglement(int fk_idcentre, string pCentre, string pClient, string pOrdre)
        {
            try
            {
                using(galadbEntities context = new galadbEntities() )
	            {
		            var _TRANSCAISB = context.TRANSCAISB;
                    IEnumerable<object> query = null;
                    query =
                    from x in _TRANSCAISB
                    where x.FK_IDCENTRE == fk_idcentre && x.CENTRE == pCentre && x.CLIENT == pClient && x.ORDRE == pOrdre
                    select new
                    {
                        x.PK_ID,
                        x.CENTRE,
                        x.CLIENT,
                        x.ORDRE,
                        //NOM = x.LCLIENT1.CLIENT1.NOMABON,
                        x.REFEM,
                        x.NDOC,
                        x.COPER,
                        DENR = x.DATECREATION,
                        x.MONTANT,
                        x.CRET,
                        x.MODEREG,
                        x.ORIGINE,
                        x.CAISSE,
                        x.ECART,
                        x.MOISCOMPT,
                        x.TOP1,
                        x.DATEVALEUR,
                        x.ACQUIT,
                        x.MATRICULE,
                        x.DATEFLAG,
                        x.NUMCHEQ,
                        x.USERCREATION,
                        x.DATECREATION,
                        x.DATEMODIFICATION,
                        x.USERMODIFICATION,
                        x.BANQUE,
                        x.GUICHET,
                        x.FK_IDCENTRE,
                        x.FK_IDCOPER,
                        x.FK_IDLIBELLETOP,
                        //x.LCLIENT1.FK_IDCLIENT,
                        x.POSTE,
                        x.DATETRANS,
                        //LIBELLENATURE = x.NATURE1.LIBCOURT,
                        //LIBELLECOPER = x.COPER1.LIBCOURT,
                        DC = "C"
                };
               return Galatee.Tools.Utility.ListToDataTable(query);
               }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static DataTable RetourneFactureClient(int fk_idcentre, string pCentre, string pClient, string pOrdre)
        {
            try
            {

                using (galadbEntities context = new galadbEntities())
                {
                    var _LCLIENT = context.LCLIENT;
                    IEnumerable<object> query = null;
                    query =
                    from x in _LCLIENT
                    where x.FK_IDCENTRE == fk_idcentre && x.CENTRE == pCentre && x.CLIENT == pClient && x.ORDRE == pOrdre 
                    select new
                    {
                        x.PK_ID,
                        x.CENTRE,
                        x.CLIENT,
                        x.ORDRE,
                        NOM = x.CLIENT1.NOMABON ,
                        x.REFEM,
                        x.NDOC,
                        x.COPER,
                        x.DENR,
                        x.EXIG,
                        x.MONTANT,
                        x.CAPUR,
                        x.CRET,
                        x.MODEREG,
                        x.ORIGINE,
                        x.CAISSE,
                        x.ECART,
                        x.MOISCOMPT,
                        x.TOP1,
                        x.EXIGIBILITE,
                        x.FRAISDERETARD,
                        x.REFERENCEPUPITRE,
                        x.IDLOT,
                        x.DATEVALEUR,
                        x.REFERENCE,
                        x.REFEMNDOC,
                        x.ACQUIT,
                        x.MATRICULE,
                        x.TAXESADEDUIRE,
                        x.DATEFLAG,
                        x.MONTANTTVA,
                        x.IDCOUPURE,
                        x.AGENT_COUPURE,
                        x.RDV_COUPURE,
                        x.NUMCHEQ,
                        x.OBSERVATION_COUPURE,
                        x.USERCREATION,
                        x.DATECREATION,
                        x.DATEMODIFICATION,
                        x.USERMODIFICATION,
                        x.BANQUE,
                        x.GUICHET,
                        x.FK_IDCENTRE,
                        x.FK_IDCOPER,
                        x.FK_IDLIBELLETOP,
                        x.FK_IDCLIENT,
                        x.FK_IDPOSTE,
                        x.POSTE,
                        x.DATETRANS,
                        LIBELLECOPER = x.COPER1 .LIBCOURT ,
                        DC = "D"
                        
                    };
                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static DataTable RetourneEncaissement(int Fk_idcentre, string pCentre, string pClient, string pOrdre,List<string> lstPeriode)
        {
            try
            {
                List<CsLclient> LstEncaissement = new List<CsLclient>();
                galadbEntities context = new galadbEntities();
                IEnumerable<object> query = null;
                var _TRANSCAISSE = context.TRANSCAISSE;
                query =
                from _LeTRANSCAISSE in _TRANSCAISSE
                where (_LeTRANSCAISSE.FK_IDCENTRE == Fk_idcentre &&
                       _LeTRANSCAISSE.CENTRE == pCentre &&
                       _LeTRANSCAISSE.CLIENT == pClient &&
                       _LeTRANSCAISSE.ORDRE == pOrdre &&
                       _LeTRANSCAISSE.COPER != Enumere.CoperTimbre &&
                       lstPeriode.Contains(_LeTRANSCAISSE.REFEM) &&
                       (_LeTRANSCAISSE.TOPANNUL != "O" || _LeTRANSCAISSE.TOPANNUL == null))
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
                    DTRANS = _LeTRANSCAISSE.DTRANS,
                    DENR = _LeTRANSCAISSE.DTRANS,
                    DATEENCAISSEMENT = _LeTRANSCAISSE.DTRANS,
                    FK_IDCENTRE = _LeTRANSCAISSE.FK_IDCENTRE,
                    FK_IDMODEREG = _LeTRANSCAISSE.FK_IDMODEREG,
                    FK_IDCAISSIERE = _LeTRANSCAISSE.FK_IDCAISSIERE,
                    FK_IDHABILITATIONCAISSE = _LeTRANSCAISSE.FK_IDHABILITATIONCAISSE,
                    SAISIPAR = _LeTRANSCAISSE.SAISIPAR,
                    NUMDEM = _LeTRANSCAISSE.NUMDEM,
                    LIBELLECOPER = _LeTRANSCAISSE.COPER1.LIBCOURT,
                    LIBELLEMODREG = _LeTRANSCAISSE.MODEREG1.LIBELLE,
                    NOMCAISSIERE = _LeTRANSCAISSE.HABILITATIONCAISSE == null ? _LeTRANSCAISSE.ADMUTILISATEUR1.LIBELLE : _LeTRANSCAISSE.HABILITATIONCAISSE.ADMUTILISATEUR.LIBELLE


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
                    DC = "C"
                };


                IEnumerable<object> query1 = null;
                var _TRANSCAISB = context.TRANSCAISB;
                query1 =
                from _LeTRANSCAISB in _TRANSCAISB
                where (_LeTRANSCAISB.FK_IDCENTRE == Fk_idcentre &&
                       _LeTRANSCAISB.CENTRE == pCentre &&
                       _LeTRANSCAISB.CLIENT == pClient &&
                       _LeTRANSCAISB.ORDRE == pOrdre &&
                       _LeTRANSCAISB.COPER != Enumere.CoperTimbre &&
                       lstPeriode.Contains(_LeTRANSCAISB.REFEM) &&
                       (_LeTRANSCAISB.TOPANNUL != "O" || _LeTRANSCAISB.TOPANNUL == null))
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
                    NOMCAISSIERE = _LeTRANSCAISB.HABILITATIONCAISSE == null ? _LeTRANSCAISB.ADMUTILISATEUR1.LIBELLE : _LeTRANSCAISB.HABILITATIONCAISSE.ADMUTILISATEUR.LIBELLE


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
                    DC = "C"
                };
                var result = query.Union(query1);
                return Galatee.Tools.Utility.ListToDataTable(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static DataTable RetourneFactureNegative(int Fk_idcentre, string pCentre, string pClient, string pOrdre)
        {
            try
            {

                List<CsLclient> LstEncaissement = new List<CsLclient>();
                galadbEntities context = new galadbEntities();
                IEnumerable<object> query = null;
                var _LCLIENT = context.LCLIENT;
                query =
                from _LeCLIENT in _LCLIENT
                where (_LeCLIENT.FK_IDCENTRE == Fk_idcentre &&
                       _LeCLIENT.CENTRE == pCentre &&
                       _LeCLIENT.CLIENT == pClient &&
                       _LeCLIENT.ORDRE == pOrdre  &&
                       _LeCLIENT.MONTANT <0)
                orderby _LeCLIENT.DENR
                select new
                {
                    _LeCLIENT.CENTRE,
                    _LeCLIENT.CLIENT,
                    _LeCLIENT.ORDRE,
                    _LeCLIENT.CAISSE,
                    _LeCLIENT.ACQUIT,
                    _LeCLIENT.MATRICULE,
                    _LeCLIENT.NDOC,
                    _LeCLIENT.REFEM,
                    _LeCLIENT.DENR,
                    DTRANS = _LeCLIENT.DENR ,
                    _LeCLIENT.MONTANT ,
                    DATEENCAISSEMENT = _LeCLIENT.DENR,
                    _LeCLIENT.FK_IDCENTRE,
                    DC=  "D",
                    _LeCLIENT.COPER,
                    _LeCLIENT.TOP1,
                    _LeCLIENT.FK_IDCOPER,
                    _LeCLIENT.FK_IDLIBELLETOP
                };
                var result = query ;
                return Galatee.Tools.Utility.ListToDataTable(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static DataTable RetourneEncaissement(int Fk_idcentre, string pCentre, string pClient, string pOrdre)
        {
            try
            {

                List<CsLclient> LstEncaissement = new List<CsLclient>();
                galadbEntities context = new galadbEntities();
                IEnumerable<object> query = null;
                var _TRANSCAISSE = context.TRANSCAISSE ;
                query =
                from _LeTRANSCAISSE in _TRANSCAISSE
                where (_LeTRANSCAISSE.FK_IDCENTRE == Fk_idcentre &&
                       _LeTRANSCAISSE.CENTRE == pCentre &&
                       _LeTRANSCAISSE.CLIENT == pClient &&
                       _LeTRANSCAISSE.ORDRE == pOrdre &&
                       _LeTRANSCAISSE.COPER != Enumere.CoperTimbre &&
                       (_LeTRANSCAISSE.TOPANNUL != "O" || _LeTRANSCAISSE.TOPANNUL == null))
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
                    DTRANS = _LeTRANSCAISSE.DTRANS,
                    DENR = _LeTRANSCAISSE.DTRANS,
                    DATEENCAISSEMENT = _LeTRANSCAISSE.DTRANS,
                    FK_IDCENTRE = _LeTRANSCAISSE.FK_IDCENTRE,
                    FK_IDMODEREG = _LeTRANSCAISSE.FK_IDMODEREG,
                    FK_IDCAISSIERE = _LeTRANSCAISSE.FK_IDCAISSIERE,
                    FK_IDHABILITATIONCAISSE = _LeTRANSCAISSE.FK_IDHABILITATIONCAISSE,
                    FK_IDCOPER = _LeTRANSCAISSE.FK_IDCOPER,
                    FK_IDLIBELLETOP = _LeTRANSCAISSE.FK_IDLIBELLETOP,
                    SAISIPAR = _LeTRANSCAISSE.SAISIPAR,
                    NUMDEM = _LeTRANSCAISSE.NUMDEM,
                    LIBELLECOPER = _LeTRANSCAISSE.COPER1.LIBCOURT,
                    LIBELLEMODREG = _LeTRANSCAISSE.MODEREG1.LIBELLE,
                    NOMCAISSIERE = _LeTRANSCAISSE.HABILITATIONCAISSE == null ? _LeTRANSCAISSE.ADMUTILISATEUR1.LIBELLE : _LeTRANSCAISSE.HABILITATIONCAISSE.ADMUTILISATEUR.LIBELLE,
                    DC = _LeTRANSCAISSE.DC,
                    COPER = _LeTRANSCAISSE.COPER,
                    TOP1 = _LeTRANSCAISSE.TOP1  

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
                    pTemp.Key.DC,
                    pTemp.Key.COPER,
                    pTemp.Key.TOP1,
                    pTemp.Key.FK_IDCOPER,
                    pTemp.Key.FK_IDLIBELLETOP
                };


                IEnumerable<object> query1 = null;
                var _TRANSCAISB = context.TRANSCAISB;
                query1 =
                from _LeTRANSCAISB in _TRANSCAISB
                where (_LeTRANSCAISB.FK_IDCENTRE == Fk_idcentre &&
                       _LeTRANSCAISB.CENTRE == pCentre &&
                       _LeTRANSCAISB.CLIENT == pClient &&
                       _LeTRANSCAISB.ORDRE == pOrdre &&
                       _LeTRANSCAISB.COPER != Enumere.CoperTimbre &&
                       (_LeTRANSCAISB.TOPANNUL != "O" || _LeTRANSCAISB.TOPANNUL == null))
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
                    DTRANS = _LeTRANSCAISB.DTRANS,
                    DENR = _LeTRANSCAISB.DTRANS,
                    DATEENCAISSEMENT = _LeTRANSCAISB.DTRANS,
                    FK_IDCENTRE = _LeTRANSCAISB.FK_IDCENTRE,
                    FK_IDMODEREG = _LeTRANSCAISB.FK_IDMODEREG,
                    FK_IDCAISSIERE = _LeTRANSCAISB.FK_IDCAISSIERE,
                    FK_IDHABILITATIONCAISSE = _LeTRANSCAISB.FK_IDHABILITATIONCAISSE,
                    FK_IDCOPER = _LeTRANSCAISB.FK_IDCOPER,
                    FK_IDLIBELLETOP = _LeTRANSCAISB.FK_IDLIBELLETOP,
                    SAISIPAR = _LeTRANSCAISB.SAISIPAR,
                    NUMDEM = _LeTRANSCAISB.NUMDEM,
                    LIBELLECOPER = _LeTRANSCAISB.COPER1.LIBCOURT,
                    LIBELLEMODREG = _LeTRANSCAISB.MODEREG1.LIBELLE,
                    NOMCAISSIERE = _LeTRANSCAISB.HABILITATIONCAISSE == null ? _LeTRANSCAISB.ADMUTILISATEUR1.LIBELLE : _LeTRANSCAISB.HABILITATIONCAISSE.ADMUTILISATEUR.LIBELLE,
                    DC = _LeTRANSCAISB.DC,
                    COPER = _LeTRANSCAISB.COPER,
                    TOP1 = _LeTRANSCAISB.TOP1

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
                    DC ="C",
                    pTemp.Key.COPER,
                    pTemp.Key.TOP1,
                    pTemp.Key.FK_IDCOPER,
                    pTemp.Key.FK_IDLIBELLETOP
                };
                var result = query.Union(query1);
                //var result = query;
                return Galatee.Tools.Utility.ListToDataTable(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static List<TRANSCAISB> RetourneTranscaisB(string pCentre, string pClient, string pOrdre)
        {
            try
            {
                galadbEntities context = new galadbEntities();
                IEnumerable<object> query = null;
                var _TRANSCAISB = context.TRANSCAISB;
                query =
                from _LeTRANSCAISB in _TRANSCAISB
                 where (_LeTRANSCAISB.CENTRE == pCentre && _LeTRANSCAISB.CLIENT == pClient &&
                       _LeTRANSCAISB.ORDRE == pOrdre)
                 select new {
                            _LeTRANSCAISB. CENTRE ,
                            _LeTRANSCAISB. CLIENT ,
                            _LeTRANSCAISB. ORDRE ,
                            _LeTRANSCAISB. CAISSE ,
                            _LeTRANSCAISB. ACQUIT ,
                            _LeTRANSCAISB. MATRICULE ,
                            _LeTRANSCAISB. NDOC ,
                            _LeTRANSCAISB. REFEM ,
                            _LeTRANSCAISB.  MONTANT ,
                            _LeTRANSCAISB. DC ,
                            _LeTRANSCAISB. COPER ,
                            _LeTRANSCAISB.  PERCU ,
                            _LeTRANSCAISB.  RENDU ,
                            _LeTRANSCAISB. MODEREG ,
                            _LeTRANSCAISB. PLACE ,
                            _LeTRANSCAISB.DTRANS ,
                            _LeTRANSCAISB.DEXIG ,
                            _LeTRANSCAISB. BANQUE ,
                            _LeTRANSCAISB. GUICHET ,
                            _LeTRANSCAISB. ORIGINE ,
                            _LeTRANSCAISB.  ECART ,
                            _LeTRANSCAISB. TOPANNUL ,
                            _LeTRANSCAISB. MOISCOMPT ,
                            _LeTRANSCAISB. TOP1 ,
                            _LeTRANSCAISB. TOURNEE ,
                            _LeTRANSCAISB. NUMDEM ,
                            _LeTRANSCAISB. DATEVALEUR ,
                            _LeTRANSCAISB. DATEFLAG ,
                            _LeTRANSCAISB. NUMCHEQ ,
                            _LeTRANSCAISB. SAISIPAR ,
                            _LeTRANSCAISB.DATEENCAISSEMENT ,
                            _LeTRANSCAISB. CANCELLATION ,
                            _LeTRANSCAISB. USERCREATION ,
                            _LeTRANSCAISB.  DATECREATION ,
                            _LeTRANSCAISB.DATEMODIFICATION ,
                            _LeTRANSCAISB. USERMODIFICATION ,
                            _LeTRANSCAISB. PK_ID ,
                            _LeTRANSCAISB. FK_IDCENTRE ,
                            _LeTRANSCAISB.FK_IDHABILITATIONCAISSE  ,
                            _LeTRANSCAISB. FK_IDMODEREG ,
                            _LeTRANSCAISB. FK_IDLIBELLETOP ,
                            _LeTRANSCAISB.FK_IDCAISSIERE  ,
                            _LeTRANSCAISB. FK_IDCOPER
                 };
                return (List<TRANSCAISB >)query;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static DataTable RechercherClient(int Fk_idcentre, string pCentre, string pClient, string pOrdre, string pProduit, string pNomClient)
        {
            try
            {
                galadbEntities context = new galadbEntities();                
                IEnumerable<object> query = null;
                var _CLIENT = context.CLIENT;
                query =
                from _LeCLIENT in _CLIENT
                where
                (_LeCLIENT.CENTRE == pCentre && _LeCLIENT.FK_IDCENTRE == Fk_idcentre) &&
                (_LeCLIENT.REFCLIENT == pClient || string.IsNullOrEmpty(pClient)) &&
                (_LeCLIENT.NOMABON.Contains(pNomClient) || string.IsNullOrEmpty(pNomClient)) &&
                (_LeCLIENT.ABON.FirstOrDefault().PRODUIT1.CODE  == pProduit || string.IsNullOrEmpty(pProduit)) 
                select new
                {
                    CENTRE = _LeCLIENT.CENTRE,
                    CLIENT = _LeCLIENT.REFCLIENT,
                    _LeCLIENT.ORDRE,
                    _LeCLIENT.NOMABON,
                    _LeCLIENT.FK_IDCENTRE                      
                };
                return Galatee.Tools.Utility.ListToDataTable(query.Take(1000));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static DataTable RechercherClientTournee(int Fk_idcentre, string Centre, string pTournee, string pOrdreTourne, string pProduit)
        {
            try
            {
                string Tournee = string.IsNullOrEmpty(pTournee) ? "" : pTournee;
                string OrdreTournee = string.IsNullOrEmpty(pOrdreTourne) ? "" : pOrdreTourne;
                galadbEntities context = new galadbEntities();
                IEnumerable<object> query = null;
                var _AG = context.AG ;
                query =
                from _LeAg in _AG
                from _LeClient in _LeAg.CLIENT1 
                where
                (_LeClient.CENTRE == Centre && _LeClient.FK_IDCENTRE == Fk_idcentre) &&
                (_LeAg.TOURNEE1.CODE  == Tournee || string.IsNullOrEmpty(Tournee)) &&
                (_LeAg.ORDTOUR == OrdreTournee || string.IsNullOrEmpty(OrdreTournee))
                group new { _LeClient }
                by new { _LeClient.CENTRE, _LeClient.REFCLIENT , _LeClient.ORDRE, _LeClient.NOMABON ,_LeClient.FK_IDCENTRE   } into pTemp3

                select new
                {
                    CENTRE = pTemp3.Key.CENTRE,
                    CLIENT = pTemp3.Key.REFCLIENT ,
                    pTemp3.Key.ORDRE,
                    pTemp3.Key.NOMABON,
                    pTemp3.Key.FK_IDCENTRE                     
                };
                return Galatee.Tools.Utility.ListToDataTable(query.Take(1000));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static DataTable RechercherClientCompteur( string pNumCompteur)
        {
            try
            {
                string NumCompteur = string.IsNullOrEmpty(pNumCompteur) ? "" : pNumCompteur;

                galadbEntities context = new galadbEntities();
                IEnumerable<object> query = null;
                var _AG = context.AG;
                query =
                from _LeAG in _AG
                from _LeClient in _LeAG.CLIENT1
                from _leAbon in _LeClient.ABON
                from _leCompteur in _leAbon.CANALISATION
                where
                (_leCompteur.COMPTEUR.NUMERO == pNumCompteur )
                group new { _LeClient }
                by new { _LeClient.CENTRE, _LeClient.REFCLIENT, _LeClient.ORDRE, _LeClient.NOMABON, _LeClient.FK_IDCENTRE,_LeClient.PK_ID  } into pTemp3
                select new
                {
                    CENTRE = pTemp3.Key.CENTRE,
                    CLIENT = pTemp3.Key.REFCLIENT,
                    pTemp3.Key.ORDRE,
                    pTemp3.Key.NOMABON,
                    pTemp3.Key.FK_IDCENTRE,
                    pTemp3.Key.PK_ID 

                };
                return Galatee.Tools.Utility.ListToDataTable(query.Take(1000));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable RechercherClientCompteur(int Fk_idclient, string Centre, string pNumCompteur, string pProduit)
        {
            try
            {
                string NumCompteur = string.IsNullOrEmpty(pNumCompteur) ? "" : pNumCompteur;

                galadbEntities context = new galadbEntities();
                IEnumerable<object> query = null;
                var _AG = context.AG ;
                query =
                from _LeAG in _AG
                from _LeClient in _LeAG.CLIENT1
                from _leAbon in _LeClient.ABON
                from _leCompteur in _leAbon.CANALISATION 
                where
                (_LeClient.CENTRE == Centre && _leCompteur.FK_IDCENTRE == Fk_idclient) &&
                (_leCompteur.COMPTEUR.NUMERO    == pNumCompteur || string.IsNullOrEmpty(pNumCompteur)) 
                group new { _LeClient }
                by new { _LeClient.CENTRE, _LeClient.REFCLIENT, _LeClient.ORDRE, _LeClient.NOMABON,_LeClient.FK_IDCENTRE   } into pTemp3
                select new
                {
                    CENTRE = pTemp3.Key.CENTRE,
                    CLIENT = pTemp3.Key.REFCLIENT,
                    pTemp3.Key.ORDRE,
                    pTemp3.Key.NOMABON,
                    pTemp3.Key.FK_IDCENTRE 

                };
                return Galatee.Tools.Utility.ListToDataTable(query.Take(1000));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static DataTable RechercherClientAdresseElectrique(int fk_idcentre, string Centre, string AdresseElectrique)
        {
            try
            {
                galadbEntities context = new galadbEntities();
                IEnumerable<object> query = null;
                var _AG = context.AG ;
                query =
                from _LeAg in _AG
                from _leBrt in _LeAg.BRT
                from _LeClient in _LeAg.CLIENT1
                where _leBrt.ADRESSERESEAU == AdresseElectrique && _leBrt.CENTRE == Centre && _leBrt.FK_IDCENTRE == fk_idcentre
                group new { _LeClient }
                by new { _LeClient.CENTRE, _LeClient.REFCLIENT, _LeClient.ORDRE, _LeClient.NOMABON,_LeClient.FK_IDCENTRE   } into pTemp3
                select new
                {
                    CENTRE = pTemp3.Key.CENTRE,
                    CLIENT = pTemp3.Key.REFCLIENT,
                    pTemp3.Key.ORDRE,
                    pTemp3.Key.NOMABON,
                    pTemp3.Key.FK_IDCENTRE 

                };
                return Galatee.Tools.Utility.ListToDataTable(query.Take(1000));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static DataTable RechercherClientAdresseGeographique(int fk_idcentre, string Centre, string pTournee, string pOrdreTourne, string pLongitude,string pLatitude,string pPorte,string pRue,string pLot, string pProduit)
        {
            try
            {
                galadbEntities context = new galadbEntities();
                IEnumerable<object> query = null;
                var _AG = context.AG ;
                query =
                from _LeAg in _AG
                from _LeClient in _LeAg.CLIENT1
                from _leBrt in _LeAg.BRT 
                where
                (_LeClient.CENTRE == Centre && _LeClient.FK_IDCENTRE == fk_idcentre) &&
                (_LeAg.TOURNEE1.CODE == pTournee || string.IsNullOrEmpty(pTournee)) &&
                (_LeAg.ORDTOUR == pOrdreTourne || string.IsNullOrEmpty(pOrdreTourne))&&
                (_leBrt.LONGITUDE == pLongitude || string.IsNullOrEmpty(pLongitude)) &&
                (_leBrt.LATITUDE  == pLatitude || string.IsNullOrEmpty(pLatitude))&&
                (_LeAg.PORTE == pPorte  || string.IsNullOrEmpty(pPorte)) &&
                (_LeAg.RUE == pRue || string.IsNullOrEmpty(pRue)) &&
                (_LeAg.CPARC == pLot || string.IsNullOrEmpty(pLot))
                group new { _LeClient }
                by new { _LeClient.CENTRE, _LeClient.REFCLIENT, _LeClient.ORDRE, _LeClient.NOMABON,_LeClient.FK_IDCENTRE   } into pTemp3

                select new
                {
                    CENTRE = pTemp3.Key.CENTRE,
                    CLIENT = pTemp3.Key.REFCLIENT,
                    pTemp3.Key.ORDRE,
                    pTemp3.Key.NOMABON,
                    pTemp3.Key.FK_IDCENTRE 
                };
                return Galatee.Tools.Utility.ListToDataTable(query.Take(1000));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static DataTable RetourneCanalisationOnly(int fk_idcentre, string pCentre, string pClient, string pProduit, int? pPoint)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    string point = pPoint == null ? string.Empty : pPoint.ToString();

                    IEnumerable<object> query = null;
                    var _CANALISATION = context.CANALISATION;
                    query =
                        (from _LeCANALISATION in _CANALISATION
                        where
                        _LeCANALISATION.CENTRE == pCentre &&
                              _LeCANALISATION.CLIENT == pClient &&
                              _LeCANALISATION.FK_IDCENTRE == fk_idcentre &&
                        (string.IsNullOrEmpty(pProduit) || _LeCANALISATION.PRODUIT == pProduit) &&
                        (string.IsNullOrEmpty(point) || _LeCANALISATION.POINT == pPoint)
                        select new
                        {
                            _LeCANALISATION.CENTRE,
                            _LeCANALISATION.CLIENT,
                            _LeCANALISATION.PRODUIT,
                            _LeCANALISATION.PROPRIO,
                            _LeCANALISATION.REGLAGECOMPTEUR,
                            _LeCANALISATION.POINT,
                            _LeCANALISATION.BRANCHEMENT,
                            _LeCANALISATION.SURFACTURATION,
                            _LeCANALISATION.DEBITANNUEL,
                            _LeCANALISATION.REPERAGECOMPTEUR,
                            _LeCANALISATION.POSE,
                            _LeCANALISATION.DEPOSE,
                            _LeCANALISATION.USERCREATION,
                            _LeCANALISATION.DATECREATION,
                            _LeCANALISATION.DATEMODIFICATION,
                            _LeCANALISATION.USERMODIFICATION,
                            _LeCANALISATION.PK_ID,
                            _LeCANALISATION.FK_IDABON,
                            _LeCANALISATION.FK_IDCENTRE,
                            _LeCANALISATION.FK_IDCOMPTEUR,
                            _LeCANALISATION.FK_IDPRODUIT,
                            _LeCANALISATION.FK_IDPROPRIETAIRE,
                            _LeCANALISATION.FK_IDREGLAGECOMPTEUR,
                            _LeCANALISATION.ORDREAFFICHAGE

                        }).Take(1);
                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static DataTable RetourneAbon(int fk_idcentre, string pCentre, string pClient, string pOrdre)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {

                    IEnumerable<object> query = null;
                    var _Ag = context.AG ;
                    query =
                    from _LeAg in _Ag
                    from _LeClient in _LeAg.CLIENT1 
                    from _LeAbon in _LeClient.ABON
                    where  _LeClient.CENTRE == pCentre && 
                           _LeClient.FK_IDCENTRE == fk_idcentre &&
                    _LeClient.REFCLIENT == pClient && ( _LeClient.ORDRE == pOrdre || string.IsNullOrEmpty(pOrdre))
                     select new
                        {
                            _LeAbon.CENTRE  ,
                            _LeAbon.CLIENT  ,
                            _LeAbon.ORDRE  ,
                            _LeAbon.PRODUIT,
                            _LeAbon.TYPETARIF,
                            _LeAbon.FORFAIT,
                            _LeAbon.FORFPERSO,
                            _LeAbon.AVANCE,
                            _LeAbon.DAVANCE,
                            _LeAbon.REGROU,
                            _LeAbon.PERFAC,
                            _LeAbon.DABONNEMENT,
                            _LeAbon.DRES,
                            _LeAbon.DATERACBRT,
                            _LeAbon.NBFAC,
                            _LeAbon.PERREL,
                            _LeAbon.RECU,
                            _LeAbon.RISTOURNE,
                            _LeAbon.CONSOMMATIONMAXI,
                            _LeAbon.PUISSANCE,
                            _LeAbon.COEFFAC,
                            _LeAbon.USERCREATION,
                            _LeAbon.DATECREATION,
                            _LeAbon.DATEMODIFICATION,
                            _LeAbon.USERMODIFICATION,
                            _LeAbon.FK_IDCLIENT,
                            _LeAbon.PK_ID,
                            _LeAbon.FK_IDCENTRE,
                            _LeAbon.FK_IDPRODUIT,
                            _LeAbon.FK_IDFORFAIT,
                            _LeAbon.FK_IDMOISREL,
                            _LeAbon.FK_IDMOISFAC,
                            _LeAbon.FK_IDTYPETARIF,
                            _LeAbon.FK_IDPERIODICITEFACTURE,
                            _LeAbon.FK_IDPERIODICITERELEVE ,
                            _LeAbon.MOISFAC,
                            _LeAbon.MOISREL,
                            _LeAbon.NOMBREDEFOYER,
                            _LeAbon.TYPECOMPTAGE ,
                            _LeAbon.DEBUTEXONERATIONTVA ,
                            _LeAbon.FINEXONERATIONTVA ,
                            _LeAbon.ESTEXONERETVA ,

                            NOMABON = _LeAbon.CLIENT1.NOMABON, 
                            CATEGORIE = _LeAbon.CLIENT1.CATEGORIE ,
                            FK_IDCATEGORIE = _LeAbon.CLIENT1.FK_IDCATEGORIE ,
                            LIBELLECENTRE = _LeAbon.CENTRE1.LIBELLE,
                            LIBELLEPRODUIT = _LeAbon.PRODUIT1.LIBELLE,
                            LIBELLETARIF = _LeAbon.TYPETARIF1.LIBELLE,
                            LIBELLEFORFAIT = _LeAbon.FORFAIT1.LIBELLE,
                            LIBELLEMOISFACT = _LeAbon.MOIS.LIBELLE,
                            LIBELLEMOISIND = _LeAbon.MOIS1.LIBELLE,
                            LIBELLEFREQUENCE = _LeAbon.PERIODICITE.LIBELLE ,  
                            LIBELLETYPECOMPTAGE= _LeAbon.TYPECOMPTAGE1.LIBELLE    
                        };
                    return Galatee.Tools.Utility.ListToDataTable(query);
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static DataTable RetourneAbonProduit(int fk_idcentre, string pCentre, string pClient, string pOrdre,string pProduit)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {

                    IEnumerable<object> query = null;
                    var _Ag = context.AG;
                    query =
                    from _LeAg in _Ag
                    from _LeClient in _LeAg.CLIENT1
                    from _LeAbon in _LeClient.ABON
                    where _LeClient.CENTRE == pCentre &&
                           _LeClient.FK_IDCENTRE == fk_idcentre &&
                    _LeClient.REFCLIENT == pClient && (_LeClient.ORDRE == pOrdre || string.IsNullOrEmpty(pOrdre)) &&
                    _LeAbon.PRODUIT == pProduit 
                    select new
                    {
                        _LeAbon.CENTRE,
                        _LeAbon.CLIENT,
                        _LeAbon.ORDRE,
                        _LeAbon.PRODUIT,
                        _LeAbon.TYPETARIF,
                        _LeAbon.FORFAIT,
                        _LeAbon.FORFPERSO,
                        _LeAbon.AVANCE,
                        _LeAbon.DAVANCE,
                        _LeAbon.REGROU,
                        _LeAbon.PERFAC,
                        _LeAbon.DABONNEMENT,
                        _LeAbon.DRES,
                        _LeAbon.DATERACBRT,
                        _LeAbon.NBFAC,
                        _LeAbon.PERREL,
                        _LeAbon.RECU,
                        _LeAbon.RISTOURNE,
                        _LeAbon.CONSOMMATIONMAXI,
                        _LeAbon.PUISSANCE,
                        _LeAbon.COEFFAC,
                        _LeAbon.USERCREATION,
                        _LeAbon.DATECREATION,
                        _LeAbon.DATEMODIFICATION,
                        _LeAbon.USERMODIFICATION,
                        _LeAbon.FK_IDCLIENT,
                        _LeAbon.PK_ID,
                        _LeAbon.FK_IDCENTRE,
                        _LeAbon.FK_IDPRODUIT,
                        _LeAbon.FK_IDFORFAIT,
                        _LeAbon.FK_IDMOISREL,
                        _LeAbon.FK_IDMOISFAC,
                        _LeAbon.FK_IDTYPETARIF,
                        _LeAbon.FK_IDPERIODICITEFACTURE,
                        _LeAbon.FK_IDPERIODICITERELEVE,
                        _LeAbon.MOISFAC,
                        _LeAbon.MOISREL,
                        _LeAbon.NOMBREDEFOYER,

                        NOMABON = _LeAbon.CLIENT1.NOMABON,
                        CATEGORIE = _LeAbon.CLIENT1.CATEGORIE,
                        FK_IDCATEGORIE = _LeAbon.CLIENT1.FK_IDCATEGORIE,
                        LIBELLECENTRE = _LeAbon.CENTRE1.LIBELLE,
                        LIBELLEPRODUIT = _LeAbon.PRODUIT1.LIBELLE,
                        LIBELLETARIF = _LeAbon.TYPETARIF1.LIBELLE,
                        LIBELLEFORFAIT = _LeAbon.FORFAIT1.LIBELLE,
                        LIBELLEMOISFACT = _LeAbon.MOIS.LIBELLE,
                        LIBELLEMOISIND = _LeAbon.MOIS1.LIBELLE,
                        LIBELLEFREQUENCE = _LeAbon.PERIODICITE.LIBELLE
                    };
                    return Galatee.Tools.Utility.ListToDataTable(query);
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static DataTable RetourneClient(int fk_idcentre, string pCentre, string pClient, string pOrdre)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    string Ordre = pOrdre == null ? "" : pOrdre;
                    string vide = "";

                    IEnumerable<object> query = null;
                    var _CLIENT = context.CLIENT;
                  
                        query =
                        from _LeCLIENT in _CLIENT
                        where
                              _LeCLIENT.FK_IDCENTRE == fk_idcentre &&
                              _LeCLIENT.CENTRE == pCentre &&
                              _LeCLIENT.REFCLIENT == pClient &&
                              (_LeCLIENT.ORDRE == pOrdre || Ordre.Equals(vide))
                        select new
                        {

                            _LeCLIENT.CENTRE,
                            _LeCLIENT.REFCLIENT,
                            _LeCLIENT.ORDRE,
                            _LeCLIENT.DENABON,
                            _LeCLIENT.NOMABON,
                            _LeCLIENT.DENMAND,
                            _LeCLIENT.NOMMAND,
                            _LeCLIENT.ADRMAND1,
                            _LeCLIENT.ADRMAND2,
                            _LeCLIENT.CPOS,
                            _LeCLIENT.BUREAU,
                            _LeCLIENT.DINC,
                            _LeCLIENT.NOMTIT,
                            _LeCLIENT.BANQUE,
                            _LeCLIENT.GUICHET,
                            _LeCLIENT.COMPTE,
                            _LeCLIENT.RIB,
                            _LeCLIENT.PROPRIO,
                            _LeCLIENT.CODECONSO,
                            _LeCLIENT.CATEGORIE,
                            _LeCLIENT.CODERELANCE,
                            _LeCLIENT.NOMCOD,
                            _LeCLIENT.MOISNAIS,
                            _LeCLIENT.ANNAIS,
                            _LeCLIENT.NOMPERE,
                            _LeCLIENT.NOMMERE,
                            _LeCLIENT.NATIONNALITE,
                            _LeCLIENT.CNI,
                            _LeCLIENT.TELEPHONE,
                            _LeCLIENT.MATRICULE,
                            _LeCLIENT.REGROUPEMENT ,
                            _LeCLIENT.REGEDIT,
                            _LeCLIENT.FACTURE,
                            _LeCLIENT.DMAJ,
                            _LeCLIENT.REFERENCEPUPITRE,
                            _LeCLIENT.PAYEUR,
                            _LeCLIENT.SOUSACTIVITE,
                            _LeCLIENT.AGENTFACTURE,
                            _LeCLIENT.AGENTRECOUVR,
                            _LeCLIENT.AGENTASSAINI,
                            _LeCLIENT.REGROUCONTRAT,
                            _LeCLIENT.INSPECTION,
                            _LeCLIENT.REGLEMENT,
                            _LeCLIENT.DECRET,
                            _LeCLIENT.CONVENTION,
                            _LeCLIENT.REFERENCEATM,
                            _LeCLIENT.PK_ID,
                            _LeCLIENT.DATECREATION,
                            _LeCLIENT.DATEMODIFICATION,
                            _LeCLIENT.USERCREATION,
                            _LeCLIENT.USERMODIFICATION,
                            _LeCLIENT.FK_IDMODEPAIEMENT,
                            _LeCLIENT.FK_IDAG,
                            _LeCLIENT.FK_IDCODECONSO,
                            _LeCLIENT.FK_IDCATEGORIE,
                            _LeCLIENT.FK_IDRELANCE,
                            _LeCLIENT.FK_IDNATIONALITE,
                            _LeCLIENT.FK_IDREGROUPEMENT,
                            _LeCLIENT.FK_IDCENTRE,
                            _LeCLIENT.FK_TYPECLIENT,
                            _LeCLIENT.FK_IDUSAGE,
                            _LeCLIENT.FK_IDPROPRIETAIRE ,
                            _LeCLIENT.MODEPAIEMENT,
                            _LeCLIENT.FAX,
                            _LeCLIENT.CODEIDENTIFICATIONNATIONALE,
                            LIBELLEMODEPAIEMENT = _LeCLIENT.MODEPAIEMENT1.LIBELLE,
                            LIBELLECODECONSO = _LeCLIENT.CODECONSOMMATEUR .LIBELLE,
                            LIBELLECATEGORIE = _LeCLIENT.CATEGORIECLIENT .LIBELLE,
                            LIBELLERELANCE = _LeCLIENT.RELANCE .LIBELLE,
                            LIBELLENATIONALITE = _LeCLIENT.NATIONALITE .LIBELLE,
                            LIBELLEPAYEUR = _LeCLIENT.PAYEUR1.NOM ,
                            LIBELLEREGCLI = _LeCLIENT.REGROUPEMENT1.NOM,
                            LIBELLESITE = _LeCLIENT.CENTRE1.SITE.LIBELLE ,
                            LIBELLECENTRE = _LeCLIENT.CENTRE1.LIBELLE,
                            LIBELLETYPEPIECE = _LeCLIENT.PIECEIDENTITE.LIBELLE,
                            LIBELLEUSAGE = _LeCLIENT.USAGE.LIBELLE,
                        };
                        return Galatee.Tools.Utility.ListToDataTable(query);
                    
                   }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable RetourneAdministration(int fk_idclient)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = null;
                    var _ADMINISTRATION = context.ADMINISTRATION_INSTITUT;
                    query =
                    from x in _ADMINISTRATION
                    where
                      x.FK_IDCLIENT == fk_idclient
                    select new
                    {
                        x.PK_ID,
                        x.NOMMANDATAIRE,
                        x.PRENOMMANDATAIRE,
                        x.RANGMANDATAIRE,
                        x.NOMSIGNATAIRE,
                        x.PRENOMSIGNATAIRE,
                        x.RANGSIGNATAIRE,
                        x.FK_IDCLIENT ,
                        x.NOMABON
                    };

                    return Galatee.Tools.Utility.ListToDataTable(query);
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable RetourneSociete(int fk_idclient)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = null;
                    var _SOCIETE = context.SOCIETEPRIVE;
                    query =
                    from x in _SOCIETE
                    where
                      x.FK_IDCLIENT == fk_idclient
                    select new
                    {
                        x.PK_ID,
                        x.NUMEROREGISTRECOMMERCE,
                        x.FK_IDSTATUTJURIQUE,
                        x.CAPITAL,
                        x.IDENTIFICATIONFISCALE,
                        x.DATECREATION,
                        x.SIEGE,
                        x.FK_IDCLIENT ,
                        x.NOMMANDATAIRE,
                        x.PRENOMMANDATAIRE,
                        x.RANGMANDATAIRE,
                        x.NOMSIGNATAIRE,
                        x.PRENOMSIGNATAIRE,
                        x.RANGSIGNATAIRE,
                        x.NOMABON,
                        LIBELLESTATUSJURIDIQUE = x.STATUTJURIQUE.LIBELLE
                    };

                    return Galatee.Tools.Utility.ListToDataTable(query);
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable RetournePersonnePhysique(int fk_idclient)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = null;
                    var _PERSONNEPHYSIQUE = context.PERSONNEPHYSIQUE;
                    query =
                    from x in _PERSONNEPHYSIQUE
                    where
                      x.FK_IDCLIENT == fk_idclient
                    select new
                    {
                        x.PK_ID,
                        x.DATENAISSANCE,
                        x.NUMEROPIECEIDENTITE,
                        x.DATEFINVALIDITE,
                        x.FK_IDCLIENT ,
                        x.FK_IDPIECEIDENTITE,
                        x.NOMABON
                    };
                    return Galatee.Tools.Utility.ListToDataTable(query);
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable RetourneinfoPropriotaire(int fk_idclient)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = null;
                    var _INFOPROPIOTAIRE = context.INFOPROPRIETAIRE;
                    query =
                    from x in _INFOPROPIOTAIRE
                    where
                      x.FK_IDCLIENT == fk_idclient
                    select new
                    {
                        x.PK_ID,
                        x.NOM,
                        x.PRENOM,
                        x.DATENAISSANCE,
                        x.NUMEROPIECEIDENTITE,
                        x.DATEFINVALIDITE,
                        x.FK_IDCLIENT ,
                        x.FK_IDPIECEIDENTITE,
                        x.FK_IDNATIONNALITE,
                        x.FAX,
                        x.BOITEPOSTALE,
                        x.EMAIL,
                        x.TELEPHONEMOBILE,
                        x.TELEPHONEFIXE,
                        LIBELLEPIECE = x.PIECEIDENTITE.LIBELLE,
                        LIBELLENATIONNALITE = x.NATIONALITE.LIBELLE,
                    };
                    return Galatee.Tools.Utility.ListToDataTable(query);
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public static DataTable RetourneAG(int fk_idcentre,string pCentre, string pClient, string pOrdre)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = null;
                    var _AG = context.AG ;
                        query =
                        from _LeAG in _AG
                        from _Leclient in _LeAG.CLIENT1 
                        where

                        _Leclient.FK_IDCENTRE  == fk_idcentre &&
                        _Leclient.CENTRE == pCentre &&
                        _Leclient.REFCLIENT == pClient &&
                        (_Leclient.ORDRE == pOrdre || string.IsNullOrEmpty(pOrdre))
                        select new
                        {
                            _LeAG.CENTRE,
                            _LeAG.CLIENT,
                            _LeAG.NOMP,
                            _Leclient.ORDRE,
                             COMMUNE=    _LeAG.COMMUNE  ,
                             QUARTIER=    _LeAG.QUARTIER  ,
                             CODERUE = _LeAG.RUES.CODE,
                             RUE = _LeAG.RUE,
                            _LeAG.ETAGE,
                            _LeAG.PORTE,
                            _LeAG.CADR,
                            _LeAG.REGROU,
                            _LeAG.CPARC,
                             TOURNEE=    _LeAG.TOURNEE1.CODE ,
                            _LeAG.ORDTOUR,
                            _LeAG.SECTEUR,
                            _LeAG.CPOS,
                            _LeAG.TELEPHONE,
                            _LeAG.FAX,
                            _LeAG.EMAIL,
                            _LeAG.USERCREATION,
                            _LeAG.DATECREATION,
                            _LeAG.DATEMODIFICATION,
                            _LeAG.USERMODIFICATION,
                            _LeAG.PK_ID,
                            _LeAG.FK_IDTOURNEE,
                            _LeAG.FK_IDQUARTIER,
                            _LeAG.FK_IDCOMMUNE,
                            _LeAG.FK_IDRUE,
                            _LeAG.FK_IDCENTRE ,


                            LIBELLECOMMUNE = _LeAG.COMMUNE1.LIBELLE,
                            LIBELLEQUARTIER = _LeAG.QUARTIER1 .LIBELLE,
                            LIBELLESECTEUR = _LeAG.SECTEUR1 .LIBELLE,
                            LIBELLERUE = _LeAG.RUES.LIBELLE,
                            LIBELLETOURNEE = _LeAG.TOURNEE1 .LIBELLE,

                        };
                    return Galatee.Tools.Utility.ListToDataTable(query);
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable RetourneCanalisationClasseur(int fk_idcentre, string pCentre, string pClient, string pProduit, int? pPoint)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    string point = pPoint == null ? string.Empty : pPoint.ToString();

                    IEnumerable<object> query = null;
                    var _CANALISATION = context.CANALISATION ;
                    query =
                        (from _LeCANALISATION in _CANALISATION
                    where
                    _LeCANALISATION.CENTRE == pCentre &&
                          _LeCANALISATION.CLIENT == pClient &&
                          _LeCANALISATION.FK_IDCENTRE == fk_idcentre &&
                    (string.IsNullOrEmpty(pProduit) || _LeCANALISATION .PRODUIT == pProduit) &&
                    (string.IsNullOrEmpty(point) || _LeCANALISATION.POINT == pPoint)
                    select new
                    {
                        _LeCANALISATION.CENTRE,
                        _LeCANALISATION.CLIENT,
                        _LeCANALISATION.PRODUIT,
                        _LeCANALISATION.PROPRIO,
                        _LeCANALISATION.POINT,
                        _LeCANALISATION.BRANCHEMENT,
                         MARQUE= _LeCANALISATION.COMPTEUR.MARQUE,
                         TYPECOMPTEUR = _LeCANALISATION.COMPTEUR.TYPECOMPTEUR,
                         NUMERO = _LeCANALISATION.COMPTEUR.NUMERO,
                         TCOMPT=   _LeCANALISATION.COMPTEUR.TYPECOMPTEUR1 .CODE  ,
                         MCOMPT = _LeCANALISATION.COMPTEUR.MARQUECOMPTEUR.CODE,
                         _LeCANALISATION.REGLAGECOMPTEUR,

                        _LeCANALISATION.COMPTEUR.COEFLECT,
                        _LeCANALISATION.COMPTEUR.CADRAN ,
                        _LeCANALISATION.COMPTEUR.ANNEEFAB,
                        _LeCANALISATION.SURFACTURATION,
                        _LeCANALISATION.DEBITANNUEL,
                        _LeCANALISATION.DEPOSE,
                        _LeCANALISATION.POSE,
                        _LeCANALISATION.USERCREATION,
                        _LeCANALISATION.DATECREATION,
                        _LeCANALISATION.DATEMODIFICATION,
                        _LeCANALISATION.USERMODIFICATION,
                        _LeCANALISATION.PK_ID,
                        _LeCANALISATION.FK_IDREGLAGECOMPTEUR ,
                        _LeCANALISATION.COMPTEUR.FK_IDTYPECOMPTEUR ,
                        _LeCANALISATION.COMPTEUR.FK_IDPRODUIT,
                        _LeCANALISATION.COMPTEUR.FK_IDMARQUECOMPTEUR,
                        _LeCANALISATION.FK_IDABON,
                        _LeCANALISATION.FK_IDCENTRE,
                        _LeCANALISATION.FK_IDCOMPTEUR,
                        _LeCANALISATION.FK_IDPROPRIETAIRE,

                        _LeCANALISATION.COMPTEUR.FK_IDETATCOMPTEUR,
                        _LeCANALISATION.COMPTEUR.FK_IDSTATUTCOMPTEUR ,
                        _LeCANALISATION.COMPTEUR.FK_IDCALIBRE ,


                         LIBELLECENTRE=  _LeCANALISATION.CENTRE1.LIBELLE ,
                         LIBELLEPRODUIT= _LeCANALISATION.PRODUIT1.LIBELLE ,
                         LIBELLEREGLAGECOMPTEUR = _LeCANALISATION.REGLAGECOMPTEUR1 .LIBELLE,
                         LIBELLEMARQUE = _LeCANALISATION.COMPTEUR.MARQUECOMPTEUR.LIBELLE,
                         LIBELLETYPECOMPTEUR = _LeCANALISATION.COMPTEUR.TYPECOMPTEUR1.LIBELLE ,
                         //LIBELLETYPECOMPTAGE = _LeCANALISATION.COMPTEUR.TYPECOMPTAGE1.LIBELLE ,
                         
                    }).OrderByDescending(t=>t.POSE);
                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static DataTable RetourneCanalisationResilier(int fk_idcentre, string pCentre, string pClient, string pProduit)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {

                    IEnumerable<object> query = null;
                    var _CANALISATION = context.CANALISATION;
                    query =
                        (from _LeCANALISATION in _CANALISATION
                        where
                        _LeCANALISATION.CENTRE == pCentre &&
                              _LeCANALISATION.CLIENT == pClient &&
                              _LeCANALISATION.FK_IDCENTRE == fk_idcentre &&
                              _LeCANALISATION.DEPOSE != null &&
                        (string.IsNullOrEmpty(pProduit) || _LeCANALISATION.PRODUIT == pProduit) 
                        select new
                        {
                            _LeCANALISATION.CENTRE,
                            _LeCANALISATION.CLIENT,
                            _LeCANALISATION.PRODUIT,
                            _LeCANALISATION.PROPRIO,
                            _LeCANALISATION.POINT,
                            _LeCANALISATION.BRANCHEMENT,
                            MARQUE = _LeCANALISATION.COMPTEUR.MARQUE,
                            TYPECOMPTEUR = _LeCANALISATION.COMPTEUR.TYPECOMPTEUR,
                            NUMERO = _LeCANALISATION.COMPTEUR.NUMERO,
                            TCOMPT = _LeCANALISATION.COMPTEUR.TYPECOMPTEUR1.CODE,
                            MCOMPT = _LeCANALISATION.COMPTEUR.MARQUECOMPTEUR.CODE,
                            _LeCANALISATION.REGLAGECOMPTEUR,

                            _LeCANALISATION.COMPTEUR.COEFLECT,
                            _LeCANALISATION.COMPTEUR.CADRAN,
                            _LeCANALISATION.COMPTEUR.ANNEEFAB,
                            _LeCANALISATION.SURFACTURATION,
                            _LeCANALISATION.DEBITANNUEL,
                            _LeCANALISATION.DEPOSE,
                            _LeCANALISATION.POSE,
                            _LeCANALISATION.USERCREATION,
                            _LeCANALISATION.DATECREATION,
                            _LeCANALISATION.DATEMODIFICATION,
                            _LeCANALISATION.USERMODIFICATION,
                            _LeCANALISATION.PK_ID,
                            _LeCANALISATION.FK_IDREGLAGECOMPTEUR,
                            _LeCANALISATION.COMPTEUR.FK_IDTYPECOMPTEUR,
                            _LeCANALISATION.COMPTEUR.FK_IDPRODUIT,
                            _LeCANALISATION.COMPTEUR.FK_IDMARQUECOMPTEUR,
                            _LeCANALISATION.FK_IDABON,
                            _LeCANALISATION.FK_IDCENTRE,
                            _LeCANALISATION.FK_IDCOMPTEUR,
                            _LeCANALISATION.FK_IDPROPRIETAIRE,

                            _LeCANALISATION.COMPTEUR.FK_IDETATCOMPTEUR,
                            _LeCANALISATION.COMPTEUR.FK_IDSTATUTCOMPTEUR,
                            _LeCANALISATION.COMPTEUR.FK_IDCALIBRE,


                            LIBELLECENTRE = _LeCANALISATION.CENTRE1.LIBELLE,
                            LIBELLEPRODUIT = _LeCANALISATION.PRODUIT1.LIBELLE,
                            LIBELLEREGLAGECOMPTEUR = _LeCANALISATION.REGLAGECOMPTEUR1.LIBELLE,
                            LIBELLEMARQUE = _LeCANALISATION.COMPTEUR.MARQUECOMPTEUR.LIBELLE,
                            LIBELLETYPECOMPTEUR = _LeCANALISATION.COMPTEUR.TYPECOMPTEUR1.LIBELLE,
                            //LIBELLETYPECOMPTAGE = _LeCANALISATION.COMPTEUR.TYPECOMPTAGE1.LIBELLE ,

                        }).OrderByDescending(u=>u.DEPOSE);
                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static DataTable RetourneCanalisation(int fk_idcentre,string pCentre, string pClient, string pProduit, int? pPoint)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    string point = pPoint == null ? string.Empty : pPoint.ToString();

                    IEnumerable<object> query = null;
                    var _CANALISATION = context.CANALISATION ;
                    query =
                        from _LeCANALISATION in _CANALISATION
                    where
                    _LeCANALISATION.CENTRE == pCentre &&
                          _LeCANALISATION.CLIENT == pClient &&
                          _LeCANALISATION.FK_IDCENTRE == fk_idcentre &&
                          _LeCANALISATION.DEPOSE == null  &&
                    (string.IsNullOrEmpty(pProduit) || _LeCANALISATION .PRODUIT == pProduit) &&
                    (string.IsNullOrEmpty(point) || _LeCANALISATION.POINT == pPoint)
                    select new
                    {
                        _LeCANALISATION.CENTRE,
                        _LeCANALISATION.CLIENT,
                        _LeCANALISATION.PRODUIT,
                        _LeCANALISATION.PROPRIO,
                        _LeCANALISATION.POINT,
                        _LeCANALISATION.BRANCHEMENT,
                         MARQUE= _LeCANALISATION.COMPTEUR.MARQUE,
                         TYPECOMPTEUR = _LeCANALISATION.COMPTEUR.TYPECOMPTEUR,
                         NUMERO = _LeCANALISATION.COMPTEUR.NUMERO,
                         TCOMPT=   _LeCANALISATION.COMPTEUR.TYPECOMPTEUR1 .CODE  ,
                         MCOMPT = _LeCANALISATION.COMPTEUR.MARQUECOMPTEUR.CODE,
                         CODECALIBRECOMPTEUR = _LeCANALISATION.COMPTEUR.CALIBRECOMPTEUR.LIBELLE,
                         _LeCANALISATION.REGLAGECOMPTEUR,

                        _LeCANALISATION.COMPTEUR.COEFLECT,
                        _LeCANALISATION.COMPTEUR.CADRAN ,
                        _LeCANALISATION.COMPTEUR.ANNEEFAB,
                        _LeCANALISATION.SURFACTURATION,
                        _LeCANALISATION.DEBITANNUEL,
                        _LeCANALISATION.DEPOSE,
                        _LeCANALISATION.POSE,
                        _LeCANALISATION.USERCREATION,
                        _LeCANALISATION.DATECREATION,
                        _LeCANALISATION.DATEMODIFICATION,
                        _LeCANALISATION.USERMODIFICATION,
                        _LeCANALISATION.PK_ID,
                        _LeCANALISATION.FK_IDREGLAGECOMPTEUR ,
                        _LeCANALISATION.COMPTEUR.FK_IDTYPECOMPTEUR ,
                        _LeCANALISATION.COMPTEUR.FK_IDPRODUIT,
                        _LeCANALISATION.COMPTEUR.FK_IDMARQUECOMPTEUR,
                         ETATCOMPTEUR= _LeCANALISATION.COMPTEUR.ETATCOMPTEUR.CODE  ,

                        _LeCANALISATION.FK_IDABON,
                        _LeCANALISATION.FK_IDCENTRE,
                        _LeCANALISATION.FK_IDCOMPTEUR,
                        _LeCANALISATION.FK_IDPROPRIETAIRE,

                        _LeCANALISATION.ABON.FK_IDTYPECOMPTAGE ,
                        LIBELLETYPECOMPTAGE = _LeCANALISATION.ABON.TYPECOMPTAGE1.LIBELLE,
                        TYPECOMPTAGE = _LeCANALISATION.ABON.TYPECOMPTAGE ,  

                        _LeCANALISATION.COMPTEUR.FK_IDETATCOMPTEUR,
                        _LeCANALISATION.COMPTEUR.FK_IDSTATUTCOMPTEUR ,
                        _LeCANALISATION.COMPTEUR.FK_IDCALIBRE ,


                         LIBELLECENTRE = _LeCANALISATION.CENTRE1.LIBELLE,
                         LIBELLEPRODUIT= _LeCANALISATION.PRODUIT1.LIBELLE ,
                         LIBELLEREGLAGECOMPTEUR = _LeCANALISATION.REGLAGECOMPTEUR1 .LIBELLE,
                         LIBELLEMARQUE = _LeCANALISATION.COMPTEUR.MARQUECOMPTEUR.LIBELLE,
                         LIBELLETYPECOMPTEUR = _LeCANALISATION.COMPTEUR.TYPECOMPTEUR1.LIBELLE  
                         
                    };
                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static DataTable RetourneCanalisation( string pCentre, string pClient, string pProduit, int? pPoint)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    string point = pPoint == null ? string.Empty : pPoint.ToString();

                    IEnumerable<object> query = null;
                    var _CANALISATION = context.CANALISATION;
                    query =
                        from _LeCANALISATION in _CANALISATION
                        where
                        _LeCANALISATION.CENTRE == pCentre &&
                              _LeCANALISATION.CLIENT == pClient &&
                             
                              _LeCANALISATION.DEPOSE == null &&
                        (string.IsNullOrEmpty(pProduit) || _LeCANALISATION.PRODUIT == pProduit) &&
                        (string.IsNullOrEmpty(point) || _LeCANALISATION.POINT == pPoint)
                        select new
                        {
                            _LeCANALISATION.CENTRE,
                            _LeCANALISATION.CLIENT,
                            _LeCANALISATION.PRODUIT,
                            _LeCANALISATION.PROPRIO,
                            _LeCANALISATION.POINT,
                            _LeCANALISATION.BRANCHEMENT,
                            MARQUE = _LeCANALISATION.COMPTEUR.MARQUE,
                            TYPECOMPTEUR = _LeCANALISATION.COMPTEUR.TYPECOMPTEUR,
                            NUMERO = _LeCANALISATION.COMPTEUR.NUMERO,
                            TCOMPT = _LeCANALISATION.COMPTEUR.TYPECOMPTEUR1.CODE,
                            MCOMPT = _LeCANALISATION.COMPTEUR.MARQUECOMPTEUR.CODE,
                            _LeCANALISATION.REGLAGECOMPTEUR,

                            _LeCANALISATION.COMPTEUR.COEFLECT,
                            _LeCANALISATION.COMPTEUR.CADRAN,
                            _LeCANALISATION.COMPTEUR.ANNEEFAB,
                            _LeCANALISATION.SURFACTURATION,
                            _LeCANALISATION.DEBITANNUEL,
                            _LeCANALISATION.DEPOSE,
                            _LeCANALISATION.POSE,
                            _LeCANALISATION.USERCREATION,
                            _LeCANALISATION.DATECREATION,
                            _LeCANALISATION.DATEMODIFICATION,
                            _LeCANALISATION.USERMODIFICATION,
                            _LeCANALISATION.PK_ID,
                            _LeCANALISATION.FK_IDREGLAGECOMPTEUR,
                            _LeCANALISATION.COMPTEUR.FK_IDTYPECOMPTEUR,
                            _LeCANALISATION.COMPTEUR.FK_IDPRODUIT,
                            _LeCANALISATION.COMPTEUR.FK_IDMARQUECOMPTEUR,
                            _LeCANALISATION.FK_IDABON,
                            _LeCANALISATION.FK_IDCENTRE,
                            _LeCANALISATION.FK_IDCOMPTEUR,
                            _LeCANALISATION.FK_IDPROPRIETAIRE,

                            _LeCANALISATION.COMPTEUR.FK_IDETATCOMPTEUR,
                            _LeCANALISATION.COMPTEUR.FK_IDSTATUTCOMPTEUR,
                            _LeCANALISATION.COMPTEUR.FK_IDCALIBRE,


                            LIBELLECENTRE = _LeCANALISATION.CENTRE1.LIBELLE,
                            LIBELLEPRODUIT = _LeCANALISATION.PRODUIT1.LIBELLE,
                            LIBELLEREGLAGECOMPTEUR = _LeCANALISATION.REGLAGECOMPTEUR1.LIBELLE,
                            LIBELLEMARQUE = _LeCANALISATION.COMPTEUR.MARQUECOMPTEUR.LIBELLE,
                            LIBELLETYPECOMPTEUR = _LeCANALISATION.COMPTEUR.TYPECOMPTEUR1.LIBELLE,
                            //LIBELLETYPECOMPTAGE = _LeCANALISATION.COMPTEUR.TYPECOMPTAGE1.LIBELLE ,

                        };
                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
      
        
        
        public static DataTable RetourneEvenement(int fk_idcentre, string pCentre, string pClient, string pOrdre, string pProduit, int? pPoint, int? pEvenement)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = null;
                    var _EVENEMENT = context.EVENEMENT;

                    query =
                    (from _LeEVENEMENT in _EVENEMENT
                    where _LeEVENEMENT.CENTRE  == pCentre &&
                          _LeEVENEMENT.CLIENT  == pClient &&
                          _LeEVENEMENT.ORDRE == pOrdre &&
                          _LeEVENEMENT.FK_IDCENTRE  == fk_idcentre  &&
                          (string.IsNullOrEmpty(pProduit) || _LeEVENEMENT.PRODUIT  == pProduit) &&
                          (pPoint == null   || _LeEVENEMENT.POINT == pPoint) &&
                          (pEvenement == null  || _LeEVENEMENT.NUMEVENEMENT == pEvenement)
                    select new
                    {
                        _LeEVENEMENT.CENTRE,
                        _LeEVENEMENT.CLIENT,
                        _LeEVENEMENT.PRODUIT,
                        _LeEVENEMENT.POINT,
                        _LeEVENEMENT.NUMEVENEMENT,
                        _LeEVENEMENT.ORDRE,
                        _LeEVENEMENT.COMPTEUR,
                        _LeEVENEMENT.DATEEVT,
                        _LeEVENEMENT.PERIODE,
                        _LeEVENEMENT.CODEEVT,
                        _LeEVENEMENT.INDEXEVT,
                        _LeEVENEMENT.CAS,
                        _LeEVENEMENT.ENQUETE,
                        _LeEVENEMENT.CONSO,
                        _LeEVENEMENT.CONSONONFACTUREE,
                        _LeEVENEMENT.LOTRI,
                        _LeEVENEMENT.FACTURE,
                        _LeEVENEMENT.SURFACTURATION,
                        _LeEVENEMENT.STATUS,
                        _LeEVENEMENT.TYPECONSO,
                        _LeEVENEMENT.REGLAGECOMPTEUR,
                        _LeEVENEMENT.TYPETARIF,
                        _LeEVENEMENT.FORFAIT,
                        _LeEVENEMENT.CATEGORIE,
                        _LeEVENEMENT.CODECONSO,
                        _LeEVENEMENT.PROPRIO,
                        _LeEVENEMENT.MODEPAIEMENT,
                        _LeEVENEMENT.MATRICULE,
                        _LeEVENEMENT.FACPER,
                        _LeEVENEMENT.QTEAREG,
                        _LeEVENEMENT.DERPERF,
                        _LeEVENEMENT.DERPERFN,
                        _LeEVENEMENT.CONSOFAC,
                        _LeEVENEMENT.REGIMPUTE,
                        _LeEVENEMENT.REGCONSO,
                        _LeEVENEMENT.COEFLECT,
                        _LeEVENEMENT.COEFCOMPTAGE,
                        _LeEVENEMENT.PUISSANCE,
                        _LeEVENEMENT.TYPECOMPTAGE,
                        _LeEVENEMENT.TYPECOMPTEUR ,
                        _LeEVENEMENT.COEFK1,
                        _LeEVENEMENT.COEFK2,
                        _LeEVENEMENT.COEFFAC,
                        _LeEVENEMENT.USERCREATION,
                        _LeEVENEMENT.DATECREATION,
                        _LeEVENEMENT.DATEMODIFICATION,
                        _LeEVENEMENT.USERMODIFICATION,
                        _LeEVENEMENT.FK_IDABON,
                        _LeEVENEMENT.PK_ID,
                        PK_IDEVENT = _LeEVENEMENT.PK_ID,
                        //_LeEVENEMENT.FK_IDCAS,
                        _LeEVENEMENT.FK_IDCENTRE,
                        _LeEVENEMENT.FK_IDPRODUIT ,
                        _LeEVENEMENT.FK_IDCANALISATION,
                        _LeEVENEMENT.FK_IDCOMPTEUR 
                    });
                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static DataTable RetourneBRT(int fk_idcentre, string pCentre, string pClient, string pProduit)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = null;
                    var brt = context.BRT;

                    query =
                    from _LeBRT in brt
                    where _LeBRT.CENTRE == pCentre &&
                          _LeBRT.CLIENT  == pClient &&
                          _LeBRT.FK_IDCENTRE == fk_idcentre &&
                          (string.IsNullOrEmpty(pProduit) || _LeBRT.PRODUIT == pProduit)
                    select new
                    {
                        _LeBRT.DIAMBRT,
                        _LeBRT.CENTRE,
                        _LeBRT.CLIENT ,
                        _LeBRT.PRODUIT    ,
                        _LeBRT. DRAC  ,
                        _LeBRT. DRES  ,
                        _LeBRT. SERVICE  ,
                        _LeBRT. CATBRT  ,
                        _LeBRT. LONGBRT  ,
                        _LeBRT. NATBRT  ,
                        _LeBRT. NBPOINT  ,
                        _LeBRT. RESEAU  ,
                        _LeBRT. TRONCON  ,
                        _LeBRT. DMAJ  ,
                        _LeBRT. PUISSANCEINSTALLEE  ,
                        _LeBRT. PERTES  ,
                        _LeBRT. COEFPERTES  ,
                        _LeBRT. APPTRANSFO  ,
                        _LeBRT. CODEBRT  ,
                        _LeBRT. CODEPOSTE  ,
                        _LeBRT.NOMBRETRANSFORMATEUR   ,
                        _LeBRT. ANFAB  ,
                        _LeBRT. LONGITUDE  ,
                        _LeBRT. LATITUDE  ,
                        _LeBRT.ADRESSERESEAU,
                        _LeBRT. USERCREATION  ,
                        _LeBRT. DATECREATION  ,
                        _LeBRT. DATEMODIFICATION  ,
                        _LeBRT. USERMODIFICATION  ,
                        _LeBRT. PK_ID  ,
                        _LeBRT. FK_IDCENTRE  ,
                        _LeBRT. FK_IDPRODUIT  ,
                        _LeBRT.FK_IDTYPEBRANCHEMENT   ,
                        _LeBRT.FK_IDAG ,
                        _LeBRT.FK_IDPOSTESOURCE,
                        CODEPOSTESOURCE = _LeBRT.POSTESOURCE.CODE,
                        LIBELLEPOSTESOURCE = _LeBRT.POSTESOURCE.LIBELLE,

                        _LeBRT.FK_IDDEPARTHTA,
                        CODEDEPARTHTA = _LeBRT.DEPARTHTA.CODE,
                        LIBELLEDEPARTHTA = _LeBRT.DEPARTHTA.LIBELLE,

                        _LeBRT.FK_IDPOSTETRANSFORMATION,
                        CODETRANSFORMATEUR = _LeBRT.POSTETRANSFORMATION.CODE,
                        LIBELLETRANSFORMATEUR = _LeBRT.POSTETRANSFORMATION.LIBELLE,

                        _LeBRT.FK_IDQUARTIER,
                        CODEQUARTIER = _LeBRT.QUARTIER.CODE,
                        LIBELLEQUARTIER = _LeBRT.QUARTIER.LIBELLE,

                        _LeBRT.FK_IDDEPARTBT,
                        CODEDEPARTBT = _LeBRT.DEPARTBT.CODE,
                        LIBELLEDEPARTBT = _LeBRT.DEPARTBT.LIBELLE,

                        _LeBRT.NEOUDFINAL, 


                        LIBELLEPRODUIT = _LeBRT.PRODUIT1.LIBELLE,
                        LIBELLECENTRE = _LeBRT.CENTRE1 .LIBELLE,
                        //LIBELLETOURNEE = _LeBRT.TOURNEE1 .LIBELLE,
                        LIBELLETYPEBRANCHEMENT = _LeBRT.TYPEBRANCHEMENT.LIBELLE,
                        CODETYPEBRANCHEMENT =_LeBRT.TYPEBRANCHEMENT.CODE 
                    };
                    return Galatee.Tools.Utility.ListToDataTable(query);
                };

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static DataTable RetourneScellage(int fk_idbrt)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = null;
                    var scellage = context.POSE_SCELLE_DEMANDE ;

                    query =
                    from x in scellage
                    where x.FK_IDBRT == fk_idbrt 
                    select new
                    {
                        x.PK_ID,
                        x.NUM_SCELLE,
                        x.FK_IDORGANE_SCELLABLE,
                        x.FK_IDDEMANDE,
                        x.FK_IDBRT,
                        x.NOMBRE,
                        x.DATECREATION,
                        x.USERCREATION,
                        x.CERTIFICAT ,
                        LIBELLEORGANE_SCELLABLE = x.ORGANE_SCELLABLE.LIBELLE 
                    };
                    return Galatee.Tools.Utility.ListToDataTable(query);
                };

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

       public static DataTable RetourneScellageCompteur(string numero,string marque)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = null;
                    var scellage = context.TbCompteurBTA  ;

                    query =
                    from x in scellage
                    where x.Numero_Compteur == numero && x.MARQUE == marque  
                    select new
                    {
                     CapotMoteur_ID_Scelle1 = x.Scelles1.Numero_Scelle ,
                     CapotMoteur_ID_Scelle2 =  x.Scelles2.Numero_Scelle ,
                     CapotMoteur_ID_Scelle3 =  x.Scelles3 .Numero_Scelle ,
                     Cache_Scelle = x.Scelles .Numero_Scelle 
                    };
                    return Galatee.Tools.Utility.ListToDataTable(query);
                };

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        

        public static CLIENT RetourneCompteClient(int pk_Id)
        {
            try
            {
                galadbEntities c = new galadbEntities();
                var query_ = (from d in c.CLIENT
                              where (d.PK_ID == pk_Id)
                              select d).FirstOrDefault();
                return (CLIENT)query_;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //public static DataTable RetournePagisol(string pCentre, string pLotri, string pProduit)
        //{
        //    try
        //    {
        //        using (galadbEntities context = new galadbEntities())
        //        {
        //            IEnumerable<object> query = null;
        //            var _PAGISOL = context.PAGISOL;
        //            string produit = string.IsNullOrEmpty(pProduit) ? "" : pProduit;
        //            string Lotri = string.IsNullOrEmpty(pLotri) ? "" : pLotri;
        //            string Centre = string.IsNullOrEmpty(pCentre) ? "" : pCentre;
        //            string vide = "";

        //            query =
        //            from _LePAGISOL in _PAGISOL
        //            where (_LePAGISOL.CENTRE  == pCentre || Centre == vide) &&
        //                  (_LePAGISOL.LOTRI  == pLotri || Lotri == vide) &&
        //                  (_LePAGISOL.LOTRI  == pProduit || produit == vide)
        //            select new
        //            {
        //                _LePAGISOL.LOTRI,
        //                _LePAGISOL.CENTRE,
        //                _LePAGISOL.TOURNEE,
        //                _LePAGISOL.ORDTOUR,
        //                _LePAGISOL.CLIENT,
        //                _LePAGISOL.PRODUIT,
        //             LIBELLEPRODUIT=   _LePAGISOL.PRODUIT1.LIBELLE ,
        //                _LePAGISOL.POINT,
        //                _LePAGISOL.CAS,
        //                _LePAGISOL.STATUT,
        //                _LePAGISOL.TOPEDIT,
        //                _LePAGISOL.CATEGORIECLIENT,
        //                _LePAGISOL.FREQUENCE,
        //                _LePAGISOL.TFAC,
        //                _LePAGISOL.DDEB,
        //                _LePAGISOL.DFIN,
        //                _LePAGISOL.QTEFAC,
        //                _LePAGISOL.AIED,
        //                _LePAGISOL.NIED,
        //                _LePAGISOL.PERFAC,
        //                _LePAGISOL.FACTURE,
        //                _LePAGISOL.PK_ID,
        //                _LePAGISOL.FK_IDEVENEMENT,
        //                _LePAGISOL.DATECREATION,
        //                _LePAGISOL.DATEMODIFICATION,
        //                _LePAGISOL.USERCREATION,
        //                _LePAGISOL.USERMODIFICATION,
        //                _LePAGISOL.FK_IDPRODUIT,
        //                _LePAGISOL.FK_IDCENTRE,
        //                _LePAGISOL.FK_IDCATEGORIECLIENT 
        //            };
        //            return Galatee.Tools.Utility.ListToDataTable(query);
        //        };

        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public static string  RetourneReglementDevis(string centre,string Client,string Ordre)
        {
            try
            {
                string RefemNdoc = string .Empty ;
                using (galadbEntities context = new galadbEntities())
                {
                    TRANSCAISSE  _LaReglementTranscaisseDevis = context.TRANSCAISSE .FirstOrDefault (x=> x.CENTRE   == centre && x.CLIENT  == Client && x.ORDRE == Ordre && x.COPER == Enumere.CoperOdQPA ) ;
                    if (_LaReglementTranscaisseDevis != null && !string.IsNullOrEmpty(_LaReglementTranscaisseDevis.CENTRE) && 
                        !string.IsNullOrEmpty(_LaReglementTranscaisseDevis.CLIENT ))
                        RefemNdoc = _LaReglementTranscaisseDevis.REFEM + ";" + _LaReglementTranscaisseDevis.NDOC;
                    else
                    {
                        TRANSCAISB _LaReglementTranscaisBDevis = context.TRANSCAISB .FirstOrDefault(x => x.CENTRE == centre && x.CLIENT == Client && x.ORDRE == Ordre && x.COPER == Enumere.CoperOdQPA);
                        if (_LaReglementTranscaisBDevis != null && !string.IsNullOrEmpty(_LaReglementTranscaisBDevis.CENTRE) &&
                        !string.IsNullOrEmpty(_LaReglementTranscaisBDevis.CLIENT))
                            RefemNdoc = _LaReglementTranscaisBDevis.REFEM + ";" + _LaReglementTranscaisBDevis.NDOC;
                    }
                }
                return RefemNdoc;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static DataTable RetourneModep(string pModep)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    string Modep = !string.IsNullOrEmpty(pModep) ? pModep : "";
                    string vide = "";

                    var _MODEPAIEMENT = context.MODEPAIEMENT;
                    IEnumerable<object> query = 
                    from _TMODEPAIEMENT in _MODEPAIEMENT
                    where _TMODEPAIEMENT.CODE  == pModep || pModep.Equals(vide)
                    select new
                    {
                        _TMODEPAIEMENT.CODE  ,
                        _TMODEPAIEMENT.LIBELLE,
                        _TMODEPAIEMENT.USERCREATION,
                        _TMODEPAIEMENT.USERMODIFICATION,
                        _TMODEPAIEMENT.DATECREATION,
                        _TMODEPAIEMENT.DATEMODIFICATION
                    };
                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static DataTable RetourneListeCodeRegroupement(string pRegcli)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    string Regcli = !string.IsNullOrEmpty(pRegcli) ? pRegcli : "";
                    string vide = "";

                    var _REGCLI = context.REGROUPEMENT ;
                    IEnumerable<object> query = 
                        from _leREGCLI in _REGCLI
                        where (_leREGCLI.CODE   == pRegcli || Regcli.Equals(vide))
                        select new
                        {
                            _leREGCLI.CODE  ,
                            _leREGCLI.  NOM ,
                            _leREGCLI.  ADRESSE ,
                            _leREGCLI.  DATECREATION ,
                            _leREGCLI. DATEMODIFICATION ,
                            _leREGCLI.  USERCREATION ,
                            _leREGCLI.  USERMODIFICATION ,
                            _leREGCLI.  PK_ID  
                        };
                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static DataTable RetourneToutCadran()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    var _CADRAN = context.CATEGORIECLIENT ;
                    IEnumerable<object> query = null;
                    query =
                    from _LECADRAN in _CADRAN
                    select new
                    {
                        _LECADRAN.PK_ID ,
                        _LECADRAN.CODE ,
                        _LECADRAN.LIBELLE,
                        _LECADRAN.USERCREATION,
                        _LECADRAN.USERMODIFICATION,
                        _LECADRAN.DATECREATION,
                        _LECADRAN.DATEMODIFICATION
                    };
                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DEMANDE RetourneDetailDemande(int pkId_Demande, galadbEntities context)
        {
            try
            {
                DEMANDE _laDemande = context.DEMANDE.FirstOrDefault (p => (p.PK_ID == pkId_Demande)) ;             
                return _laDemande;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static DEMANDE RetourneDetailDemandeByDevis(ObjDEVIS leDevis, galadbEntities context)
        {
            try
            {
                DEMANDE _laDemande = context.DEMANDE.FirstOrDefault(p => (p.CENTRE  == leDevis.CODECENTRE && p.NUMDEM == leDevis.NUMDEVIS  ));
                return _laDemande;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static DataTable RetourneDemandeClientType(string pCentre, string pClient, string pOrdre,int pIdCentre, string pTdem)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = null;
                    var _DEMANDE = context.DEMANDE;
                    query =
                    from p in _DEMANDE
                    where
                           (p.CENTRE == pCentre )
                        && (p.CLIENT == pClient )
                        && (string.IsNullOrEmpty(pOrdre) || p.ORDRE == pOrdre )
                        && (p.TYPEDEMANDE  == pTdem )
                        && (p.FK_IDCENTRE == pIdCentre)
                       
                    select new
                    {
                        p.NUMDEM,
                        p.CENTRE,
                        p.NUMPERE,
                        p.TYPEDEMANDE,
                        p.DPRRDV,
                        p.DPRDEV,
                        p.DPREX,
                        p.DREARDV,
                        p.DREADEV,
                        p.DREAEX,
                        p.HRDVPR,
                        p.FDEM,
                        p.FREP,
                        p.NOMPERE,
                        p.NOMMERE,
                        p.MATRICULE,
                        p.STATUT,
                        p.DCAISSE,
                        p.NCAISSE,
                        p.EXDAG,
                        p.EXDBRT,
                        p.PRODUIT,
                        p.EXCL,
                        p.CLIENT,
                        p.EXCOMPT,
                        p.COMPTEUR,
                        p.EXEVT,
                        p.CTAXEG,
                        p.DATED,
                        p.REFEM,
                        p.ORDRE,
                        p.TOPEDIT,
                        p.FACTURE,
                        p.DATEFLAG,
                        p.USERCREATION,
                        p.DATECREATION,
                        p.DATEMODIFICATION,
                        p.DATEFIN ,
                        p.USERMODIFICATION,
                        p.ETAPEDEMANDE,
                        p.PK_ID,
                        p.FK_IDCENTRE,
                        p.FK_IDADMUTILISATEUR,
                        p.FK_IDTYPEDEMANDE,
                        p.FK_IDPRODUIT,
                        p.STATUTDEMANDE,
                        p.ANNOTATION,
                        p.FICHIERJOINT,
                        p.ISSUPPRIME  
                    };
                    return Galatee.Tools.Utility.ListToDataTable(query);

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public static int? RetourneTypeDemandeByCode(string pTdem)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    var TYPEDEMANDE = context.TYPEDEMANDE.FirstOrDefault(t => t.CODE == pTdem);
                    if (TYPEDEMANDE != null)
                        return TYPEDEMANDE.PK_ID;
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable RetourneDemandeClientTypes(string pCentre, string pClient, string pOrdre, int pIdCentre, string tdem)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = null;
                    var _DEMANDE = context.DEMANDE;
                    var TYPEDEMAND = context.TYPEDEMANDE.FirstOrDefault(t => t.CODE == tdem);
                    query =
                    from p in _DEMANDE
                    where
                           (p.CENTRE == pCentre)
                        && (p.CLIENT == pClient)
                        && (string.IsNullOrEmpty(pOrdre) || p.ORDRE == pOrdre)

                        && (p.FK_IDCENTRE == pIdCentre)

                    select new
                    {
                        p.NUMDEM,
                        p.CENTRE,
                        p.NUMPERE,
                        TYPEDEMANDE = TYPEDEMAND.CODE,
                        p.DPRRDV,
                        p.DPRDEV,
                        p.DPREX,
                        p.DREARDV,
                        p.DREADEV,
                        p.DREAEX,
                        p.HRDVPR,
                        p.FDEM,
                        p.FREP,
                        p.NOMPERE,
                        p.NOMMERE,
                        p.MATRICULE,
                        p.STATUT,
                        p.DCAISSE,
                        p.NCAISSE,
                        p.EXDAG,
                        p.EXDBRT,
                        p.PRODUIT,
                        p.EXCL,
                        p.CLIENT,
                        p.EXCOMPT,
                        p.COMPTEUR,
                        p.EXEVT,
                        p.CTAXEG,
                        p.DATED,
                        p.REFEM,
                        p.ORDRE,
                        p.TOPEDIT,
                        p.FACTURE,
                        p.DATEFLAG,
                        p.USERCREATION,
                        p.DATECREATION,
                        p.DATEMODIFICATION,
                        p.USERMODIFICATION,
                        p.ETAPEDEMANDE,
                        p.PK_ID,
                        p.FK_IDCENTRE,
                        p.FK_IDADMUTILISATEUR,
                        FK_IDTYPEDEMANDE = TYPEDEMAND.PK_ID,
                        p.FK_IDPRODUIT,
                        p.STATUTDEMANDE,
                        p.ANNOTATION,
                        p.FICHIERJOINT,
                        p.ISSUPPRIME
                    };
                    return Galatee.Tools.Utility.ListToDataTable(query);

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable RetourneDemandeClientTypes(string pCentre, string pClient, string pOrdre, int pIdCentre)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = null;
                    var _DEMANDE = context.DEMANDE;
                    query =
                    from p in _DEMANDE
                    where
                           (p.CENTRE == pCentre)
                        && (p.CLIENT == pClient)
                        && (string.IsNullOrEmpty(pOrdre) || p.ORDRE == pOrdre)

                        && (p.FK_IDCENTRE == pIdCentre)

                    select new
                    {
                        p.NUMDEM,
                        p.CENTRE,
                        p.NUMPERE,
                        p.TYPEDEMANDE,
                        p.DPRRDV,
                        p.DPRDEV,
                        p.DPREX,
                        p.DREARDV,
                        p.DREADEV,
                        p.DREAEX,
                        p.HRDVPR,
                        p.FDEM,
                        p.FREP,
                        p.NOMPERE,
                        p.NOMMERE,
                        p.MATRICULE,
                        p.STATUT,
                        p.DCAISSE,
                        p.NCAISSE,
                        p.EXDAG,
                        p.EXDBRT,
                        p.PRODUIT,
                        p.EXCL,
                        p.CLIENT,
                        p.EXCOMPT,
                        p.COMPTEUR,
                        p.EXEVT,
                        p.CTAXEG,
                        p.DATED,
                        p.REFEM,
                        p.ORDRE,
                        p.TOPEDIT,
                        p.FACTURE,
                        p.DATEFLAG,
                        p.USERCREATION,
                        p.DATECREATION,
                        p.DATEMODIFICATION,
                        p.USERMODIFICATION,
                        p.ETAPEDEMANDE,
                        p.FK_IDTYPECLIENT,
                        p.PK_ID,
                        p.FK_IDCENTRE,
                        p.FK_IDADMUTILISATEUR,
                        p.FK_IDTYPEDEMANDE,
                        p.FK_IDPRODUIT,
                        p.STATUTDEMANDE,
                        p.ANNOTATION,
                        p.FICHIERJOINT,
                        p.ISSUPPRIME
                    };
                    return Galatee.Tools.Utility.ListToDataTable(query);

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }






        
        public static DataTable IsDernierEvtEnFacturation(string pCentre, string pClient, string pOrdre)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                   int MaxNumEvt = context.EVENEMENT.Where(t => t.CENTRE == pCentre && t.CLIENT == pClient && t.ORDRE == pOrdre ).Max(p => p.NUMEVENEMENT );

                    IEnumerable<object> query = null;
                    var _EVENEMENT = context.EVENEMENT ;
                    query =
                    from p in _EVENEMENT
                    where
                           (p.CENTRE == pCentre)
                        && (p.CLIENT == pClient)
                        && (p.ORDRE == pOrdre)
                        && (p.NUMEVENEMENT == MaxNumEvt)
                        && (p.STATUS == Enumere.EvenementReleve ||
                            p.STATUS == Enumere.EvenementFacture ||
                            p.STATUS == Enumere.EvenementDefacture)
                    select new
                    {
                        p.  CENTRE ,
                        p.  CLIENT ,
                        p.  PRODUIT ,
                        p.  NUMEVENEMENT ,
                        p.  ORDRE ,
                        p.  COMPTEUR ,
                        p.  DATEEVT ,
                        p.  PERIODE ,
                        p.  CODEEVT ,
                        p.  INDEXEVT ,
                        p.  CAS ,
                        p.  ENQUETE ,
                        p.  CONSO ,
                        p.  CONSONONFACTUREE ,
                        p.  LOTRI ,
                        p.  FACTURE ,
                        p.  SURFACTURATION ,
                        p.  STATUS ,
                        p.  TYPECONSO ,
                        p.REGLAGECOMPTEUR,
                     
                        p.  MATRICULE ,
                        p.  FACPER ,
                        p.  QTEAREG ,
                        p.  DERPERF ,
                        p.  DERPERFN ,
                        p.  CONSOFAC ,
                        p.  REGIMPUTE ,
                        p.  REGCONSO ,
                        p. COEFLECT ,
                        p.  COEFCOMPTAGE ,
                        p. PUISSANCE ,
                        p.  TYPECOMPTAGE ,
                        p. TYPECOMPTEUR  ,
                        p. COEFK1 ,
                        p. COEFK2 ,
                        p.  COEFFAC ,
                        p.  USERCREATION ,
                        p. DATECREATION ,
                        p.  DATEMODIFICATION ,
                        p.  USERMODIFICATION ,
                        p.  PK_ID ,
                        //p.  FK_IDCAS ,
                        p.  FK_IDCENTRE ,
                        p.  FK_IDPRODUIT ,
                        p.  FK_IDCANALISATION 
                    };
                    return Galatee.Tools.Utility.ListToDataTable(query.Take(1));

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static DataTable RetourneListeDemandeClient(int pIdCentre, string pCentre, string pClient, string pOrdre)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = null;
                    var _DEMANDE = context.DEMANDE;
                    query =
                    from p in _DEMANDE
                    where
                           (p.FK_IDCENTRE == pIdCentre )
                        && (p.CENTRE == pCentre || string.IsNullOrEmpty(pCentre))
                        && (p.CLIENT == pClient || string.IsNullOrEmpty(pClient))
                        && (p.ORDRE  == pOrdre || string.IsNullOrEmpty(pOrdre))
                        && (p.ISSUPPRIME == false || p.ISSUPPRIME == null) 
                    select new
                    {
                        p.NUMDEM,
                        p.CENTRE,
                        p.NUMPERE,
                        p.TYPEDEMANDE,
                        p.DPRRDV,
                        p.DPRDEV,
                        p.DPREX,
                        p.DREARDV,
                        p.DREADEV,
                        p.DREAEX,
                        p.HRDVPR,
                        p.FDEM,
                        p.FREP,
                        p.NOMPERE,
                        p.NOMMERE,
                        p.MATRICULE,
                        p.STATUT,
                        p.DCAISSE,
                        p.NCAISSE,
                        p.EXDAG,
                        p.EXDBRT,
                        p.PRODUIT,
                        p.EXCL,
                        p.CLIENT,
                        p.EXCOMPT,
                        p.COMPTEUR,
                        p.EXEVT,
                        p.CTAXEG,
                        p.DATED,
                        p.REFEM,
                        p.ORDRE,
                        p.TOPEDIT,
                        p.FACTURE,
                        p.DATEFLAG,
                        p.USERCREATION,
                        p.DATECREATION,
                        p.DATEMODIFICATION,
                        p.USERMODIFICATION,
                        p.ETAPEDEMANDE,
                        p.PK_ID,
                        p.FK_IDCENTRE,
                        p.FK_IDADMUTILISATEUR,
                        p.FK_IDTYPEDEMANDE,
                        p.FK_IDPRODUIT,
                        p.STATUTDEMANDE,
                        p.ANNOTATION,
                        p.FICHIERJOINT,
                        LIBELLETYPEDEMANDE = p.TYPEDEMANDE1.LIBELLE 
                    };
                    return Galatee.Tools.Utility.ListToDataTable(query);

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static DataTable  RetourneListeDemandeModification(string pCentre, string pNumdem, string pTdem,string pProduit, DateTime? datedemande,string matricule)
        {
            try
            {
                List<string> _lstTypeDemande = new List<string>();
                if (string.IsNullOrEmpty(pTdem))
                {
                    _lstTypeDemande.Add(Enumere.ModificationAbonnement);
                    _lstTypeDemande.Add(Enumere.ModificationAdresse );
                    _lstTypeDemande.Add(Enumere.ModificationBranchement );
                    _lstTypeDemande.Add(Enumere.ModificationClient);
                    _lstTypeDemande.Add(Enumere.ModificationCompteur ); 
                }
                else
                    _lstTypeDemande.Add(pTdem); 


                DateTime? pDatedemande = datedemande == null ? null : datedemande;
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = null;
                    var _DEMANDE = context.DEMANDE;
                    query =
                    from p in _DEMANDE
                    where
                         (p.NUMDEM == pNumdem || string.IsNullOrEmpty(pNumdem))
                        && (p.CENTRE == pCentre || string.IsNullOrEmpty(pCentre))
                        && (_lstTypeDemande.Contains(p.TYPEDEMANDE))
                        && (p.PRODUIT == pProduit || string.IsNullOrEmpty(pProduit))
                        && (p.DATECREATION >= pDatedemande || pDatedemande == null)
                        && (p.STATUT == Enumere.DemandeStatusEnAttente && string.IsNullOrEmpty(p.STATUTDEMANDE))
                        && (p.ISSUPPRIME == false || p.ISSUPPRIME == null)  
 

                    select new
                    {
                        p.NUMDEM,
                        p.CENTRE,
                        p.NUMPERE,
                        p.TYPEDEMANDE,
                        p.DPRRDV,
                        p.DPRDEV,
                        p.DPREX,
                        p.DREARDV,
                        p.DREADEV,
                        p.DREAEX,
                        p.HRDVPR,
                        p.FDEM,
                        p.FREP,
                        p.NOMPERE,
                        p.NOMMERE,
                        p.MATRICULE,
                        p.STATUT,
                        p.DCAISSE,
                        p.NCAISSE,
                        p.EXDAG,
                        p.EXDBRT,
                        p.PRODUIT,
                        p.EXCL,
                        p.CLIENT,
                        p.EXCOMPT,
                        p.COMPTEUR,
                        p.EXEVT,
                        p.CTAXEG,
                        p.DATED,
                        p.REFEM,
                        p.ORDRE,
                        p.TOPEDIT,
                        p.FACTURE,
                        p.DATEFLAG,
                        p.USERCREATION,
                        p.DATECREATION,
                        p.DATEMODIFICATION,
                        p.USERMODIFICATION,
                        p.ETAPEDEMANDE,
                        p.PK_ID,
                        p.FK_IDCENTRE,
                        p.FK_IDADMUTILISATEUR,
                        p.FK_IDTYPEDEMANDE,
                        p.FK_IDPRODUIT,
                        p.STATUTDEMANDE,
                        p.ANNOTATION,
                        p.FICHIERJOINT
                    };
                    return Galatee.Tools.Utility.ListToDataTable(query);

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static DataTable  RetourneListeDemandeModificationPourSuvie(string pCentre, string pNumdem, string pTdem, string pProduit, DateTime? datedemande, string matricule)
        {
            try
            {
                List<string> _lstTypeDemande = new List<string>();
                if (string.IsNullOrEmpty(pTdem))
                {
                    _lstTypeDemande.Add(Enumere.ModificationAbonnement);
                    _lstTypeDemande.Add(Enumere.ModificationAdresse);
                    _lstTypeDemande.Add(Enumere.ModificationBranchement);
                    _lstTypeDemande.Add(Enumere.ModificationClient);
                    _lstTypeDemande.Add(Enumere.ModificationCompteur);
                }
                else
                    _lstTypeDemande.Add(pTdem);


                DateTime? pDatedemande = datedemande == null ? null : datedemande;
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = null;
                    var _DEMANDE = context.DEMANDE;
                    query =
                    from p in _DEMANDE
                    where
                        (p.NUMDEM == pNumdem || string.IsNullOrEmpty(pNumdem))
                    && (p.CENTRE == pCentre || string.IsNullOrEmpty(pCentre))
                    && (_lstTypeDemande.Contains(p.TYPEDEMANDE))
                    && (p.PRODUIT == pProduit || string.IsNullOrEmpty(pProduit))
                    && (p.DATECREATION >= pDatedemande || pDatedemande == null)
                    && ((p.STATUT == Enumere.DemandeStatusEnAttente) || (p.STATUTDEMANDE != null && !string.IsNullOrEmpty(p.ANNOTATION)))
                    && (p.MATRICULE == matricule || string.IsNullOrEmpty(matricule))
                    && (p.ISSUPPRIME == false || p.ISSUPPRIME == null)  
 

                    select new
                    {
                        p.NUMDEM,
                        p.CENTRE,
                        p.NUMPERE,
                        p.TYPEDEMANDE,
                        p.DPRRDV,
                        p.DPRDEV,
                        p.DPREX,
                        p.DREARDV,
                        p.DREADEV,
                        p.DREAEX,
                        p.HRDVPR,
                        p.FDEM,
                        p.FREP,
                        p.NOMPERE,
                        p.NOMMERE,
                        p.MATRICULE,
                        p.STATUT,
                        p.DCAISSE,
                        p.NCAISSE,
                        p.EXDAG,
                        p.EXDBRT,
                        p.PRODUIT,
                        p.EXCL,
                        p.CLIENT,
                        p.EXCOMPT,
                        p.COMPTEUR,
                        p.EXEVT,
                        p.CTAXEG,
                        p.DATED,
                        p.REFEM,
                        p.ORDRE,
                        p.TOPEDIT,
                        p.FACTURE,
                        p.DATEFLAG,
                        p.USERCREATION,
                        p.DATECREATION,
                        p.DATEMODIFICATION,
                        p.USERMODIFICATION,
                        p.ETAPEDEMANDE,
                        p.PK_ID,
                        p.FK_IDCENTRE,
                        p.FK_IDADMUTILISATEUR,
                        p.FK_IDTYPEDEMANDE,
                        p.FK_IDPRODUIT,
                        p.STATUTDEMANDE,
                        p.ANNOTATION,
                        p.FICHIERJOINT
                    };
                    return Galatee.Tools.Utility.ListToDataTable(query);

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static DataTable  RetourneListeDemande(string pCentre, string pNumdem, List<string> LstTdem, DateTime ? pDatedebut, DateTime ? pDateFin,
                                     DateTime ? pDatedemande, string pNumerodebut, string pNumerofin, string pStatus)
        {
            //cmd.CommandText = "SPX_GUI_RETOURNE_DABON";
            try
            {

                string centre = string.IsNullOrEmpty(pCentre) ? "" : pCentre;
                string numdem = string.IsNullOrEmpty(pNumdem) ? "" : pNumdem;
                DateTime ? datedebut =pDatedebut==null  ? null  : pDatedebut;
                DateTime ? dateFin =pDateFin==null  ? null  : pDateFin;
                DateTime ? datedemande =pDatedemande==null  ? null  : pDatedemande;
                string numerodebut = string.IsNullOrEmpty(pNumerodebut) ? "" : pNumerodebut;
                string numerofin = string.IsNullOrEmpty(pNumerofin) ? "" : pNumerofin;
                string status = string.IsNullOrEmpty(pStatus) ? "" : pStatus;
                using (galadbEntities context = new galadbEntities() )
                {
                    IEnumerable<object> query = null;
                    var _DEMANDE = context.DEMANDE;
                    query =
                    from p in _DEMANDE
                    where
                        (p.NUMDEM == pNumdem || string.IsNullOrEmpty(pNumdem))
                    && (p.CENTRE == pCentre || string.IsNullOrEmpty(pCentre))
                    && (LstTdem.Count == 0 || LstTdem.Contains(p.TYPEDEMANDE))
                    && (p.DATECREATION >= pDatedemande || pDatedemande == null)
                    && (p.STATUT == pStatus || string.IsNullOrEmpty(pStatus))
                    && (p.ISSUPPRIME == false || p.ISSUPPRIME == null)  
                    select new
                    {
                        p.NUMDEM,
                        p.CENTRE,
                        p.NUMPERE,
                        p.TYPEDEMANDE,
                        p.DPRRDV,
                        p.DPRDEV,
                        p.DPREX,
                        p.DREARDV,
                        p.DREADEV,
                        p.DREAEX,
                        p.HRDVPR,
                        p.FDEM,
                        p.FREP,
                        p.NOMPERE,
                        p.NOMMERE,
                        p.MATRICULE,
                        p.STATUT,
                        p.DCAISSE,
                        p.NCAISSE,
                        p.EXDAG,
                        p.EXDBRT,
                        p.PRODUIT,
                        p.EXCL,
                        p.CLIENT,
                        p.EXCOMPT,
                        p.COMPTEUR,
                        p.EXEVT,
                        p.CTAXEG,
                        p.DATED,
                        p.REFEM,
                        p.ORDRE,
                        p.TOPEDIT,
                        p.FACTURE,
                        p.DATEFLAG,
                        p.USERCREATION,
                        p.DATECREATION,
                        p.DATEMODIFICATION,
                        p.USERMODIFICATION,
                        p.ETAPEDEMANDE,
                        p.PK_ID,
                        p.FK_IDCENTRE,
                        p.FK_IDADMUTILISATEUR,
                        p.FK_IDTYPEDEMANDE,
                        p.FK_IDPRODUIT,
                        p.STATUTDEMANDE,
                        p.ANNOTATION,
                        p.FICHIERJOINT,
                        LIBELLETYPEDEMANDE= p.TYPEDEMANDE1.LIBELLE,
                        LIBELLEPRODUIT = p.PRODUIT1.LIBELLE,
                        LIBELLECENTRE = p.CENTRE1.LIBELLE,
                    };
                    return Galatee.Tools.Utility.ListToDataTable(query);

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable RetourneListeDemandeVisual(int? pCentre, string pNumdem, List<string> LstTdem, DateTime? pDatedebut, DateTime? pDateFin,
                                DateTime? pDatedemande, string pNumerodebut, string pNumerofin, string pStatus)
        {
            //cmd.CommandText = "SPX_GUI_RETOURNE_DABON";
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = null;
                    var _DEMANDE = context.DEMANDE;
                    var _DEMANDEwkf = context.DEMANDE_WORFKLOW;
                    var _wkfEtap = context.ETAPE ;
                    query =
                    from p in _DEMANDE
                    join wk in _DEMANDEwkf on p.NUMDEM equals wk.CODE_DEMANDE_TABLETRAVAIL
                    where
                        (p.NUMDEM == pNumdem || string.IsNullOrEmpty(pNumdem))
                    && (p.FK_IDCENTRE == pCentre || pCentre == null)
                    && (LstTdem.Count == 0 || LstTdem.Contains(p.TYPEDEMANDE))
                    && ((pDatedemande == null &&  
                      pDatedebut != null && pDateFin != null && 
                      p.DATECREATION >= pDatedebut && 
                      p.DATECREATION < pDateFin) ||
                      (pDatedemande == null &&
                      pDatedebut != null && pDateFin == null &&
                      p.DATECREATION >= pDatedebut) ||
                      (pDatedemande == null &&
                      pDatedebut == null && pDateFin < null) ||
                      (pDatedemande == null &&
                      pDatedebut == null && pDateFin == null) ||
                      (pDatedemande != null &&
                      pDatedebut == null &&  pDateFin == null &&
                      p.DATECREATION.Day == pDatedemande.Value.Day &&
                      p.DATECREATION.Month == pDatedemande.Value.Month &&
                      p.DATECREATION.Year == pDatedemande.Value.Year)) 
                    //&& (p.STATUT == pStatus || string.IsNullOrEmpty(pStatus))
                    //&& (p.ISSUPPRIME == false || p.ISSUPPRIME == null)
                    select new
                    {
                        p.NUMDEM,
                        p.CENTRE,
                        p.NUMPERE,
                        p.TYPEDEMANDE,
                        p.DPRRDV,
                        p.DPRDEV,
                        p.DPREX,
                        p.DREARDV,
                        p.DREADEV,
                        p.DREAEX,
                        p.HRDVPR,
                        p.FDEM,
                        p.FREP,
                        p.NOMPERE,
                        p.NOMMERE,
                        p.MATRICULE,
                        p.STATUT,
                        p.DCAISSE,
                        p.NCAISSE,
                        p.EXDAG,
                        p.EXDBRT,
                        p.PRODUIT,
                        p.EXCL,
                        p.CLIENT,
                        p.EXCOMPT,
                        p.COMPTEUR,
                        p.EXEVT,
                        p.CTAXEG,
                        p.DATED,
                        p.REFEM,
                        p.ORDRE,
                        p.TOPEDIT,
                        p.FACTURE,
                        p.DATEFLAG,
                        p.USERCREATION,
                        p.DATECREATION,
                        p.DATEMODIFICATION,
                        p.USERMODIFICATION,
                        p.ETAPEDEMANDE,
                        p.PK_ID,
                        p.FK_IDCENTRE,
                        p.FK_IDADMUTILISATEUR,
                        p.FK_IDTYPEDEMANDE,
                        p.FK_IDPRODUIT,
                        p.STATUTDEMANDE,
                        p.ANNOTATION,
                        p.FICHIERJOINT,
                        LIBELLETYPEDEMANDE = p.TYPEDEMANDE1.LIBELLE,
                        LIBELLEPRODUIT = p.PRODUIT1.LIBELLE,
                        LIBELLECENTRE = p.CENTRE1.LIBELLE,
                        wk.FK_IDSTATUS 
                    };
                    return Galatee.Tools.Utility.ListToDataTable(query);

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable RetourneDAG(int Fk_Idcentre, string pCentre, string pNumDem)
        {
            //cmd.CommandText = "SPX_GUI_RETOURNE_DABON";
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = null;
                    var _DAG = context.DAG;

                    query =
                    from _LeDAG in _DAG
                    where
                     ((_LeDAG.CENTRE == pCentre || string.IsNullOrEmpty(pCentre)) &&
                      (_LeDAG.NUMDEM == pNumDem || string.IsNullOrEmpty(pNumDem)) &&
                      (_LeDAG.FK_IDCENTRE == Fk_Idcentre))
                    select new
                    {

                        _LeDAG.ETAGE,
                        _LeDAG.CENTRE,
                        _LeDAG.CLIENT,
                        _LeDAG.NOMP,
                        _LeDAG.COMMUNE,
                        _LeDAG.QUARTIER,
                        _LeDAG.RUE,
                        _LeDAG.PORTE,
                        _LeDAG.CADR,
                        _LeDAG.REGROU,
                        _LeDAG.CPARC,
                        _LeDAG.DMAJ,
                        _LeDAG.TOURNEE,
                        _LeDAG.ORDTOUR,
                        _LeDAG.SECTEUR,
                        _LeDAG.CPOS,
                        _LeDAG.TELEPHONE,
                        _LeDAG.FAX,
                        _LeDAG.EMAIL,
                        _LeDAG.USERCREATION,
                        _LeDAG.DATECREATION,
                        _LeDAG.DATEMODIFICATION,
                        _LeDAG.USERMODIFICATION,
                        _LeDAG.PK_ID,
                        _LeDAG.FK_IDTOURNEE,
                        _LeDAG.FK_IDQUARTIER,
                        LIBELLEQUARTIER = _LeDAG.QUARTIER1.LIBELLE,
                        _LeDAG.FK_IDCOMMUNE,
                        LIBELLECOMMUNE = _LeDAG.COMMUNE1.LIBELLE,
                        _LeDAG.FK_IDRUE,
                        LIBELLERUE = _LeDAG.RUES.LIBELLE,
                        _LeDAG.FK_IDCENTRE,
                        LIBELLESECTEUR = _LeDAG.SECTEUR1.LIBELLE,
                        _LeDAG.FK_IDSECTEUR,
                        _LeDAG.NUMDEM,
                        _LeDAG.FK_IDDEMANDE
                    };
                    return Galatee.Tools.Utility.ListToDataTable(query);
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static DataTable RetourneDBrt(int Fk_Idcentre, string pCentre, string pNumDem)
        {
            //cmd.CommandText = "SPX_GUI_RETOURNE_DABON";
            try
            {

                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = null;
                    var _DBRT = context.DBRT;

                    query =
                    from _LeDBRT in _DBRT
                    where
                     ((_LeDBRT.CENTRE == pCentre || string.IsNullOrEmpty(pCentre)) &&
                      (_LeDBRT.NUMDEM == pNumDem || string.IsNullOrEmpty(pNumDem)) &&
                      (_LeDBRT.FK_IDCENTRE == Fk_Idcentre))
                    select new
                    {
                        _LeDBRT.CENTRE,
                        _LeDBRT.CLIENT,
                        _LeDBRT.PRODUIT,
                        _LeDBRT.DRAC,
                        _LeDBRT.DRES,
                        _LeDBRT.SERVICE,
                        _LeDBRT.CATBRT,
                        _LeDBRT.DIAMBRT,
                        _LeDBRT.LONGBRT,
                        _LeDBRT.NATBRT,
                        _LeDBRT.NBPOINT,
                        _LeDBRT.RESEAU,
                        _LeDBRT.TRONCON,
                        _LeDBRT.DMAJ,
                        _LeDBRT.TRANSFORMATEUR,
                        _LeDBRT.PUISSANCEINSTALLEE,
                        _LeDBRT.PERTES,
                        _LeDBRT.COEFPERTES,
                        _LeDBRT.APPTRANSFO,
                        _LeDBRT.CODEBRT,
                        _LeDBRT.CODEPOSTE,
                        _LeDBRT.MARQUETRANSFO,
                        _LeDBRT.ANFAB,
                        _LeDBRT.LONGITUDE,
                        _LeDBRT.LATITUDE,
                        _LeDBRT.ADRESSERESEAU,
                        _LeDBRT.USERCREATION,
                        _LeDBRT.DATECREATION,
                        _LeDBRT.DATEMODIFICATION,
                        _LeDBRT.USERMODIFICATION,
                        _LeDBRT.PK_ID,
                        _LeDBRT.FK_IDCENTRE,
                        _LeDBRT.FK_IDPRODUIT,
                        _LeDBRT.FK_IDTYPEBRANCHEMENT,
                        LIBELLETYPEBRANCHEMENT = _LeDBRT.TYPEBRANCHEMENT.LIBELLE,
                        CODETYPEBRANCHEMENT =_LeDBRT.TYPEBRANCHEMENT.CODE ,
                        _LeDBRT.FK_IDDEMANDE,
                        _LeDBRT.NUMDEM,

                        _LeDBRT.FK_IDPOSTESOURCE,
                        CODEPOSTESOURCE = _LeDBRT.POSTESOURCE.CODE,
                        LIBELLEPOSTESOURCE = _LeDBRT.POSTESOURCE.LIBELLE,

                        _LeDBRT.FK_IDDEPARTHTA,
                        CODEDEPARTHTA = _LeDBRT.DEPARTHTA.CODE,
                        LIBELLEDEPARTHTA = _LeDBRT.DEPARTHTA.LIBELLE,

                        _LeDBRT.FK_IDPOSTETRANSFORMATION,
                        CODETRANSFORMATEUR = _LeDBRT.POSTETRANSFORMATION.CODE,
                        LIBELLETRANSFORMATEUR = _LeDBRT.POSTETRANSFORMATION.LIBELLE,

                        _LeDBRT.FK_IDQUARTIER,
                        CODEQUARTIER = _LeDBRT.QUARTIER.CODE,
                        LIBELLEQUARTIER = _LeDBRT.QUARTIER.LIBELLE,

                        _LeDBRT.FK_IDDEPARTBT,
                        CODEDEPARTBT = _LeDBRT.DEPARTBT.CODE,
                        LIBELLEDEPARTBT = _LeDBRT.DEPARTBT.LIBELLE,

                        _LeDBRT.NEOUDFINAL, 
                    };
                    return Galatee.Tools.Utility.ListToDataTable(query);
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static DataTable RetourneDClient(int Fk_Idcentre, string pCentre, string pNumDem)
        {
            //cmd.CommandText = "SPX_GUI_RETOURNE_DABON";
            try
            {

                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = null;
                    var _DCLIENT = context.DCLIENT;
                    query =
                    from _LeDCLIENT in _DCLIENT
                    where
                     ((_LeDCLIENT.CENTRE == pCentre || string.IsNullOrEmpty(pCentre)) &&
                     (_LeDCLIENT.NUMDEM == pNumDem || string.IsNullOrEmpty(pNumDem)) &&
                     (_LeDCLIENT.FK_IDCENTRE == Fk_Idcentre))
                    select new
                    {
                        _LeDCLIENT.ISFACTUREEMAIL,
                        _LeDCLIENT.ISFACTURESMS,
                        _LeDCLIENT.TELEPHONEFIXE,
                        _LeDCLIENT.CODEIDENTIFICATIONNATIONALE,
                        _LeDCLIENT.EMAIL,
                        _LeDCLIENT.NUMPROPRIETE,
                        _LeDCLIENT.CENTRE,
                        _LeDCLIENT.REFCLIENT,
                        _LeDCLIENT.ORDRE,
                        _LeDCLIENT.DENABON,
                        _LeDCLIENT.NOMABON,
                        _LeDCLIENT.DENMAND,
                        _LeDCLIENT.NOMMAND,
                        _LeDCLIENT.ADRMAND1,
                        _LeDCLIENT.ADRMAND2,
                        _LeDCLIENT.CPOS,
                        _LeDCLIENT.BUREAU,
                        _LeDCLIENT.DINC,
                        _LeDCLIENT.MODEPAIEMENT,
                        _LeDCLIENT.NOMTIT,
                        _LeDCLIENT.BANQUE,
                        _LeDCLIENT.GUICHET,
                        _LeDCLIENT.COMPTE,
                        _LeDCLIENT.RIB,
                        _LeDCLIENT.PROPRIO,
                        _LeDCLIENT.CODECONSO,
                        _LeDCLIENT.CATEGORIE,
                        _LeDCLIENT.CODERELANCE,
                        _LeDCLIENT.NOMCOD,
                        _LeDCLIENT.MOISNAIS,
                        _LeDCLIENT.ANNAIS,
                        _LeDCLIENT.NUMDEM,
                        _LeDCLIENT.NOMPERE,
                        _LeDCLIENT.NOMMERE,
                        _LeDCLIENT.NATIONNALITE,
                        _LeDCLIENT.CNI,
                        _LeDCLIENT.TELEPHONE,
                        _LeDCLIENT.MATRICULE,
                        _LeDCLIENT.REGROUPEMENT,
                        _LeDCLIENT.REGEDIT,
                        _LeDCLIENT.FACTURE,
                        _LeDCLIENT.DMAJ,
                        _LeDCLIENT.REFERENCEPUPITRE,
                        _LeDCLIENT.PAYEUR,
                        _LeDCLIENT.SOUSACTIVITE,
                        _LeDCLIENT.AGENTFACTURE,
                        _LeDCLIENT.AGENTRECOUVR,
                        _LeDCLIENT.AGENTASSAINI,
                        _LeDCLIENT.REGROUCONTRAT,
                        _LeDCLIENT.INSPECTION,
                        _LeDCLIENT.REGLEMENT,
                        _LeDCLIENT.DECRET,
                        _LeDCLIENT.CONVENTION,
                        _LeDCLIENT.REFERENCEATM,
                        _LeDCLIENT.PK_ID,
                        _LeDCLIENT.DATECREATION,
                        _LeDCLIENT.DATEMODIFICATION,
                        _LeDCLIENT.USERCREATION,
                        _LeDCLIENT.USERMODIFICATION,
                        _LeDCLIENT.FK_IDMODEPAIEMENT,
                        _LeDCLIENT.FK_IDCODECONSO,
                        _LeDCLIENT.FK_IDCATEGORIE,
                        _LeDCLIENT.FK_IDPROPRIETAIRE ,
                        LIBELLECATEGORIE = _LeDCLIENT.CATEGORIECLIENT.LIBELLE,
                        _LeDCLIENT.FK_IDRELANCE,
                        _LeDCLIENT.FK_IDPIECEIDENTITE,
                        LIBELLETYPEPIECE = _LeDCLIENT.PIECEIDENTITE.LIBELLE,
                        _LeDCLIENT.NUMEROPIECEIDENTITE,
                        _LeDCLIENT.FK_IDNATIONALITE,
                        _LeDCLIENT.FK_IDREGROUPEMENT,
                        _LeDCLIENT.FK_IDDEMANDE,
                        _LeDCLIENT.FK_IDCENTRE,
                        _LeDCLIENT.FK_TYPECLIENT,
                        _LeDCLIENT.FK_IDUSAGE,
                        _LeDCLIENT.FAX,
                        _LeDCLIENT.BOITEPOSTAL 
                    };

                    return Galatee.Tools.Utility.ListToDataTable(query);
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static DataTable RetourneDcanalisationEnMagazin(int Fk_Idcentre, string pCentre, string pNumDem)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = null;
                    var _DCANALISATION = context.DCANALISATION;

                    query =
                    from _LeDCANALISATION in _DCANALISATION
                    join _COMPTEUR in context.MAGASINVIRTUEL
                   on new { FK_IDCOMPTEUR = _LeDCANALISATION.FK_IDCOMPTEUR.Value } equals new { FK_IDCOMPTEUR = _COMPTEUR.PK_ID }
                    where
                     ((_LeDCANALISATION.CENTRE == pCentre ) &&
                      (_LeDCANALISATION.NUMDEM == pNumDem ) &&
                      (_LeDCANALISATION.FK_IDCENTRE == Fk_Idcentre))
                    select new
                    {
                        _LeDCANALISATION.CENTRE,
                        _LeDCANALISATION.CLIENT,
                        _LeDCANALISATION.NUMDEM,
                        _LeDCANALISATION.PRODUIT,
                        _LeDCANALISATION.POINT,
                        _LeDCANALISATION.BRANCHEMENT,
                        _LeDCANALISATION.SURFACTURATION,
                        _LeDCANALISATION.DEBITANNUEL,
                        //_LeDCANALISATION.POSE ,
                        //_LeDCANALISATION.DEPOSE ,
                        _LeDCANALISATION.USERCREATION,
                        _LeDCANALISATION.DATECREATION,
                        _LeDCANALISATION.DATEMODIFICATION,
                        _LeDCANALISATION.USERMODIFICATION,
                        _LeDCANALISATION.PK_ID,
                        _LeDCANALISATION.FK_IDCENTRE,
                        _LeDCANALISATION.FK_IDCOMPTEUR,
                        _LeDCANALISATION.FK_IDPRODUIT,
                        _LeDCANALISATION.FK_IDDEMANDE,

                        _LeDCANALISATION.FK_IDPROPRIETAIRE,
                        _LeDCANALISATION.PROPRIO,


                        // Compteur
                        _COMPTEUR.NUMERO,
                        TYPECOMPTEUR = _COMPTEUR.TYPECOMPTEUR.CODE,
                        _COMPTEUR.FK_IDCALIBRECOMPTEUR ,
                        _COMPTEUR.MARQUE,
                        LIBELLEMARQUE =_COMPTEUR.MARQUECOMPTEUR.LIBELLE,
                        _COMPTEUR.COEFLECT,
                        _COMPTEUR.COEFCOMPTAGE,
                        _COMPTEUR.CADRAN,
                        _COMPTEUR.ANNEEFAB,
                        _COMPTEUR.FONCTIONNEMENT,
                        CAS=Enumere.CasPoseCompteur
                        // AKO 11/11/2015   _LeDCANALISATION.COMMENTAIRE
                    };


                    return Galatee.Tools.Utility.ListToDataTable(query);
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static DataTable RetourneCompteurEnMagazinByID(int IdMagazinvirtuel)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = null;
                    var x = context.MAGASINVIRTUEL ;

                    query =
                    from mg in x
                    where
                    mg.PK_ID == IdMagazinvirtuel 
                    select new
                    {
                        // Compteur
                        mg.NUMERO,
                        TYPECOMPTEUR = mg.TYPECOMPTEUR.CODE,
                        //mg.DIAMETRE,
                        mg.MARQUE,
                        LIBELLEMARQUE = mg.MARQUECOMPTEUR.LIBELLE,
                        mg.COEFLECT,
                        mg.COEFCOMPTAGE,
                        mg.CADRAN,
                        mg.ANNEEFAB,
                        mg.FONCTIONNEMENT,
                        CAS = Enumere.CasPoseCompteur
                    };


                    return Galatee.Tools.Utility.ListToDataTable(query);
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static DataTable RetourneDcanalisation(int Fk_Idcentre, string pCentre, string pNumDem)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = null;
                    var _DCANALISATION = context.DCANALISATION;

                    query =
                    from _LeDCANALISATION in _DCANALISATION
                    where 
                    _LeDCANALISATION.FK_IDCENTRE == Fk_Idcentre && 
                    _LeDCANALISATION.CENTRE == pCentre && 
                    _LeDCANALISATION.NUMDEM == pNumDem 
                    select new
                    {
                        _LeDCANALISATION.CENTRE,
                        _LeDCANALISATION.CLIENT,
                        _LeDCANALISATION.NUMDEM,
                        _LeDCANALISATION.PRODUIT,
                        _LeDCANALISATION.POINT,
                        _LeDCANALISATION.BRANCHEMENT,
                        _LeDCANALISATION.SURFACTURATION,
                        _LeDCANALISATION.DEBITANNUEL,
                        //_LeDCANALISATION.DEPOSE ,
                        //_LeDCANALISATION.POSE ,
                        _LeDCANALISATION.USERCREATION,
                        _LeDCANALISATION.DATECREATION,
                        _LeDCANALISATION.DATEMODIFICATION,
                        _LeDCANALISATION.USERMODIFICATION,
                        _LeDCANALISATION.PK_ID,
                        _LeDCANALISATION.FK_IDCENTRE,
                        _LeDCANALISATION.FK_IDCOMPTEUR,
                        _LeDCANALISATION.FK_IDPRODUIT,
                        _LeDCANALISATION.FK_IDDEMANDE,
                        _LeDCANALISATION.FK_IDMAGAZINVIRTUEL ,
                        _LeDCANALISATION.FK_IDPROPRIETAIRE,
                        _LeDCANALISATION.PROPRIO,

                        // Compteur
                       _LeDCANALISATION.COMPTEUR.NUMERO,
                        _LeDCANALISATION.COMPTEUR.TYPECOMPTEUR1.CODE,
                        //_LeDCANALISATION.COMPTEUR.TYPECOMPTAGE,
                        _LeDCANALISATION.REGLAGECOMPTEUR ,
                        _LeDCANALISATION.COMPTEUR.MARQUE,
                        LIBELLEMARQUE = _LeDCANALISATION.COMPTEUR.MARQUECOMPTEUR.LIBELLE,
                        _LeDCANALISATION.COMPTEUR.COEFLECT,
                        _LeDCANALISATION.COMPTEUR.COEFCOMPTAGE,
                        _LeDCANALISATION.COMPTEUR.CADRAN,
                        _LeDCANALISATION.COMPTEUR.ANNEEFAB,
                        _LeDCANALISATION.COMPTEUR.FONCTIONNEMENT,
                   // AKO 11/11/2015   _LeDCANALISATION.COMMENTAIRE
                    };
                 
                               
                    return Galatee.Tools.Utility.ListToDataTable(query);
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static DataTable RetourneDEvenement(int Fk_Idcentre, string pCentre, string pNumDem)
        {
            try
            {

                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = null;
                    var _DEVENEMENT = context.DEVENEMENT;

                    query =
                    from _LeDEVENEMENT in _DEVENEMENT
                    where
                     ((_LeDEVENEMENT.CENTRE == pCentre || string.IsNullOrEmpty(pCentre)) &&
                     (_LeDEVENEMENT.NUMDEM == pNumDem || string.IsNullOrEmpty(pNumDem)) &&
                     (_LeDEVENEMENT.FK_IDCENTRE == Fk_Idcentre))

                    select new
                    {
                        _LeDEVENEMENT.CENTRE,
                        _LeDEVENEMENT.CLIENT,
                        _LeDEVENEMENT.PRODUIT,
                        _LeDEVENEMENT.POINT,
                        _LeDEVENEMENT.NUMEVENEMENT,
                        _LeDEVENEMENT.ORDRE,
                        _LeDEVENEMENT.COMPTEUR,
                        _LeDEVENEMENT.DATEEVT,
                        _LeDEVENEMENT.PERIODE,
                        _LeDEVENEMENT.CODEEVT,
                        _LeDEVENEMENT.INDEXEVT,
                        _LeDEVENEMENT.CAS,
                        _LeDEVENEMENT.ENQUETE,
                        _LeDEVENEMENT.CONSO,
                        _LeDEVENEMENT.CONSONONFACTUREE,
                        _LeDEVENEMENT.LOTRI,
                        _LeDEVENEMENT.FACTURE,
                        _LeDEVENEMENT.SURFACTURATION,
                        _LeDEVENEMENT.STATUS,
                        _LeDEVENEMENT.TYPECONSO,
                        _LeDEVENEMENT.REGLAGECOMPTEUR,
                        _LeDEVENEMENT.MATRICULE,
                        _LeDEVENEMENT.FACPER,
                        _LeDEVENEMENT.QTEAREG,
                        _LeDEVENEMENT.DERPERF,
                        _LeDEVENEMENT.DERPERFN,
                        _LeDEVENEMENT.CONSOFAC,
                        _LeDEVENEMENT.REGIMPUTE,
                        _LeDEVENEMENT.REGCONSO,
                        _LeDEVENEMENT.COEFLECT,
                        _LeDEVENEMENT.COEFCOMPTAGE,
                        _LeDEVENEMENT.PUISSANCE,
                        _LeDEVENEMENT.TYPECOMPTAGE,
                        _LeDEVENEMENT.TYPECOMPTEUR,
                        _LeDEVENEMENT.TYPETARIF ,
                        _LeDEVENEMENT.CATEGORIE ,
                        _LeDEVENEMENT.COEFK1,
                        _LeDEVENEMENT.COEFK2,
                        _LeDEVENEMENT.COEFFAC,
                        _LeDEVENEMENT.USERCREATION,
                        _LeDEVENEMENT.DATECREATION,
                        _LeDEVENEMENT.DATEMODIFICATION,
                        _LeDEVENEMENT.USERMODIFICATION,
                        _LeDEVENEMENT.PK_ID,
                        _LeDEVENEMENT.NUMDEM,
                        _LeDEVENEMENT.FK_IDABON,
                        _LeDEVENEMENT.FK_IDCANALISATION ,
                        _LeDEVENEMENT.FK_IDCENTRE,
                        _LeDEVENEMENT.FK_IDPRODUIT,
                        _LeDEVENEMENT.FK_IDDEMANDE,
                        _LeDEVENEMENT.FK_IDCOMPTEUR ,
                        _LeDEVENEMENT.COMMENTAIRE ,
                        _LeDEVENEMENT.FK_IDTOURNEE,
                        _LeDEVENEMENT.TOURNEE,
                        _LeDEVENEMENT.ORDTOUR,
                        _LeDEVENEMENT.PERFAC,
                        _LeDEVENEMENT.CONSOMOYENNEPRECEDENTEFACTURE,
                        _LeDEVENEMENT.DATERELEVEPRECEDENTEFACTURE,
                        _LeDEVENEMENT.CASPRECEDENTEFACTURE,
                        _LeDEVENEMENT.INDEXPRECEDENTEFACTURE,
                        _LeDEVENEMENT.PERIODEPRECEDENTEFACTURE,
                        _LeDEVENEMENT.ORDREAFFICHAGE,
                        _LeDEVENEMENT.NOUVEAUCOMPTEUR,
                        _LeDEVENEMENT.PUISSANCEINSTALLEE,
                        _LeDEVENEMENT.COEFKR1,
                        _LeDEVENEMENT.COEFKR2,
                        _LeDEVENEMENT.QTEAREGPRECEDENT,
                        _LeDEVENEMENT.ISCONSOSEULE,

                    };
                    return Galatee.Tools.Utility.ListToDataTable(query);
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static DataTable RetourneDemandeDetailCout(int Fk_Idcentre, string pCentre, string pNumDem)
        {
            try
            {

                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = null;
                    var _RUBRIQUEDEMANDE = context.RUBRIQUEDEMANDE;

                    query =
                    from x in _RUBRIQUEDEMANDE
                    where
                     ((x.CENTRE == pCentre || string.IsNullOrEmpty(pCentre)) &&
                     (x.NUMDEM == pNumDem || string.IsNullOrEmpty(pNumDem)))

                    select new
                    {
                        x.NUMDEM,
                        x.CENTRE,
                        x.NDOC,
                        x.REFEM,
                        x.CLIENT,
                        x.ORDRE,
                        x.COPER,
                        x.MONTANTHT,
                        x.MONTANTTAXE,
                        x.TAXE,
                        x.DATECREATION,
                        x.DATEMODIFICATION,
                        x.USERCREATION,
                        x.USERMODIFICATION,
                        x.PK_ID,
                        x.FK_IDCENTRE,
                        x.FK_IDDEMANDE,
                        x.FK_IDCOPER,
                        x.FK_IDTAXE,
                        LIBELLETAXE = x.TAXE1.LIBELLE,
                        x.COPER1.LIBELLE
                    };
                    return Galatee.Tools.Utility.ListToDataTable(query);
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static DataTable RetourneDemandeDetailCout(string centre, string client, string ordre)
        {
            try
            {

                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = null;
                    var _RUBRIQUEDEMANDE = context.RUBRIQUEDEMANDE;

                    query =
                    from x in _RUBRIQUEDEMANDE
                    where
                     ((x.CENTRE == centre || string.IsNullOrEmpty(centre)) &&
                     (x.CLIENT == client || string.IsNullOrEmpty(client)) &&
                     (x.ORDRE == ordre || string.IsNullOrEmpty(ordre))
                     )

                    select new
                    {
                        x.NUMDEM,
                        x.CENTRE,
                        x.NDOC,
                        x.REFEM,
                        x.CLIENT,
                        x.ORDRE,
                        x.COPER,
                        x.MONTANTHT,
                        x.MONTANTTAXE,
                        x.TAXE,
                        x.DATECREATION,
                        x.DATEMODIFICATION,
                        x.USERCREATION,
                        x.USERMODIFICATION,
                        x.PK_ID,
                        x.FK_IDCENTRE,
                        x.FK_IDDEMANDE,
                        x.FK_IDCOPER,
                        x.FK_IDTAXE,
                        LIBELLETAXE = x.TAXE1.LIBELLE,
                        x.COPER1.LIBELLE
                    };
                    return Galatee.Tools.Utility.ListToDataTable(query);
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static DataTable RetourneDemandeTdem(int Fk_IDtdem)
        {
            try
            {

                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = null;
                    var _Letdem = context.TYPEDEMANDE;

                    query =
                    from _tdem in _Letdem
                    where _tdem.PK_ID == Fk_IDtdem
                    select new
                    {
                        _tdem.CODE,
                        _tdem.DEMOPTION1,
                        _tdem.DEMOPTION2,
                        _tdem.DEMOPTION3,
                        _tdem.DEMOPTION4,
                        _tdem.DEMOPTION5,
                        _tdem.DEMOPTION6,
                        _tdem.DEMOPTION7,
                        _tdem.DEMOPTION8,
                        _tdem.DEMOPTION9,
                        _tdem.DEMOPTION10,
                        _tdem.DEMOPTION11,
                        _tdem.DEMOPTION12,
                        _tdem.DEMOPTION13,
                        _tdem.DEMOPTION14,
                        _tdem.DEMOPTION15,
                        _tdem.DEMOPTION16,
                        _tdem.DEMOPTION17,
                        _tdem.DEMOPTION18,
                        _tdem.DEMOPTION19,
                        _tdem.DEMOPTION20,
                        _tdem.LIBELLE,
                        _tdem.EVT1,
                        _tdem.EVT2,
                        _tdem.EVT3,
                        _tdem.EVT4,
                        _tdem.EVT5,
                        _tdem.USERCREATION,
                        _tdem.DATECREATION,
                        _tdem.DATEMODIFICATION,
                        _tdem.USERMODIFICATION,
                        _tdem.PK_ID
                    };
                    return Galatee.Tools.Utility.ListToDataTable(query);
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static DataTable RetourneDemandeAchatTimbre(int Fk_IDtdem)
        {
            try
            {

                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = null;
                    var _LeTimbre = context.ELEMENTACHATTIMBRE ;

                    query =
                    from x in _LeTimbre
                    where x.FK_IDDEMANDE  == Fk_IDtdem
                    select new
                    {
                        x.NUMDEM,
                        x.QUANTITE,
                        x.TAXE,
                        x.MONTANT,
                        x.DATECREATION,
                        x.DATEMODIFICATION,
                        x.USERCREATION,
                        x.USERMODIFICATION,
                        x.PK_ID,
                        x.FK_IDDEMANDE,
                        x.FK_IDTIMBRE,
                        x.FK_IDCOPER,
                        x.FK_IDTAXE,
                        CODE = x.TYPETIMBRE.CODE ,
                        DESIGNATION = x.TYPETIMBRE.LIBELLE,
                        PRIXUNITAIRE = x.TYPETIMBRE.MONTANT 
                    };
                    return Galatee.Tools.Utility.ListToDataTable(query);
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static DataTable RetourneDCompteur(int Fk_IdDemande)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = null;
                    var _DCOMPTEUR = context.DCOMPTEUR ;

                    query =
                    from _LeDCOMPTEUR in _DCOMPTEUR
                    from _LeCanal in _LeDCOMPTEUR.COMPTEUR.CANALISATION 
                    where
                    _LeDCOMPTEUR.FK_IDDEMANDE == Fk_IdDemande 
                    select new
                    {
                        _LeDCOMPTEUR.PK_ID ,
                        _LeDCOMPTEUR.PRODUIT,
                        _LeDCOMPTEUR.NUMERO,
                        _LeDCOMPTEUR.TYPECOMPTEUR,
                        _LeDCOMPTEUR.MARQUE,
                        _LeDCOMPTEUR.COEFLECT,
                        _LeDCOMPTEUR.COEFCOMPTAGE,
                        _LeDCOMPTEUR.CADRAN,
                        _LeDCOMPTEUR.ANNEEFAB,
                        _LeDCOMPTEUR.FONCTIONNEMENT,
                        _LeDCOMPTEUR.PLOMBAGE,
                        _LeDCOMPTEUR.USERCREATION,
                        _LeDCOMPTEUR.DATECREATION,
                        _LeDCOMPTEUR.DATEMODIFICATION,
                        _LeDCOMPTEUR.USERMODIFICATION,

                        _LeDCOMPTEUR.FK_IDTYPECOMPTEUR,
                        _LeDCOMPTEUR.FK_IDPRODUIT,
                        _LeDCOMPTEUR.FK_IDMARQUECOMPTEUR,
                        _LeDCOMPTEUR.FK_IDCALIBRE,
                        _LeDCOMPTEUR.FK_IDSTATUTCOMPTEUR,
                        _LeDCOMPTEUR.FK_IDETATCOMPTEUR ,
                        _LeDCOMPTEUR.FK_IDCOMPTEUR,
                        _LeDCOMPTEUR.FK_IDDEMANDE ,

                        _LeCanal.CENTRE ,
                        _LeCanal.CLIENT ,
                        _LeCanal.POINT ,
                        CODECALIBRECOMPTEUR = _LeDCOMPTEUR.CALIBRECOMPTEUR.LIBELLE,
                        LIBELLETYPECOMPTEUR = _LeDCOMPTEUR.TYPECOMPTEUR1.LIBELLE,
                        LIBELLEMARQUE = _LeDCOMPTEUR.MARQUECOMPTEUR.LIBELLE,

                    };
                    return Galatee.Tools.Utility.ListToDataTable(query);
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public static DataTable GetDemandeByNumIdDemande(int pNumEtape)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                    from P in context.DEMANDE
                    orderby
                      P.NUMDEM descending

                    where P.PK_ID == pNumEtape
                    select new
                    {
                        P.MOTIF,
                        P.NUMDEM,
                        P.CENTRE,
                        P.NUMPERE,
                        P.TYPEDEMANDE,
                        P.DPRRDV,
                        P.DPRDEV,
                        P.DPREX,
                        P.DREARDV,
                        P.DREADEV,
                        P.DREAEX,
                        P.HRDVPR,
                        P.FDEM,
                        P.FREP,
                        P.NOMPERE,
                        P.NOMMERE,
                        P.MATRICULE,
                        P.STATUT,
                        P.DCAISSE,
                        P.NCAISSE,
                        P.EXDAG,
                        P.EXDBRT,
                        P.PRODUIT,
                        P.EXCL,
                        P.CLIENT,
                        P.EXCOMPT,
                        P.COMPTEUR,
                        P.EXEVT,
                        P.CTAXEG,
                        P.DATED,
                        P.REFEM,
                        P.ORDRE,
                        P.TOPEDIT,
                        P.FACTURE,
                        P.DATEFLAG,
                        P.TRANSMIS,
                        P.STATUTDEMANDE,
                        P.FICHIERJOINT,
                        P.ANNOTATION,
                        P.USERCREATION,
                        P.DATECREATION,
                        P.DATEMODIFICATION,
                        P.USERMODIFICATION,
                        P.ETAPEDEMANDE,
                        P.ISSUPPRIME,
                        P.USERSUPPRESSION,
                        P.DATESUPPRESSION,
                        P.PK_ID,
                        P.FK_IDCENTRE,
                        P.FK_IDCLIENT,
                        P.FK_IDADMUTILISATEUR,
                        P.FK_IDTYPEDEMANDE,
                        P.FK_IDPRODUIT,
                        INITIERPAR =context.ADMUTILISATEUR.FirstOrDefault(t=>t.MATRICULE == P.USERCREATION) != null ? context.ADMUTILISATEUR.FirstOrDefault(t=>t.MATRICULE == P.USERCREATION).LIBELLE : string.Empty,  
                        LIBELLECENTRE = P.CENTRE1.LIBELLE,
                        LIBELLETYPEDEMANDE = P.TYPEDEMANDE1.LIBELLE,
                        LIBELLEPRODUIT = P.PRODUIT1.LIBELLE,
                        LIBELLESITE = P.CENTRE1.SITE.LIBELLE,
                        P.ISCHANGECOMPTEUR ,
                        P.ISCONTROLE ,
                        P.ISEXTENSION ,
                        P.ISPRESTATION,
                        P.ISMUTATION ,
                        P.ISPOSE,
                        P.ISETALONNAGE ,
                        P.REGLAGECOMPTEUR ,
                        P.PUISSANCESOUSCRITE,
                        P.DATEFIN,
                        P.ISMETREAFAIRE,
                        P.TYPECOMPTAGE ,
                        P.FK_IDPUISSANCESOUSCRITE ,
                        P.FK_IDREGLAGECOMPTEUR ,
                        P.FK_IDTYPECOMPTAGE,
                        P.ISDEVISCOMPLEMENTAIRE ,
                        P.ISBONNEINITIATIVE,
                        P.ISCOMMUNE,
                        P.ISEDM,
                        P.NOMBREDEFOYER,
                        P.ISDEFINITIF,
                        P.ISPROVISOIR,
                        P.FK_IDDEMANDE ,
                        P.ISPASSERCAISSE,
                        P.ISDEVISHT,
                        CODEREGLAGECOMPTEUR= P.REGLAGECOMPTEUR1.REGLAGE,
                        SITE =  P.CENTRE1.CODESITE 


                    };
                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        public static DataTable RetourneAbon(string centre, string client, string ordre)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = null;
                    var _DABON = context.ABON;
                    var DABON = context.DABON;
                    query =
                    from _LeDABON in _DABON
                    join LeDABON in DABON on new { _LeDABON.CENTRE, _LeDABON.CLIENT, _LeDABON.ORDRE } equals new { LeDABON.CENTRE, LeDABON.CLIENT, LeDABON.ORDRE }
                    where
                     ((_LeDABON.CENTRE == centre || string.IsNullOrEmpty(centre)) &&
                     (_LeDABON.CLIENT == client || string.IsNullOrEmpty(client)) &&
                     (_LeDABON.ORDRE == ordre))
                    select new
                    {
                        LeDABON.ISAUGMENTATIONPUISSANCE,
                        LeDABON.ISDIMINUTIONPUISSANCE,
                        LeDABON.NOUVELLEPUISSANCE,
                        _LeDABON.CENTRE,
                        _LeDABON.CLIENT,
                        _LeDABON.ORDRE,
                        _LeDABON.PRODUIT,
                        _LeDABON.TYPETARIF,
                        _LeDABON.PUISSANCE,
                        _LeDABON.FORFAIT,
                        _LeDABON.FORFPERSO,
                        _LeDABON.AVANCE,
                        _LeDABON.DAVANCE,
                        _LeDABON.REGROU,
                        _LeDABON.PERFAC,
                        _LeDABON.MOISFAC,
                        _LeDABON.DABONNEMENT,
                        _LeDABON.DRES,
                        _LeDABON.DATERACBRT,
                        _LeDABON.NBFAC,
                        _LeDABON.PERREL,
                        _LeDABON.MOISREL,
                        _LeDABON.DMAJ,
                        _LeDABON.RECU,
                        _LeDABON.RISTOURNE,
                        _LeDABON.CONSOMMATIONMAXI,
                        _LeDABON.COEFFAC,
                        _LeDABON.USERCREATION,
                        _LeDABON.DATECREATION,
                        _LeDABON.DATEMODIFICATION,
                        _LeDABON.USERMODIFICATION,
                        _LeDABON.PK_ID,
                        _LeDABON.FK_IDCENTRE,
                        _LeDABON.FK_IDPRODUIT,
                        _LeDABON.FK_IDFORFAIT,
                        _LeDABON.FK_IDMOISREL,
                        _LeDABON.FK_IDMOISFAC,
                        _LeDABON.NOMBREDEFOYER,

                        //_LeDABON.FK_IDDEMANDE,
                        _LeDABON.FK_IDTYPETARIF,
                        _LeDABON.FK_IDPERIODICITERELEVE,
                        _LeDABON.FK_IDPERIODICITEFACTURE,
                        LIBELLECENTRE = _LeDABON.CENTRE1.LIBELLE,
                        LIBELLEPRODUIT = _LeDABON.PRODUIT1.LIBELLE,
                        LIBELLETARIF = _LeDABON.TYPETARIF1.LIBELLE,
                        LIBELLEFORFAIT = _LeDABON.FORFAIT1.LIBELLE,
                        LIBELLEMOISFACT = _LeDABON.MOIS.LIBELLE,
                        LIBELLEMOISIND = _LeDABON.MOIS1.LIBELLE,
                        LIBELLEFREQUENCE = LeDABON.PERIODICITE1.LIBELLE,
                        //_LeDABON.NUMDEM
                    };

                    return Galatee.Tools.Utility.ListToDataTable(query);
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static DataTable Retournecanalisation(string centre, string client, string ordre)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = null;
                    var _DCANALISATION = context.CANALISATION;

                    query =
                    from _LeDCANALISATION in _DCANALISATION
                    where
                     ((_LeDCANALISATION.CENTRE == centre || string.IsNullOrEmpty(centre)) &&
                      (_LeDCANALISATION.CLIENT == client || string.IsNullOrEmpty(client)))
                    //(_LeDCANALISATION.ORDRERELEVE == Fk_Idcentre )
                    select new
                    {
                        _LeDCANALISATION.CENTRE,
                        _LeDCANALISATION.CLIENT,
                        //_LeDCANALISATION.NUMDEM,
                        _LeDCANALISATION.PRODUIT,
                        _LeDCANALISATION.POINT,
                        _LeDCANALISATION.BRANCHEMENT,
                        _LeDCANALISATION.SURFACTURATION,
                        _LeDCANALISATION.DEBITANNUEL,
                        //_LeDCANALISATION.POSE ,
                        //_LeDCANALISATION.DEPOSE ,
                        _LeDCANALISATION.USERCREATION,
                        _LeDCANALISATION.DATECREATION,
                        _LeDCANALISATION.DATEMODIFICATION,
                        _LeDCANALISATION.USERMODIFICATION,
                        _LeDCANALISATION.PK_ID,
                        _LeDCANALISATION.FK_IDCENTRE,
                        _LeDCANALISATION.FK_IDCOMPTEUR,
                        _LeDCANALISATION.FK_IDPRODUIT,
                        //_LeDCANALISATION.FK_IDDEMANDE ,

                        // Compteur
                        _LeDCANALISATION.COMPTEUR.NUMERO,
                        _LeDCANALISATION.COMPTEUR.TYPECOMPTEUR,
                        //_LeDCANALISATION.COMPTEUR.TYPECOMPTAGE,
                        _LeDCANALISATION.REGLAGECOMPTEUR ,
                        _LeDCANALISATION.COMPTEUR.MARQUE,
                        _LeDCANALISATION.COMPTEUR.COEFLECT,
                        _LeDCANALISATION.COMPTEUR.COEFCOMPTAGE,
                        _LeDCANALISATION.COMPTEUR.CADRAN,
                        _LeDCANALISATION.COMPTEUR.ANNEEFAB,
                        _LeDCANALISATION.COMPTEUR.FONCTIONNEMENT,
                    };
                    return Galatee.Tools.Utility.ListToDataTable(query);
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static DataTable RetourneEvenement(string centre, string client, string ordre)
        {
            try
            {

                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = null;
                    var _DEVENEMENT = context.EVENEMENT;

                    query =
                    from _LeDEVENEMENT in _DEVENEMENT
                    where
                     ((_LeDEVENEMENT.CENTRE == centre || string.IsNullOrEmpty(centre)) &&
                     (_LeDEVENEMENT.CLIENT == client || string.IsNullOrEmpty(client)) &&
                     (_LeDEVENEMENT.ORDRE == ordre))

                    select new
                    {
                        _LeDEVENEMENT.CENTRE,
                        _LeDEVENEMENT.CLIENT,
                        _LeDEVENEMENT.PRODUIT,
                        _LeDEVENEMENT.POINT,
                        _LeDEVENEMENT.NUMEVENEMENT,
                        _LeDEVENEMENT.ORDRE,
                        _LeDEVENEMENT.COMPTEUR,
                        _LeDEVENEMENT.DATEEVT,
                        _LeDEVENEMENT.PERIODE,
                        _LeDEVENEMENT.CODEEVT,
                        _LeDEVENEMENT.INDEXEVT,
                        _LeDEVENEMENT.CAS,
                        _LeDEVENEMENT.ENQUETE,
                        _LeDEVENEMENT.CONSO,
                        _LeDEVENEMENT.CONSONONFACTUREE,
                        _LeDEVENEMENT.LOTRI,
                        _LeDEVENEMENT.FACTURE,
                        _LeDEVENEMENT.SURFACTURATION,
                        _LeDEVENEMENT.STATUS,
                        _LeDEVENEMENT.TYPECONSO,
                        _LeDEVENEMENT.REGLAGECOMPTEUR,
                        _LeDEVENEMENT.MATRICULE,
                        _LeDEVENEMENT.FACPER,
                        _LeDEVENEMENT.QTEAREG,
                        _LeDEVENEMENT.DERPERF,
                        _LeDEVENEMENT.DERPERFN,
                        _LeDEVENEMENT.CONSOFAC,
                        _LeDEVENEMENT.REGIMPUTE,
                        _LeDEVENEMENT.REGCONSO,
                        _LeDEVENEMENT.COEFLECT,
                        _LeDEVENEMENT.COEFCOMPTAGE,
                        _LeDEVENEMENT.PUISSANCE,
                        _LeDEVENEMENT.TYPECOMPTAGE,
                        _LeDEVENEMENT.TYPECOMPTEUR,
                        _LeDEVENEMENT.COEFK1,
                        _LeDEVENEMENT.COEFK2,
                        _LeDEVENEMENT.COEFFAC,
                        _LeDEVENEMENT.USERCREATION,
                        _LeDEVENEMENT.DATECREATION,
                        _LeDEVENEMENT.DATEMODIFICATION,
                        _LeDEVENEMENT.USERMODIFICATION,
                        _LeDEVENEMENT.PK_ID,
                        //_LeDEVENEMENT.NUMDEM,
                        //_LeDEVENEMENT.  FK_IDABON  ,
                        _LeDEVENEMENT.FK_IDCENTRE,
                        _LeDEVENEMENT.FK_IDPRODUIT,
                        //_LeDEVENEMENT.FK_IDDEMANDE
                    };
                    return Galatee.Tools.Utility.ListToDataTable(query);
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static DataTable RetourneAG(string pCentre, string pClient, string pOrdre)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = null;
                    var _DAG = context.AG;

                    query =
                    from _LeDAG in _DAG

                    where
                     ((_LeDAG.CENTRE == pCentre || string.IsNullOrEmpty(pCentre)) &&
                      (_LeDAG.CLIENT == pClient || string.IsNullOrEmpty(pClient)) &&
                      (_LeDAG.CLIENT1.OrderByDescending(c => c.ORDRE).FirstOrDefault().ORDRE == pOrdre))
                    select new
                    {

                        _LeDAG.ETAGE,
                        _LeDAG.CENTRE,
                        _LeDAG.CLIENT,
                        _LeDAG.NOMP,
                        _LeDAG.COMMUNE,
                        _LeDAG.QUARTIER,
                        _LeDAG.RUE,
                        _LeDAG.PORTE,
                        _LeDAG.CADR,
                        _LeDAG.REGROU,
                        _LeDAG.CPARC,
                        _LeDAG.DMAJ,
                        _LeDAG.TOURNEE,
                        _LeDAG.ORDTOUR,
                        _LeDAG.SECTEUR,
                        _LeDAG.CPOS,
                        _LeDAG.TELEPHONE,
                        _LeDAG.FAX,
                        _LeDAG.EMAIL,
                        _LeDAG.USERCREATION,
                        _LeDAG.DATECREATION,
                        _LeDAG.DATEMODIFICATION,
                        _LeDAG.USERMODIFICATION,
                        _LeDAG.PK_ID,
                        _LeDAG.FK_IDTOURNEE,
                        _LeDAG.FK_IDQUARTIER,
                        LIBELLEQUARTIER = _LeDAG.QUARTIER1.LIBELLE,
                        _LeDAG.FK_IDCOMMUNE,
                        LIBELLECOMMUNE = _LeDAG.COMMUNE1.LIBELLE,
                        _LeDAG.FK_IDRUE,
                        LIBELLERUE = _LeDAG.RUES.LIBELLE,
                        _LeDAG.FK_IDCENTRE,
                        LIBELLESECTEUR = _LeDAG.SECTEUR1.LIBELLE,
                        _LeDAG.FK_IDSECTEUR
                    };
                    return Galatee.Tools.Utility.ListToDataTable(query);
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static DataTable RetourneBrt(string centre, string client, string ordre)
        {
            //cmd.CommandText = "SPX_GUI_RETOURNE_DABON";
            try
            {

                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = null;
                    var _DBRT = context.BRT;
                    var DBRT = context.DBRT;
                    query =
                    from _LeDBRT in _DBRT
                    join LeDBRT in DBRT on new { _LeDBRT.CENTRE, _LeDBRT.CLIENT, _LeDBRT.AG.CLIENT1.OrderByDescending(c => c.ORDRE).FirstOrDefault().ORDRE } equals new { LeDBRT.CENTRE, LeDBRT.CLIENT, LeDBRT.DEMANDE.ORDRE }
                    where
                     ((_LeDBRT.CENTRE == centre || string.IsNullOrEmpty(centre)) &&
                      (_LeDBRT.CLIENT == client || string.IsNullOrEmpty(client)) &&
                      (_LeDBRT.AG.CLIENT1.OrderByDescending(c => c.ORDRE).FirstOrDefault().ORDRE == ordre))
                    select new
                    {
                        _LeDBRT.CENTRE,
                        _LeDBRT.CLIENT,
                        _LeDBRT.PRODUIT,
                        //_LeDBRT.TOURNEE,
                        //_LeDBRT.ORDTOUR,
                        _LeDBRT.DRAC,
                        _LeDBRT.DRES,
                        _LeDBRT.SERVICE,
                        _LeDBRT.CATBRT,
                        _LeDBRT.DIAMBRT,
                        _LeDBRT.LONGBRT,
                        _LeDBRT.NATBRT,
                        _LeDBRT.NBPOINT,
                        _LeDBRT.RESEAU,
                        _LeDBRT.TRONCON,
                        _LeDBRT.DMAJ,
                        _LeDBRT.PUISSANCEINSTALLEE,
                        _LeDBRT.PERTES,
                        _LeDBRT.COEFPERTES,
                        _LeDBRT.APPTRANSFO,
                        _LeDBRT.CODEBRT,
                        _LeDBRT.CODEPOSTE,
                        _LeDBRT.NOMBRETRANSFORMATEUR ,
                        _LeDBRT.ANFAB,
                        _LeDBRT.LONGITUDE,
                        _LeDBRT.LATITUDE,
                        _LeDBRT.ADRESSERESEAU,
                        _LeDBRT.USERCREATION,
                        _LeDBRT.DATECREATION,
                        _LeDBRT.DATEMODIFICATION,
                        _LeDBRT.USERMODIFICATION,
                        _LeDBRT.PK_ID,
                        _LeDBRT.FK_IDCENTRE,
                        _LeDBRT.FK_IDPRODUIT,
                        //_LeDBRT.FK_IDTOURNEE,
                        _LeDBRT.FK_IDTYPEBRANCHEMENT,
                        LIBELLETYPEBRANCHEMENT = _LeDBRT.TYPEBRANCHEMENT.LIBELLE,

                        _LeDBRT.FK_IDPOSTESOURCE,
                        CODEPOSTESOURCE = _LeDBRT.POSTESOURCE.CODE,
                        LIBELLEPOSTESOURCE = _LeDBRT.POSTESOURCE.LIBELLE ,

                        _LeDBRT.FK_IDDEPARTHTA,
                        CODEDEPARTHTA = _LeDBRT.DEPARTHTA.CODE,
                        LIBELLEDEPARTHTA= _LeDBRT.DEPARTHTA.LIBELLE ,

                        _LeDBRT.FK_IDPOSTETRANSFORMATION,
                        CODETRANSFORMATEUR = _LeDBRT.POSTETRANSFORMATION.CODE,
                        LIBELLETRANSFORMATEUR = _LeDBRT.POSTETRANSFORMATION.LIBELLE ,

                        _LeDBRT.FK_IDQUARTIER,
                        CODEQUARTIER = _LeDBRT.QUARTIER.CODE,
                        LIBELLEQUARTIER = _LeDBRT.QUARTIER.LIBELLE ,

                        _LeDBRT.FK_IDDEPARTBT,
                        CODEDEPARTBT = _LeDBRT.DEPARTBT.CODE,
                        LIBELLEDEPARTBT = _LeDBRT.DEPARTBT.LIBELLE ,

                        _LeDBRT.NEOUDFINAL, 
                        //_LeDBRT.NUMDEM
                    };
                    return Galatee.Tools.Utility.ListToDataTable(query);
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static DataTable RetourneClient(string centre, string client, string ordre)
        {
            //cmd.CommandText = "SPX_GUI_RETOURNE_DABON";
            try
            {

                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = null;
                    var _DCLIENT = context.CLIENT;
                    var DCLIENT = context.DCLIENT;
                    query =
                    from _LeDCLIENT in _DCLIENT
                    join LeDCLIENT in DCLIENT on new { _LeDCLIENT.CENTRE, _LeDCLIENT.REFCLIENT, _LeDCLIENT.ORDRE } equals new { LeDCLIENT.CENTRE, LeDCLIENT.REFCLIENT, LeDCLIENT.ORDRE }
                    where
                     ((_LeDCLIENT.CENTRE == centre || string.IsNullOrEmpty(centre)) &&
                     (_LeDCLIENT.REFCLIENT == client || string.IsNullOrEmpty(client)) &&
                     (_LeDCLIENT.ORDRE == ordre))
                    select new
                    {
                        _LeDCLIENT.EMAIL,
                        _LeDCLIENT.NUMPROPRIETE,
                        _LeDCLIENT.CENTRE,
                        _LeDCLIENT.REFCLIENT,
                        _LeDCLIENT.ORDRE,
                        _LeDCLIENT.DENABON,
                        _LeDCLIENT.NOMABON,
                        _LeDCLIENT.DENMAND,
                        _LeDCLIENT.NOMMAND,
                        _LeDCLIENT.ADRMAND1,
                        _LeDCLIENT.ADRMAND2,
                        _LeDCLIENT.CPOS,
                        _LeDCLIENT.BUREAU,
                        _LeDCLIENT.DINC,
                        _LeDCLIENT.MODEPAIEMENT,
                        _LeDCLIENT.NOMTIT,
                        _LeDCLIENT.BANQUE,
                        _LeDCLIENT.GUICHET,
                        _LeDCLIENT.COMPTE,
                        _LeDCLIENT.RIB,
                        _LeDCLIENT.PROPRIO,
                        _LeDCLIENT.CODECONSO,
                        _LeDCLIENT.CATEGORIE,
                        _LeDCLIENT.CODERELANCE,
                        _LeDCLIENT.NOMCOD,
                        _LeDCLIENT.MOISNAIS,
                        _LeDCLIENT.ANNAIS,
                        //_LeDCLIENT.NUMDEM,
                        _LeDCLIENT.NOMPERE,
                        _LeDCLIENT.NOMMERE,
                        _LeDCLIENT.NATIONNALITE,
                        _LeDCLIENT.CNI,
                        _LeDCLIENT.TELEPHONE,
                        _LeDCLIENT.MATRICULE,
                        _LeDCLIENT.REGROUPEMENT,
                        _LeDCLIENT.REGEDIT,
                        _LeDCLIENT.FACTURE,
                        _LeDCLIENT.DMAJ,
                        _LeDCLIENT.REFERENCEPUPITRE,
                        _LeDCLIENT.PAYEUR,
                        _LeDCLIENT.SOUSACTIVITE,
                        _LeDCLIENT.AGENTFACTURE,
                        _LeDCLIENT.AGENTRECOUVR,
                        _LeDCLIENT.AGENTASSAINI,
                        _LeDCLIENT.REGROUCONTRAT,
                        _LeDCLIENT.INSPECTION,
                        _LeDCLIENT.REGLEMENT,
                        _LeDCLIENT.DECRET,
                        _LeDCLIENT.CONVENTION,
                        _LeDCLIENT.REFERENCEATM,
                        _LeDCLIENT.PK_ID,
                        _LeDCLIENT.DATECREATION,
                        _LeDCLIENT.DATEMODIFICATION,
                        _LeDCLIENT.USERCREATION,
                        _LeDCLIENT.USERMODIFICATION,
                        _LeDCLIENT.FK_IDMODEPAIEMENT,
                        _LeDCLIENT.FK_IDCODECONSO,
                        _LeDCLIENT.FK_IDCATEGORIE,
                        LIBELLECATEGORIE = _LeDCLIENT.CATEGORIECLIENT.LIBELLE,
                        _LeDCLIENT.FK_IDRELANCE,
                        _LeDCLIENT.FK_IDPIECEIDENTITE,
                        LIBELLETYPEPIECE = _LeDCLIENT.PIECEIDENTITE.LIBELLE,
                        _LeDCLIENT.NUMEROPIECEIDENTITE,
                        _LeDCLIENT.FK_IDNATIONALITE,
                        _LeDCLIENT.FK_IDREGROUPEMENT,
                        //_LeDCLIENT.FK_IDDEMANDE,
                        _LeDCLIENT.FK_IDCENTRE,

                        //Les champ ci-dessous sont A completer dans la table CLIENT

                        LeDCLIENT.FK_TYPECLIENT,
                        LeDCLIENT.FK_IDUSAGE,
                        LeDCLIENT.FAX,
                        LeDCLIENT.BOITEPOSTAL
                    };

                    return Galatee.Tools.Utility.ListToDataTable(query);
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
       

        public static DataTable RetourneTypeBranchementProduit(int fk_idProduit)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = null;
                    var _LstParametrageProduit = context.PARAMETREBRANCHEMENT;
                    query =
                    from _LeParametrageProduit in _LstParametrageProduit
                    where _LeParametrageProduit.PRODUIT1.PK_ID == fk_idProduit 
                    select new
                    {
                        _LeParametrageProduit.CENTRE,
                        _LeParametrageProduit.PRODUIT,
                        _LeParametrageProduit.NBPOINTSMAXI,
                        _LeParametrageProduit.GESTIONTRANSFO,
                        _LeParametrageProduit.MODESAISIEINDEX,
                        _LeParametrageProduit.USERCREATION,
                        _LeParametrageProduit.DATECREATION,
                        _LeParametrageProduit.DATEMODIFICATION,
                        _LeParametrageProduit.USERMODIFICATION,
                        _LeParametrageProduit.PK_ID,
                        _LeParametrageProduit.FK_IDPRODUIT,
                        _LeParametrageProduit.FK_IDCENTRE
                    };

                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static bool MisAjourDAg(DEMANDE _LaDemandeExiste, DAG pEntite, galadbEntities pContext)
        {
            try
            {
                bool Resultat = true;
                DAG _LeAg = new DAG();
                using (galadbEntities Context1 = new galadbEntities())
                {
                    _LeAg = Context1.DAG.FirstOrDefault(p => p.NUMDEM == pEntite.NUMDEM);
                };
                if (_LeAg != null)
                {
                    pEntite.PK_ID = _LeAg.PK_ID;
                    Resultat = Entities.UpdateEntity<DAG>(pEntite);
                }
                else
                {
                    pEntite.FK_IDDEMANDE = _LaDemandeExiste.PK_ID;
                    Resultat = Entities.InsertEntity<DAG>(pEntite, pContext);
                }
                return Resultat;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public static bool MisAjourDAbon(DEMANDE _LaDemandeExiste, DABON pEntite, galadbEntities pContext)
        {
            try
            {
                bool Resultat = true;
                DABON _LeAbon = new DABON();
                galadbEntities Context1 = new galadbEntities();
                _LeAbon = Context1.DABON.FirstOrDefault(p => p.NUMDEM == pEntite.NUMDEM);

                if (_LeAbon != null)
                {
                    pEntite.PK_ID = _LeAbon.PK_ID;
                    pEntite.FK_IDDEMANDE = _LaDemandeExiste.PK_ID;
                    Resultat = Entities.UpdateEntity<DABON>(pEntite, pContext);
                }
                else
                {
                    pEntite.FK_IDDEMANDE = _LaDemandeExiste.PK_ID;
                    Resultat = Entities.InsertEntity<DABON>(pEntite, pContext);
                }
                return Resultat;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public static bool MisAjourDbrt(DEMANDE _LaDemandeExiste, DBRT pEntite, galadbEntities pContext)
        {
            try
            {
                bool Resultat = true;
                DBRT _Lebrt = new DBRT();
                galadbEntities Context1 = new galadbEntities();
                _Lebrt = Context1.DBRT.FirstOrDefault(p => p.NUMDEM == pEntite.NUMDEM);

                if (_Lebrt != null)
                {
                    pEntite.PK_ID = _Lebrt.PK_ID;
                    pEntite.FK_IDDEMANDE = _LaDemandeExiste.PK_ID;
                    Resultat = Entities.UpdateEntity<DBRT>(pEntite, pContext);
                }
                else
                {
                    pEntite.FK_IDDEMANDE = _LaDemandeExiste.PK_ID;
                    Resultat = Entities.InsertEntity<DBRT>(pEntite, pContext);
                }
                return Resultat;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public static bool MisAjourDClient(DEMANDE _LaDemandeExiste, DCLIENT pEntite, galadbEntities pContext)
        {
            try
            {
                bool Resultat = true;
                DCLIENT _LeDclient = new DCLIENT();
                galadbEntities Context1 = new galadbEntities();
                _LeDclient = Context1.DCLIENT.FirstOrDefault(p => p.NUMDEM == pEntite.NUMDEM);

                if (_LeDclient != null)
                {
                    pEntite.PK_ID = _LeDclient.PK_ID;
                    Resultat = Entities.UpdateEntity<DCLIENT>(pEntite, pContext);
                }
                else
                {
                    pEntite.FK_IDDEMANDE = _LaDemandeExiste.PK_ID;
                    Resultat = Entities.InsertEntity<DCLIENT>(pEntite, pContext);
                }
                Context1.Dispose();
                return Resultat;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public static bool MisAjourDCanalisation(DEMANDE _LaDemandeExiste, List<DCANALISATION> _LesCanalisation, galadbEntities pContext)
        {
            try
            {
                galadbEntities Context1 = new galadbEntities();
                List<DCANALISATION> _LeDCanal = Context1.DCANALISATION.Where(p => p.FK_IDDEMANDE  == _LaDemandeExiste.PK_ID ).ToList();
                if (_LeDCanal != null && _LeDCanal.Count != 0)
                {
                    _LesCanalisation.ForEach(p => p.FK_IDDEMANDE = _LaDemandeExiste.PK_ID);
                    foreach (DCANALISATION item in _LesCanalisation)
                        item.PK_ID = _LeDCanal.FirstOrDefault(t => t.POINT == item.POINT).PK_ID;

                    Entities.UpdateEntity<DCANALISATION>(_LesCanalisation, pContext);
                }
                else
                {
                    _LesCanalisation.ForEach(p => p.FK_IDDEMANDE = _LaDemandeExiste.PK_ID);
                    Entities.InsertEntity<DCANALISATION>(_LesCanalisation, pContext);
                    //DABON _LeAbon = Context1.DABON.FirstOrDefault(p => p.FK_IDDEMANDE  == _LaDemandeExiste.PK_ID );
                    //if (_LeAbon != null && !string.IsNullOrEmpty(_LeAbon.NUMDEM))
                    //{
                    //    foreach (COMPTEUR item in _LesCompteur)
                    //    {
                    //        CsCanalisation _LeCompteurBrut = _LesCompteurBrut.FirstOrDefault(t => t.NUMERO == item.NUMERO && t.MARQUE == item.MARQUE);
                    //        DCANALISATION leCann = _LesCanalisation.FirstOrDefault(t => t.POINT == _LeCompteurBrut.POINT);
                    //        DEVENEMENT leEvt = _LesEvenement.FirstOrDefault(t => t.POINT == _LeCompteurBrut.POINT);
                    //        item.DCANALISATION.Add(leCann);
                    //    }
                    //    Entities.InsertEntity<COMPTEUR>(_LesCompteur, pContext);
                    //}
                }
                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static bool MisAjourDepannage(DEMANDE _LaDemandeExiste, DEPANNAGE  pEntite, galadbEntities pContext)
        {
            try
            {
                bool Resultat = true;
                DEPANNAGE _Ledepannage = new DEPANNAGE();
                galadbEntities Context1 = new galadbEntities();
                _Ledepannage = Context1.DEPANNAGE.FirstOrDefault(p => p.FK_IDDEMANDE == _LaDemandeExiste.PK_ID && pEntite.PK_ID == p.PK_ID);

                if (_Ledepannage != null)
                {
                    pEntite.PK_ID = _Ledepannage.PK_ID;
                    pEntite.FK_IDDEMANDE = _LaDemandeExiste.PK_ID;
                    Resultat = Entities.UpdateEntity<DEPANNAGE>(pEntite, pContext);
                }
                else
                {
                    pEntite.FK_IDDEMANDE = _LaDemandeExiste.PK_ID;
                    Resultat = Entities.InsertEntity<DEPANNAGE>(pEntite, pContext);
                }
                return Resultat;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static bool MisAjourDCompteur(DEMANDE _LaDemandeExiste, List<DCOMPTEUR> pEntite, galadbEntities pContext)
        {
            try
            {
                galadbEntities Context1 = new galadbEntities();
                List<DCOMPTEUR> _LeDCanal = Context1.DCOMPTEUR.Where(p => p.FK_IDDEMANDE == _LaDemandeExiste.PK_ID).ToList();
                if (_LeDCanal != null && _LeDCanal.Count != 0)
                {
                    pEntite.ForEach(p => p.FK_IDDEMANDE = _LaDemandeExiste.PK_ID);
                    Entities.UpdateEntity<DCOMPTEUR>(pEntite, pContext);
                }
                else
                {
                    pEntite.ForEach(p => p.FK_IDDEMANDE = _LaDemandeExiste.PK_ID);
                    Entities.InsertEntity<DCOMPTEUR>(pEntite, pContext);
                }
                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public static bool MisAjourAnnotationDemande(DEMANDE _LaDemandeExiste, DANNOTATION  pEntite, galadbEntities pContext)
        {
            try
            {
                bool Resultat = true;
                DANNOTATION _LeAnnotation = new DANNOTATION();
                galadbEntities Context1 = new galadbEntities();
                _LeAnnotation = Context1.DANNOTATION.FirstOrDefault(p => p.FK_IDDEMANDE == _LaDemandeExiste.PK_ID && pEntite.PK_ID == p.PK_ID);

                if (_LeAnnotation != null)
                {
                    pEntite.PK_ID = _LeAnnotation.PK_ID;
                    pEntite.FK_IDDEMANDE = _LaDemandeExiste.PK_ID;
                    Resultat = Entities.UpdateEntity<DANNOTATION>(pEntite, pContext);
                }
                else
                {
                    pEntite.FK_IDDEMANDE = _LaDemandeExiste.PK_ID;
                    Resultat = Entities.InsertEntity<DANNOTATION>(pEntite, pContext);
                }
                return Resultat;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public static bool MisAjourControlTravaux(DEMANDE _LaDemandeExiste, CONTROLETRAVAUX  pEntite, galadbEntities pContext)
        {
            try
            {
                bool Resultat = true;
                CONTROLETRAVAUX _LeControl = new CONTROLETRAVAUX();
                galadbEntities Context1 = new galadbEntities();
                _LeControl = Context1.CONTROLETRAVAUX.FirstOrDefault(p => p.FK_IDDEMANDE == _LaDemandeExiste.PK_ID && pEntite.PK_ID==p.PK_ID );

                if (_LeControl != null)
                {
                    pEntite.PK_ID = _LeControl.PK_ID;
                    pEntite.FK_IDDEMANDE = _LaDemandeExiste.PK_ID;
                    Resultat = Entities.UpdateEntity<CONTROLETRAVAUX>(pEntite, pContext);
                }
                else
                {
                    pEntite.FK_IDDEMANDE = _LaDemandeExiste.PK_ID;
                    Resultat = Entities.InsertEntity<CONTROLETRAVAUX>(pEntite, pContext);
                }
                return Resultat;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public static bool MisAjourDOrdreTravail(DEMANDE _LaDemandeExiste, DORDRETRAVAIL  pEntite, galadbEntities pContext)
        {
            try
            {
                bool Resultat = true;
                DORDRETRAVAIL _LeOT= new DORDRETRAVAIL();
                galadbEntities Context1 = new galadbEntities();
                _LeOT = Context1.DORDRETRAVAIL.FirstOrDefault(p => p.FK_IDDEMANDE == _LaDemandeExiste.PK_ID);

                if (_LeOT != null)
                {
                    pEntite.PK_ID = _LeOT.PK_ID;
                    pEntite.FK_IDDEMANDE = _LaDemandeExiste.PK_ID;
                    Resultat = Entities.UpdateEntity<DORDRETRAVAIL>(pEntite, pContext);
                }
                else
                {
                    pEntite.FK_IDDEMANDE = _LaDemandeExiste.PK_ID;
                    Resultat = Entities.InsertEntity<DORDRETRAVAIL>(pEntite, pContext);
                }
                return Resultat;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        //public static bool MisAjourEltDevis(DEMANDE _LaDemandeExiste, List<ELEMENTDEVIS> LstEntite, galadbEntities pContext)
        //{
        //    try
        //    {
        //        bool Resultat = true;
        //        List<ELEMENTDEVIS> _LeseltDevis = new List<ELEMENTDEVIS>();
        //        using (galadbEntities Context1 = new galadbEntities())
        //        {
        //            var eltdevis = Context1.ELEMENTDEVIS.Where(p => p.NUMDEM == _LaDemandeExiste.NUMDEM && p.FK_IDDEMANDE == _LaDemandeExiste.PK_ID);
        //            _LeseltDevis = eltdevis != null ? eltdevis.ToList() : null;
        //        }
        //        if (_LeseltDevis != null && _LeseltDevis.Count != 0)
        //        {
        //            var lstAjout = LstEntite.Where(t => !_LeseltDevis.Any(y => y.FK_IDMATERIELDEVIS == t.FK_IDMATERIELDEVIS && y.FK_IDRUBRIQUEDEVIS == t.FK_IDRUBRIQUEDEVIS && t.NUMFOURNITURE == y.NUMFOURNITURE));
        //            List<ELEMENTDEVIS> LstAAjouter = new List<ELEMENTDEVIS>();
        //            if (lstAjout != null)
        //                LstAAjouter = lstAjout.ToList();

        //            var lstSupprim = _LeseltDevis.Where(t => !LstEntite.Any(y => y.FK_IDMATERIELDEVIS == t.FK_IDMATERIELDEVIS && y.FK_IDRUBRIQUEDEVIS == t.FK_IDRUBRIQUEDEVIS && t.NUMFOURNITURE == y.NUMFOURNITURE));
        //            List<ELEMENTDEVIS> LstASupprimer = lstSupprim != null ? lstSupprim.ToList() : new List<ELEMENTDEVIS>();

        //            if (lstSupprim != null)
        //                Resultat = Entities.DeleteEntity<ELEMENTDEVIS>(LstASupprimer, pContext);

        //            if (LstAAjouter != null)
        //            {
        //                LstAAjouter.ForEach(t => t.FK_IDDEMANDE = _LaDemandeExiste.PK_ID);
        //                Resultat = Entities.InsertEntity<ELEMENTDEVIS>(LstAAjouter, pContext);
        //            }
        //            var LstAMettreAJour = _LeseltDevis.Where(t => LstEntite.Any(y => y.FK_IDMATERIELDEVIS == t.FK_IDMATERIELDEVIS && y.FK_IDRUBRIQUEDEVIS == t.FK_IDRUBRIQUEDEVIS && t.NUMFOURNITURE == y.NUMFOURNITURE));
        //            if (LstAMettreAJour != null)
        //                Resultat = Entities.UpdateEntity<ELEMENTDEVIS>(LstAMettreAJour.ToList(), pContext);
        //        }
        //        else
        //        {
        //            LstEntite.ForEach(t => t.FK_IDDEMANDE = _LaDemandeExiste.PK_ID);
        //            Resultat = Entities.InsertEntity<ELEMENTDEVIS>(LstEntite, pContext);
        //        }
        //        return Resultat;
        //    }
        //    catch (Exception e)
        //    {
        //        throw e;
        //    }
        //}


        public static bool MisAjourEltDevis(DEMANDE _LaDemandeExiste, List<ELEMENTDEVIS> LstEntite, galadbEntities pContext)
        {
            try
            {
                bool Resultat = true;
                List<ELEMENTDEVIS> _LeseltDevis = new List<ELEMENTDEVIS>();
                using (galadbEntities Context1 = new galadbEntities())
                {
                    var eltdevis = Context1.ELEMENTDEVIS.Where(p => p.NUMDEM == _LaDemandeExiste.NUMDEM && p.FK_IDDEMANDE == _LaDemandeExiste.PK_ID);
                    _LeseltDevis = eltdevis != null ? eltdevis.ToList() : null;
                }
                if (_LeseltDevis != null && _LeseltDevis.Count != 0)
                {
                    Resultat = Entities.DeleteEntity<ELEMENTDEVIS>(_LeseltDevis, pContext);

                    LstEntite.ForEach(t => t.FK_IDDEMANDE = _LaDemandeExiste.PK_ID);
                    Resultat = Entities.InsertEntity<ELEMENTDEVIS>(LstEntite, pContext);
                }
                else
                {
                    LstEntite.ForEach(t => t.FK_IDDEMANDE = _LaDemandeExiste.PK_ID);
                    Resultat = Entities.InsertEntity<ELEMENTDEVIS>(LstEntite, pContext);
                }
                return Resultat;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static bool MisAjourEltAppareil(DEMANDE _LaDemandeExiste, List<APPAREILSDEVIS> LstEntite, galadbEntities pContext)
        {
            try
            {
                bool Resultat = true;
                List<APPAREILSDEVIS> _LesAppareilDevis = new List<APPAREILSDEVIS>();
                using (galadbEntities Context1 = new galadbEntities())
                {
                    _LesAppareilDevis = Context1.APPAREILSDEVIS.Where(p => p.NUMDEM  == _LaDemandeExiste.NUMDEM && p.FK_IDDEMANDE  == _LaDemandeExiste.PK_ID).ToList();
                }
                if (_LesAppareilDevis != null)
                {
                    List<string> codeAppareil = new List<string>();
                    foreach (var item in _LesAppareilDevis)
                        codeAppareil.Add(item.CODEAPPAREIL);

                    List<string> codeAppareilFin = new List<string>();
                    foreach (var item in LstEntite)
                        codeAppareilFin.Add(item.CODEAPPAREIL);

                    List<APPAREILSDEVIS> LstASupprimer = LstEntite.Where(t => !codeAppareil.Contains(t.CODEAPPAREIL)).ToList();
                    List<APPAREILSDEVIS> LstAAjouter = _LesAppareilDevis.Where(t => !codeAppareilFin.Contains(t.CODEAPPAREIL)).ToList();
                    Resultat = Entities.DeleteEntity<APPAREILSDEVIS>(LstASupprimer, pContext);
                    LstAAjouter.ForEach(t => t.FK_IDDEMANDE  = _LaDemandeExiste.PK_ID);
                    Resultat = Entities.InsertEntity<APPAREILSDEVIS>(LstAAjouter, pContext);
                }
                else
                {
                    LstEntite.ForEach(t => t.FK_IDDEMANDE  = _LaDemandeExiste.PK_ID);
                    Resultat = Entities.InsertEntity<APPAREILSDEVIS>(LstEntite, pContext);
                }
                return Resultat;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public static bool MisAjourFraixParticipation(DEMANDE _LaDemandeExiste, List<FRAIXPARICIPATIONDEVIS> LstEntite, galadbEntities pContext)
        {
            try
            {
                bool Resultat = true;
                List<FRAIXPARICIPATIONDEVIS> _LstFraixParticipation = new List<FRAIXPARICIPATIONDEVIS>();
                using (galadbEntities Context1 = new galadbEntities())
                {
                    _LstFraixParticipation = Context1.FRAIXPARICIPATIONDEVIS.Where(p => p.FK_IDDEMANDE == _LaDemandeExiste.PK_ID).ToList();
                }
                if (_LstFraixParticipation != null && _LstFraixParticipation.Count > 0)
                {
                    Resultat = Entities.DeleteEntity<FRAIXPARICIPATIONDEVIS>(_LstFraixParticipation, pContext);
                    LstEntite.ForEach(t => t.FK_IDDEMANDE = _LaDemandeExiste.PK_ID);
                    Resultat = Entities.InsertEntity<FRAIXPARICIPATIONDEVIS>(LstEntite, pContext);
                }
                else
                {
                    LstEntite.ForEach(t => t.FK_IDDEMANDE = _LaDemandeExiste.PK_ID);
                    Resultat = Entities.InsertEntity<FRAIXPARICIPATIONDEVIS>(LstEntite, pContext);
                }
                return Resultat;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static bool MisAjourOrganeScelleDemande(DEMANDE _LaDemandeExiste, List<POSE_SCELLE_DEMANDE> LstEntite, galadbEntities pContext)
        {
            try
            {
                bool Resultat = true;
                List<POSE_SCELLE_DEMANDE> _LstOrganeScelleDemande = new List<POSE_SCELLE_DEMANDE>();
                List<Scelles> ScelleAMettreAjour = new List<Scelles>();
                List<string> LstNumeroScelle = new List<string>();
                using (galadbEntities Context1 = new galadbEntities())
                {
                    foreach (POSE_SCELLE_DEMANDE item in LstEntite)
                        LstNumeroScelle.Add(item.NUM_SCELLE);

                    _LstOrganeScelleDemande = Context1.POSE_SCELLE_DEMANDE.Where(p => p.FK_IDDEMANDE == _LaDemandeExiste.PK_ID).ToList();
                    ScelleAMettreAjour = pContext.Scelles.Where(s => LstNumeroScelle.Contains(s.Numero_Scelle)).ToList();
                    ScelleAMettreAjour.ForEach(s => s.Status_ID = 5);
                }
                if (_LstOrganeScelleDemande != null && _LstOrganeScelleDemande.Count > 0)
                {
                    Resultat = Entities.DeleteEntity<POSE_SCELLE_DEMANDE>(_LstOrganeScelleDemande, pContext);
                    LstEntite.ForEach(t => t.FK_IDDEMANDE = _LaDemandeExiste.PK_ID);
                    Resultat = Entities.InsertEntity<POSE_SCELLE_DEMANDE>(LstEntite, pContext);
                }
                else
                {
                    LstEntite.ForEach(t => t.FK_IDDEMANDE = _LaDemandeExiste.PK_ID);
                    Resultat = Entities.InsertEntity<POSE_SCELLE_DEMANDE>(LstEntite, pContext);
                }
                #region MisajourScelle
         
                Entities.UpdateEntity<Scelles>(ScelleAMettreAjour, pContext);
                #endregion

                return Resultat;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static bool MisAjourDepannageDemande(DEMANDE _LaDemandeExiste, DEPANNAGE  pEntite, galadbEntities pContext)
        {
            try
            {
                bool Resultat = true;
                DEPANNAGE _LeOT = new DEPANNAGE();
                galadbEntities Context1 = new galadbEntities();
                _LeOT = Context1.DEPANNAGE.FirstOrDefault(p => p.FK_IDDEMANDE == _LaDemandeExiste.PK_ID);

                if (_LeOT != null)
                {
                    pEntite.PK_ID = _LeOT.PK_ID;
                    pEntite.FK_IDDEMANDE = _LaDemandeExiste.PK_ID;
                    Resultat = Entities.UpdateEntity<DEPANNAGE>(pEntite, pContext);
                }
                else
                {
                    pEntite.FK_IDDEMANDE = _LaDemandeExiste.PK_ID;
                    Resultat = Entities.InsertEntity<DEPANNAGE>(pEntite, pContext);
                }
                return Resultat;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static bool MisAjourTravaux(DEMANDE _LaDemandeExiste, TRAVAUXDEVIS pEntite, galadbEntities pContext)
        {
            try
            {
                bool Resultat = true;
                TRAVAUXDEVIS _Letrvx = new TRAVAUXDEVIS();
                galadbEntities Context1 = new galadbEntities();
                _Letrvx = Context1.TRAVAUXDEVIS.FirstOrDefault(p => p.NUMDEM == pEntite.NUMDEM);
                pEntite.NUMDEM = pEntite.NUMDEM;
                if (_Letrvx != null)
                {
                    pEntite.PK_ID = _Letrvx.PK_ID;
                    Resultat = Entities.UpdateEntity<TRAVAUXDEVIS>(pEntite, pContext);
                }
                else
                {
                    pEntite.FK_IDDEMANDE = _LaDemandeExiste.PK_ID;
                    Resultat = Entities.InsertEntity<TRAVAUXDEVIS>(pEntite, pContext);
                }
                Context1.Dispose();
                return Resultat;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        //public static bool MisAjourDEvenement(DEMANDE _LaDemandeExiste, List<DEVENEMENT> LstEntite, galadbEntities pContext)
        //{
        //    try
        //    {
        //        bool Resultat = true;
        //        string NumDemande = LstEntite[0].NUMDEM;
        //        DEVENEMENT _LeDEvenement = new DEVENEMENT();
        //        using (galadbEntities Context1 = new galadbEntities())
        //        {
        //        //    _LeDEvenement = Context1.DEVENEMENT.Where(p => p.NUMDEM == NumDemande);
        //        //}
        //        //if (_LeDEvenement != null)
        //        //{
        //        //    LstEntite.ForEach(t => t.PK_ID = _LeDEvenement.PK_ID);
        //        //    Resultat = Entities.UpdateEntity<DEVENEMENT>(LstEntite, pContext);
        //        //}
        //        //else
        //        //{
        //        //    LstEntite.ForEach(t => t.FK_IDNUMDEM = _LaDemandeExiste.PK_ID);
        //        //    Resultat = Entities.InsertEntity<DEVENEMENT>(LstEntite, pContext);
        //        //}
        //        List<DEVENEMENT> ev = Context1.DEVENEMENT.Where (e => e.FK_IDDEMANDE  == _LaDemandeExiste.PK_ID ).ToList();
        //        foreach (var item in LstEntite)
        //        {
        //            if (ev!=null)
        //            {
        //                DEVENEMENT evtPoint = ev.FirstOrDefault(t => t.POINT == item.POINT);
        //                if (evtPoint != null && item.PK_ID == 0) item.PK_ID = evtPoint.PK_ID;
        //                Resultat = Entities.UpdateEntity<DEVENEMENT>(item, pContext) && Resultat;
        //            }
        //            else
        //            {
        //                LstEntite.ForEach(t => t.FK_IDDEMANDE = _LaDemandeExiste.PK_ID);
        //                Resultat = Entities.InsertEntity<DEVENEMENT>(item, pContext) && Resultat;
        //            }
        //        }
        //        }
        //        return Resultat;
        //    }
        //    catch (Exception e)
        //    {
        //        throw e;
        //    }
        //}

        public static bool MisAjourDEvenement(DEMANDE _LaDemandeExiste, List<DEVENEMENT> LstEntite, galadbEntities pContext)
        {
            try
            {
                bool Resultat = true;
                string NumDemande = LstEntite[0].NUMDEM;
                DEVENEMENT _LeDEvenement = new DEVENEMENT();
                using (galadbEntities Context1 = new galadbEntities())
                {
                    //    _LeDEvenement = Context1.DEVENEMENT.Where(p => p.NUMDEM == NumDemande);
                    //}
                    //if (_LeDEvenement != null)
                    //{
                    //    LstEntite.ForEach(t => t.PK_ID = _LeDEvenement.PK_ID);
                    //    Resultat = Entities.UpdateEntity<DEVENEMENT>(LstEntite, pContext);
                    //}
                    //else
                    //{
                    //    LstEntite.ForEach(t => t.FK_IDNUMDEM = _LaDemandeExiste.PK_ID);
                    //    Resultat = Entities.InsertEntity<DEVENEMENT>(LstEntite, pContext);
                    //}

                    foreach (var item in LstEntite)
                    {
                        var ev = Context1.DEVENEMENT.FirstOrDefault(e => e.PK_ID == item.PK_ID);
                        if (ev != null)
                        {

                            Resultat = Entities.UpdateEntity<DEVENEMENT>(item, pContext) && Resultat;
                        }
                        else
                        {
                            LstEntite.ForEach(t => t.FK_IDDEMANDE = _LaDemandeExiste.PK_ID);
                            Resultat = Entities.InsertEntity<DEVENEMENT>(item, pContext) && Resultat;
                        }
                    }
                }
                return Resultat;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public static bool MisAjourDsociete(DEMANDE _LaDemandeExiste, DSOCIETEPRIVE pEntite, galadbEntities pContext)
        {
            try
            {
                bool Resultat = true;
                DSOCIETEPRIVE _LeDsociete = new DSOCIETEPRIVE();
                galadbEntities Context1 = new galadbEntities();
                _LeDsociete = Context1.DSOCIETEPRIVE.FirstOrDefault(p => p.FK_IDDEMANDE == _LaDemandeExiste.PK_ID);

                if (_LeDsociete != null)
                {
                    pEntite.PK_ID = _LeDsociete.PK_ID;
                    pEntite.FK_IDDEMANDE = _LaDemandeExiste.PK_ID;
                    Resultat = Entities.UpdateEntity<DSOCIETEPRIVE>(pEntite, pContext);
                }
                else
                {
                    pEntite.FK_IDDEMANDE = _LaDemandeExiste.PK_ID;
                    Resultat = Entities.InsertEntity<DSOCIETEPRIVE>(pEntite, pContext);
                }
                return Resultat;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static bool MisAjourDAdministration(DEMANDE _LaDemandeExiste, DADMINISTRATION_INSTITUT  pEntite, galadbEntities pContext)
        {
            try
            {
                bool Resultat = true;
                DADMINISTRATION_INSTITUT _LeDAdministration= new DADMINISTRATION_INSTITUT();
                galadbEntities Context1 = new galadbEntities();
                _LeDAdministration = Context1.DADMINISTRATION_INSTITUT.FirstOrDefault(p => p.FK_IDDEMANDE == _LaDemandeExiste.PK_ID);

                if (_LeDAdministration != null)
                {
                    pEntite.PK_ID = _LeDAdministration.PK_ID;
                    pEntite.FK_IDDEMANDE = _LaDemandeExiste.PK_ID;
                    Resultat = Entities.UpdateEntity<DADMINISTRATION_INSTITUT>(pEntite, pContext);
                }
                else
                {
                    pEntite.FK_IDDEMANDE = _LaDemandeExiste.PK_ID;
                    Resultat = Entities.InsertEntity<DADMINISTRATION_INSTITUT>(pEntite, pContext);
                }
                return Resultat;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static bool MisAjourDPersonnePhysique(DEMANDE _LaDemandeExiste, DPERSONNEPHYSIQUE  pEntite, galadbEntities pContext)
        {
            try
            {
                bool Resultat = true;
                DPERSONNEPHYSIQUE _LeDpersonnephysique = new DPERSONNEPHYSIQUE();
                galadbEntities Context1 = new galadbEntities();
                _LeDpersonnephysique = Context1.DPERSONNEPHYSIQUE.FirstOrDefault(p => p.FK_IDDEMANDE == _LaDemandeExiste.PK_ID);

                if (_LeDpersonnephysique != null)
                {
                    pEntite.PK_ID = _LeDpersonnephysique.PK_ID;
                    pEntite.FK_IDDEMANDE = _LaDemandeExiste.PK_ID;
                    Resultat = Entities.UpdateEntity<DPERSONNEPHYSIQUE>(pEntite, pContext);
                }
                else
                {
                    pEntite.FK_IDDEMANDE = _LaDemandeExiste.PK_ID;
                    Resultat = Entities.InsertEntity<DPERSONNEPHYSIQUE>(pEntite, pContext);
                }
                return Resultat;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static bool MisAjourDInfoProprietaire(DEMANDE _LaDemandeExiste, DINFOPROPRIETAIRE  pEntite, galadbEntities pContext)
        {
            try
            {
                bool Resultat = true;
                DINFOPROPRIETAIRE _LeDInfoProprietaire = new DINFOPROPRIETAIRE();
                galadbEntities Context1 = new galadbEntities();
                _LeDInfoProprietaire = Context1.DINFOPROPRIETAIRE.FirstOrDefault(p => p.FK_IDDEMANDE == _LaDemandeExiste.PK_ID);

                if (_LeDInfoProprietaire != null)
                {
                    pEntite.PK_ID = _LeDInfoProprietaire.PK_ID;
                    pEntite.FK_IDDEMANDE = _LaDemandeExiste.PK_ID;
                    Resultat = Entities.UpdateEntity<DINFOPROPRIETAIRE>(pEntite, pContext);
                }
                else
                {
                    pEntite.FK_IDDEMANDE = _LaDemandeExiste.PK_ID;
                    Resultat = Entities.InsertEntity<DINFOPROPRIETAIRE>(pEntite, pContext);
                }
                return Resultat;
            }
            catch (Exception e)
            {
                throw e;
            }
        }



        //public static bool MisAjourDetailDemande(DEMANDE _LaDemandeExiste, List<RUBRIQUEDEMANDE> LstEntite, galadbEntities pContext)
        //{
        //    try
        //    {
        //        bool Resultat = true;
        //        string NumDemande = LstEntite[0].NUMDEM;
        //        List<RUBRIQUEDEMANDE> _LesCout = new List<RUBRIQUEDEMANDE>();
        //        galadbEntities Context1 = new galadbEntities();
        //            _LesCout = Context1.RUBRIQUEDEMANDE.Where(p => p.NUMDEM == NumDemande).ToList();
        //        if (_LesCout.Count != 0)
        //        {

        //            foreach (RUBRIQUEDEMANDE item in LstEntite)
        //            {
        //               if ( Context1.RUBRIQUEDEMANDE.FirstOrDefault(t => t.PK_ID  == item.PK_ID ))

        //            Resultat = Entities.UpdateEntity<RUBRIQUEDEMANDE>(LstEntite, pContext);
        //            }
        //        }
        //        else
        //        {
        //            foreach (RUBRIQUEDEMANDE item in LstEntite)
        //            {
        //                if (string.IsNullOrEmpty(item.NDOC))
        //                  item.NDOC = NumeroFacture(item.FK_IDCENTRE);

        //                if (string.IsNullOrEmpty(item.REFEM))
        //                    item.REFEM = System.DateTime.Today.Year + System.DateTime.Today.Month.ToString("00");

        //                item.FK_IDDEMANDE = _LaDemandeExiste.PK_ID;
        //            }
        //            Resultat = Entities.InsertEntity<RUBRIQUEDEMANDE>(LstEntite, pContext);
        //        }
        //        return Resultat;
        //    }
        //    catch (Exception e)
        //    {
        //        throw e;
        //    }
        //}
        public static bool MisAjourDetailDemande(DEMANDE _LaDemandeExiste, List<RUBRIQUEDEMANDE> LstEntite, galadbEntities pContext)
        {
            try
            {
                bool Resultat = true;
                List<RUBRIQUEDEMANDE> _LeRubriques = new List<RUBRIQUEDEMANDE>();
                using (galadbEntities Context1 = new galadbEntities())
                {
                    var rubrique = Context1.RUBRIQUEDEMANDE.Where(p => p.NUMDEM == _LaDemandeExiste.NUMDEM && p.FK_IDDEMANDE == _LaDemandeExiste.PK_ID);
                    _LeRubriques = rubrique != null ? rubrique.ToList() : null;
                }
                if (_LeRubriques != null && _LeRubriques.Count != 0)
                {
                    List<string > idRubriqueInit = new List<string >();
                    foreach (var item in _LeRubriques)
                        idRubriqueInit.Add(item.COPER );

                    List<string > idRubriqueFin = new List<string >();
                    foreach (var item in LstEntite)
                        idRubriqueFin.Add(item.COPER );


                    var lstAjout = LstEntite.Where(t => !idRubriqueInit.Contains(t.COPER ));

                    List<RUBRIQUEDEMANDE> LstAAjouter = new List<RUBRIQUEDEMANDE>();
                    if (lstAjout != null)
                    {
                        LstAAjouter = lstAjout.ToList();
                    }

                    var lstSupprim = _LeRubriques.Where(t => !idRubriqueFin.Contains(t.COPER ));
                    List<RUBRIQUEDEMANDE> LstASupprimer = lstSupprim != null ? lstSupprim.ToList() : new List<RUBRIQUEDEMANDE>();

                    if (lstSupprim != null)
                        Resultat = Entities.DeleteEntity<RUBRIQUEDEMANDE>(LstASupprimer, pContext);

                    if (LstAAjouter != null)
                        LstAAjouter.ForEach(t => t.FK_IDDEMANDE = _LaDemandeExiste.PK_ID);

                    List<RUBRIQUEDEMANDE> LstAMettreAJour = LstEntite.Where(t => !lstAjout.Select(e => e.COPER ).Contains(t.COPER ) && !LstASupprimer.Select(elt => elt.COPER ).Contains(t.COPER )).ToList();
                    if (LstAMettreAJour != null)
                    {
                        LstAMettreAJour.ForEach(t => t.PK_ID = _LeRubriques.FirstOrDefault(y => y.COPER == t.COPER).PK_ID);
                        Resultat = Entities.UpdateEntity<RUBRIQUEDEMANDE>(LstAMettreAJour.ToList(), pContext);
                    }
                    Resultat = Entities.InsertEntity<RUBRIQUEDEMANDE>(LstAAjouter, pContext);
                }
                else
                {
                    galadbEntities ctxtint = new galadbEntities();
                    var rubrique = ctxtint.RUBRIQUEDEMANDE.Where(p => p.NUMDEM == _LaDemandeExiste.NUMDEM && p.FK_IDDEMANDE == _LaDemandeExiste.PK_ID);
                    if (_LeRubriques != null && _LeRubriques.Count != 0) return true;

                    if (_LaDemandeExiste.TYPEDEMANDE == Enumere.AugmentationPuissance ||
                        _LaDemandeExiste.TYPEDEMANDE == Enumere.DimunitionPuissance    )
                    {

                        List<LCLIENT> lesFacture = RetourneFactureDemande(LstEntite); 
                        Resultat = Entities.InsertEntity<LCLIENT>(lesFacture, pContext);

                        ABON _LeAbonnement = pContext.ABON.FirstOrDefault(t => t.FK_IDCENTRE == _LaDemandeExiste.FK_IDCENTRE && t.CENTRE == _LaDemandeExiste.CENTRE && t.CLIENT == _LaDemandeExiste.CLIENT && _LaDemandeExiste.ORDRE == t.ORDRE);
                        DataTable dte = AccueilProcedures.RetourneFactureAvanceFromAbon(_LaDemandeExiste.FK_IDCENTRE, _LaDemandeExiste.CENTRE, _LaDemandeExiste.CLIENT, _LaDemandeExiste.ORDRE, _LeAbonnement.DAVANCE.Value , _LeAbonnement.AVANCE.Value);
                        CsLclient lstFactiureAvance = Galatee.Tools.Utility.GetEntityFromQuery<CsLclient>(dte).FirstOrDefault();
                        if (lstFactiureAvance != null && lstFactiureAvance.CLIENT != null)
                        {
                            galadbEntities _LeContextInter = new galadbEntities();
                            TRANSCAISB Avance = Galatee.Tools.Utility.ConvertEntity<TRANSCAISB, CsLclient>(lstFactiureAvance);

                            COPER leCoperRemb = _LeContextInter.COPER.FirstOrDefault(t => t.CODE == Enumere.CoperRAC);

                            Avance.COPER = leCoperRemb.CODE;
                            Avance.FK_IDCOPER = leCoperRemb.PK_ID;
                            Avance.FK_IDLCLIENT = lstFactiureAvance.PK_ID;
                            Avance.USERCREATION = _LaDemandeExiste.MATRICULE;
                            Avance.DATECREATION = System.DateTime.Now;
                            Avance.PK_ID = 0;
                            Avance.DTRANS = System.DateTime.Today ;
                            Entities.InsertEntity<TRANSCAISB>(Avance, pContext);
                        }
                    }
                    else
                    {
                        if (_LaDemandeExiste.TYPEDEMANDE == Enumere.Etalonage ||
                            _LaDemandeExiste.TYPEDEMANDE == Enumere.DepannageMT )
                        {
                            List<LCLIENT> lesFacture = RetourneFactureDemande(LstEntite);
                            Resultat = Entities.InsertEntity<LCLIENT>(lesFacture, pContext);
                        }
                        LstEntite.ForEach(t => t.FK_IDDEMANDE = _LaDemandeExiste.PK_ID);
                    }
                    Entities.InsertEntity<RUBRIQUEDEMANDE>(LstEntite, pContext);
                }
                return Resultat;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public static List<LCLIENT> RetourneFactureBrtEp(List<RUBRIQUEDEMANDE> LesFactureAregle)
        {
            try
            {
                RUBRIQUEDEMANDE laFacture = LesFactureAregle.First();
                galadbEntities ctxInter = new galadbEntities();
                ADMUTILISATEUR leUser = ctxInter.ADMUTILISATEUR.FirstOrDefault(t => t.MATRICULE == laFacture.USERCREATION);
                LIBELLETOP leTop = ctxInter.LIBELLETOP.FirstOrDefault(t => t.CODE == Enumere.TopGuichet);
                CENTRE leCentre = ctxInter.CENTRE.FirstOrDefault(t => t.CODE == laFacture.CENTRE);
                int idCentre = 0;
                if (leCentre != null && string.IsNullOrEmpty(leCentre.CODE))
                    idCentre = leCentre.PK_ID;

                CLIENT leClient = ctxInter.CLIENT.FirstOrDefault(t => t.REFCLIENT == leCentre.COMPTEECLAIRAGEPUBLIC 
                                                                     && t.ORDRE == laFacture.ORDRE);
                List<LCLIENT> lesFactureRetours = new List<LCLIENT>();
                foreach (var FactureAregle in LesFactureAregle)
                {
                    LCLIENT laFactureDemande = new LCLIENT();
                    laFactureDemande.CENTRE = FactureAregle.CENTRE;
                    laFactureDemande.CLIENT = FactureAregle.CLIENT;
                    laFactureDemande.ORDRE = FactureAregle.ORDRE;
                    laFactureDemande.REFEM = FactureAregle.REFEM;
                    laFactureDemande.NDOC = FactureAregle.NDOC;
                    laFactureDemande.COPER = FactureAregle.COPER;
                    laFactureDemande.DENR = System.DateTime.Today;
                    laFactureDemande.EXIG = 0;
                    laFactureDemande.MONTANT = FactureAregle.MONTANTHT + FactureAregle.MONTANTTAXE;
                    laFactureDemande.DC = Enumere.Debit;
                    laFactureDemande.MOISCOMPT = System.DateTime.Today.Year + System.DateTime.Today.Month.ToString("00");
                    laFactureDemande.TOP1 = Enumere.TopGuichet;
                    laFactureDemande.EXIGIBILITE = System.DateTime.Today;
                    laFactureDemande.MATRICULE = FactureAregle.USERCREATION;
                    laFactureDemande.USERCREATION = FactureAregle.USERCREATION;
                    laFactureDemande.DATECREATION = FactureAregle.DATECREATION;
                    laFactureDemande.DATEMODIFICATION = FactureAregle.DATEMODIFICATION;
                    laFactureDemande.USERMODIFICATION = FactureAregle.USERMODIFICATION;
                    laFactureDemande.FK_IDCENTRE = leClient.FK_IDCENTRE;
                    laFactureDemande.FK_IDADMUTILISATEUR = leUser.PK_ID;
                    laFactureDemande.FK_IDCOPER = FactureAregle.FK_IDCOPER;
                    laFactureDemande.FK_IDLIBELLETOP = leTop.PK_ID;
                    laFactureDemande.FK_IDCLIENT = leClient.PK_ID;
                    lesFactureRetours.Add(laFactureDemande);
                }
                return lesFactureRetours;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static List<LCLIENT> RetourneFactureDepannageEp(List<RUBRIQUEDEMANDE> LesFactureAregle)
        {
            try
            {
                RUBRIQUEDEMANDE laFacture = LesFactureAregle.First();
                galadbEntities ctxInter = new galadbEntities();
                ADMUTILISATEUR leUser = ctxInter.ADMUTILISATEUR.FirstOrDefault(t => t.MATRICULE == laFacture.USERCREATION);
                LIBELLETOP leTop = ctxInter.LIBELLETOP.FirstOrDefault(t => t.CODE == Enumere.TopGuichet);
                CENTRE leCentre = ctxInter.CENTRE.FirstOrDefault(t => t.CODE == laFacture.CENTRE);
                int idCentre = 0;
                if (leCentre != null && string.IsNullOrEmpty(leCentre.CODE))
                    idCentre = leCentre.PK_ID;

                CLIENT leClient = ctxInter.CLIENT.FirstOrDefault(t =>
                                                                    //t.FK_IDCENTRE == idCentre 
                                                                    // &&  
                                                                     t.CENTRE == laFacture.CENTRE
                                                                     && t.REFCLIENT == laFacture.CLIENT
                                                                     && t.ORDRE == laFacture.ORDRE);
                List<LCLIENT> lesFactureRetours = new List<LCLIENT>();
                foreach (var FactureAregle in LesFactureAregle)
                {
                    LCLIENT laFactureDemande = new LCLIENT();
                    laFactureDemande.CENTRE = FactureAregle.CENTRE;
                    laFactureDemande.CLIENT = FactureAregle.CLIENT;
                    laFactureDemande.ORDRE = FactureAregle.ORDRE;
                    laFactureDemande.REFEM = FactureAregle.REFEM;
                    laFactureDemande.NDOC = FactureAregle.NDOC;
                    laFactureDemande.COPER = FactureAregle.COPER;
                    laFactureDemande.DENR = System.DateTime.Today;
                    laFactureDemande.EXIG = 0;
                    laFactureDemande.MONTANT = FactureAregle.MONTANTHT + FactureAregle.MONTANTTAXE;
                    laFactureDemande.DC = Enumere.Debit;
                    laFactureDemande.MOISCOMPT = System.DateTime.Today.Year + System.DateTime.Today.Month.ToString("00");
                    laFactureDemande.TOP1 = Enumere.TopGuichet;
                    laFactureDemande.EXIGIBILITE = System.DateTime.Today;
                    laFactureDemande.MATRICULE = FactureAregle.USERCREATION;
                    laFactureDemande.USERCREATION = FactureAregle.USERCREATION;
                    laFactureDemande.DATECREATION = FactureAregle.DATECREATION;
                    laFactureDemande.DATEMODIFICATION = FactureAregle.DATEMODIFICATION;
                    laFactureDemande.USERMODIFICATION = FactureAregle.USERMODIFICATION;
                    laFactureDemande.FK_IDCENTRE = leClient.FK_IDCENTRE;
                    laFactureDemande.FK_IDADMUTILISATEUR = leUser.PK_ID;
                    laFactureDemande.FK_IDCOPER = FactureAregle.FK_IDCOPER;
                    laFactureDemande.FK_IDLIBELLETOP = leTop.PK_ID;
                    laFactureDemande.FK_IDCLIENT = leClient.PK_ID;
                    lesFactureRetours.Add(laFactureDemande);
                }
                return lesFactureRetours;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static  List<LCLIENT> RetourneFactureDemande(List<RUBRIQUEDEMANDE>  LesFactureAregle)
        {
            try
            {
                RUBRIQUEDEMANDE laFacture = LesFactureAregle.First();
                galadbEntities ctxInter = new galadbEntities();
                ADMUTILISATEUR leUser = ctxInter.ADMUTILISATEUR.FirstOrDefault(t => t.MATRICULE == laFacture.USERCREATION);
                LIBELLETOP  leTop = ctxInter.LIBELLETOP.FirstOrDefault(t=>t.CODE  == Enumere.TopGuichet  );
                CLIENT leClient = ctxInter.CLIENT.FirstOrDefault(t => t.FK_IDCENTRE == laFacture.FK_IDCENTRE
                                                                     && t.CENTRE == laFacture.CENTRE
                                                                     && t.REFCLIENT == laFacture.CLIENT
                                                                     && t.ORDRE == laFacture.ORDRE);
                List<LCLIENT> lesFactureRetours = new List<LCLIENT>();
                foreach (var FactureAregle in LesFactureAregle)
                {
                    LCLIENT laFactureDemande = new LCLIENT();
                    laFactureDemande.CENTRE = FactureAregle.CENTRE;
                    laFactureDemande.CLIENT = FactureAregle.CLIENT;
                    laFactureDemande.ORDRE = FactureAregle.ORDRE;
                    laFactureDemande.REFEM = FactureAregle.REFEM;
                    laFactureDemande.NDOC = FactureAregle.NDOC;
                    laFactureDemande.COPER = FactureAregle.COPER;
                    laFactureDemande.DENR = System.DateTime.Today;
                    laFactureDemande.EXIG = 0;
                    laFactureDemande.MONTANT = FactureAregle.MONTANTHT + FactureAregle.MONTANTTAXE;
                    laFactureDemande.DC = Enumere.Debit;
                    laFactureDemande.MOISCOMPT = System.DateTime.Today.Year + System.DateTime.Today.Month.ToString("00");
                    laFactureDemande.TOP1 = Enumere.TopGuichet;
                    laFactureDemande.EXIGIBILITE = System.DateTime.Today;
                    laFactureDemande.MATRICULE = FactureAregle.USERCREATION;
                    laFactureDemande.USERCREATION = FactureAregle.USERCREATION;
                    laFactureDemande.DATECREATION = FactureAregle.DATECREATION;
                    laFactureDemande.DATEMODIFICATION = FactureAregle.DATEMODIFICATION;
                    laFactureDemande.USERMODIFICATION = FactureAregle.USERMODIFICATION;
                    laFactureDemande.FK_IDCENTRE = FactureAregle.FK_IDCENTRE;
                    laFactureDemande.FK_IDADMUTILISATEUR = leUser.PK_ID;
                    laFactureDemande.FK_IDCOPER = FactureAregle.FK_IDCOPER;
                    laFactureDemande.FK_IDLIBELLETOP = leTop.PK_ID;
                    laFactureDemande.FK_IDCLIENT = leClient.PK_ID;
                    lesFactureRetours.Add(laFactureDemande);
                }
                return lesFactureRetours;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static TRANSCAISB RetourneAvance(RUBRIQUEDEMANDE LsFactureAregle)
        {
            try
            {
                TRANSCAISB _LeAvance = new TRANSCAISB();
                galadbEntities _LeContextInter = new galadbEntities();
                List<TRANSCAISB> lstAvance = _LeContextInter.TRANSCAISB.Where(t => t.FK_IDCENTRE == LsFactureAregle.FK_IDCENTRE &&
                                                                             t.CENTRE == LsFactureAregle.CENTRE &&
                                                                             t.CLIENT == LsFactureAregle.CLIENT &&
                                                                             t.ORDRE == LsFactureAregle.ORDRE &&
                                                                             t.LCLIENT.COPER == Enumere.CoperCAU).OrderByDescending(u => u.DATETRANS).ToList();
                if (lstAvance != null && lstAvance.Count != 0)
                {
                    COPER leCoperRemb = _LeContextInter.COPER.FirstOrDefault(t => t.CODE == Enumere.CoperRembAvance);
                    if (leCoperRemb != null)
                    {
                        _LeAvance = lstAvance.First();
                        _LeAvance.COPER = leCoperRemb.CODE;
                        _LeAvance.FK_IDCOPER = leCoperRemb.PK_ID;
                        _LeAvance.DTRANS = System.DateTime.Today;
                    }
                }
                _LeContextInter.Dispose();

                return _LeAvance;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static bool MisAjourSuivieDemande(DEMANDE _LaDemandeExiste, List<SUIVIDEMANDE > LstEntite, galadbEntities pContext)
        {
            try
            {
                bool Resultat = true;
                string NumDemande = LstEntite[0].NUMDEM;
                List<SUIVIDEMANDE> _LesCout = new List<SUIVIDEMANDE>();
                using (galadbEntities Context1 = new galadbEntities())
                {
                    _LesCout = Context1.SUIVIDEMANDE.Where(p => p.NUMDEM == NumDemande).ToList();
                }
                if (_LesCout.Count != 0)
                {
                    foreach (SUIVIDEMANDE item in LstEntite)
                    {
                        if (_LesCout.FirstOrDefault(t=>t.COMMENTAIRE == item.COMMENTAIRE )!= null )
                            continue ;
                        Resultat = Entities.InsertEntity<SUIVIDEMANDE>(item, pContext);
                    }
                }
                else
                {
                    LstEntite.ForEach(t => t.FK_IDDEMANDE = _LaDemandeExiste.PK_ID);
                    Resultat = Entities.InsertEntity<SUIVIDEMANDE>(LstEntite, pContext);
                }
                return Resultat;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static bool ValiderRejetDemande(CsDemandeBase pDemande, galadbEntities pContext)
        {
            try
            {
                bool Resultat = false ;
                galadbEntities _LeContextInter = new galadbEntities();
                DEMANDE _Demande = Entities.ConvertObject<DEMANDE, CsDemandeBase>(pDemande);

                _Demande.FK_IDADMUTILISATEUR = _LeContextInter.ADMUTILISATEUR.FirstOrDefault(p => p.MATRICULE == _Demande.MATRICULE).PK_ID;
                _Demande.FK_IDTYPEDEMANDE = _LeContextInter.TYPEDEMANDE.FirstOrDefault(p => p.CODE == _Demande.TYPEDEMANDE).PK_ID;
                _Demande.FK_IDPRODUIT = _LeContextInter.PRODUIT.FirstOrDefault(p => p.CODE == _Demande.PRODUIT).PK_ID;
                int leIdCentre = pDemande.FK_IDCENTRE;
                int? leIdProduit = pDemande.FK_IDPRODUIT = _Demande.FK_IDPRODUIT;
                    Resultat = Entities.UpdateEntity <DEMANDE>(_Demande, pContext);
                return Resultat;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public static string  GetNumDevis(CsDemandeBase entity)
        {
            string NumeroDepartDevis = "1";
            try
            {
               string  pNumeroDevis = string.Empty;
                galadbEntities context = new galadbEntities();

                var devisenBase = context.CENTRE.FirstOrDefault(t => t.PK_ID == entity.FK_IDCENTRE);
                if (devisenBase != null && !string.IsNullOrWhiteSpace( devisenBase.CODE) )
                {
                    string CodeSite = devisenBase.SITE.CODE;
                    int numDevis = (devisenBase.NUMERODEMANDE .Value + 1);
                    if (numDevis > 0)
                    {
                        pNumeroDevis = numDevis.ToString().PadLeft(7, '0');
                        NumeroDepartDevis = entity.CENTRE + CodeSite + pNumeroDevis;
                    }
                    devisenBase.NUMERODEMANDE++;
                    context.SaveChanges();
                }
                context.Dispose();
                return NumeroDepartDevis;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string GetNumDevis(int id_centre)
        {
            string NumeroDepartDevis = "1";
            try
            {
                string pNumeroDevis = string.Empty;
                galadbEntities context = new galadbEntities();

                var devisenBase = context.CENTRE.FirstOrDefault(t => t.PK_ID == id_centre);
                if (devisenBase != null && !string.IsNullOrWhiteSpace(devisenBase.CODE))
                {
                    string CodeSite = devisenBase.SITE.CODE;
                    int numDevis = (devisenBase.NUMERODEMANDE.Value + 1);
                    if (numDevis > 0)
                    {
                        pNumeroDevis = numDevis.ToString().PadLeft(7, '0');
                        NumeroDepartDevis = devisenBase.CODE + CodeSite + pNumeroDevis;
                    }
                    devisenBase.NUMERODEMANDE++;
                    context.SaveChanges();
                }
                context.Dispose();
                return NumeroDepartDevis;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool MisAjourDemande(CsDemande pDemande, galadbEntities pContext)
        {
            try
            {
                bool Resultat = true;
                bool IsUpdate = false;
                galadbEntities _LeContextInter = new galadbEntities();
                List<RUBRIQUEDEMANDE> _LstDemandeCout = new List<RUBRIQUEDEMANDE>();
                List<int?> _lstIdCompteur = new List<int?>();
                Enumere.Pk_Id_DernierDemandeInserer = 0;
                DEMANDE _Demande = Entities.ConvertObject<DEMANDE, CsDemandeBase>(pDemande.LaDemande);
                DEMANDE _LaDemandeExiste = _LeContextInter.DEMANDE.FirstOrDefault(p => p.CENTRE == _Demande.CENTRE && p.NUMDEM == _Demande.NUMDEM);
                DateTime? dateDEmande = System.DateTime.Now;

                _Demande.FK_IDADMUTILISATEUR = _LeContextInter.ADMUTILISATEUR.FirstOrDefault(p => p.MATRICULE == _Demande.MATRICULE).PK_ID;
                _Demande.DATED = dateDEmande;
                _Demande.FK_IDTYPEDEMANDE = pDemande.LaDemande.FK_IDTYPEDEMANDE;
                if(!string.IsNullOrEmpty(_Demande.PRODUIT)) 
                _Demande.FK_IDPRODUIT = _LeContextInter.PRODUIT.FirstOrDefault(p => p.CODE == _Demande.PRODUIT).PK_ID;


                if (_LaDemandeExiste != null)
                {
                    IsUpdate = true;
                    dateDEmande = _LaDemandeExiste.DATED;
                }
                _Demande.FK_IDTYPEDEMANDE = pDemande.LaDemande.FK_IDTYPEDEMANDE;
                _Demande.DATED = dateDEmande;

                DAG _LeAg = new DAG();
                if (pDemande.Ag != null)
                {
                    _LeAg = Entities.ConvertObject<DAG, CsAg>(pDemande.Ag);
                    if (_LeAg != null)
                        if (IsUpdate)
                            _LeAg.FK_IDDEMANDE = _LaDemandeExiste.PK_ID;
                }

                DABON _LeAbon = new DABON();
                if (pDemande.Abonne != null)
                {
                    _LeAbon = Entities.ConvertObject<DABON, CsAbon>(pDemande.Abonne);
                    if (_LeAbon != null)
                        if (IsUpdate)
                            _LeAbon.FK_IDDEMANDE = _LaDemandeExiste.PK_ID;
                }
                DCLIENT _LeClient = new DCLIENT();

                if (pDemande.LeClient != null)
                {
                    _LeClient = Entities.ConvertObject<DCLIENT, CsClient>(pDemande.LeClient);
                    if (_LeClient != null)
                        if (IsUpdate)
                            _LeClient.FK_IDDEMANDE = _LaDemandeExiste.PK_ID;
                }
                DINFOPROPRIETAIRE _LInfoProprietaire = new DINFOPROPRIETAIRE();
                if (pDemande.InfoProprietaire_ != null)
                {
                    _LInfoProprietaire = Entities.ConvertObject<DINFOPROPRIETAIRE, CsInfoProprietaire>(pDemande.InfoProprietaire_);
                    if (IsUpdate)
                        _LInfoProprietaire.FK_IDDEMANDE = _LaDemandeExiste.PK_ID;
                }
                DPERSONNEPHYSIQUE _LaPersonnePhysique = new DPERSONNEPHYSIQUE();
                if (pDemande.PersonePhysique != null && !string.IsNullOrEmpty(pDemande.PersonePhysique.NOMABON))
                {
                    _LaPersonnePhysique = Entities.ConvertObject<DPERSONNEPHYSIQUE, CsPersonePhysique>(pDemande.PersonePhysique);
                    if (_LaPersonnePhysique != null)
                        if (IsUpdate)
                            _LaPersonnePhysique.FK_IDDEMANDE = _LaDemandeExiste.PK_ID;
                }
                DADMINISTRATION_INSTITUT _LAdinistraionInstitut = new DADMINISTRATION_INSTITUT();
                if (pDemande.AdministrationInstitut  != null && !string.IsNullOrEmpty(pDemande.AdministrationInstitut.NOMABON))
                {
                    _LAdinistraionInstitut = Entities.ConvertObject<DADMINISTRATION_INSTITUT, CsAdministration_Institut>(pDemande.AdministrationInstitut );
                    if (_LAdinistraionInstitut != null)
                        if (IsUpdate)
                            _LAdinistraionInstitut.FK_IDDEMANDE = _LaDemandeExiste.PK_ID;
                }
                DSOCIETEPRIVE _LaSocietePrive = new DSOCIETEPRIVE();
                if (pDemande.SocietePrives != null && !string.IsNullOrEmpty(pDemande.SocietePrives.NOMABON))
                {
                    _LaSocietePrive = Entities.ConvertObject<DSOCIETEPRIVE, CsSocietePrive>(pDemande.SocietePrives);
                    if (_LaSocietePrive != null)
                        if (IsUpdate)
                            _LaSocietePrive.FK_IDDEMANDE = _LaDemandeExiste.PK_ID;
                }
                DBRT _LeBrt = new DBRT();
                if (pDemande.Branchement != null)
                {
                    _LeBrt = Entities.ConvertObject<DBRT, CsBrt>(pDemande.Branchement);
                    if (_LeBrt != null)
                        if (IsUpdate)
                            _LeBrt.FK_IDDEMANDE = _LaDemandeExiste.PK_ID;
                }
                TRAVAUXDEVIS _LeTrvx = new TRAVAUXDEVIS();
                if (pDemande.TravauxDevis != null)
                {
                    _LeTrvx = Entities.ConvertObject<TRAVAUXDEVIS, ObjTRAVAUXDEVIS>(pDemande.TravauxDevis);
                    if (_LeTrvx != null)
                        if (IsUpdate)
                            _LeTrvx.FK_IDDEMANDE = _LaDemandeExiste.PK_ID;
                }
                SUIVIDEMANDE _LesuivisDemande = new SUIVIDEMANDE();
                if (pDemande.SuivisDemande != null)
                {
                    _LesuivisDemande = Entities.ConvertObject<SUIVIDEMANDE, ObjSUIVIDEVIS>(pDemande.SuivisDemande);
                    if (_LesuivisDemande != null)
                        if (IsUpdate)
                            _LesuivisDemande.FK_IDDEMANDE = _LaDemandeExiste.PK_ID;
                }
                List<DCANALISATION> _LesCanaux = new List<DCANALISATION>();
                if (pDemande.LstCanalistion != null && pDemande.LstCanalistion.Count != 0)
                {
                        _LesCanaux = Entities.ConvertObject<DCANALISATION, CsCanalisation>(pDemande.LstCanalistion);
                        if (IsUpdate)
                            _LesCanaux.ForEach(t => t.FK_IDDEMANDE = _LaDemandeExiste.PK_ID);
                    
                }
                List<ELEMENTDEVIS> _LesEltDevis = new List<ELEMENTDEVIS>();
                if (pDemande.EltDevis != null && pDemande.EltDevis.Count != 0)
                {
                    _LesEltDevis = Entities.ConvertObject<ELEMENTDEVIS, ObjELEMENTDEVIS>(pDemande.EltDevis);
                    if (IsUpdate)
                        _LesEltDevis.ForEach(t => t.FK_IDDEMANDE = _LaDemandeExiste.PK_ID);
                }
                List<ELEMENTACHATTIMBRE> _LesEltTimbre = new List<ELEMENTACHATTIMBRE>();
                if (pDemande.LstEltTimbre != null && pDemande.LstEltTimbre.Count != 0)
                {
                    _LesEltTimbre = Entities.ConvertObject<ELEMENTACHATTIMBRE, CsElementAchatTimbre>(pDemande.LstEltTimbre);
                    if (IsUpdate)
                        _LesEltDevis.ForEach(t => t.FK_IDDEMANDE = _LaDemandeExiste.PK_ID);
                }
                List<DCOMPTEUR> _LeDCompteur = new List<DCOMPTEUR>();
                {
                    if (pDemande.LaDemande.TYPEDEMANDE == Enumere.ModificationCompteur ||
                        pDemande.LaDemande.TYPEDEMANDE == Enumere.TransfertSiteNonMigre )
                    {
                        if (pDemande.LstCanalistion != null && pDemande.LstCanalistion.Count != 0)
                        {
                            _LeDCompteur = Entities.ConvertObject<DCOMPTEUR, CsCanalisation>(pDemande.LstCanalistion);
                            _LeDCompteur.ForEach(t => t.FK_IDSTATUTCOMPTEUR = 2);
                            _LeDCompteur.ForEach(t => t.FK_IDETATCOMPTEUR = 1);
                        }
                        if (_LesCanaux != null && _LesCanaux.Count != 0)
                        {
                            DataTable dtcana = AccueilProcedures.RetourneDcanalisationCompteur(pDemande.LaDemande.PK_ID);
                            List<CsCanalisation> lsComp = Entities.GetEntityListFromQuery<CsCanalisation>(dtcana);
                            if (lsComp != null && lsComp.Count != 0)
                            {
                                 List<DCANALISATION> _LeDC = Entities.ConvertObject<DCANALISATION, CsCanalisation>(lsComp);
                                _LesCanaux.Clear();
                                _LesCanaux.AddRange(_LeDC);
                            }
                        }
                    }
                }
                List<DEVENEMENT> _LstEvenement = new List<DEVENEMENT>();
                {
                    if (pDemande.LstEvenement != null && pDemande.LstEvenement.Count != 0)
                    { 
                        _LstEvenement = Entities.ConvertObject<DEVENEMENT, CsEvenement>(pDemande.LstEvenement);
                        _LstEvenement.ForEach(t => t.FK_IDCAS = _LeContextInter.CASIND.FirstOrDefault(c=>c.CODE==t.CAS).PK_ID); 

                    }
                    if (IsUpdate)
                    {
                        _LstEvenement.ForEach(t => t.FK_IDCAS = _LeContextInter.CASIND.FirstOrDefault(c => c.CODE == t.CAS).PK_ID);
                        _LstEvenement.ForEach(t => t.FK_IDDEMANDE  = _LaDemandeExiste.PK_ID); 
                    }
                }
                List<RUBRIQUEDEMANDE> _LstCout = new List<RUBRIQUEDEMANDE>();
                {
                    if (pDemande.LstCoutDemande != null && pDemande.LstCoutDemande.Count != 0)
                    {
                        _LstCout = Entities.ConvertObject<RUBRIQUEDEMANDE, CsDemandeDetailCout>(pDemande.LstCoutDemande);
                        foreach (RUBRIQUEDEMANDE item in _LstCout)
                        {
                            if(string.IsNullOrEmpty( item.NDOC))
                            item.NDOC = NumeroFacture(item.FK_IDCENTRE);

                            if (IsUpdate)
                                item.FK_IDDEMANDE = _LaDemandeExiste.PK_ID;
                        }
                    }
                }
                List<APPAREILSDEVIS> _LstObjetAppareil = new List<APPAREILSDEVIS>();
                {
                    if (pDemande.AppareilDevis != null && pDemande.AppareilDevis.Count != 0)
                        _LstObjetAppareil = Entities.ConvertObject<APPAREILSDEVIS, ObjAPPAREILSDEVIS>(pDemande.AppareilDevis);
                    if (IsUpdate)
                        _LstObjetAppareil.ForEach(t => t.FK_IDDEMANDE  = _LaDemandeExiste.PK_ID);
                }

                List<FRAIXPARICIPATIONDEVIS> _LstFraixParticipation = new List<FRAIXPARICIPATIONDEVIS>();
                if (pDemande.LstFraixParticipation != null && pDemande.LstFraixParticipation.Count != 0)
                {
                    _LstFraixParticipation = new List<FRAIXPARICIPATIONDEVIS>();
                    foreach (var item in pDemande.LstFraixParticipation)
                    {
                        FRAIXPARICIPATIONDEVIS obj = new FRAIXPARICIPATIONDEVIS();
                        obj.ESTEXONERE = item.ESTEXONERE;
                        obj.MONTANT  = item.MONTANT;
                        obj.PREUVE = item.PREUVE;
                        obj.CENTRE = item.CENTRE;
                        obj.ORDRE = item.ORDRE;
                        obj.FK_IDCLIENT = item.FK_IDCLIENT;
                        obj.REF_CLIENT = item.REF_CLIENT;
                        _LstFraixParticipation.Add(obj);
                    }
                }

                List<POSE_SCELLE_DEMANDE> _LstOrganeScelleDemande = new List<POSE_SCELLE_DEMANDE>();
                if (pDemande.LstOrganeScelleDemande != null && pDemande.LstOrganeScelleDemande.Count != 0)
                {
                    _LstOrganeScelleDemande = new List<POSE_SCELLE_DEMANDE>();
                    foreach (var item in pDemande.LstOrganeScelleDemande)
                    {
                        POSE_SCELLE_DEMANDE obj = new POSE_SCELLE_DEMANDE();
                        obj.PK_ID  = item.PK_ID ;
                        obj.FK_IDDEMANDE = item.FK_IDDEMANDE;
                        obj.FK_IDORGANE_SCELLABLE = item.FK_IDORGANE_SCELLABLE;
                        obj.NOMBRE = item.NOMBRE;
                        obj.NUM_SCELLE = item.NUM_SCELLE;
                        obj.CERTIFICAT  = item.CERTIFICAT ;
                        _LstOrganeScelleDemande.Add(obj);
                    }
                }
                if (IsUpdate)
                    _LstOrganeScelleDemande.ForEach(t => t.FK_IDDEMANDE = _LaDemandeExiste.PK_ID);

                DORDRETRAVAIL _LOrdreTravail = new DORDRETRAVAIL();
                if (pDemande.OrdreTravail  != null)
                {
                    _LOrdreTravail = Entities.ConvertObject<DORDRETRAVAIL, CsOrdreTravail>(pDemande.OrdreTravail);
                    if (_LOrdreTravail != null)
                        if (IsUpdate)
                            _LOrdreTravail.FK_IDDEMANDE = _LaDemandeExiste.PK_ID;
                }
                CONTROLETRAVAUX _ControleTravaux = new CONTROLETRAVAUX();
                if (pDemande.LstControleTvx != null && pDemande.LstControleTvx.Count != 0)
                {
                    _ControleTravaux = Entities.ConvertObject<CONTROLETRAVAUX, CsControleTravaux >(pDemande.LstControleTvx.First());
                    if (_ControleTravaux != null)
                        if (IsUpdate)
                            _ControleTravaux.FK_IDDEMANDE = _LaDemandeExiste.PK_ID;
                }
                DANNOTATION _LAnnotation = new DANNOTATION();
                if (pDemande.AnnotationDemande != null)
                {
                    _LAnnotation = Entities.ConvertObject<DANNOTATION, CsAnnotation>(pDemande.AnnotationDemande);
                    if (_LAnnotation != null)
                        if (IsUpdate)
                            _LAnnotation.FK_IDDEMANDE = _LaDemandeExiste.PK_ID;
                }
                DTRANSFERT _LeTransfert = new DTRANSFERT();
                if (pDemande.Transfert  != null)
                {
                    _LeTransfert = Entities.ConvertObject<DTRANSFERT,CsDtransfert >(pDemande.Transfert );
                    if (_LeTransfert != null)
                        if (IsUpdate)
                            _LeTransfert.FK_IDDEMANDE = _LaDemandeExiste.PK_ID;
                }
                DEPANNAGE _LeDepannage = new DEPANNAGE();
                if (pDemande.Depannage  != null)
                {
                    _LeDepannage = Entities.ConvertObject<DEPANNAGE, CsDepannage>(pDemande.Depannage);
                    if (_LeDepannage != null)
                        if (IsUpdate)
                            _LeDepannage.FK_IDDEMANDE = _LaDemandeExiste.PK_ID;
                }

                if (_Demande.DATECREATION == null) _Demande.DATECREATION = System.DateTime.Today;
                if (string.IsNullOrEmpty(_Demande.USERCREATION)) _Demande.USERCREATION = "99999";
                #region Mise a jour
                if (IsUpdate)
                {
                    if (pDemande.ObjetScanne != null && pDemande.ObjetScanne.Count != 0)
                    {
                        foreach (ObjDOCUMENTSCANNE item in pDemande.ObjetScanne)
                        {
                            DOCUMENTSCANNE leDoc = _LeContextInter.DOCUMENTSCANNE.FirstOrDefault(t => t.PK_ID == item.PK_ID);
                            if (leDoc == null)
                            {
                                item.FK_IDDEMANDE = _LaDemandeExiste.PK_ID;
                                InsertDocumentScanne(pContext, item);
                            }
                        }
                    }
                    if (string.IsNullOrEmpty(_Demande.NUMDEM))
                    {
                        _Demande.NUMDEM = pDemande.LaDemande.NUMDEM = _Demande.NUMDEM = GetNumDevis(pDemande.LaDemande);
                        if (_Demande.TYPEDEMANDE == Enumere.BranchementAbonement ||
                                     _Demande.TYPEDEMANDE == Enumere.BranchementSimple ||
                                     _Demande.TYPEDEMANDE == Enumere.BranchementAbonnementMt ||
                        _Demande.TYPEDEMANDE == Enumere.BranchementAbonementExtention ||

                                     _Demande.TYPEDEMANDE == Enumere.BranchementAbonnementEp)
                            _Demande.CLIENT = _Demande.NUMDEM.Substring(2, 11);

                        if (_LeAg != null)
                        {
                            _LeAg.NUMDEM = _Demande.NUMDEM;
                            if (_Demande.TYPEDEMANDE == Enumere.BranchementAbonement ||
                                         _Demande.TYPEDEMANDE == Enumere.BranchementSimple ||
                                         _Demande.TYPEDEMANDE == Enumere.BranchementAbonnementMt ||
                        _Demande.TYPEDEMANDE == Enumere.BranchementAbonementExtention ||

                                         _Demande.TYPEDEMANDE == Enumere.BranchementAbonnementEp )
                                _LeAg.CLIENT = _Demande.CLIENT;

                        }
                        if (_LeClient != null)
                        {
                            _LeClient.NUMDEM = _Demande.NUMDEM;
                            if (_Demande.TYPEDEMANDE == Enumere.BranchementAbonement ||
                                         _Demande.TYPEDEMANDE == Enumere.BranchementSimple ||
                                         _Demande.TYPEDEMANDE == Enumere.BranchementAbonnementMt ||
                                         _Demande.TYPEDEMANDE == Enumere.BranchementAbonementExtention ||
                                         _Demande.TYPEDEMANDE == Enumere.BranchementAbonnementEp)
                                _LeClient.REFCLIENT = _Demande.CLIENT;
                        }
                        if (_LeAbon != null)
                        {
                            _LeAbon.NUMDEM = _Demande.NUMDEM;
                            if (_Demande.TYPEDEMANDE == Enumere.BranchementAbonement ||
                                         _Demande.TYPEDEMANDE == Enumere.BranchementSimple ||
                                         _Demande.TYPEDEMANDE == Enumere.BranchementAbonnementMt ||
                        _Demande.TYPEDEMANDE == Enumere.BranchementAbonementExtention ||

                                         _Demande.TYPEDEMANDE == Enumere.BranchementAbonnementEp)
                                _LeAbon.CLIENT = _Demande.CLIENT;
                        }
                        if (_LeBrt != null)
                        {
                            _LeBrt.NUMDEM = _Demande.NUMDEM;
                            if (_Demande.TYPEDEMANDE == Enumere.BranchementAbonement ||
                                         _Demande.TYPEDEMANDE == Enumere.BranchementSimple ||
                                         _Demande.TYPEDEMANDE == Enumere.BranchementAbonnementMt ||
                        _Demande.TYPEDEMANDE == Enumere.BranchementAbonementExtention ||

                                         _Demande.TYPEDEMANDE == Enumere.BranchementAbonnementEp)
                                _LeBrt.CLIENT = _Demande.CLIENT;
                        }
                        if (_LstEvenement != null)
                        {
                            _LstEvenement.ForEach(t => t.NUMDEM = _Demande.NUMDEM);
                            _LstEvenement.ForEach(t => t.CLIENT = _Demande.CLIENT);
                        }
                        if (_LstCout != null)
                        {
                            _LstCout.ForEach(t => t.NUMDEM = _Demande.NUMDEM);
                            if (_Demande.TYPEDEMANDE == Enumere.BranchementAbonement ||
                                         _Demande.TYPEDEMANDE == Enumere.BranchementSimple ||
                                         _Demande.TYPEDEMANDE == Enumere.BranchementAbonnementMt ||
                        _Demande.TYPEDEMANDE == Enumere.BranchementAbonementExtention ||

                                         _Demande.TYPEDEMANDE == Enumere.BranchementAbonnementEp)
                                _LstCout.ForEach(t => t.CLIENT = _Demande.CLIENT);
                        }
                        if (_LesCanaux != null) _LesCanaux.ForEach(t => t.NUMDEM = _Demande.NUMDEM);
                        if (_LstObjetAppareil != null) _LstObjetAppareil.ForEach(t => t.NUMDEM = _Demande.NUMDEM);
                        if (_LstObjetAppareil != null) _LstObjetAppareil.ForEach(t => t.NUMDEM = _Demande.NUMDEM);

                    }
                    MiseTableTemp(_LaDemandeExiste, _LeClient,
                                  _LeAg, _LeAbon, _LeBrt,
                                  _LstEvenement, _LstCout,
                                  _LesCanaux,_LesEltDevis, _LstObjetAppareil, 
                                  _LeTrvx, _LstFraixParticipation,
                                  _LstOrganeScelleDemande, _LOrdreTravail,
                                  _ControleTravaux, _LeDCompteur, _LesuivisDemande,
                                  _LAnnotation,_LeDepannage,_LaPersonnePhysique,_LaSocietePrive,
                                  _LAdinistraionInstitut,_LInfoProprietaire, pContext);
                    Resultat = Entities.UpdateEntity<DEMANDE>(_Demande, pContext);
                }
                #endregion
                #region Insertion
                else
                {
                    List<DABON> _lstAbon = new List<DABON>();
                    List<DAG> _lstAg = new List<DAG>();
                    List<DCLIENT> _lstClient = new List<DCLIENT>();
                    List<DBRT> _lstbrt = new List<DBRT>();
                    List<DOCUMENTSCANNE> lstDocumentScan = new List<DOCUMENTSCANNE>();
                    List<ELEMENTACHATTIMBRE> lsteltTimbre = new List<ELEMENTACHATTIMBRE>();
                    if (pDemande.ObjetScanne != null && pDemande.ObjetScanne.Count != 0)
                        lstDocumentScan = Entities.ConvertObject<DOCUMENTSCANNE, ObjDOCUMENTSCANNE>(pDemande.ObjetScanne);

                    if (_LesEltTimbre != null && _LesEltTimbre.Count != 0)
                        lsteltTimbre = _LesEltTimbre;

                    if (!string.IsNullOrEmpty(_LeAbon.CENTRE))
                        _lstAbon.Add(_LeAbon);

                    if (!string.IsNullOrEmpty(_LeAg.CENTRE))
                        _lstAg.Add(_LeAg);

                    if (!string.IsNullOrEmpty(_LeClient.CENTRE))
                        _lstClient.Add(_LeClient);

                    if (!string.IsNullOrEmpty(_LeBrt.CENTRE))
                        _lstbrt.Add(_LeBrt);

                    if (string.IsNullOrEmpty(_Demande.NUMDEM))
                    {
                        _Demande.NUMDEM = pDemande.LaDemande.NUMDEM = _Demande.NUMDEM = GetNumDevis(pDemande.LaDemande);
                        if (_Demande.TYPEDEMANDE == Enumere.BranchementAbonement ||
                                _Demande.TYPEDEMANDE == Enumere.BranchementAbonementExtention ||
                                _Demande.TYPEDEMANDE == Enumere.BranchementSimple ||
                                _Demande.TYPEDEMANDE == Enumere.BranchementAbonnementMt ||
                                _Demande.TYPEDEMANDE == Enumere.BranchementAbonnementEp)
                            _Demande.CLIENT =pDemande.LaDemande.CLIENT  = _Demande.NUMDEM.Substring(2, 11);
                        
                        if (_LeAg != null)
                        {
                            _LeAg.NUMDEM = _Demande.NUMDEM;
                            if (_Demande.TYPEDEMANDE == Enumere.BranchementAbonement ||
                                _Demande.TYPEDEMANDE == Enumere.BranchementAbonementExtention ||
                                _Demande.TYPEDEMANDE == Enumere.BranchementSimple ||
                                _Demande.TYPEDEMANDE == Enumere.BranchementAbonnementMt ||
                                _Demande.TYPEDEMANDE == Enumere.BranchementAbonnementEp)
                            _LeAg.CLIENT = _Demande.CLIENT;

                        }
                        if (_LeClient != null)
                        {
                            _LeClient.NUMDEM = _Demande.NUMDEM;
                            if (_Demande.TYPEDEMANDE == Enumere.BranchementAbonement ||
                                _Demande.TYPEDEMANDE == Enumere.BranchementAbonementExtention ||
                                _Demande.TYPEDEMANDE == Enumere.BranchementSimple ||
                                _Demande.TYPEDEMANDE == Enumere.BranchementAbonnementMt ||
                                _Demande.TYPEDEMANDE == Enumere.BranchementAbonnementEp)
                            _LeClient.REFCLIENT = _Demande.CLIENT;
                        }
                        if (_LeAbon != null)
                        {
                            _LeAbon.NUMDEM = _Demande.NUMDEM;
                            if (_Demande.TYPEDEMANDE == Enumere.BranchementAbonement ||
                                _Demande.TYPEDEMANDE == Enumere.BranchementAbonementExtention ||
                                _Demande.TYPEDEMANDE == Enumere.BranchementSimple ||
                                _Demande.TYPEDEMANDE == Enumere.BranchementAbonnementMt ||
                                _Demande.TYPEDEMANDE == Enumere.BranchementAbonnementEp)
                            _LeAbon.CLIENT = _Demande.CLIENT;
                        }
                        if (_LeBrt != null)
                        {
                            _LeBrt.NUMDEM = _Demande.NUMDEM;
                            if (_Demande.TYPEDEMANDE == Enumere.BranchementAbonement ||
                                  _Demande.TYPEDEMANDE == Enumere.BranchementAbonementExtention ||
                                  _Demande.TYPEDEMANDE == Enumere.BranchementSimple ||
                                  _Demande.TYPEDEMANDE == Enumere.BranchementAbonnementMt ||
                                  _Demande.TYPEDEMANDE == Enumere.BranchementAbonnementEp)
                            _LeBrt.CLIENT = _Demande.CLIENT;
                        }
                        if (_LstEvenement != null)
                        {
                            _LstEvenement.ForEach(t => t.NUMDEM  = _Demande.NUMDEM );
                            if (_Demande.TYPEDEMANDE == Enumere.BranchementAbonement ||
                                _Demande.TYPEDEMANDE == Enumere.BranchementAbonementExtention ||
                                _Demande.TYPEDEMANDE == Enumere.BranchementSimple ||
                                _Demande.TYPEDEMANDE == Enumere.BranchementAbonnementMt ||
                                _Demande.TYPEDEMANDE == Enumere.BranchementAbonnementEp)
                            _LstEvenement.ForEach(t => t.CLIENT = _Demande.CLIENT);
                        }
                        if (_LesCanaux != null)
                        {
                            _LesCanaux.ForEach(t => t.NUMDEM = _Demande.NUMDEM);
                            if (_Demande.TYPEDEMANDE == Enumere.BranchementAbonement ||
                                _Demande.TYPEDEMANDE == Enumere.BranchementAbonementExtention ||
                                _Demande.TYPEDEMANDE == Enumere.BranchementSimple ||
                                _Demande.TYPEDEMANDE == Enumere.BranchementAbonnementMt ||
                                _Demande.TYPEDEMANDE == Enumere.BranchementAbonnementEp)
                                _LesCanaux.ForEach(t => t.CLIENT = _Demande.CLIENT);
                        }
                        if (_LesEltDevis != null && _LesEltDevis.Count != 0)
                        {
                            _LesEltDevis.ForEach(t => t.NUMDEM = _Demande.NUMDEM);
                            if (_Demande.TYPEDEMANDE == Enumere.BranchementAbonement ||
                                _Demande.TYPEDEMANDE == Enumere.BranchementAbonementExtention ||
                                _Demande.TYPEDEMANDE == Enumere.BranchementSimple ||
                                _Demande.TYPEDEMANDE == Enumere.BranchementAbonnementMt ||
                                _Demande.TYPEDEMANDE == Enumere.BranchementAbonnementEp)
                                _LesEltDevis.ForEach(t => t.NUMDEM = _Demande.NUMDEM);
                        }
                        if (_LstObjetAppareil != null && _LstObjetAppareil.Count != 0)
                            _LstObjetAppareil.ForEach(t => t.NUMDEM = _Demande.NUMDEM);

                        if (_LstCout != null)
                        {
                            _LstCout.ForEach(t => t.NUMDEM = _Demande.NUMDEM);
                            if (_Demande.TYPEDEMANDE == Enumere.BranchementAbonement ||
                                _Demande.TYPEDEMANDE == Enumere.BranchementAbonementExtention ||
                                _Demande.TYPEDEMANDE == Enumere.BranchementSimple ||
                                _Demande.TYPEDEMANDE == Enumere.BranchementAbonnementMt ||
                                _Demande.TYPEDEMANDE == Enumere.BranchementAbonnementEp)
                            _LstCout.ForEach(t => t.CLIENT = _Demande.CLIENT);
                        }
                        if (_LesCanaux != null) 
                            _LesCanaux.ForEach(t => t.NUMDEM = _Demande.NUMDEM);
                        if (_LstObjetAppareil != null) _LstObjetAppareil.ForEach(t => t.NUMDEM = _Demande.NUMDEM);
                      

                        if (lsteltTimbre != null) lsteltTimbre.ForEach(t => t.NUMDEM = _Demande.NUMDEM);

                    }
                    foreach (DOCUMENTSCANNE item in lstDocumentScan)
                    {
                        if (pContext.DOCUMENTSCANNE.FirstOrDefault(d => d.PK_ID == item.PK_ID) == null)
                        {
                            item.PK_ID = Guid.NewGuid();
                            _Demande.DOCUMENTSCANNE.Add(item);
                        }
                    }

                    foreach (APPAREILSDEVIS item in _LstObjetAppareil)
                        _Demande.APPAREILSDEVIS.Add(item);

                    foreach (FRAIXPARICIPATIONDEVIS item in _LstFraixParticipation)
                        _Demande.FRAIXPARICIPATIONDEVIS.Add(item);

                    foreach (ELEMENTACHATTIMBRE item in lsteltTimbre)
                        _Demande.ELEMENTACHATTIMBRE .Add(item);

                    if (_LeDepannage.FK_IDCOMMUNE != 0 && _LeDepannage.FK_IDCOMMUNE != null)
                        _Demande.DEPANNAGE.Add(_LeDepannage);

                    if (_LaSocietePrive != null && !string.IsNullOrEmpty( _LaSocietePrive.NOMABON))
                        _Demande.DSOCIETEPRIVE.Add(_LaSocietePrive);

                    if (_LAdinistraionInstitut != null && !string.IsNullOrEmpty(_LAdinistraionInstitut.NOMABON))
                        _Demande.DADMINISTRATION_INSTITUT.Add(_LAdinistraionInstitut);

                    if (_LaPersonnePhysique != null && !string.IsNullOrEmpty(_LaPersonnePhysique.NOMABON))
                        _Demande.DPERSONNEPHYSIQUE.Add(_LaPersonnePhysique);

                    if (_LInfoProprietaire != null && _LInfoProprietaire.DATENAISSANCE!=null )
                        _Demande.DINFOPROPRIETAIRE.Add(_LInfoProprietaire);

                    Resultat = Entities.InsertEntity<DEMANDE>(_Demande, pContext);
                    pDemande.LaDemande.PK_ID = _Demande.PK_ID;

                    if (_lstAg.Count != 0 && !string.IsNullOrEmpty(_lstAg[0].CENTRE))
                    {
                        _LeAg.FK_IDDEMANDE = _Demande.PK_ID;
                        Entities.InsertEntity<DAG>(_lstAg, pContext);
                    }


                    if (_lstClient.Count != 0 && !string.IsNullOrEmpty(_lstClient[0].CENTRE))
                    {
                        _LeClient.FK_IDDEMANDE = _Demande.PK_ID;
                        Entities.InsertEntity<DCLIENT>(_LeClient, pContext);
                    }

                    if (_lstbrt.Count != 0 && !string.IsNullOrEmpty(_lstbrt[0].CENTRE))
                    {
                        _lstbrt.ForEach(t => t.FK_IDDEMANDE = _Demande.PK_ID);
                        Entities.InsertEntity<DBRT>(_lstbrt, pContext);
                    }

                    if (_lstAbon.Count > 0)
                    {
                        _lstAbon.ForEach(t => t.FK_IDDEMANDE = _Demande.PK_ID);
                        Entities.InsertEntity<DABON>(_lstAbon, pContext);
                    }
                    if (_LeDCompteur.Count > 0 && 
                        (pDemande.LaDemande.TYPEDEMANDE == Enumere.ModificationCompteur ||
                        pDemande.LaDemande.TYPEDEMANDE == Enumere.TransfertSiteNonMigre ))
                    {
                        _LeDCompteur.ForEach(t => t.FK_IDDEMANDE = _Demande.PK_ID);
                        Entities.InsertEntity<DCOMPTEUR>(_LeDCompteur, pContext);
                    }
               

                    if (_LstEvenement.Count != 0 && !string.IsNullOrEmpty(_LstEvenement[0].CENTRE))
                    {
                        _LstEvenement.ForEach(t => t.FK_IDDEMANDE = _Demande.PK_ID);
                        Entities.InsertEntity<DEVENEMENT>(_LstEvenement, pContext);
                    }

                    if (_LstCout.Count != 0 && !string.IsNullOrEmpty(_LstCout[0].CENTRE))
                    {
                        _LstCout.ForEach(t => t.FK_IDDEMANDE = _Demande.PK_ID);
                        Entities.InsertEntity<RUBRIQUEDEMANDE>(_LstCout, pContext);
                    }
                    if (_LstFraixParticipation.Count > 0)
                    {
                        _LstFraixParticipation.ForEach(t => t.FK_IDDEMANDE  = _Demande.PK_ID);
                        Entities.InsertEntity<FRAIXPARICIPATIONDEVIS>(_LstFraixParticipation, pContext);
                    }

                    if (_LesuivisDemande != null && !string.IsNullOrEmpty(_LesuivisDemande.COMMENTAIRE ))
                    {
                        _LesuivisDemande.FK_IDDEMANDE = _Demande.PK_ID;
                        Entities.InsertEntity<SUIVIDEMANDE>(_LesuivisDemande, pContext);
                    }
                    if (_LAnnotation != null && !string.IsNullOrEmpty(_LAnnotation.MATRICULE ))
                    {
                        _LAnnotation.FK_IDDEMANDE = _Demande.PK_ID;
                        Entities.InsertEntity<DANNOTATION>(_LAnnotation, pContext);
                    }
                    if (_LeTransfert != null && _LeTransfert.FK_IDCENTREORIGINE !=0 )
                    {
                        _LeTransfert.FK_IDDEMANDE = _Demande.PK_ID;
                        _LeTransfert.NUMDEM  = _Demande.NUMDEM ;
                        Entities.InsertEntity<DTRANSFERT>(_LeTransfert, pContext);
                    }
                    if (_LesCanaux.Count != 0 && !string.IsNullOrEmpty(_LesCanaux[0].CENTRE))
                    {
                        _LesCanaux.ForEach(t => t.FK_IDDEMANDE = _Demande.PK_ID);
                        Entities.InsertEntity<DCANALISATION >(_LesCanaux, pContext);
                    }
                    if (_LesEltDevis.Count != 0  )
                    {
                        _LesEltDevis.ForEach(t => t.FK_IDDEMANDE = _Demande.PK_ID);
                        Entities.InsertEntity<ELEMENTDEVIS >(_LesEltDevis, pContext);
                    }
                    if (_LstObjetAppareil.Count != 0)
                    {
                        _LstObjetAppareil.ForEach(t => t.FK_IDDEMANDE = _Demande.PK_ID);
                        Entities.InsertEntity<APPAREILSDEVIS>(_LstObjetAppareil, pContext);
                    }
                    _LeContextInter.Dispose();
                }
                #endregion
                return Resultat;
            }
            catch (DbEntityValidationException ex)
            {
                GenericEntityExeptionandler(ex);
                return false;
            }
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

        public static bool InsertDocumentScanne(galadbEntities pCommand, ObjDOCUMENTSCANNE entity)
        {
            try
            {
                return Entities.InsertEntity<Galatee.Entity.Model.DOCUMENTSCANNE>(Entities.ConvertObject<Galatee.Entity.Model.DOCUMENTSCANNE, ObjDOCUMENTSCANNE>(entity), pCommand);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static bool UpdateDocumentScanne(galadbEntities pCommand, ObjDOCUMENTSCANNE entity)
        {
            try
            {
                return Entities.UpdateEntity <Galatee.Entity.Model.DOCUMENTSCANNE>(Entities.ConvertObject<Galatee.Entity.Model.DOCUMENTSCANNE, ObjDOCUMENTSCANNE>(entity), pCommand);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static bool MiseAjoursBrtSimple(CsDemande pDemande, galadbEntities pContext)
        {
            try
            {
                bool Resultat = true;
                galadbEntities _LeContextInter = new galadbEntities();
                List<RUBRIQUEDEMANDE> _LstDemandeCout = new List<RUBRIQUEDEMANDE>();
                DEMANDE _Demande = new DEMANDE();
                AG _LeAg = new AG();
                CLIENT _LeClient = new CLIENT();
                if (pDemande.LaDemande != null )
                    _Demande = Entities.ConvertObject<DEMANDE, CsDemandeBase>(pDemande.LaDemande);

                int leIdCentre = pDemande.LaDemande.FK_IDCENTRE;
                int? leIdProduit = pDemande.LaDemande.FK_IDPRODUIT;
                _Demande.FK_IDADMUTILISATEUR = _LeContextInter.ADMUTILISATEUR.FirstOrDefault(p => p.MATRICULE == _Demande.MATRICULE ).PK_ID;
                _Demande.FK_IDTYPEDEMANDE  = _LeContextInter.TYPEDEMANDE.FirstOrDefault(p => p.CODE  == _Demande.TYPEDEMANDE).PK_ID;

                BRT _LeBrt = new BRT();
                if (pDemande.Branchement != null)
                    _LeBrt = Entities.ConvertObject<BRT, CsBrt>(pDemande.Branchement);

                List<LCLIENT> _LstFacture = new List<LCLIENT>();
                List<RUBRIQUEDEMANDE> _LstCout = new List<RUBRIQUEDEMANDE>();
                if (pDemande.LstCoutDemande != null)
                {
                    _LstCout = Entities.ConvertObject<RUBRIQUEDEMANDE, CsDemandeDetailCout>(pDemande.LstCoutDemande);
                    foreach (RUBRIQUEDEMANDE item in _LstCout)
                    {
                        if (string.IsNullOrEmpty(item.NDOC))
                            item.NDOC = NumeroFacture(item.FK_IDCENTRE );
                    }
                }

                if (_LstCout != null && _LstCout.Count != 0)
                    _LstFacture = GetLclientFromCoutDemande(_LstCout);

                if (pDemande.LstCoutDemande != null && pDemande.LstCoutDemande.Count != 0)
                {
                    List<TRANSCAISSE> _lesReglementtranscaisse = _LeContextInter.TRANSCAISSE.Where(p => p.CENTRE == pDemande.LaDemande.CENTRE &&
                             p.CLIENT == pDemande.LaDemande.CLIENT &&
                             p.ORDRE == pDemande.LaDemande.ORDRE).ToList();

                    if (_lesReglementtranscaisse != null && _lesReglementtranscaisse.Count != 0)
                        _LstFacture.AddRange(GetLclientFromTranscaisse(_lesReglementtranscaisse, _LstCout, 0, leIdCentre));
                    else
                    {

                        List<TRANSCAISB> _lesReglementTranscaisB = _LeContextInter.TRANSCAISB.Where(p => p.CENTRE == pDemande.LaDemande.CENTRE &&
                          p.CLIENT == pDemande.LaDemande.CLIENT &&
                          p.ORDRE == pDemande.LaDemande.ORDRE).ToList();
                        if (_lesReglementTranscaisB != null && _lesReglementTranscaisB.Count != 0)
                            _LstFacture.AddRange(GetLclientFromTranscaisB(_lesReglementTranscaisB, _LstCout, 0, leIdCentre));
                    }
                }
     
                if (pDemande.LeClient != null)
                {
                    _LeClient = Entities.ConvertObject<CLIENT, CsClient>(pDemande.LeClient);
                    if (_LstFacture != null && _LstFacture.Count != 0)
                        _LeClient.LCLIENT = _LstFacture;
                }
                if (pDemande.Ag != null)
                {
                    _LeAg = Entities.ConvertObject<AG, CsAg>(pDemande.Ag);
                    _LeAg.CLIENT1.Add(_LeClient);
                    _LeAg.BRT.Add(_LeBrt);
                }
                return Resultat = Entities.InsertEntity<AG>(_LeAg, pContext);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public static List<LCLIENT> GetLclientFromCoutDemande(List<RUBRIQUEDEMANDE> _LstCout)
        {
            galadbEntities _leContext = new galadbEntities();
            List<LCLIENT> _LstsClient = new List<LCLIENT>();
            foreach (RUBRIQUEDEMANDE item in _LstCout)
            {
                LCLIENT _leClient = new LCLIENT()
                {
                    CENTRE = item.CENTRE,
                    CLIENT = item.CLIENT,
                    ORDRE = item.ORDRE,
                    COPER = item.COPER,
                    REFEM = item.REFEM ,
                    TOP1 = Enumere.TopGuichet,
                    MATRICULE = item.USERCREATION,
                    DENR = DateTime.Parse(item.DATECREATION.ToString()),
                    DATEMODIFICATION = item.DATEMODIFICATION,
                    MOISCOMPT = item.DATECREATION.Value.Year + item.DATECREATION.Value.Month.ToString("00"),
                    DATECREATION = item.DATECREATION,
                    MONTANT = item.MONTANTHT + item.MONTANTTAXE,
                    MONTANTTVA = item.MONTANTTAXE,
                    NDOC = item.NDOC,
                    DC = Enumere.Debit,
                    FK_IDADMUTILISATEUR = _leContext.ADMUTILISATEUR.FirstOrDefault(p => p.MATRICULE == item.USERCREATION).PK_ID,
                    FK_IDCOPER = _leContext.COPER.FirstOrDefault(p => p.CODE == item.COPER).PK_ID,
                    FK_IDLIBELLETOP = _leContext.LIBELLETOP.FirstOrDefault(p => p.CODE == Enumere.TopGuichet).PK_ID,
                    FK_IDCENTRE = item.FK_IDCENTRE ,
                };

                //LCLIENT _leClient = new LCLIENT();
               
                //   _leClient.CENTRE = item.CENTRE;
                //   _leClient.CLIENT = item.CLIENT;
                //   _leClient.ORDRE = item.ORDRE;
                //   _leClient.COPER = item.COPER;
                //   _leClient.TOP1 = Enumere.TopGuichet;
                //   _leClient.MATRICULE = item.USERCREATION;
                //   _leClient.DENR = DateTime.Parse(item.DATECREATION.ToString());
                //   _leClient.DATEMODIFICATION = item.DATEMODIFICATION;
                //   _leClient.MOISCOMPT = item.DATECREATION.Value.Year + item.DATECREATION.Value.Month.ToString("00");
                //   _leClient.NATURE = item.NATURE;
                //   _leClient.DATECREATION = item.DATECREATION;
                //   _leClient.MONTANT = item.MONTANTHT + item.MONTANTTAXE;
                //   _leClient.MONTANTTVA = item.MONTANTTAXE;
                //   _leClient.NDOC = item.NDOC;
                //   _leClient.DC = Enumere.Debit;
                //   _leClient.FK_IDADMUTILISATEUR = _leContext.ADMUTILISATEUR.FirstOrDefault(p => p.MATRICULE == item.USERCREATION).PK_ID;
                //   _leClient.FK_IDCOPER = _leContext.COPER.FirstOrDefault(p => p.CODE == item.COPER).PK_ID;
                //   _leClient.FK_IDLIBELLETOP = _leContext.LIBELLETOP.FirstOrDefault(p => p.CODE == Enumere.TopGuichet).PK_ID;
                //   _leClient.FK_IDNATURE = _leContext.NATURE.FirstOrDefault(p => p.CODE == item.NATURE).PK_ID;
                //   _leClient.FK_IDCENTRE = IdCentre;
                //   _leClient.FK_IDCLIENT = IdClient;

                _LstsClient.Add(_leClient);
            }
            _leContext.Dispose();
            return _LstsClient;
        }
        public static List<LCLIENT> GetLclientFromTranscaisB(List<TRANSCAISB> _LstReglement, List<RUBRIQUEDEMANDE> _LstCout, int IdClient, int IdCentre)
        {
            galadbEntities _leContext = new galadbEntities();
            List<LCLIENT> _LstsClient = new List<LCLIENT>();
            foreach (TRANSCAISB item in _LstReglement)
            {
                LCLIENT _leClient = new LCLIENT()
                {
                    CENTRE           = item.CENTRE,
                    CLIENT           = item.CLIENT,
                    ORDRE            = item.ORDRE,
                    COPER            = item.COPER,
                    TOP1             = item.TOP1 ,
                    MATRICULE        = item.MATRICULE ,
                    DENR             = DateTime.Parse(item.DTRANS.ToString()) ,
                    DATEMODIFICATION = item.DATEMODIFICATION ,
                    DATEVALEUR   = item.DATEVALEUR ,
                    MOISCOMPT    = item.MOISCOMPT ,
                    DATECREATION = item.DATECREATION,
                    MONTANT      = item.MONTANT ,
                    NDOC         = item.NDOC ,
                    MODEREG      = item.MODEREG ,
                    NUMCHEQ      = item.NUMCHEQ ,
                    BANQUE       = item.BANQUE ,
                    CAISSE       = item.CAISSE ,
                    DC           = item.DC ,
                    FK_IDADMUTILISATEUR = _leContext.ADMUTILISATEUR.FirstOrDefault(p => p.MATRICULE == item.USERCREATION).PK_ID,
                    FK_IDCOPER          = _leContext.COPER.FirstOrDefault(p => p.CODE == item.COPER).PK_ID,
                    FK_IDLIBELLETOP     = _leContext.LIBELLETOP.FirstOrDefault(p => p.CODE == Enumere.TopGuichet).PK_ID,
                    FK_IDCENTRE         = IdCentre,
                    FK_IDCLIENT         = IdClient
                };
                _LstsClient.Add(_leClient);
            }

            return _LstsClient;
        }
        public static List<LCLIENT> GetLclientFromTranscaisse(List<TRANSCAISSE> _LstReglement, List<RUBRIQUEDEMANDE> _LstCout, int IdClient, int IdCentre)
        {
            galadbEntities _leContext = new galadbEntities();
            List<LCLIENT> _LstsClient = new List<LCLIENT>();
            foreach (TRANSCAISSE item in _LstReglement)
            {
                LCLIENT _leClient = new LCLIENT()
                {
                    CENTRE = item.CENTRE,
                    CLIENT = item.CLIENT,
                    ORDRE = item.ORDRE,
                    REFEM = item.REFEM ,
                    ACQUIT = item.ACQUIT ,
                    COPER = item.COPER,
                    TOP1 = item.TOP1,
                    MATRICULE = item.MATRICULE,
                    DENR = DateTime.Parse(item.DTRANS.ToString()),
                    DATEMODIFICATION = item.DATEMODIFICATION,
                    DATEVALEUR = item.DATEVALEUR,
                    MOISCOMPT = item.MOISCOMPT,
                    DATECREATION = item.DATECREATION,
                    MONTANT = item.MONTANT,
                    NDOC = item.NDOC,
                    MODEREG = item.MODEREG,
                    NUMCHEQ = item.NUMCHEQ,
                    BANQUE = item.BANQUE,
                    CAISSE = item.CAISSE,
                    DC =  item.DC == Enumere.Debit ? Enumere.Credit : Enumere.Debit ,
                    FK_IDADMUTILISATEUR = _leContext.ADMUTILISATEUR.FirstOrDefault(p => p.MATRICULE == item.USERCREATION).PK_ID,
                    FK_IDCOPER = _leContext.COPER.FirstOrDefault(p => p.CODE == item.COPER).PK_ID,
                    FK_IDLIBELLETOP = _leContext.LIBELLETOP.FirstOrDefault(p => p.CODE == Enumere.TopGuichet).PK_ID,
                    FK_IDCENTRE = IdCentre,
                    FK_IDCLIENT = IdClient
                };
                _LstsClient.Add(_leClient);
            }

            return _LstsClient;
        }
        public static void MiseAjoursAbonBrtAbonnementMt(CsDemande pDemande, galadbEntities pContext)
        {
            try
            {
                galadbEntities contextinter = new galadbEntities();
                DEMANDE _Demande = new DEMANDE();
                if (pDemande.LaDemande != null)
                {
                    _Demande = Entities.ConvertObject<DEMANDE, CsDemandeBase>(pDemande.LaDemande);
                    _Demande.DATEFIN = System.DateTime.Today.Date;
                }
                List<DCANALISATION> lesComptDemande = contextinter.DCANALISATION.Where(t => t.FK_IDDEMANDE == pDemande.LaDemande.PK_ID).ToList();

                AG _LeAG = new AG();
                if (pDemande.Ag != null)
                {
                    pDemande.Ag.DATECREATION = System.DateTime.Today.Date;
                    _LeAG = Entities.ConvertObject<AG, CsAg>(pDemande.Ag);
                    _LeAG.FK_IDTOURNEE = contextinter.TOURNEE.FirstOrDefault(t => t.FK_IDCENTRE == pDemande.Ag.FK_IDCENTRE && t.CODE == pDemande.Ag.TOURNEE).PK_ID;
                }

                BRT _LeBrt = new BRT();
                if (pDemande.Branchement != null)
                {
                    pDemande.Branchement.DATECREATION = System.DateTime.Today.Date;

                    _LeBrt = Entities.ConvertObject<BRT, CsBrt>(pDemande.Branchement);
                    _LeBrt.DRAC = System.DateTime.Today;
                    _LeBrt.SERVICE = "1";
                }

                CLIENT _LeClient = new CLIENT();
                if (pDemande.LeClient != null)
                {
                    pDemande.LeClient.DATECREATION = System.DateTime.Today.Date;
                    _LeClient = Entities.ConvertObject<CLIENT, CsClient>(pDemande.LeClient);
                }
                INFOPROPRIETAIRE _LeProprio = new INFOPROPRIETAIRE();
                if (pDemande.InfoProprietaire_ != null && !string.IsNullOrEmpty(pDemande.InfoProprietaire_.NOM))
                    _LeProprio = Entities.ConvertObject<INFOPROPRIETAIRE, CsInfoProprietaire>(pDemande.InfoProprietaire_);

                SOCIETEPRIVE _LaSociete = new SOCIETEPRIVE();
                if (pDemande.SocietePrives != null && !string.IsNullOrEmpty(pDemande.SocietePrives.NOMABON))
                    _LaSociete = Entities.ConvertObject<SOCIETEPRIVE, CsSocietePrive>(pDemande.SocietePrives);

                ADMINISTRATION_INSTITUT _LeInstitue = new ADMINISTRATION_INSTITUT();
                if (pDemande.AdministrationInstitut != null && !string.IsNullOrEmpty(pDemande.AdministrationInstitut.NOMABON))
                    _LeInstitue = Entities.ConvertObject<ADMINISTRATION_INSTITUT, CsAdministration_Institut>(pDemande.AdministrationInstitut);

                PERSONNEPHYSIQUE _LePersonPhysique = new PERSONNEPHYSIQUE();
                if (pDemande.PersonePhysique != null && !string.IsNullOrEmpty(pDemande.PersonePhysique.NOMABON))
                    _LePersonPhysique = Entities.ConvertObject<PERSONNEPHYSIQUE, CsPersonePhysique>(pDemande.PersonePhysique);

                ABON _LeAbon = new ABON();
                if (pDemande.Abonne != null)
                {
                    if (pDemande.Abonne.PK_ID == 0)
                    {
                        DataTable dt = RetourneDAbon(pDemande.LaDemande.PK_ID);
                        CsAbon leAb = Galatee.Tools.Utility.GetEntityFromQuery<CsAbon>(dt).FirstOrDefault();
                        if (leAb != null)
                            pDemande.Abonne.PK_ID = leAb.PK_ID;
                    }
                    pDemande.Abonne.DATECREATION = System.DateTime.Today.Date;
                    _LeAbon = Entities.ConvertObject<ABON, CsAbon>(pDemande.Abonne);
                }
                if (pDemande.LstFraixParticipation != null)
                {
                    List<LCLIENT> _LesFraisParticipationFact = new List<LCLIENT>();
                    foreach (CsFraixParticipation item in pDemande.LstFraixParticipation.Where(t => t.ESTEXONERE == false).ToList())
                    {
                        string NDOC = NumeroFacture(pDemande.LaDemande.PK_ID);
                        List<TRANSCAISB> _LesFraisParticipation = new List<TRANSCAISB>();
                        LCLIENT laFactureParticpation = new LCLIENT()
                        {
                            CENTRE = item.CENTRE,
                            CLIENT = item.REF_CLIENT,
                            ORDRE = item.ORDRE,
                            TOP1 = Enumere.TopGuichet,
                            COPER = Enumere.CoperFact,
                            DC = Enumere.Debit,
                            REFEM = System.DateTime.Today.Year.ToString() + System.DateTime.Today.Month.ToString("00"),
                            NDOC = NDOC,
                            MONTANT = 0,
                            MONTANTTVA = 0,
                            DENR = System.DateTime.Today,
                            DATECREATION = System.DateTime.Today,
                            USERCREATION = pDemande.LaDemande.MATRICULE,
                            FK_IDCENTRE = pDemande.LaDemande.FK_IDCENTRE,
                            FK_IDCLIENT = contextinter.CLIENT.FirstOrDefault(p => p.CENTRE == item.CENTRE && p.REFCLIENT == item.REF_CLIENT && p.ORDRE == item.ORDRE).PK_ID,
                            FK_IDADMUTILISATEUR = contextinter.ADMUTILISATEUR.FirstOrDefault(p => p.MATRICULE == pDemande.LaDemande.MATRICULE).PK_ID,
                            FK_IDCOPER = contextinter.COPER.FirstOrDefault(p => p.CODE == Enumere.CoperFact).PK_ID,
                            FK_IDLIBELLETOP = contextinter.LIBELLETOP.FirstOrDefault(p => p.CODE == Enumere.TopGuichet).PK_ID
                        };
                        TRANSCAISB leFrais = new TRANSCAISB()
                        {
                            CENTRE = item.CENTRE,
                            CLIENT = item.REF_CLIENT,
                            ORDRE = item.ORDRE,
                            FK_IDCENTRE = pDemande.LaDemande.FK_IDCENTRE,
                            COPER = Enumere.CoperFAB,
                            DC = Enumere.Debit,
                            MONTANT = item.MONTANT,
                            TOP1 = Enumere.TopGuichet,
                            REFEM = System.DateTime.Today.Year.ToString() + System.DateTime.Today.Month.ToString("00"),
                            NDOC = NDOC,
                            ACQUIT = Enumere.AcquitLettrageAuto,
                            FK_IDCOPER = contextinter.COPER.FirstOrDefault(p => p.CODE == Enumere.CoperFAB).PK_ID,
                            FK_IDLIBELLETOP = contextinter.LIBELLETOP.FirstOrDefault(p => p.CODE == Enumere.TopGuichet).PK_ID,
                            DATECREATION = System.DateTime.Today,
                            USERCREATION = pDemande.LaDemande.MATRICULE,
                            DTRANS = System.DateTime.Today
                        };
                        laFactureParticpation.TRANSCAISB.Add(leFrais);
                        Entities.InsertEntity<LCLIENT>(laFactureParticpation, pContext);

                    }
                }
                List<COMPTEUR> lstcompteur = new List<COMPTEUR>();
                foreach (var item_ in pDemande.LstCanalistion)
                {
                    CANALISATION leCanal = new CANALISATION();
                    COMPTEUR can = new COMPTEUR();
                    can.ANNEEFAB = item_.ANNEEFAB;
                    can.CADRAN = item_.CADRAN;
                    can.COEFCOMPTAGE = item_.COEFCOMPTAGE;
                    can.COEFLECT = item_.COEFLECT;
                    can.DATECREATION = System.DateTime.Now;
                    can.DATEMODIFICATION = item_.DATEMODIFICATION;
                    can.MISEENSERVICE = System.DateTime.Today.Date;

                    can.FK_IDCALIBRE = item_.FK_IDCALIBRE ;
                    can.FK_IDMARQUECOMPTEUR =item_.FK_IDMARQUECOMPTEUR!= null ? int.Parse(item_.FK_IDMARQUECOMPTEUR.ToString()):1 ;
                    can.FK_IDPRODUIT = item_.FK_IDPRODUIT;
                    can.FK_IDTYPECOMPTEUR = item_.FK_IDTYPECOMPTEUR.Value;
                    can.FK_IDSTATUTCOMPTEUR = 1;
                    can.FK_IDETATCOMPTEUR = 1;
                    can.FONCTIONNEMENT = item_.FONCTIONNEMENT;
                    can.MARQUE = item_.MARQUE;

                    can.NUMERO = item_.NUMERO;
                    can.TYPECOMPTEUR = item_.TYPECOMPTEUR;

                    can.PLOMBAGE = item_.PLOMBAGE;
                    can.PRODUIT = item_.PRODUIT;
                    can.USERCREATION = item_.USERCREATION;
                    can.USERMODIFICATION = item_.USERMODIFICATION;
                    lstcompteur.Add(can);
                }

                List<CANALISATION> _LeCompteur = new List<CANALISATION>();
                if (pDemande.LstCanalistion != null)
                {
                    _LeCompteur = Entities.ConvertObject<CANALISATION, CsCanalisation>(pDemande.LstCanalistion);
                    _LeCompteur.ForEach(t => t.PROPRIO = pDemande.LeClient.PROPRIO);
                    _LeCompteur.ForEach(t => t.FK_IDPROPRIETAIRE = pContext.PROPRIETAIRE.FirstOrDefault(p => p.CODE == pDemande.LeClient.PROPRIO).PK_ID);
                }

                List<EVENEMENT> _LstEvenement = new List<EVENEMENT>();
                if (pDemande.LstEvenement != null)
                    _LstEvenement = Entities.ConvertObject<EVENEMENT, CsEvenement>(pDemande.LstEvenement);
                _LstEvenement.ForEach(t => t.STATUS = 99);

                List<LCLIENT> _LstFacture = new List<LCLIENT>();
                if (pDemande.LstCoutDemande != null && pDemande.LstCoutDemande.Count != 0)
                {
                    if (pDemande.LaDemande.TYPEDEMANDE != Enumere.BranchementAbonnementEp)
                    {
                        foreach (CsDemandeDetailCout item in pDemande.LstCoutDemande)
                        {
                            if (item.COPER == Enumere.CoperCAU)
                            {
                                _LeAbon.AVANCE = (item.MONTANTHT + item.MONTANTTAXE);
                                _LeAbon.DAVANCE = item.DATECREATION;
                            }
                            LCLIENT lelClient = pContext.LCLIENT.FirstOrDefault(t => t.CENTRE == item.CENTRE && t.CLIENT == item.CLIENT && t.ORDRE == item.ORDRE && t.FK_IDCENTRE == item.FK_IDCENTRE && t.NDOC == item.NDOC && t.REFEM == item.REFEM);
                            if (lelClient != null && !string.IsNullOrEmpty(lelClient.CENTRE))
                            {
                                lelClient.FK_IDCLIENT = _LeClient.PK_ID;
                                lelClient.COPER = item.COPER;
                                lelClient.FK_IDCOPER = item.FK_IDCOPER;
                                lelClient.MONTANT = (item.MONTANTHT + item.MONTANTTAXE);
                            }
                        }
                    }
                    else
                    {
                        if (pDemande.LaDemande.ISCOMMUNE == true)
                        {
                            List<LCLIENT> _LesFacture = new List<LCLIENT>();
                            List<RUBRIQUEDEMANDE> lesFacture = contextinter.RUBRIQUEDEMANDE.Where(t => t.FK_IDDEMANDE == pDemande.LaDemande.PK_ID).ToList();
                            if (lesFacture != null)
                            {
                                // Inserer la facture dans le compte de la ville
                                List<LCLIENT> _LesFactureVille = RetourneFactureBrtEp(lesFacture);
                                if (_LesFactureVille != null && _LesFactureVille.Count != 0)
                                    _LesFacture.AddRange(_LesFactureVille);

                                ADMUTILISATEUR leUser = contextinter.ADMUTILISATEUR.FirstOrDefault(t => t.MATRICULE == pDemande.LaDemande.MATRICULE);
                                LIBELLETOP leTop = contextinter.LIBELLETOP.FirstOrDefault(t => t.CODE == Enumere.TopGuichet);
                                List<LCLIENT> lesFactureRetours = new List<LCLIENT>();
                                foreach (var FactureAregle in lesFacture)
                                {
                                    LCLIENT laFactureDemande = new LCLIENT();
                                    laFactureDemande.CENTRE = FactureAregle.CENTRE;
                                    laFactureDemande.CLIENT = FactureAregle.CLIENT;
                                    laFactureDemande.ORDRE = FactureAregle.ORDRE;
                                    laFactureDemande.REFEM = FactureAregle.REFEM;
                                    laFactureDemande.NDOC = FactureAregle.NDOC;
                                    laFactureDemande.COPER = FactureAregle.COPER;
                                    laFactureDemande.DENR = System.DateTime.Today;
                                    laFactureDemande.EXIG = 0;
                                    laFactureDemande.MONTANT = FactureAregle.MONTANTHT + FactureAregle.MONTANTTAXE;
                                    laFactureDemande.DC = Enumere.Debit;
                                    laFactureDemande.MOISCOMPT = System.DateTime.Today.Year + System.DateTime.Today.Month.ToString("00");
                                    laFactureDemande.TOP1 = Enumere.TopGuichet;
                                    laFactureDemande.EXIGIBILITE = System.DateTime.Today;
                                    laFactureDemande.MATRICULE = FactureAregle.USERCREATION;
                                    laFactureDemande.USERCREATION = FactureAregle.USERCREATION;
                                    laFactureDemande.DATECREATION = FactureAregle.DATECREATION;
                                    laFactureDemande.DATEMODIFICATION = FactureAregle.DATEMODIFICATION;
                                    laFactureDemande.USERMODIFICATION = FactureAregle.USERMODIFICATION;
                                    laFactureDemande.FK_IDCENTRE = FactureAregle.FK_IDCENTRE;
                                    laFactureDemande.FK_IDADMUTILISATEUR = leUser.PK_ID;
                                    laFactureDemande.FK_IDCOPER = FactureAregle.FK_IDCOPER;
                                    laFactureDemande.FK_IDLIBELLETOP = leTop.PK_ID;
                                    laFactureDemande.FK_IDCLIENT = _LeClient.PK_ID;
                                    _LesFacture.Add(laFactureDemande);
                                }
                                Entities.InsertEntity<LCLIENT>(_LesFacture, pContext);
                            }
                        }
                    }
                }
                List<POSE_SCELLE_DEMANDE> organeScelleDemande = new List<POSE_SCELLE_DEMANDE>();
                if (pDemande.LaDemande != null)
                {
                    organeScelleDemande = pContext.POSE_SCELLE_DEMANDE.Where(t => t.FK_IDDEMANDE == pDemande.LaDemande.PK_ID).ToList();
                }

                Entities.InsertEntity<AG>(_LeAG, pContext);


                _LeBrt.FK_IDAG = _LeAG.PK_ID;
                Entities.InsertEntity<BRT>(_LeBrt, pContext);

                if (organeScelleDemande != null && organeScelleDemande.Count != 0)
                {
                    organeScelleDemande.ForEach(t => t.FK_IDBRT = _LeBrt.PK_ID);
                    organeScelleDemande.ForEach(t => t.USERCREATION = pDemande.LaDemande.USERCREATION);
                    organeScelleDemande.ForEach(t => t.DATECREATION = System.DateTime.Now);
                }
                _LeClient.FK_IDAG = _LeAG.PK_ID;
                Entities.InsertEntity<CLIENT>(_LeClient, pContext);

                _LeAbon.FK_IDCLIENT = _LeClient.PK_ID;
                Entities.InsertEntity<ABON>(_LeAbon, pContext);

                if (_LaSociete != null && !string.IsNullOrEmpty(_LaSociete.NOMABON))
                {
                    _LaSociete.FK_IDCLIENT = _LeClient.PK_ID;
                    Entities.InsertEntity<SOCIETEPRIVE>(_LaSociete, pContext);
                }
                if (_LeProprio != null && !string.IsNullOrEmpty(_LeProprio.NOM))
                {
                    _LeProprio.FK_IDCLIENT = _LeClient.PK_ID;
                    Entities.InsertEntity<INFOPROPRIETAIRE>(_LeProprio, pContext);
                }
                if (_LeInstitue != null && !string.IsNullOrEmpty(_LeInstitue.NOMABON))
                {
                    _LeInstitue.FK_IDCLIENT = _LeClient.PK_ID;
                    Entities.InsertEntity<ADMINISTRATION_INSTITUT>(_LeInstitue, pContext);
                }
                if (_LePersonPhysique != null && !string.IsNullOrEmpty(_LePersonPhysique.NOMABON))
                {
                    _LePersonPhysique.FK_IDCLIENT = _LeClient.PK_ID;
                    Entities.InsertEntity<PERSONNEPHYSIQUE>(_LePersonPhysique, pContext);
                }
                Entities.InsertEntity<COMPTEUR>(lstcompteur);

                foreach (CsCanalisation item in pDemande.LstCanalistion)
                {
                    CANALISATION leCan = _LeCompteur.FirstOrDefault(t => t.POINT == item.POINT);
                    COMPTEUR leCompteurDuPoint = lstcompteur.FirstOrDefault(t => t.TYPECOMPTEUR == item.TYPECOMPTEUR);
                    leCan.FK_IDCOMPTEUR = leCompteurDuPoint.PK_ID;
                    leCan.FK_IDABON = _LeAbon.PK_ID;
                }
                Entities.InsertEntity<CANALISATION>(_LeCompteur, pContext);


                foreach (EVENEMENT item in _LstEvenement)
                {
                    item.FK_IDABON = _LeAbon.PK_ID;
                    item.FK_IDCANALISATION = _LeCompteur.FirstOrDefault(t => t.POINT == item.POINT).PK_ID;

                    CANALISATION leCan = _LeCompteur.FirstOrDefault(t => t.POINT == item.POINT);
                    COMPTEUR leCompteurDuPoint = lstcompteur.FirstOrDefault(t => t.TYPECOMPTEUR == item.TYPECOMPTEUR);
                    item.FK_IDCOMPTEUR = leCompteurDuPoint.PK_ID;


                    if (pDemande.LaDemande.TYPEDEMANDE == Enumere.BranchementAbonnementEp)
                    {
                        item.TYPETARIF = _LeAbon.TYPETARIF;
                        item.PUISSANCE = _LeAbon.PUISSANCE;
                    }
                }
                Entities.InsertEntity<EVENEMENT>(_LstEvenement, pContext);
            }

            catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            {
                // Retrieve the error messages as a list of strings.
                var errorMessages = ex.EntityValidationErrors
                        .SelectMany(x => x.ValidationErrors)
                        .Select(x => x.ErrorMessage);

                // Join the list to a single string.
                var fullErrorMessage = string.Join("; ", errorMessages);

                // Combine the original exception message with the new one.
                var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

                //return exceptionMessage;
                // Throw a new DbEntityValidationException with the improved exception message.
                throw new System.Data.Entity.Validation.DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);

            }
        }
        public static void MiseAjoursAbonBrtAbonnement(CsDemande pDemande, galadbEntities pContext)
        {
            try
            {
                galadbEntities contextinter = new galadbEntities();
                DEMANDE _Demande = new DEMANDE();
                if (pDemande.LaDemande != null)
                {
                    _Demande = Entities.ConvertObject<DEMANDE, CsDemandeBase>(pDemande.LaDemande);
                    _Demande.DATEFIN = System.DateTime.Today.Date;
                }
                List<DCANALISATION> lesComptDemande = contextinter.DCANALISATION.Where(t => t.FK_IDDEMANDE == pDemande.LaDemande.PK_ID).ToList();

                AG _LeAG = new AG();
                if (pDemande.Ag != null)
                {
                    pDemande.Ag.DATECREATION  = System.DateTime.Today.Date;
                    _LeAG = Entities.ConvertObject<AG, CsAg>(pDemande.Ag);
                    _LeAG.FK_IDTOURNEE = contextinter.TOURNEE.FirstOrDefault(t => t.FK_IDCENTRE == pDemande.Ag.FK_IDCENTRE && t.CODE == pDemande.Ag.TOURNEE).PK_ID;
                }

                BRT _LeBrt = new BRT();
                if (pDemande.Branchement != null)
                {
                    pDemande.Branchement.DATECREATION = System.DateTime.Today.Date;

                    _LeBrt = Entities.ConvertObject<BRT, CsBrt>(pDemande.Branchement);
                    _LeBrt.DRAC = System.DateTime.Today;
                    _LeBrt.SERVICE  = "1";
                }
          
                CLIENT _LeClient = new CLIENT();
                if (pDemande.LeClient != null)
                {
                    pDemande.LeClient.DATECREATION = System.DateTime.Today.Date;
                    _LeClient = Entities.ConvertObject<CLIENT, CsClient>(pDemande.LeClient);
                }
                INFOPROPRIETAIRE _LeProprio = new INFOPROPRIETAIRE();
                if (pDemande.InfoProprietaire_ != null && !string.IsNullOrEmpty(pDemande.InfoProprietaire_.NOM))
                    _LeProprio = Entities.ConvertObject<INFOPROPRIETAIRE, CsInfoProprietaire>(pDemande.InfoProprietaire_);
                
                SOCIETEPRIVE _LaSociete = new SOCIETEPRIVE();
                if (pDemande.SocietePrives != null && !string.IsNullOrEmpty(pDemande.SocietePrives.NOMABON))
                    _LaSociete = Entities.ConvertObject<SOCIETEPRIVE, CsSocietePrive >(pDemande.SocietePrives );

                ADMINISTRATION_INSTITUT _LeInstitue = new ADMINISTRATION_INSTITUT();
                if (pDemande.AdministrationInstitut != null && !string.IsNullOrEmpty(pDemande.AdministrationInstitut.NOMABON))
                    _LeInstitue = Entities.ConvertObject<ADMINISTRATION_INSTITUT, CsAdministration_Institut >(pDemande.AdministrationInstitut);

                PERSONNEPHYSIQUE _LePersonPhysique = new PERSONNEPHYSIQUE();
                if (pDemande.PersonePhysique != null && !string.IsNullOrEmpty(pDemande.PersonePhysique.NOMABON))
                    _LePersonPhysique = Entities.ConvertObject<PERSONNEPHYSIQUE, CsPersonePhysique >(pDemande.PersonePhysique);

                ABON _LeAbon = new ABON();
                if (pDemande.Abonne != null)
                {
                    if (pDemande.Abonne.PK_ID ==0) 
                    {
                        DataTable  dt = RetourneDAbon(pDemande.LaDemande.PK_ID);
                        CsAbon leAb = Galatee.Tools.Utility.GetEntityFromQuery<CsAbon>(dt).FirstOrDefault();
                        if (leAb != null )
                            pDemande.Abonne.PK_ID = leAb.PK_ID ;
                    }
                    pDemande.Abonne.DATECREATION = System.DateTime.Today.Date;
                    _LeAbon = Entities.ConvertObject<ABON, CsAbon>(pDemande.Abonne);
                }
                if (pDemande.LstFraixParticipation != null)
                {
                    List<LCLIENT> _LesFraisParticipationFact = new List<LCLIENT>();
                    foreach (CsFraixParticipation  item in pDemande.LstFraixParticipation.Where(t=>t.ESTEXONERE == false ).ToList())
                    {
                        string NDOC = NumeroFacture(pDemande.LaDemande.PK_ID);
                        List<TRANSCAISB> _LesFraisParticipation = new List<TRANSCAISB>();
                        LCLIENT laFactureParticpation = new LCLIENT() { 
                         CENTRE = item.CENTRE ,
                         CLIENT =item.REF_CLIENT ,
                         ORDRE = item.ORDRE,
                         TOP1 = Enumere.TopGuichet,
                         COPER = Enumere.CoperFact,
                         DC = Enumere.Debit,
                         REFEM = System.DateTime.Today.Year .ToString() + System.DateTime.Today.Month.ToString("00"),
                         NDOC = NDOC,
                         MONTANT = 0,
                         MONTANTTVA = 0,
                         DENR = System.DateTime.Today,
                         DATECREATION  = System.DateTime.Today,
                         USERCREATION = pDemande.LaDemande.MATRICULE,
                         FK_IDCENTRE = pDemande.LaDemande.FK_IDCENTRE,
                         FK_IDCLIENT =  contextinter.CLIENT .FirstOrDefault(p => p.CENTRE  == item.CENTRE && p.REFCLIENT == item.REF_CLIENT && p.ORDRE ==item.ORDRE ).PK_ID,
                         FK_IDADMUTILISATEUR = contextinter.ADMUTILISATEUR.FirstOrDefault(p => p.MATRICULE == pDemande.LaDemande.MATRICULE).PK_ID,
                         FK_IDCOPER = contextinter.COPER.FirstOrDefault(p => p.CODE  == Enumere.CoperFact ).PK_ID,
                         FK_IDLIBELLETOP = contextinter.LIBELLETOP.FirstOrDefault(p => p.CODE == Enumere.TopGuichet).PK_ID
                        };
                        TRANSCAISB leFrais = new TRANSCAISB()
                        {
                            CENTRE = item.CENTRE,
                            CLIENT = item.REF_CLIENT,
                            ORDRE = item.ORDRE,
                            FK_IDCENTRE = pDemande.LaDemande.FK_IDCENTRE,
                            COPER = Enumere.CoperFAB,
                            DC = Enumere.Debit,
                            MONTANT = item.MONTANT ,
                            TOP1 = Enumere.TopGuichet,
                            REFEM = System.DateTime.Today.Year.ToString() + System.DateTime.Today.Month.ToString("00"),
                            NDOC = NDOC ,
                            ACQUIT = Enumere.AcquitLettrageAuto ,
                            FK_IDCOPER = contextinter.COPER.FirstOrDefault(p => p.CODE == Enumere.CoperFAB).PK_ID,
                            FK_IDLIBELLETOP = contextinter.LIBELLETOP.FirstOrDefault(p => p.CODE == Enumere.TopGuichet ).PK_ID,
                            DATECREATION = System.DateTime.Today,
                            USERCREATION = pDemande.LaDemande.MATRICULE,
                            DTRANS = System.DateTime.Today
                        };
                        laFactureParticpation.TRANSCAISB.Add(leFrais);
                        Entities.InsertEntity<LCLIENT>(laFactureParticpation, pContext);

                    }
                }
                List<COMPTEUR> lstcompteur = new List<COMPTEUR>();
                foreach (var item_ in pDemande.LstCanalistion)
                {
                    CANALISATION leCanal = new CANALISATION();
                    MAGASINVIRTUEL item = pContext.MAGASINVIRTUEL.FirstOrDefault(m => m.PK_ID == item_.FK_IDMAGAZINVIRTUEL );
                    COMPTEUR can = new COMPTEUR();
                    can.ANNEEFAB = item.ANNEEFAB;
                    can.CADRAN = item.CADRAN;
                    can.COEFCOMPTAGE = item.COEFCOMPTAGE;
                    can.COEFLECT = item.COEFLECT;
                    can.DATECREATION = System.DateTime.Now ;
                    can.DATEMODIFICATION = item.DATEMODIFICATION;
                    can.MISEENSERVICE = System.DateTime.Today.Date;

                    can.FK_IDCALIBRE = item.FK_IDCALIBRECOMPTEUR;
                    can.FK_IDMARQUECOMPTEUR = item.FK_IDMARQUECOMPTEUR;
                    can.FK_IDPRODUIT = item.FK_IDPRODUIT;
                    can.FK_IDTYPECOMPTEUR = item_.FK_IDTYPECOMPTEUR.Value;
                    can.FK_IDSTATUTCOMPTEUR  = 1;
                    can.FK_IDETATCOMPTEUR = 1;
                    can.FONCTIONNEMENT = item.FONCTIONNEMENT;
                    can.MARQUE = item.MARQUE;

                    can.NUMERO = item_.NUMERO;
                    can.TYPECOMPTEUR = item_.TYPECOMPTEUR;

                    can.PLOMBAGE = item.PLOMBAGE;
                    can.PRODUIT = item_.PRODUIT;
                    can.USERCREATION = item.USERCREATION;
                    can.USERMODIFICATION = item.USERMODIFICATION;
                    lstcompteur.Add(can);
                }

                List<CANALISATION> _LeCompteur = new List<CANALISATION>();
                if (pDemande.LstCanalistion != null)
                {
                    _LeCompteur = Entities.ConvertObject<CANALISATION, CsCanalisation>(pDemande.LstCanalistion);
                    _LeCompteur.ForEach(t => t.PROPRIO = pDemande.LeClient.PROPRIO);
                    _LeCompteur.ForEach(t => t.FK_IDPROPRIETAIRE = pContext.PROPRIETAIRE.FirstOrDefault(p => p.CODE == pDemande.LeClient.PROPRIO).PK_ID);
                }

                List<EVENEMENT> _LstEvenement = new List<EVENEMENT>();
                if (pDemande.LstEvenement != null)
                    _LstEvenement = Entities.ConvertObject<EVENEMENT, CsEvenement>(pDemande.LstEvenement);
                _LstEvenement.ForEach(t => t.STATUS = 99);

                List<LCLIENT> _LstFacture = new List<LCLIENT>();
                if (pDemande.LstCoutDemande != null && pDemande.LstCoutDemande.Count != 0)
                {
                    if (pDemande.LaDemande.TYPEDEMANDE != Enumere.BranchementAbonnementEp)
                    {
                        foreach (CsDemandeDetailCout item in pDemande.LstCoutDemande)
                        {
                            if (item.COPER == Enumere.CoperCAU)
                            {
                                _LeAbon.AVANCE = (item.MONTANTHT + item.MONTANTTAXE);
                                _LeAbon.DAVANCE = item.DATECREATION;
                            }
                            LCLIENT lelClient = pContext.LCLIENT.FirstOrDefault(t => t.CENTRE == item.CENTRE && t.CLIENT == item.CLIENT && t.ORDRE == item.ORDRE && t.FK_IDCENTRE == item.FK_IDCENTRE && t.NDOC == item.NDOC && t.REFEM == item.REFEM);
                            if (lelClient != null && !string.IsNullOrEmpty(lelClient.CENTRE))
                            {
                                lelClient.FK_IDCLIENT = _LeClient.PK_ID;
                                lelClient.COPER = item.COPER;
                                lelClient.FK_IDCOPER = item.FK_IDCOPER;
                                lelClient.MONTANT = (item.MONTANTHT + item.MONTANTTAXE);
                            }
                        }
                    }
                    else
                    {
                        if (pDemande.LaDemande.ISCOMMUNE == true)
                        {
                            List<LCLIENT> _LesFacture = new List<LCLIENT>();
                            List<RUBRIQUEDEMANDE> lesFacture = contextinter.RUBRIQUEDEMANDE.Where(t => t.FK_IDDEMANDE == pDemande.LaDemande.PK_ID).ToList();
                            if (lesFacture != null)
                            {
                                // Inserer la facture dans le compte de la ville
                                List<LCLIENT> _LesFactureVille = RetourneFactureBrtEp(lesFacture);
                                if (_LesFactureVille != null && _LesFactureVille.Count != 0)
                                    _LesFacture.AddRange(_LesFactureVille);

                                ADMUTILISATEUR leUser = contextinter.ADMUTILISATEUR.FirstOrDefault(t => t.MATRICULE == pDemande.LaDemande.MATRICULE);
                                LIBELLETOP leTop = contextinter.LIBELLETOP.FirstOrDefault(t => t.CODE == Enumere.TopGuichet);
                                List<LCLIENT> lesFactureRetours = new List<LCLIENT>();
                                foreach (var FactureAregle in lesFacture)
                                {
                                    LCLIENT laFactureDemande = new LCLIENT();
                                    laFactureDemande.CENTRE = FactureAregle.CENTRE;
                                    laFactureDemande.CLIENT = FactureAregle.CLIENT;
                                    laFactureDemande.ORDRE = FactureAregle.ORDRE;
                                    laFactureDemande.REFEM = FactureAregle.REFEM;
                                    laFactureDemande.NDOC = FactureAregle.NDOC;
                                    laFactureDemande.COPER = FactureAregle.COPER;
                                    laFactureDemande.DENR = System.DateTime.Today;
                                    laFactureDemande.EXIG = 0;
                                    laFactureDemande.MONTANT = FactureAregle.MONTANTHT + FactureAregle.MONTANTTAXE;
                                    laFactureDemande.DC = Enumere.Debit;
                                    laFactureDemande.MOISCOMPT = System.DateTime.Today.Year + System.DateTime.Today.Month.ToString("00");
                                    laFactureDemande.TOP1 = Enumere.TopGuichet;
                                    laFactureDemande.EXIGIBILITE = System.DateTime.Today;
                                    laFactureDemande.MATRICULE = FactureAregle.USERCREATION;
                                    laFactureDemande.USERCREATION = FactureAregle.USERCREATION;
                                    laFactureDemande.DATECREATION = FactureAregle.DATECREATION;
                                    laFactureDemande.DATEMODIFICATION = FactureAregle.DATEMODIFICATION;
                                    laFactureDemande.USERMODIFICATION = FactureAregle.USERMODIFICATION;
                                    laFactureDemande.FK_IDCENTRE = FactureAregle.FK_IDCENTRE;
                                    laFactureDemande.FK_IDADMUTILISATEUR = leUser.PK_ID;
                                    laFactureDemande.FK_IDCOPER = FactureAregle.FK_IDCOPER;
                                    laFactureDemande.FK_IDLIBELLETOP = leTop.PK_ID;
                                    laFactureDemande.FK_IDCLIENT = _LeClient.PK_ID;
                                    _LesFacture.Add(laFactureDemande);
                                }
                                Entities.InsertEntity<LCLIENT>(_LesFacture, pContext);
                            }
                        }
                    }
                }
                List<POSE_SCELLE_DEMANDE> organeScelleDemande = new List<POSE_SCELLE_DEMANDE>();
                if (pDemande.LaDemande != null)
                {
                   organeScelleDemande = pContext.POSE_SCELLE_DEMANDE.Where(t => t.FK_IDDEMANDE == pDemande.LaDemande.PK_ID).ToList();
                }

                Entities.InsertEntity<AG>(_LeAG, pContext);


                _LeBrt.FK_IDAG = _LeAG.PK_ID;
                Entities.InsertEntity<BRT>(_LeBrt, pContext);

                if (organeScelleDemande != null && organeScelleDemande.Count != 0)
                {
                    organeScelleDemande.ForEach(t => t.FK_IDBRT = _LeBrt.PK_ID);
                    organeScelleDemande.ForEach(t => t.USERCREATION = pDemande.LaDemande.USERCREATION);
                    organeScelleDemande.ForEach(t => t.DATECREATION  = System.DateTime.Now );
                }
                _LeClient.FK_IDAG = _LeAG.PK_ID;
                Entities.InsertEntity<CLIENT>(_LeClient, pContext);

                _LeAbon.FK_IDCLIENT = _LeClient.PK_ID;
                Entities.InsertEntity<ABON>(_LeAbon, pContext);

                if (_LaSociete != null && !string.IsNullOrEmpty( _LaSociete.NOMABON) )
                {
                    _LaSociete.FK_IDCLIENT = _LeClient.PK_ID;
                    Entities.InsertEntity<SOCIETEPRIVE>(_LaSociete, pContext);
                }
                if (_LeProprio != null && !string.IsNullOrEmpty(_LeProprio.NOM ))
                {
                    _LeProprio.FK_IDCLIENT = _LeClient.PK_ID;
                    Entities.InsertEntity<INFOPROPRIETAIRE>(_LeProprio, pContext);
                }
                if (_LeInstitue != null && !string.IsNullOrEmpty(_LeInstitue.NOMABON))
                {
                    _LeInstitue.FK_IDCLIENT = _LeClient.PK_ID;
                    Entities.InsertEntity<ADMINISTRATION_INSTITUT>(_LeInstitue, pContext);
                }
                if (_LePersonPhysique != null && !string.IsNullOrEmpty(_LePersonPhysique.NOMABON))
                {
                    _LePersonPhysique.FK_IDCLIENT = _LeClient.PK_ID;
                    Entities.InsertEntity<PERSONNEPHYSIQUE >(_LePersonPhysique, pContext);
                }
                Entities.InsertEntity<COMPTEUR>(lstcompteur, pContext);

                foreach (CsCanalisation  item in pDemande.LstCanalistion )
                {
                    CANALISATION leCan = _LeCompteur.FirstOrDefault(t => t.POINT == item.POINT);
                    COMPTEUR leCompteurDuPoint = lstcompteur.FirstOrDefault(t => t.TYPECOMPTEUR == item.TYPECOMPTEUR);
                    leCan.FK_IDCOMPTEUR = leCompteurDuPoint.PK_ID;
                    leCan.FK_IDABON = _LeAbon.PK_ID;
                }
                Entities.InsertEntity<CANALISATION>(_LeCompteur, pContext);


                foreach (EVENEMENT item in _LstEvenement)
                {
                    item.FK_IDABON = _LeAbon.PK_ID;
                    item.FK_IDCANALISATION = _LeCompteur.FirstOrDefault(t => t.POINT == item.POINT).PK_ID;

                    CANALISATION leCan = _LeCompteur.FirstOrDefault(t => t.POINT == item.POINT);
                    COMPTEUR leCompteurDuPoint = lstcompteur.FirstOrDefault(t => t.TYPECOMPTEUR == item.TYPECOMPTEUR);
                    item.FK_IDCOMPTEUR = leCompteurDuPoint.PK_ID;


                    if (pDemande.LaDemande.TYPEDEMANDE == Enumere.BranchementAbonnementEp)
                    {
                        item.TYPETARIF = _LeAbon.TYPETARIF;
                        item.PUISSANCE  = _LeAbon.PUISSANCE ;
                    }
                }
                Entities.InsertEntity<EVENEMENT>(_LstEvenement, pContext);
            }
           
            catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            {
                // Retrieve the error messages as a list of strings.
                var errorMessages = ex.EntityValidationErrors
                        .SelectMany(x => x.ValidationErrors)
                        .Select(x => x.ErrorMessage);

                // Join the list to a single string.
                var fullErrorMessage = string.Join("; ", errorMessages);

                // Combine the original exception message with the new one.
                var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

                //return exceptionMessage;
                // Throw a new DbEntityValidationException with the improved exception message.
                throw new System.Data.Entity.Validation.DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);

            }
        }
        public static bool MiseAjoursApDp(CsDemande pDemande, galadbEntities pContext)
        {
            try
            {
                galadbEntities contextinter = new galadbEntities();
                DEMANDE _Demande = new DEMANDE();
                if (pDemande.LaDemande != null)
                    _Demande = Entities.ConvertObject<DEMANDE, CsDemandeBase>(pDemande.LaDemande);

                List<CANALISATION> lstCanalisation = new List<CANALISATION>();
                ABON _LeAbon = pContext.ABON.FirstOrDefault(t => t.FK_IDCENTRE == pDemande.Abonne.FK_IDCENTRE && t.CENTRE == pDemande.Abonne.CENTRE && t.CLIENT == pDemande.Abonne.CLIENT && pDemande.Abonne.ORDRE == t.ORDRE);
                if (_LeAbon != null)
                {
                    _LeAbon.FK_IDTYPETARIF = pDemande.Abonne.FK_IDTYPETARIF;
                    _LeAbon.TYPETARIF = pDemande.Abonne.TYPETARIF ;
                    _LeAbon.PUISSANCE  = pDemande.Abonne.PUISSANCE ;

                    if (pDemande.LstCoutDemande != null && pDemande.LstCoutDemande.Count != 0)
                    {
                        CsDemandeDetailCout leAvance = pDemande.LstCoutDemande.FirstOrDefault(t => t.COPER == Enumere.CoperCAU);
                        if (leAvance != null && !string.IsNullOrEmpty(leAvance.CENTRE))
                        {
                            _LeAbon.AVANCE = (leAvance.MONTANTHT + leAvance.MONTANTTAXE);
                            _LeAbon.DAVANCE = leAvance.DATECREATION;
                        }
                    }
                    lstCanalisation = pContext.CANALISATION.Where(t => t.FK_IDABON == _LeAbon.PK_ID && t.DEPOSE == null).ToList();
                    foreach (CANALISATION item in lstCanalisation)
                        item.DEPOSE = System.DateTime.Today;
                }
                Entities.UpdateEntity<ABON>(_LeAbon, pContext);

                List<CANALISATION> _LeCompteur = new List<CANALISATION>();
                if (pDemande.LstCanalistion != null)
                {
                    _LeCompteur = Entities.ConvertObject<CANALISATION, CsCanalisation>(pDemande.LstCanalistion);
                    _LeCompteur.ForEach(t => t.PROPRIO = pDemande.LeClient.PROPRIO);
                    _LeCompteur.ForEach(t => t.FK_IDPROPRIETAIRE = pContext.PROPRIETAIRE.FirstOrDefault(p => p.CODE == pDemande.LeClient.PROPRIO).PK_ID);
                }

                List<COMPTEUR> lstcompteur = new List<COMPTEUR>();
                foreach (var item_ in pDemande.LstCanalistion)
                {
                    MAGASINVIRTUEL item = pContext.MAGASINVIRTUEL.FirstOrDefault(m => m.PK_ID == item_.FK_IDMAGAZINVIRTUEL);
                    COMPTEUR can = new COMPTEUR();
                    can.ANNEEFAB = item.ANNEEFAB;
                    can.CADRAN = item.CADRAN;
                    can.COEFCOMPTAGE = item.COEFCOMPTAGE;
                    can.COEFLECT = item.COEFLECT;
                    can.DATECREATION = System.DateTime.Now;
                    can.DATEMODIFICATION = item.DATEMODIFICATION;
                    can.MISEENSERVICE = System.DateTime.Today.Date;

                    can.FK_IDCALIBRE = item.FK_IDCALIBRECOMPTEUR;
                    can.FK_IDMARQUECOMPTEUR = item.FK_IDMARQUECOMPTEUR;
                    can.FK_IDPRODUIT = item.FK_IDPRODUIT;
                    can.FK_IDTYPECOMPTEUR = item_.FK_IDTYPECOMPTEUR.Value;
                    can.FK_IDSTATUTCOMPTEUR = 1;
                    can.FK_IDETATCOMPTEUR = 1;
                    can.FONCTIONNEMENT = item.FONCTIONNEMENT;
                    can.MARQUE = item.MARQUE;

                    can.NUMERO = item_.NUMERO;
                    can.TYPECOMPTEUR = item_.TYPECOMPTEUR;

                    can.PLOMBAGE = item.PLOMBAGE;
                    can.PRODUIT = item_.PRODUIT;
                    can.USERCREATION = item.USERCREATION;
                    can.USERMODIFICATION = item.USERMODIFICATION;

                    lstcompteur.Add(can);
                }

                List<EVENEMENT> _LstEvenement = new List<EVENEMENT>();
                if (pDemande.LstEvenement != null)
                    _LstEvenement = Entities.ConvertObject<EVENEMENT, CsEvenement>(pDemande.LstEvenement);
                _LstEvenement.ForEach(t => t.STATUS = 99);


            

                BRT leBrt = contextinter.BRT.FirstOrDefault(t => t.FK_IDCENTRE == pDemande.LaDemande.FK_IDCENTRE && t.CENTRE == pDemande.LaDemande.CENTRE && t.CLIENT == pDemande.LaDemande.CLIENT);
                List<POSE_SCELLE_DEMANDE> organeScelleDemande = new List<POSE_SCELLE_DEMANDE>();
                if (pDemande.LaDemande != null)
                {
                    organeScelleDemande = pContext.POSE_SCELLE_DEMANDE.Where(t => t.FK_IDDEMANDE == pDemande.LaDemande.PK_ID).ToList();
                }
                if (organeScelleDemande != null && organeScelleDemande.Count != 0 && leBrt != null && leBrt.PK_ID != 0 && leBrt.PK_ID != null)
                {
                    organeScelleDemande.ForEach(t => t.FK_IDBRT = leBrt.PK_ID);
                    organeScelleDemande.ForEach(t => t.USERCREATION = pDemande.LaDemande.USERCREATION);
                    organeScelleDemande.ForEach(t => t.DATECREATION = System.DateTime.Now);
                }


                Entities.InsertEntity<COMPTEUR>(lstcompteur, pContext);

                _LeCompteur.ForEach(t => t.FK_IDABON = _LeAbon.PK_ID);
                _LeCompteur.ForEach(t => t.FK_IDCOMPTEUR = lstcompteur.FirstOrDefault().PK_ID);

                Entities.InsertEntity<CANALISATION>(_LeCompteur, pContext);

                foreach (EVENEMENT item in _LstEvenement)
                {
                    item.FK_IDABON = _LeAbon.PK_ID;
                    item.FK_IDCANALISATION = _LeCompteur.FirstOrDefault(t => t.POINT == item.POINT).PK_ID;
                    item.FK_IDCOMPTEUR = lstcompteur.FirstOrDefault().PK_ID;
                }
                Entities.InsertEntity<EVENEMENT>(_LstEvenement, pContext);
                return true;
            }

            catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            {
                // Retrieve the error messages as a list of strings.
                var errorMessages = ex.EntityValidationErrors
                        .SelectMany(x => x.ValidationErrors)
                        .Select(x => x.ErrorMessage);

                // Join the list to a single string.
                var fullErrorMessage = string.Join("; ", errorMessages);

                // Combine the original exception message with the new one.
                var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

                //return exceptionMessage;
                // Throw a new DbEntityValidationException with the improved exception message.
                throw new System.Data.Entity.Validation.DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);

            }
        }
        public static bool MiseAjoursAbonnementSeul(CsDemande pDemande, galadbEntities pContext)
        {
            try
            {
                galadbEntities contextinter = new galadbEntities();
                DEMANDE _Demande = new DEMANDE();
                if (pDemande.LaDemande != null)
                {
                    pDemande.LaDemande.DATEFIN = System.DateTime.Today.Date;
                    _Demande = Entities.ConvertObject<DEMANDE, CsDemandeBase>(pDemande.LaDemande);
                }
                CLIENT _LeClient = new CLIENT();
                if (pDemande.LeClient != null)
                {
                    pDemande.LeClient.DATECREATION  = System.DateTime.Today.Date;
                    _LeClient = Entities.ConvertObject<CLIENT, CsClient>(pDemande.LeClient);
                    _LeClient.FK_IDAG = contextinter.AG.FirstOrDefault(t => t.FK_IDCENTRE == _LeClient.FK_IDCENTRE && t.CENTRE == _LeClient.CENTRE && t.CLIENT == _LeClient.REFCLIENT).PK_ID;
                    #region Vider les propriété d'identification sur l'objet client
                    _LeClient.NUMEROPIECEIDENTITE = string.Empty;
                    _LeClient.FK_IDPIECEIDENTITE = null;
                    #endregion
                }
                ABON _LeAbon = new ABON();
                if (pDemande.Abonne != null)
                {
                    pDemande.Abonne.DATECREATION = System.DateTime.Today.Date;
                    _LeAbon = Entities.ConvertObject<ABON, CsAbon>(pDemande.Abonne);
                }
                List<CANALISATION> _LeCompteur = new List<CANALISATION>();
                if (pDemande.LstCanalistion != null)
                {
                    pDemande.LstCanalistion.ForEach(o=>o.DATECREATION = System.DateTime.Today.Date);
                    _LeCompteur = Entities.ConvertObject<CANALISATION, CsCanalisation>(pDemande.LstCanalistion);
                    _LeCompteur.ForEach(t => t.PROPRIO = pDemande.LeClient.PROPRIO);
                    _LeCompteur.ForEach(t => t.FK_IDPROPRIETAIRE = pContext.PROPRIETAIRE.FirstOrDefault(p => p.CODE == pDemande.LeClient.PROPRIO).PK_ID);
                }

                List<COMPTEUR> lstcompteur = new List<COMPTEUR>();

                foreach (var item_ in pDemande.LstCanalistion)
                {
                    MAGASINVIRTUEL item = pContext.MAGASINVIRTUEL.FirstOrDefault(m => m.PK_ID == item_.FK_IDMAGAZINVIRTUEL);
                    COMPTEUR can = new COMPTEUR();
                    can.ANNEEFAB = item.ANNEEFAB;
                    can.CADRAN = item.CADRAN;
                    can.COEFCOMPTAGE = item.COEFCOMPTAGE;
                    can.COEFLECT = item.COEFLECT;
                    can.DATECREATION = System.DateTime.Now;
                    can.DATEMODIFICATION = item.DATEMODIFICATION;
                    can.MISEENSERVICE = System.DateTime.Today.Date;

                    can.FK_IDCALIBRE = item.FK_IDCALIBRECOMPTEUR;
                    can.FK_IDMARQUECOMPTEUR = item.FK_IDMARQUECOMPTEUR;
                    can.FK_IDPRODUIT = item.FK_IDPRODUIT;
                    can.FK_IDTYPECOMPTEUR = item_.FK_IDTYPECOMPTEUR.Value;
                    can.FK_IDSTATUTCOMPTEUR = 1;
                    can.FK_IDETATCOMPTEUR = 1;
                    can.FONCTIONNEMENT = item.FONCTIONNEMENT;
                    can.MARQUE = item.MARQUE;

                    can.NUMERO = item_.NUMERO;
                    can.TYPECOMPTEUR = item_.TYPECOMPTEUR;

                    can.PLOMBAGE = item.PLOMBAGE;
                    can.PRODUIT = item_.PRODUIT;
                    can.USERCREATION = item.USERCREATION;
                    can.USERMODIFICATION = item.USERMODIFICATION;

                    lstcompteur.Add(can);
                }

                List<EVENEMENT> _LstEvenement = new List<EVENEMENT>();
                if (pDemande.LstEvenement != null)
                    _LstEvenement = Entities.ConvertObject<EVENEMENT, CsEvenement>(pDemande.LstEvenement);
                _LstEvenement.ForEach(t => t.STATUS = 99);

                List<LCLIENT> _LstFacture = new List<LCLIENT>();
                if (pDemande.LstCoutDemande != null && pDemande.LstCoutDemande.Count != 0)
                {
                    foreach (CsDemandeDetailCout item in pDemande.LstCoutDemande)
                    {
                        if (item.COPER == Enumere.CoperCAU)
                        {
                            _LeAbon.AVANCE = (item.MONTANTHT + item.MONTANTTAXE);
                            _LeAbon.DAVANCE = item.DATECREATION;
                        }
                        LCLIENT lelClient = pContext.LCLIENT.FirstOrDefault(t => t.FK_IDCENTRE == item.FK_IDCENTRE && t.CENTRE == item.CENTRE && 
                                                                                 t.CLIENT == item.CLIENT && t.ORDRE == item.ORDRE && 
                                                                                 t.NDOC == item.NDOC && t.REFEM == item.REFEM);
                        if (lelClient != null && !string.IsNullOrEmpty(lelClient.CENTRE))
                        {
                            lelClient.FK_IDCLIENT = _LeClient.PK_ID;
                            lelClient.COPER = item.COPER;
                            lelClient.FK_IDCOPER = item.FK_IDCOPER;
                            lelClient.MONTANT = (item.MONTANTHT + item.MONTANTTAXE);
                        }
                    }
                }

                List<POSE_SCELLE_DEMANDE> organeScelleDemande = new List<POSE_SCELLE_DEMANDE>();
                if (pDemande.LaDemande != null)
                {
                    organeScelleDemande = pContext.POSE_SCELLE_DEMANDE.Where(t => t.FK_IDDEMANDE == pDemande.LaDemande.PK_ID).ToList();
                }

                BRT leBrt = contextinter.BRT.FirstOrDefault (t => t.FK_IDCENTRE == pDemande.LaDemande.FK_IDCENTRE && t.CENTRE == pDemande.LaDemande.CENTRE && t.CLIENT == pDemande.LaDemande.CLIENT);

                if (organeScelleDemande != null && organeScelleDemande.Count != 0 && leBrt != null && leBrt.PK_ID != 0 && leBrt.PK_ID != null )
                {
                    organeScelleDemande.ForEach(t => t.FK_IDBRT = leBrt.PK_ID);
                    organeScelleDemande.ForEach(t => t.USERCREATION = pDemande.LaDemande.USERCREATION);
                    organeScelleDemande.ForEach(t => t.DATECREATION = System.DateTime.Now);
                }

                Entities.InsertEntity<CLIENT>(_LeClient, pContext);

                _LeAbon.FK_IDCLIENT = _LeClient.PK_ID;
                Entities.InsertEntity<ABON>(_LeAbon, pContext);

                Entities.InsertEntity<COMPTEUR>(lstcompteur, pContext);

                _LeCompteur.ForEach(t => t.FK_IDABON = _LeAbon.PK_ID);
                _LeCompteur.ForEach(t => t.FK_IDCOMPTEUR = lstcompteur.FirstOrDefault().PK_ID);

                Entities.InsertEntity<CANALISATION>(_LeCompteur, pContext);

                foreach (EVENEMENT item in _LstEvenement)
                {
                    item.FK_IDABON = _LeAbon.PK_ID;
                    item.FK_IDCANALISATION = _LeCompteur.FirstOrDefault(t => t.POINT == item.POINT).PK_ID;
                    item.FK_IDCOMPTEUR = lstcompteur.FirstOrDefault().PK_ID;
                }
                Entities.InsertEntity<EVENEMENT>(_LstEvenement, pContext);
                return true;
            }

            catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            {
                // Retrieve the error messages as a list of strings.
                var errorMessages = ex.EntityValidationErrors
                        .SelectMany(x => x.ValidationErrors)
                        .Select(x => x.ErrorMessage);

                // Join the list to a single string.
                var fullErrorMessage = string.Join("; ", errorMessages);

                // Combine the original exception message with the new one.
                var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

                //return exceptionMessage;
                // Throw a new DbEntityValidationException with the improved exception message.
                throw new System.Data.Entity.Validation.DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);

            }
        }
        public static bool MiseAjoursDepannage(CsDemande pDemande, galadbEntities pContext)
        {
            try
            {
                galadbEntities _LeContextInter = new galadbEntities();
                List<LCLIENT> _LesFacture = new  List<LCLIENT>();
                List<RUBRIQUEDEMANDE> lesFacture = _LeContextInter.RUBRIQUEDEMANDE.Where(t => t.FK_IDDEMANDE == pDemande.LaDemande.PK_ID).ToList();
                if (lesFacture != null)
                {
                    // Inserer la facture dans le compte de la ville
                    _LesFacture = RetourneFactureDepannageEp(lesFacture);
                 Entities.InsertEntity<LCLIENT>(_LesFacture,pContext);
                }
                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static bool MiseAjoursResiliation(CsDemande pDemande, galadbEntities pContext)
        {
            try
            {
                galadbEntities _LeContextInter = new galadbEntities();
                ABON _LeAbon = new ABON();
                TRANSCAISB _LeAvance = new TRANSCAISB();
                List<CANALISATION> lstCanalisation = new List<CANALISATION>();

                if (pDemande.Abonne != null)
                {
                    pDemande.Abonne.USERMODIFICATION = pDemande.LaDemande.MATRICULE ;
                    ABON _LeAbonnement = pContext.ABON.FirstOrDefault(t => t.FK_IDCENTRE == pDemande.Abonne.FK_IDCENTRE && t.CENTRE == pDemande.Abonne.CENTRE && t.CLIENT == pDemande.Abonne.CLIENT && pDemande.Abonne.ORDRE == t.ORDRE);
                    if (_LeAbonnement != null)
                    {
                        _LeAbonnement.DRES = System.DateTime.Today;
                        _LeAbonnement.USERMODIFICATION = pDemande.LaDemande.MATRICULE;
                        
                        lstCanalisation = pContext.CANALISATION.Where(t => t.FK_IDABON == _LeAbonnement.PK_ID && t.DEPOSE == null ).ToList();
                        foreach (CANALISATION item in lstCanalisation)
                        {
                            item.DEPOSE = System.DateTime.Today;
                            item.USERMODIFICATION = pDemande.LaDemande.MATRICULE;
                        }
                    }
                    if (_LeAbonnement.AVANCE != null && _LeAbonnement.AVANCE != 0)
                    {
                        _LeAvance = RetourneAvance(_LeAbonnement);
                        DataTable dte = AccueilProcedures.RetourneFactureAvanceFromAbon(_LeAbonnement.FK_IDCENTRE ,_LeAbonnement.CENTRE ,_LeAbonnement.CLIENT ,_LeAbonnement.ORDRE ,_LeAbonnement.DAVANCE.Value ,_LeAbonnement.AVANCE.Value );
                        CsLclient lstFactiureAvance = Galatee.Tools.Utility.GetEntityFromQuery<CsLclient>(dte).FirstOrDefault();
                        if (lstFactiureAvance != null && lstFactiureAvance.CLIENT != null)
                        {
                            _LeAvance.NDOC = lstFactiureAvance.NDOC;
                            _LeAvance.FK_IDLCLIENT = lstFactiureAvance.PK_ID;
                            _LeAvance.USERCREATION = pDemande.LaDemande.MATRICULE;
                            _LeAvance.DATECREATION = System.DateTime.Now;
                            if (_LeAvance.CLIENT != null)
                                Entities.InsertEntity<TRANSCAISB>(_LeAvance, pContext);
                        }
                        else
                        {
                            LCLIENT laFactureAvnce = new LCLIENT();
                            string Periode = (_LeAbonnement.DAVANCE.Value.Year.ToString()+ _LeAbonnement.DAVANCE.Value.Month.ToString("00"));
                            laFactureAvnce = RetourneFactureAvance(_LeAbonnement, _LeAbonnement.AVANCE.Value, Periode);
                            _LeAvance.USERCREATION = pDemande.LaDemande.MATRICULE;
                            _LeAvance.DATECREATION = System.DateTime.Now;
                            _LeAvance.REFEM = laFactureAvnce.REFEM ;
                            _LeAvance.NDOC  = laFactureAvnce.NDOC;
                            laFactureAvnce.TRANSCAISB.Add(_LeAvance);
                            Entities.InsertEntity<LCLIENT>(laFactureAvnce, pContext);
                        }
                    }
                    _LeContextInter.Dispose();
                }
                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static void MiseAjoursTransfertSiteNonMigre(CsDemande pDemande, galadbEntities pContext)
        {
            try
            {
                galadbEntities contextinter = new galadbEntities();
                DEMANDE _Demande = new DEMANDE();
                if (pDemande.LaDemande != null)
                {
                    pDemande.LaDemande.DATEFIN = System.DateTime.Today.Date;
                    _Demande = Entities.ConvertObject<DEMANDE, CsDemandeBase>(pDemande.LaDemande);
                }

                DataTable dtcanal = AccueilProcedures.RetourneDcanalisationSeul(pDemande.LaDemande.PK_ID);
                pDemande.LstCanalistion = Entities.GetEntityListFromQuery<CsCanalisation>(dtcanal);

                AG _LeAG = new AG();
                if (pDemande.Ag != null)
                {
                    pDemande.Ag.DATECREATION = System.DateTime.Today.Date;
                    _LeAG = Entities.ConvertObject<AG, CsAg>(pDemande.Ag);
                    _LeAG.FK_IDTOURNEE = contextinter.TOURNEE.FirstOrDefault(t => t.FK_IDCENTRE == pDemande.Ag.FK_IDCENTRE && t.CODE == pDemande.Ag.TOURNEE).PK_ID;
                }

                BRT _LeBrt = new BRT();
                if (pDemande.Branchement != null)
                {
                    pDemande.Branchement.DATECREATION = System.DateTime.Today.Date;

                    _LeBrt = Entities.ConvertObject<BRT, CsBrt>(pDemande.Branchement);
                    _LeBrt.DRAC = System.DateTime.Today;
                    _LeBrt.SERVICE = "1";
                }

                CLIENT _LeClient = new CLIENT();
                if (pDemande.LeClient != null)
                {
                    pDemande.LeClient.DATECREATION = System.DateTime.Today.Date;
                    _LeClient = Entities.ConvertObject<CLIENT, CsClient>(pDemande.LeClient);
                }
                INFOPROPRIETAIRE _LeProprio = new INFOPROPRIETAIRE();
                if (pDemande.InfoProprietaire_ != null && !string.IsNullOrEmpty(pDemande.InfoProprietaire_.NOM))
                    _LeProprio = Entities.ConvertObject<INFOPROPRIETAIRE, CsInfoProprietaire>(pDemande.InfoProprietaire_);

                SOCIETEPRIVE _LaSociete = new SOCIETEPRIVE();
                if (pDemande.SocietePrives != null && !string.IsNullOrEmpty(pDemande.SocietePrives.NOMABON))
                    _LaSociete = Entities.ConvertObject<SOCIETEPRIVE, CsSocietePrive>(pDemande.SocietePrives);

                ADMINISTRATION_INSTITUT _LeInstitue = new ADMINISTRATION_INSTITUT();
                if (pDemande.AdministrationInstitut != null && !string.IsNullOrEmpty(pDemande.AdministrationInstitut.NOMABON))
                    _LeInstitue = Entities.ConvertObject<ADMINISTRATION_INSTITUT, CsAdministration_Institut>(pDemande.AdministrationInstitut);

                PERSONNEPHYSIQUE _LePersonPhysique = new PERSONNEPHYSIQUE();
                if (pDemande.PersonePhysique != null && !string.IsNullOrEmpty(pDemande.PersonePhysique.NOMABON))
                    _LePersonPhysique = Entities.ConvertObject<PERSONNEPHYSIQUE, CsPersonePhysique>(pDemande.PersonePhysique);

                ABON _LeAbon = new ABON();
                if (pDemande.Abonne != null)
                {
                    pDemande.Abonne.DATECREATION = System.DateTime.Today.Date;
                    _LeAbon = Entities.ConvertObject<ABON, CsAbon>(pDemande.Abonne);
                }
                List<COMPTEUR> lstcompteur = new List<COMPTEUR>();
                foreach (var item_ in pDemande.LstCanalistion)
                {
                    CANALISATION leCanal = new CANALISATION();
                    DCOMPTEUR item = pContext.DCOMPTEUR .FirstOrDefault(m => m.FK_IDDEMANDE  == item_.FK_IDDEMANDE );
                    COMPTEUR can = new COMPTEUR();
                    can.ANNEEFAB = item.ANNEEFAB;
                    can.CADRAN = item.CADRAN;
                    can.COEFCOMPTAGE = item.COEFCOMPTAGE;
                    can.COEFLECT = item.COEFLECT;
                    can.DATECREATION = System.DateTime.Now;
                    can.DATEMODIFICATION = item.DATEMODIFICATION;
                    can.MISEENSERVICE = System.DateTime.Today.Date;

                    can.FK_IDCALIBRE = item.FK_IDCALIBRE ;
                    can.FK_IDMARQUECOMPTEUR = item.FK_IDMARQUECOMPTEUR;
                    can.FK_IDPRODUIT = item.FK_IDPRODUIT;
                    can.FK_IDTYPECOMPTEUR = item.FK_IDTYPECOMPTEUR ;
                    can.FK_IDSTATUTCOMPTEUR = 1;
                    can.FK_IDETATCOMPTEUR = 1;
                    can.FONCTIONNEMENT = item.FONCTIONNEMENT;
                    can.MARQUE = item.MARQUE;

                    can.NUMERO = item.NUMERO ;
                    can.TYPECOMPTEUR = item.TYPECOMPTEUR;

                    can.PLOMBAGE = item.PLOMBAGE;
                    can.PRODUIT = item_.PRODUIT;
                    can.USERCREATION = item.USERCREATION;
                    can.USERMODIFICATION = item.USERMODIFICATION;
                    lstcompteur.Add(can);
                }

                List<CANALISATION> _LeCompteur = new List<CANALISATION>();
                if (pDemande.LstCanalistion != null)
                {
                    _LeCompteur = Entities.ConvertObject<CANALISATION, CsCanalisation >(pDemande.LstCanalistion);
                    _LeCompteur.ForEach(t => t.PROPRIO = pDemande.LeClient.PROPRIO);
                    _LeCompteur.ForEach(t => t.FK_IDPROPRIETAIRE = pContext.PROPRIETAIRE.FirstOrDefault(p => p.CODE == pDemande.LeClient.PROPRIO).PK_ID);
                }

                List<EVENEMENT> _LstEvenement = new List<EVENEMENT>();
                if (pDemande.LstEvenement != null)
                    _LstEvenement = Entities.ConvertObject<EVENEMENT, CsEvenement>(pDemande.LstEvenement);
                _LstEvenement.ForEach(t => t.STATUS = 99);

                List<LCLIENT> _LstFacture = new List<LCLIENT>();
                if (pDemande.LstCoutDemande != null && pDemande.LstCoutDemande.Count != 0)
                {
                    foreach (CsDemandeDetailCout item in pDemande.LstCoutDemande)
                    {
                        if (item.COPER == Enumere.CoperCAU)
                        {
                            _LeAbon.AVANCE = (item.MONTANTHT + item.MONTANTTAXE);
                            _LeAbon.DAVANCE = item.DATECREATION;
                        }
                        LCLIENT lelClient = pContext.LCLIENT.FirstOrDefault(t => t.CENTRE == item.CENTRE && t.CLIENT == item.CLIENT && t.ORDRE == item.ORDRE && t.FK_IDCENTRE == item.FK_IDCENTRE && t.NDOC == item.NDOC && t.REFEM == item.REFEM);
                        if (lelClient != null && !string.IsNullOrEmpty(lelClient.CENTRE))
                        {
                            lelClient.FK_IDCLIENT = _LeClient.PK_ID;
                            lelClient.COPER = item.COPER;
                            lelClient.FK_IDCOPER = item.FK_IDCOPER;
                            lelClient.MONTANT = (item.MONTANTHT + item.MONTANTTAXE);
                        }
                    }
                }
                List<POSE_SCELLE_DEMANDE> organeScelleDemande = new List<POSE_SCELLE_DEMANDE>();
                if (pDemande.LaDemande != null)
                {
                    organeScelleDemande = pContext.POSE_SCELLE_DEMANDE.Where(t => t.FK_IDDEMANDE == pDemande.LaDemande.PK_ID).ToList();
                }

                Entities.InsertEntity<AG>(_LeAG, pContext);


                _LeBrt.FK_IDAG = _LeAG.PK_ID;
                Entities.InsertEntity<BRT>(_LeBrt, pContext);

                if (organeScelleDemande != null && organeScelleDemande.Count != 0)
                {
                    organeScelleDemande.ForEach(t => t.FK_IDBRT = _LeBrt.PK_ID);
                    organeScelleDemande.ForEach(t => t.USERCREATION = pDemande.LaDemande.USERCREATION);
                    organeScelleDemande.ForEach(t => t.DATECREATION = System.DateTime.Now);
                }
                _LeClient.FK_IDAG = _LeAG.PK_ID;
                Entities.InsertEntity<CLIENT>(_LeClient, pContext);

                _LeAbon.FK_IDCLIENT = _LeClient.PK_ID;
                Entities.InsertEntity<ABON>(_LeAbon, pContext);

                if (_LaSociete != null && !string.IsNullOrEmpty(_LaSociete.NOMABON))
                {
                    _LaSociete.FK_IDCLIENT = _LeClient.PK_ID;
                    Entities.InsertEntity<SOCIETEPRIVE>(_LaSociete, pContext);
                }
                if (_LeProprio != null && !string.IsNullOrEmpty(_LeProprio.NOM))
                {
                    _LeProprio.FK_IDCLIENT = _LeClient.PK_ID;
                    Entities.InsertEntity<INFOPROPRIETAIRE>(_LeProprio, pContext);
                }
                if (_LeInstitue != null && !string.IsNullOrEmpty(_LeInstitue.NOMABON))
                {
                    _LeInstitue.FK_IDCLIENT = _LeClient.PK_ID;
                    Entities.InsertEntity<ADMINISTRATION_INSTITUT>(_LeInstitue, pContext);
                }
                if (_LePersonPhysique != null && !string.IsNullOrEmpty(_LePersonPhysique.NOMABON))
                {
                    _LePersonPhysique.FK_IDCLIENT = _LeClient.PK_ID;
                    Entities.InsertEntity<PERSONNEPHYSIQUE>(_LePersonPhysique, pContext);
                }
                Entities.InsertEntity<COMPTEUR>(lstcompteur, pContext);

                foreach (CANALISATION item in _LeCompteur)
                {
                    CANALISATION leCan = _LeCompteur.FirstOrDefault(t => t.POINT == item.POINT);
                    COMPTEUR leCompteurDuPoint = lstcompteur.FirstOrDefault();
                    leCan.FK_IDCOMPTEUR = leCompteurDuPoint.PK_ID;
                    leCan.FK_IDABON = _LeAbon.PK_ID;
                }
                Entities.InsertEntity<CANALISATION>(_LeCompteur, pContext);

                foreach (EVENEMENT item in _LstEvenement)
                {
                    item.FK_IDABON = _LeAbon.PK_ID;
                    item.FK_IDCANALISATION = _LeCompteur.FirstOrDefault(t => t.POINT == item.POINT).PK_ID;
                    item.FK_IDCOMPTEUR = lstcompteur.FirstOrDefault().PK_ID;
                    if (pDemande.LaDemande.TYPEDEMANDE == Enumere.BranchementAbonnementEp)
                    {
                        item.TYPETARIF = _LeAbon.TYPETARIF;
                        item.PUISSANCE = _LeAbon.PUISSANCE;
                    }
                }
                Entities.InsertEntity<EVENEMENT>(_LstEvenement, pContext);
            }

            catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            {
                // Retrieve the error messages as a list of strings.
                var errorMessages = ex.EntityValidationErrors
                        .SelectMany(x => x.ValidationErrors)
                        .Select(x => x.ErrorMessage);

                // Join the list to a single string.
                var fullErrorMessage = string.Join("; ", errorMessages);

                // Combine the original exception message with the new one.
                var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

                //return exceptionMessage;
                // Throw a new DbEntityValidationException with the improved exception message.
                throw new System.Data.Entity.Validation.DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);

            }
        }

        public static  LCLIENT RetourneFactureAvance(ABON   FactureAregle,decimal montant,string periode)
        {

            galadbEntities ctx = new galadbEntities();
            COPER leCoper = ctx.COPER.FirstOrDefault(t => t.CODE == Enumere.CoperCAU );
            LIBELLETOP leTope = ctx.LIBELLETOP.FirstOrDefault(t => t.CODE == Enumere.TopCaisse);
            ADMUTILISATEUR leuser = ctx.ADMUTILISATEUR.FirstOrDefault(t => t.MATRICULE == FactureAregle.USERMODIFICATION );

            try
            {
                LCLIENT laFactureDemande = new LCLIENT();
                laFactureDemande.CENTRE = FactureAregle.CENTRE;
                laFactureDemande.CLIENT = FactureAregle.CLIENT;
                laFactureDemande.ORDRE = FactureAregle.ORDRE;
                laFactureDemande.REFEM = periode;
                laFactureDemande.NDOC = NumeroFacture(FactureAregle.FK_IDCENTRE);
                laFactureDemande.COPER = Enumere.CoperCAU;
                laFactureDemande.DENR = System.DateTime.Today ;
                laFactureDemande.EXIG = 0;
                laFactureDemande.MONTANT = 0;
                laFactureDemande.DC = Enumere.Debit ;
                laFactureDemande.MOISCOMPT = System.DateTime.Today.Year.ToString()+ System.DateTime.Today.Month.ToString("00");
                laFactureDemande.TOP1 = Enumere.TopGuichet;
                laFactureDemande.EXIGIBILITE = System.DateTime.Today;
                laFactureDemande.MATRICULE = FactureAregle.USERMODIFICATION ;
                laFactureDemande.MONTANTTVA = 0;
                laFactureDemande.USERCREATION = FactureAregle.USERCREATION;
                laFactureDemande.DATECREATION = FactureAregle.DATECREATION;
                laFactureDemande.DATEMODIFICATION = FactureAregle.DATEMODIFICATION;
                laFactureDemande.USERMODIFICATION = FactureAregle.USERMODIFICATION;
                laFactureDemande.FK_IDCENTRE = FactureAregle.FK_IDCENTRE;
                laFactureDemande.FK_IDADMUTILISATEUR = leuser.PK_ID ;
                laFactureDemande.FK_IDCOPER = leCoper.PK_ID ;
                laFactureDemande.FK_IDLIBELLETOP = leTope.PK_ID ;
                laFactureDemande.FK_IDCLIENT = FactureAregle.FK_IDCLIENT;

                return laFactureDemande;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public static DataTable RetourneFactureAvanceFromAbon(int idcentre,string centre,string client,string ordre ,DateTime Denreg, decimal montant)
        {
            try
            {
                string refem = Denreg.Year.ToString() + Denreg.Month.ToString("00");
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = null;
                    var _x = context.LCLIENT ;
                    query =
                    (from t in _x
                     where t.FK_IDCENTRE == idcentre &&
                             t.CENTRE == centre  &&
                             t.CLIENT == client   &&
                             t.ORDRE == ordre &&
                             t.COPER == Enumere.CoperCAU &&
                             t.REFEM == refem &&
                             //t.DENR == Denreg && 
                             t.MONTANT == montant
                    

                     select new
                     {
                        t. PK_ID ,
                        t. CENTRE ,
                        t. CLIENT ,
                        t. ORDRE ,
                        t. REFEM ,
                        t. NDOC ,
                        t. COPER ,
                        t. DENR ,
                        t. EXIG ,
                        t. MONTANT ,
                        t. CAPUR ,
                        t. CRET ,
                        t. MODEREG ,
                        t. DC ,
                        t. ORIGINE ,
                        t. CAISSE ,
                        t. ECART ,
                        t. MOISCOMPT ,
                        t. TOP1 ,
                        t. EXIGIBILITE ,
                        t. FRAISDERETARD ,
                        t. REFERENCEPUPITRE ,
                        t. IDLOT ,
                        t. DATEVALEUR ,
                        t. REFERENCE ,
                        t. REFEMNDOC ,
                        t. ACQUIT ,
                        t. MATRICULE ,
                        t. TAXESADEDUIRE ,
                        t. DATEFLAG ,
                        t. MONTANTTVA ,
                        t. IDCOUPURE ,
                        t. AGENT_COUPURE ,
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
                        t. POSTE ,
                        t. DATETRANS ,
                        t. ISNONENCAISSABLE ,
                        t. FK_IDMORATOIRE ,
                        t. FK_IDMOTIFCHEQUEINPAYE
                     });
                    return Galatee.Tools.Utility.ListToDataTable(query);
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static DataTable RetourneAvanceFromAbon(CsClient leClient)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = null;
                    var _x = context.ABON ;
                    query =
                    (from t in _x
                     where t.FK_IDCENTRE == leClient.FK_IDCENTRE &&
                             t.CENTRE == leClient.CENTRE &&
                             t.CLIENT == leClient.REFCLIENT &&
                             t.ORDRE == leClient.ORDRE 

                     select new
                     {
                         t.CENTRE,
                         t.CLIENT,
                         t.ORDRE,
                         DENR= t.DAVANCE ,
                         COPER= Enumere.CoperCAU ,
                         t.FK_IDCENTRE,
                         MONTANT=  t.AVANCE ,
                         SOLDEFACTURE = t.AVANCE  * (-1),
                     });
                    return Galatee.Tools.Utility.ListToDataTable(query);
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static TRANSCAISB  RetourneAvance(ABON leAbon)
        {
            try
            {
                galadbEntities _LeContextInter = new galadbEntities();
                COPER leCoperRemb = _LeContextInter.COPER.FirstOrDefault(t => t.CODE == Enumere.CoperRAC);
                LIBELLETOP  leLibelleTop = _LeContextInter.LIBELLETOP .FirstOrDefault(t => t.CODE == Enumere.TopGuichet);
                
                    TRANSCAISB Latransaction = new TRANSCAISB() {
                    CENTRE = leAbon.CENTRE,
                    CLIENT = leAbon.CLIENT,
                    ORDRE = leAbon.ORDRE,
                    MONTANT = leAbon.AVANCE,
                    REFEM = leAbon.DAVANCE.Value.Year.ToString() + leAbon.DAVANCE.Value.Month.ToString("00"),
                    COPER = leCoperRemb.CODE,
                    FK_IDCOPER = leCoperRemb.PK_ID,
                    FK_IDCENTRE = leAbon.FK_IDCENTRE ,
                 DTRANS  = System.DateTime.Today ,
                 FK_IDLIBELLETOP = leLibelleTop.PK_ID ,
                 TOP1 = leLibelleTop.CODE  
                };
                return Latransaction;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static DataTable RetourneAvance(CsClient leClient)
         {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = null;
                    var _x = context.TRANSCAISB ;
                    query =
                    (from t in _x 
                    where   t.FK_IDCENTRE == leClient.FK_IDCENTRE &&
                            t.CENTRE == leClient.CENTRE &&
                            t.CLIENT == leClient.REFCLIENT  &&
                            t.ORDRE == leClient.ORDRE &&
                            t.LCLIENT.COPER == Enumere.CoperCAU

                    select new
                    {
                         t.CENTRE,
                         t.CLIENT,
                         t.ORDRE,
                         t.CAISSE,
                         t.ACQUIT,
                         t.MATRICULE,
                         t.MODEREG,
                         t.NUMCHEQ,
                         t.BANQUE,
                         t.NDOC,
                         t.REFEM,
                         t.COPER ,
                         EXIGIBLITE = t.DTRANS,
                         t.DTRANS,
                         t.FK_IDCENTRE,
                         t.FK_IDMODEREG,
                         t.FK_IDCAISSIERE,
                         t.MONTANT ,
                         SOLDEFACTURE = t.MONTANT * (-1) ,
                         NUMDEM = t.NUMDEM,
                         LIBELLECOPER = t.COPER1.LIBELLE,
                         LIBELLEMODREG = t.MODEREG1.LIBELLE
                    }).OrderByDescending(u => u.DTRANS.Value );
                    return Galatee.Tools.Utility.ListToDataTable(query);
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }




        public static bool LotDuUserExite(List<LOTRI> _LstLotri,string user,string centre ,string lotri)
        {
            return _LstLotri.FirstOrDefault(l => l.USERCREATION == user && l.CENTRE == centre && lotri == l.NUMLOTRI) != null ? true : false;
        }
        public static List<CsTranscaisse> RetourneReglementDemande(string centre, string Client, string Ordre, string NumDemande)
        {
            try
            {
                using (galadbEntities  context = new galadbEntities())
                {
                    List<CsTranscaisse> lstEncaissement = new List<CsTranscaisse>();
                    List<TRANSCAISSE> _LaReglementTranscaisseDemande = context.TRANSCAISSE.Where(x => x.CENTRE == centre && x.CLIENT == Client && x.ORDRE == Ordre && x.NUMDEM == NumDemande).ToList();
                    if (_LaReglementTranscaisseDemande != null && _LaReglementTranscaisseDemande.Count != 0)
                    {
                        lstEncaissement = Entities.ConvertObject<CsTranscaisse, TRANSCAISSE>(_LaReglementTranscaisseDemande);
                        lstEncaissement.ForEach(t => t.FK_IDMATRICULE = _LaReglementTranscaisseDemande[0].FK_IDCAISSIERE.Value   );
                    }
                    else
                    {
                        List<TRANSCAISB> _LaReglementTranscaisBDemande = context.TRANSCAISB.Where(x => x.CENTRE == centre && x.CLIENT == Client && x.ORDRE == Ordre && x.NUMDEM == NumDemande).ToList();
                        if (_LaReglementTranscaisBDemande != null && _LaReglementTranscaisBDemande.Count != 0)
                            lstEncaissement = Entities.ConvertObject<CsTranscaisse, TRANSCAISB>(_LaReglementTranscaisBDemande);
                    }
                    return lstEncaissement; 
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static bool MiseAjoursReabonnement(CsDemande pDemande, galadbEntities pContext)
        {
             try
            {
                galadbEntities _LeContextInter = new galadbEntities();
                List<RUBRIQUEDEMANDE> _LstDemandeCout = new List<RUBRIQUEDEMANDE>();
                DEMANDE _Demande = new DEMANDE();
                AG _LeAg = new AG();
                ABON _LeAbon = new ABON();
                CLIENT _LeClient = new CLIENT();

                if (pDemande.Abonne != null)
                {
                    _LeAbon = Entities.ConvertObject<ABON, CsAbon>(pDemande.Abonne);
                    ABON abinit = _LeContextInter.ABON.FirstOrDefault(p => p.FK_IDCENTRE == _LeAbon.FK_IDCENTRE && p.CENTRE1.CODE == pDemande.Abonne.CENTRE && p.CLIENT1.REFCLIENT == pDemande.Abonne.CLIENT && p.CLIENT1.ORDRE == pDemande.Abonne.ORDRE);
                    _LeAbon.DATECREATION = abinit.DATECREATION;
                    _LeAbon.USERCREATION = abinit.USERCREATION;
                    _LeAbon.PK_ID = abinit.PK_ID;
                    _LeAbon.FK_IDCLIENT = abinit.FK_IDCLIENT;
                }
                BRT _LeBrt=new BRT();
                if (pDemande.Branchement != null)
                {
                    _LeBrt = Entities.ConvertObject<BRT, CsBrt>(pDemande.Branchement);
                    BRT abinit = _LeContextInter.BRT.FirstOrDefault(p => p.FK_IDCENTRE == _LeBrt.FK_IDCENTRE && p.CENTRE == _LeBrt.CENTRE && _LeBrt.CLIENT == p.CLIENT);
                    _LeBrt.PK_ID = abinit.PK_ID;
                    _LeBrt.FK_IDAG  = abinit.FK_IDAG ;
                }


                List<CANALISATION> _LeCompteur = new List<CANALISATION>();
                if (pDemande.LstCanalistion != null)
                    _LeCompteur = Entities.ConvertObject<CANALISATION, CsCanalisation>(pDemande.LstCanalistion);

                List<COMPTEUR> lstcompteur = new List<COMPTEUR>();
                foreach (var item_ in pDemande.LstCanalistion)
                {
                    MAGASINVIRTUEL item = pContext.MAGASINVIRTUEL.FirstOrDefault(m => m.PK_ID == item_.FK_IDMAGAZINVIRTUEL );
                    COMPTEUR can = new COMPTEUR();
                    can.ANNEEFAB = item.ANNEEFAB;
                    can.CADRAN = item.CADRAN;
                    can.COEFCOMPTAGE = item.COEFCOMPTAGE;
                    can.COEFLECT = item.COEFLECT;
                    can.DATECREATION = item.DATECREATION;
                    can.DATEMODIFICATION = item.DATEMODIFICATION;
                    can.FK_IDCALIBRE  = item.FK_IDCALIBRECOMPTEUR ;
                    can.FK_IDMARQUECOMPTEUR = item.FK_IDMARQUECOMPTEUR;
                    can.FK_IDPRODUIT = item.FK_IDPRODUIT;
                    //can.FK_IDTYPECOMPTAGE = item.FK_IDTYPECOMPTEUR;
                    can.FK_IDTYPECOMPTEUR = item.FK_IDTYPECOMPTEUR;
                    can.FONCTIONNEMENT = item.FONCTIONNEMENT;
                    can.MARQUE = item.MARQUE;
                    can.NUMERO = item.NUMERO;
                    can.PLOMBAGE = item.PLOMBAGE;
                    can.PRODUIT = item.PRODUIT.CODE  ;
                    //can.TYPECOMPTAGE = item.TYPECOMPTAGE;
                    can.TYPECOMPTEUR = pContext.TYPECOMPTEUR.FirstOrDefault(t => t.PK_ID == item.FK_IDTYPECOMPTEUR).CODE;
                    can.USERCREATION = item.USERCREATION;
                    can.USERMODIFICATION = item.USERMODIFICATION;
                    lstcompteur.Add(can);
                }

                List<EVENEMENT> _LstEvenement = new List<EVENEMENT>();
                if (pDemande.LstEvenement != null)
                    _LstEvenement = Entities.ConvertObject<EVENEMENT, CsEvenement>(pDemande.LstEvenement);

                if (pDemande.LstCoutDemande != null && pDemande.LstCoutDemande.Count != 0)
                {
                    foreach (CsDemandeDetailCout item in pDemande.LstCoutDemande)
                    {
                        if (item.COPER == Enumere.CoperCAU)
                        {
                            _LeAbon.AVANCE = (item.MONTANTHT + item.MONTANTTAXE);
                            _LeAbon.DAVANCE = item.DATECREATION;
                        }
                        LCLIENT lelClient = pContext.LCLIENT.FirstOrDefault(t => t.CENTRE == item.CENTRE && 
                                                                                 t.CLIENT == item.CLIENT && 
                                                                                 t.ORDRE == item.ORDRE && 
                                                                                 t.FK_IDCENTRE == item.FK_IDCENTRE && 
                                                                                 t.NDOC == item.NDOC && 
                                                                                 t.REFEM == item.REFEM);
                        if (lelClient != null && !string.IsNullOrEmpty(lelClient.CENTRE))
                        {
                            lelClient.FK_IDCLIENT = _LeClient.PK_ID;
                            lelClient.COPER = item.COPER;
                            lelClient.FK_IDCOPER = item.FK_IDCOPER;
                            lelClient.MONTANT = (item.MONTANTHT + item.MONTANTTAXE);
                        }
                    }
                }
                _LeContextInter.Dispose();

                Entities.InsertEntity<COMPTEUR>(lstcompteur, pContext);

                _LeCompteur.ForEach(t => t.FK_IDABON = _LeAbon.PK_ID);
                _LeCompteur.ForEach(t => t.FK_IDCOMPTEUR = lstcompteur.FirstOrDefault().PK_ID);
                Entities.InsertEntity<CANALISATION>(_LeCompteur, pContext);

                foreach (EVENEMENT item in _LstEvenement)
                {
                    item.FK_IDABON = _LeAbon.PK_ID;
                    item.FK_IDCANALISATION = _LeCompteur.FirstOrDefault(t => t.POINT == item.POINT).PK_ID;
                    item.FK_IDCOMPTEUR = lstcompteur.FirstOrDefault().PK_ID;
                }
                Entities.InsertEntity<EVENEMENT>(_LstEvenement, pContext);

                if (_LeAbon != null && !string.IsNullOrEmpty(_LeAbon.CLIENT))
                    Entities.UpdateEntity<ABON>(_LeAbon, pContext);

                if (_LeBrt != null && _LeBrt.FK_IDCENTRE != 0)
                    Entities.UpdateEntity<BRT>(_LeBrt, pContext);

                 return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public static bool MiseAjoursEtalonnageCompteur(CsDemande pDemande, galadbEntities pContext)
        {
            try
            {
                int leIdCentre = pDemande.LaDemande.FK_IDCENTRE;
                int? leIdProduit = pDemande.LaDemande.FK_IDPRODUIT;
                galadbEntities _LeContextInter = new galadbEntities();

                List<CANALISATION> lesAncCompteur = new List<CANALISATION>();
                ABON _LeAbon = _LeContextInter.ABON.FirstOrDefault(t => t.FK_IDCENTRE == pDemande.LaDemande.FK_IDCENTRE && t.CENTRE == pDemande.LaDemande.CENTRE && t.CLIENT == pDemande.LaDemande.CLIENT && t.ORDRE == pDemande.LaDemande.ORDRE);
                List<DCANALISATION> lesComptDemande = _LeContextInter.DCANALISATION.Where(t => t.FK_IDDEMANDE == pDemande.LaDemande.PK_ID).ToList();
                if (_LeAbon != null)
                {
                    lesAncCompteur = pContext.CANALISATION.Where(t => t.FK_IDABON == _LeAbon.PK_ID && t.DEPOSE == null).ToList();
                    foreach (CANALISATION item in lesAncCompteur)
                        item.DEPOSE = System.DateTime.Today;
                }
                List<CANALISATION> _LeCompteur = new List<CANALISATION>();
                if (pDemande.LstCanalistion != null)
                {
                    _LeCompteur = Entities.ConvertObject<CANALISATION, CsCanalisation>(pDemande.LstCanalistion);
                    _LeCompteur.ForEach(t => t.PROPRIO = pDemande.LeClient.PROPRIO);
                    _LeCompteur.ForEach(t => t.FK_IDPROPRIETAIRE = pContext.PROPRIETAIRE.FirstOrDefault(p => p.CODE == pDemande.LeClient.PROPRIO).PK_ID);
                }

                List<COMPTEUR> lstcompteur = new List<COMPTEUR>();

                foreach (var item_ in pDemande.LstCanalistion)
                {
                    MAGASINVIRTUEL item = pContext.MAGASINVIRTUEL.FirstOrDefault(m => m.PK_ID == item_.FK_IDMAGAZINVIRTUEL);
                    COMPTEUR can = new COMPTEUR();
                    can.ANNEEFAB = item.ANNEEFAB;
                    can.CADRAN = item.CADRAN;
                    can.COEFCOMPTAGE = item.COEFCOMPTAGE;
                    can.COEFLECT = item.COEFLECT;
                    can.DATECREATION = System.DateTime.Now;
                    can.DATEMODIFICATION = item.DATEMODIFICATION;
                    can.MISEENSERVICE = System.DateTime.Today.Date;

                    can.FK_IDCALIBRE = item.FK_IDCALIBRECOMPTEUR;
                    can.FK_IDMARQUECOMPTEUR = item.FK_IDMARQUECOMPTEUR;
                    can.FK_IDPRODUIT = item.FK_IDPRODUIT;
                    can.FK_IDTYPECOMPTEUR = item.FK_IDTYPECOMPTEUR;
                    can.FK_IDSTATUTCOMPTEUR = 1;
                    can.FK_IDETATCOMPTEUR = 1;
                    can.FONCTIONNEMENT = item.FONCTIONNEMENT;
                    can.MARQUE = item.MARQUE;

                    can.NUMERO = item.NUMERO;
                    can.TYPECOMPTEUR = item.TYPECOMPTEUR.CODE;

                    can.PLOMBAGE = item.PLOMBAGE;
                    can.PRODUIT = item_.PRODUIT;
                    can.USERCREATION = item.USERCREATION;
                    can.USERMODIFICATION = item.USERMODIFICATION;
                    lstcompteur.Add(can);

                    item.ETAT = Enumere.CompteurLie;
                }
                List<EVENEMENT> _LstEvenement = new List<EVENEMENT>();
                if (pDemande.LstEvenement != null)
                    _LstEvenement = Entities.ConvertObject<EVENEMENT, CsEvenement>(pDemande.LstEvenement);
                _LstEvenement.Where(y => y.CAS == Enumere.CasPoseCompteur).ToList().ForEach(t => t.STATUS = 99);

                List<POSE_SCELLE_DEMANDE> organeScelleDemande = new List<POSE_SCELLE_DEMANDE>();
                if (pDemande.LaDemande != null)
                    organeScelleDemande = pContext.POSE_SCELLE_DEMANDE.Where(t => t.FK_IDDEMANDE == pDemande.LaDemande.PK_ID).ToList();

                BRT leBrt = _LeContextInter.BRT.FirstOrDefault(t => t.FK_IDCENTRE == pDemande.LaDemande.FK_IDCENTRE && t.CENTRE == pDemande.LaDemande.CENTRE && t.CLIENT == pDemande.LaDemande.CLIENT);
                if (organeScelleDemande != null && organeScelleDemande.Count != 0)
                {
                    organeScelleDemande.ForEach(t => t.FK_IDBRT = leBrt.PK_ID);
                    organeScelleDemande.ForEach(t => t.USERCREATION = pDemande.LaDemande.USERCREATION);
                    organeScelleDemande.ForEach(t => t.DATECREATION = System.DateTime.Now);
                }

                Entities.InsertEntity<COMPTEUR>(lstcompteur, pContext);
                foreach (CANALISATION  item in _LeCompteur)
                {
                   item.PK_ID = lesComptDemande.FirstOrDefault(u => u.POINT == item.POINT).PK_ID;
                }
                _LeCompteur.ForEach(t => t.FK_IDABON = _LeAbon.PK_ID);
                _LeCompteur.ForEach(t => t.FK_IDCOMPTEUR = lstcompteur.FirstOrDefault().PK_ID);

                Entities.InsertEntity<CANALISATION>(_LeCompteur, pContext);
                int nuEvt = 0;

                foreach (EVENEMENT item in _LstEvenement.OrderByDescending(t => t.CAS))
                {

                    if (nuEvt == 0)
                    {
                        List<EVENEMENT> lesVtc = _LeContextInter.EVENEMENT.Where(t => t.FK_IDCENTRE == item.FK_IDCENTRE &&
                                                                                      t.CENTRE == item.CENTRE &&
                                                                                      t.CLIENT == item.CLIENT &&
                                                                                      t.ORDRE == item.ORDRE &&
                                                                                      t.POINT == item.POINT).ToList();
                        if (lesVtc != null && lesVtc.Count != 0)
                            nuEvt = lesVtc.Max(t => t.NUMEVENEMENT);
                    }
                    nuEvt = nuEvt + 1;
                    item.NUMEVENEMENT = nuEvt;
                    item.FK_IDABON = _LeAbon.PK_ID;
                    item.FK_IDCANALISATION = _LeCompteur.FirstOrDefault(t => t.POINT == item.POINT).PK_ID;
                    item.FK_IDCOMPTEUR = lstcompteur.FirstOrDefault().PK_ID;
                }
                Entities.InsertEntity<EVENEMENT>(_LstEvenement, pContext);

                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static bool MiseAjoursChangementCompteur(CsDemande pDemande, galadbEntities pContext)
        {
            try
            {
                int leIdCentre = pDemande.LaDemande.FK_IDCENTRE;
                int? leIdProduit = pDemande.LaDemande.FK_IDPRODUIT;
                galadbEntities _LeContextInter = new galadbEntities();

                List<CANALISATION> lesAncCompteur = new List<CANALISATION>();
                ABON _LeAbon = _LeContextInter.ABON.FirstOrDefault(t => t.FK_IDCENTRE == pDemande.LaDemande.FK_IDCENTRE && t.CENTRE == pDemande.LaDemande.CENTRE && t.CLIENT == pDemande.LaDemande.CLIENT && t.ORDRE == pDemande.LaDemande.ORDRE);
                if (_LeAbon != null)
                {
                    lesAncCompteur = pContext.CANALISATION.Where(t => t.FK_IDABON == _LeAbon.PK_ID && t.DEPOSE == null).ToList();
                    foreach (CANALISATION item in lesAncCompteur)
                        item.DEPOSE = System.DateTime.Today;
                }
                List<CANALISATION> _LeCompteur = new List<CANALISATION>();
                COMPTEUR _LeCompteurNouv = new COMPTEUR();
                if (pDemande.LstCanalistion != null)
                {
                    _LeCompteur = Entities.ConvertObject<CANALISATION, CsCanalisation>(pDemande.LstCanalistion.Where(t=>t.CAS == Enumere.CasPoseCompteur).ToList());
                    _LeCompteur.ForEach(t => t.PROPRIO = pDemande.LeClient.PROPRIO);
                    _LeCompteur.ForEach(t => t.FK_IDPROPRIETAIRE = pContext.PROPRIETAIRE.FirstOrDefault(p => p.CODE == pDemande.LeClient.PROPRIO).PK_ID);
                }

                List<COMPTEUR> lstcompteur = new List<COMPTEUR>();

                foreach (var item_ in pDemande.LstCanalistion.Where(t=>t.CAS == Enumere.CasPoseCompteur).ToList())
                {
                    MAGASINVIRTUEL item = pContext.MAGASINVIRTUEL.FirstOrDefault(m => m.PK_ID == item_.FK_IDMAGAZINVIRTUEL);
                    COMPTEUR can = new COMPTEUR();
                    can.ANNEEFAB = item.ANNEEFAB;
                    can.CADRAN = item.CADRAN;
                    can.COEFCOMPTAGE = item.COEFCOMPTAGE;
                    can.COEFLECT = item.COEFLECT;
                    can.DATECREATION = System.DateTime.Now;
                    can.DATEMODIFICATION = item.DATEMODIFICATION;
                    can.MISEENSERVICE = System.DateTime.Today.Date;

                    can.FK_IDCALIBRE = item.FK_IDCALIBRECOMPTEUR;
                    can.FK_IDMARQUECOMPTEUR = item.FK_IDMARQUECOMPTEUR;
                    can.FK_IDPRODUIT = item.FK_IDPRODUIT;
                    can.FK_IDTYPECOMPTEUR = item.FK_IDTYPECOMPTEUR;
                    can.FK_IDSTATUTCOMPTEUR = 1;
                    can.FK_IDETATCOMPTEUR = 1;
                    can.FONCTIONNEMENT = item.FONCTIONNEMENT;
                    can.MARQUE = item.MARQUE;

                    can.NUMERO = item.NUMERO;
                    can.TYPECOMPTEUR = item.TYPECOMPTEUR.CODE ;

                    can.PLOMBAGE = item.PLOMBAGE;
                    can.PRODUIT = item_.PRODUIT;
                    can.USERCREATION = item.USERCREATION;
                    can.USERMODIFICATION = item.USERMODIFICATION;

                    lstcompteur.Add(can);
                }

                List<EVENEMENT> _LstEvenement = new List<EVENEMENT>();
                if (pDemande.LstEvenement != null)
                    _LstEvenement = Entities.ConvertObject<EVENEMENT, CsEvenement>(pDemande.LstEvenement);
                _LstEvenement.Where(y=>y.CAS == Enumere.CasPoseCompteur ).ToList().ForEach(t => t.STATUS = 99);

                List<POSE_SCELLE_DEMANDE> organeScelleDemande = new List<POSE_SCELLE_DEMANDE>();
                if (pDemande.LaDemande != null)
                    organeScelleDemande = pContext.POSE_SCELLE_DEMANDE.Where(t => t.FK_IDDEMANDE == pDemande.LaDemande.PK_ID).ToList();

                BRT leBrt = _LeContextInter.BRT.FirstOrDefault(t => t.FK_IDCENTRE == pDemande.LaDemande.FK_IDCENTRE && t.CENTRE == pDemande.LaDemande.CENTRE && t.CLIENT == pDemande.LaDemande.CLIENT);
                if (organeScelleDemande != null && organeScelleDemande.Count != 0)
                {
                    organeScelleDemande.ForEach(t => t.FK_IDBRT = leBrt.PK_ID);
                    organeScelleDemande.ForEach(t => t.USERCREATION = pDemande.LaDemande.USERCREATION);
                    organeScelleDemande.ForEach(t => t.DATECREATION = System.DateTime.Now);
                }
                Entities.InsertEntity<COMPTEUR>(lstcompteur, pContext);
                foreach (CsCanalisation item in pDemande.LstCanalistion.Where(t=>t.CAS == Enumere.CasPoseCompteur ).ToList())
                {
                    CANALISATION leCan = _LeCompteur.FirstOrDefault(t => t.POINT == item.POINT);
                    COMPTEUR leCompteurDuPoint = lstcompteur.FirstOrDefault(t => t.TYPECOMPTEUR == item.TYPECOMPTEUR);
                    leCan.FK_IDCOMPTEUR = leCompteurDuPoint.PK_ID;
                    leCan.FK_IDABON = _LeAbon.PK_ID;
                }
                Entities.InsertEntity<CANALISATION>(_LeCompteur, pContext);
                int nuEvt = 0;
                foreach (EVENEMENT item in _LstEvenement.OrderByDescending(t=>t.CAS ))
                {
                    if (nuEvt == 0)
                    {
                        List<EVENEMENT> lesVtc = _LeContextInter.EVENEMENT.Where(t => t.FK_IDCENTRE == item.FK_IDCENTRE &&
                                                                                      t.CENTRE == item.CENTRE &&
                                                                                      t.CLIENT == item.CLIENT &&
                                                                                      t.ORDRE == item.ORDRE &&
                                                                                      t.POINT == item.POINT).ToList();
                        if (lesVtc != null && lesVtc.Count != 0)
                            nuEvt = lesVtc.Max(t => t.NUMEVENEMENT);
                    }
                    nuEvt = nuEvt + 1;
                    item.NUMEVENEMENT = nuEvt;
                    item.FK_IDABON = _LeAbon.PK_ID;
                    item.FK_IDCANALISATION = _LeCompteur.FirstOrDefault(t => t.POINT == item.POINT).PK_ID;
                    item.FK_IDCOMPTEUR = lstcompteur.FirstOrDefault().PK_ID;
                }
                Entities.InsertEntity<EVENEMENT>(_LstEvenement, pContext);
                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        
        public static bool MiseAjoursFermetureOuvertureBrt(CsDemande pDemande, galadbEntities pContext)
        {
            try
            {
                int leIdCentre = pDemande.LaDemande.FK_IDCENTRE;
                int? leIdProduit = pDemande.LaDemande.FK_IDPRODUIT;
                galadbEntities _LeContextInter = new galadbEntities();

                BRT _LeBrt = new BRT();
                if (pDemande.Branchement != null)
                {
                    _LeBrt = Entities.ConvertObject<BRT, CsBrt>(pDemande.Branchement);
                    _LeBrt.FK_IDCENTRE = leIdCentre;
                    _LeBrt.FK_IDPRODUIT = leIdProduit.Value ;

                    TYPEBRANCHEMENT FK_IDTYPEBR = _LeContextInter.TYPEBRANCHEMENT.FirstOrDefault(p => p.PK_ID == _LeBrt.FK_IDTYPEBRANCHEMENT );
                   if(FK_IDTYPEBR != null)
                       _LeBrt.FK_IDTYPEBRANCHEMENT = FK_IDTYPEBR.PK_ID;
                   //TOURNEE FK_IDTOUR = _LeContextInter.TOURNEE.FirstOrDefault(p => p.CODE == _LeBrt.TOURNEE);
                   //if (FK_IDTYPEBR != null)
                   //    _LeBrt.FK_IDTOURNEE = FK_IDTOUR.PK_ID;
                    //_LeBrt.FK_IDCLIENT  = _LeContextInter.CLIENT .FirstOrDefault(p => p.CENTRE  == _LeBrt.CENTRE1.CODECENTRE  && p.REFCLIENT == _LeBrt.CLIENT.REFCLIENT ).PK_ID;
                }

                List<EVENEMENT> _LstEvenement = new List<EVENEMENT>();
                if (pDemande.LstEvenement != null)
                {
                    _LstEvenement = Entities.ConvertObject<EVENEMENT, CsEvenement>(pDemande.LstEvenement);
                    foreach (EVENEMENT item in _LstEvenement)
                    {
                        //item.FK_IDCAS = _LeContextInter.CASIND.FirstOrDefault(p => p.CODE  == item.CAS).PK_ID;
                        item.FK_IDCENTRE = leIdCentre;
                        item.FK_IDPRODUIT = leIdProduit.Value ;
                        //item.FK_IDCANALISATION = _LeContextInter.CANALISATION.FirstOrDefault(p => p.CENTRE == item.CENTRE && p.CLIENT == item.CLIENT && p.POINT == item.POINT).PK_ID;
                    }
                }

                Entities.InsertEntity<EVENEMENT>(_LstEvenement,pContext );
                Entities.UpdateEntity<BRT>(_LeBrt, pContext);
                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static bool MiseAjoursDepannagePrepaye(CsDemande pDemande, galadbEntities pContext)
        {
            try
            {
                int leIdCentre = pDemande.LaDemande.FK_IDCENTRE;
                int? leIdProduit = pDemande.LaDemande.FK_IDPRODUIT;
                galadbEntities _LeContextInter = new galadbEntities();

                List<CANALISATION> lesAncCompteur = new List<CANALISATION>();
                ABON _LeAbon = _LeContextInter.ABON.FirstOrDefault(t => t.FK_IDCENTRE == pDemande.LaDemande.FK_IDCENTRE && t.CENTRE == pDemande.LaDemande.CENTRE && t.CLIENT == pDemande.LaDemande.CLIENT && t.ORDRE == pDemande.LaDemande.ORDRE);
                if (_LeAbon != null)
                {
                    lesAncCompteur = pContext.CANALISATION.Where(t => t.FK_IDABON == _LeAbon.PK_ID && t.DEPOSE == null).ToList();
                    foreach (CANALISATION item in lesAncCompteur)
                        item.DEPOSE = System.DateTime.Today;
                }

                DCANALISATION _LstDacanalisation = _LeContextInter.DCANALISATION.FirstOrDefault (t => t.FK_IDDEMANDE  == pDemande.LaDemande.PK_ID);

                List<CANALISATION> _LeCompteur = new List<CANALISATION>();
                COMPTEUR _LeCompteurNouv = new COMPTEUR();
                if (pDemande.LstCanalistion != null)
                {
                    _LeCompteur = Entities.ConvertObject<CANALISATION, CsCanalisation>(pDemande.LstCanalistion);
                    _LeCompteur.ForEach(t => t.PROPRIO = pDemande.LeClient.PROPRIO);
                    _LeCompteur.ForEach(t => t.PK_ID = _LstDacanalisation.PK_ID );
                    _LeCompteur.ForEach(t => t.FK_IDPROPRIETAIRE = pContext.PROPRIETAIRE.FirstOrDefault(p => p.CODE == pDemande.LeClient.PROPRIO).PK_ID);
                }

                List<COMPTEUR> lstcompteur = new List<COMPTEUR>();

                foreach (var item_ in pDemande.LstCanalistion)
                {
                    MAGASINVIRTUEL item = pContext.MAGASINVIRTUEL.FirstOrDefault(m => m.PK_ID == item_.FK_IDMAGAZINVIRTUEL);
                    COMPTEUR can = new COMPTEUR();
                    can.ANNEEFAB = item.ANNEEFAB;
                    can.CADRAN = item.CADRAN;
                    can.COEFCOMPTAGE = item.COEFCOMPTAGE;
                    can.COEFLECT = item.COEFLECT;
                    can.DATECREATION = System.DateTime.Now;
                    can.DATEMODIFICATION = item.DATEMODIFICATION;
                    can.MISEENSERVICE = System.DateTime.Today.Date;

                    can.FK_IDCALIBRE = item.FK_IDCALIBRECOMPTEUR;
                    can.FK_IDMARQUECOMPTEUR = item.FK_IDMARQUECOMPTEUR;
                    can.FK_IDPRODUIT = item.FK_IDPRODUIT;
                    can.FK_IDTYPECOMPTEUR = item.FK_IDTYPECOMPTEUR;
                    can.FK_IDSTATUTCOMPTEUR = 1;
                    can.FK_IDETATCOMPTEUR = 1;
                    can.FONCTIONNEMENT = item.FONCTIONNEMENT;
                    can.MARQUE = item.MARQUE;

                    can.NUMERO = item.NUMERO;
                    can.TYPECOMPTEUR = item.TYPECOMPTEUR.CODE;

                    can.PLOMBAGE = item.PLOMBAGE;
                    can.PRODUIT = item_.PRODUIT;
                    can.USERCREATION = item.USERCREATION;
                    can.USERMODIFICATION = item.USERMODIFICATION;

                    lstcompteur.Add(can);
                }

                List<EVENEMENT> _LstEvenement = new List<EVENEMENT>();
                if (pDemande.LstEvenement != null)
                    _LstEvenement = Entities.ConvertObject<EVENEMENT, CsEvenement>(pDemande.LstEvenement);
                _LstEvenement.Where(y => y.CAS == Enumere.CasPoseCompteur).ToList().ForEach(t => t.STATUS = 99);

                List<POSE_SCELLE_DEMANDE> organeScelleDemande = new List<POSE_SCELLE_DEMANDE>();
                if (pDemande.LaDemande != null)
                    organeScelleDemande = pContext.POSE_SCELLE_DEMANDE.Where(t => t.FK_IDDEMANDE == pDemande.LaDemande.PK_ID).ToList();

                BRT leBrt = _LeContextInter.BRT.FirstOrDefault(t => t.FK_IDCENTRE == pDemande.LaDemande.FK_IDCENTRE && t.CENTRE == pDemande.LaDemande.CENTRE && t.CLIENT == pDemande.LaDemande.CLIENT);
                if (organeScelleDemande != null && organeScelleDemande.Count != 0)
                {
                    organeScelleDemande.ForEach(t => t.FK_IDBRT = leBrt.PK_ID);
                    organeScelleDemande.ForEach(t => t.USERCREATION = pDemande.LaDemande.USERCREATION);
                    organeScelleDemande.ForEach(t => t.DATECREATION = System.DateTime.Now);
                }

                Entities.InsertEntity<COMPTEUR>(lstcompteur, pContext);


                foreach (CsCanalisation item in pDemande.LstCanalistion)
                {
                    CANALISATION leCan = _LeCompteur.FirstOrDefault(t => t.POINT == item.POINT);
                    COMPTEUR leCompteurDuPoint = lstcompteur.FirstOrDefault(t => t.TYPECOMPTEUR == item.TYPECOMPTEUR);
                    leCan.FK_IDCOMPTEUR = leCompteurDuPoint.PK_ID;
                    leCan.FK_IDABON = _LeAbon.PK_ID;
                }
                Entities.InsertEntity<CANALISATION>(_LeCompteur, pContext);


                foreach (EVENEMENT item in _LstEvenement)
                {
                    item.FK_IDABON = _LeAbon.PK_ID;
                    item.FK_IDCANALISATION = _LeCompteur.FirstOrDefault(t => t.POINT == item.POINT).PK_ID;
                    item.FK_IDCOMPTEUR = lstcompteur.FirstOrDefault().PK_ID;
                }
                int nuEvt = 0;

                foreach (EVENEMENT item in _LstEvenement.OrderByDescending(t => t.CAS))
                {

                    if (nuEvt == 0)
                    {
                        List<EVENEMENT> lesVtc = _LeContextInter.EVENEMENT.Where(t => t.FK_IDCENTRE == item.FK_IDCENTRE &&
                                                                                      t.CENTRE == item.CENTRE &&
                                                                                      t.CLIENT == item.CLIENT &&
                                                                                      t.ORDRE == item.ORDRE &&
                                                                                      t.POINT == item.POINT).ToList();
                        if (lesVtc != null && lesVtc.Count != 0)
                            nuEvt = lesVtc.Max(t => t.NUMEVENEMENT);
                    }
                    nuEvt = nuEvt + 1;
                    item.NUMEVENEMENT = nuEvt;
                    item.FK_IDABON = _LeAbon.PK_ID;
                    item.FK_IDCANALISATION = _LeCompteur.FirstOrDefault(t => t.POINT == item.POINT).PK_ID;
                    item.FK_IDCOMPTEUR = lstcompteur.FirstOrDefault().PK_ID;
                }
                Entities.InsertEntity<EVENEMENT>(_LstEvenement, pContext);

                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        //public static bool MiseAjoursSuppressionPagisol(CsPagisol _LePagisol)
        //{
        //    try
        //    {
        //        PAGISOL _Pagisol = new PAGISOL();
        //        EVENEMENT _LeEvt = new EVENEMENT();
        //       using (galadbEntities ctontext = new galadbEntities())
        //          {
        //          _Pagisol = Entities.ConvertObject<PAGISOL, CsPagisol>(_LePagisol);
        //          _LeEvt = ctontext.EVENEMENT.FirstOrDefault(p => p.PK_ID == _Pagisol.FK_IDEVENEMENT);
        //          if (_LeEvt != null)
        //             _LeEvt.STATUS = Enumere.EvenementSupprimer;
        //           };
        //           using (galadbEntities pcontext = new galadbEntities())
        //           {
        //               Entities.DeleteEntity(_Pagisol, pcontext);
        //               Entities.UpdateEntity(_LeEvt, pcontext);
        //               pcontext.SaveChanges();
        //           };
        //       return true;
        //    }
        //    catch (Exception e)
        //    {
        //        throw e;
        //    }
        //}
        public static bool MiseTableTemp(DEMANDE _LaDemandeExiste, DCLIENT Leclient, DAG Ag, DABON Abonnement, DBRT branchement,
                                         List<DEVENEMENT> _LstEvenement, List<RUBRIQUEDEMANDE> _LstDemandeCout,
                                         List<DCANALISATION> _LstCompteur, List<ELEMENTDEVIS> _LstEltDevis, 
                                         List<APPAREILSDEVIS> _LstEltAppareil, TRAVAUXDEVIS leTrvx, 
                                         List<FRAIXPARICIPATIONDEVIS> _LstFraixParticipation,
                                         List<POSE_SCELLE_DEMANDE> _LstOrganeScelleDemande,
                                         DORDRETRAVAIL _LeOT, CONTROLETRAVAUX leControle,
                                         List<DCOMPTEUR> lstdCompteur, SUIVIDEMANDE _LesuivisDemande, 
                                         DANNOTATION leAnnotation, DEPANNAGE  leDepannage, 
                                         DPERSONNEPHYSIQUE laPersonnePhysique,
                                         DSOCIETEPRIVE laSociete,DADMINISTRATION_INSTITUT leAdministration,
                                         DINFOPROPRIETAIRE lesInfoProprio,
                                        galadbEntities pContext)
        {
            try
            {

                if (Leclient != null && !string.IsNullOrEmpty(Leclient.NUMDEM))
                    MisAjourDClient(_LaDemandeExiste, Leclient, pContext);

                if (Ag != null && !string.IsNullOrEmpty(Ag.NUMDEM))
                    MisAjourDAg(_LaDemandeExiste, Ag, pContext);

                if (Abonnement != null && !string.IsNullOrEmpty(Abonnement.NUMDEM))
                    MisAjourDAbon(_LaDemandeExiste, Abonnement, pContext);

                if (branchement != null && !string.IsNullOrEmpty(branchement.NUMDEM))
                    MisAjourDbrt(_LaDemandeExiste, branchement, pContext);

                if (_LstCompteur != null && _LstCompteur.Count != 0)
                    MisAjourDCanalisation(_LaDemandeExiste, _LstCompteur, pContext);

                if (_LstDemandeCout != null && _LstDemandeCout.Count != 0)
                    MisAjourDetailDemande(_LaDemandeExiste, _LstDemandeCout, pContext);

                if (_LstEvenement != null && _LstEvenement.Count != 0)
                    MisAjourDEvenement(_LaDemandeExiste, _LstEvenement, pContext);

                if (_LstEltDevis != null && _LstEltDevis.Count != 0)
                    MisAjourEltDevis(_LaDemandeExiste, _LstEltDevis, pContext);

                if (_LstEltAppareil != null && _LstEltAppareil.Count != 0)
                    MisAjourEltAppareil(_LaDemandeExiste, _LstEltAppareil, pContext);

                if (leTrvx != null && leTrvx.ORDRE != 0)
                    MisAjourTravaux(_LaDemandeExiste, leTrvx, pContext);
                if (_LstFraixParticipation != null && _LstFraixParticipation.Count != 0)
                    MisAjourFraixParticipation(_LaDemandeExiste, _LstFraixParticipation, pContext);

                if (_LstOrganeScelleDemande != null && _LstOrganeScelleDemande.Count != 0)
                    MisAjourOrganeScelleDemande(_LaDemandeExiste, _LstOrganeScelleDemande, pContext);

                if (_LeOT != null && !string.IsNullOrEmpty(_LeOT.MATRICULE))
                    MisAjourDOrdreTravail(_LaDemandeExiste, _LeOT, pContext);

                if (leControle != null && !string.IsNullOrEmpty(leControle.COMMENTAIRE))
                    MisAjourControlTravaux(_LaDemandeExiste, leControle, pContext);

                if (lstdCompteur != null && lstdCompteur.Count != 0)
                    MisAjourDCompteur(_LaDemandeExiste, lstdCompteur, pContext);

                if (leAnnotation != null && !string.IsNullOrEmpty(leAnnotation.MATRICULE))
                    MisAjourAnnotationDemande(_LaDemandeExiste, leAnnotation, pContext);

                if (leDepannage != null && leDepannage.FK_IDCOMMUNE != 0 && leDepannage.FK_IDCOMMUNE != null)
                    MisAjourDepannageDemande(_LaDemandeExiste, leDepannage, pContext);

                if (laPersonnePhysique != null && !string.IsNullOrEmpty(laPersonnePhysique.NOMABON))
                    MisAjourDPersonnePhysique(_LaDemandeExiste, laPersonnePhysique, pContext);

                if (laSociete != null && !string.IsNullOrEmpty(laSociete.NOMABON))
                    MisAjourDsociete(_LaDemandeExiste, laSociete, pContext);

                if (leAdministration != null && !string.IsNullOrEmpty(leAdministration.NOMABON))
                    MisAjourDAdministration(_LaDemandeExiste, leAdministration, pContext);

                if (lesInfoProprio != null && lesInfoProprio.DATENAISSANCE != null)
                    MisAjourDInfoProprietaire(_LaDemandeExiste, lesInfoProprio, pContext);

                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public static bool MiseAJourModifClientAbonnement(CsDemande pDemande, galadbEntities pContext)
        {
            try
            {
                galadbEntities _LeContextInter = new galadbEntities();
                int leIdCentre =_LeContextInter.CENTRE.FirstOrDefault(c=>c.CODE==pDemande.LaDemande.CENTRE).PK_ID;
                int? leIdProduit = pDemande.LaDemande.FK_IDPRODUIT;
                bool Resultat = false;
                CLIENT _LeClient = new CLIENT();
                if (pDemande.LeClient != null)
                {
                    _LeClient = Entities.ConvertObject<CLIENT, CsClient>(pDemande.LeClient);
                    _LeClient.FK_IDCENTRE = leIdCentre;
                    _LeClient.FK_IDCODECONSO = _LeContextInter.CODECONSOMMATEUR.FirstOrDefault(p => p.CODE  == _LeClient.CODECONSO).PK_ID;
                    _LeClient.FK_IDRELANCE = _LeContextInter.RELANCE.FirstOrDefault(p => p.CODE == _LeClient.CODERELANCE).PK_ID;
                    _LeClient.FK_IDCATEGORIE = _LeContextInter.CATEGORIECLIENT.FirstOrDefault(p => p.CODE == _LeClient.CATEGORIE).PK_ID;
                    _LeClient.FK_IDMODEPAIEMENT = _LeContextInter.MODEPAIEMENT.FirstOrDefault(p => p.CODE == _LeClient.MODEPAIEMENT) != null ? _LeContextInter.RELANCE.FirstOrDefault(p => p.CODE == _LeClient.MODEPAIEMENT).PK_ID : _LeClient.FK_IDMODEPAIEMENT;
                    _LeClient.FK_IDMODEPAIEMENT = 9;
                    //_LeClient.FK_IDMODEP = _LeContextInter.MODEPAIEMENT.FirstOrDefault(p => p.CODE_MODEP == _LeClient.MODEP) != null ? _LeContextInter.RELANCE.FirstOrDefault(p => p.CODE == _LeClient.MODEP).PK_ID : _LeClient.FK_IDMODEP;
                    _LeClient.FK_IDNATIONALITE = _LeContextInter.NATIONALITE.FirstOrDefault(p => p.CODE  == _LeClient.NATIONNALITE) != null ? _LeContextInter.NATIONALITE.FirstOrDefault(p => p.CODE == _LeClient.NATIONNALITE).PK_ID : _LeClient.FK_IDNATIONALITE;
                    //_LeClient.FK_IDREGCLI = _LeContextInter.REGCLI.FirstOrDefault(p => p.CODE == _LeClient.r) != null ? _LeContextInter.REGCLI.FirstOrDefault(p => p.CODE == _LeClient.REGCLI).PK_ID : _LeClient.FK_IDREGCLI;
                }

                ABON _LeAbon = new ABON();
                if (pDemande.Abonne != null)
                {
                    _LeAbon = Entities.ConvertObject<ABON, CsAbon>(pDemande.Abonne);
                    _LeAbon.FK_IDPRODUIT = leIdProduit.Value ;
                    _LeAbon.FK_IDCENTRE = leIdCentre;
                    _LeAbon.FK_IDTYPETARIF = _LeContextInter.TYPETARIF.FirstOrDefault(p => p.CODE == pDemande.Abonne.TYPETARIF) != null ? _LeContextInter.TYPETARIF.FirstOrDefault(p => p.CODE == pDemande.Abonne.TYPETARIF).PK_ID : _LeAbon.FK_IDTYPETARIF;
                    _LeAbon.FK_IDMOISFAC = _LeContextInter.MOIS.FirstOrDefault(p => p.CODE == pDemande.Abonne.MOISFAC) != null ? _LeContextInter.MOIS.FirstOrDefault(p => p.CODE == pDemande.Abonne.MOISFAC).PK_ID : _LeAbon.FK_IDMOISFAC;
                    _LeAbon.FK_IDMOISREL = _LeContextInter.MOIS.FirstOrDefault(p => p.CODE == pDemande.Abonne.MOISREL) != null ? _LeContextInter.MOIS.FirstOrDefault(p => p.CODE == pDemande.Abonne.MOISREL).PK_ID : _LeAbon.FK_IDMOISREL;
                    _LeAbon.FK_IDFORFAIT = _LeContextInter.FORFAIT.FirstOrDefault(p => p.CODE == pDemande.Abonne.FORFAIT) != null ? _LeContextInter.FORFAIT.FirstOrDefault(p => p.CODE == pDemande.Abonne.FORFAIT).PK_ID : _LeAbon.FK_IDFORFAIT;
                }
                _LeContextInter.Dispose();
                Resultat = Entities.UpdateEntity<ABON>(_LeAbon, pContext);
                return Resultat;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public static bool MiseAJourModifAbonnement(CsDemande pDemande, galadbEntities pContext)
        {
            try
            {
               galadbEntities _LeContextInter = new galadbEntities();
               bool Resultat = false;
               ABON _LeAbon = new ABON();
                if (pDemande.Abonne  != null)
                {
                    _LeAbon = Entities.ConvertObject<ABON , CsAbon >(pDemande.Abonne );
                    ABON Abon = _LeContextInter.ABON.FirstOrDefault(t => t.CENTRE == _LeAbon.CENTRE && t.CLIENT == _LeAbon.CLIENT && t.FK_IDCENTRE == _LeAbon.FK_IDCENTRE && t.ORDRE  == _LeAbon.ORDRE );
                    _LeAbon.FK_IDCLIENT = Abon.FK_IDCLIENT;
                    _LeAbon.PK_ID = Abon.PK_ID;
                    _LeAbon.USERCREATION = Abon.USERCREATION;
                    _LeAbon.DATECREATION = Abon.DATECREATION;
                    if (Abon.DRES != null && _LeAbon.DRES == null)
                    {
                        List<ABON> AbonOrdreSup = _LeContextInter.ABON.Where (t => t.CENTRE == _LeAbon.CENTRE && t.CLIENT == _LeAbon.CLIENT && t.FK_IDCENTRE == _LeAbon.FK_IDCENTRE).ToList();
                        if (AbonOrdreSup != null && AbonOrdreSup.Count != 0)
                        {
                            List<int> AbonOrdreSupCon = AbonOrdreSup.Select(o => int.Parse(o.ORDRE)).ToList();
                            int ordre = int.Parse(_LeAbon.ORDRE);
                            if(!AbonOrdreSupCon.Exists(o => o > ordre))
                                LeveDeResiliation(Abon, pContext);
                        }
                    }
                }
                _LeContextInter.Dispose();
                Resultat = Entities.UpdateEntity<ABON>(_LeAbon, pContext);
                return Resultat;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static bool LeveDeResiliation(ABON  leAbonnement, galadbEntities pContext)
        {
            try
            {
                bool Resultat = false;
                if (leAbonnement.PRODUIT == Enumere.ElectriciteMT)
                {
                    for (int point = 1; point <= 6; point++)
                    {
                        CANALISATION _LeCanalisation = pContext.CANALISATION.Where(t => t.FK_IDABON == leAbonnement.PK_ID && t.POINT == point).OrderByDescending(u => u.DEPOSE).ToList().First();
                        if (_LeCanalisation != null && !string.IsNullOrEmpty(_LeCanalisation.CLIENT) && _LeCanalisation.DEPOSE != null )
                            _LeCanalisation.DEPOSE = null;
                    }
                }
                else
                {
                    CANALISATION _LCanlisation = pContext.CANALISATION.Where(t => t.FK_IDABON == leAbonnement.PK_ID).OrderByDescending(u => u.DEPOSE).ToList().First();
                    if (_LCanlisation != null && !string.IsNullOrEmpty(_LCanlisation.CLIENT) && _LCanlisation.DEPOSE != null)
                        _LCanlisation.DEPOSE = null;
                }
                return Resultat;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static bool MiseAJourModifAdresse(CsDemande pDemande, galadbEntities pContext)
        {
            try
            {
               galadbEntities _LeContextInter = new galadbEntities();
               bool Resultat = false;
                AG _LeAg = new AG();
                if (pDemande.Ag != null)
                {
                    _LeAg = Entities.ConvertObject<AG, CsAg>(pDemande.Ag);
                    AG ag = _LeContextInter.AG.FirstOrDefault(t => t.CENTRE == _LeAg.CENTRE && t.CLIENT == _LeAg.CLIENT && t.FK_IDCENTRE == _LeAg.FK_IDCENTRE);
                    _LeAg.PK_ID = ag.PK_ID;
                }
                _LeContextInter.Dispose();

                Resultat = Entities.UpdateEntity<AG>(_LeAg, pContext);

                return Resultat;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public static bool MiseAJourModifBranchement(CsDemande pDemande, galadbEntities pContext)
        {
            try
            {
                galadbEntities _LeContextInter = new galadbEntities();
                bool Resultat = false;
                BRT _LeBrt = new BRT();
                if (pDemande.Branchement  != null)
                {
                    _LeBrt = Entities.ConvertObject<BRT, CsBrt >(pDemande.Branchement );
                    BRT brt = _LeContextInter.BRT.FirstOrDefault(t => t.CENTRE == _LeBrt.CENTRE && t.CLIENT == _LeBrt.CLIENT && t.FK_IDCENTRE == _LeBrt.FK_IDCENTRE);
                    _LeBrt.FK_IDAG = brt.FK_IDAG;
                    _LeBrt.PK_ID = brt.PK_ID;
                }
                _LeContextInter.Dispose();
                Resultat = Entities.UpdateEntity<BRT>(_LeBrt, pContext);
                return Resultat;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public static bool MiseAJourModifClient(CsDemande pDemande, galadbEntities pContext)
        {
            try
            {
                galadbEntities _LeContextInter = new galadbEntities();
                bool Resultat = false;
                CLIENT _LeClient = new CLIENT();
                if (pDemande.LeClient != null)
                {
                    _LeClient = Entities.ConvertObject<CLIENT, CsClient>(pDemande.LeClient);
                    CLIENT client = _LeContextInter.CLIENT.FirstOrDefault(p => p.CENTRE == _LeClient.CENTRE && p.REFCLIENT == _LeClient.REFCLIENT && p.ORDRE == _LeClient.ORDRE && p.FK_IDCENTRE == _LeClient.FK_IDCENTRE);
                    _LeClient.FK_IDAG = client.FK_IDAG ;
                    _LeClient.PK_ID = client.PK_ID;
                }
                _LeContextInter.Dispose();
                Resultat = Entities.UpdateEntity<CLIENT>(_LeClient, pContext);
                return Resultat;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool MiseAJourModifCompteur(CsDemande pDemande, galadbEntities pContext)
        {
            try
            {
                int Resultat = -1;
                if (pDemande.LstCanalistion  != null)
                {
                    using (galadbEntities ctx = new galadbEntities())
                    {
                        List<DCOMPTEUR> _LesDCompteur = ctx.DCOMPTEUR.Where (p => p.FK_IDDEMANDE == pDemande.LaDemande.PK_ID).ToList();
                        foreach (var _LeCompteur in _LesDCompteur)
                        {
                            COMPTEUR Compteur = ctx.COMPTEUR.FirstOrDefault(p => p.PK_ID == _LeCompteur.FK_IDCOMPTEUR);
                            if (Compteur != null)
                            {
                                if (pDemande.LaDemande.PRODUIT == Enumere.ElectriciteMT )
                                Compteur.NUMERO = Compteur.NUMERO.Substring(0,5) + _LeCompteur.NUMERO;
                                else
                                    Compteur.NUMERO = _LeCompteur.NUMERO;

                                Compteur.ANNEEFAB = _LeCompteur.ANNEEFAB;
                                Compteur.CADRAN = _LeCompteur.CADRAN;
                                Compteur.MARQUE = _LeCompteur.MARQUE;
                                Compteur.FK_IDMARQUECOMPTEUR = _LeCompteur.FK_IDMARQUECOMPTEUR;
                                Compteur.FK_IDCALIBRE = _LeCompteur.FK_IDCALIBRE;
                            }
                        }
                        Resultat = ctx.SaveChanges(); 
                    }
                }
                return Resultat== -1 ?false : true ;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //public static bool MiseAJourModifCompteur(CsDemande pDemande, galadbEntities pContext)
        //{
        //    try
        //    {
        //        galadbEntities _LeContextInter = new galadbEntities();
        //        bool Resultat = false;
        //        List<CANALISATION> _LeCompteur = new List<CANALISATION>();
        //        List<CPROFAC> _LstEvtCprofac = new List<CPROFAC>();
        //        List<CsProduitFacture> _LstEvtCprofacTrans = new List<CsProduitFacture>();
        //        List<EVENEMENT> _LstEvtCompteur = new List<EVENEMENT>();
        //        List<CsEvenement> _LstEvtTrans = new List<CsEvenement>();

        //        if (pDemande.LstCanalistion != null)
        //        {
        //            //ABO07Entities _LeContextInterAbo07 = new ABO07Entities();
        //            _LeCompteur = Entities.ConvertObject<CANALISATION, CsCanalisation>(pDemande.LstCanalistion);
        //            //string AncCompteur = _LeContextInter.CANALISATION.FirstOrDefault(p => p.CENTRE == pDemande.LaDemande.CENTRE &&
        //            //                                                                             p.CLIENT == pDemande.LaDemande.CLIENT &&
        //            //                                                                             p.FK_IDCENTRE == leIdCentre).COMPTEUR;

        //            foreach (CANALISATION item in _LeCompteur)
        //            {
        //                //BRT leBrt = _LeContextInter.BRT.FirstOrDefault(p => p.CENTRE == item.CENTRE && p.CLIENT == item.CLIENT && p.FK_IDCENTRE == leIdCentre);
        //                //string Ordre = leBrt.CLIENT1.ORDRE;

        //               // item.FK_IDPRODUIT = leIdProduit;
        //               // if (item.PRODUIT.CODE  != Enumere.ElectriciteMT)
        //               //     item.FK_IDDIAMETRE = _LeContextInter.DIACOMPTEUR.FirstOrDefault(p => p.CODE == item.DIACOMPTEUR .CODE  && p.FK_IDPRODUIT  == item.FK_IDPRODUIT ).PK_ID;
        //               // else
        //               // {
        //               //     string CodeTypeComptage = item.TYPECOMPTAGE.PadLeft(4, '0');
        //               //     item.FK_IDTYPECOMPTAGE = _LeContextInter.TYPEBRANCHEMENT.FirstOrDefault(p => p.CODE == CodeTypeComptage && p.FK_IDPRODUIT  == item.FK_IDPRODUIT ).PK_ID;
        //               // }
        //               // item.FK_IDMCOMPT = _LeContextInter.MARQUECOMPTEUR.FirstOrDefault(p => p.CODE == item.MARQUECOMPTEUR.CODE ).PK_ID;
        //               // item.FK_IDTCOMPT = _LeContextInter.TCOMPTEUR .FirstOrDefault(p => p.TYPE == item.TCOMPT && p.FK_IDPRODUIT  == item.FK_IDPRODUIT ).PK_ID;
        //               // List<EVENEMENT> lstevt = _LeContextInter.EVENEMENT.Where(p => p.CENTRE == item.CENTRE && p.CLIENT == item.CLIENT && p.POINT == item.POINT && p.PRODUIT == item.PRODUIT && p.COMPTEUR == AncCompteur && p.ORDRE == Ordre).ToList();
        //               // _LstEvtTrans = Entities.ConvertObject<CsEvenement, EVENEMENT>(lstevt);
        //               ////List<CPROFAC>  _LstCprofac = _LeContextInterAbo07.CPROFAC.Where(p => p.CENTRE == item.CENTRE && p.CLIENT == item.CLIENT && p.POINT == item.POINT && p.PRODUIT == item.PRODUIT && p.COMPTEUR == AncCompteur).ToList();
        //               ////_LstEvtCprofacTrans = Entities.ConvertObject<CsProduitFacture, CPROFAC>(_LstCprofac);

        //            }
        //            //_LeContextInterAbo07.Dispose();
        //            _LeContextInter.Dispose();
        //        }
        //        Resultat = Entities.UpdateEntity<CANALISATION>(_LeCompteur, pContext);
        //        if (_LstEvtTrans != null && _LstEvtTrans.Count != 0)
        //        {
        //            _LstEvtCompteur = Entities.ConvertObject<EVENEMENT, CsEvenement>(_LstEvtTrans);
        //            foreach (EVENEMENT item in _LstEvtCompteur)
        //            {
        //                item.COMPTEUR = _LeCompteur[0].COMPTEUR.NUMERO ;
        //                //item.DIAMETRE = _LeCompteur[0].DIAMETRE;
        //                //item.TCOMPT = _LeCompteur[0].TCOMPT;
        //            }
        //            Resultat = Entities.UpdateEntity<EVENEMENT>(_LstEvtCompteur, pContext);
        //        }
        //        //if (_LstEvtCprofacTrans != null && _LstEvtCprofacTrans.Count != 0)
        //        //{
        //        //    _LstEvtCprofac = Entities.ConvertObject<CPROFAC, CsProduitFacture>(_LstEvtCprofacTrans);
        //        //    foreach (CPROFAC item in _LstEvtCprofac)
        //        //    {
        //        //        item.COMPTEUR = _LeCompteur[0].COMPTEUR;   
        //        //        item.DIAMETRE  = _LeCompteur[0].DIAMETRE  ;   
        //        //        item.TCOMPT  = _LeCompteur[0].TCOMPT ;   
        //        //    }
        //        //    Resultat = Entities.UpdateEntity<CPROFAC>(_LstEvtCprofac, pContext);
        //        //}
        //        return Resultat;

        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        public static bool MiseAJourVariationPuissance(CsDemande pDemande, galadbEntities pContext)
        {
            try
            {
                bool Resultat = true;
                galadbEntities _LeContextInter = new galadbEntities();
                List<RUBRIQUEDEMANDE> _LstDemandeCout = new List<RUBRIQUEDEMANDE>();
                DEMANDE _Demande = new DEMANDE();
                if (pDemande.LaDemande != null)
                    _Demande = Entities.ConvertObject<DEMANDE, CsDemandeBase>(pDemande.LaDemande);

                _Demande.FK_IDADMUTILISATEUR = _LeContextInter.ADMUTILISATEUR.FirstOrDefault(p => p.MATRICULE == _Demande.MATRICULE).PK_ID;
                _Demande.FK_IDTYPEDEMANDE = _LeContextInter.TYPEDEMANDE.FirstOrDefault(p => p.CODE == _Demande.TYPEDEMANDE).PK_ID;
                int IdClient = _LeContextInter.CLIENT.FirstOrDefault(p => p.CENTRE == _Demande.CENTRE && p.REFCLIENT == _Demande.CLIENT && p.ORDRE == _Demande.ORDRE && p.FK_IDCENTRE == _Demande.FK_IDCENTRE ).PK_ID;
                BRT _leBrt = _LeContextInter.BRT .FirstOrDefault(p => p.CENTRE == _Demande.CENTRE && p.CLIENT  == _Demande.CLIENT &&  p.FK_IDCENTRE == _Demande.FK_IDCENTRE );

                ABON _LeAbon = new ABON();
                if (pDemande.Abonne  != null)
                {
                    _LeAbon = pContext.ABON.First(p => p.FK_IDCLIENT == IdClient);
                    if (_LeAbon != null)
                    {
                        _LeAbon.AVANCE = pDemande.LstCoutDemande.Sum(t => t.MONTANTHT + t.MONTANTTAXE);
                        List<CANALISATION> lstCanalisation = pContext.CANALISATION.Where(t => t.FK_IDABON == _LeAbon.PK_ID && t.DEPOSE == null).ToList();
                        foreach (CANALISATION item in lstCanalisation)
                        {
                            item.DEPOSE = System.DateTime.Today;
                            item.USERMODIFICATION = pDemande.LaDemande.MATRICULE;
                        }

                    }
                }
                List<COMPTEUR> lstcompteur = new List<COMPTEUR>();
                foreach (var item_ in pDemande.LstCanalistion.Where(t=>t.CAS == Enumere.CasPoseCompteur ).ToList())
                {
                    CANALISATION leCanal = new CANALISATION();
                    MAGASINVIRTUEL item = pContext.MAGASINVIRTUEL.FirstOrDefault(m => m.PK_ID == item_.FK_IDMAGAZINVIRTUEL);
                    COMPTEUR can = new COMPTEUR();
                    can.ANNEEFAB = item.ANNEEFAB;
                    can.CADRAN = item.CADRAN;
                    can.COEFCOMPTAGE = item.COEFCOMPTAGE;
                    can.COEFLECT = item.COEFLECT;
                    can.DATECREATION = System.DateTime.Now;
                    can.DATEMODIFICATION = item.DATEMODIFICATION;
                    can.MISEENSERVICE = System.DateTime.Today.Date;

                    can.FK_IDCALIBRE = item.FK_IDCALIBRECOMPTEUR;
                    can.FK_IDMARQUECOMPTEUR = item.FK_IDMARQUECOMPTEUR;
                    can.FK_IDPRODUIT = item.FK_IDPRODUIT;
                    can.FK_IDTYPECOMPTEUR = item_.FK_IDTYPECOMPTEUR.Value;
                    can.FK_IDSTATUTCOMPTEUR = 1;
                    can.FK_IDETATCOMPTEUR = 1;
                    can.FONCTIONNEMENT = item.FONCTIONNEMENT;
                    can.MARQUE = item.MARQUE;

                    can.NUMERO = item_.NUMERO;
                    can.TYPECOMPTEUR = item_.TYPECOMPTEUR;

                    can.PLOMBAGE = item.PLOMBAGE;
                    can.PRODUIT = item_.PRODUIT;
                    can.USERCREATION = item.USERCREATION;
                    can.USERMODIFICATION = item.USERMODIFICATION;
                    lstcompteur.Add(can);
                }

                List<CANALISATION> _LeCompteur = new List<CANALISATION>();
                if (pDemande.LstCanalistion != null)
                {
                    _LeCompteur = Entities.ConvertObject<CANALISATION, CsCanalisation>(pDemande.LstCanalistion.Where(y=>y.CAS ==Enumere.CasPoseCompteur).ToList());
                    _LeCompteur.ForEach(t => t.PROPRIO = pDemande.LeClient.PROPRIO);
                    _LeCompteur.ForEach(t => t.FK_IDPROPRIETAIRE = pContext.PROPRIETAIRE.FirstOrDefault(p => p.CODE == pDemande.LeClient.PROPRIO).PK_ID);
                }

                List<EVENEMENT> _LstEvenement = new List<EVENEMENT>();
                if (pDemande.LstEvenement != null)
                    _LstEvenement = Entities.ConvertObject<EVENEMENT, CsEvenement>(pDemande.LstEvenement);
                _LstEvenement.ForEach(t => t.STATUS = 99);

                List<POSE_SCELLE_DEMANDE> organeScelleDemande = new List<POSE_SCELLE_DEMANDE>();
                if (pDemande.LaDemande != null)
                {
                    organeScelleDemande = pContext.POSE_SCELLE_DEMANDE.Where(t => t.FK_IDDEMANDE == pDemande.LaDemande.PK_ID).ToList();
                }
                if (organeScelleDemande != null && organeScelleDemande.Count != 0)
                {
                    organeScelleDemande.ForEach(t => t.FK_IDBRT = _leBrt.PK_ID);
                    organeScelleDemande.ForEach(t => t.USERCREATION = pDemande.LaDemande.USERCREATION);
                    organeScelleDemande.ForEach(t => t.DATECREATION = System.DateTime.Now);
                }

                _LeAbon.FK_IDCLIENT = IdClient;
                Entities.InsertEntity<ABON>(_LeAbon, pContext);
                Entities.InsertEntity<COMPTEUR>(lstcompteur, pContext);
                foreach (CsCanalisation item in pDemande.LstCanalistion.Where(o=>o.CAS == Enumere.CasPoseCompteur ).ToList())
                {
                    CANALISATION leCan = _LeCompteur.FirstOrDefault(t => t.POINT == item.POINT);
                    COMPTEUR leCompteurDuPoint = lstcompteur.FirstOrDefault(t => t.TYPECOMPTEUR == item.TYPECOMPTEUR);
                    leCan.FK_IDCOMPTEUR = leCompteurDuPoint.PK_ID;
                    leCan.FK_IDABON = _LeAbon.PK_ID;
                }
                Entities.InsertEntity<CANALISATION>(_LeCompteur, pContext);
                foreach (EVENEMENT item in _LstEvenement)
                {
                   
                }
                int nuEvt = 0;

                foreach (EVENEMENT item in _LstEvenement.OrderByDescending(t => t.CAS))
                {
                    if (nuEvt == 0)
                    {
                        List<EVENEMENT> lesVtc = _LeContextInter.EVENEMENT.Where(t => t.FK_IDCENTRE == item.FK_IDCENTRE &&
                                                                                      t.CENTRE == item.CENTRE &&
                                                                                      t.CLIENT == item.CLIENT &&
                                                                                      t.ORDRE == item.ORDRE &&
                                                                                      t.POINT == item.POINT).ToList();
                        if (lesVtc != null && lesVtc.Count != 0)
                            nuEvt = lesVtc.Max(t => t.NUMEVENEMENT);
                    }
                    nuEvt = nuEvt + 1;
                    item.NUMEVENEMENT = nuEvt;
                    item.FK_IDABON = _LeAbon.PK_ID;
                    item.FK_IDCANALISATION = _LeCompteur.FirstOrDefault(t => t.POINT == item.POINT).PK_ID;
                    item.FK_IDCOMPTEUR = lstcompteur.FirstOrDefault().PK_ID;
                }
                Entities.InsertEntity<EVENEMENT>(_LstEvenement, pContext);
                _LeContextInter.Dispose();

                return Resultat;

            }
            catch (Exception e)
            {
                throw e;
            }
        }

     
        //public static bool MiseAJourDeposeCompteur(CsDemande pDemande, galadbEntities pContext)
        //{
        //    try
        //    {
        //        bool Resultat = true;
        //        galadbEntities _LeContextInter = new galadbEntities();
        //        List<RUBRIQUEDEMANDE> _LstDemandeCout = new List<RUBRIQUEDEMANDE>();
        //        DEMANDE _Demande = new DEMANDE();
        //        if (pDemande.LaDemande != null )
        //            _Demande = Entities.ConvertObject<DEMANDE, CsDemandeBase>(pDemande.LaDemande);

        //        _Demande.FK_IDADMUTILISATEUR = _LeContextInter.ADMUTILISATEUR.FirstOrDefault(p => p.MATRICULE == _Demande.MATRICULE ).PK_ID;
        //        _Demande.FK_IDTYPEDEMANDE = _LeContextInter.TYPEDEMANDE.FirstOrDefault(p => p.CODE == _Demande.TYPEDEMANDE).PK_ID;
        //        int IdClient = _LeContextInter.CLIENT.FirstOrDefault(p => p.CENTRE == _Demande.CENTRE && p.REFCLIENT == _Demande.CLIENT && p.ORDRE == _Demande.ORDRE && p.FK_IDCENTRE == _Demande.FK_IDCLIENT).PK_ID;

        //        List<EVENEMENT> _LstEvenement = new List<EVENEMENT>();
        //        if (pDemande.LstEvenement != null)
        //        {
        //            _LstEvenement = Entities.ConvertObject<EVENEMENT, CsEvenement>(pDemande.LstEvenement);
        //            foreach (EVENEMENT item in _LstEvenement)
        //            {
        //                //item.FK_IDCAS = _LeContextInter.CASIND.FirstOrDefault(p => p.CODE == item.CAS).PK_ID;
                        
        //                //item.FK_IDCANALISATION = _LeContextInter.CANALISATION .FirstOrDefault(p => p.CENTRE == item.CENTRE && p.CLIENT  == item.CLIENT  &&  p.PRODUIT == item.PRODUIT).PK_ID;
        //            }
        //        }
        //        List<CANALISATION> _LeCompteur = new List<CANALISATION>();
        //        if (pDemande.LstCanalistion != null )
        //        {
        //            _LeCompteur = Entities.ConvertObject<CANALISATION, CsCanalisation>(pDemande.LstCanalistion);
        //            foreach (CANALISATION item in _LeCompteur)
        //            {
        //                //item.PK_ID = _LeContextInter.CANALISATION.FirstOrDefault(p => p.CENTRE == item.CENTRE && p.CLIENT == item.CLIENT && p.PRODUIT == item.PRODUIT).PK_ID;
        //                ////item.FK_IDBRT  = _LeContextInter.BRT .FirstOrDefault(p => p.CENTRE == item.CENTRE && p.CLIENT == item.CLIENT && p.PRODUIT == item.PRODUIT).PK_ID;
        //                //if (item.PRODUIT  != Enumere.ElectriciteMT)
        //                //    item.COMPTEUR.FK_IDDIAMETRECOMPTEUR = _LeContextInter.DIAMETRECOMPTEUR.FirstOrDefault(p => p.CODE == item.COMPTEUR.DIAMETRECOMPTEUR.CODE && p.FK_IDPRODUIT == item.FK_IDPRODUIT).PK_ID;
        //                //else
        //                //{
        //                //    //string CodeTypeComptage = item.COMPTEUR.TYPECOMPTAGE.PadLeft(4, '0');
        //                //    //item.COMPTEUR.FK_IDTYPECOMPTAGE = _LeContextInter.TYPEBRANCHEMENT.FirstOrDefault(p => p.CODE == CodeTypeComptage ).PK_ID;
        //                //}
        //                //item.COMPTEUR.FK_IDMARQUECOMPTEUR  = _LeContextInter.MARQUECOMPTEUR.FirstOrDefault(p => p.CODE == item.COMPTEUR.MARQUECOMPTEUR.CODE).PK_ID;
        //                //item.COMPTEUR.FK_IDTYPECOMPTEUR  = _LeContextInter.TYPECOMPTEUR .FirstOrDefault(p => p.CODE == item.COMPTEUR.TYPECOMPTEUR1 .CODE && p.FK_IDPRODUIT == item.PRODUIT1.PK_ID).PK_ID;
        //            }
        //        }
        //        List<LCLIENT> _LstFacture = new List<LCLIENT>();

        //        List<RUBRIQUEDEMANDE> _LstCout = new List<RUBRIQUEDEMANDE>();
        //        if (pDemande.LstCoutDemande != null)
        //        {
        //            _LstCout = Entities.ConvertObject<RUBRIQUEDEMANDE, CsDemandeDetailCout>(pDemande.LstCoutDemande);
        //            foreach (RUBRIQUEDEMANDE item in _LstCout)
        //            {
        //                item.FK_IDTAXE = _LeContextInter.TAXE.FirstOrDefault(p => p.CODE == item.TAXE).PK_ID;
        //                item.FK_IDCOPER  = _LeContextInter.COPER .FirstOrDefault(p => p.CODE  == item.COPER ).PK_ID;

        //                if (string.IsNullOrEmpty(item.NDOC))
        //                    item.NDOC = NumeroFacture(item.FK_IDCENTRE );
                        
        //            }
        //        }

        //        if (_LstCout != null && _LstCout.Count != 0)
        //            _LstFacture = GetLclientFromCoutDemande(_LstCout);

        //        if (pDemande.LstCoutDemande!= null && pDemande.LstCoutDemande.Count != 0)
        //        {
        //            List<TRANSCAISSE> _lesReglementtranscaisse = _LeContextInter.TRANSCAISSE.Where(p => p.CENTRE == pDemande.LaDemande.CENTRE &&
        //                     p.CLIENT == pDemande.LaDemande.CLIENT  &&
        //                     p.ORDRE == pDemande.LaDemande.ORDRE).ToList();

        //            if (_lesReglementtranscaisse != null && _lesReglementtranscaisse.Count != 0)
        //                _LstFacture.AddRange(GetLclientFromTranscaisse(_lesReglementtranscaisse, _LstCout, 0, pDemande.LaDemande.FK_IDCENTRE));
        //            else
        //            {

        //                List<TRANSCAISB> _lesReglementTranscaisB = _LeContextInter.TRANSCAISB.Where(p => p.CENTRE == pDemande.LaDemande.CENTRE &&
        //                  p.CLIENT == pDemande.LaDemande.CLIENT &&
        //                  p.ORDRE == pDemande.LaDemande.ORDRE).ToList();
        //                if (_lesReglementTranscaisB != null && _lesReglementTranscaisB.Count != 0)
        //                    _LstFacture.AddRange(GetLclientFromTranscaisB(_lesReglementTranscaisB, _LstCout, 0, pDemande.LaDemande.FK_IDCENTRE));
        //            }
        //        }
        //        _LstFacture.ForEach(t => t.FK_IDCLIENT = IdClient); 
        //        _LeContextInter.Dispose();
        //        if (_LeCompteur != null  && _LeCompteur.Count != 0)
        //        Resultat = Entities.UpdateEntity<CANALISATION>(_LeCompteur, pContext);

        //        if (_LstEvenement != null && _LstEvenement.Count != 0)
        //         Resultat = Entities.InsertEntity<EVENEMENT>(_LstEvenement, pContext);

        //        if (_LstFacture != null && _LstFacture.Count != 0)
        //        Resultat = Entities.InsertEntity<LCLIENT >(_LstFacture, pContext);

        //        return Resultat;

        //    }
        //    catch (Exception e)
        //    {
        //        throw e;
        //    }
        //}
        public static bool MiseAJourRepriseIndex(CsDemande pDemande, galadbEntities pContext)
        {
            try
            {
                galadbEntities _LeContextInter = new galadbEntities();
                //int leIdCentre = _LeContextInter.CENTRE.FirstOrDefault(c => c.CODECENTRE == pDemande.LaDemande.CENTRE).PK_ID;
                //int? leIdProduit = pDemande.LaDemande.FK_IDPRODUIT;
                bool Resultat = false;
                List<EVENEMENT> _LstEvenements = new List<EVENEMENT>();
                if (pDemande.LstEvenement != null)
                {
                    _LstEvenements = Entities.ConvertObject<EVENEMENT, CsEvenement>(pDemande.LstEvenement);
                    //foreach (EVENEMENT item in _LstEvenements)
                    //{
                    //    //item.FK_IDCAS = _LeContextInter.CASIND.FirstOrDefault(p => p.CODE == item.CAS).PK_ID;
                    //    item.FK_IDCENTRE = leIdCentre;
                    //    item.FK_IDPRODUIT = leIdProduit.Value ;
                    //}
                }
                _LeContextInter.Dispose();
                Resultat = Entities.UpdateEntity<EVENEMENT>(_LstEvenements, pContext);
                return Resultat;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static bool MiseAJourRegularisationAvance(CsDemande pDemande, galadbEntities pContext)
        {
            try
            {
                galadbEntities _LeContextInter = new galadbEntities();
                int leIdCentre = _LeContextInter.CENTRE.FirstOrDefault(c => c.CODE == pDemande.LaDemande.CENTRE).PK_ID;
                int? leIdProduit = pDemande.LaDemande.FK_IDPRODUIT;
                bool Resultat = false;
                ABON _LeAbon = new ABON();
                if (pDemande.Abonne != null)
                {
                    _LeAbon = Entities.ConvertObject<ABON, CsAbon>(pDemande.Abonne);
                    _LeAbon.FK_IDPRODUIT = leIdProduit.Value ;
                    _LeAbon.FK_IDCENTRE = leIdCentre;
                    _LeAbon.FK_IDTYPETARIF = _LeContextInter.TYPETARIF.FirstOrDefault(p => p.CODE == pDemande.Abonne.TYPETARIF) != null ? _LeContextInter.TYPETARIF.FirstOrDefault(p => p.CODE == pDemande.Abonne.TYPETARIF).PK_ID : _LeAbon.FK_IDTYPETARIF;
                    _LeAbon.FK_IDMOISFAC = _LeContextInter.MOIS.FirstOrDefault(p => p.CODE == pDemande.Abonne.MOISFAC) != null ? _LeContextInter.MOIS.FirstOrDefault(p => p.CODE == pDemande.Abonne.MOISFAC).PK_ID : _LeAbon.FK_IDMOISFAC;
                    _LeAbon.FK_IDMOISREL = _LeContextInter.MOIS.FirstOrDefault(p => p.CODE == pDemande.Abonne.MOISREL) != null ? _LeContextInter.MOIS.FirstOrDefault(p => p.CODE == pDemande.Abonne.MOISREL).PK_ID : _LeAbon.FK_IDMOISREL;
                    _LeAbon.FK_IDFORFAIT = _LeContextInter.FORFAIT.FirstOrDefault(p => p.CODE == pDemande.Abonne.FORFAIT) != null ? _LeContextInter.FORFAIT.FirstOrDefault(p => p.CODE == pDemande.Abonne.FORFAIT).PK_ID : _LeAbon.FK_IDFORFAIT;
                }

                LCLIENT _leClient = new LCLIENT();
               
                    _leClient .CENTRE = pDemande.Abonne.CENTRE;
                    _leClient .CLIENT = pDemande.Abonne.CLIENT;
                    _leClient .ORDRE = pDemande.Abonne.ORDRE;
                    _leClient .COPER = Enumere.CoperCAU ;
                    _leClient .REFEM = pDemande.Abonne.DATECREATION.Year  + pDemande.Abonne.DATECREATION.Month.ToString("00");
                    _leClient .TOP1 = Enumere.TopGuichet;
                    _leClient .MATRICULE = pDemande.Abonne.USERCREATION;
                    _leClient .DENR =pDemande.Abonne.DATECREATION;
                    _leClient .DATEMODIFICATION = pDemande.Abonne.DATEMODIFICATION;
                    _leClient .MOISCOMPT = pDemande.Abonne.DATECREATION.Year + pDemande.Abonne.DATECREATION.Month.ToString("00");
                    _leClient .DATECREATION = pDemande.Abonne.DATECREATION;
                    _leClient .MONTANT = pDemande.Abonne.AVANCE  ;
                    _leClient .NDOC = NumeroFacture(pDemande.Abonne.FK_IDCENTRE );
                    _leClient .DC = Enumere.Debit;
                    _leClient .FK_IDADMUTILISATEUR = _LeContextInter.ADMUTILISATEUR.FirstOrDefault(p => p.MATRICULE == pDemande.Abonne.USERMODIFICATION ).PK_ID;
                    _leClient .FK_IDCOPER = _LeContextInter.COPER.FirstOrDefault(p => p.CODE == Enumere.CoperCAU ).PK_ID;
                    _leClient .FK_IDLIBELLETOP = _LeContextInter.LIBELLETOP.FirstOrDefault(p => p.CODE == Enumere.TopGuichet).PK_ID;
                    _leClient .FK_IDCENTRE = leIdCentre;
                    _leClient.FK_IDCLIENT = pDemande.Abonne.FK_IDCLIENT;
                    _leClient.MATRICULE  = pDemande.Abonne.USERMODIFICATION ;
                

                //LCLIENT _leClient = new LCLIENT()
                //{
                //    CENTRE = pDemande.Abonne.CENTRE,
                //    CLIENT = pDemande.Abonne.CLIENT,
                //    ORDRE = pDemande.Abonne.ORDRE,
                //    COPER = Enumere.CoperCAU ,
                //    REFEM = pDemande.Abonne.DATECREATION.Year  + pDemande.Abonne.DATECREATION.Month.ToString("00"),
                //    TOP1 = Enumere.TopGuichet,
                //    MATRICULE = pDemande.Abonne.USERCREATION,
                //    DENR =pDemande.Abonne.DATECREATION,
                //    DATEMODIFICATION = pDemande.Abonne.DATEMODIFICATION,
                //    MOISCOMPT = pDemande.Abonne.DATECREATION.Year + pDemande.Abonne.DATECREATION.Month.ToString("00"),
                //    NATURE = Enumere.NatureCAU ,
                //    DATECREATION = pDemande.Abonne.DATECREATION,
                //    MONTANT = pDemande.Abonne.AVANCE  ,
                //    //MONTANTTVA = item.MONTANTTAXE,
                //    NDOC = NumeroFacture(pDemande.Abonne.CENTRE),
                //    DC = Enumere.Debit,
                //    FK_IDADMUTILISATEUR = _LeContextInter.ADMUTILISATEUR.FirstOrDefault(p => p.MATRICULE == pDemande.Abonne.USERCREATION).PK_ID,
                //    FK_IDCOPER = _LeContextInter.COPER.FirstOrDefault(p => p.CODE == Enumere.CoperCAU ).PK_ID,
                //    FK_IDLIBELLETOP = _LeContextInter.LIBELLETOP.FirstOrDefault(p => p.CODE == Enumere.TopGuichet).PK_ID,
                //    FK_IDNATURE = _LeContextInter.NATURE.FirstOrDefault(p => p.CODE == Enumere.NatureCAU ).PK_ID,
                //    FK_IDCENTRE = leIdCentre,
                //    FK_IDCLIENT = pDemande.Abonne.FK_IDCLIENT ,
                      //MATRICULE = pDemande.Abonne.USERMODIFICATION

                //};

                _LeContextInter.Dispose();
                Resultat = Entities.UpdateEntity<ABON>(_LeAbon, pContext);
                Resultat = Entities.InsertEntity<LCLIENT>(_leClient, pContext);
                return Resultat;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static DataTable RetourneCompteClientTransfert(string pCentre, string pClient, string pOrdre, string Orientation)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = null;
                    var _LCLIENT = context.LCLIENT;
                    query =
                    from _LeLCLIENT in _LCLIENT
                    where (_LeLCLIENT.CENTRE == pCentre && _LeLCLIENT.CLIENT == pClient &&
                          _LeLCLIENT.ORDRE == pOrdre  && _LeLCLIENT.DC == Orientation)
                    select _LeLCLIENT;
                    return Galatee.Tools.Utility.ListToDataTable(query);
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static bool TranfertCompteClient(CsLclient FactureAregle, CsLclient Client2, string Orientation)
        {

            try
            {
                galadbEntities context = new galadbEntities();

                string ValOrientation = string.Empty;
                if (FactureAregle.DC == "D")
                    ValOrientation = Enumere.CoperTransfertDebit;
                else
                    ValOrientation = Enumere.CoperTransfertDette;

                CsLclient Lclient = new CsLclient()
                {

                    PK_ID = Client2.PK_ID,
                    CENTRE = Client2.CENTRE,
                    CLIENT = Client2.CLIENT,
                    ORDRE = Client2.ORDRE,
                    REFEM = FactureAregle.REFEM,
                    NDOC = FactureAregle.NDOC,
                    NATURE = FactureAregle.NATURE,
                    COPER = ValOrientation,
                    DENR = FactureAregle.DENR,//
                    EXIG = FactureAregle.EXIG,
                    MONTANT = FactureAregle.MONTANT,
                    CAPUR = FactureAregle.CAPUR,
                    CRET = FactureAregle.CRET,
                    MODEREG = FactureAregle.MODEREG,
                    DC = FactureAregle.DC,
                    ORIGINE = FactureAregle.ORIGINE,
                    CAISSE = FactureAregle.CAISSE,
                    ECART = FactureAregle.ECART,//
                    MOISCOMPT = FactureAregle.MOISCOMPT,
                    TOP1 = FactureAregle.TOP1,
                    EXIGIBILITE = FactureAregle.EXIGIBILITE,
                    FRAISDERETARD = FactureAregle.FRAISDERETARD,
                    REFERENCEPUPITRE = FactureAregle.REFERENCEPUPITRE,
                    IDLOT = FactureAregle.IDLOT,//
                    DATEVALEUR = FactureAregle.DATEVALEUR,
                    REFERENCE = FactureAregle.REFERENCE,
                    REFEMNDOC = FactureAregle.REFEMNDOC,
                    ACQUIT = FactureAregle.ACQUIT,
                    MATRICULE = FactureAregle.MATRICULE,
                    TAXESADEDUIRE = FactureAregle.TAXESADEDUIRE,//
                    DATEFLAG = FactureAregle.DATEFLAG,//
                    MONTANTTVA = FactureAregle.MONTANTTVA,
                    IDCOUPURE = FactureAregle.IDCOUPURE,
                    AGENT_COUPURE = FactureAregle.AGENT_COUPURE,
                    RDV_COUPURE = FactureAregle.RDV_COUPURE,//
                    NUMCHEQ = FactureAregle.NUMCHEQ,
                    OBSERVATION_COUPURE = FactureAregle.OBSERVATION_COUPURE,
                    USERCREATION = FactureAregle.USERCREATION,
                    DATECREATION = DateTime.Now,
                    DATEMODIFICATION = null,
                    USERMODIFICATION = FactureAregle.USERMODIFICATION,
                    BANQUE = FactureAregle.BANQUE,
                    GUICHET = FactureAregle.GUICHET,
                    FK_IDCENTRE = Client2.FK_IDCENTRE,
                    FK_IDNATURE = FactureAregle.FK_IDNATURE,
                    FK_IDADMUTILISATEUR = FactureAregle.FK_IDADMUTILISATEUR,
                    FK_IDCOPER = context.COPER.FirstOrDefault(t => t.CODE == ValOrientation).PK_ID,
                    FK_IDLIBELLETOP = FactureAregle.FK_IDLIBELLETOP,
                    FK_IDCLIENT = Client2.FK_IDCLIENT,
                    //FK_IDPOSTE = FactureAregle.FK_IDPOSTE,
                    //POSTE = FactureAregle.POSTE


                };

                // valorisation des propriétés foreign key
               

                LCLIENT _LClient = Entities.ConvertObject<LCLIENT, CsLclient>(Lclient);
                
                bool resultat =Entities.InsertEntity<LCLIENT>(_LClient);
                    context.Dispose();
                    return resultat;
            }
            catch (Exception ex)
            {
                return false;
                throw ex;
            }
        }
        public static bool DemandeTranfertCompteClient(CsDemande lademande, CsLclient Client2, string Orientation)
        {

            try
            {
                galadbEntities context = new galadbEntities();

                //string ValOrientation = string.Empty;
                //if (FactureAregle.DC == "D")
                //    ValOrientation = Enumere.CoperTransfertDebit;
                //else
                //    ValOrientation = Enumere.CoperTransfertDette;

                //CsDemande Lclient = new CsDemande()
                //{

                //    NUMDEM = Client2.PK_IDLCLIENT,
                //    CENTRE = Client2.CENTRE,
                //    TYPEDEMANDE = Client2.CLIENT,
                //    ORDRE = Client2.ORDRE,
                //    STATUT = FactureAregle.REFEM,
                //    ANNOTATION = FactureAregle.REFEM, 
                //    PRODUIT = FactureAregle.NDOC,
                //    USERCREATION = FactureAregle.NATURE,
                //    DATECREATION = ValOrientation,
                //    PK_ID = FactureAregle.DENR,//
                //    FK_IDCENTRE = FactureAregle.EXIG,
                //    FK_IDADMUTILISATEUR = FactureAregle.MONTANT,
                //    FK_IDTDEM = FactureAregle.CAPUR,
                //    FK_IDPRODUIT = FactureAregle.CRET,
                //    MODEREG = FactureAregle.MODEREG,
                //    DC = FactureAregle.DC,
                //    ORIGINE = FactureAregle.ORIGINE


                //};

                //// valorisation des propriétés foreign key


                //LCLIENT _LClient = Entities.ConvertObject<LCLIENT, CsLclient>(Lclient);

                //bool resultat = Entities.InsertEntity<LCLIENT>(_LClient);
                //context.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                return false;
                throw ex;
            }
        }
        public static bool DetailDemandeTranfertCompteClient(CsLclient FactureAregle, CsLclient Client2, string Orientation)
        {

            try
            {
                galadbEntities context = new galadbEntities();

                //string ValOrientation = string.Empty;
                //if (FactureAregle.DC == "D")
                //    ValOrientation = Enumere.CoperTransfertDebit;
                //else
                //    ValOrientation = Enumere.CoperTransfertDette;

                //CsDemandeDetailCout LcDemande = new CsDemandeDetailCout()
                //{

                //    NUMDEM = Client2.NUMDEM,
                //    CENTRE = Client2.CENTRE,
                //    CLIENT = Client2.CLIENT,
                //    ORDRE = Client2.ORDRE,
                //    TYPEDEMANDE = FactureAregle.REFEM,
                //    STATUT = FactureAregle.NDOC,
                //    REFEM = FactureAregle.NATURE,
                //    USERCREATION = ValOrientation,
                //    DATECREATION = FactureAregle.DENR,//
                //    PK_ID = FactureAregle.EXIG,
                //    FK_IDCENTRE = FactureAregle.MONTANT,
                //    COPER = FactureAregle.MONTANT,
                //    FK_IDDEMANDE = FactureAregle.DC,
                //    MONTANTHT = FactureAregle.DC,
                //    MONTANTTAXE = FactureAregle.DC

                //};

                //// valorisation des propriétés foreign key


                //RUBRIQUEDEMANDE _LcDemande = Entities.ConvertObject<RUBRIQUEDEMANDE, CsDemandeDetailCout>(LcDemande);

                //bool resultat = Entities.InsertEntity<RUBRIQUEDEMANDE>(_LcDemande);
                //context.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                return false;
                throw ex;
            }
        }
        public static List<PAYEUR>  RetourneLesPayeur(galadbEntities context)
        {
            try
            {
                return context.PAYEUR.ToList();
                  
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static bool MajPayeur(CsLePayeur lePayeur, int Action)
        {
            try
            {
                galadbEntities pContext = new galadbEntities();
                PAYEUR _LePayeur = Entities.ConvertObject<PAYEUR, CsPayeur>(lePayeur.Payeur);
                List<CLIENT> _LesClient= Entities.ConvertObject<CLIENT , CsClient >(lePayeur.ClientPayeur.ToList());
                bool Resultat = false;
                if (Action == 1)
                {
                    if (_LePayeur != null)
                        Resultat = Entities.InsertEntity<PAYEUR>(_LePayeur, pContext);
                    if (_LesClient != null)
                        Resultat = Entities.UpdateEntity<CLIENT>(_LesClient, pContext);
                }
                if (Action == 2)
                {
                    if (_LePayeur != null)
                        Resultat = Entities.UpdateEntity <PAYEUR>(_LePayeur, pContext);
                    if (_LesClient != null)
                        Resultat = Entities.UpdateEntity<CLIENT>(_LesClient, pContext);
                }
                if (Action == 3)
                {
                    if (_LePayeur != null)
                        Resultat = Entities.DeleteEntity<PAYEUR>(_LePayeur, pContext);
                    if (_LesClient != null)
                        Resultat = Entities.UpdateEntity<CLIENT>(_LesClient, pContext);
                }
                return Resultat;  

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static DataTable RetourneEvenementDernierEvenement(string centre, string client, string ordre, string produit, int FK_POINT)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    List<EVENEMENT> _lstEvt = new List<EVENEMENT>();
                    List<EVENEMENT> _lstEvtInit = context.EVENEMENT.Where(e => e.CENTRE == centre && e.CLIENT == client && e.ORDRE == ordre && e.POINT == FK_POINT && e.PRODUIT == produit).ToList();
                    if (_lstEvtInit != null && _lstEvtInit.Count != 0)
                    {
                        int max = context.EVENEMENT.Where(p => p.CENTRE == centre && p.CLIENT == client && p.ORDRE == ordre && p.POINT == FK_POINT && p.PRODUIT == produit).Max(p => p.NUMEVENEMENT);
                        EVENEMENT _evenement = context.EVENEMENT.Where(p => p.CENTRE == centre && p.CLIENT == client && p.ORDRE == ordre && p.POINT == FK_POINT && p.PRODUIT == produit && p.NUMEVENEMENT == max ).First();

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
                                                        x.REGLAGECOMPTEUR ,
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
        public static DataTable RetourneFactureCampagne(string IdCampagen, string pCentre, string pClient, string pOrdre)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = null;
                    var _LCLIENT = context.LCLIENT;
                    var _LaCampagne = context.CAMPAGNE;
                    var _LeDetailCampagne = context.DETAILCAMPAGNE ;

                    query =
                    (from y in _LeDetailCampagne
                    join z in _LCLIENT on new { y.CENTRE, y.CLIENT, y.ORDRE, y.IDCOUPURE } equals new { z.CENTRE, z.CLIENT, z.ORDRE, z.IDCOUPURE }
                    where (y.CENTRE == pCentre && y.CLIENT == pClient &&
                           y.ORDRE == pOrdre && z.IDCOUPURE == IdCampagen)
                    select new
                    {
                                       y.IDCOUPURE, 
                                       y.CENTRE ,
                                       y.CLIENT ,
                                       y.ORDRE ,
                                       y.TOURNEE ,
                                       y.ORDTOUR ,
                                       y.CATEGORIECLIENT ,
                                       y.SOLDEDUE ,
                                       y.NOMBREFACTURE,
                                       y.PK_ID ,
                                       y.DATECREATION ,
                                       y.DATEMODIFICATION ,
                                       y.USERCREATION ,
                                       y.USERMODIFICATION ,
                                       y.FK_IDCENTRE ,
                                       y.FK_IDTOURNEE ,
                                       y.FK_IDCATEGORIECLIENT ,
                                       y.FK_IDCAMPAGNE ,
                                       y.FK_IDCLIENT   
                    }).Distinct();
                    return Galatee.Tools.Utility.ListToDataTable(query);
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static DataTable RetourneTypeCoupure()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = null;
                    var _LCoupure = context.TYPECOUPURE ;
                    query =
                    from x in _LCoupure
                     select new
                     {
                        x.CODE ,
                        x.COPER ,
                        x.COUT ,
                        x.LIBELLE ,
                        x.PK_ID 
                     };
                    return Galatee.Tools.Utility.ListToDataTable(query);
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #region Cli300
        public static DataTable RetourneCodeControle()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = null;
                    var _CODECONTROLE = context.CODECONTROLE;

                    query =
                    from _LeCODECONTROLE in _CODECONTROLE
                    select new
                    {
                        _LeCODECONTROLE.PK_ID,
                        _LeCODECONTROLE.LIBELLE,
                        _LeCODECONTROLE.CODE
                    };
                    return Galatee.Tools.Utility.ListToDataTable(query);
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static DataTable RetourneTypeLot()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = null;
                    var _TYPELOT = context.TYPELOT;
                    query =
                    from _LeTYPELOTE in _TYPELOT
                    select new
                    {
                        _LeTYPELOTE.PK_ID,
                        _LeTYPELOTE.CODECONTROLE,
                        _LeTYPELOTE.CODETRAITEMENT,
                         LIBELLE = _LeTYPELOTE.LIBELLE ,
                        _LeTYPELOTE.REFERENCE,
                        _LeTYPELOTE.CODE,
                        DC = _LeTYPELOTE.CODECONTROLE1.COPER.DC ,
                        FK_IDCOPER = _LeTYPELOTE.CODECONTROLE1.COPER.PK_ID,
                        COPER = _LeTYPELOTE.CODECONTROLE1.COPER.CODE 
                    };
                    return Galatee.Tools.Utility.ListToDataTable(query);
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static DataTable RetourneOrigine()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = null;
                    var _ORIGINELOT = context.ORIGINELOT;

                    query =
                    from _LeORIGINELOT in _ORIGINELOT
                    select new
                    {
                        _LeORIGINELOT.SITE,
                        _LeORIGINELOT.LIBELLE,
                        _LeORIGINELOT.ORIGINE,
                        _LeORIGINELOT.PK_ID
                    };
                    return Galatee.Tools.Utility.ListToDataTable(query);
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static int? RetourneMaxIDlot()
        {
            try
            {
                int? LotMaxe = null;
                using (galadbEntities ctontext = new galadbEntities())
                {
                    LotMaxe = ctontext.LOTCOMPTECLIENT.Max(p => p.IDLOT);
                    if (LotMaxe == null) LotMaxe = 1;
                }
                return LotMaxe;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static DataTable RetourneListeDesDetailLot(int pIdLot)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = null;
                    var _DETAILLOT = context.DETAILLOT;

                    query =
                    from _LeDETAILLOT in _DETAILLOT
                    where _LeDETAILLOT.IDLOT == pIdLot
                    select new
                    {
                        _LeDETAILLOT.IDLOT,
                        _LeDETAILLOT.NUMEROLIGNE,
                        _LeDETAILLOT.CENTRE,
                        _LeDETAILLOT.CLIENT,
                        _LeDETAILLOT.ORDRE,
                        _LeDETAILLOT.REFEM,
                        _LeDETAILLOT.NDOC,
                        _LeDETAILLOT.MONTANT,
                        _LeDETAILLOT.COPER,
                        _LeDETAILLOT.MODEREG,
                        _LeDETAILLOT.LOTCOMPTECLIENT.TYPELOT,
                        _LeDETAILLOT.LOTCOMPTECLIENT.NUMEROLOT ,
                        _LeDETAILLOT.ACQUIT,
                        _LeDETAILLOT.DATEPIECE,
                        _LeDETAILLOT.DATESAISIE,
                        _LeDETAILLOT.EXIGIBILITE,
                        _LeDETAILLOT.ECART,
                        _LeDETAILLOT.CODEERR,
                        _LeDETAILLOT.SENS,
                        _LeDETAILLOT.REFERENCE,
                        _LeDETAILLOT.MATRICULE,
                        _LeDETAILLOT.DATETRAIT,
                        _LeDETAILLOT.REFEMNDOC,
                        _LeDETAILLOT.PK_ID,
                        _LeDETAILLOT.FK_IDLOTCOMPECLIENT,
                        _LeDETAILLOT.DATECREATION,
                        _LeDETAILLOT.DATEMODIFICATION,
                        _LeDETAILLOT.USERCREATION,
                        _LeDETAILLOT.USERMODIFICATION,
                        _LeDETAILLOT.FK_IDCENTRE,
                        //_LeDETAILLOT.FK_IDLOTCOMPECLIENT,
                        _LeDETAILLOT.FK_IDCOPER,
                        _LeDETAILLOT.FK_IDMATRICULE,
                        _LeDETAILLOT.FK_IDCLIENT ,
                        _LeDETAILLOT.FK_IDLCLIENT 
                    };
                    return Galatee.Tools.Utility.ListToDataTable(query);
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static DataTable RetourneListeDesTypeLot(string pOrigine, string pTypeLot)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {

                    string Origine = string.IsNullOrEmpty(pOrigine) ? "" : pOrigine;
                    string TypeLot = string.IsNullOrEmpty(pTypeLot) ? "" : pTypeLot;
                    string vide = "";

                    IEnumerable<object> query = null;
                    var _LOTCOMPTECLIENT = context.LOTCOMPTECLIENT;

                    query =
                    from _LeLOTCOMPTECLIENT in _LOTCOMPTECLIENT
                    where
                      (_LeLOTCOMPTECLIENT.ORIGINE  == pOrigine || Origine.Equals(vide)) &&
                      (_LeLOTCOMPTECLIENT.TYPELOT   == pTypeLot || TypeLot.Equals(vide))
                    select new
                    {
                        _LeLOTCOMPTECLIENT.ORIGINE,
                        _LeLOTCOMPTECLIENT.NUMEROLOT,
                        _LeLOTCOMPTECLIENT.MOISCOMPTABLE,
                        _LeLOTCOMPTECLIENT.TYPELOT,
                        _LeLOTCOMPTECLIENT.PARAMETRE,
                        _LeLOTCOMPTECLIENT.IDLOT,
                        _LeLOTCOMPTECLIENT.CAISSE,
                        _LeLOTCOMPTECLIENT.DATEDECREATION,
                        _LeLOTCOMPTECLIENT.STATUS,
                        _LeLOTCOMPTECLIENT.MONTANT,
                        _LeLOTCOMPTECLIENT.REFERENCE,
                        _LeLOTCOMPTECLIENT.PK_ID,
                        _LeLOTCOMPTECLIENT.DATECREATION,
                        _LeLOTCOMPTECLIENT.DATEMODIFICATION,
                        _LeLOTCOMPTECLIENT.USERCREATION,
                        _LeLOTCOMPTECLIENT.USERMODIFICATION,
                        _LeLOTCOMPTECLIENT.FK_IDLOT ,

                    };
                    return Galatee.Tools.Utility.ListToDataTable(query);
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static bool MiseAJourLot(CsLotCompteClient _LeLot, galadbEntities pContext)
        {
            try
            {
                bool Resultat = true;
                LOTCOMPTECLIENT _LotAMettreAJour = new LOTCOMPTECLIENT();
                if (_LeLot != null)
                    _LotAMettreAJour = Entities.ConvertObject<LOTCOMPTECLIENT, CsLotCompteClient>(_LeLot);

                LOTCOMPTECLIENT _objectLot = new LOTCOMPTECLIENT();
                using (galadbEntities ctontext = new galadbEntities())
                {
                    if (_LotAMettreAJour != null)
                    {
                        _objectLot = ctontext.LOTCOMPTECLIENT.FirstOrDefault(p => p.NUMEROLOT == _LotAMettreAJour.NUMEROLOT);
                        if (_objectLot != null )
                            Entities.UpdateEntity<LOTCOMPTECLIENT>(_LotAMettreAJour);
                        else
                            Entities.InsertEntity<LOTCOMPTECLIENT>(_LotAMettreAJour);
                    }
                };
                return Resultat;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public static bool MiseAJourDetailLot(CsSaisieDeMasse leLot, galadbEntities pContext)
        {
            try
            {
                bool Resultat = true;
                using (galadbEntities ctontext = new galadbEntities())
                {
                    List<CsDetailLot> _LeDetailLot = new List<CsDetailLot>();
                    _LeDetailLot= leLot.LstDetailLot ;
                    int FkidLot = 0;
                    int idlot = _LeDetailLot[0].IDLOT;
                    string typeLot = _LeDetailLot[0].TYPELOT;
                    LOTCOMPTECLIENT _leLot = ctontext.LOTCOMPTECLIENT.FirstOrDefault(p => p.NUMEROLOT == leLot.LotCompteClient.NUMEROLOT );
                    if (_leLot != null) FkidLot = _leLot.PK_ID;
                    foreach (CsDetailLot item in _LeDetailLot)
                    {
                        item.FK_IDLOTCOMPECLIENT = FkidLot;
                        DETAILLOT _objectDetailLot = Entities.ConvertObject<DETAILLOT, CsDetailLot>(item);
                        if (item.IsACTION == Enumere.IsInsertion)
                            Entities.InsertEntity<DETAILLOT>(_objectDetailLot);
                        if (item.IsACTION == Enumere.IsUpdate)
                            Entities.UpdateEntity<DETAILLOT>(_objectDetailLot);
                        if (item.IsACTION == Enumere.IsDelete)
                            Entities.DeleteEntity<DETAILLOT>(_objectDetailLot);
                    }
                };
                return Resultat;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public static DataTable RetourneListeDesCompteAdjusteLot(int pIdLot)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = null;
                    var _LCLIENT = context.LCLIENT;

                    query =
                    from _LeLCLIENT in _LCLIENT
                    where _LeLCLIENT.IDLOT  == pIdLot
                    select new
                    {
                        _LeLCLIENT.CENTRE,
                        _LeLCLIENT.CLIENT,
                        _LeLCLIENT.ORDRE,
                        _LeLCLIENT.REFEM,
                        _LeLCLIENT.NDOC,
                        _LeLCLIENT.COPER,
                        _LeLCLIENT.DENR,
                        _LeLCLIENT.EXIG,
                        _LeLCLIENT.MONTANT,
                        _LeLCLIENT.CAPUR,
                        _LeLCLIENT.CRET,
                        _LeLCLIENT.MODEREG,
                        _LeLCLIENT.DC,
                        _LeLCLIENT.ORIGINE,
                        _LeLCLIENT.CAISSE,
                        _LeLCLIENT.ECART,
                        _LeLCLIENT.MOISCOMPT,
                        _LeLCLIENT.TOP1,
                        _LeLCLIENT.EXIGIBILITE,
                        _LeLCLIENT.FRAISDERETARD,
                        _LeLCLIENT.REFERENCEPUPITRE,
                        _LeLCLIENT.IDLOT,
                        _LeLCLIENT.DATEVALEUR,
                        _LeLCLIENT.REFERENCE,
                        _LeLCLIENT.REFEMNDOC,
                        _LeLCLIENT.ACQUIT,
                        _LeLCLIENT.MATRICULE,
                        _LeLCLIENT.ADMUTILISATEUR.LIBELLE,
                        _LeLCLIENT.TAXESADEDUIRE,
                        _LeLCLIENT.DATEFLAG,
                        _LeLCLIENT.MONTANTTVA,
                        _LeLCLIENT.IDCOUPURE,
                        _LeLCLIENT.AGENT_COUPURE,
                        _LeLCLIENT.RDV_COUPURE,
                        _LeLCLIENT.NUMCHEQ,
                        _LeLCLIENT.OBSERVATION_COUPURE,
                        _LeLCLIENT.FK_IDADMUTILISATEUR,
                        _LeLCLIENT.FK_IDCENTRE,
                        _LeLCLIENT.FK_IDLIBELLETOP,
                        _LeLCLIENT.USERCREATION,
                        _LeLCLIENT.DATECREATION,
                        _LeLCLIENT.DATEMODIFICATION,
                        _LeLCLIENT.USERMODIFICATION
                    };
                    return Galatee.Tools.Utility.ListToDataTable(query);
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable RetourneNatureByCoper(string pCoper)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = null;
                    var _NATGEN = context.NATGEN;

                    query =
                    from _LeNATGEN in _NATGEN
                    where _LeNATGEN.COPER  == pCoper
                    select new
                    {
                        _LeNATGEN.CENTRE,
                        _LeNATGEN.AFFECT,
                        _LeNATGEN.CLIPRO,
                        _LeNATGEN.COPER,
                        _LeNATGEN.TRANS,
                        _LeNATGEN.CATEGORIECLIENT,
                        //_LeNATGEN.CODETARIF,
                        _LeNATGEN.DATECREATION,
                        _LeNATGEN.DATEMODIFICATION,
                        _LeNATGEN.USERCREATION,
                        _LeNATGEN.USERMODIFICATION,
                        _LeNATGEN.PK_ID,
                        _LeNATGEN.FK_IDCOPER,
                        _LeNATGEN.FK_IDCENTRE
                    };
                    return Galatee.Tools.Utility.ListToDataTable(query);
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static DataTable RetourneCoper(string pCoper)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = null;
                    var _COPER = context.COPER;

                    query =
                    from _LeCOPER in _COPER
                    where _LeCOPER.CODE  == pCoper
                    select new
                    {
                          _LeCOPER. CODE ,
                        _LeCOPER. LIBCOURT ,
                        _LeCOPER. LIBELLE ,
                        _LeCOPER. COMPTGENE ,
                        _LeCOPER. DC ,
                        _LeCOPER. CTRAIT ,
                        _LeCOPER. DMAJ ,
                        _LeCOPER. TRANS ,
                        _LeCOPER. COMPTEANNEXE1 ,
                        _LeCOPER. USERCREATION ,
                        _LeCOPER.  DATECREATION ,
                        _LeCOPER. DATEMODIFICATION ,
                        _LeCOPER. USERMODIFICATION ,
                        _LeCOPER. PK_ID
                    };
                    return Galatee.Tools.Utility.ListToDataTable(query);
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
        public static void EcrireFichier(string Message, string CheminLog)
        {

            string Buffer = "";
            FileInfo Fichier = new FileInfo(CheminLog);

            if (Fichier.Exists) // on verifie ke le fichier existe
            {
                StreamReader Lecture = new StreamReader(CheminLog, ASCIIEncoding.Default); // on ouvre le fichier
                Buffer = Lecture.ReadToEnd(); // on met la totalité du fichier dans une variable
                Lecture.Close(); // on ferme
            }

            if (Buffer == null || Buffer == "") // on verifie si y a kelke chose dans le fichier, si oui...
            {
                StreamWriter Ecriture = new StreamWriter(CheminLog, false, ASCIIEncoding.Default); // le boolean à false permet d'écraser le fichier existant
                Ecriture.Write(Message + "\r\n"); // on écrit la variable et sa valeur
                Ecriture.Close(); // on ferme
            }
            else // si non...
            {
                StreamWriter Ecriture = new StreamWriter(CheminLog, true, ASCIIEncoding.Default); // le boolean à false permet d'ajouter un ligne sans écraser le fichier
                Ecriture.Write(Message + "\r\n"); // on ajoute la variable plus la valeur (un saut a la ligne avant)
                Ecriture.Close(); // on ferme
            }
        }
        public static DataTable RetourneTousApplicationTaxe()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    var _Etape = context.TAXE.ToList();
                    IEnumerable<object> query =
                    from _etape in _Etape
                    select new CsCodeTaxeApplication
                    {
                        PK_ID = _etape.PK_ID.ToString(),
                        DATECREATION = _etape.DATECREATION,
                        DATEMODIFICATION = _etape.DATEMODIFICATION,
                        LIBELLE = _etape.LIBELLE,
                        PK_CODEAPPLICATION = _etape.CODE,
                        USERCREATION = _etape.USERCREATION,
                        USERMODIFICATION = _etape.USERMODIFICATION
                    };
                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static DataTable ChargerCoutDemande()
        {
            try
            {

                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = null;
                    var CoutDemande = context.COUTDEMANDE ;
                    query =
                    from x in CoutDemande
                    select new
                    {
                        x.PK_ID,
                        x.CENTRE,
                        x.PRODUIT,
                        x.TYPEDEMANDE,
                        x.COPER,
                        x.CATEGORIE,
                        x.REGLAGECOMPTEUR ,
                        x.PUISSANCE,
                        x.TYPETARIF,
                        x.OBLIGATOIRE,
                        x.AUTOMATIQUE,
                        x.SUBVENTIONNEE,
                        x.MONTANT,
                        x.TAXE,
                        x.DATECREATION,
                        x.DATEMODIFICATION,
                        x.USERCREATION,
                        x.USERMODIFICATION,
                        x.FK_IDPRODUIT,
                        x.FK_IDCOPER,
                        x.FK_IDTYPEDEMANDE,
                        x.FK_IDCENTRE,
                        x.FK_IDTAXE,
                        x.FK_IDTYPETARIF,
                        x.FK_IDREGLAGECOMPTEUR ,
                        x.FK_IDCATEGORIECLIENT,
                        x.FK_IDPUISSANCESOUSCRITE,
                        LIBELLECOPER = x.COPER1.LIBELLE ,

                    };

                    return Galatee.Tools.Utility.ListToDataTable(query);
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        #region Dabon
        public static DataTable RetourneDAbon(int Fk_Idcentre, string pCentre, string pNumDem)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = null;
                    var _DABON = context.DABON;

                    query =
                    from _LeDABON in _DABON
                    where
                     ((_LeDABON.CENTRE == pCentre || string.IsNullOrEmpty(pCentre)) &&
                     (_LeDABON.NUMDEM == pNumDem || string.IsNullOrEmpty(pNumDem)) &&
                     (_LeDABON.FK_IDCENTRE == Fk_Idcentre))
                    select new
                    {
                        _LeDABON.CENTRE,
                        _LeDABON.CLIENT,
                        _LeDABON.ORDRE,
                        _LeDABON.PRODUIT,
                        _LeDABON.TYPETARIF,
                        _LeDABON.PUISSANCE,
                        _LeDABON.FORFAIT,
                        _LeDABON.FORFPERSO,
                        _LeDABON.AVANCE,
                        _LeDABON.DAVANCE,
                        _LeDABON.REGROU,
                        _LeDABON.PERFAC,
                        _LeDABON.MOISFAC,
                        _LeDABON.DABONNEMENT,
                        _LeDABON.DRES,
                        _LeDABON.DATERACBRT,
                        _LeDABON.NBFAC,
                        _LeDABON.PERREL,
                        _LeDABON.MOISREL,
                        _LeDABON.DMAJ,
                        _LeDABON.RECU,
                        _LeDABON.RISTOURNE,
                        _LeDABON.CONSOMMATIONMAXI,
                        _LeDABON.COEFFAC,
                        _LeDABON.USERCREATION,
                        _LeDABON.DATECREATION,
                        _LeDABON.DATEMODIFICATION,
                        _LeDABON.USERMODIFICATION,
                        _LeDABON.PK_ID,
                        _LeDABON.FK_IDCENTRE,
                        _LeDABON.FK_IDPRODUIT,
                        _LeDABON.FK_IDFORFAIT,
                        _LeDABON.FK_IDMOISREL,
                        _LeDABON.FK_IDMOISFAC,
                        _LeDABON.FK_IDDEMANDE,
                        _LeDABON.FK_IDTYPETARIF,
                        _LeDABON.FK_IDPERIODICITERELEVE,
                        _LeDABON.FK_IDPERIODICITEFACTURE,
                        LIBELLECENTRE = _LeDABON.CENTRE1.LIBELLE,
                        LIBELLEPRODUIT = _LeDABON.PRODUIT1.LIBELLE,
                        LIBELLETARIF = _LeDABON.TYPETARIF1.LIBELLE,
                        LIBELLEFORFAIT = _LeDABON.FORFAIT1.LIBELLE,
                        LIBELLEMOISFACT = _LeDABON.MOIS.LIBELLE,
                        LIBELLEMOISIND = _LeDABON.MOIS1.LIBELLE,
                        LIBELLEFREQUENCE = _LeDABON.PERIODICITE1.LIBELLE,
                        _LeDABON.NUMDEM
                    };

                    return Galatee.Tools.Utility.ListToDataTable(query);
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        #endregion
        

        #region Sylla


        public static DataTable RetourneEncaissement(int Fk_idcentre, string pCentre, string pClient, string pOrdre, string REFEM)
        {
            try
            {

                List<CsLclient> LstEncaissement = new List<CsLclient>();
                galadbEntities context = new galadbEntities();
                IEnumerable<object> query = null;
                var _TRANSCAISSE = context.TRANSCAISSE;
                query =
                from _LeTRANSCAISSE in _TRANSCAISSE
                where (
                       _LeTRANSCAISSE.REFEM == REFEM &&
                       _LeTRANSCAISSE.FK_IDCENTRE == Fk_idcentre &&
                       _LeTRANSCAISSE.CENTRE == pCentre &&
                       _LeTRANSCAISSE.CLIENT == pClient &&
                       _LeTRANSCAISSE.ORDRE == pOrdre &&
                       _LeTRANSCAISSE.COPER != Enumere.CoperTimbre &&
                       (_LeTRANSCAISSE.TOPANNUL != "O" || _LeTRANSCAISSE.TOPANNUL == null))
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
                    DTRANS = _LeTRANSCAISSE.DTRANS,
                    DENR = _LeTRANSCAISSE.DTRANS,
                    DATEENCAISSEMENT = _LeTRANSCAISSE.DTRANS,
                    FK_IDCENTRE = _LeTRANSCAISSE.FK_IDCENTRE,
                    FK_IDMODEREG = _LeTRANSCAISSE.FK_IDMODEREG,
                    FK_IDCAISSIERE = _LeTRANSCAISSE.FK_IDCAISSIERE,
                    FK_IDHABILITATIONCAISSE = _LeTRANSCAISSE.FK_IDHABILITATIONCAISSE,
                    SAISIPAR = _LeTRANSCAISSE.SAISIPAR,
                    NUMDEM = _LeTRANSCAISSE.NUMDEM,
                    LIBELLECOPER = _LeTRANSCAISSE.COPER1.LIBCOURT,
                    LIBELLEMODREG = _LeTRANSCAISSE.MODEREG1.LIBELLE,
                    NOMCAISSIERE = _LeTRANSCAISSE.HABILITATIONCAISSE == null ? _LeTRANSCAISSE.ADMUTILISATEUR1.LIBELLE : _LeTRANSCAISSE.HABILITATIONCAISSE.ADMUTILISATEUR.LIBELLE


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
                    DC = "C"
                };


                IEnumerable<object> query1 = null;
                var _TRANSCAISB = context.TRANSCAISB;
                query1 =
                from _LeTRANSCAISB in _TRANSCAISB
                where (_LeTRANSCAISB.FK_IDCENTRE == Fk_idcentre &&
                       _LeTRANSCAISB.CENTRE == pCentre &&
                       _LeTRANSCAISB.CLIENT == pClient &&
                       _LeTRANSCAISB.ORDRE == pOrdre &&
                       _LeTRANSCAISB.COPER != Enumere.CoperTimbre &&
                       (_LeTRANSCAISB.TOPANNUL != "O" || _LeTRANSCAISB.TOPANNUL == null))
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
                    NOMCAISSIERE = _LeTRANSCAISB.HABILITATIONCAISSE == null ? _LeTRANSCAISB.ADMUTILISATEUR1.LIBELLE : _LeTRANSCAISB.HABILITATIONCAISSE.ADMUTILISATEUR.LIBELLE


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
                    DC = "C"
                };
                var result = query.Union(query1);
                return Galatee.Tools.Utility.ListToDataTable(result);
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


        #region 12/01/2016

        public static DataTable RetourneListeSortieMaterielLivre()
        {
            using (galadbEntities context = new galadbEntities())
            {
                IEnumerable<object> query = null;
                var _sortie = context.SORTIEMATERIEL;
                query =
                from p in _sortie
                where !string.IsNullOrEmpty(p.LIVRE )
                select new
                {
                    p.PK_ID,
                    p.FK_IDLIVREUR,
                    p.FK_IDRECEPTEUR,
                    p.FK_IDDEMANDE,
                };
                return Galatee.Tools.Utility.ListToDataTable(query);
            }
        }
        public static DataTable RetourneAbonnementAgent(string matricule)
        {
            using (galadbEntities context = new galadbEntities())
            {
                IEnumerable<object> query = null;
                var _user = context.ADMUTILISATEUR ;
                query =
                from p in _user 
                join c in context.CLIENT on p.MATRICULE equals c.MATRICULE into pTempAgent
                from j in pTempAgent.DefaultIfEmpty()
                where p.MATRICULE == matricule
                select new
                {
                    j.CENTRE ,
                    j.REFCLIENT ,
                    j.ORDRE ,
                    MATRICULEAGENT = p.MATRICULE 
                };
                return Galatee.Tools.Utility.ListToDataTable(query);
            }
        }

        public static DataTable RetourneSortieMateriel()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = null;
                    var _Programmation = context.SORTIEMATERIEL ;
                    query =
                    from x in _Programmation
                    join d in context.DEMANDE on x.FK_IDDEMANDE equals d.PK_ID 
                    select new
                    {
                        x.FK_IDDEMANDE,
                        NUMDEM = context.DEMANDE .FirstOrDefault(t => t.PK_ID == x.FK_IDDEMANDE ).NUMDEM ,                      
                        x.DATELIVRAISON,
                        x.DATERECEPTION,
                        LIVREUR = context.ADMUTILISATEUR.FirstOrDefault(t => t.PK_ID == x.FK_IDLIVREUR).LIBELLE,
                        RECEPTEUR = context.ADMUTILISATEUR.FirstOrDefault(t => t.PK_ID == x.FK_IDRECEPTEUR ).LIBELLE,
                    };
                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static DataTable RetourneProgrammation()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = null;
                    var _Programmation = context.PROGRAMMATION;
                    query =
                    from p in _Programmation 
                    from c in p.DEMANDE.DCLIENT 
                    select new
                    {
                        p.PK_ID,
                        p.FK_IDDEMANDE,
                        p.FK_IDEQUIPE,
                        p.DATEPROGRAMME,
                        p.ESTACTIF,
                        p.ISCOMPTEURLIVRE ,
                        p.ISMATERIELLIVRE ,
                        p.DEMANDE.TYPEDEMANDE ,
                        c.NOMABON,
                        p.DEMANDE.NUMDEM ,
                        p.DEMANDE.ISPRESTATION ,
                        LIBELLETYPEDEMANDE = p.DEMANDE.TYPEDEMANDE1.LIBELLE ,

                    };
                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public static bool DesactivationProgrammation(List<int> pIdDemandeDevis)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    List<PROGRAMMATION> prgtion = context.PROGRAMMATION.Where(p => pIdDemandeDevis.Contains(p.FK_IDDEMANDE.Value)).ToList();

                    prgtion.ForEach(p => p.ESTACTIF = false);

                    Entities.UpdateEntity(prgtion);
                    context.SaveChanges();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        #endregion

        #endregion
        #region fomba 29/10/2015

        public static DataTable RetourneListeDemandeByIdSansClient(List<int> demandes, int IdTypeDemande)
        {
            //cmd.CommandText = "SPX_GUI_RETOURNE_DABON";
            try
            {

                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = null;
                    var _DEMANDE = context.DEMANDE;
                    query =
                    from p in _DEMANDE
                    from x in p.DEPANNAGE 
                    where demandes.Contains(p.PK_ID) && p.FK_IDTYPEDEMANDE == IdTypeDemande 
                    select new
                    {
                        p.NUMDEM,
                        p.CENTRE,
                        p.NUMPERE,
                        p.TYPEDEMANDE,
                        p.DPRRDV,
                        p.DPRDEV,
                        p.DPREX,
                        p.DREARDV,
                        p.DREADEV,
                        p.DREAEX,
                        p.HRDVPR,
                        p.FDEM,
                        p.FREP,
                        p.NOMPERE,
                        p.NOMMERE,
                        p.MATRICULE,
                        p.STATUT,
                        p.DCAISSE,
                        p.NCAISSE,
                        p.EXDAG,
                        p.EXDBRT,
                        p.PRODUIT,
                        p.EXCL,
                        p.CLIENT,
                        p.EXCOMPT,
                        p.EXEVT,
                        p.CTAXEG,
                        p.DATED,
                        p.REFEM,
                        p.ORDRE,
                        p.TOPEDIT,
                        p.FACTURE,
                        p.DATEFLAG,
                        //p.NUMDEM,
                        p.USERCREATION,
                        p.DATECREATION,
                        p.DATEMODIFICATION,
                        p.USERMODIFICATION,
                        p.ETAPEDEMANDE,
                        p.PK_ID,
                        p.FK_IDCENTRE,
                        p.FK_IDADMUTILISATEUR,
                        p.FK_IDTYPEDEMANDE,
                        p.FK_IDPRODUIT,
                        p.STATUTDEMANDE,
                        p.ANNOTATION,
                        p.FICHIERJOINT,
                        p.REGLAGECOMPTEUR,
                        p.PUISSANCESOUSCRITE,
                        p.ISETALONNAGE,
                        p.ISEXTENSION,
                        p.ISGRANDCOMPTE,
                        LIBELLECOMMUNE = x.COMMUNE.LIBELLE,
                        LIBELLEQUARTIER=   x.QUARTIER.LIBELLE ,
                        LIBELLERUES=   x.RUES.LIBELLE ,
                        p.TYPECOMPTAGE,
                        p.FK_IDPUISSANCESOUSCRITE,
                        p.FK_IDREGLAGECOMPTEUR,
                        p.FK_IDTYPECOMPTAGE 
                    };
                    return Galatee.Tools.Utility.ListToDataTable(query);

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable RetourneListeDemandeEtapeById(List<int> demandes, int IdTypeDemande)
        {
            //cmd.CommandText = "SPX_GUI_RETOURNE_DABON";
            try
            {

                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = null;
                    var _DEMANDE = context.DEMANDE;
                    query =
                    from P in _DEMANDE
                    from cn in P.DCLIENT
                    where demandes.Contains(P.PK_ID) && P.FK_IDTYPEDEMANDE == IdTypeDemande 
                    select new
                    {
                        P.MOTIF,
                        P.NUMDEM,
                        P.CENTRE,
                        P.NUMPERE,
                        P.TYPEDEMANDE,
                        P.DPRRDV,
                        P.DPRDEV,
                        P.DPREX,
                        P.DREARDV,
                        P.DREADEV,
                        P.DREAEX,
                        P.HRDVPR,
                        P.FDEM,
                        P.FREP,
                        P.NOMPERE,
                        P.NOMMERE,
                        P.MATRICULE,
                        P.STATUT,
                        P.DCAISSE,
                        P.NCAISSE,
                        P.EXDAG,
                        P.EXDBRT,
                        P.PRODUIT,
                        P.EXCL,
                        P.CLIENT,
                        P.EXCOMPT,
                        P.COMPTEUR,
                        P.EXEVT,
                        P.CTAXEG,
                        P.DATED,
                        P.REFEM,
                        P.ORDRE,
                        P.TOPEDIT,
                        P.FACTURE,
                        P.DATEFLAG,
                        P.TRANSMIS,
                        P.STATUTDEMANDE,
                        P.FICHIERJOINT,
                        P.ANNOTATION,
                        P.USERCREATION,
                        P.DATECREATION,
                        P.DATEMODIFICATION,
                        P.USERMODIFICATION,
                        P.ETAPEDEMANDE,
                        P.ISSUPPRIME,
                        P.USERSUPPRESSION,
                        P.DATESUPPRESSION,
                        P.PK_ID,
                        P.FK_IDCENTRE,
                        P.FK_IDCLIENT,
                        P.FK_IDADMUTILISATEUR,
                        P.FK_IDTYPEDEMANDE,
                        P.FK_IDPRODUIT,
                        NOMCLIENT = cn.NOMABON,
                        LIBELLECENTRE = P.CENTRE1.LIBELLE,
                        LIBELLETYPEDEMANDE = P.TYPEDEMANDE1.LIBELLE,
                        LIBELLEPRODUIT = P.PRODUIT1.LIBELLE,
                        LIBELLESITE = P.CENTRE1.SITE.LIBELLE,
                        P.ISCHANGECOMPTEUR,
                        P.ISCONTROLE,
                        P.ISEXTENSION,
                        P.ISPRESTATION,
                        P.ISMUTATION,
                        P.ISPOSE,
                        P.ISETALONNAGE,
                        P.REGLAGECOMPTEUR,
                        P.PUISSANCESOUSCRITE,
                        P.DATEFIN,
                        P.ISMETREAFAIRE,
                        P.TYPECOMPTAGE,
                        P.FK_IDPUISSANCESOUSCRITE,
                        P.FK_IDREGLAGECOMPTEUR,
                        P.FK_IDTYPECOMPTAGE,
                        P.ISDEVISCOMPLEMENTAIRE,
                        P.ISBONNEINITIATIVE,
                        P.ISCOMMUNE,
                        P.ISEDM,
                        P.NOMBREDEFOYER,
                        P.ISDEFINITIF,
                        P.ISPROVISOIR,
                        P.FK_IDDEMANDE,
                        P.ISPASSERCAISSE,
                        P.ISDEVISHT,
                        CODEREGLAGECOMPTEUR = P.REGLAGECOMPTEUR1.REGLAGE,
                        SITE =  P.CENTRE1.CODESITE 
 
 
                    };
                    return Galatee.Tools.Utility.ListToDataTable(query);

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable RetourneListeDemandeById(List<int> demandes)
        {
            //cmd.CommandText = "SPX_GUI_RETOURNE_DABON";
            try
            {

                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = null;
                    var _DEMANDE = context.DEMANDE;
                    query =
                    from P in _DEMANDE
                    from cn in P.DCLIENT
                    where demandes.Contains(P.PK_ID)
                    select new
                    {
                        P.MOTIF,
                        P.NUMDEM,
                        P.CENTRE,
                        P.NUMPERE,
                        P.TYPEDEMANDE,
                        P.DPRRDV,
                        P.DPRDEV,
                        P.DPREX,
                        P.DREARDV,
                        P.DREADEV,
                        P.DREAEX,
                        P.HRDVPR,
                        P.FDEM,
                        P.FREP,
                        P.NOMPERE,
                        P.NOMMERE,
                        P.MATRICULE,
                        P.STATUT,
                        P.DCAISSE,
                        P.NCAISSE,
                        P.EXDAG,
                        P.EXDBRT,
                        P.PRODUIT,
                        P.EXCL,
                        P.CLIENT,
                        P.EXCOMPT,
                        P.COMPTEUR,
                        P.EXEVT,
                        P.CTAXEG,
                        P.DATED,
                        P.REFEM,
                        P.ORDRE,
                        P.TOPEDIT,
                        P.FACTURE,
                        P.DATEFLAG,
                        P.TRANSMIS,
                        P.STATUTDEMANDE,
                        P.FICHIERJOINT,
                        P.ANNOTATION,
                        P.USERCREATION,
                        P.DATECREATION,
                        P.DATEMODIFICATION,
                        P.USERMODIFICATION,
                        P.ETAPEDEMANDE,
                        P.ISSUPPRIME,
                        P.USERSUPPRESSION,
                        P.DATESUPPRESSION,
                        P.PK_ID,
                        P.FK_IDCENTRE,
                        P.FK_IDCLIENT,
                        P.FK_IDADMUTILISATEUR,
                        P.FK_IDTYPEDEMANDE,
                        P.FK_IDPRODUIT,
                        NOMCLIENT = cn.NOMABON,
                        LIBELLECENTRE = P.CENTRE1.LIBELLE,
                        LIBELLETYPEDEMANDE = P.TYPEDEMANDE1.LIBELLE,
                        LIBELLEPRODUIT = P.PRODUIT1.LIBELLE,
                        LIBELLESITE = P.CENTRE1.SITE.LIBELLE,
                        P.ISCHANGECOMPTEUR ,
                        P.ISCONTROLE ,
                        P.ISEXTENSION ,
                        P.ISPRESTATION,
                        P.ISMUTATION ,
                        P.ISPOSE,
                        P.ISETALONNAGE ,
                        P.REGLAGECOMPTEUR ,
                        P.PUISSANCESOUSCRITE,
                        P.DATEFIN,
                        P.ISMETREAFAIRE,
                        P.TYPECOMPTAGE ,
                        P.FK_IDPUISSANCESOUSCRITE ,
                        P.FK_IDREGLAGECOMPTEUR ,
                        P.FK_IDTYPECOMPTAGE,
                        P.ISDEVISCOMPLEMENTAIRE,
                        P.ISBONNEINITIATIVE,
                        P.ISCOMMUNE,
                        P.ISEDM,
                        P.NOMBREDEFOYER,
                        P.ISDEFINITIF,
                        P.ISPROVISOIR,
                        P.FK_IDDEMANDE ,
                        P.ISPASSERCAISSE,
                        P.ISDEVISHT,
                        CODEREGLAGECOMPTEUR = P.REGLAGECOMPTEUR1.REGLAGE,
                        SITE =  P.CENTRE1.CODESITE 
 
                    };
                    return Galatee.Tools.Utility.ListToDataTable(query);

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable RetourneListeCompteurLaboratoireLibre()
        {
            //cmd.CommandText = "SPX_GUI_RETOURNE_DABON";
            try
            {

                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = null;
                    var _Compteur = context.TbCompteurBTA;
                    var _typecompteur = context.TYPEBRANCHEMENT;
                    query =
                    from p in _Compteur
                    join tp in _typecompteur on p.Type_Compteur_ID equals tp.PK_ID

                    where
                        p.StatutCompteur.Equals("Affecté")


                    select new
                    {

                        NUMERO = p.Numero_Compteur,
                        TYPECOMPTEUR = tp.LIBELLE,
                        //DIAMETRE = p.DIAMETRE,
                        MARQUE = p.MARQUE,
                        ANNEEFAB = p.ANNEEFAB,

                        p.CADRAN,
                        p.USERCREATION,
                        p.DATECREATION,
                        p.DATEMODIFICATION,
                        p.USERMODIFICATION,
                        p.FK_IDMARQUECOMPTEUR,
                        FK_IDTYPECOMPTEUR = tp.PK_ID,
                        p.FK_IDCALIBRECOMPTEUR 

                    };
                    return Galatee.Tools.Utility.ListToDataTable(query);


                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable RetourneListeCompteurMagasinCentre(List<string> lstCodeSite)
        {
            //cmd.CommandText = "SPX_GUI_RETOURNE_DABON";
            try
            {

                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = null;
                    var _Compteur = context.MAGASINVIRTUEL;
                    query =
                    from p in _Compteur
                    where
                        p.ETAT.Equals(Enumere.CompteurAffecte)
                        && lstCodeSite.Contains(p.CENTRE.CODESITE)

                    select new
                    {

                        NUMERO = p.NUMERO,
                        TYPECOMPTEUR = p.TYPECOMPTEUR.CODE ,
                        DIAMETRE = p.CALIBRECOMPTEUR ,
                        MARQUE = p.MARQUECOMPTEUR.CODE,
                        ANNEEFAB = p.ANNEEFAB,
                        p.PK_ID,
                        p.CADRAN,
                        p.USERCREATION,
                        p.DATECREATION,
                        p.DATEMODIFICATION,
                        p.USERMODIFICATION,
                        p.FK_IDMARQUECOMPTEUR,
                        FK_IDTYPECOMPTEUR = p.TYPECOMPTEUR.PK_ID,
                        p.FK_IDCALIBRECOMPTEUR ,
                        LIBELLETYPE = p.TYPECOMPTEUR.LIBELLE ,
                        LIBELLECALIBRE = p.CALIBRECOMPTEUR1.LIBELLE ,
                        LIBELLEMARQUE =p.MARQUECOMPTEUR.LIBELLE ,
                        CODEPRODUIT = p.PRODUIT.CODE ,
                        CODECENTRE = p.CENTRE.CODE ,
                        CODESITE = p.CENTRE.CODESITE 
                    };
                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        // TABLE GREC
        public static DataTable RetourneListeGroupe(string centre)
        {
            //cmd.CommandText = "SPX_GUI_RETOURNE_DABON";
            try
            {

                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = null;
                    var _groupe = context.GRC_GROUPE;
                    query =
                    from p in _groupe
                    //where 
                    //p.ID_ENTITE.Equals(centre) && 
                    //p.ID_TYPE_GROUPE  == 2
                    select new
                    {

                        p.ID,
                        p.LIBELLE,
                        //p.ID_TYPE_GROUPE,
                        p.ID_ENTITE,
                        p.ID_TYPE_RECLAMATION,
                        p.EST_SUPPRIME,
                        p.ID_NATURE_BASE,
                        p.CREER_PAR,
                        p.DATE_CREATION,
                        p.DERNIER_UTILISATEUR,
                        p.DATE_MODIFICATION

                    };
                    return Galatee.Tools.Utility.ListToDataTable(query);


                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable RetourneDetailTypePanne()
        {
            //cmd.CommandText = "SPX_GUI_RETOURNE_DABON";
            try
            {

                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = null;
                    var _groupe = context.GRC_PANNE ;
                    query =
                    from p in _groupe
                    where 
                    p.EST_SUPPRIME != true 
                    select new
                    {
                        p.ID,
                       LIBELLE= p.LIBELLE.TrimEnd(),
                        p.ID_TYPE_RECLAMATION,
                    };
                    return Galatee.Tools.Utility.ListToDataTable(query);


                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable RetourneTypePanne()
        {
            //cmd.CommandText = "SPX_GUI_RETOURNE_DABON";
            try
            {

                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = null;
                    var _groupe = context.GRC_TYPE_RECLAMATION ;
                    query =
                    from p in _groupe
                    select new
                    {
                        p.ID,
                        p.LIBELLE,
                        p.CODE 
                    };
                    return Galatee.Tools.Utility.ListToDataTable(query);


                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public static DataTable RetourneModeCommunication()
        {
            //cmd.CommandText = "SPX_GUI_RETOURNE_DABON";
            try
            {

                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = null;
                    var _groupe = context.GRC_MODE_COMMUNICATION ;
                    query =
                    from p in _groupe
                    select new
                    {
                        p.ID,
                        p.LIBELLE,
                    };
                    return Galatee.Tools.Utility.ListToDataTable(query);


                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable RetourneListeAgentGroupe(Guid groupe)
        {
            //cmd.CommandText = "SPX_GUI_RETOURNE_DABON";
            try
            {

                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = null;
                    var _groupe = context.GRC_GROUPE_AFFECTE;
                    query =
                    from p in _groupe
                    where p.ID_GROUPE.Equals(groupe)
                    select new
                    {

                        MATRICULE = p.MATRICULE_AGENT

                    };
                    return Galatee.Tools.Utility.ListToDataTable(query);


                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable RetourneVehicule()
        {
            //cmd.CommandText = "SPX_GUI_RETOURNE_DABON";
            try
            {

                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = null;
                    var _groupe = context.GRC_VEHICULE ;
                    query =
                    from p in _groupe
                    select new
                    {
                        p. ID ,
                        p. LIBELLE ,
                        p. IMMATRICULATION ,
                        p. MARQUE ,
                        p. ID_TYPE_VEHICULE ,
                        p.EST_SUPRIME ,
                        p. ID_ENTITE ,
                        p. DATE_MISE_EN_SERVICE ,
                        p. DATE_FIN_UTILISATION ,
                        p. ID_STATUT ,
                        p. EXPLOITATION ,
                        p. IMMOBILISATION ,
                        p. NUMERORADIO ,
                        p. CREER_PAR ,
                        p. DATE_CREATION ,
                        p. DERNIER_UTILISATEUR ,
                        p. DATE_MODIFICATION
                    };
                    return Galatee.Tools.Utility.ListToDataTable(query);


                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable RetourneGroupeDepannageCommune()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = null;
                    var _groupe = context.COMMUNEGROUPEDEPANNAGE ;
                    query =
                    from p in _groupe
                    select new
                    {
                       p.FK_IDGROUPEVALIDATION ,
                       p.CODECOMMUNE ,
                       p.PK_ID 
                    };
                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        // 
        public static MAGASINVIRTUEL RetourneCompteur(int pk_id)
        {
            using (galadbEntities context = new galadbEntities())
            {
                return context.MAGASINVIRTUEL.FirstOrDefault(c => c.PK_ID == pk_id);
            }
        }

        public static DataTable RetourneListeSortieMaterielLivre(int iddemande)
        {
            using (galadbEntities context = new galadbEntities())
            {
                IEnumerable<object> query = null;
                var _sortie = context.SORTIEMATERIEL;
                query =
                from p in _sortie
                where p.FK_IDDEMANDE == iddemande && !string.IsNullOrEmpty( p.LIVRE)
                select new
                {
                    p.PK_ID,
                    p.FK_IDLIVREUR,
                    p.FK_IDRECEPTEUR,
                    p.FK_IDDEMANDE,
                };
                return Galatee.Tools.Utility.ListToDataTable(query);
            }
        }

        public static DataTable RetourneListeSortieAutreMaterielLivre(int iddemande)
        {
            using (galadbEntities context = new galadbEntities())
            {
                return new DataTable();
                //IEnumerable<object> query = null;
                //var _sortie = context.SORTIEAUTREMATERIEL;
                //query =
                //from p in _sortie
                //where p.FK_IDDEMANDE == iddemande
                //select new
                //{
                //    p.PK_ID,
                //    p.FK_IDDEMANDE,
                //    p.FK_IDTYPEMATERIEL,
                //    p.LIBELLE,
                //    p.NOMBRE,
                //    p.LIVRE,
                //    p.RECU

                //};
                //return Galatee.Tools.Utility.ListToDataTable(query);
            }
        }


        public static bool InsertCompteurMagasin(List<MAGASINVIRTUEL> LesCompteur)
        {
            try
            {
                List<TbCompteurBTA> lstcompteuraffect = new List<TbCompteurBTA>();

                using (galadbEntities ctontext = new galadbEntities())
                {
                    foreach (MAGASINVIRTUEL uncompt in LesCompteur)
                    {
                        lstcompteuraffect.Add(ctontext.TbCompteurBTA.FirstOrDefault(x => x.Numero_Compteur == uncompt.NUMERO));
                    }
                    lstcompteuraffect.ForEach(c => c.StatutCompteur = "Transféré");


                }
                using (galadbEntities ctontext = new galadbEntities())
                {
                    LesCompteur.ForEach(c => c.ETAT = Enumere.CompteurAffecte);
                    Entities.InsertEntity<MAGASINVIRTUEL>(LesCompteur);
                    Entities.UpdateEntity<TbCompteurBTA>(lstcompteuraffect);

                    ctontext.SaveChanges();
                    return true;
                }
            }
            catch (DbEntityValidationException ex)
            {
                // Retrieve the error messages as a list of strings.
                var errorMessages = ex.EntityValidationErrors
                        .SelectMany(x => x.ValidationErrors)
                        .Select(x => x.ErrorMessage);

                // Join the list to a single string.
                var fullErrorMessage = string.Join("; ", errorMessages);

                // Combine the original exception message with the new one.
                var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

                return false;
                // Throw a new DbEntityValidationException with the improved exception message.
                throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
            }
        }


        #region Modifié le 31-03-2016 par sylla


        #region Nouveau Code 
        public static bool InsertLiaisonCanalisation(List<DCANALISATION> LsCanal,List<CsDemandeBase> lstDemandeALier)
        {
            try
            {
                int id;
                int res = -1;
                DEMANDE DEM = new DEMANDE();
                List<MAGASINVIRTUEL> lstcpt = new List<MAGASINVIRTUEL>();
                using (galadbEntities ctontext = new galadbEntities())
                {
                    foreach (CsDemandeBase  item in lstDemandeALier)
                    {
                        DEM = ctontext.DEMANDE.FirstOrDefault(c => c.PK_ID == item.PK_ID );
                        DEM.STATUT = ((int)Enumere.EtapeBrtAbonSanExt.Programation).ToString();

                        DCANALISATION leCptLier = LsCanal.FirstOrDefault(t => t.FK_IDDEMANDE == item.PK_ID);
                        if (leCptLier != null && leCptLier.FK_IDDEMANDE != 0)
                        {
                            MAGASINVIRTUEL cpt = ctontext.MAGASINVIRTUEL.FirstOrDefault(c => c.PK_ID == leCptLier.FK_IDMAGAZINVIRTUEL );
                            cpt.ETAT = Enumere.CompteurLie;
                        }
                        if (leCptLier.PRODUIT == Enumere.ElectriciteMT)
                        {
                            List<DCANALISATION> ListCanalisation = new List<DCANALISATION>();
                            for (int i = 1; i <= 6; i++)
                            {
                                DCANALISATION leCpt = Galatee.Tools.Utility.RetourneCopyObjet<DCANALISATION>(leCptLier);
                                leCpt.POINT = i;
                                ListCanalisation.Add(leCpt);
                            }
                            Entities.InsertEntity<DCANALISATION>(ListCanalisation, ctontext);
                        }
                        else
                        {
                            leCptLier.POINT = 1;
                            Entities.InsertEntity<DCANALISATION>(leCptLier, ctontext);
                        }
                    }
                   res= ctontext.SaveChanges();
                }
                return res == -1 ? false : true;
            }
            catch (DbEntityValidationException ex)
            {

                var errorMessages = ex.EntityValidationErrors
                       .SelectMany(x => x.ValidationErrors)
                       .Select(x => x.ErrorMessage);

                // Join the list to a single string.
                var fullErrorMessage = string.Join("; ", errorMessages);

                // Combine the original exception message with the new one.
                var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

                return false;
                // Throw a new DbEntityValidationException with the improved exception message.
                throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);

                throw ex;
            }

        }

        #endregion

        #region Ancien Code
        //public static bool InsertLiaisonCanalisation(List<DCANALISATION> LsCanal, List<COMPTEUR> lstcompteur)
        //{
        //    try
        //    {
        //        int id;
        //        DEMANDE DEM = new DEMANDE();
        //        List<MAGASINVIRTUEL> lstcpt = new List<MAGASINVIRTUEL>();
        //        using (galadbEntities ctontext = new galadbEntities())
        //        {
        //            id = LsCanal.FirstOrDefault().FK_IDDEMANDE;
        //            DEM = ctontext.DEMANDE.FirstOrDefault(c => c.PK_ID == id);
        //            DEM.STATUT = ((int)Enumere.EtapeBrtAbonSanExt.Programation).ToString();

        //            foreach (DCANALISATION can in LsCanal)
        //            {
        //                int idL = int.Parse(can.FK_IDCOMPTEUR.ToString());
        //                MAGASINVIRTUEL cpt = ctontext.MAGASINVIRTUEL.FirstOrDefault(c => c.PK_ID == idL);
        //                cpt.ETAT = Enumere.CompteurLie;
        //                lstcpt.Add(cpt);
        //            }

        //        }
        //        using (galadbEntities ctontext = new galadbEntities())
        //        {

        //            Entities.InsertEntity<DCANALISATION>(LsCanal, ctontext);
        //            //Entities.InsertEntity<COMPTEUR>(lstcompteur, ctontext);

        //            ctontext.SaveChanges();
        //        }

        //        using (galadbEntities ctontext = new galadbEntities())
        //        {
        //            Entities.UpdateEntity<MAGASINVIRTUEL>(lstcpt);
        //            Entities.UpdateEntity<DEMANDE>(DEM);
        //            ctontext.SaveChanges();
        //        }

        //        return true;
        //    }
        //    catch (DbEntityValidationException ex)
        //    {

        //        var errorMessages = ex.EntityValidationErrors
        //               .SelectMany(x => x.ValidationErrors)
        //               .Select(x => x.ErrorMessage);

        //        // Join the list to a single string.
        //        var fullErrorMessage = string.Join("; ", errorMessages);

        //        // Combine the original exception message with the new one.
        //        var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

        //        return false;
        //        // Throw a new DbEntityValidationException with the improved exception message.
        //        throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);

        //        throw ex;
        //    }

        //}

        #endregion

        #endregion


        public static bool InsertGroupe(GRC_GROUPE legroupe, List<ADMUTILISATEUR> lstAgent)
        {
            try
            {
                using (galadbEntities ctontext = new galadbEntities())
                {
                    Entities.InsertEntity<GRC_GROUPE>(legroupe);
                    List<GRC_GROUPE_AFFECTE> lstmembre = new List<GRC_GROUPE_AFFECTE>();

                    foreach (ADMUTILISATEUR unagent in lstAgent)
                    {

                        DateTime? date = null;

                        GRC_GROUPE_AFFECTE leMembre = new GRC_GROUPE_AFFECTE()
                        {
                            ID = Guid.NewGuid(),
                            ID_GROUPE = legroupe.ID,
                            MATRICULE_AGENT = unagent.MATRICULE,
                            CREER_PAR = legroupe.CREER_PAR,
                            DATE_CREATION = DateTime.Now,
                            DERNIER_UTILISATEUR = legroupe.CREER_PAR,
                            DATE_MODIFICATION = DateTime.Now,
                            EST_SUPPRIME = false

                        };

                        lstmembre.Add(leMembre);

                    }
                    Entities.UpdateEntity<GRC_GROUPE_AFFECTE>(lstmembre);
                    ctontext.SaveChanges();
                    return true;
                }
            }
            catch (DbEntityValidationException ex)
            {

                var errorMessages = ex.EntityValidationErrors
                       .SelectMany(x => x.ValidationErrors)
                       .Select(x => x.ErrorMessage);

                // Join the list to a single string.
                var fullErrorMessage = string.Join("; ", errorMessages);

                // Combine the original exception message with the new one.
                var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

                // Throw a new DbEntityValidationException with the improved exception message.
                throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
                return false;
                throw ex;
            }
        }

        public static bool InsertProgrammation(Guid idgroupe, List<DEMANDE> lstDemande, DateTime dateprog)
        {
            try
            {
                List<DEMANDE> lstdem = new List<DEMANDE>();
                List<PROGRAMMATION> lstprogrammation = new List<PROGRAMMATION>();

                using (galadbEntities context = new galadbEntities())
                {

                    foreach (DEMANDE demande in lstDemande)
                    {
                        PROGRAMMATION prog = new PROGRAMMATION()
                        {
                            DATEPROGRAMME = dateprog,
                            FK_IDEQUIPE = idgroupe,
                            FK_IDDEMANDE = demande.PK_ID,
                            ESTACTIF=true
                        };

                        demande.STATUT = ((int)Enumere.EtapeBrtAbonSanExt.SortieMat).ToString();
                        lstprogrammation.Add(prog);
                        lstdem.Add(demande);
                    }
                }

                using (galadbEntities context = new galadbEntities())
                {
                    Entities.InsertEntity(lstprogrammation);
                    Entities.UpdateEntity(lstdem);
                    return true;
                }

            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public static bool InsertSortieMateriel(int IdLivreur, int IdRecepteur, List<CsCanalisation> lstDemande, bool IsExtension)
        {
            try
            {
                int res = -1;
                int? recept = IdRecepteur;
                if (IdRecepteur == 0)
                    recept = null;

                using (galadbEntities context = new galadbEntities())
                {
                    List<SORTIEMATERIEL> lstMateriel = new List<SORTIEMATERIEL>();
                    foreach (CsCanalisation can in lstDemande)
                    {
                        SORTIEMATERIEL prog = new SORTIEMATERIEL()
                        {
                            FK_IDLIVREUR = IdLivreur,
                            FK_IDRECEPTEUR = recept,
                            FK_IDDEMANDE = can.FK_IDDEMANDE,
                            ACTIF = true,
                            LIVRE = Enumere.CompteurLivre,
                            DATELIVRAISON = System.DateTime.Today,
                            DATECREATION = System.DateTime.Now
                        };
                        lstMateriel.Add(prog);
                        List<PROGRAMMATION> lstProgramme = context.PROGRAMMATION.Where(t => t.FK_IDDEMANDE == can.FK_IDDEMANDE).ToList();
                        if (lstProgramme != null && lstProgramme.Count != 0)
                        {
                            if (!IsExtension)
                            lstProgramme.ForEach(t => t.ISMATERIELLIVRE = true);
                            else
                                lstProgramme.ForEach(t => t.ISMATERIELEXTLIVRE  = true);
                        }
                    }
                    Entities.InsertEntity(lstMateriel, context);
                    res = context.SaveChanges();
                }
                return res == -1 ? false : true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public static bool InsertSortieMaterielEP(int IdLivreur, int IdRecepteur, List<CsDemandeBase> lstDemande, bool IsExtension)
        {
            try
            {
                int res = -1;
                int? recept = IdRecepteur;
                if (IdRecepteur == 0)
                    recept = null;

                using (galadbEntities context = new galadbEntities())
                {
                    List<SORTIEMATERIEL> lstMateriel = new List<SORTIEMATERIEL>();
                    foreach (CsDemandeBase  can in lstDemande)
                    {
                        SORTIEMATERIEL prog = new SORTIEMATERIEL()
                        {
                            FK_IDLIVREUR = IdLivreur,
                            FK_IDRECEPTEUR = recept,
                            FK_IDDEMANDE = can.PK_ID ,
                            ACTIF = true,
                            LIVRE = Enumere.CompteurLivre,
                            DATELIVRAISON = System.DateTime.Today,
                            DATECREATION = System.DateTime.Now
                        };
                        lstMateriel.Add(prog);
                        List<PROGRAMMATION> lstProgramme = context.PROGRAMMATION.Where(t => t.FK_IDDEMANDE == can.PK_ID ).ToList();
                        if (lstProgramme != null && lstProgramme.Count != 0)
                        {
                            if (!IsExtension)
                                lstProgramme.ForEach(t => t.ISMATERIELLIVRE = true);
                            else
                                lstProgramme.ForEach(t => t.ISMATERIELEXTLIVRE = true);
                        }
                    }
                    Entities.InsertEntity(lstMateriel, context);
                    res = context.SaveChanges();
                }
                return res == -1 ? false : true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static bool InsertValidationSortieMateriel(List<CsCanalisation> lstDemande, int idrecepteur)
        {
            try
            {
                int resultat = -1;
                using (galadbEntities context = new galadbEntities())
                {
                    foreach (CsCanalisation can in lstDemande)
                    {
                        List<SORTIEMATERIEL> lesCompteur = context.SORTIEMATERIEL.Where(t => t.FK_IDDEMANDE  == can.FK_IDDEMANDE ).ToList();
                        lesCompteur.ForEach(t => t.FK_IDRECEPTEUR = idrecepteur);
                        lesCompteur.ForEach(t => t.RECU = Enumere.CompteurRecu);
                        lesCompteur.ForEach(t => t.DATERECEPTION = System.DateTime.Today);

                        DEMANDE dem = context.DEMANDE.FirstOrDefault(c => c.PK_ID == can.FK_IDDEMANDE);
                        dem.STATUT = ((int)Enumere.EtapeBrtAbonSanExt.CrTravaux).ToString();
                    }
                    resultat = context.SaveChanges();
                }
                return resultat == -1 ? false : true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        public static bool InsertValidationSortieMateriel( int idrecepteur, List<SORTIEMATERIEL> lstSortie)
        {
            try
            {
                int res = -1;
                using (galadbEntities context = new galadbEntities())
                {
                    lstSortie.ForEach(c => c.FK_IDRECEPTEUR = idrecepteur);
                    lstSortie.ForEach(c => c.RECU = Enumere.CompteurRecu);
                    Entities.UpdateEntity<SORTIEMATERIEL>(lstSortie, context);
                    res =  context.SaveChanges();
                }
                return res == -1 ? false : true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        #region SortieCompteur
        public static bool InsertSortieCompteur(int IdLivreur, int IdRecepteur, List<DCANALISATION> lstDemande)
        {
            try
            {
                List<SORTIECOMPTEUR> LstSortie = new List<SORTIECOMPTEUR>();
                List<MAGASINVIRTUEL> LstMagVirt = new List<MAGASINVIRTUEL>();

                int res = -1;
                int? recept = IdRecepteur;
                if (IdRecepteur == 0)
                    recept = null;

                using (galadbEntities context = new galadbEntities())
                {
                    foreach (DCANALISATION can in lstDemande)
                    {
                        SORTIECOMPTEUR prog = new SORTIECOMPTEUR()
                        {
                            FK_IDLIVREUR = IdLivreur,
                            FK_IDRECEPTEUR = recept,
                            FK_IDDEMANDE = can.FK_IDDEMANDE,
                            FK_IDCANALISATION = can.PK_ID,
                            ACTIF = true,
                            LIVRE = Enumere.CompteurLivre,
                            DATELIVRAISON = System.DateTime.Today, 
                            DATECREATION  = System.DateTime.Now  
                        };
                        LstSortie.Add(prog);
                        MAGASINVIRTUEL cpt = context.MAGASINVIRTUEL.FirstOrDefault(c => c.PK_ID == can.FK_IDMAGAZINVIRTUEL);
                        if (cpt != null)
                            cpt.ETAT = Enumere.CompteurLivre;

                        List<PROGRAMMATION> lstProgramme = context.PROGRAMMATION.Where(t => t.FK_IDDEMANDE == can.FK_IDDEMANDE).ToList();
                        if (lstProgramme != null && lstProgramme.Count != 0)
                        {
                            lstProgramme.ForEach(t => t.ISCOMPTEURLIVRE = true);
                            lstProgramme.ForEach(t => t.ESTACTIF  = true);
                        }
                    }
                    Entities.InsertEntity(LstSortie, context);
                   res= context.SaveChanges();
                }
                return res == -1 ? false : true;
            }
            catch (Exception ex)
            {
                return false  ;
            }
        }
        public static bool InsertValidationSortieCompteur(List<DCANALISATION> lstDemande, int idrecepteur)
        {
            try
            {
                int resultat = -1;
                using (galadbEntities context = new galadbEntities())
                {
                    foreach (DCANALISATION can in lstDemande)
                    {
                        List<SORTIECOMPTEUR> lesCompteur = context.SORTIECOMPTEUR.Where(t => t.FK_IDCANALISATION == can.PK_ID).ToList();
                        lesCompteur.ForEach(t => t.FK_IDRECEPTEUR = idrecepteur);
                        lesCompteur.ForEach(t => t.RECU = Enumere.CompteurRecu);
                        lesCompteur.ForEach(t => t.DATERECEPTION  = System.DateTime.Today );

                        DEMANDE dem = context.DEMANDE.FirstOrDefault(c => c.PK_ID == can.FK_IDDEMANDE);
                        dem.STATUT = ((int)Enumere.EtapeBrtAbonSanExt.CrTravaux).ToString();

                        MAGASINVIRTUEL cpt = context.MAGASINVIRTUEL.FirstOrDefault(c => c.PK_ID == can.FK_IDMAGAZINVIRTUEL);
                        cpt.ETAT = Enumere.CompteurRecu;
                       
                    }
                    resultat = context.SaveChanges();
                }
                return resultat == -1 ? false : true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        #endregion

        #endregion

        public static DataTable ChargerOrdreDeTravail(int iddemande)
        {
            try
            {

                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = null;
                    var OrdreTravail = context.DORDRETRAVAIL;
                    query =
                    from x in OrdreTravail
                    where x.FK_IDDEMANDE == iddemande 
                    select new
                    {
                       x.PK_ID,
                       x.DATECREATION ,
                       x.DATEMODIFICATION ,
                       x.FK_IDADMUTILISATEUR ,
                       LIBELLEAGENT =  x.ADMUTILISATEUR.LIBELLE ,
                       x.MATRICULE ,
                       x.PRESTATAIRE ,
                       x.FK_IDDEMANDE ,
                       x.DATEDEBUTTRAVAUX ,
                       x.DATEFINTRAVAUX,
                       x.COMMENTAIRE ,
                       x.USERCREATION ,
                       x.USERMODIFICATION 
                    };

                    return Galatee.Tools.Utility.ListToDataTable(query);
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable ChargerControleTravaux(int iddemande)
        {
            try
            {

                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = null;
                    var Control = context.CONTROLETRAVAUX ;
                    query =
                    from x in Control
                    where x.FK_IDDEMANDE == iddemande
                    select new
                    {
                        x.PK_ID,
                        x.DATECREATION,
                        x.DATEMODIFICATION,
                        LIBELLEAGENT = x.ADMUTILISATEUR.LIBELLE,
                        x.FK_IDDEMANDE,
                        x.DATECONTROLE ,
                        x.COMMENTAIRE,
                        x.USERCREATION,
                        x.USERMODIFICATION
                    };

                    return Galatee.Tools.Utility.ListToDataTable(query);
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public static DataTable ChargerAnnotationDemande(int iddemande)
        {
            try
            {

                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = null;
                    var Control = context.DANNOTATION ;
                    query =
                    from x in Control
                    where x.FK_IDDEMANDE == iddemande
                    select new
                    {
                        x.PK_ID,
                        x.DATECREATION,
                        x.DATEMODIFICATION,
                        LIBELLEAGENT = x.ADMUTILISATEUR.LIBELLE,
                        x.FK_IDDEMANDE,
                        x.MATRICULE  ,
                        x.COMMENTAIRE,
                        x.USERCREATION,
                        x.USERMODIFICATION
                    };

                    return Galatee.Tools.Utility.ListToDataTable(query);
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable ChargerDemandeDepanage(int iddemande)
        {
            try
            {

                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = null;
                    var Control = context.DEPANNAGE ;
                    query =
                    from x in Control
                    join y in context.TRAVAUXDEVIS on x.FK_IDDEMANDE equals y.FK_IDDEMANDE into result
                    from p in result .DefaultIfEmpty()
                    where x.FK_IDDEMANDE == iddemande
                    select new
                    {
                      x.PK_ID ,
                      x.FK_IDRUE ,
                      x.FK_IDCENTRE ,
                      x.FK_IDCOMMUNE ,
                      x.FK_IDDEMANDE ,
                      x.FK_IDQUARTIER ,
                      x.FK_IDSECTEUR ,
                      x.FK_IDTYPEDEPANNE ,
                      LARUE = x.RUES.LIBELLE,
                      LESECTEUR = x.SECTEUR.LIBELLE,
                      LACOMMUNE = x.COMMUNE .LIBELLE,
                      LEQUARTIER = x.QUARTIER.LIBELLE,
                      MODERECUEIL = x.GRC_MODE_COMMUNICATION.LIBELLE,
                      TYPEDEPANNE = x.GRC_PANNE.LIBELLE,
                      DESCRIPTIONPANNE = x.DESCRIPTIONPANNE ,
                      x.ISPERSONNEEXTERIEUR,
                      p.PROCESVERBAL ,
                      PANNETRAITE =  p.GRC_PANNE.LIBELLE, 
                      x.HEUREDEBUT ,
                      x.HEUREFIN ,
                      x.ISRESEAU ,
                      x.ISBRANCHEMENT ,
                      x.ISPROVISOIR ,
                      x.ISDEFINITIF 
                    };

                    return Galatee.Tools.Utility.ListToDataTable(query);
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable ChargerDemandeTransfert(int iddemande)
        {
            try
            {

                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = null;
                    var Control = context.DTRANSFERT ;
                    query =
                    from x in Control
                    where x.FK_IDDEMANDE == iddemande
                    select new
                    {
                        x.PK_ID,
                        x.FK_IDCENTREORIGINE ,
                        x.FK_IDCENTRETRANSFERT,
                        x.FK_IDDEMANDE,
                        x.FK_IDREGROUPEMENT ,
                        LIBELLESITEORIGINE = x.CENTRE.SITE.LIBELLE ,
                        LIBELLECENTREORIGINE = x.CENTRE.LIBELLE,
                        LIBELLESITETRANSFERT = x.CENTRE1.SITE.LIBELLE,
                        LIBELLECENTRETRANSFERT  = x.CENTRE1.LIBELLE,
                        CODECENTRETRANSFERT = x.CENTRE1.CODE ,
                        CODEREGROUPEMENT = x.REGROUPEMENT.CODE,
                        LIBELLEREGROUPEMENT = x.REGROUPEMENT.NOM 
                    };

                    return Galatee.Tools.Utility.ListToDataTable(query);
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region RechercheByIdDEmande
        public static DataTable RetourneDAG(int fk_idemande)
        {
            //cmd.CommandText = "SPX_GUI_RETOURNE_DABON";
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = null;
                    var _DAG = context.DAG;

                    query =
                    from _LeDAG in _DAG
                    where
                      _LeDAG.FK_IDDEMANDE == fk_idemande
                    select new
                    {

                        _LeDAG.ETAGE,
                        _LeDAG.CENTRE,
                        _LeDAG.CLIENT,
                        _LeDAG.NOMP,
                        _LeDAG.COMMUNE,
                        _LeDAG.QUARTIER,
                        _LeDAG.RUE,
                        _LeDAG.PORTE,
                        _LeDAG.CADR,
                        _LeDAG.REGROU,
                        _LeDAG.CPARC,
                        _LeDAG.DMAJ,
                        _LeDAG.TOURNEE,
                        _LeDAG.ORDTOUR,
                        _LeDAG.SECTEUR,
                        _LeDAG.CPOS,
                        _LeDAG.TELEPHONE,
                        _LeDAG.FAX,
                        _LeDAG.EMAIL,
                        _LeDAG.USERCREATION,
                        _LeDAG.DATECREATION,
                        _LeDAG.DATEMODIFICATION,
                        _LeDAG.USERMODIFICATION,
                        _LeDAG.PK_ID,
                        _LeDAG.FK_IDTOURNEE,
                        _LeDAG.FK_IDQUARTIER,
                        LIBELLEQUARTIER = _LeDAG.QUARTIER1.LIBELLE,
                        _LeDAG.FK_IDCOMMUNE,
                        LIBELLECOMMUNE = _LeDAG.COMMUNE1.LIBELLE,
                        _LeDAG.FK_IDRUE,
                        LIBELLERUE = _LeDAG.RUES.LIBELLE,
                        _LeDAG.FK_IDCENTRE,
                        LIBELLESECTEUR = _LeDAG.SECTEUR1.LIBELLE,
                        _LeDAG.FK_IDSECTEUR,
                        _LeDAG.NUMDEM,
                        _LeDAG.FK_IDDEMANDE
                    };
                    return Galatee.Tools.Utility.ListToDataTable(query);
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static DataTable RetourneDBrt(int fk_idemande)
        {
            //cmd.CommandText = "SPX_GUI_RETOURNE_DABON";
            try
            {

                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = null;
                    var _DBRT = context.DBRT;

                    query =
                    from _LeDBRT in _DBRT
                    where
                      _LeDBRT.FK_IDDEMANDE == fk_idemande 
                    select new
                    {
                        _LeDBRT.CENTRE,
                        _LeDBRT.CLIENT,
                        _LeDBRT.PRODUIT,
                        _LeDBRT.DRAC,
                        _LeDBRT.DRES,
                        _LeDBRT.SERVICE,
                        _LeDBRT.CATBRT,
                        _LeDBRT.DIAMBRT,
                        _LeDBRT.LONGBRT,
                        _LeDBRT.NATBRT,
                        _LeDBRT.NBPOINT,
                        _LeDBRT.RESEAU,
                        _LeDBRT.TRONCON,
                        _LeDBRT.DMAJ,
                        _LeDBRT.TRANSFORMATEUR,
                        _LeDBRT.PUISSANCEINSTALLEE,
                        _LeDBRT.PERTES,
                        _LeDBRT.COEFPERTES,
                        _LeDBRT.APPTRANSFO,
                        _LeDBRT.CODEBRT,
                        _LeDBRT.CODEPOSTE,
                        _LeDBRT.MARQUETRANSFO,
                        _LeDBRT.ANFAB,
                        _LeDBRT.LONGITUDE,
                        _LeDBRT.LATITUDE,
                        _LeDBRT.ADRESSERESEAU,
                        _LeDBRT.USERCREATION,
                        _LeDBRT.DATECREATION,
                        _LeDBRT.DATEMODIFICATION,
                        _LeDBRT.USERMODIFICATION,
                        _LeDBRT.PK_ID,
                        _LeDBRT.FK_IDCENTRE,
                        _LeDBRT.FK_IDPRODUIT,
                        _LeDBRT.FK_IDTYPEBRANCHEMENT,
                        LIBELLETYPEBRANCHEMENT = _LeDBRT.TYPEBRANCHEMENT.LIBELLE,
                        CODETYPEBRANCHEMENT = _LeDBRT.TYPEBRANCHEMENT.CODE,
                        _LeDBRT.FK_IDDEMANDE,
                        _LeDBRT.NUMDEM,

                        _LeDBRT.FK_IDPOSTESOURCE,
                        CODEPOSTESOURCE = _LeDBRT.POSTESOURCE.CODE,
                        LIBELLEPOSTESOURCE = _LeDBRT.POSTESOURCE.LIBELLE,

                        _LeDBRT.FK_IDDEPARTHTA,
                        CODEDEPARTHTA = _LeDBRT.DEPARTHTA.CODE,
                        LIBELLEDEPARTHTA = _LeDBRT.DEPARTHTA.LIBELLE,

                        _LeDBRT.FK_IDPOSTETRANSFORMATION,
                        CODETRANSFORMATEUR = _LeDBRT.POSTETRANSFORMATION.CODE,
                        LIBELLETRANSFORMATEUR = _LeDBRT.POSTETRANSFORMATION.LIBELLE,

                        _LeDBRT.FK_IDQUARTIER,
                        CODEQUARTIER = _LeDBRT.QUARTIER.CODE,
                        LIBELLEQUARTIER = _LeDBRT.QUARTIER.LIBELLE,

                        _LeDBRT.FK_IDDEPARTBT,
                        CODEDEPARTBT = _LeDBRT.DEPARTBT.CODE,
                        LIBELLEDEPARTBT = _LeDBRT.DEPARTBT.LIBELLE,

                        _LeDBRT.NEOUDFINAL,
                        _LeDBRT.NOMBRETRANSFORMATEUR 
                    };
                    return Galatee.Tools.Utility.ListToDataTable(query);
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static DataTable RetourneDClient(int fk_idemande)
        {
            //cmd.CommandText = "SPX_GUI_RETOURNE_DABON";
            try
            {

                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = null;
                    var _DCLIENT = context.DCLIENT;
                    query =
                    from _LeDCLIENT in _DCLIENT
                    where
                      _LeDCLIENT.FK_IDDEMANDE  == fk_idemande  
                    select new
                    {
                        _LeDCLIENT.ISFACTUREEMAIL,
                        _LeDCLIENT.ISFACTURESMS,
                        _LeDCLIENT.TELEPHONEFIXE,
                        _LeDCLIENT.CODEIDENTIFICATIONNATIONALE,
                        _LeDCLIENT.EMAIL,
                        _LeDCLIENT.NUMPROPRIETE,
                        _LeDCLIENT.CENTRE,
                        _LeDCLIENT.REFCLIENT,
                        _LeDCLIENT.ORDRE,
                        _LeDCLIENT.DENABON,
                        _LeDCLIENT.NOMABON,
                        _LeDCLIENT.DENMAND,
                        _LeDCLIENT.NOMMAND,
                        _LeDCLIENT.ADRMAND1,
                        _LeDCLIENT.ADRMAND2,
                        _LeDCLIENT.CPOS,
                        _LeDCLIENT.BUREAU,
                        _LeDCLIENT.DINC,
                        _LeDCLIENT.MODEPAIEMENT,
                        _LeDCLIENT.NOMTIT,
                        _LeDCLIENT.BANQUE,
                        _LeDCLIENT.GUICHET,
                        _LeDCLIENT.COMPTE,
                        _LeDCLIENT.RIB,
                        _LeDCLIENT.PROPRIO,
                        _LeDCLIENT.CODECONSO,
                        _LeDCLIENT.CATEGORIE,
                        _LeDCLIENT.CODERELANCE,
                        _LeDCLIENT.NOMCOD,
                        _LeDCLIENT.MOISNAIS,
                        _LeDCLIENT.ANNAIS,
                        _LeDCLIENT.NUMDEM,
                        _LeDCLIENT.NOMPERE,
                        _LeDCLIENT.NOMMERE,
                        _LeDCLIENT.NATIONNALITE,
                        _LeDCLIENT.CNI,
                        _LeDCLIENT.TELEPHONE,
                        _LeDCLIENT.MATRICULE,
                        _LeDCLIENT.REGROUPEMENT,
                        _LeDCLIENT.REGEDIT,
                        _LeDCLIENT.FACTURE,
                        _LeDCLIENT.DMAJ,
                        _LeDCLIENT.REFERENCEPUPITRE,
                        _LeDCLIENT.PAYEUR,
                        _LeDCLIENT.SOUSACTIVITE,
                        _LeDCLIENT.AGENTFACTURE,
                        _LeDCLIENT.AGENTRECOUVR,
                        _LeDCLIENT.AGENTASSAINI,
                        _LeDCLIENT.REGROUCONTRAT,
                        _LeDCLIENT.INSPECTION,
                        _LeDCLIENT.REGLEMENT,
                        _LeDCLIENT.DECRET,
                        _LeDCLIENT.CONVENTION,
                        _LeDCLIENT.REFERENCEATM,
                        _LeDCLIENT.PK_ID,
                        _LeDCLIENT.DATECREATION,
                        _LeDCLIENT.DATEMODIFICATION,
                        _LeDCLIENT.USERCREATION,
                        _LeDCLIENT.USERMODIFICATION,
                        _LeDCLIENT.FK_IDMODEPAIEMENT,
                        _LeDCLIENT.FK_IDCODECONSO,
                        _LeDCLIENT.FK_IDCATEGORIE,
                        _LeDCLIENT.FK_IDPROPRIETAIRE,
                        _LeDCLIENT.FK_IDRELANCE,
                        _LeDCLIENT.FK_IDPIECEIDENTITE,
                        _LeDCLIENT.NUMEROPIECEIDENTITE,
                        _LeDCLIENT.FK_IDNATIONALITE,
                        _LeDCLIENT.FK_IDREGROUPEMENT,
                        _LeDCLIENT.FK_IDDEMANDE,
                        _LeDCLIENT.FK_IDCENTRE,
                        _LeDCLIENT.FK_TYPECLIENT,
                        _LeDCLIENT.FK_IDUSAGE,
                        _LeDCLIENT.FAX,
                        _LeDCLIENT.BOITEPOSTAL,
                        LIBELLEMODEPAIEMENT = _LeDCLIENT.MODEPAIEMENT1.LIBELLE,
                        LIBELLECODECONSO = _LeDCLIENT.CODECONSOMMATEUR.LIBELLE,
                        LIBELLECATEGORIE = _LeDCLIENT.CATEGORIECLIENT.LIBELLE,
                        LIBELLERELANCE  = _LeDCLIENT.RELANCE.LIBELLE,
                        LIBELLENATIONALITE = _LeDCLIENT.NATIONALITE.LIBELLE,
                        LIBELLEPAYEUR = _LeDCLIENT.PAYEUR1.NOM,
                        LIBELLEREGCLI = _LeDCLIENT.REGROUPEMENT1.NOM,
                        LIBELLESITE = _LeDCLIENT.CENTRE1.SITE.LIBELLE,
                        LIBELLECENTRE = _LeDCLIENT.CENTRE1.LIBELLE,
                        LIBELLEUSAGE = _LeDCLIENT.USAGE.LIBELLE ,
                        LIBELLETYPEPIECE = _LeDCLIENT.PIECEIDENTITE.LIBELLE 

                    };

                    return Galatee.Tools.Utility.ListToDataTable(query);
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable RetourneDAdministration(int fk_idemande)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = null;
                    var _DADMINISTRATION= context.DADMINISTRATION_INSTITUT ;
                    query =
                    from x in _DADMINISTRATION
                    where
                      x.FK_IDDEMANDE == fk_idemande
                    select new
                    {
                        x.PK_ID,
                        x.NOMMANDATAIRE,
                        x.PRENOMMANDATAIRE,
                        x.RANGMANDATAIRE,
                        x.NOMSIGNATAIRE,
                        x.PRENOMSIGNATAIRE,
                        x.RANGSIGNATAIRE,
                        x.FK_IDDEMANDE,
                        x.NOMABON
                    };

                    return Galatee.Tools.Utility.ListToDataTable(query);
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable RetourneDSociete(int fk_idemande)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = null;
                    var _DSOCIETE = context.DSOCIETEPRIVE ;
                    query =
                    from x in _DSOCIETE
                    where
                      x.FK_IDDEMANDE == fk_idemande
                    select new
                    {
                            x. PK_ID ,
                            x.  NUMEROREGISTRECOMMERCE ,
                            x. FK_IDSTATUTJURIQUE ,
                            x.  CAPITAL ,
                            x.  IDENTIFICATIONFISCALE ,
                            x. DATECREATION ,
                            x.  SIEGE ,
                            x. FK_IDDEMANDE ,
                            x.  NOMMANDATAIRE ,
                            x.  PRENOMMANDATAIRE ,
                            x.  RANGMANDATAIRE ,
                            x.  NOMSIGNATAIRE ,
                            x.  PRENOMSIGNATAIRE ,
                            x.  RANGSIGNATAIRE ,
                            x.  NOMABON,
                            LIBELLESTATUSJURIDIQUE = x.STATUTJURIQUE.LIBELLE 
                    };

                    return Galatee.Tools.Utility.ListToDataTable(query);
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable RetourneDPersonnePhysique(int fk_idemande)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = null;
                    var _DPERSONNEPHYSIQUE = context.DPERSONNEPHYSIQUE;
                    query =
                    from x in _DPERSONNEPHYSIQUE
                    where
                      x.FK_IDDEMANDE == fk_idemande
                    select new
                    {
                        x.PK_ID,
                        x.DATENAISSANCE,
                        x.NUMEROPIECEIDENTITE,
                        x.DATEFINVALIDITE,
                        x.FK_IDDEMANDE,
                        x.FK_IDPIECEIDENTITE,
                        x.NOMABON 
                    };
                    return Galatee.Tools.Utility.ListToDataTable(query);
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable RetourneDinfoPropriotaire(int fk_idemande)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = null;
                    var _DINFOPROPIOTAIRE = context.DINFOPROPRIETAIRE ;
                    query =
                    from x in _DINFOPROPIOTAIRE
                    where
                      x.FK_IDDEMANDE == fk_idemande
                    select new
                    {
                        x.PK_ID,
                        x.NOM,
                        x.PRENOM,
                        x.DATENAISSANCE,
                        x.NUMEROPIECEIDENTITE,
                        x.DATEFINVALIDITE,
                        x.FK_IDDEMANDE,
                        x.FK_IDPIECEIDENTITE,
                        x.FK_IDNATIONNALITE,
                        x.FAX,
                        x.BOITEPOSTALE,
                        x.EMAIL,
                        x.TELEPHONEMOBILE,
                        x.TELEPHONEFIXE,
                        LIBELLEPIECE = x.PIECEIDENTITE.LIBELLE,
                        LIBELLENATIONNALITE = x.NATIONALITE.LIBELLE,
                    };
                    return Galatee.Tools.Utility.ListToDataTable(query);
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable RetourneDcanalisationEnMagazin(int fk_idemande)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = null;
                    var _DCANALISATION = context.DCANALISATION;

                    query =
                    from _LeDCANALISATION in _DCANALISATION
                    join _COMPTEUR in context.MAGASINVIRTUEL
                   on new { FK_IDCOMPTEUR = _LeDCANALISATION.FK_IDCOMPTEUR.Value } equals new { FK_IDCOMPTEUR = _COMPTEUR.PK_ID }
                    where
                       _LeDCANALISATION.FK_IDDEMANDE  ==fk_idemande 
                    select new
                    {
                        _LeDCANALISATION.CENTRE,
                        _LeDCANALISATION.CLIENT,
                        _LeDCANALISATION.NUMDEM,
                        _LeDCANALISATION.PRODUIT,
                        _LeDCANALISATION.POINT,
                        _LeDCANALISATION.BRANCHEMENT,
                        _LeDCANALISATION.SURFACTURATION,
                        _LeDCANALISATION.DEBITANNUEL,
                        //_LeDCANALISATION.POSE ,
                        //_LeDCANALISATION.DEPOSE ,
                        _LeDCANALISATION.USERCREATION,
                        _LeDCANALISATION.DATECREATION,
                        _LeDCANALISATION.DATEMODIFICATION,
                        _LeDCANALISATION.USERMODIFICATION,
                        _LeDCANALISATION.PK_ID,
                        _LeDCANALISATION.FK_IDCENTRE,
                        _LeDCANALISATION.FK_IDCOMPTEUR,
                        _LeDCANALISATION.FK_IDPRODUIT,
                        _LeDCANALISATION.FK_IDDEMANDE,

                        _LeDCANALISATION.FK_IDPROPRIETAIRE,
                        _LeDCANALISATION.PROPRIO,


                        // Compteur
                        _COMPTEUR.NUMERO,
                        TYPECOMPTEUR = _COMPTEUR.TYPECOMPTEUR.CODE,
                        //_COMPTEUR.DIAMETRE,
                        _COMPTEUR.MARQUE,
                        LIBELLEMARQUE = _COMPTEUR.MARQUECOMPTEUR.LIBELLE,
                        _COMPTEUR.COEFLECT,
                        _COMPTEUR.COEFCOMPTAGE,
                        _COMPTEUR.CADRAN,
                        _COMPTEUR.ANNEEFAB,
                        _COMPTEUR.FONCTIONNEMENT,
                        CAS = Enumere.CasPoseCompteur
                        // AKO 11/11/2015   _LeDCANALISATION.COMMENTAIRE
                    };


                    return Galatee.Tools.Utility.ListToDataTable(query);
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static DataTable RetourneDAbon(int fk_iddemande)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = null;
                    var _DABON = context.DABON;

                    query =
                    from _LeDABON in _DABON
                    where
                     _LeDABON.FK_IDDEMANDE  == fk_iddemande 
                    select new
                    {
                        _LeDABON.CENTRE,
                        _LeDABON.CLIENT,
                        _LeDABON.ORDRE,
                        _LeDABON.NUMDEM,
                        _LeDABON.PRODUIT,
                        _LeDABON.TYPETARIF,
                        _LeDABON.PUISSANCE,
                        _LeDABON.FORFAIT,
                        _LeDABON.FORFPERSO,
                        _LeDABON.AVANCE,
                        _LeDABON.DAVANCE,
                        _LeDABON.REGROU,
                        _LeDABON.PERFAC,
                        _LeDABON.MOISFAC,
                        _LeDABON.DABONNEMENT,
                        _LeDABON.DRES,
                        _LeDABON.DATERACBRT,
                        _LeDABON.NBFAC,
                        _LeDABON.PERREL,
                        _LeDABON.MOISREL,
                        _LeDABON.DMAJ,
                        _LeDABON.RECU,
                        _LeDABON.RISTOURNE,
                        _LeDABON.CONSOMMATIONMAXI,
                        _LeDABON.TYPECOMPTAGE,
                        _LeDABON.FK_IDTYPECOMPTAGE,
                        _LeDABON.ISBORNEPOSTE,
                        _LeDABON.COEFFAC,
                        _LeDABON.USERCREATION,
                        _LeDABON.DATECREATION,
                        _LeDABON.DATEMODIFICATION,
                        _LeDABON.USERMODIFICATION,
                        _LeDABON.PK_ID,
                        _LeDABON.FK_IDCENTRE,
                        _LeDABON.FK_IDPRODUIT,
                        _LeDABON.FK_IDFORFAIT,
                        _LeDABON.FK_IDMOISREL,
                        _LeDABON.FK_IDMOISFAC,
                        _LeDABON.FK_IDTYPETARIF,
                        _LeDABON.FK_IDPERIODICITEFACTURE,
                        _LeDABON.FK_IDPERIODICITERELEVE,
                        _LeDABON.FK_IDDEMANDE,
                        _LeDABON.ESTEXONERETVA,
                        _LeDABON.DEBUTEXONERATIONTVA,
                        _LeDABON.FINEXONERATIONTVA,
                        _LeDABON.ISAUGMENTATIONPUISSANCE,
                        _LeDABON.ISDIMINUTIONPUISSANCE,
                        _LeDABON.NOUVELLEPUISSANCE,
                        _LeDABON.NOMBREDEFOYER,

                        LIBELLECENTRE = _LeDABON.CENTRE1.LIBELLE,
                        LIBELLEPRODUIT = _LeDABON.PRODUIT1.LIBELLE,
                        LIBELLETARIF = _LeDABON.TYPETARIF1.LIBELLE,
                        LIBELLEFORFAIT = _LeDABON.FORFAIT1.LIBELLE,
                        LIBELLEMOISFACT = _LeDABON.MOIS.LIBELLE,
                        LIBELLEMOISIND = _LeDABON.MOIS1.LIBELLE,
                        LIBELLEFREQUENCE = _LeDABON.PERIODICITE1.LIBELLE,
                    };

                    return Galatee.Tools.Utility.ListToDataTable(query);
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static DataTable RetourneDcanalisationSeul(int fk_iddemande)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = null;
                    var _DCANALISATION = context.DCANALISATION;

                    query =
                    from _LeCANALISATION in _DCANALISATION
                    where
                    _LeCANALISATION.FK_IDDEMANDE == fk_iddemande
                    select new
                    {
                        _LeCANALISATION.CENTRE,
                        _LeCANALISATION.CLIENT,
                        _LeCANALISATION.NUMDEM,
                        _LeCANALISATION.PRODUIT,
                        _LeCANALISATION.PROPRIO,
                        _LeCANALISATION.REGLAGECOMPTEUR,
                        _LeCANALISATION.POINT,
                        _LeCANALISATION.BRANCHEMENT,
                        _LeCANALISATION.SURFACTURATION,
                        _LeCANALISATION.DEBITANNUEL,
                        _LeCANALISATION.REPERAGECOMPTEUR,
                        _LeCANALISATION.POSE,
                        _LeCANALISATION.DEPOSE,
                        _LeCANALISATION.USERCREATION,
                        _LeCANALISATION.DATECREATION,
                        _LeCANALISATION.DATEMODIFICATION,
                        _LeCANALISATION.USERMODIFICATION,
                        _LeCANALISATION.PK_ID,
                        _LeCANALISATION.FK_IDCENTRE,
                        _LeCANALISATION.FK_IDCOMPTEUR,
                        _LeCANALISATION.FK_IDPRODUIT,
                        _LeCANALISATION.FK_IDDEMANDE,
                        _LeCANALISATION.FK_IDPROPRIETAIRE,
                        _LeCANALISATION.FK_IDREGLAGECOMPTEUR,
                        _LeCANALISATION.ORDREAFFICHAGE,
                        CAS = Enumere.CasPoseCompteur
                    };
                    return Galatee.Tools.Utility.ListToDataTable(query);
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static DataTable RetourneDcanalisationMagazinVirtuel(int fk_iddemande)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = null;
                    var _DCANALISATION = context.DCANALISATION;

                    query =
                    from _LeCANALISATION in _DCANALISATION
                    where
                    _LeCANALISATION.FK_IDDEMANDE == fk_iddemande
                    select new
                    {
                        _LeCANALISATION.CENTRE,
                        _LeCANALISATION.CLIENT,
                        _LeCANALISATION.NUMDEM,
                        _LeCANALISATION.PRODUIT,
                        _LeCANALISATION.PROPRIO,
                        _LeCANALISATION.REGLAGECOMPTEUR,
                        _LeCANALISATION.POINT,
                        _LeCANALISATION.BRANCHEMENT,
                        _LeCANALISATION.SURFACTURATION,
                        _LeCANALISATION.DEBITANNUEL,
                        _LeCANALISATION.REPERAGECOMPTEUR,
                        _LeCANALISATION.POSE,
                        _LeCANALISATION.DEPOSE,
                        _LeCANALISATION.USERCREATION,
                        _LeCANALISATION.DATECREATION,
                        _LeCANALISATION.DATEMODIFICATION,
                        _LeCANALISATION.USERMODIFICATION,
                        _LeCANALISATION.PK_ID,
                        _LeCANALISATION.FK_IDCENTRE,
                        _LeCANALISATION.FK_IDCOMPTEUR,
                        _LeCANALISATION.FK_IDPRODUIT,
                        _LeCANALISATION.FK_IDDEMANDE,
                        _LeCANALISATION.FK_IDPROPRIETAIRE,
                        _LeCANALISATION.FK_IDREGLAGECOMPTEUR,
                        _LeCANALISATION.ORDREAFFICHAGE,
                        _LeCANALISATION.FK_IDMAGAZINVIRTUEL,
                         _LeCANALISATION.MAGASINVIRTUEL.NUMERO ,
                        TYPECOMPTEUR= _LeCANALISATION.MAGASINVIRTUEL.TYPECOMPTEUR.CODE   ,
                        FK_IDTYPECOMPTEUR = _LeCANALISATION.MAGASINVIRTUEL.TYPECOMPTEUR.PK_ID,
                         _LeCANALISATION.MAGASINVIRTUEL.MARQUECOMPTEUR.CODE   ,
                        LIBELLEMARQUE = _LeCANALISATION.MAGASINVIRTUEL.MARQUECOMPTEUR.LIBELLE,
                        LIBELLETYPECOMPTEUR = _LeCANALISATION.MAGASINVIRTUEL.TYPECOMPTEUR.LIBELLE,
                         _LeCANALISATION.MAGASINVIRTUEL.COEFLECT   ,
                         _LeCANALISATION.MAGASINVIRTUEL.COEFCOMPTAGE    ,
                         _LeCANALISATION.MAGASINVIRTUEL.CADRAN     ,
                         _LeCANALISATION.MAGASINVIRTUEL.ANNEEFAB      ,
                         _LeCANALISATION.MAGASINVIRTUEL.FONCTIONNEMENT       ,
                         LIBELLEREGLAGECOMPTEUR =_LeCANALISATION.REGLAGECOMPTEUR1.LIBELLE ,
                         CAS = Enumere.CasPoseCompteur 
                    };
                    return Galatee.Tools.Utility.ListToDataTable(query);
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static DataTable RetourneDcanalisationCompteur(int fk_iddemande)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = null;
                    var _DCANALISATION = context.DCANALISATION;

                    query =
                    from _LeCANALISATION in _DCANALISATION
                    where
                    _LeCANALISATION.FK_IDDEMANDE == fk_iddemande  
                    select new
                    {
                        _LeCANALISATION.NUMDEM ,
                        _LeCANALISATION.CENTRE,
                        _LeCANALISATION.CLIENT,
                        _LeCANALISATION.PRODUIT,
                        _LeCANALISATION.PROPRIO,
                        _LeCANALISATION.POINT,
                        _LeCANALISATION.BRANCHEMENT,
                        MARQUE = _LeCANALISATION.COMPTEUR.MARQUE,
                        TYPECOMPTEUR = _LeCANALISATION.COMPTEUR.TYPECOMPTEUR,
                        NUMERO = _LeCANALISATION.COMPTEUR.NUMERO,
                        TCOMPT = _LeCANALISATION.COMPTEUR.TYPECOMPTEUR1.CODE,
                        MCOMPT = _LeCANALISATION.COMPTEUR.MARQUECOMPTEUR.CODE,
                        _LeCANALISATION.REGLAGECOMPTEUR,

                        _LeCANALISATION.COMPTEUR.COEFLECT,
                        _LeCANALISATION.COMPTEUR.CADRAN,
                        _LeCANALISATION.COMPTEUR.ANNEEFAB,
                        _LeCANALISATION.SURFACTURATION,
                        _LeCANALISATION.DEBITANNUEL,
                        _LeCANALISATION.DEPOSE,
                        _LeCANALISATION.POSE,
                        _LeCANALISATION.USERCREATION,
                        _LeCANALISATION.DATECREATION,
                        _LeCANALISATION.DATEMODIFICATION,
                        _LeCANALISATION.USERMODIFICATION,
                        _LeCANALISATION.PK_ID,
                        _LeCANALISATION.FK_IDREGLAGECOMPTEUR,
                        _LeCANALISATION.COMPTEUR.FK_IDTYPECOMPTEUR,
                        _LeCANALISATION.COMPTEUR.FK_IDPRODUIT,
                        _LeCANALISATION.COMPTEUR.FK_IDMARQUECOMPTEUR,
                        _LeCANALISATION.FK_IDCENTRE,
                        _LeCANALISATION.FK_IDCOMPTEUR,
                        _LeCANALISATION.FK_IDPROPRIETAIRE,

                        _LeCANALISATION.COMPTEUR.FK_IDETATCOMPTEUR,
                        _LeCANALISATION.COMPTEUR.FK_IDSTATUTCOMPTEUR,
                        _LeCANALISATION.COMPTEUR.FK_IDCALIBRE,

                        LIBELLECENTRE = _LeCANALISATION.CENTRE1.LIBELLE,
                        LIBELLEPRODUIT = _LeCANALISATION.PRODUIT1.LIBELLE,
                        LIBELLEREGLAGECOMPTEUR = _LeCANALISATION.REGLAGECOMPTEUR1.LIBELLE,
                        LIBELLEMARQUE = _LeCANALISATION.COMPTEUR.MARQUECOMPTEUR.LIBELLE,
                        LIBELLETYPECOMPTEUR = _LeCANALISATION.COMPTEUR.TYPECOMPTEUR1.LIBELLE,
                    };
                    return Galatee.Tools.Utility.ListToDataTable(query);
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static DataTable RetourneDEvenement(int fk_iddemande)
        {
            try
            {

                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = null;
                    var _DEVENEMENT = context.DEVENEMENT;

                    query =
                    from _LeDEVENEMENT in _DEVENEMENT
                    where
                     _LeDEVENEMENT.FK_IDDEMANDE  == fk_iddemande 

                    select new
                    {
                        _LeDEVENEMENT.CENTRE,
                        _LeDEVENEMENT.CLIENT,
                        _LeDEVENEMENT.PRODUIT,
                        _LeDEVENEMENT.POINT,
                        _LeDEVENEMENT.NUMEVENEMENT,
                        _LeDEVENEMENT.ORDRE,
                        _LeDEVENEMENT.COMPTEUR,
                        _LeDEVENEMENT.DATEEVT,
                        _LeDEVENEMENT.PERIODE,
                        _LeDEVENEMENT.CODEEVT,
                        _LeDEVENEMENT.INDEXEVT,
                        _LeDEVENEMENT.CAS,
                        _LeDEVENEMENT.ENQUETE,
                        _LeDEVENEMENT.CONSO,
                        _LeDEVENEMENT.CONSONONFACTUREE,
                        _LeDEVENEMENT.LOTRI,
                        _LeDEVENEMENT.FACTURE,
                        _LeDEVENEMENT.SURFACTURATION,
                        _LeDEVENEMENT.STATUS,
                        _LeDEVENEMENT.TYPECONSO,
                        _LeDEVENEMENT.REGLAGECOMPTEUR,
                        _LeDEVENEMENT.MATRICULE,
                        _LeDEVENEMENT.FACPER,
                        _LeDEVENEMENT.QTEAREG,
                        _LeDEVENEMENT.DERPERF,
                        _LeDEVENEMENT.DERPERFN,
                        _LeDEVENEMENT.CONSOFAC,
                        _LeDEVENEMENT.REGIMPUTE,
                        _LeDEVENEMENT.REGCONSO,
                        _LeDEVENEMENT.COEFLECT,
                        _LeDEVENEMENT.COEFCOMPTAGE,
                        _LeDEVENEMENT.PUISSANCE,
                        _LeDEVENEMENT.PROPRIO ,
                        _LeDEVENEMENT.TYPECOMPTAGE,
                        _LeDEVENEMENT.TYPECOMPTEUR,
                        _LeDEVENEMENT.TYPETARIF,
                        _LeDEVENEMENT.CATEGORIE,
                        _LeDEVENEMENT.COEFK1,
                        _LeDEVENEMENT.COEFK2,
                        _LeDEVENEMENT.COEFFAC,
                        _LeDEVENEMENT.USERCREATION,
                        _LeDEVENEMENT.DATECREATION,
                        _LeDEVENEMENT.DATEMODIFICATION,
                        _LeDEVENEMENT.USERMODIFICATION,
                        _LeDEVENEMENT.PK_ID,
                        _LeDEVENEMENT.NUMDEM,
                        _LeDEVENEMENT.FK_IDABON,
                        _LeDEVENEMENT.FK_IDCANALISATION,
                        _LeDEVENEMENT.FK_IDCENTRE,
                        _LeDEVENEMENT.FK_IDPRODUIT,
                        _LeDEVENEMENT.FK_IDDEMANDE,
                        _LeDEVENEMENT.FK_IDCOMPTEUR,
                        _LeDEVENEMENT.COMMENTAIRE,
                        _LeDEVENEMENT.FK_IDTOURNEE,
                        _LeDEVENEMENT.TOURNEE,
                        _LeDEVENEMENT.ORDTOUR,
                        _LeDEVENEMENT.PERFAC,
                        _LeDEVENEMENT.CONSOMOYENNEPRECEDENTEFACTURE,
                        _LeDEVENEMENT.DATERELEVEPRECEDENTEFACTURE,
                        _LeDEVENEMENT.CASPRECEDENTEFACTURE,
                        _LeDEVENEMENT.INDEXPRECEDENTEFACTURE,
                        _LeDEVENEMENT.PERIODEPRECEDENTEFACTURE,
                        _LeDEVENEMENT.ORDREAFFICHAGE,
                        _LeDEVENEMENT.NOUVEAUCOMPTEUR,
                        _LeDEVENEMENT.PUISSANCEINSTALLEE,
                        _LeDEVENEMENT.COEFKR1,
                        _LeDEVENEMENT.COEFKR2,
                        _LeDEVENEMENT.QTEAREGPRECEDENT,
                        _LeDEVENEMENT.ISCONSOSEULE,

                    };
                    return Galatee.Tools.Utility.ListToDataTable(query);
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static DataTable RetourneDemandeDetailCout(int fk_iddemande)
        {
            try
            {

                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = null;
                    var _RUBRIQUEDEMANDE = context.RUBRIQUEDEMANDE;

                    query =
                    from x in _RUBRIQUEDEMANDE
                    where
                    x.FK_IDDEMANDE == fk_iddemande

                    select new
                    {
                        x.NUMDEM,
                        x.CENTRE,
                        x.NDOC,
                        x.REFEM,
                        x.CLIENT,
                        x.ORDRE,
                        x.COPER,
                        x.MONTANTHT,
                        x.MONTANTTAXE,
                        x.TAXE,
                        x.DATECREATION,
                        x.DATEMODIFICATION,
                        x.USERCREATION,
                        x.USERMODIFICATION,
                        x.PK_ID,
                        x.FK_IDCENTRE,
                        x.FK_IDDEMANDE,
                        x.FK_IDCOPER,
                        x.FK_IDTAXE,
                        LIBELLETAXE = x.TAXE1.LIBELLE,
                        x.COPER1.LIBELLE
                    };
                    return Galatee.Tools.Utility.ListToDataTable(query);
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable RetourneClientByReference(string RefClient, List<int> idCentre)
        {
            try
            {

                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = null;
                    var _client = context.CLIENT ;
                    query =
                    from x in _client
                    from y  in x.ABON 
                    where
                    x.REFCLIENT == RefClient && idCentre.Contains(x.FK_IDCENTRE)
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
                        x.REGROUPEMENT,
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
                        x.REGLEMENT,
                        x.DECRET,
                        x.CONVENTION,
                        x.REFERENCEATM,
                        x.PK_ID,
                        x.DATECREATION,
                        x.DATEMODIFICATION,
                        x.USERCREATION,
                        x.USERMODIFICATION,
                        x.FK_IDMODEPAIEMENT,
                        x.FK_IDAG,
                        x.FK_IDCODECONSO,
                        x.FK_IDCATEGORIE,
                        x.FK_IDRELANCE,
                        x.FK_IDNATIONALITE,
                        x.FK_IDREGROUPEMENT,
                        x.FK_IDCENTRE,
                        x.FK_TYPECLIENT,
                        x.FK_IDUSAGE,
                        x.FK_IDPROPRIETAIRE,
                        x.MODEPAIEMENT,
                        x.FAX,
                        y.DRES ,
                        y.PRODUIT ,
                        x.CODEIDENTIFICATIONNATIONALE,
                        LIBELLEMODEPAIEMENT = x.MODEPAIEMENT1.LIBELLE,
                        LIBELLECODECONSO = x.CODECONSOMMATEUR.LIBELLE,
                        LIBELLECATEGORIE = x.CATEGORIECLIENT.LIBELLE,
                        LIBELLERELANCE = x.RELANCE.LIBELLE,
                        LIBELLENATIONALITE = x.NATIONALITE.LIBELLE,
                        LIBELLEPAYEUR = x.PAYEUR1.NOM,
                        LIBELLEREGCLI = x.REGROUPEMENT1.NOM,
                        LIBELLESITE = x.CENTRE1.SITE.LIBELLE,
                        LIBELLECENTRE = x.CENTRE1.LIBELLE,
                    };
                    return Galatee.Tools.Utility.ListToDataTable(query);
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable RetourneClientByReferenceOrdre(string RefClient,string Ordre, List<int> idCentre)
        {
            try
            {

                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = null;
                    var _client = context.CLIENT;
                    query =
                    from x in _client
                    from y in x.ABON
                    where
                    x.REFCLIENT == RefClient && x.ORDRE == Ordre && idCentre.Contains(x.FK_IDCENTRE)
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
                        x.REGROUPEMENT,
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
                        x.REGLEMENT,
                        x.DECRET,
                        x.CONVENTION,
                        x.REFERENCEATM,
                        x.PK_ID,
                        x.DATECREATION,
                        x.DATEMODIFICATION,
                        x.USERCREATION,
                        x.USERMODIFICATION,
                        x.FK_IDMODEPAIEMENT,
                        x.FK_IDAG,
                        x.FK_IDCODECONSO,
                        x.FK_IDCATEGORIE,
                        x.FK_IDRELANCE,
                        x.FK_IDNATIONALITE,
                        x.FK_IDREGROUPEMENT,
                        x.FK_IDCENTRE,
                        x.FK_TYPECLIENT,
                        x.FK_IDUSAGE,
                        x.FK_IDPROPRIETAIRE,
                        x.MODEPAIEMENT,
                        x.FAX,
                        y.DRES,
                        y.PRODUIT,
                        x.CODEIDENTIFICATIONNATIONALE,
                        LIBELLEMODEPAIEMENT = x.MODEPAIEMENT1.LIBELLE,
                        LIBELLECODECONSO = x.CODECONSOMMATEUR.LIBELLE,
                        LIBELLECATEGORIE = x.CATEGORIECLIENT.LIBELLE,
                        LIBELLERELANCE = x.RELANCE.LIBELLE,
                        LIBELLENATIONALITE = x.NATIONALITE.LIBELLE,
                        LIBELLEPAYEUR = x.PAYEUR1.NOM,
                        LIBELLEREGCLI = x.REGROUPEMENT1.NOM,
                        LIBELLESITE = x.CENTRE1.SITE.LIBELLE,
                        LIBELLECENTRE = x.CENTRE1.LIBELLE,
                    };
                    return Galatee.Tools.Utility.ListToDataTable(query);
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void CreeNouveauAbonnement(CsDemande pDemande,List<LCLIENT> leCompteClient, galadbEntities pContext)
        {


            try
            {
                galadbEntities contextinter = new galadbEntities();
                DEMANDE _Demande = new DEMANDE();
                if (pDemande.LaDemande != null)
                    _Demande = Entities.ConvertObject<DEMANDE, CsDemandeBase>(pDemande.LaDemande);
                AG _LeAG = new AG();
                if (pDemande.Ag != null)
                {
                    _LeAG = Entities.ConvertObject<AG, CsAg>(pDemande.Ag);
                    _LeAG.FK_IDCENTRE = pDemande.Transfert.FK_IDCENTRETRANSFERT;
                }

                BRT _LeBrt = new BRT();
                if (pDemande.Branchement != null)
                {
                    _LeBrt = Entities.ConvertObject<BRT, CsBrt>(pDemande.Branchement);
                    _LeBrt.FK_IDCENTRE = pDemande.Transfert.FK_IDCENTRETRANSFERT;

                }

                CLIENT _LeClient = new CLIENT();
                if (pDemande.LeClient != null)
                {
                    _LeClient = Entities.ConvertObject<CLIENT, CsClient>(pDemande.LeClient);
                    #region Vider les propriété d'identification sur l'objet client
                    _LeClient.NUMEROPIECEIDENTITE = string.Empty;
                    _LeClient.FK_IDPIECEIDENTITE = null;
                    #endregion
                    _LeClient.FK_IDCENTRE = pDemande.Transfert.FK_IDCENTRETRANSFERT;
                    if (pDemande.Transfert.FK_IDREGROUPEMENT != null)
                    {
                        _LeClient.REGROUPEMENT = pDemande.Transfert.CODEREGROUPEMENT;
                        _LeClient.FK_IDREGROUPEMENT = pDemande.Transfert.FK_IDCENTRETRANSFERT;
                    }

                }
                ABON _LeAbon = new ABON();
                if (pDemande.Abonne != null)
                {
                    _LeAbon = Entities.ConvertObject<ABON, CsAbon>(pDemande.Abonne);
                    _LeAbon.FK_IDCENTRE = pDemande.Transfert.FK_IDCENTRETRANSFERT;

                }
                List<CANALISATION> _LeCompteur = new List<CANALISATION>();
                if (pDemande.LstCanalistion != null)
                {
                    _LeCompteur = Entities.ConvertObject<CANALISATION, CsCanalisation>(pDemande.LstCanalistion);
                    _LeCompteur.ForEach(t => t.PROPRIO = pDemande.LeClient.PROPRIO);
                    _LeCompteur.ForEach(t => t.FK_IDPROPRIETAIRE = contextinter.PROPRIETAIRE.FirstOrDefault(p => p.CODE == pDemande.LeClient.PROPRIO).PK_ID);
                    _LeCompteur.ForEach(t => t.FK_IDCENTRE = pDemande.Transfert.FK_IDCENTRETRANSFERT);
                    _LeCompteur.ForEach(t => t.FK_IDABON = _LeAbon.PK_ID );
                }
                contextinter.Dispose();
                List<EVENEMENT> _LstEvenement = new List<EVENEMENT>();
                if (pDemande.LstEvenement != null)
                {
                    _LstEvenement = Entities.ConvertObject<EVENEMENT, CsEvenement>(pDemande.LstEvenement);
                    _LstEvenement.ForEach(t => t.FK_IDCENTRE = pDemande.Transfert.FK_IDCENTRETRANSFERT);
                }
                Entities.InsertEntity<AG>(_LeAG, pContext);

                _LeBrt.FK_IDAG = _LeAG.PK_ID;
                Entities.InsertEntity<BRT>(_LeBrt, pContext);

                _LeClient.FK_IDAG = _LeAG.PK_ID;
                Entities.InsertEntity<CLIENT>(_LeClient, pContext);

                _LeAbon.FK_IDCLIENT = _LeClient.PK_ID;
                Entities.InsertEntity<ABON>(_LeAbon, pContext);

                leCompteClient.ForEach(t => t.FK_IDCENTRE = pDemande.Transfert.FK_IDCENTRETRANSFERT);
                leCompteClient.ForEach(t => t.FK_IDCLIENT = _LeClient.PK_ID );
                Entities.InsertEntity<LCLIENT>(leCompteClient, pContext);

                #endregion
                _LeCompteur.ForEach(t => t.FK_IDABON = _LeAbon.PK_ID);
                Entities.InsertEntity<CANALISATION>(_LeCompteur, pContext);
                foreach (EVENEMENT item in _LstEvenement)
                {
                    item.FK_IDABON = _LeAbon.PK_ID;
                    item.FK_IDCANALISATION = _LeCompteur.FirstOrDefault(t => t.POINT == item.POINT).PK_ID;
                }
                Entities.InsertEntity<EVENEMENT>(_LstEvenement, pContext);
            }

            catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            {
                // Retrieve the error messages as a list of strings.
                var errorMessages = ex.EntityValidationErrors
                        .SelectMany(x => x.ValidationErrors)
                        .Select(x => x.ErrorMessage);

                // Join the list to a single string.
                var fullErrorMessage = string.Join("; ", errorMessages);

                // Combine the original exception message with the new one.
                var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

                //return exceptionMessage;
                // Throw a new DbEntityValidationException with the improved exception message.
                throw new System.Data.Entity.Validation.DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);

            }
        }
        public static void FermerAncienAbonnement(CsDemande pDemande, galadbEntities pContext)
        {
            try
            {
                List<CANALISATION> lstCanalisation = new List<CANALISATION>();
                BRT lebrt = pContext.BRT.FirstOrDefault(t => t.FK_IDCENTRE == pDemande.Branchement.FK_IDCENTRE &&
                                                           t.CENTRE == pDemande.Branchement.CENTRE &&
                                                           t.CLIENT == pDemande.Branchement.CLIENT);
                if (lebrt != null )
                    lebrt.DRES = System.DateTime.Today ;

                ABON _LeAbonnement = pContext.ABON.FirstOrDefault(t => t.FK_IDCENTRE == pDemande.Abonne.FK_IDCENTRE && 
                                                                       t.CENTRE == pDemande.Abonne.CENTRE &&
                                                                       t.CLIENT == pDemande.Abonne.CLIENT &&
                                                                       pDemande.Abonne.ORDRE == t.ORDRE);
                    if (_LeAbonnement != null)
                    {
                        _LeAbonnement.DRES = System.DateTime.Today;
                        lstCanalisation = pContext.CANALISATION.Where(t => t.FK_IDABON == _LeAbonnement.PK_ID && t.DEPOSE == null ).ToList();
                        foreach (CANALISATION item in lstCanalisation)
                            item.DEPOSE    = System.DateTime.Today;
                    }
            }

            catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            {
                // Retrieve the error messages as a list of strings.
                var errorMessages = ex.EntityValidationErrors
                        .SelectMany(x => x.ValidationErrors)
                        .Select(x => x.ErrorMessage);

                // Join the list to a single string.
                var fullErrorMessage = string.Join("; ", errorMessages);

                // Combine the original exception message with the new one.
                var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

                //return exceptionMessage;
                // Throw a new DbEntityValidationException with the improved exception message.
                throw new System.Data.Entity.Validation.DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);

            }
        }
        public static DataTable VerifieMatriculeAgent(string MatriculeAgent)
        {
            try
            {

                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = null;
                    var _client = context.CLIENT;
                    query =
                    from x in _client
                    where
                    x.MATRICULE == MatriculeAgent
                    select new
                    {
                        x.CENTRE,
                        x.REFCLIENT,
                        x.ORDRE 
                    };
                    return Galatee.Tools.Utility.ListToDataTable(query);
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static DataTable RetourneTypeMateriel()
        {
            try
            {

                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = null;
                    var _Materiel = context.TYPEMATERIEL ;
                    query =
                    from x in _Materiel
                    select new
                    {
                        x.CODE ,
                        x.LIBELLE ,
                        x.PK_ID ,
                        CODEPRODUIT = x.PRODUIT.CODE 
                    };
                    return Galatee.Tools.Utility.ListToDataTable(query);
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #region TRANSITION
        public static DataTable RetourneLaDemande(string NumClient)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = null;
                    var _Demande = context.DEMANDE;
                    query =
                    (from x in _Demande
                     where
                     !context.DEMANDE_WORFKLOW.Any(t => t.CODE_DEMANDE_TABLETRAVAIL == x.NUMDEM) &&
                     x.STATUT == "2" && x.DCAISSE != null &&
                     (x.TYPEDEMANDE == Enumere.BranchementAbonement || x.TYPEDEMANDE ==Enumere.AbonnementSeul) &&
                     x.CLIENT == NumClient
                     select new
                     {
                         x.NUMDEM,
                         x.CENTRE,
                         x.NUMPERE,
                         x.TYPEDEMANDE,
                         x.DPRRDV,
                         x.DPRDEV,
                         x.DPREX,
                         x.DREARDV,
                         x.DREADEV,
                         x.DREAEX,
                         x.HRDVPR,
                         x.FDEM,
                         x.FREP,
                         x.NOMPERE,
                         x.NOMMERE,
                         x.MATRICULE,
                         x.STATUT,
                         x.DCAISSE,
                         x.NCAISSE,
                         x.EXDAG,
                         x.EXDBRT,
                         x.PRODUIT,
                         x.EXCL,
                         x.CLIENT,
                         x.EXCOMPT,
                         x.COMPTEUR,
                         x.EXEVT,
                         x.CTAXEG,
                         x.DATED,
                         x.REFEM,
                         x.ORDRE,
                         x.TOPEDIT,
                         x.FACTURE,
                         x.OPERATIONDIVERSE,
                         x.DATEFLAG,
                         x.USERCREATION,
                         x.DATECREATION,
                         x.DATEMODIFICATION,
                         x.USERMODIFICATION,
                         x.ETAPEDEMANDE,
                         x.PK_ID,
                         x.FK_IDCENTRE,
                         x.FK_IDCLIENT,
                         x.FK_IDADMUTILISATEUR,
                         x.FK_IDTYPEDEMANDE,
                         x.FK_IDPRODUIT,
                         x.TRANSMIS,
                         x.STATUTDEMANDE,
                         x.ANNOTATION,
                         x.FICHIERJOINT,
                         x.ISSUPPRIME,
                         x.USERSUPPRESSION,
                         x.DATESUPPRESSION,
                         x.ISEXTENSION,
                         x.ISPRESTATION,
                         x.ISFOURNITURE,
                         x.ISPOSE,
                         x.MOTIF,
                         x.FK_IDTYPECLIENT,
                         x.ISCHANGECOMPTEUR,
                         x.ISCONTROLE,
                         x.DATEFIN,
                         x.ISMUTATION,
                         x.REGLAGECOMPTEUR,
                         x.PUISSANCESOUSCRITE,
                         x.ISGRANDCOMPTE,
                         x.ISETALONNAGE,
                         x.ISMETREAFAIRE,
                         x.ISDEMANDEREJETERINIT,
                         x.TYPECOMPTAGE,
                         x.FK_IDTYPECOMPTAGE,
                         x.FK_IDPUISSANCESOUSCRITE,
                         x.FK_IDREGLAGECOMPTEUR,
                         x.ISDEVISCOMPLEMENTAIRE,
                         x.ISBONNEINITIATIVE,
                         x.ISEDM,
                         x.ISCOMMUNE,
                         x.NOMBREDEFOYER,
                         x.ISDEFINITIF,
                         x.ISPROVISOIR,
                         x.FK_IDDEMANDE,
                         x.ISDEVISHT,
                         x.ISPASSERCAISSE,
                        SITE =  x.CENTRE1.CODESITE 

                     }).OrderByDescending(u => u.DCAISSE);
                    return Galatee.Tools.Utility.ListToDataTable(query);
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        
        public static void MiseAjoursAbonBrtCreation(CsDemande pDemande, galadbEntities pContext)
        {
            try
            {
                galadbEntities contextinter = new galadbEntities();
                DEMANDE _Demande = new DEMANDE();
                List<DCANALISATION> _leCannal = new List<DCANALISATION>();
                DBRT _leBrt = new DBRT();
                if (pDemande.LaDemande != null)
                    _Demande = Entities.ConvertObject<DEMANDE, CsDemandeBase>(pDemande.LaDemande);

                List<DCANALISATION> lesComptDemande = contextinter.DCANALISATION.Where(t => t.NUMDEM  == pDemande.LaDemande.NUMDEM ).ToList();
                DBRT leBrt= contextinter.DBRT.FirstOrDefault (t => t.NUMDEM == pDemande.LaDemande.NUMDEM) ;
                DAG  lAgt= contextinter.DAG .FirstOrDefault (t => t.NUMDEM == pDemande.LaDemande.NUMDEM) ;
                DCLIENT  leClient= contextinter.DCLIENT .FirstOrDefault (t => t.NUMDEM == pDemande.LaDemande.NUMDEM) ;
                DABON leAbon = contextinter.DABON.FirstOrDefault(t => t.NUMDEM == pDemande.LaDemande.NUMDEM);

                AG _LeAG = new AG();
                if (pDemande.Ag != null)
                {
                    pDemande.Ag.CLIENT = lAgt.CLIENT;
                    pDemande.Ag.PK_ID  = lAgt.PK_ID ;
                    _LeAG = Entities.ConvertObject<AG, CsAg>(pDemande.Ag);
                    _LeAG.FK_IDTOURNEE = contextinter.TOURNEE.FirstOrDefault(t => t.FK_IDCENTRE == pDemande.Ag.FK_IDCENTRE && t.CODE == pDemande.Ag.TOURNEE).PK_ID;
                }

                BRT _LeBrt = new BRT();
                if (pDemande.Branchement != null)
                {
                    pDemande.Branchement.PK_ID = leBrt.PK_ID;
                    pDemande.Branchement.CLIENT = leBrt.CLIENT;
                    _LeBrt = Entities.ConvertObject<BRT, CsBrt>(pDemande.Branchement);
                    _LeBrt.DRAC = System.DateTime.Today;
                    _LeBrt.SERVICE = "1";
                }

                CLIENT _LeClient = new CLIENT();
                if (pDemande.LeClient != null)
                {
                    pDemande.LeClient.PK_ID = leClient.PK_ID;
                    pDemande.LeClient.REFCLIENT  = leClient.REFCLIENT ;
                    _LeClient = Entities.ConvertObject<CLIENT, CsClient>(pDemande.LeClient);
                }
                ABON _LeAbon = new ABON();
                if (pDemande.Abonne != null)
                {
                    pDemande.Abonne.PK_ID = leAbon.PK_ID;
                    pDemande.Abonne.CLIENT = leAbon.CLIENT;
                    _LeAbon = Entities.ConvertObject<ABON, CsAbon>(pDemande.Abonne);
                }

                List<COMPTEUR> lstcompteur = new List<COMPTEUR>();
                foreach (var item_ in pDemande.LstCanalistion)
                {
                    CANALISATION leCanal = new CANALISATION();
                    COMPTEUR can = new COMPTEUR();
                    can.ANNEEFAB = item_.ANNEEFAB;
                    can.CADRAN = item_.CADRAN;
                    can.COEFCOMPTAGE = item_.COEFCOMPTAGE;
                    can.COEFLECT = item_.COEFLECT;
                    can.DATECREATION = System.DateTime.Now;
                    can.DATEMODIFICATION = item_.DATEMODIFICATION;
                    can.MISEENSERVICE = System.DateTime.Today.Date;

                    can.FK_IDCALIBRE = item_.FK_IDCALIBRE;
                    can.FK_IDMARQUECOMPTEUR = item_.FK_IDMARQUECOMPTEUR.Value ;
                    can.FK_IDPRODUIT = item_.FK_IDPRODUIT;
                    can.FK_IDTYPECOMPTEUR = item_.FK_IDTYPECOMPTEUR.Value;
                    can.FK_IDSTATUTCOMPTEUR = 1;
                    can.FK_IDETATCOMPTEUR = 1;
                    can.FONCTIONNEMENT = item_.FONCTIONNEMENT;
                    can.MARQUE = item_.MARQUE;

                    can.NUMERO = item_.NUMERO;
                    can.TYPECOMPTEUR = item_.TYPECOMPTEUR;

                    can.PRODUIT = item_.PRODUIT;
                    can.USERCREATION = item_.USERCREATION;
                    can.USERMODIFICATION = item_.USERMODIFICATION;
                    lstcompteur.Add(can);
                }
                bool CompteurTrouve = false;
                string NumeroCompteur = pDemande.LstCanalistion.FirstOrDefault().NUMERO;
                COMPTEUR leCompteur = contextinter.COMPTEUR.FirstOrDefault(y => y.NUMERO == NumeroCompteur);
                if (leCompteur != null && !string.IsNullOrEmpty(leCompteur.NUMERO))
                {
                    CANALISATION laCana = contextinter.CANALISATION.FirstOrDefault(y => y.FK_IDCOMPTEUR == leCompteur.PK_ID && y.DEPOSE == null);
                    CsCanalisation l = Entities.ConvertObject<CsCanalisation, CANALISATION>(laCana);
                    if (l != null && !string.IsNullOrEmpty(l.CLIENT))
                        Entities.InsertEntity<COMPTEUR>(lstcompteur);
                    else
                    {
                        CompteurTrouve = true;
                        pDemande.LaDemande.MOTIF = "Le compteur exist déja sur la reférence " + laCana.CLIENT;
                    }
                }
                else
                    Entities.InsertEntity<COMPTEUR>(lstcompteur);

                List<CANALISATION> _LeCompteur = new List<CANALISATION>();
                if (pDemande.LstCanalistion != null)
                {
                    _LeCompteur = Entities.ConvertObject<CANALISATION, CsCanalisation>(pDemande.LstCanalistion);
                    _LeCompteur.ForEach(t => t.CLIENT = leAbon.CLIENT );
                    _LeCompteur.ForEach(y => y.PROPRIO = Enumere.LOCATAIRE );
                    _LeCompteur.ForEach(y => y.FK_IDPROPRIETAIRE = 2);
                    foreach (CsCanalisation item in pDemande.LstCanalistion)
                    {

                        CANALISATION leCan = _LeCompteur.FirstOrDefault(t => t.POINT == item.POINT);
                        if (!CompteurTrouve)
                        {
                            COMPTEUR leCompteurDuPoint = lstcompteur.FirstOrDefault(t => t.TYPECOMPTEUR == item.TYPECOMPTEUR);
                            leCan.FK_IDCOMPTEUR = leCompteurDuPoint.PK_ID;
                        }
                        else
                        {
                            COMPTEUR lesCompteur = contextinter.COMPTEUR.FirstOrDefault(y => y.NUMERO == item.NUMERO);
                            leCan.FK_IDCOMPTEUR = lesCompteur.PK_ID;
                        }
                        leCan.FK_IDABON = _LeAbon.PK_ID;
                        leCan.PK_ID = lesComptDemande.FirstOrDefault(u => u.POINT == item.POINT).PK_ID;
                    }
                }
                Entities.InsertEntity<CANALISATION>(_LeCompteur, pContext);
                List<EVENEMENT> _LstEvenement = new List<EVENEMENT>();
                if (pDemande.LstEvenement != null)
                {
                    pDemande.LstEvenement.ForEach(u => u.CLIENT = leClient.REFCLIENT);
                    _LstEvenement = Entities.ConvertObject<EVENEMENT, CsEvenement>(pDemande.LstEvenement);
                    _LstEvenement.ForEach(t => t.STATUS = 99);
                }

                List<LCLIENT> _LstFacture = new List<LCLIENT>();
                if (pDemande.LstCoutDemande != null && pDemande.LstCoutDemande.Count != 0)
                {
                    if (pDemande.LaDemande.TYPEDEMANDE != Enumere.BranchementAbonnementEp)
                    {
                        foreach (CsDemandeDetailCout item in pDemande.LstCoutDemande)
                        {
                            if (item.COPER == Enumere.CoperCAU)
                            {
                                _LeAbon.AVANCE = (item.MONTANTHT + item.MONTANTTAXE);
                                _LeAbon.DAVANCE = item.DATECREATION;
                            }
                            LCLIENT lelClient = pContext.LCLIENT.FirstOrDefault(t => t.CENTRE == item.CENTRE && t.CLIENT == item.CLIENT && t.ORDRE == item.ORDRE && t.FK_IDCENTRE == item.FK_IDCENTRE && t.NDOC == item.NDOC && t.REFEM == item.REFEM);
                            if (lelClient != null && !string.IsNullOrEmpty(lelClient.CENTRE))
                            {
                                lelClient.FK_IDCLIENT = _LeClient.PK_ID;
                                lelClient.COPER = item.COPER;
                                lelClient.FK_IDCOPER = item.FK_IDCOPER;
                                lelClient.MONTANT = (item.MONTANTHT + item.MONTANTTAXE);
                            }
                        }
                    }
                }
                if (pDemande.LaDemande.TYPEDEMANDE != Enumere.AbonnementSeul)
                {
                    Entities.InsertEntity<AG>(_LeAG, pContext);

                    _LeBrt.FK_IDAG = _LeAG.PK_ID;
                    Entities.InsertEntity<BRT>(_LeBrt, pContext);
                }
                _LeClient.FK_IDAG = _LeAG.PK_ID;
                Entities.InsertEntity<CLIENT>(_LeClient, pContext);

                _LeAbon.FK_IDCLIENT = _LeClient.PK_ID;
                Entities.InsertEntity<ABON>(_LeAbon, pContext);

            
                int nuEvt = 0;
                foreach (EVENEMENT item in _LstEvenement.OrderByDescending(t => t.CAS))
                {
                    item.NUMEVENEMENT = nuEvt;
                    item.FK_IDABON = _LeAbon.PK_ID;
                    item.FK_IDCANALISATION = _LeCompteur.FirstOrDefault(t => t.POINT == item.POINT).PK_ID;
                    item.FK_IDCOMPTEUR = lstcompteur.FirstOrDefault().PK_ID;
                }
                Entities.InsertEntity<EVENEMENT>(_LstEvenement, pContext);
            }

            catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            {
                // Retrieve the error messages as a list of strings.
                var errorMessages = ex.EntityValidationErrors
                        .SelectMany(x => x.ValidationErrors)
                        .Select(x => x.ErrorMessage);

                // Join the list to a single string.
                var fullErrorMessage = string.Join("; ", errorMessages);

                // Combine the original exception message with the new one.
                var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

                //return exceptionMessage;
                // Throw a new DbEntityValidationException with the improved exception message.
                throw new System.Data.Entity.Validation.DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);

            }
        }

        public static void MiseAjoursAbonBrtAbonnementTransition(CsDemande pDemande, galadbEntities pContext)
        {
            try
            {
                galadbEntities contextinter = new galadbEntities();
                DEMANDE _Demande = new DEMANDE();
                List<DCANALISATION> _leCannal = new List<DCANALISATION>();
                DBRT _leBrt = new DBRT();
                if (pDemande.LaDemande != null)
                    _Demande = Entities.ConvertObject<DEMANDE, CsDemandeBase>(pDemande.LaDemande);

                List<DCANALISATION> lesComptDemande = contextinter.DCANALISATION.Where(t => t.NUMDEM  == pDemande.LaDemande.NUMDEM ).ToList();
                DBRT leBrt= contextinter.DBRT.FirstOrDefault (t => t.NUMDEM == pDemande.LaDemande.NUMDEM) ;
                string Client = leBrt.CLIENT;


                AG _LeAG = new AG();
                if (pDemande.Ag != null)
                {
                    _LeAG = Entities.ConvertObject<AG, CsAg>(pDemande.Ag);
                    _LeAG.FK_IDTOURNEE = contextinter.TOURNEE.FirstOrDefault(t => t.FK_IDCENTRE == pDemande.Ag.FK_IDCENTRE && t.CODE == pDemande.Ag.TOURNEE).PK_ID;
                }

                BRT _LeBrt = new BRT();
                if (pDemande.Branchement != null)
                {
                    pDemande.Branchement.PK_ID = leBrt.PK_ID;
                    _LeBrt = Entities.ConvertObject<BRT, CsBrt>(pDemande.Branchement);
                    _LeBrt.DRAC = System.DateTime.Today;
                    _LeBrt.SERVICE = "1";
                }

                CLIENT _LeClient = new CLIENT();
                if (pDemande.LeClient != null)
                    _LeClient = Entities.ConvertObject<CLIENT, CsClient>(pDemande.LeClient);

                ABON _LeAbon = new ABON();
                if (pDemande.Abonne != null)
                    _LeAbon = Entities.ConvertObject<ABON, CsAbon>(pDemande.Abonne);

                List<COMPTEUR> lstcompteur = new List<COMPTEUR>();
                foreach (var item_ in pDemande.LstCanalistion)
                {
                    CANALISATION leCanal = new CANALISATION();
                    COMPTEUR can = new COMPTEUR();
                    can.ANNEEFAB = item_.ANNEEFAB;
                    can.CADRAN = item_.CADRAN;
                    can.COEFCOMPTAGE = item_.COEFCOMPTAGE;
                    can.COEFLECT = item_.COEFLECT;
                    can.DATECREATION = System.DateTime.Now;
                    can.DATEMODIFICATION = item_.DATEMODIFICATION;
                    can.MISEENSERVICE = System.DateTime.Today.Date;

                    can.FK_IDCALIBRE = item_.FK_IDCALIBRE;
                    can.FK_IDMARQUECOMPTEUR = item_.FK_IDMARQUECOMPTEUR.Value ;
                    can.FK_IDPRODUIT = item_.FK_IDPRODUIT;
                    can.FK_IDTYPECOMPTEUR = item_.FK_IDTYPECOMPTEUR.Value;
                    can.FK_IDSTATUTCOMPTEUR = 1;
                    can.FK_IDETATCOMPTEUR = 1;
                    can.FONCTIONNEMENT = item_.FONCTIONNEMENT;
                    can.MARQUE = item_.MARQUE;

                    can.NUMERO = item_.NUMERO;
                    can.TYPECOMPTEUR = item_.TYPECOMPTEUR;

                    can.PRODUIT = item_.PRODUIT;
                    can.USERCREATION = item_.USERCREATION;
                    can.USERMODIFICATION = item_.USERMODIFICATION;
                    lstcompteur.Add(can);
                }
                Entities.InsertEntity<COMPTEUR>(lstcompteur);
                List<CANALISATION> _LeCompteur = new List<CANALISATION>();
                if (pDemande.LstCanalistion != null)
                {
                    _LeCompteur = Entities.ConvertObject<CANALISATION, CsCanalisation>(pDemande.LstCanalistion);
                    _LeCompteur.ForEach(t => t.PROPRIO = pDemande.LeClient.PROPRIO);
                    _LeCompteur.ForEach(t => t.FK_IDPROPRIETAIRE = pContext.PROPRIETAIRE.FirstOrDefault(p => p.CODE == pDemande.LeClient.PROPRIO).PK_ID);
                    foreach (CsCanalisation item in pDemande.LstCanalistion)
                    {
                        CANALISATION leCan = _LeCompteur.FirstOrDefault(t => t.POINT == item.POINT);
                        COMPTEUR leCompteurDuPoint = lstcompteur.FirstOrDefault(t => t.TYPECOMPTEUR == item.TYPECOMPTEUR);
                        leCan.FK_IDCOMPTEUR = leCompteurDuPoint.PK_ID;
                        leCan.FK_IDABON = _LeAbon.PK_ID;
                        leCan.PK_ID = lesComptDemande.FirstOrDefault(u => u.POINT == item.POINT).PK_ID;
                    }
                }
                Entities.InsertEntity<CANALISATION>(_LeCompteur, pContext);
                List<EVENEMENT> _LstEvenement = new List<EVENEMENT>();
                if (pDemande.LstEvenement != null)
                    _LstEvenement = Entities.ConvertObject<EVENEMENT, CsEvenement>(pDemande.LstEvenement);
                _LstEvenement.ForEach(t => t.STATUS = 99);

                List<LCLIENT> _LstFacture = new List<LCLIENT>();
                if (pDemande.LstCoutDemande != null && pDemande.LstCoutDemande.Count != 0)
                {
                    if (pDemande.LaDemande.TYPEDEMANDE != Enumere.BranchementAbonnementEp)
                    {
                        foreach (CsDemandeDetailCout item in pDemande.LstCoutDemande)
                        {
                            if (item.COPER == Enumere.CoperCAU)
                            {
                                _LeAbon.AVANCE = (item.MONTANTHT + item.MONTANTTAXE);
                                _LeAbon.DAVANCE = item.DATECREATION;
                            }
                            LCLIENT lelClient = pContext.LCLIENT.FirstOrDefault(t => t.CENTRE == item.CENTRE && t.CLIENT == item.CLIENT && t.ORDRE == item.ORDRE && t.FK_IDCENTRE == item.FK_IDCENTRE && t.NDOC == item.NDOC && t.REFEM == item.REFEM);
                            if (lelClient != null && !string.IsNullOrEmpty(lelClient.CENTRE))
                            {
                                lelClient.FK_IDCLIENT = _LeClient.PK_ID;
                                lelClient.COPER = item.COPER;
                                lelClient.FK_IDCOPER = item.FK_IDCOPER;
                                lelClient.MONTANT = (item.MONTANTHT + item.MONTANTTAXE);
                            }
                        }
                    }
                }
                if (pDemande.LaDemande.TYPEDEMANDE != Enumere.AbonnementSeul)
                {
                    Entities.InsertEntity<AG>(_LeAG, pContext);

                    _LeBrt.FK_IDAG = _LeAG.PK_ID;
                    Entities.InsertEntity<BRT>(_LeBrt, pContext);
                }
                _LeClient.FK_IDAG = _LeAG.PK_ID;
                Entities.InsertEntity<CLIENT>(_LeClient, pContext);

                _LeAbon.FK_IDCLIENT = _LeClient.PK_ID;
                Entities.InsertEntity<ABON>(_LeAbon, pContext);

            
                int nuEvt = 0;
                foreach (EVENEMENT item in _LstEvenement.OrderByDescending(t => t.CAS))
                {
                    item.NUMEVENEMENT = nuEvt;
                    item.FK_IDABON = _LeAbon.PK_ID;
                    item.FK_IDCANALISATION = _LeCompteur.FirstOrDefault(t => t.POINT == item.POINT).PK_ID;
                    item.FK_IDCOMPTEUR = lstcompteur.FirstOrDefault().PK_ID;
                }
                Entities.InsertEntity<EVENEMENT>(_LstEvenement, pContext);
            }

            catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            {
                // Retrieve the error messages as a list of strings.
                var errorMessages = ex.EntityValidationErrors
                        .SelectMany(x => x.ValidationErrors)
                        .Select(x => x.ErrorMessage);

                // Join the list to a single string.
                var fullErrorMessage = string.Join("; ", errorMessages);

                // Combine the original exception message with the new one.
                var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

                //return exceptionMessage;
                // Throw a new DbEntityValidationException with the improved exception message.
                throw new System.Data.Entity.Validation.DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);

            }
        }
        public static DataTable GetDemandeByTypdeDemande(string TypeDemande)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                    from P in context.DEMANDE
                    orderby
                      P.NUMDEM descending

                    where P.TYPEDEMANDE == TypeDemande
                    select new
                    {
                        P.MOTIF,
                        P.NUMDEM,
                        P.CENTRE,
                        P.NUMPERE,
                        P.TYPEDEMANDE,
                        P.DPRRDV,
                        P.DPRDEV,
                        P.DPREX,
                        P.DREARDV,
                        P.DREADEV,
                        P.DREAEX,
                        P.HRDVPR,
                        P.FDEM,
                        P.FREP,
                        P.NOMPERE,
                        P.NOMMERE,
                        P.MATRICULE,
                        P.STATUT,
                        P.DCAISSE,
                        P.NCAISSE,
                        P.EXDAG,
                        P.EXDBRT,
                        P.PRODUIT,
                        P.EXCL,
                        P.CLIENT,
                        P.EXCOMPT,
                        P.COMPTEUR,
                        P.EXEVT,
                        P.CTAXEG,
                        P.DATED,
                        P.REFEM,
                        P.ORDRE,
                        P.TOPEDIT,
                        P.FACTURE,
                        P.DATEFLAG,
                        P.TRANSMIS,
                        P.STATUTDEMANDE,
                        P.FICHIERJOINT,
                        P.ANNOTATION,
                        P.USERCREATION,
                        P.DATECREATION,
                        P.DATEMODIFICATION,
                        P.USERMODIFICATION,
                        P.ETAPEDEMANDE,
                        P.ISSUPPRIME,
                        P.USERSUPPRESSION,
                        P.DATESUPPRESSION,
                        P.PK_ID,
                        P.FK_IDCENTRE,
                        P.FK_IDCLIENT,
                        P.FK_IDADMUTILISATEUR,
                        P.FK_IDTYPEDEMANDE,
                        P.FK_IDPRODUIT,
                        INITIERPAR = context.ADMUTILISATEUR.FirstOrDefault(t => t.MATRICULE == P.USERCREATION) != null ? context.ADMUTILISATEUR.FirstOrDefault(t => t.MATRICULE == P.USERCREATION).LIBELLE : string.Empty,
                        LIBELLECENTRE = P.CENTRE1.LIBELLE,
                        LIBELLETYPEDEMANDE = P.TYPEDEMANDE1.LIBELLE,
                        LIBELLEPRODUIT = P.PRODUIT1.LIBELLE,
                        LIBELLESITE = P.CENTRE1.SITE.LIBELLE,
                        P.ISCHANGECOMPTEUR,
                        P.ISCONTROLE,
                        P.ISEXTENSION,
                        P.ISPRESTATION,
                        P.ISMUTATION,
                        P.ISPOSE,
                        P.ISETALONNAGE,
                        P.REGLAGECOMPTEUR,
                        P.PUISSANCESOUSCRITE,
                        P.DATEFIN,
                        P.ISMETREAFAIRE,
                        P.TYPECOMPTAGE,
                        P.FK_IDPUISSANCESOUSCRITE,
                        P.FK_IDREGLAGECOMPTEUR,
                        P.FK_IDTYPECOMPTAGE,
                        P.ISDEVISCOMPLEMENTAIRE,
                        P.ISBONNEINITIATIVE,
                        P.ISCOMMUNE,
                        P.ISEDM,
                        P.NOMBREDEFOYER,
                        P.ISDEFINITIF,
                        P.ISPROVISOIR,
                        P.FK_IDDEMANDE,
                        P.ISPASSERCAISSE,
                        P.ISDEVISHT,
                        CODEREGLAGECOMPTEUR = P.REGLAGECOMPTEUR1.REGLAGE,
                        SITE =  P.CENTRE1.CODESITE 


                    };
                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public static bool ? InsererCompteurEvtTransition(CsDemande pDemande, galadbEntities pContext)
        {
            try
            {
                galadbEntities contextinter = new galadbEntities();
                DEMANDE _Demande = new DEMANDE();
                if (pDemande.LaDemande != null)
                    _Demande = Entities.ConvertObject<DEMANDE, CsDemandeBase>(pDemande.LaDemande);

                int IdAbon = 0;
                CsCanalisation leCanals =pDemande.LstCanalistion.First();
                List<DCANALISATION> lesComptDemande = contextinter.DCANALISATION.Where(t => t.NUMDEM  == pDemande.LaDemande.NUMDEM ).ToList();
                ABON _LeAbonnement = pContext.ABON.FirstOrDefault(t => t.FK_IDCENTRE == pDemande.LaDemande.FK_IDCENTRE && t.CENTRE == pDemande.LaDemande.CENTRE && t.CLIENT == pDemande.LaDemande.CLIENT && pDemande.LaDemande.ORDRE == t.ORDRE);
                if (_LeAbonnement != null)
                    IdAbon = _LeAbonnement.PK_ID;
                List<COMPTEUR> lstcompteur = new List<COMPTEUR>();

                List<CANALISATION> lstCannalisation = pContext.CANALISATION.Where(t => t.FK_IDCENTRE == pDemande.LaDemande.FK_IDCENTRE && t.CENTRE == pDemande.LaDemande.CENTRE &&
                                                                                       t.CLIENT == pDemande.LaDemande.CLIENT && t.DEPOSE == null).ToList();
                if (lstCannalisation != null && lstCannalisation.Count != 0)
                {
                    DateTime? DateDepose  = pDemande.LstEvenement.FirstOrDefault(t=>t.CAS == Enumere.CasDeposeCompteur).DATEEVT ;
                    lstCannalisation.ForEach(o => o.DEPOSE = DateDepose);
                 }
                if (contextinter.COMPTEUR.FirstOrDefault(t => t.NUMERO == leCanals.NUMERO && t.MARQUE == leCanals.MARQUE) != null) return null ;
                foreach (var item_ in pDemande.LstCanalistion)
                {
                    CANALISATION leCanal = new CANALISATION();
                    COMPTEUR can = new COMPTEUR();
                    can.ANNEEFAB = item_.ANNEEFAB;
                    can.CADRAN = item_.CADRAN;
                    can.COEFCOMPTAGE = item_.COEFCOMPTAGE;
                    can.COEFLECT = item_.COEFLECT;
                    can.DATECREATION = System.DateTime.Now;
                    can.DATEMODIFICATION = item_.DATEMODIFICATION;
                    can.MISEENSERVICE = System.DateTime.Today.Date;

                    can.FK_IDCALIBRE = item_.FK_IDCALIBRE;
                    can.FK_IDMARQUECOMPTEUR = item_.FK_IDMARQUECOMPTEUR.Value;
                    can.FK_IDPRODUIT = item_.FK_IDPRODUIT;
                    can.FK_IDTYPECOMPTEUR = item_.FK_IDTYPECOMPTEUR.Value;
                    can.FK_IDSTATUTCOMPTEUR = 1;
                    can.FK_IDETATCOMPTEUR = 1;
                    can.FONCTIONNEMENT = item_.FONCTIONNEMENT;
                    can.MARQUE = item_.MARQUE;

                    can.NUMERO = item_.NUMERO;
                    can.TYPECOMPTEUR = item_.TYPECOMPTEUR;

                    can.PRODUIT = item_.PRODUIT;
                    can.USERCREATION = item_.USERCREATION;
                    can.USERMODIFICATION = item_.USERMODIFICATION;
                    lstcompteur.Add(can);
                }

                List<CANALISATION> _LeCompteur = new List<CANALISATION>();
                if (pDemande.LstCanalistion != null)
                    _LeCompteur = Entities.ConvertObject<CANALISATION, CsCanalisation>(pDemande.LstCanalistion);

                List<EVENEMENT> _LstEvenement = new List<EVENEMENT>();
                if (pDemande.LstEvenement != null)
                    _LstEvenement = Entities.ConvertObject<EVENEMENT, CsEvenement>(pDemande.LstEvenement);
                _LstEvenement.Where(u=>u.CAS ==Enumere.CasPoseCompteur ).ToList().ForEach(t => t.STATUS = 99);


                Entities.InsertEntity<COMPTEUR>(lstcompteur);

                foreach (CsCanalisation item in pDemande.LstCanalistion)
                {
                    CANALISATION leCan = _LeCompteur.FirstOrDefault(t => t.POINT == item.POINT);
                    DCANALISATION leDcan = lesComptDemande.FirstOrDefault(t => t.POINT == item.POINT);
                    if (leDcan != null && !string.IsNullOrEmpty(leDcan.CLIENT ))
                        leCan.PK_ID = leDcan.PK_ID;
                    COMPTEUR leCompteurDuPoint = lstcompteur.FirstOrDefault(t => t.TYPECOMPTEUR == item.TYPECOMPTEUR);
                    leCan.FK_IDCOMPTEUR = leCompteurDuPoint.PK_ID;
                    leCan.FK_IDABON = IdAbon;
                }
                Entities.InsertEntity<CANALISATION>(_LeCompteur, pContext);

                int nuEvt = 0;
                List<CsEvenement> lsEvtInsere = new List<CsEvenement>();
                foreach (EVENEMENT item in _LstEvenement.OrderByDescending(t => t.CAS))
                {
                        List<EVENEMENT> lesVtc = contextinter.EVENEMENT.Where(t => t.FK_IDCENTRE == item.FK_IDCENTRE &&
                                                                                      t.CENTRE == item.CENTRE &&
                                                                                      t.CLIENT == item.CLIENT &&
                                                                                      t.ORDRE == item.ORDRE &&
                                                                                      t.POINT == item.POINT).ToList();
                        if (lesVtc != null && lesVtc.Count != 0)
                            nuEvt = lesVtc.Max(t => t.NUMEVENEMENT);

                        if (lsEvtInsere.FirstOrDefault(i => i.POINT == item.POINT) != null)
                            nuEvt = lsEvtInsere.FirstOrDefault(i => i.POINT == item.POINT).NUMEVENEMENT;

                    nuEvt = nuEvt + 1;
                    item.NUMEVENEMENT = nuEvt;
                    item.FK_IDABON = IdAbon;
                    item.FK_IDCANALISATION = _LeCompteur.FirstOrDefault(t => t.POINT == item.POINT).PK_ID;
                    item.FK_IDCOMPTEUR = lstcompteur.FirstOrDefault().PK_ID;

                    CANALISATION leCan = _LeCompteur.FirstOrDefault(t => t.POINT == item.POINT);
                    COMPTEUR leCompteurDuPoint = lstcompteur.FirstOrDefault(t => t.TYPECOMPTEUR == item.TYPECOMPTEUR);
                    item.FK_IDCOMPTEUR = leCompteurDuPoint.PK_ID;

                    lsEvtInsere.Add(new CsEvenement() {POINT = item.POINT ,NUMEVENEMENT = nuEvt  });
                }
                Entities.InsertEntity<EVENEMENT>(_LstEvenement, pContext);
                pContext.SaveChanges();
                return true;
            }

            catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            {
                // Retrieve the error messages as a list of strings.
                var errorMessages = ex.EntityValidationErrors
                        .SelectMany(x => x.ValidationErrors)
                        .Select(x => x.ErrorMessage);

                // Join the list to a single string.
                var fullErrorMessage = string.Join("; ", errorMessages);

                // Combine the original exception message with the new one.
                var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

                //return exceptionMessage;
                // Throw a new DbEntityValidationException with the improved exception message.
                throw new System.Data.Entity.Validation.DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
                return false;

            }
        }

        #endregion
    }
}
