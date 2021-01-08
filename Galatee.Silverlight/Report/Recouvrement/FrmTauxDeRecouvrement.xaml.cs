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

namespace Galatee.Silverlight.Report
{
    public partial class FrmTauxDeRecouvrement : ChildWindow
    {
        public FrmTauxDeRecouvrement()
        {
            InitializeComponent();
            ChargerDonneeDuSite();
        }
        string leEtatExecuter = string.Empty;

        public FrmTauxDeRecouvrement(string typeEtat)
        {
            InitializeComponent();
            ChargerDonneeDuSite();
            leEtatExecuter = typeEtat;
            prgBar.Visibility = System.Windows.Visibility.Collapsed;
            
        }
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {

            if (this.Txt_LibelleSite.Tag == null)
            {
                Message.Show("Sélectionnez le site", "Information");
                return;
            }
            prgBar.Visibility = System.Windows.Visibility.Visible ;

            List<int> LsIdCentre =new List<int>();
            if (this.Txt_LibelleCentre.Tag != null)
                LsIdCentre = (List<int>)this.Txt_LibelleCentre.Tag;
            else
                LsIdCentre = LstCentrePerimetre.Where(t => t.FK_IDCODESITE == (int)this.Txt_LibelleSite.Tag).Select(i => i.PK_ID).ToList();
           
            if (leEtatExecuter == SessionObject.TauxRecouvrement)
                TauxRecouvrement(LsIdCentre);
            else if (leEtatExecuter == SessionObject.TauxEncaissement )
                TauxEncaissement(LsIdCentre);
        }
        private void TauxRecouvrement(List<int> IdCentre)
        {
            Galatee.Silverlight.ServiceReport.ReportServiceClient service1 = new Galatee.Silverlight.ServiceReport.ReportServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Report"));
            service1.RetourneTauxDeRecouvrementCompleted += (sr, res) =>
            {
                prgBar.Visibility = System.Windows.Visibility.Collapsed;

                if (res != null && res.Cancelled)
                    return;

                if (res.Result != null && res.Result.Count != 0)
                    Utility.ActionExportation<ServicePrintings.CsLclient, ServiceReport.CsLclient>(res.Result, null, string.Empty, SessionObject.CheminImpression, "TauxRecouvrement", "Report", true, "xlsx");
                else
                {
                    Message.ShowInformation("Aucune information trouvée", "Report");
                    return;
                }
            };
            service1.RetourneTauxDeRecouvrementAsync(IdCentre);
            service1.CloseAsync();

        }
        private void TauxEncaissement(List<int> IdCentre)
        {
            Galatee.Silverlight.ServiceReport.ReportServiceClient service1 = new Galatee.Silverlight.ServiceReport.ReportServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Report"));
            service1.RetourneTauxDeEncaissementCompleted += (sr, res) =>
            {
                prgBar.Visibility = System.Windows.Visibility.Collapsed;

                if (res != null && res.Cancelled)
                    return;

                if (res.Result != null && res.Result.Count != 0)
                    Utility.ActionExportation<ServicePrintings.CsLclient, ServiceReport.CsLclient>(res.Result, null, string.Empty, SessionObject.CheminImpression, "TauxEncaissement", "Report", true, "xlsx");
                else
                {
                    Message.ShowInformation("Aucune information trouvée", "Report");
                    return;
                }
            };
            service1.RetourneTauxDeEncaissementAsync(IdCentre);
            service1.CloseAsync();

        }
        private void AvanceSurConsomation(List<int> IdCentre, DateTime dateDebut, DateTime dateFin)
        {
            Galatee.Silverlight.ServiceReport.ReportServiceClient service1 = new Galatee.Silverlight.ServiceReport.ReportServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Report"));
            service1.ReturneAvanceSurConsoCompleted += (sr, res) =>
            {
                if (res != null && res.Cancelled)
                    return;

                if (res.Result != null && res.Result.Count != 0)
                    Utility.ActionDirectOrientation<ServicePrintings.CsLclient, ServiceReport.CsLclient>(res.Result, null, SessionObject.CheminImpression, "AvanceSurConsomation", "Report", true);
                else
                {
                    Message.ShowInformation("Aucune information trouvée", "Report");
                    return;
                }
            };
            service1.ReturneAvanceSurConsoAsync(IdCentre, dateDebut, dateFin);
            service1.CloseAsync();

        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
        List<Galatee.Silverlight.ServiceRecouvrement.CsUtilisateur> lstPia = new List<Galatee.Silverlight.ServiceRecouvrement.CsUtilisateur>();
        List<Galatee.Silverlight.ServiceAccueil.CsCentre> LstCentrePerimetre = new List<ServiceAccueil.CsCentre>();
        List<Galatee.Silverlight.ServiceAccueil.CsSite> lstSite = new List<Galatee.Silverlight.ServiceAccueil.CsSite>();
        Galatee.Silverlight.ServiceAccueil.CsSite lSiteSelect = new Galatee.Silverlight.ServiceAccueil.CsSite();
        List<Galatee.Silverlight.ServiceAccueil.CsProduit> lProduitSelect = new List<Galatee.Silverlight.ServiceAccueil.CsProduit>();
        private void ChargerDonneeDuSite()
        {
            try
            {
                List<int> lstCentreSelect = new List<int>();
                if (SessionObject.LstCentre != null && SessionObject.LstCentre.Count > 0)
                {
                    LstCentrePerimetre = Shared.ClasseMEthodeGenerique.RetourCentreByPerimetre(SessionObject.LstCentre.Where(p => p.CODE != SessionObject.Enumere.Generale).ToList(), UserConnecte.listeProfilUser);
                    lstSite = Shared.ClasseMEthodeGenerique.RetourneSiteByCentre(LstCentrePerimetre);
                    if (lstSite.Count == 1)
                    {
                        this.Txt_LibelleSite.Text = lstSite.First().LIBELLE;
                        this.Txt_LibelleSite.Tag = lstSite.First().PK_ID;
                        this.btn_Site.Tag = lstSite.First().CODE;
                        this.btn_Site.IsEnabled = false;
                    }
                    if (LstCentrePerimetre.Count == 1)
                    {
                        this.Txt_LibelleCentre.Text = LstCentrePerimetre.First().LIBELLE;
                        this.btn_Centre.IsEnabled = false;
                    }
                    return;
                }
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.ListeDesDonneesDesSiteCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    SessionObject.LstCentre = args.Result;
                    LstCentrePerimetre = Shared.ClasseMEthodeGenerique.RetourCentreByPerimetre(SessionObject.LstCentre.Where(p => p.CODE != SessionObject.Enumere.Generale).ToList(), UserConnecte.listeProfilUser);
                    lstSite = Shared.ClasseMEthodeGenerique.RetourneSiteByCentre(LstCentrePerimetre);
                    if (lstSite.Count == 1)
                    {
                        this.Txt_LibelleSite.Text = lstSite.First().LIBELLE;
                        this.btn_Site.Tag = lstSite.First().CODE;
                        this.Txt_LibelleSite.Tag = lstSite.First().PK_ID;
                        this.btn_Site.IsEnabled = false;
                    }
                    if (LstCentrePerimetre.Count == 1)
                    {
                        this.Txt_LibelleCentre.Text = LstCentrePerimetre.First().LIBELLE;
                        this.btn_Centre.IsEnabled = false;
                    }
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
        private void btn_Site_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (lstSite.Count > 0)
                {
                    this.btn_Site.IsEnabled = false;
                    List<object> _Listgen = Galatee.Silverlight.Shared.ClasseMEthodeGenerique.RetourneListeObjet(lstSite);
                    Galatee.Silverlight.MainView.UcListeGenerique ctr = new Galatee.Silverlight.MainView.UcListeGenerique(_Listgen, "CODE", "LIBELLE", "Liste");
                    ctr.Closed += new EventHandler(galatee_OkClickedSite);
                    ctr.Show();
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message,"Report");
            }

        }
        void galatee_OkClickedSite(object sender, EventArgs e)
        {
            Galatee.Silverlight.MainView.UcListeGenerique ctrs = sender as Galatee.Silverlight.MainView.UcListeGenerique;
            if (ctrs.isOkClick)
            {
                Galatee.Silverlight.ServiceAccueil.CsSite leSite = (Galatee.Silverlight.ServiceAccueil.CsSite)ctrs.MyObject;
                this.Txt_LibelleSite.Text = leSite.LIBELLE;
                this.Txt_LibelleSite.Tag = leSite.PK_ID;
                List<ServiceAccueil.CsCentre> lsiteCentre = LstCentrePerimetre.Where(t => t.FK_IDCODESITE == (int)this.Txt_LibelleSite.Tag).ToList();
                if (lsiteCentre.Count == 1)
                {
                    List<int> lstIdCentre = new List<int>();
                    lstIdCentre.Add(lsiteCentre.First().PK_ID);
                    this.Txt_LibelleCentre.Text = lsiteCentre.First().LIBELLE;
                    this.Txt_LibelleCentre.Tag = lstIdCentre;
                    this.btn_Centre.IsEnabled = true;
                }
                else
                {
                    this.Txt_LibelleCentre.Text = string.Empty;
                    this.Txt_LibelleCentre.Tag = null;
                    this.btn_Centre.IsEnabled = true;
                }
            }
            this.btn_Site.IsEnabled = true;

        }

        private void btn_Centre_Click(object sender, RoutedEventArgs e)
        {
            if (this.Txt_LibelleSite.Tag != null)
            {
                List<ServiceAccueil.CsCentre> lstCentreSite = LstCentrePerimetre.Where(t => t.FK_IDCODESITE == (int)this.Txt_LibelleSite.Tag).ToList();
                if (lstCentreSite.Count > 0)
                {
                    this.btn_Centre.IsEnabled = false;
                    List<object> _Listgen = Galatee.Silverlight.Shared.ClasseMEthodeGenerique.RetourneListeObjet(lstCentreSite);
                    Galatee.Silverlight.MainView.UcListeGenerique ctr = new Galatee.Silverlight.MainView.UcListeGenerique(_Listgen, "CODE", "LIBELLE", "Liste");
                    ctr.Closed += new EventHandler(galatee_OkClickedClient);
                    ctr.Show();
                }

            }
        }
        void galatee_OkClickedClient(object sender, EventArgs e)
        {
            Galatee.Silverlight.MainView.UcListeGenerique ctrs = sender as Galatee.Silverlight.MainView.UcListeGenerique;
            if (ctrs.isOkClick)
            {
                ServiceAccueil.CsCentre leCentre = (ServiceAccueil.CsCentre)ctrs.MyObject;

                List<int> lstIdCentre = new List<int>();
                lstIdCentre.Add(leCentre.PK_ID);
                this.Txt_LibelleCentre.Text = leCentre.LIBELLE;
                this.Txt_LibelleCentre.Tag = lstIdCentre;

                
            }
            this.btn_Centre.IsEnabled = true;
        }
    }
}

