using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Galatee.Structure;
using Galatee.Entity.Model;

namespace Galatee.DataAccess
{
    public class DBAPPLICATIONTAXE
    {

        public List<CsApplicationTaxe> SelectAllApplicationTaxe()
        {
            try
            {
                return Entities.GetEntityListFromQuery<CsApplicationTaxe>(ParamProcedure.PARAM_APPLICATIONTAXE_RETOURNE());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Delete(CsApplicationTaxe pEntity)
        {
            try
            {
                return Entities.DeleteEntity<Galatee.Entity.Model.APPLICATIONTAXE>(Entities.ConvertObject<Galatee.Entity.Model.APPLICATIONTAXE, CsApplicationTaxe>(pEntity));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Delete(List<CsApplicationTaxe> pEntity)
        {
            try
            {
                return Entities.DeleteEntity<Galatee.Entity.Model.APPLICATIONTAXE>(Entities.ConvertObject<Galatee.Entity.Model.APPLICATIONTAXE, CsApplicationTaxe>(pEntity));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Update(CsApplicationTaxe pEntity)
        {
            try
            {
                return Entities.UpdateEntity<Galatee.Entity.Model.APPLICATIONTAXE>(Entities.ConvertObject<Galatee.Entity.Model.APPLICATIONTAXE, CsApplicationTaxe>(pEntity));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Update(List<CsApplicationTaxe> pEntityCollection)
        {
            try
            {
                return Entities.UpdateEntity<Galatee.Entity.Model.APPLICATIONTAXE>(Entities.ConvertObject<Galatee.Entity.Model.APPLICATIONTAXE, CsApplicationTaxe>(pEntityCollection));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Insert(CsApplicationTaxe pEntity)
        {
            try
            {
                return Entities.InsertEntity<Galatee.Entity.Model.APPLICATIONTAXE>(Entities.ConvertObject<Galatee.Entity.Model.APPLICATIONTAXE, CsApplicationTaxe>(pEntity));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Insert(List<CsApplicationTaxe> pEntityCollection)
        {
            try
            {
                return Entities.InsertEntity<Galatee.Entity.Model.APPLICATIONTAXE>(Entities.ConvertObject<Galatee.Entity.Model.APPLICATIONTAXE, CsApplicationTaxe>(pEntityCollection));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
