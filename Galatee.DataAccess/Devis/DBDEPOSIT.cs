

#region Using directives

using System;
using System.Collections.Generic;
using System.Data;
using System.Collections;
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.ComponentModel;
using Galatee.Structure ;
using System.Data.SqlClient;
using Galatee.DataAccess;
using Galatee.Entity.Model;
//using Galatee.Structure;

#endregion

namespace Galatee.DataAccess
{
	///<summary>
	/// This class is the Data Access Logic Component implementation for the <see cref="DEVISPIA"/> business entity.
	///</summary>
	[DataObject]
    public partial class DBDEPOSIT : DBBase
	{
        /*
        private static List<ObjDEPOSIT> Fill(IDataReader reader, List<ObjDEPOSIT> rows, int start, int pageLength)
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

                ObjDEPOSIT c = new ObjDEPOSIT();
                c.Id = (Convert.IsDBNull(reader["Id"])) ? (long)0 : (long)reader["Id"];
                c.CENTRE = (Convert.IsDBNull(reader["CENTRE"])) ? string.Empty : (System.String)reader["CENTRE"];
                c.CLIENT = (Convert.IsDBNull(reader["CLIENT"])) ? string.Empty : (System.String)reader["CLIENT"];
                c.ORDRE = (Convert.IsDBNull(reader["ORDRE"])) ? string.Empty : (System.String)reader["ORDRE"];
                c.DEPOSIT = (Convert.IsDBNull(reader["DEPOSIT"])) ? (decimal)0 : (decimal)reader["DEPOSIT"];
                c.RECEIPT = (Convert.IsDBNull(reader["RECEIPT"])) ? string.Empty : (System.String)reader["RECEIPT"];
                c.DATEENC = (Convert.IsDBNull(reader["DATEENC"])) ? DateTime.MinValue : (DateTime)reader["DATEENC"];
                c.NUMDEVIS = (Convert.IsDBNull(reader["NUMDEVIS"])) ? null : (System.String)reader["NUMDEVIS"];
                c.NOM = (Convert.IsDBNull(reader["NOM"])) ? null : (System.String)reader["NOM"];
                c.TOTAL = (Convert.IsDBNull(reader["TOTAL"])) ? (decimal)0 : (decimal)reader["TOTAL"];
                c.IDENTITE = (Convert.IsDBNull(reader["IDENTITE"])) ? null : (System.String)reader["IDENTITE"];
                c.TOPANNUL = (Convert.IsDBNull(reader["TOPANNUL"])) ? null : (System.String)reader["TOPANNUL"];
                c.MONTANTTVA = (Convert.IsDBNull(reader["MONTANTTVA"])) ? (decimal)0 : (decimal)reader["MONTANTTVA"];
                c.BANQUE = (Convert.IsDBNull(reader["BANQUE"])) ? null : (System.String)reader["BANQUE"];
                c.COMPTE = (Convert.IsDBNull(reader["COMPTE"])) ? null : (System.String)reader["COMPTE"];
                c.IDLETTER = (Convert.IsDBNull(reader["IDLETTER"])) ? Guid.Empty : (System.Guid)reader["IDLETTER"];
                c.ISCREATED = !(Convert.IsDBNull(reader["ISCREATED"])) && (System.Boolean)reader["ISCREATED"];
                rows.Add(c);
            }
            return rows;
        }

        public static List<ObjDEPOSIT> SearchByReceipt(string receipt)
        {
            SqlConnection connection = new SqlConnection(Session.GetSqlConnexionString());
            //Objet Command
            SqlCommand command = new SqlCommand("SPX_DEVIS_DEPOSIT_RETOURNEByReceipt", connection);
            command.CommandType = CommandType.StoredProcedure;

            SqlParameter param = command.Parameters.Add(new SqlParameter("@Receipt", receipt));
            param.Direction = ParameterDirection.Input;
            SetDBNullParametre(command.Parameters);
            try
            {
                //Ouverture
                if (connection.State == ConnectionState.Closed)
                    connection.Open();

                //Object datareader
                SqlDataReader reader = command.ExecuteReader();
                List<ObjDEPOSIT> tmp = new List<ObjDEPOSIT>();
                Fill(reader, tmp, 0, int.MaxValue);
                reader.Close();

                return tmp;
            }
            catch (Exception ex)
            {
                throw ex;
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

        public static List<ObjDEPOSIT> SearchAllDeposit()
        {
            SqlConnection connection = new SqlConnection(Session.GetSqlConnexionString());

            //Objet Command
            SqlCommand command = new SqlCommand("SPX_DEVIS_DEPOSIT_RETOURNE", connection);
            command.CommandType = CommandType.StoredProcedure;

            try
            {
                //Ouverture
                if (connection.State == ConnectionState.Closed)
                    connection.Open();

                //Object datareader
                SqlDataReader reader = command.ExecuteReader();

                List<ObjDEPOSIT> tmp = new List<ObjDEPOSIT>();
                Fill(reader, tmp, 0, int.MaxValue);
                reader.Close();

                return tmp;
            }
            catch (Exception ex)
            {
                throw ex;
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

        public static ObjDEPOSIT SearchByNumDevis(string numDevis)
        {
            SqlConnection connection = new SqlConnection(Session.GetSqlConnexionString());

            //Objet Command
            SqlCommand command = new SqlCommand("SPX_DEVIS_DEPOSIT_RETOURNEByNumDevis", connection);
            command.CommandType = CommandType.StoredProcedure;

            SqlParameter param = command.Parameters.Add(new SqlParameter("@NUMDEVIS", numDevis));
            param.Direction = ParameterDirection.Input;

            SetDBNullParametre(command.Parameters);

            try
            {
                //Ouverture
                if (connection.State == ConnectionState.Closed)
                    connection.Open();

                //Object datareader
                SqlDataReader reader = command.ExecuteReader();

                List<ObjDEPOSIT> tmp = new List<ObjDEPOSIT>();
                Fill(reader, tmp, 0, int.MaxValue);
                reader.Close();
                if (tmp.Count == 1)
                {
                    return tmp[0];
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
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

        public static List<ObjDEPOSIT> SearchByNumReceipt(string pReceipt)
        {
            SqlConnection connection = new SqlConnection(Session.GetSqlConnexionString());

            SqlCommand command = new SqlCommand("SPX_DEVIS_DEPOSIT_RETOURNEByReceipt", connection);
            command.CommandType = CommandType.StoredProcedure;

            SqlParameter param = command.Parameters.Add(new SqlParameter("@Receipt", pReceipt));
            param.Direction = ParameterDirection.Input;

            SetDBNullParametre(command.Parameters);

            try
            {
                //Ouverture
                if (connection.State == ConnectionState.Closed)
                    connection.Open();

                //Object datareader
                SqlDataReader reader = command.ExecuteReader();

                List<ObjDEPOSIT> tmp = new List<ObjDEPOSIT>();
                Fill(reader, tmp, 0, int.MaxValue);
                reader.Close();

                return tmp;
            }
            catch (Exception ex)
            {
                throw ex;
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

        public static bool UpdateDeposit(SqlCommand command, ObjDEPOSIT entity)
        {
            //Objet Command
            int rowsAffected = -1;
            command.CommandText = "SPX_DEVIS_DEPOSIT_UPDATE";
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.Clear();

            command.Parameters.Add("@Id", SqlDbType.BigInt).Value = entity.Id;
            command.Parameters.Add("@RECEIPT", SqlDbType.VarChar, 19).Value = entity.RECEIPT;
            command.Parameters.Add("@CENTRE", SqlDbType.VarChar, 3).Value = entity.CENTRE;
            command.Parameters.Add("@CLIENT", SqlDbType.VarChar, 20).Value = entity.CLIENT;
            command.Parameters.Add("@ORDRE", SqlDbType.VarChar, 2).Value = entity.ORDRE;
            command.Parameters.Add("@NUMDEVIS", SqlDbType.VarChar, 8).Value = entity.NUMDEVIS;
            command.Parameters.Add("@NOM", SqlDbType.VarChar, 63).Value = entity.NOM;
            command.Parameters.Add("@TOTAL", SqlDbType.Decimal).Value = entity.TOTAL;
            command.Parameters.Add("@MONTANTTVA", SqlDbType.Decimal).Value = entity.MONTANTTVA;
            command.Parameters.Add("@BANQUE", SqlDbType.VarChar, 31).Value = entity.BANQUE;
            command.Parameters.Add("@COMPTE", SqlDbType.VarChar, 31).Value = entity.COMPTE;
            command.Parameters.Add("@IDLETTER", SqlDbType.UniqueIdentifier).Value = entity.IDLETTER != Guid.Empty ?  entity.IDLETTER : null;
            command.Parameters.Add("@ISCREATED", SqlDbType.Bit).Value = entity.ISCREATED;
            command.Parameters.Add("@DEPOSIT", SqlDbType.Decimal).Value = entity.DEPOSIT;
            command.Parameters.Add("@DATEENC", SqlDbType.DateTime).Value = entity.DATEENC;
            command.Parameters.Add("@IDENTITE", SqlDbType.VarChar, 15).Value = entity.IDENTITE;
            command.Parameters.Add("@TOPANNUL", SqlDbType.VarChar, 1).Value = entity.TOPANNUL;

            SetDBNullParametre(command.Parameters);

            try
            {
                rowsAffected= command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return Convert.ToBoolean(rowsAffected);
        }

        public static bool InsertDeposit(SqlCommand command, ObjDEPOSIT entity)
        {
            //Objet Command
            int rowsAffected = -1;
            command.CommandText = "SPX_DEVIS_DEPOSIT_INSERER";
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.Clear();

            command.Parameters.Add("@RECEIPT", SqlDbType.VarChar, 19).Value = entity.RECEIPT;
            command.Parameters.Add("@CENTRE", SqlDbType.VarChar, 3).Value = entity.CENTRE;
            command.Parameters.Add("@CLIENT", SqlDbType.VarChar, 20).Value = entity.CLIENT;
            command.Parameters.Add("@ORDRE", SqlDbType.VarChar, 2).Value = entity.ORDRE;
            command.Parameters.Add("@NUMDEVIS", SqlDbType.VarChar, 8).Value = entity.NUMDEVIS;
            command.Parameters.Add("@NOM", SqlDbType.VarChar, 63).Value = entity.NOM;
            command.Parameters.Add("@TOTAL", SqlDbType.Decimal).Value = entity.TOTAL;
            command.Parameters.Add("@MONTANTTVA", SqlDbType.Decimal).Value = entity.MONTANTTVA;
            command.Parameters.Add("@BANQUE", SqlDbType.VarChar, 31).Value = entity.BANQUE;
            command.Parameters.Add("@COMPTE", SqlDbType.VarChar, 31).Value = entity.COMPTE;
            command.Parameters.Add("@IDLETTER", SqlDbType.UniqueIdentifier).Value = entity.IDLETTER;
            command.Parameters.Add("@ISCREATED", SqlDbType.Bit).Value = entity.ISCREATED;
            command.Parameters.Add("@DEPOSIT", SqlDbType.Decimal).Value = entity.DEPOSIT;
            command.Parameters.Add("@DATEENC", SqlDbType.DateTime).Value = entity.DATEENC;
            command.Parameters.Add("@IDENTITE", SqlDbType.VarChar,15).Value = entity.IDENTITE;
            command.Parameters.Add("@TOPANNUL", SqlDbType.VarChar,1).Value = entity.TOPANNUL;
            
            SetDBNullParametre(command.Parameters);

            try
            {
                rowsAffected = command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Convert.ToBoolean(rowsAffected);
        }

        public static void UpdateDeposit(ObjDEPOSIT entity)
        {
            //Objet connection
            string connectString = Session.GetSqlConnexionString();
            //Objet connection
            SqlConnection connection = new SqlConnection(connectString);

            //Objet Command
            SqlCommand command = new SqlCommand("SPX_DEPOSIT_UPDATE", connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.Clear();

            command.Parameters.Add("@Id", SqlDbType.BigInt).Value = entity.Id;
            command.Parameters.Add("@RECEIPT", SqlDbType.VarChar, 19).Value = entity.RECEIPT;
            command.Parameters.Add("@CENTRE", SqlDbType.VarChar, 3).Value = entity.CENTRE;
            command.Parameters.Add("@CLIENT", SqlDbType.VarChar, 20).Value = entity.CLIENT;
            command.Parameters.Add("@ORDRE", SqlDbType.VarChar, 2).Value = entity.ORDRE;
            command.Parameters.Add("@NUMDEVIS", SqlDbType.VarChar, 8).Value = entity.NUMDEVIS;
            command.Parameters.Add("@NOM", SqlDbType.VarChar, 63).Value = entity.NOM;
            command.Parameters.Add("@TOTAL", SqlDbType.Decimal).Value = entity.TOTAL;
            command.Parameters.Add("@MONTANTTVA", SqlDbType.Decimal).Value = entity.MONTANTTVA;
            command.Parameters.Add("@BANQUE", SqlDbType.VarChar, 31).Value = entity.BANQUE;
            command.Parameters.Add("@COMPTE", SqlDbType.VarChar, 31).Value = entity.COMPTE;
            command.Parameters.Add("@IDLETTER", SqlDbType.UniqueIdentifier).Value = entity.IDLETTER;
            command.Parameters.Add("@ISCREATED", SqlDbType.Bit).Value = entity.ISCREATED;
            command.Parameters.Add("@DEPOSIT", SqlDbType.Decimal).Value = entity.DEPOSIT;
            command.Parameters.Add("@DATEENC", SqlDbType.DateTime).Value = entity.DATEENC;
            command.Parameters.Add("@IDENTITE", SqlDbType.VarChar, 15).Value = entity.IDENTITE;
            command.Parameters.Add("@TOPANNUL", SqlDbType.VarChar, 1).Value = entity.TOPANNUL;

            SetDBNullParametre(command.Parameters);

            try
            {
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
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
        

        private static List<ObjDEPOSIT> Fill(IDataReader reader, List<ObjDEPOSIT> rows, int start, int pageLength)
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

                ObjDEPOSIT c = new ObjDEPOSIT();
                c.PK_ID = (Convert.IsDBNull(reader["Id"])) ? (int)0 : (int)reader["Id"];
                c.FK_CENTRE = (Convert.IsDBNull(reader["CENTRE"])) ? string.Empty : (System.String)reader["CENTRE"];
                c.CLIENT = (Convert.IsDBNull(reader["CLIENT"])) ? string.Empty : (System.String)reader["CLIENT"];
                c.ORDRE = (Convert.IsDBNull(reader["ORDRE"])) ? string.Empty : (System.String)reader["ORDRE"];
                c.DEPOSIT = (Convert.IsDBNull(reader["DEPOSIT"])) ? (decimal)0 : (decimal)reader["DEPOSIT"];
                c.RECEIPT = (Convert.IsDBNull(reader["RECEIPT"])) ? string.Empty : (System.String)reader["RECEIPT"];
                c.DATEENC = (Convert.IsDBNull(reader["DATEENC"])) ? DateTime.MinValue : (DateTime)reader["DATEENC"];
                c.FK_NUMDEVIS = (Convert.IsDBNull(reader["NUMDEVIS"])) ? null : (System.String)reader["NUMDEVIS"];
                c.NOM = (Convert.IsDBNull(reader["NOM"])) ? null : (System.String)reader["NOM"];
                c.TOTAL = (Convert.IsDBNull(reader["TOTAL"])) ? (decimal)0 : (decimal)reader["TOTAL"];
                c.IDENTITE = (Convert.IsDBNull(reader["IDENTITE"])) ? null : (System.String)reader["IDENTITE"];
                c.TOPANNUL = (Convert.IsDBNull(reader["TOPANNUL"])) ? null : (System.String)reader["TOPANNUL"];
                c.MONTANTTVA = (Convert.IsDBNull(reader["MONTANTTVA"])) ? (decimal)0 : (decimal)reader["MONTANTTVA"];
                c.BANQUE = (Convert.IsDBNull(reader["BANQUE"])) ? null : (System.String)reader["BANQUE"];
                c.COMPTE = (Convert.IsDBNull(reader["COMPTE"])) ? null : (System.String)reader["COMPTE"];
                c.IDLETTER = (Convert.IsDBNull(reader["IDLETTER"])) ? Guid.Empty : (System.Guid)reader["IDLETTER"];
                c.ISCREATED = !(Convert.IsDBNull(reader["ISCREATED"])) && (System.Boolean)reader["ISCREATED"];
                rows.Add(c);
            }
            return rows;
        }
        */
        public static List<ObjDEPOSIT> SearchByReceipt(string pReceipt)
        {
            try
            {
                return Entities.GetEntityListFromQuery<ObjDEPOSIT>(DevisProcedures.DEVIS_DEPOSIT_RETOURNEByReceipt(pReceipt));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<ObjDEPOSIT> SearchAllDeposit()
        {
            try
            {
                return Entities.GetEntityListFromQuery<ObjDEPOSIT>(DevisProcedures.DEVIS_DEPOSIT_RETOURNE());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static ObjDEPOSIT SearchByNumDevis(string pNumDevis)
        {
            try
            {
                return Entities.GetEntityFromQuery<ObjDEPOSIT>(DevisProcedures.DEVIS_DEPOSIT_RETOURNEByNumdevis(pNumDevis));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<ObjDEPOSIT> SearchByNumReceipt(string pReceipt)
        {
            try
            {
                return Entities.GetEntityListFromQuery<ObjDEPOSIT>(DevisProcedures.DEVIS_DEPOSIT_RETOURNEByReceipt(pReceipt));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool UpdateDeposit(galadbEntities pCommand, ObjDEPOSIT entity)
        {
            try
            {
                return Entities.UpdateEntity<Galatee.Entity.Model.DEPOSIT>(Entities.ConvertObject<Galatee.Entity.Model.DEPOSIT, ObjDEPOSIT>(entity), pCommand);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool InsertDeposit(galadbEntities pCommand, ObjDEPOSIT entity)
        {
            try
            {
                return Entities.InsertEntity<Galatee.Entity.Model.DEPOSIT>(Entities.ConvertObject<Galatee.Entity.Model.DEPOSIT, ObjDEPOSIT>(entity), pCommand);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool UpdateDeposit(ObjDEPOSIT entity)
        {
            try
            {
                return Entities.UpdateEntity<Galatee.Entity.Model.DEPOSIT>(Entities.ConvertObject<Galatee.Entity.Model.DEPOSIT, ObjDEPOSIT>(entity));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
	}
}