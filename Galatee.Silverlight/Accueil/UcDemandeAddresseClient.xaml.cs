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
    public partial class UcDemandeAddresseClient : UserControl
    {
        #region Variable global
        List<CsCommune> LstCommuneCentre = new List<CsCommune>() ;
        CsCommune LaCommuneSelect = new CsCommune();

        List<CsQuartier> LstQuartierCommuneSelect = new List<CsQuartier>();
        CsQuartier LeQuartierSelect = new CsQuartier();

        List<CsSecteur> LstSecteurQuartierSelect = new List<CsSecteur>();
        CsSecteur LeSecteurSelect = new CsSecteur();


        List<CsRues> LstRuesAll = new List<CsRues>();
        List<CsRues> LstRuesSecteurSelect = new List<CsRues>();
        CsRues LaRueSelect = new CsRues();


        CsTournee LaTourneSelect = new CsTournee();

        CsDevis LeDevis = new CsDevis();
        CsAg _leAg = new CsAg();
        CsAbon _leAbon = new CsAbon();
        public  CsDemande LaDemande = new CsDemande();
        List<CsTournee> LstZone;
        public CsDemande MaDemande
        {
            get { return LaDemande; }
            set { LaDemande = value; }
        }
        bool IsUpdate = false;
        bool IsAbonExiste = false;

        string TypeDemande = string.Empty;
        #endregion

        public UcDemandeAddresseClient()
        {
            InitializeComponent();
            Initctrl();
        }
        public UcDemandeAddresseClient(CsDemande _LaDemande, bool _IsUpdate)
        {
            try
            {
                InitializeComponent();
                translate();
                IsUpdate = _IsUpdate;
                LaDemande = _LaDemande;
                TypeDemande = _LaDemande.LaDemande.TYPEDEMANDE ;

                if (LaDemande.Ag == null) LaDemande.Ag = new CsAg();

                ChargerLaListeDesCommunes();
                ChargeQuartier();
                ChargerTournee();
                ChargeSecteur();
                ChargeRue();
                this.Txt_NumDevis.MaxLength = SessionObject.Enumere.TailleNumDevis;

                if (IsUpdate)
                {
                    if (LaDemande.Ag != null) _leAg = LaDemande.Ag;
                    else _leAg = new CsAg();
                    AfficherInfoAdresse(_leAg);
                    LeDevis = LaDemande.LeDevis;
                    if (LeDevis != null && LeDevis.LeDevis != null && !string.IsNullOrEmpty( LeDevis.LeDevis.CODECENTRE))
                        this.Txt_NumDevis.Text =LeDevis.LeDevis == null ? string.Empty : LeDevis.LeDevis.NUMDEVIS ;
                    if (LeDevis != null && (_leAg == null && string.IsNullOrEmpty(_leAg.CENTRE)) )
                    {
                        if (LeDevis.LeDevis.DATEREGLEMENT != null && SessionObject.Enumere.IsReglementDevisPrisEnCompteAuGuichet)
                        {
                            if (LeDevis.LeDevis.CODEPRODUIT != LaDemande.LaDemande.PRODUIT)
                                Message.ShowError(Langue.Msg_Incoherence, Langue.lbl_Menu);
                            //else
                            //    Message.ShowError(Langue.Msg_DevisRegle, Langue.lbl_Menu);
                        }
                        else
                            RemplireInfoDevis(LeDevis.LaDemandeDevis);
                    }
                    //if (!string.IsNullOrEmpty(LaDemande.LaDemande.NUMDEVIS ))
                    //    this.Txt_NumDevis.Text = LaDemande.LaDemande.NUMDEVIS;
                    this.Txt_NumDevis.IsReadOnly = true;
                    this.Txt_Client .IsReadOnly = true;


                    
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message,Langue.lbl_Menu);
            }
        }
        private void translate()
        {
            //Gestion de la langue
            //SessionObject.cul = new System.Globalization.CultureInfo("en-US"); ;
            //SessionObject.res_man.GetString("btn_clear",SessionObject.cul);
            //string text = "";
            //text = SessionObject.res_man.GetString("btn_clear", System.Threading.Thread.CurrentThread.CurrentCulture );
            //this.lbl_adresse.Content = SessionObject.res_man.GetString("btn_clear", System.Threading.Thread.CurrentThread.CurrentUICulture);
            this.lbl_adresse.Content = Langue.lbl_adresse;
            this.lbl_Avance.Content = Langue.lbl_Avance;
            this.lbl_Commune.Content = Langue.lbl_Commune;
            this.lbl_Etage.Content = Langue.lbl_Etage;
            this.lbl_Lot.Content = Langue.lbl_Lot;
            this.lbl_NomProprietaire.Content = Langue.lbl_NomProprietaire;
            this.lbl_Numdevis.Content = Langue.lbl_Numdevis;
            this.lbl_NumRue.Content = Langue.lbl_NumRue;
            this.lbl_Ordre.Content = Langue.lbl_Ordre;
            this.lbl_Quartier.Content = Langue.lbl_Quartier;
            this.lbl_RegroupementCompteur.Content = Langue.lbl_RegroupementCompteur;
            this.lbl_Rue.Content = Langue.lbl_Rue;
            this.lbl_Secteur.Content = Langue.lbl_Secteur;
            this.lbl_Sequence.Content = Langue.lbl_Ordre;
            this.lbl_Telephone.Content = Langue.lbl_Telephone;
            this.lbl_Tournee.Content = Langue.lbl_Tournee;
            this.lbl_autresInfo.Content = Langue.lbl_autresInfo; 

        }
        private void Initctrl()
        {
            //
            this.lbl_Numdevis.Visibility = System.Windows.Visibility.Collapsed;
            this.Txt_NumDevis.Visibility = System.Windows.Visibility.Collapsed;
            this.Txt_DateAction.Visibility = System.Windows.Visibility.Collapsed;
            this.lbl_Action.Visibility = System.Windows.Visibility.Collapsed;

            this.Txt_Avance .Visibility = System.Windows.Visibility.Collapsed;
            this.lbl_Avance .Visibility = System.Windows.Visibility.Collapsed;

            this.Rdb_Existing.Visibility = System.Windows.Visibility.Collapsed;
            this.Rdb_New.Visibility = System.Windows.Visibility.Collapsed;

            this.Txt_Client.MaxLength = SessionObject.Enumere.TailleClient;
            this.Rdb_New.IsChecked = true;

            this.Txt_PeriodeAFacturer.Visibility = System.Windows.Visibility.Collapsed;
            this.label5.Visibility = System.Windows.Visibility.Collapsed;

            this.Txt_CodeCommune.MaxLength = SessionObject.Enumere.TailleCodeQuartier;

            if (TypeDemande == SessionObject.Enumere.ModificationAdresse)
                return;

            #region BRANCHEMENT SIMPLE || BRANCHEMENT ABONEMENT
            if (TypeDemande == SessionObject.Enumere.BranchementSimple
                   || TypeDemande == SessionObject.Enumere.BranchementAbonement)
            {
                if (SessionObject.Enumere.IsDevisPrisEnCompteAuGuichet)
                {
                    this.lbl_Numdevis.Visibility = System.Windows.Visibility.Visible;
                    this.Txt_NumDevis.Visibility = System.Windows.Visibility.Visible;
                    this.Rdb_Existing.Visibility = System.Windows.Visibility.Collapsed;
                    this.Rdb_New.Visibility = System.Windows.Visibility.Collapsed;
                    this.Txt_DateAction.Visibility = System.Windows.Visibility.Collapsed;
                    this.lbl_Action.Visibility = System.Windows.Visibility.Collapsed;
                    this.Txt_Avance.Visibility = System.Windows.Visibility.Collapsed;
                    this.lbl_Avance.Visibility = System.Windows.Visibility.Collapsed;
                    Txt_NumDevis.Focus();
                }
                else
                {
                    Txt_NumDevis.Visibility = System.Windows.Visibility.Collapsed;
                    this.lbl_Numdevis.Visibility = System.Windows.Visibility.Collapsed;
                    this.Rdb_Existing.Visibility = System.Windows.Visibility.Visible;
                    this.Rdb_New.Visibility = System.Windows.Visibility.Visible;
                    this.Rdb_New.IsChecked = true;
                    this.Txt_Client.Focus();
                }
                if (SessionObject.Enumere.IsRefClientSaisie)
                    ChargerLaPoliceDisponible();

            }
            #endregion
            #region REABONEMENT
            else if (TypeDemande == SessionObject.Enumere.Reabonnement)
            {
                this.Txt_Avance.Visibility = System.Windows.Visibility.Collapsed;
                this.Txt_DateAction.Visibility = System.Windows.Visibility.Collapsed;
                this.lbl_Action.Visibility = System.Windows.Visibility.Collapsed;
                this.lbl_Avance.Visibility = System.Windows.Visibility.Collapsed;

            }
            #endregion
            #region RESILIATION
            else if (TypeDemande == SessionObject.Enumere.Resiliation)
            {
                this.Txt_DateAction.Visibility = System.Windows.Visibility.Visible;
                this.lbl_Action.Visibility = System.Windows.Visibility.Visible;
                lbl_Action.Content = Langue.lbl_DateResiliation;
                this.Txt_Avance.Visibility = System.Windows.Visibility.Visible;
                this.lbl_Avance.Visibility = System.Windows.Visibility.Visible;
                if (!IsUpdate)
                    this.Txt_DateAction.Text = DateTime.Now.ToShortDateString();
            }
            #endregion
            #region FERMETURE DE BRANCHEMENT
            else if (TypeDemande == SessionObject.Enumere.FermetureBrt ||
                     TypeDemande == SessionObject.Enumere.DeposeCompteur)
            {
                this.lbl_Action.Visibility = System.Windows.Visibility.Visible;
                this.Txt_DateAction.Visibility = System.Windows.Visibility.Visible;
                this.lbl_Action.Content = "Date fermeture";
                if (!IsUpdate)
                    this.Txt_DateAction.Text = DateTime.Now.ToShortDateString();
            }
            #endregion
            #region REACTIVATION DE BRANCHEMENT
            else if (TypeDemande == SessionObject.Enumere.ReouvertureBrt)
            {
                this.lbl_Action.Content = "Date ouverture";
                this.lbl_Action.Visibility = System.Windows.Visibility.Visible;
                this.Txt_DateAction.Visibility = System.Windows.Visibility.Visible;
                if (!IsUpdate)
                    this.Txt_DateAction.Text = DateTime.Now.ToShortDateString();
            }
            #endregion
            #region FACTURE MANUELLE
            else if (TypeDemande == SessionObject.Enumere.FactureManuelle ||
                  TypeDemande == SessionObject.Enumere.AvoirConsomation)
            {
                this.Txt_PeriodeAFacturer.Visibility = System.Windows.Visibility.Collapsed;
                this.label5.Visibility = System.Windows.Visibility.Collapsed;
            }
            #endregion

            if (LaDemande.Ag != null) _leAg = LaDemande.Ag;
            else _leAg = new CsAg();
            AfficherInfoAdresse(_leAg);
        }
        private void VideControle()
        {
            this.Txt_CodeCommune.Text = this.Txt_LibelleCommune.Text = string.Empty;
            this.Txt_CodeQuartier .Text = this.Txt_LibelleQuartier.Text = string.Empty;
            this.Txt_CodeSecteur .Text = this.Txt_LibelleSecteur.Text = string.Empty;
            this.Txt_CodeNomRue .Text = this.Txt_NomRue .Text = string.Empty;
            this.Txt_CodeCommune.Text = this.Txt_LibelleCommune.Text = string.Empty;
            this.Txt_NomClient.Text = string.Empty;
            this.Txt_NumRue.Text = string.Empty;
            this.Txt_Etage.Text = string.Empty;
            this.Txt_Porte.Text = string.Empty;
            this.Txt_AutreInformation.Text = string.Empty;
            this.Txt_Tournee.Text = string.Empty;
            this.Txt_Telephone.Text = string.Empty;
            this.Txt_Email.Text = string.Empty;
            this.Txt_Fax.Text = string.Empty;
            this.Txt_OrdreTour .Text = string.Empty;
        }
        private void ChargerLaPoliceDisponible()
        {
            CsPoliceDispo _Lapolice = new CsPoliceDispo();
            AcceuilServiceClient service1 = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
            service1.RetournePoliceDisponibleCompleted += (s, args) =>
            {
                if (args.Error != null && args.Cancelled)
                    return;
                _Lapolice = args.Result;
                if (_Lapolice != null)
                {
                    this.Txt_Client.Text = _Lapolice.AG;
                    this.Txt_Client.IsReadOnly = true;
                }
            };
            service1.RetournePoliceDisponibleAsync(LaDemande.LaDemande.CENTRE);
            service1.CloseAsync();
        }

        //private void ChargerLaListeDesCommunes(string centre)
        //{
        //    try
        //    {
        //        LstCommuneAll = new List<CsCommune>();
        //        AcceuilServiceClient service = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
        //        service.ChargerLesBrancheDesCommuneCompleted += (s, args) =>
        //        {
        //            if (args.Error != null && args.Cancelled)
        //                return;
        //            LstCommuneAll.AddRange(args.Result);
        //            if (!string.IsNullOrEmpty(this.Txt_CodeCommune.Text) &&
        //                LstCommuneAll != null &&
        //                LstCommuneAll.Count != 0)
        //            {
        //                CsCommune _LaCommune = ClasseMEthodeGenerique.RetourneObjectFromList(LstCommuneAll, this.Txt_CodeCommune.Text, "CODE");
        //                if (_LaCommune != null && !string.IsNullOrEmpty(_LaCommune.LIBELLE))
        //                {
        //                    LaCommuneSelect = _LaCommune;
        //                    this.Txt_LibelleCommune.Text = _LaCommune.LIBELLE;
        //                }
        //            }
        //        };
        //        service.ChargerLesBrancheDesCommuneAsync(centre, null);
        //        service.CloseAsync();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //}
        //private void ChargeQuartier(string Centre, string Commune)
        //{
        //    try
        //    {
        //        LstQuartierAll = new List<CsQuartier>();
        //        AcceuilServiceClient service = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
        //        service.ChargerLesQartiersDesCommuneCompleted += (s, args) =>
        //        {
        //            if (args != null && args.Cancelled)
        //                return;
        //            LstQuartierAll.AddRange(args.Result);
        //            if (!string.IsNullOrEmpty(this.Txt_CodeQuartier.Text) &&
        //            LstQuartierAll != null &&
        //            LstQuartierAll.Count != 0)
        //            {
        //                CsQuartier _LeQuartie = ClasseMEthodeGenerique.RetourneObjectFromList(LstQuartierAll, this.Txt_CodeQuartier.Text, "CODE");
        //                if (_LeQuartie != null && !string.IsNullOrEmpty(_LeQuartie.LIBELLE))
        //                {
        //                    this.Txt_LibelleQuartier.Text = _LeQuartie.LIBELLE;
        //                    LeQuartierSelect = _LeQuartie;
        //                }
        //            }
        //        };
        //        service.ChargerLesQartiersDesCommuneAsync(string.Empty, string.Empty, string.Empty);
        //        service.CloseAsync();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //}
        //private void ChargeSecteur(string Quartier, string secteur)
        //{
        //    try
        //    {
        //        LstSecteurAll = new List<CsSecteur>();
        //        AcceuilServiceClient service = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
        //        service.ChargerLesSecteursDesQuartierCompleted += (s, args) =>
        //        {
        //            if (args != null && args.Cancelled)
        //                return;
        //            LstSecteurAll=args.Result;
        //            if (!string.IsNullOrEmpty(this.Txt_CodeSecteur .Text) &&
        //            LstSecteurAll != null &&
        //            LstSecteurAll.Count != 0)
        //            {
        //                CsSecteur _LeSecteur = ClasseMEthodeGenerique.RetourneObjectFromList(LstSecteurAll, this.Txt_CodeQuartier.Text, "CODE");
        //                if (_LeSecteur != null && !string.IsNullOrEmpty(_LeSecteur.LIBELLE))
        //                {
        //                    this.Txt_LibelleSecteur.Text = _LeSecteur.LIBELLE;
        //                    LeSecteurSelect = _LeSecteur;
        //                }
        //            }
        //        };
        //        service.ChargerLesSecteursDesQuartierAsync(LaDemande.LaDemande.CENTRE, string.Empty, string.Empty);
        //        service.CloseAsync();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        //private void ChargeRue(string Quartier, string secteur)
        //{
        //    try
        //    {
        //        LstRuesAll = new List<CsRues >();
        //        AcceuilServiceClient service = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
        //        service.ChargerLesRueDesSecteurCompleted += (s, args) =>
        //        {
        //            if (args != null && args.Cancelled)
        //                return;
        //            LstRuesAll.AddRange(args.Result);
        //            if (!string.IsNullOrEmpty(this.Txt_CodeNomRue .Text) &&
        //            LstRuesAll != null &&
        //            LstRuesAll.Count != 0)
        //            {
        //                CsRues _LaRue = ClasseMEthodeGenerique.RetourneObjectFromList(LstRuesAll, this.Txt_CodeNomRue.Text, "CODE");
        //                if (_LaRue != null && !string.IsNullOrEmpty(_LaRue.LIBELLE))
        //                {
        //                    this.Txt_NomRue.Text = _LaRue.LIBELLE;
        //                    LaRueSelect = _LaRue;
        //                }
        //            }
        //        };
        //        service.ChargerLesRueDesSecteurAsync(LaDemande.LaDemande.CENTRE, string.Empty, string.Empty);
        //        service.CloseAsync();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        //private void ChargerTournee(string leCentre)
        //{
        //    AcceuilServiceClient service = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
        //    service.ChargerLesTourneesDesSecteurCompleted += (s, args) =>
        //    {
        //        LstZone = new List<CsTournee>();
        //        if (args != null && args.Cancelled)
        //            return;
        //        LstZone.AddRange(args.Result);
        //        if (LstZone != null && LstZone.Count != 0 && !string.IsNullOrEmpty(this.Txt_Tournee.Text))
        //        { 
        //            CsTournee _Latourne = ClasseMEthodeGenerique.RetourneObjectFromList(LstZone,this.Txt_Tournee.Text, "IDTOURNEE");
        //            LaTourneSelect = _Latourne;
        //        }
        //    };
        //    service.ChargerLesTourneesDesSecteurAsync(leCentre, null);
        //    service.CloseAsync();
        //}

        void RemplireTextbox<T>(TextBox leTextbox, List<T> LstRef, string _valeurSelect, string _precherche,string pChampAffich) where T : new() 
        {


            T ObjetRetourne = new T();
            foreach (T item in LstRef)
            {
                // Recuperation des types
                System.Reflection.PropertyInfo[] properties1 = item.GetType().GetProperties();

                // Test de l'unicité des deux types
                // Remplacement des valeurs
                for (int attrNum = 0; attrNum < properties1.Length; attrNum++)
                {
                    if (properties1[attrNum].Name.Equals(_precherche))
                    {
                        object value2 = properties1[attrNum].GetValue(item, null);
                        if (value2.ToString() == _valeurSelect)
                        {
                            ObjetRetourne = item;
                            leTextbox.Text = RecupereLibelle(ObjetRetourne, pChampAffich);
                        }
                    }
                }
            }
        }

        string  RecupereLibelle<T>(T Object, string pChampAffich)
        {
            // Recuperation des types
            System.Reflection.PropertyInfo[] properties1 = Object.GetType().GetProperties();

            // Test de l'unicité des deux types
            // Remplacement des valeurs
            for (int attrNum = 0; attrNum < properties1.Length; attrNum++)
            {
                if (properties1[attrNum].Name.Equals(pChampAffich))
                {
                    object value2 = properties1[attrNum].GetValue(Object, null);
                    return value2.ToString();
                }
            }
            return string.Empty;
        }

        private void ChargerLaListeDesCommunes()
        {
            try
            {
                if (SessionObject.LstCommune != null && SessionObject.LstCommune.Count != 0)
                {
                    LstCommuneCentre = SessionObject.LstCommune.Where(t => t.CENTRE == LaDemande.LaDemande.CENTRE || t.CENTRE == "00000").ToList();
                    if (!string.IsNullOrEmpty(this.Txt_CodeCommune.Text))
                        RemplireTextbox<CsCommune>(this.Txt_LibelleCommune, LstCommuneCentre, this.Txt_CodeCommune.Text, "CODE", "LIBELLE");
                }
                else
                {
                    AcceuilServiceClient service = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                    service.ChargerCommuneCompleted  += (s, args) =>
                    {
                        if (args.Error != null && args.Cancelled)
                            return;
                        SessionObject.LstCommune = args.Result;
                        LstCommuneCentre = SessionObject.LstCommune.Where(t => t.CENTRE == LaDemande.LaDemande.CENTRE || t.CENTRE == "00000").ToList();
                        if (!string.IsNullOrEmpty(this.Txt_CodeCommune.Text))
                            RemplireTextbox<CsCommune>(this.Txt_LibelleCommune, LstCommuneCentre, this.Txt_CodeCommune.Text, "CODE", "LIBELLE");
                    };
                    service.ChargerCommuneAsync ();
                    service.CloseAsync();
                }
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
                if (SessionObject.LstQuartier != null && SessionObject.LstQuartier.Count != 0)
                {
                    if (!string.IsNullOrEmpty(this.Txt_CodeQuartier.Text))
                        RemplireTextbox<CsQuartier>(this.Txt_LibelleQuartier, SessionObject.LstQuartier, this.Txt_CodeQuartier.Text, "CODE", "LIBELLE");
                }
                else
                {
                    AcceuilServiceClient service = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                    service.ChargerLesQartiersCompleted += (s, args) =>
                    {
                        if (args != null && args.Cancelled)
                            return;
                        SessionObject.LstQuartier = args.Result;
                        if (!string.IsNullOrEmpty(this.Txt_CodeQuartier.Text))
                            RemplireTextbox<CsQuartier>(this.Txt_LibelleQuartier, SessionObject.LstQuartier, this.Txt_CodeQuartier.Text, "CODE", "LIBELLE");
                    };
                    service.ChargerLesQartiersAsync();
                    service.CloseAsync();
                }
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

                if (SessionObject.LstSecteur != null && SessionObject.LstSecteur.Count != 0)
                {
                    if (!string.IsNullOrEmpty(this.Txt_CodeCommune.Text))
                        RemplireTextbox<CsSecteur>(this.Txt_LibelleSecteur, SessionObject.LstSecteur, this.Txt_CodeSecteur.Text, "CODE", "LIBELLE");
                }
                else
                {

                    AcceuilServiceClient service = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                    service.ChargerLesSecteursCompleted += (s, args) =>
                    {
                        if (args != null && args.Cancelled)
                            return;
                        SessionObject.LstSecteur = args.Result;
                    };
                    service.ChargerLesSecteursAsync();
                    service.CloseAsync();
                }
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

                AcceuilServiceClient service = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                service.ChargerLesRueDesSecteurCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    SessionObject.LstRues = args.Result;
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
            AcceuilServiceClient service = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
            service.ChargerLesTourneesCompleted  += (s, args) =>
            {
                if (args != null && args.Cancelled)
                    return;
                SessionObject.LstZone = args.Result;
            };
            service.ChargerLesTourneesAsync ();
            service.CloseAsync();
        }

        private void RetourneOrdre(int fk_idcentre, string centre, string client, string produit, string tdem)
        {
            string OrdreMax = string.Empty;
            AcceuilServiceClient service = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
            service.RetourneOrdreMaxCompleted += (s, args) =>
            {
                if (args != null && args.Cancelled)
                    return;
                OrdreMax = args.Result;
                if (!string.IsNullOrEmpty(OrdreMax))
                {
                    if (tdem == SessionObject.Enumere.BranchementAbonement ||
                        tdem == SessionObject.Enumere.BranchementSimple ||
                        //tdem == SessionObject.Enumere.AbonnementSeul ||
                        tdem == SessionObject.Enumere.FermetureBrt)
                    {
                        string LeMessage = string.Empty;
                        //if (tdem == SessionObject.Enumere.AbonnementSeul)
                        //    LeMessage = Langue.MsgAbonnementExist;
                        //else
                            LeMessage = SessionObject.Enumere.MessageBranchementInexistent;

                        IsControleActif(false);
                        Message.ShowInformation(LeMessage,Langue.lbl_Menu);
                    }
                    else if (tdem == SessionObject.Enumere.ChangementCompteur ||
                        tdem == SessionObject.Enumere.PoseCompteur ||
                        tdem == SessionObject.Enumere.AugmentationPuissance ||
                        tdem == SessionObject.Enumere.DimunitionPuissance  ||
                        tdem == SessionObject.Enumere.DeposeCompteur)
                    {
                        RetourneInfoAddresse(fk_idcentre,centre, client, OrdreMax, tdem);
                    }
                    else if (tdem == SessionObject.Enumere.Reabonnement ||
                             tdem == SessionObject.Enumere.FactureManuelle ||
                        tdem == SessionObject.Enumere.AvoirConsomation ||
                        tdem == SessionObject.Enumere.Resiliation ||
                        tdem == SessionObject.Enumere.AbonnementSeul )
                        RetourneInfoAbon(fk_idcentre,centre, client, OrdreMax, produit, tdem);

                }
                else
                {
                    if (tdem == SessionObject.Enumere.BranchementAbonement ||
                        tdem == SessionObject.Enumere.BranchementSimple)
                        RetourneInfoAddresse(fk_idcentre,centre, client, OrdreMax, tdem);
                    else if (tdem == SessionObject.Enumere.FermetureBrt ||
                             tdem == SessionObject.Enumere.AbonnementSeul ||
                             tdem == SessionObject.Enumere.ReouvertureBrt)
                    {
                        this.Txt_Ordre.Text = "01";
                        RetourneInfoBrt(fk_idcentre,centre, client, "01", produit, tdem);
                    }
                    else if (tdem == SessionObject.Enumere.Reabonnement ||
                             tdem == SessionObject.Enumere.FactureManuelle ||
                             tdem == SessionObject.Enumere.AvoirConsomation ||
                             tdem == SessionObject.Enumere.Resiliation ||
                             tdem == SessionObject.Enumere.AbonnementSeul ||
                             tdem == SessionObject.Enumere.ChangementCompteur ||
                             tdem == SessionObject.Enumere.PoseCompteur ||
                             tdem == SessionObject.Enumere.DeposeCompteur)
                    {
                        Message.ShowInformation("Aucune information ne correspond à vos critère ", "Accueil");
                        return;
                    }
                }

            };
            service.RetourneOrdreMaxAsync(centre, client, produit);
            service.CloseAsync();
        }
        private void RetourneInfoAbon(int fk_idcentre, string centre, string client, string ordre, string produit, string tdem)
        {
            CsAbon AbonementRecherche = new CsAbon();
            AcceuilServiceClient service = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
            service.RetourneAbonCompleted += (s, args) =>
            {
                if (args != null && args.Cancelled)
                    return;
                if( AbonementRecherche ==null) return ;
                AbonementRecherche = args.Result.FirstOrDefault(p => p.PRODUIT == produit);
                if (tdem == SessionObject.Enumere.AbonnementSeul)
                {
                    if ( AbonementRecherche != null)
                    {
                        if (AbonementRecherche.DRES != null)
                        {
                            IsAbonExiste = true;
                            RetourneInfoAddresse(fk_idcentre,centre, client, ordre, tdem);
                        }
                        else
                        {

                            SessionObject.EtatControlCourant = false;
                            IsControleActif(false);
                            Message.ShowError(Langue.MsgAbonnementExist, Langue.lbl_Menu);
                        }
                    }
                    else
                    {
                        this.Txt_Ordre.Text = ordre;
                        RetourneInfoAddresse(fk_idcentre,centre, client, ordre, tdem);
                    }
                }
                else if (tdem == SessionObject.Enumere.BranchementSimple ||
                         tdem == SessionObject.Enumere.BranchementAbonement)
                {
                    this.Txt_Ordre.Text = "01";
                    RetourneInfoAddresse(fk_idcentre,centre, client, ordre, tdem);
                }
                else if (tdem == SessionObject.Enumere.Reabonnement)
                {
                    if (AbonementRecherche.DRES == null)
                    {
                        var w = new MessageBoxControl.MessageBoxChildWindow(Langue.lbl_Menu, Langue.MsgAbonnementExist, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        w.OnMessageBoxClosed += (_, result) =>
                        {
                        };
                        w.Show();
                    }
                    else
                    {
                        RetourneInfoAddresse(fk_idcentre,centre, client, ordre, tdem);
                    }
                }
                else if (tdem == SessionObject.Enumere.FactureManuelle ||
                    tdem == SessionObject.Enumere.AvoirConsomation ||
                         tdem == SessionObject.Enumere.Resiliation)
                {
                    if (AbonementRecherche.DRES == null)
                    {
                        this.Txt_Avance.Text =Convert.ToDecimal( AbonementRecherche.AVANCE).ToString(SessionObject.FormatMontant );
                        RetourneInfoAddresse(fk_idcentre,centre, client, ordre, tdem);
                    }
                    else
                    {
                        var w = new MessageBoxControl.MessageBoxChildWindow(Langue.lbl_Menu, Langue.MsgAbonneResilié, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        w.OnMessageBoxClosed += (_, result) =>
                        {
                        };
                        w.Show();

                    }
                }

            };
            service.RetourneAbonAsync(fk_idcentre,centre, client, ordre);
            service.CloseAsync();
        }
        private void RetourneInfoBrt(int fk_idcentre, string centre, string client, string ordre, string produit, string tdem)
        {
            List<CsBrt> brtRecherche = new List<CsBrt>();
            CsBrt LebrtRecherche = new CsBrt();
            AcceuilServiceClient service = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
            service.RetourneBranchementCompleted += (s, args) =>
            {
                if (args != null && args.Cancelled)
                    return;
                brtRecherche = args.Result;
                if (brtRecherche!= null )
                LebrtRecherche = brtRecherche.FirstOrDefault(p=>p.PRODUIT == produit);
                if (LebrtRecherche != null && !string.IsNullOrEmpty(LebrtRecherche.PRODUIT))
                {
                    LaDemande.Branchement = LebrtRecherche;
                    if (tdem == SessionObject.Enumere.BranchementSimple ||
                        tdem == SessionObject.Enumere.BranchementAbonement)
                    {
                        IsControleActif(false);
                        Message.ShowError(Langue.MsgExistBrt, Langue.lbl_Menu);
                        SessionObject.EtatControlCourant = false;
                    }
                    else if (tdem == SessionObject.Enumere.FermetureBrt ||
                             tdem == SessionObject.Enumere.AbonnementSeul ||
                             tdem == SessionObject.Enumere.ReouvertureBrt)
                    {
                        if (tdem == SessionObject.Enumere.FermetureBrt && 
                            LaDemande.Branchement.DRES != null)
                            Message.ShowError(Langue.Msg_BranchementDejaFerme, Langue.lbl_Menu);
                       else if (tdem == SessionObject.Enumere.ReouvertureBrt  && 
                            LaDemande.Branchement.DRES == null)
                            Message.ShowError(Langue.Msg_BranchementNonFerme , Langue.lbl_Menu);
                        else
                            RetourneInfoAddresse(fk_idcentre,centre, client, ordre, tdem);
                    }
                }
                else
                {
                    if (tdem == SessionObject.Enumere.BranchementSimple ||
                        tdem == SessionObject.Enumere.BranchementAbonement)
                    {
                        IsControleActif(true);
                        this.Txt_Ordre.Text = "01";
                    }
                    if (tdem == SessionObject.Enumere.FermetureBrt ||
                         tdem == SessionObject.Enumere.AbonnementSeul)
                    {
                        IsControleActif(false);
                        var w = new MessageBoxControl.MessageBoxChildWindow(Langue.lbl_Menu, Langue.MsgInextBrt, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        w.OnMessageBoxClosed += (_, result) =>
                        {
                        };
                        w.Show();
                    }
                }
            };
            service.RetourneBranchementAsync(fk_idcentre,centre, client, produit);
            service.CloseAsync();
        }
        private void RetourneInfoClient(int fk_idcentre,string centre, string client, string ordre)
        {
            CsClient ClientRecherche = new CsClient();
            AcceuilServiceClient service = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
            service.RetourneClientCompleted += (s, args) =>
            {
                if (args != null && args.Cancelled)
                    return;
                ClientRecherche = args.Result;
            };
            service.RetourneClientAsync(fk_idcentre,centre, client, ordre);
            service.CloseAsync();
        }
        private void RetourneInfoCanalisation(int fk_idcentre, string centre, string client, string produit, int? point)
        {
            List<CsCanalisation> LstCanalisationRecherche = new List<CsCanalisation>();
            AcceuilServiceClient service = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
            service.RetourneCanalisationCompleted += (s, args) =>
            {
                if (args != null && args.Cancelled)
                    return;
                LstCanalisationRecherche = args.Result;
            };
            service.RetourneCanalisationAsync(fk_idcentre,centre, client, produit, point);
            service.CloseAsync();
        }
        private void RechercheDevis(string NumDevis)
        {
            //AcceuilServiceClient service = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
            //service.RetourneDevisCompleted += (s, args) =>
            //{
            //    if (args != null && args.Cancelled)
            //        return;
            //    LeDevis = args.Result;
            //    if (LeDevis != null)
            //    {
            //        LaDemande.LeDevis = LeDevis;
            //        if (LeDevis.LeDevis.DATEREGLEMENT!=null && SessionObject.Enumere.IsReglementDevisPrisEnCompteAuGuichet  )
            //        {
            //            if (LeDevis.LeDevis.CODEPRODUIT    != LaDemande.LaDemande.PRODUIT)
            //                Message.ShowError(Langue.Msg_Incoherence,Langue.lbl_Menu);
            //            else
            //                Message.ShowError(Langue.Msg_DevisRegle, Langue.lbl_Menu);
            //        }
            //        else if (LeDevis.LeDevis.CODE != LaDemande.LaDemande.CENTRE && LeDevis.LeDevis.FK_IDCENTRE  != LaDemande.LaDemande.FK_IDCENTRE  && SessionObject.Enumere.IsReglementDevisPrisEnCompteAuGuichet)
            //        {
            //                Message.ShowError("Ce devis n'est pas pour ce centre", Langue.lbl_Menu);
            //        }
            //        else
            //            RemplireInfoDevis(LeDevis.LaDemandeDevis );
            //    }
            //};
            //service.RetourneDevisAsync(NumDevis);
            //service.CloseAsync();

        }
        private void RetourneInfoAddresse(int fk_idcentre,string centre, string client, string ordre, string tdem)
        {
            try
            {
                CsAg AdresseRechercher = new CsAg();
                AcceuilServiceClient service = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                service.RetourneAdresseCompleted += (s, args) =>
                {
                    if (args.Error != null && args.Cancelled)
                        return;
                    AdresseRechercher = args.Result;
                    if (string.IsNullOrEmpty(AdresseRechercher.CENTRE))
                    {
                        if (tdem == SessionObject.Enumere.AbonnementSeul)
                        {
                            IsControleActif(false);
                            var w = new MessageBoxControl.MessageBoxChildWindow(Langue.lbl_Menu, Langue.MsgInextBrt, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            w.OnMessageBoxClosed += (_, result) =>
                            {
                            };
                            w.Show();
                        }
                        else if (tdem == SessionObject.Enumere.BranchementSimple ||
                                 tdem == SessionObject.Enumere.BranchementAbonement)
                        {
                            IsControleActif(true);
                        }
                    }
                    else
                    {
                        LaDemande.Ag = AdresseRechercher;
                        if (tdem == SessionObject.Enumere.BranchementSimple ||
                            tdem == SessionObject.Enumere.BranchementAbonement ||
                            tdem == SessionObject.Enumere.AbonnementSeul ||
                            tdem == SessionObject.Enumere.Resiliation ||
                            tdem == SessionObject.Enumere.Reabonnement ||
                            tdem == SessionObject.Enumere.ChangementCompteur ||
                            tdem == SessionObject.Enumere.PoseCompteur ||
                            tdem == SessionObject.Enumere.DeposeCompteur ||
                            tdem == SessionObject.Enumere.FactureManuelle ||
                            tdem == SessionObject.Enumere.DimunitionPuissance ||
                            tdem == SessionObject.Enumere.AugmentationPuissance  ||
                            tdem == SessionObject.Enumere.AvoirConsomation)
                        {
                            if (tdem == SessionObject.Enumere.BranchementSimple ||
                            tdem == SessionObject.Enumere.BranchementAbonement ||
                            (tdem == SessionObject.Enumere.AbonnementSeul && !IsAbonExiste))
                                this.Txt_Ordre.Text = "01";
                            else
                            {
                                if (tdem == SessionObject.Enumere.AbonnementSeul && IsAbonExiste)
                                    ordre = (int.Parse(ordre )+ 1).ToString("00");
                                this.Txt_Ordre.Text = ordre;
                            }
                            LaDemande.LaDemande.ORDRE = this.Txt_Ordre.Text;
                            AfficherInfoAdresse(AdresseRechercher);
                        }

                        if (tdem == SessionObject.Enumere.Reabonnement ||
                            tdem == SessionObject.Enumere.FermetureBrt ||
                            tdem == SessionObject.Enumere.ReouvertureBrt )
                        {

                            //this.Txt_Ordre.Text = AdresseRechercher.ORDRE;
                            AfficherInfoAdresse(AdresseRechercher);
                        }
                    }
                };
                service.RetourneAdresseAsync(fk_idcentre , centre, client, ordre);
                service.CloseAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        private void AfficherInfoAdresse(CsAg AdresseDemande)
        {
            try
            {

             
                    this.Txt_Client.Text = string.IsNullOrEmpty(LaDemande.LaDemande.CLIENT) ? string.Empty : LaDemande.LaDemande.CLIENT;
                    this.Txt_Ordre.Text = string.IsNullOrEmpty(LaDemande.LaDemande.ORDRE) ? string.Empty : LaDemande.LaDemande.ORDRE;
               
                this.Txt_NomClient.Text = string.IsNullOrEmpty(AdresseDemande.NOMP) ? string.Empty : AdresseDemande.NOMP;
                this.Txt_CodeCommune.Text = string.IsNullOrEmpty(AdresseDemande.COMMUNE) ? string.Empty : AdresseDemande.COMMUNE;
                if (SessionObject.LstCommune != null && SessionObject.LstCommune.Count != 0 && !string.IsNullOrEmpty(AdresseDemande.COMMUNE))
                {
                    CsCommune _LAcommune = ClasseMEthodeGenerique.RetourneObjectFromList(SessionObject.LstCommune, AdresseDemande.COMMUNE, "CODE");
                    if (!string.IsNullOrEmpty(_LAcommune.LIBELLE))
                        this.Txt_LibelleCommune.Text = _LAcommune.LIBELLE;
                }

                
                this.Txt_CodeQuartier.Text = string.IsNullOrEmpty(AdresseDemande.QUARTIER) ? string.Empty : AdresseDemande.QUARTIER;
                if (SessionObject.LstQuartier != null && SessionObject.LstQuartier.Count != 0 && !string.IsNullOrEmpty(AdresseDemande.QUARTIER))
                {
                    CsQuartier _LeQuartier = ClasseMEthodeGenerique.RetourneObjectFromList(SessionObject.LstQuartier, AdresseDemande.QUARTIER, "CODE");
                    if (!string.IsNullOrEmpty(_LeQuartier.LIBELLE))
                        this.Txt_LibelleQuartier  .Text = _LeQuartier.LIBELLE;
                }
                this.Txt_CodeSecteur.Text = string.IsNullOrEmpty(AdresseDemande.SECTEUR) ? string.Empty : AdresseDemande.SECTEUR;
                if (SessionObject.LstSecteur != null && SessionObject.LstSecteur.Count != 0 && !string.IsNullOrEmpty(AdresseDemande.SECTEUR))
                {
                    CsSecteur _LeSecteur = ClasseMEthodeGenerique.RetourneObjectFromList(SessionObject.LstSecteur, AdresseDemande.SECTEUR, "CODE");
                    if (!string.IsNullOrEmpty(_LeSecteur.LIBELLE))
                        this.Txt_LibelleSecteur .Text = _LeSecteur.LIBELLE;
                }
                this.Txt_CodeNomRue .Text = string.IsNullOrEmpty(AdresseDemande.RUE) ? string.Empty : AdresseDemande.RUE;
                if (SessionObject.LstRues != null && SessionObject.LstRues.Count != 0 && !string.IsNullOrEmpty(AdresseDemande.RUE))
                {
                    CsRues _LaRue = ClasseMEthodeGenerique.RetourneObjectFromList(SessionObject.LstRues, AdresseDemande.RUE, "CODE");
                    if (!string.IsNullOrEmpty(_LaRue.LIBELLE ))
                        this.Txt_NomRue .Text = _LaRue.LIBELLE;
                }

                if(LaDemande.Branchement != null)
                this.Txt_DateAction.Text = (LaDemande.Branchement.DRAC == null) ? string.Empty : Convert.ToDateTime( LaDemande.Branchement.DRAC.ToString()).ToShortDateString();


                this.Txt_Etage.Text = string.IsNullOrEmpty(AdresseDemande.ETAGE) ? string.Empty : AdresseDemande.ETAGE;
                this.Txt_Email.Text = string.IsNullOrEmpty(AdresseDemande.EMAIL) ? string.Empty : AdresseDemande.EMAIL;
                this.Txt_Telephone.Text = string.IsNullOrEmpty(AdresseDemande.TELEPHONE) ? string.Empty : AdresseDemande.TELEPHONE;
                this.Txt_Fax.Text = string.IsNullOrEmpty(AdresseDemande.FAX) ? string.Empty : AdresseDemande.FAX;
                this.Txt_OrdreTour.Text = string.IsNullOrEmpty(AdresseDemande.ORDTOUR) ? string.Empty : AdresseDemande.ORDTOUR;
                this.Txt_Tournee.Text = string.IsNullOrEmpty(AdresseDemande.TOURNEE) ? string.Empty : AdresseDemande.TOURNEE;
                //this.Txt_Avance.Text = decimal.Parse(LaDemande.Abon_AVANCE.ToString()).ToString("N2");
                if (TypeDemande == SessionObject.Enumere.Resiliation ||
                    TypeDemande == SessionObject.Enumere.AbonnementSeul ||
                    TypeDemande == SessionObject.Enumere.FermetureBrt ||
                    TypeDemande == SessionObject.Enumere.FactureManuelle ||
                    TypeDemande == SessionObject.Enumere.AvoirConsomation ||
                    TypeDemande == SessionObject.Enumere.ChangementCompteur ||
                    TypeDemande == SessionObject.Enumere.PoseCompteur ||
                    TypeDemande == SessionObject.Enumere.DeposeCompteur ||
                    TypeDemande == SessionObject.Enumere.ModificationAdresse ||
                    TypeDemande == SessionObject.Enumere.Reabonnement)
                    IsControleActif(false);
                else
                    IsControleActif(true);

                //if (IsUpdate)
                //{
                //    this.Txt_Client.IsEnabled = false;
                //    this.Txt_Ordre.IsEnabled = false;
                //}
            }
            catch (Exception ex)
            {

                throw ex;
            }


        }
        private void RemplireInfoDevis(ObjDEMANDEDEVIS  _LeDemandeDevis)
        {
            try
            {
                //new FrmDemande().Txt_CodeCentre.Text = string.IsNullOrEmpty(_LeDemandeDevis.CENTRE) ? string.Empty : _LeDemandeDevis.CENTRE;
                this.Txt_NumDevis.Text = string.IsNullOrEmpty(_LeDemandeDevis.NUMDEVIS) ? string.Empty : _LeDemandeDevis.NUMDEVIS;
                this.Txt_Client.Text = string.IsNullOrEmpty(_LeDemandeDevis.CLIENT) ? string.Empty : _LeDemandeDevis.CLIENT;
                this.Txt_Ordre.Text = string.IsNullOrEmpty(_LeDemandeDevis.ORDRECLIENT) ? string.Empty : _LeDemandeDevis.ORDRECLIENT;
                this.Txt_NomClient.Text = string.IsNullOrEmpty(_LeDemandeDevis.NOM) ? string.Empty : _LeDemandeDevis.NOM;
                this.Txt_CodeCommune.Text = string.IsNullOrEmpty(_LeDemandeDevis.COMMUNE) ? string.Empty : _LeDemandeDevis.COMMUNE;
                this.Txt_CodeQuartier.Text = string.IsNullOrEmpty(_LeDemandeDevis.QUARTIER) ? string.Empty : _LeDemandeDevis.QUARTIER;
                if (SessionObject.LstCommune != null && SessionObject.LstCommune.Count != 0 && !string.IsNullOrEmpty(_LeDemandeDevis.COMMUNE))
                {
                    //CsCommune _LAcommune = ClasseMEthodeGenerique.RetourneObjectFromList(SessionObject.LstCommune, _LeDemandeDevis.COMMUNE, "COMMUNE");
                    CsCommune _LAcommune = SessionObject.LstCommune.FirstOrDefault(c => c.CODE == _LeDemandeDevis.COMMUNE);
                    if (!string.IsNullOrEmpty(_LAcommune.LIBELLE))
                    {
                        this.Txt_LibelleCommune.Text = _LAcommune.LIBELLE;
                        this.Txt_CodeCommune.Tag = _LAcommune.PK_ID;

                    }
                }
                if (SessionObject.LstQuartier != null && SessionObject.LstQuartier.Count != 0 && !string.IsNullOrEmpty(_LeDemandeDevis.QUARTIER))
                {
                    //CsQuartier _LeQuartier = ClasseMEthodeGenerique.RetourneObjectFromList(SessionObject.LstQuartier, _LeDemandeDevis.QUARTIER, "QUARTIER");
                    CsQuartier _LeQuartier = SessionObject.LstQuartier.FirstOrDefault(q => q.CODE == _LeDemandeDevis.QUARTIER);
                    if (!string.IsNullOrEmpty(_LeQuartier.LIBELLE))
                    {
                        this.Txt_LibelleQuartier.Text = _LeQuartier.LIBELLE;
                        this.Txt_CodeQuartier.Tag = _LeQuartier.PK_ID;
                    }
                }
                this.Txt_CodeSecteur.Text = string.IsNullOrEmpty(_LeDemandeDevis.SECTEUR) ? string.Empty : _LeDemandeDevis.SECTEUR;
                this.Txt_CodeNomRue.Text = string.IsNullOrEmpty(_LeDemandeDevis.RUE) ? string.Empty : _LeDemandeDevis.RUE;
                //if (string.IsNullOrEmpty(this.Txt_NumRue.Text))
                //{ 
                //    if (SessionObject.LstRues != null && SessionObject.LstRues.Count != 0)
                //        this.Txt_CodeNomRue.Tag = SessionObject.LstRues.FirstOrDefault(t => t.CODE == "00000").PK_ID; 
                //}
                this.Txt_Telephone.Text = string.IsNullOrEmpty(_LeDemandeDevis.NUMTEL) ? string.Empty : _LeDemandeDevis.NUMTEL;
                this.Txt_OrdreTour.Text = string.IsNullOrEmpty(_LeDemandeDevis.ORDTOUR) ? string.Empty : _LeDemandeDevis.ORDTOUR;
                this.Txt_Tournee.Text = string.IsNullOrEmpty(_LeDemandeDevis.TOURNEE) ? string.Empty : _LeDemandeDevis.TOURNEE;
                if (SessionObject.LstZone != null && SessionObject.LstZone.Count != 0 && !string.IsNullOrEmpty(_LeDemandeDevis.TOURNEE ))
                {
                    //CsQuartier _LeQuartier = ClasseMEthodeGenerique.RetourneObjectFromList(SessionObject.LstQuartier, _LeDemandeDevis.QUARTIER, "QUARTIER");
                    CsTournee _LaTournee = SessionObject.LstZone.FirstOrDefault(q => q.CODE == _LeDemandeDevis.TOURNEE);
                    if (!string.IsNullOrEmpty(_LaTournee.LIBELLE))
                        this.Txt_Tournee.Tag = _LaTournee.PK_ID;
                }

                IsControleActif(true);
            }
            catch (Exception ex)
            {

                throw ex;
            }


        }
        private void IsControleActif(bool Etat)
        {
            this.btn_Commune.IsEnabled = Etat;
            this.btn_Quartier.IsEnabled = Etat;
            this.btn_Secteur.IsEnabled = Etat;
            this.btn_Rue.IsEnabled = Etat;
            this.btn_zone.IsEnabled = Etat;
            this.btn_regroupementcpt.IsEnabled = Etat;

            this.Txt_NomClient.IsReadOnly = !Etat;
            this.Txt_Avance.IsReadOnly = !Etat;
            this.Txt_Porte.IsReadOnly = !Etat;
            this.Txt_CodeCommune.IsReadOnly = !Etat;
            //this.Txt_LibelleCommune.IsReadOnly = !Etat;
            this.Txt_CodeQuartier.IsReadOnly = !Etat;
            //this.Txt_LibelleQuartier.IsReadOnly = !Etat;
            this.Txt_CodeSecteur .IsReadOnly = !Etat;
            //this.Txt_LibelleSecteur.IsReadOnly = !Etat;
            this.Txt_CodeNomRue.IsReadOnly = !Etat;
            this.Txt_Etage.IsReadOnly = !Etat;
            this.Txt_Fax.IsReadOnly = !Etat;
            this.Txt_AutreInformation.IsReadOnly = !Etat;
            this.Txt_Partiel.IsReadOnly = !Etat;

            this.Txt_NomRue.IsReadOnly = !Etat;
            this.Txt_NumRue.IsReadOnly = !Etat;
            this.Txt_CodePostale.IsReadOnly = !Etat;
            this.Txt_Telephone.IsReadOnly = !Etat;
            this.Txt_OrdreTour.IsReadOnly = !Etat;
            this.Txt_Email.IsReadOnly = !Etat;
            this.Txt_Tournee.IsReadOnly = !Etat;
            this.Txt_CodeGroupementCompteut.IsReadOnly = !Etat;
            this.Txt_LibelleGroupementCompteur.IsReadOnly = !Etat;
            if (TypeDemande == SessionObject.Enumere.Resiliation ||
                TypeDemande == SessionObject.Enumere.FermetureBrt)
                this.Txt_DateAction.Focus();


        }
        private void VerifieExisteDemande(string centre, string client, string Ordre, int  idCentre, string tdem)
        {

            if (!string.IsNullOrEmpty(Txt_Client.Text) && Txt_Client.Text.Length == SessionObject.Enumere.TailleClient)
            {
                if (!IsUpdate)
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
                                SessionObject.EtatControlCourant = false;
                                Message.ShowError("Il existe déja une demande de ce type sur ce client", "Accueil");
                            }
                        }


                    };
                    service.RetourneDemandeClientTypeAsync(centre, client, Ordre, idCentre, tdem);
                    service.CloseAsync();
                }
            }
           
        }
        private void VerifieInformation(int fk_idcentre, string centre, string client, string Ordre, string produit, string tdem)
        {
            if (tdem == SessionObject.Enumere.AbonnementSeul ||
                tdem == SessionObject.Enumere.Resiliation ||
                tdem == SessionObject.Enumere.Reabonnement ||
                tdem == SessionObject.Enumere.FactureManuelle ||
                tdem == SessionObject.Enumere.AvoirConsomation ||
                tdem == SessionObject.Enumere.ChangementCompteur ||
                tdem == SessionObject.Enumere.FermetureBrt ||
                tdem == SessionObject.Enumere.PoseCompteur ||
                tdem == SessionObject.Enumere.DimunitionPuissance ||
                tdem == SessionObject.Enumere.AugmentationPuissance  ||
                tdem == SessionObject.Enumere.DeposeCompteur ||
                tdem == SessionObject.Enumere.ReouvertureBrt)
            {
                RetourneOrdre(fk_idcentre, centre, client, produit, tdem);
            }
            else if (tdem == SessionObject.Enumere.BranchementSimple ||
                     tdem == SessionObject.Enumere.BranchementAbonement)
            {
                if (this.Rdb_New.IsChecked == true)
                    RetourneInfoBrt(fk_idcentre, centre, client, Ordre, produit, tdem);
                else
                    RetourneOrdre(fk_idcentre, centre, client, produit, tdem);
            }
        }


        public void EnregisterDemande(CsDemande _LaDemande)
        {

            try
            {
                _LaDemande.LaDemande.MATRICULE = UserConnecte.matricule;
                _LaDemande.LaDemande.USERCREATION = UserConnecte.matricule;
                _LaDemande.LaDemande.USERMODIFICATION = UserConnecte.matricule;
                _LaDemande.LaDemande.DATECREATION = System.DateTime.Now;
                _LaDemande.LaDemande.DATEMODIFICATION = System.DateTime.Now;
                _LaDemande.LaDemande.USERMODIFICATION = UserConnecte.matricule;
                _LaDemande.LaDemande.MATRICULE = UserConnecte.matricule;
                _LaDemande.LaDemande.NUMDEM  = !string.IsNullOrEmpty(this.Txt_NumDevis.Text )? this.Txt_NumDevis.Text : string.Empty  ;

                _LaDemande.LaDemande.CENTRE = string.IsNullOrEmpty(_LaDemande.LaDemande.CENTRE) ? string.Empty : _LaDemande.LaDemande.CENTRE;
                _LaDemande.LaDemande.CLIENT = string.IsNullOrEmpty(_LaDemande.LaDemande.CLIENT) ? string.Empty : _LaDemande.LaDemande.CLIENT;
                _LaDemande.LaDemande.ORDRE = string.IsNullOrEmpty(this.Txt_Ordre.Text) ? string.Empty : this.Txt_Ordre.Text;


                _LaDemande.Ag .NUMDEM = string.IsNullOrEmpty(_LaDemande.LaDemande.NUMDEM) ? string.Empty : _LaDemande.LaDemande.NUMDEM;
                _LaDemande.Ag.CENTRE = string.IsNullOrEmpty(_LaDemande.LaDemande.CENTRE) ? string.Empty : _LaDemande.LaDemande.CENTRE;
                _LaDemande.Ag.FK_IDCENTRE = _LaDemande.LaDemande.FK_IDCENTRE;
                _LaDemande.Ag.CLIENT = string.IsNullOrEmpty(_LaDemande.LaDemande.CLIENT) ? string.Empty : _LaDemande.LaDemande.CLIENT;

                _LaDemande.Ag.NOMP = string.IsNullOrEmpty(this.Txt_NomClient.Text) ? null : this.Txt_NomClient.Text;

                _LaDemande.Ag.COMMUNE = string.IsNullOrEmpty(this.Txt_CodeCommune.Text) ? null : this.Txt_CodeCommune.Text;
                _LaDemande.Ag.QUARTIER = string.IsNullOrEmpty(this.Txt_CodeQuartier.Text) ? null : this.Txt_CodeQuartier.Text;
                _LaDemande.Ag.RUE = string.IsNullOrEmpty(this.Txt_CodeNomRue.Text) ? null : this.Txt_CodeNomRue.Text;
                _LaDemande.Ag.FK_IDCOMMUNE = this.Txt_CodeCommune.Tag == null ? _LaDemande.Ag.FK_IDCOMMUNE : int.Parse(this.Txt_CodeCommune.Tag.ToString());
                _LaDemande.Ag.FK_IDQUARTIER = this.Txt_CodeQuartier.Tag == null ? _LaDemande.Ag.FK_IDQUARTIER : int.Parse(this.Txt_CodeQuartier.Tag.ToString());
                _LaDemande.Ag.FK_IDRUE = this.Txt_CodeNomRue.Tag == null ? _LaDemande.Ag.FK_IDRUE : int.Parse(this.Txt_CodeNomRue.Tag.ToString());


                _LaDemande.Ag.TELEPHONE = string.IsNullOrEmpty(this.Txt_Telephone.Text) ? null : this.Txt_Telephone.Text;
                _LaDemande.Ag.EMAIL = string.IsNullOrEmpty(this.Txt_Email.Text) ? null : this.Txt_Email.Text;
                _LaDemande.Ag.TOURNEE = string.IsNullOrEmpty(this.Txt_Tournee.Text) ? null : this.Txt_Tournee.Text;
                _LaDemande.Ag.FK_IDTOURNEE = this.Txt_Tournee.Tag == null ? _LaDemande.Ag.FK_IDTOURNEE : int.Parse(this.Txt_Tournee.Tag.ToString());

                _LaDemande.Ag.PORTE = string.IsNullOrEmpty(this.Txt_Porte.Text) ? null : this.Txt_Porte.Text;
                _LaDemande.Ag.ETAGE = string.IsNullOrEmpty(this.Txt_Etage.Text) ? null : this.Txt_Etage.Text;


                _LaDemande.Ag.ORDTOUR = string.IsNullOrEmpty(this.Txt_OrdreTour.Text) ? null : this.Txt_OrdreTour.Text;
                _LaDemande.Ag.QUARTIER = string.IsNullOrEmpty(this.Txt_CodeQuartier.Text) ? null : this.Txt_CodeQuartier.Text;
                _LaDemande.Ag.FAX = string.IsNullOrEmpty(this.Txt_Fax.Text) ? null : this.Txt_Fax.Text;
                _LaDemande.Ag.USERCREATION = UserConnecte.matricule;
                _LaDemande.Ag.USERMODIFICATION = UserConnecte.matricule;
                _LaDemande.Ag.DATECREATION = System.DateTime.Now;
                _LaDemande.Ag.DATEMODIFICATION = System.DateTime.Now;
                
            }
            catch (Exception)
            {
                
                throw;
            }
           
            //else if (_TypeDemande   == SessionObject.Enumere.Resiliation)
            //{

            //    _LaDemande.Abon_DRES = null;
            //    if (!string.IsNullOrEmpty(this.Txt_DateAction.Text)) _LaDemande.Abon_DRES = DateTime.Parse(this.Txt_DateAction.Text);
            //    _LaDemande.Evenement_PERIODE = string.IsNullOrEmpty(this.Txt_PeriodeAFacturer.Text) ? null : this.Txt_PeriodeAFacturer.Text;
            //}
            //else if (_TypeDemande   == SessionObject.Enumere.FermetureBrt)
            //{
            //    _LaDemande.Brt_DRES = null;
            //    if (!string.IsNullOrEmpty(this.Txt_DateAction.Text))
            //        _LaDemande.Brt_DRES = DateTime.Parse(this.Txt_DateAction.Text);
            //}
           
            //if (!string.IsNullOrEmpty(this.Txt_DateAction.Text))
                //_LaDemande = DateTime.Parse(this.Txt_DateAction.Text);

        }

        private void HandleLostFocus<T>(Library.NumericTextBox Code,TextBox Libelle,List<T> listItems)
        {
            if (!string.IsNullOrEmpty(Code.Text) &&
                Code.Text.Length == SessionObject.Enumere.TailleCodeQuartier &&
                listItems.Count != 0)
            {
                Code.Text = Code.Text.PadLeft(SessionObject.Enumere.TailleCodeQuartier, '0');
            }
            else
            {
                Code.Text = string.Empty;
                Libelle.Text = string.Empty;
            }
        }
        private void HandleLostFocus<T>(TextBox Code, TextBox Libelle, List<T> listItems)
        {
            if (!string.IsNullOrEmpty(Code.Text) &&
                Code.Text.Length == SessionObject.Enumere.TailleCodeQuartier &&
                listItems.Count != 0)
            {
                Code.Text = Code.Text.PadLeft(SessionObject.Enumere.TailleCodeQuartier, '0');
            }
            else
            {
                Code.Text = string.Empty;
                Libelle.Text = string.Empty;
            }
        }

        #region Evenement
        private void Txt_Client_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(Txt_Client.Text))
            {
                VideControle();
                return;            
            }
            SessionObject.EtatControlCourant = true ;
            this.Txt_Ordre.Text = string.Empty;
            LaDemande.LaDemande.ORDRE = string.Empty;
                ClasseMEthodeGenerique.IsChampObligatoireSaisie(Txt_Client);
                if (!string.IsNullOrEmpty(LaDemande.LaDemande.PRODUIT))
                    if (!string.IsNullOrEmpty(Txt_Client.Text) && Txt_Client.Text.Length == SessionObject.Enumere.TailleClient)
                    {
                        if (!IsUpdate)
                        {
                            LaDemande.LaDemande.CLIENT = Txt_Client.Text;
                            VerifieInformation(LaDemande.LaDemande.FK_IDCENTRE , LaDemande.LaDemande.CENTRE, LaDemande.LaDemande.CLIENT, LaDemande.LaDemande.ORDRE, LaDemande.LaDemande.PRODUIT, LaDemande.LaDemande.TYPEDEMANDE);
                        }
                        //else
                        //    RetourneDadresse(LaDemande);
                    }

               


              
            
        }
        private void Txt_Client_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(this.Txt_Client.Text))
                this.Txt_Client.Text = this.Txt_Client.Text.PadLeft(SessionObject.Enumere.TailleClient, '0');
        }
        private void Txt_Ordre_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (this.Txt_Ordre.Text.Length == SessionObject.Enumere.TailleOrdre)
            {
                LaDemande.LaDemande.ORDRE = this.Txt_Ordre.Text;
                VerifieExisteDemande(LaDemande.LaDemande.CENTRE, LaDemande.LaDemande.CLIENT, LaDemande.LaDemande.ORDRE, LaDemande.LaDemande.FK_IDCENTRE, LaDemande.LaDemande.TYPEDEMANDE);
                EnregisterDemande(LaDemande); 
            }
        }
        private void Txt_NumDevis_TextChanged(object sender, TextChangedEventArgs e)
        {
            ClasseMEthodeGenerique.IsChampObligatoireSaisie(Txt_NumDevis);
            if (this.Txt_NumDevis.Text.Length == SessionObject.Enumere.TailleNumDevis && !IsUpdate ) 
            RechercheDevis(this.Txt_NumDevis.Text);
        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Initctrl();
        }
        private void UserControl_LostFocus(object sender, RoutedEventArgs e)
        {
            EnregisterDemande(LaDemande); 
        }
        private void Txt_DateAction_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (this.Txt_DateAction.Text.Length == SessionObject.Enumere.TailleDate)
                if (ClasseMEthodeGenerique.IsDateValide(this.Txt_DateAction.Text)==null )
                    Message.ShowError(Langue.Msg_DateInvalide,Langue.lbl_Menu);
                else 
                {
                    if(LaDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.Resiliation   )
                    {
                    _leAbon.DRES = DateTime.Parse(this.Txt_DateAction.Text);
                    LaDemande.Abonne = _leAbon;
                    }
                    else if (TypeDemande == SessionObject.Enumere.FermetureBrt)
                    {
                        if (LaDemande.Branchement != null)
                        LaDemande.Branchement.DRES = DateTime.Parse(this.Txt_DateAction.Text);
                    }
                    else if (TypeDemande == SessionObject.Enumere.ReouvertureBrt)
                    {
                        if (LaDemande.Branchement != null)
                        {
                            LaDemande.Branchement.DRAC = DateTime.Parse(this.Txt_DateAction.Text);
                            LaDemande.Branchement.DRES = null;
                        }
                    }
                    else if (TypeDemande == SessionObject.Enumere.DeposeCompteur ||
                        TypeDemande == SessionObject.Enumere.PoseCompteur)
                        LaDemande.LaDemande.DATEFLAG = DateTime.Parse(this.Txt_DateAction.Text); 
               }
        }
        private void btn_regroupementcpt_Click(object sender, RoutedEventArgs e)
        {

        }
        //private void checkBox1_Checked(object sender, RoutedEventArgs e)
        //{

        //    if (chk_recevoirEmail.IsChecked == true)
        //        this.Txt_Email.Background = new SolidColorBrush(Color.FromArgb(100, 173, 216, 230));
        //    else
        //        this.Txt_Email.Background = new SolidColorBrush(Colors.Transparent);
        //}
        #endregion
        #region Commune
        private void Txt_CodeCommune_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                HandleLostFocus<CsCommune>((Library.NumericTextBox)sender, this.Txt_LibelleCommune,LstCommuneCentre);
            }
            catch (Exception ex)
            {
               Message.ShowError(ex.Message, Langue.lbl_Menu);
            }
        }
        private void Txt_CodeCommune_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(this.Txt_CodeCommune.Text) &&
                this.Txt_CodeCommune.Text.Length == SessionObject.Enumere.TailleCodeQuartier &&
                LstCommuneCentre.Count != 0)
            {
                LaCommuneSelect = ClasseMEthodeGenerique.RetourneObjectFromList(LstCommuneCentre, this.Txt_CodeCommune.Text, "CODE");
                if (!string.IsNullOrEmpty(LaCommuneSelect.LIBELLE))
                {
                    this.Txt_LibelleCommune.Text = LaCommuneSelect.LIBELLE;
                    this.Txt_CodeCommune.Tag = LaCommuneSelect.PK_ID;
                    LstQuartierCommuneSelect = SessionObject.LstQuartier.Where(p => p.FK_IDCOMMUNE == LaCommuneSelect.PK_ID  || p.COMMUNE == "00000").ToList();
                }
                else
                {
                    var w = new MessageBoxControl.MessageBoxChildWindow(Langue.lbl_Menu, Langue.Msg_CommuneNonTrouve, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    w.OnMessageBoxClosed += (_, result) =>
                    {
                        this.Txt_LibelleCommune.Text=string.Empty;
                        this.Txt_CodeCommune.Text = string.Empty;
                        this.Txt_CodeCommune.Focus();
                    };
                    w.Show();
                }
            }
        }
        private void btn_Commune_Click(object sender, RoutedEventArgs e)
        {
            if (LaDemande != null && LaDemande.LeCentre != null &&  LaDemande.LeCentre.CODE != null && !string.IsNullOrEmpty(LaDemande.LeCentre.CODE ))
                 LstCommuneCentre = SessionObject.LstCommune.Where(p=>p.CENTRE == LaDemande.LeCentre.CODE ).ToList();
            if (LstCommuneCentre != null && LstCommuneCentre.Count != 0)
            {
                this.btn_Commune.IsEnabled = false;
                    List<object> _LstCommune = ClasseMEthodeGenerique.RetourneListeObjet(LstCommuneCentre);
                    UcListeGenerique ctr = new UcListeGenerique(_LstCommune, "CODE", "LIBELLE", Langue.lbl_ListeCommune);
                    ctr.Closed += new EventHandler(galatee_OkClickedBtnCommune);
                    ctr.Show();
            }
        }
        void galatee_OkClickedBtnCommune(object sender, EventArgs e)
        {
            this.btn_Commune.IsEnabled = true ;
            UcListeGenerique ctrs = sender as UcListeGenerique;
            if (ctrs.isOkClick)
            {
                CsCommune _CommuneSelect = (CsCommune)ctrs.MyObject;
                if (_CommuneSelect != null)
                {
                    this.Txt_CodeCommune.Text = _CommuneSelect.CODE;
                    LstQuartierCommuneSelect = SessionObject.LstQuartier.Where(p => p.FK_IDCOMMUNE == _CommuneSelect.PK_ID || p.COMMUNE == "00000").ToList();
                }
            }
        }
        #endregion
        #region Quartier
        private void Txt_CodeQuartier_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                HandleLostFocus<CsQuartier>((Library.NumericTextBox)sender, this.Txt_LibelleQuartier, SessionObject.LstQuartier);
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Langue.lbl_Menu);
            }
        }
        private void Txt_CodeQuartier_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (SessionObject.LstQuartier .Count != 0 && !string.IsNullOrEmpty(this.Txt_CodeQuartier.Text) &&
                this.Txt_CodeQuartier.Text.Length == SessionObject.Enumere.TailleCodeQuartier)
            {
                LeQuartierSelect = SessionObject.LstQuartier.FirstOrDefault(p => p.CODE == this.Txt_CodeQuartier.Text);
                if (LeQuartierSelect != null)
                {
                    this.Txt_LibelleQuartier.Text = LeQuartierSelect.LIBELLE;
                    this.Txt_CodeQuartier.Tag = LeQuartierSelect.PK_ID;
                    LstSecteurQuartierSelect = SessionObject.LstSecteur.Where(p => p.CODE == LaDemande.Ag.SECTEUR|| p.CODE == "00000").ToList();
                }
                else
                {
                    var w = new MessageBoxControl.MessageBoxChildWindow(Langue.lbl_Menu, Langue.Msg_QuartierNonTrouve, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    w.OnMessageBoxClosed += (_, result) =>
                    {
                        this.Txt_LibelleQuartier.Text = string.Empty;
                        this.Txt_CodeQuartier.Text = string.Empty;
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
                List<object> _LstObjQuartier = ClasseMEthodeGenerique.RetourneListeObjet(LstQuartierCommuneSelect.OrderByDescending(k=>k.COMMUNE ).ToList() );
                UcListeGenerique ctr = new UcListeGenerique(_LstObjQuartier, "CODE", "LIBELLE", Langue.lbl_ListeQuartiers);
                ctr.Closed += new EventHandler(galatee_OkClickedBtnQuartier);
                ctr.Show();
            }
        }
        void galatee_OkClickedBtnQuartier(object sender, EventArgs e)
        {
            this.btn_Quartier.IsEnabled = true ;
            UcListeGenerique ctrs = sender as UcListeGenerique;
            if (ctrs.isOkClick)
            {
                CsQuartier _LeQuartier = (CsQuartier)ctrs.MyObject;
                if (_LeQuartier != null)
                    this.Txt_CodeQuartier.Text = _LeQuartier.CODE;
                this.Txt_CodeQuartier.Tag  = _LeQuartier.PK_ID ;
                LstSecteurQuartierSelect = SessionObject.LstSecteur.Where(p => p.FK_IDQUARTIER  == _LeQuartier.PK_ID || p.CODE == "00000").ToList();
            }
        }
        #endregion
        #region Tournee
        private void Txt_Tournee_LostFocus(object sender, RoutedEventArgs e)
        {
            if (LstZone != null && LstZone.Count == 0)
                LstZone = SessionObject.LstZone.Where(p => p.CENTRE == LaDemande.LeCentre.CODE).ToList();
            if (!string.IsNullOrEmpty(this.Txt_Tournee.Text))
            {
                LaTourneSelect = LstZone.FirstOrDefault(p => p.CENTRE == LaDemande.LaDemande.CENTRE && p.CODE == this.Txt_Tournee.Text);
                if (LaTourneSelect == null)
                {
                    var w = new MessageBoxControl.MessageBoxChildWindow(Langue.lbl_Menu, Langue.MsgTourneNonTrouvee, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    w.OnMessageBoxClosed += (_, result) =>
                    {
                        this.Txt_Tournee.Focus();
                    };
                    w.Show();
                }
                else this.Txt_Tournee.Tag = LaTourneSelect.PK_ID;

            }

        }
        private void btn_zone_Click(object sender, RoutedEventArgs e)
        {
            LstZone = SessionObject.LstZone.Where(p => p.CENTRE == LaDemande.LeCentre.CODE).ToList();
            if (LstZone != null && LstZone.Count != 0)
            {
                if (!string.IsNullOrEmpty(LaDemande.LaDemande.CENTRE))
                {
                    this.btn_zone.IsEnabled = false;
                    List<object> _LstObj = ClasseMEthodeGenerique.RetourneListeObjet(LstZone.Where(p => p.CENTRE == LaDemande.LaDemande.CENTRE && p.FK_IDCENTRE == LaDemande.LaDemande.FK_IDCENTRE).ToList());
                    UcListeGenerique ctr = new UcListeGenerique(_LstObj, "CODE", "LIBELLE", Langue.lbl_ListeQuartiers);
                    ctr.Closed += new EventHandler(galatee_OkClickedBtnTournee);
                    ctr.Show();
                }
            }
        }
        void galatee_OkClickedBtnTournee(object sender, EventArgs e)
        {
            this.btn_zone.IsEnabled = true ;
            UcListeGenerique ctrs = sender as UcListeGenerique;
            if (ctrs.isOkClick)
            {
                CsTournee _LaTournee = (CsTournee)ctrs.MyObject;
                this.Txt_Tournee.Text = _LaTournee.CODE;
                this.Txt_Tournee.Tag = _LaTournee.PK_ID;
                LaTourneSelect = _LaTournee;
            }
        }
        #endregion
        #region Secteur
        private void Txt_CodeSecteur_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                HandleLostFocus<CsSecteur>((TextBox)sender, this.Txt_LibelleSecteur, SessionObject.LstSecteur);
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Langue.lbl_Menu);
            }
        }
        private void Txt_CodeSecteur_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (SessionObject.LstSecteur.Count   != 0 && !string.IsNullOrEmpty(this.Txt_CodeSecteur.Text) &&
                this.Txt_CodeSecteur.Text.Length == SessionObject.Enumere.TailleCodeQuartier)
            {
                LeSecteurSelect = SessionObject.LstSecteur.FirstOrDefault(p => p.CODE == this.Txt_CodeSecteur.Text);
                if (LeSecteurSelect != null)
                {
                    this.Txt_LibelleSecteur.Text = LeSecteurSelect.LIBELLE;
                    this.Txt_CodeSecteur.Tag = LeSecteurSelect.PK_ID;
                    LstRuesSecteurSelect = SessionObject.LstRues.Where(p => p.FK_IDSECTEUR  == LeSecteurSelect.PK_ID  || p.CODE == "00000").ToList();
                    
                }
                else
                {
                    var w = new MessageBoxControl.MessageBoxChildWindow(Langue.lbl_Menu, Langue.Msg_QuartierNonTrouve, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    w.OnMessageBoxClosed += (_, result) =>
                    {
                        this.Txt_LibelleSecteur.Text = string.Empty;
                        this.Txt_CodeSecteur.Text = string.Empty;
                        this.Txt_CodeSecteur.Focus();
                    };
                    w.Show();
                }
            }
        }
        private void btn_Secteur_Click(object sender, RoutedEventArgs e)
        {
            if (LstSecteurQuartierSelect != null && LstSecteurQuartierSelect.Count != 0)
            {
                this.btn_Secteur.IsEnabled = false;
                List<object> _LstObjSecteur = ClasseMEthodeGenerique.RetourneListeObjet(LstSecteurQuartierSelect);
                UcListeGenerique ctr = new UcListeGenerique(_LstObjSecteur, "CODE", "LIBELLE", Langue.lbl_ListeDesSecteurs);
                ctr.Closed += new EventHandler(galatee_OkClickedBtnSecteur);
                ctr.Show();
            }
        }
        void galatee_OkClickedBtnSecteur(object sender, EventArgs e)
        {
            this.btn_Secteur.IsEnabled = true ;
            UcListeGenerique ctrs = sender as UcListeGenerique;
            if (ctrs.isOkClick)
            {
                CsSecteur _LeSecteur = (CsSecteur)ctrs.MyObject;
                if (_LeSecteur != null)
                    this.Txt_CodeSecteur.Text = _LeSecteur.CODE;
            }
        }
        #endregion
        #region Rue
        private void Txt_CodeNomRue_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                HandleLostFocus<CsRues>((TextBox)sender, this.Txt_NomRue, SessionObject.LstRues);
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Langue.lbl_Menu);
            }
        }
        private void Txt_CodeNomRue_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (SessionObject.LstRues  .Count != 0 && !string.IsNullOrEmpty(this.Txt_CodeNomRue.Text) &&
                this.Txt_CodeNomRue.Text.Length == SessionObject.Enumere.TailleCodeQuartier)
            {
                //CsRues _LaRue = ClasseMEthodeGenerique.RetourneObjectFromList(LstRuesAll, "CODE", this.Txt_CodeNomRue.Text);
                LaRueSelect = SessionObject.LstRues.FirstOrDefault(p => p.CODE == this.Txt_CodeNomRue.Text);
                if (LaRueSelect != null)
                {
                    this.Txt_NomRue.Text = LaRueSelect.LIBELLE;
                    this.Txt_CodeNomRue.Tag  = LaRueSelect.PK_ID ;
                }
                else
                {
                    var w = new MessageBoxControl.MessageBoxChildWindow(Langue.lbl_Menu, Langue.Msg_QuartierNonTrouve, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    w.OnMessageBoxClosed += (_, result) =>
                    {
                        this.Txt_NomRue.Text = string.Empty;
                        this.Txt_CodeNomRue.Text = string.Empty;
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
                this.btn_Rue.IsEnabled = false;
                List<object> _LstObjSecteur = ClasseMEthodeGenerique.RetourneListeObjet(LstRuesSecteurSelect);
                UcListeGenerique ctr = new UcListeGenerique(_LstObjSecteur, "CODE", "LIBELLE", Langue.lbl_ListeDesSecteurs);
                ctr.Closed += new EventHandler(galatee_OkClickedBtnRue);
                ctr.Show();
            }
        }
        void galatee_OkClickedBtnRue(object sender, EventArgs e)
        {
            this.btn_Rue.IsEnabled = true ;
            UcListeGenerique ctrs = sender as UcListeGenerique;
            if (ctrs.isOkClick)
            {
                CsRues _LaRue = (CsRues)ctrs.MyObject;
                if (_LaRue != null)
                    this.Txt_CodeNomRue.Text = _LaRue.CODE;
            }
        }
        #endregion
        private void Txt_NumDevis_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty( this.Txt_NumDevis.Text))
            this.Txt_NumDevis.Text = this.Txt_NumDevis.Text.PadLeft(SessionObject.Enumere.TailleNumDevis, '0');
        }

        private void Txt_Tournee_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (this.Txt_Tournee.Text.Length == SessionObject.Enumere.TailleCodeTournee)
            {
                LstZone = SessionObject.LstZone.Where(p => p.CENTRE == LaDemande.LeCentre.CODE && p.FK_IDCENTRE == LaDemande.LaDemande.FK_IDCENTRE ).ToList();
                if (LstZone != null && LstZone.Count != 0)
                {
                    if (!string.IsNullOrEmpty(this.Txt_Tournee.Text))
                    {
                        CsTournee laTournee = LstZone.FirstOrDefault(t => t.CODE == this.Txt_Tournee.Text);
                        if (laTournee != null)
                            this.Txt_Tournee.Tag = laTournee.PK_ID;
                    }
                }
            }
        }
    }
}
