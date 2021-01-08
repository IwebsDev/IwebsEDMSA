using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Galatee.Structure;
using System.Data.SqlClient;
using Galatee.DataAccess;

namespace Galatee.DataAccess
{
    public class DB_INIT : Galatee.DataAccess.Parametrage.DbBase
    {

        public DB_INIT()
        {
            ConnectionString = Session.GetSqlConnexionString();
        }
        public DB_INIT(string ConnStr)
        {
            ConnectionString = ConnStr;
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

        #region Méthodes de mise à jour de la table INIT

        public void DeleteINIT(string Centre, string Produit, string Ntable, string Zone)
        {
            cn = new SqlConnection(ConnectionString);
            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = EnumProcedureStockee.DeleteINIT;

            cmd.Parameters.Clear();
            cmd.Parameters.Add("@CENTRE", SqlDbType.VarChar).Value = Centre;
            cmd.Parameters.Add("@PRODUIT", SqlDbType.VarChar).Value = Produit;
            cmd.Parameters.Add("@NTABLE", SqlDbType.VarChar).Value = Ntable;
            cmd.Parameters.Add("@ZONE", SqlDbType.VarChar).Value = Zone;

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
                throw new Exception(EnumProcedureStockee.DeleteINIT + ":" + ex.Message);
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                    cn.Close();
                cmd.Dispose();
            }
        }

        public void Update(List<CsInit> row)
        {
            foreach (CsInit ta in row)
            {
                int rowsAffected = -1;
                cn = new SqlConnection(ConnectionString);
                cmd = new SqlCommand();
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = EnumProcedureStockee.UpdateINIT;
                cmd.Parameters.Clear();

                try
                {
                    cmd.Parameters.Add("@CENTRE", SqlDbType.VarChar).Value = ta.CENTRE;
                    cmd.Parameters.Add("@PRODUIT", SqlDbType.VarChar).Value = ta.PRODUIT;
                    cmd.Parameters.Add("@NTABLE", SqlDbType.SmallInt).Value = int.Parse(ta.NTABLE);
                    cmd.Parameters.Add("@ZONE", SqlDbType.SmallInt).Value = int.Parse(ta.ZONE);
                    cmd.Parameters.Add("@CONTENU", SqlDbType.SmallInt).Value = ta.CONTENU;
                    cmd.Parameters.Add("@OBLIG", SqlDbType.VarChar).Value = ta.OBLIG;
                    cmd.Parameters.Add("@DMAJ", SqlDbType.DateTime).Value = ta.DMAJ;
                    cmd.Parameters.Add("@TRANS", SqlDbType.VarChar).Value = ta.TRANS;
                    cmd.Parameters.Add("@ROWID", SqlDbType.Timestamp).Value = ta.ROWID;

                    DBBase.SetDBNullParametre(cmd.Parameters);

                    if (cn.State == ConnectionState.Closed)
                        cn.Open();
                    StartTransaction(cn);

                    rowsAffected = cmd.ExecuteNonQuery();
                    //cmd.ExecuteNonQuery(); // Exécution de la procédure stockée
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
            }

        }

        public void Insert(List<CsInit> row)
        {
            foreach (CsInit ta in row)
            {
                int rowsAffected = -1;
                cn = new SqlConnection(ConnectionString);
                cmd = new SqlCommand();
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = EnumProcedureStockee.InsertINIT;

                cmd.Parameters.Clear();

                try
                {
                    cmd.Parameters.Add("@CENTRE", SqlDbType.VarChar).Value = ta.CENTRE;
                    cmd.Parameters.Add("@PRODUIT", SqlDbType.VarChar).Value = ta.PRODUIT;
                    cmd.Parameters.Add("@NTABLE", SqlDbType.SmallInt).Value = int.Parse(ta.NTABLE);
                    cmd.Parameters.Add("@ZONE", SqlDbType.SmallInt).Value = int.Parse(ta.ZONE);
                    cmd.Parameters.Add("@OBLIG", SqlDbType.VarChar).Value = ta.OBLIG;
                    cmd.Parameters.Add("@CONTENU", SqlDbType.VarChar).Value = ta.CONTENU;
                    cmd.Parameters.Add("@DMAJ", SqlDbType.DateTime).Value = ta.DMAJ;
                    cmd.Parameters.Add("@TRANS", SqlDbType.VarChar).Value = ta.TRANS;

                    DBBase.SetDBNullParametre(cmd.Parameters);     

                    if (cn.State == ConnectionState.Closed)
                        cn.Open();
                    StartTransaction(cn);

                    rowsAffected = cmd.ExecuteNonQuery();
                    //cmd.ExecuteNonQuery(); // Exécution de la procédure stockée
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
            }

        }

        public bool Testunicite(string Centre, string Produit, string Ntable, string Zone)
        {
            bool Result = false;
            try
            {
                cn = new SqlConnection(ConnectionString);
                cmd = new SqlCommand();
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = EnumProcedureStockee.SelectINITByKey.Trim();
                cmd.Parameters.Clear();
                cmd.Parameters.Add("@CENTRE", SqlDbType.VarChar).Value = Centre;
                cmd.Parameters.Add("@PRODUIT", SqlDbType.VarChar).Value = Produit;
                cmd.Parameters.Add("@NTABLE", SqlDbType.SmallInt).Value = int.Parse(Ntable);
                cmd.Parameters.Add("@ZONE", SqlDbType.SmallInt).Value = int.Parse(Zone);

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

        public DataSet SelectAll_INIT()
        {
            cn = new SqlConnection(ConnectionString);
            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = EnumProcedureStockee.SelectINIT;

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
                throw new Exception(EnumProcedureStockee.SelectINIT + ":" + ex.Message);
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                    cn.Close(); // Fermeture de la connection 
                cmd.Dispose();
            }
        }

        public DataSet SELECT_Produits()
        {
            cn = new SqlConnection(ConnectionString);
            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = EnumProcedureStockee.SelectTaAllProduit;

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
                throw new Exception(EnumProcedureStockee.SelectINIT + ":" + ex.Message);
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                    cn.Close(); // Fermeture de la connection 
                cmd.Dispose();
            }
        }
        #endregion

        #region Méthodes de gestion des transactions

        private void StartTransaction(SqlConnection _conn)
        {
            if ((_Transaction) && (_conn != null))
            {
                cmd.Transaction = this.BeginTransaction(_conn);
            }
        }

        private void CommitTransaction(SqlTransaction _pSqlTransaction)
        {
            if ((_Transaction) && (_pSqlTransaction != null))
            {
                this.Commit(_pSqlTransaction);
            }
        }

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
