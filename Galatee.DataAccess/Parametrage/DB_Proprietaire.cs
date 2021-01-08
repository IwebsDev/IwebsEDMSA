using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Galatee.Structure;
using Galatee.Entity.Model;

namespace Galatee.DataAccess
{
    public class DB_Proprietaire : Galatee.DataAccess.Parametrage.DbBase
    {
        /*
        private string ConnectionString;

        public DB_Proprietaire()
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

        public DB_Proprietaire(string ConnStr)
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

        public List<CsProprietaire> SelectAllProprietaire()
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
                              CommandText = EnumProcedureStockee.SelectPROPRIETAIRE
                          };
                IDataReader reader = cmd.ExecuteReader();
                var rows = new List<CsProprietaire>();
                Fill(reader, rows, int.MinValue, int.MaxValue);
                reader.Close();
                return rows;
            }
            catch (Exception ex)
            {
                throw new Exception(EnumProcedureStockee.SelectPROPRIETAIRE + ":" + ex.Message);
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                    cn.Close(); // Fermeture de la connection 
                cmd.Dispose();
            }
        }

        public bool Delete(CsProprietaire pProprietaire)
        {
            try
            {
                cn = new SqlConnection(ConnectionString);
                cmd = new SqlCommand
                {
                    Connection = cn,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = EnumProcedureStockee.DeletePROPRIETAIRE
                };
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@CODE", pProprietaire.CODE);
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
                throw new Exception(EnumProcedureStockee.DeletePROPRIETAIRE + ":" + ex.Message);
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                cn.Close();
                cmd.Dispose();
            }
        }

        public bool Delete(List<CsProprietaire> pProprietairesCollection)
        {
            int number = 0;
            foreach (CsProprietaire entity in pProprietairesCollection)
            {
                if (Delete(entity))
                {
                    number++;
                }
            }
            return number != 0;
        }

        public static List<CsProprietaire> Fill(IDataReader reader, List<CsProprietaire> rows, int start, int pageLength)
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

                var c = new CsProprietaire();
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

        public bool Update(CsProprietaire pProprietaire)
        {
                cn = new SqlConnection(ConnectionString);
                cmd = new SqlCommand
                          {
                              Connection = cn,
                              CommandType = CommandType.StoredProcedure,
                              CommandText = EnumProcedureStockee.UpdatePROPRIETAIRE
                          };
                cmd.Parameters.Clear();

                try
                {
                    cmd.Parameters.AddWithValue("@CODE", pProprietaire.CODE);
                    cmd.Parameters.AddWithValue("@LIBELLE", pProprietaire.LIBELLE);
                    cmd.Parameters.AddWithValue("@OriginalCODE", pProprietaire.OriginalCODE);
                    cmd.Parameters.AddWithValue("@DATECREATION", pProprietaire.DATECREATION);
                    cmd.Parameters.AddWithValue("@DATEMODIFICATION", pProprietaire.DATEMODIFICATION);
                    cmd.Parameters.AddWithValue("@USERCREATION", pProprietaire.USERCREATION);
                    cmd.Parameters.AddWithValue("@USERMODIFICATION", pProprietaire.USERMODIFICATION);
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

        public bool Update(List<CsProprietaire> pProprietaireCollection)
        {
            int number = 0;
            foreach (CsProprietaire entity in pProprietaireCollection)
            {
                if (Update(entity))
                {
                    number++;
                }
            }
            return number != 0;
        }

        public bool Insert(CsProprietaire pProprietaire)
        {
                cn = new SqlConnection(ConnectionString);
                cmd = new SqlCommand
                {
                    Connection = cn,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = EnumProcedureStockee.InsertPROPRIETAIRE
                };
                cmd.Parameters.Clear();

                try
                {
                    cmd.Parameters.AddWithValue("@CODE", pProprietaire.CODE);
                    cmd.Parameters.AddWithValue("@LIBELLE", pProprietaire.LIBELLE);
                    cmd.Parameters.AddWithValue("@DATECREATION", pProprietaire.DATECREATION);
                    cmd.Parameters.AddWithValue("@DATEMODIFICATION", pProprietaire.DATEMODIFICATION);
                    cmd.Parameters.AddWithValue("@USERCREATION", pProprietaire.USERCREATION);
                    cmd.Parameters.AddWithValue("@USERMODIFICATION", pProprietaire.USERMODIFICATION);
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

        public bool Insert(List<CsProprietaire> pProprietaireCollection)
        {
            int number = 0;
            foreach (CsProprietaire entity in pProprietaireCollection)
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

        public List<CsProprietaire> SelectAllProprietaire()
        {
            try
            {
                return Entities.GetEntityListFromQuery<CsProprietaire>(ParamProcedure.PARAM_PROPRIETAIRE_RETOURNE());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Delete(CsProprietaire pProprietaire)
        {
            try
            {
                var prop=(Entities.ConvertObject<Galatee.Entity.Model.PROPRIETAIRE, CsProprietaire>(pProprietaire));
                return Entities.DeleteEntity<Galatee.Entity.Model.PROPRIETAIRE>(prop);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Delete(List<CsProprietaire> pProprietaireCollection)
        {
            try
            {
                return Entities.DeleteEntity<Galatee.Entity.Model.PROPRIETAIRE>(Entities.ConvertObject<Galatee.Entity.Model.PROPRIETAIRE, CsProprietaire>(pProprietaireCollection));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool Update(CsProprietaire pProprietaire)
        {
            try
            {
                return Entities.UpdateEntity<Galatee.Entity.Model.PROPRIETAIRE>(Entities.ConvertObject<Galatee.Entity.Model.PROPRIETAIRE, CsProprietaire>(pProprietaire));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Update(List<CsProprietaire> pProprietaireCollection)
        {
            try
            {
                return Entities.UpdateEntity<Galatee.Entity.Model.PROPRIETAIRE>(Entities.ConvertObject<Galatee.Entity.Model.PROPRIETAIRE, CsProprietaire>(pProprietaireCollection));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool Insert(CsProprietaire pProprietaire)
        {
            try
            {
                return Entities.InsertEntity<Galatee.Entity.Model.PROPRIETAIRE>(Entities.ConvertObject<Galatee.Entity.Model.PROPRIETAIRE, CsProprietaire>(pProprietaire));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Insert(List<CsProprietaire> pProprietaireCollection)
        {
            try
            {
                return Entities.InsertEntity<Galatee.Entity.Model.PROPRIETAIRE>(Entities.ConvertObject<Galatee.Entity.Model.PROPRIETAIRE, CsProprietaire>(pProprietaireCollection));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
