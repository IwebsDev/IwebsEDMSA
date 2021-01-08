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
    public class DB_REGCLI /*: Galatee.DataAccess.Parametrage.DbBase*/
    {
        /*
        /// <summary>
        /// DB_BANQUES2
        /// </summary>
        public DB_REGCLI()
        {
            ConnectionString = Session.GetSqlConnexionString();
        }
        /// <summary>
        /// DB_BANQUES2
        /// </summary>
        /// <param name="ConnStr"></param>
        public DB_REGCLI(string ConnStr)
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


        public List<CsRegCli> SelectAllRegCli()
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
                    CommandText = EnumProcedureStockee.SelectREGCLI
                };
                IDataReader reader = cmd.ExecuteReader();
                var rows = new List<CsRegCli>();
                Fill(reader, rows, int.MinValue, int.MaxValue);
                reader.Close();
                return rows;
            }
            catch (Exception ex)
            {
                throw new Exception(EnumProcedureStockee.SelectREGCLI + ":" + ex.Message);
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                    cn.Close(); // Fermeture de la connection 
                cmd.Dispose();
            }
        }

        public bool Delete(CsRegCli pRegCli)
        {
            try
            {
                cn = new SqlConnection(ConnectionString);
                cmd = new SqlCommand
                {
                    Connection = cn,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = EnumProcedureStockee.DeleteREGCLI
                };
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@REGCLI", pRegCli.REGCLI);
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
                throw new Exception(EnumProcedureStockee.DeleteREGCLI + ":" + ex.Message);
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                    cn.Close();
                cmd.Dispose();
            }
        }

        public bool Delete(List<CsRegCli> pRegCliCollection)
        {
            int number = 0;
            foreach (CsRegCli entity in pRegCliCollection)
            {
                if (Delete(entity))
                {
                    number++;
                }
            }
            return number != 0;
        }

        private bool Update(CsRegCli pRegCli)
        {
            cn = new SqlConnection(ConnectionString);
            cmd = new SqlCommand
            {
                Connection = cn,
                CommandType = CommandType.StoredProcedure,
                CommandText = EnumProcedureStockee.UpdateREGCLI
            };
            cmd.Parameters.Clear();

            try
            {

                cmd.Parameters.AddWithValue("@REGCLI", pRegCli.REGCLI);
                cmd.Parameters.AddWithValue("@OriginalREGCLI", pRegCli.OriginalREGCLI);
                cmd.Parameters.AddWithValue("@DENOM", pRegCli.DENOM);
                cmd.Parameters.AddWithValue("@NOM", pRegCli.NOM);
                cmd.Parameters.AddWithValue("@ADR1", pRegCli.ADR1);
                cmd.Parameters.AddWithValue("@ADR2", pRegCli.ADR2);
                cmd.Parameters.AddWithValue("@CODPOS", pRegCli.CODPOS);
                cmd.Parameters.AddWithValue("@BUREAU", pRegCli.BUREAU);
                cmd.Parameters.AddWithValue("@TRAITFAC", pRegCli.TRAITFAC);
                cmd.Parameters.AddWithValue("@TRANS", pRegCli.TRANS);
                cmd.Parameters.AddWithValue("@REFERENCEPUPITRE", pRegCli.REFERENCEPUPITRE);
                cmd.Parameters.AddWithValue("@DATECREATION", pRegCli.DATECREATION);
                cmd.Parameters.AddWithValue("@DATEMODIFICATION", pRegCli.DATEMODIFICATION);
                cmd.Parameters.AddWithValue("@USERCREATION",pRegCli.USERCREATION);
                cmd.Parameters.AddWithValue("@USERMODIFICATION",pRegCli.USERMODIFICATION);

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

        public bool Update(List<CsRegCli> pRegCliCollection)
        {
            int number = 0;
            foreach (CsRegCli entity in pRegCliCollection)
            {
                if (Update(entity))
                {
                    number++;
                }
            }
            return number != 0;
        }

        private bool Insert(CsRegCli pRegCli)
        {
            cn = new SqlConnection(ConnectionString);
            cmd = new SqlCommand
            {
                Connection = cn,
                CommandType = CommandType.StoredProcedure,
                CommandText = EnumProcedureStockee.InsertREGCLI
            };
            cmd.Parameters.Clear();

            try
            {
                cmd.Parameters.AddWithValue("@REGCLI", pRegCli.REGCLI);
                cmd.Parameters.AddWithValue("@DENOM", pRegCli.DENOM);
                cmd.Parameters.AddWithValue("@NOM", pRegCli.NOM);
                cmd.Parameters.AddWithValue("@ADR1", pRegCli.ADR1);
                cmd.Parameters.AddWithValue("@ADR2", pRegCli.ADR2);
                cmd.Parameters.AddWithValue("@CODPOS", pRegCli.CODPOS);
                cmd.Parameters.AddWithValue("@BUREAU", pRegCli.BUREAU);
                cmd.Parameters.AddWithValue("@TRAITFAC", pRegCli.TRAITFAC);
                cmd.Parameters.AddWithValue("@TRANS", pRegCli.TRANS);
                cmd.Parameters.AddWithValue("@REFERENCEPUPITRE", pRegCli.REFERENCEPUPITRE);
                cmd.Parameters.AddWithValue("@DATECREATION", pRegCli.DATECREATION);
                cmd.Parameters.AddWithValue("@DATEMODIFICATION", pRegCli.DATEMODIFICATION);
                cmd.Parameters.AddWithValue("@USERCREATION", pRegCli.USERCREATION);
                cmd.Parameters.AddWithValue("@USERMODIFICATION", pRegCli.USERMODIFICATION);

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

        public bool Insert(List<CsRegCli> pRegCliCollection)
        {
            int number = 0;
            foreach (CsRegCli entity in pRegCliCollection)
            {
                if (Insert(entity))
                {
                    number++;
                }
            }
            return number != 0;
        }

        public static List<CsRegCli> Fill(IDataReader reader, List<CsRegCli> rows, int start, int pageLength)
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

                var c = new CsRegCli();
                c.REGCLI = (Convert.IsDBNull(reader["REGCLI"])) ? string.Empty : (System.String)reader["REGCLI"];
                c.OriginalREGCLI = (Convert.IsDBNull(reader["REGCLI"])) ? string.Empty : (System.String)reader["REGCLI"];
                c.DENOM = (Convert.IsDBNull(reader["DENOM"])) ? (byte)0 : (System.Byte)reader["DENOM"];
                c.NOM = (Convert.IsDBNull(reader["NOM"])) ? string.Empty : (System.String)reader["NOM"];
                c.ADR1 = (Convert.IsDBNull(reader["ADR1"])) ? string.Empty : (System.String)reader["ADR1"];
                c.ADR2 = (Convert.IsDBNull(reader["ADR2"])) ? string.Empty : (System.String)reader["ADR2"];
                c.CODPOS = (Convert.IsDBNull(reader["CODPOS"])) ? string.Empty : (System.String)reader["CODPOS"];
                c.BUREAU = (Convert.IsDBNull(reader["BUREAU"])) ? string.Empty : (System.String)reader["BUREAU"];
                c.TRAITFAC = (Convert.IsDBNull(reader["TRAITFAC"])) ? null : (System.String)reader["TRAITFAC"];
                c.TRANS = (Convert.IsDBNull(reader["TRANS"])) ? null : (System.String)reader["TRANS"];
                c.REFERENCEPUPITRE = (Convert.IsDBNull(reader["REFERENCEPUPITRE"])) ? null : (short?)reader["REFERENCEPUPITRE"];
                c.DATECREATION = (Convert.IsDBNull(reader["DATECREATION"])) ? DateTime.MinValue : (System.DateTime)reader["DATECREATION"];
                if (Convert.IsDBNull(reader["DATEMODIFICATION"]))
                    c.DATEMODIFICATION = null;
                else
                    c.DATEMODIFICATION = (System.DateTime)reader["DATEMODIFICATION"];
                c.USERCREATION = (Convert.IsDBNull(reader["USERCREATION"])) ? string.Empty : (System.String)reader["USERCREATION"];
                c.USERMODIFICATION = (Convert.IsDBNull(reader["USERMODIFICATION"])) ? null : (System.String)reader["USERMODIFICATION"];
                c.LIBELLEDENOMINATION = (Convert.IsDBNull(reader["LIBELLEDENOMINATION"])) ? string.Empty : (System.String)reader["LIBELLEDENOMINATION"];
                rows.Add(c);
            }
            return rows;
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
        */

        public List<CsRegCli> SelectAllRegCli()
        {
            try
            {
                return Entities.GetEntityListFromQuery<CsRegCli>(ParamProcedure.PARAM_REGCLI_RETOURNE());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Delete(CsRegCli pRegCli)
        {
            try
            {
                return Entities.DeleteEntity<Galatee.Entity.Model.REGROUPEMENT>(Entities.ConvertObject<Galatee.Entity.Model.REGROUPEMENT, CsRegCli>(pRegCli));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Delete(List<CsRegCli> pRegCliCollection)
        {
            try
            {
                return Entities.DeleteEntity<Galatee.Entity.Model.REGROUPEMENT>(Entities.ConvertObject<Galatee.Entity.Model.REGROUPEMENT, CsRegCli>(pRegCliCollection));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool Update(CsRegCli pRegCli)
        {
            try
            {
                return Entities.UpdateEntity<Galatee.Entity.Model.REGROUPEMENT>(Entities.ConvertObject<Galatee.Entity.Model.REGROUPEMENT, CsRegCli>(pRegCli));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Update(List<CsRegCli> pRegCliCollection)
        {
            try
            {
                return Entities.UpdateEntity<Galatee.Entity.Model.REGROUPEMENT>(Entities.ConvertObject<Galatee.Entity.Model.REGROUPEMENT, CsRegCli>(pRegCliCollection));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool Insert(CsRegCli pRegCli)
        {
            try
            {
                return Entities.InsertEntity<Galatee.Entity.Model.REGROUPEMENT>(Entities.ConvertObject<Galatee.Entity.Model.REGROUPEMENT, CsRegCli>(pRegCli));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Insert(List<CsRegCli> pRegCliCollection)
        {
            try
            {
                return Entities.InsertEntity<Galatee.Entity.Model.REGROUPEMENT>(Entities.ConvertObject<Galatee.Entity.Model.REGROUPEMENT, CsRegCli>(pRegCliCollection));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
