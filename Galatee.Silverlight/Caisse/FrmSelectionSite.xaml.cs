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
    public partial class FrmSelectionSite : ChildWindow
    {
        public FrmSelectionSite()
        {
            InitializeComponent();
        }
        CsClient leSiteSelectionner = new CsClient();
        public FrmSelectionSite(List<CsClient> _lstClient)
        {
            InitializeComponent();
            if (_lstClient != null && _lstClient.Count != 0)
            {
                Cbo_ListeSite.ItemsSource  = null;
                Cbo_ListeSite.ItemsSource = _lstClient;
                Cbo_ListeSite.DisplayMemberPath = "LIBELLESITE"; 
            }
        }
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.Cbo_ListeSite.SelectedItem != null)
                leSiteSelectionner = (CsClient)this.Cbo_ListeSite.SelectedItem;
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}

