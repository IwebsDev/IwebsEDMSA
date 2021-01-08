using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;

namespace Galatee.DataAccess.Parametrage
{
    /// <summary>
    /// Classe de base des classes d'accès aux données 
    /// </summary>
    public  class DbBase : IDisposable
    {
        
        private SqlTransaction _SqlTransaction = null;
        /// <summary>
        /// Champ SqlTransaction
        /// </summary>
        protected  SqlTransaction SqlTransaction
        {
            get { 

                return _SqlTransaction; 
            
            }
            set { _SqlTransaction = value; }
        }

        /// <summary>
        /// Méthode permettant de démarrer une transaction
        /// </summary>
        /// <param name="pConnection"></param>
        /// <returns></returns>
        protected SqlTransaction BeginTransaction(SqlConnection pConnection)
        {

            try
            {
                _SqlTransaction = pConnection.BeginTransaction();
                return _SqlTransaction;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        /// <summary>
        /// Méthode permettant de valider une transaction
        /// </summary>
        /// <param name="pSqlTransaction"></param>
        protected void Commit(SqlTransaction pSqlTransaction)
        {
            try
            {
                pSqlTransaction.Commit();
            }
            catch (Exception ex )
            {                
                throw ex;
            }
        }
        /// <summary>
        /// Méthode permettant d'annuler une transaction
        /// </summary>
        /// <param name="pSqlTransaction"></param>
        protected void RollBack(SqlTransaction pSqlTransaction)
        {
            try
            {
                pSqlTransaction.Rollback();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Renseigner les paramètres  vides
        /// </summary>
        /// <param name="parameters"></param>
        protected void SetDBNullParametre(SqlParameterCollection parameters)
        {
            foreach (SqlParameter Parameter in parameters)
            {
                if (Parameter.Value == null)
                {
                    Parameter.Value = DBNull.Value;
                }
            }
        }

        #region IDisposable Membres
        /// <summary>
        /// Méthode de libération des ressources necessaire a une transaction
        /// </summary>
        public void Dispose()
        {
            this.Dispose();
        }

        #endregion
    }
}
