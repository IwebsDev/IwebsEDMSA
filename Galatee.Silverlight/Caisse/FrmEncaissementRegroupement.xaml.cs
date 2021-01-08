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
using System.Threading;
using System.Collections.ObjectModel;
using Galatee.Silverlight.ServiceCaisse;
//using Galatee.Silverlight.serviceWeb;
using Galatee.Silverlight.Shared;
using System.ServiceModel.Description;
using Galatee.Silverlight.Resources.Caisse;
using System.Globalization;
using System.ComponentModel;
using Galatee.Silverlight.Classes;

namespace Galatee.Silverlight.Caisse
{
    public partial class FrmEncaissementRegroupement : ChildWindow, INotifyPropertyChanged
    {
       
        int debutClient = SessionObject.Enumere.TailleCentre;
        int debutOrdre = SessionObject.Enumere.TailleCentre + SessionObject.Enumere.TailleClient;
        int TailleReferenceClient = SessionObject.Enumere.TailleCentre +
                            SessionObject.Enumere.TailleClient +
                            SessionObject.Enumere.TailleOrdre;

        List<CsLclient> ListeFactureAregle = new List<CsLclient>();
        ObservableCollection<CsLclient> ListeFactureAregleAfficher = new ObservableCollection<CsLclient>();
        List<CsClient> LstClient = new List<CsClient>();
        List<CsLclient> lesEltInitial = new List<CsLclient>();
        public System.Windows.Threading.DispatcherTimer timer = new System.Windows.Threading.DispatcherTimer();
        public System.Windows.Threading.DispatcherTimer timer1 = new System.Windows.Threading.DispatcherTimer();
        
        public FrmEncaissementRegroupement()
        {
            InitializeComponent();
            try
            {
                translateControls();
                InitialiseControle();
                this.btn_Regroupement.IsEnabled = false;
                this.txtCodeRegroupement.MaxLength = SessionObject.Enumere.TailleCodeRegroupement;
                this.Txt_DateEncaissement.Text = System.DateTime.Today.Date.ToShortDateString();
                ChargerCodeRegroupement();
                RecuperationNumCaisse();
                this.txtCodeRegroupement.Focus();
                prgBar.Visibility = System.Windows.Visibility.Collapsed;


                timer.Interval = new TimeSpan(3, 0, 0);
                timer.Tick += timer_Tick;
                timer.Start();
            }
            catch (Exception ex)
            {
              Message.ShowError(ex, Langue.errorTitle);
            }        
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            Message.ShowInformation("Session de caisse fermée", "Session");
            timer.Stop();
            timer = null;
            this.Close();
        }
        private void RecuperationNumCaisse()
        {
            this.IsEnabled = false;

            CaisseServiceClient srv;
            int loadingHandler = LoadingManager.BeginLoading(Langue.msg_test_connexion);
            srv = new CaisseServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Caisse"));
            srv.RetourneNumeroRecuCompleted += (s, es) =>
            {
                try
                {
                    if (es.Error != null || es.Cancelled || es.Result == "" || es.Result == null)
                        if (SessionObject.DernierNumeroDeRecu <= -1) // Dans ce cas on n'a pas encore recuperé de numero de la journée
                        {
                            if (string.IsNullOrEmpty(es.Result)) // Si aucun numero n'est retourné, il ne s'agit surement pas d'une caissiere
                                Message.ShowError(Langue.msg_encaissement_interdit, Langue.informationTitle);
                            else
                                Message.ShowError(Langue.msg_error_dernier_numero_recu, Langue.informationTitle);

                            SessionObject.IsServerDown = true;
                            this.DialogResult = false;
                        }
                        else
                            this.IsEnabled = true;

                    else
                    {
                       
                        SessionObject.DernierNumeroDeRecu = Decimal.Parse(es.Result);
                        this.IsEnabled = true;
                    }
                }
                catch (Exception ex)
                {
                    Message.ShowError(ex, Langue.errorTitle);
                }
                finally
                {
                    LoadingManager.EndLoading(loadingHandler);
                }
            };
            srv.RetourneNumeroRecuAsync(SessionObject.LaCaisseCourante.FK_IDCAISSE,UserConnecte.matricule  );
        }

