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
using Galatee.Silverlight;
using Galatee.Silverlight.ServiceInterfaceComptable;
           
namespace InterfaceCompta
{
    public partial class FrmReexportation : ChildWindow
    {
        public FrmReexportation()
        {
            InitializeComponent();
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
        public void RetourneListeDeSite()
        {
            try
            {
                InterfaceComptableServiceClient service = new InterfaceComptableServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Index"));
                service.RetourneListeDeSiteCompleted += (s, args) =>
                {
                    try
                    {
                        if (args.Cancelled || args.Error != null || args.Result == null)
                        {
                            string error = args.Error.InnerException.ToString();
                            return;
                        }
                        else
                        {
                            this.CmbSiteEcriture.ItemsSource = null;
                            if (args.Result != null && args.Result.Count != 0)
                            {
                                this.CmbSiteEcriture.ItemsSource = args.Result;
                                this.CmbSiteEcriture.DisplayMemberPath = "LIBELLE";
                                CmbSiteEcriture.IsEnabled = true;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Message.ShowError(ex, Galatee.Silverlight.Resources.Caisse.Langue.errorTitle);
                    }
                };
                service.RetourneListeDeSiteAsync();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void RetourneCodeOperation()
        {
            try
            {
                //InterfaceComptableServiceClient serviceOperation = new InterfaceComptableServiceClient();
                //serviceOperation.RetourneCodeOperationCompleted += (s, args) =>
                //{
                //    try
                //    {
                //        if (args.Cancelled || args.Error != null
                //            )
                //        {
                //            string error = args.Error.InnerException.ToString();
                //            return;
                //        }
                //        else
                //        {
                //            this.CmbOperationEcriture.ItemsSource = null;
                //            if (args.Result != null && args.Result.Count != 0)
                //            {
                //                this.CmbOperationEcriture.ItemsSource = args.Result;
                //                this.CmbOperationEcriture.DisplayMemberPath = "LIBELLE";
                //                CmbOperationEcriture.IsEnabled = true;
                //            }
                //        }
                //    }
                //    catch (Exception ex)
                //    {
                //        Message.ShowError(ex, Galatee.Silverlight.Resources.Caisse.Langue.errorTitle);
                //    }
                //};
                //serviceOperation.RetourneCodeOperationAsync();

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        private void CmbSiteEcriture_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        private void ChildWindow_Loaded_2(object sender, RoutedEventArgs e)
        {
            RetourneListeDeSite();
            RetourneCodeOperation();

        }

        private void CmbOperationEcriture_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void RadioButton_Checked_1(object sender, SelectionChangedEventArgs e)
        {

        }

    }
}

