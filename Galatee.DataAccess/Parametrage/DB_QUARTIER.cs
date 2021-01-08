using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;

using Galatee.Structure;
using Galatee.Entity.Model;

namespace Galatee.DataAccess
{
    public class DB_QUARTIER /*: Galatee.DataAccess.Parametrage.DbBase*/
    {
        /*
        public DB_QUARTIER()
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

        public static List<CsQuartier> Fill(IDataReader reader, List<CsQuartier> rows, int start, int pageLength)
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

                CsQuartier c = new CsQuartier();
                //c.CENTRE = (Convert.IsDBNull(reader["CENTRE"])) ? string.Empty : (System.String)reader["CENTRE"];
                //c.OriginalCENTRE = (Convert.IsDBNull(reader["CENTRE"])) ? string.Empty : (System.String)reader["CENTRE"];
                //c.COMMUNE = (Convert.IsDBNull(reader["COMMUNE"])) ? string.Empty : (System.String)reader["COMMUNE"];
                //c.OriginalCOMMUNE = (Convert.IsDBNull(reader["COMMUNE"])) ? string.Empty : (System.String)reader["COMMUNE"];
                //c.QUARTIER = (Convert.IsDBNull(reader["QUARTIER"])) ? string.Empty : (System.String)reader["QUARTIER"];
                c.OriginalQUARTIER = (Convert.IsDBNull(reader["QUARTIER"])) ? string.Empty : (System.String)reader["QUARTIER"];
                c.LIBELLE = (Convert.IsDBNull(reader["LIBELLE"])) ? null : (System.String)reader["LIBELLE"];
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


        public List<CsQuartier> SelectAllQuartier()
        {
            cn = new SqlConnection(ConnectionString);
            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SPX_PARAM_QUARTIER_RETOURNE";

            try
            {
                if (cn.State == ConnectionState.Closed)
                    cn.Open();

                StartTransaction(cn);

                IDataReader reader = cmd.ExecuteReader();
                var rows = new List<CsQuartier>();
                Fill(reader, rows, int.MinValue, int.MaxValue);
                reader.Close();
                return rows;
            }
            catch (Exception ex)
            {
                RollBackTransaction(cmd.Transaction);
                throw new Exception("SPX_PARAM_QUARTIER_RETOURNE" + ":" + ex.Message);
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                    cn.Close(); // Fermeture de la connection 
                cmd.Dispose();
            }
        }

        public List<CsQuartier> SelectAllQuartierByCommune(CsCommune pCommune)
        {
            try
            {
                cn = new SqlConnection(ConnectionString);
                cmd = new SqlCommand
                          {
                              Connection = cn,
                              CommandType = CommandType.StoredProcedure,
                              CommandText = "SPX_PARAM_QUARTIER_RETOURNEByCOMMUNE"
                          };
                cmd.Parameters.Clear();
                //cmd.Parameters.AddWithValue("@COMMUNE", pCommune.CODE);

                if (cn.State == ConnectionState.Closed)
                    cn.Open();

                StartTransaction(cn);

                IDataReader reader = cmd.ExecuteReader();
                var rows = new List<CsQuartier>();
                Fill(reader, rows, int.MinValue, int.MaxValue);
                reader.Close();
                return rows;
            }
            catch (Exception ex)
            {
                RollBackTransaction(cmd.Transaction);
                throw new Exception("SPX_PARAM_QUARTIER_RETOURNEByCOMMUNE" + ":" + ex.Message);
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                    cn.Close(); // Fermeture de la connection 
                cmd.Dispose();
            }
        }

        public bool Delete(CsQuartier pQuartier)
        {
            try
            {
                cn = new SqlConnection(ConnectionString);
                cmd = new SqlCommand
                {
                    Connection = cn,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "SPX_PARAM_QUARTIER_SUPPRIMER"
                };
                cmd.Parameters.Clear();
                //cmd.Parameters.AddWithValue("@CENTRE", pQuartier.CENTRE);
                //cmd.Parameters.AddWithValue("@COMMUNE", pQuartier.COMMUNE);
                //cmd.Parameters.AddWithValue("@QUARTIER", pQuartier.QUARTIER);
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
                throw new Exception("SPX_PARAM_QUARTIER_SUPPRIMER" + ":" + ex.Message);
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                    cn.Close();
                cmd.Dispose();
            }
        }

        public bool Delete(List<CsQuartier> pQuartierCollection)
        {
            int number = 0;
            foreach (CsQuartier entity in pQuartierCollection)
            {
                if (Delete(entity))
                {
                    number++;
                }
            }
            return number != 0;
        }

        public bool Update(CsQuartier pQuartier)
        {
            cn = new SqlConnection(ConnectionString);
            cmd = new SqlCommand
            {
                Connection = cn,
                CommandType = CommandType.StoredProcedure,
                CommandText = "SPX_PARAM_QUARTIER_UPDATE"
            };
            cmd.Parameters.Clear();

            try
            {
                //cmd.Parameters.AddWithValue("CENTRE", pQuartier.CENTRE);
                //cmd.Parameters.AddWithValue("OriginalCENTRE", pQuartier.OriginalCENTRE);
                //cmd.Parameters.AddWithValue("COMMUNE", pQuartier.COMMUNE);
                //cmd.Parameters.AddWithValue("OriginalCOMMUNE", pQuartier.OriginalCOMMUNE);
                //cmd.Parameters.AddWithValue("QUARTIER", pQuartier.QUARTIER);
                cmd.Parameters.AddWithValue("OriginalQUARTIER", pQuartier.OriginalQUARTIER);
                cmd.Parameters.AddWithValue("LIBELLE", pQuartier.LIBELLE);
                cmd.Parameters.AddWithValue("TRANS", pQuartier.TRANS);
                cmd.Parameters.AddWithValue("DATECREATION", pQuartier.DATECREATION);
                cmd.Parameters.AddWithValue("DATEMODIFICATION", pQuartier.DATEMODIFICATION);
                cmd.Parameters.AddWithValue("USERCREATION", pQuartier.USERCREATION);
                cmd.Parameters.AddWithValue("USERMODIFICATION",pQuartier.USERMODIFICATION);

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

        public bool Update(List<CsQuartier> pQuartierCollection)
        {
            int number = 0;
            foreach (CsQuartier entity in pQuartierCollection)
            {
                if (Update(entity))
                {
                    number++;
                }
            }
            return number != 0;
        }

        public bool Insert(CsQuartier pQuartier)
        {
            cn = new SqlConnection(ConnectionString);
            cmd = new SqlCommand
            {
                Connection = cn,
                CommandType = CommandType.StoredProcedure,
                CommandText = "SPX_PARAM_QUARTIER_INSERER"
            };
            cmd.Parameters.Clear();

            try
            {
                //cmd.Parameters.AddWithValue("CENTRE", pQuartier.CENTRE);
                //cmd.Parameters.AddWithValue("COMMUNE", pQuartier.COMMUNE);
                //cmd.Parameters.AddWithValue("QUARTIER", pQuartier.QUARTIER);
                cmd.Parameters.AddWithValue("LIBELLE", pQuartier.LIBELLE);
                cmd.Parameters.AddWithValue("TRANS", pQuartier.TRANS);
                cmd.Parameters.AddWithValue("DATECREATION", pQuartier.DATECREATION);
                cmd.Parameters.AddWithValue("DATEMODIFICATION", pQuartier.DATEMODIFICATION);
                cmd.Parameters.AddWithValue("USERCREATION", pQuartier.USERCREATION);
                cmd.Parameters.AddWithValue("USERMODIFICATION", pQuartier.USERMODIFICATION);

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

        public bool Insert(List<CsQuartier> pSiteCollection)
        {
            int number = 0;
            foreach (CsQuartier entity in pSiteCollection)
            {
                if (Insert(entity))
                {
                    number++;
                }
            }
            return number != 0;
        }

        public bool TestuniciteQuartier(CsQuartier pQuartier)
        {
            bool Result = false;
            try
            {
                cn = new SqlConnection(ConnectionString);
                cmd = new SqlCommand();
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SPX_PARAM_QUARTIER_RETOURNEByCENTRECOMMUNEQUARTIER";

                cmd.Parameters.Clear();

                //cmd.Parameters.Add("@CENTRE", SqlDbType.VarChar).Value = pQuartier.CENTRE;
                //cmd.Parameters.Add("@COMMUNE", SqlDbType.VarChar).Value = pQuartier.COMMUNE;
                //cmd.Parameters.Add("@QUARTIER", SqlDbType.VarChar).Value = pQuartier.QUARTIER;

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
                throw new Exception("SPX_PARAM_QUARTIER_RETOURNEByCENTRECOMMUNEQUARTIER" + ":" + ex.Message);
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

        */

