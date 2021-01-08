using Galatee.Silverlight.Resources.Parametrage;
using Galatee.Silverlight.ServiceParametrage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
//using Galatee.Silverlight.ServiceExecuterActionWorkflow;
using System.Globalization;
using Galatee.Silverlight.Workflow;
using Galatee.Silverlight.ServiceWorkflow;

namespace Galatee.Silverlight.Workflow
{
    public partial class UcWKFListeAutreDemandeEtape : ChildWindow
    {

        List<DemandeWorkflowInformation> LstLesDemandes;
        List<CsAffectationDemandeUser> lsDemandesAffectes;
        DemandeWorkflowInformation DmdSelectionnee;
        List<CsCopieDmdConditionBranchement> toutesConditionsDeLEtape;
        List<DemandeWorkflowInformation> __demandesCochees;
        Guid FKRWorkflowCentre;
        Guid _RAffEtapeWorkflow;
        int nombreEtapeCircuit = 0;
        int FKEtape;
        Guid _OperationID;
        CsEtape _LEtape;
        string NomOperation = string.Empty;
        string CodeDemande = string.Empty;
        string LeControle = string.Empty;
        List<Galatee.Silverlight.ServiceAccueil.CsRenvoiRejet> _LesRenvoisEtapes;
        bool IsTraitementParLot = false;
        public UcWKFListeAutreDemandeEtape(Guid Operation, int IDEtape, int NbreEtape)
        {
            try
            {
                InitializeComponent();
                FKEtape = IDEtape;
                _OperationID = Operation;
                nombreEtapeCircuit = NbreEtape;
                SessionObject.IsChargerDashbord = false;
                _LEtape = null != SessionObject._ToutesLesEtapesWorkflows ? SessionObject._ToutesLesEtapesWorkflows.FirstOrDefault(e => e.PK_ID == IDEtape):null ;
                ChargerListDesSite(null );
                Translate();
                prgBar.Visibility = System.Windows.Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, "Tableau de bord, Fenetre demande Liste");
            }
        }
        public UcWKFListeAutreDemandeEtape(Guid Operation, List<Guid> lstIdDemande, bool _IstraitementLot, string _NomOperation, int IDEtape)
        {
            try
            {
                InitializeComponent();
                Translate();
                NomOperation = _NomOperation;
                _OperationID = Operation;
                FKEtape = IDEtape;
           
                IsTraitementParLot = _IstraitementLot;
                SessionObject.IsChargerDashbord = false;
                ChargerListDesSite(null );
                _LEtape = null != SessionObject._ToutesLesEtapesWorkflows ? SessionObject._ToutesLesEtapesWorkflows.FirstOrDefault(e => e.PK_ID == IDEtape) : null;
                prgBar.Visibility = System.Windows.Visibility.Collapsed;

                //On cache dabord le bouton
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, "Tableau de bord, Fenetre demande Liste");
            }
        }
        public UcWKFListeAutreDemandeEtape(bool IsConsultation, Guid Operation, List<Guid> lstIdDemande, bool _IstraitementLot, string _NomOperation, int IDEtape)
        {
            try
            {
                InitializeComponent();
                Translate();
                NomOperation = _NomOperation;
                _OperationID = Operation;
                FKEtape = IDEtape;
                IsTraitementParLot = _IstraitementLot;
                SessionObject.IsChargerDashbord = false;
                ChargerListDesSite(lstIdDemande);
                _LEtape = null != SessionObject._ToutesLesEtapesWorkflows ? SessionObject._ToutesLesEtapesWorkflows.FirstOrDefault(e => e.PK_ID == IDEtape) : null;
                if (IsConsultation) EditerButton.Visibility = System.Windows.Visibility.Collapsed;

                if (Operation == new Guid(SessionObject.OperationCampagne))
                    this.dtgrdParametre.Columns[3].Visibility = System.Windows.Visibility.Collapsed;

                prgBar.Visibility = System.Windows.Visibility.Collapsed;

                //On cache dabord le bouton
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, "Tableau de bord, Fenetre demande Liste");
            }
        }
        public UcWKFListeAutreDemandeEtape(Guid Operation, int IDEtape,bool _IstraitementLot,  int NbreEtape)
        {
            try
            {
                InitializeComponent();
                FKEtape = IDEtape;
                _OperationID = Operation;
                nombreEtapeCircuit = NbreEtape;

                _LEtape = null != SessionObject._ToutesLesEtapesWorkflows ? SessionObject._ToutesLesEtapesWorkflows.FirstOrDefault(e => e.PK_ID == IDEtape) : null;

                Translate();
                GetData(null);
                IsTraitementParLot = _IstraitementLot;
                SessionObject.IsChargerDashbord = false;

                //On cache dabord le bouton
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, "Tableau de bord, Fenetre demande Liste");
            }
        }
        public UcWKFListeAutreDemandeEtape(Guid Operation, int IDEtape, int NbreEtape, string _NomOperation)
        {
            try
            {
                InitializeComponent();
                FKEtape = IDEtape;
                _OperationID = Operation;
                NomOperation = _NomOperation;
                prgBar.Visibility = System.Windows.Visibility.Collapsed;
                nombreEtapeCircuit = NbreEtape;
                SessionObject.IsChargerDashbord = false;
               
                _LEtape = null != SessionObject._ToutesLesEtapesWorkflows ? SessionObject._ToutesLesEtapesWorkflows.FirstOrDefault(e => e.PK_ID == IDEtape) : null ;
                ChargerListDesSite(null );
                Translate();
                EditerButton.IsEnabled = false;
                prgBar.Visibility = System.Windows.Visibility.Collapsed;

            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, "Tableau de bord");
            }
        }

        public UcWKFListeAutreDemandeEtape(Guid Operation, int IDEtape, bool _IstraitementLot, int NbreEtape, string _NomOperation)
        {
            try
            {
                InitializeComponent();
                FKEtape = IDEtape;
                _OperationID = Operation;
                NomOperation = _NomOperation;
                prgBar.Visibility = System.Windows.Visibility.Collapsed;
                nombreEtapeCircuit = NbreEtape;
                SessionObject.IsChargerDashbord = false;
                ChargerListDesSite(null);

                _LEtape = null != SessionObject._ToutesLesEtapesWorkflows ? SessionObject._ToutesLesEtapesWorkflows.FirstOrDefault(e => e.PK_ID == IDEtape) : null;
                IsTraitementParLot = _IstraitementLot;
                Translate();
                prgBar.Visibility = System.Windows.Visibility.Collapsed;

            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, "Tableau de bord");
            }
        }
        #region Fonctions

        void Translate()
        {
            CancelButton.Content = Languages.Annuler;
        }
        //void GetData()
        //{
        //    if (Guid.Empty != _OperationID && 0 != FKEtape)
        //    {
        //        LstLesDemandes = new List<DemandeWorkflowInformation>();
        //        ParametrageClient client = new ParametrageClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Parametrage"));
        //        //ON recherche avant tout les demandes qui nous on été affectés, on n'affiche que celle-là,
        //        //car elle doivent être traitées avant les autres
        //        client.CheckForAffectationForEtapeCompleted += (__, ar) =>
        //        {
        //            if (ar.Cancelled || ar.Error != null)
        //            {
        //                string error = ar.Error.Message;
        //                Message.Show(error, Languages.ListeCodePoste);
        //                return;
        //            }
        //            if (ar.Result == null)
        //            {
        //                Message.ShowError(Languages.msgErreurChargementDonnees, Languages.Parametrage);
        //                return;
        //            }
        //            if (ar.Result)
        //            {
        //                //Si ya des affectation on les ajoutes

        //                //--Recherche des affectations
        //                client.GetLesDemandesAffecteForMatriculeUserCompleted += (___, ar_) =>
        //                {
        //                    if (ar_.Cancelled || ar_.Error != null)
        //                    {
        //                        string error = ar_.Error.Message;
        //                        Message.Show(error, Languages.ListeCodePoste);
        //                        return;
        //                    }
        //                    if (ar_.Result == null)
        //                    {
        //                        Message.ShowError(Languages.msgErreurChargementDonnees, Languages.Parametrage);
        //                        return;
        //                    }
        //                    Dictionary<CsAffectationDemandeUser, CsVwJournalDemande> LesDemandesAffectes = ar_.Result;
        //                    foreach (var affectationDmd in LesDemandesAffectes)
        //                    {
        //                        var dmdInfo = new DemandeWorkflowInformation()
        //                        {
        //                            ALERTE = affectationDmd.Value.ALERTE,
        //                            CODE = affectationDmd.Value.CODE,
        //                            CONTROLEETAPE = affectationDmd.Value.CONTROLEETAPE,
        //                            DATECREATION = affectationDmd.Value.DATECREATION,
        //                            DATEDERNIEREMODIFICATION = affectationDmd.Value.DATEDERNIEREMODIFICATION,
        //                            DUREE = affectationDmd.Value.DUREE,
        //                            ETAPEPRECEDENTE = affectationDmd.Value.ETAPEPRECEDENTE,
        //                            FK_IDCENTRE = affectationDmd.Value.FK_IDCENTRE,
        //                            FK_IDETAPE = affectationDmd.Value.FK_IDETAPE,
        //                            FK_IDETAPEACTUELLE = affectationDmd.Value.FK_IDETAPEACTUELLE,
        //                            FK_IDGROUPEVALIDATIOIN = affectationDmd.Value.FK_IDGROUPEVALIDATIOIN,
        //                            FK_IDMENU = affectationDmd.Value.FK_IDMENU,
        //                            FK_IDOPERATION = affectationDmd.Value.FK_IDOPERATION,
        //                            FK_IDSTATUS = affectationDmd.Value.FK_IDSTATUS,
        //                            FK_IDWORKFLOW = affectationDmd.Value.FK_IDWORKFLOW,
        //                            IDCIRCUIT = affectationDmd.Value.IDCIRCUIT,
        //                            IDETAPE = affectationDmd.Value.IDETAPE,
        //                            MATRICULEUSERCREATION = affectationDmd.Value.MATRICULEUSERCREATION,
        //                            ORDRE = affectationDmd.Value.ORDRE,
        //                            NOM = affectationDmd.Value.NOM,
        //                            NOMOPERATION = NomOperation,
        //                            ALLCENTRE = affectationDmd.Value.ALLCENTRE,
        //                            CODECENTRE = affectationDmd.Value.CODECENTRE,
        //                            CODESITE = affectationDmd.Value.CODESITE,
        //                            IDCENTRE = affectationDmd.Value.IDCENTRE,
        //                            IDSITE = affectationDmd.Value.IDSITE,
        //                            LIBELLECENTRE = affectationDmd.Value.LIBELLECENTRE,
        //                            LIBELLESITE = affectationDmd.Value.LIBELLESITE,
        //                            FK_IDLIGNETABLETRAVAIL = affectationDmd.Value.FK_IDLIGNETABLETRAVAIL,
        //                            FK_IDTABLETRAVAIL = affectationDmd.Value.FK_IDTABLETRAVAIL,
        //                            ESTAFFECTE = true,
        //                            UTILISATEURAFFECTE = affectationDmd.Key.MATRICULEUSER,
        //                            CODE_DEMANDE_TABLETRAVAIL = affectationDmd.Value.CODE_DEMANDE_TABLETRAVAIL,
        //                            MODIFICATION = affectationDmd.Value.MODIFICATION,
        //                            FK_IDETAPECIRCUIT = Guid.Empty //Généralement à une étape d'affectation, pas besoin de condition d'action
        //                        };

        //                        if (dmdInfo.DATEDERNIEREMODIFICATION.HasValue && dmdInfo.DUREE.HasValue)
        //                        {
        //                            dmdInfo.DATEFINTRAITEMENT = dmdInfo.DATEDERNIEREMODIFICATION.Value.AddDays(dmdInfo.DUREE.Value);
        //                        }
        //                        dmdInfo.COMBINAISON_FKETAPE_FKOPERATION = "";
        //                        dmdInfo.STATUT = EnumStatutDemande.RecupererLibelleStatutDemande((WorkflowManager.STATUSDEMANDE)affectationDmd.Value.FK_IDSTATUS);
        //                        LstLesDemandes.Add(dmdInfo);
        //                    }

        //                    #region Les autres demandes

        //                    //On va maintenant rechercher toutes les autres demandes, et n'afficher que celle qui
        //                    //ne sont pas affectées         
        //                    client.SelectVwJournalDemandeCompleted += (sender, args) =>
        //                    {
        //                        if (args.Cancelled || args.Error != null)
        //                        {
        //                            string error = args.Error.Message;
        //                            Message.Show(error, Languages.ListeCodePoste);
        //                            return;
        //                        }
        //                        if (args.Result == null)
        //                        {
        //                            Message.ShowError(Languages.msgErreurChargementDonnees, Languages.Parametrage);
        //                            return;
        //                        }

        //                        if (null == LstLesDemandes) LstLesDemandes = new List<DemandeWorkflowInformation>();

        //                        var filtre = args.Result.Where(jrnl => jrnl.FK_IDETAPE == FKEtape && jrnl.FK_IDOPERATION == _OperationID
        //                            && null != jrnl.CODE)
        //                            .ToList();
        //                        foreach (var dmd in filtre)
        //                        {
        //                            if (LstLesDemandes.Where(d => d.CODE_DEMANDE_TABLETRAVAIL == dmd.CODE_DEMANDE_TABLETRAVAIL)
        //                                .FirstOrDefault() == null)
        //                            {
        //                                var dmdInfo = new DemandeWorkflowInformation()
        //                                {
        //                                    ALERTE = dmd.ALERTE,
        //                                    CODE = dmd.CODE,
        //                                    CONTROLEETAPE = dmd.CONTROLEETAPE,
        //                                    DATECREATION = dmd.DATECREATION,
        //                                    DATEDERNIEREMODIFICATION = dmd.DATEDERNIEREMODIFICATION,
        //                                    DUREE = dmd.DUREE,
        //                                    ETAPEPRECEDENTE = dmd.ETAPEPRECEDENTE,
        //                                    FK_IDCENTRE = dmd.FK_IDCENTRE,
        //                                    FK_IDETAPE = dmd.FK_IDETAPE,
        //                                    FK_IDETAPEACTUELLE = dmd.FK_IDETAPEACTUELLE,
        //                                    FK_IDGROUPEVALIDATIOIN = dmd.FK_IDGROUPEVALIDATIOIN,
        //                                    FK_IDMENU = dmd.FK_IDMENU,
        //                                    FK_IDOPERATION = dmd.FK_IDOPERATION,
        //                                    FK_IDSTATUS = dmd.FK_IDSTATUS,
        //                                    FK_IDWORKFLOW = dmd.FK_IDWORKFLOW,
        //                                    IDCIRCUIT = dmd.IDCIRCUIT,
        //                                    IDETAPE = dmd.IDETAPE,
        //                                    MATRICULEUSERCREATION = dmd.MATRICULEUSERCREATION,
        //                                    ORDRE = dmd.ORDRE,
        //                                    NOM = dmd.NOM,
        //                                    NOMOPERATION = NomOperation,
        //                                    ALLCENTRE = dmd.ALLCENTRE,
        //                                    CODECENTRE = dmd.CODECENTRE,
        //                                    CODESITE = dmd.CODESITE,
        //                                    IDCENTRE = dmd.IDCENTRE,
        //                                    IDSITE = dmd.IDSITE,
        //                                    LIBELLECENTRE = dmd.LIBELLECENTRE,
        //                                    LIBELLESITE = dmd.LIBELLESITE,
        //                                    FK_IDLIGNETABLETRAVAIL = dmd.FK_IDLIGNETABLETRAVAIL,
        //                                    FK_IDTABLETRAVAIL = dmd.FK_IDTABLETRAVAIL,
        //                                    ESTAFFECTE = false,
        //                                    UTILISATEURAFFECTE = string.Empty,
        //                                    CODE_DEMANDE_TABLETRAVAIL = dmd.CODE_DEMANDE_TABLETRAVAIL,
        //                                    MODIFICATION = dmd.MODIFICATION,
        //                                    FK_IDETAPECIRCUIT = dmd.FK_IDETAPECIRCUIT
        //                                };

        //                                if (dmdInfo.DATEDERNIEREMODIFICATION.HasValue && dmdInfo.DUREE.HasValue)
        //                                {
        //                                    dmdInfo.DATEFINTRAITEMENT = dmdInfo.DATEDERNIEREMODIFICATION.Value.AddDays(dmdInfo.DUREE.Value);
        //                                }
        //                                dmdInfo.COMBINAISON_FKETAPE_FKOPERATION = "";
        //                                dmdInfo.STATUT = EnumStatutDemande.RecupererLibelleStatutDemande((WorkflowManager.STATUSDEMANDE)dmd.FK_IDSTATUS);
        //                                LstLesDemandes.Add(dmdInfo);
        //                            }
        //                        }

        //                        //On affcihe ou on cache les boutons selon les infos de l'étape
        //                        //AfficherOuCacherLesBoutonsActions();

        //                        //Tout est fini, on change le titre de la fenêtre
        //                        if (LstLesDemandes.Count > 0)
        //                        {
        //                            this.Title = LstLesDemandes.First().NOMOPERATION + " - " + LstLesDemandes.First().NOM + " ("
        //                                + LstLesDemandes.Count + " demande(s))";

        //                            NomOperation = LstLesDemandes.First().NOMOPERATION;
        //                        }

        //                        //Bon normalement si c'est fini je vais chercher les conditions de l'étape,
        //                        #region Gestion Condition Action (Commenté)
        //                        //client.SelectConditionParEtapesCompleted += (l, a) =>
        //                        //{
        //                        //    toutesConditionsDeLEtape = new List<CsCopieDmdConditionBranchement>();
        //                        //    LoadingManager.EndLoading(back);
        //                        //    if (a.Cancelled || a.Error != null)
        //                        //    {
        //                        //        string error = a.Error.Message;
        //                        //        Message.Show(error, Languages.ListeCodePoste);
        //                        //        return;
        //                        //    }
        //                        //    if (a.Result == null)
        //                        //    {
        //                        //        Message.ShowError(Languages.msgErreurChargementDonnees, Languages.Parametrage);
        //                        //        return;
        //                        //    }
        //                        //    toutesConditionsDeLEtape.AddRange(a.Result.Where(c => c.PEUT_TRANSMETTRE_SI_FAUX.HasValue &&
        //                        //        !c.PEUT_TRANSMETTRE_SI_FAUX.Value).ToList());
        //                        //};
        //                        //List<Guid> g = LstLesDemandes.Where(l => l.FK_IDETAPECIRCUIT.HasValue)
        //                        //    .Select(l => l.FK_IDETAPECIRCUIT.Value)
        //                        //    .ToList();
        //                        //client.SelectConditionParEtapesAsync(g);
        //                        #endregion

        //                        dtgrdParametre.ItemsSource = LstLesDemandes;

        //                        //WCO le 15/01/2016
        //                        #region Renvois Rejet
        //                        //On a fini, on cherche les renvoi etape
        //                        client.GetLesRenvoisRejetCompleted += (rsender, rargs) =>
        //                        {
        //                            if (rargs.Cancelled || args.Error != null)
        //                            {
        //                                string error = args.Error.Message;
        //                                Message.Show(error, Languages.ListeCodePoste);
        //                                return;
        //                            }
        //                            if (rargs.Result == null)
        //                            {
        //                                Message.ShowError(Languages.msgErreurChargementDonnees, Languages.Parametrage);
        //                                return;
        //                            }
        //                            _LesRenvoisEtapes = new List<ServiceAccueil.CsRenvoiRejet>();
        //                            foreach (var r in rargs.Result)
        //                            {
        //                                _LesRenvoisEtapes.Add(new ServiceAccueil.CsRenvoiRejet()
        //                                {
        //                                    PK_ID = r.PK_ID,
        //                                    FK_IDETAPE = r.FK_IDETAPE,
        //                                    FK_IDETAPEACTUELLE = r.FK_IDETAPEACTUELLE,
        //                                    FK_IDRAFFECTATION = r.FK_IDRAFFECTATION
        //                                });
        //                            }
        //                            //_LesRenvoisEtapes = rargs.Result;
        //                        };
        //                        client.GetLesRenvoisRejetAsync(FKEtape);
        //                        #endregion

        //                    };
        //                    client.SelectVwJournalDemandeAsync();

        //                    #endregion
        //                };
        //                client.GetLesDemandesAffecteForMatriculeUserAsync(UserConnecte.matricule, FKEtape, _OperationID);
        //            }
        //            //Dans le cas contraitre, on va récupérer les autres demandes
        //            else
        //            {
        //                client.SelectVwJournalDemandeCompleted += (sender, args) =>
        //                {
        //                    //LoadingManager.EndLoading(back);

        //                    if (args.Cancelled || args.Error != null)
        //                    {
        //                        string error = args.Error.Message;
        //                        Message.Show(error, Languages.ListeCodePoste);
        //                        return;
        //                    }
        //                    if (args.Result == null)
        //                    {
        //                        Message.ShowError(Languages.msgErreurChargementDonnees, Languages.Parametrage);
        //                        return;
        //                    }

        //                    LstLesDemandes = new List<DemandeWorkflowInformation>();
        //                    var filtre = args.Result.Where(jrnl => jrnl.FK_IDETAPE == FKEtape && jrnl.FK_IDOPERATION == _OperationID
        //                        && null != jrnl.CODE)
        //                        .ToList();
        //                    foreach (var dmd in filtre)
        //                    {
        //                        var dmdInfo = new DemandeWorkflowInformation()
        //                        {
        //                            ALERTE = dmd.ALERTE,
        //                            CODE = dmd.CODE,
        //                            CONTROLEETAPE = dmd.CONTROLEETAPE,
        //                            DATECREATION = dmd.DATECREATION,
        //                            DATEDERNIEREMODIFICATION = dmd.DATEDERNIEREMODIFICATION,
        //                            DUREE = dmd.DUREE,
        //                            ETAPEPRECEDENTE = dmd.ETAPEPRECEDENTE,
        //                            FK_IDCENTRE = dmd.FK_IDCENTRE,
        //                            FK_IDETAPE = dmd.FK_IDETAPE,
        //                            FK_IDETAPEACTUELLE = dmd.FK_IDETAPEACTUELLE,
        //                            FK_IDGROUPEVALIDATIOIN = dmd.FK_IDGROUPEVALIDATIOIN,
        //                            FK_IDMENU = dmd.FK_IDMENU,
        //                            FK_IDOPERATION = dmd.FK_IDOPERATION,
        //                            FK_IDSTATUS = dmd.FK_IDSTATUS,
        //                            FK_IDWORKFLOW = dmd.FK_IDWORKFLOW,
        //                            IDCIRCUIT = dmd.IDCIRCUIT,
        //                            IDETAPE = dmd.IDETAPE,
        //                            MATRICULEUSERCREATION = dmd.MATRICULEUSERCREATION,
        //                            ORDRE = dmd.ORDRE,
        //                            NOM = dmd.NOM,
        //                            NOMOPERATION = NomOperation,
        //                            ALLCENTRE = dmd.ALLCENTRE,
        //                            CODECENTRE = dmd.CODECENTRE,
        //                            CODESITE = dmd.CODESITE,
        //                            IDCENTRE = dmd.IDCENTRE,
        //                            IDSITE = dmd.IDSITE,
        //                            LIBELLECENTRE = dmd.LIBELLECENTRE,
        //                            LIBELLESITE = dmd.LIBELLESITE,
        //                            FK_IDLIGNETABLETRAVAIL = dmd.FK_IDLIGNETABLETRAVAIL,
        //                            FK_IDTABLETRAVAIL = dmd.FK_IDTABLETRAVAIL,
        //                            ESTAFFECTE = false,
        //                            UTILISATEURAFFECTE = string.Empty,
        //                            CODE_DEMANDE_TABLETRAVAIL = dmd.CODE_DEMANDE_TABLETRAVAIL,
        //                            MODIFICATION = dmd.MODIFICATION,
        //                            FK_IDETAPECIRCUIT = dmd.FK_IDETAPECIRCUIT
        //                        };

        //                        if (dmdInfo.DATEDERNIEREMODIFICATION.HasValue && dmdInfo.DUREE.HasValue)
        //                        {
        //                            dmdInfo.DATEFINTRAITEMENT = dmdInfo.DATEDERNIEREMODIFICATION.Value.AddDays(dmdInfo.DUREE.Value);
        //                        }
        //                        dmdInfo.COMBINAISON_FKETAPE_FKOPERATION = "";
        //                        dmdInfo.STATUT = EnumStatutDemande.RecupererLibelleStatutDemande((WorkflowManager.STATUSDEMANDE)dmd.FK_IDSTATUS);
        //                        LstLesDemandes.Add(dmdInfo);
        //                    }



        //                    //AfficherOuCacherLesBoutonsActions();

        //                    //Avant de faire le binding, on va chercher les demandes affectées à l'étape suivante,
        //                    //et n'étant pas encore traitées
        //                    client.CheckForAffectationFromEtapeCompleted += (aff_sender, aff_args) =>
        //                    {
        //                        if (aff_args.Cancelled || aff_args.Error != null)
        //                        {
        //                            string error = aff_args.Error.Message;
        //                            Message.Show(error, Languages.ListeCodePoste);
        //                            return;
        //                        }
        //                        if (aff_args.Result == null)
        //                        {
        //                            Message.ShowError(Languages.msgErreurChargementDonnees, Languages.Parametrage);
        //                            return;
        //                        }

        //                        if (aff_args.Result)
        //                        {
        //                            //on va chercher les étapes affectées, pour une éventuelle affectation                                     
        //                            client.GetLesDemandesAffecteFromMatriculeUserCompleted += (dsender, dargs) =>
        //                            {
        //                                //LoadingManager.EndLoading(back);
        //                                if (dargs.Cancelled || dargs.Error != null)
        //                                {
        //                                    string error = aff_args.Error.Message;
        //                                    Message.Show(error, Languages.ListeCodePoste);
        //                                    return;
        //                                }
        //                                if (dargs.Result == null)
        //                                {
        //                                    Message.ShowError(Languages.msgErreurChargementDonnees, Languages.Parametrage);
        //                                    return;
        //                                }

        //                                Dictionary<CsAffectationDemandeUser, CsVwJournalDemande> LesDemandesAffectes = dargs.Result;
        //                                foreach (var affectationDmd in LesDemandesAffectes)
        //                                {
        //                                    var _dmdInfo2e = new DemandeWorkflowInformation()
        //                                    {
        //                                        ALERTE = affectationDmd.Value.ALERTE,
        //                                        CODE = affectationDmd.Value.CODE,
        //                                        CONTROLEETAPE = affectationDmd.Value.CONTROLEETAPE,
        //                                        DATECREATION = affectationDmd.Value.DATECREATION,
        //                                        DATEDERNIEREMODIFICATION = affectationDmd.Value.DATEDERNIEREMODIFICATION,
        //                                        DUREE = affectationDmd.Value.DUREE,
        //                                        ETAPEPRECEDENTE = affectationDmd.Value.ETAPEPRECEDENTE,
        //                                        FK_IDCENTRE = affectationDmd.Value.FK_IDCENTRE,
        //                                        FK_IDETAPE = affectationDmd.Value.FK_IDETAPE,
        //                                        FK_IDETAPEACTUELLE = affectationDmd.Value.FK_IDETAPEACTUELLE,
        //                                        FK_IDGROUPEVALIDATIOIN = affectationDmd.Value.FK_IDGROUPEVALIDATIOIN,
        //                                        FK_IDMENU = affectationDmd.Value.FK_IDMENU,
        //                                        FK_IDOPERATION = affectationDmd.Value.FK_IDOPERATION,
        //                                        FK_IDSTATUS = affectationDmd.Value.FK_IDSTATUS,
        //                                        FK_IDWORKFLOW = affectationDmd.Value.FK_IDWORKFLOW,
        //                                        IDCIRCUIT = affectationDmd.Value.IDCIRCUIT,
        //                                        IDETAPE = affectationDmd.Value.IDETAPE,
        //                                        MATRICULEUSERCREATION = affectationDmd.Value.MATRICULEUSERCREATION,
        //                                        ORDRE = affectationDmd.Value.ORDRE,
        //                                        NOM = affectationDmd.Value.NOM,
        //                                        NOMOPERATION = NomOperation,
        //                                        ALLCENTRE = affectationDmd.Value.ALLCENTRE,
        //                                        CODECENTRE = affectationDmd.Value.CODECENTRE,
        //                                        CODESITE = affectationDmd.Value.CODESITE,
        //                                        IDCENTRE = affectationDmd.Value.IDCENTRE,
        //                                        IDSITE = affectationDmd.Value.IDSITE,
        //                                        LIBELLECENTRE = affectationDmd.Value.LIBELLECENTRE,
        //                                        LIBELLESITE = affectationDmd.Value.LIBELLESITE,
        //                                        FK_IDLIGNETABLETRAVAIL = affectationDmd.Value.FK_IDLIGNETABLETRAVAIL,
        //                                        FK_IDTABLETRAVAIL = affectationDmd.Value.FK_IDTABLETRAVAIL,
        //                                        ESTAFFECTE = true,
        //                                        UTILISATEURAFFECTE = affectationDmd.Key.MATRICULEUSER,
        //                                        CODE_DEMANDE_TABLETRAVAIL = affectationDmd.Value.CODE_DEMANDE_TABLETRAVAIL,
        //                                        MODIFICATION = affectationDmd.Value.MODIFICATION,
        //                                        FK_IDETAPECIRCUIT = Guid.Empty //Pas besoin de condition d'action
        //                                    };

        //                                    if (_dmdInfo2e.DATEDERNIEREMODIFICATION.HasValue && _dmdInfo2e.DUREE.HasValue)
        //                                    {
        //                                        _dmdInfo2e.DATEFINTRAITEMENT = _dmdInfo2e.DATEDERNIEREMODIFICATION.Value.AddDays(_dmdInfo2e.DUREE.Value);
        //                                    }
        //                                    _dmdInfo2e.COMBINAISON_FKETAPE_FKOPERATION = "";
        //                                    _dmdInfo2e.STATUT = EnumStatutDemande.RecupererLibelleStatutDemande((WorkflowManager.STATUSDEMANDE)affectationDmd.Value.FK_IDSTATUS);
        //                                    LstLesDemandes.Add(_dmdInfo2e);
        //                                }

        //                                //Après tous les chargements, on fait le binding
        //                                dtgrdParametre.ItemsSource = LstLesDemandes;

        //                                //WCO le 15/01/2016
        //                                #region Renvois Rejet
        //                                //On a fini, on cherche les renvoi etape
        //                                client.GetLesRenvoisRejetCompleted += (rsender, rargs) =>
        //                                {
        //                                    if (rargs.Cancelled || args.Error != null)
        //                                    {
        //                                        string error = args.Error.Message;
        //                                        Message.Show(error, Languages.ListeCodePoste);
        //                                        return;
        //                                    }
        //                                    if (rargs.Result == null)
        //                                    {
        //                                        Message.ShowError(Languages.msgErreurChargementDonnees, Languages.Parametrage);
        //                                        return;
        //                                    }

        //                                    foreach (var r in rargs.Result)
        //                                    {
        //                                        _LesRenvoisEtapes.Add(new ServiceAccueil.CsRenvoiRejet()
        //                                        {
        //                                            PK_ID = r.PK_ID,
        //                                            FK_IDETAPE = r.FK_IDETAPE,
        //                                            FK_IDETAPEACTUELLE = r.FK_IDETAPEACTUELLE,
        //                                            FK_IDRAFFECTATION = r.FK_IDRAFFECTATION
        //                                        });
        //                                    }
        //                                };
        //                                client.GetLesRenvoisRejetAsync(FKEtape);
        //                                #endregion


        //                                //Tout est fini, on change le titre de la fenêtre
        //                                if (LstLesDemandes.Count > 0)
        //                                {
        //                                    this.Title = LstLesDemandes.First().NOMOPERATION + " - " + LstLesDemandes.First().NOM + " ("
        //                                        + LstLesDemandes.Count + " demande(s))";

        //                                    NomOperation = LstLesDemandes.First().NOMOPERATION;
        //                                    //On va cherche maintenant les conditions d'action 

        //                                    #region Gestion Condition Action (Commenté)
        //                                    //client.SelectConditionParEtapesCompleted += (l, a) =>
        //                                    //{
        //                                    //    toutesConditionsDeLEtape = new List<CsCopieDmdConditionBranchement>();

        //                                    //    LoadingManager.EndLoading(back);
        //                                    //    if (a.Cancelled || a.Error != null)
        //                                    //    {
        //                                    //        string error = a.Error.Message;
        //                                    //        Message.Show(error, Languages.ListeCodePoste);
        //                                    //        return;
        //                                    //    }
        //                                    //    if (a.Result == null)
        //                                    //    {
        //                                    //        Message.ShowError(Languages.msgErreurChargementDonnees, Languages.Parametrage);
        //                                    //        return;
        //                                    //    }
        //                                    //    toutesConditionsDeLEtape.AddRange(a.Result.Where(c => c.PEUT_TRANSMETTRE_SI_FAUX.HasValue &&
        //                                    //        !c.PEUT_TRANSMETTRE_SI_FAUX.Value).ToList());
        //                                    //};
        //                                    //List<Guid> g = LstLesDemandes.Where(l => l.FK_IDETAPECIRCUIT.HasValue)
        //                                    //    .Select(l => l.FK_IDETAPECIRCUIT.Value)
        //                                    //    .ToList();
        //                                    //client.SelectConditionParEtapesAsync(g);
        //                                    #endregion
        //                                }
        //                            };
        //                            client.GetLesDemandesAffecteFromMatriculeUserAsync(UserConnecte.matricule, FKEtape, _OperationID);
        //                        }
        //                        else
        //                        {
        //                            //Après tous les chargements, on fait le binding
        //                            dtgrdParametre.ItemsSource = LstLesDemandes;
        //                            //Tout est fini, on change le titre de la fenêtre
        //                            if (LstLesDemandes.Count > 0)
        //                            {
        //                                this.Title = LstLesDemandes.First().NOMOPERATION + " - " + LstLesDemandes.First().NOM + " ("
        //                                    + LstLesDemandes.Count + " demande(s))";

        //                                NomOperation = LstLesDemandes.First().NOMOPERATION;
        //                            }
        //                        }
        //                    };
        //                    client.CheckForAffectationFromEtapeAsync(FKEtape, _OperationID, UserConnecte.matricule);
        //                };
        //                client.SelectVwJournalDemandeAsync();
        //            }
        //        };
        //        client.CheckForAffectationForEtapeAsync(FKEtape, _OperationID, UserConnecte.matricule);
        //    }
        //}
        void GetData(List<int> LesCentreHabilite)
        {
            if (Guid.Empty != _OperationID && 0 != FKEtape)
            {
                LstLesDemandes = new List<DemandeWorkflowInformation>();
                ParametrageClient client = new ParametrageClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Parametrage"));
                {
                    client.RetourneDemandeWkfEtapeCompleted += (sender, args) =>
                    {
                        //LoadingManager.EndLoading(back);

                        if (args.Cancelled || args.Error != null)
                        {
                            string error = args.Error.Message;
                            Message.Show(error, Languages.ListeCodePoste);
                            return;
                        }
                        if (args.Result == null)
                        {
                            Message.ShowError(Languages.msgErreurChargementDonnees, Languages.Parametrage);
                            return;
                        }

                        LstLesDemandes = new List<DemandeWorkflowInformation>();
                        var filtre = args.Result.Where(jrnl => jrnl.FK_IDETAPE == FKEtape && jrnl.FK_IDOPERATION == _OperationID
                            //var filtre = args.Result.Where(jrnl => jrnl.FK_IDETAPEACTUELLE  == FKEtape && jrnl.FK_IDOPERATION == _OperationID
                            && null != jrnl.CODE && LesCentreHabilite.Contains(jrnl.FK_IDCENTRE))
                            .ToList();
                        foreach (var dmd in filtre)
                        {
                            var dmdInfo = new DemandeWorkflowInformation()
                            {
                                ALERTE = dmd.ALERTE,
                                CODE = dmd.CODE,
                                CONTROLEETAPE = dmd.CONTROLEETAPE,
                                DATECREATION = dmd.DATECREATION,
                                DATEDERNIEREMODIFICATION = dmd.DATEDERNIEREMODIFICATION,
                                DUREE = dmd.DUREE,
                                ETAPEPRECEDENTE = dmd.ETAPEPRECEDENTE,
                                FK_IDCENTRE = dmd.FK_IDCENTRE,
                                FK_IDETAPE = dmd.FK_IDETAPE,
                                FK_IDETAPEACTUELLE = dmd.FK_IDETAPEACTUELLE,
                                FK_IDGROUPEVALIDATIOIN = dmd.FK_IDGROUPEVALIDATIOIN,
                                FK_IDMENU = dmd.FK_IDMENU,
                                FK_IDOPERATION = dmd.FK_IDOPERATION,
                                FK_IDSTATUS = dmd.FK_IDSTATUS,
                                FK_IDWORKFLOW = dmd.FK_IDWORKFLOW,
                                IDCIRCUIT = dmd.IDCIRCUIT,
                                IDETAPE = dmd.IDETAPE,
                                MATRICULEUSERCREATION = dmd.MATRICULEUSERCREATION,
                                ORDRE = dmd.ORDRE,
                                NOM = dmd.NOM,
                                NOMABON = dmd.NOMABON,
                                NOMOPERATION = NomOperation,
                                ALLCENTRE = dmd.ALLCENTRE,
                                CODECENTRE = dmd.CODECENTRE,
                                CODESITE = dmd.CODESITE,
                                IDCENTRE = dmd.IDCENTRE,
                                IDSITE = dmd.IDSITE,
                                LIBELLECENTRE = dmd.LIBELLECENTRE,
                                LIBELLESITE = dmd.LIBELLESITE,
                                FK_IDLIGNETABLETRAVAIL = dmd.FK_IDLIGNETABLETRAVAIL,
                                FK_IDTABLETRAVAIL = dmd.FK_IDTABLETRAVAIL,
                                ESTAFFECTE = false,
                                UTILISATEURAFFECTE = string.Empty,
                                CODE_DEMANDE_TABLETRAVAIL = dmd.CODE_DEMANDE_TABLETRAVAIL,
                                MODIFICATION = dmd.MODIFICATION,
                                FK_IDETAPECIRCUIT = dmd.FK_IDETAPECIRCUIT
                            };

                            if (dmdInfo.DATEDERNIEREMODIFICATION.HasValue && dmdInfo.DUREE.HasValue)
                            {
                                dmdInfo.DATEFINTRAITEMENT = dmdInfo.DATEDERNIEREMODIFICATION.Value.AddDays(dmdInfo.DUREE.Value);
                            }
                            dmdInfo.COMBINAISON_FKETAPE_FKOPERATION = "";
                            dmdInfo.STATUT = EnumStatutDemande.RecupererLibelleStatutDemande((WorkflowManager.STATUSDEMANDE)dmd.FK_IDSTATUS);
                            LstLesDemandes.Add(dmdInfo);
                        }
                        //Après tous les chargements, on fait le binding
                        //dtgrdParametre.ItemsSource = LstLesDemandes;
                        dtgrdParametre.ItemsSource = LstLesDemandes.OrderByDescending(t => t.DATEDERNIEREMODIFICATION).ToList();

                        //Tout est fini, on change le titre de la fenêtre
                        if (LstLesDemandes.Count > 0)
                        {
                            this.Title = LstLesDemandes.First().NOMOPERATION + " - " + LstLesDemandes.First().NOM + " ("
                                + LstLesDemandes.Count + " demande(s))";

                            NomOperation = LstLesDemandes.First().NOMOPERATION;
                            //On va cherche maintenant les conditions d'action 
                        }
                    };
                    client.RetourneDemandeWkfEtapeAsync(LesCentreHabilite, FKEtape, UserConnecte.matricule);
                }
            }
        }

        void GetDataWkf(List<int> lstidcentre, List<Guid> lstIdDemande, int FKEtape, string Matricule)
        {
            try
            {
                if (lstIdDemande != null && lstIdDemande.Count != 0)
                {
                    prgBar.Visibility = System.Windows.Visibility.Visible;
                    LstLesDemandes = new List<DemandeWorkflowInformation>();
                    ParametrageClient client = new ParametrageClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Parametrage"));
                    client.SelectAllDemandeUserCompleted += (sender, args) =>
                    {
                        if (args.Cancelled || args.Error != null)
                        {
                            string error = args.Error.Message;
                            Message.Show(error, Languages.ListeCodePoste);
                            return;
                        }
                        if (args.Result == null)
                        {
                            Message.ShowError(Languages.msgErreurChargementDonnees, Languages.Parametrage);
                            return;
                        }

                        if (null == LstLesDemandes) LstLesDemandes = new List<DemandeWorkflowInformation>();
                        foreach (var dmd in args.Result)
                        {
                            if (LstLesDemandes.Where(d => d.CODE_DEMANDE_TABLETRAVAIL == dmd.CODE_DEMANDE_TABLETRAVAIL)
                                .FirstOrDefault() == null)
                            {
                                var dmdInfo = new DemandeWorkflowInformation()
                                {
                                    ALERTE = dmd.ALERTE,
                                    CODE = dmd.CODE,
                                    CONTROLEETAPE = dmd.CONTROLEETAPE,
                                    DATECREATION = dmd.DATECREATION,
                                    DATEDERNIEREMODIFICATION = dmd.DATEDERNIEREMODIFICATION,
                                    DUREE = dmd.DUREE,
                                    ETAPEPRECEDENTE = dmd.ETAPEPRECEDENTE,
                                    FK_IDCENTRE = dmd.FK_IDCENTRE,
                                    FK_IDETAPE = dmd.FK_IDETAPE,
                                    FK_IDETAPEACTUELLE = dmd.FK_IDETAPEACTUELLE,
                                    FK_IDGROUPEVALIDATIOIN = dmd.FK_IDGROUPEVALIDATIOIN,
                                    FK_IDMENU = dmd.FK_IDMENU,
                                    FK_IDOPERATION = dmd.FK_IDOPERATION,
                                    FK_IDSTATUS = dmd.FK_IDSTATUS,
                                    FK_IDWORKFLOW = dmd.FK_IDWORKFLOW,
                                    IDCIRCUIT = dmd.IDCIRCUIT,
                                    IDETAPE = dmd.IDETAPE,
                                    MATRICULEUSERCREATION = dmd.MATRICULEUSERCREATION,
                                    ORDRE = dmd.ORDRE,
                                    NOM = dmd.NOM,
                                    NOMABON = dmd.NOMABON,
                                    NOMOPERATION = NomOperation,
                                    ALLCENTRE = dmd.ALLCENTRE,
                                    CODECENTRE = dmd.CODECENTRE,
                                    CODESITE = dmd.CODESITE,
                                    IDCENTRE = dmd.IDCENTRE,
                                    IDSITE = dmd.IDSITE,
                                    LIBELLECENTRE = dmd.LIBELLECENTRE,
                                    LIBELLESITE = dmd.LIBELLESITE,
                                    FK_IDLIGNETABLETRAVAIL = dmd.FK_IDLIGNETABLETRAVAIL,
                                    FK_IDTABLETRAVAIL = dmd.FK_IDTABLETRAVAIL,
                                    ESTAFFECTE = false,
                                    UTILISATEURAFFECTE = string.Empty,
                                    CODE_DEMANDE_TABLETRAVAIL = dmd.CODE_DEMANDE_TABLETRAVAIL,
                                    MODIFICATION = dmd.MODIFICATION,
                                    FK_IDETAPECIRCUIT = dmd.FK_IDETAPECIRCUIT
                                };

                                if (dmdInfo.DATEDERNIEREMODIFICATION.HasValue && dmdInfo.DUREE.HasValue)
                                {
                                    dmdInfo.DATEFINTRAITEMENT = dmdInfo.DATEDERNIEREMODIFICATION.Value.AddDays(dmdInfo.DUREE.Value);
                                }
                                dmdInfo.COMBINAISON_FKETAPE_FKOPERATION = "";
                                dmdInfo.STATUT = EnumStatutDemande.RecupererLibelleStatutDemande((WorkflowManager.STATUSDEMANDE)dmd.FK_IDSTATUS);
                                LstLesDemandes.Add(dmdInfo);
                            }
                        }
                        //Tout est fini, on change le titre de la fenêtre
                        if (LstLesDemandes.Count > 0)
                        {
                            this.Title = LstLesDemandes.First().NOMOPERATION + " - " + LstLesDemandes.First().NOM + " ("
                                + LstLesDemandes.Count + " demande(s))";

                            NomOperation = LstLesDemandes.First().NOMOPERATION;
                        }
                        dtgrdParametre.ItemsSource = LstLesDemandes.OrderByDescending(t => t.DATEDERNIEREMODIFICATION).ToList();
                        prgBar.Visibility = System.Windows.Visibility.Collapsed;
                    };
                    client.SelectAllDemandeUserAsync(lstidcentre, lstIdDemande, FKEtape, Matricule);
                }
            }
            catch (Exception ex)
            {
                prgBar.Visibility = System.Windows.Visibility.Collapsed;
                throw ex;
            }
        }
        #endregion


        List<ServiceAccueil.CsCentre> lesCentre = new List<ServiceAccueil.CsCentre>();
        List<ServiceAccueil.CsSite> lesSite = new List<ServiceAccueil.CsSite>();
        private List<ServiceAccueil.CsCentre> _listeDesCentreExistant = null;

        void ChargerListDesSite(List<Guid> lstIdDemande)
        {
            try
            {

                if (SessionObject.LstCentre != null && SessionObject.LstCentre.Count != 0)
                {
                    SessionObject.ModuleEnCours = "Accueil";
                    lesCentre = Shared.ClasseMEthodeGenerique.RetourCentreByPerimetre(SessionObject.LstCentre, UserConnecte.listeProfilUser);
                    lesSite = Shared.ClasseMEthodeGenerique.RetourneSiteByCentre(lesCentre);
                    _listeDesCentreExistant = lesCentre;
                    if (lesCentre != null && lesCentre.Count != 0)
                    {
                        if (lstIdDemande != null && lstIdDemande.Count != 0)
                            GetDataWkf(lesCentre.Select(t => t.PK_ID).ToList(), lstIdDemande, FKEtape, UserConnecte.matricule);
                        else
                            GetData(lesCentre.Select(t => t.PK_ID).ToList());
                    }
                    RemplirCentrePerimetre(lesCentre, lesSite);
                    return;
                }
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.ListeDesDonneesDesSiteAsync(false);
                service.ListeDesDonneesDesSiteCompleted += (s, args) =>
                {
                    try
                    {
                        if (args != null && args.Cancelled)
                            return;
                        SessionObject.LstCentre = args.Result;
                        if (SessionObject.LstCentre.Count != 0)
                        {
                            SessionObject.ModuleEnCours = "Accueil";
                            lesCentre = Shared.ClasseMEthodeGenerique.RetourCentreByPerimetre(SessionObject.LstCentre, UserConnecte.listeProfilUser);
                            lesSite = Shared.ClasseMEthodeGenerique.RetourneSiteByCentre(lesCentre);
                            _listeDesCentreExistant = lesCentre;
                            if (lesCentre != null && lesCentre.Count != 0)
                            {
                                if (lstIdDemande != null && lstIdDemande.Count != 0)
                                    GetDataWkf(lesCentre.Select(t => t.PK_ID).ToList(), lstIdDemande, FKEtape, UserConnecte.matricule);
                                else
                                    GetData(lesCentre.Select(t => t.PK_ID).ToList());
                            }
                            RemplirCentrePerimetre(lesCentre, lesSite);
                        }
                        else
                        {
                            Message.ShowInformation("Aucun site trouvé en base.", "Erreur");
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        Message.ShowError(ex, "Erreur");
                    }
                };
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
            }
        }
        private void RemplirCentreDuSite(int pIdSite, int pIdcentre)
        {
            try
            {
                Cbo_Centre.Items.Clear();
                if (_listeDesCentreExistant != null &&
                    _listeDesCentreExistant.Count != 0)
                {
                    List<ServiceAccueil.CsCentre> lesCentreDuPerimetreAction = _listeDesCentreExistant.Where(p => p.FK_IDCODESITE == pIdSite).ToList();
                    if (lesCentreDuPerimetreAction != null)
                    {
                        foreach (var item in lesCentreDuPerimetreAction)
                            Cbo_Centre.Items.Add(item);
                        if (lesCentreDuPerimetreAction.Count == 1)
                            Cbo_Centre.SelectedItem = lesCentreDuPerimetreAction.First();
                    }
                    //Cbo_Centre.ItemsSource = lesCentreDuPerimetreAction;
                    Cbo_Centre.SelectedValuePath = "PK_ID";
                    Cbo_Centre.DisplayMemberPath = "LIBELLE";

                    if (pIdcentre != 0)
                        this.Cbo_Centre.SelectedItem = _listeDesCentreExistant.First(t => t.PK_ID == pIdcentre);
                    if (_listeDesCentreExistant.Count == 1)
                        this.Cbo_Centre.SelectedItem = _listeDesCentreExistant.First();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void RemplirCentrePerimetre(List<ServiceAccueil.CsCentre> lstCentre, List<ServiceAccueil.CsSite> lstSite)
        {
            try
            {
                Cbo_Centre.Items.Clear();
                if (_listeDesCentreExistant != null &&
                    _listeDesCentreExistant.Count != 0)
                {
                    if (lstCentre != null)
                    {
                        this.Cbo_Centre.Items.Add(new ServiceAccueil.CsCentre());
                        foreach (var item in lstCentre)
                        {
                            Cbo_Centre.Items.Add(item);
                        }
                    }
                    Cbo_Centre.SelectedValuePath = "PK_ID";
                    Cbo_Centre.DisplayMemberPath = "LIBELLE";

                    if (lstSite != null)
                    {
                        this.Cbo_Site.Items.Add(new ServiceAccueil.CsSite());
                        foreach (var item in lstSite)
                        {
                            Cbo_Site.Items.Add(item);
                        }
                    }
                    Cbo_Site.SelectedValuePath = "PK_ID";
                    Cbo_Site.DisplayMemberPath = "LIBELLE";

                    if (lstSite != null && lstSite.Count == 1)
                        Cbo_Site.SelectedItem = lstSite.First();

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void Cbo_Site_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (this.Cbo_Site.SelectedItem != null)
                {
                    var csSite = Cbo_Site.SelectedItem as ServiceAccueil.CsSite;
                    if (csSite != null)
                    {
                        this.txtSite.Text = csSite.CODE ?? string.Empty;
                        RemplirCentreDuSite(csSite.PK_ID, 0);
                    }
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, "Site");
            }
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (__demandesCochees == null) __demandesCochees = new List<DemandeWorkflowInformation>();
            else if (__demandesCochees != null && __demandesCochees.Count != 0)
                __demandesCochees.Clear();
            foreach (DemandeWorkflowInformation item in dtgrdParametre.ItemsSource as List<DemandeWorkflowInformation>)
            {
                if (checkSelectedItem((CheckBox)this.dtgrdParametre.Columns[0].GetCellContent(item) as CheckBox))
                    __demandesCochees.Add(item);
            }
            //seulement le demande qui ont été coché
            if (null != __demandesCochees && __demandesCochees.Count >= 1)
            {

                List<string> codesDemandes = new List<string>();
                foreach (var dmd in __demandesCochees)
                {
                    codesDemandes.Add(dmd.CODE);
                }

                //Transmission par groupe ou non
                TransmettreDemande(codesDemandes);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            SessionObject.IsChargerDashbord = true ;
            this.DialogResult = false;
        }

        void TransmettreDemande(DemandeWorkflowInformation dmdInfo)
        {
            prgBar.Visibility = System.Windows.Visibility.Visible;
            WorkflowClient client = new WorkflowClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Workflow"));
            client.Endpoint.Binding.OpenTimeout = new TimeSpan(0, 1, 0);
            client.Endpoint.Binding.CloseTimeout = new TimeSpan(5, 0, 0);
            client.Endpoint.Binding.SendTimeout = new TimeSpan(5, 0, 0);
            client.ExecuterActionSurDemandeCompleted += (sender, args) =>
            {
                prgBar.Visibility = System.Windows.Visibility.Collapsed;
                if (args.Cancelled || args.Error != null)
                {
                    string error = args.Error.Message;
                    Message.Show(error, Languages.ListeCodePoste);
                    return;
                }
                if (args.Result == null)
                {
                    Message.ShowError(Languages.msgErreurChargementDonnees, Languages.Parametrage);
                    return;
                }
                if (args.Result.StartsWith("ERR"))
                {
                    Message.ShowError(args.Result, Languages.Parametrage);
                }
                else
                {
                    Message.ShowInformation(args.Result, Languages.Parametrage);
                    if (lesCentre != null && lesCentre.Count != 0)
                        GetData(lesCentre.Select(t => t.PK_ID).ToList());
                }
            };
            client.ExecuterActionSurDemandeAsync(dmdInfo.CODE, SessionObject.Enumere.TRANSMETTRE, UserConnecte.matricule, string.Empty);
        }
        void TransmettreDemande(List<string> Codes)
        {
            prgBar.Visibility = System.Windows.Visibility.Visible;

            WorkflowClient client = new WorkflowClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Workflow"));
            client.Endpoint.Binding.OpenTimeout = new TimeSpan(0, 1, 0);
            client.Endpoint.Binding.CloseTimeout = new TimeSpan(5, 0, 0);
            client.Endpoint.Binding.SendTimeout = new TimeSpan(5, 0, 0);
            client.ExecuterActionSurPlusieursDemandesCompleted += (sender, args) =>
            {
                prgBar.Visibility = System.Windows.Visibility.Collapsed;
                if (args.Cancelled || args.Error != null)
                {
                    string error = args.Error.Message;
                    Message.Show(error, Languages.ListeCodePoste);
                    return;
                }
                if (args.Result == null)
                {
                    Message.ShowError(Languages.msgErreurChargementDonnees, Languages.Parametrage);
                    return;
                }
                if (args.Result.StartsWith("ERR"))
                {
                    Message.ShowError(args.Result, Languages.Parametrage);
                }
                else
                {
                    Message.ShowInformation(args.Result, Languages.Parametrage);
                    if (lesCentre != null && lesCentre.Count != 0)
                        GetData(lesCentre.Select(t => t.PK_ID).ToList());
                }
            };
            client.ExecuterActionSurPlusieursDemandesAsync(Codes, SessionObject.Enumere.TRANSMETTRE, UserConnecte.matricule, string.Empty);
        }

        #region Gestion DataGrid

        private void dtgrdParametre_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //ActiverOuDesactiverBouton();
        }
     
        #endregion

        private void RejeterButton_Click(object sender, RoutedEventArgs e)
        {
            //Rejet de la demande            
            //On appel la fenetre de rejet, pour entrer le motif de la demande
            //if (dtgrdParametre.SelectedItem != null
            //    && dtgrdParametre.SelectedItems.Count == 1)
            if (__demandesCochees != null && __demandesCochees.Count == 1)
            {
                DemandeWorkflowInformation dmdInfo = dtgrdParametre.SelectedItem as DemandeWorkflowInformation;
                if (null != dmdInfo)
                {

                    #region Commenté, remplacé par l'appel de la fenetre de renseignement du motif du rejet

                    //prgBar.Visibility = System.Windows.Visibility.Visible;
                    //LblChargement.Visibility = System.Windows.Visibility.Visible;
                    //OKButton.IsEnabled = false;
                    //RejeterButton.IsEnabled = false;

                    //ExecuterActionWorkflow param = new ExecuterActionWorkflow()
                    //{
                    //    CodeDemande = dmdInfo.CODE,
                    //    CodeAction = CODEACTION.REJETER,
                    //    MatriculeUser = UserConnecte.matricule
                    //};
                    //ExecuterActionWorkflowRequest request = new ExecuterActionWorkflowRequest()
                    //{
                    //    ExecuterActionWorkflow = param
                    //};
                    //ExecuterActionWorkflowClient client = new ExecuterActionWorkflowClient();
                    //client.Endpoint.Binding.OpenTimeout = new TimeSpan(0, 1, 0);
                    //client.Endpoint.Binding.CloseTimeout = new TimeSpan(5, 0, 0);
                    //client.Endpoint.Binding.SendTimeout = new TimeSpan(5, 0, 0);

                    //client.ExecuterActionWorkflowCompleted += (ssender, args) =>
                    //{
                    //    prgBar.Visibility = System.Windows.Visibility.Collapsed;
                    //    LblChargement.Visibility = System.Windows.Visibility.Collapsed;
                    //    OKButton.IsEnabled = true;
                    //    RejeterButton.IsEnabled = true;

                    //    if (args.Cancelled || args.Error != null)
                    //    {
                    //        string error = args.Error.Message;
                    //        Message.Show(error, Languages.ListeCodePoste);
                    //        return;
                    //    }
                    //    if (args.Result == null)
                    //    {
                    //        Message.ShowError(Languages.msgErreurChargementDonnees, Languages.Parametrage);
                    //        return;
                    //    }
                    //    if (args.Result.StartsWith("ERR"))
                    //    {
                    //        Message.ShowError(args.Result, Languages.Parametrage);
                    //    }
                    //    else
                    //    {
                    //        Message.ShowInformation(args.Result, Languages.Parametrage);
                    //        GetData();
                    //    }
                    //};
                    //client.ExecuterActionWorkflowAsync(param);

                    #endregion

                    if (null == _LesRenvoisEtapes || _LesRenvoisEtapes.Count == 0)
                    {
                        UcWKFMotifRejet ucMotif = new UcWKFMotifRejet(dmdInfo);
                        ucMotif.Closed += (_sender, args) =>
                        {
                            //On relance juste le getdata
                            if (lesCentre != null && lesCentre.Count != 0)
                                GetData(lesCentre.Select(t => t.PK_ID).ToList());
                        };
                        ucMotif.Show();
                    }
                    else
                    {
                        DemandeWorkflowInformation __LaDemande = __demandesCochees.First();
                        //Et on appelle la fenetre
                        UcWKFSelectEtape ucform = new UcWKFSelectEtape(__LaDemande, _LesRenvoisEtapes);
                        ucform.Closed += ucform_Closed;
                        DmdSelectionnee = __LaDemande;
                        ucform.Show();
                    }
                }
            }
        }

        //private void ConsulterButton_Click(object sender, RoutedEventArgs e)
        //{

        //    if (__demandesCochees != null && __demandesCochees.Count == 1)
        //    {
        //        //DemandeWorkflowInformation dmdInfo = dtgrdParametre.SelectedItem as DemandeWorkflowInformation;
        //        DemandeWorkflowInformation dmdInfo = __demandesCochees.First();
        //        if (null != dmdInfo)
        //        {
        //            //Si le control existe et que c'est consultation seulement on l'appel
        //            if (string.Empty != LeControle)
        //            {
        //                Type typeTheControl = Type.GetType(LeControle);
        //                var constructor = typeTheControl.GetConstructors();

        //                //On cherche le constructeur avec un seul parametre en Int32
        //                //var leBonC = constructor.Where(c => c.GetParameters().Count() == 1 && c.GetParameters().First()
        //                //    .Position == 0 && (c.GetParameters().First().ParameterType == typeof(int) || c.GetParameters().First().ParameterType == typeof(Guid)
        //                //    || c.GetParameters().First().ParameterType == typeof(string)))
        //                //    .FirstOrDefault();
        //                var leBonC = constructor.Where(c => c.GetParameters().Count() == 2 && c.GetParameters().First()
        //                    .Position == 0 && (c.GetParameters().First().ParameterType == typeof(int) || c.GetParameters().First().ParameterType == typeof(Guid)
        //                    || c.GetParameters().First().ParameterType == typeof(string)) &&
        //                    c.GetParameters()[1].ParameterType == typeof(SessionObject.ExecMode))
        //                    .FirstOrDefault();
        //                if (null != leBonC)
        //                {
        //                    try
        //                    {
        //                        Type leParam = leBonC.GetParameters()[0].ParameterType;
        //                        CultureInfo provider = new CultureInfo("fr-FR");
        //                        var laVraieValeur = Convert.ChangeType(dmdInfo.FK_IDLIGNETABLETRAVAIL, leParam, provider);
        //                        ChildWindow cw = Activator.CreateInstance(typeTheControl, new object[] { laVraieValeur, SessionObject.ExecMode.Consultation }) as ChildWindow;
        //                        if (null != cw)
        //                        {
        //                            cw.Show();
        //                        }
        //                    }
        //                    catch
        //                    {
        //                        //Control par défaut
        //                        //UcWKFDetailDemande detailForm = new UcWKFDetailDemande(dmdInfo, NomOperation);
        //                        Galatee.Silverlight.Devis.UcConsultationDevis detailForm = new Galatee.Silverlight.Devis.UcConsultationDevis(int.Parse(dmdInfo.FK_IDLIGNETABLETRAVAIL));
        //                        detailForm.Show();
        //                    }
        //                }
        //                else
        //                {
        //                    //Control par défaut
        //                    Galatee.Silverlight.Devis.UcConsultationDevis detailForm = new Galatee.Silverlight.Devis.UcConsultationDevis(int.Parse(dmdInfo.FK_IDLIGNETABLETRAVAIL));
        //                    detailForm.Show();
        //                }
        //            }
        //            else
        //            {
        //                //Control par défaut
        //                Galatee.Silverlight.Devis.UcConsultationDevis detailForm = new Galatee.Silverlight.Devis.UcConsultationDevis(int.Parse(dmdInfo.FK_IDLIGNETABLETRAVAIL));
        //                detailForm.Show();
        //            }
        //        }
        //    }
        //}
        private void ConsulterButton_Click(object sender, RoutedEventArgs e)
        {
            //ON récupère la demande sélectionnée, 
            try
            {
                if (dtgrdParametre.SelectedItem != null )
                {
                    DemandeWorkflowInformation SelectedObject = (DemandeWorkflowInformation)dtgrdParametre.SelectedItem;
                    Galatee.Silverlight.Devis.UcConsultationDevis detailForm = new Galatee.Silverlight.Devis.UcConsultationDevis(int.Parse(SelectedObject.FK_IDLIGNETABLETRAVAIL));
                    detailForm.Show();
                }
            }
            catch (Exception ex)
            {
                Message.ShowError("Impossible de traiter la demande. Détails de l'erreur : " + ex.Message, "Traiter une demande");
            }
          
        }

        private void EditerButton_Click(object sender, RoutedEventArgs e)
        {
            //ON récupère la demande sélectionnée, 
            try
            {
                    if (null != LstLesDemandes.First().CONTROLEETAPE && string.Empty != LstLesDemandes.First().CONTROLEETAPE)
                    {
                        //On voit si le formulaire est en modification ou en consultation seulement                                        
                        LeControle = LstLesDemandes.First().CONTROLEETAPE;
                        if (__demandesCochees == null) __demandesCochees = new List<DemandeWorkflowInformation>();
                        else if (__demandesCochees != null && __demandesCochees.Count != 0)
                            __demandesCochees.Clear();
                        foreach (DemandeWorkflowInformation item in dtgrdParametre.ItemsSource as List<DemandeWorkflowInformation>)
                        {
                            if (checkSelectedItem((CheckBox)this.dtgrdParametre.Columns[0].GetCellContent(item) as CheckBox))
                                __demandesCochees.Add(item);
                        }
                    }
                    Type typeTheControl = Type.GetType(LeControle);
                    var constructor = typeTheControl.GetConstructors();
                    //Si c'est traiter par demande unique
                    if (!_LEtape.IS_TRAITEMENT_LOT && __demandesCochees.Count == 1)
                    {
                        DemandeWorkflowInformation __LaDemande = __demandesCochees.First();
                        //On cherche le constructeur avec un seul parametre en Int32
                        var leBonC = constructor.Where(c => c.GetParameters().Count() == 1 && c.GetParameters().First()
                            .Position == 0 && (c.GetParameters().First().ParameterType == typeof(int) || c.GetParameters().First().ParameterType == typeof(Guid)
                            || c.GetParameters().First().ParameterType == typeof(string)))
                            .FirstOrDefault();
                        if (null != leBonC)
                        {
                            Type leParam = leBonC.GetParameters()[0].ParameterType;
                            CultureInfo provider = new CultureInfo("fr-FR");
                            try
                            {
                                if (leParam == typeof(int) || leParam == typeof(Guid))
                                {
                                    var laVraieValeur = Convert.ChangeType(__LaDemande.FK_IDLIGNETABLETRAVAIL, leParam, provider);
                                    ChildWindow cw = Activator.CreateInstance(typeTheControl, laVraieValeur) as ChildWindow;
                                    if (null != cw)
                                    {
                                        SessionObject.IsChargerDashbord = false;
                                        cw.Show();
                                        cw.Closed += cw_Closed;
                                    }
                                }
                                else if (leParam == typeof(string))
                                {
                                    var laVraieValeur = Convert.ChangeType(__LaDemande.CODE, leParam, provider);
                                    ChildWindow cw = Activator.CreateInstance(typeTheControl, laVraieValeur) as ChildWindow;
                                    if (null != cw)
                                    {
                                        SessionObject.IsChargerDashbord = false;
                                        cw.Show();
                                        cw.Closed += cw_Closed;
                                    }
                                }
                            }
                            catch
                            {
                                if (leParam == typeof(string))
                                {
                                    var laVraieValeur = Convert.ChangeType(__LaDemande.CODE, leParam, provider);
                                    ChildWindow cw = Activator.CreateInstance(typeTheControl, laVraieValeur) as ChildWindow;
                                    if (null != cw)
                                    {
                                        SessionObject.IsChargerDashbord = false;
                                        cw.Show();
                                        cw.Closed += cw_Closed;
                                    }
                                }
                            }
                        }
                        else
                        {
                            Message.ShowError("Le formulaire associé à cette étape n'a pas de constructeur approprié", "Erreur");
                        }
                    }
                    else if (!_LEtape.IS_TRAITEMENT_LOT && __demandesCochees.Count < 1)
                    {
                        Message.ShowError("Impossible de traiter plusieurs demande à cette étape", "Erreur traiter une demande");
                    }
                    else if (_LEtape.IS_TRAITEMENT_LOT)
                    {
                        //On cherche le constructeur avec deux parametres en List<Int32> et Int32
                        var leBonC = constructor.Where(c => c.GetParameters().Count() == 2 && c.GetParameters().First().Position == 0 && 
                            (c.GetParameters().First().ParameterType == typeof(List<int>) || 
                            c.GetParameters().First().ParameterType == typeof(List<Guid>) || 
                            c.GetParameters().First().ParameterType == typeof(List<int>)) && 
                            c.GetParameters()[1].Position == 1 && c.GetParameters()[1].ParameterType == typeof(int))
                            .FirstOrDefault();
                        if (null != leBonC)
                        {
                            Type leParam = leBonC.GetParameters()[0].ParameterType;
                            CultureInfo provider = new CultureInfo("fr-FR");
                            List<int> valeurs = __demandesCochees.Select(d => int.Parse(d.FK_IDLIGNETABLETRAVAIL))
                                .ToList<int>();
                            //var laVraieValeur = Convert.ChangeType(, leParam, provider);
                            ChildWindow cw = Activator.CreateInstance(typeTheControl, valeurs, FKEtape) as ChildWindow;
                            if (null != cw)
                            {
                                SessionObject.IsChargerDashbord = false;
                                cw.Closed += cw_Closed;
                                cw.Show();
                            }
                        }
                        else Message.ShowError("Impossible de traiter les demandes par lot", "Traiter la demande par lot");
                    }
            }
            catch (Exception ex)
            {
                Message.ShowError("Impossible de traiter la demande. Détails de l'erreur : " + ex.Message, "Traiter une demande");
            }
        }
        private void dtgrdParametre_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            var dmdRow = e.Row.DataContext as DemandeWorkflowInformation;
            if (dmdRow != null)
            {
                if (dmdRow.DATEFINTRAITEMENT.HasValue)
                {
                    int delai = ((DateTime)dmdRow.DATEFINTRAITEMENT.Value - DateTime.Today)
                        .Days;
                    if (delai <= 0)
                    {
                        SolidColorBrush SolidColorBrush = new SolidColorBrush(Colors.Red);
                        e.Row.Foreground = SolidColorBrush;
                    }
                }
                if (dmdRow.ESTAFFECTE)
                {
                    if (dmdRow.UTILISATEURAFFECTE == UserConnecte.matricule)
                    {
                        SolidColorBrush SolidColorBrush = new SolidColorBrush(Colors.Green);
                        e.Row.Foreground = SolidColorBrush;
                        e.Row.FontWeight = FontWeights.Bold;
                    }
                    else if (dmdRow.UTILISATEURAFFECTE != UserConnecte.matricule)
                    {
                        SolidColorBrush SolidColorBrush = new SolidColorBrush(Colors.LightGray);
                        e.Row.Foreground = SolidColorBrush;
                        e.Row.FontWeight = FontWeights.Bold;
                        e.Row.IsEnabled = false;
                    }
                }

                if (dmdRow.FK_IDSTATUS.HasValue && dmdRow.FK_IDSTATUS.Value == (int)WorkflowManager.STATUSDEMANDE.Rejetee)
                {
                    SolidColorBrush SolidColorBrush = new SolidColorBrush(Colors.Blue);
                    e.Row.Foreground = SolidColorBrush;
                    e.Row.FontWeight = FontWeights.Bold;
                }
            }
        }

        private void AffecterButton_Click(object sender, RoutedEventArgs e)
        {
            //ON récupère la demande sélectionnée,             
            //if (dtgrdParametre.SelectedItem != null && dtgrdParametre.SelectedItems.Count == 1)
            if (__demandesCochees != null && __demandesCochees.Count == 1)
            {
                //DemandeWorkflowInformation __LaDemande = dtgrdParametre.SelectedItem as DemandeWorkflowInformation;
                DemandeWorkflowInformation __LaDemande = __demandesCochees.First();
                //Et on appelle la fenetre
                UserAgentPickerAffectation ucform = new UserAgentPickerAffectation(__LaDemande.CODE, FKEtape,
                    __LaDemande.FK_IDCENTRE, _OperationID, __LaDemande.FK_IDWORKFLOW);
                ucform.Closed += ucform_Closed;
                DmdSelectionnee = __LaDemande;
                ucform.Show();
            }
        }

        void ucform_Closed(object sender, EventArgs e)
        {
            //On recharge les demandes
            if (lesCentre != null && lesCentre.Count != 0)
                GetData(lesCentre.Select(t => t.PK_ID).ToList());
        }

        private void AffecterDemandeButton_Click(object sender, RoutedEventArgs e)
        {
            //if (dtgrdParametre.SelectedItem != null && dtgrdParametre.SelectedItems.Count == 1)
            if (__demandesCochees != null && __demandesCochees.Count == 1)
            {
                //DemandeWorkflowInformation __LaDemande = dtgrdParametre.SelectedItem as DemandeWorkflowInformation;
                DemandeWorkflowInformation __LaDemande = __demandesCochees.First();
                //Et on appelle la fenetre
                UcWKFSelectEtape ucform = new UcWKFSelectEtape(__LaDemande);
                ucform.Closed += ucform_Closed;
                DmdSelectionnee = __LaDemande;
                ucform.Show();
            }
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            //if (null == __demandesCochees) __demandesCochees = new List<DemandeWorkflowInformation>();
            //if (null != dtgrdParametre.SelectedItem && null != ((DemandeWorkflowInformation)dtgrdParametre.SelectedItem))
            //    __demandesCochees.Add((DemandeWorkflowInformation)dtgrdParametre.SelectedItem);

            //ActiverOuDesactiverBoutonParCheckBox();


        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            ////Retrait des éléments décochés
            //if (null == __demandesCochees) __demandesCochees = new List<DemandeWorkflowInformation>();
            //if (null != dtgrdParametre.SelectedItem && null != ((DemandeWorkflowInformation)dtgrdParametre.SelectedItem))
            //{
            //    var toDelete = __demandesCochees.Where(d => d.CODE == ((DemandeWorkflowInformation)dtgrdParametre.SelectedItem).CODE)
            //        .FirstOrDefault();
            //    if (null != toDelete) __demandesCochees.Remove(toDelete);
            //}
            //ActiverOuDesactiverBoutonParCheckBox();
        }

        private void EditerParLotButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //ON récupère la demande sélectionnée, 
                if (__demandesCochees != null
                    && __demandesCochees.Count >= 1 && string.Empty != LeControle)
                {
                    Type typeTheControl = Type.GetType(LeControle);
                    var constructor = typeTheControl.GetConstructors();

                    //On cherche le constructeur avec deux parametres en List<Int32> et Int32
                    var leBonC = constructor.Where(c => c.GetParameters().Count() == 2 && c.GetParameters().First()
                        .Position == 0 && (c.GetParameters().First().ParameterType == typeof(List<int>) || c.GetParameters().First().ParameterType == typeof(List<Guid>)
                        || c.GetParameters().First().ParameterType == typeof(List<int>)) && c.GetParameters()[1].Position == 1 && c.GetParameters()[1].ParameterType == typeof(int))
                        .FirstOrDefault();
                    if (null != leBonC)
                    {
                        Type leParam = leBonC.GetParameters()[0].ParameterType;
                        CultureInfo provider = new CultureInfo("fr-FR");
                        List<int> valeurs = __demandesCochees.Select(d => int.Parse(d.FK_IDLIGNETABLETRAVAIL))
                            .ToList<int>();
                        //var laVraieValeur = Convert.ChangeType(, leParam, provider);
                        ChildWindow cw = Activator.CreateInstance(typeTheControl, valeurs, FKEtape) as ChildWindow;
                        if (null != cw)
                        {
                            //cw.Closed += cw_Closed;
                            cw.Show();
                        }
                    }
                    else Message.ShowError("Impossible de traiter les demandes par lot", "Traiter la demande par lot");
                }
            }
            catch (Exception ex)
            {
                Message.ShowError("Impossible de traiter les demandes par lots. Details de :" + ex.Message, "Traiter les demandes par lot");
            }
        }

        void cw_Closed(object sender, EventArgs e)
        {
            //On recharge les demandes
            if (lesCentre != null && lesCentre.Count != 0)
                GetData(lesCentre.Select(t => t.PK_ID).ToList());
        }
        DateTime lastClick = DateTime.Now;
        private void dgMyDataGrid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            DataGrid dg = (sender as DataGrid);
            var allObjects = dg.ItemsSource as List<DemandeWorkflowInformation>;
            if (dg.SelectedItem != null)
            {
                DemandeWorkflowInformation SelectedObject = (DemandeWorkflowInformation)dg.SelectedItem;
                //bool IsCocher = checkSelectedItem((CheckBox)this.dtgrdParametre.Columns[0].GetCellContent(SelectedObject) as CheckBox);
                //if (IsCocher)
                //{
                //    DechekerSelectedItem((CheckBox)this.dtgrdParametre.Columns[0].GetCellContent(SelectedObject) as CheckBox);
                //    SelectedObject.IsSelect = false;
                //    EditerButton.IsEnabled = false;
                //}
                //else
                //{
                    checkerSelectedItem((CheckBox)this.dtgrdParametre.Columns[0].GetCellContent(SelectedObject) as CheckBox);
                    SelectedObject.IsSelect = true ;
                    if (!IsTraitementParLot)
                    {
                        foreach (DemandeWorkflowInformation item in (dg.ItemsSource as List<DemandeWorkflowInformation>).Where(t => t.CODE != SelectedObject.CODE).ToList())
                        {
                            if (checkSelectedItem((CheckBox)this.dtgrdParametre.Columns[0].GetCellContent(item) as CheckBox))
                            {
                                DechekerSelectedItem((CheckBox)this.dtgrdParametre.Columns[0].GetCellContent(item) as CheckBox);
                                SelectedObject.IsSelect = false ;
                            }
                        }
                    }
                    if ((DateTime.Now - lastClick).Ticks < 2500000)
                        EditerButton_Click(null, null);
                    lastClick = DateTime.Now;
                //}
            
            }
        }
        bool checkSelectedItem(CheckBox check)
        {
            if (check != null)
            {
                CheckBox chk = check;
                return chk.IsChecked.Value;
            }
            else return false;
        }

        void checkerSelectedItem(CheckBox check)
        {
            try
            {
                CheckBox chk = check;
                chk.IsChecked = true;
                EditerButton.IsEnabled = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        void DechekerSelectedItem(CheckBox check)
        {
            try
            {
                CheckBox chk = check;
                chk.IsChecked = false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void ChildWindow_Closing_1(object sender, System.ComponentModel.CancelEventArgs e)
        {
            SessionObject.IsChargerDashbord = true ;
        }

        private void btn_RechercheClient_Click(object sender, RoutedEventArgs e)
        {
            if (LstLesDemandes != null && LstLesDemandes.Count != 0)
            {
                if (this.Cbo_Site.SelectedItem != null && this.Cbo_Centre.SelectedItem != null )
                {
                    List<DemandeWorkflowInformation> lesDemandes = LstLesDemandes.Where(t=>t.FK_IDCENTRE == ((ServiceAccueil.CsCentre)this.Cbo_Centre.SelectedItem).PK_ID ).OrderBy(t=>t.CODE_DEMANDE_TABLETRAVAIL).ToList();
                    this.dtgrdParametre.ItemsSource = null;
                    this.dtgrdParametre.ItemsSource = lesDemandes;
                }
                if (this.Cbo_Site.SelectedItem != null && this.Cbo_Centre.SelectedItem == null)
                {
                    List<ServiceAccueil.CsCentre> lesCentreDuSite = _listeDesCentreExistant.Where(p => p.FK_IDCODESITE == ((ServiceAccueil.CsSite )this.Cbo_Site.SelectedItem).PK_ID).ToList();
                    List<int> lstCentreSiteDist = new List<int>();
                    foreach (var item in lesCentreDuSite)
                        lstCentreSiteDist.Add(item.PK_ID);

                    List<DemandeWorkflowInformation> lesDemandes = LstLesDemandes.Where(t => lstCentreSiteDist.Contains(t.FK_IDCENTRE)).ToList();
                    this.dtgrdParametre.ItemsSource = null;
                    this.dtgrdParametre.ItemsSource = lesDemandes;
                }
                if (!string.IsNullOrEmpty(this.Txt_NumDevis.Text))
                {
                    List<DemandeWorkflowInformation> lesDemandes = LstLesDemandes.Where(t => t.CODE_DEMANDE_TABLETRAVAIL == this.Txt_NumDevis.Text).ToList();
                    this.dtgrdParametre.ItemsSource = null;
                    this.dtgrdParametre.ItemsSource = lesDemandes;
                }
                if ((this.Cbo_Site.SelectedItem == null || (this.Cbo_Site.SelectedItem != null && string.IsNullOrEmpty(((ServiceAccueil.CsSite )Cbo_Site.SelectedItem).CODE))) &&
                    ((this.Cbo_Centre.SelectedItem == null ||(this.Cbo_Centre.SelectedItem != null && string.IsNullOrEmpty(((ServiceAccueil.CsCentre  )Cbo_Centre.SelectedItem).CODE)))) &&
                    (string.IsNullOrEmpty(this.Txt_NumDevis.Text)))
                {
                    List<DemandeWorkflowInformation> lesDemandes = LstLesDemandes;
                    this.dtgrdParametre.ItemsSource = null;
                    this.dtgrdParametre.ItemsSource = lesDemandes;
                } 
            }
        }

        private void Cbo_Centre_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (this.Cbo_Centre.SelectedItem != null)
                {
                    ServiceAccueil.CsCentre centre = Cbo_Centre.SelectedItem as ServiceAccueil.CsCentre;
                    if (centre != null)
                    {
                        this.txtCentre.Text = centre.CODE ?? string.Empty;
                        this.txtCentre.Tag = centre.PK_ID;
                    }
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, "Demande");
            }
        }
    }
}

