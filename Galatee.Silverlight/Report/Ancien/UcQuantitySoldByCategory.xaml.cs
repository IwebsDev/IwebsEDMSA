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
    public partial class UcQuantitySoldByCategory : ChildWindow
    {

        string _Coper = "001"; // SessionObject.Enumere.FactureGeneraleCoper;
        string _LibelleCoper = "Monthly billing";
        public UcQuantitySoldByCategory()
        {
            InitializeComponent();
        }
        Dictionary<string, string> Parameters;

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            GetPrintObjects("Report");
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }



        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public  void GetPrintObjects(string module)
        {
            try
            {
                this.Parameters = new Dictionary<string, string>();
                this.Parameters.Add("pCoper", _LibelleCoper);

                int loaderHandler = LoadingManager.BeginLoading("Please Wait for pass payment ... ");
                //List<CsSales> reportPrint=new List<CsSales>();
                List<ServiceReport.CsCprofac> listprofac = new List<ServiceReport.CsCprofac>();

                ReportServiceClient service = new ReportServiceClient(Utility.ProtocoleFacturation(),Utility.EndPoint((module)));
                string key = Utility.getKey();
                service.QUANTITY_SOLD_BY_CATEGAsync(Txt_year.Text, _Coper, key, this.Parameters);
                service.QUANTITY_SOLD_BY_CATEGCompleted += (er, res) =>
                {
                    try
                    {
                        if (res.Error != null || res.Cancelled)
                        {
                            LoadingManager.EndLoading(loaderHandler);
                            Message.ShowInformation("Erreur survenue lors de l'appel service", "ERROR");
                            return;
                        }
                        if (res.Result == null)
                        {
                            LoadingManager.EndLoading(loaderHandler);
                            Message.ShowInformation("Impossible d'afficher le rapport", "ERROR");
                            return;
                            
                        }
                        ////Code en cas de succès
                        //reportPrint = res.Result;
                        //Dictionary<string, string> param = new Dictionary<string, string>();
                        //Dictionary<string, string> param = null;
                        //string key = Utility.getKey();

                        ////Effectue l'aperçcu avant imprèssion
                        Utility.ActionPreview<ServicePrintings.CsSales>(null,"QuantitySoldByCategory",module,key);
                        //Utility.ActionPreview<ServicePrintings.CsSales, Galatee.Silverlight.ServiceReport.CsSales>(reportPrint, param, "QuantitySoldByCategory", module);
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

                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void RadioButton_Checked_1(object sender, RoutedEventArgs e)
        {
           
            switch (((RadioButton)sender).Name)
            {
                case "Facturation_Mensuelle":

                    _Coper = "001"; // SessionObject.Enumere.FactureGeneraleCoper;
                    _LibelleCoper = "Monthly billing";
                    break;
                case "Facturation_Mensuelle_Résiliation":

                    _Coper = "002"; // SessionObject.Enumere.FactureManuelleCoper;
                    _LibelleCoper = "Manual and termination billing";
                    break;
                case "Tous":

                    _Coper = null;
                    _LibelleCoper = "All the billing";
                    break;
                default:
                    break;
            }


        }
    }
}