        public List<CsQuartier> SelectAllQuartier()
        {
            try
            {
                return Entities.GetEntityListFromQuery<CsQuartier>(ParamProcedure.PARAM_QUARTIER_RETOURNE());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CsQuartier> SelectAllQuartierByCommune(int pCommune)
        {
            try
            {
                return Entities.GetEntityListFromQuery<CsQuartier>(ParamProcedure.PARAM_QUARTIER_RETOURNEByCommune(pCommune));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Delete(CsQuartier pQuartier)
        {
            try
            {
                return Entities.DeleteEntity<Galatee.Entity.Model.QUARTIER>(Entities.ConvertObject<Galatee.Entity.Model.QUARTIER, CsQuartier>(pQuartier));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Delete(List<CsQuartier> pQuartierCollection)
        {
            try
            {
                return Entities.DeleteEntity<Galatee.Entity.Model.QUARTIER>(Entities.ConvertObject<Galatee.Entity.Model.QUARTIER, CsQuartier>(pQuartierCollection));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Update(CsQuartier pQuartier)
        {
            try
            {
                return Entities.UpdateEntity<Galatee.Entity.Model.QUARTIER>(Entities.ConvertObject<Galatee.Entity.Model.QUARTIER, CsQuartier>(pQuartier));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Update(List<CsQuartier> pQuartierCollection)
        {
            try
            {
                return Entities.UpdateEntity<Galatee.Entity.Model.QUARTIER>(Entities.ConvertObject<Galatee.Entity.Model.QUARTIER, CsQuartier>(pQuartierCollection));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Insert(CsQuartier pQuartier)
        {
            try
            {
                return Entities.InsertEntity<Galatee.Entity.Model.QUARTIER>(Entities.ConvertObject<Galatee.Entity.Model.QUARTIER, CsQuartier>(pQuartier));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Insert(List<CsQuartier> pQuartierCollection)
        {
            try
            {
                return Entities.InsertEntity<Galatee.Entity.Model.QUARTIER>(Entities.ConvertObject<Galatee.Entity.Model.QUARTIER, CsQuartier>(pQuartierCollection));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
