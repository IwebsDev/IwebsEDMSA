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

namespace Galatee.Silverlight.Recouvrement
{
    public partial class UcDateRDV : ChildWindow
    {
        public UcDateRDV()
        {
            InitializeComponent();
            this.Title = "Date de rendez-vous";
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
        public DateTime  MyDateRdv { get; set; }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true ;
        }

        private void calendar1_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.calendar1.SelectedDate > System.DateTime.Today)
                MyDateRdv = this.calendar1.SelectedDate.Value;
            this.DialogResult = false;
        }
    }
}

