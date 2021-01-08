using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;
using Galatee.Structure;
using Galatee.DataAccess;

namespace Galatee.WorkflowManager
{

    public sealed class CodeActivityAnnulerSuspendre : CodeActivity<RESULTACTION>
    {
        #region In Parametres

        public InArgument<string> CodeDemande { get; set; }
        public InArgument<CODEACTION> CodeAction { get; set; }
        public InArgument<string> MatriculeUser { get; set; }

        #endregion

        #region Out Parameter

        public OutArgument<string> MessageErreur { get; set; }

        #endregion

        // If your activity returns a value, derive from CodeActivity<TResult>
        // and return the value from the Execute method.
        protected override RESULTACTION  Execute(CodeActivityContext context)
        {
            RESULTACTION result = RESULTACTION.ERREURINCONNUE;

            string _codeDemande = context.GetValue<string>(CodeDemande);
            string _matricule = context.GetValue<string>(MatriculeUser);
            DB_WORKFLOW dbWKF = new DB_WORKFLOW();

            CsDemandeWorkflow dmd = dbWKF.SelectLaDemande(_codeDemande);
            if (null != dmd)
            {

                CODEACTION codeA = context.GetValue<CODEACTION>(CodeAction);
                KeyValuePair<CsCopieDmdCircuit, CsCopieDmdConditionBranchement> EtapeCourrante = dbWKF.RecupererEtapeCourante(dmd.CODE);
                string _actionOnDemande = string.Empty;
                if (null != EtapeCourrante.Key)
                {
                    try
                    {
                        if (codeA == CODEACTION.ANNULER)
                        {
                            //On annule la demande
                            dmd.FK_IDSTATUS = (int)STATUSDEMANDE.Annulee;
                            _actionOnDemande = "Annulation  à l'étape " + EtapeCourrante.Key.LIBELLEETAPE;
                            result = RESULTACTION.ANNULEE;
                        }
                        else if (codeA == CODEACTION.SUSPENDRE)
                        {
                            //On suspend la demande
                            dmd.FK_IDSTATUS = (int)STATUSDEMANDE.Suspendue;
                            _actionOnDemande = "Suspendue à l'étape " + EtapeCourrante.Key.LIBELLEETAPE;
                            result = RESULTACTION.SUSPENDUE;
                        }

                        dmd.DATEDERNIEREMODIFICATION = DateTime.Now;
                        //On met à jour la demande
                        bool insertion = dbWKF.UpdateDemande(new List<CsDemandeWorkflow>() { dmd });

                        if (insertion)
                        {
                            //Maintenant on met le journal
                            CsJournalDemandeWorkflow jrnal = new CsJournalDemandeWorkflow();
                            jrnal.PK_ID = Guid.NewGuid();
                            jrnal.CODE_DEMANDE = dmd.CODE;
                            jrnal.FK_IDDEMANDE = dmd.PK_ID;
                            jrnal.LIBELLEACTION = _actionOnDemande;
                            jrnal.DATEACTION = DateTime.Today.Date;
                            jrnal.OBSERVATIONS = string.Empty;
                            jrnal.MATRICULEUSERACTION = _matricule;

                            dbWKF.InsertJournalDemande(new List<CsJournalDemandeWorkflow>() { jrnal });
                            MessageErreur.Set(context, "");                            
                        }
                        else
                        {
                            MessageErreur.Set(context, "Une erreur interne est survenue");
                            result = RESULTACTION.ERREURINCONNUE;
                        }                        
                    }
                    catch (Exception ex)
                    {
                        MessageErreur.Set(context, ex.Message);
                        result = RESULTACTION.ERREURINCONNUE;
                    }
                }
                else
                {
                    MessageErreur.Set(context, "Impossible d'effectuer une action sur une demande n'étant pas à une étape");
                    result = RESULTACTION.ERREURINCONNUE;
                }
            }
            else
            {
                MessageErreur.Set(context, "Impossible d'annuler une demande nulle");
                result = RESULTACTION.ERREURINCONNUE;
            }

            return result;
        }
    }
}
