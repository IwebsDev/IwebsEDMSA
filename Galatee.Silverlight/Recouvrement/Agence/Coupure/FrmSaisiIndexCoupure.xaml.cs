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
using MessageBoxControl;

namespace Galatee.Silverlight.Recouvrement
{
    public partial class FrmSaisiIndexCoupure : ChildWindow
    {
        List<CsCentre> lstCentreCampagne = new List<CsCentre>();
        ObservableCollection<CsDetailCampagne> lesClientCampagne = new ObservableCollection<CsDetailCampagne>();
        List<CsSite> lstSiteCampagne = new List<CsSite>();
        List<CsSite> lstAgentCampagne = new List<CsSite>();
        List<CsTournee> lstTourneeCampagne = new List<CsTournee>();
        public FrmSaisiIndexCoupure()
        {
           try 
	        {	        
		         InitializeComponent();
                 InitialiserControle();
                 ChargerDonneeDuSite();
                 ChargeTypeCoupure();
                 ChargeObservation();
                 this.txt_DateReleve.Text = System.DateTime.Today.ToShortDateString();
                 prgBar.Visibility = System.Windows.Visibility.Collapsed;
	        }
	        catch (Exception ex)
	        {
	        Message.ShowError(ex,Galatee.Silverlight.Resources.Langue.errorTitle);
	        }
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
                        ChargerPiaAgence(lstSite.First().CODE);
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
                        ChargerPiaAgence(lstSite.First().CODE);
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
                        ChargerPiaAgence(_LeSiteClient.CODE);
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


        private void ChargerPiaAgence(string CodeSite)
        {
            try
            {

                Galatee.Silverlight.ServiceRecouvrement.RecouvrementServiceClient service = new Galatee.Silverlight.ServiceRecouvrement.RecouvrementServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Recouvrement"));
                service.RetournePIAAgenceCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    List<CsUtilisateur> lstPia = new List<CsUtilisateur>();
                    lstPia.Add(new CsUtilisateur(){ LIBELLE ="Aucun"});
                    lstPia.AddRange(args.Result);

                    this.cmbAgent.ItemsSource = null;
                    this.cmbAgent.ItemsSource = lstPia;
                    this.cmbAgent.DisplayMemberPath = "LIBELLE";
                    return;
                };
                service.RetournePIAAgenceAsync(CodeSite);
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public event EventHandler Closed;

      
        public void ClosedEnventHandler()
        {
            if (this.Closed!=null)
	        {
                this.Closed(this, null);
	        }
        }
        public CsDetailCampagne  ClientSelect = new CsDetailCampagne();
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
                return;
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

         void Recherche(string CodeSite, string IdCampagne, int IdPia, DateTime? DateDebut, DateTime? DateFin, string Centre, string Client, string Ordre)
         {
             try
             {
                 if (lesClientCampagne.Count != 0) lesClientCampagne.Clear();
                 this.lvwResultat.ItemsSource = null;
                 prgBar.Visibility = System.Windows.Visibility.Visible ;
                 RecouvrementServiceClient client = new RecouvrementServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Recouvrement"));
                 client.RechercheClientCampagneCompleted += (ss, args) =>
                 {
                     try
                     {
                         prgBar.Visibility = System.Windows.Visibility.Collapsed ;
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

                     }
                     catch (Exception ex)
                     {
                         Message.ShowError(ex, Galatee.Silverlight.Resources.Langue.errorTitle);
                     }
                 };
                 client.RechercheClientCampagneAsync(CodeSite, IdCampagne, IdPia, DateDebut, DateFin, Centre, Client, Ordre,1);


             }
             catch (Exception ex)
             {
                 throw ex;
             }
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
                 if (lesClientCampagne.Count != 0) lesClientCampagne.Clear();
                 this.lvwResultat.ItemsSource = null;

                 Recherche(CodeSite, IdCampagne, IdPia, DateDebut, DateFin, Centre, Client, Ordre);
             }
             catch (Exception ex)
             {
              Message.ShowError(ex, Galatee.Silverlight.Resources.Langue.errorTitle);
             }
         }

         private void lvwResultat_SelectionChanged(object sender, SelectionChangedEventArgs e)
         {
             try
             {
                 ClientSelect = lvwResultat.SelectedItem as CsDetailCampagne;
                 ChangeSelectedItemColor();

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


         private void dgMyDataGrid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
         {
             DataGrid dg = (sender as DataGrid);
             var allObjects = dg.ItemsSource as List<CsLclient>;
             if (dg.SelectedItem != null)
             {
                 CsDetailCampagne SelectedObject = (CsDetailCampagne)dg.SelectedItem;

                 if (SelectedObject.IsSelect == false)
                 {
                     SelectedObject.IsSelect = true;
                 }
                 else
                     SelectedObject.IsSelect = false;
             }
         }

         private void dtp_DateRendezVous_CalendarClosed(object sender, RoutedEventArgs e)
         {
             if (lvwResultat.SelectedItem != null)
             {
                 CsDetailCampagne SelectedObject = (CsDetailCampagne)lvwResultat.SelectedItem;
                 SelectedObject.DATERENDEZVOUS = dtpDate.SelectedDate  ;
                 ReinitialiserGrid();
             }
         }
         List<Galatee.Silverlight.ServiceAccueil.CsTypeCoupure> lstTypeCoupure = new List<Galatee.Silverlight.ServiceAccueil.CsTypeCoupure>();
         private void ChargeTypeCoupure()
         {
             try
             {
                 Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient client = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                 client.RetourneTypeCoupureAsync();
                 client.RetourneTypeCoupureCompleted += (es, result) =>
                 {
                     try
                     {
                         if (result.Cancelled || result.Error != null)
                         {
                             string error = result.Error.Message;
                             Message.ShowError("Erreur à l'exécution du service", "SelectCentreCampagne");
                             return;
                         }

                         if (result.Result == null)
                         {
                             Message.ShowInformation("Aucune donnée trouvée", "SelectCentreCampagne");
                             return;
                         }
                         lstTypeCoupure = result.Result.Where(t => t.CODE != "BT" && t.CODE != "MT").ToList(); 
                         this.cbo_TypeCoupure.ItemsSource = null;
                         this.cbo_TypeCoupure.ItemsSource = lstTypeCoupure ;
                         this.cbo_TypeCoupure.DisplayMemberPath = "LIBELLE";
                     }
                     catch (Exception ex)
                     {
                         Message.ShowError(ex, Galatee.Silverlight.Resources.Langue.errorTitle);
                     }
                 };

             }
             catch (Exception ex)
             {
                 throw ex;
             }
         }

         List<CsObservation> lstObservation = new List<CsObservation>();
         private void ChargeObservation()
         {
             try
             {
                 RecouvrementServiceClient client = new RecouvrementServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Recouvrement"));
                 client.RetourneObservationAsync();
                 client.RetourneObservationCompleted += (es, result) =>
                 {
                     try
                     {
                         if (result.Cancelled || result.Error != null)
                         {
                             string error = result.Error.Message;
                             Message.ShowError("Erreur à l'exécution du service", "SelectCentreCampagne");
                             return;
                         }

                         if (result.Result == null)
                         {
                             Message.ShowInformation("Aucune donnée trouvée", "SelectCentreCampagne");
                             return;
                         }
                         lstObservation = result.Result;
                         this.cbo_Observation.ItemsSource = null;
                         this.cbo_Observation.ItemsSource = lstObservation;
                         this.cbo_Observation.DisplayMemberPath = "LIBELLE";
                     }
                     catch (Exception ex)
                     {
                         Message.ShowError(ex, Galatee.Silverlight.Resources.Langue.errorTitle);
                     }
                 };

             }
             catch (Exception ex)
             {
                 throw ex;
             }
         }

         private void btn_Valider_Click(object sender, RoutedEventArgs e)
         {
             if (lvwResultat.SelectedItem != null &&
                 this.cbo_TypeCoupure.SelectedItem != null &&
                 !string.IsNullOrEmpty(this.txt_DateReleve.Text))
             {
                 CsDetailCampagne LeEvtSelect = (CsDetailCampagne)this.lvwResultat.SelectedItem;
                 if (LeEvtSelect.FRAIS != null && LeEvtSelect.FRAIS != 0 &&
                     LeEvtSelect.FRAIS != ((Galatee.Silverlight.ServiceAccueil.CsTypeCoupure)cbo_TypeCoupure.SelectedItem).COUT)
                 {
                     string Message = "Les frais de " + LeEvtSelect.FRAIS + " ont déja été injectés dans le compte de ce client" + "\n\r" +
                                                                           "Voulez vous le remplacer par "+((Galatee.Silverlight.ServiceAccueil.CsTypeCoupure)cbo_TypeCoupure.SelectedItem).COUT;

                     var ws = new MessageBoxControl.MessageBoxChildWindow("Index " ,Message, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Information);
                     ws.OnMessageBoxClosed += (l, results) =>
                     {
                         if (ws.Result == MessageBoxResult.OK)
                         {
                             LeEvtSelect.INDEX = string.IsNullOrEmpty(this.txt_index.Text) ? 0 : int.Parse(this.txt_index.Text);
                             LeEvtSelect.DATECOUPURE = Convert.ToDateTime(this.txt_DateReleve.Text);
                             LeEvtSelect.DATERDVCLIENT = this.txt_DateReleve.Text;
                             LeEvtSelect.TYPECOUPURE = ((Galatee.Silverlight.ServiceAccueil.CsTypeCoupure)cbo_TypeCoupure.SelectedItem).CODE;
                             LeEvtSelect.FRAIS = ((Galatee.Silverlight.ServiceAccueil.CsTypeCoupure)cbo_TypeCoupure.SelectedItem).COUT;
                             LeEvtSelect.FK_TYPECOUPURE = ((Galatee.Silverlight.ServiceAccueil.CsTypeCoupure)cbo_TypeCoupure.SelectedItem).PK_ID;
                             LeEvtSelect.MONTANTFRAIS = ((Galatee.Silverlight.ServiceAccueil.CsTypeCoupure)cbo_TypeCoupure.SelectedItem).COUT;
                             LeEvtSelect.MATRICULE = UserConnecte.matricule;
                             LeEvtSelect.FK_IDOBSERVATION = null;
                             if (this.cbo_Observation.SelectedItem != null)
                                 LeEvtSelect.FK_IDOBSERVATION = ((CsObservation)this.cbo_Observation.SelectedItem).PK_ID;

                             LeEvtSelect.FK_IDCOPER = SessionObject.LstDesCopers.First(t => t.CODE == SessionObject.Enumere.CoperFRP).PK_ID;
                             LeEvtSelect.FK_IDADMUTILISATEUR = UserConnecte.PK_ID;
                             LeEvtSelect.FK_IDLIBELLETOP = SessionObject.LstDesLibelleTop.First(t => t.CODE == SessionObject.Enumere.TopMoratoire).PK_ID;
                             LeEvtSelect.ISNONENCAISSABLE = true;
                             List<CsDetailCampagne> lstDetCampagne = new List<CsDetailCampagne>();
                             lstDetCampagne.Add(LeEvtSelect);
                             InsertionIndexCoupure(lstDetCampagne);
                             return;
                         }
                         else
                             return;
                     };
                     ws.Show();
                 }
                 LeEvtSelect.INDEX =string.IsNullOrEmpty(this.txt_index.Text ) ? 0 : int.Parse(this.txt_index.Text);
                 LeEvtSelect.DATECOUPURE = Convert.ToDateTime(this.txt_DateReleve.Text);
                 LeEvtSelect.DATERDVCLIENT = this.txt_DateReleve.Text;
                 LeEvtSelect.TYPECOUPURE = ((Galatee.Silverlight.ServiceAccueil.CsTypeCoupure)cbo_TypeCoupure.SelectedItem).CODE;
                 LeEvtSelect.FRAIS = ((Galatee.Silverlight.ServiceAccueil.CsTypeCoupure)cbo_TypeCoupure.SelectedItem).COUT;
                 LeEvtSelect.FK_TYPECOUPURE = ((Galatee.Silverlight.ServiceAccueil.CsTypeCoupure)cbo_TypeCoupure.SelectedItem).PK_ID;
                 LeEvtSelect.MONTANTFRAIS = ((Galatee.Silverlight.ServiceAccueil.CsTypeCoupure)cbo_TypeCoupure.SelectedItem).COUT;
                 LeEvtSelect.MATRICULE = UserConnecte.matricule;
                 LeEvtSelect.FK_IDOBSERVATION = null;
                 if (this.cbo_Observation.SelectedItem != null)
                     LeEvtSelect.FK_IDOBSERVATION = ((CsObservation)this.cbo_Observation.SelectedItem).PK_ID;

                 LeEvtSelect.FK_IDCOPER = SessionObject.LstDesCopers.First(t => t.CODE == SessionObject.Enumere.CoperFRP).PK_ID;
                 LeEvtSelect.FK_IDADMUTILISATEUR = UserConnecte.PK_ID;
                 LeEvtSelect.FK_IDLIBELLETOP = SessionObject.LstDesLibelleTop.First(t => t.CODE == SessionObject.Enumere.TopMoratoire).PK_ID;
                 LeEvtSelect.ISNONENCAISSABLE = true;
                 List<CsDetailCampagne> lstDetCampagnes = new List<CsDetailCampagne>();
                 lstDetCampagnes.Add(LeEvtSelect);
                 InsertionIndexCoupure(lstDetCampagnes);
             }
             else
             {
                 if (this.cbo_TypeCoupure.SelectedItem == null)
                     Message.ShowInformation("Saisir le type de coupure", "Recouvrement");
                 if (string.IsNullOrEmpty(this.txt_DateReleve.Text))
                     Message.ShowInformation("Saisir la date de relève", "Recouvrement");

                 return;
             }


         }
         int NbrElet = 0;
         private void InsertionIndexCoupure(List<CsDetailCampagne> Lst)
         {
             try
             {
                 RecouvrementServiceClient client = new RecouvrementServiceClient(Utility.ProtocoleIndex(), Utility.EndPoint("Recouvrement"));
                 client.InsertListIndexCompleted += (ss, ress) =>
                 {
                     try
                     {
                         if (ress.Cancelled || ress.Error != null)
                         {
                             Message.ShowError("Erreur survenue lors de l'appel service", "Informations");
                             return;
                         }

                         if (ress.Result == null)
                         {
                             Message.ShowInformation("Erreur lors de l'insertion des index de campange! Veuillez réessayer svp ", "Informations");
                             return;
                         }
                         NbrElet++;
                         if (NbrElet == 5)
                             RemoveElementGridApresInsert();
                         else
                             ReloadDatagraid();

                         this.txt_index.Text = string.Empty;
                         this.txt_index.Focus();
                     }
                     catch (Exception ex)
                     {
                         Message.ShowError(ex, "Erreur");
                     }
                 };
                 client.InsertListIndexAsync(Lst);
             }
             catch (Exception ex)
             {
                 throw ex;
             }
         }
         private void cbo_TypeCoupure_SelectionChanged(object sender, SelectionChangedEventArgs e)
         {

         }
         private void dtpDteCoupure_CalendarClosed(object sender, RoutedEventArgs e)
         {
         }

         private void txt_index_LostFocus(object sender, RoutedEventArgs e)
         {
         }

         

         private void cbo_Observation_SelectionChanged(object sender, SelectionChangedEventArgs e)
         {

         }

         private void ChildWindow_KeyDown_1(object sender, KeyEventArgs e)
         {
             if (e.Key.Equals(Key.Enter))
                 btn_Valider_Click(null, null);
         }

         private void Txt_NumCampagne_TextChanged(object sender, TextChangedEventArgs e)
         {

         }

         private void cmbAgent_SelectionChanged(object sender, SelectionChangedEventArgs e)
         {
             if (cmbAgent.SelectedItem != null )
             {
                 this.txtAgent.Tag = null;
                 CsUtilisateur leUser = ((CsUtilisateur)cmbAgent.SelectedItem);
                 if (leUser.LIBELLE != "Aucun")
                 {
                     this.txtAgent.Text = leUser.MATRICULE;
                     this.txtAgent.Tag = leUser.PK_ID;
                 }
             }
         }

         private void RemoveElementGridApresInsert()
         {
             try
             {
                 lvwResultat.IsReadOnly = false;
                 ObservableCollection<CsDetailCampagne> listCouranteDansLaGridTemp = new ObservableCollection<CsDetailCampagne>();

                 List<CsDetailCampagne> lstNonSaisie = lesClientCampagne.Where(f => f.DATECOUPURE == null ).ToList();
                 foreach (CsDetailCampagne item in lstNonSaisie)
                     listCouranteDansLaGridTemp.Add(item);
                 lesClientCampagne = listCouranteDansLaGridTemp;
                 lvwResultat.ItemsSource = null;
                 lvwResultat.ItemsSource = lesClientCampagne;
                 if (lesClientCampagne.Count != 0)
                     lvwResultat.SelectedItem = lesClientCampagne.First();
             }
             catch (Exception ex)
             {
                 throw ex;
             }
         }

         private void ReloadDatagraid()
         {
             try
             {
                 lvwResultat.IsReadOnly = false;
                 ObservableCollection<CsDetailCampagne> listCouranteDansLaGridTemp = new ObservableCollection<CsDetailCampagne>();

                 List<CsDetailCampagne> lstNonSaisie = lesClientCampagne.Where(f => f.DATECOUPURE == null).ToList();
                 foreach (CsDetailCampagne item in lstNonSaisie)
                     listCouranteDansLaGridTemp.Add(item);
                 if (listCouranteDansLaGridTemp.Count != 0)
                     lvwResultat.SelectedItem = listCouranteDansLaGridTemp.First();
             }
             catch (Exception ex)
             {
                 throw ex;
             }
         }
    }
}

