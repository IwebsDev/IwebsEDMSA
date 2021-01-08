using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Galatee.Structure;
using Galatee.Entity.Model;

namespace Galatee.DataAccess
{
    public class DB_TypeCentre /*: Galatee.DataAccess.Parametrage.DbBase*/
    {
        /*
        private string ConnectionString;

        public DB_TypeCentre()
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

        public DB_TypeCentre(string ConnStr)
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

        public List<CsTypeCentre> SelectAllTypeCentre()
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
                              CommandText = EnumProcedureStockee.SelectTYPECENTRE
                          };
                IDataReader reader = cmd.ExecuteReader();
                var rows = new List<CsTypeCentre>();
                Fill(reader, rows, int.MinValue, int.MaxValue);
                reader.Close();
                return rows;
            }
            catch (Exception ex)
            {
                throw new Exception(EnumProcedureStockee.SelectTYPECENTRE + ":" + ex.Message);
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                    cn.Close(); // Fermeture de la connection 
                cmd.Dispose();
            }
        }

        public bool Delete(CsTypeCentre pTypeCentre)
        {
            try
            {
                cn = new SqlConnection(ConnectionString);
                cmd = new SqlCommand
                {
                    Connection = cn,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = EnumProcedureStockee.DeleteTYPECENTRE
                };
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@CODETYPE", pTypeCentre.PK_CODETYPE);
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
                throw new Exception(EnumProcedureStockee.DeleteTYPECENTRE + ":" + ex.Message);
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                cn.Close();
                cmd.Dispose();
            }
        }

        public bool Delete(List<CsTypeCentre> pTypeCentreCollection)
        {
            int number = 0;
            foreach (CsTypeCentre entity in pTypeCentreCollection)
            {
                if (Delete(entity))
                {
                    number++;
                }
            }
            return number != 0;
        }

        public static List<CsTypeCentre> Fill(IDataReader reader, List<CsTypeCentre> rows, int start, int pageLength)
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

                var c = new CsTypeCentre();
                c.PK_CODETYPE = (Convert.IsDBNull(reader["CODETYPE"])) ? string.Empty : (System.String)reader["CODETYPE"];
                c.OriginalCODETYPE = (Convert.IsDBNull(reader["CODETYPE"])) ? string.Empty : (System.String)reader["CODETYPE"];
                c.LIBELLE = (Convert.IsDBNull(reader["LIBELLE"])) ? string.Empty : (System.String)reader["LIBELLE"];
                c.USERCREATION = (Convert.IsDBNull(reader["USERCREATION"])) ? string.Empty : (System.String)reader["USERCREATION"];
                c.USERMODIFICATION = (Convert.IsDBNull(reader["USERMODIFICATION"])) ? string.Empty : (System.String)reader["USERMODIFICATION"];
                c.DATECREATION = (Convert.IsDBNull(reader["DATECREATION"])) ? (DateTime?)null : (System.DateTime)reader["DATECREATION"];
                c.DATEMODIFICATION = (Convert.IsDBNull(reader["DATEMODIFICATION"])) ? (DateTime?)null : (System.DateTime)reader["DATEMODIFICATION"];
				rows.Add(c);
			}
			return rows;
		}

        public bool Update(CsTypeCentre pTypeCentre)
        {
                cn = new SqlConnection(ConnectionString);
                cmd = new SqlCommand
                          {
                              Connection = cn,
                              CommandType = CommandType.StoredProcedure,
                              CommandText = EnumProcedureStockee.UpdateTYPECENTRE
                          };
                cmd.Parameters.Clear();

                try
                {
                    cmd.Parameters.AddWithValue("@CODETYPE", pTypeCentre.PK_CODETYPE);
                    cmd.Parameters.AddWithValue("@LIBELLE", pTypeCentre.LIBELLE);
                    cmd.Parameters.AddWithValue("@OriginalCODETYPE", pTypeCentre.OriginalCODETYPE);
                    cmd.Parameters.AddWithValue("@DATECREATION", pTypeCentre.DATECREATION);
                    cmd.Parameters.AddWithValue("@DATEMODIFICATION", pTypeCentre.DATEMODIFICATION);
                    cmd.Parameters.AddWithValue("@USERCREATION", pTypeCentre.USERCREATION);
                    cmd.Parameters.AddWithValue("@USERMODIFICATION", pTypeCentre.USERMODIFICATION);
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

        public bool Update(List<CsTypeCentre> pCsTypeCentreCollection)
        {
            int number = 0;
            foreach (CsTypeCentre entity in pCsTypeCentreCollection)
            {
                if (Update(entity))
                {
                    number++;
                }
            }
            return number != 0;
        }

        public bool Insert(CsTypeCentre pTypeCentre)
        {
                cn = new SqlConnection(ConnectionString);
                cmd = new SqlCommand
                {
                    Connection = cn,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = EnumProcedureStockee.InsertTYPECENTRE
                };
                cmd.Parameters.Clear();

                try
                {
                    cmd.Parameters.AddWithValue("@CODETYPE", pTypeCentre.PK_CODETYPE);
                    cmd.Parameters.AddWithValue("@LIBELLE", pTypeCentre.LIBELLE);
                    cmd.Parameters.AddWithValue("@DATECREATION", pTypeCentre.DATECREATION);
                    cmd.Parameters.AddWithValue("@DATEMODIFICATION", pTypeCentre.DATEMODIFICATION);
                    cmd.Parameters.AddWithValue("@USERCREATION", pTypeCentre.USERCREATION);
                    cmd.Parameters.AddWithValue("@USERMODIFICATION", pTypeCentre.USERMODIFICATION);
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

        public bool Insert(List<CsTypeCentre> pTypeCentreCollection)
        {
            int number = 0;
            foreach (CsTypeCentre entity in pTypeCentreCollection)
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

        public List<CsTypeCentre> SelectAllTypeCentre()
        {
            try
            {
                return Entities.GetEntityListFromQuery<CsTypeCentre>(ParamProcedure.PARAM_TYPECENTRE_RETOURNE());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Delete(CsTypeCentre pTypeCentre)
        {
            try
            {
                return Entities.DeleteEntity<Galatee.Entity.Model.TYPECENTRE>(Entities.ConvertObject<Galatee.Entity.Model.TYPECENTRE, CsTypeCentre>(pTypeCentre));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Delete(List<CsTypeCentre> pTypeCentreCollection)
        {
            try
            {
                return Entities.DeleteEntity<Galatee.Entity.Model.TYPECENTRE>(Entities.ConvertObject<Galatee.Entity.Model.TYPECENTRE, CsTypeCentre>(pTypeCentreCollection));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Update(CsTypeCentre pTypeCentre)
        {
            try
            {
                return Entities.UpdateEntity<Galatee.Entity.Model.TYPECENTRE>(Entities.ConvertObject<Galatee.Entity.Model.TYPECENTRE, CsTypeCentre>(pTypeCentre)) ;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Update(List<CsTypeCentre> pTypeCentreCollection)
        {
            try
            {
                return Entities.UpdateEntity<Galatee.Entity.Model.TYPECENTRE>(Entities.ConvertObject<Galatee.Entity.Model.TYPECENTRE, CsTypeCentre>(pTypeCentreCollection));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Insert(CsTypeCentre pTypeCentre)
        {
            try
            {
                return Entities.InsertEntity<Galatee.Entity.Model.TYPECENTRE>(Entities.ConvertObject<Galatee.Entity.Model.TYPECENTRE, CsTypeCentre>(pTypeCentre));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Insert(List<CsTypeCentre> pTypeCentreCollection)
        {
            try
            {
                return Entities.InsertEntity<Galatee.Entity.Model.TYPECENTRE>(Entities.ConvertObject<Galatee.Entity.Model.TYPECENTRE, CsTypeCentre>(pTypeCentreCollection));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
