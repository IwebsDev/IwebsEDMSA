using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Galatee.Structure;
using System.Data;


namespace WcfService
{
    // REMARQUE : vous pouvez utiliser la commande Renommer du menu Refactoriser pour changer le nom d'interface "IAcceuilService" à la fois dans le code et le fichier de configuration.
    [ServiceContract]
    [ServiceKnownType(typeof(CsImageFile))]
    [ServiceKnownType(typeof(CsFactureClient))]
    public interface IAcceuilService
    {
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool LicenseLooking();
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool LicenceOK();
        [OperationContract]
        [FaultContract(typeof(Errors))]
        CsCanalisation VerifieSiCompteurExiste(CsCompteur Compteur);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        CsClient RetourneClient(int FK_IDCENTRE, string Centre, string Client, string ordre);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsClient> RetournelstClient(int fk_idcentre, string centre, string client, string Ordre);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsStatistiqueTravaux_Brt_Ext> RetourneStatistiqueTravaux_Brt_Ext(List<string> codeproduit, string periode);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsDonnesStatistiqueDemande> RetourneDonnesStatistiqueDemande(string codesite, List<string> codeproduit, List<string> codetypedemande, string periode);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsReclamationRcl> RetourneReclamation(int fk_idcentre, string centre, string client, string ordre, string numerodeamnde);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool ValiderReclamation(CsDemandeReclamation LaDemande);


        [OperationContract]
        [FaultContract(typeof(Errors))]
        string ValiderRepriseIndex(CsDemande laDemande);


        [OperationContract]
        [FaultContract(typeof(Errors))]
        string ValiderAnnulationFacture(CsDemande laDemande, int idEntfac);


        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsRclValidation> SelectAllValidation();
        [OperationContract]
        [FaultContract(typeof(Errors))]
        string ValiderInitReclamation(CsDemandeReclamation LaDemande);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        CsDemandeReclamation RetourDemandeReclamation(int IDDEMANDE);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsTypeReclamationRcl> SelectAllTypeReclamationRcl();
        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsModeReception> SelectAllModeReception();
        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsCompteurBta> SelectAllCompteurInDisponible();
        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsCompteurBta> SelectAllCompteur();

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsCompteurBta> SelectAllCompteurPourAffectation();

        [OperationContract]
        [FaultContract(typeof(Errors))]
        int InsertMargasinVirtuelle(List<CsCompteurBta> sCompteur, List<CsCompteurBta> sCompteur1);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsDemandeBase> RetourneListeDemandeCritere(int? Idcentre, string numdem, List<string> LstTdem, DateTime? datedebut, DateTime? dateFin,
                                                   DateTime? datedemande, string numerodebut, string numerofin, string status, string Commune, string Quatier, string Secteur, string Rue, string Porte, string Etage, string NumeroLot, string Compteur, string Nom);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool EclipseSimpleCompteurTransition(CsDemande laDemande);
        #region Accueil
        //[OperationContract]
        //[FaultContract(typeof(Errors))]
        //List<CsEvenement> RetourneEvenementCanalisationPeriode(CsAbon leAbonnement, string Periode);
        
        #region Gui00

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsSite> retourneCssite();

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsModepaiement> RetourneCodeModePayement();


        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsCategorieClient> RetourneCatCli(string key);


        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsUtilisateur> RetourneListeAllUser();

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsCentre> ListeDesDonneesDesSite(bool IsIncrementeDemandeNum);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsProduit> ListeDesProduit();

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsCommune> ChargerCommune();


        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsQuartier> ChargerLesQartiers();

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsSecteur> ChargerLesSecteurs();

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsTournee> ChargerLesTournees();

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsPosteSource> ChargerPosteSource();

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsDepart> ChargerDepartHTA();

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsPosteTransformation> ChargerPosteTransformateur();


        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsRues> ChargerLesRueDesSecteur();

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsCodeConsomateur> RetourneCodeConsomateur();

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsCategorieClient> RetourneCategorie();

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsNatureClient> RetourneNature();

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsFermable> RetourneFermable();

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsNationalite> RetourneNationnalite();

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsDenomination> RetourneListeDenominationAll();


        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsRegCli> RetourneTousCodeRegroupement();

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsReglageCompteur> ChargerReglageCompteur();

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsCalibreCompteur> ChargerCalibreCompteur();

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsCadran> RetourneToutCadran();

