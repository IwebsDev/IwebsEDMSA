﻿using System;
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
using System.Collections.ObjectModel;
using System.IO;
using System.Threading;
using Galatee.Silverlight.Resources.Devis;
using Galatee.Silverlight.Library;
using Galatee.Silverlight.Shared;
using System.Text.RegularExpressions;
using Galatee.Silverlight.MainView;

namespace Galatee.Silverlight.Accueil
{
    public partial class FrmCreationAbon : ChildWindow
    {
        private UcImageScanne formScanne = null;
        private Object ModeExecution = null;
        private List<CsTournee> _listeDesTourneeExistant = null;
        private List<CsCommune> _listeDesCommuneExistant = null;
        private List<CsCommune> _listeDesCommuneExistantCentre = null;
        private List<CsReglageCompteur> _listeDesReglageCompteurExistant = null;
        private List<CsCentre> _listeDesCentreExistant = null;
        private CsTdem _leTypeDemandeExistant = null;
        private string Tdem = null;
        private List<CsRues> _listeDesRuesExistant = null;
        private List<CsQuartier> _listeDesQuartierExistant = null;
        private List<ObjAPPAREILS> listAppareilsSelectionnes = null;
        bool isPreuveSelectionnee = false;
        private DataGrid _dataGrid = null;
        private List<CsUsage> lstusage = new List<CsUsage>();

        List<Galatee.Silverlight.ServiceAccueil.CsCoutDemande> LstDesCoutsDemande = new List<Galatee.Silverlight.ServiceAccueil.CsCoutDemande>();

        public string nom;
        public string prenom;
        public DateTime? datefinvalidité = new DateTime();
        public DateTime? datenaissance = new DateTime();
        public string numeropiece;
        public int? typepiece;

        public string NUMEROREGISTRECOMMERCE;
        public int? FK_IDSTATUTJURIQUE;
        public decimal? CAPITAL;
        public string IDENTIFICATIONFISCALE;
        public DateTime? DATECREATION = new DateTime();
        public string SIEGE;
        public string NOMMANDATAIRE;
        public string PRENOMMANDATAIRE;
        public string RANGMANDATAIRE;
        public string NOMSIGNATAIRE;
        public string PRENOMSIGNATAIRE;
        public string RANGSIGNATAIRE;

        public string DENOMINATION;
        public string NOMMANDATAIRE1;
        public string NOMSIGNATAIRE1;
        public string PRENOMMANDATAIRE1;
        public string PRENOMSIGNATAIRE1;
        public string RANGMANDATAIRE1;
        public string RANGSIGNATAIRE1;


        public FrmCreationAbon()
        {
            InitializeComponent();
            //Txt_Porte.MaxLength = 5;
            //Txt_Etage.MaxLength = 2;

            this.Txt_CodeConso.MaxLength = SessionObject.Enumere.TailleCodeConso;
            this.Txt_CodeRegroupement.MaxLength = SessionObject.Enumere.TailleCodeRegroupement;
            this.TxtCategorieClient.MaxLength = SessionObject.Enumere.TailleCodeCategorie;
            this.Txt_usage.MaxLength = SessionObject.Enumere.TailleUsage;
            this.txt_Commune.MaxLength = SessionObject.Enumere.TailleCommune;
            this.txt_Quartier.MaxLength = SessionObject.Enumere.TailleQuartier;
            this.txt_NumSecteur.MaxLength = SessionObject.Enumere.TailleSecteur;
            this.txt_NumRue.MaxLength = SessionObject.Enumere.TailleRue;
            this.txt_MaticuleAgent.MaxLength = SessionObject.Enumere.TailleMatricule ;
            this.Txt_Client.MaxLength = SessionObject.Enumere.TailleClient;

            txt_FinPeriodeExo.MaxLength = 7;
            txt_DebutPeriodeExo.MaxLength = 7;

            this.txtAdresse.MaxLength = 100;
            this.Txt_Email.MaxLength = 50;

            txt_Reglage.Visibility = Visibility.Collapsed;
            Btn_Reglage.Visibility = Visibility.Collapsed;
            this.tabItemPersoneAdministration.Visibility = System.Windows.Visibility.Collapsed;
            this.tabItemPersoneMoral.Visibility = System.Windows.Visibility.Collapsed;
            this.tab_proprio.Visibility = System.Windows.Visibility.Collapsed;

            dtg_TarifClient.Visibility = System.Windows.Visibility.Collapsed;
            txt_MaticuleAgent.Visibility = System.Windows.Visibility.Collapsed;
            lbl_MatriculeAgent.Visibility = System.Windows.Visibility.Collapsed;

            label1_Copy1.Visibility = System.Windows.Visibility.Collapsed ;
            Txt_Client.Visibility = System.Windows.Visibility.Collapsed ;
            tbControleClient.IsEnabled = false;

            this.dtp_RendezVousPrev.SelectedDate = System.DateTime.Today.AddDays(15);

            ChargerPuissanceInstalle();
            ChargerForfait();
            ChargerTarifParCategorieMt();
            ChargerTypeDocument();
            ChargerCategorieClient_TypeClient();
            ChargerNatureClient_TypeClient();
            ChargerUsage_NatureClient();
            ChargerCategorieClient_Usage();
            RemplirStatutJuridique();
            RemplirTourneeExistante();
            RemplirCategorieClient();
            RemplirPieceIdentite();
            RemplirUsage();
            RemplirCodeRegroupement();
            RemplirCodeConsomateur();
            RemplirSecteur();
            RemplirNationnalite();
            RemplirCommune();
            RemplirListeDesQuartierExistant();
            RemplirListeDesRuesExistant();
            RemplirListeDesDiametresExistant();
            RemplirTypeClient();
            RemplirProprietaire();
            ChargerMois();
            ChargerFrequence();
            ChargerDiametreCompteur();
            ChargerMarque();
            ChargerTypeCompteur();
            ChargerTypeBranchement();
            //Activation de la zone de recherche en fonction du type de demande
            ActivationEnFonctionDeTdem();
            ChargerListDesSite();

            //label8.Visibility = System.Windows.Visibility.Collapsed;
            this.Txt_DateAbonnement.Text = System.DateTime.Today.Date.ToShortDateString();

        }

        CsDemandeBase laDemandeSelect = null;
        CsDemande laDetailDemande = null;

        private List<ObjDOCUMENTSCANNE> ObjetScanne = new List<ObjDOCUMENTSCANNE>();
        private List<ObjPIECEIDENTITE> ListeTYpePiece = new List<ObjPIECEIDENTITE>();
        private List<CsStatutJuridique> ListStatuJuridique = new List<CsStatutJuridique>();
        public ObservableCollection<CsTypeDOCUMENTSCANNE> LstTypeDocument = new ObservableCollection<CsTypeDOCUMENTSCANNE>();
        public List<CsCATEGORIECLIENT_TYPECLIENT> LstCategorieClient_TypeClient = new List<CsCATEGORIECLIENT_TYPECLIENT>();
        public List<CsNATURECLIENT_TYPECLIENT> LstNatureClient_TypeClient = new List<CsNATURECLIENT_TYPECLIENT>();
        public List<CsUSAGE_NATURECLIENT> LstUsage_NatureClient = new List<CsUSAGE_NATURECLIENT>();
        public List<CsCATEGORIECLIENT_USAGE> LstCategorieClient_Usage = new List<CsCATEGORIECLIENT_USAGE>();
        public List<CsProprietaire> Lsttypeprop = new List<CsProprietaire>();

        List<CsTarif> LstTarif = new List<CsTarif>();
        List<CsForfait> LstForfait = new List<CsForfait>();
        List<CsTarif> LstPuissanceTarif = new List<CsTarif>();
        List<CsPuissance> LstPuissanceMt = new List<CsPuissance>();
        List<CsFrequence> LstFrequence = new List<CsFrequence>();
        List<CsMois> LstMois = new List<CsMois>();
        List<CsCodeTaxeApplication> LstCodeApplicationTaxe = new List<CsCodeTaxeApplication>();

