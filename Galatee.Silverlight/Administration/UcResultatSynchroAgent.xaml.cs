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

namespace Galatee.Silverlight.Administration
{
    public partial class UcResultatSynchroAgent : ChildWindow
    {
        public UcResultatSynchroAgent()
        {
            InitializeComponent();
        }
        public UcResultatSynchroAgent(List<string> lstResultat)
        {
            InitializeComponent();
            this.lblMiseAJour.Content = lstResultat[0];
            this.lblModifier.Content = lstResultat[1];
            this.lblRejeter.Content = lstResultat[2];
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

