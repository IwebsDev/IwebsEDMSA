using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Galatee.Structure;
using Galatee.Entity.Model;

namespace Galatee.DataAccess
{
    public class DBUNITECOMPTAGE
    {

        public List<CsUniteComptage> SelectAllUniteComptage()
        {
            try
            {
                return Entities.GetEntityListFromQuery<CsUniteComptage>(ParamProcedure.PARAM_UNITECOMPTAGE_RETOURNE());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Delete(CsUniteComptage pEntity)
        {
            try
            {
                return Entities.DeleteEntity<Galatee.Entity.Model.UNITECOMPTAGE>(Entities.ConvertObject<Galatee.Entity.Model.UNITECOMPTAGE, CsUniteComptage>(pEntity));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Delete(List<CsUniteComptage> pEntity)
        {
            try
            {
                return Entities.DeleteEntity<Galatee.Entity.Model.UNITECOMPTAGE>(Entities.ConvertObject<Galatee.Entity.Model.UNITECOMPTAGE, CsUniteComptage>(pEntity));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Update(CsUniteComptage pEntity)
        {
            try
            {
                return Entities.UpdateEntity<Galatee.Entity.Model.UNITECOMPTAGE>(Entities.ConvertObject<Galatee.Entity.Model.UNITECOMPTAGE, CsUniteComptage>(pEntity));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Update(List<CsUniteComptage> pEntityCollection)
        {
            try
            {
                return Entities.UpdateEntity<Galatee.Entity.Model.UNITECOMPTAGE>(Entities.ConvertObject<Galatee.Entity.Model.UNITECOMPTAGE, CsUniteComptage>(pEntityCollection));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Insert(CsUniteComptage pEntity)
        {
            try
            {
                return Entities.InsertEntity<Galatee.Entity.Model.UNITECOMPTAGE>(Entities.ConvertObject<Galatee.Entity.Model.UNITECOMPTAGE, CsUniteComptage>(pEntity));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Insert(List<CsUniteComptage> pEntityCollection)
        {
            try
            {
                return Entities.InsertEntity<Galatee.Entity.Model.UNITECOMPTAGE>(Entities.ConvertObject<Galatee.Entity.Model.UNITECOMPTAGE, CsUniteComptage>(pEntityCollection));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
