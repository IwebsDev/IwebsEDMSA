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
using Galatee.Silverlight.Resources.Devis;
//using Galatee.Silverlight.serviceWeb;
using Galatee.Silverlight.MainView;
using Galatee.Silverlight.Shared;
using System.IO;
using System.Collections.ObjectModel;

namespace Galatee.Silverlight.Accueil
{
    public partial class FrmModificationAdresse : ChildWindow
    {
        public FrmModificationAdresse()
        {
            InitializeComponent();
        }

        private UcImageScanne formScanne = null;

        List<CsCommune> LstCommuneAll;
        List<CsQuartier> LstQuartierAll;

        List<CsSecteur> LstSecteurAll;
        private List<CsCentre> _listeDesCentreExistant = null;
        List<CsRues> LstRuesAll;

        CsAg _LeAg = new CsAg();

        bool IsUpdate = false;
        List<CsTypeBranchement> LstTypeBranchement = new List<CsTypeBranchement>();

        List<CsCommune> LstCommune = new List<CsCommune>();
        List<CsQuartier> LstQuartier = new List<CsQuartier>();
        List<CsTournee> LstZone;

        public ObservableCollection<ObjDOCUMENTSCANNE> LstPiece = new ObservableCollection<ObjDOCUMENTSCANNE>();
        public ObservableCollection<CsTypeDOCUMENTSCANNE> LstTypeDocument = new ObservableCollection<CsTypeDOCUMENTSCANNE>();
        private byte[] image;

        string Tdem = string.Empty;
        CsDemande laDetailDemande = null;
        bool IsRejeterDemande = false;

        void Translate()
        {
            //Gestion de la langue
            this.lbl_Commune.Content = Langue.lbl_Commune;
            this.lbl_Etage.Content = Langue.lbl_Etage;
            this.lbl_NomProprietaire.Content = Langue.lbl_NomProprietaire;
            this.lbl_Quartier.Content = Langue.lbl_Quartier;
            this.lbl_Rue.Content = Langue.lbl_Rue;
            this.lbl_Secteur.Content = Langue.lbl_Secteur;
            this.lbl_Sequence.Content = Langue.lbl_Ordre;
            this.lbl_Tournee.Content = Langue.lbl_Tournee;
        }


