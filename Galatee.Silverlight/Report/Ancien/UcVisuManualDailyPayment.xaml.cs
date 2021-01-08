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
    public partial class UcVisuManualDailyPayment : ChildWindow
    {
        public UcVisuManualDailyPayment()
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





        List<CsReglement> listeDeReglement = new List<CsReglement>();
        List<CsReglement> listeDeDates = new List<CsReglement>();

        protected void Page_Load(object sender, EventArgs e)
        {
            InitialiseCtrl();
        }
        public string Matricule { get; set; }
        private void InitialiseCtrl()
        {
            #region Remplissage du comboBox des collecteurs


            int loaderHandler = LoadingManager.BeginLoading("Please Wait for pass payment ... ");


            ReportServiceClient service = new ReportServiceClient(Utility.Protocole(), Utility.EndPoint("Report"));

            //List<CsSales> reportPrint = new List<CsSales>();

            service.SelectionAgentParNomAsync(Matricule);
            service.SelectionAgentParNomCompleted += (er, res) =>
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
                        List<CsReglement> liste = res.Result; 
                        if (liste != null || liste.Count > 0)
                        {
                            listeDeReglement = liste;
                            foreach (CsReglement reg in liste)
                                this.CmbCollector.Items.Add(reg.NOMCAISSIERE);
                        }
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



            #endregion
        }

        protected void CmbCollector_SelectedIndexChanged(object sender, EventArgs e)
        {
            int loaderHandler = LoadingManager.BeginLoading("Please Wait for pass payment ... ");
            ReportServiceClient service = new ReportServiceClient(Utility.Protocole(), Utility.EndPoint("Report"));
            CsReglement reglementSelection = listeDeReglement.ElementAt(CmbCollector.SelectedIndex);
            List<CsReglement> liste = new List<CsReglement>();

            service.SelectionDateParAgentAsync(reglementSelection.MATRICULE.ToString());
            service.SelectionDateParAgentCompleted += (er, res) =>
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
                        liste = res.Result;

                        //List<CsReglement> liste = service.SelectionAgentParNom(Matricule);
                        if (liste != null || liste.Count > 0)
                        {
                            listeDeReglement = liste;
                            foreach (CsReglement reg in liste)
                                this.CmbDatePayment.Items.Add(reg.DATEENCAISSEMENT.ToString());
                        }
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

        public  List<ServiceReport.CsReglement> GetPrintObjects(string module)
        {
            try
            {
                string MatriculeCollector = (string.IsNullOrEmpty(CmbCollector.SelectedValue.ToString())) ? null : ((CsReglement)listeDeReglement.ElementAt(CmbCollector.SelectedIndex)).MATRICULE.ToString();
                string Date = (string.IsNullOrEmpty(CmbDatePayment.SelectedValue.ToString())) ? null : ((CsReglement)listeDeDates.ElementAt(CmbDatePayment.SelectedIndex)).DATEENCAISSEMENT.Value.ToShortDateString();




                int loaderHandler = LoadingManager.BeginLoading("Please Wait for pass payment ... ");
                ReportServiceClient service = new ReportServiceClient(Utility.Protocole(), Utility.EndPoint(module));

                List<ServiceReport.CsReglement> reportPrint = new List<CsReglement>();


                service.ManualDailyPaymentAsync(MatriculeCollector, Date, null, null);
                service.SelectionDateParAgentCompleted += (er, res) =>
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

