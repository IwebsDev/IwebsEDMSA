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
using Galatee.Silverlight.ServiceCaisse;

namespace Galatee.Silverlight.Caisse
{
    public partial class BilletageCaisse : ChildWindow
    {
        public BilletageCaisse()
        {
            InitializeComponent();
            ReturneAllMonaie();
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        void ReturneAllMonaie()
        {
            try
            {
                CaisseServiceClient service = new CaisseServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Caisse"));
                service.ReturneAllMonaieAsync();
                service.ReturneAllMonaieCompleted += (s, args) =>
                {
                    if (args.Cancelled || args.Error != null)
                    {
                        Message.ShowError("Erreur de chargement des reçus de la caisse. Réessayez svp !", "Erreur");
                        this.DialogResult = true;
                    }
                    this.Lsv_Monaie.ItemsSource = null;
                    this.Lsv_Monaie.ItemsSource = args.Result;
                };
                service.CloseAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}

