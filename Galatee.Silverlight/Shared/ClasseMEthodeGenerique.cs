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
using Galatee.Silverlight.ServiceAccueil;
//using Galatee.Silverlight.serviceWeb;
using System.Collections.Generic;
using System.Reflection;
using System.Collections;
namespace Galatee.Silverlight.Shared
{
    static class ClasseMEthodeGenerique
    {
        public delegate void MethodeDeCallBack<T>(T param);


        public static List<string> RetourneListeDesMoisComptable()
        {
            List<string> LstMoisCompt = new List<string>();
            LstMoisCompt.Add(System.DateTime.Now.Month.ToString("00") + System.DateTime.Now.Year.ToString());

            return LstMoisCompt;
        }
        public static bool IsFormatPeriodeValide(string periode)
        {
            if (periode.Length == 7 && periode.Substring(2, 1) == "/")
                return true;
            else return false;
        }
        public static string FormatPeriodeMMAAAA(string periode)
        {
            if (periode.Length == 6)
                return periode.Substring(4, 2).PadLeft(2, '0') + "/" + periode.Substring(0, 4);
            else return string.Empty;
        }
        public static string ReformatePeriode(string Periode)
        {
            return (Periode.Substring(3, 4) + Periode.Substring(0, 2));
        }
        public static List<CsLotri> RetourneDistinctLotri(List<CsLotri> _LstLot)
        {
            try
            {
                List<CsLotri> _lstLotriDistinct = new List<CsLotri>();
                var ListLotriTemp = (from p in _LstLot
                                     group new { p } by new { p.NUMLOTRI, p.CENTRE, p.PERIODE } into pResult
                                     select new
                                     {
                                         pResult.Key.NUMLOTRI,
                                         pResult.Key.CENTRE,
                                         pResult.Key.PERIODE
                                     });
                foreach (var item in ListLotriTemp)
                {
                    CsLotri leLot = new CsLotri()
                    {
                        CENTRE = item.CENTRE,
                        NUMLOTRI = item.NUMLOTRI,
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

        public static List<CsLotri> RetourneDistinctTournee(List<CsLotri> lstLori)
        {
            try
            {
                List<CsLotri> _lstLotriDistinct = new List<CsLotri>();
                var ListLotriTemp = (from p in lstLori
                                     group new { p } by new { p.NUMLOTRI, p.CENTRE, p.TOURNEE } into pResult
                                     select new
                                     {
                                         pResult.Key.NUMLOTRI,
                                         pResult.Key.CENTRE,
                                         pResult.Key.TOURNEE
                                     });
                foreach (var item in ListLotriTemp)
                {
                    CsLotri leLot = new CsLotri()
                    {
                        CENTRE = item.CENTRE,
                        NUMLOTRI = item.NUMLOTRI,
                        TOURNEE = item.TOURNEE
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
        public static string FormatPeriodeAAAAMM(string periode)
        {
            if (periode.Length == 7)
                return periode.Substring(3, 4) + periode.Substring(0, 2);
            else return string.Empty;
        }
        public static string DernierJourDuMois(int mois, int annee)
        {
            try
            {
                DateTime now = DateTime.Now;
                int nbDays = DateTime.DaysInMonth(annee, mois);
                return new DateTime(annee, mois, nbDays, 23, 59, 59, 999).ToShortDateString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static CsEvenement DernierEvenement(List<CsEvenement> ListEvt, string produit)
        {
            try
            {
                CsEvenement _LeDernierEvt = new CsEvenement();
                return _LeDernierEvt = ListEvt.FirstOrDefault(p => p.NUMEVENEMENT == ListEvt.Max(t => t.NUMEVENEMENT) && p.PRODUIT == produit);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static CsEvenement DernierEvenementFacture(List<CsEvenement> ListEvt, string produit)
        {
            try
            {
                CsEvenement _LeDernierEvtFact = new CsEvenement();
                List<CsEvenement> _LstDernierEvtFact = ListEvt.Where(p => p.PRODUIT == produit &&
                                             (p.STATUS == SessionObject.Enumere.EvenementFacture ||
                                              p.STATUS == SessionObject.Enumere.EvenementMisAJour ||
                                              p.STATUS == SessionObject.Enumere.EvenementPurger)).ToList();
                return _LeDernierEvtFact = _LstDernierEvtFact.FirstOrDefault(p => p.NUMEVENEMENT == (_LstDernierEvtFact.Max(t => t.NUMEVENEMENT)));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static List<object> RetourneListeObjet<T>(List<T> _LaListe) where T : new()
        {
            try
            {
                List<object> _ListObject = new List<object>();
                foreach (object item in _LaListe)
                    _ListObject.Add(item);
                return _ListObject;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static  T RetourneObjectFromList<T>(List<T> _LaListe, string _valeurSelect, string _precherche) where T : new()
        {
            try
            {
                T ObjetRetourne = new T();
                foreach (T item in _LaListe)
                {
                    // Recuperation des types
                    PropertyInfo[] properties1 = item.GetType().GetProperties();

                    // Test de l'unicité des deux types
                    // Remplacement des valeurs
                    for (int attrNum = 0; attrNum < properties1.Length; attrNum++)
                    {
                        if (properties1[attrNum].Name.Equals(_precherche))
                        {
                            object value2 = properties1[attrNum].GetValue(item, null);
                            if (value2.ToString() == _valeurSelect)
                                ObjetRetourne = item;
                        }
                    }
                }
                return ObjetRetourne;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public static bool IsObjetInList<T>(List<T> _LaListe, string _valeurSelect, string _precherche) 
        {
            try
            {
                bool Istrouve = false ;
                foreach (T item in _LaListe)
                {
                    // Recuperation des types
                    PropertyInfo[] properties1 = item.GetType().GetProperties();

                    // Test de l'unicité des deux types
                    // Remplacement des valeurs
                    for (int attrNum = 0; attrNum < properties1.Length; attrNum++)
                    {
                        if (properties1[attrNum].Name.Equals(_precherche))
                        {
                            object value2 = properties1[attrNum].GetValue(item, null);
                            if (value2.ToString() == _valeurSelect)
                                Istrouve = true;
                        }
                    }
                }
                return Istrouve;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public static void IsChampObligatoireSaisie(TextBox leTextbox)
        {
            try
            {
                Brush _laCouleur = leTextbox.Background;
                if (leTextbox.Background != new SolidColorBrush(Colors.Transparent) &&
                    !string.IsNullOrEmpty(leTextbox.Text))
                    leTextbox.Background = new SolidColorBrush(Colors.Transparent);
                else
                    leTextbox.Background = new SolidColorBrush(Color.FromArgb(100, 173, 216, 230));
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public static bool IsDateSaisieValide(DateTime? DateFin, DateTime? DateDebut)
        {
            if (DateFin > DateDebut)
                return true;
            else return false;
        }
        public static List<T> RetourneListCopy<T>(List<T> _LaListe) where T : new()
        {
            try
            {
                List<T> results = new List<T>();
                foreach (T item in _LaListe)
                {
                    T obj = new T();
                    var properties11 = obj.GetType().GetProperties();
                    var properties = item.GetType().GetProperties();
                    foreach (var f in properties) // Récuperation des valeurs des proprietes de l'objet
                    {
                        PropertyInfo res = properties11.FirstOrDefault(p => p.Name.ToUpper() == f.Name.ToUpper());
                        if (res != null)
                        {
                            var o = f.GetValue(item , null);
                            f.SetValue(obj, o, null);
                        }
                    }
                    results.Add(obj);
                }

                return results.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<Galatee.Silverlight.ServiceFacturation.CsLotri> RegroupementdeLot(List<Galatee.Silverlight.ServiceFacturation.CsLotri> _LstLot)
        {
            try
            {
                // La grid doit afficher le detail d un recu par mode de reglement
                var LotRegrp = (from p in _LstLot
                                group new { p } by new { p.CENTRE, p.NUMLOTRI, p.PERIODE ,p.FK_IDCENTRE ,p.PRODUIT } into pResult
                                select new
                                {
                                    pResult.Key.CENTRE,
                                    pResult.Key.NUMLOTRI,
                                    pResult.Key.PERIODE,
                                    pResult.Key.FK_IDCENTRE ,
                                    pResult.Key.PRODUIT 
                                });

                List<Galatee.Silverlight.ServiceFacturation.CsLotri> _LstLotAffiche = new List<Galatee.Silverlight.ServiceFacturation.CsLotri>();

                foreach (var r in LotRegrp.OrderByDescending(p => p.NUMLOTRI))
                {
                    Galatee.Silverlight.ServiceFacturation.CsLotri _leLot = new Galatee.Silverlight.ServiceFacturation.CsLotri();
                    _leLot.CENTRE = r.CENTRE;
                    _leLot.NUMLOTRI = r.NUMLOTRI;
                    _leLot.PERIODE = r.PERIODE;
                    _leLot.FK_IDCENTRE = r.FK_IDCENTRE;
                    _leLot.PRODUIT  = r.PRODUIT ;
                    _LstLotAffiche.Add(_leLot);
                }
                return _LstLotAffiche;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        
        public static List<Galatee.Silverlight.ServiceFacturation.CsLotri> RegroupementdeJet(List<Galatee.Silverlight.ServiceFacturation.CsLotri> _LstLot)
        {
            try
            {
                // La grid doit afficher le detail d un recu par mode de reglement
                var LotRegrp = (from p in _LstLot
                                group new { p } by new { p.CENTRE, p.NUMLOTRI, p.JET ,p.FK_IDCENTRE,p.PRODUIT,p.USERCREATION ,p.USERMODIFICATION    } into pResult
                                select new
                                {
                                    pResult.Key.CENTRE,
                                    pResult.Key.NUMLOTRI,
                                    pResult.Key.PRODUIT ,
                                    pResult.Key.JET,
                                    pResult.Key.FK_IDCENTRE,
                                    pResult.Key.USERCREATION,
                                    pResult.Key.USERMODIFICATION,
                                });

                List<Galatee.Silverlight.ServiceFacturation.CsLotri> _LstLotAffiche = new List<Galatee.Silverlight.ServiceFacturation.CsLotri>();

                foreach (var r in LotRegrp.OrderByDescending(p => p.NUMLOTRI))
                {
                    Galatee.Silverlight.ServiceFacturation.CsLotri _leLot = new Galatee.Silverlight.ServiceFacturation.CsLotri();
                    _leLot.CENTRE = r.CENTRE;
                    _leLot.NUMLOTRI = r.NUMLOTRI;
                    _leLot.JET = r.JET;
                    _leLot.PRODUIT  = r.PRODUIT ;
                    _leLot.FK_IDCENTRE = r.FK_IDCENTRE;
                    _leLot.USERCREATION = r.USERCREATION;
                    _leLot.USERMODIFICATION = r.USERMODIFICATION;
                    _LstLotAffiche.Add(_leLot);
                }
                return _LstLotAffiche;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static List<Galatee.Silverlight.ServiceAccueil.CsSite> RetourneSiteByCentre(List<Galatee.Silverlight.ServiceAccueil.CsCentre> _lstCentre)
        {
            try
            {
                // La grid doit afficher le detail d un recu par mode de reglement
                var leCentres = (from p in _lstCentre
                                group new { p } by new { p.CODESITE , p.FK_IDCODESITE , p.LIBELLESITE  } into pResult
                                select new
                                {
                                    pResult.Key.CODESITE,
                                    pResult.Key.FK_IDCODESITE,
                                    pResult.Key.LIBELLESITE
                                });

                List<Galatee.Silverlight.ServiceAccueil.CsSite> _LstSite = new List<Galatee.Silverlight.ServiceAccueil.CsSite>();

                foreach (var r in leCentres.OrderByDescending(p => p.CODESITE ))
                {
                    Galatee.Silverlight.ServiceAccueil.CsSite _leSite = new Galatee.Silverlight.ServiceAccueil.CsSite();
                    _leSite.CODE = r.CODESITE;
                    _leSite.PK_ID  = r.FK_IDCODESITE;
                    _leSite.LIBELLE  = r.LIBELLESITE;
                    _LstSite.Add(_leSite);
                }
                return _LstSite;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public static T RetourneCopyObjet<T>(T objectInit) where T : new()
        {
            try
            {
                T results = new T();
              
                    T obj = new T();
                    var properties11 = obj.GetType().GetProperties();
                    var properties = objectInit.GetType().GetProperties();
                    foreach (var f in properties) // Récuperation des valeurs des proprietes de l'objet
                    {
                        PropertyInfo res = properties11.FirstOrDefault(p => p.Name.ToUpper() == f.Name.ToUpper());
                        if (res != null)
                        {
                            var o = f.GetValue(objectInit, null);
                            f.SetValue(obj, o, null);
                        }
                    }
                    results= obj;

                return results;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static  bool IsDateValide(string _DateSaisie)
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

        public static  DateTime? IsDateValider(string _DateSaisie)
        {
            DateTime date;
            if (!DateTime.TryParse(_DateSaisie, out date))
                return null;
            else return date;
        }

        public static List<Galatee.Silverlight.ServiceCaisse.CsLclient> DistinctModePaiement(List<Galatee.Silverlight.ServiceCaisse.CsLclient> _lstReglement)
        {
            try
            {
                // La grid doit afficher le detail d un recu par mode de reglement
                var LotRegrp = (from p in _lstReglement
                                group new { p } by new { p.MODEREG , p.LIBELLEMODREG  } into pResult
                                select new
                                {
                                    pResult.Key.MODEREG,
                                    pResult.Key.LIBELLEMODREG
                                });

                List<Galatee.Silverlight.ServiceCaisse.CsLclient> _LstModeReglement = new List<Galatee.Silverlight.ServiceCaisse.CsLclient>();

                foreach (var r in LotRegrp.OrderByDescending(p => p.MODEREG ))
                {
                    Galatee.Silverlight.ServiceCaisse.CsLclient _leLot = new Galatee.Silverlight.ServiceCaisse.CsLclient();
                    _leLot.MODEREG  = r.MODEREG ;
                    _leLot.LIBELLEMODREG  = r.LIBELLEMODREG ;
                    _LstModeReglement.Add(_leLot);
                }
                return _LstModeReglement;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public static List<ServiceAccueil.CsCentre> RetourCentreByPerimetreTotal(List<ServiceAccueil.CsCentre> lesCentre, List<Galatee.Silverlight.ServiceAuthenInitialize.CsProfil> lstProfil)
        {
            try
            {
                List<ServiceAccueil.CsCentre> lstCentreDuModule = new List<ServiceAccueil.CsCentre>();
                foreach (Galatee.Silverlight.ServiceAuthenInitialize.CsProfil item in lstProfil)
                {
                    foreach (Galatee.Silverlight.ServiceAuthenInitialize.CsCentreProfil items in item.LESCENTRESPROFIL)
                        lstCentreDuModule.Add(lesCentre.FirstOrDefault(t => t.PK_ID == items.FK_IDCENTRE));
                }
                return lstCentreDuModule;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        public static List<ServiceAccueil.CsCentre> RetourCentreByPerimetre(List<ServiceAccueil.CsCentre> lesCentre, List<Galatee.Silverlight.ServiceAuthenInitialize.CsProfil> lstProfil)
        {
            try
            {
                List<ServiceAccueil.CsCentre> lstCentreDuModule = new List<ServiceAccueil.CsCentre>();
                Galatee.Silverlight.ServiceAuthenInitialize.CsProfil leProfil = lstProfil.FirstOrDefault(t => t.MODULE == SessionObject.ModuleEnCours);

                List<int> lstCentreDist = new List<int>();
                var lstCentreDistnct = leProfil.LESCENTRESPROFIL.Select(t => new { t.FK_IDCENTRE}).Distinct().ToList();
                lstCentreDist.AddRange(lstCentreDistnct.Select(p => p.FK_IDCENTRE).ToList());

                lstCentreDuModule = lesCentre.Where(t => lstCentreDist.Contains(t.PK_ID)).ToList();
                return lstCentreDuModule;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }


        public static List<ServiceAdministration.CsUtilisateur> RetourUserByPerimetre(List<ServiceAdministration.CsUtilisateur> lesUsers, List<Galatee.Silverlight.ServiceAuthenInitialize.CsProfil> lstProfil)
        {
            try
            {
                List<ServiceAdministration.CsUtilisateur> lstUsersDuModule = new List<ServiceAdministration.CsUtilisateur>();
                Galatee.Silverlight.ServiceAuthenInitialize.CsProfil leProfil = lstProfil.FirstOrDefault(t => t.MODULE == SessionObject.ModuleEnCours);

                List<int> lstUsersDist = new List<int>();
                var lstUsersDistnct = leProfil.LESCENTRESPROFIL.Select(t => new { t.FK_IDADMUTILISATEUR }).Distinct().ToList();
                foreach (var item in lstUsersDistnct)
                    lstUsersDist.Add(item.FK_IDADMUTILISATEUR);

                lstUsersDuModule = lesUsers.Where(t => lstUsersDist.Contains(t.PK_ID)).ToList();
                return lstUsersDuModule;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }


        public static List<ServiceAccueil.CsSite> RetourneSiteByCentre(List<ServiceAccueil.CsCentre> lstCentreGlobal, List<ServiceAccueil.CsCentre> lstCentrePerimetre)
        {
            try
            {
                List<int> lstIdCentrePerimetre = new List<int>();
                foreach (ServiceAccueil.CsCentre item in lstCentrePerimetre)
                    lstIdCentrePerimetre.Add(item.PK_ID);

                List<ServiceAccueil.CsSite> lstSiteDuModule = new List<ServiceAccueil.CsSite>();
                var lesCentre = lstCentreGlobal.Where(t => lstIdCentrePerimetre.Contains(t.PK_ID)).Select(l => new { l.FK_IDCODESITE, l.CODESITE, l.LIBELLESITE }).Distinct().ToList();
                foreach (var item in lesCentre)
                    lstSiteDuModule.Add(new ServiceAccueil.CsSite { PK_ID = item.FK_IDCODESITE, CODE = item.CODESITE, LIBELLE = item.LIBELLESITE });
                return lstSiteDuModule;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public static ServiceAccueil.CParametre RetourneClasseValue<T>(T _Classe, List<string> _lesColone) where T : new()
        {
            try
            {
                ServiceAccueil.CParametre LavaleurColonne  =new ServiceAccueil.CParametre();
                // Recuperation des types
                PropertyInfo[] properties1 = _Classe.GetType().GetProperties();
                for (int attrNum = 0; attrNum < properties1.Length; attrNum++)
                {
                    if (_lesColone.Contains(properties1[attrNum].Name.ToUpper()))
                    {
                        object value2 = properties1[attrNum].GetValue(_Classe, null);
                        if (properties1[attrNum].Name.ToUpper() == "CODE")
                          LavaleurColonne.CODE = value2.ToString();
                        else if (properties1[attrNum].Name.ToUpper() == "LIBELLE")
                        {
                            if (value2 != null )
                            LavaleurColonne.LIBELLE = value2.ToString();
                        }
                        else if (properties1[attrNum].Name.ToUpper() == "PK_ID")
                            LavaleurColonne.PK_ID = int.Parse(value2.ToString());
                        else if (properties1[attrNum].Name.ToUpper() == "ISSELECT")
                            LavaleurColonne.IsSelect = Convert.ToBoolean(value2);
                    }
                }
                return LavaleurColonne;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public static List<ServiceAccueil.CParametre> RetourneValueFromClasse<T>(List<T> _Classe) where T : new()
        {
            try
            {
                List<string> _lesColone = new List<string>();
                _lesColone.Add("CODE");
                _lesColone.Add("LIBELLE");
                _lesColone.Add("PK_ID");
                _lesColone.Add("ISSELECT");
                List<ServiceAccueil.CParametre> ListParametre = new List<CParametre>();
                foreach (T item in _Classe)
                 ListParametre.Add(RetourneClasseValue<T>(item,_lesColone)) ;  
                return ListParametre;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        public static List<ServiceAdministration.CsCentre> ConvertTypeCentre(List<ServiceAccueil.CsCentre> lesCentre)
        {
            try
            {
                List<ServiceAdministration.CsCentre> lesCentres = new List<ServiceAdministration.CsCentre>();
                foreach (ServiceAccueil.CsCentre item in lesCentre)
                {
                    ServiceAdministration.CsCentre leCentres = new ServiceAdministration.CsCentre();
                    leCentres.CODE = item.CODE;
                    leCentres.LIBELLE = item.LIBELLE;
                    leCentres.TYPECENTRE = item.TYPECENTRE;
                    leCentres.CODESITE = item.CODESITE;
                    leCentres.ADRESSE = item.ADRESSE;
                    leCentres.TELRENSEIGNEMENT = item.TELRENSEIGNEMENT;
                    leCentres.TELDEPANNAGE = item.TELDEPANNAGE;
                    leCentres.NUMEROAUTOCLIENT = item.NUMEROAUTOCLIENT;
                    leCentres.GESTIONAUTOAVANCECONSO = item.GESTIONAUTOAVANCECONSO;
                    leCentres.GESTIONAUTOFRAIS = item.GESTIONAUTOFRAIS;
                    leCentres.NUMEROFACTUREPARCLIENT = item.NUMEROFACTUREPARCLIENT;
                    leCentres.DATECREATION = item.DATECREATION;
                    leCentres.DATEMODIFICATION = item.DATEMODIFICATION;
                    leCentres.USERCREATION = item.USERCREATION;
                    leCentres.USERMODIFICATION = item.USERMODIFICATION;
                    leCentres.PK_ID = item.PK_ID;
                    leCentres.FK_IDCODESITE = item.FK_IDCODESITE;
                    leCentres.FK_IDTYPECENTRE = item.FK_IDTYPECENTRE;
                    leCentres.FK_IDNIVEAUTARIF = item.FK_IDNIVEAUTARIF;
                    leCentres.IsSelect = item.IsSelect;
                    leCentres.LIBELLESITE = item.LIBELLESITE;
                    leCentres.LIBELLETYPECENTRE = item.LIBELLETYPECENTRE;
                    leCentres.CODENIVEAUTARIF = item.CODENIVEAUTARIF;
                    leCentres.NUMDEM = item.NUMDEM;
                    leCentres.NUMERODEMANDE = item.NUMERODEMANDE;
                    lesCentres.Add(leCentres);

                }
                return lesCentres;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public static List<ServiceAdministration.CsSite> ConvertTypeSite(List<ServiceAccueil.CsCentre> lesCentre)
        {
            try
            {
                List<ServiceAccueil.CsSite> lesSite = new List<CsSite>();
                var lstCentreDistnct = lesCentre.Select(t => new { t.FK_IDCODESITE   , t.CODESITE  , t.LIBELLESITE }).Distinct().ToList();
                List<ServiceAdministration.CsSite> LesSite= new List<ServiceAdministration.CsSite>();
                foreach (var item in lstCentreDistnct)
                {
                    ServiceAdministration.CsSite leSite= new ServiceAdministration.CsSite();
                    leSite.PK_ID = item.FK_IDCODESITE;
                    leSite.CODE = item.CODESITE ;
                    leSite.LIBELLE = item.LIBELLESITE;
                    LesSite.Add(leSite);
                }
                return LesSite;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public static void InitWOrkflow(string IdDemandeCree, int centreDemandeCree, string NumeroDemandeTableTravail)
        {
            //Ajouté par WCO le 04/08/2015
            //Pour l'insertion d'une demande directement dans ma table
            Workflow.WorkflowDmdManager managerWkf = new Workflow.WorkflowDmdManager();
            managerWkf.InsertionDemandeWorkflowComplete += (str_rsl) =>
            {
                if (str_rsl.StartsWith("ERR"))
                {
                    Message.ShowError(new Exception(str_rsl), "Création d'une demande");
                }
                else
                {
                    Message.ShowInformation("La demande a été créée avec succès. Numéro de votre demande : " + NumeroDemandeTableTravail,
                        Silverlight.Resources.Devis.Languages.txtDevis);
                }
            };
            managerWkf.InsererMaDemande(IdDemandeCree, "Galatee.Silverlight.Devis.UcInitialisation", centreDemandeCree,
                NumeroDemandeTableTravail);
        }

        //WCO - WOrkflow le 28/10/2015 a 19h43
        public static void InitWOrkflow(string IdDemandeCree, int centreDemandeCree, string FrmInitialisation, string NumeroDemandeTableTravail)
        {
            //Ajouté par WCO le 04/08/2015
            //Pour l'insertion d'une demande directement dans ma table
            Workflow.WorkflowDmdManager managerWkf = new Workflow.WorkflowDmdManager();
            managerWkf.InsertionDemandeWorkflowComplete += (str_rsl) =>
            {
                if (str_rsl.StartsWith("ERR"))
                {
                    Message.ShowError(new Exception(str_rsl), "Création d'une demande");
                }
                else
                {
                    Message.ShowInformation("La demande a été créée avec succès. Numéro de votre demande : " + NumeroDemandeTableTravail,
                        Silverlight.Resources.Devis.Languages.txtDevis);
                }
            };
            managerWkf.InsererMaDemande(IdDemandeCree, FrmInitialisation, centreDemandeCree, NumeroDemandeTableTravail);
        }

        //BSY - WOrkflow le 24/09/2016 a 00h12
        public static void InitWOrkflow(string IdDemandeCree, int centreDemandeCree, string FrmInitialisation, string NumeroDemandeTableTravail, MethodeDeCallBack<dynamic> handler, dynamic Pamatre)
        {
            //Ajouté par WCO le 04/08/2015
            //Pour l'insertion d'une demande directement dans ma table
            Workflow.WorkflowDmdManager managerWkf = new Workflow.WorkflowDmdManager();
            managerWkf.InsertionDemandeWorkflowComplete += (str_rsl) =>
            {
                if (str_rsl.StartsWith("ERR"))
                {
                    Message.ShowError(new Exception(str_rsl), "Création d'une demande");
                }
                else
                {
                    handler(Pamatre);
                    Message.ShowInformation("La demande a été créée avec succès. Numéro de votre demande : " + NumeroDemandeTableTravail,
                        Silverlight.Resources.Devis.Languages.txtDevis);
                }
            };
            managerWkf.InsererMaDemande(IdDemandeCree, FrmInitialisation, centreDemandeCree, NumeroDemandeTableTravail);
        }


        public static void InitWOrkflowToGroupValidation(string IdDemandeCree, int centreDemandeCree, string NumeroDemandeTableTravail,Guid LeIdGroupeValidation, int codeTdem)
        {
            try
            {
                //Ajouté par WCO le 04/08/2015
                //Pour l'insertion d'une demande directement dans ma table
                Workflow.WorkflowDmdManager managerWkf = new Workflow.WorkflowDmdManager();
                managerWkf.InsertionDemandeWorkflowComplete += (str_rsl) =>
                {
                    if (str_rsl.StartsWith("ERR"))
                    {
                        Message.ShowError(new Exception(str_rsl), "Création d'une demande");
                    }
                    else
                    {
                        Message.ShowInformation("La demande a été créée avec succès. Numéro de votre demande : " + NumeroDemandeTableTravail,
                            Silverlight.Resources.Devis.Languages.txtDevis);

                        CsTdem leTydemande = SessionObject.LstTypeDemande.FirstOrDefault(t => t.CODE == SessionObject.Enumere.DemandeReclamation);
                        if (leTydemande != null && leTydemande.PK_ID == codeTdem)
                            MiseAJourGroupSurCopieCircuit(IdDemandeCree, NumeroDemandeTableTravail, LeIdGroupeValidation);

                        //RetourneInformationDemande(int.Parse(IdDemandeCree));
                    }
                };
                managerWkf.InsererMaDemandeToGroupeDeValidation(IdDemandeCree, codeTdem, centreDemandeCree,LeIdGroupeValidation, NumeroDemandeTableTravail);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }





        private static void MiseAJourGroupSurCopieCircuit(string idDemande, string numdem, Guid idGroup)
        {

            Galatee.Silverlight.ServiceParametrage.ParametrageClient service = new Galatee.Silverlight.ServiceParametrage.ParametrageClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Parametrage"));

            service.UpdateGroupValidationCopieCircuitCompleted += (wkf, wsen) =>
            {
                if (null != wsen && wsen.Cancelled)
                {
                    Message.ShowError("Echec ", "Echec à la connexion au serveur");
                    return;
                }
                if (!wsen.Result)
                {
                    Message.ShowError("Echec ", "Echec mise à jour du groupe de validation sur le circuit de la demande.");
                    return;
                }
                else
                {

                }
            };
            service.UpdateGroupValidationCopieCircuitAsync(idDemande, numdem, idGroup);
        }





        //WCO - Workflow le 28/10/2015 a 19h44
        public static void InitWOrkflow(string IdDemandeCree, int centreDemandeCree, string NumeroDemandeTableTravail, int codeTdem)
        {
            try
            {
                //Ajouté par WCO le 04/08/2015
                //Pour l'insertion d'une demande directement dans ma table
                Workflow.WorkflowDmdManager managerWkf = new Workflow.WorkflowDmdManager();
                managerWkf.InsertionDemandeWorkflowComplete += (str_rsl) =>
                {
                    if (str_rsl.StartsWith("ERR"))
                    {
                        Message.ShowError(new Exception(str_rsl), "Création d'une demande");
                    }
                    else
                    {
                        Message.ShowInformation("La demande a été créée avec succès. Numéro de votre demande : " + NumeroDemandeTableTravail,
                            Silverlight.Resources.Devis.Languages.txtDevis);
                        RetourneInformationDemande(int.Parse(IdDemandeCree));
                    }
                };
                managerWkf.InsererMaDemande(IdDemandeCree, codeTdem, centreDemandeCree, NumeroDemandeTableTravail);
            }
            catch (Exception ex)
            { 
                throw ex;
            }
        }
        public static void TransmettreDemande(List<string> Codes, bool Isverbose)
        {

            try
            {
                Galatee.Silverlight.ServiceWorkflow.WorkflowClient client = new Galatee.Silverlight.ServiceWorkflow.WorkflowClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Workflow"));
                client.Endpoint.Binding.OpenTimeout = new TimeSpan(0, 1, 0);
                client.Endpoint.Binding.CloseTimeout = new TimeSpan(5, 0, 0);
                client.Endpoint.Binding.SendTimeout = new TimeSpan(5, 0, 0);
                client.ExecuterActionSurPlusieursDemandesCompleted += (sender, args) =>
                {
                    if (args.Cancelled || args.Error != null)
                    {
                        string error = args.Error.Message;
                        Message.Show(error, "Erreur a l'appel du service => ExecuterActionSurPlusieursDemandes");
                        return;
                    }
                    if (args.Result == null)
                    {
                        //Message.ShowError(Languages.msgErreurChargementDonnees, "Erreur a l'appel du service => ExecuterActionSurPlusieursDemandes");
                        return;
                    }
                    if (args.Result.StartsWith("ERR"))
                    {
                        //Message.ShowError(args.Result, Languages.Parametrage);
                    }
                    else
                    {
                        if (Isverbose)
                            Message.ShowInformation(args.Result, "Demande transmise avec succès");
                    }
                };
                client.ExecuterActionSurPlusieursDemandesAsync(Codes, SessionObject.Enumere.TRANSMETTRE, UserConnecte.matricule, string.Empty);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
       public static void TransmettreDemande(List<string> Codes,bool Isverbose,ChildWindow leControle)
        {

            try
            {
                Galatee.Silverlight.ServiceWorkflow.WorkflowClient client = new Galatee.Silverlight.ServiceWorkflow.WorkflowClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Workflow"));
                client.Endpoint.Binding.OpenTimeout = new TimeSpan(0, 1, 0);
                client.Endpoint.Binding.CloseTimeout = new TimeSpan(5, 0, 0);
                client.Endpoint.Binding.SendTimeout = new TimeSpan(5, 0, 0);
                client.ExecuterActionSurPlusieursDemandesCompleted += (sender, args) =>
                {
                    if (args.Cancelled || args.Error != null)
                    {
                        string error = args.Error.Message;
                        Message.Show(error, "Erreur a l'appel du service => ExecuterActionSurPlusieursDemandes");
                        return;
                    }
                    if (args.Result == null)
                    {
                        //Message.ShowError(Languages.msgErreurChargementDonnees, "Erreur a l'appel du service => ExecuterActionSurPlusieursDemandes");
                        return;
                    }
                    if (args.Result.StartsWith("ERR"))
                    {
                        //Message.ShowError(args.Result, Languages.Parametrage);
                    }
                    else
                    {
                        if (Isverbose )
                        Message.ShowInformation(args.Result, "Demande transmise avec succès");
                        //foreach (string  item in Codes)
                        // RetourneInformationDemande(int.Parse(item));
                        leControle.Close();
                    }
                };
                client.ExecuterActionSurPlusieursDemandesAsync(Codes, SessionObject.Enumere.TRANSMETTRE, UserConnecte.matricule, string.Empty);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
       public static void TransmettreDemande(List<CsDemandeBase> lesDemande, bool Isverbose, ChildWindow leControle)
       {

           try
           {
               AcceuilServiceClient clientDevis = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
               clientDevis.TransmettreDemandeCompleted += (ss, b) =>
               {
                   if (b.Cancelled || b.Error != null)
                   {
                       string error = b.Error.Message;
                       Message.ShowError(error, Silverlight.Resources.Devis.Languages.txtDevis);
                       return;
                   }
                   if (string.IsNullOrEmpty(b.Result))
                   {
                       if (Isverbose)
                           Message.ShowInformation("Demande transmise avec succès", "Demande");
                   }
                   else
                       Message.ShowError(b.Result, "Demande");

                   leControle.Close();
               };
               clientDevis.TransmettreDemandeAsync(lesDemande);
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }
       public static void AnnulerDemande(List<string> Codes, bool Isverbose, ChildWindow leControle)
       {

           try
           {
               Galatee.Silverlight.ServiceWorkflow.WorkflowClient client = new Galatee.Silverlight.ServiceWorkflow.WorkflowClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Workflow"));
               client.Endpoint.Binding.OpenTimeout = new TimeSpan(0, 1, 0);
               client.Endpoint.Binding.CloseTimeout = new TimeSpan(5, 0, 0);
               client.Endpoint.Binding.SendTimeout = new TimeSpan(5, 0, 0);
               client.ExecuterActionSurPlusieursDemandesCompleted += (sender, args) =>
               {
                   if (args.Cancelled || args.Error != null)
                   {
                       string error = args.Error.Message;
                       Message.Show(error, "Erreur a l'appel du service => ExecuterActionSurPlusieursDemandes");
                       return;
                   }
                   if (args.Result == null)
                   {
                       //Message.ShowError(Languages.msgErreurChargementDonnees, "Erreur a l'appel du service => ExecuterActionSurPlusieursDemandes");
                       return;
                   }
                   if (args.Result.StartsWith("ERR"))
                   {
                       //Message.ShowError(args.Result, Languages.Parametrage);
                   }
                   else
                   {
                       if (Isverbose)
                           Message.ShowInformation(args.Result, "Demande transmise avec succès");
                       leControle.Close();
                   }
               };
               client.ExecuterActionSurPlusieursDemandesAsync(Codes, SessionObject.Enumere.ANNULER , UserConnecte.matricule, string.Empty);
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }

       public static void RejeterDemande(CsDemande LaDemande, bool isVerbose = false)
        {
            //Rejet de la demande            
            //On appel la fenetre de rejet, pour entrer le motif de la demande
            //if (dtgrdParametre.SelectedItem != null
            //    && dtgrdParametre.SelectedItems.Count == 1)
            if (LaDemande.InfoDemande != null)
            {
                if (null != LaDemande.InfoDemande.LiteRejet && LaDemande.InfoDemande.LiteRejet.Count > 1)
                {
                    //Et on appelle la fenetre
                    Galatee.Silverlight.Workflow.UcWKFSelectEtape ucform = new Galatee.Silverlight.Workflow.UcWKFSelectEtape(LaDemande.InfoDemande, LaDemande.InfoDemande.LiteRejet);
                  
                    ucform.Show();
 
                }
                else
                {
                    if (isVerbose)
                    {
                        Galatee.Silverlight.Workflow.UcWKFMotifRejet ucMotif = new Galatee.Silverlight.Workflow.UcWKFMotifRejet(LaDemande.InfoDemande);
                        ucMotif.Closed += (_sender, args) =>
                        {
                            //On relance juste le getdata
                        };
                        ucMotif.Show();
                    }
                    else
                    {
                        Galatee.Silverlight.ServiceWorkflow.WorkflowClient client = new Galatee.Silverlight.ServiceWorkflow.WorkflowClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Workflow"));
                        client.Endpoint.Binding.OpenTimeout = new TimeSpan(0, 1, 0);
                        client.Endpoint.Binding.CloseTimeout = new TimeSpan(5, 0, 0);
                        client.Endpoint.Binding.SendTimeout = new TimeSpan(5, 0, 0);
                        client.ExecuterActionSurDemandeCompleted += (sender, args) =>
                        {

                            if (args.Cancelled || args.Error != null)
                            {
                                string error = args.Error.Message;
                                Message.Show(error, "Rejet demande");
                                return;
                            }
                            if (args.Result == null)
                            {
                                Message.ShowError("", "Rejet demande");
                                return;
                            }
                            if (args.Result.StartsWith("ERR"))
                            {
                                Message.ShowError(args.Result, "Rejet demande");
                            }

                        };
                        client.ExecuterActionSurDemandeAsync(LaDemande.InfoDemande.CODE, Galatee.Silverlight.SessionObject.Enumere.REJETER, UserConnecte.matricule, "Rejet");
                    }
                }

            }
        }
       public static void NotifierMail(List<CsUtilisateur> lstUtilisateur, string CodeTypeMail)
       {

           try
           {
               List<string> lstDestainataire = new List<string>();
               foreach (CsUtilisateur item in lstUtilisateur)
                   lstDestainataire.Add(item.E_MAIL);
               
               Galatee.Silverlight.ServiceWorkflow.WorkflowClient client = new Galatee.Silverlight.ServiceWorkflow.WorkflowClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Workflow"));
               client.Endpoint.Binding.OpenTimeout = new TimeSpan(0, 1, 0);
               client.Endpoint.Binding.CloseTimeout = new TimeSpan(5, 0, 0);
               client.Endpoint.Binding.SendTimeout = new TimeSpan(5, 0, 0);
               client.NotificationMailCompleted += (sender, args) =>
               {
                   if (args.Cancelled || args.Error != null)
                   {
                       string error = args.Error.Message;
                       //Message.Show(error, "Erreur a l'appel du service => ExecuterActionSurPlusieursDemandes");
                       return;
                   }
                  
               };
               client.NotificationMailAsync(lstDestainataire, CodeTypeMail);
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }

       private static void RetourneInformationDemande(int IdDemandeCree)
       {
           AcceuilServiceClient client = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
           client.RetourneInfoDemandeWkfByIdDemandeCompleted += (ssender, args) =>
           {
               if (args.Cancelled || args.Error != null)
               {
                   string error = args.Error.Message;
                   Message.ShowError(error, Silverlight.Resources.Devis.Languages.txtDevis);
                   return;
               }
               if (args.Result == null)
               {
                   Message.ShowError(Silverlight.Resources.Devis.Languages.AucunesDonneesTrouvees, Silverlight.Resources.Devis.Languages.txtDevis);
                   return;
               }
               else
               {
                   //List<ServiceAccueil.CsUtilisateur> leUser = new List<ServiceAccueil.CsUtilisateur>();
                   //if (args.Result != null )
                   //{
                   //    foreach (ServiceAccueil.CsUtilisateur item in args.Result.UtilisateurEtapeSuivante)
                   //        leUser.Add(item);
                   //    Shared.ClasseMEthodeGenerique.NotifierMailDemande(leUser, "0001", args.Result.CODE_DEMANDE_TABLE_TRAVAIL, args.Result.LIBELLEDEMANDE );
                   //}
                  
               }
           };
           client.RetourneInfoDemandeWkfByIdDemandeAsync(IdDemandeCree);
       }

       public static void NotifierMailDemande(List<ServiceAccueil.CsUtilisateur> lstUtilisateur, string CodeTypeMail,string Numerodemande,string TypeDemande)
       {

           try
           {
               List<string> lstDestainataire = new List<string>();
               foreach (ServiceAccueil.CsUtilisateur item in lstUtilisateur)
                   lstDestainataire.Add(item.E_MAIL);

               Galatee.Silverlight.ServiceWorkflow.WorkflowClient client = new Galatee.Silverlight.ServiceWorkflow.WorkflowClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Workflow"));
               client.Endpoint.Binding.OpenTimeout = new TimeSpan(0, 1, 0);
               client.Endpoint.Binding.CloseTimeout = new TimeSpan(5, 0, 0);
               client.Endpoint.Binding.SendTimeout = new TimeSpan(5, 0, 0);
               client.NotificationMailDemandeAsync(lstDestainataire, Numerodemande, TypeDemande, CodeTypeMail);
               client.NotificationMailDemandeCompleted += (sender, args) =>
               {
                   if (args.Cancelled || args.Error != null)
                   {
                       string error = args.Error.Message;
                       //Message.Show(error, "Erreur a l'appel du service => ExecuterActionSurPlusieursDemandes");
                       return;
                   }

               };
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }

        public static bool RetourneValueFromClasse<T>(T _Classe, string _Colonne) where T : new()
        {
            try
            {
                bool LavaleurColonne = false;
                // Recuperation des types
                PropertyInfo[] properties1 = _Classe.GetType().GetProperties();
                for (int attrNum = 0; attrNum < properties1.Length; attrNum++)
                {
                    if (properties1[attrNum].Name.Equals(_Colonne))
                    {
                        object value2 = properties1[attrNum].GetValue(_Classe, null);
                        LavaleurColonne = Convert.ToBoolean(value2);
                        break;
                    }
                }
                return LavaleurColonne;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public static void FormStatLoopVisualTree(DependencyObject obj, bool State)
        {

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {

                if (obj is TextBox)
                    ((TextBox)obj).IsReadOnly = !State;
                if (obj is CheckBox)
                    ((CheckBox)obj).IsEnabled = State;
                if (obj is Button)
                    ((PasswordBox)obj).IsEnabled = State;
                if (obj is ComboBox)
                    ((ComboBox)obj).IsEnabled = State;
                if (obj is DataGrid)
                    ((DataGrid)obj).IsReadOnly = !State;

                FormStatLoopVisualTree(VisualTreeHelper.GetChild(obj, i), State);
            }
        }
        public static void RejeterDemande(CsDemande laDetailDemande,ChildWindow leControlParent)
        {
            try
            {
                ServiceAccueil.CsDemande laDemande = new ServiceAccueil.CsDemande();
                laDemande.LaDemande = Utility.ConvertType<ServiceAccueil.CsDemandeBase, CsDemandeBase>(laDetailDemande.LaDemande);
                if (laDetailDemande.InfoDemande != null)
                {
                    laDemande.InfoDemande = new ServiceAccueil.CsInfoDemandeWorkflow()
                    {
                        CODE = laDetailDemande.InfoDemande.CODE,
                        CODE_DEMANDE_TABLE_TRAVAIL = laDetailDemande.InfoDemande.CODE_DEMANDE_TABLE_TRAVAIL,
                        CODETDEM = laDetailDemande.InfoDemande.CODETDEM,

                    };
                    Galatee.Silverlight.Shared.ClasseMEthodeGenerique.RejeterDemande(laDemande, true);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static void RejeterDemande(CsDemande laDetailDemande)
        {
            try
            {
                ServiceAccueil.CsDemande laDemande = new ServiceAccueil.CsDemande();
                laDemande.LaDemande = Utility.ConvertType<ServiceAccueil.CsDemandeBase,CsDemandeBase>(laDetailDemande.LaDemande);
                if (laDetailDemande.InfoDemande != null)
                {
                    laDemande.InfoDemande = new ServiceAccueil.CsInfoDemandeWorkflow()
                    {
                        CODE = laDetailDemande.InfoDemande.CODE,
                        CODE_DEMANDE_TABLE_TRAVAIL = laDetailDemande.InfoDemande.CODE_DEMANDE_TABLE_TRAVAIL,
                        CODETDEM = laDetailDemande.InfoDemande.CODETDEM,

                    };
                    Galatee.Silverlight.Shared.ClasseMEthodeGenerique.RejeterDemande(laDemande, true);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static  void AffectationUser(CsDemande laDetailDemande)
        {
            try
            {
                //Affectation de la demande
                Galatee.Silverlight.ServiceParametrage.CsAffectationDemandeUser lAffectation = new Galatee.Silverlight.ServiceParametrage.CsAffectationDemandeUser();
                lAffectation.CODEDEMANDE = laDetailDemande.InfoDemande.CENTRE;
                lAffectation.FK_IDETAPE = laDetailDemande.InfoDemande.FK_IDETAPESUIVANTE;
                lAffectation.MATRICULEUSER = laDetailDemande.OrdreTravail.MATRICULE;
                lAffectation.OPERATIONID = laDetailDemande.InfoDemande.FK_IDOPERATION;
                lAffectation.CENTREID = laDetailDemande.LaDemande.FK_IDCENTRE;
                lAffectation.WORKFLOWID = laDetailDemande.InfoDemande.FK_IDWORKFLOW;
                lAffectation.FK_IDETAPEFROM = laDetailDemande.InfoDemande.FK_IDETAPEACTUELLE;
                lAffectation.MATRICULEUSERCREATION = UserConnecte.matricule;
                lAffectation.PK_ID = Guid.NewGuid();

                Galatee.Silverlight.ServiceParametrage.ParametrageClient client = new Galatee.Silverlight.ServiceParametrage.ParametrageClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Parametrage"));
                client.AffecterDemandeCompleted += (af_sender, args) =>
                {
                    if (args.Cancelled || args.Error != null)
                    {
                        string error = args.Error.Message;
                        return;
                    }
                    if (args.Result == null)
                    {
                        Message.ShowError("Une erreur est survenue", "AffectationUser");
                        return;
                    }
                    if (args.Result != null)
                    {
                        if (!args.Result)
                        {
                            Message.ShowInformation("Une erreur est survenue", "Affectation de demande");
                        }
                    }
                };
                client.AffecterDemandeAsync(new List<Galatee.Silverlight.ServiceParametrage.CsAffectationDemandeUser>() { lAffectation });
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public static void FermetureEcran( ChildWindow leControle)
        {
            var ws = new MessageBoxControl.MessageBoxChildWindow("Fermeture", Galatee.Silverlight.Resources.Langue.msgFermetureEcran, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Information);
            ws.OnMessageBoxClosed += (l, results) =>
            {
                if (ws.Result == MessageBoxResult.OK)
                {
                    //leControle.Close();
                    leControle.DialogResult = false;
                }
            };
            ws.Show();

        }

        public static void SetMachineAndPortFromEndPoint(System.ServiceModel.EndpointAddress ep)
        {
            Dictionary<string, string> ValeurRetour = new Dictionary<string, string>();
            string adresseserviceImpr = ep.ToString();
            string Chaine2 = adresseserviceImpr.Replace("http://", " ").Trim();
            string[] split2 = Chaine2.Split('/');
            string[] split3 = split2[0].Split(':');
             //=split3[0] + ";" + split3[1];
            ValeurRetour.Add(split3[0], split3[1]);

            SessionObject.ServerEndPointName = split3[0];
            SessionObject.ServerEndPointPort = split3[1];
        }


        public static bool checkSelectedItem(CheckBox check)
        {
            CheckBox chk = check;
            return chk.IsChecked.Value;
        }
        public static void checkerSelectedItem(CheckBox check)
        {
            try
            {
                CheckBox chk = check;
                chk.IsChecked = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static void DecheckerSelectedItem(CheckBox check)
        {
            try
            {
                CheckBox chk = check;
                chk.IsChecked = false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static void UncheckAllItem(CheckBox check, bool value)
        {
            try
            {
                CheckBox chk = check;
                chk.IsChecked = value;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public static  int JourOuvrer(DateTime dtmStart, DateTime dtmEnd)
        {
            // This function includes the start and end date in the count if they fall on a weekday
            int dowStart = ((int)dtmStart.DayOfWeek == 0 ? 7 : (int)dtmStart.DayOfWeek);
            int dowEnd = ((int)dtmEnd.DayOfWeek == 0 ? 7 : (int)dtmEnd.DayOfWeek);
            TimeSpan tSpan = dtmEnd - dtmStart;
            if (dowStart <= dowEnd)
            {
                return (((tSpan.Days / 7) * 5) + Math.Max((Math.Min((dowEnd + 1), 6) - dowStart), 0));
            }
            else
            {
                return (((tSpan.Days / 7) * 5) + Math.Min((dowEnd + 6) - Math.Min(dowStart, 6), 5));
            }
        }

        public static bool CompareObjet<T>(T _Objet1, T _Objet2) where T : new()
        {
            string Chm = string.Empty;
            string ch = string.Empty;
            try
            {
            
                bool Resultat = false;

                // Recuperation des types
                PropertyInfo[] properties1 = _Objet1.GetType().GetProperties();
                PropertyInfo[] properties11 = _Objet2.GetType().GetProperties();

                 foreach (var f in properties1) // Récuperation des valeurs des propriete de l'objet
                   {
                       Chm = f.Name;
                       PropertyInfo l = properties11.FirstOrDefault(p => p.Name == f.Name);
                        if (l != null )
                        {
                            object value1 = f.GetValue(_Objet1, null);
                            object value2 = l.GetValue(_Objet2, null);
                            if (value1 == null && value2 != null)
                            {
                                if (!string.IsNullOrEmpty(value2.ToString()))
                                {
                                    Resultat = true;
                                    break;
                                }
                            }
                            if (value1 != null && value2 == null)
                            {
                                if (!string.IsNullOrEmpty(value1.ToString()))
                                {
                                    Resultat = true;
                                    break;
                                }
                            }
                            if (value1 != null && value2 != null)
                            {
                                if (value2.ToString() != value1.ToString())
                                {
                                    Resultat = true;
                                    break;
                                }
                            }
                        }
                    }
                    return Resultat;
                
            }
            catch (Exception ex)
            {
                ch = Chm;
                throw ex;
            }
        }

       public static  T ParseObject<T, P>(T objetARemplir, P objetATransferer)
        {
            int indexval = 0;
            try
            {
                T objetARemp = objetARemplir;
                // Recuperation des types
                PropertyInfo[] properties1 = objetARemplir.GetType().GetProperties();
                PropertyInfo[] properties2 = objetATransferer.GetType().GetProperties();

                // Test de l'unicité des deux types
                if (properties1.Length == properties2.Length)
                {
                    // Remplacement des valeurs

                    for (int attrNum = 0; attrNum < properties2.Length; attrNum++)
                    {
                        indexval = indexval + 1;
                        if (properties1[attrNum].GetType().Equals(properties2[attrNum].GetType()))
                        {
                            object value2 = properties2[attrNum].GetValue(objetATransferer, null);
                            properties1[attrNum].SetValue(objetARemp, value2, null);
                        }
                    }

                    return objetARemp;

                }
                else
                    throw new Exception("Les types n'ont pas la meme structure");
            }
            catch (Exception ex)
            {
                int t = indexval;
                throw ex;
            }

        }

    }
}
