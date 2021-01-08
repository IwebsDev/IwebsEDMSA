
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
    public class DBREDEVANCE
    {

        public bool Delete(CsRedevance entity)
        {
            try
            {
                return Entities.DeleteEntity<Galatee.Entity.Model.REDEVANCE>(Entities.ConvertObject<Galatee.Entity.Model.REDEVANCE, CsRedevance>(entity));
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<CsRedevance> GetAll()
        {
            try
            {
                return Entities.GetEntityListFromQuery<CsRedevance>(ParamProcedure.PARAM_REDEVANCE_RETOURNE());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public CsRedevance GetById(CsRedevance entity)
        {
            try
            {
                return Entities.GetEntityFromQuery<CsRedevance>(ParamProcedure.PARAM_REDEVANCERETOURNEByPkId(entity.PK_ID));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Insert(List<CsRedevance> pEntityCollection)
        {
            try
            {
                return Entities.InsertEntity<Galatee.Entity.Model.REDEVANCE>(Entities.ConvertObject<Galatee.Entity.Model.REDEVANCE, CsRedevance>(pEntityCollection));
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public bool Update(List<CsRedevance> pEntityCollection)
        {
            try
            {
                return Entities.UpdateEntity<Galatee.Entity.Model.REDEVANCE>(Entities.ConvertObject<Galatee.Entity.Model.REDEVANCE, CsRedevance>(pEntityCollection));
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
} 


