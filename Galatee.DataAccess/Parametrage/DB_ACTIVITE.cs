using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Galatee.Structure;
using Galatee.Entity.Model;

namespace Galatee.DataAccess
{
    public static class DB_ACTIVITE  
    {


        public static List<CsActivite > SelectAllActivite()
        {
            try
            {
                return Entities.GetEntityListFromQuery<CsActivite>(ParamProcedure.PARAM_ACTIVITE_RETOURNE());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool Delete(CsActivite pMarqueModel)
        {
            try
            {
                return Entities.DeleteEntity<Galatee.Entity.Model.RefActivite>(Entities.ConvertObject<Galatee.Entity.Model.RefActivite, CsActivite>(pMarqueModel));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool Delete(List<CsActivite> pMarqueModel)
        {
            try
            {
                return Entities.DeleteEntity<Galatee.Entity.Model.RefActivite>(Entities.ConvertObject<Galatee.Entity.Model.RefActivite, CsActivite>(pMarqueModel));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool Update(CsActivite pMarqueModel)
        {
            try
            {
                return Entities.UpdateEntity<Galatee.Entity.Model.RefActivite>(Entities.ConvertObject<Galatee.Entity.Model.RefActivite, CsActivite>(pMarqueModel));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool Update(List<CsActivite> pTMarqueModelCollection)
        {
            try
            {
                return Entities.UpdateEntity<Galatee.Entity.Model.RefActivite>(Entities.ConvertObject<Galatee.Entity.Model.RefActivite, CsActivite>(pTMarqueModelCollection));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool Insert(CsActivite pTcompt)
        {
            try
            {
                return Entities.InsertEntity<Galatee.Entity.Model.RefActivite>(Entities.ConvertObject<Galatee.Entity.Model.RefActivite, CsActivite>(pTcompt));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool Insert(List<CsActivite> pTcomptCollection)
        {
            try
            {
                return Entities.InsertEntity<Galatee.Entity.Model.RefActivite>(Entities.ConvertObject<Galatee.Entity.Model.RefActivite, CsActivite>(pTcomptCollection));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
