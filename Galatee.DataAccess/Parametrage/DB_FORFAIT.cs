using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Galatee.Structure;
using Galatee.Entity.Model;

namespace Galatee.DataAccess
{
    /// <summary>
    /// DB_FORFAIT
    /// </summary>
    public class DB_FORFAIT /*: Galatee.DataAccess.Parametrage.DbBase*/
    {
        /*
        /// <summary>
        /// DB_FORFAIT
        /// </summary>
        public DB_FORFAIT()
        {
            ConnectionString = Session.GetSqlConnexionString();
        }
        /// <summary>
        /// DB_FORFAIT
        /// </summary>
        /// <param name="ConnStr"></param>
        public DB_FORFAIT(string ConnStr)
        {
            ConnectionString = ConnStr;
        }
        /// <summary>
        /// ConnectionString
        /// </summary>
        private string ConnectionString;// = string.Empty;
        private SqlConnection cn = null;

        private bool _Transaction;
        /// <summary>
        /// Transaction
        /// </summary>
        public bool Transaction
        {
            get { return _Transaction; }
            set { _Transaction = value; }

        }

        private SqlCommand cmd = null;
        #region Méthodes de mise à jour de la table FORFAIT
        /// <summary>
        /// SelectAll_FORFAIT
        /// </summary>
        /// <returns></returns>
        public DataSet SelectAll_FORFAIT()
        {
            cn = new SqlConnection(ConnectionString);
            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = EnumProcedureStockee.SelectFORFAIT;
           
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
                throw new Exception(EnumProcedureStockee.SelectFORFAIT + ":" + ex.Message);
            }
            finally
            {

                if (cn.State == ConnectionState.Open)
                    cn.Close(); // Fermeture de la connection 
                    cmd.Dispose();
            }
        }
        /// <summary>
        /// Delete_FORFAIT
        /// </summary>
        /// <param name="Centre"></param>
        /// <param name="Produit"></param>
        /// <param name="Forfait"></param>
        public void Delete_FORFAIT(string Centre, string Produit, string Forfait)
        {
            cn = new SqlConnection(ConnectionString);
            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = EnumProcedureStockee.DeleteFORFAIT;
            cmd.Parameters.Clear();

            cmd.Parameters.Add("@CENTRE", SqlDbType.VarChar).Value = Centre;
            cmd.Parameters.Add("@PRODUIT", SqlDbType.VarChar).Value = Produit;
            cmd.Parameters.Add("@FORFAIT", SqlDbType.VarChar).Value = Forfait;

            
            try
            {
                

                if (cn.State == ConnectionState.Closed)
                    cn.Open();
                StartTransaction(cn);
                cmd.ExecuteNonQuery();
                CommitTransaction(cmd.Transaction);
               
            }
            catch (Exception ex)
            {
                RollBackTransaction(cmd.Transaction);
                throw new Exception(EnumProcedureStockee.DeleteFORFAIT + ":" + ex.Message);
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                cn.Close();
                cmd.Dispose();
            }
        }
        /// <summary>
        /// MiseAJour_FORFAIT
        /// </summary>
        /// <param name="row"></param>
        public void MiseAJour_FORFAIT(List<CsForfait> rows)
        {
            cn = new SqlConnection(ConnectionString);
            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = EnumProcedureStockee.UpdateFORFAIT.Trim();


            try
            {
                foreach (CsForfait row in rows)
                {
                    cmd.Parameters.Clear();

                    //cmd.Parameters.Add("@CENTRE", SqlDbType.VarChar).Value = row.CENTRE;
                    //cmd.Parameters.Add("@PRODUIT", SqlDbType.VarChar).Value = row.PRODUIT;
                    //cmd.Parameters.Add("@FORFAIT", SqlDbType.VarChar).Value = row.FORFAIT;
                    cmd.Parameters.Add("@LIBELLE", SqlDbType.VarChar).Value = row.LIBELLE;
                    cmd.Parameters.Add("@DMAJ", SqlDbType.DateTime).Value = row.DMAJ;
                    cmd.Parameters.Add("@TRANS", SqlDbType.VarChar).Value = row.TRANS;
                    cmd.Parameters.Add("@ROWID", SqlDbType.Timestamp).Value = row.ROWID;

                    DBBase.SetDBNullParametre(cmd.Parameters);

                    if (cn.State == ConnectionState.Closed)
                        cn.Open();
                    StartTransaction(cn);
                    cmd.ExecuteNonQuery(); // Exécution de la procédure stockée
                }

                CommitTransaction(cmd.Transaction);
                
            }
            
            catch (Exception ex)
            {
                RollBackTransaction(cmd.Transaction);
                throw new Exception(EnumProcedureStockee.UpdateFORFAIT + ":" + ex.Message);
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                cn.Close(); // Fermeture de la connection 
                cmd.Dispose();
            }
        }
        /// <summary>
        /// Insertion_FORFAIT
        /// </summary>
        /// <param name="row"></param>
        public void Insertion_FORFAIT(List<CsForfait> rows)
        {
            cn = new SqlConnection(ConnectionString);
            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = EnumProcedureStockee.InsertFORFAIT.Trim();


            try
            {
                foreach (CsForfait row in rows)
                {
                    cmd.Parameters.Clear();
                    //cmd.Parameters.Add("@CENTRE", SqlDbType.VarChar).Value = row.CENTRE;
                    //cmd.Parameters.Add("@PRODUIT", SqlDbType.VarChar).Value = row.PRODUIT;
                    //cmd.Parameters.Add("@FORFAIT", SqlDbType.VarChar).Value = row.FORFAIT;
                    cmd.Parameters.Add("@LIBELLE", SqlDbType.VarChar).Value = row.LIBELLE;
                    cmd.Parameters.Add("@DMAJ", SqlDbType.DateTime).Value = row.DMAJ;
                    cmd.Parameters.Add("@TRANS", SqlDbType.VarChar).Value = row.TRANS;

                    DBBase.SetDBNullParametre(cmd.Parameters);

                    if (cn.State == ConnectionState.Closed)
                        cn.Open();
                    StartTransaction(cn);
                    cmd.ExecuteNonQuery(); // Exécution de la procédure stockée
                }

                CommitTransaction(cmd.Transaction);
                
            }
            catch (Exception ex)
            {
                RollBackTransaction(cmd.Transaction);
                throw new Exception(EnumProcedureStockee.InsertFORFAIT + ":" + ex.Message);
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                cn.Close(); // Fermeture de la connection 
                cmd.Dispose();
            }
        }
        /// <summary>
        /// Testunicite_FORFAIT
        /// </summary>
        /// <param name="Centre"></param>
        /// <param name="Produit"></param>
        /// <param name="Forfait"></param>
        /// <returns></returns>
        public bool Testunicite_FORFAIT(string Centre, string Produit, string Forfait)
        {
            bool Result = false;
            try
            {
                cn = new SqlConnection(ConnectionString);
                cmd = new SqlCommand();
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = EnumProcedureStockee.SelectFORFAITByKey.Trim();
                cmd.Parameters.Clear();

                cmd.Parameters.Add("@CENTRE", SqlDbType.VarChar).Value = Centre;
                cmd.Parameters.Add("@PRODUIT", SqlDbType.VarChar).Value = Produit;
                cmd.Parameters.Add("@FORFAIT", SqlDbType.VarChar).Value = Forfait;


                DBBase.SetDBNullParametre(cmd.Parameters);

                if (cn.State == ConnectionState.Closed)
                    cn.Open();
                StartTransaction(cn);
                SqlDataReader reader = cmd.ExecuteReader();


                if (reader.HasRows)
                {
                    Result = true;
                }
                reader.Close();
                CommitTransaction(cmd.Transaction);

            }
            catch (Exception ex)
            {
                RollBackTransaction(cmd.Transaction);
                throw ex;
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                    cn.Close(); // Fermeture de la connection 
                cmd.Dispose();
            }
            return Result;
        }
        /// <summary>
        /// Testunicite_FORFAITByRowId
        /// </summary>
        /// <param name="RowId"></param>
        /// <returns></returns>
        public bool Testunicite_FORFAITByRowId(Byte[] RowId)
        {
            bool Result = false;
            try
            {
                cn = new SqlConnection(ConnectionString);
                cmd = new SqlCommand();
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = EnumProcedureStockee.SelectFORFAITByRowId.Trim();
                cmd.Parameters.Clear();

                cmd.Parameters.Add("@ROWID", SqlDbType.Timestamp).Value = RowId;

                if (cn.State == ConnectionState.Closed)
                    cn.Open();
                StartTransaction(cn);
                SqlDataReader reader = cmd.ExecuteReader();


                if (reader.HasRows)
                {
                    Result = true;
                }
                reader.Close();
                CommitTransaction(cmd.Transaction);

            }
            catch (Exception ex)
            {
                RollBackTransaction(cmd.Transaction);
                throw ex;
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                    cn.Close(); // Fermeture de la connection 
                cmd.Dispose();
            }
            return Result;
        }
        /// <summary>
        /// StartTransaction
        /// </summary>
        /// <param name="_conn"></param>
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

        */

