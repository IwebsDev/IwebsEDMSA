using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Galatee.Structure;
using Galatee.Entity.Model;

namespace Galatee.DataAccess
{
    public class DB_Denomination : Galatee.DataAccess.Parametrage.DbBase
    {
        /*
        private string ConnectionString;

        public DB_Denomination()
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

        public DB_Denomination(string ConnStr)
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

        public List<CsDenomination> SelectAllDenomination()
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
                              CommandText = EnumProcedureStockee.SelectDENOMINATION
                          };
                IDataReader reader = cmd.ExecuteReader();
                var rows = new List<CsDenomination>();
                Fill(reader, rows, int.MinValue, int.MaxValue);
                reader.Close();
                return rows;
            }
            catch (Exception ex)
            {
                throw new Exception(EnumProcedureStockee.SelectDENOMINATION + ":" + ex.Message);
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                    cn.Close(); // Fermeture de la connection 
                cmd.Dispose();
            }
        }

        public bool Delete(CsDenomination pDenomination)
        {
            try
            {
                cn = new SqlConnection(ConnectionString);
                cmd = new SqlCommand
                {
                    Connection = cn,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = EnumProcedureStockee.DeleteDENOMINATION
                };
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@ID", pDenomination.ID);
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
                throw new Exception(EnumProcedureStockee.DeleteDENOMINATION + ":" + ex.Message);
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                cn.Close();
                cmd.Dispose();
            }
        }

        public bool Delete(List<CsDenomination> pDenominationCollection)
        {
            int number = 0;
            foreach (CsDenomination entity in pDenominationCollection)
            {
                if (Delete(entity))
                {
                    number++;
                }
            }
            return number != 0;
        }

        private static List<CsDenomination> Fill(IDataReader reader, List<CsDenomination> rows, int start, int pageLength)
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

                var c = new CsDenomination
                                       {
                                           ID = (Convert.IsDBNull(reader["ID"])) ? (byte) 0 : (System.Byte) reader["ID"],
                                           CODE =
                                               (Convert.IsDBNull(reader["CODE"]))
                                                   ? string.Empty
                                                   : (System.String) reader["CODE"],
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

        private bool Update(CsDenomination pDenomination)
        {
                cn = new SqlConnection(ConnectionString);
                cmd = new SqlCommand
                          {
                              Connection = cn,
                              CommandType = CommandType.StoredProcedure,
                              CommandText = EnumProcedureStockee.UpdateDENOMINATION
                          };
                cmd.Parameters.Clear();

                try
                {
                    cmd.Parameters.AddWithValue("@ID", pDenomination.ID);
                    cmd.Parameters.AddWithValue("@CODE", pDenomination.CODE);
                    cmd.Parameters.AddWithValue("@LIBELLE", pDenomination.LIBELLE);
                    cmd.Parameters.AddWithValue("@DATECREATION", pDenomination.DATECREATION);
                    cmd.Parameters.AddWithValue("@DATEMODIFICATION", pDenomination.DATEMODIFICATION);
                    cmd.Parameters.AddWithValue("@USERCREATION", pDenomination.USERCREATION);
                    cmd.Parameters.AddWithValue("@USERMODIFICATION", pDenomination.USERMODIFICATION);
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

        public bool Update(List<CsDenomination> pDenominationCollection)
        {
            int number = 0;
            foreach (CsDenomination entity in pDenominationCollection)
            {
                if (Update(entity))
                {
                    number++;
                }
            }
            return number != 0;
        }

        private bool Insert(CsDenomination pDenomination)
        {
                cn = new SqlConnection(ConnectionString);
                cmd = new SqlCommand
                {
                    Connection = cn,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = EnumProcedureStockee.InsertDENOMINATION
                };
                cmd.Parameters.Clear();

                try
                {
                    cmd.Parameters.AddWithValue("@CODE", pDenomination.CODE);
                    cmd.Parameters.AddWithValue("@LIBELLE", pDenomination.LIBELLE);
                    cmd.Parameters.AddWithValue("@DATECREATION", pDenomination.DATECREATION);
                    cmd.Parameters.AddWithValue("@DATEMODIFICATION", pDenomination.DATEMODIFICATION);
                    cmd.Parameters.AddWithValue("@USERCREATION", pDenomination.USERCREATION);
                    cmd.Parameters.AddWithValue("@USERMODIFICATION", pDenomination.USERMODIFICATION);
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

        public bool Insert(List<CsDenomination> pDenominationCollection)
        {
            int number = 0;
            foreach (CsDenomination entity in pDenominationCollection)
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

        public List<CsDenomination> SelectAllDenomination()
        {
            try
            {
                return Entities.GetEntityListFromQuery<CsDenomination>(ParamProcedure.PARAM_DENOMINATION_RETOURNE());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Delete(CsDenomination pDenomination)
        {
            try
            {
                return Entities.DeleteEntity<Galatee.Entity.Model.DENOMINATION>(Entities.ConvertObject<Galatee.Entity.Model.DENOMINATION, CsDenomination>(pDenomination));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Delete(List<CsDenomination> pDenominationCollection)
        {
            try
            {
                return Entities.DeleteEntity<Galatee.Entity.Model.DENOMINATION>(Entities.ConvertObject<Galatee.Entity.Model.DENOMINATION, CsDenomination>(pDenominationCollection));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool Update(CsDenomination pDenomination)
        {
            try
            {
                return Entities.UpdateEntity<Galatee.Entity.Model.DENOMINATION>(Entities.ConvertObject<Galatee.Entity.Model.DENOMINATION, CsDenomination>(pDenomination));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Update(List<CsDenomination> pDenominationCollection)
        {
            try
            {
                return Entities.UpdateEntity<Galatee.Entity.Model.DENOMINATION>(Entities.ConvertObject<Galatee.Entity.Model.DENOMINATION, CsDenomination>(pDenominationCollection));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool Insert(CsDenomination pDenomination)
        {
            try
            {
                return Entities.InsertEntity<Galatee.Entity.Model.DENOMINATION>(Entities.ConvertObject<Galatee.Entity.Model.DENOMINATION, CsDenomination>(pDenomination));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Insert(List<CsDenomination> pDenominationCollection)
        {
            try
            {
                return Entities.InsertEntity<Galatee.Entity.Model.DENOMINATION>(Entities.ConvertObject<Galatee.Entity.Model.DENOMINATION, CsDenomination>(pDenominationCollection));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
