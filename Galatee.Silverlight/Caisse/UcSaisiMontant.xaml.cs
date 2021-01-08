using Galatee.Silverlight.ServiceCaisse;
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

namespace Galatee.Silverlight.Caisse
{
    public partial class UcSaisiMontant : ChildWindow
    {
        public UcSaisiMontant()
        {
            InitializeComponent();
        }
        public   CsLclient SelectedObject = new CsLclient();
        public UcSaisiMontant(CsLclient _SelectedObject)
        {
            InitializeComponent();
            SelectedObject = _SelectedObject;
        }

 
       public bool isOkClick = false;
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(this.txt_Montant.Text))
            {
                SelectedObject.MONTANTPAYE  = Convert.ToDecimal(this.txt_Montant.Text);
                isOkClick = true;
            }
            this.DialogResult = true;

        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void ChildWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key.Equals(Key.Enter))
            {
                if (!string.IsNullOrEmpty(this.txt_Montant.Text))
                {
                    SelectedObject.MONTANTPAYE = Convert.ToDecimal(this.txt_Montant.Text);
                    isOkClick = true;
                }
                this.DialogResult = true;
            }
        }
    }
}

