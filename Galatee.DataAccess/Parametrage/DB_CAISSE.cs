using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Galatee.Structure;
using Galatee.Entity.Model;
namespace Galatee.DataAccess

{
   
    public class DB_CAISSE : Galatee.DataAccess.Parametrage.DbBase
    {
        

        public List<CsCaisse> SelectAllCaisse()
        {
            try
            {
                return Entities.GetEntityListFromQuery<CsCaisse>(CommonProcedures.RetourneTousCaisse());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Delete(CsCaisse cCaisse)
        {
            try
            {
                return Entities.DeleteEntity<Galatee.Entity.Model.CAISSE>(Entities.ConvertObject<Galatee.Entity.Model.CAISSE, CsCaisse>(cCaisse));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public CsCaisse SelectCaisseByCaisseId(int cCaisse)
        {
            try
            {
                return Entities.GetEntityFromQuery<CsCaisse>(ParamProcedure.PARAM_CAISSE_RETOURNEById(cCaisse));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Delete(List<CsCaisse> cCaisseCollection)
        {
            try
            {
                return Entities.DeleteEntity<Galatee.Entity.Model.CAISSE>(Entities.ConvertObject<Galatee.Entity.Model.CAISSE, CsCaisse>(cCaisseCollection));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Update(CsCaisse cCaisse)
        {
            try
            {
                return Entities.UpdateEntity<Galatee.Entity.Model.CAISSE>(Entities.ConvertObject<Galatee.Entity.Model.CAISSE, CsCaisse>(cCaisse));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Update(List<CsCaisse> cCaisseCollection)
        {
            try
            {
                return Entities.UpdateEntity<Galatee.Entity.Model.CAISSE>(Entities.ConvertObject<Galatee.Entity.Model.CAISSE, CsCaisse>(cCaisseCollection));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Insert(CsCaisse cCaisse)
        {
            try
            {
                return Entities.InsertEntity<Galatee.Entity.Model.CAISSE>(Entities.ConvertObject<Galatee.Entity.Model.CAISSE, CsCaisse>(cCaisse));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Insert(List<CsCaisse> pCaissetCollection)
        {
            try
            {
                return Entities.InsertEntity<Galatee.Entity.Model.CAISSE>(Entities.ConvertObject<Galatee.Entity.Model.CAISSE, CsCaisse>(pCaissetCollection));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
