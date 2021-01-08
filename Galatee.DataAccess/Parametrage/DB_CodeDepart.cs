using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Galatee.Structure;
using Galatee.Entity.Model;

namespace Galatee.DataAccess
{
    public class DB_CodeDepart 
    {

        //public List<CsCodeDepart> SelectAllCodeDepart()
        //{
        //    try
        //    {
        //        return Entities.GetEntityListFromQuery<CsCodeDepart>(ParamProcedure.PARAM_CODEDEPART_RETOURNE());
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public bool Delete(CsCodeDepart pCodeDepart)
        //{
        //    try
        //    {
        //        return Entities.DeleteEntity<Galatee.Entity.Model.DEPARTBT>(Entities.ConvertObject<Galatee.Entity.Model.DEPARTBT, CsCodeDepart>(pCodeDepart));
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public bool Delete(List<CsCodeDepart> pCodeDepartCollection)
        {
            try
            {
                //return Entities.DeleteEntity<Galatee.Entity.Model.DEPARTBT>(Entities.ConvertObject<Galatee.Entity.Model.DEPARTBT, CsCodeDepart>(pCodeDepartCollection));


                foreach (CsCodeDepart st in pCodeDepartCollection)
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

        public bool Update(CsCodeDepart pCodeDepart)
        {
            try
            {
                return Entities.UpdateEntity<Galatee.Entity.Model.DEPARTHTA>(Entities.ConvertObject<Galatee.Entity.Model.DEPARTHTA, CsCodeDepart>(pCodeDepart));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Update(List<CsCodeDepart> pCodeDepartCollection)
        {
            try
            {
                return Entities.UpdateEntity<Galatee.Entity.Model.DEPARTHTA>(Entities.ConvertObject<Galatee.Entity.Model.DEPARTHTA, CsCodeDepart>(pCodeDepartCollection));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Insert(CsCodeDepart pCodeDepart)
        {
            try
            {
                return Entities.InsertEntity<Galatee.Entity.Model.DEPARTHTA>(Entities.ConvertObject<Galatee.Entity.Model.DEPARTHTA, CsCodeDepart>(pCodeDepart));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Insert(List<CsCodeDepart> pCodeDepartCollection)
        {
            try
            {
                return Entities.InsertEntity<Galatee.Entity.Model.DEPARTHTA>(Entities.ConvertObject<Galatee.Entity.Model.DEPARTHTA, CsCodeDepart>(pCodeDepartCollection));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public List<CsCodeDepart> SelectAllCodeDepart()
        {
            SqlConnection cn = new SqlConnection(Session.GetSqlConnexionString());
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandTimeout = 1800;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SPX_ACC_GET_DEPARTHTA";
            if (cmd.Parameters != null && cmd.Parameters.Count != 0) cmd.Parameters.Clear();
            DBBase.SetDBNullParametre(cmd.Parameters);
            try
            {
                if (cn.State == ConnectionState.Closed)
                    cn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);
                return Entities.GetEntityListFromQuery<CsCodeDepart>(dt);
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


        public bool Delete(CsCodeDepart del)
        {
            SqlConnection cn = new SqlConnection(Session.GetSqlConnexionString());
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandTimeout = 1800;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SPX_ACC_DELETE_DEPARTHTA";
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
