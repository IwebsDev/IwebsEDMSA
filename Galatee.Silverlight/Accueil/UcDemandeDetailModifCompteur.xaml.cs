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
using Galatee.Silverlight.Resources.Accueil;
using Galatee.Silverlight.Shared;


namespace Galatee.Silverlight.Accueil
{
    public partial class UcDemandeDetailModifCompteur : UserControl
    {

        List<CsTcompteur> LstTypeCompteur = new List<CsTcompteur>();
        List<CsDiacomp> LstDiametre = new List<CsDiacomp>();
        List<CsMarqueCompteur> LstMarque = new List<CsMarqueCompteur>();
        List<CsCadran> LstCadran = new List<CsCadran>();

        CsTcompteur LeTypeCompteurSelect = new CsTcompteur();
        CsDiacomp LeDiametreSelect = new CsDiacomp();
        CsMarqueCompteur LeMarqueSelect = new CsMarqueCompteur();
        CsCadran LeCadranSelect = new CsCadran();


        CsCanalisation CanalisationAfficher = new CsCanalisation();
        CsEvenement EvenementAfficher = new CsEvenement();
        List<CsEvenement> LstEvenement;

        CsEvenement LsDernierEvenement = new CsEvenement();
        int NumCompteur = 0;
        decimal initValue = 0;
        bool IsUpdate = false;
        string TypeDemande;
        public CsDemande LaDemande = new CsDemande();
        public UcDemandeDetailModifCompteur()
        {

            InitializeComponent();
        }
        public UcDemandeDetailModifCompteur(CsCanalisation _leCompteur, string _typeDemande)
        {
            InitializeComponent();
            Translate();
            TypeDemande = _typeDemande;
            CanalisationAfficher = _leCompteur;
            if (LaDemande.LstCanalistion == null)
                LaDemande.LstCanalistion = new List<CsCanalisation>();
            if (LaDemande.LstEvenement == null)
                LaDemande.LstEvenement = new List<CsEvenement>();
            ChargerCadran();
            ChargerMarque();
            ChargerTypeCompteur();
            ChargerDiametreCompteur();
            AfficherCannalisationDemande(_leCompteur);
        }
        public UcDemandeDetailModifCompteur(CsDemande _LaDemande, int _NumCompteur, bool _IsUpdate)
        {
            InitializeComponent();
            Translate();
            LaDemande = _LaDemande;
            TypeDemande = _LaDemande.LaDemande.TYPEDEMANDE;
            if (LaDemande.LstCanalistion == null)
                LaDemande.LstCanalistion = new List<CsCanalisation>();
            if (LaDemande.LstEvenement == null)
                LaDemande.LstEvenement = new List<CsEvenement>();
            CanalisationAfficher = LaDemande.LstCanalistion[0];
            IsUpdate = _IsUpdate;
            ChargerCadran();
            ChargerMarque();
            ChargerTypeCompteur();
            ChargerDiametreCompteur();
            NumCompteur = _NumCompteur;
            ChargerDonnees();
        }
        void Translate()
        {
            this.lbl_Diametre.Content = Langue.lbl_diametre;
            this.lbl_Localisation.Content = Langue.lbl_Localisation;
            this.lbl_Marque.Content = Langue.lbl_Marque;
            this.lbl_NumeroCompteur.Content = Langue.lbl_NumeroCompteur;
            this.lbl_type.Content = Langue.lbl_type;
            this.lbl_AnneFabrication.Content = Langue.lbl_AnneFabrication;
            this.Chk_CoefMultiplication.Content = Langue.lbl_CoefMultiplication;
            this.lbl_cadran.Content = Langue.lbl_cadran;
        }
        void ChargerDiametreCompteur()
        {
            try
            {
                if (SessionObject.LstDiametreCompteur.Count != 0)
                {
                    if (TypeDemande == SessionObject.Enumere.ModificationCompteur)
                        LstDiametre = SessionObject.LstDiametreCompteur;
                    else
                        LstDiametre = SessionObject.LstDiametreCompteur.Where(p => p.PRODUIT == LaDemande.LaDemande.PRODUIT).ToList();
                    if (LstDiametre.Count != 0)
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
                        LstDiametre = new List<CsDiacomp>();
                        if (args != null && args.Cancelled)
                            return;
                        LstDiametre = args.Result;
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
                            CsCadran _LeCadran = ClasseMEthodeGenerique.RetourneObjectFromList(LstCadran, this.Txt_CodeCadran.Text, "CODECADRAN");
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
                        LstCadran = new List<CsCadran>();
                        if (args != null && args.Cancelled)
                            return;
                        LstCadran = args.Result;
                        if (LstCadran != null && LstCadran.Count != 0)
                        {
                            if (!string.IsNullOrEmpty(this.Txt_CodeCadran.Text) &&
                                (string.IsNullOrEmpty(this.Txt_LibelleDigit.Text)))
                            {
                                CsCadran _LeCadran = ClasseMEthodeGenerique.RetourneObjectFromList(LstCadran, this.Txt_CodeCadran.Text, "CODECADRAN");
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
                        LstMarque = new List<CsMarqueCompteur>();
                        if (args != null && args.Cancelled)
                            return;
                        LstMarque = args.Result;
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
            LstTypeCompteur = new List<CsTcompteur>();

            if (SessionObject.LstTypeCompteur.Count != 0)
            {
                if (TypeDemande == SessionObject.Enumere.ModificationCompteur)
                    LstTypeCompteur = SessionObject.LstTypeCompteur;
                else
                    LstTypeCompteur = SessionObject.LstTypeCompteur.Where(p => p.PRODUIT == LaDemande.LaDemande.PRODUIT).ToList();
                if (LstTypeCompteur.Count != 0)
                {
                    if (!string.IsNullOrEmpty(this.Txt_CodeTypeCompteur.Text) &&
                               (string.IsNullOrEmpty(this.Txt_LibelleTypeCompteur.Text)))
                    {
                        CsTcompteur _LeType = ClasseMEthodeGenerique.RetourneObjectFromList(LstTypeCompteur, this.Txt_CodeTypeCompteur.Text, "CODE");
                        if (_LeType != null && !string.IsNullOrEmpty(_LeType.LIBELLE))
                            this.Txt_LibelleTypeCompteur.Text = _LeType.LIBELLE;
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
                            (string.IsNullOrEmpty(this.Txt_LibelleTypeCompteur.Text)))
                        {
                            CsTcompteur _LeType = ClasseMEthodeGenerique.RetourneObjectFromList(LstTypeCompteur, this.Txt_CodeTypeCompteur.Text, "CODE");
                            if (_LeType != null && !string.IsNullOrEmpty(_LeType.LIBELLE))
                                this.Txt_LibelleTypeCompteur.Text = _LeType.LIBELLE;
                        }
                    }
                };
                service.ChargerTypeAsync();
                service.CloseAsync();
            }
        }
        void ChargerDonnees()
        {
            try
            {
                if (TypeDemande != SessionObject.Enumere.ModificationCompteur )
                {
                    CsCanalisation _LaCannalisationAfficher = new CsCanalisation();
                    if (LaDemande.LstCanalistion != null && LaDemande.LstCanalistion.Count != 0) _LaCannalisationAfficher = LaDemande.LstCanalistion[0];
                    CsEvenement _LeEvtAffiche = new CsEvenement();
                    if (LaDemande.LstCanalistion != null && LaDemande.LstCanalistion.Count != 0) _LeEvtAffiche = LaDemande.LstEvenement[0];
                    AfficherCannalisationDemande(_LaCannalisationAfficher, _LeEvtAffiche);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        void InitialiseCtrl()
        {
            try
            {
                if (TypeDemande != SessionObject.Enumere.ModificationCompteur)
                {
                    if (LaDemande.LeTypeDemande.DEMOPTION7 == SessionObject.Enumere.CodeObligatoire)
                    {
                        this.Txt_NumCompteur.IsReadOnly = true;
                        this.Txt_CodeDiametre.IsReadOnly = true;
                        this.Txt_CodeCadran.IsReadOnly = true;
                        this.Txt_CodeMarque.IsReadOnly = true;
                        this.Txt_AnneeFab.IsReadOnly = true;

                        this.btn_Cadran.IsEnabled = false;
                        this.btn_DiametreCompteur.IsEnabled = false;
                        this.btn_Marque.IsEnabled = false;
                        this.btn_typeCompteur.IsEnabled = false;

                        this.Txt_CodeTypeCompteur.IsReadOnly = true;
                        this.Btn_ModifCompteur.Visibility = System.Windows.Visibility.Visible;


                        //UcListeCompteurAuto _ctrl = new UcListeCompteurAuto(LaDemande);
                        //_ctrl.Closed += new EventHandler(_ctrl_Closed);
                        //_ctrl.Show();
                    }
                  
                    
                    this.Txt_Client.Text = string.IsNullOrEmpty(LaDemande.LaDemande.CLIENT) ? string.Empty : LaDemande.LaDemande.CLIENT;
                    this.Txt_Addresse.Text = string.IsNullOrEmpty(LaDemande.LaDemande.CLIENT) ? string.Empty : LaDemande.LaDemande.CLIENT;
                    this.Txt_Ordre.Text = string.IsNullOrEmpty(LaDemande.LaDemande.ORDRE) ? string.Empty : LaDemande.LaDemande.ORDRE;

                    this.Txt_CoefDeMultiplication.Text = initValue.ToString();
                    this.Chk_CoefMultiplication.IsChecked = true;

                    if (!IsUpdate)
                    {
                        if (SessionObject.Enumere.IsDevisPrisEnCompteAuGuichet && LaDemande.LeDevis != null)
                            AfficherCannalisationDemandeDevis(LaDemande.LeDevis);
                    }
                }
                else
                {
                    if (TypeDemande == SessionObject.Enumere.ModificationCompteur)
                        AfficherCannalisationDemande(CanalisationAfficher);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        void _ctrl_Closed(object sender, EventArgs e)
        {
            //AfficherDemande(LaDemande);
        }
        private void IsControleCompteurActif(bool Etat)
        {
            this.Txt_NumCompteur.IsReadOnly = !Etat;
            this.Txt_AnneeFab.IsReadOnly = !Etat;
            this.Txt_CodeTypeCompteur.IsReadOnly = !Etat;
            this.Txt_CodeMarque.IsReadOnly = !Etat;
            this.Txt_CodeDiametre.IsReadOnly = !Etat;
            this.Txt_CodeCadran.IsReadOnly = !Etat;
            this.Txt_Locatlisation.IsReadOnly = !Etat;
            this.Txt_CoefDeMultiplication.IsReadOnly = !Etat;

            this.btn_typeCompteur.IsEnabled = Etat;
            this.btn_Marque.IsEnabled = Etat;
            this.btn_DiametreCompteur.IsEnabled = Etat;
            this.btn_Cadran.IsEnabled = Etat;
            this.Chk_CoefMultiplication.IsEnabled = Etat;
        }
        private void IsControleActif(bool Etat)
        {
            this.Txt_NumCompteur.IsReadOnly = !Etat;
            this.Txt_AnneeFab.IsReadOnly = !Etat;
            this.Txt_CodeTypeCompteur.IsReadOnly = !Etat;
            this.Txt_CodeMarque.IsReadOnly = !Etat;
            this.Txt_CodeDiametre.IsReadOnly = !Etat;
            this.Txt_CodeCadran.IsReadOnly = !Etat;
            this.Txt_Locatlisation.IsReadOnly = !Etat;
            this.Txt_CoefDeMultiplication.IsReadOnly = !Etat;

    
         

            this.btn_typeCompteur.IsEnabled = Etat;
            this.btn_Marque.IsEnabled = Etat;
            this.btn_DiametreCompteur.IsEnabled = Etat;
            this.btn_Cadran.IsEnabled = Etat;
            this.Chk_CoefMultiplication.IsEnabled = Etat;


        }

        private void RetourneInfoCanalisation(int fk_idcentre,string centre, string client, string produit, int? point)
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
                    if (LaDemande.LaDemande.TYPEDEMANDE != SessionObject.Enumere.ChangementCompteur)
                        LaDemande.LstCanalistion = _LstCannalisation;
                    string Ordre = LaDemande.LaDemande.ORDRE;
                    if (LaDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.AbonnementSeul && LaDemande.LaDemande.ORDRE != "01")
                         Ordre = (int.Parse(LaDemande.LaDemande.ORDRE) - 1).ToString("00");

                        RetourneEvenement(fk_idcentre,centre, client, Ordre, produit, CanalisationAfficher.POINT);
                }
            };
            service.RetourneCanalisationAsync(fk_idcentre,centre, client, produit, point);
            service.CloseAsync();

        }
        private void RetourneInfo(CsDemande _LaDemande)
        {
            //if (LaDemande.LeTypeDemande.DEMOPTION10 == "O")
            //    RetourneInfoDEvenement(LaDemande);
            //else
            //    RetourneInfoDCanalisation(LaDemande);

        }
        void AfficherCannalisationDemande(CsCanalisation LaCanalisation)
        {
            this.Txt_NumCompteur.Text = (string.IsNullOrEmpty(LaCanalisation.NUMERO)) ? string.Empty : LaCanalisation.NUMERO;
            this.Txt_AnneeFab.Text = (string.IsNullOrEmpty(LaCanalisation.ANNEEFAB)) ? string.Empty : LaCanalisation.ANNEEFAB;

            this.Txt_CodeTypeCompteur.Text = (string.IsNullOrEmpty(LaCanalisation.TYPECOMPTEUR)) ? string.Empty : LaCanalisation.TYPECOMPTEUR;
            CsTcompteur _LeTypeCompte = ClasseMEthodeGenerique.RetourneObjectFromList(LstTypeCompteur, this.Txt_CodeTypeCompteur.Text, "CODE");
            if (!string.IsNullOrEmpty(_LeTypeCompte.LIBELLE))
            {
                LeTypeCompteurSelect = _LeTypeCompte;
                this.Txt_LibelleTypeCompteur.Text = _LeTypeCompte.LIBELLE;
            }

            this.Txt_CodeMarque.Text = (string.IsNullOrEmpty(LaCanalisation.MARQUE)) ? string.Empty : LaCanalisation.MARQUE;
            CsMarqueCompteur _LaMarque = ClasseMEthodeGenerique.RetourneObjectFromList(LstMarque, this.Txt_CodeMarque.Text, "CODE");
            if (!string.IsNullOrEmpty(_LaMarque.LIBELLE))
                this.Txt_LibelleMarque.Text = _LaMarque.LIBELLE;

            this.Txt_CodeCadran.Text =  LaCanalisation.CADRAN.Value.ToString();
            CsCadran _LeCadran = ClasseMEthodeGenerique.RetourneObjectFromList(LstCadran, this.Txt_CodeCadran.Text, "CODECADRAN");
            if (!string.IsNullOrEmpty(_LeCadran.LIBELLE))
            {
                this.Txt_LibelleDigit.Text = _LeCadran.LIBELLE;
                LeCadranSelect = _LeCadran;
            }

            this.Txt_CodeDiametre.Text = (string.IsNullOrEmpty(LaCanalisation.DIAMETRE)) ? string.Empty : LaCanalisation.DIAMETRE;
            CsDiacomp _LeDiametre = ClasseMEthodeGenerique.RetourneObjectFromList(LstDiametre, this.Txt_CodeDiametre.Text, "CODE");
            if (!string.IsNullOrEmpty(_LeDiametre.LIBELLE))
            {
                LeDiametreSelect = _LeDiametre;
                this.Txt_LibelleDiametre.Text = _LeDiametre.LIBELLE;
            }

            if (LaCanalisation.COEFLECT != 0)
                this.Chk_CoefMultiplication.IsChecked = true;

        }
        void AfficherCannalisationDemande(CsCanalisation LaCanalisation, CsEvenement LeEvt)
        {
            CsEvenement _LeEvt = new CsEvenement();
            if ((LaDemande.LeTypeDemande.CODE == SessionObject.Enumere.ChangementCompteur && NumCompteur == 1) || LaDemande.LeTypeDemande.CODE != SessionObject.Enumere.ChangementCompteur)
            {
                this.Txt_NumCompteur.Text = (string.IsNullOrEmpty(LaCanalisation.NUMERO)) ? string.Empty : LaCanalisation.NUMERO;
                this.Txt_AnneeFab.Text = (string.IsNullOrEmpty(LaCanalisation.ANNEEFAB)) ? string.Empty : LaCanalisation.ANNEEFAB;

                this.Txt_CodeTypeCompteur.Text = (string.IsNullOrEmpty(LaCanalisation.TYPECOMPTEUR)) ? string.Empty : LaCanalisation.TYPECOMPTEUR;
                CsTcompteur _LeTypeCompte = ClasseMEthodeGenerique.RetourneObjectFromList(LstTypeCompteur, this.Txt_CodeTypeCompteur.Text, "CODE");
                if (!string.IsNullOrEmpty(_LeTypeCompte.LIBELLE))
                {
                    LeTypeCompteurSelect = _LeTypeCompte;
                    this.Txt_LibelleTypeCompteur.Text = _LeTypeCompte.LIBELLE;
                }

                this.Txt_CodeMarque.Text = (string.IsNullOrEmpty(LaCanalisation.MARQUE)) ? string.Empty : LaCanalisation.MARQUE;
                CsMarqueCompteur _LaMarque = ClasseMEthodeGenerique.RetourneObjectFromList(LstMarque, this.Txt_CodeMarque.Text, "CODE");
                if (!string.IsNullOrEmpty(_LaMarque.LIBELLE))
                    this.Txt_LibelleMarque.Text = _LaMarque.LIBELLE;

                this.Txt_CodeCadran.Text =  LaCanalisation.CADRAN.Value.ToString();
                CsCadran _LeCadran = ClasseMEthodeGenerique.RetourneObjectFromList(LstCadran, this.Txt_CodeCadran.Text, "CODECADRAN");
                if (!string.IsNullOrEmpty(_LeCadran.LIBELLE))
                {
                    this.Txt_LibelleDigit.Text = _LeCadran.LIBELLE;
                    LeCadranSelect = _LeCadran;
                }

                this.Txt_CodeDiametre.Text = (string.IsNullOrEmpty(LaCanalisation.DIAMETRE)) ? string.Empty : LaCanalisation.DIAMETRE;
                CsDiacomp _LeDiametre = ClasseMEthodeGenerique.RetourneObjectFromList(LstDiametre, this.Txt_CodeDiametre.Text, "CODE");
                if (!string.IsNullOrEmpty(_LeDiametre.LIBELLE))
                {
                    LeDiametreSelect = _LeDiametre;
                    this.Txt_LibelleDiametre.Text = _LeDiametre.LIBELLE;
                }

                if (LaCanalisation.COEFLECT != 0)
                    this.Chk_CoefMultiplication.IsChecked = true;
           
            }
      
        }

        private void RetourneInfoDernierEvenementFacture(int fk_idcentre,string centre, string client, string ordre, string produit, int point)
        {
            LsDernierEvenement = new CsEvenement();
            LstEvenement = new List<CsEvenement>();
            AcceuilServiceClient service = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
            service.RetourneEvenementCompleted += (s, args) =>
            {
                if (args != null && args.Cancelled)
                    return;
                LstEvenement = args.Result;
                List<CsEvenement> LstEvenementFacture = new List<CsEvenement>();
                int EvtMAx = 0;
                if (LstEvenement != null && LstEvenement.Count != 0)
                {

                    CsEvenement _LeDernierEvt = LstEvenement.FirstOrDefault(p => p.NUMEVENEMENT == LstEvenement.Max(t => t.NUMEVENEMENT));
                    //if (_LeDernierEvt.CAS  == SessionObject.Enumere.CasCreation)
                    //    LaDemande.IsAvecMisaJourDernierEvt = true;

                    LstEvenementFacture = LstEvenement.Where(p => !string.IsNullOrEmpty(p.DERPERF) && !string.IsNullOrEmpty(p.DERPERFN)).ToList();
                    if (LstEvenementFacture != null && LstEvenementFacture.Count != 0)
                    {
                        EvtMAx = LstEvenementFacture.Max(p => p.NUMEVENEMENT);
                        CsEvenement _LeDernier = LstEvenementFacture.FirstOrDefault(p => p.NUMEVENEMENT == EvtMAx);
                        if (EvtMAx < LstEvenement.Count)
                            _LeDernier.NUMEVENEMENT = LstEvenement[LstEvenement.Count - 1].NUMEVENEMENT;
                        LsDernierEvenement = _LeDernier;
                        List<CsEvenement> _LstEvt = new List<CsEvenement>();
                        _LstEvt.Add(LsDernierEvenement);
                        //AfficherDemande(LsDernierEvenement);
                    }
                }
            };
            service.RetourneEvenementAsync(fk_idcentre,centre, client, ordre, produit, point);
            service.CloseAsync();

        }
        private void RetourneEvenement(int fk_idcentre,string centre, string client, string ordre, string produit, int point)
        {
            AcceuilServiceClient service = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
            service.RetourneEvenementCompleted += (s, args) =>
            {
                if (args != null && args.Cancelled)
                    return;
                LstEvenement = args.Result;
                if (LstEvenement != null && LstEvenement.Count != 0)
                {
                    if ((LaDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.AbonnementSeul ||
                        LaDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.ChangementCompteur ||
                        LaDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.FermetureBrt ||
                        LaDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.ReouvertureBrt) && !IsUpdate)
                    {
                        CsEvenement _LeDernierEvt = ClasseMEthodeGenerique.DernierEvenement(LstEvenement, produit);
                        AfficherCannalisationDemande(CanalisationAfficher, _LeDernierEvt);
                    }

                }
            };
            service.RetourneEvenementAsync(fk_idcentre,centre, client, ordre, produit, point);
            service.CloseAsync();

        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            InitialiseCtrl();
        }

        public void EnregistrerInfoSaisie(CsDemande _LaDemande)
        {
            try
            {
                CsCanalisation _LeCompteur = new CsCanalisation();

                    _LeCompteur.CENTRE = _LaDemande.LaDemande.CENTRE;
                    _LeCompteur.CLIENT = _LaDemande.LaDemande.CLIENT;
                    _LeCompteur.PRODUIT = _LaDemande.LaDemande.PRODUIT;
                    _LeCompteur.NUMDEM = _LaDemande.LaDemande.NUMDEM;
                    _LeCompteur.PROPRIO  = "1";
                    _LeCompteur.POINT = 1;
                    if (CanalisationAfficher != null && CanalisationAfficher.PK_ID != 0)
                        _LeCompteur.PK_ID = CanalisationAfficher.PK_ID;
                    _LeCompteur.NUMERO = string.IsNullOrEmpty(this.Txt_NumCompteur.Text) ? string.Empty : this.Txt_NumCompteur.Text;
                    _LeCompteur.ANNEEFAB = string.IsNullOrEmpty(this.Txt_AnneeFab.Text) ? string.Empty : this.Txt_AnneeFab.Text;
                    _LeCompteur.ETATCOMPT = SessionObject.Enumere.CompteurActifValeur;
                    _LeCompteur.TYPECOMPTEUR = string.IsNullOrEmpty(this.Txt_CodeTypeCompteur.Text) ? "0" : this.Txt_CodeTypeCompteur.Text;

                    _LeCompteur.MARQUE = string.IsNullOrEmpty(this.Txt_CodeMarque.Text) ? "00" : this.Txt_CodeMarque.Text;
                    _LeCompteur.DIAMETRE = string.IsNullOrEmpty(this.Txt_CodeDiametre.Text) ? string.Empty : this.Txt_CodeDiametre.Text;
                    _LeCompteur.CADRAN = Convert.ToByte(Txt_CodeCadran.Text);
                    _LeCompteur.COEFLECT = string.IsNullOrEmpty(this.Txt_CoefDeMultiplication.Text) ? 0 : int.Parse(this.Txt_CoefDeMultiplication.Text);
                    _LeCompteur.USERCREATION  = UserConnecte.matricule;
                    _LeCompteur.USERMODIFICATION = UserConnecte.matricule;
                    _LeCompteur.DATECREATION =  System.DateTime.Now;
                    _LeCompteur.DATEMODIFICATION =  System.DateTime.Now;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        private void Btn_ModifCompteur_Click(object sender, RoutedEventArgs e)
        {
            //UcListeCompteurAuto _ctrl = new UcListeCompteurAuto(LaDemande);
            //_ctrl.Closed += new EventHandler(_ctrl_Closed);
            //_ctrl.Show();
        }




        private void btn_MoreInfo_Click(object sender, RoutedEventArgs e)
        {
            UcInfoCompeur ctr = new UcInfoCompeur();
            ctr.Show();
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
                        EnregistrerInfoSaisie(LaDemande);
                    }
                    else
                    {
                        var w = new MessageBoxControl.MessageBoxChildWindow(Langue.lbl_Menu, Langue.MsgEltInexistent, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        w.OnMessageBoxClosed += (_, result) =>
                        {
                            this.Txt_CodeDiametre.Focus();
                        };
                        w.Show();
                        Txt_CodeDiametre.Text = string.Empty;
                        Txt_LibelleDiametre.Text = string.Empty;
                    }
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Langue.lbl_Menu);
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
                this.btn_DiametreCompteur.IsEnabled = true;
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
                UcListeGenerique ctrs = sender as UcListeGenerique;
                if (ctrs.GetisOkClick)
                {
                    CsDiacomp _LeDiametre = (CsDiacomp)ctrs.MyObject;
                    this.Txt_CodeDiametre.Text = _LeDiametre.CODE;
                    this.btn_DiametreCompteur.IsEnabled = true;
                }
                this.btn_DiametreCompteur.IsEnabled = true;
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Langue.lbl_Menu);
            }
        }

        private void Txt_CodeTypeCompteur_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (this.Txt_CodeTypeCompteur.Text.Length == SessionObject.Enumere.TailleCodeTypeCompteur && (LstTypeCompteur != null && LstTypeCompteur.Count != 0))
                {
                    CsTcompteur _LeTypeCompte = ClasseMEthodeGenerique.RetourneObjectFromList(LstTypeCompteur, this.Txt_CodeTypeCompteur.Text, "CODE");
                    if (!string.IsNullOrEmpty(_LeTypeCompte.LIBELLE))
                    {
                        LeTypeCompteurSelect = _LeTypeCompte;
                        this.Txt_LibelleTypeCompteur.Text = _LeTypeCompte.LIBELLE;
                        EnregistrerInfoSaisie(LaDemande);
                    }
                    else
                    {
                        var w = new MessageBoxControl.MessageBoxChildWindow(Langue.lbl_Menu, Langue.MsgEltInexistent, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        w.OnMessageBoxClosed += (_, result) =>
                        {
                            this.Txt_CodeTypeCompteur.Focus();
                        };
                        w.Show();
                        Txt_CodeTypeCompteur.Text = string.Empty;
                        Txt_LibelleTypeCompteur.Text = string.Empty;
                    }
                }
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
                    List<object> _LstObject = ClasseMEthodeGenerique.RetourneListeObjet(LstTypeCompteur);
                    UcListeGenerique ctr = new UcListeGenerique(_LstObject, "TYPE", "LIBELLE", "Liste");
                    ctr.Closed += new EventHandler(galatee_OkClickedbtntypeCompteur);
                    ctr.Show();
                }
                this.btn_typeCompteur.IsEnabled = true;
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
                UcListeGenerique ctrs = sender as UcListeGenerique;
                if (ctrs.GetisOkClick)
                {
                    CsTcompteur _LeTypeCompteur = (CsTcompteur)ctrs.MyObject;
                    this.Txt_CodeTypeCompteur.Text = _LeTypeCompteur.CODE ;
                }
                this.btn_typeCompteur.IsEnabled = true;
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
                    {
                        this.Txt_LibelleMarque.Text = _LaMarque.LIBELLE;
                        EnregistrerInfoSaisie(LaDemande);
                    }
                    else
                    {
                        var w = new MessageBoxControl.MessageBoxChildWindow(Langue.lbl_Menu, Langue.MsgEltInexistent, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        w.OnMessageBoxClosed += (_, result) =>
                        {
                            this.Txt_CodeMarque.Focus();
                        };
                        w.Show();
                        Txt_CodeMarque.Text = string.Empty;
                        Txt_LibelleMarque.Text = string.Empty;
                    }
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Langue.lbl_Menu);
            }
        }
        private void btn_Marque_Click(object sender, RoutedEventArgs e)
        {
            this.btn_Marque.IsEnabled = false;

            if (LstMarque.Count != 0)
            {
                List<object> _LstObject = ClasseMEthodeGenerique.RetourneListeObjet(LstMarque);
                UcListeGenerique ctr = new UcListeGenerique(_LstObject, "CODE", "LIBELLE", "Liste");
                ctr.Closed += new EventHandler(galatee_OkClickedbtn_Marque);
                ctr.Show();
            }
            this.btn_Marque.IsEnabled = true;
        }
        void galatee_OkClickedbtn_Marque(object sender, EventArgs e)
        {
            try
            {
                UcListeGenerique ctrs = sender as UcListeGenerique;
                if (ctrs.GetisOkClick)
                {
                    CsMarqueCompteur _LaMarque = (CsMarqueCompteur)ctrs.MyObject;
                    this.Txt_CodeMarque.Text = _LaMarque.CODE;
                    this.btn_Marque.IsEnabled = true;
                }
                else
                    this.btn_Marque.IsEnabled = true;
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
                    CsCadran _LeCadran = ClasseMEthodeGenerique.RetourneObjectFromList(LstCadran, this.Txt_CodeCadran.Text, "CODECADRAN");
                    if (!string.IsNullOrEmpty(_LeCadran.LIBELLE))
                    {
                        this.Txt_LibelleDigit.Text = _LeCadran.LIBELLE;
                        LeCadranSelect = _LeCadran;
                        EnregistrerInfoSaisie(LaDemande);
                    }
                    else
                    {
                        var w = new MessageBoxControl.MessageBoxChildWindow(Langue.lbl_Menu, Langue.MsgEltInexistent, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        w.OnMessageBoxClosed += (_, result) =>
                        {
                            this.Txt_CodeCadran.Focus();
                        };
                        w.Show();
                        Txt_CodeCadran.Text = string.Empty;
                        Txt_LibelleDigit.Text = string.Empty;
                    }
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
                    UcListeGenerique ctr = new UcListeGenerique(_LstObject, "CODECADRAN", "LIBELLE", "Liste");
                    ctr.Closed += new EventHandler(galatee_OkClickedbtnCadran);
                    ctr.Show();
                }
                this.btn_Cadran.IsEnabled = true;
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
                if (ctrs.GetisOkClick)
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

        private void Txt_AnneeFab_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(this.Txt_AnneeFab.Text))
                EnregistrerInfoSaisie(LaDemande);


        }

        private void Txt_NumCompteur_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(this.Txt_NumCompteur.Text))
                EnregistrerInfoSaisie(LaDemande);
        }


        private void Txt_CodeDiametre_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                HandleLostFocus<CsDiacomp>(Txt_CodeDiametre, Txt_LibelleDiametre, LstDiametre, SessionObject.Enumere.TailleDiametre);
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Langue.lbl_Menu);
            }
        }


        private void HandleLostFocus<T>(Library.NumericTextBox Code, TextBox Libelle, List<T> listItems)
        {
            if (!string.IsNullOrEmpty(Code.Text) &&
                Code.Text.Length == SessionObject.Enumere.TailleCodeMateriel &&
                listItems.Count != 0)
            {
                Code.Text = Code.Text.PadLeft(SessionObject.Enumere.TailleCodeMateriel, '0');
            }
            else
            {
                Code.Text = string.Empty;
                Libelle.Text = string.Empty;
            }
        }
        private void HandleLostFocus<T>(TextBox Code, TextBox Libelle, List<T> listItems, int Taille)
        {
            if (!string.IsNullOrEmpty(Code.Text) &&
                Code.Text.Length == Taille &&
                listItems.Count != 0)
            {
                Code.Text = Code.Text.PadLeft(Taille, '0');
            }
            else
            {
                Code.Text = string.Empty;
                Libelle.Text = string.Empty;
            }
        }

        private void Txt_CodeTypeCompteur_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                HandleLostFocus<CsTcompteur>(Txt_CodeTypeCompteur, Txt_LibelleTypeCompteur, LstTypeCompteur, SessionObject.Enumere.TailleCodeTypeCompteur);
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Langue.lbl_Menu);
            }
        }

        private void Txt_CodeMarque_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                HandleLostFocus<CsMarqueCompteur>(Txt_CodeMarque, Txt_LibelleMarque, LstMarque, SessionObject.Enumere.TailleCodeMarqueCompteur);
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Langue.lbl_Menu);
            }
        }

        private void Txt_CodeCadran_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                HandleLostFocus<CsCadran>(Txt_CodeCadran, Txt_LibelleDigit, LstCadran, SessionObject.Enumere.TailleDigitCompteur);
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Langue.lbl_Menu);
            }
        }

    }
}