        [OperationContract]
        [FaultContract(typeof(Errors))]
        string NumeroFacture(int PkidCentre);


        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsMarqueCompteur> RetourneToutMarque();

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsTypeBranchement> ChargerTypeBranchement();

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsDiametreBranchement> ChargerDiamentreBranchement();

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsMaterielBranchement> RetourneMaterielBranchement();

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsTarif> ChargerTarif();

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsTarif> ChargerTarifPuissance();

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsForfait> ChargerForfait();

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsPuissance> ChargerPuissanceSouscrite();

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsPuissance> ChargerPuissanceReglageCompteur();

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsPuissance> ChargerPuissanceInstalle();

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsRubriqueDevis> ChargerRubrique();

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsTarif> ChargerTarifParReglageCompteur();

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsTarif> ChargerTarifParCategorie();



        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsMois> ChargerTousMois();

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsFrequence> ChargerTousFrequence();

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsCodeTaxeApplication> RetourneTousApplicationTaxe();

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsTcompteur> ChargerType();

       

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsTypeComptage> ChargerTypeComptage();

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsEtapeDemande> ChargerEtapeDemande();

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsSpeSite> ChargerSpecificiteSite(string _pSite);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsClientRechercher> RechercherClient(CsClientRechercher _CritereClient);


        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsAbon> RetourneAbon(int FK_IDCENTRE, string Centre, string Client, string ordre);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        CsAg RetourneAdresse(int FK_IDCENTRE, string Centre, string Client, string ordre);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsBrt> RetourneBranchement(int FK_IDCENTRE, string centre, string client, string produit);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        string RetourneOrdreMax(int fk_idcentre, string centre, string client, string produit);


        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsCoutDemande> ChargerCoutDemande();


        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool EditionCompteClient(List<CsReglement> _LeCompteClient, string key);


        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsLclient> RetourneListeFactureClient(int fk_idcentre, string Centre, string Client, string Ordre);


        //[OperationContract]
        //[FaultContract(typeof(Errors))]
        //List<CsReglement> RetourneListeReglement(string Centre, string Client, string ordre);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        //List<CsCanalisation> RetourneCanalisationClasseur(int fk_idCentre, string Centre, string Client, int fk_idAbon, string produit, int? point);
        List<CsCanalisation> RetourneCanalisationClasseur(int fk_idCentre, string Centre, string Client, List<CsAbon> lstAbon, string produit, int? point);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsCanalisation> RetourneCanalisation(int fk_idCentre, string centre, string client, string produit, int? point);

        [OperationContract]
        [FaultContract(typeof(Errors))]
          CsCanalisation RetourneCanalisationResilier(int fk_idCentre, string Centre, string Client, string produit);


        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsEvenement> RetourneEvenement(int fk_idcentre, string centre, string client, string Ordre, string produit, int point);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsEvenement> RetourneDernierEvenementFacturer(int fk_idcentre, string centre, string client, string Ordre, string produit, int? point);
 
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool ValiderDemande(CsDemande LaDemannde);

                [OperationContract]
        [FaultContract(typeof(Errors))]
        string ValiderAchatimbreDemande(CsDemande LaDemannde);


        [OperationContract]
        [FaultContract(typeof(Errors))]
        string ValiderDemandeInitailisation(CsDemande LaDemannde);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        CsDemande GeDetailByFromClient(CsClient leclient);
       
        //[OperationContract]
        //[FaultContract(typeof(Errors))]
        //CsDemande RetourneDetailClientFromRefClient(int fk_idcentre, string centre, string client, string ordre);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool ValiderRejetDemande(CsDemandeBase LaDemannde);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsCasind> RetourneListeDesCas();

        [OperationContract]
        [FaultContract(typeof(Errors))]
        CsDemande RetourneLaDemande(string centre, string numdemande, int Fk_IDcentre);

  


        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsCtax> RetourneListeTaxe();

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsTdem> RetourneOptionDemande();
       
        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsParametreBranchement> RetourneParametreBranchement();

        [OperationContract]
        [FaultContract(typeof(Errors))]
        CsDemande GetDemandeByNumIdDemande(int pIdDemandeDevis);

        //[OperationContract]
        //[FaultContract(typeof(Errors))]
        //List<CsPagisol> ValiderSuppression(CsPagisol _LePAgisol);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsRegCli> RetourneCodeRegroupement();

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsSite> RetourneTousSite();

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsDemandeBase> RetourneListeDemande(int? centre, string numdem, List<string> LstTdem, DateTime? datedebut, DateTime? dateFin,
                                                           DateTime? datedemande, string numerodebut, string numerofin, string status);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsDemandeBase> RetourneListeDemandeModification(string centre, string numdem, string LstTdem, string produit, DateTime? datedebut, string matricule);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsDemandeBase> RetourneListeDemandeClient(CsClientRechercher leClient);



        [OperationContract]
        [FaultContract(typeof(Errors))]
        CsDemandeBase RetourneDemandeClientType(CsClient leClient);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool IsDernierEvtEnFacturation(string pCentre, string pClient, string pOrdre);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsDemandeBase> RetourneListeDemandeModificationPourSuvie(string centre, string numdem, string LstTdem, string produit, DateTime? datedebut, string matricule);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        CsDemande RetourneDetailDemande(CsDemandeBase laDemande);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsEvenement> RetourneEvenementDeLaCanalisation(List<CsCanalisation> LstCanalisation);


