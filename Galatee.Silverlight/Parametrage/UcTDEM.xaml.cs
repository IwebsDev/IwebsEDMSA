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
//using Galatee.Silverlight.serviceWeb;
using System.ComponentModel;
using System.Collections.ObjectModel;
using Galatee.Silverlight.ServiceParametrage;

namespace Galatee.Silverlight.Parametrage
{
    public partial class UcTDEM : ChildWindow, INotifyPropertyChanged
    {
        public UcTDEM()
        {
            InitializeComponent();
        }

        public UcTDEM(int numero, string text, string TableNom)
        {
            InitializeComponent();
            num = numero;
            this.Title = text;
            this.TableName = TableNom;

            this.GetData(numero);
        }

        int num;
        int rowIndex = -1;
        int rankSelected;
        int atteintchange = 0;

        #region INotifyPropertyChanged Membres

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string nompropriete)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(nompropriete));
        }
        #endregion

        ObservableCollection<CsTypeDemande> donnesDatagrid = new ObservableCollection<CsTypeDemande>();

        public ObservableCollection<CsTypeDemande> DonnesDatagrid
        {
            get { return donnesDatagrid; }
            set
            {
                if (value == donnesDatagrid)
                    return;
                donnesDatagrid = value;
                // NotifyPropertyChanged("donnesDatagrid");
                NotifyPropertyChanged("DonnesDatagrid");
            }
        }


        private string TableName;
        private string LibelleEtat = string.Empty;

        List<CsProduit> rowcomboselectedObject = new List<CsProduit>();
        List<DateTime?> rowselectDate = new List<DateTime?>();
        List<CsTypeDemande> majData = new List<CsTypeDemande>();
        List<CsTa> listeTEvents = null;
        List<bool> columnAmount = new List<bool>();

          /// <summary>
          /// PERMET DE VALORISER LES DATAGRID ET   
          /// LES COMBOBOX AU CHARGEMENT DE LA PAGE
          /// </summary>
          /// <param name="datagrid"></param>
          /// <param name="services"></param>

        void cbo_Observation_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                //if (loadOnlyOne <= dataTableCombo.Count)
                //{
                //    var src = sender as ComboBox;
                //    if (src != null)
                //    {
                //        var data = src.DataContext as aCampagne;
                //        switch (src.Name)
                //        {
                //            case "cbo_Observation":
                //                src.ItemsSource = dataTableCombo;
                //                src.SelectedValue = "Code";
                //                src.DisplayMemberPath = "Libelle";
                //                loadOnlyOne++;
                //                break;
                //        }
                //    }
                //}
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }

        }

        void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        void GetData(int numtable)
        {
            try
            {
                //DonnesDatagrid = null;
                //majData.Clear();

                ParametrageClient client = new ParametrageClient(Utility.Protocole(), Utility.EndPoint("Parametrage"));
                client.SelectAllTypeDemandeCompleted += (ssender, args) =>
                {
                    if (args.Cancelled || args.Error != null)
                    {
                        string error = args.Error.Message;
                        MessageBox.Show(error, ".SelectAllTypeDemande", MessageBoxButton.OK);
                        desableProgressBar();
                        this.DialogResult = true;
                        return;
                    }

                    if (args.Result == null || args.Result.Count == 0)
                    {
                        MessageBox.Show("No data found ", ".SelectAllTypeDemande", MessageBoxButton.OK);
                        desableProgressBar();
                        this.DialogResult = true;
                        return;
                    }

                    ParametrageClient proxy = new ParametrageClient(Utility.Protocole(), Utility.EndPoint("Parametrage"));
                    proxy.SELECTCodeLibelleByNUMCompleted += (senders, results) =>
                        {
                            if (results.Cancelled || results.Error != null)
                            {
                                string error = results.Error.Message;
                                MessageBox.Show(error, ".SELECTCodeLibelleByNUM", MessageBoxButton.OK);
                                desableProgressBar();
                                this.DialogResult = true;
                                return;
                            }

                            if (results.Result == null || results.Result.Count == 0)
                            {
                                MessageBox.Show("No data found ", ".SELECTCodeLibelleByNUM", MessageBoxButton.OK);
                                desableProgressBar();
                                this.DialogResult = true;
                                return;
                            }

                            listeTEvents = new List<CsTa>();
                            listeTEvents.AddRange(results.Result);

                            InitialiserCombobox(listeTEvents);
                            List<CsTypeDemande> listeCas = new List<CsTypeDemande>();
                            listeCas.AddRange(args.Result);
                            foreach (CsTypeDemande cas in listeCas)
                                DonnesDatagrid.Add(cas);
                            // ajout de la derniere liste pour la sais de nouvelle donnes

                            AddNewDataInDataGrid();
                            DataContext = this;
                        };
                    proxy.SELECTCodeLibelleByNUMAsync(int.Parse(SessionObject.Enumere.EvenementsValeurParDefaut));
                    
                };
                client.SelectAllTypeDemandeAsync();
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }


        }

        void InitialiserCombobox(List<CsTa> liste)
        {
            cbo_evnt1.ItemsSource = cbo_evnt2.ItemsSource = 
            cbo_evnt3.ItemsSource = cbo_evnt4.ItemsSource = cbo_evnt5.ItemsSource = liste;
        }

        void AddNewDataInDataGrid()
        {
            DonnesDatagrid.Add(new CsTypeDemande() { DMAJ = DateTime.Now });
        }

        void desableProgressBar()
        {
            //progressBar1.IsIndeterminate = false;
            //progressBar1.Visibility = Visibility.Collapsed;
        }

        void allowProgressBar()
        {
            //progressBar1.IsEnabled = true;
            //progressBar1.Visibility = Visibility.Visible;
            //progressBar1.IsIndeterminate = true;
        }

        void ValiderSaisieDatagrid()
        {
            dgINIT.UpdateLayout();

        }

        void InitialiserDonneeDataGrid(List<CsTypeDemande> listecas)
        {
            try
            {
                while (DonnesDatagrid.Count > 1)
                {
                    if (!string.IsNullOrEmpty(DonnesDatagrid[DonnesDatagrid.Count - 1].TDEM) &&
                       !string.IsNullOrEmpty(DonnesDatagrid[DonnesDatagrid.Count - 1].CENTRE))
                        DonnesDatagrid.RemoveAt(DonnesDatagrid.Count - 1);
                    else
                        DonnesDatagrid.RemoveAt(DonnesDatagrid.Count - 2);
                }

                foreach (CsTypeDemande cas in listecas)
                     DonnesDatagrid.Add(cas);

                CsTypeDemande casnull = DonnesDatagrid.FirstOrDefault(p => string.IsNullOrEmpty(p.CENTRE) && string.IsNullOrEmpty(p.TDEM));
                DonnesDatagrid.Remove(casnull);
                DonnesDatagrid.Add(new CsTypeDemande() { DMAJ = DateTime.Now });

                majData.Clear();
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            ValiderSaisieDatagrid();
            List<CsTypeDemande> liste = new List<CsTypeDemande>();

            foreach (CsTypeDemande cas in DonnesDatagrid)
                liste.Add(cas);

            ParametrageClient insert = new ParametrageClient(Utility.Protocole(), Utility.EndPoint("Parametrage"));
            insert.insertOrUpdateTypeDemandeCompleted += (snder, insertR) =>
            {
                if (insertR.Cancelled || insertR.Error != null)
                {
                    string error = insertR.Error.Message;
                    MessageBox.Show(error, "insertOrUpdateTypeDemande", MessageBoxButton.OK);
                    desableProgressBar();
                    return;
                }

                if (insertR.Result == null || insertR.Result.Count == 0)
                {
                    MessageBox.Show("Error on insert ", "insertOrUpdateTypeDemande", MessageBoxButton.OK);
                    desableProgressBar();
                    return;
                }

                InitialiserDonneeDataGrid(insertR.Result);

            };
            insert.insertOrUpdateTypeDemandeAsync(liste, majData);
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Voulez vous vraiment supprimer cet element", "Confirmation", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                CsTypeDemande selected = dgINIT.SelectedItem as CsTypeDemande;
                if (selected != null)
                {
                    ParametrageClient delete = new ParametrageClient(Utility.Protocole(), Utility.EndPoint("Parametrage"));
                    delete.Delete_TYPEDEMANDECompleted += (del, argDel) =>
                    {
                        if (argDel.Cancelled || argDel.Error != null)
                        {
                            string error = argDel.Error.Message;
                            MessageBox.Show(error, "Delete_TYPEDEMANDE", MessageBoxButton.OK);
                            desableProgressBar();
                            return;
                        }

                        if (argDel.Result == false)
                        {
                            MessageBox.Show("Error on insert/update ", "Delete_TYPEDEMANDE", MessageBoxButton.OK);
                            desableProgressBar();
                            return;
                        }

                        DonnesDatagrid.Remove(selected);
                    };
                    delete.Delete_TYPEDEMANDEAsync(selected.CENTRE, selected.TDEM);
                }
                else
                    MessageBox.Show("vous devez selectionner un item dans la liste ");
            }
        }

        void AjouterListeMaj(CsTypeDemande casSelected)
        {
            try
            {
                if (majData.Count > 0 && majData != null)
                {
                    if (majData.First(p => p.ROWID == casSelected.ROWID) != null)
                    {
                        majData.Remove(casSelected);
                        majData.Add(casSelected);
                    }
                    else
                        majData.Add(casSelected);
                }
                else
                    majData.Add(casSelected);
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
        }

        private void cbo_evnt1_DropDownClosed(object sender, EventArgs e)
        {
            if (dgINIT.SelectedItem == null || cbo_evnt1.SelectedItem == null)
                return;
            CsTa index = new CsTa();
            index = cbo_evnt1.SelectedItem as CsTa;
            CsTypeDemande casSelected = dgINIT.SelectedItem as CsTypeDemande;
            //casSelected.EVT1 = index.PK_CODE;

            AjouterListeMaj(casSelected);
        }

        private void cbo_evnt2_DropDownClosed(object sender, EventArgs e)
        {
            if (dgINIT.SelectedItem == null || cbo_evnt2.SelectedItem == null)
                return;
            CsTa index = new CsTa();
            index = cbo_evnt2.SelectedItem as CsTa;
            CsTypeDemande casSelected = dgINIT.SelectedItem as CsTypeDemande;
            //casSelected.EVT2 = index.PK_CODE;

            AjouterListeMaj(casSelected);
        }

        private void cbo_evnt3_DropDownClosed(object sender, EventArgs e)
        {
            if (dgINIT.SelectedItem == null || cbo_evnt3.SelectedItem == null)
                return;
            CsTa index = new CsTa();
            index = cbo_evnt3.SelectedItem as CsTa;
            CsTypeDemande casSelected = dgINIT.SelectedItem as CsTypeDemande;
            //casSelected.EVT3 = index.PK_CODE;

            AjouterListeMaj(casSelected);
        }

        private void cbo_evnt4_DropDownClosed(object sender, EventArgs e)
        {
            if (dgINIT.SelectedItem == null || cbo_evnt4.SelectedItem == null)
                return;
            CsTa index = new CsTa();
            index = cbo_evnt4.SelectedItem as CsTa;
            CsTypeDemande casSelected = dgINIT.SelectedItem as CsTypeDemande;
            //casSelected.EVT4 = index.PK_CODE;

            AjouterListeMaj(casSelected);
        }

        private void cbo_evnt5_DropDownClosed(object sender, EventArgs e)
        {
            if (dgINIT.SelectedItem == null || cbo_evnt5.SelectedItem == null)
                return;
            CsTa index = new CsTa();
            index = cbo_evnt5.SelectedItem as CsTa;
            CsTypeDemande casSelected = dgINIT.SelectedItem as CsTypeDemande;
            //casSelected.EVT5 = index.PK_CODE;

            AjouterListeMaj(casSelected);
        }

        private void dgINIT_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CsTypeDemande demande = dgINIT.SelectedItem as CsTypeDemande;
            //CsTa event1 = listeTEvents.FirstOrDefault(p => p.PK_CODE == demande.EVT1);
            //CsTa event2 = listeTEvents.FirstOrDefault(p => p.PK_CODE == demande.EVT2);
            //CsTa event3 = listeTEvents.FirstOrDefault(p => p.PK_CODE == demande.EVT3);
            //CsTa event4 = listeTEvents.FirstOrDefault(p => p.PK_CODE == demande.EVT4);
            //CsTa event5 = listeTEvents.FirstOrDefault(p => p.PK_CODE == demande.EVT5);

            //cbo_evnt1.SelectedItem = event1;
            //cbo_evnt2.SelectedItem = event2;
            //cbo_evnt3.SelectedItem = event3;
            //cbo_evnt4.SelectedItem = event4;
            //cbo_evnt5.SelectedItem = event5;

        }

        private void dgINIT_CellEditEnded(object sender, DataGridCellEditEndedEventArgs e)
        {
            CsTypeDemande u = e.Row.DataContext as CsTypeDemande; //fetch the row data

            try
            {
                if (majData.Count > 0 && majData != null)
                {
                    if (majData.First(p => p.ROWID == u.ROWID) != null)
                    {
                        majData.Remove(u);
                        majData.Add(u);
                    }
                    else
                        majData.Add(u);
                }
                else
                    majData.Add(u);
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
        }

    }
}


