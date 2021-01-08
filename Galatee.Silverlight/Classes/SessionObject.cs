using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
//using Galatee.Silverlight.serviceWeb;
using Galatee.Silverlight.ServiceAuthenInitialize;
using Galatee.Silverlight.ServiceCaisse;
using Galatee.Silverlight.ServiceAdministration;
using System.Collections.Generic;
using System.Reflection;
using Galatee.Silverlight.Classes;
using System.ComponentModel;
using System.Windows.Threading;
using System.IO.IsolatedStorage;
using System.IO;
using Galatee.Silverlight.ServicePrintings;
using System.Xml.Serialization;
using System.Text;
using System.Linq;
using System.Xml.Linq;
using Galatee.Silverlight;
using System.Xml;
using Galatee.Silverlight.Caisse;
using System.Collections.ObjectModel;
using Galatee.Silverlight.ServiceRecouvrement;
using Galatee.Silverlight.ServiceParametrage;
//using Galatee.Silverlight.ServiceEclairagePublic;

namespace Galatee.Silverlight
{
    public static class SessionObject
    {
        public static System.Globalization.CultureInfo cul;
        public static System.Resources.ResourceManager res_man = new System.Resources.ResourceManager("Galatee.Silverlight.Resources.Accueil.Langue.en-US", typeof(Galatee.Silverlight.Resources.Accueil.Langue).Assembly);


        public const string Devise = "FCFA";
        public const string Centime = " ";

        public static System.ServiceModel.EndpointAddress LocalServiceCaisse;
        public static string ServerEtatInit = "Online";
        public static string CodeFonctionMetreur = "001";
        public static string CodeFonctionReleveur = "320";
        public static bool IsFacturationPartiel =false;
        public static List<CsFraisTimbre> LstFraisTimbre = new List<CsFraisTimbre>();
        public static ServiceAuthenInitialize.EnumereWrap Enumere;
        public static ServiceAuthenInitialize.EnumProcedureStockee EnumereProcedureStockee;
        public static string EtatCaisse;
        public static string ModuleEnCours;
        
        public static bool IsCaisseOuverte = false;
        public static bool PosteNonCaisse = false ;

        public static string NatureByLibelleCourtFraisCheqImpaye = string.Empty;
        public static string NatureByLibelleCourtCheqImpaye = string.Empty;
        public static string moisComptable = "201211";
    



        public static bool  IsChargerDashbord = false ;

        public static BusyIndicator busyIndicator = new BusyIndicator();
        public static string CodeCaisse = "040";
        public static string DernierNumeroAcquit = string.Empty;
        public static object objectSelected;
        public static Viewbox ViewBox;
        public static DataGrid gridUtilisateur;
        public static List<int> busyIndicatorHandlers = new List<int>();
        public static List<BusyIndicator> ListeDesChargementsEnCours;
        public static string moduleCourant = string.Empty;
        public static List<string> OpenedModules = new List<string>();
        public static List<ContextMenuItem> MenuContextuelItem = new List<ContextMenuItem>();
        public static Galatee.Silverlight.ServiceAuthenInitialize.CsStrategieSecurite securiteActive = null;
        /// <summary>
        /// Le numero de dernier recu delivré, il est relatif uniquement a la caissiere actuelle
        /// </summary>
        public static decimal? DernierNumeroDeRecu = -1;
        public static string cheminServeurBD;
        public static string DefaultPrinter = string.Empty;
        public static IList<Galatee.Silverlight.ServiceCaisse.CsBanque> ListeBanques = null;
        public static IList<CsModereglement> ListeModesReglement = null;
        public static MenuItem MenuItemClicked = null;
        /// <summary>
        /// Indique si le serveur est inaccessible
        /// </summary>
        public static bool IsServerDown = false;


        public static string ConnexionGaladb = "galadb";
        public static string ConnexionAbo07 = "ABO07";

