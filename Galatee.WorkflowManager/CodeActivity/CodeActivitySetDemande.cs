using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;
using Galatee.DataAccess;
using Galatee.Entity;
using Galatee.Structure;

namespace Galatee.WorkflowManager
{

    public sealed class CodeActivitySetDemande : CodeActivity<bool>
    {
        #region Parametres

        public InArgument<int> CentreId { get; set; }
        public InArgument<Guid> WKFId { get; set; }
        public InArgument<Guid> OpId { get; set; }
        public InArgument<string> PKIDLine { get; set; }
        public InArgument<string> MatriculeUser { get; set; }
        public InArgument<string> CodeDemandeTravail { get; set; }

        #endregion

        #region Résultat

        public OutArgument<string> CodeDemande { get; set; }
        public OutArgument<Guid> PKRWKF { get; set; }
        public OutArgument<Guid> PKIDDemande { get; set; }
        public OutArgument<string> Emails { get; set; }

        #endregion

        // If your activity returns a value, derive from CodeActivity<TResult>
        // and return the value from the Execute method.
        protected override bool Execute(CodeActivityContext context)
        {
            bool _result = true;
            try
            {
                //On obtient les valeurs des paramètres
                string pk_IDLine = context.GetValue<string>(this.PKIDLine);
                int cId = context.GetValue<int>(this.CentreId);
                Guid _wkfId = context.GetValue<Guid>(this.WKFId);
                Guid _opId = context.GetValue<Guid>(this.OpId);
                string _matUser = context.GetValue<string>(this.MatriculeUser);
                string _codeDemandeTravail = context.GetValue<string>(this.CodeDemandeTravail);

                //On recherche les infos sur le circuit 
                CsRWorkflow rWKFCentre = null;
                rWKFCentre = new DB_WORKFLOW().SelectAllRWorkflowCentre(_wkfId, cId, _opId)
                    .FirstOrDefault();
                if (null != rWKFCentre)
                {
                    //Récupération des infos
                    CsCentre centre = new DB_Centre().SelectAllCentre().Where(c => c.PK_ID == cId).FirstOrDefault();
                    CsWorkflow workflow = new DB_WORKFLOW().SelectAllWorkflows().Where(w => w.PK_ID == _wkfId)
                        .FirstOrDefault();
                    CsOperation operation = new DB_WORKFLOW().SelectAllOperation2().Where(o => o.PK_ID == _opId)
                        .FirstOrDefault();

                    //Récupération du circuit
                    Dictionary<CsRAffectationEtapeWorkflow, CsConditionBranchement> lsRAffCircuit = new Dictionary<CsRAffectationEtapeWorkflow, CsConditionBranchement>();
                    lsRAffCircuit = new DB_WORKFLOW().SelectAllAffectationEtapeWorkflow(rWKFCentre.PK_ID);

                    List<CsRAffectationEtapeWorkflow> leCircuitNormal = lsRAffCircuit.Keys.Where(aff => !aff.FK_IDRETAPEWORKFLOWORIGINE.HasValue ||
                        aff.FK_IDRETAPEWORKFLOWORIGINE.Value == Guid.Empty)
                        .OrderBy(aff => aff.ORDRE)
                        .ToList();

                    CsRAffectationEtapeWorkflow _1sStep = leCircuitNormal.FirstOrDefault();
                    CsRAffectationEtapeWorkflow _2ndStep = leCircuitNormal.Where(step => step.ORDRE == _1sStep.ORDRE + 1)
                        .FirstOrDefault();                    

                    //Création de la demande
                    CsDemandeWorkflow dmd = new CsDemandeWorkflow()
                    {
                        PK_ID = Guid.NewGuid(),
                        DATECREATION = DateTime.Today.Date,
                        MATRICULEUSERCREATION = _matUser,
                        ALLCENTRE = false,
                        FK_IDCENTRE = cId,
                        FK_IDOPERATION = _opId,
                        FK_IDRWORKLOW = rWKFCentre.PK_ID,
                        FK_IDSTATUS = (int)STATUSDEMANDE.Initiee,
                        FK_IDWORKFLOW = _wkfId,
                        FK_IDLIGNETABLETRAVAIL = pk_IDLine,
                        FK_IDETAPEPRECEDENTE = 0,
                        FK_IDETAPEACTUELLE = _1sStep.FK_IDETAPE,
                        FK_IDETAPESUIVANTE = _2ndStep.FK_IDETAPE,
                        CODE = centre.CODESITE + centre.CODE + DateTime.Today.Year + DateTime.Today.Month + 
                            DateTime.Now.Minute +
                            DateTime.Now.Millisecond,
                        FK_IDTABLETRAVAIL = workflow.FK_IDTABLE_TRAVAIL.Value,
                        CODE_DEMANDE_TABLETRAVAIL = _codeDemandeTravail,
                        DATEDERNIEREMODIFICATION = DateTime.Today.Date
                    };


                    _result = new DB_WORKFLOW().InsertDemande(new List<CsDemandeWorkflow>() { dmd });

                    if (_result) /* tout es bon */
                    {
                        CodeDemande.Set(context, dmd.CODE);
                        PKIDDemande.Set(context, dmd.PK_ID);
                        PKRWKF.Set(context, dmd.FK_IDRWORKLOW);
                        
                        //On récupère les emails pour notifier les utilisateurs de l'arrivée de la demande
                        KeyValuePair<CsGroupeValidation, List<CsRHabilitationGrouveValidation>> grpValidation = new DB_WORKFLOW().SelectAllGroupeValidation()
                            .Where(g => g.Key.PK_ID == _1sStep.FK_IDGROUPEVALIDATIOIN)
                            .FirstOrDefault();

                        if (null != grpValidation.Key)
                        {
                            if (string.Empty != grpValidation.Key.EMAILDIFFUSION) 
                                Emails.Set(context, grpValidation.Key.EMAILDIFFUSION + ";");
                            else
                            {
                                string _email = string.Empty;
                                foreach (var habilUser in grpValidation.Value) _email += habilUser.EMAIL + ";";                                   

                                Emails.Set(context, _email);
                            }
                        }
                    }
                }
                else
                {
                    _result = false;
                    CodeDemande.Set(context, "Aucun circuit n'a été configuré pour cette opération et ce centre");
                    PKIDDemande.Set(context, Guid.Empty);
                    PKRWKF.Set(context, Guid.Empty);
                }
            }
            catch (Exception ex)
            {
                _result = false;
                CodeDemande.Set(context, ex.Message);
            }


            return _result;

        }
    }
}
