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
using System.Collections.ObjectModel;

namespace Galatee.Silverlight.Workflow
{
    public partial class UcWKFListeDemandeEtape : ChildWindow
    {

        //List<DemandeWorkflowInformation> LstLesDemandes;
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
        public UcWKFListeDemandeEtape(Guid Operation, int IDEtape, int NbreEtape)
        {
            try
            {
                InitializeComponent();
                FKEtape = IDEtape;
                _OperationID = Operation;
                nombreEtapeCircuit = NbreEtape;
                SessionObject.IsChargerDashbord = false;
                _LEtape = null != SessionObject._ToutesLesEtapesWorkflows ? SessionObject._ToutesLesEtapesWorkflows.FirstOrDefault(e => e.PK_ID == IDEtape):null ;
                ChargerListDesSite();
                Translate();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, "Tableau de bord, Fenetre demande Liste");
            }
        }
        public UcWKFListeDemandeEtape(Guid Operation, List<Guid> lstIdDemande, bool _IstraitementLot, string _NomOperation, int IDEtape)
        {
            try
            {
                InitializeComponent();
                Translate();
                NomOperation = _NomOperation;
                _OperationID = Operation;
                FKEtape = IDEtape;
                ChargerEtapesWorkflow();
                
                IsTraitementParLot = _IstraitementLot;
                SessionObject.IsChargerDashbord = false;
                ChargerListDesSite();
                _LEtape = null != SessionObject._ToutesLesEtapesWorkflows ? SessionObject._ToutesLesEtapesWorkflows.FirstOrDefault(e => e.PK_ID == IDEtape) : null;

                this.Cbo_Commune.Visibility = System.Windows.Visibility.Collapsed;
                this.Cbo_quartier.Visibility = System.Windows.Visibility.Collapsed;
                //On cache dabord le bouton
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, "Tableau de bord, Fenetre demande Liste");
            }
        }
        bool IsConsultationSeul = false;
        public UcWKFListeDemandeEtape(bool IsConsultation, Guid Operation, bool _IstraitementLot, string _NomOperation, int IDEtape)
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
                ChargerListDesSite();
                IsConsultationSeul = IsConsultation;
                _LEtape = null != SessionObject._ToutesLesEtapesWorkflows ? SessionObject._ToutesLesEtapesWorkflows.FirstOrDefault(e => e.PK_ID == IDEtape) : null;
                if (IsConsultation) EditerButton.Visibility = System.Windows.Visibility.Collapsed;
                ChargerEtapesWorkflow();
                prgBar.Visibility = System.Windows.Visibility.Collapsed;

                //On cache dabord le bouton
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, "Tableau de bord, Fenetre demande Liste");
            }
        }

        public UcWKFListeDemandeEtape(Guid Operation, int IDEtape,bool _IstraitementLot,  int NbreEtape)
        {
            try
            {
                InitializeComponent();
                FKEtape = IDEtape;
                _OperationID = Operation;
                nombreEtapeCircuit = NbreEtape;
                ChargerEtapesWorkflow();

                _LEtape = null != SessionObject._ToutesLesEtapesWorkflows ? SessionObject._ToutesLesEtapesWorkflows.FirstOrDefault(e => e.PK_ID == IDEtape) : null;

                Translate();
                IsTraitementParLot = _IstraitementLot;
                SessionObject.IsChargerDashbord = false;
                ChargerListDesSite();
                //On cache dabord le bouton
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, "Tableau de bord, Fenetre demande Liste");
            }
        }
        public UcWKFListeDemandeEtape(Guid Operation, int IDEtape, int NbreEtape, string _NomOperation)
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
                ChargerEtapesWorkflow();
               
                _LEtape = null != SessionObject._ToutesLesEtapesWorkflows ? SessionObject._ToutesLesEtapesWorkflows.FirstOrDefault(e => e.PK_ID == IDEtape) : null ;
                ChargerListDesSite();
                Translate();
                EditerButton.IsEnabled = false;
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, "Tableau de bord");
            }
        }

        public UcWKFListeDemandeEtape(Guid Operation, int IDEtape, bool _IstraitementLot, int NbreEtape, string _NomOperation)
        {
            try
            {
                InitializeComponent();
                FKEtape = IDEtape;
                _OperationID = Operation;
                NomOperation = _NomOperation;
                prgBar.Visibility = System.Windows.Visibility.Collapsed;
                nombreEtapeCircuit = NbreEtape;
                ChargerEtapesWorkflow();
                SessionObject.IsChargerDashbord = false;
                ChargerListDesSite();

                _LEtape = null != SessionObject._ToutesLesEtapesWorkflows ? SessionObject._ToutesLesEtapesWorkflows.FirstOrDefault(e => e.PK_ID == IDEtape) : null;
                IsTraitementParLot = _IstraitementLot;
                Translate();

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
        void RechercheDemande(List<int> LesCentreHabilite,string NumDemande,bool IsToutAfficher)
        {
            if (Guid.Empty != _OperationID && 0 != FKEtape)
            {
                prgBar.Visibility = System.Windows.Visibility.Visible;
                LstLesDemandes = new List<DemandeWorkflowInformation>();
                ParametrageClient client = new ParametrageClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Parametrage"));
                {
                    client.RetourneDemandeWkfClientCompleted += (sender, args) =>
                    {
                        //LoadingManager.EndLoading(back);
                        prgBar.Visibility = System.Windows.Visibility.Collapsed ;
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
                                LIBELLEPRODUIT = dmd.LIBELLEPRODUIT,
                                FK_IDLIGNETABLETRAVAIL = dmd.FK_IDLIGNETABLETRAVAIL,
                                FK_IDTABLETRAVAIL = dmd.FK_IDTABLETRAVAIL,
                                ESTAFFECTE = false,
                                UTILISATEURAFFECTE = string.Empty,
                                CODE_DEMANDE_TABLETRAVAIL = dmd.CODE_DEMANDE_TABLETRAVAIL,
                                MODIFICATION = dmd.MODIFICATION,
                                FK_IDETAPECIRCUIT = dmd.FK_IDETAPECIRCUIT,
                                LIBELLECOMMUNE = dmd.LIBELLECOMMUNE,
                                LIBELLEQUARTIER = dmd.LIBELLEQUARTIER,
                                QUARTIER = dmd.QUARTIER,
                                COMMUNE = dmd.COMMUNE
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
                        dtgrdParametre.ItemsSource = LstLesDemandes;
                        dtgrdParametre.ItemsSource = LstLesDemandes.OrderBy(t => t.DATEDERNIEREMODIFICATION).ToList();

                        //Tout est fini, on change le titre de la fenêtre
                        if (LstLesDemandes.Count > 0)
                        {
                            this.Title = LstLesDemandes.First().NOMOPERATION + " - " + LstLesDemandes.First().NOM + " ("
                                + LstLesDemandes.Count + " demande(s))";

                            NomOperation = LstLesDemandes.First().NOMOPERATION;
                            //On va cherche maintenant les conditions d'action 
                        }

                        var lstCommune = LstLesDemandes.Select(u => new { u.COMMUNE, u.LIBELLECOMMUNE }).Distinct();
                        var lstQuartier = LstLesDemandes.Select(u => new { u.QUARTIER, u.LIBELLEQUARTIER }).Distinct();
                        List<CsCommune> Commune = new List<CsCommune>();
                        Commune.Add(new CsCommune
                        {
                            CODE = "A001",
                            LIBELLE = "TOUS"
                        });
                        foreach (var item in lstCommune)
                        {
                            Commune.Add(new CsCommune
                            {
                                CODE = item.COMMUNE,
                                LIBELLE = item.LIBELLECOMMUNE
                            });
                        }
                        List<CsQuartier> Quartier = new List<CsQuartier>();
                        Quartier.Add(new CsQuartier
                        {
                            CODE = "A001",
                            LIBELLE = "TOUS"
                        });
                        foreach (var item in lstQuartier)
                        {
                            Quartier.Add(new CsQuartier
                            {
                                CODE = item.QUARTIER,
                                LIBELLE = item.LIBELLEQUARTIER
                            });
                        }

                        this.Cbo_quartier.ItemsSource = null;
                        this.Cbo_quartier.DisplayMemberPath = "LIBELLE";
                        this.Cbo_quartier.ItemsSource = Quartier;

                        this.Cbo_Commune.ItemsSource = null;
                        this.Cbo_Commune.DisplayMemberPath = "LIBELLE";
                        this.Cbo_Commune.ItemsSource = Commune;

                    };
                    client.RetourneDemandeWkfClientAsync(LesCentreHabilite, FKEtape, UserConnecte.matricule, NumDemande,IsToutAfficher);
                }
            }
        }
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
                                LIBELLEPRODUIT  = dmd.LIBELLEPRODUIT ,
                                FK_IDLIGNETABLETRAVAIL = dmd.FK_IDLIGNETABLETRAVAIL,
                                FK_IDTABLETRAVAIL = dmd.FK_IDTABLETRAVAIL,
                                ESTAFFECTE = false,
                                UTILISATEURAFFECTE = string.Empty,
                                CODE_DEMANDE_TABLETRAVAIL = dmd.CODE_DEMANDE_TABLETRAVAIL,
                                MODIFICATION = dmd.MODIFICATION,
                                FK_IDETAPECIRCUIT = dmd.FK_IDETAPECIRCUIT,
                                LIBELLECOMMUNE =dmd.LIBELLECOMMUNE ,
                                LIBELLEQUARTIER =dmd.LIBELLEQUARTIER ,
                                QUARTIER =dmd.QUARTIER ,
                                COMMUNE = dmd.COMMUNE 
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
                        dtgrdParametre.ItemsSource = LstLesDemandes;
                        dtgrdParametre.ItemsSource = LstLesDemandes.OrderBy(t => t.DATEDERNIEREMODIFICATION).ToList();

                        //Tout est fini, on change le titre de la fenêtre
                        if (LstLesDemandes.Count > 0)
                        {
                            this.Title = LstLesDemandes.First().NOMOPERATION + " - " + LstLesDemandes.First().NOM + " ("
                                + LstLesDemandes.Count + " demande(s))";

                            NomOperation = LstLesDemandes.First().NOMOPERATION;
                            //On va cherche maintenant les conditions d'action 
                        }

                        var  lstCommune = LstLesDemandes.Select(u =>new {u.COMMUNE , u.LIBELLECOMMUNE}).Distinct();
                        var lstQuartier = LstLesDemandes.Select(u =>new {u.QUARTIER , u.LIBELLEQUARTIER}).Distinct();
                        List<CsCommune> Commune = new List<CsCommune>();
                        Commune.Add(new CsCommune {
                         CODE ="A001",
                         LIBELLE ="TOUS" });
                        foreach (var item in lstCommune)
                        {
                            Commune.Add(new CsCommune
                            {
                                CODE = item.COMMUNE ,
                                LIBELLE = item.LIBELLECOMMUNE  
                            });
                        }
                        List<CsQuartier> Quartier = new List<CsQuartier>();
                        Quartier.Add(new CsQuartier 
                        {
                            CODE = "A001",
                            LIBELLE = "TOUS"
                        });
                        foreach (var item in lstQuartier)
                        {
                            Quartier.Add(new CsQuartier
                            {
                                CODE = item.QUARTIER ,
                                LIBELLE = item.LIBELLEQUARTIER 
                            });
                        } 

                        this.Cbo_quartier.ItemsSource = null;
                        this.Cbo_quartier.DisplayMemberPath = "LIBELLE";
                        this.Cbo_quartier.ItemsSource = Quartier;

                        this.Cbo_Commune.ItemsSource = null;
                        this.Cbo_Commune.DisplayMemberPath  = "LIBELLE";
                        this.Cbo_Commune.ItemsSource = Commune;
                        
                    };
                    client.RetourneDemandeWkfEtapeAsync(LesCentreHabilite, FKEtape, UserConnecte.matricule);
                }
            }
        }
        void GetDataWkf(List<int> lstidcentre, List<Guid> lstIdDemande,int FKEtape ,string Matricule)
        {
            try
            {
                //if (lstIdDemande != null && lstIdDemande.Count != 0)
                //{
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
                                    LIBELLEPRODUIT = dmd.LIBELLEPRODUIT,
                                    FK_IDLIGNETABLETRAVAIL = dmd.FK_IDLIGNETABLETRAVAIL,
                                    FK_IDTABLETRAVAIL = dmd.FK_IDTABLETRAVAIL,
                                    ESTAFFECTE = false,
                                    UTILISATEURAFFECTE = string.Empty,
                                    CODE_DEMANDE_TABLETRAVAIL = dmd.CODE_DEMANDE_TABLETRAVAIL,
                                    MODIFICATION = dmd.MODIFICATION,
                                    FK_IDETAPECIRCUIT = dmd.FK_IDETAPECIRCUIT,
                                    LIBELLECOMMUNE = dmd.LIBELLECOMMUNE,
                                    LIBELLEQUARTIER = dmd.LIBELLEQUARTIER ,
                                    QUARTIER = dmd.QUARTIER,
                                    COMMUNE = dmd.COMMUNE 

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

                        var lstCommune = LstLesDemandes.Select(u => new { u.COMMUNE, u.LIBELLECOMMUNE }).Distinct();
                        var lstQuartier = LstLesDemandes.Select(u => new { u.QUARTIER, u.LIBELLEQUARTIER }).Distinct();
                        List<CsCommune> Commune = new List<CsCommune>();
                        Commune.Add(new CsCommune
                        {
                            CODE = "A001",
                            LIBELLE = "TOUS"
                        });
                        foreach (var item in lstCommune)
                        {
                            Commune.Add(new CsCommune
                            {
                                CODE = item.COMMUNE,
                                LIBELLE = item.LIBELLECOMMUNE
                            });
                        }
                        List<CsQuartier> Quartier = new List<CsQuartier>();
                        Quartier.Add(new CsQuartier
                        {
                            CODE = "A001",
                            LIBELLE = "TOUS"
                        });
                        foreach (var item in lstQuartier)
                        {
                            Quartier.Add(new CsQuartier
                            {
                                CODE = item.QUARTIER,
                                LIBELLE = item.LIBELLEQUARTIER
                            });
                        }

                        this.Cbo_quartier.ItemsSource = null;
                        this.Cbo_quartier.DisplayMemberPath = "LIBELLE";
                        this.Cbo_quartier.ItemsSource = Quartier;

                        this.Cbo_Commune.ItemsSource = null;
                        this.Cbo_Commune.DisplayMemberPath = "LIBELLE";
                        this.Cbo_Commune.ItemsSource = Commune;

                        dtgrdParametre.ItemsSource = LstLesDemandes.OrderBy (t => t.DATEDERNIEREMODIFICATION).ToList();
                        prgBar.Visibility = System.Windows.Visibility.Collapsed ;
                    };
                    client.SelectAllDemandeUserAsync(lstidcentre,lstIdDemande, FKEtape, Matricule);
                //}
            }
            catch (Exception ex)
            {
                prgBar.Visibility = System.Windows.Visibility.Collapsed;
                throw ex;
            }
        }
        void GetDataWkfEtape(List<int> lstidcentre, int FKEtape, string Matricule)
        {
            try
            {
                //if (lstIdDemande != null && lstIdDemande.Count != 0)
                //{
                prgBar.Visibility = System.Windows.Visibility.Visible;
                LstLesDemandes = new List<DemandeWorkflowInformation>();
                ParametrageClient client = new ParametrageClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Parametrage"));
                client.SelectAllDemandeUserEtapeCompleted += (sender, args) =>
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
                                LIBELLEPRODUIT = dmd.LIBELLEPRODUIT,
                                FK_IDLIGNETABLETRAVAIL = dmd.FK_IDLIGNETABLETRAVAIL,
                                FK_IDTABLETRAVAIL = dmd.FK_IDTABLETRAVAIL,
                                ESTAFFECTE = false,
                                UTILISATEURAFFECTE = string.Empty,
                                CODE_DEMANDE_TABLETRAVAIL = dmd.CODE_DEMANDE_TABLETRAVAIL,
                                MODIFICATION = dmd.MODIFICATION,
                                FK_IDETAPECIRCUIT = dmd.FK_IDETAPECIRCUIT,
                                LIBELLECOMMUNE = dmd.LIBELLECOMMUNE,
                                LIBELLEQUARTIER = dmd.LIBELLEQUARTIER,
                                QUARTIER = dmd.QUARTIER,
                                COMMUNE = dmd.COMMUNE

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

                    var lstCommune = LstLesDemandes.Select(u => new { u.COMMUNE, u.LIBELLECOMMUNE }).Distinct();
                    var lstQuartier = LstLesDemandes.Select(u => new { u.QUARTIER, u.LIBELLEQUARTIER }).Distinct();
                    List<CsCommune> Commune = new List<CsCommune>();
                    Commune.Add(new CsCommune
                    {
                        CODE = "A001",
                        LIBELLE = "TOUS"
                    });
                    foreach (var item in lstCommune)
                    {
                        Commune.Add(new CsCommune
                        {
                            CODE = item.COMMUNE,
                            LIBELLE = item.LIBELLECOMMUNE
                        });
                    }
                    List<CsQuartier> Quartier = new List<CsQuartier>();
                    Quartier.Add(new CsQuartier
                    {
                        CODE = "A001",
                        LIBELLE = "TOUS"
                    });
                    foreach (var item in lstQuartier)
                    {
                        Quartier.Add(new CsQuartier
                        {
                            CODE = item.QUARTIER,
                            LIBELLE = item.LIBELLEQUARTIER
                        });
                    }

                    this.Cbo_quartier.ItemsSource = null;
                    this.Cbo_quartier.DisplayMemberPath = "LIBELLE";
                    this.Cbo_quartier.ItemsSource = Quartier;

                    this.Cbo_Commune.ItemsSource = null;
                    this.Cbo_Commune.DisplayMemberPath = "LIBELLE";
                    this.Cbo_Commune.ItemsSource = Commune;

                    dtgrdParametre.ItemsSource = LstLesDemandes.OrderBy(t => t.DATEDERNIEREMODIFICATION).ToList();
                    prgBar.Visibility = System.Windows.Visibility.Collapsed;
                };
                client.SelectAllDemandeUserEtapeAsync(lstidcentre, FKEtape, Matricule);
                //}
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

        void ChargerListDesSite()
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
                            GetDataWkfEtape(lesCentre.Select(t => t.PK_ID).ToList(),FKEtape, UserConnecte.matricule);
                    
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
                                GetDataWkfEtape(lesCentre.Select(t => t.PK_ID).ToList(), FKEtape, UserConnecte.matricule);
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
        private void ChargerEtapesWorkflow()
        {
            if (SessionObject._ToutesLesEtapesWorkflows != null && SessionObject._ToutesLesEtapesWorkflows.Count != 0) return;
            Galatee.Silverlight.ServiceParametrage.ParametrageClient service = new Galatee.Silverlight.ServiceParametrage.ParametrageClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Parametrage"));
            service.SelectAllEtapesCompleted += (s, args) =>
            {
                if (args != null && args.Cancelled)
                    return;
                SessionObject._ToutesLesEtapesWorkflows = args.Result;
             
            };
            service.SelectAllEtapesAsync();
            service.CloseAsync();
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
                        this.txtCentre.Text = string.Empty;
                        this.Cbo_Centre.SelectedItem = null;
                        
                        RemplirCentreDuSite(csSite.PK_ID, 0);
                    }
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, "Site");
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            SessionObject.IsChargerDashbord = true ;
            this.DialogResult = false;
        }

        #region Gestion DataGrid

        private void dtgrdParametre_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //ActiverOuDesactiverBouton();
        }
     
        #endregion

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
                this.EditerButton.IsEnabled = false;
                this.ConsulterButton.IsEnabled = false;
                this.CancelButton.IsEnabled = false;

                if (null != LstLesDemandes.First().CONTROLEETAPE && string.Empty != LstLesDemandes.First().CONTROLEETAPE)
                {
                    //On voit si le formulaire est en modification ou en consultation seulement                                        
                    LeControle = LstLesDemandes.First().CONTROLEETAPE;
                    if (__demandesCochees == null) __demandesCochees = new List<DemandeWorkflowInformation>();
                    else if (__demandesCochees != null && __demandesCochees.Count != 0)
                        __demandesCochees.Clear();
                    if (this.dtgrdParametre.ItemsSource != null)
                    {
                        if (__demandesCochees.Count != 0) __demandesCochees.Clear();
                        List<DemandeWorkflowInformation> lesDemandeSelect = ((List<DemandeWorkflowInformation>)this.dtgrdParametre.ItemsSource).Where(t => t.IsSelect).ToList();
                        __demandesCochees.AddRange(lesDemandeSelect);
                    }
                    if ((LeControle == "Galatee.Silverlight.Devis.UcLiasonCompteur" ||
                        LeControle == "Galatee.Silverlight.Devis.UcProgrammation") && __demandesCochees.Count == 0)
                        __demandesCochees.AddRange(dtgrdParametre.ItemsSource as List<DemandeWorkflowInformation>);
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
                                    this.IsEnabled = false;
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
                        //Message.ShowError("Le formulaire associé à cette étape n'a pas de constructeur approprié", "Erreur");
                    }
                }
                else if (!_LEtape.IS_TRAITEMENT_LOT && __demandesCochees.Count < 1)
                {
                    //Message.ShowError("Impossible de traiter plusieurs demande à cette étape", "Erreur traiter une demande");
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
                //Message.ShowError("Impossible de traiter la demande. Détails de l'erreur : " + ex.Message, "Traiter une demande");
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
                    return;
                }
               else  if (dmdRow.ESTAFFECTE)
                {
                    if (dmdRow.UTILISATEURAFFECTE == UserConnecte.matricule)
                    {
                        SolidColorBrush SolidColorBrush = new SolidColorBrush(Colors.Green);
                        e.Row.Foreground = SolidColorBrush;
                        e.Row.FontWeight = FontWeights.Bold;
                        return;

                    }
                    else if (dmdRow.UTILISATEURAFFECTE != UserConnecte.matricule)
                    {
                        SolidColorBrush SolidColorBrush = new SolidColorBrush(Colors.LightGray);
                        e.Row.Foreground = SolidColorBrush;
                        e.Row.FontWeight = FontWeights.Bold;
                        e.Row.IsEnabled = false;
                        return;

                    }
                   
                }
               else  if (dmdRow.FK_IDSTATUS.HasValue && dmdRow.FK_IDSTATUS.Value == (int)WorkflowManager.STATUSDEMANDE.Rejetee)
                {
                    SolidColorBrush SolidColorBrush = new SolidColorBrush(Colors.Blue);
                    e.Row.Foreground = SolidColorBrush;
                    e.Row.FontWeight = FontWeights.Bold;
                }
                else
                {
                    SolidColorBrush SolidColorBrush = new SolidColorBrush(Colors.Black );
                    e.Row.Foreground = SolidColorBrush;
                    e.Row.FontWeight = FontWeights.Normal;

                }
              
            }
        }


        void ucform_Closed(object sender, EventArgs e)
        {
            //On recharge les demandes
            if (lesCentre != null && lesCentre.Count != 0)
                GetData(lesCentre.Select(t => t.PK_ID).ToList());
        }
        private void EditerParLotButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //ON récupère la demande sélectionnée, 
                if (this.dtgrdParametre.ItemsSource != null)
                {
                    if (__demandesCochees.Count != 0) __demandesCochees.Clear();
                    List<DemandeWorkflowInformation> lesDemandeSelect = ((List<DemandeWorkflowInformation>)this.dtgrdParametre.ItemsSource).Where(t => t.IsSelect).ToList();
                    __demandesCochees.AddRange(lesDemandeSelect);
                }
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
            this.IsEnabled = true;
            this.EditerButton.IsEnabled = true ;
            this.ConsulterButton.IsEnabled = true;
            this.CancelButton.IsEnabled = true;
            List<ServiceAccueil.CsCentre> lesCentre = Shared.ClasseMEthodeGenerique.RetourCentreByPerimetreTotal(SessionObject.LstCentre , UserConnecte.listeProfilUser);
             if (lesCentre != null && lesCentre.Count  !=0)
                GetDataWkfEtape(lesCentre.Select(t => t.PK_ID).ToList(), FKEtape, UserConnecte.matricule);
            //GetData();
        }
        DateTime lastClick = DateTime.Now;
        private void dgMyDataGrid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            DataGrid dg = (sender as DataGrid);
            var allObjects = dg.ItemsSource as List<DemandeWorkflowInformation>;
            if (dg.SelectedItem != null)
            {
                DemandeWorkflowInformation SelectedObject = (DemandeWorkflowInformation)dg.SelectedItem;
                checkerSelectedItem((CheckBox)this.dtgrdParametre.Columns[0].GetCellContent(SelectedObject) as CheckBox);
                SelectedObject.IsSelect = true;
                if (!IsTraitementParLot)
                {
                    foreach (DemandeWorkflowInformation item in (dg.ItemsSource as List<DemandeWorkflowInformation>).Where(t => t.CODE != SelectedObject.CODE).ToList())
                    {
                        if (checkSelectedItem((CheckBox)this.dtgrdParametre.Columns[0].GetCellContent(item) as CheckBox))
                        {
                            DechekerSelectedItem((CheckBox)this.dtgrdParametre.Columns[0].GetCellContent(item) as CheckBox);
                            item.IsSelect = false;
                        }
                    }
                }
                if ((DateTime.Now - lastClick).Ticks < 2500000)
                {
                    if (IsConsultationSeul)
                        ConsulterButton_Click(null, null);
                    else
                        EditerButton_Click(null, null);
                }
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
        List<string> pCommnue = new  List<string>();
        List<string> pQuartier =new  List<string>();
        List<int> lstCentreSiteDist = new List<int>();
        List<int> lstCentreSelect= new List<int>();
        
        private void btn_RechercheClient_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                if (this.Cbo_Site.SelectedItem != null)
                {
                    List<ServiceAccueil.CsCentre> lesCentreDuSite = _listeDesCentreExistant.Where(p => p.FK_IDCODESITE == ((ServiceAccueil.CsSite)this.Cbo_Site.SelectedItem).PK_ID).ToList();
                    foreach (var item in lesCentreDuSite)
                        lstCentreSiteDist.Add(item.PK_ID);
                }
                if (LstLesDemandes != null)
                {

                    if (!string.IsNullOrEmpty(this.Txt_NumDevis.Text))
                    {
                        List<DemandeWorkflowInformation> lesDemandes =
                       LstLesDemandes.Where(t => (lstCentreSiteDist.Count == 0 || lstCentreSiteDist.Contains(t.FK_IDCENTRE)) &&
                                                   (lstCentreSelect.Count == 0 || lstCentreSelect.Contains(t.FK_IDCENTRE)) &&
                                                   (pCommnue.Count == 0 || pCommnue.Contains(t.COMMUNE)) &&
                                                   (pQuartier.Count == 0 || pQuartier.Contains(t.QUARTIER)) &&
                                                   (string.IsNullOrEmpty(this.Txt_NumDevis.Text) || t.CODE_DEMANDE_TABLETRAVAIL == this.Txt_NumDevis.Text)
                                                   ).ToList();
                        if (lesDemandes != null && lesDemandes.Count() != 0)
                        {
                            this.dtgrdParametre.ItemsSource = null;
                            this.dtgrdParametre.ItemsSource = lesDemandes;
                        }
                        else 
                          RechercheDemande(lesCentre.Select(t => t.PK_ID).ToList(), this.Txt_NumDevis.Text, false);
                    }
                    else if (Chk_ToutAfficher.IsChecked.Value == true)
                        RechercheDemande(lesCentre.Select(t => t.PK_ID).ToList(), this.Txt_NumDevis.Text, true);

                         
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void Cbo_Centre_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (this.Cbo_Centre.SelectedItem != null)
                {
                    if (lstCentreSelect.Count != 0) lstCentreSelect.Clear();
                    ServiceAccueil.CsCentre centre = Cbo_Centre.SelectedItem as ServiceAccueil.CsCentre;
                    if (centre != null)
                    {
                        this.txtCentre.Text = centre.CODE ?? string.Empty;
                        this.txtCentre.Tag = centre.PK_ID;
                        lstCentreSelect.Add(centre.PK_ID);
                    }
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, "Demande");
            }
        }

        private void Cbo_Commune_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.Cbo_Commune.SelectedItem != null)
            {
                if (pCommnue.Count != 0) pCommnue.Clear();
                CsCommune commune = Cbo_Commune.SelectedItem as CsCommune;
                if (commune != null && commune.LIBELLE  !="TOUS")
                    pCommnue.Add(commune.CODE );
            }
        }

        private void Cbo_quartier_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.Cbo_quartier.SelectedItem != null)
            {
                if(pQuartier.Count != 0)  pQuartier.Clear();
                CsQuartier quartier = Cbo_quartier.SelectedItem as CsQuartier;
                if (quartier != null && quartier.LIBELLE != "TOUS")
                    pQuartier.Add(quartier.CODE );
            }
        }

        private void Txt_NumDevis_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(this.Txt_NumDevis.Text))
            {
                this.dtgrdParametre.ItemsSource = null;
                this.dtgrdParametre.ItemsSource = LstLesDemandes;
            }
        }
    }
}

