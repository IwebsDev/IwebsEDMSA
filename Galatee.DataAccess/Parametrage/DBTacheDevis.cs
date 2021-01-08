

#region Using directives

using System;
using System.Collections.Generic;
using System.Data;
using System.Collections;
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.ComponentModel;
//using Galate.StructureDevis;
using Galatee.Structure;


using System.Data.SqlClient;
using Galatee.Entity.Model;

#endregion

namespace Galatee.DataAccess
{
	///<summary>
	/// This class is the Data Access Logic Component implementation for the <see cref="TACHEDEVIS"/> business entity.
	///</summary>
	[DataObject]
    public partial class DBTACHEDEVIS /*: Galatee.DataAccess.Parametrage.DbBase*/
	{
        /*
	    public bool Transaction { get; set; }

	    private SqlCommand cmd = null;
        private SqlConnection cn = null;

        public static List<ObjTACHEDEVIS> Fill(IDataReader reader, List<ObjTACHEDEVIS> rows, int start, int pageLength)
        {
            // advance to the starting row
            for (int i = 0; i < start; i++)
            {
                if (!reader.Read())
                    return rows; // not enough rows, just return
            }

            for (int i = 0; i < pageLength; i++)
            {
                if (!reader.Read())
                    break; // we are done

                ObjTACHEDEVIS c = new ObjTACHEDEVIS();
                c.PK_ID = (Convert.IsDBNull(reader["Id"])) ? (int)0 : (System.Int32)reader["Id"];
                c.OriginalPK_ID = (Convert.IsDBNull(reader["Id"])) ? (int)0 : (System.Int32)reader["Id"];
                c.LIBELLE = (Convert.IsDBNull(reader["Libelle"])) ? string.Empty : (System.String)reader["Libelle"];
                c.USERCREATION = (Convert.IsDBNull(reader["USERCREATION"])) ? string.Empty : (System.String)reader["USERCREATION"];
                c.USERMODIFICATION = (Convert.IsDBNull(reader["USERMODIFICATION"])) ? string.Empty : (System.String)reader["USERMODIFICATION"];
                c.DATECREATION = (Convert.IsDBNull(reader["DATECREATION"])) ? (DateTime?)null : (System.DateTime)reader["DATECREATION"];
                c.DATEMODIFICATION = (Convert.IsDBNull(reader["DATEMODIFICATION"])) ? (DateTime?)null : (System.DateTime)reader["DATEMODIFICATION"];
                rows.Add(c);
            }
            return rows;
        }

        public static ObjTACHEDEVIS GetById(int? id)
        {
            ObjTACHEDEVIS row = new ObjTACHEDEVIS();

            string connectString = Session.GetSqlConnexionString();

            SqlConnection connection = new SqlConnection(connectString);
            SqlCommand command = new SqlCommand("SPX_PARAM_TACHEDEVIS_RETOURNEById", connection);
            command.CommandType = CommandType.StoredProcedure;

            SqlParameter param = command.Parameters.Add(new SqlParameter("@Id", id));
            param.Direction = ParameterDirection.Input;

            try
            {
                //Ouverture
                connection.Open();

                //Object datareader
                SqlDataReader reader = command.ExecuteReader();

                List<ObjTACHEDEVIS> tmp = new List<ObjTACHEDEVIS>();
                Fill(reader, tmp, 0, int.MaxValue);
                reader.Close();

                if (tmp.Count == 1)
                {
                    row = tmp[0];
                }
                else if (tmp.Count == 0)
                {
                    row = null;
                }
                return row;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                command.Dispose();
            }
        }

        public static List<ObjTACHEDEVIS> GetAll()
        {
            string connectString = Session.GetSqlConnexionString();
            SqlConnection connection = new SqlConnection(connectString);
            SqlCommand command = new SqlCommand("SPX_PARAM_TACHEDEVIS_RETOURNE", connection);
            command.CommandType = CommandType.StoredProcedure;

            try
            {


                if (connection.State == ConnectionState.Closed)
                    connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                List<ObjTACHEDEVIS> rows = new List<ObjTACHEDEVIS>();
                Fill(reader, rows, 0, int.MaxValue);
                reader.Close();
                return rows;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                //Fermeture base
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                connection.Dispose();
                command.Dispose();
            }

        }

        public bool Delete(ObjTACHEDEVIS pTacheDevis)
        {
            try
            {
                cn = new SqlConnection(Session.GetSqlConnexionString());
                cmd = new SqlCommand
                {
                    Connection = cn,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "SPX_PARAM_TACHEDEVIS_SUPPRIMER"
                };
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@Id", pTacheDevis.PK_ID);
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
                throw ex;
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                    cn.Close();
                cmd.Dispose();
            }
        }

        public bool Delete(List<ObjTACHEDEVIS> pTacheDevisCollection)
        {
            int number = 0;
            foreach (ObjTACHEDEVIS entity in pTacheDevisCollection)
            {
                if (Delete(entity))
                {
                    number++;
                }
            }
            return number != 0;
        }

        public bool Update(ObjTACHEDEVIS pTacheDevis)
        {
            cn = new SqlConnection(Session.GetSqlConnexionString());
            cmd = new SqlCommand
            {
                Connection = cn,
                CommandType = CommandType.StoredProcedure,
                CommandText = "SPX_PARAM_TACHEDEVIS_UPDATE"
            };
            cmd.Parameters.Clear();

            try
            {
                cmd.Parameters.AddWithValue("@Id", pTacheDevis.PK_ID);
                cmd.Parameters.AddWithValue("@LIBELLE", pTacheDevis.LIBELLE);
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

        public bool Update(List<ObjTACHEDEVIS> pTacheDevisCollection)
        {
            int number = 0;
            foreach (ObjTACHEDEVIS entity in pTacheDevisCollection)
            {
                if (Update(entity))
                {
                    number++;
                }
            }
            return number != 0;
        }

        public bool Insert(ObjTACHEDEVIS pTacheDevis)
        {
            cn = new SqlConnection(Session.GetSqlConnexionString());
            cmd = new SqlCommand
            {
                Connection = cn,
                CommandType = CommandType.StoredProcedure,
                CommandText = "SPX_PARAM_TACHEDEVIS_INSERER"
            };
            cmd.Parameters.Clear();

            try
            {
                cmd.Parameters.AddWithValue("@LIBELLE", pTacheDevis.LIBELLE);
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

        public bool Insert(List<ObjTACHEDEVIS> pTypeDevisCollection)
        {
            int number = 0;
            foreach (ObjTACHEDEVIS entity in pTypeDevisCollection)
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
            if ((Transaction) && (_conn != null))
            {
                cmd.Transaction = this.BeginTransaction(_conn);
            }
        }

        private void CommitTransaction(SqlTransaction _pSqlTransaction)
        {
            if ((Transaction) && (_pSqlTransaction != null))
            {
                this.Commit(_pSqlTransaction);
            }
        }

        private void RollBackTransaction(SqlTransaction _pSqlTransaction)
        {
            if ((Transaction) && (_pSqlTransaction != null))
            {
                this.RollBack(_pSqlTransaction);
            }
        }

        */

