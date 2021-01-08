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

namespace Galatee.Silverlight.InterfaceComptable
{
    public partial class FrmComptabilisation : ChildWindow
    {
        public FrmComptabilisation()
        {
            InitializeComponent();
            ChargerDonneeDuSite();
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
        public FrmComptabilisation(string typeEtat)
        {
            InitializeComponent();
            ChargerDonneeDuSite();
            leEtatExecuter = typeEtat;
            prgBar.Visibility = System.Windows.Visibility.Collapsed;
        }
        List<int> lesCentreCaisse = new List<int>();
        List<Galatee.Silverlight.ServiceAccueil.CsCentre> LstCentrePerimetre = new List<ServiceAccueil.CsCentre>();
        List<Galatee.Silverlight.ServiceAccueil.CsSite> lstSite = new List<Galatee.Silverlight.ServiceAccueil.CsSite>();
        Galatee.Silverlight.ServiceAccueil.CsSite lSiteSelect = null;
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
            lSiteSelect = new ServiceAccueil.CsSite();
            Galatee.Silverlight.MainView.UcListeGenerique ctrs = sender as Galatee.Silverlight.MainView.UcListeGenerique;
            if (ctrs.isOkClick)
            {
                Galatee.Silverlight.ServiceAccueil.CsSite leSite = (Galatee.Silverlight.ServiceAccueil.CsSite)ctrs.MyObject;
                this.Txt_LibelleSite.Text = leSite.LIBELLE;
                this.Txt_LibelleSite.Tag = leSite.PK_ID;
                lSiteSelect = leSite;
            }
            this.btn_Site.IsEnabled = true;

        }
        public static object GetPropValue(object src, string propName)
        {
            return src.GetType().GetProperty(propName).GetValue(src, null);
        }
        Dictionary<string, string> param = new Dictionary<string, string>();
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (param.Count != 0) param.Clear();
            string key = Utility.getKey();
            param.Add("pDateFin", this.dtp_DateFin.SelectedDate.ToString());
            param.Add("pAgence", this.Txt_LibelleSite.Text );

