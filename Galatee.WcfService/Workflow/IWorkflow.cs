using Galatee.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Galatee.WorkflowManager;

namespace WcfService.Workflow
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IWorkflow" in both code and config file together.
    [ServiceContract]
    public interface IWorkflow
    {

        [OperationContract]
        [FaultContract(typeof(Errors))]
        string InsererMaDemande(int centreId, Guid workflowId, Guid OpId, string IDVotreLigne, string MatriculeUser,
            string CodeDeVotreDemande);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        string InsererMaDemandeToGroupeValidation(int centreId, Guid workflowId, Guid OpId, Guid IdGroupeValidation, string IDVotreLigne, string MatriculeUser,
            string CodeDeVotreDemande);




        [OperationContract]
        [FaultContract(typeof(Errors))]
        string ExecuterActionSurDemande(string CodeDemande, int CodeAction, string MatriculeUser, string Commentaire);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        string AllerALEtape(string CodeDemande, int CodeAction, Guid EtapeId, string MatriculeUser, string Commentaire);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        string ExecuterActionSurPlusieursDemandes(List<string> CodesDemandes, int CodeAction, string MatriculeUser, string Commentaire);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        string ExecuterActionSurDemandeParPkIDLigne(List<int> pkIds, int FkidEtapeActuelle, int CodeAction, string MatriculeUser, string Commentaire);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool VerifierConditionDemande(string codeDemande, int FKIDTableTravail, string PKIDLigne);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        CsInfoDemandeWorkflow RecupererInfoDemandeParOperationId(string codeDemande, Guid Operation);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        CsInfoDemandeWorkflow RecupererInfoDemandeParCodeTDem(CsDemandeBase lademande);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        void NotificationMail(List<string> lstDestinataire, string CodeTypeMail);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        void NotificationMailDemande(List<string> lstDestinataire, string NumeroDemande, string TypeDemande, string CodeTypeMail);

         
    }
}
