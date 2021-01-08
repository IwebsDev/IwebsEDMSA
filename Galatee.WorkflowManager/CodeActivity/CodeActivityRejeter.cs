using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;
using Galatee.Structure;
using Galatee.DataAccess;

namespace Galatee.WorkflowManager
{

    public sealed class CodeActivityRejeter : CodeActivity<RESULTACTION>
    {

        #region In Parametres

        public InArgument<string> CodeDemande { get; set; }
        public InArgument<string> MatriculeUser { get; set; }
        public InArgument<string> Commentaire { get; set; }

        #endregion

        #region Out Parameter

        public OutArgument<string> MessageErreur { get; set; }

        #endregion


        // If your activity returns a value, derive from CodeActivity<TResult>
        // and return the value from the Execute method.
        protected override RESULTACTION Execute(CodeActivityContext context)
        {
            RESULTACTION _resultAction = RESULTACTION.ERREURINCONNUE;

            string _matriculeUser = context.GetValue<string>(MatriculeUser);
            string _codeDemande = context.GetValue<string>(CodeDemande);
            string _actionOnDemande = string.Empty;
            string _commentaireRejet = context.GetValue<string>(Commentaire);
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
                    //if (null != table)
                    //{
                        //Avant de rejeter  la demande on cherche les infos de l'étape actuelle
                        KeyValuePair<CsCopieDmdCircuit, CsCopieDmdConditionBranchement> CurrentStep = dbWKF.RecupererEtapeCourante(_codeDemande);
                        if (null != CurrentStep.Key)
                        {
                            //ici on pas besoin de condition, on rejette ooh
                            bool LastStep = false;
                            string msgErr = string.Empty;
                            //On rejette
                            if (dbWKF.RejeteDemande(_codeDemande, dmdWorkflow.FK_IDETAPEACTUELLE, ref msgErr, ref LastStep,
                                ref dmdWorkflow))
                            {
                                //On a bien transmis,
                                dmdWorkflow.FK_IDSTATUS = (int)STATUSDEMANDE.Rejetee;   //Pour la prochaine étape
                                dmdWorkflow.DATEDERNIEREMODIFICATION = DateTime.Now;
                                _resultAction = RESULTACTION.REJETEE;
                                _actionOnDemande = "Rejet de l'étape " + CurrentStep.Key.LIBELLEETAPE;
                                MessageErreur.Set(context, "");

                                #region CommentaireRejet

                                DBAdmUsers dbUser = new DBAdmUsers();
                                CsUtilisateur u = dbUser.GetUtilisateurByMatricule(_matriculeUser);
                                
                                //ON supprime les autres commentaires de la demande concernée
                                dbWKF.DeleteCommentaireRejet(dmdWorkflow.PK_ID);
                                CsCommentaireRejet commentaire = new CsCommentaireRejet()
                                {
                                    PK_ID = Guid.NewGuid(),
                                    DATECOMMENTAIRE = DateTime.Today.Date,
                                    CODEDEMANDE = dmdWorkflow.CODE_DEMANDE_TABLETRAVAIL,
                                    COMMENTAIRE = _commentaireRejet,
                                    PIECE_JOINTE = null,
                                    FK_IDDEMANDE = dmdWorkflow.PK_ID,
                                    UTILISATEUR = (null != u && null != u.NOM && string.Empty != u.NOM) ? u.NOM : _matriculeUser
                                };

                                dbWKF.InsertCommentaireRejet(new List<CsCommentaireRejet>() { commentaire });

                                #endregion

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
                                    jrnal.MATRICULEUSERACTION = _matriculeUser;

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
                                //On na pas bien rejeté, donc on check si on est déjà à la 1ere étape
                                if (LastStep)
                                {
                                    //Bon la, la demande est comme si elle est initiée
                                    dmdWorkflow.FK_IDSTATUS = (int)STATUSDEMANDE.Rejetee;
                                    _resultAction = RESULTACTION.DEBUTDECIRCUIT;
                                    
                                    //Si on est revenue au début du circuit, on n'a plus de journaux à insérer,
                                    //car aucune action n'a été faite en principe
                                    bool update = dbWKF.UpdateDemande(new List<CsDemandeWorkflow>() { dmdWorkflow });
                                    
                                    //Suppresion de la demande dans la table des affectations user
                                    dbWKF.SupprimerAffectationDemande(dmdWorkflow.CODE);
                                    
                                    MessageErreur.Set(context, "");
                                }
                                else
                                {
                                    //La la, ya vraiment une erreur qui s'est produite
                                    _resultAction = RESULTACTION.ERREURINCONNUE;
                                    MessageErreur.Set(context, msgErr);
                                }
                            }

                            GC.SuppressFinalize(dbWKF);
                            return _resultAction;
                        }
                        else
                        {
                            GC.SuppressFinalize(dbWKF);
                            MessageErreur.Set(context, "Impossible de rejeter une demande n'étant à aucune étape");
                            return RESULTACTION.ERREURINCONNUE;
                        }
                    //}
                    //else
                    //{
                    //    GC.SuppressFinalize(dbWKF);
                    //    MessageErreur.Set(context, "Impossible de rejeter une demande n'étant pas liée à une table de travail");
                    //    return RESULTACTION.ERREURINCONNUE;
                    //}
                }
                else
                {
                    GC.SuppressFinalize(dbWKF);
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
