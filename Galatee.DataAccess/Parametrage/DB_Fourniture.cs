using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Galatee.Structure;
using Galatee.Entity.Model;

namespace Galatee.DataAccess
{
    public class DB_Fourniture : Galatee.DataAccess.Parametrage.DbBase
    {
        public List<CsFourniture> SelectAllFourniture()
        {
            try
            {
                return Entities.GetEntityListFromQuery<CsFourniture>(CommonProcedures.RetourneTousFourniture());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Delete(CsFourniture cFourniture)
        {
            try
            {
                return Entities.DeleteEntity<Galatee.Entity.Model.FOURNITURE>(Entities.ConvertObject<Galatee.Entity.Model.FOURNITURE, CsFourniture>(cFourniture));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public CsFourniture SelectFournitureByFournitureId(int cFourniture)
        {
            try
            {
                return Entities.GetEntityFromQuery<CsFourniture>(ParamProcedure.PARAM_FOURNITURE_RETOURNEById(cFourniture));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Delete(List<CsFourniture> cFournitureCollection)
        {
            try
            {
                return Entities.DeleteEntity<Galatee.Entity.Model.FOURNITURE>(Entities.ConvertObject<Galatee.Entity.Model.FOURNITURE, CsFourniture>(cFournitureCollection));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Update(CsFourniture cFourniture)
        {
            try
            {
                return Entities.UpdateEntity<Galatee.Entity.Model.FOURNITURE>(Entities.ConvertObject<Galatee.Entity.Model.FOURNITURE, CsFourniture>(cFourniture));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Update(List<CsFourniture> cFournitureCollection)
        {
            try
            {
                return Entities.UpdateEntity<Galatee.Entity.Model.FOURNITURE>(Entities.ConvertObject<Galatee.Entity.Model.FOURNITURE, CsFourniture>(cFournitureCollection));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Insert(CsFourniture cFourniture)
        {
            try
            {
                return Entities.InsertEntity<Galatee.Entity.Model.FOURNITURE>(Entities.ConvertObject<Galatee.Entity.Model.FOURNITURE, CsFourniture>(cFourniture));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Insert(List<CsFourniture> cFournitureCollection)
        {
            try
            {
                return Entities.InsertEntity<Galatee.Entity.Model.FOURNITURE>(Entities.ConvertObject<Galatee.Entity.Model.FOURNITURE, CsFourniture>(cFournitureCollection));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

 
    }
}
