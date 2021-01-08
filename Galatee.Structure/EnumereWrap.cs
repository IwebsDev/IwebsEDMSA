
using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace Galatee.Structure
{
    [DataContract]
    public class EnumereWrap
    {
        // 
         [DataMember] public string Eau { get; set; }
         [DataMember] public string Electricite { get; set; }
         [DataMember] public string Prepaye { get; set; }
         [DataMember] public string ElectriciteMT { get; set; }
         [DataMember] public string Accueil { get; set; }
         [DataMember] public string Caisse{ get; set; }

         [DataMember] public int NombrePointFournitureMT{ get; set; }
         [DataMember] public int NombrePointFournitureBT{ get; set; }

         [DataMember] public string CoperFondCaisse { get; set; }
         [DataMember] public string CoperTransfertDebit { get; set; }
         [DataMember] public string CoperTransfertDette { get; set; }

         [DataMember] public string Generale { get; set; }
         [DataMember] public string CodeSiteScaBT { get; set; }
         [DataMember] public string CodeSiteScaMT { get; set; }

         [DataMember] public int IDGenerale { get; set; }
         [DataMember] public string DemandeScelle { get; set; }
         [DataMember] public string IDDemandeScelle { get; set; }

         [DataMember] public string Debit  { get; set; }
         [DataMember] public string Credit { get; set; }

         [DataMember] public string StatusDemandeInitier { get; set; }
         [DataMember] public string StatusDemandeValider { get; set; }
         [DataMember] public string StatusDemandeRejeter { get; set; }
         [DataMember] public string StatusDemandeRetirer { get; set; }
         [DataMember] public string ConnexionGALADB { get; set; }
         [DataMember] public string ConnexionABO07 { get; set; }



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
        public enum NumTable
        {
            Category = 12,
            MtTable = 58,
            service = 17    /* LKK le 23/08/2011*/
        }
        //Enumérés concernant des données de référence en base
        public enum EnumCompteur
        {
            Monophase = 1,
            Dial = 2,
            Triphase = 3,
            Four = 4
        }
        // NUM TABLE

        [DataMember] public int  TRANSMETTRE { get; set; }
        [DataMember] public int  REJETER { get; set; }
        [DataMember] public int  ANNULER { get; set; }
        [DataMember] public int  SUSPENDRE { get; set; }


        [DataMember] public string Monophase { get; set; }
        [DataMember] public string Dial { get; set; }
        [DataMember] public string Triphase { get; set; }
        [DataMember] public string Four { get; set; }

        [DataMember] public string Category { get; set; }
        [DataMember] public string MontantFraisTravaux { get; set; }


        //Type de cheque
         [DataMember] public string EtatChequeChequeSurPlace { get; set; }
         [DataMember] public string EtatChequeChequeHorsPlace { get; set; }

        // ModePayement
         [DataMember] public string ModePayementEspece { get; set; }
         [DataMember] public string ModePayementCheque { get; set; }
         [DataMember] public string ModePayementAjustement { get; set; }

        // EtatDeCaisse
         [DataMember] public string EtatDeCaisseInexistante { get; set; }
         [DataMember] public string EtatDeCaisseDejaCloture { get; set; }
         [DataMember] public string EtatDeCaisseNonCloture { get; set; }
         [DataMember] public string EtatDeCaisseValider { get; set; }
         [DataMember] public string EtatDeCaissePasDeNumCaiss { get; set; }
         [DataMember] public string EtatDeCaissePasCassier { get; set; }
         [DataMember] public string EtatDeCaisseOuverteALaDemande { get; set; }
         [DataMember] public string EtatDeCaisseAutreSessionOuvert { get; set; }

        // Coper
         [DataMember] public string CoperNAF { get; set; }
         [DataMember] public string CoperRGT { get; set; }
         [DataMember] public string CoperMOR { get; set; }
         [DataMember] public string CoperMorSolde { get; set; }
         [DataMember] public string CoperFact { get; set; }
         [DataMember] public string CoperRNA { get; set; }
         [DataMember] public string CoperRCD { get; set; }
         [DataMember] public string CoperOdQPA { get; set; }
         [DataMember] public string CoperOdDFA { get; set; }
         [DataMember] public string CoperOdC01 { get; set; }
         [DataMember] public string CoperFRP { get; set; }
         [DataMember] public string CoperEXO { get; set; }
         [DataMember] public string CoperCAU { get; set; }
         [DataMember] public string CoperFPO { get; set; }
         [DataMember] public string CoperFDO { get; set; }
         [DataMember] public string CoperTRV { get; set; }
         [DataMember] public string CoperFPS { get; set; }
         [DataMember] public string CoperFAB { get; set; }
         [DataMember] public  string FactureGeneraleCoper { get; set; }
         [DataMember] public  string FactureManuelleCoper { get; set; }
         [DataMember] public string CoperFPA { get; set; }
         [DataMember] public string CoperACT { get; set; }
         [DataMember] public string CoperRembAvance { get; set; }
         [DataMember] public string CoperRAC { get; set; }
         [DataMember] public string CoperRAD { get; set; }
         [DataMember] public string CoperUDC { get; set; }
         [DataMember] public string CoperRDD { get; set; }
         [DataMember] public string CoperDSD { get; set; }
         [DataMember] public string CoperEtudeEtSurveillance { get; set; }
         [DataMember] public string CoperRembTrvx { get; set; }
         [DataMember] public string CoperSoldeSuiteTransfer { get; set; }
         [DataMember] public string CoperFactureTrvxEtDivers { get; set; }
        
         [DataMember] public string CodeSansTaxe { get; set; }
        
        

        #region Fourniture
       [DataMember] public  int FournitureCable { get; set; }
        #endregion


         [DataMember] public int TailleDesReferenceClient { get; set; }
         [DataMember] public int TailleCentre { get; set; }
         [DataMember] public int TailleClient { get; set; }
         [DataMember] public int TailleOrdre { get; set; }
         [DataMember] public int TailleNumDevis { get; set; }
         [DataMember] public int TailleMatricule { get; set; }
         [DataMember] public int TaillePeriode { get; set; }
         [DataMember] public int TailleFacture { get; set; }
         [DataMember] public int TailleNumCaisse { get; set; }
         [DataMember] public int TailleCodeConso { get; set; }
         [DataMember] public int TailleCodeRelance { get; set; }
         [DataMember] public int TailleCodeCategorie { get; set; }
         [DataMember] public int TailleCodeTypeCompteur { get; set; }
         [DataMember] public int TailleCodeTypeClient { get; set; }
         [DataMember] public int TailleCodeNationalite { get; set; }
         [DataMember] public int TailleDiametre  { get; set; }
         [DataMember] public int TailleUsage  { get; set; }

         [DataMember] public int TailleCommune  { get; set; }
         [DataMember] public int TailleQuartier  { get; set; }
         [DataMember] public int TailleSecteur  { get; set; }
         [DataMember] public int TailleRue  { get; set; }

         [DataMember] public int TaillePeriodicite { get; set; }
         [DataMember] public int TailleMoisDeFacturation { get; set; }
         [DataMember] public int TailleCodeQuartier { get; set; }
        [DataMember] public int TailleCodeMarqueCompteur { get; set; }
        [DataMember] public int TailleCodeCivilite { get; set; }
        [DataMember] public int TailleTarif { get; set; }
        [DataMember] public int TailleForfait { get; set; }
        [DataMember] public int TailleCodeProduit { get; set; }
        [DataMember] public int TailleNumeroLigneBatch { get; set; }
        [DataMember] public int TailleCodeTournee { get; set; }
        [DataMember] public int TailleNumeroDemande { get; set; }
        [DataMember] public int TailleNumeroBatch { get; set; }
        [DataMember] public int TailleDate { get; set; }
        [DataMember] public int TailleCodeMateriel { get; set; }
        [DataMember] public int TailleDiametreBranchement { get; set; }
        [DataMember] public int TailleCas { get; set; }
        [DataMember] public int TailleNoeudbrt { get; set; }
        [DataMember] public int TailleSequencePost { get; set; }
        [DataMember] public int TailleNumeroPost { get; set; }
        [DataMember] public int TailleCodePoste { get; set; }
        [DataMember] public int TailleTypeComptage { get; set; }
        [DataMember] public int TailleTypeBranchement { get; set; }
        [DataMember] public int TailleCodeRegroupement { get; set; }
        [DataMember] public int TailleNumeroCheque { get; set; }
        [DataMember] public int NombreDejour { get; set; }

        [DataMember] public int NombreMois { get; set; }
        [DataMember] public string Message { get; set; }
        [DataMember] public int TailleDigitCompteur { get; set; }
        

        // CaisseStat
        [DataMember] public string CaisseStatAnnulation { get; set; }
        [DataMember] public string CaisseStatFONDCAISSE { get; set; }

        // ActionRecu

        [DataMember] public string ActionRecuAnnulation { get; set; }
        [DataMember] public string ActionRecuDuplicat { get; set; }
        [DataMember] public string ActionRecuEditionNormale { get; set; }

       // OperationDeCaisse
        [DataMember] public string OperationDeCaisseEncaissementFacture { get; set; }
        [DataMember] public string OperationDeCaisseEncaissementFactureManuel { get; set; }
        [DataMember] public string OperationDeCaisseEncaissementMoratoire { get; set; }
        [DataMember] public string OperationDeCaisseEncaissementDevis { get; set; }

        [DataMember] public bool  UtilisationAutoDeNaf { get; set; }

        //OperationDeMoratoire
        [DataMember] public string CodeFonctionCaisse { get; set; }
        [DataMember]
        public string CodeFonctionPIA_E { get; set; }
        [DataMember]
        public string CodeFonctionPIA_O { get; set; }
        [DataMember] public string CodeFonctionMetreur { get; set; }

        [DataMember] public string CompteurActif { get; set; }
        [DataMember] public string CompteurActifValeur { get; set; }
        [DataMember] public string CompteurInactif { get; set; }
        [DataMember] public string CompteurInactifValeur { get; set; }
        
        [DataMember]  public string  BranchementSimple { get; set; }
        [DataMember]  public string Reabonnement { get; set; }

        [DataMember] public string BranchementAbonement { get; set; }
        [DataMember] public string BranchementAbonementExtention { get; set; }


        [DataMember] public string AbonnementSeul { get; set; }
        [DataMember] public string ChangementCompteur { get; set; }
        [DataMember] public string FactureManuelle { get; set; }
        [DataMember]  public string AvoirConsomation { get; set; }
        [DataMember]  public string AutorisationAvancementTravaux { get; set; }
        [DataMember] public string PoseCompteur { get; set; }
        [DataMember] public string DeposeCompteur { get; set; }

        [DataMember] public string RegularisationAvance { get; set; }
        [DataMember] public string Etalonage { get; set; }
        [DataMember] public string RechercheDemandeParNum { get; set; }
        [DataMember] public string RechercheDemandeDebit { get; set; }
        [DataMember] public string RechercheDemandeCredit { get; set; }
        [DataMember] public string Resiliation { get; set; }
        [DataMember] public string FermetureBrt { get; set; }
        [DataMember] public string RemboursementTrvxNonRealise { get; set; }
        [DataMember] public string ReouvertureBrt { get; set; }
        [DataMember] public string RemboursementAvance { get; set; }
        [DataMember] public string VerificationCompteur { get; set; }

        [DataMember] public string ModificationAdresseEtBranchemant { get; set; }
        [DataMember] public string ModificationAbonementEtClient { get; set; }
        [DataMember] public string AchatTimbre { get; set; }

        [DataMember] public string ModificationAbonnement{ get; set; }
        [DataMember] public string ModificationAdresse{ get; set; }
        [DataMember] public string ModificationCompteur{ get; set; }
        [DataMember] public string ModificationBranchement{ get; set; }
        [DataMember] public string ModificationClient{ get; set; }
        [DataMember] public string CorrectionDeDonnes{ get; set; }
        [DataMember] public string ReprisIndex { get; set; }
        [DataMember] public string AnnulationFacture { get; set; }
        [DataMember] public string DimunitionPuissance { get; set; }
        [DataMember] public string AugmentationPuissance { get; set; }
        [DataMember] public string TransfertAbonnement { get; set; }
        [DataMember] public string BranchementAbonnementMt { get; set; }
        [DataMember] public string BranchementAbonnementEp { get; set; }
        [DataMember] public string DepannageEp { get; set; }
        [DataMember] public string DemandeFraude { get; set; }
        [DataMember]public string DemandeReclamation { get; set; }
        [DataMember] public string RemboursementParticipation { get; set; }
        [DataMember] public string DepannageMT { get; set; }
        [DataMember] public string DepannagePrepayer { get; set; }
        [DataMember] public string DepannageClient { get; set; }
        [DataMember] public string DepannageMaintenance { get; set; }
        [DataMember] public string TransfertSiteNonMigre { get; set; }
        [DataMember] public string ChangementProduit { get; set; }

        

        [DataMember] public string ActionCreationlot { get; set; }
        [DataMember] public string ActionEditionlot { get; set; }
        [DataMember] public string ActionSaisielot { get; set; }
        [DataMember] public string ActionFacturation { get; set; }
        [DataMember] public string PROPRIETRAIRE { get; set; }
        [DataMember] public string LOCATAIRE { get; set; }

        [DataMember] public bool IsGestionGlobaleCoutFournitureDevis { get; set; }
        [DataMember] public bool IsUtilisateurCreeParAgent { get; set; }

        [DataMember]public string  IsBranchementSubventione { get; set; }
        [DataMember]public string IsBranchementNonSubventione{ get; set; }
        [DataMember]public bool IsFacturationPartielle{ get; set; }
        [DataMember]public bool  IsCompteurAttribuerAuto{ get; set; }
        [DataMember]public bool  IsDistanceSupplementaireFacture{ get; set; }
        [DataMember]public bool  IsResilierPriseEcompte{ get; set; }

        
        
        //Status evenement
        [DataMember] public int EvenementCree { get; set; }
        [DataMember] public int EvenementReleve { get; set; }
        [DataMember] public int EvenementFacture { get; set; }
        [DataMember] public int EvenementMisAJour { get; set; }
        
        [DataMember] public int EvenementDefacture { get; set; }
        [DataMember] public int EvenementRejeter { get; set; }
        [DataMember] public int EvenementAnnule { get; set; }
        [DataMember] public int EvenementSupprimer { get; set; }
        [DataMember] public int EvenementPurger { get; set; }
        // Status pagerie

        [DataMember] public string  PagerieEnquetable { get; set; }
        [DataMember] public string  PagerieNonEnquetable { get; set; }
        [DataMember] public string PagerieConfirme { get; set; }
//Niveau de tarif
        [DataMember] public string NiveauTarif_Com { get; set; }
        [DataMember] public string NiveauTarif_Nat { get; set; }
        [DataMember] public string NiveauTarif_Cent { get; set; }

    //Mode application de tarif
        [DataMember] public string ModeApplicationTarifDate { get; set; }
        [DataMember] public string ModeApplicationTarifPeriode  { get; set; }
   //Code evenement
       [DataMember] public string EvenementCodeNormale { get; set; }

        [DataMember] public string EvenementCodeDeposeCpt { get; set; }
        [DataMember]  public string EvenementCodePoseCpt { get; set; }
        [DataMember] public string EvenementCodeFermetureBrt { get; set; }
        [DataMember] public string EvenementCodeOuvertureBrt { get; set; }
        [DataMember] public string EvenementFermetureBrtAvecDepose { get; set; }
        [DataMember] public string EvenementCodeForfait { get; set; }

        [DataMember] public string EvenementCodeSuspensionAbon { get; set; }
        [DataMember] public string EvenementCodeCreationLot { get; set; }
        [DataMember] public string EvenementCodeFactureIsole { get; set; }
        [DataMember] public string EvenementCodeResiliation { get; set; }
        [DataMember] public string EvenementCodeAvoirConso { get; set; }
        [DataMember] public string EvenementCodeRejetFacture { get; set; }
        [DataMember] public string EvenementCodeDefacturationLot { get; set; }
        [DataMember] public string EvenementCodeFactureAjustementNeg { get; set; }
        [DataMember] public string EvenementCodeFactureAjustementPos { get; set; }

        

        //Status demande
        [DataMember] public string DemandeStatusEnAttente { get; set; }
        [DataMember] public string DemandeStatusPasseeEncaisse { get; set; }
        [DataMember] public string DemandeStatusPriseEnCompte { get; set; }
        [DataMember] public string DemandeStatusEnCaisse { get; set; }
        
        //lotri evenement
        [DataMember] public string  LotriTermination { get; set; }
        [DataMember] public string  LotriManuel { get; set; }
        [DataMember] public string  LotriAjustement { get; set; }
        [DataMember]
        public string LotriChangementCompteur { get; set; }
        
        //Cas de saisie
               [DataMember] public string CasPoseCompteur { get; set; }
               [DataMember] public string CasDeposeCompteur { get; set; }
               [DataMember] public string CasCreation { get; set; }
               [DataMember] public string CasNindesSup { get; set; }
               [DataMember] public string CasPassageZero { get; set; }

        
               //operatoin recouvrement
               [DataMember] public string TraitementImpaye { get; set; }
               [DataMember] public string CoperChqImp { get; set; }
               [DataMember] public string TopCheqImpaye { get; set; }
               [DataMember] public string LibNatureCheqImpaye { get; set; }
               [DataMember]  public string MontantFraisRDC { get; set; }
               [DataMember] public string CoperFraisChqImp { get; set; }

               [DataMember] public int  IsInsertion { get; set; }
               [DataMember] public int IsUpdate { get; set; }
               [DataMember] public int IsDelete { get; set; }

               // Mode de Calcul
               [DataMember] public string FacturationNormale { get; set; }
               [DataMember] public string FacturationForfaitAvecRegul { get; set; }
               [DataMember] public string FacturationBloqueSansRegul { get; set; }
               [DataMember] public string FacturationTarifAnnuelUniquement { get; set; }
               [DataMember] public string FacturationTarifUnitaireUniquement { get; set; }
               [DataMember] public string FacturationForfaitSansRegul { get; set; }
               [DataMember] public string FacturationBloqueAvecRegul { get; set; }
               [DataMember] public string FacturationEstimerAvecRegul { get; set; }
               [DataMember] public string FacturationEstimerSanRegul { get; set; }
        /*Top LCLIENT*/
               [DataMember] public string  TopGuichet { get; set; }
               [DataMember] public string TopFacturation { get; set; }
               [DataMember] public string TopCaisse { get; set; }
               [DataMember] public string TopSaisieDeMasse { get; set; }
               [DataMember] public string TopPortables { get; set; }
               [DataMember] public string TopPaiementsDeplaces  { get; set; }
               [DataMember] public string TopMoratoire { get; set; }
        /**/

                [DataMember]public string  TypeComptageMaximetre { get; set; }
                [DataMember]public string  TypeComptageHoraire { get; set; }
                [DataMember]public string  TypeComptageReactif { get; set; }
                [DataMember]public string  TypeComptagePoint { get; set; }
                [DataMember]public string  TypeComptagePleinne { get; set; }
                [DataMember]public string  TypeComptageCreuse { get; set; }

                [DataMember]public int  BorneConsohoraire { get; set; }
        

        #region PARAMETRAGE
        
         [DataMember] public string CodeFacultatif { get; set; }
         [DataMember] public string EvenementsValeurParDefaut { get; set; }
         [DataMember] public string LibFacultatif { get; set; }
         [DataMember] public string CodeInterdit { get; set; }
         [DataMember] public string LibInterdit { get; set; }
         [DataMember] public string CodeObligatoire { get; set; }
         [DataMember] public string LibObligatoire { get; set; }

         [DataMember] public string CodeNormale { get; set; }
         [DataMember] public string LibNormale { get; set; }
         [DataMember] public string CodeForfait { get; set; }
         [DataMember] public string LibForfait { get; set; }
         [DataMember] public string CodeBloqueeSansRegularisation { get; set; }
         [DataMember] public string LibBloqueeSansRegularisation { get; set; }
         [DataMember] public string CodeSansTarifUnitaire { get; set; }
         [DataMember] public string LibSansTarifUnitaire { get; set; }
         [DataMember] public string CodeSansTarifAnnuel { get; set; }
         [DataMember] public string LibSansTarifAnnuel { get; set; }
         [DataMember] public string CodeForfaitSansRegularisation { get; set; }
         [DataMember] public string LibForfaitSansRegularisation { get; set; }
         [DataMember] public string CodeBloqueeAvecRegularisation { get; set; }
         [DataMember] public string LibBloqueeAvecRegularisation { get; set; }
         [DataMember] public string CodeEstimeeAvecRegularisation { get; set; }
         [DataMember] public string LibEstimeeAvecRegularisation { get; set; }
         [DataMember] public string CodeEstimeeSansRegularisation { get; set; }
         [DataMember] public string LibEstimeeSansRegularisation { get; set; } 
        #endregion

        // HGB - 28/02/2013 
         #region ADMINISTRATION
        [DataMember] public byte UserAcitveAccount { get; set; }
        [DataMember] public byte UserInAcitveAccount { get; set; }
        [DataMember] public byte UserLockeAcitveAccount { get; set; }
        [DataMember] public byte  UserLockeSessionAccount { get; set; }


        //HGB - 04/03/2013
        [DataMember] public byte ActionSurCentre { get; set; }
        [DataMember] public byte ActionSurSite { get; set; }
        [DataMember] public byte ActionSurTousSite { get; set; }
         #endregion

        //Action facturation
         [DataMember] public string Simulation { get; set; }
         [DataMember] public string Defacturation { get; set; }
         [DataMember] public string DestructionSimulation { get; set; }
         [DataMember] public string Normal { get; set; }
         [DataMember] public string DemandeDefacturation { get; set; }

         [DataMember] public string NOLIEN { get; set; }
         [DataMember] public string LIE_A_REDEVANCE { get; set; }
         [DataMember] public string A_DES_PARAMETRES { get; set; }
         [DataMember] public string LIE_ET_A_DES_PARAMETRES { get; set; }

        
        //
        //
         [DataMember] public string MiseAJours { get; set; }
         [DataMember] public string NonMiseAJours { get; set; }
         [DataMember] public string NonMiseAbonne { get; set; }
         [DataMember] public string SimulationFacture { get; set; }

         [DataMember] public int StatusScelleAbîmé { get; set; }
         [DataMember] public int StatusScelleRompu { get; set; }
         [DataMember] public int StatusScelleRemis { get; set; }
         [DataMember] public int StatusScelleDisponible { get; set; }
         [DataMember] public int StatusScelleTransféré { get; set; }
         [DataMember] public int StatusScellePosé { get; set; }
         [DataMember] public int StatusScelleEgaré { get; set; }

         [DataMember] public string CategorieAgentEdm { get; set; }
         [DataMember] public string CategorieEp { get; set; }
         [DataMember] public string CategorieConsoInterne { get; set; }
         [DataMember] public string CodeConsomateurIndeterminer { get; set; }
         [DataMember] public string UsageEp { get; set; }
         [DataMember] public string NatureEp { get; set; }
         [DataMember] public string CodeConsomateurEp { get; set; }
        //
         #region Scelles

             [DataMember]
             public  int ScelleAbime { get; set; }
             [DataMember]
             public  int ScelleRompu { get; set; }
             [DataMember]
             public  int ScelleRemis { get; set; }
             [DataMember]
             public  int ScelleDisponible { get; set; }
             [DataMember]
             public  int ScelleTransfere { get; set; }
             [DataMember]
             public  int ScellePose { get; set; }
             [DataMember]
             public  int ScelleEgare { get; set; }

         #endregion
         #region TypeDocument
         [DataMember] public string Preuve { get; set; }
         [DataMember] public string Schema { get; set; }
         [DataMember] public string Manuscrit { get; set; }
         [DataMember] public string PieceIdentite { get; set; }
         [DataMember] public string Contrat { get; set; }
         [DataMember] public string TitrePropriete { get; set; }
         [DataMember] public string DossierPromoteur { get; set; }
         [DataMember] public string AutorisationMairie { get; set; }
         [DataMember] public string InstructionDG { get; set; }
         [DataMember] public string DemandePrestation { get; set; }
         [DataMember] public string FicheDexoneration { get; set; }

        #endregion
         #region Rubrique
         [DataMember] public string LIGNEHTA { get; set; }
         [DataMember] public string POSTEHTABT { get; set; }
         [DataMember] public string LIGNEBT { get; set; }
         [DataMember] public string ENSEMBLECOMPTAGE { get; set; }
         [DataMember] public string DEVISBRANCHEMENT { get; set; }
         [DataMember] public string DEVISDEXTENSION { get; set; }

        #endregion

         #region EtatCompteur
         [DataMember] public string CompteurAffecte { get; set; }
         #endregion

         [DataMember] public string NationnaliteMali { get; set; }

         [DataMember] public string CheminImpressionServeur { get; set; }
         [DataMember] public string CheminImpressionClient { get; set; }
        


 


    }
}





