using Galatee.Silverlight.ServiceRecouvrement;
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

namespace Galatee.Silverlight.Precontentieux
{
    public partial class FrmListeMoratoireEmis : ChildWindow
    {
        public FrmListeMoratoireEmis()
        {
            InitializeComponent();
            prgBar.Visibility = System.Windows.Visibility.Collapsed  ;
            this.Rdb_Tout.IsChecked = true;
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (lvwResultat.ItemsSource != null && (lstSource != null && lstSource.Count != 0))
            {
                Utility.ActionDirectOrientation<ServicePrintings.CsDetailMoratoire, ServiceRecouvrement.CsDetailMoratoire>(lstSource, null, SessionObject.CheminImpression, "SuivieMoratoire", "Recouvrement", true);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
           Galatee.Silverlight.Shared.ClasseMEthodeGenerique.FermetureEcran(this);
        }
        List<int> IdDesCentre = new List<int>();
        List<ServiceAccueil.CsSite> lstSite = new List<ServiceAccueil.CsSite>();
        List<ServiceAccueil.CsCentre> lesCentre = new List<ServiceAccueil.CsCentre>();
        private void ChargerDonneeDuSite()
        {
            try
            {
                if (SessionObject.LstCentre != null && SessionObject.LstCentre.Count != 0)
                {
                    lesCentre = Shared.ClasseMEthodeGenerique.RetourCentreByPerimetre(SessionObject.LstCentre, UserConnecte.listeProfilUser);
                    lstSite = Shared.ClasseMEthodeGenerique.RetourneSiteByCentre(lesCentre);
                    if (lstSite.Count == 1)
                    {
                        this.Txt_Site.Text = lstSite.First().CODE;
                        txt_LibelleSite.Text = lstSite.First().LIBELLE;
                        this.Txt_Site.Tag = lstSite.First().PK_ID;
                    }
                    if (lesCentre.Count == 1)
                    {
                        this.Txt_Centre.Text = lesCentre.First().CODE;
                        txt_libellecentre.Text = lesCentre.First().LIBELLE;
                        this.Txt_Centre.Tag = lesCentre.First().PK_ID;
                    }
                    foreach (ServiceAccueil.CsCentre item in lesCentre)
                        IdDesCentre.Add(item.PK_ID);
                    return;

                }

                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.ListeDesDonneesDesSiteCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    SessionObject.LstCentre = args.Result;
                    List<int> IdDesCentre = new List<int>();
                    lesCentre = Shared.ClasseMEthodeGenerique.RetourCentreByPerimetre(SessionObject.LstCentre, UserConnecte.listeProfilUser);
                    lstSite = Shared.ClasseMEthodeGenerique.RetourneSiteByCentre(lesCentre);
                    if (lstSite.Count == 1)
                    {
                        this.Txt_Site.Text = lstSite.First().CODE;
                        txt_LibelleSite.Text = lstSite.First().LIBELLE;
                        this.Txt_Site.Tag = lstSite.First().PK_ID;
                    }
                    if (lesCentre.Count == 1)
                    {
                        this.Txt_Centre.Text = lesCentre.First().CODE;
                        txt_libellecentre.Text = lesCentre.First().LIBELLE;
                        this.Txt_Centre.Tag = lesCentre.First().PK_ID;
                    }
                    foreach (ServiceAccueil.CsCentre item in lesCentre)
                        IdDesCentre.Add(item.PK_ID);
                };
                service.ListeDesDonneesDesSiteAsync(true);
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
                List<string> _LstColonneAffich = new List<string>();
                _LstColonneAffich.Add("CODE");
                _LstColonneAffich.Add("LIBELLE");
                List<object> obj = Shared.ClasseMEthodeGenerique.RetourneListeObjet(lstSite);
                MainView.UcListeGenerique ctrl = new MainView.UcListeGenerique(obj, _LstColonneAffich, false, "Site");
                ctrl.Closed += new EventHandler(galatee_OkClickedSite);
                ctrl.Show();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
            }
        }

