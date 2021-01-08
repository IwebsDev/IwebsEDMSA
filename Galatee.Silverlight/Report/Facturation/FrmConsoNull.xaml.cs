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
    public partial class FrmConsoNull : ChildWindow
    {
        public FrmConsoNull()
        {
            InitializeComponent();
            ChargerDonneeDuSite();
        }
        List<int> lstCentre = new List<int>(); 
        List<Galatee.Silverlight.ServiceAccueil.CsCentre> LstCentrePerimetre = new List<ServiceAccueil.CsCentre>();
        List<Galatee.Silverlight.ServiceAccueil.CsSite> lstSite = new List<ServiceAccueil.CsSite>();
        private void ChargerDonneeDuSite()
        {
            try
            {
                List<int> lstCentreSelect = new List<int>();

                if (SessionObject.LstCentre != null && SessionObject.LstCentre.Count > 0)
                {
                    LstCentrePerimetre = Shared.ClasseMEthodeGenerique.RetourCentreByPerimetre(SessionObject.LstCentre.Where(p => p.CODE != SessionObject.Enumere.Generale).ToList(), UserConnecte.listeProfilUser);
                    lstSite =Shared.ClasseMEthodeGenerique.RetourneSiteByCentre(LstCentrePerimetre);
                    foreach (Galatee.Silverlight.ServiceAccueil.CsCentre item in LstCentrePerimetre)
                        lstCentre.Add(item.PK_ID);
                    if (lstSite.Count == 1)
                    {
                        this.Txt_CodeSite.Text = lstSite.First().CODE;
                        this.Txt_LibelleSite.Text = lstSite.First().LIBELLE;
                        this.Txt_CodeSite.Tag = lstSite.First().PK_ID;
                        this.btn_Site.IsEnabled = false;
                    }
                    if (LstCentrePerimetre.Count == 1)
                    {
                        lstCentreSelect.Add(LstCentrePerimetre.First().PK_ID);
                        this.Txt_CodeCentre.Text = LstCentrePerimetre.First().CODE;
                        this.Txt_LibelleCentre.Text = LstCentrePerimetre.First().LIBELLE;
                        this.Txt_CodeCentre.Tag = lstCentreSelect;
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
                    foreach (Galatee.Silverlight.ServiceAccueil.CsCentre item in LstCentrePerimetre)
                        lstCentre.Add(item.PK_ID);
                    if (lstSite.Count == 1)
                    {
                        this.Txt_CodeSite.Text = lstSite.First().CODE;
                        this.Txt_LibelleSite.Text = lstSite.First().LIBELLE;
                        this.Txt_CodeSite.Tag = lstSite.First().PK_ID;
                        this.btn_Site.IsEnabled = false;
                    }
                    if (LstCentrePerimetre.Count == 1)
                    {
                        lstCentreSelect.Add(LstCentrePerimetre.First().PK_ID);
                        this.Txt_CodeCentre.Text = LstCentrePerimetre.First().CODE;
                        this.Txt_LibelleCentre.Text = LstCentrePerimetre.First().LIBELLE;
                        this.Txt_CodeCentre.Tag = lstCentreSelect;
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
                Message.ShowError(ex.Message, "Report");
            }
        }

        void galatee_OkClickedSite(object sender, EventArgs e)
        {
            Galatee.Silverlight.MainView.UcListeGenerique ctrs = sender as Galatee.Silverlight.MainView.UcListeGenerique;
            if (ctrs.isOkClick)
            {
                this.btn_Site.IsEnabled = true;
                Galatee.Silverlight.ServiceAccueil.CsSite leSite = (Galatee.Silverlight.ServiceAccueil.CsSite)ctrs.MyObject;
                this.Txt_CodeSite.Text = leSite.CODE;
                this.Txt_CodeSite.Tag = leSite.PK_ID;
                this.Txt_LibelleSite.Text = leSite.LIBELLE;
                List<ServiceAccueil.CsCentre> lsiteCentre = LstCentrePerimetre.Where(t => t.FK_IDCODESITE == (int)this.Txt_CodeSite.Tag).ToList();
                if (lsiteCentre.Count == 1)
                {
                    List<int> lstCentreSelect = new List<int>();
                    lstCentreSelect.Add(lsiteCentre.First().PK_ID);
                    this.Txt_CodeCentre.Text = lsiteCentre.First().CODE;
                    this.Txt_LibelleCentre.Text = lsiteCentre.First().LIBELLE;
                    this.Txt_CodeCentre.Tag = lstCentreSelect;
                }
            }
            this.btn_Site.IsEnabled = true;
        }
        private void Txt_CodeSite_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(Txt_CodeSite.Text) && Txt_CodeSite.Text.Length == SessionObject.Enumere.TailleCentre)
                {
                    Galatee.Silverlight.ServiceAccueil.CsSite _LeSiteClient =Shared.ClasseMEthodeGenerique.RetourneObjectFromList(lstSite, this.Txt_CodeSite.Text, "CODE");
                    if (!string.IsNullOrEmpty(_LeSiteClient.LIBELLE))
                    {
                        this.Txt_LibelleSite.Text = _LeSiteClient.LIBELLE;
                        this.Txt_CodeSite.Text = _LeSiteClient.CODE;
                        this.Txt_CodeSite.Tag = _LeSiteClient.PK_ID;
                        List<int> lstCentreSelect = new List<int>();

                        List<ServiceAccueil.CsCentre> lsiteCentre = LstCentrePerimetre.Where(t => t.FK_IDCODESITE == (int)this.Txt_CodeSite.Tag).ToList();
                        if (lsiteCentre.Count == 1)
                        {
                            lstCentreSelect.Add(lsiteCentre.First().PK_ID);
                            this.Txt_CodeCentre.Text = lsiteCentre.First().CODE;
                            this.Txt_LibelleCentre.Text = lsiteCentre.First().LIBELLE;
                            this.Txt_CodeCentre.Tag = lstCentreSelect;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, "Facturation");

            }
        }

        private void btn_Centre_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (LstCentrePerimetre.Count != 0)
                {
                    List<ServiceAccueil.CParametre> lstParametre = Shared.ClasseMEthodeGenerique.RetourneValueFromClasse<ServiceAccueil.CsCentre>(LstCentrePerimetre);
                    Shared.UcListeParametre ctr = new Shared.UcListeParametre(lstParametre, true, "Centre");
                    ctr.Closed += new EventHandler(galatee_OkClickedCentre);
                    ctr.Show();
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
            }
        }
        private void galatee_OkClickedCentre(object sender, EventArgs e)
        {
            try
            {

                Shared.UcListeParametre ctrs = sender as Shared.UcListeParametre;
                if (ctrs.isOkClick)
                {
                    if (lstCentre.Count != 0) lstCentre.Clear();
                    List<ServiceAccueil.CParametre> _LesCentreSelect = ctrs.MyObjectList as List<ServiceAccueil.CParametre>;
                    string Cent = string.Empty ;
                    foreach (ServiceAccueil.CParametre item in _LesCentreSelect)
                    {
                        lstCentre.AddRange(_LesCentreSelect.Where(o=>o.CODE ==item.CODE && o.PK_ID == item.PK_ID ).Select(m=>m.PK_ID).ToList());
                        Cent = Cent + " " + item.CODE;
                    }
                    this.Txt_LibelleCentre.Text = Cent ;
                    this.Txt_CodeCentre.Tag = lstCentre;
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
            }
        }
        private void Txt_CodeCentre_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(Txt_CodeCentre.Text) && Txt_CodeCentre.Text.Length == SessionObject.Enumere.TailleCentre)
                {
                    ServiceAccueil.CsCentre _LeCentreClient =Shared.ClasseMEthodeGenerique.RetourneObjectFromList(LstCentrePerimetre.Where(t => t.FK_IDCODESITE == (int)this.Txt_CodeSite.Tag).ToList(), this.Txt_CodeCentre.Text, "CODE");
                    if (!string.IsNullOrEmpty(_LeCentreClient.LIBELLE))
                    {
                        List<int> lstCentreSelect = new List<int>();
                        lstCentreSelect.Add(_LeCentreClient.PK_ID);
                        this.Txt_CodeCentre.Text = _LeCentreClient.CODE;
                        this.Txt_LibelleCentre.Text = _LeCentreClient.LIBELLE;
                        this.Txt_CodeCentre.Tag = lstCentreSelect;
                    }
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, "Facturation");

            }
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
                if (lesDeCentre != null && !string.IsNullOrEmpty(this.txt_Periode.Text))
                    ConsoNull();

            }
        }
        Dictionary<string, List<int>> lesDeCentre = new Dictionary<string, List<int>>();
        private void ConsoNull()
        {
            if (lesDeCentre.Count != 0) lesDeCentre.Clear();
            if (this.Txt_CodeSite.Tag == null)
            {
                Message.ShowInformation("Selectionner le site ", "Message");
                return;
            }
            if (this.Txt_CodeCentre.Tag == null)
                lesDeCentre.Add(this.Txt_CodeSite.Text, LstCentrePerimetre.Where(i => i.CODESITE == this.Txt_CodeSite.Text).Select(p => p.PK_ID).ToList());

            else
                lesDeCentre.Add(this.Txt_CodeSite.Text, (List<int>)this.Txt_CodeCentre.Tag);

            if (!string.IsNullOrEmpty(this.txt_Periode.Text))
            {
                if (Shared.ClasseMEthodeGenerique.IsFormatPeriodeValide(this.txt_Periode.Text))
                {
                    string periode = string.IsNullOrEmpty(this.txt_Periode.Text) ? string.Empty : Shared.ClasseMEthodeGenerique.FormatPeriodeAAAAMM( this.txt_Periode.Text);
                    RetourneConsoNull(lesDeCentre, periode);
                }
                else
                    Message.ShowInformation("Format de la période incorrect", "Reporting");
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

  
 


        private void RetourneConsoNull(Dictionary<string, List<int>> lstSiteCentre, string Periode)
        {
            Galatee.Silverlight.ServiceReport.ReportServiceClient service1 = new Galatee.Silverlight.ServiceReport.ReportServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Report"));
            service1.ReturneConsNullCompleted += (sr, res) =>
            {
                if (res != null && res.Cancelled)
                    return;

                if (res.Result != null && res.Result.Count != 0)
                {
                    string Rdlc = string.Empty;
                    Rdlc = "ConsoNull";
                    List<ServiceReport.CsEvenement> LstSourceDonnee = res.Result;
                    if (this.Chk_Recap.IsChecked == true )
                    {
                        Rdlc = "ConsoNullRecap";
                        var lstConsoNullDistinct = LstSourceDonnee.Select(k => new { k.CENTRE,k.PRODUIT  }).Distinct();
                        List<ServiceReport.CsEvenement> lst = new List<ServiceReport.CsEvenement>();
                        foreach (var item in lstConsoNullDistinct)
                        {
                            ServiceReport.CsEvenement l = new ServiceReport.CsEvenement();
                            l.CENTRE = item.CENTRE;
                            l.PRODUIT = item.PRODUIT;
                            l.NOMBRECLIENTLOT = LstSourceDonnee.Where(i => i.CENTRE == item.CENTRE && i.PRODUIT == item.PRODUIT ).ToList().Count();
                            lst.Add(l);
                        }
                        LstSourceDonnee = lst;
                    }
                    if (OptionImpression == SessionObject.EnvoiPrinter)
                        Utility.ActionDirectOrientation<ServicePrintings.CsEvenement, ServiceReport.CsEvenement>(LstSourceDonnee, param, SessionObject.CheminImpression, Rdlc, "Report", true);
                    else if (OptionImpression == SessionObject.EnvoiExecl)
                        Utility.ActionExportation<ServicePrintings.CsEvenement, ServiceReport.CsEvenement>(LstSourceDonnee, param, string.Empty, SessionObject.CheminImpression, Rdlc, "Report", true, "xlsx");

                    else if (OptionImpression == SessionObject.EnvoiWord)
                        Utility.ActionExportation<ServicePrintings.CsEvenement, ServiceReport.CsEvenement>(LstSourceDonnee, param, string.Empty, SessionObject.CheminImpression, Rdlc, "Report", true, "doc");

                    else if (OptionImpression == SessionObject.EnvoiPdf)
                        Utility.ActionExportation<ServicePrintings.CsEvenement, ServiceReport.CsEvenement>(LstSourceDonnee, param, string.Empty, SessionObject.CheminImpression, Rdlc, "Report", true, "pdf");
                }
                else
                {
                    Message.ShowInformation("Aucune information trouvée", "Report");
                    return;
                }
            };
            service1.ReturneConsNullAsync(lstSiteCentre, Periode);
            service1.CloseAsync();


        }

    }
}

