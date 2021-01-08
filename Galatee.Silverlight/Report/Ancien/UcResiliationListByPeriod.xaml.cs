using Galatee.Silverlight.ServiceReport;
using System;
using System.Collections.Generic;
using System.Globalization;
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
    public partial class UcResiliationListByPeriod : ChildWindow
    {
        public UcResiliationListByPeriod()
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







        Dictionary<string, string> Parameters;
        string reportName = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            Recapitulatif.IsChecked = true;
        }

        public  List<CsFacture> GetPrintObjects(string module)
        {
            try
            {
                if (!string.IsNullOrEmpty(Txt_DateDebut.Text) && !string.IsNullOrEmpty(Txt_DateFin.Text))
                {
                    //CultureInfo culture = CultureInfo.CurrentCulture("fr-FR");
                    DateTime _DateDebut = DateTime.Parse(Txt_DateDebut.Text.ToString());
                    DateTime _DateFin = DateTime.Parse(Txt_DateFin.Text.ToString());                   

                   
                    Dictionary<string, string> parameters = new Dictionary<string, string>();
                    parameters.Add("pDateDebut", Txt_DateDebut.Text);
                    parameters.Add("pDateFin", Txt_DateFin.Text);
                    this.Parameters = parameters;
                    int loaderHandler = LoadingManager.BeginLoading("Please Wait for pass payment ... ");

                    ReportServiceClient service = new ReportServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint(module));
                    // string mois = (Cmb_month.SelectedIndex < 0) ? null : (Cmb_month.SelectedIndex + 1).ToString("00");
                    // le type "01" correspond a AffichageListeAbonnement
                    List<ServiceReport.CsFacture> reportPrint = new List<CsFacture>();

                    service.ListeAbonnementsParPeriodeAsync(_DateDebut, _DateFin, 01, null, parameters);
                    service.ListeAbonnementsParPeriodeCompleted += (er, res) =>
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
                                Dictionary<string, string> param = new Dictionary<string,string>();
                                string key = Utility.getKey();
                                param.Add("pDateDebut", _DateDebut.ToString());
                                param.Add("pDateFin", _DateFin.ToString());
                                //Effectue l'aperçcu avant imprèssion
                                Utility.Action<ServicePrintings.CsFacture, Galatee.Silverlight.ServiceReport.CsFacture>(reportPrint, param, SessionObject.DefaultPrinter, "VisuReportNewElectricityServiceConnection", module);
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

                   //this.DefinedRDLC = reportName;

                    return reportPrint;
                }
                throw new Exception("Vous devez choisir des dates");
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
                case "Recapitulatif":
                    reportName = "ResiliationAbonnementParPeriode";
                    break;
                case "Detail":
                    reportName = "ResiliationAbonnementParPeriodeDetail";
                    break;

                default:
                    break;
            }
        }   

    }
}

