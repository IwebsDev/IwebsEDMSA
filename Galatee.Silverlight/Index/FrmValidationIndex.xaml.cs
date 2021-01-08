using Galatee.Silverlight.MainView;
using Galatee.Silverlight.Resources.Accueil;
using Galatee.Silverlight.ServiceFacturation;
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

namespace Galatee.Silverlight.Facturation
{
    public partial class FrmValidationIndex : ChildWindow
    {
        ServiceAccueil.CsDemande laDetailDemande = null;

        public FrmValidationIndex(int IdDemande)
        {
            InitializeComponent();
            this.txtClient.IsReadOnly = true;
            this.txtOrdre.IsReadOnly = true;

            this.txtClient.MaxLength = SessionObject.Enumere.TailleClient;
            this.txtOrdre.MaxLength = SessionObject.Enumere.TailleOrdre;
            ChargerDonneeDuSite();
            RemplirTypeDemande();
            ChargeDetailDEvis(IdDemande);
        }

        
        private void ChargeDetailDEvis(int IdDemandeDevis)
        {

            ServiceAccueil.AcceuilServiceClient client = new ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
            client.ChargerDetailDemandeCompleted += (ssender, args) =>
            {
                if (args.Cancelled || args.Error != null)
                {
                    LayoutRoot.Cursor = Cursors.Arrow;
                    string error = args.Error.Message;
                    Message.ShowError(error, this.Title.ToString());
                    return;
                }
                if (args.Result == null)
                {
                    LayoutRoot.Cursor = Cursors.Arrow;
                    Message.ShowError(Silverlight.Resources.Devis.Languages.AucunesDonneesTrouvees, this.Title.ToString());
                    return;
                }
                else
                {
                    laDetailDemande = new ServiceAccueil.CsDemande();
                    laDetailDemande = args.Result;
                    this.Txt_CodeSite.Text = laDetailDemande.LaDemande.NUMDEM.Substring(3, 3);
                    this.Txt_CodeCentre.Text = laDetailDemande.LaDemande.CENTRE;
                    this.txtClient.Text = laDetailDemande.LaDemande.CLIENT;
                    this.txtOrdre.Text = laDetailDemande.LaDemande.ORDRE;
                    this.Txt_NomAbon.Text = laDetailDemande.LeClient.NOMABON;
                    this.Txt_Produit.Text = laDetailDemande.LaDemande.LIBELLEPRODUIT;
                    this.Txt_PeriodeEnCour.Text =  Shared.ClasseMEthodeGenerique.FormatPeriodeMMAAAA(laDetailDemande.LstEvenement.First().PERIODE);
                    this.TxtCompteur.Text = laDetailDemande.LstEvenement.First().COMPTEUR;
                    this.Txt_Demande.Text = laDetailDemande.LaDemande.NUMDEM;

                    this.txtMotifDemande.Text = laDetailDemande.LaDemande.MOTIF;
                    this.Txt_AncIndex.Text = laDetailDemande.LstEvenement.First().INDEXEVTPRECEDENT.ToString();
                    this.Txt_NouvIndex.Text = laDetailDemande.LstEvenement.First().INDEXEVT.ToString();
                }
            };
            client.ChargerDetailDemandeAsync (IdDemandeDevis,string.Empty );
        }



