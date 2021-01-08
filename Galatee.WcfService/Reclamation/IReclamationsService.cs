using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Galatee.Structure;
using Galatee.DataAccess;

namespace WcfService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IReclamationsService" in both code and config file together.
    [ServiceContract]
    public interface IReclamationsService
    {
        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsTypeReclamationRcl> SelectAllTypeReclamationRcl();

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsModeReception> SelectAllModeReception();

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsRclValidation> SelectAllValidation();

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsReclamationRcl> SelectAllReclamationRcl();

        [OperationContract]
        [FaultContract(typeof(Errors))]
        string ValiderInitReclamation(CsDemandeReclamation LaDemande);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool ValiderReclamation(CsDemandeReclamation LaDemande);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsReclamationRcl> RetourneReclamation(int fk_idcentre, string centre, string client, string ordre, string numerodeamnde);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        CsDemandeReclamation RetourDemandeReclamation(int IDDEMANDE);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<cStatistiqueReclamation> RetourStatistiqueReclamation(DateTime dateDebut, DateTime dateFin, List<int> lstCentre);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsReclamationRcl> ReclamationParAgent(DateTime dateDebut, DateTime dateFin, List<int> lstCentre);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsReclamationRcl> ListDesReclamation(DateTime dateDebut, DateTime dateFin, List<int> lstCentre, List<int> TypeReclamation);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<cStatistiqueReclamation> SuiviTauxTraitement(DateTime dateDebut, DateTime dateFin, List<int> lstidcentre);

        #region FRAUDE
        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsClient> RetourneClient(int fk_idcentre, string centre, string client, string Ordre);
        #endregion
    }
}