        //[OperationContract]
        //[FaultContract(typeof(Errors))]
        //List<CsEvenement> RetourneDernierEvenementDeLaCanalisation(CsAbon leAbonnement);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        CsClasseurClient RetourneClasseurClient(CsClientRechercher _LeClientRecherche);


        //[OperationContract]
        //[FaultContract(typeof(Errors))]
        //List<CsLclient> RetourneListeFactureClient(string Centre, string Client, string Ordre);


        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsLclient> RetourneListeReglement(int fk_idcentre, string Centre, string Client, string Ordre);


        [OperationContract]
        [FaultContract(typeof(Errors))]
        CsCompteClient RetourneLeCompteClient(int fk_idcentre, string centre, string client, string ordre);


        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsCompteClient> RetourneTousLesCompteClient(int fk_idcentre, string centre, string client, List<string> ordre);


        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool EditionClasseurClient(CsClasseurClient _leClasseurClient, string key);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        CsClasseurClient RetourneModificationBrt(CsClientRechercher _LeClientRecherche);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool MAJDemande(CsDemandeBase _LaDemandeMiseAJoure);



        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsLclient> RetourneCompteClientTransfert(CsClientRechercher LeClient, string Orientation);


        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool MajPayeur(CsLePayeur _LePayeur, int Action);


        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsLePayeur> RetourneLesPayeur();


        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsCAMPAGNE> RetourneListeDesCampagne(List<int> lstCentre);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsDetailCampagne> RetourneFactureCampagne(string IdCampagen, string centre, string client, string ordre);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsTypeCoupure> RetourneTypeCoupure();

        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool InsererFraisPose(CsLclient laFacture);



        #endregion
        #region Cli330
        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsCodeControle> RetourneCodeControle();

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsTypeLot> RetourneTypeLot();

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsOrigineLot> RetourneOrigine();

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsLotCompteClient> RetourneListeDesTypeLot(string Origine, string TypeLot);


        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsDetailLot> RetourneListeDesDetailLot(int Idlot);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool ValiderSaisieBactch(CsSaisieDeMasse _LaSaisie);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        CsNatgen RetourneNatureParCoper(string Coper);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        CsCoper RetourneCoper(string Coper);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsCoper> RetourneTousCoper();

        [OperationContract]
        [FaultContract(typeof(Errors))]
        int? RetourneMaxIDlot();

        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool ValiderMiseAJourBatch(List<CsLotCompteClient> ListLot);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsDetailLot> RetourneDetailLot(List<CsLotCompteClient> LesLot);


        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsLclient> RetourneCompteAjuste(List<CsLotCompteClient> ListLot);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool PurgeLot(List<CsLotCompteClient> ListLot);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        ObjDOCUMENTSCANNE ReturneObjetScan(CsDemandeBase laDamande);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool ValiderCreationFactureIsole(List<CsEvenement> lstEvenement);

        //
        #endregion
        #region Sylla
        #region 13/05/2017
        [OperationContract]
        [FaultContract(typeof(Errors))]
        CsTypeComptage AdapterComptage(int? IDPUISSANCE, int PuissanceUtilise, int? NOMBRETRANSFORMATEUR);
        #endregion
        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsTypeDOCUMENTSCANNE> ChargerTypeDocument();

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsCATEGORIECLIENT_TYPECLIENT> ChargerCategorieClient_TypeClient();
        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsNATURECLIENT_TYPECLIENT> ChargerNatureClient_TypeClient();
        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsUSAGE_NATURECLIENT> ChargerUsage_NatureClient();
        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsCATEGORIECLIENT_USAGE> ChargerCategorieClient_Usage();
        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsProprietaire> RemplirProprietaire();

        [OperationContract]
        [FaultContract(typeof(Errors))]
        CsCriteresDevis GetParametresDistance(string pMaxi, string pSeuil, string pMaxiSubvention);

        #region Table APPAREILS

        [OperationContract]
        [FaultContract(typeof(Errors))]
        ObjAPPAREILS GetAppareilByCodeAppareil(int pCodeAppareil);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<ObjAPPAREILS> GetAllAppareil();

        #endregion


        #region 12/01/2016

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsDemande> GetDemandeByListeNumIdDemande(List<int> pIdDemandeDevis);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsProgarmmation> RetourneProgrammation();

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsSortieMateriel> RetourneSortieMateriel();

        [OperationContract]
        [FaultContract(typeof(Errors))]
        CsTdem RetourneTypeDemandeFromIdEtapeWkf(int idEtape);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool DesactivationProgrammation(List<int> pIdDemandeDevis);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool MiseAJourElementDevis(List<CsDemande> LstDemannde);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsDemandeBase> RetourneListeDemandeAvalider(int IdReleveur);
        #endregion

