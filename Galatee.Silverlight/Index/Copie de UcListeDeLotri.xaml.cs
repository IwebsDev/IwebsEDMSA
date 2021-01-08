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
    public partial class UcListeDeLotri : ChildWindow
    {
        public UcListeDeLotri()
        {
            InitializeComponent();
        }
        public UcListeDeLotri(List<CsLotri> lstLot)
        {
            InitializeComponent();
            this.dataGrid1.ItemsSource = null;
            this.dataGrid1.ItemsSource = lstLot;
            prgBar.Visibility = System.Windows.Visibility.Collapsed;
        }
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.OKButton.IsEnabled = false;
            prgBar.Visibility = System.Windows.Visibility.Visible;

            List<CsLotri> lstLot = ((List<CsLotri>)this.dataGrid1.ItemsSource).Where(t => t.IsSelect).ToList();
            VerificationLot(lstLot);
        }
        private void  VerificationLot(List<CsLotri> leslot)
        {
            try
            {
               Suppression(leslot);
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
            }
        }
        private void Suppression(List<CsLotri> leslot)
        {
            try
            {
                prgBar.Visibility = System.Windows.Visibility.Visible ;
                FacturationServiceClient service = new FacturationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Facturation"));
                service.DeleteLotriAsync(leslot);
                service.DeleteLotriCompleted += (s, args) =>
                {
                    try
                    {
                        this.OKButton.IsEnabled = false;
                        prgBar.Visibility = System.Windows.Visibility.Collapsed;
                        if (args.Cancelled || args.Error != null)
                        {
                            Message.ShowError("Erreur d'invocation du service.", "Erreur");
                            return;
                        }
                        if (args.Result)
                        {
                            Message.ShowInformation("Suppression validée", "Information");
                            this.DialogResult = true;
                        }
                        else
                            Message.ShowInformation("Une erreur est survenu à la suppression", Galatee.Silverlight.Resources.Index.Langue.libelleModule);

                    }
                    catch (Exception ex)
                    {
                        OKButton.IsEnabled = true;
                        Message.ShowError(ex, Galatee.Silverlight.Resources.Langue.errorTitle);
                    }
                };
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
            }
        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
        private void dgMyDataGridMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            DataGrid dg = (sender as DataGrid);

            if (dg.SelectedItem != null)
            {
                CsLotri SelectedObject = (CsLotri)dg.SelectedItem;
                ((List<CsLotri>)dg.ItemsSource).Where(t => t.NUMLOTRI != SelectedObject.NUMLOTRI).ToList().ForEach(y=>y.IsSelect =false );

                List<CsLotri> lesFactureSelect = ((List<CsLotri>)dg.ItemsSource).Where(t=>t.NUMLOTRI == SelectedObject.NUMLOTRI).ToList();

                if (SelectedObject.IsSelect == false)
                    lesFactureSelect.ForEach(t => t.IsSelect = true);
                else
                    lesFactureSelect.ForEach(t => t.IsSelect = false );
            }
        }

        private void btn_ToutCentre_Click(object sender, RoutedEventArgs e)
        {
            List<CsLotri> lstLot = ((List<CsLotri>)this.dataGrid1.ItemsSource).ToList();
            foreach (CsLotri item in lstLot)
                item.IsSelect = true;
        }

        private void btn_rienCentre_Click(object sender, RoutedEventArgs e)
        {
            List<CsLotri> lstLot = ((List<CsLotri>)this.dataGrid1.ItemsSource).ToList();
            foreach (CsLotri item in lstLot)
                item.IsSelect = false;
        }

    }
}

