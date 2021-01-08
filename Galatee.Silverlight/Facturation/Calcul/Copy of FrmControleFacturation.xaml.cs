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
using Galatee.Silverlight.ServiceFacturation;
using Galatee.Silverlight.Resources.Facturation;

namespace Galatee.Silverlight.Facturation
{
    public partial class FrmControleFacturation : ChildWindow
    {
        List<CsLotri> ListeLotri = new List<CsLotri>();
        public FrmControleFacturation()
        {
            InitializeComponent();
            ChargerDonneeDuSite();
        }
        List<int> lesCentrePerimetre = new List<int>();

        private void ChargerDonneeDuSite()
        {
            try
            {
                if (SessionObject.LstCentre != null && SessionObject.LstCentre.Count > 0)
                {
                    List<Galatee.Silverlight.ServiceAccueil.CsCentre> LstCentre = Shared.ClasseMEthodeGenerique.RetourCentreByPerimetre(SessionObject.LstCentre.Where(p => p.CODE != SessionObject.Enumere.Generale).ToList(), UserConnecte.listeProfilUser);
                    List<Galatee.Silverlight.ServiceAccueil.CsSite> lstSite = Galatee.Silverlight.Shared.ClasseMEthodeGenerique.RetourneSiteByCentre(LstCentre);
                    foreach (Galatee.Silverlight.ServiceAccueil.CsCentre item in LstCentre)
                        lesCentrePerimetre.Add(item.PK_ID);
                    ChargerLotriAll(lesCentrePerimetre);
                    return;
                }
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                service.ListeDesDonneesDesSiteCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    SessionObject.LstCentre = args.Result;
                    List<Galatee.Silverlight.ServiceAccueil.CsCentre> LstCentre = Shared.ClasseMEthodeGenerique.RetourCentreByPerimetre(SessionObject.LstCentre.Where(p => p.CODE != SessionObject.Enumere.Generale).ToList(), UserConnecte.listeProfilUser);
                    List<Galatee.Silverlight.ServiceAccueil.CsSite> lstSite = Galatee.Silverlight.Shared.ClasseMEthodeGenerique.RetourneSiteByCentre(LstCentre);
                    foreach (Galatee.Silverlight.ServiceAccueil.CsCentre item in LstCentre)
                        lesCentrePerimetre.Add(item.PK_ID);

                    ChargerLotriAll(lesCentrePerimetre);
                };
                service.ListeDesDonneesDesSiteAsync(false);
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid1.ItemsSource != null)
            {
                List<CsAnnomalie> ListeAnnomalie = (List<CsAnnomalie>)dataGrid1.ItemsSource;
                Utility.ActionDirectOrientation<ServicePrintings.CsAnnomalie, ServiceFacturation.CsAnnomalie>(ListeAnnomalie, null, SessionObject.CheminImpression, "AnomalieFacturation", "Facturation", true);
            }
        }
 
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Shared.ClasseMEthodeGenerique.FermetureEcran(this);
        }
        private void ChargerLotriAll(List<int> idCentre)
        {
            ListeLotri = new List<CsLotri>();
            FacturationServiceClient service = new FacturationServiceClient(Utility.Protocole(), Utility.EndPoint("Facturation"));
            service.ChargerLotriDejaFactureCompleted += (s, args) =>
            {
                if (args != null && args.Cancelled)
                    return;
                ListeLotri = args.Result;
                if (ListeLotri == null || ListeLotri.Count == 0)
                    return;
                ListeLotri = ListeLotri.ToList();
                foreach (CsLotri item in ListeLotri)
                  item.DATEEXIG  =  Convert.ToDateTime(Convert.ToDateTime(item.DFAC) + TimeSpan.FromDays((int)item.EXIG));

                dataGridlLot.ItemsSource = null;
                dataGridlLot.ItemsSource = ClasseMethodeGenerique.DistinctLotri(ListeLotri) ;
            };
            service.ChargerLotriDejaFactureAsync(idCentre);
            service.CloseAsync();
        }
        private void dgMyDataGrid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            DataGrid dg = (sender as DataGrid);
            var allObjects = dg.ItemsSource as List<CsLotri >;
            if (dg.SelectedItem != null)
            {

                CsLotri SelectedObject = (CsLotri)dg.SelectedItem;
                allObjects.Where(t => t.PK_ID != ((CsLotri)dg.SelectedItem).PK_ID).ToList().ForEach(t => t.IsSelect = false);
                if (SelectedObject.IsSelect == false)
                {
                    SelectedObject.IsSelect = true;
                    dataGrid1.ItemsSource = null;
                    dataGrid1.ItemsSource = ListeLotri.Where(t => t.NUMLOTRI == SelectedObject.NUMLOTRI).ToList();
                }
                else
                {
                    dataGrid1.ItemsSource = null;
                    SelectedObject.IsSelect = false;
                }
            }
        }

        private void chk_Checked_1(object sender, RoutedEventArgs e)
        {
            if (dataGridlLot.ItemsSource != null)
            {
                var lstSelect = ((List<CsLotri>)dataGridlLot.ItemsSource).ToList();
                if (lstSelect != null && this.dataGridlLot.SelectedItem != null)
                {
                    CsLotri leSelect = (CsLotri)this.dataGridlLot.SelectedItem;
                    if (leSelect == null)
                    {
                        Message.ShowInformation("Sélectionner le lot", "Facturation");
                        return;
                    }
                    leSelect.IsSelect = true;
                    dataGrid1.ItemsSource = null;
                    dataGrid1.ItemsSource = ListeLotri.Where(t => t.NUMLOTRI == leSelect.NUMLOTRI).ToList();
                }
            }
        }

        private void chk_Unchecked_1(object sender, RoutedEventArgs e)
        {
            if (dataGridlLot.ItemsSource != null)
            {
                var lstProduit = ((List<CsLotri>)dataGridlLot.ItemsSource).ToList();
                if (lstProduit != null && this.dataGridlLot.SelectedItem != null)
                {
                    CsLotri leSelect = (CsLotri)this.dataGridlLot.SelectedItem;
                    if (leSelect == null)
                    {
                        Message.ShowInformation("Sélectionner le lot", "Facturation");
                        return;
                    }
                    leSelect.IsSelect = false ;
                    dataGrid1.ItemsSource = null;
                }
            }
        }

        private void btn_AfficherAnnomalie_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridlLot.SelectedItem != null)
            {
                        CsLotri SelectedObject = (CsLotri)dataGridlLot.SelectedItem;
                        List<CsAnnomalie>   ListeAnnomalie = new List<CsAnnomalie >();
                        FacturationServiceClient service = new FacturationServiceClient(Utility.Protocole(), Utility.EndPoint("Facturation"));
                        service.RetourneControleFacturesCompleted += (s, args) =>
                        {
                            if (args != null && args.Cancelled)
                                return;
                            ListeAnnomalie = args.Result;
                            if (ListeAnnomalie == null || ListeAnnomalie.Count == 0)
                                return;

                            this.dataGrid1.ItemsSource = null;
                            this.dataGrid1.ItemsSource = ListeAnnomalie;
                        };
                        service.RetourneControleFacturesAsync(SelectedObject);
                        service.CloseAsync();

                
            }
        }

    }
}

