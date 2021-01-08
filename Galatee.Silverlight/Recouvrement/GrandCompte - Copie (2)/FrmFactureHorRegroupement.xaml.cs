using Galatee.Silverlight.ServiceRecouvrement;
using Galatee.Silverlight.Tarification.Helper;
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
    public partial class FrmFactureHorRegroupement : ChildWindow
    {
        #region EventHandling

        public delegate void MethodeEventHandler(object sender, CustumEventArgs e);
        public event MethodeEventHandler CallBack;
        CustumEventArgs MyEventArg = new CustumEventArgs();


        protected virtual void OnEvent(CustumEventArgs e)
        {
            if (CallBack != null)
                CallBack(this, e);
        }

        #endregion

        List<CsLclient> ListFacture_Selectionner = new List<CsLclient>();

        public FrmFactureHorRegroupement()
        {
            InitializeComponent();
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            MyEventArg.Bag = ListFacture_Selectionner;
            OnEvent(MyEventArg);
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            allowProgressBar();
            Galatee.Silverlight.ServiceRecouvrement.RecouvrementServiceClient service = new Galatee.Silverlight.ServiceRecouvrement.RecouvrementServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Recouvrement"));
            service.RetourneListeFactureNonSoldeCompleted += (s, args) =>
            {
                if (args != null && args.Cancelled)
                {
                    Message.ShowError("Une erreur est survenu lors du traitement", "Erreur");
                }
                if (args.Result==null || args.Result.Count<=0)
                {
                    Message.ShowInformation("Aucune données corresponte aux critères", "Information");
                }
                //Lstfacture = args.Result;
                dg_facture.ItemsSource = args.Result;
                desableProgressBar();
                //btn_Rech.IsEnabled = true;
                //desableProgressBar();
                return;
            };
            string centre = !string.IsNullOrWhiteSpace(txt_centre.Text) ? txt_centre.Text : string.Empty;
            string client = !string.IsNullOrWhiteSpace(txt_client.Text) ? txt_client.Text : string.Empty;
            string ordre = !string.IsNullOrWhiteSpace(txt_ordre.Text) ? txt_ordre.Text : string.Empty;
            string periode = !string.IsNullOrWhiteSpace(txt_periode.Text) ? txt_periode.Text : string.Empty;


            service.RetourneListeFactureNonSoldeAsync(centre, client, ordre, 0, periode);
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            ListFacture_Selectionner.Add((CsLclient)dg_facture.SelectedItem);
        }
        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            ListFacture_Selectionner.Remove((CsLclient)dg_facture.SelectedItem);
        }

        private void dg_facture_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        void allowProgressBar()
        {
            progressBar1.IsEnabled = true;
            progressBar1.Visibility = Visibility.Visible;
            progressBar1.IsIndeterminate = true;
        }
        void desableProgressBar()
        {
            progressBar1.IsIndeterminate = false;
            progressBar1.Visibility = Visibility.Collapsed;
        }
    }
}

