using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Galatee.Structure;

namespace WcfService
{
    // REMARQUE : vous pouvez utiliser la commande Renommer du menu Refactoriser pour changer le nom d'interface "ICaisseService" à la fois dans le code et le fichier de configuration.
    [ServiceContract]
    public interface ICaisseService
    {
        #region CAISSE

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsFraisTimbre> RetourneListeTimbre(string ServerMode);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsReglementRecu> CashierPayments(string key, Dictionary<string, string> parametresRDLC, List<CsReglementRecu> res);
        
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool AjustementDeFondCaisse(string NumCaisse, string NouveauFond, string MoisCompt, string matricule);
        
        [OperationContract]
        [FaultContract(typeof(Errors))]
        CsHabilitationCaisse HabiliterCaisse(CsHabilitationCaisse laCaisseHAbil);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool ReverserCaisse(List<CsReversementCaisse>  laCaisseHAbil);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsHabilitationCaisse> RetourneCaisseHabiliterCentre(CsCentre leCentre);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsHabilitationCaisse> RetourneSuppervisionCaisse(List<int> Centre, DateTime? dateCaisse, bool IsEncours);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsRegCli> ChargerListeCodeRegroupement();

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsPayeur> ChargerListePayeur();

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsLclient > ChargerListeFacturePayeur(CsPayeur lePayeur);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsCodeRegroupement> CodeRegroupement();

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsReglement> RetourneRecuDeManuelCaisseList(string Caisse);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsClient> RetourneClientsParReference(string route, string sens);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsClient> RetourneClientsParAmount(decimal amount, string sens, List<int> lstIdCentre);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsClient> RetourneClientsParNoms(string names);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsLclient> RetourneListeFacture(string centre, string Client, string ordre, int foreignkey);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsLclient> RetourneListeFactureNonSolde(CsClient leClient);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsLclient> RetourneListeFactureNonSoldeCaisse(CsClient leClient);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsLclient> RetourneListeFactureNonSoldeClient(List<CsClient> lesClient);
        

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsLclient> RetourneListeFactureNonSoldeByRegroupement(List<int> LstIdRegroupement, List<string> periode);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool setDetailMoratoireInWebPart(string key, List<CsDetailMoratoire> objectList, Dictionary<string, string> parameters);


        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool setDetailDsiconnectionInWebPart(string key, List<aDisconnection> objectList, Dictionary<string, string> parameters);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool setDetailCampagneInWebPart(string key, List<aCampagne> objectList, Dictionary<string, string> parameters);


        [OperationContract]
        [FaultContract(typeof(Errors))]
        string RetourneNumCaisse(string MatriculeConnecter);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsLclient > RetourneListeFactureReg(int _CodeReg);



        [OperationContract]
        [FaultContract(typeof(Errors))]
        string VerifieEtatCaisse(string matricule, int? Fk_IdHabilCaisse);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsCtax> RetourneListeTaxe();

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<string> RetourneListeDesClientsReg(int IdCodeRegroupement);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsBanque > RetourneListeDesBanques();

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CParametre> ListeCaisse();

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsLclient> RetourneDemande(string NumDemande, bool IsExtension);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        CsLclient RetourneLeDevis(string NumDevis);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsCoper> RetourneListeDeCoperOD();

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsMoratoire> RetourneListeMoratoire(string caisse, string acquit, string matricule);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        CsParametresGeneraux  RetourneListeTa58(string CodeTable);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        string NumeroFacture(int PkidCentre);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        CsReglement[] RetourneListePaiementDuRecu(string numcaisse, string numrecu, string matricle);

    

        /*[OperationContract]
        [FaultContract(typeof(Errors))]
        string RetourneNumeroRecu(int idCaisse);
        */

        [OperationContract]
        [FaultContract(typeof(Errors))]
        string RetourneNumeroRecu(int? idcaisse, string Matricule);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsClient> TestClientExist(string centre, string Client, string ordre);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool? ValideOuvertureCaisse(DateTime? date, string matriculeCaisse, string numcaisse, string matriculeClerk, string raison, string saisipar, bool CaisseEstManuel);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool? FermetureCaisse(CsHabilitationCaisse _laCaisseHabil);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsCaisse> ChargerCaisseDisponible();

