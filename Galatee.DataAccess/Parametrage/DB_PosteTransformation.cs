using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Galatee.Structure;
using Galatee.Entity.Model;

namespace Galatee.DataAccess
{
    public class DB_PosteTransformation /*: Galatee.DataAccess.Parametrage.DbBase*/
    {

        //public List<CsPosteTransformation> SelectAllPosteTransformation()
        //{
        //    try
        //    {
        //        return Entities.GetEntityListFromQuery<CsPosteTransformation>(ParamProcedure.PARAM_POSTETRANSFORMATION_RETOURNE());
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public bool Delete(CsPosteTransformation pCodePoste)
        //{
        //    try
        //    {
        //        return Entities.DeleteEntity<Galatee.Entity.Model.POSTETRANSFORMATION>(Entities.ConvertObject<Galatee.Entity.Model.POSTETRANSFORMATION, CsPosteTransformation>(pCodePoste));
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public bool Delete(List<CsPosteTransformation> pCodePosteCollection)
        {
            try
            {
                //return Entities.DeleteEntity<Galatee.Entity.Model.POSTETRANSFORMATION>(Entities.ConvertObject<Galatee.Entity.Model.POSTETRANSFORMATION, CsPosteTransformation>(pCodePosteCollection));

                foreach (CsPosteTransformation st in pCodePosteCollection)
                {
                    if (!Delete(st))
                        return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Update(CsPosteTransformation pCodePoste)
        {
            try
            {
                return InsertOrUpdate(pCodePoste);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Update(List<CsPosteTransformation> pCodePosteCollection)
        {
            try
            {
                foreach (CsPosteTransformation st in pCodePosteCollection)
                {
                    if (!InsertOrUpdate(st))
                        return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Insert(CsPosteTransformation pCodePoste)
        {
            try
            {
                return InsertOrUpdate(pCodePoste);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Insert(List<CsPosteTransformation> pCodePosteCollection)
        {
            try
            {
                foreach (CsPosteTransformation st in pCodePosteCollection)
                {
                    if (!InsertOrUpdate(st))
                        return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CsPosteTransformation> SelectAllPosteTransformation()
        {
            SqlConnection cn = new SqlConnection(Session.GetSqlConnexionString());
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandTimeout = 1800;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SPX_ACC_GET_POSTETRANSFORMATION";
            if (cmd.Parameters != null && cmd.Parameters.Count != 0) cmd.Parameters.Clear();
            DBBase.SetDBNullParametre(cmd.Parameters);
            try
            {
                if (cn.State == ConnectionState.Closed)
                    cn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);
                return Entities.GetEntityListFromQuery<CsPosteTransformation>(dt);
            }
            catch (Exception ex)
            {
                throw new Exception(cmd.CommandText + ":" + ex.Message);
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                    cn.Close(); // Fermeture de la connection 
                cmd.Dispose();
            }
        }


        public bool Delete(CsPosteTransformation del)
        {
            SqlConnection cn = new SqlConnection(Session.GetSqlConnexionString());
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandTimeout = 1800;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SPX_ACC_DELETE_POSTETRANSFORMATION";
            if (cmd.Parameters != null && cmd.Parameters.Count != 0) cmd.Parameters.Clear();

            cmd.Parameters.Add("@PK_ID", SqlDbType.Int).Value = del.PK_ID;

            DBBase.SetDBNullParametre(cmd.Parameters);
            try
            {
                if (cn.State == ConnectionState.Closed)
                    cn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                return !reader.HasRows;
            }
            catch (Exception ex)
            {
                throw new Exception(cmd.CommandText + ":" + ex.Message);
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                    cn.Close(); // Fermeture de la connection 
                cmd.Dispose();
            }
        }



        public bool InsertOrUpdate(CsPosteTransformation st)
        {
            SqlConnection cn = new SqlConnection(Session.GetSqlConnexionString());
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandTimeout = 1800;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SPX_ACC_INSERTORUPDATE_POSTETRANSFORMATION";
            if (cmd.Parameters != null && cmd.Parameters.Count != 0) cmd.Parameters.Clear();

            cmd.Parameters.Add("@PK_ID", SqlDbType.Int).Value = st.PK_ID;
            cmd.Parameters.Add("@CODE", SqlDbType.VarChar, 6).Value = st.CODE;
            cmd.Parameters.Add("@LIBELLE", SqlDbType.VarChar, 50).Value = st.LIBELLE;
            cmd.Parameters.Add("@USERCREATION", SqlDbType.VarChar, 20).Value = st.USERCREATION;
            cmd.Parameters.Add("@USERMODIFICATION", SqlDbType.VarChar, 20).Value = st.USERMODIFICATION;
            cmd.Parameters.Add("@FK_IDDEPARTHTA", SqlDbType.Int).Value = st.FK_IDDEPARTHTA;

            DBBase.SetDBNullParametre(cmd.Parameters);
            try
            {
                if (cn.State == ConnectionState.Closed)
                    cn.Open();
                return (cmd.ExecuteNonQuery() > 0);
            }
            catch (Exception ex)
            {
                throw new Exception(cmd.CommandText + ":" + ex.Message);
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                    cn.Close(); // Fermeture de la connection 
                cmd.Dispose();
            }
        }


    }
}
