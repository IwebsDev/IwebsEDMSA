using Galatee.Silverlight.ServiceReport;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public partial class UcEditionImpayeSector : ChildWindow
    {
        private ObservableCollection<CsZone> categories = new ObservableCollection<CsZone>();
        string TypeEdition = string.Empty;
        string reportName = string.Empty;
        public string DefinedRDLC { get; set; }
        List<CsFacture> listefinale = new List<CsFacture>();

        Action<List<string> , string , string , string , string , Dictionary<string, string>> ServiceMethode;

        
        public UcEditionImpayeSector()
        {
            InitializeComponent();
            FillMonth();

            FillSecteurs();

            ddbSolde.Items.Add("");
            ddbSolde.Items.Add(">=");
            ddbSolde.Items.Add("<=");
            ddbSolde.Items.Add("=");
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.GetPrintObjects("Report");
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void FillMonth()
        {
            try
            {
                int loaderHandler = LoadingManager.BeginLoading("Please Wait for pass payment ... ");

                Cmb_month.Items.Clear();
                ReportServiceClient service = new ReportServiceClient();
                service.GetMoisAsync();
                service.GetMoisCompleted += (er, res) =>
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
                            var listeMois = res.Result;
                            Cmb_month.ItemsSource = listeMois;
                            Cmb_month.DisplayMemberPath = "LIBELLE";
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

        public void FillSecteurs()
        {
            try
            {
                ReportServiceClient service = new ReportServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Report"));
                //categories = service.SELECT_ALL_CATEGORY_By_NUMTABLE(12); // 12 represente la category
                //ddbSecteur.Items.Clear();
                //foreach (CsTa item in categories)
                //{
                //    string cat = item.PK_CODE.Substring(4, 2);
                //    item.PK_CODE = cat;
                //    ddbSecteur.Items.Add(cat);
                //}

                int loaderHandler = LoadingManager.BeginLoading("Please Wait for pass payment ... ");
                string key = Utility.getKey();
                service.SELECT_ALL_TOURNEEAsync();
                service.SELECT_ALL_TOURNEECompleted += (er, res) =>
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
                             foreach (var item in res.Result)
	                        {
                                categories.Add(item);
	                        } 

                            Cmb_categ.ItemsSource = categories;
                            //Cmb_categ.DisplayMemberPath = "LIBELLE";
                            //Cmb_categ.SelectedValuePath = "NUM";
                            
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

        public void GetPrintObjects(string module)
        {
            try
            {
                Dictionary<string, string> listeParam = new Dictionary<string, string>();

                string ReadingCode = string.Empty;
                string Zone = string.Empty;
               

                string ValueMois = string.Empty;
                string ValueJour = string.Empty;
                string ValueAnnee = string.Empty;

                string Date = (string.IsNullOrEmpty(Txt_DateDebut.Text)) ? null : Txt_DateDebut.Text;
                //Parameters.Add("pDate", string.IsNullOrEmpty(Date) ? string.Empty : Date);
                listeParam.Add("pDate", string.IsNullOrEmpty(Date) ? string.Empty : Date);
                

                int Operation = ddbSolde.SelectedIndex;


                int ValueMontant = 0;
                ValueMois = (Cmb_month.SelectedIndex <= 0) ? null : (Cmb_month.SelectedIndex + 1).ToString("00");
                ValueJour = Date;

                if (this.txtMarge.Text != string.Empty)
                    ValueMontant = int.Parse(this.txtMarge.Text);

                string Annee = null;
                if (!string.IsNullOrEmpty(this.txtYear.Text))
                    if (string.IsNullOrEmpty(ValueMois))
                        Annee = this.txtYear.Text;
                    else
                        Annee = ValueMois + "/" + this.txtYear.Text;

                //Parameters.Add("pAnnee", Annee);
                listeParam.Add("pAnnee", Annee);

                if (this.txtYear.Text != string.Empty)
                    ValueAnnee = this.txtYear.Text;

                List<CsZone> ListeDesCategoriesSelectionnee = new List<CsZone>();

                foreach (var item in categories)
                {
                    if (item.COCHER)
                        ListeDesCategoriesSelectionnee.Add((categories.FirstOrDefault(c => c.Code == item.Code)));
                }

                if (ListeDesCategoriesSelectionnee.Count == 0)
                {
                    ListeDesCategoriesSelectionnee.Add(new CsZone());
                }
                List<string> ListCodeCategorie = new List<string>();
                // Le nom du report n'a pas encore ete specifié
                foreach (CsZone UneCategorie in ListeDesCategoriesSelectionnee)
                {
                    ListCodeCategorie.Add(UneCategorie.Code);
                  
                }
                ServiceMethode(ListCodeCategorie, TypeEdition, ValueJour, ValueMois, ValueAnnee, listeParam);
                

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void SPX_RPT_SELECT_FACTURES_TOURNEE_DETAIL(List<string> Listcategorie, string TypeEdition, string ValueJour, string ValueMois, string ValueAnnee, Dictionary<string, string> listeParam)
        {
            int loaderHandler = LoadingManager.BeginLoading("Please Wait for catégorie detail ... ");
            ReportServiceClient service = new ReportServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Report"));

            //List<ServiceReport.CsFacture> liste = service.SPX_RPT_AREAS_CATEGORY_DETAIL(categorie, TypeEdition, ValueJour, ValueMois, ValueAnnee, null, listeParam);
            List<ServiceReport.CsFacture> liste = new List<CsFacture>();
            string key = Utility.getKey();
            service.SPX_RPT_SELECT_FACTURES_TOURNEE_DETAILAsync(Listcategorie, TypeEdition, ValueJour, ValueMois, ValueAnnee, key, listeParam);
            service.SPX_RPT_SELECT_FACTURES_TOURNEE_DETAILCompleted += (er, res) =>
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

                    liste = res.Result;
                    
                    List<ServiceReport.CsFacture> reportPrint = new List<CsFacture>();

                    if (this.ddbSolde.SelectedValue != null && this.ddbSolde.SelectedValue.ToString() != string.Empty &&
                        this.txtMarge.Text != string.Empty)
                    {
                        reportPrint = ConstruireNouvelleListe(liste);
                    }
                    else
                    { reportPrint.AddRange(liste); }

                    listefinale.AddRange(reportPrint);
                    this.DefinedRDLC = reportName;

                    Utility.ActionPreview<ServicePrintings.CsFacture, ServiceReport.CsFacture>(listefinale,listeParam, this.DefinedRDLC, "Report");
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

        //private void SPX_RPT_AREAS_CATEGORY(List<string> Listcategorie, string TypeEdition, string ValueJour, string ValueMois, string ValueAnnee, Dictionary<string, string> listeParam)
        //{

        //    int loaderHandler = LoadingManager.BeginLoading("Please Wait for catégorie detail ... ");
        //    ReportServiceClient service = new ReportServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Report"));

        //    //List<ServiceReport.CsFacture> liste = service.SPX_RPT_AREAS_CATEGORY_DETAIL(categorie, TypeEdition, ValueJour, ValueMois, ValueAnnee, null, listeParam);
        //    List<ServiceReport.CsFacture> liste = new List<CsFacture>();
        //    string key = Utility.getKey();
        //    service.SPX_RPT_AREAS_CATEGORYAsync(Listcategorie, TypeEdition, ValueJour, ValueMois, ValueAnnee, key, listeParam);
        //    service.SPX_RPT_AREAS_CATEGORYCompleted += (er, res) =>
        //    {
        //        try
        //        {
        //            if (res.Error != null || res.Cancelled)
        //            {
        //                LoadingManager.EndLoading(loaderHandler);
        //                Message.ShowInformation("Erreur survenue lors de l'appel service", "ERROR");
        //                return;
        //            }
        //            if (res.Result == null)
        //            {
        //                LoadingManager.EndLoading(loaderHandler);
        //                Message.ShowInformation("Impossible d'afficher le rapport", "ERROR");
        //                return;
        //            }
                    
        //            liste = res.Result;

        //            List<ServiceReport.CsFacture> reportPrint = new List<CsFacture>();

        //            if (this.ddbSolde.SelectedValue != null && this.ddbSolde.SelectedValue.ToString() != string.Empty &&
        //                this.txtMarge.Text != string.Empty)
        //            {
        //                reportPrint = ConstruireNouvelleListe(liste);
        //            }
        //            else
        //            { reportPrint.AddRange(liste); }

        //            listefinale.AddRange(reportPrint);
        //            this.DefinedRDLC = reportName;

        //            Utility.ActionPreview<ServicePrintings.CsFacture, Galatee.Silverlight.ServiceReport.CsFacture>(listefinale, null, this.DefinedRDLC, "Report");
        //        }
        //        catch (Exception ex)
        //        {
        //            MessageBox.Show(ex.Message);
        //        }
        //        finally
        //        {
        //            LoadingManager.EndLoading(loaderHandler);
        //        }
        //    };


            
        //}

        private List<CsFacture> ConstruireNouvelleListe(List<CsFacture> rows)
        {

            List<CsFacture> NouvelleListe = new List<CsFacture>();
            foreach (CsFacture elts in rows)
            {
                int Operation = ddbSolde.SelectedIndex;
                int ValueMontant = 0;
                if (this.txtMarge.Text != string.Empty)
                    ValueMontant = int.Parse(this.txtMarge.Text);

                CsFacture c = new CsFacture();
                //switch (Operation)
                //{
                //    case 1:
                if (elts.SoldeClient >= ValueMontant)
                {
                    if (Recapitulatif.IsChecked == true)
                    {
                        c.TOURNEE = elts.TOURNEE;
                        c.SoldeClient = elts.SoldeClient;
                    }
                    else if (Detail.IsChecked == true)
                    {
                        c.CENTRE = elts.CENTRE;
                        c.CLIENT = elts.CLIENT;
                        c.ORDRE = elts.ORDRE;
                        c.NOM = elts.NOM;
                        c.TOURNEE = elts.TOURNEE;
                        c.SoldeClient = elts.SoldeClient;
                    }
                    NouvelleListe.Add(c);
                }
            }







            //List<CsFacture> NouvelleListe = new List<CsFacture>();
            //foreach (CsFacture elts in rows)
            //{
            //    int Operation = ddbSolde.SelectedIndex;
            //    int ValueMontant = 0;
            //    if (this.txtMarge.Text != string.Empty)
            //        ValueMontant = int.Parse(this.txtMarge.Text);

            //    CsFacture c = new CsFacture();
            //    switch (Operation)
            //    {
            //        case 1:
            //            if (elts.MONTANTFACTURE >= ValueMontant)
            //            {
            //                if (Recapitulatif.IsChecked==true)
            //                {
            //                    c.Categorie = elts.Categorie;
            //                    c.MONTANTFACTURE = elts.MONTANTFACTURE;
            //                }
            //                else if (Detail.IsChecked==true)
            //                {
            //                    c.CENTRE = elts.CENTRE;
            //                    c.CLIENT = elts.CLIENT;
            //                    c.ORDRE = elts.ORDRE;
            //                    c.NOM = elts.NOM;
            //                    c.Categorie = elts.Categorie;
            //                    c.MONTANTFACTURE = elts.MONTANTFACTURE;
            //                    c.TOURNEE = elts.TOURNEE;
            //                }
            //                NouvelleListe.Add(c);
            //            }
            //            break;
            //        case 2:
            //            if (elts.MONTANTFACTURE >= ValueMontant)
            //            {
            //                if (Recapitulatif.IsChecked == true)
            //                {
            //                    c.Categorie = elts.Categorie;
            //                    c.MONTANTFACTURE = elts.MONTANTFACTURE;
            //                }
            //                else if (Detail.IsChecked == true)
            //                {
            //                    c.CENTRE = elts.CENTRE;
            //                    c.CLIENT = elts.CLIENT;
            //                    c.ORDRE = elts.ORDRE;
            //                    c.NOM = elts.NOM;
            //                    c.Categorie = elts.Categorie;
            //                    c.MONTANTFACTURE = elts.MONTANTFACTURE;
            //                    c.TOURNEE = elts.TOURNEE;
            //                }
            //                NouvelleListe.Add(c);
            //            }
            //            break;
            //        case 3:
            //            if (elts.MONTANTFACTURE >= ValueMontant)
            //            {
            //                if (Recapitulatif.IsChecked == true)
            //                {
            //                    c.Categorie = elts.Categorie;
            //                    c.MONTANTFACTURE = elts.MONTANTFACTURE;
            //                }
            //                else if (Detail.IsChecked == true)
            //                {
            //                    c.CENTRE = elts.CENTRE;
            //                    c.CLIENT = elts.CLIENT;
            //                    c.ORDRE = elts.ORDRE;
            //                    c.NOM = elts.NOM;
            //                    c.Categorie = elts.Categorie;
            //                    c.MONTANTFACTURE = elts.MONTANTFACTURE;
            //                    c.TOURNEE = elts.TOURNEE;
            //                }
            //                NouvelleListe.Add(c);
            //            }
            //            break;
            //    }
            //}

            return NouvelleListe;
        }

        private void Detail_Checked(object sender, RoutedEventArgs e)
        {
            if (((RadioButton)sender).Name == "Recapitulatif")
            {
                TypeEdition = "1";
                reportName = "ImpayesTournee";
                ServiceMethode = SPX_RPT_SELECT_FACTURES_TOURNEE_DETAIL;
            }
            else if (((RadioButton)sender).Name == "Detail")
            {
                TypeEdition = "2";
                reportName = "DetailImpayesTournee";
                ServiceMethode = SPX_RPT_SELECT_FACTURES_TOURNEE_DETAIL;
            }
        }
    }
}

