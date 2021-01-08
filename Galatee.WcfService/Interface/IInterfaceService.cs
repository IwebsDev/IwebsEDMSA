using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Galatee.Structure;

namespace WcfService
{
    // REMARQUE : vous pouvez utiliser la commande Renommer du menu Refactoriser pour changer le nom d'interface "IInterfaceService" à la fois dans le code et le fichier de configuration.
    [ServiceContract]
    public interface IInterfaceService
    {
        [OperationContract]
        List<CsLibelle> RetourneTousMoisComptables();

        //[OperationContract]
        // CsParametre RetourneParametresCompta();

        [OperationContract]
        List<CsLibelle> RetourneSousMenuCompta();

        //[OperationContract]
        //[FaultContract(typeof(Errors))]
        //List<CsCtax> SelectAll_CTAX();

        //[OperationContract]
        //[FaultContract(typeof(Errors))]
        //List<CsCentre> RetourneTousDirecteur();

        //[OperationContract]
        //[FaultContract(typeof(Errors))]
        //List<CsCorrespondance> RetourneCorrespondanceCompta();

        //[OperationContract]
        //[FaultContract(typeof(Errors))]
        //List<CsLibelle> RetourneActiviteProduits();

        //[OperationContract]
        //[FaultContract(typeof(Errors))]
        //List<CsJournal> RetourneJournals();

        //[OperationContract]
        //[FaultContract(typeof(Errors))]
        //List<CsCompteCritere> RetourneCsCompteCriteres();

        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool? ComptabilisationFacturation(string pMoisCompta, string pImprimante, int pGestionMoisCompta, string fileName, string fileHeader, int pTypeEnc, int pTypeMenu);
    }
}
