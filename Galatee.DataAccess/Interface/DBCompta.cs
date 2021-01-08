using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Galatee.Structure;

namespace Galatee.DataAccess
{
     public class DBCompta : Galatee.DataAccess.Parametrage.DbBase
    {
         public DBCompta()
        {
            ConnectionString = Session.GetSqlConnexionString();
            
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

        public List<CsLibelle> RetourneTousMoisComptables()
        {
            CsLibelle _ta;
            List<CsLibelle> ListeTA = new List<CsLibelle>();

            cn = new SqlConnection(ConnectionString);
            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = EnumProcedureStockee.SelectARRETE;

            try
            {

                if (this.cn.State == ConnectionState.Closed)
                    this.cn.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                if (!reader.HasRows)
                {
                    reader.Close();
                   // throw new Exception("Erreur survenue.");
                }
                while (reader.Read())
                {
                    _ta = new CsLibelle();
                    _ta.LIBELLE = reader.GetValue(0).ToString().Trim();
                    ListeTA.Add(_ta);
                }
                //Fermeture reader
                reader.Close();
                return ListeTA;
            }
            catch (Exception ex)
            {
                throw new Exception(EnumProcedureStockee.SelectARRETE + ":" + ex.Message);
            }
            finally
            {
                if (this.cn.State == ConnectionState.Open)
                    this.cn.Close(); // Fermeture de la connection 
                cmd.Dispose();
            }
        }

        public List<CsComptable> RetourneComptaDuMoisComptable(string pMoisCompta,string pTypeEnc)
        {
            CsComptable _ta;
            List<CsComptable> ListeTA = new List<CsComptable>();
            ConnectionString = Session.GetSqlConnexionStringAbo07();
            cn = new SqlConnection(ConnectionString);
            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = EnumProcedureStockee.RetourneComptaParMoisCompta;
            cmd.Parameters.Clear();
            cmd.Parameters.Add("@MOISCOMPTA", SqlDbType.VarChar).Value = pMoisCompta;
            cmd.Parameters.Add("@TYPEENC", SqlDbType.VarChar).Value = pTypeEnc;

            DBBase.SetDBNullParametre(cmd.Parameters);
            try
            {

                if (this.cn.State == ConnectionState.Closed)
                    this.cn.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                if (!reader.HasRows)
                {
                    reader.Close();
                    // throw new Exception("Erreur survenue.");
                }
                while (reader.Read())
                {
                    _ta = new CsComptable();
                    _ta.MOISCOMPTA = string.IsNullOrEmpty(reader.GetValue(0).ToString()) ? string.Empty : reader.GetValue(0).ToString().Trim();
                    _ta.CENTRE = string.IsNullOrEmpty(reader.GetValue(1).ToString()) ? string.Empty : reader.GetValue(1).ToString().Trim();
                    _ta.NATURE = string.IsNullOrEmpty(reader.GetValue(2).ToString()) ? string.Empty : reader.GetValue(2).ToString().Trim();
                    _ta.COMMUNE = string.IsNullOrEmpty(reader.GetValue(3).ToString()) ? string.Empty : reader.GetValue(3).ToString().Trim();
                    _ta.PRODUIT = string.IsNullOrEmpty(reader.GetValue(4).ToString()) ? string.Empty : reader.GetValue(4).ToString().Trim();
                    _ta.REDEVANCE = string.IsNullOrEmpty(reader.GetValue(5).ToString()) ? string.Empty : reader.GetValue(5).ToString().Trim();
                    _ta.REDHT = string.IsNullOrEmpty(reader.GetValue(6).ToString()) ? 0 : Convert.ToDecimal(reader.GetValue(6).ToString().Trim());
                    _ta.REDTAXE = string.IsNullOrEmpty(reader.GetValue(7).ToString()) ? 0 : Convert.ToDecimal(reader.GetValue(7).ToString().Trim());
                    ListeTA.Add(_ta);
                }
                //Fermeture reader
                reader.Close();
                return ListeTA;
            }
            catch (Exception ex)
            {
                throw new Exception(EnumProcedureStockee.SelectARRETE + ":" + ex.Message);
            }
            finally
            {
                if (this.cn.State == ConnectionState.Open)
                    this.cn.Close(); // Fermeture de la connection 
                cmd.Dispose();
            }
        }

        public List<CsSchema> RetourneDonneesTableSchema()
        {
            CsSchema _ta;
            List<CsSchema> ListeTA = new List<CsSchema>();
            ConnectionString = Session.GetSqlConnexionStringAbo07();
            cn = new SqlConnection(ConnectionString);
            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = EnumProcedureStockee.SelectSCHEMAS;
            cmd.Parameters.Clear();

            DBBase.SetDBNullParametre(cmd.Parameters);
            try
            {

                if (this.cn.State == ConnectionState.Closed)
                    this.cn.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                if (!reader.HasRows)
                {
                    reader.Close();
                    throw new Exception(EnumProcedureStockee.SelectSCHEMAS + ":" + "n'a retourné aucun jeu de caractère");
                }
                while (reader.Read())
                {
                    _ta = new CsSchema();
                    _ta.CENTRE = string.IsNullOrEmpty(reader.GetValue(0).ToString()) ? string.Empty : reader.GetValue(0).ToString().Trim();
                    _ta.NUM = string.IsNullOrEmpty(reader.GetValue(1).ToString()) ? string.Empty : reader.GetValue(1).ToString().Trim();
                    _ta.GENE0 = string.IsNullOrEmpty(reader.GetValue(2).ToString()) ? string.Empty : reader.GetValue(2).ToString().Trim();
                    _ta.ANAL0 = string.IsNullOrEmpty(reader.GetValue(3).ToString()) ? string.Empty : reader.GetValue(3).ToString().Trim();
                    _ta.GENE1 = string.IsNullOrEmpty(reader.GetValue(4).ToString()) ? string.Empty : reader.GetValue(4).ToString().Trim();
                    _ta.ANAL1 = string.IsNullOrEmpty(reader.GetValue(5).ToString()) ? string.Empty : reader.GetValue(5).ToString().Trim();
                    _ta.SENS1 = string.IsNullOrEmpty(reader.GetValue(6).ToString()) ? string.Empty : reader.GetValue(6).ToString().Trim();
                    _ta.SIGN1 = string.IsNullOrEmpty(reader.GetValue(7).ToString()) ? string.Empty : reader.GetValue(7).ToString().Trim();
                    _ta.GENE2 = string.IsNullOrEmpty(reader.GetValue(8).ToString()) ? string.Empty : reader.GetValue(8).ToString().Trim();
                    _ta.ANAL2 = string.IsNullOrEmpty(reader.GetValue(9).ToString()) ? string.Empty : reader.GetValue(9).ToString().Trim();
                    _ta.SENS2 = string.IsNullOrEmpty(reader.GetValue(10).ToString()) ? string.Empty : reader.GetValue(10).ToString().Trim();
                    _ta.SIGN2 = string.IsNullOrEmpty(reader.GetValue(11).ToString()) ? string.Empty : reader.GetValue(11).ToString().Trim();
                    _ta.GENE3 = string.IsNullOrEmpty(reader.GetValue(12).ToString()) ? string.Empty : reader.GetValue(12).ToString().Trim();
                    _ta.ANAL3 = string.IsNullOrEmpty(reader.GetValue(13).ToString()) ? string.Empty : reader.GetValue(13).ToString().Trim();
                    _ta.SENS3 = string.IsNullOrEmpty(reader.GetValue(14).ToString()) ? string.Empty : reader.GetValue(14).ToString().Trim();
                    _ta.SIGN3 = string.IsNullOrEmpty(reader.GetValue(15).ToString()) ? string.Empty : reader.GetValue(15).ToString().Trim();
                    _ta.GENE4 = string.IsNullOrEmpty(reader.GetValue(16).ToString()) ? string.Empty : reader.GetValue(16).ToString().Trim();
                    _ta.ANAL4 = string.IsNullOrEmpty(reader.GetValue(17).ToString()) ? string.Empty : reader.GetValue(17).ToString().Trim();
                    _ta.SENS4 = string.IsNullOrEmpty(reader.GetValue(18).ToString()) ? string.Empty : reader.GetValue(18).ToString().Trim();
                    _ta.SIGN4 = string.IsNullOrEmpty(reader.GetValue(19).ToString()) ? string.Empty : reader.GetValue(19).ToString().Trim();
                    _ta.GENE5 = string.IsNullOrEmpty(reader.GetValue(20).ToString()) ? string.Empty : reader.GetValue(20).ToString().Trim();
                    _ta.ANAL5 = string.IsNullOrEmpty(reader.GetValue(21).ToString()) ? string.Empty : reader.GetValue(21).ToString().Trim();
                    _ta.SENS5 = string.IsNullOrEmpty(reader.GetValue(22).ToString()) ? string.Empty : reader.GetValue(22).ToString().Trim();
                    _ta.SIGN5 = string.IsNullOrEmpty(reader.GetValue(23).ToString()) ? string.Empty : reader.GetValue(23).ToString().Trim();
                    _ta.GENE6 = string.IsNullOrEmpty(reader.GetValue(24).ToString()) ? string.Empty : reader.GetValue(24).ToString().Trim();
                    _ta.ANAL6 = string.IsNullOrEmpty(reader.GetValue(25).ToString()) ? string.Empty : reader.GetValue(25).ToString().Trim();
                    _ta.SENS6 = string.IsNullOrEmpty(reader.GetValue(26).ToString()) ? string.Empty : reader.GetValue(26).ToString().Trim();
                    _ta.SIGN6 = string.IsNullOrEmpty(reader.GetValue(27).ToString()) ? string.Empty : reader.GetValue(27).ToString().Trim();
                    ListeTA.Add(_ta);
                }
                //Fermeture reader
                reader.Close();
                return ListeTA;
            }
            catch (Exception ex)
            {
                throw new Exception(EnumProcedureStockee.SelectSCHEMAS + ":" + ex.Message);
            }
            finally
            {
                if (this.cn.State == ConnectionState.Open)
                    this.cn.Close(); // Fermeture de la connection 
                cmd.Dispose();
            }
        }

        public List<CsParametre> RetourneParametresCompta()
        {
            CsParametre _ta;
            List<CsParametre> ListeTA = new List<CsParametre>();

            cn = new SqlConnection(ConnectionString);
            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SPX_INTERFACE_RETOURNE_PARAMETRES";

            try
            {

                if (this.cn.State == ConnectionState.Closed)
                    this.cn.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                if (!reader.HasRows)
                {
                    reader.Close();
                    // throw new Exception("Erreur survenue.");
                }
                while (reader.Read())
                {
                    _ta = new CsParametre();
                    _ta.GestionAutoMoisCompt =string.IsNullOrEmpty(reader.GetValue(0).ToString()) ? string.Empty: reader.GetValue(0).ToString().Trim();
                    _ta.TypeExportFichier = string.IsNullOrEmpty(reader.GetValue(1).ToString()) ? string.Empty : reader.GetValue(1).ToString().Trim();
                    _ta.ModeTraitement = string.IsNullOrEmpty(reader.GetValue(2).ToString()) ? string.Empty : reader.GetValue(2).ToString().Trim();
                    _ta.CheminFichier = string.IsNullOrEmpty(reader.GetValue(3).ToString()) ? string.Empty : reader.GetValue(3).ToString().Trim();
                    _ta.CompteAnalytique = string.IsNullOrEmpty(reader.GetValue(4).ToString()) ? string.Empty : reader.GetValue(4).ToString().Trim();
                    ListeTA.Add(_ta);
                }
                //Fermeture reader
                reader.Close();
                return ListeTA;
            }
            catch (Exception ex)
            {
                throw new Exception("SPX_INTERFACE_RETOURNE_PARAMETRES" + ":" + ex.Message);
            }
            finally
            {
                if (this.cn.State == ConnectionState.Open)
                    this.cn.Close(); // Fermeture de la connection 
                cmd.Dispose();
            }
        }

        public List<CsLibelle> RetourneActiviteProduits()
        {
            CsLibelle _ta;
            List<CsLibelle> ListeTA = new List<CsLibelle>();

            cn = new SqlConnection(ConnectionString);
            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = EnumProcedureStockee.SelectActiviteProduit;

            try
            {

                if (this.cn.State == ConnectionState.Closed)
                    this.cn.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                if (!reader.HasRows)
                {
                    reader.Close();
                    throw new Exception(EnumProcedureStockee.SelectActiviteProduit + ":" + " n'a retourné aucun jeu de caractères");
                }
                while (reader.Read())
                {
                    _ta = new CsLibelle();
                    _ta.CODE = string.IsNullOrEmpty(reader.GetValue(0).ToString()) ? string.Empty : reader.GetValue(0).ToString().Trim();
                    _ta.LIBELLE = string.IsNullOrEmpty(reader.GetValue(1).ToString()) ? string.Empty : reader.GetValue(1).ToString().Trim();
                    ListeTA.Add(_ta);
                }
                //Fermeture reader
                reader.Close();
                return ListeTA;
            }
            catch (Exception ex)
            {
                throw new Exception(EnumProcedureStockee.SelectActiviteProduit + ":" + ex.Message);
            }
            finally
            {
                if (this.cn.State == ConnectionState.Open)
                    this.cn.Close(); // Fermeture de la connection 
                cmd.Dispose();
            }
        }

        public List<CsJournal> RetourneJournals()
        {
            CsJournal _ta;
            List<CsJournal> ListeTA = new List<CsJournal>();

            cn = new SqlConnection(ConnectionString);
            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = EnumProcedureStockee.SelectJournal;

            try
            {

                if (this.cn.State == ConnectionState.Closed)
                    this.cn.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                if (!reader.HasRows)
                {
                    reader.Close();
                    throw new Exception(EnumProcedureStockee.SelectJournal + ":" + " n'a retourné aucun jeu de caractères");
                }
                while (reader.Read())
                {
                    _ta = new CsJournal();
                    _ta.CodeAgence = string.IsNullOrEmpty(reader.GetValue(0).ToString()) ? string.Empty : reader.GetValue(0).ToString().Trim();
                    _ta.Ventes = string.IsNullOrEmpty(reader.GetValue(1).ToString()) ? string.Empty : reader.GetValue(1).ToString().Trim();
                    _ta.Encaiss = string.IsNullOrEmpty(reader.GetValue(2).ToString()) ? string.Empty : reader.GetValue(2).ToString().Trim();
                    _ta.PaieCaisse = string.IsNullOrEmpty(reader.GetValue(3).ToString()) ? string.Empty : reader.GetValue(3).ToString().Trim();
                    _ta.OperDivers = string.IsNullOrEmpty(reader.GetValue(4).ToString()) ? string.Empty : reader.GetValue(4).ToString().Trim();
                    _ta.CodeService = string.IsNullOrEmpty(reader.GetValue(5).ToString()) ? string.Empty : reader.GetValue(5).ToString().Trim();
                    ListeTA.Add(_ta);
                }
                //Fermeture reader
                reader.Close();
                return ListeTA;
            }
            catch (Exception ex)
            {
                throw new Exception(EnumProcedureStockee.SelectJournal + ":" + ex.Message);
            }
            finally
            {
                if (this.cn.State == ConnectionState.Open)
                    this.cn.Close(); // Fermeture de la connection 
                cmd.Dispose();
            }
        }

        public List<CsCompteCritere> RetourneCsCompteCriteres()
        {
            CsCompteCritere _ta;
            List<CsCompteCritere> ListeTA = new List<CsCompteCritere>();

            cn = new SqlConnection(ConnectionString);
            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = EnumProcedureStockee.SelectCompteCritere;

            try
            {

                if (this.cn.State == ConnectionState.Closed)
                    this.cn.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                if (!reader.HasRows)
                {
                    reader.Close();
                    throw new Exception(EnumProcedureStockee.SelectCompteCritere + ":" + " n'a retourné aucun jeu de caractères");
                }
                while (reader.Read())
                {
                    _ta = new CsCompteCritere();
                    _ta.Centre = string.IsNullOrEmpty(reader.GetValue(0).ToString()) ? string.Empty : reader.GetValue(0).ToString().Trim();
                    _ta.Coper = string.IsNullOrEmpty(reader.GetValue(1).ToString()) ? string.Empty : reader.GetValue(1).ToString().Trim();
                    _ta.Produit = string.IsNullOrEmpty(reader.GetValue(2).ToString()) ? string.Empty : reader.GetValue(2).ToString().Trim();
                    _ta.catCli = string.IsNullOrEmpty(reader.GetValue(3).ToString()) ? string.Empty : reader.GetValue(3).ToString().Trim();
                    _ta.CompteGene = string.IsNullOrEmpty(reader.GetValue(4).ToString()) ? string.Empty : reader.GetValue(4).ToString().Trim();
                    ListeTA.Add(_ta);
                }
                //Fermeture reader
                reader.Close();
                return ListeTA;
            }
            catch (Exception ex)
            {
                throw new Exception(EnumProcedureStockee.SelectCompteCritere + ":" + ex.Message);
            }
            finally
            {
                if (this.cn.State == ConnectionState.Open)
                    this.cn.Close(); // Fermeture de la connection 
                cmd.Dispose();
            }
        }

        public List<CsCorrespondance> RetourneCorrespondanceCompta()
        {
            CsCorrespondance _ta;
            List<CsCorrespondance> ListeTA = new List<CsCorrespondance>();

            cn = new SqlConnection(ConnectionString);
            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = EnumProcedureStockee.SelectAllCorrespondance;

            try
            {

                if (this.cn.State == ConnectionState.Closed)
                    this.cn.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                if (!reader.HasRows)
                {
                    reader.Close();
                    // throw new Exception("Erreur survenue.");
                }
                while (reader.Read())
                {
                    _ta = new CsCorrespondance();
                    _ta.ClasseCompte = string.IsNullOrEmpty(reader.GetValue(0).ToString()) ? string.Empty : reader.GetValue(0).ToString().Trim();
                    _ta.Ci = string.IsNullOrEmpty(reader.GetValue(1).ToString()) ? string.Empty : reader.GetValue(1).ToString().Trim();
                    _ta.Filiere = string.IsNullOrEmpty(reader.GetValue(2).ToString()) ? string.Empty : reader.GetValue(2).ToString().Trim();
                    _ta.SScompte = string.IsNullOrEmpty(reader.GetValue(3).ToString()) ? string.Empty : reader.GetValue(3).ToString().Trim();
                    _ta.Localisation = string.IsNullOrEmpty(reader.GetValue(4).ToString()) ? string.Empty : reader.GetValue(4).ToString().Trim();
                    _ta.CodeAgence = string.IsNullOrEmpty(reader.GetValue(5).ToString()) ? string.Empty : reader.GetValue(5).ToString().Trim();
                    ListeTA.Add(_ta);
                }
                //Fermeture reader
                reader.Close();
                return ListeTA;
            }
            catch (Exception ex)
            {
                throw new Exception(EnumProcedureStockee.SelectAllCorrespondance + ":" + ex.Message);
            }
            finally
            {
                if (this.cn.State == ConnectionState.Open)
                    this.cn.Close(); // Fermeture de la connection 
                cmd.Dispose();
            }
        }

        public List<CsLibelle> RetourneSousMenuCompta()
        {
            CsLibelle _ta;
            List<CsLibelle> ListeTA = new List<CsLibelle>();

            cn = new SqlConnection(ConnectionString);
            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SPX_INTERFACE_RETOURNE_SOUS_MENUS_COMPTA_FAC";

            try
            {
                if (this.cn.State == ConnectionState.Closed)
                    this.cn.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                if (!reader.HasRows)
                {
                    reader.Close();
                    // throw new Exception("Erreur survenue.");
                }
                while (reader.Read())
                {
                    _ta = new CsLibelle();
                    _ta.CODE = string.IsNullOrEmpty(reader.GetValue(0).ToString()) ? string.Empty : reader.GetValue(0).ToString().Trim();
                    _ta.LIBELLE = string.IsNullOrEmpty(reader.GetValue(1).ToString()) ? string.Empty : reader.GetValue(1).ToString().Trim();
                    ListeTA.Add(_ta);
                }
                //Fermeture reader
                reader.Close();
                return ListeTA;
            }
            catch (Exception ex)
            {
                throw new Exception("SPX_INTERFACE_RETOURNE_SOUS_MENUS_COMPTA_FAC" + ":" + ex.Message);
            }
            finally
            {
                if (this.cn.State == ConnectionState.Open)
                    this.cn.Close(); // Fermeture de la connection 
                cmd.Dispose();
            }
        }
    }
}
