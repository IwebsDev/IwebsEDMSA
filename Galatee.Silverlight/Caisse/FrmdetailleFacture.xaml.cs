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
using Galatee.Silverlight.ServiceCaisse;
using System.Collections.ObjectModel;

namespace Galatee.Silverlight.Caisse
{
    public partial class FrmdetailleFacture : ChildWindow
    {
        public FrmdetailleFacture()
        {
            InitializeComponent();
        }
        public FrmdetailleFacture(List<CsLclient>lstfactureRegle)
        {
            InitializeComponent();
            dtgrdParametre.ItemsSource = new ObservableCollection<CsLclient>(lstfactureRegle.Where(t=>t.Selectionner==true));
        }
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}

