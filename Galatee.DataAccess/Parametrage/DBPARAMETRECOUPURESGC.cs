using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Galatee.Structure;
using Galatee.Entity.Model;

namespace Galatee.DataAccess
{
    public class DBPARAMETRECOUPURESGC : Galatee.DataAccess.Parametrage.DbBase
    {
        public List<CsParametreCoupureSGC > SelectAllParametre()
        {
            try
            {
                return Entities.GetEntityListFromQuery<CsParametreCoupureSGC>(ParamProcedure.RetourneParamatreSCGC());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Delete(CsParametreCoupureSGC pParametreCoupureSGC)
        {
            try
            {
                return Entities.DeleteEntity<Galatee.Entity.Model.PARAMETRECOUPURESGC>(Entities.ConvertObject<Galatee.Entity.Model.PARAMETRECOUPURESGC, CsParametreCoupureSGC>(pParametreCoupureSGC));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Delete(List<CsParametreCoupureSGC> pParametreCoupureSGCCollection)
        {
            try
            {
                return Entities.DeleteEntity<Galatee.Entity.Model.PARAMETRECOUPURESGC>(Entities.ConvertObject<Galatee.Entity.Model.PARAMETRECOUPURESGC, CsParametreCoupureSGC>(pParametreCoupureSGCCollection));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool Update(CsParametreCoupureSGC pParametreCoupureSGC)
        {
            try
            {
                return Entities.UpdateEntity<Galatee.Entity.Model.PARAMETRECOUPURESGC>(Entities.ConvertObject<Galatee.Entity.Model.PARAMETRECOUPURESGC, CsParametreCoupureSGC>(pParametreCoupureSGC));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Update(List<CsParametreCoupureSGC> pParametreCoupureSGCCollection)
        {
            try
            {
                return Entities.UpdateEntity<Galatee.Entity.Model.PARAMETRECOUPURESGC>(Entities.ConvertObject<Galatee.Entity.Model.PARAMETRECOUPURESGC, CsParametreCoupureSGC>(pParametreCoupureSGCCollection));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool Insert(CsParametreCoupureSGC pParametreCoupureSGCCollection)
        {
            try
            {
                return Entities.InsertEntity<Galatee.Entity.Model.PARAMETRECOUPURESGC>(Entities.ConvertObject<Galatee.Entity.Model.PARAMETRECOUPURESGC, CsParametreCoupureSGC>(pParametreCoupureSGCCollection));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Insert(List<CsParametreCoupureSGC> pParametreCoupureSGCCollectionCollection)
        {
            try
            {
                return Entities.InsertEntity<Galatee.Entity.Model.PARAMETRECOUPURESGC>(Entities.ConvertObject<Galatee.Entity.Model.PARAMETRECOUPURESGC, CsParametreCoupureSGC>(pParametreCoupureSGCCollectionCollection));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public static CsParametreCoupureSGC GetById(string id)
        //{
        //    try
        //    {
        //        return Entities.GetEntityFromQuery<CsParametreCoupureSGC>(ParamProcedure.RetourneParamatreSCGC(id));
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

    }
}
