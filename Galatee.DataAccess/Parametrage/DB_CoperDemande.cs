using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Galatee.Structure;
using Galatee.Entity.Model;


namespace Galatee.DataAccess
{
   public class DB_CoperDemande : Galatee.DataAccess.Parametrage.DbBase
    {
        public List<CsCoutDemande> SelectAllCoperDemande()
        { 
            try
            {
                return Entities.GetEntityListFromQuery<CsCoutDemande>(ParamProcedure.PARAM_COUTDEMANDE_RETOURNE());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Delete(CsCoutDemande cCoperDemande)
        {
            try
            {
                return Entities.DeleteEntity<Galatee.Entity.Model.COUTDEMANDE>(Entities.ConvertObject<Galatee.Entity.Model.COUTDEMANDE, CsCoutDemande>(cCoperDemande));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public CsCoutDemande SelectCoperDemandeByCoperDemandeId(int cCoperDemande)
        {
            try
            {
                return Entities.GetEntityFromQuery<CsCoutDemande>(ParamProcedure.PARAM_COUTDEMANDE_RETOURNEById(cCoperDemande));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Delete(List<CsCoutDemande> cCoperDemande)
        {
            try
            {
                return Entities.DeleteEntity<Galatee.Entity.Model.COUTDEMANDE>(Entities.ConvertObject<Galatee.Entity.Model.COUTDEMANDE, CsCoutDemande >(cCoperDemande));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Update(CsCoutDemande cCoperDemande)
        {
            try
            {
                return Entities.UpdateEntity<Galatee.Entity.Model.COUTDEMANDE>(Entities.ConvertObject<Galatee.Entity.Model.COUTDEMANDE, CsCoutDemande >(cCoperDemande));

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Update(List<CsCoutDemande> cCoperDemande)
        {
            try
            {
                return Entities.UpdateEntity<Galatee.Entity.Model.COUTDEMANDE>(Entities.ConvertObject<Galatee.Entity.Model.COUTDEMANDE, CsCoutDemande>(cCoperDemande));

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Insert(CsCoutDemande cCoperDemande)
        {
            try
            {
                return Entities.InsertEntity<Galatee.Entity.Model.COUTDEMANDE>(Entities.ConvertObject<Galatee.Entity.Model.COUTDEMANDE, CsCoutDemande>(cCoperDemande));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Insert(List<CsCoutDemande> cCoperDemande)
        {
            try
            {
                return Entities.InsertEntity<Galatee.Entity.Model.COUTDEMANDE>(Entities.ConvertObject<Galatee.Entity.Model.COUTDEMANDE, CsCoutDemande>(cCoperDemande));

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
