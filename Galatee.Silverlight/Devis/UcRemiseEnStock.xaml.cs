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
using Galatee.Silverlight.Resources.Devis;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace Galatee.Silverlight.Devis
{
    public partial class UcRemiseEnStock : ChildWindow, INotifyPropertyChanged
    {
        public CsDemande  DevisSelectionne { get; set; }
        public List<ObjELEMENTDEVIS> listElementDevis { get; set; }
        ObservableCollection<ObjELEMENTDEVIS> donnesDatagrid = new ObservableCollection<ObjELEMENTDEVIS>();
        public List<ObjELEMENTDEVIS> Elements { get; set; }
        private bool _selection = false;

        public UcRemiseEnStock()
        {
            InitializeComponent();
        }

        public UcRemiseEnStock(CsDemande pDevis)
        {
            try
            {
                InitializeComponent();
                DevisSelectionne = pDevis;
            }
            catch (Exception ex)
            {
                Message.Show(ex.Message, Languages.txtDevis);
            }
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

        private void RemplirListeElementDevis(CsDemande pDevis)
        {
            try
            {
                LayoutRoot.Cursor = Cursors.Wait;
             foreach (var item in pDevis.EltDevis.Where(t=>t.CODECOPER== SessionObject.Enumere.CoperTRV  ))
	            DonnesDatagrid.Add(item);
                                
            this.dataGridElementDevis.ItemsSource = DonnesDatagrid;
            LayoutRoot.Cursor = Cursors.Arrow;
             
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //private void RemplirListeElementDevis(CsDemandeBase  pDevis)
        //{
        //    try
        //    {
        //        LayoutRoot.Cursor = Cursors.Wait;
        //        List<ObjDEVIS> laliste = new List<ObjDEVIS>();
        //        var _client = new AccesServiceWCF().GetDevisClient();

        //        _client.SelectElementsDevisByDevisIdCompleted += (send, result) =>
        //        {
        //            if (result.Cancelled || result.Error != null)
        //            {
        //                LayoutRoot.Cursor = Cursors.Arrow;
        //                string error = result.Error.Message;
        //                Message.Show(error,
        //                                Galatee.Silverlight.Resources.
        //                                    Devis.Languages.txtDevis);
        //                return;
        //            }
        //            if (result.Result == null)
        //            {
        //                LayoutRoot.Cursor = Cursors.Arrow;
        //                Message.Show(Galatee.Silverlight.Resources.Devis.Languages.msgErreurChargementDonnees, Galatee.Silverlight.Resources.Devis.Languages.txtDevis);
        //                return;
        //            }
        //            if (result.Result != null && result.Result.Count > 0)
        //            {
        //                if (result.Result.Where(t=>t.ISDEFAULT == true ) != null)
        //                    foreach (var item in result.Result.Where(t => t.ISDEFAULT == true))
        //                    {
        //                        DonnesDatagrid.Add(item);
        //                    }
        //                this.dataGridElementDevis.ItemsSource = DonnesDatagrid;
        //                LayoutRoot.Cursor = Cursors.Arrow;
        //            }
        //            else
        //            {
        //                LayoutRoot.Cursor = Cursors.Arrow;
        //                return;
        //            }
        //        };
        //        _client.SelectElementsDevisByDevisIdAsync(pDevis.PK_ID, int.Parse(pDevis.ORDRE) , false);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                var rowCount = dataGridElementDevis.ItemsSource != null ? dataGridElementDevis.ItemsSource.OfType<object>().Count() : 0;
                if (rowCount == 0)
                    throw new Exception(Languages.msgEmptyFournitures);

                //int quantite = 0; ObjELEMENTDEVIS elt; 
                List<ObjELEMENTDEVIS> _elements = new List<ObjELEMENTDEVIS>();

                foreach (ObjELEMENTDEVIS item in dataGridElementDevis.ItemsSource)
                {
                    item.QUANTITECONSOMMEE = item.QUANTITE - (item.QUANTITEREMISENSTOCK == null ? 0 : item.QUANTITEREMISENSTOCK);
                    item.DATEMODIFICATION = DateTime.Now;
                    item.USERCREATION = UserConnecte.matricule;
                    _elements.Add(item);
                }

                if (_elements.Count == 0)
                    throw new Exception(Languages.msgEmptyRemisStock);

                var messageBox = new MessageBoxControl.MessageBoxChildWindow(Languages.lblGestionDeDevis, Languages.msgConfirmRemisStock, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
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
                Message.Show(ex.Message, Languages.txtDevis);
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
                if(DevisSelectionne != null)
                    RemplirListeElementDevis(DevisSelectionne);
            }
            catch (Exception ex)
            {
                Message.Show(ex.Message, Languages.txtDevis);
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

        private void dataGridElementDevis_CellEditEnded(object sender, DataGridCellEditEndedEventArgs e)
        {
            try
            {
                dataGridElementDevis.UpdateLayout();

                if (e.Column.DisplayIndex == this.dataGridElementDevis.Columns[0].DisplayIndex)
                {
                    if (!Convert.ToBoolean( ((TextBlock)this.dataGridElementDevis.Columns[0].GetCellContent(dataGridElementDevis.SelectedItem)).Text))
                        ((TextBlock)this.dataGridElementDevis.Columns[0].GetCellContent(dataGridElementDevis.SelectedItem)).Text = "0";
                }

                if (e.Column.DisplayIndex == this.dataGridElementDevis.Columns[3].DisplayIndex)
                {
                    if (dataGridElementDevis.SelectedItem != null)
                    {
                        int? retour = 0;
                        ObjELEMENTDEVIS selected = (ObjELEMENTDEVIS)dataGridElementDevis.SelectedItem;
                        if(selected != null)
                        {
                            retour = selected.QUANTITEREMISENSTOCK != null ? (int?)selected.QUANTITEREMISENSTOCK : 0;
                            if (retour != null && retour > selected.QUANTITE)
                            {
                                this.dataGridElementDevis.UpdateLayout();
                                ((ObjELEMENTDEVIS)dataGridElementDevis.SelectedItem).QUANTITEREMISENSTOCK = 0;
                                throw new Exception(Languages.msgMauvaiseQuantiteRetour);
                            }
                            else
                            {
                                dataGridElementDevis.UpdateLayout();
                                ((ObjELEMENTDEVIS)dataGridElementDevis.SelectedItem).REMISE = 1;
                            }
                        }
                    }
                }
                dataGridElementDevis.UpdateLayout();
            }
            catch (Exception ex)
            {
                Message.Show(ex.Message, Languages.txtDevis);
            }
        }

        private void dataGridElementDevis_PreparingCellForEdit(object sender, DataGridPreparingCellForEditEventArgs e)
        {
            try
            {
                if (e.Column.DisplayIndex == this.dataGridElementDevis.Columns[3].DisplayIndex)
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

        private void BtnSelectionTotal_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (this._selection)
                {
                    this._selection = false; this.BtnSelectionTotal.Content = "Sélection totale";
                    foreach (ObjELEMENTDEVIS item in dataGridElementDevis.ItemsSource)
                    {
                        item.REMISE = 0;
                        item.QUANTITEREMISENSTOCK = 0;
                    }
                }
                else
                {
                    this._selection = true; this.BtnSelectionTotal.Content = "Désélection totale";
                    foreach (ObjELEMENTDEVIS item in dataGridElementDevis.ItemsSource)
                    {
                        item.REMISE = 1;
                        item.QUANTITEREMISENSTOCK = item.QUANTITE;
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

