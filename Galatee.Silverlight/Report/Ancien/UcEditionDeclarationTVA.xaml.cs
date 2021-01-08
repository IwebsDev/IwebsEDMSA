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
    public partial class UcEditionDeclarationTVA : ChildWindow
    {
        List<CsArrete> listeMois = new List<CsArrete>();
        public string DefinedRDLC = string.Empty;
        public UcEditionDeclarationTVA()
        {
            InitializeComponent();
            FillMonths();
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

        private void FillMonths()
        {
            try
            {
                int loaderHandler = LoadingManager.BeginLoading("Please Wait for pass payment ... ");
                ddbMois.Items.Clear();
                ReportServiceClient service = new ReportServiceClient();
                service.GetALLMoisComptableAsync();
                service.GetALLMoisComptableCompleted += (er, res) =>
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
                            listeMois = res.Result;
                            ddbMois.Items.Add("");
                            foreach (CsArrete mois in listeMois)
                                ddbMois.Items.Add(mois.ANNMOIS);
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
                MessageBox.Show(ex.Message);
            }
        }

        public  List<CsDeclarationTVA> GetPrintObjects(string module)
        {
            try
            {
                string mois = string.Empty;
                if (this.ddbMois.SelectedItem != null)
                {
                    if (string.IsNullOrEmpty(ddbMois.SelectedValue.ToString()))
                        mois = null;
                    else
                        mois = listeMois.FirstOrDefault(t => t.ANNMOIS == ddbMois.SelectedValue).MOISCOMPTABLE;
                }
                //if (!string.IsNullOrEmpty(mois))
                //{
                string Annee = (string.IsNullOrEmpty(this.txtYear.Text)) ? null : this.txtYear.Text;
                string reportName = "DeclarationTVA";
                this.DefinedRDLC = reportName;
                int loaderHandler = LoadingManager.BeginLoading("Please Wait for pass payment ... ");
                ReportServiceClient service = new ReportServiceClient(Utility.Protocole(), Utility.EndPoint("Report"));
                List<CsDeclarationTVA> liste = new List<CsDeclarationTVA>(); ;

                service.RPT_DECLARATION_TVAAsync(mois, Annee, null, null);
                service.RPT_DECLARATION_TVACompleted += (er, res) =>
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
                           liste= res.Result;
                           //Dictionary<string, string> param = new Dictionary<string, string>();
                           Dictionary<string, string> param = null;
                           string key = Utility.getKey();
                           
                           //Effectue l'aperçcu avant imprèssion
                           Utility.ActionPreview<ServicePrintings.CsDeclarationTVA, CsDeclarationTVA>(liste, param, "DeclarationTVA", "Report");
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

                return liste;
                }             
            
            catch (Exception)
            {
                throw;
            }
        }
    }
}

