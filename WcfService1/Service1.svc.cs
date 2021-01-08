using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using Galatee.Structure;
using System.Collections;

namespace WcfService1
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class Service1 : IService1
    {
        //Dictionnaire des parametres pour le Dataset 
        static Hashtable parametres = new Hashtable();
        //Dictionnaire des données a imprimer
        static Hashtable dicosCsPrints = new Hashtable();

        /// <summary>
        /// Enregistre la liste des objets participants au report dans un hashtable static
        /// </summary>
        /// <param name="ObjectTable">Un hashtable constitué de la clé de l'user et de la liste des donnée à imprimer</param>
        /// <param name="key">La clé de l'utilisateur qui lance l'impression</param>        
        /// <param name="parameters">Les parametres additionnels de report</param>
        /// <returns>True pour reussite et false pour echec</returns>
        public bool? setFromWebPart(List<CsPrint> ObjectTable, string key, Dictionary<string, string> parameter)
        {
            try
            {
                ErrorManager.WriteInLogFile(this, key);

                if (dicosCsPrints.Contains(key))
                {
                    dicosCsPrints.Remove(key);
                }
                dicosCsPrints.Add(key, ObjectTable);

                if (parametres.Contains(key))
                    parametres.Remove(key);
                parametres.Add(key, parameter);
                return true;
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }

        public List<CsPrint> GetCsPrintFromWebPart(string key)
        {
            try
            {
                ErrorManager.WriteInLogFile(this, "Lecture debut");
                ErrorManager.WriteInLogFile(this, key);

                List<CsPrint> csprints = new List<CsPrint>();
                csprints = dicosCsPrints[key] as List<CsPrint>;

                if (csprints == null)
                    ErrorManager.WriteInLogFile(this, "Lecture Non OK");
                else
                    ErrorManager.WriteInLogFile(this, "Lecture OK");

                return csprints;
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
            //finally {
            //    if (dicosCsPrints.ContainsKey(key))
            //        dicosCsPrints.Remove(key);
            //}
        }

        public Dictionary<string, string> getParameters(string key)
        {
            try
            {
                return parametres[key] as Dictionary<string, string>;
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
            //finally {

            //    if (parametres.ContainsKey(key))
            //        parametres.Remove(key);
            //}
        }

        public bool? SetErrorsFromSilverlightWebPrinting(string methodInvoquante, string Error)
        {
            try
            {
                ErrorManager.WriteInLogFileFromWeb(this, methodInvoquante, Error);
                return true;
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }
        public List<CsCampagnesBTAAccessiblesParLUO> Stat_LoadCampgne()
        {
            try
            {
                return new List<CsCampagnesBTAAccessiblesParLUO>();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }
        public List<CsHabilitationCaisse> EtatCaisse()
        {
            try
            {
                return new List<CsHabilitationCaisse>();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }
        public List<CsDemandeBase> DemandeBase()
        {
            try
            {
                return new List<CsDemandeBase>();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }
        public List<CsReglementRecu> ReportListeEncaissements()
        {
            try
            {
                return new List<CsReglementRecu>();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public List<CsHabilitationProgram> ListeCsHabilitationProgram()
        {
            try
            {
                return new List<CsHabilitationProgram>();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }
        public List<CustumEclairagePublic> GetCustumEclairagePublic()
        {
            try
            {
                return new List<CustumEclairagePublic>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CsDetailCampagne> GetDetailCampagne()
        {
            try
            {
                return new List<CsDetailCampagne>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CsHabilitationMetier> EtatHabilitationMetier()
        {
            try
            {
                return new List<CsHabilitationMetier>();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }
        public List<CsHabilitationMenu> EtatHabilitationMenu()
        {
            try
            {
                return new List<CsHabilitationMenu>();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }
        public List<CsAnnomalie> RetourneAnomalie()
        {
            try
            {
                return new List<CsAnnomalie>();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }
        public List<CsEditionDevis> EditionDevis()
        {
            return new List<CsEditionDevis>();
        }
        public List<CsOrdreTravail> EditerOT()
        {
            return new List<CsOrdreTravail>();
        }
        public List<CsEvenement> EditerEnquete()
        {
            return new List<CsEvenement>();
        }
        public List<CsAvisDeCoupureClient> EditerAvisClient()
        {
            return new List<CsAvisDeCoupureClient>();
        }
    }
}
