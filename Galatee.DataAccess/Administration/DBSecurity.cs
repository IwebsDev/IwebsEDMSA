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
    public class DBSecurity : Galatee.DataAccess.Parametrage.DbBase
    {

        public DBSecurity()
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

        //public bool Update(CsUtilisateur admUsers)
        //{
        //    cn = new SqlConnection(ConnectionString);
        //    cmd = new SqlCommand();
        //    cmd.Connection = cn;
        //    //SqlCommand cmd = new SqlCommand("spx_AdmUsers_Update", this.ConnectionString);
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.CommandText = "spx_Adm_Uers_Update";
        //    //cmd.CommandText = "spx_AdmUsers_Update";
        //    cmd.Parameters.Clear();
        //    //cmd.Parameters.AddWithValue("@IdUser", admUsers.IdUser);
        //    cmd.Parameters.Add("@LoginName", SqlDbType.VarChar).Value = admUsers.LoginName;
        //    cmd.Parameters.Add("@DateCreation", SqlDbType.DateTime).Value = admUsers.DateCreation;
        //    cmd.Parameters.Add("@DateDerniereModification", SqlDbType.DateTime).Value = admUsers.DateDerniereModification;
        //    cmd.Parameters.Add("@DateDebutValidite", SqlDbType.DateTime).Value = admUsers.DateDebutValidite;
        //    cmd.Parameters.Add("@DateFinValidite", SqlDbType.DateTime).Value = admUsers.DateFinValidite;
        //    cmd.Parameters.Add("@IdStatusCompte", SqlDbType.VarChar).Value = admUsers.IdStatusCompte;
        //    cmd.Parameters.Add("@RoleID", SqlDbType.VarChar).Value = admUsers.RoleID;
        //    cmd.Parameters.Add("@DateDerniereModificationPassword", SqlDbType.DateTime).Value = admUsers.DateDerniereModificationPassword;
        //    cmd.Parameters.Add("@InitUserPassword", SqlDbType.VarChar).Value = admUsers.InitUserPassword;
        //    cmd.Parameters.Add("@NombreEchecsOuvertureSession", SqlDbType.Int).Value = admUsers.NombreEchecsOuvertureSession;
        //    cmd.Parameters.Add("@DateDerniereConnexion", SqlDbType.DateTime).Value = admUsers.DateDerniereConnexion;
        //    cmd.Parameters.Add("@DerniereConnexionReussie", SqlDbType.DateTime).Value = admUsers.DerniereConnexionReussie;
        //    cmd.Parameters.Add("@DateDernierVerrouillage", SqlDbType.DateTime).Value = admUsers.DateDernierVerrouillage;
        //    cmd.Parameters.Add("@Centre", SqlDbType.VarChar).Value = admUsers.Centre;
        //    cmd.Parameters.Add("@Matricule", SqlDbType.VarChar).Value = admUsers.Matricule;
        //    cmd.Parameters.Add("@DisplayName", SqlDbType.VarChar).Value = admUsers.DisplayName;
        //    cmd.Parameters.Add("@fonction", SqlDbType.VarChar).Value = admUsers.Fonction;
        //    cmd.Parameters.Add("@NumCaisse", SqlDbType.VarChar).Value = admUsers.NumCaisse;
        //    cmd.Parameters.Add("@branche", SqlDbType.VarChar).Value = admUsers.Branche;
        //    //cmd.Parameters.AddWithValue("@DisplayName",  admUsers.DisplayName);
        //   // cmd.Parameters.AddWithValue("@Nom",  admUsers.Nom);
        //   // cmd.Parameters.AddWithValue("@Prenoms",  admUsers.Prenoms);
        //   // cmd.Parameters.AddWithValue("@Agent_ID", admUsers.Agent_ID);
            
        //   // cmd.Parameters.AddWithValue("@EstMutiSociete", admUsers.EstMutiSociete);
  
        //    /*Adaptation Galatee - Par OLA*/
        //    //cmd.Parameters.AddWithValue("@Centre", admUsers.Centre);
        //    //cmd.Parameters.AddWithValue("@Matricule", admUsers.Matricule);
        //    //cmd.Parameters.AddWithValue("@NomPrenoms", admUsers.DisplayName);
        //    //cmd.Parameters.AddWithValue("@fonction", admUsers.Fonction);
        //    //cmd.Parameters.AddWithValue("@NumCaisse", admUsers.NumCaisse);
        //    //cmd.Parameters.AddWithValue("@branche", admUsers.Branche);
        //    /*Fin - le 14/10/2009*/

        //    DBBase.SetDBNullParametre(cmd.Parameters);
 
        //    try
        //    {

        //        if (cn.State == ConnectionState.Closed)
        //            cn.Open();
        //        cmd.ExecuteNonQuery();
        //        return true;
        //    }
        //    catch(Exception ex)
        //    {

        //        string error= ex.Message;
        //        return false;
            
        //    }
        //    finally
        //    {
        //        if (cn.State == ConnectionState.Open)
        //            cn.Close(); // Fermeture de la connection 
        //        cmd.Dispose();
        //    }
        //}

        //public void Insert(CsUtilisateur admUsers)
        //{
        //    cn = new SqlConnection(ConnectionString);
        //    cmd = new SqlCommand();
        //    cmd.Connection = cn;
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.CommandText = "spx_AdmUsers_Insert";
        //    cmd.Parameters.Clear();
        //    //cmd.Parameters.AddWithValue("@IdUser", admUsers.IdUser);
        //    //cmd.Parameters.AddWithValue("@LoginName", admUsers.LoginName);
        //    //cmd.Parameters.AddWithValue("@DisplayName", admUsers.DisplayName);
        //    //cmd.Parameters.AddWithValue("@Nom", admUsers.Nom);
        //    //cmd.Parameters.AddWithValue("@Prenoms", admUsers.Prenoms);
        //    //cmd.Parameters.AddWithValue("@Agent_ID", admUsers.Agent_ID);
        //    cmd.Parameters.Add("@LoginName", SqlDbType.VarChar).Value = admUsers.LoginName;
        //    cmd.Parameters.Add("@DateCreation", SqlDbType.DateTime).Value = admUsers.DateCreation;
        //    cmd.Parameters.Add("@DateDerniereModification", SqlDbType.DateTime).Value = admUsers.DateDerniereModification;
        //    cmd.Parameters.Add("@DateDebutValidite", SqlDbType.DateTime).Value = admUsers.DateDebutValidite;
        //    cmd.Parameters.Add("@DateFinValidite", SqlDbType.DateTime).Value = admUsers.DateFinValidite;
        //    cmd.Parameters.Add("@IdStatusCompte", SqlDbType.TinyInt).Value =Convert.ToInt16(admUsers.IdStatusCompte);
        //    cmd.Parameters.Add("@Password", SqlDbType.VarChar).Value = admUsers.Password;

        //    //cmd.Parameters.Add("@RoleID", SqlDbType.VarChar).Value = admUsers.RoleID;
        //    cmd.Parameters.Add("@DateDerniereModificationPassword", SqlDbType.DateTime).Value = admUsers.DateDerniereModificationPassword;
        //    cmd.Parameters.Add("@InitUserPassword", SqlDbType.Bit).Value = admUsers.InitUserPassword==true ? 0:1;
        //    cmd.Parameters.Add("@NombreEchecsOuvertureSession", SqlDbType.TinyInt).Value = admUsers.NombreEchecsOuvertureSession;
        //    cmd.Parameters.Add("@DateDerniereConnexion", SqlDbType.DateTime).Value = admUsers.DateDerniereConnexion;
        //    cmd.Parameters.Add("@DerniereConnexionReussie", SqlDbType.Bit).Value = admUsers.DerniereConnexionReussie == true? 0:1;
        //    cmd.Parameters.Add("@DateDernierVerrouillage", SqlDbType.DateTime).Value = admUsers.DateDernierVerrouillage;
        //    cmd.Parameters.Add("@Centre", SqlDbType.VarChar).Value = admUsers.Centre;
        //    cmd.Parameters.Add("@Matricule", SqlDbType.VarChar).Value = admUsers.Matricule;
        //    cmd.Parameters.Add("@NomPrenoms", SqlDbType.VarChar).Value = admUsers.DisplayName;
        //    cmd.Parameters.Add("@fonction", SqlDbType.VarChar).Value = admUsers.Fonction;
        //    cmd.Parameters.Add("@NumCaisse", SqlDbType.VarChar).Value = admUsers.NumCaisse;
        //    cmd.Parameters.Add("@branche", SqlDbType.VarChar).Value = admUsers.Branche;
        //    cmd.Parameters.Add("@PerimetreAction", SqlDbType.SmallInt).Value =Convert.ToInt16(admUsers.PerimetreAction);

        //    /*Adaptation Galatee - Par OLA*/
        //    //cmd.Parameters.AddWithValue("@Centre", admUsers.Centre);
        //    //cmd.Parameters.AddWithValue("@Matricule", admUsers.Matricule);
        //    //cmd.Parameters.AddWithValue("@NomPrenoms", admUsers.DisplayName);
        //    //cmd.Parameters.AddWithValue("@fonction", admUsers.Fonction);
        //    //cmd.Parameters.AddWithValue("@NumCaisse", admUsers.NumCaisse);
        //    //cmd.Parameters.AddWithValue("@branche", admUsers.Branche);
        //    /*Fin - le 14/10/2009*/

        //    DBBase.SetDBNullParametre(cmd.Parameters);

        //    try
        //    {

        //        if (cn.State == ConnectionState.Closed)
        //            cn.Open();
        //        cmd.ExecuteNonQuery();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);

        //    }
        //    finally
        //    {
        //        if (cn.State == ConnectionState.Open)
        //            cn.Close(); // Fermeture de la connection 
        //        cmd.Dispose();
        //    }
        //}

        //public bool Delete(Guid IdUser)
        //{
        //    cn = new SqlConnection(ConnectionString);
        //    cmd = new SqlCommand();
        //    cmd.Connection = cn;
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.CommandText = "spx_AdmUsers_Delete";
        //    cmd.Parameters.Clear();
        //    cmd.Parameters.Add("@IdUser", SqlDbType.VarChar).Value = IdUser;


        //    DBBase.SetDBNullParametre(cmd.Parameters);

        //    try
        //    {

        //        if (cn.State == ConnectionState.Closed)
        //            cn.Open();
        //        cmd.ExecuteNonQuery();
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        string error = ex.Message;
        //        return false;

        //    }
        //    finally
        //    {
        //        if (cn.State == ConnectionState.Open)
        //            cn.Close(); // Fermeture de la connection 
        //        cmd.Dispose();
        //    }
        //}

        /*Adaptation Galatee - Par HGB*/
        public bool Delete(string Matricule)
        {
            cn = new SqlConnection(ConnectionString);
            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "spx_AdmUsers_DeleteByMatricule";
            cmd.Parameters.Clear();
            cmd.Parameters.Add("@Matricule", SqlDbType.VarChar).Value = Matricule;

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


        public void Update(CsStrategieSecurite admStrategieSecurite)
        {
            //SqlCommand cmd = new SqlCommand("spx_AdmStrategieSecurite_Update", cn);
            try
            {
                Galatee.Entity.Model.ADMSTRATEGIESECURITE entity = Entities.ConvertObject<Galatee.Entity.Model.ADMSTRATEGIESECURITE, CsStrategieSecurite>(admStrategieSecurite);
              Entities.UpdateEntity<Galatee.Entity.Model.ADMSTRATEGIESECURITE>(entity);
            }
            catch(Exception e)
            {
              throw e;
            }
        }

        public void Insert(CsStrategieSecurite admStrategieSecurite)
        {
            try
            {
                //SqlCommand cmd = new SqlCommand("spx_AdmStrategieSecurite_Insert", cn);
                Galatee.Entity.Model.ADMSTRATEGIESECURITE entity = Entities.ConvertObject<Galatee.Entity.Model.ADMSTRATEGIESECURITE,CsStrategieSecurite>(admStrategieSecurite);
                entity.PK_ID = new Guid();
                Entities.InsertEntity<Galatee.Entity.Model.ADMSTRATEGIESECURITE>(entity);
            }
            catch (Exception e)
            {
              throw e;
            }
        }

        public void Delete(Guid IdStrategieSecurite)
        {
            //SqlCommand cmd = new SqlCommand("spx_AdmStrategieSecurite_Delete", cn);
            try
            {
                Galatee.Entity.Model.ADMSTRATEGIESECURITE entity = new Galatee.Entity.Model.galadbEntities().ADMSTRATEGIESECURITE.Where(pk => pk.PK_ID == IdStrategieSecurite).FirstOrDefault();
                Entities.DeleteEntity<Galatee.Entity.Model.ADMSTRATEGIESECURITE>(entity);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void SetActif(Guid IdStrategieSecurite)
        {


  

            try
            {
                using (Galatee.Entity.Model.galadbEntities context = new galadbEntities())
                {
                    Galatee.Entity.Model.ADMSTRATEGIESECURITE entity = context.ADMSTRATEGIESECURITE.FirstOrDefault (pk => pk.PK_ID == IdStrategieSecurite);
                    entity.ACTIF = true;
                    List<Galatee.Entity.Model.ADMSTRATEGIESECURITE> lstentity = context.ADMSTRATEGIESECURITE.Where(pk => pk.PK_ID != IdStrategieSecurite).ToList();
                    lstentity.ForEach(t => t.ACTIF = false);
                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
              
            }
        }

        public CsStrategieSecurite GetActif()
        {
            cn = new SqlConnection(ConnectionString);
            SqlCommand cmd = new SqlCommand("spx_AdmStrategieSecurite_GetActif", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataReader reader = null;

            List<CsStrategieSecurite> rows = new List<CsStrategieSecurite>();

            try
            {
                if (cn.State == ConnectionState.Closed)
                    cn.Open();

                reader = cmd.ExecuteReader();
                Fill(reader, rows, 0, int.MaxValue);
                reader.Close();

                if (rows.Count != 1) return rows[0];
                return rows[0];
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

        public List<CsStrategieSecurite> GetAll()
        {
            try
            {
                //SqlCommand cmd = new SqlCommand("spx_AdmStrategieSecurite_GetAll", cn);
                //DataTable dt = Galatee.Entity.Model.AdminProcedures.RetourneListeToutStrategieSecurite();
                //return Galatee.Tools.Utility.GetEntityFromQuery<CsStrategieSecurite>(dt).ToList();
                DataTable dt = DB_ParametresGeneraux.SelectAllDonneReference("ADMSTRATEGIESECURITE");
                return Entities.GetEntityListFromQuery<CsStrategieSecurite>(dt);


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private List<CsStrategieSecurite> Fill(SqlDataReader reader, List<CsStrategieSecurite> rows, int start, int pageLength)
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

                    CsStrategieSecurite c = new CsStrategieSecurite();
                    c.PK_IDSTRATEGIESECURITE = (Convert.IsDBNull(reader["IdStrategieSecurite"])) ? Guid.Empty : (System.Guid)reader["IdStrategieSecurite"];
                    c.LIBELLE = (Convert.IsDBNull(reader["Libelle"])) ? string.Empty : (System.String)reader["Libelle"];
                    c.ACTIF = (Convert.IsDBNull(reader["Actif"])) ? false : (System.Boolean)reader["Actif"];
                    c.HISTORIQUENOMBREPASSWORD = (Convert.IsDBNull(reader["HistoriqueNombrePassword"])) ? (byte)0 : (System.Byte)reader["HistoriqueNombrePassword"];
                    c.DUREEMINIMALEPASSWORD = (Convert.IsDBNull(reader["DureeMinimalePassword"])) ? (short)0 : (System.Int16)reader["DureeMinimalePassword"];
                    c.DUREEMAXIMALEPASSWORD = (Convert.IsDBNull(reader["DureeMaximalePassword"])) ? (short)0 : (System.Int16)reader["DureeMaximalePassword"];
                    c.LONGUEURMINIMALEPASSWORD = (Convert.IsDBNull(reader["LongueurMinimalePassword"])) ? (byte)0 : (System.Byte)reader["LongueurMinimalePassword"];
                    c.CHIFFREMENTREVERSIBLEPASSWORD = (Convert.IsDBNull(reader["ChiffrementReversiblePassword"])) ? false : (System.Boolean)reader["ChiffrementReversiblePassword"];
                    c.TOUCHEVERROUILLAGESESSION = (Convert.IsDBNull(reader["ToucheVerrouillageSession"])) ? string.Empty : (System.String)reader["ToucheVerrouillageSession"];
                    c.NOMBREMAXIMALECHECSOUVERTURESESSION = (Convert.IsDBNull(reader["NombreMaximalEchecsOuvertureSession"])) ? (byte)0 : (System.Byte)reader["NombreMaximalEchecsOuvertureSession"];
                   // c.DureeVeilleSession = (Convert.IsDBNull(reader["DureeVeilleSession"])) ? (short)0 : (System.Int16)reader["DureeVeilleSession"];
                    //c.SeuilVerrouillageCompte = (Convert.IsDBNull(reader["SeuilVerrouillageCompte"])) ? (short)0 : (System.Int16)reader["SeuilVerrouillageCompte"];
                    c.DUREEVERROUILLAGECOMPTE = (Convert.IsDBNull(reader["DureeVerrouillageCompte"])) ? (short)0 : (System.Int16)reader["DureeVerrouillageCompte"];
                    c.REINITIALISERCOMPTEURVERROUILLAGECOMPTEAPRES = (Convert.IsDBNull(reader["ReinitialiserCompteurVerrouillagesCompteApres"])) ? (short)0 : (System.Int16)reader["ReinitialiserCompteurVerrouillagesCompteApres"];
                    c.NEPASCONTENIRNOMCOMPTE = (Convert.IsDBNull(reader["NePasContenirNomCompte"])) ? false : (System.Boolean)reader["NePasContenirNomCompte"];
                    c.NOMBREMINIMALCARACTERESMAJUSCULES = (Convert.IsDBNull(reader["NombreMinimalCaracteresMajuscules"])) ? (byte)0 : (System.Byte)reader["NombreMinimalCaracteresMajuscules"];
                    //c.NombreMinimalCaracteresMinuscules = (Convert.IsDBNull(reader["NombreMinimalCaracteresMinuscules"])) ? (byte)0 : (System.Byte)reader["NombreMinimalCaracteresMinuscules"];
                   // c.NOMBREMINIMALCARACTERESCHIFFRES = (Convert.IsDBNull(reader["NombreMinimalCaracteresChiffres"])) ? (byte)0 : (System.Byte)reader["NombreMinimalCaracteresChiffres"];
                    //c.NombreMinimalCaracteresNonAlphabetiques = (Convert.IsDBNull(reader["NombreMinimalCaracteresNonAlphabetiques"])) ? (byte)0 : (System.Byte)reader["NombreMinimalCaracteresNonAlphabetiques"];

                    rows.Add(c);
                }

                return rows;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

       
    }
}
