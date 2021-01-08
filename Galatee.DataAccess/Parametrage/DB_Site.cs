using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Galatee.Structure;
using Galatee.Entity.Model;

namespace Galatee.DataAccess
{
    public class DB_Site : Galatee.DataAccess.Parametrage.DbBase
    {
        /*
        private string ConnectionString;

        public DB_Site()
        {
           try
            {
                ConnectionString = Session.GetSqlConnexionString();
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }

        public DB_Site(string ConnStr)
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

        public List<CsSite> SelectAllSite()
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
                              CommandText = EnumProcedureStockee.SelectSITE
                          };
                IDataReader reader = cmd.ExecuteReader();
                var rows = new List<CsSite>();
                Fill(reader, rows, int.MinValue, int.MaxValue);
                reader.Close();
                return rows;
            }
            catch (Exception ex)
            {
                throw new Exception(EnumProcedureStockee.SelectSITE + ":" + ex.Message);
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                    cn.Close(); // Fermeture de la connection 
                cmd.Dispose();
            }
        }

        public bool Delete(CsSite pSite)
        {
            try
            {
                cn = new SqlConnection(ConnectionString);
                cmd = new SqlCommand
                {
                    Connection = cn,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = EnumProcedureStockee.DeleteSITE
                };
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@CODESITE", pSite.PK_CODESITE );
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
                throw new Exception(EnumProcedureStockee.DeleteSITE + ":" + ex.Message);
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                cn.Close();
                cmd.Dispose();
            }
        }

        public bool Delete(List<CsSite> pSiteCollection)
        {
            int number = 0;
            foreach (CsSite entity in pSiteCollection)
            {
                if (Delete(entity))
                {
                    number++;
                }
            }
            return number != 0;
        }

        public static List<CsSite> Fill(IDataReader reader, List<CsSite> rows, int start, int pageLength)
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

                CsSite c = new CsSite();
                c.PK_CODESITE  = (Convert.IsDBNull(reader["CODESITE"])) ? string.Empty : (System.String)reader["CODESITE"];
                c.OriginalSITE = (Convert.IsDBNull(reader["CODESITE"])) ? string.Empty : (System.String)reader["CODESITE"];
                c.LIBELLE = (Convert.IsDBNull(reader["LIBELLE"])) ? string.Empty : (System.String)reader["LIBELLE"];
                c.SERVEUR = (Convert.IsDBNull(reader["SERVEUR"])) ? string.Empty : (System.String)reader["SERVEUR"];
                c.USERID = (Convert.IsDBNull(reader["USERID"])) ? string.Empty : (System.String)reader["USERID"];
                c.PWD = (Convert.IsDBNull(reader["PWD"])) ? string.Empty : (System.String)reader["PWD"];
                c.CATALOGUE = (Convert.IsDBNull(reader["CATALOGUE"])) ? string.Empty : (System.String)reader["CATALOGUE"];
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

        public bool Update(CsSite pSite)
        {
                cn = new SqlConnection(ConnectionString);
                cmd = new SqlCommand
                          {
                              Connection = cn,
                              CommandType = CommandType.StoredProcedure,
                              CommandText = EnumProcedureStockee.UpdateSITE
                          };
                cmd.Parameters.Clear();

                try
                {

                    cmd.Parameters.AddWithValue("@CODESITE", pSite.PK_CODESITE );
                    cmd.Parameters.AddWithValue("@OriginalCODESITE", pSite.OriginalSITE);
                    cmd.Parameters.AddWithValue("@LIBELLE", pSite.LIBELLE);
                    cmd.Parameters.AddWithValue("@SERVEUR", pSite.SERVEUR);
                    cmd.Parameters.AddWithValue("@USERID", pSite.USERID);
                    cmd.Parameters.AddWithValue("@PWD", pSite.PWD);
                    cmd.Parameters.AddWithValue("@CATALOGUE", pSite.CATALOGUE);
                    cmd.Parameters.AddWithValue("@DATECREATION", pSite.DATECREATION);
                    cmd.Parameters.AddWithValue("@DATEMODIFICATION", pSite.DATEMODIFICATION);
                    cmd.Parameters.AddWithValue("@USERCREATION", pSite.USERCREATION);
                    cmd.Parameters.AddWithValue("@USERMODIFICATION", pSite.USERMODIFICATION);
                  
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

        public bool Update(List<CsSite> pCsSiteCollection)
        {
            int number = 0;
            foreach (CsSite entity in pCsSiteCollection)
            {
                if (Update(entity))
                {
                    number++;
                }
            }
            return number != 0;
        }

        public bool Insert(CsSite pSite)
        {
                cn = new SqlConnection(ConnectionString);
                cmd = new SqlCommand
                {
                    Connection = cn,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = EnumProcedureStockee.InsertSITE
                };
                cmd.Parameters.Clear();

                try
                {
                    cmd.Parameters.AddWithValue("@CODESITE", pSite.PK_CODESITE );
                    cmd.Parameters.AddWithValue("@LIBELLE", pSite.LIBELLE);
                    cmd.Parameters.AddWithValue("@SERVEUR", pSite.SERVEUR);
                    cmd.Parameters.AddWithValue("@USERID", pSite.USERID);
                    cmd.Parameters.AddWithValue("@PWD", pSite.PWD);
                    cmd.Parameters.AddWithValue("@CATALOGUE", pSite.CATALOGUE);
                    cmd.Parameters.AddWithValue("@DATECREATION", pSite.DATECREATION);
                    cmd.Parameters.AddWithValue("@DATEMODIFICATION", pSite.DATEMODIFICATION);
                    cmd.Parameters.AddWithValue("@USERCREATION", pSite.USERCREATION);
                    cmd.Parameters.AddWithValue("@USERMODIFICATION", pSite.USERMODIFICATION);
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

        public bool Insert(List<CsSite> pSiteCollection)
        {
            int number = 0;
            foreach (CsSite entity in pSiteCollection)
            {
                if (Insert(entity))
                {
                    number++;
                }
            }
            return number != 0;
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

        */

