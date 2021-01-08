using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Galatee.DataAccess;
using Galatee.Structure;


namespace WcfService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IInterfaceComptableService" in both code and config file together.
    [ServiceContract]
    public interface IInterfaceComptableService
    {
        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsCentre> RetourneListeDeSite();

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsCaisse> RetourneCaisse();

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsCoper> RetourneCodeOperation();

        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool RetourneFichierComptable(List<CsComptabilisation> LstEcriture);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsBalance> RetourneBalanceAgee(string CodeSite, DateTime? Datefin);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsBalance> RetourneBalanceAuxilliaire(string CodeSite, DateTime? Datefin);

        //[OperationContract]
        //[FaultContract(typeof(Errors))]
        //bool RetourneBalanceAuxilliaire_Block(string CodeSite, DateTime? Datefin, bool chk_Excel_IsChecked, string Libelle_Site, string CheminImpression, int Offset, string matricule, string ServerEndPointName, string ServerEndPointPort,string WebAppBaseAdreese);




        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsComptabilisation> RetourneAllOperationByCritere(int IdCentre, List<int> lstIdcaisse, List<string> OperationSelect, DateTime? DateCaisseDebut, DateTime? DateCaisseFin, DateTime? Date);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        Dictionary<List<CsComptabilisation>, List<CsEcritureComptable>> Retournefacture(List<CsOperationComptable> lesOperationCpt, List<int> IdCentre,DateTime? DateCaisseDebut, DateTime? DateCaisseFin, string matricule, string Site);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        Dictionary<List<CsComptabilisation>, List<CsEcritureComptable>> RetourneEncaissement(List<CsOperationComptable> lesOperationCpt, List<CsCaisse> lstCaisse, DateTime? DateCaisseDebut, DateTime? DateCaisseFin, string matricule, string Site);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        Dictionary<List<CsComptabilisation>, List<CsEcritureComptable>> RetourneMiseAJourGrandCompte(CsOperationComptable OperationCpt, DateTime? DateCaisseDebut, DateTime? DateCaisseFin, string matricule, string Site);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsComptabilisation> RetourneEtatMiseAJourGrandCompte(DateTime? DateCaisseDebut, DateTime? DateCaisseFin);

 
        [OperationContract]
        [FaultContract(typeof(Errors))]
        Dictionary<List<CsComptabilisation>, List<CsEcritureComptable>> RetourneDEncaissement(List<CsOperationComptable> lesOperationCpt, List<CsCaisse> lstCaisse, DateTime? DateCaisseDebut, DateTime? DateCaisseFin, string matricule, string Site);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool PurgeComptabilisation(List<int> IdOpertation, string CodeSite, DateTime? DateDebut, DateTime? DateFin);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsLclient> RetourneAvanceSurConsomation(string CodeSite, bool IsResilier, DateTime? DateDebut, DateTime? DateFin);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsClient> RetourneProvision(string CodeSite, List<string> lstCateg, List<string> lstProd, DateTime? DateDebut, DateTime? DateFin);

        #region Interfacage Comptable Sylla
        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsTypeFactureComptable> RetourneTypeFacture();
        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsCompteSpecifique> RetourneCompteSpecifique();
        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsTypeCompte> RetourneTypeCompte();

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsOperationComptable> RetourneOperationComptable();

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsEcritureComptable> IsOperationExiste(List<CsEcritureComptable> LigneComptable);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool InsertionLigneComptableGenerer(List<CsEcritureComptable> LigneComptable);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsBanqueCompte> RetourneBanqueCentre();

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsCentreCompte> RetourneParamCentre();


        #endregion

    
    }
}
