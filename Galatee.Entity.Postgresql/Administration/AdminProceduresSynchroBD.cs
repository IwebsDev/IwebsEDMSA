using Galatee.Structure;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galatee.Entity.Model
{
    public class AdminProceduresSynchroBD
    {
        public static DataTable LoadAgentBaseDistantez(string Requette, string Provider, string DataSource, string IniialeCatalog, string UserId, string Password)
        {
            try
            {
                DataTable dataTable = new DataTable();

                string ConnectionString = "Server=" + DataSource + ";Database=" + IniialeCatalog + ";User Id=" + UserId + ";Password=" + Password;

                switch (Provider)
                {
                    case "SqlClient Data Provider":
                        GetDataFromSqlServer(Requette, dataTable,ConnectionString);
                        break;
                    case "OracleClient Data Provider":
                        GetDataFromOracle(Requette, dataTable, ConnectionString);
                        break;
                    default:
                        Console.WriteLine("Default case");
                        break;
                }




                

                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool TestBaseDistante(string Requette, string Provider, string DataSource, string IniialeCatalog, string UserId, string Password)
        {
            try
            {
                bool resul = false;
                DataTable dataTable=new DataTable();
                string ConnectionString = "Server=" + DataSource + ";Database=" + IniialeCatalog + ";User Id=" + UserId + ";Password=" + Password;

                switch (Provider)
                {
                    case "SqlClient Data Provider":
                        resul = TestFromSqlServer(Requette, dataTable, ConnectionString);
                        break;
                    case "OracleClient Data Provider":
                        resul = TestFromOracle(Requette, dataTable, ConnectionString);
                        break;
                    default:
                        Console.WriteLine("Default case");
                        break;
                }






                return resul;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private static void GetDataFromOracle(string Requette, DataTable dataTable, string ConnectionString)
        {
            ////Initialisation de connection
            //OracleConnection conn = new OracleConnection(ConnectionString);
            //OracleCommand cmd = new OracleCommand(Requette, conn);
            //conn.Open();

            //// Creation de l'adatper de données
            //OracleDataAdapter da = new OracleDataAdapter(cmd);
            //// Chargement de la datatable
            //da.Fill(dataTable);
            //conn.Close();
            //da.Dispose();
        }

        private static bool TestFromOracle(string Requette, DataTable dataTable, string ConnectionString)
        {
            ////Initialisation de connection
            //OracleConnection conn = new OracleConnection(ConnectionString);
            ////OracleCommand cmd = new OracleCommand(Requette, conn);
            //conn.Open();

            ////// Creation de l'adatper de données
            ////OracleDataAdapter da = new OracleDataAdapter(cmd);
            ////// Chargement de la datatable
            ////da.Fill(dataTable);
            ////conn.Close();
            ////da.Dispose();
            return true;
        }

        private static void GetDataFromSqlServer(string Requette, DataTable dataTable, string ConnectionString)
        {
            //Initialisation de connection
            SqlConnection conn = new SqlConnection(ConnectionString);
            SqlCommand cmd = new SqlCommand(Requette, conn);
            conn.Open();

            // Creation de l'adatper de données
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            // Chargement de la datatable
            da.Fill(dataTable);
            conn.Close();
            da.Dispose();
        }

        private static bool TestFromSqlServer(string Requette, DataTable dataTable, string ConnectionString)
        {

            //Initialisation de connection
            SqlConnection conn = new SqlConnection(ConnectionString);
            //SqlCommand cmd = new SqlCommand(Requette, conn);
            conn.Open();
            return true;
            //// Creation de l'adatper de données
            //SqlDataAdapter da = new SqlDataAdapter(cmd);
            //// Chargement de la datatable
            //da.Fill(dataTable);
            //conn.Close();
            //da.Dispose();
        }

        public static string LoadRequest()
        {
            using (galadbEntities Context = new galadbEntities())
            {
                return Context.PARAMETRESGENERAUX.FirstOrDefault(p => p.CODE == "000406").LIBELLE;
            }
        }

        public static bool SaveAgent(List<Structure.CsAgent> agenttosynchro, string Requette)
        {
            using (galadbEntities Context=new galadbEntities())
            {
                
                PARAMETRESGENERAUX PARAMETRESGENERAUX = Context.PARAMETRESGENERAUX.FirstOrDefault(p => p.CODE == "000406");
                if (PARAMETRESGENERAUX!=null)
                {
                    PARAMETRESGENERAUX.LIBELLE = Requette;

                    List<CsAgent> NouveauAgent = agenttosynchro.Where(a => a.ACTIF == true)!=null?agenttosynchro.Where(a => a.ACTIF == true).ToList():new List<CsAgent>();
                    List<AGENT> AgentToInsert = ConvertAgent(NouveauAgent);

                    List<string> MatriculeAgent_a_Desactiver=agenttosynchro.Where(a=>a.ACTIF==false).Select(a=>a.MATRICULE).ToList();
                    List<ADMUTILISATEUR> UTILISATEUR = Context.ADMUTILISATEUR.Where(u=>MatriculeAgent_a_Desactiver.Contains(u.MATRICULE))!=null?Context.ADMUTILISATEUR.Where(u=>MatriculeAgent_a_Desactiver.Contains(u.MATRICULE)).ToList():new List<ADMUTILISATEUR>();

                    foreach (var item in UTILISATEUR)
                    {
                        item.STATUSCOMPTE = Enumere.StatusSupprimer;
                    }
                    Entities.UpdateEntity<ADMUTILISATEUR>(UTILISATEUR, Context);
                    Entities.UpdateEntity<PARAMETRESGENERAUX>(PARAMETRESGENERAUX, Context);
                    bool result = Entities.InsertEntity<AGENT>(AgentToInsert, Context);

                    Context.SaveChanges();
                    return result;
                }
                else
                {
                    return false;
                }
               
            }
        }
        private static List<AGENT> ConvertAgent(List<Structure.CsAgent> agenttosynchro)
        {
            List<AGENT> AgentToInsert = new List<AGENT>();
            foreach (var item in agenttosynchro)
            {
                AGENT agent = new AGENT();
                agent.COMPTEWINDOWS = item.COMPTEWINDOWS;
                agent.MATRICULE = item.MATRICULE;
                agent.NOM = item.NOM;
                agent.PRENOM = item.PRENOM;

                AgentToInsert.Add(agent);
            }
            return AgentToInsert;
        }


        public static List<string> GetProviderList()
        {
            try
            {
                    DataTable provider = System.Data.Common.DbProviderFactories.GetFactoryClasses();
                    string[] ProviderList = new string[provider.Rows.Count + 1];
                    ProviderList[0] = string.Empty;
                    if (provider != null)
                    {
                        for (int i = 1; i < provider.Rows.Count + 1; i++)
                        {
                            ProviderList[i] = provider.Rows[i - 1][0].ToString();
                        }
                    }
                    return ProviderList.ToList();
            }
            catch (Exception)
            {
                
                throw;
            }
        }
        public static List<string> GetSQLDatabaseList(string serverInstanceName, bool useWindowsAuthentication, string username, string password)
        {
            try
            {
                SqlConnectionStringBuilder csb = new SqlConnectionStringBuilder();
                csb.DataSource = serverInstanceName;
                //cmbServers.Text 'cboSrcDB.Text 
                csb.IntegratedSecurity = useWindowsAuthentication;
                //csb.TrustServerCertificate = useWindowsAuthentication 
                csb.InitialCatalog = "master";
                if (!useWindowsAuthentication)
                {
                    csb.UserID = username;
                    csb.Password = password;
                }


                SqlConnection conn = new SqlConnection(csb.ToString());


                // The 'where name like 'f%_' will filter out just those databases begining with "F" or "f" 
                SqlDataAdapter da = new SqlDataAdapter("Select name from sysdatabases ", conn);
                //removed the following from above query so that all databases would be shown 
                // where name like 'f%_' 
                try
                {
                    DataTable dt = new DataTable("Databases");
                    int rowsAffected = da.Fill(dt);
                    if (dt == null || rowsAffected <= 0)
                    {
                        return null;
                    }

                    int f = -1;
                    string[] databases = new string[dt.Rows.Count];
                    foreach (DataRow r in dt.Rows)
                    {
                        databases[System.Math.Max(System.Threading.Interlocked.Increment(ref f), f - 1)] = r["name"].ToString();
                    }
                    da.Dispose();
                    Array.Sort(databases);
                    return databases.ToList();
                }
                catch (SqlException ex)
                {
                    return null;
                }
                finally
                {
                    conn.Close();
                }
            }
            catch (Exception)
            {
                
                throw;
            }
        }
}
}
