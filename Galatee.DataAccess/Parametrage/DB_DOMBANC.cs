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
    public class DB_DOMBANC /*: Galatee.DataAccess.Parametrage.DbBase*/
    {
        /*
        /// <summary>
        /// DB_BANQUES2
        /// </summary>
        public DB_DOMBANC()
        {
            ConnectionString = Session.GetSqlConnexionString();
        }
        /// <summary>
        /// DB_BANQUES2
        /// </summary>
        /// <param name="ConnStr"></param>
        public DB_DOMBANC(string ConnStr)
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


        public List<CsDomBanc> SelectAllDomBanc()
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
                    CommandText = EnumProcedureStockee.SelectDOMBANC
                };
                IDataReader reader = cmd.ExecuteReader();
                var rows = new List<CsDomBanc>();
                Fill(reader, rows, int.MinValue, int.MaxValue);
                reader.Close();
                return rows;
            }
            catch (Exception ex)
            {
                throw new Exception(EnumProcedureStockee.SelectDOMBANC + ":" + ex.Message);
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                    cn.Close(); // Fermeture de la connection 
                cmd.Dispose();
            }
        }

        public bool Delete(CsDomBanc pDomBanc)
        {
            try
            {
                cn = new SqlConnection(ConnectionString);
                cmd = new SqlCommand
                {
                    Connection = cn,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = EnumProcedureStockee.DeleteDOMBANC
                };
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@BANQUE", pDomBanc.BANQUE);
                cmd.Parameters.AddWithValue("@GUICHET", pDomBanc.GUICHET);
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
                throw new Exception(EnumProcedureStockee.DeleteDOMBANC + ":" + ex.Message);
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                    cn.Close();
                cmd.Dispose();
            }
        }

        public bool Delete(List<CsDomBanc> pDomBancCollection)
        {
            int number = 0;
            foreach (CsDomBanc entity in pDomBancCollection)
            {
                if (Delete(entity))
                {
                    number++;
                }
            }
            return number != 0;
        }

        private bool Update(CsDomBanc pDomBanc)
        {
            cn = new SqlConnection(ConnectionString);
            cmd = new SqlCommand
            {
                Connection = cn,
                CommandType = CommandType.StoredProcedure,
                CommandText = EnumProcedureStockee.UpdateDOMBANC
            };
            cmd.Parameters.Clear();

            try
            {

                cmd.Parameters.AddWithValue("@BANQUE", pDomBanc.BANQUE);
                cmd.Parameters.AddWithValue("@OriginalBANQUE", pDomBanc.OriginalBANQUE);
                cmd.Parameters.AddWithValue("@GUICHET", pDomBanc.GUICHET);
                cmd.Parameters.AddWithValue("@OriginalGUICHET", pDomBanc.OriginalGUICHET);
                cmd.Parameters.AddWithValue("@COMPTE", pDomBanc.COMPTE);
                cmd.Parameters.AddWithValue("@TRANS", pDomBanc.TRANS);
                cmd.Parameters.AddWithValue("@COMPTA", pDomBanc.COMPTA);
                cmd.Parameters.AddWithValue("@LIBELLE", pDomBanc.LIBELLE);
                cmd.Parameters.AddWithValue("@DATECREATION", pDomBanc.DATECREATION);
                cmd.Parameters.AddWithValue("@DATEMODIFICATION", pDomBanc.DATEMODIFICATION);
                cmd.Parameters.AddWithValue("@USERCREATION", pDomBanc.USERCREATION);
                cmd.Parameters.AddWithValue("@USERMODIFICATION", pDomBanc.USERMODIFICATION);

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

        public bool Update(List<CsDomBanc> pDomBancCollection)
        {
            int number = 0;
            foreach (CsDomBanc entity in pDomBancCollection)
            {
                if (Update(entity))
                {
                    number++;
                }
            }
            return number != 0;
        }

        private bool Insert(CsDomBanc pDomBanc)
        {
            cn = new SqlConnection(ConnectionString);
            cmd = new SqlCommand
            {
                Connection = cn,
                CommandType = CommandType.StoredProcedure,
                CommandText = EnumProcedureStockee.InsertDOMBANC
            };
            cmd.Parameters.Clear();

            try
            {
                cmd.Parameters.AddWithValue("@BANQUE", pDomBanc.BANQUE);
                cmd.Parameters.AddWithValue("@GUICHET", pDomBanc.GUICHET);
                cmd.Parameters.AddWithValue("@COMPTE", pDomBanc.COMPTE);
                cmd.Parameters.AddWithValue("@TRANS", pDomBanc.TRANS);
                cmd.Parameters.AddWithValue("@COMPTA", pDomBanc.COMPTA);
                cmd.Parameters.AddWithValue("@LIBELLE", pDomBanc.LIBELLE);
                cmd.Parameters.AddWithValue("@DATECREATION", pDomBanc.DATECREATION);
                cmd.Parameters.AddWithValue("@DATEMODIFICATION", pDomBanc.DATEMODIFICATION);
                cmd.Parameters.AddWithValue("@USERCREATION", pDomBanc.USERCREATION);
                cmd.Parameters.AddWithValue("@USERMODIFICATION", pDomBanc.USERMODIFICATION);

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

        public bool Insert(List<CsDomBanc> pDomBancCollection)
        {
            int number = 0;
            foreach (CsDomBanc entity in pDomBancCollection)
            {
                if (Insert(entity))
                {
                    number++;
                }
            }
            return number != 0;
        }

        public static List<CsDomBanc> Fill(IDataReader reader, List<CsDomBanc> rows, int start, int pageLength)
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

                CsDomBanc c = new CsDomBanc();
                c.BANQUE = (Convert.IsDBNull(reader["BANQUE"])) ? string.Empty : (System.String)reader["BANQUE"];
                c.OriginalBANQUE = (Convert.IsDBNull(reader["BANQUE"])) ? string.Empty : (System.String)reader["BANQUE"];
                c.GUICHET = (Convert.IsDBNull(reader["GUICHET"])) ? string.Empty : (System.String)reader["GUICHET"];
                c.OriginalGUICHET = (Convert.IsDBNull(reader["GUICHET"])) ? string.Empty : (System.String)reader["GUICHET"];
                c.COMPTE = (Convert.IsDBNull(reader["COMPTE"])) ? null : (System.String)reader["COMPTE"];
                c.TRANS = (Convert.IsDBNull(reader["TRANS"])) ? null : (System.String)reader["TRANS"];
                c.COMPTA = (Convert.IsDBNull(reader["COMPTA"])) ? null : (System.String)reader["COMPTA"];
                c.LIBELLE = (Convert.IsDBNull(reader["LIBELLE"])) ? null : (System.String)reader["LIBELLE"];
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

        public List<CsDomBanc> SelectAllDomBanc()
        {
            try
            {
                return Entities.GetEntityListFromQuery<CsDomBanc>(ParamProcedure.PARAM_DOMBANC_RETOURNE());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Delete(CsDomBanc pDomBanc)
        {
            try
            {
                return Entities.DeleteEntity<Galatee.Entity.Model.DOMBANC>(Entities.ConvertObject<Galatee.Entity.Model.DOMBANC, CsDomBanc>(pDomBanc));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Delete(List<CsDomBanc> pDomBancCollection)
        {
            try
            {
                return Entities.DeleteEntity<Galatee.Entity.Model.DOMBANC>(Entities.ConvertObject<Galatee.Entity.Model.DOMBANC, CsDomBanc>(pDomBancCollection));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool Update(CsDomBanc pDomBanc)
        {
            try
            {
                return Entities.UpdateEntity<Galatee.Entity.Model.DOMBANC>(Entities.ConvertObject<Galatee.Entity.Model.DOMBANC, CsDomBanc>(pDomBanc));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Update(List<CsDomBanc> pDomBancCollection)
        {
            try
            {
                return Entities.UpdateEntity<Galatee.Entity.Model.DOMBANC>(Entities.ConvertObject<Galatee.Entity.Model.DOMBANC, CsDomBanc>(pDomBancCollection));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool Insert(CsDomBanc pDomBanc)
        {
            try
            {
                return Entities.InsertEntity<Galatee.Entity.Model.DOMBANC>(Entities.ConvertObject<Galatee.Entity.Model.DOMBANC, CsDomBanc>(pDomBanc));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Insert(List<CsDomBanc> pDomBancCollection)
        {
            try
            {
                return Entities.InsertEntity<Galatee.Entity.Model.DOMBANC>(Entities.ConvertObject<Galatee.Entity.Model.DOMBANC, CsDomBanc>(pDomBancCollection));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
