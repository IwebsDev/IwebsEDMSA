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

namespace Galatee.Silverlight.Accueil
{
    public partial class FrmPeriodeEdition : ChildWindow
    {
        public bool IsOKclick = false;
        public string PeriodeDebut = string.Empty;
        public string PeriodeFin = string.Empty;
        public FrmPeriodeEdition()
        {
            InitializeComponent();
            this.txt_PeriodeDebut.MaxLength = SessionObject.Enumere.TaillePeriode + 1;
            this.txt_PeriodeFin.MaxLength = SessionObject.Enumere.TaillePeriode + 1;
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            IsOKclick = true;
            PeriodeDebut = string.IsNullOrEmpty(this.txt_PeriodeDebut.Text) ? string.Empty : this.txt_PeriodeDebut.Text;
            PeriodeFin = string.IsNullOrEmpty(this.txt_PeriodeFin.Text) ? string.Empty : this.txt_PeriodeFin.Text; 
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}

