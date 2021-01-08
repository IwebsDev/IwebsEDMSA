using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Galatee.Structure;
using Galatee.Entity.Model;

namespace Galatee.DataAccess
{
    public class DBTYPECLIENT : Galatee.DataAccess.Parametrage.DbBase
    {
        /*
        private string ConnectionString;

        public DB_TypeClient()
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

        public DB_TypeClient(string ConnStr)
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

        public List<CsTypeClient> SelectAllTypeClient()
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
                              CommandText = EnumProcedureStockee.SelectTypeClient
                          };
                IDataReader reader = cmd.ExecuteReader();
                var rows = new List<CsTypeClient>();
                Fill(reader, rows, int.MinValue, int.MaxValue);
                reader.Close();
                return rows;
            }
            catch (Exception ex)
            {
                throw new Exception(EnumProcedureStockee.SelectTypeClient + ":" + ex.Message);
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                    cn.Close(); // Fermeture de la connection 
                cmd.Dispose();
            }
        }

        public bool Delete(CsTypeClient pTypeClient)
        {
            try
            {
                cn = new SqlConnection(ConnectionString);
                cmd = new SqlCommand
                {
                    Connection = cn,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = EnumProcedureStockee.DeleteTypeClient
                };
                cmd.Parameters.Clear();
                //cmd.Parameters.AddWithValue("@ID", pTypeClient.ID);
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
                throw new Exception(EnumProcedureStockee.DeleteTypeClient + ":" + ex.Message);
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                cn.Close();
                cmd.Dispose();
            }
        }

        public bool Delete(List<CsTypeClient> pTypeClientCollection)
        {
            int number = 0;
            foreach (CsTypeClient entity in pTypeClientCollection)
            {
                if (Delete(entity))
                {
                    number++;
                }
            }
            return number != 0;
        }

        private static List<CsTypeClient> Fill(IDataReader reader, List<CsTypeClient> rows, int start, int pageLength)
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

                var c = new CsTypeClient
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

        private bool Update(CsTypeClient pTypeClient)
        {
                cn = new SqlConnection(ConnectionString);
                cmd = new SqlCommand
                          {
                              Connection = cn,
                              CommandType = CommandType.StoredProcedure,
                              CommandText = EnumProcedureStockee.UpdateTypeClient
                          };
                cmd.Parameters.Clear();

                try
                {
                    //cmd.Parameters.AddWithValue("@ID", pTypeClient.ID);
                    //cmd.Parameters.AddWithValue("@CODE", pTypeClient.CODE);
                    cmd.Parameters.AddWithValue("@LIBELLE", pTypeClient.LIBELLE);
                    cmd.Parameters.AddWithValue("@DATECREATION", pTypeClient.DATECREATION);
                    cmd.Parameters.AddWithValue("@DATEMODIFICATION", pTypeClient.DATEMODIFICATION);
                    cmd.Parameters.AddWithValue("@USERCREATION", pTypeClient.USERCREATION);
                    cmd.Parameters.AddWithValue("@USERMODIFICATION", pTypeClient.USERMODIFICATION);
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

        public bool Update(List<CsTypeClient> pTypeClientCollection)
        {
            int number = 0;
            foreach (CsTypeClient entity in pTypeClientCollection)
            {
                if (Update(entity))
                {
                    number++;
                }
            }
            return number != 0;
        }

        private bool Insert(CsTypeClient pTypeClient)
        {
                cn = new SqlConnection(ConnectionString);
                cmd = new SqlCommand
                {
                    Connection = cn,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = EnumProcedureStockee.InsertTypeClient
                };
                cmd.Parameters.Clear();

                try
                {
                    //cmd.Parameters.AddWithValue("@CODE", pTypeClient.CODE);
                    cmd.Parameters.AddWithValue("@LIBELLE", pTypeClient.LIBELLE);
                    cmd.Parameters.AddWithValue("@DATECREATION", pTypeClient.DATECREATION);
                    cmd.Parameters.AddWithValue("@DATEMODIFICATION", pTypeClient.DATEMODIFICATION);
                    cmd.Parameters.AddWithValue("@USERCREATION", pTypeClient.USERCREATION);
                    cmd.Parameters.AddWithValue("@USERMODIFICATION", pTypeClient.USERMODIFICATION);
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

        public bool Insert(List<CsTypeClient> pTypeClientCollection)
        {
            int number = 0;
            foreach (CsTypeClient entity in pTypeClientCollection)
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


        public bool Delete(CsTypeClient pTypeClient)
        {
            try
            {
                return Entities.DeleteEntity<Galatee.Entity.Model.TYPECLIENT>(Entities.ConvertObject<Galatee.Entity.Model.TYPECLIENT, CsTypeClient>(pTypeClient));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Delete(List<CsTypeClient> pTypeClientCollection)
        {
            try
            {
                return Entities.DeleteEntity<Galatee.Entity.Model.TYPECLIENT>(Entities.ConvertObject<Galatee.Entity.Model.TYPECLIENT, CsTypeClient>(pTypeClientCollection));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool Update(CsTypeClient pTypeClient)
        {
            try
            {
                return Entities.UpdateEntity<Galatee.Entity.Model.TYPECLIENT>(Entities.ConvertObject<Galatee.Entity.Model.TYPECLIENT, CsTypeClient>(pTypeClient));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Update(List<CsTypeClient> pTypeClientCollection)
        {
            try
            {
                return Entities.UpdateEntity<Galatee.Entity.Model.TYPECLIENT>(Entities.ConvertObject<Galatee.Entity.Model.TYPECLIENT, CsTypeClient>(pTypeClientCollection));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool Insert(CsTypeClient pTypeClient)
        {
            try
            {
                return Entities.InsertEntity<Galatee.Entity.Model.TYPECLIENT>(Entities.ConvertObject<Galatee.Entity.Model.TYPECLIENT, CsTypeClient>(pTypeClient));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Insert(List<CsTypeClient> pTypeClientCollection)
        {
            try
            {
                return Entities.InsertEntity<Galatee.Entity.Model.TYPECLIENT>(Entities.ConvertObject<Galatee.Entity.Model.TYPECLIENT, CsTypeClient>(pTypeClientCollection));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static CsTypeClient GetById(string id)
        {
            try
            {
                return Entities.GetEntityFromQuery<CsTypeClient>(ParamProcedure.RetourneTypeClient(id));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    #region ADO .Net from Entity : Stephen 29-01-2019

        public List<CsTypeClient> SelectAllTypeClient()
        {
            try
            {
                //return Entities.GetEntityListFromQuery<CsTypeClient>(ParamProcedure.RetourneTypeClient(null));
                DataTable dt = DB_ParametresGeneraux.SelectAllDonneReference("TYPECLIENT");
                return Entities.GetEntityListFromQuery<CsTypeClient>(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    #endregion


    }
}
