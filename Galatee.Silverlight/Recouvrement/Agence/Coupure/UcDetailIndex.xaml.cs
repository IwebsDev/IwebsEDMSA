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
    public partial class UcDetailIndex : ChildWindow
    {

        public UcDetailIndex()
        {
            InitializeComponent();
        }
        List<CsTypeCoupure> lesTypeCoupure = new List<CsTypeCoupure>();
        public CsDetailCampagne leDetailCampagne = new CsDetailCampagne();
        public UcDetailIndex(List<CsTypeCoupure> lstTypeCoupure)
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
            if (!string.IsNullOrEmpty(this.txt_index.Text) && !string.IsNullOrEmpty(dtpDteCoupure.Text) && cbo_TypeCoupure.SelectedItem != null)
            {
                leDetailCampagne.INDEX = int.Parse(this.txt_index.Text);
                leDetailCampagne.DATECOUPURE = dtpDteCoupure.SelectedDate.Value ;
                leDetailCampagne.TYPECOUPURE = ((CsTypeCoupure)cbo_TypeCoupure.SelectedItem).CODE;
                leDetailCampagne.MONTANTFRAIS  = ((CsTypeCoupure)cbo_TypeCoupure.SelectedItem).COUT ;
                leDetailCampagne.FK_IDOBSERVATION = ((CsTypeCoupure)cbo_TypeCoupure.SelectedItem).PK_ID ;
            }
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}

