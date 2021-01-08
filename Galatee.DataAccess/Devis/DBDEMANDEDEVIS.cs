
#region using directives

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Galatee.DataAccess;
using Galatee.Structure;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Collections;
using System.Data.SqlClient;
using System.Diagnostics;
using Galatee.Entity.Model;

#endregion

namespace Galatee.DataAccess
{

    /// <summary>
    ///	This class is the base repository for the CRUD operations on the ObjDEMANDEDEVIS objects.
    /// </summary>
    public class DBDEMANDEDEVIS : DBBase
    {
        /*
        public static bool Delete(SqlCommand pCommand, System.String numDevis)
        {
            //Database database = GetDatabase(transactionManager);
            //DbCommand command = database.GetStoredProcCommand("SPX_DEVIS_DEMANDEDEVIS_SUPPRIMER");
            int rowsAffected = -1;
            pCommand.Parameters.Clear();
            pCommand.CommandType = CommandType.StoredProcedure;
            pCommand.CommandText = "SPX_DEVIS_DEMANDEDEVIS_SUPPRIMER";
            pCommand.Parameters.AddWithValue("@NumDevis", numDevis);
            try
            {
                rowsAffected = pCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            if (rowsAffected == 0)
            {
                return false;
            }
            return Convert.ToBoolean(rowsAffected);
        }
        
        public static bool Delete(SqlCommand pCommand, List<ObjDEMANDEDEVIS> entityCollection)
        {
            int number = 0;
            foreach (ObjDEMANDEDEVIS entity in entityCollection)
            {
                if (Delete(pCommand, entity.NumDevis))
                {
                    number++;
                }
            }
            return number > 0 ;
        }
       
        public List<ObjDEMANDEDEVIS> GetAll()
        {
            string connectString = Session.GetSqlConnexionString();
            SqlConnection connection = new SqlConnection(connectString);
            SqlCommand command = new SqlCommand("SPX_DEVIS_DEMANDEDEVIS_RETOURNE", connection);
            command.CommandType = CommandType.StoredProcedure;

            IDataReader reader = null;
            try
            {
                if (connection.State == ConnectionState.Closed)
                    connection.Open();
                reader = command.ExecuteReader();
                //Create Collection
                List<ObjDEMANDEDEVIS> rows = new List<ObjDEMANDEDEVIS>();
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
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                connection.Dispose();
                command.Dispose();
            }
           
        }//end getall
        
        public static ObjDEMANDEDEVIS GetByNumDevis(String numDevis)
        {
            string connectString = Session.GetSqlConnexionString();
            SqlConnection connection = new SqlConnection(connectString);
            SqlCommand command = new SqlCommand("SPX_DEVIS_DEMANDEDEVIS_RETOURNEByNumDevis", connection);
            command.CommandType = CommandType.StoredProcedure;
            IDataReader reader = null;
            command.Parameters.AddWithValue("@NumDevis", numDevis);

            try
            {
                if (connection.State == ConnectionState.Closed)
                    connection.Open();
               reader = command.ExecuteReader();
               //Create collection and fill
               List<ObjDEMANDEDEVIS> tmp = new List<ObjDEMANDEDEVIS>();
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
                throw  ex;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                connection.Dispose();
                command.Dispose();
            }
           
        }
        
        public static bool InsertDemandeDevis(ObjDEMANDEDEVIS entity, SqlCommand pCommand)
        {
            pCommand.CommandText = "SPX_DEVIS_DEMANDEDEVIS_INSERER";
            pCommand.CommandType = CommandType.StoredProcedure;
            int rowsAffected = -1;

            pCommand.Parameters.Clear();
            pCommand.Parameters.AddWithValue("@NumDevis", entity.NumDevis);
            pCommand.Parameters.AddWithValue("@Centre", entity.Centre);
            pCommand.Parameters.AddWithValue("@Client", entity.Client);
            pCommand.Parameters.AddWithValue("@OrdreClient", entity.OrdreClient);
            pCommand.Parameters.AddWithValue("@Diametre", entity.Diametre);
            pCommand.Parameters.AddWithValue("@Nom", entity.Nom);
            pCommand.Parameters.AddWithValue("@Tournee", entity.Tournee);
            pCommand.Parameters.AddWithValue("@Profession", entity.Profession);
            pCommand.Parameters.AddWithValue("@NumLot", entity.NumLot);
            pCommand.Parameters.AddWithValue("@Parcelle", entity.Parcelle);
            pCommand.Parameters.AddWithValue("@Section_Par", entity.SectionPar);
            pCommand.Parameters.AddWithValue("@Quartier", entity.Quartier);
            pCommand.Parameters.AddWithValue("@Secteur", entity.Secteur);
            pCommand.Parameters.AddWithValue("@NumTel", entity.NumTel);
            pCommand.Parameters.AddWithValue("@NumRue", entity.NumRue);
            pCommand.Parameters.AddWithValue("@NumPorte", entity.NumPorte);
            pCommand.Parameters.AddWithValue("@NumPoteauProche", entity.NumPoteauProche);
            pCommand.Parameters.AddWithValue("@Category", entity.Category);
            pCommand.Parameters.AddWithValue("@DateDemande", entity.DateDemande);
            pCommand.Parameters.AddWithValue("@Adresse", entity.Adresse);
            pCommand.Parameters.AddWithValue("@Commune", entity.Commune);
            pCommand.Parameters.AddWithValue("@RepereProche", entity.RepereProche);
            pCommand.Parameters.AddWithValue("@Ordtour", entity.Ordtour);
            pCommand.Parameters.AddWithValue("@Longitude", entity.Longitude);
            pCommand.Parameters.AddWithValue("@Latitude", entity.Latitude);

            try
            {
                SetDBNullParametre(pCommand.Parameters);
                rowsAffected = pCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Convert.ToBoolean(rowsAffected);
        }
        
        public static bool Update(ObjDEMANDEDEVIS entity, SqlCommand pCommand)
        {
            pCommand.CommandText = "SPX_DEVIS_DEMANDEDEVIS_UPDATE";
            pCommand.CommandType = CommandType.StoredProcedure;
            int rowsAffected = -1;

            pCommand.Parameters.Clear();
            pCommand.Parameters.AddWithValue("@NumDevis", entity.NumDevis);
            pCommand.Parameters.AddWithValue("@OriginalNumDevis", entity.OriginalNumDevis);
            pCommand.Parameters.AddWithValue("@Centre", entity.Centre);
            pCommand.Parameters.AddWithValue("@Client", entity.Client);
            pCommand.Parameters.AddWithValue("@OrdreClient", entity.OrdreClient);
            pCommand.Parameters.AddWithValue("@Diametre", entity.Diametre);
            pCommand.Parameters.AddWithValue("@Nom", entity.Nom);
            pCommand.Parameters.AddWithValue("@Tournee", entity.Tournee);
            pCommand.Parameters.AddWithValue("@Profession", entity.Profession);
            pCommand.Parameters.AddWithValue("@NumLot", entity.NumLot);
            pCommand.Parameters.AddWithValue("@Parcelle", entity.Parcelle);
            pCommand.Parameters.AddWithValue("@Section_Par", entity.SectionPar);
            pCommand.Parameters.AddWithValue("@Quartier", entity.Quartier);
            pCommand.Parameters.AddWithValue("@Secteur", entity.Secteur);
            pCommand.Parameters.AddWithValue("@NumTel", entity.NumTel);
            pCommand.Parameters.AddWithValue("@NumRue", entity.NumRue);
            pCommand.Parameters.AddWithValue("@NumPorte", entity.NumPorte);
            pCommand.Parameters.AddWithValue("@NumPoteauProche", entity.NumPoteauProche);
            pCommand.Parameters.AddWithValue("@Category", entity.Category);
            pCommand.Parameters.AddWithValue("@DateDemande", entity.DateDemande);
            pCommand.Parameters.AddWithValue("@Adresse", entity.Adresse);
            pCommand.Parameters.AddWithValue("@Commune", entity.Commune);
            pCommand.Parameters.AddWithValue("@RepereProche", entity.RepereProche);
            pCommand.Parameters.AddWithValue("@Ordtour", entity.Ordtour);
            pCommand.Parameters.AddWithValue("@Longitude", entity.Longitude);
            pCommand.Parameters.AddWithValue("@Latitude", entity.Latitude);

            try
            {
                SetDBNullParametre(pCommand.Parameters);
                rowsAffected = pCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Convert.ToBoolean(rowsAffected);
        }

        public static List<ObjDEMANDEDEVIS> Fill(IDataReader reader, List<ObjDEMANDEDEVIS> rows, int start, int pageLength)
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

                ObjDEMANDEDEVIS c = new ObjDEMANDEDEVIS();
                c.NumDevis = (Convert.IsDBNull(reader["NumDevis"])) ? string.Empty : (System.String)reader["NumDevis"];
                c.OriginalNumDevis = (Convert.IsDBNull(reader["NumDevis"])) ? string.Empty : (System.String)reader["NumDevis"];
                c.Centre = (Convert.IsDBNull(reader["Centre"])) ? string.Empty : (System.String)reader["Centre"];
                c.Client = (Convert.IsDBNull(reader["Client"])) ? string.Empty : (System.String)reader["Client"];
                c.OrdreClient = (Convert.IsDBNull(reader["OrdreClient"])) ? string.Empty : (System.String)reader["OrdreClient"];
                c.Diametre = (Convert.IsDBNull(reader["Diametre"])) ? null : (System.String)reader["Diametre"];
                c.Nom = (Convert.IsDBNull(reader["Nom"])) ? null : (System.String)reader["Nom"];
                c.Tournee = (Convert.IsDBNull(reader["Tournee"])) ? null : (System.String)reader["Tournee"];
                c.Profession = (Convert.IsDBNull(reader["Profession"])) ? null : (System.String)reader["Profession"];
                c.NumLot = (Convert.IsDBNull(reader["NumLot"])) ? null : (System.String)reader["NumLot"];
                c.Parcelle = (Convert.IsDBNull(reader["Parcelle"])) ? null : (System.String)reader["Parcelle"];
                c.SectionPar = (Convert.IsDBNull(reader["Section_Par"])) ? null : (System.String)reader["Section_Par"];
                c.Quartier = (Convert.IsDBNull(reader["Quartier"])) ? null : (System.String)reader["Quartier"];
                c.Secteur = (Convert.IsDBNull(reader["Secteur"])) ? null : (System.String)reader["Secteur"];
                c.NumTel = (Convert.IsDBNull(reader["NumTel"])) ? null : (System.String)reader["NumTel"];
                c.NumRue = (Convert.IsDBNull(reader["NumRue"])) ? null : (System.String)reader["NumRue"];
                c.NumPorte = (Convert.IsDBNull(reader["NumPorte"])) ? null : (System.String)reader["NumPorte"];
                c.NumPoteauProche = (Convert.IsDBNull(reader["NumPoteauProche"])) ? null : (System.String)reader["NumPoteauProche"];
                c.Category = (Convert.IsDBNull(reader["Category"])) ? null : (System.String)reader["Category"];
                if (Convert.IsDBNull(reader["DateDemande"]))
                    c.DateDemande = null;
                else
                    c.DateDemande = (System.DateTime)reader["DateDemande"];
                c.Adresse = (Convert.IsDBNull(reader["Adresse"])) ? null : (System.String)reader["Adresse"];
                c.Commune = (Convert.IsDBNull(reader["Commune"])) ? null : (System.String)reader["Commune"];
                c.RepereProche = (Convert.IsDBNull(reader["RepereProche"])) ? null : (System.String)reader["RepereProche"];
                c.Ordtour = (Convert.IsDBNull(reader["Ordtour"])) ? null : (System.String)reader["Ordtour"];
                c.Longitude = (Convert.IsDBNull(reader["Longitude"])) ? null : (System.String)reader["Longitude"];
                c.Latitude = (Convert.IsDBNull(reader["Latitude"])) ? null : (System.String)reader["Latitude"];
                c.LibelleTournee = (Convert.IsDBNull(reader["LibelleTournee"])) ? null : (System.String)reader["LibelleTournee"];
                c.LibelleCommune = (Convert.IsDBNull(reader["LibelleCommune"])) ? null : (System.String)reader["LibelleCommune"];
                c.LibelleQuartier = (Convert.IsDBNull(reader["LibelleQuartier"])) ? null : (System.String)reader["LibelleQuartier"];
                c.LibelleRue = (Convert.IsDBNull(reader["LibelleRue"])) ? null : (System.String)reader["LibelleRue"];
                c.LibelleDiametre = (Convert.IsDBNull(reader["LibelleDiametre"])) ? null : (System.String)reader["LibelleDiametre"];
                c.LibelleCategorie = (Convert.IsDBNull(reader["LibelleCategorie"])) ? null : (System.String)reader["LibelleCategorie"];

                rows.Add(c);
            }
            return rows;
        }
         */

