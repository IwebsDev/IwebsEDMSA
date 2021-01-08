using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Galatee.Structure;
using System.Transactions;
using Galatee.Entity.Model;

namespace Galatee.DataAccess
{
    public class DBAdmUsers : Galatee.DataAccess.Parametrage.DbBase
    {
  

        public DBAdmUsers()
        {
            //Copers = Entities.GetEntityListFromQuery<COPER>(CommonProcedures.RetourneTousCoper());
            //Tops = Entities.GetEntityListFromQuery<LIBELLETOP>(CommonProcedures.RetourneTousLibelleTop());
            //Natures = Entities.GetEntityListFromQuery<NATURE>(CommonProcedures.RetourneNature());
            //Moderegs = Entities.GetEntityListFromQuery<MODEREG>(CommonProcedures.RetourneTousModeReglement());
            //Centres = Entities.GetEntityListFromQuery<CENTRE>(CommonProcedures.RetourneTousCentres());

            //Pour la connexion via ADO .Net :Steph 25-01-2019
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

        public bool Update(CsUtilisateur admUsers,bool IsInitpassword)
        {
            int resultat = -1;
            using ( galadbEntities context = new galadbEntities())
            {
                //ADMUTILISATEUR user = ObtenirUtilisateur(admUsers, IsInitpassword);

                ADMUTILISATEUR user = context.ADMUTILISATEUR.FirstOrDefault(m => m.PK_ID == admUsers.PK_ID);

                if (IsInitpassword)
                {
                    //ADMHISTORIQUEPASSWORD historique_password = ObtenirHistoriquePassword(admUsers);
                    //Entities.InsertEntity(historique_password, context);

                    user.INITUSERPASSWORD = admUsers.INITUSERPASSWORD;
                    user.FK_IDSTATUS = 1;
                    user.NOMBREECHECSOUVERTURESESSION = 0;
                    user.PASSE = admUsers.PASSE;
                    user.DATEDERNIEREMODIFICATIONPASSWORD = DateTime.Now;

                    Entities.UpdateEntity(user, context);
                    resultat= context.SaveChanges();
                    //Galatee.Tools.Utility.SendMail(user.E_MAIL, "Réinitialisation de mot de pass", "Votre nouveau mot de passe est " + user.FONCTION); 
                }
                else
                {
                    //ADMHISTORIQUECONNECTION historique_Conect = new ADMHISTORIQUECONNECTION() { DateConnection = System.DateTime.Now, FK_IDADMUTILISATEUR = admUsers.PK_ID, Login = admUsers.LOGINNAME, Matricule = admUsers.MATRICULE };
                    //Entities.InsertEntity(historique_Conect, context);

                    user.LIBELLE = admUsers.LIBELLE;
                    user.INITUSERPASSWORD = admUsers.INITUSERPASSWORD;
                    user.FK_IDSTATUS = byte.Parse(admUsers.FK_IDSTATUS.ToString());
                    user.NOMBREECHECSOUVERTURESESSION = admUsers.NOMBREECHECSOUVERTURESESSION;
                    user.PERIMETREACTION = byte.Parse(admUsers.PERIMETREACTION.ToString());
                    user.DATEDEBUTVALIDITE = admUsers.DATEDEBUTVALIDITE;
                    user.DATEFINVALIDITE = admUsers.DATEFINVALIDITE;
                    user.PASSE = admUsers.PASSE;
                    user.DATEDERNIEREMODIFICATIONPASSWORD = admUsers.DATEDERNIEREMODIFICATIONPASSWORD;
                    user.DERNIERECONNEXIONREUSSIE = admUsers.DERNIERECONNEXIONREUSSIE;
                    user.MATRICULE = admUsers.MATRICULE;
                    user.DATEDERNIERECONNEXION = admUsers.DATEDERNIERECONNEXION;
                    
                    
                    Entities.UpdateEntity(user, context);
                    resultat = context.SaveChanges();
                }
                return resultat == -1 ? false : true;
            }                    
        }

        private ADMHISTORIQUEPASSWORD ObtenirHistoriquePassword(CsUtilisateur puser)
        {
            ADMHISTORIQUEPASSWORD historique_user = new ADMHISTORIQUEPASSWORD();
            galadbEntities context = new galadbEntities();
            ADMUTILISATEUR  user = context.ADMUTILISATEUR.FirstOrDefault(m => m.PK_ID == puser.PK_ID);

            historique_user.DATECREATION = DateTime.Now;
            historique_user.DATEENREGISTREMENT  = DateTime.Now;
            historique_user.DATEMODIFICATION = user.DATEMODIFICATION;
            historique_user.FK_IDADMUTILISATEUR = user.PK_ID;
            historique_user.IDUSER = user.MATRICULE;
            historique_user.PASSWORD = user.PASSE;
            historique_user.USERCREATION = puser.USERCREATION;
            historique_user.USERMODIFICATION = puser.USERCREATION;

            context.Dispose();
            return historique_user;
        }



        private ADMUTILISATEUR ObtenirUtilisateur(CsUtilisateur puser, bool IsInitpassword)
        {
            using (galadbEntities context = new galadbEntities())
            {
                ADMUTILISATEUR user = context.ADMUTILISATEUR.FirstOrDefault(m => m.PK_ID == puser.PK_ID);

                if (IsInitpassword)
                {
                    user.INITUSERPASSWORD = puser.INITUSERPASSWORD;
                    user.FK_IDSTATUS = 1;
                    user.NOMBREECHECSOUVERTURESESSION = 0;
                    user.PASSE = puser.PASSE;
                    user.DATEDERNIEREMODIFICATIONPASSWORD = DateTime.Now;
                }
                else
                {
                    user.LIBELLE = puser.LIBELLE;
                    user.INITUSERPASSWORD = puser.INITUSERPASSWORD;
                    user.FK_IDSTATUS = byte.Parse(puser.FK_IDSTATUS.ToString());
                    user.NOMBREECHECSOUVERTURESESSION = puser.NOMBREECHECSOUVERTURESESSION;
                    user.PERIMETREACTION = byte.Parse(puser.PERIMETREACTION.ToString());
                    user.DATEDEBUTVALIDITE = puser.DATEDEBUTVALIDITE;
                    user.DATEFINVALIDITE = puser.DATEFINVALIDITE;
                    user.PASSE = puser.PASSE;
                    //user.ESTSUPPRIMER = puser.ESTSUPPRIMER ;
                    user.DATEDERNIEREMODIFICATIONPASSWORD = puser.DATEDERNIEREMODIFICATIONPASSWORD;
                    user.DERNIERECONNEXIONREUSSIE = puser.DERNIERECONNEXIONREUSSIE;
                    user.MATRICULE = puser.MATRICULE;
                    user.DATEDERNIERECONNEXION = puser.DATEDERNIERECONNEXION;
                    //user.FK_IDCENTRE  = puser.FK_IDCENTRE ;
                    //user.FK_IDSTATUS = byte.Parse(puser.FK_IDSTATUS.ToString());
                }
                return user;
            }
        }

        public bool Insert(CsUtilisateur admUsers)
        {
            try
            {
                //ObtenirLesIdentifiant(admUsers);
                Galatee.Entity.Model.ADMUTILISATEUR admUser = Galatee.Tools.Utility.ConvertEntity<Galatee.Entity.Model.ADMUTILISATEUR, CsUtilisateur>(admUsers);
                using (galadbEntities context = new galadbEntities())
                {
                    return Entities.InsertEntity<ADMUTILISATEUR>(admUser);
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool saveUserProfil(CsUtilisateur csUser, List<CsProfil> lesProfils)
        {
            try
            {
                Galatee.Entity.Model.ADMUTILISATEUR _user = Galatee.Tools.Utility.ConvertEntity<Galatee.Entity.Model.ADMUTILISATEUR, CsUtilisateur>(csUser);
                // valued menudu profil from menuProfil
               // List<PROFILSUTILISATEUR> lst = new List<PROFILSUTILISATEUR>();
                foreach (var item in lesProfils)
                {
                    PROFILSUTILISATEUR _profilUser = new PROFILSUTILISATEUR()
                    {
                        FK_IDADMUTILISATEUR = csUser.PK_ID,
                        FK_IDPROFIL = item.PK_ID 
                    };
                    _user.PROFILSUTILISATEUR.Add(_profilUser);
                    //lst.Add(_profilUser);
                }

                //_user.PROFILSUTILISATEUR.add(_profilUser);

                return Entities.InsertEntity<ADMUTILISATEUR>(_user);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool saveUserProfilCentre(CsUtilisateur csUser)
        {
            try
            {
                Galatee.Entity.Model.ADMUTILISATEUR _user = Galatee.Tools.Utility.ConvertEntity<Galatee.Entity.Model.ADMUTILISATEUR, CsUtilisateur>(csUser);

                List<Galatee.Entity.Model.CENTREDUPROFIL> ctreProfilAjouter = new List<CENTREDUPROFIL>();
                List<Galatee.Entity.Model.PROFILSUTILISATEUR> ProfilAjouter = new List<PROFILSUTILISATEUR>();
                foreach (CsProfil  item in csUser.LESPROFILSUTILISATEUR )
                {
                    ProfilAjouter.Add ( new PROFILSUTILISATEUR()
                    {
                        FK_IDADMUTILISATEUR = csUser.PK_ID,
                        FK_IDPROFIL = item.FK_IDPROFIL,
                        DATEDEBUT = item.DATEDEBUT ,
                        DATEFIN = item.DATEFIN 
                    });
                    foreach (CsCentreProfil items in item.LESCENTRESPROFIL )
                    {
                        ctreProfilAjouter.Add(new CENTREDUPROFIL()
                        {
                            FK_IDADMUTILISATEUR = csUser.PK_ID,
                            FK_IDPROFIL = items.FK_IDPROFIL,
                            FK_IDCENTRE = items.FK_IDCENTRE ,
                            DATEDEBUTVALIDITE =  items.DATEDEBUTVALIDITE ,
                            DATEFINVALIDITE = items.DATEFINVALIDITE
                        });
                    }
                }
                // centre affectation user
                CENTREAFFECTATION _uncentreaffect = new CENTREAFFECTATION()
                {
                    FK_IDADMUTILISATEUR = csUser.PK_ID,
                    FK_IDCENTRE = csUser.FK_IDCENTRE,
                    DATEDEBUTVALIDITE = (csUser.DATEDEBUTVALIDITE.HasValue) ? csUser.DATEDEBUTVALIDITE.Value : DateTime.Today,
                    DATEFINVALIDITE = csUser.DATEFINVALIDITE

                };
                _user.CENTREAFFECTATION.Add(_uncentreaffect);
                _user.CENTREDUPROFIL = ctreProfilAjouter;
                _user.PROFILSUTILISATEUR = ProfilAjouter;
                return Entities.InsertEntity<ADMUTILISATEUR>(_user);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        CAISSE CreationFondDeCaisse(CsUtilisateur pAdmin)
        {
            try
            {
               // int numcaisse = string.IsNullOrEmpty(pAdmin.NumCaisse) ? 0 : int.Parse(pAdmin.NumCaisse);
               //if (numcaisse > 0)
               //{

               //    galadbEntities pcontext = new galadbEntities();

               //    using (galadbEntities context = new galadbEntities())
               //    {
               //        // manipulation de la table TRANSCAISSE pour le fond de caisse
               //        List<TRANSCAISSE> _entityTransCaisse = context.TRANSCAISSE.Where(tr => tr.CAISSE == pAdmin.NUMCAISSE && tr.COPER == Enumere.CoperFondCaisse).ToList();//.FirstOrDefault();
               //        if (_entityTransCaisse.Count() == 0)// cet objet n'existe pas en base
               //        {

               //            TRANSCAISSE trcaisse = CreerTransCaisseFondCaisse(pAdmin);
               //            CAISSE caisse = CreerCaisseFondCaisse(pAdmin);

                         
               //            //trcaisse.CAISSE1 = caisse;
               //            //trcaisse.ADMUTILISATEUR = pAdmin;

               //            trcaisse.FK_IDCENTRE = pcontext.CENTRE.FirstOrDefault (c => c.CODECENTRE == pAdmin.CENTRE).PK_ID;
               //            trcaisse.FK_IDCOPER = pcontext.COPER.FirstOrDefault(c => c.CODE  == trcaisse.COPER).PK_ID;
               //            trcaisse.FK_IDLIBELLETOP = pcontext.LIBELLETOP .FirstOrDefault(t => t.CODE == trcaisse.TOP1).PK_ID;
               //            trcaisse.FK_IDMODEREG = pcontext.MODEREG .FirstOrDefault(m => m.CODE  == trcaisse.MODEREG).PK_ID;
               //            trcaisse.FK_IDNATURE = pcontext.NATURE .FirstOrDefault(n => n.CODE  == trcaisse.NATURE).PK_ID;

               //            trcaisse.CENTRE = pAdmin.CENTRE;
               //            trcaisse.MATRICULE = pAdmin.MATRICULE;
               //            trcaisse.CAISSE = pAdmin.NUMCAISSE;
               //            trcaisse.NATURE = trcaisse.NATURE;
               //            trcaisse.MODEREG = trcaisse.MODEREG;

               //            pAdmin.FK_IDCENTRE = pcontext.CENTRE.FirstOrDefault(c => c.CODECENTRE == pAdmin.CENTRE).PK_ID;
               //            pAdmin.FK_IDFONCTION = context.FONCTION.First(f => f.CODE == pAdmin.FONCTION).PK_ID;

               //            caisse.TRANSCAISSE.Add(trcaisse);
               //            caisse.ADMUTILISATEUR.Add(pAdmin);

               //            return caisse;
               //        }
               //        else
               //        {
               //            TRANSCAISSE _trcaisse = _entityTransCaisse.First();

               //            _trcaisse.MATRICULE = pAdmin.MATRICULE;
               //            _trcaisse.CENTRE = pAdmin.CENTRE;
               //            _trcaisse.FK_IDCENTRE = pAdmin.FK_IDCENTRE;
               //            _trcaisse.ADMUTILISATEUR = pAdmin;

               //             //trcaisse.USERMODIFICATION = pAdmin.USERMODIFICATION;
               //             //trcaisse.DATEMODIFICATION = pAdmin.DATEMODIFICATION;

               //             //trcaisse.CAISSE1.USERMODIFICATION = pAdmin.USERMODIFICATION;
               //             //trcaisse.CAISSE1.DATEMODIFICATION = pAdmin.DATEMODIFICATION;

               //            return _trcaisse.CAISSE1;
               //        }
               //    }
               //}
               //else
               //{ 
               //    using(galadbEntities context = new  galadbEntities())
               //    {
               //        pAdmin.FK_IDNUMCAISSE = context.CAISSE.First(c => c.NUMCAISSE == "000").PK_ID;
               //        pAdmin.FK_IDCENTRE = context.CENTRE.FirstOrDefault(c => c.CODECENTRE == pAdmin.CENTRE).PK_ID;
               //        pAdmin.FK_IDFONCTION = context.FONCTION.First(f => f.CODE == pAdmin.FONCTION).PK_ID;
               //        pAdmin.NUMCAISSE = "000";
               //        //pAdmin.CAISSE = context.CAISSE.First(c => c.NUMCAISSE == "000");//.PK_ID;
               //        //pAdmin.CENTRE1 = Centres.First(c => c.CODECENTRE == pAdmin.CENTRE);//.PK_ID;
               //        //pAdmin.FONCTION1 = context.FONCTION.First(f => f.CODE == pAdmin.FONCTION);//.PK_ID;
               //    }
               //}

               return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private CAISSE CreerCaisseFondCaisse(ADMUTILISATEUR pAdmin)
        {
            try
            {
                CAISSE caisse = new CAISSE()
                   {
                       //NUMCAISSE = pAdmin.NUMCAISSE,
                       ACQUIT = Enumere.AcquitInitial,
                       BORDEREAU = Enumere.BordereauInitial,
                       FONDCAISSE = 0,
                       TYPECAISSE = Enumere.TypeCaisse,
                       USERCREATION = pAdmin.USERCREATION,
                   };

                return caisse;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private ADMUTILISATEUR  User(CsUtilisateur  pAdmin)
        {
            try
            {
                ADMUTILISATEUR user = new ADMUTILISATEUR()
                {
                    FK_IDCENTRE  = pAdmin.FK_IDCENTRE   ,
                    //FK_IDFONCTION  =pAdmin.FK_IDFONCTION  ,
                     
                };

                return user;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private TRANSCAISSE CreerTransCaisseFondCaisse(ADMUTILISATEUR pAdmin)
        {
            try
            {
                TRANSCAISSE trcaisse = new TRANSCAISSE()
                   {
                       //FK_IDCENTRE = pAdmin.FK_IDCENTRE,       
                       ORDRE = "//",
                       //CAISSE = pAdmin.NUMCAISSE,
                       ACQUIT = "/////////",
                       NDOC = "//////",
                       REFEM = "//////",
                       MONTANT = 0,
                       DC = Enumere.Debit,
                       COPER = Enumere.CoperFondCaisse,
                       TOP1 = Enumere.Top1Administration,
                       PERCU = 0,
                       RENDU = 0,
                       MODEREG = Enumere.ModePayementEspece,
                       DTRANS = DateTime.Now,
                       USERCREATION = pAdmin.USERCREATION,

                   };

                return trcaisse;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public CsUtilisateur GetUtilisateurByMatricule(string Matricule)
        {
            return new CsUtilisateur();
        }

        //private void ObtenirLesIdentifiant(CsUtilisateur admUsers)
        //{
        //    try
        //    {
        //        using (galadbEntities context = new galadbEntities())
        //        {
        //            FONCTION fxtion = context.FONCTION.FirstOrDefault(f => f.CODE == admUsers.FONCTION);
        //            CENTRE centre = context.CENTRE.FirstOrDefault(c => c.CODECENTRE == admUsers.CENTRE );
        //            admUsers.FONCTION  = fxtion.PK_ID.ToString();
        //            admUsers.CE = centre.PK_ID.ToString();
        //        };
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public bool Delete(Guid IdUser)
        {
            cn = new SqlConnection(ConnectionString);
            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "spx_AdmUsers_Delete";
            cmd.Parameters.Clear();
            cmd.Parameters.Add("@IdUser", SqlDbType.VarChar).Value = IdUser;


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

        /*Adaptation Galatee - Par HGB*/
        public bool Delete(string Matricule)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    ADMUTILISATEUR user = context.ADMUTILISATEUR.First(a => a.MATRICULE == Matricule);
                    user.ESTSUPPRIMER = true;
                    //user.DATEMODIFICATION = 
                    //user.USERMODIFICATION = 
                    return DeleteUser(user);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool DeleteUser(ADMUTILISATEUR pUtilisateur)
        {

            try
            {
                //Galatee.Entity.Model.ADMUTILISATEUR user = Galatee.Tools.Utility.ConvertEntity<Galatee.Entity.Model.ADMUTILISATEUR, CsUtilisateur>(pUtilisateur);
                return Entities.UpdateEntity<Galatee.Entity.Model.ADMUTILISATEUR>(pUtilisateur);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool? Delete(CsUtilisateur pUtilisateur)
        {
            try
            {
                int Returva = -1;
                using (galadbEntities ctx=new galadbEntities())
                {
                    ADMUTILISATEUR leUser = ctx.ADMUTILISATEUR.FirstOrDefault(m => m.PK_ID == pUtilisateur.PK_ID);
                    if (leUser != null ) leUser.ESTSUPPRIMER = true ;

                    List<PROFILSUTILISATEUR>  lesProfilUser = ctx.PROFILSUTILISATEUR.Where (m => m.FK_IDADMUTILISATEUR  == pUtilisateur.PK_ID).ToList();
                    if (lesProfilUser != null && lesProfilUser.Count != 0) lesProfilUser.ForEach(i=>i.DATEFIN = DateTime.Now );

                    List<CENTREDUPROFIL> lesCentre = ctx.CENTREDUPROFIL.Where(m => m.FK_IDADMUTILISATEUR  == pUtilisateur.PK_ID).ToList();
                    if (lesCentre != null && lesCentre.Count != 0) lesCentre.ForEach(i => i.DATEFINVALIDITE  = DateTime.Now);
                    Returva =ctx.SaveChanges();
                }
                return Returva == -1 ? false : true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        ADMUTILISATEUR ObtenirAdmUser(CsUtilisateur pUtilisateur)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    ADMUTILISATEUR user = context.ADMUTILISATEUR.First(a => a.MATRICULE == pUtilisateur.MATRICULE );
                    user.ESTSUPPRIMER = true;
                    user.DATEMODIFICATION = pUtilisateur.DATEMODIFICATION;
                    user.USERMODIFICATION = pUtilisateur.USERMODIFICATION;
                    return user;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CsUtilisateur> GetAllUser()
        {
            //cmd.CommandText = "SPX_ADM_RETOURNE_ALLUSER";

            try
            {
                DataTable dt = AdminProcedures.RetourneListeToutUtilisateur();
                return Entities.GetEntityListFromQuery<CsUtilisateur>(dt);
                    //c.FK_Centre  = (Convert.IsDBNull(reader["CENTRE"])) ? String.Empty : (System.String)reader["CENTRE"];
                    //c.PK_Matricule = (Convert.IsDBNull(reader["MATRICULE"])) ? String.Empty : (System.String)reader["MATRICULE"];
                    //c.Nom  = (Convert.IsDBNull(reader["LIBELLE"])) ? String.Empty : (System.String)reader["LIBELLE"];
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //ATO le 10/04/2013
        //public List<CsUtilisateur> GetUserByIdFonctionMatriculeNom(string pIdFonction, string pMatriculeAgent, string pNomAgent)
        //{
        //    try
        //    {
        //        DataTable TableAdmUser = new DataTable();
        //        //DataRow[] dr = AdminProcedures.DEVIS_ADMUTILISATEUR_SEARCH_AGENTByIdFonctionMatriculeNom().Select(string.Format(Enumere.FiltrerAdmUtilisateur, string.IsNullOrEmpty(pIdFonction) ? "NULL" : pIdFonction,
        //        //string.IsNullOrEmpty(pMatriculeAgent) ? "NULL" : pMatriculeAgent,
        //        //string.IsNullOrEmpty(pNomAgent) ? "NULL" : pNomAgent));
        //        DataView dv = new DataView(AdminProcedures.DEVIS_ADMUTILISATEUR_SEARCH_AGENTByIdFonctionMatriculeNom(),string.Format(Enumere.FiltrerAdmUtilisateur, string.IsNullOrEmpty(pIdFonction) ? "NULL" : pIdFonction,
        //        string.IsNullOrEmpty(pMatriculeAgent) ? "NULL" : pMatriculeAgent,
        //        string.IsNullOrEmpty(pNomAgent) ? "NULL" : pNomAgent),"PK_MATRICULE ASC",DataViewRowState.CurrentRows);

        //        TableAdmUser = dv.ToTable();

        //        return Entities.GetEntityListFromQuery<CsUtilisateur>(TableAdmUser);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}


        public CsUtilisateur GetByMatricule(string pMatricule)
        {
            try
            {
                return Entities.GetEntityFromQuery<CsUtilisateur>(AdminProcedures.RetourneUtilisateurParMatricule(pMatricule));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CsUtilisateur> GetUserByIdFonctionMatriculeNom(string pIdFonction, string pMatriculeAgent, string pNomAgent)
        {
            try
            {

                System.Data.DataTable dt = AdminProcedures.DEVIS_ADMUTILISATEUR_SEARCH_AGENTByIdFonctionMatriculeNom(pIdFonction, pMatriculeAgent, pNomAgent);

                return Entities.GetEntityListFromQuery<CsUtilisateur>(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CsUtilisateur> GetUserByIdGroupeValidationMatriculeNom(Guid pIdGroupeValidation, int IdCentreDemande, string CodeProfil , string  Matricule,string NomAgent)
        {
            try
            {

                System.Data.DataTable dt = AdminProcedures.GetUserByIdGroupeValidationMatriculeNom(pIdGroupeValidation, IdCentreDemande, CodeProfil,Matricule ,NomAgent );
                return  Entities.GetEntityListFromQuery<CsUtilisateur>(dt);
                //List<CsUtilisateur> lst = Entities.GetEntityListFromQuery<CsUtilisateur>(dt);
                //if (lst.Count != 0)
                //    return lst.Where(t => t.STATUSCOMPTE == Enumere.StatusActive).ToList();
                //else
                //    return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool InsertPoste(CsPoste lePoste)
        {
            try
            {

                return Galatee.Entity.Model.AdminProcedures.InsererPoste(lePoste);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool UpdatePoste(CsPoste lePoste)
        {
            try
            {
                return Galatee.Entity.Model.AdminProcedures.UpdatePoste(lePoste);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List< CsHistoriquePassword>  RetourneHistoriquePassword(int  iduser)
        {
            try
            {
                DataTable dt = Galatee.Entity.Model.AdminProcedures.RetourneHistoriquePassword(iduser);
                return Entities.GetEntityListFromQuery<CsHistoriquePassword>(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CsHistoriquePassword> RetourneHistoriqueConnection(int iduser)
        {
            try
            {
                DataTable dt = Galatee.Entity.Model.AdminProcedures.RetourneHistoriqueConnection(iduser);
                return Entities.GetEntityListFromQuery<CsHistoriquePassword>(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool  SynchroniseDonneesAD()
        {
            try
            {
                Galatee.Tools.ADUtilities monAd = new Galatee.Tools.ADUtilities("s2kouakoul", "Lud@vic10", "inova.local");
                List<Galatee.Tools.ADUser> users = monAd.GetUser();
                List<AGENT> lstDesUSerAdInsert = new List<AGENT>();
                List<AGENT> lstDesUSerAdUpdate = new List<AGENT>();
                galadbEntities ContextInter = new galadbEntities();
                int Resultat = -1;
                int i = 1;
                foreach (Galatee.Tools.ADUser u in users)
                {
                    if (string.IsNullOrEmpty(u.UserName) || !u.UserName.Contains("s2"))
                        continue;

                    if (ContextInter.AGENT.FirstOrDefault(t => t.COMPTEWINDOWS == u.UserName ) != null)
                        continue;
                    else
                    {
                        AGENT leUser = new AGENT()
                        {
                            NOM = u.DisplayName,
                            // Email = u.Email ,
                            //Groupe = u.Groupe ,
                            COMPTEWINDOWS = u.UserName,
                            MATRICULE = u.UserName.Substring(0,3) + i
                            

                        };
                        lstDesUSerAdInsert.Add(leUser);
                        i++;
                    }
                }
                using (galadbEntities Context = new galadbEntities())
                {
                    Entities.InsertEntity <AGENT>(lstDesUSerAdInsert);
                     Resultat=  Context.SaveChanges();
                }
                return (Resultat == -1) ? false : true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

      public bool InsertUpdateUser(CsUtilisateur admUsers)
        {
            try
            {


                List<CsProfil> lstProfilAsupp = new List<CsProfil>();
                List<CsProfil> lstProfilAAjout = new List<CsProfil>();
                List<CsProfil> lstProfilNonModifier = new List<CsProfil>();

                List<CsProfil> lstProfilInit = new List<CsProfil>();
                List<CsCentreProfil> lstCentreASupprimer = new List<CsCentreProfil>();
                List<CsCentreProfil> lstCentreAjouter = new List<CsCentreProfil>();

                CsCentreProfil leAncientCentreAffectation = new CsCentreProfil();
                CENTREAFFECTATION leNouveauCentre = new CENTREAFFECTATION();
                List<int> lstIdCentreInit = new List<int>();
                List<int> lstIdProfilAnc = new List<int>();
                List<int> lstIdProfilNouv = new List<int>();

                List<int> lstIdProfilAJouter = new List<int>();
                List<int> lstIdProfilSupprimer = new List<int>();
                List<int> lstIdProfilNonModifier = new List<int>();

                 //DataTable query1 = Galatee.Entity.Model.AuthentProcedures.GetProfilUserByLoginName(admUsers.PK_ID );
                 //lstProfilInit = Tools.Utility.GetEntityListFromQuery <CsProfil>(query1);

                lstProfilInit = new DBAuthentification().GetProfilActifUser(admUsers.PK_ID);


                // Mutation
                 //if (admUsers.FK_IDCENTRE != admUsers.FK_IDANCIENCENTRE)
                 //{
                     //DataTable query3 = Galatee.Entity.Model.AuthentProcedures.GetCentreAffectation(admUsers.PK_ID);
                     //leAncientCentreAffectation = Tools.Utility.GetEntityFromQuery<CsCentreProfil>(query3).First();

                     //leAncientCentreAffectation = new DBAuthentification().GetAllCentreProfilActif(admUsers.PK_ID);

                 //    leNouveauCentre = new CENTREAFFECTATION()
                 //    {
                 //        FK_IDADMUTILISATEUR = admUsers.PK_ID,
                 //        FK_IDCENTRE = admUsers.FK_IDCENTRE,
                 //        DATEDEBUTVALIDITE = (admUsers.DATEDEBUTVALIDITE.HasValue) ? admUsers.DATEDEBUTVALIDITE.Value : DateTime.Today,
                 //        DATEFINVALIDITE = admUsers.DATEFINVALIDITE

                 //    };
                 //}
                //

                foreach (CsProfil item in lstProfilInit)
                     lstIdProfilAnc.Add(item.FK_IDPROFIL);

                foreach (CsProfil item in admUsers.LESPROFILSUTILISATEUR)
                {
                    if (admUsers.PK_ID != 0) item.FK_IDADMUTILISATEUR = admUsers.PK_ID;
                    lstIdProfilNouv.Add(item.FK_IDPROFIL);
                }
                // Ajout
                 lstIdProfilAJouter = lstIdProfilNouv.Where(t => !lstIdProfilAnc.Contains(t)).ToList();
                 lstProfilAAjout = admUsers.LESPROFILSUTILISATEUR.Where(t => lstIdProfilAJouter.Contains(t.FK_IDPROFIL)).ToList();
                 foreach (CsProfil item in lstProfilAAjout)
                 {
                     if (admUsers.PK_ID != 0) item.FK_IDADMUTILISATEUR = admUsers.PK_ID;
                     if (admUsers.PK_ID != 0) item.LESCENTRESPROFIL.ForEach(i=>i.FK_IDADMUTILISATEUR = admUsers.PK_ID);
                     lstCentreAjouter.AddRange(item.LESCENTRESPROFIL);
                 }
                //

                // Supprimer
                 lstIdProfilSupprimer = lstIdProfilAnc.Where(t => !lstIdProfilNouv.Contains(t)).ToList();
                 lstProfilAsupp = lstProfilInit.Where(t => lstIdProfilSupprimer.Contains(t.FK_IDPROFIL)).ToList();
                 foreach (CsProfil item in lstProfilAsupp)
                 {
                     //DataTable query2 = Galatee.Entity.Model.AuthentProcedures.GetCentreDesProfilsUserByLoginName(admUsers.PK_ID, item.FK_IDPROFIL);
                     //lstCentreASupprimer.AddRange(Tools.Utility.GetEntityListFromQuery<CsCentreProfil>(query2)); 

                     lstCentreASupprimer.AddRange(new DBAuthentification().GetCentreProfilActif(admUsers.PK_ID, item.FK_IDPROFIL));
                 }
                //

                 // Non modifier mais centre a verifier
                 lstIdProfilNonModifier = lstIdProfilAnc.Where(t => lstIdProfilNouv.Contains(t)).ToList();
                 lstProfilNonModifier = admUsers.LESPROFILSUTILISATEUR.Where(t => lstIdProfilNonModifier.Contains(t.FK_IDPROFIL)).ToList();
                 foreach (CsProfil item in lstProfilNonModifier)
                 {
                     //DataTable query2 = Galatee.Entity.Model.AuthentProcedures.GetCentreDesProfilsUserByLoginName(admUsers.PK_ID, item.FK_IDPROFIL);
                     //List<CsCentreProfil> lstCentreInt = Tools.Utility.GetEntityListFromQuery<CsCentreProfil>(query2);

                     List<CsCentreProfil> lstCentreInt  =new DBAuthentification().GetCentreProfilActif(admUsers.PK_ID, item.FK_IDPROFIL);

                     List<int> IdCentreInit = new List<int>();
                     foreach (CsCentreProfil items in lstCentreInt)
                         IdCentreInit.Add(items.FK_IDCENTRE);

                     List<int> IdCentreNouv = new List<int>();
                     if (item.LESCENTRESPROFIL == null) continue;
                         foreach (CsCentreProfil items in item.LESCENTRESPROFIL)
                             IdCentreNouv.Add(items.FK_IDCENTRE);
                     List<int> lstCentreAJouter = IdCentreNouv.Where(t => !IdCentreInit.Contains(t)).ToList();
                     lstCentreAjouter.AddRange(item.LESCENTRESPROFIL.Where(t => lstCentreAJouter.Contains(t.FK_IDCENTRE )).ToList());

                     List<int> lstCentreSupprimer = IdCentreInit.Where(t => !IdCentreNouv.Contains(t)).ToList();
                     lstCentreASupprimer.AddRange(lstCentreInt.Where(t => lstCentreSupprimer.Contains(t.FK_IDCENTRE )).ToList());
                 }


                 Galatee.Entity.Model.ADMUTILISATEUR admUser = Galatee.Tools.Utility.ConvertEntity<Galatee.Entity.Model.ADMUTILISATEUR, CsUtilisateur>(admUsers);

                 List<Galatee.Entity.Model.PROFILSUTILISATEUR> ProfilAjouter = RetourneProfil(lstProfilAAjout);
                 List<Galatee.Entity.Model.CENTREDUPROFIL> ctreProfilAjouter = RetourneCentreProfil(lstCentreAjouter);
                 List<Galatee.Entity.Model.CENTREDUPROFIL> ctreProfilSupp = RetourneCentreProfil(lstCentreASupprimer);
                 List<Galatee.Entity.Model.PROFILSUTILISATEUR> ProfilSupp = RetourneProfil(lstProfilAsupp);
                 Galatee.Entity.Model.CENTREAFFECTATION leAnCentreAffect = Galatee.Tools.Utility.ConvertEntity<CENTREAFFECTATION, CsCentreProfil>(leAncientCentreAffectation);


                 ctreProfilSupp.ForEach(t => t.DATEFINVALIDITE = System.DateTime.Today);
                 ProfilSupp.ForEach(t => t.DATEFIN = System.DateTime.Today);

               


                 using (galadbEntities context = new galadbEntities())
                 {
                     Entities.UpdateEntity<Galatee.Entity.Model.ADMUTILISATEUR>(admUser, context);
                     Entities.UpdateEntity<Galatee.Entity.Model.PROFILSUTILISATEUR>(ProfilSupp, context);
                     Entities.UpdateEntity<Galatee.Entity.Model.CENTREDUPROFIL>(ctreProfilSupp, context);
                     if (leAnCentreAffect != null && leAnCentreAffect.FK_IDADMUTILISATEUR != 0 && leAnCentreAffect.FK_IDCENTRE != 0 )
                     Entities.UpdateEntity<Galatee.Entity.Model.CENTREAFFECTATION>(leAnCentreAffect, context);

                     Entities.InsertEntity<Galatee.Entity.Model.CENTREDUPROFIL>(ctreProfilAjouter, context);
                     Entities.InsertEntity<Galatee.Entity.Model.PROFILSUTILISATEUR>(ProfilAjouter, context);
                     if (leNouveauCentre.FK_IDADMUTILISATEUR != 0 && leNouveauCentre.FK_IDCENTRE != 0)
                     Entities.InsertEntity<Galatee.Entity.Model.CENTREAFFECTATION>(leNouveauCentre, context);

                     context.SaveChanges();
                 }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return true;
        }
      private List<CENTREDUPROFIL> RetourneCentreProfil(List<CsCentreProfil > lstCentreProfil)
      {
          try
          {
              List<CENTREDUPROFIL> lstCentreProfilUser = new List<CENTREDUPROFIL>();
              foreach (CsCentreProfil  item in lstCentreProfil)
              {
                  lstCentreProfilUser.Add(new CENTREDUPROFIL()
                  {
                      FK_IDPROFIL = item.FK_IDPROFIL,
                      FK_IDADMUTILISATEUR = item.FK_IDADMUTILISATEUR,
                      FK_IDCENTRE = item.FK_IDCENTRE ,
                      DATEDEBUTVALIDITE = item.DATEDEBUTVALIDITE ,
                      DATEFINVALIDITE = item.DATEFINVALIDITE ,
                      PK_ID = item .PK_ID 
                  });
              }
              return lstCentreProfilUser;
          }
          catch (Exception ex)
          {
              
              throw ex;
          }
      
      }

      private List<PROFILSUTILISATEUR> RetourneProfil(List<CsProfil> lstProfil)
      {
          try
          {
              List<PROFILSUTILISATEUR> lstProfilUser = new List<PROFILSUTILISATEUR>();
              foreach (CsProfil item in lstProfil)
              {
                  lstProfilUser.Add(new PROFILSUTILISATEUR()
                  {
                      FK_IDPROFIL = item.FK_IDPROFIL,
                      FK_IDADMUTILISATEUR = item.FK_IDADMUTILISATEUR,
                      DATEDEBUT = item.DATEDEBUT,
                      DATEFIN = item.DATEFIN,
                      PK_ID = item.PK_ID
                  });
              }
              return lstProfilUser;
          }
          catch (Exception ex)
          {

              throw ex;
          }

      }

        public static bool MisAjourProfil(CsUtilisateur admUsers, List<CsProfil> anciensProfil, galadbEntities pContext)
        {
            try
            {
                List<int> aajouterCentreIdDuProfils = new List<int>();
                List<int> asupprimerCentreIdProfils = new List<int>();

                List<int> aajouterIdDuProfils = new List<int>();
                List<int> asupprimerIdProfils = new List<int>();

                List<CsProfil> lstProfilFin = admUsers.LESPROFILSUTILISATEUR;

                if (lstProfilFin != null &&
                    lstProfilFin.Count != 0)
                {
                    List<string> IdProfilsInit = new List<string>();
                    List<string> IdProfilsInitFin = new List<string>();

                    List<string> codeAppareilFin = new List<string>();
                    foreach (var item in anciensProfil)
                        IdProfilsInit.Add(item.CODE);

                    foreach (var item in lstProfilFin)
                        IdProfilsInitFin.Add(item.CODE);

                    List<CsProfil> LstASupprimer = anciensProfil.Where(t => !IdProfilsInitFin.Contains(t.CODE )).ToList();
                    List<CsProfil> LstAAjouter = lstProfilFin.Where(t => !IdProfilsInitFin.Contains(t.CODE)).ToList();
                    //foreach (CsProfil item in LstASupprimer)
                    //{
                    //    asupprimerIdProfils.Add(item.PK_ID);
                    //    foreach (var items in item.LESCENTRESDUPROFIL)
                    //    { 
                    //      fro
                    //    }
                    //}
                    
                  
                }
                else
                {
                    
                }
                return false;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public bool UpdateUserProfilCentre(CsUtilisateur csUser, List<CsCentreDuProfil> lstCentreDuProfilajouter)
        {
            bool ttz = false, tt = false;
            try
            {


                Galatee.Entity.Model.ADMUTILISATEUR _user = Galatee.Tools.Utility.ConvertEntity<Galatee.Entity.Model.ADMUTILISATEUR, CsUtilisateur>(csUser);

                foreach (CsCentreDuProfil centreprofil in lstCentreDuProfilajouter)
                {
                    // le profil
                    PROFILSUTILISATEUR _profilUser = new PROFILSUTILISATEUR()
                    {
                        FK_IDADMUTILISATEUR = csUser.PK_ID,
                        FK_IDPROFIL = centreprofil.unprofil.PK_ID,
                        //DATEDEBUTVALIDITE = (centreprofil.DATEDEBUTVALIDITE.HasValue) ? centreprofil.DATEDEBUTVALIDITE.Value : DateTime.Now,
                    };

                    tt = Entities.InsertEntity<PROFILSUTILISATEUR>(_profilUser);

                    //_user.PROFILSUTILISATEUR.Add(_profilUser);   

                    //les centre du profil
                    foreach (CsCentre centre in centreprofil.lescentres)
                    {
                        CENTREDUPROFIL _uncentre = new CENTREDUPROFIL()
                        {
                            FK_IDADMUTILISATEUR = csUser.PK_ID,
                            FK_IDPROFIL = centreprofil.unprofil.PK_ID,
                            FK_IDCENTRE = centre.PK_ID,
                            DATEDEBUTVALIDITE = (centreprofil.DATEDEBUTVALIDITE.HasValue) ? centreprofil.DATEDEBUTVALIDITE.Value : DateTime.Now,
                            DATEFINVALIDITE = centreprofil.DATEFINVALIDITE

                        };
                        ttz = Entities.InsertEntity<CENTREDUPROFIL>(_uncentre);

                    }

                }

                return ttz && tt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        void MettreFinCentreProfilUtilisateur(int IdadmUsers, int IdancienProfil)
        {
            try
            {
                List<Galatee.Entity.Model.CENTREDUPROFIL> lst_entity = new List<CENTREDUPROFIL>();

                using (Galatee.Entity.Model.galadbEntities context = new galadbEntities())
                {
                    lst_entity = context.CENTREDUPROFIL.Where(pk => pk.FK_IDADMUTILISATEUR == IdadmUsers)
                        .Where(pk => pk.FK_IDPROFIL == IdancienProfil)
                        .Where(pk => pk.DATEFINVALIDITE == null)
                        //.Where(pk => pk.FK_IDCENTRE == IdancienCentre)
                        //.Where(pk => pk.DATEFINVALIDITE > DateTime.Today.AddDays(-1))
                        .ToList();// date d'hier a 00h:00min:00s non expiré
                }

                foreach (var entity in lst_entity)
                {
                    if (entity.PK_ID != 0)
                    {
                        entity.DATEFINVALIDITE = DateTime.Now;
                        Entities.UpdateEntity<Galatee.Entity.Model.CENTREDUPROFIL>(entity);
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        void MettreFinProfilUtilisateur(int IdadmUsers, int IdancienProfil)
        {
            try
            {
                List<Galatee.Entity.Model.PROFILSUTILISATEUR> lst_entity = new List<PROFILSUTILISATEUR>();

                using (Galatee.Entity.Model.galadbEntities context = new galadbEntities())
                {
                    lst_entity = context.PROFILSUTILISATEUR.Where(pk => pk.FK_IDADMUTILISATEUR == IdadmUsers)
                        .Where(pk => pk.FK_IDPROFIL == IdancienProfil)
                        //.Where(pk => pk.DATEFINVALIDITE == null)
                        ////.Where(pk => pk.DATEFINVALIDITE > DateTime.Today.AddDays(-1))
                        .ToList();// date d'hier a 00h:00min:00s non expiré
                }

                foreach (var entity in lst_entity)
                {
                    if (entity.PK_ID != 0)
                    {
                        //entity.DATEFINVALIDITE = DateTime.Now;
                        Entities.UpdateEntity<Galatee.Entity.Model.PROFILSUTILISATEUR>(entity);
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public bool InsertionMutation(Galatee.Structure.CsUtilisateur lutilisateur, string newCentre, DateTime datedebut, DateTime? datefin)
        {
            try
            {
                Galatee.Entity.Model.ADMUTILISATEUR admUser = Galatee.Tools.Utility.ConvertEntity<Galatee.Entity.Model.ADMUTILISATEUR, CsUtilisateur>(lutilisateur);
                CENTREAFFECTATION Exaffectation = null;
                List<CENTREDUPROFIL> lstProfilUser = null;
                using (galadbEntities context0 = new galadbEntities())
                {
                    Exaffectation = context0.CENTREAFFECTATION.FirstOrDefault(x => x.FK_IDADMUTILISATEUR == lutilisateur.PK_ID);
                    lstProfilUser = context0.CENTREDUPROFIL.Where(p => p.FK_IDADMUTILISATEUR == lutilisateur.PK_ID).ToList();

                    lstProfilUser.ForEach(c => c.DATEFINVALIDITE = DateTime.Now);
                    Exaffectation.DATEFINVALIDITE = DateTime.Now;

                    context0.Dispose();
                };

                using (galadbEntities context = new galadbEntities())
                {
                    admUser.DATEMODIFICATION = DateTime.Now;
                    admUser.FK_IDCENTRE = context.CENTRE.FirstOrDefault(x => x.CODE == newCentre).PK_ID;

                    CENTREAFFECTATION affectation = new CENTREAFFECTATION()
                    {
                        FK_IDADMUTILISATEUR = admUser.PK_ID,
                        FK_IDCENTRE = admUser.FK_IDCENTRE,
                        DATEDEBUTVALIDITE = datedebut,
                        DATEFINVALIDITE = datefin
                    };




                    Entities.UpdateEntity<ADMUTILISATEUR>(admUser);
                    Entities.UpdateEntity<CENTREAFFECTATION>(Exaffectation);
                    Entities.UpdateEntity<CENTREDUPROFIL>(lstProfilUser);
                    Entities.InsertEntity<CENTREAFFECTATION>(affectation);

                    context.SaveChanges();
                    context.Dispose();
                    return true;
                };


            }
            catch (Exception ex)
            {
                return false;
                throw ex;
            }
        }

        public List<CsHistoriquePassword> RetourneHistoriqueConnectionFromListUser(List<int?> iduser)
        {
            try
            {
                DataTable dt = Galatee.Entity.Model.AdminProcedures.RetourneHistoriqueConnectionFromListUser(iduser);
                return Entities.GetEntityListFromQuery<CsHistoriquePassword>(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CsHistoriquePassword> RetourneHistoriquePasswordFromListUser(List<int?> iduser)
        {
            try
            {
                DataTable dt = Galatee.Entity.Model.AdminProcedures.RetourneHistoriquePasswordFromListUser(iduser);
                return Entities.GetEntityListFromQuery<CsHistoriquePassword>(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool? DeleteList(List<CsUtilisateur> pUtilisateur)
        {
            try
            {
                List<ADMUTILISATEUR> user = new List<ADMUTILISATEUR>();
                foreach (var item in pUtilisateur)
                    user.Add(ObtenirAdmUser(item));
                return Entities.UpdateEntity<ADMUTILISATEUR>(user);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        #region ADO .Net from Entity : Stephen 25-01-2019
            #region Entity
            #endregion

        public List<CsPoste> GetAllPoste()
        {
            cn = new SqlConnection(ConnectionString);
            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandTimeout = 180;
            cmd.CommandType = CommandType.StoredProcedure;
            if (cmd.Parameters != null && cmd.Parameters.Count != 0) cmd.Parameters.Clear();
            cmd.CommandText = "SPX_ADMIN_GETALLPOSTE";

            DBBase.SetDBNullParametre(cmd.Parameters);
            try
            {
                if (cn.State == ConnectionState.Closed)
                    cn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);
                return Entities.GetEntityListFromQuery<CsPoste>(dt);
            }
            catch (Exception ex)
            {
                throw new Exception(cmd.CommandText + ":" + ex.Message);
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                    cn.Close(); // Fermeture de la connection 
                cmd.Dispose();
            }
        }

        //26-01-2019
        public List<CsUtilisateur> GetAll()
        {
            try
            {
                //DataTable objet = Galatee.Entity.Model.AdminProcedures.RetourneListeToutUtilisateur();
                //DataTable objet = DB_ParametresGeneraux.SelectAllDonneReference("ADMUTILISATEUR");
                //return Galatee.Tools.Utility.GetEntityFromQuery<CsUtilisateur>(objet).ToList();

                DB_ParametresGeneraux db = new DB_ParametresGeneraux();
                List<CsUtilisateur> _LstItem = new List<CsUtilisateur>();
                _LstItem = db.RetourneListeAllUsers();
                return _LstItem;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CsUtilisateur> GetAllPerimetre(List<int> lstCentrePerimetreAction)
        {
            try
            {
                //DataTable objet = Galatee.Entity.Model.AdminProcedures.RetourneListePerimetreUtilisateur(lstCentrePerimetreAction);
                DataTable dt = DB_ParametresGeneraux.SelectAllDonneReference("ADMUTILISATEUR");
                return Galatee.Tools.Utility.GetEntityFromQuery<CsUtilisateur>(dt).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CsAgent> ChargeListeDesAgents()
        {
            try
            {
                //DataTable dt = Galatee.Entity.Model.AdminProcedures.ChargeListeDesAgents();
                DataTable dt = DB_ParametresGeneraux.SelectAllDonneReference("AGENT");
                List<CsAgent> _ListItem = Entities.GetEntityListFromQuery<CsAgent>(dt);
                foreach(CsAgent item in _ListItem)
                     item.IDENTITE = item.NOM +"  "+ item.PRENOM ;

                return _ListItem;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //08-03-2019
        public List<CsPoste> RetourneListePoste()
        {
            try
            {
                //return Entities.GetEntityListFromQuery<CsPoste>(AdminProcedures.RetourneListePoste());

                DB_ParametresGeneraux db = new DB_ParametresGeneraux();
                List<CsPoste> _LstItem = new List<CsPoste>();
                _LstItem = db.RetourneListePoste();
                return _LstItem;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }




        #endregion


    }
}
