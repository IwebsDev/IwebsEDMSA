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
    // REMARQUE : vous pouvez utiliser la commande Renommer du menu Refactoriser pour changer le nom d'interface "IFacturationService" à la fois dans le code et le fichier de configuration.
    [ServiceContract]
    public interface IFacturationService
    {
        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsReleveur> RetourneReleveurCentre_(List<int> Liscentre, int id_user);        

        #region Sylla 11-05-2017

        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool? EnvoyerFacturesNew(List<CsEnteteFacture> facturesAEnvoyer, string Rdlc, string Matricule, string Serveur, string Port, string PortWeb);
        
        #endregion

        #region Facturation
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool verifieSimulation(List<CsLotri> pLot);

        //[OperationContract]
        //[FaultContract(typeof(Errors))]
        //bool? EnvoyerFacturesNew(List<CsEnteteFacture> facturesAEnvoyer, string Rdlc, string Matricule);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool verifieSaisieTotal(List<CsLotri> pLot);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsLotri> ChargerLotri(List<int> lstCentreHabil);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsLotri> ChargerLotriDejaFacture(List<int> lstCentreHabil, string Matricule);


        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsLotri> ChargerLotriPourDefacturation(Dictionary<string, List<int>> lstSiteCentre, string UserConnect, bool IsValidation);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsLotri> ChargerLotriPourEnquete(Dictionary<string, List<int>> lstSiteCentre, bool IsLotCourant, string Periode);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsLotri> ChargerLotriPourEditionIndex(Dictionary<string, List<int>> lstSiteCentre, bool IsLotCourant, string Periode);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsLotri> ChargerLotriPourSaisie(Dictionary<string, List<int>> lstSiteCentre, string UserConnect, bool IsLotCourant, string Periode);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsLotri> ChargerLotriPourCalcul(Dictionary<string, List<int>> lstSiteCentre, string matricule, bool IsLotCourant, string Periode);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsLotri> ChargerLotriPourMiseAJour(List<int> lstCentre, string UserConnect);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsLotri> ChargerLotriDejaMiseAJour(List<int> lstCentreHabil, string Matricule);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsLotri> ChargerLotriPourEdition(Dictionary<string, List<int>> lstSiteCentre, string matricule, bool IsLotCourant, string Periode);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsLotri> ChargerLotriPourRejetClient(Dictionary<string, List<int>> lstSiteCentre, string matricule, bool IsLotCourant, string Periode);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool RetirerClientLotFact(List<CsEvenement> lesEvenementLot);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsEvenement> ChargerDesEvenement(List<CsLotri> lstLot);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsEvenement> ChargerDesEvenementPourRejet(List<CsLotri> lstLot);


        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsEvenement> ChargerDesEvenementClientLot(CsClient leClient, string NumeroLot);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsLotri> ChargerLotriNonMisAJours(List<int> lstCentre);


        [OperationContract]
        [FaultContract(typeof(Errors))]
        CParametre ReturnCparam();

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsLotri> ChargerDetailLotri(List<int> lstIdCentre, string Matricule);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsLotri> ChargerDetailLotriPourDefacturation(List<int> LstIdCentre);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsEnteteFacture> retourneFactureDecroissance(List<int> lstIdCentre, CsLotri leLot);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsClient> VerifieClient(List<CsLotri> lesLot, string Client);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool VerifieExisteLotClient(CsClient LeClient, string Numlot);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsEvenement> RetourneEvenementClient(CsClient LeClient);


        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsEvenement> RetourneEvenementCorrectionIndex(CsClient LeClient);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsEnteteFacture> retourneFacturePourDuplicat(int idCentre, string centre, string client, string ordre);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool? InsererLstEvenement(List<CsEvenement> pEvenement);



        #region Edition de facture

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<String> ListeDeJet(string LotRi);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<String> ListeDePeriode();

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<String> ListeDeFormat();

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsEnteteFacture> RetourneClientDuneBorne(string centre, string client, string lotRi, string periode);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsFactureClient> RetourneFacturesRegroupement(string regroupementDebut, string regroupementFin, List<string> LstPeriode, List<string> Produit, int? idcentre, string rdlc);
        //bool? RetourneFacturesRegroupement(string regroupementDebut, string regroupementFin, List<string> LstPeriode, List<string> Produit, int? idcentre, string rdlc, Dictionary<string, string> parameters, string key);


        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsFactureClient> RetourneFacturesPeriode(List<CsCentre> lstCentre, string periodeDebut, string PeriodeFin, string centreTournee,
            string tourneeDebut, string tourneeFin, string centreReprise, string clientReprise,
            string centreStop, string clientStop, string rdl);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsFactureClient> RetourneFactures(CsLotri leLotSelect, CsTournee laTournee,
                    CsClient leClient, bool DejaMiseAjour, string rdl);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsAnnomalie> RetourneAnnomalieFactures(CsLotri leLotSelect, string centreTournee,
                     string tourneeDebut, string tourneeFin, string centreReprise, string clientReprise,
                    string centreStop, string clientStop, string periodeSelectionne, string rdl);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsAnnomalie> RetourneControleFactures(CsLotri leLotSelect);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsFactureClient> ExposeFactureClient();


        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsLotri> retourneListeAMaj(string lotri, byte onlyLotriNum);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<string> retourneMoisComptable(string statut, string dateDebut, string dateFin);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        CsStatFacturation MiseAjourLots(List<CsLotri> lots);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsFactureClient> retourneDuplicatas(string centre, string client, string ordre);



        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool? EditerFactures(CsLotri leLotSelect, CsTournee laTournee,
             CsClient leClient, bool DejaMiseAjour, string Rdlc, Dictionary<string, string> param, string CheminImpression, string Matricule, string Serveur, string Port, string PortWeb);
        
        
        #endregion

        #region Calcul Facture

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsFactureBrut> CalculeDuLotGeneral(List<CsLotri> ListLotSelectione, bool IsSimulation);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        CsStatFacturation DefacturerLot(List<CsLotri> _ListDesLots, string Action);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool DemandeDefacturerLot(List<CsLotri> _ListDesLots);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool ValiderRejetDefacturation(List<CsLotri> _ListDesLots);


        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsEnteteFacture> retourneFactureAnnulation(int idCentre, string centre, string client, string ordre);


        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool VerifierPaiementFacture(CsEnteteFacture laFacture);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool ValiderFacturation(List<CsFactureBrut> laFacturation, bool IsFactureResil);

        //[OperationContract]
        //[FaultContract(typeof(Errors))]
        //bool ValiderAnnulationFacture(CsEnteteFacture laFacture);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsLotri> ChargerLotriFromEntfac(List<int> lstCentre);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsAction> retourneActionFact(CsLotri leLot);
        #endregion

        #region Liste Redevence
        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsRedevance> LoadAllRedevance();

        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool SaveRedevance(List<CsRedevance> ListeRedevanceToUpdate, List<CsRedevance> ListeRedevanceToInserte, List<CsRedevance> ListeRedevanceToDelete);

        #endregion

        #endregion

        #region Index
        [OperationContract]
        [FaultContract(typeof(Errors))]
        CsTableReference ChargerTableDeReference(int Num, string Code, string Centre);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        string MiseAJourEvenementTSP(string CONSO, string DATECREATION, string IsSaisi, string STATUS, string STATUSPAGERIE, string MATRICULE, string IsFromPageri, string CAS, string INDEXEVT, string DATEEVT, string PK_ID);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsLotri> RetourneListeLotNonTraiteParReleveur(string NumPortable);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsEvenement> RetourneListeLotSelonCritere(string NumLot, string Centre, string Tournee);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsProduit> RetourneProduitService(int NumEntreTa, string CodeTable, string centre);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool? CreationClientIndexHorsLot(CsEvenement ev);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool? InsererEvenement(CsEvenement ev);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        CsSaisiHorsLot RetourneInfoClientHorsLot(int Fk_IDcentre, string Centre, string Client, string produit, string tournee);


        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsCentre> ListeDesDonneesDesSite();

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsTournee> ChargerListeDesTournees();

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsEvenement> IsBatchExistDansPagerie(string _NumLot);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsLotri> RetourneListeLotNonTraite(List<int> lsCentrePerimetre);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsEvenement> ListeDesEnquete(List<CsLotri> loSelectionnee);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsEvenement> ListeDesEnqueteCasOnze(List<CsLotri> loSelectionnee);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        int? ConstruireLot(List<CsCentre> LstCentre, List<CsProduit> LstProduit, List<CsCategorieClient> LstCategorie, List<CsFrequence> LstPeriodicite, List<CsTournee> LstTournee, string Lotri, string periode, string Matricule);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsEvenement> ConstruireLotAvecEdition(List<CsCentre> LstCentre, List<CsProduit> LstProduit, List<CsCategorieClient> LstCategorie, List<CsFrequence> LstPeriodicite, List<CsTournee> LstTournee, string Lotri, string periode, string Matricule);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsEvenement> ChargerListeDesEvenements(List<CsLotri> _lstLotri, string sequence);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsEvenement> ChargerListeDesEvenementsNonFacture(List<CsCanalisation> leCompteur);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsCasind> RetourneListeDesCas(string centre, string cas);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool DechargementTsp(List<CsEvenement> LstEvt);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool InsererTransferTsp(List<CsEvenement> LstEvt);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsEvenement> ChargerListeDesEvenementsTsp(List<CsLotri> _lstLotri, string sequence);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsEvenement> ChargerListeDesTransfertsp(CsLotri _lstLotri);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsLotri> ChargerDistinctLotriTransfertsp(List<int> isCentre);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        CsEvenement MisAJourEvenement(List<CsEvenement> LstEvt);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool RepriseIndex(CsEvenement LstEvt);


        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool MisAJourEvenementSynchoTSP(List<CsEvenement> LstEvt);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        CsEvenement MisAJourEvenementIndex(List<CsEvenement> LstEvt);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool ValiderEnquete(List<CsEvenement> LstEvt);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsCanalisation> RetourneCanalisation(int Fk_IDcentre, string Centre, string Client, string produit, int? point);


        [OperationContract]
        [FaultContract(typeof(Errors))]
        CsEvenement RetourneEvenementPose(CsEvenement csevenement);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool VerificationNumeroCompteur(CsEvenement leEvt);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        CsCanalisation RetourneCanalisationbyIdEvenement(int? Fk_IdCompteur);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        CsSaisiIndexIndividuel RetourneEvenementNonFact(CsCanalisation LaCanalisation);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsSaisiIndexIndividuel> RetourneListEvenementNonFact(List<CsCanalisation> LesCanalisation);




        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsEvenement> EditerLotGenerale(List<CsLotri> LstLotri, int typeRequete);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsCasind> ListeDesCas(string pLotri, List<int> LesCentre, List<int?> lesTournee, string typeCas);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsEvenement> EditerListeDesCas(string Lotri, List<int> LesCentre, List<int?> Latournee, List<CsCasind> Cas);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsEvenement> EditerListeDesCasConfirmer(string Lotri, List<int> LesCentre, List<int?> Latournee, List<CsCasind> Cas);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool? UpdateUneTournee(CsTournee LaTournee);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool? InsererUneTournee(CsTournee LaTournee);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool SupprimerUneTournee(CsTournee LaTournee);


        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsReleveur> RetourneReleveurCentre(List<int> Liscentre);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool UpdateUnReleveur(CsReleveur LeReleveur);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool InsererUnReleveur(CsReleveur LeReleveur);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool SupprimerUnReleveur(CsReleveur LeReleveur);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsUtilisateur> RetourneAllUser();

        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool setListRiInWebPart(string key, List<CsEnregRI> objectList, Dictionary<string, string> parameters);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsEnregRI> getListRiFromWebPart(string key);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool DeleteListRiFromWebPart(string key);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsTournee> ChargerTourneeReleveur(int Fk_IdReleveur);



        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsLotri> ChargerLotriPourDelete(string matricule, string DebutLot, string Finlot);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool DeleteLotri(List<CsLotri> lesLot);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool EffacerEvtLotri(string UserConnecter);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool VerificationEtatDuLot(List<CsLotri> lesLot);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool ValiderAffectation(List<CsTournee> lstTourne);

        //[OperationContract]
        //[FaultContract(typeof(Errors))]
        //List<CsEvenement> RetourneDernierEvenementDeLaCanalisation(CsAbon leAbonnement);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsAbon> RetourneAbon(int fk_idcentre, string Centre, string Client, string Ordre);


        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsAnnomalie> RetourneLesteAnomalie(string lotri, int idCentre);


        [OperationContract]
        [FaultContract(typeof(Errors))]
        void SupprimeAnnomalie(string lotri, int idCentre);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool PurgeDeLot(List<CsLotri> leslotri);
        #endregion

        #region envoi de mail
        [OperationContract]
        List<CsEnteteFacture> ListeDesClientPourEnvoieMail(List<int> CentreClient, List<string> lstPeriode, bool sms, bool email);

        [OperationContract]
        List<CsFactureClient> EnvoyerFactures(List<CsEnteteFacture> facturesAEnvoyer, string Rdlc);
        #endregion

        #region Suivie de la facturation
                [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsEvenement> ChargerCasFacture(Dictionary<string, List<int>> lstSiteCentre, string Lotri, string Periode);


        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsEvenement> ChargerDetailCasFacture(Dictionary<string, List<int>> lstSiteCentre, string Lotri, string Periode, List<string> Cas);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsEvenement> ChargerClientNonConstituer(Dictionary<string, List<int>> lstSiteCentre, string Periode);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsEvenement> EtatStatistique(Dictionary<string, List<int>> lstSiteCentre, string Periode, string Lotri, string TypeEtat);


        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsTournee> ChargerAllTourneeReleveur(List<int> lstIdCentre);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool SaveAffectationTourne(List<CsTournee> ListeDesTourneAAffecter);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsComparaisonFacture> ComparaisonFacture(List<int> LstCentre, string PerideDebut, string Lotri1, string PeriodeFin, string Lotri2, bool IsMT);
        #endregion


    }
}
