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
using Galatee.Silverlight.Resources;
using System.Threading;
using Galatee.Silverlight.Resources.Devis;

namespace Galatee.Silverlight.Devis
{
    public partial class UcAbonnePicker : ChildWindow
    {
        private string produitSelectionne = string.Empty;

        public UcAbonnePicker()
        {
            InitializeComponent();
            Initialisation();
        }


        public UcAbonnePicker(string pProduit)
        {
            InitializeComponent();
            produitSelectionne = pProduit;
            Initialisation();
        }

        private void RemplirCentre()
        {
            try
            {
                LayoutRoot.Cursor = Cursors.Wait;
                var client =  new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                client.SelectAllCentreCompleted += (ssender, args) =>
                                                       {
                                                           if (args.Cancelled || args.Error != null)
                                                           {
                                                               LayoutRoot.Cursor = Cursors.Arrow;
                                                               string error = args.Error.Message;
                                                               Message.Show(error, Languages.txtDevis);
                                                               return;
                                                           }

                                                           Cbo_Centre.Items.Clear();
                                                           foreach (var item in args.Result)
                                                           {
                                                               Cbo_Centre.Items.Add(item);
                                                           }

                                                           Cbo_Centre.SelectedValuePath = "CodeCentre";
                                                           Cbo_Centre.DisplayMemberPath = "Libelle";

                                                           LayoutRoot.Cursor = Cursors.Arrow;
                                                       };
                client.SelectAllCentreAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void Initialisation()
        {
            try
            {
                RemplirCentre();
            }
            catch (Exception ex)
            {
                Message.Show(ex.Message, Languages.txtDevis);
            }

        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Dtg_abonne.SelectedItem != null)
                {
                    //abonneSelectionne = Dtg_abonne.SelectedItem as cClient;
                    this.DialogResult = true;
                }
                else
                {
                    throw new Exception(Languages.msgEmptyUser);
                }
            }
            catch (Exception ex)
            {
                Message.Show(ex.Message, Languages.txtDevis);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void Cbo_Centre_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (Cbo_Centre.SelectedItem != null)
                {
                    var objCentre = Cbo_Centre.SelectedItem as  ServiceAccueil.CsCentre;
                    if (objCentre != null)
                        Txt_Centre.Text = objCentre.CODE;
                }
            }
            catch (Exception ex)
            {
                Message.Show(ex.Message, Languages.txtDevis);
            }
        }

        private void Btn_search_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Recherche();
            }
            catch (Exception ex)
            {
                Message.Show(ex.Message, Languages.txtDevis);
            }
        }
        void Translate()
        {
            this.Lab_Centre.Content = Languages.lblCentre;
            this.Lab_Client.Content = Languages.lbl_Client;
            this.Lab_Nom.Content = Languages.lblNom;
            this.Btn_reset.Content = Languages.Btn_reset;
            this.Btn_search.Content = Languages.Btn_search;
        }
        private void Recherche()
        {
            //try
            //{
            //    LayoutRoot.Cursor = Cursors.Wait;
            //    Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient clientDevis = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
            //    busyIndicator.IsBusy = true;
            //    BtnOK.IsEnabled = false;
            //    //CsClient critere = new CsClient();

            //    string CLIENT = (string.IsNullOrEmpty(this.Txt_Client.Text)) ? null : this.Txt_Client.Text;
            //    string NomAbonne = (string.IsNullOrEmpty(this.Txt_Nom.Text)) ? null : this.Txt_Nom.Text;
            //    string CENTRE = (string.IsNullOrEmpty(this.Txt_Centre.Text))
            //                         ? null
            //                         : this.Txt_Centre.Text;

            //    clientDevis.SearchListClientCompleted += (sen, result) =>
            //                                                 {
            //                                                     if (result.Cancelled || result.Error != null)
            //                                                     {
            //                                                         LayoutRoot.Cursor = Cursors.Arrow;
            //                                                         string error = result.Error.Message;
            //                                                         Message.Show(error, Languages.txtDevis);
            //                                                         BtnOK.IsEnabled = true;
            //                                                         busyIndicator.IsBusy = false;
            //                                                         return;
            //                                                     }
            //                                                     if (result.Result.Count == 0)
            //                                                     {
            //                                                         LayoutRoot.Cursor = Cursors.Arrow;
            //                                                         Message.Show(
            //                                                             "Aucun abonné ne correspond aux critères saisis !",
            //                                                             Languages.txtDevis);
            //                                                     }
                                                                     
            //                                                     if (result.Result != null && result.Result.Count > 0)
            //                                                     {
            //                                                         this.Dtg_abonne.ItemsSource = result.Result;
            //                                                     }
            //                                                     BtnOK.IsEnabled = true;
            //                                                     busyIndicator.IsBusy = false;
            //                                                     LayoutRoot.Cursor = Cursors.Arrow;
            //                                                 };
            //    clientDevis.SearchListClientAsync(CENTRE,CLIENT,NomAbonne, produitSelectionne);
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
        }

        private void Btn_reset_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.Dtg_abonne.ItemsSource = null;
                this.Cbo_Centre.SelectedItem = null;
                this.Txt_Centre.Text = string.Empty;
                this.Txt_Nom.Text = string.Empty;
                this.Txt_Centre.Text = string.Empty;
                //abonneSelectionne = null;
            }
            catch (Exception ex)
            {
                Message.Show(ex.Message, Languages.txtDevis);
            }
        }
    }
}

