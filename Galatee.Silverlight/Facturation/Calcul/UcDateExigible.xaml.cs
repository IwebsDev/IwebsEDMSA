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

namespace Galatee.Silverlight.Facturation
{
    public partial class UcDateExigible : ChildWindow
    {
        public UcDateExigible()
        {
            InitializeComponent();
            this.datePicker1.SelectedDate = System.DateTime.Today.AddDays(15);
            MyDateExige = this.datePicker1.SelectedDate; 
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            OkClick = true;
            this.DialogResult = true;
        }
        public DateTime? MyDateExige { get; set; }
        public int MyExige { get; set; }
        public bool? IsFaturationTotal = false;
        public bool OkClick = false;

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true ;
        }

        private void datePicker1_CalendarClosed(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(this.datePicker1.Text))
                {
                    MyDateExige = this.datePicker1.SelectedDate;
                    DateTime dt1 = Convert.ToDateTime(MyDateExige);
                    TimeSpan ts = dt1 - System.DateTime.Today.Date;
                    int days = ts.Days;
                    if (days > 23)
                        Message.ShowWarning(Galatee.Silverlight.Resources.Facturation.Langue.MsgDateExigibiliteTropLong, "Facturation");
                    if (days < 15)
                        Message.ShowWarning(Galatee.Silverlight.Resources.Facturation.Langue.MsgDateExigibiliteTropCourte, "Facturation");
                    IsFaturationTotal = true;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

    
    }
}

