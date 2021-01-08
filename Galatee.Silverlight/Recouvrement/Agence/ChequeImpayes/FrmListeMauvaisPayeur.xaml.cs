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

namespace Galatee.Silverlight.Recouvrement
{
    public partial class FrmListeMauvaisPayeur : ChildWindow
    {
        public FrmListeMauvaisPayeur()
        {
            InitializeComponent();
            ChargerDonneeDuSite();
        }
        List<int> lesCentreCaisse = new List<int>();
        List<Galatee.Silverlight.ServiceAccueil.CsCentre> LstCentrePerimetre = new List<ServiceAccueil.CsCentre>();
        List<Galatee.Silverlight.ServiceAccueil.CsSite> lstSite = new List<ServiceAccueil.CsSite>();
        private void ChargerDonneeDuSite()
        {
            try
            {
                if (SessionObject.LstCentre != null && SessionObject.LstCentre.Count > 0)
                {
                    LstCentrePerimetre = Shared.ClasseMEthodeGenerique.RetourCentreByPerimetre(SessionObject.LstCentre.Where(p => p.CODE != SessionObject.Enumere.Generale).ToList(), UserConnecte.listeProfilUser);
                    lstSite = Galatee.Silverlight.Shared.ClasseMEthodeGenerique.RetourneSiteByCentre(LstCentrePerimetre);
                    foreach (Galatee.Silverlight.ServiceAccueil.CsCentre item in LstCentrePerimetre)
                        lesCentreCaisse.Add(item.PK_ID);

                    if (lstSite.Count == 1)
                    {
                        this.Txt_CodeSite.Text = lstSite.First().CODE;
                        this.Txt_LibelleSite.Text = lstSite.First().LIBELLE;
                        this.Txt_CodeSite.Tag = lstSite.First().CODE;
                        this.btn_Site.IsEnabled = false;


                    }
                    if (LstCentrePerimetre.Count == 1)
                    {
                        this.Txt_LibelleCentre.Text = LstCentrePerimetre.First().LIBELLE;

                        lstCentreSelect.Add(LstCentrePerimetre.First());
                        this.Cbo_Centre.ItemsSource = null;
                        this.Cbo_Centre.ItemsSource = lstCentreSelect;
                        this.Cbo_Centre.DisplayMemberPath = "LIBELLE";
                    }
                    else
                    {
                        this.Txt_LibelleCentre.Visibility = System.Windows.Visibility.Collapsed;
                        this.Cbo_Centre.Visibility = System.Windows.Visibility.Visible;
                    }
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
                    foreach (Galatee.Silverlight.ServiceAccueil.CsCentre item in LstCentre)
                        lesCentreCaisse.Add(item.PK_ID);
                };
                service.ListeDesDonneesDesSiteAsync(false);
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
                if (lstSite.Count > 0)
                {
                    this.btn_Site.IsEnabled = false;
                    Dictionary<string, string> _LstColonneAffich = new Dictionary<string, string>();
                    _LstColonneAffich.Add("CODE", "SITE");
                    _LstColonneAffich.Add("LIBELLE", "LIBELLE");

                    List<object> obj = Shared.ClasseMEthodeGenerique.RetourneListeObjet(lstSite);
                    MainView.UcListeGenerique ctrl = new MainView.UcListeGenerique(obj, _LstColonneAffich, false, "Lots");
                    ctrl.Closed += new EventHandler(galatee_OkClickedSite);
                    ctrl.Show();
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, "btn_Site_Click");
            }
        }

        void galatee_OkClickedSite(object sender, EventArgs e)
        {
            lstCentreSelect.Clear();
            Galatee.Silverlight.MainView.UcListeGenerique ctrs = sender as Galatee.Silverlight.MainView.UcListeGenerique;
            if (ctrs.isOkClick)
            {
                this.btn_Site.IsEnabled = true;
                Galatee.Silverlight.ServiceAccueil.CsSite leSite = (Galatee.Silverlight.ServiceAccueil.CsSite)ctrs.MyObject;
                this.Txt_CodeSite.Text = leSite.CODE;
                this.Txt_CodeSite.Tag = leSite.CODE;
                this.Txt_LibelleSite.Text = leSite.LIBELLE;
                List<ServiceAccueil.CsCentre> lsiteCentre = LstCentrePerimetre.Where(t => t.CODESITE == this.Txt_CodeSite.Tag.ToString()).ToList();
                if (lsiteCentre.Count == 1)
                {
                    this.Txt_LibelleCentre.Text = LstCentrePerimetre.First().LIBELLE;
                    this.Txt_LibelleCentre.Visibility = System.Windows.Visibility.Visible;
                    this.btn_Centre.Visibility = System.Windows.Visibility.Visible;
                    this.Txt_LibelleCentre.Visibility = System.Windows.Visibility.Visible;


                    lstCentreSelect.AddRange(lsiteCentre);
                    this.Cbo_Centre.ItemsSource = null;
                    this.Cbo_Centre.DisplayMemberPath = "LIBELLE";
                    this.Cbo_Centre.ItemsSource = lstCentreSelect;
                    this.Cbo_Centre.SelectedItem = lsiteCentre;
                    this.Cbo_Centre.Visibility = System.Windows.Visibility.Collapsed;

                }
                else
                {
                    this.btn_Centre.Visibility = System.Windows.Visibility.Visible;
                    this.Txt_LibelleCentre.Visibility = System.Windows.Visibility.Collapsed;
                    this.Cbo_Centre.Visibility = System.Windows.Visibility.Visible;
                }
            }
            else
                this.btn_Site.IsEnabled = true;
        }

