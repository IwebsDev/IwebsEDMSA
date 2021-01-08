using Galatee.Entity.Model;
using Galatee.Structure;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galatee.DataAccess
{
    public  class DbWorkFlow
    {
        SqlConnection sqlConnection;
        SqlConnection sqlConnectionAbo07;
        private SqlCommand cmd = null;
        private SqlConnection cn = null;
        private string ConnectionString;
        private string Abo07ConnectionString;

        public DbWorkFlow()
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
        public string InsererMaDemandeBase(string PKIDLaLigne, int CodeTDem, int FKIDCentreDemande, string codeDemande_TableTravail)
        {
            string _CodeDemande = string.Empty;
            try
            {
                CsVwConfigurationWorkflowCentre wkfInfo = null;
                var LOperation = new DB_WORKFLOW().SelectAllOperation().FirstOrDefault(f => f.CODE_TDEM == CodeTDem.ToString());
                if (null != LOperation)
                {
                    Guid WorkflowId = Guid.Empty;
                    List<CsVwConfigurationWorkflowCentre> lstConfig = new DB_WORKFLOW().SelectAllConfigurationWorkflowCentre().Where(t => t.CENTREID == FKIDCentreDemande && t.OPERATIONID == LOperation.PK_ID).ToList();
                    if (lstConfig != null && lstConfig.Count != 0)
                        wkfInfo = lstConfig.First();
                    WorkflowId = (null != wkfInfo) ? wkfInfo.PK_ID : Guid.Empty;
                    return InsererMaDemande(wkfInfo.CENTREID, wkfInfo.PK_ID, LOperation.PK_ID, PKIDLaLigne, "99999", codeDemande_TableTravail);
                }
            }
            catch (Exception ex)
            {
                _CodeDemande = ex.Message;
            }
            return "ERR : " + _CodeDemande;

        }
        public string InsererMaDemande(int centreId, Guid workflowId, Guid OpId, string IDVotreLigne, string MatriculeUser,
          string CodeDeVotreDemande)
        {
            string _CodeDemande = string.Empty;
            bool _result = true;
            try
            {
                //On obtient les valeurs des paramètres
                string pk_IDLine = IDVotreLigne;
                int cId = centreId;
                Guid _wkfId = workflowId;
                Guid _opId = OpId;
                string _matUser = MatriculeUser;
                string _codeDemandeTravail = CodeDeVotreDemande;

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
                    if (!string.IsNullOrEmpty(_1sStep.CONDITION))
                    {

                        string msgErr = string.Empty;
                        CsConditionBranchement laContion = new CsConditionBranchement();
                        foreach (var item in lsRAffCircuit)
                        {
                            if (item.Key.PK_ID == _1sStep.PK_ID)
                            {
                                laContion = item.Value;
                                break;
                            }
                        }

                        //On utilise la condition pour transmettre
                        bool onABienTeste = true;
                        CsDemandeBase dmds = new DBAccueil().GetDemandeByNumIdDemande(int.Parse(IDVotreLigne));
                        bool conditionRespecte =dmds!=null? ConditionChecker.CheckIfConditionIsRespected<CsDemandeBase>(laContion.NOM, dmds,
                            ref msgErr, ref onABienTeste):false;
                        if (onABienTeste)
                        {
                            if (laContion.FK_IDETAPEVRAIE.HasValue && laContion.FK_IDETAPEFAUSE.HasValue)
                            {

                            }
                            else if (conditionRespecte && laContion.FK_IDETAPEVRAIE.HasValue && 0 != laContion.FK_IDETAPEVRAIE.Value)
                            {
                                CsRAffectationEtapeWorkflow leEtape = leCircuitNormal.FirstOrDefault(t => t.FK_IDETAPE == laContion.FK_IDETAPEVRAIE);
                                if (null == leEtape)
                                    msgErr = "ERR : Aucune étape n'a été configurée avec cette ID";
                                else
                                {
                                    _1sStep.FK_IDETAPE = leEtape.FK_IDETAPE;
                                    CsRAffectationEtapeWorkflow EtapeSuiv = leCircuitNormal.FirstOrDefault(t => t.ORDRE == leEtape.ORDRE + 1);
                                    _2ndStep.FK_IDETAPE = EtapeSuiv == null ? 0 : EtapeSuiv.FK_IDETAPE;
                                }
                            }
                        }
                    }
                    //Création de la demande

                    CsDemandeWorkflow dmd = new CsDemandeWorkflow();

                    dmd.PK_ID = Guid.NewGuid();
                    dmd.DATECREATION = DateTime.Today.Date;
                    dmd.MATRICULEUSERCREATION = _matUser;
                    dmd.ALLCENTRE = false;
                    dmd.FK_IDCENTRE = cId;
                    dmd.FK_IDOPERATION = _opId;
                    dmd.FK_IDRWORKLOW = rWKFCentre.PK_ID;
                    dmd.FK_IDSTATUS = (int)Enumere.STATUSDEMANDE.Initiee;
                    dmd.FK_IDWORKFLOW = _wkfId;
                    dmd.FK_IDLIGNETABLETRAVAIL = pk_IDLine;
                    dmd.FK_IDETAPEPRECEDENTE = 0;
                    dmd.FK_IDETAPEACTUELLE = _1sStep.FK_IDETAPE;
                    dmd.FK_IDETAPESUIVANTE = null != _2ndStep ? _2ndStep.FK_IDETAPE : 0;
                    dmd.CODE = centre.CODESITE + centre.CODE + DateTime.Today.Year + DateTime.Today.Month + DateTime.Today.Day + DateTime.Today.Hour +
                            DateTime.Now.Minute +DateTime.Now.Second +
                            DateTime.Now.Millisecond;
                    dmd.FK_IDTABLETRAVAIL = workflow.FK_IDTABLE_TRAVAIL.Value;
                    dmd.CODE_DEMANDE_TABLETRAVAIL = _codeDemandeTravail;
                    dmd.DATEDERNIEREMODIFICATION = DateTime.Today.Date;


                    _result = new DB_WORKFLOW().InsertDemande(new List<CsDemandeWorkflow>() { dmd });

                    if (_result) /* tout es bon */
                    {
                        _CodeDemande = dmd.CODE;

                        //On récupère les emails pour notifier les utilisateurs de l'arrivée de la demande
                        KeyValuePair<CsGroupeValidation, List<CsRHabilitationGrouveValidation>> grpValidation = new DB_WORKFLOW().SelectAllGroupeValidation()
                            .Where(g => g.Key.PK_ID == _1sStep.FK_IDGROUPEVALIDATIOIN)
                            .FirstOrDefault();

                        #region Création de la copie

                        _result = new DB_WORKFLOW().CopieCicruitEtapeDemande(rWKFCentre.PK_ID, dmd.PK_ID, _CodeDemande);
                        if (!_result) _CodeDemande = "Une erreur s'est produite";

                        #endregion
                    }
                }
                else
                {
                    _result = false;

                    _CodeDemande = "Aucun circuit n'a été configuré pour cette opération et ce centre";
                }
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
                //On obtient les valeurs des paramètres
                string pk_IDLine = IDVotreLigne;
                int cId = centreId;
                Guid _wkfId = workflowId;
                Guid _opId = OpId;
                string _matUser = MatriculeUser;
                string _codeDemandeTravail = CodeDeVotreDemande;

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
                    if (!string.IsNullOrEmpty(_1sStep.CONDITION))
                    {

                        string msgErr = string.Empty;
                        CsConditionBranchement laContion = new CsConditionBranchement();
                        foreach (var item in lsRAffCircuit)
                        {
                            if (item.Key.PK_ID == _1sStep.PK_ID)
                            {
                                laContion = item.Value;
                                break;
                            }
                        }

                        //On utilise la condition pour transmettre
                        bool onABienTeste = true;
                        CsDemandeBase dmds = new DBAccueil().GetDemandeByNumIdDemande(int.Parse(IDVotreLigne));
                        bool conditionRespecte = ConditionChecker.CheckIfConditionIsRespected<CsDemandeBase>(laContion.NOM, dmds,
                            ref msgErr, ref onABienTeste);
                        if (onABienTeste)
                        {
                            if (laContion.FK_IDETAPEVRAIE.HasValue && laContion.FK_IDETAPEFAUSE.HasValue)
                            {

                            }
                            else if (conditionRespecte && laContion.FK_IDETAPEVRAIE.HasValue && 0 != laContion.FK_IDETAPEVRAIE.Value)
                            {
                                CsEtape leEtape = new DB_WORKFLOW().SelectAllEtapesByIdEtape(_opId).FirstOrDefault(t => t.PK_ID == laContion.FK_IDETAPEVRAIE);
                                if (null == leEtape)
                                    msgErr = "ERR : Aucune étape n'a été configurée avec cette ID";
                                else
                                    _1sStep.FK_IDETAPE = leEtape.PK_ID;

                            }
                        }
                    }
                    //Création de la demande

                    CsDemandeWorkflow dmd = new CsDemandeWorkflow();

                    dmd.PK_ID = Guid.NewGuid();
                    dmd.DATECREATION = DateTime.Today.Date;
                    dmd.MATRICULEUSERCREATION = _matUser;
                    dmd.ALLCENTRE = false;
                    dmd.FK_IDCENTRE = cId;
                    dmd.FK_IDOPERATION = _opId;
                    dmd.FK_IDRWORKLOW = rWKFCentre.PK_ID;
                    dmd.FK_IDSTATUS = (int)STATUSDEMANDE.Initiee;
                    dmd.FK_IDWORKFLOW = _wkfId;
                    dmd.FK_IDLIGNETABLETRAVAIL = pk_IDLine;
                    dmd.FK_IDETAPEPRECEDENTE = 0;
                    dmd.FK_IDETAPEACTUELLE = _1sStep.FK_IDETAPE;
                    dmd.FK_IDETAPESUIVANTE = null != _2ndStep ? _2ndStep.FK_IDETAPE : 0;
                    dmd.CODE = centre.CODESITE + centre.CODE + DateTime.Today.Year + DateTime.Today.Month + DateTime.Today.Day + DateTime.Today.Hour +
                                   DateTime.Now.Minute + DateTime.Now.Second +
                                   DateTime.Now.Millisecond;
                    dmd.FK_IDTABLETRAVAIL = workflow.FK_IDTABLE_TRAVAIL.Value;
                    dmd.CODE_DEMANDE_TABLETRAVAIL = _codeDemandeTravail;
                    dmd.DATEDERNIEREMODIFICATION = DateTime.Today.Date;


                    _result = new DB_WORKFLOW().InsertDemande(new List<CsDemandeWorkflow>() { dmd });

                    if (_result) /* tout es bon */
                    {
                        _CodeDemande = dmd.CODE;

                        //On récupère les emails pour notifier les utilisateurs de l'arrivée de la demande
                        KeyValuePair<CsGroupeValidation, List<CsRHabilitationGrouveValidation>> grpValidation = new DB_WORKFLOW().SelectAllGroupeValidation()
                            .Where(g => g.Key.PK_ID == _1sStep.FK_IDGROUPEVALIDATIOIN)
                            .FirstOrDefault();

                        #region Création de la copie

                        _result = new DB_WORKFLOW().CopieCicruitEtapeDemandeToGroupeValidation(rWKFCentre.PK_ID, dmd.PK_ID, IdGroupeValidation, _CodeDemande);
                        if (!_result) _CodeDemande = "Une erreur s'est produite";

                        #endregion
                    }
                }
                else
                {
                    _result = false;

                    _CodeDemande = "Aucun circuit n'a été configuré pour cette opération et ce centre";
                }
            }
            catch (Exception ex)
            {
                _result = false;
                _CodeDemande = ex.Message;
            }

            if (!_result) _CodeDemande = "ERR : " + _CodeDemande;

            return _CodeDemande;
        }

        public string ExecuterActionSurDemande(string CodeDemande, int  CodeAction, string MatriculeUser, string Commentaire)
        {
            string Reponse = string.Empty;
            RESULTACTION _resultAction = RESULTACTION.ERREURINCONNUE;

            string _codeDemande = CodeDemande;
            string _matricule = MatriculeUser;
            string _commentaireRejet = Commentaire;
            string _actionOnDemande = string.Empty;

            DB_WORKFLOW dbWKF = new DB_WORKFLOW();
            CsDemandeWorkflow dmdWorkflow = null;

            switch (CodeAction)
            {
                case 1:
                    {
                        #region TRANSMETTRE

                        //On va mettre transmettre la demande à l'étape suivante
                        try
                        {
                            //On récupère toutes les infos de la demande
                            dmdWorkflow = dbWKF.SelectLaDemande(_codeDemande);
                            if (null != dmdWorkflow)
                            {
                                //Infos sur la table de travail
                                CsTableDeTravail table = dbWKF.SelectAllTableDeTravail().Where(t => t.PK_ID == dmdWorkflow.FK_IDTABLETRAVAIL)
                                    .FirstOrDefault();

                                //La demande doit être forcement liée à une table de travail                    
                                //if (null != table)        --> Une opération peut ne peut avoir une table de travail
                                //{
                                //Avant de transmettre  la demande on cherche les infos de l'étape actuelle
                                KeyValuePair<CsCopieDmdCircuit, CsCopieDmdConditionBranchement> CurrentStep = dbWKF.RecupererEtapeCourante(dmdWorkflow.CODE_DEMANDE_TABLETRAVAIL);
                                if (null != CurrentStep.Key)
                                {
                                    //On verifie la condition actuelle de l'étape
                                    bool IlYaUneCondition = (null != CurrentStep.Value
                                        && (null != CurrentStep.Key.CONDITION && string.Empty != CurrentStep.Key.CONDITION));

                                    bool transmissionOk = true;
                                    bool LastStep = false;

                                    string msgErr = string.Empty;

                                    //WCO le 12/01/2016
                                    if (IlYaUneCondition)
                                    {
                                        //On utilise la condition pour transmettre
                                        bool onABienTeste = true;

                                        CsDemandeBase dmd = new DBAccueil().GetDemandeByNumIdDemande(int.Parse(dmdWorkflow.FK_IDLIGNETABLETRAVAIL));

                                        bool conditionRespecte = ConditionChecker.CheckIfConditionIsRespected<CsDemandeBase>(CurrentStep.Value.NOM, dmd,
                                            ref msgErr, ref onABienTeste);
                                        if (onABienTeste)
                                        {
                                            if (CurrentStep.Value.FK_IDETAPEVRAIE.HasValue && CurrentStep.Value.FK_IDETAPEFAUSE.HasValue)
                                            {
                                                transmissionOk = dbWKF.TransmettreDemande(_codeDemande, dmdWorkflow.FK_IDETAPEACTUELLE, ref msgErr, ref LastStep,
                                                        ref dmdWorkflow, conditionRespecte);
                                            }
                                            else if (conditionRespecte && CurrentStep.Value.FK_IDETAPEVRAIE.HasValue && 0 != CurrentStep.Value.FK_IDETAPEVRAIE.Value)
                                            {
                                                CsCopieDmdCircuit etapeDemande = dbWKF.SelectAllCircuitEtapeDemandeWorkflow(dmdWorkflow.PK_ID)
                                                    .Keys.ToList()
                                                    .Where(e => e.FK_IDETAPE == CurrentStep.Value.FK_IDETAPEVRAIE.Value)
                                                    .FirstOrDefault();

                                                if (null == etapeDemande)
                                                {
                                                    _resultAction = RESULTACTION.ERREURINCONNUE;
                                                    msgErr = "ERR : Aucune étape n'a été configurée avec cette ID";
                                                    transmissionOk = false;
                                                }
                                                else
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
                                                }
                                            }
                                            else if ((!conditionRespecte && CurrentStep.Value.PEUT_TRANSMETTRE_SI_FAUX.HasValue && CurrentStep.Value.PEUT_TRANSMETTRE_SI_FAUX.Value)
                                                || conditionRespecte)
                                            {
                                                //On transmet quand même car la condition de saut d'étape, n'est pas respectée
                                                transmissionOk = dbWKF.TransmettreDemande(_codeDemande, dmdWorkflow.FK_IDETAPEACTUELLE, ref msgErr, ref LastStep,
                                                        ref dmdWorkflow, false);
                                            }
                                            else if (!conditionRespecte)
                                            {
                                                transmissionOk = false;
                                                msgErr = "La condition n'est pas respectée";
                                            }
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
                                            Reponse = "Une erreur interne est survenue";
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
                                            dmdWorkflow.FK_IDETAPEACTUELLE = 0;
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

                                                //On supprime la copie de la demande 
                                                
                                                /** ZEG 29/08/2017 **/
                                                //dbWKF.SupprimerCopieEtapeCircuitDemande(dmdWorkflow.CODE);
                                            }
                                        }
                                        else
                                        {
                                            //La la, ya vraiment une erreur qui s'est produite
                                            _resultAction = RESULTACTION.ERREURINCONNUE;
                                            Reponse = msgErr;
                                        }
                                    }
                                }
                                else
                                {
                                    Reponse = "Impossible d'avancer une demande n'étant à aucune étape";
                                    _resultAction = RESULTACTION.ERREURINCONNUE;
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
                                Reponse = "Impossible de transmettre une demande qui n'existe pas";
                                _resultAction = RESULTACTION.ERREURINCONNUE;
                            }
                        }
                        catch (Exception ex)
                        {
                            Reponse = ex.Message;
                            _resultAction = RESULTACTION.ERREURINCONNUE;
                        }

                        #endregion
                    }
                    break;

                case 2:
                    {
                        #region REJETER

                        try
                        {
                            //On récupère toutes les infos de la demande
                            dmdWorkflow = dbWKF.SelectLaDemande(_codeDemande);
                            //dmdWorkflow = dbWKF.SelectLaDemandePar_Code_Matricule(_codeDemande, MatriculeUser);
                            if (null != dmdWorkflow)
                            {
                                //Infos sur la table de travail
                                CsTableDeTravail table = dbWKF.SelectAllTableDeTravail().Where(t => t.PK_ID == dmdWorkflow.FK_IDTABLETRAVAIL)
                                    .FirstOrDefault();

                                //La demande doit être forcement liée à une table de travail
                                //if (null != table)
                                //{
                                //Avant de rejeter  la demande on cherche les infos de l'étape actuelle
                                KeyValuePair<CsCopieDmdCircuit, CsCopieDmdConditionBranchement> CurrentStep = dbWKF.RecupererEtapeCourante(dmdWorkflow.CODE_DEMANDE_TABLETRAVAIL);
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

                                        #region CommentaireRejet

                                        DBAdmUsers dbUser = new DBAdmUsers();
                                        CsUtilisateur u = dbUser.GetUtilisateurByMatricule(MatriculeUser);

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
                                            UTILISATEUR = (null != u && null != u.NOM && string.Empty != u.NOM) ? u.NOM : MatriculeUser,
                                            FK_IDETAPE = dmdWorkflow.FK_IDETAPEACTUELLE

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
                                            jrnal.MATRICULEUSERACTION = MatriculeUser;

                                            dbWKF.InsertJournalDemande(new List<CsJournalDemandeWorkflow>() { jrnal });

                                            //Suppresion de la demande dans la table des affectations user
                                            dbWKF.SupprimerAffectationDemande(dmdWorkflow.CODE);
                                        }
                                        else
                                        {
                                            Reponse = "Une erreur interne est survenue";
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
                                        }
                                        else
                                        {
                                            //La la, ya vraiment une erreur qui s'est produite
                                            _resultAction = RESULTACTION.ERREURINCONNUE;
                                            Reponse = msgErr;
                                        }
                                    }
                                }
                                else
                                {
                                    Reponse = "Impossible de rejeter une demande n'étant à aucune étape";
                                    _resultAction = RESULTACTION.ERREURINCONNUE;
                                }
                            }
                            else
                            {
                                Reponse = "Impossible de transmettre une demande qui n'existe pas";
                                _resultAction = RESULTACTION.ERREURINCONNUE;
                            }
                        }
                        catch (Exception ex)
                        {
                            Reponse = ex.Message;
                            _resultAction = RESULTACTION.ERREURINCONNUE;
                        }

                        #endregion
                    }
                    break;

                case 3:
                    {
                        #region ANNULER

                        dmdWorkflow = dbWKF.SelectLaDemande_NumDemande(_codeDemande);
                        if (null != dmdWorkflow)
                        {
                            dmdWorkflow.FK_IDSTATUS = (int)STATUSDEMANDE.Annulee;   //Pour la prochaine étape
                            dmdWorkflow.DATEDERNIEREMODIFICATION = DateTime.Now;
                            _resultAction = RESULTACTION.ANNULEE;
                            _actionOnDemande = "Annulation de la demande";

                            dbWKF.UpdateDemande(new List<CsDemandeWorkflow>() { dmdWorkflow });

                            CsJournalDemandeWorkflow jrnal = new CsJournalDemandeWorkflow();
                            jrnal.PK_ID = Guid.NewGuid();
                            jrnal.CODE_DEMANDE = dmdWorkflow.CODE;
                            jrnal.FK_IDDEMANDE = dmdWorkflow.PK_ID;
                            jrnal.LIBELLEACTION = _actionOnDemande;
                            jrnal.DATEACTION = DateTime.Today.Date;
                            jrnal.OBSERVATIONS = string.Empty;
                            jrnal.MATRICULEUSERACTION = MatriculeUser;

                            dbWKF.InsertJournalDemande(new List<CsJournalDemandeWorkflow>() { jrnal });

                            //Suppresion de la demande dans la table des affectations user
                            dbWKF.SupprimerAffectationDemande(dmdWorkflow.CODE);

                            _resultAction = RESULTACTION.ANNULEE;
                        }

                        #endregion
                    }
                    break;

                case 4:
                    {
                        #region SUSPENDRE

                        dmdWorkflow = dbWKF.SelectLaDemande_NumDemande(_codeDemande);
                        if (null != dmdWorkflow)
                        {
                            dmdWorkflow.FK_IDSTATUS = (int)STATUSDEMANDE.Suspendue;   //Pour la prochaine étape
                            dmdWorkflow.DATEDERNIEREMODIFICATION = DateTime.Now;
                            _resultAction = RESULTACTION.SUSPENDUE;
                            _actionOnDemande = "Annulation de la demande";

                            dbWKF.UpdateDemande(new List<CsDemandeWorkflow>() { dmdWorkflow });

                            CsJournalDemandeWorkflow jrnal = new CsJournalDemandeWorkflow();
                            jrnal.PK_ID = Guid.NewGuid();
                            jrnal.CODE_DEMANDE = dmdWorkflow.CODE;
                            jrnal.FK_IDDEMANDE = dmdWorkflow.PK_ID;
                            jrnal.LIBELLEACTION = _actionOnDemande;
                            jrnal.DATEACTION = DateTime.Today.Date;
                            jrnal.OBSERVATIONS = string.Empty;
                            jrnal.MATRICULEUSERACTION = MatriculeUser;

                            dbWKF.InsertJournalDemande(new List<CsJournalDemandeWorkflow>() { jrnal });

                            //Suppresion de la demande dans la table des affectations user
                            dbWKF.SupprimerAffectationDemande(dmdWorkflow.CODE);
                        }

                        #endregion
                    }
                    break;
            }

            if (_resultAction == RESULTACTION.ERREURINCONNUE) Reponse = "ERR : " + Reponse;
            else
            {
                if (CodeAction == Enumere.ANNULER) Reponse = "La demande a été annulée avec succès";
                else if (CodeAction == Enumere.REJETER) Reponse = "La demande a été rejetée avec succès";
                else if (CodeAction == Enumere.SUSPENDRE) Reponse = "La demande a été suspendue avec succès";
                else if (CodeAction == Enumere.TRANSMETTRE) Reponse = "La demande a été transmise avec succès";
            }

            return Reponse;
        }

        public string ExecuterActionSurDemandeTransction(string CodeDemande, int CodeAction, string MatriculeUser, string Commentaire,galadbEntities ctx)
        {
            string Reponse = string.Empty;
            RESULTACTION _resultAction = RESULTACTION.ERREURINCONNUE;

            string _codeDemande = CodeDemande;
            string _matricule = MatriculeUser;
            string _commentaireRejet = Commentaire;
            string _actionOnDemande = string.Empty;

            DB_WORKFLOW dbWKF = new DB_WORKFLOW();
            CsDemandeWorkflow dmdWorkflow = null;

            switch (CodeAction)
            {
                case 1:
                    {
                        #region TRANSMETTRE

                        //On va mettre transmettre la demande à l'étape suivante
                        try
                        {
                            //On récupère toutes les infos de la demande
                            dmdWorkflow = dbWKF.SelectLaDemande_NumDemande(_codeDemande);
                            if (null != dmdWorkflow)
                            {
                                //Infos sur la table de travail
                                CsTableDeTravail table = dbWKF.SelectAllTableDeTravail().Where(t => t.PK_ID == dmdWorkflow.FK_IDTABLETRAVAIL)
                                    .FirstOrDefault();

                                //La demande doit être forcement liée à une table de travail                    
                                //if (null != table)        --> Une opération peut ne peut avoir une table de travail
                                //{
                                //Avant de transmettre  la demande on cherche les infos de l'étape actuelle
                                KeyValuePair<CsCopieDmdCircuit, CsCopieDmdConditionBranchement> CurrentStep = dbWKF.RecupererEtapeCourante(dmdWorkflow.CODE_DEMANDE_TABLETRAVAIL);
                                if (null != CurrentStep.Key)
                                {
                                    //On verifie la condition actuelle de l'étape
                                    bool IlYaUneCondition = (null != CurrentStep.Value
                                        && (null != CurrentStep.Key.CONDITION && string.Empty != CurrentStep.Key.CONDITION));

                                    bool transmissionOk = true;
                                    bool LastStep = false;

                                    string msgErr = string.Empty;

                                    //WCO le 12/01/2016
                                    if (IlYaUneCondition)
                                    {
                                        //On utilise la condition pour transmettre
                                        bool onABienTeste = true;
                                        CsDemandeBase dmd = new DBAccueil().GetDemandeByNumIdDemande(int.Parse(dmdWorkflow.FK_IDLIGNETABLETRAVAIL));
                                        bool conditionRespecte = ConditionChecker.CheckIfConditionIsRespected<CsDemandeBase>(CurrentStep.Value.NOM, dmd,
                                            ref msgErr, ref onABienTeste);
                                        if (onABienTeste)
                                        {
                                            if (CurrentStep.Value.FK_IDETAPEVRAIE.HasValue && CurrentStep.Value.FK_IDETAPEFAUSE.HasValue)
                                            {
                                                transmissionOk = dbWKF.TransmettreDemandeByNumDem(_codeDemande, dmdWorkflow.FK_IDETAPEACTUELLE, ref msgErr, ref LastStep,
                                                        ref dmdWorkflow, conditionRespecte);
                                            }
                                            else if (conditionRespecte && CurrentStep.Value.FK_IDETAPEVRAIE.HasValue && 0 != CurrentStep.Value.FK_IDETAPEVRAIE.Value)
                                            {
                                                CsCopieDmdCircuit etapeDemande = dbWKF.SelectAllCircuitEtapeDemandeWorkflow(dmdWorkflow.PK_ID)
                                                    .Keys.ToList()
                                                    .Where(e => e.FK_IDETAPE == CurrentStep.Value.FK_IDETAPEVRAIE.Value)
                                                    .FirstOrDefault();

                                                if (null == etapeDemande)
                                                {
                                                    _resultAction = RESULTACTION.ERREURINCONNUE;
                                                    msgErr = "ERR : Aucune étape n'a été configurée avec cette ID";
                                                    transmissionOk = false;
                                                }
                                                else
                                                {
                                                    int precedent = dmdWorkflow.FK_IDETAPEACTUELLE;
                                                    dmdWorkflow.FK_IDETAPEACTUELLE = etapeDemande.FK_IDETAPE;
                                                    dmdWorkflow.FK_IDETAPECIRCUIT = etapeDemande.PK_ID;
                                                    dmdWorkflow.FK_IDETAPEPRECEDENTE = precedent;

                                                    var etapeSuivante = dbWKF.ProchaineOuPrecedenteEtape(dmdWorkflow.CODE, etapeDemande.FK_IDETAPE, etapeDemande.PK_ID, 1, conditionRespecte);
                                                    if (null != etapeSuivante.Key)
                                                    {
                                                        dmdWorkflow.FK_IDETAPESUIVANTE = etapeSuivante.Key.FK_IDETAPE;
                                                    }
                                                    else
                                                    {
                                                        //On suppose qu'on est à la dernière étape
                                                        dmdWorkflow.FK_IDETAPESUIVANTE = 0;
                                                    }
                                                }
                                            }
                                            else if ((!conditionRespecte && CurrentStep.Value.PEUT_TRANSMETTRE_SI_FAUX.HasValue && CurrentStep.Value.PEUT_TRANSMETTRE_SI_FAUX.Value)
                                                || conditionRespecte)
                                            {
                                                //On transmet quand même car la condition de saut d'étape, n'est pas respectée
                                                transmissionOk = dbWKF.TransmettreDemandeByNumDem(_codeDemande, dmdWorkflow.FK_IDETAPEACTUELLE, ref msgErr, ref LastStep,
                                                        ref dmdWorkflow, false);
                                            }
                                            else if (!conditionRespecte)
                                            {
                                                transmissionOk = false;
                                                msgErr = "La condition n'est pas respectée";
                                            }
                                        }
                                        else transmissionOk = false;
                                    }
                                    else
                                    {
                                        //On transmet sans condition
                                        transmissionOk = dbWKF.TransmettreDemandeByNumDem(_codeDemande, dmdWorkflow.FK_IDETAPEACTUELLE, ref msgErr, ref LastStep,
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
                                            Reponse = "Une erreur interne est survenue";
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
                                            dmdWorkflow.FK_IDETAPEACTUELLE = 0;
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

                                                //On supprime la copie de la demande 

                                                /** ZEG 29/08/2017 **/
                                                //dbWKF.SupprimerCopieEtapeCircuitDemande(dmdWorkflow.CODE);
                                            }
                                        }
                                        else
                                        {
                                            //La la, ya vraiment une erreur qui s'est produite
                                            _resultAction = RESULTACTION.ERREURINCONNUE;
                                            Reponse = msgErr;
                                        }
                                    }
                                }
                                else
                                {
                                    Reponse = "Impossible d'avancer une demande n'étant à aucune étape";
                                    _resultAction = RESULTACTION.ERREURINCONNUE;
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
                                Reponse = "Impossible de transmettre une demande qui n'existe pas";
                                _resultAction = RESULTACTION.ERREURINCONNUE;
                            }
                        }
                        catch (Exception ex)
                        {
                            Reponse = ex.Message;
                            _resultAction = RESULTACTION.ERREURINCONNUE;
                        }

                        #endregion
                    }
                    break;

                case 2:
                    {
                        #region REJETER

                        try
                        {
                            //On récupère toutes les infos de la demande
                            dmdWorkflow = dbWKF.SelectLaDemande_NumDemande(_codeDemande);
                            if (null != dmdWorkflow)
                            {
                                //Infos sur la table de travail
                                CsTableDeTravail table = dbWKF.SelectAllTableDeTravail().Where(t => t.PK_ID == dmdWorkflow.FK_IDTABLETRAVAIL)
                                    .FirstOrDefault();

                                //La demande doit être forcement liée à une table de travail
                                //if (null != table)
                                //{
                                //Avant de rejeter  la demande on cherche les infos de l'étape actuelle
                                KeyValuePair<CsCopieDmdCircuit, CsCopieDmdConditionBranchement> CurrentStep = dbWKF.RecupererEtapeCourante(dmdWorkflow.CODE_DEMANDE_TABLETRAVAIL);
                                if (null != CurrentStep.Key)
                                {
                                    //ici on pas besoin de condition, on rejette ooh
                                    bool LastStep = false;
                                    string msgErr = string.Empty;
                                    //On rejette
                                    if (dbWKF.RejeteDemandeByNumDem(_codeDemande, dmdWorkflow.FK_IDETAPEACTUELLE, ref msgErr, ref LastStep,
                                        ref dmdWorkflow))
                                    {
                                        //On a bien transmis,
                                        dmdWorkflow.FK_IDSTATUS = (int)STATUSDEMANDE.Rejetee;   //Pour la prochaine étape
                                        dmdWorkflow.DATEDERNIEREMODIFICATION = DateTime.Now;
                                        _resultAction = RESULTACTION.REJETEE;
                                        _actionOnDemande = "Rejet de l'étape " + CurrentStep.Key.LIBELLEETAPE;

                                        #region CommentaireRejet

                                        DBAdmUsers dbUser = new DBAdmUsers();
                                        CsUtilisateur u = dbUser.GetUtilisateurByMatricule(MatriculeUser);

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
                                            UTILISATEUR = (null != u && null != u.NOM && string.Empty != u.NOM) ? u.NOM : MatriculeUser
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
                                            jrnal.MATRICULEUSERACTION = MatriculeUser;

                                            dbWKF.InsertJournalDemande(new List<CsJournalDemandeWorkflow>() { jrnal });

                                            //Suppresion de la demande dans la table des affectations user
                                            dbWKF.SupprimerAffectationDemande(dmdWorkflow.CODE);
                                        }
                                        else
                                        {
                                            Reponse = "Une erreur interne est survenue";
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
                                        }
                                        else
                                        {
                                            //La la, ya vraiment une erreur qui s'est produite
                                            _resultAction = RESULTACTION.ERREURINCONNUE;
                                            Reponse = msgErr;
                                        }
                                    }
                                }
                                else
                                {
                                    Reponse = "Impossible de rejeter une demande n'étant à aucune étape";
                                    _resultAction = RESULTACTION.ERREURINCONNUE;
                                }
                            }
                            else
                            {
                                Reponse = "Impossible de transmettre une demande qui n'existe pas";
                                _resultAction = RESULTACTION.ERREURINCONNUE;
                            }
                        }
                        catch (Exception ex)
                        {
                            Reponse = ex.Message;
                            _resultAction = RESULTACTION.ERREURINCONNUE;
                        }

                        #endregion
                    }
                    break;

                case 3:
                    {
                        #region ANNULER

                        dmdWorkflow = dbWKF.SelectLaDemande_NumDemande(_codeDemande);
                        if (null != dmdWorkflow)
                        {
                            dmdWorkflow.FK_IDSTATUS = (int)STATUSDEMANDE.Annulee;   //Pour la prochaine étape
                            dmdWorkflow.DATEDERNIEREMODIFICATION = DateTime.Now;
                            _resultAction = RESULTACTION.ANNULEE;
                            _actionOnDemande = "Annulation de la demande";

                            dbWKF.UpdateDemande(new List<CsDemandeWorkflow>() { dmdWorkflow });

                            CsJournalDemandeWorkflow jrnal = new CsJournalDemandeWorkflow();
                            jrnal.PK_ID = Guid.NewGuid();
                            jrnal.CODE_DEMANDE = dmdWorkflow.CODE;
                            jrnal.FK_IDDEMANDE = dmdWorkflow.PK_ID;
                            jrnal.LIBELLEACTION = _actionOnDemande;
                            jrnal.DATEACTION = DateTime.Today.Date;
                            jrnal.OBSERVATIONS = string.Empty;
                            jrnal.MATRICULEUSERACTION = MatriculeUser;

                            dbWKF.InsertJournalDemande(new List<CsJournalDemandeWorkflow>() { jrnal });

                            //Suppresion de la demande dans la table des affectations user
                            dbWKF.SupprimerAffectationDemande(dmdWorkflow.CODE);

                            _resultAction = RESULTACTION.ANNULEE;
                        }

                        #endregion
                    }
                    break;

                case 4:
                    {
                        #region SUSPENDRE

                        dmdWorkflow = dbWKF.SelectLaDemande_NumDemande(_codeDemande);
                        if (null != dmdWorkflow)
                        {
                            dmdWorkflow.FK_IDSTATUS = (int)STATUSDEMANDE.Suspendue;   //Pour la prochaine étape
                            dmdWorkflow.DATEDERNIEREMODIFICATION = DateTime.Now;
                            _resultAction = RESULTACTION.SUSPENDUE;
                            _actionOnDemande = "Annulation de la demande";

                            dbWKF.UpdateDemande(new List<CsDemandeWorkflow>() { dmdWorkflow });

                            CsJournalDemandeWorkflow jrnal = new CsJournalDemandeWorkflow();
                            jrnal.PK_ID = Guid.NewGuid();
                            jrnal.CODE_DEMANDE = dmdWorkflow.CODE;
                            jrnal.FK_IDDEMANDE = dmdWorkflow.PK_ID;
                            jrnal.LIBELLEACTION = _actionOnDemande;
                            jrnal.DATEACTION = DateTime.Today.Date;
                            jrnal.OBSERVATIONS = string.Empty;
                            jrnal.MATRICULEUSERACTION = MatriculeUser;

                            dbWKF.InsertJournalDemande(new List<CsJournalDemandeWorkflow>() { jrnal });

                            //Suppresion de la demande dans la table des affectations user
                            dbWKF.SupprimerAffectationDemande(dmdWorkflow.CODE);
                        }

                        #endregion
                    }
                    break;
            }

            if (_resultAction == RESULTACTION.ERREURINCONNUE) Reponse = "ERR : " + Reponse;
            else
            {
                if (CodeAction == Enumere.ANNULER) Reponse = "La demande a été annulée avec succès";
                else if (CodeAction == Enumere.REJETER) Reponse = "La demande a été rejetée avec succès";
                else if (CodeAction == Enumere.SUSPENDRE) Reponse = "La demande a été suspendue avec succès";
                else if (CodeAction == Enumere.TRANSMETTRE) Reponse = "La demande a été transmise avec succès";
            }

            return Reponse;
        }



        public string ExecuterActionSurDemandeNumDem(string CodeDemande, int CodeAction, string MatriculeUser, string Commentaire)
        {
            string Reponse = string.Empty;
            RESULTACTION _resultAction = RESULTACTION.ERREURINCONNUE;

            string _codeDemande = CodeDemande;
            string _matricule = MatriculeUser;
            string _commentaireRejet = Commentaire;
            string _actionOnDemande = string.Empty;

            DB_WORKFLOW dbWKF = new DB_WORKFLOW();
            CsDemandeWorkflow dmdWorkflow = null;

            switch (CodeAction)
            {
                case 1:
                    {
                        #region TRANSMETTRE

                        //On va mettre transmettre la demande à l'étape suivante
                        try
                        {
                            //On récupère toutes les infos de la demande
                            dmdWorkflow = dbWKF.SelectLaDemande_NumDemande(_codeDemande);
                            if (null != dmdWorkflow)
                            {
                                //Infos sur la table de travail
                                CsTableDeTravail table = dbWKF.SelectAllTableDeTravail().Where(t => t.PK_ID == dmdWorkflow.FK_IDTABLETRAVAIL)
                                    .FirstOrDefault();

                                //La demande doit être forcement liée à une table de travail                    
                                //if (null != table)        --> Une opération peut ne peut avoir une table de travail
                                //{
                                //Avant de transmettre  la demande on cherche les infos de l'étape actuelle
                                KeyValuePair<CsCopieDmdCircuit, CsCopieDmdConditionBranchement> CurrentStep = dbWKF.RecupererEtapeCourante(dmdWorkflow.CODE_DEMANDE_TABLETRAVAIL);
                                if (null != CurrentStep.Key)
                                {
                                    //On verifie la condition actuelle de l'étape
                                    bool IlYaUneCondition = (null != CurrentStep.Value
                                        && (null != CurrentStep.Key.CONDITION && string.Empty != CurrentStep.Key.CONDITION));

                                    bool transmissionOk = true;
                                    bool LastStep = false;

                                    string msgErr = string.Empty;

                                    //WCO le 12/01/2016
                                    if (IlYaUneCondition)
                                    {
                                        //On utilise la condition pour transmettre
                                        bool onABienTeste = true;

                                        CsDemandeBase dmd = new DBAccueil().GetDemandeByNumIdDemande(int.Parse(dmdWorkflow.FK_IDLIGNETABLETRAVAIL));

                                        bool conditionRespecte = ConditionChecker.CheckIfConditionIsRespected<CsDemandeBase>(CurrentStep.Value.NOM, dmd,
                                            ref msgErr, ref onABienTeste);
                                        if (onABienTeste)
                                        {
                                            if (CurrentStep.Value.FK_IDETAPEVRAIE.HasValue && CurrentStep.Value.FK_IDETAPEFAUSE.HasValue)
                                            {
                                                transmissionOk = dbWKF.TransmettreDemandeByNumDem(_codeDemande, dmdWorkflow.FK_IDETAPEACTUELLE, ref msgErr, ref LastStep,
                                                        ref dmdWorkflow, conditionRespecte);
                                            }
                                            else if (conditionRespecte && CurrentStep.Value.FK_IDETAPEVRAIE.HasValue && 0 != CurrentStep.Value.FK_IDETAPEVRAIE.Value)
                                            {
                                                CsCopieDmdCircuit etapeDemande = dbWKF.SelectAllCircuitEtapeDemandeWorkflow(dmdWorkflow.PK_ID)
                                                    .Keys.ToList()
                                                    .Where(e => e.FK_IDETAPE == CurrentStep.Value.FK_IDETAPEVRAIE.Value)
                                                    .FirstOrDefault();

                                                if (null == etapeDemande)
                                                {
                                                    _resultAction = RESULTACTION.ERREURINCONNUE;
                                                    msgErr = "ERR : Aucune étape n'a été configurée avec cette ID";
                                                    transmissionOk = false;
                                                }
                                                else
                                                {
                                                    int precedent = dmdWorkflow.FK_IDETAPEACTUELLE;
                                                    dmdWorkflow.FK_IDETAPEACTUELLE = etapeDemande.FK_IDETAPE;
                                                    dmdWorkflow.FK_IDETAPECIRCUIT = etapeDemande.PK_ID;
                                                    dmdWorkflow.FK_IDETAPEPRECEDENTE = precedent;

                                                    var etapeSuivante = dbWKF.ProchaineOuPrecedenteEtape(dmdWorkflow.CODE, etapeDemande.FK_IDETAPE, etapeDemande.PK_ID, 1, conditionRespecte);
                                                    if (null != etapeSuivante.Key)
                                                    {
                                                        dmdWorkflow.FK_IDETAPESUIVANTE = etapeSuivante.Key.FK_IDETAPE;
                                                    }
                                                    else
                                                    {
                                                        //On suppose qu'on est à la dernière étape
                                                        dmdWorkflow.FK_IDETAPESUIVANTE = 0;
                                                    }
                                                }
                                            }
                                            else if ((!conditionRespecte && CurrentStep.Value.PEUT_TRANSMETTRE_SI_FAUX.HasValue && CurrentStep.Value.PEUT_TRANSMETTRE_SI_FAUX.Value)
                                                || conditionRespecte)
                                            {
                                                //On transmet quand même car la condition de saut d'étape, n'est pas respectée
                                                transmissionOk = dbWKF.TransmettreDemandeByNumDem(_codeDemande, dmdWorkflow.FK_IDETAPEACTUELLE, ref msgErr, ref LastStep,
                                                        ref dmdWorkflow, false);
                                            }
                                            else if (!conditionRespecte)
                                            {
                                                transmissionOk = false;
                                                msgErr = "La condition n'est pas respectée";
                                            }
                                        }
                                        else transmissionOk = false;
                                    }
                                    else
                                    {
                                        //On transmet sans condition
                                        transmissionOk = dbWKF.TransmettreDemandeByNumDem(_codeDemande, dmdWorkflow.FK_IDETAPEACTUELLE, ref msgErr, ref LastStep,
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
                                            Reponse = "Une erreur interne est survenue";
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
                                            dmdWorkflow.FK_IDETAPEACTUELLE = 0;
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

                                                //On supprime la copie de la demande 
                                                
                                                /** ZEG 29/08/2017 **/
                                                //dbWKF.SupprimerCopieEtapeCircuitDemande(dmdWorkflow.CODE);
                                            }
                                        }
                                        else
                                        {
                                            //La la, ya vraiment une erreur qui s'est produite
                                            _resultAction = RESULTACTION.ERREURINCONNUE;
                                            Reponse = msgErr;
                                        }
                                    }
                                }
                                else
                                {
                                    Reponse = "Impossible d'avancer une demande n'étant à aucune étape";
                                    _resultAction = RESULTACTION.ERREURINCONNUE;
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
                                Reponse = "Impossible de transmettre une demande qui n'existe pas";
                                _resultAction = RESULTACTION.ERREURINCONNUE;
                            }
                        }
                        catch (Exception ex)
                        {
                            Reponse = ex.Message;
                            _resultAction = RESULTACTION.ERREURINCONNUE;
                        }

                        #endregion
                    }
                    break;

                case 2:
                    {
                        #region REJETER

                        try
                        {
                            //On récupère toutes les infos de la demande
                            dmdWorkflow = dbWKF.SelectLaDemande_NumDemande(_codeDemande);
                            if (null != dmdWorkflow)
                            {
                                //Infos sur la table de travail
                                CsTableDeTravail table = dbWKF.SelectAllTableDeTravail().Where(t => t.PK_ID == dmdWorkflow.FK_IDTABLETRAVAIL)
                                    .FirstOrDefault();

                                //La demande doit être forcement liée à une table de travail
                                //if (null != table)
                                //{
                                //Avant de rejeter  la demande on cherche les infos de l'étape actuelle
                                KeyValuePair<CsCopieDmdCircuit, CsCopieDmdConditionBranchement> CurrentStep = dbWKF.RecupererEtapeCourante(dmdWorkflow.CODE_DEMANDE_TABLETRAVAIL);
                                if (null != CurrentStep.Key)
                                {
                                    //ici on pas besoin de condition, on rejette ooh
                                    bool LastStep = false;
                                    string msgErr = string.Empty;
                                    //On rejette
                                    if (dbWKF.RejeteDemandeByNumDem(_codeDemande, dmdWorkflow.FK_IDETAPEACTUELLE, ref msgErr, ref LastStep,
                                        ref dmdWorkflow))
                                    {
                                        //On a bien transmis,
                                        dmdWorkflow.FK_IDSTATUS = (int)STATUSDEMANDE.Rejetee;   //Pour la prochaine étape
                                        dmdWorkflow.DATEDERNIEREMODIFICATION = DateTime.Now;
                                        _resultAction = RESULTACTION.REJETEE;
                                        _actionOnDemande = "Rejet de l'étape " + CurrentStep.Key.LIBELLEETAPE;

                                        #region CommentaireRejet

                                        DBAdmUsers dbUser = new DBAdmUsers();
                                        CsUtilisateur u = dbUser.GetUtilisateurByMatricule(MatriculeUser);

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
                                            UTILISATEUR = (null != u && null != u.NOM && string.Empty != u.NOM) ? u.NOM : MatriculeUser
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
                                            jrnal.MATRICULEUSERACTION = MatriculeUser;

                                            dbWKF.InsertJournalDemande(new List<CsJournalDemandeWorkflow>() { jrnal });

                                            //Suppresion de la demande dans la table des affectations user
                                            dbWKF.SupprimerAffectationDemande(dmdWorkflow.CODE);
                                        }
                                        else
                                        {
                                            Reponse = "Une erreur interne est survenue";
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
                                        }
                                        else
                                        {
                                            //La la, ya vraiment une erreur qui s'est produite
                                            _resultAction = RESULTACTION.ERREURINCONNUE;
                                            Reponse = msgErr;
                                        }
                                    }
                                }
                                else
                                {
                                    Reponse = "Impossible de rejeter une demande n'étant à aucune étape";
                                    _resultAction = RESULTACTION.ERREURINCONNUE;
                                }
                            }
                            else
                            {
                                Reponse = "Impossible de transmettre une demande qui n'existe pas";
                                _resultAction = RESULTACTION.ERREURINCONNUE;
                            }
                        }
                        catch (Exception ex)
                        {
                            Reponse = ex.Message;
                            _resultAction = RESULTACTION.ERREURINCONNUE;
                        }

                        #endregion
                    }
                    break;

                case 3:
                    {
                        #region ANNULER

                        dmdWorkflow = dbWKF.SelectLaDemande_NumDemande(_codeDemande);
                        if (null != dmdWorkflow)
                        {
                            dmdWorkflow.FK_IDSTATUS = (int)STATUSDEMANDE.Annulee;   //Pour la prochaine étape
                            dmdWorkflow.DATEDERNIEREMODIFICATION = DateTime.Now;
                            _resultAction = RESULTACTION.ANNULEE;
                            _actionOnDemande = "Annulation de la demande";

                            dbWKF.UpdateDemande(new List<CsDemandeWorkflow>() { dmdWorkflow });

                            CsJournalDemandeWorkflow jrnal = new CsJournalDemandeWorkflow();
                            jrnal.PK_ID = Guid.NewGuid();
                            jrnal.CODE_DEMANDE = dmdWorkflow.CODE;
                            jrnal.FK_IDDEMANDE = dmdWorkflow.PK_ID;
                            jrnal.LIBELLEACTION = _actionOnDemande;
                            jrnal.DATEACTION = DateTime.Today.Date;
                            jrnal.OBSERVATIONS = string.Empty;
                            jrnal.MATRICULEUSERACTION = MatriculeUser;

                            dbWKF.InsertJournalDemande(new List<CsJournalDemandeWorkflow>() { jrnal });

                            //Suppresion de la demande dans la table des affectations user
                            dbWKF.SupprimerAffectationDemande(dmdWorkflow.CODE);

                            _resultAction = RESULTACTION.ANNULEE;
                        }

                        #endregion
                    }
                    break;

                case 4:
                    {
                        #region SUSPENDRE

                        dmdWorkflow = dbWKF.SelectLaDemande_NumDemande(_codeDemande);
                        if (null != dmdWorkflow)
                        {
                            dmdWorkflow.FK_IDSTATUS = (int)STATUSDEMANDE.Suspendue;   //Pour la prochaine étape
                            dmdWorkflow.DATEDERNIEREMODIFICATION = DateTime.Now;
                            _resultAction = RESULTACTION.SUSPENDUE;
                            _actionOnDemande = "Annulation de la demande";

                            dbWKF.UpdateDemande(new List<CsDemandeWorkflow>() { dmdWorkflow });

                            CsJournalDemandeWorkflow jrnal = new CsJournalDemandeWorkflow();
                            jrnal.PK_ID = Guid.NewGuid();
                            jrnal.CODE_DEMANDE = dmdWorkflow.CODE;
                            jrnal.FK_IDDEMANDE = dmdWorkflow.PK_ID;
                            jrnal.LIBELLEACTION = _actionOnDemande;
                            jrnal.DATEACTION = DateTime.Today.Date;
                            jrnal.OBSERVATIONS = string.Empty;
                            jrnal.MATRICULEUSERACTION = MatriculeUser;

                            dbWKF.InsertJournalDemande(new List<CsJournalDemandeWorkflow>() { jrnal });

                            //Suppresion de la demande dans la table des affectations user
                            dbWKF.SupprimerAffectationDemande(dmdWorkflow.CODE);
                        }

                        #endregion
                    }
                    break;
            }

            if (_resultAction == RESULTACTION.ERREURINCONNUE) Reponse = "ERR : " + Reponse;
            else
            {
                if (CodeAction == Enumere.ANNULER) Reponse = "La demande a été annulée avec succès";
                else if (CodeAction == Enumere.REJETER) Reponse = "La demande a été rejetée avec succès";
                else if (CodeAction == Enumere.SUSPENDRE) Reponse = "La demande a été suspendue avec succès";
                else if (CodeAction == Enumere.TRANSMETTRE) Reponse = "La demande a été transmise avec succès";
            }

            return Reponse;
        }


        public string AllerALEtape(string CodeDemande, int CodeAction, Guid EtapeId, string MatriculeUser, string Commentaire)
        {
            RESULTACTION _result = RESULTACTION.ERREURINCONNUE;
            string Reponse = string.Empty;

            try
            {
                string _codeDemande = CodeDemande;
                string _commentaire = Commentaire;
                Guid _fkIdEtape = EtapeId;
                string _matriculeUser = MatriculeUser;


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
                        Reponse = "ERR : Aucune étape n'a été configurée avec cette ID";

                        return Reponse;
                    }


                    switch (CodeAction)
                    {
                        case 1:
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

                        case 2:
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
                Reponse = "ERR : " + ex.Message;
            }

            return Reponse;
        }

        public string ExecuterActionSurPlusieursDemandes(List<string> CodesDemandes, int CodeAction, string MatriculeUser, string Commentaire)
        {
            //Pour chaque demande, on appelle le service interne ExecuterAction
            string response = string.Empty;
            if (null != CodesDemandes && CodesDemandes.Count > 0)
            {
                foreach (string str in CodesDemandes)
                {
                    response = ExecuterActionSurDemande(str, CodeAction, MatriculeUser, Commentaire);
                    if (response.StartsWith("ERR")) break;
                }
            }

            if (!response.StartsWith("ERR"))
            {
                if (CodeAction == Enumere.TRANSMETTRE) response = "Les demandes ont été transmises avec succès";
                else if (CodeAction == Enumere.REJETER) response = "Les demandes ont été rejetées avec succès";
            }

            return response;
        }

        public string ExecuterActionSurDemandeParPkIDLigne(List<int> pkIds, int FkidEtapeActuelle, int CodeAction, string MatriculeUser, string Commentaire)
        {
            //On va récupérer la demande en fonction de l'id de la ligne de la table de travail renseigné
            DB_WORKFLOW dbWkf = new DB_WORKFLOW();
            List<string> CodesDemandes = new List<string>();
            foreach (int i in pkIds)
            {
                var dmd = dbWkf.GetLaDemandeParIdTableTravail(i, FkidEtapeActuelle);
                if (null != dmd)
                {
                    CodesDemandes.Add(dmd.CODE);
                    GC.SuppressFinalize(dmd);
                }
            }
            //On delete le dbwkf pour vider la mémoire
            GC.SuppressFinalize(dbWkf);

            //Ensuite on parcours chaque codedemande pour effectué l'action sur celle-ci
            string response = string.Empty;
            if (null != CodesDemandes && CodesDemandes.Count > 0)
            {
                foreach (string str in CodesDemandes)
                {
                    response = ExecuterActionSurDemande(str, CodeAction, MatriculeUser, Commentaire);
                    if (response.StartsWith("ERR")) break;
                }
            }

            if (!response.StartsWith("ERR"))
            {
                if (CodeAction == Enumere.TRANSMETTRE) response = "Les demandes ont été transmises avec succès";
                else if (CodeAction == Enumere.REJETER) response = "Les demandes ont été rejetées avec succès";
            }

            return response;
        }

        public bool VerifierConditionDemande(string codeDemande, int FKIDTableTravail, string PKIDLigne)
        {
            try
            {
                //Récupération des infos de la table de travail
                var table = new DB_WORKFLOW().SelectAllTableDeTravail().Where(t => t.PK_ID == FKIDTableTravail)
                    .FirstOrDefault();
                if (null != table)
                {
                    string msgErr = string.Empty;
                    bool conditionOk = true;

                }

                return true;
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
                //On sélection l'opération
                DB_WORKFLOW dbWorkflow = new DB_WORKFLOW();
                DBAccueil dbAccueil = new DBAccueil();
                List<CsCentre> centres = dbAccueil.RetourneCentre();
                List<CsSite> sites = dbAccueil.RetourneTousSite();
                var Op = dbWorkflow.SelectAllOperation2().FirstOrDefault(o => o.PK_ID == Operation);
                if (null != Op)
                {
                    List<CsEtape> etapes = dbWorkflow.SelectAllEtapes(Op.PK_ID);
                    //On recherche la demande selon le codedemande et l'opération
                    var dmd = dbWorkflow.SelectAllDemande().FirstOrDefault(d => d.CODE == codeDemande && d.FK_IDOPERATION == Op.PK_ID);
                    if (null != dmd)
                    {
                        var c = centres.FirstOrDefault(_c => _c.PK_ID == dmd.FK_IDCENTRE);
                        var etapeActuelle = etapes.FirstOrDefault(e => e.PK_ID == dmd.FK_IDETAPEACTUELLE);
                        var etapeSuivante = etapes.FirstOrDefault(e => e.PK_ID == dmd.FK_IDETAPESUIVANTE);
                        var etapePrecedente = etapes.FirstOrDefault(e => e.PK_ID == dmd.FK_IDETAPEPRECEDENTE);
                        return new CsInfoDemandeWorkflow()
                        {
                            PK_ID = dmd.PK_ID,
                            DATECREATION = dmd.DATECREATION,
                            CODE = dmd.CODE,
                            CENTRE = null != c ? c.LIBELLE : string.Empty,
                            FK_IDCENTRE = dmd.FK_IDCENTRE,
                            CODE_DEMANDE_TABLE_TRAVAIL = dmd.CODE_DEMANDE_TABLETRAVAIL,
                            CODETDEM = Op.CODE_TDEM,
                            ETAPE_ACTUELLE = null != etapeActuelle ? etapeActuelle.NOM : string.Empty,
                            ETAPE_PRECEDENTE = null != etapePrecedente ? etapePrecedente.NOM : string.Empty,
                            ETAPE_SUIVANTE = null != etapeSuivante ? etapeSuivante.NOM : string.Empty,
                            FK_IDOPERATION = Op.PK_ID,
                            FK_IDSITE = null != c ? c.FK_IDCODESITE : 0,
                            SITE = null != c ? sites.FirstOrDefault(s => s.PK_ID == c.FK_IDCODESITE).LIBELLE : string.Empty,
                            IDLIGNETABLETRAVAIL = dmd.FK_IDLIGNETABLETRAVAIL
                        };
                    }
                    else return null;
                }
                else return null;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void NotificationMailDemande(List<string> lstDestinataire, string NumeroDemande, string TypeDemande, string CodeTypeMail)
        {
            try
            {
                //Galatee.Tools.Utility.SendMail();


                CsParametreSMTP leServerMail = new DB_NOTIFICATION().SelectAllSMTP().FirstOrDefault();
                CsNotificaton leMAil = new DB_NOTIFICATION().SelectNotificationByTypeMail(CodeTypeMail).FirstOrDefault();
                List<string> lstAdresse = new List<string>();
                if (leServerMail != null && leMAil != null)
                {
                    leMAil.SERVEURSMTP = leServerMail.SERVEURSMTP;
                    //leMAil.PASSWORD = Galatee.Tools.Utility.DecryptLicense(leServerMail.PASSWORD);
                    leMAil.PASSWORD = leServerMail.PASSWORD;
                    lstDestinataire.Add("iwebsmail@edm-sa.ml.com");
                    foreach (var item in lstDestinataire)
                    {
                        //string[] valeur = item.Split('@');
                        //lstAdresse.Add(valeur[0].Trim() + "@edm-sa.com.ml");

                    }

                    leMAil.PORT = leServerMail.PORT;
                    leMAil.SSL = leServerMail.SSL;
                    leMAil.LOGIN = leServerMail.LOGIN;
                    leMAil.MESSAGE = string.Format(leMAil.MESSAGE, TypeDemande, NumeroDemande);
                    //Galatee.Tools.Utility.EnvoiMail(leMAil.SERVEURSMTP, leMAil.LOGIN, leMAil.PORT, leMAil.PASSWORD, leMAil.OBJET, leMAil.MESSAGE, leMAil.SSL.Value, lstDestinataire);
                    Galatee.Tools.Utility.EnvoiMail(lstDestinataire, leMAil.MESSAGE);
                }
            }
            catch (Exception ex)
            {
            }
        }


        public void ModifierCircuitTransferAbonnement(CsDtransfert ledemande, SqlCommand cmds)
        {


            cmds.CommandTimeout = 1800;
            cmds.CommandType = CommandType.StoredProcedure;
            cmds.CommandText = "SPX_ACC_MODIFIECIRCUITTRANSFERT";
            cmds.Parameters.Add("@IDCENTREORIGINE", SqlDbType.Int).Value = ledemande.FK_IDCENTREORIGINE;
            cmds.Parameters.Add("@IDCENTREDESTINATION", SqlDbType.Int).Value = ledemande.FK_IDCENTRETRANSFERT;
            cmds.Parameters.Add("@IDDEMANDE", SqlDbType.Int).Value = ledemande.FK_IDDEMANDE;
            DBBase.SetDBNullParametre(cmds.Parameters);
            try
            {
                if (cmds.Connection.State == ConnectionState.Closed)
                    cmds.Connection.Open();
                cmds.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        
    }

}
