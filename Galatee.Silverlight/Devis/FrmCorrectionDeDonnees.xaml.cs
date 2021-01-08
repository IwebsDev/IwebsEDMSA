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
using System.Collections.ObjectModel;
using System.IO;
using System.Threading;
using Galatee.Silverlight.Resources.Devis;
using Galatee.Silverlight.Library;
using Galatee.Silverlight.Shared;
using System.Text.RegularExpressions;

namespace Galatee.Silverlight.Accueil
{
    public partial class FrmCorrectionDeDonnees : ChildWindow
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
        private DataGrid _dataGrid = null;
        private List<CsUsage> lstusage = new List<CsUsage>();
        bool isDocumentSelectionnee = false;
        string CodeProduit = string.Empty;
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


        CsAbon leAbonInit = new CsAbon();
        CsAg leAgInit = new CsAg();
        CsBrt leBrtInit = new CsBrt();
        CsCanalisation leCanalInit = new CsCanalisation();
        CsClient leClientInit = new CsClient();

        public FrmCorrectionDeDonnees()
        {
            InitializeComponent();
            txt_FinPeriodeExo.MaxLength = 7;
            txt_DebutPeriodeExo.MaxLength = 7;
            this.Tdem = SessionObject.Enumere.CorrectionDeDonnes;
            this.txt_tdem.Text = SessionObject.LstTypeDemande.FirstOrDefault(y => y.CODE == this.Tdem).LIBELLE;

        }
        public FrmCorrectionDeDonnees(string Tdem, string IsInit)
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

                txt_Reglage.Visibility = Visibility.Collapsed;
                Btn_Reglage.Visibility = Visibility.Collapsed;

                this.tabItemPersoneAdministration.Visibility = System.Windows.Visibility.Collapsed;
                this.tabItemPersoneMoral.Visibility = System.Windows.Visibility.Collapsed;


