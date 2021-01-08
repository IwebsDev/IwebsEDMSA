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
using Galatee.Silverlight.ServiceAccueil   ;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading;
using Galatee.Silverlight.Resources.Devis;
using Galatee.Silverlight.Library;
using Galatee.Silverlight.Shared;

namespace Galatee.Silverlight.Accueil
{
    public partial class FrmAutreDemande : ChildWindow
    {
        public Galatee.Silverlight.SessionObject.ExecMode ExecMode {get;set;}
        CsDemandeBase laDemandeSelect = null;
        bool isPreuveSelectionnee = false;
        private UcImageScanne formScanne = null;
        public ObservableCollection<CsTypeDOCUMENTSCANNE> LstTypeDocument = new ObservableCollection<CsTypeDOCUMENTSCANNE>();
        CsDemande laDetailDemande = null;
        private List<CsCentre> _listeDesCentreExistant = null;
        private List<CsReglageCompteur> _listeDesReglageCompteurExistant = null;

        public FrmAutreDemande()
        {
            InitializeComponent();

            ctrl = new Galatee.Silverlight.Shared.UcFichierJoint(null, false);
            Vwb.Stretch = Stretch.None;
            Vwb.Child = ctrl;


            this.dtp_RendezVousPrev.SelectedDate = System.DateTime.Today.AddDays(15);

            prgBar.Visibility = System.Windows.Visibility.Collapsed;
            ChargerPuissanceInstalle();
            this.Txt_ReferenceClient.MaxLength = SessionObject.Enumere.TailleClient ;
            prgBar.Visibility = System.Windows.Visibility.Collapsed;
            this.Chk_Metre.IsChecked = true;
            if (Tdem != SessionObject.Enumere.RemboursementAvance)
            {
                this.Txt_Ordre.Visibility = System.Windows.Visibility.Collapsed;
                this.lbl_Ordre.Visibility = System.Windows.Visibility.Collapsed;
            }
            RemplirListeDesReglageExistant();
            
            ChargerListDesSite();
            AfficherOuMasquer(tabItemCompte, false);
        }
        string Tdem = string.Empty;
        public FrmAutreDemande(string TypeDemande,string Init)
        {
            InitializeComponent();

            ctrl = new Galatee.Silverlight.Shared.UcFichierJoint(null, false);
            Vwb.Stretch = Stretch.None;
            Vwb.Child = ctrl;

            this.dtp_RendezVousPrev.SelectedDate = System.DateTime.Today.AddDays(15);

            ChargerListDesSite();
            ChargerPuissanceInstalle();
            RemplirListeDesReglageExistant();

            this.Txt_ReferenceClient.MaxLength = SessionObject.Enumere.TailleClient;
            prgBar.Visibility = System.Windows.Visibility.Collapsed;

            lbl_ancienPuiss_Copy.Visibility = System.Windows.Visibility.Collapsed;
            libeNPI.Visibility = System.Windows.Visibility.Collapsed;
            txt_AncPuissanceInstalle.Visibility = System.Windows.Visibility.Collapsed;
            Cbo_PuissanceInstalle .Visibility = System.Windows.Visibility.Collapsed;
            Tdem = TypeDemande;
            this.txt_Reglage.Visibility = System.Windows.Visibility.Collapsed;
            this.label21.Visibility = System.Windows.Visibility.Collapsed;
            this.Btn_Reglage.Visibility = System.Windows.Visibility.Collapsed;

            if (Tdem != SessionObject.Enumere.RemboursementAvance)
            {
                this.Txt_Ordre.Visibility = System.Windows.Visibility.Collapsed;
                this.lbl_Ordre.Visibility = System.Windows.Visibility.Collapsed;
            }
            if (Tdem == SessionObject.Enumere.ChangementProduit )
            {
                this.txt_Reglage.Visibility = System.Windows.Visibility.Visible ;
                this.label21.Visibility = System.Windows.Visibility.Visible;
                this.Btn_Reglage.Visibility = System.Windows.Visibility.Visible;
                AfficherOuMasquer(tabItemAbon, false);

            }
            
            AfficherOuMasquer(tabItemCompte, false);
            AfficherOuMasquer(tab_demande, false);
        }
        bool IsRejeterDemande = false;

        Galatee.Silverlight.Shared.UcFichierJoint ctrl = null;

        public FrmAutreDemande(int idDevis)
        {
            InitializeComponent();

            ctrl = new Galatee.Silverlight.Shared.UcFichierJoint(null, false);
            Vwb.Stretch = Stretch.None;
            Vwb.Child = ctrl;

            this.dtp_RendezVousPrev.SelectedDate = System.DateTime.Today.AddDays(15);

            AfficherOuMasquer(tabItemCompte, false);
            AfficherOuMasquer(tab_demande, false);
            this.btn_RechercheClient.IsEnabled = false;
            RemplirListeDesReglageExistant();
            ChargeDetailDEvis(idDevis);

        }
        
        private void ChargeDetailDEvis(int IdDemandeDevis)
        {

            AcceuilServiceClient client = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
            client.ChargerDetailDemandeAsync(IdDemandeDevis,string.Empty );
            client.ChargerDetailDemandeCompleted += (ssender, args) =>
            {
                if (args.Cancelled || args.Error != null)
                {
                    LayoutRoot.Cursor = Cursors.Arrow;
                    string error = args.Error.Message;
                    Message.ShowError(error, Silverlight.Resources.Devis.Languages.txtDevis);
                    return;
                }
                if (args.Result == null)
                {
                    LayoutRoot.Cursor = Cursors.Arrow;
                    Message.ShowError(Silverlight.Resources.Devis.Languages.AucunesDonneesTrouvees, Silverlight.Resources.Devis.Languages.txtDevis);
                    return;
                }
                else
                {
                    laDetailDemande = new CsDemande();
                    laDetailDemande = args.Result;
                    this.txtSite.Text = string.IsNullOrEmpty(laDetailDemande.LaDemande.LIBELLESITE) ? string.Empty : laDetailDemande.LaDemande.LIBELLESITE;
                    this.txtCentre.Text = string.IsNullOrEmpty(laDetailDemande.LaDemande.LIBELLECENTRE) ? string.Empty : laDetailDemande.LaDemande.LIBELLECENTRE;
                    this.Txt_ReferenceClient.Text = string.IsNullOrEmpty(laDetailDemande.LaDemande.CLIENT) ? string.Empty : laDetailDemande.LaDemande.CLIENT;
                    this.txt_Produit.Text = string.IsNullOrEmpty(laDetailDemande.LaDemande.LIBELLEPRODUIT) ? string.Empty : laDetailDemande.LaDemande.LIBELLEPRODUIT;
                    this.txt_Produit.Tag = laDetailDemande.LaDemande.FK_IDPRODUIT;
                    this.txt_tdem.Text = string.IsNullOrEmpty(laDetailDemande.LaDemande.LIBELLETYPEDEMANDE) ? string.Empty : laDetailDemande.LaDemande.LIBELLETYPEDEMANDE;
                    Tdem = string.IsNullOrEmpty(laDetailDemande.LaDemande.TYPEDEMANDE) ? string.Empty : laDetailDemande.LaDemande.TYPEDEMANDE;
                    this.txt_tdem.Tag = SessionObject.LstTypeDemande.FirstOrDefault(t => t.CODE == Tdem);
                    this.txt_Motif.Text = string.IsNullOrEmpty(laDetailDemande.LaDemande.MOTIF) ? string.Empty : laDetailDemande.LaDemande.MOTIF;

                    this.txtSite.IsReadOnly = true;
                    this.txtCentre.IsReadOnly = true;
                    this.Txt_ReferenceClient.IsReadOnly = true;
                    this.txt_Produit.IsReadOnly = true;
                    this.txt_tdem.IsReadOnly = true;

                    RemplireOngletClient(laDetailDemande.LeClient);
                    RemplirOngletAbonnement(laDetailDemande.Abonne);
                    RemplireOngletFacture(laDetailDemande.LstCoutDemande);
                    RemplireOngletObjetScanne(laDetailDemande.ObjetScanne);
                    if (((CsTdem)this.txt_tdem.Tag).CODE == SessionObject.Enumere.AugmentationPuissance ||
                        ((CsTdem)this.txt_tdem.Tag).CODE == SessionObject.Enumere.DimunitionPuissance)
                    {
                        AfficherOuMasquer(tab_demande, true);
                        txt_ancienPuiss.Text = laDetailDemande.Abonne.PUISSANCE.Value.ToString(SessionObject.FormatMontant);
                        this.Cbo_PuissanceSouscrite.ItemsSource = null;
                        this.Cbo_PuissanceSouscrite.DisplayMemberPath = "VALEUR";
                        this.Cbo_PuissanceSouscrite.ItemsSource = SessionObject.LstPuissance.Where(t => t.PRODUIT == laDetailDemande.Abonne.PRODUIT).ToList();

                        if (laDetailDemande.LaDemande.PRODUIT == SessionObject.Enumere.ElectriciteMT)
                        {
                            lbl_ancienPuiss_Copy.Visibility = System.Windows.Visibility.Visible ;
                            libeNPI.Visibility = System.Windows.Visibility.Visible;
                            txt_AncPuissanceInstalle.Visibility = System.Windows.Visibility.Visible;
                            Cbo_PuissanceInstalle.Visibility = System.Windows.Visibility.Visible;
                        }

                    }
                    IsRejeterDemande = true;
                }
                
            };
        }



