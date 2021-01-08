using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Galatee.Structure;
using Galatee.DataAccess;

namespace WcfService
{
    // REMARQUE : vous pouvez utiliser la commande Renommer du menu Refactoriser pour changer le nom d'interface "IRecouvrementService" à la fois dans le code et le fichier de configuration.
    [ServiceContract]
    public interface IRecouvrementService
    {

        #region Ajustement
        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsLotComptClient> LoadAllAjustement();

        [OperationContract]
        [FaultContract(typeof(Errors))]
        int SaveAjustement(List<CsLotComptClient> ListeAjustementToUpdate, List<CsLotComptClient> ListeAjustementToInserte, List<CsLotComptClient> ListeAjustementToDelete);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsLclient> MiseAjourComptAjustement(List<CsDetailLot> lstDetailPaiement, int Id);
        #endregion

        #region PNT
 [OperationContract]
        [FaultContract(typeof(Errors))]
        bool InsertCampagneBTA(CsCampagnesBTAAccessiblesParLUO CampBAT);

 [OperationContract]
 [FaultContract(typeof(Errors))]
 List<CsCampagneGc> RechercheCampane(string numerocamp);

 [OperationContract]
 [FaultContract(typeof(Errors))]
 List<CsLclient> RechercheDetailCampane(string numerocamp);

 [OperationContract]
 [FaultContract(typeof(Errors))]
 List<CsLclient> RechercheMandatemant(string numeroMandatement);

 [OperationContract]
 [FaultContract(typeof(Errors))]
 List<CsLclient> RechercheMiseAJour(string numeroAvisCredit);

 [OperationContract]
 [FaultContract(typeof(Errors))]
 List<CsLclient> RecherchePaiement(string numeroAvisCredit);