        public static string CheminImpression;
        public static string CheminExportation;
        public static string CheminDocumentScanne;
        public static string EnvoiPrinter = "Imprimante";
        public static string EnvoiPdf = "pdf";
        public static string EnvoiExecl = "xlsx";
        public static string EnvoiWord = "doc";


        public enum StatutDemande
        {
            Initiée=1,
            En_cours=2,
            Suspendue=3,
            Annulée=4,
            Rejetée=5,
            Terminée=6
        }  



        public static string ServerEndPointName = string.Empty;
        public static string ServerEndPointPort = string.Empty;
        public static string MachineName = string.Empty;
        public static string MachinePort = string.Empty;

        public static List<Galatee.Silverlight.ServiceAccueil.CsForfait> LstForfait = new List<Galatee.Silverlight.ServiceAccueil.CsForfait>();
        public static List<Galatee.Silverlight.ServiceAccueil.CsRubriqueDevis> LstRubriqueDevis = new List<Galatee.Silverlight.ServiceAccueil.CsRubriqueDevis>();
        public static List<Galatee.Silverlight.ServiceAccueil.CsPuissance> LstPuissance = new List<Galatee.Silverlight.ServiceAccueil.CsPuissance>();
        public static List<Galatee.Silverlight.ServiceAccueil.CsPuissance> LstPuissanceParReglageCompteur = new List<Galatee.Silverlight.ServiceAccueil.CsPuissance>();
        public static List<Galatee.Silverlight.ServiceAccueil.CsPuissance> LstPuissanceInstalle = new List<Galatee.Silverlight.ServiceAccueil.CsPuissance>();
        public static List<Galatee.Silverlight.ServiceAccueil.CsCodeTaxeApplication> LstCodeApplicationTaxe = new List<Galatee.Silverlight.ServiceAccueil.CsCodeTaxeApplication>();
        public static List<Galatee.Silverlight.ServiceAccueil.CsFrequence> LstFrequence = new List<Galatee.Silverlight.ServiceAccueil.CsFrequence>();
        public static List<Galatee.Silverlight.ServiceAccueil.CsMois> LstMois = new List<Galatee.Silverlight.ServiceAccueil.CsMois>();
        public static List<Galatee.Silverlight.ServiceAccueil.CsTarif> LstTarif = new List<Galatee.Silverlight.ServiceAccueil.CsTarif>();
        public static List<Galatee.Silverlight.ServiceAccueil.CsTarif> LstTarifParReglageCompteur = new List<Galatee.Silverlight.ServiceAccueil.CsTarif>();
        public static List<Galatee.Silverlight.ServiceAccueil.CsTarif> LstTarifCategorie = new List<Galatee.Silverlight.ServiceAccueil.CsTarif>();
        public static List<Galatee.Silverlight.ServiceAccueil.CsModepaiement> LstModePaiement = new List<Galatee.Silverlight.ServiceAccueil.CsModepaiement>();
        public static List<Galatee.Silverlight.ServiceAccueil.CsTypeComptage> LstTypeComptage = new List<Galatee.Silverlight.ServiceAccueil.CsTypeComptage>();


        public static List<Galatee.Silverlight.ServiceAccueil.CsTarif> LstTarifPuissance = new List<Galatee.Silverlight.ServiceAccueil.CsTarif>();
        public static List<Galatee.Silverlight.ServiceAccueil.CsCommune> LstCommune = new List<Galatee.Silverlight.ServiceAccueil.CsCommune>();
        public static List<Galatee.Silverlight.ServiceAccueil.CsQuartier> LstQuartier = new List<Galatee.Silverlight.ServiceAccueil.CsQuartier>();
        public static List<Galatee.Silverlight.ServiceAccueil.CsRues> LstRues = new List<Galatee.Silverlight.ServiceAccueil.CsRues>();
        public static List<Galatee.Silverlight.ServiceAccueil.CsTournee> LstZone = new List<Galatee.Silverlight.ServiceAccueil.CsTournee>();
        public static List<Galatee.Silverlight.ServiceAccueil.CsSecteur> LstSecteur = new List<Galatee.Silverlight.ServiceAccueil.CsSecteur>();
        public static List<Galatee.Silverlight.ServiceAccueil.ObjAPPAREILS> LstAppareil = new List<Galatee.Silverlight.ServiceAccueil.ObjAPPAREILS>();

