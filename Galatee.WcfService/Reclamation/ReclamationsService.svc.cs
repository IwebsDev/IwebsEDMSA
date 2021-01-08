using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Galatee.Structure;
using Galatee.DataAccess;
using System.ServiceModel.Activation;
//using Microsoft.Reporting.WebForms;
using System.IO;
using System.Web.Hosting;
using System.Data.SqlClient;


namespace WcfService.Reclamation
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "ReclamationsService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select ReclamationsService.svc or ReclamationsService.svc.cs at the Solution Explorer and start debugging.
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class ReclamationsService : IReclamationsService
    {

        #region TypeReclamationRcl
        public List<CsTypeReclamationRcl> SelectAllTypeReclamationRcl()
        {
            try
            {
                return new DBRECLAMATION().SelectAllTypeReclamationRcl();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }
        #endregion

        #region Validation
        public List<CsRclValidation> SelectAllValidation()
        {
            try
            {
                return new DBRECLAMATION().SelectAllValidation();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }
        #endregion

         


        #region ReclamationRcl
        public List<CsReclamationRcl> SelectAllReclamationRcl()
        {
            try
            {
                return new DBRECLAMATION().SelectAllReclamationRcl();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }
        #endregion

        #region Validation init
        public string ValiderInitReclamation(CsDemandeReclamation LaDemande)
        {
            return new DBRECLAMATION().ValiderInitReclamation(LaDemande);
        }
        #endregion

        #region Valider
        public bool  ValiderReclamation(CsDemandeReclamation LaDemande)
        {
            return new DBRECLAMATION().ValiderReclamation(LaDemande);
        }
        #endregion

        #region RetourinfoReclamation
        public CsDemandeReclamation RetourDemandeReclamation(int IDDEMANDE)
        {
            return new DBRECLAMATION().RetourneLaDemande(IDDEMANDE);
        }
        #endregion

    
        public List<CsReclamationRcl> RetourneReclamation(int fk_idcentre, string centre, string client, string ordre,string numerodeamnde)
        {
            try
            {
                DBRECLAMATION db = new DBRECLAMATION();
                return db.RetourneReclamation(fk_idcentre, centre, client, ordre, numerodeamnde);
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                throw ex;
            }
        }
        public List<cStatistiqueReclamation> RetourStatistiqueReclamation(DateTime dateDebut, DateTime dateFin, List<int> lstCentre)
        {
            return new DBRECLAMATION().RetourStatistiqueReclamation(lstCentre, dateDebut, dateFin);
        }
        public List<CsReclamationRcl> ReclamationParAgent(DateTime dateDebut, DateTime dateFin, List<int> lstCentre)
        {
            return new DBRECLAMATION().ReclamationParAgentSpx(dateDebut, dateFin, lstCentre);
        }
        public List<CsReclamationRcl> ListDesReclamation(DateTime dateDebut, DateTime dateFin, List<int> lstCentre, List<int> TypeReclamation)
        {
            return new DBRECLAMATION().ListDesReclamationSpx(dateDebut, dateFin, lstCentre, TypeReclamation);
        }
        public List<cStatistiqueReclamation> SuiviTauxTraitement(DateTime dateDebut, DateTime dateFin, List<int> lstidcentre)
        {
            return new DBRECLAMATION().SuiviTauxTraitementSpx(dateDebut, dateFin, lstidcentre);
        }

        #region Fraude
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


        #region ModeReception
        public List<CsModeReception> SelectAllModeReception()
        {
            try
            {
                return new DBRECLAMATION().SelectAllModeReception();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }
        #endregion
    }
}