        #endregion
        #region fomba 29/10/2015

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsDemandeBase> RetourneListeDemandeById(List<int> lesdemande);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsDemandeBase> RetourneListeDemandeEtapeById(List<int> lesdemande, int IdTypeDemande);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsDemandeBase> RetourneListeDemandeByIdSansClient(List<int> demandes, int IdTypeDemande);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsCompteur> RetourneListeCompteurLabo();

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsCompteur> RetourneListeCompteurMagasin(List<string> lstCodeSite);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsGroupe> RetourneListeGroupe(string codecentre);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsSortieMateriel> RetourneListeSortieMaterielLivre(int iddemande);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsSortieAutreMateriel> RetourneListeSortieAutreMaterielLivre(int iddemande);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool InsertCompteurMagasin(List<CsCompteur> lstCompt);

        //[OperationContract]
        //[FaultContract(typeof(Errors))]
        //bool InsertLiaisonCanalisation(List<CsDemande> lstDemande);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsDemandeBase> InsertLiaisonCompteur(List<CsDemandeBase> lstDemandeCanalisation);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool InsertCompteurEvenementCannalisation(CsDemandeBase _LaDemande, CsCanalisation leCompteur, CsEvenement leEvt);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        CsDemande InsertCompteurEvenementCannalisationMt(CsDemande _LaDemande);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool InsertGroupe(CsGroupe legroupe, List<CsUtilisateur> lstAgent);

        //[OperationContract]
        //[FaultContract(typeof(Errors))]
        //string InsertProgrammation(Guid idgroupe, List<CsDemandeBase> lstDemande, DateTime pdate);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool InsertSortieMaterielEP(int IdLivreur, int IdRecepteur, List<CsDemandeBase> lstCompteurValide, bool IsExtension);

        //[OperationContract]
        //[FaultContract(typeof(Errors))]
        //bool InsertSortieMateriel(int IdLivreur, int IdRecepteur, List<CsCanalisation> lstCompteurValide, bool IsExtension);

        //[OperationContract]
        //[FaultContract(typeof(Errors))]
        //bool InsertSortieCompteur(int IdLivreur, int IdRecepteur, List<CsCanalisation> lstCompteur);

        #endregion
        #region Table PIECEIDENTITE
        [OperationContract]
        [FaultContract(typeof(Errors))]
        ObjPIECEIDENTITE GetPieceIdentiteById(int id);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<ObjPIECEIDENTITE> GetAllPieceIdentite();


        #endregion
        #region Table TYPECLIENT

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsTypeClient> GetAllTypeClient();
        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsUsage> GetAllUsage();
        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsStatutJuridique> GetAllStatutJuridique();
        #endregion

        [OperationContract]
        [FaultContract(typeof(Errors))]
        CsContrat EditerContrat();
 
        [OperationContract]
        [FaultContract(typeof(Errors))]
        CsEditionDevis EditionDevis();

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<ObjELEMENTDEVIS> SelectAllMateriel();

        [OperationContract]
        [FaultContract(typeof(Errors))]
        CsDemande GetDevisByNumDemande(string pNumDemande);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        CsDemande ChargerDetailDemandeConsultation(int pIdDemandeDevis);

        //[OperationContract]
        //[FaultContract(typeof(Errors))]
        //List<CsCanalisation> GetDemandeByListeNumIdDemandeSortieCompteur(DateTime? DateProgramme, Guid IdEquipe, int? EtapeActuelle);

        //[OperationContract]
        //[FaultContract(typeof(Errors))]
        //List<CsCanalisation> GetDemandeByListeNumIdDemandeSortieMateriel(DateTime? DateProgramme, Guid IdEquipe, int? EtapeActuelle);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        Dictionary<List<CsClientRechercher>, List<CsLclient>> RechercherClientRegrouper(CsRegCli leRegroupement);

  
        #endregion

        #region Devis
        #region CR tRAVAUX
        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsOrganeScellable> LoadListeOrganeScellable(int FK_IDTDEM,int FK_IDPRODUIT);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsOrganeScelleDemande> LoadListerganeScelleDemande(int FK_IDTDEM);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<Galatee.Structure.CsScelle> LoadListeScelle();
        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<Galatee.Structure.CsScelle> LoadListeScelles(int IdAgentRec, int fk_TypeDemande);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<Galatee.Structure.CsScelle> LoadListeScellesDemande(int IdAgentRec, int fk_TypeDemande, int Activite_ID);
        #endregion
        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsEvenement> RetourneEvenementClient(CsClient LeClient);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        CsEvenement VerifieEvenementNonFacturer(CsEvenement leEvt);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool  ClotureValiderDemande(CsDemande LaDemannde);
        #region Scelle
        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsActivite> RetourneListeActivite();


        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsCouleurActivite> RetourneListeCouleurScelle(int Activite_ID);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsDscelle> RetourneListeDemandeScelle(int fk_dem);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsLotScelle> RetourneListeScelle();

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsLotScelle> RetourneListeScelleByCentre(int IdCentreRecuperationDeLot);


        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsDetailAffectationScelle> RetourneListeDetailAffectationScelle(int IdDemande);


