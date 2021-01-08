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
using Galatee.Silverlight.ServiceCaisse;
using System.Windows.Shapes;
using Galatee.Silverlight.Shared;

namespace Galatee.Silverlight.Shared
{
    public partial class UcsearchBill1 : UserControl
    {
        public UcsearchBill1()
        {
            InitializeComponent();
        }
        public ChildWindow _child
        { get; set; }

        public UcsearchBill1(ChildWindow w)
        {
            InitializeComponent();
            _child = w;
        }

        public string ClientRef
           { get; set; }

        private void btnsearch_Click(object sender, RoutedEventArgs e)
        {
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
            //   Message.Show(ex,Galatee.Silverlight.Resources.Langue.errorTitle);
            //}
            
            
        }

        void clientClosed(object sender, EventArgs e)
        {
            try
            {
                UcConsulClientAccount ac = sender as UcConsulClientAccount;
                if (ac.DialogResult == true)
                {
                    //ClientRef = ac.selectedClient.ReferenceClient;
                    _child.DialogResult = true;
                }
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            _child.DialogResult = true;
        }
    }
}
