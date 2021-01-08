using Galatee.Silverlight.ServiceInterfaceComptable;
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

namespace Galatee.Silverlight.InterfaceComptable
{
    public partial class UcOperationCaisse : ChildWindow
    {
        public UcOperationCaisse()
        {
            InitializeComponent();
        }
        public bool isOkClick = false;
        public event EventHandler Closed;
        public List<CsOperationComptable > MyObjectList { get; set; }
        List<CsOperationComptable> _ListeOperation = new List<CsOperationComptable>();
        public UcOperationCaisse(List<ServiceInterfaceComptable.CsOperationComptable> ListeOperation)
        {
            InitializeComponent();
            dtgOperation.ItemsSource = ListeOperation;
            _ListeOperation = ListeOperation;

        }
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            getCloseAfterSelection();
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        void getCloseAfterSelection()
        {
            try
            {
                if (Closed != null)
                {
                    isOkClick = true;
                    //MyObjectList = _ListeOperation.Where(t => t.IsSelect == true).ToList();
                    MyObjectList = _ListeOperation.Where(t => t.IsSelect == true && t.CODE != "14").ToList(); // Exclure Operation ResiliationAbonnement

                    Closed(this, new EventArgs());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void btn_TousSelect_Click_1(object sender, RoutedEventArgs e)
        {
            if (_ListeOperation != null && _ListeOperation.Count != 0)
                _ListeOperation.ForEach(t => t.IsSelect = true);
         }

        private void btn_TousDeSelect_Click(object sender, RoutedEventArgs e)
        {
            if (_ListeOperation != null && _ListeOperation.Count != 0)
                _ListeOperation.ForEach(t => t.IsSelect = false );
        }
        private void dgMyDataGrid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            DataGrid dg = (sender as DataGrid);
            var allObjects = dg.ItemsSource as List<CsLclient>;
            if (dg.SelectedItem != null)
            {
                CsOperationComptable SelectedObject = (CsOperationComptable)dg.SelectedItem;
                if (SelectedObject.IsSelect == false)
                    SelectedObject.IsSelect = true;
                else
                    SelectedObject.IsSelect = false;

            }
        }



    }
}

