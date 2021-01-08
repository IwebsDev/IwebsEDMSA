using Galatee.DataAccess;
using Galatee.Structure;
using Galatee.WorkflowManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace WcfService.Workflow
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Workflow" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Workflow.svc or Workflow.svc.cs at the Solution Explorer and start debugging.
    public class Workflow : IWorkflow
    {
        public string InsererMaDemande(int centreId, Guid workflowId, Guid OpId, string IDVotreLigne, string MatriculeUser,
                               string CodeDeVotreDemande)
        {
            string _CodeDemande = string.Empty;
            bool _result = true;
            try
            {
                return new DbWorkFlow().InsererMaDemande(centreId, workflowId, OpId, IDVotreLigne, MatriculeUser, CodeDeVotreDemande); 
            }
            catch (Exception ex)
            {
                _result = false;
                _CodeDemande = ex.Message;
            }

            if (!_result) _CodeDemande = "ERR : " + _CodeDemande;

            return _CodeDemande;
        }

        public string InsererMaDemandeToGroupeValidation(int centreId, Guid workflowId, Guid OpId, Guid IdGroupeValidation, string IDVotreLigne, string MatriculeUser,
    string CodeDeVotreDemande)
        {
            string _CodeDemande = string.Empty;
            bool _result = true;
            try
            {
                return new DbWorkFlow().InsererMaDemandeToGroupeValidation(centreId, workflowId, OpId, IdGroupeValidation, IDVotreLigne, MatriculeUser, CodeDeVotreDemande); 
            }
            catch (Exception ex)
            {
                _result = false;
                _CodeDemande = ex.Message;
            }

            if (!_result) _CodeDemande = "ERR : " + _CodeDemande;

            return _CodeDemande;
        }

 
         
        public string ExecuterActionSurDemande(string CodeDemande, int CodeAction, string MatriculeUser, string Commentaire)
        {
            return new DbWorkFlow().ExecuterActionSurDemande(CodeDemande, CodeAction, MatriculeUser, Commentaire); 

        }
        public string AllerALEtape(string CodeDemande, int CodeAction, Guid EtapeId, string MatriculeUser, string Commentaire)
        {
            Galatee.DataAccess.RESULTACTION _result = Galatee.DataAccess.RESULTACTION.ERREURINCONNUE;
            string Reponse = string.Empty;

            try
            {
                return new DbWorkFlow().AllerALEtape(CodeDemande, CodeAction, EtapeId,MatriculeUser, Commentaire); 
            }
            catch (Exception ex)
            {
                _result = Galatee.DataAccess.RESULTACTION.ERREURINCONNUE;
                Reponse = "ERR : " + ex.Message;
            }

            return Reponse;
        }


        public string ExecuterActionSurPlusieursDemandes(List<string> CodesDemandes, int CodeAction, string MatriculeUser, string Commentaire)
        {
            return new DbWorkFlow().ExecuterActionSurPlusieursDemandes(CodesDemandes, CodeAction, MatriculeUser, Commentaire); 
        }

        public string ExecuterActionSurDemandeParPkIDLigne(List<int> pkIds, int FkidEtapeActuelle, int CodeAction, string MatriculeUser, string Commentaire)
        {
            return new DbWorkFlow().ExecuterActionSurDemandeParPkIDLigne(pkIds, FkidEtapeActuelle, CodeAction, MatriculeUser, Commentaire); 
        }

        public bool VerifierConditionDemande(string codeDemande, int FKIDTableTravail, string PKIDLigne)
        {
            try
            {
                //Récupération des infos de la table de travail
                return new DbWorkFlow().VerifierConditionDemande(codeDemande, FKIDTableTravail, PKIDLigne); 
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public CsInfoDemandeWorkflow RecupererInfoDemandeParOperationId(string codeDemande, Guid Operation)
        {
            try
            {
                return new DbWorkFlow().RecupererInfoDemandeParOperationId(codeDemande, Operation); 
                 
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public CsInfoDemandeWorkflow RecupererInfoDemandeParCodeTDem(CsDemandeBase laDemande)
        {
            try
            {
                //On sélection l'opération
                return new DB_WORKFLOW().RecupererInfoDemandeParCodeDemande(laDemande);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void  NotificationMail(List<string > lstDestinataire, string CodeTypeMail)
        {
            CsParametreSMTP leServerMail = new DB_NOTIFICATION().SelectAllSMTP().FirstOrDefault();
            CsNotificaton  leMAil = new DB_NOTIFICATION().SelectNotificationByTypeMail(CodeTypeMail).FirstOrDefault();
            if (leServerMail != null && leMAil != null )
            {
                leMAil.SERVEURSMTP = leServerMail.PASSWORD;
                leMAil.PASSWORD  = leServerMail.SERVEURSMTP;
                leMAil.PORT = leServerMail.PORT;
                leMAil.SSL = leServerMail.SSL;
                leMAil.LOGIN = leServerMail.LOGIN;
                //Galatee.Tools.Utility.EnvoiMail(leMAil.SERVEURSMTP, leMAil.LOGIN, leMAil.PORT, leMAil.PASSWORD, leMAil.OBJET, leMAil.MESSAGE, leMAil.SSL.Value, lstDestinataire);
            }
        }
        public void NotificationMailDemande(List<string> lstDestinataire, string NumeroDemande, string TypeDemande, string CodeTypeMail)
        {
            try
            {
                foreach (var item in lstDestinataire)
                    ErrorManager.WriteInLogFile(this, item); 
  
                 new DbWorkFlow().NotificationMailDemande(lstDestinataire, NumeroDemande, TypeDemande, CodeTypeMail);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
            }
        }
    }
}
