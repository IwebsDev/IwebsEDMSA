using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
////
using System.Configuration;
using System.Data.SqlClient;
using Galatee.Structure;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;

//using GestionChequeImpaye.DataAccess;e
//using GestionChequeImpaye.Common;
//using GestionChequeImpaye.DataAcess;

namespace Galatee.DataAccess
{
    public static class Session
    {
        //private static List<SqlConnection> _ConnectionCollection;
        public static string GetSqlConnexionString()
        {
            try
            {
                string Message = string.Empty;
                // Nouvelle gestion
                //var connexion = ConfigurationManager.ConnectionStrings["galadbEntities"].ConnectionString;
                var connexion = GetsqlString();
                if (!IsConnexionValide(connexion, ref Message))
                    throw new Exception("Enregistrement annulé :" + Environment.NewLine + Message);
                return connexion;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static bool IsConnexionValide(string connString, ref string Erreur)
        {
            SqlConnection sqlConnection = new SqlConnection(connString);
            Erreur = string.Empty;
            //- Test des informations de connexion
            if (string.IsNullOrEmpty(sqlConnection.DataSource))
            {
                Erreur = "Serveur sql non renseigné";
                return false;
            }
            if (string.IsNullOrEmpty(sqlConnection.Database))
            {
                Erreur = "Base de données non renseignée";
                return false;
            }

            return EstSqlConnexionStringValide(connString, ref Erreur);
        }

        //public static string GetSqlConnexionStringAbo07()
        //{
        //    try
        //    {
        //        string Message = string.Empty;
        //        // Nouvelle gestion
        //        var connexion = ConfigurationManager.ConnectionStrings["ABO07Entities"].ConnectionString;
        //        if (!IsConnexionValide(connexion, ref Message))
        //            throw new Exception("Enregistrement annulé :" + Environment.NewLine + Message);
        //        return connexion;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        public static string GetSqlConnexionStringAbo07()
        {
            try
            {
                string Message = string.Empty;
                // Nouvelle gestion
                //var connexion = ConfigurationManager.ConnectionStrings["galadbEntities"].ConnectionString;
                var connexion = GetsqlStringAbo07();
                if (!IsConnexionValide(connexion, ref Message))
                    throw new Exception("Enregistrement annulé :" + Environment.NewLine + Message);
                return connexion;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public static string GetsqlString()
        //{

        //    ConnectionStringSettingsCollection lesChainesDeConnexion = ConfigurationManager.ConnectionStrings;
        //    ConnectionStringSettings laChaineDeConnexion = lesChainesDeConnexion["galadbEntities"];
        //    string[] caract = laChaineDeConnexion.ConnectionString.Split(new string[] { "data source" }, StringSplitOptions.RemoveEmptyEntries);
        //    string[] chaine = caract[1].Split(new string[] { "MultipleActiveResultSetsnew" }, StringSplitOptions.RemoveEmptyEntries);
        //    string[] connexion = chaine[0].Split(new Char[] { ';' });
        //    string connectionString = "data source" + connexion[0] + ";" + connexion[1] + ";" + connexion[2] + ";" + connexion[3];

        //    return connectionString;
           
        //}

        public static string GetsqlString()
        {
            /* ZEG 01/06/2020
            ConnectionStringSettingsCollection lesChainesDeConnexion = ConfigurationManager.ConnectionStrings;
            ConnectionStringSettings laChaineDeConnexion = lesChainesDeConnexion["galadbEntities"];

            string[] caract = laChaineDeConnexion.ConnectionString.Split(new string[] { "data source" }, StringSplitOptions.RemoveEmptyEntries);
            string[] chaine = caract[1].Split(new string[] { "MultipleActiveResultSetsnew" }, StringSplitOptions.RemoveEmptyEntries);
            string[] connexion = chaine[0].Split(new Char[] { ';' });
            string connectionString = "data source" + connexion[0] + ";" + connexion[1] + ";" + connexion[2] + ";" + connexion[3];
             */


            ConnectionStringSettingsCollection lesChainesDeConnexion = ConfigurationManager.ConnectionStrings;
            ConnectionStringSettings laChaineDeConnexion = lesChainesDeConnexion["galadbEntities"];

            string[] caract = laChaineDeConnexion.ConnectionString.Split(new string[] { "data source" }, StringSplitOptions.RemoveEmptyEntries);
            string[] chaine = caract[1].Split(new string[] { "MultipleActiveResultSetsnew" }, StringSplitOptions.RemoveEmptyEntries);
            string[] connexion = chaine[0].Split(new Char[] { ';' });
            string[] uid = connexion[2].Split(new string[] { "user id=" }, StringSplitOptions.RemoveEmptyEntries);
            string[] pwd = connexion[3].Split(new string[] { "password=" }, StringSplitOptions.RemoveEmptyEntries);

            string userId = string.Empty;
            string password = string.Empty;

            if (!string.IsNullOrEmpty(uid[0]))
                userId = INOVA.GALATEE.SECURITY.Crypteur.DecrypterText(uid[0]);

            if (!string.IsNullOrEmpty(pwd[0]))
                password = INOVA.GALATEE.SECURITY.Crypteur.DecrypterText(pwd[0]);


            string connectionString = "data source" + connexion[0] + ";" + connexion[1] + ";user id=" + userId + ";password=" + password;


            return connectionString;

        }
        public static string GetsqlString(string NomConnextion)
        {

            ConnectionStringSettingsCollection lesChainesDeConnexion = ConfigurationManager.ConnectionStrings;
            ConnectionStringSettings laChaineDeConnexion = lesChainesDeConnexion[NomConnextion];
            string[] caract = laChaineDeConnexion.ConnectionString.Split(new string[] { "data source" }, StringSplitOptions.RemoveEmptyEntries);
            string[] chaine = caract[1].Split(new string[] { "MultipleActiveResultSetsnew" }, StringSplitOptions.RemoveEmptyEntries);
            string[] connexion = chaine[0].Split(new Char[] { ';' });
            string[] userid = connexion[3].Split('"');
            string connectionString = "data source" + connexion[0] + ";" + userid[0] + ";" + connexion[1];
            //"Data Source= LAP-SI-GAL-10.inova.local:1521/orcl.inova.local;User Id=SYSTEM;Password=passwordA1;"
            return connectionString;

        }
        //public static string GetsqlStringAbo07()
        //{

        //    ConnectionStringSettingsCollection lesChainesDeConnexion = ConfigurationManager.ConnectionStrings;
        //    ConnectionStringSettings laChaineDeConnexion = lesChainesDeConnexion["ABO07Entities"];
        //    string[] caract = laChaineDeConnexion.ConnectionString.Split(new string[] { "data source" }, StringSplitOptions.RemoveEmptyEntries);
        //    string[] chaine = caract[1].Split(new string[] { "MultipleActiveResultSetsnew" }, StringSplitOptions.RemoveEmptyEntries);
        //    string[] connexion = chaine[0].Split(new Char[] { ';' });
        //    string connectionString = "data source" + connexion[0] + ";" + connexion[1] + ";" + connexion[2] + ";" + connexion[3];

        //    return connectionString;

        //}

        public static string GetsqlStringAbo07()
        {

            /* ZEG 01/06/2020
             ConnectionStringSettingsCollection lesChainesDeConnexion = ConfigurationManager.ConnectionStrings;
             ConnectionStringSettings laChaineDeConnexion = lesChainesDeConnexion["ABO07Entities"];
             string[] caract = laChaineDeConnexion.ConnectionString.Split(new string[] { "data source" }, StringSplitOptions.RemoveEmptyEntries);
             string[] chaine = caract[1].Split(new string[] { "MultipleActiveResultSetsnew" }, StringSplitOptions.RemoveEmptyEntries);
             string[] connexion = chaine[0].Split(new Char[] { ';' });
             string connectionString = "data source" + connexion[0] + ";" + connexion[1] + ";" + connexion[2] + ";" + connexion[3];
             */

            ConnectionStringSettingsCollection lesChainesDeConnexion = ConfigurationManager.ConnectionStrings;
            ConnectionStringSettings laChaineDeConnexion = lesChainesDeConnexion["ABO07Entities"];

            string[] caract = laChaineDeConnexion.ConnectionString.Split(new string[] { "data source" }, StringSplitOptions.RemoveEmptyEntries);
            string[] chaine = caract[1].Split(new string[] { "MultipleActiveResultSetsnew" }, StringSplitOptions.RemoveEmptyEntries);
            string[] connexion = chaine[0].Split(new Char[] { ';' });
            string[] uid = connexion[2].Split(new string[] { "user id=" }, StringSplitOptions.RemoveEmptyEntries);
            string[] pwd = connexion[3].Split(new string[] { "password=" }, StringSplitOptions.RemoveEmptyEntries);

            string userId = string.Empty;
            string password = string.Empty;

            if (!string.IsNullOrEmpty(uid[0]))
                userId = INOVA.GALATEE.SECURITY.Crypteur.DecrypterText(uid[0]);

            if (!string.IsNullOrEmpty(pwd[0]))
                password = INOVA.GALATEE.SECURITY.Crypteur.DecrypterText(pwd[0]);


            string connectionString = "data source" + connexion[0] + ";" + connexion[1] + ";user id=" + userId + ";password=" + password;

            return connectionString;

        }



        private static bool EstSqlConnexionStringValide(string connectionString, ref string Erreur)
        {
            //- Test de la connexion
            try
            {
                SqlConnection Connexion = new SqlConnection(connectionString);
                //- Tentative d'ouverture de la connexion
                Connexion.Open();
                //- Message réussite
                //- Test ok. Fermeture de la connexion
                Connexion.Close();

                return true;
            }
            catch (Exception ex)
            {
                Erreur = ex.Message;
                return false;
            }
        }

        //    public Database GetDatabase()
        //public static  SqlDatabase GetDatabase()
        //{
        //    return GetDatabase(null);

        //}

        /// <summary>
        /// Retourne un objet <see cref="Database"/> dépendant de la connexion
        /// </summary>
        //public Database GetDatabase(Fraude.DataAcess.TransactionManager transactionManager)
        //public static SqlDatabase GetDatabase(Galatee.DataAccess.TransactionManager transactionManager)
        //{
        //    //Database database = null;
        //    SqlDatabase database = null;
        //    //new SqlDatabase(GalateeConfig.ConnectionStrings[_ConnectionStrings_Name].ConnectionString);
        //    if (transactionManager != null)
        //    {
        //        database = transactionManager.Database;

        //    }
        //    else if (!string.IsNullOrEmpty(GetSqlConnexionString()))
        //    {
        //        //DatabaseProviderFactory factory = db.AddInParameter(;// new DatabaseProviderFactory(ConfigurationSourceFactory.Create());

        //        //database =  factory.Create(ConnectionStrings_Name);
        //        database = new SqlDatabase(Session.GetSqlConnexionString());
        //    }
        //    else
        //    {
        //        database = new SqlDatabase(Session.GetSqlConnexionString());

        //        //database = new SqlDatabase(GalateeConfig.ConnectionStrings[GetSqlConnexionString()].ConnectionString); ;
        //    }

        //    return database;

        //}

    }

    //public static class Session
    //{
    //    private static List<SqlConnection> _ConnectionCollection;

    //    public static List<SqlConnection> ConnectionCollection
    //    {
    //        get
    //        {

    //            if (_ConnectionCollection == null)
    //            {
    //                _ConnectionCollection = new List<SqlConnection>();
    //                DBSITE db = new DBSITE();
    //                SqlConnection sqlCnct = null;
    //                string conncetionstring = string.Empty;
    //                SITECollection lstSite = db.GetAll();

    //                foreach (SITE site in lstSite)
    //                {
    //                    conncetionstring = GetSqlConnexionString(site);
    //                    //DBLClient dbc = new DBLClient();
    //                    sqlCnct = new SqlConnection(conncetionstring);
    //                    _ConnectionCollection.Add(sqlCnct);
    //                }
    //                if (_ConnectionCollection.Count == 0)
    //                {
    //                    _ConnectionCollection = null;
    //                    throw new Exception("Aucune connection GALATEE trouvée");
    //                }
    //            }

    //            return _ConnectionCollection;
    //        }
    //    }

    //    public static string GetSqlConnexionString(SITE site)
    //    {
    //        SqlConnectionStringBuilder StrBuild = new SqlConnectionStringBuilder();
    //        string Message = string.Empty;
    //        //- Récupération des informations de connexion
    //        StrBuild.DataSource = site.SERVEUR;

    //        //this.myGalaDBConnection.InitialCatalog = "GALADB";
    //        StrBuild.InitialCatalog = site.CATALOGUE;  //ABO00

    //        StrBuild.IntegratedSecurity = false;

    //        StrBuild.PersistSecurityInfo = true;

    //        if (!StrBuild.IntegratedSecurity)
    //        {
    //            StrBuild.UserID = site.USERID;
    //            StrBuild.Password = site.PWD;
    //        }
    //        if (!IsConnexionValide(StrBuild, ref Message))
    //            throw new Exception("Enregistrement annulé :" + Environment.NewLine + Message);
    //        return StrBuild.ConnectionString;
    //    }

    //    private static bool IsConnexionValide(SqlConnectionStringBuilder connString, ref string Erreur)
    //    {
    //        Erreur = string.Empty;

    //        //- Test des informations de connexion
    //        if (string.IsNullOrEmpty(connString.DataSource))
    //        {
    //            Erreur = "Serveur sql non renseigné";
    //            return false;
    //        }
    //        if (string.IsNullOrEmpty(connString.InitialCatalog))
    //        {
    //            Erreur = "Base de données non renseignée";
    //            return false;
    //        }

    //        //GetSqlConnexionStringBuilder();

    //        return EstSqlConnexionStringValide(connString.ConnectionString, ref Erreur);
    //    }

    //    private static bool EstSqlConnexionStringValide(string connectionString, ref string Erreur)
    //    {
    //        //- Test de la connexion
    //        try
    //        {
    //            SqlConnection Connexion = new SqlConnection(connectionString);
    //            //- Tentative d'ouverture de la connexion
    //            Connexion.Open();
    //            //- Message réussite
    //            //- Test ok. Fermeture de la connexion
    //            Connexion.Close();

    //            return true;
    //        }
    //        catch (Exception ex)
    //        {
    //            Erreur = ex.Message;
    //            return false;
    //        }
    //    }

    //    private static SqlConnection _sqlConnection = null;
    //    public static SqlConnection sqlConnection
    //    {
    //        get
    //        {

    //            if (_sqlConnection == null)
    //            {
    //                ConnectionStringSettingsCollection Cstr = null;
    //                string connectionStringName = "GALADBConnectionString";
    //                try
    //                {
    //                    Cstr = GalateeConfig.ConnectionStrings;
    //                    if (Cstr != null)
    //                    {
    //                        ConnectionStringSettings Cs = Cstr.Cast<ConnectionStringSettings>().FirstOrDefault(c => c.Name == connectionStringName);
    //                        if (Cs == null)
    //                        {
    //                            throw new Exception("Chaîne de connexion non configurée");
    //                        }
    //                        _sqlConnection = new SqlConnection(Cs.ConnectionString);
    //                        return _sqlConnection;
    //                    }
    //                    else throw new Exception("Chaîne de connexion non configurée");
    //                }
    //                catch (Exception ex)
    //                {
    //                    throw ex;
    //                }
    //            }
    //            else return _sqlConnection;
    //        }
    //    }

    //    public static SqlConnection Connection
    //    {
    //        get
    //        {
    //            if (sqlConnection.State != System.Data.ConnectionState.Open)
    //                sqlConnection.Open();
    //            return sqlConnection;
    //        }
    //    }

    //    private static SqlConnection _sqlConnection2 = null;
    //    public static SqlConnection sqlConnection2
    //    {
    //        get
    //        {

    //            if (_sqlConnection2 == null)
    //            {
    //                ConnectionStringSettingsCollection Cstr = null;
    //                string connectionStringName = "GALADB2ConnectionString";
    //                try
    //                {
    //                    Cstr = GalateeConfig.ConnectionStrings;
    //                    if (Cstr != null)
    //                    {
    //                        ConnectionStringSettings Cs = Cstr.Cast<ConnectionStringSettings>().FirstOrDefault(c => c.Name == connectionStringName);
    //                        if (Cs == null)
    //                        {
    //                            throw new Exception("Chaîne de connexion non configurée");
    //                        }
    //                        _sqlConnection2 = new SqlConnection(Cs.ConnectionString);
    //                        return _sqlConnection2;
    //                    }
    //                    else throw new Exception("Chaîne de connexion non configurée");
    //                }
    //                catch (Exception ex)
    //                {
    //                    throw ex;
    //                }
    //            }
    //            else return _sqlConnection2;
    //        }
    //    }

    //    public static SqlConnection Connection2
    //    {
    //        get
    //        {
    //            if (sqlConnection2.State != System.Data.ConnectionState.Open)
    //                sqlConnection2.Open();
    //            return sqlConnection2;
    //        }
    //    }
    //}
}