        public static List<Galatee.Silverlight.ServiceAccueil.CsCentre> LstCentre = new List<Galatee.Silverlight.ServiceAccueil.CsCentre>();
        public static List<Galatee.Silverlight.ServiceRecouvrement.CsMotifChequeImpaye> LstMotifRejetsCheque = new List<Galatee.Silverlight.ServiceRecouvrement.CsMotifChequeImpaye>();
        public static List<Galatee.Silverlight.ServiceAccueil.CsEtapeDemande> LstEtapeDemande = new List<Galatee.Silverlight.ServiceAccueil.CsEtapeDemande>();
        public static List<Galatee.Silverlight.ServiceAccueil.CsTdem> LstTypeDemande = new List<Galatee.Silverlight.ServiceAccueil.CsTdem>();

        public static List<Galatee.Silverlight.ServiceAccueil.CsCentre> LstCentreDuPerimetreAction = new List<Galatee.Silverlight.ServiceAccueil.CsCentre>();
        public static List<int> LstIdCentreDuPerimetreAction = new List<int>();
        public static List<Galatee.Silverlight.ServiceAccueil.CsSite> LstSiteDuPerimetreAction = new List<Galatee.Silverlight.ServiceAccueil.CsSite>();

        public static List<Galatee.Silverlight.ServiceAccueil.CsTypeBranchement> LstTypeBranchement = new List<Galatee.Silverlight.ServiceAccueil.CsTypeBranchement>();
        public static List<Galatee.Silverlight.ServiceAccueil.CsParametreBranchement> LstParametreBranchement = new List<Galatee.Silverlight.ServiceAccueil.CsParametreBranchement>();
        public static List<Galatee.Silverlight.ServiceAccueil.CsProduit> ListeDesProduit = new List<Galatee.Silverlight.ServiceAccueil.CsProduit>();

        public static List<Galatee.Silverlight.ServiceAccueil.CsTypeClient> LstTypeClient = new List<Galatee.Silverlight.ServiceAccueil.CsTypeClient>();

        public static List<Galatee.Silverlight.ServiceAccueil.CsCategorieClient> LstCategorie = new List<Galatee.Silverlight.ServiceAccueil.CsCategorieClient>();
        public static List<Galatee.Silverlight.ServiceAccueil.CsDenomination> LstCivilite = new List<Galatee.Silverlight.ServiceAccueil.CsDenomination>();
        public static List<Galatee.Silverlight.ServiceAccueil.CsNationalite> LstDesNationalites = new List<Galatee.Silverlight.ServiceAccueil.CsNationalite>();

        public static List<Galatee.Silverlight.ServiceAccueil.CsRegCli> LstCodeRegroupement = new List<Galatee.Silverlight.ServiceAccueil.CsRegCli>();
        public static List<Galatee.Silverlight.ServiceAccueil.CsNatureClient> LstNatureClient = new List<Galatee.Silverlight.ServiceAccueil.CsNatureClient>();
        public static List<Galatee.Silverlight.ServiceAccueil.CsFermable> LstFermable = new List<Galatee.Silverlight.ServiceAccueil.CsFermable>();
        public static List<Galatee.Silverlight.ServiceAccueil.CsCodeConsomateur> LstCodeConsomateur = new List<Galatee.Silverlight.ServiceAccueil.CsCodeConsomateur>();

