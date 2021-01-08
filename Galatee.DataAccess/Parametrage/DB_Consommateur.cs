﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Galatee.Structure;
using Galatee.Entity.Model;

namespace Galatee.DataAccess
{
    public class DB_Consommateur : Galatee.DataAccess.Parametrage.DbBase
    {
        /*
        private string ConnectionString;

        public DB_Consommateur()
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

        public DB_Consommateur(string ConnStr)
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

        public List<CsConsommateur> SelectAllConsommateur()
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
                              CommandText = EnumProcedureStockee.SelectCONSOMMATEUR
                          };
                IDataReader reader = cmd.ExecuteReader();
                var rows = new List<CsConsommateur>();
                Fill(reader, rows, int.MinValue, int.MaxValue);
                reader.Close();
                return rows;
            }
            catch (Exception ex)
            {
                throw new Exception(EnumProcedureStockee.SelectCONSOMMATEUR + ":" + ex.Message);
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                    cn.Close(); // Fermeture de la connection 
                cmd.Dispose();
            }
        }

        public bool Delete(CsConsommateur pConsommateur)
        {
            try
            {
                cn = new SqlConnection(ConnectionString);
                cmd = new SqlCommand
                {
                    Connection = cn,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = EnumProcedureStockee.DeleteCONSOMMATEUR
                };
                cmd.Parameters.Clear();
                //cmd.Parameters.AddWithValue("@CODE", pConsommateur.CODE);
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
                throw new Exception(EnumProcedureStockee.DeleteCONSOMMATEUR + ":" + ex.Message);
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                cn.Close();
                cmd.Dispose();
            }
        }

        public bool Delete(List<CsConsommateur> pConsommateurCollection)
        {
            int number = 0;
            foreach (CsConsommateur entity in pConsommateurCollection)
            {
                if (Delete(entity))
                {
                    number++;
                }
            }
            return number != 0;
        }

        public static List<CsConsommateur> Fill(IDataReader reader, List<CsConsommateur> rows, int start, int pageLength)
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

                var c = new CsConsommateur();
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

        public bool Update(CsConsommateur pConsommateur)
        {
                cn = new SqlConnection(ConnectionString);
                cmd = new SqlCommand
                          {
                              Connection = cn,
                              CommandType = CommandType.StoredProcedure,
                              CommandText = EnumProcedureStockee.UpdateCONSOMMATEUR
                          };
                cmd.Parameters.Clear();

                try
                {
                    //cmd.Parameters.AddWithValue("@CODE", pConsommateur.CODE);
                    cmd.Parameters.AddWithValue("@LIBELLE", pConsommateur.LIBELLE);
                    //cmd.Parameters.AddWithValue("@OriginalCODE", pConsommateur.OriginalCODE);
                    cmd.Parameters.AddWithValue("@DATECREATION", pConsommateur.DATECREATION);
                    cmd.Parameters.AddWithValue("@DATEMODIFICATION", pConsommateur.DATEMODIFICATION);
                    cmd.Parameters.AddWithValue("@USERCREATION", pConsommateur.USERCREATION);
                    cmd.Parameters.AddWithValue("@USERMODIFICATION", pConsommateur.USERMODIFICATION);
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

        public bool Update(List<CsConsommateur> pCsConsommateurCollection)
        {
            int number = 0;
            foreach (CsConsommateur entity in pCsConsommateurCollection)
            {
                if (Update(entity))
                {
                    number++;
                }
            }
            return number != 0;
        }

        public bool Insert(CsConsommateur pConsommateur)
        {
                cn = new SqlConnection(ConnectionString);
                cmd = new SqlCommand
                {
                    Connection = cn,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = EnumProcedureStockee.InsertCONSOMMATEUR
                };
                cmd.Parameters.Clear();

                try
                {
                    //cmd.Parameters.AddWithValue("@CODE", pConsommateur.CODE);
                    cmd.Parameters.AddWithValue("@LIBELLE", pConsommateur.LIBELLE);
                    cmd.Parameters.AddWithValue("@DATECREATION", pConsommateur.DATECREATION);
                    cmd.Parameters.AddWithValue("@DATEMODIFICATION", pConsommateur.DATEMODIFICATION);
                    cmd.Parameters.AddWithValue("@USERCREATION", pConsommateur.USERCREATION);
                    cmd.Parameters.AddWithValue("@USERMODIFICATION", pConsommateur.USERMODIFICATION);
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

        public bool Insert(List<CsConsommateur> pConsommateurCollection)
        {
            int number = 0;
            foreach (CsConsommateur entity in pConsommateurCollection)
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

        public List<CsConsommateur> SelectAllConsommateur()
        {
            try
            {
                return Entities.GetEntityListFromQuery<CsConsommateur>(ParamProcedure.PARAM_CODECONSOMMATEUR_RETOURNE());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Delete(CsConsommateur pConsommateur)
        {
            try
            {
                return Entities.DeleteEntity<Galatee.Entity.Model.CODECONSOMMATEUR>(Entities.ConvertObject<Galatee.Entity.Model.CODECONSOMMATEUR, CsConsommateur>(pConsommateur));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Delete(List<CsConsommateur> pConsommateurCollection)
        {
            try
            {
                return Entities.DeleteEntity<Galatee.Entity.Model.CODECONSOMMATEUR>(Entities.ConvertObject<Galatee.Entity.Model.CODECONSOMMATEUR, CsConsommateur>(pConsommateurCollection));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Update(CsConsommateur pConsommateur)
        {
            try
            {
                return Entities.UpdateEntity<Galatee.Entity.Model.CODECONSOMMATEUR>(Entities.ConvertObject<Galatee.Entity.Model.CODECONSOMMATEUR, CsConsommateur>(pConsommateur));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Update(List<CsConsommateur> pCsConsommateurCollection)
        {
            try
            {
                return Entities.UpdateEntity<Galatee.Entity.Model.CODECONSOMMATEUR>(Entities.ConvertObject<Galatee.Entity.Model.CODECONSOMMATEUR, CsConsommateur>(pCsConsommateurCollection));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Insert(CsConsommateur pConsommateur)
        {
            try
            {
                return Entities.InsertEntity<Galatee.Entity.Model.CODECONSOMMATEUR>(Entities.ConvertObject<Galatee.Entity.Model.CODECONSOMMATEUR, CsConsommateur>(pConsommateur));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Insert(List<CsConsommateur> pConsommateurCollection)
        {
            try
            {
                return Entities.InsertEntity<Galatee.Entity.Model.CODECONSOMMATEUR>(Entities.ConvertObject<Galatee.Entity.Model.CODECONSOMMATEUR, CsConsommateur>(pConsommateurCollection));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