        private void translateControls()
        {
            try
            {
                this.Btn_Payement.Content = Langue.Btn_Payement;
                this.CancelButton.Content = Langue.Btn_annuler;
                this.label5.Content = Langue.label5;
                this.Lsv_ListFacture.Columns[1].Header = Langue.lbl_Centre;
                this.Lsv_ListFacture.Columns[2].Header = Langue.lbl_Client;
                this.Lsv_ListFacture.Columns[3].Header = Langue.lbl_Ordre;
                this.Lsv_ListFacture.Columns[4].Header = Langue.Period;
                this.Lsv_ListFacture.Columns[5].Header = Langue.Bill_Number;
                this.Lsv_ListFacture.Columns[6].Header = Langue.Type;
                this.Lsv_ListFacture.Columns[7].Header = Langue.Due_on;
                this.Lsv_ListFacture.Columns[8].Header = Langue.Montant_due;
                this.Lsv_ListFacture.Columns[9].Header = Langue.Transit_Payment;
                this.Lsv_ListFacture.Columns[10].Header = Langue.Amount_to_pay;
                this.BntNth.Content = Langue.Btn_Nth;
                this.Bntall.Content = Langue.Btn_all;
            }
            catch (Exception ex)
            {
              throw ex;
            }
        }            

        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                Message.ShowError(ex, Langue.errorTitle);
            }
        }

        private void dataGrid1_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            TextBlock tb = Lsv_ListFacture.Columns[11].GetCellContent(e.Row ) as TextBlock;
            if (tb != null)
                tb.LostFocus += tb_LostFocus;

            //DataGridTextColumn tb = Lsv_ListFacture.Columns[11].GetCellContent(e.Row) as DataGridTextColumn;
            //if (tb != null)
            //    tb.LostFocus += tb_LostFocus;

            
        }

        void tb_LostFocus(object sender, RoutedEventArgs e)
        {
            if (this.Lsv_ListFacture.SelectedItem != null)
            {
                CsLclient leClientSelect = (CsLclient)Lsv_ListFacture.SelectedItem;
                TextBlock txtBlock = (TextBlock)Lsv_ListFacture.Columns[11].GetCellContent(Lsv_ListFacture.SelectedItem);
                string retrivingtext = txtBlock.Text;
                //TextBox tb = Lsv_ListFacture.Columns[11].GetCellContent(e) as TextBox;
                CsLclient leClient = ListeFactureAregle.FirstOrDefault(t => t.FK_IDCLIENT == leClientSelect.FK_IDCLIENT && t.NDOC == leClientSelect.NDOC && t.REFEM == leClientSelect.REFEM);
                if (leClient != null)
                    leClient.MONTANTPAYE = Convert.ToDecimal(retrivingtext);
                    
                this.Txt_TotalAPayer.Text = ListeFactureAregle.Where(t => t.Selectionner).Sum(p => p.MONTANTPAYE).Value.ToString(SessionObject.FormatMontant);
            }
        }
        void tb_TextChanged(object sender, TextChangedEventArgs e)
        {
           
        }
        private void BntNth_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Lsv_ListFacture.ItemsSource != null)
                {
                    List<CsLclient> lesFactureSelect = ((List<CsLclient>)Lsv_ListFacture.ItemsSource);
                    lesFactureSelect.ForEach(t => t.Selectionner = false);
                    lesFactureSelect.ForEach(t => t.MONTANTPAYE  = 0 );
                    ListeFactureAregle.Where(y => y.FK_IDCLIENT == lesFactureSelect.First().FK_IDCLIENT).ToList().ForEach(t => t.Selectionner = false);
                    this.Txt_TotalAPayer.Text = ListeFactureAregle.Where(t => t.Selectionner== true).Sum(p => p.SOLDEFACTURE).Value.ToString(SessionObject.FormatMontant);
                    this.Bntall.IsEnabled = true;
                }
            }
            catch (Exception ex)
            {
                Message.Show(ex, Langue.errorTitle);
            }
        }

        private void Bntall_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Lsv_ListFacture.ItemsSource != null)
                {
                    List<CsLclient> lesFactureSelect = ((List<CsLclient>)Lsv_ListFacture.ItemsSource);
                    lesFactureSelect.ForEach(t => t.Selectionner = true);
                    lesFactureSelect.ForEach(t => t.MONTANTPAYE = t.SOLDEFACTURE);
                    ListeFactureAregle.Where(y => y.FK_IDCLIENT == lesFactureSelect.First().FK_IDCLIENT).ToList().ForEach(t => t.Selectionner = true);
                    this.Txt_TotalAPayer.Text = ListeFactureAregle.Where(t =>t.Selectionner).Sum(p => p.SOLDEFACTURE).Value.ToString(SessionObject.FormatMontant);
                    this.BntNth.IsEnabled = true;
                }
            }
            catch (Exception ex)
            {
                Message.Show(ex, Langue.errorTitle);
            }
        }
    
        private void txt_reference_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                //if (!string.IsNullOrEmpty(txt_reference.Text) && txt_reference.Text.Length == TailleReferenceClient)
                //{ 
                //     AfficherImpayes(txt_reference.Text);
                //}
            }
            catch (Exception ex)
            {
                
                throw ex;
            } 
        }


        public event PropertyChangedEventHandler PropertyChanged;
     

        private void Lsv_ListFacture_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //if (this.Lsv_ListFacture.SelectedItem != null)
            //{


            //    CsLclient leClientSelect = (CsLclient)Lsv_ListFacture.SelectedItem;
            //    CsLclient leClient = ListeFactureAregle.FirstOrDefault(t => t.FK_IDCLIENT == leClientSelect.FK_IDCLIENT && t.NDOC == leClientSelect.NDOC && t.REFEM == leClientSelect.REFEM);
            //    if (leClient != null)
            //    {
            //        if (leClient.Selectionner == false)
            //        {
            //            leClient.Selectionner = true;
            //            leClient.MONTANTPAYE = leClient.SOLDEFACTURE;
                  
            //        }
            //        else
            //        {
            //            leClient.Selectionner = false;
            //            leClient.MONTANTPAYE = null;
            //            TextBlock txtBlock = (TextBlock)Lsv_ListFacture.Columns[11].GetCellContent(Lsv_ListFacture.SelectedItem);
            //            string retrivingtext = txtBlock.Text;
            //        }
            //    }
            //    this.Txt_TotalAPayer.Text = ListeFactureAregle.Where(t => t.Selectionner).Sum(p => p.MONTANTPAYE).Value.ToString(SessionObject.FormatMontant);
            //}
        }

        private void dgMyDataGrid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            DataGrid dg = (sender as DataGrid);
            var allObjects = dg.ItemsSource as List<CsLclient>;
            if (dg.SelectedItem != null)
            {
                CsLclient SelectedObject = (CsLclient)dg.SelectedItem;

                if (SelectedObject.Selectionner == false)
                {
                    SelectedObject.Selectionner = true;
                    SelectedObject.IsExonerationTaxe  = false ;
                    SelectedObject.MONTANTPAYE = SelectedObject.SOLDEFACTURE;
                }
                else
                {
                    SelectedObject.Selectionner = false;
                    SelectedObject.MONTANTPAYE = null;
                }
            }
        }

        DateTime lastClick = DateTime.Now;
        private void dgMyDataGrid2_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            DataGrid dg = (sender as DataGrid);
            var allObjects = dg.ItemsSource as List<CsLclient>;
            if (dg.SelectedItem != null)
            {
                CsLclient SelectedObject = (CsLclient)dg.SelectedItem;

                if (SelectedObject.Selectionner == false)
                {
                    SelectedObject.Selectionner = true;
                    SelectedObject.MONTANTPAYE = SelectedObject.SOLDEFACTURE;
                }
                else
                {
                    SelectedObject.Selectionner = false;
                    SelectedObject.MONTANTPAYE = null;
                }
                if ((DateTime.Now - lastClick).Ticks < 2500000)
                {
                    UcSaisiMontant ctrl = new UcSaisiMontant(SelectedObject);
                    ctrl.Closed += ctrl_Closed;
                    ctrl.Show();
                }
                lastClick = DateTime.Now;
                this.Txt_TotalAPayer.Text = ListeFactureAregle.Where(t => t.Selectionner).Sum(p => p.MONTANTPAYE).Value.ToString(SessionObject.FormatMontant);
            }
        }
        void ctrl_Closed(object sender, EventArgs e)
        {
            UcSaisiMontant ctrs = sender as UcSaisiMontant;
            if (ctrs.isOkClick)
            {
                CsLclient leClientMofi = ListeFactureAregle.First(t => t.PK_ID == ctrs.SelectedObject.PK_ID);
                if (leClientMofi != null)
                {
                    leClientMofi.MONTANTPAYE = ctrs.SelectedObject.MONTANTPAYE;
                    leClientMofi.Selectionner = true;
                }
                btn_Rafraichir_Click(null, null);
            }
        }
        private void btn_Rafraichir_Click(object sender, RoutedEventArgs e)
        {
            if (this.Lsv_ListFactureFinal.ItemsSource != null)
            {
                List<CsLclient> lesFacturesSelectionne =((List<CsLclient>)this.Lsv_ListFactureFinal.ItemsSource).ToList();
                this.Txt_TotalAPayer.Text = lesFacturesSelectionne.Sum(p => p.MONTANTPAYE).Value.ToString(SessionObject.FormatMontant);
            }
        }
   
        private void Cbo_ListeClient_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.Cbo_ListeClient.SelectedItem != null)
            {
                CsClient leClientSelect = (CsClient)Cbo_ListeClient.SelectedItem;
                List<CsLclient> LstFactureSelect = new List<CsLclient>();
                if (leClientSelect.REFERENCEATM == "TOUS LES CLIENTS")
                {
                    LstFactureSelect = ListeFactureAregle.OrderByDescending (t=>t.Selectionner ).ThenBy(y=>y.CENTRE ).ToList();

                    this.TxtAddress.Visibility = System.Windows.Visibility.Collapsed;
                    this.TxtNomClient.Visibility = System.Windows.Visibility.Collapsed;
                }
                else
                {
                    this.TxtAddress.Visibility = System.Windows.Visibility.Visible ;
                    this.TxtNomClient.Visibility = System.Windows.Visibility.Visible;
                    LstFactureSelect = ListeFactureAregle.Where(t => t.FK_IDCLIENT == leClientSelect.PK_ID).OrderBy(y=>y.CENTRE).ToList();
                    this.TxtNomClient.Text = string.IsNullOrEmpty(LstFactureSelect.First().NOM) ? string.Empty : LstFactureSelect.First().NOM;
                    this.TxtAddress.Text = string.IsNullOrEmpty(LstFactureSelect.First().ADRESSE) ? string.Empty : LstFactureSelect.First().ADRESSE;
                }
                Lsv_ListFacture.ItemsSource = null;
                Lsv_ListFacture.ItemsSource = LstFactureSelect;
            }
        }

    
        public void InitialiseControle()
        {
            Lsv_ListFacture.ItemsSource = null;
            BntNth.IsEnabled = false;
            ListeFactureAregle = new List<CsLclient>();
            this.TxtAddress.Text = string.Empty;
            this.TxtNomClient.Text = string.Empty;
            this.Txt_TotalAPayer.Text = string.Empty;
            this.txt_PaiementPartiel.Text = string.Empty;
            this.Txt_MontantTotal.Text = string.Empty;
            this.Lsv_ListFacture.ItemsSource = null;
            this.Lsv_ListFactureFinal.ItemsSource = null;
            this.Lsv_ListFacture.UpdateLayout();
            ListeFactureAregle.Clear();
            lesEltInitial.Clear();
            this.cbo_PeriodeRegroupement.Visibility = System.Windows.Visibility.Collapsed;
            this.Txt_Periode.Visibility = System.Windows.Visibility.Visible ;
            this.Cbo_ListeClient.ItemsSource = null;
             
        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            InitialiseControle();
        }
        private void Chk_TvaExo_Checked(object sender, RoutedEventArgs e)
        {
            List<CsLclient> lstFactureAExonerer = ClasseMEthodeGenerique.RetourneListCopy<CsLclient>(ListeFactureAregle.Where(t => !t.Selectionner || (t.Selectionner && t.IsExonerationTaxe)).ToList());
            UcExonerationFacture form = new UcExonerationFacture(lstFactureAExonerer.OrderBy(t=>t.NDOC).ToList());
            form.Closed += new EventHandler(formClosed);
            form.Show();
        }
        void formClosed(object sender, EventArgs e)
        {
            try
            {
                UcExonerationFacture ctrs = sender as UcExonerationFacture;
                if (ctrs.lesFacturesExonere != null && ctrs.lesFacturesExonere.Count != 0)
                {
                    var lstFactureExonerer = from x in ListeFactureAregle
                                             join y in ctrs.lesFacturesExonere on x.PK_ID equals y.PK_ID
                                             select x;

                    foreach (CsLclient item in lstFactureExonerer)
                    {
                        decimal montantfact = (item.SOLDEFACTURE - (item.MONTANTTVA == null ? 0 : item.MONTANTTVA)).Value;
                        item.MONTANTPAYE = (montantfact - item.MONTANTPAYPARTIEL);
                        item.Selectionner = true;
                        item.IsExonerationTaxe = true;
                    }
                    var lstFactureNonExonerer = from x in ListeFactureAregle
                                                where !ctrs.lesFacturesExonere.Any(t => t.PK_ID == x.PK_ID)
                                                select x;

                    foreach (CsLclient item in lstFactureNonExonerer)
                    {
                        decimal montantfact = (item.SOLDEFACTURE + (item.MONTANTTVA == null ? 0 : item.MONTANTTVA)).Value;
                        if (item.Selectionner)
                            item.MONTANTPAYE = montantfact - item.MONTANTPAYPARTIEL;
                        else
                            item.MONTANTPAYE = null;
                        item.IsExonerationTaxe = false;
                    }
                }
                else
                {
                    foreach (CsLclient item in ListeFactureAregle)
                    {
                        decimal montantfact = (item.SOLDEFACTURE + (item.MONTANTTVA == null ? 0 : item.MONTANTTVA)).Value;
                        item.SOLDEFACTURE = montantfact - item.MONTANTPAYPARTIEL;
                        if (item.Selectionner)
                            item.MONTANTPAYE = item.SOLDEFACTURE;
                        else
                            item.MONTANTPAYE = null;

                        item.IsExonerationTaxe = false;
                    }
                }
                this.Txt_TotalAPayer.Text = ListeFactureAregle.Where(t => t.Selectionner).Sum(p => p.SOLDEFACTURE).Value.ToString(SessionObject.FormatMontant);
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, Langue.errorTitle);
            }

        }
        private void Chk_TvaExo_Unchecked(object sender, RoutedEventArgs e)
        {
             
        }

        private void Btn_Payement_Click(object sender, RoutedEventArgs e)
        {
            timer.Stop();
            timer = null;
            timer1 = new System.Windows.Threading.DispatcherTimer();
            timer1.Interval = new TimeSpan(3, 0, 0);
            timer1.Tick += timer_Tick;
            timer1.Start();
            PayementNormal(ListeFactureAregle);
        }
        private void  ConstruireListeReglement(List<CsLclient> _ListeFactureAreglee, decimal? MontantPaye)
        {
            try
            {
                _ListeFactureAreglee.ForEach(t => t.Selectionner = false);
                  foreach (CsLclient laFactureSelect in _ListeFactureAreglee)
                    {
                        if (MontantPaye != 0)
                        {
                                if (MontantPaye >= laFactureSelect.SOLDEFACTURE)
                                {
                                    laFactureSelect.MONTANTPAYE  = laFactureSelect.SOLDEFACTURE;
                                    MontantPaye = MontantPaye - laFactureSelect.MONTANTPAYE;
                                    laFactureSelect.Selectionner  = true;
                                    continue;
                                }
                                if (MontantPaye < laFactureSelect.SOLDEFACTURE)
                                {
                                    laFactureSelect.MONTANTPAYE = MontantPaye.Value;
                                    //laFactureSelect.SOLDEFACTURE = laFactureSelect.SOLDEFACTURE - laFactureSelect.MONTANTPAYE;
                                    MontantPaye = 0;
                                    laFactureSelect.Selectionner = true;
                                    continue;
                                }
                        }
                        else break;
                    }
                  _ListeFactureAreglee.Where(t => t.Selectionner == false).ToList().ForEach(u => u.MONTANTPAYE = null);
                  if (Lsv_ListFactureFinal.ItemsSource != null)
                      this.Txt_TotalAPayer.Text = ((List<CsLclient>)Lsv_ListFactureFinal.ItemsSource).Sum(y => y.MONTANTPAYE).Value.ToString(SessionObject.FormatMontant);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        private void PayementNormal(List<CsLclient> ListeDeFacture)
        {
            try
            {
                if (Lsv_ListFactureFinal.ItemsSource != null) ((List<CsLclient>)this.Lsv_ListFactureFinal.ItemsSource).ForEach(t => t.Selectionner = true);

                List<CsLclient> lstFacture = ClasseMEthodeGenerique.RetourneListCopy<CsLclient>(ListeFactureAregle);
                lstFacture.ForEach(t => t.COPER = SessionObject.Enumere.CoperRGT);
                lstFacture.ForEach(t => t.SOLDEFACTURE = t.MONTANTPAYE);
                UcValideEncaissementRegroupement UcValider = new UcValideEncaissementRegroupement(lstFacture, SessionObject.Enumere.ActionRecuEditionNormale, true);
                UcValider.Closed += new EventHandler(UcValideEncaissementClosed);
                UcValider.Show();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        void UcValideEncaissementClosed(object sender, EventArgs e)
        {
            try
            {
                UcValideEncaissementRegroupement ctrs = sender as UcValideEncaissementRegroupement;
                if (ctrs.Yes)
                    InitialiseControle();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, Langue.errorTitle);
            }

        }

        private void btRegroupement_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Dictionary<string, string> _LstColonneAffich = new Dictionary<string, string>();
                _LstColonneAffich.Add("CODE", "CODE");
                _LstColonneAffich.Add("NOM", "LIBELLE");
                List<object> objects = ClasseMEthodeGenerique.RetourneListeObjet(SessionObject.LstCodeRegroupement);
                MainView.UcListeGenerique ctrl = new MainView.UcListeGenerique(objects, _LstColonneAffich, false, "Lots");
                ctrl.Closed += new EventHandler(galatee_OkClicked);
                ctrl.Show();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        void galatee_OkClicked(object sender, EventArgs e)
        {
            try
            {
                MainView.UcListeGenerique ctrs = sender as MainView.UcListeGenerique;
                ServiceAccueil.CsRegCli reg = (ServiceAccueil.CsRegCli)ctrs.MyObject;
                if (reg != null)
                {
                    this.txtCodeRegroupement.Text = reg.CODE;
                    this.txtCodeRegroupement.Tag = reg;
                    this.txtCodeLabel.Text = reg.NOM;
                }


            }
            catch (Exception ex)
            {
                Message.ShowError(ex, Langue.errorTitle);
            }
        }
        private void ChargerCodeRegroupement()
        {
            try
            {
                if (SessionObject.LstCodeRegroupement != null && SessionObject.LstCodeRegroupement.Count != 0)
                {
                    this.btn_Regroupement.IsEnabled = true;
                    return;
                }
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.RetourneCodeRegroupementCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    SessionObject.LstCodeRegroupement = args.Result;
                    this.btn_Regroupement.IsEnabled = true;

                };
                service.RetourneCodeRegroupementAsync();
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        private void AfficheImpaye(List<int> lstIdReg,List<string> lstPeriode)
        {
            try
            {

                List<CsLclient> _ListeFactureClient = new List<CsLclient>();
                this.Lsv_ListFacture.ItemsSource = null;
                int debutClient = SessionObject.Enumere.TailleCentre;
                int debutOrdre = SessionObject.Enumere.TailleCentre + SessionObject.Enumere.TailleClient;
               
                if (ListeFactureAregle != null && ListeFactureAregle.Count != 0)
                    ListeFactureAregle.Clear();
                if (LstClient != null && LstClient.Count != 0)
                    LstClient.Clear();

                Galatee.Silverlight.ServiceCaisse.CaisseServiceClient service = new Galatee.Silverlight.ServiceCaisse.CaisseServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Caisse"));
                service.RetourneListeFactureNonSoldeByRegroupementCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    if (args.Result == null || args.Result.Count == 0)
                    {
                        Message.ShowInformation("Aucune facture trouvée pour ce regroupement", Langue.LibelleModule);
                        return;
                    }
                    List<CsLclient> lstFactureDuClient = args.Result;
                    LstClient.Add(new CsClient() 
                    {
                        REFERENCEATM = "TOUS LES CLIENTS",
                    });
                    LstClient.AddRange(MethodeGenerics.RetourneClientFromFacture(lstFactureDuClient));
                    foreach (CsClient item in LstClient.Where(t => t.REFERENCEATM != "TOUS LES CLIENTS").ToList())
                        item.REFERENCEATM = item.CENTRE + item.REFCLIENT + item.ORDRE;

                    foreach (var item in lstFactureDuClient)
                    {
                        item.MONTANTPAYPARTIEL = item.MONTANT - item.SOLDEFACTURE;
                        if (item.MONTANTPAYPARTIEL < 0) item.MONTANTPAYPARTIEL = 0;
                    }
                    lstFactureDuClient.ForEach(t => t.MONTANTPAYPARTIEL = t.MONTANT - t.SOLDEFACTURE);

                    ListeFactureAregle.AddRange(lstFactureDuClient.OrderByDescending(p=>p.REFEM ).ToList());
                    lstFactureDuClient.ForEach(t => t.REFEM = ClasseMEthodeGenerique.FormatPeriodeMMAAAA(t.REFEM));
                    this.Txt_MontantTotal.Text = lstFactureDuClient.Sum(t => t.SOLDEFACTURE).Value.ToString(SessionObject.FormatMontant);
                    InitComboBox(LstClient);
                    InitGridView(ListeFactureAregle.OrderBy(t=>t.NDOC).ToList());
                };
                service.RetourneListeFactureNonSoldeByRegroupementAsync(lstIdReg, lstPeriode);
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        List<string> LstPeriode = new List<string>();
        private void btn_Periode_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(this.Txt_Periode.Text) || this.cbo_PeriodeRegroupement.SelectedItem != null )
            {
                if (!string.IsNullOrEmpty(this.Txt_Periode.Text) && LstPeriode.FirstOrDefault(t => t == this.Txt_Periode.Text) == null)
                {
                    LstPeriode.Add(this.Txt_Periode.Text);
                    this.cbo_Periode.ItemsSource = null;
                    this.cbo_Periode.ItemsSource = LstPeriode;
                    this.cbo_Periode.SelectedIndex = 0;
                }
                else if (this.cbo_PeriodeRegroupement.SelectedItem != null && LstPeriode.FirstOrDefault(t => t == this.Txt_Periode.Text) == null)
                {
                    LstPeriode.Add((string)this.cbo_PeriodeRegroupement.SelectedItem);
                    this.cbo_Periode.ItemsSource = null;
                    this.cbo_Periode.ItemsSource = LstPeriode;
                    this.cbo_Periode.SelectedIndex = 0;
                }
                else 
                    Message.ShowInformation("Période déja saisie", "Edition");
            }
        }
        private void btn_Rechercher_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                if (Lsv_ListFacture.ItemsSource == null)
                {
                    prgBar.Visibility = System.Windows.Visibility.Visible;
                    this.btn_Rechercher.IsEnabled = false;
                    if (ListeFactureAregle.Count != 0) ListeFactureAregle.Clear();
                    if (LstClient.Count != 0) LstClient.Clear();

                    //if (this.txtCodeRegroupement.Tag != null && LstPeriode.Count > 0)
                    if (this.txtCodeRegroupement.Tag != null)
                    {
                        List<string> ListRefem = new List<string>();
                        if (LstPeriode.Count > 0)
                        {
                            foreach (var item in LstPeriode)
                                ListRefem.Add(ClasseMEthodeGenerique.FormatPeriodeAAAAMM(item.ToString()));
                        }
                        List<int> lstIdProduit = new List<int>();
                        if (this.Chk_BT.IsChecked == true)
                            lstIdProduit.Add(SessionObject.ListeDesProduit.FirstOrDefault(t => t.CODE == SessionObject.Enumere.Electricite).PK_ID);
                        if (this.Chk_MT.IsChecked == true)
                            lstIdProduit.Add(SessionObject.ListeDesProduit.FirstOrDefault(t => t.CODE == SessionObject.Enumere.ElectriciteMT).PK_ID);

                        if (lstIdProduit.Count == 0)
                        {
                            //CsRegCli leRegroupement = Utility.ConvertType<ServiceCaisse.CsRegCli, ServiceAccueil.CsRegCli>((ServiceAccueil.CsRegCli)this.txtCodeRegroupement.Tag);
                            ServiceAccueil.CsRegCli leRegro = (ServiceAccueil.CsRegCli)this.txtCodeRegroupement.Tag;
                            CsRegCli leRegroupement = new CsRegCli() { CODE = leRegro.CODE, PK_ID = leRegro.PK_ID, NOM = leRegro.NOM };
                            lstIdProduit.Add(SessionObject.ListeDesProduit.FirstOrDefault(t => t.CODE == SessionObject.Enumere.Electricite).PK_ID);
                            lstIdProduit.Add(SessionObject.ListeDesProduit.FirstOrDefault(t => t.CODE == SessionObject.Enumere.ElectriciteMT).PK_ID);
                            lstIdProduit.Add(SessionObject.ListeDesProduit.FirstOrDefault(t => t.CODE == SessionObject.Enumere.Eau).PK_ID); /** ZEG 29/08/2017 **/
                           RemplirfactureProduit(leRegroupement, ListRefem, lstIdProduit);

                            //Remplirfacture(leRegroupement, ListRefem);
                        }
                        else
                        {
                            //CsRegCli leRegroupement = Utility.ConvertType<ServiceCaisse.CsRegCli, ServiceAccueil.CsRegCli>((ServiceAccueil.CsRegCli)this.txtCodeRegroupement.Tag);
                            ServiceAccueil.CsRegCli leRegro = (ServiceAccueil.CsRegCli)this.txtCodeRegroupement.Tag;
                            CsRegCli leRegroupement = new CsRegCli() { CODE = leRegro.CODE, PK_ID = leRegro.PK_ID, NOM = leRegro.NOM };
                            RemplirfactureProduit(leRegroupement, ListRefem, lstIdProduit);
                        }
                    }
                    else
                    {
                        Message.Show("Veuillez vous assurer que vous avez selectionner un regroupement et saisis moin une periode", "Information");
                    }
                }
                else
                {
                    if (this.cbo_Periode.ItemsSource != null)
                    {
                        List<string> lesPeriode = (List<string>)this.cbo_Periode.ItemsSource;
                        List<CsLclient> lesFacture = (List<CsLclient>)this.Lsv_ListFacture.ItemsSource;
                        this.Lsv_ListFacture.ItemsSource = null;
                        this.Lsv_ListFacture.ItemsSource = lesFacture.Where(u => lesPeriode.Contains(u.REFEM)).ToList();
                        this.Txt_MontantTotal.Text = ((List<CsLclient>)this.Lsv_ListFacture.ItemsSource).Sum(t => t.SOLDEFACTURE).Value.ToString(SessionObject.FormatMontant);

                    }
                    else
                    {
                        this.Lsv_ListFacture.ItemsSource = null;
                        this.Lsv_ListFacture.ItemsSource = ListeFactureAregle.ToList();
                        this.Txt_MontantTotal.Text = ((List<CsLclient>)this.Lsv_ListFacture.ItemsSource).Sum(t => t.SOLDEFACTURE).Value.ToString(SessionObject.FormatMontant);
                    }
                }
            }
            catch (Exception es)
            {
                prgBar.Visibility = System.Windows.Visibility.Collapsed;
                this.btn_Rechercher.IsEnabled = true;
                Message.ShowInformation("Erreur a la recherche des factures", "Information");

            }
        }
        private void Remplirfacture(CsRegCli csRegCli, List<string> listperiode)
        {
            Galatee.Silverlight.ServiceCaisse.CaisseServiceClient service = new Galatee.Silverlight.ServiceCaisse.CaisseServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Caisse"));
            service.RemplirfactureRegroupementAsync(csRegCli, listperiode);
            service.RemplirfactureRegroupementCompleted += (s, args) =>
            {
                this.btn_Rechercher.IsEnabled = true;
                prgBar.Visibility = System.Windows.Visibility.Collapsed ;

                if (args != null && args.Cancelled)
                    return;
                if (args.Result == null || args.Result.Count == 0)
                {
                    Message.ShowInformation("Aucune facture trouvée pour ce regroupement", Langue.LibelleModule);
                    return;
                }
                List<CsLclient> lstFactureDuClient = args.Result;
                lesEltInitial.AddRange(Shared.ClasseMEthodeGenerique.RetourneListCopy<CsLclient>(lstFactureDuClient));
                LstClient.Add(new CsClient()
                {
                    REFERENCEATM = "TOUS LES CLIENTS",
                });
                LstClient.AddRange(MethodeGenerics.RetourneClientFromFacture(lstFactureDuClient));
                foreach (CsClient item in LstClient.Where(t => t.REFERENCEATM != "TOUS LES CLIENTS").ToList())
                    item.REFERENCEATM = item.CENTRE + item.REFCLIENT + item.ORDRE;
                ListeFactureAregle.AddRange(lstFactureDuClient);
                lstFactureDuClient.ForEach(t => t.REFEM = ClasseMEthodeGenerique.FormatPeriodeMMAAAA(t.REFEM));
                this.Txt_MontantTotal.Text = lstFactureDuClient.Sum(t => t.SOLDEFACTURE).Value.ToString(SessionObject.FormatMontant);

                List<string> lstPeriode = new List<string>();
                var lesPeriode = ListeFactureAregle.Select(o => new { o.REFEM }).Distinct();
                foreach (var item in lesPeriode)
                    lstPeriode.Add(item.REFEM );

                cbo_PeriodeRegroupement.Visibility = System.Windows.Visibility.Visible ;
                this.Txt_Periode.Visibility = System.Windows.Visibility.Collapsed;

                this.cbo_PeriodeRegroupement .ItemsSource = null;
                this.cbo_PeriodeRegroupement.ItemsSource = lstPeriode;
                this.cbo_PeriodeRegroupement.SelectedIndex = 0;

                InitComboBox(LstClient);
                InitGridView(ListeFactureAregle);
            };
        }
        private void RemplirfactureProduit(CsRegCli csRegCli, List<string> listperiode, List<int> lstIdProduit)
        {
            Galatee.Silverlight.ServiceCaisse.CaisseServiceClient service = new Galatee.Silverlight.ServiceCaisse.CaisseServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Caisse"));
            service.RemplirfactureRegroupementAvecProduitAsync(csRegCli, listperiode, lstIdProduit);
            service.RemplirfactureRegroupementAvecProduitCompleted += (s, args) =>
            {
                this.btn_Rechercher.IsEnabled = true;
                prgBar.Visibility = System.Windows.Visibility.Collapsed;

                if (args != null && args.Cancelled)
                    return;
                if (args.Result == null || args.Result.Count == 0)
                {
                    Message.ShowInformation("Aucune facture trouvée pour ce regroupement", Langue.LibelleModule);
                    return;
                }
                List<CsLclient> lstFactureDuClient = args.Result;
                lesEltInitial.AddRange(Shared.ClasseMEthodeGenerique.RetourneListCopy<CsLclient>(lstFactureDuClient));
                LstClient.Add(new CsClient()
                {
                    REFERENCEATM = "TOUS LES CLIENTS",
                });
                LstClient.AddRange(MethodeGenerics.RetourneClientFromFacture(lstFactureDuClient));
                foreach (CsClient item in LstClient.Where(t => t.REFERENCEATM != "TOUS LES CLIENTS").ToList())
                    item.REFERENCEATM = item.CENTRE + item.REFCLIENT + item.ORDRE;

                lstFactureDuClient.ForEach(u => u.REFEMNDOC = u.REFEM);
                ListeFactureAregle.AddRange(lstFactureDuClient.OrderByDescending(o => o.REFEMNDOC).ToList());

                lstFactureDuClient.ForEach(t => t.REFEM = ClasseMEthodeGenerique.FormatPeriodeMMAAAA(t.REFEM));
                this.Txt_MontantTotal.Text = lstFactureDuClient.Sum(t => t.SOLDEFACTURE).Value.ToString(SessionObject.FormatMontant);

                List<string> lstPeriode = new List<string>();
                var lesPeriode = ListeFactureAregle.Select(o => new { o.REFEM }).OrderByDescending(l => l.REFEM).Distinct();
                foreach (var item in lesPeriode)
                    lstPeriode.Add(item.REFEM);

                cbo_PeriodeRegroupement.Visibility = System.Windows.Visibility.Visible;
                this.Txt_Periode.Visibility = System.Windows.Visibility.Collapsed;

                this.cbo_PeriodeRegroupement.ItemsSource = null;
                this.cbo_PeriodeRegroupement.ItemsSource = lstPeriode;
                this.cbo_PeriodeRegroupement.SelectedIndex = 0;


                InitComboBox(LstClient);
                InitGridView(ListeFactureAregle.OrderByDescending(t=>t.REFEMNDOC ).ToList());
            };
        }


        private void Lsv_LigneNaf_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void txtCodeRegroupement_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(this.txtCodeRegroupement.Text) && this.txtCodeRegroupement.Text.Length == SessionObject.Enumere.TailleCodeRegroupement)
            {
                ServiceAccueil.CsRegCli leReg = SessionObject.LstCodeRegroupement.FirstOrDefault(t => t.CODE == this.txtCodeRegroupement.Text);
                if (leReg != null)
                {
                    this.txtCodeRegroupement.Text = leReg.CODE;
                    this.txtCodeRegroupement.Tag = leReg;
                    this.txtCodeLabel.Text = leReg.NOM;
                }
                else
                {
                    Message.ShowInformation("Regroupement inexistant", "Caisse");
                    return;
                }

            }
        }
        private void btn_Ajouter_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                FrmRechercheClient recherche = new FrmRechercheClient(true);
                recherche.Closed += new EventHandler(FrmRechercheClientClosed);
                recherche.Show();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, Langue.errorTitle);
            }
        }
        private void InitComboBox(List<CsClient> lesClient)
        {
            Cbo_ListeClient.ItemsSource = null;
            Cbo_ListeClient.ItemsSource = lesClient;
            Cbo_ListeClient.DisplayMemberPath = "REFERENCEATM";
        }

        private void InitGridView(List<CsLclient > lesFacture)
        {
            Lsv_ListFacture.ItemsSource = null;
            Lsv_ListFacture.ItemsSource = lesFacture;
            this.Txt_MontantTotal.Text = ((List<CsLclient>)this.Lsv_ListFacture.ItemsSource).Sum(t => t.SOLDEFACTURE).Value.ToString(SessionObject.FormatMontant);

        }
        void FrmRechercheClientClosed(object sender, EventArgs e)
        {
            FrmRechercheClient recherche = sender as FrmRechercheClient;
            try
            {
                List<CsLclient> lstFactureSelect = recherche.lstFacture;
                List<CsClient> leClient= MethodeGenerics.RetourneClientFromFacture(recherche.lstFacture);
                leClient.ForEach(t=>t.REFERENCEATM = t.CENTRE + t.REFCLIENT + t.ORDRE );
                LstClient.AddRange(leClient);
                InitComboBox(LstClient);
                foreach (CsLclient item in lstFactureSelect)
	                {
                        if (ListeFactureAregle.FirstOrDefault(t => item.FK_IDCLIENT == t.FK_IDCLIENT && t.REFEM == item.REFEM && t.NDOC == item.NDOC) == null)
                            ListeFactureAregle.Add(item);
                        else
                        {
                            Message.Show("Cette facture existe déja", "Caisse");
                            continue;
                        }
		 
	                }
                this.Lsv_ListFacture.ItemsSource = null;
                this.Lsv_ListFacture.ItemsSource = ListeFactureAregle;
                if (this.Lsv_ListFacture.ItemsSource != null )
                this.Txt_MontantTotal.Text = ((List<CsLclient>)this.Lsv_ListFacture.ItemsSource).Sum(t => t.SOLDEFACTURE).Value.ToString(SessionObject.FormatMontant);

            }
            catch (Exception ex)
            {
                Message.ShowError(ex, Langue.errorTitle);
            }

        }

        private void btn_Supprimer_Click(object sender, RoutedEventArgs e)
        {
            //if (this.Cbo_ListeClient.SelectedItem != null )
            //{
            //    CsClient clientSelect = (CsClient)this.Cbo_ListeClient.SelectedItem;
            //    if (clientSelect.REFERENCEATM != "TOUS LES CLIENTS")
            //    {
            //        LstClient.Remove(clientSelect);
            //        ListeFactureAregle.RemoveAll(t => t.FK_IDCLIENT == clientSelect.PK_ID);

            //        InitComboBox(LstClient);
            //        InitGridView(ListeFactureAregle);
            //    }
            //}
            if (Lsv_ListFacture.SelectedItem != null)
            {
                ListeFactureAregle.RemoveAll(t => t.FK_IDCLIENT ==((CsLclient)Lsv_ListFacture.SelectedItem).FK_IDCLIENT);
                InitComboBox(LstClient);
                InitGridView(ListeFactureAregle);
            }
        }

        private void chk_PaiementPartiel_Checked(object sender, RoutedEventArgs e)
        {
            this.txt_PaiementPartiel.IsReadOnly = false;
            this.txt_PaiementPartiel.Focus();
        }

        private void chk_PaiementPartiel_Unchecked(object sender, RoutedEventArgs e)
        {
            this.txt_PaiementPartiel.IsReadOnly = true ;
            this.txt_PaiementPartiel.Text = string.Empty;
            //BntNth_Click(null, null);

        }

        private void txt_PaiementPartiel_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(this.txt_PaiementPartiel.Text) && this.Lsv_ListFactureFinal.ItemsSource != null)
            {
                this.txt_PaiementPartiel.Text = Convert.ToDecimal(txt_PaiementPartiel.Text).ToString(SessionObject.FormatMontant);
                List<CsLclient> lstFactureARegle = (List<CsLclient>)this.Lsv_ListFactureFinal.ItemsSource;
                ConstruireListeReglement(lstFactureARegle, Convert.ToDecimal(this.txt_PaiementPartiel.Text));
                this.Btn_Payement.Focus();
            }
        }

        private void btn_SupprimerPeriode_Click(object sender, RoutedEventArgs e)
        {
            if (this.cbo_Periode.SelectedItem != null )
            {
                if (LstPeriode.FirstOrDefault(t => t == (string)this.cbo_Periode.SelectedItem) != null)
                {
                    LstPeriode.Remove((string)this.cbo_Periode.SelectedItem);
                    this.cbo_Periode.ItemsSource = null;
                    this.cbo_Periode.ItemsSource = LstPeriode;
                    if (LstPeriode.Count != 0)
                      this.cbo_Periode.SelectedIndex = 0;
                }
            }
        }
     
        private void btn_ToutSelect_Click(object sender, RoutedEventArgs e)
        {
            if (this.Lsv_ListFacture.ItemsSource != null)
            {
                List<CsLclient> ListeSelect = ((List<CsLclient>)this.Lsv_ListFacture.ItemsSource).ToList();
                foreach (CsLclient item in ListeSelect)
                {
                    item.Selectionner = false;
                    item.MONTANTPAYE = item.SOLDEFACTURE;
                    Lsv_ListFacture.SelectedItems.Add(item);
                }
                Galatee.Silverlight.Shared.CommonMethode.TransfertDataGrid<CsLclient>(Lsv_ListFacture, Lsv_ListFactureFinal);
                if (Lsv_ListFactureFinal.ItemsSource != null)
                    this.Txt_TotalAPayer.Text = ((List<CsLclient>)Lsv_ListFactureFinal.ItemsSource).Sum(y => y.SOLDEFACTURE).Value.ToString(SessionObject.FormatMontant);
            }
        }

        private void btn_UnSeul_Click(object sender, RoutedEventArgs e)
        {
            if (this.Lsv_ListFacture.ItemsSource != null)
            {
                List<CsLclient> ListeSelect = ((List<CsLclient>)this.Lsv_ListFacture.ItemsSource).Where(t => t.Selectionner == true).ToList();
                if (ListeSelect != null && ListeSelect.Count != 0)
                {
                    foreach (CsLclient item in ListeSelect)
                    {
                        item.Selectionner = false;
                        item.MONTANTPAYE = item.SOLDEFACTURE;
                        Lsv_ListFacture.SelectedItems.Add(item);
                    }
                    Galatee.Silverlight.Shared.CommonMethode.TransfertDataGrid<CsLclient>(Lsv_ListFacture, Lsv_ListFactureFinal);
                    if (Lsv_ListFactureFinal.ItemsSource != null)
                        this.Txt_TotalAPayer.Text = ((List<CsLclient>)Lsv_ListFactureFinal.ItemsSource).Sum(y => y.MONTANTPAYE).Value.ToString(SessionObject.FormatMontant);
                }
                else
                    Message.ShowInformation("Sélectionner une facture", "Information");
            }
        }

        private void Bnt_DeselectAll_Click(object sender, RoutedEventArgs e)
        {
            if (this.Lsv_ListFactureFinal.ItemsSource != null)
            {
                List<CsLclient> ListeSelect = ((List<CsLclient>)this.Lsv_ListFactureFinal.ItemsSource).ToList();
                foreach (CsLclient item in ListeSelect)
                {
                    item.Selectionner = false;
                    item.MONTANTPAYE = null;
                    item.IsExonerationTaxe = false;
                    Lsv_ListFactureFinal.SelectedItems.Add(item);
                }
                Galatee.Silverlight.Shared.CommonMethode.TransfertDataGrid<CsLclient>(Lsv_ListFactureFinal, Lsv_ListFacture);
                if (Lsv_ListFacture.ItemsSource != null)
                    this.Txt_TotalAPayer.Text = "0";
            }
        }

        private void Bnt_DeselectUn_Click(object sender, RoutedEventArgs e)
        {
            if (this.Lsv_ListFactureFinal.ItemsSource != null)
            {
                List<CsLclient> ListeSelect = ((List<CsLclient>)this.Lsv_ListFactureFinal.ItemsSource).Where(t => t.Selectionner == true).ToList();
                if (ListeSelect != null && ListeSelect.Count != 0)
                {
                    foreach (CsLclient item in ListeSelect)
                    {
                        item.Selectionner = false;
                        item.IsExonerationTaxe = false;
                        item.MONTANTPAYE = null;
                        Lsv_ListFactureFinal.SelectedItems.Add(item);
                    }
                    Galatee.Silverlight.Shared.CommonMethode.TransfertDataGrid<CsLclient>(Lsv_ListFactureFinal, Lsv_ListFacture);
                    if (Lsv_ListFactureFinal.ItemsSource != null)
                        this.Txt_TotalAPayer.Text = ((List<CsLclient>)Lsv_ListFactureFinal.ItemsSource).Sum(y => y.MONTANTPAYE).Value.ToString(SessionObject.FormatMontant);
                }
                else
                    Message.ShowInformation("Sélectionner une facture", "Information");
            }
        }

        private void chk_Exonere_Checked(object sender, RoutedEventArgs e)
        {
            List<CsLclient> lstFactureExoneres = (List<CsLclient>)this.Lsv_ListFactureFinal.ItemsSource;
            if (lstFactureExoneres != null && this.Lsv_ListFactureFinal.SelectedItem != null)
            {
                CsLclient leEltsSelect = lstFactureExoneres.FirstOrDefault(t => t.PK_ID == ((CsLclient)this.Lsv_ListFactureFinal.SelectedItem).PK_ID);
                IsExonnere(leEltsSelect, true);
            }
        }

        private void chk_Exonere_Unchecked(object sender, RoutedEventArgs e)
        {
            List<CsLclient> lstFactureExoneres = (List<CsLclient>)this.Lsv_ListFactureFinal.ItemsSource;
            if (lstFactureExoneres != null && this.Lsv_ListFactureFinal.SelectedItem != null)
            {
                CsLclient leEltsSelect = lstFactureExoneres.FirstOrDefault(t => t.PK_ID == ((CsLclient)this.Lsv_ListFactureFinal.SelectedItem).PK_ID);
                IsExonnere(leEltsSelect, false);
            }
        }

        private void IsExonnere(CsLclient leElt, bool IsChecked)
        {
            try
            {
                List<CsLclient> lstEltfacture = new List<CsLclient>();
                lstEltfacture = ((List<CsLclient>)Lsv_ListFactureFinal.ItemsSource).ToList();
                CsLclient leElts = lstEltfacture.FirstOrDefault(t => t.PK_ID == leElt.PK_ID);
                CsLclient leEltss = lesEltInitial.FirstOrDefault(t => t.PK_ID == leElt.PK_ID);
                if (leElts != null)
                {
                    if (IsChecked)
                    {
                        decimal montantfact = (leEltss.SOLDEFACTURE - (leEltss.MONTANTTVA == null ? 0 : leEltss.MONTANTTVA)).Value;
                        leElts.MONTANTPAYE = montantfact;
                        leElts.SOLDEFACTURE = montantfact;
                        leElts.IsExonerationTaxe = true;
                    }
                    else
                    {
                        decimal montantfact = leEltss.SOLDEFACTURE.Value  ;
                        leElts.MONTANTPAYE = montantfact;
                        leElts.SOLDEFACTURE = montantfact;
                        leElts.IsExonerationTaxe = false;
 
                    }
                    if (Lsv_ListFactureFinal.ItemsSource != null)
                        this.Txt_TotalAPayer.Text = ((List<CsLclient>)Lsv_ListFactureFinal.ItemsSource).Sum(y => y.MONTANTPAYE).Value.ToString(SessionObject.FormatMontant);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        bool checkSelectedItem(CheckBox check)
        {
            CheckBox chk = check;
            return chk.IsChecked.Value;
        }

        void checkerSelectedItem(CheckBox check)
        {
            try
            {
                CheckBox chk = check;
                if (chk.IsChecked.Value)
                    chk.IsChecked = false;
                else
                    chk.IsChecked = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }

    }
}

