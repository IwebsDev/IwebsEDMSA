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
    public partial class FrmListeFacture : ChildWindow
    {
        public FrmListeFacture()
        {
            InitializeComponent();
            Top = 1;
            ChargerDonneeDuSite();
            ChargerListeDeProduit();
        }
        string TypeEdition = string.Empty;
        public FrmListeFacture(string _TypeOperation)
        {
            InitializeComponent();
            Top = 1;
            ChargerDonneeDuSite();
            ChargerListeDeProduit();
            TypeEdition = _TypeOperation;
        }
        List<int> lstCentre = new List<int>();
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
                        this.Txt_LibelleCentre.Tag = LstCentrePerimetre.First().PK_ID ;
                        lstCentre.Add(LstCentrePerimetre.First().PK_ID);
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
                            lstCentre.Add(LstCentrePerimetre.First().PK_ID);
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
                    lstCentre.Add(lsiteCentre.First().PK_ID);
                    this.Txt_LibelleCentre.Text = lsiteCentre.First().LIBELLE;
                    this.Txt_LibelleCentre.Tag = lstCentre;
                    this.btn_Centre.IsEnabled = true;
                }
                else
                {

                    lstCentre.AddRange(lsiteCentre.Where(p=>p.FK_IDCODESITE ==leSite.PK_ID ).Select(o=>o.PK_ID).ToList());

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
                this.Txt_LibelleCentre.Tag = leCentre.PK_ID ;
                lProduitSelect = leCentre.LESPRODUITSDUSITE;
            }
            this.btn_Centre.IsEnabled = true;
        }
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
        string OptionImpression = SessionObject.EnvoiPdf;
        void ctrl_Closed(object sender, EventArgs e)
        {
            this.IsEnabled = true;
            Galatee.Silverlight.Shared.FrmOptionEditon ctrs = sender as Galatee.Silverlight.Shared.FrmOptionEditon;
            if (ctrs.IsOptionChoisi)
            {
                OptionImpression = ctrs.OptionSelect;
                DateTime dateDebut = System.DateTime.Today;
                DateTime dateFin = dateDebut.AddYears(3);
             
                dateDebut = string.IsNullOrEmpty(this.dtp_DateDebut.Text) ? dateDebut : Convert.ToDateTime(this.dtp_DateDebut.Text);
                dateFin = string.IsNullOrEmpty(this.dtp_DateFin.Text) ? dateFin : Convert.ToDateTime(this.dtp_DateFin.Text);

                if (TypeEdition == SessionObject.FactureIsole)
                RetourneFactureIsole(lstCentre, dateDebut, dateFin);

                if (TypeEdition == SessionObject.FactureAnnuler)
                    RetourneAnnulation(lstCentre, dateDebut, dateFin);

            }
        }
       
        private void RetourneFactureIsole(List<int> lstCentre,DateTime DateDebut,DateTime DateFin)
        {
            Galatee.Silverlight.ServiceReport.ReportServiceClient service1 = new Galatee.Silverlight.ServiceReport.ReportServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Report"));
            service1.ReturneFactureIsoleCompleted += (sr, res) =>
            {
                if (res != null && res.Cancelled)
                    return;

                if (res.Result != null && res.Result.Count != 0)
                {
                        string Rdlc = "FactureIsole";
                        if (OptionImpression == SessionObject.EnvoiPrinter)
                            Utility.ActionDirectOrientation<ServicePrintings.CsEvenement, ServiceReport.CsEvenement>(res.Result, null, SessionObject.CheminImpression, Rdlc, "Report", true);
                        else if (OptionImpression == SessionObject.EnvoiExecl)
                            Utility.ActionExportation<ServicePrintings.CsEvenement, ServiceReport.CsEvenement>(res.Result, null, string.Empty, SessionObject.CheminImpression, Rdlc, "Report", true, "xlsx");

                        else if (OptionImpression == SessionObject.EnvoiWord)
                            Utility.ActionExportation<ServicePrintings.CsEvenement, ServiceReport.CsEvenement>(res.Result, null, string.Empty, SessionObject.CheminImpression, Rdlc, "Report", true, "doc");

                        else if (OptionImpression == SessionObject.EnvoiPdf)
                            Utility.ActionExportation<ServicePrintings.CsEvenement, ServiceReport.CsEvenement>(res.Result, null, string.Empty, SessionObject.CheminImpression, Rdlc, "Report", true, "pdf");
                }
                else
                {
                    Message.ShowInformation("Aucune information trouvée", "Report");
                    return;
                }
            };
            service1.ReturneFactureIsoleAsync(lstCentre, DateDebut, DateFin);
            service1.CloseAsync();


        }
        private void RetourneAnnulation(List<int> lstCentre, DateTime DateDebut, DateTime DateFin)
        {
            Galatee.Silverlight.ServiceReport.ReportServiceClient service1 = new Galatee.Silverlight.ServiceReport.ReportServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Report"));
            service1.ReturneAnnulationFactureTopCompleted += (sr, res) =>
            {
                if (res != null && res.Cancelled)
                    return;

                if (res.Result != null && res.Result.Count != 0)
                {
                    Dictionary<string, string> param = new Dictionary<string, string>();
                    //param.Add("pTop", Top.ToString());

                    if (Top == 1)
                        param.Add("pTop", "FACTURES");
                    else
                        param.Add("pTop", "TOP 3");
                      



                    res.Result.ForEach(p => p.DENABON = (this.dtp_DateDebut.SelectedDate.Value.Year.ToString() + this.dtp_DateDebut.SelectedDate.Value.Month.ToString("00"))); 
                    if (OptionImpression == SessionObject.EnvoiPrinter)
                        Utility.ActionDirectOrientation<ServicePrintings.CsEvenement, ServiceReport.CsEvenement>(res.Result, param, SessionObject.CheminImpression, "FactureAnnulation", "Report", true);
                    else if (OptionImpression == SessionObject.EnvoiExecl)
                        Utility.ActionExportation<ServicePrintings.CsEvenement, ServiceReport.CsEvenement>(res.Result, param, string.Empty, SessionObject.CheminImpression, "FactureAnnulation", "Report", true, "xlsx");

                    else if (OptionImpression == SessionObject.EnvoiWord)
                        Utility.ActionExportation<ServicePrintings.CsEvenement, ServiceReport.CsEvenement>(res.Result, param, string.Empty, SessionObject.CheminImpression, "FactureAnnulation", "Report", true, "doc");

                    else if (OptionImpression == SessionObject.EnvoiPdf)
                        Utility.ActionExportation<ServicePrintings.CsEvenement, ServiceReport.CsEvenement>(res.Result, param, string.Empty, SessionObject.CheminImpression, "FactureAnnulation", "Report", true, "pdf");
                }
                else
                {
                    Message.ShowInformation("Aucune information trouvée", "Report");
                    return;
                }
            };
            service1.ReturneAnnulationFactureTopAsync(lstCentre, DateDebut, DateFin,Top);
            service1.CloseAsync();


        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
        public int Top { get; set; }

        private void rbt_Top3_Checked(object sender, RoutedEventArgs e)
        {
            Top = 3;
        }

        private void rbt_Top1_Checked(object sender, RoutedEventArgs e)
        {
            Top = 1;
        }
    }
}

