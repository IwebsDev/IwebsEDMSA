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
using Galatee.Silverlight.Resources.Accueil  ;
using Galatee.Silverlight.Shared;


namespace Galatee.Silverlight.Accueil
{
    public partial class UcDemandeDetailBranchementMTA : UserControl
    {
        public UcDemandeDetailBranchementMTA()
        {
            InitializeComponent();
        }
        CsBrt _LeBranchement = new CsBrt();
        CsDemande LaDemande = new CsDemande();
        public CsDemande MaDemande
        {
            get { return LaDemande; }
            set { LaDemande = value; }
        }
        List<CsTypeBranchement> LstDiametreBrt;
        List<CsMaterielBranchement> LstDeMaterielBrt;
        List<CsPosteElectrique > LstDePosteElectrique;

        CsPosteElectrique LePosteBrtSelect = new CsPosteElectrique();
        CsTypeBranchement LeDiametreBrtSelect = new CsTypeBranchement();
        CsMaterielBranchement LeDeMaterielBrtSelect = new CsMaterielBranchement();
        CsDevis LeDevis = new CsDevis();
        List<CsCanalisation> CanalisationClientRecherche = new List<CsCanalisation>();
        CsBrt  BranchementClientRecherche = new  CsBrt();
        int InitValue = 0;
        bool IsUpdate = false;
        string TypeDemande = string.Empty;
        public UcDemandeDetailBranchementMTA(CsDemande _LaDemande,bool _IsUpdate)
        {
            try
            {
                InitializeComponent();
                Translate();
                this.Txt_CodeDiametre.MaxLength = SessionObject.Enumere.TailleDiametreBranchement;
                this.Txt_CodeMateriel.MaxLength = SessionObject.Enumere.TailleCodeMateriel;
                this.Txt_DateRacordement.MaxLength = SessionObject.Enumere.TailleDate;
                this.Txt_DateDepose.MaxLength = SessionObject.Enumere.TailleDate;
                this.Txt_DateDepose.Text = DateTime.Now.ToShortDateString();
                this.Txt_DateRacordement.Text = DateTime.Now.ToShortDateString();
                LaDemande = _LaDemande;
                TypeDemande = _LaDemande.LaDemande.TYPEDEMANDE;
                if (LaDemande.Branchement == null)
                    LaDemande.Branchement = new CsBrt();

                ChargerDiametreBranchement();
                ChargerMaterielBranchement();
                ChargeQuartier();
                ChargeDeparts();
                ChargerPosteElectrique();


                if (TypeDemande == SessionObject.Enumere.ModificationBranchement)
                {
                    this.Txt_Ordre.Visibility = System.Windows.Visibility.Collapsed;
                    this.lbl_Ordre.Visibility = System.Windows.Visibility.Collapsed;

                    this.Txt_Client.Text = string.IsNullOrEmpty(LaDemande.Branchement.CLIENT) ? string.Empty : LaDemande.LaDemande.CLIENT;
                    this.Txt_Addresse.Text = string.IsNullOrEmpty(LaDemande.Branchement.CLIENT) ? string.Empty : LaDemande.LaDemande.CLIENT;
                }

                IsUpdate = _IsUpdate;
                if (IsUpdate)
                {
                    if (LaDemande.Branchement != null) _LeBranchement = LaDemande.Branchement;
                    else _LeBranchement = new CsBrt();
                    AfficherBranchemetDemande(_LeBranchement);
                    if (TypeDemande != SessionObject.Enumere.ModificationBranchement)

                    EnregisterDemande(LaDemande);
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Langue.lbl_Menu);
            }
        }

