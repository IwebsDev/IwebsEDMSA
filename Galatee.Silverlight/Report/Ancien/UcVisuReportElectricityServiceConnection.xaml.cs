using Galatee.Silverlight.ServiceReport;
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

namespace Galatee.Silverlight.Report
{
    public partial class UcVisuReportElectricityServiceConnection : ChildWindow
    {
        public UcVisuReportElectricityServiceConnection()
        {
            InitializeComponent();

            Cmb_month.Items.Clear();
            foreach (var item in months)
                Cmb_month.Items.Add(item);
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            GetPrintObjects("Report");
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }


        private List<string> months = new List<string>() { "JANUARY", "FEBRUARY", "MARCH", "APRIL", "MAY", "JUNE", "JULY", "AUGUST", "SEPTEMBER", "OCTOBER", "NOVEMBER", "DECEMBER" };
        public List<string> Months
        {
            get { return months; }
            set { months = value; }
        }



        public  List<ServiceReport.CsConnexion> GetPrintObjects(string module)
        {
            try
            {
                int loaderHandler = LoadingManager.BeginLoading("Please Wait for pass payment ... ");
                ReportServiceClient service = new ReportServiceClient(Utility.Protocole(), Utility.EndPoint(module));
                string mois = (Cmb_month.SelectedIndex < 0) ? null : (Cmb_month.SelectedIndex + 1).ToString("00");
                List<ServiceReport.CsConnexion> reportPrint = new List<CsConnexion>();

                service.ObtenirConnexionsElectricityAsync(Txt_year.Text, mois, null);
                service.ObtenirConnexionsElectricityCompleted += (er, res) =>
                {
                    try
                    {
                        if (res.Error != null || res.Cancelled)
                        {
                            LoadingManager.EndLoading(loaderHandler);
                            throw new Exception("Cannot display report");
                        }
                        if (res.Result != null)
                        {
                            reportPrint = res.Result;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    finally
                    {
                        LoadingManager.EndLoading(loaderHandler);
                    }
                };

                return reportPrint;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