        private void btn_Centre_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (LstCentrePerimetre.Count > 0)
                {
                    this.btn_Centre.IsEnabled = false;
                    List<object> obj = Shared.ClasseMEthodeGenerique.RetourneListeObjet(LstCentrePerimetre.Where(t => t.CODESITE == this.Txt_CodeSite.Tag.ToString()).ToList());
                    Dictionary<string, string> _LstColonneAffich = new Dictionary<string, string>();
                    _LstColonneAffich.Add("CODE", "CENTRE");
                    _LstColonneAffich.Add("LIBELLE", "LIBELLE");
                    MainView.UcListeGenerique ctrl = new MainView.UcListeGenerique(obj, _LstColonneAffich, true, "Liste");
                    ctrl.Closed += new EventHandler(galatee_OkClickedCentre);
                    ctrl.Show();
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, "btn_Centre_Click");
            }
        }
        List<ServiceAccueil.CsCentre> lstCentreSelect = new List<ServiceAccueil.CsCentre>();
        void galatee_OkClickedCentre(object sender, EventArgs e)
        {
            Galatee.Silverlight.MainView.UcListeGenerique ctrs = sender as Galatee.Silverlight.MainView.UcListeGenerique;
            if (ctrs.isOkClick)
            {
                this.btn_Centre.IsEnabled = true;
                lstCentreSelect.Clear();
                foreach (var p in ctrs.MyObjectList)
                    lstCentreSelect.Add((Galatee.Silverlight.ServiceAccueil.CsCentre)p);

                this.Cbo_Centre.ItemsSource = null;
                this.Cbo_Centre.ItemsSource = lstCentreSelect;
                if (lstCentreSelect.Count != 0)
                    this.Cbo_Centre.SelectedItem = lstCentreSelect.First();
                this.Cbo_Centre.DisplayMemberPath = "LIBELLE";
            }
            else
                this.btn_Centre.IsEnabled = true;

        }
        private void ChargerMauvaisPayeur(List<int> lstIdCentre, DateTime? Datedebut, DateTime? Datefin)
        {
            try
            {
                RecouvrementServiceClient proxy = new RecouvrementServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Recouvrement"));
                proxy.ListeDesMauvaisPayerAsync(lstIdCentre, Datedebut, Datefin);
                proxy.ListeDesMauvaisPayerCompleted += (ssn, results) =>
                {
                    try
                    {
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
                        lvwResultat.ItemsSource = null;
                        lvwResultat.ItemsSource = results.Result;

                    }
                    catch (Exception ex)
                    {
                        Message.ShowError(ex, Galatee.Silverlight.Resources.Langue.errorTitle);
                    }
                };
            }
            catch (Exception ex)
            {


                throw ex;
            }
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (lvwResultat.ItemsSource != null)
            {
                List<CsDetailCampagne> lstDonnees = (List<CsDetailCampagne>)lvwResultat.ItemsSource;
                Utility.ActionDirectOrientation<ServicePrintings.CsDetailCampagne, ServiceRecouvrement.CsDetailCampagne>(lstDonnees, null, SessionObject.CheminImpression, "MauvaisPayeurs", "Recouvrement", true);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void Btn_search_Click(object sender, RoutedEventArgs e)
        {
            List<int> lstCentre = new List<int>();
            foreach (ServiceAccueil.CsCentre item in lstCentreSelect)
                lstCentre.Add(item.PK_ID);

            if (this.dtp_fin.SelectedDate == null) this.dtp_fin.SelectedDate = System.DateTime.Today;
            ChargerMauvaisPayeur(lstCentre,this.dtp_Debut.SelectedDate, this.dtp_fin.SelectedDate );
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
             if (lvwResultat.ItemsSource != null)
             {
                 List<CsDetailCampagne> lstDonnees = (List<CsDetailCampagne>)lvwResultat.ItemsSource;
                 Utility.ActionExportation<ServicePrintings.CsDetailCampagne, ServiceRecouvrement.CsDetailCampagne>(lstDonnees, null, string.Empty, SessionObject.CheminImpression, "MauvaisPayeurs", "Recouvrement", true, "xlsx");
             }
        }
    }
}