        public List<ObjDEMANDEDEVIS> GetAll()
        {
            try
            {
                return Entities.GetEntityListFromQuery<ObjDEMANDEDEVIS>(DevisProcedures.DEVIS_DEMANDEDEVIS_RETOURNE());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static ObjDEMANDEDEVIS GetById(int pIdDemandeDevis)
        {
            try
            {
                return Entities.GetEntityFromQuery<ObjDEMANDEDEVIS>(DevisProcedures.DEVIS_DEMANDEDEVIS_RETOURNEById(pIdDemandeDevis));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static ObjDEMANDEDEVIS GetDemandeDevisByIdDevis(int pIdDevis)
        {
            try
            {
                return Entities.GetEntityFromQuery<ObjDEMANDEDEVIS>(DevisProcedures.DEVIS_DEMANDEDEVIS_RETOURNEByDevisId(pIdDevis));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public static bool InsertDemandeDevis(ObjDEMANDEDEVIS entity,DEVIS LeDevis, Galatee.Entity.Model.galadbEntities pCommand)
        //{
        //    try
        //    {
        //        DEMANDEDEVIS laDemande = Entities.ConvertObject<Galatee.Entity.Model.DEMANDEDEVIS, ObjDEMANDEDEVIS>(entity);
        //        laDemande.DEVIS = LeDevis;
        //        return Entities.InsertEntity<Galatee.Entity.Model.DEMANDEDEVIS>(laDemande, pCommand);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public static bool Update(ObjDEMANDEDEVIS entity, Galatee.Entity.Model.galadbEntities pCommand)
        //{
        //    try
        //    {
        //        return Entities.UpdateEntity<Galatee.Entity.Model.DEMANDEDEVIS>(Entities.ConvertObject<Galatee.Entity.Model.DEMANDEDEVIS, ObjDEMANDEDEVIS>(entity), pCommand);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public static bool Delete(Galatee.Entity.Model.galadbEntities pCommand, ObjDEMANDEDEVIS pDemandeDevis)
        //{
        //    try
        //    {
        //        return Entities.DeleteEntity<Galatee.Entity.Model.DEMANDEDEVIS>(Entities.ConvertObject<Galatee.Entity.Model.DEMANDEDEVIS, ObjDEMANDEDEVIS>(pDemandeDevis), pCommand);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public static bool Delete(Galatee.Entity.Model.galadbEntities pCommand, List<ObjDEMANDEDEVIS> entityCollection)
        //{
        //    try
        //    {
        //        return Entities.DeleteEntity<Galatee.Entity.Model.DEMANDEDEVIS>(Entities.ConvertObject<Galatee.Entity.Model.DEMANDEDEVIS, ObjDEMANDEDEVIS>(entityCollection), pCommand);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
    }
} 