        private void UpdateEvenement()
        {
            try
            {
                ServiceAccueil.CsEvenement even = new ServiceAccueil.CsEvenement();
                even = laDetailDemande.LstEvenement.First();
                even.USERMODIFICATION = UserConnecte.matricule;

                laDetailDemande.LaDemande.MATRICULE = UserConnecte.matricule;
                laDetailDemande.LstEvenement.Clear();
                laDetailDemande.LstEvenement.Add(even);

                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.ValiderRepriseIndexCompleted += (s, args) =>
                {
                    try
                    {

                        if (args.Cancelled || args.Error != null)
                        {
                            string error = args.Error.Message;
                            Message.ShowError(error, this.Title.ToString());
                            return;
                        }
                        if (string.IsNullOrEmpty(args.Result))
                        {
                            Message.ShowInformation("Demande validée avec succès", this.Title.ToString());
                            this.DialogResult = true;
                        }
                        else
                            Message.ShowError(args.Result, this.Title.ToString());

                    }
                    catch (Exception ex)
                    {
                        Message.ShowError(ex, this.Title.ToString());
                    }
                };
                service.ValiderRepriseIndexAsync(laDetailDemande);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }




        private void RemplirTypeDemande()
        {
            try
            {
                if (SessionObject.LstTypeDemande == null || SessionObject.LstTypeDemande.Count == 0)
                {
                    ServiceAccueil.AcceuilServiceClient service1 = new ServiceAccueil.AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                    service1.RetourneOptionDemandeCompleted += (sr, res) =>
                    {
                        if (res != null && res.Cancelled)
                            return;
                        SessionObject.LstTypeDemande = res.Result;
                    };
                    service1.RetourneOptionDemandeAsync();
                    service1.CloseAsync();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                var messageBox = new MessageBoxControl.MessageBoxChildWindow("Attention", "Confirmez-vous cette demande ?", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Information);
                messageBox.OnMessageBoxClosed += (_, result) =>
                {
                    if (messageBox.Result == MessageBoxResult.OK)
                        UpdateEvenement();
                    else
                        return;
                };
                messageBox.Show();

            }
            catch (Exception es)
            {
                throw es;
            }
        }






        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        List<int> lesCentreCaisse = new List<int>();
        List<Galatee.Silverlight.ServiceAccueil.CsCentre> LstCentrePerimetre = new List<ServiceAccueil.CsCentre>();
        List<Galatee.Silverlight.ServiceAccueil.CsSite> lstSite = new List<ServiceAccueil.CsSite>();
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

                    if (lstSite.Count == 1)
                    {
                        this.Txt_CodeSite.Text = lstSite.First().CODE;
                        this.Txt_LibelleSite.Text = lstSite.First().LIBELLE;
                        this.Txt_CodeSite.Tag = lstSite.First().PK_ID;
                        lsiteCentre = LstCentrePerimetre.Where(t => t.CODESITE == lstSite.First().CODE).ToList();
                        this.btn_Site.IsEnabled = false;
                    }
                    if (LstCentrePerimetre.Count == 1)
                    {
                        this.Txt_CodeCentre.Text = LstCentrePerimetre.First().CODE;
                        this.Txt_LibelleCentre.Text = LstCentrePerimetre.First().LIBELLE;
                        this.Txt_CodeCentre.Tag = LstCentrePerimetre.First().PK_ID;
                        lProduitSelect = LstCentrePerimetre.First().LESPRODUITSDUSITE;
                        if (lProduitSelect != null && lProduitSelect.Count != 0 )
                        {
                            this.Txt_Produit.Text = lProduitSelect.First().LIBELLE;
                            this.Txt_Produit.Tag = lProduitSelect.First().CODE;
                            this.btn_Produit .Tag = lProduitSelect.First().PK_ID ;
                            //this.txtClient.IsReadOnly = false;
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
                    lstSite = ClasseMEthodeGenerique.RetourneSiteByCentre(LstCentrePerimetre);
                    foreach (Galatee.Silverlight.ServiceAccueil.CsCentre item in LstCentrePerimetre)
                        lesCentreCaisse.Add(item.PK_ID);

                    if (lstSite.Count == 1)
                    {
                        this.Txt_CodeSite.Text = lstSite.First().CODE;
                        this.Txt_LibelleSite.Text = lstSite.First().LIBELLE;
                        this.Txt_CodeSite.Tag = lstSite.First().PK_ID;
                        lsiteCentre = LstCentrePerimetre.Where(t => t.CODESITE == lstSite.First().CODE).ToList();
                        this.btn_Site.IsEnabled = false;
                    }
                    if (LstCentrePerimetre.Count == 1)
                    {
                        this.Txt_CodeCentre.Text = LstCentrePerimetre.First().CODE;
                        this.Txt_LibelleCentre.Text = LstCentrePerimetre.First().LIBELLE;
                        this.Txt_CodeCentre.Tag = LstCentrePerimetre.First().PK_ID;
                        lProduitSelect = LstCentrePerimetre.First().LESPRODUITSDUSITE;
                        if (lProduitSelect != null && lProduitSelect.Count != 0)
                        {
                            this.Txt_Produit.Text = lProduitSelect.First().LIBELLE;
                            this.Txt_Produit.Tag = lProduitSelect.First().CODE;
                            this.btn_Produit.Tag = lProduitSelect.First().PK_ID;
                            //this.txtClient.IsReadOnly = false;
                        }
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
                Message.ShowError(ex.Message, this.Title.ToString());
            }
        }
        List<ServiceAccueil.CsCentre> lsiteCentre = new List<ServiceAccueil.CsCentre>();
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
                lsiteCentre = LstCentrePerimetre.Where(t => t.CODESITE == leSite.CODE).ToList();
                if (lsiteCentre.Count == 1)
                {
                    this.Txt_CodeCentre.Text = lsiteCentre.First().CODE;
                    this.Txt_LibelleCentre.Text = lsiteCentre.First().LIBELLE;
                    this.Txt_CodeCentre.Tag = lsiteCentre.First().PK_ID;
                }
            }
            this.btn_Site.IsEnabled = true;
        }
        private void btn_Centre_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (LstCentrePerimetre.Count > 0)
                {
                    this.btn_Centre.IsEnabled = false;
                    List<object> obj = Shared.ClasseMEthodeGenerique.RetourneListeObjet(lsiteCentre);
                    Dictionary<string, string> _LstColonneAffich = new Dictionary<string, string>();
                    _LstColonneAffich.Add("CODE", "CENTRE");
                    _LstColonneAffich.Add("LIBELLE", "LIBELLE");
                    MainView.UcListeGenerique ctrl = new MainView.UcListeGenerique(obj, _LstColonneAffich, false, "Liste");
                    ctrl.Closed += new EventHandler(galatee_OkClickedCentre);
                    ctrl.Show();
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, this.Title.ToString());
            }
        }
        void galatee_OkClickedCentre(object sender, EventArgs e)
        {
            Galatee.Silverlight.MainView.UcListeGenerique ctrs = sender as Galatee.Silverlight.MainView.UcListeGenerique;
            if (ctrs.isOkClick)
            {
                this.btn_Centre.IsEnabled = true;
                Galatee.Silverlight.ServiceAccueil.CsCentre leCentre = (Galatee.Silverlight.ServiceAccueil.CsCentre)ctrs.MyObject;
                this.Txt_CodeCentre.Text = leCentre.CODE;
                this.Txt_CodeCentre.Tag = leCentre.PK_ID;
                this.Txt_LibelleCentre.Text = leCentre.LIBELLE;
                lProduitSelect = leCentre.LESPRODUITSDUSITE;
                if (lProduitSelect != null && lProduitSelect.Count != 0)
                {
                    this.Txt_Produit.Text = lProduitSelect.First().LIBELLE;
                    this.Txt_Produit.Tag = lProduitSelect.First().CODE;
                    this.btn_Produit.Tag = lProduitSelect.First().PK_ID;
                }
            }
            else
                this.btn_Centre.IsEnabled = true;

        }
        private void btn_Produit_Click(object sender, RoutedEventArgs e)
        {
            if (lProduitSelect != null && lProduitSelect.Count > 0)
            {
                this.btn_Produit.IsEnabled = false;
                List<object> _Listgen = ClasseMEthodeGenerique.RetourneListeObjet(lProduitSelect);
                UcListeGenerique ctr = new UcListeGenerique(_Listgen, "CODE", "LIBELLE", "Liste");
                ctr.Closed += new EventHandler(galatee_OkClickedProduit);
                ctr.Show();
            }
        }
        void galatee_OkClickedProduit(object sender, EventArgs e)
        {
            UcListeGenerique ctrs = sender as UcListeGenerique;
            if (ctrs.isOkClick)
            {
                this.btn_Produit.IsEnabled = true;
                ServiceAccueil.CsProduit leProduit = (ServiceAccueil.CsProduit)ctrs.MyObject;
                this.Txt_Produit.Text = leProduit.LIBELLE;
                this.Txt_Produit.Tag = leProduit.CODE;
                this.btn_Produit.Tag = leProduit.PK_ID;
                //this.txtClient.IsReadOnly = false;
            }
            this.btn_Produit.IsEnabled = true;

        }
        private void Txt_CodeSite_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(Txt_CodeSite.Text) && Txt_CodeSite.Text.Length == SessionObject.Enumere.TailleCentre)
                {
                    Galatee.Silverlight.ServiceAccueil.CsSite _LeSiteClient = ClasseMEthodeGenerique.RetourneObjectFromList(lstSite, this.Txt_CodeSite.Text, "CODE");
                    if (!string.IsNullOrEmpty(_LeSiteClient.LIBELLE))
                    {
                        this.Txt_LibelleSite.Text = _LeSiteClient.LIBELLE;
                        this.Txt_CodeSite.Text = _LeSiteClient.CODE;
                        this.Txt_CodeSite.Tag = _LeSiteClient.PK_ID;
                        List<ServiceAccueil.CsCentre> lsiteCentre = LstCentrePerimetre.Where(t => t.FK_IDCODESITE == (int)this.Txt_CodeSite.Tag).ToList();
                        if (lsiteCentre.Count == 1)
                        {
                            this.Txt_CodeCentre.Text = lsiteCentre.First().CODE;
                            this.Txt_LibelleCentre.Text = lsiteCentre.First().LIBELLE;
                            this.Txt_CodeCentre.Tag = lsiteCentre.First().PK_ID;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, this.Title.ToString());

            }
        }
        private void Txt_CodeCentre_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(Txt_CodeCentre.Text) && Txt_CodeCentre.Text.Length == SessionObject.Enumere.TailleCentre)
                {
                    ServiceAccueil.CsCentre _LeCentreClient = ClasseMEthodeGenerique.RetourneObjectFromList(LstCentrePerimetre.Where(t => t.FK_IDCODESITE == (int)this.Txt_CodeSite.Tag).ToList(), this.Txt_CodeCentre.Text, "CODE");
                    if (!string.IsNullOrEmpty(_LeCentreClient.LIBELLE))
                    {
                        this.Txt_CodeCentre.Text = _LeCentreClient.CODE;
                        this.Txt_LibelleCentre.Text = _LeCentreClient.LIBELLE;
                        this.Txt_CodeCentre.Tag = _LeCentreClient.PK_ID;

                        lProduitSelect = _LeCentreClient.LESPRODUITSDUSITE;
                        if (lProduitSelect != null && lProduitSelect.Count != 0)
                        {
                            this.Txt_Produit.Text = lProduitSelect.First().LIBELLE;
                            this.Txt_Produit.Tag = lProduitSelect.First().CODE;
                            this.btn_Produit.Tag = lProduitSelect.First().PK_ID;
                            //this.txtClient.IsReadOnly = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, this.Title.ToString());

            }
        }

        private void btnRejeter_Click(object sender, RoutedEventArgs e)
        {
            Galatee.Silverlight.Shared.ClasseMEthodeGenerique.RejeterDemande(laDetailDemande);
            this.DialogResult = false;
        }


     

    }
}

