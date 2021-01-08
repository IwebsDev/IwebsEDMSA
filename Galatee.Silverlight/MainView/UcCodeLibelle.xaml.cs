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
using Galatee.Silverlight.ServiceReport;

namespace Galatee.Silverlight.MainView
{
    public partial class UcCodeLibelle : ChildWindow
    {
        public List<CParametre> EltSelectionnes = new List<CParametre>();
        private List<CParametre> liste = new List<CParametre>();
        public List<CParametre> Liste
        {
            get { return liste; }
            set { liste = value; }
        }

        public UcCodeLibelle()
        {
            InitializeComponent();
        }

        public UcCodeLibelle(List<CParametre> DonnesARemplir)
        {
            InitializeComponent();
            this.Liste = new List<CParametre>(DonnesARemplir);
            this.Dtg_principal.ItemsSource = Liste;
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            int i = 0;
            foreach (CParametre item in Dtg_principal.SelectedItems)
            {
                //EltSelection.Add(item.Text);
                //this.Tag = this.Lsv_Reading.SelectedItems[i].Tag;
                EltSelectionnes.Add(item);
                i++;

            }
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}

