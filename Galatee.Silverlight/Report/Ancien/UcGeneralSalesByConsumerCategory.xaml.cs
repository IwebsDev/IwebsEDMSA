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
    public partial class UcGeneralSalesByConsumerCategory : ChildWindow
    {
        private List<string> months = new List<string>() { "JANUARY", "FEBRUARY", "MARCH", "APRIL", "MAY", "JUNE", "JULY", "AUGUST", "SEPTEMBER", "OCTOBER", "NOVEMBER", "DECEMBER" };
        private List<CsTa> categories = new List<CsTa>();


        string _Coper = "001"; // SessionObject.Enumere.FactureGeneraleCoper;
        string _LibelleCoper = "Monthly billing";
        Dictionary<string, string> Parameters = new Dictionary<string, string>();
        string DefinedRDLC;

        public UcGeneralSalesByConsumerCategory()
        {
            InitializeComponent();
            Cmb_month.Items.Clear();
            Cmb_month.Items.Add("  ");
            foreach (string mois in months)
                Cmb_month.Items.Add(mois);
            FillCategory();
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
        
        public void FillCategory()
        {
            try
            {
                int loaderHandler = LoadingManager.BeginLoading("Please Wait for pass payment ... ");
                ReportServiceClient service = new ReportServiceClient(Utility.Protocole(), Utility.EndPoint("Report"));
                //categories = service.SELECT_ALL_CATEGORY_By_NUMTABLE(12);

                 service.SELECT_ALL_CATEGORY_By_NUMTABLEAsync(12); // 12 represente la category
                 service.SELECT_ALL_CATEGORY_By_NUMTABLECompleted += (er, res) =>
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
                             categories.Clear();
                             categories = res.Result;
                             Cmb_cat.Items.Clear();
                             foreach (CsTa item in categories)
                                 Cmb_cat.Items.Add(item.LIBELLE);
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
            }
            catch (Exception ex)
            {
                //throw;
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }
      
        public  List<CsCprofac> GetPrintObjects(string module)
        {
            try
            {
                this.Parameters = new Dictionary<string, string>();
                this.Parameters.Add("pCoper", _LibelleCoper);
                this.DefinedRDLC = "GeneralSalesByCategory";
                int loaderHandler = LoadingManager.BeginLoading("Please Wait for pass payment ... ");
                string mois = (Cmb_month.SelectedIndex <= 0) ? null : Cmb_month.SelectedIndex.ToString("00");
                //string categ = (Cmb_cat.SelectedValue == null) ? null : Cmb_cat.SelectedValue.ToString();
                string categ = (Cmb_cat.SelectedIndex < 0) ? null : categories[Cmb_cat.SelectedIndex].CODE;
                //List<ServiceReport.CsCprofac> listprofac = new List<ServiceReport.CsCprofac>();
                
                List<ServiceReport.CsCprofac> reportPrint = new List<CsCprofac>();

                ReportServiceClient service = new ReportServiceClient(Utility.Protocole(), Utility.EndPoint(module));
                service.SPX_CPROFAC_SEL_GEN_CONSOAsync(Txt_year.Text, mois, categ, _Coper, null, Parameters);
                service.SPX_CPROFAC_SEL_GEN_CONSOCompleted += (er, res) =>
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
                            reportPrint = res.Result;
                            //Dictionary<string, string> param = new Dictionary<string, string>();
                            Dictionary<string, string> param = null;
                            string key = Utility.getKey();

                            //Effectue l'aperçcu avant imprèssion
                            Utility.ActionPreview<ServicePrintings.CsCprofac, Galatee.Silverlight.ServiceReport.CsCprofac>(reportPrint, param, "CurrentPaymentsDetails", module);
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

