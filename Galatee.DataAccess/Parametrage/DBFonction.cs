using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Galatee.Structure;
using Galatee.Entity.Model;

namespace Galatee.DataAccess
{
    public class DBFonction /*: Galatee.DataAccess.Parametrage.DbBase*/
    {
        /*
        private string ConnectionString;

        public DBFonction()
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

        public DBFonction(string ConnStr)
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

        public List<CsFonction> SelectAllFonction()
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
                              CommandText = "SPX_PARAM_FONCTION_RETOURNE"
                          };
                IDataReader reader = cmd.ExecuteReader();
                var rows = new List<CsFonction>();
                Fill(reader, rows, int.MinValue, int.MaxValue);
                reader.Close();
                return rows;
            }
            catch (Exception ex)
            {
                throw new Exception("SPX_PARAM_FONCTION_RETOURNE" + ":" + ex.Message);
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                    cn.Close(); // Fermeture de la connection 
                cmd.Dispose();
            }
        }

        public bool Delete(CsFonction pFonction)
        {
            try
            {
                cn = new SqlConnection(ConnectionString);
                cmd = new SqlCommand
                {
                    Connection = cn,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "SPX_PARAM_FONCTION_SUPPRIMER"
                };
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@CODE", pFonction.CODE);
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
                throw new Exception("SPX_PARAM_FONCTION_SUPPRIMER" + ":" + ex.Message);
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                cn.Close();
                cmd.Dispose();
            }
        }

        public CsFonction SelectFonctionByCode(string pFonction)
        {
            try
            {
                cn = new SqlConnection(ConnectionString);
                cmd = new SqlCommand
                          {
                              Connection = cn,
                              CommandType = CommandType.StoredProcedure,
                              CommandText = "SPX_PARAM_FONCTION_RETOURNEByCODE"
                          };
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@CODE", pFonction);
                if (cn.State == ConnectionState.Closed)
                    cn.Open();
                StartTransaction(cn);
                IDataReader reader = cmd.ExecuteReader();
                var rows = new List<CsFonction>();
                Fill(reader, rows, int.MinValue, int.MaxValue);
                reader.Close();
                if (rows.Count > 0)
                    return rows[0];
                else
                    return null;
            }
            catch (Exception ex)
            {
                RollBackTransaction(cmd.Transaction);
                throw new Exception("SPX_PARAM_FONCTION_RETOURNEByCODE" + ":" + ex.Message);
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                    cn.Close();
                cmd.Dispose();
            }
        }

        public bool Delete(List<CsFonction> pFonctionCollection)
        {
            int number = 0;
            foreach (CsFonction entity in pFonctionCollection)
            {
                if (Delete(entity))
                {
                    number++;
                }
            }
            return number != 0;
        }

        public static List<CsFonction> Fill(IDataReader reader, List<CsFonction> rows, int start, int pageLength)
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

                var c = new CsFonction();
				c.CODE = (Convert.IsDBNull(reader["CODE"]))?string.Empty:(System.String)reader["CODE"];
				c.OriginalCODE = (Convert.IsDBNull(reader["CODE"]))?string.Empty:(System.String)reader["CODE"];
                c.ROLENAME = (Convert.IsDBNull(reader["ROLENAME"])) ? string.Empty : (System.String)reader["ROLENAME"];
                c.USERCREATION = (Convert.IsDBNull(reader["USERCREATION"])) ? string.Empty : (System.String)reader["USERCREATION"];
                c.USERMODIFICATION = (Convert.IsDBNull(reader["USERMODIFICATION"])) ? string.Empty : (System.String)reader["USERMODIFICATION"];
                c.DATECREATION = (Convert.IsDBNull(reader["DATECREATION"])) ? (DateTime?)null : (System.DateTime)reader["DATECREATION"];
                c.DATEMODIFICATION = (Convert.IsDBNull(reader["DATEMODIFICATION"])) ? (DateTime?)null : (System.DateTime)reader["DATEMODIFICATION"];
				rows.Add(c);
			}
			return rows;
		}

        public bool Update(CsFonction pFonction)
        {
                cn = new SqlConnection(ConnectionString);
                cmd = new SqlCommand
                          {
                              Connection = cn,
                              CommandType = CommandType.StoredProcedure,
                              CommandText = "SPX_PARAM_FONCTION_UPDATE"
                          };
                cmd.Parameters.Clear();

                try
                {
                    cmd.Parameters.AddWithValue("@CODE", pFonction.CODE);
                    cmd.Parameters.AddWithValue("@LIBELLE", pFonction.ROLENAME);
                    cmd.Parameters.AddWithValue("@OriginalCODE", pFonction.OriginalCODE);
                    cmd.Parameters.AddWithValue("@DATECREATION", pFonction.DATECREATION);
                    cmd.Parameters.AddWithValue("@DATEMODIFICATION", pFonction.DATEMODIFICATION);
                    cmd.Parameters.AddWithValue("@USERCREATION", pFonction.USERCREATION);
                    cmd.Parameters.AddWithValue("@USERMODIFICATION", pFonction.USERMODIFICATION);
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

        public bool Update(List<CsFonction> pFonctionCollection)
        {
            int number = 0;
            foreach (CsFonction entity in pFonctionCollection)
            {
                if (Update(entity))
                {
                    number++;
                }
            }
            return number != 0;
        }

        public bool Insert(CsFonction pFonction)
        {
                cn = new SqlConnection(ConnectionString);
                cmd = new SqlCommand
                {
                    Connection = cn,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "SPX_PARAM_FONCTION_INSERER"
                };
                cmd.Parameters.Clear();

                try
                {
                    cmd.Parameters.AddWithValue("@CODE", pFonction.CODE);
                    cmd.Parameters.AddWithValue("@LIBELLE", pFonction.ROLENAME);
                    cmd.Parameters.AddWithValue("@DATECREATION", pFonction.DATECREATION);
                    cmd.Parameters.AddWithValue("@DATEMODIFICATION", pFonction.DATEMODIFICATION);
                    cmd.Parameters.AddWithValue("@USERCREATION", pFonction.USERCREATION);
                    cmd.Parameters.AddWithValue("@USERMODIFICATION", pFonction.USERMODIFICATION);
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

        public bool Insert(List<CsFonction> pFonctionCollection)
        {
            int number = 0;
            foreach (CsFonction entity in pFonctionCollection)
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

        public List<CsFonction> SelectAllFonction()
        {
            try
            {
                return Entities.GetEntityListFromQuery<CsFonction>(ParamProcedure.PARAM_FONCTION_RETOURNE());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Delete(CsFonction pFonction)
        {
            try
            {
                return Entities.DeleteEntity<Galatee.Entity.Model.FONCTION>(Entities.ConvertObject<Galatee.Entity.Model.FONCTION, CsFonction>(pFonction));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public CsFonction SelectFonctionByCode(int pFonctionId)
        {
            try
            {
                return Entities.GetEntityFromQuery<CsFonction>(ParamProcedure.PARAM_FONCTION_RETOURNEById(pFonctionId));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Delete(List<CsFonction> pFonctionCollection)
        {
            try
            {
                return Entities.DeleteEntity<Galatee.Entity.Model.FONCTION>(Entities.ConvertObject<Galatee.Entity.Model.FONCTION, CsFonction>(pFonctionCollection));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Update(CsFonction pFonction)
        {
            try
            {
                return Entities.UpdateEntity<Galatee.Entity.Model.FONCTION>(Entities.ConvertObject<Galatee.Entity.Model.FONCTION, CsFonction>(pFonction));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Update(List<CsFonction> pFonctionCollection)
        {
            try
            {
                return Entities.UpdateEntity<Galatee.Entity.Model.FONCTION>(Entities.ConvertObject<Galatee.Entity.Model.FONCTION, CsFonction>(pFonctionCollection));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Insert(CsFonction pFonction)
        {
            try
            {
                return Entities.InsertEntity<Galatee.Entity.Model.FONCTION>(Entities.ConvertObject<Galatee.Entity.Model.FONCTION, CsFonction>(pFonction));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Insert(List<CsFonction> pFonctionCollection)
        {
            try
            {
                return Entities.InsertEntity<Galatee.Entity.Model.FONCTION>(Entities.ConvertObject<Galatee.Entity.Model.FONCTION, CsFonction>(pFonctionCollection));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
