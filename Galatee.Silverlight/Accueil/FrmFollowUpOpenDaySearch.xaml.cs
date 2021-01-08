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

namespace Galatee.Silverlight.Accueil
{
    public partial class FrmFollowUpOpenDaySearch : ChildWindow
    {
        public FrmFollowUpOpenDaySearch()
        {
            InitializeComponent();
            Translate();
            InitializeCompoennt();

        }

        void Translate()
        {
            lbl_cashier.Content = Langue.lbl_cashier;
            lbl_keyedby.Content = Langue.lbl_keyedby ;
            lbl_Openby.Content = Langue.lbl_Openby;
            lbl_openday.Content = Langue.lbl_openday;
            lbl_paymday.Content = Langue.lbl_paymday;
            lbl_status.Content = Langue.lbl_status;

            rdb_direct.Content = Langue.rdb_direct;
            rdb_manual.Content = Langue.rdb_manual;

            btn_reset.Content = Langue.btn_reset;
            btn_Search.Content = Langue.btn_search;

            dgt_searchResult.Columns[0].Header = Langue.dg_openby;
            dgt_searchResult.Columns[1].Header = Langue.dg_openOn;
            dgt_searchResult.Columns[2].Header = Langue.dg_payday;
            dgt_searchResult.Columns[3].Header = Langue.dg_cashier;
            dgt_searchResult.Columns[4].Header = Langue.dg_keyedby;
            dgt_searchResult.Columns[5].Header = Langue.dg_raisonOuverture;
        }

        public FrmFollowUpOpenDaySearch(int numero, string text, string TableNom)
        {
            InitializeComponent();
            num = numero;
            this.Title = text;
            this.TableName = TableNom;

             this.InitializeCompoennt();
        }

        int num;
        List<CsUtilisateur> usersListe = new List<CsUtilisateur>();
        CsUtilisateur FisrtUser = null;
        List<aParam> searchList = new List<aParam>();

        private string TableName;
        private string LibelleEtat = string.Empty;
        private string combobasename = "cbo_Produit";

       
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
                            Message.Show(Langue.ErrorMsgOnInvokingService, Langue.ErrorMsgTitleOnInvokingService);
                            desableProgressBar();
                            this.DialogResult = true;
                            return;
                        }

                        if (args.Result == null || args.Result.Count == 0)
                        {
                            Message.Show(Langue.NoDataMsg,"");
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
               usercaisse.AddRange(users.Where(user => user.FONCTION  == "430").OrderBy(p => p.LIBELLE ).ToList());
               userkeyedby.AddRange(users.Where(user => user.FONCTION  != "430" ).OrderBy(p => p.LIBELLE ).ToList());
               useropenby.AddRange(users.OrderBy(p => p.LIBELLE ).ToList());
               cbo_openby.ItemsSource = useropenby;
               cbo_ref.ItemsSource = usercaisse;
               cbo_keydby.ItemsSource = userkeyedby;

               cbo_keydby.IsEnabled = false;
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

        private void cbo_keydby_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbo_keydby.SelectedItem == null)
                return;

