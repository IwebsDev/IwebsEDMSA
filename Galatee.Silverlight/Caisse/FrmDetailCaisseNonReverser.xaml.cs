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
using Galatee.Silverlight.ServiceCaisse ;

namespace Galatee.Silverlight.Caisse
{
    public partial class FrmDetailCaisseNonReverser : ChildWindow
    {
        public FrmDetailCaisseNonReverser()
        {
            InitializeComponent();
        }
        public FrmDetailCaisseNonReverser(List<CsHabilitationCaisse> lstDetailCaisseNonReverese)
        {
            InitializeComponent();
            this.dtg_CaisseNonReverse.ItemsSource = null;
            this.dtg_CaisseNonReverse.ItemsSource = lstDetailCaisseNonReverese;
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

