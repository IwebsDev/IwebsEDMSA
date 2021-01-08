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

namespace Galatee.Silverlight.Scelles
{
    public partial class FrmCompteurAffecter : ChildWindow
    {
        public FrmCompteurAffecter()
        {
            InitializeComponent();
            ChargerDonneeDuSite();
            ChargerTypeDemande();
            ChargerListeDeProduit();
            ChargerDiametreCompteur();
        }
        string leEtatExecuter = string.Empty;
        public FrmCompteurAffecter(string typeEtat)
        {
            InitializeComponent();
            ChargerDonneeDuSite();
            ChargerTypeDemande();
            ChargerListeDeProduit();
            ChargerDiametreCompteur();
            leEtatExecuter = typeEtat;
            this.Chk_Disponnible.IsChecked = true;
        }
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            EtatDesCompteur();
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
                        }
                    }
                    if (LstCentrePerimetre.Count == 1)
                    {
                        this.Txt_LibelleCentre.Text = LstCentrePerimetre.First().CODE;
                        this.Txt_LibelleCentre.Text = LstCentrePerimetre.First().LIBELLE;
                        this.Txt_LibelleCentre.Tag = LstCentrePerimetre.First();
                        lProduitSelect = LstCentrePerimetre.First().LESPRODUITSDUSITE;

                        if (lProduitSelect != null && lProduitSelect.Count != 0)
                        {
                            if (lProduitSelect.Count == 1)
                            {
                                this.Txt_Produit.Text = lProduitSelect.First().LIBELLE;
                                this.Txt_Produit.Tag = lProduitSelect.First().CODE;
                            }
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

        private void ChargerTypeDemande()
        {
            try
            {
                if (SessionObject.LstTypeDemande != null && SessionObject.LstTypeDemande.Count != 0)
                    return;

                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service1 = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                service1.RetourneOptionDemandeCompleted += (sr, res) =>
                {
                    if (res != null && res.Cancelled)
                        return;
                    SessionObject.LstTypeDemande = res.Result;
                };
                service1.RetourneOptionDemandeAsync();
                service1.CloseAsync();
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
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service1 = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
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


        private void ChargerDiametreCompteur()
        {
            try
            {
                if (SessionObject.LstCalibreCompteur != null && SessionObject.LstCalibreCompteur.Count != 0)
                    return;
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                service.ChargerCalibreCompteurCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    SessionObject.LstCalibreCompteur = args.Result;
                };
                service.ChargerCalibreCompteurAsync();
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
                this.Txt_LibelleCentre.Tag = leCentre.CODE ;
            }
            this.btn_Centre.IsEnabled = true;
        }

        private void btn_Produit_Click(object sender, RoutedEventArgs e)
        {
            if (lProduitSelect != null && lProduitSelect.Count > 0)
            {
                this.btn_Produit.IsEnabled = false;
                List<object> _Listgen = Galatee.Silverlight.Shared.ClasseMEthodeGenerique.RetourneListeObjet(lProduitSelect);
                Galatee.Silverlight.MainView.UcListeGenerique ctr = new Galatee.Silverlight.MainView.UcListeGenerique(_Listgen, "CODE", "LIBELLE", "Liste");
                ctr.Closed += new EventHandler(galatee_OkClickedProduit);
                ctr.Show();
            }
        }
        void galatee_OkClickedProduit(object sender, EventArgs e)
        {
            Galatee.Silverlight.MainView.UcListeGenerique ctrs = sender as Galatee.Silverlight.MainView.UcListeGenerique;
            if (ctrs.isOkClick)
            {
                ServiceAccueil.CsProduit leProduit = (ServiceAccueil.CsProduit)ctrs.MyObject;
                this.Txt_Produit.Text = leProduit.LIBELLE;
                this.Txt_Produit.Tag = leProduit.CODE  ;
            }
            this.btn_Produit.IsEnabled = true;
        }

        private void btn_Calibre_Click(object sender, RoutedEventArgs e)
        {
            if (SessionObject.LstCalibreCompteur != null && SessionObject.LstCalibreCompteur.Count > 0 )
            {
                if (this.Txt_Produit.Tag == null)
                {
                    Message.ShowInformation("Selectionnez le produit avant", "Reporting");
                    return;
                }
                this.btn_Calibre .IsEnabled = false;
                List<object> _Listgen = Galatee.Silverlight.Shared.ClasseMEthodeGenerique.RetourneListeObjet(SessionObject.LstCalibreCompteur.Where(t=>t.FK_IDPRODUIT == (int)this.Txt_Produit.Tag ).ToList());
                Galatee.Silverlight.MainView.UcListeGenerique ctr = new Galatee.Silverlight.MainView.UcListeGenerique(_Listgen, "CODE", "LIBELLE", "Liste");
                ctr.Closed += new EventHandler(galatee_OkClickedCalibre);
                ctr.Show();
            }
        }
        void galatee_OkClickedCalibre(object sender, EventArgs e)
        {
            Galatee.Silverlight.MainView.UcListeGenerique ctrs = sender as Galatee.Silverlight.MainView.UcListeGenerique;
            if (ctrs.isOkClick)
            {
                ServiceAccueil.CsCalibreCompteur leCalibre = (ServiceAccueil.CsCalibreCompteur)ctrs.MyObject;
                this.Txt_Calibre.Text = leCalibre.LIBELLE;
                this.Txt_Calibre.Tag = leCalibre.PK_ID;
            }
            this.btn_Produit.IsEnabled = true;
        }
        private void EtatDesCompteur()
        {
            Dictionary<string, string> param = new Dictionary<string, string>();
            string centre = this.Txt_LibelleCentre.Tag ==null  ? string.Empty :((Galatee.Silverlight.ServiceAccueil.CsCentre) this.Txt_LibelleCentre.Tag).CODE ;
            string produit = string.IsNullOrEmpty(this.Txt_Produit.Text) ? string.Empty : this.Txt_Produit.Tag.ToString();
            int? Calibre = this.Txt_Calibre.Tag != null ? (int?)this.Txt_Calibre.Tag  : null ;
            string EtatCompteur = "1";

           if (this.Chk_Disponnible .IsChecked == true && this.Chk_Attribuer .IsChecked == false  )
            {
                EtatCompteur = "1";
                param.Add("pParametre", "LISTE DES COMPTEURS DISPONIBLES");
            }
           else  if (this.Chk_Attribuer.IsChecked == true && this.Chk_Disponnible.IsChecked == true)
            {
                EtatCompteur = "3";
                param.Add("pParametre", "ETAT DES COMPTEURS");

            }
            else if (this.Chk_Attribuer.IsChecked == true && this.Chk_Disponnible.IsChecked == false  )
            {
                EtatCompteur = "2";
                param.Add("pParametre", "LISTE DES COMPTEURS ATTRIBUES");
            }
            if (string.IsNullOrEmpty(this.Txt_LibelleCentre.Text))
            {
                Message.ShowInformation("Veuillez selectionner le centre", "Reporting");
                return;
            }
            List<string> lstCEntre = new List<string>();
            lstCEntre.Add(centre);

            Galatee.Silverlight.ServiceReport.ReportServiceClient service1 = new Galatee.Silverlight.ServiceReport.ReportServiceClient(Utility.Protocole(), Utility.EndPoint("Report"));
            service1.ReturnecompteursDisponiblesEnMagasinCompleted += (sr, res) =>
            {
                if (res != null && res.Cancelled)
                    return;

                if (res.Result != null && res.Result.Count != 0)
                {

                    Utility.ActionDirectOrientation<ServicePrintings.CsCanalisation, ServiceReport.CsCanalisation>(res.Result, param, SessionObject.CheminImpression, "EtatCompteur", "Report", true);
                }
                else
                {
                    Message.ShowInformation("Aucune information trouvée", "Report");
                    return;
                }
            };
            service1.ReturnecompteursDisponiblesEnMagasinAsync(lstCEntre, Calibre, produit, EtatCompteur );
            service1.CloseAsync();
        }
    }
}

