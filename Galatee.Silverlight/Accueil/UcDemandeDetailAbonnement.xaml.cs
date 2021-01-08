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
//using Galatee.Silverlight.serviceWeb;
using Galatee.Silverlight.MainView;
using Galatee.Silverlight.Resources.Accueil ;
using Galatee.Silverlight.Shared;


namespace Galatee.Silverlight.Accueil
{
    public partial class UcDemandeDetailAbonnement : UserControl
    {
      

        List<CsTarif> LstTarif = new List<CsTarif>();
        List<CsForfait > LstForfait = new List<CsForfait>() ;
        List<CsTarif > LstPuissanceTarif = new List<CsTarif>() ;
        List<CsFrequence> LstFrequence = new List<CsFrequence>();
        List<CsMois> LstMois = new List<CsMois>();
        List<CsCodeTaxeApplication > LstCodeApplicationTaxe = new List<CsCodeTaxeApplication>();

        CsTarif LeTarifSelect = new CsTarif();
        CsForfait LeForfaitSelect = new CsForfait();
        CsPuissance LePuissanceSelect = new CsPuissance();
        CsFrequence LeFrequenceSelect = new CsFrequence();
        CsMois LeMoisFactSelect = new CsMois();
        CsMois LeIndexFactSelect = new CsMois();
        CsCodeTaxeApplication LstCodeApplicationTaxeSelect=new CsCodeTaxeApplication();



        CsDevis LeDevis = new CsDevis();
        List<CsAbon> AbonementRecherche;
        decimal InitValue = 0;
        bool IsUpdate = false;
        CsAbon _UnAbon = new CsAbon();
        public CsDemande LaDemande = new CsDemande();
        public UcDemandeDetailAbonnement()
        {
            InitializeComponent();
        }
        string typeDemande = string.Empty;
        public UcDemandeDetailAbonnement(CsAbon _leAbonnement)
        {
            InitializeComponent();
            translate();
            _UnAbon = _leAbonnement;
            AfficherInfoAbonnement(_leAbonnement);
            RemplireLibelle();
            typeDemande = SessionObject.Enumere.ModificationAbonnement;
            IsControleInactif(true);
            this.btn_PussSouscrite.IsEnabled = false;

            this.Txt_CodeFrequence.MaxLength = 1;

        }
        public UcDemandeDetailAbonnement(CsDemande _LaDemande,bool _IsUpdate)
        {
            InitializeComponent();
            translate();
            SessionObject.EtatControlCourant = true;
            LaDemande = _LaDemande;
            typeDemande = _LaDemande.LaDemande.TYPEDEMANDE;
            if (LaDemande.Abonne == null)LaDemande.Abonne = new CsAbon();
            _UnAbon = LaDemande.Abonne;
            ChargerFrequence();
            ChargerApplicationTaxe();
            ChargerMois();
            ChargerTarif();
            ChargerForfait();
            ChargerPuissance();
            
            this.Txt_Client.Text = string.IsNullOrEmpty(LaDemande.LaDemande.CLIENT) ? string.Empty : LaDemande.LaDemande.CLIENT;
            this.Txt_Addresse.Text = string.IsNullOrEmpty(LaDemande.LaDemande.REFEM) ? string.Empty : LaDemande.LaDemande.REFEM;
            this.Txt_Ordre.Text = string.IsNullOrEmpty(LaDemande.LaDemande.ORDRE) ? string.Empty : LaDemande.LaDemande.ORDRE;

            this.Txt_CodePussanceSoucrite.Text = InitValue.ToString("N2");
            this.Txt_CodePuissanceUtilise.Text = InitValue.ToString("N2");
            this.Txt_Consomation.Text = InitValue.ToString("N2");
            this.Txt_CodeRistoune.Text = InitValue.ToString("N2");
            if (LaDemande.LaDemande.PRODUIT != SessionObject.Enumere.Electricite)
            {
                this.Txt_CodePussanceSoucrite.IsReadOnly = true;
                this.Txt_CodePuissanceUtilise.IsReadOnly = true;
                this.Txt_Consomation.IsReadOnly = true;
                this.Txt_CodeRistoune.IsReadOnly = true;
            }
            IsUpdate = _IsUpdate;
            chargerInformation();
          
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

            if (LaDemande.LaDemande.TYPEDEMANDE    == SessionObject.Enumere.Reabonnement ||
                LaDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.DimunitionPuissance ||
                LaDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.AugmentationPuissance )
            {
                if (!IsUpdate)
                    RetourneInfoAbon(LaDemande.LaDemande.FK_IDCENTRE ,LaDemande.LaDemande.CENTRE, LaDemande.LaDemande.CLIENT, LaDemande.LaDemande.ORDRE, LaDemande.LaDemande.PRODUIT);
                this.Txt_DateResiliation.IsReadOnly = true;
            }
            else if (LaDemande.LaDemande.TYPEDEMANDE    == SessionObject.Enumere.BranchementAbonement ||
                     LaDemande.LaDemande.TYPEDEMANDE    == SessionObject.Enumere.AbonnementSeul)
            {
                this.Txt_DateResiliation.Visibility = System.Windows.Visibility.Collapsed;
                this.lbl_DateResiliation.Visibility = System.Windows.Visibility.Collapsed;
                this.Txt_DateAbonnement.Text = DateTime.Now.ToShortDateString();
            }

            if (IsUpdate)
            {
                CsAbon _LAbon = new CsAbon();
                if (LaDemande.Abonne != null) _LAbon = LaDemande.Abonne;
                AfficherInfoAbonnement(_LAbon);
                if (typeDemande != SessionObject.Enumere.ModificationAbonnement)
                    EnregistrerDemande(LaDemande);
             }
        }
        private void InitialiseCtrl()
        {
            try
            {
                    this.Txt_Client.Text = LaDemande.LaDemande.CLIENT;
                    this.Txt_Addresse.Text = LaDemande.LaDemande.CLIENT;
                    this.Txt_Ordre.Text = LaDemande.LaDemande.ORDRE;
                    CsAbon _LAbon = new CsAbon();
                    if (LaDemande.Abonne != null) _LAbon = LaDemande.Abonne;
                    AfficherInfoAbonnement(_LAbon);
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message + "=>InitialiseCtrl" , Langue.lbl_Menu);
            }
        }

