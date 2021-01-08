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
using Galatee.Silverlight.ServiceAccueil;

namespace Galatee.Silverlight.Accueil
{
    public partial class UcListeReglageCompteur : ChildWindow
    {
        public bool isOkClick = false;
        public   CsReglageCompteur leReglageSelect = new CsReglageCompteur();
        public UcListeReglageCompteur(List<CsReglageCompteur> lstReglage )
        {
            InitializeComponent();
            this.dtgAppareils.ItemsSource = null;
            this.dtgAppareils.ItemsSource = lstReglage;
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            isOkClick = true;
            if (this.dtgAppareils.SelectedItem != null)
                leReglageSelect = (CsReglageCompteur)dtgAppareils.SelectedItem;
            this.DialogResult = true;

        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void dtgAppareils_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            var dmdRow = e.Row.DataContext as CsReglageCompteur;
            if (dmdRow != null)
            {
                if (dmdRow.IsRecommender)
                {
                    SolidColorBrush SolidColorBrush = new SolidColorBrush(Colors.Green );
                    e.Row.Foreground = SolidColorBrush;
                    e.Row.FontWeight = FontWeights.Bold;
                }
            }
        }
    }
}

