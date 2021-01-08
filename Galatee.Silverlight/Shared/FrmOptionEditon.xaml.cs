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

namespace Galatee.Silverlight.Shared
{
    public partial class FrmOptionEditon : ChildWindow
    {
        public string OptionSelect = SessionObject.EnvoiPrinter ;
        public bool IsOptionChoisi = false;
        public FrmOptionEditon()
        {
            InitializeComponent();
        }
    
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void Btn_Execl_Click_1(object sender, RoutedEventArgs e)
        {
            OptionSelect = SessionObject.EnvoiExecl;
            IsOptionChoisi = true;
            this.DialogResult = false;

        }

        private void Btn_Imprimante_Click_1(object sender, RoutedEventArgs e)
        {
            OptionSelect = SessionObject.EnvoiPrinter ;
            IsOptionChoisi = true;
            this.DialogResult = false;
        }

        private void Btn_Worde_Click_1(object sender, RoutedEventArgs e)
        {
            OptionSelect = SessionObject.EnvoiWord ;
            IsOptionChoisi = true;
            this.DialogResult = false;
        }

        private void Btn_Pdf_Click_1(object sender, RoutedEventArgs e)
        {
            OptionSelect = SessionObject.EnvoiPdf ;
            IsOptionChoisi = true;
            this.DialogResult = false;
        }
    }
}

