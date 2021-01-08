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
using Galatee.Silverlight.Resources.Accueil ;
//using Galatee.Silverlight.serviceWeb;
using Galatee.Silverlight.MainView;
using Galatee.Silverlight.Shared;
using System.IO;


namespace Galatee.Silverlight.Accueil
{
    public partial class FrmModificationCompteur : ChildWindow
    {
        public FrmModificationCompteur()
        {
            InitializeComponent();
        }
        List<CsTcompteur> LstTypeCompteur = new List<CsTcompteur>();
        List<CsDiacomp> LstDiametre = new List<CsDiacomp>();
        List<CsMarqueCompteur> LstMarque = new List<CsMarqueCompteur>();
        List<CsCadran> LstCadran = new List<CsCadran>();

        CsTcompteur LeTypeCompteurSelect = new CsTcompteur();
        CsDiacomp LeDiametreSelect = new CsDiacomp();
        CsMarqueCompteur LeMarqueSelect = new CsMarqueCompteur();
        CsCadran LeCadranSelect = new CsCadran();

        CsCentre LeCentreSelect = new CsCentre();
        CsProduit LeProduitSelect = new CsProduit();


        CsCanalisation CanalisationAfficher = new CsCanalisation();
        CsEvenement EvenementAfficher = new CsEvenement();
        List<CsCentre> LstCentre = new List<CsCentre>();
        List<CsProduit> ListeDesProduitDuSite = new List<CsProduit>();
        bool IsUpdate = false;
        CsEvenement LsDernierEvenement = new CsEvenement();
        decimal initValue = 0;
        CsDemande LaDemande = new CsDemande();
        public CsDemande MyDemande
        {
            get { return LaDemande; }
            set { LaDemande = value; }
        }
        public FrmModificationCompteur(CsDemande _LaDemande)
        {
            InitializeComponent();
            Translate();
            LaDemande = _LaDemande;
            ChargerCadran();
            ChargerMarque();
            ChargerTypeCompteur();
            ChargerDiametreCompteur ();
            ChargerDonneeDuSite();
            ChargerListeDeProduit();
            if (LaDemande.LstCanalistion == null)
                LaDemande.LstCanalistion = new List<CsCanalisation>();
            _LaDemande.LaDemande.STATUTDEMANDE = null;
            CanalisationAfficher = LaDemande.LstCanalistion[0];
            TypeDemande = LaDemande.LaDemande.TYPEDEMANDE;
            this.lnkMotif.Visibility = System.Windows.Visibility.Collapsed;
            this.btn_Supprime.Visibility = System.Windows.Visibility.Collapsed;
            this.btn_Modifier.Visibility = System.Windows.Visibility.Collapsed;
            this.btn_Rechercher .Visibility = System.Windows.Visibility.Collapsed;


       

            this.Txt_Ordre.Text = string.IsNullOrEmpty(_LaDemande.LaDemande.ORDRE) ? string.Empty : _LaDemande.LaDemande.ORDRE;
            this.Txt_NumDemande.Text = string.IsNullOrEmpty(_LaDemande.LaDemande.NUMDEM) ? string.Empty : _LaDemande.LaDemande.NUMDEM;
            this.Txt_NumDemande.IsReadOnly = true;
            this.Txt_CodeCentre.IsReadOnly = true;
            this.Txt_CodeProduit.IsReadOnly = true;
            this.Txt_Client.IsReadOnly = true;
            this.Txt_Ordre.IsReadOnly = true;
            IsUpdate = true;

            this.Txt_CodeCentre.MaxLength = SessionObject.Enumere.TailleCentre;
            this.Txt_CodeProduit.MaxLength = SessionObject.Enumere.TailleCodeProduit;
            this.Txt_Client.MaxLength = SessionObject.Enumere.TailleClient;

            this.btn_Centre.IsEnabled = false;
            this.btn_Produit.IsEnabled = false;
            this.btn_Site.IsEnabled = false;
            this.Txt_CodeSite.IsReadOnly = true;
            this.Txt_LibelleSite.IsReadOnly = true;


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
                        this.btn_Supprime.Visibility = System.Windows.Visibility.Visible ;
                        this.btn_Modifier.Visibility = System.Windows.Visibility.Visible ;
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

        string TypeDemande = string.Empty;
        public FrmModificationCompteur(string _typeDemande)
        {
            InitializeComponent();
            Translate();
            TypeDemande = _typeDemande;
            if (LaDemande.LaDemande== null) LaDemande.LaDemande = new CsDemandeBase ();
            if (LaDemande.LeCentre == null) LaDemande.LeCentre = new CsCentre();
            if (LaDemande.LeProduit == null) LaDemande.LeProduit = new CsProduit();
            if (LaDemande.LstCanalistion == null)
                LaDemande.LstCanalistion = new List<CsCanalisation>();
            this.lnkMotif.Visibility = System.Windows.Visibility.Collapsed;
            this.btn_Supprime.Visibility = System.Windows.Visibility.Collapsed ;
            this.btn_Modifier.Visibility = System.Windows.Visibility.Collapsed;
            this.Txt_Ordre.Visibility = System.Windows.Visibility.Collapsed;
            this.lbl_Ordre .Visibility = System.Windows.Visibility.Collapsed;

            ChargerCadran();
            ChargerMarque();
            ChargerTypeCompteur();
            ChargerDiametreCompteur();
            ChargerDonneeDuSite();
            ChargerListeDeProduit();

            this.Txt_CodeCentre.MaxLength = SessionObject.Enumere.TailleCentre;
            this.Txt_CodeProduit.MaxLength = SessionObject.Enumere.TailleCodeProduit;
            this.Txt_Client.MaxLength = SessionObject.Enumere.TailleClient;
        }
        void Translate()
        {
            this.lbl_Diametre.Content = Langue.lbl_diametre;
            this.lbl_Localisation.Content = Langue.lbl_Localisation;
            this.lbl_Marque.Content = Langue.lbl_Marque;
            this.lbl_NumeroCompteur.Content = Langue.lbl_NumeroCompteur;
            this.lbl_cadran.Content = Langue.lbl_cadran;
            this.lbl_type.Content = Langue.lbl_type;
            this.lbl_AnneFabrication.Content = Langue.lbl_AnneFabrication;
        }

        void InitialiseCtrl()
        {
            this.Txt_CoefDeMultiplication.Text = initValue.ToString();
            this.Chk_CoefMultiplication.IsChecked = true;
            CsCanalisation _LaCannalisationAfficher = new CsCanalisation();
            if (LaDemande.LstCanalistion != null && LaDemande.LstCanalistion.Count != 0) _LaCannalisationAfficher = LaDemande.LstCanalistion[0];
            AfficherCannalisationDemande(_LaCannalisationAfficher);
            RemplireLibelle();

            if (!string.IsNullOrEmpty(LaDemande.LaDemande.ANNOTATION))
                this.lnkMotif.Visibility = System.Windows.Visibility.Visible;
        }
        void ChargerDiametreCompteur()
        {
            try
            {
                if (SessionObject.LstDiametreCompteur.Count != 0)
                {
                    //LstDiametre = SessionObject.LstDiametreCompteur.Where(p => p.PRODUIT == LaDemande.LaDemande.PRODUIT).ToList();
                    LstDiametre = SessionObject.LstDiametreCompteur;
                    if (LstDiametre != null && LstDiametre.Count != 0)
                    {
                        if (!string.IsNullOrEmpty(this.Txt_CodeDiametre.Text) &&
                            string.IsNullOrEmpty(this.Txt_LibelleDiametre.Text))
                        {
                            CsDiacomp _LeCompteur = ClasseMEthodeGenerique.RetourneObjectFromList(LstDiametre, this.Txt_CodeDiametre.Text, "CODE");
                            if (_LeCompteur != null && !string.IsNullOrEmpty(_LeCompteur.LIBELLE))
                                this.Txt_LibelleDiametre.Text = _LeCompteur.LIBELLE;
                        }
                    }
                }
                else
                {
                    AcceuilServiceClient service = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                    service.ChargerDiametreCompteurCompleted += (s, args) =>
                    {
                        if (args != null && args.Cancelled)
                            return;
                        LstDiametre = args.Result;
                        SessionObject.LstDiametreCompteur = LstDiametre;
                        if (LstDiametre != null && LstDiametre.Count != 0)
                        {
                            if (!string.IsNullOrEmpty(this.Txt_CodeDiametre.Text) &&
                                string.IsNullOrEmpty(this.Txt_LibelleDiametre.Text))
                            {
                                CsDiacomp _LeCompteur = ClasseMEthodeGenerique.RetourneObjectFromList(LstDiametre, this.Txt_CodeDiametre.Text, "CODE");
                                if (_LeCompteur != null && !string.IsNullOrEmpty(_LeCompteur.LIBELLE))
                                    this.Txt_LibelleDiametre.Text = _LeCompteur.LIBELLE;
                            }
                        }
                    };
                    service.ChargerDiametreCompteurAsync();
                    service.CloseAsync();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        void ChargerCadran()
        {
            try
            {
                if (SessionObject.LstCadran.Count != 0)
                {
                    LstCadran = SessionObject.LstCadran;
                    if (LstCadran != null && LstCadran.Count != 0)
                    {
                        if (!string.IsNullOrEmpty(this.Txt_CodeCadran.Text) &&
                            (string.IsNullOrEmpty(this.Txt_LibelleDigit.Text)))
                        {
                            CsCadran _LeCadran = ClasseMEthodeGenerique.RetourneObjectFromList(LstCadran, this.Txt_CodeCadran.Text, "CODE");
                            if (_LeCadran != null && !string.IsNullOrEmpty(_LeCadran.LIBELLE))
                                this.Txt_LibelleDigit.Text = _LeCadran.LIBELLE;
                        }
                    }
                }
                else
                {
                    AcceuilServiceClient service = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                    service.RetourneToutCadranCompleted += (s, args) =>
                    {
                        if (args != null && args.Cancelled)
                            return;
                        LstCadran = args.Result;
                        SessionObject.LstCadran = LstCadran;
                        if (LstCadran != null && LstCadran.Count != 0)
                        {
                            if (!string.IsNullOrEmpty(this.Txt_CodeCadran.Text) &&
                                (string.IsNullOrEmpty(this.Txt_LibelleDigit.Text)))
                            {
                                CsCadran _LeCadran = ClasseMEthodeGenerique.RetourneObjectFromList(LstCadran, this.Txt_CodeCadran.Text, "CODE");
                                if (_LeCadran != null && !string.IsNullOrEmpty(_LeCadran.LIBELLE))
                                    this.Txt_LibelleDigit.Text = _LeCadran.LIBELLE;
                            }
                        }
                    };
                    service.RetourneToutCadranAsync();
                    service.CloseAsync();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        void ChargerMarque()
        {
            try
            {
                if (SessionObject.LstMarque.Count != 0)
                {
                    LstMarque = SessionObject.LstMarque;
                    if (LstMarque != null && LstMarque.Count != 0)
                    {
                        if (!string.IsNullOrEmpty(this.Txt_CodeMarque.Text) &&
                            (string.IsNullOrEmpty(this.Txt_LibelleMarque.Text)))
                        {
                            CsMarqueCompteur _LaMarque = ClasseMEthodeGenerique.RetourneObjectFromList(LstMarque, this.Txt_CodeMarque.Text, "CODE");
                            if (_LaMarque != null && !string.IsNullOrEmpty(_LaMarque.LIBELLE))
                                this.Txt_LibelleMarque.Text = _LaMarque.LIBELLE;
                        }
                    }
                }
                else
                {
                    AcceuilServiceClient service = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                    service.RetourneToutMarqueCompleted += (s, args) =>
                    {
                        if (args != null && args.Cancelled)
                            return;
                        LstMarque = args.Result;
                        SessionObject.LstMarque = LstMarque;
                        if (LstMarque != null && LstMarque.Count != 0)
                        {
                            if (!string.IsNullOrEmpty(this.Txt_CodeMarque.Text) &&
                                (string.IsNullOrEmpty(this.Txt_LibelleMarque.Text)))
                            {
                                CsMarqueCompteur _LaMarque = ClasseMEthodeGenerique.RetourneObjectFromList(LstMarque, this.Txt_CodeMarque.Text, "CODE");
                                if (_LaMarque != null && !string.IsNullOrEmpty(_LaMarque.LIBELLE))
                                    this.Txt_LibelleMarque.Text = _LaMarque.LIBELLE;
                            }
                        }
                    };
                    service.RetourneToutMarqueAsync();
                    service.CloseAsync();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        void ChargerTypeCompteur()
        {
            try
            {
                if (SessionObject.LstTypeCompteur.Count  != 0)
                {
                    LstTypeCompteur = SessionObject.LstTypeCompteur;
                    if (LstTypeCompteur != null && LstTypeCompteur.Count != 0)
                    {
                        if (!string.IsNullOrEmpty(this.Txt_CodeTypeCompteur.Text) &&
                            (string.IsNullOrEmpty(this.Txt_LibelleTypeClient.Text)))
                        {
                            CsTcompteur _LeType = ClasseMEthodeGenerique.RetourneObjectFromList(LstTypeCompteur, this.Txt_CodeTypeCompteur.Text, "TYPE");
                            if (_LeType != null && !string.IsNullOrEmpty(_LeType.LIBELLE))
                                this.Txt_LibelleTypeClient.Text = _LeType.LIBELLE;
                        }
                    }
                }
                else
                {
                    AcceuilServiceClient service = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                    service.ChargerTypeCompleted += (s, args) =>
                    {
                        if (args != null && args.Cancelled)
                            return;
                        LstTypeCompteur = args.Result;
                        if (LstTypeCompteur != null && LstTypeCompteur.Count != 0)
                        {
                            if (!string.IsNullOrEmpty(this.Txt_CodeTypeCompteur.Text) &&
                                (string.IsNullOrEmpty(this.Txt_LibelleTypeClient.Text)))
                            {
                                CsTcompteur _LeType = ClasseMEthodeGenerique.RetourneObjectFromList(LstTypeCompteur, this.Txt_CodeTypeCompteur.Text, "TYPE");
                                if (_LeType != null && !string.IsNullOrEmpty(_LeType.LIBELLE))
                                    this.Txt_LibelleTypeClient.Text = _LeType.LIBELLE;
                            }
                        }
                    };
                    service.ChargerTypeAsync();
                    service.CloseAsync();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        List<CsSite> lstSite = new List<CsSite>();
        void ChargerDonneeDuSite()
        {
            try
            {
                if (SessionObject.LstCentre != null && SessionObject.LstCentre.Count != 0 && IsUpdate)
                {
                    LstCentre = Shared.ClasseMEthodeGenerique.RetourCentreByPerimetre(SessionObject.LstCentre.Where(p => p.CODE != SessionObject.Enumere.Generale).ToList(), UserConnecte.listeProfilUser);
                    lstSite = ClasseMEthodeGenerique.RetourneSiteByCentre(LstCentre);
                    if (lstSite != null)
                    {
                        List<CsSite> _LstSite = lstSite.Where(p => p.CODESITE != SessionObject.Enumere.Generale).ToList();
                        if (_LstSite.Count == 1)
                        {
                            this.Txt_CodeSite.Text = _LstSite[0].CODESITE;
                            this.Txt_LibelleSite.Text = _LstSite[0].LIBELLE;
                            this.btn_Site.IsEnabled = false;
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
                                this.btn_Centre.IsEnabled = false;
                                this.Txt_CodeCentre.IsReadOnly = true;
                            }
                        }
                    }



                    return;
                }
                AcceuilServiceClient service = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                service.ListeDesDonneesDesSiteCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    SessionObject.LstCentre = args.Result;
                    LstCentre = SessionObject.LstCentre;
                    LstCentre = Shared.ClasseMEthodeGenerique.RetourCentreByPerimetre(SessionObject.LstCentre.Where(p => p.CODE != SessionObject.Enumere.Generale).ToList(), UserConnecte.listeProfilUser);
                    lstSite = Galatee.Silverlight.Shared.ClasseMEthodeGenerique.RetourneSiteByCentre(LstCentre);
                    if (lstSite != null)
                    {
                        List<CsSite> _LstSite = lstSite.Where(p => p.CODESITE != SessionObject.Enumere.Generale).ToList();
                        if (_LstSite.Count == 1 )
                        {
                            this.Txt_CodeSite.Text = _LstSite[0].CODESITE;
                            this.Txt_LibelleSite.Text = _LstSite[0].LIBELLE;
                            this.btn_Site.IsEnabled = false;
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
                                this.btn_Centre.IsEnabled = false;
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
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            InitialiseCtrl();
        }
        private void RetourneInfoCanalisation(int fk_idcentre, string centre, string client, string produit, int? point)
        {
            int res = LoadingManager.BeginLoading(Langue.En_Cours);
            try
            {
                List<CsCanalisation> _LstCannalisation = new List<CsCanalisation>();
                CanalisationAfficher = new CsCanalisation();
                AcceuilServiceClient service = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                service.RetourneCanalisationCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;

                    CanalisationAfficher = args.Result.FirstOrDefault(p => p.PRODUIT == produit);
                    if (CanalisationAfficher != null)
                    {
                        _LstCannalisation.Add(CanalisationAfficher);
                        AfficherCannalisationDemande(CanalisationAfficher);
                    }
                    else
                        Message.ShowInformation("Aucune information trouvée", "Recherche de compteur");
                    LoadingManager.EndLoading(res);


                };
                service.RetourneCanalisationAsync(fk_idcentre,centre, client, produit, point);
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                LoadingManager.EndLoading(res);
                throw ex;
            }
        }
 
        void AfficherCannalisationDemande(CsCanalisation LaCanalisation)
        {
                this.Txt_CodeCentre .Text = (string.IsNullOrEmpty(LaCanalisation.CENTRE)) ? string.Empty : LaCanalisation.CENTRE;
                this.Txt_CodeProduit.Text = (string.IsNullOrEmpty(LaCanalisation.PRODUIT)) ? string.Empty : LaCanalisation.PRODUIT;
                this.Txt_Client.Text = (string.IsNullOrEmpty(LaCanalisation.CLIENT)) ? string.Empty : LaCanalisation.CLIENT;

               
                this.Txt_NumCompteur.Text = (string.IsNullOrEmpty(LaCanalisation.NUMERO)) ? string.Empty : LaCanalisation.NUMERO;
                this.Txt_AnneeFab.Text = (string.IsNullOrEmpty(LaCanalisation.ANNEEFAB)) ? string.Empty : LaCanalisation.ANNEEFAB;


                this.Txt_CodeTypeCompteur.Text = (LstTypeCompteur.FirstOrDefault(c => c.PK_ID == LaCanalisation.FK_IDTYPECOMPTEUR) == null) ? string.Empty : LstTypeCompteur.FirstOrDefault(c => c.PK_ID == LaCanalisation.FK_IDTYPECOMPTEUR).CODE;
                this.Txt_CodeMarque.Text = (LstMarque.FirstOrDefault(c => c.PK_ID == LaCanalisation.FK_IDMARQUECOMPTEUR) == null) ? string.Empty : LstMarque.FirstOrDefault(c => c.PK_ID == LaCanalisation.FK_IDMARQUECOMPTEUR).CODE;
                this.Txt_CodeDiametre.Text = (string.IsNullOrEmpty(LaCanalisation.DIAMETRE)) ? string.Empty : LaCanalisation.DIAMETRE;
                if(LaCanalisation.CADRAN != null)
            this.Txt_CodeCadran.Text =  LaCanalisation.CADRAN.Value.ToString();
                if (LaCanalisation.COEFLECT != 0)
                    this.Chk_CoefMultiplication.IsChecked = true;
        }
        private void RemplireLibelle()
        {
            if (this.Txt_CodeCentre.Text.Length == SessionObject.Enumere.TailleCentre)
            {
                CsCentre _LeCentre = ClasseMEthodeGenerique.RetourneObjectFromList<CsCentre>(LstCentre, this.Txt_CodeCentre.Text, "CODE");
                if (_LeCentre != null && !string.IsNullOrEmpty(_LeCentre.CODE))
                    this.Txt_LibelleCentre.Text = _LeCentre.LIBELLE;
            }
            if (this.Txt_CodeTypeCompteur.Text.Length == SessionObject.Enumere.TailleCodeTypeCompteur && (LstTypeCompteur != null && LstTypeCompteur.Count != 0))
            {
                CsTcompteur _LeTypeCompte = ClasseMEthodeGenerique.RetourneObjectFromList(LstTypeCompteur, this.Txt_CodeTypeCompteur.Text, "TYPE");
                if (!string.IsNullOrEmpty(_LeTypeCompte.LIBELLE))
                {
                    LeTypeCompteurSelect = _LeTypeCompte;
                    this.Txt_LibelleTypeClient.Text = _LeTypeCompte.LIBELLE;
                }
            }
            if (this.Txt_CodeCadran.Text.Length == SessionObject.Enumere.TailleDigitCompteur && LstCadran != null && LstCadran.Count != 0)
            {
                CsCadran _LeCadran = ClasseMEthodeGenerique.RetourneObjectFromList(LstCadran, this.Txt_CodeCadran.Text, "CODE");
                if (!string.IsNullOrEmpty(_LeCadran.LIBELLE))
                {
                    this.Txt_LibelleDigit.Text = _LeCadran.LIBELLE;
                    LeCadranSelect = _LeCadran;
                }
            }
            if (this.Txt_CodeDiametre.Text.Length == SessionObject.Enumere.TailleDiametre && (LstDiametre != null && LstDiametre.Count != 0))
            {
                CsDiacomp _LeDiametre = ClasseMEthodeGenerique.RetourneObjectFromList(LstDiametre, this.Txt_CodeDiametre.Text, "CODE");
                if (!string.IsNullOrEmpty(_LeDiametre.LIBELLE))
                {
                    LeDiametreSelect = _LeDiametre;
                    this.Txt_LibelleDiametre.Text = _LeDiametre.LIBELLE;
                }
            }
            if (this.Txt_CodeProduit.Text.Length == SessionObject.Enumere.TailleCodeProduit)
            {
                LaDemande.LaDemande.PRODUIT = this.Txt_CodeProduit.Text;
                CsProduit _LeProduitSelect = ClasseMEthodeGenerique.RetourneObjectFromList(ListeDesProduitDuSite, this.Txt_CodeProduit.Text, "CODE");
                if (!string.IsNullOrEmpty(_LeProduitSelect.LIBELLE))
                {
                    this.Txt_LibelleProduit.Text = _LeProduitSelect.LIBELLE;
                    LaDemande.LeProduit = _LeProduitSelect;
                    LaDemande.LaDemande.FK_IDPRODUIT = _LeProduitSelect.PK_ID;

                }
            }
            if (this.Txt_CodeMarque.Text.Length == SessionObject.Enumere.TailleCodeMarqueCompteur && (LstMarque != null && LstMarque.Count != 0))
            {
                CsMarqueCompteur _LaMarque = ClasseMEthodeGenerique.RetourneObjectFromList(LstMarque, this.Txt_CodeMarque.Text, "CODE");
                if (!string.IsNullOrEmpty(_LaMarque.LIBELLE))
                    this.Txt_LibelleMarque.Text = _LaMarque.LIBELLE;
            }
           
        }
        private void btn_DiametreCompteur_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.btn_DiametreCompteur.IsEnabled = false;
                if (LstDiametre.Count != 0)
                {
                    List<object> _LstObject = ClasseMEthodeGenerique.RetourneListeObjet(LstDiametre);
                    UcListeGenerique ctr = new UcListeGenerique(_LstObject, "CODE", "LIBELLE", Langue.lbl_ListeDiametre);
                    ctr.Closed += new EventHandler(galatee_OkClickedBtnDiametre);
                    ctr.Show();
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Langue.lbl_Menu);
            }
        }
        void galatee_OkClickedBtnDiametre(object sender, EventArgs e)
        {
            try
            {
                this.btn_DiametreCompteur.IsEnabled = true;
                UcListeGenerique ctrs = sender as UcListeGenerique;
                if (ctrs.isOkClick )
                {
                    CsDiacomp _LeDiametre = (CsDiacomp)ctrs.MyObject;
                    this.Txt_CodeDiametre.Text = _LeDiametre.CODE ;
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Langue.lbl_Menu);
            }
        }
        private void btn_Marque_Click(object sender, RoutedEventArgs e)
        {
            if (LstMarque.Count != 0)
            {
                this.btn_Marque.IsEnabled = false;
                List<object> _LstObject = ClasseMEthodeGenerique.RetourneListeObjet(LstMarque);
                UcListeGenerique ctr = new UcListeGenerique(_LstObject, "CODE", "LIBELLE", "Liste");
                ctr.Closed += new EventHandler(galatee_OkClickedbtn_Marque);
                ctr.Show();
            }
        }
        void galatee_OkClickedbtn_Marque(object sender, EventArgs e)
        {
            try
            {
                this.btn_Marque.IsEnabled = true;
                UcListeGenerique ctrs = sender as UcListeGenerique;
                if (ctrs.isOkClick )
                {
                    CsMarqueCompteur _LaMarque = (CsMarqueCompteur)ctrs.MyObject;
                    this.Txt_CodeMarque.Text = _LaMarque.CODE;
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Langue.lbl_Menu);
            }
        }

        private void btn_Cadran_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (LstCadran != null && LstCadran.Count != 0)
                {
                    this.btn_Cadran.IsEnabled = false;
                    List<object> _LstObject = ClasseMEthodeGenerique.RetourneListeObjet(LstCadran);
                    UcListeGenerique ctr = new UcListeGenerique(_LstObject, "CODE", "LIBELLE", "Liste");
                    ctr.Closed += new EventHandler(galatee_OkClickedbtnCadran);
                    ctr.Show();
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Langue.lbl_Menu);
            }
        }
        void galatee_OkClickedbtnCadran(object sender, EventArgs e)
        {
            try
            {
                UcListeGenerique ctrs = sender as UcListeGenerique;
                if (ctrs.isOkClick )
                {
                    CsCadran _LeCadran = (CsCadran)ctrs.MyObject;
                    this.Txt_CodeCadran.Text = _LeCadran.CODE;
                }
                this.btn_Cadran.IsEnabled = true;
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Langue.lbl_Menu);
            }

        }

        private void btn_typeCompteur_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (LstTypeCompteur.Count != 0)
                {
                    this.btn_typeCompteur.IsEnabled = false;
                    List<object> _LstObject = ClasseMEthodeGenerique.RetourneListeObjet(LstTypeCompteur.Where(t=>t.PRODUIT == this.Txt_CodeProduit .Text ).ToList());
                    UcListeGenerique ctr = new UcListeGenerique(_LstObject, "TYPE", "LIBELLE", "Liste");
                    ctr.Closed += new EventHandler(galatee_OkClickedbtntypeCompteur);
                    ctr.Show();
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Langue.lbl_Menu);
            }
        }
        void galatee_OkClickedbtntypeCompteur(object sender, EventArgs e)
        {
            try
            {
                this.btn_typeCompteur.IsEnabled = true;
                UcListeGenerique ctrs = sender as UcListeGenerique;
                if (ctrs.GetisOkClick)
                {
                    CsTcompteur _LeTypeCompteur = (CsTcompteur)ctrs.MyObject;
                    this.Txt_CodeTypeCompteur.Text = _LeTypeCompteur.CODE ;
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
                CsProduit _LeProduitSelect = new CsProduit();
                UcListeGenerique ctrs = sender as UcListeGenerique;
                if (ctrs.isOkClick)
                {
                    _LeProduitSelect = (CsProduit)ctrs.MyObject;
                    this.Txt_CodeProduit.Text = _LeProduitSelect.CODE;
                }
            }
            catch (Exception ex)
            {

                throw ex;
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
        private void Txt_Client_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(this.Txt_Client.Text))
                this.Txt_Client.Text = this.Txt_Client.Text.PadLeft(SessionObject.Enumere.TailleClient, '0');
        }
        private void btn_Rechercher_Click_1(object sender, RoutedEventArgs e)
        {
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
                AcceuilServiceClient service = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                service.RetourneOrdreMaxCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    OrdreMax = args.Result;
                    this.lbl_Ordre .Visibility = System.Windows.Visibility.Visible;
                    this.Txt_Ordre.Visibility = System.Windows.Visibility.Visible;
                    if (OrdreMax != null)
                    {
                        this.Txt_Ordre.IsReadOnly = true;
                        this.Txt_Ordre.Text = OrdreMax;
                        VerifieExisteDemande(this.Txt_CodeCentre.Text, this.Txt_Client.Text, this.Txt_Ordre.Text, LaDemande.LaDemande.FK_IDCENTRE, TypeDemande);
                    }
                    else
                        Message.ShowError("Ce client n'est pas abonné", "Compteur");
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
                        VerifieDernierEvt(this.Txt_CodeCentre.Text, this.Txt_Client.Text, string.Empty);
                    }
                };
                service.IsDernierEvtEnFacturationAsync(centre, client, Ordre);
                service.CloseAsync();
            }

        }
        private void VerifieExisteDemande(string centre, string client,string ordre, int idCentre, string tdem)
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
                            if (args.Result.STATUT != SessionObject.Enumere.DemandeStatusPriseEnCompte && args.Result.ISSUPPRIME != true )
                            {
                                Message.ShowError("Il existe déja une demande de ce type sur ce client", "Accueil");
                                return;
                            }
                        }
                        RetourneInfoCanalisation(idCentre, this.Txt_CodeCentre.Text, this.Txt_Client.Text, this.Txt_CodeProduit.Text, null);
                    };
                    service.RetourneDemandeClientTypeAsync(centre, client, ordre, idCentre, tdem);
                    service.CloseAsync();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        public event EventHandler Closed;
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            EnregistrerInfoSaisie(LaDemande);
            ValidationDemande(LaDemande);
            this.DialogResult = false;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            InitialiseCtrl();
        }

        private void Txt_CodeTypeCompteur_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (this.Txt_CodeTypeCompteur.Text.Length == SessionObject.Enumere.TailleCodeTypeCompteur && (LstTypeCompteur != null && LstTypeCompteur.Count != 0))
                {
                    CsTcompteur _LeTypeCompte = ClasseMEthodeGenerique.RetourneObjectFromList(LstTypeCompteur.Where(n => n.PRODUIT == "03").ToList(), this.Txt_CodeTypeCompteur.Text, "CODE");
                    if (!string.IsNullOrEmpty(_LeTypeCompte.LIBELLE))
                    {
                        LeTypeCompteurSelect = _LeTypeCompte;
                        this.Txt_LibelleTypeClient.Text = _LeTypeCompte.LIBELLE;
                    }
                    else
                    {
                        var w = new MessageBoxControl.MessageBoxChildWindow(Langue.lbl_Menu, Langue.MsgEltInexistent, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        w.OnMessageBoxClosed += (_, result) =>
                        {
                            this.Txt_CodeTypeCompteur.Focus();
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

        private void Txt_CodeDiametre_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (this.Txt_CodeDiametre.Text.Length == SessionObject.Enumere.TailleDiametre && (LstDiametre != null && LstDiametre.Count != 0))
                {
                    CsDiacomp _LeDiametre = ClasseMEthodeGenerique.RetourneObjectFromList(LstDiametre, this.Txt_CodeDiametre.Text, "CODE");
                    if (!string.IsNullOrEmpty(_LeDiametre.LIBELLE))
                    {
                        LeDiametreSelect = _LeDiametre;
                        this.Txt_LibelleDiametre.Text = _LeDiametre.LIBELLE;
                    }
                    else
                    {
                        var w = new MessageBoxControl.MessageBoxChildWindow(Langue.lbl_Menu, Langue.MsgEltInexistent, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        w.OnMessageBoxClosed += (_, result) =>
                        {
                            this.Txt_CodeDiametre.Focus();
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

        private void Txt_CodeMarque_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (this.Txt_CodeMarque.Text.Length == SessionObject.Enumere.TailleCodeMarqueCompteur && (LstMarque != null && LstMarque.Count != 0))
                {
                    CsMarqueCompteur _LaMarque = ClasseMEthodeGenerique.RetourneObjectFromList(LstMarque, this.Txt_CodeMarque.Text, "CODE");
                    if (!string.IsNullOrEmpty(_LaMarque.LIBELLE))
                        this.Txt_LibelleMarque.Text = _LaMarque.LIBELLE;
                    else
                    {
                        var w = new MessageBoxControl.MessageBoxChildWindow(Langue.lbl_Menu, Langue.MsgEltInexistent, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        w.OnMessageBoxClosed += (_, result) =>
                        {
                            this.Txt_CodeMarque.Focus();
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

        private void Txt_CodeCadran_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (this.Txt_CodeCadran.Text.Length == SessionObject.Enumere.TailleDigitCompteur && LstCadran != null && LstCadran.Count != 0)
                {
                    this.Txt_CodeCadran.Text = (this.Txt_CodeCadran.Text.Length > 1) ? this.Txt_CodeCadran.Text : "0" + this.Txt_CodeCadran.Text;
                    
                    CsCadran _LeCadran = ClasseMEthodeGenerique.RetourneObjectFromList(LstCadran, this.Txt_CodeCadran.Text.ToString(), "CODE");
                    if (!string.IsNullOrEmpty(_LeCadran.LIBELLE))
                    {
                        this.Txt_LibelleDigit.Text = _LeCadran.LIBELLE;
                        LeCadranSelect = _LeCadran;
                    }
                    else
                    {
                        var w = new MessageBoxControl.MessageBoxChildWindow(Langue.lbl_Menu, Langue.MsgEltInexistent, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        w.OnMessageBoxClosed += (_, result) =>
                        {
                            this.Txt_CodeCadran.Focus();
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
        ObjDOCUMENTSCANNE leDoc = new ObjDOCUMENTSCANNE();
        public void EnregistrerInfoSaisie(CsDemande _LaDemande)
        {
            try
            {
                CsCanalisation _LeCompteur = CanalisationAfficher;
                    _LeCompteur.NUMDEM = this.Txt_NumDemande.Text  ;
                    _LeCompteur.POINT = 1;
                    _LeCompteur.NUMERO = string.IsNullOrEmpty(this.Txt_NumCompteur.Text) ? string.Empty : this.Txt_NumCompteur.Text;
                    _LeCompteur.ANNEEFAB = string.IsNullOrEmpty(this.Txt_AnneeFab.Text) ? string.Empty : this.Txt_AnneeFab.Text;
                    _LeCompteur.ETATCOMPT = SessionObject.Enumere.CompteurActifValeur;
                    _LeCompteur.TYPECOMPTEUR  = string.IsNullOrEmpty(this.Txt_CodeTypeCompteur.Text) ? "0" : this.Txt_CodeTypeCompteur.Text;

                    _LeCompteur.MARQUE  = string.IsNullOrEmpty(this.Txt_CodeMarque.Text) ? "00" : this.Txt_CodeMarque.Text;
                    _LeCompteur.DIAMETRE = string.IsNullOrEmpty(this.Txt_CodeDiametre.Text) ? string.Empty : this.Txt_CodeDiametre.Text;
                    _LeCompteur.CADRAN = Convert.ToByte( this.Txt_CodeCadran.Text);
                    _LeCompteur.COEFLECT = string.IsNullOrEmpty(this.Txt_CoefDeMultiplication.Text) ? 0 : int.Parse(this.Txt_CoefDeMultiplication.Text);
                    _LeCompteur.USERMODIFICATION = UserConnecte.matricule;
                    _LeCompteur.DATEMODIFICATION = System.DateTime.Now ;
                    _LeCompteur.USERCREATION = UserConnecte.matricule ;
                    _LeCompteur.DATECREATION = System.DateTime.Now;
                    _LeCompteur.FK_IDABON = CanalisationAfficher.FK_IDABON;
                    _LeCompteur.ETATCOMPT = CanalisationAfficher.ETATCOMPT ;

                    LaDemande.LaDemande.NUMDEM = this.Txt_NumDemande.Text;
                    LaDemande.LaDemande.STATUT = SessionObject.Enumere.DemandeStatusEnAttente;
                    LaDemande.LaDemande.TYPEDEMANDE = TypeDemande;
                    LaDemande.LaDemande.CENTRE = CanalisationAfficher.CENTRE ;
                    LaDemande.LaDemande.CLIENT = CanalisationAfficher.CLIENT;
                    LaDemande.LaDemande.ORDRE  = this.Txt_Ordre.Text ;

                    LaDemande.LaDemande.USERCREATION = UserConnecte.matricule;
                    LaDemande.LaDemande.USERMODIFICATION = UserConnecte.matricule;
                    LaDemande.LaDemande.DATECREATION = System.DateTime.Now ;
                    LaDemande.LaDemande.DATEMODIFICATION = System.DateTime.Now;

                List<CsCanalisation> _LstCompteur = new List<CsCanalisation>();
                _LstCompteur.Add(_LeCompteur);
                LaDemande.LstCanalistion = _LstCompteur;
                if (lnkLetter.Tag != null)
                {
                    leDoc = SaveFile((byte[])lnkLetter.Tag, 1, null);
                    //_Lademande.LeDocumentScanne = leDoc;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
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

    }
}

