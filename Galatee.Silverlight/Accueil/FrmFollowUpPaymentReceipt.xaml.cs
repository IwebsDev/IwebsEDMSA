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
using Galatee.Silverlight.Resources.Accueil;
using System.ComponentModel;
using System.Collections.ObjectModel;
using Galatee.Silverlight.ServiceAccueil;
using Galatee.Silverlight.Resources.Accueil;

namespace Galatee.Silverlight.Accueil
{
    public partial class FrmFollowUpPaymentReceipt : ChildWindow
    {
        public FrmFollowUpPaymentReceipt()
        {
            InitializeComponent();
            Translate();
            InitializeCompoennt();

        }

        void Translate()
        {
            lbl_cashier.Content = Langue.lbl_cashier;
            lbl_paymday.Content = Langue.lbl_paymday;
            lbl_status.Content = Langue.lbl_status;

            rdb_direct.Content = Langue.rdb_direct;
            rdb_manual.Content = Langue.rdb_manual;

            btn_reset.Content = Langue.btn_reset;
            btn_Search.Content = Langue.btn_search;

            dgt_searchResult.Columns[0].Header = Langue.dg_route;
            dgt_searchResult.Columns[1].Header = Langue.dg_amountpaid;
            dgt_searchResult.Columns[2].Header = Langue.dg_date;
            dgt_searchResult.Columns[3].Header = Langue.dg_cancelReason;
            dgt_searchResult.Columns[4].Header = Langue.dg_keyedby;
        }

        public FrmFollowUpPaymentReceipt(int numero, string text, string TableNom)
        {
            InitializeComponent();
            num = numero;
            this.Title = text;
             this.InitializeCompoennt();
        }

        int num;
        List<CsUtilisateur> usersListe = new List<CsUtilisateur>();
        CsUtilisateur FisrtUser = null;
       
          /// <summary>
          /// PERMET DE VALORISER LES DATAGRID ET   
          /// LES COMBOBOX AU CHARGEMENT DE LA PAGE
          /// </summary>
          /// <param name="datagrid"></param>
          /// <param name="services"></param>

       void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

       void InitializeCompoennt()
        {
            try
            {
                // Initialisation variables globales 

                usersListe.Clear();
                FisrtUser = new CsUtilisateur() { MATRICULE = "   ", LIBELLE  = "   " };
                AcceuilServiceClient accueilInit = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                accueilInit.RetourneListeAllUserCompleted += (ssender, args) =>
                    {
                        if (args.Cancelled || args.Error != null)
                        {
                            string error = args.Error.Message;
                            MessageBox.Show(Langue.ErrorMsgOnInvokingService,Langue.ErrorMsgTitleOnInvokingService, MessageBoxButton.OK);
                            desableProgressBar();
                            this.DialogResult = true;
                            return;
                        }

                        if (args.Result == null || args.Result.Count == 0)
                        {
                            MessageBox.Show(Langue.NoDataMsg,"", MessageBoxButton.OK);
                            desableProgressBar();
                            this.DialogResult = true;
                            return;
                        }

                        CsUtilisateur user = new CsUtilisateur();
                        usersListe.AddRange(args.Result);
                        FillCombobox(usersListe);

                        // disable radionbutton
                        rdb_direct.IsChecked = true;

                    };
                accueilInit.RetourneListeAllUserAsync();

            }
            catch (Exception ex)
            {
                string error= ex.Message;
            }

        }

       private void FillCombobox(List<CsUtilisateur> users)
       {
           try
           {
               List<CsUtilisateur> usercaisse = new List<CsUtilisateur>();
               List<CsUtilisateur> userkeyedby = new List<CsUtilisateur>();
               List<CsUtilisateur> useropenby = new List<CsUtilisateur>();
               useropenby.Add(FisrtUser);
               userkeyedby.Add(FisrtUser);
               usercaisse.Add(FisrtUser);
               usercaisse.AddRange(users.Where(user => user.FONCTION    == "430").OrderBy(p=>p.LIBELLE ).ToList());
               userkeyedby.AddRange(users.Where(user => user.FONCTION  != "430").OrderBy(p => p.LIBELLE ).ToList());
               useropenby.AddRange(users.OrderBy(p => p.LIBELLE ).ToList());
               cbo_ref.ItemsSource = usercaisse;
           }
           catch (Exception ex)
           {
               string error = ex.Message;
           }
       }

