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
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IServiceTSP" in both code and config file together.
    [ServiceContract]
    public interface IServiceTSP
    {
        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsMarqueCompteur> ChargerMarqueCompteur();

        [OperationContract]
        [FaultContract(typeof(Errors))]
        //bool InsertTransfertData(string NumeroLot);
        bool InsertTransfertData(string NumeroLot, string Tournee);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsCasind> ChargerCas();

        [OperationContract]
        [FaultContract(typeof(Errors))]
        //List<CsLotri> ChargerLotri(string NumeroLot, List<string> lstLotDejaCharge);
        List<CsLotri> ChargerLotri(string NumeroLot, List<string> lstLotDejaCharge, bool ReleveIndexChecked);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsEvenement> ChargerEvenementTSP(string NumeroLot, List<string> Tournee, bool IsEnquete);    
        
        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsStatutTransfert> ChargerStatusTransfert(); 

        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool ValiderChargementTSP(List<CsEvenement> lstEvenementTransfere, bool EstComplet);  
    }
}
