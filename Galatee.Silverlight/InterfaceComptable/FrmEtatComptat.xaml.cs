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
    public partial class FrmEtatComptat : ChildWindow
    {
        public FrmEtatComptat()
        {
            InitializeComponent();
            ChargerDonneeDuSite();
            ChargerCategorie();
            prgBar.Visibility = System.Windows.Visibility.Collapsed;
        }
        string leEtatExecuter = string.Empty;

        public FrmEtatComptat(string typeEtat)
        {
            InitializeComponent();
            ChargerDonneeDuSite();
            ChargerCategorie();
            leEtatExecuter = typeEtat;
            prgBar.Visibility = System.Windows.Visibility.Collapsed;
            if (leEtatExecuter == SessionObject.AvanceSurConso)
            {
                this.Txt_LibelleCategorie.Visibility = System.Windows.Visibility.Collapsed;
                this.btn_Categorie.Visibility = System.Windows.Visibility.Collapsed;
                this.lbl_Centre_Copy2.Visibility = System.Windows.Visibility.Collapsed;
            }
            if (leEtatExecuter == SessionObject.ExtractionImpayes)
            {
                this.rdb_Actif.Visibility = System.Windows.Visibility.Collapsed;
                this.rdb_Resilier.Visibility = System.Windows.Visibility.Collapsed;
            }
        }
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<int> lstCentre = new List<int>();
                List<string> lstCateg = new List<string>();
                List<string> lstProduit = new List<string>();
                DateTime dateDebut = System.DateTime.Today;
                DateTime dateFin = dateDebut.AddYears(3);
                string Typedemande = string.Empty;
                string Produit = string.Empty;
                bool IsResilier = rdb_Resilier.IsChecked == true ? true : false;
                lstProduit.Add(SessionObject.Enumere.Electricite);
                lstProduit.Add(SessionObject.Enumere.ElectriciteMT);

                dateDebut = string.IsNullOrEmpty(this.dtp_DateDebut.Text) ? dateDebut : Convert.ToDateTime(this.dtp_DateDebut.Text);
                dateFin = string.IsNullOrEmpty(this.dtp_DateFin.Text) ? dateFin : Convert.ToDateTime(this.dtp_DateFin.Text);

                prgBar.Visibility = System.Windows.Visibility.Visible;
                if (this.Txt_LibelleCategorie.Tag != null)
                    lstCateg = ((List<string>)this.Txt_LibelleCategorie.Tag);

                if (leEtatExecuter == SessionObject.AvanceSurConso)
                    AvanceSurConsomation(btn_Site.Tag.ToString(), IsResilier, dateDebut, dateFin);
                if (leEtatExecuter == SessionObject.ExtractionImpayes)
                    Provision(btn_Site.Tag.ToString(), lstCateg, lstProduit, dateDebut, dateFin);
            }
            catch (Exception ex)
            {
                prgBar.Visibility = System.Windows.Visibility.Collapsed ;
                Message.Show(ex.Message ,"Erreur");
            }
            
        }
        private void AvanceSurConsomation(string CodeSite, bool IsResilier, DateTime? DateDebut, DateTime? DateFin)
        {
            Galatee.Silverlight.ServiceInterfaceComptable.InterfaceComptableServiceClient service1 = new ServiceInterfaceComptable.InterfaceComptableServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("InterfaceComptable"));
            service1.RetourneAvanceSurConsomationCompleted += (sr, res) =>
            {
                prgBar.Visibility = System.Windows.Visibility.Collapsed;

                if (res != null && res.Cancelled)
                    return;

                if (res.Result != null && res.Result.Count != 0)
                    Utility.ActionExportation<ServicePrintings.CsLclient, ServiceInterfaceComptable.CsLclient>(res.Result, null,string.Empty, SessionObject.CheminImpression, "AvanceSurConsomation", "InterfaceComptable", true, "xlsx");

                else
                {
                    Message.ShowInformation("Aucune information trouvée", "Report");
                    return;
                }
            };
            service1.RetourneAvanceSurConsomationAsync(CodeSite,IsResilier,DateDebut,DateFin);
            service1.CloseAsync();
        }
        private void Provision(string CodeSite, List<string> lstCateg, List<string> lstProd, DateTime? DateDebut, DateTime? DateFin)
        {
            Galatee.Silverlight.ServiceInterfaceComptable.InterfaceComptableServiceClient service1 = new ServiceInterfaceComptable.InterfaceComptableServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("InterfaceComptable"));
            service1.RetourneProvisionCompleted += (sr, res) =>
            {
                prgBar.Visibility = System.Windows.Visibility.Collapsed;

                if (res != null && res.Cancelled)
                    return;

                if (res.Result != null && res.Result.Count != 0)
                    Utility.ActionExportation<ServicePrintings.CsClient, ServiceInterfaceComptable.CsClient>(res.Result, null, string.Empty, SessionObject.CheminImpression, "ProvisionComptable", "InterfaceComptable", true, "xlsx");

                else
                {
                    Message.ShowInformation("Aucune information trouvée", "Report");
                    return;
                }
            };
            service1.RetourneProvisionAsync(CodeSite, lstCateg, lstProd, DateDebut, DateFin);
            service1.CloseAsync();
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
            }
            this.btn_Site.IsEnabled = true;

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
                        this.Txt_LibelleCategorie.Tag = ListeCategorie.Select(t => t.CODE ).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
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
    }
}