        public UcDemandeDetailBranchementMTA(CsBrt _leBrt, string _typeDemande)
        {
            try
            {
                InitializeComponent();
                Translate();
                TypeDemande = _typeDemande;
                SessionObject.EtatControlCourant = true;

                if (LaDemande.LaDemande == null) LaDemande.LaDemande = new CsDemandeBase();
                LaDemande.LaDemande.PRODUIT = _leBrt.PRODUIT;
                this.Txt_CodeDiametre.MaxLength = SessionObject.Enumere.TailleDiametreBranchement;
                this.Txt_CodeMateriel.MaxLength = SessionObject.Enumere.TailleCodeMateriel;
                this.Txt_DateRacordement.MaxLength = SessionObject.Enumere.TailleDate;
                this.Txt_DateDepose.MaxLength = SessionObject.Enumere.TailleDate;
                ChargerDiametreBranchement();
                ChargerMaterielBranchement();
                ChargerPosteElectrique();
                ChargeQuartier();
                ChargeDeparts();
                this.Txt_Client.Text = string.IsNullOrEmpty(_leBrt.CLIENT) ? string.Empty : _leBrt.CLIENT;
                this.Txt_Addresse.Text = string.IsNullOrEmpty(_leBrt.CLIENT) ? string.Empty : _leBrt.CLIENT;
                this.Txt_Ordre.Visibility = System.Windows.Visibility.Collapsed;
                this.lbl_Ordre .Visibility = System.Windows.Visibility.Collapsed;
                AfficherBranchemetDemande(_leBrt);
               
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Langue.lbl_Menu);
            }
        }
        void Translate()
        {
            //Gestion de la langue
            this.lbl_adresse.Content = Langue.lbl_adresse;
            this.lbl_Client.Content = Langue.lbl_client;
            this.lbl_Codification.Content = Langue.lbl_Codification;
            this.lbl_DateAbonnement.Content = Langue.lbl_DateRacordement;
            this.lbl_DateResil.Content = Langue.lbl_DateFermeture ;
            this.lbl_Depart.Content = Langue.lbl_Depart;
            this.lbl_diametre.Content = Langue.lbl_TypeComptage;
            this.lbl_latitude.Content = Langue.lbl_latitude;
            this.lbl_longitude.Content = Langue.lbl_longitude;
            this.lbl_Materiel.Content = Langue.lbl_TypeBranchement ;
            this.lbl_latitude.Content = Langue.lbl_latitude;
            this.lbl_NoeudFinal.Content = Langue.lbl_NoeudFinal;
            this.lbl_NumeroPoste.Content = Langue.lbl_NumeroPoste;
            this.lbl_Ordre.Content = Langue.lbl_Ordre;
            this.lbl_QuartierDuPoste.Content = Langue.lbl_QuartierDuPoste;
            this.rdb_InService.Content = Langue.rdb_EnService;
            this.rdb_deconnecter.Content = Langue.rdb_HorsService;
            //
        }
        void InitCtrl()
        {
            if (TypeDemande == SessionObject.Enumere.ModificationBranchement)
                return;

            this.Txt_Client.Text = string.IsNullOrEmpty(LaDemande.LaDemande.CLIENT ) ? string.Empty : LaDemande.LaDemande.CLIENT;
            this.Txt_Addresse.Text = string.IsNullOrEmpty(LaDemande.LaDemande.CLIENT) ? string.Empty : LaDemande.LaDemande.CLIENT;
            this.Txt_Ordre.Text = string.IsNullOrEmpty(LaDemande.LaDemande.ORDRE) ? string.Empty : LaDemande.LaDemande.ORDRE; 
            this.rdb_InService.IsChecked = true;
            #region BRANCHEMENT SIMPLE && BRANCHEMENT ABONNEMENT
            if (TypeDemande    == SessionObject.Enumere.BranchementSimple||
                TypeDemande   == SessionObject.Enumere.BranchementAbonement)
            {
                this.Txt_DateDepose.Visibility = System.Windows.Visibility.Collapsed;
                this.lbl_DateResil .Visibility = System.Windows.Visibility.Collapsed;
                if (!IsUpdate)
                {
                    if (SessionObject.Enumere.IsDevisPrisEnCompteAuGuichet && LaDemande.LeDevis!= null )
                        AfficherBranchemetDemandeDevis(LaDemande.LeDevis );
                }
            }
            #endregion
            #region ABONNEMENT SEUL
            else if ((TypeDemande   == SessionObject.Enumere.AbonnementSeul))
            {
                this.Txt_DateDepose.Visibility = System.Windows.Visibility.Collapsed;
                this.lbl_DateResil.Visibility = System.Windows.Visibility.Collapsed;
                if (!IsUpdate)
                    RetourneInfoBranchement(LaDemande.LaDemande.FK_IDCENTRE ,LaDemande.LaDemande.CENTRE, LaDemande.LaDemande.CLIENT, LaDemande.LaDemande.PRODUIT);
              
            }
            #endregion
            #region REABONNEMENT
            else if (TypeDemande   == SessionObject.Enumere.Reabonnement)
            {
                if (!IsUpdate)
                    RetourneInfoBranchement(LaDemande.LaDemande.FK_IDCENTRE ,LaDemande.LaDemande.CENTRE, LaDemande.LaDemande.CLIENT, LaDemande.LaDemande.PRODUIT);
            }
            #endregion
            #region FERMETURE DE BRANCHEMENT
            else if ((TypeDemande   == SessionObject.Enumere.FermetureBrt) || 
                (TypeDemande   == SessionObject.Enumere.ReouvertureBrt))
            {
                if (!IsUpdate)
                    RetourneInfoBranchement(LaDemande.LaDemande.FK_IDCENTRE ,LaDemande.LaDemande.CENTRE, LaDemande.LaDemande.CLIENT, LaDemande.LaDemande.PRODUIT );
            }
            if (IsUpdate)
            {
                AfficherBranchemetDemande(LaDemande.Branchement);
                EnregisterDemande(LaDemande);
            }

            #endregion

        }
        void ChargerTypeCompteur(string produit, string type, string centre)
        {

            AcceuilServiceClient service = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
            service.ChargerTypeMtCompleted += (s, args) =>
            {
                if (args != null && args.Cancelled)
                    return;
                SessionObject.LstTypeCompteur .AddRange(args.Result);
            };
            service.ChargerTypeMtAsync(produit, type, centre);
            service.CloseAsync();
        }

