using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;

using Galatee.Structure;


namespace Galatee.DataAccess
{
   public class DB_REGROU : Galatee.DataAccess.Parametrage.DbBase
    {
        public DB_REGROU()
        {
            ConnectionString = Session.GetSqlConnexionString();
        }
        public DB_REGROU(string ConnStr)
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

        #region Méthodes de mise à jour de la table REGROU

        public DataSet SelectAll_REGROU()
        {
            cn = new SqlConnection(ConnectionString);
            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = EnumProcedureStockee.SelectREGROU;

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
                throw new Exception(EnumProcedureStockee.SelectREGROU + ":" + ex.Message);
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                    cn.Close(); // Fermeture de la connection 
                cmd.Dispose();
            }
        }

        public void Delete_REGROU(string Centre, string CodeRegroupement, string CodeProduit)
        {
            cn = new SqlConnection(ConnectionString);
            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = EnumProcedureStockee.DeleteREGROU;
            cmd.Parameters.Clear();

            SqlParameter paramCentre = cmd.Parameters.Add("@Centre", SqlDbType.VarChar);
            paramCentre.Direction = ParameterDirection.Input;

            SqlParameter paramRegrou = cmd.Parameters.Add("@Regrou", SqlDbType.VarChar);
            paramRegrou.Direction = ParameterDirection.Input;

            SqlParameter paramProduit = cmd.Parameters.Add("@Produit", SqlDbType.VarChar);
            paramProduit.Direction = ParameterDirection.Input;

            // recuperer les informations de la ligne selectionnée
            paramCentre.Value = Centre;
            paramRegrou.Value = CodeRegroupement;
            paramProduit.Value = CodeProduit;

            DBBase.SetDBNullParametre(cmd.Parameters);

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
                throw new Exception(EnumProcedureStockee.DeleteREGROU + ":" + ex.Message);
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                    cn.Close();
                cmd.Dispose();
            }
        }

        public void MiseAJour_REGROU(List<CsRegrou> rows)
        {
            cn = new SqlConnection(ConnectionString);
            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = EnumProcedureStockee.UpdateREGROU.Trim();



            try
            {
                foreach (CsRegrou row in rows)
                {
                    cmd.Parameters.Clear();

                    cmd.Parameters.Add("@CENTRE", SqlDbType.VarChar).Value = row.CENTRE;
                    cmd.Parameters.Add("@REGROU", SqlDbType.VarChar).Value = row.REGROU;
                    cmd.Parameters.Add("@PRODUIT", SqlDbType.VarChar).Value = row.PRODUIT;
                    cmd.Parameters.Add("@NOM", SqlDbType.VarChar).Value = row.NOM;
                    cmd.Parameters.Add("@CUBGEN", SqlDbType.Int).Value = row.CUBGEN;
                    cmd.Parameters.Add("@CUBDIV", SqlDbType.Int).Value = row.CUBDIV;
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
                throw new Exception(EnumProcedureStockee.UpdateREGROU + ":" + ex.Message);
            }
                finally
                {
                    if (cn.State == ConnectionState.Open)
                        cn.Close(); // Fermeture de la connection 
                    cmd.Dispose();
                }
        }

        public void Insertion_REGROU(List<CsRegrou> rows)
        {
            cn = new SqlConnection(ConnectionString);
            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = EnumProcedureStockee.InsertREGROU.Trim();


           try
            {
                foreach (CsRegrou row in rows)
                {
                    cmd.Parameters.Clear();

                    cmd.Parameters.Add("@CENTRE", SqlDbType.VarChar).Value = row.CENTRE;
                    cmd.Parameters.Add("@PRODUIT", SqlDbType.VarChar).Value = row.PRODUIT;
                    cmd.Parameters.Add("@NOM", SqlDbType.VarChar).Value = row.NOM;
                    cmd.Parameters.Add("@REGROU", SqlDbType.VarChar).Value = row.REGROU;
                    cmd.Parameters.Add("@CUBGEN", SqlDbType.Int).Value = row.CUBGEN;
                    cmd.Parameters.Add("@CUBDIV", SqlDbType.Int).Value = row.CUBDIV;
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
                throw new Exception(EnumProcedureStockee.InsertREGROU + ":" + ex.Message);
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                    cn.Close(); // Fermeture de la connection 
                cmd.Dispose();
            }
        }

        public bool Testunicite_REGROU(string Centre, string CodeRegroupement, string CodeProduit)
        {
            bool Result = false;
            try
            {
                cn = new SqlConnection(ConnectionString);
                cmd = new SqlCommand();
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = EnumProcedureStockee.SelectREGROUByKey.Trim();


                cmd.Parameters.Clear();
                cmd.Parameters.Add("@CENTRE", SqlDbType.VarChar).Value = Centre;
                cmd.Parameters.Add("@REGROU", SqlDbType.VarChar).Value = CodeRegroupement;
                cmd.Parameters.Add("@PRODUIT", SqlDbType.VarChar).Value = CodeProduit;


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
                throw new Exception(EnumProcedureStockee.SelectREGROUByKey + ":" + ex.Message);
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                    cn.Close(); // Fermeture de la connection 
                cmd.Dispose();
            }
            return Result;
        }

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
