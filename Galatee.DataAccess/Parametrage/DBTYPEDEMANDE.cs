
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
	/// This class is the Data Access Logic Component implementation for the <see cref="TDEM"/> business entity.
	///</summary>
	[DataObject]
    public partial class DBTYPEDEMANDE /*: Galatee.DataAccess.Parametrage.DbBase*/
	{
        /*
        public DBTDEM() : base() { }

        private SqlConnection cn = null;

        private bool _Transaction;

        public bool Transaction
        {
            get { return _Transaction; }
            set { _Transaction = value; }

        }

        private SqlCommand cmd = null;

        public static List<CsTdem> Fill(IDataReader reader, List<CsTdem> rows, int start, int pageLength)
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

                CsTdem c = new CsTdem();
                c.Id = (Convert.IsDBNull(reader["Id"])) ? (int)0 : (System.Int32)reader["Id"];
                c.OriginalId = (Convert.IsDBNull(reader["Id"])) ? (int)0 : (System.Int32)reader["Id"];
                c.Libelle = (Convert.IsDBNull(reader["Libelle"])) ? string.Empty : (System.String)reader["Libelle"];
                c.CodeProduit = (Convert.IsDBNull(reader["CodeProduit"])) ? string.Empty : (System.String)reader["CodeProduit"];
                rows.Add(c);
            }
            return rows;
        }

        public static CsTdem GetById(int? id)
        {
            CsTdem row = new CsTdem();

            //Jab 06.02.2013
            //string connectString = GalateeConfig.ConnectionStrings[Enumere.Connexion].ConnectionString;
            //Objet connection
            SqlConnection connection = new SqlConnection(Session.GetSqlConnexionString());
            //Objet Command
            //Fin Jab
            SqlCommand command = new SqlCommand("spx_TDEM_GetById", connection);
            command.CommandType = CommandType.StoredProcedure;

            SqlParameter param = command.Parameters.Add(new SqlParameter("@Id", id));
            param.Direction = ParameterDirection.Input;

            try
            {
                //Ouverture
                connection.Open();

                //Object datareader
                SqlDataReader reader = command.ExecuteReader();

                List<CsTdem> tmp = new List<CsTdem>();
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

        public static List<CsTdem> GetByCodeProduit(System.String codeProduit)
        {
            //Jab 06.02.2013
            //string connectString = GalateeConfig.ConnectionStrings[Enumere.Connexion].ConnectionString;
            //Objet connection
            SqlConnection connection = new SqlConnection(Session.GetSqlConnexionString());
            //Objet Command
            //Fin Jab
            //Objet Command
            SqlCommand command = new SqlCommand("spx_TDEM_GetByCodeProduit", connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.Add("@CodeProduit", SqlDbType.VarChar, 2).Value = codeProduit;

            try
            {
                //Ouverture
                connection.Open();

                //Object datareader
                SqlDataReader reader = command.ExecuteReader();

                List<CsTdem> rows = new List<CsTdem>();
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

        public static List<CsTdem> GetAll()
        {
            //Jab 06.02.2013
            //string connectString = GalateeConfig.ConnectionStrings[Enumere.Connexion].ConnectionString;
            //Objet connection
            SqlConnection connection = new SqlConnection(Session.GetSqlConnexionString());
            //Objet Command
            //Fin Jab
            //Objet Command
            SqlCommand command = new SqlCommand("SPX_PARAM_TDEM_RETOURNE", connection);
            command.CommandType = CommandType.StoredProcedure;

            try
            {
                //Ouverture
                connection.Open();

                //Object datareader
                SqlDataReader reader = command.ExecuteReader();

                List<CsTdem> rows = new List<CsTdem>();
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

        public bool Delete(CsTdem pTypeDevis)
        {
            try
            {
                cn = new SqlConnection(Session.GetSqlConnexionString());
                cmd = new SqlCommand
                {
                    Connection = cn,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "SPX_PARAM_TDEM_SUPPRIMER"
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

        public bool Delete(List<CsTdem> pTypeDevisCollection)
        {
            int number = 0;
            foreach (CsTdem entity in pTypeDevisCollection)
            {
                if (Delete(entity))
                {
                    number++;
                }
            }
            return number != 0;
        }

        public bool Update(CsTdem pTypeDevis)
        {
            cn = new SqlConnection(Session.GetSqlConnexionString());
            cmd = new SqlCommand
            {
                Connection = cn,
                CommandType = CommandType.StoredProcedure,
                CommandText = "SPX_PARAM_TDEM_UPDATE"
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

        public bool Update(List<CsTdem> pTypeDevisCollection)
        {
            int number = 0;
            foreach (CsTdem entity in pTypeDevisCollection)
            {
                if (Update(entity))
                {
                    number++;
                }
            }
            return number != 0;
        }

        public bool Insert(CsTdem pTypeDevis)
        {
            cn = new SqlConnection(Session.GetSqlConnexionString());
            cmd = new SqlCommand
            {
                Connection = cn,
                CommandType = CommandType.StoredProcedure,
                CommandText = "SPX_PARAM_TDEM_INSERER"
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

        public bool Insert(List<CsTdem> pTypeDevisCollection)
        {
            int number = 0;
            foreach (CsTdem entity in pTypeDevisCollection)
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

        //public static CsTdem GetById(int id)
        //{
        //    try
        //    {
        //        return Entities.GetEntityFromQuery<CsTdem>(ParamProcedure.PARAM_TDEM_RETOURNEById(id));
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public static List<CsTdem> GetByProduitId(int ProduitId)
        //{
        //    try
        //    {
        //        return Entities.GetEntityListFromQuery<CsTdem>(ParamProcedure.PARAM_TDEM_RETOURNEByProduitId(ProduitId));
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public static List<CsTdem> GetAll()
        {
            try
            {
                //return Entities.GetEntityListFromQuery<CsTdem>(ParamProcedure.PARAM_TDEM_RETOURNE());
                DataTable dt = DB_ParametresGeneraux.SelectAllDonneReference("TYPEDEMANDE");
                return Entities.GetEntityListFromQuery<CsTdem>(dt);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public bool Delete(CsTdem pTypeDevis)
        //{
        //    try
        //    {
        //        return Entities.DeleteEntity<Galatee.Entity.Model.TDEM>(Entities.ConvertObject<Galatee.Entity.Model.TDEM, CsTdem>(pTypeDevis));
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public bool Delete(List<CsTdem> pTypeDevisCollection)
        //{
        //    try
        //    {
        //        return Entities.DeleteEntity<Galatee.Entity.Model.TDEM>(Entities.ConvertObject<Galatee.Entity.Model.TDEM, CsTdem>(pTypeDevisCollection));
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public bool Update(CsTdem pTypeDevis)
        //{
        //    try
        //    {
        //        return Entities.UpdateEntity<Galatee.Entity.Model.TDEM>(Entities.ConvertObject<Galatee.Entity.Model.TDEM, CsTdem>(pTypeDevis));
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public bool Update(List<CsTdem> pTypeDevisCollection)
        //{
        //    try
        //    {
        //        return Entities.UpdateEntity<Galatee.Entity.Model.TDEM>(Entities.ConvertObject<Galatee.Entity.Model.TDEM, CsTdem>(pTypeDevisCollection));
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public bool Insert(CsTdem pTypeDevis)
        //{
        //    try
        //    {
        //        return Entities.InsertEntity<Galatee.Entity.Model.TDEM>(Entities.ConvertObject<Galatee.Entity.Model.TDEM, CsTdem>(pTypeDevis));
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public bool Insert(List<CsTdem> pTypeDevisCollection)
        //{
        //    try
        //    {
        //        return Entities.InsertEntity<Galatee.Entity.Model.TDEM>(Entities.ConvertObject<Galatee.Entity.Model.TDEM, CsTdem>(pTypeDevisCollection));
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
	}
}