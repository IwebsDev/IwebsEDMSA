using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Galatee.Structure;
using Galatee.DataAccess;
using System.ServiceModel.Activation;

namespace WcfService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "IFraudeService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select IFraudeService.svc or IFraudeService.svc.cs at the Solution Explorer and start debugging.
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)] 
    public class FraudeService : IFraudeService
    {
        #region BOBOU

        #region SOURCECONTROLE
        public List<CsSourceControle> SelectAllSourceControle()
        {
            try
            {
                return new DBFRAUDE().SelectAllSouscontrole();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }
        #endregion

        #region MoyenDenonciation
        public List<CsMoyenDenomciation> SelectAllMoyenDenomciation()
        {
            try
            {
                return new DBFRAUDE().SelectAllMoyenDenomciation();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }
        #endregion

        #region Client Iweb
        public List<CsClient> SelectAllClient()
        {
            try
            {
                return new DBFRAUDE().SelectAllClientIWebs();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public List<CsClient> RetourneClient(int fk_idcentre, string centre, string client, string Ordre)
        {
            try
            {
                DBFRAUDE db = new DBFRAUDE();
                return db.RetourneClient(fk_idcentre, centre, client, Ordre);
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                throw ex;
            }
        }

        #endregion

        #region Fraude
        public int InsertFraudeDenociateur(CsDemandeFraude sDemandeFraude)
        {
            try
            {
                return new DBFRAUDE().InsertFraudeDenociateur(sDemandeFraude.Fraude, sDemandeFraude.Denonciateur, sDemandeFraude.ClientFraude);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return 1;
            }
        }
        public bool InsertFraude(CsFraude sFraude)
        {
            try
            {
                return new DBFRAUDE().InsertFraude(sFraude);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }
        public bool InsertLesFraude(List<CsFraude> sFraude)
        {
            try
            {
                return new DBFRAUDE().InsertLesFraude(sFraude);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }
        #endregion

        #region Denonciation
        public bool InsertDenoncition(List<CsDenonciateur> sDenonciation)
        {
            try
            {
                return new DBFRAUDE().InsertDenonciation(sDenonciation);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool UpdateDenoncition(List<CsDenonciateur> sDenonciation)
        {
            try
            {
                return new DBFRAUDE().UpdateDenonciation(sDenonciation);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }
        #endregion

        #region Client Fraude
        public CsClientFraude InsertClientFraude(List<CsClientFraude> sClientFraude)
        {
            try
            {


                return new DBFRAUDE().InsertClientFraude(sClientFraude);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                throw ex;
            }
        }
        public List<CsClientFraude> RetourneClientFraude(string identifieClient, int pkCentre)
        {
            try
            {

                return new DBFRAUDE().SelectAllClientFraude(identifieClient, pkCentre);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                throw ex;
            }
        }

        #endregion

        #region Controle
        public bool InsertControle(List<CsControle> sControle)
        {
            try
            {
                return new DBFRAUDE().InsertControle(sControle);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool UpdateControle(List<CsControle> sControle)
        {
            try
            {
                return new DBFRAUDE().UpdateControle(sControle);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public List<CsControle> SelectAllControle()
        {
            try
            {
                return new DBFRAUDE().SelectAllControle();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }
        #endregion

        #region Controleur
        public bool InsertControleur(List<CsControleur> sControleur)
        {
            try
            {
                return new DBFRAUDE().InsertControleur(sControleur);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool UpdateControleur(List<CsControleur> sControleur)
        {
            try
            {
                return new DBFRAUDE().UpdateControleur(sControleur);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public List<CsControleur> SelectAllControleur()
        {
            try
            {
                return new DBFRAUDE().SelectAllControleur();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }
        #endregion

        #region Compteur fraude
        public bool InsertCompteurFraude(List<CsCompteurFraude> sCompteurFraude)
        {
            try
            {
                return new DBFRAUDE().InsertCompteurFraude(sCompteurFraude);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool UpdateCompteurFraude(List<CsCompteurFraude> sCompteurFraude)
        {
            try
            {
                return new DBFRAUDE().UpdateCompteurFraude(sCompteurFraude);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public List<CsCompteurFraude> SelectAllCompteurFraude()
        {
            try
            {
                return new DBFRAUDE().SelectAllCompteurFraude();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }
        #endregion

        public List<CsUsage> SelectAllUsage()
        {
            try
            {
                return new DBFRAUDE().SelectAllUsage();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public List<CsTypeFraude> SelectAllTypeFraude()
        {
            try
            {
                return new DBFRAUDE().SelectAllTypeFraude();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public List<CsPhaseCompteur> SelectAllPhaseCompteur()
        {
            try
            {
                return new DBFRAUDE().SelectAllPhaseCompteur();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public List<CsMArqueDisjoncteur> SelectAllMarqueDisjoncteur()
        {
            try
            {
                return new DBFRAUDE().SelectAllMarqueDisjoncteur();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public List<CsActionSurCompteur> SelectAllActionSurCompteur()
        {
            try
            {
                return new DBFRAUDE().SelectAllActionSurCompteur();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public List<CsOrganeFraude> SelectAllOrganeFraude()
        {
            try
            {
                return new DBFRAUDE().SelectAllOrganeFraude();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public List<CsQualiteExpert> SelectAllQualiteExpert()
        {
            try
            {
                return new DBFRAUDE().SelectAllQualiteExpert();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }
        public List<CsDecisionfrd> SelectAllDecision()
        {
            try
            {
                return new DBFRAUDE().SelectAllDecisionfrd();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }


        public bool InsertControleFraude(CsDemandeFraude sDemandeFraude)
        {
            try
            {
                return new DBFRAUDE().InsertControleFraude(sDemandeFraude);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool InsertionFraudeAudite(CsDemandeFraude sDemandeFraude)
        {
            try
            {
                return new DBFRAUDE().InsertionFraudeAudite(sDemandeFraude);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool InsertionFraudeConsommation(CsDemandeFraude sDemandeFraude)
        {
            try
            {
                return new DBFRAUDE().InsertionFraudeConsommation(sDemandeFraude);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }
        public string ValiderDemandeInitailisation(CsDemandeFraude LaDemannde)
        {

            try
            {
                return new DBFRAUDE().ValiderDemandeFraude(LaDemannde);

            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return string.Empty;
            }
        }

        public CsDemandeFraude RetourDemandeFraude(int IDDEMANDE)
        {
            return new DBFRAUDE().RetourneLaDemande(IDDEMANDE);
        }

        public string ValiderDemandeControle(CsDemandeFraude LaDemannde)
        {
            return new DBFRAUDE().ValiderDemandeControle(LaDemannde);
        }
        public string ValiderDemandeAudition(CsDemandeFraude LaDemannde)
        {
            return new DBFRAUDE().ValiderDemandeAudition(LaDemannde);
        }
        public string ValiderDemandeConsommation(CsDemandeFraude LaDemannde)
        {
            return new DBFRAUDE().ValiderDemandeConsommation(LaDemannde);
        }
        public string ValiderDemandeEmissionFacture(CsDemandeFraude LaDemannde)
        {
            return new DBFRAUDE().ValiderDemandeEmissionFacture(LaDemannde);
        }
        public string ValiderDemandeControleIndex(CsDemandeFraude LaDemannde)
        {
            return new DBFRAUDE().ValiderDemandeControleIndex(LaDemannde);
        }
        public List<CsEvenement> ConsommationByPeriodeMois(CsDemandeFraude laDemande, int mois, string periode)
        {
            return new DBFRAUDE().ConsommationByPeriodeMois(laDemande, mois, periode);
        }
        #endregion

        public List<CsEvenement> RetourneEvenement(int fk_idcentre, string centre, string client, string Ordre)
        {
            try
            {

                DBFRAUDE db = new DBFRAUDE();
                return db.RetourneEvenement(fk_idcentre, centre, client, Ordre);
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }


    }
}