        private void ChargerDiametreBranchement()
        {
            try
            {
                //if (SessionObject.LstDiametreBrt.Count != 0)
                //{
                //    LstDiametreBrt = SessionObject.LstDiametreBrt.Where(p=>p.PRODUIT == LaDemande.LaDemande.PRODUIT ).ToList();
                // if (LstDiametreBrt != null && LstDiametreBrt.Count != 0)
                // {
                //     if (!string.IsNullOrEmpty(this.Txt_CodeDiametre.Text))
                //     {
                //         CsDiametreBranchement _LeDiametre = ClasseMEthodeGenerique.RetourneObjectFromList(LstDiametreBrt, this.Txt_CodeDiametre.Text, "CODE");
                //         if (_LeDiametre != null && !string.IsNullOrEmpty(_LeDiametre.LIBELLE))
                //             this.Txt_LibelleDiametre.Text = _LeDiametre.LIBELLE;
                //     }
                // }
                //}
                //else
                //{
                //    AcceuilServiceClient service = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                //    service.ChargerDiamentreBranchementCompleted += (s, args) =>
                //    {
                //        if ((args != null && args.Cancelled) || (args.Error != null))
                //            return;
                //        SessionObject.LstDiametreBrt = args.Result;
                //        LstDiametreBrt = SessionObject.LstDiametreBrt;
                //        if (LstDiametreBrt != null && LstDiametreBrt.Count != 0)
                //        {
                //            if (!string.IsNullOrEmpty(this.Txt_CodeDiametre.Text))
                //            {
                //                CsDiametreBranchement _LeDiametre = ClasseMEthodeGenerique.RetourneObjectFromList(LstDiametreBrt, this.Txt_CodeDiametre.Text, "CODE");
                //                if (_LeDiametre != null && !string.IsNullOrEmpty(_LeDiametre.LIBELLE))
                //                    this.Txt_LibelleDiametre.Text = _LeDiametre.LIBELLE;
                //            }
                //        }
                //    };
                //    service.ChargerDiamentreBranchementAsync();
                //    service.CloseAsync();
                //}
            }
            catch (Exception es)
            {

                MessageBox.Show(es.Message);
            }

        }
        private void ChargerMaterielBranchement()
        {
            try
            {
                if (SessionObject.LstDeMaterielBrt.Count != 0)
                {
                    LstDeMaterielBrt = SessionObject.LstDeMaterielBrt.Where(p => p.PRODUIT == LaDemande.LaDemande.PRODUIT).ToList();
                    this.Txt_CodeMateriel.Text = string.IsNullOrEmpty(_LeBranchement.NATBRT) ? string.Empty : _LeBranchement.NATBRT;
                    if (LstDeMaterielBrt != null && LstDeMaterielBrt.Count != 0)
                    {
                        if (!string.IsNullOrEmpty(this.Txt_CodeMateriel.Text))
                        {
                            CsMaterielBranchement _LeMateriel = ClasseMEthodeGenerique.RetourneObjectFromList(LstDeMaterielBrt, this.Txt_CodeMateriel.Text, "CODE");
                            if (_LeMateriel != null && !string.IsNullOrEmpty(_LeMateriel.LIBELLE))
                                this.Txt_LibelleMateriel.Text = _LeMateriel.LIBELLE;
                        }
                    }
                
                }
                else
                {
                    AcceuilServiceClient service = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                    service.RetourneMaterielBranchementCompleted += (s, args) =>
                    {
                        if ((args != null && args.Cancelled) || (args.Error != null))
                            return;
                        SessionObject.LstDeMaterielBrt = args.Result;
                        LstDeMaterielBrt = SessionObject.LstDeMaterielBrt.Where(p => p.PRODUIT == LaDemande.LaDemande.PRODUIT).ToList();
                        this.Txt_CodeMateriel.Text = string.IsNullOrEmpty(_LeBranchement.NATBRT) ? string.Empty : _LeBranchement.NATBRT;
                        if (LstDeMaterielBrt != null && LstDeMaterielBrt.Count != 0)
                        {
                            if (!string.IsNullOrEmpty(this.Txt_CodeMateriel.Text))
                            {
                                CsMaterielBranchement _LeMateriel = ClasseMEthodeGenerique.RetourneObjectFromList(LstDeMaterielBrt, this.Txt_CodeMateriel.Text, "CODE");
                                if (_LeMateriel != null && !string.IsNullOrEmpty(_LeMateriel.LIBELLE))
                                    this.Txt_LibelleMateriel.Text = _LeMateriel.LIBELLE;
                            }
                        }
                    };
                    service.RetourneMaterielBranchementAsync();
                    service.CloseAsync();
                }
            }
            catch (Exception es)
            {
                throw es;
            }
        }
        private void ChargerPosteElectrique()
        {
            try
            {
                if (SessionObject.LsDesPosteElectriques .Count != 0)
                {
                    LstDePosteElectrique = SessionObject.LsDesPosteElectriques;
                    if (LstDePosteElectrique != null && LstDePosteElectrique.Count != 0)
                    {
                        if (!string.IsNullOrEmpty(this.Txt_LibelleDepart .Text))
                        {
                            CsPosteElectrique _LePosteElect = LstDePosteElectrique.FirstOrDefault(p => p.CODE == LaDemande.Branchement.CODEPOSTE);
                            if (_LePosteElect != null && !string.IsNullOrEmpty(_LePosteElect.LIBELLE))
                                this.Txt_SequenceNumPoste.Text = _LePosteElect.LIBELLE;
                        }
                    }

                }
                else
                {

                    AcceuilServiceClient service = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                    service.ChargerLesPosteElectriqueCompleted += (s, args) =>
                    {
                        if (args != null && args.Cancelled)
                            return;
                        SessionObject.LsDesPosteElectriques = args.Result;

                        LstDePosteElectrique = SessionObject.LsDesPosteElectriques;
                        this.Txt_CodePoste.Text = string.IsNullOrEmpty(_LeBranchement.CODEPOSTE) ? string.Empty : _LeBranchement.CODEPOSTE;
                        if (LstDePosteElectrique != null && LstDePosteElectrique.Count != 0)
                        {
                            if (!string.IsNullOrEmpty(this.Txt_CodePoste.Text))
                            {
                                CsPosteElectrique _LePoste= ClasseMEthodeGenerique.RetourneObjectFromList(LstDePosteElectrique, this.Txt_CodePoste .Text, "CODE");
                                if (_LePoste != null && !string.IsNullOrEmpty(_LePoste.LIBELLE))
                                    this.Txt_LibellePoste .Text = _LePoste.LIBELLE;
                            }
                        }

                    };
                    service.ChargerLesPosteElectriqueAsync();
                    service.CloseAsync();
                }
            }
            catch (Exception es)
            {

                MessageBox.Show(es.Message);
            }
        }
        private void IsControleActif(bool Etat)
        {
            this.Txt_CodeDiametre.IsReadOnly = !Etat;
            this.Txt_CodeMateriel.IsReadOnly = !Etat;
            this.Txt_DateDepose.IsReadOnly = !Etat;
            this.Txt_DateRacordement.IsReadOnly = !Etat;

            this.btn_diametre.IsEnabled = Etat;
            this.btn_materiel.IsEnabled = Etat;
            this.rdb_deconnecter.IsEnabled = Etat;
            this.rdb_InService.IsEnabled = Etat;
        }

