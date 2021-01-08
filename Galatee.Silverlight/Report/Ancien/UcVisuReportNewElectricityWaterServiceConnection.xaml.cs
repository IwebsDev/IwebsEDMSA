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
    public partial class UcVisuReportNewElectricityWaterServiceConnection : ChildWindow
    {
        int IdProd = 0;
        public string DefinedRDLC { get; set; }
        bool Etat = false;
        public UcVisuReportNewElectricityWaterServiceConnection()
        {
            InitializeComponent();
            LoadProduit();
        }

        private void LoadProduit()
        {

            ReportServiceClient service = new ReportServiceClient();
            List<ServiceReport.CsProduitFacture> reportPrint = new List<CsProduitFacture>();

            int loaderHandler = LoadingManager.BeginLoading("Traitement en cours ... ");
            service.RetourneTousProduitAsync();
            service.RetourneTousProduitCompleted += (er, res) =>
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
                        reportPrint.Add(new CsProduitFacture{PK_ID=0,CODE="00", LIBELLEPRODUIT="TOUS"});
                        reportPrint.AddRange(res.Result);

                        //Charger la combo
                        Cmb_Produit.ItemsSource = reportPrint;
    //                    foreach (var item in reportPrint)
    //{
    //     item.LIBELLEPRODUIT;
    //         item.PK_ID
    //}
                        Cmb_Produit.DisplayMemberPath = "LIBELLEPRODUIT";
                        Cmb_Produit.SelectedValuePath = "CODE";
                        Cmb_Produit.SelectedValue = "00";
                       

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

            this.DefinedRDLC = "ConnexionsElectricite";
            //return reportPrint;

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

        public List<ServiceReport.CsConnexion> GetPrintObjects(string module)
        {
            try
            {
                //if (Txt_Year.Text.Length != 4)
                //{
                //    return null;
                //}             
                //else
                //{
                    //IdProd =int.Parse(Cmb_Produit.SelectedValue.ToString());
                    ReportServiceClient service = new ReportServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint(module));
                    //string mois = (Cmb_month.SelectedIndex < 0) ? null : (Cmb_month.SelectedIndex + 1).ToString("00");
                    List<ServiceReport.CsConnexion> reportPrint = new List<CsConnexion>();

                    int loaderHandler = LoadingManager.BeginLoading("Please Wait for pass payment ... ");
                    service.GetNewBranchementsByProduitAsync(Txt_Year.Text, IdProd, Etat);
                    service.GetNewBranchementsByProduitCompleted += (er, res) =>
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
                                reportPrint = res.Result.OrderBy(c => c.Mois).ToList();

                                Dictionary<string, string> dico = new Dictionary<string, string>();
                                dico.Add("annee", Txt_Year.Text);
                                //Effectue l'aperçcu avant imprèssion
                                //Utility.ActionPreview<ServicePrintings.CsConnexion, Galatee.Silverlight.ServiceReport.CsConnexion>(reportPrint, dico, DefinedRDLC, module);
                                Utility.ActionDirectOrientation <ServicePrintings.CsConnexion, Galatee.Silverlight.ServiceReport.CsConnexion>(reportPrint, dico,SessionObject.DefaultPrinter, "VisuReportNewElectricityServiceConnection", module,false );
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

                    //this.DefinedRDLC = "ConnexionsElectricite";
                    return reportPrint;
                //}
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void Cmb_Produit_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            IdProd =((CsProduitFacture)Cmb_Produit.SelectedItem).PK_ID;
        }

        private void Cercle_Checked(object sender, RoutedEventArgs e)
        {
            switch (((RadioButton)sender).Name)
            {

                case "Resilier":
                    Etat = false;
                    //DefinedRDLC = "VisuReportElectricityServiceConnection";
                //DefinedRDLC = "NouveauBranchementParMoi";
                    break;
                case "Actif":
                    Etat = true;
                    //DefinedRDLC = "VisuReportNewElectricityServiceConnection";
                    break;
                default:
                    break;
            }
        }

    }
}
