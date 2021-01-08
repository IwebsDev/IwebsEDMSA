using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Galatee.Entity.Model;
using Galatee.Structure;
using Galatee.Entity.Model.Rpnt;

namespace Galatee.DataAccess
{
    public class DBAUDIT
    {
        public static List<CsAUDIT> GetAllAudit()
        {
            try
            {
                return Entities.GetEntityListFromQuery<CsAUDIT>(RpntProcedures.RPNT_CONNEXIONACTIVE_RETOURNE()); ;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CsAUDIT> GetAllAuditSelonCriteres(string pAgent, string pNomMachine, DateTime pDateDebut, DateTime pDateFin, int? pTypedAction)
        {
            try
            {
                return Entities.GetEntityListFromQuery<CsAUDIT>(RpntProcedures.RPNT_AUDIT_RETOURNESelonCriteres(pAgent, pNomMachine, pDateDebut, pDateFin, pTypedAction));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool DeleteAudit(CsAUDIT pAudit)
        {
            try
            {
                return Entities.DeleteEntity<AUDIT>(Entities.ConvertObject<AUDIT, CsAUDIT>(pAudit));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool InsertAudit(CsAUDIT pAudit)
        {
            try
            {
                return Entities.InsertEntity<AUDIT>(Entities.ConvertObject<AUDIT, CsAUDIT>(pAudit));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool UpdateAudit(CsAUDIT pAudit)
        {
            try
            {
                return Entities.UpdateEntity<AUDIT>(Entities.ConvertObject<AUDIT, CsAUDIT>(pAudit));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
