
#region Using directives

using System;
using System.Collections.Generic;
using System.Data;
using System.Collections;
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.ComponentModel;
//using Galate.StructureDevis;
//
using System.Data.SqlClient;
using Galatee.Structure;
using Galatee.Entity.Model;
//

#endregion

namespace Galatee.DataAccess 
{
	///<summary>
	/// This class is the Data Access Logic Component implementation for the <see cref=""/> business entity.
	///</summary>
	[DataObject]
    public partial class DBSUIVIDEVIS /*: DBBase */
	{
        /*
        public static List<ObjSUIVIDEVIS> FillSuivi(IDataReader reader, List<ObjSUIVIDEVIS> rows, int start, int pageLength)
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

                ObjSUIVIDEVIS c = new ObjSUIVIDEVIS();
                c.Id = (Convert.IsDBNull(reader["Id"])) ? (int)0 : (System.Int32)reader["Id"];
                c.NumDevis = (Convert.IsDBNull(reader["NumDevis"])) ? null : (System.String)reader["NumDevis"];
                c.IdEtape = (Convert.IsDBNull(reader["IdEtape"])) ? (int)0 : (System.Int32)reader["IdEtape"];
                c.Duree = (Convert.IsDBNull(reader["Duree"])) ? (int)0 : (System.Int32)reader["Duree"];
                c.Agent = (Convert.IsDBNull(reader["Agent"])) ? null : (System.String)reader["Agent"];
                c.Commentaire = (Convert.IsDBNull(reader["Commentaire"])) ? null : (System.String)reader["Commentaire"];
                c.NomAgent = (Convert.IsDBNull(reader["NomAgent"])) ? null : (System.String)reader["NomAgent"];
                c.LibelleTache = (Convert.IsDBNull(reader["LibelleTache"])) ? null : (System.String)reader["LibelleTache"];
                rows.Add(c);
            }
            return rows;
        }

        public static List<ObjSUIVIDEVIS> Fill(IDataReader reader, List<ObjSUIVIDEVIS> rows, int start, int pageLength)
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

                ObjSUIVIDEVIS c = new ObjSUIVIDEVIS();
                c.Id = (Convert.IsDBNull(reader["Id"])) ? (int)0 : (System.Int32)reader["Id"];
                c.NumDevis = (Convert.IsDBNull(reader["NumDevis"])) ? null : (System.String)reader["NumDevis"];
                c.IdEtape = (Convert.IsDBNull(reader["IdEtape"])) ? (int)0 : (System.Int32)reader["IdEtape"];
                c.Duree = (Convert.IsDBNull(reader["Duree"])) ? (int)0 : (System.Int32)reader["Duree"];
                c.Agent = (Convert.IsDBNull(reader["Agent"])) ? null : (System.String)reader["Agent"];
                c.Commentaire = (Convert.IsDBNull(reader["Commentaire"])) ? null : (System.String)reader["Commentaire"];
                rows.Add(c);
            }
            return rows;
        }

        public static ObjSUIVIDEVIS GetSuiviDevisByNumDevisProduitIdEtape(string num, string pProduit, int pIdEtape)
        {
            ObjSUIVIDEVIS row = new ObjSUIVIDEVIS();
            string connectString = Session.GetSqlConnexionString();

            SqlConnection connection = new SqlConnection(connectString);
            SqlCommand command = new SqlCommand("SPX_DEVIS_SUIVIDEVIS_RETOURNEByNumDevisProduitIdEtape", connection);
            command.CommandType = CommandType.StoredProcedure;

            SqlParameter param = command.Parameters.Add(new SqlParameter("@NumDevis", num));
            param.Direction = ParameterDirection.Input;

            SqlParameter param0 = command.Parameters.Add(new SqlParameter("@Produit", pProduit));
            param0.Direction = ParameterDirection.Input;

            SqlParameter param1 = command.Parameters.Add(new SqlParameter("@IdEtape", pIdEtape));
            param1.Direction = ParameterDirection.Input;

            SetDBNullParametre(command.Parameters);

            try
            {
                //Ouverture
                if (connection.State == ConnectionState.Closed)
                    connection.Open();

                //Object datareader
                SqlDataReader reader = command.ExecuteReader();

                List<ObjSUIVIDEVIS> tmp = new List<ObjSUIVIDEVIS>();
                FillSuivi(reader, tmp, 0, int.MaxValue);
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

        public static ObjSUIVIDEVIS GetSuiviDevisByNumDevisEtape(string pNumDevis, int pIdEtape)
        {
            ObjSUIVIDEVIS row = new ObjSUIVIDEVIS();
            string connectString = Session.GetSqlConnexionString();

            SqlConnection connection = new SqlConnection(connectString);
            SqlCommand command = new SqlCommand("SPX_DEVIS_SUIVIDEVIS_RETOURNEByNumDevisIdEtape", connection);
            command.CommandType = CommandType.StoredProcedure;

            SqlParameter param = command.Parameters.Add(new SqlParameter("@NumDevis", pNumDevis));
            param.Direction = ParameterDirection.Input;

            SqlParameter param1 = command.Parameters.Add(new SqlParameter("@IdEtape", pIdEtape));
            param1.Direction = ParameterDirection.Input;

            SetDBNullParametre(command.Parameters);

            try
            {
                //Ouverture
                if (connection.State == ConnectionState.Closed)
                    connection.Open();

                //Object datareader
                SqlDataReader reader = command.ExecuteReader();

                List<ObjSUIVIDEVIS> tmp = new List<ObjSUIVIDEVIS>();
                FillSuivi(reader, tmp, 0, int.MaxValue);
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
                if(connection.State == ConnectionState.Open)
                connection.Close();
                connection.Dispose();
                command.Dispose();
            }
            return row;
        }

        public static List<ObjSUIVIDEVIS> GetSuiviDevisByNumDevis(CsCriteresDevis pCritereDevis)
        {
            DateTime date, currentDate;
            currentDate = DateTime.Now.Date;
            TimeSpan difference;

            string connectString = Session.GetSqlConnexionString();

            SqlConnection connection = new SqlConnection(connectString);
            SqlCommand command = new SqlCommand("SPX_DEVIS_SUIVIDEVIS_RETOURNEByNumDevis", connection);
            command.CommandType = CommandType.StoredProcedure;
            if (pCritereDevis != null)
            {
                SqlParameter paramNumDevis = command.Parameters.Add(new SqlParameter("@NumDevis", pCritereDevis.NumeroDevis));
                paramNumDevis.Direction = ParameterDirection.Input;
            }
            SetDBNullParametre(command.Parameters);

            try
            {
                if (connection.State == ConnectionState.Closed)
                    connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                List<ObjSUIVIDEVIS> tmp = new List<ObjSUIVIDEVIS>();
                FillSuivi(reader, tmp, 0, int.MaxValue);

                foreach (var item in tmp)
                {
                    if (pCritereDevis != null)
                    {
                        date = (DateTime)pCritereDevis.DateEtape;
                        difference = (currentDate.Date - date.Date);
                        item.Delai = difference.TotalDays;
                    }
                }
                reader.Close();

                return tmp;
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
        }

        public static bool UpdateSuiviDevis(ObjSUIVIDEVIS entity)
        {
            string connectString = Session.GetSqlConnexionString();
            int rowsAffected = -1;
            SqlConnection connection = new SqlConnection(connectString);
            SqlCommand command = new SqlCommand("SPX_DEVIS_SUIVIDEVIS_UPDATE", connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.Clear();

            command.Parameters.Add("@Id", SqlDbType.Int).Value = entity.Id;
            command.Parameters.Add("@Duree", SqlDbType.Int).Value = entity.Duree;
            command.Parameters.Add("@IdEtape", SqlDbType.Int).Value = entity.IdEtape;
            command.Parameters.Add("@NumDevis", SqlDbType.VarChar,8).Value = entity.NumDevis;
            command.Parameters.Add("@Commentaire", SqlDbType.VarChar,1024).Value = entity.Commentaire;
            command.Parameters.Add("@Agent", SqlDbType.VarChar, 5).Value = entity.Agent;

            SetDBNullParametre(command.Parameters);

            if (connection.State == ConnectionState.Closed)
                connection.Open();

            SqlTransaction trans = connection.BeginTransaction();
            command.Transaction = trans;

            try
            {
               rowsAffected =  command.ExecuteNonQuery();
                trans.Commit();
            }
            catch (Exception ex)
            {
                trans.Rollback();
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
            return Convert.ToBoolean(rowsAffected);
        }

        public static bool UpdateSuiviDevis(SqlCommand command, ObjSUIVIDEVIS entity)
        {
            int rowsAffected = -1;
            command.CommandText = "SPX_DEVIS_SUIVIDEVIS_UPDATE";
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.Clear();

            command.Parameters.Add("@Id", SqlDbType.Int).Value = entity.Id;
            command.Parameters.Add("@Duree", SqlDbType.Int).Value = entity.Duree;
            command.Parameters.Add("@IdEtape", SqlDbType.Int).Value = entity.IdEtape;
            command.Parameters.Add("@NumDevis", SqlDbType.VarChar, 8).Value = entity.NumDevis;
            command.Parameters.Add("@Commentaire", SqlDbType.VarChar, 1024).Value = entity.Commentaire;
            command.Parameters.Add("@Agent", SqlDbType.VarChar, 5).Value = entity.Agent;

            SetDBNullParametre(command.Parameters);

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

        public static bool InsertSuiviDevis(SqlCommand command, ObjSUIVIDEVIS entity)
        {
            int rowsAffected = -1;
            command.CommandText = "SPX_DEVIS_SUIVIDEVIS_INSERER";
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.Clear();

            command.Parameters.Add("@NumDevis", SqlDbType.VarChar, 8).Value = entity.NumDevis;
            command.Parameters.Add("@IdEtape", SqlDbType.Int).Value = entity.IdEtape;
            command.Parameters.Add("@Duree", SqlDbType.Int).Value = entity.Duree;
            command.Parameters.Add("@Agent", SqlDbType.VarChar, 5).Value = entity.Agent;
            command.Parameters.Add("@Commentaire", SqlDbType.VarChar, 1024).Value = entity.Commentaire;

            SetDBNullParametre(command.Parameters);

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

        public static ObjSUIVIDEVIS GetSuiviDevisByDevisIdProduitIdEtape(int pIdDevis, int pIdEtape)
        {
           try
           {
               return Entities.GetEntityFromQuery<ObjSUIVIDEVIS>(DevisProcedures.DEVIS_SUIVIDEVIS_RETOURNEByDevisIdIdEtape(pIdDevis, pIdEtape));
           }
           catch (Exception ex)
           {
               throw ex;
           }
        }

        public static ObjSUIVIDEVIS GetSuiviDevisByDevisIdEtape(int pIdDevis, int pIdEtape)
        {
            try
            {
                return Entities.GetEntityFromQuery<ObjSUIVIDEVIS>(DevisProcedures.DEVIS_SUIVIDEVIS_RETOURNEByDevisIdIdEtape(pIdDevis, pIdEtape));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<ObjSUIVIDEVIS> GetSuiviDevisByDevisId(CsCriteresDevis pCritereDevis)
        {
            List<ObjSUIVIDEVIS> listActualisee = new List<ObjSUIVIDEVIS>();
            try
            {
                 var result=   Entities.GetEntityListFromQuery<ObjSUIVIDEVIS>(DevisProcedures.DEVIS_SUIVIDEVIS_RETOURNEByDevisId(pCritereDevis));
                 foreach (var item in result)
                 {
                     //item.DUREE = CalculerDelai(pCritereDevis);
                     listActualisee.Add(item);
                 }
                 return listActualisee;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public static bool UpdateSuiviDevis(ObjSUIVIDEVIS entity)
        //{
        //    try
        //    {
        //        return Entities.UpdateEntity<Galatee.Entity.Model.SUIVIDEVIS>(Entities.ConvertObject<Galatee.Entity.Model.SUIVIDEVIS, ObjSUIVIDEVIS>(entity));
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public static bool UpdateSuiviDevis(galadbEntities command, ObjSUIVIDEVIS entity)
        //{
        //    try
        //    {
        //        return Entities.UpdateEntity<Galatee.Entity.Model.SUIVIDEVIS>(Entities.ConvertObject<Galatee.Entity.Model.SUIVIDEVIS, ObjSUIVIDEVIS>(entity), command);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public static SUIVIDEVIS GetSuiviDevis(ObjSUIVIDEVIS entity)
        //{
        //    try
        //    {
        //        return Entities.ConvertObject<Galatee.Entity.Model.SUIVIDEVIS, ObjSUIVIDEVIS>(entity);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        //public static bool InsertSuiviDevis(galadbEntities command, ObjSUIVIDEVIS entity)
        //{
        //    try
        //    {
        //        return Entities.InsertEntity<Galatee.Entity.Model.SUIVIDEVIS>(Entities.ConvertObject<Galatee.Entity.Model.SUIVIDEVIS, ObjSUIVIDEVIS>(entity), command);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        private static double CalculerDelai(CsCriteresDevis pCritereDevis)
        {
            DateTime date, currentDate;
            currentDate = DateTime.Now.Date;
            TimeSpan difference;
            try
            {
                if (pCritereDevis != null)
                {
                    date = (DateTime)pCritereDevis.DateEtape;
                    difference = (currentDate.Date - date.Date);
                    return difference.TotalDays;
                }
                return 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
	}
}