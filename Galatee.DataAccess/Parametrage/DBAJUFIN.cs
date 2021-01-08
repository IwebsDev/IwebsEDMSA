
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Galatee.Structure;
using System.ComponentModel;
using Galatee.Entity.Model;

namespace Galatee.DataAccess
{
    [DataObject]
    public class DBAJUFIN
    {

        public bool Delete(CsAjufin entity)
        {
            try
            {
                return Entities.DeleteEntity<Galatee.Entity.Model.AJUFIN>(Entities.ConvertObject<Galatee.Entity.Model.AJUFIN, CsAjufin>(entity));
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<CsAjufin> GetAll()
        {
            try
            {
                return Entities.GetEntityListFromQuery<CsAjufin>(ParamProcedure.PARAM_AJUFIN_RETOURNE());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public CsAjufin GetById(CsAjufin entity)
        {
            try
            {
                return Entities.GetEntityFromQuery<CsAjufin>(ParamProcedure.PARAM_AJUFIN_RETOURNEById(entity.PK_ID));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Insert(List<CsAjufin> pEntityCollection)
        {
            try
            {
                return Entities.InsertEntity<Galatee.Entity.Model.AJUFIN>(Entities.ConvertObject<Galatee.Entity.Model.AJUFIN, CsAjufin>(pEntityCollection));
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public bool Update(List<CsAjufin> pEntityCollection)
        {
            try
            {
                return Entities.UpdateEntity<Galatee.Entity.Model.AJUFIN>(Entities.ConvertObject<Galatee.Entity.Model.AJUFIN, CsAjufin>(pEntityCollection));
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
} 


