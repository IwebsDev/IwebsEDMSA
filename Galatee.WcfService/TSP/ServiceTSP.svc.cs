using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Galatee.Structure;
using Galatee.DataAccess;
using System.ServiceModel.Activation;
//using Microsoft.Reporting.WebForms;
using System.IO;
using System.Web.Hosting;
using System.Data.SqlClient;

namespace WcfService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "ServiceTSP" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select ServiceTSP.svc or ServiceTSP.svc.cs at the Solution Explorer and start debugging.
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class ServiceTSP : IServiceTSP
    {

        public bool InsertTransfertData(string NumeroLot, string Tournee)
        {
            try
            {
                DBIndex db = new DBIndex();
                return db.InsertTransfertData(NumeroLot, Tournee);
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return false;
            }
        }
        public List<CsCasind > ChargerCas()
        {
            try
            {
                FacturationService db = new FacturationService();
                return db.RetourneListeDesCas(string.Empty, string.Empty);
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }
        public List<CsStatutTransfert> ChargerStatusTransfert()
        {
            try
            {
                DBIndex db = new DBIndex();
                return db.RetourneListeDesStatusTransfert();
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }
        public List<CsMarqueCompteur> ChargerMarqueCompteur()
        {
            try
            {
                AcceuilService db = new AcceuilService();
                return db.RetourneToutMarque();
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }
        public List<CsLotri> ChargerLotri(string NumeroLot, List<string> lstLotDejaCharge, bool ReleveIndexChecked)
        {
            try
            {
                DBIndex db = new DBIndex();
                return db.ChargerLotTsp(NumeroLot, lstLotDejaCharge, ReleveIndexChecked);
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null ;
            }
        }
        public List<CsEvenement > ChargerEvenementTSP(string NumeroLot, List<string> Tournee,bool IsEnquete)
        {
            try
            {
                DBIndex db = new DBIndex();
                return db.ChargerEvenementTsp(NumeroLot, Tournee, IsEnquete);
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }
        static List<CsEvenement> lstEvenementTransfereok = new List<CsEvenement>();
        public bool ValiderChargementTSP(List<CsEvenement> lstEvenementTransfere,bool EstComplet)
        {
            try
            {
                if (EstComplet==true)
                {
                    lstEvenementTransfereok.AddRange(lstEvenementTransfere);
                    List<CsMapperTransfert> lst = Galatee.Tools.Utility.ConvertListType<CsMapperTransfert, CsEvenement>(lstEvenementTransfereok);
                    new DBIndex().ValiderChargementTSP(lst);
                    lstEvenementTransfereok = new List<CsEvenement>();
                    return true;
                }
                else
                {
                    lstEvenementTransfereok.AddRange(lstEvenementTransfere);
                    return true;
                }
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                lstEvenementTransfereok = new List<CsEvenement>();

                return false ;
            }
        }
    }
}
