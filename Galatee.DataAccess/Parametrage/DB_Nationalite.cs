using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Galatee.Structure;
using Galatee.Entity.Model;

namespace Galatee.DataAccess
{
    public class DB_Nationalite : Galatee.DataAccess.Parametrage.DbBase
    {
        /*
        private string ConnectionString;

        public DB_Nationalite()
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

        public DB_Nationalite(string ConnStr)
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

        public List<CsNationalite> SelectAllNationalite()
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
                              CommandText = EnumProcedureStockee.SelectNATIONALITE
                          };
                IDataReader reader = cmd.ExecuteReader();
                var rows = new List<CsNationalite>();
                Fill(reader, rows, int.MinValue, int.MaxValue);
                reader.Close();
                return rows;
            }
            catch (Exception ex)
            {
                throw new Exception(EnumProcedureStockee.SelectNATIONALITE + ":" + ex.Message);
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                    cn.Close(); // Fermeture de la connection 
                cmd.Dispose();
            }
        }

        public bool Delete(CsNationalite pNationalite)
        {
            try
            {
                cn = new SqlConnection(ConnectionString);
                cmd = new SqlCommand
                {
                    Connection = cn,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = EnumProcedureStockee.DeleteNATIONALITE
                };
                cmd.Parameters.Clear();
                //cmd.Parameters.AddWithValue("@ID", pNationalite.ID);
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
                throw new Exception(EnumProcedureStockee.DeleteNATIONALITE + ":" + ex.Message);
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                cn.Close();
                cmd.Dispose();
            }
        }

        public bool Delete(List<CsNationalite> pNationaliteCollection)
        {
            int number = 0;
            foreach (CsNationalite entity in pNationaliteCollection)
            {
                if (Delete(entity))
                {
                    number++;
                }
            }
            return number != 0;
        }

        private static List<CsNationalite> Fill(IDataReader reader, List<CsNationalite> rows, int start, int pageLength)
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

                var c = new CsNationalite
                                       {
                                           //ID = (Convert.IsDBNull(reader["ID"])) ? (byte) 0 : (System.Byte) reader["ID"],
                                           //CODE =
                                           //    (Convert.IsDBNull(reader["CODE"]))
                                           //        ? string.Empty
                                           //        : (System.String) reader["CODE"],
                                           LIBELLE =
                                               (Convert.IsDBNull(reader["LIBELLE"]))
                                                   ? null
                                                   : (System.String) reader["LIBELLE"],
                                           DATECREATION =
                                               (Convert.IsDBNull(reader["DATECREATION"]))
                                                   ? DateTime.MinValue
                                                   : (System.DateTime) reader["DATECREATION"]
                                       };
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

        private bool Update(CsNationalite pNationalite)
        {
                cn = new SqlConnection(ConnectionString);
                cmd = new SqlCommand
                          {
                              Connection = cn,
                              CommandType = CommandType.StoredProcedure,
                              CommandText = EnumProcedureStockee.UpdateNATIONALITE
                          };
                cmd.Parameters.Clear();

                try
                {
                    //cmd.Parameters.AddWithValue("@ID", pNationalite.ID);
                    //cmd.Parameters.AddWithValue("@CODE", pNationalite.CODE);
                    cmd.Parameters.AddWithValue("@LIBELLE", pNationalite.LIBELLE);
                    cmd.Parameters.AddWithValue("@DATECREATION", pNationalite.DATECREATION);
                    cmd.Parameters.AddWithValue("@DATEMODIFICATION", pNationalite.DATEMODIFICATION);
                    cmd.Parameters.AddWithValue("@USERCREATION", pNationalite.USERCREATION);
                    cmd.Parameters.AddWithValue("@USERMODIFICATION", pNationalite.USERMODIFICATION);
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

        public bool Update(List<CsNationalite> pNationaliteCollection)
        {
            int number = 0;
            foreach (CsNationalite entity in pNationaliteCollection)
            {
                if (Update(entity))
                {
                    number++;
                }
            }
            return number != 0;
        }

        private bool Insert(CsNationalite pNationalite)
        {
                cn = new SqlConnection(ConnectionString);
                cmd = new SqlCommand
                {
                    Connection = cn,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = EnumProcedureStockee.InsertNATIONALITE
                };
                cmd.Parameters.Clear();

                try
                {
                    //cmd.Parameters.AddWithValue("@CODE", pNationalite.CODE);
                    cmd.Parameters.AddWithValue("@LIBELLE", pNationalite.LIBELLE);
                    cmd.Parameters.AddWithValue("@DATECREATION", pNationalite.DATECREATION);
                    cmd.Parameters.AddWithValue("@DATEMODIFICATION", pNationalite.DATEMODIFICATION);
                    cmd.Parameters.AddWithValue("@USERCREATION", pNationalite.USERCREATION);
                    cmd.Parameters.AddWithValue("@USERMODIFICATION", pNationalite.USERMODIFICATION);
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

        public bool Insert(List<CsNationalite> pNationaliteCollection)
        {
            int number = 0;
            foreach (CsNationalite entity in pNationaliteCollection)
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

        public List<CsNationalite> SelectAllNationalite()
        {
            try
            {
                return Entities.GetEntityListFromQuery<CsNationalite>(ParamProcedure.RetourneNationnalite(null));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Delete(CsNationalite pNationalite)
        {
            try
            {
                return Entities.DeleteEntity<Galatee.Entity.Model.NATIONALITE>(Entities.ConvertObject<Galatee.Entity.Model.NATIONALITE, CsNationalite>(pNationalite));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Delete(List<CsNationalite> pNationaliteCollection)
        {
            try
            {
                return Entities.DeleteEntity<Galatee.Entity.Model.NATIONALITE>(Entities.ConvertObject<Galatee.Entity.Model.NATIONALITE, CsNationalite>(pNationaliteCollection));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool Update(CsNationalite pNationalite)
        {
            try
            {
                return Entities.UpdateEntity<Galatee.Entity.Model.NATIONALITE>(Entities.ConvertObject<Galatee.Entity.Model.NATIONALITE, CsNationalite>(pNationalite));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Update(List<CsNationalite> pNationaliteCollection)
        {
            try
            {
                return Entities.UpdateEntity<Galatee.Entity.Model.NATIONALITE>(Entities.ConvertObject<Galatee.Entity.Model.NATIONALITE, CsNationalite>(pNationaliteCollection));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool Insert(CsNationalite pNationalite)
        {
            try
            {
                return Entities.InsertEntity<Galatee.Entity.Model.NATIONALITE>(Entities.ConvertObject<Galatee.Entity.Model.NATIONALITE, CsNationalite>(pNationalite));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Insert(List<CsNationalite> pNationaliteCollection)
        {
            try
            {
                return Entities.InsertEntity<Galatee.Entity.Model.NATIONALITE>(Entities.ConvertObject<Galatee.Entity.Model.NATIONALITE, CsNationalite>(pNationaliteCollection));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
