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
using Galatee.Silverlight.ServiceFacturation ;


namespace Galatee.Silverlight.Facturation
{
    public partial class UcTypeFacturation : ChildWindow
    {
        public UcTypeFacturation()
        {
            InitializeComponent();
            this.btn_definitif.IsEnabled = false;
            this.btn_Partielle .IsEnabled = false;
            if (!SessionObject.IsFacturationPartiel)
            {
                this.btn_definitif.Visibility = System.Windows.Visibility.Collapsed;
                this.btn_Partielle.Visibility = System.Windows.Visibility.Collapsed;
                this.Height = 136;
            }

        }
        public DateTime ? MyDateExige { get; set; }
        public int MyExige { get; set; }
        public bool? IsFaturationTotal = false;
        public bool OkClick = false;
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            OkClick = true;
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void btn_Partielle_Click(object sender, RoutedEventArgs e)
        {
            OkClick = true;
            IsFaturationTotal = false;
            this.DialogResult = false;


        }

        private void datePicker1_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
           
        }


        private void btn_definitif_Click(object sender, RoutedEventArgs e)
        {
            OkClick = true;
            IsFaturationTotal = true ;
            this.DialogResult = false;
           
        }

       

        private void datePicker1_CalendarClosed(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(this.datePicker1.Text))
            {
                this.btn_Partielle.IsEnabled = true;
                this.btn_definitif.IsEnabled = true;
                MyDateExige = this.datePicker1.SelectedDate;


                DateTime dt1 = Convert.ToDateTime(MyDateExige);
                TimeSpan ts = dt1 - System.DateTime.Today.Date;
                int days = ts.Days;
                if (days > 23)
                {
                    Message.ShowWarning(Galatee.Silverlight.Resources.Facturation.Langue.MsgDateExigibiliteTropLong, "Facturation");
                    return;
                }
                if (days < 15)
                {
                    Message.ShowWarning(Galatee.Silverlight.Resources.Facturation.Langue.MsgDateExigibiliteTropCourte, "Facturation");
                    return;
                }
                OkClick = true;
                IsFaturationTotal = true;
                this.DialogResult = false;
            }
        }
    }
}

