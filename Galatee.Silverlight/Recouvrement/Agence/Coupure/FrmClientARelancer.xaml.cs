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
using Galatee.Silverlight.ServiceRecouvrement;
//using Galatee.Silverlight.ServicePrintings;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace Galatee.Silverlight.Recouvrement
{
    public partial class FrmLettreRelance : ChildWindow
    {
        List<CsCentre> lstCentreCampagne = new List<CsCentre>();
        List<CsCAMPAGNE> lesCampagne = new List<CsCAMPAGNE>();
        ObservableCollection<CsDetailCampagne> lesClientCampagne = new ObservableCollection<CsDetailCampagne>();
        List<CsSite> lstSiteCampagne = new List<CsSite>();
        List<CsSite> lstAgentCampagne = new List<CsSite>();
        public FrmLettreRelance()
        {
           try 
	        {	        
		         InitializeComponent();
                 InitialiserControle();
                 ChargerDonneeDuSite();
                 prgBar.Visibility = System.Windows.Visibility.Collapsed;
	        }
	        catch (Exception ex)
	        {
	        Message.ShowError(ex,Galatee.Silverlight.Resources.Langue.errorTitle);
	        }
        }
        List<int> lesCentreCaisse = new List<int>();
        List<Galatee.Silverlight.ServiceAccueil.CsCentre> LstCentrePerimetre = new List<ServiceAccueil.CsCentre>();
        List<Galatee.Silverlight.ServiceAccueil.CsSite> lstSite = new List<ServiceAccueil.CsSite>();
        private void ChargerDonneeDuSite()
        {
            try
            {
                List<int> lstCentreSelect = new List<int>();

                if (SessionObject.LstCentre != null && SessionObject.LstCentre.Count > 0)
                {
                    LstCentrePerimetre = Shared.ClasseMEthodeGenerique.RetourCentreByPerimetre(SessionObject.LstCentre.Where(p => p.CODE != SessionObject.Enumere.Generale).ToList(), UserConnecte.listeProfilUser);
                    lstSite = Shared.ClasseMEthodeGenerique.RetourneSiteByCentre(LstCentrePerimetre);
                    foreach (Galatee.Silverlight.ServiceAccueil.CsCentre item in LstCentrePerimetre)
                        lesCentreCaisse.Add(item.PK_ID);
                    if (lstSite.Count == 1)
                    {
                        this.Txt_CodeSite.Text = lstSite.First().CODE;
                        this.Txt_LibelleSite.Text = lstSite.First().LIBELLE;
                        this.Txt_CodeSite.Tag = lstSite.First().PK_ID;
                        this.btn_Site.IsEnabled = false;
                    }
                    return;
                }
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.ListeDesDonneesDesSiteCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    SessionObject.LstCentre = args.Result;
                    LstCentrePerimetre = Shared.ClasseMEthodeGenerique.RetourCentreByPerimetre(SessionObject.LstCentre.Where(p => p.CODE != SessionObject.Enumere.Generale).ToList(), UserConnecte.listeProfilUser);
                    lstSite = Shared.ClasseMEthodeGenerique.RetourneSiteByCentre(LstCentrePerimetre);
                    foreach (Galatee.Silverlight.ServiceAccueil.CsCentre item in LstCentrePerimetre)
                        lesCentreCaisse.Add(item.PK_ID);
                    if (lstSite.Count == 1)
                    {
                        this.Txt_CodeSite.Text = lstSite.First().CODE;
                        this.Txt_LibelleSite.Text = lstSite.First().LIBELLE;
                        this.Txt_CodeSite.Tag = lstSite.First().PK_ID;
                        this.btn_Site.IsEnabled = false;
                    }
                    return;
                };
                service.ListeDesDonneesDesSiteAsync(false);
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void btn_Site_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (lstSite.Count > 0)
                {
                    this.btn_Site.IsEnabled = false;
                    Dictionary<string, string> _LstColonneAffich = new Dictionary<string, string>();
                    _LstColonneAffich.Add("CODE", "SITE");
                    _LstColonneAffich.Add("LIBELLE", "LIBELLE");

                    List<object> obj = Shared.ClasseMEthodeGenerique.RetourneListeObjet(lstSite);
                    MainView.UcListeGenerique ctrl = new MainView.UcListeGenerique(obj, _LstColonneAffich, false, "Lots");
                    ctrl.Closed += new EventHandler(galatee_OkClickedSite);
                    ctrl.Show();
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, "Recouvrement");
            }
        }
        private void Txt_CodeSite_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(Txt_CodeSite.Text) && Txt_CodeSite.Text.Length == SessionObject.Enumere.TailleCentre)
                {
                    Galatee.Silverlight.ServiceAccueil.CsSite _LeSiteClient = Shared.ClasseMEthodeGenerique.RetourneObjectFromList(lstSite, this.Txt_CodeSite.Text, "CODE");
                    if (!string.IsNullOrEmpty(_LeSiteClient.LIBELLE))
                    {
                        this.Txt_LibelleSite.Text = _LeSiteClient.LIBELLE;
                        this.Txt_CodeSite.Text = _LeSiteClient.CODE;
                        this.Txt_CodeSite.Tag = _LeSiteClient.PK_ID;
                    }
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, "Facturation");

            }
        }
        void galatee_OkClickedSite(object sender, EventArgs e)
        {
            Galatee.Silverlight.MainView.UcListeGenerique ctrs = sender as Galatee.Silverlight.MainView.UcListeGenerique;
            if (ctrs.isOkClick)
            {
                this.btn_Site.IsEnabled = true;
                Galatee.Silverlight.ServiceAccueil.CsSite leSite = (Galatee.Silverlight.ServiceAccueil.CsSite)ctrs.MyObject;
                this.Txt_CodeSite.Text = leSite.CODE;
                this.Txt_CodeSite.Tag = leSite.PK_ID;
                this.Txt_LibelleSite.Text = leSite.LIBELLE;
            }
            this.btn_Site.IsEnabled = true;
        }
        private void InitialiserControle()
        {
            try
            {
               
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public event EventHandler Closed;

        string campaign = string.Empty;
        public bool IsGettingIdCoupure = false;
        public void ClosedEnventHandler()
        {
            if (this.Closed!=null)
	        {
                this.Closed(this, null);
	        }
        }

        public CsDetailCampagne  ClientSelect;
        public CsClient ClientRechercheSelect;

        private void ReinitialiserGrid()
        {
            try
            {
                int indexElementSelected = this.lvwResultat.SelectedIndex + 1;
                if (indexElementSelected <= lesClientCampagne.Count() - 1)
                {
                    lvwResultat.IsReadOnly = true;
                    lvwResultat.SelectedIndex = indexElementSelected;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        List<DataGridRow> Rows = new List<DataGridRow>();
        private void ChangeSelectedItemColor()
        {
            try
            {
                //to get the current row binding value
                CsDetailCampagne currentRow = (CsDetailCampagne)lvwResultat.SelectedItem;

                //to read the currentRow
                DataGridRow selectedRow = Rows[lvwResultat.SelectedIndex];
                //color row
                var backgroundRectangle = SearchFrameworkElement(selectedRow, "BackgroundRectangle") as Rectangle;
                if (backgroundRectangle != null)
                {
                    backgroundRectangle.Fill = new SolidColorBrush(Colors.Cyan);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public static FrameworkElement SearchFrameworkElement(FrameworkElement parentFrameworkElement, string childFrameworkElementNameToSearch)
        {
            try
            {
                FrameworkElement childFrameworkElementFound = null;
                SearchFrameworkElement(parentFrameworkElement, ref childFrameworkElementFound, childFrameworkElementNameToSearch);
                return childFrameworkElementFound;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static void SearchFrameworkElement(FrameworkElement parentFrameworkElement, ref FrameworkElement childFrameworkElementToFind, string childFrameworkElementName)
        {
            try
            {
                int childrenCount = VisualTreeHelper.GetChildrenCount(parentFrameworkElement);
                if (childrenCount > 0)
                {
                    FrameworkElement childFrameworkElement = null;
                    for (int i = 0; i < childrenCount; i++)
                    {
                        childFrameworkElement = (FrameworkElement)VisualTreeHelper.GetChild(parentFrameworkElement, i);
                        if (childFrameworkElement != null && childFrameworkElement.Name.Equals(childFrameworkElementName))
                        {
                            childFrameworkElementToFind = childFrameworkElement;
                            return;
                        }
                        SearchFrameworkElement(childFrameworkElement, ref childFrameworkElementToFind, childFrameworkElementName);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void btnReinitialiser_Click(object sender, EventArgs e)
        {
            try
            {
                this.lvwResultat.ItemsSource = null;
                //this.cmbCentre.SelectedItem = lCentre.First();
                this.dtpDate.Text = null;
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, Galatee.Silverlight.Resources.Langue.errorTitle);
            }
        }
         private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            ClosedEnventHandler();
            this.DialogResult = false;
        }
        private void btnsearch_Click(object sender, RoutedEventArgs e)
         {
             try
             {
                 string CodeSite = string.Empty; string IdCampagne = string.Empty; int IdPia = 0; DateTime? DateDebut = null; DateTime? DateFin = null; string Centre = null; string Client = null; string Ordre = null;
                 if (!string.IsNullOrEmpty(this.Txt_CodeSite.Text)) CodeSite = this.Txt_CodeSite.Text;
                 if (!string.IsNullOrEmpty(this.Txt_NumCampagne.Text)) IdCampagne = this.Txt_NumCampagne.Text;
                 if (cmbAgent.SelectedItem != null) IdPia = ((CsUtilisateur)cmbAgent.SelectedItem).PK_ID;
                 if (this.dtpDate.SelectedDate != null) DateDebut = this.dtpDate.SelectedDate.Value;
                 if (this.dtpDateFin.SelectedDate != null) DateFin = this.dtpDateFin.SelectedDate.Value;
                 if (!string.IsNullOrEmpty(this.txtReferenceClient.Text)) Client = this.txtReferenceClient.Text;
                 if (!string.IsNullOrEmpty(this.txtOrdeClient.Text)) Ordre = this.txtOrdeClient.Text;
                 Recherche(CodeSite, IdCampagne, IdPia, DateDebut, DateFin, Centre, Client, Ordre);
             }
             catch (Exception ex)
             {
                 Message.ShowError(ex, Galatee.Silverlight.Resources.Langue.errorTitle);
             }
         }
        void Recherche(string CodeSite, string IdCampagne, int IdPia, DateTime? DateDebut, DateTime? DateFin, string Centre, string Client, string Ordre)
        {
            try
            {
                RecouvrementServiceClient client = new RecouvrementServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Recouvrement"));
                client.RechercheClientCampagneCompleted += (ss, args) =>
                {
                    try
                    {
                        if (args.Cancelled || args.Error != null)
                        {
                            string error = args.Error.Message;
                            Message.ShowError("Erreur à l'exécution du service", "SearchCampagne");
                            return;
                        }

                        if (args.Result == null || args.Result.Count == 0)
                        {
                            Message.ShowInformation("Aucune donnée trouvée", "SearchCampagne");
                            return;
                        }

                        List<CsDetailCampagne> detailcampagnes = new List<CsDetailCampagne>();
                        detailcampagnes = args.Result;
                        lesClientCampagne.Clear();
                        foreach (CsDetailCampagne item in detailcampagnes)
                            lesClientCampagne.Add(item);

                        this.lvwResultat.ItemsSource = null;
                        this.lvwResultat.ItemsSource = lesClientCampagne;

                        OKButton.Visibility = System.Windows.Visibility.Visible;

                    }
                    catch (Exception ex)
                    {
                        Message.ShowError(ex, Galatee.Silverlight.Resources.Langue.errorTitle);
                    }
                };
                client.RechercheClientCampagneAsync(CodeSite, IdCampagne, IdPia, DateDebut, DateFin, Centre, Client, Ordre, 4);


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

         private void lvwResultat_SelectionChanged(object sender, SelectionChangedEventArgs e)
         {
             try
             {
                 ClientSelect = lvwResultat.SelectedItem as CsDetailCampagne;
                 if (ClientSelect != null)
                 {
                     if (ClientSelect.IsSelect == false )
                       ClientSelect.IsSelect = true;

                    else  if (ClientSelect.IsSelect == true )
                         ClientSelect.IsSelect = false;
                 }
             }
             catch (Exception ex)
             {
              Message.ShowError(ex, Galatee.Silverlight.Resources.Langue.errorTitle);
             }
         }

         private void btnOk_Click(object sender, RoutedEventArgs e)
         {
             try
             {
                 if (Closed != null)
                 {
                     Closed(this, new EventArgs());
                     this.DialogResult = true;
                 }
             }
             catch (Exception ex)
             {
              Message.ShowError(ex, Galatee.Silverlight.Resources.Langue.errorTitle);
             }
         }

         private void btnreset_Click(object sender, RoutedEventArgs e)
         {

         }

         private void cmbSite_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
         {
             try
             {
                 
             }
             catch (Exception ex)
             {
                 Message.ShowError(ex, Galatee.Silverlight.Resources.Langue.errorTitle);
             }
         }

         private void cmbCampagne_SelectionChanged(object sender, SelectionChangedEventArgs e)
         {
         }

         private void dgMyDataGrid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
         {
             DataGrid dg = (sender as DataGrid);
             //var allObjects = ((ObservableCollection<CsDetailCampagne>)dg.ItemsSource).ToList();
             //foreach (var item in allObjects)
             //    item.IsSelect = false;
             if (dg.SelectedItem != null)
             {
                 CsDetailCampagne SelectedObject = (CsDetailCampagne)dg.SelectedItem;
                 if (SelectedObject.IsSelect == false)
                     SelectedObject.IsSelect = true;
                 else
                     SelectedObject.IsSelect = false;
             }
         }
         private void chk_Unchecked_1(object sender, RoutedEventArgs e)
         {
             var lst = ((ObservableCollection<CsDetailCampagne>)lvwResultat.ItemsSource).ToList();
             if (lst != null && this.lvwResultat.SelectedItem != null)
             {
                 CsDetailCampagne laSelect = (CsDetailCampagne)this.lvwResultat.SelectedItem;
                 if (laSelect == null)
                 {
                     Message.ShowInformation("Sélectionner la catégorie", "Index");
                     return;
                 }
                 laSelect.IsSelect = false;
             }
         }

         private void chk_Checked_1(object sender, RoutedEventArgs e)
         {
             try
             {
                 if (lvwResultat.ItemsSource != null)
                 {
                     var lst = ((ObservableCollection<CsDetailCampagne>)lvwResultat.ItemsSource).ToList();
                     if (lst != null && this.lvwResultat.SelectedItem != null)
                     {
                         CsDetailCampagne laSelect = (CsDetailCampagne)this.lvwResultat.SelectedItem;
                         if (laSelect == null)
                         {
                             Message.ShowInformation("Sélectionner la catégorie", "Index");
                             return;
                         }
                         laSelect.IsSelect = true;
                     }
                 }
             }
             catch (Exception ex)
             {
                 Message.ShowError(ex.Message, "Erreur");
             }
         }
         private void UpdateRelance(List<CsDetailCampagne> lstDetailCampagne,bool IsExport)
         {
             RecouvrementServiceClient client = new RecouvrementServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Recouvrement"));
             client.UpdateDetailCampagneSuiteRelanceAsync(lstDetailCampagne);
             client.UpdateDetailCampagneSuiteRelanceCompleted += (ss, args) =>
             {
                 try
                 {
                     if (args.Cancelled || args.Error != null)
                     {
                         string error = args.Error.Message;
                         Message.ShowError("Erreur à l'exécution du service", "SearchCampagne");
                         return;
                     }

                     if (args.Result == true )
                     {
                     if (IsExport)
                         Utility.ActionExportation<ServicePrintings.CsDetailCampagne, ServiceRecouvrement.CsDetailCampagne>(lstDetailCampagne, null, string.Empty, SessionObject.CheminImpression, "LettreDeRelance", "Recouvrement", true, "Word");
                      else
                         Utility.ActionDirectOrientation<ServicePrintings.CsDetailCampagne, ServiceRecouvrement.CsDetailCampagne>(lstDetailCampagne, null,SessionObject.CheminImpression, "LettreDeRelance", "Recouvrement", true);
                     }
                     OKButton.Visibility = System.Windows.Visibility.Visible;
                 }
                 catch (Exception ex)
                 {
                     this.btnsearch.IsEnabled = true;
                     Message.ShowError(ex, Galatee.Silverlight.Resources.Langue.errorTitle);
                 }
             };
         }
         private void ExportFile_Click_1(object sender, RoutedEventArgs e)
         {
             List<CsDetailCampagne> lstClientEnRendezVous = lesClientCampagne.Where(t => t.IsSelect == true).ToList();
             if (lstClientEnRendezVous == null || lstClientEnRendezVous.Count == 0) return;
             UpdateRelance(lstClientEnRendezVous, true);
         }
         private void OKButton_Click(object sender, RoutedEventArgs e)
         {
             try
             {
                 List<CsDetailCampagne> lstClientEnRendezVous = lesClientCampagne.Where(t => t.IsSelect == true).ToList();
                 if (lstClientEnRendezVous == null || lstClientEnRendezVous.Count == 0) return;
                 UpdateRelance(lstClientEnRendezVous, false );
             }
             catch (Exception ex)
             {
                 Message.ShowError(ex, Galatee.Silverlight.Resources.Langue.errorTitle);
             }
         }
    }
}

