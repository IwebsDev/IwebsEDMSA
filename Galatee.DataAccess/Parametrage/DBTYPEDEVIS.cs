
/*********************************************************************************
GENERER LE : 13/02/2010 20:02:57
*********************************************************************************/



#region Using directives

using System;
using System.Collections.Generic;
using System.Data;
using System.Collections;
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.ComponentModel;
using Galatee.Structure;


using System.Data.SqlClient;
using Galatee.Entity.Model;

#endregion

namespace Galatee.DataAccess
{

	///<summary>
	/// This class is the Data Access Logic Component implementation for the <see cref="TYPEDEVIS"/> business entity.
	///</summary>
	[DataObject]
    public partial class DBTYPEDEVIS /*: Galatee.DataAccess.Parametrage.DbBase*/
	{
        /*
        public DBTYPEDEVIS() : base() { }

        private SqlConnection cn = null;

        private bool _Transaction;

        public bool Transaction
        {
            get { return _Transaction; }
            set { _Transaction = value; }

        }

        private SqlCommand cmd = null;

        public static List<ObjTYPEDEVIS> Fill(IDataReader reader, List<ObjTYPEDEVIS> rows, int start, int pageLength)
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

                ObjTYPEDEVIS c = new ObjTYPEDEVIS();
                c.Id = (Convert.IsDBNull(reader["Id"])) ? (int)0 : (System.Int32)reader["Id"];
                c.OriginalId = (Convert.IsDBNull(reader["Id"])) ? (int)0 : (System.Int32)reader["Id"];
                c.Libelle = (Convert.IsDBNull(reader["Libelle"])) ? string.Empty : (System.String)reader["Libelle"];
                c.CodeProduit = (Convert.IsDBNull(reader["CodeProduit"])) ? string.Empty : (System.String)reader["CodeProduit"];
                rows.Add(c);
            }
            return rows;
        }

        public static ObjTYPEDEVIS GetById(int? id)
        {
            ObjTYPEDEVIS row = new ObjTYPEDEVIS();

            //Jab 06.02.2013
            //string connectString = GalateeConfig.ConnectionStrings[Enumere.Connexion].ConnectionString;
            //Objet connection
            SqlConnection connection = new SqlConnection(Session.GetSqlConnexionString());
            //Objet Command
            //Fin Jab
            SqlCommand command = new SqlCommand("spx_TYPEDEVIS_GetById", connection);
            command.CommandType = CommandType.StoredProcedure;

            SqlParameter param = command.Parameters.Add(new SqlParameter("@Id", id));
            param.Direction = ParameterDirection.Input;

            try
            {
                //Ouverture
                connection.Open();

                //Object datareader
                SqlDataReader reader = command.ExecuteReader();

                List<ObjTYPEDEVIS> tmp = new List<ObjTYPEDEVIS>();
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

        public static List<ObjTYPEDEVIS> GetByCodeProduit(System.String codeProduit)
        {
            //Jab 06.02.2013
            //string connectString = GalateeConfig.ConnectionStrings[Enumere.Connexion].ConnectionString;
            //Objet connection
            SqlConnection connection = new SqlConnection(Session.GetSqlConnexionString());
            //Objet Command
            //Fin Jab
            //Objet Command
            SqlCommand command = new SqlCommand("spx_TYPEDEVIS_GetByCodeProduit", connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.Add("@CodeProduit", SqlDbType.VarChar, 2).Value = codeProduit;

            try
            {
                //Ouverture
                connection.Open();

                //Object datareader
                SqlDataReader reader = command.ExecuteReader();

                List<ObjTYPEDEVIS> rows = new List<ObjTYPEDEVIS>();
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
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                command.Dispose();
            }
        }

        public static List<ObjTYPEDEVIS> GetAll()
        {
            //Jab 06.02.2013
            //string connectString = GalateeConfig.ConnectionStrings[Enumere.Connexion].ConnectionString;
            //Objet connection
            SqlConnection connection = new SqlConnection(Session.GetSqlConnexionString());
            //Objet Command
            //Fin Jab
            //Objet Command
            SqlCommand command = new SqlCommand("SPX_PARAM_TYPEDEVIS_RETOURNE", connection);
            command.CommandType = CommandType.StoredProcedure;

            try
            {
                //Ouverture
                connection.Open();

                //Object datareader
                SqlDataReader reader = command.ExecuteReader();

                List<ObjTYPEDEVIS> rows = new List<ObjTYPEDEVIS>();
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
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                command.Dispose();
            }
        }

        public bool Delete(ObjTYPEDEVIS pTypeDevis)
        {
            try
            {
                cn = new SqlConnection(Session.GetSqlConnexionString());
                cmd = new SqlCommand
                {
                    Connection = cn,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "SPX_PARAM_TYPEDEVIS_SUPPRIMER"
                };
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@Id", pTypeDevis.Id);
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

        public bool Delete(List<ObjTYPEDEVIS> pTypeDevisCollection)
        {
            int number = 0;
            foreach (ObjTYPEDEVIS entity in pTypeDevisCollection)
            {
                if (Delete(entity))
                {
                    number++;
                }
            }
            return number != 0;
        }

        public bool Update(ObjTYPEDEVIS pTypeDevis)
        {
            cn = new SqlConnection(Session.GetSqlConnexionString());
            cmd = new SqlCommand
            {
                Connection = cn,
                CommandType = CommandType.StoredProcedure,
                CommandText = "SPX_PARAM_TYPEDEVIS_UPDATE"
            };
            cmd.Parameters.Clear();

            try
            {
                cmd.Parameters.AddWithValue("@Id", pTypeDevis.Id);
                cmd.Parameters.AddWithValue("@OriginalId", pTypeDevis.OriginalId);
                cmd.Parameters.AddWithValue("@LIBELLE", pTypeDevis.Libelle);
                cmd.Parameters.AddWithValue("@CodeProduit", pTypeDevis.CodeProduit);
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

        public bool Update(List<ObjTYPEDEVIS> pTypeDevisCollection)
        {
            int number = 0;
            foreach (ObjTYPEDEVIS entity in pTypeDevisCollection)
            {
                if (Update(entity))
                {
                    number++;
                }
            }
            return number != 0;
        }

        public bool Insert(ObjTYPEDEVIS pTypeDevis)
        {
            cn = new SqlConnection(Session.GetSqlConnexionString());
            cmd = new SqlCommand
            {
                Connection = cn,
                CommandType = CommandType.StoredProcedure,
                CommandText = "SPX_PARAM_TYPEDEVIS_INSERER"
            };
            cmd.Parameters.Clear();

            try
            {
                cmd.Parameters.AddWithValue("@Id", pTypeDevis.Id);
                cmd.Parameters.AddWithValue("@LIBELLE", pTypeDevis.Libelle);
                cmd.Parameters.AddWithValue("@CodeProduit", pTypeDevis.CodeProduit);
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

        public bool Insert(List<ObjTYPEDEVIS> pTypeDevisCollection)
        {
            int number = 0;
            foreach (ObjTYPEDEVIS entity in pTypeDevisCollection)
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

        public static ObjTYPEDEVIS GetById(int id)
        {
            try
            {
                return Entities.GetEntityFromQuery<ObjTYPEDEVIS>(ParamProcedure.PARAM_TYPEDEVIS_RETOURNEById(id));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<ObjTYPEDEVIS> GetByProduitId(int ProduitId)
        {
            try
            {
                return Entities.GetEntityListFromQuery<ObjTYPEDEVIS>(ParamProcedure.PARAM_TYPEDEVIS_RETOURNEByProduitId(ProduitId));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<ObjTYPEDEVIS> GetAll()
        {
            try
            {
                return Entities.GetEntityListFromQuery<ObjTYPEDEVIS>(ParamProcedure.PARAM_TYPEDEVIS_RETOURNE());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public bool Delete(ObjTYPEDEVIS pTypeDevis)
        //{
        //    try
        //    {
        //        return Entities.DeleteEntity<Galatee.Entity.Model.TYPEDEVIS>(Entities.ConvertObject<Galatee.Entity.Model.TYPEDEVIS, ObjTYPEDEVIS>(pTypeDevis));
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public bool Delete(List<ObjTYPEDEVIS> pTypeDevisCollection)
        //{
        //    try
        //    {
        //        return Entities.DeleteEntity<Galatee.Entity.Model.TYPEDEVIS>(Entities.ConvertObject<Galatee.Entity.Model.TYPEDEVIS, ObjTYPEDEVIS>(pTypeDevisCollection));
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public bool Update(ObjTYPEDEVIS pTypeDevis)
        //{
        //    try
        //    {
        //        return Entities.UpdateEntity<Galatee.Entity.Model.TYPEDEVIS>(Entities.ConvertObject<Galatee.Entity.Model.TYPEDEVIS, ObjTYPEDEVIS>(pTypeDevis));
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public bool Update(List<ObjTYPEDEVIS> pTypeDevisCollection)
        //{
        //    try
        //    {
        //        return Entities.UpdateEntity<Galatee.Entity.Model.TYPEDEVIS>(Entities.ConvertObject<Galatee.Entity.Model.TYPEDEVIS, ObjTYPEDEVIS>(pTypeDevisCollection));
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public bool Insert(ObjTYPEDEVIS pTypeDevis)
        //{
        //    try
        //    {
        //        return Entities.InsertEntity<Galatee.Entity.Model.TYPEDEVIS>(Entities.ConvertObject<Galatee.Entity.Model.TYPEDEVIS, ObjTYPEDEVIS>(pTypeDevis));
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public bool Insert(List<ObjTYPEDEVIS> pTypeDevisCollection)
        //{
        //    try
        //    {
        //        return Entities.InsertEntity<Galatee.Entity.Model.TYPEDEVIS>(Entities.ConvertObject<Galatee.Entity.Model.TYPEDEVIS, ObjTYPEDEVIS>(pTypeDevisCollection));
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
	}
}