using Galatee.Entity.Model;
using Galatee.Structure;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.EntityClient;
using System.Data;
using System.Data.SqlClient;

namespace Galatee.DataAccess
{
    [DataObject]
    public class DB_WORKFLOW : Galatee.DataAccess.Parametrage.DbBase
    {

        public DB_WORKFLOW()
        {
            try
            {
                ConnectionString = Session.GetSqlConnexionString();
            }
            catch (Exception)
            {
                throw;
            }
        }
        private string ConnectionString;
        private SqlCommand cmd = null;

        public SqlCommand Cmd
        {
            get { return cmd; }
            set { cmd = value; }
        }
        private SqlConnection cn = null;
        #region Gestion Etapes & Operations

        public List<CsOperation> SelectAllOperation()
        {
            try
            {
                return Entities.GetEntityListFromQuery<CsOperation>(ParamProcedure.LISTE_WKF_OPERATION_PARENT());
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<CsOperation> SelectAllOperation2()
        {
            try
            {
                return Entities.GetEntityListFromQuery<CsOperation>(ParamProcedure.LISTE_WKF_ALLOPERATIONS());
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<CsOperationFormulaire> SelectAllViewOperationFormulaire()
        {
            try
            {
                return Entities.GetEntityListFromQuery<CsOperationFormulaire>(ParamProcedure.LISTE_WKF_VW_OPERATION_FORMULAIRE());
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<CsFormulaire> SelectAllFormulaire()
        {
            try
            {
                return Entities.GetEntityListFromQuery<CsFormulaire>(ParamProcedure.LISTE_FORMULAIRE());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public  CsEtape  SelectAllEtapesbyId(int IdEtape)
        {
            try
            {
                CsEtape  lsEtapes = new  CsEtape();
                lsEtapes = Entities.GetEntityFromQuery<CsEtape>(ParamProcedure.LISTE_WKF_ETAPESbyId(IdEtape));
                return lsEtapes;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CsEtape> SelectAllEtapes()
        {
            try
            {
                List<CsEtape> lsEtapes = new List<CsEtape>();
                lsEtapes = Entities.GetEntityListFromQuery<CsEtape>(ParamProcedure.LISTE_WKF_ETAPES());
                List<CsFormulaire> lsFormulaires = SelectAllFormulaire();
                lsEtapes.ForEach((CsEtape step) =>
                {
                    CsFormulaire form = null;
                    if (step.FK_IDFORMULAIRE.HasValue)
                    {
                        form = lsFormulaires.Where(f => f.PK_ID == step.FK_IDFORMULAIRE)
                        .FirstOrDefault();
                        if (null != form)
                        {
                            step.FK_IDFORMULAIRE = form.PK_ID;
                            step.FULLNAMECONTROLE = form.FULLNAMECONTROLE;
                            step.LIBELLEFORMULAIRE = form.FORMULAIRE1;
                        }
                    }
                    if (null != form) GC.SuppressFinalize(form);
                });
                return lsEtapes;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CsVwConfigurationWorkflowCentre> SelectAllConfigurationWorkflowCentre()
        {
            try
            {
                return Entities.GetEntityListFromQuery<CsVwConfigurationWorkflowCentre>(ParamProcedure.LISTE_WKF_CONFIGURATIONWORKFLOWCENTRE());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CsEtape> SelectAllEtapesByIdEtape(Guid OperationId)
        {
            try
            {
                List<CsEtape> lsEtapes = new List<CsEtape>();
                lsEtapes = Entities.GetEntityListFromQuery<CsEtape>(ParamProcedure.LISTE_WKF_ETAPES(OperationId));
                return lsEtapes.Where(t=>!string.IsNullOrEmpty( t.CONTROLEETAPE)).ToList() ;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CsEtape> SelectAllEtapes(Guid OperationId)
        {
            try
            {
                List<CsEtape> lsEtapes = new List<CsEtape>();
                lsEtapes = Entities.GetEntityListFromQuery<CsEtape>(ParamProcedure.LISTE_WKF_ETAPES(OperationId));
                List<CsFormulaire> lsFormulaires = SelectAllFormulaire();
                lsEtapes.ForEach((CsEtape step) =>
                {
                    CsFormulaire form = null;
                    if (step.FK_IDFORMULAIRE.HasValue)
                    {
                        form = lsFormulaires.Where(f => f.PK_ID == step.FK_IDFORMULAIRE)
                        .FirstOrDefault();
                        if (null != form)
                        {
                            step.FK_IDFORMULAIRE = form.PK_ID;
                            step.FULLNAMECONTROLE = form.FULLNAMECONTROLE;
                            step.LIBELLEFORMULAIRE = form.FORMULAIRE1;
                        }
                    }
                    if (null != form) GC.SuppressFinalize(form);
                });
                return lsEtapes;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool InsertEtape(List<CsEtape> lsEtapes)
        {
            try
            {
                return Entities.InsertEntity<ETAPE>(Entities.ConvertObject<ETAPE, CsEtape>(lsEtapes));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool UpdateEtape(List<CsEtape> lsEtapes)
        {
            try
            {
                return Entities.UpdateEntity<ETAPE>(Entities.ConvertObject<ETAPE, CsEtape>(lsEtapes));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool DeleteEtape(List<CsEtape> lsEtapes)
        {
            try
            {
                return Entities.DeleteEntity<ETAPE>(Entities.ConvertObject<ETAPE, CsEtape>(lsEtapes));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region groupes Validations

        public Dictionary<CsGroupeValidation, List<CsRHabilitationGrouveValidation>> SelectAllGroupeValidation()
        {
            try
            {
                Dictionary<CsGroupeValidation, List<CsRHabilitationGrouveValidation>> Datas = new Dictionary<CsGroupeValidation, List<CsRHabilitationGrouveValidation>>();
                List<CsGroupeValidation> lsGrp =this.LISTE_WKF_GROUPES_VALIDATION();
                lsGrp.ForEach((CsGroupeValidation grp) =>
                {
                    List<CsRHabilitationGrouveValidation> lsHabil = GetHabilitationGroupeValidation(grp.PK_ID);
                    Datas.Add(grp, lsHabil);
                });
                return Datas;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public Dictionary<CsGroupeValidation, List<CsRHabilitationGrouveValidation>> SelectAllGroupeValidationUser(int UserId)
        {
            try
            {
                Dictionary<CsGroupeValidation, List<CsRHabilitationGrouveValidation>> Datas = new Dictionary<CsGroupeValidation, List<CsRHabilitationGrouveValidation>>();
                List<CsGroupeValidation> lsGrp = this.LISTE_WKF_GROUPES_VALIDATION();

                string groupValidation = DBBase.RetourneStringListeObject(lsGrp.Select(i => i.PK_ID.ToString()).ToList());
                List<CsRHabilitationGrouveValidation> lsHabil = GetHabilitationGroupeValidationUser(groupValidation, UserId);
                List<CsGroupeValidation> lsGrpUser = lsGrp.Where(p => lsHabil.Select(o => o.FK_IDGROUPE_VALIDATION).ToList().Contains(p.PK_ID)).ToList();
                lsGrpUser.ForEach((CsGroupeValidation grp) =>
                {
                    List<CsRHabilitationGrouveValidation> lsHabilUser = lsHabil.Where(p=>p.FK_IDGROUPE_VALIDATION==grp.PK_ID ).ToList();
                    Datas.Add(grp, lsHabilUser);
                });
                return Datas;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CsGroupeValidation> SelectAllGroupeValidationSpecifique(int TypeGroupe)
        {
            try
            {
                List<CsGroupeValidation> lstGV = this.LISTE_WKF_GROUPES_VALIDATION();
                if (lstGV != null)
                    return lstGV.Where(t => t.VALEURSPECIFIQUE == TypeGroupe).ToList();
                else return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool InsertGroupeValidation(Dictionary<CsGroupeValidation, List<CsRHabilitationGrouveValidation>> lsGroupes)
        {
            try
            {
                //On insert dabord les groupes
                bool insertH = true ;
                bool insertGrp = Entities.InsertEntity<GROUPE_VALIDATION>(Entities.ConvertObject<GROUPE_VALIDATION, CsGroupeValidation>(lsGroupes.Keys
                    .ToList()));
                if (insertGrp)
                {
                    //On insert maintenant les habilitations
                    List<CsRHabilitationGrouveValidation> lsHabilitations = new List<CsRHabilitationGrouveValidation>();
                    foreach (List<CsRHabilitationGrouveValidation> kVp in lsGroupes.Values)
                    {
                        if (null != lsGroupes.Values) lsHabilitations.AddRange(kVp.ToList());
                    }
                    if (lsHabilitations != null && lsHabilitations.Count != 0)
                        insertH = Entities.InsertEntity<RHABILITATIONGROUPEVALIDATION>(Entities.ConvertObject<RHABILITATIONGROUPEVALIDATION,
                        CsRHabilitationGrouveValidation>(lsHabilitations));

                    return insertGrp && insertH;
                }
                else return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool UpdateGroupValidation(Dictionary<CsGroupeValidation, List<CsRHabilitationGrouveValidation>> lsGroupes)
        {
            try
            {
                //Pour chaque Groupe On supprime les habilitations et on recrée
                //On insert dabord les groupes
                bool insertGrp = Entities.UpdateEntity<GROUPE_VALIDATION>(Entities.ConvertObject<GROUPE_VALIDATION, CsGroupeValidation>(lsGroupes.Keys
                    .ToList()));
                if (insertGrp)
                {
                    //On supprime les anciennes habilitations
                    foreach (var grp in lsGroupes.Keys)
                    {
                        DeleteHabilitationFromGrpId(grp.PK_ID);
                    }
                    //Ensuite on insert les nouvelles valeurs
                    List<CsRHabilitationGrouveValidation> lsHabilitations = new List<CsRHabilitationGrouveValidation>();
                    foreach (List<CsRHabilitationGrouveValidation> kVp in lsGroupes.Values)
                    {
                        lsHabilitations.AddRange(kVp.ToList());
                    }
                    bool insertH = Entities.InsertEntity<RHABILITATIONGROUPEVALIDATION>(Entities.ConvertObject<RHABILITATIONGROUPEVALIDATION,
                    CsRHabilitationGrouveValidation>(lsHabilitations));

                    return insertGrp && insertH;
                }
                else return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool DeleteHabilitationFromGrpId(Guid grpId)
        {
            try
            {
                galadbEntities context = new galadbEntities();
                List<RHABILITATIONGROUPEVALIDATION> lsHabilitations = context.RHABILITATIONGROUPEVALIDATION
                    .Where(h => h.FK_IDGROUPE_VALIDATION == grpId)
                    .ToList();
                //Maintenant on supprime
                foreach (var H in lsHabilitations)
                {
                    if (context.Entry(H).State == System.Data.EntityState.Detached)
                    {
                        context.RHABILITATIONGROUPEVALIDATION.Attach(H);
                    }
                    context.RHABILITATIONGROUPEVALIDATION.Remove(H);
                    context.SaveChanges();
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool DeleteGroupeValidation(List<CsGroupeValidation> lsGroupesValidations)
        {
            try
            {
                //on delete dabord les habilitations
                lsGroupesValidations.ForEach((CsGroupeValidation grp) =>
                {
                    DeleteHabilitationFromGrpId(grp.PK_ID);
                });
                //Ensuite on delete les groupes
                bool deleteGrp = Entities.DeleteEntity<GROUPE_VALIDATION>(Entities.ConvertObject<GROUPE_VALIDATION, CsGroupeValidation>(lsGroupesValidations));
                return deleteGrp;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

     
        #endregion

        public bool Insert(List<CsOperation> lsOperations)
        {
            try
            {
                return Entities.InsertEntity<OPERATION>(Entities.ConvertObject<OPERATION, CsOperation>(lsOperations));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool UpdateOperation(List<CsOperation> lsOperations)
        {
            try
            {
                return Entities.UpdateEntity<OPERATION>(Entities.ConvertObject<OPERATION, CsOperation>(lsOperations));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool DeleteOperation(List<CsOperation> lsOperations)
        {
            try
            {
                List<CsOperation> SousOperation = new List<CsOperation>();
                lsOperations.ForEach((CsOperation op) =>
                {
                    List<CsOperation> __sousOp = Entities.GetEntityListFromQuery<CsOperation>(ParamProcedure.LISTE_WKF_ALLSOUSOPERATIONS(op.PK_ID));
                    SousOperation.AddRange(__sousOp);
                });
                //A la fin on ajoute les opérations à supprimer
                SousOperation.AddRange(lsOperations);
                return Entities.DeleteEntity<OPERATION>(Entities.ConvertObject<OPERATION, CsOperation>(SousOperation));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Workflows

        public List<CsWorkflow> SelectAllWorkflows()
        {
            try
            {
                return Entities.GetEntityListFromQuery<CsWorkflow>(ParamProcedure.LISTE_WKF());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CsTableDeTravail> SelectAllTableDeTravail()
        {
            try
            {
                return Entities.GetEntityListFromQuery<CsTableDeTravail>(ParamProcedure.LISTE_WKF_TABLETRAVAIL());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool InsertWorkflow(List<CsWorkflow> lsWorkflows)
        {
            try
            {
                return Entities.InsertEntity<WORKFLOW>(Entities.ConvertObject<WORKFLOW, CsWorkflow>(lsWorkflows));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool UpdateWorkflow(List<CsWorkflow> lsWorkflows)
        {
            try
            {
                return Entities.UpdateEntity<WORKFLOW>(Entities.ConvertObject<WORKFLOW, CsWorkflow>(lsWorkflows));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool DeleteWorkflow(List<CsWorkflow> lsWorkflows)
        {
            try
            {
                return Entities.DeleteEntity<WORKFLOW>(Entities.ConvertObject<WORKFLOW, CsWorkflow>(lsWorkflows));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Dictionary<CsRAffectationEtapeWorkflow, CsConditionBranchement> SelectAllAffectationEtapeWorkflow(Guid pRWKF)
        {
            try
            {
                var lsEtapesAffWKF = Entities.GetEntityListFromQuery<CsRAffectationEtapeWorkflow>(ParamProcedure.LISTE_WKF_ETAPESWORFKLOW(pRWKF));
                //Ensuite les conditions s'il en existe
                Dictionary<CsRAffectationEtapeWorkflow, CsConditionBranchement> TheDatas = new Dictionary<CsRAffectationEtapeWorkflow, CsConditionBranchement>();
                foreach (CsRAffectationEtapeWorkflow aff in lsEtapesAffWKF)
                {
                    galadbEntities context = new galadbEntities();
                    var theGroupeValidation = context.GROUPE_VALIDATION.Where(grp => grp.PK_ID == aff.FK_IDGROUPEVALIDATIOIN)
                        .FirstOrDefault();
                    if (null != theGroupeValidation) aff.GROUPEVALIDATION = theGroupeValidation.GROUPENAME;

                    var lsConditions = Entities.GetEntityListFromQuery<CsConditionBranchement>(ParamProcedure.LISTE_WKF_CONDITION(aff.PK_ID));
                    if (0 != lsConditions.Count)
                    {
                        CsConditionBranchement lCondition = lsConditions.FirstOrDefault();
                        aff.CONDITION = lCondition.COLONNENAME + " " + lCondition.OPERATEUR + " " + lCondition.VALUE;

                        var Step = context.ETAPE.Where(e => e.PK_ID == lCondition.FK_IDETAPEVRAIE.Value)
                            .FirstOrDefault();
                        if (null != Step) aff.ETAPECONDITIONVRAIE = Step.NOM;
                    }
                    TheDatas.Add(aff, lsConditions.FirstOrDefault());

                    context.Dispose();
                    GC.SuppressFinalize(context);
                }
                return TheDatas;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Dictionary<CsRAffectationEtapeWorkflow, CsConditionBranchement> SelectAllAffectationEtapeCircuitDetourne(Guid pOrigineAff)
        {
            try
            {
                var lsEtapesAffWKF = Entities.GetEntityListFromQuery<CsRAffectationEtapeWorkflow>(ParamProcedure.LISTE_WKF_ETAPESWORFKLOWDETOURNE(pOrigineAff));
                //Ensuite les conditions s'il en existe
                Dictionary<CsRAffectationEtapeWorkflow, CsConditionBranchement> TheDatas = new Dictionary<CsRAffectationEtapeWorkflow, CsConditionBranchement>();
                foreach (CsRAffectationEtapeWorkflow aff in lsEtapesAffWKF)
                {
                    galadbEntities context = new galadbEntities();
                    var theGroupeValidation = context.GROUPE_VALIDATION.Where(grp => grp.PK_ID == aff.FK_IDGROUPEVALIDATIOIN)
                        .FirstOrDefault();
                    if (null != theGroupeValidation) aff.GROUPEVALIDATION = theGroupeValidation.GROUPENAME;

                    var lsConditions = Entities.GetEntityListFromQuery<CsConditionBranchement>(ParamProcedure.LISTE_WKF_CONDITION(aff.PK_ID));
                    if (null != lsConditions.FirstOrDefault())
                    {
                        aff.CONDITION = lsConditions.FirstOrDefault().COLONNENAME + " " + lsConditions.FirstOrDefault().OPERATEUR + " "
                            + lsConditions.FirstOrDefault().VALUE;

                        var Step = context.ETAPE.Where(e => e.PK_ID == lsConditions.FirstOrDefault().FK_IDETAPEVRAIE.Value)
                            .FirstOrDefault();
                        if (null != Step) aff.ETAPECONDITIONVRAIE = Step.NOM;
                    }
                    TheDatas.Add(aff, lsConditions.FirstOrDefault());

                    context.Dispose();
                    GC.SuppressFinalize(context);
                }
                return TheDatas;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool InsertAffectationEtape(Dictionary<CsRAffectationEtapeWorkflow, CsConditionBranchement> lsEtapes)
        {
            try
            {
                //On enregistre dabord les étapes
                bool insertEtape = Entities.InsertEntity<RAFFECTATIONETAPEWORKFLOW>(Entities.ConvertObject<RAFFECTATIONETAPEWORKFLOW,
                    CsRAffectationEtapeWorkflow>(lsEtapes.Keys.ToList()));
                bool insertCondition = true;
                try
                {
                    List<CsConditionBranchement> LesConditionsNonNulles = new List<CsConditionBranchement>();
                    foreach (var C in lsEtapes.Values)
                        if (null != C) LesConditionsNonNulles.Add(C);
                    if (insertEtape) insertCondition = Entities.InsertEntity<CONDITIONBRANCHEMENT>(Entities.ConvertObject<CONDITIONBRANCHEMENT,
                        CsConditionBranchement>(LesConditionsNonNulles));
                }
                catch (NullReferenceException nullEx) { }
                catch (Exception Inex) { insertCondition = false; }
                return insertCondition && insertEtape;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool InsertAffectationEtape(List<Dictionary<CsRAffectationEtapeWorkflow, CsConditionBranchement>> lsEtapes)
        {
            try
            {
                //On enregistre dabord les étapes
                bool insert = true;
                foreach (var elm in lsEtapes)
                {
                    insert = InsertAffectationEtape(elm);
                }
                return insert;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool UpdateAffectationEtape(Dictionary<CsRAffectationEtapeWorkflow, CsConditionBranchement> lsEtapes)
        {
            try
            {
                bool deleteC = true;
                foreach (CsRAffectationEtapeWorkflow Aff in lsEtapes.Keys.ToList())
                {
                    deleteC = DeleteConditionEtape(Aff.PK_ID);
                    if (deleteC) continue;
                    else break;
                }
                if (!deleteC) return false;

                bool insertE = Entities.UpdateEntity<RAFFECTATIONETAPEWORKFLOW>(Entities.ConvertObject<RAFFECTATIONETAPEWORKFLOW,
                    CsRAffectationEtapeWorkflow>(lsEtapes.Keys.ToList()));
                bool insertC = true;
                if (insertE)
                {
                    insertC = Entities.InsertEntity<CONDITIONBRANCHEMENT>(Entities.ConvertObject<CONDITIONBRANCHEMENT,
                        CsConditionBranchement>(lsEtapes.Values.ToList()));
                }
                return insertC && insertE;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool DeleteConditionEtape(Guid pAffEtapWkf)
        {
            try
            {
                //On retourne les étapes
                List<CsConditionBranchement> lsConditions = Entities.GetEntityListFromQuery<CsConditionBranchement>(ParamProcedure.LISTE_WKF_CONDITION(pAffEtapWkf));
                //Une fois qu'on a la liste, on les supprimes
                return Entities.DeleteEntity<CONDITIONBRANCHEMENT>(Entities.ConvertObject<CONDITIONBRANCHEMENT, CsConditionBranchement>(lsConditions));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool DeleteCircuitDetourneFromOrigine(Guid pAffEtapWkf)
        {
            try
            {
                //On retourne les étapes
                List<CsRAffectationEtapeWorkflow> lsEtapeDuCircuit = Entities.GetEntityListFromQuery<CsRAffectationEtapeWorkflow>(ParamProcedure.LISTE_WKF_ETAPESWORFKLOWDETOURNE(pAffEtapWkf));
                //Une fois qu'on a la liste, on les supprimes
                return Entities.DeleteEntity<RAFFECTATIONETAPEWORKFLOW>(Entities.ConvertObject<RAFFECTATIONETAPEWORKFLOW,
                    CsRAffectationEtapeWorkflow>(lsEtapeDuCircuit));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool DeleteWorkflowAffectationEtape(List<CsRAffectationEtapeWorkflow> lsEtapes)
        {
            try
            {
                //Avant de delete les étapes, on delete les conditions
                bool deleteC = true;
                foreach (CsRAffectationEtapeWorkflow aff in lsEtapes)
                {
                    deleteC = DeleteConditionEtape(aff.PK_ID) && DeleteCircuitDetourneFromOrigine(aff.PK_ID);
                    //On supprime les renvois
                    DeleteRenvoiRejet(aff.PK_ID);

                    if (deleteC) continue;
                    else break;
                }
                if (!deleteC) return false;

                return deleteC && Entities.DeleteEntity<RAFFECTATIONETAPEWORKFLOW>(Entities.ConvertObject<RAFFECTATIONETAPEWORKFLOW,
                    CsRAffectationEtapeWorkflow>(lsEtapes));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CsRWorkflow> SelectAllRWorkflowCentre(Guid pKIDWKF, int CpKID, Guid OpPKID)
        {
            try
            {
                return Entities.GetEntityListFromQuery<CsRWorkflow>(ParamProcedure.LISTE_WKF_RAFFECTATION_WKF_CENTRE(pKIDWKF, CpKID, OpPKID));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool InsertRWorkflowCentre(List<CsRWorkflow> lsWorkflows)
        {
            try
            {
                return Entities.InsertEntity<RWORFKLOWCENTRE>(Entities.ConvertObject<RWORFKLOWCENTRE, CsRWorkflow>(lsWorkflows));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool UpdateRWorkflowCentre(List<CsRWorkflow> lsWorkflows)
        {
            try
            {
                return Entities.UpdateEntity<RWORFKLOWCENTRE>(Entities.ConvertObject<RWORFKLOWCENTRE, CsRWorkflow>(lsWorkflows));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool DeleteRWorkflowCentre(List<CsRWorkflow> lsWorkflows)
        {
            try
            {
                return Entities.DeleteEntity<RWORFKLOWCENTRE>(Entities.ConvertObject<RWORFKLOWCENTRE, CsRWorkflow>(lsWorkflows));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool ConfigurerPlusieurWorkflowEtCentre(List<CsRWorkflow> rwkfCentres, Dictionary<CsRAffectationEtapeWorkflow, CsConditionBranchement> configurationsEtapes,
            List<CsRenvoiRejet> _Renvois)
        {
            try
            {
                //Parcours de chaque éléments
                bool configOk = true;
                foreach (var rwkf in rwkfCentres)
                {
                    var ilExist = SelectAllRWorkflowCentre(rwkf.FK_IDWORKFLOW, rwkf.FK_IDCENTRE, rwkf.FK_IDOPERATION).FirstOrDefault();
                    bool update = ilExist != null;
                    //Si ça existe on supprime toutes les configuration de ce rworfklow
                    if (update)
                    {
                        var etapes = SelectAllAffectationEtapeWorkflow(ilExist.PK_ID);
                        bool deleteOk = DeleteWorkflowAffectationEtape(etapes.Keys.ToList());
                        if (!deleteOk)
                        {
                            configOk = false;
                            break;
                        }
                        //Si la suppression est ok, on réinsert tout
                        Dictionary<CsRAffectationEtapeWorkflow, CsConditionBranchement> etapesToInsert = new Dictionary<CsRAffectationEtapeWorkflow, CsConditionBranchement>();
                        foreach (var aff in configurationsEtapes.Keys.Where(k => k.FK_RWORKFLOWCENTRE == rwkf.PK_ID).ToList())
                        {
                            aff.FK_RWORKFLOWCENTRE = ilExist.PK_ID;
                            etapesToInsert.Add(aff, configurationsEtapes[aff]);
                        }
                        bool reInsert = InsertAffectationEtape(etapesToInsert);
                        configOk = deleteOk && reInsert;
                    }
                    else
                    {
                        Dictionary<CsRAffectationEtapeWorkflow, CsConditionBranchement> etapesToInsert = new Dictionary<CsRAffectationEtapeWorkflow, CsConditionBranchement>();
                        foreach (var aff in configurationsEtapes.Keys.Where(k => k.FK_RWORKFLOWCENTRE == rwkf.PK_ID).ToList())
                        {
                            etapesToInsert.Add(aff, configurationsEtapes[aff]);
                        }
                        bool insertWkf = InsertRWorkflowCentre(new List<CsRWorkflow>() { rwkf });
                        bool reInsert = InsertAffectationEtape(etapesToInsert);
                        configOk = reInsert && insertWkf;
                    }

                    if (!configOk) break;
                }
                if (configOk)
                {
                    //On insert les renvois
                    InsertRenvoiRejet(_Renvois);
                }
                return configOk;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string[] LesColonnesDeLaTable(string table)
        {
            try
            {
                string[] Colonnes = default(string[]);
                using (galadbEntities context = new galadbEntities())
                {
                    Colonnes = context.Database.SqlQuery<string>("select COLUMN_NAME from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME='" + table + "'")
                        .ToArray();
                }
                return Colonnes;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Demandes

        public List<CsDemandeWorkflow> SelectAllDemande()
        {
            try
            {
                return Entities.GetEntityListFromQuery<CsDemandeWorkflow>(ParamProcedure.LISTE_WKF_DEMANDE());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public CsDemandeWorkflow SelectLaDemande(string CodeDemande)
        {
            try
            {
                return Entities.GetEntityListFromQuery<CsDemandeWorkflow>(ParamProcedure.LISTE_WKF_DEMANDE(CodeDemande))
                    .FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public CsDemandeWorkflow SelectLaDemandePar_Code_Matricule(string CodeDemande,string matricule)
        {
            try
            {
                return Entities.GetEntityListFromQuery<CsDemandeWorkflow>(ParamProcedure.LISTE_WKF_DEMANDE_Code_Matricule(CodeDemande, matricule))
                    .FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CsDemandeWorkflow> SelectLaDemande(List<string> pListIdDemande)
        {
            try
            {
                return Entities.GetEntityListFromQuery<CsDemandeWorkflow>(ParamProcedure.LISTE_WKF_DEMANDE(pListIdDemande));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public CsDemandeWorkflow SelectLaDemande_NumDemande(string CodeDemande)
        {
            try
            {
                return Entities.GetEntityListFromQuery<CsDemandeWorkflow>(ParamProcedure.LISTE_WKF_DEMANDEBYNUM_DEMANDE(CodeDemande))
                    .FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Permet de déterminer la prochaine étape, ou l'étape précédente, selon que l'on veut
        //transmettre ou rejete, avec l'ID de la demande
        private KeyValuePair<CsCopieDmdCircuit, CsCopieDmdConditionBranchement> ProchaineOuPrecedenteEtape(Guid pKIDDemande,
            int EtapeActuelle, Guid PKIDEtapeActuelle, int NextOrBefore, bool conditionRespecte)
        {
            Dictionary<CsCopieDmdCircuit, CsCopieDmdConditionBranchement> leCircuit = SelectAllCircuitEtapeDemandeWorkflow(pKIDDemande);

            //KeyValuePair<CsCopieDmdCircuit, CsCopieDmdConditionBranchement> lEtape = leCircuit.Where(e => e.Key.FK_IDETAPE == EtapeActuelle && e.Key.PK_ID == PKIDEtapeActuelle)
            //        .FirstOrDefault();

            KeyValuePair<CsCopieDmdCircuit, CsCopieDmdConditionBranchement> lEtape = leCircuit.Where(e => e.Key.FK_IDETAPE == EtapeActuelle)
            .FirstOrDefault();
            KeyValuePair<CsCopieDmdCircuit, CsCopieDmdConditionBranchement> leNextOrBeforeStep = new KeyValuePair<CsCopieDmdCircuit,
                CsCopieDmdConditionBranchement>(null, null);
            if (NextOrBefore == 1 || NextOrBefore == -1)
            {
                if (conditionRespecte && NextOrBefore == 1) //On avance avec la condition
                {
                    leNextOrBeforeStep = leCircuit.Where(e => e.Key.FK_IDRETAPEWORKFLOWORIGINE == lEtape.Key.PK_ID
                        && e.Key.ORDRE == 1)
                        .FirstOrDefault();
                }
                else if (!conditionRespecte) //On avance sans condition
                {
                    //On avance ou on recule sans condition

                    //si on recule on regarde si on ne se trouve pas à la 1ere étape d'un circuit de 
                    //détournement, si c'est le cas, on va à l'étape d'origine
                    if (NextOrBefore == -1 && lEtape.Key.ORDRE == 1 && lEtape.Key.FK_IDRETAPEWORKFLOWORIGINE.HasValue &&
                        lEtape.Key.FK_IDRETAPEWORKFLOWORIGINE != Guid.Empty)
                    {
                        leNextOrBeforeStep = leCircuit.Where(e => e.Key.PK_ID == lEtape.Key.FK_IDRETAPEWORKFLOWORIGINE)
                            .FirstOrDefault();
                    }
                    else
                    {
                        //Encore une petite complication, à ce niveau, on test si l'étape en cour à un FK_IDRETAPEWORKFLOWORIGINE
                        //c-a-d qu'elle fait partir d'un circuit de détournement, dans ce cas là, on retourne la prochaine étape de ce
                        //circuit là
                        //Dans le cas contraire on prend la procédure normal
                        if (null != lEtape.Key.FK_IDRETAPEWORKFLOWORIGINE && lEtape.Key.FK_IDRETAPEWORKFLOWORIGINE.HasValue
                            && lEtape.Key.FK_IDRETAPEWORKFLOWORIGINE.Value != Guid.Empty)
                        {
                            leNextOrBeforeStep = leCircuit.Where(e => e.Key.ORDRE == lEtape.Key.ORDRE + NextOrBefore
                                && (e.Key.FK_IDRETAPEWORKFLOWORIGINE != null && e.Key.FK_IDRETAPEWORKFLOWORIGINE == lEtape.Key.FK_IDRETAPEWORKFLOWORIGINE))
                                .FirstOrDefault();
                        }
                        else //Procédure normale
                            leNextOrBeforeStep = leCircuit.Where(e => e.Key.ORDRE == lEtape.Key.ORDRE + NextOrBefore
                                && ((e.Key.FK_IDRETAPEWORKFLOWORIGINE == null && !e.Key.FK_IDRETAPEWORKFLOWORIGINE.HasValue)
                                || e.Key.FK_IDRETAPEWORKFLOWORIGINE == Guid.Empty))
                                .FirstOrDefault();
                    }
                }
            }

            //On release toutes les autres étapes
            GC.SuppressFinalize(leCircuit);
            GC.SuppressFinalize(lEtape);

            return leNextOrBeforeStep;

        }
        
        public KeyValuePair<CsCopieDmdCircuit, CsCopieDmdConditionBranchement> ProchaineOuPrecedenteEtapeByNumDem(string NumDema,
            int EtapeActuelle, Guid PKIDEtapeActuelle, int NextOrBefore, bool conditionRespecte)
        {
            CsDemandeWorkflow dmd = SelectLaDemande_NumDemande(NumDema);
            return ProchaineOuPrecedenteEtape(dmd.PK_ID, EtapeActuelle, PKIDEtapeActuelle, NextOrBefore, conditionRespecte);
        }

        public KeyValuePair<CsCopieDmdCircuit, CsCopieDmdConditionBranchement> ProchaineOuPrecedenteEtape(string codeDemande,
            int EtapeActuelle, Guid PKIDEtapeActuelle, int NextOrBefore, bool conditionRespecte)
        {
            CsDemandeWorkflow dmd = SelectLaDemande(codeDemande);
            return ProchaineOuPrecedenteEtape(dmd.PK_ID, EtapeActuelle, PKIDEtapeActuelle, NextOrBefore, conditionRespecte);
        }
        //public KeyValuePair<CsCopieDmdCircuit, CsCopieDmdConditionBranchement> RecupererEtapeCouranteByDemande(CsDemandeWorkflow dmd)
        //{
        //    KeyValuePair<CsCopieDmdCircuit, CsCopieDmdConditionBranchement> kVp = default(KeyValuePair<CsCopieDmdCircuit, CsCopieDmdConditionBranchement>);

        //    //CsDemandeWorkflow dmd = SelectLaDemande(codeDemande);
        //    //CsDemandeWorkflow dmd = SelectLaDemande_NumDemande(codeDemande);
        //    if (null != dmd)
        //    {
        //        var leCircuit = SelectAllCircuitEtapeDemandeWorkflow(dmd.CODE);
        //        Parallel.ForEach(leCircuit, (item, state) =>
        //        {
        //            if (item.Key.FK_IDETAPE == dmd.FK_IDETAPEACTUELLE)
        //            {
        //                kVp = item;
        //                state.Break();
        //            }
        //        });
        //    }
        //    return kVp;
        //}
        public KeyValuePair<CsCopieDmdCircuit, CsCopieDmdConditionBranchement> RecupererEtapeCouranteByDemande(CsDemandeWorkflow dmd)
        {
            KeyValuePair<CsCopieDmdCircuit, CsCopieDmdConditionBranchement> kVp = default(KeyValuePair<CsCopieDmdCircuit, CsCopieDmdConditionBranchement>);
            if (null != dmd)
            {
                var leCircuit = SelectAllCircuitEtapeDemandeWorkflow(dmd);
                Parallel.ForEach(leCircuit, (item, state) =>
                {
                    if (item.Key.FK_IDETAPE == dmd.FK_IDETAPEACTUELLE)
                    {
                        kVp = item;
                        state.Break();
                    }
                });
            }
            return kVp;
        }
        public KeyValuePair<CsCopieDmdCircuit, CsCopieDmdConditionBranchement> RecupererEtapeCourante(string codeDemande)
        {
            KeyValuePair<CsCopieDmdCircuit, CsCopieDmdConditionBranchement> kVp = default(KeyValuePair<CsCopieDmdCircuit, CsCopieDmdConditionBranchement>);

            //CsDemandeWorkflow dmd = SelectLaDemande(codeDemande);
            CsDemandeWorkflow dmd = SelectLaDemande_NumDemande(codeDemande);
            

            if (null != dmd)
            {
                var leCircuit = SelectAllCircuitEtapeDemandeWorkflow(dmd.CODE );
                Parallel.ForEach(leCircuit, (item, state) =>
                {
                    if (item.Key.FK_IDETAPE == dmd.FK_IDETAPEACTUELLE)
                    {
                        kVp = item;
                        state.Break();
                    }
                });
            }
            return kVp;
        }

        public KeyValuePair<CsCopieDmdCircuit, CsCopieDmdConditionBranchement> RecupererEtapeCourantebyCodeWorkflow(string codeDemande)
        {
            KeyValuePair<CsCopieDmdCircuit, CsCopieDmdConditionBranchement> kVp = default(KeyValuePair<CsCopieDmdCircuit, CsCopieDmdConditionBranchement>);

            CsDemandeWorkflow dmd = SelectLaDemande(codeDemande);
            if (null != dmd)
            {
                var leCircuit = SelectAllCircuitEtapeDemandeWorkflow(dmd.CODE);
                Parallel.ForEach(leCircuit, (item, state) =>
                {
                    if (item.Key.FK_IDETAPE == dmd.FK_IDETAPEACTUELLE)
                    {
                        kVp = item;
                        state.Break();
                    }
                });
            }
            return kVp;
        }
        public bool InsertDemande(List<CsDemandeWorkflow> lsDemandes)
        {
            try
            {
                return Entities.InsertEntity<DEMANDE_WORFKLOW>(Entities.ConvertObject<DEMANDE_WORFKLOW, CsDemandeWorkflow>(lsDemandes));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool UpdateDemande(List<CsDemandeWorkflow> lsDemandes)
        {
            try
            {
                return Entities.UpdateEntity<DEMANDE_WORFKLOW>(Entities.ConvertObject<DEMANDE_WORFKLOW, CsDemandeWorkflow>(lsDemandes));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool DeleteDemande(List<CsDemandeWorkflow> lsDemandes)
        {
            try
            {
                return Entities.DeleteEntity<DEMANDE_WORFKLOW>(Entities.ConvertObject<DEMANDE_WORFKLOW, CsDemandeWorkflow>(lsDemandes));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool TransmettreDemandeByNumDem(string NumDem, int CurrentStep, ref string msgErr, ref bool DejaAlaDerniereEtape,
     ref CsDemandeWorkflow dmd, bool ConditionRespecte = false)
        {
            try
            {
                //On fait un deuxième niveau de vérification, pour être sûre qu'elle n'est pas nulle
                //On ne sait jamais ooh
                if (null != dmd)
                {
                    bool transmission = true;
                    //On recherche la prochaine étape en précisant qu'a l'étape actuelle la condition est respectée
                    int currentStep = dmd.FK_IDETAPEACTUELLE;
                    KeyValuePair<CsCopieDmdCircuit, CsCopieDmdConditionBranchement> NextStep = ProchaineOuPrecedenteEtapeByNumDem(NumDem,
                        currentStep, (dmd.FK_IDETAPECIRCUIT.HasValue) ? dmd.FK_IDETAPECIRCUIT.Value : Guid.Empty, 1, ConditionRespecte);

                    if (null != NextStep.Key)
                    {
                        //On l'appelle la prochaine étape de la prochaine étape, en supposant que la condition n'est pas respectée
                        KeyValuePair<CsCopieDmdCircuit, CsCopieDmdConditionBranchement> NextNextStep = ProchaineOuPrecedenteEtape(dmd.PK_ID,
                                NextStep.Key.FK_IDETAPE, NextStep.Key.PK_ID, 1, false);
                        dmd.FK_IDETAPEPRECEDENTE = currentStep;
                        dmd.FK_IDETAPEACTUELLE = NextStep.Key.FK_IDETAPE;
                        dmd.FK_IDETAPESUIVANTE = (null != NextNextStep.Key) ? NextNextStep.Key.FK_IDETAPE : 0;
                        dmd.FK_IDETAPECIRCUIT = NextStep.Key.PK_ID;
                        DejaAlaDerniereEtape = false;
                    }

                    //si c'est null, c'est que nous sommes déjà à la dernière étape
                    else if (null == NextStep.Key)
                    {
                        transmission = false;
                        DejaAlaDerniereEtape = true;
                    }

                    GC.SuppressFinalize(NextStep);
                    return transmission;
                }
                else
                {
                    msgErr = "La demande n'existe pas dans la base de données";
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public bool TransmettreDemande(string codeDemande, int CurrentStep, ref string msgErr, ref bool DejaAlaDerniereEtape,
            ref CsDemandeWorkflow dmd, bool ConditionRespecte = false)
        {
            try
            {
                //On fait un deuxième niveau de vérification, pour être sûre qu'elle n'est pas nulle
                //On ne sait jamais ooh
                if (null != dmd)
                {
                    bool transmission = true;
                    //On recherche la prochaine étape en précisant qu'a l'étape actuelle la condition est respectée
                    int currentStep = dmd.FK_IDETAPEACTUELLE;
                    KeyValuePair<CsCopieDmdCircuit, CsCopieDmdConditionBranchement> NextStep = ProchaineOuPrecedenteEtape(codeDemande,
                        currentStep, (dmd.FK_IDETAPECIRCUIT.HasValue) ? dmd.FK_IDETAPECIRCUIT.Value : Guid.Empty, 1, ConditionRespecte);

                    if (null != NextStep.Key)
                    {
                        //On l'appelle la prochaine étape de la prochaine étape, en supposant que la condition n'est pas respectée
                        KeyValuePair<CsCopieDmdCircuit, CsCopieDmdConditionBranchement> NextNextStep = ProchaineOuPrecedenteEtape(dmd.PK_ID,
                                NextStep.Key.FK_IDETAPE, NextStep.Key.PK_ID, 1, false);
                        dmd.FK_IDETAPEPRECEDENTE = currentStep;
                        dmd.FK_IDETAPEACTUELLE = NextStep.Key.FK_IDETAPE;
                        dmd.FK_IDETAPESUIVANTE = (null != NextNextStep.Key) ? NextNextStep.Key.FK_IDETAPE : 0;
                        dmd.FK_IDETAPECIRCUIT = NextStep.Key.PK_ID;
                        DejaAlaDerniereEtape = false;
                    }

                    //si c'est null, c'est que nous sommes déjà à la dernière étape
                    else if (null == NextStep.Key)
                    {
                        transmission = false;
                        DejaAlaDerniereEtape = true;
                    }

                    GC.SuppressFinalize(NextStep);
                    return transmission;
                }
                else
                {
                    msgErr = "La demande n'existe pas dans la base de données";
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public bool RejeteDemandeByNumDem(string NumDem, int CurrentStep, ref string msgErr, ref bool DejaAlaPremiereEtape,
    ref CsDemandeWorkflow dmd)
        {
            try
            {
                if (null != dmd)
                {
                    bool transmission = true;
                    //On recherche l'étape précédente sans besoin de condition
                    int currentStep = dmd.FK_IDETAPEACTUELLE;
                    KeyValuePair<CsCopieDmdCircuit, CsCopieDmdConditionBranchement> PreviewStep = ProchaineOuPrecedenteEtapeByNumDem(NumDem,
                        currentStep, (dmd.FK_IDETAPECIRCUIT.HasValue) ? dmd.FK_IDETAPECIRCUIT.Value : Guid.Empty, -1, false);

                    if (null != PreviewStep.Key)
                    {
                        //On l'appelle l'étape précédente de l'étape précédente, en supposant que'il n'existe aucune
                        //condition
                        KeyValuePair<CsCopieDmdCircuit, CsCopieDmdConditionBranchement> PreviewPreviewStep = ProchaineOuPrecedenteEtapeByNumDem(NumDem,
                                PreviewStep.Key.FK_IDETAPE, PreviewStep.Key.PK_ID, -1, false);
                        dmd.FK_IDETAPEPRECEDENTE = (null != PreviewPreviewStep.Key) ? PreviewPreviewStep.Key.FK_IDETAPE : 0;
                        dmd.FK_IDETAPEACTUELLE = PreviewStep.Key.FK_IDETAPE;
                        dmd.FK_IDETAPESUIVANTE = currentStep;
                        dmd.FK_IDETAPECIRCUIT = PreviewStep.Key.PK_ID;
                        DejaAlaPremiereEtape = false;
                        transmission = true;
                    }
                    else
                    {
                        //Ah la la, ma très chère demande tu es à la 1ere étape, donc c'est comme ci, on refuse la demande
                        //mais la on ne supprime pas, ni annuler, on notifie juste qu'on ne peut pas rejeter
                        DejaAlaPremiereEtape = true;
                        transmission = false;
                        msgErr = "Déjà à la 1ere étape du circuit";
                    }

                    GC.SuppressFinalize(PreviewStep);
                    return transmission;
                }
                else
                {
                    msgErr = "La demande n'existe pas dans la base de données";
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public bool RejeteDemande(string codeDemande, int CurrentStep, ref string msgErr, ref bool DejaAlaPremiereEtape,
            ref CsDemandeWorkflow dmd)
        {
            try
            {
                if (null != dmd)
                {
                    bool transmission = true;
                    //On recherche l'étape précédente sans besoin de condition
                    int currentStep = dmd.FK_IDETAPEACTUELLE;
                    KeyValuePair<CsCopieDmdCircuit, CsCopieDmdConditionBranchement> PreviewStep = ProchaineOuPrecedenteEtape(codeDemande,
                        currentStep, (dmd.FK_IDETAPECIRCUIT.HasValue) ? dmd.FK_IDETAPECIRCUIT.Value : Guid.Empty, -1, false);

                    if (null != PreviewStep.Key)
                    {
                        //On l'appelle l'étape précédente de l'étape précédente, en supposant que'il n'existe aucune
                        //condition
                        KeyValuePair<CsCopieDmdCircuit, CsCopieDmdConditionBranchement> PreviewPreviewStep = ProchaineOuPrecedenteEtape(codeDemande,
                                PreviewStep.Key.FK_IDETAPE, PreviewStep.Key.PK_ID, -1, false);
                        dmd.FK_IDETAPEPRECEDENTE = (null != PreviewPreviewStep.Key) ? PreviewPreviewStep.Key.FK_IDETAPE : 0;
                        dmd.FK_IDETAPEACTUELLE = PreviewStep.Key.FK_IDETAPE;
                        dmd.FK_IDETAPESUIVANTE = currentStep;
                        dmd.FK_IDETAPECIRCUIT = PreviewStep.Key.PK_ID;
                        DejaAlaPremiereEtape = false;
                        transmission = true;
                    }
                    else
                    {
                        //Ah la la, ma très chère demande tu es à la 1ere étape, donc c'est comme ci, on refuse la demande
                        //mais la on ne supprime pas, ni annuler, on notifie juste qu'on ne peut pas rejeter
                        DejaAlaPremiereEtape = true;
                        transmission = false;
                        msgErr = "Déjà à la 1ere étape du circuit";
                    }

                    GC.SuppressFinalize(PreviewStep);
                    return transmission;
                }
                else
                {
                    msgErr = "La demande n'existe pas dans la base de données";
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CsStatus> SelectAllStatus()
        {
            try
            {
                return Entities.GetEntityListFromQuery<CsStatus>(ParamProcedure.LISTE_WKF_STATUS());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Dictionary<CsCopieDmdCircuit, CsCopieDmdConditionBranchement> SelectAllCircuitEtapeDemandeWorkflow(Guid pkIDDmd)
        {
            try
            {
                var lsEtapesAffWKF = Entities.GetEntityListFromQuery<CsCopieDmdCircuit>(ParamProcedure.LISTE_WKF_CIRCUIT_DEMANDE(pkIDDmd));
                //Ensuite les conditions s'il en existe
                Dictionary<CsCopieDmdCircuit, CsCopieDmdConditionBranchement> TheDatas = new Dictionary<CsCopieDmdCircuit, CsCopieDmdConditionBranchement>();
                foreach (CsCopieDmdCircuit aff in lsEtapesAffWKF)
                {
                    galadbEntities context = new galadbEntities();
                    var theGroupeValidation = context.GROUPE_VALIDATION.Where(grp => grp.PK_ID == aff.FK_IDGROUPEVALIDATIOIN)
                        .FirstOrDefault();
                    if (null != theGroupeValidation) aff.GROUPEVALIDATION = theGroupeValidation.GROUPENAME;

                    var lsConditions = Entities.GetEntityListFromQuery<CsCopieDmdConditionBranchement>(ParamProcedure.LISTE_WKF_CIRCUIT_COPIE_CONDITION(aff.PK_ID));
                    if (0 != lsConditions.Count)
                    {
                        CsCopieDmdConditionBranchement lCondition = lsConditions.FirstOrDefault();
                        aff.CONDITION = lCondition.COLONNENAME + " " + lCondition.OPERATEUR + " " + lCondition.VALUE;

                        var Step = context.ETAPE.Where(e => e.PK_ID == lCondition.FK_IDETAPEVRAIE.Value)
                            .FirstOrDefault();
                        if (null != Step) aff.ETAPECONDITIONVRAIE = Step.NOM;
                    }
                    TheDatas.Add(aff, lsConditions.FirstOrDefault());

                    context.Dispose();
                    GC.SuppressFinalize(context);
                }
                return TheDatas;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public Dictionary<CsCopieDmdCircuit, CsCopieDmdConditionBranchement> SelectAllCircuitEtapeDemandeWorkflow(CsDemandeWorkflow laDemande)
        {
            try
            {
                var lsEtapesAffWKF = Entities.GetEntityListFromQuery<CsCopieDmdCircuit>(ParamProcedure.LISTE_WKF_CIRCUIT_DEMANDE(laDemande.CODE )).FirstOrDefault(t=>t.FK_IDETAPE == laDemande.FK_IDETAPEACTUELLE );
                var theGroupeValidation = Entities.GetEntityFromQuery<CsGroupeValidation>(ParamProcedure.LISTE_WKF_GROUPES_VALIDATION_BY_ID(lsEtapesAffWKF.FK_IDGROUPEVALIDATIOIN ));
                var lsEtapeWKF = Entities.GetEntityListFromQuery<CsEtape>(ParamProcedure.LISTE_WKF_ETAPES());
                //Ensuite les conditions s'il en existe
                Dictionary<CsCopieDmdCircuit, CsCopieDmdConditionBranchement> TheDatas = new Dictionary<CsCopieDmdCircuit, CsCopieDmdConditionBranchement>();
                //foreach (CsCopieDmdCircuit aff in lsEtapesAffWKF)
                //{
                    //var theGroupeValidation = lsGroupValidationWKF.Where(grp => grp.PK_ID == aff.FK_IDGROUPEVALIDATIOIN)
                    //    .FirstOrDefault();
                    if (null != theGroupeValidation) lsEtapesAffWKF.GROUPEVALIDATION = theGroupeValidation.GROUPENAME;

                    var lsConditions = Entities.GetEntityListFromQuery<CsCopieDmdConditionBranchement>(ParamProcedure.LISTE_WKF_CIRCUIT_COPIE_CONDITION(lsEtapesAffWKF.PK_ID));
                    if (0 != lsConditions.Count)
                    {
                        CsCopieDmdConditionBranchement lCondition = lsConditions.FirstOrDefault();
                        lsEtapesAffWKF.CONDITION = lCondition.COLONNENAME + " " + lCondition.OPERATEUR + " " + lCondition.VALUE;

                        var Step = lsEtapeWKF.Where(e => e.PK_ID == lCondition.FK_IDETAPEVRAIE.Value)
                            .FirstOrDefault();
                        if (null != Step) lsEtapesAffWKF.ETAPECONDITIONVRAIE = Step.NOM;
                    }
                    TheDatas.Add(lsEtapesAffWKF, lsConditions.FirstOrDefault());
                //}
                return TheDatas;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public Dictionary<CsCopieDmdCircuit, CsCopieDmdConditionBranchement> SelectAllCircuitEtapeDemandeWorkflow(string codeDemande)
        {
            try
            {
                var lsEtapesAffWKF = Entities.GetEntityListFromQuery<CsCopieDmdCircuit>(ParamProcedure.LISTE_WKF_CIRCUIT_DEMANDE(codeDemande));
                var lsGroupValidationWKF =this.LISTE_WKF_GROUPES_VALIDATION();
                var lsEtapeWKF = Entities.GetEntityListFromQuery<CsEtape >(ParamProcedure.LISTE_WKF_ETAPES());
                //Ensuite les conditions s'il en existe
                Dictionary<CsCopieDmdCircuit, CsCopieDmdConditionBranchement> TheDatas = new Dictionary<CsCopieDmdCircuit, CsCopieDmdConditionBranchement>();
                foreach (CsCopieDmdCircuit aff in lsEtapesAffWKF)
                {
                    var theGroupeValidation = lsGroupValidationWKF.Where(grp => grp.PK_ID == aff.FK_IDGROUPEVALIDATIOIN)
                        .FirstOrDefault();
                    if (null != theGroupeValidation) aff.GROUPEVALIDATION = theGroupeValidation.GROUPENAME;

                    var lsConditions = Entities.GetEntityListFromQuery<CsCopieDmdConditionBranchement>(ParamProcedure.LISTE_WKF_CIRCUIT_COPIE_CONDITION(aff.PK_ID));
                    if (0 != lsConditions.Count)
                    {
                        CsCopieDmdConditionBranchement lCondition = lsConditions.FirstOrDefault();
                        aff.CONDITION = lCondition.COLONNENAME + " " + lCondition.OPERATEUR + " " + lCondition.VALUE;

                        var Step = lsEtapeWKF.Where(e => e.PK_ID == lCondition.FK_IDETAPEVRAIE.Value)
                            .FirstOrDefault();
                        if (null != Step) aff.ETAPECONDITIONVRAIE = Step.NOM;
                    }
                    TheDatas.Add(aff, lsConditions.FirstOrDefault());
                }
                return TheDatas;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool InsertCopieCircuit(Dictionary<CsCopieDmdCircuit, CsCopieDmdConditionBranchement> lsEtapes)
        {
            try
            {
                //On enregistre dabord les étapes
                bool insertEtape = Entities.InsertEntity<COPIE_DMD_CIRCUIT>(Entities.ConvertObject<COPIE_DMD_CIRCUIT,
                    CsCopieDmdCircuit>(lsEtapes.Keys.ToList()));
                bool insertCondition = true;
                try
                {
                    List<CsCopieDmdConditionBranchement> LesConditionsNonNulles = new List<CsCopieDmdConditionBranchement>();
                    foreach (var C in lsEtapes.Values)
                        if (null != C) LesConditionsNonNulles.Add(C);
                    if (insertEtape) insertCondition = Entities.InsertEntity<COPIE_DMD_CONDITIONBRANCHEMENT>(Entities.ConvertObject<COPIE_DMD_CONDITIONBRANCHEMENT,
                        CsCopieDmdConditionBranchement>(LesConditionsNonNulles));
                }
                catch (NullReferenceException nullEx) { }
                catch (Exception Inex) { insertCondition = false; }
                return insertCondition && insertEtape;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool CopieCicruitEtapeDemande(Guid pRWKF, Guid pkDmd, string codeDemande)
        {
            try
            {
                //On récupère la demande
                CsDemandeWorkflow dmd = SelectLaDemande(codeDemande);
                //On fait un select du circuit actuelle,
                //ensuite on le copie
                Dictionary<CsRAffectationEtapeWorkflow, CsConditionBranchement> leCircuit = SelectAllAffectationEtapeWorkflow(pRWKF);
                //La copie permet juste l'insertion avec le code de la demande et son id
                Dictionary<CsCopieDmdCircuit, CsCopieDmdConditionBranchement> copieCircuit = new Dictionary<CsCopieDmdCircuit, CsCopieDmdConditionBranchement>();
                //Ce dictionaire va nous permettre de pouvoir garder en mémoire les anciens FK_IDAFFECTATIONETAPEORIGINE,
                //Pour les circuits de détournement, afin de pouvoir les remplacer par les nouveaux PK_ID
                Dictionary<Guid, Guid> OrigineGuidToReplace = new Dictionary<Guid, Guid>();

                foreach (KeyValuePair<CsRAffectationEtapeWorkflow, CsConditionBranchement> kVp in leCircuit)
                {
                    CsCopieDmdCircuit copieAff = new CsCopieDmdCircuit()
                    {
                        PK_ID = Guid.NewGuid(), //C'est une nouvelle insertion donc un nouveau GUID
                        CODE_DEMANDE = codeDemande,
                        FK_IDDEMANDE = pkDmd,
                        CODEETAPE = kVp.Key.CODEETAPE,
                        CONDITION = kVp.Key.CONDITION,
                        ETAPECONDITIONVRAIE = kVp.Key.ETAPECONDITIONVRAIE,
                        FK_IDETAPE = kVp.Key.FK_IDETAPE,
                        FK_IDGROUPEVALIDATIOIN = kVp.Key.FK_IDGROUPEVALIDATIOIN,
                        FK_IDRETAPEWORKFLOWORIGINE = kVp.Key.FK_IDRETAPEWORKFLOWORIGINE,
                        FK_RWORKFLOWCENTRE = kVp.Key.FK_RWORKFLOWCENTRE,
                        FROMCONDITION = kVp.Key.FROMCONDITION,
                        GROUPE_VALIDATION = kVp.Key.GROUPE_VALIDATION,
                        GROUPEVALIDATION = kVp.Key.GROUPEVALIDATION,
                        LIBELLEETAPE = kVp.Key.LIBELLEETAPE,
                        ORDRE = kVp.Key.ORDRE
                    };
                    //Ensuite on essaie de voir si on a un FK_IDRETAPEWORKFLOWORIGINE à remplacer
                    if (copieAff.FK_IDRETAPEWORKFLOWORIGINE.HasValue)
                    {
                        if (OrigineGuidToReplace.Keys.Contains(copieAff.FK_IDRETAPEWORKFLOWORIGINE.Value))
                            copieAff.FK_IDRETAPEWORKFLOWORIGINE = OrigineGuidToReplace[copieAff.FK_IDRETAPEWORKFLOWORIGINE.Value];
                    }

                    //On va chercher son circuit de détournement
                    var Detournement = SelectAllAffectationEtapeCircuitDetourne(kVp.Key.PK_ID);
                    //si on a un circuit de détournement, on garde l'origine pour le remplacement
                    bool replacementPKID = (null != Detournement && 0 < Detournement.Count);
                    if (replacementPKID)
                    {
                        OrigineGuidToReplace.Add(kVp.Key.PK_ID, copieAff.PK_ID);
                    }

                    if (null != kVp.Value)
                    {
                        CsCopieDmdConditionBranchement copieCondition = new CsCopieDmdConditionBranchement()
                        {
                            PK_ID = Guid.NewGuid(),
                            FK_IDCOPIE_DMD_CIRCUIT = copieAff.PK_ID,
                            COLONNENAME = kVp.Value.COLONNENAME,
                            FK_IDETAPEFAUSE = kVp.Value.FK_IDETAPEFAUSE,
                            FK_IDETAPEVRAIE = kVp.Value.FK_IDETAPEVRAIE,
                            FK_IDTABLETRAVAIL = kVp.Value.FK_IDTABLETRAVAIL,
                            NOM = kVp.Value.NOM,
                            OPERATEUR = kVp.Value.OPERATEUR,
                            VALUE = kVp.Value.VALUE,
                            PEUT_TRANSMETTRE_SI_FAUX = kVp.Value.PEUT_TRANSMETTRE_SI_FAUX
                        };
                        copieCircuit.Add(copieAff, copieCondition);
                    }
                    else copieCircuit.Add(copieAff, null);
                }

                //On insère l'ID de la 1ere étape de son circuit à lui (la copie)
                if (null != dmd)
                {
                    var la1ereEtape = copieCircuit.Keys.Where(et => et.ORDRE == 1 && !et.FK_IDRETAPEWORKFLOWORIGINE.HasValue
                        && null == et.FK_IDRETAPEWORKFLOWORIGINE)
                        .FirstOrDefault();
                    if (null != la1ereEtape) dmd.FK_IDETAPECIRCUIT = la1ereEtape.PK_ID;
                }

                return InsertCopieCircuit(copieCircuit) && UpdateDemande(new List<CsDemandeWorkflow>() { dmd });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool CopieCicruitEtapeDemandeToGroupeValidation(Guid pRWKF, Guid pkDmd,Guid IdGroupeValidation, string codeDemande)
        {
            try
            {
                //On récupère la demande
                CsDemandeWorkflow dmd = SelectLaDemande(codeDemande);
                //On fait un select du circuit actuelle,
                //ensuite on le copie
                Dictionary<CsRAffectationEtapeWorkflow, CsConditionBranchement> leCircuit = SelectAllAffectationEtapeWorkflow(pRWKF);
                //La copie permet juste l'insertion avec le code de la demande et son id
                Dictionary<CsCopieDmdCircuit, CsCopieDmdConditionBranchement> copieCircuit = new Dictionary<CsCopieDmdCircuit, CsCopieDmdConditionBranchement>();
                //Ce dictionaire va nous permettre de pouvoir garder en mémoire les anciens FK_IDAFFECTATIONETAPEORIGINE,
                //Pour les circuits de détournement, afin de pouvoir les remplacer par les nouveaux PK_ID
                Dictionary<Guid, Guid> OrigineGuidToReplace = new Dictionary<Guid, Guid>();

                foreach (KeyValuePair<CsRAffectationEtapeWorkflow, CsConditionBranchement> kVp in leCircuit)
                {
                    Guid idGv = new Guid();
                    if (kVp.Key.FK_IDGROUPEVALIDATIOIN == Guid.Parse("5EF82549-7DFF-4127-A57B-8CC3499001B3"))  //Groupe autres
                        idGv = IdGroupeValidation;
                    else
                        idGv = kVp.Key.FK_IDGROUPEVALIDATIOIN;

                    CsCopieDmdCircuit copieAff = new CsCopieDmdCircuit()
                    {
                        PK_ID = Guid.NewGuid(), //C'est une nouvelle insertion donc un nouveau GUID
                        CODE_DEMANDE = codeDemande,
                        FK_IDDEMANDE = pkDmd,
                        CODEETAPE = kVp.Key.CODEETAPE,
                        CONDITION = kVp.Key.CONDITION,
                        ETAPECONDITIONVRAIE = kVp.Key.ETAPECONDITIONVRAIE,
                        FK_IDETAPE = kVp.Key.FK_IDETAPE,
                        FK_IDGROUPEVALIDATIOIN = idGv,
                        FK_IDRETAPEWORKFLOWORIGINE = kVp.Key.FK_IDRETAPEWORKFLOWORIGINE,
                        FK_RWORKFLOWCENTRE = kVp.Key.FK_RWORKFLOWCENTRE,
                        FROMCONDITION = kVp.Key.FROMCONDITION,
                        GROUPE_VALIDATION = kVp.Key.GROUPE_VALIDATION,
                        GROUPEVALIDATION = kVp.Key.GROUPEVALIDATION,
                        LIBELLEETAPE = kVp.Key.LIBELLEETAPE,
                        ORDRE = kVp.Key.ORDRE
                    };
                    //Ensuite on essaie de voir si on a un FK_IDRETAPEWORKFLOWORIGINE à remplacer
                    if (copieAff.FK_IDRETAPEWORKFLOWORIGINE.HasValue)
                    {
                        if (OrigineGuidToReplace.Keys.Contains(copieAff.FK_IDRETAPEWORKFLOWORIGINE.Value))
                            copieAff.FK_IDRETAPEWORKFLOWORIGINE = OrigineGuidToReplace[copieAff.FK_IDRETAPEWORKFLOWORIGINE.Value];
                    }

                    //On va chercher son circuit de détournement
                    var Detournement = SelectAllAffectationEtapeCircuitDetourne(kVp.Key.PK_ID);
                    //si on a un circuit de détournement, on garde l'origine pour le remplacement
                    bool replacementPKID = (null != Detournement && 0 < Detournement.Count);
                    if (replacementPKID)
                    {
                        OrigineGuidToReplace.Add(kVp.Key.PK_ID, copieAff.PK_ID);
                    }

                    if (null != kVp.Value)
                    {
                        CsCopieDmdConditionBranchement copieCondition = new CsCopieDmdConditionBranchement()
                        {
                            PK_ID = Guid.NewGuid(),
                            FK_IDCOPIE_DMD_CIRCUIT = copieAff.PK_ID,
                            COLONNENAME = kVp.Value.COLONNENAME,
                            FK_IDETAPEFAUSE = kVp.Value.FK_IDETAPEFAUSE,
                            FK_IDETAPEVRAIE = kVp.Value.FK_IDETAPEVRAIE,
                            FK_IDTABLETRAVAIL = kVp.Value.FK_IDTABLETRAVAIL,
                            NOM = kVp.Value.NOM,
                            OPERATEUR = kVp.Value.OPERATEUR,
                            VALUE = kVp.Value.VALUE,
                            PEUT_TRANSMETTRE_SI_FAUX = kVp.Value.PEUT_TRANSMETTRE_SI_FAUX
                        };
                        copieCircuit.Add(copieAff, copieCondition);
                    }
                    else copieCircuit.Add(copieAff, null);
                }

                //On insère l'ID de la 1ere étape de son circuit à lui (la copie)
                if (null != dmd)
                {
                    var la1ereEtape = copieCircuit.Keys.Where(et => et.ORDRE == 1 && !et.FK_IDRETAPEWORKFLOWORIGINE.HasValue
                        && null == et.FK_IDRETAPEWORKFLOWORIGINE)
                        .FirstOrDefault();
                    if (null != la1ereEtape) dmd.FK_IDETAPECIRCUIT = la1ereEtape.PK_ID;
                }

                return InsertCopieCircuit(copieCircuit) && UpdateDemande(new List<CsDemandeWorkflow>() { dmd });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool InsertJournalDemande(List<CsJournalDemandeWorkflow> lsJournaux)
        {
            try
            {
                return Entities.InsertEntity<JOURNAL_DEMANDE_WORKFLOW>(Entities.ConvertObject<JOURNAL_DEMANDE_WORKFLOW,
                    CsJournalDemandeWorkflow>(lsJournaux));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CsJournalDemandeWorkflow> SelectJournalDeLaDemande(string codeDemande)
        {
            try
            {
                return Entities.GetEntityListFromQuery<CsJournalDemandeWorkflow>(ParamProcedure.LISTE_WKF_JOURNAL_DEMANDE_WORKFLOW(codeDemande));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Gestion Tableau de bord

        public List<CsVwDashboardDemande> SelectVwDashBoardDemande()
        {
            try
            {
                return Entities.GetEntityListFromQuery<CsVwDashboardDemande>(ParamProcedure.LISTE_WKF_DASHBOARD_DEMANDE());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CsVwJournalDemande> SelectVwJournalDemande()
        {
            try
            {
                return Entities.GetEntityListFromQuery<CsVwJournalDemande>(ParamProcedure.LISTE_WKF_VW_JOURNAL_DEMANDE());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CsVwJournalDemande> SelectAllDemandeUser(List<int> lstCentreId, List<Guid> lstIdDemande, int Fk_idetape, string Matricule)
        {
            try
            {
                List<CsVwJournalDemande> lstResultat = new List<CsVwJournalDemande>();
                //List<CsVwJournalDemande> lsAffectation = Entities.GetEntityListFromQuery<CsVwJournalDemande>(ParamProcedure.LISTE_WKF_AFFECTATION_ETAPEUSER(lstIdDemande, Matricule));
                List<CsVwJournalDemande> lsDemandeWfk = RetourneDemandeWkfEtape(lstCentreId, Fk_idetape, Matricule,true );

                //lsAffectation.ForEach(t => t.UTILISATEURAFFECTE = Matricule);
                //lstResultat.AddRange(lsAffectation.Where(o=>!lsDemandeWfk.Any(p=>p.FK_IDLIGNETABLETRAVAIL == o.FK_IDLIGNETABLETRAVAIL)).ToList());
                lstResultat.AddRange(lsDemandeWfk);
                return lstResultat;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CsVwJournalDemande> SelectAllDemandeUserEtape(List<int> lstCentreId, int Fk_idetape, string Matricule)
        {
            try
            {
                List<CsVwJournalDemande> lstResultat = new List<CsVwJournalDemande>();
                //List<CsVwJournalDemande> lsAffectation = Entities.GetEntityListFromQuery<CsVwJournalDemande>(ParamProcedure.LISTE_WKF_AFFECTATION_ETAPEUSER(Fk_idetape, Matricule));
                List<CsVwJournalDemande> lsDemandeWfk = RetourneDemandeWkfEtape(lstCentreId, Fk_idetape, Matricule,false );

                //lsAffectation.ForEach(t => t.UTILISATEURAFFECTE = Matricule);
                //lstResultat.AddRange(lsAffectation.Where(o => !lsDemandeWfk.Any(p => p.FK_IDLIGNETABLETRAVAIL == o.FK_IDLIGNETABLETRAVAIL)).ToList());
                lstResultat.AddRange(lsDemandeWfk);
                return lstResultat;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CsAffectationDemandeUser> SelectLesAffectationsTraitementDemande(string matriculeUser)
        {
            try
            {
                return Entities.GetEntityListFromQuery<CsAffectationDemandeUser>(ParamProcedure.LISTE_WKF_AFFECTATION_DEMANDE_TRAITEMENT())
                    .Where(aff => aff.MATRICULEUSER == matriculeUser)
                    .ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CsAffectationDemandeUser> SelectLesAffectationsTraitementDemandeUSer(string Matricule,int IdEtape)
        {
            try
            {
                return Entities.GetEntityListFromQuery<CsAffectationDemandeUser>(ParamProcedure.LISTE_WKF_AFFECTATION_DEMANDE_TRAITEMENT(Matricule, IdEtape))
                    .ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CsAffectationDemandeUser> SelectLesAffectationsTraitementDemande()
        {
            try
            {
                return Entities.GetEntityListFromQuery<CsAffectationDemandeUser>(ParamProcedure.LISTE_WKF_AFFECTATION_DEMANDE_TRAITEMENT())
                    .ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CsAffectationDemandeUser> SelectDemandeAffecteUser(List<int>lstDemande,string Matricule)
        {
            try
            {
                return Entities.GetEntityListFromQuery<CsAffectationDemandeUser>(ParamProcedure.LISTE_WKF_AFFECTATION_ETAPEUSER(lstDemande, Matricule))
                    .ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CsAffectationDemandeUser> SelectDemandeAllUser(List<int> lstDemande, string Matricule)
        {
            try
            {
                return Entities.GetEntityListFromQuery<CsAffectationDemandeUser>(ParamProcedure.LISTE_WKF_VW_JOURNAL_DEMANDE(lstDemande, Matricule))
                    .ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool AffecterDemande(List<CsAffectationDemandeUser> lesAffectations)
        {
            try
            {
                foreach (var aff in lesAffectations)
                {
                    CsDemandeBase laDem = new DBAccueil().GetDemandeByNumIdDemande(aff.FK_IDDEMANDE);
                    bool Res = false;
                    AFFECTATIONDEMANDEUSER affec = new AFFECTATIONDEMANDEUSER();
                    using (galadbEntities ctx = new galadbEntities())
                    {
                        aff.CODEDEMANDE = laDem.NUMDEM;
                        affec = ctx.AFFECTATIONDEMANDEUSER.FirstOrDefault(o => o.FK_IDDEMANDE == aff.FK_IDDEMANDE);
                    }
                    if (affec != null)
                        {
                            /*Les nouvelle valeur */
                            affec.FK_IDUSERAFFECTER = aff.FK_IDUSERAFFECTER;
                            affec.MATRICULEUSER = aff.MATRICULEUSER;
                            affec.MATRICULEUSERCREATION = aff.MATRICULEUSERCREATION;

                            Res = Entities.UpdateEntity<AFFECTATIONDEMANDEUSER>(affec);

                        }
                        else
                            Res = Entities.InsertEntity<AFFECTATIONDEMANDEUSER>(Entities.ConvertObject<AFFECTATIONDEMANDEUSER, CsAffectationDemandeUser>(aff));
                  

                    //if (Res)
                        new DbWorkFlow().ExecuterActionSurDemandeTransction(aff.CODEDEMANDE, Enumere.TRANSMETTRE, aff.MATRICULEUSERCREATION, string.Empty, new galadbEntities());

                }
                return true;

                //return Entities.InsertEntity<AFFECTATIONDEMANDEUSER>(Entities.ConvertObject<AFFECTATIONDEMANDEUSER,
                //    CsAffectationDemandeUser>(GoodValues));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public bool AffecterDemande(List<CsAffectationDemandeUser> lesAffectations)
        //{
        //    try
        //    {
        //        foreach (var aff in lesAffectations)
        //        {
        //            CsDemandeBase laDem = new DBAccueil().GetDemandeByNumIdDemande(aff.FK_IDDEMANDE);
        //            //GoodValues.Add(new CsAffectationDemandeUser()
        //            //{
        //            //    CENTREID = aff.CENTREID,
        //            //    CODEDEMANDE = aff.CODEDEMANDE,
        //            //    FK_IDETAPE = aff.FK_IDETAPE,
        //            //    FK_IDETAPEFROM = aff.FK_IDETAPEFROM,
        //            //    MATRICULEUSER = uAffecte.MATRICULE,
        //            //    MATRICULEUSERCREATION = aff.MATRICULEUSERCREATION,
        //            //    OPERATIONID = aff.OPERATIONID,
        //            //    PK_ID = aff.PK_ID,
        //            //    WORKFLOWID = aff.WORKFLOWID
        //            //});
        //            aff.CODEDEMANDE = laDem.NUMDEM;
        //            bool Res = Entities.InsertEntity<AFFECTATIONDEMANDEUSER>(Entities.ConvertObject<AFFECTATIONDEMANDEUSER, CsAffectationDemandeUser>(aff));
        //            if (Res)
        //                new DbWorkFlow().ExecuterActionSurDemandeTransction(aff.CODEDEMANDE, Enumere.TRANSMETTRE, aff.MATRICULEUSERCREATION, string.Empty, new galadbEntities());

        //        }
        //        return true;

        //        //return Entities.InsertEntity<AFFECTATIONDEMANDEUSER>(Entities.ConvertObject<AFFECTATIONDEMANDEUSER,
        //        //    CsAffectationDemandeUser>(GoodValues));
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        public bool SupprimerAffectationDemande(List<CsAffectationDemandeUser> lsAffectations)
        {
            try
            {
                return Entities.DeleteEntity<AFFECTATIONDEMANDEUSER>(Entities.ConvertObject<AFFECTATIONDEMANDEUSER,
                    CsAffectationDemandeUser>(lsAffectations));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool SupprimerAffectationDemande(string codeDemandeAffecte)
        {
            try
            {
                List<CsAffectationDemandeUser> _affectations = new List<CsAffectationDemandeUser>();
                _affectations = SelectLesAffectationsTraitementDemande().Where(x => x.CODEDEMANDE == codeDemandeAffecte)
                    .ToList();

                return SupprimerAffectationDemande(_affectations);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CsCommentaireRejet> SelectCommentaireRejet(Guid pKIDDemande)
        {
            try
            {
                return Entities.GetEntityListFromQuery<CsCommentaireRejet>(ParamProcedure.LISTE_COMMENTAIRE_REJET(pKIDDemande))
                    .ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

 

        public bool InsertCommentaireRejet(List<CsCommentaireRejet> lsCommentaires)
        {
            try
            {
                return Entities.InsertEntity<COMMENTAIRE_REJET>(Entities.ConvertObject<COMMENTAIRE_REJET,
                    CsCommentaireRejet>(lsCommentaires));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool DeleteCommentaireRejet(Guid pKIDDemande)
        {
            try
            {
                List<CsCommentaireRejet> commentairesDeLaDemande = SelectCommentaireRejet(pKIDDemande);
                return Entities.DeleteEntity<COMMENTAIRE_REJET>(Entities.ConvertObject<COMMENTAIRE_REJET, CsCommentaireRejet>(commentairesDeLaDemande));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        public bool SupprimerCopieEtapeCircuitDemande(string codeDemande)
        {
            try
            {
                CsDemandeWorkflow demande = SelectLaDemande(codeDemande);
                if (null != demande)
                {
                    var etapeCircuit = SelectEtapesFromDemande(demande.PK_ID);
                    List<CsCopieDmdConditionBranchement> conditions = new List<CsCopieDmdConditionBranchement>();
                    foreach (var e in etapeCircuit)
                    {
                        var __conditions = SelectConditionCopieCirucit(e.PK_ID);
                        if (null != __conditions && __conditions.Count > 0) conditions.AddRange(__conditions.ToList());
                    }

                    //Suppression maintenant du circuit
                    bool deleteCondition = Entities.DeleteEntity<COPIE_DMD_CONDITIONBRANCHEMENT>(Entities.ConvertObject<COPIE_DMD_CONDITIONBRANCHEMENT, CsCopieDmdConditionBranchement>(conditions));
                    bool deleteEtape = Entities.DeleteEntity<COPIE_DMD_CIRCUIT>(Entities.ConvertObject<COPIE_DMD_CIRCUIT, CsCopieDmdCircuit>(etapeCircuit));

                    return deleteCondition && deleteEtape;
                }
                else return false;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public CsDemandeWorkflow GetLaDemandeParIdTableTravail(int pkidTableTravail, int fkidEtape)
        {
            try
            {
                var demandes = Entities.GetEntityListFromQuery<CsDemandeWorkflow>(ParamProcedure.LISTE_WKF_DEMANDEWORKFLOW_PAR_ID_TABLETRAVAIL(pkidTableTravail, fkidEtape));
                if (null != demandes && demandes.Count == 1)
                    return demandes.First();
                else return null;
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        public List<CsCopieDmdCircuit> SelectEtapesFromDemande(Guid pKIDDemande)
        {
            try
            {
                return Entities.GetEntityListFromQuery<CsCopieDmdCircuit>(ParamProcedure.LISTE_WKF_CIRCUIT_DEMANDE(pKIDDemande));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CsCopieDmdConditionBranchement> SelectConditionCopieCirucit(Guid IDEtapeCopieCircuit)
        {
            try
            {
                return Entities.GetEntityListFromQuery<CsCopieDmdConditionBranchement>(ParamProcedure.LISTE_WKF_CIRCUIT_COPIE_CONDITION(IDEtapeCopieCircuit));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        //public CsInfoDemandeWorkflow RecupererInfoDemandeParCodeTDem(CsDemandeBase laDemande)
        //{
        //    try
        //    {
        //        ////On recherche la demande selon le codedemande et l'opération
        //        var dmd = SelectLaDemande_NumDemande(laDemande.NUMDEM);
        //        var etapeCourante = new KeyValuePair<CsCopieDmdCircuit, CsCopieDmdConditionBranchement>();
        //        if (null != dmd)
        //        {
        //            List<CsEtape> etapes = SelectAllEtapesByIdEtape(dmd.FK_IDOPERATION);

        //            if (dmd.FK_IDETAPEACTUELLE != 0 && (dmd.FK_IDSTATUS != (int)Enumere.STATUSDEMANDE.Terminee && dmd.FK_IDSTATUS != (int)Enumere.STATUSDEMANDE.Annulee))
        //                etapeCourante = RecupererEtapeCouranteByDemande(dmd);
        //            List<CsUtilisateur> lsUsers = new List<CsUtilisateur>();

        //            if (etapeCourante.Key != null)
        //                lsUsers = new DBAdmUsers().GetUserByIdGroupeValidationMatriculeNom(etapeCourante.Key.FK_IDGROUPEVALIDATIOIN, dmd.FK_IDCENTRE, string.Empty, string.Empty, string.Empty);
        //            var etapeActuelle = etapes.FirstOrDefault(e => e.PK_ID == dmd.FK_IDETAPEACTUELLE);
        //            var etapeSuivante = etapes.FirstOrDefault(e => e.PK_ID == dmd.FK_IDETAPESUIVANTE);
        //            var etapePrecedente = etapes.FirstOrDefault(e => e.PK_ID == dmd.FK_IDETAPEPRECEDENTE);
        //            return new CsInfoDemandeWorkflow()
        //            {
        //                PK_ID = dmd.PK_ID,
        //                DATECREATION = dmd.DATECREATION,
        //                CODE = dmd.CODE,
        //                ETAPE_ACTUELLE = null != etapeActuelle ? etapeActuelle.NOM : string.Empty,
        //                FK_IDETAPEACTUELLE = null != etapeActuelle ? etapeActuelle.PK_ID : 0,
        //                CODEETAPE = null != etapeActuelle ? etapeActuelle.CODE : string.Empty,


        //                ETAPE_PRECEDENTE = null != etapePrecedente ? etapePrecedente.NOM : "TERMINER",
        //                ETAPE_SUIVANTE = null != etapeSuivante ? etapeSuivante.NOM : "TERMINER",
        //                FK_IDETAPESUIVANTE = null != etapeSuivante ? etapeSuivante.PK_ID : 0,
        //                LIBELLEDEMANDE = laDemande.LIBELLETYPEDEMANDE,
        //                CODE_DEMANDE_TABLE_TRAVAIL = laDemande.NUMDEM,
        //                CODETDEM = laDemande.FK_IDTYPEDEMANDE.ToString(),

        //                FK_IDOPERATION = dmd.FK_IDOPERATION,
        //                FK_IDWORKFLOW = dmd.FK_IDRWORKLOW,
        //                IDLIGNETABLETRAVAIL = dmd.FK_IDLIGNETABLETRAVAIL,
        //                UtilisateurEtapeSuivante = lsUsers,
        //                LiteRejet = GetLesRenvoisRejet(null != etapeActuelle ? etapeActuelle.PK_ID : 0)
        //            };
        //        }
        //        else return null;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //}

        public List<CsRenvoiRejet> GetLesRenvoisRejet(int fkIdEtapeActuelle)
        {
            try
            {
                return Entities.GetEntityListFromQuery<CsRenvoiRejet>(ParamProcedure.LISTE_RENVOI_REJET(fkIdEtapeActuelle))
                    .ToList();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
     
        public bool InsertRenvoiRejet(List<CsRenvoiRejet> _renvois)
        {
            try
            {
                return Entities.InsertEntity<RENVOI_REJET>(Entities.ConvertObject<RENVOI_REJET,
                    CsRenvoiRejet>(_renvois));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool DeleteRenvoiRejet(int fkIdEtapeActuelle)
        {
            try
            {
                var _renvois = GetLesRenvoisRejet(fkIdEtapeActuelle);
                return Entities.DeleteEntity<RENVOI_REJET>(Entities.ConvertObject<RENVOI_REJET, CsRenvoiRejet>(_renvois));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool DeleteRenvoiRejet(Guid RaffID)
        {
            try
            {
                var _renvois = Entities.GetEntityListFromQuery<CsRenvoiRejet>(ParamProcedure.LISTE_RENVOI_REJET(RaffID))
                    .ToList();
                return Entities.DeleteEntity<RENVOI_REJET>(Entities.ConvertObject<RENVOI_REJET, CsRenvoiRejet>(_renvois));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region Procedurestoke
        public List<CsVwJournalDemande> RetourneDemandeWkfEtapeAffecter(int Fk_idetape, string @Matricule)
        {
            cn = new SqlConnection(ConnectionString);

            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandTimeout = 3000;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SPX_WKF_JOURNAL_DEMANDE_AFFECTATION_ETAPE";
            cmd.Parameters.Add("@FK_IDETAPE", SqlDbType.Int).Value = Fk_idetape;
            cmd.Parameters.Add("@Matricule", SqlDbType.VarChar, 10).Value = @Matricule;

            DBBase.SetDBNullParametre(cmd.Parameters);
            try
            {
                if (cn.State == ConnectionState.Closed)
                    cn.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);

                return Entities.GetEntityListFromQuery<CsVwJournalDemande>(dt); ;

            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                    cn.Close(); // Fermeture de la connection 
                cmd.Dispose();
            }
        }

        public List<CsVwJournalDemande> RetourneListeDemande(List<int> lstCentreId, int Fk_idetape, string Matricule, string NumeroDemande, bool IsToutAfficher)
        {
            try
            {
                if (IsToutAfficher)
                    return RetourneDemandeWkfEtape(lstCentreId, Fk_idetape, Matricule, IsToutAfficher);
                else return RetourneDemandeWkfClient(lstCentreId, Fk_idetape, Matricule, NumeroDemande);
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }

        public List<CsVwJournalDemande> RetourneDemandeWkfClient(List<int> lstCentreId, int Fk_idetape, string Matricule,string NumeroDemande)
        {
            cn = new SqlConnection(ConnectionString);

            string Idc = DBBase.RetourneStringListeObject(lstCentreId);

            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandTimeout = 3000;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SPX_WKF_DEMANDE_ETAPE_CLIENT";
            cmd.Parameters.Add("@FK_IDETAPE", SqlDbType.Int).Value = Fk_idetape;
            cmd.Parameters.Add("@MATRICULE", SqlDbType.VarChar, 6).Value = Matricule;
            cmd.Parameters.Add("@IDCENTRELIST", SqlDbType.VarChar, int.MaxValue).Value = Idc;
            cmd.Parameters.Add("@NUMDEM", SqlDbType.VarChar, 20).Value = NumeroDemande;


            DBBase.SetDBNullParametre(cmd.Parameters);
            try
            {
                if (cn.State == ConnectionState.Closed)
                    cn.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);

                return Entities.GetEntityListFromQuery<CsVwJournalDemande>(dt); ;

            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                    cn.Close(); // Fermeture de la connection 
                cmd.Dispose();
            }
        }

        public List<CsVwJournalDemande> RetourneDemandeWkfEtape(List<int> lstCentreId, int Fk_idetape, string Matricule, bool IsToutAfficher)
        {
            cn = new SqlConnection(ConnectionString);

            string Idc = DBBase.RetourneStringListeObject(lstCentreId);

            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandTimeout = 3000;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SPX_WKF_JOURNAL_DEMANDE_ETAPE";
            cmd.Parameters.Add("@FK_IDETAPE", SqlDbType.Int).Value = Fk_idetape;
            cmd.Parameters.Add("@MATRICULE", SqlDbType.VarChar, 6).Value = Matricule;
            cmd.Parameters.Add("@IDCENTRELIST", SqlDbType.VarChar, int.MaxValue).Value = Idc;
            cmd.Parameters.Add("@ISTOUTAFFICHER", SqlDbType.Bit).Value = IsToutAfficher;


            DBBase.SetDBNullParametre(cmd.Parameters);
            try
            {
                if (cn.State == ConnectionState.Closed)
                    cn.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);

                return Entities.GetEntityListFromQuery<CsVwJournalDemande>(dt); ;

            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                    cn.Close(); // Fermeture de la connection 
                cmd.Dispose();
            }
        }
        public List<CsVwJournalDemande> RetourneDemandeWkf()
        {
            cn = new SqlConnection(ConnectionString);

            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandTimeout = 3000;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SPX_WKF_JOURNAL_DEMANDE";

            DBBase.SetDBNullParametre(cmd.Parameters);
            try
            {
                if (cn.State == ConnectionState.Closed)
                    cn.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);

                return Entities.GetEntityListFromQuery<CsVwJournalDemande>(dt); ;

            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                    cn.Close(); // Fermeture de la connection 
                cmd.Dispose();
            }
        }
        public List<CsVwDashboardDemande> RetourneTypeDemandeDashboard(List<int> lstCentre, int IdAgentConnet)
        {
            cn = new SqlConnection(ConnectionString);
            string Idc = DBBase.RetourneStringListeObject(lstCentre);

            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandTimeout = 3000;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SPX_WKF_DASHBORDDEMANDE";
            cmd.Parameters.Add("@IDCENTRELIST", SqlDbType.VarChar, int.MaxValue).Value = Idc;
            cmd.Parameters.Add("@FK_IDUTILISATEUR", SqlDbType.Int).Value = IdAgentConnet;

            DBBase.SetDBNullParametre(cmd.Parameters);
            try
            {
                if (cn.State == ConnectionState.Closed)
                    cn.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);

                return Entities.GetEntityListFromQuery<CsVwDashboardDemande>(dt); ;

            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                    cn.Close(); // Fermeture de la connection 
                cmd.Dispose();
            }
        }
        public List<CsVwDashboardDemande> RetourneDashboard()
        {
            cn = new SqlConnection(ConnectionString);

            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandTimeout = 3000;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SPX_WKF_DASHBORDDEMANDE";

            DBBase.SetDBNullParametre(cmd.Parameters);
            try
            {
                if (cn.State == ConnectionState.Closed)
                    cn.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);

                return Entities.GetEntityListFromQuery<CsVwDashboardDemande>(dt); ;

            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                    cn.Close(); // Fermeture de la connection 
                cmd.Dispose();
            }
        }
        public CsInfoDemandeWorkflow RecupererInfoDemandeParCodeDemande(CsDemandeBase laDemande )
        {
            SqlConnection laConnection = DBBase.InitConnection(ConnectionString);
            try
            {
                return RecupererInfoDemandeParCodeTDem(laDemande, laConnection);
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                if (laConnection.State == ConnectionState.Open)
                    laConnection.Close(); // Fermeture de la connection 
                 
            }
        }

        public CsInfoDemandeWorkflow RecupererInfoDemandeParCodeTDem(CsDemandeBase laDemande, SqlConnection laConnection)
        {
            try
            {
                CsInfoDemandeWorkflow InfDemande = new CsInfoDemandeWorkflow();
                cmd = new SqlCommand();
                cmd.Connection = laConnection;
                cmd.CommandTimeout = 3000;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SPX_ACC_RETOURNEETAPEDEMANDE";
                cmd.Parameters.Add("@NUMERODEMANDE", SqlDbType.VarChar, 20).Value = laDemande.NUMDEM ;
                DBBase.SetDBNullParametre(cmd.Parameters);
                try
                {
                    if (laConnection.State == ConnectionState.Closed)
                        laConnection.Open();

                    SqlDataReader reader = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(reader);

                    InfDemande = Entities.GetEntityFromQuery<CsInfoDemandeWorkflow>(dt);
                    if (InfDemande != null && !string.IsNullOrEmpty(InfDemande.CODE_DEMANDE_TABLE_TRAVAIL))
                    {
                        InfDemande.LiteRejet = new List<CsRenvoiRejet>();
                        InfDemande.LiteRejet = RetourneRejetWkfEtape(InfDemande.FK_IDETAPEACTUELLE);
                    }
                    return InfDemande;
                }
                catch (Exception ex)
                {
                    return null;
                }
                finally
                {
                    cmd.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }



        public bool UpdateCopieCircuit(string idDemande, string numDemande, Guid idGroupe)
        {
            cn = new SqlConnection(ConnectionString);
            cmd = new SqlCommand();
            cmd.Connection = cn;

            cmd.CommandTimeout = 3000;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SPX_WKF_UPDATE_GROUP_SUR_COPIECIRCUIT";

            if (cmd.Parameters.Count != 0) cmd.Parameters.Clear();

            cmd.Parameters.Add("@idDemande", SqlDbType.VarChar, 10).Value = idDemande;
            cmd.Parameters.Add("@numdem", SqlDbType.VarChar, 20).Value = numDemande;
            cmd.Parameters.Add("@idGroup", SqlDbType.UniqueIdentifier).Value = idGroupe;

            DBBase.SetDBNullParametre(cmd.Parameters);
            try
            {
                if (cmd.Connection.State == ConnectionState.Closed)
                    cmd.Connection.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                int c = -1;

                if (reader.Read())
                    c = (Convert.IsDBNull(reader["NOMBRE"])) ? 0 : (int)reader["NOMBRE"];

                reader.Close();


                return (c > 0);
            }
            catch (Exception ex)
            {
                throw new Exception(cmd.CommandText + " : " + ex.Message);
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                    cn.Close(); // Fermeture de la connection 
                cmd.Dispose();
            }

        }




        public List<CsRenvoiRejet> RetourneRejetWkfEtape(int Fk_idetape)
        {
            cn = new SqlConnection(ConnectionString);

            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandTimeout = 3000;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SPX_ACC_RETOURNEETAPEREJET";
            cmd.Parameters.Add("@ETAPEACTUELLE", SqlDbType.Int).Value = Fk_idetape;

            DBBase.SetDBNullParametre(cmd.Parameters);
            try
            {
                if (cn.State == ConnectionState.Closed)
                    cn.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);

                return Entities.GetEntityListFromQuery<CsRenvoiRejet>(dt); ;

            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                    cn.Close(); // Fermeture de la connection 
                cmd.Dispose();
            }
        }
        public List<CsCommentaireRejet> SelectCommentaireRejet(string NumDemande, SqlConnection laConnection)
        {
             
            cmd = new SqlCommand();
            cmd.Connection = laConnection;
            cmd.CommandTimeout = 3000;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SPX_WKF_SELECT_COMMENTAIRE";
            cmd.Parameters.Add("@NUMDEM", SqlDbType.VarChar ,20).Value = NumDemande;

            DBBase.SetDBNullParametre(cmd.Parameters);
            try
            {
                if (laConnection.State == ConnectionState.Closed)
                    laConnection.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);

                return Entities.GetEntityListFromQuery<CsCommentaireRejet>(dt); ;

            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                cmd.Dispose();
            }
        }
        public List<CsRHabilitationGrouveValidation> GetHabilitationGroupeValidationUser(string GrpPKID,int UserId)
        {
            cn = new SqlConnection(ConnectionString);

            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandTimeout = 3000;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SPX_WKF_LISTE_HABILITATION_GROUPE_VALIDATION_USER";
            cmd.Parameters.Add("@IDGROUPEVALIDATION", SqlDbType.VarChar ,int.MaxValue ).Value = GrpPKID;
            cmd.Parameters.Add("@USERID", SqlDbType.Int).Value = UserId;

            DBBase.SetDBNullParametre(cmd.Parameters);
            try
            {
                if (cn.State == ConnectionState.Closed)
                    cn.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);

                return Entities.GetEntityListFromQuery<CsRHabilitationGrouveValidation>(dt); ;

            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                    cn.Close(); // Fermeture de la connection 
                cmd.Dispose();
            }
        }
        public List<CsRHabilitationGrouveValidation> GetHabilitationGroupeValidation(Guid GrpPKID)
        {
            cn = new SqlConnection(ConnectionString);

            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandTimeout = 3000;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SPX_WKF_LISTE_HABILITATION_GROUPE_VALIDATION";
            cmd.Parameters.Add("@IDGROUPEVALIDATION", SqlDbType.UniqueIdentifier).Value = GrpPKID;

            DBBase.SetDBNullParametre(cmd.Parameters);
            try
            {
                if (cn.State == ConnectionState.Closed)
                    cn.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);

                return Entities.GetEntityListFromQuery<CsRHabilitationGrouveValidation>(dt); ;

            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                    cn.Close(); // Fermeture de la connection 
                cmd.Dispose();
            }
        }
        public bool VerifierAppartenanceGroupeValidation(int UserId)
        {
            cn = new SqlConnection(ConnectionString);

            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandTimeout = 3000;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SPX_WKF_VERIFIERAPPARTENANCEGROUPEVALIDATION";
            cmd.Parameters.Add("@UserId", SqlDbType.Int ).Value = @UserId;

            DBBase.SetDBNullParametre(cmd.Parameters);
            try
            {
                if (cn.State == ConnectionState.Closed)
                    cn.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);

                if (Entities.GetEntityListFromQuery<CsRHabilitationGrouveValidation>(dt).Count != 0)
                    return true;
                else return false;
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                    cn.Close(); // Fermeture de la connection 
                cmd.Dispose();
            }
        }
        public List<CsGroupeValidation> LISTE_WKF_GROUPES_VALIDATION()
        {
            cn = new SqlConnection(ConnectionString);

            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandTimeout = 3000;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SPX_WKF_SELECT_GROUPEDEVALIDATION";

            DBBase.SetDBNullParametre(cmd.Parameters);
            try
            {
                if (cn.State == ConnectionState.Closed)
                    cn.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);

                return Entities.GetEntityListFromQuery<CsGroupeValidation>(dt);
                
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                    cn.Close(); // Fermeture de la connection 
                cmd.Dispose();
            }
        }

        #endregion

    }
}
