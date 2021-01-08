using Galatee.Silverlight.ServiceFacturation;
using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using System.Collections;
namespace Galatee.Silverlight.Facturation
{
    public static class ClasseMethodeGenerique
    {
        public static List<CsLotri> RetourneDistinctLotri(List<CsLotri> _LstLot)
        {
            try
            {
                List<CsLotri> _lstLotriDistinct = new List<CsLotri>();
                var ListLotriTemp = (from p in _LstLot
                                     group new { p } by new { p.NUMLOTRI, p.PERIODE  } into pResult
                                     select new
                                     {
                                         pResult.Key.NUMLOTRI,
                                         pResult.Key.PERIODE,
                                     });
                foreach (var item in ListLotriTemp)
                {
                    CsLotri leLot = new CsLotri()
                    {
                        NUMLOTRI = item.NUMLOTRI,
                        PERIODE = item.PERIODE,
                    };
                    _lstLotriDistinct.Add(leLot);
                }
                return _lstLotriDistinct;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public static List<CsLotri> RetourneDistinctTournee(List<CsLotri> lstLori)
        {
            try
            {
                List<CsLotri> _lstLotriDistinct = new List<CsLotri>();
                var ListLotriTemp = (from p in lstLori
                                     group new { p } by new { p.FK_IDCENTRE, p.NUMLOTRI, p.CENTRE, p.TOURNEE, p.FK_IDTOURNEE } into pResult
                                     select new
                                     {
                                         pResult.Key.NUMLOTRI,
                                         pResult.Key.CENTRE,
                                         pResult.Key.TOURNEE,
                                         pResult.Key.FK_IDCENTRE,
                                         pResult.Key.FK_IDTOURNEE
                                     });
                foreach (var item in ListLotriTemp)
                {
                    CsLotri leLot = new CsLotri()
                    {
                        CENTRE = item.CENTRE,
                        NUMLOTRI = item.NUMLOTRI,
                        TOURNEE = item.TOURNEE,
                        FK_IDCENTRE = item.FK_IDCENTRE,
                        FK_IDTOURNEE = item.FK_IDTOURNEE
                    };
                    _lstLotriDistinct.Add(leLot);
                }
                return _lstLotriDistinct;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        public static List<CsLotri> RetourneDistinctPeriode(List<CsLotri> lstLori)
        {
            try
            {
                List<CsLotri> _lstLotriDistinct = new List<CsLotri>();
                var ListLotriTemp = (from p in lstLori
                                     group new { p } by new { p.PERIODE  } into pResult
                                     select new
                                     {
                                         pResult.Key.PERIODE 
                                     });
                foreach (var item in ListLotriTemp)
                {
                    CsLotri leLot = new CsLotri()
                    {
                        PERIODE = item.PERIODE 
                    };
                    _lstLotriDistinct.Add(leLot);
                }
                return _lstLotriDistinct;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        
        public static List<CsLotri> RetourneDistinctCentreFromCentre(List<ServiceAccueil.CsCentre> _LstCentre)
        {
            try
            {
                List<CsLotri> _lstCentreLotriDistinct = new List<CsLotri>();
                var ListCentreTemp = (from p in _LstCentre
                                     group new { p } by new { p.CODE , p.PK_ID ,p.LIBELLE  } into pResult
                                     select new
                                     {
                                         pResult.Key.CODE ,
                                         pResult.Key.PK_ID ,
                                         pResult.Key.LIBELLE 
                                     });
                foreach (var item in ListCentreTemp)
                {
                    CsLotri leCentreLot = new CsLotri()
                    {
                        CENTRE = item.CODE,
                        FK_IDCENTRE = item.PK_ID ,
                        LIBELLECENTRE = item.LIBELLE 
                    };
                    _lstCentreLotriDistinct.Add(leCentreLot);
                }
                return _lstCentreLotriDistinct;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public static List<CsLotri> RetourneDistinctCentreOnly(List<CsLotri> _LstLot)
        {
            try
            {
                List<CsLotri> _lstCentreLotriDistinct = new List<CsLotri>();
                var ListLotriTemp = (from p in _LstLot
                                     group new { p } by new { p.CENTRE, p.FK_IDCENTRE,p.LIBELLECENTRE } into pResult
                                     select new
                                     {
                                         pResult.Key.CENTRE,
                                         pResult.Key.FK_IDCENTRE,
                                         pResult.Key.LIBELLECENTRE
                                     });
                foreach (var item in ListLotriTemp)
                {
                    CsLotri leCentreLot = new CsLotri()
                    {
                        CENTRE = item.CENTRE,
                        FK_IDCENTRE = item.FK_IDCENTRE,
                        LIBELLECENTRE = item.LIBELLECENTRE
                    };
                    _lstCentreLotriDistinct.Add(leCentreLot);
                }
                return _lstCentreLotriDistinct;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public static List<CsLotri> RetourneDistinctCentre(List<CsLotri> _LstLot)
        {
            try
            {
                List<CsLotri> _lstCentreLotriDistinct = new List<CsLotri>();
                var ListLotriTemp = (from p in _LstLot
                                     group new { p } by new {p.NUMLOTRI , p.CENTRE, p.FK_IDCENTRE, p.PERIODE, p.LIBELLECENTRE } into pResult
                                     select new
                                     {
                                         pResult.Key.CENTRE,
                                         pResult.Key.FK_IDCENTRE,
                                         pResult.Key.PERIODE,
                                         pResult.Key.NUMLOTRI ,
                                         pResult.Key.LIBELLECENTRE
                                     });
                foreach (var item in ListLotriTemp)
                {
                    CsLotri leCentreLot = new CsLotri()
                    {
                        CENTRE = item.CENTRE,
                        FK_IDCENTRE = item.FK_IDCENTRE,
                        PERIODE = item.PERIODE,
                        NUMLOTRI = item.NUMLOTRI ,
                        LIBELLECENTRE = item.LIBELLECENTRE
                    };
                    _lstCentreLotriDistinct.Add(leCentreLot);
                }
                return _lstCentreLotriDistinct;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public static List<CsLotri> RetourneDistinctLotriMiseAJour(List<CsLotri> _LstLot)
        {
            try
            {
                List<CsLotri> _lstLotriDistinct = new List<CsLotri>();
                var ListLotriTemp = (from p in _LstLot
                                     group new { p } by new { p.NUMLOTRI, p.CENTRE, p.PERIODE, p.JET, p.NOMUSER,p.FK_IDCENTRE,p.USERCREATION  } into pResult
                                     select new
                                     {
                                         pResult.Key.NUMLOTRI,
                                         pResult.Key.CENTRE,
                                         pResult.Key.PERIODE,
                                         pResult.Key.JET,
                                         pResult.Key.NOMUSER,
                                         pResult.Key.FK_IDCENTRE ,
                                         pResult.Key.USERCREATION  
                                     });
                foreach (var item in ListLotriTemp)
                {
                    CsLotri leLot = new CsLotri()
                    {
                        CENTRE = item.CENTRE,
                        NUMLOTRI = item.NUMLOTRI,
                        PERIODE = item.PERIODE,
                        NOMUSER = item.NOMUSER ,
                        FK_IDCENTRE = item.FK_IDCENTRE ,
                        USERCREATION = item.USERCREATION ,
                        JET = item.JET 
                    };
                    _lstLotriDistinct.Add(leLot);
                }
                return _lstLotriDistinct;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public static List<CsLotri> DistinctLotriPeriodeProduit(List<CsLotri> lstLotInit)
        {
            List<CsLotri> lstLotResult = new List<CsLotri>();
            var lstLotDistinct = lstLotInit.Select(t => new { t.NUMLOTRI, t.PERIODE,t.PRODUIT,t.USERCREATION   }).Distinct().ToList();
            foreach (var item in lstLotDistinct)
                lstLotResult.Add(new CsLotri { NUMLOTRI = item.NUMLOTRI, PERIODE = item.PERIODE,PRODUIT = item.PRODUIT,USERCREATION=item.USERCREATION   });

            return lstLotResult;

        }
        public static List<CsLotri> DistinctLotriJet(List<CsLotri> lstLotInit)
        {
            List<CsLotri> lstLotResult = new List<CsLotri>();
            var lstLotDistinct = lstLotInit.Select(t => new { t.NUMLOTRI,t.PERIODE,t.JET   }).Distinct().ToList();
            foreach (var item in lstLotDistinct)
                lstLotResult.Add(new CsLotri { NUMLOTRI = item.NUMLOTRI,PERIODE = item.PERIODE,JET =item.JET  });

            return lstLotResult;

        }
        public static List<CsLotri> DistinctLotri(List<CsLotri> lstLotInit)
        {
            List<CsLotri> lstLotResult = new List<CsLotri>();
            var lstLotDistinct = lstLotInit.Select(t => new { t.NUMLOTRI, t.PERIODE }).Distinct().ToList();
            foreach (var item in lstLotDistinct)
                lstLotResult.Add(new CsLotri { NUMLOTRI = item.NUMLOTRI, PERIODE = item.PERIODE });

            return lstLotResult;

        }
        public static List<CsLotri> DistinctLotriJetProduit(List<CsLotri> lstLotInit)
        {
            List<CsLotri> lstLotResult = new List<CsLotri>();
            var lstLotDistinct = lstLotInit.Select(t => new { t.NUMLOTRI, t.PERIODE,t.JET,t.PRODUIT   }).Distinct().ToList();
            foreach (var item in lstLotDistinct)
                lstLotResult.Add(new CsLotri { NUMLOTRI = item.NUMLOTRI, PERIODE = item.PERIODE,JET = item.JET ,PRODUIT = item.PRODUIT  });

            return lstLotResult;

        }
        public static List<CsLotri> DistinctJetProduit(List<CsLotri> lstLotInit)
        {
            List<CsLotri> lstLotResult = new List<CsLotri>();
            var lstLotDistinct = lstLotInit.Select(t => new { t.JET, t.PRODUIT }).Distinct().ToList();
            foreach (var item in lstLotDistinct)
                lstLotResult.Add(new CsLotri {JET = item.JET,PRODUIT = item.PRODUIT  });

            return lstLotResult;

        }
        public static List<CsLotri> DistinctLotriJetMiseJour(List<CsLotri> lstLotInit)
        {
            List<CsLotri> lstLotResult = new List<CsLotri>();
            var lstLotDistinct = lstLotInit.Select(t => new {t.CENTRE ,t.FK_IDCENTRE, t.NUMLOTRI, t.JET,t.NOMUSER  }).Distinct().ToList();
            foreach (var item in lstLotDistinct)
                lstLotResult.Add(new CsLotri {CENTRE = item.CENTRE ,FK_IDCENTRE = item.FK_IDCENTRE , NUMLOTRI = item.NUMLOTRI, JET = item.JET,NOMUSER = item.NOMUSER  });

            return lstLotResult;

        }

        public static bool IsLotIsole(string Numlot)
        { 
            string FactureIsoleIndex = "00002";
            string FactureResiliationIndex = "00001";
            string FactureAnnulatinIndex = "00003";

            if (new string[] { FactureIsoleIndex, FactureIsoleIndex, FactureResiliationIndex }.Contains(Numlot.Substring(SessionObject.Enumere.TailleCentre, (SessionObject.Enumere.TailleNumeroBatch - SessionObject.Enumere.TailleCentre))))
                return true;
            else return false;
        
        }
        public static List<CsLotri> RetourneDistinctNumLot(List<CsLotri> lstLori)
        {
            try
            {
                List<CsLotri> _lstLotriDistinct = new List<CsLotri>();
                var ListLotriTemp = (from p in lstLori
                                     group new { p } by new { p.FK_IDCENTRE, p.LIBELLECENTRE, p.NUMLOTRI, p.CENTRE, p.PERIODE, p.TOURNEE, p.FK_IDTOURNEE, p.NOMUSER } into pResult
                                     select new
                                     {
                                         pResult.Key.NUMLOTRI,
                                         pResult.Key.CENTRE,
                                         pResult.Key.PERIODE,
                                         pResult.Key.TOURNEE,
                                         pResult.Key.NOMUSER,
                                         pResult.Key.FK_IDCENTRE,
                                         pResult.Key.FK_IDTOURNEE,
                                         pResult.Key.LIBELLECENTRE
                                     });
                foreach (var item in ListLotriTemp)
                {
                    CsLotri leLot = new CsLotri()
                    {
                        CENTRE = item.CENTRE,
                        NUMLOTRI = item.NUMLOTRI,
                        PERIODE = item.PERIODE,
                        TOURNEE = item.TOURNEE,
                        NOMUSER = item.NOMUSER,
                        FK_IDTOURNEE = item.FK_IDTOURNEE,
                        FK_IDCENTRE = item.FK_IDCENTRE,
                        LIBELLECENTRE = item.LIBELLECENTRE
                    };
                    _lstLotriDistinct.Add(leLot);
                }
                return _lstLotriDistinct;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public static List<CsProduit> ConvertInProduitFactObject(List<ServiceAccueil.CsProduit> lstObjAccueil)
        {
            try
            {
                List<CsProduit> lstProduitIndex = new List<CsProduit>();
                foreach (ServiceAccueil.CsProduit item in lstObjAccueil)
                {
                    CsProduit leProduit = new CsProduit();
                    leProduit.CODE = item.CODE;
                    leProduit.LIBELLE = item.LIBELLE;
                    leProduit.PK_ID = item.PK_ID;
                    leProduit.GESTIONTRANSFO = item.GESTIONTRANSFO;
                    leProduit.MODESAISIE = item.MODESAISIE;
                    leProduit.OriginalCODE = item.OriginalCODE;
                    leProduit.DATECREATION = item.DATECREATION;
                    leProduit.DATEMODIFICATION = item.DATEMODIFICATION;
                    leProduit.USERCREATION = item.USERCREATION;
                    leProduit.USERMODIFICATION = item.USERMODIFICATION;
                    leProduit.IsNewRow = item.IsNewRow;
                    leProduit.IsSelect = item.IsSelect;
                    leProduit.FK_IDCENTRE = item.FK_IDCENTRE;
                    lstProduitIndex.Add(leProduit);
                }
                return lstProduitIndex;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public static List<CsCentre> ConvertInCentreFactObject(List<ServiceAccueil.CsCentre> lstObjAccueil)
        {
            try
            {
                List<CsCentre> lstCentreIndex = new List<CsCentre>();
                foreach (ServiceAccueil.CsCentre item in lstObjAccueil)
                {
                    CsCentre leCentre = new CsCentre();
                    leCentre.CODE = item.CODE;
                    leCentre.LIBELLE = item.LIBELLE;
                    leCentre.TYPECENTRE = item.TYPECENTRE;
                    leCentre.CODESITE = item.CODESITE;
                    leCentre.ADRESSE = item.ADRESSE;
                    leCentre.TELRENSEIGNEMENT = item.TELRENSEIGNEMENT;
                    leCentre.TELDEPANNAGE = item.TELDEPANNAGE;
                    leCentre.NUMEROAUTOCLIENT = item.NUMEROAUTOCLIENT;
                    leCentre.GESTIONAUTOAVANCECONSO = item.GESTIONAUTOAVANCECONSO;
                    leCentre.GESTIONAUTOFRAIS = item.GESTIONAUTOFRAIS;
                    leCentre.NUMEROFACTUREPARCLIENT = item.NUMEROFACTUREPARCLIENT;
                    leCentre.DATECREATION = item.DATECREATION;
                    leCentre.DATEMODIFICATION = item.DATEMODIFICATION;
                    leCentre.USERCREATION = item.USERCREATION;
                    leCentre.USERMODIFICATION = item.USERMODIFICATION;
                    leCentre.PK_ID = item.PK_ID;
                    leCentre.FK_IDCODESITE = item.FK_IDCODESITE;
                    leCentre.FK_IDTYPECENTRE = item.FK_IDTYPECENTRE;
                    leCentre.FK_IDNIVEAUTARIF = item.FK_IDNIVEAUTARIF;
                    leCentre.IsSelect = item.IsSelect;
                    leCentre.LIBELLESITE = item.LIBELLESITE;
                    leCentre.LIBELLETYPECENTRE = item.LIBELLETYPECENTRE;
                    leCentre.CODENIVEAUTARIF   = item.CODENIVEAUTARIF;
                    leCentre.NUMDEM = item.NUMDEM;
                    leCentre.NUMERODEMANDE = item.NUMERODEMANDE;
                    lstCentreIndex.Add(leCentre);
                }
                return lstCentreIndex;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public static List<CsCategorieClient> ConvertInCategFactObject(List<ServiceAccueil.CsCategorieClient> lstObjAccueil)
        {
            try
            {
                List<CsCategorieClient> lstCategFac= new List<CsCategorieClient>();
                foreach (ServiceAccueil.CsCategorieClient item in lstObjAccueil)
                {
                    CsCategorieClient leCateg = new CsCategorieClient();
                    leCateg.PK_ID = item.PK_ID;
                    leCateg.CODE = item.CODE;
                    leCateg.LIBELLE = item.LIBELLE;
                    lstCategFac.Add(leCateg);
                }
                return lstCategFac;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public static List<CsFrequence> ConvertInFrequenceFactObject(List<ServiceAccueil.CsFrequence> lstObjAccueil)
        {
            try
            {
                List<CsFrequence> lstFequeceFac = new List<CsFrequence>();
                foreach (ServiceAccueil.CsFrequence item in lstObjAccueil)
                {
                    CsFrequence leFreq = new CsFrequence();
                    leFreq.PK_ID = item.PK_ID;
                    leFreq.CODE = item.CODE;
                    leFreq.LIBELLE = item.LIBELLE;
                    lstFequeceFac.Add(leFreq);
                }
                return lstFequeceFac;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public static List<CsTournee> ConvertInTourneeFactObject(List<ServiceAccueil.CsTournee > lstObjAccueil)
        {
            try
            {
                List<CsTournee> lstTourneeFac = new List<CsTournee>();
                foreach (ServiceAccueil.CsTournee item in lstObjAccueil)
                {
                    CsTournee leTourne = new CsTournee();
                    leTourne.PK_ID = item.PK_ID;
                    leTourne.CODE = item.CODE;
                    leTourne.LIBELLE = item.LIBELLE;
                    leTourne.FK_IDCENTRE = item.FK_IDCENTRE ;
                    leTourne.CENTRE = item.CENTRE ;
                    leTourne.FK_IDADMUTILISATEUR = item.FK_IDADMUTILISATEUR;
                    lstTourneeFac.Add(leTourne);
                }
                return lstTourneeFac;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public static bool IsDateValide(string _DateSaisie)
        {
            try
            {
                DateTime date;
                if (!DateTime.TryParse(_DateSaisie, out date))
                {
                    return false;
                }
                else return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static List<CsFactureClient> RetourneDistinctClientFacture(List<CsFactureClient> _LstFacture)
        {
            try
            {
                List<CsFactureClient> lstClientResult = new List<CsFactureClient>();
                var lstClientDistinct = _LstFacture.Select(t => new { t.Centre, t.Client, t.Ordre }).Distinct().ToList();
                foreach (var item in lstClientDistinct)
                    lstClientResult.Add(new CsFactureClient { Centre = item.Centre, Client = item.Client, Ordre = item.Ordre });
                return lstClientResult;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        public static bool IsLotIsole(CsLotri leLot)
        {
            string FactureIsoleIndex = "00002";
            string FactureResiliationIndex = "00001";
            string FactureAnnulatinIndex = "00003";
            if (new string[] { FactureIsoleIndex, FactureResiliationIndex, FactureAnnulatinIndex }.Contains(leLot.NUMLOTRI.Substring(SessionObject.Enumere.TailleCentre, (SessionObject.Enumere.TailleNumeroBatch - SessionObject.Enumere.TailleCentre))))
                return true;
            else
                return false;
        }

        public static List<CsLotri> LissLotIsoleUer(List<CsLotri> leLot)
        {
            return leLot.Where(t => (t.USERCREATION == UserConnecte.matricule) && IsLotIsole(t)).ToList();
        }
    }
}
