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
using Galatee.Silverlight.ServiceRecouvrement ;
using System.Collections.ObjectModel;

namespace Galatee.Silverlight.Precontentieux
{
    public partial class FrmdetailleFacture : ChildWindow
    {
        public FrmdetailleFacture()
        {
            InitializeComponent();
        }
        public bool IsOKclick = false;
        public FrmdetailleFacture(List<CsLclient>lstfactureRegle)
        {
            InitializeComponent();
            dtgrdParametre.ItemsSource = new ObservableCollection<CsLclient>(lstfactureRegle);
        }
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            IsOKclick = true;
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void dgMyDataGrid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            DataGrid dg = (sender as DataGrid);
            var allObjects = dg.ItemsSource as List<CsLclient>;
            if (dg.SelectedItem != null)
            {
                CsLclient SelectedObject = (CsLclient)dg.SelectedItem;

                if (SelectedObject.Selectionner == false)
                    SelectedObject.Selectionner = true;
                else
                    SelectedObject.Selectionner = false;
            }
        }
    }
}