            CsUtilisateur user = cbo_keydby.SelectedItem as CsUtilisateur;
            txt_keyby.Text = user.MATRICULE  ;
        }

        private void cbo_openby_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbo_openby.SelectedItem == null)
                return;

            CsUtilisateur user = cbo_openby.SelectedItem as CsUtilisateur;
            txt_openby.Text = user.MATRICULE;
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void rdb_direct_Checked(object sender, RoutedEventArgs e)
        {
            txt_keyby.IsEnabled = cbo_keydby.IsEnabled = !(rdb_direct.IsChecked == true);
        }

        private void rdb_manual_Checked(object sender, RoutedEventArgs e)
        {
            txt_keyby.IsEnabled = cbo_keydby.IsEnabled = (rdb_manual.IsChecked == true);
        }

        private void btn_Search_Click(object sender, RoutedEventArgs e)
        {
            aParam critere = new aParam();
            critere.Cashier = cbo_ref.SelectedItem == null ? string.Empty : cbo_ref.SelectedItem == FisrtUser ? string.Empty : (cbo_ref.SelectedItem as CsUtilisateur).MATRICULE == null ? string.Empty : (cbo_ref.SelectedItem as CsUtilisateur).MATRICULE;
            critere.KeyedBy = cbo_keydby.SelectedItem == null ? string.Empty : cbo_keydby.SelectedItem == FisrtUser ? string.Empty : (cbo_keydby.SelectedItem as CsUtilisateur).MATRICULE == null ? string.Empty : (cbo_keydby.SelectedItem as CsUtilisateur).MATRICULE;
            critere.Who = cbo_openby.SelectedItem == null ? string.Empty : cbo_openby.SelectedItem == FisrtUser ? string.Empty : (cbo_openby.SelectedItem as CsUtilisateur).MATRICULE == null ? string.Empty : (cbo_openby.SelectedItem as CsUtilisateur).MATRICULE;
            critere.isDirect = txt_keyby.IsEnabled == false ;
            critere.Day = dtpDate_paymentDay.SelectedDate.ToString();
            critere.When = dtp_openday.SelectedDate.ToString();
            AcceuilServiceClient accueilshearch = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
            accueilshearch.GET_OPENING_DAYSCompleted += (searchS, resultS) =>
            {
                if (resultS.Cancelled || resultS.Error != null)
                {
                    string error = resultS.Error.Message;
                    MessageBox.Show(Langue.ErrorMsgOnInvokingService, Langue.ErrorMsgTitleOnInvokingService, MessageBoxButton.OK);
                    var dialogResult = new DialogResult(Langue.ErrorMsgOnInvokingService,Langue.ErrorMsgTitleOnInvokingService, false, true, false);
                    desableProgressBar();
                    dgt_searchResult.ItemsSource = null;
                    return;
                }

                if (resultS.Result == null || resultS.Result.Count == 0)
                {
                    var dialogResult = new DialogResult(Langue.NoDataMsg,"", false, true, false);
                    dialogResult.Show();
                    //MessageBox.Show(Langue.NoDataMsg, "", MessageBoxButton.OK);
                    desableProgressBar();
                    dgt_searchResult.ItemsSource = null;
                    return;
                }

                searchList.AddRange(resultS.Result);
                dgt_searchResult.ItemsSource = resultS.Result;
            };
            accueilshearch.GET_OPENING_DAYSAsync(critere);
        }

        private void btn_reset_Click(object sender, RoutedEventArgs e)
        {
            ResetComponent();
        }

        private void ResetComponent()
        {
            try
            {
                cbo_keydby.SelectedItem = cbo_keydby.Items[0];
                cbo_openby.SelectedItem = cbo_openby.Items[0];
                cbo_ref.SelectedItem = cbo_ref.Items[0];

                rdb_direct.IsChecked = true;
                rdb_manual.IsChecked = false;

                dtp_openday.Text = string.Empty;
                dtpDate_paymentDay.Text = string.Empty;

                dgt_searchResult.ItemsSource = null;
            }
            catch (Exception ex)
            {

                string error = ex.Message ;
            }
        }

        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            Dictionary<string,string> rdlcParam = new Dictionary<string,string>();
            rdlcParam = CreateRdlcParam();
            AcceuilServiceClient accueilInit = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
            accueilInit.SetOpendaySearchToWebPartCompleted += (sprint, resultprint) =>
                { 
                
                };
            accueilInit.SetOpendaySearchToWebPartAsync(Utility.getKey(), searchList, rdlcParam);
        }

        private Dictionary<string, string> CreateRdlcParam()
        {
            try
            {
            Dictionary<string,string> rdlcParam = new Dictionary<string,string>();
            rdlcParam.Add("cashier", "");
            return rdlcParam;

            }
            catch (Exception ex )
            {
                string error = ex.Message;
                return null;
            }
        }
        
    }
}


