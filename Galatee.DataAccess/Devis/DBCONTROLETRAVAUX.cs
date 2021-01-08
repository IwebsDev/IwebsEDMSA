using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Galatee.Structure;
using System.Data.SqlClient;
using System.Data;
using Galatee.Entity.Model;

namespace Galatee.DataAccess
{
    public class DBCONTROLETRAVAUX
    {
        /*
        public static CsControleTravaux SelectControles(string numDevis, byte Ordre)
        {
            CsControleTravaux control = new CsControleTravaux();

            string connectString = Session.GetSqlConnexionString();
            //Objet connection
            SqlConnection connection = new SqlConnection(connectString);
            //Objet Command
            SqlCommand command = new SqlCommand("SPX_CONTROLETRAVAUX_SelByNumDevis", connection);
            command.CommandType = CommandType.StoredProcedure;

            SqlParameter param = command.Parameters.Add(new SqlParameter("@NumDevis", numDevis));
            param.Direction = ParameterDirection.Input;
            SqlParameter param1 = command.Parameters.Add(new SqlParameter("@Ordre", Ordre));
            param1.Direction = ParameterDirection.Input;

            try
            {

                //Ouverture
                if (connection.State == ConnectionState.Closed)
                    connection.Open();

                //Object datareader
                SqlDataReader reader = command.ExecuteReader();

                if (!reader.HasRows)
                {
                    reader.Close();
                    return control;
                }
                while (reader.Read())
                {
                    control.NumDevis = reader.GetValue(0).ToString().Trim();
                    control.Ordre = byte.Parse(reader.GetValue(1).ToString().Trim());
                    control.MatriculeChefEquipe = reader.GetValue(2).ToString().Trim();
                    control.NomChefEquipe = reader.GetValue(3).ToString().Trim();
                    control.MetMoyControle = reader.GetValue(4).ToString().Trim();
                    //control.DateDebutTrvx = DateTime.Parse(reader.GetValue(5).ToString().Trim());
                    //control.DateFinTrvx = DateTime.Parse(reader.GetValue(6).ToString().Trim());
                    if (reader.GetValue(5).ToString().Trim() != string.Empty)
                        control.DateControle = DateTime.Parse(reader.GetValue(5).ToString().Trim());
                    control.VolumeTerTrvx = reader.GetValue(6).ToString().Trim();
                    control.DegradationVoie = reader.GetValue(7).ToString().Trim();
                    control.Note = byte.Parse(reader.GetValue(8).ToString().Trim());

                }
                //Fermeture reader
                reader.Close();
                return control;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                command.Dispose();
                connection.Dispose();

            }


        }

        public static bool InsertControl(ObjDEVIS devis, CsControleTravaux controle)
        {
            string connectString = Session.GetSqlConnexionString();
            //Objet connection
            SqlConnection connection = new SqlConnection(connectString);
            //Objet Command
            SqlCommand command = new SqlCommand("SPX_CONTROLETRAVAUX_INSERT", connection);
            command.CommandType = CommandType.StoredProcedure;


            command.Parameters.Clear();

            SqlParameter param0 = command.Parameters.Add(new SqlParameter("@NumDevis", SqlDbType.VarChar));
            param0.Direction = ParameterDirection.Input;
            SqlParameter param1 = command.Parameters.Add(new SqlParameter("@Ordre", SqlDbType.TinyInt));
            param1.Direction = ParameterDirection.Input;
            SqlParameter param2 = command.Parameters.Add(new SqlParameter("@MatriculeChefEquipe", SqlDbType.VarChar));
            param2.Direction = ParameterDirection.Input;
            SqlParameter param3 = command.Parameters.Add(new SqlParameter("@NomChefEquipe", SqlDbType.VarChar));
            param3.Direction = ParameterDirection.Input;
            SqlParameter param4 = command.Parameters.Add(new SqlParameter("@MetMoyControle", SqlDbType.VarChar));
            param4.Direction = ParameterDirection.Input;
            SqlParameter param5 = command.Parameters.Add(new SqlParameter("@DateControle", SqlDbType.DateTime));
            param5.Direction = ParameterDirection.Input;
            SqlParameter param6 = command.Parameters.Add(new SqlParameter("@VolumeTerTrvx", SqlDbType.VarChar));
            param6.Direction = ParameterDirection.Input;
            SqlParameter param7 = command.Parameters.Add(new SqlParameter("@DegradationVoie", SqlDbType.VarChar));
            param7.Direction = ParameterDirection.Input;
            SqlParameter param8 = command.Parameters.Add(new SqlParameter("@Note", SqlDbType.TinyInt));
            param8.Direction = ParameterDirection.Input;

            param0.Value = controle.NumDevis;
            param1.Value = controle.Ordre;
            param2.Value = controle.MatriculeChefEquipe;
            param3.Value = controle.NomChefEquipe;
            param4.Value = controle.MetMoyControle;
            param5.Value = (controle.DateControle == null) ? DateTime.Now : controle.DateControle;
            param6.Value = controle.VolumeTerTrvx;
            param7.Value = controle.DegradationVoie;
            param8.Value = controle.Note;

            if (connection.State == ConnectionState.Closed)
                connection.Open();

            SqlTransaction trans = connection.BeginTransaction();
            command.Transaction = trans;

            try
            {
                command.ExecuteNonQuery();
                trans.Commit();
                return true;
            }
            catch (Exception ex)
            {
                trans.Rollback();
                throw new Exception(ex.Message);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                command.Dispose();
                connection.Dispose();
            }
        }

        public static bool Updatecontroles(ObjDEVIS devis, CsControleTravaux controle)
        {
            string connectString = Session.GetSqlConnexionString();
            //Objet connection
            SqlConnection connection = new SqlConnection(connectString);
            //Objet Command
            SqlCommand command = new SqlCommand("SPX_CONTROLETRAVAUX_UPDATE", connection);
            command.CommandType = CommandType.StoredProcedure;


            command.Parameters.Clear();
            SqlParameter param0 = command.Parameters.Add(new SqlParameter("@NumDevis", SqlDbType.VarChar));
            param0.Direction = ParameterDirection.Input;
            SqlParameter param1 = command.Parameters.Add(new SqlParameter("@Ordre", SqlDbType.TinyInt));
            param1.Direction = ParameterDirection.Input;
            SqlParameter param2 = command.Parameters.Add(new SqlParameter("@MatriculeChefEquipe", SqlDbType.VarChar));
            param2.Direction = ParameterDirection.Input;
            SqlParameter param3 = command.Parameters.Add(new SqlParameter("@NomChefEquipe", SqlDbType.VarChar));
            param3.Direction = ParameterDirection.Input;
            SqlParameter param4 = command.Parameters.Add(new SqlParameter("@MetMoyControle", SqlDbType.VarChar));
            param4.Direction = ParameterDirection.Input;
            SqlParameter param5 = command.Parameters.Add(new SqlParameter("@Datecontrole", SqlDbType.DateTime));
            param5.Direction = ParameterDirection.Input;
            SqlParameter param6 = command.Parameters.Add(new SqlParameter("@VolumeTerTrvx", SqlDbType.VarChar));
            param6.Direction = ParameterDirection.Input;
            SqlParameter param7 = command.Parameters.Add(new SqlParameter("@DegradationVoie", SqlDbType.VarChar));
            param7.Direction = ParameterDirection.Input;
            SqlParameter param8 = command.Parameters.Add(new SqlParameter("@Note", SqlDbType.TinyInt));
            param8.Direction = ParameterDirection.Input;

            param0.Value = controle.NumDevis;
            param1.Value = controle.Ordre;
            param2.Value = controle.MatriculeChefEquipe;
            param3.Value = controle.NomChefEquipe;
            param4.Value = controle.MetMoyControle;
            param5.Value = controle.DateControle;
            param6.Value = controle.VolumeTerTrvx;
            param7.Value = controle.DegradationVoie;
            param8.Value = controle.Note;

            if (connection.State == ConnectionState.Closed)
                connection.Open();

            SqlTransaction trans = connection.BeginTransaction();
            command.Transaction = trans;

            try
            {
                command.ExecuteNonQuery();
                trans.Commit();
                return true;
            }
            catch (Exception ex)
            {
                trans.Rollback();
                throw new Exception(ex.Message);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                command.Dispose();
                connection.Dispose();
            }
        }
         
         */

