using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;

using Galatee.Structure;
using Galatee.Entity.Model;

namespace Galatee.DataAccess
{
    public class DB_RUES /*: Galatee.DataAccess.Parametrage.DbBase*/
    {
        /*
        public DB_RUES()
        {
            ConnectionString = Session.GetSqlConnexionString();
        }

        private string ConnectionString;
        private SqlConnection cn = null;

        private bool _Transaction;

        public bool Transaction
        {
            get { return _Transaction; }
            set { _Transaction = value; }

        }

        private SqlCommand cmd = null;

        #region Méthodes de mise à jour de la table RUES


        public static List<CsRues> Fill(IDataReader reader, List<CsRues> rows, int start, int pageLength)
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

                CsRues c = new CsRues();
                c.CENTRE = (Convert.IsDBNull(reader["CENTRE"])) ? string.Empty : (System.String)reader["CENTRE"];
                c.OriginalCENTRE = (Convert.IsDBNull(reader["CENTRE"])) ? string.Empty : (System.String)reader["CENTRE"];
                c.COMMUNE = (Convert.IsDBNull(reader["COMMUNE"])) ? string.Empty : (System.String)reader["COMMUNE"];
                c.OriginalCOMMUNE = (Convert.IsDBNull(reader["COMMUNE"])) ? string.Empty : (System.String)reader["COMMUNE"];
                c.RUE = (Convert.IsDBNull(reader["RUE"])) ? string.Empty : (System.String)reader["RUE"];
                c.OriginalRUE = (Convert.IsDBNull(reader["RUE"])) ? string.Empty : (System.String)reader["RUE"];
                c.NRUE = (Convert.IsDBNull(reader["NRUE"])) ? null : (System.String)reader["NRUE"];
                c.TRANS = (Convert.IsDBNull(reader["TRANS"])) ? null : (System.String)reader["TRANS"];
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

        public  List<CsRues> SelectAllRues()
        {
            cn = new SqlConnection(ConnectionString);
            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SPX_PARAM_RUES_RETOURNE";

            try
            {
                if (cn.State == ConnectionState.Closed)
                    cn.Open();

                StartTransaction(cn);

                IDataReader reader = cmd.ExecuteReader();
                var rows = new List<CsRues>();
                Fill(reader, rows, int.MinValue, int.MaxValue);
                reader.Close();
                return rows;
            }
            catch (Exception ex)
            {
                RollBackTransaction(cmd.Transaction);
                throw new Exception("SPX_PARAM_RUES_RETOURNE" + ":" + ex.Message);
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                    cn.Close(); // Fermeture de la connection 
                cmd.Dispose();
            }
        }

        public List<CsRues> SelectAllRuesByCommune(CsCommune pCommune)
        {
            try
            {
                cn = new SqlConnection(ConnectionString);
                cmd = new SqlCommand
                          {
                              Connection = cn,
                              CommandType = CommandType.StoredProcedure,
                              CommandText = "SPX_PARAM_RUES_RETOURNEByCOMMUNE"
                          };
                cmd.Parameters.Clear();
                //cmd.Parameters.AddWithValue("@COMMUNE", pCommune.CODE);

                if (cn.State == ConnectionState.Closed)
                    cn.Open();

                StartTransaction(cn);

                IDataReader reader = cmd.ExecuteReader();
                var rows = new List<CsRues>();
                Fill(reader, rows, int.MinValue, int.MaxValue);
                reader.Close();
                return rows;
            }
            catch (Exception ex)
            {
                RollBackTransaction(cmd.Transaction);
                throw new Exception("SPX_PARAM_RUES_RETOURNEByCOMMUNE" + ":" + ex.Message);
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                    cn.Close(); // Fermeture de la connection 
                cmd.Dispose();
            }
        }

        public bool Delete(CsRues pRues)
        {
            try
            {
                cn = new SqlConnection(ConnectionString);
                cmd = new SqlCommand
                {
                    Connection = cn,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "SPX_PARAM_RUES_SUPPRIMER"
                };
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@CENTRE", pRues.CENTRE);
                cmd.Parameters.AddWithValue("@COMMUNE", pRues.COMMUNE);
                cmd.Parameters.AddWithValue("@RUE", pRues.RUE);
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
                throw new Exception("SPX_PARAM_RUES_SUPPRIMER" + ":" + ex.Message);
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                    cn.Close();
                cmd.Dispose();
            }
        }

        public bool Delete(List<CsRues> pRuesCollection)
        {
            int number = 0;
            foreach (CsRues entity in pRuesCollection)
            {
                if (Delete(entity))
                {
                    number++;
                }
            }
            return number != 0;
        }

        public bool Update(CsRues pRues)
        {
            cn = new SqlConnection(ConnectionString);
            cmd = new SqlCommand
            {
                Connection = cn,
                CommandType = CommandType.StoredProcedure,
                CommandText = "SPX_PARAM_RUES_UPDATE"
            };
            cmd.Parameters.Clear();

            try
            {
                cmd.Parameters.AddWithValue("CENTRE", pRues.CENTRE);
                cmd.Parameters.AddWithValue( "OriginalCENTRE", pRues.OriginalCENTRE);
                cmd.Parameters.AddWithValue( "COMMUNE", pRues.COMMUNE);
                cmd.Parameters.AddWithValue( "OriginalCOMMUNE", pRues.OriginalCOMMUNE);
                cmd.Parameters.AddWithValue( "RUE", pRues.RUE);
                cmd.Parameters.AddWithValue( "OriginalRUE", pRues.OriginalRUE);
                cmd.Parameters.AddWithValue( "NRUE", pRues.NRUE);
                cmd.Parameters.AddWithValue( "TRANS", pRues.TRANS);
                cmd.Parameters.AddWithValue( "DATECREATION", pRues.DATECREATION);
                cmd.Parameters.AddWithValue( "DATEMODIFICATION", pRues.DATEMODIFICATION);
                cmd.Parameters.AddWithValue( "USERCREATION", pRues.USERCREATION);
                cmd.Parameters.AddWithValue( "USERMODIFICATION",pRues.USERMODIFICATION);

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

        public bool Update(List<CsRues> pCsSiteCollection)
        {
            int number = 0;
            foreach (CsRues entity in pCsSiteCollection)
            {
                if (Update(entity))
                {
                    number++;
                }
            }
            return number != 0;
        }

        public bool Insert(CsRues pRues)
        {
            cn = new SqlConnection(ConnectionString);
            cmd = new SqlCommand
            {
                Connection = cn,
                CommandType = CommandType.StoredProcedure,
                CommandText = "SPX_PARAM_RUES_INSERER"
            };
            cmd.Parameters.Clear();

            try
            {
                cmd.Parameters.AddWithValue("CENTRE", pRues.CENTRE);
                cmd.Parameters.AddWithValue("COMMUNE", pRues.COMMUNE);
                cmd.Parameters.AddWithValue("RUE", pRues.RUE);
                cmd.Parameters.AddWithValue("NRUE", pRues.NRUE);
                cmd.Parameters.AddWithValue("TRANS", pRues.TRANS);
                cmd.Parameters.AddWithValue("DATECREATION", pRues.DATECREATION);
                cmd.Parameters.AddWithValue("DATEMODIFICATION", pRues.DATEMODIFICATION);
                cmd.Parameters.AddWithValue("USERCREATION", pRues.USERCREATION);
                cmd.Parameters.AddWithValue("USERMODIFICATION", pRues.USERMODIFICATION);
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

        public bool Insert(List<CsRues> pSiteCollection)
        {
            int number = 0;
            foreach (CsRues entity in pSiteCollection)
            {
                if (Insert(entity))
                {
                    number++;
                }
            }
            return number != 0;
        }

        public bool TestuniciteRues(CsRues pRues)
        {
            bool Result = false;
            try
            {
                cn = new SqlConnection(ConnectionString);
                cmd = new SqlCommand();
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SPX_PARAM_RUES_RETOURNEByCENTRECOMMUNERUE";

                cmd.Parameters.Clear();

                cmd.Parameters.Add("@CENTRE", SqlDbType.VarChar).Value = pRues.CENTRE;
                cmd.Parameters.Add("@COMMUNE", SqlDbType.VarChar).Value = pRues.COMMUNE;
                cmd.Parameters.Add("@RUE", SqlDbType.VarChar).Value = pRues.RUE;

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
                throw new Exception("SPX_PARAM_RUES_RETOURNEByCENTRECOMMUNERUE" + ":" + ex.Message);
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                    cn.Close(); // Fermeture de la connection 
                cmd.Dispose();
            }
            return Result;
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
        #endregion
       */

