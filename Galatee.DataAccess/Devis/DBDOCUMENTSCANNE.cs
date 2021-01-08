
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
	[DataObject]
    public partial class DBDOCUMENTSCANNE : DBBase
	{
        /*
        private static List<ObjDOCUMENTSCANNE> Fill(IDataReader reader, List<ObjDOCUMENTSCANNE> rows, int start, int pageLength)
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

                ObjDOCUMENTSCANNE c = new ObjDOCUMENTSCANNE();
                c.Id = (Convert.IsDBNull(reader["Id"])) ? Guid.Empty : (System.Guid)reader["Id"];
                c.OriginalId = (Convert.IsDBNull(reader["Id"])) ? Guid.Empty : (System.Guid)reader["Id"];
                c.NomDocument = (Convert.IsDBNull(reader["NomDocument"])) ? string.Empty : (System.String)reader["NomDocument"];
                c.Contenu = (Convert.IsDBNull(reader["Contenu"])) ? new byte[] { } : (System.Byte[])reader["Contenu"];
                rows.Add(c);
            }
            return rows;
        }

        public static ObjDOCUMENTSCANNE GetDocumentScanneById(Guid pId)
        {
            ObjDOCUMENTSCANNE row = new ObjDOCUMENTSCANNE();
            string connectString = Session.GetSqlConnexionString();

            SqlConnection connection = new SqlConnection(connectString);
            //Objet Command
            SqlCommand command = new SqlCommand("SPX_DEVIS_DOCUMENTSCANNE_RETOURNEById", connection);
            command.CommandType = CommandType.StoredProcedure;

            SqlParameter param = command.Parameters.Add(new SqlParameter("@Id", pId));
            param.Direction = ParameterDirection.Input;

            try
            {
                //Ouverture
                if (connection.State == ConnectionState.Closed)
                    connection.Open();

                //Object datareader
                SqlDataReader reader = command.ExecuteReader();

                List<ObjDOCUMENTSCANNE> tmp = new List<ObjDOCUMENTSCANNE>();
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
                connection.Dispose();
                command.Dispose();
            }
            return row;
        }

        public static bool InsertDocumentScanne(SqlCommand command, ObjDOCUMENTSCANNE entity)
        {
            //Objet Command
            int rowsAffected = -1;
            command.CommandText = "SPX_DEVIS_DOCUMENTSCANNE_INSERER";
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.Clear();

            command.Parameters.Add("@Id", SqlDbType.UniqueIdentifier).Value = entity.Id;
            command.Parameters.Add("@NomDocument", SqlDbType.VarChar, 63).Value = entity.NomDocument;
            command.Parameters.Add("@Contenu", SqlDbType.Image).Value = entity.Contenu;

            SetDBNullParametre(command.Parameters);

            try
            {
                rowsAffected = command.ExecuteNonQuery();
                entity.OriginalId = entity.Id;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return Convert.ToBoolean(rowsAffected);
        }

        public static bool UpdateDocumentScanne(SqlCommand command, ObjDOCUMENTSCANNE entity)
        {
            //Objet Command
            int rowsAffected = -1;
            command.CommandText = "SPX_DEVIS_DOCUMENTSCANNE_UPDATE";
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.Clear();

            command.Parameters.Add("@Id", SqlDbType.UniqueIdentifier).Value = entity.Id;
            command.Parameters.Add("@OriginalId", SqlDbType.UniqueIdentifier).Value = entity.OriginalId;
            command.Parameters.Add("@NomDocument", SqlDbType.VarChar, 63).Value = entity.NomDocument;
            command.Parameters.Add("@Contenu", SqlDbType.Image).Value = entity.Contenu;

            SetDBNullParametre(command.Parameters);

            try
            {
                rowsAffected = command.ExecuteNonQuery();
                entity.OriginalId = entity.Id;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return Convert.ToBoolean(rowsAffected);
        }

        public static bool DeleteDocumentScanne(SqlCommand command, ObjDOCUMENTSCANNE entity)
        {
            //Objet Command
            int rowsAffected = -1;
            command.CommandText = "SPX_DEVIS_DOCUMENTSCANNE_SUPPRIMER";
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.Clear();

            command.Parameters.AddWithValue("@Id", entity.Id);

            try
            {
                rowsAffected =  command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return Convert.ToBoolean(rowsAffected);
        }
        */

        public static ObjDOCUMENTSCANNE GetDocumentScanneById(Guid pId)
        {
            try
            {
                return Entities.GetEntityFromQuery<ObjDOCUMENTSCANNE>(DevisProcedures.DEVIS_DOCUMENTSCANNE_RETOURNEById(pId));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool InsertDocumentScanne(galadbEntities pCommand, ObjDOCUMENTSCANNE entity)
        {
            try
            {
                return Entities.InsertEntity<Galatee.Entity.Model.DOCUMENTSCANNE>(Entities.ConvertObject<Galatee.Entity.Model.DOCUMENTSCANNE, ObjDOCUMENTSCANNE>(entity), pCommand);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool UpdateDocumentScanne(galadbEntities pCommand, ObjDOCUMENTSCANNE entity)
        {
            try
            {
                return Entities.UpdateEntity<Galatee.Entity.Model.DOCUMENTSCANNE>(Entities.ConvertObject<Galatee.Entity.Model.DOCUMENTSCANNE, ObjDOCUMENTSCANNE>(entity), pCommand);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool DeleteDocumentScanne(galadbEntities pCommand, ObjDOCUMENTSCANNE entity)
        {
            try
            {
                return Entities.DeleteEntity<Galatee.Entity.Model.DOCUMENTSCANNE>(Entities.ConvertObject<Galatee.Entity.Model.DOCUMENTSCANNE, ObjDOCUMENTSCANNE>(entity), pCommand);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
	}
}