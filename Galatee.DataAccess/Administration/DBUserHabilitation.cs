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
     public class DBUserHabilitation : Galatee.DataAccess.Parametrage.DbBase
    {
         public DBUserHabilitation()
        {
        //    ConnectionString = Session.GetSqlConnexionString();
            
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
        public bool Update(CsAdmRoles admRoles)
        {
            cn = new SqlConnection(ConnectionString);
            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "spx_AdmRoles_Update";
            cmd.Parameters.Clear();
            cmd.Parameters.Add("@RoleID", SqlDbType.VarChar).Value = admRoles.RoleID;
            cmd.Parameters.Add("@RoleName", SqlDbType.VarChar).Value = admRoles.RoleName;
            cmd.Parameters.Add("@RoleDisplayName", SqlDbType.VarChar).Value = admRoles.RoleDisplayName;

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

        public bool Insert(CsAdmRoles admRoles)
        {
            cn = new SqlConnection(ConnectionString);
            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "spx_AdmRoles_Insert";
            cmd.Parameters.Clear();
            cmd.Parameters.Add("@Centre", SqlDbType.VarChar).Value = admRoles.Centre;
            cmd.Parameters.Add("@RoleID", SqlDbType.VarChar).Value = admRoles.RoleID;
            cmd.Parameters.Add("@CodeFonc", SqlDbType.VarChar).Value = admRoles.CodeFonc;
            cmd.Parameters.Add("@RoleName", SqlDbType.VarChar).Value = admRoles.RoleName;
            cmd.Parameters.Add("@RoleDisplayName", SqlDbType.VarChar).Value = admRoles.RoleDisplayName;
            
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

        public bool Delete(Guid ID, string CodeFonc)
        {
            cn = new SqlConnection(ConnectionString);
            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "spx_AdmRoles_Delete";
            cmd.Parameters.Clear();
            cmd.Parameters.Add("@RoleID", SqlDbType.VarChar).Value = ID;
            cmd.Parameters.Add("@CodeFonc", SqlDbType.VarChar).Value = CodeFonc;


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

        public List<CsLibelle> SELECT_ALL_By_NUMTABLE(int NUM)
        {
            cn = new SqlConnection(ConnectionString);

            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SPX_TA_SELECT_ALL_By_NUM";
            cmd.Parameters.Add("@NUM", SqlDbType.SmallInt).Value = NUM;

            try
            {

                if (cn.State == ConnectionState.Closed)
                    cn.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                List<CsLibelle> rows = new List<CsLibelle>();
                FillTa(reader, rows, 0, int.MaxValue);

                return rows;
            }
            catch (Exception ex)
            {
                throw new Exception("SPX_TA_SELECT_ALL_By_NUM" + ":" + ex.Message);
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                    cn.Close(); // Fermeture de la connection 
                cmd.Dispose();
            }
        }

        public static List<CsLibelle> FillTa(SqlDataReader reader, List<CsLibelle> rows, int start, int pageLength)
        {
            for (int i = 0; i < start; i++)
            {
                if (!reader.Read())
                    return rows;
            }
            string LClient = string.Empty;
            for (int i = 0; i < pageLength; i++)
            {
                if (!reader.Read())
                    break;

                try
                {
                    CsLibelle c = new CsLibelle();
                    c.CODE = (Convert.IsDBNull(reader["CODE"])) ? String.Empty : (System.String)reader["CODE"];
                    c.LIBELLE = (Convert.IsDBNull(reader["LIBELLE"])) ? String.Empty : (System.String)reader["LIBELLE"];
                    rows.Add(c);
                }
                catch (Exception ex)
                {

                    throw ex;
                }
            }
            return rows;
        }

        public List<CSMenuGalatee> ChargerMenuEtSousMenuHabilite()
        {
            cn = new SqlConnection(ConnectionString);

            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandTimeout = 360;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SPX_ADM_LISTEMENU";
            try
            {
                if (cn.State == ConnectionState.Closed)
                    cn.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                List<CSMenuGalatee> rows = new List<CSMenuGalatee>();
                FillMenu(reader, rows, 0, int.MaxValue);
                reader.Close();
                return rows;
            }
            catch (Exception ex)
            {
                throw new Exception("SPX_ADM_LISTEMENU" + ":" + ex.Message);
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                    cn.Close(); // Fermeture de la connection 
                cmd.Dispose();
            }
        }

        public List<CSMenuGalatee> FillMenu(SqlDataReader reader, List<CSMenuGalatee> rows, int start, int pageLength)
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

                    CSMenuGalatee c = new CSMenuGalatee();
                    c.MenuID = (Convert.IsDBNull(reader["MenuID"])) ? 0 : (System.Int32)reader["MenuID"];
                    c.MenuText = (Convert.IsDBNull(reader["MenuText"])) ? String.Empty : (System.String)reader["MenuText"];
                    c.MainMenuID = (Convert.IsDBNull(reader["MainMenuID"])) ? 0 : (System.Int32)reader["MainMenuID"];
                    c.FormName = (Convert.IsDBNull(reader["FormName"])) ? String.Empty : (System.String)reader["FormName"];
                    c.Module = (Convert.IsDBNull(reader["Module"])) ? String.Empty : (System.String)reader["Module"];
                    if (Convert.IsDBNull(reader["TypeReedition"]))
                        c.TypeReedition = null;
                    else
                        c.TypeReedition =  (System.Byte)reader["TypeReedition"];
                    rows.Add(c);
                }
                return rows;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CsAdmMenu> MenusSelectByFonction(int metier)
        {
            try
            {
                DataTable dt = Galatee.Entity.Model.AdminProcedures.RetourneMenusParFonction(metier);
                return Galatee.Tools.Utility.GetEntityFromQuery<CsAdmMenu>(dt).ToList();
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CsModule> HabilitationSelectByMetierModule(string metier)
        {
            //cmd.CommandText = "spx_ADM_HabilitationProgram_SelectByFonction";
            try
            {
                DataTable dt = Galatee.Entity.Model.AdminProcedures.RetourneHabilitationProgramParModule(metier);
                return Galatee.Tools.Utility.GetEntityFromQuery<CsModule>(dt).ToList();
                // prog = new CsHabilitationProgram();
                // if (reader.GetValue(0).ToString().Trim() != string.Empty)
                //     prog.CodeFonction = reader.GetValue(0).ToString().Trim();
                //// prog.IdProgram = reader.GetValue(1);
                // //if (reader.GetValue(2).ToString().Trim() != string.Empty)
                // //    prog.IdGroupProgram = reader.GetValue(2).ToString().Trim();

                // if (reader.GetValue(3).ToString().Trim() != string.Empty)
                //     prog.IdMenu = reader.GetValue(3).ToString().Trim();
                // programmes.Add(prog);
            }
            //Fermeture reader
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CsHabilitationProgram> ProgramSelectAll()
        {
            CsHabilitationProgram prog;
            List<CsHabilitationProgram> programmes = new List<CsHabilitationProgram>();
            cn = new SqlConnection(ConnectionString);
            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandText = "spx_col_Program_Select_SelectAll";

            try
            {

                if (this.cn.State == ConnectionState.Closed)
                    this.cn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (!reader.HasRows)
                {
                    reader.Close();
                    this.cn.Close();
                    return null;
                }
                while (reader.Read())
                {
                    //prog = new CsHabilitationProgram();
                    //if (reader.GetValue(0).ToString().Trim() != string.Empty)
                    //    prog.Id = reader.GetValue(0).ToString().Trim();
                    ////if (reader.GetValue(1).ToString().Trim() != string.Empty)
                    ////    prog.IdGroupProgram = reader.GetValue(1).ToString().Trim();
                    //prog.Code = reader.GetValue(2).ToString().Trim();
                    //prog.Libelle = reader.GetValue(3).ToString().Trim();
                    //programmes.Add(prog);
                }
                //Fermeture reader
                reader.Close();

                return programmes;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (this.cn.State == ConnectionState.Open)
                    this.cn.Close(); // Fermeture de la connection 
                cmd.Dispose();
            }
        }

         /// <summary>
         /// Renvoie tous les modules , programmes et menus respectifs
         /// </summary>
         /// <returns></returns>

        public List<CsModule> RetourneAllModuleFonction()
        {
            
            try
            {

                DataTable dt = Galatee.Entity.Model.AdminProcedures.RetourneListeToutModuleFonction();
                return Entities.GetEntityListFromQuery<CsModule>(dt).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public List<CsHabilitationProgram> ProgramSelectById(int id)
        {
            CsHabilitationProgram line;

            List<CsHabilitationProgram> programmes = new List<CsHabilitationProgram>();
            cn = new SqlConnection(ConnectionString);
            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandText = "spx_Program_SelectById";

            cmd.Parameters.Add("@Id", SqlDbType.Int).Value = id;
            DBBase.SetDBNullParametre(cmd.Parameters);

            try
            {
                List<CsHabilitationProgram> rows = new List<CsHabilitationProgram>();
                if (this.cn.State == ConnectionState.Closed)
                    this.cn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (!reader.HasRows)
                {
                    reader.Close();

                    return null;
                }
                while (reader.Read())
                {
                    //line = new CsHabilitationProgram();
                    //if (reader.GetValue(0).ToString().Trim() != string.Empty)
                    //    line.Id = reader.GetValue(0).ToString().Trim();
                    ////if (reader.GetValue(1).ToString().Trim() != string.Empty)
                    //    //line.IdGroupProgram =reader.GetValue(1).ToString().Trim();
                    //line.Code = reader.GetValue(2).ToString().Trim();
                    //line.Libelle = reader.GetValue(3).ToString().Trim();
                    //rows.Add(line);
                }
                //Fermeture reader
                reader.Close();
                return rows;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (this.cn.State == ConnectionState.Open)
                    this.cn.Close(); // Fermeture de la connection 
                cmd.Dispose();
            }
        }

        public List<CsHabilitationProgram> ProgramSelectByAllMetier(List<CsHabilitationProgram> idListe)
        {
            List<CsHabilitationProgram> LstHablitation = new List<CsHabilitationProgram>();
            foreach (CsHabilitationProgram item in idListe)
              LstHablitation.AddRange(ProgramSelectById(item.Id.Value));
            return LstHablitation;
        }
        public List<CsAdmMenu> MenusSelectByProfil(int metier)
        {
            try
            {
                DataTable dt = Galatee.Entity.Model.AdminProcedures.RetourneMenusParProfil(metier);
                return Galatee.Tools.Utility.GetEntityFromQuery<CsAdmMenu>(dt).ToList();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CsProfil> ProfilParfonction(int idFonction)
        {
            DataTable dt = Galatee.Entity.Model.AdminProcedures.RetourneProfilByFonction(idFonction);

            return Galatee.Tools.Utility.GetEntityFromQuery<CsProfil>(dt).ToList();
        }
        public List<CsHabilitationCentreProfil> ProfilCentre(int id_profil)
        {
            DataTable dt = Galatee.Entity.Model.AdminProcedures.RetourneCentreDuProfil(id_profil);

            return Galatee.Tools.Utility.GetEntityFromQuery<CsHabilitationCentreProfil>(dt).ToList();
        }
        public List<CsFonction> RetourneFonctionByCode(string codefonction)
        {
            DataTable dt = Galatee.Entity.Model.AdminProcedures.RetourneFonctionByCode(codefonction);

            return Galatee.Tools.Utility.GetEntityFromQuery<CsFonction>(dt).ToList();
        }

        public List<CSMenuGalatee> RetourneMenusParProfil(int idprofil)
        {
            DataTable dt = Galatee.Entity.Model.AdminProcedures.RetourneMenusParProfil(idprofil);

            return Galatee.Tools.Utility.GetEntityFromQuery<CSMenuGalatee>(dt).ToList();
        }

        public List<CsHabilitationMetier> RetourneFonctionProfilCentre(string codefonction)
        {
            List<CsHabilitationMetier> listeDesHabilitations = new List<CsHabilitationMetier>(); ;

            List<CsFonction> lsfonction = RetourneFonctionByCode(codefonction);

            foreach (CsFonction fonction in lsfonction)
            {
                List<CsProfil> lesprofil = ProfilParfonction(fonction.PK_ID);

                foreach (CsProfil profil in lesprofil)
                {
                    List<CsHabilitationCentreProfil> lesCentreprofil = ProfilCentre(profil.PK_ID);
                    foreach (CsHabilitationCentreProfil centre in lesCentreprofil)
                    {
                        CsHabilitationMetier habilitation = new CsHabilitationMetier()
                        {
                            IDFONCTION = fonction.PK_ID,
                            LIBELLEFONCTION = fonction.ROLEDISPLAYNAME,
                            IDPROFIL = profil.PK_ID,
                            LIBELLEPROFIL = profil.LIBELLE,
                            IDCENTRE = centre.FK_IDCENTRE,
                            DATEDEBUTVALIDITE = centre.DATEDEBUTVALIDITE,
                            DATEFINVALIDITE = centre.DATEFINVALIDITE

                        };
                        listeDesHabilitations.Add(habilitation);
                    }
                }

            }

            return listeDesHabilitations;

        }

        public List<CsHabilitationMenu> RetourneFonctionProfilMenu(string codefonction)
        {
            List<CsHabilitationMenu> listeDesHabilitations = new List<CsHabilitationMenu>(); ;

            List<CsFonction> lsfonction = RetourneFonctionByCode(codefonction);

            foreach (CsFonction fonction in lsfonction)
            {
                List<CsProfil> lesprofil = ProfilParfonction(fonction.PK_ID);

                foreach (CsProfil profil in lesprofil)
                {
                    List<CSMenuGalatee> lesCentreprofil = RetourneMenusParProfil(profil.PK_ID);
                    foreach (CSMenuGalatee centre in lesCentreprofil)
                    {
                        CsHabilitationMenu habilitation = new CsHabilitationMenu()
                        {
                            IDFONCTION = fonction.PK_ID,
                            LIBELLEFONCTION = fonction.ROLEDISPLAYNAME,
                            IDPROFIL = profil.PK_ID,
                            LIBELLEPROFIL = profil.LIBELLE,
                            IDMENU = centre.PK_ID,
                            LIBELLEMENU = centre.MenuText,
                            LIBELLEFROMULAIRE = centre.FormName 

                        };
                        if (!string.IsNullOrEmpty(habilitation.LIBELLEFROMULAIRE))
                        listeDesHabilitations.Add(habilitation);
                    }
                }

            }
            return listeDesHabilitations;
        }

        public List<CsHabilitationMenu> RetourneProfilUtilisateur(List<int> idUtilisateur)
        {
            try
            {
                DataTable dt = AdminProcedures.RetourneUserByUser(idUtilisateur);
                List<CsUtilisateur> lstUser = Galatee.Tools.Utility.GetEntityListFromQuery<CsUtilisateur>(dt);

                DataTable dts = AdminProcedures.RetourneProfilByUser(idUtilisateur);
                List<CsProfil> lstProfil = Galatee.Tools.Utility.GetEntityListFromQuery<CsProfil>(dts);

                DataTable dtss = AdminProcedures.RetourneCentreByUser(idUtilisateur);
                List<CsCentreProfil> lstCentreProfil = Galatee.Tools.Utility.GetEntityListFromQuery<CsCentreProfil>(dtss);

                DataTable dtsss = AdminProcedures.RetourneMenuByUser();
                List<CSMenuGalatee> lstMenu = Galatee.Tools.Utility.GetEntityListFromQuery<CSMenuGalatee>(dtsss);

                List<CsHabilitationMenu> lstMenuUser = new List<CsHabilitationMenu>();
                foreach (CsUtilisateur item in lstUser)
                {
                    foreach (CsProfil items in lstProfil.Where(t=>t.FK_IDADMUTILISATEUR == item.PK_ID).ToList())
                    {
                        //foreach (CsCentreProfil itemsCentreProfil in lstCentreProfil.Where(t=>t.FK_IDPROFIL == items.FK_IDPROFIL).ToList())
                        //{
                            foreach (CSMenuGalatee lmenu in lstMenu.Where(t => t.FK_IDPROFIL == items.FK_IDPROFIL && !string.IsNullOrEmpty(t.FormName)).ToList())
                            {
                                CsHabilitationMenu habilitation = new CsHabilitationMenu()
                                {
                                    MATRICULE = item.MATRICULE,
                                    NOMUTILISATEUR = item.LIBELLE,
                                    LIBELLEPROFIL = items.LIBELLE,
                                    //LIBELLECENTRE = itemsCentreProfil.LIBELLECENTRE,
                                    DDEBUTVALIDITE = items.DATEDEBUT == null ? string.Empty : items.DATEDEBUT.ToString(),
                                    DFINVALIDITE = items.DATEFIN == null ? string.Empty : items.DATEFIN.ToString(),
                                    LIBELLEMENU = lmenu.MenuText 
                                };
                               lstMenuUser.Add(habilitation);
                            }
                        //}
                    }
                }
                return lstMenuUser;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    #region ADO .Net from Entity : Stephen 26-01-2019

        public List<CsFonction> SELECT_All_Fonction()
        {
            try
            {
                //DataTable dt = Galatee.Entity.Model.CommonProcedures.RetourneTousFonctions();
                DataTable dt = DB_ParametresGeneraux.SelectAllDonneReference("FONCTION");
                return Galatee.Tools.Utility.GetEntityFromQuery<CsFonction>(dt).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(EnumProcedureStockee.SelectTaCodeLibelleByNum + ":" + ex.Message);
            }
        }
        public List<CsProgramMenu> RetourneAllModuleProgram()
        {
            try
            {
                //DataTable dt = Galatee.Entity.Model.AdminProcedures.RetourneListeToutModuleEtSousMenu();
                DB_ParametresGeneraux db = new DB_ParametresGeneraux();
                List<CsProgramMenu> _LstProgramMenu = new List<CsProgramMenu>();
                _LstProgramMenu = db.RetourneListeAllModulesetSousmenus();
                return _LstProgramMenu;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    #endregion



    }
}