        [OperationContract]
        [FaultContract(typeof(Errors))]
        CsInfoDemandeWorkflow RetourneInfoDemandeWkf(CsDemandeBase laDemande);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        CsInfoDemandeWorkflow RetourneInfoDemandeWkfByIdDemande(int pIdDemandeDevis);
        #endregion

        #region DEVIS

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<ObjDEVIS> GetDevisByIdEtapeDevis(int pIdEtapeDevis);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        ObjDEVIS GetDevisByNumDevis(string pNumDevis);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        ObjDEVIS GetDevisByDevisId(int pDevisId);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<ObjDEVIS> GetAllDevis();
        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<ObjDEVIS> GetDevisByCentre(String centre);
   
        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<ObjDEVIS> GetDevisByIdPieceIdentite(int idPieceIdentite);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<ObjDEVIS> GetDevisByCodeProduit(int codeProduit);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<ObjDEVIS> GetDevisBySite(int site);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<ObjDEVIS> GetDevisByIdTypeDevis(int idTypeDevis);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<ObjDEVIS> GetDevisByIsAnalysed(Boolean isAnalysed);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<ObjDEVIS> GetDevisByCodeProduitIdTypeDevis(int codeProduit, int idTypeDevis);

        #region AKO
        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsDemandeBase> GetDevisByNumEtape(int pNumEtape);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        CsDemande GetDevisByNumIdDevis(int pNumEtape);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsLclient> ChargerCompteDeResiliation(CsClient leClient);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsLclient> ChargerFraisParticipation(CsClient leClient);
        #endregion

        #region Table MATRICULE
        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsUtilisateur> GetUserByIdFonctionMatriculeNom(string pIdFonction, string pMatriculeAgent, string pNomAgent);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsUtilisateur> GetUserByIdGroupeValidationMatriculeNom(Guid pIdGroupeValidation, int IdCentreDemande, string CodeProfil, string Matricule, string NomAgent);
        #endregion

        #region Table DENOMINATION
        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsDenomination> SelectAllDenomination();
        #endregion

        #region Table PRODUIT
        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsProduit> SelectAllProduit();
        [OperationContract]
        [FaultContract(typeof(Errors))]
        CsProduit SelectProduitByProduitId(int pProduitId);

        #endregion

        #region Table CATEGORIECLIENT
        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsCategorieClient> SelectAllCategorieClient();
        #endregion

        #region Table PARAMETRESGENERAUX
        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsParametresGeneraux> SelectAllParametresGeneraux();
        [OperationContract]
        [FaultContract(typeof(Errors))]
        CsParametresGeneraux SelectParametresGenerauxByCode(string pCode);

        #endregion

        #region NATIONALITE
        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsNationalite> SelectAllNationalite();
        #endregion

        #region Table CENTRE

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsCentre> SelectAllCentre();

        [OperationContract]
        [FaultContract(typeof(Errors))]
        CsCentre SelectCentreByIdSiteIdCentre(int pIdSite, int pIdCentre);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsCentre> SelectCentreBySiteId(int pSiteId);
        #endregion

        #region Table SITE
        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsSite> SelectAllSites();
        #endregion

        #region Table TYPECENTRE
        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsTypeCentre> SelectAllTypeCentre();
        #endregion

        #region Table TYPEDEVIS

        [OperationContract]
        [FaultContract(typeof(Errors))]
        ObjTYPEDEVIS GetTypeDevisById(int id);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<ObjTYPEDEVIS> GetTypeDevisByProduitId(int IdProduit);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<ObjTYPEDEVIS> GetAllTypeDevis();
        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsTdem> GetAllTypeDemande();
        #endregion

        #region Table TACHEDEVIS

        [OperationContract]
        [FaultContract(typeof(Errors))]
        ObjTACHEDEVIS GetTacheDevisById(int id);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<ObjTACHEDEVIS> GetAllTacheDevis();
        #endregion

        #region Table FONCTION
        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsFonction> SelectAllFonction();
        [OperationContract]
        [FaultContract(typeof(Errors))]
        CsFonction SelectFonctionByCode(int pCodeFonction);
        #endregion

        #region Table COMMUNE

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsCommune> SelectAllCommune();

        #endregion

        #region Table QUARTIER

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsQuartier> SelectAllQuartier();
        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsQuartier> SelectAllQuartierByCommune(CsCommune pCommune);
        #endregion

        #region Table RUES

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsRues> SelectAllRues();
        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsRues> SelectAllRuesByCommune(CsCommune pCommune);



        #endregion

        #region Table DIACOMP

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsCalibreCompteur> SelectAllCalibreCompteur();

        #endregion

        #region SUIVIDEVIS
        [OperationContract]
        [FaultContract(typeof(Errors))]
        ObjSUIVIDEVIS GetSuiviDevisByDevisIdEtape(int num, int etape);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<ObjSUIVIDEVIS> GetSuiviDevisByDevisId(CsCriteresDevis pCriteresDevis);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        ObjSUIVIDEVIS GetSuiviDevisByDevisIdProduitIdEtape(int num, string pProduit, int pIdEtape);

