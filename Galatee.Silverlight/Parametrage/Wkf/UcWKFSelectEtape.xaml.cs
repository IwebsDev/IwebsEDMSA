using Galatee.Silverlight.Library;
using Galatee.Silverlight.ServiceParametrage;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Threading;
using System.Threading.Tasks;
using Galatee.Silverlight.Resources.Parametrage;
using System.ComponentModel;

namespace Galatee.Silverlight.Parametrage
{
    public partial class UcWKFSelectEtape : ChildWindow, INotifyPropertyChanged
    {

        #region Membres

        KeyValuePair<CsRAffectationEtapeWorkflow, CsConditionBranchement> ObjetSelectionne;
        public KeyValuePair<CsRAffectationEtapeWorkflow, CsConditionBranchement> Step { get; set; }
        public List<CsRenvoiRejet> LesRenvoisRejets { get; set; }
        CsRAffectationEtapeWorkflow _detournement;
        List<CsRAffectationEtapeWorkflow> _leCircuitDetourne;
        public List<CsRAffectationEtapeWorkflow> LeCircuitDetourne
        {
            get { return _leCircuitDetourne; }
            set { _leCircuitDetourne = value; }
        }
        public CsRAffectationEtapeWorkflow FirsTEtapeDetournementCircuit
        {
            get { return _detournement; }
            set { _detournement = value; }
        }
        CsConditionBranchement TheCondition;
        SessionObject.ExecMode _execMode = SessionObject.ExecMode.Creation; //Par défaut;
        List<CsEtape> _ttsLesEtapes;
        List<CsEtape> _lesEtapesANePasChoisir;
        CsEtape _EtapePrecedente;
        KeyValuePair<CsRAffectationEtapeWorkflow, CsConditionBranchement> _StepAndCondition;
        ObservableCollection<CsRAffectationEtapeWorkflow> donnesDatagrid = new ObservableCollection<CsRAffectationEtapeWorkflow>();
        ObservableCollection<CsRAffectationEtapeWorkflow> _SecondedonnesDatagrid = new ObservableCollection<CsRAffectationEtapeWorkflow>();
        private DataGrid dataGrid = null;
        CsTableDeTravail _workingTable;
        List<CsGroupeValidation> _LesGroupesValidation;

        //On aura besoin de ces variables au cas où on doit appeler cette même fenêtre pour configurer un autre circuit de détournement
        List<CsEtape> StepAllReadyChoosed;
        Dictionary<Guid, CsConditionBranchement> _RelationAffectationEtapeCondition = new Dictionary<Guid, CsConditionBranchement>();
        List<CsRAffectationEtapeWorkflow> lesAnciennesAffectations;        

        //Spécifie si on autorise l'utilisation des conditions, lors de la spécifications des étapes
        bool _useCondition = true;
        public bool UseCondition
        {
            get { return _useCondition; }
            set {
                _useCondition = value;
                if (!value)
                {
                    ((Grid)GroupBox.Content).Children.Remove(lblCondition);
                    ((Grid)GroupBox.Content).Children.Remove(txtCondition);
                    ((Grid)GroupBox.Content).Children.Remove(HPLSupprime);

                    Height = Height - GroupBoxdtGrid.Height;
                    LayoutRoot.Children.Remove(GroupBoxdtGrid);
                }
            }
        }


        #endregion


        #region Constructeurs

        public UcWKFSelectEtape(List<CsEtape> lsEtapes, CsEtape BeforeStep, List<CsEtape> EtapeDejaChoisie, DataGrid pdtGrid, CsTableDeTravail theWorkingTable, 
            List<CsGroupeValidation> grpValidations)
        {
            try
            {                
                InitializeComponent();

                dataGrid = pdtGrid;
                if (dataGrid != null) donnesDatagrid = dataGrid.ItemsSource as ObservableCollection<CsRAffectationEtapeWorkflow>;
                
                _ttsLesEtapes = lsEtapes;
                _workingTable = theWorkingTable;
                _LesGroupesValidation = grpValidations;
                _lesEtapesANePasChoisir = EtapeDejaChoisie;
                _EtapePrecedente = BeforeStep;
                SetData();                
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, "Configuration Etape");
            }
        }

