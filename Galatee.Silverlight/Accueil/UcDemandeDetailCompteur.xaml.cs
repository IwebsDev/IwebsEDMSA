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
    public partial class UcDemandeDetailCompteur : UserControl
    {

        List<CsTcompteur> LstTypeCompteur = new List<CsTcompteur>();
        List<CsDiacomp> LstDiametre = new List<CsDiacomp>();
        List<CsMarqueCompteur > LstMarque = new List<CsMarqueCompteur>();
        List<CsCadran > LstCadran = new List<CsCadran>() ;

        CsCanalisation CanalisationAfficher= new CsCanalisation();
        CsEvenement EvenementAfficher = new CsEvenement();
        List<CsEvenement> LstEvenement;

        CsEvenement LsDernierEvenement = new CsEvenement();
        int NumCompteur = 0;
        decimal initValue = 0;
        bool IsUpdate = false;
        public  CsDemande LaDemande = new CsDemande();
        public UcDemandeDetailCompteur()
        {

            InitializeComponent();
        }

        public UcDemandeDetailCompteur(CsDemande _LaDemande,bool _IsUpdate)
        {
            InitializeComponent();
            Translate();
            this.Btn_ModifCompteur.Visibility = System.Windows.Visibility.Collapsed ;
            LaDemande = _LaDemande;
            if (LaDemande.LstCanalistion == null )
                LaDemande.LstCanalistion = new List<CsCanalisation>();
            if (LaDemande.LstEvenement  == null)
                LaDemande.LstEvenement = new List<CsEvenement>();
            IsUpdate = _IsUpdate;
            ChargerMarque();
            ChargerTypeCompteur();
            ChargerDiametreCompteur();
            this.Txt_CodeCadran.MaxLength = 1;


           
            ChargerDonnees();
    

        }
        void Translate()
        {
            this.lbl_consom .Content = Langue.lbl_consommation;
            this.lbl_Date.Content = Langue.lbl_Date;
            this.lbl_Diametre.Content = Langue.lbl_diametre;
            this.lbl_Localisation.Content = Langue.lbl_Localisation;
            this.lbl_Marque.Content = Langue.lbl_Marque;
            this.lbl_NumeroCompteur.Content = Langue.lbl_NumeroCompteur;
            this.lbl_periode.Content = Langue.lbl_periode;
            this.lbl_Reading.Content = Langue.lbl_Index;
            this.lbl_type.Content = Langue.lbl_type;
            this.lbl_AnneFabrication.Content = Langue.lbl_AnneFabrication;
            this.Chk_CoefMultiplication.Content = Langue.lbl_CoefMultiplication;
            this.btn_duplication.Content = Langue.btn_Duplication;
            this.btn_FistReading.Content = Langue.btn_PremierReleve;
            this.btn_MoreInfo.Content = Langue.btn_PlusInfo;
            this.btn_readingDetail.Content = Langue.btn_DetailReleve;
            this.lbl_cadran.Content = Langue.lbl_cadran;
        }
        void ChargerDiametreCompteur()
        {
            try
            {
                if (SessionObject.LstDiametreCompteur.Count != 0)
                {
                    LstDiametre = SessionObject.LstDiametreCompteur.Where(p => p.PRODUIT == LaDemande.LaDemande.PRODUIT).ToList();
                    if (LstDiametre.Count != 0)
                    {
                        if (!IsUpdate)
                        {
                            this.Txt_CodeDiametre.Text = LstDiametre[0].CODE;
                            this.Txt_LibelleDiametre.Text = LstDiametre[0].LIBELLE;
                            this.Txt_CodeDiametre.Tag = LstDiametre[0].PK_ID;
                        }
                        else
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
        void ChargerMarque()
        {
            try
            {
                if (SessionObject.LstMarque.Count != 0)
                {
                    LstMarque = SessionObject.LstMarque;
                    if (LstMarque != null && LstMarque.Count != 0)
                    {
                        if(!IsUpdate)
                        {
                            this.Txt_CodeMarque.Text = LstMarque[0].CODE;
                            this.Txt_LibelleMarque.Text = LstMarque[0].LIBELLE;
                            this.Txt_CodeMarque.Tag = LstMarque[0].PK_ID;
                        }
                        else
                        {
                        if (!string.IsNullOrEmpty(this.Txt_CodeMarque.Text) &&
                            (string.IsNullOrEmpty(this.Txt_LibelleMarque.Text)))
                        {
                            CsMarqueCompteur _LaMarque = ClasseMEthodeGenerique.RetourneObjectFromList(LstMarque, this.Txt_CodeMarque.Text, "CODE");
                            if (_LaMarque != null && !string.IsNullOrEmpty(_LaMarque.LIBELLE))
                                this.Txt_CodeMarque.Text = _LaMarque.CODE;

                           
                        }
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
                                    this.Txt_CodeMarque.Text = _LaMarque.CODE;
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
                LstTypeCompteur = SessionObject.LstTypeCompteur.Where(p => p.PRODUIT == LaDemande.LaDemande.PRODUIT).ToList();
                if (LstTypeCompteur.Count != 0)
                {
                    if (!IsUpdate)
                    {
                        this.Txt_CodeTypeCompteur.Text = LstTypeCompteur[0].CODE;
                        this.Txt_LibelleTypeCompteur.Text = LstTypeCompteur[0].LIBELLE;
                        this.Txt_CodeTypeCompteur.Tag = LstTypeCompteur[0].PK_ID;
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(this.Txt_CodeTypeCompteur.Text) &&
                                   (string.IsNullOrEmpty(this.Txt_LibelleTypeCompteur.Text)))
                        {
                            CsTcompteur _LeType = ClasseMEthodeGenerique.RetourneObjectFromList(LstTypeCompteur, this.Txt_CodeTypeCompteur.Text, "CODE");
                            if (_LeType != null && !string.IsNullOrEmpty(_LeType.LIBELLE))
                                this.Txt_CodeTypeCompteur.Text = _LeType.CODE;
                        }
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
                                this.Txt_CodeTypeCompteur.Text = _LeType.CODE;
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
                if (!IsUpdate)
                {

                    if (LaDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.AbonnementSeul ||
                        LaDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.Reabonnement ||
                        LaDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.FermetureBrt ||
                        LaDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.ReouvertureBrt)
                        RetourneInfoCanalisation(LaDemande.LaDemande.FK_IDCENTRE,LaDemande.LaDemande.CENTRE, LaDemande.LaDemande.CLIENT, LaDemande.LaDemande.PRODUIT, null);
                    if (LaDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.ChangementCompteur ||
                        LaDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.PoseCompteur ||
                       (LaDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.DeposeCompteur) ||
                       LaDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.FermetureBrt ||
                        LaDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.Resiliation)
                        RetourneInfoCanalisation(LaDemande.LaDemande.FK_IDCENTRE,LaDemande.LaDemande.CENTRE, LaDemande.LaDemande.CLIENT, LaDemande.LaDemande.PRODUIT, null);
                    if (LaDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.BranchementSimple ||
                        LaDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.FermetureBrt ||
                        LaDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.ReouvertureBrt ||
                        LaDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.FermetureBrt ||
                        LaDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.BranchementAbonement)
                    {
                        this.txt_periode.Text = System.DateTime.Now.Month.ToString().PadLeft(2, '0') + "/" + System.DateTime.Now.Year.ToString();

                        this.Txt_DateEvenement.Text = DateTime.Now.ToShortDateString();
                        Txt_CodeCadran.Text = "7";
                        Txt_Index.Text = "0";
                    }
                }
                else
                {
                    CsCanalisation _LaCannalisationAfficher = new CsCanalisation();
                    if (LaDemande.LstCanalistion != null && LaDemande.LstCanalistion.Count != 0) _LaCannalisationAfficher = LaDemande.LstCanalistion[0];
                    CsEvenement _LeEvtAffiche = new CsEvenement();
                    if (LaDemande.LstEvenement != null && LaDemande.LstEvenement.Count != 0) _LeEvtAffiche = LaDemande.LstEvenement[0];
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
                if (LaDemande.LeTypeDemande.DEMOPTION7 == SessionObject.Enumere.CodeObligatoire)
                {
                    this.Txt_NumCompteur.IsReadOnly = true;
                    this.Txt_CodeDiametre.IsReadOnly = true;
                    this.Txt_CodeCadran.IsReadOnly = true;
                    this.Txt_CodeMarque.IsReadOnly = true;
                    this.Txt_AnneeFab.IsReadOnly = true;

                    this.btn_DiametreCompteur.IsEnabled = false;
                    this.btn_Marque.IsEnabled = false;
                    this.btn_typeCompteur.IsEnabled = false;

                    this.Txt_CodeTypeCompteur.IsReadOnly = true;
                    this.Btn_ModifCompteur.Visibility = System.Windows.Visibility.Visible;


                    //UcListeCompteurAuto _ctrl = new UcListeCompteurAuto(LaDemande);
                    //_ctrl.Closed += new EventHandler(_ctrl_Closed);
                    //_ctrl.Show();
                }
                if (LaDemande.LeTypeDemande.DEMOPTION10 != "O")
                {
                    this.Txt_DateEvenement.Background = new SolidColorBrush(Colors.Transparent);
                    this.txt_periode.Background = new SolidColorBrush(Colors.Transparent);
                    this.Txt_Index.Background = new SolidColorBrush(Colors.Transparent);

                    this.Txt_DateEvenement.IsEnabled = false;
                    this.txt_periode.IsEnabled = false;
                    this.Txt_Index.IsEnabled = false;
                    this.Txt_Consomation.IsEnabled = false;

                    this.btn_FistReading.IsEnabled = false;
                    this.btn_readingDetail.IsEnabled = false;
                    this.button1.IsEnabled = false;
                }
                else
                {
                    this.Txt_DateEvenement.Background = new SolidColorBrush(Color.FromArgb(100, 173, 216, 230));
                    this.txt_periode.Background = new SolidColorBrush(Color.FromArgb(100, 173, 216, 230));
                    this.Txt_Index.Background = new SolidColorBrush(Color.FromArgb(100, 173, 216, 230));

                   
                }
                this.Txt_Client.Text = string.IsNullOrEmpty(LaDemande.LaDemande.CLIENT) ? string.Empty : LaDemande.LaDemande.CLIENT;
                this.Txt_Addresse.Text = string.IsNullOrEmpty(LaDemande.LaDemande.CLIENT) ? string.Empty : LaDemande.LaDemande.CLIENT;
                this.Txt_Ordre.Text = string.IsNullOrEmpty(LaDemande.LaDemande.ORDRE) ? string.Empty : LaDemande.LaDemande.ORDRE;


                this.Txt_CoefDeMultiplication.Text = initValue.ToString();
                this.Chk_CoefMultiplication.IsChecked = true;
                CsCanalisation _LaCannalisationAfficher = new CsCanalisation();
                if (LaDemande.LstCanalistion != null && LaDemande.LstCanalistion.Count != 0) _LaCannalisationAfficher = LaDemande.LstCanalistion[0];
                CsEvenement _LeEvtAffiche = new CsEvenement();
                if (LaDemande.LstEvenement != null && LaDemande.LstEvenement.Count != 0)
                {
                    _LeEvtAffiche = LaDemande.LstEvenement[0];
                    AfficherCannalisationDemande(_LaCannalisationAfficher, _LeEvtAffiche);
                }
                if (!IsUpdate && !SessionObject.IsCompteur1Saisie )
                {
                    if (SessionObject.Enumere.IsDevisPrisEnCompteAuGuichet && LaDemande.LeDevis != null )
                        AfficherCannalisationDemandeDevis(LaDemande.LeDevis);
                }
                if (LaDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.BranchementSimple ||
                       LaDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.FermetureBrt ||
                       LaDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.ReouvertureBrt ||
                       LaDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.FermetureBrt ||
                       LaDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.BranchementAbonement)
                {
                    if(string.IsNullOrEmpty(this.txt_periode.Text))
                        this.txt_periode.Text = System.DateTime.Now.Month.ToString().PadLeft(2, '0') + "/" + System.DateTime.Now.Year.ToString();

                    if (string.IsNullOrEmpty(this.Txt_DateEvenement.Text))
                        this.Txt_DateEvenement.Text = DateTime.Now.ToShortDateString();

                    if (string.IsNullOrEmpty(this.Txt_CodeCadran.Text))
                        this.Txt_CodeCadran.Text = "7";

                    if (string.IsNullOrEmpty(this.Txt_Index.Text))
                        this.Txt_Index.Text = "0";
                }
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }
        int idCompteSelect = 0;
        void _ctrl_Closed(object sender, EventArgs e)
        {
            //UcListeCompteurAuto ctrs = sender as UcListeCompteurAuto;
            //if (ctrs.leCompteurSelect != null )
            //{
            //    CsComptDispo _LeCompt = (CsComptDispo)ctrs.leCompteurSelect ;
            //    //this.Txt_CodeDiametre.Text = string.IsNullOrEmpty(_LeCompt.DIAMETRE)? string.Empty : _LeCompt.DIAMETRE;
            //    //this.Txt_AnneeFab.Text = string.IsNullOrEmpty(_LeCompt.ANNEEFAB) ? string.Empty : _LeCompt.ANNEEFAB;
            //    ////this.Txt_CodeCadran.Text =  _LeCompt.CADRAN.Value.ToString();
            //    //this.Txt_CodeMarque.Text = string.IsNullOrEmpty(_LeCompt.MARQUE) ? string.Empty : _LeCompt.MARQUE; ;
            //    //this.Txt_CodeTypeCompteur.Text = string.IsNullOrEmpty(_LeCompt.TYPECOMPTEUR) ? string.Empty : _LeCompt.TYPECOMPTEUR;
            //    //this.Txt_NumCompteur.Text = string.IsNullOrEmpty(_LeCompt.COMPTEUR) ? string.Empty : _LeCompt.COMPTEUR;
            //    idCompteSelect = _LeCompt.PK_ID;
            //    //this.btn_Rechercher.Visibility = System.Windows.Visibility.Collapsed ;
            //}
            this.btn_DiametreCompteur.IsEnabled = true;
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

            if (LaDemande.LeTypeDemande.DEMOPTION10 != "O")
            {
                this.Txt_DateEvenement.IsReadOnly = !Etat;
                this.txt_periode.IsReadOnly = !Etat;
                this.Txt_Index.IsReadOnly = !Etat;
                this.Txt_Consomation.IsReadOnly = !Etat;
            }
            else
            {
                this.Txt_DateEvenement.IsReadOnly = Etat;
                this.txt_periode.IsReadOnly = Etat;
                this.Txt_Index.IsReadOnly = Etat;
                this.Txt_Consomation.IsReadOnly = Etat;
            
            }

            this.btn_typeCompteur.IsEnabled = Etat;
            this.btn_Marque.IsEnabled = Etat;
            this.btn_DiametreCompteur.IsEnabled = Etat;
            this.Chk_CoefMultiplication.IsEnabled = Etat;


        }
        private void IsControleEvenementActif(bool Etat)
        {
            this.Txt_DateEvenement.IsReadOnly = !Etat;
            this.txt_periode.IsReadOnly = !Etat;
            this.Txt_Index.IsReadOnly = !Etat;
            this.Txt_Consomation.IsReadOnly = !Etat;
        }
        private void RetourneInfoCanalisation(int fk_idcentre, string centre, string client,string produit,int? point)
        {
            List<CsCanalisation> _LstCannalisation = new List<CsCanalisation>();
            CanalisationAfficher = new CsCanalisation();
            AcceuilServiceClient service = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
            service.RetourneCanalisationCompleted += (s, args) =>
            {
                if (args != null && args.Cancelled)
                    return;

                CanalisationAfficher = args.Result.FirstOrDefault(p => p.PRODUIT  == produit);
                if (LaDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.ChangementCompteur && NumCompteur == 2)
                {
                    this.Txt_CodeDiametre.Text = (string.IsNullOrEmpty(CanalisationAfficher.DIAMETRE)) ? string.Empty : CanalisationAfficher.DIAMETRE;
                    CsDiacomp _LeDiametre = ClasseMEthodeGenerique.RetourneObjectFromList(LstDiametre, this.Txt_CodeDiametre.Text, "CODE");
                    if (!string.IsNullOrEmpty(_LeDiametre.LIBELLE))
                    {
                        this.Txt_LibelleDiametre.Text = _LeDiametre.LIBELLE;
                        this.Txt_CodeDiametre.Tag  = _LeDiametre.PK_ID ;
                    }
                    this.btn_DiametreCompteur.IsEnabled = false;
                    this.Txt_CodeDiametre.IsReadOnly = true;
                    return;
                };
                if (CanalisationAfficher != null)
                {
                    if (LaDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.DeposeCompteur && CanalisationAfficher.ETATCOMPT == SessionObject.Enumere.CompteurInactifValeur)
                    {
                        Message.ShowInformation(Langue.msgCompteurDejaDepose, Langue.lbl_Menu);
                        SessionObject.EtatControlCourant = false;
                        return;
                    }
                    if (LaDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.PoseCompteur  && CanalisationAfficher.ETATCOMPT == SessionObject.Enumere.CompteurActifValeur )
                    {
                        Message.ShowInformation(Langue.msgCompteurDejaActif , Langue.lbl_Menu);
                        SessionObject.EtatControlCourant = false;
                        return;
                    }
                    _LstCannalisation.Add(CanalisationAfficher);
                    if (LaDemande.LaDemande.TYPEDEMANDE != SessionObject.Enumere.ChangementCompteur )
                    LaDemande.LstCanalistion = _LstCannalisation;
                    if (LaDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.AbonnementSeul && LaDemande.LaDemande.ORDRE != "01")
                    {
                        string Ordre = (int.Parse(LaDemande.LaDemande.ORDRE) - 1).ToString("00");
                        RetourneEvenement(fk_idcentre,centre, client, Ordre, produit, CanalisationAfficher.POINT);
                    }
                    else
                        RetourneEvenement(fk_idcentre,centre, client, LaDemande.LaDemande.ORDRE, produit, CanalisationAfficher.POINT);
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
        void AfficherCannalisationDemande(CsCanalisation  LaCanalisation,CsEvenement LeEvt)
        {

            #region Caompteut
            this.Txt_NumCompteur.Text = (string.IsNullOrEmpty(LaCanalisation.NUMERO)) ? string.Empty : LaCanalisation.NUMERO;
            this.Txt_AnneeFab.Text = (string.IsNullOrEmpty(LaCanalisation.ANNEEFAB)) ? string.Empty : LaCanalisation.ANNEEFAB;

            this.Txt_CodeTypeCompteur.Text = (LaCanalisation.TYPECOMPTEUR == null) ? string.Empty : LaCanalisation.TYPECOMPTEUR.ToString();
            if(string.IsNullOrEmpty(Txt_CodeTypeCompteur.Text))
                this.Txt_CodeTypeCompteur.Text = (LeEvt.TYPECOMPTEUR == null) ? string.Empty : LeEvt.TYPECOMPTEUR.ToString();
            CsTcompteur _LeTypeCompte = ClasseMEthodeGenerique.RetourneObjectFromList(LstTypeCompteur, this.Txt_CodeTypeCompteur.Text, "CODE");
            if (!string.IsNullOrEmpty(_LeTypeCompte.LIBELLE))
            {
                this.Txt_LibelleTypeCompteur.Text = _LeTypeCompte.LIBELLE;
                this.Txt_CodeTypeCompteur.Tag = _LeTypeCompte.PK_ID;
            }

            this.Txt_CodeMarque.Text = (LaCanalisation.MARQUE == null) ? string.Empty : LaCanalisation.MARQUE;
            CsMarqueCompteur _LaMarque = ClasseMEthodeGenerique.RetourneObjectFromList(LstMarque, this.Txt_CodeMarque.Text, "CODE");
            if (!string.IsNullOrEmpty(_LaMarque.LIBELLE))
            {
                this.Txt_LibelleMarque.Text = _LaMarque.LIBELLE;
                this.Txt_CodeMarque.Tag = _LaMarque.PK_ID;
            }
            this.Txt_CodeCadran.Text =  LaCanalisation.CADRAN.Value.ToString();
        

            this.Txt_CodeDiametre.Text = (string.IsNullOrEmpty(LaCanalisation.DIAMETRE)) ? string.Empty : LaCanalisation.DIAMETRE;
            CsDiacomp _LeDiametre = ClasseMEthodeGenerique.RetourneObjectFromList(LstDiametre, this.Txt_CodeDiametre.Text, "CODE");
            if (!string.IsNullOrEmpty(_LeDiametre.LIBELLE))
            {
                this.Txt_LibelleDiametre.Text = _LeDiametre.LIBELLE;
                this.Txt_CodeDiametre.Tag = _LeDiametre.PK_ID;
            }

            if (LaCanalisation.COEFLECT != 0)
                this.Chk_CoefMultiplication.IsChecked = true;

            #endregion
            #region Evenement
            if (LeEvt != null && !string.IsNullOrEmpty(LeEvt.CENTRE))
            {
                this.Txt_DateEvenement.Text = Convert.ToDateTime(LeEvt.DATEEVT.Value).ToShortDateString();
                this.txt_periode.Text = Galatee.Silverlight.Shared.ClasseMEthodeGenerique.FormatPeriodeMMAAAA(LeEvt.PERIODE.ToString());
                this.Txt_Index.Text = LeEvt.INDEXEVT.ToString();
            }
            #endregion
            //this.txt_periode.Text = System.DateTime.Now.Year.ToString() + System.DateTime.Now.Month.ToString().PadLeft(2, '0'); 
            if (LaDemande.LaDemande.TYPEDEMANDE  == SessionObject.Enumere.FermetureBrt||
                LaDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.ReouvertureBrt )
                IsControleActif(false);
            if ((LaDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.ChangementCompteur && NumCompteur == 1) || LaDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.DeposeCompteur )
            {
                IsControleCompteurActif(false);
                IsControleEvenementActif (true);
            }
        }
        void AfficherCannalisationDemandeDevis(CsDevis LaDemandeDevis)
        {
            this.Txt_NumCompteur.Text = (string.IsNullOrEmpty(LaDemandeDevis.LeDevis.NUMEROCTR)) ? string.Empty : LaDemandeDevis.LeDevis.NUMEROCTR;
            this.Txt_AnneeFab.Text = string.IsNullOrEmpty(LaDemandeDevis.LeDevis.DATEFABRICATIONCTR.ToString()) ? string.Empty : LaDemandeDevis.LeDevis.DATEFABRICATIONCTR.Value.Year.ToString() ;
            if (!string.IsNullOrEmpty(LaDemandeDevis.LeDevis.IDTYPECTR))
                this.Txt_CodeTypeCompteur.Text = (string.IsNullOrEmpty(LaDemandeDevis.LeDevis.IDTYPECTR )) ? string.Empty : LaDemandeDevis.LeDevis.IDTYPECTR ;
            if(!string.IsNullOrEmpty(LaDemandeDevis.LeDevis.IDMARQUECTR ))
                this.Txt_CodeMarque.Text = (string.IsNullOrEmpty(LaDemandeDevis.LeDevis.IDMARQUECTR )) ? string.Empty :LaDemandeDevis.LeDevis.IDMARQUECTR;
            if (!string.IsNullOrEmpty(LaDemandeDevis.LaDemandeDevis.DIAMETRE))
                this.Txt_CodeDiametre.Text = (string.IsNullOrEmpty(LaDemandeDevis.LaDemandeDevis.DIAMETRE )) ? string.Empty : LaDemandeDevis.LaDemandeDevis.DIAMETRE;

            //if (SessionObject.LstDiametreCompteur != null && SessionObject.LstDiametreCompteur.Count != 0 && !string.IsNullOrEmpty(LaDemandeDevis.LaDemandeDevis.DIAMETRE))
            //{
            //    CsCommune _LAcommune = SessionObject.LstCommune.FirstOrDefault(c => c.CODE == LaDemandeDevis.LaDemandeDevis.DIAMETRE);
            //    if (!string.IsNullOrEmpty(_LAcommune.LIBELLE))
            //    {
            //        this.Txt_LibelleCommune.Text = _LAcommune.LIBELLE;
            //        this.Txt_CodeCommune.Tag = _LAcommune.PK_ID;

            //    }
            //}

            this.Txt_DateEvenement.Text =(string.IsNullOrEmpty(LaDemandeDevis.LeDevis.DATEPOSECTR .ToString() )) ? string.Empty : Convert.ToDateTime( LaDemandeDevis.LeDevis.DATEPOSECTR.Value).ToShortDateString();
            this.txt_periode.Text = (string.IsNullOrEmpty(LaDemandeDevis.LeDevis.DATEPOSECTR .ToString() )) ? string.Empty : LaDemandeDevis.LeDevis.DATEPOSECTR.Value.Year.ToString() + LaDemandeDevis.LeDevis.DATEPOSECTR.Value.Month .ToString("0");
            this.Txt_Index.Text =(string.IsNullOrEmpty(LaDemandeDevis.LeDevis.INDEXPOSECTR .ToString() )) ? string.Empty : LaDemandeDevis.LeDevis.INDEXPOSECTR .ToString();
        }
        private void RetourneInfoDernierEvenementFacture(int fk_idcentre,string centre, string client, string ordre, string produit,int point)
        {
            LsDernierEvenement = new CsEvenement();
            LstEvenement = new List<CsEvenement>();
            AcceuilServiceClient service = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
            service.RetourneEvenementCompleted  += (s, args) =>
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
        CsEvenement _LeDernierEvt = new CsEvenement();
        private void RetourneEvenement(int fk_idcentre, string centre, string client, string ordre, string produit, int point)
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
                        LaDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.DeposeCompteur ||
                        LaDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.Reabonnement  ||
                        LaDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.FermetureBrt ||
                        LaDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.ReouvertureBrt ) && !IsUpdate)
                    {

                       _LeDernierEvt = ClasseMEthodeGenerique.DernierEvenement(LstEvenement, produit);
                        AfficherCannalisationDemande(CanalisationAfficher , _LeDernierEvt);
                     
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
                byte initvalue = 1;
                         CsCanalisation _LeCompteur = new CsCanalisation();
                         if (_LaDemande.LstCanalistion == null || _LaDemande.LstCanalistion.Count == 0) _LaDemande.LstCanalistion = new List<CsCanalisation>();
                         else _LeCompteur = _LaDemande.LstCanalistion[0];
 
                        _LeCompteur.CENTRE = _LaDemande.LaDemande.CENTRE;
                        _LeCompteur.CLIENT = _LaDemande.LaDemande.CLIENT;
                        _LeCompteur.PRODUIT = _LaDemande.LaDemande.PRODUIT;
                        _LeCompteur.NUMDEM = _LaDemande.LaDemande.NUMDEM;
                        _LeCompteur.POINT = 1;
                        if (CanalisationAfficher != null && CanalisationAfficher.PK_ID != 0)
                            _LeCompteur.PK_ID = CanalisationAfficher.PK_ID;
                        _LeCompteur.NUMERO = string.IsNullOrEmpty(this.Txt_NumCompteur.Text) ? string.Empty : this.Txt_NumCompteur.Text;
                        _LeCompteur.ANNEEFAB = string.IsNullOrEmpty(this.Txt_AnneeFab.Text) ? string.Empty : this.Txt_AnneeFab.Text;
                        _LeCompteur.ETATCOMPT = SessionObject.Enumere.CompteurActifValeur;
                        _LeCompteur.TYPECOMPTEUR = string.IsNullOrEmpty(this.Txt_CodeTypeCompteur.Text) ? string .Empty  : this.Txt_CodeTypeCompteur.Text;

                        _LeCompteur.MARQUE = string.IsNullOrEmpty(this.Txt_CodeMarque.Text) ? string .Empty  : this.Txt_CodeMarque.Text;
                        _LeCompteur.DIAMETRE = string.IsNullOrEmpty(this.Txt_CodeDiametre.Text) ? string.Empty : this.Txt_CodeDiametre.Text;
                        _LeCompteur.CADRAN = string.IsNullOrEmpty(this.Txt_CodeCadran.Text) ? initvalue : byte.Parse(this.Txt_CodeCadran.Text);
                        _LeCompteur.COEFLECT = string.IsNullOrEmpty(this.Txt_CoefDeMultiplication.Text) ? 0 : int.Parse(this.Txt_CoefDeMultiplication.Text);
                        _LeCompteur.USERCREATION = UserConnecte.matricule;
                        _LeCompteur.USERMODIFICATION = UserConnecte.matricule;
                        _LeCompteur.DATECREATION =  System.DateTime.Now;
                        _LeCompteur.DATEMODIFICATION =  System.DateTime.Now;
                        _LeCompteur.FK_IDABON = _LeCompteur.FK_IDABON;
                        _LeCompteur.FK_IDCENTRE = LaDemande.LaDemande.FK_IDCENTRE;
                        _LeCompteur.FK_IDPRODUIT  = LaDemande.LaDemande.FK_IDPRODUIT .Value ;
                        _LeCompteur.FK_IDDIAMETRECOMPTEUR = this.Txt_CodeDiametre.Tag == null ? _LeCompteur.FK_IDDIAMETRECOMPTEUR : int.Parse(this.Txt_CodeDiametre.Tag.ToString());
                        _LeCompteur.FK_IDMARQUECOMPTEUR = this.Txt_CodeMarque.Tag == null ? _LeCompteur.FK_IDMARQUECOMPTEUR : int.Parse(this.Txt_CodeMarque.Tag.ToString());
                        _LeCompteur.FK_IDTYPECOMPTEUR = this.Txt_CodeTypeCompteur.Tag == null ? _LeCompteur.FK_IDTYPECOMPTEUR : int.Parse(this.Txt_CodeTypeCompteur.Tag.ToString());
                        _LeCompteur.FK_IDPROPRIETAIRE = 1;
                        _LeCompteur.PROPRIO  = "1";

                        if (_LaDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.DeposeCompteur)
                        { _LeCompteur.ETAT = "0"; _LeCompteur.DATEDEPOSE = LaDemande.LaDemande.DATEFLAG; _LeCompteur.FINSERVICE = LaDemande.LaDemande.DATEFLAG; _LeCompteur.FK_IDETATCOMPTEUR = 1; }
                        else 
                        { _LeCompteur.ETAT = "1"; _LeCompteur.DATEPOSE = LaDemande.LaDemande.DATEFLAG; _LeCompteur.MISEENSERVICE = LaDemande.LaDemande.DATEFLAG; _LeCompteur.FK_IDETATCOMPTEUR = 2; }
                        if (_LaDemande.LstCanalistion!= null && _LaDemande.LstCanalistion.Count == 0)
                        _LaDemande.LstCanalistion.Add( _LeCompteur);
                        else
                            _LaDemande.LstCanalistion[0] =_LeCompteur;


                        if (LaDemande.LeTypeDemande.DEMOPTION10 == "O")
                        {
                            if (!string.IsNullOrEmpty(this.Txt_Index.Text) &&
                                !string.IsNullOrEmpty(this.txt_periode.Text) &&
                                !string.IsNullOrEmpty(this.Txt_DateEvenement.Text))
                            {
                                CsEvenement _LeEvt = new CsEvenement();
                                if (_LaDemande.LstEvenement == null || _LaDemande.LstEvenement.Count == 0) _LaDemande.LstEvenement = new List<CsEvenement>();
                                else _LeEvt = _LaDemande.LstEvenement[0];


                                int? _AncIndex = _LeEvt.INDEXEVT;
                                _LeEvt.NUMDEM = _LaDemande.LaDemande.NUMDEM;
                                _LeEvt.CENTRE = _LaDemande.LaDemande.CENTRE;
                                _LeEvt.CLIENT = _LaDemande.LaDemande.CLIENT;
                                _LeEvt.ORDRE = _LaDemande.LaDemande.ORDRE;
                                _LeEvt.PRODUIT = _LaDemande.LaDemande.PRODUIT;
                                _LeEvt.DIAMETRE = _LeCompteur.DIAMETRE;
                                _LeEvt.TYPECOMPTEUR = _LeCompteur.TYPECOMPTEUR;
                                _LeEvt.COMPTEUR = _LeCompteur.NUMERO;
                                _LeEvt.IDCANALISATION = _LeCompteur.PK_ID;
                                _LeEvt.FK_IDPRODUIT  = _LaDemande.LaDemande.FK_IDPRODUIT.Value ;
                                _LeEvt.FK_IDCENTRE  = _LaDemande.LaDemande.FK_IDCENTRE ;
                                _LeEvt.MATRICULE = _LaDemande.LaDemande.MATRICULE;
                                _LeEvt.INDEXEVT = null;
                                if (!string.IsNullOrEmpty(this.Txt_Index.Text))
                                    _LeEvt.INDEXEVT = int.Parse(this.Txt_Index.Text);

                                if (LaDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.BranchementAbonement ||
                                    LaDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.AbonnementSeul ||
                                    LaDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.BranchementSimple)
                                {
                                    if (!string.IsNullOrEmpty(this.Txt_DateEvenement.Text))
                                        _LeCompteur.DATEPOSE = _LeCompteur.MISEENSERVICE = DateTime.Parse(Txt_DateEvenement.Text);
                                }
                                _LeEvt.POINT = _LeCompteur.POINT;
                                if (_LaDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.ChangementCompteur ||
                                    _LaDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.FermetureBrt ||
                                    _LaDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.DeposeCompteur)
                                    _LeEvt.CAS = SessionObject.Enumere.CasDeposeCompteur;
                                else
                                    _LeEvt.CAS = SessionObject.Enumere.CasPoseCompteur;
                                _LeEvt.CODEEVT = SessionObject.Enumere.EvenementCodeNormale;
                                _LeEvt.STATUS = SessionObject.Enumere.EvenementCree;

                                _LeEvt.PERIODE = string.IsNullOrEmpty(this.txt_periode.Text) ? string.Empty : Galatee.Silverlight.Shared.ClasseMEthodeGenerique.FormatPeriodeAAAAMM(txt_periode.Text);
                                _LeEvt.CONSO = null;
                                if (!string.IsNullOrEmpty(this.Txt_Consomation.Text))
                                    _LeEvt.CONSO = int.Parse(Txt_Consomation.Text);
                                else if (!string.IsNullOrEmpty(this.Txt_Index.Text))
                                    _LeEvt.CONSO = int.Parse(this.Txt_Index.Text) - _AncIndex;
                                _LeEvt.DATEEVT = null;
                                if (!string.IsNullOrEmpty(this.Txt_DateEvenement.Text))
                                    _LeEvt.DATEEVT = DateTime.Parse(Txt_DateEvenement.Text);
                                _LeEvt.STATUS = SessionObject.Enumere.EvenementReleve;

                                _LeEvt.USERCREATION = UserConnecte.matricule;
                                _LeEvt.USERMODIFICATION = UserConnecte.matricule;
                                _LeEvt.DATECREATION = System.DateTime.Now;
                                _LeEvt.DATEMODIFICATION = System.DateTime.Now;

                                if (_LaDemande.LstEvenement != null && _LaDemande.LstEvenement.Count == 0)
                                    _LaDemande.LstEvenement.Add(_LeEvt);
                                else
                                    _LaDemande.LstEvenement[0] = _LeEvt;                              
                            }
                        }

            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }

        //public void EnregistrerInfoSaisieLocal(CsDemande _LaDemande)
        //{
        //    try
        //    {
        //        byte initvalue = 1;
        //        CsCanalisation _LeCompteur = new CsCanalisation();
        //        if (_LaDemande.LstCanalistion == null || _LaDemande.LstCanalistion.Count == 0) _LaDemande.LstCanalistion = new List<CsCanalisation>();
        //        else _LeCompteur = _LaDemande.LstCanalistion[0];

        //        _LeCompteur.CENTRE = _LaDemande.LaDemande.CENTRE;
        //        _LeCompteur.CLIENT = _LaDemande.LaDemande.CLIENT;
        //        _LeCompteur.PRODUIT = _LaDemande.LaDemande.PRODUIT;
        //        _LeCompteur.NUMDEM = _LaDemande.LaDemande.NUMDEM;
        //        _LeCompteur.POINT = 1;
        //        if (CanalisationAfficher != null && CanalisationAfficher.PK_ID != 0)
        //            _LeCompteur.PK_ID = CanalisationAfficher.PK_ID;
        //        _LeCompteur.NUMERO = string.IsNullOrEmpty(this.Txt_NumCompteur.Text) ? string.Empty : this.Txt_NumCompteur.Text;
        //        _LeCompteur.ANNEEFAB = string.IsNullOrEmpty(this.Txt_AnneeFab.Text) ? string.Empty : this.Txt_AnneeFab.Text;
        //        _LeCompteur.ETATCOMPT = SessionObject.Enumere.CompteurActifValeur;
        //        _LeCompteur.TYPECOMPTEUR = string.IsNullOrEmpty(this.Txt_CodeTypeCompteur.Text) ? string.Empty : this.Txt_CodeTypeCompteur.Text;

        //        _LeCompteur.MARQUE = string.IsNullOrEmpty(this.Txt_CodeMarque.Text) ? string.Empty : this.Txt_CodeMarque.Text;
        //        _LeCompteur.DIAMETRE = string.IsNullOrEmpty(this.Txt_CodeDiametre.Text) ? string.Empty : this.Txt_CodeDiametre.Text;
        //        _LeCompteur.CADRAN = string.IsNullOrEmpty(this.Txt_CodeCadran.Text) ? initvalue : byte.Parse(this.Txt_CodeCadran.Text);
        //        _LeCompteur.COEFLECT = string.IsNullOrEmpty(this.Txt_CoefDeMultiplication.Text) ? 0 : int.Parse(this.Txt_CoefDeMultiplication.Text);
        //        _LeCompteur.USERCREATION = UserConnecte.matricule;
        //        _LeCompteur.USERMODIFICATION = UserConnecte.matricule;
        //        _LeCompteur.DATECREATION = System.DateTime.Now;
        //        _LeCompteur.DATEMODIFICATION = System.DateTime.Now;
        //        _LeCompteur.FK_IDABON = idCompteSelect;
        //        _LeCompteur.FK_IDABON = idCompteSelect;
        //        _LeCompteur.FK_IDCENTRE = LaDemande.LaDemande.FK_IDCENTRE;
        //        _LeCompteur.FK_IDPRODUIT = LaDemande.LaDemande.FK_IDPRODUIT.Value;
        //        _LeCompteur.FK_IDDIAMETRECOMPTEUR = this.Txt_CodeDiametre.Tag == null ? _LeCompteur.FK_IDDIAMETRECOMPTEUR : int.Parse(this.Txt_CodeDiametre.Tag.ToString());
        //        _LeCompteur.FK_IDMARQUECOMPTEUR = this.Txt_CodeMarque.Tag == null ? _LeCompteur.FK_IDMARQUECOMPTEUR : int.Parse(this.Txt_CodeMarque.Tag.ToString());
        //        _LeCompteur.FK_IDTYPECOMPTEUR = this.Txt_CodeTypeCompteur.Tag == null ? _LeCompteur.FK_IDTYPECOMPTEUR : int.Parse(this.Txt_CodeTypeCompteur.Tag.ToString());
        //        _LeCompteur.PROPRIO = "0";
        //        _LeCompteur.FK_IDPROPRIETAIRE  = 0;

        //        if (_LaDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.DeposeCompteur)
        //        { _LeCompteur.ETAT = "0"; _LeCompteur.DATEDEPOSE = LaDemande.LaDemande.DATEFLAG; _LeCompteur.FINSERVICE = LaDemande.LaDemande.DATEFLAG; _LeCompteur.FK_IDETATCOMPTEUR = 0; }
        //        if (_LaDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.PoseCompteur)
        //        { _LeCompteur.ETAT = "1"; _LeCompteur.DATEPOSE = LaDemande.LaDemande.DATEFLAG; _LeCompteur.MISEENSERVICE = LaDemande.LaDemande.DATEFLAG; _LeCompteur.FK_IDETATCOMPTEUR = 1; }
        //        if (_LaDemande.LstCanalistion != null && _LaDemande.LstCanalistion.Count == 0)
        //            _LaDemande.LstCanalistion.Add(_LeCompteur);
        //        else
        //            _LaDemande.LstCanalistion[0] = _LeCompteur;


        //        if (LaDemande.LeTypeDemande.DEMOPTION10 == "O")
        //        {
        //            if (!string.IsNullOrEmpty(this.Txt_Index.Text) &&
        //                !string.IsNullOrEmpty(this.txt_periode.Text) &&
        //                !string.IsNullOrEmpty(this.Txt_DateEvenement.Text))
        //            {
        //                CsEvenement _LeEvt = new CsEvenement();
        //                if (_LaDemande.LstEvenement == null || _LaDemande.LstEvenement.Count == 0) _LaDemande.LstEvenement = new List<CsEvenement>();
        //                else _LeEvt = _LaDemande.LstEvenement[0];


        //                int? _AncIndex = _LeEvt.INDEXEVT;
        //                _LeEvt.NUMDEM = _LaDemande.LaDemande.NUMDEM;
        //                _LeEvt.CENTRE = _LaDemande.LaDemande.CENTRE;
        //                _LeEvt.CLIENT = _LaDemande.LaDemande.CLIENT;
        //                _LeEvt.ORDRE = _LaDemande.LaDemande.ORDRE;
        //                _LeEvt.PRODUIT = _LaDemande.LaDemande.PRODUIT;
        //                _LeEvt.DIAMETRE = _LeCompteur.DIAMETRE;
        //                _LeEvt.TYPECOMPTEUR = _LeCompteur.TYPECOMPTEUR;
        //                _LeEvt.COMPTEUR = _LeCompteur.NUMERO;
        //                _LeEvt.IDCANALISATION = _LeCompteur.PK_ID;
        //                _LeEvt.FK_IDPRODUIT = _LaDemande.LaDemande.FK_IDPRODUIT.Value;
        //                _LeEvt.FK_IDCENTRE = _LaDemande.LaDemande.FK_IDCENTRE;
        //                _LeEvt.MATRICULE = _LaDemande.LaDemande.MATRICULE;
        //                _LeEvt.INDEXEVT = null;
        //                if (!string.IsNullOrEmpty(this.Txt_Index.Text))
        //                    _LeEvt.INDEXEVT = int.Parse(this.Txt_Index.Text);

        //                if (LaDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.BranchementAbonement ||
        //                    LaDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.AbonnementSeul ||
        //                    LaDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.BranchementSimple)
        //                {
        //                    if (!string.IsNullOrEmpty(this.Txt_DateEvenement.Text))
        //                        _LeCompteur.DATEPOSE = _LeCompteur.MISEENSERVICE = DateTime.Parse(Txt_DateEvenement.Text);
        //                }
        //                _LeEvt.POINT = _LeCompteur.POINT;
        //                if (_LaDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.ChangementCompteur ||
        //                    _LaDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.FermetureBrt ||
        //                    _LaDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.DeposeCompteur)
        //                {
        //                    _LeEvt.CAS = SessionObject.Enumere.CasDeposeCompteur;
        //                    //_LeEvt.FK_IDCAS = SessionObject.LsDesCas.FirstOrDefault(t => t.NUMCAS == SessionObject.Enumere.CasDeposeCompteur).PK_ID;
        //                }
        //                else
        //                {
        //                    _LeEvt.CAS = SessionObject.Enumere.CasPoseCompteur;
        //                    //_LeEvt.FK_IDCAS = SessionObject.LsDesCas.FirstOrDefault(t => t.NUMCAS == SessionObject.Enumere.CasDeposeCompteur).PK_ID;
        //                }
        //                _LeEvt.CODEEVT = SessionObject.Enumere.EvenementCodeNormale;
        //                _LeEvt.STATUS = SessionObject.Enumere.EvenementCree;

        //                _LeEvt.PERIODE = string.IsNullOrEmpty(this.txt_periode.Text) ? string.Empty : Galatee.Silverlight.Shared.ClasseMEthodeGenerique.FormatPeriodeAAAAMM(txt_periode.Text);
        //                _LeEvt.CONSO = null;
        //                if (!string.IsNullOrEmpty(this.Txt_Consomation.Text))
        //                    _LeEvt.CONSO = int.Parse(Txt_Consomation.Text);
        //                else if (!string.IsNullOrEmpty(this.Txt_Index.Text))
        //                    _LeEvt.CONSO = int.Parse(this.Txt_Index.Text) - _AncIndex;
        //                _LeEvt.DATEEVT = null;
        //                if (!string.IsNullOrEmpty(this.Txt_DateEvenement.Text))
        //                    _LeEvt.DATEEVT = DateTime.Parse(Txt_DateEvenement.Text);
        //                _LeEvt.STATUS = SessionObject.Enumere.EvenementReleve;

        //                _LeEvt.USERCREATION = UserConnecte.matricule;
        //                _LeEvt.USERMODIFICATION = UserConnecte.matricule;
        //                _LeEvt.DATECREATION = System.DateTime.Now;
        //                _LeEvt.DATEMODIFICATION = System.DateTime.Now;

        //                if (_LaDemande.LstEvenement != null && _LaDemande.LstEvenement.Count == 0)
        //                    _LaDemande.LstEvenement.Add(_LeEvt);
        //                else
        //                    _LaDemande.LstEvenement[0] = _LeEvt;



        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        throw ex;
        //    }
        //}

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
                if (this.Txt_CodeDiametre.Text.Length == SessionObject.Enumere.TailleDiametre  && (LstDiametre !=null && LstDiametre.Count != 0))
                {
                    CsDiacomp _LeDiametre = ClasseMEthodeGenerique.RetourneObjectFromList(LstDiametre, this.Txt_CodeDiametre.Text, "CODE");
                    if (!string.IsNullOrEmpty( _LeDiametre.LIBELLE) )
                    {
                        this.Txt_LibelleDiametre .Text = _LeDiametre.LIBELLE;
                        this.Txt_CodeDiametre.Tag = _LeDiametre.PK_ID;

                    }
                    else
                    {
                        var w = new MessageBoxControl.MessageBoxChildWindow(Langue.lbl_Menu, Langue.MsgEltInexistent , MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        w.OnMessageBoxClosed += (_, result) =>
                        {
                            this.Txt_CodeDiametre.Focus();
                        };
                        w.Show();
                        Txt_CodeDiametre.Text = string.Empty;
                        Txt_LibelleDiametre.Text = string.Empty;
                        this.Txt_CodeDiametre.Tag = null;
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
                if (ctrs.isOkClick )
                {
                    CsDiacomp _LeDiametre = (CsDiacomp)ctrs.MyObject;
                    this.Txt_CodeDiametre.Text = _LeDiametre.CODE ;
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
                        this.Txt_LibelleTypeCompteur.Text = _LeTypeCompte.LIBELLE;
                        this.Txt_CodeTypeCompteur.Tag = _LeTypeCompte.PK_ID;
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
                        Txt_CodeTypeCompteur.Tag = null ;

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
                    UcListeGenerique ctr = new UcListeGenerique(_LstObject, "CODE", "LIBELLE", "Liste");
                    ctr.Closed += new EventHandler(galatee_OkClickedbtntypeCompteur);
                    ctr.Show();
                }
            }
            catch (Exception ex )
            {
                Message.ShowError(ex.Message, Langue.lbl_Menu);
            }
        }
        void galatee_OkClickedbtntypeCompteur(object sender, EventArgs e)
        {
            try
            {
                UcListeGenerique ctrs = sender as UcListeGenerique;
                if (ctrs.isOkClick )
                {
                    CsTcompteur _LeTypeCompteur = (CsTcompteur)ctrs.MyObject;
                    this.Txt_CodeTypeCompteur.Text = _LeTypeCompteur.CODE ;
                }
                this.btn_typeCompteur.IsEnabled = true;
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message,Langue.lbl_Menu );
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
                        this.Txt_LibelleMarque .Text = _LaMarque.LIBELLE;
                        this.Txt_CodeMarque.Tag = _LaMarque.PK_ID;

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
                        this.Txt_CodeMarque.Tag = null;

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
        }
        void galatee_OkClickedbtn_Marque(object sender, EventArgs e)
        {
            try
            {
                UcListeGenerique ctrs = sender as UcListeGenerique;
                if (ctrs.isOkClick )
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

        private void Txt_AnneeFab_LostFocus(object sender, RoutedEventArgs e)
        {
            //if (!string.IsNullOrEmpty( this.Txt_AnneeFab.Text) )
            //    EnregistrerInfoSaisie(LaDemande);

           
        }

        private void Txt_NumCompteur_LostFocus(object sender, RoutedEventArgs e)
        {
            //if (!string.IsNullOrEmpty(this.Txt_NumCompteur.Text))
            //    EnregistrerInfoSaisie(LaDemande);
        }

        private void Txt_DateEvenement_LostFocus(object sender, RoutedEventArgs e)
        {
            //if (!string.IsNullOrEmpty(this.Txt_DateEvenement.Text))
            //    EnregistrerInfoSaisie(LaDemande);
        }

        private void txt_periode_LostFocus(object sender, RoutedEventArgs e)
        {

            if (this.txt_periode.Text.Length != 7)
            {
                this.txt_periode.Text = this.txt_periode.Text.PadLeft(7, '0');
            }
 
            //    EnregistrerInfoSaisie(LaDemande);
        }

        private void Txt_Index_LostFocus(object sender, RoutedEventArgs e)
        {
            if (_LeDernierEvt != null && _LeDernierEvt.INDEXEVT != null)
            {
                this.Txt_Consomation.Text = (Convert.ToDecimal(this.Txt_Index.Text) - _LeDernierEvt.INDEXEVT).ToString();
            }
            //if (!string.IsNullOrEmpty(this.Txt_Index.Text))
            //    EnregistrerInfoSaisie(LaDemande);
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

        private void UserControl_LostFocus_1(object sender, RoutedEventArgs e)
        {
            EnregistrerInfoSaisie(LaDemande);
        }

        private void txt_periode_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txt_periode.Text.Length == 7)
            {
                if (!Galatee.Silverlight.Shared.ClasseMEthodeGenerique.IsFormatPeriodeValide(txt_periode.Text))
                {
                    Message.ShowError(Galatee.Silverlight.Resources.Index.Langue.MsgFormatInvalide, Langue.lbl_Menu);
                    txt_periode.Focus();
                    return;
                }
            }
        }

    }
}
