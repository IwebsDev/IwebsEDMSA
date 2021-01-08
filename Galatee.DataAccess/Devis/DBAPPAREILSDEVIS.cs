
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
	/// This class is the Data Access Logic Component implementation for the <see cref="APPAREILSDEVIS"/> business entity.
	///</summary>
	[DataObject]
	public partial class DBAPPAREILSDEVIS : DBBase
	{
        /*
        public static List<ObjAPPAREILSDEVIS> Fill(IDataReader reader, List<ObjAPPAREILSDEVIS> rows, int start, int pageLength)
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

                ObjAPPAREILSDEVIS c = new ObjAPPAREILSDEVIS();
                c.NumDevis = (Convert.IsDBNull(reader["NumDevis"])) ? string.Empty : (System.String)reader["NumDevis"];
                c.OriginalNumDevis = (Convert.IsDBNull(reader["NumDevis"])) ? string.Empty : (System.String)reader["NumDevis"];
                c.CodeAppareil = (Convert.IsDBNull(reader["CodeAppareil"])) ? (int)0 : (System.Int32)reader["CodeAppareil"];
                c.OriginalCodeAppareil = (Convert.IsDBNull(reader["CodeAppareil"])) ? (int)0 : (System.Int32)reader["CodeAppareil"];
                if (Convert.IsDBNull(reader["Nbre"]))
                    c.Nbre = null;
                else
                    c.Nbre = (System.Int32)reader["Nbre"];
                if (Convert.IsDBNull(reader["Puissance"]))
                    c.Puissance = null;
                else
                    c.Puissance = (System.Int32)reader["Puissance"];
                rows.Add(c);
            }
            return rows;
        }

        public static List<ObjAPPAREILSDEVIS> FillForDevis(IDataReader reader, List<ObjAPPAREILSDEVIS> rows, int start, int pageLength)
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

                ObjAPPAREILSDEVIS c = new ObjAPPAREILSDEVIS();
                c.NumDevis = (Convert.IsDBNull(reader["NumDevis"])) ? string.Empty : (System.String)reader["NumDevis"];
                c.OriginalNumDevis = (Convert.IsDBNull(reader["NumDevis"])) ? string.Empty : (System.String)reader["NumDevis"];
                c.CodeAppareil = (Convert.IsDBNull(reader["CodeAppareil"])) ? (int)0 : (System.Int32)reader["CodeAppareil"];
                c.OriginalCodeAppareil = (Convert.IsDBNull(reader["CodeAppareil"])) ? (int)0 : (System.Int32)reader["CodeAppareil"];
                if (Convert.IsDBNull(reader["Nbre"]))
                    c.Nbre = null;
                else
                    c.Nbre = (System.Int32)reader["Nbre"];
                if (Convert.IsDBNull(reader["Puissance"]))
                    c.Puissance = null;
                else
                    c.Puissance = (System.Int32)reader["Puissance"];
                c.Designation = (Convert.IsDBNull(reader["Designation"])) ? string.Empty : (System.String)reader["Designation"];

                rows.Add(c);
            }
            return rows;
        }

        public static List<ObjAPPAREILSDEVIS> GetByNumDevis(System.String numDevis)
        {
            SqlConnection connection = new SqlConnection(Session.GetSqlConnexionString());
            SqlCommand command = new SqlCommand("SPX_DEVIS_APPAREILSDEVIS_RETOURNEByNumDevis", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@NumDevis", SqlDbType.VarChar, 8).Value = numDevis;

            try
            {
                //Ouverture
                if (connection.State == ConnectionState.Closed)
                    connection.Open();

                //Object datareader
                SqlDataReader reader = command.ExecuteReader();

                List<ObjAPPAREILSDEVIS> rows = new List<ObjAPPAREILSDEVIS>();
                FillForDevis(reader, rows, 0, int.MaxValue);
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
                connection.Dispose();
            }
        }

        public static List<ObjAPPAREILSDEVIS> GetByAll()
        {
            SqlConnection connection = new SqlConnection(Session.GetSqlConnexionString());
            SqlCommand command = new SqlCommand("SPX_DEVIS_APPAREILSDEVIS_RETOURNE", connection);
            command.CommandType = CommandType.StoredProcedure;

            try
            {
                //Ouverture
                if (connection.State == ConnectionState.Closed)
                    connection.Open();

                //Object datareader
                SqlDataReader reader = command.ExecuteReader();

                List<ObjAPPAREILSDEVIS> rows = new List<ObjAPPAREILSDEVIS>();
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
                connection.Dispose();
            }
        }

        public bool Delete(ObjAPPAREILSDEVIS entity)
        {
            SqlConnection connection = new SqlConnection(Session.GetSqlConnexionString());
            //Fin jab
            SqlCommand command = new SqlCommand("SPX_DEVIS_APPAREILSDEVIS_SUPPRIMER", connection);
            command.CommandType = CommandType.StoredProcedure;

            int rowsAffected = -1;

            command.Parameters.Add("@NumDevis", SqlDbType.VarChar, 8).Value = entity.NumDevis;
            command.Parameters.Add("@CodeAppareil", SqlDbType.Int).Value = entity.CodeAppareil;

            if (command.Connection.State == ConnectionState.Closed)
                command.Connection.Open();
            try
            {

                rowsAffected = command.ExecuteNonQuery();
                if (rowsAffected == 0)
                {
                    //throw new DataException("The record has been already deleted.");
                    return false;
                }
                return Convert.ToBoolean(rowsAffected);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                command.Dispose();
                connection.Dispose();
            }
        }

        public int Delete(List<ObjAPPAREILSDEVIS> entityCollection)
        {
            int number = 0;
            foreach (ObjAPPAREILSDEVIS entity in entityCollection)
            {
                if (Delete(entity))
                {
                    number++;
                }
            }
            return number;
        }

        public static bool Insert(ObjAPPAREILSDEVIS entity)
        {
            SqlConnection connection = new SqlConnection(Session.GetSqlConnexionString());
            SqlCommand command = new SqlCommand("SPX_DEVIS_APPAREILSDEVIS_INSERER", connection);
            command.CommandType = CommandType.StoredProcedure;

            int rowsAffected = -1;

            command.Parameters.Add("@NumDevis", SqlDbType.VarChar, 8).Value = entity.NumDevis;
            command.Parameters.Add("@CodeAppareil", SqlDbType.Int).Value = entity.CodeAppareil;
            command.Parameters.Add("@Nbre", SqlDbType.Int).Value = entity.Nbre;
            command.Parameters.Add("@Puissance", SqlDbType.Int).Value = entity.Puissance;

            if (command.Connection.State == ConnectionState.Closed)
                command.Connection.Open();

            try
            {

                SetDBNullParametre(command.Parameters);
                rowsAffected = command.ExecuteNonQuery();

                return Convert.ToBoolean(rowsAffected);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                command.Dispose();
                connection.Dispose();
            }
        }

        public static bool Insert(List<ObjAPPAREILSDEVIS> entityCollection)
        {
            int number = 0;
            foreach (ObjAPPAREILSDEVIS entity in entityCollection)
            {
                if (entity.Nbre == null && (entity.Nbre != 0))
                {
                    if (Insert(entity))
                    {
                        number++;
                    }
                }
            }
            return number > 0;
        }

        public static bool Insert(ObjAPPAREILSDEVIS entity, SqlCommand pCommand)
        {
            int rowsAffected = -1;
            try
            {
                pCommand.Parameters.Clear();
                pCommand.CommandText = ".SPX_DEVIS_APPAREILSDEVIS_INSERER";
                pCommand.CommandType = CommandType.StoredProcedure;

                pCommand.Parameters.Add("@NumDevis", SqlDbType.VarChar, 8).Value = entity.NumDevis;
                pCommand.Parameters.Add("@CodeAppareil", SqlDbType.Int).Value = entity.CodeAppareil;
                pCommand.Parameters.Add("@Nbre", SqlDbType.Int).Value = entity.Nbre;
                pCommand.Parameters.Add("@Puissance", SqlDbType.Int).Value = entity.Puissance;

                if (pCommand.Connection.State == ConnectionState.Closed)
                    pCommand.Connection.Open();

                SetDBNullParametre(pCommand.Parameters);
                rowsAffected = pCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Convert.ToBoolean(rowsAffected);
        }

        public static bool Delete(string pIdDevis, SqlCommand pCommand)
        {
            int rowsAffected = -1;
            try
            {
                pCommand.Parameters.Clear();
                pCommand.CommandText = "SPX_DEVIS_APPAREILSDEVIS_SUPPRIMER";
                pCommand.CommandType = CommandType.StoredProcedure;
                SqlParameter param = pCommand.Parameters.Add(new SqlParameter("@NumDevis", pNumDevis));
                param.Direction = ParameterDirection.Input;

                //Object datareader
                rowsAffected = pCommand.ExecuteNonQuery();

                return Convert.ToBoolean(rowsAffected);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static bool Insert(List<ObjAPPAREILSDEVIS> entityCollection, SqlCommand pCommand)
        {
            int number = 0;
            foreach (ObjAPPAREILSDEVIS entity in entityCollection)
            {
                if (Insert(entity, pCommand))
                {
                    number++;
                }
            }
            return number != 0;
        }
        */

        public static List<ObjAPPAREILSDEVIS> GetByNumDevis(int pIdDevis)
        {
            try
            {
                return Entities.GetEntityListFromQuery<ObjAPPAREILSDEVIS>(DevisProcedures.DEVIS_APPAREILSDEVIS_RETOURNEByDevisId(pIdDevis));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<ObjAPPAREILSDEVIS> GetByAll()
        {
            try
            {
                return Entities.GetEntityListFromQuery<ObjAPPAREILSDEVIS>(DevisProcedures.DEVIS_APPAREILSDEVIS_RETOURNE());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool Delete(ObjAPPAREILSDEVIS entity)
        {
            try
            {
                return Entities.DeleteEntity<Galatee.Entity.Model.APPAREILSDEVIS>(Entities.ConvertObject<Galatee.Entity.Model.APPAREILSDEVIS, ObjAPPAREILSDEVIS>(entity));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool Delete(List<ObjAPPAREILSDEVIS> entityCollection)
        {
            try
            {
                return Entities.DeleteEntity<Galatee.Entity.Model.APPAREILSDEVIS>(Entities.ConvertObject<Galatee.Entity.Model.APPAREILSDEVIS, ObjAPPAREILSDEVIS>(entityCollection));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool Delete(List<ObjAPPAREILSDEVIS> entityCollection , galadbEntities pCommand)
        {
            try
            {
                return Entities.DeleteEntity<Galatee.Entity.Model.APPAREILSDEVIS>(Entities.ConvertObject<Galatee.Entity.Model.APPAREILSDEVIS, ObjAPPAREILSDEVIS>(entityCollection), pCommand);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool Insert(ObjAPPAREILSDEVIS entity)
        {
            try
            {
                return Entities.InsertEntity<Galatee.Entity.Model.APPAREILSDEVIS>(Entities.ConvertObject<Galatee.Entity.Model.APPAREILSDEVIS, ObjAPPAREILSDEVIS>(entity));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool Insert(List<ObjAPPAREILSDEVIS> pEntityCollection)
        {
            try
            {
                return Entities.InsertEntity<Galatee.Entity.Model.APPAREILSDEVIS>(Entities.ConvertObject<Galatee.Entity.Model.APPAREILSDEVIS, ObjAPPAREILSDEVIS>(pEntityCollection));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool Insert(List<ObjAPPAREILSDEVIS> pEntityCollection, galadbEntities pCommand)
        {
            try
            {
                return Entities.InsertEntity<Galatee.Entity.Model.APPAREILSDEVIS>(Entities.ConvertObject<Galatee.Entity.Model.APPAREILSDEVIS, ObjAPPAREILSDEVIS>(pEntityCollection),pCommand);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static List<APPAREILSDEVIS> GetAppareil(List<ObjAPPAREILSDEVIS> pEntityCollection)
        {
            try
            {
                return Entities.ConvertObject<Galatee.Entity.Model.APPAREILSDEVIS, ObjAPPAREILSDEVIS>(pEntityCollection);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static bool Update(ObjAPPAREILSDEVIS entity)
        {
            try
            {
                return Entities.UpdateEntity<Galatee.Entity.Model.APPAREILSDEVIS>(Entities.ConvertObject<Galatee.Entity.Model.APPAREILSDEVIS, ObjAPPAREILSDEVIS>(entity));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool Update(List<ObjAPPAREILSDEVIS> pEntityCollection)
        {
            try
            {
                return Entities.UpdateEntity<Galatee.Entity.Model.APPAREILSDEVIS>(Entities.ConvertObject<Galatee.Entity.Model.APPAREILSDEVIS, ObjAPPAREILSDEVIS>(pEntityCollection));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }  
	}
}