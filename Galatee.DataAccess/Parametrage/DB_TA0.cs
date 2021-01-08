using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Galatee.Structure;

namespace Galatee.DataAccess
{
    public class DB_TA0 : Galatee.DataAccess.Parametrage.DbBase
    {

        private string ConnectionString;
        public DB_TA0()
        {
           try
            {
                //ConnectionString = GalateeConfig.ConnectionStrings[Enumere.ConnexionGALADB].ConnectionString;
                ConnectionString = Session.GetSqlConnexionString();
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }
        public DB_TA0(string ConnStr)
        {
            ConnectionString = ConnStr;
        }

        private SqlConnection cn = null;

        private bool _Transaction;

        public bool Transaction
        {
            get { return _Transaction; }
            set { _Transaction = value; }

        }
        private SqlCommand cmd = null;

        public DataSet SelectAllTa0()
        {
            cn = new SqlConnection(ConnectionString);
            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = EnumProcedureStockee.SelectTa0;
           
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
                throw new Exception(EnumProcedureStockee.SelectTa0 + ":" + ex.Message);
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                    cn.Close(); // Fermeture de la connection 
                    cmd.Dispose();

            }
        }
        public void DeleteTa0(string Code)
        {
            cn = new SqlConnection(ConnectionString);
            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = EnumProcedureStockee.DeleteTa0;
            cmd.Parameters.Clear();
           
            SqlParameter paramCode = cmd.Parameters.Add("@CODE", SqlDbType.VarChar);
            paramCode.Direction = ParameterDirection.Input;
           
            // recuperer les informations de la ligne selectionnée
            paramCode.Value = Code;
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
                throw new Exception(EnumProcedureStockee.DeleteTa0 + ":" + ex.Message);
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                cn.Close();
                cmd.Dispose();
            }
        }
        public void Update(DataRow row)
        {
            cn = new SqlConnection(ConnectionString);
            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = EnumProcedureStockee.UpdateTa0;
            cmd.Parameters.Clear();

            try
            {
                SqlParameter CODE = cmd.Parameters.AddWithValue("@CODE", row["CODE"]);
                CODE.Direction = ParameterDirection.Input;
                SqlParameter LIBELLE = cmd.Parameters.AddWithValue("@LIBELLE", row["LIBELLE"]);
                LIBELLE.Direction = ParameterDirection.Input;
                SqlParameter NIVEAUH = cmd.Parameters.AddWithValue("@NIVEAUH", row["NIVEAUH"]);
                NIVEAUH.Direction = ParameterDirection.Input;
                SqlParameter NIVEAUF = cmd.Parameters.AddWithValue("@NIVEAUF", row["NIVEAUF"]);
                NIVEAUF.Direction = ParameterDirection.Input;
                SqlParameter ACCES = cmd.Parameters.AddWithValue("@ACCES", row["ACCES"]);
                ACCES.Direction = ParameterDirection.Input;
                SqlParameter ECRAN = cmd.Parameters.AddWithValue("@ECRAN", row["ECRAN"]);
                ECRAN.Direction = ParameterDirection.Input;
                SqlParameter DMAJ = cmd.Parameters.AddWithValue("@DMAJ", row["DMAJ"]);
                DMAJ.Direction = ParameterDirection.Input;
                SqlParameter TRANS = cmd.Parameters.AddWithValue("@TRANS", row["TRANS"]);
                TRANS.Direction = ParameterDirection.Input;
                SqlParameter ROWID = cmd.Parameters.AddWithValue("@ROWID", row["ROWID"]);
                ROWID.Direction = ParameterDirection.Input;

                if (cn.State == ConnectionState.Closed)
                    cn.Open();
                StartTransaction(cn);

                cmd.ExecuteNonQuery(); // Exécution de la procédure stockée
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

        public void Insert(DataRow row)
        {
            cn = new SqlConnection(ConnectionString);
            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = EnumProcedureStockee.InsertTa0;

            cmd.Parameters.Clear();

            try
            {
                SqlParameter CODE = cmd.Parameters.AddWithValue("@CODE", row["CODE"].ToString().PadLeft(6,'0'));
                CODE.Direction = ParameterDirection.Input;
                SqlParameter LIBELLE = cmd.Parameters.AddWithValue("@LIBELLE", row["LIBELLE"]);
                LIBELLE.Direction = ParameterDirection.Input;
                SqlParameter NIVEAUH = cmd.Parameters.AddWithValue("@NIVEAUH", row["NIVEAUH"]);
                NIVEAUH.Direction = ParameterDirection.Input;
                SqlParameter NIVEAUF = cmd.Parameters.AddWithValue("@NIVEAUF", row["NIVEAUF"]);
                NIVEAUF.Direction = ParameterDirection.Input;
                SqlParameter ACCES = cmd.Parameters.AddWithValue("@ACCES", row["ACCES"]);
                ACCES.Direction = ParameterDirection.Input;
                SqlParameter ECRAN = cmd.Parameters.AddWithValue("@ECRAN", row["ECRAN"]);
                ECRAN.Direction = ParameterDirection.Input;
                SqlParameter DMAJ = cmd.Parameters.AddWithValue("@DMAJ", row["DMAJ"]);
                ECRAN.Direction = ParameterDirection.Input;
                SqlParameter TRANS = cmd.Parameters.AddWithValue("@TRANS", row["TRANS"]);
                TRANS.Direction = ParameterDirection.Input;

                if (cn.State == ConnectionState.Closed)
                    cn.Open();
                StartTransaction(cn);

                cmd.ExecuteNonQuery(); // Exécution de la procédure stockée
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

        public bool Testunicite(string Code)
        {
            bool Result = false;
            try
            {
                cn = new SqlConnection(ConnectionString);
                cmd = new SqlCommand();
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = EnumProcedureStockee.SelectTa0ByKey.Trim();
                //this.VerifierDonnesSaisies(Centre, Code, Numero);
                cmd.Parameters.Clear();
                cmd.Parameters.Add("@CODE", SqlDbType.VarChar).Value = Code.PadLeft(6,'0');


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

        //public bool TestuniciteParRowId(Byte[] RowId)
        //{
        //    bool Result = false;
        //    try
        //    {
        //        cn = new SqlConnection(ConnectionString);
        //        cmd = new SqlCommand();
        //        cmd.Connection = cn;
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.CommandText = Galate.Datacess.EnumProcedureStockee.SelectTa0ByRowid.Trim();

        //        cmd.Parameters.Clear();
        //        cmd.Parameters.Add("@ROWID", SqlDbType.Timestamp).Value = RowId;

        //        if (cn.State == ConnectionState.Closed)
        //            cn.Open();
        //        StartTransaction(cn);

        //        SqlDataReader reader = cmd.ExecuteReader();
        //        if (reader.HasRows)
        //        {
        //            Result = true;
        //        }
        //        reader.Close();

        //        CommitTransaction(cmd.Transaction);

        //    }
        //    catch (Exception ex)
        //    {
        //        RollBackTransaction(cmd.Transaction);
        //        throw ex;
        //    }
        //    finally
        //    {
        //        if (cn.State == ConnectionState.Open)
        //            cn.Close(); // Fermeture de la connection 
        //        cmd.Dispose();
        //    }
        //    return Result;
        //}

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
    }
}
