using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Galatee.Structure;
using Galatee.Entity.Model;

namespace Galatee.DataAccess
{
    public class DB_NumeroInstallation /*: Galatee.DataAccess.Parametrage.DbBase*/
    {
        /*
        private string ConnectionString;

        public DB_NumeroInstallation()
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

        public DB_NumeroInstallation(string ConnStr)
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

        public List<CsNumeroInstallation> SelectAllNumeroInstallation()
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
                              CommandText = EnumProcedureStockee.SelectNUMEROINSTALLATION
                          };
                IDataReader reader = cmd.ExecuteReader();
                var rows = new List<CsNumeroInstallation>();
                Fill(reader, rows, int.MinValue, int.MaxValue);
                reader.Close();
                return rows;
            }
            catch (Exception ex)
            {
                throw new Exception(EnumProcedureStockee.SelectNUMEROINSTALLATION + ":" + ex.Message);
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                    cn.Close(); // Fermeture de la connection 
                cmd.Dispose();
            }
        }

        public bool Delete(CsNumeroInstallation pNumeroInstallation)
        {
            try
            {
                cn = new SqlConnection(ConnectionString);
                cmd = new SqlCommand
                {
                    Connection = cn,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = EnumProcedureStockee.DeleteNUMEROINSTALLATION
                };
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@CODE", pNumeroInstallation.CODE);
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
                throw new Exception(EnumProcedureStockee.DeleteNUMEROINSTALLATION + ":" + ex.Message);
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                cn.Close();
                cmd.Dispose();
            }
        }

        public bool Delete(List<CsNumeroInstallation> pNumeroInstallationCollection)
        {
            int number = 0;
            foreach (CsNumeroInstallation entity in pNumeroInstallationCollection)
            {
                if (Delete(entity))
                {
                    number++;
                }
            }
            return number != 0;
        }

        public static List<CsNumeroInstallation> Fill(IDataReader reader, List<CsNumeroInstallation> rows, int start, int pageLength)
		{
			// advance to the starting row
			for (int i = 0; i < start; i++)
			{
				if (! reader.Read() )
					return rows; // not enough rows, just return
			}

			for (int i = 0; i < pageLength; i++)
			{
				if (!reader.Read())
					break; // we are done

                var c = new CsNumeroInstallation();
				c.CODE = (Convert.IsDBNull(reader["CODE"]))?string.Empty:(System.String)reader["CODE"];
				c.OriginalCODE = (Convert.IsDBNull(reader["CODE"]))?string.Empty:(System.String)reader["CODE"];
                c.LIBELLE = (Convert.IsDBNull(reader["LIBELLE"])) ? string.Empty : (System.String)reader["LIBELLE"];
                c.USERCREATION = (Convert.IsDBNull(reader["USERCREATION"])) ? string.Empty : (System.String)reader["USERCREATION"];
                c.USERMODIFICATION = (Convert.IsDBNull(reader["USERMODIFICATION"])) ? string.Empty : (System.String)reader["USERMODIFICATION"];
                c.DATECREATION = (Convert.IsDBNull(reader["DATECREATION"])) ? (DateTime?)null : (System.DateTime)reader["DATECREATION"];
                c.DATEMODIFICATION = (Convert.IsDBNull(reader["DATEMODIFICATION"])) ? (DateTime?)null : (System.DateTime)reader["DATEMODIFICATION"];
				rows.Add(c);
			}
			return rows;
		}

        public bool Update(CsNumeroInstallation pNumeroInstallation)
        {
                cn = new SqlConnection(ConnectionString);
                cmd = new SqlCommand
                          {
                              Connection = cn,
                              CommandType = CommandType.StoredProcedure,
                              CommandText = EnumProcedureStockee.UpdateNUMEROINSTALLATION
                          };
                cmd.Parameters.Clear();

                try
                {
                    cmd.Parameters.AddWithValue("@CODE", pNumeroInstallation.CODE);
                    cmd.Parameters.AddWithValue("@LIBELLE", pNumeroInstallation.LIBELLE);
                    cmd.Parameters.AddWithValue("@OriginalCODE", pNumeroInstallation.OriginalCODE);
                    cmd.Parameters.AddWithValue("@DATECREATION", pNumeroInstallation.DATECREATION);
                    cmd.Parameters.AddWithValue("@DATEMODIFICATION", pNumeroInstallation.DATEMODIFICATION);
                    cmd.Parameters.AddWithValue("@USERCREATION", pNumeroInstallation.USERCREATION);
                    cmd.Parameters.AddWithValue("@USERMODIFICATION", pNumeroInstallation.USERMODIFICATION);
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

        public bool Update(List<CsNumeroInstallation> pCsNumeroInstallationCollection)
        {
            int number = 0;
            foreach (CsNumeroInstallation entity in pCsNumeroInstallationCollection)
            {
                if (Update(entity))
                {
                    number++;
                }
            }
            return number != 0;
        }

        public bool Insert(CsNumeroInstallation pNumeroInstallation)
        {
                cn = new SqlConnection(ConnectionString);
                cmd = new SqlCommand
                {
                    Connection = cn,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = EnumProcedureStockee.InsertNUMEROINSTALLATION
                };
                cmd.Parameters.Clear();

                try
                {
                    cmd.Parameters.AddWithValue("@CODE", pNumeroInstallation.CODE);
                    cmd.Parameters.AddWithValue("@LIBELLE", pNumeroInstallation.LIBELLE);
                    cmd.Parameters.AddWithValue("@DATECREATION", pNumeroInstallation.DATECREATION);
                    cmd.Parameters.AddWithValue("@DATEMODIFICATION", pNumeroInstallation.DATEMODIFICATION);
                    cmd.Parameters.AddWithValue("@USERCREATION", pNumeroInstallation.USERCREATION);
                    cmd.Parameters.AddWithValue("@USERMODIFICATION", pNumeroInstallation.USERMODIFICATION);
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

        public bool Insert(List<CsNumeroInstallation> pNumeroInstallationCollection)
        {
            int number = 0;
            foreach (CsNumeroInstallation entity in pNumeroInstallationCollection)
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

        public List<CsNumeroInstallation> SelectAllNumeroInstallation()
        {
            try
            {
                return Entities.GetEntityListFromQuery<CsNumeroInstallation>(ParamProcedure.PARAM_NUMEROINSTALLATION_RETOURNE());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Delete(CsNumeroInstallation pNumeroInstallation)
        {
            try
            {
                return Entities.DeleteEntity<Galatee.Entity.Model.NUMEROINSTALLATION>(Entities.ConvertObject<Galatee.Entity.Model.NUMEROINSTALLATION, CsNumeroInstallation>(pNumeroInstallation));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Delete(List<CsNumeroInstallation> pNumeroInstallationCollection)
        {
            try
            {
                return Entities.DeleteEntity<Galatee.Entity.Model.NUMEROINSTALLATION>(Entities.ConvertObject<Galatee.Entity.Model.NUMEROINSTALLATION, CsNumeroInstallation>(pNumeroInstallationCollection));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Update(CsNumeroInstallation pNumeroInstallation)
        {
            try
            {
                return Entities.UpdateEntity<Galatee.Entity.Model.NUMEROINSTALLATION>(Entities.ConvertObject<Galatee.Entity.Model.NUMEROINSTALLATION, CsNumeroInstallation>(pNumeroInstallation));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Update(List<CsNumeroInstallation> pNumeroInstallationCollection)
        {
            try
            {
                return Entities.UpdateEntity<Galatee.Entity.Model.NUMEROINSTALLATION>(Entities.ConvertObject<Galatee.Entity.Model.NUMEROINSTALLATION, CsNumeroInstallation>(pNumeroInstallationCollection));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Insert(CsNumeroInstallation pNumeroInstallation)
        {
            try
            {
                return Entities.InsertEntity<Galatee.Entity.Model.NUMEROINSTALLATION>(Entities.ConvertObject<Galatee.Entity.Model.NUMEROINSTALLATION, CsNumeroInstallation>(pNumeroInstallation));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Insert(List<CsNumeroInstallation> pNumeroInstallationCollection)
        {
            try
            {
                return Entities.InsertEntity<Galatee.Entity.Model.NUMEROINSTALLATION>(Entities.ConvertObject<Galatee.Entity.Model.NUMEROINSTALLATION, CsNumeroInstallation>(pNumeroInstallationCollection));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
