using Galatee.Silverlight.MainView;
using Galatee.Silverlight.ServiceRecouvrement;
using Galatee.Silverlight.Shared;
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

namespace Galatee.Silverlight.Recouvrement
{
    public partial class FrmSaisiRDVHorsCampagne : ChildWindow
    {
        public FrmSaisiRDVHorsCampagne()
        {
            InitializeComponent();
            ChargerDonneeDuSite();
            prgBar.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (Lsv_ListFacture.ItemsSource != null  && !string.IsNullOrEmpty( this.Txt_DateRendezVous.Text))
                ValiderRendezVous(_UnClient, Convert.ToDateTime(this.Txt_DateRendezVous.Text));
        }
        private void ValiderRendezVous(CsClient leClient,DateTime laDateRendezVous)
        {
            try
            {
                Galatee.Silverlight.ServiceRecouvrement.RecouvrementServiceClient client = new Galatee.Silverlight.ServiceRecouvrement.RecouvrementServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Recouvrement"));
                client.SaveRDVCoupureHorsCampagneAsync(leClient, laDateRendezVous);
                client.SaveRDVCoupureHorsCampagneCompleted += (ss, args) =>
                {
                    try
                    {
                        if (args.Cancelled || args.Error != null)
                        {
                            string error = args.Error.Message;
                            Message.ShowError("Error à l'appel du service", "SearchCampagne");
                            return;
                        }
                        if (args.Result == true)
                        {
                            Message.ShowInformation ("Mise a jour validée", "Rendez-vous");
                            this.DialogResult = false;
                            return;
                        }
                        this.DialogResult = false;
                    }
                    catch (Exception ex)
                    {
                        Message.ShowError(ex, Galatee.Silverlight.Resources.Langue.errorTitle);
                    }
                };
            }
            catch (Exception ex)
            {
              Message.ShowError(ex.Message , Galatee.Silverlight.Resources.Langue.errorTitle);
            }
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
                    lstSite = ClasseMEthodeGenerique.RetourneSiteByCentre(LstCentrePerimetre);
                    foreach (Galatee.Silverlight.ServiceAccueil.CsCentre item in LstCentrePerimetre)
                        lesCentreCaisse.Add(item.PK_ID);

                    if (lstSite != null && lstSite.Count != 0)
                    {
                        if (lstSite.Count == 1)
                        {
                            lSiteSelect = lstSite.First();
                            this.Txt_LibelleSite.Text = lstSite.First().LIBELLE;
                            this.btn_Site.IsEnabled = false;
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
                    lstSite = ClasseMEthodeGenerique.RetourneSiteByCentre(LstCentrePerimetre);
                    foreach (Galatee.Silverlight.ServiceAccueil.CsCentre item in LstCentrePerimetre)
                        lesCentreCaisse.Add(item.PK_ID);
                    if (lstSite != null && lstSite.Count != 0)
                    {
                        if (lstSite.Count == 1)
                        {
                            lSiteSelect = lstSite.First();
                            this.Txt_LibelleSite.Text = lstSite.First().LIBELLE;
                            this.btn_Site.IsEnabled = false;
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

        private void btn_Site_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (lstSite.Count > 0)
                {
                    List<object> _Listgen = ClasseMEthodeGenerique.RetourneListeObjet(lstSite);
                    UcListeGenerique ctr = new UcListeGenerique(_Listgen, "CODE", "LIBELLE", "Liste");
                    ctr.Closed += new EventHandler(galatee_OkClickedSite);
                    ctr.Show();
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, "Campagne");
            }

        }
        void galatee_OkClickedSite(object sender, EventArgs e)
        {
            UcListeGenerique ctrs = sender as UcListeGenerique;
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
                    this.btn_Centre.IsEnabled = false;
                    lProduitSelect = lsiteCentre.First().LESPRODUITSDUSITE;

                }
                else
                {
                    this.Txt_LibelleCentre.Text = string.Empty;
                    this.Txt_LibelleCentre.Tag = null;
                    this.btn_Centre.IsEnabled = true;
                }
            }

        }
        private void btn_Centre_Click(object sender, RoutedEventArgs e)
        {
            if (this.Txt_LibelleSite.Tag != null)
            {
                List<Galatee.Silverlight.ServiceAccueil.CsCentre> lstCentreSite = LstCentrePerimetre.Where(t => t.FK_IDCODESITE == (int)this.Txt_LibelleSite.Tag).ToList();
                if (lstCentreSite.Count > 0)
                {
                    this.btn_Site.IsEnabled = false;
                    List<object> _Listgen = Galatee.Silverlight.Shared.ClasseMEthodeGenerique.RetourneListeObjet(lstCentreSite);
                    UcListeGenerique ctr = new UcListeGenerique(_Listgen, "CODE", "LIBELLE", "Liste");
                    ctr.Closed += new EventHandler(galatee_OkClickedClient);
                    ctr.Show();
                }

            }
        }
        void galatee_OkClickedClient(object sender, EventArgs e)
        {
            UcListeGenerique ctrs = sender as UcListeGenerique;
            if (ctrs.isOkClick)
            {
                this.btn_Centre.IsEnabled = true;
                Galatee.Silverlight.ServiceAccueil.CsCentre leCentre = (Galatee.Silverlight.ServiceAccueil.CsCentre)ctrs.MyObject;
                this.Txt_LibelleCentre.Text = leCentre.LIBELLE;
                this.Txt_LibelleCentre.Tag = leCentre;

            }

        }
        CsClient _UnClient = new CsClient();

    private void AfficherImpayes()
        {
            try
            {

                this.Lsv_ListFacture.ItemsSource = null;
                if (this.Txt_LibelleCentre.Tag != null)
                {
                    _UnClient.CENTRE = ((ServiceAccueil. CsCentre)this.Txt_LibelleCentre.Tag).CODE;
                    _UnClient.REFCLIENT = this.txtReferenceClient.Text;
                    _UnClient.ORDRE = this.txtOrdeClient.Text;
                    _UnClient.FK_IDCENTRE = ((ServiceAccueil.CsCentre)this.Txt_LibelleCentre.Tag).PK_ID;
                }
                else
                {
                    Message.ShowInformation("Sélectionnez le centre", "Index");
                    return;
                }
                prgBar.Visibility = System.Windows.Visibility.Visible;

                RecouvrementServiceClient client = new RecouvrementServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Recouvrement"));
                client.RetourneListeFactureNonSoldeCaisseAsync(_UnClient);
                client.RetourneListeFactureNonSoldeCaisseCompleted += (s, args) =>
                {
                    prgBar.Visibility = System.Windows.Visibility.Collapsed;
                    if (args != null && args.Cancelled)
                        return;
                    if (args.Result == null || args.Result.Count == 0)
                    {
                        Message.ShowInformation("Ce client n'existe pas", "Rendeez-vous");
                        return;
                    }
                    List<CsLclient> lstFactureDuClient = args.Result;
                    lstFactureDuClient.ForEach(t => t.REFEMNDOC = t.REFEM);
                    List<CsClient> LstClientDeLaReference = MethodeGenerics.RetourneClientFromFacture(lstFactureDuClient);
                       
                    lstFactureDuClient.ForEach(t => t.REFEM = ClasseMEthodeGenerique.FormatPeriodeMMAAAA(t.REFEM));
                    foreach (var item in lstFactureDuClient)
                    {
                        item.MONTANTPAYPARTIEL = item.MONTANT - item.SOLDEFACTURE;
                        if (item.MONTANTPAYPARTIEL < 0) item.MONTANTPAYPARTIEL = 0;
                    }
                    lstFactureDuClient.ForEach(t => t.MONTANTPAYPARTIEL  = t.MONTANT - t.SOLDEFACTURE );
                    this.Txt_SoldeClient.Text = lstFactureDuClient.Sum(t => t.SOLDEFACTURE).Value.ToString(SessionObject.FormatMontant);
                    _UnClient.PK_ID = lstFactureDuClient.First().FK_IDCLIENT;
                    Lsv_ListFacture.ItemsSource = null;
                    Lsv_ListFacture.ItemsSource = lstFactureDuClient.Where(t => t.FK_IDCLIENT == lstFactureDuClient.First().FK_IDCLIENT);
                };
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message ,"Rendez-vous");
            }
          }

    private void btnsearch_Click(object sender, RoutedEventArgs e)
    {
        AfficherImpayes();
    }
    }
}

