
#region Using directives

using System;
using System.Data;
using System.Collections;
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.ComponentModel;
using System.Data.SqlClient;
using Galatee.Structure;
using System.Collections.Generic;
using Galatee.DataAccess;
using Galatee.Entity.Model;

#endregion

namespace Galatee.DataAccess 
{
	///<summary>
	/// This class is the Data Access Logic Component implementation for the <see cref="DEVISPIA"/> business entity.
	///</summary>
	[DataObject]
    public partial class DBDEVISPIA : DBBase
	{
        /*
        public static List<ObjDEVISPIA> FillWithName(IDataReader reader, List<ObjDEVISPIA> rows, int start, int pageLength)
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

                ObjDEVISPIA c = new ObjDEVISPIA();
                c.NumDevis = (Convert.IsDBNull(reader["NumDevis"])) ? string.Empty : (System.String)reader["NumDevis"];
                c.OriginalNumDevis = (Convert.IsDBNull(reader["NumDevis"])) ? string.Empty : (System.String)reader["NumDevis"];
                c.Ordre = (Convert.IsDBNull(reader["Ordre"])) ? (byte)0 : (System.Byte)reader["Ordre"];
                c.OriginalOrdre = (Convert.IsDBNull(reader["Ordre"])) ? (byte)0 : (System.Byte)reader["Ordre"];
                c.MatriculePIA = (Convert.IsDBNull(reader["MatriculePIA"])) ? string.Empty : (System.String)reader["MatriculePIA"];
                if (Convert.IsDBNull(reader["DatePIA"]))
                    c.DatePIA = null;
                else
                    c.DatePIA = (System.DateTime)reader["DatePIA"];
                c.NomMetreur = (Convert.IsDBNull(reader["NomMetreur"])) ? string.Empty : (System.String)reader["NomMetreur"];
                rows.Add(c);
            }
            return rows;
        }

        public static List<ObjDEVISPIA> Fill(IDataReader reader, List<ObjDEVISPIA> rows, int start, int pageLength)
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

                ObjDEVISPIA c = new ObjDEVISPIA();
                c.NumDevis = (Convert.IsDBNull(reader["NumDevis"])) ? string.Empty : (System.String)reader["NumDevis"];
                c.OriginalNumDevis = (Convert.IsDBNull(reader["NumDevis"])) ? string.Empty : (System.String)reader["NumDevis"];
                c.Ordre = (Convert.IsDBNull(reader["Ordre"])) ? (byte)0 : (System.Byte)reader["Ordre"];
                c.OriginalOrdre = (Convert.IsDBNull(reader["Ordre"])) ? (byte)0 : (System.Byte)reader["Ordre"];
                c.MatriculePIA = (Convert.IsDBNull(reader["MatriculePIA"])) ? string.Empty : (System.String)reader["MatriculePIA"];
                if (Convert.IsDBNull(reader["DatePIA"]))
                    c.DatePIA = null;
                else
                    c.DatePIA = (System.DateTime)reader["DatePIA"];
                rows.Add(c);
            }
            return rows;
        }


        public static ObjDEVISPIA GetByNumDevisOrdre(string numDevis, byte ordre)
        {
            ObjDEVISPIA row = new ObjDEVISPIA();
            string connectString = Session.GetSqlConnexionString();

            //Objet connection
            SqlConnection connection = new SqlConnection(connectString);
            //Objet Command
            SqlCommand command = new SqlCommand("SPX_DEVIS_DEVISPIA_RETOURNEByNumDevisOrdre", connection);
            command.CommandType = CommandType.StoredProcedure;

            SqlParameter param = command.Parameters.Add(new SqlParameter("@NumDevis", numDevis));
            param.Direction = ParameterDirection.Input;
            SqlParameter param1 = command.Parameters.Add(new SqlParameter("@Ordre", ordre));
            param1.Direction = ParameterDirection.Input;

            try
            {
                if (connection.State == ConnectionState.Closed)
                    connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                List<ObjDEVISPIA> tmp = new List<ObjDEVISPIA>();
                FillWithName(reader, tmp, 0, int.MaxValue);
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
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                connection.Dispose();
                command.Dispose();
            }
            return row;
        }

        public static bool Insert(SqlCommand command, ObjDEVISPIA entity)
        {
            //Objet Command
            int rowsAffected = -1;
            command.CommandText = "SPX_DEVIS_DEVISPIA_INSERER";
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.Clear();

            command.Parameters.Add("@NumDevis", SqlDbType.VarChar, 8).Value = entity.NumDevis;
            command.Parameters.Add("@Ordre", SqlDbType.TinyInt).Value = entity.Ordre;
            command.Parameters.Add("@MatriculePIA", SqlDbType.VarChar, 5).Value = entity.MatriculePIA;
            command.Parameters.Add("@DatePIA", SqlDbType.DateTime).Value = entity.DatePIA;

            SetDBNullParametre(command.Parameters);

            try
            {
                rowsAffected = command.ExecuteNonQuery();
                entity.OriginalNumDevis = entity.NumDevis;
                entity.OriginalOrdre = entity.Ordre;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return Convert.ToBoolean(rowsAffected);
        }

        public static bool Delete(SqlCommand command, ObjDEVIS entity)
        {
            //Objet Command
            int rowsAffected = -1;
            command.CommandText = "SPX_DEVIS_DEVISPIA_SUPPRIMER";
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Clear();

            command.Parameters.Add("@NumDevis", SqlDbType.VarChar, 8).Value = entity.PK_NUMDEVIS;
            command.Parameters.Add("@Ordre", SqlDbType.TinyInt).Value = entity.ORDRE;

            try
            {
                rowsAffected = command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return Convert.ToBoolean(rowsAffected);
        }
         
          */

