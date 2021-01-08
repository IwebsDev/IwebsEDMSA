using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Galatee.Structure;
using Galatee.Entity.Model;

namespace Galatee.DataAccess
{
    public class DB_Produit : Galatee.DataAccess.Parametrage.DbBase
    {
        /*
        private string ConnectionString;

        public DB_Produit()
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

        public DB_Produit(string ConnStr)
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

        public ProduitCollection SelectAllProduit()
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
                              CommandText = EnumProcedureStockee.SelectPRODUIT
                          };
                IDataReader reader = cmd.ExecuteReader();
                var rows = new ProduitCollection();
                Fill(reader, rows, int.MinValue, int.MaxValue);
                reader.Close();
                return rows;
            }
            catch (Exception ex)
            {
                throw new Exception(EnumProcedureStockee.SelectPRODUIT + ":" + ex.Message);
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                    cn.Close(); // Fermeture de la connection 
                cmd.Dispose();
            }
        }

        public bool Delete(CsProduit pProduit)
        {
            try
            {
                cn = new SqlConnection(ConnectionString);
                cmd = new SqlCommand
                {
                    Connection = cn,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = EnumProcedureStockee.DeletePRODUIT
                };
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@CODE", pProduit.PK_PRODUIT );
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
                throw new Exception(EnumProcedureStockee.DeletePRODUIT + ":" + ex.Message);
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                cn.Close();
                cmd.Dispose();
            }
        }

        public CsProduit SelectProduitByCode(string pProduit)
        {
            try
            {
                cn = new SqlConnection(ConnectionString);
                cmd = new SqlCommand
                {
                    Connection = cn,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "SPX_PARAM_PRODUIT_RETOURNEByCODE"
                };
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@CODE", pProduit);
                if (cn.State == ConnectionState.Closed)
                    cn.Open();
                StartTransaction(cn);
                IDataReader reader = cmd.ExecuteReader();
                var rows = new ProduitCollection();
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
                throw new Exception("SPX_PARAM_PRODUIT_RETOURNEByCODE" + ":" + ex.Message);
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                    cn.Close();
                cmd.Dispose();
            }
        }

        public bool Delete(ProduitCollection pProduitCollection)
        {
            int number = 0;
            foreach (CsProduit entity in pProduitCollection)
            {
                if (Delete(entity))
                {
                    number++;
                }
            }
            return number != 0;
        }

        public static ProduitCollection Fill(IDataReader reader,ProduitCollection rows, int start, int pageLength)
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

                var c = new CsProduit();
				c.PK_PRODUIT  = (Convert.IsDBNull(reader["CODE"]))?string.Empty:(System.String)reader["CODE"];
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

        public bool Update(CsProduit pProduit)
        {
                cn = new SqlConnection(ConnectionString);
                cmd = new SqlCommand
                          {
                              Connection = cn,
                              CommandType = CommandType.StoredProcedure,
                              CommandText = EnumProcedureStockee.UpdatePRODUIT
                          };
                cmd.Parameters.Clear();

                try
                {
                    cmd.Parameters.AddWithValue("@CODE", pProduit.PK_PRODUIT );
                    cmd.Parameters.AddWithValue("@LIBELLE", pProduit.LIBELLE);
                    cmd.Parameters.AddWithValue("@OriginalCODE", pProduit.OriginalCODE);
                    cmd.Parameters.AddWithValue("@DATECREATION", pProduit.DATECREATION);
                    cmd.Parameters.AddWithValue("@DATEMODIFICATION", pProduit.DATEMODIFICATION);
                    cmd.Parameters.AddWithValue("@USERCREATION", pProduit.USERCREATION);
                    cmd.Parameters.AddWithValue("@USERMODIFICATION", pProduit.USERMODIFICATION);
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

        public bool Update(ProduitCollection pProduitCollection)
        {
            int number = 0;
            foreach (CsProduit entity in pProduitCollection)
            {
                if (Update(entity))
                {
                    number++;
                }
            }
            return number != 0;
        }

        public bool Insert(CsProduit pProduit)
        {
                cn = new SqlConnection(ConnectionString);
                cmd = new SqlCommand
                {
                    Connection = cn,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = EnumProcedureStockee.InsertPRODUIT
                };
                cmd.Parameters.Clear();

                try
                {
                   cmd.Parameters.AddWithValue("@CODE", pProduit.PK_PRODUIT );
                    cmd.Parameters.AddWithValue("@LIBELLE", pProduit.LIBELLE);
                    cmd.Parameters.AddWithValue("@DATECREATION", pProduit.DATECREATION);
                    cmd.Parameters.AddWithValue("@DATEMODIFICATION", pProduit.DATEMODIFICATION);
                    cmd.Parameters.AddWithValue("@USERCREATION", pProduit.USERCREATION);
                    cmd.Parameters.AddWithValue("@USERMODIFICATION", pProduit.USERMODIFICATION);
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

        public bool Insert(ProduitCollection pProduitCollection)
        {
            int number = 0;
            foreach (CsProduit entity in pProduitCollection)
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

        public List<CsProduit> SelectAllProduit()
        {
            try
            {
                return Entities.GetEntityListFromQuery<CsProduit>(CommonProcedures.RetourneTousProduit());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Delete(CsProduit pProduit)
        {
            try
            {
                return Entities.DeleteEntity<Galatee.Entity.Model.PRODUIT>(Entities.ConvertObject<Galatee.Entity.Model.PRODUIT, CsProduit>(pProduit));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public CsProduit SelectProduitByProduitId(int pProduitId)
        {
            try
            {
                return Entities.GetEntityFromQuery<CsProduit>(ParamProcedure.PARAM_PRODUIT_RETOURNEById(pProduitId));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Delete(List<CsProduit> pProduitCollection)
        {
            try
            {
                return Entities.DeleteEntity<Galatee.Entity.Model.PRODUIT>(Entities.ConvertObject<Galatee.Entity.Model.PRODUIT, CsProduit>(pProduitCollection));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
               
        public bool Update(CsProduit pProduit)
        {
            try
            {
                return Entities.UpdateEntity<Galatee.Entity.Model.PRODUIT>(Entities.ConvertObject<Galatee.Entity.Model.PRODUIT, CsProduit>(pProduit));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Update(List<CsProduit> pProduitCollection)
        {
            try
            {
                return Entities.UpdateEntity<Galatee.Entity.Model.PRODUIT>(Entities.ConvertObject<Galatee.Entity.Model.PRODUIT, CsProduit>(pProduitCollection));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Insert(CsProduit pProduit)
        {
            try
            {
                return Entities.InsertEntity<Galatee.Entity.Model.PRODUIT>(Entities.ConvertObject<Galatee.Entity.Model.PRODUIT, CsProduit>(pProduit));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Insert(List<CsProduit> pProduitCollection)
        {
            try
            {
                return Entities.InsertEntity<Galatee.Entity.Model.PRODUIT>(Entities.ConvertObject<Galatee.Entity.Model.PRODUIT, CsProduit>(pProduitCollection));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