        void desableProgressBar()
        {
            progressBar1.IsIndeterminate = false;
            progressBar1.Visibility = Visibility.Collapsed;
        }

        void allowProgressBar()
        {
            progressBar1.IsEnabled = true;
            progressBar1.Visibility = Visibility.Visible;
            progressBar1.IsIndeterminate = true;
        }

        void resetInsertData()
        {
            //txtcentre.Text = txtforfait.Text = txtLibelle.Text = string.Empty ;
            //cbo_refproducts.ItemsSource = null;
            //cbo_refproducts.ItemsSource = produits;

        }

        void btnReset_Click(object sender, RoutedEventArgs e)
        {
            resetInsertData();
        }

        void bntCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void cbo_ref_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbo_ref.SelectedItem == null)
                return;

            CsUtilisateur user = cbo_ref.SelectedItem as CsUtilisateur;
            txt_ref.Text = user.MATRICULE  ;
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void btn_Search_Click(object sender, RoutedEventArgs e)
        {
            aParam critere = new aParam();
            critere.Cashier = cbo_ref.SelectedItem == null ? string.Empty : cbo_ref.SelectedItem == FisrtUser ? string.Empty : (cbo_ref.SelectedItem as CsUtilisateur).MATRICULE == null ? string.Empty : (cbo_ref.SelectedItem as CsUtilisateur).MATRICULE;
            critere.isDirect = rdb_direct.IsChecked == true ;
            critere.Day = dtpDate_paymentDay.SelectedDate.ToString();
            critere.Receipt = string.IsNullOrEmpty(txt_receipt.Text) ? string.Empty : txt_receipt.Text;
            critere.DateReceipt = dtpDate_paymentDay.SelectedDate == null ? string.Empty : dtpDate_paymentDay.SelectedDate.ToString();

            AcceuilServiceClient accueilshearch = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
            accueilshearch.GET_PAYMENT_BY_ACQUITCompleted += (searchS, resultS) =>
            {
                if (resultS.Cancelled || resultS.Error != null)
                {
                    string error = resultS.Error.Message;
                    MessageBox.Show(Langue.ErrorMsgOnInvokingService, Langue.ErrorMsgTitleOnInvokingService, MessageBoxButton.OK);
                    desableProgressBar();
                    dgt_searchResult.ItemsSource = null;
                    return;
                }

                if (resultS.Result == null || resultS.Result.Count == 0)
                {
                    MessageBox.Show(Langue.NoDataMsg, "", MessageBoxButton.OK);
                    desableProgressBar();
                    dgt_searchResult.ItemsSource = null;
                    return;
                }

                dgt_searchResult.ItemsSource = resultS.Result;
            };
            accueilshearch.GET_PAYMENT_BY_ACQUITAsync(critere);
        }

        private void btn_reset_Click(object sender, RoutedEventArgs e)
        {
            ResetComponent();
        }

        private void ResetComponent()
        {
            try
            {
                cbo_ref.SelectedItem = cbo_ref.Items[0];
                rdb_direct.IsChecked = true;
                rdb_manual.IsChecked = false;
                dtpDate_paymentDay.Text = string.Empty;
                dgt_searchResult.ItemsSource = null;
            }
            catch (Exception ex)
            {

                string error = ex.Message ;
            }
        }

        private void txt_receipt_TextChanged(object sender, TextChangedEventArgs e)
        {
            btn_Search.IsEnabled = !string.IsNullOrEmpty(txt_receipt.Text) ;
        }
        
    }
}