        public static List<Galatee.Silverlight.ServiceAccueil.CsReglageCompteur> LstReglageCompteur = new List<Galatee.Silverlight.ServiceAccueil.CsReglageCompteur>();
        public static List<Galatee.Silverlight.ServiceAccueil.CsCalibreCompteur> LstCalibreCompteur = new List<Galatee.Silverlight.ServiceAccueil.CsCalibreCompteur>();
        public static List<Galatee.Silverlight.ServiceAccueil.CsCadran> LstCadran = new List<Galatee.Silverlight.ServiceAccueil.CsCadran>();
        public static List<Galatee.Silverlight.ServiceAccueil.CsMarqueCompteur> LstMarque = new List<Galatee.Silverlight.ServiceAccueil.CsMarqueCompteur>();
        public static List<Galatee.Silverlight.ServiceAccueil.CsTcompteur> LstTypeCompteur = new List<Galatee.Silverlight.ServiceAccueil.CsTcompteur>();
        public static List<ServiceAccueil.CsProprietaire> Lsttypeprop = new List<ServiceAccueil.CsProprietaire>();

       
        public static List<Galatee.Silverlight.ServiceAccueil.CsCtax> LstDesTaxe = new List<Galatee.Silverlight.ServiceAccueil.CsCtax>();
        public static List<Galatee.Silverlight.ServiceAccueil.CsCoutDemande> LstDesCoutDemande = new List<Galatee.Silverlight.ServiceAccueil.CsCoutDemande>();
        public static List<Galatee.Silverlight.ServiceAccueil.CsCoper> LstDesCopers = new List<Galatee.Silverlight.ServiceAccueil.CsCoper>();
        public static List<Galatee.Silverlight.ServiceAccueil.CsCasind> LstDesCas = new List<Galatee.Silverlight.ServiceAccueil.CsCasind>();
        public static List<Galatee.Silverlight.ServiceAccueil.CsDepart> LsDesDepartHTA = new List<Galatee.Silverlight.ServiceAccueil.CsDepart>();
        public static List<Galatee.Silverlight.ServiceAccueil.CsPosteSource> LsDesPosteElectriquesSource = new List<Galatee.Silverlight.ServiceAccueil.CsPosteSource>();
        public static List<Galatee.Silverlight.ServiceAccueil.CsPosteTransformation> LsDesPosteElectriquesTransformateur = new List<Galatee.Silverlight.ServiceAccueil.CsPosteTransformation>();

        public static List<Galatee.Silverlight.ServiceAdministration.CsPoste> ListePoste = new List<Galatee.Silverlight.ServiceAdministration.CsPoste>();
        public static List<Galatee.Silverlight.ServiceAdministration.CsFonction> ListeFonction = new List<Galatee.Silverlight.ServiceAdministration.CsFonction>();
        public static List<Galatee.Silverlight.ServiceAdministration.CsProgramMenu> ListeProgramMenu = new List<Galatee.Silverlight.ServiceAdministration.CsProgramMenu>();
        public static List<Galatee.Silverlight.ServiceAdministration.CsUtilisateur> ListeDesUtilisateurs = new List<Galatee.Silverlight.ServiceAdministration.CsUtilisateur>();
        public static List<Galatee.Silverlight.ServiceAdministration.CsProfil> ListeDesProfils = new List<Galatee.Silverlight.ServiceAdministration.CsProfil>();
        public static List<Galatee.Silverlight.ServiceAdministration.CsCentre> ListeDesCentreAdm = new List<Galatee.Silverlight.ServiceAdministration.CsCentre>();
        public static List<Galatee.Silverlight.ServiceAdministration.CsSite> ListeDesSitesAdm = new List<Galatee.Silverlight.ServiceAdministration.CsSite>();


        public static List<Galatee.Silverlight.ServiceAccueil.CsOrigineLot> ListeOrigine = new List<Galatee.Silverlight.ServiceAccueil.CsOrigineLot>();
        public static List<Galatee.Silverlight.ServiceAccueil.CsTypeLot> ListeTypeLot = new List<Galatee.Silverlight.ServiceAccueil.CsTypeLot>();
        public static List<Galatee.Silverlight.ServiceCaisse.CsCaisse> ListeCaisse = new List<Galatee.Silverlight.ServiceCaisse.CsCaisse>();
        public static List<Galatee.Silverlight.ServiceCaisse.CsLibelleTop> LstDesLibelleTop = new List<Galatee.Silverlight.ServiceCaisse.CsLibelleTop>();