        private void AfficherOuMasquer(TabItem pTabItem, bool pValue)
        {
            try
            {
                pTabItem.Visibility = pValue ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        List<ServiceAccueil.CsCentre> lesCentre = new List<ServiceAccueil.CsCentre>();
        List<ServiceAccueil.CsSite> lesSite = new List<ServiceAccueil.CsSite>();
        List<int> lstIdCentre = new List<int>();
        void ChargerListDesSite()
        {
            try
            {

                if (SessionObject.LstCentre != null && SessionObject.LstCentre.Count != 0)
                {
                    lesCentre = Shared.ClasseMEthodeGenerique.RetourCentreByPerimetre(SessionObject.LstCentre, UserConnecte.listeProfilUser);
                    lesSite = Shared.ClasseMEthodeGenerique.RetourneSiteByCentre(lesCentre);
                    _listeDesCentreExistant = lesCentre;

                    foreach (ServiceAccueil.CsCentre item in lesCentre)
                        lstIdCentre.Add(item.PK_ID);
                    return;
                }
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.ListeDesDonneesDesSiteAsync(false);
                service.ListeDesDonneesDesSiteCompleted += (s, args) =>
                {
                    try
                    {
                        if (args != null && args.Cancelled)
                            return;
                        SessionObject.LstCentre = args.Result;
                        if (SessionObject.LstCentre.Count != 0)
                        {
                            lesCentre = Shared.ClasseMEthodeGenerique.RetourCentreByPerimetre(SessionObject.LstCentre, UserConnecte.listeProfilUser);
                            lesSite = Shared.ClasseMEthodeGenerique.RetourneSiteByCentre(lesCentre);
                            _listeDesCentreExistant = lesCentre;

                            foreach (ServiceAccueil.CsCentre item in lesCentre)
                                lstIdCentre.Add(item.PK_ID);

                        }
                        else
                        {
                            Message.ShowInformation("Aucun site trouvé en base.", "Erreur");
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        Message.ShowError(ex, "Erreur");
                    }
                };
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
            }
        }
        public ObservableCollection<ObjDOCUMENTSCANNE> LstPiece = new ObservableCollection<ObjDOCUMENTSCANNE>();
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.OKButton.IsEnabled = false;
                this.CancelButton.IsEnabled = false ;
                ValiderInitialisation(null, true);
            }
            catch (Exception ex)
            {
                this.DialogResult = false;
                Message.Show(ex.Message, Languages.txtDevis);
            }
        }
        private void ValiderInitialisation(CsDemande demandedevis, bool IsTransmetre)
        {

            try
            {
                if (!VerifieChampObligatioire()) return;

                demandedevis = GetDemandeDevisFromScreen(demandedevis, false);
                this.DialogResult = true;
                if (demandedevis != null)
                {
                    demandedevis.LaDemande.MATRICULE = UserConnecte.matricule;
                    if (Tdem == SessionObject.Enumere.DimunitionPuissance ||
                        Tdem == SessionObject.Enumere.AugmentationPuissance ||
                        Tdem == SessionObject.Enumere.Resiliation)
                    {
                        demandedevis.LeClient = laDetailDemande.LeClient;
                        demandedevis.LeClient.DATECREATION = System.DateTime.Now;
                        demandedevis.LeClient.USERCREATION = UserConnecte.matricule;

                        demandedevis.Abonne = laDetailDemande.Abonne;
                        demandedevis.Abonne.DATECREATION = System.DateTime.Now;
                        demandedevis.Abonne.USERCREATION = UserConnecte.matricule;

                        demandedevis.Branchement = laDetailDemande.Branchement;
                        demandedevis.Branchement.DATECREATION = System.DateTime.Now;
                        demandedevis.Branchement.USERCREATION = UserConnecte.matricule;

                        demandedevis.LstCanalistion = laDetailDemande.LstCanalistion;


                        if ((Tdem == SessionObject.Enumere.DimunitionPuissance ||
                        Tdem == SessionObject.Enumere.AugmentationPuissance) &&
                        demandedevis.LaDemande.PRODUIT == SessionObject.Enumere.ElectriciteMT && this.Cbo_PuissanceInstalle.SelectedItem != null)
                            demandedevis.Branchement.PUISSANCEINSTALLEE = ((CsPuissance)this.Cbo_PuissanceInstalle.SelectedItem).VALEUR;

                        demandedevis.Ag = laDetailDemande.Ag;
                        demandedevis.Ag.DATECREATION = System.DateTime.Now;
                        demandedevis.Ag.USERCREATION = UserConnecte.matricule;

                    }
                    if (Tdem == SessionObject.Enumere.Etalonage ||
                        Tdem == SessionObject.Enumere.VerificationCompteur ||
                        Tdem == SessionObject.Enumere.ChangementCompteur)
                    {
                        demandedevis.LeClient = laDetailDemande.LeClient;
                        demandedevis.LeClient.DATECREATION = System.DateTime.Now;
                        demandedevis.LeClient.USERCREATION = UserConnecte.matricule;

                        demandedevis.Branchement = laDetailDemande.Branchement;
                        demandedevis.Branchement.DATECREATION = System.DateTime.Now;
                        demandedevis.Branchement.USERCREATION = UserConnecte.matricule;

                        demandedevis.Abonne = laDetailDemande.Abonne;
                        demandedevis.Abonne.DATECREATION = System.DateTime.Now;
                        demandedevis.Abonne.USERCREATION = UserConnecte.matricule;

                        demandedevis.Ag = laDetailDemande.Ag;
                        demandedevis.Ag.DATECREATION = System.DateTime.Now;
                        demandedevis.Ag.USERCREATION = UserConnecte.matricule;

                        demandedevis.LaDemande.REGLAGECOMPTEUR = laDetailDemande.LstCanalistion.First().REGLAGECOMPTEUR;
                        demandedevis.LaDemande.FK_IDREGLAGECOMPTEUR = laDetailDemande.LstCanalistion.First().FK_IDREGLAGECOMPTEUR;
                    }
                    if (Tdem == SessionObject.Enumere.ChangementProduit)
                    {
                        //demandedevis.LaDemande.ORDRE = "01";
                        //demandedevis.LaDemande.CLIENT = string.Empty;
                        demandedevis.LaDemande.PRODUIT = lblProduit.Tag.ToString();
                        demandedevis.LaDemande.FK_IDPRODUIT = (int)this.txt_Produit.Tag;
                        demandedevis.LaDemande.FK_IDCLIENT = laDetailDemande.LeClient.PK_ID;
                        if (this.txt_Reglage.Tag != null)
                        {
                            demandedevis.LaDemande.FK_IDREGLAGECOMPTEUR = (this.txt_Reglage.Tag == null) ? null : (int?)this.txt_Reglage.Tag;
                            demandedevis.LaDemande.REGLAGECOMPTEUR = this.Btn_Reglage.Tag == null ? string.Empty : this.Btn_Reglage.Tag.ToString();
                        }

                        demandedevis.Ag = laDetailDemande.Ag;
                        demandedevis.Ag.DATECREATION = System.DateTime.Now;
                        demandedevis.Ag.USERCREATION = UserConnecte.matricule;
                        //demandedevis.Ag.DATEMODIFICATION = null;
                        //demandedevis.Ag.USERMODIFICATION = null;
                        //demandedevis.Ag.CLIENT = string.Empty;

                        //demandedevis.LeClient = laDetailDemande.LeClient;
                        //demandedevis.LeClient.DATECREATION = System.DateTime.Now;
                        //demandedevis.LeClient.USERCREATION = UserConnecte.matricule;
                        //demandedevis.LeClient.DATEMODIFICATION = null;
                        //demandedevis.LeClient.USERMODIFICATION = null;
                        //demandedevis.LeClient.REFCLIENT = string.Empty;
                        //demandedevis.LeClient.ORDRE = "01";


                        demandedevis.LeClient = laDetailDemande.LeClient;
                        demandedevis.LeClient.DATECREATION = System.DateTime.Now;
                        demandedevis.LeClient.USERCREATION = UserConnecte.matricule;

                        demandedevis.Abonne = laDetailDemande.Abonne;
                        demandedevis.Abonne.DATECREATION = System.DateTime.Now;
                        demandedevis.Abonne.USERCREATION = UserConnecte.matricule;
                        demandedevis.Abonne.PRODUIT = demandedevis.LaDemande.PRODUIT;
                        demandedevis.Abonne.FK_IDPRODUIT = int.Parse(demandedevis.LaDemande.FK_IDPRODUIT.ToString());

                        demandedevis.Branchement = laDetailDemande.Branchement;
                        demandedevis.Branchement.DATECREATION = System.DateTime.Now;
                        demandedevis.Branchement.USERCREATION = UserConnecte.matricule;
                        demandedevis.Branchement.PRODUIT = demandedevis.LaDemande.PRODUIT;
                        demandedevis.Branchement.FK_IDPRODUIT = int.Parse(demandedevis.LaDemande.FK_IDPRODUIT.ToString());

                        demandedevis.LstCanalistion = laDetailDemande.LstCanalistion;



                    }

                    demandedevis.LaDemande.DPRDEV = this.dtp_RendezVousPrev.SelectedDate;


                    Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient client = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                    client.CreeDemandeCompleted += (ss, b) =>
                    {
                        if (b.Cancelled || b.Error != null)
                        {
                            string error = b.Error.Message;
                            Message.ShowError(error, Silverlight.Resources.Devis.Languages.txtDevis);
                            return;
                        }
                        if (b.Result != null)
                        {
                            List<CsDemandeBase> leDemandeAEditer = new List<CsDemandeBase>();
                            demandedevis.LaDemande.NOMCLIENT = demandedevis.LeClient.NOMABON;
                            demandedevis.LaDemande.LIBELLETYPEDEMANDE = txt_tdem.Text;
                            demandedevis.LaDemande.NUMDEM = b.Result.NUMDEM; ;
                            demandedevis.LaDemande.LIBELLEPRODUIT = this.txt_Produit.Text;
                            demandedevis.LaDemande.MOTIF = this.txt_Motif.Text;
                            demandedevis.LaDemande.LIBELLE = "Imprimé le " + DateTime.Now + " sur le poste " + SessionObject.LePosteCourant.NOMPOSTE + " par " + UserConnecte.nomUtilisateur + "(" + UserConnecte.matricule + ") du centre " + UserConnecte.LibelleCentre;
                            leDemandeAEditer.Add(demandedevis.LaDemande);
                            Utility.ActionDirectOrientation<ServicePrintings.CsDemandeBase, CsDemandeBase>(leDemandeAEditer, null, SessionObject.CheminImpression, "AccuseRecption", "Accueil", true);

                            Message.ShowInformation("La demande a été créée avec succès. Numéro de votre demande : " + b.Result.NUMDEM,
                            Silverlight.Resources.Devis.Languages.txtDevis);
                        }
                    };
                    client.CreeDemandeAsync(demandedevis, IsTransmetre);
                }
            }
            catch (Exception ex)
            {
                Message.ShowError("Une erreur s'est produite à la validation ", "ValiderDemandeInitailisation");
            }
        }

        private CsDemande GetDemandeDevisFromScreen(CsDemande pDemandeDevis, bool isTransmettre)
        {
            try
            {
                if (pDemandeDevis == null)
                {
                    pDemandeDevis = new CsDemande();
                    pDemandeDevis.LaDemande = new CsDemandeBase();
                    pDemandeDevis.Abonne = new CsAbon();
                    pDemandeDevis.Ag = new CsAg();
                    pDemandeDevis.Branchement = new CsBrt();
                    pDemandeDevis.LeClient = new CsClient();
                    pDemandeDevis.ObjetScanne = new List<ObjDOCUMENTSCANNE>();
                    pDemandeDevis.AppareilDevis = new List<ObjAPPAREILSDEVIS>();
                    pDemandeDevis.LaDemande.DATECREATION = DateTime.Now;
                    pDemandeDevis.LaDemande.USERCREATION = UserConnecte.matricule;
                    pDemandeDevis.LaDemande.FK_IDADMUTILISATEUR = UserConnecte.PK_ID;
                }
                #region Demande

                    pDemandeDevis.LaDemande.ISNEW = true;
                    pDemandeDevis.LaDemande.ORDRE = laDetailDemande.LeClient.ORDRE;
                    pDemandeDevis.LaDemande.PRODUIT = laDetailDemande.Abonne.PRODUIT;
                    pDemandeDevis.LaDemande.FK_IDPRODUIT = laDetailDemande.Abonne.FK_IDPRODUIT;
                    pDemandeDevis.LaDemande.CLIENT  = laDetailDemande.Abonne.CLIENT ;
                    pDemandeDevis.LaDemande.CENTRE = laDetailDemande.Abonne.CENTRE;
                    pDemandeDevis.LaDemande.FK_IDCENTRE = laDetailDemande.Abonne.FK_IDCENTRE;
                    pDemandeDevis.LaDemande.DATEMODIFICATION = DateTime.Now;
                    pDemandeDevis.LaDemande.MOTIF = txt_Motif.Text;
                    pDemandeDevis.LaDemande.ISMETREAFAIRE = this.Chk_Metre.IsChecked == true ? false  : true ;

                if (txt_tdem.Tag != null)
                {
                    var typeDevis = (CsTdem)txt_tdem.Tag;
                    if (typeDevis != null)
                    {
                        pDemandeDevis.LaDemande.FK_IDTYPEDEMANDE = typeDevis.PK_ID;
                        pDemandeDevis.LaDemande.TYPEDEMANDE = typeDevis.CODE;
                        Tdem = typeDevis.CODE;
                    }
                }
                #region Doc Scanne
                if (pDemandeDevis.ObjetScanne == null) laDetailDemande.ObjetScanne = new List<ObjDOCUMENTSCANNE>();
                if (ctrl != null && ctrl.LstPiece != null)
                {
                    pDemandeDevis.ObjetScanne.Clear();
                    pDemandeDevis.ObjetScanne.AddRange(ctrl.LstPiece.Where(i => i.ISNEW == true || i.ISTOREMOVE == true));
                }
                #endregion
                #region AP/DP
                if ((Tdem == SessionObject.Enumere.AugmentationPuissance || Tdem == SessionObject.Enumere.DimunitionPuissance ))
                {
                    pDemandeDevis.Abonne.ISAUGMENTATIONPUISSANCE = Tdem == SessionObject.Enumere.AugmentationPuissance ? true : false ;
                    pDemandeDevis.Abonne.ISDIMINUTIONPUISSANCE = Tdem == SessionObject.Enumere.DimunitionPuissance ? true : false ;
                    decimal NOUVELLEPUISSANCE = 0;
                    decimal ANCIENNEPUISSANCE = 0;
                    if (pDemandeDevis.LaDemande.PRODUIT != SessionObject.Enumere.ElectriciteMT)
                    {
                        if (this.Cbo_diametre.SelectedItem != null)
                        {
                            pDemandeDevis.LaDemande.REGLAGECOMPTEUR = ((CsReglageCompteur)Cbo_diametre.SelectedItem).CODE;
                            pDemandeDevis.LaDemande.FK_IDREGLAGECOMPTEUR  = ((CsReglageCompteur)Cbo_diametre.SelectedItem).PK_ID ;
                        }
                        else
                        {
                            Message.ShowError(" Selectionner le reglage", "Information demande");
                            return null;
                        }
                    }
                    if (Cbo_PuissanceSouscrite.SelectedItem != null )
                    {
                        NOUVELLEPUISSANCE = ((CsPuissance)Cbo_PuissanceSouscrite.SelectedItem).VALEUR; 

                        decimal.TryParse(txt_ancienPuiss.Text, out ANCIENNEPUISSANCE);

                        if (pDemandeDevis.Abonne.ISAUGMENTATIONPUISSANCE)
                        {
                            if (NOUVELLEPUISSANCE < ANCIENNEPUISSANCE)
                            {
                                Message.ShowError(" La nouvelle puissance doit etre supérieur à l'ancien ", "Information demande");
                                return null;
                            }
                        }
                        if (pDemandeDevis.Abonne.ISDIMINUTIONPUISSANCE)
                        {
                            if (ANCIENNEPUISSANCE < NOUVELLEPUISSANCE)
                            {
                                Message.ShowError(" L'ancien puissance doit etre supérieur à nouvel ", "Information demande");
                                return null;
                            }
                        }
                        pDemandeDevis.LaDemande.ANCIENNEPUISSANCE  = laDetailDemande.Abonne.PUISSANCE  ;
                        pDemandeDevis.Abonne.NOUVELLEPUISSANCE = NOUVELLEPUISSANCE;
                        pDemandeDevis.LaDemande.PUISSANCESOUSCRITE = NOUVELLEPUISSANCE;
                    }
                    if (Cbo_PuissanceInstalle.SelectedItem != null)
                    {
                        NOUVELLEPUISSANCE = 0;
                        ANCIENNEPUISSANCE = 0;

                        NOUVELLEPUISSANCE = ((CsPuissance)Cbo_PuissanceInstalle.SelectedItem).VALEUR;
                        decimal.TryParse(txt_AncPuissanceInstalle.Text, out ANCIENNEPUISSANCE);
                        pDemandeDevis.Branchement.PUISSANCEINSTALLEE  = NOUVELLEPUISSANCE;
                        pDemandeDevis.LaDemande.PUISSANCESOUSCRITE =decimal.Parse( txt_ancienPuiss.Text);
                    }
                    if (Cbo_PuissanceSouscrite.SelectedItem == null && Cbo_PuissanceInstalle.SelectedItem == null)
                    {
                        Message.ShowError("Veuillez sélectionner la puissance", "Information demande");
                        return null;
                    }
                }

                if (((CsTdem)this.txt_tdem.Tag).CODE == SessionObject.Enumere.ChangementCompteur)
                {
                    pDemandeDevis.LaDemande.REGLAGECOMPTEUR = laDetailDemande.LstCanalistion.First().REGLAGECOMPTEUR;
                    pDemandeDevis.LaDemande.FK_IDREGLAGECOMPTEUR  = laDetailDemande.LstCanalistion.First().FK_IDREGLAGECOMPTEUR ;
                }
                #endregion
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
        void Translate()
        {

        }
        private bool VerifieChampObligatioire()
        {
            try
            {
                if (string.IsNullOrEmpty(this.txtCentre.Text))
                        throw new Exception("Saisir le client");
                 return true;
            }
            catch (Exception ex)
            {
                this.OKButton.IsEnabled = true ;
                this.CancelButton.IsEnabled = true ;
		        Message.ShowInformation(ex.Message ,"Accueil");
                return false;
            }
        }

        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            
        }
        private void Cbo_Centre_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void btn_transmetre_Click(object sender, RoutedEventArgs e)
        {
        }
        private void VerifieExisteDemande(CsClient leClient)
        {

            try
            {
                if (!string.IsNullOrEmpty(Txt_ReferenceClient.Text) && Txt_ReferenceClient.Text.Length == SessionObject.Enumere.TailleClient)
                {
                    string OrdreMax = string.Empty;
                    AcceuilServiceClient service = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                    service.RetourneDemandeClientTypeCompleted += (s, args) =>
                    {
                        if (args != null && args.Cancelled)
                            return;
                        if (args.Result != null)
                        {
                            if (args.Result.DATEFIN  == null  && args.Result.ISSUPPRIME != true)
                            {
                                Message.ShowInformation ("Il existe une demande numero " + args.Result.NUMDEM + " sur ce client", "Accueil");
                                prgBar.Visibility = System.Windows.Visibility.Collapsed;
                                return;
                            }
                        }
                        ChargeDetailDEvis(leClient);
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
        private void btn_RechercheClient_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                prgBar.Visibility = System.Windows.Visibility.Visible;
                
                if (Txt_ReferenceClient.Text.Length == SessionObject.Enumere.TailleClient && 
                    Tdem != SessionObject.Enumere.RemboursementAvance )
                    ChargerClientFromReference(this.Txt_ReferenceClient.Text);
                else
                {
                    if (Txt_ReferenceClient.Text.Length == SessionObject.Enumere.TailleClient &&
                        Tdem == SessionObject.Enumere.RemboursementAvance)
                    {
                        ChargerClientFromReference(this.Txt_ReferenceClient.Text, this.Txt_Ordre.Text);
                    }
                    else
                    {
                        Message.Show("La reference saisie n'est pas correcte", "Infomation");
                        prgBar.Visibility = System.Windows.Visibility.Collapsed;
                    }
                }
            }
            catch (Exception ex)
            {
                Message.ShowInformation(ex.Message, "Demande");
            }
        }
        private void RemplireOngletObjetScanne(List<ObjDOCUMENTSCANNE> _LstDocumentScanne)
        {
            try
            {
                #region DocumentScanne
                ctrl = new Galatee.Silverlight.Shared.UcFichierJoint(_LstDocumentScanne, false);
                Vwb.Stretch = Stretch.None;
                Vwb.Child = ctrl;
                #endregion
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        private void RemplireOngletAdresse(CsAg _LeAdresse)
        {
            try
            {
                if (_LeAdresse != null)
                {

                    this.tab3_txt_NomClientBrt.Text = string.IsNullOrEmpty(_LeAdresse.NOMP) ? string.Empty : _LeAdresse.NOMP;
                    this.tab3_txt_LibelleCommune.Text = string.IsNullOrEmpty(_LeAdresse.LIBELLECOMMUNE) ? string.Empty : _LeAdresse.LIBELLECOMMUNE;
                    this.tab3_txt_LibelleQuartier.Text = string.IsNullOrEmpty(_LeAdresse.LIBELLEQUARTIER) ? string.Empty : _LeAdresse.LIBELLEQUARTIER;
                    this.tab3_txt_Secteur.Text = string.IsNullOrEmpty(_LeAdresse.LIBELLESECTEUR) ? string.Empty : _LeAdresse.LIBELLESECTEUR;
                    this.tab3_txt_NumRue.Text = string.IsNullOrEmpty(_LeAdresse.RUE) ? string.Empty : _LeAdresse.RUE;

                    this.tab3_txt_etage.Text = string.IsNullOrEmpty(_LeAdresse.ETAGE) ? string.Empty : _LeAdresse.ETAGE;
                    this.tab3_txt_NumLot.Text = string.IsNullOrEmpty(_LeAdresse.CADR) ? string.Empty : _LeAdresse.CADR;

                    this.tab3_txt_Telephone.Text = string.IsNullOrEmpty(_LeAdresse.TELEPHONE) ? string.Empty : _LeAdresse.TELEPHONE;
                    this.tab3_txt_OrdreTour.Text = string.IsNullOrEmpty(_LeAdresse.ORDTOUR) ? string.Empty : _LeAdresse.ORDTOUR;
                    this.tab3_txt_tournee.Text = string.IsNullOrEmpty(_LeAdresse.TOURNEE) ? string.Empty : _LeAdresse.TOURNEE;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        private void RemplireOngletClient(CsClient _LeClient)
        {
            try
            {
                if (_LeClient != null)
                {

                    this.Txt_NomClient.Text = (string.IsNullOrEmpty(_LeClient.NOMABON) ? string.Empty : _LeClient.NOMABON);
                    //this.Txt_Telephone1.Text = string.IsNullOrEmpty(_LeClient.TELEPHONE) ? string.Empty : _LeClient.TELEPHONE;
                    //this.tab12_txt_addresse.Text = string.IsNullOrEmpty(_LeClient.ADRMAND1) ? string.Empty : _LeClient.ADRMAND1;
                    //this.tab12_txt_addresse2.Text = string.IsNullOrEmpty(_LeClient.ADRMAND2) ? string.Empty : _LeClient.ADRMAND2;
                    //this.txt_NINA.Text = string.IsNullOrEmpty(_LeClient.NUMEROIDCLIENT) ? string.Empty : _LeClient.NUMEROIDCLIENT;
                    this.tab12_Txt_LibelleCodeConso.Text = string.IsNullOrEmpty(_LeClient.LIBELLECODECONSO) ? string.Empty : _LeClient.LIBELLECODECONSO;
                    this.tab12_Txt_LibelleCategorie.Text = string.IsNullOrEmpty(_LeClient.LIBELLECATEGORIE) ? string.Empty : _LeClient.LIBELLECATEGORIE;
                    this.tab12_Txt_LibelleEtatClient.Text = string.IsNullOrEmpty(_LeClient.LIBELLERELANCE) ? string.Empty : _LeClient.LIBELLERELANCE;
                    this.tab12_Txt_LibelleTypeClient.Text = string.IsNullOrEmpty(_LeClient.LIBELLENATURECLIENT) ? string.Empty : _LeClient.LIBELLENATURECLIENT;
                    this.tab12_Txt_Nationnalite.Text = string.IsNullOrEmpty(_LeClient.LIBELLENATIONALITE) ? string.Empty : _LeClient.LIBELLENATIONALITE;
                    this.tab12_Txt_Datecreate.Text = string.IsNullOrEmpty(_LeClient.DATECREATION.ToString()) ? string.Empty : Convert.ToDateTime(_LeClient.DATECREATION).ToShortDateString();
                    //this.tab12_Txt_DateModif.Text = string.IsNullOrEmpty(_LeClient.DATEMODIFICATION.ToString()) ? string.Empty : Convert.ToDateTime(_LeClient.DATEMODIFICATION).ToShortDateString();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void RemplirOngletAbonnement(CsAbon  _LeAbon)
        {
            if (_LeAbon != null)
            {
                this.Txt_CodeTarif.Text = !string.IsNullOrEmpty(_LeAbon.TYPETARIF) ? _LeAbon.TYPETARIF : string.Empty;
                this.Txt_CodePussanceSoucrite.Text = !string.IsNullOrEmpty(_LeAbon.PUISSANCE.Value.ToString()) ? _LeAbon.PUISSANCE.Value.ToString() : string.Empty;

                if (_LeAbon.PUISSANCE != null)
                    this.Txt_CodePussanceSoucrite.Text = Convert.ToDecimal(_LeAbon.PUISSANCE.ToString()).ToString("N2");
                if (_LeAbon.PUISSANCEUTILISEE != null)
                    this.Txt_CodePuissanceUtilise.Text = Convert.ToDecimal(_LeAbon.PUISSANCEUTILISEE.Value).ToString("N2");

                this.Txt_CodeForfait.Text = string.IsNullOrEmpty(_LeAbon.FORFAIT) ? string.Empty : _LeAbon.FORFAIT;
                this.Txt_LibelleForfait.Text = string.IsNullOrEmpty(_LeAbon.LIBELLEFORFAIT) ? string.Empty : _LeAbon.LIBELLEFORFAIT;

                this.Txt_CodeTarif.Text = string.IsNullOrEmpty(_LeAbon.TYPETARIF) ? string.Empty : _LeAbon.TYPETARIF;
                this.Txt_LibelleTarif.Text = !string.IsNullOrEmpty(_LeAbon.LIBELLETARIF) ? _LeAbon.LIBELLETARIF : string.Empty;

                this.Txt_CodeFrequence.Text = string.IsNullOrEmpty(_LeAbon.PERFAC) ? string.Empty : _LeAbon.PERFAC;
                this.Txt_LibelleFrequence.Text = !string.IsNullOrEmpty(_LeAbon.LIBELLEFREQUENCE) ? _LeAbon.LIBELLEFREQUENCE : string.Empty;

                this.Txt_CodeMoisIndex.Text = string.IsNullOrEmpty(_LeAbon.MOISREL) ? string.Empty : _LeAbon.MOISREL;
                this.Txt_LibelleMoisIndex.Text = !string.IsNullOrEmpty(_LeAbon.LIBELLEMOISIND) ? _LeAbon.LIBELLEMOISIND : string.Empty;

                this.Txt_CodeMoisFacturation.Text = string.IsNullOrEmpty(_LeAbon.MOISFAC) ? string.Empty : _LeAbon.MOISFAC;
                this.Txt_LibMoisFact.Text = !string.IsNullOrEmpty(_LeAbon.LIBELLEMOISFACT) ? _LeAbon.LIBELLEMOISFACT : string.Empty;

                this.Txt_DateAbonnement.Text = (_LeAbon.DABONNEMENT == null) ?string.Empty  : Convert.ToDateTime(_LeAbon.DABONNEMENT.Value).ToShortDateString();
            }
        }

        private void RemplireOngletFacture(List<CsDemandeDetailCout>  _LesFactClient)
        {
            try
            {
                if (_LesFactClient != null && _LesFactClient.Count != 0)
                {
                    _LesFactClient.ForEach(t => t.MONTANTTTC = t.MONTANTHT + t.MONTANTTAXE);
                    this.LsvFacture.ItemsSource = null;
                    this.LsvFacture.ItemsSource = _LesFactClient;
                    this.Txt_TotalHt.Text = _LesFactClient.Sum(t => t.MONTANTHT).Value.ToString(SessionObject.FormatMontant);
                    this.Txt_totalTaxe .Text = _LesFactClient.Sum(t => t.MONTANTTAXE ).Value .ToString(SessionObject.FormatMontant );
                    this.Txt_TotalTTC.Text = _LesFactClient.Sum(t => t.MONTANTTTC).Value.ToString(SessionObject.FormatMontant);
                    AfficherOuMasquer(tabItemCompte, true );
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        private void ChargerPuissanceInstalle()
        {
            try
            {
                if (SessionObject.LstPuissanceInstalle != null && SessionObject.LstPuissanceInstalle.Count != 0)
                {

                    List<Galatee.Silverlight.ServiceAccueil.CsPuissance> _lstPuissance = SessionObject.LstPuissanceInstalle;
                    if (_lstPuissance != null && _lstPuissance.Count != 0)
                    {
                        List<ServiceAccueil.CsPuissance> lesPuissance = _lstPuissance.Where(t => t.PRODUIT == SessionObject.Enumere.ElectriciteMT).ToList();
                        Cbo_PuissanceInstalle.SelectedValuePath = "FK_IDPUISSANCE";
                        Cbo_PuissanceInstalle.DisplayMemberPath = "VALEUR";
                        Cbo_PuissanceInstalle.ItemsSource = lesPuissance;
                        if (lesPuissance != null && lesPuissance.Count == 1)
                        {
                            Cbo_PuissanceInstalle.SelectedItem = lesPuissance.First();
                            Cbo_PuissanceInstalle.Tag = lesPuissance.First();
                        }
                        return;
                    }
                }

                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.ChargerPuissanceInstalleCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    SessionObject.LstPuissanceInstalle = args.Result;
                    List<Galatee.Silverlight.ServiceAccueil.CsPuissance> _lstPuissance = SessionObject.LstPuissanceInstalle;
                    if (_lstPuissance != null && _lstPuissance.Count != 0)
                    {
                        List<ServiceAccueil.CsPuissance> lesPuissance = _lstPuissance.Where(t => t.PRODUIT == SessionObject.Enumere.ElectriciteMT).ToList();
                        Cbo_PuissanceInstalle.SelectedValuePath = "FK_IDPUISSANCE";
                        Cbo_PuissanceInstalle.DisplayMemberPath = "VALEUR";
                        Cbo_PuissanceInstalle.ItemsSource = lesPuissance;
                        if (lesPuissance != null && lesPuissance.Count == 1)
                        {
                            Cbo_PuissanceInstalle.SelectedItem = lesPuissance.First();
                            Cbo_PuissanceInstalle.Tag = lesPuissance.First();
                        }
                        return;
                    }
                };
                service.ChargerPuissanceInstalleAsync();
                service.CloseAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        private void Cbo_Produit_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void cbo_typedoc_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void txtSite_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
        private void txtCentre_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void ChargerClientFromReference(string ReferenceClient)
        {
            try
            {
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.RetourneClientByReferenceAsync(ReferenceClient, lstIdCentre);
                service.RetourneClientByReferenceCompleted += (s, args) =>
                {
                    if ((args != null && args.Cancelled) || (args.Error != null))
                        return;
                  
                    if (args.Result != null && args.Result.Count > 0)
                    {
                        if (args.Result != null && args.Result.Count > 1)
                        {
                            List<object> _Listgen = ClasseMEthodeGenerique.RetourneListeObjet(args.Result);
                            Galatee.Silverlight.MainView.UcListeGenerique ctr = new Galatee.Silverlight.MainView.UcListeGenerique(_Listgen, "CENTRE", "LIBELLESITE", "Liste des site");
                            ctr.Show();
                            ctr.Closed += new EventHandler(galatee_OkClickedChoixClient);
                        }
                        else
                        {
                            CsClient leClient = args.Result.First();
                            leClient.TYPEDEMANDE = Tdem;
                            VerifieExisteDemande(leClient);
                        }
                    }
                    else
                    {
                        Message.ShowError("Aucun client correspondant à ces critères n'a été trouvé", "Demande");
                        prgBar.Visibility = System.Windows.Visibility.Collapsed;
                        return;
                    }
                };
                service.CloseAsync();

            }
            catch (Exception)
            {
                prgBar.Visibility = System.Windows.Visibility.Collapsed;
                Message.ShowError("Erreur au chargement des données", "Demande");
            }
        }


        private void ChargerClientFromReference(string ReferenceClient,string Ordre)
        {
            try
            {
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.RetourneClientByReferenceOrdreCompleted += (s, args) =>
                {
                    if ((args != null && args.Cancelled) || (args.Error != null))
                        return;
                    if (args.Result != null && args.Result.Count > 0)
                    {
                        if (args.Result != null && args.Result.Count > 1)
                        {
                            List<object> _Listgen = ClasseMEthodeGenerique.RetourneListeObjet(args.Result);
                            Galatee.Silverlight.MainView.UcListeGenerique ctr = new Galatee.Silverlight.MainView.UcListeGenerique(_Listgen, "CENTRE", "LIBELLESITE", "Liste des site");
                            ctr.Show();
                            ctr.Closed += new EventHandler(galatee_OkClickedChoixClient);
                        }
                        else 
                        {
                            CsClient leClient = args.Result.First();
                            leClient.TYPEDEMANDE = Tdem;
                            VerifieExisteDemande(leClient);
                        }
                    }
                    else
                    {
                        Message.ShowError("Aucun client correspondant à ces critères n'a été trouvé", "Demande");
                        prgBar.Visibility = System.Windows.Visibility.Collapsed;
                        return;
                    }
                };
                service.RetourneClientByReferenceOrdreAsync (ReferenceClient,Ordre , lstIdCentre);
                service.CloseAsync();

            }
            catch (Exception)
            {
                prgBar.Visibility = System.Windows.Visibility.Collapsed;
                Message.ShowError("Erreur au chargement des données", "Demande");
            }
        }

        private void galatee_OkClickedChoixClient(object sender, EventArgs e)
        {
            Galatee.Silverlight.MainView.UcListeGenerique ctrs = sender as Galatee.Silverlight.MainView.UcListeGenerique;
            if (ctrs.isOkClick)
            {
               CsClient  _UnClient = (CsClient)ctrs.MyObject;
               _UnClient.TYPEDEMANDE = Tdem;
               VerifieExisteDemande(_UnClient);
            }
        }

        private List<CsClient> DistinctSiteClient(List<CsClient> lstClient)
        {
            try
            {
                List<CsClient> lstCentreDistClientOrdreProduit = new List<CsClient>();
                var lstCentreDistnct = lstClient.Select(t => new { t.LIBELLESITE ,t.FK_IDCENTRE , t.CENTRE ,t.REFCLIENT,t.PRODUIT   }).Distinct().ToList();
                foreach (var item in lstCentreDistnct)
                {
                    CsClient leClient = new CsClient() 
                    {
                      FK_IDCENTRE = item.FK_IDCENTRE ,
                      CENTRE = item.CENTRE ,
                      REFCLIENT = item.REFCLIENT ,
                      PRODUIT = item.PRODUIT 
                    };
                    lstCentreDistClientOrdreProduit.Add(leClient);
                }
                return lstCentreDistClientOrdreProduit;
            }
            catch (Exception)
            {
                
                throw;
            }
        }


        private void ChargeDetailDEvis(CsClient leclient)
        {

            try
            {
                leclient.TYPEDEMANDE = Tdem;
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient client = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                client.ChargerDetailClientAsync(leclient);
                client.ChargerDetailClientCompleted += (ssender, args) =>
                {
                    prgBar.Visibility = System.Windows.Visibility.Collapsed;

                    if (args.Cancelled || args.Error != null)
                    {
                        string error = args.Error.Message;
                        Message.ShowError(error, Silverlight.Resources.Devis.Languages.txtDevis);
                        return;
                    }
                    if (args.Result == null)
                    {
                        Message.ShowError(Silverlight.Resources.Devis.Languages.AucunesDonneesTrouvees, Silverlight.Resources.Devis.Languages.txtDevis);
                        return;
                    }
                    else
                    {

                        laDetailDemande = new CsDemande();
                        laDetailDemande = args.Result;
                        this.txtSite.Text = string.IsNullOrEmpty(laDetailDemande.LeClient.LIBELLESITE) ? string.Empty : laDetailDemande.LeClient.LIBELLESITE;
                        this.txtCentre.Text = string.IsNullOrEmpty(laDetailDemande.LeClient.LIBELLECENTRE) ? string.Empty : laDetailDemande.LeClient.LIBELLECENTRE;
                        this.txt_Produit.Text = string.IsNullOrEmpty(laDetailDemande.Abonne.LIBELLEPRODUIT) ? string.Empty : laDetailDemande.Abonne.LIBELLEPRODUIT;
                        this.txt_Produit.Tag  = laDetailDemande.Abonne.FK_IDPRODUIT ;
                        this.txt_tdem.Text = SessionObject.LstTypeDemande.FirstOrDefault(t => t.CODE == Tdem).LIBELLE;
                        txt_tdem.Tag = SessionObject.LstTypeDemande.FirstOrDefault(t => t.CODE == Tdem);

                        if (((CsTdem)this.txt_tdem.Tag).CODE == SessionObject.Enumere.ChangementProduit)
                        {
                            List<CsProduit> lstProduitSite = (lesCentre.FirstOrDefault(t => t.PK_ID == laDetailDemande.Ag.FK_IDCENTRE).LESPRODUITSDUSITE).ToList();
                            this.txt_Produit.Text = lstProduitSite.FirstOrDefault(u => u.PK_ID  != laDetailDemande.Abonne.FK_IDPRODUIT).LIBELLE;
                            this.txt_Produit.Tag = lstProduitSite.FirstOrDefault(u => u.PK_ID != laDetailDemande.Abonne.FK_IDPRODUIT).PK_ID;
                            lblProduit.Tag = lstProduitSite.FirstOrDefault(u => u.PK_ID != laDetailDemande.Abonne.FK_IDPRODUIT).CODE;

                            if (laDetailDemande.LstCanalistion != null && laDetailDemande.LstCanalistion.Count() != 0)
                            {
                                CsReglageCompteur leRegl = SessionObject.LstReglageCompteur.FirstOrDefault(o => o.PK_ID == laDetailDemande.LstCanalistion.FirstOrDefault().FK_IDREGLAGECOMPTEUR );
                                if (leRegl != null)
                                {
                                    this.txt_Reglage.Text = leRegl.LIBELLE;
                                    this.txt_Reglage.Tag = leRegl.PK_ID;
                                    this.Btn_Reglage.Tag = leRegl.CODE;
                                }
                            }

                        }

                        RemplireOngletClient(laDetailDemande.LeClient);
                        RemplirOngletAbonnement(laDetailDemande.Abonne);
                        RemplireOngletAdresse(laDetailDemande.Ag);


                        if (((CsTdem)this.txt_tdem.Tag).CODE == SessionObject.Enumere.Resiliation && laDetailDemande.Abonne.DRES != null)

                        {
                            Message.ShowInformation("Ce client est déjà résilié", "Information");
                            this.OKButton.IsEnabled = false;
                            return;
                        }


                        if (((CsTdem)this.txt_tdem.Tag).CODE == SessionObject.Enumere.AugmentationPuissance  ||
                            ((CsTdem)this.txt_tdem.Tag).CODE == SessionObject.Enumere.DimunitionPuissance )
                        {
                            if (laDetailDemande.Abonne.DRES != null)
                            {
                                Message.ShowInformation("Ce client est déjà résilié", "Information");
                                this.OKButton.IsEnabled = false;
                                return;
                            }
                            else
                            {
                                if (((CsTdem)this.txt_tdem.Tag).CODE == SessionObject.Enumere.AbonnementSeul ||
                                    ((CsTdem)this.txt_tdem.Tag).CODE == SessionObject.Enumere.Reabonnement )
                                {
                                    Message.ShowInformation("Il existe une abonnement actif sur cette référence", "Information");
                                    this.OKButton.IsEnabled = false;
                                    return;
                                }
                                else 
                                this.OKButton.IsEnabled = true;
                            }
                        }
                        if (((CsTdem)this.txt_tdem.Tag).CODE == SessionObject.Enumere.AugmentationPuissance ||
                            ((CsTdem)this.txt_tdem.Tag).CODE == SessionObject.Enumere.DimunitionPuissance)
                        {
                            AfficherOuMasquer(tab_demande, true);
                            txt_ancienPuiss.Text = laDetailDemande.Abonne.PUISSANCE.Value.ToString(SessionObject.FormatMontant );
                                this.Cbo_PuissanceSouscrite.ItemsSource = null;
                                this.Cbo_PuissanceSouscrite.DisplayMemberPath = "VALEUR";
                            if (((CsTdem)this.txt_tdem.Tag).CODE == SessionObject.Enumere.AugmentationPuissance)
                                this.Cbo_PuissanceSouscrite.ItemsSource = SessionObject.LstPuissance.Where(t => t.PRODUIT == laDetailDemande.Abonne.PRODUIT &&  t.VALEUR > laDetailDemande.Abonne.PUISSANCE ).ToList();
                            else
                                this.Cbo_PuissanceSouscrite.ItemsSource = SessionObject.LstPuissance.Where(t => t.PRODUIT == laDetailDemande.Abonne.PRODUIT && t.VALEUR < laDetailDemande.Abonne.PUISSANCE).ToList();

                            if (laDetailDemande.Abonne.PRODUIT == SessionObject.Enumere.ElectriciteMT)
                            {
                                Cbo_diametre.Visibility = System.Windows.Visibility.Collapsed;
                                label.Visibility = System.Windows.Visibility.Collapsed;

                                lbl_ancienPuiss_Copy.Visibility = System.Windows.Visibility.Visible;
                                libeNPI.Visibility = System.Windows.Visibility.Visible;
                                txt_AncPuissanceInstalle.Visibility = System.Windows.Visibility.Visible;
                                Cbo_PuissanceInstalle.Visibility = System.Windows.Visibility.Visible;
                                txt_AncPuissanceInstalle.Text = laDetailDemande.Branchement.PUISSANCEINSTALLEE.Value.ToString(SessionObject.FormatMontant);

                            }
                        }
                        if (((CsTdem)this.txt_tdem.Tag).CODE == SessionObject.Enumere.RemboursementParticipation )
                            ChargerFraisParticipation(leclient);
                    }
                };
            }
            catch (Exception ex)
            {
                prgBar.Visibility = System.Windows.Visibility.Collapsed;
                Message.ShowError("Erreur au chargement des donnéés", "Demande");
            }
        }
       //private void ChargerCompteDeResiliation(CsClient _UnClient)
       // {

       //     AcceuilServiceClient client = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
       //     client.ChargerCompteDeResiliationCompleted += (s, args) =>
       //     {
       //         if (args != null && args.Cancelled)
       //             return;
       //         if (args.Result == null || args.Result.Count == 0)
       //         {
       //             Message.ShowInformation("Ce client n'existe pas", "RetourneListeFactureNonSolde");
       //             return;
       //         }
       //         List<CsLclient> lstFactureDuClient = args.Result;
       //         lstFactureDuClient.ForEach(t => t.REFEM = ClasseMEthodeGenerique.FormatPeriodeMMAAAA(t.REFEM));
       //         if (lstFactureDuClient != null && lstFactureDuClient.Count != 0)
       //         {
       //             AfficherOuMasquer(tabItemCompte, true);
       //             Txt_TotalHt.Visibility = System.Windows.Visibility.Collapsed ;
       //             Txt_totalTaxe.Visibility = System.Windows.Visibility.Collapsed ;
       //             lbl_total.Content = "Solde client";
       //             LsvFacture.ItemsSource = lstFactureDuClient;
       //             Txt_TotalTTC.Text = lstFactureDuClient.Sum(t => t.SOLDEFACTURE).Value.ToString(SessionObject.FormatMontant);
       //         }
       //     };
       //     client.ChargerCompteDeResiliationAsync(_UnClient);
       //     client.CloseAsync();
        
       // }
       private void ChargerFraisParticipation(CsClient _UnClient)
       {

           AcceuilServiceClient client = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
           client.ChargerFraisParticipationCompleted += (s, args) =>
           {
               if (args != null && args.Cancelled)
                   return;
               if (args.Result == null || args.Result.Count == 0)
               {
                   Message.ShowInformation("Ce client n'a pas de frais de participation a rembourser", "Info");
                   return;
               }
               List<CsLclient> lstFactureDuClient = args.Result;
               lstFactureDuClient.ForEach(t => t.REFEM = ClasseMEthodeGenerique.FormatPeriodeMMAAAA(t.REFEM));
               if (lstFactureDuClient != null && lstFactureDuClient.Count != 0)
               {
                   AfficherOuMasquer(tabItemCompte, true);
                   LsvFacture.ItemsSource = lstFactureDuClient;
               }
           };
           client.ChargerFraisParticipationAsync(_UnClient);
           client.CloseAsync();

       }
       private void RemplirListeDesReglageExistant()
       {
           try
           {
               if (SessionObject.LstReglageCompteur != null && SessionObject.LstReglageCompteur.Count != 0)
               {
                   _listeDesReglageCompteurExistant = SessionObject.LstReglageCompteur;
                   return;
               }
               AcceuilServiceClient service = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
               service.ChargerReglageCompteurCompleted += (s, args) =>
               {
                   if (args != null && args.Cancelled)
                       return;
                   SessionObject.LstReglageCompteur = args.Result;
                   _listeDesReglageCompteurExistant = SessionObject.LstReglageCompteur;

               };
               service.ChargerReglageCompteurAsync();
               service.CloseAsync();
           }
           catch (Exception ex)
           {

               throw ex;
           }
       }
       private void RemplirListeDesDiametresExistant(CsPuissance laPuissance)
       {

           try
           {
               if (SessionObject.LstReglageCompteur != null && SessionObject.LstReglageCompteur.Count != 0)
               {

                   if (Tdem  == SessionObject.Enumere.AugmentationPuissance ||
                       Tdem  == SessionObject.Enumere.DimunitionPuissance)
                   {
                       List<Galatee.Silverlight.ServiceAccueil.CsPuissance> _lstPuissance = SessionObject.LstPuissanceParReglageCompteur;
                       if (_lstPuissance != null && _lstPuissance.Count != 0)
                       {
                           List<ServiceAccueil.CsPuissance> lesPuissanceRegalage = _lstPuissance.Where(t => t.FK_IDPRODUIT ==(int)this.txt_Produit.Tag && t.VALEUR == laPuissance.VALEUR).ToList();
                           List<Galatee.Silverlight.ServiceAccueil.CsReglageCompteur> lstReglageCompteur = SessionObject.LstReglageCompteur.Where(p => p.FK_IDPRODUIT ==(int)this.txt_Produit.Tag && lesPuissanceRegalage.Select(y => y.FK_IDREGLAGECOMPTEUR).Contains(p.PK_ID)).ToList();
                           Cbo_diametre.SelectedValuePath = "PK_ID";
                           Cbo_diametre.DisplayMemberPath = "LIBELLE";
                           Cbo_diametre.ItemsSource = null;
                           Cbo_diametre.ItemsSource = lstReglageCompteur;
                           if (lstReglageCompteur != null && lstReglageCompteur.Count == 1)
                           {
                               Cbo_diametre.SelectedItem = lstReglageCompteur.First();
                               Cbo_diametre.Tag = lstReglageCompteur.First();
                           }
                       }
                       return;
                   }
               }
           }
           catch (Exception es)
           {

               MessageBox.Show(es.Message);
           }
       }

       private void Cbo_PuissanceSouscrite_SelectionChanged(object sender, SelectionChangedEventArgs e)
       {
           if (this.Cbo_PuissanceSouscrite.SelectedItem != null)
               RemplirListeDesDiametresExistant((CsPuissance)this.Cbo_PuissanceSouscrite.SelectedItem);
       }

       private void Btn_Reglage_Click(object sender, RoutedEventArgs e)
       {
           try
           {
               if (this.txt_Produit.Tag  != null)
               {
                   var UcListReglage = new Galatee.Silverlight.Accueil.UcListeReglageCompteur(_listeDesReglageCompteurExistant.Where(t => t.FK_IDPRODUIT ==int.Parse(this.txt_Produit.Tag.ToString())).ToList());
                   UcListReglage.Closed += new EventHandler(UcListReglage_Closed);
                   this.IsEnabled = false;
                   UcListReglage.Show();
               }

           }
           catch (Exception ex)
           {
               Message.ShowError(ex.Message, Languages.txtDevis);
           }
       }
       private void UcListReglage_Closed(object sender, EventArgs e)
       {
           try
           {
               this.IsEnabled = true;
               UcListeReglageCompteur ctrs = sender as UcListeReglageCompteur;
               if (ctrs.isOkClick)
               {
                   if (ctrs.leReglageSelect != null)
                   {
                       this.txt_Reglage.Text = ctrs.leReglageSelect.LIBELLE;
                       this.txt_Reglage.Tag = ctrs.leReglageSelect.PK_ID;
                       this.Btn_Reglage.Tag = ctrs.leReglageSelect.CODE;
                   }
               }
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }


    }
}