        public List<CsSite> SelectAllSite()
        {
            try
            {
                return Entities.GetEntityListFromQuery<CsSite>(ParamProcedure.PARAM_SITE_RETOURNE());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Delete(CsSite pSite)
        {
            try
            {
                return Entities.DeleteEntity<Galatee.Entity.Model.SITE>(Entities.ConvertObject<Galatee.Entity.Model.SITE, CsSite>(pSite));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Delete(List<CsSite> pSiteCollection)
        {
            try
            {
                return Entities.DeleteEntity<Galatee.Entity.Model.SITE>(Entities.ConvertObject<Galatee.Entity.Model.SITE, CsSite>(pSiteCollection));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Update(CsSite pSite)
        {
            try
            {
                return Entities.UpdateEntity<Galatee.Entity.Model.SITE>(Entities.ConvertObject<Galatee.Entity.Model.SITE, CsSite>(pSite));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Update(List<CsSite> pCsSiteCollection)
        {
            try
            {
                return Entities.UpdateEntity<Galatee.Entity.Model.SITE>(Entities.ConvertObject<Galatee.Entity.Model.SITE, CsSite>(pCsSiteCollection));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Insert(CsSite pSite)
        {
            try
            {
                return Entities.InsertEntity<Galatee.Entity.Model.SITE>(Entities.ConvertObject<Galatee.Entity.Model.SITE, CsSite>(pSite));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Insert(List<CsSite> pSiteCollection)
        {
            try
            {
                return Entities.InsertEntity<Galatee.Entity.Model.SITE>(Entities.ConvertObject<Galatee.Entity.Model.SITE, CsSite>(pSiteCollection));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool TesterConnexion(CsSite pSite, out string pErreur)
        {
            pErreur = string.Empty;
            string connexion = "Data Source={0};Initial Catalog={1};User ID={2};Password={3};Persist Security Info=True";
            try
            {
                string myConnexion = string.Format(connexion, pSite.SERVEUR, pSite.CATALOGUE, pSite.USERID, pSite.PWD);
                var sqlConnexion = new SqlConnection(myConnexion);
                sqlConnexion.Open();
                return true;
            }
            catch (Exception ex)
            {
                pErreur = ex.Message;
                return false;
            }
        }
    }
}