        public static List<Galatee.Silverlight.ServiceScelles.CsActivite> LstDesActivitee = new List<Galatee.Silverlight.ServiceScelles.CsActivite>();
        public static List<Galatee.Silverlight.ServiceScelles.CsRefFournisseurs> LstDesFournisseur = new List<Galatee.Silverlight.ServiceScelles.CsRefFournisseurs>();
        public static List<Galatee.Silverlight.ServiceScelles.CsRefOrigineScelle> LstDesOrigineScelle = new List<Galatee.Silverlight.ServiceScelles.CsRefOrigineScelle>();
        public static List<Galatee.Silverlight.ServiceScelles.CsCouleurActivite> LstDesCouleur = new List<Galatee.Silverlight.ServiceScelles.CsCouleurActivite>();
        public static List<Galatee.Silverlight.ServiceScelles.CsMarque_Modele> LstMarqueModele = new List<Galatee.Silverlight.ServiceScelles.CsMarque_Modele>();
        public static List<Galatee.Silverlight.ServiceScelles.CsRefEtatCompteur> LstEtatCompteur = new List<Galatee.Silverlight.ServiceScelles.CsRefEtatCompteur>();


        public static Galatee.Silverlight.ServiceCaisse.CsParametresGeneraux TauxMinimunDemande = new Galatee.Silverlight.ServiceCaisse.CsParametresGeneraux();

        public static Galatee.Silverlight.ServiceAdministration.CsPoste LePosteCourant = new Galatee.Silverlight.ServiceAdministration.CsPoste();

        public static List<Galatee.Silverlight.ServiceParametrage.CsOperation> ListeDesOperation = new List<Galatee.Silverlight.ServiceParametrage.CsOperation>();
        public static List<Galatee.Silverlight.ServiceParametrage.CsEtape> ListeDesEtapes = new List<Galatee.Silverlight.ServiceParametrage.CsEtape>();
        public static List<Galatee.Silverlight.ServiceReclamation.CsTypeReclamationRcl> ListeDesReclamation = new List<Galatee.Silverlight.ServiceReclamation.CsTypeReclamationRcl>();

        #region Devis
        public static List<Galatee.Silverlight.ServiceAccueil.CsDiametreBranchement> LstDiametreDevis = new List<Galatee.Silverlight.ServiceAccueil.CsDiametreBranchement>();
        #endregion

        const string formatFrancais = "#,0.";
        const string formatAnglais = "#,0.00#";
        public static bool IsBusyOn = false;
        public static int buzyValue = 1;
        public static string FormatMontant = formatFrancais;
        public static bool EtatControlCourant = true;
        public static bool IsCompteur1Saisie = false;
        public static decimal distanceMaxiElectricite = 0;
        public static decimal seuilDistanceElectricite = 0;
        public static decimal distanceMaxiSubventionElectricite = 0;
        public static string CodeNina = "NINA";
        public static string CodeModeRecueilTelephone = "TELEPHONE";

        

        public static decimal distanceMaxiEau = 0;
        public static decimal seuilDistanceEau = 0;
        public static decimal distanceMaxiSubventionEau = 0;
        public static Galatee.Silverlight.ServiceCaisse.CsHabilitationCaisse LaCaisseCourante = new Galatee.Silverlight.ServiceCaisse.CsHabilitationCaisse();
        public static int?  iDCaisseDeclaree = null; /** ZEG 29/08/2017 **/


        /// <summary>
        /// Liste utilisée pour enregistrer les fenetres d'encaissement ouvertes.
        /// En cas de travail hors ligne, elle seront toutes fermées
        /// </summary>
        public static List<ChildWindow> ListeControlesCaisse = new List<ChildWindow>();

        public enum TypeDocumentScanneDevis
        {
            Lettre = 1,
            Propriete,
        }

        public enum sens
        {
            sup = 1,
            inf = -1,
            egal = 0
        }

