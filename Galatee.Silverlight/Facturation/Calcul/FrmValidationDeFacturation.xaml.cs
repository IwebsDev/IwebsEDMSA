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
    public partial class FrmValidationDeFacturation : ChildWindow
    {
        List<CsLotri> ListeLotri = new List<CsLotri>();
        public FrmValidationDeFacturation()
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
                    Dictionary<string, List<int>> lesDeCentre = new Dictionary<string, List<int>>();
                    foreach (ServiceAccueil.CsSite item in lstSite)
                        lesDeCentre.Add(item.CODE, LstCentre.Where(i => i.CODESITE == item.CODE).Select(p => p.PK_ID).ToList());
                    ChargerLotriAll(lesDeCentre);
                    return;
                }
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.ListeDesDonneesDesSiteCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    SessionObject.LstCentre = args.Result;
                    List<Galatee.Silverlight.ServiceAccueil.CsCentre> LstCentre = Shared.ClasseMEthodeGenerique.RetourCentreByPerimetre(SessionObject.LstCentre.Where(p => p.CODE != SessionObject.Enumere.Generale).ToList(), UserConnecte.listeProfilUser);
                    List<Galatee.Silverlight.ServiceAccueil.CsSite> lstSite = Galatee.Silverlight.Shared.ClasseMEthodeGenerique.RetourneSiteByCentre(LstCentre);
                    Dictionary<string, List<int>> lesDeCentre = new Dictionary<string, List<int>>();
                    foreach (ServiceAccueil.CsSite item in lstSite)
                        lesDeCentre.Add(item.CODE, LstCentre.Where(i => i.CODESITE == item.CODE).Select(p => p.PK_ID).ToList());
                    ChargerLotriAll(lesDeCentre);
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
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid1.ItemsSource != null)
            {
                List<CsLotri> _lesLotsSelect = (List<CsLotri>)dataGrid1.ItemsSource;
                DefacturationLot(_lesLotsSelect);
            }
            else
                Message.Show("Sélectionnez un lot", "");
        }
 
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Shared.ClasseMEthodeGenerique.FermetureEcran(this);
        }
        string Action = SessionObject.Enumere.Defacturation;
        private void DefacturationLot(List<CsLotri> ListLotSelect)
        {
            int res = LoadingManager.BeginLoading(Galatee.Silverlight.Resources.Accueil.Langue.En_Cours);
            try
            {
                CsStatFacturation _laStat = new CsStatFacturation();
                FacturationServiceClient service = new FacturationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Facturation"));
                service.DefacturerLotCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    _laStat = args.Result;
                    if (_laStat != null)
                        Message.Show("Nombre de client :" + _laStat.NombreCalcule + "\r\n  Montant défacturé : " + decimal.Parse(_laStat.Montant.ToString()).ToString("N2"), "Statistique");
                    this.OKButton.IsEnabled = true;
                    LoadingManager.EndLoading(res);
                    this.DialogResult = false;
                };
                service.DefacturerLotAsync(ListLotSelect, Action);
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                LoadingManager.EndLoading(res);
                throw ex;
            }
        }

        private void RejeterDefacturationLot(List<CsLotri> ListLotSelect)
        {
            int res = LoadingManager.BeginLoading(Galatee.Silverlight.Resources.Accueil.Langue.En_Cours);
            try
            {
                CsStatFacturation _laStat = new CsStatFacturation();
                FacturationServiceClient service = new FacturationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Facturation"));
                service.ValiderRejetDefacturationCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    if (args.Result)
                        Message.Show(Galatee.Silverlight.Resources.Facturation.Langue.msgRejetDefacturation,Galatee.Silverlight.Resources.Facturation.Langue.LibelleModule );
                    this.OKButton.IsEnabled = true;
                    LoadingManager.EndLoading(res);
                    this.DialogResult = false;
                };
                service.ValiderRejetDefacturationAsync(ListLotSelect, Action);
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                LoadingManager.EndLoading(res);
                throw ex;
            }
        }
        private void ChargerLotriAll(Dictionary<string, List<int>> lstSiteCentre)
        {
            ListeLotri = new List<CsLotri>();
            FacturationServiceClient service = new FacturationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Facturation"));
            service.ChargerLotriPourDefacturationCompleted += (s, args) =>
            {
                if (args != null && args.Cancelled)
                    return;
                ListeLotri = args.Result;
                if (ListeLotri == null || ListeLotri.Count == 0)
                    return;
                ListeLotri = ListeLotri.ToList();
                foreach (CsLotri item in ListeLotri)
                {
                    if (item.DFAC != null && item.EXIG != null )
                    item.DATEEXIG = Convert.ToDateTime(Convert.ToDateTime(item.DFAC) + TimeSpan.FromDays((int)item.EXIG));
                }
                dataGridlLot.ItemsSource = null;
                dataGridlLot.ItemsSource = ClasseMethodeGenerique.DistinctLotri(ListeLotri) ;
            };
            service.ChargerLotriPourDefacturationAsync(lstSiteCentre,UserConnecte.matricule ,true );
            service.CloseAsync();
        }

        private void OKButtonRejeter_Click(object sender, RoutedEventArgs e)
        {

            if (dataGrid1.ItemsSource != null)
            {
                List<CsLotri> _lesLotsSelect = (List<CsLotri>)dataGrid1.ItemsSource;
                RejeterDefacturationLot(_lesLotsSelect);
            }
            else
                Message.Show("Sélectionnez un lot", "");
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

    }
}

