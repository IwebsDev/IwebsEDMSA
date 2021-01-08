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
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IIScelleService" in both code and config file together.
    [ServiceContract]
    public interface IIScelleService
    {
        #region Scelle

        #region Sylla 09/01/2017
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool VerifieCompteurExisteNew(CsCompteur leCpt);
        #endregion
        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsActivite> RetourneListeActivite();


        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsCouleurActivite> RetourneListeCouleurScelle(int Activite_ID);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsCouleurActivite> RetourneListeAllCouleurScelle();

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsDscelle> RetourneListeDemandeScelle(int fk_dem);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsLotScelle> RetourneListeScelle();

        [OperationContract]
        [FaultContract(typeof(Errors))]
        string InsertDemandeScelle(CsDemandeBase lademande, CsDscelle dscelle);


        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsDetailAffectationScelle> RetourneListeDetailAffectationScelle(int IdDemande);

     
        [OperationContract]
        [FaultContract(typeof(Errors))]
        string ValidationReception(List<CsDetailAffectationScelle> ListeScelle, string MatriculAgent, int idetapeActuelle, string numdem);


        #endregion

        #region Bobou

        #region MARGASIN VIRTUELLE
        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsCompteurBta> SelectAllMargasinVirtuelle();

        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool DeleteMargasinVirtuelle(CsCompteurBta sCompteur);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        int UpdateMargasinVirtuelle(List<CsCompteurBta> sCompteur);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        int InsertMargasinVirtuelle(List<CsCompteurBta> sCompteur, List<CsCompteurBta> sCompteur1);


        #endregion




        #region Fournisseurs
        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsRefFournisseurs> RetourneListeFournisseurs();
        #endregion

        #region LotMagasinGeneral
        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsLotMagasinGeneral> SelectAllLotMagasinGeneral();

        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool DeleteLotMagasinGeneral(CsLotMagasinGeneral cLotMagasinGeneral);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool UpdateLotMagasinGeneral(List<CsLotMagasinGeneral> cLotMagasinGeneral);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool InsertLotMagasinGeneral(List<CsLotMagasinGeneral> cLotMagasinGeneral);


        #endregion

        #region OrigineScelle
        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsRefOrigineScelle> RetourneListeOrigineScelle();
        [OperationContract]
        [FaultContract(typeof(Errors))]
        CsRefOrigineScelle RetourneListeOrigineScelleById(int RefOrigineScelleByid);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool ValiderListeSaisieSelonDonneesEnBase(Dictionary<string, string> ListeSaisie, int OrigineLotsDeLaListe);

        #endregion

        #region RefEat
        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsRefEtatCompteur> RetourneEtatCompteur();
        #endregion

        #region vWRechercheCompteur
        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsRechercheCompteur> RetourneListRechercheCompteur();
        [OperationContract]
        [FaultContract(typeof(Errors))]
        CsRechercheCompteur RetourneRechercheCompteurById(string Numero_compteur);
        #endregion

        #region Marque_Modele
        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsMarque_Modele> RetourneListMarque_Modele();
        //[OperationContract]
        //[FaultContract(typeof(Errors))]
        //List<CsMarque_Modele> RetourneRechercheCompteurById(int id_Marque_Modele);
        #endregion

        #region CompteurBTa
        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsCompteurBta> SelectAllCompteur();

        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool DeleteCompteur(CsCompteurBta sCompteur);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        int UpdateCompteur(List<CsCompteurBta> sCompteur);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        int UpdateCompteurSuiteModif(List<CsCompteurBta> sCompteur);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool VerifierEtatCompteur(CsCompteurBta sCompteur);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        string  InsertCompteur(List<CsCompteurBta> sCompteur);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsCompteurBta> SelectAllCompteurNonAffecte();

        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool VerifieCompteurExiste(CsCompteurBta leCpt);
     
        #endregion

        #region Scelles
        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsRemiseScelleByAg> RetourneListScelle(int pk_id);
        #endregion

        #region Remise Scelle
        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsRemiseScelles> SelectAllRemiseScelle();

        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool DeleteRemise(CsRemiseScelles sRemise);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool UpdateRemise(List<CsRemiseScelles> sRemise);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        int InsertRemise(List<CsRemiseScelles> sRemi);


        #endregion

        #region Motifs 
        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsMotifsScelle> SelectAllMotisScelle();
        #endregion

        #region Selles All
        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsScelle> SelectAllScelles();

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsScelle> RetourneScellesListeByAgence(int IdCentre);
        #endregion
     

        #region tblot
        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsTbLot> RetourneListelot();
        #endregion

        #region Utilisatuer
        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsUtilisateur> RetourneListeAllUser();
        #endregion

        #region retours Scelle
        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsRetourScelles> SelectAllRetourScelle();

        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool DeleteRetours(CsRetourScelles sRemise);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool UpdateRetourse(List<CsRetourScelles> sRemise);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        int InsertRetours(List<CsRetourScelles> sRemi);


        #endregion

        #endregion

        #region Status Scellé
        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsRefStatutsScelles> SelectAllStatutsScelles();

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsRemiseScelleByAg> RetourneListScelleByStatus(int pk_id, int Status);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsRemiseScelleByAg> RetourneListScelleByCentre(int idCentre);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsRemiseScelleByAg> SCELLES_RETOURNE_Pour_ScellageCpt(int pk_ID);
        #endregion

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsCompteurBta> RetourneCompteurBtaByNumCptNumScelle(string NumeroCompteur, string NumeroScelle);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsCompteurBta> RetourneCompteurAffecter(string CodeAgence);
    }
}