        public enum ExecMode
        {
            Default, Creation, Recherche, Consultation, Modification, ChangementMotDePasse, Suppression, Active, Transmettre, Commenter, Muter
        }

        public enum frequenceMoratoire
        {
            Month = 1,
            ForNight = 2,
            Week = 3
        }

        public static string[] LoadComboBoxData()
        {
            string[] strArray =
                {

                    "=",
                    ">=",
                    "<="
                };
            return strArray;

        }

        public static string[] Cret()
        {
            string[] strArray =
                {

                    "A","B","C","D","E","F",
                    "G","H","I","J","K","L"
                };
            return strArray;

        }

        public static string[] TypeOperationClasseur()
        {
            string[] strArray =
                {
                    "TOUT","REGLEMENT","FACTURE","IMPAYES"
                };
            return strArray;

        }
        public static string[] TypeOperationClasseurReg()
        {
            string[] strArray =
                {
                    "TOUT","REGLEMENT","IMPAYES","MANDATEMENT"
                };
            return strArray;

        }
        public static string[] TypeOperationNonLieAuCentre()
        {
            string[] strArray =
                {
                    "DEPEP","CMSCG"
                };
            return strArray;

        }

        // creation du dictionnaire des tables TA0 pour la creation dynamique des child window
        // par HGB 26/12/2012

        public static Dictionary<int, string> getTableDetails()
        {
            Dictionary<int, string> codeToWindow = new Dictionary<int, string>() { 
          
            {0, "FrmGeneric"},
            {1001, "UcINIT"},
            {1002, "UcRue"},
            {1003, "UcREGROU"},
            {1005, "UcDIACOMP"},
            {1006, "UcTCOMPT"},
            {1007, "UcPuissance"},
            {1010, "UcTARIF"},
            {1011, "UcFORFAIT"},
            {1014, "UcBANQUES2"},
            {1016, "UcCASIND"},
            {1023, "UcCTAX"},
            {1025, "UcMATRICULE"},
            {1034, "UcTDEM"},
            {1038, "UcREGCLI"},
            {1039, "FrmClientRegrp"},
            {1041, "UcGEOGES"},
            {1044, "UcQUARTIER"},
            {1046, "UcMESSAGE"},
            {1048, ""},
            {1049, ""},
            {1053, "UcSPESITE"},
            {1054, ""},
            {1055, "UcREGEXO"},
            {1058, "UcDIRECTEUR"},
            {1059, "UcDEMCOUT"},
            {1060, "UcMODEREG"},
            {1061, "UcFRAISTIMB"},
            {1062, "UcFRAISHP"},
            {1063, ""},
            {1064, "UcCOPER"},
            {1065, "UcNATURE"},
            {1066, "UcDOMBANC"},
            {1067, "UcCOPEROD"},
            {1068, "UcARRETE"},
            {1069, ""},
            {1070, "UcTAXCOMP"},
            {1071, "UcIMPRIM"},
            {1072, "UcREDEVANCE"},
            {1073, "UcNATGEN"},
            {1074, "UcSCHEMAS"},
            {1075, "UcAJUFIN"},
            {1076, "UcFRAISCONTENTIEUX"},
            {1077, "UcLIBRUBACTION"},
            {1078, "UcSECURITEMATRICULE"},
            {1079, "UcMONNAIE"},
            {1080, "UcDEFPARAMABON"},
            {1081, "UcPARAMABONUTILISE"},
            {1082, "UcCENTREENCAISSABLE"},
            {1083, "UcTYPELOT"},
            {1084, "UcCONTROLELIGNE"},
            {1085, "UcCONTROLESECONDAIRE"},
            {1086, "UcCONTROLEPIECE"},
            {1087, ""},
            {1088, ""},
            {1089, ""},
            {1090, ""},
            {1091, ""},
            {1094, ""},
            {1100, "UcMULTIMONNAIES"},
            {1101, "UcCodeControle"},
            {1102, "UcOrigineLot"},
            {1107, "UcSECTEUR"},
            {1113, "UcCOUTPUISSANCE"},
            {1116, "UcTYPEBRANCHEMENT"},
            {1122, "UcPUISPERTES"},
            {1123, "UcCOMPTAGE"},
            {1124, "UcTRANSFOCOMPTAGE"},
            {2000, "FrmGenericDevis"},
            {2001, "UcTypeDevis"},
            {2002, "UcAppareils"},
            {2003, "UcFourniture"},
            {2004, "UcCaracteristiques"},
            {2005, "UcEtapeDevis"},
            {2006, "UcETAPERECLAMATION"},
            {2007, "UcTYPERECLAMATION"},
            {2008, "UcETAPEFONCTION"},
            {2009, "UcPRESTATAIRE"},
            {2010, "UcStatutReclamation"},
            {2011, "UcGroupeReclamation"},
            {2012, "UcGroupProgram"},
            {2013, "UcProgram"},
            {2014, "UcHabilitationProgram"},
            {2015, "UcProduit"},
            {2016, "UcMotifRejet"},
            {2017, "UcFonction"},
            {2018, "UcCentre"},
            {2019, "UcSite"},
            {2020, "UcMotifRejet"},
            {2021, "UcEtapeFonctionCheque"},
            {2047, "UcTypeCentre"},
            {2048, "UcHabilitationCheque"}
            //putain quelle tache fastidieuz   26/12/2012 HGB 
        };
            return codeToWindow;
        }


