using Galatee.Silverlight.ServiceRecouvrement;
using Galatee.Silverlight.Shared;
using Galatee.Silverlight.Tarification.Helper;
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

namespace Galatee.Silverlight.Recouvrement
{
    public partial class FrmFactureHorRegroupement : ChildWindow
    {
        #region EventHandling

        public delegate void MethodeEventHandler(object sender, CustumEventArgs e);
        public event MethodeEventHandler CallBack;
        CustumEventArgs MyEventArg = new CustumEventArgs();


        protected virtual void OnEvent(CustumEventArgs e)
        {
            if (CallBack != null)
                CallBack(this, e);
        }

        #endregion

        List<CsLclient> ListFacture_Selectionner = new List<CsLclient>();

        public FrmFactureHorRegroupement()
        {
            InitializeComponent();
            ChargerDonneeDuSite();
        }


        private void ChargerDonneeDuSite()
        {
            try
            {
                if (SessionObject.LstCentre.Count != 0)
                {
                    LstCentre = SessionObject.LstCentre;
                    cbo_centre.ItemsSource = LstCentre;
                    cbo_centre.DisplayMemberPath = "LIBELLE";
                    cbo_centre.SelectedValuePath = "PK_ID";

                    lstSite = ClasseMEthodeGenerique.RetourneSiteByCentre(LstCentre);
                    if (lstSite != null)
                    {
                        List<Galatee.Silverlight.ServiceAccueil.CsSite> _LstSite = lstSite.Where(p => p.CODE != SessionObject.Enumere.Generale).ToList();
                        if (_LstSite.Count > 0)
                        {
                            cbo_Site.ItemsSource = _LstSite;
                            cbo_Site.DisplayMemberPath = "LIBELLE";
                            cbo_Site.SelectedValuePath = "CODE";
                        }
                    }
                    return;
                }
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                service.ListeDesDonneesDesSiteCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    SessionObject.LstCentre = args.Result;
                    LstCentre = SessionObject.LstCentre;
                    cbo_centre.ItemsSource = LstCentre;
                    cbo_centre.DisplayMemberPath = "CODECENTRE";
                    cbo_centre.SelectedValuePath = "PK_ID";



                    lstSite = ClasseMEthodeGenerique.RetourneSiteByCentre(LstCentre);
                    if (lstSite != null)
                    {
                        List<Galatee.Silverlight.ServiceAccueil.CsSite> _LstSite = lstSite.Where(p => p.CODE != SessionObject.Enumere.Generale).ToList();
                        if (_LstSite.Count > 0)
                        {
                            cbo_Site.ItemsSource = _LstSite;
                            cbo_Site.DisplayMemberPath = "LIBELLE";
                            cbo_Site.SelectedValuePath = "CODE";
                        }
                    }
                };
                service.ListeDesDonneesDesSiteAsync(false);
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, "ChargerDonneeDuSite");

            }
        }



        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            MyEventArg.Bag = ListFacture_Selectionner;
            OnEvent(MyEventArg);
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            allowProgressBar();


            CsClient leClient = new CsClient();
            leClient.CENTRE = txt_centre.Text;
            leClient.REFCLIENT  = txt_client.Text;
            leClient.ORDRE  = txt_ordre.Text;

            if (string.IsNullOrWhiteSpace(txt_centre.Text) || string.IsNullOrWhiteSpace(txt_client.Text) || string.IsNullOrWhiteSpace(txt_ordre.Text))
            {
                Message.ShowInformation("Le centre ,la reference client et l'ordre sont obligatoire", "Information");
                return;
            }

            Galatee.Silverlight.ServiceRecouvrement.RecouvrementServiceClient service = new Galatee.Silverlight.ServiceRecouvrement.RecouvrementServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Recouvrement"));
            service.RetourneListeFactureNonSoldeGCAsync(leClient);
            service.RetourneListeFactureNonSoldeGCCompleted += (s, args) =>
            {
                if (args != null && args.Cancelled)
                {
                    Message.ShowError("Une erreur est survenu lors du traitement", "Erreur");
                }
                if (args.Result==null || args.Result.Count<=0)
                {
                    Message.ShowInformation("Aucune données corresponte aux critères", "Information");
                }
                if (!string.IsNullOrEmpty(this.txt_periode.Text))
                dg_facture.ItemsSource = args.Result.Where(t=>t.REFEM ==this.txt_periode.Text).ToList();
                else
                    dg_facture.ItemsSource = args.Result;

                desableProgressBar();
                return;
            };
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            ListFacture_Selectionner.Add((CsLclient)dg_facture.SelectedItem);
        }
        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            ListFacture_Selectionner.Remove((CsLclient)dg_facture.SelectedItem);
        }

        private void dg_facture_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        void allowProgressBar()
        {
            progressBar1.IsEnabled = true;
            progressBar1.Visibility = Visibility.Visible;
            progressBar1.IsIndeterminate = true;
        }
        void desableProgressBar()
        {
            progressBar1.IsIndeterminate = false;
            progressBar1.Visibility = Visibility.Collapsed;
        }

        public List<ServiceAccueil.CsCentre> LstCentre = new List<ServiceAccueil.CsCentre>();

        public List<ServiceAccueil.CsSite> lstSite = new List<ServiceAccueil.CsSite>();

        private void cbo_Site_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbo_Site.SelectedItem!=null)
            {
                LstCentre = SessionObject.LstCentre;
                txt_site.Text = ((Galatee.Silverlight.ServiceAccueil.CsSite)cbo_Site.SelectedItem).CODE;
                var DataSource = LstCentre.Where(c => c.CODESITE == ((Galatee.Silverlight.ServiceAccueil.CsSite)cbo_Site.SelectedItem).CODE);
                if (DataSource != null)
                {
                    cbo_centre.ItemsSource = DataSource;
                    cbo_centre.DisplayMemberPath = "LIBELLE";
                    cbo_centre.SelectedValuePath = "PK_ID";
                } 
            }
        }

        private void cbo_centre_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbo_centre.SelectedItem!=null)
            {
                txt_centre.Text = ((Galatee.Silverlight.ServiceAccueil.CsCentre)cbo_centre.SelectedItem).CODE; 
            }
        }

        private void txt_site_TextChanged(object sender, TextChangedEventArgs e)
        {
            //cbo_Site.SelectedItem=  lstSite.FirstOrDefault(s => s.CODE == txt_centre.Text);
        }

        private void txt_centre_TextChanged(object sender, TextChangedEventArgs e)
        {
            //cbo_centre.SelectedItem = LstCentre.FirstOrDefault(s => s.CODE == txt_centre.Text);
        }
    }
}

