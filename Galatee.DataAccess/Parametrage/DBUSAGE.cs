using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Galatee.Structure;
using Galatee.Entity.Model;

namespace Galatee.DataAccess
{
    public class DBUSAGE : Galatee.DataAccess.Parametrage.DbBase
    {
        /*
        private string ConnectionString;

        public DB_Usage()
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

        public DB_Usage(string ConnStr)
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

        public List<CsUsage> SelectAllUsage()
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
                              CommandText = EnumProcedureStockee.SelectUsage
                          };
                IDataReader reader = cmd.ExecuteReader();
                var rows = new List<CsUsage>();
                Fill(reader, rows, int.MinValue, int.MaxValue);
                reader.Close();
                return rows;
            }
            catch (Exception ex)
            {
                throw new Exception(EnumProcedureStockee.SelectUsage + ":" + ex.Message);
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                    cn.Close(); // Fermeture de la connection 
                cmd.Dispose();
            }
        }

        public bool Delete(CsUsage pUsage)
        {
            try
            {
                cn = new SqlConnection(ConnectionString);
                cmd = new SqlCommand
                {
                    Connection = cn,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = EnumProcedureStockee.DeleteUsage
                };
                cmd.Parameters.Clear();
                //cmd.Parameters.AddWithValue("@ID", pUsage.ID);
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
                throw new Exception(EnumProcedureStockee.DeleteUsage + ":" + ex.Message);
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                cn.Close();
                cmd.Dispose();
            }
        }

        public bool Delete(List<CsUsage> pUsageCollection)
        {
            int number = 0;
            foreach (CsUsage entity in pUsageCollection)
            {
                if (Delete(entity))
                {
                    number++;
                }
            }
            return number != 0;
        }

        private static List<CsUsage> Fill(IDataReader reader, List<CsUsage> rows, int start, int pageLength)
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

                var c = new CsUsage
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

        private bool Update(CsUsage pUsage)
        {
                cn = new SqlConnection(ConnectionString);
                cmd = new SqlCommand
                          {
                              Connection = cn,
                              CommandType = CommandType.StoredProcedure,
                              CommandText = EnumProcedureStockee.UpdateUsage
                          };
                cmd.Parameters.Clear();

                try
                {
                    //cmd.Parameters.AddWithValue("@ID", pUsage.ID);
                    //cmd.Parameters.AddWithValue("@CODE", pUsage.CODE);
                    cmd.Parameters.AddWithValue("@LIBELLE", pUsage.LIBELLE);
                    cmd.Parameters.AddWithValue("@DATECREATION", pUsage.DATECREATION);
                    cmd.Parameters.AddWithValue("@DATEMODIFICATION", pUsage.DATEMODIFICATION);
                    cmd.Parameters.AddWithValue("@USERCREATION", pUsage.USERCREATION);
                    cmd.Parameters.AddWithValue("@USERMODIFICATION", pUsage.USERMODIFICATION);
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

        public bool Update(List<CsUsage> pUsageCollection)
        {
            int number = 0;
            foreach (CsUsage entity in pUsageCollection)
            {
                if (Update(entity))
                {
                    number++;
                }
            }
            return number != 0;
        }

        private bool Insert(CsUsage pUsage)
        {
                cn = new SqlConnection(ConnectionString);
                cmd = new SqlCommand
                {
                    Connection = cn,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = EnumProcedureStockee.InsertUsage
                };
                cmd.Parameters.Clear();

                try
                {
                    //cmd.Parameters.AddWithValue("@CODE", pUsage.CODE);
                    cmd.Parameters.AddWithValue("@LIBELLE", pUsage.LIBELLE);
                    cmd.Parameters.AddWithValue("@DATECREATION", pUsage.DATECREATION);
                    cmd.Parameters.AddWithValue("@DATEMODIFICATION", pUsage.DATEMODIFICATION);
                    cmd.Parameters.AddWithValue("@USERCREATION", pUsage.USERCREATION);
                    cmd.Parameters.AddWithValue("@USERMODIFICATION", pUsage.USERMODIFICATION);
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

        public bool Insert(List<CsUsage> pUsageCollection)
        {
            int number = 0;
            foreach (CsUsage entity in pUsageCollection)
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

        public List<CsUsage> SelectAllUsage()
        {
            try
            {
                //return Entities.GetEntityListFromQuery<CsUsage>(ParamProcedure.RetourneUsage(null));
                DataTable dt = DB_ParametresGeneraux.SelectAllDonneReference("USAGE");
                return Entities.GetEntityListFromQuery<CsUsage>(dt);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Delete(CsUsage pUsage)
        {
            try
            {
                return Entities.DeleteEntity<Galatee.Entity.Model.USAGE>(Entities.ConvertObject<Galatee.Entity.Model.USAGE, CsUsage>(pUsage));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Delete(List<CsUsage> pUsageCollection)
        {
            try
            {
                return Entities.DeleteEntity<Galatee.Entity.Model.USAGE>(Entities.ConvertObject<Galatee.Entity.Model.USAGE, CsUsage>(pUsageCollection));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool Update(CsUsage pUsage)
        {
            try
            {
                return Entities.UpdateEntity<Galatee.Entity.Model.USAGE>(Entities.ConvertObject<Galatee.Entity.Model.USAGE, CsUsage>(pUsage));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Update(List<CsUsage> pUsageCollection)
        {
            try
            {
                return Entities.UpdateEntity<Galatee.Entity.Model.USAGE>(Entities.ConvertObject<Galatee.Entity.Model.USAGE, CsUsage>(pUsageCollection));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool Insert(CsUsage pUsage)
        {
            try
            {
                return Entities.InsertEntity<Galatee.Entity.Model.USAGE>(Entities.ConvertObject<Galatee.Entity.Model.USAGE, CsUsage>(pUsage));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Insert(List<CsUsage> pUsageCollection)
        {
            try
            {
                return Entities.InsertEntity<Galatee.Entity.Model.USAGE>(Entities.ConvertObject<Galatee.Entity.Model.USAGE, CsUsage>(pUsageCollection));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static CsUsage GetById(string id)
        {
            try
            {
                return Entities.GetEntityFromQuery<CsUsage>(ParamProcedure.RetourneUsage(id));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