        /// <summary>
        /// Renvoi Liste des mois de l'année
        /// </summary>
        /// <returns></returns> 
        public static List<string> ObtenirLesMois()
        {
            return new List<string> { "Janvier", "Fevrier", "Mars", "Avril", "Mai", "Juin", "Juillet", "Aout", "Septembre", "Octobre", "Novembre", "Décembre" };
        }

        /// <summary>
        /// Test la presence d'une connexion reseau et execute l'une des methodes passée
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="P"></typeparam>
        /// <param name="aExecuterEnCasDeSucces">Methode a executer en cas de succes du test</param>
        /// <param name="aExecuterEnCasDechec">Methode a executer en cas d'echec du test</param>
        /// <param name="executerEnBackground">Indique s'il le test affiche un indicateur visuel de chargement</param>

        public static int NombreElementPageSaisiIndex = 6;
        public static List<String> periode = new List<String>();

        #region SIG

        public static List<CsCAMPAGNE> campagne_coupure = new List<CsCAMPAGNE>();


        #endregion

        #region Tarificaction

        #region Liste des redevence

        public static List<ServiceTarification.CsRedevance> ListeRedevence = new List<ServiceTarification.CsRedevance>();
        public static List<ServiceTarification.CsVariableDeTarification> ListeVariableTarif = new List<ServiceTarification.CsVariableDeTarification>();
        

        #endregion

        #region Liste des recheche de tarification

        public static List<ServiceTarification.CsRechercheTarif> ListeRechercheTarif = new List<ServiceTarification.CsRechercheTarif>();

        #endregion

        #region Liste Variable de Tarification

        public static List<ServiceTarification.CsModeCalcul> ListeModeCalcule = new List<ServiceTarification.CsModeCalcul>();
        public static List<ServiceTarification.CsModeApplicationTarif> ListeModeApplicationTarif = new List<ServiceTarification.CsModeApplicationTarif>();
        public static List<ServiceTarification.CsUniteComptage> ListeUniteComptage = new List<ServiceTarification.CsUniteComptage>();
        #endregion

        #region TarifFacturation

        public static List<ServiceTarification.CsTarifFacturation> ListeTarifFacturation = new List<ServiceTarification.CsTarifFacturation>();
        public static List<ServiceAdministration.CsAgent> ListeDesAgents = new List<ServiceAdministration.CsAgent>();
        
        #endregion

        #endregion
        #region Ajustement

        public static List<CsLotComptClient> ListeAjustement = new List<CsLotComptClient>();
        #endregion
        //WCO 12/01/2016
        #region Workflow

