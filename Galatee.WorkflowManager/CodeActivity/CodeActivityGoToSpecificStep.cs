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

    public sealed class CodeActivityGoToSpecificStep : CodeActivity<RESULTACTION>
    {

        #region InArgument

        public InArgument<string> CodeDemande { get; set; }
        public InArgument<CODEACTION> CodeAction { get; set; }
        public InArgument<string> Commentaire { get; set; }
        public InArgument<Guid> FKIDEtape { get; set; }
        public InArgument<string> Matricule { get; set; }

        #endregion

        #region OutArgument

        public OutArgument<string> MessageErreur { get; set; }

        #endregion

        // If your activity returns a value, derive from CodeActivity<TResult>
        // and return the value from the Execute method.
        protected override RESULTACTION Execute(CodeActivityContext context)
        {
            RESULTACTION _result = RESULTACTION.ERREURINCONNUE;
            try
            {
                string _codeDemande = CodeDemande.Get(context);
                string _commentaire = Commentaire.Get(context);
                Guid _fkIdEtape = FKIDEtape.Get(context);
                string _matriculeUser = Matricule.Get(context);

                CODEACTION _codeAction = CodeAction.Get(context);

                DB_WORKFLOW dbWKF = new DB_WORKFLOW();
                CsDemandeWorkflow dmdWorkflow = dbWKF.SelectLaDemande(_codeDemande);

                if (null != dmdWorkflow)
                {
                    //Journal
                    CsJournalDemandeWorkflow jrnal = null;
                    CsCopieDmdCircuit etapeDemande = dbWKF.SelectAllCircuitEtapeDemandeWorkflow(dmdWorkflow.PK_ID)
                        .Keys.ToList()
                        .Where(e => e.PK_ID == _fkIdEtape)
                        .FirstOrDefault();
                    
                    if (null == etapeDemande)
                    {
                        _result = RESULTACTION.ERREURINCONNUE;
                        MessageErreur.Set(context, "Aucune étape n'a été configurée avec cette ID");

                        return _result;
                    }
                    

                    switch (_codeAction)
                    {
                        case CODEACTION.TRANSMETTRE:
                            {
                                int precedent = dmdWorkflow.FK_IDETAPEACTUELLE;
                                dmdWorkflow.FK_IDETAPEACTUELLE = etapeDemande.FK_IDETAPE;
                                dmdWorkflow.FK_IDETAPECIRCUIT = etapeDemande.PK_ID;
                                dmdWorkflow.FK_IDETAPEPRECEDENTE = precedent;

                                var etapeSuivante = dbWKF.ProchaineOuPrecedenteEtape(dmdWorkflow.CODE, etapeDemande.FK_IDETAPE, etapeDemande.PK_ID, 1, false);
                                if (null != etapeSuivante.Key)
                                {
                                    dmdWorkflow.FK_IDETAPESUIVANTE = etapeSuivante.Key.FK_IDETAPE;                                    
                                }
                                else
                                {
                                    //On suppose qu'on est à la dernière étape
                                    dmdWorkflow.FK_IDETAPESUIVANTE = 0;
                                }

                                dbWKF.DeleteCommentaireRejet(dmdWorkflow.PK_ID);
                                CsCommentaireRejet Com = new CsCommentaireRejet()
                                {
                                    CODEDEMANDE = dmdWorkflow.CODE,
                                    DATECOMMENTAIRE = DateTime.Today.Date,
                                    FK_IDDEMANDE = dmdWorkflow.PK_ID,
                                    PK_ID = Guid.NewGuid(),
                                    COMMENTAIRE = _commentaire,
                                    PIECE_JOINTE = null,
                                    UTILISATEUR = _matriculeUser 
                                };
                                dbWKF.InsertCommentaireRejet(new List<CsCommentaireRejet>() { Com });
                                                              

                                dmdWorkflow.FK_IDSTATUS = (int)STATUSDEMANDE.EnAttenteValidation;
                                dmdWorkflow.DATEDERNIEREMODIFICATION = DateTime.Now;

                                _result = RESULTACTION.TRANSMISE;
                                MessageErreur.Set(context, "");

                                jrnal = new CsJournalDemandeWorkflow()
                                {
                                    CODE_DEMANDE = dmdWorkflow.CODE,
                                    DATEACTION = DateTime.Today.Date,
                                    FK_IDDEMANDE = dmdWorkflow.PK_ID,
                                    PK_ID = Guid.NewGuid(),
                                    LIBELLEACTION = "Transmission à l'étape " + etapeDemande.LIBELLEETAPE,
                                    MATRICULEUSERACTION = _matriculeUser,
                                    OBSERVATIONS = string.Empty
                                };
                            }
                            break;

                        case CODEACTION.REJETER:
                            {
                                var etapeSuivante = dbWKF.ProchaineOuPrecedenteEtape(dmdWorkflow.CODE, etapeDemande.FK_IDETAPE, etapeDemande.PK_ID, -1, false);
                                if (null != etapeSuivante.Key) dmdWorkflow.FK_IDETAPEPRECEDENTE = etapeSuivante.Key.FK_IDETAPE;
                                else
                                {
                                    //On suppose qu'on est à la 1ere étape
                                    dmdWorkflow.FK_IDETAPEPRECEDENTE = 0;
                                }

                                dbWKF.DeleteCommentaireRejet(dmdWorkflow.PK_ID);
                                CsCommentaireRejet Com = new CsCommentaireRejet()
                                {
                                    CODEDEMANDE = dmdWorkflow.CODE,
                                    DATECOMMENTAIRE = DateTime.Today.Date,
                                    FK_IDDEMANDE = dmdWorkflow.PK_ID,
                                    PK_ID = Guid.NewGuid(),
                                    COMMENTAIRE = _commentaire,
                                    PIECE_JOINTE = null,
                                    UTILISATEUR = _matriculeUser
                                };
                                dbWKF.InsertCommentaireRejet(new List<CsCommentaireRejet>() { Com });

                                int suivant = dmdWorkflow.FK_IDETAPEACTUELLE;
                                dmdWorkflow.FK_IDETAPEACTUELLE = etapeDemande.FK_IDETAPE;
                                dmdWorkflow.FK_IDETAPECIRCUIT = etapeDemande.PK_ID;
                                dmdWorkflow.FK_IDETAPESUIVANTE = suivant;                                

                                dmdWorkflow.FK_IDSTATUS = (int)STATUSDEMANDE.Rejetee;
                                dmdWorkflow.DATEDERNIEREMODIFICATION = DateTime.Now;

                                _result = RESULTACTION.REJETEE;
                                MessageErreur.Set(context, "");

                                jrnal = new CsJournalDemandeWorkflow()
                                {
                                    CODE_DEMANDE = dmdWorkflow.CODE,
                                    DATEACTION = DateTime.Today.Date,
                                    FK_IDDEMANDE = dmdWorkflow.PK_ID,
                                    PK_ID = Guid.NewGuid(),
                                    LIBELLEACTION = "Rejet à l'étape " + etapeDemande.LIBELLEETAPE,
                                    MATRICULEUSERACTION = _matriculeUser,
                                    OBSERVATIONS = string.Empty
                                };
                            }
                            break;
                    }

                    if (_result != RESULTACTION.ERREURINCONNUE)
                    {
                        dbWKF.UpdateDemande(new List<CsDemandeWorkflow>() { dmdWorkflow });
                        dbWKF.SupprimerAffectationDemande(dmdWorkflow.CODE);

                        //Insertion du journal   
                        dbWKF.InsertJournalDemande(new List<CsJournalDemandeWorkflow>() { jrnal });
                    }
                    
                }
            }
            catch (Exception ex)
            {
                _result = RESULTACTION.ERREURINCONNUE;
                MessageErreur.Set(context, ex.Message);
            }
            return _result;
        }
    }
}