        public static ObjDEVISPIA GetByDevisIdOrdre(int IdDevis, int ordre)
        {
            try
            {
                return Entities.GetEntityFromQuery<ObjDEVISPIA>(DevisProcedures.DEVIS_DEVISPIA_RETOURNEByDevisIdOrdre(IdDevis, ordre));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static ObjDEVISPIA GetById(int pId)
        {
            try
            {
                return Entities.GetEntityFromQuery<ObjDEVISPIA>(DevisProcedures.DEVIS_DEVISPIA_RETOURNEById(pId));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public static bool Insert(galadbEntities pCommand, ObjDEVISPIA entity)
        //{
        //    try
        //    {
        //        return Entities.InsertEntity<Galatee.Entity.Model.DEVISPIA>(Entities.ConvertObject<Galatee.Entity.Model.DEVISPIA, ObjDEVISPIA>(entity), pCommand);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public static bool Delete(galadbEntities pCommand, ObjDEVISPIA entity)
        //{
        //    try
        //    {
        //        return Entities.DeleteEntity<Galatee.Entity.Model.DEVISPIA>(Entities.ConvertObject<Galatee.Entity.Model.DEVISPIA, ObjDEVISPIA>(entity), pCommand);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public static bool update(galadbEntities pCommand, ObjDEVISPIA entity)
        //{
        //    try
        //    {
        //        return Entities.UpdateEntity<Galatee.Entity.Model.DEVISPIA>(Entities.ConvertObject<Galatee.Entity.Model.DEVISPIA, ObjDEVISPIA>(entity), pCommand);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public static bool InsertOrUpdate(galadbEntities pCommand, ObjDEVISPIA entity)
        //{
        //    try
        //    {
        //        DataTable dt = DevisProcedures.DEVIS_DEVISPIA_RETOURNEByDevisIdOrdre(entity.FK_IDDEVIS, entity.ORDRE);
        //        if (dt != null && dt.Rows.Count > 0)
        //        {
        //            var devisPia = Entities.GetEntityFromQuery<ObjDEVISPIA>(dt);
        //            if (devisPia != null)
        //                entity.PK_ID = devisPia.PK_ID;
        //            return Entities.UpdateEntity<Galatee.Entity.Model.DEVISPIA>(Entities.ConvertObject<Galatee.Entity.Model.DEVISPIA, ObjDEVISPIA>(entity), pCommand);
        //        }
        //        else
        //            return Entities.InsertEntity<Galatee.Entity.Model.DEVISPIA>(Entities.ConvertObject<Galatee.Entity.Model.DEVISPIA, ObjDEVISPIA>(entity), pCommand);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
	}
}