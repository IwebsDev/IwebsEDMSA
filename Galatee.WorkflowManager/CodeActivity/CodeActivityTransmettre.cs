using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;
using Galatee.DataAccess;
using Galatee.Structure;

namespace Galatee.WorkflowManager
{

    public sealed class CodeActivityTransmettre : CodeActivity<RESULTACTION>
    {

        #region In Parametres

        public InArgument<string> CodeDemande { get; set; }
        public InArgument<string> MatriculeUser { get; set; }

        #endregion

        #region Out Parameter

        public OutArgument<string> MessageErreur { get; set; }

        #endregion

        // If your activity returns a value, derive from CodeActivity<TResult>
        // and return the value from the Execute method.
        protected override RESULTACTION Execute(CodeActivityContext context)
        {
            RESULTACTION _resultAction = RESULTACTION.ERREURINCONNUE;

            string _codeDemande = context.GetValue<string>(CodeDemande);
            string _matricule = context.GetValue<string>(MatriculeUser);
            string _actionOnDemande = string.Empty;
            //On va mettre transmettre la demande à l'étape suivante
            try
            {
                DB_WORKFLOW dbWKF = new DB_WORKFLOW();

                //On récupère toutes les infos de la demande
                CsDemandeWorkflow dmdWorkflow = dbWKF.SelectLaDemande(_codeDemande);
                if (null != dmdWorkflow)
                {
                    //Infos sur la table de travail
                    CsTableDeTravail table = dbWKF.SelectAllTableDeTravail().Where(t => t.PK_ID == dmdWorkflow.FK_IDTABLETRAVAIL)
                        .FirstOrDefault();

                    //La demande doit être forcement liée à une table de travail                    
                    //if (null != table)        --> Une opération peut ne peut avoir une table de travail
                    //{
                        //Avant de transmettre  la demande on cherche les infos de l'étape actuelle
                        KeyValuePair<CsCopieDmdCircuit, CsCopieDmdConditionBranchement> CurrentStep = dbWKF.RecupererEtapeCourante(_codeDemande);
                        if (null != CurrentStep.Key)
                        {
                            //On verifie la condition actuelle de l'étape
                            bool IlYaUneCondition = (null != CurrentStep.Value
                                && (null != CurrentStep.Key.CONDITION && string.Empty != CurrentStep.Key.CONDITION));

                            bool transmissionOk = true;
                            bool LastStep = false;

                            string msgErr = string.Empty;
                            if (IlYaUneCondition)
                            {
                                //On utilise la condition pour transmettre
                                bool onABienTeste = true;                                
                                bool conditionRespecte = ConditionChecker.CheckIfConditionIsRespected(CurrentStep.Key.CONDITION,
                                    table.TABLE_NAME, ref msgErr, dmdWorkflow.FK_IDLIGNETABLETRAVAIL, ref onABienTeste);
                                if (onABienTeste)
                                {
                                    transmissionOk = dbWKF.TransmettreDemande(_codeDemande, dmdWorkflow.FK_IDETAPEACTUELLE, ref msgErr, ref LastStep,
                                            ref dmdWorkflow, conditionRespecte);
                                }
                                else transmissionOk = false;
                            }
                            else
                            {
                                //On transmet sans condition
                                transmissionOk = dbWKF.TransmettreDemande(_codeDemande, dmdWorkflow.FK_IDETAPEACTUELLE, ref msgErr, ref LastStep,
                                    ref dmdWorkflow, false);
                            }
                            if (transmissionOk)
                            {
                                //On a bien transmis,
                                dmdWorkflow.FK_IDSTATUS = (int)STATUSDEMANDE.EnAttenteValidation;   //Pour la prochaine étape
                                dmdWorkflow.DATEDERNIEREMODIFICATION = DateTime.Now;
                                _resultAction = RESULTACTION.TRANSMISE;


                                /** ZEG 29/08/2017 **/
                                //_actionOnDemande = "Validation à l'étape " + CurrentStep.Key.LIBELLEETAPE;
                                CsEtape _etape = dbWKF.SelectAllEtapes().Find(e => e.PK_ID == dmdWorkflow.FK_IDETAPEPRECEDENTE);
                                _actionOnDemande = "Etape " + _etape.NOM + " traitée";
                                /****/

                                
                                
                                MessageErreur.Set(context, "");

                                #region Mise à jour de la demande et insertion des journaux
                                //On met à jour la demande
                                bool update = dbWKF.UpdateDemande(new List<CsDemandeWorkflow>() { dmdWorkflow });
                                if (update)
                                {
                                    //maintenant on écrit dans le journal de la demande
                                    CsJournalDemandeWorkflow jrnal = new CsJournalDemandeWorkflow();
                                    jrnal.PK_ID = Guid.NewGuid();
                                    jrnal.CODE_DEMANDE = dmdWorkflow.CODE;
                                    jrnal.FK_IDDEMANDE = dmdWorkflow.PK_ID;
                                    jrnal.LIBELLEACTION = _actionOnDemande;
                                    jrnal.DATEACTION = DateTime.Today.Date;
                                    jrnal.OBSERVATIONS = string.Empty;
                                    jrnal.MATRICULEUSERACTION = _matricule;

                                    dbWKF.InsertJournalDemande(new List<CsJournalDemandeWorkflow>() { jrnal });

                                    //Suppresion de la demande dans la table des affectations user
                                    dbWKF.SupprimerAffectationDemande(dmdWorkflow.CODE);
                                }
                                else
                                {
                                    MessageErreur.Set(context, "Une erreur interne est survenue");
                                    _resultAction = RESULTACTION.ERREURINCONNUE;
                                }

                                #endregion

                            }
                            else
                            {
                                //On na pas bien transmis, donc on check si on est déjà à la dernière étape
                                if (LastStep)
                                {
                                    //Bon la, la demande est terminée oh, donc après on va parser la table pour la purger
                                    //si le paramètre SUPPRIMER_DEMANDE_TERMINE est à OUI
                                    dmdWorkflow.FK_IDSTATUS = (int)STATUSDEMANDE.Terminee;
                                    dmdWorkflow.DATEDERNIEREMODIFICATION = DateTime.Now;
                                    _resultAction = RESULTACTION.FINDECIRCUIT;

                                    //Si on est à la fin du circuit, on n'a plus de journaux à insérer,
                                    //car aucune action n'a été faite en principe, mais on va juste insérer
                                    //demande terminée par ... le ... à l'étape ...
                                    bool update = dbWKF.UpdateDemande(new List<CsDemandeWorkflow>() { dmdWorkflow });
                                    if (update)
                                    {
                                        /** ZEG 29/08/2017 **/
                                        CsEtape _etape2 = dbWKF.SelectAllEtapes().Find(e => e.PK_ID == dmdWorkflow.FK_IDETAPEPRECEDENTE);
                                        
                                        //Insertion du journal
                                        CsJournalDemandeWorkflow jrnal = new CsJournalDemandeWorkflow()
                                        {
                                            CODE_DEMANDE = dmdWorkflow.CODE,
                                            DATEACTION = DateTime.Today.Date,
                                            FK_IDDEMANDE = dmdWorkflow.PK_ID,
                                            PK_ID = Guid.NewGuid(),

                                            /** ZEG 29/08/2017 **/
                                            //LIBELLEACTION = "Dernière validation à l'étape " + CurrentStep.Key.LIBELLEETAPE,
                                            LIBELLEACTION = "Dernière étape " + _etape2.NOM + " validée",

                                            
                                            MATRICULEUSERACTION = _matricule,
                                            OBSERVATIONS = string.Empty
                                        };

                                        dbWKF.InsertJournalDemande(new List<CsJournalDemandeWorkflow>() { jrnal });

                                        //Suppresion de la demande dans la table des affectations user
                                        dbWKF.SupprimerAffectationDemande(dmdWorkflow.CODE); 
                                    }
                                    MessageErreur.Set(context, "");
                                }
                                else
                                {
                                    //La la, ya vraiment une erreur qui s'est produite
                                    _resultAction = RESULTACTION.ERREURINCONNUE;
                                    MessageErreur.Set(context, msgErr);
                                }
                            }

                            
                            return _resultAction;
                        }
                        else
                        {
                            MessageErreur.Set(context, "Impossible d'avancer une demande n'étant à aucune étape");
                            return RESULTACTION.ERREURINCONNUE;
                        }
                    //}
                    //else
                    //{
                    //    MessageErreur.Set(context, "Impossible de transmettre une demande n'étant pas liée à une table de travail");
                    //    return RESULTACTION.ERREURINCONNUE;
                    //}
                }
                else
                {
                    MessageErreur.Set(context, "Impossible de transmettre une demande qui n'existe pas");
                    return RESULTACTION.ERREURINCONNUE;
                }
            }
            catch (Exception ex)
            {
                MessageErreur.Set(context, ex.Message);
                return RESULTACTION.ERREURINCONNUE;
            }

        }
    }
}