        void galatee_OkClickedSite(object sender, EventArgs e)
        {
            try
            {
                MainView.UcListeGenerique ctrs = sender as MainView.UcListeGenerique;
                if (ctrs.isOkClick)
                {
                    ServiceAccueil.CsSite param = (ServiceAccueil.CsSite)ctrs.MyObject;//.VALEUR;
                    this.Txt_Site.Text = param.CODE;
                    txt_LibelleSite.Text = param.LIBELLE;
                    this.Txt_Site.Tag = param.PK_ID;
                    this.btn_Centre.IsEnabled = true;
                    List<ServiceAccueil.CsCentre> lsiteCentre = lesCentre.Where(t => t.FK_IDCODESITE  ==(int) this.Txt_Site.Tag).ToList();
                    if (lsiteCentre.Count == 1)
                    {
                        this.Txt_Centre.Text = lsiteCentre.First().CODE;
                        this.txt_libellecentre .Text = lsiteCentre.First().LIBELLE;
                        this.Txt_Centre .Tag = lsiteCentre.First().PK_ID;
                    }

                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
            }
        }
        private void btn_Centre_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<string> _LstColonneAffich = new List<string>();
                _LstColonneAffich.Add("CODE");
                _LstColonneAffich.Add("LIBELLE");
                List<object> obj = Shared.ClasseMEthodeGenerique.RetourneListeObjet(lesCentre.Where(t => t.FK_IDCODESITE == (int)this.Txt_Site.Tag).ToList());
                MainView.UcListeGenerique ctrl = new MainView.UcListeGenerique(obj, _LstColonneAffich, false, "Lots");
                ctrl.Closed += new EventHandler(galatee_OkClickedCentre);
                ctrl.Show();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
            }
        }

        void galatee_OkClickedCentre(object sender, EventArgs e)
        {
            try
            {
                MainView.UcListeGenerique ctrs = sender as MainView.UcListeGenerique;
                ServiceAccueil.CsCentre param = (ServiceAccueil.CsCentre)ctrs.MyObject;//.VALEUR;
                this.Txt_Centre.Text = param.CODE;
                txt_libellecentre.Text = param.LIBELLE;
                this.Txt_Centre.Tag = param.PK_ID;
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
            }
        }

