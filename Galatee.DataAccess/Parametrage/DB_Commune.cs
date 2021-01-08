using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Galatee.Structure;
using Galatee.Entity.Model;

namespace Galatee.DataAccess
{
    public class DB_Commune : Galatee.DataAccess.Parametrage.DbBase
    {
        /*
        private string ConnectionString;

        public DB_Commune()
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

        public DB_Commune(string ConnStr)
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

        public List<CsCommune> SelectAllCommune()
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
                              CommandText = "SPX_PARAM_COMMUNE_RETOURNE"
                          };
                IDataReader reader = cmd.ExecuteReader();
                var rows = new List<CsCommune>();
                Fill(reader, rows, int.MinValue, int.MaxValue);
                reader.Close();
                return rows;
            }
            catch (Exception ex)
            {
                throw new Exception("SPX_PARAM_COMMUNE_RETOURNE" + ":" + ex.Message);
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                    cn.Close(); // Fermeture de la connection 
                cmd.Dispose();
            }
        }

        public bool Delete(CsCommune pCommune)
        {
            try
            {
                cn = new SqlConnection(ConnectionString);
                cmd = new SqlCommand
                {
                    Connection = cn,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "SPX_PARAM_COMMUNE_SUPPRIMER"
                };
                cmd.Parameters.Clear();
                //cmd.Parameters.AddWithValue("@CODE", pCommune.PK_COMMUNE);
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
                throw new Exception("SPX_PARAM_COMMUNE_SUPPRIMER" + ":" + ex.Message);
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                cn.Close();
                cmd.Dispose();
            }
        }

        public bool Delete(List<CsCommune> pCommuneCollection)
        {
            int number = 0;
            foreach (CsCommune entity in pCommuneCollection)
            {
                if (Delete(entity))
                {
                    number++;
                }
            }
            return number != 0;
        }

        public static List<CsCommune> Fill(IDataReader reader, List<CsCommune> rows, int start, int pageLength)
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

                var c = new CsCommune();
                //c.PK_COMMUNE = (Convert.IsDBNull(reader["CODE"])) ? string.Empty : (System.String)reader["CODE"];
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

        public bool Update(CsCommune pCommune)
        {
                cn = new SqlConnection(ConnectionString);
                cmd = new SqlCommand
                          {
                              Connection = cn,
                              CommandType = CommandType.StoredProcedure,
                              CommandText = "SPX_PARAM_COMMUNE_UPDATE"
                          };
                cmd.Parameters.Clear();

                try
                {
                    //cmd.Parameters.AddWithValue("@CODE", pCommune.CODE);
                    cmd.Parameters.AddWithValue("@LIBELLE", pCommune.LIBELLE);
                    cmd.Parameters.AddWithValue("@OriginalCODE", pCommune.OriginalCODE);
                    cmd.Parameters.AddWithValue("@DATECREATION", pCommune.DATECREATION);
                    cmd.Parameters.AddWithValue("@DATEMODIFICATION", pCommune.DATEMODIFICATION);
                    cmd.Parameters.AddWithValue("@USERCREATION", pCommune.USERCREATION);
                    cmd.Parameters.AddWithValue("@USERMODIFICATION", pCommune.USERMODIFICATION);
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

        public bool Update(List<CsCommune> pCommuneCollection)
        {
            int number = 0;
            foreach (CsCommune entity in pCommuneCollection)
            {
                if (Update(entity))
                {
                    number++;
                }
            }
            return number != 0;
        }

        public bool Insert(CsCommune pCommune)
        {
                cn = new SqlConnection(ConnectionString);
                cmd = new SqlCommand
                {
                    Connection = cn,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "SPX_PARAM_COMMUNE_INSERER"
                };
                cmd.Parameters.Clear();

                try
                {
                    //cmd.Parameters.AddWithValue("@CODE", pCommune.CODE);
                    cmd.Parameters.AddWithValue("@LIBELLE", pCommune.LIBELLE);
                    cmd.Parameters.AddWithValue("@DATECREATION", pCommune.DATECREATION);
                    cmd.Parameters.AddWithValue("@DATEMODIFICATION", pCommune.DATEMODIFICATION);
                    cmd.Parameters.AddWithValue("@USERCREATION", pCommune.USERCREATION);
                    cmd.Parameters.AddWithValue("@USERMODIFICATION", pCommune.USERMODIFICATION);
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

        public bool Insert(List<CsCommune> pCommuneCollection)
        {
            int number = 0;
            foreach (CsCommune entity in pCommuneCollection)
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

        public List<CsCommune> SelectAllCommune()
        {
            try
            {
                return Entities.GetEntityListFromQuery<CsCommune>(ParamProcedure.PARAM_COMMUNE_RETOURNE());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Delete(CsCommune pCommune)
        {
            try
            {
                return Entities.DeleteEntity<Galatee.Entity.Model.COMMUNE>(Entities.ConvertObject<Galatee.Entity.Model.COMMUNE, CsCommune>(pCommune));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Delete(List<CsCommune> pCommuneCollection)
        {
            try
            {
                return Entities.DeleteEntity<Galatee.Entity.Model.COMMUNE>(Entities.ConvertObject<Galatee.Entity.Model.COMMUNE, CsCommune>(pCommuneCollection));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Update(CsCommune pCommune)
        {
            try
            {
                return Entities.UpdateEntity<Galatee.Entity.Model.COMMUNE>(Entities.ConvertObject<Galatee.Entity.Model.COMMUNE, CsCommune>(pCommune));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Update(List<CsCommune> pCommuneCollection)
        {
            try
            {
                return Entities.UpdateEntity<Galatee.Entity.Model.COMMUNE>(Entities.ConvertObject<Galatee.Entity.Model.COMMUNE, CsCommune>(pCommuneCollection));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Insert(CsCommune pCommune)
        {
            try
            {
                return Entities.InsertEntity<Galatee.Entity.Model.COMMUNE>(Entities.ConvertObject<Galatee.Entity.Model.COMMUNE, CsCommune>(pCommune));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Insert(List<CsCommune> pCommuneCollection)
        {
            try
            {
                return Entities.InsertEntity<Galatee.Entity.Model.COMMUNE>(Entities.ConvertObject<Galatee.Entity.Model.COMMUNE, CsCommune>(pCommuneCollection));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
