using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Galatee.Structure;
using Galatee.Entity.Model;
using System.IO;
//using Excel;
using System.Data;



namespace Galatee.DataAccess
{
    public class DbImportFichier
    {
        public aImportFichier RetourneImportFichier(int CodeImport)
        {
            try
            {
                PARAMETRAGEIMPORT dts = Galatee.Entity.Model.ImportFichierProcedure.RetourneImportFichier(CodeImport);

                aImportFichier limport = new aImportFichier()
                {
                    CODE = dts.CODE,
                    LIBELLE = dts.LIBELLE,
                    DESCRIPTION = dts.DESCRIPTION,
                    COMMANDE = dts.COMMANDE,
                    REPERTOIRE = dts.REPERTOIRE,
                    FICHIER = dts.FICHIER,
                    NBPARAMETRE = dts.NBPARAMETRE,
                    ISPROCEDURE= bool.Parse(dts.ISPROCEDURE.ToString()),
                    BASEDEDONNE=dts.BASEDEDONNE,
                    MOTDEPASSE=dts.MOTDEPASSE,
                    PROVIDER=dts.PROVIDER,
                    SERVER = dts.SERVER,
                    UTILISATEUR=dts.UTILISATEUR,
                    REQUTETTEBASEDISTANTE=dts.REQUTETTEBASEDISTANTE
                };

                return limport;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int MAJImportFichier(aImportFichier ImportFichier)
        {
            try
            {
                return ImportFichierProcedure.MAJImportFichier(ImportFichier);
            }
            catch (Exception ex)
            {
                return 0;
                throw ex;
            }

        }
        public bool DeleteImport(int Codeimport)
        {
            try
            {
                return Galatee.Entity.Model.ImportFichierProcedure.DeleteImportFichier(Codeimport); ;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<string > ImporterFichier(int code)
        {

            aImportFichier parametre = RetourneImportFichier(code);
            List<string> result = null;
            if (!string.IsNullOrEmpty(parametre.COMMANDE) && !string.IsNullOrEmpty(parametre.REPERTOIRE) && !string.IsNullOrEmpty(parametre.FICHIER))
            {
                string path = @"" + parametre.REPERTOIRE + @"\" + parametre.FICHIER; //Chemin du fichier

                //Lecture du fichier text

                if (File.Exists(path))
                {
                    result = lectureFichier(path, parametre);
                }

            }
            return result;

        }

        public List<string> lectureFichier(string fichier, aImportFichier nbparam)
        {
            try
            {
                List<string> listeagentImport = new List<string>();
                List<string> listeagentModif = new List<string>();
                List<string> ListAgent = new List<string>();

                List<string> LesAgent = new List<string>();
                CsAgent _Agent = null;

                string Extension = Path.GetExtension(nbparam.FICHIER);

                switch (Extension.ToUpper())
                {
                    case ".CSV":
                        ListAgent = GetCSV(fichier, nbparam);
                        break;
                    case ".TXT":
                        ListAgent = GetCSV(fichier, nbparam);
                        break;
                    case ".XLS":
                        ListAgent = GetExcel(fichier, ".XLS", nbparam);
                        break;
                    case ".XLSX":
                        ListAgent = GetExcel(fichier, ".XLSX", nbparam);
                        break;

                    default:
                        return null;
                }

                try
                {
                 return  InsertDonneeImport(nbparam, listeagentImport, listeagentModif, ListAgent);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            catch (Exception ex)
            {
                throw ex;
                // Code exécuté en cas d'exception 
                //Response.Write("Une erreur est survenue au cours de la lecture !");
                //Response.Write("</br>");
                //Response.Write(ex.Message);
            }
        }

        public  List<string > InsertDonneeImport(aImportFichier nbparam, List<string> listeagentImport, List<string> listeagentModif, List<string> ListAgent)
        {
            int nbechec = 0;
            List<string> RetoureTraitement = new List<string>();
            if (ListAgent.Count > 0)
            {

                string lagent = null;
                foreach (string agent in ListAgent)
                {
                    lagent = null;

                    try
                    {
                        if (agent != null)
                            lagent = ImportFichierProcedure.ExcecuteProcedure(agent, nbparam.COMMANDE, nbparam.CODE, nbparam.NBPARAMETRE, nbparam.ISPROCEDURE);

                        if (lagent != null)
                        {
                            if (lagent == string.Empty)
                                listeagentModif.Add(lagent);
                            else
                                listeagentImport.Add(lagent);
                            
                        }

                    }
                    catch(Exception ex)
                    {
                        Galatee.Tools.Utility.EcrireFichier(nbparam.LIBELLE + ":  Echec sur " + agent, new DB_ParametresGeneraux().SelectParametresGenerauxByCode("000406").LIBELLE + @"\Log_Import_Du_" + DateTime.Now.Day + "-" + DateTime.Now.Month + "-" + DateTime.Now.Year + ".txt");
                        nbechec++;
                        return null ;
                    }
                }

            }
            RetoureTraitement.Add("Nombre total mis a jours = " + listeagentImport.Count);
            RetoureTraitement.Add("Nombre total modification= " + listeagentModif.Count);
            RetoureTraitement.Add("Nombre total echec =" + nbechec);
            //Galatee.Tools.Utility.EcrireFichier(nbparam.LIBELLE + ": Ajout = " + listeagentImport.Count + " ;Modification= " + listeagentModif.Count + "; Echec =" + nbechec, new DB_ParametresGeneraux().SelectParametresGenerauxByCode("000406").LIBELLE + @"\Log_Import_Du_" + DateTime.Now.Day + "-" + DateTime.Now.Month + "-" + DateTime.Now.Year + ".txt");
            return RetoureTraitement;
        }

        private List<String> GetCSV(string fichier, aImportFichier nbparam)
        {
            int index = 0;

            // Création d'une instance de StreamReader pour permettre la lecture de notre fichier 
            StreamReader monStreamReader = new StreamReader(fichier, System.Text.Encoding.Default);
            string ligne = string.Empty;
            string ListAgent ;

            List<string> LesAgent = new List<string>();
            // Lecture de toutes les lignes et affichage de chacune sur la page 
            //while (ligne != null)
            //{
                ligne = monStreamReader.ReadToEnd();
                // if (!string.IsNullOrEmpty(ligne.Trim()))                    
                ListAgent= ligne;
                //  index++; 

            //}
                ListAgent.Trim(new Char[] { '\r', '\n'});
            // Pour chaque ligne , creer un objet Pls_Ref_Operateur

            int i = 0;
            string element = null;
            //foreach (string item in ListAgent)
            //{

                string[] tabOper = ListAgent.Split(new char[] { '|' });

                foreach (string elmt in tabOper)
                {
                    //if (element == null)
                    //    element += tabOper[i] != null ? tabOper[i] : string.Empty;
                    //else
                    //    element += tabOper[i] != null ? ";" + tabOper[i] : ";" + string.Empty;

                    //i++;
                    //if (i == nbparam.NBPARAMETRE)
                    //{
                    LesAgent.Add(elmt);
                       
                    //}
                }
            //}
            //index++;

            // Fermeture du StreamReader (attention très important) 
            monStreamReader.Close();
            // monStreamReader.Dispose();

            return LesAgent;

        }

        private List<string> GetExcel(string fichier, string extension, aImportFichier nbparam)
        {
            int index = 0;

            // Création d'une instance de StreamReader pour permettre la lecture de notre fichier 
            FileStream stream = File.Open(fichier, FileMode.Open, FileAccess.Read);

            //IExcelDataReader excelReader = null;
            //if (extension == ".XLS")
            //{//for excel 2003
            //    excelReader = ExcelReaderFactory.CreateBinaryReader(stream);
            //}
            //else
            //{
            //    if (extension == ".XLSX")
            //    {// for Excel 2007
            //        excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
            //    }
            //}

            //excelReader.IsFirstRowAsColumnNames = true;
            //System.Data.DataTable result = excelReader.AsDataSet().Tables[0];

            List<string> LesAgent = new List<string>();
            //foreach (DataRow row in result.Rows)
            //{
            //    string elmt = null;
            //    for (int i = 0; i < nbparam.NBPARAMETRE; i++)
            //    {
            //        if (!string.IsNullOrEmpty(Convert.ToString(row[0])))
            //        {
            //            if (elmt == null)
            //                elmt += Convert.ToString(row[i]) != null ? Convert.ToString(row[i]) : string.Empty;
            //            else
            //                elmt += Convert.ToString(row[i]) != null ? ";" + Convert.ToString(row[i]) : ";" + string.Empty;
            //        }
            //    }

            //    LesAgent.Add(elmt);

            //}

            return LesAgent;

        }


        public List<aImportFichier> RetourneAllImportFichier()
        {
            try
            {
                List<aImportFichier> result = new List<aImportFichier>();
                List<PARAMETRAGEIMPORT> lstdts = Galatee.Entity.Model.ImportFichierProcedure.RetourneAllImportFichier();

                foreach (PARAMETRAGEIMPORT dts in lstdts)
                {
                    aImportFichier limport = new aImportFichier()
                    {
                        CODE = dts.CODE,
                        LIBELLE = dts.LIBELLE,
                        DESCRIPTION = dts.DESCRIPTION,
                        COMMANDE = dts.COMMANDE,
                        REPERTOIRE = dts.REPERTOIRE,
                        FICHIER = dts.FICHIER,
                        NBPARAMETRE = dts.NBPARAMETRE,
                        BASEDEDONNE=dts.BASEDEDONNE,
                        MOTDEPASSE=dts.MOTDEPASSE,
                        PROVIDER=dts.PROVIDER,
                        REQUTETTEBASEDISTANTE=dts.REQUTETTEBASEDISTANTE,
                        SERVER=dts.SERVER,
                        ISPROCEDURE=dts.ISPROCEDURE.Value,
                        UTILISATEUR=dts.UTILISATEUR
                    };

                    result.Add(limport);
                }

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public aImportFichierColonne RetourneImportFichierColonne(int CodeColonne)
        {
            try
            {
                IMPORTCOLONNE dts = Galatee.Entity.Model.ImportFichierProcedure.RetourneImportFichierColonne(CodeColonne);

                aImportFichierColonne lacolonne = new aImportFichierColonne()
                {
                    ID_COLONNE = dts.ID_COLONNE,
                    NOM = dts.NOM,
                    TYPE = dts.TYPE,
                    LONGUEUR = dts.LONGUEUR,
                    DESCRIPTION = dts.DESCRIPTION,
                    ID_PARAMETRAGE = dts.ID_PARAMETRAGE
                };

                return lacolonne;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool MAJImportFichierColonne(aImportFichierColonne Importcolonne)
        {
            try
            {
                ImportFichierProcedure.MAJImportColonne(Importcolonne);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return true;
        }

        public bool DeleteImportColonne(int CodeColonne)
        {
            try
            {
                return Galatee.Entity.Model.ImportFichierProcedure.DeleteImportFichierColonne(CodeColonne); ;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public List<aImportFichierColonne> RetourneAllImportColonne(int CodeImport)
        {
            try
            {
                List<aImportFichierColonne> lescolonnes = new List<aImportFichierColonne>();
                List<IMPORTCOLONNE> lstdts = Galatee.Entity.Model.ImportFichierProcedure.RetourneAllImportFichierColonne(CodeImport);

                foreach (IMPORTCOLONNE dts in lstdts)
                {
                    aImportFichierColonne lacolonne = new aImportFichierColonne()
                    {
                        ID_COLONNE = dts.ID_COLONNE,
                        NOM = dts.NOM,
                        TYPE = dts.TYPE,
                        LONGUEUR = dts.LONGUEUR,
                        DESCRIPTION = dts.DESCRIPTION,
                        ID_PARAMETRAGE = dts.ID_PARAMETRAGE
                    };
                    lescolonnes.Add(lacolonne);
                }

                return lescolonnes;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
