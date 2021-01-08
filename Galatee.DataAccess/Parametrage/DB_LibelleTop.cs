using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Galatee.Structure;
using Galatee.Entity.Model;

namespace Galatee.DataAccess
{
    public class DB_LibelleTop /*: Galatee.DataAccess.Parametrage.DbBase*/
    {
        /*
        private string ConnectionString;

        public DB_LibelleTop()
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

        public DB_LibelleTop(string ConnStr)
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

        public List<CsLibelleTop> SelectAllLibelleTop()
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
                              CommandText = EnumProcedureStockee.SelectLIBELLETOP
                          };
                IDataReader reader = cmd.ExecuteReader();
                var rows = new List<CsLibelleTop>();
                Fill(reader, rows, int.MinValue, int.MaxValue);
                reader.Close();
                return rows;
            }
            catch (Exception ex)
            {
                throw new Exception(EnumProcedureStockee.SelectLIBELLETOP + ":" + ex.Message);
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                    cn.Close(); // Fermeture de la connection 
                cmd.Dispose();
            }
        }

        public bool Delete(CsLibelleTop pLibelleTop)
        {
            try
            {
                cn = new SqlConnection(ConnectionString);
                cmd = new SqlCommand
                {
                    Connection = cn,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = EnumProcedureStockee.DeleteLIBELLETOP
                };
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@CODE", pLibelleTop.CODE);
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
                throw new Exception(EnumProcedureStockee.DeleteLIBELLETOP + ":" + ex.Message);
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                cn.Close();
                cmd.Dispose();
            }
        }

        public bool Delete(List<CsLibelleTop> pLibelleTopCollection)
        {
            int number = 0;
            foreach (CsLibelleTop entity in pLibelleTopCollection)
            {
                if (Delete(entity))
                {
                    number++;
                }
            }
            return number != 0;
        }

        public static List<CsLibelleTop> Fill(IDataReader reader, List<CsLibelleTop> rows, int start, int pageLength)
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

                var c = new CsLibelleTop();
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

        public bool Update(CsLibelleTop pLibelleTop)
        {
                cn = new SqlConnection(ConnectionString);
                cmd = new SqlCommand
                          {
                              Connection = cn,
                              CommandType = CommandType.StoredProcedure,
                              CommandText = EnumProcedureStockee.UpdateLIBELLETOP
                          };
                cmd.Parameters.Clear();

                try
                {
                    cmd.Parameters.AddWithValue("@CODE", pLibelleTop.CODE);
                    cmd.Parameters.AddWithValue("@LIBELLE", pLibelleTop.LIBELLE);
                    cmd.Parameters.AddWithValue("@OriginalCODE", pLibelleTop.OriginalCODE);
                    cmd.Parameters.AddWithValue("@DATECREATION", pLibelleTop.DATECREATION);
                    cmd.Parameters.AddWithValue("@DATEMODIFICATION", pLibelleTop.DATEMODIFICATION);
                    cmd.Parameters.AddWithValue("@USERCREATION", pLibelleTop.USERCREATION);
                    cmd.Parameters.AddWithValue("@USERMODIFICATION", pLibelleTop.USERMODIFICATION);
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

        public bool Update(List<CsLibelleTop> pCsLibelleTopCollection)
        {
            int number = 0;
            foreach (CsLibelleTop entity in pCsLibelleTopCollection)
            {
                if (Update(entity))
                {
                    number++;
                }
            }
            return number != 0;
        }

        public bool Insert(CsLibelleTop pLibelleTop)
        {
                cn = new SqlConnection(ConnectionString);
                cmd = new SqlCommand
                {
                    Connection = cn,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = EnumProcedureStockee.InsertLIBELLETOP
                };
                cmd.Parameters.Clear();

                try
                {
                    cmd.Parameters.AddWithValue("@CODE", pLibelleTop.CODE);
                    cmd.Parameters.AddWithValue("@LIBELLE", pLibelleTop.LIBELLE);
                    cmd.Parameters.AddWithValue("@DATECREATION", pLibelleTop.DATECREATION);
                    cmd.Parameters.AddWithValue("@DATEMODIFICATION", pLibelleTop.DATEMODIFICATION);
                    cmd.Parameters.AddWithValue("@USERCREATION", pLibelleTop.USERCREATION);
                    cmd.Parameters.AddWithValue("@USERMODIFICATION", pLibelleTop.USERMODIFICATION);
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

        public bool Insert(List<CsLibelleTop> pLibelleTopCollection)
        {
            int number = 0;
            foreach (CsLibelleTop entity in pLibelleTopCollection)
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

        public List<CsLibelleTop> SelectAllLibelleTop()
        {
            try
            {
                return Entities.GetEntityListFromQuery<CsLibelleTop>(ParamProcedure.PARAM_LIBELLETOP_RETOURNE());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Delete(CsLibelleTop pLibelleTop)
        {
            try
            {
                return Entities.DeleteEntity<Galatee.Entity.Model.LIBELLETOP>(Entities.ConvertObject<Galatee.Entity.Model.LIBELLETOP, CsLibelleTop>(pLibelleTop));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Delete(List<CsLibelleTop> pLibelleTopCollection)
        {
            try
            {
                return Entities.DeleteEntity<Galatee.Entity.Model.LIBELLETOP>(Entities.ConvertObject<Galatee.Entity.Model.LIBELLETOP, CsLibelleTop>(pLibelleTopCollection));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Update(CsLibelleTop pLibelleTop)
        {
            try
            {
                return Entities.UpdateEntity<Galatee.Entity.Model.LIBELLETOP>(Entities.ConvertObject<Galatee.Entity.Model.LIBELLETOP, CsLibelleTop>(pLibelleTop));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Update(List<CsLibelleTop> pLibelleTopCollection)
        {
            try
            {
                return Entities.UpdateEntity<Galatee.Entity.Model.LIBELLETOP>(Entities.ConvertObject<Galatee.Entity.Model.LIBELLETOP, CsLibelleTop>(pLibelleTopCollection));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Insert(CsLibelleTop pLibelleTop)
        {
            try
            {
                return Entities.InsertEntity<Galatee.Entity.Model.LIBELLETOP>(Entities.ConvertObject<Galatee.Entity.Model.LIBELLETOP, CsLibelleTop>(pLibelleTop));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Insert(List<CsLibelleTop> pLibelleTopCollection)
        {
            try
            {
                return Entities.InsertEntity<Galatee.Entity.Model.LIBELLETOP>(Entities.ConvertObject<Galatee.Entity.Model.LIBELLETOP, CsLibelleTop>(pLibelleTopCollection));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