        public List<CsForfait> SelectAllForfait()
        {
            try
            {
                return Entities.GetEntityListFromQuery<CsForfait>(ParamProcedure.PARAM_FORFAIT_RETOURNE());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CsForfait> SelectForfaitByForfaitCentreProduit(CsForfait pForfait)
        {
            try
            {
                return Entities.GetEntityListFromQuery<CsForfait>(ParamProcedure.PARAM_FORFAITRETOURNEByIdProduitIdForfaitId(pForfait.PK_ID , pForfait.FK_IDCENTRE, pForfait.FK_IDPRODUIT));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Delete(CsForfait pForfaits)
        {
            try
            {
                return Entities.DeleteEntity<Galatee.Entity.Model.FORFAIT>(Entities.ConvertObject<Galatee.Entity.Model.FORFAIT, CsForfait>(pForfaits));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Delete(List<CsForfait> pForfaitsCollection)
        {
            try
            {
                return Entities.DeleteEntity<Galatee.Entity.Model.FORFAIT>(Entities.ConvertObject<Galatee.Entity.Model.FORFAIT, CsForfait>(pForfaitsCollection));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Update(CsForfait pForfaits)
        {
            try
            {
                return Entities.UpdateEntity<Galatee.Entity.Model.FORFAIT>(Entities.ConvertObject<Galatee.Entity.Model.FORFAIT, CsForfait>(pForfaits));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Update(List<CsForfait> pForfaitsCollection)
        {
            try
            {
                return Entities.UpdateEntity<Galatee.Entity.Model.FORFAIT>(Entities.ConvertObject<Galatee.Entity.Model.FORFAIT, CsForfait>(pForfaitsCollection));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Insert(CsForfait pForfaits)
        {
            try
            {
                return Entities.InsertEntity<Galatee.Entity.Model.FORFAIT>(Entities.ConvertObject<Galatee.Entity.Model.FORFAIT, CsForfait>(pForfaits));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Insert(List<CsForfait> pForfaitsCollection)
        {
            try
            {
                return Entities.InsertEntity<Galatee.Entity.Model.FORFAIT>(Entities.ConvertObject<Galatee.Entity.Model.FORFAIT, CsForfait>(pForfaitsCollection));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

