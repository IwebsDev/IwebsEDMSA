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
    public partial class FrmNombreDeJourMoyen : ChildWindow
    {
        public FrmNombreDeJourMoyen()
        {
            InitializeComponent();
            ChargerDonneeDuSite();
            ChargerListeDeProduit();
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
        private void NombreDeJourMoyen(string CodeSite, string Periode )
        {
            Galatee.Silverlight.ServiceReport.ReportServiceClient service1 = new Galatee.Silverlight.ServiceReport.ReportServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Report"));
            service1.ReturneNombreMoyenDeFacturationCompleted += (sr, res) =>
            {
                if (res != null && res.Cancelled)
                    return;

                if (res.Result != null && res.Result.Count != 0)
                {
                    string Rdlc = string.Empty;
                    Rdlc = "NombreMoyenDeJour";
                    if (OptionImpression == SessionObject.EnvoiPrinter)
                        Utility.ActionDirectOrientation<ServicePrintings.CsEvenement, ServiceReport.CsEvenement>(res.Result, param, SessionObject.CheminImpression, Rdlc, "Report", true);
                    else if (OptionImpression == SessionObject.EnvoiExecl)
                        Utility.ActionExportation<ServicePrintings.CsEvenement, ServiceReport.CsEvenement>(res.Result, param, string.Empty, SessionObject.CheminImpression, Rdlc, "Report", true, "xlsx");

                    else if (OptionImpression == SessionObject.EnvoiWord)
                        Utility.ActionExportation<ServicePrintings.CsEvenement, ServiceReport.CsEvenement>(res.Result, param, string.Empty, SessionObject.CheminImpression, Rdlc, "Report", true, "doc");

                    else if (OptionImpression == SessionObject.EnvoiPdf)
                        Utility.ActionExportation<ServicePrintings.CsEvenement, ServiceReport.CsEvenement>(res.Result, param, string.Empty, SessionObject.CheminImpression, Rdlc, "Report", true, "pdf");

                }
                else
                {
                    Message.ShowInformation("Aucune information trouvée", "Report");
                    return;
                }
            };
            service1.ReturneNombreMoyenDeFacturationAsync(CodeSite, Periode);
            service1.CloseAsync();
        
        
        }
     
        Dictionary<string, string> param = null;
        string OptionImpression = SessionObject.EnvoiPdf;
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                Galatee.Silverlight.Shared.FrmOptionEditon ctrl = new Shared.FrmOptionEditon();
                ctrl.Closed += ctrl_Closed;
                this.IsEnabled = false;
                ctrl.Show();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        void ctrl_Closed(object sender, EventArgs e)
        {
            this.IsEnabled = true;
            Galatee.Silverlight.Shared.FrmOptionEditon ctrs = sender as Galatee.Silverlight.Shared.FrmOptionEditon;
            if (ctrs.IsOptionChoisi)
            {
                OptionImpression = ctrs.OptionSelect;
                if (lSiteSelect != null && !string.IsNullOrEmpty(this.txt_Periode.Text))
                    NombreDeJourMoyen(lSiteSelect.CODE, Shared.ClasseMEthodeGenerique.FormatPeriodeAAAAMM(this.txt_Periode.Text));

            }
        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}

