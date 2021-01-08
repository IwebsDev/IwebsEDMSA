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
    public partial class FrmImpayer : ChildWindow
    {
        public FrmImpayer()
        {
            InitializeComponent();
            ChargerDonneeDuSite();
            ChargerListeDeProduit();
            ChargerTournee();
            ChargerCategorie();
            prgBar.Visibility = System.Windows.Visibility.Collapsed ;
            Chk_ExportExcel.IsChecked = true;
            this.rdb_Recap.IsChecked = true;

        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            //this.DialogResult = true;
            if (this.btn_Site.Tag == null )
            {
             Message.ShowInformation("Selectionnez le site","Reporting");
             return ;
            
            }

            List<int> lstCentre =new List<int>();
            List<int> lstCategorie =new List<int>();
            List<int> lstTournee =new List<int>();
            lstTournee = (List<int>)this.Txt_LibelleTournee.Tag;
            lstCategorie =(List<int>) this.Txt_LibelleCategorie.Tag;
            if (this.Txt_LibelleCentre.Tag !=null )
                lstCentre.AddRange((List<int>)this.Txt_LibelleCentre.Tag);
            else
                lstCentre.AddRange (LstCentrePerimetre.Where(o=>o.CODESITE == this.btn_Site.Tag.ToString()).Select(y=>y.PK_ID).ToList());


            Dictionary<string, List<int>> lesDeCentre = new Dictionary<string, List<int>>();
            lesDeCentre.Add(this.btn_Site.Tag.ToString(), lstCentre);
            bool IsDetail = false;
            if (this.rdb_Detail.IsChecked == true)
                IsDetail = true ;
            ImpayerCategorie(lesDeCentre, lstCategorie, lstTournee, IsDetail);
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
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
                            this.btn_Site.Tag = lstSite.First().PK_ID ;
                        }
                    }
                    if (LstCentrePerimetre.Count == 1)
                    {
                        List<int> lstIdCentre = new List<int>();
                        lstIdCentre.Add(LstCentrePerimetre.First().PK_ID);
                        this.Txt_LibelleCentre.Text = LstCentrePerimetre.First().CODE;
                        this.Txt_LibelleCentre.Tag = lstIdCentre;

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
                            List<int> lstIdCentre = new List<int>();
                            lstIdCentre.Add(LstCentrePerimetre.First().PK_ID);
                            this.Txt_LibelleCentre.Text = LstCentrePerimetre.First().LIBELLE;
                            this.Txt_LibelleCentre.Tag = lstIdCentre;
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

        private void ChargerCategorie()
        {
            try
            {
                if (SessionObject.LstCategorie.Count != 0)
                    return;
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
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

        private void ImpayerCategorie(Dictionary<string, List<int>> lstCentre, List<int> lstIdCategorieClient, List<int> lstIdTournee,bool IsDetail)
        {
            prgBar.Visibility = System.Windows.Visibility.Visible;
            Galatee.Silverlight.ServiceReport.ReportServiceClient service1 = new Galatee.Silverlight.ServiceReport.ReportServiceClient(Utility.ProtocoleFacturation (), Utility.EndPoint("Report"));
            service1.ReturneListeDesImpayesCompleted += (sr, res) =>
            {
                prgBar.Visibility = System.Windows.Visibility.Collapsed ;

                if (res != null && res.Cancelled)
                    return;

                if (res.Result != null && res.Result.Count != 0)
                {
                    string Rdlc = "ImpayeCategorieTournee";
                    if (btn_Site.Tag.ToString() == SessionObject.Enumere.CodeSiteScaBT ||
                        btn_Site.Tag.ToString() == SessionObject.Enumere.CodeSiteScaMT)
                        Rdlc = "ImpayeCategorieGCTournee";

                    if (this.rdb_Detail.IsChecked==true )
                        Rdlc = "ImpayeCategorieDetail";

                    List<ServiceReport.CsLclient> lstClient = new List<ServiceReport.CsLclient>();
                    var lesClient = res.Result.Select(y => new { y.CENTRE , y.CLIENT , y.ORDRE  }).Distinct();
                    foreach (var item in lesClient)
                        lstClient.Add(new ServiceReport.CsLclient() { CENTRE = item.CENTRE, CLIENT = item.CLIENT, ORDRE = item.ORDRE });
                    int Passage = 0;
                    string[] tableau = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "AB", "AC", "AD", "AE", "AF", "AG", "AH" };
                    while (lstClient.Where(o => o.Selectionner != true).Count() != 0)
                    {
                        string NomFichier = Rdlc + tableau[Passage];
                        List<string> clientSelectionne = lstClient.Where(m => m.Selectionner != true).Take(2000).Select(o => o.CLIENT).ToList();
                        List<ServiceReport.CsLclient> factureAEditer = res.Result.Where(p => clientSelectionne.Contains(p.CLIENT)).ToList();

                        if (Chk_ExportExcel.IsChecked != true)
                            Utility.ActionDirectOrientation<ServicePrintings.CsLclient, ServiceReport.CsLclient>(res.Result, null, SessionObject.CheminImpression, Rdlc, "Report", true);
                        else
                            Utility.ActionExportation<ServicePrintings.CsLclient, ServiceReport.CsLclient>(res.Result, null, string.Empty, SessionObject.CheminImpression, Rdlc + "Xls", "Report", true, "xlsx");

                        lstClient.Where(p => clientSelectionne.Contains(p.CLIENT)).ToList().ForEach(p => p.Selectionner  = true);
                        Passage++;
                    }
               
                }
                else
                {
                    Message.ShowInformation("Aucune information trouvée", "Report");
                    return;
                }
            };
            service1.ReturneListeDesImpayesAsync(lstCentre, lstIdCategorieClient, lstIdTournee, IsDetail);
            service1.CloseAsync();

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
                btn_Site.Tag = leSite.CODE;
                lSiteSelect = leSite;
                List<ServiceAccueil.CsCentre> lsiteCentre = LstCentrePerimetre.Where(t => t.FK_IDCODESITE == (int)this.Txt_LibelleSite.Tag).ToList();
                if (lsiteCentre.Count == 1)
                {
                    List<int> lstIdCentreSelect = new List<int>();
                    lstIdCentreSelect.Add(lsiteCentre.First().PK_ID);
                    this.Txt_LibelleCentre.Text = lsiteCentre.First().LIBELLE;
                    this.Txt_LibelleCentre.Tag = lstIdCentreSelect;
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
            try
            {
                if (LstCentrePerimetre.Count != 0)
                {
                    List<ServiceAccueil.CParametre> lstParametre = Shared.ClasseMEthodeGenerique.RetourneValueFromClasse<ServiceAccueil.CsCentre>(LstCentrePerimetre);
                    Shared.UcListeParametre ctr = new Galatee.Silverlight.Shared.UcListeParametre(lstParametre, true, "Centre");
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
                    List<ServiceAccueil.CParametre> _LesCentreSelect = ctrs.MyObjectList as List<ServiceAccueil.CParametre>;
                    List<string> lstCentre = _LesCentreSelect.Select(t => t.CODE).ToList();
                    foreach (string item in lstCentre)
                        this.Txt_LibelleCentre.Text = item + " ";
                    this.Txt_LibelleCentre.Tag = _LesCentreSelect.Select(t => t.PK_ID).ToList();
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
            }
        }

        private void btnCategorie_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<ServiceAccueil.CParametre> lstParametre = Shared.ClasseMEthodeGenerique.RetourneValueFromClasse<ServiceAccueil.CsCategorieClient>(SessionObject.LstCategorie);
                Shared.UcListeParametre ctr = new Galatee.Silverlight.Shared.UcListeParametre(lstParametre, true, "Liste de catégorie");
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
                                this.Txt_LibelleCategorie.Text = p.CODE;
                            else
                                this.Txt_LibelleCategorie.Text = this.Txt_LibelleCategorie.Text + "  " + p.CODE;
                            passage++;
                        }
                        this.Txt_LibelleCategorie.Tag = ListeCategorie.Select(t=>t.PK_ID).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
            }
        }

        private void btnzone_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (SessionObject.LstZone == null || SessionObject.LstZone.Count == 0)
                    return;

                if (this.Txt_LibelleCentre.Tag == null)
                {
                    Message.ShowInformation("Selectionnez le centre", "Reporting");
                    return;
                }
                List<int> lstIdCentreSelect = (List<int>)this.Txt_LibelleCentre.Tag;
                List<ServiceAccueil.CsTournee> lstTourneCentre = SessionObject.LstZone.Where(t =>lstIdCentreSelect.Contains(t.FK_IDCENTRE)).ToList();
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
                        this.Txt_LibelleTournee.Tag = ListeDesZones.Select(t=>t.PK_ID).ToList();
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

