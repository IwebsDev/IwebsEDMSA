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

namespace Galatee.Silverlight.Devis
{
    public partial class UcAbonnement : ChildWindow
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
        private DataGrid _dataGrid = null;
        private List<CsUsage> lstusage = new List<CsUsage>();
        List<Galatee.Silverlight.ServiceAccueil.CsCoutDemande> LstDesCoutsDemande = new List<Galatee.Silverlight.ServiceAccueil.CsCoutDemande>();
        Galatee.Silverlight.Shared.UcFichierJoint ctrl = null;
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

        public UcAbonnement()
        {
            InitializeComponent();
            this.Chk_FactureDiff.Visibility = System.Windows.Visibility.Collapsed;
            txt_FinPeriodeExo.MaxLength = 7;
            txt_DebutPeriodeExo.MaxLength = 7;

        }

        int IdDemandeDevis = 0;
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

        public UcAbonnement(string Tdem, string IsInit)
        {
            try
            {
                InitializeComponent();
                this.Tdem = Tdem;
                ModeExecution = SessionObject.ExecMode.Creation;

                Txt_Porte.MaxLength = 5;
                Txt_Etage.MaxLength = 2;

                this.Txt_CodeConso.MaxLength = SessionObject.Enumere.TailleCodeConso;
                this.Txt_CodeRegroupement.MaxLength = SessionObject.Enumere.TailleCodeRegroupement;
                this.TxtCategorieClient.MaxLength = SessionObject.Enumere.TailleCodeCategorie;
                this.Txt_usage.MaxLength = SessionObject.Enumere.TailleUsage;
                this.txt_Commune.MaxLength = SessionObject.Enumere.TailleCommune;
                this.txt_Quartier.MaxLength = SessionObject.Enumere.TailleQuartier;
                this.txt_NumSecteur.MaxLength = SessionObject.Enumere.TailleSecteur;
                this.txt_NumRue.MaxLength = SessionObject.Enumere.TailleRue;
                this.txtNumeroPiece.MaxLength = 20;
                //Societe
                Txt_Numeronina.MaxLength = 20;
                Txt_NomMandataire.MaxLength = 50;
                Txt_PrenomMandataire.MaxLength = 50;
                Txt_NomClientSociete.MaxLength = 50;
                //
                Txt_NomClient_PersonePhysiq.MaxLength = 50;
                Txt_NomClientAdministration.MaxLength = 50;

                txt_FinPeriodeExo.MaxLength = 7;
                txt_DebutPeriodeExo.MaxLength = 7;

                btn_tarif.Visibility = System.Windows.Visibility.Collapsed;
                txt_Reglage.Visibility = Visibility.Collapsed;
                Btn_Reglage.Visibility = Visibility.Collapsed;
              
                this.tabItemPersoneAdministration.Visibility = System.Windows.Visibility.Collapsed;
                this.tabItemPersoneMoral.Visibility = System.Windows.Visibility.Collapsed;
                this.tab_proprio.Visibility = System.Windows.Visibility.Collapsed;
                this.Chk_FactureDiff.Visibility = System.Windows.Visibility.Collapsed;

                this.lbl_TypeDeComptage.Visibility = System.Windows.Visibility.Collapsed;
                this.Txt_TypeDeComptage.Visibility = System.Windows.Visibility.Collapsed;

                AfficherOuMasquer(tabItemRejet, false);

                dtg_TarifClient.Visibility = System.Windows.Visibility.Collapsed;
                txt_MaticuleAgent.Visibility = System.Windows.Visibility.Collapsed;
                lbl_MatriculeAgent.Visibility = System.Windows.Visibility.Collapsed;
                tbControleClient.IsEnabled = false;
                this.dtp_RendezVousPrev.SelectedDate = System.DateTime.Today.AddDays(15);

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

                this.Cbo_Zone.Visibility = System.Windows.Visibility.Collapsed;
                this.label4.Visibility = System.Windows.Visibility.Collapsed;
                this.TxtOrdreTournee.Visibility = System.Windows.Visibility.Collapsed;
                label8.Visibility = System.Windows.Visibility.Collapsed;



            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.txtDevis);
            }
        }

        public UcAbonnement(int IdDemande)
        {
            try
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
                txt_FinPeriodeExo.MaxLength = 7;
                txt_DebutPeriodeExo.MaxLength = 7;

                txt_Reglage.Visibility = Visibility.Collapsed;
                Btn_Reglage.Visibility = Visibility.Collapsed;
                this.tabItemPersoneAdministration.Visibility = System.Windows.Visibility.Collapsed;
                this.tabItemPersoneMoral.Visibility = System.Windows.Visibility.Collapsed;
                this.tab_proprio.Visibility = System.Windows.Visibility.Collapsed;

                dtg_TarifClient.Visibility = System.Windows.Visibility.Collapsed;
                txt_MaticuleAgent.Visibility = System.Windows.Visibility.Collapsed;
                lbl_MatriculeAgent.Visibility = System.Windows.Visibility.Collapsed;
                tbControleClient.IsEnabled = false;
                this.Chk_FactureDiff.Visibility = System.Windows.Visibility.Collapsed;

                this.dtp_RendezVousPrev.SelectedDate = System.DateTime.Today.AddDays(15);
                this.lbl_TypeDeComptage.Visibility = System.Windows.Visibility.Collapsed;
                this.Txt_TypeDeComptage.Visibility = System.Windows.Visibility.Collapsed;

                AfficherOuMasquer(tabItemRejet, false);

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

                ChargeDetailDEvis(IdDemande);

                this.Cbo_Zone.Visibility = System.Windows.Visibility.Collapsed;
                this.label4.Visibility = System.Windows.Visibility.Collapsed;
                this.TxtOrdreTournee.Visibility = System.Windows.Visibility.Collapsed;
                label8.Visibility = System.Windows.Visibility.Collapsed;

            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.txtDevis);
            }
        }
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

        private void ChargerTarifParCategorieMt(CsDemande laDemande)
        {
            try
            {
                if (SessionObject.LstTarifCategorie.Count != 0)
                {
                    List<CsTarif> LstTarifCategorie = SessionObject.LstTarifCategorie.Where(p => p.PRODUIT == laDetailDemande.LaDemande.PRODUIT && p.CATEGORIE == laDemande.LeClient.CATEGORIE).ToList();
                    foreach (var item in LstTarifCategorie)
                        lstDesTarif.Add(item);
                    if (lstDesTarif.Count != 0)
                    {
                        this.Txt_CodeTarif.Text = lstDesTarif.First().CODE;
                        this.Txt_LibelleTarif.Text = lstDesTarif.First().LIBELLE;
                        this.Txt_CodeTarif.Tag = lstDesTarif.First().FK_IDTYPETARIF;
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
                        List<CsTarif> LstTarifCategorie = SessionObject.LstTarifCategorie.Where(p => p.PRODUIT == laDetailDemande.LaDemande.PRODUIT && p.CATEGORIE == laDemande.LeClient.CATEGORIE).ToList();
                        foreach (var item in LstTarifCategorie)
                            lstDesTarif.Add(item);
                        if (lstDesTarif.Count != 0)
                        {
                            this.Txt_CodeTarif.Text = lstDesTarif.First().CODE;
                            this.Txt_LibelleTarif.Text = lstDesTarif.First().LIBELLE;
                            this.Txt_CodeTarif.Tag = lstDesTarif.First().FK_IDTYPETARIF;
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

        private void ChargeDetailDEvis(int IdDemandeDevis)
        {
            Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient client = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
            client.ChargerDetailDemandeAsync(IdDemandeDevis,string.Empty );
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
                    laDetailDemande = args.Result;
                    this.Tdem = laDetailDemande.LaDemande.TYPEDEMANDE;
                    ActivationEnFonctionDeTdem();
                    RemplirCommuneParCentre(laDetailDemande.LaDemande.FK_IDCENTRE);
                    Tdem = laDetailDemande.LaDemande.TYPEDEMANDE;
                    this.Txt_TypeDevis.Text = !string.IsNullOrEmpty(laDetailDemande.LaDemande.LIBELLETYPEDEMANDE) ? laDetailDemande.LaDemande.LIBELLETYPEDEMANDE : string.Empty;
                    this.Txt_NumDevis.Text = !string.IsNullOrEmpty(laDetailDemande.LaDemande.NUMDEM) ? laDetailDemande.LaDemande.NUMDEM : string.Empty;
                    laDemandeSelect = laDetailDemande.LaDemande;
                    RenseignerDemande(laDetailDemande);
                    RenseignerAbon(laDetailDemande);
                    RenseignerAG(laDetailDemande);
                    RenseignerInformationsBrt(laDetailDemande);
                    RenseignerAppareil(laDetailDemande);
                    RenseignerClient(laDetailDemande);
                    RenseignerFourniture(laDetailDemande);
                    RenseignerInformationsRejetDemande(laDetailDemande);
                    #region DocumentScanne
                    ctrl = new Galatee.Silverlight.Shared.UcFichierJoint(laDetailDemande.ObjetScanne,false );
                    Vwb.Stretch = Stretch.None;
                    Vwb.Child = ctrl;
                    #endregion
                    
                }
                LayoutRoot.Cursor = Cursors.Arrow;
            };
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
                        Cbo_Zone.Tag = lstTournee.Where(t => t.FK_IDCENTRE == pCentreId).First();
                    }
                    if (laDetailDemande.Ag != null && laDetailDemande.Ag.FK_IDTOURNEE != null && laDetailDemande.Ag.FK_IDTOURNEE != 0)
                    {
                        Cbo_Zone.SelectedItem = lstTournee.FirstOrDefault(p => p.PK_ID == laDetailDemande.Ag.FK_IDTOURNEE);
                        Cbo_Zone.Tag = lstTournee.FirstOrDefault(p => p.PK_ID == laDetailDemande.Ag.FK_IDTOURNEE);

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
                    if (laDetailDemande.Ag != null && laDetailDemande.Ag.FK_IDTOURNEE != null && laDetailDemande.Ag.FK_IDTOURNEE != 0)
                    {
                        Cbo_Zone.SelectedItem = lstTournee.FirstOrDefault(p => p.PK_ID == laDetailDemande.Ag.FK_IDTOURNEE);
                        Cbo_Zone.Tag = lstTournee.FirstOrDefault(p => p.PK_ID == laDetailDemande.Ag.FK_IDTOURNEE);

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
        private bool VerifieChampObligation()
        {
            try
            {
                bool ReturnValue = true;
                if (laDetailDemande.LaDemande.TYPEDEMANDE  == SessionObject.Enumere.BranchementAbonement ||
                    laDetailDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.BranchementSimple ||
                    laDetailDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.AbonnementSeul)
                {
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

                        //if (Cbo_TypePiecePersonnePhysique.SelectedItem != null)
                        //    throw new Exception("Selectionnez le type de pièce ");

                        //if (string.IsNullOrEmpty(this.Txt_Capital.Text))
                        //    throw new Exception("Saisir le capitale de l'entreprise");

                        //if (string.IsNullOrEmpty(this.Txt_IdentiteFiscale.Text))
                        //    throw new Exception("Saisir le numéro d'identité fiscale");


                        //if (string.IsNullOrEmpty(this.Txt_NomSignataire.Text))
                        //    throw new Exception("Saisir le nom du signataire");

                        //if (string.IsNullOrEmpty(this.Txt_PrenomSignataire.Text))
                        //    throw new Exception("Saisir le prenom du signataire");

                        if (Cbo_Type_Proprietaire.SelectedItem == null)
                            Cbo_Type_Proprietaire.SelectedItem = SessionObject.Lsttypeprop.FirstOrDefault();

                    }
                    if (((CsTypeClient)Cbo_Type_Client.SelectedItem).CODE.Trim() == "003".Trim())
                    {
                        tabC_Onglet.SelectedItem = tabItemClientInfo;
                        this.tbControleClient.SelectedItem = tabItemPersoneAdministration;

                        if (string.IsNullOrEmpty(this.Txt_NomClientAdministration.Text))
                            throw new Exception("Saisir le nom de client");

                        //if (string.IsNullOrEmpty(this.Txt_PrenomMandataireAdministration.Text))
                        //    throw new Exception("Saisir le prénom du mandataire ");

                        //if (string.IsNullOrEmpty(this.Txt_NomMandataireAdministration.Text))
                        //    throw new Exception("Saisir le nom du mandataire");

                        //if (string.IsNullOrEmpty(this.Txt_PrenomSignataireAdministration.Text))
                        //    throw new Exception("Saisir la prenom du signataire ");

                        //if (string.IsNullOrEmpty(this.Txt_NomSignataireAdministration.Text))
                        //    throw new Exception("Saisir du signataire ");
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
 
                    #endregion

                    #region information abonnement

                    tabC_Onglet.SelectedItem = tabItemContrat;
                    if (string.IsNullOrEmpty(this.TxtCategorieClient.Text))
                        throw new Exception("Selectionnez la catégorie du client ");

                    //if (Cbo_Usage.SelectedItem == null)
                    //    throw new Exception("Selectionnez l'usage ");


                    if (string.IsNullOrEmpty(this.Txt_CodeConso.Text))
                        throw new Exception("Selectionnez le code consommateur ");

                    if (string.IsNullOrEmpty(this.Txt_CodeConso.Text))
                        throw new Exception("Selectionnez le code consommateur ");

                    if (laDetailDemande.LaDemande.PRODUIT  != SessionObject.Enumere.ElectriciteMT)
                    {
                        if (string.IsNullOrEmpty(this.txt_Reglage.Text))
                            throw new Exception("Selectionnez le calibre ");
                    }

                    if (this.TxtCategorieClient.Text == SessionObject.Enumere.CategorieAgentEdm)
                    {
                        if (string.IsNullOrEmpty(this.txt_MaticuleAgent.Text))
                            throw new Exception("Entrer le matricule de l'agent ");

                        if (this.txt_MaticuleAgent.Text.Length != SessionObject.Enumere.TailleMatricule)
                            throw new Exception("Le matricule agent tient sur 5 positions.");
                    }

                    if (string.IsNullOrEmpty(Txt_CodeTarif.Text))
                        throw new Exception("Saisir le code tarif");

                    if (string.IsNullOrEmpty(Txt_CodeForfait.Text))
                        throw new Exception("Saisir le forfait");
                    #endregion
                    #region Adresse géographique
                    tabC_Onglet.SelectedItem = tabAdressGeographique;
                    if (string.IsNullOrEmpty(this.txt_Commune.Text))
                        throw new Exception("Séléctionnez la commune ");

                    if (string.IsNullOrEmpty(this.txt_Quartier.Text))
                        throw new Exception("Séléctionnez le quartier ");
                    #endregion
                }
                return ReturnValue;

            }
            catch (Exception ex)
            {
                this.Btn_Transmettre.IsEnabled = true;
                Message.ShowInformation(ex.Message, "Accueil");
                return false;
            }

        }
 
        ObjELEMENTDEVIS _element = new ObjELEMENTDEVIS();
        public List<ObjELEMENTDEVIS> MyElements { get; set; }
        public CsCtax Taxe { get; set; }
        decimal taux = (decimal)0;
        void ChargerCoutDemande()
        {
            try
            {
                if (SessionObject.LstDesCoutDemande.Count != 0)
                {
                    decimal? pPuissanceSouscrite = 0;
                    pPuissanceSouscrite = laDetailDemande.LaDemande.PUISSANCESOUSCRITE;

                    string typedemande = laDetailDemande.LaDemande.TYPEDEMANDE;
                    if (laDetailDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.AugmentationPuissance ||
                        laDetailDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.DimunitionPuissance)
                        typedemande = SessionObject.Enumere.BranchementAbonement;

                    LstDesCoutsDemande = SessionObject.LstDesCoutDemande.Where(p => p.TYPEDEMANDE == typedemande).OrderBy(c=>c.PK_ID).ToList();
                    if (!string.IsNullOrEmpty(laDetailDemande.LaDemande.CENTRE))
                        LstDesCoutsDemande = LstDesCoutsDemande.Where(p => p.CENTRE == laDetailDemande.LaDemande.CENTRE || p.CENTRE == "000").ToList();

                    if (!string.IsNullOrEmpty(laDetailDemande.LaDemande.PRODUIT))
                        LstDesCoutsDemande = LstDesCoutsDemande.Where(p => p.PRODUIT == laDetailDemande.LaDemande.PRODUIT || p.PRODUIT == "00").ToList();

                    if (LstDesCoutsDemande.Count != 0)
                    {
                        string pDiametre = string.Empty;
                        if (laDetailDemande.LaDemande != null)
                            pDiametre = string.IsNullOrEmpty(laDetailDemande.LaDemande.REGLAGECOMPTEUR) ? string.Empty : laDetailDemande.LaDemande.REGLAGECOMPTEUR;
                        LstDesCoutsDemande = LstDesCoutsDemande.Where(p => p.REGLAGECOMPTEUR == pDiametre || string.IsNullOrEmpty(p.REGLAGECOMPTEUR)).ToList();

                        string pCategorie = string.Empty;
                        if (laDetailDemande.LeClient != null)
                            pCategorie = string.IsNullOrEmpty(laDetailDemande.LeClient.CATEGORIE) ? string.Empty : laDetailDemande.LeClient.CATEGORIE;
                        LstDesCoutsDemande = LstDesCoutsDemande.Where(p => p.CATEGORIE == pCategorie || string.IsNullOrEmpty(p.CATEGORIE)).ToList();

                        if (laDetailDemande.LaDemande.PRODUIT == SessionObject.Enumere.ElectriciteMT)
                        {
                            if (laDetailDemande.LaDemande != null)
                            {
                                if (laDetailDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.AugmentationPuissance &&
                                     laDetailDemande.LaDemande.PRODUIT == SessionObject.Enumere.ElectriciteMT && this.Chk_FactureDiff.IsChecked == true)
                                    pPuissanceSouscrite = laDetailDemande.LaDemande.PUISSANCESOUSCRITE - laDetailDemande.LaDemande.ANCIENNEPUISSANCE;
                                else
                                {
                                    if (laDetailDemande.LaDemande.PUISSANCESOUSCRITE != null)
                                        pPuissanceSouscrite = laDetailDemande.LaDemande.PUISSANCESOUSCRITE;
                                    else
                                        pPuissanceSouscrite = laDetailDemande.Abonne.PUISSANCE;
                                }
                            }

                            //CsCoutDemande leCoutAvance = LstDesCoutsDemande.FirstOrDefault(t => t.COPER == SessionObject.Enumere.CoperCAU);
                            //if (leCoutAvance != null)
                            //    leCoutAvance.MONTANT = leCoutAvance.MONTANT * pPuissanceSouscrite;

                        }

                        /**Autre cout**/
                        foreach (Galatee.Silverlight.ServiceAccueil.CsCoutDemande item in LstDesCoutsDemande.Where(t => t.COPER != SessionObject.Enumere.CoperTRV && t.COPER != SessionObject.Enumere.CoperFAB).ToList())
                        {
                            int idtaxe = item.FK_IDTAXE;
                            Galatee.Silverlight.ServiceAccueil.CsCtax tax = SessionObject.LstDesTaxe.FirstOrDefault(t => t.PK_ID == item.FK_IDTAXE);
                            if (tax != null)
                                taux = tax.TAUX;

                            if (item.COPER == SessionObject.Enumere.CoperCAU && laDetailDemande.LaDemande.PRODUIT == SessionObject.Enumere.ElectriciteMT)
                            {
                                if (MyElements != null && MyElements.Count != 0)
                                {
                                    ObjELEMENTDEVIS laCaution = MyElements.FirstOrDefault(t => t.CODECOPER == SessionObject.Enumere.CoperCAU);
                                    if (laCaution != null && !string.IsNullOrEmpty(laCaution.CODECOPER))
                                        MyElements.Remove(laCaution);
                                }
                            }

                            if (item.MONTANT != null && item.MONTANT != 0)
                            {
                                _element = new ObjELEMENTDEVIS();
                                _element.NUMDEVIS = laDetailDemande.LaDemande.NUMDEM;
                                _element.NUMDEM  = laDetailDemande.LaDemande.NUMDEM;
                                _element.DESIGNATION = _element.LIBELLE = item.LIBELLECOPER;
                                _element.PRIX = item.MONTANT != null ? (decimal)item.MONTANT : 0;
                                _element.COUTFOURNITURE = item.MONTANT != null ? (decimal)item.MONTANT : 0;

                                if (item.COPER == SessionObject.Enumere.CoperCAU &&
                                    laDetailDemande.LaDemande.PRODUIT == SessionObject.Enumere.ElectriciteMT && pPuissanceSouscrite != null)
                                {
                                    string separateur = System.Globalization.NumberFormatInfo.CurrentInfo.CurrencyDecimalSeparator;
                                    string [] ps = pPuissanceSouscrite.ToString().Split(new char[] { separateur[0] });
                                    _element.QUANTITE = int.Parse(ps[0]);
                                }
                                else
                                    _element.QUANTITE = 1;
                                _element.MONTANTHT = (item.MONTANT != null && _element.QUANTITE != null) ? (int)_element.QUANTITE * (decimal)item.MONTANT : 0;
                                _element.MONTANTTAXE = (decimal)Math.Ceiling((double)(_element.COUT * taux));
                                _element.MONTANTTTC = _element.MONTANTHT + _element.MONTANTTAXE;

                                _element.FK_IDTAXE = idtaxe;
                                _element.TAUXTAXE = taux;
                                _element.ISEXTENSION = false;

                                _element.TVARECAP = _element.MONTANTTAXE.Value.ToString(SessionObject.FormatMontant);
                                _element.ISDEFAULT = true;
                                _element.NUMFOURNITURE = item.COPER;
                                _element.CODECOPER = item.COPER;
                                _element.FK_IDCOPER = item.FK_IDCOPER;
                                _element.FK_IDCOUTCOPER = item.PK_ID;
                                _element.FK_IDMATERIELDEVIS = null;
                                _element.FK_IDDEMANDE = laDetailDemande.LaDemande.PK_ID;
                                _element.ISFOURNITURE = true;
                                _element.ISPOSE = true;
                                _element.USERCREATION  = UserConnecte.matricule ;
                                if (MyElements == null)
                                    MyElements = new List<ObjELEMENTDEVIS>();
                                this.MyElements.Add(_element);
                            }
                        }
                    }
                }
                dataGridForniture.ItemsSource = null;
                dataGridForniture.ItemsSource = MyElements;
                laDetailDemande.EltDevis = MyElements;


                this.Txt_TotalHt.Text = MyElements.Sum(t => t.MONTANTHT).Value.ToString(SessionObject.FormatMontant);
                this.Txt_TotalTva.Text = MyElements.Sum(t => t.MONTANTTAXE).Value.ToString(SessionObject.FormatMontant);
                this.Txt_TotalTtc.Text = MyElements.Sum(t => t.MONTANTTTC).Value.ToString(SessionObject.FormatMontant);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        //private void VerifieAbonnementEDM(CsDemande demandedevis, bool IsTransmetre)
        //{

        //    AcceuilServiceClient client = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
        //    client.VerifieMatriculeAgentCompleted += (ssender, args) =>
        //    {
        //        if (args.Cancelled || args.Error != null)
        //        {
        //            LayoutRoot.Cursor = Cursors.Arrow;
        //            string error = args.Error.Message;
        //            Message.ShowError(error, Silverlight.Resources.Devis.Languages.txtDevis);
        //            return;
        //        }
        //        if (args.Result == null)
        //        {
        //            LayoutRoot.Cursor = Cursors.Arrow;
        //            Message.ShowError(Silverlight.Resources.Devis.Languages.AucunesDonneesTrouvees, Silverlight.Resources.Devis.Languages.txtDevis);
        //            return;
        //        }
        //        else
        //        {
        //            CsClient leClient = new CsClient();
        //            leClient = args.Result;
        //            if (leClient != null)
        //            {
        //                if (!string.IsNullOrEmpty(leClient.MATRICULEAGENT))
        //                {
        //                    if (!string.IsNullOrEmpty( leClient.CENTRE )) 
        //                    Message.ShowInformation("Ce Agent possède déja un abonnement " + leClient.CENTRE + "  " + leClient.REFCLIENT + " " + leClient.ORDRE, "Client EDM");
        //                    else 
        //                    {
        //                        demandedevis.LaDemande.MATRICULE = UserConnecte.matricule;
        //                        Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient clients = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
        //                        clients.ValiderDemandeInitailisationCompleted += (ss, b) =>
        //                        {
        //                            if (b.Cancelled || b.Error != null)
        //                            {
        //                                string error = b.Error.Message;
        //                                Message.ShowError(error, Silverlight.Resources.Devis.Languages.txtDevis);
        //                                return;
        //                            }
        //                            string numedemande = string.Empty;
        //                            string Client = string.Empty;
        //                            if (IsTransmetre)
        //                            {
        //                                string Retour = b.Result;
        //                                string[] coupe = Retour.Split('.');
        //                                Shared.ClasseMEthodeGenerique.InitWOrkflow(coupe[0], demandedevis.LaDemande.FK_IDCENTRE, coupe[1], demandedevis.LaDemande.FK_IDTYPEDEMANDE);
        //                                numedemande = coupe[1];
        //                                Client = coupe[2];
        //                            }
        //                            List<CsDemandeBase> leDemandeAEditer = new List<CsDemandeBase>();
        //                            demandedevis.LaDemande.NOMCLIENT = demandedevis.LeClient.NOMABON;
        //                            demandedevis.LaDemande.LIBELLETYPEDEMANDE = txt_tdem.Text;
        //                            demandedevis.LaDemande.NUMDEM = numedemande;
        //                            demandedevis.LaDemande.CLIENT = Client;
        //                            demandedevis.LaDemande.LIBELLEPRODUIT = demandedevis.LaDemande.LIBELLEPRODUIT :


        //                            leDemandeAEditer.Add(demandedevis.LaDemande);
        //                            Utility.ActionDirectOrientation<ServicePrintings.CsDemandeBase, CsDemandeBase>(leDemandeAEditer, null, SessionObject.CheminImpression, "AccuseRecption", "Accueil", true);
        //                            FermerFenetre();
        //                        };
        //                        clients.ValiderDemandeInitailisationAsync(demandedevis);
        //                    }
        //                }
        //            }
        //            else
        //               Message.ShowInformation("Matricule agent incorect ", "Client EDM");
        //        }
        //    };
        //    client.VerifieMatriculeAgentAsync(demandedevis.LeClient.MATRICULE  );
        //}
        private void ValidationDevis(CsDemande laDetailDemande, bool IsTransmetre)
        {
            try
            {
                #region Abon
                if (this.Txt_DateAbonnement.Text == null)
                {
                    Message.Show("Saisir la date d'abonnement", "ValidationDevis");
                    return;
                }
                RemplirCommuneParCentre(laDetailDemande.LaDemande.FK_IDCENTRE);

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
                    laDetailDemande.Abonne.FK_IDTYPECOMPTAGE = laDetailDemande.LaDemande.FK_IDTYPECOMPTAGE;
                    laDetailDemande.Abonne.TYPECOMPTAGE = laDetailDemande.LaDemande.TYPECOMPTAGE;
                }
                else
                {
                    laDetailDemande.LaDemande.FK_IDREGLAGECOMPTEUR = (this.txt_Reglage.Tag == null) ? null : (int?)this.txt_Reglage.Tag;
                    laDetailDemande.LaDemande.REGLAGECOMPTEUR = this.Btn_Reglage.Tag == null ? string.Empty : this.Btn_Reglage.Tag.ToString();
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
                #endregion
                #region AG
                if (laDetailDemande.Ag == null) laDetailDemande.Ag = new CsAg();
                if (Cbo_Commune.SelectedItem != null)
                {
                    var commune = Cbo_Commune.SelectedItem as CsCommune;
                    if (commune != null)
                    {
                        laDetailDemande.Ag.FK_IDCOMMUNE = commune.PK_ID;
                        laDetailDemande.Ag.COMMUNE = commune.CODE;
                    }
                }
                laDetailDemande.Ag.FK_IDTOURNEE = null;
                if (Cbo_Quartier.SelectedItem != null)
                {
                    var quartier = Cbo_Quartier.SelectedItem as CsQuartier;
                    if (quartier != null)
                    {
                        laDetailDemande.Ag.FK_IDQUARTIER = quartier.PK_ID;
                        laDetailDemande.Ag.QUARTIER = quartier.CODE;
                    }
                }
                if (!string.IsNullOrEmpty(this.txt_NumRue.Text))
                    laDetailDemande.Ag.RUE = this.txt_NumRue.Text;

                if (Cbo_Secteur.SelectedItem != null)
                {
                    var Secteur = Cbo_Secteur.SelectedItem as ServiceAccueil.CsSecteur;
                    if (Secteur != null)
                    {
                        laDetailDemande.Ag.FK_IDSECTEUR = Secteur.PK_ID;
                        laDetailDemande.Ag.SECTEUR = Secteur.CODE;
                    }
                }
                if (Cbo_Zone.SelectedItem != null)
                {
                    var tournee = Cbo_Zone.SelectedItem as ServiceAccueil.CsTournee;
                    if (tournee != null)
                    {
                        laDetailDemande.Ag.FK_IDTOURNEE = tournee.PK_ID;
                        laDetailDemande.Ag.TOURNEE = tournee.CODE;
                    }
                }
                if (!string.IsNullOrEmpty(this.TxtOrdreTournee.Text))
                    laDetailDemande.Ag.ORDTOUR = this.TxtOrdreTournee.Text;
                laDetailDemande.Ag.ETAGE = string.IsNullOrEmpty(this.Txt_Etage.Text) ? null : this.Txt_Etage.Text;
                laDetailDemande.Ag.PORTE = string.IsNullOrEmpty(this.Txt_Porte.Text) ? null : this.Txt_Porte.Text;
                laDetailDemande.Ag.CENTRE = string.IsNullOrEmpty(laDetailDemande.LaDemande.CENTRE) ? null : laDetailDemande.LaDemande.CENTRE;
                laDetailDemande.Ag.CLIENT = string.IsNullOrEmpty(laDetailDemande.LaDemande.CLIENT) ? null : laDetailDemande.LaDemande.CLIENT;
                laDetailDemande.Ag.NUMDEM = string.IsNullOrEmpty(this.Txt_NumDevis.Text) ? null : this.Txt_NumDevis.Text;
                laDetailDemande.Ag.FK_IDCENTRE = laDetailDemande.LaDemande.FK_IDCENTRE;
                laDetailDemande.Ag.DATECREATION = DateTime.Now;
                laDetailDemande.Ag.USERCREATION = UserConnecte.matricule;
                #endregion
                #region Client

                if (laDetailDemande.LaDemande.TYPEDEMANDE != SessionObject.Enumere.DimunitionPuissance &&
                  laDetailDemande.LaDemande.TYPEDEMANDE != SessionObject.Enumere.AugmentationPuissance &&
                  laDetailDemande.LaDemande.TYPEDEMANDE != SessionObject.Enumere.ChangementProduit)
                {
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
                        }
                    }
                    laDetailDemande.LeClient.DENABON = string.IsNullOrEmpty(this.Txt_Civilite.Text) ? null : this.Txt_Civilite.Text;
                    laDetailDemande.LeClient.TELEPHONEFIXE = string.IsNullOrEmpty(this.txt_Telephone_Fixe.Text) ? null : this.txt_Telephone_Fixe.Text;
                    laDetailDemande.LeClient.FAX = string.IsNullOrEmpty(this.Txt_NumFax.Text) ? null : this.Txt_NumFax.Text;
                    laDetailDemande.LeClient.BOITEPOSTAL = string.IsNullOrEmpty(this.Txt_BoitePostale.Text) ? null : this.Txt_BoitePostale.Text;
                    laDetailDemande.LeClient.TELEPHONE = string.IsNullOrEmpty(this.txt_Telephone.Text) ? null : this.txt_Telephone.Text;
                    laDetailDemande.LeClient.NUMPROPRIETE = string.IsNullOrEmpty(this.txtPropriete.Text) ? null : this.txtPropriete.Text;
                    laDetailDemande.LeClient.CENTRE = string.IsNullOrEmpty(laDetailDemande.LaDemande.CENTRE) ? null : laDetailDemande.LaDemande.CENTRE;
                    laDetailDemande.LeClient.REFCLIENT = string.IsNullOrEmpty(laDetailDemande.LaDemande.CLIENT) ? null : laDetailDemande.LaDemande.CLIENT;
                    laDetailDemande.LeClient.FK_IDCENTRE = laDetailDemande.LaDemande.FK_IDCENTRE;
                    laDetailDemande.LeClient.DATECREATION = DateTime.Now;
                    laDetailDemande.LeClient.USERCREATION = UserConnecte.matricule;
                    laDetailDemande.LeClient.NUMDEM = string.IsNullOrEmpty(this.Txt_NumDevis.Text) ? null : this.Txt_NumDevis.Text;
                    laDetailDemande.LeClient.ISFACTUREEMAIL = chk_Email.IsChecked.Value;
                    laDetailDemande.LeClient.ISFACTURESMS = chk_SMS.IsChecked.Value;
                    if (!string.IsNullOrWhiteSpace(this.Txt_Email.Text))
                    {
                        if (IsEmail(this.Txt_Email.Text))
                        {
                            laDetailDemande.LeClient.EMAIL = string.IsNullOrEmpty(this.Txt_Email.Text) ? null : this.Txt_Email.Text.Trim();
                        }
                        else
                        {
                            Message.Show("Veuillez saisi un email client correct", "Erreur");
                            return;
                        }
                    }

                    #region Sylla 24/09/2015

                    if (this.Cbo_Type_Client.SelectedItem != null)
                    {
                        CsTypeClient TypeClient = (CsTypeClient)this.Cbo_Type_Client.SelectedItem;
                        if (TypeClient.CODE != null)
                        {
                            string codetypeclient = TypeClient.CODE;
                            switch (codetypeclient.Trim())
                            {
                                case "001":
                                    {
                                        #region Personne Physique
                                        this.tabItemPersoneMoral.Visibility = System.Windows.Visibility.Collapsed;
                                        this.tabItemPersoneAdministration.Visibility = System.Windows.Visibility.Collapsed;
                                        this.tabItemPersonnePhysique.Visibility = System.Windows.Visibility.Visible;
                                        tbControleClient.SelectedItem = this.tabItemPersonnePhysique;
                                        GetPersonnPhyqueData(laDetailDemande);
                                        #endregion
                                        break;
                                    }
                                case "002":
                                    {
                                        tabItemPersonnePhysique.Visibility = System.Windows.Visibility.Collapsed;
                                        tabItemPersoneAdministration.Visibility = System.Windows.Visibility.Collapsed;
                                        tabItemPersoneMoral.Visibility = System.Windows.Visibility.Visible;
                                        tbControleClient.SelectedItem = this.tabItemPersoneMoral;
                                        if (GetSocietePriveData(laDetailDemande) == null)
                                            return;
                                        break;
                                    }
                                case "003":
                                    {
                                        tabItemPersonnePhysique.Visibility = System.Windows.Visibility.Collapsed;
                                        tabItemPersoneMoral.Visibility = System.Windows.Visibility.Collapsed;
                                        tabItemPersoneAdministration.Visibility = System.Windows.Visibility.Visible;
                                        tbControleClient.SelectedItem = this.tabItemPersoneAdministration;
                                        GetAdministraionInstitutData(laDetailDemande);
                                        break;
                                    }
                                default:
                                    break;
                            }
                        }
                        else
                        {
                            Message.Show("Veuillez renseigné les informations spécifique au typr de client", "Demande");
                            return;
                        }

                    }
                    else
                    {
                        Message.Show("Veuillez renseigné les informations spécifique au typr de client", "Demande");
                        return;
                    }

                    //GetSocieteProprietaire(laDetailDemande);

                    #endregion
                    laDetailDemande.LeClient.CODEIDENTIFICATIONNATIONALE = string.IsNullOrEmpty(this.Txt_Numeronina.Text) ? null : this.Txt_Numeronina.Text.Trim();
                    laDetailDemande.LeClient.FK_IDRELANCE = 1;
                    laDetailDemande.LeClient.CODERELANCE = "0";
                    laDetailDemande.LeClient.MODEPAIEMENT = "0";
                    laDetailDemande.LeClient.FK_IDMODEPAIEMENT = 1;
                }

                #endregion
                #region Doc Scanne
                if (laDetailDemande.ObjetScanne == null) laDetailDemande.ObjetScanne = new List<ObjDOCUMENTSCANNE>();
                if (ctrl != null && ctrl.LstPiece != null)
                {

                    laDetailDemande.ObjetScanne.Clear();
                    laDetailDemande.ObjetScanne.AddRange(ctrl.LstPiece.Where(i=>i.ISNEW == true || i.ISTOREMOVE ==true  ));
                }
                #endregion


                laDetailDemande.LstCanalistion.ForEach(c => c.PROPRIO = laDetailDemande.LeClient.PROPRIO);
                laDetailDemande.LstCanalistion.ForEach(c => c.FK_IDPROPRIETAIRE = laDetailDemande.LeClient.FK_IDPROPRIETAIRE);
                if ((laDetailDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.AugmentationPuissance ||
                    laDetailDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.DimunitionPuissance ||
                    laDetailDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.Reabonnement ||
                    laDetailDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.AbonnementSeul)
                      && laDetailDemande.LaDemande.ISMETREAFAIRE == false)
                {
                    laDetailDemande.EltDevis = MyElements;
                    laDetailDemande.EltDevis.ForEach(u => u.NUMDEM = laDetailDemande.LaDemande.NUMDEM);
                    laDetailDemande.EltDevis.ForEach(u => u.USERCREATION = UserConnecte.matricule);
                    laDetailDemande.EltDevis.ForEach(u => u.DATECREATION = System.DateTime.Now);
                }
                else
                    laDetailDemande.EltDevis = null;


                AcceuilServiceClient clientDevis = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                clientDevis.ValiderInformationAbonnementAsync(laDetailDemande, IsTransmetre);
                clientDevis.ValiderInformationAbonnementCompleted += (ss, b) =>
                {
                    if (b.Cancelled || b.Error != null)
                    {
                        string error = b.Error.Message;
                        Message.ShowError(error, Silverlight.Resources.Devis.Languages.txtDevis);
                        return;
                    }
                    if (string.IsNullOrEmpty(b.Result))
                    {
                        if (IsTransmetre)
                            Message.ShowInformation("Demande transmise avec succès", "Demande");
                        else
                            Message.ShowInformation("Mise à jour éffectué avec succès", "Demande");
                        this.DialogResult = false;
                    }
                    else
                        Message.ShowError(b.Result, "Demande");
                };
            }
            catch (Exception ex)
            {
                LayoutRoot.Cursor = Cursors.Arrow;
                throw ex;
            }
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
                    //ReloadCategClient(SessionObject.LstCategorie.Where(t => t.CODE != SessionObject.Enumere.CategorieEp).ToList());
                    ReloadCategClient(SessionObject.LstCategorie);
                    return;
                }
                AcceuilServiceClient service = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.RetourneCategorieCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    SessionObject.LstCategorie = args.Result;
                    Cbo_Categorie.Items.Clear();
                    //ReloadCategClient(SessionObject.LstCategorie.Where(t => t.CODE != SessionObject.Enumere.CategorieEp).ToList());
                    ReloadCategClient(SessionObject.LstCategorie);
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
                    LstPuissanceMt = SessionObject.LstPuissance.Where(t => t.PRODUIT == laDetailDemande.LaDemande.PRODUIT).ToList();
                    return;
                }
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.ChargerPuissanceSouscriteCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    SessionObject.LstPuissance = args.Result;
                    LstPuissanceMt = SessionObject.LstPuissance.Where(t => t.PRODUIT == laDetailDemande.LaDemande.PRODUIT).ToList();
                };
                service.ChargerPuissanceSouscriteAsync();
                service.CloseAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void ChargerForfait(CsDemande laDemande)
        {
            try
            {
                if (SessionObject.LstForfait.Count != 0)
                    LstForfait = SessionObject.LstForfait.Where(p => p.PRODUIT == laDemande.LaDemande.PRODUIT).ToList();
                else
                {
                    AcceuilServiceClient service = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                    service.ChargerForfaitCompleted += (s, args) =>
                    {
                        if (args != null && args.Cancelled)
                            return;

                        SessionObject.LstForfait = args.Result;
                        LstForfait = SessionObject.LstForfait.Where(p => p.PRODUIT == laDemande.LaDemande.PRODUIT).ToList();
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

        private void ChargerFrequence(CsDemande laDemande)
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
        private void ChargerMois(CsDemande laDemande)
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

        List<CsTarif> lstDesTarif = new List<CsTarif>();
        private void ChargerPuissanceEtTarif(int idProduit, int? idPuissance, int? idCategorie, int? idReglageCompteur,int? idtarif )
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
                        this.Txt_CodePussanceSoucrite.Text = string.Empty ;
                        this.Txt_CodePussanceSoucrite.Tag = null;
                    }

                    if (lstDesTarif == null) lstDesTarif = new List<CsTarif>();
                    if (lstDesTarif.Count != 0) lstDesTarif.Clear();

                    foreach (CsTarif item in LstPuissanceTarif)
                        lstDesTarif.Add(new CsTarif { PK_ID = item.PK_ID ,CODE =item.CODE ,LIBELLE = item.LIBELLE });
                    if (lstDesTarif != null && lstDesTarif.Count == 1)
                    {
                        this.Txt_CodeTarif.Text = lstDesTarif.First().CODE;
                        this.Txt_LibelleTarif.Text = lstDesTarif.First().LIBELLE;
                        this.Txt_CodeTarif.Tag = lstDesTarif.First().PK_ID;
                    }
                    else
                    {
                        this.Txt_CodeTarif.Text = string.Empty ;
                        this.Txt_LibelleTarif.Text = string.Empty;
                        this.Txt_CodeTarif.Tag = null;
                    }
                };
                service.ChargerTypeTarifAsync(idProduit, idPuissance, idCategorie, idReglageCompteur, idtarif);
                service.CloseAsync();
          
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
                //if (item.CODE.Trim() == "004".Trim()) continue;
                //if (item.CODE.Trim() == "004".Trim()) continue;
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
                ListeQuartierFiltres.AddRange(_listeDesQuartierExistant.Where(q => q.FK_IDCOMMUNE == pCommuneId && q.CODE != DataReferenceManager.QuartierInconnu).ToList());

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
        private void RenseignerAG(CsDemande laDemande)
        {
            try
            {
                if (laDemande != null)
                {
                    #region Ag
                    if (laDemande.Ag != null)
                    {
                        txt_Commune.Text = !string.IsNullOrEmpty(laDemande.Ag.COMMUNE) ? laDemande.Ag.COMMUNE : string.Empty;
                        txt_Quartier.Text = !string.IsNullOrEmpty(laDemande.Ag.QUARTIER) ? laDemande.Ag.QUARTIER : string.Empty;
                        txt_NumRue.Text = !string.IsNullOrEmpty(laDemande.Ag.RUE) ? laDemande.Ag.RUE : string.Empty;
                        txtPropriete.Text = !string.IsNullOrEmpty(laDemande.LeClient.NUMPROPRIETE) ? laDemande.LeClient.NUMPROPRIETE : string.Empty;

                        //txt_Telephone.Text = !string.IsNullOrEmpty(laDemande.Ag.TELEPHONE) ? laDemande.Ag.TELEPHONE : string.Empty;
                        //txt_NumLot.Text = !string.IsNullOrEmpty(laDemande.Ag.l) ? pInformationsDevis.DemandeDevis.NUMLOT : string.Empty;

                        if (laDemande.Ag.FK_IDCOMMUNE != null && laDemande.Ag.FK_IDCOMMUNE != 0)
                        {
                            foreach (CsCommune commune in Cbo_Commune.Items)
                            {
                                if (commune.PK_ID == laDemande.Ag.FK_IDCOMMUNE)
                                {
                                    Cbo_Commune.SelectedItem = commune;
                                    break;
                                }
                            }
                        }
                        if (laDemande.Ag.FK_IDQUARTIER != null && laDemande.Ag.FK_IDQUARTIER != 0)
                        {
                            foreach (CsQuartier quartier in Cbo_Quartier.Items)
                            {
                                if (quartier.PK_ID == laDemande.Ag.FK_IDQUARTIER)
                                {
                                    Cbo_Quartier.SelectedItem = quartier;
                                    break;
                                }
                            }
                        }
                        this.txt_NumRue.Text = string.IsNullOrEmpty(laDemande.Ag.RUE) ? string.Empty : laDemande.Ag.RUE;
                        if (!string.IsNullOrWhiteSpace(laDemande.Ag.SECTEUR))
                        {
                            foreach (Galatee.Silverlight.ServiceAccueil.CsSecteur secteur in Cbo_Secteur.Items)
                            {
                                if (secteur.CODE == laDemande.Ag.SECTEUR)
                                {
                                    Cbo_Secteur.SelectedItem = secteur;
                                    break;
                                }
                            }
                        }
                    }
                    Txt_Porte.Text = laDemande.Ag.PORTE != null ? laDemande.Ag.PORTE : string.Empty;
                    Txt_Etage.Text = laDemande.Ag.ETAGE != null ? laDemande.Ag.ETAGE : string.Empty;
                    #endregion
                }

            }
            catch (Exception ex)
            {
                Message.ShowInformation("Erreur à l'affichage des données", "Init");
            }
        }
        private void RenseignerClient(CsDemande laDemande)
        {
            try
            {
                if (laDemande != null)
                {
                    #region Client
                    if (laDemande.LeClient != null)
                    {

                        if (laDemande.LeClient.FK_TYPECLIENT != null && laDemande.LeClient.FK_TYPECLIENT != 0)
                        {
                            CsTypeClient TypeClient = SessionObject.LstTypeClient.FirstOrDefault(t => t.PK_ID == laDemande.LeClient.FK_TYPECLIENT);
                            this.Cbo_Type_Client.SelectedItem = TypeClient;
                        }
                        if (laDemande.LeClient.FK_TYPECLIENT == null || laDemande.LeClient.FK_TYPECLIENT == 0)
                        {
                            CsTypeClient TypeClient = SessionObject.LstTypeClient.FirstOrDefault(t => t.CODE.Trim() == "001");
                            this.Cbo_Type_Client.SelectedItem = TypeClient;
                        }
                        foreach (CsCategorieClient categorieClient in Cbo_Categorie.Items)
                        {
                            if (categorieClient.PK_ID == laDemande.LeClient.FK_IDCATEGORIE)
                            {
                                Cbo_Categorie.SelectedItem = categorieClient;
                                break;
                            }
                        }
                        if (laDemande.LeClient.FK_IDNATIONALITE != 0)
                        {
                            ServiceAccueil.CsNationalite laNation = SessionObject.LstDesNationalites.FirstOrDefault(t => t.PK_ID == laDemande.LeClient.FK_IDNATIONALITE);
                            this.Cbo_Nationnalite.SelectedItem = laNation;
                        }
                        foreach (ObjPIECEIDENTITE piece in Cbo_TypePiecePersonnePhysique.Items)
                        {
                            if (piece.PK_ID == laDemande.LeClient.FK_IDPIECEIDENTITE)
                            {
                                Cbo_TypePiecePersonnePhysique.SelectedItem = piece;
                                break;
                            }
                        }

                        if (laDemande.LeClient.FK_IDCODECONSO != 0)
                        {
                            ServiceAccueil.CsCodeConsomateur codeConso = SessionObject.LstCodeConsomateur.FirstOrDefault(t => t.PK_ID == laDemande.LeClient.FK_IDCODECONSO);
                            this.Cbo_CodeConso.SelectedItem = codeConso;
                        }
                        if (laDemande.LeClient.FK_IDREGROUPEMENT != 0)
                        {
                            ServiceAccueil.CsRegCli regroup = SessionObject.LstCodeRegroupement.FirstOrDefault(t => t.PK_ID == laDemande.LeClient.FK_IDREGROUPEMENT);
                            this.Cbo_Regroupement.SelectedItem = regroup;
                        }
                        if (!string.IsNullOrWhiteSpace(laDemande.LeClient.PROPRIO))
                        {
                            CsProprietaire typeprop = SessionObject.Lsttypeprop.FirstOrDefault(t => t.CODE == laDemande.LeClient.PROPRIO);
                            this.Cbo_Type_Proprietaire.SelectedItem = typeprop;
                        }

                        RemplirInfopersonnephysique(laDemande);
                        RemplirInfoSocietePrive(laDemande);
                        RemplirInfoAdmnistrationInstitut(laDemande);
                        RemplirInfoPropritaire(laDemande);

                        Txt_NumFax.Text = !string.IsNullOrEmpty(laDemande.LeClient.FAX) ? laDemande.LeClient.FAX : string.Empty;
                        Txt_BoitePostale.Text = !string.IsNullOrEmpty(laDemande.LeClient.BOITEPOSTAL) ? laDemande.LeClient.BOITEPOSTAL : string.Empty;
                        TxtCategorieClient.Text = !string.IsNullOrEmpty(laDemande.LeClient.CATEGORIE) ? laDemande.LeClient.CATEGORIE : string.Empty;
                        //txtNumeroPiece.Text = !string.IsNullOrEmpty(laDemande.LeClient.NUMEROPIECEIDENTITE) ? laDemande.LeClient.NUMEROPIECEIDENTITE : string.Empty;
                        txtAdresse.Text = !string.IsNullOrEmpty(laDemande.LeClient.ADRMAND1) ? laDemande.LeClient.ADRMAND1 : string.Empty;
                        txt_Telephone.Text = !string.IsNullOrEmpty(laDemande.LeClient.TELEPHONE) ? laDemande.LeClient.TELEPHONE : string.Empty;
                        Txt_Numeronina.Text = !string.IsNullOrEmpty(laDemande.LeClient.CODEIDENTIFICATIONNATIONALE) ? laDemande.LeClient.CODEIDENTIFICATIONNATIONALE : string.Empty;
                        txt_ordre.Text = !string.IsNullOrEmpty(laDemande.LeClient.ORDRE) ? laDemande.LeClient.ORDRE : string.Empty;
                        Txt_Email.Text = !string.IsNullOrEmpty(laDemande.LeClient.EMAIL) ? laDemande.LeClient.EMAIL : string.Empty;
                        txt_Telephone_Fixe.Text = !string.IsNullOrEmpty(laDemande.LeClient.TELEPHONEFIXE) ? laDemande.LeClient.TELEPHONEFIXE : string.Empty;
                        chk_SMS.IsChecked = laDemande.LeClient.ISFACTURESMS != null ? laDemande.LeClient.ISFACTURESMS : false;
                        chk_Email.IsChecked = laDemande.LeClient.ISFACTUREEMAIL != null ? laDemande.LeClient.ISFACTUREEMAIL : false;

                    }
                    #endregion
                }

            }
            catch (Exception ex)
            {
                Message.ShowInformation("Erreur à l'affichage des données", "Init");
            }
        }
        private void RenseignerAbon(CsDemande laDemande)
        {
            try
            {
                if (laDemande != null)
                {
                    #region Abon
                    if (laDetailDemande.Abonne != null)
                    {
                        this.Txt_CodeTarif.Text = !string.IsNullOrEmpty(laDetailDemande.Abonne.TYPETARIF) ? laDetailDemande.Abonne.TYPETARIF : string.Empty;

                        if (laDetailDemande.Abonne.PUISSANCE != null)
                            this.Txt_CodePussanceSoucrite.Text = Convert.ToDecimal(laDetailDemande.Abonne.PUISSANCE.ToString()).ToString("N2");

                        if (laDetailDemande.Branchement  != null && laDetailDemande.Branchement.PUISSANCEINSTALLEE != null )
                            this.Txt_CodePuissanceUtilise.Text = Convert.ToDecimal(laDetailDemande.Branchement.PUISSANCEINSTALLEE).ToString("N2");

                        this.Txt_CodeRistoune.Text = string.IsNullOrEmpty(laDetailDemande.Abonne.RISTOURNE.ToString()) ? string.Empty : Convert.ToDecimal(laDetailDemande.Abonne.RISTOURNE.Value).ToString("N2");

                        this.Txt_CodeForfait.Text = string.IsNullOrEmpty(laDetailDemande.Abonne.FORFAIT) ? string.Empty : laDetailDemande.Abonne.FORFAIT;
                        this.Txt_LibelleForfait.Text = string.IsNullOrEmpty(laDetailDemande.Abonne.LIBELLEFORFAIT) ? string.Empty : laDetailDemande.Abonne.LIBELLEFORFAIT;

                        this.Txt_CodeTarif.Text = string.IsNullOrEmpty(laDetailDemande.Abonne.TYPETARIF) ? string.Empty : laDetailDemande.Abonne.TYPETARIF;
                        this.Txt_LibelleTarif.Text = !string.IsNullOrEmpty(laDetailDemande.Abonne.LIBELLETARIF) ? laDetailDemande.Abonne.LIBELLETARIF : string.Empty;

                        this.Txt_CodeFrequence.Text = string.IsNullOrEmpty(laDetailDemande.Abonne.PERFAC) ? string.Empty : laDetailDemande.Abonne.PERFAC;
                        this.Txt_LibelleFrequence.Text = !string.IsNullOrEmpty(laDetailDemande.Abonne.LIBELLEFREQUENCE) ? laDetailDemande.Abonne.LIBELLEFREQUENCE : string.Empty;

                        this.Txt_CodeMoisIndex.Text = string.IsNullOrEmpty(laDetailDemande.Abonne.MOISREL) ? string.Empty : laDetailDemande.Abonne.MOISREL;
                        this.Txt_LibelleMoisIndex.Text = !string.IsNullOrEmpty(laDetailDemande.Abonne.LIBELLEMOISIND) ? laDetailDemande.Abonne.LIBELLEMOISIND : string.Empty;

                        this.Txt_CodeMoisFacturation.Text = string.IsNullOrEmpty(laDetailDemande.Abonne.MOISFAC) ? string.Empty : laDetailDemande.Abonne.MOISFAC;
                        this.Txt_LibMoisFact.Text = !string.IsNullOrEmpty(laDetailDemande.Abonne.LIBELLEMOISFACT) ? laDetailDemande.Abonne.LIBELLEMOISFACT : string.Empty;

                        this.Txt_DateAbonnement.Text = (laDetailDemande.Abonne.DABONNEMENT == null) ? DateTime.Now.ToShortDateString() : Convert.ToDateTime(laDetailDemande.Abonne.DABONNEMENT.Value).ToShortDateString();

                        this.Chk_IsExonneration.IsChecked = (laDetailDemande.Abonne.ESTEXONERETVA == true) ? true : false;
                        this.txt_DebutPeriodeExo.Text = string.IsNullOrEmpty(laDetailDemande.Abonne.DEBUTEXONERATIONTVA) ? string.Empty :
                                                 Shared.ClasseMEthodeGenerique.FormatPeriodeMMAAAA(laDetailDemande.Abonne.DEBUTEXONERATIONTVA);

                        this.txt_FinPeriodeExo.Text = string.IsNullOrEmpty(laDetailDemande.Abonne.FINEXONERATIONTVA) ? string.Empty :
                                   Shared.ClasseMEthodeGenerique.FormatPeriodeMMAAAA(laDetailDemande.Abonne.FINEXONERATIONTVA);

                        if (laDetailDemande.Abonne.PRODUIT == SessionObject.Enumere.ElectriciteMT)
                        {
                            this.lbl_TypeDeComptage.Visibility = System.Windows.Visibility.Visible;
                            this.Txt_TypeDeComptage.Visibility = System.Windows.Visibility.Visible;
                            this.Txt_TypeDeComptage.Text = string.IsNullOrEmpty(laDetailDemande.Abonne.LIBELLETYPECOMPTAGE) ? string.Empty : laDetailDemande.Abonne.LIBELLETYPECOMPTAGE;
                        }
                    }
                    else
                        this.Txt_DateAbonnement.Text = DateTime.Now.ToShortDateString();

                    //if (laDemandeSelect.PRODUIT != SessionObject.Enumere.ElectriciteMT)
                    //{
                    //}
                    //else
                    //{

                    //    int? idPuissance = null;
                    //    if (this.Txt_CodePussanceSoucrite.Tag != null)
                    //        idPuissance = (int)this.Txt_CodePussanceSoucrite.Tag;

                    //    int? idCategorie = null;
                    //    if (Cbo_Categorie.SelectedItem != null)
                    //        idCategorie = (Cbo_Categorie.SelectedItem as CsCategorieClient).PK_ID;


                    //    int idProduit = 0;
                    //    if (laDetailDemande.LaDemande.FK_IDPRODUIT != null)
                    //        idProduit = laDetailDemande.LaDemande.FK_IDPRODUIT.Value;


                    //    int? idReglage = null;
                    //    if (this.txt_Reglage.Tag != null)
                    //        idReglage = (int)this.txt_Reglage.Tag;

                    //    if (idCategorie != null && idProduit != null && idReglage != null )
                    //    ChargerPuissanceEtTarif(idProduit, idPuissance, idCategorie, idReglage);
                    //}
                    ChargerFrequence(laDetailDemande);
                    ChargerMois(laDetailDemande);
                    if (laDemandeSelect.PRODUIT != SessionObject.Enumere.ElectriciteMT)
                    {
                        int? idPuissance = null;
                        if (this.Txt_CodePussanceSoucrite.Tag != null)
                            idPuissance = (int)this.Txt_CodePussanceSoucrite.Tag;

                        int? idCategorie = null;
                        if (Cbo_Categorie.SelectedItem != null)
                            idCategorie = (Cbo_Categorie.SelectedItem as CsCategorieClient).PK_ID;


                        int idProduit = 0;
                        if (laDetailDemande.LaDemande.FK_IDPRODUIT != null)
                            idProduit = laDetailDemande.LaDemande.FK_IDPRODUIT.Value;


                        int? idReglage = null;
                        if (this.txt_Reglage.Tag != null)
                            idReglage = (int)this.txt_Reglage.Tag;

                        if (idCategorie != null && idProduit != null && idReglage != null)
                        ChargerPuissanceEtTarif(idProduit, idPuissance, idCategorie, idReglage,null);
                    }
                    else
                    {
                        ChargerTarifParCategorieMt(laDetailDemande);
                        ChargerPuissance();
                    }
                    ChargerForfait(laDetailDemande);
                    #endregion
                }

            }
            catch (Exception ex)
            {
                Message.ShowInformation("Erreur à l'affichage des données", "Init");
            }
        }
        private void RenseignerDemande(CsDemande laDemande)
        {
            try
            {
                if (laDemande != null)
                {
                    #region Demande
                    if (laDemande.LaDemande != null)
                    {
                        Txt_NumDevis.Text = !string.IsNullOrEmpty(laDemande.LaDemande.NUMDEM) ? laDemande.LaDemande.NUMDEM : string.Empty;
                        if (laDemande.LaDemande.FK_IDREGLAGECOMPTEUR != null)
                        {
                            CsReglageCompteur leRegl = SessionObject.LstReglageCompteur.FirstOrDefault(o => o.PK_ID == laDemande.LaDemande.FK_IDREGLAGECOMPTEUR);
                            if (leRegl != null)
                            {
                                this.txt_Reglage.Text = leRegl.LIBELLE;
                                this.txt_Reglage.Tag = leRegl.PK_ID;
                                this.Btn_Reglage.Tag = leRegl.CODE;
                            }
                        }
                        if (laDemande.LaDemande.PRODUIT != SessionObject.Enumere.ElectriciteMT)
                        {
                            label21.Visibility = Visibility.Visible;
                            txt_Reglage.Visibility = Visibility.Visible;
                            Btn_Reglage.Visibility = Visibility.Visible;
                        }
                    }
                    #endregion
                    if (laDetailDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.AugmentationPuissance ||
                        laDetailDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.DimunitionPuissance ||
                        laDetailDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.ChangementProduit  ||
                        laDetailDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.BranchementAbonnementEp)
                        tabItemClientInfo.Visibility = System.Windows.Visibility.Collapsed;

                    if (laDetailDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.BranchementAbonnementEp)
                    {
                        RemplirTourneeExistante(laDetailDemande.LaDemande.FK_IDCENTRE);
                        this.Cbo_Zone.Visibility = System.Windows.Visibility.Visible;
                        this.label4.Visibility = System.Windows.Visibility.Visible;
                        this.TxtOrdreTournee.Visibility = System.Windows.Visibility.Visible;
                        label8.Visibility = System.Windows.Visibility.Visible;
                    }
                    if (laDetailDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.AugmentationPuissance &&
                        laDetailDemande.LaDemande.PRODUIT == SessionObject.Enumere.ElectriciteMT)
                    {
                        this.Chk_FactureDiff.Visibility = System.Windows.Visibility.Visible;
                    }
                }

            }
            catch (Exception ex)
            {
                Message.ShowInformation("Erreur à l'affichage des données", "Init");
            }
        }
        private void RenseignerFourniture(CsDemande laDemande)
        {
            try
            {
                if (laDemande != null)
                {
                    #region Fourniture

                    if (laDemande != null && (laDemande.EltDevis != null && laDemande.EltDevis.Count > 0))
                    {

                        if (laDemande.LaDemande.PRODUIT != SessionObject.Enumere.ElectriciteMT)
                        {
                            //this.dataGridElementDevis.Visibility = System.Windows.Visibility.Collapsed;
                            //this.dataGridForniture.Visibility = System.Windows.Visibility.Visible;
                            RemplirListeMateriel(laDemande.EltDevis);
                        }
                        else
                        {
                            //this.dataGridElementDevis.Visibility = System.Windows.Visibility.Visible;
                            //this.dataGridForniture.Visibility = System.Windows.Visibility.Collapsed;
                            RemplirListeMaterielMT(laDemande.EltDevis, SessionObject.LstRubriqueDevis);
                        }
                    }
                    if (laDemande.LaDemande.ISMETREAFAIRE == false &&
                        (laDetailDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.AugmentationPuissance ||
                         laDetailDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.DimunitionPuissance ||
                         laDetailDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.Reabonnement  ||  /* Afficher les couts pour le reabonnement 23/06/2017 */
                         laDetailDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.AbonnementSeul))
                        ChargerCoutDemande();
                    this.tabC_Onglet.SelectedIndex = 4;
                    #endregion
                }
            }
            catch (Exception ex)
            {
                Message.ShowInformation("Erreur à l'affichage des données", "Init");
            }
        }
        private void RenseignerAppareil(CsDemande laDemande)
        {
            try
            {
                if (laDemande != null)
                {
                    #region Apperiel
                    if (laDemande.AppareilDevis != null && laDemande.AppareilDevis.Count != 0)
                    {
                        Cbo_ListeAppareils.Items.Clear();
                        List<ObjAPPAREILS> lstAppareil = new List<ObjAPPAREILS>();
                        foreach (ObjAPPAREILSDEVIS item in laDemande.AppareilDevis)
                        {
                            ObjAPPAREILS Appareil = new ObjAPPAREILS()
                            {
                                CODEAPPAREIL = item.CODEAPPAREIL,
                                DESIGNATION = item.DESIGNATION,
                                NOMBRE = item.NBRE.Value,
                                PUISSANCE = item.PUISSANCE.Value,
                                TEMPSUTILISATION = item.TEMPSUTILISATION,
                                DISPLAYLABEL = item.DESIGNATION,
                                PK_IDAPPAREILDEVIS = item.FK_IDCODEAPPAREIL,
                                PK_ID = item.FK_IDCODEAPPAREIL
                            };

                            Cbo_ListeAppareils.Items.Add(Appareil);
                        }
                        Cbo_ListeAppareils.SelectedValuePath = "CODEAPPAREIL";
                        Cbo_ListeAppareils.DisplayMemberPath = "DESIGNATION";
                        Cbo_ListeAppareils.SelectedIndex = 0;
                    }
                    #endregion
                }

            }
            catch (Exception ex)
            {
                Message.ShowInformation("Erreur à l'affichage des données", "Init");
            }
        }
        private void RenseignerInformationsBrt(CsDemande laDemande)
        {
            try
            {
                if (laDemande != null && laDemande.Branchement != null)
                {
                    if (laDemande.LaDemande.PRODUIT != SessionObject.Enumere.ElectriciteMT)
                    {
                        if (SessionObject.LstReglageCompteur != null && SessionObject.LstReglageCompteur.Count != 0)
                            this.Txt_Typedecompteur.Text = SessionObject.LstReglageCompteur.FirstOrDefault(t => t.CODE == laDemande.LaDemande.REGLAGECOMPTEUR).LIBELLE;
                    }
                    this.Txt_Distance.Text = laDetailDemande.Branchement.LONGBRT == null ? string.Empty : laDetailDemande.Branchement.LONGBRT.ToString();
                    this.Txt_Tournee.Text = string.IsNullOrEmpty(laDetailDemande.Ag.TOURNEE) ? string.Empty : laDetailDemande.Ag.TOURNEE;
                    this.TxtOrdreTournee.Text = string.IsNullOrEmpty(laDetailDemande.Ag.ORDTOUR) ? string.Empty : laDetailDemande.Ag.ORDTOUR;
                    this.TxtOrdreTournee1.Text = string.IsNullOrEmpty(laDetailDemande.Ag.ORDTOUR) ? string.Empty : laDetailDemande.Ag.ORDTOUR;
                    this.TxtPuissance.Text = laDetailDemande.LaDemande.PUISSANCESOUSCRITE == null ? string.Empty : laDetailDemande.LaDemande.PUISSANCESOUSCRITE.Value.ToString(SessionObject.FormatMontant);
                    this.TxtLongitude.Text = string.IsNullOrEmpty(laDetailDemande.Branchement.LONGITUDE) ? string.Empty : laDetailDemande.Branchement.LONGITUDE;
                    this.TxtLatitude.Text = string.IsNullOrEmpty(laDetailDemande.Branchement.LATITUDE) ? string.Empty : laDetailDemande.Branchement.LATITUDE;

                    dgListeFraixParicipation.ItemsSource = null;
                    dgListeFraixParicipation.ItemsSource = laDemande.LstFraixParticipation;

                    Txt_LibelleTypeBrt.Text = string.IsNullOrEmpty(laDetailDemande.Branchement.LIBELLETYPEBRANCHEMENT) ? string.Empty : laDetailDemande.Branchement.LIBELLETYPEBRANCHEMENT;
                    Txt_LibellePosteSource.Text = string.IsNullOrEmpty(laDetailDemande.Branchement.LIBELLEPOSTESOURCE) ? string.Empty : laDetailDemande.Branchement.LIBELLEPOSTESOURCE;
                    Txt_LibelleDepartHTA.Text = string.IsNullOrEmpty(laDetailDemande.Branchement.LIBELLEDEPARTHTA) ? string.Empty : laDetailDemande.Branchement.LIBELLEDEPARTHTA;
                    Txt_LibelleQuartierPoste.Text = string.IsNullOrEmpty(laDetailDemande.Branchement.LIBELLEQUARTIER) ? string.Empty : laDetailDemande.Branchement.LIBELLEQUARTIER;
                    Txt_LibellePosteTransformateur.Text = string.IsNullOrEmpty(laDetailDemande.Branchement.LIBELLETRANSFORMATEUR) ? string.Empty : laDetailDemande.Branchement.LIBELLETRANSFORMATEUR;
                    Txt_LibelleDepartBt.Text = string.IsNullOrEmpty(laDetailDemande.Branchement.DEPARTBT) ? string.Empty : laDetailDemande.Branchement.DEPARTBT;

                    Txt_BranchementProche.Text = laDetailDemande.Branchement.NBPOINT == null ? "0" : laDetailDemande.Branchement.NBPOINT.ToString();
                    AfficherOuMasquer(tabItemMetre, true);
                }
                else
                    AfficherOuMasquer(tabItemMetre, false);

            }
            catch (Exception ex)
            {
                Message.ShowInformation("Erreur au chargement du metré", "Demande");
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
 

        private void ActivationEnFonctionDeTdem()
        {
            try
            {
                //if (txt_tdem.Tag != null)
                //{
                if (this.Tdem != SessionObject.Enumere.BranchementAbonement)
                {
                    lab_ListeAppareils.Visibility = Visibility.Collapsed;
                    Cbo_ListeAppareils.Visibility = Visibility.Collapsed;
                    Btn_ListeAppareils.Visibility = Visibility.Collapsed;

                    label21.Visibility = Visibility.Collapsed;
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
                    label21.Visibility = Visibility.Visible;
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


        private void GetAdministraionInstitutData(CsDemande pDemandeDevis)
        {
            pDemandeDevis.AdministrationInstitut = new CsAdministration_Institut();

            pDemandeDevis.AdministrationInstitut.PK_ID = Pk_IdAdministration != 0 ? Pk_IdAdministration : 0;
            pDemandeDevis.AdministrationInstitut.FK_IDDEMANDE = pDemandeDevis.LaDemande.PK_ID;
            pDemandeDevis.AdministrationInstitut.NOMMANDATAIRE = Txt_NomMandataireAdministration.Text;
            pDemandeDevis.AdministrationInstitut.PRENOMMANDATAIRE = Txt_PrenomMandataireAdministration.Text;
            pDemandeDevis.AdministrationInstitut.RANGMANDATAIRE = Txt_RangMandataireAdministration.Text;
            pDemandeDevis.AdministrationInstitut.NOMSIGNATAIRE = Txt_NomSignataireAdministration.Text;
            pDemandeDevis.AdministrationInstitut.PRENOMSIGNATAIRE = Txt_PrenomSignataireAdministration.Text;
            pDemandeDevis.AdministrationInstitut.RANGSIGNATAIRE = Txt_RangSignataireAdministration.Text;
            pDemandeDevis.AdministrationInstitut.NOMABON = Txt_NomClientAdministration.Text;
            pDemandeDevis.LeClient.NOMABON = Txt_NomClientAdministration.Text;

        }

        private void GetPersonnPhyqueData(CsDemande pDemandeDevis)
        {
            int? mynull = null;
            pDemandeDevis.PersonePhysique = new CsPersonePhysique();
            pDemandeDevis.PersonePhysique.PK_ID = Pk_IdPersPhys != 0 ? Pk_IdPersPhys : 0;
            pDemandeDevis.PersonePhysique.NOMABON = Txt_NomClient_PersonePhysiq.Text;
            pDemandeDevis.PersonePhysique.DATEFINVALIDITE = Convert.ToDateTime(dtp_DateValidite.Text);
            pDemandeDevis.PersonePhysique.DATENAISSANCE = Convert.ToDateTime(dtp_DateNaissance.Text);
            pDemandeDevis.PersonePhysique.NUMEROPIECEIDENTITE = txtNumeroPiece.Text.Trim();
            pDemandeDevis.PersonePhysique.FK_IDPIECEIDENTITE = Cbo_TypePiecePersonnePhysique.SelectedItem != null ? ((ObjPIECEIDENTITE)Cbo_TypePiecePersonnePhysique.SelectedItem).PK_ID : mynull;
            pDemandeDevis.LeClient.NOMABON = Txt_NomClient_PersonePhysiq.Text;
            pDemandeDevis.LeClient.NUMEROPIECEIDENTITE = txtNumeroPiece.Text.Trim();
            pDemandeDevis.LeClient.FK_IDPIECEIDENTITE = Cbo_TypePiecePersonnePhysique.SelectedItem != null ? ((ObjPIECEIDENTITE)Cbo_TypePiecePersonnePhysique.SelectedItem).PK_ID : mynull;



        }
        private CsDemande GetSocietePriveData(CsDemande pDemandeDevis)
        {
            pDemandeDevis.SocietePrives = new CsSocietePrive();
            int? Mynull = null;
            decimal capital = 0;
            if (!decimal.TryParse(Txt_Capital.Text, out capital))
            {
                Message.Show("veuillez saisir une valeur numerique", "Demande");
                return null;
            }
            pDemandeDevis.SocietePrives.PK_ID = Pk_IdSocoiete != 0 ? Pk_IdSocoiete : 0;
            pDemandeDevis.SocietePrives.NUMEROREGISTRECOMMERCE = Txt_RegistreCommerce.Text;
            pDemandeDevis.SocietePrives.FK_IDSTATUTJURIQUE = Cbo_StatutJuridique.SelectedItem != null ? ((CsStatutJuridique)Cbo_StatutJuridique.SelectedItem).PK_ID : Mynull;
            pDemandeDevis.SocietePrives.CAPITAL = capital;
            pDemandeDevis.SocietePrives.IDENTIFICATIONFISCALE = Txt_IdentiteFiscale.Text;
            pDemandeDevis.SocietePrives.DATECREATION = dtp_DateCreation.SelectedDate;
            pDemandeDevis.SocietePrives.SIEGE = Txt_Siege.Text;
            pDemandeDevis.SocietePrives.NOMMANDATAIRE = Txt_NomMandataire.Text;
            pDemandeDevis.SocietePrives.PRENOMMANDATAIRE = Txt_PrenomMandataire.Text;
            pDemandeDevis.SocietePrives.RANGMANDATAIRE = Txt_RangMandataire.Text;
            pDemandeDevis.SocietePrives.NOMSIGNATAIRE = Txt_NomSignataire.Text;
            pDemandeDevis.SocietePrives.PRENOMSIGNATAIRE = Txt_PrenomSignataire.Text;
            pDemandeDevis.SocietePrives.RANGSIGNATAIRE = Txt_RangSignataire.Text;
            pDemandeDevis.SocietePrives.NOMABON = Txt_NomClientSociete.Text;
            pDemandeDevis.LeClient.NOMABON = Txt_NomClientSociete.Text;

            return pDemandeDevis;
        }

        private CsDemande GetSocieteProprietaire(CsDemande pDemandeDevis)
        {
            pDemandeDevis.InfoProprietaire_ = new CsInfoProprietaire();
            int? Mynull = null;

            pDemandeDevis.InfoProprietaire_.PK_ID = Pk_IdPropietaire != 0 ? Pk_IdPropietaire : 0;
            pDemandeDevis.InfoProprietaire_.FK_IDNATIONNALITE = Cbo_Nationalite_Proprio.SelectedItem != null ? ((Galatee.Silverlight.ServiceAccueil.CsNationalite)Cbo_Nationalite_Proprio.SelectedItem).PK_ID : Mynull;
            pDemandeDevis.InfoProprietaire_.BOITEPOSTALE = Txt_BoitePosta_Proprio.Text;
            pDemandeDevis.InfoProprietaire_.DATEFINVALIDITE = dtp_finvalidationProprio.SelectedDate;
            pDemandeDevis.InfoProprietaire_.DATENAISSANCE = dtp_DateNaissanceProprio.SelectedDate;
            pDemandeDevis.InfoProprietaire_.EMAIL = Txt_Email_Proprio.Text;

            if (!string.IsNullOrWhiteSpace(this.Txt_Email_Proprio.Text))
            {
                if (IsEmail(this.Txt_Email_Proprio.Text))
                {
                    pDemandeDevis.InfoProprietaire_.EMAIL = string.IsNullOrEmpty(this.Txt_Email_Proprio.Text) ? null : this.Txt_Email_Proprio.Text;
                }
                else
                {
                    Message.Show("Veuillez saisi un email propriétaire correct", "Erreur");
                    return null;
                }
            }
            pDemandeDevis.InfoProprietaire_.FAX = Txt_Faxe_Proprio.Text;
            pDemandeDevis.InfoProprietaire_.FK_IDCLIENT = pDemandeDevis.LeClient.PK_ID;
            pDemandeDevis.InfoProprietaire_.FK_IDPIECEIDENTITE = Cbo_TypePiecePersonnePhysiqueProprio.SelectedItem != null ? ((ObjPIECEIDENTITE)Cbo_TypePiecePersonnePhysiqueProprio.SelectedItem).PK_ID : Mynull;
            pDemandeDevis.InfoProprietaire_.NOM = Txt_NomProprio_PersonePhysiq.Text;
            pDemandeDevis.InfoProprietaire_.PRENOM = Txt_PrenomProprio_PersonePhysiq.Text;
            pDemandeDevis.InfoProprietaire_.TELEPHONEMOBILE = txt_Telephone_Proprio.Text;
            pDemandeDevis.InfoProprietaire_.NUMEROPIECEIDENTITE = txtNumeroPieceProprio.Text.Trim();


            return pDemandeDevis;
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
                if (laDetailDemande.LaDemande.FK_IDPRODUIT  != null)
                {
                    var UcListReglage = new Galatee.Silverlight.Accueil.UcListeReglageCompteur(_listeDesReglageCompteurExistant.Where(t => t.FK_IDPRODUIT == laDetailDemande.LaDemande.FK_IDPRODUIT).ToList());
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

                        int? idPuissance = null;
                   

                        int? idCategorie = null;
                        if (Cbo_Categorie.SelectedItem != null)
                            idCategorie = (Cbo_Categorie.SelectedItem as CsCategorieClient).PK_ID;


                        int idProduit = 0;
                        if (laDetailDemande.LaDemande.FK_IDPRODUIT != null)
                            idProduit = laDetailDemande.LaDemande.FK_IDPRODUIT.Value;


                        int? idReglage = null;
                        if (this.txt_Reglage.Tag != null)
                            idReglage = (int)this.txt_Reglage.Tag;

                        if (idCategorie != null && idProduit != null && idReglage != null)
                        ChargerPuissanceEtTarif(idProduit, idPuissance, idCategorie, ctrs.leReglageSelect.PK_ID,null );
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
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
        private void Cbo_Categorie_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (Cbo_Categorie.SelectedItem != null)
                {
                    var cat = ((CsCategorieClient)Cbo_Categorie.SelectedItem);
                    ReloadUsageForCateg(cat);

                    int? idPuissance = null;
 

                    int? idCategorie = null;
                    if (Cbo_Categorie.SelectedItem != null)
                        idCategorie = (Cbo_Categorie.SelectedItem as CsCategorieClient).PK_ID;


                    int idProduit = 0;
                    if (laDetailDemande.LaDemande.FK_IDPRODUIT != null)
                        idProduit = laDetailDemande.LaDemande.FK_IDPRODUIT.Value;


                    int? idReglage = null;
                    if (this.txt_Reglage.Tag != null)
                        idReglage = (int)this.txt_Reglage.Tag;

                    if (cat != null)
                    {
                        TxtCategorieClient.Text = cat.CODE ?? string.Empty;
                        this.TxtCategorieClient.Tag = cat.PK_ID;

                        if (idCategorie != null && idProduit != null && idReglage != null)
                        ChargerPuissanceEtTarif(idProduit, idPuissance, cat.PK_ID, idReglage,null );
                        if (cat.CODE == SessionObject.Enumere.CategorieAgentEdm)
                        {
                            this.txt_MaticuleAgent.Visibility = System.Windows.Visibility.Visible;
                            this.lbl_MatriculeAgent.Visibility = System.Windows.Visibility.Visible;
                        }
                        else
                        {
                            this.txt_MaticuleAgent.Visibility = System.Windows.Visibility.Collapsed ;
                            this.lbl_MatriculeAgent.Visibility = System.Windows.Visibility.Collapsed ;
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
                        Txt_CodeRegroupement.Text = Regroupement.CODE ?? string.Empty;
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
                this.Txt_NomMandataire.Text = (laDemande.SocietePrives.NOMMANDATAIRE == null) ? string.Empty : laDemande.SocietePrives.NOMMANDATAIRE;
                this.Txt_PrenomMandataire.Text = (laDemande.SocietePrives.PRENOMMANDATAIRE == null) ? string.Empty : laDemande.SocietePrives.PRENOMMANDATAIRE;
                //this.Txt_NomClientSociete.Text = (laDemande.SocietePrives.NOMABON == null) ? string.Empty : laDemande.SocietePrives.NOMABON;
                this.Txt_NomClientSociete.Text = (laDemande.LeClient.NOMABON == null) ? string.Empty : laDemande.LeClient.NOMABON;
                this.Txt_RangMandataire.Text = (laDemande.SocietePrives.RANGMANDATAIRE == null) ? string.Empty : laDemande.SocietePrives.RANGMANDATAIRE;
                this.Txt_NomSignataire.Text = (laDemande.SocietePrives.NOMSIGNATAIRE == null) ? string.Empty : laDemande.SocietePrives.NOMSIGNATAIRE;
                this.Txt_PrenomSignataire.Text = (laDemande.SocietePrives.PRENOMSIGNATAIRE == null) ? string.Empty : laDemande.SocietePrives.PRENOMSIGNATAIRE;
                this.Txt_RangSignataire.Text =  (laDemande.SocietePrives.RANGSIGNATAIRE == null) ? string.Empty : laDemande.SocietePrives.RANGSIGNATAIRE;
                this.dtp_DateCreation.SelectedDate = laDemande.SocietePrives.DATECREATION;
                this.Cbo_StatutJuridique.SelectedItem = ListStatuJuridique.FirstOrDefault(t => t.PK_ID == laDemande.SocietePrives.FK_IDSTATUTJURIQUE);

            }

        }
        private void RemplirInfopersonnephysique(CsDemande laDemande)
        {
            if (laDemande.PersonePhysique != null)
            {
                Pk_IdPersPhys = laDemande.PersonePhysique.PK_ID != null ? laDemande.PersonePhysique.PK_ID : 0;
                //this.Txt_NomClient_PersonePhysiq.Text = laDemande.PersonePhysique.NOMABON != null ? laDemande.PersonePhysique.NOMABON : string.Empty;
                this.Txt_NomClient_PersonePhysiq.Text = laDemande.LeClient.NOMABON != null ? laDemande.LeClient.NOMABON : string.Empty;
                this.Txt_Civilite.Text = laDemande.LeClient.DENABON != null ? laDemande.LeClient.DENABON : string.Empty;
                this.Txt_libelleCivilite.Text = laDemande.LeClient.LIBELLEDENOMINATION != null ? laDemande.LeClient.LIBELLEDENOMINATION : string.Empty;
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
                //this.Txt_NomClientAdministration.Text = !string.IsNullOrEmpty(laDemande.AdministrationInstitut.NOMABON) ? laDemande.AdministrationInstitut.NOMABON : string.Empty;
                this.Txt_NomClientAdministration.Text = !string.IsNullOrEmpty(laDemande.LeClient.NOMABON) ? laDemande.LeClient.NOMABON : string.Empty;
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
       
        private int Pk_IdPropietaire = 0;

        private int Pk_IdPersPhys = 0;
        private int Pk_IdSocoiete = 0;
        private int Pk_IdAdministration = 0;
        private int PK_Id_Tdem = 0;

   
 
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

        private void Btn_Transmettre_Click(object sender, RoutedEventArgs e)
        {

            if (!VerifieChampObligation()) return;

            this.Btn_Rejeter.IsEnabled = false;
            this.Btn_Transmettre.IsEnabled = false;
            if (laDetailDemande.InfoDemande.CODEETAPE != "ENCAI")
                ValidationDevis(laDetailDemande, true);
            else
                ValidationDevis(laDetailDemande, false);

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
            if (laDetailDemande.LaDemande.FK_IDCENTRE  != null)
                idCentre = laDetailDemande.LaDemande.FK_IDCENTRE;

            int idCategorie = 0;
            if (Cbo_Categorie.SelectedItem != null)
                idCategorie = (Cbo_Categorie.SelectedItem as CsCategorieClient).PK_ID;

            int idReglageCompteur = 0;
            if (this.txt_Reglage.Tag != null)
                idReglageCompteur = (int)this.txt_Reglage.Tag;

            int idProduit = 0;
            if (laDetailDemande.LaDemande.FK_IDPRODUIT  != null)
                idProduit = laDetailDemande.LaDemande.FK_IDPRODUIT.Value ;

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
                if (laDetailDemande.LaDemande.PRODUIT != SessionObject.Enumere.ElectriciteMT)
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
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Galatee.Silverlight.Resources.Accueil.Langue.lbl_Menu);
            }
        }
        private void galatee_OkClickedBtnpuissanceSouscriteMt(object sender, EventArgs e)
        {
            try
            {
                this.IsEnabled = true;
                Galatee.Silverlight.MainView.UcListeGenerique ctrs = sender as Galatee.Silverlight.MainView.UcListeGenerique;
                if (ctrs.isOkClick)
                {
                    CsPuissance _LaPuissanceSelect = (CsPuissance)ctrs.MyObject;
                    this.Txt_CodePussanceSoucrite.Text = _LaPuissanceSelect.VALEUR.ToString();
                    this.Txt_CodePussanceSoucrite.Tag = _LaPuissanceSelect.PK_ID;

                    if (laDetailDemande.LaDemande.ISMETREAFAIRE == false &&
                   (laDetailDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.AugmentationPuissance ||
                    laDetailDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.DimunitionPuissance ||
                    laDetailDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.AbonnementSeul))
                    {
                        laDetailDemande.LaDemande.PUISSANCESOUSCRITE = _LaPuissanceSelect.VALEUR;
                        laDetailDemande.LaDemande.FK_IDPUISSANCESOUSCRITE = _LaPuissanceSelect.PK_ID;
                        ChargerCoutDemande();
                    }
                    if (laDetailDemande.LaDemande.PRODUIT == SessionObject.Enumere.ElectriciteMT &&
                        (laDetailDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.BranchementAbonnementMt))
                    {
                        if (laDetailDemande.EltDevis != null && laDetailDemande.EltDevis.Count != 0)
                        { 
                            int idCoperAvance = SessionObject.LstDesCopers.FirstOrDefault(t=>t.CODE == SessionObject.Enumere.CoperCAU ).PK_ID ;
                            ObjELEMENTDEVIS leAvance = laDetailDemande.EltDevis.FirstOrDefault(t => t.FK_IDCOPER == idCoperAvance);
                            if (leAvance != null && leAvance.QUANTITE != 0)
                            {
                                leAvance.MONTANTHT = leAvance.PRIX_UNITAIRE * (decimal)_LaPuissanceSelect.VALEUR;
                                leAvance.MONTANTTAXE = 0;
                                leAvance.QUANTITE = (int)_LaPuissanceSelect.VALEUR;
                                leAvance.MONTANTTTC = leAvance.MONTANTHT;
                                dataGridForniture.ItemsSource = null;
                                RemplirListeMaterielMT(laDetailDemande.EltDevis, SessionObject.LstRubriqueDevis);
                            }
                        }
                    
                    }
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
                this.btn_PussSouscrite.IsEnabled = true;
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Galatee.Silverlight.Resources.Accueil.Langue.lbl_Menu);
            }
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
                    this.Txt_CodePussanceSoucrite.Tag = _LaPuissanceSelect.FK_IDPUISSANCE ;

                    int idCategorie = 0;
                    if (Cbo_Categorie.SelectedItem != null)
                        idCategorie = (Cbo_Categorie.SelectedItem as CsCategorieClient).PK_ID;

                    int idReglageCompteur = 0;
                    if (this.txt_Reglage.Tag != null)
                        idReglageCompteur = (int)this.txt_Reglage.Tag;

                    int idProduit = 0;
                    if (laDetailDemande.LaDemande.FK_IDPRODUIT != null)
                        idProduit = laDetailDemande.LaDemande.FK_IDPRODUIT.Value;

                    if (idCategorie != null && idProduit != null && idReglageCompteur != null)
                    ChargerPuissanceEtTarif(idProduit, _LaPuissanceSelect.PK_ID,idCategorie,idReglageCompteur,null);
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
            //&& this.Txt_CodePussanceSoucrite .Text.Length == SessionObject.Enumere.)
            {

                CsTarif _LaPuissance = Shared.ClasseMEthodeGenerique.RetourneObjectFromList(LstPuissanceTarif, this.Txt_CodePussanceSoucrite.Text, "CODEPUISSANCE");
                if (_LaPuissance != null)
                {
                    this.Txt_CodePussanceSoucrite.Tag = _LaPuissance.PK_ID;


                
                }
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
        private void RemplirListeMateriel(List<ObjELEMENTDEVIS> lstEltDevis)
        {
            List<ObjELEMENTDEVIS> lstFourgenerale = new List<ObjELEMENTDEVIS>();
            if (lstEltDevis.Count != 0)
            {
                List<ObjELEMENTDEVIS> lstFourExtension = new List<ObjELEMENTDEVIS>();
                List<ObjELEMENTDEVIS> lstFourBranchement = new List<ObjELEMENTDEVIS>();

                lstFourExtension = lstEltDevis.Where(t => t.ISEXTENSION == true).ToList();
                lstFourBranchement = lstEltDevis.Where(t => t.ISEXTENSION == false).ToList();

                ObjELEMENTDEVIS leSeparateur = new ObjELEMENTDEVIS();
                leSeparateur.DESIGNATION = "----------------------------------";

                if (lstFourBranchement.Count != 0)
                {
                    ObjELEMENTDEVIS leResultatBranchanchement = new ObjELEMENTDEVIS();
                    leResultatBranchanchement.DESIGNATION = "TOTAL BRANCHEMENT ";
                    leResultatBranchanchement.IsCOLORIE = true;
                    leResultatBranchanchement.MONTANTHT = lstEltDevis.Where(t => t.ISEXTENSION == false).Sum(t => t.MONTANTHT);
                    leResultatBranchanchement.MONTANTTAXE = lstEltDevis.Where(t => t.ISEXTENSION == false).Sum(t => t.MONTANTTAXE);
                    leResultatBranchanchement.MONTANTTTC = lstEltDevis.Where(t => t.ISEXTENSION == false).Sum(t => t.MONTANTTTC);

                    lstFourgenerale.AddRange(lstFourBranchement);
                    lstFourgenerale.Add(leSeparateur);
                    lstFourgenerale.Add(leResultatBranchanchement);
                    lstFourgenerale.Add(new ObjELEMENTDEVIS()
                    {
                        DESIGNATION = "    "
                    });

                }
                if (lstFourExtension.Count != 0)
                {
                    ObjELEMENTDEVIS leResultatExtension = new ObjELEMENTDEVIS();
                    leResultatExtension.DESIGNATION = "TOTAL EXTENSION ";
                    leResultatExtension.IsCOLORIE = true;
                    leResultatExtension.MONTANTHT = lstEltDevis.Where(t => t.ISEXTENSION == true).Sum(t => t.MONTANTHT);
                    leResultatExtension.MONTANTTAXE = lstEltDevis.Where(t => t.ISEXTENSION == true).Sum(t => t.MONTANTTAXE);
                    leResultatExtension.MONTANTTTC = lstEltDevis.Where(t => t.ISEXTENSION == true).Sum(t => t.MONTANTTTC);

                    lstFourgenerale.AddRange(lstFourExtension);
                    lstFourgenerale.Add(leSeparateur);
                    lstFourgenerale.Add(leResultatExtension);

                    lstFourgenerale.Add(new ObjELEMENTDEVIS()
                    {
                        DESIGNATION = "    "
                    });
                }
                if (lstFourBranchement.Count != 0 || lstFourExtension.Count != 0)
                {
                    ObjELEMENTDEVIS leResultatGeneral = new ObjELEMENTDEVIS();
                    leResultatGeneral.DESIGNATION = "TOTAL GENERAL ";
                    leResultatGeneral.IsCOLORIE = true;
                    leResultatGeneral.MONTANTHT = lstEltDevis.Sum(t => t.MONTANTHT);
                    leResultatGeneral.MONTANTTAXE = lstEltDevis.Sum(t => t.MONTANTTAXE);
                    leResultatGeneral.MONTANTTTC = lstEltDevis.Sum(t => t.MONTANTTTC);
                    lstFourgenerale.Add(leSeparateur);
                    lstFourgenerale.Add(leResultatGeneral);
                }
            }
            this.dataGridForniture.ItemsSource = null;
            this.dataGridForniture.ItemsSource = lstFourgenerale;

            this.Txt_TotalTtc.Text = ((List<ObjELEMENTDEVIS>)dataGridForniture.ItemsSource).ToList().Where(t => t.QUANTITE != null && t.QUANTITE != 0).Sum(y => y.MONTANTTTC).Value.ToString(SessionObject.FormatMontant);
            this.Txt_TotalTva.Text = ((List<ObjELEMENTDEVIS>)dataGridForniture.ItemsSource).ToList().Where(t => t.QUANTITE != null && t.QUANTITE != 0).Sum(y => y.MONTANTTAXE).Value.ToString(SessionObject.FormatMontant);
            this.Txt_TotalHt.Text = ((List<ObjELEMENTDEVIS>)dataGridForniture.ItemsSource).ToList().Where(t => t.QUANTITE != null && t.QUANTITE != 0).Sum(y => y.MONTANTHT).Value.ToString(SessionObject.FormatMontant);
        }
        private void RemplirListeMaterielMT(List<ObjELEMENTDEVIS> lstEltDevis, List<CsRubriqueDevis> leRubriques)
        {
            try
            {
                ObjELEMENTDEVIS leSeparateur = new ObjELEMENTDEVIS();
                leSeparateur.LIBELLE = "----------------------------------";
                leSeparateur.ISDEFAULT = true;
                List<ObjELEMENTDEVIS> lstFourgenerale = new List<ObjELEMENTDEVIS>();

                foreach (CsRubriqueDevis item in leRubriques)
                {
                    bool MiseAZereLigne = false;
                    List<ObjELEMENTDEVIS> lstFourRubrique = lstEltDevis.Where(t => t.FK_IDRUBRIQUEDEVIS == item.PK_ID).ToList();
                    if (lstFourRubrique != null && lstFourRubrique.Count != 0)
                    {
                        int CoperTrv = SessionObject.LstDesCopers.FirstOrDefault(t => t.CODE == SessionObject.Enumere.CoperTRV).PK_ID;
                        lstFourRubrique.ForEach(t => t.FK_IDCOPER = CoperTrv);
                        if (item.PK_ID == 1 && laDetailDemande.Branchement.CODEBRT == "0001")
                        {
                            decimal? MontantLigne = 0;

                            ObjELEMENTDEVIS leIncidence = lstEltDevis.FirstOrDefault(t => t.ISGENERE == true);
                            leIncidence.FK_IDRUBRIQUEDEVIS = item.PK_ID;
                            leIncidence.QUANTITE = 1;
                            leIncidence.FK_IDCOPER = CoperTrv;
                            leIncidence.MONTANTTAXE = 0;
                            leIncidence.MONTANTHT = 0;
                            leIncidence.MONTANTTTC = leIncidence.QUANTITE * (leIncidence.COUTUNITAIRE_FOURNITURE + leIncidence.COUTUNITAIRE_POSE) * (-1);
                            if (lstFourRubrique.FirstOrDefault(t => t.ISGENERE) == null)
                                lstFourRubrique.Add(leIncidence);
                            MontantLigne = lstFourRubrique.Sum(t => t.MONTANTTTC);
                            if (MontantLigne < 0)
                                MiseAZereLigne = true;

                        }
                        decimal? MontantTotRubrique = lstFourRubrique.Sum(t => t.MONTANTTTC);
                        decimal? MontantTotRubriqueHt = lstFourRubrique.Sum(t => t.MONTANTHT);
                        decimal? MontantTotRubriqueTaxe = lstFourRubrique.Sum(t => t.MONTANTTAXE);
                        if (MiseAZereLigne == true)
                        { MontantTotRubrique = 0; MontantTotRubriqueHt = 0; MontantTotRubriqueTaxe = 0; }
                        ObjELEMENTDEVIS leResultatBranchanchement = new ObjELEMENTDEVIS();
                        leResultatBranchanchement.DESIGNATION = "Sous Total " + item.LIBELLE;
                        leResultatBranchanchement.IsCOLORIE = true;
                        leResultatBranchanchement.FK_IDRUBRIQUEDEVIS = item.PK_ID;
                        leResultatBranchanchement.ISDEFAULT = true;
                        leResultatBranchanchement.MONTANTHT = MontantTotRubriqueHt;
                        leResultatBranchanchement.MONTANTTAXE = MontantTotRubriqueTaxe;
                        leResultatBranchanchement.MONTANTTTC = MontantTotRubrique;

                        lstFourgenerale.AddRange(lstFourRubrique);
                        lstFourgenerale.Add(leSeparateur);
                        lstFourgenerale.Add(leResultatBranchanchement);
                        lstFourgenerale.Add(new ObjELEMENTDEVIS()
                        {
                            LIBELLE = "    "
                        });
                    }

                }
                if (lstFourgenerale.Count != 0)
                {
                    decimal? MontantTotRubrique = lstFourgenerale.Where(t => t.QUANTITE != null && t.QUANTITE != 0).Sum(t => t.MONTANTTTC);
                    decimal? MontantTotRubriqueHt = lstFourgenerale.Where(t => t.QUANTITE != null && t.QUANTITE != 0).Sum(t => t.MONTANTHT);
                    decimal? MontantTotRubriqueTaxe = lstFourgenerale.Where(t => t.QUANTITE != null && t.QUANTITE != 0).Sum(t => t.MONTANTTAXE);
                    if (MontantTotRubrique < 0)
                    { MontantTotRubrique = 0; MontantTotRubriqueHt = 0; MontantTotRubriqueTaxe = 0; }


                    ObjELEMENTDEVIS leSurveillance = new ObjELEMENTDEVIS();
                    leSurveillance.DESIGNATION = "Etude et surveillance ";
                    leSurveillance.ISFORTRENCH = true;
                    leSurveillance.QUANTITE = 1;
                    leSurveillance.MONTANTHT = MontantTotRubriqueHt * (decimal)(0.10); ;
                    leSurveillance.MONTANTTAXE = MontantTotRubriqueTaxe * (decimal)(0.10); ;
                    leSurveillance.MONTANTTTC = MontantTotRubrique * (decimal)(0.10);
                    leSurveillance.FK_IDCOPER = SessionObject.LstDesCopers.FirstOrDefault(t => t.CODE == SessionObject.Enumere.CoperTRV).PK_ID;
                    leSurveillance.FK_IDTAXE = SessionObject.LstDesTaxe.FirstOrDefault(t => t.CODE == SessionObject.Enumere.CodeSansTaxe).PK_ID;

                    lstFourgenerale.Add(leSurveillance);


                    ObjELEMENTDEVIS leResultatGeneral = new ObjELEMENTDEVIS();
                    leResultatGeneral.DESIGNATION = "TOTAL FACTURE TRAVAUX ";
                    //leResultatGeneral.IsCOLORIE = true;
                    leResultatGeneral.ISDEFAULT = true;
                    leResultatGeneral.MONTANTHT = MontantTotRubrique;
                    leResultatGeneral.MONTANTTAXE = MontantTotRubriqueHt;
                    leResultatGeneral.MONTANTTTC = MontantTotRubriqueTaxe;
                    lstFourgenerale.Add(leSeparateur);
                    lstFourgenerale.Add(leResultatGeneral);
                }
                ObjELEMENTDEVIS leResultatGeneralaVANCE = new ObjELEMENTDEVIS();
                leResultatGeneralaVANCE.DESIGNATION = "FACTURE AVANCE SUR CONSOMMATION ";
                //leResultatGeneralaVANCE.IsCOLORIE = true;
                leResultatGeneralaVANCE.ISDEFAULT = true;
                leResultatGeneralaVANCE.QUANTITE = lstEltDevis.FirstOrDefault (t => t.CODECOPER == SessionObject.Enumere.CoperCAU).QUANTITE ;
                leResultatGeneralaVANCE.MONTANTHT = lstEltDevis.Where(t => t.CODECOPER == SessionObject.Enumere.CoperCAU).Sum(y => y.MONTANTHT);
                leResultatGeneralaVANCE.MONTANTTAXE = lstEltDevis.Where(t => t.CODECOPER == SessionObject.Enumere.CoperCAU).Sum(y => y.MONTANTTAXE);
                leResultatGeneralaVANCE.MONTANTTTC = lstEltDevis.Where(t => t.CODECOPER == SessionObject.Enumere.CoperCAU).Sum(y => y.MONTANTTTC);
                leResultatGeneralaVANCE.FK_IDCOPER = SessionObject.LstDesCopers.FirstOrDefault(t => t.CODE == SessionObject.Enumere.CoperCAU).PK_ID;
                leResultatGeneralaVANCE.FK_IDTAXE = SessionObject.LstDesTaxe.FirstOrDefault(t => t.CODE == SessionObject.Enumere.CodeSansTaxe).PK_ID;

                lstFourgenerale.Add(leSeparateur);
                lstFourgenerale.Add(leResultatGeneralaVANCE);
                this.dataGridForniture.ItemsSource = null;
                this.dataGridForniture.ItemsSource = lstFourgenerale;

                this.Txt_TotalTtc.Text = ((List<ObjELEMENTDEVIS>)dataGridForniture.ItemsSource).ToList().Where(t => t.QUANTITE != null && t.QUANTITE != 0).Sum(y => y.MONTANTTTC).Value.ToString(SessionObject.FormatMontant);
                this.Txt_TotalTva.Text = ((List<ObjELEMENTDEVIS>)dataGridForniture.ItemsSource).ToList().Where(t => t.QUANTITE != null && t.QUANTITE != 0).Sum(y => y.MONTANTTAXE).Value.ToString(SessionObject.FormatMontant);
                this.Txt_TotalHt.Text = ((List<ObjELEMENTDEVIS>)dataGridForniture.ItemsSource).ToList().Where(t => t.QUANTITE != null && t.QUANTITE != 0).Sum(y => y.MONTANTHT).Value.ToString(SessionObject.FormatMontant);
            }
            catch (Exception ex)
            {
                Message.Show("Erreur au chargement des couts", "");
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

        private void Btn_Rejeter_Click(object sender, RoutedEventArgs e)
        {
            this.Btn_Rejeter.IsEnabled = false;
            this.Btn_Transmettre.IsEnabled = false;
            Shared.ClasseMEthodeGenerique.RejeterDemande(laDetailDemande);
        }
        private void AfficherOuMasquer(TabItem pTabItem, bool pValue)
        {
            try
            {
                pTabItem.Visibility = pValue ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void RenseignerInformationsRejetDemande(CsDemande lademande)
        {
            try
            {
                //if (lademande != null && (lademande.LstCommentaire != null && lademande.LstCommentaire.Count != 0))
                if (lademande != null && (lademande.AnnotationDemande != null && lademande.AnnotationDemande.Count != 0))
                {
                    //List<CsCommentaireRejet> lstRejets = new List<CsCommentaireRejet>();
                    List<CsAnnotation> lstRejets = new List<CsAnnotation>();
                    lstRejets.AddRange(lademande.AnnotationDemande);
                    dtg_RejetDemande.ItemsSource = lstRejets;
                    AfficherOuMasquer(tabItemRejet, true);

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void Chk_FactureDiff_Checked(object sender, RoutedEventArgs e)
        {
            if (laDetailDemande.LaDemande.PRODUIT == SessionObject.Enumere.ElectriciteMT)
            {
                if (laDetailDemande.LaDemande != null)
                    ChargerCoutDemande();
            }
        }

        private void Chk_FactureDiff_Unchecked(object sender, RoutedEventArgs e)
        {
            if (laDetailDemande.LaDemande.PRODUIT == SessionObject.Enumere.ElectriciteMT)
            {
                if (laDetailDemande.LaDemande != null)
                    ChargerCoutDemande();
            }
        }

        private void hyperlinkButtonPropScannee___Click(object sender, RoutedEventArgs e)
        {
            MemoryStream memoryStream = new MemoryStream(((HyperlinkButton)sender).Tag as byte[]);
            var ucImageScanne = new UcImageScanne(memoryStream, SessionObject.ExecMode.Modification);
            ucImageScanne.Show();
        }

        private void txt_MaticuleAgent_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(this.txt_MaticuleAgent.Text) && this.txt_MaticuleAgent.Text.Length == SessionObject.Enumere.TailleMatricule)
            {
                try
                {
                    Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient client = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                    client.VerifierMatriculeAgentAsync(this.txt_MaticuleAgent.Text);
                    client.VerifierMatriculeAgentCompleted += (ssender, args) =>
                    {
                        LayoutRoot.Cursor = Cursors.Arrow;

                        if (args.Cancelled || args.Error != null)
                        {
                            string error = args.Error.Message;
                            Message.ShowError(error, Silverlight.Resources.Devis.Languages.txtDevis);
                            return;
                        }
                        if (args.Result != null)
                        {
                            string abon = ((CsAbon)args.Result).CENTRE + "-" + ((CsAbon)args.Result).CLIENT + "-" + ((CsAbon)args.Result).ORDRE;

                            Message.ShowInformation("Ce matricule possède déjà un abonnement actif : " + abon, Silverlight.Resources.Devis.Languages.txtDevis);
                            this.txt_MaticuleAgent.Text = string.Empty;
                            return;
                        }
                    };
                }
                catch (Exception ex)
                {

                    throw ex;
                }
            }

        }
    }
}

