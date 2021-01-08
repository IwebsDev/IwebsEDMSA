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

namespace Galatee.Silverlight.Recouvrement.GrandCompte
{
    public partial class FrmMontantMandatementVerser : ChildWindow
    {
        public decimal montant = 0;
        public int Pk_Id = 0;
        public FrmMontantMandatementVerser( int Pk_id)
        {
            InitializeComponent();
            this.Pk_Id = Pk_id;
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void TextBox_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            if (!decimal.TryParse(txt_montant.Text,out montant))
            {
                Message.ShowError("Veuillez saisi une valeur numerique", "Erreur");
                txt_montant.Text = string.Empty;
            }
        }
    }
}

