using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Galatee.Structure;
using Galatee.Entity.Model;

namespace Galatee.DataAccess
{
   public class DB_CoutCoper : Galatee.DataAccess.Parametrage.DbBase
    {

        public List<CsCoutCoper> SelectAllCoutCoper()
        {
            try
            {
                //return Entities.GetEntityListFromQuery<CsCoutCoper>(CommonProcedures.RetourneTousCoutCoper());
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Delete(CsCoutCoper cCoutCoper)
        {
            try
            {
                return true;

                //return Entities.DeleteEntity<Galatee.Entity.Model.COUTDEMANDE>(Entities.ConvertObject<Galatee.Entity.Model.COUTDEMANDE, CsCoutCoper>(cCoutCoper));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public CsCoutCoper SelectCoperByCoperId(int cCoutCoper)
        {
            try
            {
                //return Entities.GetEntityFromQuery<CsCoutCoper>(ParamProcedure.PARAM_COUTCOPER_RETOURNEById(cCoutCoper));
                return null;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Delete(List<CsCoutCoper> cCoutCoper)
        {
            try
            {
                return true;

                //return Entities.DeleteEntity<Galatee.Entity.Model.COUTCOPER>(Entities.ConvertObject<Galatee.Entity.Model.COUTCOPER, CsCoutCoper>(cCoutCoper));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Update(CsCoutCoper cCoutCoper)
        {
            try
            {
                return true;

                //return Entities.UpdateEntity<Galatee.Entity.Model.COUTCOPER>(Entities.ConvertObject<Galatee.Entity.Model.COUTCOPER, CsCoutCoper>(cCoutCoper));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Update(List<CsCoutCoper> cCoutCoper)
        {
            try
            {
                //return Entities.UpdateEntity<Galatee.Entity.Model.COUTCOPER>(Entities.ConvertObject<Galatee.Entity.Model.COUTCOPER, CsCoutCoper>(cCoutCoper));
                return true;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Insert(CsCoutCoper cCoutCoper)
        {
            try
            {
                //return Entities.InsertEntity<Galatee.Entity.Model.COUTCOPER>(Entities.ConvertObject<Galatee.Entity.Model.COUTCOPER, CsCoutCoper>(cCoutCoper));
                return true;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Insert(List<CsCoutCoper> cCoutCoper)
        {
            try
            {
                //return Entities.InsertEntity<Galatee.Entity.Model.COUTCOPER>(Entities.ConvertObject<Galatee.Entity.Model.COUTCOPER, CsCoutCoper>(cCoutCoper));
                return true;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



    }
}
