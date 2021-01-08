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
using Galatee.Silverlight.MainView;
using Galatee.Silverlight.Shared;
using Galatee.Silverlight.ServiceFacturation;
using Galatee.Silverlight.Resources.Facturation ;
using Galatee.Silverlight;
using System.Collections.ObjectModel;
using Galatee.Silverlight.Facturation.Edition;

namespace Galatee.Silverlight.Facturation
{
    public partial class FrmMiseAJourFacture : ChildWindow
    {

        ObservableCollection<CsLotri> dataGridObjects = new ObservableCollection<CsLotri>();
        private List<CsLotri> ListeDesLotriAfficher = new List<CsLotri>();

        IList<CsLotri> lotSelectionnes = new List<CsLotri>();
       
        ObservableCollection<LstMoisCpt> moisComptable = new ObservableCollection<LstMoisCpt>();


        public FrmMiseAJourFacture()
        {
            try
            {
                this.Resources.Add("moisComptable", moisComptable);
                InitializeComponent();
                ChargerDonneeDuSite();
                prgBar.Visibility = System.Windows.Visibility.Collapsed;


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        List<int> IdDesCentre = new List<int>();

        private void ChargerDonneeDuSite()
        {
            try
            {
                if (SessionObject.LstCentre != null && SessionObject.LstCentre.Count != 0)
                {
                    List<ServiceAccueil.CsCentre> lesCentre = Shared.ClasseMEthodeGenerique.RetourCentreByPerimetre(SessionObject.LstCentre, UserConnecte.listeProfilUser);
                    List<ServiceAccueil.CsSite> lstSite = Shared.ClasseMEthodeGenerique.RetourneSiteByCentre(lesCentre);
                    foreach (ServiceAccueil.CsCentre item in lesCentre)
                        IdDesCentre.Add(item.PK_ID);
                    ChargerListeLotriPourMAJ(IdDesCentre);
                    return;

                }

                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.ListeDesDonneesDesSiteCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    SessionObject.LstCentre = args.Result;
                    List<ServiceAccueil.CsCentre> lesCentre = Shared.ClasseMEthodeGenerique.RetourCentreByPerimetre(SessionObject.LstCentre, UserConnecte.listeProfilUser);
                    List<ServiceAccueil.CsSite> lstSite = Shared.ClasseMEthodeGenerique.RetourneSiteByCentre(lesCentre);
                    foreach (ServiceAccueil.CsCentre item in lesCentre)
                        IdDesCentre.Add(item.PK_ID);
                    ChargerListeLotriPourMAJ(IdDesCentre);

                };
                service.ListeDesDonneesDesSiteAsync(true);
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        List<CsLotri> lstLotInit = new List<CsLotri>();

        private void ChargerListeLotriPourMAJ(List<int> lstCentre)
        {
            try
            {
                prgBar.Visibility = System.Windows.Visibility.Visible ;
                ListeDesLotriAfficher = new List<CsLotri>();
                FacturationServiceClient service = new FacturationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Facturation"));
                service.ChargerLotriPourMiseAJourAsync(lstCentre,UserConnecte.matricule );
                service.ChargerLotriPourMiseAJourCompleted += (s, args) =>
                {
                    prgBar.Visibility = System.Windows.Visibility.Collapsed;
                    try
                    {
                        if (args.Error != null && args.Cancelled)
                        {
                            Message.ShowError("Erreur d'invocation du service.", "Erreur");
                            return;
                        }

                        if (args.Result == null)
                        {
                            Message.ShowError("Aucune donnée retournée du système.", "Erreur");
                            return;
                        }
                        List<CsLotri> lstGolbalLot = new List<CsLotri>();
                        lstGolbalLot = args.Result ;
                        lstLotInit = lstGolbalLot; 
                        if (lstGolbalLot.Count != 0)
                            ListeDesLotriAfficher = Facturation.ClasseMethodeGenerique.DistinctLotriJetProduit(lstGolbalLot);
                        string MoisCpt =Shared.ClasseMEthodeGenerique.FormatPeriodeMMAAAA(  System.DateTime.Now.Year.ToString() + System.DateTime.Now.Month.ToString("00"));
                        ListeDesLotriAfficher.ForEach(t => t.MOISCOMPTA = MoisCpt);

                        dtgFactures.ItemsSource = null;
                        dtgFactures.ItemsSource = ListeDesLotriAfficher;
                    }
                    catch (Exception ex)
                    {
                        Message.Show("Erreur remplissage cmb :" + ex.Message, "Erreur inconnue");

                    }
                };
            }
            catch (Exception ex)
            {
                Message.Show("Erreur remplissage cmb :" + ex.Message, "Erreur inconnue");
            }
        }
        private void dgMyDataGrid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            DataGrid dg = (sender as DataGrid);
            var allObjects = dg.ItemsSource as ObservableCollection<CsLotri>;
            if (dg.SelectedItem != null)
            {
                CsLotri SelectedObject =( (CsLotri )dg.SelectedItem);
                if (SelectedObject.IsSelect  == false)
                    SelectedObject.IsSelect = true;
                else
                    SelectedObject.IsSelect = false;
            }
        }
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                prgBar.Visibility = System.Windows.Visibility.Visible;
                List<CsLotri> LotPourMaj = new List<CsLotri>();
                this.OKButton.IsEnabled = false;
                var  ligneLotriSelect = ((List<CsLotri>)dtgFactures.ItemsSource).Where(t => t.IsSelect == true).ToList();
                if (ligneLotriSelect != null && ligneLotriSelect.Count > 0)
                {
                    foreach (CsLotri item in ligneLotriSelect)
                    {
                        LotPourMaj.AddRange( lstLotInit.Where(t => t.NUMLOTRI == item.NUMLOTRI && t.PERIODE == item.PERIODE && t.JET == item.JET).ToList());
                        LotPourMaj.ForEach(t => t.MATRICULE = UserConnecte.matricule);
                        LotPourMaj.ForEach(t => t.MOISCOMPTA = Shared.ClasseMEthodeGenerique.FormatPeriodeAAAAMM(item.MOISCOMPTA));
                    }
                   
                    FacturationServiceClient service = new FacturationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Facturation"));
                    service.MiseAjourLotsAsync(LotPourMaj);
                    service.MiseAjourLotsCompleted += (erreur, resultat) =>
                    {
                        prgBar.Visibility = System.Windows.Visibility.Collapsed ;

                        if (resultat.Error != null || resultat.Cancelled)
                            Message.Show("Une erreur s'est produite", "Erreur");
                        else
                        {
                            CsStatFacturation _laStat = resultat.Result;
                            if (_laStat != null)
                            {
                                this.DialogResult = false;
                                UcResultatFacturation ctrl = new UcResultatFacturation(_laStat);
                                ctrl.Show();
                                prgBar.Visibility = System.Windows.Visibility.Collapsed;
                            }
                            this.OKButton.IsEnabled = true;
                        }
                    };

                }
                else
                {
                    Message.Show("Vous devez choisir un lot dans la liste", "Erreur");
                    this.OKButton.IsEnabled = true;
                    prgBar.Visibility = System.Windows.Visibility.Collapsed;
                    return;
                }

            }
            catch (Exception ex)
            {
                Message.Show("Erreur à la mise à jour", "Erreur");

            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void cbo_Mois_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            string   moisSelectionne =((LstMoisCpt) ((ComboBox)sender).SelectedItem).LeMois;

            var lotSelectionne = ((CsLotriViewModel)dtgFactures.SelectedItem);
            var dataSource = (ObservableCollection<CsLotriViewModel>)dtgFactures.ItemsSource;
            int index = dataSource.IndexOf(lotSelectionne);

            lotSelectionne.CsLotri.MOISCOMPTA = moisSelectionne.Substring(3, 4) + moisSelectionne.Substring(0, 2);
            lotSelectionne.CsLotri.IsSelect = true;
            dataSource[index] = lotSelectionne;
            dtgFactures.ItemsSource = dataSource;
        }

        private void btn_fermer_Click(object sender, RoutedEventArgs e)
        {

            this.DialogResult = false;
        }
    }
}

