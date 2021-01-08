using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Galatee.Structure;
using Galatee.Entity.Model;

namespace Galatee.DataAccess
{
    public class DB_PosteSource 
    {

        //public List<CsPosteSource> SelectAllPosteSource()
        //{
        //    try
        //    {
        //        return Entities.GetEntityListFromQuery<CsPosteSource>(ParamProcedure.PARAM_POSTESOURCE_RETOURNE());
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public bool Delete(CsPosteSource pPosteSource)
        //{
        //    try
        //    {
        //        return Entities.DeleteEntity<Galatee.Entity.Model.POSTESOURCE>(Entities.ConvertObject<Galatee.Entity.Model.POSTESOURCE, CsPosteSource>(pPosteSource));
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public bool Delete(List<CsPosteSource> pPosteSourceCollection)
        {
            try
            {
                //return Entities.DeleteEntity<Galatee.Entity.Model.POSTESOURCE>(Entities.ConvertObject<Galatee.Entity.Model.POSTESOURCE, CsPosteSource>(pPosteSourceCollection));

                foreach (CsPosteSource st in pPosteSourceCollection)
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

        public bool Update(CsPosteSource pPosteSource)
        {
            try
            {
                return Entities.UpdateEntity<Galatee.Entity.Model.POSTESOURCE>(Entities.ConvertObject<Galatee.Entity.Model.POSTESOURCE, CsPosteSource>(pPosteSource));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Update(List<CsPosteSource> pPosteSourceCollection)
        {
            try
            {
                return Entities.UpdateEntity<Galatee.Entity.Model.POSTESOURCE>(Entities.ConvertObject<Galatee.Entity.Model.POSTESOURCE, CsPosteSource>(pPosteSourceCollection));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Insert(CsPosteSource pPosteSource)
        {
            try
            {
                return Entities.InsertEntity<Galatee.Entity.Model.POSTESOURCE>(Entities.ConvertObject<Galatee.Entity.Model.POSTESOURCE, CsPosteSource>(pPosteSource));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Insert(List<CsPosteSource> pPosteSourceCollection)
        {
            try
            {
                return Entities.InsertEntity<Galatee.Entity.Model.POSTESOURCE>(Entities.ConvertObject<Galatee.Entity.Model.POSTESOURCE, CsPosteSource>(pPosteSourceCollection));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public List<CsPosteSource> SelectAllPosteSource()
        {
            SqlConnection cn = new SqlConnection(Session.GetSqlConnexionString());
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandTimeout = 1800;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SPX_ACC_GET_POSTESOURCE";
            if (cmd.Parameters != null && cmd.Parameters.Count != 0) cmd.Parameters.Clear();
            DBBase.SetDBNullParametre(cmd.Parameters);
            try
            {
                if (cn.State == ConnectionState.Closed)
                    cn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);
                return Entities.GetEntityListFromQuery<CsPosteSource>(dt);
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


        public bool Delete(CsPosteSource del)
        {
            SqlConnection cn = new SqlConnection(Session.GetSqlConnexionString());
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandTimeout = 1800;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SPX_ACC_DELETE_POSTESOURCE";
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


    }
}
