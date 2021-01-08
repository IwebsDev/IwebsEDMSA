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

namespace Galatee.Silverlight.Recouvrement
{
    public partial class UcTypeCoupure : ChildWindow
    {
        public UcTypeCoupure()
        {
            InitializeComponent();
        }
        List<CsTypeCoupure> lesTypeCoupure = new List<CsTypeCoupure>();
        public UcTypeCoupure(List<CsTypeCoupure> lstTypeCoupure)
        {
            InitializeComponent();
            lesTypeCoupure = lstTypeCoupure;
            lesTypeCoupure.ForEach(t => t.LIBELLE = t.LIBELLE + " " + t.COUT);
            cbo_TypeCoupure.ItemsSource = null;
            cbo_TypeCoupure.ItemsSource = lesTypeCoupure;
            cbo_TypeCoupure.DisplayMemberPath = "LIBELLE";
        }
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
     
        CsDetailCampagne dtailCampagneSelect = new CsDetailCampagne();
        public CsTypeCoupure typeCoupure = new CsTypeCoupure();
        private void cbo_TypeCoupure_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbo_TypeCoupure.SelectedItem != null)
            typeCoupure = (CsTypeCoupure)cbo_TypeCoupure.SelectedItem;
        }
    }
}

