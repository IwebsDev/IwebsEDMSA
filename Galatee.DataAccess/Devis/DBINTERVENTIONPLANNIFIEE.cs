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
    public class DBINTERVENTIONPLANNIFIEE : Galatee.DataAccess.Parametrage.DbBase
    {
        /*
        private string ConnectionString;
        private SqlConnection cn = null;

        public bool Transaction { get; set; }

        private SqlCommand cmd = null;

        public DBINTERVENTIONPLANNIFIEE()
        {
            ConnectionString = Session.GetSqlConnexionString();
        }

        public DBINTERVENTIONPLANNIFIEE(string ConnStr)
        {
            ConnectionString = ConnStr;
        }

        public static List<CsInterventionPlannifiee> Fill(IDataReader reader, List<CsInterventionPlannifiee> rows, int start, int pageLength)
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

                CsInterventionPlannifiee c = new CsInterventionPlannifiee();
                c.Id = (Convert.IsDBNull(reader["Id"])) ? (int)0 : (System.Int32)reader["Id"];
                c.DateRendezVous = (Convert.IsDBNull(reader["DateRendezVous"])) ? DateTime.MinValue : (System.DateTime)reader["DateRendezVous"];
                c.Intervention = (Convert.IsDBNull(reader["Intervention"])) ? string.Empty : (System.String)reader["Intervention"];
                c.Responsable = (Convert.IsDBNull(reader["Responsable"])) ? string.Empty : (System.String)reader["Responsable"];
                rows.Add(c);
            }
            return rows;
        }

        public bool Update(CsInterventionPlannifiee entity)
        {
            cn = new SqlConnection(ConnectionString);
            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SPX_DEVIS_INTERVENTIONPLANNIFIEE_UPDATE";
            int rowsAffected = -1;

            cmd.Parameters.AddWithValue("@Id", entity.Id);
            cmd.Parameters.AddWithValue("@DateRendezVous", entity.DateRendezVous);
            cmd.Parameters.AddWithValue("@Intervention", entity.Intervention);
            cmd.Parameters.AddWithValue("@Responsable", entity.Responsable);

            try
            {
                rowsAffected = cmd.ExecuteNonQuery();

                return Convert.ToBoolean(rowsAffected);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                    cn.Close();
                cmd.Dispose();
            }
        }

        public bool Insert(CsInterventionPlannifiee entity)
        {
            cn = new SqlConnection(ConnectionString);
            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SPX_DEVIS_INTERVENTIONPLANNIFIEE_INSERER";
            int rowsAffected = -1;

            //cmd.Parameters.AddWithValue("@Id", entity.Id);
            cmd.Parameters.AddWithValue("@DateRendezVous", entity.DateRendezVous);
            cmd.Parameters.AddWithValue("@Intervention", entity.Intervention);
            cmd.Parameters.AddWithValue("@Responsable", entity.Responsable);

            try
            {
                rowsAffected = cmd.ExecuteNonQuery();

                return Convert.ToBoolean(rowsAffected);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                    cn.Close();
                cmd.Dispose();
            }
        }

        public List<CsInterventionPlannifiee> GetAll()
        {
            cn = new SqlConnection(ConnectionString);
            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SPX_DEVIS_INTERVENTIONPLANNIFIEE_RETOURNE";
            IDataReader reader = null;
            try
            {
                reader = cmd.ExecuteReader();
                List<CsInterventionPlannifiee> rows = new List<CsInterventionPlannifiee>();
                Fill(reader, rows, int.MinValue, int.MaxValue);
                reader.Close();
                return rows;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                    cn.Close();
                cmd.Dispose();
            }
        }

        public List<CsInterventionPlannifiee> GetByResponsable(string pMatricule)
        {
            cn = new SqlConnection(ConnectionString);
            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "spx_INTERVENTIONPLANNIFIEE_GetByResponsable";
            IDataReader reader = null;
            try
            {
                cmd.Parameters.AddWithValue("@Responsable", pMatricule);
                reader = cmd.ExecuteReader();
                List<CsInterventionPlannifiee> rows = new List<CsInterventionPlannifiee>();
                Fill(reader, rows, int.MinValue, int.MaxValue);
                reader.Close();
                return rows;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                    cn.Close();
                cmd.Dispose();
            }
        }

        public List<CsInterventionPlannifiee> GetById(int pId)
        {
            cn = new SqlConnection(ConnectionString);
            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SPX_DEVIS_INTERVENTIONPLANNIFIEE_RETOURNEById";
            IDataReader reader = null;
            try
            {
                cmd.Parameters.AddWithValue("@Id", pId);
                reader = cmd.ExecuteReader();
                List<CsInterventionPlannifiee> rows = new List<CsInterventionPlannifiee>();
                Fill(reader, rows, int.MinValue, int.MaxValue);
                reader.Close();
                return rows;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                    cn.Close();
                cmd.Dispose();
            }
        }

        public bool Delete(System.Int32 id)
        {
            cn = new SqlConnection(ConnectionString);
            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SPX_DEVIS_INTERVENTIONPLANNIFIEE_SUPPRIMER";
            int rowsAffected = -1;

            cmd.Parameters.AddWithValue("@Id", id);

            try
            {
                rowsAffected = cmd.ExecuteNonQuery();


                if (rowsAffected == 0)
                {
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
                if (cn.State == ConnectionState.Open)
                    cn.Close();
                cmd.Dispose();
            }
        }
        */

        public bool Update(CsInterventionPlannifiee entity)
        {
            try
            {
                return Entities.UpdateEntity<Galatee.Entity.Model.INTERVENTIONPLANNIFIEE>(Entities.ConvertObject<Galatee.Entity.Model.INTERVENTIONPLANNIFIEE, CsInterventionPlannifiee>(entity));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Insert(CsInterventionPlannifiee entity)
        {
            try
            {
                return Entities.InsertEntity<Galatee.Entity.Model.INTERVENTIONPLANNIFIEE>(Entities.ConvertObject<Galatee.Entity.Model.INTERVENTIONPLANNIFIEE, CsInterventionPlannifiee>(entity));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CsInterventionPlannifiee> GetAll()
        {
            try
            {
                return Entities.GetEntityListFromQuery<CsInterventionPlannifiee>(DevisProcedures.DEVIS_INTERVENTIONPLANNIFIEE_RETOURNE());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CsInterventionPlannifiee> GetByResponsable(string pMatricule)
        {
            try
            {
                return Entities.GetEntityListFromQuery<CsInterventionPlannifiee>(DevisProcedures.INTERVENTIONPLANNIFIEE_GetByResponsable(pMatricule));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CsInterventionPlannifiee> GetById(int pId)
        {
            try
            {
                return Entities.GetEntityListFromQuery<CsInterventionPlannifiee>(DevisProcedures.DEVIS_INTERVENTIONPLANNIFIEE_RETOURNEById(pId));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Delete(CsInterventionPlannifiee pInterventionPlannifiee)
        {
            try
            {
                return Entities.DeleteEntity<Galatee.Entity.Model.INTERVENTIONPLANNIFIEE>(Entities.ConvertObject<Galatee.Entity.Model.INTERVENTIONPLANNIFIEE, CsInterventionPlannifiee>(pInterventionPlannifiee));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
