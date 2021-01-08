
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
    public class DBNATURECLIENT
    {

        public bool Delete(CsNatureClient entity)
        {
            try
            {
                return Entities.DeleteEntity<Galatee.Entity.Model.NATURECLIENT>(Entities.ConvertObject<Galatee.Entity.Model.NATURECLIENT, CsNatureClient>(entity));
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<CsNatureClient> GetAll()
        {
            try
            {
                return Entities.GetEntityListFromQuery<CsNatureClient>(ParamProcedure.PARAM_NATURECLIENT_RETOURNE());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public CsNatureClient GetById(CsNatureClient entity)
        {
            try
            {
                return Entities.GetEntityFromQuery<CsNatureClient>(ParamProcedure.PARAM_NATURECLIENT_RETOURNEByCODE(entity.PK_ID ));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Insert(List<CsNatureClient> pEntityCollection)
        {
            try
            {
                return Entities.InsertEntity<Galatee.Entity.Model.NATURECLIENT>(Entities.ConvertObject<Galatee.Entity.Model.NATURECLIENT, CsNatureClient>(pEntityCollection));
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public bool Update(List<CsNatureClient> pEntityCollection)
        {
            try
            {
                return Entities.UpdateEntity<Galatee.Entity.Model.NATURECLIENT>(Entities.ConvertObject<Galatee.Entity.Model.NATURECLIENT, CsNatureClient>(pEntityCollection));
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
} 


