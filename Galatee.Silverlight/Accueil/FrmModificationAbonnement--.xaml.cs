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
using Galatee.Silverlight.ServiceAccueil;
using Galatee.Silverlight.Resources.Devis;
using Galatee.Silverlight.Shared;
using Galatee.Silverlight.MainView;
using Galatee.Silverlight.Resources.Accueil ;
using System.IO;

namespace Galatee.Silverlight.Accueil
{
    public partial class FrmModificationAbonnement : ChildWindow
    {
        List<CsTarif> LstTarif = new List<CsTarif>();
        List<CsForfait> LstForfait = new List<CsForfait>();
        List<CsPuissance> LstPuissance = new List<CsPuissance>();
        List<CsFrequence> LstFrequence = new List<CsFrequence>();
        List<CsMois> LstMois = new List<CsMois>();
        List<CsCodeTaxeApplication> LstCodeApplicationTaxe = new List<CsCodeTaxeApplication>();
        List<CsTypeBranchement> LstTypeBranchement = new List<CsTypeBranchement>();
        private List<CsCentre> _listeDesCentreExistant = null;
        CsTarif LeTarifSelect = new CsTarif();
        CsForfait LeForfaitSelect = new CsForfait();
        CsPuissance LePuissanceSelect = new CsPuissance();
        CsFrequence LeFrequenceSelect = new CsFrequence();
        CsMois LeMoisFactSelect = new CsMois();
        CsMois LeIndexFactSelect = new CsMois();
        CsCentre LeCentreSelect = new CsCentre();
        CsProduit LeProduitSelect = new CsProduit();
        CsDemandeBase laDemandeSelect = null;
        CsCodeTaxeApplication LstCodeApplicationTaxeSelect = new CsCodeTaxeApplication();
        List<CsCentre> LstCentre = new List<CsCentre>();
        List<CsProduit> ListeDesProduitDuSite = new List<CsProduit>();


        CsDevis LeDevis = new CsDevis();
        List<CsAbon> AbonementRecherche;
        CsAbon LeAbonne = new CsAbon();
        decimal InitValue = 0;
        bool IsUpdate = false;
        string TypeDemande, NatureCLient = string.Empty;
        //CsAbon _LeAbon = new CsAbon();
        public CsDemande LaDemande = new CsDemande();
        public FrmModificationAbonnement()
        {
            InitializeComponent();
            this.btn_Supprime.Visibility = System.Windows.Visibility.Collapsed;
            this.btn_Modifier.Visibility = System.Windows.Visibility.Collapsed;
        }
        public FrmModificationAbonnement(CsDemande _LaDemande)
        {
            InitializeComponent();
            translate();
            _LaDemande.LaDemande.STATUTDEMANDE = null;
            LaDemande = _LaDemande;
            if (LaDemande.Abonne == null) LaDemande.Abonne = new CsAbon();
            AbonementRecherche = new List<CsAbon>();
            AbonementRecherche.Add(LaDemande.Abonne);
          
            ChargerFrequence();
            ChargerApplicationTaxe();
            ChargerMois();
            ChargerTarif();
            ChargerForfait();
            ChargerPuissance();
            TypeDemande = LaDemande.LaDemande.TYPEDEMANDE;
            this.Txt_CodePussanceSoucrite.Text = InitValue.ToString();
            this.Txt_CodePuissanceUtilise.Text = InitValue.ToString();
            this.Txt_Consomation.Text = InitValue.ToString();
            this.Txt_CodeRistoune.Text = InitValue.ToString();
            this.Txt_CodeCentre.MaxLength = SessionObject.Enumere.TailleCentre;
            this.Txt_CodeProduit.MaxLength = SessionObject.Enumere.TailleCodeProduit;
            this.Txt_Client.MaxLength = SessionObject.Enumere.TailleClient;
            this.btn_Supprime.Visibility = System.Windows.Visibility.Collapsed;
            this.btn_Modifier.Visibility = System.Windows.Visibility.Collapsed;
            if (LaDemande.LaDemande.PRODUIT != SessionObject.Enumere.Electricite)
            {
                this.Txt_CodePussanceSoucrite.IsReadOnly = true;
                this.Txt_CodePuissanceUtilise.IsReadOnly = true;
                this.Txt_Consomation.IsReadOnly = true;
                this.Txt_CodeRistoune.IsReadOnly = true;
            }

            this.btn_Centre.IsEnabled = false;
            this.btn_Produit .IsEnabled = false;
            this.btn_Site .IsEnabled = false;
            this.Txt_CodeSite.IsReadOnly = true;
            //this.Txt_LibelleSite .IsReadOnly = true;

            this.lnkMotif.Visibility = System.Windows.Visibility.Collapsed;

            this.Txt_NumDemande.IsReadOnly = true;
            this.Txt_CodeCentre.IsReadOnly = true;
            this.Txt_CodeProduit.IsReadOnly = true;
            this.Txt_Client.IsReadOnly = true;
            this.Txt_Ordre.IsReadOnly = true;
            this.Txt_CodeCentre.Text = string.IsNullOrEmpty(LaDemande.LaDemande.CENTRE) ? string.Empty : LaDemande.LaDemande.CENTRE;
            this.Txt_CodeProduit.Text = string.IsNullOrEmpty(LaDemande.LaDemande.PRODUIT) ? string.Empty : LaDemande.LaDemande.PRODUIT;
            
            btn_Rechercher.IsEnabled = false;
            //ChargerDonneeDuSite();
            ChargerListDesSite();
            ChargerListeDeProduit();
            chargerInformation();
        }
        public FrmModificationAbonnement(string _TypeDemande)
        {
            InitializeComponent();
            this.btn_Supprime.Visibility = System.Windows.Visibility.Collapsed;
            this.btn_Modifier.Visibility = System.Windows.Visibility.Collapsed;
            TypeDemande = _TypeDemande;
            if (LaDemande.Abonne == null) LaDemande.Abonne = new CsAbon();
            if (LaDemande.LaDemande == null) LaDemande.LaDemande = new CsDemandeBase();
            if (LaDemande.LeCentre == null) LaDemande.LeCentre = new CsCentre();
            if (LaDemande.LeProduit == null) LaDemande.LeProduit = new CsProduit();
            ChargerFrequence();
            ChargerApplicationTaxe();
            ChargerMois();
            ChargerTarif();
            ChargerForfait();
            ChargerPuissance();
            this.Txt_CodePussanceSoucrite.Text = InitValue.ToString();
            this.Txt_CodePuissanceUtilise.Text = InitValue.ToString();
            this.Txt_Consomation.Text = InitValue.ToString();
            this.Txt_CodeRistoune.Text = InitValue.ToString();
            this.lnkMotif.Visibility = System.Windows.Visibility.Collapsed;


            this.Txt_CodeCentre.MaxLength = SessionObject.Enumere.TailleCentre;
            this.Txt_CodeProduit.MaxLength = SessionObject.Enumere.TailleCodeProduit;
            this.Txt_Client.MaxLength = SessionObject.Enumere.TailleClient;
            if (LaDemande.LaDemande.PRODUIT != SessionObject.Enumere.Electricite)
            {
                this.Txt_CodePussanceSoucrite.IsReadOnly = true;
                this.Txt_CodePuissanceUtilise.IsReadOnly = true;
                this.Txt_Consomation.IsReadOnly = true;
                this.Txt_CodeRistoune.IsReadOnly = true;
            }
            IsGiserChamp(true);
            //ChargerDonneeDuSite();
            ChargerListDesSite();
            ChargerListeDeProduit();
            translate();
        }

        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            InitialiseCtrl();
        }