        #endregion

        #region DEPOSIT
        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<ObjDEPOSIT> ListeDepot();

        [OperationContract]
        [FaultContract(typeof(Errors))]
        ObjDEPOSIT SearchByNumDevis(string numDevis);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<ObjDEPOSIT> SearchByReceipt(string receipt);

        #endregion

        #region DOCUMENTSCANNE
        [OperationContract]
        [FaultContract(typeof(Errors))]
        ObjDOCUMENTSCANNE GetDocumentScanneById(Guid pId);

        #endregion

        #region DEVISPIA
        [OperationContract]
        [FaultContract(typeof(Errors))]
        ObjDEVISPIA GetDevisPiaByDevisIdOrdre(int pDevisId, int ordre);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        ObjDEVISPIA GetDevisPiaById(int pId);
        #endregion

        #region FOURNITURE

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<ObjFOURNITURE> SelectFournituresByCodeProduitByIdTypeDevis(int pCodeProduit, int pIdTypeDevis, string pDiametre);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        ObjFOURNITURE SelectFournituresByNumFourniture(int pNumFourniture);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool DeleteFourniture(List<ObjFOURNITURE> entityCollection);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<ObjFOURNITURE> GetAllFourniture();

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<ObjFOURNITURE> GetFournitureByIdTypeDevis(int idTypeDevis);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        ObjFOURNITURE GetFournitureByNumFourniture(int numFourniture);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool InsertFourniture(List<ObjFOURNITURE> entityCollection);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool UpdateFourniture(List<ObjFOURNITURE> entityCollection);

        #endregion

        #region ELEMENTSDEVIS

        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool InsertUnElementsDevis(ObjELEMENTDEVIS entity);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool InsertUneListeElementsDevis(List<ObjELEMENTDEVIS> entityCollection);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool UpdateUnElementsDevis(ObjELEMENTDEVIS entity);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool UpdateUneListeElementsDevis(List<ObjELEMENTDEVIS> entityCollection);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool DeleteUnElementsDevis(ObjELEMENTDEVIS entity);

        //[OperationContract]
        //[FaultContract(typeof(Errors))]
        //bool UpdateElementsDevisConsomme(List<ObjELEMENTDEVIS> _lElements, byte ordre);


        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool UpdateElementsDevisRemisEnStock(List<ObjELEMENTDEVIS> _lElements);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool UpdateElementsDevisValide(List<ObjELEMENTDEVIS> _lElements);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<ObjELEMENTDEVIS> SelectElementsDevisByDevisId(int pDevisId, int ordre, bool isSummary);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<ObjELEMENTDEVIS> SelectElementsDevisConsommeByDevisId(int pDevisId, int ordre);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<ObjELEMENTDEVIS> SelectElementsDevisByDevisIdForValidationRemiseStock(int pDevisId, int ordre, bool isSummary);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool UpdateOrInsertElementsDeDevis(List<ObjELEMENTDEVIS> entityCollection, ObjMATRICULE pAgent);

        //[OperationContract]
        //[FaultContract(typeof(Errors))]
        //List<ObjELEMENTDEVIS> SelectElementsDevisComparaisonByNumDevis(string numDevis, byte ordre);

        //[OperationContract]
        //[FaultContract(typeof(Errors))]
        //byte? SelectElementsDevisMaxOrdre(string numDevis);

        #endregion

        #region Table CTAX
        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsCtax> GetAllCtax();
        [OperationContract]
        [FaultContract(typeof(Errors))]
        CsCtax GetCtaxByCENTRECTAXDEBUTAPPLICATION(CsCtax entity);
        #endregion

        #region Table BANQUE
        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<aBanque> SelectAllBanque();
        #endregion

        #region Table MARQUECOMPTEUR
        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsMarqueCompteur> SelectAllMarqueCompteur();
        #endregion

        #region Table TCOMPT
        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsTcompteur> SelectAllTcompt();
        #endregion

        #region Table INTERVENTIONPLANNIFIEE
        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsInterventionPlannifiee> GetByResponsable(string pMatricule);
        #endregion

        #region TRAVAUXDEVIS
        [OperationContract]
        [FaultContract(typeof(Errors))]
        ObjTRAVAUXDEVIS SelectTravaux(int pDevisId, int Ordre);
        #endregion

        #region CONTROLETRAVAUX
        [OperationContract]
        [FaultContract(typeof(Errors))]
        CsControleTravaux SelectControles(int pDevisId, int Ordre);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool InsertControl(CsControleTravaux controle);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool Updatecontroles(CsControleTravaux controle);
        #endregion

        #region PUISSANCE
        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsPuissance> GetPuissanceParProduit(string pProduit);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsPuissance> GetPuissanceParProduitId(int pProduitId);
        #endregion

