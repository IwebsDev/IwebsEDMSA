using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace Galatee.Structure
{
    public static class Enumere
    {

        #region  PARAMETRAGE

        public static string CodeFacultatif = "F";
        public static string LibFacultatif = "Facultatif";

        public static string CodeInterdit = "I";
        public static string LibInterdit = "Interdit";

        public static string CodeObligatoire = "O";
        public static string LibObligatoire = "Obligatoire";

        public static string CodeNormale = "1";
        public static string LibNormale = "Normale";

        public static string CodeForfait = "2";
        public static string LibForfait = "Forfait";

        public static string CodeBloqueeSansRegularisation = "3";
        public static string LibBloqueeSansRegularisation = "BloqueeSansRegularisation";

        public static string CodeSansTarifUnitaire = "4";
        public static string LibSansTarifUnitaire = "SansTarifUnitaire";

        public static string CodeSansTarifAnnuel = "5";
        public static string LibSansTarifAnnuel = "SansTarifAnnuel";

        public static string CodeForfaitSansRegularisation = "6";
        public static string LibForfaitSansRegularisation = "ForfaitSansRegularisation";

        public static string CodeBloqueeAvecRegularisation = "7";
        public static string LibBloqueeAvecRegularisation = "BloqueeAvecRegularisation";

        public static string CodeEstimeeAvecRegularisation = "8";
        public static string LibEstimeeAvecRegularisation = "EstimeeAvecRegularisation";

        public static string CodeEstimeeSansRegularisation = "9";
        public static string LibEstimeeSansRegularisation = "EstimeeSansRegularisation";
        public static string EvenementsValeurParDefaut = "35";
        //public static string FiltrerMenuPourGestionEtapeDevis = "MODULE = 'Devis' and MAINMENUID IS NOT NULL AND FormName IS NOT NULL AND FormName <> ' ' AND CONVERT(VARCHAR(50),MENUID) LIKE '%17%'";
        #endregion

        #region ADMINISTRATION
        public static byte UserAcitveAccount = 1;
        public static byte UserInAcitveAccount = 2;
        public static byte UserLockeAcitveAccount = 3;
        public static byte UserLockeSessionAccount = 4;

        public static byte ActionSurCentre = 1;
        public static byte ActionSurSite = 2;
        public static byte ActionSurTousSite = 3;

        public enum PerimetreAction
        {
            Centre = 1,
            DirectionDeRattachement = 2,
            Global = 3
        }

        #endregion

        #region INTERFACE MODULE
        ///// valeurs pour les statuts du mois comptable //////
        public static string NON_CLOTURE = "0";
        public static string CLOTURE_PROVISOIRE = "1";
        public static string CLOTURE_DEFINITIF = "2";
        public static string CLOTURE_PURGE = "3";
        public static string FIN_CLOTURE = "4";
        public enum ORIGINE_LIGNE
        {
            FACTURE_NORMALE = 0,
            DEMANDE,
            SAISIE_DE_MASSE
        }

        public static int NBCOMPTEANNEXE = 6;
        public static int CPTstatic4 = 4;

        // determiner l'origine de la comptabilisation ( facturation ou encaissement ou guichet)
        public static int FACTURATION = 0;
        public static int ATURES = 1;
        public static int ENCAISSEMENT = 2;
        public static int GUICHET = 3;

        // pour les sous menus du menu comptabilisation de facture
        public static int NORMALE = 300;
        public static int ANNULE = 301;
        public static int ISOLE = 302;
        public static int RESILIE = 303;
        public static int AVOIR = 304;
        public static int AJUSTE = 305;

        ////// Valeurs pour le paramètre 139 de la table TA-58 //////
        public static int GESTIONMANUELLE = 0;// Gestion manuelle du mois comptable
        public static int GESTIONAUTO = 1; // Gestion automatique du mois comptable
        public static int GESTIONCLOTUREMANUELLE = 2;  // Gestion manuelle du mois comptable en fonction de la date du jour
        public static int GESTIONCLOTURESTRICTE = 3; // Gestion stricte du mois comptable en fonction de la date du jour

        // definition de l'enumeraire TypeExport de fichier
        public static int SATTI = 1;
        public static int AGRESSO = 2;
        public static int ORACLEAPP = 3;
        public static int SIZAWATER = 4;
        #endregion
        #region Nombre de point de fourniture
        public static int NombrePointFournitureMT = 6;
        public static int NombrePointFournitureBT = 1;
        #endregion
        public static int Pk_Id_DernierDemandeInserer;
        public static string ACTION_CONSTITUTION = "000010";
        public static string ACTION_CONFIRMATION_ENQUETE = "000060";
        public static string ACTION_SAISIE_INDEX = "000030";
        public static string ACTION_CALCUL_FACTURE = "000070";
        public enum EtapeBrtAbonSanExt
        {

            Liaison = 4,
            Programation = 5,
            SortieMat = 6,
            ValidationMAt = 7,
            CrTravaux = 8
        }
        public enum STATUSDEMANDE
        {
            Initiee = 1,
            EnAttenteValidation = 2,
            Suspendue = 3,
            Annulee = 4,
            Rejetee = 5,
            Terminee = 6
        }
   
        public static int TRANSMETTRE = 1;
        public static int REJETER = 2;
        public static int ANNULER = 3;
        public static int SUSPENDRE = 4;



        public static string INITIAL = "NEW";
        public static string CODEINTERFACE = "2023";
        public static string NEGATIVE = "-1";
        public static string PROVENACE = "GALATEE";
        public static string NO = "NO";
        public static string ZERO = "0";
        public static string ALPHA = "A";
        public static string DEVISCOMPTA = "XOF";

        public static string CompteurAffecte = "Affecté";
        public static string CompteurLie = "Lié";
        public static string CompteurLivre = "Livré";
        public static string CompteurRecu = "Réçu";


        #region Code produit
        public static string Eau = "01";
        public static string Electricite = "03";
        public static string ElectriciteMT = "04";
        public static string Prepaye = "05";

        #endregion

        #region Code module
        public static string Accueil = "Counter ";
        public static string Caisse = "Cash";

        #endregion

        #region Orientation
        public static string Debit = "D";
        public static string Credit = "C";

        #endregion

        #region StatuDemande
        public static string StatusDemandeInitier = "1";
        public static string StatusDemandeValider = "2";
        public static string StatusDemandeRejeter = "3";
        public static string StatusDemandeRetirer = "4";

        #endregion

        public static string CoperTimbre = "100";
        public static string EtatServerEnLigne = "Online";
        public static List<CsChaineConnexion> LstChaineConnexion;
        public static int CPTCONST4 = 4;
        public static string ConnexionGALADB = "galadbEntities";
        public static string ConnexionABO07 = "ABO07Entities";
        public static string CompteurNonLu = "##";
        public static string MobileActionParametrage = "PARAM_MOBILE";
        public static string MobileActionHabilitation = "HABIL_MOBILE";
        public static string MobileActionChargementRI = "IMPORT_RI_MOBILE";
        public static string CompteurChanged = "++";
        public static int DernierHistoriqueEvent = 12;
        public static int EtatEvenementPurge = 99;
        public static int moduleParametrageId = 1;
        public static int moduleDevisId = 11;
        public static int moduleRecouvrement = 6;
        public static int NumeroAcquisPosition = 9;
        public static int NumeroAcquisNAFPosition = 6;

        
        // ATTENTION, l'ordre des énumérés importe dans leur utilisation
        // il faut impérativement créer le votre à la suite.
        // PAS AU DEBUT, NI AU MILIEU, SEULEMENT A LA SUITE DU DERNIER
        public enum EtatLogin1
        {
            Valide,
            Inconnu,
            MotDePasseLoginIncompatible,
            ProblemeDeValidation
        }

        [DataContract]
        public enum DataBase
        {
            [EnumMember]
            Galadb,
            [EnumMember]
            Abo07
        }

        public enum MonthEnum
        {
            January = 1,
            February,
            March,
            April,
            May,
            June,
            July,
            August,
            September,
            October,
            November,
            December
        }

        public static string FactureIsoleIndex = "00002";
        public static string FactureResiliationIndex = "00001";
        public static string FactureAnnulatinIndex = "00003";

        public enum NumTable
        {
            Category = 12,
            MtTable = 58,
            service = 17    /* LKK le 23/08/2011*/
        }

        public enum EnumCompteur
        {
            Monophase = 1,
            Dial = 2,
            Triphase = 3,
            Four = 4
        }

        #region StatusScelle
        public static int StatusScelleAbîmé = 0;
        public static int StatusScelleRompu = 1;
        public static int StatusScelleRemis = 2;
        public static int StatusScelleDisponible = 3;
        public static int StatusScelleTransféré = 4;
        public static int StatusScellePosé = 5;
        public static int StatusScelleEgaré = 6;
        #endregion
        #region Scelles

        public static int ScelleAbime = 0;
        public static int ScelleRompu = 1;
        public static int ScelleRemis = 2;
        public static int ScelleDisponible = 3;
        public static int ScelleTransfere = 4;
        public static int ScellePose = 5;
        public static int ScelleEgare = 6;

        #endregion



        #region Enumeré Etape Devis

        public enum EtapeDevis
        {
            Annulé = -1,                 //indice -1
            DossierCloturé = 0,          //indice 0
            Accueil = 1,
            AffectationTache = 2,
            Survey = 3,
            ValidationTechnique = 4,
            Facturation = 5,
            ValidationEtude = 6,
            VerificationParticipation = 7,
            EditionDevis = 8,
            ValidationDevis = 9,
            Encaissement = 10,
            AnalyseDossier = 11,
            ValidationDossier = 12,
            BonDeSortie = 13,
            ProcesVerbal = 14,
            DecisionControle = 15,
            Controle = 16,
            ValidationControleCloture = 17,
            Finalise = 18,
            Annule = 19
        }


        #endregion


      

        #region CodeTable
        public static string MontantFraisTravaux = "000207";
        #endregion

        #region Type de cheque
        public static string EtatChequeChequeSurPlace = "P";
        public static string EtatChequeChequeHorsPlace = "O";

        #endregion


        #region ModePayement
        public static string ModePayementEspece = "1";
        public static string ModePayementCheque = "2";
        public static string ModePayementAjustement = "8";
        public static string CaisseModule = "2";

        #endregion

        #region EtatDeCaisse
        public static string EtatDeCaisseInexistante = "Inexistante";
        public static string EtatDeCaisseDejaCloture = "DejaCloture";
        public static string EtatDeCaisseNonCloture = "NonCloture";
        public static string EtatDeCaisseValider = "Valider";
        public static string EtatDeCaissePasDeNumCaiss = "PasDeNumCaiss";
        public static string EtatDeCaissePasCassier = "PasCassier";
        public static string EtatDeCaisseOuverteALaDemande = "OuverteAlaDemande";
        public static string EtatDeCaisseAutreSessionOuvert = "EtatDeCaisseAutreSessionOuvert";
        
        #endregion

        #region Action
            public static string ActionCreationlot = "10";
            public static string ActionEditionlot = "20";
            public static string ActionSaisielot = "30";
            public static string ActionFacturation = "70";
        #endregion
        #region Coper
        public static string CoperNAF = "082";
        public static string CoperMOR = "086";
        public static string CoperMorSolde = "085";
        public static string CoperFact = "001";
        public static string CoperRGT = "010";
        public static string CoperRNA = "083";
        //public static string CoperSCF = "556";
        public static string CoperRCD = "075";
        public static string CoperCAU = "070";
        public static string CoperFPO = "071";  // Frais de police
        public static string CoperFDO = "072";  // Frais de dossier
        public static string CoperTRV = "073";  // Frais travaux
        public static string CoperPRE = "130";  // Frais prestation
        public static string CoperFPS = "074";
        public static string CoperFAB = "077";
        public static string CoperOdDFA = "555";
        public static string CoperOdQPA = "556"; // Paiement de devis
        public static string CoperOdC01 = "501";
        public static string CoperFRP = "076";  // Frais de remise
        public static string CoperEXO = "091";  // Exoneration de TVA
        public static string CoperFPA = "000";  // Facture de paiement anticipé
        public static string CoperACT = "088";  // Achat de timbre
        public static string CoperRembAvance = "520"; //Remboursement
        public static string CoperRAC = "087"; //RegulCredit
        public static string CoperRembTrvx = "015";
        public static string CoperSoldeSuiteTransfer= "998";
        public static string CoperFactureTrvxEtDivers = "004";
        public static string CoperFactureFraude = "002";
        public static string CoperRAD = "089";
        public static string CoperUDC = "020";
        public static string CoperRDD = "110";
        public static string CoperDSD = "000";
        public static string CoperAnnulationFacture = "005";
        public static string CoperAnnulationTransaction = "200";
        public static string CoperEtudeEtSurveillance = "093";
        
        
        
        

        
        
        public static string CodeSansTaxe = "00";  // Achat de timbre



        public static string CoperAjsutemenFondCaisse = "102";
        public static string CoperFondCaisse = "103";
        public static string CoperTransfertDebit = "600";
        public static string CoperTransfertDette = "601";
        public static string CoperTransfertSolde = "602";
        public static string CoperTransfertRemboursement = "603";
        public static string AcquitInitial = "000000001";
        public static string BordereauInitial = "1";
        public static string TypeCaisse = "00";


        // 19/02/2013 par CHK 
        public static string FactureGeneraleCoper = "001";
        public static string FactureManuelleCoper = "002";
        #endregion
        #region Fourniture
        public static int FournitureCable = 2;
        #endregion


        #region Nature
        public static string TopOuvertureCaisse = "O";
        public static string Top1Administration = "7";
        #endregion

        #region CaisseState
        public static string CaisseTopAnnulation = "O";
        public static string CaisseStatAnnulation = "CANCELLATION";
        public static string CaisseStatOuverte = "O";
        public static string CaisseStatFONDCAISSE = "FONDCAISSE";
        public static string NatureFondCaisse = "09";
        public static string PrefixCaisseHorsLigne = "D"; // préfixe pour les caisses hors lignes
        #endregion

        #region ActionRecu
        public static string ActionRecuAnnulation = "Annulation";
        public static string ActionRecuDuplicat = "Duplicata";
        public static int StatusActive = 1;
        public static int StatusSupprimer = 9;
        public static string ActionRecuEditionNormale = "Normale";
        public static string OperationDeCaisseEncaissementFacture = "Facture";
        public static string OperationDeCaisseEncaissementFactureManuel = "FactureManuel";
        public static string OperationDeCaisseEncaissementMoratoire = "Moratoire";
        public static string OperationDeCaisseEncaissementDevis = "Devis";

        public static bool UtilisationAutoDeNaf = true;

        #endregion

        #region Longueur des champ
        public static int TailleCentre = 3;
        public static int TailleClient = 11;
        public static int TailleOrdre = 2;
        public static int TailleNumDevis = 8;
        public static int TailleMatricule = 5;
        public static int TailleNumCaisse = 3;
        public static int TailleDesReferenceClient = 20;
        public static int TailleCodeConso = 4;
        public static int TailleCodeRelance = 1;
        public static int TailleCodeCategorie = 2;
        public static int TailleCodeMateriel = 1;

        public static int TailleCodeTypeCompteur = 1;
        public static int TailleCodeTypeClient = 2;

        public static int TailleCodeNationalite = 3;
        public static int TailleDiametre = 3;
        public static int TailleUsage = 3;
        public static int TailleDiametreBranchement = 4;
        public static int TaillePeriodicite = 1;
        public static int TailleMoisDeFacturation = 2;
        public static int TailleCodeQuartier = 5;
        public static int TailleCodeCivilite = 6;
        public static int TailleCodeMarqueCompteur = 2;
        public static int TailleDigitCompteur = 1;
        public static int TailleTarif = 2;
        public static int TailleForfait = 1;
        public static int TailleCodeProduit = 2;
        public static int TailleNumeroLigneBatch = 6;
        public static int TailleNumeroDemande = 13;

        public static int TailleCommune = 5;
        public static int TailleQuartier = 5;
        public static int TailleSecteur = 5;
        public static int TailleRue = 5;


        public static int TailleCodeTournee = 4;
        public static int TailleNumeroBatch = 8;
        public static int TailleCas = 2;
        public static int TailleDate = 10;

        public static int TailleNoeudbrt = 3;
        public static int TaillePeriode = 6;
        public static int TailleFacture = 6;
        public static int TailleSequencePost = 4;
        public static int TailleNumeroPost = 3;
        public static int TailleCodePoste = 2;
        public static int TailleTypeComptage = 1;
        public static int TailleTypeBranchement = 4;
        public static int TailleCodeRegroupement = 8;

        public static int TailleNumeroCheque = 7;

        public static int NombreDejour = 30;
        public static int NombreMois = 6;

        #endregion

        #region General
        public static string Generale = "000";
        public static string CodeSiteScaBT = "010";
        public static string CodeSiteScaMT = "020";
        public static int IDGenerale = 1;
        
        #endregion

        #region OperationDeMoratoire
        public static string LibellePayementMoratoire = "Repayment Agreement";
        public static string LibelleCancelMoratoire = "Cancellation Miscalleneous";
        public static string LibelleReprintMoratoire = "Reprint Agreement";

        #endregion

        #region Operation de recouvrement
        public static string TraitementImpaye = "3";
        public static string CoperChqImp = "062";
        public static string TopCheqImpaye = "8";
        public static string LibNatureCheqImpaye = "CIM";
        public static string MontantFraisRDC = "000213";
        public static string CoperFraisChqImp = "065";
        public static string LibNatureFraisCheqImpaye = "FCI";

        #endregion

        #region Gui00
        //Etat compteur
        public static string CompteurActif = "Active";
        public static string CompteurActifValeur = "1";
        public static string CompteurInactif = "Removed";
        public static string CompteurInactifValeur = "0";

        // Type compteur
        public static string CompteurPrincipal = "2";

        //Code fonction
        public static string CodeFonctionCaisse = "430";
        public static string CodeFonctionPIA_E = "260";
        public static string CodeFonctionPIA_O = "270";
        public static string CodeFonctionMetreur = "410";

        //Saisie Ref client
        public static bool IsRefClientSaisie = false;
        // Creation des client 
        public static bool IsClientCreeAuGuichet = true;
        // Creation des client 
        public static bool IsCompteurAttribuerAuto = false ;

        // Creation des client 
        public static bool IsDistanceSupplementaireFacture = false;



        //Type de demande
        public static string BranchementSimple = "04";
        public static string BranchementAbonement = "05";
        public static string BranchementAbonementExtention = "50";
        public static string Resiliation = "10";
        public static string Reabonnement = "09";
        public static string FermetureBrt = "100";
        public static string RemboursementTrvxNonRealise = "06";
        public static string RemboursementAvance = "07";
        public static string ReouvertureBrt = "07";
        public static string AbonnementSeul = "08";

    

        public static string ModificationBranchement = "26";
        public static string ModificationAbonnement = "27";
        public static string ModificationAdresse = "37";
        public static string ModificationCompteur = "38";
        public static string ModificationClient = "40";
        public static string CorrectionDeDonnes = "28";

        public static string AnnulationFacture = "15";
        public static string ReprisIndex = "33";
        public static string ChangementCompteur = "12";
        public static string FactureManuelle = "13";
        public static string VerificationCompteur = "13";
        public static string AvoirConsomation = "200";
        public static string AutorisationAvancementTravaux = "14";
        public static string RegularisationAvance= "30";
        public static string PoseCompteur = "31";
        public static string DeposeCompteur = "32";
        public static string Etalonage = "18";
        public static string RechercheDemandeParNum = "99";
        public static string RechercheDemandeDebit = "41";
        public static string RechercheDemandeCredit = "42";
        public static string RechercheDemandeSolde = "43";
        public static string RechercheDemandeRemboursement = "44";
        public static string DimunitionPuissance = "45";
        public static string AugmentationPuissance = "46";
        public static string AchatTimbre = "60";
        public static string DemandeScelle = "62";
        public static string TransfertAbonnement = "51";
        public static string BranchementAbonnementMt = "66";
        public static string BranchementAbonnementEp = "63";
        public static string DemandeFraude = "65";
        public static string DemandeReclamation = "61";
        public static string RemboursementParticipation = "67";
        public static string DepannageEp = "64";
        public static string DepannageMT = "68";
        public static string DepannagePrepayer = "69";
        public static string DepannageClient = "70";
        public static string DepannageMaintenance = "71";
        public static string TransfertSiteNonMigre = "75";
        public static string ChangementProduit = "19";


        
        //Prise en compte du devis
        public static bool IsGestionGlobaleCoutFournitureDevis = false;
        public static bool IsDevisPrisEnCompteAuGuichet = false;
        public static bool IsReglementDevisPrisEnCompteAuGuichet = false;
        public static bool IsPaiementFraixDevis = false;
        public static bool IsModificationAutoriserEnFacturation = false;
        public static bool IsUtilisateurCreeParAgent = false;
        public static bool IsResilierPriseEcompte = false;
        //Etat branchement
        public static string IsBranchementSubventione = "1";
        public static string IsBranchementNonSubventione = "0";

        //Etat facturation
        public static bool IsFacturationPartielle = false;

        //Status evenement
        public static int EvenementCree = 0;
        public static int EvenementReleve = 1;
        public static int EvenementFacture = 3;
        public static int EvenementMisAJour = 4;
        public static int EvenementDefacture = 5;
        public static int EvenementRejeter = 6;
        public static int EvenementAnnule = 7;
        public static int EvenementSupprimer = 8;
        public static int EvenementPurger = 99;
        public static int EvenementDetruit =10;

        // Status pagerie
        public static string PagerieEnquetable = "1";
        public static string PagerieNonEnquetable = "0";
        public static string PagerieConfirme = "2";

        //Status demande
        public static string DemandeStatusEnAttente = "1";
        public static string DemandeStatusPasseeEncaisse = "2";
        public static string DemandeStatusPriseEnCompte = "3";
        public static string DemandeStatusEnCaisse = "4";


        public static string TypeComptagePoint = "7";
        public static string TypeComptagePleinne = "8";
        public static string TypeComptageCreuse = "9";
        public static string TypeComptageMaximetre = "6";
        public static string TypeComptageHoraire = "A";
        public static string TypeComptageReactif = "5";

        public static int BorneConsohoraire = 720;


        

        //Niveau de tarif
        public static string NiveauTarif_Nat = "000001";
        public static string NiveauTarif_Regional = "000002";
        public static string NiveauTarif_SRegional = "000003";
        public static string NiveauTarif_Centre = "000004";
        public static string NiveauTarif_Communale = "000005";

        //Mode application de tarif
        public static string ModeApplicationTarifDate = "D";
        public static string ModeApplicationTarifPeriode = "P";


        //Code evenement
        public static string EvenementCodeNormale = "01";
        public static string EvenementCodeDeposeCpt = "02";
        public static string EvenementCodePoseCpt = "03";
        public static string EvenementCodeFermetureBrt = "04";
        public static string EvenementCodeOuvertureBrt = "05";
        public static string EvenementFermetureBrtAvecDepose = "06";
        public static string EvenementCodeForfait = "07";
        public static string EvenementCodeSuspensionAbon = "08";
        public static string EvenementCodeCreationLot = "09";
        public static string EvenementCodeFactureIsole = "90";
        public static string EvenementCodeResiliation = "91";
        public static string EvenementCodeAvoirConso = "92";
        public static string EvenementCodeRejetFacture = "93";
        public static string EvenementCodeDefacturationLot = "94";
        public static string EvenementCodeFactureAjustementNeg = "95";
        public static string EvenementCodeFactureAjustementPos = "96";


        //lotri evenement
        public static string LotriTermination = "00001";
        public static string LotriManuel = "00002";
        public static string LotriAjustement = "00003";
        public static string LotriChangementCompteur = "00004";
        // Cas de saisi
        public static string CasPoseCompteur = "92";
        public static string CasDeposeCompteur = "93";
        public static string CasCreation = "##";
        public static string CasNindesSup = "82";
        public static string CasPassageZero = "10";

        // Action detail
        public static int IsInsertion = 1;
        public static int IsUpdate = 2;
        public static int IsDelete = 3;

        // Mode de Calcul
        public static string FacturationNormale = "1";
        public static string FacturationForfaitAvecRegul = "2";
        public static string FacturationBloqueSansRegul = "3";
        public static string FacturationTarifAnnuelUniquement = "4";
        public static string FacturationTarifUnitaireUniquement = "5";
        public static string FacturationForfaitSansRegul = "6";
        public static string FacturationBloqueAvecRegul = "7";
        public static string FacturationEstimerAvecRegul = "8";
        public static string FacturationEstimerSanRegul = "9";


        // Mode de Facturation
        public static string NombrePeriode = "23";
        public static string NombreJour360 = "13";// ss prorata , 360 jours
        public static string NombreJour30 = "16"; // ss prorata , 30 jours
        public static string NombreJourPeriode30 = "19"; // ss prorata , 30 jours


        public static string CODEUNITE = "01";

        public static string PROPRIETRAIRE = "0";
        public static string LOCATAIRE = "1"; 

        

        // Type facturation
        public static string Partiel = "P";
        public static string Definitif = "D";

        // Etat mise a jours
        public static string MiseAJours = "M";
        public static string NonMiseAJours = "R";
        public static string NonMiseAbonne = "A";
        public static string SimulationFacture = "S";

        //Top 1
        public static string TopGuichet = "0";
        public static string TopFacturation = "1";
        public static string TopCaisse = "2";
        public static string TopSaisieDeMasse = "3";
        public static string TopPortables = "4";
        public static string TopPaiementsDeplaces = "5";
        public static string TopMoratoire = "6";
        #endregion

        public static string AcquitLettrageAuto = "123456789";


        #region Reclamation
        public static int NewReclamation = 1;
        public static int EnqueryVerification = 2;
        public static int DepartmentAllocation = 3;
        public static int ManagerChecking = 4;
        public static int DepartmentChecking = 5;
        public static int SupervisorChecking = 6;
        public static int PendingClosing = 7;
        public static int Closed = 8;
        /*INDEX */
        public static int Annule = 7;
        public static int Supprime = 8;
        public static int Purger = 99;

        /* Validation reclamation */
        public static int Satisfaction = 1;
        public static int NonSatisfaction = 2;
        public static int RepriseTraitement = 3;
        public static int TraitementEnCour = 4;
        public static int SecondTraitementEnCour = 5;
        public static int TroisiemeTraitementEnCour = 6;

        /*Status reclamation */
        public static int Initie = 1;
        public static int Traitement = 2;
        public static int Validation = 3;
        public static int ReTraitement = 4;
        public static int Clos = 5;
        public static int AttenteCloture = 6;
        public static int Rejet = 7;
        #endregion


        // CHK - 28/02/2013 
        public static string AffichageListeAbonnement = "01";
        public static string AffichageListeResisiliation = "02";
        public static string LibellePayementAnticipe = "Payement anticipé";
        #region Encaissement deconnecte CHK - 13/03/2013
  
        #endregion

        public static string BranchementSociaux = "00011";

        public static string Nature12 = "12";
        public static string Nature13 = "13";
        public static string Nature14 = "14";
        public static string Nature15 = "15";
        public static string Nature16 = "16";
        public static int Cas80 = 80;
        public static string FactureIsole = "90";
        public static string FactureResiliation = "91";


        #region TypeComptage
        public static string TRIPHASE_PUISSANCE = "6";
        public static string MTHT_ACTIF = "4";
        public static string TRIPHASE_REACTIF = "5";
        public static string FORMULE_FRAUDE = "110";

        public static string TYPECOMPTAGE_1 = "1";
        public static string TYPECOMPTAGE_2 = "2";
        public static string TYPECOMPTAGE_3 = "3";
        public static string TYPECOMPTAGE_4 = "4";
        #endregion

        #region TypeCompteurMT
        public static string ACTIF = MTHT_ACTIF;
        public static string ACTIF_HPOINTES = "7";
        public static string ACTIF_HPLEINES = "8";
        public static string ACTIF_HCREUSES = "9";
        public static string REACTIF = TRIPHASE_REACTIF;
        public static string HORAIRE = "A";
        public static string MAXIMETRE = TRIPHASE_PUISSANCE;
        #endregion

        public static string REDEVANCEEXONERATIONTVA = "63";

        public static string REDEVANCEPRIMEFIXE = "64";
        public static string REDEVANCEENTRETIEN = "84";
        public static string REDEVANCEMAJORATION = "74";
        public static string REDEVANCEDEPASSEMENT = "54";

        public static string REDEVANCECONSOACTIVE = "14";
        public static string REDEVANCECONSOPOINTE = "24";
        public static string REDEVANCECONSOPLEINE = "34";
        public static string REDEVANCECONSOCREUSE = "44";

        public static string REDEVANCECONSOMMATIONBT = "13";

        



         public static string  NOLIEN	=					"0"; // Redevance sans lien avec une redevance
         public static string LIE_A_REDEVANCE	=			"1"; // Redevance étant un paramètre d'une autre redevance
         public static string A_DES_PARAMETRES =			"2" ;// Redevance ayant comme paraètre d'autre redevances
         public static string LIE_ET_A_DES_PARAMETRES = "3";// Redevance étant un paramètre d'une autre redevance et admettant d'autres redevances comme paramètres

        #region Gestion du Devis
        public static string Comment = "Une partie du materiel n''a pas été utilisée";
        public static int BranchementAFinaliser = 18;
        public static string LibellemoduleDevis = "Devis";
        public static string Complete = "complété";
        public static string Simplifie = "simplifié";
        public static string Diamete = "DIAMETRE";
        public static string Agence = "AGENCE";
        public static string TypeCompteur = "TYPE COMPTEUR";
        public static string NumeroCompte = "numéro de compte";
        public static string Message1 = "PLEASE ERECT A SERVICE POLE AND A BRACKET AT THE MARKED AREAS";
        public static string Message2 = "PLEASE ERECT A BRACKET AT THE MARKED AREA";
        public static string Message3 = "PLEASE ERECT A SERVICE POLE AT THE MARKED AREA";
        public static string Message4 = "TAR ROAD-CROSSING MUST BE PAID AT THE DEPARTMENT OF WORKS";
        #endregion

        #region ActionFacturation (Defacturation,Destruction simulation)
        public static string Normal = "0";
        public static string Simulation = "1";
        public static string Defacturation = "2";
        public static string DestructionSimulation = "3";
        public static string DemandeDefacturation = "4";
        public static string DemandeDefacturationRejeter = "5";


        #endregion

   


        public static string CategorieAgentEdm = "07";
        public static string CategorieEp = "08";
        public static string CategorieConsoInterne = "09";
        public static string UsageEp = "003";
        public static string CodeConsomateurEp = "0860";
        public static string CodeConsomateurIndeterminer = "0001";

        #region OperationComptable
        public static string FactureTravaux = "01";
        public static string FactureEmissionGeneral = "03";
        public static string FactureAnnulation = "04";
        
        public static string FraisRemise = "05";
        public static string FraisEtalonnage = "06";
        public static string FraisChequeImpaye = "07";

        public static string EncaissementBranchement = "02";
        public static string EncaissementFacture = "08";
        public static string ComptaAchatTimbre = "10";
        public static string EncaisseFraisRemise = "11";
        public static string EncaisseEtalonnage = "12";
        public static string RemboursementASC = "13";
        public static string ResiliationAbonnement = "14";
        #endregion

        #region TypeDocument
        public static string Preuve = "001";
        public static string Schema = "002";
        public static string Manuscrit = "003";
        public static string PieceIdentite = "004";
        public static string Contrat = "005";
        public static string TitrePropriete = "006";
        public static string DossierPromoteur = "007";
        public static string AutorisationMairie = "008";
        public static string InstructionDG = "009";
        public static string DemandePrestation = "010";
        public static string FicheDexoneration = "011";

        
        #endregion

        #region Rubrique demande
        public static string LIGNEHTA = "001";
        public static string POSTEHTABT = "002";
        public static string LIGNEBT = "003";
        public static string ENSEMBLECOMPTAGE = "004";
        public static string DEVISBRANCHEMENT = "005";
        public static string DEVISDEXTENSION = "006";
        #endregion

        public static string NationnaliteMali = "MLI";
        public static string RegroupementParticilier = "01001000";

        public static string CheminImpressionServeur = string.Empty;
        public static string CheminImpressionClient = string.Empty;


        


    }
}



