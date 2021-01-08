using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Galatee.Structure;
using Galatee.Entity.Model;

namespace Galatee.DataAccess
{
    public class DB_CategorieBranchement : Galatee.DataAccess.Parametrage.DbBase
    {
        /*
        private string ConnectionString;

        public DB_CategorieBranchement()
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

        public DB_CategorieBranchement(string ConnStr)
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

        public List<CsCategorieBranchement> SelectAllCategorieBranchement()
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
                              CommandText = EnumProcedureStockee.SelectCATEGORIEBRANCHEMENT
                          };
                IDataReader reader = cmd.ExecuteReader();
                var rows = new List<CsCategorieBranchement>();
                Fill(reader, rows, int.MinValue, int.MaxValue);
                reader.Close();
                return rows;
            }
            catch (Exception ex)
            {
                throw new Exception(EnumProcedureStockee.SelectCATEGORIEBRANCHEMENT + ":" + ex.Message);
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                    cn.Close(); // Fermeture de la connection 
                cmd.Dispose();
            }
        }

        public bool Delete(CsCategorieBranchement pCategorieBranchement)
        {
            try
            {
                cn = new SqlConnection(ConnectionString);
                cmd = new SqlCommand
                {
                    Connection = cn,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = EnumProcedureStockee.DeleteCATEGORIEBRANCHEMENT
                };
                cmd.Parameters.Clear();
                //cmd.Parameters.AddWithValue("@CODE", pCategorieBranchement.CODE);
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
                throw new Exception(EnumProcedureStockee.DeleteCATEGORIEBRANCHEMENT+ ":" + ex.Message);
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                cn.Close();
                cmd.Dispose();
            }
        }

        public bool Delete(List<CsCategorieBranchement> pCategorieBranchemenCollection)
        {
            int number = 0;
            foreach (CsCategorieBranchement entity in pCategorieBranchemenCollection)
            {
                if (Delete(entity))
                {
                    number++;
                }
            }
            return number != 0;
        }

        public static List<CsCategorieBranchement> Fill(IDataReader reader, List<CsCategorieBranchement> rows, int start, int pageLength)
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

                var c = new CsCategorieBranchement();
                //c.CODE = (Convert.IsDBNull(reader["CODE"]))?string.Empty:(System.String)reader["CODE"];
                //c.OriginalCODE = (Convert.IsDBNull(reader["CODE"]))?string.Empty:(System.String)reader["CODE"];
                c.LIBELLE = (Convert.IsDBNull(reader["LIBELLE"])) ? string.Empty : (System.String)reader["LIBELLE"];
                c.USERCREATION = (Convert.IsDBNull(reader["USERCREATION"])) ? string.Empty : (System.String)reader["USERCREATION"];
                c.USERMODIFICATION = (Convert.IsDBNull(reader["USERMODIFICATION"])) ? string.Empty : (System.String)reader["USERMODIFICATION"];
                c.DATECREATION = (Convert.IsDBNull(reader["DATECREATION"])) ? (DateTime?)null : (System.DateTime)reader["DATECREATION"];
                c.DATEMODIFICATION = (Convert.IsDBNull(reader["DATEMODIFICATION"])) ? (DateTime?)null : (System.DateTime)reader["DATEMODIFICATION"];
				rows.Add(c);
			}
			return rows;
		}

        public bool Update(CsCategorieBranchement pCategorieBranchement)
        {
                cn = new SqlConnection(ConnectionString);
                cmd = new SqlCommand
                          {
                              Connection = cn,
                              CommandType = CommandType.StoredProcedure,
                              CommandText = EnumProcedureStockee.UpdateCATEGORIEBRANCHEMENT
                          };
                cmd.Parameters.Clear();

                try
                {
                    //cmd.Parameters.AddWithValue("@CODE", pCategorieBranchement.CODE);
                    cmd.Parameters.AddWithValue("@LIBELLE", pCategorieBranchement.LIBELLE);
                    //cmd.Parameters.AddWithValue("@OriginalCODE", pCategorieBranchement.OriginalCODE);
                    cmd.Parameters.AddWithValue("@DATECREATION", pCategorieBranchement.DATECREATION);
                    cmd.Parameters.AddWithValue("@DATEMODIFICATION", pCategorieBranchement.DATEMODIFICATION);
                    cmd.Parameters.AddWithValue("@USERCREATION", pCategorieBranchement.USERCREATION);
                    cmd.Parameters.AddWithValue("@USERMODIFICATION", pCategorieBranchement.USERMODIFICATION);
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

        public bool Update(List<CsCategorieBranchement> pCategorieBranchementCollection)
        {
            int number = 0;
            foreach (CsCategorieBranchement entity in pCategorieBranchementCollection)
            {
                if (Update(entity))
                {
                    number++;
                }
            }
            return number != 0;
        }

        public bool Insert(CsCategorieBranchement pCategorieBranchement)
        {
                cn = new SqlConnection(ConnectionString);
                cmd = new SqlCommand
                {
                    Connection = cn,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = EnumProcedureStockee.InsertCATEGORIEBRANCHEMENT
                };
                cmd.Parameters.Clear();

                try
                {
                    //cmd.Parameters.AddWithValue("@CODE", pCategorieBranchement.CODE);
                    cmd.Parameters.AddWithValue("@LIBELLE", pCategorieBranchement.LIBELLE);
                    cmd.Parameters.AddWithValue("@DATECREATION", pCategorieBranchement.DATECREATION);
                    cmd.Parameters.AddWithValue("@DATEMODIFICATION", pCategorieBranchement.DATEMODIFICATION);
                    cmd.Parameters.AddWithValue("@USERCREATION", pCategorieBranchement.USERCREATION);
                    cmd.Parameters.AddWithValue("@USERMODIFICATION", pCategorieBranchement.USERMODIFICATION);
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

        public bool Insert(List<CsCategorieBranchement> pCategorieBranchementCollection)
        {
            int number = 0;
            foreach (CsCategorieBranchement entity in pCategorieBranchementCollection)
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

        public List<CsTypeBranchement > SelectAllTypeBranchement()
        {
            try
            {
                return Entities.GetEntityListFromQuery<CsTypeBranchement>(ParamProcedure.PARAM_TYPEBRANCHEMENT_RETOURNE());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Delete(CsTypeBranchement pCategorieBranchement)
        {
            try
            {
                return Entities.DeleteEntity<Galatee.Entity.Model.TYPEBRANCHEMENT>(Entities.ConvertObject<Galatee.Entity.Model.TYPEBRANCHEMENT, CsTypeBranchement>(pCategorieBranchement));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Delete(List<CsTypeBranchement> pCategorieBranchemenCollection)
        {
            try
            {
                return Entities.DeleteEntity<Galatee.Entity.Model.TYPEBRANCHEMENT>(Entities.ConvertObject<Galatee.Entity.Model.TYPEBRANCHEMENT, CsTypeBranchement>(pCategorieBranchemenCollection));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public bool Update(CsTypeBranchement pCategorieBranchement)
        {
            try
            {
                return Entities.UpdateEntity<Galatee.Entity.Model.TYPEBRANCHEMENT>(Entities.ConvertObject<Galatee.Entity.Model.TYPEBRANCHEMENT, CsTypeBranchement>(pCategorieBranchement));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Update(List<CsTypeBranchement> pCategorieBranchementCollection)
        {
            try
            {
                return Entities.UpdateEntity<Galatee.Entity.Model.TYPEBRANCHEMENT>(Entities.ConvertObject<Galatee.Entity.Model.TYPEBRANCHEMENT, CsTypeBranchement>(pCategorieBranchementCollection));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Insert(CsTypeBranchement pCategorieBranchement)
        {
            try
            {
                return Entities.InsertEntity<Galatee.Entity.Model.TYPEBRANCHEMENT>(Entities.ConvertObject<Galatee.Entity.Model.TYPEBRANCHEMENT, CsTypeBranchement>(pCategorieBranchement));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Insert(List<CsTypeBranchement> pCategorieBranchementCollection)
        {
            try
            {
                return Entities.InsertEntity<Galatee.Entity.Model.TYPEBRANCHEMENT>(Entities.ConvertObject<Galatee.Entity.Model.TYPEBRANCHEMENT, CsTypeBranchement>(pCategorieBranchementCollection));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
