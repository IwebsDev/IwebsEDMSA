using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using  System.Data.SqlClient;
using Galatee.Structure;
using System.Data;

namespace Galatee.DataAccess
{
    public static class Transaction
    {
        public static SqlCommand OpenTransaction(Enumere.DataBase pBase)
        {
            try
            {
                switch (pBase)
                {
                    case Enumere.DataBase.Galadb:
                        return Tools.Utility.OpenTransation(Session.GetSqlConnexionString());
                    case Enumere.DataBase.Abo07:
                        return Tools.Utility.OpenTransation(Session.GetSqlConnexionStringAbo07());
                    default:
                        return Tools.Utility.OpenTransation(Session.GetSqlConnexionString());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static object OpenTransactionEntityModel(Enumere.DataBase pBase)
        {
            try
            {
                return Galatee.Entity.Model.CommonProcedures.OpenTransaction(pBase);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Commit transaction
        /// </summary>
        /// <param name="pCommand"></param>
        /// <returns></returns>
        public static bool CommitTransaction(SqlCommand pCommand)
        //public static bool CommitTransaction(object pCommand)

        {
            try
            {
                return Tools.Utility.Commit(pCommand);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (pCommand != null)
                {
                    //if (pCommand.Connection.State == ConnectionState.Open)
                    //    pCommand.Connection.Close();
                    //pCommand.Connection.Dispose();
                    //pCommand.Dispose();
                }
            }
        }
        /// <summary>
        /// RollBack transaction
        /// </summary>
        /// <param name="pCommand"></param>
        /// <returns></returns>
        public static bool RollBackTransaction(SqlCommand pCommand)
        //public static bool RollBackTransaction(object pCommand)

        {
            try
            {
                return Tools.Utility.Rollback(pCommand);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (pCommand != null)
                {
                    //if (pCommand.Connection.State == ConnectionState.Open)
                    //    pCommand.Connection.Close();
                    //pCommand.Connection.Dispose();
                    //pCommand.Dispose();
                }
            }
        }
    }
}
