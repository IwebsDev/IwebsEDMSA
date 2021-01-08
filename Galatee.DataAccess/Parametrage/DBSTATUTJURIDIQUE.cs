using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Galatee.Structure;
using Galatee.Entity.Model;

namespace Galatee.DataAccess
{
    public class DBSTATUTJURIDIQUE : Galatee.DataAccess.Parametrage.DbBase
    {
        /*
        private string ConnectionString;

        public DB_StatutJuridique()
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

        public DB_StatutJuridique(string ConnStr)
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

        public List<CsStatutJuridique> SelectAllStatutJuridique()
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
                              CommandText = EnumProcedureStockee.SelectStatutJuridique
                          };
                IDataReader reader = cmd.ExecuteReader();
                var rows = new List<CsStatutJuridique>();
                Fill(reader, rows, int.MinValue, int.MaxValue);
                reader.Close();
                return rows;
            }
            catch (Exception ex)
            {
                throw new Exception(EnumProcedureStockee.SelectStatutJuridique + ":" + ex.Message);
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                    cn.Close(); // Fermeture de la connection 
                cmd.Dispose();
            }
        }

        public bool Delete(CsStatutJuridique pStatutJuridique)
        {
            try
            {
                cn = new SqlConnection(ConnectionString);
                cmd = new SqlCommand
                {
                    Connection = cn,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = EnumProcedureStockee.DeleteStatutJuridique
                };
                cmd.Parameters.Clear();
                //cmd.Parameters.AddWithValue("@ID", pStatutJuridique.ID);
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
                throw new Exception(EnumProcedureStockee.DeleteStatutJuridique + ":" + ex.Message);
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                cn.Close();
                cmd.Dispose();
            }
        }

        public bool Delete(List<CsStatutJuridique> pStatutJuridiqueCollection)
        {
            int number = 0;
            foreach (CsStatutJuridique entity in pStatutJuridiqueCollection)
            {
                if (Delete(entity))
                {
                    number++;
                }
            }
            return number != 0;
        }

        private static List<CsStatutJuridique> Fill(IDataReader reader, List<CsStatutJuridique> rows, int start, int pageLength)
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

                var c = new CsStatutJuridique
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

        private bool Update(CsStatutJuridique pStatutJuridique)
        {
                cn = new SqlConnection(ConnectionString);
                cmd = new SqlCommand
                          {
                              Connection = cn,
                              CommandType = CommandType.StoredProcedure,
                              CommandText = EnumProcedureStockee.UpdateStatutJuridique
                          };
                cmd.Parameters.Clear();

                try
                {
                    //cmd.Parameters.AddWithValue("@ID", pStatutJuridique.ID);
                    //cmd.Parameters.AddWithValue("@CODE", pStatutJuridique.CODE);
                    cmd.Parameters.AddWithValue("@LIBELLE", pStatutJuridique.LIBELLE);
                    cmd.Parameters.AddWithValue("@DATECREATION", pStatutJuridique.DATECREATION);
                    cmd.Parameters.AddWithValue("@DATEMODIFICATION", pStatutJuridique.DATEMODIFICATION);
                    cmd.Parameters.AddWithValue("@USERCREATION", pStatutJuridique.USERCREATION);
                    cmd.Parameters.AddWithValue("@USERMODIFICATION", pStatutJuridique.USERMODIFICATION);
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

        public bool Update(List<CsStatutJuridique> pStatutJuridiqueCollection)
        {
            int number = 0;
            foreach (CsStatutJuridique entity in pStatutJuridiqueCollection)
            {
                if (Update(entity))
                {
                    number++;
                }
            }
            return number != 0;
        }

        private bool Insert(CsStatutJuridique pStatutJuridique)
        {
                cn = new SqlConnection(ConnectionString);
                cmd = new SqlCommand
                {
                    Connection = cn,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = EnumProcedureStockee.InsertStatutJuridique
                };
                cmd.Parameters.Clear();

                try
                {
                    //cmd.Parameters.AddWithValue("@CODE", pStatutJuridique.CODE);
                    cmd.Parameters.AddWithValue("@LIBELLE", pStatutJuridique.LIBELLE);
                    cmd.Parameters.AddWithValue("@DATECREATION", pStatutJuridique.DATECREATION);
                    cmd.Parameters.AddWithValue("@DATEMODIFICATION", pStatutJuridique.DATEMODIFICATION);
                    cmd.Parameters.AddWithValue("@USERCREATION", pStatutJuridique.USERCREATION);
                    cmd.Parameters.AddWithValue("@USERMODIFICATION", pStatutJuridique.USERMODIFICATION);
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

        public bool Insert(List<CsStatutJuridique> pStatutJuridiqueCollection)
        {
            int number = 0;
            foreach (CsStatutJuridique entity in pStatutJuridiqueCollection)
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

        public List<CsStatutJuridique> SelectAllStatutJuridique()
        {
            try
            {
                //return Entities.GetEntityListFromQuery<CsStatutJuridique>(ParamProcedure.RetourneStatutJuridique(null));
                DataTable dt = DB_ParametresGeneraux.SelectAllDonneReference("STATUTJURIQUE");
                return Entities.GetEntityListFromQuery<CsStatutJuridique>(dt);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Delete(CsStatutJuridique pStatutJuridique)
        {
            try
            {
                return Entities.DeleteEntity<Galatee.Entity.Model.STATUTJURIQUE>(Entities.ConvertObject<Galatee.Entity.Model.STATUTJURIQUE, CsStatutJuridique>(pStatutJuridique));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Delete(List<CsStatutJuridique> pStatutJuridiqueCollection)
        {
            try
            {
                return Entities.DeleteEntity<Galatee.Entity.Model.STATUTJURIQUE>(Entities.ConvertObject<Galatee.Entity.Model.STATUTJURIQUE, CsStatutJuridique>(pStatutJuridiqueCollection));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool Update(CsStatutJuridique pStatutJuridique)
        {
            try
            {
                return Entities.UpdateEntity<Galatee.Entity.Model.STATUTJURIQUE>(Entities.ConvertObject<Galatee.Entity.Model.STATUTJURIQUE, CsStatutJuridique>(pStatutJuridique));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Update(List<CsStatutJuridique> pStatutJuridiqueCollection)
        {
            try
            {
                return Entities.UpdateEntity<Galatee.Entity.Model.STATUTJURIQUE>(Entities.ConvertObject<Galatee.Entity.Model.STATUTJURIQUE, CsStatutJuridique>(pStatutJuridiqueCollection));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool Insert(CsStatutJuridique pStatutJuridique)
        {
            try
            {
                return Entities.InsertEntity<Galatee.Entity.Model.STATUTJURIQUE>(Entities.ConvertObject<Galatee.Entity.Model.STATUTJURIQUE, CsStatutJuridique>(pStatutJuridique));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Insert(List<CsStatutJuridique> pStatutJuridiqueCollection)
        {
            try
            {
                return Entities.InsertEntity<Galatee.Entity.Model.STATUTJURIQUE>(Entities.ConvertObject<Galatee.Entity.Model.STATUTJURIQUE, CsStatutJuridique>(pStatutJuridiqueCollection));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static CsStatutJuridique GetById(string id)
        {
            try
            {
                return Entities.GetEntityFromQuery<CsStatutJuridique>(ParamProcedure.RetourneStatutJuridique(id));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