        public UcWKFSelectEtape(List<CsEtape> lsEtapes, CsEtape BeforeStep, List<CsEtape> EtapeDejaChoisie, DataGrid pdtGrid, CsTableDeTravail theWorkingTable,
            List<CsGroupeValidation> grpValidations, bool canAddCondition)
        {
            try
            {
                InitializeComponent();

                dataGrid = pdtGrid;
                if (dataGrid != null) donnesDatagrid = dataGrid.ItemsSource as ObservableCollection<CsRAffectationEtapeWorkflow>;

                _ttsLesEtapes = lsEtapes;
                _workingTable = theWorkingTable;
                _LesGroupesValidation = grpValidations;
                _lesEtapesANePasChoisir = EtapeDejaChoisie;
                _EtapePrecedente = BeforeStep;
                UseCondition = canAddCondition;

                SetData();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, "Configuration Etape");
            }
        }
        
        public UcWKFSelectEtape(List<CsEtape> lsEtapes, CsEtape BeforeStep, List<CsEtape> EtapeDejaChoisie, DataGrid pdtGrid, CsTableDeTravail theWorkingTable, List<CsGroupeValidation> grpValidations,
            KeyValuePair<CsRAffectationEtapeWorkflow, CsConditionBranchement> lEtapeEtCondition, SessionObject.ExecMode execMode)
        {
            try
            {
                InitializeComponent();

                dataGrid = pdtGrid;
                if (dataGrid != null) donnesDatagrid = dataGrid.ItemsSource as ObservableCollection<CsRAffectationEtapeWorkflow>;
                _ttsLesEtapes = lsEtapes;
                _workingTable = theWorkingTable;
                _LesGroupesValidation = grpValidations;
                _StepAndCondition = lEtapeEtCondition;
                _EtapePrecedente = BeforeStep;
                Step = _StepAndCondition;
                _lesEtapesANePasChoisir = EtapeDejaChoisie;
                _execMode = execMode;

                SetData();
                ShowDetailEtape();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, "Configuration Etape");
            }
        }

        public UcWKFSelectEtape(List<CsEtape> lsEtapes, CsEtape BeforeStep, List<CsEtape> EtapeDejaChoisie, DataGrid pdtGrid, CsTableDeTravail theWorkingTable, List<CsGroupeValidation> grpValidations,
            KeyValuePair<CsRAffectationEtapeWorkflow, CsConditionBranchement> lEtapeEtCondition, SessionObject.ExecMode execMode,
            CsRAffectationEtapeWorkflow detournementCircuit)
        {
            try
            {
                InitializeComponent();

                dataGrid = pdtGrid;
                if (dataGrid != null) donnesDatagrid = dataGrid.ItemsSource as ObservableCollection<CsRAffectationEtapeWorkflow>;
                _ttsLesEtapes = lsEtapes;
                _workingTable = theWorkingTable;
                _LesGroupesValidation = grpValidations;
                _StepAndCondition = lEtapeEtCondition;
                _EtapePrecedente = BeforeStep;
                Step = _StepAndCondition;
                _lesEtapesANePasChoisir = EtapeDejaChoisie;
                _detournement = detournementCircuit;
                _execMode = execMode;

                SetData();
                ShowDetailEtape();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, "Configuration Etape");
            }
        }

        public UcWKFSelectEtape(List<CsEtape> lsEtapes, CsEtape BeforeStep, List<CsEtape> EtapeDejaChoisie, DataGrid pdtGrid, CsTableDeTravail theWorkingTable, List<CsGroupeValidation> grpValidations,
            KeyValuePair<CsRAffectationEtapeWorkflow, CsConditionBranchement> lEtapeEtCondition, SessionObject.ExecMode execMode,
            List<CsRAffectationEtapeWorkflow> leCircuitDetourne, bool canAddCondition)
        {
            try
            {
                InitializeComponent();

                dataGrid = pdtGrid;
                if (dataGrid != null) donnesDatagrid = dataGrid.ItemsSource as ObservableCollection<CsRAffectationEtapeWorkflow>;
                _ttsLesEtapes = lsEtapes;
                _workingTable = theWorkingTable;
                _LesGroupesValidation = grpValidations;
                _StepAndCondition = lEtapeEtCondition;
                _EtapePrecedente = BeforeStep;
                Step = _StepAndCondition;
                _lesEtapesANePasChoisir = EtapeDejaChoisie;
                _leCircuitDetourne = leCircuitDetourne;
                _execMode = execMode;
                UseCondition = canAddCondition;

                SetData();
                ShowDetailEtape();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, "Configuration Etape");
            }
        }


        public UcWKFSelectEtape(List<CsEtape> lsEtapes, CsEtape BeforeStep, List<CsEtape> EtapeDejaChoisie, DataGrid pdtGrid, CsTableDeTravail theWorkingTable, List<CsGroupeValidation> grpValidations,
           KeyValuePair<CsRAffectationEtapeWorkflow, CsConditionBranchement> lEtapeEtCondition, SessionObject.ExecMode execMode,
            bool CanAddCondition)
        {
            try
            {
                InitializeComponent();

                dataGrid = pdtGrid;
                if (dataGrid != null) donnesDatagrid = dataGrid.ItemsSource as ObservableCollection<CsRAffectationEtapeWorkflow>;
                _ttsLesEtapes = lsEtapes;
                _workingTable = theWorkingTable;
                _LesGroupesValidation = grpValidations;
                _StepAndCondition = lEtapeEtCondition;
                _EtapePrecedente = BeforeStep;
                Step = _StepAndCondition;
                _lesEtapesANePasChoisir = EtapeDejaChoisie;
                _execMode = execMode;
                UseCondition = CanAddCondition;

                SetData();
                ShowDetailEtape();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, "Configuration Etape");
            }
        }

        #endregion


        #region INotifyPropertyChanged Membres

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string nompropriete)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(nompropriete));
        }
        #endregion


        public ObservableCollection<CsRAffectationEtapeWorkflow> SecondedonnesDatagrid
        {
            get { return _SecondedonnesDatagrid; }
            set
            {
                if (value == _SecondedonnesDatagrid)
                    return;
                _SecondedonnesDatagrid = value;
                NotifyPropertyChanged("SecondedonnesDatagrid");
            }
        }


        #region Fonctions

        private bool VerifierChampDelai()
        {
            try
            {
                int.Parse(txtDelai.Text);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private void SetData()
        {
            try
            {
                if (null != _ttsLesEtapes && _ttsLesEtapes.Count > 0 && null != _LesGroupesValidation)
                {

                    cmbEtape.DisplayMemberPath = "NOM";
                    cmbEtape.SelectedValuePath = "PK_ID";
                    List<CsEtape> CeQuonPeutChoisir = new List<CsEtape>();
                    _ttsLesEtapes.ForEach((CsEtape etap) =>
                        {
                            if (null == _lesEtapesANePasChoisir.Where(et => et.PK_ID == etap.PK_ID)
                                .FirstOrDefault())
                                CeQuonPeutChoisir.Add(etap);
                        });

                    cmbEtape.ItemsSource = CeQuonPeutChoisir;

                    cmbGroupeValidation.DisplayMemberPath = "GROUPENAME";
                    cmbGroupeValidation.SelectedValuePath = "PK_ID";
                    cmbGroupeValidation.ItemsSource = _LesGroupesValidation.OrderBy(t=>t.GROUPENAME).ToList();



                    //Etape précédente
                    if (null != _EtapePrecedente) txtEtapePrecedente.Text = _EtapePrecedente.NOM;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void ShowDetailEtape()
        {
            if (null != _StepAndCondition.Key)
            {
                cmbEtape.SelectedValue = _StepAndCondition.Key.FK_IDETAPE;
                cmbGroupeValidation.SelectedValue = _StepAndCondition.Key.FK_IDGROUPEVALIDATIOIN;
                if (null != _EtapePrecedente) txtEtapePrecedente.Text = _EtapePrecedente.NOM;

                if (UseCondition && (null == _leCircuitDetourne || _leCircuitDetourne.Count == 0))
                {
                    //On suppose que c'est le 1er niveau

                    txtCondition.Text = (null != _StepAndCondition.Value) ? _StepAndCondition.Value.COLONNENAME + " " + _StepAndCondition.Value.VALUE
                        : string.Empty;

                    txtDelai.Text = (_StepAndCondition.Key.DUREE.HasValue) ? _StepAndCondition.Key.DUREE.Value.ToString() : "";

                    //Si il existe un circuit détourné, alors on va cherchés les infos
                    ParametrageClient client = new ParametrageClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Parametrage"));
                    int back = LoadingManager.BeginLoading("Chargement des données ...");
                    client.SelectAllAffectationEtapeCircuitDetourneCompleted += (affsender, args) =>
                    {
                        if (args.Cancelled || args.Error != null)
                        {
                            string error = args.Error.Message;
                            Message.Show(error, Languages.ListeWorkflow);
                            LoadingManager.EndLoading(back);
                            return;
                        }
                        if (args.Result == null)
                        {
                            Message.ShowError(Languages.msgErreurChargementDonnees, Languages.Parametrage);
                            LoadingManager.EndLoading(back);
                            return;
                        }
                        SecondedonnesDatagrid = new ObservableCollection<CsRAffectationEtapeWorkflow>();
                        lesAnciennesAffectations = new List<CsRAffectationEtapeWorkflow>();
                        _RelationAffectationEtapeCondition = new Dictionary<Guid, CsConditionBranchement>();
                        StepAllReadyChoosed = new List<CsEtape>();

                        if (args.Result != null)
                            foreach (var item in args.Result)
                            {
                                SecondedonnesDatagrid.Add(item.Key);
                                lesAnciennesAffectations.Add(item.Key);
                                _RelationAffectationEtapeCondition.Add(item.Key.PK_ID, item.Value);
                                StepAllReadyChoosed.Add(_ttsLesEtapes.Where(et => et.PK_ID == item.Key.FK_IDETAPE).FirstOrDefault());
                            }
                        dtgrdParametre.ItemsSource = SecondedonnesDatagrid.OrderBy(c => c.ORDRE);
                        LoadingManager.EndLoading(back);
                    };
                    client.SelectAllAffectationEtapeCircuitDetourneAsync(_StepAndCondition.Key.PK_ID);
                }
                else if (UseCondition && (null != _leCircuitDetourne && _leCircuitDetourne.Count > 0))
                {
                    txtCondition.Text = (null != _StepAndCondition.Value) ? _StepAndCondition.Value.COLONNENAME + " " + _StepAndCondition.Value.VALUE
                        : string.Empty;
                    //On affiche le circuit détourné
                    SecondedonnesDatagrid = new ObservableCollection<CsRAffectationEtapeWorkflow>();
                    lesAnciennesAffectations = new List<CsRAffectationEtapeWorkflow>();
                    _RelationAffectationEtapeCondition = new Dictionary<Guid, CsConditionBranchement>();
                    StepAllReadyChoosed = new List<CsEtape>();
                    foreach (var r_aff in _leCircuitDetourne)
                    {
                        SecondedonnesDatagrid.Add(r_aff);
                        lesAnciennesAffectations.Add(r_aff);
                        _RelationAffectationEtapeCondition.Add(r_aff.PK_ID, null);
                        StepAllReadyChoosed.Add(_ttsLesEtapes.Where(et => et.PK_ID == r_aff.FK_IDETAPE).FirstOrDefault());
                    }
                    dtgrdParametre.ItemsSource = SecondedonnesDatagrid.OrderBy(c => c.ORDRE);
                }
                //if (_execMode == SessionObject.ExecMode.Consultation) AllInOne.ActivateControlsFromXaml(LayoutRoot, false);                               
            }
        }

        private void UpdateParentList(List<CsRAffectationEtapeWorkflow> pListeObjet)
        {
            try
            {
                if (_execMode == SessionObject.ExecMode.Creation)
                {
                    if (pListeObjet != null && pListeObjet.Count > 0)
                        foreach (var item in pListeObjet)
                        {
                            donnesDatagrid.Add(item);
                        }
                }
                else if (_execMode == SessionObject.ExecMode.Modification)
                {
                    foreach (var item in pListeObjet)
                    {
                        donnesDatagrid.Remove(item);
                        donnesDatagrid.Add(item);
                    }
                }
                donnesDatagrid.OrderBy(p => p.PK_ID);
                DialogResult = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        #endregion

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            //On crée laffectation
            if (null != cmbEtape.SelectedValue && null != cmbGroupeValidation.SelectedValue)
            {
                if (!VerifierChampDelai())
                {
                    Message.ShowError(new Exception("Veuillez donner une durée d'exécution de l'étape"), "Affectation Etape");
                    return;
                }

                int PKIDStep = int.Parse(cmbEtape.SelectedValue.ToString());
                CsEtape LEtapeOh = _ttsLesEtapes.Where(et => et.PK_ID == PKIDStep)
                    .FirstOrDefault();

                Guid GrpPKID = Guid.Parse(cmbGroupeValidation.SelectedValue.ToString());

                if (null != LEtapeOh && Guid.Empty != GrpPKID)
                {
                    if (null == Step.Key)
                    {
                        CsRAffectationEtapeWorkflow affEtape = new CsRAffectationEtapeWorkflow()
                        {
                            PK_ID = Guid.NewGuid(),
                            CODEETAPE = LEtapeOh.CODE,
                            FK_IDETAPE = PKIDStep,
                            FK_IDGROUPEVALIDATIOIN = GrpPKID,
                            LIBELLEETAPE = LEtapeOh.NOM,
                            GROUPEVALIDATION = _LesGroupesValidation.Where(g => g.PK_ID == GrpPKID).FirstOrDefault().GROUPENAME,
                            DUREE = int.Parse(txtDelai.Text)
                        };
                        if (null != TheCondition)
                        {
                            affEtape.CONDITION = TheCondition.NOM;
                            TheCondition.FK_IDRAFFECTATIONWKF = affEtape.PK_ID;
                        }

                        Step = new KeyValuePair<CsRAffectationEtapeWorkflow, CsConditionBranchement>(affEtape, TheCondition);
                    }
                    else
                    {
                        CsRAffectationEtapeWorkflow affEtape = new CsRAffectationEtapeWorkflow()
                        {
                            PK_ID = Step.Key.PK_ID,
                            CODEETAPE = LEtapeOh.CODE,
                            FK_IDETAPE = PKIDStep,
                            FK_IDGROUPEVALIDATIOIN = GrpPKID,
                            LIBELLEETAPE = LEtapeOh.NOM,
                            ORDRE = Step.Key.ORDRE,
                            DUREE = int.Parse(txtDelai.Text)
                        };
                        if (null != TheCondition)
                        {
                            affEtape.CONDITION = TheCondition.NOM;
                            TheCondition.FK_IDRAFFECTATIONWKF = affEtape.PK_ID;
                        }

                        Step = new KeyValuePair<CsRAffectationEtapeWorkflow, CsConditionBranchement>(affEtape, TheCondition);
                    }

                    //On va maintenant récupérer les étapes du circuits de détournement
                    LeCircuitDetourne = new List<CsRAffectationEtapeWorkflow>();
                    if (null != SecondedonnesDatagrid)
                    {
                        foreach (CsRAffectationEtapeWorkflow aff in SecondedonnesDatagrid)
                        {
                            aff.FK_IDRETAPEWORKFLOWORIGINE = Step.Key.PK_ID;
                            LeCircuitDetourne.Add(aff);
                        }
                    }
                    this.DialogResult = true;
                }
                else this.DialogResult = false;
            }
            else
            {
                Message.ShowError(new Exception("Veuillez sélectionner une étape et un groupe de validation"),
                    "Affectation Etape");
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void cmbEtape_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {            
        }

        private void btnSpecifierCondition_Click(object sender, RoutedEventArgs e)
        {
            int PKIDStep = int.Parse(cmbEtape.SelectedValue.ToString());
            CsEtape step = _ttsLesEtapes.Where(et => et.PK_ID == PKIDStep)
                .FirstOrDefault();
            if (null != step)
            {
                if (null != _StepAndCondition.Key && null != _StepAndCondition.Value)
                {
                    UcWKFCondition formC = new UcWKFCondition(step, _ttsLesEtapes, _workingTable, _LesGroupesValidation,
                        _StepAndCondition.Value, SessionObject.ExecMode.Modification, _detournement);
                    formC.Closing += formC_Closing;
                    formC.Show();
                }
                else
                {
                    UcWKFCondition formC = new UcWKFCondition(step, _ttsLesEtapes, _workingTable, _LesGroupesValidation);
                    formC.Closing += formC_Closing;
                    formC.Show();
                }                
            }
        }

        void formC_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //On récupère la condition et on affiche la valeur
            CsConditionBranchement condition = ((UcWKFCondition)sender).LaCondition;
            TheCondition = condition;
            //On affiche dans le textbox
            if (null != TheCondition) txtCondition.Text = condition.COLONNENAME + " " + 
                condition.OPERATEUR + " " + condition.VALUE;
            //En créant une condition, on crée un nouveau circuit, donc un nouveau RWORKFLOWCENTRE
            //c'est un détournement de circuit, avec pour origine l'étape actuelle
            _detournement = ((UcWKFCondition)sender).FirstEtapeDetournement;

            //donc lui déjà on l'ajoute dans le ObservableCollection SecondedonnesDatagrid
            if (null == SecondedonnesDatagrid) SecondedonnesDatagrid = new ObservableCollection<CsRAffectationEtapeWorkflow>();
            SecondedonnesDatagrid.Add(_detournement);

            dtgrdParametre.ItemsSource = SecondedonnesDatagrid.OrderBy(c => c.ORDRE);
        }


        #region Gestion Menu Contextuel


        private void Supprimer()
        {
            try
            {
                if (SecondedonnesDatagrid.Count == 0)
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
                                    SecondedonnesDatagrid.Remove(selected);
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
        
        private void MenuContextuel_Opened(object sender, RoutedEventArgs e)
        {
            try
            {
                MenuContextuelModifier.IsEnabled = MenuContextuelSupprimer.IsEnabled = dtgrdParametre.SelectedItems.Count > 0;
                MenuContextuelModifier.UpdateLayout();
                MenuContextuel.UpdateLayout();
            }
            catch (Exception ex)
            {
                Message.Show(ex.Message, Languages.Fonction);
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

        private void MenuContextuelCreerEtape_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //On prend la dernière étape du datagrid
                IOrderedEnumerable<CsRAffectationEtapeWorkflow> dtGridRAff = dtgrdParametre.ItemsSource as IOrderedEnumerable<CsRAffectationEtapeWorkflow>;
                CsRAffectationEtapeWorkflow lastStep = null;

                if (null != dtGridRAff) lastStep = dtGridRAff.LastOrDefault();
                CsEtape beforeStep = null;
                if (null != lastStep) beforeStep = _ttsLesEtapes.Where(st => st.PK_ID == lastStep.FK_IDETAPE)
                    .FirstOrDefault();

                //Appel récursive de cette même fenêtre pour configurer le circuit de détournement
                //causé par la condition sur l'étape courante d'origine
                //Mais pour le moment, pas de condition à un deuxième niveau
                UcWKFSelectEtape uctl = new UcWKFSelectEtape(_ttsLesEtapes, beforeStep, StepAllReadyChoosed, dtgrdParametre, _workingTable, 
                    _LesGroupesValidation, false);
                uctl.Closing += uctl_Closing;
                uctl.Show();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void Modifier_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dtgrdParametre.SelectedItem != null)
                {
                    //Appel récursif de la même fenêtre
                    var objetselectionne = (CsRAffectationEtapeWorkflow)dtgrdParametre.SelectedItem;
                    if (null != objetselectionne)
                    {
                        IOrderedEnumerable<CsRAffectationEtapeWorkflow> dtGridRAff = dtgrdParametre.ItemsSource as IOrderedEnumerable<CsRAffectationEtapeWorkflow>;
                        CsRAffectationEtapeWorkflow lastStep = null;

                        if (null != dtGridRAff) lastStep = dtGridRAff.Where(aff => aff.ORDRE < objetselectionne.ORDRE)
                            .OrderBy(a => a.ORDRE)
                            .LastOrDefault();
                        CsEtape beforeStep = null;
                        if (null != lastStep) beforeStep = _ttsLesEtapes.Where(st => st.PK_ID == lastStep.FK_IDETAPE)
                            .FirstOrDefault();
                        List<CsEtape> IlFautQuandMemeAjouterLaMemeEtapePourQuellePuisseEtreSelecionneeDansLaFenetreDeConfiguration = new List<CsEtape>();
                        StepAllReadyChoosed.ForEach((CsEtape etap) =>
                        {
                            if (etap.PK_ID != objetselectionne.FK_IDETAPE)
                                IlFautQuandMemeAjouterLaMemeEtapePourQuellePuisseEtreSelecionneeDansLaFenetreDeConfiguration.Add(etap);
                        });

                        KeyValuePair<CsRAffectationEtapeWorkflow, CsConditionBranchement> leAffChoosed = new KeyValuePair<CsRAffectationEtapeWorkflow, CsConditionBranchement>(
                            objetselectionne, _RelationAffectationEtapeCondition[objetselectionne.PK_ID]);

                        //pour le moment, pas de condition à un deuxième niveau
                        UcWKFSelectEtape form = new UcWKFSelectEtape(_ttsLesEtapes, beforeStep, IlFautQuandMemeAjouterLaMemeEtapePourQuellePuisseEtreSelecionneeDansLaFenetreDeConfiguration, 
                            dtgrdParametre, _workingTable, _LesGroupesValidation, 
                            leAffChoosed, SessionObject.ExecMode.Modification, false);

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

        void uctl_Closing(object sender, CancelEventArgs e)
        {
            //On récupère l'etape créée
            KeyValuePair<CsRAffectationEtapeWorkflow, CsConditionBranchement> RAffAndCondition = ((UcWKFSelectEtape)sender)
                .Step;

            if (null != RAffAndCondition.Key)
            {
                //On teste d'abord l'existance
                if (null != SecondedonnesDatagrid.Where(c => c.PK_ID == RAffAndCondition.Key.PK_ID).FirstOrDefault() &&
                    _RelationAffectationEtapeCondition.Keys.Contains(RAffAndCondition.Key.PK_ID))
                {
                    //C'est la modification
                    CsRAffectationEtapeWorkflow lAncienAff = SecondedonnesDatagrid.Where(c => c.PK_ID == RAffAndCondition.Key.PK_ID).First();
                    SecondedonnesDatagrid.Remove(lAncienAff);
                    //On remet l'ordre de l'ancien
                    RAffAndCondition.Key.ORDRE = lAncienAff.ORDRE;
                    SecondedonnesDatagrid.Add(RAffAndCondition.Key);

                    //On supprime l'étape qui avait été choisi
                    var toDelete = _ttsLesEtapes.Where(et => et.PK_ID == lAncienAff.FK_IDETAPE).FirstOrDefault();
                    var toAdd = _ttsLesEtapes.Where(et => et.PK_ID == RAffAndCondition.Key.FK_IDETAPE).FirstOrDefault();
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
                else if (null == SecondedonnesDatagrid.Where(c => c.PK_ID == RAffAndCondition.Key.PK_ID).FirstOrDefault() &&
                    !_RelationAffectationEtapeCondition.Keys.Contains(RAffAndCondition.Key.PK_ID))
                {
                    RAffAndCondition.Key.ORDRE = SecondedonnesDatagrid.Count + 1;
                    SecondedonnesDatagrid.Add(RAffAndCondition.Key);
                    //On ajoute l'étape qui a été choisi
                    StepAllReadyChoosed.Add(_ttsLesEtapes.Where(et => et.PK_ID == RAffAndCondition.Key.FK_IDETAPE).FirstOrDefault());
                    //On ajoute la condition
                    RAffAndCondition.Key.CONDITION = (null != RAffAndCondition.Value) ? RAffAndCondition.Value.NOM
                        : string.Empty;

                    _RelationAffectationEtapeCondition.Add(RAffAndCondition.Key.PK_ID, RAffAndCondition.Value);
                }
            }

            if (UseCondition) dtgrdParametre.ItemsSource = SecondedonnesDatagrid.OrderBy(aff => aff.ORDRE);
        }
        

        #endregion


        private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            //Si la condition existe, on la supprime et on supprime tout le circuit de détournement 
            //à partir de cette étape si elle existe
            txtCondition.Text = string.Empty;
            TheCondition = null;
            SecondedonnesDatagrid = null;
            dtgrdParametre.ItemsSource = null;
        }

        private void BtnAjouterRejet_Click(object sender, RoutedEventArgs e)
        {
            if (null != cmbEtape.SelectedValue)
            {                                
                CsEtape __Etape = cmbEtape.SelectedItem as CsEtape;
                UcWKFSelectRenvoiRejet ucRenvoi = new UcWKFSelectRenvoiRejet(__Etape, _lesEtapesANePasChoisir);
                ucRenvoi.Closed += ucRenvoi_Closed;
                ucRenvoi.Show();
            }            
        }

        void ucRenvoi_Closed(object sender, EventArgs e)
        {
            if (null == LesRenvoisRejets) LesRenvoisRejets = new List<CsRenvoiRejet>();
            var ___renvoi = ((UcWKFSelectRenvoiRejet)sender).LeRenvoi;
            if (null != ___renvoi) LesRenvoisRejets.Add(___renvoi);

            List<InfoRenvoiRejet> yaKoiDansLaListe = new List<InfoRenvoiRejet>();
            LesRenvoisRejets.ForEach((CsRenvoiRejet rv) =>
            {
                yaKoiDansLaListe.Add(new InfoRenvoiRejet()
                {
                    LIBELLE = _ttsLesEtapes.FirstOrDefault(o => o.PK_ID == rv.FK_IDETAPE).NOM,
                    PK_ID = rv.FK_IDETAPE
                });
            });

            dtgrdParametre.ItemsSource = yaKoiDansLaListe;
        }

        private void BtnSuppRejet_Click(object sender, RoutedEventArgs e)
        {
            if (null != dtgrdParametre.SelectedItems && 1 == dtgrdParametre.SelectedItems.Count)
            {
                InfoRenvoiRejet info = dtgrdParametre.SelectedItem as InfoRenvoiRejet;
                if (null != info)
                {
                    LesRenvoisRejets.Remove(LesRenvoisRejets.FirstOrDefault(o => o.FK_IDETAPE == info.PK_ID));
                    
                    List<InfoRenvoiRejet> yaKoiDansLaListe = new List<InfoRenvoiRejet>();
                    LesRenvoisRejets.ForEach((CsRenvoiRejet rv) =>
                    {
                        yaKoiDansLaListe.Add(new InfoRenvoiRejet()
                        {
                            LIBELLE = _ttsLesEtapes.FirstOrDefault(o => o.PK_ID == rv.FK_IDETAPE).NOM,
                            PK_ID = rv.FK_IDETAPE
                        });
                    });

                    dtgrdParametre.ItemsSource = yaKoiDansLaListe;
                }
            }
        }
    }


    public class InfoRenvoiRejet
    {
        public string LIBELLE { get; set; }
        public int PK_ID { get; set; }
    }
}