        private void RetourneInfoCanalisation(int fk_idcentre, string centre, string client,string produit,int ? point)
        {
            CanalisationClientRecherche = new List<CsCanalisation>();
            AcceuilServiceClient service = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
            service.RetourneCanalisationCompleted += (s, args) =>
            {
                if (args != null && args.Cancelled)
                    return;
                CanalisationClientRecherche = args.Result;
            };
            service.RetourneCanalisationAsync(fk_idcentre,centre, client, produit, point);
            service.CloseAsync();
        }
        private void RetourneInfoBranchement(int fk_idcentre,string centre, string client, string produit)
        {
            BranchementClientRecherche = new CsBrt();
            AcceuilServiceClient service = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
            service.RetourneBranchementCompleted += (s, args) =>
            {
                if (args != null && args.Cancelled)
                    return;
                if (args.Result != null)
                {
                    BranchementClientRecherche = args.Result.FirstOrDefault(p => p.PRODUIT   == LaDemande.LaDemande.PRODUIT  );
                    if (TypeDemande == SessionObject.Enumere.FermetureBrt)
                        BranchementClientRecherche.DRES = LaDemande.Branchement.DRES;

                    LaDemande.Branchement = BranchementClientRecherche;
                    AfficherBranchemetDemande(LaDemande.Branchement);
                }

            };
            service.RetourneBranchementAsync(fk_idcentre,centre, client, produit);
            service.CloseAsync();

        }
        void AfficherBranchemetDemande(CsBrt  _LeBrtDemande)
        {
            //if (_Lademande.ISSUBVENTION == SessionObject.Enumere.IsBranchementSubventione)
            //    chk_EstSubventionne.IsChecked = true;
            //else
            ////    chk_EstSubventionne.IsChecked = false;

            //this.Txt_CodeDiametre.Text = string.IsNullOrEmpty(_LeBrtDemande.DIAMBRT) ? string.Empty : _LeBrtDemande.DIAMBRT;
            //CsDiametreBranchement _leDiametre = ClasseMEthodeGenerique.RetourneObjectFromList<CsDiametreBranchement>(LstDiametreBrt, this.Txt_CodeDiametre.Text, "CODE");
            //if (_leDiametre != null && !string.IsNullOrEmpty(_leDiametre.LIBELLE))
            //{
            //    this.Txt_LibelleDiametre.Text = _leDiametre.LIBELLE;
            //    LeDiametreBrtSelect = _leDiametre;
            //    //EnregisterDemande(LaDemande);
            //}

            this.Txt_CodeMateriel.Text = string.IsNullOrEmpty(_LeBrtDemande.NATBRT) ? string.Empty : _LeBrtDemande.NATBRT;
            CsMaterielBranchement _leMateriel = ClasseMEthodeGenerique.RetourneObjectFromList<CsMaterielBranchement>(LstDeMaterielBrt, this.Txt_CodeMateriel.Text, "CODE");
            if (_leMateriel != null && !string.IsNullOrEmpty(_leMateriel.LIBELLE))
            {
                this.Txt_LibelleMateriel.Text = _leMateriel.LIBELLE;
                LeDeMaterielBrtSelect = _leMateriel;
                //EnregisterDemande(LaDemande);
            }
            //this.Txt_LongueurBrt.Text = string.IsNullOrEmpty(_LeBrtDemande.LONGBRT.ToString()) ? InitValue.ToString() : _LeBrtDemande.LONGBRT.ToString();
            //this.Txt_NombrePoint.Text = _LeBrtDemande.NBPOINT.ToString();
            this.Txt_DateRacordement.Text = string.IsNullOrEmpty(_LeBrtDemande.DRAC.ToString()) ? string.Empty :Convert.ToDateTime( _LeBrtDemande.DRAC).ToShortDateString();
            this.Txt_DateDepose.Text = string.IsNullOrEmpty(_LeBrtDemande.DRES.ToString()) ? string.Empty : Convert.ToDateTime(_LeBrtDemande.DRES).ToShortDateString();
            this.Txt_Longitude.Text = string.IsNullOrEmpty(_LeBrtDemande.LONGITUDE) ? string.Empty : _LeBrtDemande.LONGITUDE;
            this.Txt_Latitude.Text = string.IsNullOrEmpty(_LeBrtDemande.LATITUDE) ? string.Empty : _LeBrtDemande.LATITUDE;
            this.Txt_AdresseElectrique.Text = string.IsNullOrEmpty(_LeBrtDemande.ADRESSERESEAU) ? string.Empty : _LeBrtDemande.ADRESSERESEAU;

            
            if (TypeDemande   == SessionObject.Enumere.FermetureBrt ||
                //TypeDemande == SessionObject.Enumere.Reabonnement ||
                TypeDemande == SessionObject.Enumere.ModificationBranchement  ||
                TypeDemande == SessionObject.Enumere.ReouvertureBrt )
                IsControleActif(false);
        }

