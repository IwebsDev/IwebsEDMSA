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
    public partial class UcSalesByCustomerCategory : ChildWindow
    {

        string _Coper = "001"; // SessionObject.Enumere.FactureGeneraleCoper;
        string _LibelleCoper = "Monthly billing";

        public UcSalesByCustomerCategory()
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

        private List<string> months = new List<string>() { "Janvier", "Fevrier", "Mars", "Avril", "Mai", "Juin", "Julliet", "Aout", "Septembre", "Octobre", "Novembre", "Decembre" };
        private List<CsTa> categories = new List<CsTa>();

        public void FillCategory()
        {
            try
            {
                ReportServiceClient service = new ReportServiceClient(Utility.Protocole(), Utility.EndPoint("Report"));
                int loaderHandler = LoadingManager.BeginLoading("Please Wait for pass payment ... ");

                service.SELECT_ALL_CATEGORY_By_NUMTABLEAsync(12);// 12 represente la category
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
                //this.Parameters = new Dictionary<string, string>();
                //this.Parameters.Add("pCoper", _LibelleCoper);

                int loaderHandler = LoadingManager.BeginLoading("Patienter ... ");


                string mois = (Cmb_month.SelectedIndex <= 0) ? null : Cmb_month.SelectedIndex.ToString("00");
                //string categ = (Cmb_cat.SelectedValue == null) ? null : Cmb_cat.SelectedValue.ToString();
                string categ = (Cmb_cat.SelectedIndex < 0) ? null : categories[Cmb_cat.SelectedIndex].CODE;
                //List<ServiceReport.CsCprofac> listprofac = new List<ServiceReport.CsCprofac>();

                ReportServiceClient service = new ReportServiceClient(Utility.Protocole(),Utility.EndPoint(module));

                List<ServiceReport.CsCprofac> reportPrint = new List<CsCprofac>();

                service.SPX_CPROFAC_SEL_ELEC_CONSOAsync(Txt_year.Text, mois, categ, _Coper);
                service.SPX_CPROFAC_SEL_ELEC_CONSOCompleted += (er, res) =>
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
                case "Autre_Facturation":

                    _Coper = "002"; // SessionObject.Enumere.FactureManuelleCoper;
                    _LibelleCoper = "Other billing";
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