     [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsCampagnesBTAAccessiblesParLUO> GetCampagneBTAControle();


        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<Galatee.Structure.Rpnt.CsRefMethodesDeDetectionClientsBTA> GetMethodeDetectionBTA();

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsCategorieClient> GetTypeClient();

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsTypeTarif> GetTypeTarif();

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsReleveur> GetAgentZont();

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsGroupeDeFacturation> GetGroupeFacture();

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<string> GetMois();

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsTcompteur> GetTypeCompteur();


        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsClient> GetClientEligible(string CodeMethode, int FkidCentre, string CodeTypeClient, string CodeTypeTarif, string CodeTypeCompteur, string CodeAgentZone, string CodeGroupe, string nombremois, string txt_comparaison_periode1, string txt_comparaison_periode2, string txt_Code_Cas, double Nombre_Recurence, double pourcentage);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsBrt> GetBranchementBTAControle(List<CsClient> ClientSelection);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool SaveCampagneElement(List<CsCampagnesBTAAccessiblesParLUO> ListeCampBAT);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsClient> GetClienteBTADuLotControle(Galatee.Structure.Rpnt.CstbLotsDeControleBTA Lot);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool InitialisationFraud(List<Galatee.Structure.Rpnt.CstbElementsLotDeControleBTA> ListElementLot);

        #endregion

        #region SHARED 

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsLclient > RetourneListeFactureMoratoire(string centre, string Client, string ordre, int pkid);


        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsClient> TestClientExist(string centre, string Client, string ordre);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsMotifChequeImpaye> RetourneMotifChequeImpaye();

        #endregion

        #region RECOUVREMENT
        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsClient> LoadClientCampgne(string Idcoupure);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsCAMPAGNE> RetourneCampagneDelaitleCoupure(List<int> lstCentre);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsDetailCampagne> EditerCLientaPoser(DateTime? dateDebut, DateTime? Datefin);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsDetailCampagne> RetourneDonneePaiementFraisCampagne(DateTime? dateDebut, DateTime? Datefin);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsDetailCampagne> RetourneClientPourLettreRealnce(List<CsCAMPAGNE> lesCampagnes);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool SaveAffectationTourne(List<CsTournee> ListeDesTourneAAffecter);


        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsDetailCampagne> RechercherClientCampagnePourRDV(CsCAMPAGNE Campagne, CsClient leClientRech);


        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool? InsertDetailMoratoire(List<CsLclient> ListeMoratoire, List<CsLclient> listeFacture);

      
        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsDetailCampagne> RetourneDonneeIndexSaisie(CsCAMPAGNE Campagne, CsClient leClientRech);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsDetailCampagne> RetourneDonneeAnnulationFrais(CsCAMPAGNE Campagne, CsClient leClientRech);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool ValidationAnnulationFrais(List<CsDetailCampagne> ListClientCampagne);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool ValidationAutorisationFrais(List<CsDetailCampagne> ListClientCampagne);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool ValidationReposeCompteur(List<CsDetailCampagne> campagne);

        //[OperationContract]
        //[FaultContract(typeof(Errors))]
        //bool InsererFraisPose(List<CsLclient> lesFacture);

     
        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsCAMPAGNE> RetourneDonneesReeditionAvisCoupure(List<int> lstCentre);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsCAMPAGNE> RetourneDonneesSaisieIndexAvisCoupure(List<int> lstCentre);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool? InsertChecqueImpayes(List<CsLclient> LignecompteClient);

 

        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool? SuppressionMoratoireDuClient(int Idmoratoire);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsDetailMoratoire> VerifiePaiementMoratoire(int IdMoratoire);


        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool MiseAJourMoratoire(int Idmoratoire);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsDetailMoratoire> RetourneMoratoireDuClient(string centre, string client, string ordre);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsDetailCampagne> ListeDesChequesImpayes(List<int> idcentre, DateTime? DateDebut, DateTime? DateFin, int Nbre);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsDetailCampagne> EditionClientAReposer(List<CsCAMPAGNE> lstCampagne);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsLclient> RetourneClientsDuChecqhe(string numChq, string banque,string guichet);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        CsLclient VerifieChequeSaisie(string numChq, string banque);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsDetailCampagne> RechercherSuiviCampagne(List<CsCAMPAGNE> lesCampagnes);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsDetailCampagne> ControleCampagne(List<CsCAMPAGNE> lesCampagnes);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsDetailCampagne> ClientAFactureCampagne(List<CsCAMPAGNE> lesCampagnes);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        string RetourneNumFactureNaf(int Pkidcentre);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsCentre> SelectCentreCampagne();

        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool? UpdateIndex(CsDetailCampagne campagne);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool?  InsertIndex(CsDetailCampagne campagne);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool? InsertIndexSGC(CsDetailCampagne campagne);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        string InsertListIndex(List<CsDetailCampagne> campagne);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<aCampagne> RechercherCampagneParCoupure(CsCAMPAGNE Campagne, CsClient leClientRech);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsAvisDeCoupureClient > returnAvisReedtionCoupure(CsCAMPAGNE laCampagne,bool IsListe);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsTournee> RetourneTourneePIA(List<int> lstIdCentre);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsTournee> RetourneTourneeParPIA(string CodeSite);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsUtilisateur> RetournePIAAgence(string CodeSite);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsTournee> RetourneTourneeFromCampagne(List<CsCAMPAGNE> lstCampagne);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsUtilisateur> RetourneListeUtiliasteurPia(int IdCentre, string CodeFonction);


        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsAvisDeCoupureClient> TraitementAvisCoupure(CsAvisCoupureEdition AvisCoupure, aDisconnection dis, bool IsListe);


        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsLclient > TraitementAvisCoupureGC(CsAvisCoupureEdition AvisCoupure, aDisconnection dis, bool IsListe);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsDetailCampagnePrecontentieux> CREE_CAMPAGNE_PRECONTENTIEUX(int idcentre, DateTime? DateDebut, DateTime? DateFin, decimal SoldeDu, int Fk_idMatricule, string matricule);
        
        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsCategorieClient> RetourneCategorie();


        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsTournee> RetourneTournee();

        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool SaveRDVCoupure(List<CsDetailCampagne> lstClientRDV);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool SaveRDVCoupureHorsCampagne(CsClient leClient, DateTime DateRendezVous);


        [OperationContract]
        [FaultContract(typeof(Errors))]
        CsCodeConsomateur retourneCodeConso();

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsSite> RetourneSite();

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsDetailCampagne> RetourneCampagneRendezVousCoupure(List<int> lstCentre, string idCampagne);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsDetailCampagne> RetourneCampagneReglementCoupure(List<int> lstCentre, string idCampagne);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        CsLclient VerifieFraisDejaSaisi(CsDetailCampagne leClient);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsLclient> RetourneListeFactureNonSoldePourMoratoire(CsClient leClient);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsDetailCampagne> ListeDesClientEnRDV(string CodeSite, DateTime? DateDebut, DateTime? DateFin);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsDetailCampagne> ListeDesClientIndexSaisi(List<CsCAMPAGNE> lstCampagne);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsDetailCampagne> ListeDesClientFraisSaisi(List<CsCAMPAGNE> lstCampagne);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsDetailCampagne> ListeDesMauvaisPayer(List<int> lstIdCentre, DateTime? Datedebut, DateTime? Datefin);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsDetailCampagne> ListeDesMoratoiresNonRespecte(List<int> lstIdCentre, DateTime? Datedebut, DateTime? Datefin);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsDetailMoratoire> ListeDesMoratoiresEmis(List<int> lstIdCentre, DateTime? Datedebut, DateTime? Datefin, bool IsPrecontentieux);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsObservation> RetourneObservation();

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsLclient> RetourneListeFactureNonSoldeCaisse(CsClient leClient);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool UpdateDetailCampagneSuiteRelance(List<CsDetailCampagne> lesCampagnes);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsDetailCampagne> ListeDesClientRelance(List<CsCAMPAGNE> lesCampagnes);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsDetailCampagne> ListeDesClientAResilier(List<CsCAMPAGNE> lesCampagnes);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsDetailCampagne> RechercheClientCampagne(string CodeSite, string IdCampagne, int IdPia, DateTime? DateDebut, DateTime? DateFin, string Centre, string Client, string Ordre, int TypeEdition);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsDetailCampagne> RechercheClientCampagneScgc(string CodeSite,string Centre, string Client, string Ordre, int TypeEdition);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsCAMPAGNE> RechercheCampagne(string CodeSite, string IdCampagne, int IdPia, DateTime? DateDebut, DateTime? DateFin, int TypeEdition);

        #region Sylla 20/06/2016

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<string> RecupererPeriodeDePlage(string PeriodeDebut, string PeriodeFin);

        #endregion
        #endregion

        #region Sylla

        #region Sylla 05/06/2017
        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<string> MiseAJourCategorie7(List<string> lines, string matricule);
        #endregion
        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsRegCli> RetourneCodeRegroupement();
        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsAffectationGestionnaire> RemplirAffectation();
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool? SaveAffection(List<CsRegCli> ListRegCliAffecter, int? ID_USER);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsLclient> Remplirfacture(CsRegCli csRegCli, List<string> listperiode);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsLclient> RemplirfactureAvecProduit(CsRegCli csRegCli, List<string> listperiode, List<int> idProduit);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<Dico> SaveCampane(List<CsLclient> ListFacturation, CsRegCli csRegCli, int? ID_USER);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        CsCampagneGc VerifierCampagneExiste(CsRegCli csRegCli, string periode);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsLclient> RetourneListeFactureNonSolde(string centre, string client, string ordre, int foreignkey, string REFEM);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsLclient> RetourneListeFactureNonSoldeGC(CsClient leClient);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsCampagneGc> RemplirCampagne(string Matricule);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool? SaveMandatement(List<CsDetailCampagneGc> ListMandatementGc, bool IsAvisCerdit);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool? SavePaiement(List<CsPaiementGc> ListMandatementGc, CsDetailMandatementGc Facture_Payer_Partiellement=null);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsCampagneGc> RetournCampagneByRegcli(CsRegCli csRegCli, string periode);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool? SaisiPaiement(decimal? Montant, int Id);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsLclient> MiseAjourCompt(List<CsDetailPaiementGc> lstDetailPaiement, int Id);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsRegCli> RetourneCodeRegroupementByCampagne(int IdCampagne);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        CsCampagneGc RemplirCampagneById(int IdCampagne, string Statut);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsCampagneGc> ChargerCampagne(int IdRegroupement);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsDetailCampagne> ChargerClientPourSaisiIndex(int idCampagne);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsLclient> AutorisationDePaiement(string centre, string client, string ordre);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool ValidateAutorisation(CsLclient leFacture);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsCAMPAGNE> RetourneCampagnePrecontentieux(List<int> lstCentre);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsDetailCampagnePrecontentieux> RetourneDetailPrecontentieux(int IdCampagne);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool MajDetailPrecontentieux(List<CsDetailCampagnePrecontentieux> lstIdDetailCamp);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool? UpdateDetailMoratoire(List<CsDetailMoratoire> ListeMoratoire);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsDetailCampagnePrecontentieux> RechercherSuiviCampagnePrecontentieux(CsCAMPAGNE lesCampagnes);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsDetailCampagnePrecontentieux> RechercherAbonnemtPrepayePrecontentieux(List<CsDetailCampagnePrecontentieux> lesClient);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsDetailCampagnePrecontentieux> RechercherClientSolde(CsCAMPAGNE lesCampagnes);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool AnnulerCampagne(string numeroCampagne);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool InsererDechargePrecontentieux(CsPrecontentieuxDechargement Decharge);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsClientRechercher > RechercheClientCompteur(string NumCompteur);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsDetailCampagnePrecontentieux> RechercheAbonneLier(CsCAMPAGNE laCampagne);


        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsDetailCampagnePrecontentieux> REEDITION_CAMPAGNE_PRECONTENTIEUX(string IdCampagne);
        #endregion

        [OperationContract]
        [FaultContract(typeof(Errors))]
        CsDetailCampagnePrecontentieux RetourneClientByReferenceOrdre(int idCentre, string client, string Ordre);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsRegCli> RetourneRegroupementGestionnaires(int IdGestionnaire);



        [OperationContract]
        [FaultContract(typeof(Errors))]
        string InsererFraisPose(List<CsLclient> lesFacture);
    }
}
