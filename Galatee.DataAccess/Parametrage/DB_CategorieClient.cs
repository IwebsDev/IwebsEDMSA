using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Galatee.Structure;
using Galatee.Entity.Model;

namespace Galatee.DataAccess
{
    public class DB_CategorieClient : Galatee.DataAccess.Parametrage.DbBase
    {
/*
        private string ConnectionString;

        public DB_CategorieClient()
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

        public DB_CategorieClient(string ConnStr)
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

        public List<CsCategorieClient> SelectAllCategorieClient()
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
                              CommandText = EnumProcedureStockee.SelectCATEGORIECLIENT
                          };
                IDataReader reader = cmd.ExecuteReader();
                var rows = new List<CsCategorieClient>();
                Fill(reader, rows, int.MinValue, int.MaxValue);
                reader.Close();
                return rows;
            }
            catch (Exception ex)
            {
                throw new Exception(EnumProcedureStockee.SelectCATEGORIECLIENT + ":" + ex.Message);
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                    cn.Close(); // Fermeture de la connection 
                cmd.Dispose();
            }
        }

        public CsCategorieClient SelectCategorieClientByCode(string pCategorieClient)
        {
            try
            {
                cn = new SqlConnection(ConnectionString);
                cmd = new SqlCommand
                {
                    Connection = cn,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = EnumProcedureStockee.SelectCATEGORIECLIENTByKey
                };
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@CODE", pCategorieClient);
                if (cn.State == ConnectionState.Closed)
                    cn.Open();
                IDataReader reader = cmd.ExecuteReader();
                var rows = new List<CsCategorieClient>();
                Fill(reader, rows, int.MinValue, int.MaxValue);
                reader.Close();
                if (rows.Count == 0)
                    return null;
                else
                    return rows[0];
            }
            catch (Exception ex)
            {
                RollBackTransaction(cmd.Transaction);
                throw new Exception(EnumProcedureStockee.SelectCATEGORIECLIENTByKey + ":" + ex.Message);
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                    cn.Close();
                cmd.Dispose();
            }
        }

        public bool Delete(CsCategorieClient pCategorieClient)
        {
            try
            {
                cn = new SqlConnection(ConnectionString);
                cmd = new SqlCommand
                {
                    Connection = cn,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = EnumProcedureStockee.DeleteCATEGORIECLIENT
                };
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@CODE", pCategorieClient.CODE);
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
                throw new Exception(EnumProcedureStockee.DeleteCATEGORIECLIENT + ":" + ex.Message);
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                cn.Close();
                cmd.Dispose();
            }
        }

        public bool Delete(List<CsCategorieClient> pCategorieClientCollection)
        {
            int number = 0;
            foreach (CsCategorieClient entity in pCategorieClientCollection)
            {
                if (Delete(entity))
                {
                    number++;
                }
            }
            return number != 0;
        }

        public static List<CsCategorieClient> Fill(IDataReader reader, List<CsCategorieClient> rows, int start, int pageLength)
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

                var c = new CsCategorieClient();
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

        public bool Update(CsCategorieClient pCategorieClient)
        {
                cn = new SqlConnection(ConnectionString);
                cmd = new SqlCommand
                          {
                              Connection = cn,
                              CommandType = CommandType.StoredProcedure,
                              CommandText = EnumProcedureStockee.UpdateCATEGORIECLIENT
                          };
                cmd.Parameters.Clear();

                try
                {
                    cmd.Parameters.AddWithValue("@CODE", pCategorieClient.CODE);
                    cmd.Parameters.AddWithValue("@LIBELLE", pCategorieClient.LIBELLE);
                    cmd.Parameters.AddWithValue("@OriginalCODE", pCategorieClient.OriginalCODE);
                    cmd.Parameters.AddWithValue("@DATECREATION", pCategorieClient.DATECREATION);
                    cmd.Parameters.AddWithValue("@DATEMODIFICATION", pCategorieClient.DATEMODIFICATION);
                    cmd.Parameters.AddWithValue("@USERCREATION", pCategorieClient.USERCREATION);
                    cmd.Parameters.AddWithValue("@USERMODIFICATION", pCategorieClient.USERMODIFICATION);
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

        public bool Update(List<CsCategorieClient> pCategorieClientCollection)
        {
            int number = 0;
            foreach (CsCategorieClient entity in pCategorieClientCollection)
            {
                if (Update(entity))
                {
                    number++;
                }
            }
            return number != 0;
        }

        public bool Insert(CsCategorieClient pCategorieClient)
        {
                cn = new SqlConnection(ConnectionString);
                cmd = new SqlCommand
                {
                    Connection = cn,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = EnumProcedureStockee.InsertCATEGORIECLIENT
                };
                cmd.Parameters.Clear();

                try
                {
                    cmd.Parameters.AddWithValue("@CODE", pCategorieClient.CODE);
                    cmd.Parameters.AddWithValue("@LIBELLE", pCategorieClient.LIBELLE);
                    cmd.Parameters.AddWithValue("@DATECREATION", pCategorieClient.DATECREATION);
                    cmd.Parameters.AddWithValue("@DATEMODIFICATION", pCategorieClient.DATEMODIFICATION);
                    cmd.Parameters.AddWithValue("@USERCREATION", pCategorieClient.USERCREATION);
                    cmd.Parameters.AddWithValue("@USERMODIFICATION", pCategorieClient.USERMODIFICATION);
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

        public bool Insert(List<CsCategorieClient> pCategorieClientCollection)
        {
            int number = 0;
            foreach (CsCategorieClient entity in pCategorieClientCollection)
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

        public List<CsCategorieClient> SelectAllCategorieClient()
        {
            try
            {
                return Entities.GetEntityListFromQuery<CsCategorieClient>(CommonProcedures.RetourneCategorieClient());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public CsCategorieClient SelectCategorieClientByCode(int pCategorieClientId)
        {
            try
            {
                return Entities.GetEntityFromQuery<CsCategorieClient>(ParamProcedure.PARAM_CATEGORIECLIENT_RETOURNEById(pCategorieClientId));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Delete(CsCategorieClient pCategorieClient)
        {
            try
            {

                return Entities.DeleteEntity<Galatee.Entity.Model.CATEGORIECLIENT>(Entities.ConvertObject<Galatee.Entity.Model.CATEGORIECLIENT, CsCategorieClient>(pCategorieClient));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Delete(List<CsCategorieClient> pCategorieClientCollection)
        {
            try
            {
                return Entities.DeleteEntity<Galatee.Entity.Model.CATEGORIECLIENT>(Entities.ConvertObject<Galatee.Entity.Model.CATEGORIECLIENT, CsCategorieClient>(pCategorieClientCollection));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Update(CsCategorieClient pCategorieClient)
        {
            try
            {
                return Entities.UpdateEntity<Galatee.Entity.Model.CATEGORIECLIENT>(Entities.ConvertObject<Galatee.Entity.Model.CATEGORIECLIENT, CsCategorieClient>(pCategorieClient));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Update(List<CsCategorieClient> pCategorieClientCollection)
        {
            try
            {
                return Entities.UpdateEntity<Galatee.Entity.Model.CATEGORIECLIENT>(Entities.ConvertObject<Galatee.Entity.Model.CATEGORIECLIENT, CsCategorieClient>(pCategorieClientCollection));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Insert(CsCategorieClient pCategorieClient)
        {
            try
            {
                return Entities.InsertEntity<Galatee.Entity.Model.CATEGORIECLIENT>(Entities.ConvertObject<Galatee.Entity.Model.CATEGORIECLIENT, CsCategorieClient>(pCategorieClient));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Insert(List<CsCategorieClient> pCategorieClientCollection)
        {
            try
            {
                return Entities.InsertEntity<Galatee.Entity.Model.CATEGORIECLIENT>(Entities.ConvertObject<Galatee.Entity.Model.CATEGORIECLIENT, CsCategorieClient>(pCategorieClientCollection));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
