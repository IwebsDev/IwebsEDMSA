using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Galatee.Structure;

namespace Galatee.DataAccess
{
    /// <summary>
    /// DB_CTAX
    /// </summary>
    public class DB_COMMON : Galatee.DataAccess.Parametrage.DbBase
    {
        /// <summary>
        /// DB_CTAX
        /// </summary>
        public DB_COMMON()
        {
            ConnectionString = Session.GetSqlConnexionString();
        }

         private string ConnectionString;
         private SqlConnection cn = null;

         private bool _Transaction;

         public bool Transaction
         {
             get { return _Transaction; }
             set { _Transaction = value; }

         }

         private SqlCommand cmd = null;


        #region Méthodes de mise à jour de la table X
        /// <summary>
        
        /// </summary>
        /// <returns></returns>
         public DataSet SelectCTALLTableContent(string procedureName)
        {
            cn = new SqlConnection(ConnectionString);
            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = procedureName;
            try
            {
                if (cn.State == ConnectionState.Closed)
                    cn.Open();
                StartTransaction(cn);

                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmd;
                DataSet ds = new DataSet();
                adapter.Fill(ds);

                CommitTransaction(cmd.Transaction);

                return ds;
            }
            catch (Exception ex)
            {
                RollBackTransaction(cmd.Transaction);
                throw new Exception(EnumProcedureStockee.SelectTCOMPT + ":" + ex.Message);
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                    cn.Close(); // Fermeture de la connection 
                cmd.Dispose();
            }
        }
        
        private void StartTransaction(SqlConnection _conn)
        {
            if ((_Transaction) && (_conn != null))
            {
                cmd.Transaction = this.BeginTransaction(_conn);
            }
        }
        /// <summary>
        /// CommitTransaction
        /// </summary>
        /// <param name="_pSqlTransaction"></param>
        private void CommitTransaction(SqlTransaction _pSqlTransaction)
        {
            if ((_Transaction) && (_pSqlTransaction != null))
            {
                this.Commit(_pSqlTransaction);
            }
        }
        /// <summary>
        /// RollBackTransaction
        /// </summary>
        /// <param name="_pSqlTransaction"></param>
        private void RollBackTransaction(SqlTransaction _pSqlTransaction)
        {
            if ((_Transaction) && (_pSqlTransaction != null))
            {
                this.RollBack(_pSqlTransaction);
            }

        }

        #endregion
    }
}
