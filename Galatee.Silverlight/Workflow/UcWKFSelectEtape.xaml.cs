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
using Galatee.Silverlight.ServiceWorkflow;



namespace Galatee.Silverlight.Workflow
{
    public partial class UcWKFSelectEtape : ChildWindow
    {
        DemandeWorkflowInformation dmd;
        Galatee.Silverlight.ServiceAccueil.CsInfoDemandeWorkflow _infoDmd;
        List<CsCopieDmdCircuit> lstEtapes;
        ObservableCollection<CsCopieDmdCircuit> LesEtapesCircuits;
        CsCopieDmdCircuit copieCircuitSelectionne;
        List<Galatee.Silverlight.ServiceAccueil.CsRenvoiRejet> _renvois = null;
        public UcWKFSelectEtape(DemandeWorkflowInformation _dmd)
        {
            InitializeComponent();
            try
            {
                if (null != _dmd)
                {
                    dmd = _dmd;

                    Translate();
                    GetData();
                }

                prgBar.Visibility = System.Windows.Visibility.Collapsed;
                LblChargement.Visibility = System.Windows.Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, "Tableau de bord, Fenetre demande Liste");
            }
        }

        public UcWKFSelectEtape(DemandeWorkflowInformation _dmd, List<Galatee.Silverlight.ServiceAccueil.CsRenvoiRejet> possibiliteRenvoi)
        {
            InitializeComponent();
            try
            {
                if (null != _dmd)
                {
                    dmd = _dmd;
                    Translate();
                    GetData();
                }

                prgBar.Visibility = System.Windows.Visibility.Collapsed;
                LblChargement.Visibility = System.Windows.Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, "Tableau de bord, Fenetre demande Liste");
            }
        }

        public UcWKFSelectEtape(Galatee.Silverlight.ServiceAccueil.CsInfoDemandeWorkflow _dmd, List<Galatee.Silverlight.ServiceAccueil.CsRenvoiRejet> possibiliteRenvoi)
        {
            InitializeComponent();
            try
            {
                if (null != _dmd)
                {

                    _infoDmd = _dmd;

                    Translate();
                    GetData();
                }

                prgBar.Visibility = System.Windows.Visibility.Collapsed;
                LblChargement.Visibility = System.Windows.Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, "Tableau de bord, Fenetre demande Liste");
            }
        }


        #region Fonctions

        void Translate()
        {
            CancelButton.Content = Galatee.Silverlight.Resources.Parametrage.Languages.Annuler;
        }

        void GetData()
        {
            int back = LoadingManager.BeginLoading("Chargement de la liste ...");
            lstEtapes = new List<CsCopieDmdCircuit>();
            ParametrageClient client = new ParametrageClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Parametrage"));
            //ON recherche avant tout les demandes qui nous on été affectés, on n'affiche que celle-là,
            //car elle doivent être traitées avant les autres
            client.SelectEtapesFromDemandeCompleted += (__, ar) =>
            {
                LoadingManager.EndLoading(back);
                dtgrdParametre.ItemsSource = null;

                if (ar.Cancelled || ar.Error != null)
                {
                    string error = ar.Error.Message;
                    Message.Show(error, Languages.ListeCodePoste);
                    return;
                }
                if (ar.Result == null)
                {
                    Message.ShowError(Languages.msgErreurChargementDonnees, Languages.Parametrage);
                    return;
                }

                lstEtapes = ar.Result.OrderBy(o => o.ORDRE)
                    .ToList();
                LesEtapesCircuits = new ObservableCollection<CsCopieDmdCircuit>();

                if (null != _renvois)
                {
                    foreach (var o in lstEtapes)
                    {
                        if (null != _renvois.FirstOrDefault(r => r.FK_IDETAPEACTUELLE == o.FK_IDETAPE))
                            LesEtapesCircuits.Add(o);
                    }
                }
                else LesEtapesCircuits = new ObservableCollection<CsCopieDmdCircuit>(lstEtapes);

                dtgrdParametre.ItemsSource = LesEtapesCircuits;
            };
            client.SelectEtapesFromDemandeAsync(null != dmd.CODE ? dmd.CODE : _infoDmd.CODE);
        }

        #endregion


        public ObservableCollection<CsCopieDmdCircuit> DonnesDatagrid
        {
            get
            {
                LesEtapesCircuits = new ObservableCollection<CsCopieDmdCircuit>(lstEtapes);
                return new ObservableCollection<CsCopieDmdCircuit>(lstEtapes);
            }
            set
            {
                if (null != value)
                {
                    LesEtapesCircuits = new ObservableCollection<CsCopieDmdCircuit>(value);
                    lstEtapes = value.ToList();
                }
            }
        }


        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            //Affectation de la demande
            if (null != dtgrdParametre.SelectedItems && dtgrdParametre.SelectedItems.Count == 1 && string.Empty != TxtJustificatif.Text)
            {
                var etape = dtgrdParametre.SelectedItems[0] as CsCopieDmdCircuit;
                if (null != etape && null != dmd)
                {

                    prgBar.Visibility = System.Windows.Visibility.Visible;
                    LblChargement.Visibility = System.Windows.Visibility.Visible;

                    int _action = (dmd.ORDRE > etape.ORDRE) ? SessionObject.Enumere.REJETER : SessionObject.Enumere.TRANSMETTRE;

                    WorkflowClient client = new WorkflowClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Workflow"));
                    client.Endpoint.Binding.OpenTimeout = new TimeSpan(0, 1, 0);
                    client.Endpoint.Binding.CloseTimeout = new TimeSpan(5, 0, 0);
                    client.Endpoint.Binding.SendTimeout = new TimeSpan(5, 0, 0);

                    client.AllerALEtapeCompleted += (_sender, _args) =>
                    {
                        prgBar.Visibility = System.Windows.Visibility.Collapsed;
                        LblChargement.Visibility = System.Windows.Visibility.Collapsed;

                        if (_args.Cancelled || _args.Error != null)
                        {
                            string error = _args.Error.Message;
                            Message.Show(error, Languages.ListeCodePoste);
                            return;
                        }
                        if (_args.Result == null)
                        {
                            Message.ShowError(Languages.msgErreurChargementDonnees, Languages.Parametrage);
                            return;
                        }
                        if (_args.Result.ToString().StartsWith("ERR"))
                        {
                            Message.ShowError(_args.Result.ToString(), Languages.Parametrage);
                        }
                        else
                        {
                            Message.ShowInformation(_args.Result.ToString(), Languages.Parametrage);
                        }
                    };
                    client.AllerALEtapeAsync(null != dmd.CODE ? dmd.CODE : _infoDmd.CODE, _action, etape.PK_ID, UserConnecte.matricule, TxtJustificatif.Text);
                }
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void TxtJustificatif_TextChanged(object sender, TextChangedEventArgs e)
        {
            OKButton.IsEnabled = (null != dtgrdParametre.SelectedItems && dtgrdParametre.SelectedItems.Count == 1 && string.Empty != TxtJustificatif.Text);
        }


        #region Gestion du DataGrid

        private void dtgrdParametre_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            var etapeRow = e.Row.DataContext as CsCopieDmdCircuit;
            if (etapeRow != null)
            {
                if (etapeRow.ORDRE == dmd.ORDRE)
                {
                    SolidColorBrush SolidColorBrush = new SolidColorBrush(Colors.Green);
                    e.Row.Foreground = SolidColorBrush;
                    e.Row.FontWeight = FontWeights.Bold;
                }
            }
        }

        private void dtgrdParametre_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            OKButton.IsEnabled = (null != dtgrdParametre.SelectedItems && dtgrdParametre.SelectedItems.Count == 1 && string.Empty != TxtJustificatif.Text);
        }

        #endregion
    }
}

