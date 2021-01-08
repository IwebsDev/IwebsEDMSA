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
    public partial class UcSubscriptionListByPeriod : ChildWindow
    {
        public UcSubscriptionListByPeriod()
        {
            InitializeComponent();
            Recapitulatif.IsChecked = true;
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
        public Dictionary<string,string> Parameters { get; set; }
        public string DefinedRDLC { get; set; }
        public  List<CsFacture> GetPrintObjects(string module)
        {
            try
            {
                if (!string.IsNullOrEmpty(Txt_DateDebut.Text) && !string.IsNullOrEmpty(Txt_DateFin.Text))
                {
                    //CultureInfo culture = CultureInfo.("fr-FR");
                    //DateTime _DateDebut = DateTime.Parse(Txt_DateDebut.Text.ToString(culture));
                    //DateTime _DateFin = DateTime.Parse(Txt_DateFin.Text.ToString(culture));
                    DateTime _DateDebut = DateTime.Parse(Txt_DateDebut.Text.ToString());
                    DateTime _DateFin = DateTime.Parse(Txt_DateFin.Text.ToString());
                    string reportName = string.Empty;

                    if (Recapitulatif.IsChecked == true)
                        reportName = "NouvelAbonnementParPeriode";
                    else
                        if (Detail.IsChecked==true)
                        {
                            reportName = "NouvelAbonnementParPeriodeDetail";
                        }

                    int loaderHandler = LoadingManager.BeginLoading("Please Wait for pass payment ... ");
                    Dictionary<string, string> parameters = new Dictionary<string, string>();
                    parameters.Add("pDateDebut", Txt_DateDebut.Text);
                    parameters.Add("pDateFin", Txt_DateFin.Text);
                    this.Parameters = parameters;

                    ReportServiceClient service = new ReportServiceClient(Utility.Protocole(),Utility.EndPoint(module));
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
                                //Dictionary<string, string> param = new Dictionary<string, string>();
                                Dictionary<string, string> param = new Dictionary<string,string>();
                                string key = Utility.getKey();
                                this.DefinedRDLC = reportName;
                                param.Add("pDateDebut", _DateDebut.ToString());
                                param.Add("pDateFin", _DateFin.ToString());
                                //Effectue l'aperçcu avant imprèssion
                                Utility.ActionDirectOrientation<ServicePrintings.CsFacture, Galatee.Silverlight.ServiceReport.CsFacture>(reportPrint, param, SessionObject.DefaultPrinter, "VisuReportNewElectricityServiceConnection", module, false);

                                //Utility.ActionPreview<ServicePrintings.CsFacture, Galatee.Silverlight.ServiceReport.CsFacture>(reportPrint, param, reportName, module);
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
                throw new Exception("Vous devez choisir des dates");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void Txt_DateFin_TextChanged(object sender, EventArgs e)
        {

        }   



    }
}

