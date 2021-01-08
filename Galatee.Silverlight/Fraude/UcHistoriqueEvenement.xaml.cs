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

namespace Galatee.Silverlight.Fraude
{
    public partial class UcHistoriqueEvenement : ChildWindow
    {
        public UcHistoriqueEvenement()
        {
            InitializeComponent();
        }
        public UcHistoriqueEvenement(List<Galatee.Silverlight.ServiceFraude.CsEvenement> lstHistorique)
        {
            InitializeComponent();
            this.DataGridEvenement.ItemsSource = lstHistorique;
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

