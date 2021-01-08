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
    public class DBCONNEXIONACTIVE
    {
        public static List<CsCONNEXIONACTIVE> GetAllConnexionActive()
        {
            try
            {
                return Entities.GetEntityListFromQuery<CsCONNEXIONACTIVE>(RpntProcedures.RPNT_CONNEXIONACTIVE_RETOURNE());;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool DeConnecterUser(CsCONNEXIONACTIVE pConnectionActive)
        {
            bool result = false;
            try
            {
                if (pConnectionActive != null)
                   result = DeleteConnectionActive(pConnectionActive);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool DeleteConnectionActive(CsCONNEXIONACTIVE pConnectionActive)
        {
            try
            {
                return Entities.DeleteEntity<CONNEXIONACTIVE>(Entities.ConvertObject<CONNEXIONACTIVE, CsCONNEXIONACTIVE>(pConnectionActive));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool InsertConnectionActive(CsCONNEXIONACTIVE pConnectionActive)
        {
            try
            {
                return Entities.InsertEntity<CONNEXIONACTIVE>(Entities.ConvertObject<CONNEXIONACTIVE, CsCONNEXIONACTIVE>(pConnectionActive));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool UpdateConnectionActive(CsCONNEXIONACTIVE pConnectionActive)
        {
            try
            {
                return Entities.UpdateEntity<CONNEXIONACTIVE>(Entities.ConvertObject<CONNEXIONACTIVE, CsCONNEXIONACTIVE>(pConnectionActive));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
