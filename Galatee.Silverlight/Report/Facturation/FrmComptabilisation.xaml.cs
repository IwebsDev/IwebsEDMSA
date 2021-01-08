using Galatee.Silverlight.ServiceReport;
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
    public partial class FrmComptabilisation : ChildWindow
    {
        public FrmComptabilisation()
        {
            InitializeComponent();
            ChargerDonneeDuSite();
            prgBar.Visibility = System.Windows.Visibility.Collapsed;
        }
        string leEtatExecuter = string.Empty;
        public FrmComptabilisation(string typeEtat)
        {
            InitializeComponent();
            ChargerDonneeDuSite();
            leEtatExecuter = typeEtat;
            prgBar.Visibility = System.Windows.Visibility.Collapsed;
            Chk_Categorie.Visibility = (typeEtat == SessionObject.StatVenteCummuler ||
                typeEtat == SessionObject.Statfacturation || typeEtat == SessionObject.CompteurFacturePeriode) ? Visibility.Visible : Visibility.Collapsed;
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
                            this.Txt_LibelleSite.Text = lSiteSelect.LIBELLE;
                            this.Txt_LibelleSite.Tag = lSiteSelect.PK_ID;
                            lProduit = LstCentrePerimetre.FirstOrDefault(t => t.FK_IDCODESITE == (int)this.Txt_LibelleSite.Tag).LESPRODUITSDUSITE.First();
                        }
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
                Message.ShowError(ex.Message, "Report");
            }

        }
        ServiceAccueil.CsProduit lProduit = new ServiceAccueil.CsProduit();
        void galatee_OkClickedSite(object sender, EventArgs e)
        {
            Galatee.Silverlight.MainView.UcListeGenerique ctrs = sender as Galatee.Silverlight.MainView.UcListeGenerique;
            if (ctrs.isOkClick)
            {
                Galatee.Silverlight.ServiceAccueil.CsSite leSite = (Galatee.Silverlight.ServiceAccueil.CsSite)ctrs.MyObject;
                this.Txt_LibelleSite.Text = leSite.LIBELLE;
                this.Txt_LibelleSite.Tag = leSite.PK_ID;
                lSiteSelect = leSite;
                lProduit = LstCentrePerimetre.FirstOrDefault(t => t.FK_IDCODESITE == (int)this.Txt_LibelleSite.Tag).LESPRODUITSDUSITE.First();
            }
            this.btn_Site.IsEnabled = true;
        }
      
        public void retourneFacture(bool IsGroup)
            {
                try
                {
                    prgBar.Visibility = System.Windows.Visibility.Visible;

                string Compte = string.Empty;
                Galatee.Silverlight.ServiceReport.ReportServiceClient service = new Galatee.Silverlight.ServiceReport.ReportServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Report"));
                service.ReturneCompabilisationRecapAsync(LstCentrePerimetre.Where(t => t.FK_IDCODESITE == lSiteSelect.PK_ID).Select(o => o.PK_ID).ToList(), Shared.ClasseMEthodeGenerique.FormatPeriodeAAAAMM(this.txt_Periode.Text), IsGroup);
                service.ReturneCompabilisationRecapCompleted += (s, args) =>
                    {
                        try
                        {
                            prgBar.Visibility = System.Windows.Visibility.Collapsed;

                            if (args.Cancelled || args.Error != null)
                            {
                                string error = args.Error.InnerException.ToString();
                                return;
                            }
                            if (args.Result != null && args.Result.Count == 0)
                            {
                                Message.ShowInformation("Aucune données trouvées", "Comptabilisation");
                                return;
                            }
                            else
                            {
                                string Rdlc = string.Empty;
                                if (leEtatExecuter == SessionObject.ComptaFacturation) Rdlc ="ComptabilisationFac";
                                else if (leEtatExecuter == SessionObject.RecapComptaFacturation)  Rdlc ="RecapFacturationGroup";
                                 
                                if (OptionImpression == SessionObject.EnvoiPrinter)
                                    Utility.ActionDirectOrientation<ServicePrintings.CsComptabilisation, ServiceReport.CsComptabilisation>(args.Result, param, SessionObject.CheminImpression, Rdlc, "Report", true);
                                else if (OptionImpression == SessionObject.EnvoiExecl)
                                    Utility.ActionExportation<ServicePrintings.CsComptabilisation, ServiceReport.CsComptabilisation>(args.Result, param, string.Empty, SessionObject.CheminImpression,Rdlc, "Report", true, "xlsx");

                                else if (OptionImpression == SessionObject.EnvoiWord)
                                    Utility.ActionExportation<ServicePrintings.CsComptabilisation, ServiceReport.CsComptabilisation>(args.Result, param, string.Empty, SessionObject.CheminImpression, Rdlc, "Report", true, "doc");

                                else if (OptionImpression == SessionObject.EnvoiPdf)
                                    Utility.ActionExportation<ServicePrintings.CsComptabilisation, ServiceReport.CsComptabilisation>(args.Result, param, string.Empty, SessionObject.CheminImpression, Rdlc, "Report", true, "pdf");

                            }
                        }
                        catch (Exception ex)
                        {
                            Message.ShowError(ex, Galatee.Silverlight.Resources.Caisse.Langue.errorTitle);
                        }
                    };


                }
                catch (Exception)
                {

                    throw;
                }

            }
        private void RetourneEncaissemntMoisComptat(List<int> lesDeCentre, string Periode)
        {
            prgBar.Visibility = System.Windows.Visibility.Visible;

            Galatee.Silverlight.ServiceReport.ReportServiceClient service1 = new Galatee.Silverlight.ServiceReport.ReportServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Report"));
            service1.ReturneEncaissementParMoisComptableCompleted += (sr, res) =>
            {
                prgBar.Visibility = System.Windows.Visibility.Collapsed;

                if (res != null && res.Cancelled)
                    return;

                if (res.Result != null && res.Result.Count != 0)
                {
                    string Rdlc = "FacEncaissementParMoisComptable";
                    if (OptionImpression == SessionObject.EnvoiPrinter)
                        Utility.ActionDirectOrientation<ServicePrintings.CsTranscaisse, ServiceReport.CsTranscaisse>(res.Result, null, SessionObject.CheminImpression, Rdlc, "Report", true);
                    else if (OptionImpression == SessionObject.EnvoiExecl)
                        Utility.ActionExportation<ServicePrintings.CsTranscaisse, ServiceReport.CsTranscaisse>(res.Result, null, string.Empty, SessionObject.CheminImpression, Rdlc, "Report", true, "xlsx");

                    else if (OptionImpression == SessionObject.EnvoiWord)
                        Utility.ActionExportation<ServicePrintings.CsTranscaisse, ServiceReport.CsTranscaisse>(res.Result, null, string.Empty, SessionObject.CheminImpression, Rdlc, "Report", true, "doc");

                    else if (OptionImpression == SessionObject.EnvoiPdf)
                        Utility.ActionExportation<ServicePrintings.CsTranscaisse, ServiceReport.CsTranscaisse>(res.Result, null, string.Empty, SessionObject.CheminImpression, Rdlc, "Report", true, "pdf");
                }
                else
                {
                    Message.ShowInformation("Aucune information trouvée", "Report");
                    return;
                }
            };
            service1.ReturneEncaissementParMoisComptableAsync(lesDeCentre, Periode);
            service1.CloseAsync();


        }
        private void RetourneCompteurPeriode(Dictionary<string, List<int>> lesDeCentre, string Periode,bool IsStat)
        {
            prgBar.Visibility = System.Windows.Visibility.Visible;

            Galatee.Silverlight.ServiceReport.ReportServiceClient service1 = new Galatee.Silverlight.ServiceReport.ReportServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Report"));
            service1.ReturneCompteurParProduitPeriodeCompleted += (sr, res) =>
            {
                prgBar.Visibility = System.Windows.Visibility.Collapsed ;

                if (res != null && res.Cancelled)
                    return;
                 
                if (res.Result != null && res.Result.Count != 0)
                {
                    string Rdlc = "CompteurParProduitPeriode";
                    if (IsStat) Rdlc = "CompteurParProduitPeriodeStat";

                    if (OptionImpression == SessionObject.EnvoiPrinter)
                        Utility.ActionDirectOrientation<ServicePrintings.CsEvenement, ServiceReport.CsEvenement>(res.Result, null , SessionObject.CheminImpression, Rdlc, "Report", true);
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
            service1.ReturneCompteurParProduitPeriodeAsync(lesDeCentre, Periode, IsStat);
            service1.CloseAsync();


        }
        Dictionary<string, string> param = new Dictionary<string,string>();
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
                RechercherDonnee();

            }
        }

        private void RechercherDonnee()
        {
            string key = Utility.getKey();
            if (leEtatExecuter == SessionObject.RecapComptaFacturation)  retourneFacture(true );
            else if ( leEtatExecuter == SessionObject.ComptaFacturation)
                retourneFacture(false);
            else if (leEtatExecuter == SessionObject.Statfacturation )
            {
                bool EstBT = false ;
                bool IsStat = this.Chk_Categorie.IsChecked == true ? true : false ;
                if (lProduit.CODE == SessionObject.Enumere.Electricite) EstBT = true;
                Galatee.Silverlight.ServiceReport.ReportServiceClient service = new Galatee.Silverlight.ServiceReport.ReportServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Report"));
                service.ReturneStatistiqueCompleted += (s, args) =>
                {
                    prgBar.Visibility = System.Windows.Visibility.Collapsed;

                    if (args != null && args.Cancelled)
                        return;

                    if (param != null && param.Count() != 0) param.Clear();
                    param.Add("pPeriode", this.txt_Periode.Text);
                    param.Add("pAgence", this.Txt_LibelleSite.Text );
                    string Rdlc = string.Empty;

                    List<ServiceReport.CsStatFact > lstStatfact = args.Result;
                    if (!EstBT)
                    {
                        if (leEtatExecuter == SessionObject.Statfacturation)
                            Rdlc = this.Chk_Categorie.IsChecked == false ? "RecapStatFacturationMt" : "RecapStatFacturationMtStat";
                    }
                    else
                    {
                        if (leEtatExecuter == SessionObject.Statfacturation)
                            Rdlc = this.Chk_Categorie.IsChecked == false ? "RecapStatFacturationBtFac" : "RecapStatFacturationBtStat";
                    }

                    if (OptionImpression == SessionObject.EnvoiPrinter)
                        Utility.ActionDirectOrientation<ServicePrintings.CsStatFact, ServiceReport.CsStatFact>(args.Result, param, SessionObject.CheminImpression, Rdlc, "Report", true);
                    else if (OptionImpression == SessionObject.EnvoiExecl)
                        Utility.ActionExportation<ServicePrintings.CsStatFact, ServiceReport.CsStatFact>(args.Result, param, string.Empty, SessionObject.CheminImpression, Rdlc, "Report", true, "xlsx");

                    else if (OptionImpression == SessionObject.EnvoiWord)
                        Utility.ActionExportation<ServicePrintings.CsStatFact, ServiceReport.CsStatFact>(args.Result, param, string.Empty, SessionObject.CheminImpression, Rdlc, "Report", true, "doc");

                    else if (OptionImpression == SessionObject.EnvoiPdf)
                        Utility.ActionExportation<ServicePrintings.CsStatFact, ServiceReport.CsStatFact>(args.Result, param, string.Empty, SessionObject.CheminImpression, Rdlc, "Report", true, "pdf");
                };
                service.ReturneStatistiqueAsync(lSiteSelect.CODE, Shared.ClasseMEthodeGenerique.FormatPeriodeAAAAMM(this.txt_Periode.Text), lProduit.CODE, IsStat);
                service.CloseAsync();
            }
            else if (leEtatExecuter == SessionObject.StatVenteCummuler)
            {
                bool IsStat = this.Chk_Categorie.IsChecked == true ? true : false;
                prgBar.Visibility = System.Windows.Visibility.Collapsed ;

                List<int> lstCentre = LstCentrePerimetre.Where(p => p.FK_IDCODESITE == (int)this.Txt_LibelleSite.Tag).Select(l => l.PK_ID).ToList();
                if (param != null && param.Count() != 0) param.Clear();
                //param.Add("pAgence", this.Txt_LibelleSite.Text );
                Galatee.Silverlight.ServiceReport.ReportServiceClient service = new Galatee.Silverlight.ServiceReport.ReportServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Report"));
                service.ReturneVenteCummuleCompleted += (s, args) =>
                {
                    prgBar.Visibility = System.Windows.Visibility.Collapsed;

                    if (args != null && args.Cancelled)
                        return;
                    string Rdlc = string.Empty;

                    Rdlc = this.Chk_Categorie.IsChecked == true ? "VenteCummul" : "VenteCummulCategorie";
                    if (OptionImpression == SessionObject.EnvoiPrinter)
                        Utility.ActionDirectOrientation<ServicePrintings.CsRedevanceFacture, ServiceReport.CsRedevanceFacture>(args.Result, param, SessionObject.CheminImpression, Rdlc, "Report", true);
                    else if (OptionImpression == SessionObject.EnvoiExecl)
                        Utility.ActionExportation<ServicePrintings.CsRedevanceFacture, ServiceReport.CsRedevanceFacture>(args.Result, param, string.Empty, SessionObject.CheminImpression, Rdlc + "Xls", "Report", true, "xlsx");

                    else if (OptionImpression == SessionObject.EnvoiWord)
                        Utility.ActionExportation<ServicePrintings.CsRedevanceFacture, ServiceReport.CsRedevanceFacture>(args.Result, param, string.Empty, SessionObject.CheminImpression, Rdlc, "Report", true, "doc");

                    else if (OptionImpression == SessionObject.EnvoiPdf)
                        Utility.ActionExportation<ServicePrintings.CsRedevanceFacture, ServiceReport.CsRedevanceFacture>(args.Result, param, string.Empty, SessionObject.CheminImpression, Rdlc, "Report", true, "pdf");
                };
                service.ReturneVenteCummuleAsync(lstCentre, Shared.ClasseMEthodeGenerique.FormatPeriodeAAAAMM(this.txt_Periode.Text), IsStat);
                service.CloseAsync();
            }
            else if (leEtatExecuter == SessionObject.CompteurFacturePeriode)
            {
                bool IsStat = this.Chk_Categorie.IsChecked == true ? true : false;
                Dictionary<string, List<int>> lesDeCentre = new Dictionary<string, List<int>>();
                if (this.Txt_LibelleSite.Tag == null)
                {
                    Message.ShowInformation("Selectionner le site ", "Message");
                    return;
                }
                lesDeCentre.Add(lSiteSelect.CODE, LstCentrePerimetre.Where(i => i.CODESITE == lSiteSelect.CODE).Select(p => p.PK_ID).ToList());
                RetourneCompteurPeriode(lesDeCentre, Shared.ClasseMEthodeGenerique.FormatPeriodeAAAAMM(this.txt_Periode.Text), IsStat);
            }
            else if (leEtatExecuter == SessionObject.EncaissementCumule )
            RetourneEncaissemntMoisComptat(LstCentrePerimetre.Where(i => i.CODESITE == lSiteSelect.CODE).Select(p => p.PK_ID).ToList(), Shared.ClasseMEthodeGenerique.FormatPeriodeAAAAMM(this.txt_Periode.Text));
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}