        public static CsControleTravaux SelectControles(int pIdDevis, int pOrdre)
        {
            try
            {
                return Entities.GetEntityFromQuery<CsControleTravaux>(DevisProcedures.DEVIS_CONTROLETRAVAUX_RETOURNEByDevisId(pIdDevis, pOrdre));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public static bool InsertControl(CsControleTravaux pControle)
        //{
        //    try
        //    {
        //        //pControle.FK_IDMATRICULE =GetAgentByMatricul(pControle.USERCREATION);
        //        return Entities.InsertEntity<Galatee.Entity.Model.CONTROLETRAVAUX>(Entities.ConvertObject<Galatee.Entity.Model.CONTROLETRAVAUX, CsControleTravaux>(pControle));
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}


        public static bool InsertControl(CsControleTravaux pControle)
        {
            try
            {
                //pControle.FK_IDMATRICULE =GetAgentByMatricul(pControle.USERCREATION);
                bool DmdSaved = false;
                DEMANDE laDemande = new DEMANDE();
                using (galadbEntities context = new galadbEntities())
                {
                    laDemande = context.DEMANDE.FirstOrDefault(d => d.PK_ID == pControle.FK_IDDEMANDE);
                    laDemande.ISCONTROLE = true;
                }
                DmdSaved = Entities.UpdateEntity(laDemande);
                var CrtTravaux = Entities.ConvertObject<Galatee.Entity.Model.CONTROLETRAVAUX, CsControleTravaux>(pControle);
                CrtTravaux.NUMDEM = laDemande.NUMDEM;
                CrtTravaux.FK_IDDEMANDE = laDemande.PK_ID;
                return DmdSaved && Entities.InsertEntity<Galatee.Entity.Model.CONTROLETRAVAUX>(CrtTravaux);

            }
            catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            {
                // Retrieve the error messages as a list of strings.
                var errorMessages = ex.EntityValidationErrors
                        .SelectMany(x => x.ValidationErrors)
                        .Select(x => x.ErrorMessage);

                // Join the list to a single string.
                var fullErrorMessage = string.Join("; ", errorMessages);

                // Combine the original exception message with the new one.
                var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

                //return exceptionMessage;
                // Throw a new DbEntityValidationException with the improved exception message.
                throw new System.Data.Entity.Validation.DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);

            }
        }


        private static int GetAgentByMatricul(string p)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    return context.ADMUTILISATEUR.First(d => d.MATRICULE==p).PK_ID;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool Updatecontroles( CsControleTravaux pControle)
        {
            try
            {
                return Entities.UpdateEntity<Galatee.Entity.Model.CONTROLETRAVAUX>(Entities.ConvertObject<Galatee.Entity.Model.CONTROLETRAVAUX, CsControleTravaux>(pControle));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