        public List<CsRues> SelectAllRues()
        {
            try
            {
                return Entities.GetEntityListFromQuery<CsRues>(ParamProcedure.PARAM_RUES_RETOURNE());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CsRues> SelectAllRuesByCommune(int pQuartierId)
        {
            try
            {
                return Entities.GetEntityListFromQuery<CsRues>(ParamProcedure.PARAM_RUES_RETOURNEByCommune(pQuartierId));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Delete(CsRues pRues)
        {
            try
            {
                return Entities.DeleteEntity<Galatee.Entity.Model.RUES>(Entities.ConvertObject<Galatee.Entity.Model.RUES, CsRues>(pRues));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Delete(List<CsRues> pRuesCollection)
        {
            try
            {
                return Entities.DeleteEntity<Galatee.Entity.Model.RUES>(Entities.ConvertObject<Galatee.Entity.Model.RUES, CsRues>(pRuesCollection));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Update(CsRues pRues)
        {
            try
            {
                return Entities.UpdateEntity<Galatee.Entity.Model.RUES>(Entities.ConvertObject<Galatee.Entity.Model.RUES, CsRues>(pRues));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool Update(List<CsRues> pRuesCollection)
        {
            try
            {
                return Entities.UpdateEntity<Galatee.Entity.Model.RUES>(Entities.ConvertObject<Galatee.Entity.Model.RUES, CsRues>(pRuesCollection));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Insert(CsRues pRues)
        {
            try
            {
                return Entities.InsertEntity<Galatee.Entity.Model.RUES>(Entities.ConvertObject<Galatee.Entity.Model.RUES, CsRues>(pRues));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Insert(List<CsRues> pRuesCollection)
        {
            try
            {
                return Entities.InsertEntity<Galatee.Entity.Model.RUES>(Entities.ConvertObject<Galatee.Entity.Model.RUES, CsRues>(pRuesCollection));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
