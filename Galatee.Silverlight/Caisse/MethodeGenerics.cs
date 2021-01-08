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
using System.Runtime.Serialization;




namespace Galatee.Silverlight.Caisse
{
    static class MethodeGenerics
    {
        public static List<CsClient> RetourneClientFromFacture(List<CsLclient > ListeFactureAregle)
        {
            try
            {
                
                List<CsClient> lstClientFacture = new List<CsClient>();
                if (ListeFactureAregle.Count > 0)
                {
                    var lstClientFactureDistnct = ListeFactureAregle.Select(t => new { t.CENTRE, t.CLIENT, t.ORDRE, t.FK_IDCLIENT,t.LIBELLESITE ,t.NOM,t.FK_IDCENTRE  }).Distinct().ToList();
                    foreach (var item in lstClientFactureDistnct)
                    {

                        lstClientFacture.Add(new CsClient { CENTRE = item.CENTRE, REFCLIENT = item.CLIENT, ORDRE = item.ORDRE, PK_ID = item.FK_IDCLIENT, LIBELLESITE = item.LIBELLESITE,NOMABON = item.NOM,FK_IDCENTRE = item.FK_IDCENTRE    });
                    }
                }
                return lstClientFacture;
            }
            catch (Exception ex )
            {
                
                throw ex;
            }
        }

        public static List<Galatee.Silverlight.ServiceAccueil.CsSite> RetourneClientFromFacture(List<Galatee.Silverlight.ServiceAccueil.CsCentre > ListeCentre)
        {
            try
            {
                List<Galatee.Silverlight.ServiceAccueil.CsSite> lstSiteCentre = new List<Galatee.Silverlight.ServiceAccueil.CsSite>();
                if (ListeCentre.Count > 0)
                {
                    var lstClientFactureDistnct = ListeCentre.Select(t => new { t.CODESITE , t.FK_IDCODESITE}).Distinct().ToList();
                    foreach (var item in lstClientFactureDistnct)
                        lstSiteCentre.Add(new Galatee.Silverlight.ServiceAccueil.CsSite { CODE = item.CODESITE, PK_ID = item.FK_IDCODESITE });

                }
                return lstSiteCentre;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public static  List<CsHabilitationCaisse> RetourneDistincCaissier(List<CsHabilitationCaisse> _lstDesCaisseOuverte)
        {
            List<CsHabilitationCaisse> lstDesHabilCaisse = new List<CsHabilitationCaisse>();
            if (_lstDesCaisseOuverte.Count > 0)
            {
                var lstCaisseDistnct = _lstDesCaisseOuverte.Select(t => new { t.MATRICULE, t.NOMCAISSE }).Distinct().ToList();
                foreach (var item in lstCaisseDistnct)
                    lstDesHabilCaisse.Add(new CsHabilitationCaisse { MATRICULE = item.MATRICULE, NOMCAISSE = item.NOMCAISSE });
            }
            return lstDesHabilCaisse;
        }


        public static List<CsLclient> RetourneDistincAcquitement(List<CsLclient> _lstDesEncaissement)
        {
            List<CsLclient> lstDesAcquit = new List<CsLclient>();
            if (_lstDesEncaissement.Count > 0)
            {
                var lstCaisseDistnct = _lstDesEncaissement.Select(t => new { t.MATRICULE, t.FK_IDHABILITATIONCAISSE ,t.ACQUIT ,t.MOTIFANNULATION ,t.NUMDEM  }).Distinct().ToList();
                foreach (var item in lstCaisseDistnct)
                    lstDesAcquit.Add(new CsLclient { MATRICULE = item.MATRICULE, FK_IDHABILITATIONCAISSE = item.FK_IDHABILITATIONCAISSE, ACQUIT  = item.ACQUIT,MOTIFANNULATION = item.MOTIFANNULATION ,NUMDEM =item.NUMDEM   });
            }
            return lstDesAcquit;
        }
    }
}
