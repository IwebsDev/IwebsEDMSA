using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Galatee.Structure;
using Galatee.Entity.Model;

namespace Galatee.DataAccess
{
    public class DB_APPAREILS : Galatee.DataAccess.Parametrage.DbBase
    {
        public List<CsAppareils> SelectAllAppareils()
        {
            try
            {
                return Entities.GetEntityListFromQuery<CsAppareils>(CommonProcedures.RetourneTousAppareils());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Delete(CsAppareils cAppareils)
        {
            try
            {
                return Entities.DeleteEntity<Galatee.Entity.Model.APPAREILS>(Entities.ConvertObject<Galatee.Entity.Model.APPAREILS, CsAppareils>(cAppareils));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public CsAppareils SelectAppareilsByAppareilsId(int cAppareils)
        {
            try
            {
                return Entities.GetEntityFromQuery<CsAppareils>(ParamProcedure.PARAM_APPAREILS_RETOURNEByCodeAppareil(cAppareils));

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Delete(List<CsAppareils> cAppareilsCollection)
        {
            try
            {
                return Entities.DeleteEntity<Galatee.Entity.Model.APPAREILS>(Entities.ConvertObject<Galatee.Entity.Model.APPAREILS, CsAppareils>(cAppareilsCollection));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Update(CsAppareils cAppareils)
        {
            try
            {
                return Entities.UpdateEntity<Galatee.Entity.Model.APPAREILS>(Entities.ConvertObject<Galatee.Entity.Model.APPAREILS, CsAppareils>(cAppareils));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Update(List<CsAppareils> cAppareilsCollection)
        {
            try
            {
                return Entities.UpdateEntity<Galatee.Entity.Model.APPAREILS>(Entities.ConvertObject<Galatee.Entity.Model.APPAREILS, CsAppareils>(cAppareilsCollection));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Insert(CsAppareils cAppareils)
        {
            try
            {
                return Entities.InsertEntity<Galatee.Entity.Model.APPAREILS>(Entities.ConvertObject<Galatee.Entity.Model.APPAREILS, CsAppareils>(cAppareils));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Insert(List<CsAppareils> pAppareilsCollection)
        {
            try
            {
                return Entities.InsertEntity<Galatee.Entity.Model.APPAREILS>(Entities.ConvertObject<Galatee.Entity.Model.APPAREILS, CsAppareils>(pAppareilsCollection));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
