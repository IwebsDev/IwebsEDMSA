using Galatee.Silverlight.Resources.Parametrage;
using Galatee.Silverlight.ServiceParametrage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Galatee.Silverlight.Parametrage
{
    public partial class UcWKFCondition : ChildWindow
    {

        CsEtape OrigineEtape;
        CsConditionBranchement _theCondition;
        CsRAffectationEtapeWorkflow _rAffDirectementBranchee;
        List<CsGroupeValidation> GroupeValidation;
        public CsConditionBranchement LaCondition
        {
            get { return _theCondition; }
            set { _theCondition = value; }
        }
        public CsRAffectationEtapeWorkflow FirstEtapeDetournement
        {
            get { return _rAffDirectementBranchee; }
            set { _rAffDirectementBranchee = value; }
        }
        List<CsEtape> _lsEtapes;
        CsTableDeTravail TableTravailConcernée;
        SessionObject.ExecMode _execMode = SessionObject.ExecMode.Creation;
        string[] _LesColonnes;
        string[] Operateurs = new List<string>() { "<", ">", "<>", "=", "<=", ">=" } .ToArray();
        bool _conditionAction = false;

        public UcWKFCondition(CsEtape _theStep, List<CsEtape> lsStep, CsTableDeTravail _table, List<CsGroupeValidation> grpValidation)
        {
            try
            {
                InitializeComponent();
                _lsEtapes = lsStep;
                TableTravailConcernée = _table;
                OrigineEtape = _theStep;
                GroupeValidation = grpValidation;
                SetData();
                
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, "Définition Condition Etape");
            }
        }

        public UcWKFCondition(CsEtape _theStep, List<CsEtape> lsStep, CsTableDeTravail _table, List<CsGroupeValidation> grpValidation,
            CsConditionBranchement lCondition, SessionObject.ExecMode execMode, CsRAffectationEtapeWorkflow DirectEtapeBranchement)
        {
            try
            {
                InitializeComponent();
                _lsEtapes = lsStep;
                TableTravailConcernée = _table;
                OrigineEtape = _theStep;
                GroupeValidation = grpValidation;
                _theCondition = lCondition;
                _rAffDirectementBranchee = DirectEtapeBranchement;
                _execMode = execMode;

                SetData();
                ShowDetail();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, "Définition Condition Etape");
            }
        }

        public UcWKFCondition(CsEtape _theStep, CsTableDeTravail _table, List<CsGroupeValidation> grpValidation, bool ForAction)
        {
            try
            {
                InitializeComponent();                
                TableTravailConcernée = _table;
                OrigineEtape = _theStep;
                GroupeValidation = grpValidation;
                _conditionAction = ForAction;
                SetData();                
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, "Définition Condition Etape");
            }
        }

        public UcWKFCondition(CsEtape _theStep, CsTableDeTravail _table, List<CsGroupeValidation> grpValidation, bool ForAction,
            CsConditionBranchement lCondition)
        {
            try
            {
                InitializeComponent();
                TableTravailConcernée = _table;
                OrigineEtape = _theStep;
                GroupeValidation = grpValidation;
                _conditionAction = ForAction;
                SetData();
                ShowDetail();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, "Définition Condition Etape");
            }
        }


        #region Fonctions

        private void SetData()
        {
            if (null != _lsEtapes)
            {
                cmbEtapeConditionVrai.DisplayMemberPath = "NOM";
                cmbEtapeConditionVrai.SelectedValuePath = "PK_ID";
                cmbEtapeConditionVrai.ItemsSource = _lsEtapes;

                cmbGroupeValidation.DisplayMemberPath = "GROUPENAME";
                cmbGroupeValidation.SelectedValuePath = "PK_ID";
                cmbGroupeValidation.ItemsSource = GroupeValidation;                
            }

            if (_conditionAction)
            {
                //Définition d'une condition d'action

                Title = "Définition d'une condition d'action";
                cmbGroupeValidation.Visibility = System.Windows.Visibility.Collapsed;
                cmbEtapeConditionVrai.Visibility = System.Windows.Visibility.Collapsed;
                lblEtape.Visibility = System.Windows.Visibility.Collapsed;
                lblGroupe.Visibility = System.Windows.Visibility.Collapsed;
            }

            cmbGroupeValidation.Visibility = System.Windows.Visibility.Collapsed;
            cmbOperateur.Items.Clear();
            foreach (var str in Operateurs)
            {
                cmbOperateur.Items.Add(str);
            }
            
            //Affichage des colonnes
            int back = LoadingManager.BeginLoading("Chargement des données ...");
            ParametrageClient client = new ParametrageClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Parametrage"));
            client.GetColumnsOfWorkingTableCompleted += (clmnsender, args) =>
                {
                    if (args.Cancelled || args.Error != null)
                    {
                        string error = args.Error.Message;
                        Message.Show(error, "Définition Condition");
                        return;
                    }
                    if (args.Result == null)
                    {
                        Message.ShowError(Languages.msgErreurChargementDonnees, Languages.Parametrage);
                        return;
                    }

                    cmbColonne.Items.Clear();
                    if (args.Result != null) _LesColonnes = args.Result.ToArray();

                    _LesColonnes.ToList().ForEach((string str) =>
                    {
                        cmbColonne.Items.Add(str);
                    });

                    LoadingManager.EndLoading(back);
                };
            client.GetColumnsOfWorkingTableAsync((null != TableTravailConcernée) ? TableTravailConcernée.TABLE_NAME : string.Empty);
        }

        private void ShowDetail()
        {
            if (!_conditionAction)
            {
                if (null != _theCondition)
                {
                    if (string.Empty != _theCondition.OPERATEUR) cmbColonne.SelectedIndex = _LesColonnes.ToList().IndexOf(_theCondition.OPERATEUR);
                    txtValeur.Text = _theCondition.VALUE;

                    if (_theCondition.FK_IDETAPEVRAIE.HasValue && _theCondition.FK_IDETAPEVRAIE.Value != 0)
                    {
                        cmbEtapeConditionVrai.SelectedValue = _theCondition.FK_IDETAPEVRAIE.Value;
                    }
                }
                if (null != _rAffDirectementBranchee)
                {
                    if (Guid.Empty != _rAffDirectementBranchee.FK_IDGROUPEVALIDATIOIN) cmbGroupeValidation.SelectedValue =
                        _rAffDirectementBranchee.FK_IDGROUPEVALIDATIOIN;
                }
            }
            else if (_conditionAction && null != _theCondition)
            {
                if (string.Empty != _theCondition.OPERATEUR) cmbColonne.SelectedIndex = _LesColonnes.ToList().IndexOf(_theCondition.OPERATEUR);
                txtValeur.Text = _theCondition.VALUE;
            }
        }

        #endregion

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (_execMode == SessionObject.ExecMode.Creation)
            {
                //Création de la condition
                if (!_conditionAction)
                {
                    _theCondition = new CsConditionBranchement()
                    {
                        PK_ID = Guid.NewGuid(),
                        COLONNENAME = _LesColonnes.ToList()[cmbColonne.SelectedIndex],
                        FK_IDETAPEVRAIE = int.Parse(cmbEtapeConditionVrai.SelectedValue.ToString()),
                        FK_IDTABLETRAVAIL = (null != TableTravailConcernée) ? TableTravailConcernée.PK_ID : 0,
                        NOM = _LesColonnes.ToList()[cmbColonne.SelectedIndex] + " " + Operateurs.ToList()[cmbOperateur.SelectedIndex]
                            + " " + txtValeur.Text,
                        OPERATEUR = Operateurs.ToList()[cmbOperateur.SelectedIndex],
                        VALUE = txtValeur.Text,
                        FK_IDETAPEFAUSE = null,    
                        PEUT_TRANSMETTRE_SI_FAUX = true,
                    };
                    _rAffDirectementBranchee = new CsRAffectationEtapeWorkflow()
                    {
                        PK_ID = Guid.NewGuid(),
                        FK_IDETAPE = _theCondition.FK_IDETAPEVRAIE.Value,
                        FK_IDGROUPEVALIDATIOIN = Guid.Empty,
                        ORDRE = 1,
                        FROMCONDITION = true,
                        CONDITION = _theCondition.NOM,
                        LIBELLEETAPE = _lsEtapes.Where(et => et.PK_ID == _theCondition.FK_IDETAPEVRAIE).First()
                            .NOM
                    };
                }
                else
                {
                    _theCondition = new CsConditionBranchement()
                    {
                        PK_ID = Guid.NewGuid(),
                        COLONNENAME = _LesColonnes.ToList()[cmbColonne.SelectedIndex],                        
                        FK_IDTABLETRAVAIL = (null != TableTravailConcernée) ? TableTravailConcernée.PK_ID : 0,
                        NOM = _LesColonnes.ToList()[cmbColonne.SelectedIndex] + " " + Operateurs.ToList()[cmbOperateur.SelectedIndex]
                            + " " + txtValeur.Text,
                        OPERATEUR = Operateurs.ToList()[cmbOperateur.SelectedIndex],
                        VALUE = txtValeur.Text,
                        FK_IDETAPEFAUSE = null,
                        PEUT_TRANSMETTRE_SI_FAUX = false
                    };
                }
                this.DialogResult = true;
            }
            else if (_execMode == SessionObject.ExecMode.Modification)
            {
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

    }
}

