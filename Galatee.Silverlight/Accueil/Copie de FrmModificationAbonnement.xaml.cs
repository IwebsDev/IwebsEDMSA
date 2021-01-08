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
using System.Collections.ObjectModel;

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
        private UcImageScanne formScanne = null;
      
        List<CsProduit> ListeDesProduitDuSite = new List<CsProduit>();
        public ObservableCollection<ObjDOCUMENTSCANNE> LstPiece = new ObservableCollection<ObjDOCUMENTSCANNE>();
        public ObservableCollection<CsTypeDOCUMENTSCANNE> LstTypeDocument = new ObservableCollection<CsTypeDOCUMENTSCANNE>();
        private byte[] image;

        List<CsAbon> AbonementRecherche;
        CsAbon LeAbonne = new CsAbon();
        decimal InitValue = 0;
        string Tdem = string.Empty;
        CsDemande laDetailDemande = null;
        string CodeProduit = string.Empty;
        public FrmModificationAbonnement()
        {
            InitializeComponent();
        }

        public FrmModificationAbonnement(string _TypeDemande,string Init)
        {
            InitializeComponent();
       
            ChargerFrequence();
            ChargerApplicationTaxe();
            ChargerMois();
            ChargerTarif();
            ChargerForfait();
            ChargerPuissanceSouscrite();
            ChargerTypeDocument();
            this.Txt_CodePussanceSoucrite.Text = InitValue.ToString();
            this.Txt_CodePuissanceUtilise.Text = InitValue.ToString();

            this.Txt_TypeDeComptage.Visibility = Visibility.Collapsed;
            this.lbl_TypeDeComptage.Visibility = Visibility.Collapsed; 
            Tdem = _TypeDemande;
        
            IsGiserChamp(true);
            ChargerDonneeDuSite();
            translate();
        }
        public FrmModificationAbonnement(int iddemande)
        {
            InitializeComponent();

            ChargerFrequence();
            ChargerApplicationTaxe();
            ChargerMois();
            ChargerTarif();
            ChargerForfait();
            ChargerPuissanceSouscrite();
            ChargerTypeDocument();
            this.Txt_CodePussanceSoucrite.Text = InitValue.ToString();
            this.Txt_CodePuissanceUtilise.Text = InitValue.ToString();

            this.Txt_TypeDeComptage.Visibility = Visibility.Collapsed;
            this.lbl_TypeDeComptage.Visibility = Visibility.Collapsed; 

            IsGiserChamp(true);
            ChargerDonneeDuSite();
            ChargeDetailDEvis(iddemande);
            translate();
        }


        bool IsRejeterDemande = false;

        List<CsCentre> LstCentre = new List<CsCentre>();
        List<Galatee.Silverlight.ServiceAccueil.CsCentre> LstCentrePerimetre = new List<ServiceAccueil.CsCentre>();
        List<Galatee.Silverlight.ServiceAccueil.CsSite> lstSite = new List<Galatee.Silverlight.ServiceAccueil.CsSite>();
        List<int> lstIdCentre = new List<int>();
        private void ChargerDonneeDuSite()
        {
            try
            {
                if (SessionObject.LstCentre != null && SessionObject.LstCentre.Count > 0)
                {
                    LstCentrePerimetre = Shared.ClasseMEthodeGenerique.RetourCentreByPerimetre(SessionObject.LstCentre.Where(p => p.CODE != SessionObject.Enumere.Generale).ToList(), UserConnecte.listeProfilUser);
                    lstSite = ClasseMEthodeGenerique.RetourneSiteByCentre(LstCentrePerimetre);
                    foreach (Galatee.Silverlight.ServiceAccueil.CsCentre item in LstCentrePerimetre)
                        lstIdCentre.Add(item.PK_ID);

                    if (lstSite != null && lstSite.Count != 0)
                    {
                        if (lstSite.Count == 1)
                        if (LstCentrePerimetre != null && LstCentrePerimetre.Count != 0)
                        {
                            if (LstCentrePerimetre.Count == 1)
                                ListeDesProduitDuSite = LstCentrePerimetre.First().LESPRODUITSDUSITE;
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
                        lstIdCentre.Add(item.PK_ID);
                };
                service.ListeDesDonneesDesSiteAsync(false);
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        string OrdreMax = string.Empty;
        private void RechercheAbonnement(CsClient leClient)
        {
            try
            {
                AcceuilServiceClient service = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.RetourneOrdreMaxCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    OrdreMax = args.Result;
                    if (OrdreMax != null)
                    {
                        leClient.ORDRE = OrdreMax;
                        VerifieExisteDemande(leClient);
                    }
                };
                service.RetourneOrdreMaxAsync(leClient.FK_IDCENTRE.Value, leClient.CENTRE, leClient.REFCLIENT, leClient.PRODUIT);
                service.CloseAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void IsGiserChamp(bool Etat)
        {
            this.Txt_CodeTarif.IsReadOnly = Etat;
            this.btn_tarifs.IsEnabled = !Etat;

            this.Txt_CodePussanceSoucrite.IsReadOnly = Etat;
            this.btn_PussSouscrite.IsEnabled = !Etat;

            this.Txt_CodePuissanceUtilise.IsReadOnly = Etat;

            this.Txt_CodeForfait.IsReadOnly = Etat;

            this.btn_frequence.IsEnabled = !Etat;
            this.Txt_CodeFrequence.IsReadOnly = Etat;

            this.btn_moisdefacturation.IsEnabled = !Etat;
            this.Txt_CodeMoisFacturation.IsReadOnly = Etat;

            this.Txt_CodeForfait.IsReadOnly = Etat;
            this.btn_forfait.IsEnabled = !Etat;
        }
        private void translate()
        {
            // Gestion de la langue
            this.lbl_DateAbonnement.Content = Langue.lbl_DateAbonnement;
            this.lbl_Forfait.Content = Langue.lbl_Forfait;
            this.lbl_MoisFact.Content = Langue.lbl_MoisFact;
            this.lbl_Periodicite.Content = Langue.lbl_Periodicite;
            this.lbl_PuissanceSouscrite.Content = Langue.lbl_PuissanceSouscrite;
            this.lbl_PuissanceUtilise.Content = Langue.lbl_PuissanceUtilise;
            this.lbl_Tarif.Content = Langue.lbl_Tarif;
            //
        }
     
 
        public bool EnregisterDemande(CsDemande _Lademande)
        {
            try
            {
                _Lademande.Abonne.PUISSANCE = string.IsNullOrEmpty(this.Txt_CodePussanceSoucrite.Text) ? 0 : Convert.ToDecimal(this.Txt_CodePussanceSoucrite.Text);
                _Lademande.Abonne.PUISSANCEUTILISEE = string.IsNullOrEmpty(this.Txt_CodePuissanceUtilise.Text) ? 0 : Convert.ToDecimal(this.Txt_CodePuissanceUtilise.Text);
                _Lademande.Abonne.NATURECLIENT = (AbonementRecherche != null && AbonementRecherche[0].NATURECLIENT != null && !string.IsNullOrEmpty(AbonementRecherche[0].NATURECLIENT)) ? AbonementRecherche[0].NATURECLIENT : string.Empty;
                _Lademande.Abonne.FORFAIT = string.IsNullOrEmpty(this.Txt_CodeForfait.Text) ? null : this.Txt_CodeForfait.Text;
                _Lademande.Abonne.DABONNEMENT = string.IsNullOrEmpty(this.Txt_DateAbonnement.Text) ? _Lademande.Abonne.DABONNEMENT : Convert.ToDateTime(this.Txt_DateAbonnement.Text);
                _Lademande.Abonne.AVANCE = string.IsNullOrEmpty(this.Txt_AvanceSurConso.Text) ? 0 : Convert.ToDecimal(this.Txt_AvanceSurConso.Text);

                if (!string.IsNullOrEmpty(this.Txt_DateResiliation.Text))
                _Lademande.Abonne.DRES =  Convert.ToDateTime(this.Txt_DateResiliation.Text);
                else
                    _Lademande.Abonne.DRES =null ;

                _Lademande.Abonne.TYPETARIF = this.Txt_CodeTarif.Text;
                _Lademande.Abonne.PERFAC = this.Txt_CodeFrequence.Text;
                _Lademande.Abonne.CLIENT = this.Txt_ReferenceClient.Text;
                _Lademande.Abonne.MOISFAC = this.Txt_CodeMoisFacturation.Text;
                _Lademande.Abonne.ESTEXONERETVA = this.chk_EstExoneration.IsChecked .Value;

                if (_Lademande.Abonne.ESTEXONERETVA == true)
                {
                    _Lademande.Abonne.DEBUTEXONERATIONTVA = string.IsNullOrEmpty(this.txt_DebutPeriodeExo.Text) ? null : ClasseMEthodeGenerique.FormatPeriodeAAAAMM(this.txt_DebutPeriodeExo.Text);
                    _Lademande.Abonne.FINEXONERATIONTVA = string.IsNullOrEmpty(this.txt_FinPeriodeExo.Text) ? null : ClasseMEthodeGenerique.FormatPeriodeAAAAMM(this.txt_FinPeriodeExo.Text);
                }
                else
                {
                    _Lademande.Abonne.DEBUTEXONERATIONTVA = null;
                    _Lademande.Abonne.FINEXONERATIONTVA = null;
                }
                _Lademande.Abonne.USERMODIFICATION = UserConnecte.matricule;
                _Lademande.Abonne.DATEMODIFICATION = System.DateTime.Now;
                _Lademande.Abonne.DATECREATION  = System.DateTime.Now;
                _Lademande.Abonne.USERCREATION = UserConnecte.matricule;
                if (Txt_CodeMoisFacturation.Tag != null )
                _Lademande.Abonne.FK_IDMOISFAC = (int)Txt_CodeMoisFacturation.Tag;
                if (Txt_CodeFrequence.Tag != null )
                _Lademande.Abonne.FK_IDPERIODICITEFACTURE = (int)Txt_CodeFrequence.Tag;
                if (Txt_CodeTarif.Tag != null )
                    _Lademande.Abonne.FK_IDTYPETARIF = (int)Txt_CodeTarif.Tag;
                if (Txt_CodeForfait.Tag != null)
                _Lademande.Abonne.FK_IDFORFAIT  = (int)Txt_CodeForfait.Tag;

                if (this.Txt_TypeDeComptage.Tag != null)
                {
                    _Lademande.Abonne.TYPECOMPTAGE = this.Txt_TypeDeComptage.Tag.ToString();
                    _Lademande.Abonne.FK_IDTYPECOMPTAGE = (int)this.lbl_TypeDeComptage.Tag;
                }

                if (_Lademande.LaDemande == null) _Lademande.LaDemande = new CsDemandeBase();
                _Lademande.LaDemande.MATRICULE = UserConnecte.matricule;
                _Lademande.LaDemande.CENTRE = _Lademande.Abonne.CENTRE;
                _Lademande.LaDemande.CLIENT = _Lademande.Abonne.CLIENT ;
                _Lademande.LaDemande.ORDRE = _Lademande.Abonne.ORDRE;
                _Lademande.LaDemande.PRODUIT = _Lademande.Abonne.PRODUIT ;
                _Lademande.LaDemande.STATUT = SessionObject.Enumere.DemandeStatusEnAttente;
                _Lademande.LaDemande.TYPEDEMANDE = Tdem ;
                _Lademande.LaDemande.FK_IDTYPEDEMANDE = SessionObject.LstTypeDemande.FirstOrDefault(t => t.CODE == Tdem) != null ? SessionObject.LstTypeDemande.FirstOrDefault(t => t.CODE == Tdem).PK_ID : 0;
                _Lademande.LaDemande.FK_IDCENTRE = (int)txtCentre.Tag;
                _Lademande.LaDemande.FK_IDPRODUIT = (int)txt_Produit.Tag;

                _Lademande.LaDemande.USERCREATION = UserConnecte.matricule;
                _Lademande.LaDemande.USERMODIFICATION = UserConnecte.matricule;
                _Lademande.LaDemande.DATECREATION = System.DateTime.Now;
                _Lademande.LaDemande.DATEMODIFICATION = System.DateTime.Now;
                _Lademande.LaDemande.ISDEMANDEREJETERINIT  = false ;

                _Lademande.Branchement  = null;
                //_Lademande.Ag =  ;
                //_Lademande.LeClient = ;
                _Lademande.LstCanalistion = null;
                _Lademande.LstEvenement = null;

                #region Doc Scanne
                if (_Lademande.ObjetScanne == null) _Lademande.ObjetScanne = new List<ObjDOCUMENTSCANNE>();
                _Lademande.ObjetScanne.AddRange(LstPiece);
                #endregion

                return true;
            }
            catch (Exception ex)
            {
                this.OKButton.IsEnabled = true;
                Message.ShowError(ex.Message, Langue.lbl_Menu);
                return false;
            }

        }

        private void ChargerPuissanceSouscrite()
        {
            try
            {
                if (SessionObject.LstPuissance.Count != 0)
                {
                    LstPuissance = SessionObject.LstPuissance;
                }
                else
                {
                    AcceuilServiceClient service = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                    service.ChargerPuissanceSouscriteCompleted += (s, args) =>
                    {
                        if (args != null && args.Cancelled)
                            return;
                        SessionObject.LstPuissance = args.Result;
                        LstPuissance = SessionObject.LstPuissance;

                    };
                    service.ChargerPuissanceSouscriteAsync();
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
                    AcceuilServiceClient service = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
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
                    AcceuilServiceClient service = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
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
                AcceuilServiceClient service = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
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
                    AcceuilServiceClient service = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
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

        private void AfficherInfoAbonnement(CsAbon _LeAbonnementdemande)
        {
            try
            {

                this.Txt_CodeForfait.Text = string.IsNullOrEmpty(_LeAbonnementdemande.FORFAIT) ? string.Empty : _LeAbonnementdemande.FORFAIT;
                this.Txt_CodeTarif.Text = string.IsNullOrEmpty(_LeAbonnementdemande.TYPETARIF) ? string.Empty : _LeAbonnementdemande.TYPETARIF;
                this.Txt_CodeFrequence.Text = string.IsNullOrEmpty(_LeAbonnementdemande.PERFAC) ? string.Empty : _LeAbonnementdemande.PERFAC;
                this.Txt_CodeMoisFacturation.Text = string.IsNullOrEmpty(_LeAbonnementdemande.MOISFAC) ? string.Empty : _LeAbonnementdemande.MOISFAC;
                this.Txt_DateAbonnement.Text = (_LeAbonnementdemande.DABONNEMENT == null) ? string.Empty : Convert.ToDateTime(_LeAbonnementdemande.DABONNEMENT.Value).ToShortDateString();
                this.Txt_DateResiliation.Text = (_LeAbonnementdemande.DRES == null) ? string.Empty : Convert.ToDateTime(_LeAbonnementdemande.DRES.Value).ToShortDateString();
                this.Txt_LibelleFrequence.Text = string.IsNullOrEmpty(_LeAbonnementdemande.LIBELLEFREQUENCE) ? string.Empty : _LeAbonnementdemande.LIBELLEFREQUENCE;
                this.Txt_LibelleTarif.Text = string.IsNullOrEmpty(_LeAbonnementdemande.LIBELLETARIF) ? string.Empty : _LeAbonnementdemande.LIBELLETARIF;
                this.Txt_LibMoisFact.Text = string.IsNullOrEmpty(_LeAbonnementdemande.LIBELLEMOISFACT) ? string.Empty : _LeAbonnementdemande.LIBELLEMOISFACT;
                this.Txt_LibelleForfait.Text = string.IsNullOrEmpty(_LeAbonnementdemande.LIBELLEFORFAIT) ? string.Empty : _LeAbonnementdemande.LIBELLEFORFAIT;
                this.txt_DebutPeriodeExo.Text = string.IsNullOrEmpty(_LeAbonnementdemande.DEBUTEXONERATIONTVA) ? string.Empty :ClasseMEthodeGenerique.FormatPeriodeMMAAAA ( _LeAbonnementdemande.DEBUTEXONERATIONTVA);
                this.txt_FinPeriodeExo.Text = string.IsNullOrEmpty(_LeAbonnementdemande.FINEXONERATIONTVA) ? string.Empty : ClasseMEthodeGenerique.FormatPeriodeMMAAAA(_LeAbonnementdemande.FINEXONERATIONTVA);
                this.chk_EstExoneration.IsChecked = _LeAbonnementdemande.ESTEXONERETVA == true ? true : false;
                this.Txt_AvanceSurConso.Text = _LeAbonnementdemande.AVANCE!= null ? Convert.ToDecimal(_LeAbonnementdemande.AVANCE.Value).ToString() : "0";

                if (_LeAbonnementdemande.PUISSANCE != null)
                    this.Txt_CodePussanceSoucrite.Text = Convert.ToDecimal(_LeAbonnementdemande.PUISSANCE.ToString()).ToString("N2");
                if (_LeAbonnementdemande.PUISSANCEUTILISEE != null)
                    this.Txt_CodePuissanceUtilise.Text = Convert.ToDecimal(_LeAbonnementdemande.PUISSANCEUTILISEE.Value).ToString("N2");


                if (_LeAbonnementdemande.PRODUIT == SessionObject.Enumere.ElectriciteMT)
                {
                    this.Txt_TypeDeComptage.Visibility = Visibility.Visible;
                    this.lbl_TypeDeComptage.Visibility = Visibility.Visible;

                    this.Txt_TypeDeComptage.Text = _LeAbonnementdemande.LIBELLETYPECOMPTAGE;
                }
                if (_LeAbonnementdemande.PRODUIT != SessionObject.Enumere.ElectriciteMT)
                {
                    int idCategorie = 0;
                    if (laDetailDemande.LeClient != null && laDetailDemande.LeClient.FK_IDCATEGORIE != null)
                        idCategorie = laDetailDemande.LeClient.FK_IDCATEGORIE.Value;

                    //int idReglageCompteur = 0;
                    //if (laDetailDemande.LstCanalistion != null && laDetailDemande.LstCanalistion.Count != 0)
                    //    idReglageCompteur = laDetailDemande.LstCanalistion.First().FK_IDREGLAGECOMPTEUR.Value;

                    int idProduit = 0;
                    if (laDetailDemande.Abonne.FK_IDPRODUIT != null)
                        idProduit = laDetailDemande.Abonne.FK_IDPRODUIT;

                    int IdPuissanceSouscrite = SessionObject.LstPuissance.FirstOrDefault(t => t.VALEUR  == laDetailDemande.Abonne.PUISSANCE).PK_ID;

                    if (idCategorie != null && idProduit != null )
                        ChargerPuissanceEtTarif(idProduit, IdPuissanceSouscrite, idCategorie, null );
                }
                IsGiserChamp(false);

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
     
        private void Txt_CodeTarif_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(this.Txt_CodeTarif.Text) && LstTarif != null && LstTarif.Count != 0 &&
                this.Txt_CodeTarif.Text.Length == SessionObject.Enumere.TailleTarif)
            {
                CsTarif _LeTarif = LstTarif.FirstOrDefault(p => p.PRODUIT == CodeProduit && p.CODE == this.Txt_CodeTarif.Text);
                if (_LeTarif != null)
                {
                    this.Txt_LibelleTarif.Text = _LeTarif.LIBELLE;
                    Txt_CodeTarif.Tag = _LeTarif.PK_ID ;
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
                if ( !string.IsNullOrEmpty( CodeProduit))
                {
                    this.btn_tarifs.IsEnabled = false;
                    if (laDetailDemande != null && laDetailDemande.Abonne != null && this.Txt_CodeTarif.Tag != null)
                    {
                        if (laDetailDemande.Abonne.PRODUIT != SessionObject.Enumere.ElectriciteMT)
                        {
                            int idCategorie = 0;
                            if (laDetailDemande.LeClient  != null && laDetailDemande.LeClient.FK_IDCATEGORIE != null )
                                idCategorie = laDetailDemande.LeClient.FK_IDCATEGORIE.Value  ;

                            
                            int idProduit = 0;
                            if (laDetailDemande.LaDemande.FK_IDPRODUIT != null)
                                idProduit = laDetailDemande.Abonne.FK_IDPRODUIT;

                            int IdPuissanceSouscrite = SessionObject.LstPuissance.FirstOrDefault(t => t.VALEUR  == laDetailDemande.Abonne.PUISSANCE).PK_ID;

                            if (idCategorie != null && idProduit != null )
                                ChargerPuissanceEtTarif(idProduit, IdPuissanceSouscrite, idCategorie, null );
                        }
                    }


                    List<object> _LstObjet = ClasseMEthodeGenerique.RetourneListeObjet(LstTarif);
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
        List<CsTarif> lstDesTarif = new List<CsTarif>();
        private void ChargerPuissanceEtTarif(int idProduit, int? idPuissance, int? idCategorie, int? idReglageCompteur)
        {
            AcceuilServiceClient service = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
            service.ChargerTypeTarifCompleted += (s, args) =>
            {
                if (args != null && args.Cancelled)
                    return;
                lstDesTarif = args.Result;
                if (lstDesTarif != null && lstDesTarif.Count == 1)
                {
                    this.Txt_CodeTarif.Text = lstDesTarif.First().CODE;
                    this.Txt_LibelleTarif.Text = lstDesTarif.First().LIBELLE;
                    this.Txt_CodeTarif.Tag = lstDesTarif.First().PK_ID;
                }
                else
                {
                    this.Txt_CodeTarif.Text = string.Empty;
                    this.Txt_LibelleTarif.Text = string.Empty;
                    this.Txt_CodeTarif.Tag = null;
                }
            };
            service.ChargerTypeTarifAsync(idProduit, idPuissance, idCategorie, idReglageCompteur);
            service.CloseAsync();

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
                    this.Txt_CodeTarif.Tag  = _LeTarif.PK_ID   ;
                    if (laDetailDemande != null && laDetailDemande.Abonne != null && this.Txt_CodeTarif.Tag != null)
                    {
                        if (laDetailDemande.Abonne.PRODUIT != SessionObject.Enumere.ElectriciteMT)
                        {
                           List<CsTarif> lstTarifReglage = SessionObject.LstTarifParReglageCompteur.Where(t => t.FK_IDTYPETARIF == ((CsTarif)Txt_CodeTarif.Tag).PK_ID).ToList();
                           var lesPuissanceReglage = SessionObject.LstPuissanceParReglageCompteur.Where(t => lstTarifReglage.Any(y => y.FK_IDREGLAGECOMPTEUR == t.FK_IDREGLAGECOMPTEUR));
                           var lesPuissance = SessionObject.LstPuissance.Where(t => lesPuissanceReglage.Any(y => y.FK_IDPUISSANCE  == t.PK_ID ));
                           LstPuissance = new List<CsPuissance>();
                           foreach (var item in lesPuissance)
                               LstPuissance.Add(new CsPuissance() { 
                                FK_IDREGLAGECOMPTEUR =item.FK_IDREGLAGECOMPTEUR ,
                                FK_IDPUISSANCE= item.FK_IDPUISSANCE ,
                                FK_IDPRODUIT = _LeTarif.FK_IDPRODUIT ,
                                LIBELLE = item.LIBELLE,
                                CODE = item.CODE 
                               });
                        }
                    }
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
                if ( laDetailDemande != null  &&  laDetailDemande.Abonne != null )
                {
                    List<object> _LstObjet = new List<object>(); 
                    if (this.Txt_CodeTarif.Tag != null && laDetailDemande.Abonne.PRODUIT != SessionObject.Enumere.ElectriciteMT)
                    {
                        _LstObjet = ClasseMEthodeGenerique.RetourneListeObjet(LstPuissance.Where(t => t.FK_IDPRODUIT == ((CsTarif)this.Txt_CodeTarif.Tag).FK_IDPRODUIT).ToList());
                        UcListeGenerique ctr = new UcListeGenerique(_LstObjet, "CODE", "LIBELLE", "Liste");
                        ctr.Closed += new EventHandler(galatee_OkClickedBtnpuissanceSouscrite);
                        ctr.Show();
                    }
                    else
                    {
                        _LstObjet = ClasseMEthodeGenerique.RetourneListeObjet(LstPuissance.Where(t => t.FK_IDPRODUIT == laDetailDemande.Abonne.FK_IDPRODUIT ).ToList());
                        UcListeGenerique ctr = new UcListeGenerique(_LstObjet, "CODE", "LIBELLE", "Liste");
                        ctr.Closed += new EventHandler(galatee_OkClickedBtnpuissanceSouscrite);
                        ctr.Show();
                    }
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
                    this.Txt_CodePussanceSoucrite.Text = Convert.ToDecimal(_LaPuissanceSelect.CODE).ToString("N2");

                    if (laDetailDemande.Abonne.PRODUIT == SessionObject.Enumere.ElectriciteMT)
                    {
                        int? nbrtransfo = null;
                        if (laDetailDemande.Branchement.NOMBRETRANSFORMATEUR != null)
                            nbrtransfo = laDetailDemande.Branchement.NOMBRETRANSFORMATEUR;
                        int puissansSouscrit = Convert.ToInt32(_LaPuissanceSelect.VALEUR);
                        int puissansInstalle = int.Parse(Txt_CodePuissanceUtilise.Text);
                        ChargeTypeComptage(nbrtransfo, puissansSouscrit, puissansInstalle);
                    }

                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Langue.lbl_Menu);
            }
        }

        private void Txt_CodeForfait_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (LstForfait.Count != 0 && this.Txt_CodeForfait.Text.Length == SessionObject.Enumere.TailleForfait)
                {
                    CsForfait _LeForfait = LstForfait.FirstOrDefault(p => p.PRODUIT == CodeProduit && p.CODE == this.Txt_CodeForfait.Text);
                    if (_LeForfait != null)
                    {
                        this.Txt_LibelleForfait.Text = _LeForfait.LIBELLE;
                        this.Txt_CodeForfait.Tag = _LeForfait.PK_ID;
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
                    List<object> _LstObjet = ClasseMEthodeGenerique.RetourneListeObjet(LstForfait.Where(t => t.PRODUIT == CodeProduit).ToList());
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
                    this.Txt_CodeForfait.Tag  = _Leforfait.PK_ID ;
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
                        this.Txt_CodeMoisFacturation.Tag  = _LeMois.PK_ID ;
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
                    this.Txt_CodeMoisFacturation.Tag  = _LeMois.PK_ID ;
                }
                this.btn_moisdefacturation.IsEnabled = true;
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

        private void Txt_CodePussanceSoucrite_TextChanged(object sender, TextChangedEventArgs e)
        {
            //if (LstPuissance != null && LstPuissance.Count != 0)
            //&& this.Txt_CodePussanceSoucrite .Text.Length == SessionObject.Enumere.)
            //{

                //CsPuissance _LaPuissance = LstPuissance.FirstOrDefault(i=>i.VALEUR == Convert.ToDecimal(Txt_CodePussanceSoucrite.Text) && i.PRODUIT == laDetailDemande.Abonne.PRODUIT );
                //if (_LaPuissance != null)
                //{
                //    if (laDetailDemande.Abonne.PRODUIT == SessionObject.Enumere.ElectriciteMT)
                //    {
                //        int? nbrtransfo = null;
                //        if (laDetailDemande.Branchement.NOMBRETRANSFORMATEUR != null)
                //            nbrtransfo = laDetailDemande.Branchement.NOMBRETRANSFORMATEUR;
                //        int puissansSouscrit = Convert.ToInt32(_LaPuissance.VALEUR);
                //        int puissansInstalle = int.Parse(Txt_CodePuissanceUtilise.Text);
                //        ChargeTypeComptage(nbrtransfo, puissansSouscrit, puissansInstalle);
                //    }

                //}
                //else
                //{
                //    var w = new MessageBoxControl.MessageBoxChildWindow(Langue.lbl_Menu, Langue.MsgEltInexistent, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                //    w.OnMessageBoxClosed += (_, result) =>
                //    {
                //        this.Txt_CodePussanceSoucrite.Focus();
                //    };
                //    w.Show();
                //}
            //}
        }

        private void AdapterComptage(int? puissancesouscrite, int PuissanceUtilise, int? NOMBRETRANSFORMATEUR)
        {
            AcceuilServiceClient service = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
            //service.AdapterComptageCompleted += (s, args) =>
            //{
            //    if (args != null && args.Cancelled)
            //        return;
            //    Txt_TypeDeComptage.Text = args.Result.LIBELLE;
            //    Txt_TypeDeComptage.Tag = args.Result.CODE;
            //};
            //service.AdapterComptageAsync(puissancesouscrite, PuissanceUtilise, NOMBRETRANSFORMATEUR);
            //service.CloseAsync();
        }
        private void ChargeTypeComptage(int? nbrtransfo, int puissanceSouscrit, int puissanceInstalle)
        {
            try
            {
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.RetourneTypeComptageCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    List<CsTypeComptage> lesType = args.Result;
                    if (lesType != null && lesType.Count != 0)
                    {
                        Txt_TypeDeComptage.Text = lesType.First().LIBELLE;
                        Txt_TypeDeComptage.Tag = lesType.First().CODE;
                        lbl_TypeDeComptage.Tag = lesType.First().PK_ID;
                        return;
                    }
                };
                service.RetourneTypeComptageAsync(nbrtransfo, puissanceSouscrit, puissanceInstalle);
                service.CloseAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        private void Txt_CodeFrequence_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (LstFrequence.Count != 0 && this.Txt_CodeFrequence.Text.Length == SessionObject.Enumere.TaillePeriodicite)
                {
                    CsFrequence _LaFrequence = ClasseMEthodeGenerique.RetourneObjectFromList(LstFrequence, this.Txt_CodeFrequence.Text, "CODE");
                    if (_LaFrequence != null)
                    {
                        this.Txt_LibelleFrequence.Text = _LaFrequence.LIBELLE;
                        this.Txt_CodeFrequence.Tag  = _LaFrequence.PK_ID ;
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
        private void ValidationDemande(CsDemande _LaDemande)
        {
            try
            {
                //Lancer la transaction de mise a jour en base
                AcceuilServiceClient service1 = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service1.ValiderDemandeInitailisationAsync(_LaDemande);
                service1.ValiderDemandeInitailisationCompleted += (sr, res) =>
                {
                    if (!string.IsNullOrEmpty(res.Result))
                    {
                        if (!IsRejeterDemande)
                        {
                            string Retour = res.Result;
                            string[] coupe = Retour.Split('.');
                            Shared.ClasseMEthodeGenerique.InitWOrkflow(coupe[0], _LaDemande.LaDemande.FK_IDCENTRE, coupe[1], _LaDemande.LaDemande.FK_IDTYPEDEMANDE);
                        }
                        else
                        {
                            List<string> codes = new List<string>();
                            codes.Add(laDetailDemande.InfoDemande.CODE);

                            ClasseMEthodeGenerique.TransmettreDemande(codes, true, this);
                            //List<CsUtilisateur> leUser = new List<CsUtilisateur>();
                            //if (laDetailDemande.InfoDemande != null && laDetailDemande.InfoDemande.CODE != null)
                            //{
                            //    foreach (CsUtilisateur item in laDetailDemande.InfoDemande.UtilisateurEtapeSuivante)
                            //        leUser.Add(item);
                            //    Shared.ClasseMEthodeGenerique.NotifierMailDemande(leUser, "0001", laDetailDemande.LaDemande.NUMDEM, laDetailDemande.LaDemande.LIBELLETYPEDEMANDE);
                            //}
                        }



                    }
                    if (Closed != null)
                        Closed(this, new EventArgs());
                    this.DialogResult = true;
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
            this.OKButton.IsEnabled = false;
            if (EnregisterDemande(laDetailDemande))
                ValidationDemande(laDetailDemande);
        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
      
        private void Txt_ReferenceClient_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(this.Txt_ReferenceClient.Text))
                this.Txt_ReferenceClient.Text = this.Txt_ReferenceClient.Text.PadLeft(SessionObject.Enumere.TailleClient, '0');
        }

        private void cbo_typedoc_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        private void ChargerTypeDocument()
        {
            try
            {
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.ChargerTypeDocumentCompleted += (s, args) =>
                {
                    if ((args != null && args.Cancelled) || (args.Error != null))
                        return;

                    foreach (var item in args.Result)
                    {
                        LstTypeDocument.Add(item);
                    }
                    cbo_typedoc.ItemsSource = LstTypeDocument;
                    cbo_typedoc.DisplayMemberPath = "LIBELLE";
                    cbo_typedoc.SelectedValuePath = "PK_ID";

                    //this.Resources.Add("FuelList", LstTypeDocument);

                };
                service.ChargerTypeDocumentAsync();
                service.CloseAsync();

            }
            catch (Exception)
            {

                throw;
            }
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (cbo_typedoc.SelectedItem != null)
            {
                // Create an instance of the open file dialog box.
                var openDialog = new OpenFileDialog();
                // Set filter options and filter index.
                openDialog.Filter =
                    "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";
                openDialog.FilterIndex = 1;
                openDialog.Multiselect = false;
                // Call the ShowDialog method to show the dialog box.
                bool? userClickedOk = openDialog.ShowDialog();
                // Process input if the user clicked OK.
                if (userClickedOk == true)
                {
                    if (openDialog.Files != null && openDialog.Files.Count() > 0 && openDialog.File != null)
                    {
                        FileStream stream = openDialog.File.OpenRead();
                        var memoryStream = new MemoryStream();
                        stream.CopyTo(memoryStream);
                        image = memoryStream.GetBuffer();
                        formScanne = new UcImageScanne(memoryStream, SessionObject.ExecMode.Creation);
                        formScanne.Closed += new EventHandler(GetInformationFromChildWindowImagePreuve);
                        formScanne.Show();
                    }
                }
            }
        }
        private void GetInformationFromChildWindowImagePreuve(object sender, EventArgs e)
        {
            this.LstPiece.Add(new ObjDOCUMENTSCANNE { PK_ID = Guid.NewGuid(), NOMDOCUMENT = ((CsTypeDOCUMENTSCANNE)cbo_typedoc.SelectedItem).LIBELLE, FK_IDTYPEDOCUMENT = ((CsTypeDOCUMENTSCANNE)cbo_typedoc.SelectedItem).PK_ID, CONTENU = image, DATECREATION = DateTime.Now, DATEMODIFICATION = DateTime.Now, USERCREATION = UserConnecte.matricule, USERMODIFICATION = UserConnecte.matricule });
            this.dgListePiece.ItemsSource = this.LstPiece;
        }
        private void hyperlinkButtonPropScannee__Click(object sender, RoutedEventArgs e)
        {
            MemoryStream memoryStream = new MemoryStream(((HyperlinkButton)sender).Tag as byte[]);
            var ucImageScanne = new UcImageScanne(memoryStream, SessionObject.ExecMode.Modification);
            ucImageScanne.Show();
        }
        private void dgListePiece_CurrentCellChanged(object sender, EventArgs e)
        {
            dgListePiece.BeginEdit();
        }
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            var messageBox = new MessageBoxControl.MessageBoxChildWindow("Attention", "Ête-vous sure de vouloire supprimer la ligne?", MessageBoxControl.MessageBoxButtons.OkCancel, MessageBoxControl.MessageBoxIcon.Information);
            messageBox.OnMessageBoxClosed += (_, result) =>
            {
                if (messageBox.Result == MessageBoxResult.OK)
                {
                    ObjDOCUMENTSCANNE Fraix = (ObjDOCUMENTSCANNE)dgListePiece.SelectedItem;
                    this.LstPiece.Remove(Fraix);
                    this.dgListePiece.ItemsSource = this.LstPiece;
                }
                else
                {
                    return;
                }
            };
            messageBox.Show();
        }

        private void btn_RechercheClient_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                prgBar.Visibility = System.Windows.Visibility.Visible;
                if (Txt_ReferenceClient.Text.Length == SessionObject.Enumere.TailleClient)
                    ChargerClientFromReference(this.Txt_ReferenceClient.Text);
                else
                {
                    Message.Show("La reference saisie n'est pas correcte", "Infomation");
                }
            }
            catch (Exception ex)
            {
                Message.ShowInformation(ex.Message, "Demande");
            }
        }

        private void ChargerClientFromReference(string ReferenceClient)
        {
            try
            {
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.RetourneClientByReferenceCompleted += (s, args) =>
                {
                    if ((args != null && args.Cancelled) || (args.Error != null))
                        return;
                    if (args.Result != null && args.Result.Count > 1)
                    {
                        List<object> _Listgen = ClasseMEthodeGenerique.RetourneListeObjet(args.Result);
                        Galatee.Silverlight.MainView.UcListeGenerique ctr = new Galatee.Silverlight.MainView.UcListeGenerique(_Listgen, "CENTRE", "LIBELLESITE", "Liste des site");
                        ctr.Show();
                        ctr.Closed += new EventHandler(galatee_OkClickedChoixClient);
                    }
                    else
                    {
                        if (args.Result != null && args.Result.Count == 1)
                        {
                            CsClient leClient = args.Result.First();
                            leClient.TYPEDEMANDE = Tdem;
                            VerifieExisteDemande(leClient);
                        }
                        else
                            Message.ShowInformation("Aucun client trouvé pour le critère", "Information");
                    }
                };
                service.RetourneClientByReferenceAsync(ReferenceClient, lstIdCentre);
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
                CsClient _UnClient = (CsClient)ctrs.MyObject;
                _UnClient.TYPEDEMANDE = Tdem;
                VerifieExisteDemande(_UnClient);
            }
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
                            if (args.Result.DATEFIN == null && args.Result.ISSUPPRIME != true)
                            {
                                Message.ShowInformation("Il existe une demande numero " + args.Result.NUMDEM + " sur ce client", "Accueil");
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
        private void ChargeDetailDEvis(CsClient leclient)
        {

            try
            {
                leclient.TYPEDEMANDE = Tdem;
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient client = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                client.GeDetailByFromClientCompleted += (ssender, args) =>
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
                        //if (laDetailDemande.Abonne != null && laDetailDemande.Abonne.DRES == null)
                        //{
                            this.txtSite.Text = string.IsNullOrEmpty(laDetailDemande.LeClient.LIBELLESITE) ? string.Empty : laDetailDemande.LeClient.LIBELLESITE;
                            this.txtCentre.Text = string.IsNullOrEmpty(laDetailDemande.LeClient.LIBELLECENTRE) ? string.Empty : laDetailDemande.LeClient.LIBELLECENTRE;
                            this.txt_Produit.Text = string.IsNullOrEmpty(laDetailDemande.Abonne.LIBELLEPRODUIT) ? string.Empty : laDetailDemande.Abonne.LIBELLEPRODUIT;
                            this.txt_Produit.Tag = laDetailDemande.Abonne.FK_IDPRODUIT;
                            this.txtCentre.Tag = laDetailDemande.Abonne.FK_IDCENTRE;
                            CodeProduit = laDetailDemande.Abonne.PRODUIT;
                            this.txt_tdem.Text = SessionObject.LstTypeDemande.FirstOrDefault(t => t.CODE == Tdem).LIBELLE;
                            txt_tdem.Tag = SessionObject.LstTypeDemande.FirstOrDefault(t => t.CODE == Tdem);
                            AfficherInfoAbonnement(laDetailDemande.Abonne);
                            AfficherDocumentScanne(laDetailDemande.ObjetScanne);
                            if (laDetailDemande.Abonne.PRODUIT != SessionObject.Enumere.Electricite)
                            {
                                this.Txt_CodePussanceSoucrite.IsReadOnly = true;
                                this.Txt_CodePuissanceUtilise.IsReadOnly = true;
                            } 
                            
                            //if (laDetailDemande.LeClient.CATEGORIE != SessionObject.Enumere.CategorieEp)
                            //{
                            //    lbl_NbreFoyer.Visibility = System.Windows.Visibility.Collapsed;
                            //    this.Txt_NombreFoyer.Visibility = System.Windows.Visibility.Collapsed;
                            //}
                        //}
                        //else
                        //{
                        //    Message.ShowInformation("Ce abonné est résilié", "Info");
                        //    return;
                        //}
                    }
                };
                client.GeDetailByFromClientAsync(leclient);
            }
            catch (Exception ex)
            {
                prgBar.Visibility = System.Windows.Visibility.Collapsed;
                Message.ShowError("Erreur au chargement des donnéés", "Demande");
            }
        }
        private void ChargeDetailDEvis(int IdDemandeDevis)
        {

            AcceuilServiceClient client = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
            client.GetDevisByNumIdDevisCompleted += (ssender, args) =>
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
                    this.txt_Produit.Tag = laDetailDemande.Abonne.FK_IDPRODUIT;
                    this.txtCentre.Tag = laDetailDemande.Abonne.FK_IDCENTRE;
                    CodeProduit = laDetailDemande.Abonne.PRODUIT;
                    Tdem = laDetailDemande.LaDemande.TYPEDEMANDE;
                    this.txt_tdem.Text = SessionObject.LstTypeDemande.FirstOrDefault(t => t.CODE == Tdem).LIBELLE;
                    txt_tdem.Tag = SessionObject.LstTypeDemande.FirstOrDefault(t => t.CODE == Tdem);

                    this.Txt_Motif.Text = string.IsNullOrEmpty(laDetailDemande.LaDemande.MOTIF) ? string.Empty : laDetailDemande.LaDemande.MOTIF;
                    if (laDetailDemande.LstCommentaire != null && laDetailDemande.LstCommentaire.Count != 0)
                        this.Txt_MotifRejet.Text = laDetailDemande.LstCommentaire.First().COMMENTAIRE;


                    AfficherInfoAbonnement(laDetailDemande.Abonne);
                    AfficherDocumentScanne(laDetailDemande.ObjetScanne);
                    if (laDetailDemande.Abonne.PRODUIT != SessionObject.Enumere.Electricite)
                    {
                        this.Txt_CodePussanceSoucrite.IsReadOnly = true;
                        this.Txt_CodePuissanceUtilise.IsReadOnly = true;
                    }
                    IsGiserChamp(false);
                    IsRejeterDemande = true;
                }

            };
            client.GetDevisByNumIdDevisAsync(IdDemandeDevis);
        }

        private void AfficherDocumentScanne(List<ObjDOCUMENTSCANNE> _LesDocScanne)
        {
            try
            {
                if (_LesDocScanne != null && _LesDocScanne.Count != 0)
                {
                    this.dgListePiece.ItemsSource = null;
                    this.dgListePiece.ItemsSource = _LesDocScanne;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
       

    }
}