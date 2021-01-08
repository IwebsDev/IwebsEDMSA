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
    public partial class FrmRepriseIndex : ChildWindow
    {
        ServiceAccueil.CsDemande laDetailDemande = null;
        
        public FrmRepriseIndex()
        {
            InitializeComponent();
            this.txtClient.IsReadOnly = true;
            this.txtOrdre.IsReadOnly = true;
            this.txtDemande.Visibility = System.Windows.Visibility.Collapsed;
            this.labDemande.Visibility = System.Windows.Visibility.Collapsed;
            this.labMotifRejet.Visibility = System.Windows.Visibility.Collapsed;
            this.txtMotifRejet.Visibility = System.Windows.Visibility.Collapsed;

            this.OKButton.IsEnabled = (this.Cbo_Compteur.SelectedItem != null) && !string.IsNullOrEmpty(this.txtMotifDemande.Text) && !string.IsNullOrEmpty(this.Txt_NouvIndex.Text);

            this.txtClient.MaxLength = SessionObject.Enumere.TailleClient;
            this.txtOrdre.MaxLength = SessionObject.Enumere.TailleOrdre;
            ChargerDonneeDuSite();
            RemplirTypeDemande();
        }




        public FrmRepriseIndex(int IdDemande)
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

                    if (this.Cbo_Compteur.ItemsSource != null)
                        this.Cbo_Compteur.SelectedItem = ((List<CsEvenement>)this.Cbo_Compteur.ItemsSource).First(a => a.FK_IDCOMPTEUR == laDetailDemande.LstEvenement.First().FK_IDCOMPTEUR);
                    this.txtDemande.Text = laDetailDemande.LaDemande.NUMDEM;
                    this.txtMotifDemande.Text = laDetailDemande.LaDemande.MOTIF;
                    this.txtMotifRejet.Text = laDetailDemande.AnnotationDemande.First().COMMENTAIRE;
                    this.Txt_AncIndex.Text = laDetailDemande.LstEvenement.First().INDEXEVTPRECEDENT.ToString();
                    this.Txt_NouvIndex.Text = laDetailDemande.LstEvenement.First().INDEXEVT.ToString();

                    CsClient rech = new CsClient();

                    rech.FK_IDABON = laDetailDemande.LstEvenement.First().FK_IDABON;
                    rech.FK_IDPRODUIT = laDetailDemande.LstEvenement.First().FK_IDPRODUIT;
                    rech.FK_IDCENTRE = laDetailDemande.LstEvenement.First().FK_IDCENTRE;
                    rech.CENTRE = laDetailDemande.LstEvenement.First().CENTRE;
                    rech.REFCLIENT = laDetailDemande.LstEvenement.First().CLIENT;
                    rech.ORDRE = laDetailDemande.LstEvenement.First().ORDRE;
                    rech.PRODUIT = laDetailDemande.LstEvenement.First().PRODUIT;
                    rech.PERIODE = Shared.ClasseMEthodeGenerique.FormatPeriodeAAAAMM(this.Txt_PeriodeEnCour.Text);
                    RetourneEvenement(rech);

                }
            };
            client.ChargerDetailDemandeAsync (IdDemandeDevis,string.Empty );
        }








        private void RemplirTypeDemande()
        {
            try
            {
                if (SessionObject.LstTypeDemande == null || SessionObject.LstTypeDemande.Count == 0)
                {
                    ServiceAccueil.AcceuilServiceClient service1 = new ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
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
                    {
                        if (this.laDetailDemande != null)
                            MiseAjourDemande();
                        else
                            ValiderInitialisation();
                    }
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





        private void MiseAjourDemande()
        {
            try
            {


                ServiceAccueil.CsEvenement ev = new ServiceAccueil.CsEvenement();
                CsEvenement leVt = (CsEvenement)this.Cbo_Compteur.SelectedItem;

                ev.PK_ID = leVt.PK_ID;
                ev.INDEXEVT = int.Parse(this.Txt_NouvIndex.Text);
                ev.INDEXEVTPRECEDENT = int.Parse(this.Txt_AncIndex.Text);
                ev.USERMODIFICATION = UserConnecte.matricule;
                ev.NUMDEM = this.laDetailDemande.LaDemande.NUMDEM;
                ev.FK_IDDEMANDE = this.laDetailDemande.LaDemande.PK_ID;


                this.laDetailDemande.LstEvenement.Clear();
                this.laDetailDemande.LstEvenement.Add(ev);
                this.laDetailDemande.LaDemande.ORDRE = txtOrdre.Text;
                this.laDetailDemande.LaDemande.PRODUIT = leVt.PRODUIT;
                this.laDetailDemande.LaDemande.FK_IDPRODUIT = leVt.FK_IDPRODUIT;
                this.laDetailDemande.LaDemande.CLIENT = leVt.CLIENT;
                this.laDetailDemande.LaDemande.SITE = Txt_CodeSite.Text;
                this.laDetailDemande.LaDemande.CENTRE = leVt.CENTRE;
                this.laDetailDemande.LaDemande.FK_IDCENTRE = leVt.FK_IDCENTRE;
                this.laDetailDemande.LaDemande.DATEMODIFICATION = DateTime.Now;
                this.laDetailDemande.LaDemande.USERMODIFICATION = UserConnecte.matricule;
                this.laDetailDemande.LaDemande.MOTIF = txtMotifDemande.Text;
                this.laDetailDemande.LaDemande.ISMETREAFAIRE = false;
                this.laDetailDemande.LaDemande.TYPEDEMANDE = SessionObject.LstTypeDemande.First(a => a.CODE == SessionObject.Enumere.ReprisIndex).CODE;
                this.laDetailDemande.LaDemande.FK_IDTYPEDEMANDE = SessionObject.LstTypeDemande.First(a => a.CODE == SessionObject.Enumere.ReprisIndex).PK_ID;



                ServiceAccueil.AcceuilServiceClient clientDevis = new ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                clientDevis.ValidationDemandeCompleted += (ss, b) =>
                {
                    if (b.Cancelled || b.Error != null)
                    {
                        string error = b.Error.Message;
                        Message.ShowError(error, "Reprise d'index");
                        return;
                    }
                    if (string.IsNullOrEmpty(b.Result))
                    {
                        Message.ShowInformation("Demande transmise avec succès", "Reprise d'index");
                        this.DialogResult = true;
                    }
                    else
                        Message.ShowError(b.Result, "Reprise d'index");
                };
                clientDevis.ValidationDemandeAsync(laDetailDemande, true);
            }
            catch (Exception ex)
            {
                Message.ShowError("Une erreur s'est produite à la validation ", "Reprise d'index");
            }
        }

        private void ValiderInitialisation()
        {
            ServiceAccueil.CsDemande demandedevis = null;
            try
            {
                demandedevis = GetDemandeDevisFromScreen(demandedevis);
                if (demandedevis != null)
                {
                    demandedevis.LaDemande.MATRICULE = UserConnecte.matricule;

                    Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient client = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                    client.CreeDemandeCompleted += (ss, b) =>
                    {
                        if (b.Cancelled || b.Error != null)
                        {
                            string error = b.Error.Message;
                            Message.ShowError(error, "Reprise d'index");
                            return;
                        }
                        if (b.Result != null)
                        {
                            this.DialogResult = true;
                            Message.ShowInformation("La demande a été créée avec succès. Numéro de votre demande : " + b.Result.NUMDEM, "Reprise d'index");
                        }
                        else
                            Message.ShowInformation("Problème lors de la mise à jour de la demande", this.Title.ToString());

                    };
                    client.CreeDemandeAsync(demandedevis, true);
                }
            }
            catch (Exception ex)
            {
                Message.ShowError("Une erreur s'est produite à la validation ", "Reprise d'index");
            }
        }

        private ServiceAccueil.CsDemande GetDemandeDevisFromScreen(ServiceAccueil.CsDemande pDemandeDevis)
        {
            try
            {
                if (pDemandeDevis == null)
                {
                    pDemandeDevis = new ServiceAccueil.CsDemande();
                    pDemandeDevis.LaDemande = new ServiceAccueil.CsDemandeBase();
                    pDemandeDevis.Abonne = new ServiceAccueil.CsAbon();
                    pDemandeDevis.Ag = new ServiceAccueil.CsAg();
                    pDemandeDevis.Branchement = new ServiceAccueil.CsBrt();
                    pDemandeDevis.LeClient = new ServiceAccueil.CsClient();
                    pDemandeDevis.ObjetScanne = new List<ServiceAccueil.ObjDOCUMENTSCANNE>();
                    pDemandeDevis.AppareilDevis = new List<ServiceAccueil.ObjAPPAREILSDEVIS>();
                    pDemandeDevis.LstEvenement = new List<ServiceAccueil.CsEvenement>();
                    pDemandeDevis.LaDemande.DATECREATION = DateTime.Now;
                    pDemandeDevis.LaDemande.USERCREATION = UserConnecte.matricule;
                    pDemandeDevis.LaDemande.FK_IDADMUTILISATEUR = UserConnecte.PK_ID;
                }
                #region Demande

                if (this.Cbo_Compteur.SelectedItem != null && !string.IsNullOrEmpty(Txt_NouvIndex.Text))
                {
                    ServiceAccueil.CsEvenement ev = new ServiceAccueil.CsEvenement();
                    CsEvenement leVt = (CsEvenement)this.Cbo_Compteur.SelectedItem;

                    ev.PK_ID = leVt.PK_ID;
                    ev.INDEXEVT = int.Parse(this.Txt_NouvIndex.Text);
                    ev.INDEXEVTPRECEDENT = int.Parse(this.Txt_AncIndex.Text);
                    ev.USERCREATION = UserConnecte.matricule;

                    pDemandeDevis.LaDemande.ISNEW = true;
                    pDemandeDevis.LstEvenement.Add(ev);
                    pDemandeDevis.LaDemande.ORDRE = txtOrdre.Text;
                    pDemandeDevis.LaDemande.PRODUIT = leVt.PRODUIT;
                    pDemandeDevis.LaDemande.FK_IDPRODUIT = leVt.FK_IDPRODUIT;
                    pDemandeDevis.LaDemande.CLIENT = leVt.CLIENT;
                    pDemandeDevis.LaDemande.SITE = Txt_CodeSite.Text;
                    pDemandeDevis.LaDemande.CENTRE = leVt.CENTRE;
                    pDemandeDevis.LaDemande.FK_IDCENTRE = leVt.FK_IDCENTRE;
                    pDemandeDevis.LaDemande.DATECREATION = DateTime.Now;
                    pDemandeDevis.LaDemande.USERCREATION = UserConnecte.matricule;
                    pDemandeDevis.LaDemande.MOTIF = txtMotifDemande.Text;
                    pDemandeDevis.LaDemande.ISMETREAFAIRE = false;
                    pDemandeDevis.LaDemande.TYPEDEMANDE = SessionObject.LstTypeDemande.First(a => a.CODE == SessionObject.Enumere.ReprisIndex).CODE;
                    pDemandeDevis.LaDemande.FK_IDTYPEDEMANDE = SessionObject.LstTypeDemande.First(a => a.CODE == SessionObject.Enumere.ReprisIndex).PK_ID;
                }

                #endregion


                return pDemandeDevis;
            }
            catch (Exception ex)
            {

                throw ex;
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
                        //this.btn_Site.IsEnabled = false;
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
                            this.txtClient.IsReadOnly = false;
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
                        //this.btn_Site.IsEnabled = false;
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
                            this.txtClient.IsReadOnly = false;
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
                    //this.btn_Site.IsEnabled = false;
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
                Message.ShowError(ex.Message, "Facturation");
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
            //this.btn_Site.IsEnabled = true;
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
                Message.ShowError(ex.Message, "Facturation");
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
                this.txtClient.IsReadOnly = false;
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
                Message.ShowError(ex.Message, "Facturation");

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
                            this.txtClient.IsReadOnly = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, "Facturation");

            }
        }
        private void Txt_PeriodeEnCour_TextChanged(object sender, TextChangedEventArgs e)
        {

            if (this.Txt_PeriodeEnCour.Text.Length == 7)
                if (!Galatee.Silverlight.Shared.ClasseMEthodeGenerique.IsFormatPeriodeValide(Txt_PeriodeEnCour.Text))
                    Message.ShowInformation("Le format de la période n'est pas valide", "Facturation");
        }

        private void Btn_Recherche_Click(object sender, RoutedEventArgs e)
        {
            this.Txt_NouvIndex.Text = string.Empty;
            this.Txt_AncIndex.Text = string.Empty;
            this.Cbo_Compteur.SelectedItem  = null;
            this.Txt_NomAbon.Text = string.Empty;

            if (this.Txt_CodeCentre.Tag != null && !string.IsNullOrEmpty( this.Txt_PeriodeEnCour.Text) )
            {
                int idcentre = int.Parse(this.Txt_CodeCentre.Tag.ToString());
                ServiceAccueil.CsClient leClient = new ServiceAccueil.CsClient();
                leClient.CENTRE = Txt_CodeCentre.Text ;
                leClient.REFCLIENT = this.txtClient.Text;
                leClient.PRODUIT  = this.Txt_Produit.Tag.ToString() ;

                leClient.FK_IDCENTRE = idcentre;
                string OrdreMax = string.Empty;
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.RetourneOrdreMaxCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    OrdreMax = args.Result;
                    if (OrdreMax != null)
                    {
                        leClient.ORDRE = OrdreMax;
                        this.txtOrdre.Text = OrdreMax;
                        //RetourneInfoClient(leClient);
                        VerifieExisteDemande(leClient);
                    }
                    else
                    {
                        Message.ShowInformation("Abonnement non trouvé", "Facturation");
                        return;
                    }
                };
                service.RetourneOrdreMaxAsync(leClient.FK_IDCENTRE.Value, leClient.CENTRE, leClient.REFCLIENT, leClient.PRODUIT);
                service.CloseAsync();

            }
            else
            {
                Message.ShowInformation("Saisir la période à modifier", "Facturation");
                return;
            }
        }


        private void VerifieExisteDemande(ServiceAccueil.CsClient leClient)
        {

            try
            {
                if (!string.IsNullOrEmpty(leClient.REFCLIENT) && leClient.REFCLIENT.Length == SessionObject.Enumere.TailleClient)
                {
                    leClient.PERIODE = Shared.ClasseMEthodeGenerique.FormatPeriodeAAAAMM(this.Txt_PeriodeEnCour.Text);
                    leClient.TYPEDEMANDE = SessionObject.Enumere.ReprisIndex;

                    ServiceAccueil.AcceuilServiceClient service = new ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                    service.RetourneDemandeClientTypeCompleted += (s, args) =>
                    {
                        if (args != null && args.Cancelled)
                            return;
                        if (args.Result != null)
                        {
                            if (args.Result.DATEFIN == null && args.Result.ISSUPPRIME != true)
                            {
                                Message.ShowInformation("Il existe une demande numéro " + args.Result.NUMDEM + " sur ce client", "Accueil");
                                return;
                            }
                        }

                        int idcentre = int.Parse(this.Txt_CodeCentre.Tag.ToString());
                        CsClient leClientRecherche = new CsClient();
                        leClientRecherche.CENTRE = Txt_CodeCentre.Text;
                        leClientRecherche.REFCLIENT = this.txtClient.Text;
                        leClientRecherche.PRODUIT = this.Txt_Produit.Tag.ToString();

                        leClientRecherche.FK_IDCENTRE = idcentre;
                        leClientRecherche.ORDRE = leClient.ORDRE;


                        RetourneInfoClient(leClientRecherche);
                    };
                    service.RetourneDemandeClientTypeAsync(leClient);
                    service.CloseAsync();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }



        private void RetourneEvenement(CsClient leClient)
        {
            try
            {
                FacturationServiceClient ClientSrv = new FacturationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Facturation"));
                ClientSrv.RetourneEvenementCorrectionIndexCompleted += (e, argsss) =>
                {
                    if (argsss != null && argsss.Cancelled)
                        return;
                    List<CsEvenement> Res = argsss.Result;

                    this.Cbo_Compteur.ItemsSource = null;
                    this.Cbo_Compteur.ItemsSource = Res;
                    this.Cbo_Compteur.DisplayMemberPath = "COMPTEUR";
                    if (Res.Count == 1)
                        this.Cbo_Compteur.SelectedItem = Res.First();
                };
                ClientSrv.RetourneEvenementCorrectionIndexAsync(leClient);
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        
        }


       CsAbon leAbonnement = new CsAbon();
       private void RetourneInfoClient(CsClient leClientRech)
       {
           FacturationServiceClient service = new FacturationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Facturation"));
           service.RetourneAbonCompleted  += (s, args) =>
           {
               if (args != null && args.Cancelled)
                   return;
               if (args.Result == null)
               {
                   Message.ShowError(Langue.Msg_AbonnemtInexistant, Galatee.Silverlight.Resources.Facturation.Langue.LibelleModule);
                   return;
               }
               if ( args.Result != null && args.Result.Count== 1)
                    leAbonnement = args.Result.First();
               this.Txt_NomAbon.Text = leAbonnement.NOMABON;
               leClientRech.FK_IDABON = leAbonnement.PK_ID;
               leClientRech.FK_IDPRODUIT = leAbonnement.FK_IDPRODUIT;
               leClientRech.FK_IDCENTRE = leAbonnement.FK_IDCENTRE;
               leClientRech.PERIODE =Shared.ClasseMEthodeGenerique.FormatPeriodeAAAAMM( this.Txt_PeriodeEnCour.Text) ;
               if (leAbonnement != null && !string.IsNullOrEmpty(leAbonnement.CENTRE))
                   RetourneEvenement(leClientRech);
           };
           service.RetourneAbonAsync(leClientRech.FK_IDCENTRE.Value , leClientRech.CENTRE, leClientRech.REFCLIENT, leClientRech.ORDRE);
           service.CloseAsync();
       
       }
        private void Cbo_Compteur_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.Cbo_Compteur.SelectedItem != null)
            {
                CsEvenement leEvtSelect = (CsEvenement)this.Cbo_Compteur.SelectedItem;
                this.Txt_AncIndex.Text = leEvtSelect.INDEXPRECEDENTEFACTURE .ToString();
            }

            this.OKButton.IsEnabled = (this.Cbo_Compteur.SelectedItem != null) && !string.IsNullOrEmpty(this.txtMotifDemande.Text) && !string.IsNullOrEmpty(this.Txt_NouvIndex.Text);

        }



        private void txtMotifDemande_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.OKButton.IsEnabled = (this.Cbo_Compteur.SelectedItem != null) && !string.IsNullOrEmpty(this.txtMotifDemande.Text) && !string.IsNullOrEmpty(this.Txt_NouvIndex.Text);
        }

        private void Txt_NouvIndex_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.OKButton.IsEnabled = (this.Cbo_Compteur.SelectedItem != null) && !string.IsNullOrEmpty(this.txtMotifDemande.Text) && !string.IsNullOrEmpty(this.Txt_NouvIndex.Text);
        }

     

    }
}