        #region ENTREPRISE

        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool DeleteEntreprise(CsEntreprise entity);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        CsEntreprise GetAllEntreprise();

        [OperationContract]
        [FaultContract(typeof(Errors))]
        CsEntreprise GetEntrepriseById(CsEntreprise entity);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool InsertEntreprise(CsEntreprise entity);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool UpdateEntreprise(CsEntreprise entity);

        #endregion

        #region PARAMETRE
        [OperationContract]
        [FaultContract(typeof(Errors))]
        CParametre RetourneParametre();
        #endregion

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsCommune> ChargerLesBrancheDesCommune();

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsQuartier> ChargerLesQartiersDesCommune();

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsSecteur> ChargerLesSecteursDesQuartier();

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsTournee> ChargerLesTourneesDesSecteur();

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsReglageCompteur> ChargerRegalgeCompteur();


        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<ObjAPPAREILSDEVIS> GetAppareilByDevis(string NumDevis);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool ValiderFacturation(List<CsFactureBrut> laFacturation, bool IsFactureResil);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsFactureBrut> CalculFactureResilation(List<CsEvenement> lstEvenement);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsFactureClient> EditionFactureResiliation(List<CsFactureBrut> laFacturation);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool AnnulerFactureResiliation(List<CsFactureBrut> laFacturation);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool AutorisationDemande(List<CsDemandeDetailCout> leFacturation);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsClient> RetourneClientByReference(string client, List<int> idCentre);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsClient> RetourneClientByReferenceOrdre(string client, string Ordre, List<int> idCentre);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        string ValiderDemandeRemboursementAvance(CsDemande LaDemannde, List<CsLclient> lesFacture);


        [OperationContract]
        [FaultContract(typeof(Errors))]
        string ValiderDemandeTransfert(CsDemande LaDemannde);


        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool LettrageSuiteAPDP(CsDemande LaDemande);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsTypeComptage> RetourneTypeComptage(int? nbrtransfo, int puissanceSouscrite, int puissanceInstalle);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<ObjELEMENTDEVIS> RetourneElementDEvisFromIdDemande(List<int> idDemande);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsCanalisation> RetourneAncienCompteur(CsDemandeBase laDemande);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsOrganeScelleDemande> RetourneScellage(int fk_idbranchement);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsOrganeScelleDemande> RetourneScellageCompteur(string numero, string marque);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsTarifClient> RetourneTarifClient(int idcentre, int idcategorie, int idreglageCompteur, int? idtypecomptage, string propriotaire, int idproduit);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsModeCommunication> RetourneModeCommunication();

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsVehicule> RetourneVehicule();

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsGroupeDepannageCommune> RetourneGroupeDepannageCommune();

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsTypePanne> RetourneTypePanne();

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsTypePanne> RetourneDetailTypePanne();

        [OperationContract]
        [FaultContract(typeof(Errors))]
        CsClient VerifieMatriculeAgent(string MatriculeAgent);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsRubriqueDevis> RetourneTypeMateriel();
        #endregion
        #endregion
        #region Transition
        [OperationContract]
        [FaultContract(typeof(Errors))]
        CsDemande GetDemandeByRefClient(string pNumDemandeDevis);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool ValiderCreation(CsDemande pDemandeDevis);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        CsDemandeBase ValiderMiseAjourDemandeTransition(CsDemande pDemandeDevis);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsEvenement> RetourneDernierEvtFacture(CsDemandeBase laDemande);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsDemande> GetDemandeByTypdeDemande(string Typedemande);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool? InsererCompteurEvtTransition(CsDemande demande);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool InsertionLigneComptableGenerer(List<CsEcritureComptable> LigneComptable);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsEcritureComptable> IsOperationExiste(List<CsEcritureComptable> LigneComptable);
	    #endregion

        #region Sylla 09/01/2017
        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsFactureClient> returnCsFactureClient();

        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool VerifieCompteurExisteNew(CsCompteur leCpt);
        #endregion

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsMandatementGc> MandatementClient(CsRegCli leClient);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        CsInfoDemandeWorkflow RetourneEtapeDemande(CsDemandeBase laDemande);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        CsAbon VerifierMatriculeAgent(string matricule);


        #region scelle
        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsScelle> RetourneScellesListeByAgence(int IdCentre);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        int InsertRemise(List<CsRemiseScelles> sRemi);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsTbLot> RetourneLotDeScelleAffectation(int IdCentre);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        string VerificationDemandeTransfert(CsDtransfert demandetrf, CsDemandeBase laDemande);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsProgarmmation> ChargerListeProgram(List<int> lstIdCentre, string NumerodeProgramme, DateTime? DateDebut, DateTime? DateFin, string IdEquipe, int idEtape, bool IsCompteur);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsProgarmmation> ChargerListeProgramReedition(List<int> lstIdCentre, string NumerodeProgramme, DateTime? DateDebut, DateTime? DateFin, string IdEquipe, int TypeEtat);

