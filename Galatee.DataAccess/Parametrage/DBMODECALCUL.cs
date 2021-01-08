using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Galatee.Structure;
using Galatee.Entity.Model;

namespace Galatee.DataAccess
{
    public class DBMODECALCUL
    {

        public List<CsModeCalcul> SelectAllModeCalcul()
        {
            try
            {
                return Entities.GetEntityListFromQuery<CsModeCalcul>(ParamProcedure.PARAM_MODECALCUL_RETOURNE());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Delete(CsModeCalcul pEntity)
        {
            try
            {
                return Entities.DeleteEntity<Galatee.Entity.Model.MODECALCUL>(Entities.ConvertObject<Galatee.Entity.Model.MODECALCUL, CsModeCalcul>(pEntity));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Delete(List<CsModeCalcul> pEntity)
        {
            try
            {
                return Entities.DeleteEntity<Galatee.Entity.Model.MODECALCUL>(Entities.ConvertObject<Galatee.Entity.Model.MODECALCUL, CsModeCalcul>(pEntity));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Update(CsModeCalcul pEntity)
        {
            try
            {
                return Entities.UpdateEntity<Galatee.Entity.Model.MODECALCUL>(Entities.ConvertObject<Galatee.Entity.Model.MODECALCUL, CsModeCalcul>(pEntity));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Update(List<CsModeCalcul> pEntityCollection)
        {
            try
            {
                return Entities.UpdateEntity<Galatee.Entity.Model.MODECALCUL>(Entities.ConvertObject<Galatee.Entity.Model.MODECALCUL, CsModeCalcul>(pEntityCollection));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Insert(CsModeCalcul pEntity)
        {
            try
            {
                return Entities.InsertEntity<Galatee.Entity.Model.MODECALCUL>(Entities.ConvertObject<Galatee.Entity.Model.MODECALCUL, CsModeCalcul>(pEntity));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Insert(List<CsModeCalcul> pEntityCollection)
        {
            try
            {
                return Entities.InsertEntity<Galatee.Entity.Model.MODECALCUL>(Entities.ConvertObject<Galatee.Entity.Model.MODECALCUL, CsModeCalcul>(pEntityCollection));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
