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

namespace Galatee.Silverlight.Facturation
{
    public partial class FrmControleFacturation : ChildWindow
    {
        public FrmControleFacturation()
        {
            InitializeComponent();
            ChargerDonneeDuSite();
            RetourneCompteSpecifique();
            RetourneTypeCompte();
            RetourneCentreCompte();
            RetourneOperationCompte();
            prgBar.Visibility = System.Windows.Visibility.Collapsed;
        }

        public List<Galatee.Silverlight.ServiceInterfaceComptable.CsCompteSpecifique> ListeCompteSpecifique { get; set; }
        public List<Galatee.Silverlight.ServiceInterfaceComptable.CsEcritureComptable> ListeLigneComptable  ;

        public List<ServiceInterfaceComptable.CsCentreCompte> ListeCentreParametrage { get; set; }
        public List<CsRedevance> ListeRedevance = new List<CsRedevance>();
        public List<ServiceInterfaceComptable.CsCoper> ListeOperation { get; set; }
        public List<ServiceInterfaceComptable.CsOperationComptable> ListeOperationComptable = new List<ServiceInterfaceComptable.CsOperationComptable>();
        public List<ServiceInterfaceComptable.CsTypeCompte> ListeTypeCompte = new List<Galatee.Silverlight.ServiceInterfaceComptable.CsTypeCompte>();

   
        string leEtatExecuter = string.Empty;
        public FrmControleFacturation(string typeEtat)
        {
            InitializeComponent();
            ChargerDonneeDuSite();
            RetourneCompteSpecifique();
            RetourneTypeCompte();
            RetourneCentreCompte();
            RetourneOperationCompte();
            ChargerCategorie();
            RetourneRedevance();
            leEtatExecuter = typeEtat;
            prgBar.Visibility = System.Windows.Visibility.Collapsed;

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
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
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
        public static object GetPropValue(object src, string propName)
        {
            return src.GetType().GetProperty(propName).GetValue(src, null);
        }
        public void retourneFacture(bool IsGroup)
            {
                try
                {
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
                                if (leEtatExecuter == SessionObject.ComptaFacturation)
                                Utility.ActionDirectOrientation<ServicePrintings.CsComptabilisation, ServiceReport.CsComptabilisation>(args.Result, null, SessionObject.CheminImpression, "ComptabilisationFac", "Report", false);
                                else if (leEtatExecuter == SessionObject.RecapComptaFacturation)
                                    Utility.ActionDirectOrientation<ServicePrintings.CsComptabilisation, ServiceReport.CsComptabilisation>(args.Result, null, SessionObject.CheminImpression, "RecapFacturationGroup", "Report", false);

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

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            string key = Utility.getKey();
            Dictionary<string, string> param = new Dictionary<string, string>();
            prgBar.Visibility = System.Windows.Visibility.Visible ;

            if (leEtatExecuter == SessionObject.RecapComptaFacturation)  retourneFacture(true );
            else if ( leEtatExecuter == SessionObject.ComptaFacturation)
                retourneFacture(false);
            else if (leEtatExecuter == SessionObject.Statfacturation ||
                leEtatExecuter == SessionObject.StatfacturationStat)
            {
                bool EstBT = false ;
                if (lProduit.CODE == SessionObject.Enumere.Electricite) EstBT = true;
                Galatee.Silverlight.ServiceReport.ReportServiceClient service = new Galatee.Silverlight.ServiceReport.ReportServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Report"));
                service.ReturneStatistiqueCompleted += (s, args) =>
                {
                    prgBar.Visibility = System.Windows.Visibility.Collapsed;

                    if (args != null && args.Cancelled)
                        return;
                   
                    List<ServiceReport.CsStatFact > lstStatfact = args.Result;
                    if (!EstBT)
                    {
                        param.Add("pPeriode", this.txt_Periode.Text);
                        if (chk_Exporter.IsChecked == true)
                            Utility.ActionExportation<ServicePrintings.CsStatFact, ServiceReport.CsStatFact>(args.Result, param, SessionObject.CheminImpression, "RecapStatFacturationMtStat", "Report", true, "xlsx");
                        else
                            Utility.ActionDirectOrientation<ServicePrintings.CsStatFact, ServiceReport.CsStatFact>(lstStatfact, param, SessionObject.CheminImpression, "RecapStatFacturationMtStat", "Report", false);
                    }
                    else
                    {
                        if (chk_Exporter.IsChecked == true)
                            Utility.ActionExportation<ServicePrintings.CsStatFact, ServiceReport.CsStatFact>(args.Result, param, SessionObject.CheminImpression, "RecapStatFacturationBt", "Report", true, "xlsx");
                        else
                            Utility.ActionDirectOrientation<ServicePrintings.CsStatFact, ServiceReport.CsStatFact>(lstStatfact, param, SessionObject.CheminImpression, "RecapStatFacturationBt", "Report", false);
                    }
                };
                service.ReturneStatistiqueAsync(lSiteSelect.CODE , Shared.ClasseMEthodeGenerique.FormatPeriodeAAAAMM(this.txt_Periode.Text), lProduit.CODE);
                service.CloseAsync();
            }
            //else if (leEtatExecuter == SessionObject.StatfacturationStat)
            //{
            //    bool EstBT = false;
            //    if (lProduit.CODE == SessionObject.Enumere.Electricite) EstBT = true;
            //    Galatee.Silverlight.ServiceReport.ReportServiceClient service = new Galatee.Silverlight.ServiceReport.ReportServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Report"));
            //    service.ReturneStatiqueDesVenteStatCompleted += (s, args) =>
            //    {
            //        prgBar.Visibility = System.Windows.Visibility.Collapsed;

            //        if (args != null && args.Cancelled)
            //            return;

            //        param.Add("pPeriode", this.txt_Periode.Text);
            //        param.Add("pAgence", this.Txt_LibelleSite.Text);

            //        List<ServiceReport.CsStatFact> lstStatfact = args.Result;
            //        if (!EstBT)
            //        {
            //            if (chk_Exporter.IsChecked == true)
            //                Utility.ActionExportation<ServicePrintings.CsStatFact, ServiceReport.CsStatFact>(args.Result, param, SessionObject.CheminImpression, "RecapStatFacturationMtStat", "Report", true, "xlsx");
            //            else
            //                Utility.ActionDirectOrientation<ServicePrintings.CsStatFact, ServiceReport.CsStatFact>(lstStatfact, param, SessionObject.CheminImpression, "RecapStatFacturationMtStat", "Report", false);
            //        }
            //        else
            //        {
            //            if (chk_Exporter.IsChecked == true)
            //                Utility.ActionExportation<ServicePrintings.CsStatFact, ServiceReport.CsStatFact>(args.Result, param, SessionObject.CheminImpression, "RecapStatFacturationBtStat", "Report", true, "xlsx");
            //            else
            //                Utility.ActionDirectOrientation<ServicePrintings.CsStatFact, ServiceReport.CsStatFact>(lstStatfact, param, SessionObject.CheminImpression, "RecapStatFacturationBtStat", "Report", false);
            //        }
            //    };
            //    service.ReturneStatiqueDesVenteStatAsync(LstCentrePerimetre.Where(t => t.FK_IDCODESITE == lSiteSelect.PK_ID).Select(o => o.PK_ID).ToList(), Shared.ClasseMEthodeGenerique.FormatPeriodeAAAAMM(this.txt_Periode.Text), lProduit.CODE);
            //    service.CloseAsync();
            //}
            else if (leEtatExecuter == SessionObject.StatVenteCummuler)
            {
                bool EstBT = false;
                if (lProduit.CODE == SessionObject.Enumere.Electricite) EstBT = true;
                Galatee.Silverlight.ServiceReport.ReportServiceClient service = new Galatee.Silverlight.ServiceReport.ReportServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Report"));
                service.ReturneVenteCummuleCompleted += (s, args) =>
                {
                    prgBar.Visibility = System.Windows.Visibility.Collapsed;

                    if (args != null && args.Cancelled)
                        return;

                    List<ServiceReport.CsRedevanceFacture > lstStatfact = args.Result;
                    if (!EstBT)
                    {
                        if (chk_Exporter.IsChecked == true)
                            Utility.ActionExportation<ServicePrintings.CsRedevanceFacture, ServiceReport.CsRedevanceFacture>(args.Result, param, SessionObject.CheminImpression, "RecapStatFacturationMtStat", "Report", true, "xlsx");
                        else
                            Utility.ActionDirectOrientation<ServicePrintings.CsRedevanceFacture, ServiceReport.CsRedevanceFacture>(args.Result, param, SessionObject.CheminImpression, "RecapStatFacturationMtStat", "Report", false);
                    }
                    else
                    {
                        if (chk_Exporter.IsChecked == true)
                            Utility.ActionExportation<ServicePrintings.CsRedevanceFacture, ServiceReport.CsRedevanceFacture>(args.Result, param, SessionObject.CheminImpression, "VenteCummul", "Report", true, "xlsx");
                        else
                            Utility.ActionDirectOrientation<ServicePrintings.CsRedevanceFacture, ServiceReport.CsRedevanceFacture>(args.Result, param, SessionObject.CheminImpression, "VenteCummul", "Report", false);
                    }
                };
                service.ReturneVenteCummuleAsync( lSiteSelect.CODE, Shared.ClasseMEthodeGenerique.FormatPeriodeAAAAMM(this.txt_Periode.Text), lProduit.CODE);
                service.CloseAsync();
            }

        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
        public void RetourneCompteSpecifique()
            {
                try
                {
                    Galatee.Silverlight.ServiceInterfaceComptable.InterfaceComptableServiceClient service = new Galatee.Silverlight.ServiceInterfaceComptable.InterfaceComptableServiceClient(Utility.ProtocoleIndex(), Utility.EndPoint("InterfaceComptable"));
                    service.RetourneCompteSpecifiqueCompleted += (s, args) =>
                    {
                        try
                        {
                            if (args.Cancelled || args.Error != null || args.Result == null)
                            {
                                string error = args.Error.InnerException.ToString();
                                return;
                            }
                            else
                            {
                                if (args.Result != null && args.Result.Count != 0)
                                {
                                    ListeCompteSpecifique = new List<Galatee.Silverlight.ServiceInterfaceComptable.CsCompteSpecifique>();
                                    ListeCompteSpecifique = args.Result;

                                }
                                else
                                {
                                    Message.ShowInformation("Le système n'a trouvé aucun Compte spécifique dans la base de données ", "Avertissement");
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Message.ShowError(ex, Galatee.Silverlight.Resources.Caisse.Langue.errorTitle);
                        }
                    };
                    service.RetourneCompteSpecifiqueAsync();

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        public void RetourneRedevance()
        {
            try
            {
                Galatee.Silverlight.ServiceReport.ReportServiceClient service = new Galatee.Silverlight.ServiceReport.ReportServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Report"));
                service.ChargerRedevanceAsync();
                service.ChargerRedevanceCompleted += (s, args) =>
                {
                    try
                    {
                        if (args.Cancelled || args.Error != null || args.Result == null)
                        {
                            string error = args.Error.InnerException.ToString();
                            return;
                        }
                        else
                        {
                            if (args.Result != null && args.Result.Count != 0)
                                ListeRedevance = args.Result;
                            else
                                Message.ShowInformation("Le système n'a trouvé aucun Compte spécifique dans la base de données ", "Avertissement");
                        }
                    }
                    catch (Exception ex)
                    {
                        Message.ShowError(ex, Galatee.Silverlight.Resources.Caisse.Langue.errorTitle);
                    }
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void ChargerCategorie()
        {
            try
            {
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                service.RetourneCategorieCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    SessionObject.LstCategorie = args.Result;
                };
                service.RetourneCategorieAsync();
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void RetourneTypeCompte()
        {
            try
            {
                Galatee.Silverlight.ServiceInterfaceComptable.InterfaceComptableServiceClient service = new Galatee.Silverlight.ServiceInterfaceComptable.InterfaceComptableServiceClient(Utility.ProtocoleIndex(), Utility.EndPoint("InterfaceComptable"));
                service.RetourneTypeCompteCompleted += (s, args) =>
                {
                    try
                    {
                        if (args.Cancelled || args.Error != null || args.Result == null)
                        {
                            string error = args.Error.InnerException.ToString();
                            return;
                        }
                        else
                        {
                            if (args.Result != null && args.Result.Count != 0)
                            {
                                ListeTypeCompte = new List<Galatee.Silverlight.ServiceInterfaceComptable.CsTypeCompte>();
                                ListeTypeCompte = args.Result;
                            }
                            else
                            {
                                Message.ShowInformation("Le système n'a trouvé aucun Compte spécifique dans la base de données ", "Avertissement");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Message.ShowError(ex, Galatee.Silverlight.Resources.Caisse.Langue.errorTitle);
                    }
                };
                service.RetourneTypeCompteAsync();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void RetourneCentreCompte()
        {
            try
            {
                Galatee.Silverlight.ServiceInterfaceComptable.InterfaceComptableServiceClient service = new Galatee.Silverlight.ServiceInterfaceComptable.InterfaceComptableServiceClient(Utility.ProtocoleIndex(), Utility.EndPoint("InterfaceComptable"));
                service.RetourneParamCentreAsync();
                service.RetourneParamCentreCompleted += (s, args) =>
                {
                    try
                    {
                        if (args.Cancelled || args.Error != null || args.Result == null)
                        {
                            string error = args.Error.InnerException.ToString();
                            return;
                        }
                        else
                        {
                            if (args.Result != null && args.Result.Count != 0)
                            {
                                ListeCentreParametrage = new List<Galatee.Silverlight.ServiceInterfaceComptable.CsCentreCompte>();
                                ListeCentreParametrage = args.Result;
                            }
                            else
                            {
                                Message.ShowInformation("Le système n'a trouvé aucun Compte spécifique dans la base de données ", "Avertissement");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Message.ShowError(ex, Galatee.Silverlight.Resources.Caisse.Langue.errorTitle);
                    }
                };

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void RetourneOperationCompte()
        {
            try
            {
                Galatee.Silverlight.ServiceInterfaceComptable.InterfaceComptableServiceClient service = new Galatee.Silverlight.ServiceInterfaceComptable.InterfaceComptableServiceClient(Utility.ProtocoleIndex(), Utility.EndPoint("InterfaceComptable"));
                service.RetourneOperationComptableCompleted += (s, args) =>
                {
                    try
                    {
                        if (args.Cancelled || args.Error != null || args.Result == null)
                        {
                            string error = args.Error.InnerException.ToString();
                            return;
                        }
                        else
                        {
                            if (args.Result != null && args.Result.Count != 0)
                            {
                                ListeOperationComptable = new List<ServiceInterfaceComptable.CsOperationComptable>();
                                ListeOperationComptable = args.Result;
                            }
                            else
                            {
                                Message.ShowInformation("Le système n'a trouvé aucun Compte spécifique dans la base de données ", "Avertissement");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Message.ShowError(ex, Galatee.Silverlight.Resources.Caisse.Langue.errorTitle);
                    }
                };
                service.RetourneOperationComptableAsync();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