        public static List<CsEtape> _ToutesLesEtapesWorkflows = new List<CsEtape>();

        #region Report
        public static string DevisValiderDelais = "21";
        public static string DevisValiderHorsDelais = "22";
        public static string TravauxValiderDelais = "31";
        public static string TravauxValiderHorsDelais = "32";
        public static string TravauxRealiser = "33";
        public static string TravauxNonRealiser = "35";
        public static string RegistreDemande = "7";
        public static string DemandeEnAttenteDeRealisation = "8";
        public static string DemandeEnAttenteLiaison = "5";


        public static string EditionProgrammation = "400010101";
        public static string EditionSortieMateriel = "400010102";
        public static string EditionSortieCompteur = "400010103";

  


        public static string AvisEmis = "400011";
        public static string AvisCoupe = "400012";
        public static string AvisRecouvre = "400015";
        public static string AvisRepose = "400013";
        public static string TauxRecouvrement = "400016";
        public static string TauxEncaissement = "400017";
        public static string MontantEncaisseControleur = "900011";
        public static string ListePreavis = "900010";
        public static string ListeMandatement = "900012";
        public static string ListePaiementMandat = "900013";
        public static string TauxMandatemant = "900014";
        public static string TauxPaiementMandat = "900015";
        public static string EmissionRegroupement = "900016";
        public static string AvanceSurConsomation = "700011";
        public static string EncaissementReversement = "700012";
        public static string EncaissementModeRegement = "700013";
        public static string Vente = "700014";
        public static string PrepaidSansAchatPeriode = "300001";
        public static string PrepaidSansJamaisAchat = "300002";

        public static string ReeditionAccuser = "400010101";
        public static string ReeditionContrat = "400010102";

        public static string ReeditionCampagne = "1510";
        public static string ReeditionMandatement = "1511";
        public static string ReeditionPaiement = "1512";
        public static string ReeditionMiseAJour = "1513";

        public static string ComptaFacturation = "50010";
        public static string RecapComptaFacturation = "50011";
        public static string StatfacturationStat = "50012";
        public static string Statfacturation  = "50001";
        public static string StatVenteCummuler = "50013";
        public static string AbonneNonConstituer = "30502";
        public static string AbonneNonSaisie = "30503";
        public static string AbonneSaisieNonFact = "30504";
        public static string AbonneFactureNonMaj = "30505";
        public static string CompteurFacturePeriode = "50007";
        public static string ReeditionProgramme = "400010104";
        public static string ReeditionSortieCompteur = "400010105";
        public static string ReeditionSortieMateriel = "400010106";
        public static string FactureIsole = "50009";
        public static string FactureAnnuler = "50008";
        public static string EncaissementCumule = "50015";
        public static string TourneePIA = "400018";

        public static string StatReclamation = "10000101";
        public static string ReclamationAgent = "10000102";
        public static string ReclamationListe = "10000103";
        public static string TauxReclamation = "10000104";

        public static string DeliaisonCompteur = "150002";
        public static string ReliaisonCompteur  = "150001";
        public static string CorrectionNumCompteur = "150003";

        public static string AvanceSurConso = "20011";
        public static string ExtractionImpayes = "20012";
        
        #endregion

        public static string FacturationClientExisteDansLot = "1";
        public static string FacturationClientCorespondPasLot = "2";
        public static string FacturationClientNonTrouve = "3";
        public static string FacturationClientValider = "4";
        #endregion

        public const string INITIAL = "NEW";
        public const string CODEINTERFACE = "2023";
        public const string NEGATIVE = "-1";
        public const string PROVENACE = "GALATEE";
        public const string NO = "NO";
        public const string ZERO = "0";
        public const string ALPHA = "A";
        public const string DEVISCOMPTA = "XOF";
        public const string OperationCampagne = "1DBD4C6E-AA6B-44B1-AE20-46A51579FA0D";
        public const string CodeEtapePgrammation = "PGM";




    }
}