        private void IsControleInactif( bool etat)
        {
            this.Txt_CodeApplicationTaxe.IsReadOnly = etat;
            this.Txt_CodeForfait.IsReadOnly = etat;
            this.Txt_CodeFrequence.IsReadOnly = etat;
            this.Txt_CodeMoisFacturation.IsReadOnly = etat;
            this.Txt_CodeMoisIndex.IsReadOnly = etat;
            this.Txt_CodePuissanceUtilise.IsReadOnly = etat;
            this.Txt_CodePussanceSoucrite.IsReadOnly = etat;
            this.Txt_CodeRistoune.IsReadOnly = etat;
            this.Txt_CodeTarif.IsReadOnly = etat;
            this.Txt_Consomation.IsReadOnly = etat;
            this.Txt_DateAbonnement.IsReadOnly = etat;
            this.Txt_DateResiliation.IsReadOnly = etat;

            this.btn_discount.IsEnabled = !etat;
            this.btn_forfait.IsEnabled = !etat;
            this.btn_frequence.IsEnabled = !etat;
            this.btn_moisdefacturation.IsEnabled = !etat;
            this.btn_MoisIndex.IsEnabled = !etat;
            this.btn_PussSouscrite.IsEnabled = !etat;
            this.btn_tarifs.IsEnabled = !etat;
            this.btn_taxeApplication.IsEnabled = !etat;

        }

