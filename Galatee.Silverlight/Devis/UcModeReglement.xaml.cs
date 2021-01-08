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
using Galatee.Silverlight.ServiceAccueil ;

namespace Galatee.Silverlight.Devis
{
    public partial class UcModeReglement : ChildWindow
    {
        private ObjDEVIS MyDevis = new ObjDEVIS();
        private string NumDevis;
        public ObjDEPOSIT Deposit {get;set;}

        public UcModeReglement()
        {
            InitializeComponent();
        }

        public UcModeReglement(string pNumDevis)
        {
            InitializeComponent();
            NumDevis = pNumDevis;
            if(Deposit == null)
                Deposit = new ObjDEPOSIT();
        }
        public UcModeReglement(int idDemande)
        {
            InitializeComponent();
            //NumDevis = pNumDevis;
            //if (Deposit == null)
            //    Deposit = new ObjDEPOSIT();
        }
        private void RemplirBanque()
        {
            try
            {
                AcceuilServiceClient client = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                client.SelectAllBanqueCompleted += (ssender, args) =>
                {
                    if (args.Cancelled || args.Error != null)
                    {
                        string error = args.Error.Message;
                        Message.Show(error, Galatee.Silverlight.Resources.Parametrage.Languages.Banque);
                        return;
                    }
                    if (args.Result != null)
                    {
                        foreach (aBanque item in args.Result)
                            Cbo_Banque.Items.Add(item);
                        Cbo_Banque.SelectedValuePath = "PK_ID";
                        Cbo_Banque.DisplayMemberPath = "LIBELLE";
                    }
                };
                client.SelectAllBanqueAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Convert.ToBoolean(this.Rdb_Banque.IsChecked) && Cbo_Banque.SelectedItem != null)
                {
                    this.Deposit.BANQUE = (this.Cbo_Banque.SelectedItem as aBanque).CODE ;
                    //this.Deposit.COMPTE = (this.Cbo_Banque.SelectedItem as aBanque).;
                }
                this.DialogResult = true;
                //this.Close();
            }
            catch (Exception ex)
            {
                Message.Show(ex.Message, Galatee.Silverlight.Resources.Devis.Languages.txtDevis);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                this.Txt_NumDevis.Text = this.NumDevis;
                this.OKButton.IsEnabled = ((Convert.ToBoolean(Rdb_Agence.IsChecked) || (this.Cbo_Banque.SelectedItem != null)));
                this.RemplirBanque();
                SelectionAgence();
                SelectionBanque();
            }
            catch (Exception ex)
            {
                Message.Show(ex.Message, Galatee.Silverlight.Resources.Devis.Languages.txtDevis);
            }
        }

        private void Cbo_Banque_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                 if (Convert.ToBoolean(Rdb_Banque.IsChecked))
                     this.OKButton.IsEnabled = (this.Cbo_Banque.SelectedItem != null);
            else
                     this.OKButton.IsEnabled = true;
                 if (this.Cbo_Banque.SelectedItem != null)
                     this.Txt_NumeroDeCompte.Text = !string.IsNullOrEmpty((this.Cbo_Banque.SelectedItem as aBanque).CODE ) ? (this.Cbo_Banque.SelectedItem as aBanque).CODE  : string.Empty;
            }
            catch (Exception ex)
            {
                Message.Show(ex.Message, Galatee.Silverlight.Resources.Devis.Languages.txtDevis);
            }
        }

        private void Rdb_Agence_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
            //    this.Rdb_Banque.IsChecked = !this.Rdb_Agence.IsChecked;
            //    if(Convert.ToBoolean(Rdb_Banque.IsChecked))
            //        this.Cbo_Banque.Visibility = this.Txt_NumeroDeCompte.Visibility = lab_Banque.Visibility = lab_NumCompte.Visibility = System.Windows.Visibility.Visible;
            //    else
            //        this.Cbo_Banque.Visibility = this.Txt_NumeroDeCompte.Visibility = lab_Banque.Visibility = lab_NumCompte.Visibility = System.Windows.Visibility.Collapsed;
            //if (Convert.ToBoolean(this.Rdb_Banque.IsChecked))
            //    this.OKButton.IsEnabled = (this.Cbo_Banque.SelectedIndex != -1);
            //else
            //    this.OKButton.IsEnabled = true;
            }
            catch (Exception ex)
            {
                Message.Show(ex.Message, Galatee.Silverlight.Resources.Devis.Languages.txtDevis);
            }
        }

        private void Rdb_Banque_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                //this.Rdb_Agence.IsChecked = !this.Rdb_Banque.IsChecked;
                //if (Convert.ToBoolean(Rdb_Banque.IsChecked))
                //    this.Cbo_Banque.Visibility = this.Txt_NumeroDeCompte.Visibility = lab_Banque.Visibility = lab_NumCompte.Visibility = System.Windows.Visibility.Visible; 
                //else
                //    this.Cbo_Banque.Visibility = this.Txt_NumeroDeCompte.Visibility = lab_Banque.Visibility = lab_NumCompte.Visibility = System.Windows.Visibility.Collapsed; 

                //if (Convert.ToBoolean(this.Rdb_Banque.IsChecked))
                //    this.OKButton.IsEnabled = (this.Cbo_Banque.SelectedIndex != -1);
                //else
                //    this.OKButton.IsEnabled = true;
            }
            catch (Exception ex)
            {
                Message.Show(ex.Message, Galatee.Silverlight.Resources.Devis.Languages.txtDevis);
            }
        }

        private void Rdb_Agence_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SelectionAgence();
            }
            catch (Exception ex)
            {
                Message.Show(ex.Message, Galatee.Silverlight.Resources.Devis.Languages.txtDevis);
            }
        }

        private void SelectionAgence()
        {
            try
            {
                this.Rdb_Banque.IsChecked = !this.Rdb_Agence.IsChecked;
                if (Convert.ToBoolean(Rdb_Banque.IsChecked))
                    this.Cbo_Banque.Visibility = this.Txt_NumeroDeCompte.Visibility = lab_Banque.Visibility = lab_NumCompte.Visibility = System.Windows.Visibility.Visible;
                else
                    this.Cbo_Banque.Visibility = this.Txt_NumeroDeCompte.Visibility = lab_Banque.Visibility = lab_NumCompte.Visibility = System.Windows.Visibility.Collapsed;
                if (Convert.ToBoolean(this.Rdb_Banque.IsChecked))
                    this.OKButton.IsEnabled = (this.Cbo_Banque.SelectedItem != null);
                else
                    this.OKButton.IsEnabled = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void Rdb_Banque_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SelectionBanque();
            }
            catch (Exception ex)
            {
                Message.Show(ex.Message, Galatee.Silverlight.Resources.Devis.Languages.txtDevis);
            }
        }

        private void SelectionBanque()
        {
            try
            {
                this.Rdb_Agence.IsChecked = !this.Rdb_Banque.IsChecked;
                if (Convert.ToBoolean(Rdb_Banque.IsChecked))
                    this.Cbo_Banque.Visibility = this.Txt_NumeroDeCompte.Visibility = lab_Banque.Visibility = lab_NumCompte.Visibility = System.Windows.Visibility.Visible;
                else
                    this.Cbo_Banque.Visibility = this.Txt_NumeroDeCompte.Visibility = lab_Banque.Visibility = lab_NumCompte.Visibility = System.Windows.Visibility.Collapsed;

                if (Convert.ToBoolean(this.Rdb_Banque.IsChecked))
                    this.OKButton.IsEnabled = (this.Cbo_Banque.SelectedItem != null);
                else
                    this.OKButton.IsEnabled = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

