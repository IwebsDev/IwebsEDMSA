

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

namespace Galatee.DataAccess 
{
	
	[DataObject]
    public partial class DBPIECEIDENTITE /*: Galatee.DataAccess.Parametrage.DbBase*/
	{
        /*
        public bool Transaction { get; set; }

        private SqlCommand cmd = null;
        private SqlConnection cn = null;

        public bool Delete(ObjPIECEIDENTITE pPieceIdentite)
        {
            try
            {
                cn = new SqlConnection(Session.GetSqlConnexionString());
                cmd = new SqlCommand
                {
                    Connection = cn,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "SPX_PARAM_PIECEIDENTITE_SUPPRIMER"
                };
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@Id", pPieceIdentite.Id);
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

        public bool Delete(List<ObjPIECEIDENTITE> pPieceIdentitesCollection)
        {
            int number = 0;
            foreach (ObjPIECEIDENTITE entity in pPieceIdentitesCollection)
            {
                if (Delete(entity))
                {
                    number++;
                }
            }
            return number != 0;
        }

        public bool Update(ObjPIECEIDENTITE pPieceIdentites)
        {
            cn = new SqlConnection(Session.GetSqlConnexionString());
            cmd = new SqlCommand
            {
                Connection = cn,
                CommandType = CommandType.StoredProcedure,
                CommandText = "SPX_PARAM_PIECEIDENTITE_UPDATE"
            };
            cmd.Parameters.Clear();

            try
            {
                cmd.Parameters.AddWithValue("@Id", pPieceIdentites.Id);
                cmd.Parameters.AddWithValue("@LIBELLE", pPieceIdentites.Libelle);
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

        public bool Update(List<ObjPIECEIDENTITE> pPieceIdentitesCollection)
        {
            int number = 0;
            foreach (ObjPIECEIDENTITE entity in pPieceIdentitesCollection)
            {
                if (Update(entity))
                {
                    number++;
                }
            }
            return number != 0;
        }

        public bool Insert(ObjPIECEIDENTITE pPieceIdentite)
        {
            cn = new SqlConnection(Session.GetSqlConnexionString());
            cmd = new SqlCommand
            {
                Connection = cn,
                CommandType = CommandType.StoredProcedure,
                CommandText = "SPX_PARAM_PIECEIDENTITE_INSERER"
            };
            cmd.Parameters.Clear();

            try
            {
                cmd.Parameters.AddWithValue("@LIBELLE", pPieceIdentite.Libelle);
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

        public bool Insert(List<ObjPIECEIDENTITE> pPieceIdentiteCollection)
        {
            int number = 0;
            foreach (ObjPIECEIDENTITE entity in pPieceIdentiteCollection)
            {
                if (Insert(entity))
                {
                    number++;
                }
            }
            return number != 0;
        }

        public static List<ObjPIECEIDENTITE> Fill(IDataReader reader, List<ObjPIECEIDENTITE> rows, int start, int pageLength)
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

                ObjPIECEIDENTITE c = new ObjPIECEIDENTITE();
                c.Id = (Convert.IsDBNull(reader["Id"])) ? (byte)0 : (System.Byte)reader["Id"];
                c.OriginalId = (Convert.IsDBNull(reader["Id"])) ? (byte)0 : (System.Byte)reader["Id"];
                c.Libelle = (Convert.IsDBNull(reader["Libelle"])) ? string.Empty : (System.String)reader["Libelle"];
                rows.Add(c);
            }
            return rows;
        }
        
        public static ObjPIECEIDENTITE GetById(byte? id)
        {
            ObjPIECEIDENTITE row = new ObjPIECEIDENTITE();
            //Jab 06.02.2013
           // string connectString = GalateeConfig.ConnectionStrings[Enumere.Connexion].ConnectionString;
            string connectString = Session.GetSqlConnexionString();

            //Fin JAb 
            //Objet connection
            SqlConnection connection = new SqlConnection(connectString);
            //Objet Command
            SqlCommand command = new SqlCommand("SPX_PARAM_PIECEIDENTITE_RETOURNEById", connection);
            command.CommandType = CommandType.StoredProcedure;

            SqlParameter param = command.Parameters.Add(new SqlParameter("@Id", id));
            param.Direction = ParameterDirection.Input;

            try
            {
                //Ouverture
                connection.Open();

                //Object datareader
                SqlDataReader reader = command.ExecuteReader();

                List<ObjPIECEIDENTITE> tmp = new List<ObjPIECEIDENTITE>();
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
                command.Dispose();
            }
            return row;
        }

        public static List<ObjPIECEIDENTITE> GetAll()
        {
            //Jab 06.02.2013
            // string connectString = GalateeConfig.ConnectionStrings[Enumere.Connexion].ConnectionString;
            string connectString = Session.GetSqlConnexionString();

            //Objet connection
            SqlConnection connection = new SqlConnection(connectString);
            //Objet Command
            SqlCommand command = new SqlCommand("SPX_PARAM_PIECEIDENTITE_RETOURNE", connection);
            command.CommandType = CommandType.StoredProcedure;

            try
            {
                //Ouverture
                connection.Open();

                //Object datareader
                SqlDataReader reader = command.ExecuteReader();

                List<ObjPIECEIDENTITE> rows = new List<ObjPIECEIDENTITE>();
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
                command.Dispose();
            }

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
        
        public bool Delete(ObjPIECEIDENTITE pPieceIdentite)
        {
            try
            {
                return Entities.DeleteEntity<Galatee.Entity.Model.PIECEIDENTITE>(Entities.ConvertObject<Galatee.Entity.Model.PIECEIDENTITE, ObjPIECEIDENTITE>(pPieceIdentite));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Delete(List<ObjPIECEIDENTITE> pPieceIdentitesCollection)
        {
            try
            {
                return Entities.DeleteEntity<Galatee.Entity.Model.PIECEIDENTITE>(Entities.ConvertObject<Galatee.Entity.Model.PIECEIDENTITE, ObjPIECEIDENTITE>(pPieceIdentitesCollection));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Update(ObjPIECEIDENTITE pPieceIdentites)
        {
            try
            {
                return Entities.UpdateEntity<Galatee.Entity.Model.PIECEIDENTITE>(Entities.ConvertObject<Galatee.Entity.Model.PIECEIDENTITE, ObjPIECEIDENTITE>(pPieceIdentites));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Update(List<ObjPIECEIDENTITE> pPieceIdentitesCollection)
        {
            try
            {
                return Entities.UpdateEntity<Galatee.Entity.Model.PIECEIDENTITE>(Entities.ConvertObject<Galatee.Entity.Model.PIECEIDENTITE, ObjPIECEIDENTITE>(pPieceIdentitesCollection));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Insert(ObjPIECEIDENTITE pPieceIdentite)
        {
            try
            {
                return Entities.InsertEntity<Galatee.Entity.Model.PIECEIDENTITE>(Entities.ConvertObject<Galatee.Entity.Model.PIECEIDENTITE, ObjPIECEIDENTITE>(pPieceIdentite));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Insert(List<ObjPIECEIDENTITE> pPieceIdentiteCollection)
        {
            try
            {
                return Entities.InsertEntity<Galatee.Entity.Model.PIECEIDENTITE>(Entities.ConvertObject<Galatee.Entity.Model.PIECEIDENTITE, ObjPIECEIDENTITE>(pPieceIdentiteCollection));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static ObjPIECEIDENTITE GetById(int id)
        {
            try
            {
                return Entities.GetEntityFromQuery<ObjPIECEIDENTITE>(ParamProcedure.PARAM_PIECEIDENTITE_RETOURNEById(id));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<ObjPIECEIDENTITE> GetAll()
        {
            try
            {
                //return Entities.GetEntityListFromQuery<ObjPIECEIDENTITE>(ParamProcedure.PARAM_PIECEIDENTITE_RETOURNE());
                DataTable dt = DB_ParametresGeneraux.SelectAllDonneReference("PIECEIDENTITE");
                return Entities.GetEntityListFromQuery<ObjPIECEIDENTITE>(dt);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
	}
}