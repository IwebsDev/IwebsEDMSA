using Galatee.Silverlight.ServiceReport;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
    public partial class UcCurrentPaymentsDetails : ChildWindow
    {
        public UcCurrentPaymentsDetails()
        {
            InitializeComponent();
            listeDesCaissieresCochees = new List<string>();
            listeDesCaissieres = new List<Galatee.Silverlight.ServiceReport.CParametre>();

            //FillCollector();
            //FillMonths();
        }

       
















         ObservableCollection<CsArrete> listeMois_ = new ObservableCollection<CsArrete>();
        ObservableCollection<CParametre> listeDesCaissieres_ = new ObservableCollection<CParametre>();
        //public event PropertyChangedEventHandler PropertyChanged;

        List<string> listeDesCaissieresCochees=new List<string>();
        List<CsArrete> listeMois=new List<CsArrete>();
        List<CsArrete> listeMoisCoche=new List<CsArrete>();
        List<Galatee.Silverlight.ServiceReport.CParametre> listeDesCaissieres =new List<CParametre>();

        string Edition = string.Empty;
        string Etatafficher = string.Empty;
        //private List<string> months = new List<string>() { "", "JANUARY", "FEBRUARY", "MARCH", "APRIL", "MAY", "JUNE", "JULY", "AUGUST", "SEPTEMBER", "OCTOBER", "NOVEMBER", "DECEMBER" };
        List<int> MoisChoisi = new List<int>();
        int annee;
        string DefinedRDLC;



        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            GetPrintObjects("Report");
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }       
        
        public List<Galatee.Silverlight.ServiceReport.CsReglement> GetPrintObjects(string module)
        {
            try
            {
                DateTime? CollectionDate = DateTime.Now.Date;
                annee = CollectionDate.Value.Year;
                MoisChoisi.Add(CollectionDate.Value.Month);
                string key = Utility.getKey();
                //Matricule de l'agent courant
                listeDesCaissieresCochees.Add(UserConnecte.matricule);
                //if (Txt_DateDebut.IsEnabled==false)
                //{
                //    annee = (this.Txt_year.Text == string.Empty) ? 0 : int.Parse(this.Txt_year.Text);
                //}
                //else
                //{
                //    annee = (Txt_DateDebut.SelectedDate != null) ? Txt_DateDebut.SelectedDate.Value.Year : 0;
                //}
                
                string reportName = "PassPaymentDetail";
                this.DefinedRDLC = reportName;


                int loaderHandler = LoadingManager.BeginLoading("Please Wait for current payment ... ");
                List<ServiceReport.CsReglement> reportPrint=new List<ServiceReport.CsReglement>();
                List<Galatee.Silverlight.ServiceReport.CsReglement> ligne = new List<Galatee.Silverlight.ServiceReport.CsReglement>();

                //Doit tenir compte des mois coché
                //GetAllSelectedCheckObjItem();
               

                List<Galatee.Silverlight.ServiceCaisse.CsReglement> ListeDesFacture = new List<Galatee.Silverlight.ServiceCaisse.CsReglement>();


                ReportServiceClient service = new ReportServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint(module));
                service.RPT_ETAT_ENCAISSEMENT_BY_COLLECTOR_CURRENTEAsync(listeDesCaissieresCochees, CollectionDate, annee, MoisChoisi, Edition,key, null);
                service.RPT_ETAT_ENCAISSEMENT_BY_COLLECTOR_CURRENTECompleted += (er, res) =>
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
                        //if (res.Result != null)
                        //{
                        //    //Code en cas de succès
                        //    reportPrint = res.Result;
                        //    //Dictionary<string, string> param = new Dictionary<string, string>();
                        //    Dictionary<string, string> param = null;
                          

                            //Effectue l'aperçcu avant imprèssion
                        Utility.ActionPreview<ServicePrintings.CsReglement>(null, this.DefinedRDLC, module, key);
                            //Utility.ActionPreview<ServicePrintings.CsReglement, Galatee.Silverlight.ServiceReport.CsReglement>(reportPrint, param, Etatafficher, module);
                        //}
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
                //return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //private void FillCollector()
        //{
        //    try
        //    {

        //        int loaderHandler = LoadingManager.BeginLoading("Please Wait for pass payment ... ");
        //        ddbCollecter.Items.Clear();
        //        ReportServiceClient service = new ReportServiceClient(Utility.Protocole(), Utility.EndPoint("Report"));
        //        service.SELECT_ALL_COLLECTORSAsync();
        //        service.SELECT_ALL_COLLECTORSCompleted += (er, res) =>
        //        {
        //            try
        //            {
        //                if (res.Error != null || res.Cancelled)
        //                {
        //                    LoadingManager.EndLoading(loaderHandler);
        //                    throw new Exception("Cannot display report");
        //                }

        //                if (res.Result != null)
        //                {
        //                    //Code en cas de succès
        //                    foreach (var item in res.Result)
        //                    {
        //                        listeDesCaissieres_.Add(item);
        //                    }
        //                    ddbCollecter.ItemsSource = listeDesCaissieres_;
        //                    //ddbCollecter.Items.Add("");
        //                    //foreach (Galatee.Silverlight.ServiceReport.CParametre collecteur in listeDesCaissieres)
        //                    //    ddbCollecter.Items.Add(collecteur.LIBELLE);
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                MessageBox.Show(ex.Message);
        //            }
        //            finally
        //            {
        //                LoadingManager.EndLoading(loaderHandler);
        //            }
        //        };
        //    }
        //    catch (Exception)
        //    { }
        //}

        //private void FillMonths()
        //{
        //    try
        //    {
        //        int loaderHandler = LoadingManager.BeginLoading("Please Wait for pass payment ... ");
        //        ddbMois.Items.Clear();
        //        ReportServiceClient service = new ReportServiceClient();
        //        service.GetALLMoisComptableAsync();
        //        service.GetALLMoisComptableCompleted += (er, res) =>
        //        {
        //            try
        //            {
        //                if (res.Error != null || res.Cancelled)
        //                {
        //                    LoadingManager.EndLoading(loaderHandler);
        //                    throw new Exception("Cannot display report");
        //                }

        //                if (res.Result != null)
        //                {
        //                    //Code en cas de succès
        //                    //listeMois.Add(new CsArrete());
        //                    listeMois.AddRange( res.Result);
        //                    foreach (var item in res.Result)
        //                    {
        //                        listeMois_.Add(item);
        //                    }

        //                    ddbMois.ItemsSource =listeMois_;
        //                    //foreach (CsArrete mois in listeMois)
        //                    //    ddbMois.Items.Add(new { ANNMOIS = mois.ANNMOIS });
                            
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                MessageBox.Show(ex.Message);
        //            }
        //            finally
        //            {
        //                LoadingManager.EndLoading(loaderHandler);
        //            }
        //        };
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }
        //}

        //protected void ddbCollecter_SelectedIndexChanged(object sender, EventArgs e)
        //{

        //}

        //protected void ddbMois_SelectedIndexChanged(object sender, EventArgs e)
        //{

        //}

        private void rdbType_Checked_1(object sender, RoutedEventArgs e)
        {
            switch (((RadioButton)sender).Name)
            {
                case "rdbTypeResumer":
                    
                     Edition = "2";
                    Etatafficher = "EncaissementParCollector";
                    break;
                case "rdbTypeResumer2":
                    
                    //Edition = "1";
                    //Etatafficher = "EncaissementParCaissiereConnectee";
                    Edition = "2";
                    Etatafficher = "EncaissementParCollectorSumary2";
                    break;
                case "rdbTypeDetail":
                    
                    //Edition = "2";
                    //Etatafficher = "EncaissementParCollectorSumary2";
                    Edition = "2";
                    Etatafficher = "EncaissementParCaissiereConnectee";
                    break;
                default:
                    break;
            }
        }

        //private ChildControl FindVisualChild<ChildControl>(DependencyObject DependencyObj)
        //where ChildControl : DependencyObject
        //{
        //    for (int i = 0; i < VisualTreeHelper.GetChildrenCount(DependencyObj); i++)
        //    {
        //        DependencyObject Child = VisualTreeHelper.GetChild(DependencyObj, i);

        //        if (Child != null && Child is ChildControl)
        //        {
        //            return (ChildControl)Child;
        //        }
        //        else
        //        {
        //            ChildControl ChildOfChild = FindVisualChild<ChildControl>(Child);

        //            if (ChildOfChild != null)
        //            {
        //                return ChildOfChild;
        //            }
        //        }
        //    }
        //    return null;
        //}

        //private void GetAllSelectedCheckObjItem()
        //{
        //    try
        //    {
        //        listeMoisCoche = listeMois_.Where(c => c.COCHER).ToList();

        //        foreach (var item in listeDesCaissieres_.Where(c => c.COCHER).ToList())
        //        {
        //            listeDesCaissieresCochees.Add(item.VALEUR);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);  
        //    }
        //}

        //private void ddbMois__SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{

        //}

        //private void CheckBox_Checked_1(object sender, RoutedEventArgs e)
        //{
        //    if (((CheckBox)sender).IsChecked==true)
        //    {
        //        Txt_DateDebut.IsEnabled = true;
        //        Txt_year.IsEnabled = false;
        //    }
           
            
        //}

        //private void CheckBox_Unchecked_1(object sender, RoutedEventArgs e)
        //{
        //    if (((CheckBox)sender).IsChecked == false)
        //    {

        //        Txt_DateDebut.IsEnabled = false;
        //        Txt_year.IsEnabled = true;
        //    }
        //}

        //private void Txt_DateDebut_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    MoisChoisi.Clear();
        //    MoisChoisi.Add(((DatePicker)Txt_DateDebut).SelectedDate.Value.Month);
        //}

    }
}

