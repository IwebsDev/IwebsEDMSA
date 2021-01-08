using Galatee.Silverlight.ServiceAdministration;
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

namespace Galatee.Silverlight.Administration
{
    public partial class FrmSynchroAnnuaire : ChildWindow
    {
        public FrmSynchroAnnuaire()
        {
            InitializeComponent();
            this.OKButton.IsEnabled = false;
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                   AdministrationServiceClient service = new AccesServiceWCF().GetAdministrationClient();
                    service.SynchroniseDonneeADCompleted  += (s, args) =>
                    {
                        if (args != null && args.Cancelled)
                            return;
                        Message.ShowInformation("Synchronisation terminée", "Administration");
                    };
                    service.SynchroniseDonneeADAsync ();
                    service.CloseAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void CheckBox_Click_1(object sender, RoutedEventArgs e)
        {
            if (Chk_ValiderSynchro.IsChecked == true)
                this.OKButton.IsEnabled = true;
            else
                this.OKButton.IsEnabled = false;
        }
    }
}

