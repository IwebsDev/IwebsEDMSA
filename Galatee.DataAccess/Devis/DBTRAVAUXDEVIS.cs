using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Galatee.Structure;
using System.Data;
using System.Data.SqlClient;
using Galatee.Entity.Model;
using Galatee.Entity.Model;

namespace Galatee.DataAccess
{
    public class DBTRAVAUXDEVIS
    {
        /*
         public static List<ObjTRAVAUXDEVIS> Fill(IDataReader reader, List<ObjTRAVAUXDEVIS> rows, int start, int pageLength)
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

                ObjTRAVAUXDEVIS c = new ObjTRAVAUXDEVIS();
                c.NumDevis = (Convert.IsDBNull(reader["NumDevis"])) ? string.Empty : (System.String)reader["NumDevis"];
                c.OriginalNumDevis = (Convert.IsDBNull(reader["NumDevis"])) ? string.Empty : (System.String)reader["NumDevis"];
                c.Ordre = (Convert.IsDBNull(reader["Ordre"])) ? (byte)0 : (System.Byte)reader["Ordre"];
                c.OriginalOrdre = (Convert.IsDBNull(reader["Ordre"])) ? (byte)0 : (System.Byte)reader["Ordre"];
                c.MatriculeChefEquipe = (Convert.IsDBNull(reader["MatriculeChefEquipe"])) ? string.Empty : (System.String)reader["MatriculeChefEquipe"];
                c.NomChefEquipe = (Convert.IsDBNull(reader["NomChefEquipe"])) ? string.Empty : (System.String)reader["NomChefEquipe"];
                c.ProcesVerbal = (Convert.IsDBNull(reader["ProcesVerbal"])) ? null : (System.String)reader["ProcesVerbal"];
                if (Convert.IsDBNull(reader["MontantRegle"]))
                    c.MontantRegle = null;
                else
                    c.MontantRegle = (System.Single)reader["MontantRegle"];
                if (Convert.IsDBNull(reader["MontantRemboursement"]))
                    c.MontantRemboursement = null;
                else
                    c.MontantRemboursement = (System.Single)reader["MontantRemboursement"];
                if (Convert.IsDBNull(reader["IdPrestataire"]))
                    c.IdPrestataire = null;
                else
                    c.IdPrestataire = (System.Byte)reader["IdPrestataire"];
                c.DatePrevisionnelle = (Convert.IsDBNull(reader["DatePrevisionnelle"])) ? DateTime.MinValue : (System.DateTime)reader["DatePrevisionnelle"];
                if (Convert.IsDBNull(reader["DateDebutTrvx"]))
                    c.DateDebutTrvx = null;
                else
                    c.DateDebutTrvx = (System.DateTime)reader["DateDebutTrvx"];
                if (Convert.IsDBNull(reader["DateFinTrvx"]))
                    c.DateFinTrvx = null;
                else
                    c.DateFinTrvx = (System.DateTime)reader["DateFinTrvx"];
                c.MatriculeReglement = (Convert.IsDBNull(reader["MatriculeReglement"])) ? null : (System.String)reader["MatriculeReglement"];
                if (Convert.IsDBNull(reader["DateReglement"]))
                    c.DateReglement = null;
                else
                    c.DateReglement = (System.DateTime)reader["DateReglement"];
                c.IsUsedForBilan = (Convert.IsDBNull(reader["IsUsedForBilan"])) ? false : (System.Boolean)reader["IsUsedForBilan"];
                rows.Add(c);
            }
            return rows;
        }

        public static ObjTRAVAUXDEVIS SelectTravaux(string numDevis, byte Ordre)
        {
            ObjTRAVAUXDEVIS row = new ObjTRAVAUXDEVIS();
            SqlCommand command = null;
            SqlConnection connection = null;
            try
            {
                string connectString = Session.GetSqlConnexionString();
                connection = new SqlConnection(connectString);
                command = new SqlCommand("SPX_DEVIS_TRAVAUXDEVIS_RETOURNEByNumDevisOrdre", connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@NumDevis", numDevis);
                command.Parameters.AddWithValue("@Ordre", Ordre);

                if(connection.State == ConnectionState.Closed)
                    connection.Open();

                //Object datareader
                SqlDataReader reader = command.ExecuteReader();

                List<ObjTRAVAUXDEVIS> tmp = new List<ObjTRAVAUXDEVIS>();
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
                if (connection != null) connection.Dispose();
                if (command !=null) command.Dispose();
            }
            return row;
        }

        public static void InsertTravaux(SqlCommand command, ObjTRAVAUXDEVIS travaux)
        {
            command.CommandText = "SPX_DEVIS_TRAVAUXDEVIS_INSERER";
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.Clear();

            command.Parameters.AddWithValue("@NumDevis", travaux.NumDevis);
            command.Parameters.AddWithValue("@Ordre",  travaux.Ordre);
            command.Parameters.AddWithValue("@MatriculeChefEquipe", travaux.MatriculeChefEquipe);
            command.Parameters.AddWithValue("@NomChefEquipe",  travaux.NomChefEquipe);
            command.Parameters.AddWithValue("@ProcesVerbal",  travaux.ProcesVerbal);
            command.Parameters.AddWithValue("@MontantRegle", travaux.MontantRegle);
            command.Parameters.AddWithValue("@MontantRemboursement",  travaux.MontantRemboursement);
            command.Parameters.AddWithValue("@IdPrestataire", travaux.IdPrestataire);
            command.Parameters.AddWithValue("@DatePrevisionnelle", travaux.DatePrevisionnelle);
            command.Parameters.AddWithValue("@DateDebutTrvx", travaux.DateDebutTrvx);
            command.Parameters.AddWithValue("@DateFinTrvx", travaux.DateFinTrvx);
            command.Parameters.AddWithValue("@MatriculeReglement",  travaux.MatriculeReglement);
            command.Parameters.AddWithValue("@DateReglement", travaux.DateReglement);
            command.Parameters.AddWithValue("@IsUsedForBilan",  travaux.IsUsedForBilan);

            DBService.SetDBNullParametre(command.Parameters);

            command.ExecuteNonQuery();
            travaux.OriginalNumDevis = travaux.NumDevis;
            travaux.OriginalOrdre = travaux.Ordre;
        }

        public static void UpdateTravaux(SqlCommand command, ObjTRAVAUXDEVIS travaux)
        {
        */

        public static ObjTRAVAUXDEVIS SelectTravaux(int pDevisId, int Ordre)
        {
                try
                {
                    return Entities.GetEntityFromQuery<ObjTRAVAUXDEVIS>(DevisProcedures.DEVIS_TRAVAUXDEVIS_RETOURNEByDevisIdOrdre(pDevisId, Ordre));
                }
                catch (Exception ex)
                {
                    throw ex;
                }
        }

        public static bool InsertTravaux(galadbEntities command, ObjTRAVAUXDEVIS travaux)
        {
            try
            {
                return Entities.InsertEntity<Galatee.Entity.Model.TRAVAUXDEVIS>(Entities.ConvertObject<Galatee.Entity.Model.TRAVAUXDEVIS, ObjTRAVAUXDEVIS>(travaux), command);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool UpdateTravaux(galadbEntities command, ObjTRAVAUXDEVIS travaux)
        {
            try
            {
                return Entities.UpdateEntity<Galatee.Entity.Model.TRAVAUXDEVIS>(Entities.ConvertObject<Galatee.Entity.Model.TRAVAUXDEVIS, ObjTRAVAUXDEVIS>(travaux), command);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
