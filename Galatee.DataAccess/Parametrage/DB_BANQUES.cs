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
    /// DB_BANQUES2
    /// </summary>
    public class DB_BANQUES /*: Galatee.DataAccess.Parametrage.DbBase*/
    {
        /*
        /// <summary>
        /// DB_BANQUES2
        /// </summary>
        public DB_BANQUES()
        {
            ConnectionString = Session.GetSqlConnexionString();
        }
        /// <summary>
        /// DB_BANQUES2
        /// </summary>
        /// <param name="ConnStr"></param>
        public DB_BANQUES(string ConnStr)
        {
            ConnectionString = ConnStr;
        }
        /// <summary>
        /// ConnectionString
        /// </summary>
        private string ConnectionString;
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

        #region Méthodes de mise à jour de la table BANQUE dans GALADB2

        public List<aBanque> SelectAllBanque()
        {
            cn = new SqlConnection(ConnectionString);
            try
            {
                if (cn.State == ConnectionState.Closed)
                    cn.Open();
                cmd = new SqlCommand
                {
                    Connection = cn,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = EnumProcedureStockee.SelectBANQUE
                };
                IDataReader reader = cmd.ExecuteReader();
                var rows = new List<aBanque>();
                Fill(reader, rows, int.MinValue, int.MaxValue);
                reader.Close();
                return rows;
            }
            catch (Exception ex)
            {
                throw new Exception(EnumProcedureStockee.SelectBANQUE + ":" + ex.Message);
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                    cn.Close(); // Fermeture de la connection 
                cmd.Dispose();
            }
        }

        public bool Delete(aBanque pBanque)
        {
            try
            {
                cn = new SqlConnection(ConnectionString);
                cmd = new SqlCommand
                {
                    Connection = cn,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = EnumProcedureStockee.DeleteBANQUE
                };
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@BANQUE", pBanque.BANQUE);
                cmd.Parameters.AddWithValue("@GUICHET", pBanque.GUICHET);
                if (cn.State == ConnectionState.Closed)
                    cn.Open();
                StartTransaction(cn);
                int rowsAffected = cmd.ExecuteNonQuery();
                CommitTransaction(cmd.Transaction);
                return Convert.ToBoolean(rowsAffected);
            }
            catch (Exception ex)
            {
                RollBackTransaction(cmd.Transaction);
                throw new Exception(EnumProcedureStockee.DeleteBANQUE + ":" + ex.Message);
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                    cn.Close();
                cmd.Dispose();
            }
        }

        public bool Delete(List<aBanque> pBanqueCollection)
        {
            int number = 0;
            foreach (aBanque entity in pBanqueCollection)
            {
                if (Delete(entity))
                {
                    number++;
                }
            }
            return number != 0;
        }

        private bool Update(aBanque pBanque)
        {
            cn = new SqlConnection(ConnectionString);
            cmd = new SqlCommand
            {
                Connection = cn,
                CommandType = CommandType.StoredProcedure,
                CommandText = EnumProcedureStockee.UpdateBANQUE
            };
            cmd.Parameters.Clear();

            try
            {
                cmd.Parameters.AddWithValue("@BANQUE", pBanque.BANQUE);
                cmd.Parameters.AddWithValue("@OriginalBANQUE", pBanque.OriginalBANQUE);
                cmd.Parameters.AddWithValue("@GUICHET", pBanque.GUICHET);
                cmd.Parameters.AddWithValue("@OriginalGUICHET", pBanque.OriginalGUICHET);
                cmd.Parameters.AddWithValue("@LIBELLE", pBanque.LIBELLE);
                cmd.Parameters.AddWithValue("@TRANS", pBanque.TRANS);
                cmd.Parameters.AddWithValue("@BANQUEMERE", pBanque.BANQUEMERE);
                cmd.Parameters.AddWithValue("@COMPTE", pBanque.COMPTE);
                cmd.Parameters.AddWithValue("@DATECREATION", pBanque.DATECREATION);
                cmd.Parameters.AddWithValue("@DATEMODIFICATION", pBanque.DATEMODIFICATION);
                cmd.Parameters.AddWithValue("@USERCREATION", pBanque.USERCREATION);
                cmd.Parameters.AddWithValue("@USERMODIFICATION", pBanque.USERMODIFICATION);
                if (cn.State == ConnectionState.Closed)
                    cn.Open();
                StartTransaction(cn);
                SetDBNullParametre(cmd.Parameters);
                int rowsAffected = cmd.ExecuteNonQuery();
                CommitTransaction(cmd.Transaction);
                return Convert.ToBoolean(rowsAffected);
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

        public bool Update(List<aBanque> pBanqueCollection)
        {
            int number = 0;
            foreach (aBanque entity in pBanqueCollection)
            {
                if (Update(entity))
                {
                    number++;
                }
            }
            return number != 0;
        }

        private bool Insert(aBanque pBanque)
        {
            cn = new SqlConnection(ConnectionString);
            cmd = new SqlCommand
            {
                Connection = cn,
                CommandType = CommandType.StoredProcedure,
                CommandText = EnumProcedureStockee.InsertBANQUE
            };
            cmd.Parameters.Clear();

            try
            {
                cmd.Parameters.AddWithValue("@BANQUE", pBanque.BANQUE);
                cmd.Parameters.AddWithValue("@GUICHET", pBanque.GUICHET);
                cmd.Parameters.AddWithValue("@LIBELLE", pBanque.LIBELLE);
                cmd.Parameters.AddWithValue("@TRANS", pBanque.TRANS);
                cmd.Parameters.AddWithValue("@BANQUEMERE", pBanque.BANQUEMERE);
                cmd.Parameters.AddWithValue("@COMPTE", pBanque.COMPTE);
                cmd.Parameters.AddWithValue("@DATECREATION", pBanque.DATECREATION);
                cmd.Parameters.AddWithValue("@DATEMODIFICATION", pBanque.DATEMODIFICATION);
                cmd.Parameters.AddWithValue("@USERCREATION", pBanque.USERCREATION);
                cmd.Parameters.AddWithValue("@USERMODIFICATION", pBanque.USERMODIFICATION);

                if (cn.State == ConnectionState.Closed)
                    cn.Open();
                StartTransaction(cn);

                SetDBNullParametre(cmd.Parameters);
                int rowsAffected = cmd.ExecuteNonQuery();
                CommitTransaction(cmd.Transaction);
                return Convert.ToBoolean(rowsAffected);
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

        public bool Insert(List<aBanque> pBanqueCollection)
        {
            int number = 0;
            foreach (aBanque entity in pBanqueCollection)
            {
                if (Insert(entity))
                {
                    number++;
                }
            }
            return number != 0;
        }

        public static List<aBanque> Fill(IDataReader reader, List<aBanque> rows, int start, int pageLength)
        {
            // advance to the starting row
            for (int i = 0; i < start; i++)
            {
                if (!reader.Read())
                    return rows; // not enough rows, just return
            }

            for (int i = 0; i < pageLength; i++)
            {
                if (!reader.Read())
                    break; // we are done

                aBanque c = new aBanque();
                c.BANQUE = (Convert.IsDBNull(reader["BANQUE"])) ? string.Empty : (System.String)reader["BANQUE"];
                c.OriginalBANQUE = (Convert.IsDBNull(reader["BANQUE"])) ? string.Empty : (System.String)reader["BANQUE"];
                c.GUICHET = (Convert.IsDBNull(reader["GUICHET"])) ? string.Empty : (System.String)reader["GUICHET"];
                c.OriginalGUICHET = (Convert.IsDBNull(reader["GUICHET"])) ? string.Empty : (System.String)reader["GUICHET"];
                c.LIBELLE = (Convert.IsDBNull(reader["LIBELLE"])) ? string.Empty : (System.String)reader["LIBELLE"];
                c.TRANS = (Convert.IsDBNull(reader["TRANS"])) ? null : (System.String)reader["TRANS"];
                c.BANQUEMERE = (Convert.IsDBNull(reader["BANQUEMERE"])) ? null : (System.String)reader["BANQUEMERE"];
                c.COMPTE = (Convert.IsDBNull(reader["COMPTE"])) ? null : (System.String)reader["COMPTE"];
                c.DATECREATION = (Convert.IsDBNull(reader["DATECREATION"])) ? DateTime.MinValue : (System.DateTime)reader["DATECREATION"];
                if (Convert.IsDBNull(reader["DATEMODIFICATION"]))
                    c.DATEMODIFICATION = null;
                else
                    c.DATEMODIFICATION = (System.DateTime)reader["DATEMODIFICATION"];
                c.USERCREATION = (Convert.IsDBNull(reader["USERCREATION"])) ? string.Empty : (System.String)reader["USERCREATION"];
                c.USERMODIFICATION = (Convert.IsDBNull(reader["USERMODIFICATION"])) ? null : (System.String)reader["USERMODIFICATION"];
                rows.Add(c);
            }
            return rows;
        }

        /// <summary>
        /// SelectAll_BANQUE
        /// </summary>
        /// <returns></returns>
        public DataSet SelectAll_BANQUE()
        {
            cn = new SqlConnection(ConnectionString);
            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = EnumProcedureStockee.SelectBANQUE2;
            
            try
            {
                StartTransaction(cn);

                if (cn.State == ConnectionState.Closed)
                    cn.Open();

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
                throw new Exception(EnumProcedureStockee.SelectBANQUE2 + ":" + ex.Message);
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                    cn.Close(); // Fermeture de la connection 
                    cmd.Dispose();
              
            }
        }
        /// <summary>
        /// Delete_BANQUE
        /// </summary>
        /// <param name="Centre"></param>
        /// <param name="Banque"></param>
        /// <param name="Guichet"></param>
        public void Delete_BANQUE(string Centre, string Banque, string Guichet)
        {
            cn = new SqlConnection(ConnectionString);
            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText =EnumProcedureStockee.DeleteBANQUE2;
            cmd.Parameters.Clear();

            cmd.Parameters.Add("@CENTRE", SqlDbType.VarChar).Value = Centre;
            cmd.Parameters.Add("@BANQUE", SqlDbType.VarChar).Value = Banque;
            cmd.Parameters.Add("@GUICHET", SqlDbType.VarChar).Value = Guichet;

           
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
                throw new Exception(EnumProcedureStockee.DeleteBANQUE2 + ":" + ex.Message);
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                cn.Close();
                cmd.Dispose();
            }
        }
        /// <summary>
        /// MiseAJour_BANQUE
        /// </summary>
        /// <param name="row"></param>
        public void MiseAJour_BANQUE(List<aBanque> rows)
        {
            cn = new SqlConnection(ConnectionString);
            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = EnumProcedureStockee.UpdateBANQUE2.Trim();

            
            cmd.Parameters.Clear();

            try
            {
                foreach (aBanque row in rows)
                   {
                    cmd.Parameters.Clear();

                    cmd.Parameters.Add("@CENTRE", SqlDbType.VarChar).Value = row.CENTRE;
                    cmd.Parameters.Add("@BANQUE", SqlDbType.VarChar).Value = row.BANQUE;
                    cmd.Parameters.Add("@GUICHET", SqlDbType.VarChar).Value = row.GUICHET;
                    cmd.Parameters.Add("@LIBELLE", SqlDbType.VarChar).Value = row.LIBELLE;
                    cmd.Parameters.Add("@DMAJ", SqlDbType.DateTime).Value = row.DMAJB;
                    cmd.Parameters.Add("@TRANS", SqlDbType.VarChar).Value = row.TRANS;
                    cmd.Parameters.Add("@BANQUEMERE", SqlDbType.VarChar).Value = row.BANQUEMERE;
                    cmd.Parameters.Add("@COMPTE", SqlDbType.VarChar).Value = row.COMPTE;
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
                throw new Exception(EnumProcedureStockee.UpdateBANQUE2 + ":" + ex.Message);
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                cn.Close(); // Fermeture de la connection 
                cmd.Dispose();
            }
        }
        /// <summary>
        /// Insertion_BANQUE
        /// </summary>
        /// <param name="row"></param>
        public void Insertion_BANQUE(List<aBanque> rows)
        {
            cn = new SqlConnection(ConnectionString);
            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = EnumProcedureStockee.InsertBANQUE2.Trim();

           
            try
            {
                foreach (aBanque row in rows)
                   {
                    cmd.Parameters.Clear();

                    cmd.Parameters.Add("@CENTRE", SqlDbType.VarChar).Value = row.CENTRE;
                    cmd.Parameters.Add("@BANQUE", SqlDbType.VarChar).Value = row.BANQUE;
                    cmd.Parameters.Add("@GUICHET", SqlDbType.VarChar).Value = row.GUICHET;
                    cmd.Parameters.Add("@LIBELLE", SqlDbType.VarChar).Value = row.LIBELLE;
                    cmd.Parameters.Add("@DMAJ", SqlDbType.DateTime).Value = row.DMAJB;
                    cmd.Parameters.Add("@TRANS", SqlDbType.VarChar).Value = row.TRANS;
                    cmd.Parameters.Add("@BANQUEMERE", SqlDbType.VarChar).Value = row.BANQUEMERE;
                    cmd.Parameters.Add("@COMPTE", SqlDbType.VarChar).Value = row.COMPTE;

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
                throw new Exception(EnumProcedureStockee.InsertBANQUE2 + ":" + ex.Message);
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                cn.Close(); // Fermeture de la connection 
                cmd.Dispose();
            }
        }
        /// <summary>
        /// Testunicite_BANQUE
        /// </summary>
        /// <param name="Centre"></param>
        /// <param name="Banque"></param>
        /// <param name="Guichet"></param>
        /// <returns></returns>
        public bool Testunicite_BANQUE(string Centre, string Banque, string Guichet)
        {
            bool Result = false;
            try
            {
                cn = new SqlConnection(ConnectionString);
                cmd = new SqlCommand();
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = EnumProcedureStockee.SelectBANQUE2ByKey.Trim();
                cmd.Parameters.Clear();

                cmd.Parameters.Add("@CENTRE", SqlDbType.VarChar).Value = Centre;
                cmd.Parameters.Add("@BANQUE", SqlDbType.VarChar).Value = Banque;
                cmd.Parameters.Add("@GUICHET", SqlDbType.VarChar).Value = Guichet;

                

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

        public List<aBanque> SelectAllBanque()
        {
            try
            {
                return Entities.GetEntityListFromQuery<aBanque>(CommonProcedures.RetourneTousBanques());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Delete(aBanque pBanque)
        {
            try
            {
                return Entities.DeleteEntity<Galatee.Entity.Model.BANQUE>(Entities.ConvertObject<Galatee.Entity.Model.BANQUE, aBanque>(pBanque));
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public bool Delete(List<aBanque> pBanqueCollection)
        {
            try
            {
                return Entities.DeleteEntity<Galatee.Entity.Model.BANQUE>(Entities.ConvertObject<Galatee.Entity.Model.BANQUE, aBanque>(pBanqueCollection));
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private bool Update(aBanque pBanque)
        {
            try
            {
                return Entities.UpdateEntity<Galatee.Entity.Model.BANQUE>(Entities.ConvertObject<Galatee.Entity.Model.BANQUE, aBanque>(pBanque));
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public bool Update(List<aBanque> pBanqueCollection)
        {
            try
            {
                return Entities.UpdateEntity<Galatee.Entity.Model.BANQUE>(Entities.ConvertObject<Galatee.Entity.Model.BANQUE, aBanque>(pBanqueCollection));
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private bool Insert(aBanque pBanque)
        {
            try
            {
                return Entities.InsertEntity<Galatee.Entity.Model.BANQUE>(Entities.ConvertObject<Galatee.Entity.Model.BANQUE, aBanque>(pBanque));
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public bool Insert(List<aBanque> pBanqueCollection)
        {
            try
            {
                return Entities.InsertEntity<Galatee.Entity.Model.BANQUE>(Entities.ConvertObject<Galatee.Entity.Model.BANQUE, aBanque>(pBanqueCollection));
            }
            catch (Exception e)
            {
                throw e;
            }
        }

    }
}