        private void ActiverZoneRecherche(string p)
        {
            try
            {

            }
            catch (Exception ex)
            {
                throw ex;
            }
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
                };
                service.ChargerTypeDocumentAsync();
                service.CloseAsync();

            }
            catch (Exception)
            {

                throw;
            }
        }
        private void ChargerCategorieClient_TypeClient()
        {
            try
            {
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.ChargerCategorieClient_TypeClientCompleted += (s, args) =>
                {
                    if ((args != null && args.Cancelled) || (args.Error != null))
                        return;

                    foreach (var item in args.Result)
                    {
                        LstCategorieClient_TypeClient.Add(item);
                    }
                    //this.Resources.Add("FuelList", LstTypeDocument);

                };
                service.ChargerCategorieClient_TypeClientAsync();
                service.CloseAsync();

            }
            catch (Exception)
            {

                throw;
            }
        }
        private void ChargerNatureClient_TypeClient()
        {
            try
            {
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.ChargerNatureClient_TypeClientCompleted += (s, args) =>
                {
                    if ((args != null && args.Cancelled) || (args.Error != null))
                        return;

                    foreach (var item in args.Result)
                    {
                        LstNatureClient_TypeClient.Add(item);
                    }
                    //this.Resources.Add("FuelList", LstTypeDocument);

                };
                service.ChargerNatureClient_TypeClientAsync();
                service.CloseAsync();

            }
            catch (Exception)
            {

                throw;
            }
        }
        private void ChargerUsage_NatureClient()
        {
            try
            {
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.ChargerUsage_NatureClientCompleted += (s, args) =>
                {
                    if ((args != null && args.Cancelled) || (args.Error != null))
                        return;

                    foreach (var item in args.Result)
                    {
                        LstUsage_NatureClient.Add(item);
                    }
                    //this.Resources.Add("FuelList", LstTypeDocument);

                };
                service.ChargerUsage_NatureClientAsync();
                service.CloseAsync();

            }
            catch (Exception)
            {

                throw;
            }
        }
        private void ChargerCategorieClient_Usage()
        {
            try
            {
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));

                service.ChargerCategorieClient_UsageCompleted += (s, args) =>
                {
                    if ((args != null && args.Cancelled) || (args.Error != null))
                        return;

                    foreach (var item in args.Result)
                    {
                        LstCategorieClient_Usage.Add(item);
                    }
                    //this.Resources.Add("FuelList", LstTypeDocument);

                };
                service.ChargerCategorieClient_UsageAsync();
                service.CloseAsync();

            }
            catch (Exception)
            {

                throw;
            }
        }
        private void Cbo_Site_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (this.Cbo_Site.SelectedItem != null)
                {
                    var csSite = Cbo_Site.SelectedItem as CsSite;
                    if (csSite != null)
                    {
                        this.txtSite.Text = csSite.CODE ?? string.Empty;
                        if (laDemandeSelect != null)
                        {
                            if (laDemandeSelect.FK_IDCENTRE != 0)
                                RemplirCentreDuSite(csSite.PK_ID, laDemandeSelect.FK_IDCENTRE);
                        }
                        else
                            RemplirCentreDuSite(csSite.PK_ID, 0);

                        if (csSite.CODE != SessionObject.Enumere.CodeSiteScaBT && csSite.CODE != SessionObject.Enumere.CodeSiteScaMT)
                        {
                            Txt_CodeRegroupement.IsReadOnly = true;
                            this.Cbo_Regroupement.IsEnabled = false;
                        }
                        else
                        {
                            Txt_CodeRegroupement.IsReadOnly = false;
                            this.Cbo_Regroupement.IsEnabled = true ;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.txtDevis);
            }
        }
        public static List<CsSite> RetourneSiteByCentre(List<CsCentre> _lstCentre)
        {
            try
            {
                // La grid doit afficher le detail d un recu par mode de reglement
                var leCentres = (from p in _lstCentre
                                 group new { p } by new { p.CODESITE, p.FK_IDCODESITE, p.LIBELLESITE } into pResult
                                 select new
                                 {
                                     pResult.Key.CODESITE,
                                     pResult.Key.FK_IDCODESITE,
                                     pResult.Key.LIBELLESITE
                                 });

                List<CsSite> _LstSite = new List<CsSite>();

                foreach (var r in leCentres.OrderByDescending(p => p.CODESITE))
                {
                    CsSite _leSite = new CsSite();
                    _leSite.CODE = r.CODESITE;
                    _leSite.PK_ID = r.FK_IDCODESITE;
                    _leSite.LIBELLE = r.LIBELLESITE;
                    _LstSite.Add(_leSite);
                }
                return _LstSite;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        private void RemplirCentreDuSite(int pIdSite, int pIdcentre)
        {
            try
            {
                Cbo_Centre.Items.Clear();
                this.txtCentre.Text = string.Empty;
                if (_listeDesCentreExistant != null &&
                    _listeDesCentreExistant.Count != 0)
                {
                    List<CsCentre> lesCentreDuPerimetreAction = _listeDesCentreExistant.Where(p => p.FK_IDCODESITE == pIdSite && p.CODE != SessionObject.Enumere.Generale).ToList();
                    if (lesCentreDuPerimetreAction != null)
                        foreach (var item in lesCentreDuPerimetreAction)
                        {
                            Cbo_Centre.Items.Add(item);
                        }
                    //Cbo_Centre.ItemsSource = lesCentreDuPerimetreAction;
                    Cbo_Centre.SelectedValuePath = "PK_ID";
                    Cbo_Centre.DisplayMemberPath = "LIBELLE";

                    if (pIdcentre != 0)
                        this.Cbo_Centre.SelectedItem = _listeDesCentreExistant.First(t => t.PK_ID == pIdcentre);
                    if (lesCentreDuPerimetreAction.Count == 1)
                        this.Cbo_Centre.SelectedItem = lesCentreDuPerimetreAction.First();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void RemplirCentrePerimetre(List<CsCentre> lstCentre, List<CsSite> lstSite)
        {
            try
            {
                Cbo_Centre.Items.Clear();
                if (_listeDesCentreExistant != null &&
                    _listeDesCentreExistant.Count != 0)
                {
                    if (lstCentre != null)
                        foreach (var item in lstCentre)
                        {
                            Cbo_Centre.Items.Add(item);
                        }
                    Cbo_Centre.SelectedValuePath = "PK_ID";
                    Cbo_Centre.DisplayMemberPath = "LIBELLE";

                    if (lstSite != null)
                        foreach (var item in lstSite)
                        {
                            Cbo_Site.Items.Add(item);
                        }
                    Cbo_Site.SelectedValuePath = "PK_ID";
                    Cbo_Site.DisplayMemberPath = "LIBELLE";

                    if (lstSite != null && lstSite.Count == 1)
                        Cbo_Site.SelectedItem = lstSite.First();

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        List<ServiceAccueil.CsCentre> lesCentre = new List<ServiceAccueil.CsCentre>();
        List<ServiceAccueil.CsSite> lesSite = new List<ServiceAccueil.CsSite>();
        void ChargerListDesSite()
        {
            try
            {

                if (SessionObject.LstCentre != null && SessionObject.LstCentre.Count != 0)
                {
                    lesCentre = Shared.ClasseMEthodeGenerique.RetourCentreByPerimetre(SessionObject.LstCentre, UserConnecte.listeProfilUser);
                    lesSite = Shared.ClasseMEthodeGenerique.RetourneSiteByCentre(lesCentre);
                    _listeDesCentreExistant = lesCentre;
                    RemplirCentrePerimetre(lesCentre, lesSite);
                    return;
                }
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.ListeDesDonneesDesSiteAsync(false);
                service.ListeDesDonneesDesSiteCompleted += (s, args) =>
                {
                    try
                    {
                        if (args != null && args.Cancelled)
                            return;
                        SessionObject.LstCentre = args.Result;
                        if (SessionObject.LstCentre.Count != 0)
                        {
                            lesCentre = Shared.ClasseMEthodeGenerique.RetourCentreByPerimetre(SessionObject.LstCentre, UserConnecte.listeProfilUser);
                            lesSite = Shared.ClasseMEthodeGenerique.RetourneSiteByCentre(lesCentre);
                            _listeDesCentreExistant = lesCentre;
                            RemplirCentrePerimetre(lesCentre, lesSite);
                        }
                        else
                        {
                            Message.ShowInformation("Aucun site trouvé en base.", "Erreur");
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        Message.ShowError(ex, "Erreur");
                    }
                };
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
            }
        }
        private void Cbo_Centre_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (this.Cbo_Centre.SelectedItem != null)
                {
                    CsCentre centre = Cbo_Centre.SelectedItem as CsCentre;
                    if (centre != null)
                    {
                        this.txtCentre.Text = centre.CODE ?? string.Empty;
                        this.txtCentre.Tag = centre.PK_ID;
                        RemplirCommuneParCentre(centre);
                        RemplirProduitCentre(centre);
                        RemplirTourneeExistante(centre.PK_ID);
                    }
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.txtDevis);
            }
        }
        private void RemplirCommuneParCentre(CsCentre centre)
        {
            try
            {

                if (_listeDesCommuneExistant != null && _listeDesCommuneExistant.Count > 0)
                    _listeDesCommuneExistantCentre = _listeDesCommuneExistant.Where(c => c.FK_IDCENTRE == centre.PK_ID) != null ? _listeDesCommuneExistant.Where(c => c.FK_IDCENTRE == centre.PK_ID).ToList() : new List<CsCommune>();
                txt_Commune.Text = string.Empty;
                Cbo_Commune.ItemsSource = _listeDesCommuneExistantCentre;
                Cbo_Commune.IsEnabled = true;
                Cbo_Commune.SelectedValuePath = "PK_ID";
                Cbo_Commune.DisplayMemberPath = "LIBELLE";

                Cbo_Commune.ItemsSource = _listeDesCommuneExistantCentre;
                if (_listeDesCommuneExistantCentre != null && _listeDesCommuneExistantCentre.Count > 0)
                {
                    if (_listeDesCommuneExistantCentre.Count == 1)
                        Cbo_Commune.SelectedItem = _listeDesCommuneExistantCentre[0];
                }
                else
                {
                    Message.ShowError("Aucune commune associée à ce centre", "Info");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void RemplirProduitCentre(CsCentre pCentre)
        {
            try
            {
                Cbo_Produit.ItemsSource = null;
                Cbo_Produit.ItemsSource = pCentre.LESPRODUITSDUSITE;
                Cbo_Produit.SelectedValuePath = "PK_ID";
                Cbo_Produit.DisplayMemberPath = "LIBELLE";
                if (pCentre.LESPRODUITSDUSITE != null && pCentre.LESPRODUITSDUSITE.Count != 0 && pCentre.LESPRODUITSDUSITE.Count == 1)
                    this.Cbo_Produit.SelectedItem = pCentre.LESPRODUITSDUSITE.First();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void Cbo_Produit_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (this.Cbo_Produit.SelectedItem != null)
                {
                    var produit = Cbo_Produit.SelectedItem as CsProduit;
                    if (produit != null)
                    {
                        this.txtProduit.Text = produit.CODE ?? string.Empty;
                        RemplirDiametreCompteur(produit.PK_ID);

                        #region Modif BSY 02-01-2016

                        if (produit.CODE == SessionObject.Enumere.ElectriciteMT)
                        {
                            lab_ListeAppareils.Visibility = Visibility.Collapsed;
                            Cbo_ListeAppareils.Visibility = Visibility.Collapsed;
                            Btn_ListeAppareils.Visibility = Visibility.Collapsed;

                            lbl_Reglage.Visibility = Visibility.Collapsed;
                            txt_Reglage.Visibility = Visibility.Collapsed;
                            Btn_Reglage.Visibility = Visibility.Collapsed;

                            lbl_Diametre.Visibility = System.Windows.Visibility.Collapsed;
                            btn_DiametreCompteur.Visibility = System.Windows.Visibility.Collapsed;
                            Txt_LibelleDiametre.Visibility = System.Windows.Visibility.Collapsed;


                            this.Cbo_TypeComptage.Visibility = System.Windows.Visibility.Visible ;
                            this.label3_Copy.Visibility = System.Windows.Visibility.Visible;

                            this.label_Puissance.Visibility = System.Windows.Visibility.Visible;
                            this.Cbo_Puissance.Visibility = System.Windows.Visibility.Visible;

                            this.lbl_NbreTransformateur.Visibility = System.Windows.Visibility.Visible;
                            this.Txt_NbreTransformateur.Visibility = System.Windows.Visibility.Visible;
                            this.Txt_NbreTransformateur.Text = "1";

                            ChargerTarifParCategorieMt();
                            CsTdem leTypeDemande = SessionObject.LstTypeDemande.FirstOrDefault(t => t.CODE == SessionObject.Enumere.BranchementAbonnementMt);
                            txt_tdem.Text = leTypeDemande.LIBELLE;
                            txt_tdem.Tag = leTypeDemande;
                        }
                        #endregion
                        else
                        {
                            lab_ListeAppareils.Visibility = Visibility.Visible;
                            Cbo_ListeAppareils.Visibility = Visibility.Visible;
                            Btn_ListeAppareils.Visibility = Visibility.Visible;
                            btn_tarif.Visibility = System.Windows.Visibility.Visible;
                            lbl_Reglage.Visibility = Visibility.Visible;
                            txt_Reglage.Visibility = Visibility.Visible;
                            Btn_Reglage.Visibility = Visibility.Visible;

                            this.Cbo_TypeComptage.Visibility = System.Windows.Visibility.Collapsed;
                            this.label3_Copy.Visibility = System.Windows.Visibility.Collapsed;
                            this.label_Puissance.Visibility = System.Windows.Visibility.Collapsed;
                            this.Cbo_Puissance.Visibility = System.Windows.Visibility.Collapsed;

                            this.lbl_NbreTransformateur.Visibility = System.Windows.Visibility.Collapsed;
                            this.Txt_NbreTransformateur.Visibility = System.Windows.Visibility.Collapsed;

                            lbl_Diametre.Visibility = System.Windows.Visibility.Visible ;
                            btn_DiametreCompteur.Visibility = System.Windows.Visibility.Visible;
                            Txt_LibelleDiametre.Visibility = System.Windows.Visibility.Visible;
                            Txt_LibelleDiametre.Text = string.Empty;
                            this.Txt_LibelleDiametre.Tag = null;

                            CsTdem leTypeDemande = SessionObject.LstTypeDemande.FirstOrDefault(t => t.CODE == SessionObject.Enumere.BranchementAbonement );
                            txt_tdem.Text = leTypeDemande.LIBELLE;
                            txt_tdem.Tag = leTypeDemande;
                        }

                        if (txtProduit.Text == SessionObject.Enumere.Eau)
                        {
                            this.lbl_Reglage.Content = "Diamètre compteur";
                            this.lbl_Diametre.Content = "Diamètre";
                            this.Txt_CodePussanceSoucrite.Text = "0";
                            this.Txt_CodePuissanceUtilise.Text = "0";
                            this.lbl_PuissanceSouscrite.IsEnabled = false;
                            this.btn_PussSouscrite.IsEnabled = false;
                            this.lbl_PuissanceUtilise.IsEnabled = false;
                            this.label_Puissance.IsEnabled = false;
                            this.Cbo_Puissance.IsEnabled = false;
                        }
                    }
                    ChargerPuissanceInstalle();
                    ChargerPuissance();
                    ChargerForfait();
                    ChargerTarifParCategorieMt();
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.txtDevis);
            }
        }

        private void RemplirTourneeExistante(int pCentreId)
        {
            try
            {
                if (SessionObject.LstZone != null && SessionObject.LstZone.Count != 0)
                {

                    List<Galatee.Silverlight.ServiceAccueil.CsTournee> lstTournee = SessionObject.LstZone;

                    Cbo_Zone.SelectedValuePath = "PK_ID";
                    Cbo_Zone.DisplayMemberPath = "CODE";
                    Cbo_Zone.ItemsSource = lstTournee.Where(t => t.FK_IDCENTRE == pCentreId);
                    if (lstTournee.Where(t => t.FK_IDCENTRE == pCentreId) != null && lstTournee.Where(t => t.FK_IDCENTRE == pCentreId).ToList().Count == 1)
                    {
                        Cbo_Zone.SelectedItem = lstTournee.Where(t => t.FK_IDCENTRE == pCentreId).First();
                        Cbo_Zone.Tag = lstTournee.Where(t => t.FK_IDCENTRE == pCentreId).First().PK_ID ;
                    }
                    return;
                }
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.ChargerLesTourneesCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    SessionObject.LstZone = args.Result;
                    List<Galatee.Silverlight.ServiceAccueil.CsTournee> lstTournee = SessionObject.LstZone;
                    Cbo_Zone.SelectedValuePath = "PK_ID";
                    Cbo_Zone.DisplayMemberPath = "CODE";
                    Cbo_Zone.ItemsSource = lstTournee.Where(t => t.FK_IDCENTRE == pCentreId);
                    if (lstTournee.Where(t => t.FK_IDCENTRE == pCentreId) != null && lstTournee.Where(t => t.FK_IDCENTRE == pCentreId).ToList().Count == 1)
                    {
                        Cbo_Zone.SelectedItem = lstTournee.Where(t => t.FK_IDCENTRE == pCentreId).First();
                        Cbo_Zone.Tag = lstTournee.Where(t => t.FK_IDCENTRE == pCentreId).First();
                    }
                };
                service.ChargerLesTourneesAsync();
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void Translate()
        {
            try
            {
                //this.ckbNoDeposit.Content = Languages.ckbNoDeposit;
                this.Btn_Annuler.Content = Languages.btnAnnuler;
                this.Title = Languages.ttlCreationDevis;
                //lab_ReceiptNumber.Content = Languages.NumeroRecu;
                //btnCheck.Content = Languages.btnRechercher;
                //lab_AmountOfDeposit.Content = Languages.MontantAccompte;
                //lab_Applicant.Content = Languages.Applicant;
                //lab_DateOfDeposit.Content = Languages.DateAccompte;
                //lnkLetter.Content = Languages.lnkLetter;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        ObjELEMENTDEVIS _element = new ObjELEMENTDEVIS();
        public List<ObjELEMENTDEVIS> MyElements { get; set; }
        public CsCtax Taxe { get; set; }
        decimal taux = (decimal)0;

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //this.Btn_Enregistrer.IsEnabled = false;
                ValidationDevis(laDetailDemande, false);
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.txtDevis);
            }
        }
        private void ValidationDevis(CsDemande laDetailDemande, bool IsTransmetre)
        {
            try
            {
                if (laDetailDemande == null)
                {
                    laDetailDemande = new CsDemande();
                    laDetailDemande.LaDemande = new CsDemandeBase();
                    laDetailDemande.Abonne = new CsAbon();
                    laDetailDemande.Ag = new CsAg();
                    laDetailDemande.Branchement = new CsBrt();
                    laDetailDemande.LeClient = new CsClient();
                    laDetailDemande.ObjetScanne = new List<ObjDOCUMENTSCANNE>();
                    laDetailDemande.LstCanalistion = new List<CsCanalisation>();
                    laDetailDemande.LstEvenement  = new List<CsEvenement >();
                    laDetailDemande.LaDemande.DATECREATION = DateTime.Now;
                    laDetailDemande.LaDemande.USERCREATION = UserConnecte.matricule;
                    laDetailDemande.LaDemande.MATRICULE  = UserConnecte.matricule;
                    laDetailDemande.LaDemande.FK_IDADMUTILISATEUR = UserConnecte.PK_ID;
                    laDetailDemande.LaDemande.CLIENT = this.Txt_Client.Text;

                }
                #region Demande
                if (Cbo_Centre.SelectedItem != null)
                {
                    var csCentre = Cbo_Centre.SelectedItem as CsCentre;
                    if (csCentre != null)
                    {
                        laDetailDemande.LaDemande.FK_IDCENTRE = csCentre.PK_ID;
                        laDetailDemande.LaDemande.CENTRE = csCentre.CODE;
                        laDetailDemande.LaDemande.LIBELLECENTRE = csCentre.LIBELLE;
                    }
                }

                if (Cbo_Produit.SelectedItem != null)
                {
                    var produit = Cbo_Produit.SelectedItem as CsProduit;
                    if (produit != null)
                    {
                        laDetailDemande.LaDemande.FK_IDPRODUIT = produit.PK_ID;
                        laDetailDemande.LaDemande.PRODUIT = produit.CODE;
                        laDetailDemande.LaDemande.LIBELLE = produit.LIBELLE;
                    }
                }

                if (txt_tdem.Tag != null)
                {
                    var typeDevis = (CsTdem)txt_tdem.Tag;
                    if (typeDevis != null)
                    {
                        laDetailDemande.LaDemande.FK_IDTYPEDEMANDE = typeDevis.PK_ID;
                        laDetailDemande.LaDemande.TYPEDEMANDE = typeDevis.CODE;
                        laDetailDemande.LaDemande.LIBELLETYPEDEMANDE  = typeDevis.LIBELLE ;
                    }
                }
                laDetailDemande.LaDemande.ORDRE = "01";
                #endregion
                #region Abon
                if (this.Txt_DateAbonnement.Text == null)
                {
                    Message.Show("Saisir la date d'abonnement", "ValidationDevis");
                    return;
                }
                if (laDetailDemande.Abonne == null)
                {
                    laDetailDemande.Abonne = new CsAbon();
                    laDetailDemande.Abonne.CENTRE = string.IsNullOrEmpty(laDetailDemande.LaDemande.CENTRE) ? null : laDetailDemande.LaDemande.CENTRE;
                    laDetailDemande.Abonne.CLIENT = string.IsNullOrEmpty(laDetailDemande.LaDemande.CLIENT) ? null : laDetailDemande.LaDemande.CLIENT;
                    laDetailDemande.Abonne.FK_IDCENTRE = laDetailDemande.LaDemande.FK_IDCENTRE;
                    laDetailDemande.Abonne.NUMDEM = string.IsNullOrEmpty(laDetailDemande.LaDemande.NUMDEM) ? string.Empty : laDetailDemande.LaDemande.NUMDEM;
                    laDetailDemande.Abonne.PRODUIT = string.IsNullOrEmpty(laDetailDemande.LaDemande.PRODUIT) ? string.Empty : laDetailDemande.LaDemande.PRODUIT;
                    laDetailDemande.Abonne.FK_IDPRODUIT = laDetailDemande.LaDemande.FK_IDPRODUIT == null ? 0 : laDetailDemande.LaDemande.FK_IDPRODUIT.Value;

                    laDetailDemande.Abonne.DATECREATION = DateTime.Now;
                    laDetailDemande.Abonne.USERCREATION = UserConnecte.matricule;
                }

                laDetailDemande.Abonne.NUMDEM = string.IsNullOrEmpty(laDetailDemande.LaDemande.NUMDEM) ? string.Empty : laDetailDemande.LaDemande.NUMDEM;
                laDetailDemande.Abonne.CENTRE = string.IsNullOrEmpty(laDetailDemande.LaDemande.CENTRE) ? string.Empty : laDetailDemande.LaDemande.CENTRE;
                laDetailDemande.Abonne.CLIENT = string.IsNullOrEmpty(laDetailDemande.LaDemande.CLIENT) ? string.Empty : laDetailDemande.LaDemande.CLIENT;
                laDetailDemande.Abonne.ORDRE = string.IsNullOrEmpty(laDetailDemande.LaDemande.ORDRE) ? string.Empty : laDetailDemande.LaDemande.ORDRE;
                laDetailDemande.Abonne.PRODUIT = string.IsNullOrEmpty(laDetailDemande.LaDemande.PRODUIT) ? string.Empty : laDetailDemande.LaDemande.PRODUIT;

                laDetailDemande.Abonne.PUISSANCE = string.IsNullOrEmpty(this.Txt_CodePussanceSoucrite.Text) ? 0 : Convert.ToDecimal(this.Txt_CodePussanceSoucrite.Text);
                laDetailDemande.Abonne.PUISSANCEUTILISEE = string.IsNullOrEmpty(this.Txt_CodePuissanceUtilise.Text) ? 0 : Convert.ToDecimal(this.Txt_CodePuissanceUtilise.Text);
                laDetailDemande.Abonne.RISTOURNE = string.IsNullOrEmpty(this.Txt_CodeRistoune.Text) ? 0 : Convert.ToDecimal(this.Txt_CodeRistoune.Text);

                laDetailDemande.Abonne.FORFAIT = string.IsNullOrEmpty(this.Txt_CodeForfait.Text) ? string.Empty : this.Txt_CodeForfait.Text;
                laDetailDemande.Abonne.TYPETARIF = string.IsNullOrEmpty(this.Txt_CodeTarif.Text) ? string.Empty : this.Txt_CodeTarif.Text;
                laDetailDemande.Abonne.PERFAC = string.IsNullOrEmpty(this.Txt_CodeFrequence.Text) ? string.Empty : this.Txt_CodeFrequence.Text;
                laDetailDemande.Abonne.MOISREL = string.IsNullOrEmpty(this.Txt_CodeMoisIndex.Text) ? string.Empty : this.Txt_CodeMoisIndex.Text;
                laDetailDemande.Abonne.MOISFAC = string.IsNullOrEmpty(this.Txt_CodeMoisFacturation.Text) ? string.Empty : this.Txt_CodeMoisFacturation.Text;

                if (laDetailDemande.LaDemande.PRODUIT == SessionObject.Enumere.ElectriciteMT)
                {
                    Galatee.Silverlight.ServiceAccueil.CsTypeComptage leTypeComptage = (Galatee.Silverlight.ServiceAccueil.CsTypeComptage)this.Txt_LibelleDiametre.Tag;
                    laDetailDemande.LaDemande.FK_IDTYPECOMPTAGE = leTypeComptage.PK_ID;
                    laDetailDemande.LaDemande.TYPECOMPTAGE = leTypeComptage.CODE ;

                    laDetailDemande.Abonne.FK_IDTYPECOMPTAGE = leTypeComptage.PK_ID;
                    laDetailDemande.Abonne.TYPECOMPTAGE = leTypeComptage.CODE;
                }
                laDetailDemande.Abonne.FK_IDCENTRE = laDetailDemande.LeClient.PK_ID;
                laDetailDemande.Abonne.FK_IDCENTRE = laDetailDemande.LaDemande.FK_IDCENTRE;
                laDetailDemande.Abonne.FK_IDPRODUIT = laDetailDemande.LaDemande.FK_IDPRODUIT.Value;
                laDetailDemande.Abonne.FK_IDFORFAIT = this.Txt_CodeForfait.Tag == null ? laDetailDemande.Abonne.FK_IDFORFAIT : (int)this.Txt_CodeForfait.Tag;
                laDetailDemande.Abonne.FK_IDMOISFAC = this.Txt_CodeMoisFacturation.Tag == null ? laDetailDemande.Abonne.FK_IDMOISFAC : (int)this.Txt_CodeMoisFacturation.Tag;
                laDetailDemande.Abonne.FK_IDMOISREL = this.Txt_CodeMoisIndex.Tag == null ? laDetailDemande.Abonne.FK_IDMOISREL : (int)this.Txt_CodeMoisIndex.Tag;
                laDetailDemande.Abonne.FK_IDTYPETARIF = this.Txt_CodeTarif.Tag == null ? laDetailDemande.Abonne.FK_IDTYPETARIF : (int)this.Txt_CodeTarif.Tag;
                laDetailDemande.Abonne.FK_IDPERIODICITEFACTURE = this.Txt_CodeFrequence.Tag == null ? laDetailDemande.Abonne.FK_IDPERIODICITEFACTURE : (int)this.Txt_CodeFrequence.Tag;
                laDetailDemande.Abonne.FK_IDPERIODICITERELEVE = this.Txt_CodeFrequence.Tag == null ? laDetailDemande.Abonne.FK_IDPERIODICITEFACTURE : (int)this.Txt_CodeFrequence.Tag;
                laDetailDemande.Abonne.DABONNEMENT = Convert.ToDateTime(this.Txt_DateAbonnement.Text);
                laDetailDemande.Abonne.ESTEXONERETVA = Chk_IsExonneration.IsChecked == true ? true : false;
                laDetailDemande.Abonne.DEBUTEXONERATIONTVA = Shared.ClasseMEthodeGenerique.FormatPeriodeAAAAMM(this.txt_DebutPeriodeExo.Text);
                laDetailDemande.Abonne.FINEXONERATIONTVA = Shared.ClasseMEthodeGenerique.FormatPeriodeAAAAMM(this.txt_FinPeriodeExo.Text);
                if (laDetailDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.BranchementAbonnementEp)
                    laDetailDemande.Abonne.NOMBREDEFOYER = laDetailDemande.LaDemande.NOMBREDEFOYER;

                laDetailDemande.Abonne.DATECREATION = DateTime.Now;
                laDetailDemande.Abonne.USERCREATION = UserConnecte.matricule;
                #endregion
                #region AG
                if (laDetailDemande.Ag == null) laDetailDemande.Ag = new CsAg();
                if (Cbo_Commune.Tag != null)
                {
                    laDetailDemande.Ag.FK_IDCOMMUNE = (int)Cbo_Commune.Tag;
                    laDetailDemande.Ag.COMMUNE = this.txt_Commune.Text;
                }
                if (Cbo_Zone.Tag != null)
                {
                    laDetailDemande.Ag.FK_IDTOURNEE = (int)Cbo_Zone.Tag;
                    laDetailDemande.Ag.TOURNEE  = ((CsTournee)Cbo_Zone.SelectedItem).CODE  ;
                }
                laDetailDemande.Ag.ORDTOUR = string.IsNullOrEmpty(this.TxtOrdreTournee.Text) ? string.Empty : this.TxtOrdreTournee.Text;
                if (Cbo_Quartier.Tag != null)
                {
                    laDetailDemande.Ag.FK_IDQUARTIER = (int)Cbo_Quartier.Tag;
                    laDetailDemande.Ag.QUARTIER = this.txt_Quartier.Text;
                }

                if (!string.IsNullOrEmpty(this.txt_NumRue.Text))
                {
                    laDetailDemande.Ag.RUE = this.txt_NumRue.Text;
                }
                if (Cbo_Secteur.Tag != null)
                {
                    laDetailDemande.Ag.FK_IDSECTEUR = (int)Cbo_Secteur.Tag;
                    laDetailDemande.Ag.SECTEUR = txt_NumSecteur.Text;
                }
                if (this.Cbo_Type_Proprietaire.SelectedItem != null)
                {
                    if (((CsProprietaire)this.Cbo_Type_Proprietaire.SelectedItem).CODE == SessionObject.Enumere.PROPRIETRAIRE)
                        laDetailDemande.Ag.NOMP = laDetailDemande.LeClient.NOMABON;
                    else
                        laDetailDemande.Ag.NOMP = Txt_NomProprio_PersonePhysiq.Text + "  " + Txt_PrenomProprio_PersonePhysiq.Text;
                }
                laDetailDemande.Ag.TELEPHONE = string.IsNullOrEmpty(this.txt_Telephone.Text) ? null : this.txt_Telephone.Text;
                laDetailDemande.Ag.ETAGE = string.IsNullOrEmpty(this.Txt_Etage.Text) ? null : this.Txt_Etage.Text;
                laDetailDemande.Ag.PORTE = string.IsNullOrEmpty(this.Txt_Porte.Text) ? null : this.Txt_Porte.Text;
                laDetailDemande.Ag.CENTRE = string.IsNullOrEmpty(laDetailDemande.LaDemande.CENTRE) ? null : laDetailDemande.LaDemande.CENTRE;
                laDetailDemande.Ag.CLIENT = string.IsNullOrEmpty(laDetailDemande.LaDemande.CLIENT) ? null : laDetailDemande.LaDemande.CLIENT;
                laDetailDemande.Ag.FK_IDCENTRE = laDetailDemande.LaDemande.FK_IDCENTRE;
                laDetailDemande.Ag.DATECREATION = DateTime.Now;
                laDetailDemande.Ag.USERCREATION = UserConnecte.matricule;

                #endregion
                #region Client
                if (laDetailDemande.LeClient == null)
                    laDetailDemande.LeClient = new CsClient();

                if (Cbo_Nationnalite.SelectedItem != null)
                {
                    var NationnaliteClient = Cbo_Nationnalite.SelectedItem as Galatee.Silverlight.ServiceAccueil.CsNationalite;
                    if (NationnaliteClient != null)
                    {
                        laDetailDemande.LeClient.NATIONNALITE = NationnaliteClient.CODE;
                        laDetailDemande.LeClient.FK_IDNATIONALITE = NationnaliteClient.PK_ID;
                    }
                }

                if (Cbo_Type_Proprietaire.SelectedItem != null)
                {
                    var TypeProprietaire = Cbo_Type_Proprietaire.SelectedItem as CsProprietaire;
                    if (TypeProprietaire != null)
                    {
                        laDetailDemande.LeClient.PROPRIO = TypeProprietaire.CODE;
                        laDetailDemande.LeClient.FK_IDPROPRIETAIRE = TypeProprietaire.PK_ID;
                    }
                }
                else
                {
                    laDetailDemande.LeClient.PROPRIO = SessionObject.Enumere.LOCATAIRE  ;
                    laDetailDemande.LeClient.FK_IDPROPRIETAIRE = 2;
                }
                #region Sylla 24/09/2015

                if (Cbo_Type_Client.SelectedItem != null)
                {
                    var TypeClient = Cbo_Type_Client.SelectedItem as CsTypeClient;
                    if (TypeClient != null)
                    {
                        //laDetailDemande.LeClient.NATIONNALITE = TypeClient.CODE;
                        laDetailDemande.LeClient.FK_TYPECLIENT = TypeClient.PK_ID;
                    }
                }

                //if (Txt_usage.Tag != null)
                //    laDetailDemande.LeClient.FK_IDUSAGE = (int)Txt_usage.Tag;
                laDetailDemande.LeClient.FK_IDUSAGE = null;

                #endregion
                if (this.txt_Reglage.Tag != null)
                {
                    laDetailDemande.LaDemande.REGLAGECOMPTEUR = this.Btn_Reglage.Tag.ToString();
                    laDetailDemande.LaDemande.FK_IDREGLAGECOMPTEUR = (int)this.txt_Reglage.Tag;
                }

                laDetailDemande.LeClient.TELEPHONEFIXE = string.IsNullOrEmpty(this.txt_Telephone_Fixe.Text) ? null : this.txt_Telephone_Fixe.Text;
                laDetailDemande.LeClient.FAX = string.IsNullOrEmpty(this.Txt_NumFax.Text) ? null : this.Txt_NumFax.Text;
                laDetailDemande.LeClient.BOITEPOSTAL = string.IsNullOrEmpty(this.Txt_BoitePostale.Text) ? null : this.Txt_BoitePostale.Text;
                laDetailDemande.LeClient.TELEPHONE = string.IsNullOrEmpty(this.txt_Telephone.Text) ? null : this.txt_Telephone.Text;
                laDetailDemande.LeClient.NUMPROPRIETE = string.IsNullOrEmpty(this.txtPropriete.Text) ? null : this.txtPropriete.Text;
                laDetailDemande.LeClient.CENTRE = string.IsNullOrEmpty(laDetailDemande.LaDemande.CENTRE) ? null : laDetailDemande.LaDemande.CENTRE;
                laDetailDemande.LeClient.REFCLIENT = string.IsNullOrEmpty(laDetailDemande.LaDemande.CLIENT) ? null : laDetailDemande.LaDemande.CLIENT;

                if (((CsTdem)txt_tdem.Tag).CODE == SessionObject.Enumere.BranchementAbonement ||
                        ((CsTdem)txt_tdem.Tag).CODE == SessionObject.Enumere.BranchementSimple ||
                     ((CsTdem)txt_tdem.Tag).CODE == SessionObject.Enumere.BranchementAbonnementMt )
                    this.txt_ordre.Text = "01";

                laDetailDemande.LeClient.ORDRE = this.txt_ordre.Text;
                laDetailDemande.LeClient.FK_IDCENTRE = laDetailDemande.LaDemande.FK_IDCENTRE;
                laDetailDemande.LeClient.DATECREATION = DateTime.Now;
                laDetailDemande.LeClient.USERCREATION = UserConnecte.matricule;
                laDetailDemande.LeClient.ISFACTUREEMAIL = chk_Email.IsChecked.Value;
                laDetailDemande.LeClient.ISFACTURESMS = chk_SMS.IsChecked.Value;

                if (!string.IsNullOrEmpty(this.Txt_Civilite.Text))
                    laDetailDemande.LeClient.DENABON = this.Txt_Civilite.Text.ToString().PadLeft(6, '0');

                if (!string.IsNullOrWhiteSpace(this.Txt_Email.Text))
                {
                    if (IsEmail(this.Txt_Email.Text))
                    {
                        laDetailDemande.LeClient.EMAIL = string.IsNullOrEmpty(this.Txt_Email.Text) ? null : this.Txt_Email.Text;
                    }
                    else
                    {
                        Message.Show("Veuillez saisi un email client correct", "Erreur");
                        this.Btn_Transmettre.IsEnabled = true;
                    }
                }

                laDetailDemande.LeClient.ADRMAND1 = txtAdresse.Text;
                if (this.Txt_CodeConso.Tag != null)
                {
                    laDetailDemande.LeClient.CODECONSO = this.Txt_CodeConso.Text;
                    laDetailDemande.LeClient.FK_IDCODECONSO = (int)this.Txt_CodeConso.Tag;
                }
                if (this.Txt_CodeRegroupement.Tag != null)
                {
                    laDetailDemande.LeClient.REGROUPEMENT = this.Txt_CodeRegroupement.Text;
                    laDetailDemande.LeClient.FK_IDREGROUPEMENT = (int)this.Txt_CodeRegroupement.Tag;
                }

                if (TxtCategorieClient.Tag != null)
                {
                    laDetailDemande.LeClient.FK_IDCATEGORIE = (int)TxtCategorieClient.Tag;
                    laDetailDemande.LeClient.CATEGORIE = TxtCategorieClient.Text;
                    if (laDetailDemande.LeClient.CATEGORIE == SessionObject.Enumere.CategorieAgentEdm && string.IsNullOrEmpty(this.txt_MaticuleAgent.Text))
                        throw new Exception("Le matricule est obligatoire pour les agents EDM");
                    laDetailDemande.LeClient.MATRICULE = this.txt_MaticuleAgent.Text;
                }
                laDetailDemande.LeClient.CODEIDENTIFICATIONNATIONALE = string.IsNullOrEmpty(this.Txt_Numeronina.Text) ? null : this.Txt_Numeronina.Text;
                laDetailDemande.LeClient.FK_IDRELANCE = 1;
                laDetailDemande.LeClient.CODERELANCE = "0";
                laDetailDemande.LeClient.MODEPAIEMENT = "0";
                laDetailDemande.LeClient.FK_IDMODEPAIEMENT = 1;

                if (Cbo_Type_Client.SelectedItem != null)
                {
                    if (((CsTypeClient)Cbo_Type_Client.SelectedItem).CODE != null)
                    {
                        string codetypeclient = ((CsTypeClient)Cbo_Type_Client.SelectedItem).CODE;
                        switch (codetypeclient.Trim())
                        {
                            case "001":
                                #region Personne Physique
                                GetPersonnPhyqueData(laDetailDemande);
                                #endregion
                                break;
                            case "002":
                                if (GetSocietePriveData(laDetailDemande) == null)
                                    return;
                                break;
                            case "003":
                                GetAdministraionInstitutData(laDetailDemande);
                                break;
                            default:
                                break;
                        }
                    }
                }
                #endregion
                #region Doc Scanne
                if (laDetailDemande.ObjetScanne == null) laDetailDemande.ObjetScanne = new List<ObjDOCUMENTSCANNE>();
                laDetailDemande.ObjetScanne.AddRange(LstPiece);
                #endregion
                #region Compteur et evenement
                     if (laDetailDemande.LstCanalistion == null)
                        laDetailDemande.LstCanalistion = new List<CsCanalisation>();
                    if (this.dtpPose.SelectedDate == null && string.IsNullOrEmpty(this.TxtperiodePose.Text) && string.IsNullOrEmpty(this.Txt_TypeBrancehment.Text ))
                    {
                        Message.Show("Verifier que les champs : date de pose,periode et typebranchement sont renseignés", "Creation ");
                        return;
                    }
                    if (laDetailDemande.LaDemande.PRODUIT == SessionObject.Enumere.ElectriciteMT && Cbo_Puissance.SelectedItem == null)
                        Message.Show("Veillez sélectionnez la puissance installée", "Creation ");

                    laDetailDemande.Branchement = new CsBrt() 
                    {
                        CENTRE = laDetailDemande.LaDemande.CENTRE,
                        CLIENT = laDetailDemande.LaDemande.CLIENT,
                        NUMDEM  = laDetailDemande.LaDemande.NUMDEM ,
                        FK_IDCENTRE = laDetailDemande.LaDemande.FK_IDCENTRE,
                        FK_IDDEMANDE = laDetailDemande.LaDemande.PK_ID ,
                        FK_IDPRODUIT = laDetailDemande.LaDemande.FK_IDPRODUIT.Value,
                        PRODUIT = laDetailDemande.LaDemande.PRODUIT,
                        LONGBRT =string.IsNullOrEmpty( this.Txt_Distance.Text)? 1 : int.Parse(this.Txt_Distance.Text),
                        FK_IDTYPEBRANCHEMENT = this.Txt_TypeBrancehment.Tag == null ? laDetailDemande.Branchement.FK_IDTYPEBRANCHEMENT : int.Parse(this.Txt_TypeBrancehment.Tag.ToString()),
                        CODEBRT = string.IsNullOrEmpty(this.Txt_TypeBrancehment.Text) ? null : this.Txt_TypeBrancehment.Text,
                        NBPOINT = (laDetailDemande.LaDemande.PRODUIT == SessionObject.Enumere.ElectriciteMT)? 6 : 1 ,
                        USERCREATION = UserConnecte.matricule,
                        DATECREATION = System.DateTime.Now 
                    };
                    laDetailDemande.Branchement.PUISSANCEINSTALLEE = Cbo_Puissance.SelectedItem != null ? ((CsPuissance)Cbo_Puissance.SelectedItem).VALEUR  : 0;
                    laDetailDemande.LstCanalistion.AddRange(((List<CsCanalisation>)this.dg_compteur.ItemsSource).ToList());
                    laDetailDemande.LstCanalistion.ForEach(y => y.PERIODE = ClasseMEthodeGenerique.FormatPeriodeAAAAMM(this.TxtperiodePose.Text));
                    laDetailDemande.LstCanalistion.ForEach(y => y.POSE = this.dtpPose.SelectedDate);
                    laDetailDemande.LstCanalistion.ForEach(y => y.DEPOSE  = null );
                    laDetailDemande.LstCanalistion.ForEach(y => y.PROPRIO = "1");
                    laDetailDemande.LstCanalistion.ForEach(y => y.FK_IDPROPRIETAIRE = 2);
                    laDetailDemande.LstCanalistion.ForEach(y => y.CLIENT = laDetailDemande.LaDemande.CLIENT);

                    foreach (var leCompteur in laDetailDemande.LstCanalistion)
                    {
                        CsEvenement leEvtPose = new CsEvenement();
                        leEvtPose.NUMDEM = laDetailDemande.LaDemande.NUMDEM;
                        leEvtPose.CENTRE = laDetailDemande.LaDemande.CENTRE;
                        leEvtPose.CLIENT = laDetailDemande.LaDemande.CLIENT;
                        leEvtPose.ORDRE = laDetailDemande.LaDemande.ORDRE;
                        leEvtPose.PRODUIT = laDetailDemande.LaDemande.PRODUIT;
                        if (laDetailDemande.LaDemande.PRODUIT != SessionObject.Enumere.ElectriciteMT)
                        {
                            leCompteur.POINT = 1;
                            leEvtPose.POINT = 1;
                        }
                        else
                            leEvtPose.POINT = leCompteur.POINT;

                        leEvtPose.FK_IDPRODUIT = laDetailDemande.LaDemande.FK_IDPRODUIT.Value;
                        leEvtPose.FK_IDCENTRE = laDetailDemande.LaDemande.FK_IDCENTRE;
                        leEvtPose.MATRICULE = laDetailDemande.LaDemande.MATRICULE;

                        leEvtPose.REGLAGECOMPTEUR = leCompteur.REGLAGECOMPTEUR;
                        leEvtPose.TYPECOMPTEUR = leCompteur.TYPECOMPTEUR;
                        leEvtPose.COMPTEUR = leCompteur.NUMERO;
                        leEvtPose.CATEGORIE = laDetailDemande.LeClient.CATEGORIE;
                        leEvtPose.USERCREATION = UserConnecte.matricule;
                        leEvtPose.USERMODIFICATION = UserConnecte.matricule;
                        leEvtPose.DATECREATION = System.DateTime.Now;
                        leEvtPose.DATEMODIFICATION = System.DateTime.Now;
                        leEvtPose.CAS = leCompteur.CAS;
                        leEvtPose.FK_IDCANALISATION = null;
                        leEvtPose.FK_IDABON = null;

                        leEvtPose.CODEEVT = SessionObject.Enumere.EvenementCodeNormale;
                        leEvtPose.STATUS = SessionObject.Enumere.EvenementPurger ;
                        leEvtPose.DATEEVT = leCompteur.POSE;
                        leEvtPose.INDEXEVT = leCompteur.INDEXEVT;
                        leEvtPose.PERIODE = Shared.ClasseMEthodeGenerique.FormatPeriodeAAAAMM(leCompteur.PERIODE);

                        leEvtPose.INDEXPRECEDENTEFACTURE = leCompteur.INDEXEVT;
                        leEvtPose.PERIODEPRECEDENTEFACTURE  = leEvtPose.PERIODE;
                        leEvtPose.TYPECOMPTAGE = laDetailDemande.Branchement.TYPECOMPTAGE;

                        leEvtPose.COEFK1 = SessionObject.LstPuissanceInstalle.FirstOrDefault(t => t.PRODUIT == laDetailDemande.Branchement.PRODUIT && t.VALEUR == laDetailDemande.Branchement.PUISSANCEINSTALLEE) == null ? null : SessionObject.LstPuissanceInstalle.FirstOrDefault(t => t.VALEUR == laDetailDemande.Branchement.PUISSANCEINSTALLEE).KPERTEACTIVE1;
                        leEvtPose.COEFK2 = SessionObject.LstPuissanceInstalle.FirstOrDefault(t => t.PRODUIT == laDetailDemande.Branchement.PRODUIT && t.VALEUR == laDetailDemande.Branchement.PUISSANCEINSTALLEE) == null ? null : SessionObject.LstPuissanceInstalle.FirstOrDefault(t => t.VALEUR == laDetailDemande.Branchement.PUISSANCEINSTALLEE).KPERTEACTIVE2;
                        if (laDetailDemande.LstEvenement != null && laDetailDemande.LstEvenement.Count != 0)
                        {
                            CsEvenement _LaCan = laDetailDemande.LstEvenement.FirstOrDefault(p => p.CAS == leEvtPose.CAS && p.POINT == leEvtPose.POINT);
                            if (_LaCan != null)
                            {
                                _LaCan.DATEEVT = leEvtPose.DATEEVT;
                                _LaCan.INDEXEVT = leCompteur.INDEXEVT;
                                _LaCan.PERIODE = leEvtPose.PERIODE;
                            }
                            else
                                laDetailDemande.LstEvenement.Add(leEvtPose);
                        }
                        else
                        {
                            laDetailDemande.LstEvenement = new List<CsEvenement>();
                            laDetailDemande.LstEvenement.Add(leEvtPose);
                        }
                    }
                #endregion
                AcceuilServiceClient clientDevis = new AcceuilServiceClient(Utility.ProtocoleFacturation (), Utility.EndPoint("Accueil"));
                clientDevis.ValiderMiseAjourDemandeTransitionAsync(laDetailDemande);
                clientDevis.ValiderMiseAjourDemandeTransitionCompleted += (ss, b) =>
                {
                    this.Btn_Enregistrer.IsEnabled = true;
                    this.Btn_Transmettre.IsEnabled = true;
                    if (b.Cancelled || b.Error != null)
                    {
                        string error = b.Error.Message;
                        Message.ShowError(error, Silverlight.Resources.Devis.Languages.txtDevis);
                        return;
                    }
                    if (b.Result != null)
                    {
                        if (!string.IsNullOrEmpty(b.Result.MOTIF))
                        {
                            Message.ShowInformation(b.Result.MOTIF, Silverlight.Resources.Devis.Languages.txtDevis);
                            this.Btn_Transmettre.IsEnabled = true;
                        }
                        else
                        {
                            Message.ShowInformation("Client créé avec succès", Silverlight.Resources.Devis.Languages.txtDevis);
                            laDetailDemande.LaDemande = b.Result;
                            EditerContrat(laDetailDemande);
                            this.DialogResult = false;
                        }
                    }
                    else
                    {
                        Message.ShowError("Erreur a la Mise à jour de la demande", Silverlight.Resources.Devis.Languages.txtDevis);
                        this.Btn_Transmettre.IsEnabled = true ;
                    }
                };
            }
            catch (Exception ex)
            {
                LayoutRoot.Cursor = Cursors.Arrow;
                throw ex;
            }
        }
        private void EditerContrat(CsDemande laDemande)
        {
            CsContrat leContrat = new CsContrat();
            leContrat.AGENCE = laDemande.LaDemande.LIBELLECENTRE;
            leContrat.BOITEPOSTALE = laDemande.LeClient.BOITEPOSTAL;
            leContrat.CATEGORIE = laDemande.LeClient.CATEGORIE;
            leContrat.CENTRE = laDemande.LaDemande.CENTRE;
            leContrat.CLIENT = laDemande.LaDemande.CLIENT;
            leContrat.CODCONSOMATEUR = laDemande.LeClient.CODECONSO;
            leContrat.CODETARIF = laDemande.Abonne == null ? string.Empty : laDemande.Abonne.TYPETARIF;
            leContrat.COMMUNE = ((CsCommune)this.Cbo_Commune.SelectedItem ).LIBELLE ;
            leContrat.LIBELLEPRODUIT = laDemande.LaDemande.LIBELLEPRODUIT;
            leContrat.LONGUEURBRANCHEMENT = laDemande.Branchement.LONGBRT.ToString();

            leContrat.NATUREBRANCHEMENT = Txt_LibelleTypeBranchement.Text ;
            leContrat.NOMCLIENT = laDemande.LeClient.NOMABON;
            leContrat.NOMPROPRIETAIRE = laDemande.Ag.NOMP;
            leContrat.NUMDEMANDE = laDemande.LaDemande.NUMDEM;
            leContrat.ORDRE = laDemande.LaDemande.ORDRE;
            leContrat.PORTE = laDemande.Ag.PORTE;
            leContrat.PUISSANCESOUSCRITE = laDemande.Abonne .PUISSANCE.Value .ToString(SessionObject.FormatMontant);
            leContrat.QUARTIER = ((CsQuartier )this.Cbo_Quartier.SelectedItem).LIBELLE;
            leContrat.QUARTIERCLIENT = ((CsQuartier)this.Cbo_Quartier.SelectedItem).LIBELLE;
            
            leContrat.FRAISTIMBRE = System.DateTime.Today.ToShortDateString();
            leContrat.TOTAL1 = laDemande.Ag.TOURNEE + "   " + laDemande.Ag.ORDTOUR;
            leContrat.NUMEROPIECE = laDemande.LeClient.LIBELLETYPEPIECE + "  N° " + laDemande.LeClient.NUMEROPIECEIDENTITE;
            leContrat.USAGE = laDemande.LeClient.LIBELLECODECONSO;
            leContrat.REGROUPEMENT  = laDemande.LeClient.REGROUPEMENT ;


            leContrat.RUE = laDemande.Ag.LIBELLERUE;
            leContrat.RUECLIENT = laDemande.Ag.LIBELLERUE;
            leContrat.TELEPHONE = laDemande.LeClient.TELEPHONE;
            List<CsContrat> lstContrat = new List<CsContrat>();
            lstContrat.Add(leContrat);

            Utility.ActionDirectOrientation<ServicePrintings.CsContrat, CsContrat>(lstContrat, null, SessionObject.CheminImpression, "CreationClient", "Accueil", true);
        }
        private void FermerFenetre()
        {
            try
            {
                DialogResult = true;
                //Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void RemplirListeDesDiametresExistant()
        {
            try
            {
                if (SessionObject.LstReglageCompteur != null && SessionObject.LstReglageCompteur.Count != 0)
                {
                    _listeDesReglageCompteurExistant = SessionObject.LstReglageCompteur;
                    return;
                }
                AcceuilServiceClient service = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.ChargerReglageCompteurCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    SessionObject.LstReglageCompteur = args.Result;
                    _listeDesReglageCompteurExistant = SessionObject.LstReglageCompteur;

                };
                service.ChargerReglageCompteurAsync();
                service.CloseAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        private void RemplirCategorieClient()
        {
            try
            {
                if (SessionObject.LstCategorie != null && SessionObject.LstCategorie.Count != 0)
                {
                    Cbo_Categorie.Items.Clear();
                    ReloadCategClient(SessionObject.LstCategorie.Where(t=>t.CODE != SessionObject.Enumere.CategorieEp).ToList());
                    return;
                }
                AcceuilServiceClient service = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.RetourneCategorieCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    SessionObject.LstCategorie = args.Result;
                    Cbo_Categorie.Items.Clear();
                    ReloadCategClient(SessionObject.LstCategorie.Where(t => t.CODE != SessionObject.Enumere.CategorieEp).ToList());
                };
                service.RetourneCategorieAsync();
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void ReloadCategClient(List<CsCategorieClient> _lstCategorieClient)
        {
            Cbo_Categorie.ItemsSource = null;
            Cbo_Categorie.ItemsSource = _lstCategorieClient;
            Cbo_Categorie.SelectedValuePath = "PK_ID";
            Cbo_Categorie.DisplayMemberPath = "LIBELLE";
        }

        private void RemplirSecteur()
        {
            try
            {
                if (SessionObject.LstSecteur != null && SessionObject.LstSecteur.Count != 0)
                {
                    Cbo_Secteur.Items.Clear();
                    Cbo_Secteur.ItemsSource = SessionObject.LstSecteur;
                    Cbo_Secteur.SelectedValuePath = "PK_ID";
                    Cbo_Secteur.DisplayMemberPath = "LIBELLE";
                    return;
                }
                else
                {

                    Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                    service.ChargerLesSecteursCompleted += (s, args) =>
                    {
                        if (args != null && args.Cancelled)
                            return;
                        SessionObject.LstSecteur = args.Result;
                        Cbo_Secteur.Items.Clear();
                        Cbo_Secteur.ItemsSource = SessionObject.LstSecteur;
                        Cbo_Secteur.SelectedValuePath = "PK_ID";
                        Cbo_Secteur.DisplayMemberPath = "LIBELLE";
                        return;
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

        private void RemplirNationnalite()
        {
            try
            {
                if (SessionObject.LstDesNationalites.Count != 0)
                {
                    Cbo_Nationnalite.Items.Clear();
                    Cbo_Nationnalite.ItemsSource = SessionObject.LstDesNationalites.OrderBy(t=>t.CODE ).ToList();
                    Cbo_Nationnalite.SelectedValuePath = "PK_ID";
                    Cbo_Nationnalite.DisplayMemberPath = "LIBELLE";

                    Cbo_Nationalite_Proprio.Items.Clear();
                    Cbo_Nationalite_Proprio.ItemsSource = SessionObject.LstDesNationalites.OrderBy(t => t.CODE).ToList();
                    Cbo_Nationalite_Proprio.SelectedValuePath = "PK_ID";
                    Cbo_Nationalite_Proprio.DisplayMemberPath = "LIBELLE";

                    Cbo_Nationnalite.SelectedItem = SessionObject.LstDesNationalites.FirstOrDefault(t => t.CODE == "MLI");
                    Cbo_Nationalite_Proprio.SelectedItem = SessionObject.LstDesNationalites.FirstOrDefault(t => t.CODE == "MLI");
                    return;
                }
                else
                {
                    Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                    service.RetourneNationnaliteCompleted += (s, args) =>
                    {
                        if (args != null && args.Cancelled)
                            return;
                        SessionObject.LstDesNationalites = args.Result;
                        Cbo_Nationnalite.Items.Clear();
                        Cbo_Nationnalite.ItemsSource = SessionObject.LstDesNationalites.OrderBy(t => t.CODE).ToList(); 
                        Cbo_Nationnalite.SelectedValuePath = "PK_ID";
                        Cbo_Nationnalite.DisplayMemberPath = "LIBELLE";


                        Cbo_Nationalite_Proprio.Items.Clear();
                        Cbo_Nationalite_Proprio.ItemsSource = SessionObject.LstDesNationalites.OrderBy(t => t.CODE).ToList();
                        Cbo_Nationalite_Proprio.SelectedValuePath = "PK_ID";
                        Cbo_Nationalite_Proprio.DisplayMemberPath = "LIBELLE";

                        Cbo_Nationnalite.SelectedItem = SessionObject.LstDesNationalites.FirstOrDefault(t => t.CODE == "MLI");
                        Cbo_Nationalite_Proprio.SelectedItem = SessionObject.LstDesNationalites.FirstOrDefault(t => t.CODE == "MLI");
                        return;
                    };
                    service.RetourneNationnaliteAsync();
                    service.CloseAsync();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void RemplirProprietaire()
        {
            try
            {
                if (SessionObject.Lsttypeprop.Count != 0)
                {
                    Cbo_Type_Proprietaire.Items.Clear();
                    Cbo_Type_Proprietaire.ItemsSource = SessionObject.Lsttypeprop;
                    Cbo_Type_Proprietaire.SelectedValuePath = "PK_ID";
                    Cbo_Type_Proprietaire.DisplayMemberPath = "LIBELLE";
                    return;
                }
                else
                {
                    Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                    service.RemplirProprietaireCompleted += (s, args) =>
                    {
                        if (args != null && args.Cancelled)
                            return;
                        SessionObject.Lsttypeprop = args.Result;
                        Cbo_Type_Proprietaire.Items.Clear();
                        Cbo_Type_Proprietaire.ItemsSource = SessionObject.Lsttypeprop;
                        Cbo_Type_Proprietaire.SelectedValuePath = "PK_ID";
                        Cbo_Type_Proprietaire.DisplayMemberPath = "LIBELLE";
                        return;
                    };
                    service.RemplirProprietaireAsync();
                    service.CloseAsync();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private void RemplirCodeConsomateur()
        {
            try
            {
                if (SessionObject.LstCodeConsomateur.Count != 0)
                {
                    Cbo_CodeConso.Items.Clear();
                    Cbo_CodeConso.ItemsSource = SessionObject.LstCodeConsomateur;
                    Cbo_CodeConso.SelectedValuePath = "PK_ID";
                    Cbo_CodeConso.DisplayMemberPath = "LIBELLE";
                    return;
                }
                else
                {
                    Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                    service.RetourneCodeConsomateurCompleted += (s, args) =>
                    {
                        if (args != null && args.Cancelled)
                            return;
                        SessionObject.LstCodeConsomateur = args.Result;
                        Cbo_CodeConso.Items.Clear();
                        Cbo_CodeConso.ItemsSource = SessionObject.LstCodeConsomateur;
                        Cbo_CodeConso.SelectedValuePath = "PK_ID";
                        Cbo_CodeConso.DisplayMemberPath = "LIBELLE";
                        return;
                    };
                    service.RetourneCodeConsomateurAsync();
                    service.CloseAsync();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void RemplirCodeRegroupement()
        {
            try
            {
                if (SessionObject.LstCodeRegroupement.Count != 0)
                {
                    List<CsRegCli> lesRegroupement = new List<CsRegCli>();
                    lesRegroupement.Add(new CsRegCli());
                    lesRegroupement.AddRange(SessionObject.LstCodeRegroupement);
                    Cbo_Regroupement.Items.Clear();
                    Cbo_Regroupement.ItemsSource = lesRegroupement;
                    Cbo_Regroupement.SelectedValuePath = "PK_ID";
                    Cbo_Regroupement.DisplayMemberPath = "NOM";
                }
                else
                {
                    Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                    service.RetourneCodeRegroupementCompleted += (s, args) =>
                    {
                        if (args != null && args.Cancelled)
                            return;
                        List<CsRegCli> lesRegroupement = new List<CsRegCli>();
                        lesRegroupement.Add(new CsRegCli());
                        lesRegroupement.AddRange(SessionObject.LstCodeRegroupement);
                        Cbo_Regroupement.Items.Clear();
                        Cbo_Regroupement.ItemsSource = lesRegroupement;
                        Cbo_Regroupement.SelectedValuePath = "PK_ID";
                        Cbo_Regroupement.DisplayMemberPath = "NOM";
                    };
                    service.RetourneCodeRegroupementAsync();
                    service.CloseAsync();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        private void RemplirPieceIdentite()
        {
            try
            {
                LayoutRoot.Cursor = Cursors.Wait;
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.GetAllPieceIdentiteAsync();
                service.GetAllPieceIdentiteCompleted += (ssender, args) =>
                {
                    if (args.Cancelled || args.Error != null)
                    {
                        LayoutRoot.Cursor = Cursors.Arrow;
                        string error = args.Error.Message;
                        Message.ShowError(error, Galatee.Silverlight.Resources.Devis.Languages.txtDevis);
                        return;
                    }
                    Cbo_TypePiecePersonnePhysique.Items.Clear();
                    Cbo_TypePiecePersonnePhysiqueProprio.Items.Clear();
                    if (args.Result != null && args.Result.Count > 0)
                        foreach (var item in args.Result)
                        {
                            Cbo_TypePiecePersonnePhysique.Items.Add(item);
                            Cbo_TypePiecePersonnePhysiqueProprio.Items.Add(item);
                        }
                    ListeTYpePiece = args.Result;
                    Cbo_TypePiecePersonnePhysiqueProprio.SelectedValuePath = "PK_ID";
                    Cbo_TypePiecePersonnePhysiqueProprio.DisplayMemberPath = "LIBELLE";
                    Cbo_TypePiecePersonnePhysique.SelectedValuePath = "PK_ID";
                    Cbo_TypePiecePersonnePhysique.DisplayMemberPath = "LIBELLE";

                    VerifierTypePiece();
                    LayoutRoot.Cursor = Cursors.Arrow;
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        CsPuissance laPuissance = new CsPuissance();
        private void ChargerPuissance()
        {
            try
            {
             
                    if (SessionObject.LstPuissance != null && SessionObject.LstPuissance.Count != 0)
                    {
                        LstPuissanceMt = SessionObject.LstPuissance;
                        if (this.Cbo_Produit.SelectedItem != null)
                        {
                        LstPuissanceMt = SessionObject.LstPuissance.Where(t => t.PRODUIT == ((CsProduit)this.Cbo_Produit.SelectedItem).CODE).ToList();
                        return;
                        }
                    }
                    Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                    service.ChargerPuissanceSouscriteCompleted += (s, args) =>
                    {
                        if (args != null && args.Cancelled)
                            return;
                        LstPuissanceMt = SessionObject.LstPuissance;

                           if (this.Cbo_Produit.SelectedItem != null)
                            {
                                     SessionObject.LstPuissance = args.Result;
                                  LstPuissanceMt = SessionObject.LstPuissance.Where(t => t.PRODUIT == ((CsProduit)this.Cbo_Produit.SelectedItem).CODE).ToList();
                            }
                    };
                    service.ChargerPuissanceSouscriteAsync();
                    service.CloseAsync();
              
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void ChargerPuissanceTarif()
        {
            try
            {
                if (SessionObject.LstTarifPuissance.Count != 0)
                {
                    LstPuissanceTarif = SessionObject.LstTarifPuissance.Where(p => p.PRODUIT == ((CsProduit )this.Cbo_Produit .SelectedItem).CODE).ToList();
                }
                else
                {
                    AcceuilServiceClient service = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                    service.ChargerTarifPuissanceCompleted += (s, args) =>
                    {
                        if (args != null && args.Cancelled)
                            return;
                        SessionObject.LstTarifPuissance = args.Result;
                        LstPuissanceTarif = SessionObject.LstTarifPuissance.Where(p => p.PRODUIT == ((CsProduit)this.Cbo_Produit.SelectedItem).CODE).ToList();

                    };
                    service.ChargerTarifPuissanceAsync();
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
                {
                    LstForfait = SessionObject.LstForfait;
                    if (this.Cbo_Produit.SelectedItem!= null )
                    LstForfait = SessionObject.LstForfait.Where(p => p.PRODUIT == ((CsProduit)this.Cbo_Produit.SelectedItem).CODE).ToList();
                }
                else
                {
                    AcceuilServiceClient service = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                    service.ChargerForfaitCompleted += (s, args) =>
                    {
                        if (args != null && args.Cancelled)
                            return;

                        SessionObject.LstForfait = args.Result;
                        if (this.Cbo_Produit.SelectedItem != null)
                        LstForfait = SessionObject.LstForfait.Where(p => p.PRODUIT == ((CsProduit)this.Cbo_Produit.SelectedItem).CODE).ToList();
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
        private void ChargerTarifParCategorieMt()
        {
            try
            {
                if (lstDesTarif != null && lstDesTarif.Count != 0)
                    lstDesTarif.Clear();
                if (SessionObject.LstTarifCategorie.Count != 0)
                {
                    if (this.Cbo_Categorie.SelectedItem != null && this.Cbo_Produit.SelectedItem != null)
                    {
                        List<CsTarif> LstTarifCategorie = SessionObject.LstTarifCategorie.Where(p => p.PRODUIT == ((CsProduit)this.Cbo_Produit.SelectedItem).CODE &&
                                                         p.CATEGORIE == ((CsCategorieClient)this.Cbo_Categorie.SelectedItem).CODE).ToList();
                        foreach (var item in LstTarifCategorie)
                            lstDesTarif.Add(item);
                        if (lstDesTarif.Count != 0)
                        {
                            this.Txt_CodeTarif.Text = lstDesTarif.First().CODE;
                            this.Txt_LibelleTarif.Text = lstDesTarif.First().LIBELLE;
                            this.Txt_CodeTarif.Tag = lstDesTarif.First().FK_IDTYPETARIF;
                        }
                    }
                }
                else
                {
                    AcceuilServiceClient service = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                    service.ChargerTarifParCategorieCompleted += (s, args) =>
                    {
                        if (args != null && args.Cancelled)
                            return;
                        SessionObject.LstTarifCategorie = args.Result;
                        if (this.Cbo_Categorie.SelectedItem != null && this.Cbo_Produit.SelectedItem != null)
                        {
                            List<CsTarif> LstTarifCategorie = SessionObject.LstTarifCategorie.Where(p => p.PRODUIT == ((CsProduit)this.Cbo_Produit.SelectedItem).CODE &&
                                p.CATEGORIE == ((CsCategorieClient)this.Cbo_Categorie.SelectedItem).CODE).ToList();
                            foreach (var item in LstTarifCategorie)
                                lstDesTarif.Add(item);
                            if (lstDesTarif.Count != 0)
                            {
                                this.Txt_CodeTarif.Text = lstDesTarif.First().CODE;
                                this.Txt_LibelleTarif.Text = lstDesTarif.First().LIBELLE;
                                this.Txt_CodeTarif.Tag = lstDesTarif.First().FK_IDTYPETARIF;
                            }
                        }
                    };
                    service.ChargerTarifParCategorieAsync();
                    service.CloseAsync();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        List<CsTarif> lstDesTarif = new List<CsTarif>();
        private void ChargerFrequence()
        {
            if (SessionObject.LstFrequence.Count != 0)
            {
                LstFrequence = SessionObject.LstFrequence;
                if (LstFrequence.Count == 1)
                {
                    this.Txt_CodeFrequence.Text = LstFrequence.First().CODE;
                    this.Txt_CodeFrequence.Tag = LstFrequence.First().PK_ID;
                    this.Txt_LibelleFrequence.Text = LstFrequence.First().LIBELLE;
                }
            }
            else
            {
                AcceuilServiceClient service = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.ChargerTousFrequenceCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    SessionObject.LstFrequence = args.Result;
                    LstFrequence = SessionObject.LstFrequence;
                    if (LstFrequence.Count == 1)
                    {
                        this.Txt_CodeFrequence.Text = LstFrequence.First().CODE;
                        this.Txt_CodeFrequence.Tag = LstFrequence.First().PK_ID;
                        this.Txt_LibelleFrequence.Text = LstFrequence.First().LIBELLE;
                    }
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
                {
                    LstMois = SessionObject.LstMois;

                    this.Txt_CodeMoisFacturation.Text = LstMois.First().CODE;
                    this.Txt_CodeMoisFacturation.Tag = LstMois.First().PK_ID;
                    this.Txt_LibMoisFact.Text = LstMois.First().LIBELLE;
                    this.Txt_CodeMoisFacturation.IsReadOnly = true;
                    this.btn_moisdefacturation.IsEnabled = false;


                    this.Txt_CodeMoisIndex.Text = LstMois.First().CODE;
                    this.Txt_CodeMoisIndex.Tag = LstMois.First().PK_ID;
                    this.Txt_LibelleMoisIndex.Text = LstMois.First().LIBELLE;
                    this.Txt_CodeMoisIndex.IsReadOnly = true;
                    this.btn_MoisIndex.IsEnabled = false;
                }
                else
                {
                    AcceuilServiceClient service = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                    service.ChargerTousMoisCompleted += (s, args) =>
                    {
                        if (args != null && args.Cancelled)
                            return;
                        SessionObject.LstMois = args.Result;
                        LstMois = SessionObject.LstMois;
                        this.Txt_CodeMoisFacturation.Text = LstMois.First().CODE;
                        this.Txt_CodeMoisFacturation.Tag = LstMois.First().PK_ID;
                        this.Txt_LibMoisFact.Text = LstMois.First().LIBELLE;
                        this.Txt_CodeMoisFacturation.IsReadOnly = true;
                        this.btn_moisdefacturation.IsEnabled = false;


                        this.Txt_CodeMoisIndex.Text = LstMois.First().CODE;
                        this.Txt_CodeMoisIndex.Tag = LstMois.First().PK_ID;
                        this.Txt_LibelleMoisIndex.Text = LstMois.First().LIBELLE;
                        this.Txt_CodeMoisIndex.IsReadOnly = true;
                        this.btn_MoisIndex.IsEnabled = false;
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
        private void ChargerApplicationTaxe(CsDemande laDemande)
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


        #region sylla le 22/09/2015


        private void RemplirUsage()
        {
            try
            {
                LayoutRoot.Cursor = Cursors.Wait;
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.GetAllUsageAsync();
                service.GetAllUsageCompleted += (ssender, args) =>
                {
                    if (args.Cancelled || args.Error != null)
                    {
                        LayoutRoot.Cursor = Cursors.Arrow;
                        string error = args.Error.Message;
                        Message.ShowError(error, Galatee.Silverlight.Resources.Devis.Languages.txtDevis);
                        return;
                    }
                    Cbo_Usage.Items.Clear();
                    if (args.Result != null && args.Result.Count > 0)
                        lstusage = args.Result;
                    ReloadlstUsage(lstusage);

                    VerifierTypePiece();
                    LayoutRoot.Cursor = Cursors.Arrow;
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void ReloadlstUsage(List<CsUsage> args)
        {
            Cbo_Usage.Items.Clear();
            Cbo_Usage.ItemsSource = null;
            foreach (var item in args)
            {
                Cbo_Usage.Items.Add(item);
            }

            Cbo_Usage.SelectedValuePath = "PK_ID";
            Cbo_Usage.DisplayMemberPath = "LIBELLE";
        }



        private void RemplirTypeClient()
        {
            try
            {
                if (SessionObject.LstTypeClient.Count != 0)
                {
                    Cbo_Type_Client.Items.Clear();
                    ReloadTypeClient(SessionObject.LstTypeClient);
                    VerifierTypePiece();
                }
                LayoutRoot.Cursor = Cursors.Wait;
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.GetAllTypeClientAsync();
                service.GetAllTypeClientCompleted += (ssender, args) =>
                {
                    if (args.Cancelled || args.Error != null)
                    {
                        LayoutRoot.Cursor = Cursors.Arrow;
                        string error = args.Error.Message;
                        Message.ShowError(error, Galatee.Silverlight.Resources.Devis.Languages.txtDevis);
                        return;
                    }
                    Cbo_Type_Client.Items.Clear();
                    SessionObject.LstTypeClient = args.Result;
                    if (args.Result != null && args.Result.Count > 0)
                        ReloadTypeClient(args.Result);

                    VerifierTypePiece();
                    LayoutRoot.Cursor = Cursors.Arrow;
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void ReloadTypeClient(List<CsTypeClient> args)
        {
            Cbo_Type_Client.ItemsSource = null;
            foreach (var item in args)
            {
                if (item.CODE.Trim() == "004".Trim()) continue;
                Cbo_Type_Client.Items.Add(item);
            }
            Cbo_Type_Client.SelectedValuePath = "PK_ID";
            Cbo_Type_Client.DisplayMemberPath = "LIBELLE";
        }


        #endregion

        private void RemplirTourneeExistante()
        {
            try
            {

                if (SessionObject.LstZone != null && SessionObject.LstZone.Count != 0)
                {
                    _listeDesTourneeExistant = SessionObject.LstZone;
                    return;
                }
                LayoutRoot.Cursor = Cursors.Wait;
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.ChargerLesTourneesCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    SessionObject.LstZone = args.Result;
                    _listeDesTourneeExistant = SessionObject.LstZone;

                };
                service.ChargerLesTourneesAsync();
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void RemplirCommune()
        {
            try
            {
                if (SessionObject.LstCommune != null && SessionObject.LstCommune.Count != 0)
                {
                    _listeDesCommuneExistant = SessionObject.LstCommune;
                    return;
                }
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.ChargerCommuneAsync();
                service.ChargerCommuneCompleted += (s, args) =>
                {
                    if (args.Error != null && args.Cancelled)
                        return;
                    SessionObject.LstCommune = args.Result;
                    _listeDesCommuneExistant = SessionObject.LstCommune;

                };
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void RemplirCommuneParCentre(int idcentre)
        {
            try
            {

                if (_listeDesCommuneExistant != null && _listeDesCommuneExistant.Count > 0)
                    _listeDesCommuneExistantCentre = _listeDesCommuneExistant.Where(c => c.FK_IDCENTRE == idcentre) != null ? _listeDesCommuneExistant.Where(c => c.FK_IDCENTRE == idcentre ).ToList() : new List<CsCommune>();
                txt_Commune.Text = string.Empty;
                Cbo_Commune.ItemsSource = _listeDesCommuneExistantCentre;
                Cbo_Commune.IsEnabled = true;
                Cbo_Commune.SelectedValuePath = "PK_ID";
                Cbo_Commune.DisplayMemberPath = "LIBELLE";

                Cbo_Commune.ItemsSource = _listeDesCommuneExistantCentre;
                if (_listeDesCommuneExistantCentre.Count > 0)
                {
                    if (_listeDesCommuneExistantCentre.Count == 1)
                        Cbo_Commune.SelectedItem = _listeDesCommuneExistantCentre[0];
                }
                else
                {
                    Message.ShowError("Aucune commune associé à ce centre", "Info");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void RemplirListeDesQuartierExistant()
        {
            try
            {

                if (SessionObject.LstQuartier != null && SessionObject.LstQuartier.Count != 0)
                {
                    _listeDesQuartierExistant = SessionObject.LstQuartier;
                    return;
                }

                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.ChargerLesQartiersAsync();
                service.ChargerLesQartiersCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    SessionObject.LstQuartier = args.Result;
                    _listeDesQuartierExistant = SessionObject.LstQuartier;
                };
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        List<CsQuartier> ListeQuartierFiltres = new List<CsQuartier>();
        private void RemplirQuartier(int pCommuneId)
        {
            List<CsQuartier> QuartierParDefaut = null;

            this.txt_Quartier.Text = string.Empty;
            try
            {
                QuartierParDefaut = _listeDesQuartierExistant.Where(q => q.FK_IDCOMMUNE == pCommuneId).ToList();
                if (QuartierParDefaut != null && QuartierParDefaut.Count > 0)
                    ListeQuartierFiltres.AddRange(QuartierParDefaut);
                //ListeQuartierFiltres.AddRange(_listeDesQuartierExistant.Where(q => q.FK_IDCOMMUNE == pCommuneId && q.CODE != DataReferenceManager.QuartierInconnu).ToList());

                if (ListeQuartierFiltres.Count > 0)
                    //foreach (var item in ListeQuartierFiltres)
                    //{
                    //    Cbo_Quartier.Items.Add(item);
                    //}
                    Cbo_Quartier.ItemsSource = null;
                Cbo_Quartier.ItemsSource = ListeQuartierFiltres;
                Cbo_Quartier.SelectedValuePath = "PK_ID";
                Cbo_Quartier.DisplayMemberPath = "LIBELLE";
                Cbo_Quartier.ItemsSource = ListeQuartierFiltres;

                //Cbo_Quartier.IsEnabled = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void RemplirRues(int pIdSecteur)
        {
            List<CsRues> ListeRuesFiltrees = new List<CsRues>();
            List<CsRues> RueParDefaut = null;
            this.txt_NumRue.Text = string.Empty;
            try
            {
                RueParDefaut = _listeDesRuesExistant.Where(q => q.CODE == DataReferenceManager.RueInconnue).ToList();
                if (RueParDefaut != null && RueParDefaut.Count > 0)
                    ListeRuesFiltrees.AddRange(RueParDefaut);
                ListeRuesFiltrees.AddRange(_listeDesRuesExistant.Where(q => q.FK_IDSECTEUR == pIdSecteur && q.CODE != DataReferenceManager.RueInconnue).ToList());

                Cbo_Rue.ItemsSource = null;
                Cbo_Rue.ItemsSource = ListeRuesFiltrees;
                Cbo_Rue.SelectedValuePath = "PK_ID";
                Cbo_Rue.DisplayMemberPath = "LIBELLE";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void RemplirListeDesRuesExistant()
        {
            try
            {

                if (SessionObject.LstRues != null && SessionObject.LstRues.Count != 0)
                {
                    _listeDesRuesExistant = SessionObject.LstRues;
                    return;
                }
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.ChargerLesRueDesSecteurCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    SessionObject.LstRues = args.Result;
                    _listeDesRuesExistant = SessionObject.LstRues;
                };
                service.ChargerLesRueDesSecteurAsync();
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void RemplirDiametreCompteur(int pIdProduit)
        {
            List<CsReglageCompteur> ListeDesDiametreFiltrees = null;
            try
            {
                if (_listeDesReglageCompteurExistant != null &&
                    _listeDesReglageCompteurExistant.FirstOrDefault(p => p.FK_IDPRODUIT == pIdProduit) != null)
                {
                    ListeDesDiametreFiltrees = _listeDesReglageCompteurExistant.Where(q => q.FK_IDPRODUIT == pIdProduit).ToList();
                }
                //if (ListeDesDiametreFiltrees != null && ListeDesDiametreFiltrees.Count > 0)

                //    Cbo_ReglageCompteur.ItemsSource = null;
                //Cbo_ReglageCompteur.ItemsSource = ListeDesDiametreFiltrees;
                //Cbo_ReglageCompteur.SelectedValuePath = "PK_ID";
                //Cbo_ReglageCompteur.DisplayMemberPath = "LIBELLE";


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void RemplirStatutJuridique()
        {
            try
            {
                LayoutRoot.Cursor = Cursors.Wait;
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.GetAllStatutJuridiqueAsync();
                service.GetAllStatutJuridiqueCompleted += (ssender, args) =>
                {
                    if (args.Cancelled || args.Error != null)
                    {
                        LayoutRoot.Cursor = Cursors.Arrow;
                        string error = args.Error.Message;
                        Message.ShowError(error, Galatee.Silverlight.Resources.Devis.Languages.txtDevis);
                        return;
                    }
                    Cbo_StatutJuridique.Items.Clear();
                    if (args.Result != null && args.Result.Count > 0)
                        foreach (var item in args.Result)
                        {
                            Cbo_StatutJuridique.Items.Add(item);
                        }
                    ListStatuJuridique = args.Result;
                    Cbo_StatutJuridique.SelectedValuePath = "PK_ID";
                    Cbo_StatutJuridique.DisplayMemberPath = "LIBELLE";

                    LayoutRoot.Cursor = Cursors.Arrow;
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
        private void ChildWindow_Closed(object sender, EventArgs e)
        {
            try
            {
                //this.DialogResult = true;
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.txtDevis);
            }
        }
        private void ChargerPuissanceInstalle()
        {
            try
            {
                if (SessionObject.LstPuissanceInstalle != null && SessionObject.LstPuissanceInstalle.Count != 0)
                {

                    List<Galatee.Silverlight.ServiceAccueil.CsPuissance> _lstPuissance = SessionObject.LstPuissanceInstalle;
                    if (_lstPuissance != null && _lstPuissance.Count != 0)
                    {
                        List<ServiceAccueil.CsPuissance> lesPuissance = _lstPuissance.Where(t => t.PRODUIT == SessionObject.Enumere.ElectriciteMT).ToList();
                        Cbo_Puissance.SelectedValuePath = "FK_IDPUISSANCE";
                        Cbo_Puissance.DisplayMemberPath = "VALEUR";
                        Cbo_Puissance.ItemsSource = lesPuissance;
                        if (lesPuissance != null && lesPuissance.Count == 1)
                        {
                            Cbo_Puissance.SelectedItem = lesPuissance.First();
                            Cbo_Puissance.Tag = lesPuissance.First();
                        }
                        return;
                    }
                }

                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.ChargerPuissanceInstalleCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    SessionObject.LstPuissanceInstalle = args.Result;
                    List<Galatee.Silverlight.ServiceAccueil.CsPuissance> _lstPuissance = SessionObject.LstPuissanceInstalle;
                    if (_lstPuissance != null && _lstPuissance.Count != 0)
                    {
                        List<ServiceAccueil.CsPuissance> lesPuissance = _lstPuissance.Where(t => t.PRODUIT == SessionObject.Enumere.ElectriciteMT).ToList();
                        Cbo_Puissance.SelectedValuePath = "FK_IDPUISSANCE";
                        Cbo_Puissance.DisplayMemberPath = "VALEUR";
                        Cbo_Puissance.ItemsSource = lesPuissance;
                        if (lesPuissance != null && lesPuissance.Count == 1)
                        {
                            Cbo_Puissance.SelectedItem = lesPuissance.First();
                            Cbo_Puissance.Tag = lesPuissance.First();
                        }
                        if (lesPuissance != null && lesPuissance.Count == 1)
                        {
                            Cbo_Puissance.SelectedItem = lesPuissance.First();
                            Cbo_Puissance.Tag = lesPuissance.First();
                        }
                        return;
                    }
                };
                service.ChargerPuissanceInstalleAsync();
                service.CloseAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        private void MakeReadOnlyOrEnabledClientInformation(bool pValue)
        {
            try
            {
                Txt_NumFax.IsReadOnly = pValue;
                Txt_BoitePostale.IsReadOnly = pValue;
                txt_Commune.IsReadOnly = pValue;
                Cbo_Quartier.IsEnabled = pValue;
                Cbo_Rue.IsEnabled = pValue;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void EnabledDemandeDevisInformations(bool pValue)
        {
            try
            {
                //ckbIdentifiable.IsEnabled = pValue;
                //Btn_RechercherClient.IsEnabled = pValue;
                //hyperlinkButtonPropScannee.IsEnabled = pValue;
                dtp_finvalidationProprio.IsEnabled = pValue;
                txtNumeroPieceProprio.IsEnabled = pValue;
                //Cbo_TypePiecePersonnePhysiqueProprio.IsEnabled = pValue;
                dtp_DateNaissanceProprio.IsEnabled = pValue;
                Txt_PrenomProprio_PersonePhysiq.IsEnabled = pValue;
                Txt_NomProprio_PersonePhysiq.IsEnabled = pValue;
                txt_Telephone_Proprio.IsEnabled = pValue;
                Txt_Email_Proprio.IsEnabled = pValue;
                Txt_BoitePosta_Proprio.IsEnabled = pValue;
                Txt_Faxe_Proprio.IsEnabled = pValue;
                Cbo_Nationalite_Proprio.IsEnabled = pValue;
                Cbo_Type_Proprietaire.IsEnabled = pValue;
                //Cbo_Tournee.IsEnabled = pValue;
                Cbo_Categorie.IsEnabled = pValue;
                //txt_Telephone.IsEnabled = pValue;
                Cbo_Secteur.IsEnabled = pValue;
                Cbo_CodeConso.IsEnabled = pValue;
                Cbo_Regroupement.IsEnabled = pValue;
                //txt_NumLot.IsEnabled = pValue;
                Cbo_Usage.IsEnabled = pValue;
                Cbo_Commune.IsEnabled = pValue;
                Cbo_Quartier.IsEnabled = pValue;
                Cbo_Rue.IsEnabled = pValue;
                Cbo_Secteur.IsEnabled = pValue;
                Cbo_Nationnalite.IsEnabled = pValue;

                #region Sylla 24/09/2015
                //tabC_Onglet.IsEnabled = pValue;
                //Cbo_Type_Client.IsEnabled = pValue;
                Txt_NumFax.IsEnabled = pValue;
                Txt_BoitePostale.IsEnabled = pValue;

                cbo_typedoc.IsEnabled = pValue;
                btn_ajoutpiece.IsEnabled = pValue;
                btn_supprimerpiece.IsEnabled = pValue;
                dgListePiece.IsEnabled = pValue;
                Txt_Etage.IsEnabled = pValue;
                Txt_Porte.IsEnabled = pValue;
                #endregion

                //txtNumeroPiece.IsEnabled = pValue;
                txtPropriete.IsEnabled = pValue;
                txtAdresse.IsEnabled = pValue;
                txt_Reglage.IsEnabled = pValue;
                Btn_Reglage.IsEnabled = pValue;

                //hyperlinkButtonPropScannee.IsEnabled = pValue;
                lab_ListeAppareils.IsEnabled = pValue;
                Cbo_ListeAppareils.IsEnabled = pValue;
                Btn_ListeAppareils.IsEnabled = pValue;
                Btn_ListeAppareils.IsEnabled = pValue;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void Cbo_TypeDevis_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ActivationEnFonctionDeTdem();
        }

        private void ActivationEnFonctionDeTdem()
        {
            try
            {
                //if (txt_tdem.Tag != null)
                //{
                if (this.Tdem != SessionObject.Enumere.BranchementAbonement)
                {
                    /*lab_ListeAppareils.Visibility = Visibility.Collapsed;
                    Cbo_ListeAppareils.Visibility = Visibility.Collapsed;
                    Btn_ListeAppareils.Visibility = Visibility.Collapsed;*/

                    lbl_Reglage.Visibility = Visibility.Collapsed;
                    txt_Reglage.Visibility = Visibility.Collapsed;
                    Btn_Reglage.Visibility = Visibility.Collapsed;
                }
                else
                {
                    if (SessionObject.LstTypeClient.Count != 0)
                        this.Cbo_Type_Client.SelectedItem = SessionObject.LstTypeClient.Where(t => t.CODE == "001").ToList();
                    //lbl_RefBranch.Visibility = Visibility.Collapsed;
                    lab_ListeAppareils.Visibility = Visibility.Visible;
                    Cbo_ListeAppareils.Visibility = Visibility.Visible;
                    Btn_ListeAppareils.Visibility = Visibility.Visible;
                    lbl_Reglage.Visibility = Visibility.Visible;
                    txt_Reglage.Visibility = Visibility.Visible;
                    Btn_Reglage.Visibility = Visibility.Visible;
                }
            }

            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.txtDevis);
            }
        }

        private void Cbo_Commune_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (Cbo_Commune.SelectedItem != null)
                {
                    CsCommune commune = Cbo_Commune.SelectedItem as CsCommune;
                    if (commune != null)
                    {
                        Cbo_Commune.SelectedItem = commune;
                        Cbo_Commune.Tag = commune.PK_ID;
                        txt_Commune.Text = commune.CODE ?? string.Empty;
                        RemplirQuartier(commune.PK_ID);
                    }
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.txtDevis);
            }
        }
        private void txt_Commune_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (this.txt_Commune.Text.Length == SessionObject.Enumere.TailleCommune)
                {
                    CsCommune laCommune = _listeDesCommuneExistantCentre.FirstOrDefault(t => t.CODE == this.txt_Commune.Text);
                    if (laCommune != null)
                    {
                        if ((this.Cbo_Commune.SelectedItem != null && (CsCommune)this.Cbo_Commune.SelectedItem != laCommune) || this.Cbo_Commune.SelectedItem == null)
                            this.Cbo_Commune.SelectedItem = laCommune;
                    }
                    else
                    {
                        Message.ShowInformation("Le code saisie n'existe pas", "Accueil");
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.txtDevis);
            }
        }
        private void _Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //ValiderInitialisation(laDetailDemande, true);
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.txtDevis);
            }
        }

        private void Cbo_Quartier_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (Cbo_Quartier.SelectedItem != null)
                {
                    var quartier = Cbo_Quartier.SelectedItem as CsQuartier;
                    if (quartier != null)
                    {
                        txt_Quartier.Text = quartier.CODE ?? string.Empty;
                        this.Cbo_Quartier.Tag = quartier.PK_ID;
                        List<ServiceAccueil.CsSecteur> lstSecteur = SessionObject.LstSecteur.Where(t => t.FK_IDQUARTIER == quartier.PK_ID).ToList();
                        this.Cbo_Secteur.ItemsSource = lstSecteur;
                    }
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.txtDevis);
            }
        }
        private void txt_Quartier_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (this.txt_Quartier.Text.Length == SessionObject.Enumere.TailleQuartier)
                {
                    CsQuartier leQuartier = ListeQuartierFiltres.FirstOrDefault(t => t.CODE == this.txt_Quartier.Text);
                    if (leQuartier != null)
                    {
                        if ((this.Cbo_Quartier.SelectedItem != null && (CsQuartier)this.Cbo_Quartier.SelectedItem != leQuartier) || this.Cbo_Quartier.SelectedItem == null)
                            this.Cbo_Quartier.SelectedItem = leQuartier;
                    }
                    else
                    {
                        Message.ShowInformation("Le code saisie n'existe pas", "Accueil");
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.txtDevis);
            }
        }
        private void Cbo_Rue_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (Cbo_Rue.SelectedItem != null)
                {
                    var Secteur = Cbo_Rue.SelectedItem as CsRues;
                    if (Secteur != null)
                        txt_NumRue.Text = Secteur.CODE ?? string.Empty;
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.txtDevis);
            }
        }
        private void txt_NumRue_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (this.txt_NumRue.Text.Length == SessionObject.Enumere.TailleRue)
                {
                    if (this.Cbo_Secteur.SelectedItem != null)
                    {
                        CsRues laRue = _listeDesRuesExistant.FirstOrDefault(t => t.CODE == this.txt_NumRue.Text && (t.FK_IDSECTEUR == (int)this.Cbo_Secteur.Tag || t.CODE == DataReferenceManager.RueInconnue));
                        if (laRue != null)
                        {
                            if ((this.Cbo_Rue.SelectedItem != null && (CsRues)this.Cbo_Rue.SelectedItem != laRue) || this.Cbo_Rue.SelectedItem == null)
                                this.Cbo_Rue.SelectedItem = laRue;
                        }
                        else
                        {
                            Message.ShowInformation("Le code saisie n'existe pas", "Accueil");
                            return;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.txtDevis);
            }
        }

        private void PropietaireWindows(Visibility stat)
        {
            this.lbl_NomProprio.Visibility = stat;
            this.Txt_NomProprio_PersonePhysiq.Visibility = stat;
            this.lbl_PrenomProprio.Visibility = stat;
            this.Txt_PrenomProprio_PersonePhysiq.Visibility = stat;
            this.lbl_DateNaissanceProprio.Visibility = stat;
            this.dtp_DateNaissanceProprio.Visibility = stat;
            this.lbl_NaturePieceIdentiteProprio.Visibility = stat;
            this.Cbo_TypePiecePersonnePhysiqueProprio.Visibility = stat;
            this.lbl_NumPieceProprio.Visibility = stat;
            this.txtNumeroPieceProprio.Visibility = stat;
            this.lbl_DateFinValiditeProprio.Visibility = stat;
            this.dtp_finvalidationProprio.Visibility = stat;
            this.txt_Telephone_Proprio.Visibility = stat;
            this.Txt_Email_Proprio.Visibility = stat;
            this.label7_Copy4.Visibility = stat;
            this.label7_Copy5.Visibility = stat;
            this.Txt_Faxe_Proprio.Visibility = stat;
            this.Txt_BoitePosta_Proprio.Visibility = stat;
            this.lbl_Nationalite_Copy1.Visibility = stat;
            this.Cbo_Nationalite_Proprio.Visibility = stat;
            this.label7_Copy6.Visibility = stat;
            this.label7_Copy7.Visibility = stat;
        }
        private void VerifierTypePiece()
        {
            try
            {
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void Cbo_Usage_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (Cbo_Usage.SelectedItem != null)
                {
                    var usage = ((CsUsage)Cbo_Usage.SelectedItem);
                    this.Txt_usage.Text = usage.CODE;
                    this.Txt_usage.Tag = usage.PK_ID;
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.txtDevis);
            }
        }
        private void Txt_usage_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (this.Txt_usage.Text.Length == SessionObject.Enumere.TailleUsage)
            {
                CsUsage leUsage = lstusage.FirstOrDefault(t => t.CODE == this.Txt_usage.Text);
                if (leUsage != null)
                {
                    if ((this.Cbo_Usage.SelectedItem != null && (CsUsage)this.Cbo_Usage.SelectedItem != leUsage) || this.Cbo_Usage.SelectedItem == null)
                        this.Cbo_Usage.SelectedItem = leUsage;
                }
                else
                {
                    Message.ShowInformation("Le code saisie n'existe pas", "Accueil");
                    return;
                }
            }
        }
        private void ReloadCategorieClientForUsage(CsUsage usage)
        {
            var myls = LstCategorieClient_Usage.Where(ct => ct.FK_IDUSAGE == usage.PK_ID);
            if (myls != null)
            {
                var templst = myls.Select(t => t.FK_IDCATEGORIECLIENT);
                var lst = SessionObject.LstCategorie.Where(t => templst.Contains(t.PK_ID)).ToList();
                ReloadCategClient(lst);

            }
        }

        private string GetClient(int pLongueurClient)
        {
            try
            {
                long client = 1;
                return client.ToString().PadLeft(pLongueurClient, '0');
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public const string MatchEmailPattern =
     @"^(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@"
+ @"((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?
				[0-9]{1,2}|25[0-5]|2[0-4][0-9])\."
+ @"([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?
				[0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|"
+ @"([a-zA-Z0-9]+[\w-]+\.)+[a-zA-Z]{1}[a-zA-Z0-9-]{1,23})$";

        public static bool IsEmail(string email)
        {
            if (email != null) return Regex.IsMatch(email, MatchEmailPattern);
            else return false;
        }


        private void GetAdministraionInstitutData(CsDemande laDetailDemande)
        {
            laDetailDemande.AdministrationInstitut = new CsAdministration_Institut();

            laDetailDemande.AdministrationInstitut.PK_ID = Pk_IdAdministration != 0 ? Pk_IdAdministration : 0;
            laDetailDemande.AdministrationInstitut.FK_IDDEMANDE = laDetailDemande.LaDemande.PK_ID;
            laDetailDemande.AdministrationInstitut.NOMMANDATAIRE = Txt_NomMandataireAdministration.Text;
            laDetailDemande.AdministrationInstitut.PRENOMMANDATAIRE = Txt_PrenomMandataireAdministration.Text;
            laDetailDemande.AdministrationInstitut.RANGMANDATAIRE = Txt_RangMandataireAdministration.Text;
            laDetailDemande.AdministrationInstitut.NOMSIGNATAIRE = Txt_NomSignataireAdministration.Text;
            laDetailDemande.AdministrationInstitut.PRENOMSIGNATAIRE = Txt_PrenomSignataireAdministration.Text;
            laDetailDemande.AdministrationInstitut.RANGSIGNATAIRE = Txt_RangSignataireAdministration.Text;
            laDetailDemande.AdministrationInstitut.NOMABON = Txt_NomClientAdministration.Text;
            laDetailDemande.LeClient.NOMABON = Txt_NomClientAdministration.Text;

        }

        private void GetPersonnPhyqueData(CsDemande laDetailDemande)
        {
            int? mynull = null;
            laDetailDemande.PersonePhysique = new CsPersonePhysique();
            laDetailDemande.PersonePhysique.PK_ID = Pk_IdPersPhys != 0 ? Pk_IdPersPhys : 0;
            laDetailDemande.PersonePhysique.NOMABON = Txt_NomClient_PersonePhysiq.Text;
            laDetailDemande.PersonePhysique.DATEFINVALIDITE = Convert.ToDateTime(dtp_DateValidite.Text);
            laDetailDemande.PersonePhysique.DATENAISSANCE = Convert.ToDateTime(dtp_DateNaissance.Text);
            laDetailDemande.PersonePhysique.NUMEROPIECEIDENTITE = txtNumeroPiece.Text.Trim();
            laDetailDemande.PersonePhysique.FK_IDPIECEIDENTITE = Cbo_TypePiecePersonnePhysique.SelectedItem != null ? ((ObjPIECEIDENTITE)Cbo_TypePiecePersonnePhysique.SelectedItem).PK_ID : mynull;
            laDetailDemande.LeClient.NOMABON = Txt_NomClient_PersonePhysiq.Text;
            laDetailDemande.LeClient.NUMEROPIECEIDENTITE = txtNumeroPiece.Text.Trim();
            laDetailDemande.LeClient.FK_IDPIECEIDENTITE = Cbo_TypePiecePersonnePhysique.SelectedItem != null ? ((ObjPIECEIDENTITE)Cbo_TypePiecePersonnePhysique.SelectedItem).PK_ID : mynull;



        }
        private CsDemande GetSocietePriveData(CsDemande laDetailDemande)
        {
            laDetailDemande.SocietePrives = new CsSocietePrive();
            int? Mynull = null;
            decimal capital = 0;
            if (!decimal.TryParse(Txt_Capital.Text, out capital))
            {
                Message.Show("veuillez saisir une valeur numerique", "Demande");
                return null;
            }
            laDetailDemande.SocietePrives.PK_ID = Pk_IdSocoiete != 0 ? Pk_IdSocoiete : 0;
            laDetailDemande.SocietePrives.NUMEROREGISTRECOMMERCE = Txt_RegistreCommerce.Text;
            laDetailDemande.SocietePrives.FK_IDSTATUTJURIQUE = Cbo_StatutJuridique.SelectedItem != null ? ((CsStatutJuridique)Cbo_StatutJuridique.SelectedItem).PK_ID : Mynull;
            laDetailDemande.SocietePrives.CAPITAL = capital;
            laDetailDemande.SocietePrives.IDENTIFICATIONFISCALE = Txt_IdentiteFiscale.Text;
            laDetailDemande.SocietePrives.DATECREATION = dtp_DateCreation.SelectedDate;
            laDetailDemande.SocietePrives.SIEGE = Txt_Siege.Text;
            laDetailDemande.SocietePrives.NOMMANDATAIRE = Txt_NomMandataire.Text;
            laDetailDemande.SocietePrives.PRENOMMANDATAIRE = Txt_PrenomMandataire.Text;
            laDetailDemande.SocietePrives.RANGMANDATAIRE = Txt_RangMandataire.Text;
            laDetailDemande.SocietePrives.NOMSIGNATAIRE = Txt_NomSignataire.Text;
            laDetailDemande.SocietePrives.PRENOMSIGNATAIRE = Txt_PrenomSignataire.Text;
            laDetailDemande.SocietePrives.RANGSIGNATAIRE = Txt_RangSignataire.Text;
            laDetailDemande.SocietePrives.NOMABON = Txt_NomClientSociete.Text;
            laDetailDemande.LeClient.NOMABON = Txt_NomClientSociete.Text;

            return laDetailDemande;
        }

        private CsDemande GetSocieteProprietaire(CsDemande laDetailDemande)
        {
            laDetailDemande.InfoProprietaire_ = new CsInfoProprietaire();
            int? Mynull = null;

            laDetailDemande.InfoProprietaire_.PK_ID = Pk_IdPropietaire != 0 ? Pk_IdPropietaire : 0;
            laDetailDemande.InfoProprietaire_.FK_IDNATIONNALITE = Cbo_Nationalite_Proprio.SelectedItem != null ? ((Galatee.Silverlight.ServiceAccueil.CsNationalite)Cbo_Nationalite_Proprio.SelectedItem).PK_ID : Mynull;
            laDetailDemande.InfoProprietaire_.BOITEPOSTALE = Txt_BoitePosta_Proprio.Text;
            laDetailDemande.InfoProprietaire_.DATEFINVALIDITE = dtp_finvalidationProprio.SelectedDate;
            laDetailDemande.InfoProprietaire_.DATENAISSANCE = dtp_DateNaissanceProprio.SelectedDate;
            laDetailDemande.InfoProprietaire_.EMAIL = Txt_Email_Proprio.Text;

            if (!string.IsNullOrWhiteSpace(this.Txt_Email_Proprio.Text))
            {
                if (IsEmail(this.Txt_Email_Proprio.Text))
                {
                    laDetailDemande.InfoProprietaire_.EMAIL = string.IsNullOrEmpty(this.Txt_Email_Proprio.Text) ? null : this.Txt_Email_Proprio.Text;
                }
                else
                {
                    Message.Show("Veuillez saisi un email propriétaire correct", "Erreur");
                    return null;
                }
            }
            laDetailDemande.InfoProprietaire_.FAX = Txt_Faxe_Proprio.Text;
            laDetailDemande.InfoProprietaire_.FK_IDCLIENT = laDetailDemande.LeClient.PK_ID;
            laDetailDemande.InfoProprietaire_.FK_IDPIECEIDENTITE = Cbo_TypePiecePersonnePhysiqueProprio.SelectedItem != null ? ((ObjPIECEIDENTITE)Cbo_TypePiecePersonnePhysiqueProprio.SelectedItem).PK_ID : Mynull;
            laDetailDemande.InfoProprietaire_.NOM = Txt_NomProprio_PersonePhysiq.Text;
            laDetailDemande.InfoProprietaire_.PRENOM = Txt_PrenomProprio_PersonePhysiq.Text;
            laDetailDemande.InfoProprietaire_.TELEPHONEMOBILE = txt_Telephone_Proprio.Text;
            laDetailDemande.InfoProprietaire_.NUMEROPIECEIDENTITE = txtNumeroPieceProprio.Text.Trim();


            return laDetailDemande;
        }

        private void Btn_ListeAppareils_Click(object sender, RoutedEventArgs e)
        {
            List<ObjAPPAREILS> listeAppareil = null;
            try
            {
                var UcListAppareils = new Galatee.Silverlight.Devis.UcListAppareils();
                if (Cbo_ListeAppareils.Items.Count > 0)
                {
                    listeAppareil = new List<ObjAPPAREILS>();
                    foreach (ObjAPPAREILS appareil in Cbo_ListeAppareils.Items)
                        listeAppareil.Add(appareil);
                }
                _listeDesReglageCompteurExistant.ForEach(t => t.IsRecommender = false);
                UcListAppareils.AppareilsSelectionnes = listeAppareil;
                UcListAppareils.Closed += new EventHandler(UcListAppareils_Closed);
                UcListAppareils.Show();

            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.txtDevis);
            }
        }

        private void UcListAppareils_Closed(object sender, EventArgs e)
        {
            try
            {
                var lappareils = ((Galatee.Silverlight.Devis.UcListAppareils)sender).AppareilsSelectionnes;
                if (lappareils != null && lappareils.Count > 0)
                    RemplirListeAppareils(lappareils);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private void Btn_Reglage_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if ((CsProduit)this.Cbo_Produit.SelectedItem != null)
                {
                    var UcListReglage = new Galatee.Silverlight.Accueil.UcListeReglageCompteur(_listeDesReglageCompteurExistant.Where(t => t.FK_IDPRODUIT == ((CsProduit)this.Cbo_Produit.SelectedItem).PK_ID).ToList());
                    UcListReglage.Closed += new EventHandler(UcListReglage_Closed);
                    this.IsEnabled = false;
                    UcListReglage.Show();
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.txtDevis);
            }
        }
        CsReglageCompteur leReglageSelect = new CsReglageCompteur();
        CsTcompteur leTypeCompteurSelect = new CsTcompteur();
        private void UcListReglage_Closed(object sender, EventArgs e)
        {
            try
            {
                this.IsEnabled = true;
                Galatee.Silverlight.Accueil.UcListeReglageCompteur ctrs = sender as Galatee.Silverlight.Accueil.UcListeReglageCompteur;
                if (ctrs.isOkClick)
                {
                    if (ctrs.leReglageSelect != null)
                    {
                        this.txt_Reglage.Text = ctrs.leReglageSelect.LIBELLE;
                        this.txt_Reglage.Tag = ctrs.leReglageSelect.PK_ID;
                        this.Btn_Reglage.Tag = ctrs.leReglageSelect.CODE;
                        leReglageSelect = ctrs.leReglageSelect;

                           int idProduit = 0;
                        if (this.Cbo_Produit.SelectedItem != null)
                            idProduit = ((CsProduit)this.Cbo_Produit.SelectedItem).PK_ID;

                        string phase = leReglageSelect.FK_IDPHASE.ToString();
                        leTypeCompteurSelect = SessionObject.LstTypeCompteur.FirstOrDefault(y => y.FK_IDPRODUIT == idProduit && y.CODE == phase);

                        int? idPuissance = null;

                        int idCategorie = 0;
                        if (Cbo_Categorie.SelectedItem != null)
                            idCategorie = (Cbo_Categorie.SelectedItem as CsCategorieClient).PK_ID;


                     

                        ChargerPuissanceEtTarif(idProduit, idPuissance, idCategorie, ctrs.leReglageSelect.PK_ID);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void ChargerPuissanceEtTarif(int idProduit, int? idPuissance, int? idCategorie, int? idReglageCompteur)
        {
            AcceuilServiceClient service = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
            service.ChargerTypeTarifCompleted += (s, args) =>
            {
                if (args != null && args.Cancelled)
                    return;
                LstPuissanceTarif = args.Result;
                if (LstPuissanceMt == null) LstPuissanceMt = new List<CsPuissance>();
                if (LstPuissanceMt.Count != 0) LstPuissanceMt.Clear();

                foreach (CsTarif item in LstPuissanceTarif)
                    LstPuissanceMt.Add(new CsPuissance { PK_ID = item.FK_IDPUISSANCE, CODE = item.PUISSANCE, VALEUR = item.VALEUR });
                if (LstPuissanceMt != null && LstPuissanceMt.Count == 1)
                {
                    this.Txt_CodePussanceSoucrite.Text = LstPuissanceMt.First().CODE;
                    this.Txt_CodePussanceSoucrite.Tag = LstPuissanceMt.First().PK_ID;
                }
                else
                {
                    if (txtProduit.Text != SessionObject.Enumere.Eau)
                    {
                        this.Txt_CodePussanceSoucrite.Text = string.Empty;
                        this.Txt_CodePussanceSoucrite.Tag = null;
                    }
                }

                if (lstDesTarif == null) lstDesTarif = new List<CsTarif>();
                if (lstDesTarif.Count != 0) lstDesTarif.Clear();

                foreach (CsTarif item in LstPuissanceTarif)
                    lstDesTarif.Add(new CsTarif { PK_ID = item.PK_ID, CODE = item.CODE, LIBELLE = item.LIBELLE });
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
            service.ChargerTypeTarifAsync(idProduit, idPuissance, idCategorie, idReglageCompteur,null );
            service.CloseAsync();

        }

        private void Cbo_Categorie_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (Cbo_Categorie.SelectedItem != null)
                {
                    var cat = ((CsCategorieClient)Cbo_Categorie.SelectedItem);
                    ReloadUsageForCateg(cat);

                    if (Cbo_Produit.SelectedItem != null &&
                        ((CsProduit)this.Cbo_Produit.SelectedItem).CODE != SessionObject.Enumere.ElectriciteMT)
                    {
                        int? idPuissance = null;

                        int? idReglageCompteur = null;
                        if (this.txt_Reglage.Tag != null)
                            idReglageCompteur = (int)this.txt_Reglage.Tag;

                        int idProduit = 0;
                        if (this.Cbo_Produit.SelectedItem != null)
                            idProduit = ((CsProduit)this.Cbo_Produit.SelectedItem).PK_ID;
                        ChargerPuissanceEtTarif(idProduit, idPuissance, cat.PK_ID, idReglageCompteur);
                    }
                    else
                        ChargerTarifParCategorieMt();
                        if (cat != null)
                        {
                            TxtCategorieClient.Text = cat.CODE ?? string.Empty;
                            this.TxtCategorieClient.Tag = cat.PK_ID;
                            if (cat.CODE == SessionObject.Enumere.CategorieAgentEdm)
                            {
                                this.txt_MaticuleAgent.Visibility = System.Windows.Visibility.Visible;
                                this.lbl_MatriculeAgent.Visibility = System.Windows.Visibility.Visible;
                            }
                            else
                            {
                                this.txt_MaticuleAgent.Visibility = System.Windows.Visibility.Collapsed;
                                this.lbl_MatriculeAgent.Visibility = System.Windows.Visibility.Collapsed;
                            }
                        }
                   
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.txtDevis);
            }
        }
        private void TxtCategorieClient_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (this.TxtCategorieClient.Text.Length == SessionObject.Enumere.TailleCodeCategorie)
            {
                CsCategorieClient leCateg = SessionObject.LstCategorie.FirstOrDefault(t => t.CODE == this.TxtCategorieClient.Text);
                if (leCateg != null)
                    this.Cbo_Categorie.SelectedItem = leCateg;
                else
                {
                    Message.ShowInformation("Le code saisie n'existe pas", "Accueil");
                    return;
                }
            }
        }

        private void RemplirListeAppareils(List<ObjAPPAREILS> lappareils)
        {
            try
            {
                int sommePuissance = 0;
                decimal intensite = 0;

                Cbo_ListeAppareils.Items.Clear();
                foreach (var item in lappareils)
                {
                    sommePuissance = sommePuissance + (item.NOMBRE * item.PUISSANCE);
                    Cbo_ListeAppareils.Items.Add(item);
                }
                Cbo_ListeAppareils.SelectedValuePath = "CODEAPPAREIL";
                Cbo_ListeAppareils.DisplayMemberPath = "DISPLAYLABEL";
                listAppareilsSelectionnes = lappareils;
                Cbo_ListeAppareils.SelectedIndex = 0;

                if (sommePuissance != 0)
                    intensite = sommePuissance / 220;
                if (laDetailDemande.LaDemande.FK_IDPRODUIT != null)
                {
                    List<CsReglageCompteur> _listeDesDiametrePuissance = _listeDesReglageCompteurExistant.Where(p => p.REGLAGE >= intensite && p.FK_IDPRODUIT == laDetailDemande.LaDemande.FK_IDPRODUIT).ToList();
                    foreach (CsReglageCompteur item in _listeDesDiametrePuissance)
                        item.IsRecommender = true;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void RemplirListeAppareilsDevis(List<ObjAPPAREILSDEVIS> pListAppareilsDevis)
        {
            try
            {

                List<ObjAPPAREILSDEVIS> lAppareilsDevis = new List<ObjAPPAREILSDEVIS>();
                ObjAPPAREILS Appareil = null;
                listAppareilsSelectionnes = new List<ObjAPPAREILS>();
                if (pListAppareilsDevis != null && pListAppareilsDevis.Count > 0)
                {
                    foreach (ObjAPPAREILSDEVIS appareildevis in pListAppareilsDevis)
                    {
                        Appareil = new ObjAPPAREILS();
                        Appareil.PK_ID = appareildevis.FK_IDCODEAPPAREIL;
                        Appareil.PK_IDAPPAREILDEVIS = appareildevis.PK_ID;
                        Appareil.DESIGNATION = appareildevis.DESIGNATION;
                        Appareil.CODEAPPAREIL = appareildevis.CODEAPPAREIL;
                        Appareil.NOMBRE = (int)appareildevis.NBRE;
                        Appareil.PUISSANCE = (int)appareildevis.PUISSANCE;
                        Appareil.DISPLAYLABEL = appareildevis.CODEAPPAREIL + "-" + appareildevis.DESIGNATION + " N " +
                                                 appareildevis.NBRE.ToString() + " P " + appareildevis.PUISSANCE.ToString();
                        listAppareilsSelectionnes.Add(Appareil);
                    }
                }
                Cbo_ListeAppareils.Items.Clear();
                foreach (var item in listAppareilsSelectionnes)
                {
                    Cbo_ListeAppareils.Items.Add(item);
                }
                Cbo_ListeAppareils.SelectedValuePath = "CODEAPPAREIL";
                Cbo_ListeAppareils.DisplayMemberPath = "DISPLAYLABEL";
                Cbo_ListeAppareils.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
  

        private void ReloadTypeclientForCateg(CsCategorieClient cat)
        {


            var myls = LstCategorieClient_TypeClient.Where(ct => ct.FK_IDCATEGORIECLIENT == cat.PK_ID);
            if (myls != null)
            {
                var templst = myls.Select(t => t.FK_IDTYPECLIENT);
                var lsttypecient = SessionObject.LstTypeClient.Where(t => templst.Contains(t.PK_ID)).ToList();
                ReloadTypeClient(lsttypecient);
            }

        }
        private void ReloadUsageForCateg(CsCategorieClient cat)
        {


            var myls = LstCategorieClient_Usage.Where(ct => ct.FK_IDCATEGORIECLIENT == cat.PK_ID);
            if (myls != null)
            {
                var templst = myls.Select(t => t.FK_IDUSAGE);
                var lst = lstusage.Where(t => templst.Contains(t.PK_ID)).ToList();
                ReloadlstUsage(lst);
            }

        }
        private void Cbo_Secteur_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (Cbo_Secteur.SelectedItem != null)
                {
                    var Secteur = Cbo_Secteur.SelectedItem as ServiceAccueil.CsSecteur;
                    if (Secteur != null)
                    {
                        txt_NumSecteur.Text = Secteur.CODE ?? string.Empty;
                        this.Cbo_Secteur.Tag = Secteur.PK_ID;
                        RemplirRues(Secteur.PK_ID);
                    }
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.txtDevis);
            }
        }
        private void txt_NumSecteur_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (this.txt_NumSecteur.Text.Length == SessionObject.Enumere.TailleSecteur)
                {
                    if (this.Cbo_Quartier.SelectedItem != null)
                    {
                        List<ServiceAccueil.CsSecteur> lstSecteur = SessionObject.LstSecteur.Where(t => t.FK_IDQUARTIER == (int)this.Cbo_Quartier.Tag).ToList();
                        CsSecteur leSecteur = lstSecteur.FirstOrDefault(t => t.CODE == this.txt_NumSecteur.Text);
                        if (leSecteur != null)
                        {
                            if ((this.Cbo_Secteur.SelectedItem != null && (CsSecteur)this.Cbo_Secteur.SelectedItem != leSecteur) || this.Cbo_Secteur.SelectedItem == null)
                                this.Cbo_Secteur.SelectedItem = leSecteur;
                        }
                        else
                        {
                            Message.ShowInformation("Le code saisie n'existe pas", "Accueil");
                            return;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.txtDevis);
            }
        }
        private void Cbo_CodeConso_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (Cbo_CodeConso.SelectedItem != null)
                {
                    var conso = ((ServiceAccueil.CsCodeConsomateur)Cbo_CodeConso.SelectedItem);
                    if (conso != null)
                    {
                        Txt_CodeConso.Text = conso.CODE ?? string.Empty;
                        this.Txt_CodeConso.Tag = conso.PK_ID;
                    }
                }
                //ActiverEnregistrerOuTransmettre();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.txtDevis);
            }
        }
        private void Txt_CodeConso_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (this.Txt_CodeConso.Text.Length == SessionObject.Enumere.TailleCodeConso)
            {
                CsCodeConsomateur leCodeSaisi = SessionObject.LstCodeConsomateur.FirstOrDefault(t => t.CODE == this.Txt_CodeConso.Text);
                if (leCodeSaisi != null)
                    this.Cbo_CodeConso.SelectedItem = leCodeSaisi;
                else
                {
                    Message.ShowInformation("Le code saisie n'existe pas", "Accueil");
                    return;
                }
            }
        }

        private void ReloadTypeclientForNature(ServiceAccueil.CsNatureClient Nature)
        {
            var myls = LstNatureClient_TypeClient.Where(ct => ct.FK_IDNATURECLIENT == Nature.PK_ID);
            if (myls != null)
            {
                var templst = myls.Select(t => t.FK_IDTYPECLIENT);
                var lsttypecient = SessionObject.LstTypeClient.Where(t => templst.Contains(t.PK_ID)).ToList();
                ReloadTypeClient(lsttypecient);
            }
        }

        private void Cbo_Regroupement_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (Cbo_Regroupement.SelectedItem != null)
                {
                    var Regroupement = ((ServiceAccueil.CsRegCli)Cbo_Regroupement.SelectedItem);
                    if (Regroupement != null)
                    {
                        Txt_CodeRegroupement.Text = Regroupement.CODE ?? string.Empty;
                       
                    }
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.txtDevis);
            }
        }
        private void Txt_CodeRegroupement_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (this.Txt_CodeRegroupement.Text.Length == SessionObject.Enumere.TailleCodeRegroupement)
            {
                CsRegCli leRegroupement = SessionObject.LstCodeRegroupement.FirstOrDefault(t => t.CODE == this.Txt_CodeRegroupement.Text);
                if (leRegroupement != null)
                {
                    //if ((this.Cbo_Regroupement.SelectedItem != null && (CsRegCli)this.Cbo_Regroupement.SelectedItem != leRegroupement) || this.Cbo_Regroupement.SelectedItem == null)
                        this.Cbo_Regroupement.SelectedItem = leRegroupement;
                        Txt_CodeRegroupement.Tag = leRegroupement.PK_ID;
                }
                else
                {
                    Message.ShowInformation("Le code saisie n'existe pas", "Accueil");
                    return;
                }
            }

        }

        private void Cbo_TypeClient_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                CsTypeClient typeclient = ((CsTypeClient)Cbo_Type_Client.SelectedItem);
                if (typeclient != null)
                {
                    tbControleClient.IsEnabled = true;
                    ReloadCategorieClientFortypeclient(typeclient);
                    switch (typeclient.CODE.Trim())
                    {
                        case "001":
                            {
                                this.tabItemPersoneMoral.Visibility = System.Windows.Visibility.Collapsed;
                                this.tabItemPersoneAdministration.Visibility = System.Windows.Visibility.Collapsed;
                                this.tabItemPersonnePhysique.Visibility = System.Windows.Visibility.Visible;
                                tbControleClient.SelectedItem = this.tabItemPersonnePhysique;
                                break;
                            }
                        case "002":
                            {
                                tabItemPersonnePhysique.Visibility = System.Windows.Visibility.Collapsed;
                                tabItemPersoneAdministration.Visibility = System.Windows.Visibility.Collapsed;
                                tabItemPersoneMoral.Visibility = System.Windows.Visibility.Visible;
                                tbControleClient.SelectedItem = this.tabItemPersoneMoral;
                                break;
                            }
                        case "003":
                            {
                                tabItemPersonnePhysique.Visibility = System.Windows.Visibility.Collapsed;
                                tabItemPersoneMoral.Visibility = System.Windows.Visibility.Collapsed;
                                tabItemPersoneAdministration.Visibility = System.Windows.Visibility.Visible;
                                tbControleClient.SelectedItem = this.tabItemPersoneAdministration;
                                this.Cbo_Nationnalite.SelectedItem = SessionObject.LstDesNationalites.First();
                                break;
                            }
                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.txtDevis);
            }
        }

        private void ReloadCategorieClientFortypeclient(CsTypeClient typeclient)
        {
            var myls = LstCategorieClient_TypeClient.Where(ct => ct.FK_IDTYPECLIENT == typeclient.PK_ID);
            if (myls != null)
            {
                var templst = myls.Select(t => t.FK_IDCATEGORIECLIENT);
                var lst = SessionObject.LstCategorie.Where(t => templst.Contains(t.PK_ID)).ToList();
                ReloadCategClient(lst);

            }
        }




        private void RemplirInfoSocietePrive(CsDemande laDemande)
        {
            if (laDemande.SocietePrives != null)
            {
                Pk_IdSocoiete = laDemande.SocietePrives.PK_ID != null ? laDemande.SocietePrives.PK_ID : 0;
                this.Txt_RegistreCommerce.Text = laDemande.SocietePrives.NUMEROREGISTRECOMMERCE;
                this.Txt_Capital.Text = laDemande.SocietePrives.CAPITAL.ToString();
                this.Txt_IdentiteFiscale.Text = laDemande.SocietePrives.IDENTIFICATIONFISCALE;
                this.Txt_Siege.Text = laDemande.SocietePrives.SIEGE;
                this.Txt_NomMandataire.Text = laDemande.SocietePrives.NOMMANDATAIRE;
                this.Txt_PrenomMandataire.Text = laDemande.SocietePrives.PRENOMMANDATAIRE;
                this.Txt_NomClientSociete.Text = laDemande.SocietePrives.NOMABON;
                this.Txt_RangMandataire.Text = laDemande.SocietePrives.RANGMANDATAIRE;
                this.Txt_NomSignataire.Text = laDemande.SocietePrives.NOMSIGNATAIRE;
                this.Txt_PrenomSignataire.Text = laDemande.SocietePrives.PRENOMSIGNATAIRE;
                this.Txt_RangSignataire.Text = laDemande.SocietePrives.RANGSIGNATAIRE;
                this.dtp_DateCreation.SelectedDate = laDemande.SocietePrives.DATECREATION;
                this.Cbo_StatutJuridique.SelectedItem = ListStatuJuridique.FirstOrDefault(t => t.PK_ID == laDemande.SocietePrives.FK_IDSTATUTJURIQUE);

            }

        }
        private void RemplirInfopersonnephysique(CsDemande laDemande)
        {
            if (laDemande.PersonePhysique != null)
            {
                Pk_IdPersPhys = laDemande.PersonePhysique.PK_ID != null ? laDemande.PersonePhysique.PK_ID : 0;
                this.Txt_NomClient_PersonePhysiq.Text = laDemande.PersonePhysique.NOMABON != null ? laDemande.PersonePhysique.NOMABON : string.Empty;
                this.txtNumeroPiece.Text = laDemande.PersonePhysique.NUMEROPIECEIDENTITE != null ? laDemande.PersonePhysique.NUMEROPIECEIDENTITE : string.Empty;
                this.dtp_DateNaissance.Text = laDemande.PersonePhysique.DATENAISSANCE != null ? laDemande.PersonePhysique.DATENAISSANCE.ToString() : DateTime.Now.ToShortDateString();
                this.dtp_DateValidite.Text = laDemande.PersonePhysique.DATEFINVALIDITE != null ? laDemande.PersonePhysique.DATEFINVALIDITE.ToString() : DateTime.Now.ToShortDateString();
                this.Cbo_TypePiecePersonnePhysique.SelectedItem = ListeTYpePiece.FirstOrDefault(t => t.PK_ID == laDemande.PersonePhysique.FK_IDPIECEIDENTITE);
            }
            else
            {
                Cbo_TypePiecePersonnePhysiqueProprio.IsEnabled = true;
                Cbo_TypePiecePersonnePhysiqueProprio.SelectedItem = SessionObject.LstTypeClient.First();

            }

        }
        private void RemplirInfoAdmnistrationInstitut(CsDemande laDemande)
        {
            if (laDemande.AdministrationInstitut != null)
            {
                Pk_IdAdministration = laDemande.AdministrationInstitut.PK_ID != null ? laDemande.AdministrationInstitut.PK_ID : 0;
                this.Txt_NomMandataireAdministration.Text =!string.IsNullOrEmpty(laDemande.AdministrationInstitut.NOMMANDATAIRE) ? laDemande.AdministrationInstitut.NOMMANDATAIRE : string.Empty ;
                this.Txt_PrenomMandataireAdministration.Text =!string.IsNullOrEmpty(laDemande.AdministrationInstitut.PRENOMMANDATAIRE) ? laDemande.AdministrationInstitut.PRENOMMANDATAIRE:string .Empty ;
                this.Txt_RangMandataireAdministration.Text =!string.IsNullOrEmpty(laDemande.AdministrationInstitut.RANGMANDATAIRE)? laDemande.AdministrationInstitut.RANGMANDATAIRE:string.Empty ;
                this.Txt_NomSignataireAdministration.Text =!string.IsNullOrEmpty(laDemande.AdministrationInstitut.NOMSIGNATAIRE)? laDemande.AdministrationInstitut.NOMSIGNATAIRE: string .Empty ;
                this.Txt_PrenomSignataireAdministration.Text = !string.IsNullOrEmpty(laDemande.AdministrationInstitut.PRENOMSIGNATAIRE)?laDemande.AdministrationInstitut.PRENOMSIGNATAIRE:string.Empty ;
                this.Txt_RangSignataireAdministration.Text =!string.IsNullOrEmpty(laDemande.AdministrationInstitut.RANGSIGNATAIRE)? laDemande.AdministrationInstitut.RANGSIGNATAIRE:string.Empty ;
                this.Txt_NomClientAdministration.Text =!string.IsNullOrEmpty( laDemande.AdministrationInstitut.NOMABON)?laDemande.AdministrationInstitut.NOMABON:string.Empty ;
            }

        }
        private void RemplirInfoPropritaire(CsDemande laDemande)
        {
            if (laDemande.InfoProprietaire_ != null)
            {
                Pk_IdPropietaire = laDemande.InfoProprietaire_.PK_ID != null ? laDemande.InfoProprietaire_.PK_ID : 0;
                this.Txt_NomProprio_PersonePhysiq.Text = laDemande.InfoProprietaire_.NOM != null ? laDemande.InfoProprietaire_.NOM : string.Empty;
                this.Txt_PrenomProprio_PersonePhysiq.Text = laDemande.InfoProprietaire_.PRENOM != null ? laDemande.InfoProprietaire_.PRENOM : string.Empty;
                this.txtNumeroPieceProprio.Text = laDemande.InfoProprietaire_.NUMEROPIECEIDENTITE != null ? laDemande.InfoProprietaire_.NUMEROPIECEIDENTITE : string.Empty;
                this.dtp_finvalidationProprio.SelectedDate = laDemande.InfoProprietaire_.DATEFINVALIDITE != null ? laDemande.InfoProprietaire_.DATEFINVALIDITE : DateTime.Now;
                this.txt_Telephone_Proprio.Text = laDemande.InfoProprietaire_.TELEPHONEMOBILE != null ? laDemande.InfoProprietaire_.TELEPHONEMOBILE : string.Empty;
                this.Txt_Email_Proprio.Text = laDemande.InfoProprietaire_.EMAIL != null ? laDemande.InfoProprietaire_.EMAIL : string.Empty;
                this.Cbo_TypePiecePersonnePhysiqueProprio.SelectedItem = ListeTYpePiece.FirstOrDefault(t => t.PK_ID == laDemande.InfoProprietaire_.FK_IDPIECEIDENTITE) != null ? ListeTYpePiece.FirstOrDefault(t => t.PK_ID == laDemande.InfoProprietaire_.FK_IDPIECEIDENTITE) : null;
                this.dtp_DateNaissanceProprio.SelectedDate = laDemande.InfoProprietaire_.DATENAISSANCE != null ? laDemande.InfoProprietaire_.DATENAISSANCE : DateTime.Now;
                this.Txt_Faxe_Proprio.Text = laDemande.InfoProprietaire_.FAX != null ? laDemande.InfoProprietaire_.FAX : string.Empty;
                this.Txt_BoitePosta_Proprio.Text = laDemande.InfoProprietaire_.BOITEPOSTALE != null ? laDemande.InfoProprietaire_.BOITEPOSTALE : string.Empty;
                this.Cbo_Nationalite_Proprio.SelectedItem = SessionObject.LstDesNationalites.FirstOrDefault(t => t.PK_ID == laDemande.InfoProprietaire_.FK_IDNATIONNALITE) != null ? SessionObject.LstDesNationalites.FirstOrDefault(t => t.PK_ID == laDemande.InfoProprietaire_.FK_IDNATIONNALITE) : null;
            }

        }

        private void Txt_NumDevis_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Txt_RefClient_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Txt_RefBranch_TextChanged(object sender, TextChangedEventArgs e)
        {

        }


        private void chk_Email_Checked(object sender, RoutedEventArgs e)
        {
            if (!chk_Email.IsChecked.Value)
            {
                Txt_Email.Text = string.Empty;
            }
            Txt_Email.IsEnabled = chk_Email.IsChecked.Value;
        }

        private void chk_SMS_Checked(object sender, RoutedEventArgs e)
        {
            if (!chk_SMS.IsChecked.Value)
            {
                txt_Telephone.Text = string.Empty;
            }
            txt_Telephone.IsEnabled = chk_SMS.IsChecked.Value;
        }

        private void tabC_Onglet_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Txt_Capital_TextChanged(object sender, TextChangedEventArgs e)
        {
            decimal capital = 0;
            if (!decimal.TryParse(Txt_Capital.Text, out capital))
            {
                Message.Show("veuillez saisir une valeur numerique", "Demande");
            }
        }

        void allowProgressBar()
        {
            progressBar1.IsEnabled = true;
            progressBar1.Visibility = Visibility.Visible;
            progressBar1.IsIndeterminate = true;
        }

        void desableProgressBar()
        {
            progressBar1.IsIndeterminate = false;
            progressBar1.Visibility = Visibility.Collapsed;
        }
        private void dgListePiece_CurrentCellChanged(object sender, EventArgs e)
        {
            dgListePiece.BeginEdit();
        }
        public ObservableCollection<ObjDOCUMENTSCANNE> LstPiece = new ObservableCollection<ObjDOCUMENTSCANNE>();
        private byte[] image;
        private int Pk_IdPropietaire = 0;

        private int Pk_IdPersPhys = 0;
        private int Pk_IdSocoiete = 0;
        private int Pk_IdAdministration = 0;
        private int PK_Id_Tdem = 0;

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
            if (LstPiece.Count() > 0)
            {
                this.isPreuveSelectionnee = true;
            }
            else
            {
                this.isPreuveSelectionnee = false;
            }
        }

        private void hyperlinkButtonPropScannee__Click(object sender, RoutedEventArgs e)
        {
            MemoryStream memoryStream = new MemoryStream(((HyperlinkButton)sender).Tag as byte[]);
            var ucImageScanne = new UcImageScanne(memoryStream, SessionObject.ExecMode.Modification);
            ucImageScanne.Show();
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
        private void Cbo_Type_Proprietaire_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Cbo_Type_Client.SelectedItem == null)
            {
                Message.ShowInformation("Selectionnez le type de client", "Demande");
                return;
            }
            if (Cbo_Type_Proprietaire.SelectedItem != null)
            {
                var typeproprio = (CsProprietaire)Cbo_Type_Proprietaire.SelectedItem;
                if (typeproprio.CODE == SessionObject.Enumere.LOCATAIRE)
                {
                    tab_proprio.Visibility = Visibility.Visible;
                    PropietaireWindows(System.Windows.Visibility.Visible);
                    this.tbControleClient.SelectedItem = tab_proprio;
                }
                else
                     tab_proprio.Visibility = Visibility.Collapsed;
            }
        }
        private void txt_Telephone_Proprio_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txt_Telephone_Proprio.Text))
            {
                double telephone;
                if (!double.TryParse(txt_Telephone_Proprio.Text, out telephone))
                {
                    Message.Show("Veuillez saisir un numéro de phone mobile proprietaire valide", "Erreur");
                    txt_Telephone_Proprio.Focus();
                }
            }

        }
        private bool VerifieChampObligation()
        {
            try
            {
                bool ReturnValue = true;
              
                    #region InformationClient
                    if ((CsTypeClient)Cbo_Type_Client.SelectedItem == null)
                        throw new Exception("Sélectionnez le type de client");


                    if (((CsTypeClient)Cbo_Type_Client.SelectedItem).CODE.Trim() == "001".Trim())
                    {
                        tabC_Onglet.SelectedItem = tabItemClientInfo;
                        this.tbControleClient.SelectedItem = tabItemPersonnePhysique;
                        if (string.IsNullOrEmpty(this.Txt_NomClient_PersonePhysiq.Text))
                            throw new Exception("Saisir le nom de client");

                        if (string.IsNullOrEmpty(dtp_DateNaissance.Text))
                            throw new Exception("Selectionnez la date de naissance");

                        if (!Shared.ClasseMEthodeGenerique.IsDateValide(dtp_DateNaissance.Text))
                            throw new Exception("La date n'est pas valide");

                        if (Cbo_TypePiecePersonnePhysique.SelectedItem == null)
                            throw new Exception("Selectionnez le type de pièce ");

                        if (string.IsNullOrEmpty(this.txtNumeroPiece.Text))
                            throw new Exception("Saisir le numéro de la pièce ");

                        if (string.IsNullOrEmpty(dtp_DateValidite.Text))
                            throw new Exception("Selectionnez la date de validité de la piéce");

                        if (!Shared.ClasseMEthodeGenerique.IsDateValide(dtp_DateNaissance.Text))
                            throw new Exception("La date n'est pas valide");



                    }
                    if (((CsTypeClient)Cbo_Type_Client.SelectedItem).CODE.Trim() == "002".Trim())
                    {
                        tabC_Onglet.SelectedItem = tabItemClientInfo;
                        this.tbControleClient.SelectedItem = tabItemPersoneMoral;

                        if (string.IsNullOrEmpty(this.Txt_NomClientSociete.Text))
                            throw new Exception("Saisir le nom de client");

                        if (Cbo_Type_Proprietaire.SelectedItem == null)
                            Cbo_Type_Proprietaire.SelectedItem = SessionObject.Lsttypeprop.FirstOrDefault();

                    }
                    if (((CsTypeClient)Cbo_Type_Client.SelectedItem).CODE.Trim() == "003".Trim())
                    {
                        tabC_Onglet.SelectedItem = tabItemClientInfo;
                        this.tbControleClient.SelectedItem = tabItemPersoneAdministration;

                        if (string.IsNullOrEmpty(this.Txt_NomClientAdministration.Text))
                            throw new Exception("Saisir le nom de client");
 
                        if (Cbo_Type_Proprietaire.SelectedItem == null)
                            Cbo_Type_Proprietaire.SelectedItem = SessionObject.Lsttypeprop.FirstOrDefault();
                    }

                    if (this.Cbo_Type_Proprietaire.SelectedItem == null)
                    {
                        tabC_Onglet.SelectedItem = tabItemClientInfo;
                        this.tbControleClient.SelectedItem = tab_AutreInfo;
                        throw new Exception("Séléctionnez si le client est propriétaire ");
                    }
                    if (Cbo_Type_Proprietaire.SelectedItem != null && ((CsProprietaire)Cbo_Type_Proprietaire.SelectedItem).CODE == "1")
                    {
                        tabC_Onglet.SelectedItem = tabItemClientInfo;
                        this.tbControleClient.SelectedItem = tab_proprio;
                        if (string.IsNullOrEmpty(this.Txt_NomProprio_PersonePhysiq.Text))
                            throw new Exception("Saisir le nom du propriétaire ");

                        if (string.IsNullOrEmpty(this.Txt_PrenomProprio_PersonePhysiq.Text))
                            throw new Exception("Saisir le prenom du propriétaire ");

                        if (Cbo_TypePiecePersonnePhysiqueProprio.SelectedItem == null)
                            throw new Exception("Sélectionnez le type de piece du propriétaire ");

                        if (string.IsNullOrEmpty(this.txtNumeroPieceProprio.Text))
                            throw new Exception("Saisir le numéro de la pièce ");
                    }

                    if (this.Cbo_Nationnalite.SelectedItem == null)
                        throw new Exception("Séléctionnez la nationnalité ");

                    if (this.Cbo_Categorie.SelectedItem == null)
                    {
                        string Categ =((CsCategorieClient)this.Cbo_Categorie.SelectedItem).CODE;
                        if (Categ == SessionObject.Enumere.CategorieAgentEdm)
                        {
                            if (string.IsNullOrEmpty( this.txt_MaticuleAgent.Text ))
                            throw new Exception("Entrer le matricule de l'agent ");

                            if (this.Cbo_Regroupement.SelectedItem == null )
                            throw new Exception("Sélectionnez le regroupement ");

                        }
                    }

                    #endregion
                    #region information abonnement

                    tabC_Onglet.SelectedItem = tabItemContrat;
                    if (string.IsNullOrEmpty(this.TxtCategorieClient.Text))
                        throw new Exception("Selectionnez la catégorie du client ");


                    if (string.IsNullOrEmpty(this.Txt_CodeConso.Text))
                        throw new Exception("Selectionnez le code consommateur ");

                    if (string.IsNullOrEmpty(this.Txt_CodeConso.Text))
                        throw new Exception("Selectionnez le code consommateur ");

                    if (((CsProduit)Cbo_Produit.SelectedItem).CODE != SessionObject.Enumere.ElectriciteMT)
                    {
                        if (string.IsNullOrEmpty(this.txt_Reglage.Text))
                            throw new Exception("Selectionnez le calibre ");
                    }
                    else
                    {
                        if ((CsTypeComptage )Cbo_TypeComptage.SelectedItem == null )
                                throw new Exception("Selectionnez le type de comptage");
                    }

                    if (this.TxtCategorieClient.Text == SessionObject.Enumere.CategorieAgentEdm)
                    {
                        if (string.IsNullOrEmpty(this.txt_MaticuleAgent.Text))
                            throw new Exception("Entrer le matricule de l'agent ");
                    }
                    
                    #endregion
                    #region Adresse géographique
                    tabC_Onglet.SelectedItem = tabAdressGeographique;
                    if (string.IsNullOrEmpty(this.txt_Commune.Text))
                        throw new Exception("Séléctionnez la commune ");

                    if (string.IsNullOrEmpty(this.txt_Quartier.Text))
                        throw new Exception("Séléctionnez le quartier ");

                    if (string.IsNullOrEmpty(this.TxtOrdreTournee.Text))
                        throw new Exception("Entrer l' ordre sur la tournée ");

                    if (this.Cbo_Zone.SelectedItem == null )
                        throw new Exception("Sélectionnez la tournée ");
                   
                    #endregion
                    #region Branchement
                    if (string.IsNullOrEmpty(this.Txt_TypeBrancehment .Text))
                        throw new Exception("Sélectionnez le type de branchement ");
                    #endregion
                    return ReturnValue;

            }
            catch (Exception ex)
            {
                this.Btn_Transmettre.IsEnabled = true;
                this.Btn_Enregistrer.IsEnabled = true;
                Message.ShowInformation(ex.Message, "Accueil");
                return false;
            }

        }
        private void Btn_Transmettre_Click(object sender, RoutedEventArgs e)
        {
            

            if (!VerifieChampObligation()) return;
            this.Btn_Enregistrer.IsEnabled = false;
            this.Btn_Transmettre.IsEnabled = false;
            ValidationDevis(null , true);
        }

        private void dtp_DateValidite_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!Shared.ClasseMEthodeGenerique.IsDateValide(dtp_DateValidite.Text) && dtp_DateValidite.Text.Length == SessionObject.Enumere.TailleDate)
            {
                Message.ShowError("La date n'est pas valide", "Accueil");
                this.dtp_DateValidite.Focus();
            }

        }

        private void dtp_DateNaissance_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!Shared.ClasseMEthodeGenerique.IsDateValide(dtp_DateNaissance.Text) && dtp_DateNaissance.Text.Length == SessionObject.Enumere.TailleDate)
            {
                Message.ShowError("La date n'est pas valide", "Accueil");
                this.dtp_DateNaissance.Focus();
            }
        }

        private void btn_tarif_Click(object sender, RoutedEventArgs e)
        {
            int idCentre = 0;
            if (this.txtCentre.Tag != null)
                idCentre = int.Parse( this.txtCentre.Tag.ToString());

            int idCategorie = 0;
            if (Cbo_Categorie.SelectedItem != null)
                idCategorie = (Cbo_Categorie.SelectedItem as CsCategorieClient).PK_ID;

            int idReglageCompteur = 0;
            if (this.txt_Reglage.Tag != null)
                idReglageCompteur = (int)this.txt_Reglage.Tag;

            int idProduit = 0;
            if (Cbo_Produit.SelectedItem != null)
                idProduit = (Cbo_Produit.SelectedItem as CsProduit).PK_ID ;

            ChargerTarifClient(idCentre, idCategorie, idReglageCompteur, null, "0", idProduit);
            dtg_TarifClient.Visibility = System.Windows.Visibility.Visible;
        }

        private void ChargerTarifClient(int idcentre, int idcategorie, int idreglageCompteur, int? idtypecomptage, string propriotaire, int idproduit)
        {
            try
            {
                AcceuilServiceClient client = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                client.RetourneTarifClientCompleted += (ssender, args) =>
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
                        List<CsTarifClient> lstTarif = args.Result;
                        lstTarif.ForEach(t => t.REDEVANCE = t.REDEVANCE + " " + t.TRANCHE.ToString());
                        this.dtg_TarifClient.ItemsSource = null;
                        this.dtg_TarifClient.ItemsSource = lstTarif.Where(t => t.TYPEREDEVANCE != "2").ToList();
                    }
                };
                client.RetourneTarifClientAsync(idcentre, idcategorie, idreglageCompteur, idtypecomptage, propriotaire, idproduit);
            }
            catch (Exception ex)
            {
                Message.ShowInformation("Erreur au chargement des tarif", "Demande");
            }
        }

        private void txtNumeroPiece_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Cbo_TypePiecePersonnePhysique.SelectedItem != null && ((ObjPIECEIDENTITE )Cbo_TypePiecePersonnePhysique.SelectedItem).LIBELLE == SessionObject.CodeNina )
                Txt_Numeronina.Text = txtNumeroPiece.Text; 
        }
       
        private void btn_civilite_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (SessionObject.LstCivilite.Count != 0)
                {
                    this.IsEnabled = false;
                    List<object> _LstObjet = ClasseMEthodeGenerique.RetourneListeObjet(SessionObject.LstCivilite);
                    Galatee.Silverlight.MainView.UcListeGenerique ctr = new Galatee.Silverlight.MainView.UcListeGenerique(_LstObjet, "CODE", "LIBELLE", "Civilité");
                    ctr.Closed += new EventHandler(galatee_OkClickedBtnCivilite);
                    ctr.Show();
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, "Civilité");
            }
        }
        private void galatee_OkClickedBtnCivilite(object sender, EventArgs e)
        {
            try
            {
                this.IsEnabled = true;
                Galatee.Silverlight.MainView.UcListeGenerique ctrs = sender as Galatee.Silverlight.MainView.UcListeGenerique;
                if (ctrs.GetisOkClick)
                {
                    CsDenomination _LeCivilite = (CsDenomination)ctrs.MyObject;
                    this.Txt_Civilite.Text = _LeCivilite.CODE;
                    this.Txt_libelleCivilite.Text = _LeCivilite.LIBELLE ;
                    this.Txt_Civilite.Tag = _LeCivilite.PK_ID;
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, "Civilité");
            }
        }

        private void Txt_CodeTarif_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(this.Txt_CodeTarif.Text) &&
                LstTarif != null && LstTarif.Count != 0 &&
                this.Txt_CodeTarif.Text.Length == SessionObject.Enumere.TailleTarif)
            {

                CsTarif _LeTarif = Shared.ClasseMEthodeGenerique.RetourneObjectFromList(LstTarif, this.Txt_CodeTarif.Text, "CODE");
                if (_LeTarif != null)
                {
                    this.Txt_LibelleTarif.Text = _LeTarif.LIBELLE;
                    this.Txt_CodeTarif.Tag = _LeTarif.PK_ID;

                    this.btn_PussSouscrite.IsEnabled = true;
                    LstPuissanceTarif = SessionObject.LstTarifPuissance.Where(t => t.PK_ID == _LeTarif.PK_ID).ToList();

                }
                else
                {
                    var w = new MessageBoxControl.MessageBoxChildWindow(Galatee.Silverlight.Resources.Accueil.Langue.lbl_Menu, Galatee.Silverlight.Resources.Accueil.Langue.MsgEltInexistent, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
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
                if (lstDesTarif.Count != 0)
                {
                    List<object> _LstObjet = Shared.ClasseMEthodeGenerique.RetourneListeObjet(lstDesTarif);
                    Galatee.Silverlight.MainView.UcListeGenerique ctr = new Galatee.Silverlight.MainView.UcListeGenerique(_LstObjet, "CODE", "LIBELLE", Galatee.Silverlight.Resources.Accueil.Langue.lbl_Menu);
                    ctr.Closed += new EventHandler(galatee_OkClickedBtnTarif);
                    this.IsEnabled = false;
                    ctr.Show();
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message,Galatee.Silverlight.Resources.Accueil.Langue.lbl_Menu);
            }
        }
        private void galatee_OkClickedBtnTarif(object sender, EventArgs e)
        {
            try
            {
                this.IsEnabled = true;
                Galatee.Silverlight.MainView.UcListeGenerique ctrs = sender as Galatee.Silverlight.MainView.UcListeGenerique;
                if (ctrs.isOkClick)
                {
                    CsTarif _LeTarif = (CsTarif)ctrs.MyObject;
                    this.Txt_CodeTarif.Text = _LeTarif.CODE;
                    this.Txt_CodeTarif.Tag = _LeTarif.PK_ID;
                    this.Txt_LibelleTarif.Text = _LeTarif.LIBELLE;
                }

            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Galatee.Silverlight.Resources.Accueil.Langue.lbl_Menu);
            }


        }
        private void btn_PussSouscrite_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (this.Cbo_Produit.SelectedItem != null )
                {
                    if (((CsProduit)this.Cbo_Produit.SelectedItem).CODE != SessionObject.Enumere.ElectriciteMT)
                    {
                        if (LstPuissanceMt.Count != 0)
                        {
                            List<object> _LstObjet = Shared.ClasseMEthodeGenerique.RetourneListeObjet(LstPuissanceMt);
                            Galatee.Silverlight.MainView.UcListeGenerique ctr = new Galatee.Silverlight.MainView.UcListeGenerique(_LstObjet, "CODE", "PUISSANCE", "Liste");
                            ctr.Closed += new EventHandler(galatee_OkClickedBtnpuissanceSouscrite);
                            this.IsEnabled = false;
                            ctr.Show();
                        }
                    }
                    else
                    {
                        if (LstPuissanceMt.Count != 0)
                        {
                            List<object> _LstObjet = Shared.ClasseMEthodeGenerique.RetourneListeObjet(LstPuissanceMt);
                            Galatee.Silverlight.MainView.UcListeGenerique ctr = new Galatee.Silverlight.MainView.UcListeGenerique(_LstObjet, "CODE", "PUISSANCE", "Liste");
                            ctr.Closed += new EventHandler(galatee_OkClickedBtnpuissanceSouscriteMt);
                            this.IsEnabled = false;
                            ctr.Show();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Galatee.Silverlight.Resources.Accueil.Langue.lbl_Menu);
            }
        }
        CsPuissance _LaPuissanceSelectMt = new CsPuissance();
        private void galatee_OkClickedBtnpuissanceSouscriteMt(object sender, EventArgs e)
        {
            try
            {
                this.IsEnabled = true;
                Galatee.Silverlight.MainView.UcListeGenerique ctrs = sender as Galatee.Silverlight.MainView.UcListeGenerique;
                if (ctrs.isOkClick)
                {
                    _LaPuissanceSelectMt = (CsPuissance)ctrs.MyObject;
                    this.Txt_CodePussanceSoucrite.Text = _LaPuissanceSelectMt.VALEUR.ToString();
                    this.Txt_CodePussanceSoucrite.Tag = _LaPuissanceSelectMt.PK_ID;
                }
                this.btn_PussSouscrite.IsEnabled = true;
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Galatee.Silverlight.Resources.Accueil.Langue.lbl_Menu);
            }
        }

        private void galatee_OkClickedBtnpuissanceSouscrite(object sender, EventArgs e)
        {
            try
            {
                this.IsEnabled = true;
                Galatee.Silverlight.MainView.UcListeGenerique ctrs = sender as Galatee.Silverlight.MainView.UcListeGenerique;
                if (ctrs.isOkClick)
                {
                    CsPuissance _LaPuissanceSelect = (CsPuissance)ctrs.MyObject;
                    this.Txt_CodePussanceSoucrite.Text = _LaPuissanceSelect.VALEUR.ToString();
                    this.Txt_CodePussanceSoucrite.Tag = _LaPuissanceSelect.FK_IDPUISSANCE;

                    int idCategorie = 0;
                    if (Cbo_Categorie.SelectedItem != null)
                        idCategorie = (Cbo_Categorie.SelectedItem as CsCategorieClient).PK_ID;

                    int idReglageCompteur = 0;
                    if (this.txt_Reglage.Tag != null)
                        idReglageCompteur = (int)this.txt_Reglage.Tag;

                    int idProduit = 0;
                    if (this.Cbo_Produit.SelectedItem != null)
                        idProduit = ((CsProduit)this.Cbo_Produit.SelectedItem).PK_ID;

                    ChargerPuissanceEtTarif(idProduit, _LaPuissanceSelect.PK_ID, idCategorie, idReglageCompteur);
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Galatee.Silverlight.Resources.Accueil.Langue.lbl_Menu);
            }
        }

        private void Txt_CodePussanceSoucrite_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (LstPuissanceTarif != null && LstPuissanceTarif.Count != 0)
            {
                CsTarif _LaPuissance = Shared.ClasseMEthodeGenerique.RetourneObjectFromList(LstPuissanceTarif, this.Txt_CodePussanceSoucrite.Text, "CODEPUISSANCE");
                if (_LaPuissance != null)
                    this.Txt_CodePussanceSoucrite.Tag = _LaPuissance.PK_ID;
                else
                {
                    var w = new MessageBoxControl.MessageBoxChildWindow(Galatee.Silverlight.Resources.Accueil.Langue.lbl_Menu, Galatee.Silverlight.Resources.Accueil.Langue.MsgEltInexistent, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
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
                if (LstForfait.Count != 0 && this.Txt_CodeForfait.Text.Length == SessionObject.Enumere.TailleForfait)
                {
                    CsForfait _LeForfait = Shared.ClasseMEthodeGenerique.RetourneObjectFromList(LstForfait, this.Txt_CodeForfait.Text, "CODE");
                    if (_LeForfait != null)
                    {
                        this.Txt_LibelleForfait.Text = _LeForfait.LIBELLE;
                        this.Txt_CodeForfait.Tag = _LeForfait.PK_ID;
                    }
                    else
                    {
                        var w = new MessageBoxControl.MessageBoxChildWindow(Galatee.Silverlight.Resources.Accueil.Langue.lbl_Menu, Galatee.Silverlight.Resources.Accueil.Langue.MsgEltInexistent, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
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
                Message.ShowError(ex.Message, Silverlight.Resources.Devis.Languages.txtDevis);
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
                    this.IsEnabled = false;
                    List<object> _LstObjet = Shared.ClasseMEthodeGenerique.RetourneListeObjet(LstForfait);
                    Galatee.Silverlight.MainView.UcListeGenerique ctr = new Galatee.Silverlight.MainView.UcListeGenerique(_LstObjet, "CODE", "LIBELLE", "Liste");
                    ctr.Closed += new EventHandler(galatee_OkClickedBtnForfait);
                    ctr.Show();
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Galatee.Silverlight.Resources.Accueil.Langue.lbl_Menu);
            }
        }
        private void galatee_OkClickedBtnForfait(object sender, EventArgs e)
        {
            try
            {
                this.IsEnabled = true;
                Galatee.Silverlight.MainView.UcListeGenerique ctrs = sender as Galatee.Silverlight.MainView.UcListeGenerique;
                if (ctrs.isOkClick)
                {
                    CsForfait _Leforfait = (CsForfait)ctrs.MyObject;
                    this.Txt_CodeForfait.Text = _Leforfait.CODE;
                    this.Txt_CodeForfait.Tag = _Leforfait.PK_ID;
                }
                this.btn_forfait.IsEnabled = true;
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Galatee.Silverlight.Resources.Accueil.Langue.lbl_Menu);
            }
        }
        private void btn_frequence_Click(object sender, RoutedEventArgs e)
        {
            if (LstFrequence != null && LstFrequence.Count != 0)
            {
                List<object> _ListObj = Shared.ClasseMEthodeGenerique.RetourneListeObjet(LstFrequence);
                this.IsEnabled = false;
                Galatee.Silverlight.MainView.UcListeGenerique ctr = new Galatee.Silverlight.MainView.UcListeGenerique(_ListObj, "CODE", "LIBELLE", "Liste");
                ctr.Closed += new EventHandler(galatee_OkClickedBtnFrequence);
                ctr.Show();
            }
        }
        private void galatee_OkClickedBtnFrequence(object sender, EventArgs e)
        {
            this.IsEnabled = true;
            Galatee.Silverlight.MainView.UcListeGenerique ctrs = sender as Galatee.Silverlight.MainView.UcListeGenerique;
            if (ctrs.isOkClick)
            {
                CsFrequence _LaFrequence = (CsFrequence)ctrs.MyObject;
                this.Txt_CodeFrequence.Text = _LaFrequence.CODE;
                this.Txt_LibelleFrequence.Text = _LaFrequence.LIBELLE;
                this.Txt_CodeFrequence.Tag = _LaFrequence.PK_ID;
            }
            this.btn_frequence.IsEnabled = true;
        }
        private void Txt_CodeFrequence_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (LstFrequence.Count != 0 && this.Txt_CodeFrequence.Text.Length == 1)
                {
                    CsFrequence _LaFrequence = Shared.ClasseMEthodeGenerique.RetourneObjectFromList(LstFrequence, this.Txt_CodeFrequence.Text, "CODE");
                    if (_LaFrequence != null)
                    {
                        this.Txt_LibelleFrequence.Text = _LaFrequence.LIBELLE;
                        this.Txt_CodeFrequence.Tag = _LaFrequence.PK_ID;
                    }
                    else
                    {
                        var w = new MessageBoxControl.MessageBoxChildWindow(Galatee.Silverlight.Resources.Accueil.Langue.lbl_Menu, Galatee.Silverlight.Resources.Accueil.Langue.MsgEltInexistent, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
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
                Message.ShowError(ex.Message, Galatee.Silverlight.Resources.Accueil.Langue.lbl_Menu);
            }
        }
        private void Txt_CodeMoisFacturation_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (LstMois.Count != 0 && this.Txt_CodeMoisFacturation.Text.Length == SessionObject.Enumere.TailleMoisDeFacturation)
                {
                    CsMois _LeMois = Shared.ClasseMEthodeGenerique.RetourneObjectFromList(LstMois, this.Txt_CodeMoisFacturation.Text, "CODE");
                    if (_LeMois != null)
                    {
                        this.Txt_LibMoisFact.Text = _LeMois.LIBELLE;
                        this.Txt_CodeMoisFacturation.Tag = _LeMois.PK_ID;
                    }
                    else
                    {
                        var w = new MessageBoxControl.MessageBoxChildWindow(Galatee.Silverlight.Resources.Accueil.Langue.lbl_Menu, Galatee.Silverlight.Resources.Accueil.Langue.MsgEltInexistent, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
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
                Message.ShowError(ex.Message, Galatee.Silverlight.Resources.Accueil.Langue.lbl_Menu);
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
                    this.IsEnabled = false;
                    List<object> _LstOject = Shared.ClasseMEthodeGenerique.RetourneListeObjet(LstMois);
                    Galatee.Silverlight.MainView.UcListeGenerique ctr = new Galatee.Silverlight.MainView.UcListeGenerique(_LstOject, "CODE", "LIBELLE", "Liste");
                    ctr.Closed += new EventHandler(galatee_OkClickedBtnMoisFact);
                    this.IsEnabled = false;
                    ctr.Show();
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Galatee.Silverlight.Resources.Accueil.Langue.lbl_Menu);
            }
        }
        private void galatee_OkClickedBtnMoisFact(object sender, EventArgs e)
        {
            try
            {
                this.IsEnabled = true;
                Galatee.Silverlight.MainView.UcListeGenerique ctrs = sender as Galatee.Silverlight.MainView.UcListeGenerique;
                if (ctrs.isOkClick)
                {
                    CsMois _LeMois = (CsMois)ctrs.MyObject;
                    this.Txt_CodeMoisFacturation.Text = _LeMois.CODE;
                    this.Txt_CodeMoisFacturation.Tag = _LeMois.PK_ID;
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Galatee.Silverlight.Resources.Accueil.Langue.lbl_Menu);
            }
        }

        private void Txt_CodeMoisIndex_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (LstMois.Count != 0 && this.Txt_CodeMoisIndex.Text.Length == SessionObject.Enumere.TailleMoisDeFacturation)
                {
                    CsMois _LeMois = Shared.ClasseMEthodeGenerique.RetourneObjectFromList(LstMois, this.Txt_CodeMoisIndex.Text, "CODE");
                    if (_LeMois != null)
                    {
                        this.Txt_LibelleMoisIndex.Text = _LeMois.LIBELLE;
                        this.Txt_CodeMoisIndex.Tag = _LeMois.PK_ID;
                    }
                    else
                    {
                        var w = new MessageBoxControl.MessageBoxChildWindow(Galatee.Silverlight.Resources.Accueil.Langue.lbl_Menu, Galatee.Silverlight.Resources.Accueil.Langue.MsgEltInexistent, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
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
                Message.ShowError(ex.Message, Galatee.Silverlight.Resources.Accueil.Langue.lbl_Menu);
            }
        }
        private void btn_MoisIndex_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (LstMois.Count != 0)
                {
                    this.IsEnabled = false;
                    List<object> _LstOject = Shared.ClasseMEthodeGenerique.RetourneListeObjet(LstMois);
                    Galatee.Silverlight.MainView.UcListeGenerique ctr = new Galatee.Silverlight.MainView.UcListeGenerique(_LstOject, "CODE", "LIBELLE", "Liste");
                    ctr.Closed += new EventHandler(galatee_OkClickedBtnMoisIndex);
                    ctr.Show();
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Galatee.Silverlight.Resources.Accueil.Langue.lbl_Menu);
            }
        }
        private void galatee_OkClickedBtnMoisIndex(object sender, EventArgs e)
        {
            try
            {
                this.IsEnabled = true;
                Galatee.Silverlight.MainView.UcListeGenerique ctrs = sender as Galatee.Silverlight.MainView.UcListeGenerique;
                if (ctrs.isOkClick)
                {
                    CsMois _LeMois = (CsMois)ctrs.MyObject;
                    this.Txt_CodeMoisIndex.Text = _LeMois.CODE;
                    this.Txt_CodeMoisIndex.Tag = _LeMois.PK_ID;
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Galatee.Silverlight.Resources.Accueil.Langue.lbl_Menu);
            }
        }
        private void Txt_DateAbonnement_TextChanged(object sender, TextChangedEventArgs e)
        {

            try
            {
                if (this.Txt_DateAbonnement.Text.Length == SessionObject.Enumere.TailleDate)
                    if (Shared.ClasseMEthodeGenerique.IsDateValide(this.Txt_DateAbonnement.Text) == null)
                    {
                        var w = new MessageBoxControl.MessageBoxChildWindow(Galatee.Silverlight.Resources.Accueil.Langue.lbl_Menu, "Date invalide", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        w.OnMessageBoxClosed += (_, result) =>
                        {
                            this.Txt_DateAbonnement.Focus();
                        };
                        w.Show();
                    }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Galatee.Silverlight.Resources.Accueil.Langue.lbl_Menu);
            }
        }
 
        private void dgMyDataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            var dmdRow = e.Row.DataContext as ObjELEMENTDEVIS;
            if (dmdRow != null)
            {
                if (dmdRow.IsCOLORIE)
                {
                    SolidColorBrush SolidColorBrush = new SolidColorBrush(Colors.Green);
                    e.Row.Foreground = SolidColorBrush;
                    e.Row.FontWeight = FontWeights.Bold;
                }
            }
        }

        List<Galatee.Silverlight.ServiceAccueil.CsTypeBranchement> LstTypeBrt;
        List<CsTcompteur> LstTypeCompteur = new List<CsTcompteur>();
        List<CsCalibreCompteur> LstCalibreCompteur = new List<CsCalibreCompteur>();
        List<CsMarqueCompteur> LstMarque = new List<CsMarqueCompteur>();
        private void ChargerTypeBranchement()
        {
            try
            {
                if (SessionObject.LstTypeBranchement != null && SessionObject.LstTypeBranchement.Count != 0)
                {
                    LstTypeBrt = SessionObject.LstTypeBranchement;
                    if (LstTypeBrt != null && LstTypeBrt.Count != 0)
                    {
                        if (!string.IsNullOrEmpty(this.Txt_TypeBrancehment.Text))
                        {

                            Galatee.Silverlight.ServiceAccueil.CsTypeBranchement _LeDiametre = Galatee.Silverlight.Shared.ClasseMEthodeGenerique.RetourneObjectFromList(LstTypeBrt, this.Txt_TypeBrancehment.Text, "CODE");
                            if (_LeDiametre != null && !string.IsNullOrEmpty(_LeDiametre.LIBELLE))
                                this.Txt_LibelleDiametre.Text = _LeDiametre.LIBELLE;
                        }
                    }

                }
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.ChargerTypeBranchementCompleted += (s, args) =>
                {
                    if ((args != null && args.Cancelled) || (args.Error != null))
                        return;
                    SessionObject.LstTypeBranchement = args.Result;
                    LstTypeBrt = SessionObject.LstTypeBranchement;
                    if (LstTypeBrt != null && LstTypeBrt.Count != 0)
                    {
                        if (!string.IsNullOrEmpty(this.Txt_TypeBrancehment.Text))
                        {
                            Galatee.Silverlight.ServiceAccueil.CsTypeBranchement _LeDiametre = Galatee.Silverlight.Shared.ClasseMEthodeGenerique.RetourneObjectFromList(LstTypeBrt, this.Txt_TypeBrancehment.Text, "CODE");
                            if (_LeDiametre != null && !string.IsNullOrEmpty(_LeDiametre.LIBELLE))
                                this.Txt_LibelleDiametre.Text = _LeDiametre.LIBELLE;
                        }
                    }
                };
                service.ChargerTypeBranchementAsync();
                service.CloseAsync();
            }
            catch (Exception es)
            {

                MessageBox.Show(es.Message);
            }

        }
        void ChargerDiametreCompteur()
        {
            try
            {
                if (SessionObject.LstCalibreCompteur.Count != 0)
                    LstCalibreCompteur = SessionObject.LstCalibreCompteur;
                else
                {
                    AcceuilServiceClient service = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                    service.ChargerCalibreCompteurCompleted += (s, args) =>
                    {
                        if (args != null && args.Cancelled)
                            return;
                        LstCalibreCompteur = args.Result;
                        SessionObject.LstCalibreCompteur = LstCalibreCompteur;
                    };
                    service.ChargerCalibreCompteurAsync();
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
                    LstMarque = SessionObject.LstMarque;
                else
                {
                    AcceuilServiceClient service = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                    service.RetourneToutMarqueCompleted += (s, args) =>
                    {
                        if (args != null && args.Cancelled)
                            return;
                        LstMarque = args.Result;
                        SessionObject.LstMarque = LstMarque;
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
        void ChargerTypeComptage()
        {
            try
            {
                if (SessionObject.LstTypeComptage != null && SessionObject.LstTypeComptage.Count != 0)
                    return;
                AcceuilServiceClient service = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.ChargerTypeComptageCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    SessionObject.LstTypeComptage = args.Result;
                };
                service.ChargerTypeComptageAsync();
                service.CloseAsync();
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
                if (SessionObject.LstTypeCompteur.Count != 0)
                    LstTypeCompteur = SessionObject.LstTypeCompteur;
                else
                {
                    AcceuilServiceClient service = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                    service.ChargerTypeCompleted += (s, args) =>
                    {
                        if (args != null && args.Cancelled)
                            return;
                        LstTypeCompteur = args.Result;
                        SessionObject.LstTypeCompteur = LstTypeCompteur;
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


        List<CsCanalisation> lstCannalisation = new List<CsCanalisation>();
        private void btn_Ajouter_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(this.Txt_CodeCadran.Text) &&
                !string.IsNullOrEmpty(this.Txt_CodeMarque.Text) &&
                !string.IsNullOrEmpty(this.Txt_AnneeFab.Text) &&
                !string.IsNullOrEmpty(this.Txt_CodeCadran.Text) &&
                !string.IsNullOrEmpty(this.TxtperiodePose.Text) &&
                Shared.ClasseMEthodeGenerique.IsFormatPeriodeValide( this.TxtperiodePose.Text) &&
                //this.Txt_LibelleDiametre.Tag != null &&
                this.DtpPose != null &&
                this.Txt_CodeMarque.Tag != null &&
                !string.IsNullOrEmpty(this.Txt_NumCompteur.Text))
                if (((CsProduit)this.Cbo_Produit.SelectedItem).CODE  == SessionObject.Enumere.ElectriciteMT)
                {
                    Galatee.Silverlight.ServiceAccueil.CsTypeComptage leTypeComptage = (Galatee.Silverlight.ServiceAccueil.CsTypeComptage)this.Txt_LibelleDiametre.Tag;
                    List<CsCanalisation> lstCanal = new List<CsCanalisation>();
                    for (int i = 1; i <= 6; i++)
                    {
                        Galatee.Silverlight.ServiceAccueil.CsCanalisation canal = new Galatee.Silverlight.ServiceAccueil.CsCanalisation()
                        {
                            CENTRE = ((CsCentre)this.Cbo_Centre.SelectedItem).CODE,
                            ORDRE = "01",
                            FK_IDCENTRE = ((CsCentre)this.Cbo_Centre.SelectedItem).PK_ID ,
                            FK_IDPRODUIT = ((CsProduit)this.Cbo_Produit.SelectedItem).PK_ID ,
                            ANNEEFAB = this.Txt_AnneeFab.Text,
                            CADRAN = byte.Parse(this.Txt_CodeCadran.Text),
                            CAS = SessionObject.Enumere.CasPoseCompteur,
                            NUMERO = this.Txt_NumCompteur.Text,
                            PRODUIT = ((CsProduit)this.Cbo_Produit.SelectedItem).CODE,
                            POINT = i,
                            INDEXEVT = 0,
                            POSE = DtpPose == null ? DateTime.Today.Date : DtpPose ,
                            TYPECOMPTAGE = leTypeComptage.CODE ,
                            FK_IDTYPECOMPTAGE = leTypeComptage.PK_ID ,
                            FK_IDMARQUECOMPTEUR = (int)this.Txt_CodeMarque.Tag,
                            LIBELLEMARQUE = Txt_LibelleMarque.Text,
                            MARQUE = Txt_CodeMarque.Text,
                            LIBELLEREGLAGECOMPTEUR = Txt_LibelleDiametre.Text,
                            FK_IDPROPRIETAIRE = SessionObject.Lsttypeprop.FirstOrDefault(t => t.CODE == SessionObject.Enumere.LOCATAIRE).PK_ID,
                            COEFLECT = 1,
                            PERIODE  = Shared.ClasseMEthodeGenerique.FormatPeriodeAAAAMM( this.TxtperiodePose.Text) ,
                            USERCREATION = UserConnecte.matricule,
                            USERMODIFICATION = UserConnecte.matricule,
                            DATECREATION = System.DateTime.Now,
                            DATEMODIFICATION = System.DateTime.Now,
                        };
                        lstCanal.Add(canal);
                    }
                    lstCannalisation = DataReferenceManager.CodificationCompteurMt(lstCanal);
                }
                else
                {
                    if ((int)this.txt_Reglage.Tag != null && leTypeCompteurSelect != null && !string.IsNullOrEmpty( leTypeCompteurSelect.LIBELLE) )
                    {
                        if (lstCannalisation != null && lstCannalisation.Count != 0) lstCannalisation.Clear();
                        Galatee.Silverlight.ServiceAccueil.CsCalibreCompteur leCalibre = (Galatee.Silverlight.ServiceAccueil.CsCalibreCompteur)this.Txt_LibelleDiametre.Tag;
                        Galatee.Silverlight.ServiceAccueil.CsCanalisation canal = new Galatee.Silverlight.ServiceAccueil.CsCanalisation()
                        {
                            CENTRE = ((CsCentre)this.Cbo_Centre.SelectedItem).CODE,
                            ORDRE = "01",
                            FK_IDCENTRE = ((CsCentre)this.Cbo_Centre.SelectedItem).PK_ID,
                            FK_IDPRODUIT = ((CsProduit)this.Cbo_Produit.SelectedItem).PK_ID,
                            ANNEEFAB = this.Txt_AnneeFab.Text,
                            CADRAN = byte.Parse(this.Txt_CodeCadran.Text),
                            CAS = SessionObject.Enumere.CasPoseCompteur,
                            PRODUIT = ((CsProduit)this.Cbo_Produit.SelectedItem).CODE,
                            NUMERO = this.Txt_NumCompteur.Text,
                            TYPECOMPTEUR = leTypeCompteurSelect.CODE ,
                            POINT = 1,
                            INDEXEVT = 0,
                            COEFLECT = 1,
                            MARQUE = Txt_CodeMarque.Text,
                            FK_IDCALIBRE = leCalibre.PK_ID,
                            FK_IDMARQUECOMPTEUR = (int)this.Txt_CodeMarque.Tag,
                            FK_IDTYPECOMPTEUR = leTypeCompteurSelect.PK_ID ,
                            LIBELLETYPECOMPTEUR = leTypeCompteurSelect.LIBELLE,
                            LIBELLEMARQUE = Txt_LibelleMarque.Text,
                            LIBELLEREGLAGECOMPTEUR = Txt_LibelleDiametre.Text,
                            REGLAGECOMPTEUR = this.Btn_Reglage.Tag.ToString(),
                            FK_IDREGLAGECOMPTEUR = (int)this.txt_Reglage.Tag,
                            POSE = DtpPose == null ? DateTime.Today.Date : DtpPose,
                            FK_IDPROPRIETAIRE = SessionObject.Lsttypeprop.FirstOrDefault(t => t.CODE == SessionObject.Enumere.LOCATAIRE).PK_ID,
                            USERCREATION = UserConnecte.matricule,
                            USERMODIFICATION = UserConnecte.matricule,
                            DATECREATION = System.DateTime.Now,
                            DATEMODIFICATION = System.DateTime.Now,
                        };
                        lstCannalisation.Add(canal);
                    }
                    else
                    {
                        Message.ShowInformation("Sélectionnez le reglage du compteur ", "Accueil");
                        tabC_Onglet.SelectedItem = tabItemContrat;
                    }
                }
            this.dg_compteur.ItemsSource = null;
            this.dg_compteur.ItemsSource = lstCannalisation;
        }
        DateTime DtpPose = new DateTime();
        DateTime DtpDePose = new DateTime();
        private void dtpPose_CalendarClosed(object sender, RoutedEventArgs e)
        {
            if (dtpPose.SelectedDate != null && dtpPose.SelectedDate.Value != null && dg_compteur.ItemsSource != null)
            {
                List<CsCanalisation> lesCompteur = (List<CsCanalisation>)dg_compteur.ItemsSource;
                lesCompteur.ForEach(t => t.POSE = dtpPose.SelectedDate);
                //LoadCompteur(lesCompteur);
                DtpPose = dtpPose.SelectedDate.Value;
                this.TxtperiodePose.Text = dtpPose.SelectedDate.Value.Month.ToString("00") + "/" + dtpPose.SelectedDate.Value.Year;
            }
        }

        private void btn_DiametreCompteur_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.btn_DiametreCompteur.IsEnabled = false;
                if (LstCalibreCompteur.Count != 0)
                {
                    if (((CsProduit)Cbo_Produit .SelectedItem).CODE   == SessionObject.Enumere.ElectriciteMT)
                    {
                        List<object> _LstObject = ClasseMEthodeGenerique.RetourneListeObjet(SessionObject.LstTypeComptage);
                        UcListeGenerique ctr = new UcListeGenerique(_LstObject, "CODE", "LIBELLE", Galatee.Silverlight.Resources.Accueil.Langue.lbl_ListeDiametre);
                        ctr.Closed += new EventHandler(galatee_OkClickedBtntypeComptage);
                        ctr.Show();
                    }
                    else
                    {

                        List<CsCalibreCompteur> lstCalibre = LstCalibreCompteur.Where(t => t.PRODUIT == ((CsProduit)this.Cbo_Produit.SelectedItem).CODE && leReglageSelect.FK_IDPHASE == t.FK_IDPHASE && leReglageSelect.REGLAGEMINI   >= t.REGLAGEMINI && leReglageSelect.REGLAGEMAXI   <= t.REGLAGEMAXI ).ToList();

                        if (lstCalibre != null && lstCalibre.Count == 1)
                        {
                            this.Txt_LibelleDiametre.Text = lstCalibre.FirstOrDefault().LIBELLE;
                            this.Txt_LibelleDiametre.Tag = lstCalibre.FirstOrDefault();
                        }
                        else
                        {
                            List<object> _LstObject = ClasseMEthodeGenerique.RetourneListeObjet(lstCalibre);
                            Dictionary<string, string> _LstColonneAffich = new Dictionary<string, string>();
                            _LstColonneAffich.Add("LIBELLE", "LIBELLE");
                            MainView.UcListeGenerique ctr = new MainView.UcListeGenerique(_LstObject, _LstColonneAffich, false, Galatee.Silverlight.Resources.Accueil.Langue.lbl_ListeDiametre);
                            ctr.Closed += new EventHandler(galatee_OkClickedBtnDiametre);
                            ctr.Show();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Galatee.Silverlight.Resources.Accueil.Langue.lbl_Menu);
            }
        }
        void galatee_OkClickedBtntypeComptage(object sender, EventArgs e)
        {
            try
            {
                this.btn_DiametreCompteur.IsEnabled = true;
                UcListeGenerique ctrs = sender as UcListeGenerique;
                if (ctrs.isOkClick)
                {
                    CsTypeComptage _LeDiametre = (CsTypeComptage)ctrs.MyObject;
                    this.Txt_LibelleDiametre.Text = _LeDiametre.LIBELLE;
                    this.Txt_LibelleDiametre.Tag = _LeDiametre;
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Galatee.Silverlight.Resources.Accueil.Langue.lbl_Menu);
            }
        }
        void galatee_OkClickedBtnDiametre(object sender, EventArgs e)
        {
            try
            {
                this.btn_DiametreCompteur.IsEnabled = true;
                UcListeGenerique ctrs = sender as UcListeGenerique;
                if (ctrs.isOkClick)
                {
                    CsCalibreCompteur _LeDiametre = (CsCalibreCompteur)ctrs.MyObject;
                    this.Txt_LibelleDiametre.Text = _LeDiametre.LIBELLE;
                    this.Txt_LibelleDiametre.Tag = _LeDiametre;
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Galatee.Silverlight.Resources.Accueil.Langue.lbl_Menu);
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
                        this.Txt_CodeMarque.Tag = _LaMarque.PK_ID;
                    }
                    else
                    {
                        var w = new MessageBoxControl.MessageBoxChildWindow(Galatee.Silverlight.Resources.Accueil.Langue.lbl_Menu, Galatee.Silverlight.Resources.Accueil.Langue.MsgEltInexistent, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
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
                Message.ShowError(ex.Message, Galatee.Silverlight.Resources.Accueil.Langue.lbl_Menu);
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
                if (ctrs.isOkClick)
                {
                    CsMarqueCompteur _LaMarque = (CsMarqueCompteur)ctrs.MyObject;
                    this.Txt_CodeMarque.Text = _LaMarque.CODE;
                    this.Txt_CodeMarque.Tag = _LaMarque.PK_ID;

                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Galatee.Silverlight.Resources.Accueil.Langue.lbl_Menu);
            }
        }
 
        private void dtpPose_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(dtpPose.Text))
            {
                DtpPose = Convert.ToDateTime(dtpPose.Text);
                List<CsCanalisation> lesCompteur = (List<CsCanalisation>)dg_compteur.ItemsSource;
                if (lesCompteur != null  && lesCompteur .Count != 0)
                lesCompteur.ForEach(t => t.POSE = DtpPose);
                this.TxtperiodePose.Text = dtpPose.SelectedDate.Value.Month.ToString("00") + "/" + dtpPose.SelectedDate.Value.Year;
            }
        }
        private void Cbo_TypeComptage_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.Cbo_TypeComptage.SelectedItem != null)
            {
                this.Txt_LibelleDiametre.Text = ((Galatee.Silverlight.ServiceAccueil.CsTypeComptage)this.Cbo_TypeComptage.SelectedItem).LIBELLE;
                this.Txt_LibelleDiametre.Tag = ((Galatee.Silverlight.ServiceAccueil.CsTypeComptage)this.Cbo_TypeComptage.SelectedItem);
                this.btn_DiametreCompteur.IsEnabled = false;
            }
        }
        private void btn_typeDeBranchement_Click(object sender, RoutedEventArgs e)
        {
            this.btn_typeDeBranchement.IsEnabled = false;
            if (LstTypeBrt.Count != 0)
            {
                List<object> _LstObj = Galatee.Silverlight.Shared.ClasseMEthodeGenerique.RetourneListeObjet(LstTypeBrt.Where(u => u.PRODUIT ==((CsProduit) this.Cbo_Produit.SelectedItem).CODE ).ToList());
                Galatee.Silverlight.MainView.UcListeGenerique ctr = new Galatee.Silverlight.MainView.UcListeGenerique(_LstObj, "CODE", "LIBELLE", Galatee.Silverlight.Resources.Accueil.Langue.lbl_ListeDiametre);
                ctr.Closed += new EventHandler(galatee_OkClickedBtnTypeBranchement);
                ctr.Show();
            }
        }

        void galatee_OkClickedBtnTypeBranchement(object sender, EventArgs e)
        {
            try
            {
                Galatee.Silverlight.MainView.UcListeGenerique ctrs = sender as Galatee.Silverlight.MainView.UcListeGenerique;
                if (ctrs.GetisOkClick)
                {
                    Galatee.Silverlight.ServiceAccueil.CsTypeBranchement _LeDiametre = (Galatee.Silverlight.ServiceAccueil.CsTypeBranchement)ctrs.MyObject;
                    this.Txt_TypeBrancehment.Text = _LeDiametre.CODE;
                    this.Txt_TypeBrancehment.Tag = _LeDiametre.PK_ID;
                    this.Txt_LibelleTypeBranchement.Text = string.IsNullOrEmpty(_LeDiametre.LIBELLE) ? string.Empty : _LeDiametre.LIBELLE;
                }
                this.btn_typeDeBranchement.IsEnabled = true;

            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Galatee.Silverlight.Resources.Accueil.Langue.lbl_Menu);
            }
        }

        private void Cbo_Zone_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (Cbo_Zone.SelectedItem != null)
                {
                    CsTournee tournee = Cbo_Zone.SelectedItem as CsTournee;
                    if (tournee != null)
                    {
                        Cbo_Zone.SelectedItem = tournee;
                        Cbo_Zone.Tag = tournee.PK_ID;
                    }
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.txtDevis);
            }
        }
        private void SaisirReference(bool EstSaisi)
        {
            if (EstSaisi)
            {
                label1_Copy1.Visibility = System.Windows.Visibility.Visible ;
                Txt_Client.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                label1_Copy1.Visibility = System.Windows.Visibility.Collapsed ;
                Txt_Client.Visibility = System.Windows.Visibility.Collapsed;
                this.Txt_Client.Text = string.Empty;
            }
        }

        private void Chk_ReferenceSaisie_Checked(object sender, RoutedEventArgs e)
        {
            SaisirReference(true);
        }

        private void Chk_ReferenceSaisie_Unchecked(object sender, RoutedEventArgs e)
        {
            SaisirReference(false);

        }

        private void Cbo_Puissance_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int nbrtransfo = int.Parse(this.Txt_NbreTransformateur.Text);
            int puissansSouscrit = _LaPuissanceSelectMt != null ? int.Parse(_LaPuissanceSelectMt.VALEUR.ToString()) : 0;
            int puissansInstalle = this.Cbo_Puissance.SelectedItem != null ? int.Parse(((CsPuissance)this.Cbo_Puissance.SelectedItem).VALEUR.ToString()) : 0;
            if (nbrtransfo != 0 && puissansSouscrit != 0 && puissansInstalle!=0)
              ChargeTypeComptage(nbrtransfo, puissansSouscrit, puissansInstalle);
        }
        private void ChargeTypeComptage(int nbrtransfo, int puissanceSouscrit, int puissanceInstalle)
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
                        Cbo_TypeComptage.SelectedValuePath = "PK_ID";
                        Cbo_TypeComptage.DisplayMemberPath = "LIBELLE";
                        Cbo_TypeComptage.ItemsSource = lesType;


                        if (lesType.Count == 1)
                        {
                            Cbo_TypeComptage.SelectedItem = lesType.First();
                            Cbo_TypeComptage.Tag = lesType.First();
                        }
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
    }
}

