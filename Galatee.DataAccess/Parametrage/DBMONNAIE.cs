
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
    public class DBMONNAIE
    {

        public bool Delete(CsMonnaie entity)
        {
            try
            {
                return Entities.DeleteEntity<Galatee.Entity.Model.MONNAIE>(Entities.ConvertObject<Galatee.Entity.Model.MONNAIE, CsMonnaie>(entity));
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<CsMonnaie> GetAll()
        {
            try
            {
                return Entities.GetEntityListFromQuery<CsMonnaie>(ParamProcedure.PARAM_MONNAIE_RETOURNE());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public CsMonnaie GetById(CsMonnaie entity)
        {
            try
            {
                return Entities.GetEntityFromQuery<CsMonnaie>(ParamProcedure.PARAM_MONNAIE_RETOURNEById(entity.PK_ID));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Insert(List<CsMonnaie> pEntityCollection)
        {
            try
            {
                return Entities.InsertEntity<Galatee.Entity.Model.MONNAIE>(Entities.ConvertObject<Galatee.Entity.Model.MONNAIE, CsMonnaie>(pEntityCollection));
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public bool Update(List<CsMonnaie> pEntityCollection)
        {
            try
            {
                return Entities.UpdateEntity<Galatee.Entity.Model.MONNAIE>(Entities.ConvertObject<Galatee.Entity.Model.MONNAIE, CsMonnaie>(pEntityCollection));
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
} 