        [OperationContract]
        [FaultContract(typeof(Errors))]
        string VerifieCaisseDejaSaisie(string dateEncaissement, string matricule, ref string err);


        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool? ValiderAnnuleEncaissement(List<CsLclient> ListFactureAnnule);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool? RejeterAnnuleEncaissement(List<CsLclient> ListFactureAnnule);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool? DemandeAnnulationEncaissement(List<CsLclient> ListFactureAnnule);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool? RetirerDemandeAnnulationEncaissement(List<CsLclient> ListFactureAnnule);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool? InsererEncaissement(List<CsLclient > ReglementAInserer, string Operation);


        [OperationContract]
        [FaultContract(typeof(Errors))]
        string RetourneNumFactureNaf(int? PkIdcentre);



        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsLclient> RetourneEtatDeCaisse(CsHabilitationCaisse laCaisse);


        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsReglement> RetourneListePaiementPourAnnulation(string caisse, string acquit, string matricule);

        //[OperationContract]
        //[FaultContract(typeof(Errors))]
        //CsReglement[] RetourneRecuDeCaisse(string Matricule);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsLclient > RetourneRecuDeCaisseList(int Caisse);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsLclient> RetourneEtatClient(int pk_id);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsLclient> RetourneRecuDeCaissePourAnnulation(int Caisse);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsModereglement> RetourneModesReglement();

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsNature> RetourneNature();


        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsLibelleTop> RetourneTousLibelleTop();

        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool setReglementInWebPart(string key, List<CsReglement> objectList, Dictionary<string, string> parameters);

 

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsFacture > RetourneClientBilling(string centre, string client, string ordre);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        Dictionary<string, string> getReglementParameters(string key);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsReglement> getReglementFromWebPart(string key);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool DeleteDataFromWebPart(string key);


        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsCaisse> ListeCaisseDisponible(string Centre,string matricule);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsCaisse> RetourneListeCaisse();

        [OperationContract]
        [FaultContract(typeof(Errors))]
        decimal? RetourneEncaissementDate(CsHabilitationCaisse laCaisse);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsHabilitationCaisse> RetourneCaisseNonCloture(List<int> ListCentreCaisse);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsHabilitationCaisse> RetourneCaisseCloture(List<int> ListCentreCaisse);

        [OperationContract]
        [FaultContract(typeof(Errors))]
         CsHabilitationCaisse RetourneCaisseEnCours(int IdNumCaisse, int IdCaissier, DateTime DateDebut);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsLclient > RetourneEncaissementPourValidationAnnulation(List<int> LstCentreCaisse);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        CsHabilitationCaisse RetourneHabileCaisseReversement(CsHabilitationCaisse laCaisse);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsHabilitationCaisse> RetourneHabileCaisseNonReversement(CsHabilitationCaisse laCaisse);


        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool MiseAJourDemandeReversement(CsDemandeReversement DemandeReversement, int Action);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsDemandeReversement> RetourneDemandeReversement();

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsLclient> ChargerListeFacturePeriode(string periodeDebut, string periodeFin, string FactureDeb, string FactureFin);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsTypeTimbre> RetouneTypeTimbre();

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsMonnaie> ReturneAllMonaie();

        [OperationContract]
        [FaultContract(typeof(Errors))]
        CsHabilitationCaisse RetouneLaCaisseCourante(string matricule);


        [OperationContract]
        [FaultContract(typeof(Errors))]
        CsHabilitationCaisse RetouneLaCaisseCouranteInseree(string matricule, CsPoste poste);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsLclient> RemplirfactureRegroupementAvecProduit(CsRegCli csRegCli, List<string> listperiode, List<int> idProduit);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsLclient> RemplirfactureRegroupement(CsRegCli csRegCli, List<string> listperiode);
        #endregion


        #region PRINTING

        [OperationContract]
        bool? PrintReceipt(byte[] pRenderStream, bool landscape);

        [OperationContract]
        List<CsHabilitationCaisse> ListeDesReversementCaisse(List<CsHabilitationCaisse> LstHabilCaisse);

        [OperationContract]
        List<CsLclient> LitseDesTransaction(CsHabilitationCaisse laCaisse);

        [OperationContract]
        List<CsLclient> HistoriqueListeEncaissements(List<CsHabilitationCaisse> lstCaisse);


        [OperationContract]
        List<CsLclient> HistoriqueDesEncaissements(string matricule, int idCentre, DateTime datedebut, DateTime datefin);


        [OperationContract]
        List<CsHabilitationCaisse> RetourneListeCaisseHabilite(List<int> LstCentreCaisse);

        [OperationContract]
        List<CsHabilitationCaisse> ListeDesCaisse(int fk_idcaisse, string centre, DateTime datedebut, DateTime datefin, bool Isferme);
        #endregion
    }
}
