using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.IO;
//using Galatee.Silverlight.serviceWeb;
using Galatee.Silverlight.ServiceCaisse;
using System.Globalization;
using System.Collections.ObjectModel;


namespace Galatee.Silverlight.Caisse
{
   static class EditionRecu
    {
       public static List<CsReglement> RecupereListDesClientDuReglement(List<CsReglement> ListeDesReglement)
       {
           List<CsReglement> _LstClient = new List<CsReglement>();
           List<IGrouping<string, CsReglement>> _LstClientRglt = ListeDesReglement.GroupBy(p => p.REFFERENCECLIENT).ToList();
           foreach (IGrouping<string, CsReglement> items in _LstClientRglt)
               _LstClient.Add(ListeDesReglement.FirstOrDefault(p => p.REFFERENCECLIENT == items.Key));
           return _LstClient;
       }
       public static List<CsReglement> RecupereListDesACquitementDuReglement(List<CsReglement> ListeDesReglement)
       {
           List<CsReglement> _LstClient = new List<CsReglement>();
           List<IGrouping<string, CsReglement>> _LstClientRglt = ListeDesReglement.GroupBy(p => p.ACQUIT ).ToList();
           foreach (IGrouping<string, CsReglement> items in _LstClientRglt)
               _LstClient.Add(ListeDesReglement.FirstOrDefault(p => p.ACQUIT  == items.Key));
           return _LstClient;
       }
        static public void RetourneListeRecuAEditer(List<CsReglement> ListeDesReglement,List<CsModereglement> LesModeReglement, string Action, List<CsReglement> retournFx, string printer)
        {
            List<CsReglement> _ListeDesClientsDuRecu = new List<CsReglement>();
            List<CsReglement> _ListeDeRecuAEditer = new List<CsReglement>();

            foreach (CsReglement _rglt in ListeDesReglement)
            {
                _rglt.REFFERENCECLIENT = _rglt.CENTRE + _rglt.CLIENT + _rglt.ORDRE;
                CsReglement _UnClientDuReglement = new CsReglement();
                _UnClientDuReglement.NOMCAISSIERE = UserConnecte.nomUtilisateur;
                _UnClientDuReglement = _ListeDesClientsDuRecu.FirstOrDefault(m => m.REFFERENCECLIENT == _rglt.REFFERENCECLIENT);

                if (_UnClientDuReglement == null)
                    _ListeDesClientsDuRecu.Add(_rglt);
            }
            if (_ListeDesClientsDuRecu.Count > 0)
            {
                foreach (CsReglement _ClientDuRecu in _ListeDesClientsDuRecu)
                {
                    List<CsReglement> _ListeDesReglemtClient = new List<CsReglement>();
                   _ListeDesReglemtClient = (from p in ListeDesReglement 
                                             where p.REFFERENCECLIENT == _ClientDuRecu.REFFERENCECLIENT
                                             select p).ToList();

                   //Total encaissemé par modereglement par client
                    List<CsModereglement> _ListReglementModeReglt = new List<CsModereglement>();
                    foreach (CsModereglement item in LesModeReglement )
                    {
                        string LeModeReglt = item.LIBELLE;
                        CsModereglement _UnModeReglt = new CsModereglement();

                        foreach (CsReglement _rglt in _ListeDesReglemtClient)
                        {
                            if (_rglt.MODEREG == item.CODE)
                            {
                                if (_rglt.COPER != SessionObject.Enumere.CoperRNA)
                                _UnModeReglt.MONTANT = _UnModeReglt.MONTANT + _rglt.MONTANTPAYE.Value;
                            }
                        }

                        _UnModeReglt.CODE = item.LIBELLE + " => " + _UnModeReglt.MONTANT.ToString() + "  ";
                        _ListReglementModeReglt.Add(_UnModeReglt);
                    }
                    _ClientDuRecu.MODEREG = string.Empty;

                    foreach (CsModereglement elt in _ListReglementModeReglt)
                    {
                        _ClientDuRecu.MODEREG = _ClientDuRecu.MODEREG + elt.CODE;
                    }
                    string _ListeDeFactureRegleParClient = string.Empty;
                    List<CsReglement> _ListeDesNumFact = new List<CsReglement>();
                    decimal? _MontantTotalPaye = 0;
                    decimal? _MontantTotalEncaisse = 0;
                    decimal? _MontantTotalRna = 0;
                    foreach (CsReglement _Reglt in _ListeDesReglemtClient)
                    {
                        if (_ListeDesNumFact.FirstOrDefault(p => p.NDOC == _Reglt.NDOC) == null)
                        {
                            _ListeDeFactureRegleParClient = _ListeDeFactureRegleParClient + ("   " + _Reglt.REFEM + "." + _Reglt.NDOC);
                            _ListeDesNumFact.Add(_Reglt);
                        }
                        if (_Reglt.COPER != SessionObject.Enumere.CoperRNA)
                            _MontantTotalEncaisse = _MontantTotalEncaisse + _Reglt.MONTANTPAYE;
                        if (_Reglt.COPER == SessionObject.Enumere.CoperRNA )
                            _MontantTotalRna = _MontantTotalRna + _Reglt.MONTANTPAYE;

                        //_MontantTotalPaye = _MontantTotalPaye + _Reglt.MONTANTPAYE;

                    }
                    _MontantTotalPaye = _MontantTotalEncaisse - _MontantTotalRna;  
                    _ClientDuRecu.MONTANTPAYE = _MontantTotalPaye.Value;
                     _ClientDuRecu.NDOC = _ListeDeFactureRegleParClient;
                    
                     if (Action.ToUpper() ==SessionObject.Enumere.ActionRecuEditionNormale.ToUpper())
                         _ClientDuRecu.SOLDEALADATE = _ClientDuRecu.SOLDEANTERIEUR - _ClientDuRecu.MONTANTPAYE;
                     else
                         _ClientDuRecu.SOLDEALADATE = _ClientDuRecu.SOLDEANTERIEUR;
                     if (Action.ToUpper() == SessionObject.Enumere.ActionRecuAnnulation.ToUpper())
                         _ClientDuRecu.SOLDEALADATE = _ClientDuRecu.SOLDEANTERIEUR + _ClientDuRecu.MONTANTPAYE;
                    _ClientDuRecu.SOLDEALADATEFORMATE = _ClientDuRecu.SOLDEALADATE== null ? "00.00":_ClientDuRecu.SOLDEALADATE.Value.ToString("0,0.00", CultureInfo.InvariantCulture);

                    _ClientDuRecu.MONTANTPAYEFORMATE = _ClientDuRecu.MONTANTPAYE == null ? "00.00" : _ClientDuRecu.MONTANTPAYE.Value.ToString("0,0.00", CultureInfo.InvariantCulture);
                    _ClientDuRecu.SOLDEANTERIEURFORMATE = _ClientDuRecu.SOLDEANTERIEUR == null ? "00.00" : _ClientDuRecu.SOLDEANTERIEUR.Value.ToString("0,0.00", CultureInfo.InvariantCulture);
                    retournFx.Add(_ClientDuRecu);
                }

            }
     
        }
        static public  List<CParametre> RetourneListImprante()
        {
            List<CParametre> ListeImprimanteRetourne = new List<CParametre>();
            //PrinterSettings.StringCollection Imprimante = null;
            //Imprimante = PrinterSettings.InstalledPrinters;
            //for (int i = 0; i < Imprimante.Count; i++)
            //{
            //    CParametre UneImprimante = new CParametre();
            //    UneImprimante.LIBELLE = Imprimante[i];
            //    ListeImprimanteRetourne.Add(UneImprimante);
            //}
            return ListeImprimanteRetourne;
        }

      
        static public List<CsReglementRecu> ReorganiserBordero(List<Galatee.Silverlight.ServiceCaisse.CsLclient> lstInit)
        {
            try
            {
                List<CsReglementRecu> listImpression = new List<CsReglementRecu>();
                string CentreEncaisse = string.Empty;
                string SiteEncaisse = string.Empty;
                if (!string.IsNullOrEmpty(lstInit[0].LIBELLEAGENCE))
                    CentreEncaisse = lstInit[0].LIBELLEAGENCE;
                else
                {
                    Galatee.Silverlight.ServiceAccueil.CsCentre leCentre = SessionObject.LstCentre.FirstOrDefault(t => t.CODE == lstInit[0].ORIGINE);
                    if (leCentre != null)
                        CentreEncaisse = leCentre.LIBELLE;
                }
                if (!string.IsNullOrEmpty(lstInit[0].LIBELLESITE))
                    SiteEncaisse = lstInit[0].LIBELLESITE;
                else
                {
                    Galatee.Silverlight.ServiceAccueil.CsCentre leCentre = SessionObject.LstCentre.FirstOrDefault(t => t.CODE == lstInit[0].ORIGINE);
                    if (leCentre != null)
                        SiteEncaisse = leCentre.LIBELLESITE;
                }

                // La grid doit afficher le detail d un recu par mode de reglement
      
                Galatee.Silverlight.ServiceCaisse.CsLclient recuPercu = lstInit.FirstOrDefault(t => t.PERCU != 0 && t.PERCU != null);
                decimal? percu = recuPercu.PERCU;
                decimal? rendu = recuPercu.RENDU;
                List<CsClient> _lstClientRecu = Galatee.Silverlight.Caisse.MethodeGenerics.RetourneClientFromFacture(lstInit);
                foreach (CsClient item in _lstClientRecu)
                {
                        List<CsLclient> lstFactureClient = lstInit.Where(t => t.FK_IDCLIENT == item.PK_ID ).ToList();

                        List<CsLclient> reglemntParAcquit = lstInit.Where(p => p.NATURE != "99" && p.FK_IDCLIENT == item.PK_ID).ToList();
                         Galatee.Silverlight.ServiceCaisse.CsLclient  ReglementAfficherParTimbre = lstInit.FirstOrDefault (p => p.NATURE == "99" && p.FK_IDCLIENT == item.PK_ID);
                        CsReglementRecu ObjImpression = new CsReglementRecu();
                        decimal? timbre = ReglementAfficherParTimbre != null && ReglementAfficherParTimbre.MONTANT != 0 ? ReglementAfficherParTimbre.MONTANT : 0;
                        string nom = SessionObject.ListeDesUtilisateurs.FirstOrDefault(p => p.MATRICULE == lstFactureClient[0].MATRICULE).LIBELLE;
                        ObjImpression.NOMCAISSIERE = nom + "(" + lstFactureClient[0].MATRICULE + " )";
                        ObjImpression.CENTREENCAISSE = CentreEncaisse;
                        ObjImpression.SITEENCAISSEMENT = SiteEncaisse;
                        ObjImpression.MONTANTHT = Convert.ToDecimal(Convert.ToDecimal(lstFactureClient.Sum(t=>t.MONTANT)).ToString(SessionObject.FormatMontant));
                        ObjImpression.MONTANTTIMBRE = Convert.ToDecimal(Convert.ToDecimal(timbre).ToString(SessionObject.FormatMontant));
                        ObjImpression.MONTANTPAYE = Convert.ToDecimal(Convert.ToDecimal(ObjImpression.MONTANTHT + timbre).ToString(SessionObject.FormatMontant));
                        ObjImpression.CENTRE = item.CENTRE;
                        ObjImpression.CLIENT = item.REFCLIENT ;
                        ObjImpression.ORDRE = item.ORDRE;
                        ObjImpression.ACQUIT = lstFactureClient[0].ACQUIT;
                        ObjImpression.NOMCLIENT = lstFactureClient[0].NOM ;
                        listImpression.Add(ObjImpression);
                    
                }
                return listImpression;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        static public List<CsReglementRecu> ReorganiserReglement(List<Galatee.Silverlight.ServiceCaisse.CsLclient> lstInit, string Action)
        {
            try
            {
                List<CsReglementRecu> listImpression = new List<CsReglementRecu>();
                string CentreEncaisse = string.Empty;
                string SiteEncaisse = string.Empty;
                if (!string.IsNullOrEmpty(lstInit[0].LIBELLEAGENCE))
                    CentreEncaisse = lstInit[0].LIBELLEAGENCE;
                else
                {
                    Galatee.Silverlight.ServiceAccueil.CsCentre leCentre = SessionObject.LstCentre.FirstOrDefault(t => t.CODE == lstInit[0].ORIGINE);
                    if (leCentre != null)
                        CentreEncaisse = leCentre.LIBELLE;
                }
                if (!string.IsNullOrEmpty(lstInit[0].LIBELLESITE))
                    SiteEncaisse = lstInit[0].LIBELLESITE;
                else
                {
                    Galatee.Silverlight.ServiceAccueil.CsCentre leCentre = SessionObject.LstCentre.FirstOrDefault(t => t.CODE == lstInit[0].ORIGINE);
                    if (leCentre != null)
                        SiteEncaisse = leCentre.LIBELLESITE;
                }

                // La grid doit afficher le detail d un recu par mode de reglement
                List<CsLclient> reglemntParAcquit = lstInit.Where(p => p.NDOC != "TIMBRE").ToList();
                Galatee.Silverlight.ServiceCaisse.CsLclient ReglementAfficherParTimbre = lstInit.FirstOrDefault  (p => p.NDOC  == "TIMBRE");
                Galatee.Silverlight.ServiceCaisse.CsLclient recuPercu = lstInit.FirstOrDefault(t => t.PERCU != 0 && t.PERCU != null);
                //decimal? percu = recuPercu.PERCU;
                //decimal? rendu = recuPercu.RENDU;

                decimal? percu = 0;
                decimal? rendu = 0;

                if (recuPercu != null)
                {
                    percu = recuPercu.PERCU;
                    rendu = recuPercu.RENDU;
                }
                
                int passage = 0;
                foreach (CsLclient item in reglemntParAcquit.OrderBy(t => t.MODEREG).ThenBy(u=>u.LIBELLECOPER).ThenBy(p => p.NDOC))
                {
                    decimal? timbre = 0;
                    if (passage ==0 )
                        timbre = ReglementAfficherParTimbre != null && ReglementAfficherParTimbre.MONTANT != 0 ? ReglementAfficherParTimbre.MONTANT : 0;

                    CsReglementRecu ObjImpression = new CsReglementRecu();
                    string nom = SessionObject.ListeDesUtilisateurs.FirstOrDefault(p => p.MATRICULE == item.MATRICULE).LIBELLE;
                    ObjImpression.LIBELLEMODREG = SessionObject.ListeModesReglement.FirstOrDefault(t => t.CODE == item.MODEREG).LIBELLE;
                    ObjImpression.NOMCAISSIERE = nom + "(" + item.MATRICULE + " )";
                    ObjImpression.CENTREENCAISSE = CentreEncaisse;
                    ObjImpression.SITEENCAISSEMENT = SiteEncaisse;
                    ObjImpression.MONTANTHT = Convert.ToDecimal(Convert.ToDecimal(item.MONTANT).ToString(SessionObject.FormatMontant));
                    ObjImpression.MONTANTTIMBRE = Convert.ToDecimal(Convert.ToDecimal(timbre).ToString(SessionObject.FormatMontant));
                    ObjImpression.MONTANTPAYE = Convert.ToDecimal(Convert.ToDecimal(item.MONTANT + timbre).ToString(SessionObject.FormatMontant));
                    ObjImpression.NDOC = item.REFEM + '/' + item.NDOC;
                    ObjImpression.PERCU = Convert.ToDecimal(Convert.ToDecimal(percu).ToString(SessionObject.FormatMontant));
                    ObjImpression.RENDU = Convert.ToDecimal(Convert.ToDecimal(rendu).ToString(SessionObject.FormatMontant));
                    ObjImpression.NOMCLIENT = item.NOM;
                    ObjImpression.CENTRE = item.CENTRE;
                    ObjImpression.CLIENT = item.CLIENT;
                    ObjImpression.ORDRE = item.ORDRE;
                    ObjImpression.CAISSE = item.CAISSE;
                    ObjImpression.ACQUIT = item.ACQUIT;
                    //ObjImpression.LIBELLEFACTURE  = item.COPER ;
                    if (listImpression != null && listImpression.FirstOrDefault(o=>o.CENTRE == item.CENTRE && o.CLIENT == item.CLIENT && o.TOPANNUL == item.LIBELLECOPER ) == null )
                    ObjImpression.TOPANNUL = item.LIBELLECOPER;
                    else
                        ObjImpression.TOPANNUL = "";

                    ObjImpression.MONTANTEXIGIBLE = item.MONTANTEXIGIBLE;
                    ObjImpression.MONTANTNONEXIGIBLE = item.MONTANTNONEXIGIBLE;
                    ObjImpression.SOLDEALADATE = item.SOLDECLIENT;
                    ObjImpression.DATEENCAISSEMENT  = item.DATECREATION ;
                    listImpression.Add(ObjImpression);
                    passage++;
                }
                return listImpression;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public   static decimal? CalculMontantTimbre(decimal montantSaisie)
        {
            decimal? leFrais = 0;
            CsFraisTimbre leFraistimbre = SessionObject.LstFraisTimbre.FirstOrDefault(p => montantSaisie >= p.BORNEINF && montantSaisie <= (p.BORNESUP + p.FRAIS));
            if (leFraistimbre != null)
                leFrais = Convert.ToDecimal(leFraistimbre.FRAIS);

            return leFrais;
        }
      
    }
}
