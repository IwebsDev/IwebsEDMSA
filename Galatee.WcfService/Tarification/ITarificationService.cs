using Galatee.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace WcfService.Tarification
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ITarificationService" in both code and config file together.
    [ServiceContract]
    public interface ITarificationService
    {
        #region Recherche Tarif
        [OperationContract]
        [FaultContract(typeof(Errors))]
        int SaveRechercheTarif(List<CsRechercheTarif> ListeRedevanceToUpdate, List<CsRechercheTarif> ListeRedevanceToInserte, List<CsRechercheTarif> ListeRedevanceToDelete);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsRechercheTarif> LoadAllRechercheTarif();

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsContenantCritereTarif> LoadAllContenantCritereTarif();
        #endregion

        #region Redevence
        [OperationContract]
        [FaultContract(typeof(Errors))]
        int SaveRedevance(List<CsRedevance> ListeRedevanceToUpdate, List<CsRedevance> ListeRedevanceToInserte, List<CsRedevance> ListeRedevanceToDelete);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsRedevance> LoadAllRedevance();
        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsTypeRedevence> LoadAllTypeRedevance();
        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsTypeLienRedevence> LoadAllTypeLienRedevance();
        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsTypeLienProduit> LoadAllTypeLienProduit();
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool CheickCodeRedevanceExist(string Code);
        #endregion

        #region Variable de tarificaton
            [OperationContract]
            [FaultContract(typeof(Errors))]
            List<CsVariableDeTarification> LoadAllVariableTarif();
        
            [OperationContract]
            [FaultContract(typeof(Errors))]
            List<CsModeCalcul> LoadAllModeCalcule();
        
            [OperationContract]
            [FaultContract(typeof(Errors))]
            List<CsModeApplicationTarif> LoadAllModeApplicationTarif();
        
            [OperationContract]
            [FaultContract(typeof(Errors))]
            int SaveVariableTarif(List<CsVariableDeTarification> ListeRedevanceToUpdate, List<CsVariableDeTarification> ListeRedevanceToInserte, List<CsVariableDeTarification> ListeRedevanceToDelete);
        #endregion

        #region TarifFacturation
            [OperationContract]
            [FaultContract(typeof(Errors))]
            List<CsTarifFacturation> LoadAllTarifFacturation(int? idCentre, int? idProduit, int? idRedevance, int? idCodeRecherche);

            [OperationContract]
            [FaultContract(typeof(Errors))]
            List<CsDetailTarifFacturation> LoadAllDetailTarifFacturation();

            [OperationContract]
            [FaultContract(typeof(Errors))]
            bool SaveTarifFacturation(CsTarifFacturation Tarification, int Action);

            [OperationContract]
            [FaultContract(typeof(Errors))]
            bool CreateTarif(List<CsTarifFacturation> Tarification);

            [OperationContract]
            [FaultContract(typeof(Errors))]
            bool DuplicationTarifVersCentre(int AncienIdCentre, int NouveauIdCentre, int? produit);

            [OperationContract]
            [FaultContract(typeof(Errors))]
            List<CsUniteComptage> LoadAllUniteComptage();  
            
            [OperationContract]
            [FaultContract(typeof(Errors))]
            List<CsTarifFacturation> LoadTarifGenerer(string FK_IDRECHERCHETARIF, string FK_IDVARIABLETARIF, string Produit);      
        #endregion

    }
}
