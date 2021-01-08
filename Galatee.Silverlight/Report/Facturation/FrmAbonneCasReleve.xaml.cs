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
    public partial class FrmAbonneCasReleve : ChildWindow
    {
        public FrmAbonneCasReleve()
        {
            InitializeComponent();
            ChargerDonneeDuSite();
            ChargerListeDeProduit();
            ChargerListeDesCas();
            ChargerTournee();
        }

        List<int> lesCentreCaisse = new List<int>();
        List<Galatee.Silverlight.ServiceAccueil.CsCentre> LstCentrePerimetre = new List<ServiceAccueil.CsCentre>();
        List<Galatee.Silverlight.ServiceAccueil.CsSite> lstSite = new List<Galatee.Silverlight.ServiceAccueil.CsSite>();
        Galatee.Silverlight.ServiceAccueil.CsSite lSiteSelect = new Galatee.Silverlight.ServiceAccueil.CsSite();
        List<Galatee.Silverlight.ServiceAccueil.CsProduit> lProduitSelect = new List<Galatee.Silverlight.ServiceAccueil.CsProduit>();
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

                    if (lstSite != null && lstSite.Count != 0)
                    {
                        if (lstSite.Count == 1)
                        {
                            lSiteSelect = lstSite.First();
                            this.Txt_LibelleSite.Text = lstSite.First().LIBELLE;
                        }
                    }
                    if (LstCentrePerimetre.Count == 1)
                    {
                        this.Txt_LibelleCentre.Text = LstCentrePerimetre.First().CODE;
                        this.Txt_LibelleCentre.Text = LstCentrePerimetre.First().LIBELLE;
                        this.Txt_LibelleCentre.Tag = LstCentrePerimetre.First();
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
                    lstSite = Galatee.Silverlight.Shared.ClasseMEthodeGenerique.RetourneSiteByCentre(LstCentrePerimetre);
                    foreach (Galatee.Silverlight.ServiceAccueil.CsCentre item in LstCentrePerimetre)
                        lesCentreCaisse.Add(item.PK_ID);
                    if (lstSite != null && lstSite.Count != 0)
                    {
                        if (lstSite.Count == 1)
                        {
                            lSiteSelect = lstSite.First();
                            this.Txt_LibelleSite.Text = lstSite.First().LIBELLE;
                        }
                    }
                    if (LstCentrePerimetre != null && LstCentrePerimetre.Count != 0)
                    {
                        if (LstCentrePerimetre.Count == 1)
                        {
                            this.Txt_LibelleCentre.Text = LstCentrePerimetre.First().LIBELLE;
                            this.Txt_LibelleCentre.Tag = LstCentrePerimetre.First().PK_ID;
                            this.btn_Centre.IsEnabled = false;
                        }
                    }
                };
                service.ListeDesDonneesDesSiteAsync(false);
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void ChargerListeDeProduit()
        {
            try
            {
                if (SessionObject.ListeDesProduit != null && SessionObject.ListeDesProduit.Count != 0)
                    return;
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service1 = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service1.ListeDesProduitCompleted += (sr, res) =>
                {
                    if (res != null && res.Cancelled)
                        return;
                    SessionObject.ListeDesProduit = res.Result;
                };
                service1.ListeDesProduitAsync();
                service1.CloseAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void ChargerListeDesCas()
        {
            if (SessionObject.LstDesCas.Count != 0)
                return;
            Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
            service.RetourneListeDesCasCompleted += (s, args) =>
            {
                if (args != null && args.Cancelled)
                    return;
                SessionObject.LstDesCas = args.Result;
            };
            service.RetourneListeDesCasAsync();
            service.CloseAsync();
        }

        private void ChargerTournee()
        {
            if (SessionObject.LstZone.Count != 0)
                return;
            Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
            service.ChargerLesTourneesCompleted += (s, args) =>
            {
                if (args != null && args.Cancelled)
                    return;
                SessionObject.LstZone = args.Result;
            };
            service.ChargerLesTourneesAsync();
            service.CloseAsync();
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
                Message.ShowError(ex.Message, "Report");
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
                lSiteSelect = leSite;
                List<ServiceAccueil.CsCentre> lsiteCentre = LstCentrePerimetre.Where(t => t.FK_IDCODESITE == (int)this.Txt_LibelleSite.Tag).ToList();
                if (lsiteCentre.Count == 1)
                {
                    this.Txt_LibelleCentre.Text = lsiteCentre.First().LIBELLE;
                    this.Txt_LibelleCentre.Tag = lsiteCentre.First();
                    lProduitSelect = lsiteCentre.First().LESPRODUITSDUSITE;
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
                this.Txt_LibelleCentre.Text = leCentre.LIBELLE;
                this.Txt_LibelleCentre.Tag = leCentre;
                lProduitSelect = leCentre.LESPRODUITSDUSITE;
            }
            this.btn_Centre.IsEnabled = true;
        }


        private void btnCas_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<ServiceAccueil.CParametre> lstParametre = Shared.ClasseMEthodeGenerique.RetourneValueFromClasse<ServiceAccueil.CsCasind >(SessionObject.LstDesCas );
                Shared.UcListeParametre ctr = new Galatee.Silverlight.Shared.UcListeParametre(lstParametre, true, "Liste de cas");
                ctr.Closed += new EventHandler(categorie_OkClicked);
                ctr.Show();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
            }

        }
        private void categorie_OkClicked(object sender, EventArgs e)
        {
            try
            {
                Shared.UcListeParametre generiq = sender as Shared.UcListeParametre;
                if (generiq.isOkClick)
                {
                    List<ServiceAccueil.CParametre> ListeCategorie = new List<ServiceAccueil.CParametre>();

                    if (generiq.MyObjectList.Count != 0)
                    {
                        int passage = 1;
                        foreach (var p in generiq.MyObjectList)
                        {
                            ListeCategorie.Add((ServiceAccueil.CParametre)p);
                            if (passage == 1)
                                this.Txt_LibelleCas.Text = p.CODE;
                            else
                                this.Txt_LibelleCas.Text = this.Txt_LibelleCas.Text + "  " + p.CODE;
                            passage++;
                        }
                        this.Txt_LibelleCas.Tag = ListeCategorie.Select(t => t.PK_ID).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
            }
        }


        private void AbonneParCasDeReleve(List<int> lstCentre, List<int?> lstTourne, string Periode, string lotri)
        {
            //Galatee.Silverlight.ServiceReport.ReportServiceClient service1 = new Galatee.Silverlight.ServiceReport.ReportServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Report"));
            //service1.ReturneNombreMoyenDeFacturationCompleted += (sr, res) =>
            //{
            //    if (res != null && res.Cancelled)
            //        return;

            //    if (res.Result != null && res.Result.Count != 0)
            //        Utility.ActionDirectOrientation<ServicePrintings.CsEvenement, ServiceReport.CsEvenement>(res.Result, null, SessionObject.CheminImpression, "Demande", "Report", true);
            //    else
            //    {
            //        Message.ShowInformation("Aucune information trouvée", "Report");
            //        return;
            //    }
            //};
            //service1.ReturneNombreMoyenDeFacturationAsync(lstCentre, lstTourne, Periode, lotri);
            //service1.CloseAsync();


        }


        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void btnzone_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (SessionObject.LstZone == null || SessionObject.LstZone.Count == 0)
                    return;

                if (this.Txt_LibelleCentre.Tag == null)
                {
                    Message.ShowInformation("Selectionnez le centre", "Repporting");
                    return;
                }
                List<ServiceAccueil.CsTournee> lstTourneCentre = SessionObject.LstZone.Where(t => t.FK_IDCENTRE == (int)this.Txt_LibelleCentre.Tag).ToList();
                List<ServiceAccueil.CParametre> lstParametre = Shared.ClasseMEthodeGenerique.RetourneValueFromClasse<ServiceAccueil.CsTournee>(lstTourneCentre);
                Shared.UcListeParametre ctr = new Galatee.Silverlight.Shared.UcListeParametre(lstParametre, true, "Tournée");
                ctr.Closed += new EventHandler(zones_OkClicked);
                ctr.Show();

            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
            }

        }
        private void zones_OkClicked(object sender, EventArgs e)
        {
            try
            {
                Shared.UcListeParametre ctrs = sender as Shared.UcListeParametre;
                if (ctrs.isOkClick)
                {
                    List<ServiceAccueil.CParametre> ListeDesZones = new List<ServiceAccueil.CParametre>();
                    if (ctrs.IsMultiselect)
                    {
                        int passage = 1;
                        foreach (var p in ctrs.MyObjectList)
                        {
                            ListeDesZones.Add((ServiceAccueil.CParametre)p);
                            if (passage == 1)
                                this.Txt_LibelleTournee.Text = p.CODE;
                            else
                                this.Txt_LibelleTournee.Text = this.Txt_LibelleTournee.Text + "  " + p.CODE;
                            passage++;
                        }
                        this.Txt_LibelleTournee.Tag = ListeDesZones.Select(t => t.PK_ID).ToList();
                    }

                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
            }
        }
    }
}