        public void EnregistrerDemande(CsDemande _Lademande)
        {
            try
            {
                _Lademande.Abonne.NUMDEM = string.IsNullOrEmpty(_Lademande.LaDemande.NUMDEM) ? string.Empty : _Lademande.LaDemande.NUMDEM;
                _Lademande.Abonne.CENTRE = string.IsNullOrEmpty(_Lademande.LaDemande.CENTRE) ? string.Empty : _Lademande.LaDemande.CENTRE;
                _Lademande.Abonne.CLIENT = string.IsNullOrEmpty(_Lademande.LaDemande.CLIENT) ? string.Empty : _Lademande.LaDemande.CLIENT;
                _Lademande.Abonne.ORDRE = string.IsNullOrEmpty(_Lademande.LaDemande.ORDRE) ? string.Empty : _Lademande.LaDemande.ORDRE;
                _Lademande.Abonne.PRODUIT = string.IsNullOrEmpty(_Lademande.LaDemande.PRODUIT) ? string.Empty : _Lademande.LaDemande.PRODUIT;

                _Lademande.Abonne.PUISSANCE = string.IsNullOrEmpty(this.Txt_CodePussanceSoucrite.Text) ? 0 : Convert.ToDecimal(this.Txt_CodePussanceSoucrite.Text);
                _Lademande.Abonne.PUISSANCEUTILISEE = string.IsNullOrEmpty(this.Txt_CodePuissanceUtilise.Text) ? 0 : Convert.ToDecimal(this.Txt_CodePuissanceUtilise.Text);
                _Lademande.Abonne.RISTOURNE = string.IsNullOrEmpty(this.Txt_CodeRistoune.Text) ? 0 : Convert.ToDecimal(this.Txt_CodeRistoune.Text);

                _Lademande.Abonne.FORFAIT = string.IsNullOrEmpty(this.Txt_CodeForfait.Text) ? string.Empty :this.Txt_CodeForfait.Text;
                _Lademande.Abonne.TYPETARIF = string.IsNullOrEmpty(this.Txt_CodeTarif.Text) ? string.Empty : this.Txt_CodeTarif.Text;
                _Lademande.Abonne.PERFAC = string.IsNullOrEmpty(this.Txt_CodeFrequence.Text) ? string.Empty : this.Txt_CodeFrequence.Text;
                _Lademande.Abonne.MOISREL = string.IsNullOrEmpty(this.Txt_CodeMoisIndex.Text) ? string.Empty : this.Txt_CodeMoisIndex.Text;
                _Lademande.Abonne.MOISFAC = string.IsNullOrEmpty(this.Txt_CodeMoisFacturation.Text) ? string.Empty : this.Txt_CodeMoisFacturation.Text;

                _Lademande.Abonne.DABONNEMENT = null;
                if (!string.IsNullOrEmpty(this.Txt_DateAbonnement.Text))
                    _Lademande.Abonne.DABONNEMENT = DateTime.Parse(this.Txt_DateAbonnement.Text);
                _Lademande.Abonne.DRES = null;
                if (_Lademande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.Reabonnement)
                    _Lademande.Abonne.DRES = null;
                else if (!string.IsNullOrEmpty(this.Txt_DateResiliation.Text))
                    _Lademande.Abonne.DRES = DateTime.Parse(this.Txt_DateResiliation.Text);
                _Lademande.Abonne.USERCREATION = UserConnecte.matricule;
                _Lademande.Abonne.USERMODIFICATION = UserConnecte.matricule;
                _Lademande.Abonne.DATECREATION = System.DateTime.Now;
                _Lademande.Abonne.DATEMODIFICATION = System.DateTime.Now;


                _Lademande.Abonne.FK_IDCENTRE = _Lademande.LaDemande.FK_IDCENTRE;
                _Lademande.Abonne.FK_IDPRODUIT = _Lademande.LaDemande.FK_IDPRODUIT.Value ;
                _Lademande.Abonne.FK_IDFORFAIT = this.Txt_CodeForfait.Tag == null ? _Lademande.Abonne.FK_IDFORFAIT : int.Parse(this.Txt_CodeForfait.Tag.ToString());
                _Lademande.Abonne.FK_IDMOISFAC = this.Txt_CodeMoisFacturation.Tag == null ? _Lademande.Abonne.FK_IDMOISFAC : int.Parse(this.Txt_CodeMoisFacturation.Tag.ToString());
                _Lademande.Abonne.FK_IDMOISREL = this.Txt_CodeMoisIndex.Tag == null ? _Lademande.Abonne.FK_IDMOISREL : int.Parse(this.Txt_CodeMoisIndex.Tag.ToString());
                _Lademande.Abonne.FK_IDTYPETARIF = this.Txt_CodeTarif.Tag == null ? _Lademande.Abonne.FK_IDTYPETARIF : int.Parse(this.Txt_CodeTarif.Tag.ToString());
                _Lademande.Abonne.FK_IDPERIODICITEFACTURE = this.Txt_CodeFrequence.Tag == null ? _Lademande.Abonne.FK_IDPERIODICITEFACTURE : int.Parse(this.Txt_CodeFrequence.Tag.ToString());
                _Lademande.Abonne.FK_IDPERIODICITERELEVE  = this.Txt_CodeFrequence.Tag == null ? _Lademande.Abonne.FK_IDPERIODICITEFACTURE : int.Parse(this.Txt_CodeFrequence.Tag.ToString());

            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message + "=>EnregistrerDemande", Langue.lbl_Menu);
            }
            
        }

        private void ChargerPuissance()
        {
            try
            {
                if (SessionObject.LstTarifPuissance .Count != 0)
                {
                    LstPuissanceTarif = SessionObject.LstTarifPuissance.Where(p => p.PRODUIT == LaDemande.LaDemande.PRODUIT).ToList();
                }
                else
                {
                    AcceuilServiceClient service = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                    service.ChargerPuissanceCompleted += (s, args) =>
                    {
                        if (args != null && args.Cancelled)
                            return;
                        SessionObject.LstPuissance = args.Result;
                        LstPuissanceTarif = SessionObject.LstTarifPuissance.Where(p => p.PRODUIT == LaDemande.LaDemande.PRODUIT).ToList();

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
                    LstForfait = SessionObject.LstForfait.Where(p => p.PRODUIT == LaDemande.LaDemande.PRODUIT).ToList();
                else
                {
                    AcceuilServiceClient service = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                    service.ChargerForfaitCompleted += (s, args) =>
                    {
                        if (args != null && args.Cancelled)
                            return;

                        SessionObject.LstForfait = args.Result;
                        LstForfait = SessionObject.LstForfait.Where(p => p.PRODUIT == LaDemande.LaDemande.PRODUIT).ToList();
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
                    LstTarif = SessionObject.LstTarif.Where(p=>p.PRODUIT == LaDemande.LaDemande.PRODUIT).ToList();
                else
                {
                    AcceuilServiceClient service = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                    service.ChargerTarifCompleted += (s, args) =>
                    {
                        if (args != null && args.Cancelled)
                            return;
                        SessionObject.LstTarif = args.Result;
                        LstTarif = SessionObject.LstTarif.Where(p => p.PRODUIT == LaDemande.LaDemande.PRODUIT).ToList();

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
            if (SessionObject.LstFrequence .Count != 0)
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
                    AcceuilServiceClient service = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
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

        private void RetourneInfoAbon(int fk_idcentre,string centre, string client, string ordre, string produit)
        {
            try
            {
                AbonementRecherche = new List<CsAbon>();
                AcceuilServiceClient service = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                service.RetourneAbonCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    AbonementRecherche = args.Result;
                    if (AbonementRecherche != null)
                    {
                        CsAbon _LeAbonnementProduit = AbonementRecherche.FirstOrDefault(p => p.PRODUIT   == produit);
                        LaDemande.Abonne = _LeAbonnementProduit;
                        AfficherInfoAbonnement(LaDemande.Abonne);
                    }
                };
                service.RetourneAbonAsync(fk_idcentre,centre, client, ordre);
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }
        private void AfficherInfoAbonnement(CsAbon _LeAbonnementdemande)
        {
            try
            {
                if (_LeAbonnementdemande.PUISSANCE != null )
                this.Txt_CodePussanceSoucrite.Text = Convert.ToDecimal( _LeAbonnementdemande.PUISSANCE.ToString()).ToString("N2");
                if (_LeAbonnementdemande.PUISSANCEUTILISEE != null)
                    this.Txt_CodePuissanceUtilise.Text = Convert.ToDecimal(_LeAbonnementdemande.PUISSANCEUTILISEE.Value ).ToString("N2");
                this.Txt_CodeRistoune.Text = string.IsNullOrEmpty(_LeAbonnementdemande.RISTOURNE.ToString()) ? string.Empty :Convert.ToDecimal( _LeAbonnementdemande.RISTOURNE.Value ).ToString("N2");
                this.Txt_CodeForfait.Text = string.IsNullOrEmpty(_LeAbonnementdemande.FORFAIT) ? string.Empty : _LeAbonnementdemande.FORFAIT;
                this.Txt_CodeTarif.Text = string.IsNullOrEmpty(_LeAbonnementdemande.TYPETARIF) ? string.Empty : _LeAbonnementdemande.TYPETARIF;
                this.Txt_CodeFrequence.Text = string.IsNullOrEmpty(_LeAbonnementdemande.PERFAC) ? string.Empty : _LeAbonnementdemande.PERFAC;
                this.Txt_CodeMoisIndex.Text = string.IsNullOrEmpty(_LeAbonnementdemande.MOISREL) ? string.Empty : _LeAbonnementdemande.MOISREL;
                this.Txt_CodeMoisFacturation.Text = string.IsNullOrEmpty(_LeAbonnementdemande.MOISFAC) ? string.Empty : _LeAbonnementdemande.MOISFAC;
                this.Txt_DateAbonnement.Text = (_LeAbonnementdemande.DABONNEMENT == null) ? DateTime.Now.ToShortDateString() : Convert.ToDateTime(_LeAbonnementdemande.DABONNEMENT.Value).ToShortDateString();
                this.Txt_DateResiliation.Text = (_LeAbonnementdemande.DRES == null) ? string.Empty : Convert.ToDateTime(_LeAbonnementdemande.DRES.Value).ToShortDateString();

                if (LstForfait.Count != 0 && this.Txt_CodeForfait.Text.Length == SessionObject.Enumere.TailleForfait)
                {
                    CsForfait _LeForfait = ClasseMEthodeGenerique.RetourneObjectFromList(LstForfait, this.Txt_CodeForfait.Text, "CODE");
                    if (_LeForfait != null)
                    {
                        this.Txt_LibelleForfait.Text = _LeForfait.LIBELLE;
                        this.Txt_CodeForfait.Tag = _LeForfait.PK_ID;
                        EnregistrerDemande(LaDemande);
                    }
                }
                if (!string.IsNullOrEmpty(this.Txt_CodeTarif.Text) &&
                              LstTarif != null && LstTarif.Count != 0 &&
                              this.Txt_CodeTarif.Text.Length == SessionObject.Enumere.TailleTarif)
                {

                    CsTarif _LeTarif = ClasseMEthodeGenerique.RetourneObjectFromList(LstTarif, this.Txt_CodeTarif.Text, "CODE");
                    if (_LeTarif != null)
                    {
                        this.Txt_LibelleTarif.Text = _LeTarif.LIBELLE;
                        this.Txt_CodeTarif.Tag = _LeTarif.PK_ID;
                        EnregistrerDemande(LaDemande);
                    }
                }
                if (LstFrequence.Count != 0 && this.Txt_CodeFrequence.Text.Length == SessionObject.Enumere.TailleMoisDeFacturation)
                {
                    CsFrequence _LaFrequence = ClasseMEthodeGenerique.RetourneObjectFromList(LstFrequence, this.Txt_CodeFrequence.Text, "CODE");
                    if (_LaFrequence != null)
                    {
                        if (_LaFrequence.LIBELLE != null)
                        {
                            this.Txt_LibelleFrequence.Text = _LaFrequence.LIBELLE;
                            this.Txt_CodeFrequence.Tag = _LaFrequence.PK_ID;
                            EnregistrerDemande(LaDemande);
                        }
                    }
                }
                if (LstMois.Count != 0 && this.Txt_CodeMoisFacturation.Text.Length == SessionObject.Enumere.TailleMoisDeFacturation)
                {
                    CsMois _LeMois = ClasseMEthodeGenerique.RetourneObjectFromList(LstMois, this.Txt_CodeMoisFacturation.Text, "CODE");
                    if (_LeMois != null)
                    {
                        if (_LeMois.LIBELLE != null)
                        {
                            this.Txt_LibMoisFact.Text = _LeMois.LIBELLE;
                            this.Txt_CodeMoisFacturation.Tag = _LeMois.PK_ID;
                            EnregistrerDemande(LaDemande);
                        }
                    }
                }
                if (LstMois.Count != 0 && this.Txt_CodeMoisIndex.Text.Length == SessionObject.Enumere.TailleMoisDeFacturation)
                {
                    CsMois _LeMois = ClasseMEthodeGenerique.RetourneObjectFromList(LstMois, this.Txt_CodeMoisIndex.Text, "CODE");
                    if (_LeMois != null)
                    {
                        this.Txt_LibelleMoisIndex.Text = _LeMois.LIBELLE;
                        this.Txt_CodeMoisIndex.Tag = _LeMois.PK_ID;
                        EnregistrerDemande(LaDemande);
                    }
                }
                if (LaDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.DimunitionPuissance ||
                    LaDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.AugmentationPuissance)
                {
                    Txt_CodeTarif.IsReadOnly = true;
                    Txt_CodePuissanceUtilise.IsReadOnly = true;
                    Txt_CodeRistoune.IsReadOnly = true;
                    Txt_CodeForfait.IsReadOnly = true;
                    Txt_CodeFrequence.IsReadOnly = true;
                    Txt_CodeMoisFacturation.IsReadOnly = true;
                    Txt_CodeMoisIndex.IsReadOnly = true;
                }


            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            InitialiseCtrl();
        }

        private void Txt_CodeTarif_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(this.Txt_CodeTarif.Text) && 
                LstTarif  != null  && LstTarif.Count != 0 &&
                this.Txt_CodeTarif.Text.Length == SessionObject.Enumere.TailleTarif )
            {

                CsTarif _LeTarif = ClasseMEthodeGenerique.RetourneObjectFromList(LstTarif, this.Txt_CodeTarif.Text, "CODE");
                if (_LeTarif != null)
                {
                    this.Txt_LibelleTarif.Text = _LeTarif.LIBELLE;
                    this.Txt_CodeTarif.Tag  = _LeTarif.PK_ID ;
                    EnregistrerDemande(LaDemande);
                    this.btn_PussSouscrite.IsEnabled = true ;
                    LstPuissanceTarif = SessionObject.LstTarifPuissance.Where(t => t.PK_ID == _LeTarif.PK_ID).ToList();
                    //LstPuissanceTarif.ForEach(t => t.CODEPUISSANCE = (decimal.Parse(t.CODEPUISSANCE)).ToString());

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
                    List<object> _LstObjet = ClasseMEthodeGenerique.RetourneListeObjet(LstTarif );
                    UcListeGenerique ctr = new UcListeGenerique(_LstObjet, "CODE", "LIBELLE", Langue.lbl_Menu);
                    ctr.Closed += new EventHandler(galatee_OkClickedBtnTarif);
                    ctr.Show();
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message + "=btn_tarifs_Click", Langue.lbl_Menu);      
            }
        }
        private void galatee_OkClickedBtnTarif(object sender, EventArgs e)
        {
            try
            {
                this.btn_tarifs.IsEnabled = true;
                UcListeGenerique ctrs = sender as UcListeGenerique;
                if (ctrs.isOkClick )
                {
                    CsTarif _LeTarif = (CsTarif)ctrs.MyObject;
                    this.Txt_CodeTarif.Text = _LeTarif.CODE   ;
                }
                
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message + "galatee_OkClickedBtnTarif", Langue.lbl_Menu);
            }


        }

        private void btn_PussSouscrite_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (LstPuissanceTarif.Count != 0)
                {
                    List<object> _LstObjet = ClasseMEthodeGenerique.RetourneListeObjet(LstPuissanceTarif);
                    UcListeGenerique ctr = new UcListeGenerique(_LstObjet, "CODEPUISSANCE", "PUISSANCE", "Liste");
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
                if (ctrs.isOkClick )
                {
                    CsTarif _LaPuissanceSelect = (CsTarif)ctrs.MyObject;
                    //this.Txt_CodePussanceSoucrite.Text =(decimal.Parse( _LaPuissanceSelect.CODEPUISSANCE )).ToString();
                    //this.Txt_CodePussanceSoucrite.Tag  = _LaPuissanceSelect.FK_IDPUISSANCE  ;
                }
                this.btn_PussSouscrite.IsEnabled = true;
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Langue.lbl_Menu);
            }
        }
        private void Txt_CodePussanceSoucrite_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (LstPuissanceTarif != null && LstPuissanceTarif.Count != 0)
            //&& this.Txt_CodePussanceSoucrite .Text.Length == SessionObject.Enumere.)
            {

                CsTarif _LaPuissance = ClasseMEthodeGenerique.RetourneObjectFromList(LstPuissanceTarif, this.Txt_CodePussanceSoucrite.Text, "CODEPUISSANCE");
                if (_LaPuissance != null)
                {
                    this.Txt_CodePussanceSoucrite.Tag = _LaPuissance.PK_ID;
                    EnregistrerDemande(LaDemande);
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
        private void Txt_CodeForfait_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (LstForfait .Count != 0 && this.Txt_CodeForfait.Text.Length == SessionObject.Enumere.TailleForfait)
                {
                    CsForfait _LeForfait = ClasseMEthodeGenerique.RetourneObjectFromList(LstForfait, this.Txt_CodeForfait.Text, "CODE");
                    if (_LeForfait != null)
                    {
                        this.Txt_LibelleForfait.Text = _LeForfait.LIBELLE;
                        this.Txt_CodeForfait.Tag  = _LeForfait.PK_ID ;
                        EnregistrerDemande(LaDemande);
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
                    this.btn_forfait .IsEnabled = false;
                    List<object> _LstObjet = ClasseMEthodeGenerique.RetourneListeObjet(LstForfait);
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
                UcListeGenerique ctrs = sender as UcListeGenerique;
                if (ctrs.isOkClick )
                {
                    CsForfait _Leforfait = (CsForfait)ctrs.MyObject;
                    this.Txt_CodeForfait.Text = _Leforfait.CODE;
                    this.Txt_CodeForfait.Tag  = _Leforfait.PK_ID ;
                }
                this.btn_forfait.IsEnabled = true;
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Langue.lbl_Menu);
            }
        }

        private void btn_frequence_Click(object sender, RoutedEventArgs e)
        {
            if (LstFrequence!= null && LstFrequence.Count != 0)
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
            if (ctrs.isOkClick )
            {
                CsFrequence _LaFrequence = (CsFrequence)ctrs.MyObject;
                this.Txt_CodeFrequence.Text = _LaFrequence.CODE;
                this.Txt_LibelleFrequence .Text = _LaFrequence.LIBELLE ;
                this.Txt_CodeFrequence.Tag  = _LaFrequence.PK_ID ;
            }
            this.btn_frequence.IsEnabled = true;
        }
        private void Txt_CodeFrequence_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (LstFrequence.Count != 0 && this.Txt_CodeFrequence.Text.Length == 1)
                {
                    CsFrequence _LaFrequence = ClasseMEthodeGenerique.RetourneObjectFromList(LstFrequence, this.Txt_CodeFrequence.Text, "CODE");
                    if (_LaFrequence != null)
                    {
                        this.Txt_LibelleFrequence.Text = _LaFrequence.LIBELLE;
                        this.Txt_CodeFrequence.Tag = _LaFrequence.PK_ID;
                        EnregistrerDemande(LaDemande);
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
                        this.Txt_CodeMoisFacturation.Tag  = _LeMois.PK_ID ;
                        EnregistrerDemande(LaDemande);
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
                    this.btn_moisdefacturation .IsEnabled = false;
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
                if (ctrs.isOkClick )
                {
                    CsMois _LeMois = (CsMois)ctrs.MyObject;
                    this.Txt_CodeMoisFacturation.Text = _LeMois.CODE;
                    this.Txt_CodeMoisFacturation.Tag  = _LeMois.PK_ID ;
                }
                this.btn_moisdefacturation .IsEnabled = true;
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
                        this.Txt_CodeMoisIndex.Tag = _LeMois.PK_ID;
                        EnregistrerDemande(LaDemande);
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
                if (ctrs.isOkClick )
                {
                    CsMois _LeMois = (CsMois)ctrs.MyObject;
                    this.Txt_CodeMoisIndex.Text = _LeMois.CODE;
                    this.Txt_CodeMoisIndex.Tag  = _LeMois.PK_ID ;
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
                    else
                    {
                        if (typeDemande != SessionObject.Enumere.ModificationAbonnement)
                            EnregistrerDemande(LaDemande);
                    }
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
                    if (ClasseMEthodeGenerique.IsDateValide(this.Txt_DateResiliation.Text)==null )
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

      


        private void RemplireLibelle()
        {
            try
            {
                if (SessionObject.LstFrequence .Count != 0 && this.Txt_CodeFrequence.Text.Length == SessionObject.Enumere.TailleMoisDeFacturation)
                {
                    CsFrequence _LaFrequence = ClasseMEthodeGenerique.RetourneObjectFromList(SessionObject.LstFrequence, this.Txt_CodeFrequence.Text, "CODE");
                    if (_LaFrequence != null)
                        this.Txt_LibelleFrequence.Text = _LaFrequence.LIBELLE;
                }
                if (SessionObject.LstMois.Count != 0 && this.Txt_CodeMoisIndex.Text.Length == SessionObject.Enumere.TailleMoisDeFacturation)
                {
                    CsMois _LeMois = ClasseMEthodeGenerique.RetourneObjectFromList(SessionObject.LstMois, this.Txt_CodeMoisIndex.Text, "CODE");
                    if (_LeMois != null)
                        this.Txt_LibelleMoisIndex.Text = _LeMois.LIBELLE;
                }
                if (SessionObject.LstMois.Count != 0 && this.Txt_CodeMoisFacturation.Text.Length == SessionObject.Enumere.TailleMoisDeFacturation)
                {
                    CsMois _LeMois = ClasseMEthodeGenerique.RetourneObjectFromList(SessionObject.LstMois, this.Txt_CodeMoisFacturation.Text, "CODE");
                    if (_LeMois != null)
                        this.Txt_LibMoisFact.Text = _LeMois.LIBELLE;
                }
                if (SessionObject.LstForfait.Count != 0 && this.Txt_CodeForfait.Text.Length == SessionObject.Enumere.TailleForfait)
                {
                    CsForfait _LeForfait = ClasseMEthodeGenerique.RetourneObjectFromList(SessionObject.LstForfait, this.Txt_CodeForfait.Text, "CODE");
                    if (_LeForfait != null)
                        this.Txt_LibelleForfait.Text = _LeForfait.LIBELLE;
                }
                if (!string.IsNullOrEmpty(this.Txt_CodeTarif.Text) &&
                    SessionObject.LstTarif != null && SessionObject.LstTarif.Count != 0 &&
                    this.Txt_CodeTarif.Text.Length == SessionObject.Enumere.TailleTarif)
                {
                    CsTarif _LeTarif = ClasseMEthodeGenerique.RetourneObjectFromList(SessionObject.LstTarif, this.Txt_CodeTarif.Text, "CODE");
                    if (_LeTarif != null)
                        this.Txt_LibelleTarif.Text = _LeTarif.LIBELLE;
                }
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }




    }
}
