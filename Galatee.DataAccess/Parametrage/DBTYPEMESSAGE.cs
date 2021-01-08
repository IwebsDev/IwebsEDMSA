using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Galatee.Structure;
using Galatee.Entity.Model;

namespace Galatee.DataAccess
{
    public class DBTYPEMESSAGE
    {

        public List<CsTypeMessage> SelectAllTypeMessage()
        {
            try
            {
                return Entities.GetEntityListFromQuery<CsTypeMessage>(ParamProcedure.PARAM_TYPEMESSAGE_RETOURNE());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Delete(CsTypeMessage pEntity)
        {
            try
            {
                return Entities.DeleteEntity<Galatee.Entity.Model.TYPEMESSAGE>(Entities.ConvertObject<Galatee.Entity.Model.TYPEMESSAGE, CsTypeMessage>(pEntity));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Delete(List<CsTypeMessage> pEntity)
        {
            try
            {
                return Entities.DeleteEntity<Galatee.Entity.Model.TYPEMESSAGE>(Entities.ConvertObject<Galatee.Entity.Model.TYPEMESSAGE, CsTypeMessage>(pEntity));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Update(CsTypeMessage pEntity)
        {
            try
            {
                return Entities.UpdateEntity<Galatee.Entity.Model.TYPEMESSAGE>(Entities.ConvertObject<Galatee.Entity.Model.TYPEMESSAGE, CsTypeMessage>(pEntity));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Update(List<CsTypeMessage> pEntityCollection)
        {
            try
            {
                return Entities.UpdateEntity<Galatee.Entity.Model.TYPEMESSAGE>(Entities.ConvertObject<Galatee.Entity.Model.TYPEMESSAGE, CsTypeMessage>(pEntityCollection));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Insert(CsTypeMessage pEntity)
        {
            try
            {
                return Entities.InsertEntity<Galatee.Entity.Model.TYPEMESSAGE>(Entities.ConvertObject<Galatee.Entity.Model.TYPEMESSAGE, CsTypeMessage>(pEntity));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Insert(List<CsTypeMessage> pEntityCollection)
        {
            try
            {
                return Entities.InsertEntity<Galatee.Entity.Model.TYPEMESSAGE>(Entities.ConvertObject<Galatee.Entity.Model.TYPEMESSAGE, CsTypeMessage>(pEntityCollection));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
