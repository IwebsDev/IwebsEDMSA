using Galatee.Silverlight.Resources.Parametrage;
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

namespace Galatee.Silverlight.Parametrage
{
    public partial class UcWKF : ChildWindow
    {

        CsWorkflow ObjetSelectionne;
        public CsWorkflow Workflow { get; set; }
        SessionObject.ExecMode _execMode = SessionObject.ExecMode.Creation; //Par défaut;
        List<CsTableDeTravail> lsTables;        
        ObservableCollection<CsWorkflow> donnesDatagrid = new ObservableCollection<CsWorkflow>();
        private DataGrid dataGrid = null;

        public UcWKF()
        {
            try
            {
                InitializeComponent();
                Translate();
                //Chargement des données par défaut pour la création
                GetDefaultData();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.FenetreOperation);
            }
        }

        public UcWKF(DataGrid pGrid)
        {
            dataGrid = pGrid;
            try
            {
                if (dataGrid != null) donnesDatagrid = dataGrid.ItemsSource as ObservableCollection<CsWorkflow>;
                InitializeComponent();
                Translate();
                //Chargement des données par défaut pour la création
                GetDefaultData();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.FenetreOperation);
            }
        }

        public UcWKF(CsWorkflow wkf, SessionObject.ExecMode execMode, DataGrid pGrid)
        {
            dataGrid = pGrid;
            try
            {
                if (dataGrid != null) donnesDatagrid = dataGrid.ItemsSource as ObservableCollection<CsWorkflow>;
                InitializeComponent();
                Translate();
                //Chargement des données par défaut pour la création
                GetDefaultData();
                Workflow = wkf;
                ObjetSelectionne = wkf;
                _execMode = execMode;

                ShowDetailsWKF();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.FenetreOperation);
            }
        }

        public UcWKF(CsWorkflow wkf, SessionObject.ExecMode execMode)
        {
            try
            {
                InitializeComponent();
                Translate();
                //Chargement des données par défaut pour la création
                GetDefaultData();
                Workflow = wkf;
                ObjetSelectionne = wkf;
                _execMode = execMode;

                ShowDetailsWKF();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.FenetreOperation);
            }
        }

        private void Translate()
        {
            try
            {
                //Title = Languages.FenetreOperation;
                OKButton.Content = Languages.OK;
                CancelButton.Content = Languages.Annuler;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void GetDefaultData()
        {
            try
            {
                int back = LoadingManager.BeginLoading("Chargement des données par défaut");
                ParametrageClient client = new ParametrageClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Parametrage"));
                client.SelectAllTableTravailCompleted += (ssender, args) =>
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
                    cmbTable.Items.Clear();
                    lsTables = new List<CsTableDeTravail>();
                    lsTables.Add(new CsTableDeTravail() { NOM = "Aucune", PK_ID = 0 });
                    if (args.Result != null)
                        foreach (var item in args.Result)
                        {
                            lsTables.Add(item);
                        }
                    cmbTable.DisplayMemberPath = "NOM";
                    cmbTable.SelectedValuePath = "PK_ID";
                    cmbTable.ItemsSource = lsTables;

                    LoadingManager.EndLoading(back);
                };
                client.SelectAllTableTravailAsync();                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void ShowDetailsWKF()
        {
            if (null != Workflow)
            {
                txtCode.Text = Workflow.CODE;
                txtNom.Text = Workflow.WORKFLOWNAME;
                txtDescription.Text = ObjetSelectionne.DESCRIPTION;                
                cmbTable.SelectedValue = ObjetSelectionne.FK_IDTABLE_TRAVAIL;
            }
            if (_execMode == SessionObject.ExecMode.Consultation)
            {
                txtCode.IsEnabled = false;
                txtNom.IsEnabled = false;
                txtDescription.IsEnabled = false;
                cmbTable.IsEnabled = false;
            }
        }


        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ParametrageClient client = new ParametrageClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Parametrage"));
                if (_execMode == SessionObject.ExecMode.Creation)
                {
                    ObjetSelectionne = new CsWorkflow()
                    {
                        PK_ID = Guid.NewGuid(),
                        CODE = txtCode.Text,
                        DESCRIPTION = txtDescription.Text,
                        WORKFLOWNAME = txtNom.Text,                        
                        FK_IDTABLE_TRAVAIL = (int.Parse(cmbTable.SelectedValue.ToString()) != 0) ?
                            int.Parse(cmbTable.SelectedValue.ToString()) : 0
                    };
                    List<CsWorkflow> toInsert = new List<CsWorkflow>() { ObjetSelectionne };
                    client.InsertWorkflowCompleted += (ssender, insertR) =>
                    {
                        if (insertR.Cancelled ||
                                insertR.Error != null)
                        {
                            Message.ShowError(insertR.Error.Message, Languages.FenetreOperation);
                            return;
                        }
                        if (!insertR.Result)
                        {
                            Message.ShowError(Languages.ErreurInsertionDonnees, Languages.FenetreOperation);
                            return;
                        }
                        DialogResult = true;
                        UpdateParentList(toInsert);
                    };
                    client.InsertWorkflowAsync(toInsert);
                }
                else if (_execMode == SessionObject.ExecMode.Modification)
                {
                    ObjetSelectionne.CODE = txtCode.Text;
                    ObjetSelectionne.DESCRIPTION = txtDescription.Text;
                    ObjetSelectionne.WORKFLOWNAME = txtNom.Text;                    
                    ObjetSelectionne.FK_IDTABLE_TRAVAIL = (int.Parse(cmbTable.SelectedValue.ToString()) != 0) ?
                        int.Parse(cmbTable.SelectedValue.ToString()) : 0;
                    List<CsWorkflow> toUpdate = new List<CsWorkflow>() { ObjetSelectionne };
                    client.UpdateWorkflowCompleted += (ssender, insertR) =>
                    {
                        if (insertR.Cancelled ||
                                insertR.Error != null)
                        {
                            Message.ShowError(insertR.Error.Message, Languages.FenetreOperation);
                            return;
                        }
                        if (!insertR.Result)
                        {
                            Message.ShowError(Languages.ErreurInsertionDonnees, Languages.FenetreOperation);
                            return;
                        }
                        DialogResult = true;
                        UpdateParentList(toUpdate);
                    };
                    client.UpdateWorkflowAsync(toUpdate);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void UpdateParentList(List<CsWorkflow> pListeObjet)
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
    }
}

