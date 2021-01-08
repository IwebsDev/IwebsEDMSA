using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galatee.Entity.Model
{
    public static class ImportFichierProcedure
    {


        public static int MAJImportFichier(Galatee.Structure.aImportFichier leParametrage)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    PARAMETRAGEIMPORT import = context.PARAMETRAGEIMPORT.FirstOrDefault(c => c.CODE == leParametrage.CODE);

                    if (import != null)
                    {
                        PARAMETRAGEIMPORT _limport = Entities.ConvertObject<PARAMETRAGEIMPORT, Galatee.Structure.aImportFichier>(leParametrage);
                         Entities.UpdateEntity<PARAMETRAGEIMPORT>(_limport);

                         return _limport.CODE;
                    }
                    else
                    {
                        PARAMETRAGEIMPORT _limport = Entities.ConvertObject<PARAMETRAGEIMPORT, Galatee.Structure.aImportFichier>(leParametrage);
                        Entities.InsertEntity<PARAMETRAGEIMPORT>(_limport);

                        return _limport.CODE;
                    }

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static bool DeleteImportFichier(int CodeImport)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    PARAMETRAGEIMPORT import = context.PARAMETRAGEIMPORT.FirstOrDefault(c => c.CODE == CodeImport);
                    if (import != null)
                    {
                        List<IMPORTCOLONNE> lstcolon = context.IMPORTCOLONNE.Where(c => c.ID_PARAMETRAGE == CodeImport).ToList();

                        foreach (IMPORTCOLONNE colon in lstcolon)
                        {
                            context.IMPORTCOLONNE.Remove(colon);
                            context.SaveChanges();
                        }


                        context.PARAMETRAGEIMPORT.Remove(import);
                        context.SaveChanges();
                        return true;
                    }
                    else
                        return false;
                };
            }
            catch (Exception ex)
            {
                return false;
                throw ex;
            }
        }
        public static PARAMETRAGEIMPORT RetourneImportFichier(int CodeImport)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    return context.PARAMETRAGEIMPORT.FirstOrDefault(c => c.CODE == CodeImport);
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static List<PARAMETRAGEIMPORT> RetourneAllImportFichier()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    return context.PARAMETRAGEIMPORT.ToList();
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool MAJImportColonne(Galatee.Structure.aImportFichierColonne lacolonne)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IMPORTCOLONNE colon = context.IMPORTCOLONNE.FirstOrDefault(c => c.ID_COLONNE == lacolonne.ID_COLONNE);

                    if (colon != null)
                    {
                        IMPORTCOLONNE _lacolonne = Entities.ConvertObject<IMPORTCOLONNE, Galatee.Structure.aImportFichierColonne>(lacolonne);
                        return Entities.UpdateEntity<IMPORTCOLONNE>(_lacolonne);
                    }
                    else
                    {
                        IMPORTCOLONNE _lacolonne = Entities.ConvertObject<IMPORTCOLONNE, Galatee.Structure.aImportFichierColonne>(lacolonne);
                        return Entities.InsertEntity<IMPORTCOLONNE>(_lacolonne);
                    }

                }; ;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static bool DeleteImportFichierColonne(int CodeColonne)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IMPORTCOLONNE colonne = context.IMPORTCOLONNE.FirstOrDefault(c => c.ID_COLONNE == CodeColonne);
                    if (colonne != null)
                    {
                        context.IMPORTCOLONNE.Remove(colonne);
                        context.SaveChanges();
                        return true;
                    }
                    else
                        return false;
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static IMPORTCOLONNE RetourneImportFichierColonne(int CodeColonne)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    return context.IMPORTCOLONNE.FirstOrDefault(c => c.ID_COLONNE == CodeColonne);
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static List<IMPORTCOLONNE> RetourneAllImportFichierColonne(int CodeImport)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    return context.IMPORTCOLONNE.Where(c => c.ID_PARAMETRAGE == CodeImport).ToList();
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public static string ExcecuteProcedure(string Agent, string COMMANDE, int? codeimport, int? NBPARAMETRE, bool isprocedure)
        {
            string laliste = null;
            
                ConnectionStringSettingsCollection lesChainesDeConnexion = ConfigurationManager.ConnectionStrings;
                //ConnectionStringSettings laChaineDeConnexion = lesChainesDeConnexion["galadbEntities"];
                //string[] caract = laChaineDeConnexion.ConnectionString.Split(new string[] { "data source" }, StringSplitOptions.RemoveEmptyEntries);
                //string[] chaine = caract[1].Split(new string[] { "MultipleActiveResultSetsnew" }, StringSplitOptions.RemoveEmptyEntries);
                //string[] connexion = chaine[0].Split(new Char[] { ';' });
                //string connectionString = "data source" + connexion[0] + ";" + connexion[1] + ";" + connexion[2] + ";" + connexion[3];

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
                int result = 0;

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand();
                    DataTable dt = new DataTable();

                    List<IMPORTCOLONNE> lescolonnes = null;
                    using (galadbEntities context = new galadbEntities())
                    {
                        lescolonnes = context.IMPORTCOLONNE.Where(c => c.ID_PARAMETRAGE == codeimport).ToList();
                    };

                    if (lescolonnes != null && lescolonnes.Count > 0)
                    {
                        if (isprocedure == true)
                        {
                        SqlParameter[] parametres = new SqlParameter[int.Parse(NBPARAMETRE.ToString())];

                        string[] tabOper = Agent.Split(new char[] { ';' });
                        int a = 0;

                        foreach (IMPORTCOLONNE col in lescolonnes)
                        {
                            parametres[a] = new SqlParameter("@" + col.NOM, col.TYPE);
                            parametres[a].Value = tabOper[a];
                            a++;
                        }


                        foreach (SqlParameter param in parametres)
                        {
                            cmd.Parameters.Add(param);
                        }

                        
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Connection = connection;
                            cmd.CommandText = COMMANDE;
                        }
                        else
                        {

                            cmd.CommandText = COMMANDE;
                            int a = 0;
                            string[] tabOper = Agent.Split(new char[] { ';' });
                            foreach (IMPORTCOLONNE col in lescolonnes)
                            {
                                cmd.Parameters.AddWithValue("@" + col.NOM, tabOper[a]);
                               a++;
                            }
                        }

                        //result = cmd.ExecuteNonQuery();
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        result = da.Fill(dt);
                        cmd.Dispose();
                        if (result > 0)
                        {
                            string resultat= string.Empty;
                            foreach (DataRow lign in dt.Rows)
                        {
                            resultat= Convert.ToString( lign[0]);
                        }
                            if (resultat=="0")
                            {
                                laliste = string.Empty;
                            }
                            else
                            {
                                string[] valeur = Agent.Split(new Char[] { ';' });
                                string elmt = null;
                                foreach (string lign in valeur)
                                {
                                    if (elmt == null)
                                        elmt += lign != null ? lign : string.Empty;
                                    else
                                        elmt += lign != null ? "       " + lign : "       " + string.Empty;


                                    laliste = elmt;
                                }
                            }
                        }
                    }

                }

            return laliste;
        }
    }
}