        public FrmModificationAdresse(string _TypeDemande,string IsInit)
        {
            InitializeComponent();
            Translate();

            Tdem = _TypeDemande;
            ChargerLaListeDesCommunes();
            ChargeQuartier();
            ChargerTournee();
            ChargeSecteur();
            ChargeRue();
            ChargerTypeDemande();
            ChargerDonneeDuSite();
            ChargerTypeDocument();
            this.Txt_ReferenceClient.MaxLength = SessionObject.Enumere.TailleClient;
        }
        public FrmModificationAdresse(int iddemande)
        {
            InitializeComponent();
            Translate();
            ChargerLaListeDesCommunes();
            ChargeQuartier();
            ChargerTournee();
            ChargeSecteur();
            ChargeRue();
            ChargerTypeDemande();
            ChargerDonneeDuSite();
            ChargerTypeDocument();
            ChargeDetailDEvis(iddemande);
            this.Txt_ReferenceClient.MaxLength = SessionObject.Enumere.TailleClient;
        }
        void Initctrl()
        {
            LstCommuneAll = SessionObject.LstCommune;
            LstQuartierAll = SessionObject.LstQuartier;
            LstSecteurAll = SessionObject.LstSecteur;
            LstRuesAll = SessionObject.LstRues;
            LstZone = SessionObject.LstZone;
        }
        private void AfficherInfoAdresse(CsAg AdresseDemande)
        {
            try
            {
                this.Txt_ReferenceClient.Text = string.IsNullOrEmpty(AdresseDemande.CLIENT) ? string.Empty : AdresseDemande.CLIENT;
                this.Txt_NomClient.Text = string.IsNullOrEmpty(AdresseDemande.NOMP) ? string.Empty : AdresseDemande.NOMP;

                this.Txt_CodeCommune.Text = string.IsNullOrEmpty(AdresseDemande.COMMUNE) ? string.Empty : AdresseDemande.COMMUNE;
                this.Txt_CodeCommune.Tag = AdresseDemande.FK_IDCOMMUNE == null ? null : (int?)AdresseDemande.FK_IDCOMMUNE;
                this.Txt_LibelleCommune.Text = string.IsNullOrEmpty(AdresseDemande.LIBELLECOMMUNE) ? string.Empty : AdresseDemande.LIBELLECOMMUNE;

                this.Txt_CodeQuartier.Text = string.IsNullOrEmpty(AdresseDemande.QUARTIER) ? string.Empty : AdresseDemande.QUARTIER;
                this.Txt_LibelleQuartier.Text = string.IsNullOrEmpty(AdresseDemande.LIBELLEQUARTIER) ? string.Empty : AdresseDemande.LIBELLEQUARTIER;
                this.Txt_CodeQuartier.Tag = AdresseDemande.FK_IDQUARTIER==null ?null: (int?) AdresseDemande.FK_IDQUARTIER;

                this.Txt_CodeSecteur.Text = string.IsNullOrEmpty(AdresseDemande.SECTEUR) ? string.Empty : AdresseDemande.SECTEUR;
                this.Txt_LibelleSecteur.Text = string.IsNullOrEmpty(AdresseDemande.LIBELLESECTEUR) ? string.Empty : AdresseDemande.LIBELLESECTEUR;
                this.Txt_CodeSecteur.Tag = AdresseDemande.FK_IDSECTEUR == null ? null : (int?)AdresseDemande.FK_IDSECTEUR;

                this.Txt_CodeNomRue.Text = string.IsNullOrEmpty(AdresseDemande.RUE) ? string.Empty : AdresseDemande.RUE;
                this.Txt_CodeNomRue.Tag = AdresseDemande.FK_IDRUE == null ? null : (int?)AdresseDemande.FK_IDRUE;


                this.Txt_Etage.Text = string.IsNullOrEmpty(AdresseDemande.ETAGE) ? string.Empty : AdresseDemande.ETAGE;
                this.Txt_OrdreTour.Text = string.IsNullOrEmpty(AdresseDemande.ORDTOUR) ? string.Empty : AdresseDemande.ORDTOUR;
                this.Txt_Tournee.Text = string.IsNullOrEmpty(AdresseDemande.TOURNEE) ? string.Empty : AdresseDemande.TOURNEE;
                this.Txt_Tournee.Tag = AdresseDemande.FK_IDTOURNEE == null ? null : (int?)AdresseDemande.FK_IDTOURNEE;

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

                AcceuilServiceClient service1 = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
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
        private void ChargerLaListeDesCommunes()
        {
            try
            {
                if (SessionObject.LstCommune.Count != 0)
                    return;
                AcceuilServiceClient service = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
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
                AcceuilServiceClient service = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
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
                AcceuilServiceClient service = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
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
                AcceuilServiceClient service = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
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
            AcceuilServiceClient service = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
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
            if ( !string.IsNullOrEmpty(this.Txt_CodeCommune.Text) &&
                this.Txt_CodeCommune.Text.Length == SessionObject.Enumere.TailleCodeQuartier &&
                LstCommuneAll.Where(t => t.FK_IDCENTRE == (int)laDetailDemande.Ag.FK_IDCENTRE).ToList().Count != 0)
            {
                CsCommune LaCommuneSelect = ClasseMEthodeGenerique.RetourneObjectFromList(LstCommuneAll.Where(t => t.FK_IDCENTRE == (int)laDetailDemande.Ag.FK_IDCENTRE).ToList(), this.Txt_CodeCommune.Text, "CODE");
                if (!string.IsNullOrEmpty(LaCommuneSelect.LIBELLE))
                {
                    this.Txt_LibelleCommune.Text = LaCommuneSelect.LIBELLE;
                    this.Txt_CodeCommune.Tag = LaCommuneSelect.PK_ID;
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
            if (laDetailDemande.Ag.FK_IDCENTRE != 0)
                LstCommuneAll = SessionObject.LstCommune.Where(p => p.FK_IDCENTRE == (int)laDetailDemande.Ag.FK_IDCENTRE).ToList();
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
            {
                this.Txt_CodeCommune.Text = _CommuneSelect.CODE;
                this.Txt_CodeCommune.Tag = _CommuneSelect.PK_ID;
            }
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
            if (this.Txt_CodeCommune.Tag != null &&  LstQuartierAll.Count != 0 && !string.IsNullOrEmpty(this.Txt_CodeQuartier.Text) &&
                this.Txt_CodeQuartier.Text.Length == SessionObject.Enumere.TailleCodeQuartier)
            {

                CsQuartier _LeQuartier = LstQuartierAll.Where(t => t.FK_IDCOMMUNE  == (int)this.Txt_CodeCommune.Tag).ToList().FirstOrDefault(p => p.CODE == this.Txt_CodeQuartier.Text);
                if (_LeQuartier != null)
                {
                    this.Txt_LibelleQuartier.Text = _LeQuartier.LIBELLE;
                    this.Txt_CodeQuartier.Tag = _LeQuartier.PK_ID;
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

            if (this.Txt_CodeCommune.Tag != null && LstQuartierAll.Where(t => t.PK_ID == (int)this.Txt_CodeCommune.Tag).ToList() != null && LstQuartierAll.Where(t => t.PK_ID == (int)this.Txt_CodeCommune.Tag).ToList().Count != 0)
            {
                this.btn_Quartier.IsEnabled = false;
                List<object> _LstObjQuartier = ClasseMEthodeGenerique.RetourneListeObjet(LstQuartierAll.Where(t => t.PK_ID == (int)this.Txt_CodeCommune.Tag).ToList().OrderByDescending(k => k.COMMUNE).ToList());
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
                {
                    this.Txt_CodeQuartier.Text = _LeQuartier.CODE;
                    this.Txt_CodeQuartier.Tag = _LeQuartier.PK_ID;
                }
            }
        }
        #endregion
        #region Tournee
        private void Txt_Tournee_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(this.Txt_Tournee.Text))
            {
                CsTournee _tourne = LstZone.FirstOrDefault(p => p.CENTRE == laDetailDemande.Ag.CENTRE && p.CODE == this.Txt_Tournee.Text);
                if (_tourne == null)
                {

                    var w = new MessageBoxControl.MessageBoxChildWindow(Langue.lbl_Menu, Langue.MsgTourneNonTrouvee, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    w.OnMessageBoxClosed += (_, result) =>
                    {
                        this.Txt_Tournee.Focus();
                    };
                    w.Show();
                }
                else
                {
                    this.Txt_Tournee.Tag = _tourne.PK_ID;
                }
            }

        }
        private void btn_zone_Click(object sender, RoutedEventArgs e)
        {
            if (LstZone != null && LstZone.Count != 0)
            {
                if ((int)laDetailDemande.Ag.FK_IDCENTRE != 0)
                {
                    List<object> _LstObj = ClasseMEthodeGenerique.RetourneListeObjet(LstZone.Where(p => p.FK_IDCENTRE == (int)laDetailDemande.Ag.FK_IDCENTRE).ToList());
                    UcListeGenerique ctr = new UcListeGenerique(_LstObj, "CODE", "LIBELLE", Langue.lbl_ListeQuartiers);
                    ctr.Closed += new EventHandler(galatee_OkClickedBtnTournee);
                    ctr.Show();
                }
            }
        }
        void galatee_OkClickedBtnTournee(object sender, EventArgs e)
        {
            UcListeGenerique ctrs = sender as UcListeGenerique;
            if (ctrs.isOkClick == true )
            {
                CsTournee _LaTournee = (CsTournee)ctrs.MyObject;
                this.Txt_Tournee.Text = _LaTournee.CODE;
                this.Txt_Tournee.Tag = _LaTournee.PK_ID;
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
            if (this.Txt_CodeQuartier.Tag != null && LstSecteurAll.Where(t => t.FK_IDQUARTIER == (int)this.Txt_CodeQuartier.Tag).ToList().Count != 0 && !string.IsNullOrEmpty(this.Txt_CodeSecteur.Text) &&
                this.Txt_CodeSecteur.Text.Length == SessionObject.Enumere.TailleCodeQuartier)
            {
                List<CsSecteur> lesSecteur = LstSecteurAll.Where(t => t.FK_IDQUARTIER == (int)this.Txt_CodeQuartier.Tag).ToList();
                CsSecteur _LeSecteur = lesSecteur.FirstOrDefault(p => p.CODE == this.Txt_CodeSecteur.Text);
                if (_LeSecteur != null)
                {
                    this.Txt_LibelleSecteur.Text = _LeSecteur.LIBELLE;
                    this.Txt_CodeSecteur.Tag = _LeSecteur.PK_ID ;
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


            if (LstSecteurAll.Where(t => t.FK_IDQUARTIER == (int)this.Txt_CodeQuartier.Tag) != null && LstSecteurAll.Where(t => t.FK_IDQUARTIER == (int)this.Txt_CodeQuartier.Tag).ToList().Count != 0)
            {
                List<object> _LstObjSecteur = ClasseMEthodeGenerique.RetourneListeObjet(LstSecteurAll.Where(t => t.FK_IDQUARTIER == (int)this.Txt_CodeQuartier.Tag).ToList());
                UcListeGenerique ctr = new UcListeGenerique(_LstObjSecteur, "CODE", "LIBELLE", Langue.lbl_ListeDesSecteurs);
                ctr.Closed += galatee_OkClickedBtnSecteur;
                ctr.Show();
            }
        }
        void galatee_OkClickedBtnSecteur(object sender, EventArgs e)
        {
            UcListeGenerique ctrs = sender as UcListeGenerique;
            CsSecteur _LeSecteur = (CsSecteur)ctrs.MyObject;
            if (_LeSecteur != null)
                this.Txt_CodeSecteur.Text = _LeSecteur.CODE;
            this.Txt_CodeSecteur.Tag = _LeSecteur.PK_ID;
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
      
   
        #endregion
        private void ActionControle(bool Etat)
        {
            this.btn_Commune.IsEnabled = Etat;
            this.btn_Quartier.IsEnabled = Etat;
            this.btn_Secteur.IsEnabled = Etat;
            this.btn_zone.IsEnabled = Etat;

            this.Txt_NomClient.IsEnabled = Etat;
            this.Txt_CodeCommune.IsEnabled = Etat;
            this.Txt_LibelleCommune.IsEnabled = Etat;
            this.Txt_CodeQuartier.IsEnabled = Etat;
            this.Txt_LibelleQuartier.IsEnabled = Etat;
            this.Txt_CodeSecteur.IsEnabled = Etat;
            this.Txt_LibelleSecteur.IsEnabled = Etat;
            this.Txt_CodeNomRue.IsEnabled = Etat;
            this.Txt_Etage.IsEnabled = Etat;

            this.Txt_OrdreTour.IsEnabled = Etat;
            this.Txt_Tournee.IsEnabled = Etat;

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
            this.OKButton.IsEnabled = false;
            EnregisterDemande(laDetailDemande);
        }
        ObjDOCUMENTSCANNE leDoc = new ObjDOCUMENTSCANNE();
        public void EnregisterDemande(CsDemande _LaDemande)
        {
            try
            {
                _LaDemande.Ag.NOMP = string.IsNullOrEmpty(this.Txt_NomClient.Text) ? null : this.Txt_NomClient.Text;
                _LaDemande.Ag.COMMUNE = string.IsNullOrEmpty(this.Txt_CodeCommune.Text) ? null : this.Txt_CodeCommune.Text;
                _LaDemande.Ag.QUARTIER = string.IsNullOrEmpty(this.Txt_CodeQuartier.Text) ? null : this.Txt_CodeQuartier.Text;
                _LaDemande.Ag.SECTEUR = string.IsNullOrEmpty(this.Txt_CodeSecteur.Text) ? null : this.Txt_CodeSecteur.Text;
                _LaDemande.Ag.RUE = string.IsNullOrEmpty(this.Txt_CodeNomRue.Text) ? null : this.Txt_CodeNomRue.Text;
                _LaDemande.Ag.PORTE = string.IsNullOrEmpty(this.Txt_Porte.Text) ? null : this.Txt_Porte.Text;
                _LaDemande.Ag.TOURNEE = string.IsNullOrEmpty(this.Txt_Tournee.Text) ? null : this.Txt_Tournee.Text;
                _LaDemande.Ag.ORDTOUR = string.IsNullOrEmpty(this.Txt_OrdreTour.Text) ? null : this.Txt_OrdreTour.Text;
                _LaDemande.Ag .ETAGE = string.IsNullOrEmpty(this.Txt_Etage.Text) ? null : this.Txt_Etage.Text;
                _LaDemande.Ag.USERCREATION = UserConnecte.matricule;
                _LaDemande.Ag.USERMODIFICATION = UserConnecte.matricule;
                _LaDemande.Ag.DATECREATION =  System.DateTime.Now;
                _LaDemande.Ag.DATEMODIFICATION =  System.DateTime.Now;
                _LaDemande.Ag.FK_IDCOMMUNE = _LaDemande.Ag.FK_IDCOMMUNE;
                _LaDemande.Ag.FK_IDSECTEUR = Txt_CodeSecteur.Tag == null ? null : (int?)Txt_CodeSecteur.Tag;
                _LaDemande.Ag.FK_IDQUARTIER = Txt_CodeQuartier.Tag == null ? null : (int?)Txt_CodeQuartier.Tag;
                _LaDemande.Ag.FK_IDRUE = Txt_CodeNomRue.Tag == null ? null : (int?)Txt_CodeNomRue.Tag;
                _LaDemande.Ag.FK_IDTOURNEE = this.Txt_Tournee.Tag == null ? null : (int?)this.Txt_Tournee.Tag;

                if (_LaDemande.LaDemande == null) _LaDemande.LaDemande = new CsDemandeBase();
                _LaDemande.LaDemande.MATRICULE = UserConnecte.matricule;
                _LaDemande.LaDemande.CENTRE = _LaDemande.Ag.CENTRE;
                _LaDemande.LaDemande.CLIENT = _LaDemande.Ag.CLIENT;
                _LaDemande.LaDemande.ORDRE = _LaDemande.LeClient != null ? _LaDemande.LeClient.ORDRE : _LaDemande.LaDemande.ORDRE;
                _LaDemande.LaDemande.STATUT = SessionObject.Enumere.DemandeStatusEnAttente;
                _LaDemande.LaDemande.TYPEDEMANDE = Tdem;
                _LaDemande.LaDemande.FK_IDTYPEDEMANDE = SessionObject.LstTypeDemande.FirstOrDefault(t => t.CODE == Tdem) != null ? SessionObject.LstTypeDemande.FirstOrDefault(t => t.CODE == Tdem).PK_ID : 0;
                _LaDemande.LaDemande.FK_IDCENTRE = (int)txtCentre.Tag;
                _LaDemande.LaDemande.MOTIF = string.IsNullOrEmpty(this.Txt_Motif.Text) ? null : this.Txt_Motif.Text;

                _LaDemande.LaDemande.USERCREATION = UserConnecte.matricule;
                _LaDemande.LaDemande.USERMODIFICATION = UserConnecte.matricule;
                _LaDemande.LaDemande.DATECREATION = System.DateTime.Now;
                _LaDemande.LaDemande.DATEMODIFICATION = System.DateTime.Now;
                _LaDemande.LaDemande.ISDEMANDEREJETERINIT = false;
                _LaDemande.LaDemande.FK_IDPRODUIT = (_LaDemande.Abonne != null && _LaDemande.Abonne.FK_IDPRODUIT != null) ? _LaDemande.Abonne.FK_IDPRODUIT : _LaDemande.LaDemande.FK_IDPRODUIT;
                _LaDemande.LaDemande.PRODUIT = (_LaDemande.Abonne != null && _LaDemande.Abonne.PRODUIT != null) ? _LaDemande.Abonne.PRODUIT : _LaDemande.LaDemande.PRODUIT;


                #region Doc Scanne
                if (_LaDemande.ObjetScanne == null) _LaDemande.ObjetScanne = new List<ObjDOCUMENTSCANNE>();
                if (LstPiece.Count != 0)
                    _LaDemande.ObjetScanne.AddRange(LstPiece);
                #endregion

                _LaDemande.Abonne = null;
                _LaDemande.Branchement  = null;
                _LaDemande.LstCanalistion  = null;
                _LaDemande.LstEvenement  = null;
                 

                ValidationDemande(_LaDemande);

            }
            catch (Exception)
            {
                Message.ShowError("Erreur à la récuperation des données", "erreur");
            }
        }
        private void ValidationDemande(CsDemande _LaDemande)
        {
            try
            {
                //Lancer la transaction de mise a jour en base
                AcceuilServiceClient service1 = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service1.CreeDemandeAsync(_LaDemande,true);
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
                    if (laDetailDemande.LaDemande != null)
                        laDetailDemande.LaDemande.FICHIERJOINT = pDocumentScane.PK_ID;
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

        private void cbo_typedoc_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void btn_RechercheClient_Click(object sender, RoutedEventArgs e)
        {
            try
            {
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
                prgBar.Visibility = System.Windows.Visibility.Visible;
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.RetourneClientByReferenceCompleted += (s, args) =>
                {
                    prgBar.Visibility = System.Windows.Visibility.Collapsed;
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
                        CsClient leClient = args.Result.First();
                        leClient.TYPEDEMANDE = Tdem;
                        VerifieExisteDemande(leClient);
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
                    prgBar.Visibility = System.Windows.Visibility.Visible;

                    string OrdreMax = string.Empty;
                    AcceuilServiceClient service = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                    service.RetourneDemandeClientTypeCompleted += (s, args) =>
                    {
                        prgBar.Visibility = System.Windows.Visibility.Collapsed;
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
                prgBar.Visibility = System.Windows.Visibility.Visible;

                leclient.TYPEDEMANDE = Tdem;
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient client = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                client.ChargerDetailClientAsync(leclient);
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
                        if (laDetailDemande.Abonne != null && laDetailDemande.Abonne.DRES == null)
                        {
                            this.txtSite.Text = string.IsNullOrEmpty(laDetailDemande.LeClient.LIBELLESITE) ? string.Empty : laDetailDemande.LeClient.LIBELLESITE;
                            this.txtCentre.Text = string.IsNullOrEmpty(laDetailDemande.LeClient.LIBELLECENTRE) ? string.Empty : laDetailDemande.LeClient.LIBELLECENTRE;
                            this.txtCentre.Tag = laDetailDemande.Abonne.FK_IDCENTRE;
                            this.txt_tdem.Text = SessionObject.LstTypeDemande.FirstOrDefault(t => t.CODE == Tdem).LIBELLE;
                            txt_tdem.Tag = SessionObject.LstTypeDemande.FirstOrDefault(t => t.CODE == Tdem);
                            AfficherInfoAdresse(laDetailDemande.Ag );
                            AfficherDocumentScanne(laDetailDemande.ObjetScanne);
                        }
                        else
                        {
                            Message.ShowInformation("Ce abonné est résilié", "Info");
                            return;
                        }
                    }
                };
            }
            catch (Exception ex)
            {
                prgBar.Visibility = System.Windows.Visibility.Collapsed;
                Message.ShowError("Erreur au chargement des donnéés", "Demande");
            }
        }
        private void ChargeDetailDEvis(int IdDemandeDevis)
        {
            prgBar.Visibility = System.Windows.Visibility.Visible;
            AcceuilServiceClient client = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
            client.ChargerDetailDemandeAsync(IdDemandeDevis,string.Empty );
            client.ChargerDetailDemandeCompleted  += (ssender, args) =>
            {
                prgBar.Visibility = System.Windows.Visibility.Collapsed;
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
                    this.txtCentre.Tag = laDetailDemande.Ag .FK_IDCENTRE;
                    Tdem = laDetailDemande.LaDemande.TYPEDEMANDE;
                    this.txt_tdem.Text = SessionObject.LstTypeDemande.FirstOrDefault(t => t.CODE == Tdem).LIBELLE;
                    txt_tdem.Tag = SessionObject.LstTypeDemande.FirstOrDefault(t => t.CODE == Tdem);
                    Tdem = laDetailDemande.LaDemande.TYPEDEMANDE;
                    this.Txt_Motif.Text = string.IsNullOrEmpty(laDetailDemande.LaDemande.MOTIF) ? string.Empty : laDetailDemande.LaDemande.MOTIF;
                    //if (laDetailDemande.LstCommentaire != null && laDetailDemande.LstCommentaire.Count != 0)
                    if (laDetailDemande.AnnotationDemande != null && laDetailDemande.AnnotationDemande.Count != 0)
                        this.Txt_MotifRejet.Text = laDetailDemande.AnnotationDemande.First().COMMENTAIRE;
                    
                    AfficherInfoAdresse(laDetailDemande.Ag );
                    AfficherDocumentScanne(laDetailDemande.ObjetScanne);
                    IsRejeterDemande = true;
                }

            };
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

