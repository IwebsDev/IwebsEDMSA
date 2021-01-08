using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Galatee.Structure;
using Galatee.Entity.Model;

namespace Galatee.DataAccess
{
     public class DBAdmRoles : Galatee.DataAccess.Parametrage.DbBase
    {
        public DBAdmRoles()
        {
            //ConnectionString = Session.GetSqlConnexionString();
            
        }

        /// </summary>
        private string ConnectionString;
        private SqlConnection cn = null;
        /// <summary>
        /// _Transaction
        /// </summary>
        private bool _Transaction;
        /// <summary>
        /// Transaction
        /// </summary>
        public bool Transaction
        {
            get { return _Transaction; }
            set { _Transaction = value; }

        }

        private SqlCommand cmd = null;
        public bool Update(CsAdmRoles admRoles)
        {
            cn = new SqlConnection(ConnectionString);
            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "spx_AdmRoles_Update";
            cmd.Parameters.Clear();
            cmd.Parameters.Add("@RoleID", SqlDbType.VarChar).Value = admRoles.RoleID;
            cmd.Parameters.Add("@RoleName", SqlDbType.VarChar).Value = admRoles.RoleName;
            cmd.Parameters.Add("@RoleDisplayName", SqlDbType.VarChar).Value = admRoles.RoleDisplayName;

            DBBase.SetDBNullParametre(cmd.Parameters);

            try
            {

                if (cn.State == ConnectionState.Closed)
                    cn.Open();
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                    cn.Close(); // Fermeture de la connection 
                cmd.Dispose();
            }
        }

        public bool Insert(CsAdmRoles admRoles)
        {
            cn = new SqlConnection(ConnectionString);
            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "spx_AdmRoles_Insert";
            cmd.Parameters.Clear();
            cmd.Parameters.Add("@Centre", SqlDbType.VarChar).Value = admRoles.Centre;
            cmd.Parameters.Add("@RoleID", SqlDbType.VarChar).Value = admRoles.RoleID;
            cmd.Parameters.Add("@CodeFonc", SqlDbType.VarChar).Value = admRoles.CodeFonc;
            cmd.Parameters.Add("@RoleName", SqlDbType.VarChar).Value = admRoles.RoleName;
            cmd.Parameters.Add("@RoleDisplayName", SqlDbType.VarChar).Value = admRoles.RoleDisplayName;
            
            DBBase.SetDBNullParametre(cmd.Parameters);

            try
            {

                if (cn.State == ConnectionState.Closed)
                    cn.Open();
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                    cn.Close(); // Fermeture de la connection 
                cmd.Dispose();
            }
        }

        public bool Delete(Guid ID, string CodeFonc)
        {
            cn = new SqlConnection(ConnectionString);
            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "spx_AdmRoles_Delete";
            cmd.Parameters.Clear();
            cmd.Parameters.Add("@RoleID", SqlDbType.VarChar).Value = ID;
            cmd.Parameters.Add("@CodeFonc", SqlDbType.VarChar).Value = CodeFonc;


            DBBase.SetDBNullParametre(cmd.Parameters);

            try
            {

                if (cn.State == ConnectionState.Closed)
                    cn.Open();
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw ex; 

            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                    cn.Close(); // Fermeture de la connection 
                cmd.Dispose();
            }
        }

        //public List<CsAdmRoles> GetAll()
        //{
        //    //cmd.CommandText = "spx_AdmRoles_GetAll";

        //    List<CsAdmRoles> rows = new List<CsAdmRoles>();

        //    try
        //    {
                
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        private List<CsAdmRoles> Fill(SqlDataReader reader, List<CsAdmRoles> rows, int start, int pageLength)
        {
            try
            {
                for (int i = 0; i < start; i++)
                {
                    if (!reader.Read())
                        return rows;
                }

                for (int i = 0; i < pageLength; i++)
                {
                    if (!reader.Read())
                        break;

                    CsAdmRoles c = new CsAdmRoles();
                    c.RoleID = (Convert.IsDBNull(reader["RoleID"])) ? Guid.Empty : (System.Guid)reader["RoleID"];
                    c.CodeFonc = (Convert.IsDBNull(reader["CodeFonc"])) ? string.Empty : (System.String)reader["CodeFonc"];
                    if (!string.IsNullOrEmpty(c.CodeFonc)) c.CodeFonc = c.CodeFonc.Substring(3, 3);
                    c.Centre = (Convert.IsDBNull(reader["Centre"])) ? string.Empty : (System.String)reader["Centre"];
                    c.RoleName = (Convert.IsDBNull(reader["RoleName"])) ? string.Empty : (System.String)reader["RoleName"];
                    c.RoleDisplayName = (Convert.IsDBNull(reader["RoleDisplayName"])) ? string.Empty : (System.String)reader["RoleDisplayName"];

                    rows.Add(c);
                }

                return rows;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public List<CsAdmRoles> GetAssocitedToMenuByMenuID(int menuId)
        {
            cn = new SqlConnection(ConnectionString);
            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "spx_AdmRoles_GetAssocitedToMenuByMenuID";
            cmd.Parameters.Clear();
            cmd.Parameters.Add("@MenuID", SqlDbType.VarChar).Value = menuId;

            DBBase.SetDBNullParametre(cmd.Parameters);
            
            SqlDataReader reader = null;

            List<CsAdmRoles> rows = new List<CsAdmRoles>();

            try
            {
                reader = cmd.ExecuteReader();
                Fill(reader, rows, 0, int.MaxValue);
                reader.Close();

                return rows;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                cmd.Dispose();
            }
        }

        public void Delete(CsAdmRoles admRoles)
        {
            Delete(admRoles.RoleID, admRoles.CodeFonc);
        }


    #region ADO .Net from Entity : Stephen 26-01-2019

        public List<CsSite> GetAllSite()
        {
            //cmd.CommandText = "spx_AdmRoles_GetAllBranche";
            try
            {
                //DataTable dt = Galatee.Entity.Model.CommonProcedures.RetourneTousSites();
                DataTable dt = DB_ParametresGeneraux.SelectAllDonneReference("SITE");
                return Entities.GetEntityListFromQuery<CsSite >(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    #endregion


    }
}
