using Galatee.Silverlight.Library;
using Galatee.Silverlight.Resources.Parametrage;
using Galatee.Silverlight.ServiceParametrage;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Galatee.Silverlight.Parametrage
{
    public partial class UcWKFConfigurationWorkflow : ChildWindow, INotifyPropertyChanged
    {

        #region Membres

        ObservableCollection<CsRAffectationEtapeWorkflow> donnesDatagrid = new ObservableCollection<CsRAffectationEtapeWorkflow>();
        List<CsRAffectationEtapeWorkflow> lesAnciennesAffectations;
        List<CsRAffectationEtapeWorkflow> ToutesLesEtapesDesCircuitsDetournes = new List<CsRAffectationEtapeWorkflow>();
        public CsOperation OperationParent { get; set; }
        public CsOperation SousOperation { get; set; }
        List<CsOperation> Operations;
        public CsWorkflow WorkflowSelectionne { get; set; }
        List<CsWorkflow> Workflows;
        List<ServiceAdministration.CsSite> _LesSites;
        List<ServiceAdministration.CsCentre> _lesCentres;
        List<CsEtape> _LesEtapes;
        List<CsEtape> _LesEtapesParOperations;
        CsTableDeTravail WorkingTable;
        List<CsTableDeTravail> _tablesTravail = new List<CsTableDeTravail>();
        List<CsGroupeValidation> _lsGroupeValidation;
        CsRWorkflow RWorkflowCentre;
        List<CsEtape> StepAllReadyChoosed;
        Dictionary<Guid, CsConditionBranchement> _RelationAffectationEtapeCondition = new Dictionary<Guid, CsConditionBranchement>();
        int CentreActuelPKID = UserConnecte.FK_IDCENTRE;
        List<CsVwConfigurationWorkflowCentre> _configuration;
        bool _tousLesCentresSelectionne = false;
        List<CsRenvoiRejet> TousLesRenvois;

        #endregion

        public UcWKFConfigurationWorkflow()
        {
            try
            {
                InitializeComponent();
                GetData();
                StepAllReadyChoosed = new List<CsEtape>();

                ActiverOuDesactiverBouttons();
                BtnAjouter.IsEnabled = false;
                btnAfficherCircuit.IsEnabled = false;
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.ListeWorkflow);
            }
        }


        #region INotifyPropertyChanged Membres

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string nompropriete)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(nompropriete));
        }
        #endregion

        public ObservableCollection<CsRAffectationEtapeWorkflow> DonnesDatagrid
        {
            get { return donnesDatagrid; }
            set
            {
                if (value == donnesDatagrid)
                    return;
                donnesDatagrid = value;
                NotifyPropertyChanged("DonnesDatagrid");
            }
        }

        private void ViderDataGrid()
        {
            dtgrdParametre.ItemsSource = null;
        }

        private void ChargerLeCircuit()
        {
            if (null != cmbWorkflow.SelectedValue && null != cmbSite.SelectedValue
                && null != cmbOperation.SelectedValue && null != cmbSousOperation.SelectedValue)
            {
                try
                {
                    //Recherche des affectations workflow étapes                
                    Guid wkfPKID = Guid.Parse(cmbWorkflow.SelectedValue.ToString());
                    int cPKID = 0;
                    int sPKID = int.Parse(cmbSite.SelectedValue.ToString());
                    if (sPKID != 0) // lorsque tous les sites ne sont pas sélectionnés
                    {
                        cPKID = int.Parse(cmbCentre.SelectedValue.ToString());
                    }
                    Guid OpPKID = Guid.Parse(cmbOperation.SelectedValue.ToString());
                    Guid sbOpPKID = Guid.Parse(cmbSousOperation.SelectedValue.ToString());
                    List<CsRWorkflow> wkfCentre = new List<CsRWorkflow>();

                    //On filtre les étapes selon l'opérations sélectionnés
                    _LesEtapesParOperations = _LesEtapes.Where(op => op.FK_IDOPERATION == sbOpPKID)
                        .ToList();

                    int back = LoadingManager.BeginLoading("Chargement des données ...");

                    ParametrageClient client = new ParametrageClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Parametrage"));

                    if (cPKID == 0 && sPKID != 0)
                    {
                        //On a sélectionné tous les centres
                        var centre = _lesCentres.Where(c => c.FK_IDCODESITE == sPKID).FirstOrDefault();
                        if (null != centre)
                        {
                            //On prend le 1er centre et on l'affiche
                            cPKID = centre.PK_ID;
                            _tousLesCentresSelectionne = true;
                        }
                    }
                    else if (cPKID == 0 && sPKID == 0)
                    {
                        //ON a sélectionné tous les centres et tous
                        var centre = _lesCentres.Where(c => c.CODESITE != "000").FirstOrDefault();
                        if (null != centre)
                        {
                            cPKID = centre.PK_ID;
                            _tousLesCentresSelectionne = true;
                        }
                    }

                    client.SelectAllRWorkflowCentreCompleted += (ssender, args) =>
                    {
                        if (args.Cancelled || args.Error != null)
                        {
                            string error = args.Error.Message;
                            Message.Show(error, Languages.ListeWorkflow);
                            return;
                        }
                        if (args.Result == null)
                        {
                            Message.ShowError(Languages.msgErreurChargementDonnees, Languages.Parametrage);
                            return;
                        }
                        wkfCentre = new List<CsRWorkflow>();

                        if (args.Result != null)
                            foreach (var item in args.Result)
                            {
                                wkfCentre.Add(item);
                            }

                        if (null != wkfCentre && wkfCentre.Count > 0)
                        {
                            //On spécifie que ce RWORKFLOWCENTRE existe
                            RWorkflowCentre = wkfCentre.First();

                            //On recherhce maintenant les étapes de cette relation Workflow Centre
                            client.SelectAllAffectationEtapeWorkflowCompleted += (affsender, _args) =>
                            {
                                if (_args.Cancelled || _args.Error != null)
                                {
                                    string error = args.Error.Message;
                                    Message.Show(error, Languages.ListeWorkflow);
                                    return;
                                }
                                if (_args.Result == null)
                                {
                                    Message.ShowError(Languages.msgErreurChargementDonnees, Languages.Parametrage);
                                    return;
                                }
                                donnesDatagrid = new ObservableCollection<CsRAffectationEtapeWorkflow>();
                                lesAnciennesAffectations = new List<CsRAffectationEtapeWorkflow>();
                                _RelationAffectationEtapeCondition = new Dictionary<Guid, CsConditionBranchement>();
                                StepAllReadyChoosed = new List<CsEtape>();
                                ToutesLesEtapesDesCircuitsDetournes = new List<CsRAffectationEtapeWorkflow>();

                                if (_args.Result != null)
                                    foreach (var item in _args.Result)
                                    {
                                        if (!item.Key.FK_IDRETAPEWORKFLOWORIGINE.HasValue || item.Key.FK_IDRETAPEWORKFLOWORIGINE.Value == Guid.Empty)
                                        {
                                            if (null != item.Value)
                                            {
                                                _RelationAffectationEtapeCondition.Add(item.Key.PK_ID, item.Value);
                                                item.Key.CONDITION = item.Value.NOM;
                                            }
                                            else _RelationAffectationEtapeCondition.Add(item.Key.PK_ID, null);

                                            donnesDatagrid.Add(item.Key);
                                            lesAnciennesAffectations.Add(item.Key);
                                            //_RelationAffectationEtapeCondition.Add(item.Key.PK_ID, item.Value);
                                            StepAllReadyChoosed.Add(_LesEtapes.Where(et => et.PK_ID == item.Key.FK_IDETAPE).FirstOrDefault());
                                        }                                        
                                        else
                                        {
                                            ToutesLesEtapesDesCircuitsDetournes.Add(item.Key);
                                        }                                       
                                    }
                                dtgrdParametre.ItemsSource = DonnesDatagrid.OrderBy(c => c.ORDRE);
                                LoadingManager.EndLoading(back);
                            };
                            client.SelectAllAffectationEtapeWorkflowAsync(wkfCentre.First().PK_ID);
                        }
                        else LoadingManager.EndLoading(back);
                    };
                    client.SelectAllRWorkflowCentreAsync(wkfPKID, cPKID, sbOpPKID);

                    if (null == _tablesTravail || _tablesTravail.Count == 0)
                    {
                        //La table de travail concerneée
                        client.SelectAllTableTravailCompleted += (wtsender, args) =>
                        {
                            if (args.Cancelled || args.Error != null)
                            {
                                string error = args.Error.Message;
                                Message.Show(error, Languages.ListeWorkflow);
                                return;
                            }
                            if (args.Result == null)
                            {
                                Message.ShowError(Languages.msgErreurChargementDonnees, Languages.Parametrage);
                                return;
                            }
                            _tablesTravail = args.Result;
                            WorkingTable = new CsTableDeTravail();

                            //ON recherche le workflow concerné
                            CsWorkflow theWKF = Workflows.Where(w => w.PK_ID == wkfPKID)
                                .FirstOrDefault();
                            if (null != theWKF)
                            {
                                WorkingTable = _tablesTravail.Where(t => t.PK_ID == theWKF.FK_IDTABLE_TRAVAIL.Value)
                                    .FirstOrDefault();
                            }
                        };
                        client.SelectAllTableTravailAsync();
                    }
                    else
                    {
                        //ON recherche le workflow concerné
                        WorkingTable = new CsTableDeTravail();

                        //ON recherche le workflow concerné
                        CsWorkflow theWKF = Workflows.Where(w => w.PK_ID == wkfPKID)
                            .FirstOrDefault();
                        if (null != theWKF)
                        {
                            WorkingTable = _tablesTravail.Where(t => t.PK_ID == theWKF.FK_IDTABLE_TRAVAIL.Value)
                                .FirstOrDefault();
                        }
                    }
                }
                catch { }
            }
            else
            {
                Message.ShowError("Veuillez sélection le site et/ou le centre, le circuit de validation, l'opération parente et l'opération enfant",
                    "Configuration Circuit de validation");
            }
        }

        private void GetData()
        {
            int back = 0;
            try
            {
                back = LoadingManager.BeginLoading("Chargement des données en cours...");                

                //Liste des workflows et opérations et sous opérations
                ParametrageClient client = new ParametrageClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Parametrage"));
                client.SelectAllWorkflowCompleted += (ssender, args) =>
                {
                    if (args.Cancelled || args.Error != null)
                    {
                        string error = args.Error.Message;
                        Message.Show(error, Languages.ListeWorkflow);
                        return;
                    }
                    if (args.Result == null)
                    {
                        Message.ShowError(Languages.msgErreurChargementDonnees, Languages.Parametrage);
                        return;
                    }
                    Workflows = new List<CsWorkflow>();
                    cmbWorkflow.Items.Clear();
                    if (args.Result != null)
                        foreach (var item in args.Result)
                        {
                            Workflows.Add(item);
                        }

                    cmbWorkflow.DisplayMemberPath = "WORKFLOWNAME";
                    cmbWorkflow.SelectedValuePath = "PK_ID";
                    cmbWorkflow.ItemsSource = Workflows;

                    //Listes des opérations
                    client.SelectAllOperationCompleted += (opsender, _args) =>
                    {
                        if (_args.Cancelled || _args.Error != null)
                        {
                            string error = _args.Error.Message;
                            Message.Show(error, Languages.ListeWorkflow);
                            return;
                        }
                        if (_args.Result == null)
                        {
                            Message.ShowError(Languages.msgErreurChargementDonnees, Languages.Parametrage);
                            return;
                        }
                        Operations = new List<CsOperation>();
                        cmbOperation.Items.Clear();
                        if (_args.Result != null)
                            foreach (var item in _args.Result)
                            {
                                Operations.Add(item);
                            }

                        cmbOperation.DisplayMemberPath = "NOM";
                        cmbOperation.SelectedValuePath = "PK_ID";
                        cmbOperation.ItemsSource = Operations.Where(op => !op.FK_ID_PARENTOPERATION.HasValue
                            || op.FK_ID_PARENTOPERATION == Guid.Empty)
                            .ToList();


                        //Récupération des sites et centre
                        ServiceAdministration.AdministrationServiceClient admClient = new ServiceAdministration.AdministrationServiceClient(Utility.Protocole(),
                            Utility.EndPoint("Administration"));
                        admClient.GetAllSiteCompleted += (_ssender, __args) =>
                        {
                            if (__args.Cancelled || __args.Error != null)
                            {
                                string error = __args.Error.Message;
                                Message.Show(error, Languages.ListeWorkflow);
                                return;
                            }
                            if (__args.Result == null)
                            {
                                Message.ShowError(Languages.msgErreurChargementDonnees, Languages.Parametrage);
                                return;
                            }
                            _LesSites = new List<ServiceAdministration.CsSite>();
                            _LesSites.Add(new ServiceAdministration.CsSite()
                            {
                                LIBELLE = "Tous les sites",
                                PK_ID = 0
                            });
                            cmbSite.Items.Clear();
                            if (__args.Result != null)
                                foreach (var item in __args.Result /*.Where(s => s.CODE != "000")*/)
                                {
                                    _LesSites.Add(item);
                                }

                            cmbSite.DisplayMemberPath = "LIBELLE";
                            cmbSite.SelectedValuePath = "PK_ID";
                            cmbSite.ItemsSource = _LesSites.Where(t=>t.CODE != "000").ToList();


                            //Les centres
                            admClient.RetourneListeDesCentreCompleted += (csender, cargs) =>
                            {
                                if (cargs.Cancelled || cargs.Error != null)
                                {
                                    string error = cargs.Error.Message;
                                    Message.Show(error, Languages.ListeWorkflow);
                                    return;
                                }
                                if (cargs.Result == null)
                                {
                                    Message.ShowError(Languages.msgErreurChargementDonnees, Languages.Parametrage);
                                    return;
                                }
                                _lesCentres = new List<ServiceAdministration.CsCentre>();
                                cmbCentre.Items.Clear();
                                if (cargs.Result != null)
                                    foreach (var item in cargs.Result)
                                    {
                                        _lesCentres.Add(item);
                                    }

                                cmbCentre.DisplayMemberPath = "LIBELLE";
                                cmbCentre.SelectedValuePath = "PK_ID";
                                cmbCentre.ItemsSource = _lesCentres;

                               
                                //On charge les différents groupe de validation, pour la sélection dans la fenêtre de configuration des étpes
                                client.SelectAllGroupeValidationCompleted += (gsender, gargs) =>
                                {
                                    if (gargs.Cancelled || gargs.Error != null)
                                    {
                                        string error = gargs.Error.Message;
                                        Message.Show(error, Languages.ListeWorkflow);
                                        return;
                                    }
                                    if (args.Result == null)
                                    {
                                        Message.ShowError(Languages.msgErreurChargementDonnees, Languages.Parametrage);
                                        return;
                                    }
                                    _lsGroupeValidation = new List<CsGroupeValidation>();
                                    if (gargs.Result != null)
                                        foreach (var item in gargs.Result)
                                        {
                                            _lsGroupeValidation.Add(item.Key);
                                        }


                                    //On charge aussi les étapes pour éviter un autre chargement dans la fenêtre de configuration
                                    client.SelectAllEtapesCompleted += (esender, eargs) =>
                                    {
                                        if (eargs.Cancelled || eargs.Error != null)
                                        {
                                            string error = eargs.Error.Message;
                                            Message.Show(error, Languages.ListeWorkflow);
                                            return;
                                        }
                                        if (eargs.Result == null)
                                        {
                                            Message.ShowError(Languages.msgErreurChargementDonnees, Languages.Parametrage);
                                            return;
                                        }
                                        _LesEtapes = new List<CsEtape>();
                                        if (eargs.Result != null)
                                            foreach (var item in eargs.Result)
                                            {
                                                _LesEtapes.Add(item);
                                            }

                                        //Chargement des configurations
                                        client.SelectAllConfigurationWorkflowCentreCompleted += (_cfg, args_cfg) =>
                                        {
                                            if (args_cfg.Cancelled || args_cfg.Error != null)
                                            {
                                                string error = args_cfg.Error.Message;
                                                Message.Show(error, Languages.ListeWorkflow);
                                                return;
                                            }
                                            if (args_cfg.Result == null)
                                            {
                                                Message.ShowError(Languages.msgErreurChargementDonnees, Languages.Parametrage);
                                                return;
                                            }
                                            _configuration = new List<CsVwConfigurationWorkflowCentre>();
                                            if (args_cfg.Result != null)
                                                foreach (var item in args_cfg.Result)
                                                {
                                                    _configuration.Add(item);
                                                }


                                            //Récupération des table de travail
                                            client.SelectAllTableTravailCompleted += (wtsender, targs) =>
                                            {
                                                //A la fin du dernier chargement
                                                LoadingManager.EndLoading(back);

                                                if (targs.Cancelled || targs.Error != null)
                                                {
                                                    string error = targs.Error.Message;
                                                    Message.Show(error, Languages.ListeWorkflow);
                                                    return;
                                                }
                                                if (targs.Result == null)
                                                {
                                                    Message.ShowError(Languages.msgErreurChargementDonnees, Languages.Parametrage);
                                                    return;
                                                }
                                                _tablesTravail = targs.Result;
                                            };
                                            client.SelectAllTableTravailAsync();
                                        };
                                        client.SelectAllConfigurationWorkflowCentreAsync();
                                    };
                                    client.SelectAllEtapesAsync();

                                };
                                client.SelectAllGroupeValidationAsync();

                            };
                            admClient.RetourneListeDesCentreAsync();

                        };
                        admClient.GetAllSiteAsync();

                    };
                    client.SelectAllOperationAsync();

                    #region Commenté

                    //if (null == _tablesTravail || _tablesTravail.Count == 0)
                    //{
                    //    //La table de travail concerneée
                    //    client.SelectAllTableTravailCompleted += (wtsender, args) =>
                    //    {
                    //        if (args.Cancelled || args.Error != null)
                    //        {
                    //            string error = args.Error.Message;
                    //            Message.Show(error, Languages.ListeWorkflow);
                    //            return;
                    //        }
                    //        if (args.Result == null)
                    //        {
                    //            Message.ShowError(Languages.msgErreurChargementDonnees, Languages.Parametrage);
                    //            return;
                    //        }
                    //        _tablesTravail = args.Result;
                    //    };
                    //    client.SelectAllTableTravailAsync();
                    //}

                    #endregion
                };
                client.SelectAllWorkflowAsync();                               
                                              
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            //Enregistrement des informations
            Guid wkfPKID = Guid.Parse(cmbWorkflow.SelectedValue.ToString());
            int cPKID = 0;
            int sPKID = int.Parse(cmbSite.SelectedValue.ToString());
            if (sPKID != 0)
            {
                cPKID = int.Parse(cmbCentre.SelectedValue.ToString());
            }
            Guid OpPKID = Guid.Parse(cmbOperation.SelectedValue.ToString());
            Guid sbOpPKID = Guid.Parse(cmbSousOperation.SelectedValue.ToString());

            //si c'est pour tous les sites
            #region Pour tous les sites
            if (sPKID == 0)
            {
                //Dans ce cas on va créer plusieurs RWORKFLOWCENTRE pour tous les centres de chaque site
                List<CsRWorkflow> lesWorkflowCentres = new List<CsRWorkflow>();
                Dictionary<int, Dictionary<CsRAffectationEtapeWorkflow, CsConditionBranchement>> lsConfigurationEtapes = new Dictionary<int,
                    Dictionary<CsRAffectationEtapeWorkflow, CsConditionBranchement>>();
                List<CsRenvoiRejet> lesRenvois = new List<CsRenvoiRejet>();
                List<CsRAffectationEtapeWorkflow> _LesDonneesDuDataGrid = DonnesDatagrid != null ? DonnesDatagrid.ToList():new List<CsRAffectationEtapeWorkflow> () ;
                var __sites = _LesSites.Where(s => s.PK_ID != 0 && (null != s.CODE && string.Empty != s.CODE /*&& "000" != s.CODE*/))
                    .ToList();
                for (int i = 0; i < __sites.Count; i++)
                {
                    var site = __sites[i];

                    //On récupère chaque centre                    
                    List<ServiceAdministration.CsCentre> __centres = _lesCentres.Where(c => c.FK_IDCODESITE == site.PK_ID)
                        .ToList();
                    for (int j = 0; j < __centres.Count; j ++)
                    {
                        var c = __centres[j];
                        
                        CsRWorkflow RWKFCentre = new CsRWorkflow()
                        {
                            FK_IDCENTRE = c.PK_ID,
                            FK_IDOPERATION = sbOpPKID,
                            FK_IDWORKFLOW = wkfPKID,
                            PK_ID = Guid.NewGuid()
                        };

                        //Récupération des affectations étapes
                        Dictionary<CsRAffectationEtapeWorkflow, CsConditionBranchement> lsAffectationsConfigurees = new Dictionary<CsRAffectationEtapeWorkflow, CsConditionBranchement>();
                        List<CsRAffectationEtapeWorkflow> NouveauCircuitsDetournes = new List<CsRAffectationEtapeWorkflow>();
                        for (int k = 0; k < _LesDonneesDuDataGrid.Count; k++)
                        {
                            var aff = _LesDonneesDuDataGrid[k];

                            CsRAffectationEtapeWorkflow NAff = new CsRAffectationEtapeWorkflow()
                            {
                                ALERTE = aff.ALERTE,
                                DUREE = aff.DUREE,
                                CODEETAPE = aff.CODEETAPE,
                                CONDITION = aff.CONDITION,
                                ETAPECONDITIONVRAIE = aff.ETAPECONDITIONVRAIE,
                                FK_IDETAPE = aff.FK_IDETAPE,
                                FK_IDGROUPEVALIDATIOIN = aff.FK_IDGROUPEVALIDATIOIN,
                                FK_IDRETAPEWORKFLOWORIGINE = aff.FK_IDRETAPEWORKFLOWORIGINE,
                                FROMCONDITION = aff.FROMCONDITION,
                                GROUPE_VALIDATION = aff.GROUPE_VALIDATION,
                                GROUPEVALIDATION = aff.GROUPEVALIDATION,
                                ORDRE = aff.ORDRE,
                                LIBELLEETAPE = aff.LIBELLEETAPE,
                                USEAFFECTATION = aff.USEAFFECTATION,
                                FK_RWORKFLOWCENTRE = aff.FK_RWORKFLOWCENTRE
                            };
                            NAff.FK_RWORKFLOWCENTRE = RWKFCentre.PK_ID;
                            //Récupération de tous les circuits de détournement, on connait déjà leur origine
                            //On les ajoutes dans la liste, mais on change les PK_ID, en vue d'une réutilisation
                            //par les autres centres
                            var NEtapes = ToutesLesEtapesDesCircuitsDetournes.Where(et => et.FK_IDRETAPEWORKFLOWORIGINE == NAff.PK_ID);
                            //On change le PK_ID, vu que ce objet sera réutilisé pour les autres centres
                            NAff.PK_ID = Guid.NewGuid();
                            if (null != NEtapes && NEtapes.Count() > 0)
                            {
                                foreach (var huum in NEtapes)
                                {
                                    huum.FK_IDRETAPEWORKFLOWORIGINE = NAff.PK_ID;
                                    huum.PK_ID = Guid.NewGuid();
                                    huum.FK_RWORKFLOWCENTRE = RWKFCentre.PK_ID;
                                }
                            }

                            if (null != _RelationAffectationEtapeCondition[aff.PK_ID])
                            {
                                CsConditionBranchement NCondition = new CsConditionBranchement()
                                {
                                    PK_ID = Guid.NewGuid(),
                                    COLONNENAME = _RelationAffectationEtapeCondition[aff.PK_ID].COLONNENAME,
                                    FK_IDETAPEFAUSE = _RelationAffectationEtapeCondition[aff.PK_ID].FK_IDETAPEFAUSE,
                                    FK_IDETAPEVRAIE = _RelationAffectationEtapeCondition[aff.PK_ID].FK_IDETAPEVRAIE,
                                    FK_IDRAFFECTATIONWKF = NAff.PK_ID,
                                    FK_IDTABLETRAVAIL = _RelationAffectationEtapeCondition[aff.PK_ID].FK_IDTABLETRAVAIL,
                                    NOM = _RelationAffectationEtapeCondition[aff.PK_ID].NOM,
                                    VALUE = _RelationAffectationEtapeCondition[aff.PK_ID].VALUE,
                                    PEUT_TRANSMETTRE_SI_FAUX = _RelationAffectationEtapeCondition[aff.PK_ID].PEUT_TRANSMETTRE_SI_FAUX,
                                    OPERATEUR = _RelationAffectationEtapeCondition[aff.PK_ID].OPERATEUR
                                };
                                lsAffectationsConfigurees.Add(NAff, NCondition);

                                GC.SuppressFinalize(NCondition);
                            }
                            else lsAffectationsConfigurees.Add(NAff, null);

                            //On ajoute les circuits détournés par une condition
                            foreach (var _huum in NEtapes)
                                lsAffectationsConfigurees.Add(_huum, null);

                            //Les renvois
                            if (null != TousLesRenvois)
                            {
                                var ___renvois = TousLesRenvois.Where(r => r.FK_IDRAFFECTATION == aff.PK_ID).ToList();
                                if (null != ___renvois && ___renvois.Count > 0)
                                {
                                    foreach (var rv in ___renvois)
                                    {
                                        rv.FK_IDRAFFECTATION = NAff.PK_ID;
                                        lesRenvois.Add(rv);
                                    }
                                }
                            }
                            //On s'assure de le disposer
                            GC.SuppressFinalize(NAff);
                        }

                        //On ajoute le RWorkflowCentre et sa configuration dans notre liste
                        //Pour l'insertion
                        lesWorkflowCentres.Add(RWKFCentre);
                        lsConfigurationEtapes.Add(c.PK_ID, lsAffectationsConfigurees);
                    }
                }

                //On lance l'enregistrement
                //On désactive tout d'abord
                //AllInOne.ActivateControlsFromXaml(LayoutRoot, false);
                Dictionary<CsRAffectationEtapeWorkflow, CsConditionBranchement> configurations = new Dictionary<CsRAffectationEtapeWorkflow, CsConditionBranchement>();
                foreach (var oo in lsConfigurationEtapes.Values)
                {
                    foreach (var __oo in oo.Keys)
                    {
                        configurations.Add(__oo, oo[__oo]);
                    }
                }
                //On lance l'enregistrement                
                AjouterOuMettreAjourPlusieursConfiguration(lesWorkflowCentres, configurations, lesRenvois);
                
            }
            #endregion

            #region C'est pour tous les centres (le site est sélectionné)

            else if (sPKID != 0 && cPKID == 0)
            {
                //Dans ce cas on va créer plusieurs RWORKFLOWCENTRE pour tous les centres de chaque site
                List<CsRWorkflow> lesWorkflowCentres = new List<CsRWorkflow>();
                Dictionary<int, Dictionary<CsRAffectationEtapeWorkflow, CsConditionBranchement>> lsConfigurationEtapes = new Dictionary<int,
                    Dictionary<CsRAffectationEtapeWorkflow, CsConditionBranchement>>();
                List<CsRenvoiRejet> lesRenvois = new List<CsRenvoiRejet>();
                List<CsRAffectationEtapeWorkflow> _LesDonneesDuDataGrid = DonnesDatagrid.ToList();

                //On récupère chaque centre
                List<ServiceAdministration.CsCentre> __centres = _lesCentres.Where(c => c.FK_IDCODESITE == sPKID)
                    .ToList();
                foreach (var c in __centres)
                {
                    CsRWorkflow RWKFCentre = new CsRWorkflow()
                    {
                        FK_IDCENTRE = c.PK_ID,
                        FK_IDOPERATION = sbOpPKID,
                        FK_IDWORKFLOW = wkfPKID,
                        PK_ID = Guid.NewGuid()
                    };

                    //Récupération des affectations étapes
                    Dictionary<CsRAffectationEtapeWorkflow, CsConditionBranchement> lsAffectationsConfigurees = new Dictionary<CsRAffectationEtapeWorkflow, CsConditionBranchement>();
                    List<CsRAffectationEtapeWorkflow> NouveauCircuitsDetournes = new List<CsRAffectationEtapeWorkflow>();
                    foreach (var aff in _LesDonneesDuDataGrid)
                    {
                        CsRAffectationEtapeWorkflow NAff = new CsRAffectationEtapeWorkflow()
                        {
                            ALERTE = aff.ALERTE,
                            DUREE = aff.DUREE,
                            CODEETAPE = aff.CODEETAPE,
                            CONDITION = aff.CONDITION,
                            ETAPECONDITIONVRAIE = aff.ETAPECONDITIONVRAIE,
                            FK_IDETAPE = aff.FK_IDETAPE,
                            FK_IDGROUPEVALIDATIOIN = aff.FK_IDGROUPEVALIDATIOIN,
                            FK_IDRETAPEWORKFLOWORIGINE = aff.FK_IDRETAPEWORKFLOWORIGINE,
                            FROMCONDITION = aff.FROMCONDITION,
                            GROUPE_VALIDATION = aff.GROUPE_VALIDATION,
                            GROUPEVALIDATION = aff.GROUPEVALIDATION,
                            ORDRE = aff.ORDRE,
                            LIBELLEETAPE = aff.LIBELLEETAPE,
                            USEAFFECTATION = aff.USEAFFECTATION,
                            FK_RWORKFLOWCENTRE = aff.FK_RWORKFLOWCENTRE
                        };
                        NAff.FK_RWORKFLOWCENTRE = RWKFCentre.PK_ID;
                        //Récupération de tous les circuits de détournement, on connait déjà leur origine
                        //On les ajoutes dans la liste, mais on change les PK_ID, en vue d'une réutilisation
                        //par les autres centres
                        var NEtapes = ToutesLesEtapesDesCircuitsDetournes.Where(et => et.FK_IDRETAPEWORKFLOWORIGINE == NAff.PK_ID);
                        //On change le PK_ID, vu que ce objet sera réutilisé pour les autres centres
                        NAff.PK_ID = Guid.NewGuid();
                        if (null != NEtapes && NEtapes.Count() > 0)
                        {
                            foreach (var huum in NEtapes)
                            {
                                huum.FK_IDRETAPEWORKFLOWORIGINE = NAff.PK_ID;
                                huum.PK_ID = Guid.NewGuid();
                                huum.FK_RWORKFLOWCENTRE = RWKFCentre.PK_ID;
                            }
                        }

                        if (null != _RelationAffectationEtapeCondition[aff.PK_ID])
                        {
                            CsConditionBranchement NCondition = new CsConditionBranchement()
                            {
                                PK_ID = Guid.NewGuid(),
                                COLONNENAME = _RelationAffectationEtapeCondition[aff.PK_ID].COLONNENAME,
                                FK_IDETAPEFAUSE = _RelationAffectationEtapeCondition[aff.PK_ID].FK_IDETAPEFAUSE,
                                FK_IDETAPEVRAIE = _RelationAffectationEtapeCondition[aff.PK_ID].FK_IDETAPEVRAIE,
                                FK_IDRAFFECTATIONWKF = NAff.PK_ID,
                                FK_IDTABLETRAVAIL = _RelationAffectationEtapeCondition[aff.PK_ID].FK_IDTABLETRAVAIL,
                                NOM = _RelationAffectationEtapeCondition[aff.PK_ID].NOM,
                                VALUE = _RelationAffectationEtapeCondition[aff.PK_ID].VALUE,
                                PEUT_TRANSMETTRE_SI_FAUX = _RelationAffectationEtapeCondition[aff.PK_ID].PEUT_TRANSMETTRE_SI_FAUX,
                                OPERATEUR = _RelationAffectationEtapeCondition[aff.PK_ID].OPERATEUR
                            };
                            lsAffectationsConfigurees.Add(NAff, NCondition);

                            GC.SuppressFinalize(NCondition);
                        }
                        else lsAffectationsConfigurees.Add(NAff, null);

                        //On ajoute les circuits détournés par une condition
                        foreach (var _huum in NEtapes)
                            lsAffectationsConfigurees.Add(_huum, null);

                        //Les renvois
                        if (null != TousLesRenvois)
                        {
                            var ___renvois = TousLesRenvois.Where(r => r.FK_IDRAFFECTATION == aff.PK_ID).ToList();
                            if (null != ___renvois && ___renvois.Count > 0)
                            {
                                foreach (var rv in ___renvois)
                                {
                                    rv.FK_IDRAFFECTATION = NAff.PK_ID;
                                    lesRenvois.Add(rv);
                                }
                            }
                        }
                        //On s'assure de le disposer
                        GC.SuppressFinalize(NAff);
                    }

                    //On ajoute le RWorkflowCentre et sa configuration dans notre liste
                    //Pour l'insertion
                    lesWorkflowCentres.Add(RWKFCentre);
                    lsConfigurationEtapes.Add(c.PK_ID, lsAffectationsConfigurees);
                }

                Dictionary<CsRAffectationEtapeWorkflow, CsConditionBranchement> configurations = new Dictionary<CsRAffectationEtapeWorkflow, CsConditionBranchement>();
                foreach (var oo in lsConfigurationEtapes.Values)
                {
                    foreach (var __oo in oo.Keys)
                    {
                        configurations.Add(__oo, oo[__oo]);
                    }
                }
                //On lance l'enregistrement                
                AjouterOuMettreAjourPlusieursConfiguration(lesWorkflowCentres, configurations, lesRenvois);
            }

            #endregion
            //Pour le moment on va se baser par centre
            #region Par Site & Centre (Déjà sélectionné)
            else
            {
                bool update = false;
                if (null == RWorkflowCentre)
                {
                    CsRWorkflow RWKFCentre = new CsRWorkflow()
                    {
                        FK_IDCENTRE = cPKID,
                        FK_IDOPERATION = sbOpPKID,
                        FK_IDWORKFLOW = wkfPKID,
                        PK_ID = Guid.NewGuid()
                    };
                    RWorkflowCentre = RWKFCentre;
                    update = false;
                }

                //On ne change aucune valeur, puisque ce sont seulement des clés étrangères
                else update = true;


                //Récupération des affectations étapes
                Dictionary<CsRAffectationEtapeWorkflow, CsConditionBranchement> lsAffectationsConfigurees = new Dictionary<CsRAffectationEtapeWorkflow, CsConditionBranchement>();
                var renvois = new List<CsRenvoiRejet>();
                foreach (var aff in DonnesDatagrid)
                {
                    aff.FK_RWORKFLOWCENTRE = RWorkflowCentre.PK_ID;
                    foreach (var rv in TousLesRenvois)
                    {
                        rv.FK_IDRAFFECTATION = aff.PK_ID;
                        renvois.Add(rv);
                    }
                    lsAffectationsConfigurees.Add(aff, _RelationAffectationEtapeCondition[aff.PK_ID]);
                }

                //Récupération de tous les circuits de détournement, on connait déjà leur origine
                //On les ajoutes dans la liste lsAffectationsconfigurees
                if (null != ToutesLesEtapesDesCircuitsDetournes)
                {
                    foreach (var aff in ToutesLesEtapesDesCircuitsDetournes)
                    {
                        aff.FK_RWORKFLOWCENTRE = RWorkflowCentre.PK_ID;
                        lsAffectationsConfigurees.Add(aff, null);
                    }
                }

                //Appel de la fonction de création de la configuration
                AjouterOuMettreAJourUneConfiguration(RWorkflowCentre, lsAffectationsConfigurees, renvois);
            }
            #endregion

        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            cmbOperation.IsEnabled = false;
            cmbSite.IsEnabled = false;
            cmbSousOperation.IsEnabled = false;
            cmbCentre.IsEnabled = false;
        }

        private void Supprimer()
        {
            try
            {
                if (DonnesDatagrid.Count == 0)
                    throw new Exception(Languages.AucuneDonneeASupprimer);
                if (dtgrdParametre.SelectedItem != null && dtgrdParametre.SelectedItems.Count == 1)
                {

                    var w = new MessageBoxControl.MessageBoxChildWindow(Languages.Parametrage, Languages.QuestionSuppressionDonnees, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                    w.OnMessageBoxClosed += (_, result) =>
                    {
                        if (w.Result == MessageBoxResult.OK)
                        {
                            var selected = dtgrdParametre.SelectedItem as CsRAffectationEtapeWorkflow;
                            if (selected != null)
                            {
                                ParametrageClient delete = new ParametrageClient(Utility.Protocole(),
                                                                                 Utility.EndPoint("Parametrage"));
                                int back = LoadingManager.BeginLoading("Suppression en cours ...");
                                delete.DeleteAffectationEtapeWorkflowCompleted += (del, argDel) =>
                                {
                                    if (argDel.Cancelled || argDel.Error != null)
                                    {
                                        Message.Show(argDel.Error.Message, Languages.Parametrage);
                                        return;
                                    }
                                    if (argDel.Result == false)
                                    {
                                        Message.Show(Languages.ErreurSuppressionDonnees, Languages.Parametrage);
                                        return;
                                    }

                                    //On cherche dans les circuits détournés
                                    LoadingManager.EndLoading(back);
                                    List<Guid> etapesdetournes = ToutesLesEtapesDesCircuitsDetournes.Where(e => e.FK_IDRETAPEWORKFLOWORIGINE == selected.PK_ID)
                                        .Select(c => c.PK_ID)
                                        .ToList();
                                    foreach (Guid g in etapesdetournes)
                                    {
                                        var toDelete = ToutesLesEtapesDesCircuitsDetournes.Where(c => c.PK_ID == g).FirstOrDefault();
                                        if (null != toDelete) ToutesLesEtapesDesCircuitsDetournes.Remove(toDelete);
                                    }
                                    DonnesDatagrid.Remove(selected);
                                    dtgrdParametre.ItemsSource = DonnesDatagrid;
                                };
                                delete.DeleteAffectationEtapeWorkflowAsync(new List<CsRAffectationEtapeWorkflow>() { selected });
                            }
                        }
                        else
                        {
                            return;
                        }
                    };
                    w.Show();
                }
                else
                {
                    throw new Exception(Languages.SelectionnerUnElement);
                }
            }
            catch (Exception ex)
            {
                Message.Show(ex.Message, Languages.Parametrage);
            }
        }

        private void ActiverOuDesactiverBouttons()
        {
            BtnCondition.IsEnabled = BtnConditionBloquante.IsEnabled = BtnDown.IsEnabled =
                BtnUp.IsEnabled = BtnModifier.IsEnabled = BtnSupprimer.IsEnabled =
                (null != dtgrdParametre.SelectedItems && dtgrdParametre.SelectedItems.Count == 1);

            //Cela signifie qu'il ya un seul élement sélectionné
            if (BtnSupprimer.IsEnabled)
            {
                CsRAffectationEtapeWorkflow aff = dtgrdParametre.SelectedItems[0] as CsRAffectationEtapeWorkflow;
                if (null != aff)
                {
                    BtnUp.IsEnabled = (aff.ORDRE > 1);
                    BtnDown.IsEnabled = (aff.ORDRE < DonnesDatagrid.Count);
                }
            }
        }

        private void dtgrdParametre_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                //aTa0 o = lvwResultat.SelectedItem as aTa0;
                string Ucname = string.Empty;
                //    if(int.Parse(o.CODE) > 0 && int.Parse(o.CODE)  < 1000)
            }
            catch (Exception ex)
            {
                Message.Show(ex.Message, Languages.ListeWorkflow);
            }
        }

        private void dtgrdParametre_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ActiverOuDesactiverBouttons();
        }
        private void dtgrdParametre_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                //ObjetSelectionne = dtgrdParametre.SelectedItem as CsWorkflow;
                SessionObject.objectSelected = dtgrdParametre.SelectedItem as CsWorkflow;
                SessionObject.gridUtilisateur = dtgrdParametre;
            }
            catch (Exception ex)
            {
                Message.Show(ex.Message, Languages.LibelleCodePoste);
            }
        }


        #region "Gestion MenuContextuel"

        private void MenuContextuelCreerEtape_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //On prend la dernière étape du datagrid
                IOrderedEnumerable<CsRAffectationEtapeWorkflow> dtGridRAff = dtgrdParametre.ItemsSource as IOrderedEnumerable<CsRAffectationEtapeWorkflow>;
                CsRAffectationEtapeWorkflow lastStep = null;

                if (null != dtGridRAff) lastStep = dtGridRAff.LastOrDefault();
                CsEtape beforeStep = null;
                if (null == StepAllReadyChoosed) StepAllReadyChoosed = new List<CsEtape>();

                CsWorkflow leCircuit = Workflows.Where(w => w.PK_ID == Guid.Parse(cmbWorkflow.SelectedValue.ToString()))
                    .First();
                if (null != leCircuit.TABLENAME && string.Empty != leCircuit.TABLENAME)
                {
                    if (null != lastStep) beforeStep = _LesEtapesParOperations.Where(st => st.PK_ID == lastStep.FK_IDETAPE)
                        .FirstOrDefault();
                    UcWKFSelectEtape uctl = new UcWKFSelectEtape(_LesEtapesParOperations, beforeStep, StepAllReadyChoosed, dtgrdParametre, WorkingTable, _lsGroupeValidation);
                    uctl.Closing += uctl_Closing;
                    uctl.Show();
                }
                else
                {
                    if (null != lastStep) beforeStep = _LesEtapesParOperations.Where(st => st.PK_ID == lastStep.FK_IDETAPE)
                        .FirstOrDefault();
                    UcWKFSelectEtape uctl = new UcWKFSelectEtape(_LesEtapesParOperations, beforeStep, StepAllReadyChoosed, dtgrdParametre, WorkingTable, _lsGroupeValidation, false);
                    uctl.Closing += uctl_Closing;
                    uctl.Show();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void uctl_Closing(object sender, CancelEventArgs e)
        {
            if (null == _RelationAffectationEtapeCondition) _RelationAffectationEtapeCondition = new Dictionary<Guid, CsConditionBranchement>();
            if (null == DonnesDatagrid) DonnesDatagrid = new ObservableCollection<CsRAffectationEtapeWorkflow>();

            //On récupère l'etape créée
            KeyValuePair<CsRAffectationEtapeWorkflow, CsConditionBranchement> RAffAndCondition = ((UcWKFSelectEtape)sender)
                .Step;

            if (null != RAffAndCondition.Key)
            {
                //On teste d'abord l'existance
                if (null != DonnesDatagrid.Where(c => c.PK_ID == RAffAndCondition.Key.PK_ID).FirstOrDefault() &&
                    _RelationAffectationEtapeCondition.Keys.Contains(RAffAndCondition.Key.PK_ID))
                {
                    //C'est la modification
                    CsRAffectationEtapeWorkflow lAncienAff = DonnesDatagrid.Where(c => c.PK_ID == RAffAndCondition.Key.PK_ID).First();
                    DonnesDatagrid.Remove(lAncienAff);
                    //On remet l'ordre de l'ancien
                    RAffAndCondition.Key.ORDRE = lAncienAff.ORDRE;
                    DonnesDatagrid.Add(RAffAndCondition.Key);

                    //On supprime l'étape qui avait été choisi
                    var toDelete = _LesEtapes.Where(et => et.PK_ID == lAncienAff.FK_IDETAPE).FirstOrDefault();
                    var toAdd = _LesEtapes.Where(et => et.PK_ID == RAffAndCondition.Key.FK_IDETAPE).FirstOrDefault();
                    StepAllReadyChoosed.Remove(toDelete);
                    //On ajoute l'étape qui vient d'être choisi, si bien sûr elle n'existe pas déjà
                    if (!StepAllReadyChoosed.Contains(toAdd)) StepAllReadyChoosed.Add(toAdd);

                    //On les supprime pour vider la mémoire
                    GC.SuppressFinalize(toDelete);
                    GC.SuppressFinalize(toAdd);

                    RAffAndCondition.Key.CONDITION = (null != RAffAndCondition.Value) ? RAffAndCondition.Value.NOM
                        : string.Empty;

                    if (_RelationAffectationEtapeCondition.Keys.Contains(RAffAndCondition.Key.PK_ID))
                        _RelationAffectationEtapeCondition[RAffAndCondition.Key.PK_ID] = RAffAndCondition.Value;
                    else _RelationAffectationEtapeCondition.Add(RAffAndCondition.Key.PK_ID, RAffAndCondition.Value);

                }
                //C'est l'insertion
                else if (null == DonnesDatagrid.Where(c => c.PK_ID == RAffAndCondition.Key.PK_ID).FirstOrDefault() &&
                    !_RelationAffectationEtapeCondition.Keys.Contains(RAffAndCondition.Key.PK_ID))
                {
                    RAffAndCondition.Key.ORDRE = DonnesDatagrid.Count + 1;
                    DonnesDatagrid.Add(RAffAndCondition.Key);
                    //On ajoute l'étape qui a été choisi
                    StepAllReadyChoosed.Add(_LesEtapes.Where(et => et.PK_ID == RAffAndCondition.Key.FK_IDETAPE).FirstOrDefault());
                    //On ajoute la condition
                    RAffAndCondition.Key.CONDITION = (null != RAffAndCondition.Value) ? RAffAndCondition.Value.NOM
                        : string.Empty;

                    _RelationAffectationEtapeCondition.Add(RAffAndCondition.Key.PK_ID, RAffAndCondition.Value);
                }
            }
            //On récupère les renvois
            List<CsRenvoiRejet> __renvois = ((UcWKFSelectEtape)sender).LesRenvoisRejets;
            if (null != __renvois && __renvois.Count > 0)
            {
                if (null == TousLesRenvois) TousLesRenvois = new List<CsRenvoiRejet>();
                __renvois.ForEach((CsRenvoiRejet rv) =>
                {
                    rv.FK_IDRAFFECTATION = RAffAndCondition.Key.PK_ID;
                    TousLesRenvois.Add(rv);
                });
            }

            //Ok que ce soit l'insertion ou la modification, on récupère les circuits détournés,
            //on connait leur origine
            if (null == ToutesLesEtapesDesCircuitsDetournes)
                ToutesLesEtapesDesCircuitsDetournes = new List<CsRAffectationEtapeWorkflow>();
            if (null != ((UcWKFSelectEtape)sender).LeCircuitDetourne && ((UcWKFSelectEtape)sender).LeCircuitDetourne.Count > 0)
            {
                //On supprime tous et on reinsert (ha ha ha ha :D)
                var lAncienCircuits = ToutesLesEtapesDesCircuitsDetournes.Where(raff => raff.FK_IDRETAPEWORKFLOWORIGINE == RAffAndCondition.Key.PK_ID)
                    .ToList();
                if (lAncienCircuits.Count > 0)
                {
                    lAncienCircuits.ForEach((CsRAffectationEtapeWorkflow r_aff) =>
                    {
                        ToutesLesEtapesDesCircuitsDetournes.Remove(r_aff);
                    });
                }
                //Reinsertion
                ToutesLesEtapesDesCircuitsDetournes.AddRange(((UcWKFSelectEtape)sender).LeCircuitDetourne);
            }
            if (null != DonnesDatagrid) dtgrdParametre.ItemsSource = DonnesDatagrid.OrderBy(aff => aff.ORDRE);
        }

        private void MenuContextuelAjouterCondition_Click(object sender, RoutedEventArgs e)
        {
            if (null != dtgrdParametre.SelectedItems && dtgrdParametre.SelectedItems.Count == 1)
            {
                CsRAffectationEtapeWorkflow affectationEtape = dtgrdParametre.SelectedItems[0] as CsRAffectationEtapeWorkflow;
                if (null != affectationEtape)
                {
                    CsEtape lEtape = _LesEtapes.Where(et => et.PK_ID == affectationEtape.FK_IDETAPE)
                        .FirstOrDefault();
                    if (null != lEtape)
                    {
                        Guid sbOpPKID = Guid.Parse(cmbSousOperation.SelectedValue.ToString());
                        UcWKFCondition formC = new UcWKFCondition(lEtape, _LesEtapes.Where(p => p.FK_IDOPERATION == sbOpPKID).ToList(), WorkingTable, 
                            _lsGroupeValidation);
                        formC.Closing += formC_Closing;
                        formC.Show();
                    }
                }
            }
        }

        private void Modifier_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dtgrdParametre.SelectedItem != null)
                {
                    var objetselectionne = (CsRAffectationEtapeWorkflow)dtgrdParametre.SelectedItem;
                    if (null != objetselectionne)
                    {
                        IOrderedEnumerable<CsRAffectationEtapeWorkflow> dtGridRAff = dtgrdParametre.ItemsSource as IOrderedEnumerable<CsRAffectationEtapeWorkflow>;
                        CsRAffectationEtapeWorkflow lastStep = null;

                        if (null != dtGridRAff) lastStep = dtGridRAff.Where(aff => aff.ORDRE < objetselectionne.ORDRE)
                            .OrderBy(a => a.ORDRE)
                            .LastOrDefault();
                        CsEtape beforeStep = null;
                        if (null != lastStep) beforeStep = _LesEtapesParOperations.Where(st => st.PK_ID == lastStep.FK_IDETAPE)
                            .FirstOrDefault();
                        List<CsEtape> IlFautQuandMemeAjouterLaMemeEtapePourQuellePuisseEtreSelecionneeDansLaFenetreDeConfiguration = new List<CsEtape>();
                        StepAllReadyChoosed.ForEach((CsEtape etap) =>
                        {
                            if (etap.PK_ID != objetselectionne.FK_IDETAPE)
                                IlFautQuandMemeAjouterLaMemeEtapePourQuellePuisseEtreSelecionneeDansLaFenetreDeConfiguration.Add(etap);
                        });

                        KeyValuePair<CsRAffectationEtapeWorkflow, CsConditionBranchement> leAffChoosed = new KeyValuePair<CsRAffectationEtapeWorkflow, CsConditionBranchement>(
                            objetselectionne, _RelationAffectationEtapeCondition[objetselectionne.PK_ID]);

                        var detournementCircuit = ToutesLesEtapesDesCircuitsDetournes.Where(aff__ => aff__.FK_IDRETAPEWORKFLOWORIGINE == objetselectionne.PK_ID)
                            .ToList();

                        UcWKFSelectEtape form = new UcWKFSelectEtape(_LesEtapesParOperations, beforeStep, IlFautQuandMemeAjouterLaMemeEtapePourQuellePuisseEtreSelecionneeDansLaFenetreDeConfiguration, dtgrdParametre, WorkingTable, _lsGroupeValidation, leAffChoosed,
                            SessionObject.ExecMode.Modification, detournementCircuit, true);
                        form.Closing += uctl_Closing;
                        form.Show();
                    }
                }
            }
            catch (Exception ex)
            {
                Message.Show(ex.Message, Languages.FenetreOperation);
            }
        }

        private void Supprimer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dtgrdParametre.SelectedItem != null)
                {
                    Supprimer();
                }
            }
            catch (Exception ex)
            {
                Message.Show(ex.Message, Languages.Fonction);
            }
        }


        private void MenuContextuel_Opened(object sender, RoutedEventArgs e)
        {
            try
            {
                //MenuContextuelModifier.IsEnabled = MenuContextuelSupprimer.IsEnabled = dtgrdParametre.SelectedItems.Count > 0;
                //MenuContextuelModifier.UpdateLayout();
                //MenuContextuel.UpdateLayout();
            }
            catch (Exception ex)
            {
                Message.Show(ex.Message, Languages.Fonction);
            }
        }


        #endregion

        private void cmbWorkflow_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Activation du site
            cmbSite.IsEnabled = true;
            ViderDataGrid();
            CheckConfigurationWorkflow();
        }

        private void cmbSite_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Activation des centres;
            cmbCentre.IsEnabled = true;
            int PKID = int.Parse(cmbSite.SelectedValue.ToString());
            if (PKID == 0)  //Tous les centres
            {
                //Activation des opérations
                cmbOperation.IsEnabled = true;
            }
            else
            {

                List<ServiceAdministration.CsCentre> _humCentres = new List<ServiceAdministration.CsCentre>();
                _humCentres.Add(new ServiceAdministration.CsCentre()
                {
                    PK_ID = 0,
                    LIBELLE = "Tous les centres"
                });
                _humCentres.AddRange(_lesCentres.Where(s => s.FK_IDCODESITE == PKID)
                    .ToList());
                cmbCentre.DisplayMemberPath = "LIBELLE";
                cmbCentre.SelectedValuePath = "PK_ID";
                cmbCentre.ItemsSource = _humCentres;
            }

            ViderDataGrid();
            CheckConfigurationWorkflow();

        }

        private void cmbOperation_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Activation des sous opérations
            Guid pKID = Guid.Parse(cmbOperation.SelectedValue.ToString());
            if (pKID != Guid.Empty)
            {
                cmbSousOperation.IsEnabled = true;
                List<CsOperation> _sousOp = Operations.Where(op => op.FK_ID_PARENTOPERATION == pKID).OrderBy(t=>t.NOM)
                    .ToList();

                cmbSousOperation.DisplayMemberPath = "NOM";
                cmbSousOperation.SelectedValuePath = "PK_ID";
                cmbSousOperation.ItemsSource = _sousOp;
            }

            ViderDataGrid();
        }

        private void cmbSousOperation_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ViderDataGrid();
            CheckConfigurationWorkflow();
        }

        private void cmbCentre_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            cmbOperation.IsEnabled = true;
            CheckConfigurationWorkflow();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            ChargerLeCircuit();
            BtnAjouter.IsEnabled = true;
        }

        private void BtnConditionBloquante_Click(object sender, RoutedEventArgs e)
        {
            if (null != dtgrdParametre.SelectedItems && dtgrdParametre.SelectedItems.Count == 1)
            {
                CsRAffectationEtapeWorkflow affectationEtape = dtgrdParametre.SelectedItems[0] as CsRAffectationEtapeWorkflow;
                if (null != affectationEtape)
                {
                    CsEtape lEtape = _LesEtapes.Where(et => et.PK_ID == affectationEtape.FK_IDETAPE)
                        .FirstOrDefault();
                    if (null != lEtape)
                    {
                        UcWKFCondition formC = new UcWKFCondition(lEtape, WorkingTable, _lsGroupeValidation, true);
                        formC.Closing += formC_Closing;
                        formC.Show();
                    }
                }
            }
        }

        void formC_Closing(object sender, CancelEventArgs e)
        {
            UcWKFCondition conditionFrm = ((UcWKFCondition)sender);
            if (null != conditionFrm.LaCondition)
            {
                CsConditionBranchement C = conditionFrm.LaCondition;
                CsRAffectationEtapeWorkflow etapeAff = dtgrdParametre.SelectedItems[0] as CsRAffectationEtapeWorkflow;
                if (null != etapeAff)
                {
                    C.FK_IDRAFFECTATIONWKF = etapeAff.PK_ID;
                    if (_RelationAffectationEtapeCondition.Keys.Contains(etapeAff.PK_ID) && null == _RelationAffectationEtapeCondition[etapeAff.PK_ID])
                    {
                        _RelationAffectationEtapeCondition[etapeAff.PK_ID] = C;
                    }
                    else if (!_RelationAffectationEtapeCondition.Keys.Contains(etapeAff.PK_ID)) _RelationAffectationEtapeCondition.Add(etapeAff.PK_ID, C);

                    IOrderedEnumerable<CsRAffectationEtapeWorkflow> ordreEtapes = DonnesDatagrid as IOrderedEnumerable<CsRAffectationEtapeWorkflow>;
                    if (null == ordreEtapes) ordreEtapes = (DonnesDatagrid as ObservableCollection<CsRAffectationEtapeWorkflow>).OrderBy(o => o.ORDRE);
                    var etp = ordreEtapes.Where(et => et.PK_ID == etapeAff.PK_ID).FirstOrDefault();
                    if (null != etp)
                    {
                        etp.CONDITION = "1(s) condition(s)";
                    }
                    DonnesDatagrid = new ObservableCollection<CsRAffectationEtapeWorkflow>(ordreEtapes.ToList());
                    dtgrdParametre.ItemsSource = DonnesDatagrid.OrderBy(c => c.ORDRE);
                }
            }
        }

        private void BtnDown_Click(object sender, RoutedEventArgs e)
        {
            //Changement de l'ordre de l'élément actuel
            CsRAffectationEtapeWorkflow etapeAff = dtgrdParametre.SelectedItems[0] as CsRAffectationEtapeWorkflow;
            IOrderedEnumerable<CsRAffectationEtapeWorkflow> ordreEtapes = dtgrdParametre.ItemsSource as IOrderedEnumerable<CsRAffectationEtapeWorkflow>;
            var etp = ordreEtapes.Where(et => et.PK_ID == etapeAff.PK_ID).FirstOrDefault();
            if (null != etp)
            {
                int index_etp = ordreEtapes.ToList().IndexOf(etp);
                //Récupération de l'étape suivante
                var nextetp = ordreEtapes.Where(et => et.ORDRE == (etp.ORDRE + 1)).FirstOrDefault();
                if (null != nextetp)
                {
                    int index_next = ordreEtapes.ToList().IndexOf(nextetp);
                    nextetp.ORDRE = etp.ORDRE;
                    ordreEtapes.ToList().Remove(nextetp);
                    ordreEtapes.ToList().Insert(index_next, nextetp);
                }
                etp.ORDRE += 1;
                ordreEtapes.ToList().Insert(index_etp, etp);
            }
            DonnesDatagrid = new ObservableCollection<CsRAffectationEtapeWorkflow>(ordreEtapes.ToList());
            dtgrdParametre.ItemsSource = DonnesDatagrid.OrderBy(c => c.ORDRE);
        }

        private void BtnUp_Click(object sender, RoutedEventArgs e)
        {
            //Changement de l'ordre de l'élément actuel
            CsRAffectationEtapeWorkflow etapeAff = dtgrdParametre.SelectedItems[0] as CsRAffectationEtapeWorkflow;
            IOrderedEnumerable<CsRAffectationEtapeWorkflow> ordreEtapes = dtgrdParametre.ItemsSource as IOrderedEnumerable<CsRAffectationEtapeWorkflow>;
            var etp = ordreEtapes.Where(et => et.PK_ID == etapeAff.PK_ID).FirstOrDefault();
            if (null != etp)
            {
                int index_etp = ordreEtapes.ToList().IndexOf(etp);
                //Récupération de l'étape suivante
                var nextetp = ordreEtapes.Where(et => et.ORDRE == (etp.ORDRE - 1)).FirstOrDefault();
                if (null != nextetp)
                {
                    int index_next = ordreEtapes.ToList().IndexOf(nextetp);
                    nextetp.ORDRE = etp.ORDRE;
                    ordreEtapes.ToList().Remove(nextetp);
                    ordreEtapes.ToList().Insert(index_next, nextetp);
                }
                etp.ORDRE -= 1;
                ordreEtapes.ToList().Insert(index_etp, etp);
            }
            DonnesDatagrid = new ObservableCollection<CsRAffectationEtapeWorkflow>(ordreEtapes.ToList());
            dtgrdParametre.ItemsSource = DonnesDatagrid.OrderBy(c => c.ORDRE);
        }

        private void AjouterOuMettreAJourUneConfiguration(CsRWorkflow rwkfCentre, Dictionary<CsRAffectationEtapeWorkflow, CsConditionBranchement> lsAffectationsConfigurees, 
            List<CsRenvoiRejet> LesRenvois)
        {
            #region Par Site & Centre (Déjà sélectionné)
            bool update = false;

            int back = LoadingManager.BeginLoading("Enregistrement de la configuration en cours ...");
            ParametrageClient client = new ParametrageClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Parametrage"));

            //On vérfie que la configuration existe dans la base de données
            client.SelectAllRWorkflowCentreCompleted += (rsender, args) =>
            {
                if (args.Cancelled || args.Error != null)
                {
                    string error = args.Error.Message;
                    Message.Show(error, Languages.ListeWorkflow);
                    update = false;
                    return;
                }
                if (args.Result == null)
                {
                    Message.ShowError(Languages.msgErreurChargementDonnees, Languages.Parametrage);
                    update = false;
                    return;
                }
                else if (args.Result.Count == 0) update = false;
                else if (args.Result.Count >= 1) update = true;

                //On lance l'enregistrement
                //On désactive tout d'abord
                //AllInOne.ActivateControlsFromXaml(LayoutRoot, false);            

                if (!update)
                {
                    //1 - RWORKFLOW Centre
                    bool insertRWKF = true;
                    //Nouvelle insertion
                    client.InsertRWorkflowCompleted += (__sender, __args) =>
                    {
                        if (args.Cancelled || args.Error != null)
                        {
                            string error = __args.Error.Message;
                            Message.Show(error, Languages.ListeWorkflow);
                            insertRWKF = false;
                            return;
                        }
                        if (!(bool)__args.Result)
                        {
                            Message.ShowError(Languages.msgErreurChargementDonnees, Languages.Parametrage);
                            insertRWKF = false;
                            return;
                        }

                        //On continue ?
                        if (insertRWKF)
                        {
                            //Enregistrement des affectations et workflows
                            client.InsertAffectationEtapeWorkflowCompleted += (affsender, _args) =>
                            {
                                if (_args.Cancelled || _args.Error != null)
                                {
                                    string error = _args.Error.Message;
                                    Message.Show(error, Languages.ListeWorkflow);
                                    return;
                                }
                                if (!(bool)_args.Result)
                                {
                                    Message.ShowError(Languages.msgErreurChargementDonnees, Languages.Parametrage);
                                    return;
                                }

                                //Renvoi Etape
                                client.AjouterRenvoisRejetCompleted += (rv_sender, rv_args) =>
                                {
                                    if (rv_args.Cancelled || rv_args.Error != null)
                                    {
                                        string error = _args.Error.Message;
                                        Message.Show(error, Languages.ListeWorkflow);
                                        return;
                                    }
                                    if (!(bool)rv_args.Result)
                                    {
                                        Message.ShowError(Languages.msgErreurChargementDonnees, Languages.Parametrage);
                                        return;
                                    }

                                    //Tout s'est bien passé, Grâce à DIEU.
                                    Message.ShowInformation("Configuration Enregistrée", "Configuration Circuit de Validation");
                                    LoadingManager.EndLoading(back);

                                    this.DialogResult = true;
                                };
                                client.AjouterRenvoisRejetAsync(LesRenvois);
                            };
                            client.InsertAffectationEtapeWorkflowAsync(lsAffectationsConfigurees);
                        }
                    };
                    client.InsertRWorkflowAsync(new List<CsRWorkflow>() { rwkfCentre });

                    if (insertRWKF) this.DialogResult = true;
                    else this.DialogResult = false;
                }

                //Modification
                else if (update)
                {
                    //On supprime tous, et on réinsert tous
                    //On a déjà la liste des précédentes Affectations
                    bool deleteAffWKF = true;

                    //Aussi la suppression va concernée les étapes d'un circuit détourné
                    client.DeleteAffectationEtapeWorkflowCompleted += (dltsender, dltargs) =>
                    {
                        if (dltargs.Cancelled || dltargs.Error != null)
                        {
                            string error = dltargs.Error.Message;
                            Message.Show(error, Languages.ListeWorkflow);
                            deleteAffWKF = false;
                            return;
                        }
                        if (!(bool)dltargs.Result)
                        {
                            Message.ShowError(Languages.msgErreurChargementDonnees, Languages.Parametrage);
                            deleteAffWKF = false;
                            return;
                        }

                        if (deleteAffWKF)
                        {
                            //Enregistrement des affectations, avec les mises à jour
                            client.InsertAffectationEtapeWorkflowCompleted += (affsender, _args) =>
                            {
                                if (_args.Cancelled || _args.Error != null)
                                {
                                    string error = args.Error.Message;
                                    Message.Show(error, Languages.ListeWorkflow);
                                    return;
                                }
                                if (!(bool)_args.Result)
                                {
                                    Message.ShowError(Languages.msgErreurChargementDonnees, Languages.Parametrage);
                                    return;
                                }

                                //Tout s'est bien passé, Grâce à DIEU.
                                Message.ShowInformation("Configuration Enregistrée", "Configuration Circuit de Validation");
                                LoadingManager.EndLoading(back);
                                //AllInOne.ActivateControlsFromXaml(LayoutRoot, true);
                                this.DialogResult = true;
                            };
                            client.InsertAffectationEtapeWorkflowAsync(lsAffectationsConfigurees);
                        }
                    };
                    client.DeleteAffectationEtapeWorkflowAsync(lesAnciennesAffectations);

                }

            };
            client.SelectAllRWorkflowCentreAsync(rwkfCentre.FK_IDWORKFLOW, rwkfCentre.FK_IDCENTRE, rwkfCentre.FK_IDOPERATION);

            #endregion

        }

        private void CheckConfigurationWorkflow()
        {
            if (null != cmbWorkflow.SelectedValue && null != cmbSite.SelectedValue
                && null != cmbOperation.SelectedValue && null != cmbSousOperation.SelectedValue)
            {
                try
                {
                    //Recherche des affectations workflow étapes                
                    Guid wkfPKID = Guid.Parse(cmbWorkflow.SelectedValue.ToString());

                    #region Info sur la table de travail

                    var workflow = Workflows.FirstOrDefault(w => w.PK_ID == wkfPKID);
                    WorkingTable = null != workflow && null != _tablesTravail && _tablesTravail.Count > 0 && workflow.FK_IDTABLE_TRAVAIL.HasValue && 0 != workflow.FK_IDTABLE_TRAVAIL.Value ?
                        _tablesTravail.FirstOrDefault(x => x.PK_ID == workflow.FK_IDTABLE_TRAVAIL.Value) : null;

                    #endregion

                    int cPKID = 0;
                    int sPKID = int.Parse(cmbSite.SelectedValue.ToString());
                    if (sPKID != 0) // lorsque tous les sites ne sont pas sélectionnés
                    {
                        cPKID = int.Parse(cmbCentre.SelectedValue.ToString());
                    }
                    Guid OpPKID = Guid.Parse(cmbOperation.SelectedValue.ToString());
                    Guid sbOpPKID = Guid.Parse(cmbSousOperation.SelectedValue.ToString());
                    List<CsRWorkflow> wkfCentre = new List<CsRWorkflow>();

                    //On filtre les étapes selon l'opérations sélectionnés
                    _LesEtapesParOperations = _LesEtapes.Where(op => op.FK_IDOPERATION == sbOpPKID)
                        .ToList();

                    //ON check l'existance de la configuration sélectionnée
                    if (cPKID != 0)
                    {
                        btnAfficherCircuit.IsEnabled = null != _configuration.Where(cfg => cfg.CENTREID == cPKID && cfg.OPERATIONID == sbOpPKID
                            && cfg.PK_ID == wkfPKID).FirstOrDefault();
                        _tousLesCentresSelectionne = false;
                    }
                    else if (cPKID == 0 && sPKID == 0)
                    {
                        //ON a sélectionné tous les centres et tous les sites,
                        //on récupère le 1er centre dont le code site est distinct de "000"
                        var centre = _lesCentres.Where(c => c.CODESITE != "000" && c.CODE != "000").FirstOrDefault();
                        if (null != centre)
                        {
                            cPKID = centre.PK_ID;
                            btnAfficherCircuit.IsEnabled = null != _configuration.Where(cfg => cfg.CENTREID == cPKID && cfg.OPERATIONID == sbOpPKID
                                && cfg.PK_ID == wkfPKID).FirstOrDefault();
                            _tousLesCentresSelectionne = true;
                        }
                    }
                    else
                    {
                        //on a sélectionnée tous les centres, dans ce cas on récupère le
                        //1er centre
                        if (null != _lesCentres.Where(c => c.FK_IDCODESITE == sPKID).FirstOrDefault())
                        {
                            cPKID = _lesCentres.Where(c => c.FK_IDCODESITE == sPKID).First().PK_ID;
                            //Pour tous les centres on récupère seulement
                            btnAfficherCircuit.IsEnabled = null != _configuration.Where(cfg => cfg.CENTREID == cPKID && cfg.OPERATIONID == sbOpPKID
                                && cfg.PK_ID == wkfPKID).FirstOrDefault();

                            _tousLesCentresSelectionne = true;
                        }
                    }
                }
                catch { }
                //on active toujours le bouton, car si ya rien on peut commencer à ajouter les étapes
                BtnAjouter.IsEnabled = true;
                RWorkflowCentre = null;
                donnesDatagrid = null;
                lesAnciennesAffectations = null;
                _RelationAffectationEtapeCondition = null;
                StepAllReadyChoosed = null;
                ToutesLesEtapesDesCircuitsDetournes = null;

                dtgrdParametre.ItemsSource = null;
            }
            else
            {
                btnAfficherCircuit.IsEnabled = false;
            }
        }

        private void AjouterOuMettreAjourPlusieursConfiguration(List<CsRWorkflow> lsRwkfCentres, Dictionary<CsRAffectationEtapeWorkflow, CsConditionBranchement> lsAffectationsConfigurees,
            List<CsRenvoiRejet> LesRenvois)
        {
            ParametrageClient client = new ParametrageClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Parametrage"));
            int back = LoadingManager.BeginLoading("Enregistrement des configurations en cours ... (" + lsRwkfCentres.Count + ")");

            client.Endpoint.Binding.OpenTimeout = new TimeSpan(0, 1, 0);
            client.Endpoint.Binding.CloseTimeout = new TimeSpan(5, 0, 0);
            client.Endpoint.Binding.SendTimeout = new TimeSpan(5, 0, 0);

            client.ConfigurerPlusieurWorkflowEtCentreCompleted += (sender, args) =>
            {
                if (args.Cancelled || args.Error != null)
                {
                    string error = args.Error.Message;
                    Message.Show(error, Languages.ListeWorkflow);
                    return;
                }
                if (!(bool)args.Result)
                {
                    Message.ShowError(Languages.msgErreurChargementDonnees, Languages.Parametrage);
                    return;
                }

                //Tout s'est bien passé, Grâce à DIEU.
                Message.ShowInformation("Configurations Enregistrées", "Configuration Circuit de Validation");
                LoadingManager.EndLoading(back);
                //AllInOne.ActivateControlsFromXaml(LayoutRoot, true);
                this.DialogResult = true;
            };
            client.ConfigurerPlusieurWorkflowEtCentreAsync(lsRwkfCentres, lsAffectationsConfigurees, LesRenvois);
        }

        private void dtgrdParametre_LoadingRow_1(object sender, DataGridRowEventArgs e)
        {            
        }
    }
}

