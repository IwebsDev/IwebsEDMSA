using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Galatee.Structure;
using Galatee.Entity.Model;

namespace Galatee.DataAccess
{
    public class DBMOIS
    {

        public List<CsMois> SelectAllMois()
        {
            try
            {
                return Entities.GetEntityListFromQuery<CsMois>(ParamProcedure.PARAM_MOIS_RETOURNE());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Delete(CsMois pEntity)
        {
            try
            {
                return Entities.DeleteEntity<Galatee.Entity.Model.MOIS>(Entities.ConvertObject<Galatee.Entity.Model.MOIS, CsMois>(pEntity));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Delete(List<CsMois> pEntity)
        {
            try
            {
                return Entities.DeleteEntity<Galatee.Entity.Model.MOIS>(Entities.ConvertObject<Galatee.Entity.Model.MOIS, CsMois>(pEntity));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Update(CsMois pEntity)
        {
            try
            {
                return Entities.UpdateEntity<Galatee.Entity.Model.MOIS>(Entities.ConvertObject<Galatee.Entity.Model.MOIS, CsMois>(pEntity));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Update(List<CsMois> pEntityCollection)
        {
            try
            {
                return Entities.UpdateEntity<Galatee.Entity.Model.MOIS>(Entities.ConvertObject<Galatee.Entity.Model.MOIS, CsMois>(pEntityCollection));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Insert(CsMois pEntity)
        {
            try
            {
                return Entities.InsertEntity<Galatee.Entity.Model.MOIS>(Entities.ConvertObject<Galatee.Entity.Model.MOIS, CsMois>(pEntity));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Insert(List<CsMois> pEntityCollection)
        {
            try
            {
                return Entities.InsertEntity<Galatee.Entity.Model.MOIS>(Entities.ConvertObject<Galatee.Entity.Model.MOIS, CsMois>(pEntityCollection));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
