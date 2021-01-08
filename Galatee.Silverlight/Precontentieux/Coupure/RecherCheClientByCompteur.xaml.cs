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

namespace Galatee.Silverlight.Precontentieux
{
    public partial class RecherCheClientByCompteur : ChildWindow
    {
        public RecherCheClientByCompteur()
        {
            InitializeComponent();
        }
        public  ServiceRecouvrement.CsClientRechercher leClient;
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.dataGrid1.SelectedItem != null)
            {
                leClient = new ServiceRecouvrement.CsClientRechercher();
                leClient = (ServiceRecouvrement.CsClientRechercher)this.dataGrid1.SelectedItem;
            }
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            RechercheCompteur(this.txt_NumCompteur.Text);
        }
        private void RechercheCompteur(string NumeroCompteur)
        {
            try
            {

                Galatee.Silverlight.ServiceRecouvrement.RecouvrementServiceClient proxy = new Galatee.Silverlight.ServiceRecouvrement.RecouvrementServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Recouvrement"));
                proxy.RechercheClientCompteurAsync(NumeroCompteur);
                proxy.RechercheClientCompteurCompleted += (ssn, args) =>
                {
                    if (args != null && args.Cancelled)
                    {
                        return;
                    }
                    if (args.Result != null && args.Result.Count != 0)
                    {
                        List<Galatee.Silverlight.ServiceRecouvrement.CsClientRechercher> _LstClient = new List<Galatee.Silverlight.ServiceRecouvrement.CsClientRechercher>();
                        _LstClient = args.Result;
                        if (_LstClient != null && _LstClient.Count != 0)
                        {
                            dataGrid1.ItemsSource = null;
                            dataGrid1.ItemsSource = _LstClient;
                            dataGrid1.SelectedItem = _LstClient[0];
                        }
                        else
                        {
                            Message.ShowInformation("Aucun client trouvé", "Info");
                            return;
                        }
                    }
                    else
                    {
                    }
                };
                proxy.CloseAsync();

            }
            catch (Exception)
            {
                Message.ShowError("Erreur au chargement des données", "Demande");
            }
        }

    }
}