        void AfficherBranchemetDemandeDevis(CsDevis _LeDemandeDevis)
        {
    
            //this.Txt_CodeDiametre.Text = string.IsNullOrEmpty(_LeDemandeDevis.LaDemandeDevis.DIAMETRE) ? string.Empty : _LeDemandeDevis.LaDemandeDevis.DIAMETRE;
            //this.Txt_CodeDiametre.Text = this.Txt_CodeDiametre.Text.PadLeft(SessionObject.Enumere.TailleDiametreBranchement, '0');
            //this.Txt_LongueurBrt.Text = string.IsNullOrEmpty(_LeDemandeDevis.LeDevis.DISTANCE.ToString()) ? InitValue.ToString() : Convert.ToDecimal(_LeDemandeDevis.LeDevis.DISTANCE.Value).ToString(SessionObject.FormatMontant);
            //this.Txt_NombrePoint.Text = _LeDemandeDevis.LeDevis.n.ToString();
            this.Txt_DateRacordement.Text = string.IsNullOrEmpty(_LeDemandeDevis.LeDevis.DATEPOSECTR.ToString()) ? string.Empty : Convert.ToDateTime(_LeDemandeDevis.LeDevis.DATEPOSECTR).ToShortDateString();
            //this.Txt_DateDepose.Text = string.IsNullOrEmpty(_LeBrtDemande.DRES.ToString()) ? string.Empty : _LeBrtDemande.DRES.ToString();
            this.Txt_Longitude.Text = string.IsNullOrEmpty(_LeDemandeDevis.LaDemandeDevis.LONGITUDE) ? string.Empty : _LeDemandeDevis.LaDemandeDevis.LONGITUDE;
            this.Txt_Latitude.Text = string.IsNullOrEmpty(_LeDemandeDevis.LaDemandeDevis.LATITUDE) ? string.Empty : _LeDemandeDevis.LaDemandeDevis.LATITUDE;
            this.Txt_AdresseElectrique.Text = string.IsNullOrEmpty(_LeDemandeDevis.LeDevis.NUMEROGPS) ? string.Empty : _LeDemandeDevis.LeDevis.NUMEROGPS;
        }
        private void btn_diametre_Click(object sender, RoutedEventArgs e)
        {
            this.btn_diametre.IsEnabled = false;
            if (LstDiametreBrt.Count != 0)
            {
                List<object> _LstObj = ClasseMEthodeGenerique.RetourneListeObjet(LstDiametreBrt);
                UcListeGenerique ctr = new UcListeGenerique(_LstObj,"CODE","LIBELLE",Langue.lbl_ListeDiametre );
                ctr.Closed += new EventHandler(galatee_OkClickedBtnDiametre);
                ctr.Show();
            }

        }
        void galatee_OkClickedBtnDiametre(object sender, EventArgs e)
        {

            try
            {
                this.btn_diametre.IsEnabled = true;
                UcListeGenerique ctrs = sender as UcListeGenerique;
                if (ctrs.GetisOkClick)
                {
                    //CsDiametreBranchement _LeDiametre = (CsDiametreBranchement)ctrs.MyObject;
                    //this.Txt_CodeDiametre.Text = _LeDiametre.CODE  ;
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Langue.lbl_Menu);
            }
        }
        private void btn_materiel_Click(object sender, RoutedEventArgs e)
        {
            this.btn_materiel .IsEnabled = false;
            if (LstDeMaterielBrt.Count != 0)
            {
                List<object> _LstObj = ClasseMEthodeGenerique.RetourneListeObjet(LstDeMaterielBrt);
                UcListeGenerique ctr = new UcListeGenerique(_LstObj, "CODE", "LIBELLE", Langue.lbl_ListeMateriel);
                ctr.Closed += new EventHandler(galatee_OkClickedBtnerMateriel);
                ctr.Show();
            }
        }
        void galatee_OkClickedBtnerMateriel(object sender, EventArgs e)
        {
            this.btn_materiel.IsEnabled = true;
            UcListeGenerique ctrs = sender as UcListeGenerique;
            if (ctrs.isOkClick)
            {
                CsMaterielBranchement _LeMateriel = (CsMaterielBranchement)ctrs.MyObject;
                this.Txt_CodeMateriel.Text = _LeMateriel.CODE ;
            }

        }
        public void EnregisterDemande(CsDemande _LaDemande)
        {

            _LaDemande.Branchement.NUMDEM = string.IsNullOrEmpty(_LaDemande.LaDemande.NUMDEM) ? string.Empty : _LaDemande.LaDemande.NUMDEM;
            _LaDemande.Branchement.CENTRE = string.IsNullOrEmpty(_LaDemande.LaDemande.CENTRE) ? string.Empty : _LaDemande.LaDemande.CENTRE;
            _LaDemande.Branchement.CLIENT = string.IsNullOrEmpty(_LaDemande.LaDemande.CLIENT) ? string.Empty : _LaDemande.LaDemande.CLIENT;
            _LaDemande.Branchement.PRODUIT = string.IsNullOrEmpty(_LaDemande.LaDemande.PRODUIT) ? string.Empty : _LaDemande.LaDemande.PRODUIT;

            _LaDemande.Branchement.FK_IDTYPEBRANCHEMENT  = LeDiametreBrtSelect.PK_ID;
            _LaDemande.Branchement.DIAMBRT = string.IsNullOrEmpty(this.Txt_CodeDiametre.Text) ? null : this.Txt_CodeDiametre.Text;

            _LaDemande.Branchement.NATBRT = string.IsNullOrEmpty(this.Txt_CodeMateriel.Text) ? "1" : this.Txt_CodeMateriel.Text;
            _LaDemande.Branchement.TYPECOMPTAGE = string.IsNullOrEmpty(this.Txt_CodeDiametre.Text) ? string.Empty : this.Txt_CodeDiametre.Text.Substring(3,1);
            _LaDemande.Branchement.CODEPOSTE = string.IsNullOrEmpty(this.Txt_CodePoste.Text) ? string.Empty : this.Txt_CodePoste.Text;
            _LaDemande.Branchement.ADRESSERESEAU = string.IsNullOrEmpty(this.Txt_AdresseElectrique.Text) ? null : this.Txt_AdresseElectrique.Text;
            _LaDemande.Branchement.LONGITUDE = string.IsNullOrEmpty(this.Txt_Longitude.Text) ? null : this.Txt_Longitude.Text;
            _LaDemande.Branchement.LATITUDE = string.IsNullOrEmpty(this.Txt_Latitude.Text) ? null : this.Txt_Latitude.Text; 

            _LaDemande.Branchement.TOURNEE = (LaDemande.Ag == null) ? string.Empty : (LaDemande.Ag.TOURNEE == null ? string.Empty : LaDemande.Ag.TOURNEE);
            _LaDemande.Branchement.FK_IDTOURNEE = (LaDemande.Ag == null) ? 0 : (LaDemande.Ag.FK_IDTOURNEE == 0 ? 0 : LaDemande.Ag.FK_IDTOURNEE.Value );
            _LaDemande.Branchement.ORDTOUR = (LaDemande.Ag == null) ? string.Empty : (LaDemande.Ag.ORDTOUR == null ? string.Empty : LaDemande.Ag.ORDTOUR);

            _LaDemande.Branchement.DRAC = null;
            _LaDemande.Branchement.USERCREATION = UserConnecte.matricule;
            _LaDemande.Branchement.USERMODIFICATION = UserConnecte.matricule;
            _LaDemande.Branchement.DATECREATION =  System.DateTime.Now;
            _LaDemande.Branchement.DATEMODIFICATION =  System.DateTime.Now;
            if (!string.IsNullOrEmpty(this.Txt_DateRacordement.Text))
                _LaDemande.Branchement.DRAC = DateTime.Parse(this.Txt_DateRacordement.Text);
            _LaDemande.Branchement.DRES = null;
            if (!string.IsNullOrEmpty(this.Txt_DateDepose.Text))
                _LaDemande.Branchement.DRES = DateTime.Parse(this.Txt_DateDepose.Text);
            if (_LaDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.BranchementAbonement ||
                _LaDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.BranchementSimple)
                _LaDemande.Branchement.SERVICE = SessionObject.Enumere.CompteurActifValeur;

            if (_LaDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.ReouvertureBrt)
                BranchementClientRecherche.DRES = null;
            if (_LaDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.FermetureBrt)
                _LaDemande.Branchement.SERVICE = SessionObject.Enumere.CompteurInactifValeur;



            //}
            //return returneValeur;
            //_LaDemande.
        }