        //[OperationContract]
        //[FaultContract(typeof(Errors))]
        //List<ObjELEMENTDEVIS> ChargerListeDonneeProgramReedition(string NumProgramme, DateTime? DateDebut, Guid IdEquipe);

        //[OperationContract]
        //[FaultContract(typeof(Errors))]
        //List<CsTarif> ChargerTypeTarif(int produit, int? puissanceSouscrite, int? Categorie, int? IdReglage);


        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsTarif> ChargerTypeTarif(int produit, int? puissanceSouscrite, int? Categorie, int? IdReglage, int? idTarif);



        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool SupprimerDevenement(CsDemande laDemande);


        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool DeLiaisonCompteur(CsDemande lstDemandeCanalisation);


        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsCanalisation> DeLiaisonCompteuriWEBS(CsDemande lstDemandeCanalisation, bool isDefectueux, bool isDoubleLiaison);


        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool ReLiaisonCompteurSimple(CsDemande lstDemandeCanalisation);
        #endregion

        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool ValiderActionSurDemande(CsDemande laDemande);


        #region ADO .NET

        [OperationContract]
        [FaultContract(typeof(Errors))]
        Dictionary<CsGroupeValidation, List<CsUtilisateur>> ActeurEtape(int IdEtape, int Iddemande);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        CsDemandeBase CreeDemande(CsDemande LaDemande, bool AvecTransmission);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        string TransmissionDemande(string NUMDEM, int idEtapeActuel, string MATRICULE);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        string TransmettreDemande(List<CsDemandeBase> pDemande);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        string AffecterDemande(List<CsAffectationDemandeUser> lesAffectations);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        string ValiderMetre(CsDemande laDemande, bool AvecTransmission);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        string ValiderEtablissementDevis(CsDemande laDemande, bool AvecTransmission);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        string ValidationDemande(CsDemande laDemande, bool AvecTransmission);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        string VerificationDemande(CsDemande laDemande, bool AvecTransmission);


        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool VerifierRemboursementEnCours(CsDemande laDemande);
        

        [OperationContract]
        [FaultContract(typeof(Errors))]
        string ValiderDevis(CsDemande laDemande, bool AvecTransmission);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        string ValiderInformationAbonnement(CsDemande laDemande, bool AvecTransmission);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        string InsertProgrammation(Guid idgroupe, List<CsDemandeBase> lstDemande, DateTime pdate);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        string InsertSortieMateriel(int IdLivreur, int IdRecepteur, int idEtape, List<CsCanalisation> lstCompteurValide, bool IsExtension);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        string InsertSortieCompteur(string programme, int IdLivreur, int IdRecepteur, int idEtape, List<CsCanalisation> lstCompteur);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        string ProcesVerbal(CsDemande laDemande, bool AvecTransmission);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<ObjELEMENTDEVIS> ChargerListeDonneeProgramReedition(string NumProgramme);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsCanalisation> GetDemandeByListeNumIdDemandeSortieCompteur(string NumeroProgramme, int? EtapeActuelle);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsCanalisation> GetDemandeByListeNumIdDemandeSortieMateriel(string NumeroProgramme, int? EtapeActuelle);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        CsDemande ChargerDetailClient(CsClient leclient);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        CsDemande ChargerDetailDemande(int idDemande, string NumDemande);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        ObjDOCUMENTSCANNE DocumentScanneContenu(ObjDOCUMENTSCANNE LeDocument);
        //List<ObjDOCUMENTSCANNE> DocumentScanneContenu(ObjDOCUMENTSCANNE LeDocument);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        CsLotri RetourneLotri();
        #endregion



        [OperationContract]
        [FaultContract(typeof(Errors))]
        string CreationDemandeSuiteRejet(CsDemande LaDemande, bool AvecTransmission);



        [OperationContract]
        [FaultContract(typeof(Errors))]
        string CreeDemandeExtension(CsDemandeBase _LaDemandeMiseAJoure);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        string AnnulationDemande(CsDemandeBase _LaDemandeMiseAJoure);



        [OperationContract]
        [FaultContract(typeof(Errors))]
        string ValidationReception(List<CsDetailAffectationScelle> ListeScelle, string MatAgent, int idetapeActuelle, string numdem);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        string InsertDemandeScelle(CsDemandeBase lademande, CsDscelle dscelle);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        string InsertAffectionScelle(int id_lademande,string NUMDEM, int IdUser, int idEtapeActuelle, string Matricule, List<CsLotScelle> LstLotScelle);


        [OperationContract]
        [FaultContract(typeof(Errors))]
        string ValiderOrdreDeTravail(CsDemande laDemande, CsAffectationDemandeUser leAffection, bool AvecTransmission);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        string ValiderSuivieTravaux(CsDemande laDemande, bool AvecTransmission);


        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool SupprimerPieceJointe(ObjDOCUMENTSCANNE piece);

       

    }
}
