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
using Galatee.Silverlight.ServiceAccueil ;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading;
using Galatee.Silverlight.Resources.Devis;
using Galatee.Silverlight.Library;
using Galatee.Silverlight.Shared;
using System.Text.RegularExpressions;

namespace Galatee.Silverlight.Accueil
{
    public partial class FrmAbonReabonnement10 : ChildWindow
    {
        private UcImageScanne formScanne = null;
        private Object ModeExecution = null;
        private List<CsTournee> _listeDesTourneeExistant = null;
        private List<CsCommune> _listeDesCommuneExistant = null;
        private List<CsCommune> _listeDesCommuneExistantCentre = null;
        private List<CsReglageCompteur> _listeDesReglageCompteurExistant = null;
        private List<CsCentre> _listeDesCentreExistant = null;
        private  CsTdem _leTypeDemandeExistant = null;
        private string Tdem = null;
        private List<CsRues> _listeDesRuesExistant = null;
        private List<CsQuartier> _listeDesQuartierExistant = null;
        private List<ObjAPPAREILS> listAppareilsSelectionnes = null;
        bool isPreuveSelectionnee = false;
        private DataGrid _dataGrid = null;
        private List<CsUsage> lstusage = new List<CsUsage>();

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




        public FrmAbonReabonnement10()
        {
            InitializeComponent();
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

        public FrmAbonReabonnement10(string Tdem, string IsInit)
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

                this.Txt_Numdemande.Visibility = System.Windows.Visibility.Collapsed;
                this.lbl_numdem.Visibility = System.Windows.Visibility.Collapsed;

                txt_Reglage.Visibility = Visibility.Collapsed;
                Btn_Reglage.Visibility = Visibility.Collapsed;
                if (Tdem != SessionObject.Enumere.AbonnementSeul )
                Chk_IsPasMetre.Visibility = System.Windows.Visibility.Collapsed;
                tab_demande.Visibility = Visibility.Collapsed;
                this.tabItemPersoneAdministration.Visibility = System.Windows.Visibility.Collapsed;
                this.tabItemPersoneMoral.Visibility = System.Windows.Visibility.Collapsed;
                this.tab_proprio.Visibility = System.Windows.Visibility.Collapsed;
                ChargerPuissance();
                ChargerTypeDocument();
                ChargerCategorieClient_TypeClient();
                ChargerNatureClient_TypeClient();
                ChargerUsage_NatureClient();
                ChargerCategorieClient_Usage();
                RemplirStatutJuridique();
                RemplirListeDesTypeDemandeExistant();
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
                ChargerListDesSite();

                //Activation de la zone de recherche en fonction du type de demande
                ActivationEnFonctionDeTdem();

            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.txtDevis);
            }
        }

        public FrmAbonReabonnement10(int idDevis)
        {
            try
            {
                InitializeComponent();
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

                this.Txt_Numdemande.Visibility = System.Windows.Visibility.Collapsed;
                this.lbl_numdem.Visibility = System.Windows.Visibility.Collapsed;

                txt_Reglage.Visibility = Visibility.Collapsed;
                Btn_Reglage.Visibility = Visibility.Collapsed;
                if (Tdem != SessionObject.Enumere.AbonnementSeul)
                    Chk_IsPasMetre.Visibility = System.Windows.Visibility.Collapsed;
                tab_demande.Visibility = Visibility.Collapsed;
                this.tabItemPersoneAdministration.Visibility = System.Windows.Visibility.Collapsed;
                this.tabItemPersoneMoral.Visibility = System.Windows.Visibility.Collapsed;
                this.tab_proprio.Visibility = System.Windows.Visibility.Collapsed;
                ChargerPuissance();
                ChargerTypeDocument();
                ChargerCategorieClient_TypeClient();
                ChargerNatureClient_TypeClient();
                ChargerUsage_NatureClient();
                ChargerCategorieClient_Usage();
                RemplirStatutJuridique();
                RemplirListeDesTypeDemandeExistant();
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
                ChargerListDesSite();

                //Activation de la zone de recherche en fonction du type de demande
                ActivationEnFonctionDeTdem();

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
        private void ChargerPuissance()
        {
            try
            {
                if (SessionObject.LstPuissance != null && SessionObject.LstPuissance.Count != 0)
                    return;
                AcceuilServiceClient service = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                service.ChargerPuissanceSouscriteCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    SessionObject.LstPuissance = args.Result;
                };
                service.ChargerPuissanceSouscriteAsync();
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
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
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
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
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
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
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
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
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
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));

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

        private void ChargeDetailDEvis(int IdDemandeDevis)
        {

            Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient client = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
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
                    Tdem = laDetailDemande.LaDemande.TYPEDEMANDE;
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
        private bool  VerifieChampObligation()
        {
           try
           {
               bool ReturnValue = true;
               if (((CsTdem)this.txt_tdem.Tag).CODE == SessionObject.Enumere.BranchementAbonement ||
                   ((CsTdem)this.txt_tdem.Tag).CODE == SessionObject.Enumere.BranchementSimple ||
                   ((CsTdem)this.txt_tdem.Tag).CODE == SessionObject.Enumere.AbonnementSeul)
               {
                   #region InformationClient
                   if ((CsTypeClient)Cbo_Type_Client.SelectedItem == null )
                       throw new Exception("Sélectionnez le type de client");


                   if (((CsTypeClient)Cbo_Type_Client.SelectedItem).CODE.Trim() == "001".Trim())
                   {
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
                       this.tbControleClient.SelectedItem = tabItemPersoneMoral;

                       if (Cbo_TypePiecePersonnePhysique.SelectedItem != null)
                           throw new Exception("Selectionnez le type de pièce ");

                       if (string.IsNullOrEmpty(this.Txt_Capital.Text))
                           throw new Exception("Saisir le capitale de l'entreprise");

                       if (string.IsNullOrEmpty(this.Txt_IdentiteFiscale.Text))
                           throw new Exception("Saisir le numéro d'identité fiscale");


                       if (string.IsNullOrEmpty(this.Txt_NomSignataire.Text))
                           throw new Exception("Saisir le nom du signataire");

                       if (string.IsNullOrEmpty(this.Txt_PrenomSignataire.Text))
                           throw new Exception("Saisir le prenom du signataire");

                       if (Cbo_StatutJuridique.SelectedItem == null)
                           throw new Exception("Séléctionnez le statut juridique");

                   }
                   if (((CsTypeClient)Cbo_Type_Client.SelectedItem).CODE.Trim() == "003".Trim())
                   {
                       this.tbControleClient.SelectedItem = tabItemPersoneAdministration;

                       if (string.IsNullOrEmpty(this.Txt_PrenomMandataireAdministration .Text))
                           throw new Exception("Saisir le prénom du mandataire ");

                       if (string.IsNullOrEmpty(this.Txt_NomMandataireAdministration.Text))
                           throw new Exception("Saisir le nom du mandataire");

                       if (string.IsNullOrEmpty(this.Txt_PrenomSignataireAdministration.Text))
                           throw new Exception("Saisir la prenom du signataire ");

                       if (string.IsNullOrEmpty(this.Txt_NomSignataireAdministration.Text))
                           throw new Exception("Saisir du signataire ");

                       if (Cbo_StatutJuridique.SelectedItem != null)
                           throw new Exception("Séléctionnez le statut juridique");
                   }

                   if (this.Cbo_Type_Proprietaire.SelectedItem == null)
                       throw new Exception("Séléctionnez si le client est propriétaire ");
                   if (Cbo_Type_Proprietaire.SelectedItem != null && ((CsProprietaire)Cbo_Type_Proprietaire.SelectedItem).CODE == "1")
                   {
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

                   if (this.Cbo_TypeDePiece.SelectedItem == null)
                       throw new Exception("Séléctionnez la nationnalité ");

                   if (string.IsNullOrEmpty(this.Txt_Numeronina.Text))
                       throw new Exception("Saisir le numéro NINA ");

                   if (string.IsNullOrEmpty(this.txt_Telephone.Text))
                       throw new Exception("Saisir le numéro de téléphone ");
                   #endregion

                   #region information abonnement

                   if (string.IsNullOrEmpty(this.TxtCategorieClient.Text))
                       throw new Exception("Selectionnez la catégorie du client ");

                   if (Cbo_Usage.SelectedItem == null)
                       throw new Exception("Selectionnez l'usage ");


                   if (string.IsNullOrEmpty(this.Txt_CodeConso.Text))
                       throw new Exception("Selectionnez le code consommateur ");

                   if (string.IsNullOrEmpty(this.Txt_CodeConso.Text))
                       throw new Exception("Selectionnez le code consommateur ");

                   //if (((CsProduit)Cbo_Produit.SelectedItem).CODE != SessionObject.Enumere.ElectriciteMT)
                   //{
                   //    if (string.IsNullOrEmpty(this.txt_Reglage.Text ))
                   //        throw new Exception("Selectionnez le calibre ");
                   //}
                   #endregion
                   #region Adresse géographique
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
                this.Btn_Transmettre.IsEnabled = true ;
                this.Btn_Enregistrer.IsEnabled = true;
		        Message.ShowInformation(ex.Message ,"Accueil");
                return false;
	        }
        
        }


        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.Btn_Enregistrer.IsEnabled = false ;
                ValiderInitialisation(laDetailDemande, false);
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.txtDevis);
            }
        }
        private void ChargeDetailDemande(int IdDemandeDevis)
        {

            AcceuilServiceClient client = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
            client.GetDevisByNumIdDevisCompleted += (ssender, args) =>
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
                    List<CsUtilisateur> leUser = new List<CsUtilisateur>();
                    CsDemande leDamande = new CsDemande();
                    leDamande = args.Result;
                    if (leDamande.InfoDemande != null && leDamande.InfoDemande.UtilisateurEtapeSuivante != null)
                    {
                        foreach (CsUtilisateur item in leDamande.InfoDemande.UtilisateurEtapeSuivante)
                            leUser.Add(item);
                        Shared.ClasseMEthodeGenerique.NotifierMailDemande(leUser, "0001", leDamande.LaDemande.NUMDEM, leDamande.LaDemande.LIBELLETYPEDEMANDE);
                    }
                }
            };
            client.GetDevisByNumIdDevisAsync(IdDemandeDevis);
        }
        private void ValiderInitialisation(CsDemande demandedevis, bool IsTransmetre)
        {

            try
            {
                
                if (!VerifieChampObligation()) return;
                // Get Devis informations from screen
                if (demandedevis != null  )
                    demandedevis = GetDemandeDevisFromScreen(demandedevis, false);
                else
                {
                   
                    demandedevis = GetDemandeDevisFromScreen(demandedevis, false);
                }
                // Get DemandeDevis informations from screen
                if (demandedevis != null)
                {
                    demandedevis.LaDemande.ETAPEDEMANDE = (int)DataReferenceManager.EtapeDevis.Accueil;
                    if (IsTransmetre)
                        demandedevis.LaDemande.ETAPEDEMANDE = null;
                    demandedevis.LaDemande.MATRICULE = UserConnecte.matricule;
                    Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient client = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                    client.ValiderDemandeInitailisationCompleted += (ss, b) =>
                    {
                        if (b.Cancelled || b.Error != null)
                        {
                            string error = b.Error.Message;
                            Message.ShowError(error, Silverlight.Resources.Devis.Languages.txtDevis);
                            return;
                        }
                        string numedemande = string.Empty;
                        string Client = string.Empty;
                        if (IsTransmetre)
                        {
                            string Retour = b.Result;
                            string[] coupe = Retour.Split('.');
                            Shared.ClasseMEthodeGenerique.InitWOrkflow(coupe[0], demandedevis.LaDemande.FK_IDCENTRE, coupe[1], demandedevis.LaDemande.FK_IDTYPEDEMANDE);
                             numedemande = coupe[0];
                             Client = coupe[2];
                        }
                        List<CsDemandeBase> leDemandeAEditer = new List<CsDemandeBase>();
                        demandedevis.LaDemande.NOMCLIENT = demandedevis.LeClient.NOMABON;
                        demandedevis.LaDemande.LIBELLETYPEDEMANDE = txt_tdem.Text;
                        demandedevis.LaDemande.NUMDEM = numedemande;
                        demandedevis.LaDemande.CLIENT = Client;
                        //demandedevis.LaDemande.LIBELLEPRODUIT =((CsProduit) this.Cbo_Produit.SelectedItem).LIBELLE  ;

                        
                        leDemandeAEditer.Add(demandedevis.LaDemande);
                        Utility.ActionDirectOrientation<ServicePrintings.CsDemandeBase, CsDemandeBase>(leDemandeAEditer, null, SessionObject.CheminImpression, "AccuseRecption", "Accueil", true);
                        FermerFenetre();
                    };
                    client.ValiderDemandeInitailisationAsync(demandedevis);
                }
            }
            catch (Exception ex)
            {
                this.Btn_Transmettre.IsEnabled = true;
                Message.ShowError("Une erreur c'est produite a la validation ", "ValiderDemandeInitailisation");
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

        List<ServiceAccueil.CsCentre> lesCentre = new List<ServiceAccueil.CsCentre>();
        List<ServiceAccueil.CsSite> lesSite = new List<ServiceAccueil.CsSite>();
        List<int> lstIdCentre = new List<int>();
        void ChargerListDesSite()
        {
            try
            {

                if (SessionObject.LstCentre != null && SessionObject.LstCentre.Count != 0)
                {
                    lesCentre = Shared.ClasseMEthodeGenerique.RetourCentreByPerimetre(SessionObject.LstCentre, UserConnecte.listeProfilUser);
                    lesSite = Shared.ClasseMEthodeGenerique.RetourneSiteByCentre(lesCentre);
                    _listeDesCentreExistant = lesCentre;

                    foreach (ServiceAccueil.CsCentre item in lesCentre)
                        lstIdCentre.Add(item.PK_ID);
                    return;
                }
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
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

                            foreach (ServiceAccueil.CsCentre item in lesCentre)
                                lstIdCentre.Add(item.PK_ID);

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
        private void RemplirListeDesDiametresExistant()
        {
            try
            {
                if (SessionObject.LstReglageCompteur != null && SessionObject.LstReglageCompteur.Count != 0)
                {
                    _listeDesReglageCompteurExistant = SessionObject.LstReglageCompteur;
                    return;
                }
                AcceuilServiceClient service = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                service.ChargerReglageCompteurCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    SessionObject.LstReglageCompteur  = args.Result;
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
        private void RemplirListeDesTypeDemandeExistant()
        {
            try
            {
                if (SessionObject.LstTypeDemande != null && SessionObject.LstTypeDemande.Count != 0)
                {
                    if (!string.IsNullOrWhiteSpace(this.Tdem))
                    {
                        _leTypeDemandeExistant = SessionObject.LstTypeDemande.FirstOrDefault (t => t.CODE == this.Tdem);
                        txt_tdem.Text = _leTypeDemandeExistant.LIBELLE;
                        txt_tdem.Tag = _leTypeDemandeExistant;
                    }
                    LayoutRoot.Cursor = Cursors.Arrow;
                    return;
                }

                AcceuilServiceClient service1 = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                service1.RetourneOptionDemandeCompleted += (sr, res) =>
                {
                    if (res != null && res.Cancelled)
                        return;
                    SessionObject.LstTypeDemande = res.Result;
                    if (!string.IsNullOrWhiteSpace(this.Tdem))
                    {
                        _leTypeDemandeExistant = SessionObject.LstTypeDemande.FirstOrDefault(t => t.CODE == this.Tdem);
                        txt_tdem.Text = _leTypeDemandeExistant.LIBELLE;
                        txt_tdem.Tag = _leTypeDemandeExistant;
                    }
                    LayoutRoot.Cursor = Cursors.Arrow;
                };
                service1.RetourneOptionDemandeAsync();
                service1.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;

            }


        }

        //private void RemplirTournee(int pCentreId)
        //{
        //    List<CsTournee> lTourneeDuCentre = null;
        //    try
        //    {
        //        Cbo_Tournee.Items.Clear();
        //        if (_listeDesTourneeExistant != null &&
        //            _listeDesTourneeExistant.FirstOrDefault(p => p.FK_IDCENTRE == pCentreId) != null)
        //        {
        //            lTourneeDuCentre = _listeDesTourneeExistant.Where(q => q.FK_IDCENTRE == pCentreId).ToList();
        //        }
        //        if (lTourneeDuCentre != null && lTourneeDuCentre.Count > 0)
        //            foreach (var item in lTourneeDuCentre)
        //            {
        //                Cbo_Tournee.Items.Add(item);
        //            }

        //        Cbo_Tournee.SelectedValuePath = "PK_ID";
        //        Cbo_Tournee.DisplayMemberPath = "CODE";
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

      
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
                AcceuilServiceClient service = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
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

                    Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
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
                    Cbo_TypeDePiece.Items.Clear();
                    Cbo_TypeDePiece.ItemsSource = SessionObject.LstDesNationalites;
                    Cbo_TypeDePiece.SelectedValuePath = "PK_ID";
                    Cbo_TypeDePiece.DisplayMemberPath = "LIBELLE";

                    Cbo_Nationalite_Proprio.Items.Clear();
                    Cbo_Nationalite_Proprio.ItemsSource = SessionObject.LstDesNationalites;
                    Cbo_Nationalite_Proprio.SelectedValuePath = "PK_ID";
                    Cbo_Nationalite_Proprio.DisplayMemberPath = "LIBELLE";
                    return;
                }
                else
                {
                    Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                    service.RetourneNationnaliteCompleted += (s, args) =>
                    {
                        if (args != null && args.Cancelled)
                            return;
                        SessionObject.LstDesNationalites = args.Result;
                        Cbo_TypeDePiece.Items.Clear();
                        Cbo_TypeDePiece.ItemsSource = SessionObject.LstDesNationalites;
                        Cbo_TypeDePiece.SelectedValuePath = "PK_ID";
                        Cbo_TypeDePiece.DisplayMemberPath = "LIBELLE";


                        Cbo_Nationalite_Proprio.Items.Clear();
                        Cbo_Nationalite_Proprio.ItemsSource = SessionObject.LstDesNationalites;
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
                    Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
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
                    Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
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
                    Cbo_Regroupement.Items.Clear();
                    Cbo_Regroupement.ItemsSource = SessionObject.LstCodeRegroupement;
                    Cbo_Regroupement.SelectedValuePath = "PK_ID";
                    Cbo_Regroupement.DisplayMemberPath = "NOM";
                }
                else
                {
                    Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                    service.RetourneCodeRegroupementCompleted += (s, args) =>
                    {
                        if (args != null && args.Cancelled)
                            return;
                        SessionObject.LstCodeRegroupement = args.Result;
                        Cbo_Regroupement.Items.Clear();
                        Cbo_Regroupement.ItemsSource = SessionObject.LstCodeRegroupement;
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
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
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

        #region sylla le 22/09/2015


        private void RemplirUsage()
        {
            try
            {
                LayoutRoot.Cursor = Cursors.Wait;
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
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
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
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
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                service.ChargerLesTourneesCompleted  += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    SessionObject.LstZone = args.Result;
                    _listeDesTourneeExistant = SessionObject.LstZone;

                };
                service.ChargerLesTourneesAsync ();
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
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                service.ChargerCommuneAsync();
                service.ChargerCommuneCompleted  += (s, args) =>
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

                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
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
            this.txt_NumRue.Text = string.Empty;
            try
            {
                RueParDefaut = _listeDesRuesExistant.Where(q => q.CODE == DataReferenceManager.RueInconnue).ToList();
                if (RueParDefaut != null && RueParDefaut.Count > 0)
                    ListeRuesFiltrees.AddRange(RueParDefaut);
                ListeRuesFiltrees.AddRange(_listeDesRuesExistant.Where(q => q.FK_IDSECTEUR  == pIdSecteur && q.CODE != DataReferenceManager.RueInconnue).ToList());

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
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
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
            List<CsReglageCompteur > ListeDesDiametreFiltrees = null;
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
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
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

        private void UcInitialisation_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                LayoutRoot.Cursor = Cursors.Wait;
                if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Creation)
                {
                    //Txt_NumDevis.Text = string.Empty;
                }
                else if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Modification || (SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Consultation)
                {
                    this.Btn_Annuler.IsEnabled = true;
                    this.Btn_Annuler.Content = Languages.btnFermer;
                }
                LayoutRoot.Cursor = Cursors.Arrow;
            }
            catch (Exception ex)
            {
                LayoutRoot.Cursor = Cursors.Arrow;
                Message.ShowError(ex.Message, Languages.txtDevis);
            }
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
                    {
                        //Txt_NumDevis.Text = !string.IsNullOrEmpty(laDemande.LaDemande.NUMDEM) ? laDemande.LaDemande.NUMDEM : string.Empty;
                        //if (laDemande.LaDemande.FK_IDCENTRE != 0)
                        //if (laDemande.LaDemande.FK_IDPRODUIT != 0)
                        if (!string.IsNullOrEmpty(laDemande.LaDemande.TYPEDEMANDE))
                        {
                            _leTypeDemandeExistant = SessionObject.LstTypeDemande.FirstOrDefault(t => t.CODE == laDemande.LaDemande.TYPEDEMANDE);
                            txt_tdem.Text = _leTypeDemandeExistant.LIBELLE;
                            txt_tdem.Tag = _leTypeDemandeExistant;
                            txt_motif.Text =!string.IsNullOrWhiteSpace( laDemande.LaDemande.MOTIF)?laDemande.LaDemande.MOTIF:string.Empty;
                        }
                    }
                    #endregion
                    if (((CsTdem)txt_tdem.Tag).CODE == SessionObject.Enumere.AbonnementSeul)
                        return;

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

                       

                        if (laDemande.LeClient.FK_IDUSAGE != 0)
                        {
                            foreach (CsUsage typePiece in Cbo_Usage.Items)
                            {
                                if (typePiece.PK_ID == laDemande.LeClient.FK_IDUSAGE)
                                {
                                    Cbo_Usage.SelectedItem = typePiece;
                                    break;
                                }
                            }
                        }

                        if (laDemande.LeClient.FK_IDNATIONALITE != 0)
                        {
                            ServiceAccueil.CsNationalite laNation = SessionObject.LstDesNationalites.FirstOrDefault(t => t.PK_ID == laDemande.LeClient.FK_IDNATIONALITE);
                            this.Cbo_TypeDePiece.SelectedItem = laNation;
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
                        #region Sylla le 24/9/2015
                        
                        RemplirInfopersonnephysique(laDemande);
                        RemplirInfoSocietePrive(laDemande);
                        RemplirInfoAdmnistrationInstitut(laDemande);
                        RemplirInfoPropritaire(laDemande);
                        #endregion

                        Txt_NumFax .Text = !string.IsNullOrEmpty(laDemande.LeClient.FAX) ? laDemande.LeClient.FAX : string.Empty;
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
                            foreach ( CsQuartier quartier in Cbo_Quartier.Items)
                            {
                                if (quartier.PK_ID == laDemande.Ag.FK_IDQUARTIER)
                                {
                                    Cbo_Quartier.SelectedItem = quartier;
                                    break;
                                }
                            }
                        }
                        if (!string.IsNullOrWhiteSpace(laDemande.Ag.RUE))
                        {
                            foreach ( CsRues rues in Cbo_Rue.Items)
                            {
                                if (rues.CODE == laDemande.Ag.RUE)
                                {
                                    Cbo_Rue.SelectedItem = rues;
                                    break;
                                }
                            }
                        }
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
                    #region DocumentScanne
                    if (laDemande.ObjetScanne != null && laDemande.ObjetScanne.Count != 0)
                    {
                        isPreuveSelectionnee = true;
                        foreach (var item in laDemande.ObjetScanne)
                        {
                            LstPiece.Add(item);
                        }
                        dgListePiece.ItemsSource = this.LstPiece;
                    }
                    else
                    {
                        isPreuveSelectionnee = false;
                    }
                    #endregion
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
                    #region Abon
                    if (laDemande.Abonne != null)
                    {
                        bool? mynull = null;
                        rbtn_AugtPiss.IsChecked = laDemande.Abonne.ISAUGMENTATIONPUISSANCE != null ? laDemande.Abonne.ISAUGMENTATIONPUISSANCE : mynull;
                        rbtn_DiminPuiss.IsChecked = laDemande.Abonne.ISDIMINUTIONPUISSANCE != null ? laDemande.Abonne.ISDIMINUTIONPUISSANCE : mynull;
                        txt_ancienPuiss.Text = laDemande.Abonne.PUISSANCE != null ? laDemande.Abonne.PUISSANCE.ToString() : string.Empty;
                        //txt_nouvelPuiss.Text = laDemande.Abonne.NOUVELLEPUISSANCE != null ? laDemande.Abonne.NOUVELLEPUISSANCE.ToString() : string.Empty;
                    }

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
                Txt_NumFax .IsReadOnly = pValue;
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
                Cbo_TypeDePiece.IsEnabled = pValue;

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
                ActiverEnregistrerOuTransmettre();
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
                        lab_ListeAppareils.Visibility = Visibility.Collapsed;
                        Cbo_ListeAppareils.Visibility = Visibility.Collapsed;
                        Btn_ListeAppareils.Visibility = Visibility.Collapsed;

                        label21.Visibility = Visibility.Collapsed;
                        txt_Reglage.Visibility = Visibility.Collapsed;
                        Btn_Reglage.Visibility = Visibility.Collapsed;
                    }
                    else
                    {
                        if (SessionObject.LstTypeClient .Count != 0)
                          this.Cbo_Type_Client.SelectedItem = SessionObject.LstTypeClient.Where(t => t.CODE == "001").ToList();
                        //lbl_RefBranch.Visibility = Visibility.Collapsed;
                        lab_ListeAppareils.Visibility = Visibility.Visible;
                        Cbo_ListeAppareils.Visibility = Visibility.Visible;
                        Btn_ListeAppareils.Visibility = Visibility.Visible;
                        label21.Visibility = Visibility.Visible;
                        txt_Reglage.Visibility = Visibility.Visible;
                        Btn_Reglage.Visibility = Visibility.Visible;
                    }
                  
                     
                    ActiverEnregistrerOuTransmettre();
                //}
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
                     CsCommune commune = Cbo_Commune.SelectedItem as  CsCommune;
                    if (commune != null)
                    {
                        Cbo_Commune.SelectedItem = commune;
                        Cbo_Commune.Tag  = commune.PK_ID ;
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
                if (this.txt_Commune.Text.Length == SessionObject.Enumere.TailleCommune )
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
                ValiderInitialisation(laDetailDemande, true);
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
                    var quartier = Cbo_Quartier.SelectedItem as  CsQuartier;
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
                    CsQuartier  leQuartier = ListeQuartierFiltres.FirstOrDefault(t => t.CODE == this.txt_Quartier.Text);
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
                    var Secteur = Cbo_Rue.SelectedItem as  CsRues;
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
        private void ActiverEnregistrerOuTransmettre()
        {
            try
            {
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        

        private void Txt_NomClient_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                ActiverEnregistrerOuTransmettre();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.txtDevis);
            }
        }

  

        private void txtNumeroPiece_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                ActiverEnregistrerOuTransmettre();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.txtDevis);
            }
        }

        private void Cbo_Tournee_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                ActiverEnregistrerOuTransmettre();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.txtDevis);
            }
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
                ActiverEnregistrerOuTransmettre();
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
        private CsDemande GetDemandeDevisFromScreen(CsDemande pDemandeDevis, bool isTransmettre)
        {
            try
            {
                if (pDemandeDevis == null)
                {
                    pDemandeDevis = new CsDemande();
                    pDemandeDevis.LaDemande = new CsDemandeBase();
                    pDemandeDevis.Abonne = new CsAbon();
                    pDemandeDevis.Ag = new CsAg();
                    pDemandeDevis.Branchement = new CsBrt();
                    pDemandeDevis.LeClient = new CsClient();
                    pDemandeDevis.ObjetScanne = new List<ObjDOCUMENTSCANNE>();
                    pDemandeDevis.AppareilDevis = new List<ObjAPPAREILSDEVIS>();
                    pDemandeDevis.LaDemande.DATECREATION = DateTime.Now;
                    pDemandeDevis.LaDemande.USERCREATION = UserConnecte.matricule;
                    pDemandeDevis.LaDemande.FK_IDADMUTILISATEUR = UserConnecte.PK_ID;
                    //pDemandeDevis.LaDemande.NUMDEM = string.IsNullOrEmpty(this.Txt_NumDevis.Text) ? null : this.Txt_NumDevis.Text;
                }
                #region Demande
                if (((CsTdem)txt_tdem.Tag).CODE == SessionObject.Enumere.Resiliation ||
                    ((CsTdem)txt_tdem.Tag).CODE == SessionObject.Enumere.Reabonnement ||
                    ((CsTdem)txt_tdem.Tag).CODE == SessionObject.Enumere.AbonnementSeul  ||
                    ((CsTdem)txt_tdem.Tag).CODE == SessionObject.Enumere.ChangementCompteur   ||
                    ((CsTdem)txt_tdem.Tag).CODE == SessionObject.Enumere.AugmentationPuissance    ||
                    ((CsTdem)txt_tdem.Tag).CODE == SessionObject.Enumere.DimunitionPuissance )
                {
                    //if (pDemandeDevis.LaDemande == null) pDemandeDevis.LaDemande = new CsDemandeBase();
                    //pDemandeDevis.LaDemande.NUMDEM = Txt_NumDevis.Text;
                    if (pDemandeDevis.LaDemande.TYPEDEMANDE != SessionObject.Enumere.BranchementAbonement)
                        pDemandeDevis.LaDemande.CLIENT = string.IsNullOrEmpty(this.Txt_ReferenceClient.Text) ? string.Empty : this.Txt_ReferenceClient.Text;
                    pDemandeDevis.LaDemande.DATEMODIFICATION = DateTime.Now;
                    pDemandeDevis.LaDemande.MOTIF = txt_motif.Text;
                    pDemandeDevis.LaDemande.ISMETREAFAIRE = this.Chk_IsPasMetre.IsChecked == true ? false : true;
                    if (this.Chk_IsPasMetre.IsChecked == true)
                    {
                        if (laCanalisationAnc != null && !string.IsNullOrEmpty(laCanalisationAnc.CLIENT))
                            pDemandeDevis.LaDemande.REGLAGECOMPTEUR = laCanalisationAnc.REGLAGECOMPTEUR;
                        if (PuissanceAnc != 0)
                            pDemandeDevis.LaDemande.PUISSANCESOUSCRITE = PuissanceAnc;
                    }
                }
                //if (Cbo_Centre.SelectedItem != null)
                //{
                //    var csCentre = Cbo_Centre.SelectedItem as CsCentre;
                //    if (csCentre != null)
                //    {
                //        pDemandeDevis.LaDemande.FK_IDCENTRE = csCentre.PK_ID;
                //        pDemandeDevis.LaDemande.CENTRE = csCentre.CODE;
                //    }
                //}

                //if (Cbo_Produit.SelectedItem != null)
                //{
                //    var produit = Cbo_Produit.SelectedItem as CsProduit;
                //    if (produit != null)
                //    {
                //        pDemandeDevis.LaDemande.FK_IDPRODUIT = produit.PK_ID;
                //        pDemandeDevis.LaDemande.PRODUIT = produit.CODE;
                //    }
                //}

                if (txt_tdem.Tag != null)
                {
                    var typeDevis = (CsTdem)txt_tdem.Tag;
                    if (typeDevis != null)
                    {
                        pDemandeDevis.LaDemande.FK_IDTYPEDEMANDE = typeDevis.PK_ID;
                        pDemandeDevis.LaDemande.TYPEDEMANDE = typeDevis.CODE;
                    }
                }
        
                #endregion
                if (Tdem != SessionObject.Enumere.Resiliation &&
                    Tdem != SessionObject.Enumere.AugmentationPuissance &&
                    Tdem != SessionObject.Enumere.ChangementCompteur  &&
                    Tdem != SessionObject.Enumere.DimunitionPuissance)
                {
                    #region Client
                    if (pDemandeDevis.LeClient == null)
                        pDemandeDevis.LeClient = new CsClient();

                    if (Cbo_TypeDePiece.SelectedItem != null)
                    {
                        var NationnaliteClient = Cbo_TypeDePiece.SelectedItem as Galatee.Silverlight.ServiceAccueil.CsNationalite;
                        if (NationnaliteClient != null)
                        {
                            pDemandeDevis.LeClient.NATIONNALITE = NationnaliteClient.CODE;
                            pDemandeDevis.LeClient.FK_IDNATIONALITE = NationnaliteClient.PK_ID;
                        }
                    }

                    if (Cbo_Type_Proprietaire.SelectedItem != null)
                    {
                        var TypeProprietaire = Cbo_Type_Proprietaire.SelectedItem as CsProprietaire;
                        if (TypeProprietaire != null)
                        {
                            pDemandeDevis.LeClient.PROPRIO = TypeProprietaire.CODE;
                            pDemandeDevis.LeClient.FK_IDPROPRIETAIRE = TypeProprietaire.PK_ID;
                        }
                    }
                    #region Sylla 24/09/2015

                    if (Cbo_Type_Client.SelectedItem != null)
                    {
                        var TypeClient = Cbo_Type_Client.SelectedItem as CsTypeClient;
                        if (TypeClient != null)
                        {
                            //pDemandeDevis.LeClient.NATIONNALITE = TypeClient.CODE;
                            pDemandeDevis.LeClient.FK_TYPECLIENT = TypeClient.PK_ID;
                        }
                    }

                    if (Txt_usage.Tag != null )
                        pDemandeDevis.LeClient.FK_IDUSAGE = (int)Txt_usage.Tag;

                    #endregion
                    if (this.txt_Reglage.Tag != null )
                        pDemandeDevis.LaDemande.REGLAGECOMPTEUR = this.Btn_Reglage.Tag.ToString();

                    pDemandeDevis.LeClient.TELEPHONEFIXE = string.IsNullOrEmpty(this.txt_Telephone_Fixe.Text) ? null : this.txt_Telephone_Fixe.Text;
                    pDemandeDevis.LeClient.FAX = string.IsNullOrEmpty(this.Txt_NumFax.Text) ? null : this.Txt_NumFax.Text;
                    pDemandeDevis.LeClient.BOITEPOSTAL = string.IsNullOrEmpty(this.Txt_BoitePostale .Text) ? null : this.Txt_BoitePostale .Text;
                    pDemandeDevis.LeClient.TELEPHONE = string.IsNullOrEmpty(this.txt_Telephone.Text) ? null : this.txt_Telephone.Text;
                    pDemandeDevis.LeClient.NUMPROPRIETE = string.IsNullOrEmpty(this.txtPropriete.Text) ? null : this.txtPropriete.Text;
                    pDemandeDevis.LeClient.CENTRE = string.IsNullOrEmpty(pDemandeDevis.LaDemande.CENTRE) ? null : pDemandeDevis.LaDemande.CENTRE;
                    pDemandeDevis.LeClient.REFCLIENT = string.IsNullOrEmpty(pDemandeDevis.LaDemande.CLIENT) ? null : pDemandeDevis.LaDemande.CLIENT;

                    if (((CsTdem)txt_tdem.Tag).CODE == SessionObject.Enumere.BranchementAbonement ||
                            ((CsTdem)txt_tdem.Tag).CODE == SessionObject.Enumere.BranchementSimple)
                        this.txt_ordre.Text = "01";

                    pDemandeDevis.LeClient.ORDRE = this.txt_ordre.Text;
                    pDemandeDevis.LeClient.FK_IDCENTRE = pDemandeDevis.LaDemande.FK_IDCENTRE;
                    pDemandeDevis.LeClient.DATECREATION = DateTime.Now;
                    pDemandeDevis.LeClient.USERCREATION = UserConnecte.matricule;
                    //pDemandeDevis.LeClient.NUMDEM = string.IsNullOrEmpty(this.Txt_NumDevis.Text) ? null : this.Txt_NumDevis.Text;
                    pDemandeDevis.LeClient.ISFACTUREEMAIL = chk_Email.IsChecked.Value;
                    pDemandeDevis.LeClient.ISFACTURESMS = chk_SMS.IsChecked.Value;


                    if (!string.IsNullOrWhiteSpace(this.Txt_Email.Text))
                    {
                        if (IsEmail(this.Txt_Email.Text))
                        {
                            pDemandeDevis.LeClient.EMAIL = string.IsNullOrEmpty(this.Txt_Email.Text) ? null : this.Txt_Email.Text;
                        }
                        else
                        {
                            Message.Show("Veuillez saisi un email client correct", "Erreur");
                            return null;
                        }
                    }

                    pDemandeDevis.LeClient.ADRMAND1 = txtAdresse.Text;
                    if (this.Txt_CodeConso .Tag  != null)
                    {
                        pDemandeDevis.LeClient.CODECONSO = this.Txt_CodeConso.Text;
                        pDemandeDevis.LeClient.FK_IDCODECONSO = (int)this.Txt_CodeConso.Tag;
                    }
                    if (this.Txt_CodeRegroupement .Tag  != null)
                    {
                        pDemandeDevis.LeClient.REGROUPEMENT = this.Txt_CodeRegroupement.Text ;
                        pDemandeDevis.LeClient.FK_IDREGROUPEMENT = (int)this.Txt_CodeRegroupement.Tag;
                    }

                    if (TxtCategorieClient.Tag  != null)
                    {
                        pDemandeDevis.LeClient.FK_IDCATEGORIE =(int) TxtCategorieClient.Tag;
                        pDemandeDevis.LeClient.CATEGORIE = TxtCategorieClient.Text ;
                        if (pDemandeDevis.LeClient.CATEGORIE == SessionObject.Enumere.CategorieAgentEdm && string.IsNullOrEmpty(this.txt_MaticuleAgent.Text))
                            throw new Exception("Le matricule est obligatoire pour les agents EDM");
                        pDemandeDevis.LeClient.AGENTFACTURE = this.txt_MaticuleAgent.Text;
                    }
                    pDemandeDevis.LeClient.CODEIDENTIFICATIONNATIONALE = string.IsNullOrEmpty(this.Txt_Numeronina.Text) ? null : this.Txt_Numeronina.Text;
                    pDemandeDevis.LeClient.FK_IDRELANCE = 1;
                    pDemandeDevis.LeClient.CODERELANCE = "0";
                    pDemandeDevis.LeClient.MODEPAIEMENT = "0";
                    pDemandeDevis.LeClient.FK_IDMODEPAIEMENT = 1;

                    #endregion
                    #region AG
                    if (pDemandeDevis.Ag == null) pDemandeDevis.Ag = new CsAg();
                    if (Cbo_Commune.Tag  != null)
                    {
                        pDemandeDevis.Ag.FK_IDCOMMUNE =(int)Cbo_Commune.Tag;
                        pDemandeDevis.Ag.COMMUNE = this.txt_Commune .Text ;
                    }
                    pDemandeDevis.Ag.FK_IDTOURNEE = null;
                    if (Cbo_Quartier.Tag  != null)
                    {
                        pDemandeDevis.Ag.FK_IDQUARTIER =(int)Cbo_Quartier.Tag;
                        pDemandeDevis.Ag.QUARTIER =this.txt_Quartier.Text ;
                    }

                    if (Cbo_Rue.Tag  != null)
                    {
                        pDemandeDevis.Ag.FK_IDRUE = (int)Cbo_Rue.Tag;
                        pDemandeDevis.Ag.RUE = this.txt_NumRue .Text ;
                    }
                    if (Cbo_Secteur.Tag  != null)
                    {
                        pDemandeDevis.Ag.FK_IDSECTEUR = (int)Cbo_Secteur.Tag ;
                        pDemandeDevis.Ag.SECTEUR = txt_NumSecteur.Text ;
                    }
                    //pDemandeDevis.Ag.NOMP  = string.IsNullOrEmpty(this.Txt_NomClient.Text) ? null : this.Txt_NomClient.Text; 
                    pDemandeDevis.Ag.TELEPHONE = string.IsNullOrEmpty(this.txt_Telephone.Text) ? null : this.txt_Telephone.Text;
                    pDemandeDevis.Ag.ETAGE = string.IsNullOrEmpty(this.Txt_Etage.Text) ? null : this.Txt_Etage.Text;
                    pDemandeDevis.Ag.PORTE = string.IsNullOrEmpty(this.Txt_Porte.Text) ? null : this.Txt_Porte.Text;
                    pDemandeDevis.Ag.CENTRE = string.IsNullOrEmpty(pDemandeDevis.LaDemande.CENTRE) ? null : pDemandeDevis.LaDemande.CENTRE;
                    pDemandeDevis.Ag.CLIENT = string.IsNullOrEmpty(pDemandeDevis.LaDemande.CLIENT) ? null : pDemandeDevis.LaDemande.CLIENT;
                    //pDemandeDevis.Ag.NUMDEM = string.IsNullOrEmpty(this.Txt_NumDevis.Text) ? null : this.Txt_NumDevis.Text;
                    pDemandeDevis.Ag.FK_IDCENTRE = pDemandeDevis.LaDemande.FK_IDCENTRE;
                    pDemandeDevis.Ag.DATECREATION = DateTime.Now;
                    pDemandeDevis.Ag.USERCREATION = UserConnecte.matricule;
              
                    #endregion
                    #region Sylla 24/09/2015

                    if (Cbo_Type_Client.SelectedItem != null)
                    {
                        if (((CsTypeClient)Cbo_Type_Client.SelectedItem).CODE != null)
                        {
                            string codetypeclient = ((CsTypeClient)Cbo_Type_Client.SelectedItem).CODE;
                            switch (codetypeclient.Trim())
                            {
                                case "001":
                                    #region Personne Physique
                                    GetPersonnPhyqueData(pDemandeDevis);
                                    #endregion
                                    break;
                                case "002":
                                    if (GetSocietePriveData(pDemandeDevis) == null)
                                        return null;
                                    break;
                                case "003":
                                    GetAdministraionInstitutData(pDemandeDevis);
                                    break;
                                default:
                                    break;
                            }
                        }
                        else
                        {
                            Message.Show("Veuillez renseigné les informations spécifique au typr de client", "Demande");
                            return null;
                        }

                    }
                    else
                    {
                        Message.Show("Veuillez renseigné les informations spécifique au typr de client", "Demande");
                        return null;
                    }

                    GetSocieteProprietaire(pDemandeDevis);

                    #endregion
                    #region Appareil
                    List<ObjAPPAREILSDEVIS> lAppareilsDevis = new List<ObjAPPAREILSDEVIS>();
                    ObjAPPAREILSDEVIS AppareilDevis = null;
                    if (listAppareilsSelectionnes != null && listAppareilsSelectionnes.Count > 0)
                    {
                        foreach (ObjAPPAREILS appareil in listAppareilsSelectionnes)
                        {
                            AppareilDevis = new ObjAPPAREILSDEVIS();
                            AppareilDevis.PK_ID = appareil.PK_IDAPPAREILDEVIS;
                            AppareilDevis.FK_IDCODEAPPAREIL = appareil.PK_ID;
                            AppareilDevis.CODEAPPAREIL = appareil.CODEAPPAREIL;
                            if (pDemandeDevis != null && pDemandeDevis.LaDemande != null)
                            {
                                AppareilDevis.FK_IDDEVIS = pDemandeDevis.LaDemande.FK_IDTYPEDEMANDE;
                                AppareilDevis.NUMDEVIS = pDemandeDevis.LaDemande.NUMDEM;
                            }
                            AppareilDevis.NBRE = appareil.NOMBRE;
                            AppareilDevis.PUISSANCE = appareil.PUISSANCE;
                            AppareilDevis.DATECREATION = DateTime.Now;
                            AppareilDevis.USERCREATION = UserConnecte.matricule;
                            lAppareilsDevis.Add(AppareilDevis);
                        }
                        pDemandeDevis.AppareilDevis = lAppareilsDevis;
                    }
                    #endregion

                }
                #region Abon

                if (((CsTdem)txt_tdem.Tag).CODE == SessionObject.Enumere.AugmentationPuissance || ((CsTdem)txt_tdem.Tag).CODE == SessionObject.Enumere.DimunitionPuissance)
                {
                    if (pDemandeDevis.Abonne == null) pDemandeDevis.Abonne = new CsAbon();
                    if (laDetailDemande != null)
                        pDemandeDevis.Abonne = laDetailDemande.Abonne != null ? laDetailDemande.Abonne : new CsAbon();

                    //pDemandeDevis.Abonne.PK_ID = 0;


                    if ((Tdem == SessionObject.Enumere.AugmentationPuissance || Tdem == SessionObject.Enumere.DimunitionPuissance ) && Cbo_PuissanceSouscrite.SelectedItem != null)
                    {

                        pDemandeDevis.Abonne.ISAUGMENTATIONPUISSANCE = Tdem == SessionObject.Enumere.AugmentationPuissance ? true : false ;
                        pDemandeDevis.Abonne.ISDIMINUTIONPUISSANCE = Tdem == SessionObject.Enumere.DimunitionPuissance ? true : false ;

                        decimal NOUVELLEPUISSANCE = 0;
                        decimal ANCIENNEPUISSANCE = 0;

                        if (Cbo_PuissanceSouscrite.SelectedItem != null )
                        {
                            NOUVELLEPUISSANCE = ((CsPuissance)Cbo_PuissanceSouscrite.SelectedItem).VALEUR; 

                            decimal.TryParse(txt_ancienPuiss.Text, out ANCIENNEPUISSANCE);

                            if (pDemandeDevis.Abonne.ISAUGMENTATIONPUISSANCE)
                            {
                                if (NOUVELLEPUISSANCE < ANCIENNEPUISSANCE)
                                {
                                    Message.ShowError(" La nouvelle puissance doit etre supérieur à l'ancien ", "Information demande");
                                    return null;
                                }

                            }
                            if (pDemandeDevis.Abonne.ISDIMINUTIONPUISSANCE)
                            {
                                if (ANCIENNEPUISSANCE < NOUVELLEPUISSANCE)
                                {
                                    Message.ShowError(" L'ancien puissance doit etre supérieur à nouvel ", "Information demande");
                                    return null;
                                }
                            }
                            pDemandeDevis.Abonne.NOUVELLEPUISSANCE = NOUVELLEPUISSANCE;
                        }
                        else
                        {
                            Message.ShowError("Veuillez saisir une valeur numerique", "Information demande");
                            return null;
                        }
                    }
                    else
                    {
                        Message.ShowError("Veuillez correctement saisir les information de nouvelle puissance", "Information demande");
                        return null;
                    }


                }
                #endregion

                if (this.Cbo_Type_Proprietaire.SelectedItem != null)
                {
                    if (((CsProprietaire)this.Cbo_Type_Proprietaire.SelectedItem).CODE == SessionObject.Enumere.PROPRIETRAIRE)
                        pDemandeDevis.Ag.NOMP = pDemandeDevis.LeClient.NOMABON;
                    else
                        pDemandeDevis.Ag.NOMP = Txt_NomProprio_PersonePhysiq.Text + "  " + Txt_PrenomProprio_PersonePhysiq.Text;
                }


                #region Doc Scanne
                if (pDemandeDevis.ObjetScanne == null) pDemandeDevis.ObjetScanne = new List<ObjDOCUMENTSCANNE>();
                pDemandeDevis.ObjetScanne.AddRange(LstPiece);
                #endregion

                pDemandeDevis.LaDemande.ISNEW = true;
                Tdem = pDemandeDevis.LaDemande.TYPEDEMANDE;
                pDemandeDevis.LaDemande.ORDRE = string.IsNullOrEmpty(txt_ordre.Text) ? "01" : txt_ordre.Text;
                pDemandeDevis.LaDemande.DATECREATION = DateTime.Now;
                //if (pDemandeDevis.LaDemande.PRODUIT == SessionObject.Enumere.ElectriciteMT)
                //{
                //    pDemandeDevis.LaDemande.TYPEDEMANDE  = SessionObject.Enumere.BranchementAbonnementMt ;
                //    pDemandeDevis.LaDemande.FK_IDTYPEDEMANDE  = SessionObject.LstTypeDemande.FirstOrDefault(t=>t.CODE == SessionObject.Enumere.BranchementAbonnementMt ).PK_ID ;
                //}

                return pDemandeDevis;
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
            pDemandeDevis.AdministrationInstitut.NOMMANDATAIRE = Txt_NomMandataireAdministration.Text;
            pDemandeDevis.AdministrationInstitut.PRENOMMANDATAIRE = Txt_PrenomMandataireAdministration.Text;
            pDemandeDevis.AdministrationInstitut.RANGMANDATAIRE = Txt_RangMandataireAdministration.Text;
            pDemandeDevis.AdministrationInstitut.NOMSIGNATAIRE = Txt_NomSignataireAdministration.Text;
            pDemandeDevis.AdministrationInstitut.PRENOMSIGNATAIRE = Txt_PrenomSignataireAdministration.Text;
            pDemandeDevis.AdministrationInstitut.RANGSIGNATAIRE = Txt_RangSignataireAdministration.Text;

        }

        private void GetPersonnPhyqueData(CsDemande pDemandeDevis)
        {
            int? mynull = null;
            pDemandeDevis.PersonePhysique = new CsPersonePhysique();
            pDemandeDevis.PersonePhysique.PK_ID = Pk_IdPersPhys != 0 ? Pk_IdPersPhys : 0;
            pDemandeDevis.PersonePhysique.NOMABON = Txt_NomClient_PersonePhysiq.Text;
            pDemandeDevis.PersonePhysique.DATEFINVALIDITE =Convert.ToDateTime(dtp_DateValidite.Text );
            pDemandeDevis.PersonePhysique.DATENAISSANCE =Convert.ToDateTime(dtp_DateNaissance.Text) ;
            pDemandeDevis.PersonePhysique.NUMEROPIECEIDENTITE = txtNumeroPiece.Text;
            pDemandeDevis.PersonePhysique.FK_IDPIECEIDENTITE = Cbo_TypePiecePersonnePhysique.SelectedItem != null ? ((ObjPIECEIDENTITE)Cbo_TypePiecePersonnePhysique.SelectedItem).PK_ID : mynull;
            pDemandeDevis.LeClient .NOMABON = pDemandeDevis.PersonePhysique.NOMABON;

         
            
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

            pDemandeDevis.LeClient.NOMABON = pDemandeDevis.SocietePrives.NOMMANDATAIRE + " " + pDemandeDevis.SocietePrives.PRENOMMANDATAIRE;
         
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
            pDemandeDevis.InfoProprietaire_.FK_IDCLIENT  = pDemandeDevis.LeClient.PK_ID;
            pDemandeDevis.InfoProprietaire_.FK_IDPIECEIDENTITE = Cbo_TypePiecePersonnePhysiqueProprio.SelectedItem != null ? ((ObjPIECEIDENTITE)Cbo_TypePiecePersonnePhysiqueProprio.SelectedItem).PK_ID : Mynull;
            pDemandeDevis.InfoProprietaire_.NOM = Txt_NomProprio_PersonePhysiq.Text;
            pDemandeDevis.InfoProprietaire_.PRENOM = Txt_PrenomProprio_PersonePhysiq.Text;
            pDemandeDevis.InfoProprietaire_.TELEPHONEMOBILE = txt_Telephone_Proprio.Text;
            pDemandeDevis.InfoProprietaire_.NUMEROPIECEIDENTITE = txtNumeroPieceProprio.Text;

        
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
                {
                    RemplirListeAppareils(lappareils);
                    ActiverEnregistrerOuTransmettre();
                }
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
                //if (this.Cbo_Produit.SelectedItem != null)
                //{
                //    var UcListReglage = new Galatee.Silverlight.Accueil.UcListeReglageCompteur(_listeDesReglageCompteurExistant.Where(t=>t.FK_IDPRODUIT ==((CsProduit)this.Cbo_Produit.SelectedItem).PK_ID ).ToList());
                //    UcListReglage.Closed += new EventHandler(UcListReglage_Closed);
                //    this.IsEnabled = false;
                //    UcListReglage.Show();
                //}

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
                this.IsEnabled =true ;
                UcListeReglageCompteur ctrs = sender as UcListeReglageCompteur;
                if (ctrs.isOkClick)
                {
                    if (ctrs.leReglageSelect != null)
                    {
                        this.txt_Reglage.Text = ctrs.leReglageSelect.LIBELLE;
                        this.txt_Reglage.Tag = ctrs.leReglageSelect.PK_ID;
                        this.Btn_Reglage.Tag = ctrs.leReglageSelect.CODE;
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

                //CsProduit leProduitSelect = Cbo_Produit.SelectedItem as CsProduit;

                if (sommePuissance != 0)
                    intensite = sommePuissance / 220;
                //if (leProduitSelect != null)
                //{
                //    List<CsReglageCompteur> _listeDesDiametrePuissance = _listeDesReglageCompteurExistant.Where(p => p.REGLAGE  >= intensite && p.FK_IDPRODUIT == leProduitSelect.PK_ID).ToList();
                //    foreach (CsReglageCompteur item in _listeDesDiametrePuissance)
                //        item.IsRecommender = true;
                //}

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

        private void txtSite_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                ActiverEnregistrerOuTransmettre();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.txtDevis);
            }
        }

        private void txtCentre_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                ActiverEnregistrerOuTransmettre();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.txtDevis);
            }
        }

        private void txtProduit_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                ActiverEnregistrerOuTransmettre();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.txtDevis);
            }
        }

        private void Cbo_ListeAppareils_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                //ActiverEnregistrerOuTransmettre();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.txtDevis);
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

                    if (cat != null)
                    {
                        TxtCategorieClient.Text = cat.CODE ?? string.Empty;
                        this.TxtCategorieClient.Tag = cat.PK_ID;
                        if (cat.CODE == SessionObject.Enumere.CategorieAgentEdm)
                            this.txt_MaticuleAgent.IsReadOnly = false;
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
                        List<ServiceAccueil.CsSecteur> lstSecteur = SessionObject.LstSecteur.Where(t => t.FK_IDQUARTIER ==(int)this.Cbo_Quartier.Tag ).ToList();
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
                CsCodeConsomateur leCodeSaisi =  SessionObject.LstCodeConsomateur.FirstOrDefault(t=>t.CODE == this.Txt_CodeConso.Text);
                if (leCodeSaisi != null )
                    this.Cbo_CodeConso.SelectedItem = leCodeSaisi;
                else
                {
                    Message.ShowInformation("Le code saisie n'existe pas","Accueil");
                    return ;
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
                    if ((this.Cbo_Regroupement.SelectedItem != null && (CsRegCli)this.Cbo_Regroupement.SelectedItem != leRegroupement) || this.Cbo_Regroupement.SelectedItem == null)
                        this.Cbo_Regroupement.SelectedItem = leRegroupement;
                }
                else
                {
                    Message.ShowInformation("Le code saisie n'existe pas", "Accueil");
                    return;
                }
            }
        }

        private void Txt_PrenomClient_TextChanged(object sender, TextChangedEventArgs e)
        {
            ActiverEnregistrerOuTransmettre();
        }
        private void Cbo_TypeClient_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                CsTypeClient typeclient = ((CsTypeClient)Cbo_Type_Client.SelectedItem);
                ReloadCategorieClientFortypeclient(typeclient);
                switch (typeclient.CODE.Trim())
                {
                    case "001":
                        {
                            this.tabItemPersoneMoral.Visibility  =  System.Windows.Visibility.Collapsed ;
                            this.tabItemPersoneAdministration.Visibility = System.Windows.Visibility.Collapsed;
                            this.tabItemPersonnePhysique.Visibility = System.Windows.Visibility.Visible;
                            tbControleClient.SelectedItem = this.tabItemPersonnePhysique;
                            break;
                        }
                    case "002":
                        {
                            tabItemPersonnePhysique.Visibility = System.Windows.Visibility.Collapsed;
                            tabItemPersoneAdministration.Visibility = System.Windows.Visibility.Collapsed;
                            tabItemPersoneMoral.Visibility = System.Windows.Visibility.Visible ;
                            tbControleClient.SelectedItem = this.tabItemPersoneMoral;
                            break;
                        }
                    case "003":
                        {
                            tabItemPersonnePhysique.Visibility = System.Windows.Visibility.Collapsed;
                            tabItemPersoneMoral.Visibility = System.Windows.Visibility.Collapsed;
                            tabItemPersoneAdministration.Visibility = System.Windows.Visibility.Visible;
                            tbControleClient.SelectedItem = this.tabItemPersoneAdministration;
                            break;
                        }
                    default:
                        break;
                }
                ActiverEnregistrerOuTransmettre();
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
        private void formUcAdministration_Institut_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            DENOMINATION = ((UcAdministration_Institut)sender).Txt_Denomination.Text;
            NOMMANDATAIRE1 = ((UcAdministration_Institut)sender).Txt_NomMandataire.Text;
            NOMSIGNATAIRE1 = ((UcAdministration_Institut)sender).Txt_NomSignataire.Text;
            PRENOMMANDATAIRE1 = ((UcAdministration_Institut)sender).Txt_PrenomMandataire.Text;
            PRENOMSIGNATAIRE1 = ((UcAdministration_Institut)sender).Txt_PrenomSignataire.Text;
            RANGMANDATAIRE1 = ((UcAdministration_Institut)sender).Txt_RangMandataire.Text;
            RANGSIGNATAIRE1 = ((UcAdministration_Institut)sender).Txt_RangSignataire.Text;
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
                this.dtp_DateNaissance.Text  = laDemande.PersonePhysique.DATENAISSANCE != null ? laDemande.PersonePhysique.DATENAISSANCE.ToString() : DateTime.Now.ToShortDateString();
                this.dtp_DateValidite.Text  = laDemande.PersonePhysique.DATEFINVALIDITE != null ? laDemande.PersonePhysique.DATEFINVALIDITE.ToString() : DateTime.Now.ToShortDateString();
                this.Cbo_TypePiecePersonnePhysique.SelectedItem = ListeTYpePiece.FirstOrDefault(t => t.PK_ID == laDemande.PersonePhysique.FK_IDPIECEIDENTITE);
            }
            else
            {
                Cbo_TypePiecePersonnePhysiqueProprio.IsEnabled = true ;
                Cbo_TypePiecePersonnePhysiqueProprio.SelectedItem = SessionObject.LstTypeClient.First();
            
            }

        }
        private void RemplirInfoAdmnistrationInstitut(CsDemande laDemande)
        {
            if (laDemande.AdministrationInstitut != null)
            {
                Pk_IdAdministration = laDemande.AdministrationInstitut.PK_ID != null ? laDemande.AdministrationInstitut.PK_ID : 0;
                this.Txt_NomMandataireAdministration.Text = laDemande.AdministrationInstitut.NOMMANDATAIRE;
                this.Txt_PrenomMandataireAdministration.Text = laDemande.AdministrationInstitut.PRENOMMANDATAIRE;
                this.Txt_RangMandataireAdministration.Text = laDemande.AdministrationInstitut.RANGMANDATAIRE;
                this.Txt_NomSignataireAdministration.Text = laDemande.AdministrationInstitut.NOMSIGNATAIRE;
                this.Txt_PrenomSignataireAdministration.Text = laDemande.AdministrationInstitut.PRENOMSIGNATAIRE;
                this.Txt_RangSignataireAdministration.Text = laDemande.AdministrationInstitut.RANGSIGNATAIRE;

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
        void formUcSocietePrive_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //laDetailDemande.LeClient.PersonePhysique = new CsPersonePhysique();

            NUMEROREGISTRECOMMERCE = ((UcSocietePrive)sender).Txt_RegistreCommerce.Text;
            decimal capital = 0;
            decimal.TryParse(((UcSocietePrive)sender).Txt_Capital.Text, out capital);
            this.FK_IDSTATUTJURIQUE = (CsStatutJuridique)((UcSocietePrive)sender).Cbo_StatutJuridique.SelectedItem != null ? ((CsStatutJuridique)((UcSocietePrive)sender).Cbo_StatutJuridique.SelectedItem).PK_ID : 0;
            CAPITAL = capital;
            IDENTIFICATIONFISCALE = ((UcSocietePrive)sender).Txt_IdentiteFiscale.Text;
            DATECREATION = ((UcSocietePrive)sender).dtp_DateCreation.SelectedDate != null ? ((UcSocietePrive)sender).dtp_DateCreation.SelectedDate : DateTime.Now;
            SIEGE = ((UcSocietePrive)sender).Txt_Siege.Text;
            NOMMANDATAIRE = ((UcSocietePrive)sender).Txt_NomMandataire.Text;
            PRENOMMANDATAIRE = ((UcSocietePrive)sender).Txt_PrenomMandataire.Text;
            RANGMANDATAIRE = ((UcSocietePrive)sender).Txt_RangMandataire.Text;
            NOMSIGNATAIRE = ((UcSocietePrive)sender).Txt_NomSignataire.Text;
            PRENOMSIGNATAIRE = ((UcSocietePrive)sender).Txt_PrenomSignataire.Text;
            RANGSIGNATAIRE = ((UcSocietePrive)sender).Txt_RangMandataire.Text;
            //RANGMANDATAIRE = ((UcSocietePrive)sender).RANGMANDATAIRE;
            //datenaissance = ((UcSocietePrive)sender).datenaissance;
            //numeropiece = ((UcSocietePrive)sender).numeropiece;
            //typepiece = ((ObjPIECEIDENTITE)((UcSocietePrive)sender).Cbo_TypePiece.SelectedItem).PK_ID;
            //laDetailDemande.LeClient.PersonePhysique. = string.IsNullOrEmpty(((UcPersone_Physique)sender).txtNumeroPiece.Text) ? null : ((UcPersone_Physique)sender).txtNumeroPiece.Text;
            //laDetailDemande.LeClient.PersonePhysique.NUMEROPIECEIDENTITE = string.IsNullOrEmpty(((UcPersone_Physique)sender).txtNumeroPiece.Text) ? null : ((UcPersone_Physique)sender).txtNumeroPiece.Text; 


        }


        private void chk_Email_Checked(object sender, RoutedEventArgs e)
        {
            if (!chk_Email.IsChecked.Value)
            {
                Txt_Email.Text = string.Empty;
            }
            Txt_Email.IsEnabled = chk_Email.IsChecked.Value;
            ActiverEnregistrerOuTransmettre();
        }
        private void chk_SMS_Checked(object sender, RoutedEventArgs e)
        {
            if (!chk_SMS.IsChecked.Value)
            {
                txt_Telephone.Text = string.Empty;
            }
            txt_Telephone.IsEnabled = chk_SMS.IsChecked.Value;
            ActiverEnregistrerOuTransmettre();
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
            ActiverEnregistrerOuTransmettre();
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
            ActiverEnregistrerOuTransmettre();
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

        private void txt_Telephone_TextChanged(object sender, TextChangedEventArgs e)
        {
 
            ActiverEnregistrerOuTransmettre();
        }

        private void txt_Telephone_Fixe_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txt_Telephone_Fixe.Text))
            {
                ActiverEnregistrerOuTransmettre();
            }
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
                if ( typeproprio.CODE == SessionObject.Enumere.LOCATAIRE )
                {
                    tab_proprio.Visibility = Visibility.Visible;
                    PropietaireWindows(System.Windows.Visibility.Visible);
                    this.tbControleClient.SelectedItem = tab_proprio;
                }
                else
                {
                    tab_proprio.Visibility = Visibility.Collapsed;
                    PropietaireWindows(System.Windows.Visibility.Visible);
                    if (((CsTypeClient)Cbo_Type_Client.SelectedItem).CODE.Trim() == "001".Trim())
                        tbControleClient.SelectedItem = this.tabItemPersonnePhysique;

                     if (((CsTypeClient)Cbo_Type_Client.SelectedItem).CODE.Trim() == "002".Trim())
                        tbControleClient.SelectedItem = this.tabItemPersoneMoral;

                     if (((CsTypeClient)Cbo_Type_Client.SelectedItem).CODE.Trim() == "003".Trim())
                        tbControleClient.SelectedItem = this.tabItemPersoneAdministration;

                }
                ActiverEnregistrerOuTransmettre();
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

        private void dtp_DateNaissance_SelectedDateChanged_1(object sender, SelectionChangedEventArgs e)
        {

            ActiverEnregistrerOuTransmettre();
        }

        private void Cbo_TypePiecePersonnePhysique_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            ActiverEnregistrerOuTransmettre();
        }

        private void dtp_finvalidation_SelectedDateChanged_1(object sender, SelectionChangedEventArgs e)
        {
            ActiverEnregistrerOuTransmettre();
        }

        private void Txt_RegistreCommerce_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            ActiverEnregistrerOuTransmettre();
        }

        private void Txt_IdentiteFiscale_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            ActiverEnregistrerOuTransmettre();
        }

        private void Txt_Siege_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            ActiverEnregistrerOuTransmettre();
        }

        private void Txt_NomMandataire_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            ActiverEnregistrerOuTransmettre();
        }

        private void Txt_PrenomMandataire_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            ActiverEnregistrerOuTransmettre();
        }

        private void Cbo_StatutJuridique_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            ActiverEnregistrerOuTransmettre();
        }

        private void dtp_DateCreation_SelectedDateChanged_1(object sender, SelectionChangedEventArgs e)
        {
            ActiverEnregistrerOuTransmettre();
        }

        private void Txt_RangSignataire_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            ActiverEnregistrerOuTransmettre();
        }

        private void Txt_PrenomSignataire_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            ActiverEnregistrerOuTransmettre();
        }

        private void Txt_NomSignataire_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            ActiverEnregistrerOuTransmettre();
        }

        private void Txt_RangMandataire_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            ActiverEnregistrerOuTransmettre();
        }

        private void Txt_Denomination_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            ActiverEnregistrerOuTransmettre();
        }

        private void Txt_PrenomMandataireAd_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            ActiverEnregistrerOuTransmettre();
        }

        private void Txt_NomMandataireAd_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            ActiverEnregistrerOuTransmettre();
        }

        private void Txt_RangSignataireAd_TextChanged_1(object sender, TextChangedEventArgs e)
        {

        }

        private void Txt_PrenomSignataireAd_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            ActiverEnregistrerOuTransmettre();
        }

        private void Txt_NomSignataireAd_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            ActiverEnregistrerOuTransmettre();
        }

        private void Txt_RangMandataireAd_TextChanged_1(object sender, TextChangedEventArgs e)
        {

        }

        private void Txt_Numeronina_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            ActiverEnregistrerOuTransmettre();
        }

        private void Txt_Email_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            ActiverEnregistrerOuTransmettre();
        }

        private void Txt_NomClient_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            try
            {
                ActiverEnregistrerOuTransmettre();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.txtDevis);
            }
        }

        private void cbo_typedoc_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Cbo_TypeDePiece_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Btn_Transmettre_Click(object sender, RoutedEventArgs e)
        {
            this.Btn_Transmettre.IsEnabled = false;
            ValiderInitialisation(laDetailDemande, true );

        }

        private void dtp_DateValidite_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!Shared.ClasseMEthodeGenerique.IsDateValide(dtp_DateValidite.Text) && dtp_DateValidite.Text.Length == SessionObject.Enumere.TailleDate)
            {
                Message.ShowError("La date n'est pas valide","Accueil");
                this.dtp_DateValidite.Focus();
            }

        }

        private void dtp_DateNaissance_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!Shared.ClasseMEthodeGenerique.IsDateValide(dtp_DateNaissance.Text) && dtp_DateNaissance.Text.Length == SessionObject.Enumere.TailleDate )
            {
                Message.ShowError("La date n'est pas valide", "Accueil");
                this.dtp_DateNaissance.Focus();
            }
        }

        private void btn_tarif_Click(object sender, RoutedEventArgs e)
        {
            //int idCentre = 0;
            //if (Cbo_Centre.SelectedItem != null)
            //    idCentre = (Cbo_Centre.SelectedItem as CsCentre).PK_ID;

            int idCategorie = 0;
            if (Cbo_Categorie .SelectedItem != null)
                idCategorie = (Cbo_Categorie.SelectedItem as CsCategorieClient ).PK_ID;

            int idReglageCompteur = 0;
            if (this.txt_Reglage.Tag  != null)
                idReglageCompteur = (int)this.txt_Reglage.Tag;

            //int idProduit = 0;
            //if (Cbo_Produit.SelectedItem != null)
            //    idProduit = (Cbo_Produit.SelectedItem as CsProduit).PK_ID;

            //ChargerTarifClient(idCentre, idCategorie, idReglageCompteur, null, "0", idProduit);

        }

        private void ChargerTarifClient(int idcentre, int idcategorie, int idreglageCompteur, int? idtypecomptage, string propriotaire, int idproduit)
        {
            try
            {
                AcceuilServiceClient client = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
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

        CsCanalisation laCanalisationAnc = new CsCanalisation();
        decimal?  PuissanceAnc = 0;
        private void RetourneCanalisationResilier(CsAbon leAbonnement)
        {

            AcceuilServiceClient client = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
            client.RetourneCanalisationResilierCompleted += (ssender, args) =>
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
            };
            client.RetourneCanalisationResilierAsync(leAbonnement.FK_IDCENTRE, leAbonnement.CENTRE, leAbonnement.CLIENT, leAbonnement.PRODUIT);
        }
    
        private void btn_RechercheClient_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //prgBar.Visibility = System.Windows.Visibility.Visible;
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
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
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
                //prgBar.Visibility = System.Windows.Visibility.Collapsed;
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
                    AcceuilServiceClient service = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                    service.RetourneDemandeClientTypeCompleted += (s, args) =>
                    {
                        if (args != null && args.Cancelled)
                            return;
                        if (args.Result != null)
                        {
                            if (args.Result.DATEFIN == null && args.Result.ISSUPPRIME != true)
                            {
                                Message.ShowInformation("Il existe une demande numero " + args.Result.NUMDEM + " sur ce client", "Accueil");
                                //prgBar.Visibility = System.Windows.Visibility.Collapsed;
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
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient client = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                client.GeDetailByFromClientCompleted += (ssender, args) =>
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

                        laDetailDemande = new CsDemande();
                        laDetailDemande = args.Result;
                        this.txtSite.Text = string.IsNullOrEmpty(laDetailDemande.LeClient.LIBELLESITE) ? string.Empty : laDetailDemande.LeClient.LIBELLESITE;
                        this.txtCentre.Text = string.IsNullOrEmpty(laDetailDemande.LeClient.LIBELLECENTRE) ? string.Empty : laDetailDemande.LeClient.LIBELLECENTRE;
                         
                        if (laDetailDemande.Abonne != null )
                            this.txt_Produit.Text = string.IsNullOrEmpty(laDetailDemande.Abonne.LIBELLEPRODUIT) ? string.Empty : laDetailDemande.Abonne.LIBELLEPRODUIT;
                         
                        if (laDetailDemande.LeClient != null && laDetailDemande.PersonePhysique == null &&
                            laDetailDemande.SocietePrives == null &&
                            laDetailDemande.AdministrationInstitut == null)
                        {
                            this.Txt_NomClient_PersonePhysiq.Text = laDetailDemande.LeClient.NOMABON != null ? laDetailDemande.LeClient.NOMABON : string.Empty;
                            this.txtNumeroPiece.Text = laDetailDemande.LeClient.NUMEROPIECEIDENTITE != null ? laDetailDemande.LeClient.NUMEROPIECEIDENTITE : string.Empty;
                        }

                        RenseignerChampsSurLeControl(laDetailDemande);
                        if (((CsTdem)txt_tdem.Tag).CODE == SessionObject.Enumere.AbonnementSeul)
                            this.txt_ordre.Text = (int.Parse(laDetailDemande.Abonne.ORDRE) + 1).ToString("00");
                        else
                            this.txt_ordre.Text = laDetailDemande.Abonne.ORDRE;

                        if (((CsTdem)txt_tdem.Tag).CODE == SessionObject.Enumere.Reabonnement ||
                            ((CsTdem)txt_tdem.Tag).CODE == SessionObject.Enumere.AbonnementSeul)
                        {
                            RetourneCanalisationResilier(laDetailDemande.Abonne);
                            PuissanceAnc = laDetailDemande.Abonne.PUISSANCE;
                            laDetailDemande.Abonne = null;
                            laDetailDemande.LstCanalistion = null;
                        }
                        desableProgressBar();
                    }
                    LayoutRoot.Cursor = Cursors.Arrow;
                };
                client.GeDetailByFromClientAsync(leclient);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

    }
}

