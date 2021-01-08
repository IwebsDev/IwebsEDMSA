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
    public partial class FrmValidationTransfertClient : ChildWindow
    {
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


        public FrmValidationTransfertClient()
        {
            InitializeComponent();
            Txt_Porte.MaxLength = 5;
            Txt_Etage.MaxLength = 2;
            prgBar.Visibility = System.Windows.Visibility.Collapsed;

            this.Txt_CodeConso.MaxLength = SessionObject.Enumere.TailleCodeConso;
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

            txt_Reglage.Visibility = Visibility.Collapsed;
            Btn_Reglage.Visibility = Visibility.Collapsed;

            this.tabItemPersoneAdministration.Visibility = System.Windows.Visibility.Collapsed;
            this.tabItemPersoneMoral.Visibility = System.Windows.Visibility.Collapsed;
            this.tab_proprio.Visibility = System.Windows.Visibility.Collapsed;


            txt_MaticuleAgent.Visibility = System.Windows.Visibility.Collapsed;
            lbl_MatriculeAgent.Visibility = System.Windows.Visibility.Collapsed;
            tbControleClient.IsEnabled = false;
            this.dtp_RendezVousPrev.SelectedDate = System.DateTime.Today.AddDays(15);
            dataGridForniture.Visibility = System.Windows.Visibility.Collapsed;
          
            ChargerCategorieClient_TypeClient();
            ChargerNatureClient_TypeClient();
            ChargerUsage_NatureClient();
            ChargerCategorieClient_Usage();
            RemplirStatutJuridique();
            RemplirTourneeExistante();
            RemplirCategorieClient();
            RemplirPieceIdentite();
            RemplirUsage();
            RemplirCodeConsomateur();
            RemplirSecteur();
            RemplirNationnalite();
            RemplirCommune();
            RemplirListeDesQuartierExistant();
            RemplirListeDesRuesExistant();
            RemplirListeDesDiametresExistant();
            RemplirTypeClient();
            RemplirProprietaire();
            //Activation de la zone de recherche en fonction du type de demande
            ActivationEnFonctionDeTdem();

            List<string> ListOperation = new List<string>();
            ListOperation = SessionObject.TypeOperationClasseur().ToList();
            tab4_cbo_Operation.ItemsSource = null;
            tab4_cbo_Operation.ItemsSource = ListOperation;
            if (ListOperation != null && ListOperation.Count != 0)
                tab4_cbo_Operation.SelectedItem = ListOperation[0];
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

        public FrmValidationTransfertClient(string Tdem, string IsInit)
        {
            try
            {
                InitializeComponent();
                this.Tdem = Tdem;
                ModeExecution = SessionObject.ExecMode.Creation;
                prgBar.Visibility = System.Windows.Visibility.Collapsed;

                Txt_Porte.MaxLength = 5;
                Txt_Etage.MaxLength = 2;

                this.Txt_CodeConso.MaxLength = SessionObject.Enumere.TailleCodeConso;
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

                txt_Reglage.Visibility = Visibility.Collapsed;
                Btn_Reglage.Visibility = Visibility.Collapsed;
              
                this.tabItemPersoneAdministration.Visibility = System.Windows.Visibility.Collapsed;
                this.tabItemPersoneMoral.Visibility = System.Windows.Visibility.Collapsed;
                this.tab_proprio.Visibility = System.Windows.Visibility.Collapsed;
                dataGridForniture.Visibility = System.Windows.Visibility.Collapsed;


                txt_MaticuleAgent.Visibility = System.Windows.Visibility.Collapsed;
                lbl_MatriculeAgent.Visibility = System.Windows.Visibility.Collapsed;
                tbControleClient.IsEnabled = false;
                this.dtp_RendezVousPrev.SelectedDate = System.DateTime.Today.AddDays(15);

                //ChargerPuissance();
                ChargerCategorieClient_TypeClient();
                ChargerNatureClient_TypeClient();
                ChargerUsage_NatureClient();
                ChargerCategorieClient_Usage();
                RemplirStatutJuridique();
                RemplirTourneeExistante();
                RemplirCategorieClient();
                RemplirPieceIdentite();
                RemplirUsage();
                RemplirCodeConsomateur();
                RemplirSecteur();
                RemplirNationnalite();
                RemplirCommune();
                RemplirListeDesQuartierExistant();
                RemplirListeDesRuesExistant();
                RemplirListeDesDiametresExistant();
                RemplirTypeClient();
                RemplirProprietaire();
                //Activation de la zone de recherche en fonction du type de demande
                ActivationEnFonctionDeTdem();

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

        public FrmValidationTransfertClient(int IdDemande)
        {
            try
            {
                InitializeComponent();

                //Txt_Porte.MaxLength = 5;
                //Txt_Etage.MaxLength = 2;
                prgBar.Visibility = System.Windows.Visibility.Collapsed;

                this.Txt_CodeConso.MaxLength = SessionObject.Enumere.TailleCodeConso;
                this.TxtCategorieClient.MaxLength = SessionObject.Enumere.TailleCodeCategorie;
                this.Txt_usage.MaxLength = SessionObject.Enumere.TailleUsage;
                this.txt_Commune.MaxLength = SessionObject.Enumere.TailleCommune;
                this.txt_Quartier.MaxLength = SessionObject.Enumere.TailleQuartier;
                this.txt_NumSecteur.MaxLength = SessionObject.Enumere.TailleSecteur;
                this.txt_NumRue.MaxLength = SessionObject.Enumere.TailleRue;


                txt_Reglage.Visibility = Visibility.Collapsed;
                Btn_Reglage.Visibility = Visibility.Collapsed;
                this.tabItemPersoneAdministration.Visibility = System.Windows.Visibility.Collapsed;
                this.tabItemPersoneMoral.Visibility = System.Windows.Visibility.Collapsed;
                this.tab_proprio.Visibility = System.Windows.Visibility.Collapsed;

                txt_MaticuleAgent.Visibility = System.Windows.Visibility.Collapsed;
                lbl_MatriculeAgent.Visibility = System.Windows.Visibility.Collapsed;
                tbControleClient.IsEnabled = false;
                dataGridForniture.Visibility = System.Windows.Visibility.Collapsed;

                ChargerCategorieClient_TypeClient();
                ChargerNatureClient_TypeClient();
                ChargerUsage_NatureClient();
                ChargerCategorieClient_Usage();
                RemplirStatutJuridique();
                RemplirTourneeExistante();
                RemplirCategorieClient();
                RemplirPieceIdentite();
                RemplirUsage();
                RemplirCodeConsomateur();
                RemplirSecteur();
                RemplirNationnalite();
                RemplirCommune();
                RemplirListeDesQuartierExistant();
                RemplirListeDesRuesExistant();
                RemplirListeDesDiametresExistant();
                RemplirTypeClient();
                RemplirProprietaire();
                ActivationEnFonctionDeTdem();
                ChargeDetailDEvis(IdDemande);
           
                List<string> ListOperation = new List<string>();
                ListOperation = SessionObject.TypeOperationClasseur().ToList();
                tab4_cbo_Operation.ItemsSource = null;
                tab4_cbo_Operation.ItemsSource = ListOperation;
                if (ListOperation != null && ListOperation.Count != 0)
                    tab4_cbo_Operation.SelectedItem = ListOperation[0];

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
                        LstCategorieClient_TypeClient.Add(item);
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
                        LstNatureClient_TypeClient.Add(item);

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
                        LstUsage_NatureClient.Add(item);

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
                        LstCategorieClient_Usage.Add(item);
                };
                service.ChargerCategorieClient_UsageAsync();
                service.CloseAsync();

            }
            catch (Exception)
            {

                throw;
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
                     this.txtCentre_Origine.Text = laDetailDemande.Transfert.LIBELLECENTREORIGINE;
                    this.txtSite_Origine .Text = laDetailDemande.Transfert.LIBELLESITEORIGINE ;
                    this.txtCentreTransfert.Text = laDetailDemande.Transfert.LIBELLECENTRETRANSFERT;
                    this.txtSiteTransfert.Text = laDetailDemande.Transfert.LIBELLESITETRANSFERT;
                    this.Txt_CodeRegroupement.Text = string.IsNullOrEmpty(laDetailDemande.Transfert.CODEREGROUPEMENT) ? string.Empty : laDetailDemande.Transfert.CODEREGROUPEMENT;
                    this.txtNumeroDemande.Text = laDetailDemande.LaDemande.NUMDEM;
                    this.txtProduit.Text = laDetailDemande.LaDemande.LIBELLEPRODUIT;
                    this.txt_Ref_Branchement.Text = laDetailDemande.LaDemande.CLIENT;
                    this.txt_ordre.Text = laDetailDemande.LaDemande.ORDRE ;

                    CsClientRechercher leclientRech = new CsClientRechercher()
                    {
                        CENTRE = laDetailDemande.LeClient.CENTRE,
                        CLIENT = laDetailDemande.LeClient.REFCLIENT,
                        ORDRE = laDetailDemande.LeClient.ORDRE,
                        FK_IDCENTRE = laDetailDemande.LeClient.FK_IDCENTRE.Value,
                    };
                    if (laDetailDemande.Abonne.PRODUIT == SessionObject.Enumere.Electricite ||
                       laDetailDemande.Abonne.PRODUIT == SessionObject.Enumere.Prepaye)
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
                    RemplirTourneeExistanteParCentre(laDetailDemande.Transfert.FK_IDCENTRETRANSFERT);
                    RetourneLeCompteClient(leclientRech);
                    RemplireOngletCanalisation(laDetailDemande.LstCanalistion);
                    this.txtNumeroDemande.Text = !string.IsNullOrEmpty(laDetailDemande.LaDemande.NUMDEM) ? laDetailDemande.LaDemande.NUMDEM : string.Empty;
                    laDemandeSelect = laDetailDemande.LaDemande;
                    RenseignerChampsSurLeControl(laDetailDemande);


                   
 
                }
                LayoutRoot.Cursor = Cursors.Arrow;
            };
        }
        private void Translate()
        {
            try
            {
                this.Btn_Annuler.Content = Languages.btnAnnuler;
                this.Title = Languages.ttlCreationDevis;
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
        private void ValidationDevis(CsDemande laDetailDemande, bool IsTransmetre)
        {
            try
            {
                #region Abon
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


                laDetailDemande.Abonne.FK_IDCENTRE = laDetailDemande.LeClient.PK_ID;
                laDetailDemande.Abonne.FK_IDCENTRE = laDetailDemande.LaDemande.FK_IDCENTRE;
                laDetailDemande.Abonne.FK_IDPRODUIT = laDetailDemande.LaDemande.FK_IDPRODUIT.Value;
                laDetailDemande.Abonne.TYPETARIF  =string.IsNullOrEmpty( this.Txt_CodeTarif.Text) ? laDetailDemande.Abonne.TYPETARIF  : this.Txt_CodeTarif.Text ;
                laDetailDemande.Abonne.FK_IDTYPETARIF = this.Txt_CodeTarif.Tag == null ? laDetailDemande.Abonne.FK_IDTYPETARIF : (int)this.Txt_CodeTarif.Tag;
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
                if (Cbo_Zone.SelectedItem != null)
                {
                    var Zone = Cbo_Zone.SelectedItem as CsTournee ;
                    if (Zone != null)
                    {
                        laDetailDemande.Ag.FK_IDTOURNEE  = Zone.PK_ID;
                        laDetailDemande.Ag.TOURNEE = Zone.CODE ;
                    }
                }
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
                //laDetailDemande.Ag.TELEPHONE = string.IsNullOrEmpty(this.txt_Telephone.Text) ? null : this.txt_Telephone.Text;
                laDetailDemande.Ag.ETAGE = string.IsNullOrEmpty(this.Txt_Etage.Text) ? null : this.Txt_Etage.Text;
                laDetailDemande.Ag.PORTE = string.IsNullOrEmpty(this.Txt_Porte.Text) ? null : this.Txt_Porte.Text;
                laDetailDemande.Ag.CENTRE = string.IsNullOrEmpty(laDetailDemande.LaDemande.CENTRE) ? null : laDetailDemande.LaDemande.CENTRE;
                laDetailDemande.Ag.CLIENT = string.IsNullOrEmpty(laDetailDemande.LaDemande.CLIENT) ? null : laDetailDemande.LaDemande.CLIENT;
                laDetailDemande.Ag.NUMDEM = string.IsNullOrEmpty(this.txtNumeroDemande.Text) ? null : this.txtNumeroDemande.Text;
                laDetailDemande.Ag.FK_IDCENTRE = laDetailDemande.LaDemande.FK_IDCENTRE;
                laDetailDemande.Ag.DATECREATION = DateTime.Now;
                laDetailDemande.Ag.USERCREATION = UserConnecte.matricule;
                #endregion
                #region Client
              
                    if (laDetailDemande.LeClient == null)
                        laDetailDemande.LeClient = new CsClient();
                    if (Cbo_Categorie.SelectedItem != null)
                    {
                        var Categorie = Cbo_Categorie.SelectedItem as CsCategorieClient;
                        if (Categorie != null)
                        {
                            laDetailDemande.LeClient.CATEGORIE = Categorie.CODE;
                            laDetailDemande.LeClient.FK_IDCATEGORIE  = Categorie.PK_ID ;
                        }
                    }
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

                    //if (Cbo_Type_Client.SelectedItem != null)
                    //{
                    //    var TypeClient = Cbo_Type_Client.SelectedItem as CsTypeClient;
                    //    if (TypeClient != null)
                    //    {
                    //        //laDetailDemande.LeClient.NATIONNALITE = TypeClient.CODE;
                    //        laDetailDemande.LeClient.FK_TYPECLIENT = TypeClient.PK_ID;
                    //    }

                    //}


                    //if (Cbo_TypePiece.SelectedItem != null)
                    //{
                    //    var TypeClient = Cbo_TypePiece.SelectedItem as CsUsage;
                    //    if (TypeClient != null)
                    //    {
                    //        laDetailDemande.LeClient.FK_IDUSAGE = TypeClient.PK_ID;
                    //    }
                    //}

                    #endregion

                    laDetailDemande.LeClient.TELEPHONEFIXE   = string.IsNullOrEmpty(this.txt_Telephone_Fixe.Text) ? null : this.txt_Telephone_Fixe.Text;
                    laDetailDemande.LeClient.FAX = string.IsNullOrEmpty(this.Txt_NumFax.Text) ? null : this.Txt_NumFax.Text;
                    laDetailDemande.LeClient.BOITEPOSTAL = string.IsNullOrEmpty(this.Txt_BoitePostale.Text) ? null : this.Txt_BoitePostale.Text;
                    laDetailDemande.LeClient.TELEPHONE = string.IsNullOrEmpty(this.txt_Telephone.Text) ? null : this.txt_Telephone.Text;
                    laDetailDemande.LeClient.NUMPROPRIETE = string.IsNullOrEmpty(this.txtPropriete.Text) ? null : this.txtPropriete.Text;
                    laDetailDemande.LeClient.CENTRE = string.IsNullOrEmpty(laDetailDemande.LaDemande.CENTRE) ? null : laDetailDemande.LaDemande.CENTRE;
                    laDetailDemande.LeClient.REFCLIENT = string.IsNullOrEmpty(laDetailDemande.LaDemande.CLIENT) ? null : laDetailDemande.LaDemande.CLIENT;
                    laDetailDemande.LeClient.FK_IDCENTRE = laDetailDemande.LaDemande.FK_IDCENTRE;
                    laDetailDemande.LeClient.DATECREATION = DateTime.Now;
                    laDetailDemande.LeClient.USERCREATION = UserConnecte.matricule;
                    laDetailDemande.LeClient.NUMDEM = string.IsNullOrEmpty(this.txtNumeroDemande.Text) ? null : this.txtNumeroDemande.Text;
                    laDetailDemande.LeClient.ISFACTUREEMAIL = chk_Email.IsChecked.Value;
                    laDetailDemande.LeClient.ISFACTURESMS = chk_SMS.IsChecked.Value;
                    if (laDetailDemande.Transfert.FK_IDREGROUPEMENT == null)
                    {
                        laDetailDemande.LeClient.FK_IDREGROUPEMENT = laDetailDemande.Transfert.FK_IDREGROUPEMENT;
                        laDetailDemande.LeClient.REGROUPEMENT = laDetailDemande.Transfert.CODEREGROUPEMENT ;
                    }
                    else
                    {
                        laDetailDemande.LeClient.FK_IDREGROUPEMENT = null;
                        laDetailDemande.LeClient.REGROUPEMENT = null;
                    }

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

                    if (this.Cbo_Type_Client.SelectedItem  != null)
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
                 

                #endregion
                #region Transfert
                laDetailDemande.Transfert.TRANSFERIMPAYE = this.Chk_TransfertImpayer.IsChecked ==true ? true : false ;
                #endregion
                #region Contrat
                if (laDetailDemande.EltDevis != null && laDetailDemande.EltDevis.Count != 0)
                {
                     
                        laDetailDemande.EltDevis = ((List<ObjELEMENTDEVIS>)this.dataGridForniture.ItemsSource).Where(o => o.QUANTITE != 0 && o.QUANTITE != null).ToList();
                        int idTrv = SessionObject.LstDesCopers.FirstOrDefault(t => t.CODE == SessionObject.Enumere.CoperTRV).PK_ID;

                        if (laDetailDemande.LstCoutDemande != null && laDetailDemande.LstCoutDemande.Count != 0)
                            laDetailDemande.LstCoutDemande = new List<CsDemandeDetailCout>();

                        foreach (ObjELEMENTDEVIS item in laDetailDemande.EltDevis)
                        {

                            CsDemandeDetailCout leCoutduDevis = new CsDemandeDetailCout();

                            leCoutduDevis.CENTRE = string.IsNullOrEmpty(laDetailDemande.LaDemande.CENTRE) ? null : laDetailDemande.LaDemande.CENTRE;
                            leCoutduDevis.CLIENT = string.IsNullOrEmpty(laDetailDemande.LaDemande.CLIENT) ? null : laDetailDemande.LaDemande.CLIENT;
                            leCoutduDevis.ORDRE = string.IsNullOrEmpty(laDetailDemande.LaDemande.ORDRE) ? null : laDetailDemande.LaDemande.ORDRE;
                            leCoutduDevis.FK_IDCENTRE = laDetailDemande.LaDemande.FK_IDCENTRE;
                            leCoutduDevis.NUMDEM = string.IsNullOrEmpty(laDetailDemande.LaDemande.NUMDEM) ? string.Empty : laDetailDemande.LaDemande.NUMDEM;
                            leCoutduDevis.COPER = SessionObject.LstDesCopers.FirstOrDefault(t => t.PK_ID == item.FK_IDCOPER).CODE;
                            leCoutduDevis.FK_IDCOPER = item.FK_IDCOPER.Value;
                            leCoutduDevis.FK_IDTAXE = item.FK_IDTAXE.Value;
                            leCoutduDevis.FK_IDDEMANDE = laDetailDemande.LaDemande.PK_ID;
                            leCoutduDevis.MONTANTHT = (decimal?)Math.Ceiling((double)item.MONTANTHT);
                            leCoutduDevis.MONTANTTAXE = (decimal?)Math.Ceiling((double)item.MONTANTTAXE);
                            leCoutduDevis.REFEM = DateTime.Today.Year.ToString() + DateTime.Today.Month.ToString("00");
                            leCoutduDevis.DATECREATION = DateTime.Now;
                            leCoutduDevis.USERCREATION = UserConnecte.matricule;
                            if (laDetailDemande.LstCoutDemande == null)
                            {
                                laDetailDemande.LstCoutDemande = new List<CsDemandeDetailCout>();
                                laDetailDemande.LstCoutDemande.Add(leCoutduDevis);
                            }
                            else
                                laDetailDemande.LstCoutDemande.Add(leCoutduDevis);
                        
                    }
                }
                #endregion

                laDetailDemande.LaDemande.FK_IDREGLAGECOMPTEUR = (this.txt_Reglage.Tag == null) ? null : (int?)this.txt_Reglage.Tag;
                laDetailDemande.LaDemande.REGLAGECOMPTEUR = this.Btn_Reglage.Tag == null ? string.Empty : this.Btn_Reglage.Tag.ToString();

                laDetailDemande.LaDemande.MATRICULE = UserConnecte.matricule;
                laDetailDemande.LaDemande = laDetailDemande.LaDemande;
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient client = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                client.ValiderDemandeTransfertCompleted += (sss, bs) =>
                {
                    this.Btn_Rejeter.IsEnabled = true;
                    this.Btn_Transmettre.IsEnabled = true;
                    prgBar.Visibility = System.Windows.Visibility.Collapsed;

                    if (bs.Cancelled || bs.Error != null)
                    {
                        string error = bs.Error.Message;
                        Message.ShowError(error, Silverlight.Resources.Devis.Languages.txtDevis);
                        return;
                    }
                    if (bs.Result == true)
                    {
                        List<string> codes = new List<string>();
                        codes.Add(laDetailDemande.InfoDemande.CODE);
                        Galatee.Silverlight.Shared.ClasseMEthodeGenerique.TransmettreDemande(codes, true, this);
                        //if (this.Chk_AvecMutation.IsChecked == true )
                        Contrat(laDetailDemande);
                    }

                };
                client.ValiderDemandeTransfertAsync(laDetailDemande, leClasseurClient.ToutLClient);
                this.DialogResult = true;
                
            }
            catch (Exception ex)
            {
                LayoutRoot.Cursor = Cursors.Arrow;
                throw ex;
            }
        }
        CsReglageCompteur ReglageCompt = new CsReglageCompteur();
        private void Contrat(CsDemande laDemande)
        {
            int idreglageCpt = 0;
            ReglageCompt = SessionObject.LstReglageCompteur.FirstOrDefault(t => t.CODE == Btn_Reglage.Tag.ToString());
            if (ReglageCompt != null)
                idreglageCpt = ReglageCompt.PK_ID;

            ChargerTarifClient(laDetailDemande.LaDemande.FK_IDCENTRE, laDetailDemande.LeClient.FK_IDCATEGORIE.Value, idreglageCpt, null, "0", laDetailDemande.LaDemande.FK_IDPRODUIT.Value);
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
            leContrat.COMMUNE = laDemande.Ag.LIBELLECOMMUNE;
            leContrat.LIBELLEPRODUIT = laDemande.LaDemande.LIBELLEPRODUIT;
            leContrat.LONGUEURBRANCHEMENT = laDemande.Branchement.LONGBRT.ToString();

            leContrat.NATUREBRANCHEMENT = laDemande.Branchement.LIBELLETYPEBRANCHEMENT;
            leContrat.NOMCLIENT = laDemande.LeClient.NOMABON;
            leContrat.NOMPROPRIETAIRE = laDemande.Ag.NOMP;
            leContrat.NUMDEMANDE = laDemande.LaDemande.NUMDEM;
            leContrat.ORDRE = laDemande.LaDemande.ORDRE;
            leContrat.PORTE = laDemande.Ag.PORTE;
            leContrat.PUISSANCESOUSCRITE = laDemande.Abonne.PUISSANCE.Value.ToString(SessionObject.FormatMontant);
            leContrat.QUARTIER = laDemande.Ag.LIBELLEQUARTIER;
            leContrat.QUARTIERCLIENT = laDemande.Ag.LIBELLEQUARTIER;
            leContrat.REGLAGEDISJONCTEUR = ReglageCompt != null ? ReglageCompt.REGLAGEMINI.Value.ToString() + "/" + ReglageCompt.REGLAGEMAXI.Value.ToString() : string.Empty;
            leContrat.CALIBRE = ReglageCompt != null ? ReglageCompt.REGLAGE.Value.ToString() : string.Empty;

            leContrat.FRAISPOLICE = ReglageCompt != null ? ReglageCompt.REGLAGE.Value.ToString() + "A" : string.Empty;
            leContrat.NBREFILS = ReglageCompt != null ? ReglageCompt.CODE.Substring(0, 1) : string.Empty;
            leContrat.FRAISTIMBRE = System.DateTime.Today.ToShortDateString();
            leContrat.TOTAL1 = laDetailDemande.Ag.TOURNEE + "   " + laDetailDemande.Ag.ORDTOUR;
            leContrat.NUMEROPIECE = laDetailDemande.LeClient.LIBELLETYPEPIECE + "  N° " + laDemande.LeClient.NUMEROPIECEIDENTITE;
            leContrat.USAGE = laDetailDemande.LeClient.LIBELLECODECONSO;

            leContrat.MONTANTPARTICAPATION = laDetailDemande.LeClient.REGROUPEMENT;

            leContrat.RUE = laDemande.Ag.LIBELLERUE;
            leContrat.RUECLIENT = laDemande.Ag.LIBELLERUE;
            leContrat.TELEPHONE = laDemande.LeClient.TELEPHONE;
            leContrat.MONTANTAVANCE = laDetailDemande.EltDevis.Where(t => t.CODECOPER == SessionObject.Enumere.CoperCAU) == null ? string.Empty :
                laDetailDemande.EltDevis.Where(t => t.CODECOPER == SessionObject.Enumere.CoperCAU).Sum(t => t.MONTANTTTC).Value.ToString(SessionObject.FormatMontant);
            List<CsContrat> lstContrat = new List<CsContrat>();
            lstContrat.Add(leContrat);
            foreach (ObjAPPAREILSDEVIS item in laDetailDemande.AppareilDevis)
            {
                CsContrat leContrats = new CsContrat();
                leContrats.APPAREIL = item.DESIGNATION;
                leContrats.QUANTITE = item.NBRE.Value;
                leContrats.OPTIONEDITION = "A";
                lstContrat.Add(leContrats);
            }
            if (laDetailDemande.EltDevis != null && laDetailDemande.EltDevis.Count != 0 && laDetailDemande.EltDevis.Where(t => t.CODECOPER == SessionObject.Enumere.CoperTRV) != null)
            {
                CsContrat leContratss = new CsContrat();
                leContratss.MONTANTRUBIQE = laDetailDemande.EltDevis.Where(t => t.CODECOPER == SessionObject.Enumere.CoperTRV).Sum(t => t.MONTANTTTC).Value;
                leContratss.TOTALGENERAL = laDetailDemande.EltDevis.Where(t => t.CODECOPER == SessionObject.Enumere.CoperTRV).Sum(t => t.MONTANTTAXE).Value.ToString(SessionObject.FormatMontant);
                leContratss.TOTAL2 = (laDetailDemande.EltDevis.FirstOrDefault().TAUXTAXE * 100).Value.ToString(SessionObject.FormatMontant) + "%";

                leContratss.RUBRIQUE = SessionObject.LstDesCopers.First(t => t.CODE == SessionObject.Enumere.CoperTRV).LIBELLE;
                leContratss.OPTIONEDITION = "R";
                if (leContratss.MONTANTRUBIQE > 0)
                    lstContrat.Add(leContratss);
            }
            if (laDetailDemande.EltDevis != null && laDetailDemande.EltDevis.Count != 0 && laDetailDemande.EltDevis.Where(t => t.CODECOPER == SessionObject.Enumere.CoperFAB) != null)
            {
                CsContrat leContratss = new CsContrat();
                leContratss.MONTANTRUBIQE = laDetailDemande.EltDevis.Where(t => t.CODECOPER == SessionObject.Enumere.CoperFAB).Sum(t => t.MONTANTTTC).Value;

                leContratss.RUBRIQUE = SessionObject.LstDesCopers.First(t => t.CODE == SessionObject.Enumere.CoperFAB).LIBELLE;
                leContratss.OPTIONEDITION = "R";
                if (leContratss.MONTANTRUBIQE > 0)
                    lstContrat.Add(leContratss);
            }
            if (laDetailDemande.EltDevis != null && laDetailDemande.EltDevis.Count != 0)
            foreach (ObjELEMENTDEVIS item in laDetailDemande.EltDevis.Where(t => t.CODECOPER != SessionObject.Enumere.CoperTRV && t.CODECOPER != SessionObject.Enumere.CoperFAB))
            {
                CsContrat leContratss = new CsContrat();
                leContratss.MONTANTRUBIQE = item.MONTANTTTC.Value;

                leContratss.RUBRIQUE = item.DESIGNATION;
                leContratss.OPTIONEDITION = "R";
                lstContrat.Add(leContratss);
            }


            if (lstTarif != null && lstTarif.Count != 0)
                foreach (CsTarifClient item in lstTarif)
                {
                    CsContrat leContratss = new CsContrat();
                    leContratss.REDEVANCE = item.REDEVANCE;
                    leContratss.TRANCHE = item.PLAGE;
                    leContratss.MONTANT = item.PRIXUNITAIRE;
                    leContratss.OPTIONEDITION = "T";
                    lstContrat.Add(leContratss);
                }
            if (lstTarif != null && lstTarif.Count != 0)
                for (int i = 0; i < 14 - (lstTarif.Count); i++)
                {
                    CsContrat leContratss = new CsContrat();
                    leContratss.CHAMPVIDE = "";
                    leContratss.OPTIONEDITION = "V";
                    lstContrat.Add(leContratss);
                }
            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("pTypedemande", laDetailDemande.LaDemande.LIBELLETYPEDEMANDE);
            Utility.ActionDirectOrientation<ServicePrintings.CsContrat, CsContrat>(lstContrat, param, SessionObject.CheminImpression, "ContratClient", "Accueil", true);
        }
        List<CsTarifClient> lstTarif = new List<CsTarifClient>();
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

                    }
                    else
                    {
                        lstTarif = args.Result;
                        lstTarif.ForEach(t => t.REDEVANCE = t.REDEVANCE + " " + t.TRANCHE.ToString());
                        EditerContrat(laDetailDemande);
                    }
                };
                client.RetourneTarifClientAsync(idcentre, idcategorie, idreglageCompteur, idtypecomptage, propriotaire, idproduit);
            }
            catch (Exception ex)
            {
                Message.ShowInformation("Erreur au chargement des tarif", "Demande");
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
                    Cbo_Commune.ItemsSource = _listeDesCommuneExistant;
                    Cbo_Commune.IsEnabled = true;
                    Cbo_Commune.SelectedValuePath = "PK_ID";
                    Cbo_Commune.DisplayMemberPath = "LIBELLE";
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
                    Cbo_Commune.ItemsSource = _listeDesCommuneExistant;
                    Cbo_Commune.IsEnabled = true;
                    Cbo_Commune.SelectedValuePath = "PK_ID";
                    Cbo_Commune.DisplayMemberPath = "LIBELLE";

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
                    Cbo_Quartier.ItemsSource = _listeDesQuartierExistant;
                    Cbo_Quartier.SelectedValuePath = "PK_ID";
                    Cbo_Quartier.DisplayMemberPath = "LIBELLE";
                    Cbo_Quartier.ItemsSource = ListeQuartierFiltres;

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
                    Cbo_Quartier.ItemsSource = _listeDesQuartierExistant;
                    Cbo_Quartier.SelectedValuePath = "PK_ID";
                    Cbo_Quartier.DisplayMemberPath = "LIBELLE";
                    Cbo_Quartier.ItemsSource = ListeQuartierFiltres;
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
        private void RenseignerChampsSurLeControl(CsDemande laDemande)
        {
            try
            {
                if (laDemande != null)
                {

                    #region Demande
                    if (laDemande.LaDemande != null)
                        txtNumeroDemande.Text = !string.IsNullOrEmpty(laDemande.LaDemande.NUMDEM) ? laDemande.LaDemande.NUMDEM : string.Empty;
                    #endregion
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
                            this.Txt_NomClient_PersonePhysiq.Text = laDetailDemande.LeClient.NOMABON != null ? laDetailDemande.LeClient.NOMABON : string.Empty;
                            this.txtNumeroPiece.Text = laDetailDemande.LeClient.NUMEROPIECEIDENTITE != null ? laDetailDemande.LeClient.NUMEROPIECEIDENTITE : string.Empty;
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
                    #region Abon
                    if (laDetailDemande.Abonne != null)
                    {
                        this.Txt_CodeTarif.Text = !string.IsNullOrEmpty(laDetailDemande.Abonne.TYPETARIF) ? laDetailDemande.Abonne.TYPETARIF : string.Empty;

                   
                        if (laDetailDemande.Abonne.PUISSANCE != null)
                            this.Txt_CodePussanceSoucrite.Text = Convert.ToDecimal(laDetailDemande.Abonne.PUISSANCE.ToString()).ToString("N2");

                        this.Txt_CodeTarif.Text = string.IsNullOrEmpty(laDetailDemande.Abonne.TYPETARIF) ? string.Empty : laDetailDemande.Abonne.TYPETARIF;
                        this.Txt_LibelleTarif.Text = !string.IsNullOrEmpty(laDetailDemande.Abonne.LIBELLETARIF) ? laDetailDemande.Abonne.LIBELLETARIF : string.Empty;

                        this.Chk_IsExonneration.IsChecked = (laDetailDemande.Abonne.ESTEXONERETVA == true) ? true : false;
                        this.txt_DebutPeriodeExo.Text = string.IsNullOrEmpty(laDetailDemande.Abonne.DEBUTEXONERATIONTVA) ? string.Empty :
                                                 Shared.ClasseMEthodeGenerique.FormatPeriodeMMAAAA(laDetailDemande.Abonne.DEBUTEXONERATIONTVA);

                        this.txt_FinPeriodeExo.Text = string.IsNullOrEmpty(laDetailDemande.Abonne.FINEXONERATIONTVA) ? string.Empty :
                                   Shared.ClasseMEthodeGenerique.FormatPeriodeMMAAAA(laDetailDemande.Abonne.FINEXONERATIONTVA);
                    }


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
                        ChargerPuissanceEtTarif(idProduit, idPuissance, idCategorie, idReglage);

                    #endregion
                }

            }
            catch (Exception ex)
            {
                Message.ShowInformation("Erreur a l'affichage des donnée", "Init");
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
                //txt_NumLot.IsEnabled = pValue;
                Cbo_Usage.IsEnabled = pValue;
                Cbo_Commune.IsEnabled = pValue;
                Cbo_Quartier.IsEnabled = pValue;
                Cbo_Rue.IsEnabled = pValue;
                Cbo_Secteur.IsEnabled = pValue;
                Cbo_Nationnalite.IsEnabled = pValue;

                #region Sylla 24/09/2015
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
                if (SessionObject.LstTypeClient.Count != 0)
                    this.Cbo_Type_Client.SelectedItem = SessionObject.LstTypeClient.Where(t => t.CODE == "001").ToList();
        
                label21.Visibility = Visibility.Visible;
                txt_Reglage.Visibility = Visibility.Visible;
                Btn_Reglage.Visibility = Visibility.Visible;
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
                var lst = SessionObject.LstCategorie;
                ReloadCategClient(lst);
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
            pDemandeDevis.PersonePhysique.DATEFINVALIDITE =!string.IsNullOrEmpty( dtp_DateValidite.Text) ?  Convert.ToDateTime(dtp_DateValidite.Text): System.DateTime.Today.AddYears(5);
            pDemandeDevis.PersonePhysique.DATENAISSANCE = !string.IsNullOrEmpty(dtp_DateNaissance.Text) ? Convert.ToDateTime(dtp_DateNaissance.Text) : System.DateTime.Today.AddYears(30);
            pDemandeDevis.PersonePhysique.NUMEROPIECEIDENTITE =!string.IsNullOrEmpty( txtNumeroPiece.Text)? txtNumeroPiece.Text.Trim():"1111111";
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

                        int idProduit = 0;
                        if (laDetailDemande.LaDemande.PRODUIT  != null)
                            idProduit = laDetailDemande.LaDemande.FK_IDPRODUIT .Value ;

                        int? idPuissance = null;

                        int idCategorie = 0;
                        if (Cbo_Categorie.SelectedItem != null)
                            idCategorie = (Cbo_Categorie.SelectedItem as CsCategorieClient).PK_ID;

                        ChargerPuissanceEtTarif(idProduit, idPuissance, idCategorie, ctrs.leReglageSelect.PK_ID);

                        if (Chk_AvecMutation.IsChecked == true)
                            ChargerCoutDemande();
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
                            this.txt_MaticuleAgent.Visibility = System.Windows.Visibility.Collapsed ;
                            this.lbl_MatriculeAgent.Visibility = System.Windows.Visibility.Collapsed ;
                        }
                    }


                        int? idPuissance = null;

                        int idReglageCompteur = 0;
                        if (this.txt_Reglage.Tag != null)
                            idReglageCompteur = (int)this.txt_Reglage.Tag;

                        int idProduit = 0;
                        if (laDetailDemande.LaDemande.PRODUIT != null)
                            idProduit = laDetailDemande.LaDemande.FK_IDPRODUIT.Value  ;
                        ChargerPuissanceEtTarif(idProduit, idPuissance, cat.PK_ID, idReglageCompteur);

                    if (Chk_AvecMutation.IsChecked == true)
                        ChargerCoutDemande();

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
                var lst = SessionObject.LstCategorie;
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
            this.Btn_Rejeter.IsEnabled = false;
            this.Btn_Transmettre.IsEnabled = false;
            prgBar.Visibility = System.Windows.Visibility.Visible ;
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
                        LstPuissanceMt = SessionObject.LstPuissanceParReglageCompteur.Where(t => t.REGLAGECOMPTEUR == laDetailDemande.LaDemande.REGLAGECOMPTEUR && t.PRODUIT == laDetailDemande.LaDemande.PRODUIT).ToList();
                        List<object> _LstObjet = Shared.ClasseMEthodeGenerique.RetourneListeObjet(LstPuissanceMt);
                        Galatee.Silverlight.MainView.UcListeGenerique ctr = new Galatee.Silverlight.MainView.UcListeGenerique(_LstObjet, "CODE", "PUISSANCE", "Liste");
                        ctr.Closed += new EventHandler(galatee_OkClickedBtnpuissanceSouscrite);
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
                    this.Txt_CodePussanceSoucrite.Tag = _LaPuissanceSelect.PK_ID;



                    int idCategorie = 0;
                    if (Cbo_Categorie.SelectedItem != null)
                        idCategorie = (Cbo_Categorie.SelectedItem as CsCategorieClient).PK_ID;

                    int idReglageCompteur = 0;
                    if (this.txt_Reglage.Tag != null)
                        idReglageCompteur = (int)this.txt_Reglage.Tag;

                    int idProduit = 0;
                    if (laDetailDemande.LaDemande.PRODUIT  != null)
                        idProduit =laDetailDemande.LaDemande.FK_IDPRODUIT .Value ;

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
  
        CsCompteClient leClasseurClient = null;
        List<CsLclient> LstReglementClient = null;
        List<CsLclient> LstFactureClient = null;

        private void RetourneLeCompteClient(CsClientRechercher leClient)
        {
            try
            {
                AcceuilServiceClient service = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.RetourneLeCompteClientCompleted += (s, args) =>
                {
                    leClasseurClient = new CsCompteClient();
                    if (args != null && args.Cancelled)
                        return;

                    leClasseurClient = args.Result;
                    if (leClasseurClient != null)
                    {
                        decimal _totalDebit = 0;
                        decimal _totalCredit = 0;

                        _totalDebit = decimal.Parse(leClasseurClient.ToutLClient.Where(t => t.DC == SessionObject.Enumere.Debit).Sum(p => p.MONTANT).ToString());
                        this.tab4_txt_TotalDebit.Text = _totalDebit.ToString(SessionObject.FormatMontant);

                        _totalCredit = leClasseurClient.ToutLClient.Where(t => t.DC == SessionObject.Enumere.Credit).Sum(p => decimal.Parse(p.MONTANT.ToString()));
                        this.tab4_txt_TotalCredit.Text = _totalCredit.ToString(SessionObject.FormatMontant);
                        tab4_txt_balance.Text = (_totalDebit - _totalCredit).ToString(SessionObject.FormatMontant); ;

                        LstReglementClient = new List<CsLclient>();
                        LstFactureClient = new List<CsLclient>();
                        if (leClasseurClient.LstFacture != null)
                            LstFactureClient = leClasseurClient.LstFacture;
                        if (leClasseurClient.LstReglement != null)
                            LstReglementClient = leClasseurClient.LstReglement;
                        RemplirTypeAction(0);
                    }
                };
                service.RetourneLeCompteClientAsync(leClient.FK_IDCENTRE, leClient.CENTRE, leClient.CLIENT, leClient.ORDRE);
                service.CloseAsync();
            }
            catch (Exception ex)
            {
            }
        }
        private void RemplirTypeAction(int Index)
        {
            try
            {
                int caseSwitch = Index;
                switch (caseSwitch)
                {
                    case 0:
                        {
                            this.tab4_dataGrid1.Visibility = System.Windows.Visibility.Collapsed;
                            this.tab4_dataGrid2.Visibility = System.Windows.Visibility.Collapsed;
                            this.tab4_dataGrid3.Visibility = System.Windows.Visibility.Visible;
                            if (leClasseurClient != null)
                            {
                                //_LeClasseur.LeCompteClient.ToutLClient = new List<CsLclient>();

                                tab4_dataGrid3.ItemsSource = null;
                                List<CsLclient> _ToutLeCompteClient = ClasseMEthodeGenerique.RetourneListCopy<CsLclient>(leClasseurClient.ToutLClient.OrderBy(t => t.REFEM).ToList());
                                List<CsLclient> _ToutLeCompteClientReg = leClasseurClient.ToutLClient.Where(p => p.DC == "C").ToList();
                                RemplireOngletToutLeCompte(_ToutLeCompteClient.OrderBy(t => t.REFEM).ToList());
                            }
                        }
                        break;
                    case 1:
                        {
                            this.tab4_dataGrid1.Visibility = System.Windows.Visibility.Visible;
                            this.tab4_dataGrid2.Visibility = System.Windows.Visibility.Collapsed;
                            this.tab4_dataGrid3.Visibility = System.Windows.Visibility.Collapsed;
                            tab4_dataGrid1.ItemsSource = null;
                            RemplireOngletReglement(LstReglementClient.OrderBy(t => t.REFEM).ToList());
                        }
                        break;
                    case 2:
                        {
                            this.tab4_dataGrid1.Visibility = System.Windows.Visibility.Collapsed;
                            this.tab4_dataGrid2.Visibility = System.Windows.Visibility.Visible;
                            this.tab4_dataGrid3.Visibility = System.Windows.Visibility.Collapsed;
                            this.tab4_dataGrid2.ItemsSource = null;
                            RemplireOngletFacture(LstFactureClient.OrderBy(t => t.REFEM).ToList());
                        }
                        break;

                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        private void RemplireOngletToutLeCompte(List<CsLclient> _LeCompteClient)
        {
            try
            {
                tab4_dataGrid3.ItemsSource = null;
                tab4_dataGrid3.ItemsSource = FormateListe(_LeCompteClient);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        private List<CsLclient> FormateListe(List<CsLclient> _LeCompteClient)
        {
            List<CsLclient> _LstFactureFinal = new List<CsLclient>();
            if (_LeCompteClient != null && ((_LeCompteClient != null && _LeCompteClient.Count != 0)))
            {
                List<CsLclient> _LstFacture = _LeCompteClient.Where(p => p.DC == SessionObject.Enumere.Debit).ToList();
                List<CsLclient> _LstEncaissement = _LeCompteClient.Where(p => p.DC == SessionObject.Enumere.Credit).ToList();
                if (_LstFacture != null && _LstFacture.Count != 0)
                    foreach (var item in _LstFacture)
                    {
                        _LstFactureFinal.Add(item);
                        List<CsLclient> lstFacture = _LstEncaissement.Where(p => p.REFEM == item.REFEM && p.NDOC == item.NDOC).ToList();
                        if (lstFacture != null && lstFacture.Count != 0)
                            _LstFactureFinal.AddRange(TransLClient(lstFacture));
                    }
                else
                    _LstFactureFinal.AddRange(_LstEncaissement);
            }
            return _LstFactureFinal;
        }
        private List<CsLclient> TransLClient(List<CsLclient> _LeTranscaisse)
        {
            List<CsLclient> _LeReglt = new List<CsLclient>();
            foreach (var item in _LeTranscaisse)
            {
                item.REFEM = string.Empty;
                item.NDOC = string.Empty;
                item.ACQUIT = string.Empty;
                _LeReglt.Add(item);
            }
            return _LeReglt;
        }
        private void RemplireOngletFacture(List<CsLclient> _LesFacture)
        {
            try
            {
                tab4_dataGrid2.ItemsSource = null;
                tab4_dataGrid2.ItemsSource = _LesFacture.OrderByDescending(p => p.DENR);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        private void RemplireOngletReglement(List<CsLclient> _LesReglement)
        {
            try
            {

                var reglemntParModereg = (from p in _LesReglement
                                          group new { p } by new { p.ACQUIT, p.DTRANS, p.NOMCAISSIERE } into pResult
                                          select new
                                          {
                                              pResult.Key.ACQUIT,
                                              pResult.Key.NOMCAISSIERE,
                                              pResult.Key.DTRANS,
                                              MONTANT = (decimal?)pResult.Where(t => t.p.ACQUIT == pResult.Key.ACQUIT).Sum(o => o.p.MONTANT)
                                          });
                tab4_dataGrid1.ItemsSource = null;
                tab4_dataGrid1.ItemsSource = reglemntParModereg.OrderByDescending(p => p.DTRANS);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        private void tab4_cbo_Operation_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.tab4_cbo_Operation.SelectedIndex >= 0)
                RemplirTypeAction(this.tab4_cbo_Operation.SelectedIndex);
        }
        private void dtgCompteur_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (this.dtgCompteur.SelectedIndex >= 0)
                {
                    CsCanalisation _LeCompteurSelect = (CsCanalisation)this.dtgCompteur.SelectedItem;
                    RemplireCannalisationProduit(_LeCompteurSelect);
                    if (laDetailDemande.LstEvenement != null && laDetailDemande.LstEvenement.Count != 0)
                        RemplireOngletEvenement(laDetailDemande.LstEvenement.Where(p => p.PRODUIT == _LeCompteurSelect.PRODUIT && p.POINT == _LeCompteurSelect.POINT).ToList());

                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Galatee.Silverlight.Resources.Accueil.Langue.lbl_Menu);

            }
        }
        private void RemplireOngletEvenement(List<CsEvenement> _LstEvenement)
        {
            try
            {
                tab5_Stab2dataGrid2.ItemsSource = null;
                if (_LstEvenement != null && _LstEvenement.Count != 0)
                {
                    if (_LstEvenement != null && _LstEvenement.Count != 0)
                    {
                        if (_LstEvenement.First().PRODUIT == SessionObject.Enumere.ElectriciteMT)
                            _LstEvenement.ForEach(t => t.REGLAGECOMPTEUR = t.TYPECOMPTAGE);
                        tab5_Stab2dataGrid2.ItemsSource = _LstEvenement.OrderBy(t => t.NUMEVENEMENT);
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        private void RemplireCannalisationProduit(CsCanalisation _LaCannalisationSelect)
        {
            try
            {
                this.tab5_txt_NumCompteur.Text = string.IsNullOrEmpty(_LaCannalisationSelect.NUMERO) ? string.Empty : _LaCannalisationSelect.NUMERO;
                this.tab5_txt_AnnefabricCompteur.Text = string.IsNullOrEmpty(_LaCannalisationSelect.ANNEEFAB) ? string.Empty : _LaCannalisationSelect.ANNEEFAB;
                this.tab5_txt_LibelleTypeCompteur.Text = string.IsNullOrEmpty(_LaCannalisationSelect.LIBELLETYPECOMPTEUR) ? string.Empty : _LaCannalisationSelect.LIBELLETYPECOMPTEUR;
                this.tab5_txt_NumCompteur.Text = string.IsNullOrEmpty(_LaCannalisationSelect.NUMERO) ? string.Empty : _LaCannalisationSelect.NUMERO;
                this.tab5_txt_MarqueCompteur.Text = string.IsNullOrEmpty(_LaCannalisationSelect.LIBELLEMARQUE) ? string.Empty : _LaCannalisationSelect.LIBELLEMARQUE;


                if (_LaCannalisationSelect.PRODUIT != SessionObject.Enumere.ElectriciteMT)
                    this.tab5_txt_LibelleDiametreCompteur.Text = string.IsNullOrEmpty(_LaCannalisationSelect.LIBELLEREGLAGECOMPTEUR) ? string.Empty : _LaCannalisationSelect.LIBELLEREGLAGECOMPTEUR;
                else
                    this.tab5_txt_LibelleDiametreCompteur.Text = string.IsNullOrEmpty(_LaCannalisationSelect.LIBELLETYPECOMPTAGE) ? string.Empty : _LaCannalisationSelect.LIBELLETYPECOMPTAGE;

                this.tab5_txt_CoefDeMultiplication.Text = _LaCannalisationSelect.COEFLECT.ToString();
                if (_LaCannalisationSelect.COEFLECT == 0)
                    this.tab5_Chk_CoefMultiplication.IsChecked = false;
                else
                    tab5_Chk_CoefMultiplication.IsChecked = true;

                if (_LaCannalisationSelect.POSE != null)
                    this.tab5_txt_DateMiseEnService.Text = Convert.ToDateTime(_LaCannalisationSelect.POSE).ToShortDateString();

                if (_LaCannalisationSelect.DEPOSE != null)
                    this.tab5_txt_DateFinServce.Text = Convert.ToDateTime(_LaCannalisationSelect.DEPOSE).ToShortDateString();


                this.tab5_txt_LibelleDigit.Text = _LaCannalisationSelect.CADRAN.ToString();
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        private void RemplireOngletCanalisation(List<CsCanalisation> _LstCannalisation)
        {
            try
            {

                if (_LstCannalisation != null && _LstCannalisation.Count != 0)
                {
                    foreach (CsCanalisation item in _LstCannalisation)
                    {
                        if (item.DEPOSE != null)
                            item.LIBELLEETATCOMPTEUR = Galatee.Silverlight.Resources.Accueil.Langue.lbl_EtatCompteDepose;
                        else
                            item.LIBELLEETATCOMPTEUR = Galatee.Silverlight.Resources.Accueil.Langue.lbl_EtatCompteActif;

                    }
                    dtgCompteur.ItemsSource = _LstCannalisation;
                    dtgCompteur.SelectedItem = _LstCannalisation[0];

                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        List<CsTarif> lstDesTarif = new List<CsTarif>();
        private void ChargerTarifParCategorie(string Produit,string Categorie, List<CsTarif> lstTarifDiametre)
        {
            try
            {
                if (SessionObject.LstTarifCategorie.Count != 0)
                {
                  if (lstDesTarif.Count != 0)
                        lstDesTarif.Clear();
                    List<CsTarif> LstTarifCategorie = SessionObject.LstTarifCategorie.Where(p => p.PRODUIT == Produit && p.CATEGORIE == Categorie).ToList();
                    var lstTarif = from x in LstTarif
                                   join y in LstTarifCategorie on x.FK_IDTYPETARIF equals y.FK_IDTYPETARIF
                                   select x;
                    foreach (var item in lstTarif)
                        lstDesTarif.Add(item);

                    if (lstDesTarif.Count != 0 && lstDesTarif.Count == 1)
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
                        List<CsTarif> LstTarifCategorie = SessionObject.LstTarifCategorie.Where(p => p.PRODUIT == Produit && p.CATEGORIE == Categorie).ToList();
                        var lstTarif = from x in LstTarif
                                       join y in LstTarifCategorie on x.FK_IDTYPETARIF equals y.FK_IDTYPETARIF
                                       select x;
                        foreach (var item in lstTarif)
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
        private void ChargerTarif(CsDemande laDemande)
        {
            try
            {
                if (SessionObject.LstTarif.Count != 0)
                    LstTarif = SessionObject.LstTarif.Where(p => p.PRODUIT == laDemande.LaDemande.PRODUIT).ToList();
                else
                {
                    AcceuilServiceClient service = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                    service.ChargerTarifCompleted += (s, args) =>
                    {
                        if (args != null && args.Cancelled)
                            return;
                        SessionObject.LstTarif = args.Result;
                        LstTarif = SessionObject.LstTarif.Where(p => p.PRODUIT == laDemande.LaDemande.PRODUIT).ToList();

                    };
                    service.ChargerTarifAsync();
                    service.CloseAsync();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        private List<CsTarif> RetourneTarifFromPuissance(string Categorie, int Idpuissance, int idProduit, string reglage)
        {
            List<CsTarif> LstTarifReglage = SessionObject.LstTarifParReglageCompteur.Where(p => p.FK_IDPRODUIT == idProduit && p.REGLAGECOMPTEUR == reglage).ToList();
            List<CsTarif> LstTarifCategorie = SessionObject.LstTarifCategorie.Where(p => p.FK_IDPRODUIT == idProduit && p.CATEGORIE == Categorie).ToList();

            var lstTarif = from x in LstTarifReglage
                           join y in LstTarifCategorie on x.FK_IDTYPETARIF equals y.FK_IDTYPETARIF
                           select x;
            foreach (var item in lstTarif)
                lstDesTarif.Add(item);

            return lstDesTarif;
        }
        ObjELEMENTDEVIS _element = new ObjELEMENTDEVIS();
        public List<ObjELEMENTDEVIS> MyElements { get; set; }
        public CsCtax Taxe { get; set; }
        decimal taux = (decimal)0;
        void ChargerCoutDemande()
        {
            try
            {

                this.dataGridForniture.ItemsSource = null;
                this.Txt_TotalHt.Text = string.Empty;
                this.Txt_TotalTva.Text = string.Empty;
                this.Txt_TotalTtc.Text = string.Empty;

                this.dataGridForniture.Visibility = System.Windows.Visibility.Visible;
                if (laDetailDemande.EltDevis != null && laDetailDemande.EltDevis.Count != 0)
                    laDetailDemande.EltDevis.Clear();

                if (MyElements != null && MyElements.Count != 0)
                    MyElements.Clear();

                if (SessionObject.LstDesCoutDemande.Count != 0)
                {
                    decimal? pPuissanceSouscrite = 0;
                    pPuissanceSouscrite = laDetailDemande.LaDemande.PUISSANCESOUSCRITE;

                    string typedemande = laDetailDemande.LaDemande.TYPEDEMANDE;
                    if (laDetailDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.TransfertAbonnement)
                        typedemande = SessionObject.Enumere.BranchementAbonement;

                    LstDesCoutsDemande = SessionObject.LstDesCoutDemande.Where(p => p.TYPEDEMANDE == typedemande).ToList();
                    if (!string.IsNullOrEmpty(laDetailDemande.LaDemande.CENTRE))
                        LstDesCoutsDemande = LstDesCoutsDemande.Where(p => p.CENTRE == laDetailDemande.LaDemande.CENTRE || p.CENTRE == "000").ToList();

                    if (!string.IsNullOrEmpty(laDetailDemande.LaDemande.PRODUIT))
                        LstDesCoutsDemande = LstDesCoutsDemande.Where(p => p.PRODUIT == laDetailDemande.LaDemande.PRODUIT || p.PRODUIT == "00").ToList();

                    if (LstDesCoutsDemande.Count != 0)
                    {
                        string pDiametre = string.Empty;
                        if (laDetailDemande.LaDemande != null)
                            pDiametre = this.Btn_Reglage.Tag == null ? string.Empty : this.Btn_Reglage.Tag.ToString();
                        LstDesCoutsDemande = LstDesCoutsDemande.Where(p => p.REGLAGECOMPTEUR == pDiametre || string.IsNullOrEmpty(p.REGLAGECOMPTEUR)).ToList();

                        string pCategorie = string.Empty;
                        if (laDetailDemande.LeClient != null)
                            pCategorie = Cbo_Categorie.SelectedItem == null ? string.Empty : ((CsCategorieClient)Cbo_Categorie.SelectedItem).CODE ;
                        LstDesCoutsDemande = LstDesCoutsDemande.Where(p => p.CATEGORIE == pCategorie || string.IsNullOrEmpty(p.CATEGORIE)).ToList();
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
                                _element.DESIGNATION = _element.LIBELLE = item.LIBELLECOPER;
                                _element.PRIX = item.MONTANT != null ? (decimal)item.MONTANT : 0;
                                _element.COUTFOURNITURE = item.MONTANT != null ? (decimal)item.MONTANT : 0;

                                if (item.COPER == SessionObject.Enumere.CoperCAU &&
                                    laDetailDemande.LaDemande.PRODUIT == SessionObject.Enumere.ElectriciteMT)
                                    _element.QUANTITE = int.Parse(pPuissanceSouscrite.ToString());
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

        private void Chk_AvecMutation_Checked(object sender, RoutedEventArgs e)
        {
            this.dataGridForniture.ItemsSource = null;
            this.Txt_TotalHt.Text = string.Empty;
            this.Txt_TotalTva.Text = string.Empty;
            this.Txt_TotalTtc.Text = string.Empty;

            this.dataGridForniture.Visibility = System.Windows.Visibility.Visible;
            if (laDetailDemande.EltDevis != null && laDetailDemande.EltDevis.Count != 0)
                laDetailDemande.EltDevis.Clear();
            ChargerCoutDemande();
            this.tabC_Onglet.SelectedIndex = 6;
        }

        private void Chk_AvecMutation_Unchecked(object sender, RoutedEventArgs e)
        {
            this.dataGridForniture.Visibility = System.Windows.Visibility.Collapsed;
            this.dataGridForniture.ItemsSource = null;
            this.Txt_TotalHt.Text = string.Empty;
            this.Txt_TotalTva.Text = string.Empty;
            this.Txt_TotalTtc.Text = string.Empty;
            if (laDetailDemande.EltDevis != null && laDetailDemande.EltDevis.Count != 0)
                laDetailDemande.EltDevis.Clear();

        }

        private void RemplirTourneeExistanteParCentre(int pCentreId)
        {
            try
            {
                if (SessionObject.LstZone != null && SessionObject.LstZone.Count != 0)
                {
                    List<Galatee.Silverlight.ServiceAccueil.CsTournee> lstTournee = SessionObject.LstZone;
                    Cbo_Zone.SelectedValuePath = "PK_ID";
                    Cbo_Zone.DisplayMemberPath = "CODE";
                    Cbo_Zone.ItemsSource = lstTournee.Where(t => t.FK_IDCENTRE == pCentreId);
                    return;
                }
               
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private void ChargerPuissanceEtTarif(int idProduit, int? idPuissance, int? idCategorie, int? idReglageCompteur)
        {
            try
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
                        this.Txt_CodePussanceSoucrite.Text = string.Empty;
                        this.Txt_CodePussanceSoucrite.Tag = null;
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
                service.ChargerTypeTarifAsync(idProduit, idPuissance, idCategorie, idReglageCompteur);
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex ;
            }

        }

    }
}

