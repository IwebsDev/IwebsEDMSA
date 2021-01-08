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
    public class DBETATSDEVIS
    {
        /*
        public static List<CsEtatBonDeSortie> EditerDevisPourBonDeSortie(string pNumDevis, int pOrdre, bool pIsSummary, string pMatricule)
        {
            var cn = new SqlConnection(Session.GetSqlConnexionString());
            SqlCommand cmd = null;
            try
            {
                if (cn.State == ConnectionState.Closed)
                    cn.Open();
                cmd = new SqlCommand
                {
                    Connection = cn,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "SPX_DEVIS_DEVIS_EditionBonSortie"
                };
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@NumDevis", pNumDevis);
                cmd.Parameters.AddWithValue("@Ordre", pOrdre);
                cmd.Parameters.AddWithValue("@IsSummary", pIsSummary);
                cmd.Parameters.AddWithValue("@Matricule", pMatricule);

                DBService.SetDBNullParametre(cmd.Parameters);

                IDataReader reader = cmd.ExecuteReader();
                var rows = new List<CsEtatBonDeSortie>();
                FillBonDeSortie(reader, rows, int.MinValue, int.MaxValue);
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

        private static List<CsEtatBonDeSortie> FillBonDeSortie(IDataReader reader, List<CsEtatBonDeSortie> rows, int start, int pageLength)
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

                CsEtatBonDeSortie c = new CsEtatBonDeSortie();

                if (Convert.IsDBNull(reader["Balance"]))
                    c.Balance = null;
                else
                    c.Balance = (double?)reader["Balance"];

                if (Convert.IsDBNull(reader["DateReglement"]))
                    c.DateReglement = null;
                else
                    c.DateReglement = (System.DateTime)reader["DateReglement"];
                c.Centre = (Convert.IsDBNull(reader["Centre"])) ? string.Empty : (System.String)reader["Centre"];
                if (!Convert.IsDBNull(reader["DEPOSIT"]))
                    c.DEPOSIT = (decimal)reader["DEPOSIT"];
                else
                    c.DEPOSIT = null;
                c.Designation = (Convert.IsDBNull(reader["Designation"])) ? string.Empty : (System.String)reader["Designation"];
                c.Message = (Convert.IsDBNull(reader["Message"])) ? string.Empty : (System.String)reader["Message"];
                c.MeterSize = (Convert.IsDBNull(reader["MeterSize"])) ? string.Empty : (System.String)reader["MeterSize"];
                if (!Convert.IsDBNull(reader["montantHT"]))
                    c.montantHT = (decimal?)reader["montantHT"];
                else
                    c.montantHT = null;
                if (!Convert.IsDBNull(reader["montantTTC"]))
                    c.montantTTC = (decimal?)reader["montantTTC"];
                else
                    c.montantTTC = null;
                c.Nom = (Convert.IsDBNull(reader["Nom"])) ? string.Empty : (System.String)reader["Nom"];
                c.numdevis = (Convert.IsDBNull(reader["numdevis"])) ? string.Empty : (System.String)reader["numdevis"];
                c.NumeroCTR = (Convert.IsDBNull(reader["NumeroCTR"])) ? string.Empty : (System.String)reader["NumeroCTR"];
                c.NumPoteauProche = (Convert.IsDBNull(reader["NumPoteauProche"])) ? string.Empty : (System.String)reader["NumPoteauProche"];
                c.ordre = (Convert.IsDBNull(reader["ordre"])) ? (byte)0 : (System.Byte)reader["ordre"];
                c.Payment = (Convert.IsDBNull(reader["Payment"])) ? string.Empty : (System.String)reader["Payment"];
                if (!Convert.IsDBNull(reader["Prix_Unitaire"]))
                    c.Prix_Unitaire = (decimal?)reader["Prix_Unitaire"];
                else
                    c.Prix_Unitaire = null;
                c.Produit = (Convert.IsDBNull(reader["Produit"])) ? string.Empty : (System.String)reader["Produit"];

                if (!Convert.IsDBNull(reader["Quantite"]))
                    c.Quantite = (int?)reader["Quantite"];
                else
                    c.Quantite = null;

                if (!Convert.IsDBNull(reader["QuantiteRemisEnStock"]))
                    c.QuantiteRemisEnStock = (int?)reader["QuantiteRemisEnStock"];
                else
                    c.QuantiteRemisEnStock = null;

                if (!Convert.IsDBNull(reader["RembourserHT"]))
                    c.RembourserHT = (double?)reader["RembourserHT"];
                else
                    c.RembourserHT = null;
                if (!Convert.IsDBNull(reader["RembourserTTC"]))
                    c.RembourserTTC = (double?)reader["RembourserTTC"];
                else
                    c.RembourserTTC = null;
                if (!Convert.IsDBNull(reader["TotalRemboursableHT"]))
                    c.TotalRemboursableHT = (double?)reader["TotalRemboursableHT"];
                else
                    c.TotalRemboursableHT = null;

                if (!Convert.IsDBNull(reader["TotalRemboursableTTC"]))
                    c.TotalRemboursableTTC = (double?)reader["TotalRemboursableTTC"];
                else
                    c.TotalRemboursableTTC = null;

                if (!Convert.IsDBNull(reader["totalTTC"]))
                    c.totalTTC = (double?)reader["totalTTC"];
                else
                    c.totalTTC = null;
                c.TYPECTR = (Convert.IsDBNull(reader["TYPECTR"])) ? string.Empty : (System.String)reader["TYPECTR"];
                c.TypeDevis = (Convert.IsDBNull(reader["TypeDevis"])) ? string.Empty : (System.String)reader["TypeDevis"];

                c.NomAgent = (Convert.IsDBNull(reader["NomAgent"])) ? string.Empty : (System.String)reader["NomAgent"];
                c.MatriculeAgent = (Convert.IsDBNull(reader["MatriculeAgent"])) ? string.Empty : (System.String)reader["MatriculeAgent"];

                rows.Add(c);
            }
            return rows;
        }

       

        public static List<CsEtatBonTravaux> EditerDevisPourBonTravaux(string pNumDevis, int pOrdre)
        {
            var cn = new SqlConnection(Session.GetSqlConnexionString());
            SqlCommand cmd = null;
            try
            {
                if (cn.State == ConnectionState.Closed)
                    cn.Open();
                cmd = new SqlCommand
                {
                    Connection = cn,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "SPX_DEVIS_DEVIS_EditionBonTravaux"
                };
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@NumDevis", pNumDevis);
                cmd.Parameters.AddWithValue("@Ordre", pOrdre);

                DBService.SetDBNullParametre(cmd.Parameters);

                IDataReader reader = cmd.ExecuteReader();
                var rows = new List<CsEtatBonTravaux>();
                FillBonTravaux(reader, rows, int.MinValue, int.MaxValue);
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

        private static List<CsEtatBonTravaux> FillBonTravaux(IDataReader reader, List<CsEtatBonTravaux> rows, int start, int pageLength)
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

                CsEtatBonTravaux c = new CsEtatBonTravaux();
                c.Route = (Convert.IsDBNull(reader["Route"])) ? string.Empty : (System.String)reader["Route"];
                c.Nom = (Convert.IsDBNull(reader["Nom"])) ? string.Empty : (System.String)reader["Nom"];
                c.Commune = (Convert.IsDBNull(reader["Commune"])) ? string.Empty : (System.String)reader["Commune"];
                c.NumLot = (Convert.IsDBNull(reader["NumLot"])) ? string.Empty : (System.String)reader["NumLot"];
                c.Quartier = (Convert.IsDBNull(reader["Quartier"])) ? string.Empty : (System.String)reader["Quartier"];
                c.NumTel = (Convert.IsDBNull(reader["NumTel"])) ? string.Empty : (System.String)reader["NumTel"];
                c.Rue = (Convert.IsDBNull(reader["Rue"])) ? string.Empty : (System.String)reader["Rue"];
                c.NumPoteauProche = (Convert.IsDBNull(reader["NumPoteauProche"])) ? null : (System.String)reader["NumPoteauProche"];
                if (Convert.IsDBNull(reader["DateValidation"]))
                    c.DateValidation = null;
                else
                    c.DateValidation = Convert.ToDateTime(reader["DateValidation"]);
                c.TypeDevis = (Convert.IsDBNull(reader["TypeDevis"])) ? string.Empty : (System.String)reader["TypeDevis"];
                c.Produit = (Convert.IsDBNull(reader["Produit"])) ? string.Empty : (System.String)reader["Produit"];
                c.Centre = (Convert.IsDBNull(reader["Centre"])) ? string.Empty : (System.String)reader["Centre"];
                if (Convert.IsDBNull(reader["totalTTC"]))
                    c.TotalTTC = null;
                else
                    c.TotalTTC = (System.Single?)reader["totalTTC"];
                c.NumDevis = (Convert.IsDBNull(reader["NumDevis"])) ? string.Empty : (System.String)reader["NumDevis"];
                c.Ordre = (Convert.IsDBNull(reader["ordre"])) ? null : (System.Byte?)reader["ordre"];
                if (Convert.IsDBNull(reader["DateDebutTrvx"]))
                    c.DateDebutTrvx = null;
                else
                    c.DateDebutTrvx = (System.DateTime?)reader["DateDebutTrvx"];
                if (Convert.IsDBNull(reader["DateFinTrvx"]))
                    c.DateFinTrvx = null;
                else
                    c.DateFinTrvx = Convert.ToDateTime(reader["DateFinTrvx"]);
                c.Matricule = (Convert.IsDBNull(reader["Matricule"])) ? string.Empty : (System.String)reader["Matricule"];
                c.ChefEquipe = (Convert.IsDBNull(reader["Chef equipe"])) ? string.Empty : (System.String)reader["Chef equipe"];
                c.ProcesVerbal = (Convert.IsDBNull(reader["ProcesVerbal"])) ? string.Empty : (System.String)reader["ProcesVerbal"];
                c.NumeroCTR = (Convert.IsDBNull(reader["NumeroCTR"])) ? string.Empty : (System.String)reader["NumeroCTR"];
                c.MeterSize = (Convert.IsDBNull(reader["MeterSize"])) ? string.Empty : (System.String)reader["MeterSize"];
                c.TYPECTR = (Convert.IsDBNull(reader["TYPECTR"])) ? string.Empty : (System.String)reader["TYPECTR"];
                c.MarqueCTR = (Convert.IsDBNull(reader["MarqueCTR"])) ? string.Empty : (System.String)reader["MarqueCTR"];
                c.Adresse = (Convert.IsDBNull(reader["Adresse"])) ? string.Empty : (System.String)reader["Adresse"];
                if (Convert.IsDBNull(reader["IndexPoseCTR"]))
                    c.IndexPoseCTR = null;
                else
                    c.IndexPoseCTR = (System.Int32?)reader["IndexPoseCTR"];
                if (Convert.IsDBNull(reader["AnneeFabricationCTR"]))
                    c.AnneeFabricationCTR = null;
                else
                    c.AnneeFabricationCTR = Convert.ToDateTime(reader["AnneeFabricationCTR"]);
                c.NearestRoute = (Convert.IsDBNull(reader["NearestRoute"])) ? string.Empty : (System.String)reader["NearestRoute"];
                c.NumeroGPS = (Convert.IsDBNull(reader["NumeroGPS"])) ? string.Empty : (System.String)reader["NumeroGPS"];
                rows.Add(c);
            }
            return rows;
        }

          

        public static List<CsEtatProcesVerbal> FillProcesVerbal(IDataReader reader, List<CsEtatProcesVerbal> rows, int start, int pageLength)
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

                CsEtatProcesVerbal c = new CsEtatProcesVerbal();
                c.Route = (Convert.IsDBNull(reader["Route"])) ? string.Empty : (System.String)reader["Route"];
                c.OriginalRoute = (Convert.IsDBNull(reader["Route"])) ? string.Empty : (System.String)reader["Route"];
                c.Nom = (Convert.IsDBNull(reader["Nom"])) ? string.Empty : (System.String)reader["Nom"];
                c.Commune = (Convert.IsDBNull(reader["Commune"])) ? string.Empty : (System.String)reader["Commune"];
                c.NumLot = (Convert.IsDBNull(reader["NumLot"])) ? string.Empty : (System.String)reader["NumLot"];
                c.Quartier = (Convert.IsDBNull(reader["Quartier"])) ? string.Empty : (System.String)reader["Quartier"];
                c.NumTel = (Convert.IsDBNull(reader["NumTel"])) ? string.Empty : (System.String)reader["NumTel"];
                c.Rue = (Convert.IsDBNull(reader["Rue"])) ? string.Empty : (System.String)reader["Rue"];
                c.NumPoteauProche = (Convert.IsDBNull(reader["NumPoteauProche"])) ? string.Empty : (System.String)reader["NumPoteauProche"];
                c.DateValidation = (Convert.IsDBNull(reader["DateValidation"])) ? string.Empty : (System.String)reader["DateValidation"];
                c.TypeDevis = (Convert.IsDBNull(reader["TypeDevis"])) ? string.Empty : (System.String)reader["TypeDevis"];
                c.Produit = (Convert.IsDBNull(reader["Produit"])) ? string.Empty : (System.String)reader["Produit"];
                c.Centre = (Convert.IsDBNull(reader["Centre"])) ? string.Empty : (System.String)reader["Centre"];
                if (Convert.IsDBNull(reader["totalTTC"]))
                    c.TotalTTC = null;
                else
                    c.TotalTTC = (System.Single)reader["totalTTC"];
                c.NumDevis = (Convert.IsDBNull(reader["NumDevis"])) ? string.Empty : (System.String)reader["NumDevis"];
                if (Convert.IsDBNull(reader["ordre"]))
                    c.Ordre = null;
                else
                    c.Ordre = (System.Byte)reader["ordre"];
                c.DateDebutTrvx = (Convert.IsDBNull(reader["DateDebutTrvx"])) ? string.Empty : (System.String)reader["DateDebutTrvx"];
                c.DateFinTrvx = (Convert.IsDBNull(reader["DateFinTrvx"])) ? string.Empty : (System.String)reader["DateFinTrvx"];
                c.Matricule = (Convert.IsDBNull(reader["Matricule"])) ? string.Empty : (System.String)reader["Matricule"];
                c.Chefequipe = (Convert.IsDBNull(reader["Chef equipe"])) ? string.Empty : (System.String)reader["Chef equipe"];
                c.ProcesVerbal = (Convert.IsDBNull(reader["ProcesVerbal"])) ? string.Empty : (System.String)reader["ProcesVerbal"];
                c.NumeroCTR = (Convert.IsDBNull(reader["NumeroCTR"])) ? string.Empty : (System.String)reader["NumeroCTR"];
                c.MeterSize = (Convert.IsDBNull(reader["MeterSize"])) ? string.Empty : (System.String)reader["MeterSize"];
                c.TYPECTR = (Convert.IsDBNull(reader["TYPECTR"])) ? string.Empty : (System.String)reader["TYPECTR"];
                c.MarqueCTR = (Convert.IsDBNull(reader["MarqueCTR"])) ? string.Empty : (System.String)reader["MarqueCTR"];
                c.Adresse = (Convert.IsDBNull(reader["Adresse"])) ? string.Empty : (System.String)reader["Adresse"];
                if (Convert.IsDBNull(reader["IndexPoseCTR"]))
                    c.IndexPoseCTR = null;
                else
                    c.IndexPoseCTR = (System.Int32)reader["IndexPoseCTR"];
                if (Convert.IsDBNull(reader["AnneeFabricationCTR"]))
                    c.AnneeFabricationCTR = null;
                else
                    c.AnneeFabricationCTR = (System.Int32)reader["AnneeFabricationCTR"];
                c.NearestRoute = (Convert.IsDBNull(reader["NearestRoute"])) ? string.Empty : (System.String)reader["NearestRoute"];
                c.NumeroGPS = (Convert.IsDBNull(reader["NumeroGPS"])) ? string.Empty : (System.String)reader["NumeroGPS"];
                rows.Add(c);
            }
            return rows;
        }

        public static List<CsEtatProcesVerbal> EditerDevisPourProcesVerbal(string pNumDevis, int pOrdre)
        {
            var cn = new SqlConnection(Session.GetSqlConnexionString());
            SqlCommand cmd = null;
            try
            {
                if (cn.State == ConnectionState.Closed)
                    cn.Open();
                cmd = new SqlCommand
                {
                    Connection = cn,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "SPX_DEVIS_DEVIS_EditionProcesVerbal"
                };
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@NumDevis", pNumDevis);
                cmd.Parameters.AddWithValue("@Ordre", pOrdre);

                DBService.SetDBNullParametre(cmd.Parameters);

                IDataReader reader = cmd.ExecuteReader();
                var rows = new List<CsEtatProcesVerbal>();
                FillProcesVerbal(reader, rows, int.MinValue, int.MaxValue);
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
         
       

        public static List<CsEtatProcesVerbal> FillBonControle(IDataReader reader, List<CsEtatProcesVerbal> rows, int start, int pageLength)
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

                CsEtatProcesVerbal c = new CsEtatProcesVerbal();
                c.Route = (Convert.IsDBNull(reader["Route"])) ? string.Empty : (System.String)reader["Route"];
                c.OriginalRoute = (Convert.IsDBNull(reader["Route"])) ? string.Empty : (System.String)reader["Route"];
                c.Nom = (Convert.IsDBNull(reader["Nom"])) ? string.Empty : (System.String)reader["Nom"];
                c.Commune = (Convert.IsDBNull(reader["Commune"])) ? string.Empty : (System.String)reader["Commune"];
                c.NumLot = (Convert.IsDBNull(reader["NumLot"])) ? string.Empty : (System.String)reader["NumLot"];
                c.Quartier = (Convert.IsDBNull(reader["Quartier"])) ? string.Empty : (System.String)reader["Quartier"];
                c.NumTel = (Convert.IsDBNull(reader["NumTel"])) ? string.Empty : (System.String)reader["NumTel"];
                c.Rue = (Convert.IsDBNull(reader["Rue"])) ? string.Empty : (System.String)reader["Rue"];
                c.NumPoteauProche = (Convert.IsDBNull(reader["NumPoteauProche"])) ? string.Empty : (System.String)reader["NumPoteauProche"];
                c.DateValidation = (Convert.IsDBNull(reader["DateValidation"])) ? string.Empty : (System.String)reader["DateValidation"];
                c.TypeDevis = (Convert.IsDBNull(reader["TypeDevis"])) ? string.Empty : (System.String)reader["TypeDevis"];
                c.Produit = (Convert.IsDBNull(reader["Produit"])) ? string.Empty : (System.String)reader["Produit"];
                c.Centre = (Convert.IsDBNull(reader["Centre"])) ? string.Empty : (System.String)reader["Centre"];
                if (Convert.IsDBNull(reader["totalTTC"]))
                    c.TotalTTC = null;
                else
                    c.TotalTTC = (System.Single)reader["totalTTC"];
                c.NumDevis = (Convert.IsDBNull(reader["NumDevis"])) ? string.Empty : (System.String)reader["NumDevis"];
                if (Convert.IsDBNull(reader["ordre"]))
                    c.Ordre = null;
                else
                    c.Ordre = (System.Byte)reader["ordre"];
                c.DateDebutTrvx = (Convert.IsDBNull(reader["DateDebutTrvx"])) ? null : (System.String)reader["DateDebutTrvx"];
                c.DateFinTrvx = (Convert.IsDBNull(reader["DateFinTrvx"])) ? null : (System.String)reader["DateFinTrvx"];
                c.Matricule = (Convert.IsDBNull(reader["Matricule"])) ? null : (System.String)reader["Matricule"];
                c.Chefequipe = (Convert.IsDBNull(reader["Chef equipe"])) ? null : (System.String)reader["Chef equipe"];
                c.ProcesVerbal = (Convert.IsDBNull(reader["ProcesVerbal"])) ? null : (System.String)reader["ProcesVerbal"];
                c.NumeroCTR = (Convert.IsDBNull(reader["NumeroCTR"])) ? null : (System.String)reader["NumeroCTR"];
                c.MeterSize = (Convert.IsDBNull(reader["MeterSize"])) ? null : (System.String)reader["MeterSize"];
                c.TYPECTR = (Convert.IsDBNull(reader["TYPECTR"])) ? null : (System.String)reader["TYPECTR"];
                c.MarqueCTR = (Convert.IsDBNull(reader["MarqueCTR"])) ? null : (System.String)reader["MarqueCTR"];
                c.Adresse = (Convert.IsDBNull(reader["Adresse"])) ? null : (System.String)reader["Adresse"];
                if (Convert.IsDBNull(reader["IndexPoseCTR"]))
                    c.IndexPoseCTR = null;
                else
                    c.IndexPoseCTR = (System.Int32)reader["IndexPoseCTR"];
                if (Convert.IsDBNull(reader["AnneeFabricationCTR"]))
                    c.AnneeFabricationCTR = null;
                else
                    c.AnneeFabricationCTR = (System.Int32)reader["AnneeFabricationCTR"];
                c.NearestRoute = (Convert.IsDBNull(reader["NearestRoute"])) ? string.Empty : (System.String)reader["NearestRoute"];
                c.NumeroGPS = (Convert.IsDBNull(reader["NumeroGPS"])) ? string.Empty : (System.String)reader["NumeroGPS"];
                c.MatriculeChefEquipe = (Convert.IsDBNull(reader["MatriculeChefEquipe"])) ? string.Empty : (System.String)reader["MatriculeChefEquipe"];
                c.NomChefEquipe = (Convert.IsDBNull(reader["NomChefEquipe"])) ? string.Empty : (System.String)reader["NomChefEquipe"];

                rows.Add(c);
            }
            return rows;
        }

        public static List<CsEtatProcesVerbal> EditerDevisPourBonControle(string pNumDevis, int pOrdre)
        {
            var cn = new SqlConnection(Session.GetSqlConnexionString());
            SqlCommand cmd = null;
            try
            {
                if (cn.State == ConnectionState.Closed)
                    cn.Open();
                cmd = new SqlCommand
                {
                    Connection = cn,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "SPX_DEVIS_DEVIS_EditionBonControle"
                };
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@NumDevis", pNumDevis);
                cmd.Parameters.AddWithValue("@Ordre", pOrdre);

                DBService.SetDBNullParametre(cmd.Parameters);

                IDataReader reader = cmd.ExecuteReader();
                var rows = new List<CsEtatProcesVerbal>();
                FillBonControle(reader, rows, int.MinValue, int.MaxValue);
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


        public static List<CsEtatBonRemboursement> FillBonRemboursement(IDataReader reader, List<CsEtatBonRemboursement> rows, int start, int pageLength)
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

                CsEtatBonRemboursement c = new CsEtatBonRemboursement();
                c.RefClient = (Convert.IsDBNull(reader["RefClient"])) ? string.Empty : (System.String)reader["RefClient"];
                c.NumDevis = (Convert.IsDBNull(reader["NumDevis"])) ? string.Empty : (System.String)reader["NumDevis"];
                if (Convert.IsDBNull(reader["ordre"]))
                    c.ordre = null;
                else
                    c.ordre = (System.Byte)reader["ordre"];
                c.Nom = (Convert.IsDBNull(reader["Nom"])) ? string.Empty : (System.String)reader["Nom"];
                c.Centre = (Convert.IsDBNull(reader["Centre"])) ? string.Empty : (System.String)reader["Centre"];
                c.TypeDevis = (Convert.IsDBNull(reader["TypeDevis"])) ? string.Empty : (System.String)reader["TypeDevis"];
                c.Produit = (Convert.IsDBNull(reader["Produit"])) ? string.Empty : (System.String)reader["Produit"];
                if (Convert.IsDBNull(reader["TotalARembourserHT"]))
                    c.TotalARembourserHT = null;
                else
                    c.TotalARembourserHT = (System.Decimal)reader["TotalARembourserHT"];
                if (Convert.IsDBNull(reader["TotalARembourserTTC"]))
                    c.TotalARembourserTTC = null;
                else
                    c.TotalARembourserTTC = (System.Decimal)reader["TotalARembourserTTC"];

                if (Convert.IsDBNull(reader["TotalRegleHT"]))
                    c.TotalRegleHT = null;
                else
                    c.TotalRegleHT = (System.Decimal)reader["TotalRegleHT"];

                if (Convert.IsDBNull(reader["TotalRegleTTC"]))
                    c.TotalRegleTTC = null;
                else
                    c.TotalRegleTTC = (System.Decimal)reader["TotalRegleTTC"];

                if (Convert.IsDBNull(reader["TotalARembourserHTOrdre"]))
                    c.TotalARembourserHTOrdre = null;
                else
                    c.TotalARembourserHTOrdre = (System.Decimal)reader["TotalARembourserHTOrdre"];

                if (Convert.IsDBNull(reader["TotalARembourserTTCOrdre"]))
                    c.TotalARembourserTTCOrdre = null;
                else
                    c.TotalARembourserTTCOrdre = (System.Decimal)reader["TotalARembourserTTCOrdre"];

                if (Convert.IsDBNull(reader["TotalRegleHTOrdre"]))
                    c.TotalRegleHTOrdre = null;
                else
                    c.TotalRegleHTOrdre = (System.Decimal)reader["TotalRegleHTOrdre"];

                if (Convert.IsDBNull(reader["TotalRegleTTCOrdre"]))
                    c.TotalRegleTTCOrdre = null;
                else
                    c.TotalRegleTTCOrdre = (System.Decimal)reader["TotalRegleTTCOrdre"];
                c.DepositReceipt = (Convert.IsDBNull(reader["DepositReceipt"])) ? string.Empty : (System.String)reader["DepositReceipt"];
                c.InvoiceReceipt = (Convert.IsDBNull(reader["InvoiceReceipt"])) ? string.Empty : (System.String)reader["InvoiceReceipt"];
                c.Comment = (Convert.IsDBNull(reader["Comment"])) ? null : (System.String)reader["Comment"];

                rows.Add(c);
            }
            return rows;
        }

        public static List<CsEtatBonRemboursement> EditerDevisPourBonRemboursement(string pNumDevis, string pComment, string pRefund)
        {
            var cn = new SqlConnection(Session.GetSqlConnexionString());
            SqlCommand cmd = null;
            try
            {
                if (cn.State == ConnectionState.Closed)
                    cn.Open();
                cmd = new SqlCommand
                {
                    Connection = cn,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "SPX_DEVIS_ELEMENTDEVIS_TotalRemboursement"
                };
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@NumDevis", pNumDevis);
                cmd.Parameters.AddWithValue("@Comment", pComment);
                cmd.Parameters.AddWithValue("@Refund", pRefund);

                DBService.SetDBNullParametre(cmd.Parameters);

                IDataReader reader = cmd.ExecuteReader();
                var rows = new List<CsEtatBonRemboursement>();
                FillBonRemboursement(reader, rows, int.MinValue, int.MaxValue);
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



        public static List<CsEtatDevis> FillDevisAEncaisser(IDataReader reader, List<CsEtatDevis> rows, int start, int pageLength)
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

                CsEtatDevis c = new CsEtatDevis();
                c.Numdevis = (Convert.IsDBNull(reader["NumDevis"])) ? string.Empty : (System.String)reader["NumDevis"];

                if (Convert.IsDBNull(reader["Ordre"]))
                    c.Ordre = null;
                else
                    c.Ordre = (byte?)reader["Ordre"];
                c.DateCreation = (Convert.IsDBNull(reader["DateCreation"])) ? string.Empty : (System.String)reader["DateCreation"];
                c.DateLimite = (Convert.IsDBNull(reader["DateLimite"])) ? string.Empty : (System.String)reader["DateLimite"];
                c.TypeDevis = (Convert.IsDBNull(reader["TypeDevis"])) ? string.Empty : (System.String)reader["TypeDevis"];
                c.Etape = (Convert.IsDBNull(reader["Etape"])) ? string.Empty : (System.String)reader["Etape"];
                if (Convert.IsDBNull(reader["Montant"]))
                    c.Montant = string.Empty;
                else
                    c.Montant = decimal.Parse(reader["Montant"].ToString()).ToString("N0");
                c.Centre = (Convert.IsDBNull(reader["Centre"])) ? string.Empty : (System.String)reader["Centre"];
                c.Produit = (Convert.IsDBNull(reader["Produit"])) ? string.Empty : (System.String)reader["Produit"];

                rows.Add(c);
            }
            return rows;
        }

        public static List<CsEtatDevis> EditerDevisAEncaisser()
        {
            var cn = new SqlConnection(Session.GetSqlConnexionString());
            SqlCommand cmd = null;
            try
            {
                if (cn.State == ConnectionState.Closed)
                    cn.Open();
                cmd = new SqlCommand
                {
                    Connection = cn,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "SPX_DEVIS_DEVIS_EditionDevisAEncCaisser"
                };

                DBService.SetDBNullParametre(cmd.Parameters);

                IDataReader reader = cmd.ExecuteReader();
                var rows = new List<CsEtatDevis>();
                FillDevisAEncaisser(reader, rows, int.MinValue, int.MaxValue);
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
         

        public static List<CsEtatDevis> FillDevisBranchementAFinaliser(IDataReader reader, List<CsEtatDevis> rows, int start, int pageLength)
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

                CsEtatDevis c = new CsEtatDevis();
                c.Numdevis = (Convert.IsDBNull(reader["NUMDEVIS"])) ? string.Empty : (System.String)reader["NUMDEVIS"];
                c.DCAISSE = (Convert.IsDBNull(reader["DCAISSE"])) ? string.Empty : (System.String)reader["DCAISSE"];
                c.NumeroCTR = (Convert.IsDBNull(reader["NumeroCTR"])) ? string.Empty : (System.String)reader["NumeroCTR"];
                c.DATEBRANCHEMENT = (Convert.IsDBNull(reader["DATEBRANCHEMENT"])) ? string.Empty : (System.String)reader["DATEBRANCHEMENT"];
                c.DEMANDE = (Convert.IsDBNull(reader["DEMANDE"])) ? string.Empty : (System.String)reader["DEMANDE"];
                c.Nom = (Convert.IsDBNull(reader["Nom"])) ? string.Empty : (System.String)reader["Nom"];
                c.Centre = (Convert.IsDBNull(reader["CENTRE"])) ? string.Empty : (System.String)reader["CENTRE"];
                c.AG = (Convert.IsDBNull(reader["AG"])) ? string.Empty : (System.String)reader["AG"];

                rows.Add(c);
            }
            return rows;
        }

        public static List<CsEtatDevis> EditerDevisBranchementAFinaliser()
        {
            var cn = new SqlConnection(Session.GetSqlConnexionString());
            SqlCommand cmd = null;
            try
            {
                if (cn.State == ConnectionState.Closed)
                    cn.Open();
                cmd = new SqlCommand
                {
                    Connection = cn,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "SPX_DEVIS_DEVIS_EditionBranchementAFinaliser"
                };

                DBService.SetDBNullParametre(cmd.Parameters);

                IDataReader reader = cmd.ExecuteReader();
                var rows = new List<CsEtatDevis>();
                FillDevisBranchementAFinaliser(reader, rows, int.MinValue, int.MaxValue);
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
*/
        public static List<CsEtatBonDeSortie> EditerDevis(int pIdDevis, int pOrdre, bool pIsSummary, string pMatricule)
        {
            List<CsEtatBonDeSortie> Listresult = new List<CsEtatBonDeSortie>();
            try
            {
                DataTable dt = DevisProcedures.DEVIS_DEVIS_EditionDEvis(pIdDevis, pOrdre, pIsSummary, pMatricule);
                List<CsEtatBonDeSortie> ListBonDeSortie = Entities.GetEntityListFromQuery<CsEtatBonDeSortie>(dt);
                if (ListBonDeSortie != null && ListBonDeSortie.Count != 0)
                {
                    
                    decimal MontanttotalTTC = FonctionDevis.RetourneMontantTotalDevis(ListBonDeSortie[0].numdevis);
                    decimal MontantTotalRemboursableHT = FonctionDevis.RetourneMontantRemboursableHT(ListBonDeSortie[0].numdevis);
                    decimal MontantTotalRemboursableTTC = FonctionDevis.RetourneMontantRemboursableHT(ListBonDeSortie[0].numdevis) + Math.Round((FonctionDevis.RetourneMontantRemboursableHT(ListBonDeSortie[0].numdevis) * (decimal)ListBonDeSortie[0].Taxe));
                    decimal MontantBalance = FonctionDevis.RetourneMontantTotalDevis(ListBonDeSortie[0].numdevis) - (decimal)ListBonDeSortie[0].DEPOSIT;

                    foreach (CsEtatBonDeSortie BonDeSortie in ListBonDeSortie)
                    {
                        if (BonDeSortie.Isdefault == true)
                        {
                            BonDeSortie.Prix_Unitaire = 0;
                            BonDeSortie.montantTTC = 0;
                        }
                        BonDeSortie.totalTTC = MontanttotalTTC;
                        BonDeSortie.TotalRemboursableHT = MontantTotalRemboursableHT;
                        BonDeSortie.TotalRemboursableTTC = MontantTotalRemboursableTTC;
                        BonDeSortie.Balance = MontantBalance;
                        Listresult.Add(BonDeSortie);
                    }
                }
                return Listresult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<CsEtatBonDeSortie> EditerDevisPourBonDeSortie(int pIdDevis, int pOrdre, bool pIsSummary, string pMatricule)
        {
            List<CsEtatBonDeSortie> Listresult = new List<CsEtatBonDeSortie>();
            try
            {
                DataTable dt = DevisProcedures.DEVIS_DEVIS_EditionBonSortie(pIdDevis, pOrdre, pIsSummary, pMatricule);
                List<CsEtatBonDeSortie> ListBonDeSortie = Entities.GetEntityListFromQuery<CsEtatBonDeSortie>(dt);
                //List<CsEtatBonDeSortie> ListBonDeSortie = DevisProcedures.DEVIS_DEVIS_EditionBonSortie_(pIdDevis, pOrdre, pIsSummary, pMatricule).ToList();
                foreach (CsEtatBonDeSortie BonDeSortie in ListBonDeSortie)
                {
                    BonDeSortie.totalTTC = FonctionDevis.RetourneMontantDevis(BonDeSortie.numdevis, pIsSummary);
                    BonDeSortie.TotalRemboursableHT = FonctionDevis.RetourneMontantRemboursableHT(BonDeSortie.numdevis);
                    BonDeSortie.TotalRemboursableTTC = FonctionDevis.RetourneMontantRemboursableHT(BonDeSortie.numdevis) + Math.Round((FonctionDevis.RetourneMontantRemboursableHT(BonDeSortie.numdevis) * (decimal)BonDeSortie.Taxe));
                    BonDeSortie.Balance = FonctionDevis.RetourneMontantDevis(BonDeSortie.numdevis, pIsSummary) - (decimal)BonDeSortie.DEPOSIT;
                    Listresult.Add(BonDeSortie);
                }
                return Listresult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

   

        public static List<CsEtatProcesVerbal> EditerDevisPourProcesVerbal(int pIdDevis, int pOrdre)
        {
            try
            {
                return Entities.GetEntityListFromQuery<CsEtatProcesVerbal>(DevisProcedures.DEVIS_DEVIS_EditionProcesVerbal(pIdDevis, pOrdre));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<CsEtatProcesVerbal> EditerDevisPourBonControle(int pIdDevis, int pOrdre)
        {
            try
            {
                return Entities.GetEntityListFromQuery<CsEtatProcesVerbal>(DevisProcedures.DEVIS_DEVIS_EditionBonControle(pIdDevis, pOrdre));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<CsEtatBonRemboursement> EditerDevisPourBonRemboursement(int pNumDevis, string pComment, decimal? pRefund)
        {
            List<CsEtatBonRemboursement> ListBonRemboursement = new List<CsEtatBonRemboursement>();
            try
            {
                ListBonRemboursement = Entities.GetEntityListFromQuery<CsEtatBonRemboursement>(DevisProcedures.DEVIS_DEVIS_EditionBonRemboursement(pNumDevis, pComment, pRefund));
                //var result = Entities.GetEntityListFromQuery<CsEtatBonRemboursement>(DevisProcedures.DEVIS_DEVIS_EditionBonRemboursement(pNumDevis, pComment, pRefund));
                //foreach (var item in result)
                //{
                //    item.DepositReceipt = item.DepositReceipt != string.Empty ? item.DepositReceipt.Substring(0, 3) + "." + item.DepositReceipt.Substring(4, 9) + item.DepositReceipt.Substring(item.DepositReceipt.Length - 1, 5) : string.Empty;
                //    item.InvoiceReceipt = item.CaisseTranscaisb + "." + item.AcquitTranscaisb + "." + item.MatriculeTranscaisb;
                //    ListBonRemboursement.Add(item);
                //}
                return ListBonRemboursement;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
