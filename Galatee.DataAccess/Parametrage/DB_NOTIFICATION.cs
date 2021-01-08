using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Galatee.Structure;
using Galatee.Entity.Model;


namespace Galatee.DataAccess
{
    public class DB_NOTIFICATION : Galatee.DataAccess.Parametrage.DbBase
    {
        public List<CsTypeNotificaton > SelectAllTypeNotification()
        { 
            try
            {
                return Entities.GetEntityListFromQuery<CsTypeNotificaton>(ParamProcedure.RetourneTypenOTIFICATION ());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CsNotificaton > SelectAllNotification()
        {
            try
            {
                return Entities.GetEntityListFromQuery<CsNotificaton >(ParamProcedure.RetourneNotification ());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CsNotificaton> SelectNotificationByTypeMail(string Code)
        {
            try
            {
                return Entities.GetEntityListFromQuery<CsNotificaton>(ParamProcedure.SelectNotificationByTypeMail(Code));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CsParametreSMTP > SelectAllSMTP()
        {
            try
            {
                return Entities.GetEntityListFromQuery<CsParametreSMTP>(ParamProcedure.RetourneParametreSMTP ());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool Insert(CsParametreSMTP SmtpParametre)
        {
            try
            {
                return Entities.InsertEntity<Galatee.Entity.Model.PARAMETRESMTP>(Entities.ConvertObject<Galatee.Entity.Model.PARAMETRESMTP, CsParametreSMTP>(SmtpParametre));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool Update(CsParametreSMTP SmtpParametre)
        {
            try
            {
                return Entities.UpdateEntity <Galatee.Entity.Model.PARAMETRESMTP>(Entities.ConvertObject<Galatee.Entity.Model.PARAMETRESMTP, CsParametreSMTP>(SmtpParametre));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
