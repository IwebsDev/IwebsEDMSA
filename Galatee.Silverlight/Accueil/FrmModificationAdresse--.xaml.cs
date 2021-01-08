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
using Galatee.Silverlight.Resources.Accueil;
//using Galatee.Silverlight.serviceWeb;
using Galatee.Silverlight.MainView;
using Galatee.Silverlight.Shared;
using System.IO;

namespace Galatee.Silverlight.Accueil
{
    public partial class FrmModificationAdresse : ChildWindow
    {
        public FrmModificationAdresse()
        {
            InitializeComponent();
        }

        List<CsCommune> LstCommuneAll;
        List<CsQuartier> LstQuartierAll;
        List<CsQuartier> LstQuartierCommuneSelect;

        List<CsSecteur> LstSecteurAll;
        List<CsSecteur> LstSecteurQuartierSelect;

        List<CsRues> LstRuesAll;
        List<CsRues> LstRuesSecteurSelect;

        CsCommune LaCommuneSelect = new CsCommune();
        CsQuartier LeQuartierSelect = new CsQuartier();
        CsTournee LaTourneSelect = new CsTournee();
        CsRues LaRueSelect = new CsRues();
        CsSecteur LeSecteurSelect = new CsSecteur();
        CsAg _LeAg = new CsAg();
        CsAg AdresseRechercher = new CsAg();

        List<CsTypeBranchement> LstTypeBranchement = new List<CsTypeBranchement>();

        List<CsCommune> LstCommune = new List<CsCommune>();
        List<CsQuartier> LstQuartier = new List<CsQuartier>();
        List<CsTournee> LstZone;
        CsDemande LaDemande = new CsDemande();

        CsCentre LeCentreSelect = new CsCentre();
        CsProduit LeProduitSelect = new CsProduit();

        CsCodeTaxeApplication LstCodeApplicationTaxeSelect = new CsCodeTaxeApplication();
        List<CsCentre> LstCentre = new List<CsCentre>();
        List<CsProduit> ListeDesProduitDuSite = new List<CsProduit>();