        private void Txt_CodeDiametre_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                HandleLostFocus<CsTypeBranchement>((TextBox)sender, Txt_LibelleDiametre, LstDiametreBrt,SessionObject.Enumere.TailleDiametreBranchement );
            }
            catch (Exception ex)
            {
               Message.ShowError(ex.Message, Langue.lbl_Menu);
            }
        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            InitCtrl();
        }

        private void Txt_DateDepose_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (this.Txt_DateDepose.Text.Length == SessionObject.Enumere.TailleDate)
                if (ClasseMEthodeGenerique.IsDateValide(this.Txt_DateDepose.Text)==null)
                {
                    var w = new MessageBoxControl.MessageBoxChildWindow(Langue.lbl_Menu, "Date invalide", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    w.OnMessageBoxClosed += (_, result) =>
                    {
                        this.Txt_DateDepose.Focus();
                    };
                    w.Show();
                }
        }
        private void Txt_DateRacordement_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (this.Txt_DateRacordement.Text.Length == SessionObject.Enumere.TailleDate)
                if (ClasseMEthodeGenerique.IsDateValide(this.Txt_DateRacordement.Text)==null )
                {
                    var w = new MessageBoxControl.MessageBoxChildWindow(Langue.lbl_Menu, "Date invalide", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    w.OnMessageBoxClosed += (_, result) =>
                    {
                        this.Txt_DateRacordement .Focus();
                    };
                    w.Show();
                }
        }

        private void Txt_CodeDiametre_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (this.Txt_CodeDiametre.Text.Length == SessionObject.Enumere.TailleDiametreBranchement && (LstDiametreBrt != null && LstDiametreBrt.Count  != 0))
                {
                    //CsDiametreBranchement _leDiametre = ClasseMEthodeGenerique.RetourneObjectFromList<CsDiametreBranchement>(LstDiametreBrt, this.Txt_CodeDiametre.Text, "CODE");
                    //if (_leDiametre != null &&  !string.IsNullOrEmpty( _leDiametre.LIBELLE))
                    //{
                    //    this.Txt_LibelleDiametre.Text = _leDiametre.LIBELLE;
                    //    LeDiametreBrtSelect = _leDiametre;
                    //    if (TypeDemande !=SessionObject.Enumere.ModificationBranchement )
                    //    EnregisterDemande(LaDemande);
                    //}
                    //else
                    //{
                    //    var w = new MessageBoxControl.MessageBoxChildWindow(Langue.lbl_Menu, Langue.MsgEltInexistent, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    //    w.OnMessageBoxClosed += (_, result) =>
                    //    {
                    //        this.Txt_CodeDiametre.Focus();
                    //        this.Txt_CodeDiametre.Text = string.Empty;
                    //        this.Txt_LibelleDiametre.Text = string.Empty;
                    //    };
                    //    w.Show();
                    //}
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Langue.lbl_Menu);
            }

        }

        private void Txt_CodeMateriel_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (this.Txt_CodeMateriel.Text.Length == SessionObject.Enumere.TailleCodeMateriel && (LstDeMaterielBrt != null && LstDeMaterielBrt.Count != 0))
                {
                    CsMaterielBranchement _leMateriel = ClasseMEthodeGenerique.RetourneObjectFromList<CsMaterielBranchement>(LstDeMaterielBrt, this.Txt_CodeMateriel.Text, "CODE");
                    if (_leMateriel != null && !string.IsNullOrEmpty( _leMateriel.LIBELLE) )
                    {
                        this.Txt_LibelleMateriel.Text = _leMateriel.LIBELLE;
                        LeDeMaterielBrtSelect = _leMateriel;
                        if (TypeDemande != SessionObject.Enumere.ModificationBranchement)

                        EnregisterDemande(LaDemande);
                    }
                    else
                    {
                        var w = new MessageBoxControl.MessageBoxChildWindow(Langue.lbl_Menu, Langue.MsgEltInexistent, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        w.OnMessageBoxClosed += (_, result) =>
                        {
                            this.Txt_CodeMateriel.Focus();
                            this.Txt_CodeMateriel.Text = string.Empty;
                            this.Txt_LibelleMateriel.Text = string.Empty;
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

        private void Txt_CodeMateriel_LostFocus(object sender, RoutedEventArgs e)
        {
           
            try
            {
                HandleLostFocus<CsMaterielBranchement>((TextBox)sender, this.Txt_LibelleMateriel, SessionObject.LstDeMaterielBrt,1);
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
        private void HandleLostFocus<T>(TextBox Code, TextBox Libelle, List<T> listItems,int Taille)
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

        private void btn_depart_Click(object sender, RoutedEventArgs e)
        {

            this.btn_depart.IsEnabled = false;
            if (LstDepart != null && LstDepart.Count != 0)
            {
                List<object> _LstObj = ClasseMEthodeGenerique.RetourneListeObjet(LstDepart);
                Dictionary<string, string> _LstColonneAffich = new Dictionary<string, string>();
                _LstColonneAffich.Add("CODE", "CODE DEPART");
                _LstColonneAffich.Add("LIBELLE", "LIBELLE");
                List<object> obj = Shared.ClasseMEthodeGenerique.RetourneListeObjet(_LstObj);
                MainView.UcListeGenerique ctrl = new MainView.UcListeGenerique(obj, _LstColonneAffich, false, "Liste de départ");
                ctrl.Closed += new EventHandler(galatee_OkClickedbtn_depart);
                ctrl.Show();
            }
        }
        void galatee_OkClickedbtn_depart(object sender, EventArgs e)
        {

            this.btn_depart.IsEnabled = true;
            UcListeGenerique ctrs = sender as UcListeGenerique;
            if (ctrs.isOkClick)
            {
                CsDepart  _LeDepart = (CsDepart )ctrs.MyObject;
                if (_LeDepart != null)
                {
                    this.Txt_Depart.Text = _LeDepart.CODE.Substring(4, 2); ;
                    this.Txt_LibelleDepart.Text = _LeDepart.LIBELLE ;
                    this.Txt_NeoudFinal.Focus();
                }
            }
        }

        private void Txt_QuartierPoste_TextChanged(object sender, TextChangedEventArgs e)
        {
            GenereCodification();
        }

        private void Txt_SequenceNumPoste_TextChanged(object sender, TextChangedEventArgs e)
        {
            GenereCodification();
        }

        private void Txt_Depart_TextChanged(object sender, TextChangedEventArgs e)
        {
            GenereCodification();
            //if (this.Txt_Depart.Text.Length == SessionObject.Enumere.TailleCodeQuartier )
            //this.Txt_LibelleDepart.Text = ClasseMEthodeGenerique.RetourneLibelleQuartier (LstQuartier, Txt_Depart.Text);
        }

        private void Txt_NeoudFinal_TextChanged(object sender, TextChangedEventArgs e)
        {

            GenereCodification();

        }
        void GenereCodification()
        {
            if (!string.IsNullOrEmpty(this.Txt_QuartierPoste.Text) &&
                !string.IsNullOrEmpty(this.Txt_SequenceNumPoste.Text) &&
                !string.IsNullOrEmpty(this.Txt_Depart.Text) &&
                !string.IsNullOrEmpty(this.Txt_NeoudFinal.Text))
                this.Txt_AdresseElectrique.Text =
                    (this.Txt_QuartierPoste.Text +
                    this.Txt_SequenceNumPoste.Text + this.Txt_Depart.Text + this.Txt_NeoudFinal.Text);
            else
                this.Txt_AdresseElectrique.Text = string.Empty;
        }

        private void btn_QuartierPoste_Click_1(object sender, RoutedEventArgs e)
        {
            this.btn_QuartierPoste.IsEnabled = false;
            if (LstQuartierSite!= null && LstQuartierSite.Count != 0)
            {
                List<object> _LstObj = ClasseMEthodeGenerique.RetourneListeObjet(LstQuartierSite);
                Dictionary<string, string> _LstColonneAffich = new Dictionary<string, string>();
                _LstColonneAffich.Add("CODE", "CODE");
                _LstColonneAffich.Add("LIBELLE", "QUARTIER");
                List<object> obj = Shared.ClasseMEthodeGenerique.RetourneListeObjet(_LstObj);
                MainView.UcListeGenerique ctrl = new MainView.UcListeGenerique(obj, _LstColonneAffich, false,Langue.lbl_ListeDiametre);
                ctrl.Closed += new EventHandler(galatee_OkClickedBtnQuartier);
                ctrl.Show();
            }
        }
        void galatee_OkClickedBtnQuartier(object sender, EventArgs e)
        {
            this.btn_QuartierPoste.IsEnabled = true;
            UcListeGenerique ctrs = sender as UcListeGenerique;
            if (ctrs.isOkClick)
            {
                CsQuartier _LeQuartier = (CsQuartier)ctrs.MyObject;
                if (_LeQuartier != null)
                {
                    this.Txt_LibelleQuartier.Text = _LeQuartier.LIBELLE;
                    Txt_QuartierPoste.Text = _LeQuartier.CODE;
                }
            }
        }
        List<CsQuartier> LstQuartierSite = new List<CsQuartier>();
        List<CsDepart> LstDepart  = new List<CsDepart>();
        private void ChargeQuartier()
        {
            try
            {
                if (SessionObject.LstQuartier != null && SessionObject.LstQuartier.Count != 0)
                {
                    LstQuartierSite = SessionObject.LstQuartier.Where(t => t.COMMUNE  == LaDemande.Ag.COMMUNE ).ToList();
                }
                else
                {
                    AcceuilServiceClient service = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                    service.ChargerLesQartiersCompleted  += (s, args) =>
                    {
                        if (args != null && args.Cancelled)
                            return;
                        SessionObject.LstQuartier = args.Result;
                        LstQuartierSite = SessionObject.LstQuartier.Where(t => t.CENTRE == LaDemande.LaDemande.CENTRE).ToList();
                    };
                    service.ChargerLesQartiersAsync ();
                    service.CloseAsync();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        private void ChargeDeparts()
        {
            if (SessionObject.LsDesDepart != null && SessionObject.LsDesDepart.Count != 0)
            {
                LstDepart = SessionObject.LsDesDepart.Where(t => t.CENTRE == LaDemande.LaDemande.CENTRE && t.FK_IDCENTRE == LaDemande.LaDemande.FK_IDCENTRE).ToList();
                return;            
            }
            AcceuilServiceClient service = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
            service.ChargerDepartCompleted += (s, args) =>
            {
                if (args != null && args.Cancelled)
                    return;
                SessionObject.LsDesDepart = args.Result;
                LstDepart = SessionObject.LsDesDepart.Where(t => t.CENTRE == LaDemande.LaDemande.CENTRE && t.FK_IDCENTRE == LaDemande.LaDemande.FK_IDCENTRE).ToList();
            };
            service.ChargerDepartAsync();
            service.CloseAsync();
        }

        private void UserControl_LostFocus_1(object sender, RoutedEventArgs e)
        {
            EnregisterDemande(LaDemande);
        }

        private void Txt_DateRacordement_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(Txt_DateRacordement.Text))
            {
                if (ClasseMEthodeGenerique.IsDateValide (Txt_DateRacordement.Text)==null)
                {
                    Message.ShowInformation(Langue.MsgDateInvalide, Langue.lbl_Menu);
                    this.Txt_DateRacordement.Focus();
                    return;
                }
            }
        }

        private void Txt_DateDepose_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(Txt_DateDepose.Text))
            {
                if (ClasseMEthodeGenerique.IsDateValide(Txt_DateDepose.Text) == null)
                {
                    Message.ShowInformation(Langue.MsgDateInvalide, Langue.lbl_Menu);
                    this.Txt_DateDepose.Focus();
                    return;
                }
            }
        }

        private void btn_Poste_Click(object sender, RoutedEventArgs e)
        {
            this.btn_Poste.IsEnabled = false;
            if (LstDePosteElectrique.Count != 0)
            {
                List<object> _LstObj = ClasseMEthodeGenerique.RetourneListeObjet(LstDePosteElectrique );
                UcListeGenerique ctr = new UcListeGenerique(_LstObj, "CODE", "LIBELLE", Langue.lbl_ListePoste);
                ctr.Closed += new EventHandler(galatee_OkClickedBtnPoste);
                ctr.Show();
            }
        }

        private void galatee_OkClickedBtnPoste(object sender, EventArgs e)
        {
            this.btn_Poste.IsEnabled = true;
            UcListeGenerique ctrs = sender as UcListeGenerique;
            if (ctrs.isOkClick)
            {
                CsPosteElectrique _LstPoste = (CsPosteElectrique)ctrs.MyObject;
                this.Txt_CodePoste.Text = _LstPoste.CODE;
            }
        }

        private void Txt_CodePoste_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (this.Txt_CodePoste.Text.Length == SessionObject.Enumere.TailleCodeQuartier && (LstDePosteElectrique != null && LstDePosteElectrique.Count != 0))
            {
                CsPosteElectrique _lePoste = ClasseMEthodeGenerique.RetourneObjectFromList<CsPosteElectrique>(LstDePosteElectrique, this.Txt_CodePoste.Text, "CODE");
                if (_lePoste != null && !string.IsNullOrEmpty(_lePoste.LIBELLE))
                {
                    this.Txt_LibellePoste .Text = _lePoste.LIBELLE;
                    LePosteBrtSelect = _lePoste;
                    if (TypeDemande != SessionObject.Enumere.ModificationBranchement)
                        EnregisterDemande(LaDemande);
                }
                else
                {
                    var w = new MessageBoxControl.MessageBoxChildWindow(Langue.lbl_Menu, Langue.MsgEltInexistent, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    w.OnMessageBoxClosed += (_, result) =>
                    {
                        this.Txt_CodePoste .Focus();
                        this.Txt_CodePoste.Text = string.Empty;
                        this.Txt_LibellePoste .Text = string.Empty;
                    };
                    w.Show();
                }
            }
        }
    }
}
