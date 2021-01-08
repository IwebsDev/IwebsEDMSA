using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Data;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Galatee.Silverlight.Shared;
using Galatee.Silverlight.ServiceCaisse;

namespace Galatee.Silverlight.Shared
{
    public partial class UcRechercheParNumFacture : UserControl
    {
        public CsClient selectedClient
            { get; set; }

        public ChildWindow _child
        { get;set ;}

        public UcRechercheParNumFacture()
        {
            InitializeComponent();
        }

        public UcRechercheParNumFacture( ChildWindow w)
        {
            InitializeComponent();
            _child = w;

            dgrdroute.Columns[0].Header = Galatee.Silverlight.Resources.Accueil.Langue.lbl_center;
            dgrdroute.Columns[1].Header = Galatee.Silverlight.Resources.Accueil.Langue.lbl_client;
            dgrdroute.Columns[2].Header = Galatee.Silverlight.Resources.Accueil.Langue.lbl_Ordre;
            dgrdroute.Columns[3].Header = Galatee.Silverlight.Resources.Langue.dg_nomprenom;
        }
        private void LayoutRoot_Loaded(object sender, RoutedEventArgs e)
        {
            //choice datagrid 
        }

   

        private void dgrdroute_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedClient = dgrdroute.SelectedItem != null ? (CsClient)dgrdroute.SelectedItem : null;
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(this.txt_PeriodeDebut.Text) && !string.IsNullOrEmpty(this.txt_PeriodeFin.Text))
            { 
            
            }


            //try
            //{
            //    if (!string.IsNullOrEmpty(Txt_from.Text) && !string.IsNullOrEmpty(Txt_to.Text) &&
            //            !string.IsNullOrEmpty(Txt_docFrom.Text) && !string.IsNullOrEmpty(Txt_docTo.Text))
            //    {
            //        btnsearch.IsEnabled = false;
            //        CaisseServiceClient client = new CaisseServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Caisse"));
            //        client.RetourneClientsParBillingPeriodCompleted += (send, args) =>
            //            {
            //                if (args.Cancelled || args.Error != null)
            //                {
            //                    Message.Show("error d'invocation du service", Galatee.Silverlight.Resources.Langue.wcf_error);
            //                    btnsearch.IsEnabled = true;
            //                    return;
            //                }

            //                if (args.Result != null)
            //                {
            //                    if (args.Result.Count == 0)
            //                    {
            //                        Message.Show("no data match for those criteria", Galatee.Silverlight.Resources.Langue.wcf_error);
            //                        btnsearch.IsEnabled = true;
            //                        return;
            //                    }
            //                    else
            //                    {
            //                        // open  other control from here 
            //                        btnsearch.IsEnabled = true;
            //                        UcConsulClientAccount clientAccount = new UcConsulClientAccount(args.Result, _child);
            //                        clientAccount.Closed += new EventHandler(clientClosed);
            //                        clientAccount.Show();
            //                    }
            //                }
            //                else
            //                {
            //                    Message.Show("no data match for those criteria", Galatee.Silverlight.Resources.Langue.wcf_error);
            //                    btnsearch.IsEnabled = true;
            //                    return;
            //                }

            //            };
            //        client.RetourneClientsParBillingPeriodAsync(Txt_from.Text, Txt_to.Text, Txt_docFrom.Text, Txt_docTo.Text);
            //    }
            //    else
            //        Message.Show("All fields are required !", Galatee.Silverlight.Resources.Langue.errorTitle);
            //}
            //catch (Exception ex)
            //{
            //    Message.Show(ex, Galatee.Silverlight.Resources.Langue.errorTitle);
            //}
                
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            _child.DialogResult = true;
            
        }

        private void dgrdamount_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                selectedClient = dgrdamount.SelectedItem != null ? (CsClient)dgrdamount.SelectedItem : null;
            }
            catch (Exception ex)
            {
                Message.Show(ex, Galatee.Silverlight.Resources.Langue.errorTitle);
            }
        }

        private void btnView_Click(object sender, RoutedEventArgs e)
        {
           

            //passage++;
        }

    
        void UcConsulClientAccountClosed(object sender, EventArgs e)
        {
            

        }
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            _child.DialogResult = true;
        }

        private void LayoutRoot_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key.Equals(Key.Enter))
            {
                btnOk_Click(null, null);
            }
        }

    }
}
