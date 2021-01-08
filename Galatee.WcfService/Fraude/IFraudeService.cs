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
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IFraudeService" in both code and config file together.
    [ServiceContract]
    public interface IFraudeService
    {
        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsSourceControle> SelectAllSourceControle();
        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsMoyenDenomciation> SelectAllMoyenDenomciation();
        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsClient> SelectAllClient();
        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsClient> RetourneClient(int fk_idcentre, string centre, string client, string Ordre);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        int InsertFraudeDenociateur(CsDemandeFraude sDemandeFraude);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool InsertFraude(CsFraude sFraude);
        [FaultContract(typeof(Errors))]
        bool InsertLesFraude(List<CsFraude> sFraude);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool InsertDenoncition(List<CsDenonciateur> sDenonciation);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        CsClientFraude InsertClientFraude(List<CsClientFraude> sClientFraude);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsClientFraude> RetourneClientFraude(string identifieClient, int pkCentre);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsTypeFraude> SelectAllTypeFraude();
        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsMArqueDisjoncteur> SelectAllMarqueDisjoncteur();
        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsActionSurCompteur> SelectAllActionSurCompteur();
        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsOrganeFraude> SelectAllOrganeFraude();
        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsQualiteExpert> SelectAllQualiteExpert();
        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsPhaseCompteur> SelectAllPhaseCompteur();
        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsDecisionfrd> SelectAllDecision();
        #region controle
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool InsertControle(List<CsControle> sControle);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool UpdateControle(List<CsControle> sControle);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsControle> SelectAllControle();
        #endregion

        #region controleur
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool InsertControleur(List<CsControleur> sControleur);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool UpdateControleur(List<CsControleur> sControleur);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsControleur> SelectAllControleur();
        #endregion

        #region CompteurFraude
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool InsertCompteurFraude(List<CsCompteurFraude> sCompteurFraude);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool UpdateCompteurFraude(List<CsCompteurFraude> sCompteurFraude);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsCompteurFraude> SelectAllCompteurFraude();
        #endregion
        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsUsage> SelectAllUsage();
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool InsertControleFraude(CsDemandeFraude sDemandeFraude);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool InsertionFraudeAudite(CsDemandeFraude sDemandeFraude);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool InsertionFraudeConsommation(CsDemandeFraude sDemandeFraude);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        string ValiderDemandeInitailisation(CsDemandeFraude LaDemannde);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        CsDemandeFraude RetourDemandeFraude(int IDDEMANDE);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        string ValiderDemandeControle(CsDemandeFraude LaDemannde);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        string ValiderDemandeAudition(CsDemandeFraude LaDemannde);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        string ValiderDemandeConsommation(CsDemandeFraude LaDemannde);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        string ValiderDemandeEmissionFacture(CsDemandeFraude LaDemannde);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        string ValiderDemandeControleIndex(CsDemandeFraude LaDemannde);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsEvenement> RetourneEvenement(int fk_idcentre, string centre, string client, string Ordre);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsEvenement> ConsommationByPeriodeMois(CsDemandeFraude laDemande, int mois, string periode);
    }
}
