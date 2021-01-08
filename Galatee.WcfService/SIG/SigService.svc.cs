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
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "SigService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select SigService.svc or SigService.svc.cs at the Solution Explorer and start debugging.
    public class SigService : ISigService
    {
        public List<CsAbonneCarte> RetourneCoordonneesAbon(CsClientRechercher client)
        {
            try
            {
                DBSig DbSig = new DBSig();
                return DbSig.RetournerCoodonnesAbonne(client);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }  
        }

        public List<CsAbonneCarte> RetourneCoordonneesAbonCampagne(string  IdCoupure)
        {
            try
            {
                DBSig DbSig = new DBSig();
                return DbSig.RetourneCoordonneesAbonCampagne(IdCoupure);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

    }
}