        private void btn_Rechercher_Click(object sender, RoutedEventArgs e)
        {
            this.btn_Rechercher.IsEnabled = false;
            List<int> lesCentreSelect = new List<int>();
            if (this.Txt_Centre.Tag != null)
                lesCentreSelect.Add((int)this.Txt_Centre.Tag);
            else
                lesCentreSelect = IdDesCentre;

            ChargerMauvaisPayeur(lesCentreSelect, this.dtp_Debut.SelectedDate, this.dtp_fin.SelectedDate);
        }
        List<CsDetailMoratoire> lstSource = new List<CsDetailMoratoire>();
        List<CsDetailMoratoire> dataTable = new List<CsDetailMoratoire>();
        private void ChargerMauvaisPayeur(List<int> lstIdCentre, DateTime? Datedebut, DateTime? Datefin)
        {
            try
            {
                prgBar.Visibility = System.Windows.Visibility.Visible;
                RecouvrementServiceClient proxy = new RecouvrementServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Recouvrement"));
                proxy.ListeDesMoratoiresEmisAsync(lstIdCentre, Datedebut, Datefin,true );
                proxy.ListeDesMoratoiresEmisCompleted += (ssn, results) =>
                {
                    try
                    {
                        this.btn_Rechercher.IsEnabled = true ;
                        prgBar.Visibility = System.Windows.Visibility.Collapsed ;
                        if (results.Cancelled || results.Error != null)
                        {
                            string error = results.Error.Message;
                            Message.ShowError("Erreur d'invocation du service. Réessayer svp !", Galatee.Silverlight.Resources.Langue.errorTitle);
                            return;
                        }
                        if (results.Result == null || results.Result.Count == 0)
                        {
                            Message.ShowError("Aucune donnée retournée!", Galatee.Silverlight.Resources.Langue.errorTitle);
                            return;
                        }
                        dataTable = Shared.ClasseMEthodeGenerique.RetourneListCopy<CsDetailMoratoire >(results.Result);
                        List<CsDetailMoratoire> lstMoratoir = dataTable;

                        if (this.Rdb_Echu.IsChecked == true)
                            lstMoratoir = lstMoratoir.Where(t => t.EXIGIBILITE.Value.Date < System.DateTime.Today.Date).ToList();
                        foreach (var item in lstMoratoir)
                        {
                            item.REFERENCE = item.CENTRE + " " + item.CLIENT + " " + item.ORDRE;
                            item.NATURE = (item.EXIGIBILITE.Value.Date < System.DateTime.Today.Date) ? "ECHU" : string.Empty;
                            CsDetailMoratoire le = new CsDetailMoratoire();
                            le = item;
                            if (lstSource.FirstOrDefault(t => t.REFERENCE == le.REFERENCE) != null)
                            {
                                le.REFERENCE = string.Empty;
                                le.NOMABON  = string.Empty;
                            }
                            lstSource.Add(le);
                        }

                        lvwResultat.ItemsSource = null;
                        lvwResultat.ItemsSource = lstSource;

                    }
                    catch (Exception ex)
                    {
                        this.btn_Rechercher.IsEnabled = true;
                        Message.ShowError(ex, Galatee.Silverlight.Resources.Langue.errorTitle);
                    }
                };
            }
            catch (Exception ex)
            {


                throw ex;
            }
        }

        private void Txt_Centre_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (lvwResultat.ItemsSource != null && (lstSource != null && lstSource.Count != 0))
            {
                Utility.ActionExportation<ServicePrintings.CsDetailMoratoire, ServiceRecouvrement.CsDetailMoratoire>(dataTable, null, string.Empty, SessionObject.CheminImpression, "SuivieMoratoire", "Recouvrement", true, "xlsx");
            }
        }

        private void ChildWindow_Loaded_1(object sender, RoutedEventArgs e)
        {
            ChargerDonneeDuSite();

        }

        private void Rdb_Echu_Checked(object sender, RoutedEventArgs e)
        {
            if (dataTable != null && dataTable.Count != 0 && lstSource != null && lstSource.Count != 0)
            {
                List<CsDetailMoratoire> lstMoratoir = dataTable.Where(t => t.EXIGIBILITE.Value.Date < System.DateTime.Today.Date).ToList();
                AfficherDataGrid(lstMoratoir);
            }
        }
        private void AfficherDataGrid(List<CsDetailMoratoire> lstMoratoir)
        {
            List<CsDetailMoratoire> lstFinal = new List<CsDetailMoratoire>();
            if (this.Rdb_Echu.IsChecked == true)
            foreach (var item in lstMoratoir)
            {
                item.REFERENCE = item.CENTRE + " " + item.CLIENT + " " + item.ORDRE;
                item.NATURE = (item.EXIGIBILITE.Value.Date >= System.DateTime.Today.Date) ? "ECHU" : string.Empty;
                CsDetailMoratoire le = new CsDetailMoratoire();
                le = item;
                if (lstFinal.FirstOrDefault(t => t.REFERENCE == le.REFERENCE) != null)
                {
                    le.REFERENCE = string.Empty;
                    le.NOMABON = string.Empty;
                }
                lstFinal.Add(le);
            }

            lvwResultat.ItemsSource = null;
            lvwResultat.ItemsSource = lstMoratoir;
        
        }

        private void Rdb_Tout_Checked_1(object sender, RoutedEventArgs e)
        {
            if (dataTable != null && dataTable.Count != 0 )
            {
                List<CsDetailMoratoire> lstMoratoir = dataTable;
                AfficherDataGrid(lstMoratoir);
            }
        }
    }
}