                txt_MaticuleAgent.Visibility = System.Windows.Visibility.Collapsed;
                lbl_MatriculeAgent.Visibility = System.Windows.Visibility.Collapsed;
                tbControleClient.IsEnabled = false;
                this.dtp_RendezVousPrev.SelectedDate = System.DateTime.Today.AddDays(15);
                ChargerDonneeDuSite();
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
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.txtDevis);
            }
        }

        public FrmCorrectionDeDonnees(int IdDemande)
        {
            try
            {
                InitializeComponent();
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

                txt_MaticuleAgent.Visibility = System.Windows.Visibility.Collapsed;
                lbl_MatriculeAgent.Visibility = System.Windows.Visibility.Collapsed;
                tbControleClient.IsEnabled = false;

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
                ChargeDetailDEvis(IdDemande);
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.txtDevis);
            }
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


        List<Galatee.Silverlight.ServiceAccueil.CsCentre> LstCentrePerimetre = new List<ServiceAccueil.CsCentre>();
        List<Galatee.Silverlight.ServiceAccueil.CsSite> lstSite = new List<ServiceAccueil.CsSite>();
        private void ChargerDonneeDuSite()
        {
            try
            {
                List<int> lstCentreSelect = new List<int>();

                if (SessionObject.LstCentre != null && SessionObject.LstCentre.Count > 0)
                {
                    LstCentrePerimetre = Shared.ClasseMEthodeGenerique.RetourCentreByPerimetre(SessionObject.LstCentre.Where(p => p.CODE != SessionObject.Enumere.Generale).ToList(), UserConnecte.listeProfilUser);
                    lstSite = Shared.ClasseMEthodeGenerique.RetourneSiteByCentre(LstCentrePerimetre);
                    return;
                }
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.ListeDesDonneesDesSiteCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    SessionObject.LstCentre = args.Result;
                    LstCentrePerimetre = Shared.ClasseMEthodeGenerique.RetourCentreByPerimetre(SessionObject.LstCentre.Where(p => p.CODE != SessionObject.Enumere.Generale).ToList(), UserConnecte.listeProfilUser);
                    lstSite = Shared.ClasseMEthodeGenerique.RetourneSiteByCentre(LstCentrePerimetre);
                    return;
                };
                service.ListeDesDonneesDesSiteAsync(false);
                service.CloseAsync();
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

        private void ChargerTarifParCategorieMt(CsDemande laDemande)
        {
            try
            {
                if (SessionObject.LstTarifCategorie.Count != 0)
                {
                    List<CsTarif> LstTarifCategorie = SessionObject.LstTarifCategorie.Where(p => p.PRODUIT == laDetailDemande.Abonne.PRODUIT && p.CATEGORIE == laDemande.LeClient.CATEGORIE).ToList();
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
                        List<CsTarif> LstTarifCategorie = SessionObject.LstTarifCategorie.Where(p => p.PRODUIT == laDetailDemande.Abonne.PRODUIT && p.CATEGORIE == laDemande.LeClient.CATEGORIE).ToList();
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
            client.GetDemandeByNumIdDemandeAsync(IdDemandeDevis);
            client.GetDemandeByNumIdDemandeCompleted += (ssender, args) =>
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
                    RemplirCommuneParCentre(laDetailDemande.LaDemande.FK_IDCENTRE);
                    Tdem = laDetailDemande.LaDemande.TYPEDEMANDE;
                    this.txt_tdem.Text = !string.IsNullOrEmpty(laDetailDemande.LaDemande.LIBELLETYPEDEMANDE) ? laDetailDemande.LaDemande.LIBELLETYPEDEMANDE : string.Empty;
                    this.txt_NumDem.Text = !string.IsNullOrEmpty(laDetailDemande.LaDemande.NUMDEM) ? laDetailDemande.LaDemande.NUMDEM : string.Empty;
                    laDemandeSelect = laDetailDemande.LaDemande;
                    RenseignerDemande(laDetailDemande);
                    RenseignerAbon(laDetailDemande);
                    RenseignerAG(laDetailDemande);
                    RenseignerClient(laDetailDemande);
                    RenseignerDocument(laDetailDemande);
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
                        throw new Exception("Séléctionnez la quartier ");
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
        private void ValidationDevis(CsDemande pDemandeDevis, bool IsTransmetre)
        {
            try
            {

              
                #region Demande
                if (pDemandeDevis.LaDemande == null) pDemandeDevis.LaDemande = new CsDemandeBase();
                pDemandeDevis.LaDemande.FK_IDCENTRE = pDemandeDevis.Ag.FK_IDCENTRE;
                pDemandeDevis.LaDemande.CENTRE = pDemandeDevis.Ag.CENTRE;
                pDemandeDevis.LaDemande.FK_IDPRODUIT = pDemandeDevis.Abonne.FK_IDPRODUIT ;
                pDemandeDevis.LaDemande.PRODUIT = pDemandeDevis.Abonne.PRODUIT ;
                //pDemandeDevis.LaDemande.FK_IDTYPEDEMANDE = typeDevis.PK_ID;
                //pDemandeDevis.LaDemande.TYPEDEMANDE = typeDevis.CODE;
                #endregion

                #region Abon
                if (this.Txt_DateAbonnement.Text == null)
                {
                    Message.Show("Saisir la date d'abonnement", "ValidationDevis");
                    return;
                }

                RemplirCommuneParCentre(pDemandeDevis.LaDemande.FK_IDCENTRE);

                pDemandeDevis.Abonne.NUMDEM = string.IsNullOrEmpty(pDemandeDevis.LaDemande.NUMDEM) ? string.Empty : pDemandeDevis.LaDemande.NUMDEM;
                pDemandeDevis.Abonne.PUISSANCE = string.IsNullOrEmpty(this.Txt_CodePussanceSoucrite.Text) ? 0 : Convert.ToDecimal(this.Txt_CodePussanceSoucrite.Text);
                pDemandeDevis.Abonne.PUISSANCEUTILISEE = string.IsNullOrEmpty(this.Txt_CodePuissanceUtilise.Text) ? 0 : Convert.ToDecimal(this.Txt_CodePuissanceUtilise.Text);
                pDemandeDevis.Abonne.RISTOURNE = string.IsNullOrEmpty(this.Txt_CodeRistoune.Text) ? 0 : Convert.ToDecimal(this.Txt_CodeRistoune.Text);

                pDemandeDevis.Abonne.FORFAIT = string.IsNullOrEmpty(this.Txt_CodeForfait.Text) ? string.Empty : this.Txt_CodeForfait.Text;
                pDemandeDevis.Abonne.TYPETARIF = string.IsNullOrEmpty(this.Txt_CodeTarif.Text) ? string.Empty : this.Txt_CodeTarif.Text;
                pDemandeDevis.Abonne.PERFAC = string.IsNullOrEmpty(this.Txt_CodeFrequence.Text) ? string.Empty : this.Txt_CodeFrequence.Text;
                pDemandeDevis.Abonne.MOISREL = string.IsNullOrEmpty(this.Txt_CodeMoisIndex.Text) ? string.Empty : this.Txt_CodeMoisIndex.Text;
                pDemandeDevis.Abonne.MOISFAC = string.IsNullOrEmpty(this.Txt_CodeMoisFacturation.Text) ? string.Empty : this.Txt_CodeMoisFacturation.Text;

                if (pDemandeDevis.LaDemande.PRODUIT == SessionObject.Enumere.ElectriciteMT)
                {
                    pDemandeDevis.Abonne.FK_IDTYPECOMPTAGE = laDetailDemande.LaDemande.FK_IDTYPECOMPTAGE;
                    pDemandeDevis.Abonne.TYPECOMPTAGE = laDetailDemande.LaDemande.TYPECOMPTAGE;
                }
                else
                {
                    pDemandeDevis.LaDemande.FK_IDREGLAGECOMPTEUR = (this.txt_Reglage.Tag == null) ? null : (int?)this.txt_Reglage.Tag;
                    pDemandeDevis.LaDemande.REGLAGECOMPTEUR = this.Btn_Reglage.Tag == null ? string.Empty : this.Btn_Reglage.Tag.ToString();
                }
                pDemandeDevis.Abonne.FK_IDCENTRE = laDetailDemande.LeClient.PK_ID;
                pDemandeDevis.Abonne.FK_IDCENTRE = laDetailDemande.LaDemande.FK_IDCENTRE;
                pDemandeDevis.Abonne.FK_IDPRODUIT = laDetailDemande.LaDemande.FK_IDPRODUIT.Value;
                pDemandeDevis.Abonne.FK_IDFORFAIT = this.Txt_CodeForfait.Tag == null ? laDetailDemande.Abonne.FK_IDFORFAIT : (int)this.Txt_CodeForfait.Tag;
                pDemandeDevis.Abonne.FK_IDMOISFAC = this.Txt_CodeMoisFacturation.Tag == null ? laDetailDemande.Abonne.FK_IDMOISFAC : (int)this.Txt_CodeMoisFacturation.Tag;
                pDemandeDevis.Abonne.FK_IDMOISREL = this.Txt_CodeMoisIndex.Tag == null ? laDetailDemande.Abonne.FK_IDMOISREL : (int)this.Txt_CodeMoisIndex.Tag;
                pDemandeDevis.Abonne.FK_IDTYPETARIF = this.Txt_CodeTarif.Tag == null ? laDetailDemande.Abonne.FK_IDTYPETARIF : (int)this.Txt_CodeTarif.Tag;
                pDemandeDevis.Abonne.FK_IDPERIODICITEFACTURE = this.Txt_CodeFrequence.Tag == null ? laDetailDemande.Abonne.FK_IDPERIODICITEFACTURE : (int)this.Txt_CodeFrequence.Tag;
                pDemandeDevis.Abonne.FK_IDPERIODICITERELEVE = this.Txt_CodeFrequence.Tag == null ? laDetailDemande.Abonne.FK_IDPERIODICITEFACTURE : (int)this.Txt_CodeFrequence.Tag;
                pDemandeDevis.Abonne.DABONNEMENT = Convert.ToDateTime(this.Txt_DateAbonnement.Text);
                pDemandeDevis.Abonne.ESTEXONERETVA = Chk_IsExonneration.IsChecked == true ? true : false;
                pDemandeDevis.Abonne.DEBUTEXONERATIONTVA = Shared.ClasseMEthodeGenerique.FormatPeriodeAAAAMM(this.txt_DebutPeriodeExo.Text);
                pDemandeDevis.Abonne.FINEXONERATIONTVA = Shared.ClasseMEthodeGenerique.FormatPeriodeAAAAMM(this.txt_FinPeriodeExo.Text);

                if (Shared.ClasseMEthodeGenerique.CompareObjet<CsAbon>(pDemandeDevis.Abonne, leAbonInit))
                    pDemandeDevis.Abonne.ISMODIFIER = true;

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

                if (Cbo_Rue.SelectedItem != null)
                {
                    var rue = Cbo_Rue.SelectedItem as CsRues;
                    if (rue != null)
                    {
                        laDetailDemande.Ag.FK_IDRUE = rue.PK_ID;
                        laDetailDemande.Ag.RUE = rue.CODE;
                    }
                }
                if (!string.IsNullOrEmpty(this.txt_NumRue.Text))
                {
                    pDemandeDevis.Ag.FK_IDRUE = (int)Cbo_Rue.Tag;
                    laDetailDemande.Ag.RUE = this.txt_NumRue.Text;
                }
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
                laDetailDemande.Ag.TELEPHONE = string.IsNullOrEmpty(this.txt_Telephone.Text) ? null : this.txt_Telephone.Text;
                laDetailDemande.Ag.ETAGE = string.IsNullOrEmpty(this.Txt_Etage.Text) ? null : this.Txt_Etage.Text;
                laDetailDemande.Ag.PORTE = string.IsNullOrEmpty(this.Txt_Porte.Text) ? null : this.Txt_Porte.Text;
                laDetailDemande.Ag.CENTRE = string.IsNullOrEmpty(laDetailDemande.LaDemande.CENTRE) ? null : laDetailDemande.LaDemande.CENTRE;
                laDetailDemande.Ag.CLIENT = string.IsNullOrEmpty(laDetailDemande.LaDemande.CLIENT) ? null : laDetailDemande.LaDemande.CLIENT;
                laDetailDemande.Ag.NUMDEM = string.IsNullOrEmpty(this.txt_NumDem.Text) ? null : this.txt_NumDem.Text;
                laDetailDemande.Ag.FK_IDCENTRE = laDetailDemande.LaDemande.FK_IDCENTRE;
       

                if (Shared.ClasseMEthodeGenerique.CompareObjet<CsAg>(pDemandeDevis.Ag , leAgInit))
                    pDemandeDevis.Ag.ISMODIFIER = true;

                laDetailDemande.Ag.DATECREATION = DateTime.Now;
                laDetailDemande.Ag.USERCREATION = UserConnecte.matricule;

                #endregion
                #region Client

                if (laDetailDemande.LaDemande.TYPEDEMANDE != SessionObject.Enumere.DimunitionPuissance &&
                  laDetailDemande.LaDemande.TYPEDEMANDE != SessionObject.Enumere.AugmentationPuissance)
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

                    #endregion

                    laDetailDemande.LeClient.TELEPHONEFIXE = string.IsNullOrEmpty(this.txt_Telephone_Fixe.Text) ? null : this.txt_Telephone_Fixe.Text;
                    laDetailDemande.LeClient.FAX = string.IsNullOrEmpty(this.Txt_NumFax.Text) ? null : this.Txt_NumFax.Text;
                    laDetailDemande.LeClient.BOITEPOSTAL = string.IsNullOrEmpty(this.Txt_BoitePostale.Text) ? null : this.Txt_BoitePostale.Text;
                    laDetailDemande.LeClient.TELEPHONE = string.IsNullOrEmpty(this.txt_Telephone.Text) ? null : this.txt_Telephone.Text;
                    laDetailDemande.LeClient.NUMPROPRIETE = string.IsNullOrEmpty(this.txtPropriete.Text) ? null : this.txtPropriete.Text;
                    laDetailDemande.LeClient.CENTRE = string.IsNullOrEmpty(laDetailDemande.LaDemande.CENTRE) ? null : laDetailDemande.LaDemande.CENTRE;
                    laDetailDemande.LeClient.REFCLIENT = string.IsNullOrEmpty(laDetailDemande.LaDemande.CLIENT) ? null : laDetailDemande.LaDemande.CLIENT;
                    laDetailDemande.LeClient.FK_IDCENTRE = laDetailDemande.LaDemande.FK_IDCENTRE;

                    laDetailDemande.LeClient.ISFACTUREEMAIL = chk_Email.IsChecked.Value;
                    //laDetailDemande.LeClient.ISFACTURESMS = chk_SMS.IsChecked.Value;
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

                    GetSocieteProprietaire(laDetailDemande);

                    #endregion
                    laDetailDemande.LeClient.CODEIDENTIFICATIONNATIONALE = string.IsNullOrEmpty(this.Txt_Numeronina.Text) ? null : this.Txt_Numeronina.Text.Trim();
                    laDetailDemande.LeClient.FK_IDRELANCE = 1;
                    laDetailDemande.LeClient.CODERELANCE = "0";
                    laDetailDemande.LeClient.MODEPAIEMENT = "0";
                    laDetailDemande.LeClient.FK_IDMODEPAIEMENT = 1;

                    if (Shared.ClasseMEthodeGenerique.CompareObjet<CsClient >(pDemandeDevis.LeClient , leClientInit ))
                        pDemandeDevis.LeClient.ISMODIFIER = true;

                    laDetailDemande.LeClient.DATECREATION = DateTime.Now;
                    laDetailDemande.LeClient.USERCREATION = UserConnecte.matricule;
                    laDetailDemande.LeClient.NUMDEM = string.IsNullOrEmpty(this.txt_NumDem.Text) ? null : this.txt_NumDem.Text;
                }

                #endregion
                #region Cannalisation
                CsCanalisation leCannalisation = new CsCanalisation();

                leCannalisation = pDemandeDevis.LstCanalistion.FirstOrDefault();
                leCannalisation.ANNEEFAB = string.IsNullOrEmpty(this.Txt_AnneeFab.Text) ? string.Empty : this.Txt_AnneeFab.Text;
                leCannalisation.CADRAN = string.IsNullOrEmpty(this.Txt_CodeCadran.Text) ? Convert.ToByte(6) : Convert.ToByte(this.Txt_CodeCadran.Text);
                leCannalisation.NUMERO = string.IsNullOrEmpty(this.Txt_NumCompteur.Text) ? string.Empty : this.Txt_NumCompteur.Text;
                leCannalisation.MARQUE = string.IsNullOrEmpty(this.Txt_CodeMarque.Text) ? string.Empty : this.Txt_CodeMarque.Text;
                //leCannalisation.TYPECOMPTEUR = string.IsNullOrEmpty(this.Txt_CodeTypeCompteur.Text) ? string.Empty : this.Txt_CodeTypeCompteur.Text;

                leCannalisation.FK_IDCALIBRE = null;
                if (this.Txt_LibelleDiametre.Tag != null)
                    leCannalisation.FK_IDCALIBRE = (int)this.Txt_LibelleDiametre.Tag;

                leCannalisation.FK_IDMARQUECOMPTEUR = (int)this.Txt_CodeMarque.Tag;
       

                if (Shared.ClasseMEthodeGenerique.CompareObjet<CsCanalisation>(leCannalisation, leCanalInit))
                    leCannalisation.ISMODIFIER = true;

                leCannalisation.USERCREATION = UserConnecte.matricule;
                leCannalisation.USERMODIFICATION = UserConnecte.matricule;
                leCannalisation.DATECREATION = System.DateTime.Now;
                leCannalisation.DATEMODIFICATION = System.DateTime.Now;

                pDemandeDevis.LstCanalistion = new List<CsCanalisation>();
                pDemandeDevis.LstCanalistion.Add(leCannalisation);

                #endregion

                #region Branchement
                pDemandeDevis.Branchement.LONGBRT = string.IsNullOrEmpty(this.Txt_LongueurBrt.Text) ? 0 : Convert.ToDecimal(this.Txt_LongueurBrt.Text);
                pDemandeDevis.Branchement.ADRESSERESEAU = string.IsNullOrEmpty(this.Txt_AdresseElectrique.Text) ? null : this.Txt_AdresseElectrique.Text;
                pDemandeDevis.Branchement.LONGITUDE = string.IsNullOrEmpty(this.Txt_Longitude.Text) ? null : this.Txt_Longitude.Text;
                pDemandeDevis.Branchement.LATITUDE = string.IsNullOrEmpty(this.Txt_Latitude.Text) ? null : this.Txt_Latitude.Text;
                pDemandeDevis.Branchement.DRAC = null;
                if (!string.IsNullOrEmpty(this.Txt_DateRacordement .Text)) 
                   pDemandeDevis.Branchement.DRAC = Convert.ToDateTime(this.Txt_DateRacordement .Text);

                pDemandeDevis.Branchement.FK_IDTYPEBRANCHEMENT = this.Txt_TypeBrancehment.Tag == null ? pDemandeDevis.Branchement.FK_IDTYPEBRANCHEMENT : (int)this.Txt_TypeBrancehment.Tag;
                pDemandeDevis.Branchement.FK_IDPOSTESOURCE = this.Txt_PosteSource.Tag == null ? pDemandeDevis.Branchement.FK_IDPOSTESOURCE : (int)this.Txt_PosteSource.Tag;
                pDemandeDevis.Branchement.FK_IDPOSTETRANSFORMATION = this.Txt_PosteTransformateur.Tag == null ? pDemandeDevis.Branchement.FK_IDPOSTETRANSFORMATION : (int)this.Txt_PosteTransformateur.Tag;
                pDemandeDevis.Branchement.FK_IDDEPARTBT = this.Txt_DepartBt.Tag == null ? pDemandeDevis.Branchement.FK_IDDEPARTBT : (int)this.Txt_DepartBt.Tag;
                pDemandeDevis.Branchement.FK_IDQUARTIER = this.Txt_QuarteirPoste.Tag == null ? pDemandeDevis.Branchement.FK_IDQUARTIER : (int)this.Txt_QuarteirPoste.Tag;
                pDemandeDevis.Branchement.FK_IDDEPARTHTA = this.Txt_DepartHTA.Tag == null ? pDemandeDevis.Branchement.FK_IDDEPARTHTA : (int)this.Txt_DepartHTA.Tag;
                pDemandeDevis.Branchement.NEOUDFINAL = string.IsNullOrEmpty(this.Txt_NeoudFinal.Text) ? null : this.Txt_NeoudFinal.Text;

                if (Cbo_Puissance.SelectedItem != null )
                    pDemandeDevis.Branchement.PUISSANCEINSTALLEE = Convert.ToDecimal((Cbo_Puissance.SelectedItem as Galatee.Silverlight.ServiceAccueil.CsPuissance).VALEUR);

                if (Shared.ClasseMEthodeGenerique.CompareObjet<CsBrt>(pDemandeDevis.Branchement, leBrtInit))
                    leCannalisation.ISMODIFIER = true;

                pDemandeDevis.Branchement.USERCREATION = UserConnecte.matricule;
                pDemandeDevis.Branchement.USERMODIFICATION = UserConnecte.matricule;
                pDemandeDevis.Branchement.DATECREATION = System.DateTime.Now;
                pDemandeDevis.Branchement.DATEMODIFICATION = System.DateTime.Now;
                #endregion
                #region Doc Scanne
                if (laDetailDemande.ObjetScanne == null) laDetailDemande.ObjetScanne = new List<ObjDOCUMENTSCANNE>();
                laDetailDemande.ObjetScanne.AddRange(LstPiece);
                #endregion

                AcceuilServiceClient clientDevis = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                clientDevis.ValiderDemandeAsync(laDetailDemande);
                clientDevis.ValiderDemandeCompleted += (ss, b) =>
                {
                    this.Btn_Rejeter.IsEnabled = true;
                    this.Btn_Transmettre.IsEnabled = true;
                    if (b.Cancelled || b.Error != null)
                    {
                        string error = b.Error.Message;
                        Message.ShowError(error, Silverlight.Resources.Devis.Languages.txtDevis);
                        return;
                    }
                    if (IsTransmetre && b.Result == true)
                    {
                        if (laDetailDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.BranchementAbonnementEp)
                        {
                            AcceuilServiceClient clientDeviss = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                            clientDeviss.ClotureValiderDemandeCompleted += (sss, bd) =>
                            {
                                if (bd.Cancelled || bd.Error != null)
                                {
                                    string error = bd.Error.Message;
                                    Message.ShowError(error, Silverlight.Resources.Devis.Languages.txtDevis);
                                    return;
                                }
                                if (bd.Result == true)
                                {
                                    List<string> codes = new List<string>();
                                    codes.Add(laDetailDemande.InfoDemande.CODE);
                                    Galatee.Silverlight.Shared.ClasseMEthodeGenerique.TransmettreDemande(codes, true, this);
                                }
                                else
                                {
                                    Message.ShowError("Erreur a la cloture de la demande", Silverlight.Resources.Devis.Languages.txtDevis);
                                }
                            };
                            clientDeviss.ClotureValiderDemandeAsync(laDetailDemande);
                        }
                        else
                        {
                            List<string> codes = new List<string>();
                            codes.Add(laDetailDemande.InfoDemande.CODE);
                            Galatee.Silverlight.Shared.ClasseMEthodeGenerique.TransmettreDemande(codes, true, this);
                            //List<CsUtilisateur> leUser = new List<CsUtilisateur>();
                            //if (laDetailDemande.InfoDemande != null && laDetailDemande.InfoDemande.CODE != null)
                            //{
                            //    foreach (CsUtilisateur item in laDetailDemande.InfoDemande.UtilisateurEtapeSuivante)
                            //        leUser.Add(item);
                            //    Shared.ClasseMEthodeGenerique.NotifierMailDemande(leUser, "0001", laDetailDemande.LaDemande.NUMDEM, laDetailDemande.LaDemande.LIBELLETYPEDEMANDE);
                            //}
                        }
                    }
                    else
                        Message.ShowError("Erreur a la mise a jour de la demande", Silverlight.Resources.Devis.Languages.txtDevis);

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
                    LstForfait = SessionObject.LstForfait.Where(p => p.PRODUIT == laDemande.Abonne.PRODUIT).ToList();
                else
                {
                    AcceuilServiceClient service = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                    service.ChargerForfaitCompleted += (s, args) =>
                    {
                        if (args != null && args.Cancelled)
                            return;

                        SessionObject.LstForfait = args.Result;
                        LstForfait = SessionObject.LstForfait.Where(p => p.PRODUIT == laDemande.Abonne.PRODUIT).ToList();
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
        private void ChargerPuissanceEtTarif(int idProduit, int? idPuissance, int? idCategorie, int? idReglageCompteur )
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
                service.ChargerTypeTarifAsync(idProduit, idPuissance, idCategorie, idReglageCompteur);
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
                    RemplirTourneeExistante(laDemande.Abonne.FK_IDCENTRE);
                    Txt_Porte.Text = laDemande.Ag.PORTE != null ? laDemande.Ag.PORTE : string.Empty;
                    Txt_Etage.Text = laDemande.Ag.ETAGE != null ? laDemande.Ag.ETAGE : string.Empty;
                    TxtOrdreTournee.Text = laDemande.Ag.ORDTOUR != null ? laDemande.Ag.ORDTOUR : string.Empty;
                    #endregion
                }

            }
            catch (Exception ex)
            {
                Message.ShowInformation("Erreur a l'affichage des donnée", "Init");
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

                        Txt_NomClient_PersonePhysiq.Text = !string.IsNullOrEmpty(laDemande.LeClient.NOMABON) ? laDemande.LeClient.NOMABON : string.Empty;
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
                        //chk_SMS.IsChecked = laDemande.LeClient.ISFACTURESMS != null ? laDemande.LeClient.ISFACTURESMS : false;
                        chk_Email.IsChecked = laDemande.LeClient.ISFACTUREEMAIL != null ? laDemande.LeClient.ISFACTUREEMAIL : false;

                    }
                    #endregion
                }

            }
            catch (Exception ex)
            {
                Message.ShowInformation("Erreur a l'affichage des donnée", "Init");
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

                        if (laDetailDemande.Abonne.PUISSANCEUTILISEE != null)
                            this.Txt_CodePuissanceUtilise.Text = Convert.ToDecimal(laDetailDemande.Abonne.PUISSANCEUTILISEE.Value).ToString("N2");
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
                    }
                    else
                        this.Txt_DateAbonnement.Text = DateTime.Now.ToShortDateString();

                    if (laDetailDemande.Abonne.PRODUIT == SessionObject.Enumere.ElectriciteMT)
                    {
                        txt_Reglage.Visibility = Visibility.Collapsed;
                        Btn_Reglage.Visibility = Visibility.Collapsed;
                    }
                    else
                    {
                        int? idPuissance = null;
                        if (this.Txt_CodePussanceSoucrite.Tag != null)
                            idPuissance = (int)this.Txt_CodePussanceSoucrite.Tag;

                        int? idCategorie = null;
                        if (Cbo_Categorie.SelectedItem != null)
                            idCategorie = (Cbo_Categorie.SelectedItem as CsCategorieClient).PK_ID;

                        int idProduit  = laDetailDemande.Abonne.FK_IDPRODUIT;

                        int? idReglage = null;
                        if (this.txt_Reglage.Tag != null)
                            idReglage = (int)this.txt_Reglage.Tag;

                        if (idCategorie != null  && idReglage != null )
                        ChargerPuissanceEtTarif(idProduit, idPuissance, idCategorie, idReglage);

                        txt_Reglage.Visibility = Visibility.Visible ;
                        Btn_Reglage.Visibility = Visibility.Visible;
                    }
                    ChargerFrequence(laDetailDemande);
                    ChargerMois(laDetailDemande);
                    if (laDetailDemande.Abonne.PRODUIT != SessionObject.Enumere.ElectriciteMT)
                    {
                        int? idPuissance = null;
                        if (this.Txt_CodePussanceSoucrite.Tag != null)
                            idPuissance = (int)this.Txt_CodePussanceSoucrite.Tag;

                        int? idCategorie = null;
                        if (Cbo_Categorie.SelectedItem != null)
                            idCategorie = (Cbo_Categorie.SelectedItem as CsCategorieClient).PK_ID;

                        int idProduit  = laDetailDemande.Abonne.FK_IDPRODUIT;

                        int? idReglage = null;
                        if (this.txt_Reglage.Tag != null)
                            idReglage = (int)this.txt_Reglage.Tag;

                        if (idCategorie != null &&  idReglage != null)
                        ChargerPuissanceEtTarif(idProduit, idPuissance, idCategorie, idReglage);
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
                Message.ShowInformation("Erreur a l'affichage des donnée", "Init");
            }
        }
        private void RenseignerDemande(CsDemande laDemande)
        {
            try
            {
                if (laDemande != null)
                {
                    RemplirTourneeExistante(laDetailDemande.Ag.FK_IDCENTRE);
                }

            }
            catch (Exception ex)
            {
                Message.ShowInformation("Erreur a l'affichage des donnée", "Init");
            }
        }
        private void RenseignerDocument(CsDemande laDemande)
        {
            try
            {
                if (laDemande != null)
                {
                    if (laDemande.ObjetScanne != null && laDemande.ObjetScanne.Count != 0)
                    {
                        foreach (var item in laDemande.ObjetScanne)
                            LstPiece.Add(item);
                        dgListePiece.ItemsSource = this.LstPiece;
                    }
                }
            }
            catch (Exception ex)
            {
                Message.ShowInformation("Erreur a l'affichage des donnée", "Init");
            }
        }
        private void RenseignerCannalisation(CsDemande laDemande)
        {
            try
            {
                if (laDemande != null)
                {
                    if (laDetailDemande.Abonne.PRODUIT != SessionObject.Enumere.ElectriciteMT)
                    {
                        if (laDetailDemande.LstCanalistion != null)
                        {
                            CsReglageCompteur leReglage = _listeDesReglageCompteurExistant.FirstOrDefault(t => t.CODE == laDetailDemande.LstCanalistion.First().REGLAGECOMPTEUR);
                            if (leReglage != null)
                            {
                                this.txt_Reglage.Text = leReglage.LIBELLE;
                                this.txt_Reglage.Tag = leReglage.PK_ID;
                                this.Btn_Reglage.Tag = leReglage.CODE;
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Message.ShowInformation("Erreur a l'affichage des donnée", "Init");
            }
        }
        private void RenseignerBranchement(CsDemande laDemande)
        {

            if (laDemande.Branchement != null && laDemande.Branchement.PRODUIT != SessionObject.Enumere.ElectriciteMT)
            {
                this.Cbo_Puissance.Visibility = System.Windows.Visibility.Collapsed;
                label_Puissance.Visibility = System.Windows.Visibility.Collapsed;
            }
            int InitValue = 0;
            this.Txt_TypeBrancehment.Text = string.IsNullOrEmpty(laDemande.Branchement.CODETYPEBRANCHEMENT) ? string.Empty : laDemande.Branchement.CODETYPEBRANCHEMENT;
            this.Txt_LibelleTypeBRT.Text = string.IsNullOrEmpty(laDemande.Branchement.LIBELLETYPEBRANCHEMENT) ? string.Empty : laDemande.Branchement.LIBELLETYPEBRANCHEMENT;

            this.Txt_LongueurBrt.Text = string.IsNullOrEmpty(laDemande.Branchement.LONGBRT.ToString()) ? InitValue.ToString() : laDemande.Branchement.LONGBRT.ToString();
            this.Txt_DateRacordement.Text = string.IsNullOrEmpty(laDemande.Branchement.DRAC.ToString()) ? string.Empty : Convert.ToDateTime(laDemande.Branchement.DRAC).ToShortDateString();
            this.Txt_NbreTransformation.Text = string.IsNullOrEmpty(laDemande.Branchement.NOMBRETRANSFORMATEUR.ToString()) ? InitValue.ToString() : laDemande.Branchement.NOMBRETRANSFORMATEUR.ToString();

            this.Txt_Longitude.Text = string.IsNullOrEmpty(laDemande.Branchement.LONGITUDE) ? string.Empty : laDemande.Branchement.LONGITUDE;
            this.Txt_Latitude.Text = string.IsNullOrEmpty(laDemande.Branchement.LATITUDE) ? string.Empty : laDemande.Branchement.LATITUDE;

            this.Txt_AdresseElectrique.Text = string.IsNullOrEmpty(laDemande.Branchement.ADRESSERESEAU) ? string.Empty : laDemande.Branchement.ADRESSERESEAU;
            this.Txt_PosteSource.Text = string.IsNullOrEmpty(laDemande.Branchement.CODEPOSTESOURCE) ? string.Empty : laDemande.Branchement.CODEPOSTESOURCE;
            this.Txt_PosteTransformateur.Text = string.IsNullOrEmpty(laDemande.Branchement.CODETRANSFORMATEUR) ? string.Empty : laDemande.Branchement.CODETRANSFORMATEUR;
            this.Txt_DepartBt.Text = string.IsNullOrEmpty(laDemande.Branchement.CODEDEPARTBT) ? string.Empty : laDemande.Branchement.CODEDEPARTBT;
            this.Txt_DepartHTA.Text = string.IsNullOrEmpty(laDemande.Branchement.CODEDEPARTHTA) ? string.Empty : laDemande.Branchement.CODEDEPARTHTA;
            this.Txt_QuarteirPoste.Text = string.IsNullOrEmpty(laDemande.Branchement.CODEQUARTIER) ? string.Empty : laDemande.Branchement.CODEQUARTIER;
            this.Txt_NeoudFinal.Text = string.IsNullOrEmpty(laDemande.Branchement.NEOUDFINAL) ? string.Empty : laDemande.Branchement.NEOUDFINAL;

            if (laDemande.Branchement != null && laDemande.Branchement.PRODUIT == SessionObject.Enumere.ElectriciteMT && laDetailDemande.Branchement.PUISSANCEINSTALLEE != null && laDetailDemande.Branchement.PUISSANCEINSTALLEE != 0)
            {
                List<ServiceAccueil.CsPuissance> lesPuissance = SessionObject.LstPuissanceInstalle.Where(t => t.PRODUIT == SessionObject.Enumere.ElectriciteMT).ToList();
                Cbo_Puissance.SelectedItem = lesPuissance.FirstOrDefault(p => p.VALEUR == laDemande.Branchement.PUISSANCEINSTALLEE);
                Cbo_Puissance.Tag = lesPuissance.FirstOrDefault(p => p.PK_ID == laDemande.Branchement.PUISSANCEINSTALLEE);
            }
        }
        private void RenseignerCompteur(CsDemande laDemande)
        {

            if (laDetailDemande.Abonne.PRODUIT == SessionObject.Enumere.ElectriciteMT)
            {
                this.btn_DiametreCompteur.IsEnabled = false;
                this.Txt_LibelleDiametre.IsEnabled = false;
                laDetailDemande.LstCanalistion.First().NUMERO = laDetailDemande.LstCanalistion.First().NUMERO.Substring(5, (laDetailDemande.LstCanalistion.First().NUMERO.Length - 5));
            }
            CsCanalisation laCanal = laDetailDemande.LstCanalistion.First();

            this.Txt_ReferenceClient.Text = (string.IsNullOrEmpty(laCanal.CLIENT)) ? string.Empty : laCanal.CLIENT;

            this.Txt_NumCompteur.Text = (string.IsNullOrEmpty(laCanal.NUMERO)) ? string.Empty : laCanal.NUMERO;
            this.Txt_AnneeFab.Text = (string.IsNullOrEmpty(laCanal.ANNEEFAB)) ? string.Empty : laCanal.ANNEEFAB;

            if (laCanal.PRODUIT != SessionObject.Enumere.ElectriciteMT)
                this.Txt_LibelleDiametre.Text = (string.IsNullOrEmpty(laCanal.CODECALIBRECOMPTEUR)) ? string.Empty : laCanal.CODECALIBRECOMPTEUR;
            else
                this.Txt_LibelleDiametre.Text = (string.IsNullOrEmpty(laCanal.LIBELLETYPECOMPTAGE)) ? string.Empty : laCanal.LIBELLETYPECOMPTAGE;

            this.Txt_LibelleMarque.Text = (string.IsNullOrEmpty(laCanal.LIBELLEMARQUE)) ? string.Empty : laCanal.LIBELLEMARQUE;

            this.Txt_CodeMarque.Text = (SessionObject.LstMarque.FirstOrDefault(c => c.PK_ID == laCanal.FK_IDMARQUECOMPTEUR) == null) ? string.Empty : SessionObject.LstMarque.FirstOrDefault(c => c.PK_ID == laCanal.FK_IDMARQUECOMPTEUR).CODE;
            if (laCanal.CADRAN != null)
                this.Txt_CodeCadran.Text = laCanal.CADRAN.Value.ToString();
            this.Txt_LibelleDiametre.Tag = laCanal.FK_IDCALIBRE;
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
                dtp_finvalidationProprio.IsEnabled = pValue;
                txtNumeroPieceProprio.IsEnabled = pValue;
                dtp_DateNaissanceProprio.IsEnabled = pValue;
                Txt_PrenomProprio_PersonePhysiq.IsEnabled = pValue;
                Txt_NomProprio_PersonePhysiq.IsEnabled = pValue;
                txt_Telephone_Proprio.IsEnabled = pValue;
                Txt_Email_Proprio.IsEnabled = pValue;
                Txt_BoitePosta_Proprio.IsEnabled = pValue;
                Txt_Faxe_Proprio.IsEnabled = pValue;
                Cbo_Nationalite_Proprio.IsEnabled = pValue;
                Cbo_Type_Proprietaire.IsEnabled = pValue;
                Cbo_Categorie.IsEnabled = pValue;
                Cbo_Secteur.IsEnabled = pValue;
                Cbo_CodeConso.IsEnabled = pValue;
                Cbo_Regroupement.IsEnabled = pValue;
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
 
            }
            catch (Exception ex)
            {
                throw ex;
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
                        ChargerPuissanceEtTarif(idProduit, idPuissance, idCategorie, ctrs.leReglageSelect.PK_ID);
                    }
                }
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
                    if (laDetailDemande.Abonne.FK_IDPRODUIT != null)
                        idProduit = laDetailDemande.Abonne.FK_IDPRODUIT;


                    int? idReglage = null;
                    if (this.txt_Reglage.Tag != null)
                        idReglage = (int)this.txt_Reglage.Tag;

                    if (cat != null)
                    {
                        TxtCategorieClient.Text = cat.CODE ?? string.Empty;
                        this.TxtCategorieClient.Tag = cat.PK_ID;

                        if (idCategorie != null && idProduit != null && idReglage != null)
                        ChargerPuissanceEtTarif(idProduit, idPuissance, cat.PK_ID, idReglage);
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
            //if (!chk_SMS.IsChecked.Value)
            //{
            //    txt_Telephone.Text = string.Empty;
            //}
            //txt_Telephone.IsEnabled = chk_SMS.IsChecked.Value;
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
                this.isDocumentSelectionnee = true;
            }
            else
            {
                this.isDocumentSelectionnee = false;
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
            var messageBox = new MessageBoxControl.MessageBoxChildWindow("Attention", "Ête-vous sure de vouloire supprimer la ligne?", MessageBoxControl.MessageBoxButtons.OkCancel, MessageBoxControl.MessageBoxIcon.Information);
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
                    PropietaireWindows(System.Windows.Visibility.Visible);
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
            this.Btn_Rejeter.IsEnabled = false;
            this.Btn_Transmettre.IsEnabled = false;
            ValidationDevis(laDetailDemande, true);

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
                    ChargerPuissanceEtTarif(idProduit, _LaPuissanceSelect.PK_ID,idCategorie,idReglageCompteur );
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
        private void DetermineTarifMt(string puissance, string Categorie, string Produit)
        {
            if (SessionObject.LstTarifCategorie.Count != 0)
            {
                List<CsTarif> LstTarifCategorie = SessionObject.LstTarifCategorie.Where(p => p.PRODUIT == Produit && p.CATEGORIE == Categorie).ToList();
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
  
        private void Btn_Rejeter_Click(object sender, RoutedEventArgs e)
        {
            this.Btn_Rejeter.IsEnabled = false;
            this.Btn_Transmettre.IsEnabled = false;
            Shared.ClasseMEthodeGenerique.RejeterDemande(laDetailDemande);

            Thread.Sleep(5000);
            this.Btn_Rejeter.IsEnabled = true;
            this.Btn_Transmettre.IsEnabled = true;
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
        private void btn_RechercheClient_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Txt_ReferenceClient.Text.Length == SessionObject.Enumere.TailleClient)
                    ChargerClientFromReference(this.Txt_ReferenceClient.Text);
                else
                    Message.Show("La reference saisie n'est pas correcte", "Infomation");
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
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
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
                service.RetourneClientByReferenceAsync(ReferenceClient, LstCentrePerimetre.Select(i=>i.PK_ID).ToList());
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
                    AcceuilServiceClient service = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
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
                client.GeDetailByFromClientCompleted += (ssender, args) =>
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
                        leAbonInit  = Shared.ClasseMEthodeGenerique.RetourneCopyObjet<CsAbon>( args.Result.Abonne );
                        this.txtSite.Text = LstCentrePerimetre.First(y => y.PK_ID == laDetailDemande.Ag.FK_IDCENTRE).LIBELLESITE;
                        this.txtCentre.Text = LstCentrePerimetre.First(y => y.PK_ID == laDetailDemande.Ag.FK_IDCENTRE).LIBELLE ;
                        RemplirCommuneParCentre(laDetailDemande.Ag.FK_IDCENTRE);
                        this.txt_NumDem.Text = !string.IsNullOrEmpty(laDetailDemande.Ag.NUMDEM) ? laDetailDemande.Ag.NUMDEM : string.Empty;
                        //RenseignerDemande(laDetailDemande);
                        RenseignerAG(laDetailDemande);
                        RenseignerClient(laDetailDemande);
                        RenseignerAbon(laDetailDemande);
                        RenseignerCannalisation(laDetailDemande);
                        RenseignerBranchement(laDetailDemande);
                        RenseignerCompteur(laDetailDemande);
                        //RenseignerDocument(laDetailDemande);
                    }
                };
                client.GeDetailByFromClientAsync(leclient);
            }
            catch (Exception ex)
            {
                Message.ShowError("Erreur au chargement des donnéés", "Demande");
            }
        }
        #region AdresseElectrique
                private void btn_PosteSource_Click(object sender, RoutedEventArgs e)
        {
            if (SessionObject.LsDesPosteElectriquesSource.Count != 0)
            {
                List<object> _LstObj = Galatee.Silverlight.Shared.ClasseMEthodeGenerique.RetourneListeObjet(SessionObject.LsDesPosteElectriquesSource);
                Galatee.Silverlight.MainView.UcListeGenerique ctr = new Galatee.Silverlight.MainView.UcListeGenerique(_LstObj, "CODE", "LIBELLE", "Liste des poste sources");
                ctr.Closed += new EventHandler(galatee_OkClickedbtn_PosteSource);
                ctr.Show();
            }
        }

        void galatee_OkClickedbtn_PosteSource(object sender, EventArgs e)
        {
            Galatee.Silverlight.MainView.UcListeGenerique ctrs = sender as Galatee.Silverlight.MainView.UcListeGenerique;
            if (ctrs.isOkClick)
            {
                Galatee.Silverlight.ServiceAccueil.CsPosteSource _LePoste = (Galatee.Silverlight.ServiceAccueil.CsPosteSource)ctrs.MyObject;
                if (_LePoste != null)
                {
                    this.Txt_PosteSource.Text = _LePoste.CODE;
                    this.Txt_LibellePosteSource.Text = _LePoste.LIBELLE;
                    this.Txt_PosteSource.Tag = _LePoste.PK_ID;
                }
            }
        }

        private void btn_depart_Click(object sender, RoutedEventArgs e)
        {
            if (SessionObject.LsDesDepartHTA.Count != 0 && this.Txt_PosteSource.Tag != null)
            {
                List<object> _LstObj = Galatee.Silverlight.Shared.ClasseMEthodeGenerique.RetourneListeObjet(SessionObject.LsDesDepartHTA.Where(t => t.FK_IDPOSTESOURCE == (int)this.Txt_PosteSource.Tag).ToList());
                Galatee.Silverlight.MainView.UcListeGenerique ctr = new Galatee.Silverlight.MainView.UcListeGenerique(_LstObj, "CODE", "LIBELLE", "Liste des departs HTA");
                ctr.Closed += new EventHandler(galatee_OkClickedbtn_depart);
                ctr.Show();
            }
        }
        void galatee_OkClickedbtn_depart(object sender, EventArgs e)
        {
            Galatee.Silverlight.MainView.UcListeGenerique ctrs = sender as Galatee.Silverlight.MainView.UcListeGenerique;
            if (ctrs.isOkClick)
            {
                Galatee.Silverlight.ServiceAccueil.CsDepart _LeDepart = (Galatee.Silverlight.ServiceAccueil.CsDepart)ctrs.MyObject;
                if (_LeDepart != null)
                {
                    this.Txt_DepartHTA.Text = _LeDepart.CODE;
                    this.Txt_LibelleDepartHTA.Text = _LeDepart.LIBELLE;
                    this.Txt_DepartHTA.Tag = _LeDepart.PK_ID;
                }
            }
        }

        private void btn_PosteTransformateur_Click_1(object sender, RoutedEventArgs e)
        {
            if (SessionObject.LsDesPosteElectriquesTransformateur.Count != 0 && this.Txt_DepartHTA.Tag != null)
            {
                List<object> _LstObj = Galatee.Silverlight.Shared.ClasseMEthodeGenerique.RetourneListeObjet(SessionObject.LsDesPosteElectriquesTransformateur.Where(t => t.FK_IDDEPARTHTA == (int)this.Txt_DepartHTA.Tag).ToList());
                Galatee.Silverlight.MainView.UcListeGenerique ctr = new Galatee.Silverlight.MainView.UcListeGenerique(_LstObj, "CODE", "LIBELLE", "Liste des postes transformateur");
                ctr.Closed += new EventHandler(galatee_OkClickedbtn_PosteTransformateur);
                ctr.Show();
            }
        }
        void galatee_OkClickedbtn_PosteTransformateur(object sender, EventArgs e)
        {
            Galatee.Silverlight.MainView.UcListeGenerique ctrs = sender as Galatee.Silverlight.MainView.UcListeGenerique;
            if (ctrs.isOkClick)
            {
                Galatee.Silverlight.ServiceAccueil.CsPosteSource _LsPoste = (Galatee.Silverlight.ServiceAccueil.CsPosteSource)ctrs.MyObject;
                if (_LsPoste != null)
                {
                    this.Txt_PosteTransformateur.Text = _LsPoste.CODE;
                    this.Txt_LibellePosteTransformateur.Text = _LsPoste.LIBELLE;
                    this.Txt_PosteTransformateur.Tag = _LsPoste.PK_ID;

                }
            }
        }

        private void btn_DepartBT_Click(object sender, RoutedEventArgs e)
        {
            if (SessionObject.LsDesDepartBT.Count != 0 && this.Txt_PosteTransformateur.Tag != null)
            {
                List<object> _LstObj = Galatee.Silverlight.Shared.ClasseMEthodeGenerique.RetourneListeObjet(SessionObject.LsDesDepartBT.Where(t => t.FK_IDPOSTETRANSFORMATION == (int)this.Txt_PosteTransformateur.Tag).ToList());
                Galatee.Silverlight.MainView.UcListeGenerique ctr = new Galatee.Silverlight.MainView.UcListeGenerique(_LstObj, "CODE", "LIBELLE", "Liste des depart bt");
                ctr.Closed += new EventHandler(galatee_OkClickedbtn_DepartBT);
                ctr.Show();
            }
        }
        void galatee_OkClickedbtn_DepartBT(object sender, EventArgs e)
        {
            Galatee.Silverlight.MainView.UcListeGenerique ctrs = sender as Galatee.Silverlight.MainView.UcListeGenerique;
            if (ctrs.isOkClick)
            {
                Galatee.Silverlight.ServiceAccueil.CsDepart _LsDepart = (Galatee.Silverlight.ServiceAccueil.CsDepart)ctrs.MyObject;
                if (_LsDepart != null)
                {
                    this.Txt_DepartBt.Text = _LsDepart.CODE;
                    this.Txt_LibelleDepartBt.Text = _LsDepart.LIBELLE;
                    this.Txt_DepartBt.Tag = _LsDepart.PK_ID;
                }
            }
        }

        private void btn_TypeBrt_Click(object sender, RoutedEventArgs e)
        {
            if (laDetailDemande.Branchement.FK_IDPRODUIT != 0 && SessionObject.LstTypeBranchement.Count != 0)
            {
                List<object> _LstObj = Galatee.Silverlight.Shared.ClasseMEthodeGenerique.RetourneListeObjet(SessionObject.LstTypeBranchement.Where(t => t.FK_IDPRODUIT == laDetailDemande.Branchement.FK_IDPRODUIT).ToList());
                Galatee.Silverlight.MainView.UcListeGenerique ctr = new Galatee.Silverlight.MainView.UcListeGenerique(_LstObj, "CODE", "LIBELLE", Galatee.Silverlight.Resources.Accueil.Langue.lbl_ListeDiametre);
                ctr.Closed += new EventHandler(galatee_OkClickedBtnTypeBrt);
                ctr.Show();
            }

        }
        void galatee_OkClickedBtnTypeBrt(object sender, EventArgs e)
        {

            try
            {
                Galatee.Silverlight.MainView.UcListeGenerique ctrs = sender as Galatee.Silverlight.MainView.UcListeGenerique;
                if (ctrs.GetisOkClick)
                {
                    Galatee.Silverlight.ServiceAccueil.CsTypeBranchement _LeTypeBrt = (Galatee.Silverlight.ServiceAccueil.CsTypeBranchement)ctrs.MyObject;
                    this.Txt_TypeBrancehment.Text = _LeTypeBrt.CODE;
                    this.Txt_LibelleTypeBRT.Text = _LeTypeBrt.LIBELLE;
                    this.Txt_TypeBrancehment.Tag = _LeTypeBrt.PK_ID;
                }
                this.btn_TypeBrt.IsEnabled = true;

            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Galatee.Silverlight.Resources.Accueil.Langue.lbl_Menu);
            }
        }
        private void btn_QuartierPoste_Click_1(object sender, RoutedEventArgs e)
        {
            if (SessionObject.LstQuartier != null && SessionObject.LstQuartier.Count != 0)
            {
                List<object> _LstObj = Galatee.Silverlight.Shared.ClasseMEthodeGenerique.RetourneListeObjet(SessionObject.LstQuartier);
                Dictionary<string, string> _LstColonneAffich = new Dictionary<string, string>();
                _LstColonneAffich.Add("CODE", "CODE");
                _LstColonneAffich.Add("LIBELLE", "QUARTIER");
                List<object> obj = Shared.ClasseMEthodeGenerique.RetourneListeObjet(_LstObj);
                MainView.UcListeGenerique ctrl = new MainView.UcListeGenerique(obj, _LstColonneAffich, false, Galatee.Silverlight.Resources.Accueil.Langue.lbl_ListeDiametre);
                ctrl.Closed += new EventHandler(galatee_OkClickedBtnQuartier);
                ctrl.Show();
            }
        }
        void galatee_OkClickedBtnQuartier(object sender, EventArgs e)
        {
            Galatee.Silverlight.MainView.UcListeGenerique ctrs = sender as Galatee.Silverlight.MainView.UcListeGenerique;
            if (ctrs.isOkClick)
            {
                ServiceAccueil.CsQuartier _LeQuartier = (ServiceAccueil.CsQuartier)ctrs.MyObject;
                if (_LeQuartier != null)
                {
                    this.Txt_LibelleQuartier.Text = _LeQuartier.LIBELLE;
                    this.Txt_QuarteirPoste.Text = _LeQuartier.CODE ;
                    Txt_QuarteirPoste.Tag  = _LeQuartier.PK_ID ;
                }
            }
        }
        private void Txt_TypeBrancehment_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (laDetailDemande.Branchement.FK_IDPRODUIT != 0 && SessionObject.LstTypeBranchement.Count != 0)
                {
                    if (this.Txt_TypeBrancehment.Text.Length == SessionObject.Enumere.TailleTypeBranchement
                        && SessionObject.LstTypeBranchement.Count != 0)
                    {
                        CsTypeBranchement _leTypeBrt = ClasseMEthodeGenerique.RetourneObjectFromList(SessionObject.LstTypeBranchement, this.Txt_TypeBrancehment.Text, "CODE");
                        if (_leTypeBrt != null && !string.IsNullOrEmpty(_leTypeBrt.LIBELLE))
                        {
                            this.Txt_LibelleTypeBRT.Text = _leTypeBrt.LIBELLE;
                            this.Txt_TypeBrancehment.Tag = _leTypeBrt.PK_ID;
                        }
                        else
                        {
                            var w = new MessageBoxControl.MessageBoxChildWindow(Galatee.Silverlight.Resources.Accueil.Langue.lbl_Menu, Galatee.Silverlight.Resources.Accueil.Langue.MsgEltInexistent, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            w.OnMessageBoxClosed += (_, result) =>
                            {
                                this.Txt_TypeBrancehment.Focus();
                            };
                            w.Show();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Galatee.Silverlight.Resources.Accueil.Langue.lbl_Menu);
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
        }

        private void Txt_NeoudFinal_TextChanged(object sender, TextChangedEventArgs e)
        {

            GenereCodification();

        }
        void GenereCodification()
        {
            if (!string.IsNullOrEmpty(this.Txt_PosteSource.Text) &&
                !string.IsNullOrEmpty(this.Txt_DepartHTA.Text) &&
                !string.IsNullOrEmpty(this.Txt_QuarteirPoste.Text) &&
                !string.IsNullOrEmpty(this.Txt_PosteTransformateur.Text) &&
                !string.IsNullOrEmpty(this.Txt_DepartBt.Text) &&
                !string.IsNullOrEmpty(this.Txt_NeoudFinal.Text))
                this.Txt_AdresseElectrique.Text =
                    (this.Txt_PosteSource.Text + this.Txt_DepartHTA.Text + this.Txt_QuarteirPoste.Text +
                     this.Txt_PosteTransformateur.Text + this.Txt_DepartBt.Text +
                     this.Txt_NeoudFinal.Text);
            else
                this.Txt_AdresseElectrique.Text = string.Empty;
        }
        #endregion

        private void btn_DiametreCompteur_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.btn_DiametreCompteur.IsEnabled = false;
                if (SessionObject.LstCalibreCompteur.Count != 0)
                {
                    if (CodeProduit == SessionObject.Enumere.ElectriciteMT)
                    {

                        List<object> _LstObj = new List<object>();
                        _LstObj = ClasseMEthodeGenerique.RetourneListeObjet(SessionObject.LstTypeComptage);
                        Dictionary<string, string> _LstColonneAffich = new Dictionary<string, string>();
                        _LstColonneAffich.Add("LIBELLE", "LIBELLE");

                        List<object> obj = Shared.ClasseMEthodeGenerique.RetourneListeObjet(_LstObj);
                        MainView.UcListeGenerique ctrl = new MainView.UcListeGenerique(obj, _LstColonneAffich, false, "Lots");
                        ctrl.Closed += new EventHandler(galatee_OkClickedBtnDiametre);
                        ctrl.Show();
                    }
                    else
                    {
                        List<object> _LstObj = new List<object>();
                        _LstObj = ClasseMEthodeGenerique.RetourneListeObjet(SessionObject.LstCalibreCompteur.Where(t => t.PRODUIT == CodeProduit).ToList());
                        Dictionary<string, string> _LstColonneAffich = new Dictionary<string, string>();
                        _LstColonneAffich.Add("LIBELLE", "LIBELLE");

                        List<object> obj = Shared.ClasseMEthodeGenerique.RetourneListeObjet(_LstObj);
                        MainView.UcListeGenerique ctrl = new MainView.UcListeGenerique(obj, _LstColonneAffich, false, "Lots");
                        ctrl.Closed += new EventHandler(galatee_OkClickedBtnDiametre);
                        ctrl.Show();

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
                Galatee.Silverlight.MainView.UcListeGenerique ctrs = sender as Galatee.Silverlight.MainView.UcListeGenerique;
                if (ctrs.isOkClick)
                {
                    CsTypeComptage _LeDiametre = (CsTypeComptage)ctrs.MyObject;
                    this.Txt_LibelleDiametre.Text = _LeDiametre.CODE;
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
                Galatee.Silverlight.MainView.UcListeGenerique ctrs = sender as Galatee.Silverlight.MainView.UcListeGenerique;
                if (ctrs.isOkClick)
                {
                    CsCalibreCompteur _LeDiametre = (CsCalibreCompteur)ctrs.MyObject;
                    this.Txt_LibelleDiametre.Text = _LeDiametre.LIBELLE;
                    this.Txt_LibelleDiametre.Tag = _LeDiametre.PK_ID;
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Galatee.Silverlight.Resources.Accueil.Langue.lbl_Menu);
            }
        }

        private void btn_Marque_Click(object sender, RoutedEventArgs e)
        {
            if (SessionObject.LstMarque .Count != 0)
            {
                this.btn_Marque.IsEnabled = false;
                List<object> _LstObject = ClasseMEthodeGenerique.RetourneListeObjet(SessionObject.LstMarque);
                Galatee.Silverlight.MainView.UcListeGenerique ctr = new Galatee.Silverlight.MainView.UcListeGenerique(_LstObject, "CODE", "LIBELLE", "Liste");
                ctr.Closed += new EventHandler(galatee_OkClickedbtn_Marque);
                ctr.Show();
            }
        }
        void galatee_OkClickedbtn_Marque(object sender, EventArgs e)
        {
            try
            {
                this.btn_Marque.IsEnabled = true;
                Galatee.Silverlight.MainView.UcListeGenerique ctrs = sender as Galatee.Silverlight.MainView.UcListeGenerique;
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
        private void Txt_CodeMarque_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (this.Txt_CodeMarque.Text.Length == SessionObject.Enumere.TailleCodeMarqueCompteur && (SessionObject.LstMarque != null && SessionObject.LstMarque.Count != 0))
                {
                    CsMarqueCompteur _LaMarque = ClasseMEthodeGenerique.RetourneObjectFromList(SessionObject.LstMarque , this.Txt_CodeMarque.Text, "CODE");
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
    }
}

