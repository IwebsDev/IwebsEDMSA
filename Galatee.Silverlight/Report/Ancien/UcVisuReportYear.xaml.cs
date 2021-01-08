﻿using Galatee.Silverlight.ServiceReport;
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
    public partial class UcVisuReportYear : ChildWindow
    {
        public UcVisuReportYear()
        {
            InitializeComponent();
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

        public  List<ServiceReport.CsConnexion> GetPrintObjects(string module)
        {
            try
            {
                ReportServiceClient service = new ReportServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint(module));
                // string mois = (Cmb_month.SelectedIndex < 0) ? null : (Cmb_month.SelectedIndex + 1).ToString("00");
                List<ServiceReport.CsConnexion> reportPrint = new List<CsConnexion>();


                int loaderHandler = LoadingManager.BeginLoading("Please Wait for pass payment ... ");
                service.ObtenirNewWaterConnexionAsync(Txt_year.Text);
                service.ObtenirNewWaterConnexionCompleted += (er, res) =>
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
                            Utility.ActionPreview<ServicePrintings.CsConnexion, Galatee.Silverlight.ServiceReport.CsConnexion>(reportPrint, null, "VisuReportYear", module);
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