        string TypeDemande = string.Empty;
        void Translate()
        {
            //Gestion de la langue
            this.lbl_Commune.Content = Langue.lbl_Commune;
            this.lbl_Etage.Content = Langue.lbl_Etage;
            this.lbl_Lot.Content = Langue.lbl_Lot;
            this.lbl_NomProprietaire.Content = Langue.lbl_NomProprietaire;
            this.lbl_NumRue.Content = Langue.lbl_NumRue;
            this.lbl_Quartier.Content = Langue.lbl_Quartier;
            this.lbl_RegroupementCompteur.Content = Langue.lbl_RegroupementCompteur;
            this.lbl_Rue.Content = Langue.lbl_Rue;
            this.lbl_Secteur.Content = Langue.lbl_Secteur;
            this.lbl_Sequence.Content = Langue.lbl_Ordre;
            this.lbl_Telephone.Content = Langue.lbl_Telephone;
            this.lbl_Tournee.Content = Langue.lbl_Tournee;
            this.lbl_AutreInfoRue.Content = Langue.lbl_AutreInfoRue;
            this.lbl_NumeroUnite.Content = Langue.lbl_NumeroUnite;
            this.lbl_autreInfo.Content = Langue.lbl_autreInfo;
            this.lbl_codeZip.Content = Langue.lbl_codeZip;
        }
        public FrmModificationAdresse(CsDemande _LaDemande)
        {
            InitializeComponent();
            Translate();
            LaDemande = _LaDemande;
            ChargerLaListeDesCommunes();
            ChargeQuartier();
            ChargerTournee();
            ChargeSecteur();
            ChargeRue();
            this.lnkMotif.Visibility = System.Windows.Visibility.Collapsed;

            this.Txt_CodeCentre.MaxLength = SessionObject.Enumere.TailleCentre;
            this.Txt_CodeProduit.MaxLength = SessionObject.Enumere.TailleCodeProduit;
            this.Txt_Client.MaxLength = SessionObject.Enumere.TailleClient;

            this.Txt_CodeCentre.IsReadOnly = true;
            this.Txt_CodeProduit.IsReadOnly = true;
            this.Txt_Client.IsReadOnly = true;
            this.Txt_Ordre.IsReadOnly = true;
            this.Txt_NumDemande .IsReadOnly = true;
            this.btn_Centre.IsEnabled = false;
            this.btn_Produit .IsEnabled = false;
            this.btn_Rechercher .IsEnabled = false;

            this.Txt_CodeCentre.Text = _LaDemande.LaDemande.CENTRE;
            this.Txt_CodeProduit .Text = _LaDemande.LaDemande.PRODUIT ;
            this.Txt_NumDemande .Text = _LaDemande.LaDemande.NUMDEM ;

            TypeDemande = _LaDemande.LaDemande.TYPEDEMANDE;
            LaDemande.LaDemande.STATUTDEMANDE = null;
            if (!string.IsNullOrEmpty(_LaDemande.LaDemande.ANNOTATION))
                this.lnkMotif.Visibility = System.Windows.Visibility.Visible;
            this.btn_Supprime.Visibility = System.Windows.Visibility.Collapsed;
            this.btn_Modifier.Visibility = System.Windows.Visibility.Collapsed;
            if (LaDemande.LaDemande.FICHIERJOINT != new Guid("00000000-0000-0000-0000-000000000000"))
                RetourneObjectScan(LaDemande.LaDemande);

        }
        void RemplireLibelle()
        {

            if (this.Txt_CodeCentre.Text.Length == SessionObject.Enumere.TailleCentre)
            {
                CsCentre _LeCentre = ClasseMEthodeGenerique.RetourneObjectFromList<CsCentre>(SessionObject.LstCentre, this.Txt_CodeCentre.Text, "CODE");
                if (_LeCentre != null && !string.IsNullOrEmpty(_LeCentre.CODE))
                {
                    this.Txt_LibelleCentre.Text = _LeCentre.LIBELLE;
                    this.Txt_CodeSite.Text = _LeCentre.CODESITE;
                    this.Txt_LibelleSite.Text = _LeCentre.LIBELLESITE;
                }
            }

            if (this.Txt_CodeProduit.Text.Length == SessionObject.Enumere.TailleCodeProduit)
            {
                CsProduit _LeProduitSelect = ClasseMEthodeGenerique.RetourneObjectFromList(SessionObject.ListeDesProduit, this.Txt_CodeProduit.Text, "CODE");
                if (!string.IsNullOrEmpty(_LeProduitSelect.LIBELLE))
                    this.Txt_LibelleProduit.Text = _LeProduitSelect.LIBELLE;
            }
            if (!string.IsNullOrEmpty(this.Txt_CodeCommune.Text) &&
                this.Txt_CodeCommune.Text.Length == SessionObject.Enumere.TailleCodeQuartier &&
                LstCommuneAll.Count != 0)
            {
                LstQuartierCommuneSelect = new List<CsQuartier>();
                CsCommune _LaCommune = ClasseMEthodeGenerique.RetourneObjectFromList(LstCommuneAll, this.Txt_CodeCommune.Text, "CODE");
                if (!string.IsNullOrEmpty(_LaCommune.LIBELLE))
                {
                    this.Txt_LibelleCommune.Text = _LaCommune.LIBELLE;
                }
            }
            if (LstRuesAll.Count != 0 && !string.IsNullOrEmpty(this.Txt_CodeNomRue.Text) &&
                this.Txt_CodeNomRue.Text.Length == SessionObject.Enumere.TailleCodeQuartier)
            {
                CsRues _LaRue = LstRuesAll.FirstOrDefault(p => p.CODE == this.Txt_CodeNomRue.Text);
                if (_LaRue != null)
                {
                    this.Txt_NomRue.Text = _LaRue.LIBELLE;
                }
            }
            if (this.Txt_CodeProduit.Text.Length == SessionObject.Enumere.TailleCodeProduit)
            {
                LaDemande.LaDemande.PRODUIT = this.Txt_CodeProduit.Text;
                CsProduit _LeProduitSelect = ClasseMEthodeGenerique.RetourneObjectFromList(ListeDesProduitDuSite, this.Txt_CodeProduit.Text, "CODE");
                if (!string.IsNullOrEmpty(_LeProduitSelect.LIBELLE))
                {
                    this.Txt_LibelleProduit.Text = _LeProduitSelect.LIBELLE;

                }
            }
            if (LstQuartierAll.Count != 0 && !string.IsNullOrEmpty(this.Txt_CodeQuartier.Text) &&
    this.Txt_CodeQuartier.Text.Length == SessionObject.Enumere.TailleCodeQuartier)
            {
                CsQuartier _LeQuartier = LstQuartierCommuneSelect.FirstOrDefault(p => p.CODE == this.Txt_CodeQuartier.Text);
                if (_LeQuartier != null)
                {
                    this.Txt_LibelleQuartier.Text = _LeQuartier.LIBELLE;
                    LeQuartierSelect = _LeQuartier;
                    if (LstSecteurAll != null && LstSecteurAll.Count != 0)
                        LstSecteurQuartierSelect = LstSecteurAll.Where(p => p.CODE == _LeQuartier.CODE || p.CODE == "00000").ToList();
                }
            }
            if (LstSecteurAll.Count != 0 && !string.IsNullOrEmpty(this.Txt_CodeSecteur.Text) &&
             this.Txt_CodeSecteur.Text.Length == SessionObject.Enumere.TailleCodeQuartier)
            {
                LstRuesSecteurSelect = new List<CsRues>();
                CsSecteur _LeSecteur = LstSecteurAll.FirstOrDefault(p => p.CODE == this.Txt_CodeSecteur.Text);
                if (_LeSecteur != null)
                {
                    this.Txt_LibelleSecteur.Text = _LeSecteur.LIBELLE;
                    LeSecteurSelect = _LeSecteur;
                    LstRuesSecteurSelect = LstRuesAll.Where(p => p.FK_IDSECTEUR == _LeSecteur.PK_ID || p.CODE == "00000").ToList();

                }
            }
        }
        public FrmModificationAdresse(string _TypeDemande)
        {
            InitializeComponent();
            Translate();

            if (LaDemande.Ag  == null) LaDemande.Ag  = new CsAg ();
            if (LaDemande.LaDemande == null) LaDemande.LaDemande = new CsDemandeBase();
            if (LaDemande.LeCentre == null) LaDemande.LeCentre = new CsCentre();
            if (LaDemande.LeProduit == null) LaDemande.LeProduit = new CsProduit();
            TypeDemande = _TypeDemande;
            ChargerLaListeDesCommunes();
            ChargeQuartier();
            ChargerTournee();
            ChargeSecteur();
            ChargeRue();
            ChargerDonneeDuSite();
            ChargerListeDeProduit();
            this.lnkMotif.Visibility = System.Windows.Visibility.Collapsed;

            this.btn_Supprime.Visibility = System.Windows.Visibility.Collapsed;
            this.btn_Modifier.Visibility = System.Windows.Visibility.Collapsed;
            this.Txt_CodeCentre.MaxLength = SessionObject.Enumere.TailleCentre;
            this.Txt_CodeProduit.MaxLength = SessionObject.Enumere.TailleCodeProduit;
            this.Txt_Client.MaxLength = SessionObject.Enumere.TailleClient;
        }
        void Initctrl()
        {
            LstCommuneAll = SessionObject.LstCommune;
            LstQuartierAll = SessionObject.LstQuartier;
            LstSecteurAll = SessionObject.LstSecteur;
            LstRuesAll = SessionObject.LstRues;
            LstZone = SessionObject.LstZone;
            if (LaDemande.Ag != null) _LeAg = LaDemande.Ag;
            else _LeAg = new CsAg();
            AfficherInfoAdresse(_LeAg);
            RemplireLibelle();
        }
        private void AfficherInfoAdresse(CsAg AdresseDemande)
        {
            try
            {
                this.Txt_Client.Text = string.IsNullOrEmpty(AdresseDemande.CLIENT) ? string.Empty : AdresseDemande.CLIENT;
                //this.Txt_Ordre.Text = string.IsNullOrEmpty(AdresseDemande.ORDRE) ? string.Empty : AdresseDemande.ORDRE;

                this.Txt_NomClient.Text = string.IsNullOrEmpty(AdresseDemande.NOMP) ? string.Empty : AdresseDemande.NOMP;
                this.Txt_CodeCommune.Text = string.IsNullOrEmpty(AdresseDemande.COMMUNE) ? string.Empty : AdresseDemande.COMMUNE;
                if (LstCommuneAll != null && LstCommuneAll.Count != 0 && !string.IsNullOrEmpty(AdresseDemande.COMMUNE))
                {
                    CsCommune _LAcommune = ClasseMEthodeGenerique.RetourneObjectFromList(LstCommuneAll, AdresseDemande.COMMUNE, "CODE");
                    if (!string.IsNullOrEmpty(_LAcommune.LIBELLE))
                        this.Txt_LibelleCommune.Text = _LAcommune.LIBELLE;
                }
                this.Txt_CodeQuartier.Text = string.IsNullOrEmpty(AdresseDemande.QUARTIER) ? string.Empty : AdresseDemande.QUARTIER;
                if (LstQuartierAll != null && LstQuartierAll.Count != 0 && !string.IsNullOrEmpty(AdresseDemande.QUARTIER))
                {
                    CsQuartier _LeQuartier = ClasseMEthodeGenerique.RetourneObjectFromList(LstQuartierAll, AdresseDemande.QUARTIER, "CODE");
                    if (!string.IsNullOrEmpty(_LeQuartier.LIBELLE))
                        this.Txt_LibelleCommune.Text = _LeQuartier.LIBELLE;
                }
                this.Txt_CodeSecteur.Text = string.IsNullOrEmpty(AdresseDemande.SECTEUR) ? string.Empty : AdresseDemande.SECTEUR;
                if (LstSecteurAll != null && LstSecteurAll.Count != 0 && !string.IsNullOrEmpty(AdresseDemande.SECTEUR))
                {
                    CsSecteur _LeSecteur = ClasseMEthodeGenerique.RetourneObjectFromList(LstSecteurAll, AdresseDemande.SECTEUR, "CODE");
                    if (!string.IsNullOrEmpty(_LeSecteur.LIBELLE))
                        this.Txt_LibelleCommune.Text = _LeSecteur.LIBELLE;
                }
                this.Txt_CodeNomRue.Text = string.IsNullOrEmpty(AdresseDemande.RUE) ? string.Empty : AdresseDemande.RUE;
                if (LstRuesAll != null && LstRuesAll.Count != 0 && !string.IsNullOrEmpty(AdresseDemande.RUE))
                {
                    CsRues _LaRue = ClasseMEthodeGenerique.RetourneObjectFromList(LstRuesAll, AdresseDemande.RUE, "CODE");
                    if (!string.IsNullOrEmpty(_LaRue.LIBELLE))
                        this.Txt_NomRue.Text = _LaRue.LIBELLE;
                }

                this.Txt_Etage.Text = string.IsNullOrEmpty(AdresseDemande.ETAGE) ? string.Empty : AdresseDemande.ETAGE;
                this.Txt_Email.Text = string.IsNullOrEmpty(AdresseDemande.EMAIL) ? string.Empty : AdresseDemande.EMAIL;
                this.Txt_Telephone.Text = string.IsNullOrEmpty(AdresseDemande.TELEPHONE) ? string.Empty : AdresseDemande.TELEPHONE;
                this.Txt_Fax.Text = string.IsNullOrEmpty(AdresseDemande.FAX) ? string.Empty : AdresseDemande.FAX;
                this.Txt_OrdreTour.Text = string.IsNullOrEmpty(AdresseDemande.ORDTOUR) ? string.Empty : AdresseDemande.ORDTOUR;
                this.Txt_Tournee.Text = string.IsNullOrEmpty(AdresseDemande.TOURNEE) ? string.Empty : AdresseDemande.TOURNEE;
            }
            catch (Exception ex)
            {

                throw ex;
            }


        }
        private void ChargerLaListeDesCommunes()
        {
            try
            {
                if (SessionObject.LstCommune.Count != 0)
                    return;
                AcceuilServiceClient service = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                service.ChargerCommuneCompleted += (s, args) =>
                {
                    if (args.Error != null && args.Cancelled)
                        return;
                    SessionObject.LstCommune = args.Result;
                    LstCommuneAll = SessionObject.LstCommune;
                };
                service.ChargerCommuneAsync();
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        private void ChargeQuartier()
        {
            try
            {
                if (SessionObject.LstQuartier.Count != 0)
                {
                    LstQuartierAll = SessionObject.LstQuartier;
                    return;
                }
                AcceuilServiceClient service = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                service.ChargerLesQartiersCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    SessionObject.LstQuartier = args.Result;
                    LstQuartierAll = SessionObject.LstQuartier;

                };
                service.ChargerLesQartiersAsync();
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        private void ChargeSecteur()
        {
            try
            {
                if (SessionObject.LstSecteur.Count != 0)
                    return;
                AcceuilServiceClient service = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                service.ChargerLesSecteursCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    SessionObject.LstSecteur = args.Result;
                    LstSecteurAll = SessionObject.LstSecteur;
                };
                service.ChargerLesSecteursAsync();
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void ChargeRue()
        {
            try
            {
                if (SessionObject.LstRues.Count != 0)
                    return;
                AcceuilServiceClient service = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                service.ChargerLesRueDesSecteurCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    SessionObject.LstRues = args.Result;
                    LstRuesAll = SessionObject.LstRues;
                };
                service.ChargerLesRueDesSecteurAsync();
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void ChargerTournee()
        {
            if (SessionObject.LstZone.Count != 0)
                return;
            AcceuilServiceClient service = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
            service.ChargerLesTourneesCompleted  += (s, args) =>
            {
                if (args != null && args.Cancelled)
                    return;
                SessionObject.LstZone = args.Result;
                LstZone = SessionObject.LstZone;
            };
            service.ChargerLesTourneesAsync ();
            service.CloseAsync();
        }

        #region Commune
        private void Txt_CodeCommune_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(this.Txt_CodeCommune.Text))
            {
                this.Txt_CodeCommune.Text = this.Txt_CodeCommune.Text.PadLeft(SessionObject.Enumere.TailleCodeQuartier, '0');
            }
        }
        private void Txt_CodeCommune_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(this.Txt_CodeCommune.Text) &&
                this.Txt_CodeCommune.Text.Length == SessionObject.Enumere.TailleCodeQuartier &&
                LstCommuneAll.Count != 0)
            {
                LaCommuneSelect = ClasseMEthodeGenerique.RetourneObjectFromList(LstCommuneAll, this.Txt_CodeCommune.Text, "CODE");
                if (!string.IsNullOrEmpty(LaCommuneSelect.LIBELLE))
                {
                    this.Txt_LibelleCommune.Text = LaCommuneSelect.LIBELLE;
                    this.Txt_CodeCommune.Tag = LaCommuneSelect.PK_ID;
                    LstQuartierCommuneSelect = SessionObject.LstQuartier.Where(p => p.FK_IDCOMMUNE == LaCommuneSelect.PK_ID || p.COMMUNE == "00000").ToList();
                }
                else
                {
                    var w = new MessageBoxControl.MessageBoxChildWindow(Langue.lbl_Menu, Langue.Msg_CommuneNonTrouve, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    w.OnMessageBoxClosed += (_, result) =>
                    {
                        this.Txt_LibelleCommune.Text = string.Empty;
                        this.Txt_CodeCommune.Text = string.Empty;
                        this.Txt_CodeCommune.Focus();
                    };
                    w.Show();
                }
            }
        }
        private void btn_Commune_Click(object sender, RoutedEventArgs e)
        {
            if (LaDemande != null && LaDemande.LeCentre != null && LaDemande.LeCentre.CODE != null && !string.IsNullOrEmpty(LaDemande.LeCentre.CODE))
                LstCommuneAll = SessionObject.LstCommune.Where(p => p.CENTRE == LaDemande.LeCentre.CODE).ToList();
            if (LstCommuneAll != null && LstCommuneAll.Count != 0)
            {
                this.btn_Commune.IsEnabled = false;
                List<object> _LstCommune = ClasseMEthodeGenerique.RetourneListeObjet(LstCommuneAll);
                UcListeGenerique ctr = new UcListeGenerique(_LstCommune, "CODE", "LIBELLE", Langue.lbl_ListeCommune);
                ctr.Closed += new EventHandler(galatee_OkClickedBtnCommune);
                ctr.Show();
            }
        }
        void galatee_OkClickedBtnCommune(object sender, EventArgs e)
        {
            UcListeGenerique ctrs = sender as UcListeGenerique;
            CsCommune _CommuneSelect = (CsCommune)ctrs.MyObject;
            if (_CommuneSelect != null)
                this.Txt_CodeCommune.Text = _CommuneSelect.CODE;
        }
        #endregion
        #region Quartier
        private void Txt_CodeQuartier_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(this.Txt_CodeQuartier.Text))
                this.Txt_CodeQuartier.Text = this.Txt_CodeQuartier.Text.PadLeft(SessionObject.Enumere.TailleCodeQuartier, '0');
        }
        private void Txt_CodeQuartier_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (LstQuartierAll.Count != 0 && !string.IsNullOrEmpty(this.Txt_CodeQuartier.Text) &&
                this.Txt_CodeQuartier.Text.Length == SessionObject.Enumere.TailleCodeQuartier)
            {
                CsQuartier _LeQuartier = LstQuartierCommuneSelect.FirstOrDefault(p => p.CODE == this.Txt_CodeQuartier.Text);
                if (_LeQuartier != null)
                {
                    this.Txt_LibelleQuartier.Text = _LeQuartier.LIBELLE;
                    LeQuartierSelect = _LeQuartier;
                    if (LstSecteurAll != null && LstSecteurAll.Count != 0)
                        LstSecteurQuartierSelect = LstSecteurAll.Where(p => p.CODE == _LeQuartier.CODE || p.CODE == "00000").ToList();
                }
                else
                {
                    var w = new MessageBoxControl.MessageBoxChildWindow(Langue.lbl_Menu, Langue.Msg_QuartierNonTrouve, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    w.OnMessageBoxClosed += (_, result) =>
                    {
                        this.Txt_CodeQuartier.Focus();

                    };
                    w.Show();
                }
            }
        }
        private void btn_Quartier_Click(object sender, RoutedEventArgs e)
        {

            if (LstQuartierCommuneSelect != null && LstQuartierCommuneSelect.Count != 0)
            {
                this.btn_Quartier.IsEnabled = false;
                List<object> _LstObjQuartier = ClasseMEthodeGenerique.RetourneListeObjet(LstQuartierCommuneSelect.OrderByDescending(k => k.COMMUNE).ToList());
                UcListeGenerique ctr = new UcListeGenerique(_LstObjQuartier, "CODE", "LIBELLE", Langue.lbl_ListeQuartiers);
                ctr.Closed += new EventHandler(galatee_OkClickedBtnQuartier);
                ctr.Show();
            }
        }
        void galatee_OkClickedBtnQuartier(object sender, EventArgs e)
        {
            this.btn_Quartier.IsEnabled = true;
            UcListeGenerique ctrs = sender as UcListeGenerique;
            if (ctrs.isOkClick)
            {
                CsQuartier _LeQuartier = (CsQuartier)ctrs.MyObject;
                if (_LeQuartier != null)
                    this.Txt_CodeQuartier.Text = _LeQuartier.CODE;
                this.Txt_CodeQuartier.Tag = _LeQuartier.PK_ID;
                LstSecteurQuartierSelect = SessionObject.LstSecteur.Where(p => p.FK_IDQUARTIER == _LeQuartier.PK_ID || p.CODE == "00000").ToList();
            }
        }
        #endregion
        #region Tournee
        private void Txt_Tournee_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(this.Txt_Tournee.Text))
            {
                CsTournee _tourne = LstZone.FirstOrDefault(p => p.CENTRE == LaDemande.LaDemande.CENTRE && p.CODE == this.Txt_Tournee.Text);
                if (_tourne == null)
                {

                    var w = new MessageBoxControl.MessageBoxChildWindow(Langue.lbl_Menu, Langue.MsgTourneNonTrouvee, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    w.OnMessageBoxClosed += (_, result) =>
                    {
                        this.Txt_Tournee.Focus();
                    };
                    w.Show();
                }
                else LaTourneSelect = _tourne;
            }

        }
        private void btn_zone_Click(object sender, RoutedEventArgs e)
        {
            if (LstZone != null && LstZone.Count != 0)
            {
                if (!string.IsNullOrEmpty(LaDemande.LaDemande.CENTRE))
                {
                    List<object> _LstObj = ClasseMEthodeGenerique.RetourneListeObjet(LstZone.Where(p => p.CENTRE == LaDemande.LaDemande.CENTRE && p.FK_IDCENTRE == LaDemande.LaDemande.FK_IDCENTRE).ToList());
                    UcListeGenerique ctr = new UcListeGenerique(_LstObj, "CODE", "LIBELLE", Langue.lbl_ListeQuartiers);
                    ctr.Closed += new EventHandler(galatee_OkClickedBtnTournee);
                    ctr.Show();
                }
            }
        }
        void galatee_OkClickedBtnTournee(object sender, EventArgs e)
        {
            UcListeGenerique ctrs = sender as UcListeGenerique;
            if (ctrs.isOkClick != null)
            {
                CsTournee _LaTournee = (CsTournee)ctrs.MyObject;
                this.Txt_Tournee.Text = _LaTournee.CODE;
                LaTourneSelect = _LaTournee;
            }
        }
        #endregion
        #region Secteur
        private void Txt_CodeSecteur_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(this.Txt_CodeSecteur.Text))
                this.Txt_CodeSecteur.Text = this.Txt_CodeSecteur.Text.PadLeft(SessionObject.Enumere.TailleCodeQuartier, '0');
        }
        private void Txt_CodeSecteur_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (LstSecteurAll.Count != 0 && !string.IsNullOrEmpty(this.Txt_CodeSecteur.Text) &&
                this.Txt_CodeSecteur.Text.Length == SessionObject.Enumere.TailleCodeQuartier)
            {
                LstRuesSecteurSelect = new List<CsRues>();
                CsSecteur _LeSecteur = LstSecteurAll.FirstOrDefault(p => p.CODE == this.Txt_CodeSecteur.Text);
                if (_LeSecteur != null)
                {
                    this.Txt_LibelleSecteur.Text = _LeSecteur.LIBELLE;
                    LeSecteurSelect = _LeSecteur;
                    LstRuesSecteurSelect = LstRuesAll.Where(p => p.FK_IDSECTEUR == _LeSecteur.PK_ID || p.CODE == "00000").ToList();

                }
                else
                {
                    var w = new MessageBoxControl.MessageBoxChildWindow(Langue.lbl_Menu, Langue.Msg_QuartierNonTrouve, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    w.OnMessageBoxClosed += (_, result) =>
                    {
                        this.Txt_CodeSecteur.Focus();
                    };
                    w.Show();
                }
            }
        }
        private void btn_Secteur_Click(object sender, RoutedEventArgs e)
        {
            //if (LstSecteurQuartierSelect != null && LstSecteurQuartierSelect.Count != 0)
            //{
            //    List<object> _LstObjSecteur = ClasseMEthodeGenerique.RetourneListeObjet(LstSecteurQuartierSelect);
            //    UcListeGenerique ctr = new UcListeGenerique(_LstObjSecteur, "CODE", "LIBELLE", Langue.lbl_ListeDesSecteurs);
            //    ctr.Closed += new EventHandler(galatee_OkClickedBtnSecteur);
            //    ctr.Show();
            //}

            if (LstSecteurAll != null && LstSecteurAll.Count != 0)
            {
                List<object> _LstObjSecteur = ClasseMEthodeGenerique.RetourneListeObjet(LstSecteurAll);
                UcListeGenerique ctr = new UcListeGenerique(_LstObjSecteur, "CODE", "LIBELLE", Langue.lbl_ListeDesSecteurs);
                ctr.Closed += new EventHandler(galatee_OkClickedBtnSecteur);
                ctr.Show();
            }
        }
        void galatee_OkClickedBtnSecteur(object sender, EventArgs e)
        {
            UcListeGenerique ctrs = sender as UcListeGenerique;
            CsSecteur _LeSecteur = (CsSecteur)ctrs.MyObject;
            if (_LeSecteur != null)
                this.Txt_CodeSecteur.Text = _LeSecteur.CODE;
        }
        #endregion
        #region Rue
        private void Txt_CodeNomRue_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(this.Txt_CodeNomRue.Text))
                    this.Txt_CodeNomRue.Text = this.Txt_CodeNomRue.Text.PadLeft(SessionObject.Enumere.TailleCodeQuartier, '0');
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Langue.lbl_Menu);
            }
        }
        private void Txt_CodeNomRue_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (LstRuesAll.Count != 0 && !string.IsNullOrEmpty(this.Txt_CodeNomRue.Text) &&
                this.Txt_CodeNomRue.Text.Length == SessionObject.Enumere.TailleCodeQuartier)
            {
                //CsRues _LaRue = ClasseMEthodeGenerique.RetourneObjectFromList(LstRuesAll, "CODE", this.Txt_CodeNomRue.Text);
                CsRues _LaRue = LstRuesAll.FirstOrDefault(p => p.CODE == this.Txt_CodeNomRue.Text);
                if (_LaRue != null)
                {
                    this.Txt_NomRue.Text = _LaRue.LIBELLE;
                    LaRueSelect = _LaRue;
                }
                else
                {
                    var w = new MessageBoxControl.MessageBoxChildWindow(Langue.lbl_Menu, Langue.Msg_QuartierNonTrouve, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    w.OnMessageBoxClosed += (_, result) =>
                    {
                        this.Txt_CodeNomRue.Focus();
                    };
                    w.Show();
                }
            }
        }
        private void btn_Rue_Click(object sender, RoutedEventArgs e)
        {
            if (LstRuesSecteurSelect != null && LstRuesSecteurSelect.Count != 0)
            {
                List<object> _LstObjSecteur = ClasseMEthodeGenerique.RetourneListeObjet(LstRuesSecteurSelect);
                UcListeGenerique ctr = new UcListeGenerique(_LstObjSecteur, "CODE", "LIBELLE", Langue.lbl_ListeDesSecteurs);
                ctr.Closed += new EventHandler(galatee_OkClickedBtnRue);
                ctr.Show();
            }
        }
        void galatee_OkClickedBtnRue(object sender, EventArgs e)
        {
            UcListeGenerique ctrs = sender as UcListeGenerique;
            CsRues _LaRue = (CsRues)ctrs.MyObject;
            if (_LaRue != null)
                this.Txt_CodeNomRue.Text = _LaRue.CODE;
        }
        #endregion
        private void ActionControle(bool Etat)
        {
            this.btn_Commune.IsEnabled = Etat;
            this.btn_Quartier.IsEnabled = Etat;
            this.btn_Secteur.IsEnabled = Etat;
            this.btn_Rue.IsEnabled = Etat;
            this.btn_zone.IsEnabled = Etat;
            this.btn_regroupementcpt.IsEnabled = Etat;

            this.Txt_NomClient.IsEnabled = Etat;
            this.Txt_Porte.IsEnabled = Etat;
            this.Txt_CodeCommune.IsEnabled = Etat;
            this.Txt_LibelleCommune.IsEnabled = Etat;
            this.Txt_CodeQuartier.IsEnabled = Etat;
            this.Txt_LibelleQuartier.IsEnabled = Etat;
            this.Txt_CodeSecteur.IsEnabled = Etat;
            this.Txt_LibelleSecteur.IsEnabled = Etat;
            this.Txt_CodeNomRue.IsEnabled = Etat;
            this.Txt_AutreInfo.IsEnabled = Etat;
            this.Txt_Etage.IsEnabled = Etat;
            this.Txt_Fax.IsEnabled = Etat;
            this.Txt_AutreInformation.IsEnabled = Etat;
            this.Txt_Partiel.IsEnabled = Etat;

            this.Txt_NomRue.IsEnabled = Etat;
            this.Txt_NumRue.IsEnabled = Etat;
            this.Txt_CodePostale.IsEnabled = Etat;
            this.Txt_Telephone.IsEnabled = Etat;
            this.Txt_OrdreTour.IsEnabled = Etat;
            this.Txt_Email.IsEnabled = Etat;
            this.Txt_Tournee.IsEnabled = Etat;
            this.Txt_CodeGroupementCompteut.IsEnabled = Etat;
            this.Txt_LibelleGroupementCompteur.IsEnabled = Etat;

        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Initctrl();
        }


        void DialogClosed(object sender, EventArgs e)
        {
            DialogResult ctrs = sender as DialogResult;
            if (ctrs.Yes) // permet de tester si l'utilisateur a click sur Ok ou NON 
            {
                ctrs.DialogResult = false;
                //ActionControle(false);
            }

        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Initctrl();
        }
        public event EventHandler Closed;
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            EnregisterDemande(LaDemande);
            ValidationDemande(LaDemande);

        }
        ObjDOCUMENTSCANNE leDoc = new ObjDOCUMENTSCANNE();

        public void EnregisterDemande(CsDemande _LaDemande)
        {
            try
            {
                if(string.IsNullOrEmpty(Txt_CodeCommune.Text) )
                    Message.ShowError("Remplir les champs obligatoires", Langue.lbl_Menu);

                _LaDemande.Ag = AdresseRechercher;
                _LaDemande.Ag.CENTRE = string.IsNullOrEmpty(this.Txt_CodeCentre.Text) ? string.Empty : this.Txt_CodeCentre.Text;
                _LaDemande.Ag.CLIENT = string.IsNullOrEmpty(this.Txt_Client.Text) ? string.Empty : this.Txt_Client.Text;
                //_LaDemande.Ag.ORDRE = string.IsNullOrEmpty(this.Txt_Ordre.Text) ? string.Empty : this.Txt_Ordre.Text;
                _LaDemande.Ag.NUMDEM = string.IsNullOrEmpty(this.Txt_NumDemande.Text) ? string.Empty : this.Txt_NumDemande.Text;
                _LaDemande.Ag.NOMP = string.IsNullOrEmpty(this.Txt_NomClient.Text) ? null : this.Txt_NomClient.Text;
                _LaDemande.Ag.COMMUNE = string.IsNullOrEmpty(this.Txt_CodeCommune.Text) ? null : this.Txt_CodeCommune.Text;
                _LaDemande.Ag.QUARTIER = string.IsNullOrEmpty(this.Txt_CodeQuartier.Text) ? null : this.Txt_CodeQuartier.Text;
                _LaDemande.Ag.SECTEUR = string.IsNullOrEmpty(this.Txt_CodeSecteur.Text) ? null : this.Txt_CodeSecteur.Text;
                _LaDemande.Ag.RUE = string.IsNullOrEmpty(this.Txt_CodeNomRue.Text) ? null : this.Txt_CodeNomRue.Text;
                _LaDemande.Ag.TELEPHONE = string.IsNullOrEmpty(this.Txt_Telephone.Text) ? null : this.Txt_Telephone.Text;
                _LaDemande.Ag.EMAIL = string.IsNullOrEmpty(this.Txt_Email.Text) ? null : this.Txt_Email.Text;
                _LaDemande.Ag.TOURNEE = string.IsNullOrEmpty(this.Txt_Tournee.Text) ? null : this.Txt_Tournee.Text;
                _LaDemande.Ag.ORDTOUR = string.IsNullOrEmpty(this.Txt_OrdreTour.Text) ? null : this.Txt_OrdreTour.Text;
                _LaDemande.Ag.FAX = string.IsNullOrEmpty(this.Txt_Fax.Text) ? null : this.Txt_Fax.Text;
                _LaDemande.Ag.PORTE = string.IsNullOrEmpty(this.Txt_Porte.Text) ? null : this.Txt_Porte.Text;
                _LaDemande.Ag .ETAGE = string.IsNullOrEmpty(this.Txt_Etage.Text) ? null : this.Txt_Etage.Text;

                _LaDemande.Ag.USERCREATION = UserConnecte.matricule;
                _LaDemande.Ag.USERMODIFICATION = UserConnecte.matricule;
                _LaDemande.Ag.DATECREATION =  System.DateTime.Now;
                _LaDemande.Ag.DATEMODIFICATION =  System.DateTime.Now;

                _LaDemande.LaDemande.NUMDEM = Txt_NumDemande.Text;
                _LaDemande.LaDemande.MATRICULE = UserConnecte.matricule;
                _LaDemande.LaDemande.CENTRE = string.IsNullOrEmpty(this.Txt_CodeCentre.Text) ? string.Empty : this.Txt_CodeCentre.Text;
                _LaDemande.LaDemande.CLIENT = string.IsNullOrEmpty(this.Txt_Client.Text) ? string.Empty : this.Txt_Client.Text;
                _LaDemande.LaDemande.ORDRE = string.IsNullOrEmpty(this.Txt_Ordre.Text) ? string.Empty : this.Txt_Ordre.Text;
                _LaDemande.LaDemande.STATUT = SessionObject.Enumere.DemandeStatusEnAttente;
                _LaDemande.LaDemande.TYPEDEMANDE = TypeDemande;
                _LaDemande.LaDemande.USERCREATION = UserConnecte.matricule;
                _LaDemande.LaDemande.USERMODIFICATION = UserConnecte.matricule;
                _LaDemande.LaDemande.DATECREATION =  System.DateTime.Now;
                _LaDemande.LaDemande.DATEMODIFICATION =  System.DateTime.Now;

                if (lnkLetter.Tag != null)
                {
                    leDoc = SaveFile((byte[])lnkLetter.Tag, 1, null);
                    //_Lademande.LeDocumentScanne = leDoc;
                }

        

    

            }
            catch (Exception)
            {
                throw;
            }
        }

        private void Txt_NumDemande_TextChanged_1(object sender, TextChangedEventArgs e)
        {

        }
        List<CsSite> lstSite = new List<CsSite>();

        private void ChargerDonneeDuSite()
        {
            try
            {
                AcceuilServiceClient service = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                service.ListeDesDonneesDesSiteCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    SessionObject.LstCentre = args.Result;

                    LstCentre = Shared.ClasseMEthodeGenerique.RetourCentreByPerimetre(SessionObject.LstCentre.Where(p => p.CODE != SessionObject.Enumere.Generale).ToList(), UserConnecte.listeProfilUser);
                    lstSite = ClasseMEthodeGenerique.RetourneSiteByCentre(LstCentre);
                    if (lstSite != null)
                    {
                        List<CsSite> _LstSite = lstSite.Where(p => p.CODESITE != SessionObject.Enumere.Generale).ToList();
                        if (_LstSite.Count == 1)
                        {
                            this.Txt_CodeSite.Text = _LstSite[0].CODESITE;
                            this.Txt_LibelleSite.Text = _LstSite[0].LIBELLE;
                            //this.btn_Site.IsEnabled = false;
                            this.Txt_CodeSite.IsReadOnly = true;
                        }
                    }
                    if (LstCentre != null)
                    {
                        List<CsCentre> _LstCentre = LstCentre.Where(p => p.CODE != SessionObject.Enumere.Generale).ToList();
                        if (_LstCentre.Count == 1)
                        {
                            this.Txt_CodeCentre.Text = _LstCentre[0].CODE;
                            this.Txt_LibelleCentre.Text = _LstCentre[0].LIBELLE;
                        }
                        else
                        {
                            CsCentre _LeCentre = new CsCentre();
                            if (!string.IsNullOrEmpty(this.Txt_CodeCentre.Text))
                                _LeCentre = LstCentre.FirstOrDefault(p => p.CODE == this.Txt_CodeCentre.Text);
                            if (_LeCentre != null && !string.IsNullOrEmpty(_LeCentre.CODE))
                            {
                                this.Txt_LibelleCentre.Text = _LeCentre.LIBELLE;
                                //this.btn_Centre.IsEnabled = false;
                                this.Txt_CodeCentre.IsReadOnly = true;
                            }
                        }
                    }
                };
                service.ListeDesDonneesDesSiteAsync(true );
                service.CloseAsync();
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
                        else
                        {
                            //CsProduit _LeProduit = ListeDesProduitDuSite.FirstOrDefault(p => p.CODE == LaDemande.LaDemande.PRODUIT);
                            //if (_LeProduit != null)
                            //{
                            //    this.Txt_CodeProduit.Text = LaDemande.LaDemande.PRODUIT;
                            //    this.Txt_LibelleProduit.Text = _LeProduit.LIBELLE;
                            //    this.btn_Produit.IsEnabled = false;
                            //    this.Txt_CodeProduit.IsReadOnly = true;
                            //}
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
                service1.ValiderDemandeWithObjectCompleted += (sr, res) =>
                {

                    string Messages = string.Empty;
                    //Si la date de d'encaissement n'est pas renseigné c-a-d que la demende est en attente
                    if (_LaDemande.LaDemande.STATUT == SessionObject.Enumere.DemandeStatusEnAttente)
                        //Msg de confirmation de l'enregistremet
                        Messages = Langue.MsgRequestSaved;
                    //Si la date d'encaissement est renseigné 
                    else
                        Messages = Langue.MsgOperationTerminee;

                    Message.ShowInformation(Messages, Langue.lbl_Menu);
                    if (Closed != null)
                        Closed(this, new EventArgs());
                    this.DialogResult = false;
                };
                service1.ValiderDemandeWithObjectAsync(_LaDemande, leDoc);
                service1.CloseAsync();
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
            //else
            //    this.btn_Centre.IsEnabled = true;


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
                    
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        private void Txt_CodeSite_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (this.Txt_CodeSite.Text.Length == SessionObject.Enumere.TailleCentre)
            {
                CsSite _LeSite = ClasseMEthodeGenerique.RetourneObjectFromList<CsSite>(lstSite, this.Txt_CodeSite.Text, "CODESITE");
                if (_LeSite != null && !string.IsNullOrEmpty(_LeSite.CODESITE))
                    this.Txt_LibelleSite.Text = _LeSite.LIBELLE;
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


        private void Txt_CodeCentre_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (this.Txt_CodeCentre.Text.Length == SessionObject.Enumere.TailleCentre)
            {
                CsCentre _LeCentre = ClasseMEthodeGenerique.RetourneObjectFromList<CsCentre>(LstCentre, this.Txt_CodeCentre.Text, "CODE");
                if (_LeCentre != null && !string.IsNullOrEmpty(_LeCentre.CODE))
                {
                    this.Txt_LibelleCentre.Text = _LeCentre.LIBELLE;
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



        private void RetourneInfoAddresse(int fk_idcentre, string centre, string client, string ordre)
        {
            int res = LoadingManager.BeginLoading(Langue.En_Cours);
            try
            {
                AcceuilServiceClient service = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                service.RetourneAdresseCompleted += (s, args) =>
                {
                    if (args.Error != null && args.Cancelled)
                        return;
                    AdresseRechercher = args.Result;
                    if (AdresseRechercher != null && !string.IsNullOrEmpty(AdresseRechercher.CENTRE))
                    {
                        LaDemande.Ag = AdresseRechercher;
                        AfficherInfoAdresse(AdresseRechercher);
                    }
                    else
                        Message.ShowInformation("Aucune information trouvée", "Recherche d'adresse");

                    LoadingManager.EndLoading(res);

                };
                service.RetourneAdresseAsync(fk_idcentre,centre, client, ordre);
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                LoadingManager.EndLoading(res);
                throw ex;
            }

        }

        private void btn_Rechercher_Click_1(object sender, RoutedEventArgs e)
        {
            if (!SessionObject.Enumere.IsModificationAutoriserEnFacturation)
                VerifieExisteDemande(this.Txt_CodeCentre.Text, this.Txt_Client.Text, this.Txt_Ordre.Text, LaDemande.LaDemande.FK_IDCENTRE, TypeDemande);
            else
                VerifieDernierEvt(this.Txt_CodeCentre.Text, this.Txt_Client.Text, this.Txt_Ordre.Text);
        }
        private void VerifieDernierEvt(string centre, string client, string Ordre)
        {

            if (!string.IsNullOrEmpty(Txt_Client.Text) && Txt_Client.Text.Length == SessionObject.Enumere.TailleClient)
            {

                LaDemande.LaDemande.CLIENT = Txt_Client.Text;
                string OrdreMax = string.Empty;
                AcceuilServiceClient service = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
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
                        VerifieExisteDemande(this.Txt_CodeCentre.Text, this.Txt_Client.Text, this.Txt_Ordre.Text, LaDemande.LaDemande.FK_IDCENTRE, TypeDemande);
                    }
                };
                service.IsDernierEvtEnFacturationAsync(centre, client, Ordre);
                service.CloseAsync();
            }

        }
        private void VerifieExisteDemande( string centre, string client, string Ordre, int idCentre, string tdem)
        {

            try
            {
                if (!string.IsNullOrEmpty(Txt_Client.Text) && Txt_Client.Text.Length == SessionObject.Enumere.TailleClient)
                {

                    LaDemande.LaDemande.CLIENT = Txt_Client.Text;
                    string OrdreMax = string.Empty;
                    AcceuilServiceClient service = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                    service.RetourneDemandeClientTypeCompleted += (s, args) =>
                    {
                        if (args != null && args.Cancelled)
                            return;
                        if (args.Result != null)
                        {
                            if (args.Result.STATUT != SessionObject.Enumere.DemandeStatusPriseEnCompte && args.Result.ISSUPPRIME != true)
                            {
                                Message.ShowError("Il existe déja une demande de ce type sur ce client", "Accueil");
                                return;
                            }
                        }
                        RetourneInfoAddresse(idCentre, this.Txt_CodeCentre.Text, this.Txt_Client.Text, this.Txt_Ordre.Text);
                    };
                    service.RetourneDemandeClientTypeAsync(centre, client, Ordre, idCentre, tdem);
                    service.CloseAsync();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        private void lnkMotif_Click(object sender, RoutedEventArgs e)
        {
            Message.ShowInformation(LaDemande.LaDemande.ANNOTATION, "Motif réjet");

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


     
    }
}

