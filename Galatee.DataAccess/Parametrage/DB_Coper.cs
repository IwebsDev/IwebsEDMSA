using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Galatee.Structure;
using Galatee.Entity.Model;

namespace Galatee.DataAccess
{
   public class DB_Coper : Galatee.DataAccess.Parametrage.DbBase
    {
       public List<CsCoper> SelectAllCoper()
        {
            try
            {
                return Entities.GetEntityListFromQuery<CsCoper>(CommonProcedures.RetourneTousCoper());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

       public bool Delete(CsCoper cCoper)
        {
            try
            {
                return Entities.DeleteEntity<Galatee.Entity.Model.COPER>(Entities.ConvertObject<Galatee.Entity.Model.COPER, CsCoper>(cCoper));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public CsCoper SelectCoperByCaisseId(int cCoper)
        {
            try
            {
                return Entities.GetEntityFromQuery<CsCoper>(ParamProcedure.PARAM_CAISSE_RETOURNEById(cCoper));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Delete(List<CsCoper> cCoperCollection)
        {
            try
            {
                return Entities.DeleteEntity<Galatee.Entity.Model.COPER>(Entities.ConvertObject<Galatee.Entity.Model.COPER, CsCoper>(cCoperCollection));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Update(CsCoper cCoper)
        {
            try
            {
                return Entities.UpdateEntity<Galatee.Entity.Model.COPER>(Entities.ConvertObject<Galatee.Entity.Model.COPER, CsCoper>(cCoper));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Update(List<CsCoper> cCoperCollection)
        {
            try
            {
                return Entities.UpdateEntity<Galatee.Entity.Model.COPER>(Entities.ConvertObject<Galatee.Entity.Model.COPER, CsCoper>(cCoperCollection));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Insert(CsCoper cCoper)
        {
            try
            {
                return Entities.InsertEntity<Galatee.Entity.Model.COPER>(Entities.ConvertObject<Galatee.Entity.Model.COPER, CsCoper>(cCoper));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Insert(List<CsCoper> pCoperCollection)
        {
            try
            {
                return Entities.InsertEntity<Galatee.Entity.Model.COPER>(Entities.ConvertObject<Galatee.Entity.Model.COPER, CsCoper>(pCoperCollection));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