        private void IsGiserChamp(bool Etat)
        {
            this.Txt_CodeTarif.IsReadOnly = Etat;
            this.btn_tarifs.IsEnabled = !Etat;

            this.Txt_CodePussanceSoucrite.IsReadOnly = Etat;
            this.btn_PussSouscrite.IsEnabled = !Etat;

            this.Txt_CodePuissanceUtilise.IsReadOnly = Etat;

            this.Txt_CodeForfait.IsReadOnly = Etat;
            this.btn_discount.IsEnabled = !Etat;

            this.Txt_ForfaitPersonalise.IsReadOnly = Etat;

            this.btn_frequence.IsEnabled = !Etat;
            this.Txt_CodeFrequence.IsReadOnly = Etat;

            this.btn_moisdefacturation.IsEnabled = !Etat;
            this.Txt_CodeMoisFacturation.IsReadOnly = Etat;

            this.btn_MoisIndex.IsEnabled = !Etat;
            this.Txt_CodeMoisIndex.IsReadOnly = Etat;

            this.Txt_CodeApplicationTaxe.IsReadOnly = Etat;
            this.btn_taxeApplication.IsEnabled = !Etat;

            this.Txt_CodeForfait.IsReadOnly = Etat;
            this.btn_forfait.IsEnabled = !Etat;
        }
        private void translate()
        {
            // Gestion de la langue
            this.lbl_ApplicationTax.Content = Langue.lbl_ApplicationTax;
            this.lbl_Client.Content = Langue.lbl_client;
            this.lbl_DateAbonnement.Content = Langue.lbl_DateAbonnement;
            this.lbl_DateResiliation.Content = Langue.lbl_DateResiliation;
            this.lbl_Forfait.Content = Langue.lbl_Forfait;
            this.lbl_ForfaitPersonaliseAnnuel.Content = Langue.lbl_ForfaitPersonaliseAnnuel;
            this.lbl_Comsomation.Content = Langue.lbl_consommation;
            this.lbl_MoisFact.Content = Langue.lbl_MoisFact;
            this.lbl_MoisReleve.Content = Langue.lbl_MoisReleve;
            this.lbl_Ordre.Content = Langue.lbl_Ordre;
            this.lbl_Periodicite.Content = Langue.lbl_Periodicite;
            this.lbl_PuissanceSouscrite.Content = Langue.lbl_PuissanceSouscrite;
            this.lbl_PuissanceUtilise.Content = Langue.lbl_PuissanceUtilise;
            this.lbl_Ristourne.Content = Langue.lbl_Ristourne;
            this.lbl_Tarif.Content = Langue.lbl_Tarif;
            this.rdb_GprInvoiceNo.Content = Langue.lbl_Oui;
            this.rdb_gprInvoiceYes.Content = Langue.lbl_Non;
            //
        }
        private void chargerInformation()
        {
            IsGiserChamp(false);
            CsAbon _LAbon = new CsAbon();
            if (LaDemande.Abonne != null) _LAbon = LaDemande.Abonne;
            AfficherInfoAbonnement(_LAbon);
            RemplireLibelle();
            if (!string.IsNullOrEmpty(LaDemande.LaDemande.ANNOTATION))
                this.lnkMotif.Visibility = System.Windows.Visibility.Visible;

            if (LaDemande.LaDemande.FICHIERJOINT != new Guid("00000000-0000-0000-0000-000000000000"))
                RetourneObjectScan(LaDemande.LaDemande);
        }
        ObjDOCUMENTSCANNE leObjectScan = new ObjDOCUMENTSCANNE();
        private void RetourneObjectScan(CsDemandeBase laDemande)
        {
            try
            {
                AcceuilServiceClient service = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                service.ReturneObjetScanCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    leObjectScan = args.Result;
                    if (leObjectScan != null)
                    {
                        this.lnkLetter.Content = Langue.msgVoirPiecejointe;
                        this.lnkLetter.Tag = leObjectScan.CONTENU;
                        this.btn_Supprime.Visibility = System.Windows.Visibility.Visible;
                        this.btn_Modifier.Visibility = System.Windows.Visibility.Visible;
                    }
                };
                service.ReturneObjetScanAsync(laDemande);
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void InitialiseCtrl()
        {
            try
            {
              //  this.Txt_CodeCentre.Text = string.IsNullOrEmpty(LaDemande.LaDemande.CENTRE) ? string.Empty : LaDemande.LaDemande.CENTRE;
                this.Txt_CodeProduit.Text = string.IsNullOrEmpty(LaDemande.LaDemande.PRODUIT) ? string.Empty : LaDemande.LaDemande.PRODUIT;
            //    this.Txt_NumDemande.Text = string.IsNullOrEmpty(LaDemande.LaDemande.NUMDEM) ? string.Empty : LaDemande.LaDemande.NUMDEM;
                this.Txt_Client.Text = string.IsNullOrEmpty(LaDemande.LaDemande.CLIENT) ? string.Empty : LaDemande.LaDemande.CLIENT;
                this.Txt_Ordre.Text = string.IsNullOrEmpty(LaDemande.LaDemande.ORDRE) ? string.Empty : LaDemande.LaDemande.ORDRE;
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Langue.lbl_Menu);
            }
        }
        ObjDOCUMENTSCANNE leDoc = new ObjDOCUMENTSCANNE();
        public bool EnregisterDemande(CsDemande _Lademande)
        {
            try
            {
                _Lademande.Abonne.NUMDEM = string.IsNullOrEmpty(this.Txt_NumDemande.Text) ? string.Empty : this.Txt_NumDemande.Text;

                _Lademande.Abonne.PUISSANCE = string.IsNullOrEmpty(this.Txt_CodePussanceSoucrite.Text) ? 0 : Convert.ToDecimal(this.Txt_CodePussanceSoucrite.Text);
                _Lademande.Abonne.PUISSANCEUTILISEE = string.IsNullOrEmpty(this.Txt_CodePuissanceUtilise.Text) ? 0 : Convert.ToDecimal(this.Txt_CodePuissanceUtilise.Text);
                _Lademande.Abonne.RISTOURNE = string.IsNullOrEmpty(this.Txt_CodeRistoune.Text) ? 0 : Convert.ToDecimal(this.Txt_CodeRistoune.Text);
                _Lademande.Abonne.NATURECLIENT = (AbonementRecherche != null && AbonementRecherche[0].NATURECLIENT != null && !string.IsNullOrEmpty(AbonementRecherche[0].NATURECLIENT)) ? AbonementRecherche[0].NATURECLIENT : string.Empty;
                _Lademande.Abonne.FORFAIT = string.IsNullOrEmpty(this.Txt_CodeForfait.Text) ? null : this.Txt_CodeForfait.Text;
                _Lademande.Abonne.TYPETARIF  = this.Txt_CodeTarif.Text;
                _Lademande.Abonne.PERFAC = this.Txt_CodeFrequence.Text;
                _Lademande.Abonne.MOISREL = this.Txt_CodeMoisIndex.Text;

                _Lademande.Abonne.MOISFAC = this.Txt_CodeMoisFacturation.Text;

                _Lademande.Abonne.DABONNEMENT = null;
                if (!string.IsNullOrEmpty(this.Txt_DateAbonnement.Text))
                    _Lademande.Abonne.DABONNEMENT = DateTime.Parse(this.Txt_DateAbonnement.Text);
                _Lademande.Abonne.DRES = null;
                if (_Lademande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.ModificationAbonnement)
                    _Lademande.Abonne.DRES = null;
                else if (!string.IsNullOrEmpty(this.Txt_DateResiliation.Text))
                    _Lademande.Abonne.DRES = DateTime.Parse(this.Txt_DateResiliation.Text);

                _Lademande.Abonne.USERCREATION = UserConnecte.matricule;
                _Lademande.Abonne.USERMODIFICATION = UserConnecte.matricule;
                _Lademande.Abonne.DATECREATION = System.DateTime.Now;
                _Lademande.Abonne.DATEMODIFICATION = System.DateTime.Now;
                _Lademande.Abonne.CENTRE = Txt_CodeCentre.Text;
                _Lademande.Abonne.FK_IDCENTRE = ((CsCentre)Txt_CodeCentre.Tag).PK_ID;
                //LeAbonne.FK_IDCENTRE = LaDemande.LeCentre.PK_ID;
              
                 _Lademande.LeClient.REFCLIENT = this.Txt_Client.Text;
                _Lademande.LeClient.CODECONSO = this.Txt_Consomation.Text;
                _Lademande.LeClient.NATURE = NatureCLient;
                _Lademande.LeClient.NUMEROPIECEIDENTITE = _Lademande.LeClient.NUMEROPIECEIDENTITE.Trim();
          

                _Lademande.LaDemande.MATRICULE = UserConnecte.matricule;
                _Lademande.LaDemande.NUMDEM = this.Txt_NumDemande.Text;
                _Lademande.LaDemande.ORDRE = this.Txt_Ordre.Text;
                _Lademande.LaDemande.CENTRE = Txt_CodeCentre.Text;
                _Lademande.LaDemande.STATUT = SessionObject.Enumere.DemandeStatusEnAttente;
                _Lademande.LaDemande.TYPEDEMANDE = TypeDemande;
                //_Lademande.LaDemande.CLIENT = AbonementRecherche[0].CLIENT;
                _Lademande.LaDemande.USERCREATION = UserConnecte.matricule;
                _Lademande.LaDemande.USERMODIFICATION = UserConnecte.matricule;
                //_Lademande.LaDemande.DATECREATION = System.DateTime.Now;
                //_Lademande.LaDemande.DATEMODIFICATION = System.DateTime.Now;
                if (lnkLetter.Tag != null)
                    leDoc = SaveFile((byte[])lnkLetter.Tag, 1, null);
                return true;
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Langue.lbl_Menu);
                return false;
            }

        }

        private void ChargerPuissance()
        {
            try
            {
                if (SessionObject.LstPuissance.Count != 0)
                {
                    LstPuissance = SessionObject.LstPuissance;
                }
                else
                {
                    AcceuilServiceClient service = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                    service.ChargerPuissanceCompleted += (s, args) =>
                    {
                        if (args != null && args.Cancelled)
                            return;
                        SessionObject.LstPuissance = args.Result;
                        LstPuissance = SessionObject.LstPuissance;

                    };
                    service.ChargerPuissanceAsync();
                    service.CloseAsync();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        private void ChargerForfait()
        {
            try
            {
                if (SessionObject.LstForfait.Count != 0)
                    LstForfait = SessionObject.LstForfait;
                else
                {
                    AcceuilServiceClient service = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                    service.ChargerForfaitCompleted += (s, args) =>
                    {
                        if (args != null && args.Cancelled)
                            return;

                        SessionObject.LstForfait = args.Result;
                        LstForfait = SessionObject.LstForfait;
                    };
                    service.ChargerForfaitAsync();
                    service.CloseAsync();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        private void ChargerTarif()
        {
            try
            {
                if (SessionObject.LstTarif.Count != 0)
                    LstTarif = SessionObject.LstTarif;
                else
                {
                    AcceuilServiceClient service = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                    service.ChargerTarifCompleted += (s, args) =>
                    {
                        if (args != null && args.Cancelled)
                            return;
                        SessionObject.LstTarif = args.Result;
                        LstTarif = SessionObject.LstTarif;

                    };
                    service.ChargerTarifAsync();
                    service.CloseAsync();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        private void ChargerFrequence()
        {
            if (SessionObject.LstFrequence.Count != 0)
                LstFrequence = SessionObject.LstFrequence;
            else
            {
                AcceuilServiceClient service = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                service.ChargerTousFrequenceCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    SessionObject.LstFrequence = args.Result;
                    LstFrequence = SessionObject.LstFrequence;
                };
                service.ChargerTousFrequenceAsync();
                service.CloseAsync();
            }
        }
        private void ChargerMois()
        {
            try
            {
                if (SessionObject.LstMois.Count != 0)
                    LstMois = SessionObject.LstMois;
                else
                {
                    AcceuilServiceClient service = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                    service.ChargerTousMoisCompleted += (s, args) =>
                    {
                        if (args != null && args.Cancelled)
                            return;
                        SessionObject.LstMois = args.Result;
                        LstMois = SessionObject.LstMois;
                    };
                    service.ChargerTousMoisAsync();
                    service.CloseAsync();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        private void ChargerApplicationTaxe()
        {
            try
            {
                if (SessionObject.LstCodeApplicationTaxe.Count != 0)
                    LstCodeApplicationTaxe = SessionObject.LstCodeApplicationTaxe;
                else
                {
                    AcceuilServiceClient service = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                    service.RetourneTousApplicationTaxeCompleted += (s, args) =>
                    {
                        if (args != null && args.Cancelled)
                            return;
                        SessionObject.LstCodeApplicationTaxe = args.Result;
                        LstCodeApplicationTaxe = SessionObject.LstCodeApplicationTaxe;
                    };
                    service.RetourneTousApplicationTaxeAsync();
                    service.CloseAsync();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void RetourneInfoAbon(int fk_idcentre, string centre, string client, string ordre, string produit)
        {
            int res = LoadingManager.BeginLoading(Langue.En_Cours);
            try
            {
                AbonementRecherche = new List<CsAbon>();
                AcceuilServiceClient service = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.RetourneAbonCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    AbonementRecherche = args.Result;
                    if (AbonementRecherche != null)
                    {
                        CsAbon _LeAbonnementProduit = AbonementRecherche.FirstOrDefault(p => p.PRODUIT == produit);
                        if (_LeAbonnementProduit != null)
                        {
                            IsGiserChamp(false);
                            LaDemande.Abonne = _LeAbonnementProduit;
                            AfficherInfoAbonnement(LaDemande.Abonne);
                        }
                        else
                            Message.ShowInformation("Aucune information trouvée", "Recherche d'abonné");
                    }
                    else
                        Message.ShowInformation("Aucune information trouvée", "Recherche d'abonné");
                    LoadingManager.EndLoading(res);
                };
                service.RetourneAbonAsync(fk_idcentre,centre, client, ordre);
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                LoadingManager.EndLoading(res);
                throw ex;
            }
        }
        private void AfficherInfoAbonnement(CsAbon _LeAbonnementdemande)
        {
            try
            {
                

                if (_LeAbonnementdemande.PUISSANCE != null)
                    this.Txt_CodePussanceSoucrite.Text = Convert.ToDecimal(_LeAbonnementdemande.PUISSANCE.ToString()).ToString("N2");
                if (_LeAbonnementdemande.PUISSANCEUTILISEE != null)
                    this.Txt_CodePuissanceUtilise.Text = Convert.ToDecimal(_LeAbonnementdemande.PUISSANCEUTILISEE.Value).ToString("N2");

                this.textBox7.Text = string.IsNullOrEmpty(_LeAbonnementdemande.COEFFAC.ToString()) ? string.Empty : _LeAbonnementdemande.COEFFAC.ToString();
               // NatureCLient = string.IsNullOrEmpty(_LeAbonnementdemande.NATURECLIENT) ? string.Empty : _LeAbonnementdemande.NATURECLIENT;
                this.Txt_NumDemande.Text = string.IsNullOrEmpty(LaDemande.LaDemande.NUMDEM) ? string.Empty : LaDemande.LaDemande.NUMDEM;
                this.Txt_CodeRistoune.Text = string.IsNullOrEmpty(_LeAbonnementdemande.RISTOURNE.ToString()) ? string.Empty : Convert.ToDecimal(_LeAbonnementdemande.RISTOURNE.Value).ToString("N2");
                this.Txt_CodeForfait.Text = string.IsNullOrEmpty(_LeAbonnementdemande.FORFAIT) ? string.Empty : _LeAbonnementdemande.FORFAIT;
                this.Txt_LibelleApplication.Text = string.IsNullOrEmpty(_LeAbonnementdemande.LIBELLEAPPLICATIONTAXE) ? string.Empty : _LeAbonnementdemande.LIBELLEAPPLICATIONTAXE;
                this.Txt_CodeTarif.Text = string.IsNullOrEmpty(_LeAbonnementdemande.TYPETARIF ) ? string.Empty : _LeAbonnementdemande.TYPETARIF;
                this.Txt_CodeFrequence.Text = string.IsNullOrEmpty(_LeAbonnementdemande.PERFAC) ? string.Empty : _LeAbonnementdemande.PERFAC;
                this.Txt_CodeMoisIndex.Text = string.IsNullOrEmpty(_LeAbonnementdemande.MOISREL) ? string.Empty : _LeAbonnementdemande.MOISREL;
                this.Txt_CodeMoisFacturation.Text = string.IsNullOrEmpty(_LeAbonnementdemande.MOISFAC) ? string.Empty : _LeAbonnementdemande.MOISFAC;
                this.Txt_DateAbonnement.Text = (_LeAbonnementdemande.DABONNEMENT == null) ? string.Empty : Convert.ToDateTime(_LeAbonnementdemande.DABONNEMENT.Value).ToShortDateString();
                this.Txt_DateResiliation.Text = (_LeAbonnementdemande.DRES == null) ? string.Empty : Convert.ToDateTime(_LeAbonnementdemande.DRES.Value).ToShortDateString();
                this.Txt_LibelleMoisIndex.Text = string.IsNullOrEmpty(_LeAbonnementdemande.LIBELLEMOISIND) ? string.Empty : _LeAbonnementdemande.LIBELLEMOISIND;
                this.Txt_LibelleFrequence.Text = string.IsNullOrEmpty(_LeAbonnementdemande.LIBELLEFREQUENCE) ? string.Empty : _LeAbonnementdemande.LIBELLEFREQUENCE;
                this.Txt_LibelleTarif.Text = string.IsNullOrEmpty(_LeAbonnementdemande.LIBELLETARIF) ? string.Empty : _LeAbonnementdemande.LIBELLETARIF;
                this.Txt_LibelleForfait.Text = string.IsNullOrEmpty(_LeAbonnementdemande.LIBELLEFORFAIT) ? string.Empty : _LeAbonnementdemande.LIBELLEFORFAIT;
                if (_LeAbonnementdemande.DRES == null) this.Txt_DateResiliation.IsReadOnly = true;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        private void Txt_CodeTarif_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(this.Txt_CodeTarif.Text) &&
                LstTarif != null && LstTarif.Count != 0 &&
                this.Txt_CodeTarif.Text.Length == SessionObject.Enumere.TailleTarif)
            {
                CsTarif _LeTarif = LstTarif.FirstOrDefault(p => p.PRODUIT == LaDemande.LaDemande.PRODUIT && p.CODE  == this.Txt_CodeTarif.Text);
                if (_LeTarif != null)
                {
                    this.Txt_LibelleTarif.Text = _LeTarif.LIBELLE;
                    LeTarifSelect = _LeTarif;
                }
                else
                {
                    var w = new MessageBoxControl.MessageBoxChildWindow(Langue.lbl_Menu, Langue.MsgEltInexistent, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    w.OnMessageBoxClosed += (_, result) =>
                    {
                        this.Txt_CodeTarif.Focus();
                    };
                    w.Show();
                }
            }
        }
        private void Txt_CodeTarif_LostFocus(object sender, RoutedEventArgs e)
        {
            if (this.Txt_CodeTarif.Text.Length != 0)
                this.Txt_CodeTarif.Text = this.Txt_CodeTarif.Text.PadLeft(SessionObject.Enumere.TailleTarif, '0');
        }
        private void btn_tarifs_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (LstTarif.Count != 0)
                {
                    this.btn_tarifs.IsEnabled = false;
                    List<object> _LstObjet = ClasseMEthodeGenerique.RetourneListeObjet(LstTarif.Where(t => t.PRODUIT == this.Txt_CodeProduit.Text).ToList());
                    UcListeGenerique ctr = new UcListeGenerique(_LstObjet, "CODE", "LIBELLE", Langue.lbl_Menu);
                    ctr.Closed += new EventHandler(galatee_OkClickedBtnTarif);
                    ctr.Show();
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Langue.lbl_Menu);
            }
        }
        private void galatee_OkClickedBtnTarif(object sender, EventArgs e)
        {
            try
            {
                this.btn_tarifs.IsEnabled = true;
                UcListeGenerique ctrs = sender as UcListeGenerique;
                if (ctrs.GetisOkClick)
                {
                    CsTarif _LeTarif = (CsTarif)ctrs.MyObject;
                    this.Txt_CodeTarif.Text = _LeTarif.CODE;
                }

            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Langue.lbl_Menu);
            }
        }


        private void btn_PussSouscrite_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (LstPuissance.Count != 0)
                {
                    List<object> _LstObjet = ClasseMEthodeGenerique.RetourneListeObjet(LstPuissance.Where(t => t.CODE == this.Txt_CodeTarif.Text).ToList());
                    UcListeGenerique ctr = new UcListeGenerique(_LstObjet, "CODE", "LIBELLE", "Liste");
                    ctr.Closed += new EventHandler(galatee_OkClickedBtnpuissanceSouscrite);
                    ctr.Show();
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Langue.lbl_Menu);
            }
        }

        private void galatee_OkClickedBtnpuissanceSouscrite(object sender, EventArgs e)
        {
            try
            {
                UcListeGenerique ctrs = sender as UcListeGenerique;
                if (ctrs.GetisOkClick)
                {
                    CsPuissance _LaPuissanceSelect = (CsPuissance)ctrs.MyObject;
                    this.Txt_CodePussanceSoucrite.Text = _LaPuissanceSelect.CODE;
                    this.Txt_CodePuissanceUtilise.Text = _LaPuissanceSelect.CODE;
                }
                else
                    this.btn_PussSouscrite.IsEnabled = true;
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Langue.lbl_Menu);
            }
        }

        private void btn_discount_Click(object sender, RoutedEventArgs e)
        {
            //if (LstDiscount.Count != 0)
            //{
            //    this.btn_discount.IsEnabled = false;
            //    List<object> _LstObject = ClasseMEthodeGenerique.RetourneListeObjet(LstDiscount);
            //    //UcListeGenerique ctr = new UcListeGenerique(_LstObject,LstDiscount[0].p
            //    //ctr.Closed += new EventHandler(galatee_OkClickedBtnDiscount);
            //    //ctr.Show();
            //}
        }
        private void galatee_OkClickedBtnDiscount(object sender, EventArgs e)
        {
            UcListeTa ctrs = sender as UcListeTa;
            this.Txt_CodeRistoune.Text = ctrs.MyElt.VALEUR;
            this.btn_discount.IsEnabled = true;
        }

        private void Txt_CodeForfait_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (LstForfait.Count != 0 && this.Txt_CodeForfait.Text.Length == SessionObject.Enumere.TailleForfait)
                {
                    CsForfait _LeForfait = LstForfait.FirstOrDefault(p => p.FK_IDPRODUIT == LaDemande.LeProduit.PK_ID && p.CODE == this.Txt_CodeForfait.Text);
                    if (_LeForfait != null)
                    {
                        this.Txt_LibelleForfait.Text = _LeForfait.LIBELLE;
                        LeForfaitSelect = _LeForfait;
                    }
                    else
                    {
                        var w = new MessageBoxControl.MessageBoxChildWindow(Langue.lbl_Menu, Langue.MsgEltInexistent, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        w.OnMessageBoxClosed += (_, result) =>
                        {
                            this.Txt_CodeForfait.Focus();
                        };
                        w.Show();
                    }
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Langue.lbl_Menu);
            }
        }
        private void Txt_CodeForfait_LostFocus(object sender, RoutedEventArgs e)
        {
            if (this.Txt_CodeForfait.Text.Length != 0)
                this.Txt_CodeForfait.Text = this.Txt_CodeForfait.Text.PadLeft(SessionObject.Enumere.TailleForfait, '0');
        }

        private void btn_forfait_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (LstForfait.Count != 0)
                {
                    this.btn_forfait.IsEnabled = false;
                    List<object> _LstObjet = ClasseMEthodeGenerique.RetourneListeObjet(LstForfait.Where(t => t.PRODUIT == this.Txt_CodeProduit.Text).ToList());
                    UcListeGenerique ctr = new UcListeGenerique(_LstObjet, "CODE", "LIBELLE", "Liste");
                    ctr.Closed += new EventHandler(galatee_OkClickedBtnForfait);
                    ctr.Show();
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Langue.lbl_Menu);
            }
        }
        private void galatee_OkClickedBtnForfait(object sender, EventArgs e)
        {
            try
            {
                this.btn_forfait.IsEnabled = true;
                UcListeGenerique ctrs = sender as UcListeGenerique;
                if (ctrs.isOkClick)
                {
                    CsForfait _Leforfait = (CsForfait)ctrs.MyObject;
                    this.Txt_CodeForfait.Text = _Leforfait.CODE;
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Langue.lbl_Menu);
            }
        }

        private void btn_frequence_Click(object sender, RoutedEventArgs e)
        {
            this.btn_frequence.IsEnabled = false;
            if (LstFrequence != null && LstFrequence.Count != 0)
            {
                List<object> _ListObj = ClasseMEthodeGenerique.RetourneListeObjet(LstFrequence);
                this.btn_frequence.IsEnabled = false;
                UcListeGenerique ctr = new UcListeGenerique(_ListObj, "CODE", "LIBELLE", "Liste");
                ctr.Closed += new EventHandler(galatee_OkClickedBtnFrequence);
                ctr.Show();
            }
        }
        private void galatee_OkClickedBtnFrequence(object sender, EventArgs e)
        {
            UcListeGenerique ctrs = sender as UcListeGenerique;
            if (ctrs.isOkClick)
            {
                CsFrequence _LaFrequence = (CsFrequence)ctrs.MyObject;
                this.Txt_CodeFrequence.Text = _LaFrequence.CODE;
                this.Txt_LibelleFrequence.Text = _LaFrequence.LIBELLE;
                this.Txt_CodeFrequence.Tag = _LaFrequence.PK_ID;
            }
            this.btn_frequence.IsEnabled = true;
        }

        private void Txt_CodeMoisFacturation_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (LstMois.Count != 0 && this.Txt_CodeMoisFacturation.Text.Length == SessionObject.Enumere.TailleMoisDeFacturation)
                {
                    CsMois _LeMois = ClasseMEthodeGenerique.RetourneObjectFromList(LstMois, this.Txt_CodeMoisFacturation.Text, "CODE");
                    if (_LeMois != null)
                    {
                        this.Txt_LibMoisFact.Text = _LeMois.LIBELLE;
                        LeMoisFactSelect = _LeMois;
                        //EnregisterDemande(LaDemande);
                    }
                    else
                    {
                        var w = new MessageBoxControl.MessageBoxChildWindow(Langue.lbl_Menu, Langue.MsgEltInexistent, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        w.OnMessageBoxClosed += (_, result) =>
                        {
                            this.Txt_CodeMoisFacturation.Focus();
                        };
                        w.Show();
                    }
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Langue.lbl_Menu);
            }
        }
        private void Txt_CodeMoisFacturation_LostFocus(object sender, RoutedEventArgs e)
        {
            if (this.Txt_CodeMoisFacturation.Text.Length != 0)
                this.Txt_CodeMoisFacturation.Text = this.Txt_CodeMoisFacturation.Text.PadLeft(SessionObject.Enumere.TailleMoisDeFacturation, '0');
        }
        private void btn_moisdefacturation_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (LstMois.Count != 0)
                {
                    this.btn_moisdefacturation.IsEnabled = false;
                    List<object> _LstOject = ClasseMEthodeGenerique.RetourneListeObjet(LstMois);
                    UcListeGenerique ctr = new UcListeGenerique(_LstOject, "CODE", "LIBELLE", "Liste");
                    ctr.Closed += new EventHandler(galatee_OkClickedBtnMoisFact);
                    ctr.Show();
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Langue.lbl_Menu);
            }
        }
        private void galatee_OkClickedBtnMoisFact(object sender, EventArgs e)
        {
            try
            {
                UcListeGenerique ctrs = sender as UcListeGenerique;
                if (ctrs.isOkClick)
                {
                    CsMois _LeMois = (CsMois)ctrs.MyObject;
                    this.Txt_CodeMoisFacturation.Text = _LeMois.CODE;
                }
                this.btn_moisdefacturation.IsEnabled = true;
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Langue.lbl_Menu);
            }
        }


        private void Txt_CodeMoisIndex_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (LstMois.Count != 0 && this.Txt_CodeMoisIndex.Text.Length == SessionObject.Enumere.TailleMoisDeFacturation)
                {
                    CsMois _LeMois = ClasseMEthodeGenerique.RetourneObjectFromList(LstMois, this.Txt_CodeMoisIndex.Text, "CODE");
                    if (_LeMois != null)
                    {
                        this.Txt_LibelleMoisIndex.Text = _LeMois.LIBELLE;
                        LeIndexFactSelect = _LeMois;

                        //EnregisterDemande(LaDemande);
                    }
                    else
                    {
                        var w = new MessageBoxControl.MessageBoxChildWindow(Langue.lbl_Menu, Langue.MsgEltInexistent, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        w.OnMessageBoxClosed += (_, result) =>
                        {
                            this.Txt_CodeMoisFacturation.Focus();
                        };
                        w.Show();
                    }
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Langue.lbl_Menu);
            }
        }
        private void btn_MoisIndex_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (LstMois.Count != 0)
                {
                    this.btn_MoisIndex.IsEnabled = false;
                    List<object> _LstOject = ClasseMEthodeGenerique.RetourneListeObjet(LstMois);
                    UcListeGenerique ctr = new UcListeGenerique(_LstOject, "CODE", "LIBELLE", "Liste");
                    ctr.Closed += new EventHandler(galatee_OkClickedBtnMoisIndex);
                    ctr.Show();
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Langue.lbl_Menu);
            }
        }
        private void galatee_OkClickedBtnMoisIndex(object sender, EventArgs e)
        {
            try
            {
                UcListeGenerique ctrs = sender as UcListeGenerique;
                if (ctrs.isOkClick)
                {
                    CsMois _LeMois = (CsMois)ctrs.MyObject;
                    this.Txt_CodeMoisIndex.Text = _LeMois.CODE;
                }
                this.btn_MoisIndex.IsEnabled = true;
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Langue.lbl_Menu);
            }
        }



        private void Txt_DateAbonnement_TextChanged(object sender, TextChangedEventArgs e)
        {

            try
            {
                if (this.Txt_DateAbonnement.Text.Length == SessionObject.Enumere.TailleDate)
                    if (ClasseMEthodeGenerique.IsDateValide(this.Txt_DateAbonnement.Text) == null)
                    {
                        var w = new MessageBoxControl.MessageBoxChildWindow(Langue.lbl_Menu, "Date invalide", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        w.OnMessageBoxClosed += (_, result) =>
                        {
                            this.Txt_DateAbonnement.Focus();
                        };
                        w.Show();
                    }
                //else 
                //EnregisterDemande(LaDemande);
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Langue.lbl_Menu);
            }
        }

        private void Txt_DateResiliation_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (this.Txt_DateResiliation.Text.Length == SessionObject.Enumere.TailleDate)
                    if (ClasseMEthodeGenerique.IsDateValide(this.Txt_DateResiliation.Text) == null)
                    {
                        var w = new MessageBoxControl.MessageBoxChildWindow(Langue.lbl_Menu, "Date invalide", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        w.OnMessageBoxClosed += (_, result) =>
                        {
                            this.Txt_DateResiliation.Focus();
                        };
                        w.Show();
                    }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Langue.lbl_Menu);

            }
        }

        private void Txt_CodePussanceSoucrite_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (LstPuissance != null && LstPuissance.Count != 0)
            //&& this.Txt_CodePussanceSoucrite .Text.Length == SessionObject.Enumere.)
            {

                CsPuissance _LaPuissance = ClasseMEthodeGenerique.RetourneObjectFromList(LstPuissance, this.Txt_CodePussanceSoucrite.Text, "PK_PUISSANCE");
                if (_LaPuissance != null)
                {
                    //EnregisterDemande(LaDemande);
                }
                else
                {
                    var w = new MessageBoxControl.MessageBoxChildWindow(Langue.lbl_Menu, Langue.MsgEltInexistent, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    w.OnMessageBoxClosed += (_, result) =>
                    {
                        this.Txt_CodePussanceSoucrite.Focus();
                    };
                    w.Show();
                }
            }
        }

        private void Txt_CodeFrequence_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (LstFrequence.Count != 0 && this.Txt_CodeFrequence.Text.Length == SessionObject.Enumere.TailleMoisDeFacturation)
                {
                    CsFrequence _LaFrequence = ClasseMEthodeGenerique.RetourneObjectFromList(LstFrequence, this.Txt_CodeFrequence.Text, "CODE");
                    if (_LaFrequence != null)
                    {
                        this.Txt_LibelleFrequence.Text = _LaFrequence.LIBELLE;
                        //EnregisterDemande(LaDemande);
                    }
                    else
                    {
                        var w = new MessageBoxControl.MessageBoxChildWindow(Langue.lbl_Menu, Langue.MsgEltInexistent, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        w.OnMessageBoxClosed += (_, result) =>
                        {
                            this.Txt_CodeFrequence.Focus();
                        };
                        w.Show();
                    }
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Langue.lbl_Menu);
            }
        }
        private void btn_Site_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (lstSite.Count > 0)
                {
                    this.btn_Site.IsEnabled = false;
                    List<object> _Listgen = ClasseMEthodeGenerique.RetourneListeObjet(lstSite);
                    UcListeGenerique ctr = new UcListeGenerique(_Listgen, "CODESITE", "LIBELLE", "Liste");
                    ctr.Closed += new EventHandler(galatee_OkClickedSite);
                    ctr.Show();
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Langue.lbl_Menu);
            }

        }


        List<CsSite> lstSite = new List<CsSite>();
        //private void ChargerDonneeDuSite()
        //{
        //    try
        //    {
        //        AcceuilServiceClient service = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
        //          service.ListeDesDonneesDesSiteAsync(false);
        //        service.ListeDesDonneesDesSiteCompleted += (s, args) =>
        //        {
        //            if (args != null && args.Cancelled)
        //                return;
        //            SessionObject.LstCentre = args.Result;
        //            LstCentre = Shared.ClasseMEthodeGenerique.RetourCentreByPerimetre(SessionObject.LstCentre.Where(p => p.CODE != SessionObject.Enumere.Generale).ToList(), UserConnecte.listeProfilUser);
        //            lstSite = ClasseMEthodeGenerique.RetourneSiteByCentre(LstCentre);
        //            if (lstSite != null)
        //            {
        //                List<CsSite> _LstSite = lstSite.Where(p => p.CODESITE != SessionObject.Enumere.Generale).ToList();
        //                if (_LstSite.Count == 1 && !IsUpdate)
        //                {
        //                    this.Txt_CodeSite.Text = _LstSite[0].CODESITE;
        //                    this.Txt_LibelleSite.Text = _LstSite[0].LIBELLE;
        //                    this.btn_Site.IsEnabled = false;
        //                    this.Txt_CodeSite.IsReadOnly = true;
        //                }
        //            }
        //            if (LstCentre != null)
        //            {
        //                List<CsCentre> _LstCentre = LstCentre.Where(p => p.CODE != SessionObject.Enumere.Generale).ToList();
        //                if (_LstCentre.Count == 1)
        //                {
        //                    this.Txt_CodeCentre.Text = _LstCentre[0].CODE;
        //                    this.Txt_LibelleCentre.Text = _LstCentre[0].LIBELLE;
        //                }
        //                else
        //                {
        //                    CsCentre _LeCentre = new CsCentre();
        //                    if (!string.IsNullOrEmpty(this.Txt_CodeCentre.Text))
        //                        _LeCentre = LstCentre.FirstOrDefault(p => p.CODE == this.Txt_CodeCentre.Text);
        //                    if (_LeCentre != null && !string.IsNullOrEmpty(_LeCentre.CODE))
        //                    {
        //                        this.Txt_LibelleCentre.Text = _LeCentre.LIBELLE;
        //                        this.btn_Centre.IsEnabled = false;
        //                        this.Txt_CodeCentre.IsReadOnly = true;
        //                    }
        //                }
        //            }
        //        };
        //        service.ListeDesDonneesDesSiteAsync(true);
        //        service.CloseAsync();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        private void RemplirCentreDuSite(int pIdSite, int pIdcentre)
        {
            try
            {
                Cbo_Centre.Items.Clear();
                if (_listeDesCentreExistant != null &&
                    _listeDesCentreExistant.Count != 0)
                {
                    List<CsCentre> lesCentreDuPerimetreAction = _listeDesCentreExistant.Where(p => p.FK_IDCODESITE == pIdSite).ToList();
                    if (lesCentreDuPerimetreAction != null)
                        foreach (var item in lesCentreDuPerimetreAction)
                        {
                            Cbo_Centre.Items.Add(item);
                        }
                    //Cbo_Centre.ItemsSource = lesCentreDuPerimetreAction;
                    Cbo_Centre.SelectedValuePath = "PK_ID";
                    Cbo_Centre.DisplayMemberPath = "LIBELLE";

                    if (pIdcentre != 0)
                        this.Cbo_Centre.SelectedItem = _listeDesCentreExistant.First(t => t.PK_ID == pIdcentre);
                    if (_listeDesCentreExistant.Count == 1)
                        this.Cbo_Centre.SelectedItem = _listeDesCentreExistant.First();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void RemplirCentrePerimetre(List<CsCentre> lstCentre, List<CsSite> lstSite)
        {
            try
            {
                Cbo_Centre.Items.Clear();
                if (_listeDesCentreExistant != null &&
                    _listeDesCentreExistant.Count != 0)
                {
                    if (lstCentre != null)
                    {
                        LstCentre = lstCentre;
                        foreach (var item in lstCentre)
                        {
                            Cbo_Centre.Items.Add(item);
                        }
                    }
                    Cbo_Centre.SelectedValuePath = "PK_ID";
                    Cbo_Centre.DisplayMemberPath = "LIBELLE";

                    if (lstSite != null)
                        foreach (var item in lstSite)
                        {
                            Cbo_Site.Items.Add(item);
                        }
                    Cbo_Site.SelectedValuePath = "PK_ID";
                    Cbo_Site.DisplayMemberPath = "LIBELLE";

                    if (lstSite != null && lstSite.Count == 1)
                        Cbo_Site.SelectedItem = lstSite.First();

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        List<ServiceAccueil.CsCentre> lesCentre = new List<ServiceAccueil.CsCentre>();
        List<ServiceAccueil.CsSite> lesSite = new List<ServiceAccueil.CsSite>();
        void ChargerListDesSite()
        {
            try
            {

                if (SessionObject.LstCentre != null && SessionObject.LstCentre.Count != 0)
                {
                    lesCentre = Shared.ClasseMEthodeGenerique.RetourCentreByPerimetre(SessionObject.LstCentre, UserConnecte.listeProfilUser);
                    lesSite = Shared.ClasseMEthodeGenerique.RetourneSiteByCentre(lesCentre);
                    _listeDesCentreExistant = lesCentre;
                    RemplirCentrePerimetre(lesCentre, lesSite);
                    return;
                }
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
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
                            RemplirCentrePerimetre(lesCentre, lesSite);
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





        private void ChargerListeDeProduit()
        {
            try
            {
                if (SessionObject.ListeDesProduit.Count != 0)
                {
                    ListeDesProduitDuSite = SessionObject.ListeDesProduit;
                    if (ListeDesProduitDuSite != null)
                    {
                        if (ListeDesProduitDuSite.Count == 1)
                        {
                            this.Txt_CodeProduit.Text = ListeDesProduitDuSite[0].CODE;
                            this.Txt_LibelleProduit.Text = ListeDesProduitDuSite[0].LIBELLE;
                            this.btn_Produit.IsEnabled = false;
                        }
                    }

                }
                else
                {
                    AcceuilServiceClient service1 = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                    service1.ListeDesProduitCompleted += (sr, res) =>
                    {
                        if (res != null && res.Cancelled)
                            return;
                        SessionObject.ListeDesProduit = res.Result;
                        ListeDesProduitDuSite = SessionObject.ListeDesProduit;
                        if (ListeDesProduitDuSite != null)
                        {
                            if (ListeDesProduitDuSite.Count == 1)
                            {
                                this.Txt_CodeProduit.Text = ListeDesProduitDuSite[0].CODE;
                                this.Txt_LibelleProduit.Text = ListeDesProduitDuSite[0].LIBELLE;
                                this.btn_Produit.IsEnabled = false;
                            }
                            else
                            {
                                CsProduit _LeProduit = ListeDesProduitDuSite.FirstOrDefault(p => p.CODE == LaDemande.LaDemande.PRODUIT);
                                if (_LeProduit != null)
                                {
                                    this.Txt_CodeProduit.Text = LaDemande.LaDemande.PRODUIT;
                                    this.Txt_LibelleProduit.Text = _LeProduit.LIBELLE;
                                    this.btn_Produit.IsEnabled = false;
                                    this.Txt_CodeProduit.IsReadOnly = true;
                                }
                            }
                        }
                    };
                    service1.ListeDesProduitAsync();
                    service1.CloseAsync();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        //private void ChargerTypeBranchement()
        //{
        //    try
        //    {
        //        if (SessionObject.LstTypeBranchement.Count != 0)
        //            LstTypeBranchement = SessionObject.LstTypeBranchement;
        //        else
        //        {
        //            AcceuilServiceClient service1 = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
        //            service1.RetourneTypeBranchementCompleted += (sr, res) =>
        //            {
        //                if (res != null && res.Cancelled)
        //                    return;
        //                SessionObject.LstTypeBranchement = res.Result;
        //                LstTypeBranchement = SessionObject.LstTypeBranchement;

        //            };
        //            service1.RetourneTypeBranchementAsync();
        //            service1.CloseAsync();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;

        //    }
        //}
        private void ValidationDemande(CsDemande _LaDemande)
        {
            try
            {
                //Lancer la transaction de mise a jour en base
                AcceuilServiceClient service1 = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                //service1.ValiderDemandeWithObjectAsync(_LaDemande, leDoc);
                //service1.ValiderDemandeWithObjectCompleted += (sr, res) =>
                service1.ValiderDemandeInitailisationAsync(_LaDemande);
                service1.ValiderDemandeInitailisationCompleted += (sr, res) =>
                {
                    string Messages = string.Empty;
                    //Si la date de d'encaissement n'est pas renseigné c-a-d que la demende est en attente
                    if (_LaDemande.LaDemande.STATUT == SessionObject.Enumere.DemandeStatusEnAttente)
                        //Msg de confirmation de l'enregistremet
                        Messages = Langue.MsgRequestSaved;
                    //Si la date d'encaissement est renseigné 
                    else
                        Messages = Langue.MsgOperationTerminee;
                    if (!string.IsNullOrEmpty(res.Result))
                    {
                        string Retour = res.Result;
                        string[] coupe = Retour.Split('.');
                        Shared.ClasseMEthodeGenerique.InitWOrkflow(coupe[0], _LaDemande.LaDemande.FK_IDCENTRE, coupe[1], _LaDemande.LaDemande.FK_IDTYPEDEMANDE);
                    }
                    Message.ShowInformation(Messages, Langue.lbl_Menu);
                     if (Closed != null)
                        Closed(this, new EventArgs());
                    this.DialogResult = false;
                };
                service1.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;

            }
        }
        public event EventHandler Closed;
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (EnregisterDemande(LaDemande))
                ValidationDemande(LaDemande);
            //if (Closed != null)
            //    Closed(this, new EventArgs());

        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void Txt_CodeCentre_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (this.Txt_CodeCentre.Text.Length == SessionObject.Enumere.TailleCentre)
            {
                CsCentre _LeCentre = ClasseMEthodeGenerique.RetourneObjectFromList<CsCentre>(LstCentre, this.Txt_CodeCentre.Text, "CODE");
                if (_LeCentre != null && !string.IsNullOrEmpty(_LeCentre.CODE))
                {
                    //this.Txt_LibelleCentre.Text = _LeCentre.LIBELLE;
                    LaDemande.LaDemande.CENTRE = _LeCentre.CODE; ;
                    LaDemande.LaDemande.FK_IDCENTRE = _LeCentre.PK_ID;
                    string numIncrementiel = _LeCentre.NUMDEM.ToString();
                    if (_LeCentre.NUMDEM.ToString().Length >= 10)
                        numIncrementiel = _LeCentre.NUMDEM.ToString().Substring(numIncrementiel.Length - 9, 9);
                    this.Txt_NumDemande.Text = _LeCentre.CODE + numIncrementiel.PadLeft(10, '0');
                    LaDemande.LeCentre = LeCentreSelect;
                    LaDemande.LaDemande.MATRICULE = UserConnecte.matricule;

                }
                else
                {
                    var w = new MessageBoxControl.MessageBoxChildWindow(Langue.lbl_Menu, Langue.MsgEltInexistent, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    w.OnMessageBoxClosed += (_, result) =>
                    {
                        this.Txt_CodeCentre.Focus();
                    };
                    w.Show();
                }
            }
        }
        private void btn_Centre_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (LstCentre.Count > 0)
                {
                    this.btn_Centre.IsEnabled = false;
                    List<object> _Listgen = ClasseMEthodeGenerique.RetourneListeObjet(LstCentre);
                    UcListeGenerique ctr = new UcListeGenerique(_Listgen, "CODE", "LIBELLE", Langue.lbl_ListeCentre);
                    ctr.Closed += new EventHandler(galatee_OkClickedCentre);
                    ctr.Show();
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Langue.lbl_Menu);
            }

        }
        void galatee_OkClickedCentre(object sender, EventArgs e)
        {
            this.btn_Centre.IsEnabled = true;
            LeCentreSelect = new CsCentre();
            UcListeGenerique ctrs = sender as UcListeGenerique;
            if (ctrs.isOkClick)
            {
                CsCentre leCentre = (CsCentre)ctrs.MyObject;
                LeCentreSelect = leCentre;
                this.Txt_CodeCentre.Text = leCentre.CODE;

                string numIncrementiel = LeCentreSelect.NUMDEM.ToString();
                if (LeCentreSelect.NUMDEM.ToString().Length >= 10)
                    numIncrementiel = LeCentreSelect.NUMDEM.ToString().Substring(numIncrementiel.Length - 9, 9);
                this.Txt_NumDemande.Text = LeCentreSelect.CODE + numIncrementiel.PadLeft(10, '0');
                LaDemande.LaDemande.FK_IDCENTRE = LeCentreSelect.PK_ID;

            }
        }

        private void btn_Produit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                btn_Produit.IsEnabled = false;
                List<object> _LstProduit = ClasseMEthodeGenerique.RetourneListeObjet(ListeDesProduitDuSite);
                UcListeGenerique ctr = new UcListeGenerique(_LstProduit, "CODE", "LIBELLE", Langue.lbl_ListeProduit);
                ctr.Closed += new EventHandler(galatee_OkClickedProduit);
                ctr.Show();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Langue.lbl_Menu);
            }
        }
        void galatee_OkClickedProduit(object sender, EventArgs e)
        {
            try
            {
                UcListeGenerique ctrs = sender as UcListeGenerique;
                if (ctrs.isOkClick)
                {
                    LeProduitSelect = (CsProduit)ctrs.MyObject;
                    this.Txt_CodeProduit.Text = LeProduitSelect.CODE;
                    IsGiserChamp(false);
                }
                btn_Produit.IsEnabled = true  ;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        private void Txt_CodeProduit_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (this.Txt_CodeProduit.Text.Length == SessionObject.Enumere.TailleCodeProduit)
                {
                    LaDemande.LaDemande.PRODUIT = this.Txt_CodeProduit.Text;
                    CsProduit _LeProduitSelect = ClasseMEthodeGenerique.RetourneObjectFromList(ListeDesProduitDuSite, this.Txt_CodeProduit.Text, "CODE");
                    if (!string.IsNullOrEmpty(_LeProduitSelect.LIBELLE))
                    {
                        CsTypeBranchement _LeTypeBrtProduitSelect = LstTypeBranchement.FirstOrDefault(p => (p.CENTRE == LaDemande.LaDemande.CENTRE || p.CENTRE == SessionObject.Enumere.Generale) &&
                                                                                                          p.PRODUIT == _LeProduitSelect.CODE);
                        if (_LeTypeBrtProduitSelect != null && !string.IsNullOrEmpty(_LeTypeBrtProduitSelect.PRODUIT))
                            LaDemande.LeTypeBranchement = _LeTypeBrtProduitSelect;

                        this.Txt_LibelleProduit.Text = _LeProduitSelect.LIBELLE;
                        LaDemande.LeProduit = _LeProduitSelect;
                        LaDemande.LaDemande.FK_IDPRODUIT = _LeProduitSelect.PK_ID;

                    }
                    else
                    {
                        var w = new MessageBoxControl.MessageBoxChildWindow(Langue.lbl_Menu, Langue.MsgEltInexistent, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        w.OnMessageBoxClosed += (_, result) =>
                        {
                            this.Txt_CodeProduit.Focus();
                        };
                        w.Show();
                    }
                }
            }
            catch (Exception ex)
            {

                Message.ShowError(ex.Message, Langue.lbl_Menu);
            }

        }
        private void btn_Rechercher_Click_1(object sender, RoutedEventArgs e)
        {
            //if (!SessionObject.Enumere.IsModificationAutoriserEnFacturation)
            //    VerifieExisteDemande(this.Txt_CodeCentre.Text, this.Txt_Client.Text, this.Txt_Ordre.Text, LaDemande.LaDemande.FK_IDCENTRE, TypeDemande);
            //else
            //    VerifieDernierEvt(this.Txt_CodeCentre.Text, this.Txt_Client.Text, this.Txt_Ordre.Text);



                        if (!SessionObject.Enumere.IsModificationAutoriserEnFacturation)
                RetourneOrdre(this.Txt_CodeCentre.Text, this.Txt_Client.Text, this.Txt_CodeProduit.Text);
            else
                VerifieDernierEvt(this.Txt_CodeCentre.Text, this.Txt_Client.Text, string.Empty);
        }
        private void RetourneOrdre(string centre, string client, string produit)
        {
            try
            {
                string OrdreMax = string.Empty;
                AcceuilServiceClient service = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.RetourneOrdreMaxCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    OrdreMax = args.Result;
                    this.lbl_Ordre.Visibility = System.Windows.Visibility.Visible;
                    this.Txt_Ordre.Visibility = System.Windows.Visibility.Visible;
                    this.Txt_Ordre.IsReadOnly = true;
                    this.Txt_Ordre.Text = OrdreMax;
                    VerifieExisteDemande(this.Txt_CodeCentre.Text, this.Txt_Client.Text, this.Txt_Ordre.Text, ((CsCentre)Txt_CodeCentre.Tag).PK_ID, TypeDemande);
                };
                service.RetourneOrdreMaxAsync(centre, client, produit);
                service.CloseAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void VerifieDernierEvt(string centre, string client, string Ordre)
        {

            if (!string.IsNullOrEmpty(Txt_Client.Text) && Txt_Client.Text.Length == SessionObject.Enumere.TailleClient)
            {
             
                    LaDemande.LaDemande.CLIENT = Txt_Client.Text;
                    string OrdreMax = string.Empty;
                    AcceuilServiceClient service = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                    service.IsDernierEvtEnFacturationCompleted += (s, args) =>
                    {
                        if (args != null && args.Cancelled)
                            return;
                        if (args.Result != null)
                        {
                            if (args.Result == true)
                            {
                                Message.ShowError(Langue.msgFacturationEnCours, "Accueil");
                                return;
                            }
                            //VerifieExisteDemande(this.Txt_CodeCentre.Text, this.Txt_Client.Text, this.Txt_Ordre.Text, ((CsCentre)Txt_CodeCentre.Tag).PK_ID, TypeDemande);
                            VerifieExisteDemande(this.Txt_CodeCentre.Text, this.Txt_Client.Text, this.Txt_Ordre.Text, ((CsCentre)Txt_CodeCentre.Tag).PK_ID, SessionObject.Enumere.ModificationAbonnement);

                        }
                    };
                    service.IsDernierEvtEnFacturationAsync(centre, client, Ordre);
                    service.CloseAsync();
                }

        }

        private void VerifieExisteDemande(string centre, string client, string Ordre, int idCentre, string tdem)
        {

            if (!string.IsNullOrEmpty(Txt_Client.Text) && Txt_Client.Text.Length == SessionObject.Enumere.TailleClient)
            {
                if (!IsUpdate)
                {
                    LaDemande.LaDemande.CLIENT = Txt_Client.Text;
                    string OrdreMax = string.Empty;
                    AcceuilServiceClient service = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                    service.RetourneDemandeTypesClientCompleted += (s, args) =>
                    {
                        if (args != null && args.Cancelled)
                            return;
                        if (args.Result != null)
                        {
                            LaDemande = args.Result;
                            //LaDemande.LaDemande.NUMDEM = string.Empty;
                            LaDemande.LaDemande.PK_ID = 0;
                            //if (args.Result.STATUT != SessionObject.Enumere.DemandeStatusPriseEnCompte && args.Result.ISSUPPRIME != true)
                            //{
                            //    Message.ShowError("Il existe déja une demande de ce type sur ce client", "Accueil");
                            //    return;
                            //}
                        }
                        //RetourneInfoAbon(((CsCentre)Txt_CodeCentre.Tag).PK_ID, this.Txt_CodeCentre.Text, this.Txt_Client.Text, this.Txt_Ordre.Text, this.Txt_CodeProduit.Text);

                        AfficherInfoAbonnement(LaDemande.Abonne);
                    };
                    service.RetourneDemandeTypesClientAsync(centre, client, Ordre, idCentre, tdem);
                    service.CloseAsync();
                }
            }

        }
        
        
        private void lnkLetter_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (lnkLetter.Tag == null)
                {
                    var openDialog = new OpenFileDialog();
                    //openDialog.Filter = "Text Files (*.txt)|*.txt";
                    openDialog.Filter = "Image files (*.jpg; *.jpeg; *.png; *.bmp) | *.jpg; *.jpeg; *.png; *.bmp";
                    openDialog.Multiselect = true;
                    bool? userClickedOK = openDialog.ShowDialog();
                    if (userClickedOK == true)
                    {
                        if (openDialog.Files != null && openDialog.Files.Count() > 0 && openDialog.File != null)
                        {
                            FileStream stream = openDialog.File.OpenRead();
                            var memoryStream = new MemoryStream();
                            stream.CopyTo(memoryStream);
                            lnkLetter.Tag = memoryStream.GetBuffer();
                            var formScanne = new UcImageScanne(memoryStream, SessionObject.ExecMode.Creation);
                            formScanne.Closed += new EventHandler(GetInformationFromChildWindowImageAutorisation);
                            formScanne.Show();

                            this.btn_Supprime.Visibility = System.Windows.Visibility.Visible;
                            this.btn_Modifier.Visibility = System.Windows.Visibility.Visible;
                        }
                    }
                }

                else
                {
                    MemoryStream memoryStream = new MemoryStream(lnkLetter.Tag as byte[]);
                    var ucImageScanne = new UcImageScanne(memoryStream, SessionObject.ExecMode.Modification);
                    ucImageScanne.Show();
                }

            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Galatee.Silverlight.Resources.Devis.Languages.txtDevis);
            }
        }
        private ObjDOCUMENTSCANNE SaveFile(byte[] pStream, int pTypeDocument, ObjDOCUMENTSCANNE pDocumentScane)
        {
            try
            {
                //Récupération du contenu.
                if (pDocumentScane == null)
                {
                    pDocumentScane = new ObjDOCUMENTSCANNE { CONTENU = pStream, PK_ID = Guid.NewGuid(), DATECREATION = DateTime.Now, USERCREATION = UserConnecte.matricule };
                    pDocumentScane.OriginalPK_ID = pDocumentScane.PK_ID;
                    pDocumentScane.ISNEW = true;
                    if (LaDemande.LaDemande != null)
                        LaDemande.LaDemande.FICHIERJOINT = pDocumentScane.PK_ID;
                }
                else
                    pDocumentScane.CONTENU = pStream;
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Galatee.Silverlight.Resources.Devis.Languages.txtDevis);
            }
            return pDocumentScane;
        }
        private void GetInformationFromChildWindowImageAutorisation(object sender, EventArgs e)
        {
            try
            {
                var form = (UcImageScanne)sender;
                if (form != null)
                {
                    if (form.DialogResult == true /*&& form.ImageScannee != null*/)
                    {
                        this.lnkLetter.Content = Langue.msgVoirPiecejointe;
                        //this.lnkLetter.Tag = form.ImageScannee;
                        //SaveFile(form.ImageScannee, (int)SessionObject.TypeDocumentScanneDevis.Lettre);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void btn_taxeApplication_Click(object sender, RoutedEventArgs e)
        {

        }
        private void Cbo_Site_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (this.Cbo_Site.SelectedItem != null)
                {
                    var csSite = Cbo_Site.SelectedItem as CsSite;
                    if (csSite != null)
                    {
                        this.Txt_CodeSite.Text = csSite.CODESITE ?? string.Empty;
                        if (laDemandeSelect != null)
                        {
                            if (laDemandeSelect.FK_IDCENTRE != 0)
                                RemplirCentreDuSite(csSite.PK_ID, laDemandeSelect.FK_IDCENTRE);
                        }
                        else
                            RemplirCentreDuSite(csSite.PK_ID, 0);

                    }
                }
                //VerifierDonneesSaisiesInformationsDevis();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Language.ToString());
            }
        }
       
        private void RemplireLibelle()
        {
            if (this.Txt_CodeCentre.Text.Length == SessionObject.Enumere.TailleCentre)
            {
                CsCentre _LeCentre = ClasseMEthodeGenerique.RetourneObjectFromList<CsCentre>(SessionObject.LstCentre, this.Txt_CodeCentre.Text, "CODE");
                if (_LeCentre != null && !string.IsNullOrEmpty(_LeCentre.CODE))
                {
                    //this.Txt_LibelleCentre.Text = _LeCentre.LIBELLE;
                    this.Txt_CodeSite.Text = _LeCentre.CODESITE;
                    //this.Txt_LibelleSite.Text = _LeCentre.LIBELLESITE;
                }
            }

            if (this.Txt_CodeProduit.Text.Length == SessionObject.Enumere.TailleCodeProduit)
            {
                CsProduit _LeProduitSelect = ClasseMEthodeGenerique.RetourneObjectFromList(SessionObject.ListeDesProduit , this.Txt_CodeProduit.Text, "CODE");
                if (!string.IsNullOrEmpty(_LeProduitSelect.LIBELLE))
                    this.Txt_LibelleProduit.Text = _LeProduitSelect.LIBELLE;
            }
            if (LstForfait.Count != 0 && this.Txt_CodeForfait.Text.Length == SessionObject.Enumere.TailleForfait)
            {
                CsForfait _LeForfait = LstForfait.FirstOrDefault(p => p.FK_IDPRODUIT == LaDemande.Abonne.FK_IDPRODUIT && p.CODE == this.Txt_CodeForfait.Text);
                if (_LeForfait != null)
                    this.Txt_LibelleForfait.Text = _LeForfait.LIBELLE;
            }
            if (LstFrequence.Count != 0 && this.Txt_CodeFrequence.Text.Length == SessionObject.Enumere.TailleMoisDeFacturation)
            {
                CsFrequence _LaFrequence = ClasseMEthodeGenerique.RetourneObjectFromList(LstFrequence, this.Txt_CodeFrequence.Text, "CODE");
                if (_LaFrequence != null)
                    this.Txt_LibelleFrequence.Text = _LaFrequence.LIBELLE;
            }
            if (LstMois.Count != 0 && this.Txt_CodeMoisFacturation.Text.Length == SessionObject.Enumere.TailleMoisDeFacturation)
            {
                CsMois _LeMois = ClasseMEthodeGenerique.RetourneObjectFromList(LstMois, this.Txt_CodeMoisFacturation.Text, "CODE");
                if (_LeMois != null)
                    this.Txt_LibMoisFact.Text = _LeMois.LIBELLE;
            }
            if (LstMois.Count != 0 && this.Txt_CodeMoisIndex.Text.Length == SessionObject.Enumere.TailleMoisDeFacturation)
            {
                CsMois _LeMois = ClasseMEthodeGenerique.RetourneObjectFromList(LstMois, this.Txt_CodeMoisIndex.Text, "CODE");
                if (_LeMois != null)
                    this.Txt_LibelleMoisIndex.Text = _LeMois.LIBELLE;
            }
            if (!string.IsNullOrEmpty(this.Txt_CodeTarif.Text) &&
               LstTarif != null && LstTarif.Count != 0 &&
               this.Txt_CodeTarif.Text.Length == SessionObject.Enumere.TailleTarif)
            {
                CsTarif _LeTarif = LstTarif.FirstOrDefault(p => p.FK_IDPRODUIT == LaDemande.Abonne.FK_IDPRODUIT && p.CODE == this.Txt_CodeTarif.Text);
                if (_LeTarif != null)
                {
                    this.Txt_LibelleTarif.Text = _LeTarif.LIBELLE;
                    LeTarifSelect = _LeTarif;
                }
            }

        }

        private void lnkMotif_Click(object sender, RoutedEventArgs e)
        {
            Message.ShowInformation(LaDemande.LaDemande.ANNOTATION, "Motif réjet");
        }

        private void Txt_Ordre_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(this.Txt_Ordre.Text))
                this.Txt_Ordre.Text = this.Txt_Ordre.Text.PadLeft(SessionObject.Enumere.TailleOrdre, '0');
        }
        private void Txt_CodeCentre_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(this.Txt_CodeCentre.Text))
                this.Txt_CodeCentre.Text = this.Txt_CodeCentre.Text.PadLeft(SessionObject.Enumere.TailleOrdre, '0');
        }
        private void Txt_Client_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(this.Txt_Client.Text))
                this.Txt_Client.Text = this.Txt_Client.Text.PadLeft(SessionObject.Enumere.TailleClient, '0');
        }

        private void btn_Supprimer_click(object sender, RoutedEventArgs e)
        {
            if (lnkLetter.Tag != null)
            {
                var w = new MessageBoxControl.MessageBoxChildWindow(Langue.lbl_Menu, Langue.msgSuppressionFichier, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                w.OnMessageBoxClosed += (_, result) =>
                {
                    if (w.Result == MessageBoxResult.OK)
                    {
                        if (LaDemande != null && LaDemande.LaDemande != null && LaDemande.LaDemande.FICHIERJOINT != new Guid("00000000-0000-0000-0000-000000000000"))
                            LaDemande.LaDemande.FICHIERJOINT = new Guid("00000000-0000-0000-0000-000000000000");

                        this.btn_Supprime.Visibility = System.Windows.Visibility.Collapsed;
                        this.btn_Modifier.Visibility = System.Windows.Visibility.Collapsed;
                        lnkLetter.Tag = null;
                        this.lnkLetter.Content = "Motif de la modification";
                    }
                };
                w.Show();
            }
            else
                Message.ShowInformation(Langue.msgAucunfichier, Langue.lbl_Menu);

        }

        private void btn_Modifier_click(object sender, RoutedEventArgs e)
        {
            try
            {
                var openDialog = new OpenFileDialog();
                //openDialog.Filter = "Text Files (*.txt)|*.txt";
                openDialog.Filter = "Image files (*.jpg; *.jpeg; *.png; *.bmp) | *.jpg; *.jpeg; *.png; *.bmp";
                openDialog.Multiselect = true;
                bool? userClickedOK = openDialog.ShowDialog();
                if (userClickedOK == true)
                {
                    if (openDialog.Files != null && openDialog.Files.Count() > 0 && openDialog.File != null)
                    {
                        FileStream stream = openDialog.File.OpenRead();
                        var memoryStream = new MemoryStream();
                        stream.CopyTo(memoryStream);
                        lnkLetter.Tag = memoryStream.GetBuffer();
                        var formScanne = new UcImageScanne(memoryStream, SessionObject.ExecMode.Creation);
                        //var formScanne = new UcImageScanne(stream, SessionObject.ExecMode.Creation);
                        formScanne.Closed += new EventHandler(GetInformationFromChildWindowImageAutorisation);
                        formScanne.Show();
                    }
                }

            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Galatee.Silverlight.Resources.Devis.Languages.txtDevis);
            }
        }


        void galatee_OkClickedSite(object sender, EventArgs e)
        {
            UcListeGenerique ctrs = sender as UcListeGenerique;
            if (ctrs.isOkClick)
            {
                this.btn_Site.IsEnabled = true;
                CsSite leSite = (CsSite)ctrs.MyObject;
                this.Txt_CodeSite.Text = leSite.CODESITE;
                this.LstCentre = LstCentre.Where(t => t.FK_IDCODESITE == leSite.PK_ID).ToList();

            }
            else
                this.btn_Centre.IsEnabled = true;


        }


        private void Cbo_Centre_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (this.Cbo_Centre.SelectedItem != null)
                {
                    CsCentre centre = Cbo_Centre.SelectedItem as CsCentre;
                    if (centre != null)
                    {
                        Txt_CodeCentre.Text = centre.CODE ?? string.Empty;
                        Txt_CodeCentre.Tag = centre;

                        
                        //RemplirCommuneParCentre(centre);
                        //RemplirProduitCentre(centre);
                    }
                    //VerifierDonneesSaisiesInformationsDevis();
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.txtDevis);
            }
        }

        //private void Txt_CodeSite_TextChanged(object sender, TextChangedEventArgs e)
        //{
        //    if (this.Txt_CodeSite.Text.Length == SessionObject.Enumere.TailleCentre)
        //    {
        //        CsSite _LeSite = ClasseMEthodeGenerique.RetourneObjectFromList<CsSite>(lstSite, this.Txt_CodeSite.Text, "CODESITE");
        //        if (_LeSite != null && !string.IsNullOrEmpty(_LeSite.CODESITE))
        //            //this.Txt_LibelleSite.Text = _LeSite.LIBELLE;
        //        else
        //        {
        //            var w = new MessageBoxControl.MessageBoxChildWindow(Langue.lbl_Menu, Langue.MsgEltInexistent, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
        //            w.OnMessageBoxClosed += (_, result) =>
        //            {
        //                this.Txt_CodeCentre.Focus();
        //            };
        //            w.Show();
        //        }
        //    }


        //}
    }
}

