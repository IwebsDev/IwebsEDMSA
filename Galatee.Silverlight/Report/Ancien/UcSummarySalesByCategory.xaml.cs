using Galatee.Silverlight.ServicePrintings;
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
    public partial class UcSummarySalesByCategory : ChildWindow
    {
        public UcSummarySalesByCategory()
        {
            InitializeComponent();
            facturerationMois.IsChecked =true;
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


        //Dictionary<string, string> parameters = new Dictionary<string, string>();

        public Dictionary<string, string> Parameters { get; set; }
        public string DefinedRDLC { get; set; }


        public  List<CsSales> GetPrintObjects(string module)
        {
            try
            {
                if (Txt_Year.Text.Length != 4)
                {
                    return null;
                }
                else
                {
                    string coper = "001"; // FactureGeneralCoper
                    string libelleCoper = "";


                    if (facturerationMois.IsChecked==true)
                    {
                        coper = "001"; // FactureGeneralCoper
                        libelleCoper = "Monthly billing";
                    }
                    else if (factureration_Manuelle_Resiliation.IsChecked == true)
                    {
                        coper = "002"; // FactureManuelleCoper
                        libelleCoper = "Manual and termination billing";
                    }
                    else if (Tous.IsChecked == true)
                    {
                        coper = null;
                        libelleCoper = "All the billing";
                    }

                    //if (parameters.Count > 0)
                    //    parameters.Clear();
                    //parameters.Add("pCoper", libelleCoper);

                    int loaderHandler = LoadingManager.BeginLoading("Please Wait for pass payment ... ");


                    ReportServiceClient service = new ReportServiceClient(Utility.Protocole(), Utility.EndPoint(module));

                    List<CsSales> reportPrint = new List<CsSales>();

                    service.QUANTITY_SOLD_BY_CATEGAsync(Txt_Year.Text, coper, null, null);
                    service.QUANTITY_SOLD_BY_CATEGCompleted += (er, res) =>
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
                                //Code en cas de succès
                                //reportPrint = res.Result;
                                //Dictionary<string, string> param = new Dictionary<string, string>();
                                Dictionary<string, string> param = null;
                                string key = Utility.getKey();
                                this.Parameters = new Dictionary<string, string>();
                                this.Parameters.Add("pCoper", libelleCoper);
                                //this.DefinedRDLC = reportName;
                                //Effectue l'aperçcu avant imprèssion
                                //Utility.ActionPreview<ServicePrintings.CsSales, Galatee.Silverlight.ServiceReport.CsSales>(reportPrint, param, "", module);
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



                    //this.DefinedRDLC = "test";

                  

                    return reportPrint;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

}

