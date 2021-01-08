using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Galatee.Structure;
using Galatee.Entity.Model;

namespace Galatee.DataAccess
{
    public class DBMarqueCompteur : Galatee.DataAccess.Parametrage.DbBase
    {
        /*
        private string ConnectionString;

        public DBMarqueCompteur()
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

        public DBMarqueCompteur(string ConnStr)
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

        public List<CsMarqueCompteur> SelectAllMarqueCompteur()
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
                              CommandText = "SPX_PARAM_MARQUECOMPTEUR_RETOURNE"
                          };
                IDataReader reader = cmd.ExecuteReader();
                var rows = new List<CsMarqueCompteur>();
                Fill(reader, rows, int.MinValue, int.MaxValue);
                reader.Close();
                return rows;
            }
            catch (Exception ex)
            {
                throw new Exception("SPX_PARAM_MARQUECOMPTEUR_RETOURNE" + ":" + ex.Message);
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                    cn.Close(); // Fermeture de la connection 
                cmd.Dispose();
            }
        }

        public bool Delete(CsMarqueCompteur pMarqueCompteur)
        {
            try
            {
                cn = new SqlConnection(ConnectionString);
                cmd = new SqlCommand
                {
                    Connection = cn,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "SPX_PARAM_MARQUECOMPTEUR_SUPPRIMER"
                };
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@CODE", pMarqueCompteur.CODE);
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
                throw new Exception("SPX_PARAM_MARQUECOMPTEUR_SUPPRIMER" + ":" + ex.Message);
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                cn.Close();
                cmd.Dispose();
            }
        }

        public bool Delete(List<CsMarqueCompteur> pCategorieBranchemenCollection)
        {
            int number = 0;
            foreach (CsMarqueCompteur entity in pCategorieBranchemenCollection)
            {
                if (Delete(entity))
                {
                    number++;
                }
            }
            return number != 0;
        }

        public static List<CsMarqueCompteur> Fill(IDataReader reader, List<CsMarqueCompteur> rows, int start, int pageLength)
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

                var c = new CsMarqueCompteur();
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

        public bool Update(CsMarqueCompteur pMarqueCompteur)
        {
                cn = new SqlConnection(ConnectionString);
                cmd = new SqlCommand
                          {
                              Connection = cn,
                              CommandType = CommandType.StoredProcedure,
                              CommandText = "SPX_PARAM_MARQUECOMPTEUR_UPDATE"
                          };
                cmd.Parameters.Clear();

                try
                {
                    cmd.Parameters.AddWithValue("@CODE", pMarqueCompteur.CODE);
                    cmd.Parameters.AddWithValue("@LIBELLE", pMarqueCompteur.LIBELLE);
                    cmd.Parameters.AddWithValue("@OriginalCODE", pMarqueCompteur.OriginalCODE);
                    cmd.Parameters.AddWithValue("@DATECREATION", pMarqueCompteur.DATECREATION);
                    cmd.Parameters.AddWithValue("@DATEMODIFICATION", pMarqueCompteur.DATEMODIFICATION);
                    cmd.Parameters.AddWithValue("@USERCREATION", pMarqueCompteur.USERCREATION);
                    cmd.Parameters.AddWithValue("@USERMODIFICATION", pMarqueCompteur.USERMODIFICATION);
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

        public bool Update(List<CsMarqueCompteur> pMarqueCompteurCollection)
        {
            int number = 0;
            foreach (CsMarqueCompteur entity in pMarqueCompteurCollection)
            {
                if (Update(entity))
                {
                    number++;
                }
            }
            return number != 0;
        }

        public bool Insert(CsMarqueCompteur pMarqueCompteur)
        {
                cn = new SqlConnection(ConnectionString);
                cmd = new SqlCommand
                {
                    Connection = cn,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "SPX_PARAM_MARQUECOMPTEUR_INSERER"
                };
                cmd.Parameters.Clear();

                try
                {
                    cmd.Parameters.AddWithValue("@CODE", pMarqueCompteur.CODE);
                    cmd.Parameters.AddWithValue("@LIBELLE", pMarqueCompteur.LIBELLE);
                    cmd.Parameters.AddWithValue("@DATECREATION", pMarqueCompteur.DATECREATION);
                    cmd.Parameters.AddWithValue("@DATEMODIFICATION", pMarqueCompteur.DATEMODIFICATION);
                    cmd.Parameters.AddWithValue("@USERCREATION", pMarqueCompteur.USERCREATION);
                    cmd.Parameters.AddWithValue("@USERMODIFICATION", pMarqueCompteur.USERMODIFICATION);
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

        public bool Insert(List<CsMarqueCompteur> pMarqueCompteurCollection)
        {
            int number = 0;
            foreach (CsMarqueCompteur entity in pMarqueCompteurCollection)
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

        public List<CsMarqueCompteur> SelectAllMarqueCompteur()
        {
            try
            {
                return Entities.GetEntityListFromQuery<CsMarqueCompteur>(ParamProcedure.PARAM_MARQUECOMPTEUR_RETOURNE());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Delete(CsMarqueCompteur pMarqueCompteur)
        {
            try
            {
                return Entities.DeleteEntity<Galatee.Entity.Model.MARQUECOMPTEUR>(Entities.ConvertObject<Galatee.Entity.Model.MARQUECOMPTEUR, CsMarqueCompteur>(pMarqueCompteur));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Delete(List<CsMarqueCompteur> pMarqueCompteurCollection)
        {
            try
            {
                return Entities.DeleteEntity<Galatee.Entity.Model.MARQUECOMPTEUR>(Entities.ConvertObject<Galatee.Entity.Model.MARQUECOMPTEUR, CsMarqueCompteur>(pMarqueCompteurCollection));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Update(CsMarqueCompteur pMarqueCompteur)
        {
            try
            {
                return Entities.UpdateEntity<Galatee.Entity.Model.MARQUECOMPTEUR>(Entities.ConvertObject<Galatee.Entity.Model.MARQUECOMPTEUR, CsMarqueCompteur>(pMarqueCompteur));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Update(List<CsMarqueCompteur> pMarqueCompteurCollection)
        {
            try
            {
                return Entities.UpdateEntity<Galatee.Entity.Model.MARQUECOMPTEUR>(Entities.ConvertObject<Galatee.Entity.Model.MARQUECOMPTEUR, CsMarqueCompteur>(pMarqueCompteurCollection));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Insert(CsMarqueCompteur pMarqueCompteur)
        {
            try
            {
                return Entities.InsertEntity<Galatee.Entity.Model.MARQUECOMPTEUR>(Entities.ConvertObject<Galatee.Entity.Model.MARQUECOMPTEUR, CsMarqueCompteur>(pMarqueCompteur));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Insert(List<CsMarqueCompteur> pMarqueCompteurCollection)
        {
            try
            {
                return Entities.InsertEntity<Galatee.Entity.Model.MARQUECOMPTEUR>(Entities.ConvertObject<Galatee.Entity.Model.MARQUECOMPTEUR, CsMarqueCompteur>(pMarqueCompteurCollection));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
