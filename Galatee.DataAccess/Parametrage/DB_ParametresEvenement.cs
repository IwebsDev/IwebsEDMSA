using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Galatee.Structure;
using Galatee.Entity.Model;

namespace Galatee.DataAccess
{
    public class DB_ParametreEvenement : Galatee.DataAccess.Parametrage.DbBase
    {
        /*
        private string ConnectionString;

        public DB_ParametreEvenement()
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

        public DB_ParametreEvenement(string ConnStr)
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

        public List<CsParametreEvenement> SelectAllParametresEvenement()
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
                              CommandText = EnumProcedureStockee.SelectPARAMETREEVENEMENT
                          };
                IDataReader reader = cmd.ExecuteReader();
                var rows = new List<CsParametreEvenement>();
                Fill(reader, rows, int.MinValue, int.MaxValue);
                reader.Close();
                return rows;
            }
            catch (Exception ex)
            {
                throw new Exception(EnumProcedureStockee.SelectPARAMETRESGENRAUX + ":" + ex.Message);
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                    cn.Close(); // Fermeture de la connection 
                cmd.Dispose();
            }
        }

        public bool Delete(CsParametreEvenement pParametreEvenement)
        {
            try
            {
                cn = new SqlConnection(ConnectionString);
                cmd = new SqlCommand
                {
                    Connection = cn,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = EnumProcedureStockee.DeletePARAMETREEVENEMENT
                };
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@CODE", pParametreEvenement.CODE);
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
                throw new Exception(EnumProcedureStockee.DeletePARAMETREEVENEMENT + ":" + ex.Message);
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                cn.Close();
                cmd.Dispose();
            }
        }

        public bool Delete(List<CsParametreEvenement> pParametreEvenementCollection)
        {
            int number = 0;
            foreach (CsParametreEvenement entity in pParametreEvenementCollection)
            {
                if (Delete(entity))
                {
                    number++;
                }
            }
            return number != 0;
        }

        public static List<CsParametreEvenement> Fill(IDataReader reader, List<CsParametreEvenement> rows, int start, int pageLength)
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

                var c = new CsParametreEvenement();
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

        public bool Update(CsParametreEvenement pParametreEvenement)
        {
                cn = new SqlConnection(ConnectionString);
                cmd = new SqlCommand
                          {
                              Connection = cn,
                              CommandType = CommandType.StoredProcedure,
                              CommandText = EnumProcedureStockee.UpdatePARAMETREEVENEMENT
                          };
                cmd.Parameters.Clear();

                try
                {
                    cmd.Parameters.AddWithValue("@CODE", pParametreEvenement.CODE);
                    cmd.Parameters.AddWithValue("@LIBELLE", pParametreEvenement.LIBELLE);
                    cmd.Parameters.AddWithValue("@OriginalCODE", pParametreEvenement.OriginalCODE);
                    cmd.Parameters.AddWithValue("@DATECREATION", pParametreEvenement.DATECREATION);
                    cmd.Parameters.AddWithValue("@DATEMODIFICATION", pParametreEvenement.DATEMODIFICATION);
                    cmd.Parameters.AddWithValue("@USERCREATION", pParametreEvenement.USERCREATION);
                    cmd.Parameters.AddWithValue("@USERMODIFICATION", pParametreEvenement.USERMODIFICATION);
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

        public bool Update(List<CsParametreEvenement> pParametreEvenementCollection)
        {
            int number = 0;
            foreach (CsParametreEvenement entity in pParametreEvenementCollection)
            {
                if (Update(entity))
                {
                    number++;
                }
            }
            return number != 0;
        }

        public bool Insert(CsParametreEvenement pParametreEvenement)
        {
                cn = new SqlConnection(ConnectionString);
                cmd = new SqlCommand
                {
                    Connection = cn,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = EnumProcedureStockee.InsertPARAMETREEVENEMENT
                };
                cmd.Parameters.Clear();

                try
                {
                    cmd.Parameters.AddWithValue("@CODE", pParametreEvenement.CODE);
                    cmd.Parameters.AddWithValue("@LIBELLE", pParametreEvenement.LIBELLE);
                    cmd.Parameters.AddWithValue("@DATECREATION", pParametreEvenement.DATECREATION);
                    cmd.Parameters.AddWithValue("@DATEMODIFICATION", pParametreEvenement.DATEMODIFICATION);
                    cmd.Parameters.AddWithValue("@USERCREATION", pParametreEvenement.USERCREATION);
                    cmd.Parameters.AddWithValue("@USERMODIFICATION", pParametreEvenement.USERMODIFICATION);
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

        public bool Insert(List<CsParametreEvenement> pParametreEvenementCollection)
        {
            int number = 0;
            foreach (CsParametreEvenement entity in pParametreEvenementCollection)
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


        public List<CsParametreEvenement> SelectAllParametresEvenement()
        {
            try
            {
                return Entities.GetEntityListFromQuery<CsParametreEvenement>(ParamProcedure.PARAM_PARAMETREEVENEMENT_RETOURNE());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Delete(CsParametreEvenement pParametreEvenement)
        {
            try
            {
                return Entities.DeleteEntity<Galatee.Entity.Model.PARAMETREEVENEMENT>(Entities.ConvertObject<Galatee.Entity.Model.PARAMETREEVENEMENT, CsParametreEvenement>(pParametreEvenement));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Delete(List<CsParametreEvenement> pParametreEvenementCollection)
        {
            try
            {
                return Entities.DeleteEntity<Galatee.Entity.Model.PARAMETREEVENEMENT>(Entities.ConvertObject<Galatee.Entity.Model.PARAMETREEVENEMENT, CsParametreEvenement>(pParametreEvenementCollection));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Update(CsParametreEvenement pParametreEvenement)
        {
            try
            {
                return Entities.UpdateEntity<Galatee.Entity.Model.PARAMETREEVENEMENT>(Entities.ConvertObject<Galatee.Entity.Model.PARAMETREEVENEMENT, CsParametreEvenement>(pParametreEvenement));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Update(List<CsParametreEvenement> pParametreEvenementCollection)
        {
            try
            {
                return Entities.UpdateEntity<Galatee.Entity.Model.PARAMETREEVENEMENT>(Entities.ConvertObject<Galatee.Entity.Model.PARAMETREEVENEMENT, CsParametreEvenement>(pParametreEvenementCollection));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Insert(CsParametreEvenement pParametreEvenement)
        {
            try
            {
                return Entities.InsertEntity<Galatee.Entity.Model.PARAMETREEVENEMENT>(Entities.ConvertObject<Galatee.Entity.Model.PARAMETREEVENEMENT, CsParametreEvenement>(pParametreEvenement));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Insert(List<CsParametreEvenement> pParametreEvenementCollection)
        {
            try
            {
                return Entities.InsertEntity<Galatee.Entity.Model.PARAMETREEVENEMENT>(Entities.ConvertObject<Galatee.Entity.Model.PARAMETREEVENEMENT, CsParametreEvenement>(pParametreEvenementCollection));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
