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
using Galatee.Silverlight.ServiceAccueil ;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Galatee.Silverlight.Resources.Devis;

namespace Galatee.Silverlight.Devis
{
    public partial class UcMaterielsConsomme : ChildWindow
    {
        private CsDemande mydevis;
        private bool _selection = false;
        public List<ObjELEMENTDEVIS> Elements { get; set; }
        ObservableCollection<ObjELEMENTDEVIS> donnesDatagrid = new ObservableCollection<ObjELEMENTDEVIS>();

        public UcMaterielsConsomme()
        {
            InitializeComponent();
        }

        public ObservableCollection<ObjELEMENTDEVIS> DonnesDatagrid
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
        #region INotifyPropertyChanged Membres

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string nompropriete)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(nompropriete));
        }
        #endregion
        public UcMaterielsConsomme(CsDemande pDevis)
        {
            InitializeComponent();
            mydevis = pDevis;
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var rowCount = dataGridMaterielConsomme.ItemsSource != null ? dataGridMaterielConsomme.ItemsSource.OfType<object>().Count() : 0;
                if (rowCount == 0)
                    throw new Exception(Languages.msgEmptyFournitures);

                 List<ObjELEMENTDEVIS> _elements = new List<ObjELEMENTDEVIS>();
                foreach (ObjELEMENTDEVIS item in dataGridMaterielConsomme.ItemsSource)
                {
                    item.USERMODIFICATION = UserConnecte.matricule;
                    item.DATEMODIFICATION = DateTime.Now;
                    item.QUANTITECONSOMMEE = (item.QUANTITECONSOMMEE != null && item.QUANTITECONSOMMEE.ToString() != string.Empty && (int)item.QUANTITECONSOMMEE > 0 && item.CONSOMME > 0) ? (int)item.QUANTITECONSOMMEE : 0;
                    _elements.Add(item);
                }
                if (_elements.Count == 0)
                    throw new Exception(Languages.msgEmptyRemisStock);

                var messageBox = new MessageBoxControl.MessageBoxChildWindow(Languages.lblGestionDeDevis, "Confirmer vous la consommation renseignée ?", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                messageBox.OnMessageBoxClosed += (_, result) =>
                {
                    if (messageBox.Result == MessageBoxResult.OK)
                    {
                        this.Elements = _elements;
                        this.DialogResult = true;
                    }
                    else
                    {
                        return;
                    }
                };
                messageBox.Show();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.lblGestionDeDevis);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {

                LayoutRoot.Cursor = Cursors.Wait;
                //foreach (var item in mydevis.EltDevis.Where(t => t.CODECOPER == SessionObject.Enumere.CoperTRV))
                foreach (var item in mydevis.EltDevis.Where(t => t.FK_IDFOURNITURE!=null))
                    DonnesDatagrid.Add(item);

                this.dataGridMaterielConsomme.ItemsSource = DonnesDatagrid;
                LayoutRoot.Cursor = Cursors.Arrow;
                        
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Galatee.Silverlight.Resources.Devis.Languages.txtDevis);
            }
        }

        private void BtnSelectionTotal_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (this._selection)
                {
                    this._selection = false; this.BtnSelectionTotal.Content = "Sélection totale";
                    foreach (ObjELEMENTDEVIS item in dataGridMaterielConsomme.ItemsSource)
                    {
                        item.CONSOMME = 0;
                        item.QUANTITECONSOMMEE = 0;
                    }
                }
                else
                {
                    this._selection = true; this.BtnSelectionTotal.Content = "Désélection totale";
                    foreach (ObjELEMENTDEVIS item in dataGridMaterielConsomme.ItemsSource)
                    {
                        item.CONSOMME = 1;
                        item.QUANTITECONSOMMEE = item.QUANTITE;
                    }
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.txtDevis);
            }
        }

        private void dataGridMaterielConsomme_CellEditEnded(object sender, DataGridCellEditEndedEventArgs e)
        {
            int? retour = 0;
            try
            {
                if (e.Column.DisplayIndex == this.dataGridMaterielConsomme.Columns[0].DisplayIndex)
                {
                    if (!Convert.ToBoolean(((TextBlock)this.dataGridMaterielConsomme.Columns[0].GetCellContent(dataGridMaterielConsomme.SelectedItem)).Text))
                        ((TextBlock)this.dataGridMaterielConsomme.Columns[0].GetCellContent(dataGridMaterielConsomme.SelectedItem)).Text = "0";
                }
                if (e.Column.DisplayIndex == this.dataGridMaterielConsomme.Columns[3].DisplayIndex)
                {
                    if (dataGridMaterielConsomme.SelectedItem != null)
                    {
                        ObjELEMENTDEVIS selected = (ObjELEMENTDEVIS)dataGridMaterielConsomme.SelectedItem;
                        if (selected != null /*&& Convert.ToBoolean(selected.CONSOMME)*/)
                        {
                            retour = selected.QUANTITECONSOMMEE != null ? (int?)selected.QUANTITECONSOMMEE : 0;
                            if (retour != null && retour > selected.QUANTITE)
                            {
                                ((ObjELEMENTDEVIS)dataGridMaterielConsomme.SelectedItem).QUANTITECONSOMMEE = 0;
                                dataGridMaterielConsomme.UpdateLayout();
                                throw new Exception(Languages.msgMauvaiseQuantiteRetour);
                            }
                            else
                            {
                                dataGridMaterielConsomme.UpdateLayout();
                                ((ObjELEMENTDEVIS)dataGridMaterielConsomme.SelectedItem).CONSOMME = 1;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.txtDevis);
            }
        }

        private void dataGridMaterielConsomme_PreparingCellForEdit(object sender, DataGridPreparingCellForEditEventArgs e)
        {
            try
            {
                if (e.Column.DisplayIndex == this.dataGridMaterielConsomme.Columns[3].DisplayIndex)
                {
                    //todo Mettre en lecture seule la ligne sélectionnée
                    //if (Convert.ToBoolean( ((TextBox)this.dataGridElementDevis.Columns[0].GetCellContent(dataGridElementDevis.SelectedItem)).Text))
                    //{
                    //    this.dataGridElementDevis.Rows[e.RowIndex].Cells[this.ARetourner.Name].ReadOnly = false;
                    //    Point pt = this.dataGridElementDevis.CurrentCellAddress;
                    //    if (pt.X == e.ColumnIndex   && 
                    //        pt.Y == e.RowIndex      && 
                    //        e.Button == MouseButtons.Left &&
                    //        dataGridElementDevis.EditMode != DataGridViewEditMode.EditProgrammatically)
                    //        dataGridElementDevis.BeginEdit(true);
                    //}
                    //else
                    //    this.dataGridElementDevis.Rows[e.RowIndex].Cells[this.ARetourner.Name].ReadOnly = true;
                }
            }
            catch (Exception ex)
            {
                Message.Show(ex.Message, Languages.txtDevis);
            }
        }

        private void dataGridMaterielConsomme_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (dataGridMaterielConsomme.SelectedItem != null)
                {
                    ObjELEMENTDEVIS selected = (ObjELEMENTDEVIS)dataGridMaterielConsomme.SelectedItem;
                    if (selected != null && Convert.ToBoolean(selected.CONSOMME))
                    {

                    }
                }
            }
            catch (Exception ex)
            {
                Message.Show(ex.Message, Languages.txtDevis);
            }
        }
       
    }
}