            prgBar.Visibility = System.Windows.Visibility.Visible ;
            if (lSiteSelect != null)
            {
                if (this.dtp_DateFin.SelectedDate == null) this.dtp_DateFin.SelectedDate = DateTime.Today;
                if (chk_auxi.IsChecked == true)
                    RetourneBalanceAuxilliaire(lSiteSelect.CODE , this.dtp_DateFin.SelectedDate);
                if (chk_BalanceAge.IsChecked == true)
                    RetourneBalanceAgee(lSiteSelect.CODE, this.dtp_DateFin.SelectedDate);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
        public void RetourneBalanceAgee(string CodeSite, DateTime? dateFin)
        {
            try
            {
                Galatee.Silverlight.ServiceInterfaceComptable.InterfaceComptableServiceClient service = new Galatee.Silverlight.ServiceInterfaceComptable.InterfaceComptableServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("InterfaceComptable"));
                service.RetourneBalanceAgeeCompleted += (s, args) =>
                {
                    try
                    {
                        prgBar.Visibility = System.Windows.Visibility.Collapsed ;
                        if (args.Cancelled || args.Error != null || args.Result == null)
                        {
                            string error = args.Error.InnerException.ToString();
                            return;
                        }
                        else
                        {
                            if (args.Result != null && args.Result.Count != 0)
                            {
                                if (chk_Excel.IsChecked == true )
                                    Utility.ActionExportation<ServicePrintings.CsBalance, ServiceInterfaceComptable.CsBalance>(args.Result, param, string.Empty, SessionObject.CheminImpression, "BalanceAgee", "InterfaceComptable", true, "xlsx");
                                 else
                                    Utility.ActionDirectOrientation<ServicePrintings.CsBalance, ServiceInterfaceComptable.CsBalance>(args.Result, param , SessionObject.CheminImpression, "BalanceAgee", "InterfaceComptable", false);
                            }
                            else
                            {
                                Message.ShowInformation("Aucune donnée trouvée ", "Balance");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Message.ShowError(ex, Galatee.Silverlight.Resources.Caisse.Langue.errorTitle);
                    }
                };
                service.RetourneBalanceAgeeAsync(CodeSite, dateFin);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void RetourneBalanceAuxilliaire(string CodeSite, DateTime? dateFin)
        {
            try
            {

                Galatee.Silverlight.ServiceInterfaceComptable.InterfaceComptableServiceClient service = new Galatee.Silverlight.ServiceInterfaceComptable.InterfaceComptableServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("InterfaceComptable"));
                service.RetourneBalanceAuxilliaireCompleted += (s, args) =>
                {
                    try
                    {
                        prgBar.Visibility = System.Windows.Visibility.Collapsed;
                        if (args.Cancelled || args.Error != null || args.Result == null)
                        {
                            string error = args.Error.InnerException.ToString();
                            return;
                        }
                        else
                        {
                            if (args.Result != null && args.Result.Count != 0)
                            {
                                List<ServiceInterfaceComptable.CsBalance> lstClient = new List<ServiceInterfaceComptable.CsBalance>();
                                //var lesClient = args.Result.Select(y => new { y.CENTRE, y.CLIENT, y.ORDRE }).OrderBy(o => o.CENTRE).Distinct();
                                //foreach (var item in lesClient)
                                //    lstClient.Add(new ServiceInterfaceComptable.CsBalance() { CENTRE = item.CENTRE, CLIENT = item.CLIENT, ORDRE = item.ORDRE });
                                //int Passage = 0;
                                //string[] tableau = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V" };
                                //while (lstClient.Where(o => o.IsSelect != true).Count() != 0)
                                //{
                                string NomFichier = "BalanceAuxilliaire" ;
                                //    string NomFichier = "BalanceAuxilliaire" + tableau[Passage];
                                //    List<string> clientSelectionne = lstClient.Where(m => m.IsSelect != true).Take(5000).Select(o => o.CLIENT).ToList();
                                //List<ServiceInterfaceComptable.CsBalance> factureAEditer = args.Result.Where(p => clientSelectionne.Contains(p.CLIENT)).ToList();
                                List<ServiceInterfaceComptable.CsBalance> factureAEditer = args.Result.ToList();
                                Dictionary<string, string> param = new Dictionary<string, string>();
                                param.Add("AnneFin", dateFin.Value.Year.ToString());
                                    if (this.chk_Excel.IsChecked == true)
                                        Utility.ActionExportation<ServicePrintings.CsBalance, ServiceInterfaceComptable.CsBalance>(factureAEditer, param, NomFichier, SessionObject.CheminImpression, "BalanceAuxilliaire", "InterfaceComptable", true, "xlsx");
                                    else
                                        Utility.ActionDirectOrientation<ServicePrintings.CsBalance, ServiceInterfaceComptable.CsBalance>(factureAEditer, param, SessionObject.CheminImpression, "BalanceAuxilliaire", "InterfaceComptable", false);
                                //    lstClient.Where(p => clientSelectionne.Contains(p.CLIENT)).ToList().ForEach(p => p.IsSelect = true);
                                //    Passage++;
                                //}
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
                service.RetourneBalanceAuxilliaireAsync(CodeSite, dateFin);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public void RetourneBalanceAuxilliaire(string CodeSite, DateTime? dateFin)
        //{
        //    try
        //    {
        //        Shared.ClasseMEthodeGenerique.SetMachineAndPortFromEndPoint(Utility.EndPoint("Printing"));
        //        Galatee.Silverlight.ServiceInterfaceComptable.InterfaceComptableServiceClient service = new Galatee.Silverlight.ServiceInterfaceComptable.InterfaceComptableServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("InterfaceComptable"));
        //        service.RetourneBalanceAuxilliaire_BlockCompleted += (s, args) =>
        //        {
        //            try
        //            {
        //                prgBar.Visibility = System.Windows.Visibility.Collapsed ;
        //                if (args.Cancelled || args.Error != null || args.Result == null)
        //                {
        //                    string error = args.Error.InnerException.ToString();
        //                    return;
        //                }
        //                else
        //                {
        //                    //if (args.Result != null && args.Result.Count != 0)
        //                    //{
        //                    if (args.Result != null && args.Result != false)
        //                    {
        //                        //string format = "pdf";
        //                        //if (chk_Excel.IsChecked == true) format = "xlsx";

        //                        //List<ServiceAccueil.CsBalance> l = Utility.ConvertListType<ServiceAccueil.CsBalance, ServiceInterfaceComptable.CsBalance>(args.Result);
        //                        //Utility.ActionExportationWithSpliting(l, param, string.Empty, SessionObject.CheminImpression, "BalanceAuxilliaire", "InterfaceComptable", true, format, 1000);
        //                        Message.ShowInformation("L'impression est en cours de traitement", "Information");
                            
        //                    }
        //                    else
        //                    {
        //                        Message.ShowInformation("Le système n'a trouvé aucun Compte spécifique dans la base de données ", "Avertissement");
        //                    }
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                Message.ShowError(ex, Galatee.Silverlight.Resources.Caisse.Langue.errorTitle);
        //            }
        //        };
        //        service.RetourneBalanceAuxilliaire_BlockAsync(CodeSite, dateFin,chk_Excel.IsChecked.Value,Txt_LibelleSite.Text,SessionObject.CheminImpression,2000,UserConnecte.matricule,SessionObject.ServerEndPointName,SessionObject.ServerEndPointPort,Utility.GetHTTPWebAppBaseAddresse());

        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public void RetourneBalanceAuxilliaire(string CodeSite, DateTime? dateFin)
        //{
        //    try
        //    {
        //        Shared.ClasseMEthodeGenerique.SetMachineAndPortFromEndPoint(Utility.EndPoint("Printing"));
        //        Galatee.Silverlight.ServiceInterfaceComptable.InterfaceComptableServiceClient service = new Galatee.Silverlight.ServiceInterfaceComptable.InterfaceComptableServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("InterfaceComptable"));
        //        service.RetourneBalanceAuxilliaireCompleted += (s, args) =>
        //        {
        //            try
        //            {
        //                prgBar.Visibility = System.Windows.Visibility.Collapsed;
        //                if (args.Cancelled || args.Error != null || args.Result == null)
        //                {
        //                    string error = args.Error.InnerException.ToString();
        //                    return;
        //                }
        //                else
        //                {
        //                    if (args.Result != null && args.Result.Count != 0)
        //                    {
        //                        string format = "pdf";
        //                        string rdlcName = "BalanceAuxilliaire";
        //                        if (chk_Excel.IsChecked == true) format = "xlsx";

        //                        Utility.ActionPreview<ServicePrintings.CsBalance, ServiceInterfaceComptable.CsBalance>(args.Result, null,rdlcName, "InterfaceComptable");
        //                        //Utility.ActionExportFormatWithSplitingPrinting<ServicePrintings.CsBalance, ServiceInterfaceComptable.CsBalance>(args.Result, null, SessionObject.CheminImpression, rdlcName, "InterfaceComptable", false, format);

        //                    }
        //                    else
        //                    {
        //                        Message.ShowInformation("Le système n'a trouvé aucun Compte spécifique dans la base de données ", "Avertissement");
        //                    }
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                Message.ShowError(ex, Galatee.Silverlight.Resources.Caisse.Langue.errorTitle);
        //            }
        //        };
        //        service.RetourneBalanceAuxilliaireAsync(CodeSite, dateFin);

        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
    }
}

