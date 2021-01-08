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
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
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
                _Lademande.Abonne.ESTEXONERETVA = this.chk_EstExoneration.IsChecked.Value;
                _Lademande.Abonne.ISBORNEPOSTE = this.chk_EstBornePoste.IsChecked.Value;

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
                if (laDetailDemande.Abonne.PRODUIT == SessionObject.Enumere.ElectriciteMT)
                    _Lademande.Branchement.PUISSANCEINSTALLEE = string.IsNullOrEmpty(this.Txt_CodePuissanceUtilise.Text) ? 0 : Convert.ToDecimal(this.Txt_CodePuissanceUtilise.Text);
                else
                    _Lademande.Branchement = null;

                if (Txt_CodeCategorie.Tag != null )
                {
                    _Lademande.LeClient.CATEGORIE = Txt_CodeCategorie.Text;
                    _Lademande.LeClient.FK_IDCATEGORIE = (int)Txt_CodeCategorie.Tag;
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

                _Lademande.Branchement = null;
                _Lademande.Ag = null  ;
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
                    AcceuilServiceClient service = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
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
                if (SessionObject.LstCodeApplicationTaxe!=null && SessionObject.LstCodeApplicationTaxe.Count != 0)
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

        private void AfficherInfoAbonnement(CsDemande _Lademande )
        {
            try
            {

                this.Txt_CodeForfait.Text = string.IsNullOrEmpty(_Lademande.Abonne .FORFAIT) ? string.Empty : _Lademande.Abonne .FORFAIT;
                this.Txt_CodeTarif.Text = string.IsNullOrEmpty(_Lademande.Abonne .TYPETARIF) ? string.Empty : _Lademande.Abonne .TYPETARIF;
                this.Txt_CodeFrequence.Text = string.IsNullOrEmpty(_Lademande.Abonne .PERFAC) ? string.Empty : _Lademande.Abonne .PERFAC;
                this.Txt_CodeMoisFacturation.Text = string.IsNullOrEmpty(_Lademande.Abonne .MOISFAC) ? string.Empty : _Lademande.Abonne .MOISFAC;
                this.Txt_DateAbonnement.Text = (_Lademande.Abonne .DABONNEMENT == null) ? string.Empty : Convert.ToDateTime(_Lademande.Abonne .DABONNEMENT.Value).ToShortDateString();
                this.Txt_DateResiliation.Text = (_Lademande.Abonne .DRES == null) ? string.Empty : Convert.ToDateTime(_Lademande.Abonne .DRES.Value).ToShortDateString();
                this.Txt_LibelleFrequence.Text = string.IsNullOrEmpty(_Lademande.Abonne .LIBELLEFREQUENCE) ? string.Empty : _Lademande.Abonne .LIBELLEFREQUENCE;
                this.Txt_LibelleTarif.Text = string.IsNullOrEmpty(_Lademande.Abonne .LIBELLETARIF) ? string.Empty : _Lademande.Abonne .LIBELLETARIF;
                this.Txt_LibMoisFact.Text = string.IsNullOrEmpty(_Lademande.Abonne .LIBELLEMOISFACT) ? string.Empty : _Lademande.Abonne .LIBELLEMOISFACT;
                this.Txt_LibelleForfait.Text = string.IsNullOrEmpty(_Lademande.Abonne .LIBELLEFORFAIT) ? string.Empty : _Lademande.Abonne .LIBELLEFORFAIT;
                this.txt_DebutPeriodeExo.Text = string.IsNullOrEmpty(_Lademande.Abonne .DEBUTEXONERATIONTVA) ? string.Empty :ClasseMEthodeGenerique.FormatPeriodeMMAAAA ( _Lademande.Abonne .DEBUTEXONERATIONTVA);
                this.txt_FinPeriodeExo.Text = string.IsNullOrEmpty(_Lademande.Abonne .FINEXONERATIONTVA) ? string.Empty : ClasseMEthodeGenerique.FormatPeriodeMMAAAA(_Lademande.Abonne .FINEXONERATIONTVA);
                this.chk_EstExoneration.IsChecked = _Lademande.Abonne.ESTEXONERETVA == true ? true : false;
                this.chk_EstBornePoste.IsChecked = _Lademande.Abonne.ISBORNEPOSTE == true ? true : false;
                this.Txt_AvanceSurConso.Text = _Lademande.Abonne .AVANCE!= null ? Convert.ToDecimal(_Lademande.Abonne .AVANCE.Value).ToString() : "0";
                this.Txt_CodePussanceSoucrite.Text =_Lademande.Abonne  != null && _Lademande.Abonne .PUISSANCE != null ? Convert.ToDecimal(_Lademande.Abonne .PUISSANCE.ToString()).ToString() :"0";
                this.Txt_CodePuissanceUtilise.Text = _Lademande.Branchement != null && _Lademande.Branchement.PUISSANCEINSTALLEE != null ? Convert.ToDecimal(_Lademande.Branchement.PUISSANCEINSTALLEE).ToString() : "0";


                if (_Lademande.Abonne .PRODUIT == SessionObject.Enumere.ElectriciteMT)
                {
                    this.Txt_TypeDeComptage.Visibility = Visibility.Visible;
                    this.lbl_TypeDeComptage.Visibility = Visibility.Visible;
                    this.Txt_TypeDeComptage.Text = _Lademande.Abonne .LIBELLETYPECOMPTAGE!=null?_Lademande.Abonne .LIBELLETYPECOMPTAGE:string.Empty;
                }
                if (_Lademande.Abonne .PRODUIT != SessionObject.Enumere.ElectriciteMT)
                {
                    this.Txt_CodeTarif.Text = laDetailDemande.Abonne.TYPETARIF;
                    this.Txt_LibelleTarif .Text = laDetailDemande.Abonne.LIBELLETARIF ;
                }
                if (_Lademande.LeClient != null)
                {
                    this.Txt_CodeCategorie.Text = _Lademande.LeClient.CATEGORIE;
                    this.Txt_LibelleCategorie .Text = _Lademande.LeClient.LIBELLECATEGORIE ;
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
                //if ( !string.IsNullOrEmpty( CodeProduit))
                //{
                //    this.btn_tarifs.IsEnabled = false;
             


                //    List<object> _LstObjet = ClasseMEthodeGenerique.RetourneListeObjet(LstTarif.Where(u=>u.FK_IDPRODUIT == laDetailDemande.Abonne.FK_IDPRODUIT).ToList());
                //    UcListeGenerique ctr = new UcListeGenerique(_LstObjet, "CODE", "LIBELLE", Langue.lbl_Menu);
                //    ctr.Closed += new EventHandler(galatee_OkClickedBtnTarif);
                //    ctr.Show();
                //}


                List<object> _LstObjet = new List<object>();

                if (laDetailDemande.Abonne.PRODUIT != SessionObject.Enumere.ElectriciteMT)
                {

                    List<CsTarif> _Liste = new List<CsTarif>();
                    List<CsTarif> maListe = new List<CsTarif>();

                    int idProduit = 0;
                    if (laDetailDemande.Abonne.FK_IDPRODUIT != 0)
                        idProduit = laDetailDemande.Abonne.FK_IDPRODUIT;

                    int? idCategorie = null;
                    if (laDetailDemande.LeClient.FK_IDCATEGORIE != null)
                        idCategorie = laDetailDemande.LeClient.FK_IDCATEGORIE;


                    int? idRegale = null;
                    if (laDetailDemande.LstCanalistion != null && laDetailDemande.LstCanalistion.Count != 0)
                        idRegale = laDetailDemande.LstCanalistion.First().FK_IDREGLAGECOMPTEUR;

                    int? idPuissance = (int?)this.Txt_CodePussanceSoucrite.Tag;




                    AcceuilServiceClient service = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                    service.ChargerTypeTarifCompleted += (s, args) =>
                    {
                        if (args != null && args.Cancelled)
                            return;
                        _Liste = args.Result;


                        List<CsTarif> _listeReg = _Liste.Where(t => t.FK_IDREGLAGECOMPTEUR == idRegale).ToList();

                        var lListe = _Liste.Select(p => new { p.CODE, p.FK_IDTYPETARIF, idProduit }).Distinct();
                        if (_listeReg != null && _listeReg.Count > 0)
                            lListe = _listeReg.Select(p => new { p.CODE, p.FK_IDTYPETARIF, idProduit }).Distinct();


                        maListe.Clear();
                        foreach (var item in lListe)
                            maListe.Add(new CsTarif
                            {
                                CODE = item.CODE,
                                PK_ID = item.FK_IDTYPETARIF,
                                FK_IDPRODUIT = item.idProduit
                            });


                        if (maListe == null)
                        {
                            Message.ShowInformation("Aucun code tarif trouvé pour cette catégorie", Langue.lbl_Menu);
                            return;

                        }

                        //List<object> _LstObjet = new List<object>();
                        List<object> lst;

                        foreach (CsTarif item in maListe)
                        {
                            lst = new List<object>();
                            lst = ClasseMEthodeGenerique.RetourneListeObjet(LstTarif.Where(t => t.CODE == item.CODE && t.FK_IDPRODUIT == idProduit).ToList());

                            _LstObjet.AddRange(lst);
                        }

                        if (_LstObjet != null && _LstObjet.Count > 0)
                        {
                            UcListeGenerique ctr = new UcListeGenerique(_LstObjet, "CODE", "LIBELLE", "Liste");
                            ctr.Closed += new EventHandler(galatee_OkClickedBtnTarif);
                            ctr.Show();

                        }
                        //else
                        //{
                        //    _LstObjet = ClasseMEthodeGenerique.RetourneListeObjet(LstTarif.Where(t => t.FK_IDPRODUIT == laDetailDemande.Abonne.FK_IDPRODUIT).ToList());
                        //    UcListeGenerique ctr = new UcListeGenerique(_LstObjet, "CODE", "LIBELLE", "Liste");
                        //    ctr.Closed += new EventHandler(galatee_OkClickedBtnTarif);
                        //    ctr.Show();
                        //}

                    };
                    service.ChargerTypeTarifAsync(idProduit, idPuissance, idCategorie, null, null);
                    service.CloseAsync();



                }




            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Langue.lbl_Menu);
            }
        }
        List<CsTarif> lstDesTarif = new List<CsTarif>();
        List<CsCategorieClient> lstCategorieDistinct = new List<CsCategorieClient>();
        private void ChargerPuissanceEtTarif(int idProduit, int? idPuissance, int? idCategorie, int? idReglageCompteur,int? idtarif)
        {
            try
            {
                AcceuilServiceClient service = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                service.ChargerTypeTarifCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    lstDesTarif = args.Result;
                    if (lstDesTarif == null)
                    {
                        Message.ShowInformation ("Aucune catégorie trouvé pour ce tarif", Langue.lbl_Menu);
                        return;
                    
                    }
                  /*
                        lstCategorieDistinct.Clear();
                        var lesDistinctCategorie = lstDesTarif.Select(p => new { p.FK_IDCATEGORIE, p.CATEGORIE, p.LIBELLECATEGORIE }).Distinct();
                        foreach (var item in lesDistinctCategorie)
                            lstCategorieDistinct.Add(new CsCategorieClient
                            {
                                CODE = item.CATEGORIE,
                                PK_ID = item.FK_IDCATEGORIE,
                                LIBELLE = item.LIBELLECATEGORIE
                            });
                        if (lstCategorieDistinct != null && lstCategorieDistinct.Count == 1)
                        {
                            this.Txt_CodeCategorie.Text = lstCategorieDistinct.First().CODE;
                            this.Txt_LibelleCategorie.Text = lstCategorieDistinct.First().LIBELLE;
                            this.Txt_CodeCategorie.Tag = lstCategorieDistinct.First().PK_ID;
                        }
                        else
                        {
                            CsCategorieClient laCateg = lstCategorieDistinct.FirstOrDefault(t => t.PK_ID == laDetailDemande.LeClient.FK_IDCATEGORIE);
                            if (laCateg != null)
                            {
                                this.Txt_CodeCategorie.Text = laCateg.CODE;
                                this.Txt_LibelleCategorie.Text = laCateg.LIBELLE;
                                this.Txt_CodeCategorie.Tag = laCateg.PK_ID;
                            }
                            else
                            {
                                this.Txt_CodeCategorie.Text = string.Empty;
                                this.Txt_LibelleCategorie.Text = string.Empty;
                                this.Txt_CodeCategorie.Tag = null;
                            }
                        }
                   * 
                   * */
                    
                        lstPuissanceDistinct.Clear();
                        var lesDistinctPuissance = lstDesTarif.Select(p => new { p.FK_IDPUISSANCE, p.PUISSANCE, p.VALEUR, p.FK_IDPRODUIT }).Distinct();
                        foreach (var item in lesDistinctPuissance)
                            lstPuissanceDistinct.Add(new CsPuissance
                            {
                                CODE = item.PUISSANCE,
                                PK_ID = item.FK_IDPUISSANCE,
                                VALEUR = item.VALEUR,
                                FK_IDPRODUIT = item.FK_IDPRODUIT
                            });
                        if (lstPuissanceDistinct != null && lstPuissanceDistinct.Count == 1)
                        {
                            this.Txt_CodePussanceSoucrite.Text = lstPuissanceDistinct.First().VALEUR.ToString();
                            this.Txt_CodePussanceSoucrite.Tag = lstPuissanceDistinct.First().PK_ID;
                        }
                        else
                        {
                            //CsPuissance laPuissance = LstPuissance.FirstOrDefault(t => t.VALEUR == laDetailDemande.Abonne.PUISSANCE
                            CsPuissance laPuissance = LstPuissance.FirstOrDefault(t => t.PK_ID == int.Parse(idPuissance.Value.ToString())
                                && t.FK_IDPRODUIT == laDetailDemande.Abonne.FK_IDPRODUIT);
                            if (laPuissance != null)
                            {
                                //this.Txt_CodePussanceSoucrite.Text = laPuissance.VALEUR.ToString();
                                this.Txt_CodePussanceSoucrite.Tag = laPuissance.PK_ID;
                            }
                            else
                            {
                                this.Txt_CodePussanceSoucrite.Text = string.Empty;
                                this.Txt_CodePussanceSoucrite.Tag = null;
                            }
                        }
                    
                };
                service.ChargerTypeTarifAsync(idProduit, idPuissance, idCategorie, idReglageCompteur, idtarif);
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                
                throw ex ;
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
                    this.Txt_CodeTarif.Tag = _LeTarif.PK_ID;
/*                    if (laDetailDemande != null && laDetailDemande.Abonne != null && this.Txt_CodeTarif.Tag != null)
                    {
                        if (laDetailDemande.Abonne.PRODUIT != SessionObject.Enumere.ElectriciteMT)
                        {
                            int idProduit = 0;
                            if (laDetailDemande.Abonne.FK_IDPRODUIT != 0)
                                idProduit = laDetailDemande.Abonne.FK_IDPRODUIT;
                            
                            int? IdRegale = null;
                            if (laDetailDemande.LstCanalistion != null && laDetailDemande.LstCanalistion.Count != 0)
                                IdRegale = laDetailDemande.LstCanalistion.First().FK_IDREGLAGECOMPTEUR;

                            if (idProduit != 0)
                                ChargerPuissanceEtTarif(idProduit, null , null, IdRegale, _LeTarif.PK_ID );
                        }
                    }
 * */

                }

            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Langue.lbl_Menu);
            }
        }

        List<CsPuissance> lstPuissanceDistinct = new List<CsPuissance>();
        private void ChargerPuissance(int idProduit, int? idPuissance, int? idCategorie, int? idReglageCompteur,int? idtarif)
        {
            try
            {
                AcceuilServiceClient service = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                service.ChargerTypeTarifCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    lstDesTarif.Clear();
                    lstDesTarif = args.Result;
                    if (lstDesTarif == null)
                    {
                        Message.ShowInformation ("Aucune catégorie trouvé pour ce tarif", Langue.lbl_Menu);
                        return;
                    
                    }
                    lstPuissanceDistinct.Clear();
                    var lesDistinctPuissance = lstDesTarif.Select(p => new { p.FK_IDPUISSANCE , p.PUISSANCE , p.VALEUR ,idProduit  }).Distinct();
                    foreach (var item in lesDistinctPuissance)
                        lstPuissanceDistinct.Add(new CsPuissance 
                        {
                            CODE = item.PUISSANCE ,
                            PK_ID = item.FK_IDPUISSANCE ,
                            VALEUR=item.VALEUR ,
                            FK_IDPRODUIT = item.idProduit 
                        });
                    if (lstPuissanceDistinct != null && lstPuissanceDistinct.Count == 1)
                    {
                        this.Txt_CodePussanceSoucrite.Text = lstPuissanceDistinct.First().VALEUR .ToString();
                        this.Txt_CodePussanceSoucrite.Tag = lstPuissanceDistinct.First().PK_ID;
                    }
                    else
                    {
                        CsPuissance laPuissance = lstPuissanceDistinct.FirstOrDefault(t => t.VALEUR  == laDetailDemande.Abonne.PUISSANCE 
                            && t.FK_IDPRODUIT==laDetailDemande.Abonne.FK_IDPRODUIT  );
                        if (laPuissance != null)
                        {
                            this.Txt_CodePussanceSoucrite.Text = laPuissance.VALEUR.ToString();
                            this.Txt_CodePussanceSoucrite.Tag = laPuissance.PK_ID;
                        }
                        else
                        {
                            this.Txt_CodePussanceSoucrite.Text = string.Empty;
                            this.Txt_CodePussanceSoucrite.Tag = null;
                        }
                    }
                };
                service.ChargerTypeTarifAsync(idProduit, idPuissance, idCategorie, idReglageCompteur, idtarif);
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                
                throw ex ;
            }

        }
        private void btn_PussSouscrite_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (laDetailDemande != null && laDetailDemande.Abonne != null)
                {
                    List<object> _LstObjet = new List<object>();

                    if (laDetailDemande.Abonne.PRODUIT != SessionObject.Enumere.ElectriciteMT)
                    {

                        List<CsTarif> _Liste = new List<CsTarif>();
                        List<CsPuissance> maListe = new List<CsPuissance>();

                        int idProduit = 0;
                        if (laDetailDemande.Abonne.FK_IDPRODUIT != 0)
                            idProduit = laDetailDemande.Abonne.FK_IDPRODUIT;

                        int? idCategorie = null;
                        if (laDetailDemande.LeClient.FK_IDCATEGORIE != null)
                            idCategorie = laDetailDemande.LeClient.FK_IDCATEGORIE;


                        int? IdRegale = null;
                        if (laDetailDemande.LstCanalistion != null && laDetailDemande.LstCanalistion.Count != 0)
                            IdRegale = laDetailDemande.LstCanalistion.First().FK_IDREGLAGECOMPTEUR;

                        //int? idTarif = (int?)this.Txt_CodeTarif.Tag;




                        AcceuilServiceClient service = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                        service.ChargerTypeTarifCompleted += (s, args) =>
                        {
                            if (args != null && args.Cancelled)
                                return;
                            _Liste = args.Result;


                            maListe.Clear();
                            var lListe = _Liste.Select(p => new { p.CODE, p.FK_IDPUISSANCE, p.PUISSANCE, p.VALEUR, idProduit}).Distinct();
                            foreach (var item in lListe)
                                maListe.Add(new CsPuissance
                                {
                                    CODE = item.PUISSANCE,
                                    PK_ID = item.FK_IDPUISSANCE,
                                    VALEUR = item.VALEUR,
                                    FK_IDPRODUIT = item.idProduit
                                });


                            if (maListe == null)
                            {
                                Message.ShowInformation("Aucune catégorie trouvée pour ce tarif", Langue.lbl_Menu);
                                return;

                            }

                            //List<object> _LstObjet = new List<object>();
                            List<object> lst;

                            foreach (CsPuissance item in maListe)
                            {
                                lst = new List<object>();
                                lst = ClasseMEthodeGenerique.RetourneListeObjet(LstPuissance.Where(t => t.VALEUR == item.VALEUR && t.FK_IDPRODUIT == idProduit).ToList());

                                _LstObjet.AddRange(lst);
                            }

                            if (_LstObjet != null && _LstObjet.Count > 0)
                            {
                                UcListeGenerique ctr = new UcListeGenerique(_LstObjet, "CODE", "LIBELLE", "Liste");
                                ctr.Closed += new EventHandler(galatee_OkClickedBtnpuissanceSouscrite);
                                ctr.Show();

                            }
                            else
                            {
                                _LstObjet = ClasseMEthodeGenerique.RetourneListeObjet(LstPuissance.Where(t => t.FK_IDPRODUIT == laDetailDemande.Abonne.FK_IDPRODUIT).ToList());
                                UcListeGenerique ctr = new UcListeGenerique(_LstObjet, "CODE", "LIBELLE", "Liste");
                                ctr.Closed += new EventHandler(galatee_OkClickedBtnpuissanceSouscrite);
                                ctr.Show();
                            }


                        };
                        service.ChargerTypeTarifAsync(idProduit, null, idCategorie, null, null);
                        service.CloseAsync();



                    }
                    else
                    {
                        _LstObjet = ClasseMEthodeGenerique.RetourneListeObjet(LstPuissance.Where(t => t.FK_IDPRODUIT == laDetailDemande.Abonne.FK_IDPRODUIT).ToList());
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
                    this.Txt_CodePussanceSoucrite.Text = _LaPuissanceSelect.VALEUR .ToString();
                    //this.Txt_CodePussanceSoucrite.Tag = _LaPuissanceSelect.VALEUR;
                    this.Txt_CodePussanceSoucrite.Tag = _LaPuissanceSelect.PK_ID;

                    if (laDetailDemande.Abonne.PRODUIT == SessionObject.Enumere.ElectriciteMT)
                    {
                        int? nbrtransfo = null;
                        decimal puissansInstal = decimal.Parse(Txt_CodePuissanceUtilise.Text);
                        if (laDetailDemande.Branchement.NOMBRETRANSFORMATEUR != null)
                            nbrtransfo = laDetailDemande.Branchement.NOMBRETRANSFORMATEUR;
                        int puissansSouscrit = Convert.ToInt32(_LaPuissanceSelect.VALEUR);
                        int puissansInstalle = Convert.ToInt16(puissansInstal);
                        ChargeTypeComptage(nbrtransfo, puissansSouscrit, puissansInstalle);
                    }
                    else
                    {
                        int idProduit = 0;
                        if (laDetailDemande.Abonne.FK_IDPRODUIT != 0)
                            idProduit = laDetailDemande.Abonne.FK_IDPRODUIT;

                        int? IdRegale = null;
                        if (laDetailDemande.LstCanalistion != null && laDetailDemande.LstCanalistion.Count != 0)
                            IdRegale = laDetailDemande.LstCanalistion.First().FK_IDREGLAGECOMPTEUR;

                        int? idtarif = null;
                        if (this.Txt_CodeTarif.Tag != null)
                            idtarif = (int)this.Txt_CodeTarif.Tag;

                        if (idProduit != null)
                            ChargerPuissanceEtTarif(idProduit, _LaPuissanceSelect.PK_ID , null, IdRegale, idtarif);
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

   

            List<object> _LstObjet = new List<object>();

            if (laDetailDemande.Abonne.PRODUIT != SessionObject.Enumere.ElectriciteMT)
            {

                List<CsTarif> _Liste = new List<CsTarif>();
                List<CsTarif> maListe = new List<CsTarif>();

                int idProduit = 0;
                if (laDetailDemande.Abonne.FK_IDPRODUIT != 0)
                    idProduit = laDetailDemande.Abonne.FK_IDPRODUIT;

                int? idCategorie = null;
                if (laDetailDemande.LeClient.FK_IDCATEGORIE != null)
                    idCategorie = laDetailDemande.LeClient.FK_IDCATEGORIE;


                int? idRegale = null;
                if (laDetailDemande.LstCanalistion != null && laDetailDemande.LstCanalistion.Count != 0)
                    idRegale = laDetailDemande.LstCanalistion.First().FK_IDREGLAGECOMPTEUR;

                int? idPuissance = (int?)this.Txt_CodePussanceSoucrite.Tag;




                AcceuilServiceClient service = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.ChargerTypeTarifCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    _Liste = args.Result;

                    List<CsTarif> _listeReg = _Liste.Where(t => t.FK_IDREGLAGECOMPTEUR == idRegale).ToList();

                    var lListe = _Liste.Select(p => new { p.CODE, p.FK_IDTYPETARIF, idProduit }).Distinct();
                    if (_listeReg != null && _listeReg.Count > 0)
                        lListe = _listeReg.Select(p => new { p.CODE, p.FK_IDTYPETARIF, idProduit }).Distinct();

                    maListe.Clear();
                    foreach (var item in lListe)
                        maListe.Add(new CsTarif
                        {
                            CODE = item.CODE,
                            PK_ID = item.FK_IDTYPETARIF,
                            FK_IDPRODUIT = item.idProduit
                        });


                    if (maListe == null)
                    {
                        Message.ShowInformation("Aucun code tarif trouvé pour cette catégorie", Langue.lbl_Menu);
                        return;

                    }

                    //List<object> _LstObjet = new List<object>();
                    List<object> lst;

                    foreach (CsTarif item in maListe)
                    {
                        lst = new List<object>();
                        lst = ClasseMEthodeGenerique.RetourneListeObjet(LstTarif.Where(t => t.CODE == item.CODE && t.FK_IDPRODUIT == idProduit).ToList());

                        _LstObjet.AddRange(lst);
                    }

                    if (_LstObjet != null && _LstObjet.Count > 0)
                    {
                        UcListeGenerique ctr = new UcListeGenerique(_LstObjet, "CODE", "LIBELLE", "Liste");
                        CsTarif _LeTarif = (CsTarif)ctr.MaListe.First();
                        this.Txt_CodeTarif.Text = _LeTarif.CODE;
                        this.Txt_CodeTarif.Tag = _LeTarif.PK_ID;

                    }
                    else
                    {
                        _LstObjet = ClasseMEthodeGenerique.RetourneListeObjet(LstTarif.Where(t => t.FK_IDPRODUIT == laDetailDemande.Abonne.FK_IDPRODUIT).ToList());
                        UcListeGenerique ctr = new UcListeGenerique(_LstObjet, "CODE", "LIBELLE", "Liste");
                        CsTarif _LeTarif = (CsTarif)ctr.MaListe.First();
                        this.Txt_CodeTarif.Text = _LeTarif.CODE;
                        this.Txt_CodeTarif.Tag = _LeTarif.PK_ID;
                    }


                };
                service.ChargerTypeTarifAsync(idProduit, idPuissance, idCategorie, null, null);
                service.CloseAsync();



            }




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
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
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
                service1.CreeDemandeAsync(_LaDemande,true );
                service1.CreeDemandeCompleted  += (sr, res) =>
                {
                    if (res.Result != null)
                    {
                        Message.ShowInformation("La demande a été créée avec succès. Numéro de votre demande : " + res.Result.NUMDEM,
                        Silverlight.Resources.Devis.Languages.txtDevis);
                        this.DialogResult = false;
                    }
                    else
                        Message.ShowError("Une erreur s'est produite a la création de la demande ", "CreeDemande");
                    
                };
                service1.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;

            }
        }
        private void ValidationDemandeSiteReprise(CsDemande _LaDemande)
        {
            try
            {
                //Lancer la transaction de mise a jour en base
                AcceuilServiceClient service1 = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service1.CreationDemandeSuiteRejetAsync(_LaDemande, true);
                service1.CreationDemandeSuiteRejetCompleted += (sr, res) =>
                {
                    if (string.IsNullOrEmpty( res.Result))
                    {
                        Message.ShowInformation("La demande transmise avec succes",
                        Silverlight.Resources.Devis.Languages.txtDevis);
                        this.DialogResult = false;
                    }
                    else
                        Message.ShowError("Une erreur s'est produite a la transmission de la demande ", "CreeDemande");

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
            if (string.IsNullOrEmpty(this.Txt_CodeCategorie.Text))
            {
                Message.ShowInformation("Sélectionnez la catégorie", "Demande");
                this.OKButton.IsEnabled = true;
                return;
            }

            if (laDetailDemande.Abonne.PRODUIT == SessionObject.Enumere.ElectriciteMT && Convert.ToDecimal(this.Txt_CodePussanceSoucrite.Text) > Convert.ToDecimal(this.Txt_CodePuissanceUtilise.Text))
            {
                Message.ShowInformation("La puissance souscrite ne doit pas être supérieure à la puissance installée", "Demande");
                this.OKButton.IsEnabled = true;
                return;
            }


            if (EnregisterDemande(laDetailDemande))
            {
                if (!IsRejeterDemande)
                ValidationDemande(laDetailDemande);
                else
                    ValidationDemandeSiteReprise(laDetailDemande);
            }
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
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
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
            var messageBox = new MessageBoxControl.MessageBoxChildWindow("Attention", "Êtes-vous sûr de vouloir supprimer la ligne?", MessageBoxControl.MessageBoxButtons.OkCancel, MessageBoxControl.MessageBoxIcon.Information);
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
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
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
                    AcceuilServiceClient service = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
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
                client.ChargerDetailClientCompleted  += (ssender, args) =>
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
                            AfficherInfoAbonnement(laDetailDemande);
                            AfficherDocumentScanne(laDetailDemande.ObjetScanne);
                            if (laDetailDemande.Abonne.PRODUIT != SessionObject.Enumere.Electricite)
                            {
                                this.Txt_CodePussanceSoucrite.IsReadOnly = true;
                                //this.Txt_CodePuissanceUtilise.IsReadOnly = true;
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

                            if (laDetailDemande.Abonne.PRODUIT == SessionObject.Enumere.Eau)
                                this.chk_EstBornePoste.Visibility = System.Windows.Visibility.Collapsed;

                    }
                };
                client.ChargerDetailClientAsync(leclient);
            }
            catch (Exception ex)
            {
                prgBar.Visibility = System.Windows.Visibility.Collapsed;
                Message.ShowError("Erreur au chargement des donnéés", "Demande");
            }
        }
        private void ChargeDetailDEvis(int IdDemandeDevis)
        {

            AcceuilServiceClient client = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
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
                    this.txt_Produit.Tag = laDetailDemande.Abonne.FK_IDPRODUIT;
                    this.txtCentre.Tag = laDetailDemande.Abonne.FK_IDCENTRE;
                    CodeProduit = laDetailDemande.Abonne.PRODUIT;
                    Tdem = laDetailDemande.LaDemande.TYPEDEMANDE;
                    this.txt_tdem.Text = SessionObject.LstTypeDemande.FirstOrDefault(t => t.CODE == Tdem).LIBELLE;
                    txt_tdem.Tag = SessionObject.LstTypeDemande.FirstOrDefault(t => t.CODE == Tdem);

                    this.Txt_Motif.Text = string.IsNullOrEmpty(laDetailDemande.LaDemande.MOTIF) ? string.Empty : laDetailDemande.LaDemande.MOTIF;
                    //if (laDetailDemande.LstCommentaire != null && laDetailDemande.LstCommentaire.Count != 0)
                    if (laDetailDemande.AnnotationDemande != null && laDetailDemande.AnnotationDemande.Count != 0)
                        this.Txt_MotifRejet.Text = laDetailDemande.AnnotationDemande.First().COMMENTAIRE;


                    AfficherInfoAbonnement(laDetailDemande);
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
            client.ChargerDetailDemandeAsync (IdDemandeDevis,string.Empty );
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

        private void btn_Categorie_Click(object sender, RoutedEventArgs e)
        {
            if (lstCategorieDistinct != null && lstCategorieDistinct.Count != 0)
            {
                List<object> _LstObject = ClasseMEthodeGenerique.RetourneListeObjet(lstCategorieDistinct);
                UcListeGenerique ctr = new UcListeGenerique(_LstObject, "CODE", "LIBELLE", "Liste");
                ctr.Closed += new EventHandler(galatee_OkClickedBtnCategorie);
                this.IsEnabled = false;
                ctr.Show();
            }
        }
        void galatee_OkClickedBtnCategorie(object sender, EventArgs e)
        {
            this.IsEnabled = true;
            UcListeGenerique ctrs = sender as UcListeGenerique;
            if (ctrs.isOkClick)
            {
                CsCategorieClient _LaCateg = (CsCategorieClient)ctrs.MyObject;
                this.Txt_CodeCategorie.Text = _LaCateg.CODE;
                if (laDetailDemande.Abonne.PRODUIT != SessionObject.Enumere.ElectriciteMT)
                {
                    this.Txt_CodeCategorie.Text = _LaCateg.CODE;
                    this.Txt_LibelleCategorie.Text = _LaCateg.LIBELLE ;
                }
            }
        }
       

    }
}