        public static ObjTACHEDEVIS GetById(int id)
        {
            try
            {
                return Entities.GetEntityFromQuery<ObjTACHEDEVIS>(ParamProcedure.PARAM_TACHEDEVIS_RETOURNEById(id));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<ObjTACHEDEVIS> GetAll()
        {
            try
            {
                return Entities.GetEntityListFromQuery<ObjTACHEDEVIS>(ParamProcedure.PARAM_TACHEDEVIS_RETOURNE());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public bool Delete(ObjTACHEDEVIS pTacheDevis)
        //{
        //    try
        //    {
        //        return Entities.DeleteEntity<Galatee.Entity.Model.TACHEDEVIS>(Entities.ConvertObject<Galatee.Entity.Model.TACHEDEVIS, ObjTACHEDEVIS>(pTacheDevis));
        //    }
        //    catch (Exception e)
        //    {
        //        throw e;
        //    }
        //}

        //public bool Delete(List<ObjTACHEDEVIS> pTacheDevisCollection)
        //{
        //    try
        //    {
        //        return Entities.DeleteEntity<Galatee.Entity.Model.TACHEDEVIS>(Entities.ConvertObject<Galatee.Entity.Model.TACHEDEVIS, ObjTACHEDEVIS>(pTacheDevisCollection));
        //    }
        //    catch (Exception e)
        //    {
        //        throw e;
        //    }
        //}

        //public bool Update(ObjTACHEDEVIS pTacheDevis)
        //{
        //    try
        //    {
        //        return Entities.UpdateEntity<Galatee.Entity.Model.TACHEDEVIS>(Entities.ConvertObject<Galatee.Entity.Model.TACHEDEVIS, ObjTACHEDEVIS>(pTacheDevis));
        //    }
        //    catch (Exception e)
        //    {
        //        throw e;
        //    }
        //}

        //public bool Update(List<ObjTACHEDEVIS> pTacheDevisCollection)
        //{
        //    try
        //    {
        //        return Entities.UpdateEntity<Galatee.Entity.Model.TACHEDEVIS>(Entities.ConvertObject<Galatee.Entity.Model.TACHEDEVIS, ObjTACHEDEVIS>(pTacheDevisCollection));
        //    }
        //    catch (Exception e)
        //    {
        //        throw e;
        //    }
        //}

        //public bool Insert(ObjTACHEDEVIS pTacheDevis)
        //{
        //    try
        //    {
        //        return Entities.InsertEntity<Galatee.Entity.Model.TACHEDEVIS>(Entities.ConvertObject<Galatee.Entity.Model.TACHEDEVIS, ObjTACHEDEVIS>(pTacheDevis));
        //    }
        //    catch (Exception e)
        //    {
        //        throw e;
        //    }
        //}

        //public bool Insert(List<ObjTACHEDEVIS> pTacheDevisCollection)
        //{
        //    try
        //    {
        //        return Entities.InsertEntity<Galatee.Entity.Model.TACHEDEVIS>(Entities.ConvertObject<Galatee.Entity.Model.TACHEDEVIS, ObjTACHEDEVIS>(pTacheDevisCollection));
        //    }
        //    catch (Exception e)
        //    {
        //        throw e;
        //    }
        //}
	}
}