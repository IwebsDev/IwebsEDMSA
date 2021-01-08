using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Collections;
using Inova.Tools.Utilities;
//
using Galatee.Structure  ;
using System.Globalization;
using System.IO;
using Galatee.Entity.Model;
using System.Threading.Tasks;


namespace Galatee.DataAccess
{
    public class DBEncaissement
    {

        List<COPER> Copers = new List<COPER>();
        List<LIBELLETOP> Tops = new List<LIBELLETOP>();
        List<MODEREG> Moderegs = new List<MODEREG>();
        List<CENTRE> Centres = new List<CENTRE>();
        List<CAISSE> Caisses = new List<CAISSE>();
        List<ADMUTILISATEUR> AdmUsers = new List<ADMUTILISATEUR>();

        public DBEncaissement()
        {
            try
            {
                ConnectionString = Session.GetSqlConnexionString();
            }
            catch (Exception)
            {
                
                throw;
            }
        }
        private string ConnectionString;
        private SqlCommand cmd = null;
        private SqlConnection cn = null;
        public List<CsRegCli> ChargerListeCodeRegroupement()
        {
            try
            {
                DataTable dt = Galatee.Entity.Model.CommonProcedures.RetourneTousCodeRegroupement();
                return Entities.GetEntityListFromQuery<CsRegCli >(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CsNature > RetourneNature()
        {
            try
            {
                DataTable dt = Galatee.Entity.Model.CommonProcedures.RetourneNature();
                return Entities.GetEntityListFromQuery<CsNature>(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CsPayeur> ChargerListePayeur()
        {
            try
            {
                DataTable dt = Galatee.Entity.Model.CommonProcedures.RetourneTousPayeur();
                return Entities.GetEntityListFromQuery<CsPayeur>(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public string RetourneNumCaisse(string MatriculeConnecte)
        {
            cn = new SqlConnection(ConnectionString);

            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandTimeout = 360;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SPX_ENC_RETOURNENUMCAISSE";
            cmd.Parameters.Add("@MatriculeAgent", SqlDbType.VarChar).Value = MatriculeConnecte;


            try
            {

                if (cn.State == ConnectionState.Closed)
                    cn.Open();
                string Numcaisse = string.Empty;
                SqlDataReader reader = cmd.ExecuteReader();
                foreach (object item in reader)
                {
                    Numcaisse = (Convert.IsDBNull(reader["NumCaisse"])) ? String.Empty : (System.String)reader["NumCaisse"];
                }
                reader.Close();
                return Numcaisse;
            }
            catch (Exception ex)
            {
                throw new Exception(cmd.CommandText  + ":" + ex.Message);
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                    cn.Close(); // Fermeture de la connection 
                cmd.Dispose();
            }
        }
        public bool IsCaisseCloturee(string NumCaisse)
        {
            cn = new SqlConnection(ConnectionString);

            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandTimeout = 360;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SPX_ENC_CAISSECLOTURE";
            cmd.Parameters.Add("@NumCaisse", SqlDbType.VarChar).Value = NumCaisse;

            try
            {

                if (cn.State == ConnectionState.Closed)
                    cn.Open();
                int resultat = 0;
                SqlDataReader reader = cmd.ExecuteReader();
                foreach (object item in reader)
                {
                    resultat = (Convert.IsDBNull(reader["Nombre"])) ? 0 : (System.Int32 )reader["Nombre"];
                }
                reader.Close();
                if (resultat != 0) return true ;
                else return false ;
            }
            catch (Exception ex)
            {
                throw new Exception(cmd.CommandText  + ":" + ex.Message);
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                    cn.Close(); // Fermeture de la connection 
                cmd.Dispose();
            }
        }
        public bool IsCaisseNonCloturee(string NumCaisse)
        {
            cn = new SqlConnection(ConnectionString);

            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandTimeout = 360;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SPX_ENC_NONCAISSECLOTURE";
            cmd.Parameters.Add("@NumCaisse", SqlDbType.VarChar).Value = NumCaisse;

            try
            {

                if (cn.State == ConnectionState.Closed)
                    cn.Open();
                int resultat = 0;
                SqlDataReader reader = cmd.ExecuteReader();
                foreach (object item in reader)
                {
                    resultat = (Convert.IsDBNull(reader["Nombre"])) ? 0 : (System.Int32)reader["Nombre"];
                }
                reader.Close();
                if (resultat != 0) return true;
                else return false;
            }
            catch (Exception ex)
            {
                throw new Exception(cmd.CommandText  + ":" + ex.Message);
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                    cn.Close(); // Fermeture de la connection 
                cmd.Dispose();
            }
        }
        public int RetourneEtatCaisse(string  matricule, int? Fk_IdHabilCaisse)
        {
            //cmd.CommandText = "SPX_ENC_VERIFIECAISSE";

            try
            {
                return FonctionCaisse.RetourneEtatCaisse(matricule,Fk_IdHabilCaisse);
                //if (reader.Read()) resultat = int.Parse(reader["RESULTAT"].ToString());   
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //public string VerifieEtatCaisse(string matricule, int? Fk_IdHabilCaisse)
        //{
        //    int _resultatRequete = RetourneEtatCaisse(matricule, Fk_IdHabilCaisse);
        //    switch (_resultatRequete)
        //    {
        //        case 0:
        //            return Enumere.EtatDeCaisseValider;
        //        case 1:
        //            return Enumere.EtatDeCaissePasCassier;
        //        case 2:
        //            return Enumere.EtatDeCaisseDejaCloture;
        //        case 3:
        //            return Enumere.EtatDeCaisseNonCloture;
        //        case 4:
        //            return Enumere.EtatDeCaisseOuverteALaDemande ;
        //        default :
        //            break;
        //    }
        //    return string.Empty;
     
        //}
        List<TRANSCAISB> ObtenirTransCaissB(List<TRANSCAISSE> transc)
        {
            string RefClient =string.Empty ;
            try
            {
                galadbEntities leContext = new galadbEntities();
                List<TRANSCAISB> transcBList = new List<TRANSCAISB>();
                foreach (var t in transc)
                {
                    RefClient = t.CENTRE + t.CLIENT + t.ORDRE ;
                    TRANSCAISB transB = new TRANSCAISB();
                    transB.ACQUIT = t.ACQUIT;
                    transB.CENTRE = t.CENTRE;
                    transB.CAISSE = t.CAISSE;
                    transB.CLIENT = t.CLIENT;
                    transB.ORDRE = t.ORDRE;
                    transB.MATRICULE = t.MATRICULE;
                    transB.NDOC = t.NDOC;
                    transB.REFEM = t.REFEM;
                    transB.MONTANT = t.MONTANT;
                    transB.DC = t.DC;
                    transB.COPER = t.COPER;
                    transB.PERCU = t.PERCU;
                    transB.RENDU = t.RENDU;
                    transB.MODEREG = t.MODEREG;
                    transB.PLACE = t.PLACE;
                    transB.DTRANS = t.DTRANS;
                    transB.DEXIG = t.DEXIG;
                    transB.BANQUE = t.BANQUE;
                    transB.GUICHET = t.GUICHET;
                    transB.ORIGINE = t.ORIGINE;
                    transB.ECART = t.ECART;
                    transB.TOPANNUL = t.TOPANNUL;
                    transB.MOISCOMPT = t.MOISCOMPT;
                    transB.TOP1 = t.TOP1;
                    transB.TOURNEE = t.TOURNEE;
                    transB.NUMDEM = t.NUMDEM;
                    transB.NUMCHEQ = t.NUMCHEQ;
                    transB.SAISIPAR = t.SAISIPAR;
                    transB.DATEENCAISSEMENT = t.DATEENCAISSEMENT;
                    transB.DATECREATION = t.DATECREATION;
                    transB.USERCREATION = t.USERCREATION;
                    transB.DATEVALEUR = t.DATEVALEUR;
                    transB.CANCELLATION = t.CANCELLATION;

                    // valuer les foreign key
                    transB.FK_IDHABILITATIONCAISSE = t.FK_IDHABILITATIONCAISSE;
                    transB.FK_IDCENTRE = t.FK_IDCENTRE;
                    transB.FK_IDCOPER = t.FK_IDCOPER;
                    transB.FK_IDLIBELLETOP = t.FK_IDLIBELLETOP;
                    transB.FK_IDMODEREG = t.FK_IDMODEREG;
                    transB.FK_IDCAISSIERE = t.FK_IDCAISSIERE;
                    transB.FK_IDAGENTSAISIE = t.FK_IDAGENTSAISIE;
                    transB.FK_IDLCLIENT = t.FK_IDLCLIENT;
                    //CaisseProcedures.ValuerForeignKey(transB);

                    transcBList.Add(transB);
                }

                return transcBList;
            }
            catch (Exception ex)
            {
                string refcli = RefClient;
                throw ex;
            }
        }
        public List<CsCaisse> ChargerCaisseDisponible()
        {
            try
            {
                //DataTable dt = new DataTable();
                DataTable dt = Galatee.Entity.Model.CommonProcedures.ChargerCaisseDisponible();
                return Entities.GetEntityListFromQuery<CsCaisse>(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CsHabilitationCaisse> RetourneListeCaisseHabilite(List<int> LsiteCentreCaisse)
        {
            try
            {
                DataTable dt = CaisseProcedures.RetourneListeCaisseHabilite(LsiteCentreCaisse);
                return Entities.GetEntityListFromQuery<CsHabilitationCaisse>(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CsHabilitationCaisse> RetourneCaisseNonCloture(List<int> LsiteCentreCaisse)
        {
            try
            {
                DataTable dt = CaisseProcedures.RetourneCaisseNonCloture(LsiteCentreCaisse);
                return Entities.GetEntityListFromQuery<CsHabilitationCaisse>(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public CsHabilitationCaisse RetourneCaisseEnCours(int IdNumCaisse, int IdCaissier, DateTime DateDebut)
        //{
        //    try
        //    {
        //        DataTable dt = CaisseProcedures.RetourneCaisseEnCours(IdNumCaisse, IdCaissier, DateDebut);
        //        return Entities.GetEntityFromQuery<CsHabilitationCaisse>(dt);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        public List<CsHabilitationCaisse> RetourneCaisseCloture(List<int> LsiteCentreCaisse)
        {
            try
            {
                DataTable dt = CaisseProcedures.RetourneCaisseCloture(LsiteCentreCaisse);
                return Entities.GetEntityListFromQuery<CsHabilitationCaisse>(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CsLclient > RetourneEncaissementPourValidationAnnulation(List<int> LsiteCentreCaisse)
        {
            try
            {
                DataTable dt = CaisseProcedures.RetourneEncaissementPourValidationAnnulation(LsiteCentreCaisse);
                return Entities.GetEntityListFromQuery<CsLclient >(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CParametre > ListeCaisse()
        {
            //cmd.CommandText = "SPX_ENC_LISTECAISSE";

            try
            {
                //DataTable dt = CommonProcedures.RetourneTousCaissiere();
                //return Entities.GetEntityListFromQuery<CParametre>(dt);
                return new List<CParametre>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool VerifieFondDeCaisse(string NumCaisse)
        {
            return true;
        }

        public List<CsClient> TestClientExistByIdRegroupement(List<int> lstIdRegroupement)
        {
            //cmd.CommandText = "SPX_ENC_RETOURNECLIENT";

            try
            {
                //rows = FillClient(reader,centre ,client ,ordre );
                DataTable dt = Galatee.Entity.Model.CaisseProcedures.RetourneClientByIdReg(lstIdRegroupement);
                List<CsClient> _Lesclient = Entities.GetEntityListFromQuery <CsClient>(dt);
                //_Lesclient.ForEach(t => t.SOLDE = Galatee.Entity.Model.FonctionCaisse.RetourneSoldeClient(t.FK_IDCENTRE ,t.CENTRE ,t.REFCLIENT ,t.ORDRE ));


                return _Lesclient;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CsClient> TestClientExist(string centre,string client,string ordre)
        {
            try
            {
                return RetourneClient(centre,client,ordre);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public List<CsClient> RetourneClientsParReference(string route, string sens )
        {

            try
            {

                return new DBAccueil().Select_client(null, string.Empty, route, string.Empty);
                //DataTable dt = CommonProcedures.RetourneClientsParReference(route, sens );
                //return Entities.GetEntityListFromQuery<CsClient>(dt);
            }
            catch (Exception ex)
            {
                //ex.Message="In DBEncaissement=>RetourneClientsParReference this error occurred :"+ex.Message;
                throw ex;
            }
            finally
            {

            }

        }

        public List<CsClient> RetourneClientsParAmount(decimal amount, string sens, List<int> lstIdCentre)
        {

            try
            {
                DataTable dt = CommonProcedures.RetourneClientsParAmount(amount,sens,lstIdCentre);
                var listclient= Entities.GetEntityListFromQuery<CsClient>(dt);
                return listclient;
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {

            }
            //cn = new SqlConnection(ConnectionString);

            //cmd = new SqlCommand();
            //cmd.Connection = cn;
            //cmd.CommandTimeout = 360;
            //cmd.CommandType = CommandType.StoredProcedure;
            //cmd.CommandText = "SPX_GEN_SEARCHCLIENTSBYAMOUNT";
            //cmd.Parameters.Add("@AMOUNT", SqlDbType.Decimal).Value = amount;
            //cmd.Parameters.Add("@SENS_COMP", SqlDbType.Int).Value = int.Parse(sens);

            //try
            //{
            //    if (cn.State == ConnectionState.Closed)
            //        cn.Open();

            //    SqlDataReader reader = cmd.ExecuteReader();

            //    List<CsClient> rows = new List<CsClient>();
            //    FillClientAmount(reader, rows);
            //    reader.Close();
            //    return rows;
            //}
            //catch (Exception ex)
            //{
            //    throw new Exception(cmd.CommandText + ":" + ex.Message);
            //}
            //finally
            //{
            //    if (cn.State == ConnectionState.Open)
            //        cn.Close(); // Fermeture de la connection 
            //    cmd.Dispose();
            //}
        }

        public List<CsClient> RetourneClientsParNoms(string names)
        {
            try
            {
                DataTable dt = CommonProcedures.RetourneClientsParNoms(names);
                return Entities.GetEntityListFromQuery<CsClient>(dt);
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {

            }
        }

        public bool TestCentreEncaissable(string Centre)
        {
            return true;
        }
        List<CsLclient> RetourneLigneDesFactureDues(List<LCLIENT> facture)
        {
            try
            {
                List<LCLIENT> client = new List<LCLIENT>();
                List<LCLIENT> tempon = new List<LCLIENT>();
                foreach (LCLIENT cl in facture)
                {
                    if (tempon.FirstOrDefault(t => t.NDOC == cl.NDOC && t.REFEM == cl.REFEM) == null)
                    {
                        decimal? solde = FonctionCaisse.RetourneSoldeDocument(cl.FK_IDCENTRE, cl.CENTRE, cl.CLIENT, cl.ORDRE, cl.NDOC, cl.REFEM);
                        if (solde != 0)
                        {
                            cl.TAXESADEDUIRE = solde;
                            client.Add(cl);
                        }
                        tempon.Add(cl);
                    }
                }
                //var nature = ctontext.NATURE;
                IEnumerable<CsLclient> query = (from f in client
                                                select new CsLclient
                                                {
                                                    CENTRE = f.CENTRE,
                                                    CLIENT = f.CLIENT,
                                                    ORDRE = f.ORDRE,
                                                    NDOC = f.NDOC,
                                                    REFEM = f.REFEM,
                                                    COPER = f.COPER,
                                                    DENR = f.DENR,
                                                    DC = f.DC,
                                                    LIBELLECOPER = f.COPER1.LIBELLE,
                                                    LIBELLENATURE = f.COPER1.LIBCOURT,
                                                    EXIGIBILITE = f.EXIGIBILITE,
                                                    SOLDEFACTURE = f.TAXESADEDUIRE.Value,
                                                    MONTANT = f.MONTANT.Value,
                                                    MONTANTTVA = f.MONTANTTVA ,
                                                    CRET = f.CRET,
                                                    ACQUIT = f.ACQUIT,
                                                    MOISCOMPT = f.MOISCOMPT,
                                                    FK_IDCLIENT = f.FK_IDCLIENT  ,
                                                    FK_IDCENTRE = f.FK_IDCENTRE , 
                                                    FK_IDLCLIENT = f.PK_ID 
                                                }).Distinct();
                

                return query.ToList() ;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CsLclient> RetourneListeFactureNonSoldeByRegroupementspx(List<int> LstIdReg, List<string> lstPeriode)
        {
            try
            {
                List<CsLclient> ListeFacture = new List<CsLclient>();
                List<CsLclient> ListeDesFacture = new List<CsLclient>();
                foreach (string item in lstPeriode)
                {
                    DataTable dt = RetourneListeFactureNonSoldeRegrouperSpx(LstIdReg.First(), item);
                    ListeFacture.AddRange(Tools.Utility.GetEntityListFromQuery<CsLclient>(dt));
                }
                return ListeDesFacture;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CsLclient> RetourneListeFactureNonSoldeByRegroupementspx(int IdReg, string Periode)
        {
            try
            {
                DataTable dt = RetourneListeFactureNonSoldeRegrouperSpx(IdReg, Periode);
                return Tools.Utility.GetEntityListFromQuery<CsLclient>(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CsLclient> RetourneListeFactureNonSoldeByRegroupementProduitspx(int IdReg, string Periode,int IdProduit)
        {
            try
            {
                DataTable dt = RetourneListeFactureNonSoldeRegrouperProduitSpx(IdReg, Periode, IdProduit);
                return Tools.Utility.GetEntityListFromQuery<CsLclient>(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CsLclient> RetourneListeFactureNonSoldeByRegroupement(List<int> LstIdReg, List<string> lstPeriode)
        {
            try
            {
                List<CsLclient> ListeFacture = new List<CsLclient>();
                List<CsLclient> ListeDesFacture = new List<CsLclient>();
                //DataTable dt = CaisseProcedures.LigneFactureClient(LstIdReg, lstPeriode);
                //ListeFacture = Galatee.Tools.Utility.GetEntityListFromQuery<CsLclient>(dt);
                ListeFacture = RetourneFactureClientRegroupe(LstIdReg, lstPeriode);
                if (ListeFacture.Count == 0) return null;
                foreach (CsLclient item in ListeFacture.Where(o=>o.SOLDEFACTURE >0))
                {
                        item.MONTANTPAYPARTIEL = item.MONTANT - item.SOLDEFACTURE;
                        ListeDesFacture.Add(item);
                }
                //foreach (CsLclient item in ListeFacture)
                //{
                //    decimal? SoldeFacture = FonctionCaisse.RetourneSoldeDocument(item.FK_IDCENTRE, item.CENTRE, item.CLIENT, item.ORDRE, item.NDOC, item.REFEM);
                //    if (SoldeFacture > 0)
                //    {
                //        item.SOLDEFACTURE = SoldeFacture;
                //        item.MONTANTPAYPARTIEL = item.MONTANT - item.SOLDEFACTURE;
                //        item.MONTANTTVA = item.MONTANTTVA;
                //        ListeDesFacture.Add(item);
                //    }
                //}
                return ListeDesFacture;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CsLclient> RetourneListeFactureNonSoldeByRegroupementId(List<int> LstIdReg)
        {
            try
            {
                List<CsLclient> ListeFacture = new List<CsLclient>();
                List<CsLclient> ListeDesFacture = new List<CsLclient>();
                DataTable dt = CaisseProcedures.LigneFactureClient(LstIdReg);
                ListeFacture = Galatee.Tools.Utility.GetEntityListFromQuery<CsLclient>(dt);
                if (ListeFacture.Count == 0) return null;
                foreach (CsLclient item in ListeFacture)
                {
                    decimal? SoldeFacture = FonctionCaisse.RetourneSoldeDocument(item.FK_IDCENTRE, item.CENTRE, item.CLIENT, item.ORDRE, item.NDOC, item.REFEM);
                    if (SoldeFacture > 0)
                    {
                        item.SOLDEFACTURE = SoldeFacture;
                        item.MONTANTPAYPARTIEL = item.MONTANT - item.SOLDEFACTURE;
                        item.MONTANTTVA = item.MONTANTTVA;
                        ListeDesFacture.Add(item);
                    }
                }
                return ListeDesFacture;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CsClient> RetourneClientFromFacture(List<CsLclient> ListeFactureAregle)
        {
            try
            {
                List<CsClient> lstClientFacture = new List<CsClient>();
                if (ListeFactureAregle.Count > 0)
                {
                    var lstClientFactureDistnct = ListeFactureAregle.Select(t => new {t.FK_IDCENTRE , t.CENTRE, t.CLIENT, t.ORDRE, t.FK_IDCLIENT }).Distinct().ToList();
                    foreach (var item in lstClientFactureDistnct)
                        lstClientFacture.Add(new CsClient {FK_IDCENTRE= item.FK_IDCENTRE , CENTRE = item.CENTRE, REFCLIENT = item.CLIENT, ORDRE = item.ORDRE, PK_ID = item.FK_IDCLIENT });

                }
                return lstClientFacture;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<CsLclient> RetourneListeFactureMoratoire(string centre, string client, string ordre, int foreignkey)
        {
            try
            {
                List<CsLclient> ListeFacture = new List<CsLclient>();
                ListeFacture = RetourneListeFactureNonSolde(centre, client, ordre, foreignkey);
                return ListeFacture.Where(t => string.IsNullOrEmpty(t.CRET)).ToList();
            }
            catch (Exception es)
            {
                
                throw es;
            }

        }

        public List<CsLclient > RetourneListeFacturePayeur(CsPayeur lePayeur)
        {
            try
            {
                //List<LCLIENT> dt = Galatee.Entity.Model.CaisseProcedures.ChargerListeFacturePayeur(lePayeur.PK_ID);
                //return  ChargerLigneFacture(dt);
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CsLclient> RetourneListeFactureReg(int _CodeReg)
        {
            try
            {
                List<CsClient> _LstClientDuCodeRegroupement = RetourneListeDesClientsRegroupe(_CodeReg);
                List<CsLclient> _ListeDesFactureClient = new List<CsLclient>();
                Parallel.ForEach(_LstClientDuCodeRegroupement, _LeClient =>
                  {
                      _ListeDesFactureClient.AddRange(RetourneListeFactureNonSolde(_LeClient.CENTRE, _LeClient.REFCLIENT, _LeClient.ORDRE, _LeClient.PK_ID));

                  });
                return _ListeDesFactureClient;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public  LCLIENT RetourneFactureNaf(CsLclient  FactureAregle)
        {
            try
            {
                LCLIENT laFactureNaf = new LCLIENT();
                using (galadbEntities ctx = new galadbEntities())
                {
                    laFactureNaf = Entities.ConvertObject<LCLIENT, CsLclient>(FactureAregle);
                    laFactureNaf.COPER = Enumere.CoperFPA;
                    laFactureNaf.DC = Enumere.Debit;
                    laFactureNaf.MONTANT = 0;
                    laFactureNaf.EXIGIBILITE = System.DateTime.Today.AddDays(30);
                    laFactureNaf.EXIG = 30;
                    laFactureNaf.DENR = DateTime.Today.Date ;
                    laFactureNaf.NDOC = FactureAregle.NDOC ;
                    laFactureNaf.USERCREATION = FactureAregle.USERCREATION;
                    laFactureNaf.DATECREATION = DateTime.Now;
                    laFactureNaf.DATEMODIFICATION  = DateTime.Now;
                    laFactureNaf.USERMODIFICATION = FactureAregle.USERMODIFICATION;
                    laFactureNaf.FK_IDCOPER = ctx.COPER.First(t => t.CODE == Enumere.CoperFPA).PK_ID;
                    laFactureNaf.FK_IDMOTIFCHEQUEINPAYE  =null ;
                    return laFactureNaf;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public  LCLIENT RetourneFactureDemande(CsLclient FactureAregle)
        {
            try
            {
                LCLIENT laFactureDemande = new LCLIENT();
                laFactureDemande.CENTRE = FactureAregle.CENTRE;
                laFactureDemande.CLIENT = FactureAregle.CLIENT;
                laFactureDemande.ORDRE = FactureAregle.ORDRE;
                laFactureDemande.REFEM = FactureAregle.REFEM;
                laFactureDemande.NDOC = FactureAregle.NDOC;
                laFactureDemande.COPER = FactureAregle.COPER;
                laFactureDemande.DENR = FactureAregle.DENR;
                laFactureDemande.EXIG = FactureAregle.EXIG;
                laFactureDemande.MONTANT = FactureAregle.MONTANT;
                laFactureDemande.CAPUR = FactureAregle.CAPUR;
                laFactureDemande.CRET = FactureAregle.CRET;
                laFactureDemande.MODEREG = FactureAregle.MODEREG;
                laFactureDemande.DC = FactureAregle.DC;
                laFactureDemande.ORIGINE = FactureAregle.ORIGINE;
                laFactureDemande.CAISSE = FactureAregle.CAISSE;
                laFactureDemande.ECART = FactureAregle.ECART;
                laFactureDemande.MOISCOMPT = FactureAregle.MOISCOMPT;
                laFactureDemande.TOP1 = FactureAregle.TOP1;
                laFactureDemande.EXIGIBILITE = FactureAregle.EXIGIBILITE;
                laFactureDemande.FRAISDERETARD = FactureAregle.FRAISDERETARD;
                laFactureDemande.REFERENCEPUPITRE = FactureAregle.REFERENCEPUPITRE;
                laFactureDemande.IDLOT = FactureAregle.IDLOT;
                laFactureDemande.DATEVALEUR = FactureAregle.DATEVALEUR;
                laFactureDemande.REFERENCE = FactureAregle.REFERENCE;
                laFactureDemande.REFEMNDOC = FactureAregle.REFEMNDOC;
                laFactureDemande.ACQUIT = FactureAregle.ACQUIT;
                laFactureDemande.MATRICULE = FactureAregle.MATRICULE;
                laFactureDemande.TAXESADEDUIRE = FactureAregle.TAXESADEDUIRE;
                laFactureDemande.DATEFLAG = FactureAregle.DATEFLAG;
                laFactureDemande.MONTANTTVA = FactureAregle.MONTANTTVA;
                laFactureDemande.IDCOUPURE = FactureAregle.IDCOUPURE;
                laFactureDemande.AGENT_COUPURE = FactureAregle.AGENT_COUPURE;
                laFactureDemande.RDV_COUPURE = FactureAregle.RDV_COUPURE;
                laFactureDemande.NUMCHEQ = FactureAregle.NUMCHEQ;
                laFactureDemande.OBSERVATION_COUPURE = FactureAregle.OBSERVATION_COUPURE;
                laFactureDemande.USERCREATION = FactureAregle.USERCREATION;
                laFactureDemande.DATECREATION = FactureAregle.DATECREATION;
                laFactureDemande.DATEMODIFICATION = FactureAregle.DATEMODIFICATION;
                laFactureDemande.USERMODIFICATION = FactureAregle.USERMODIFICATION;
                laFactureDemande.BANQUE = FactureAregle.BANQUE;
                laFactureDemande.GUICHET = FactureAregle.GUICHET;
                laFactureDemande.FK_IDCENTRE = FactureAregle.FK_IDCENTRE;
                laFactureDemande.FK_IDADMUTILISATEUR = FactureAregle.FK_IDADMUTILISATEUR;
                laFactureDemande.FK_IDCOPER = FactureAregle.FK_IDCOPER;
                laFactureDemande.FK_IDLIBELLETOP = FactureAregle.FK_IDLIBELLETOP;
                laFactureDemande.FK_IDCLIENT = FactureAregle.FK_IDCLIENT;
                laFactureDemande.POSTE = FactureAregle.POSTE;

                return laFactureDemande;
               
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public LCLIENT RetourneFactureClient(CsLclient FactureAregle)
        {
            try
            {
                galadbEntities ctx = new galadbEntities();
                LCLIENT lfac = ctx.LCLIENT.FirstOrDefault(t => t.CENTRE == FactureAregle.CENTRE && t.CLIENT == FactureAregle.CLIENT &&
                                                             t.ORDRE == FactureAregle.ORDRE && t.REFEM == FactureAregle.REFEM && t.NDOC == FactureAregle.NDOC);

                return lfac;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public LCLIENT RetourneFactureAvance(CsLclient FactureAregle,string coper)
        {
            try
            {
                galadbEntities ctx = new galadbEntities();
                LCLIENT lfac = ctx.LCLIENT.Where (t =>t.FK_IDCENTRE == FactureAregle.FK_IDCENTRE && t.CENTRE == FactureAregle.CENTRE && t.CLIENT == FactureAregle.CLIENT &&
                                                             t.ORDRE == FactureAregle.ORDRE && t.COPER == coper).OrderByDescending(o => o.DENR).FirstOrDefault();

                return lfac;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public LCLIENT RetourneFactureClientTrvx(CsLclient FactureAregle)
        {
            try
            {
                galadbEntities ctx = new galadbEntities();
                LCLIENT lfac = ctx.LCLIENT.FirstOrDefault(t => t.CENTRE == FactureAregle.CENTRE && t.CLIENT == FactureAregle.CLIENT &&
                                                               t.ORDRE == FactureAregle.ORDRE && t.COPER  == Enumere.CoperTRV );

                return lfac;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void InsererTranscaisse(CsLclient FactureAregle)
        {
            try
            {
                TRANSCAISSE transcaiss = Entities.ConvertObject<TRANSCAISSE, CsLclient>(FactureAregle);
                Entities.InsertEntity<TRANSCAISSE>(transcaiss);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void InsererTranscaisse(CsLclient FactureAregle, galadbEntities cmd)
        {
            try
            {
                TRANSCAISSE transcaiss = Entities.ConvertObject<TRANSCAISSE, CsLclient>(FactureAregle);
                Entities.InsertEntity<TRANSCAISSE>(transcaiss, cmd);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void InsererTranscaisB(CsLclient FactureAregle, galadbEntities cmd)
        {
            try
            {
                TRANSCAISB transcaiss = Entities.ConvertObject<TRANSCAISB, CsLclient>(FactureAregle);
                Entities.InsertEntity<TRANSCAISB>(transcaiss, cmd);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void  InsererLclient(CsLclient  FactureAregle, galadbEntities cmd)
        {
            try
            {
                LCLIENT Lclient = Entities.ConvertObject<LCLIENT, CsLclient>(FactureAregle);
                Entities.InsertEntity<LCLIENT>(Lclient, cmd);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
  

        public void UpdateLclient(CsLclient  FactureAregle, galadbEntities cmd)
        {
            try
            {


                LCLIENT Lclient = Entities.ConvertObject<LCLIENT, CsLclient>(FactureAregle);
                Entities.UpdateEntity<LCLIENT>(Lclient, cmd);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        //public bool InsererEncaissement(List<CsLclient> LstDesReglementAInserer, string Operation)
        //{
        //    try
        //    {
        //        string demnde = string.Empty;
        //        string Devis = string.Empty;
        //        bool  IsPrestationSeulement = false ;
               
        //        int IdLclientTimbre = 0;
        //        if (!string.IsNullOrEmpty(LstDesReglementAInserer[0].NUMDEM)) demnde = LstDesReglementAInserer[0].NUMDEM;
        //        if (LstDesReglementAInserer[0].ISPRESTATIONSEULEMENT) IsPrestationSeulement = LstDesReglementAInserer[0].ISPRESTATIONSEULEMENT ;
                
        //        List< TRANSCAISSE> LstTranscaisse = new List< TRANSCAISSE>();
        //        using (galadbEntities context = new galadbEntities())
        //        {
        //            string acquitNAF = string.Empty;
        //            bool isTimbreSaisi = false;
        //            List<COPER> LesCoper = context.COPER.ToList();
        //            //Pour chaque reglement à insérer
        //            foreach (CsLclient  FactureAregle in LstDesReglementAInserer)
        //            {
        //                LCLIENT Lclient = new LCLIENT();
        //                if (!string.IsNullOrEmpty(demnde))
        //                    FactureAregle.NUMDEM = demnde;

        //                if (FactureAregle.COPER == Enumere.CoperNAF || FactureAregle.IsPAIEMENTANTICIPE)
        //                {
        //                    List<TRANSCAISSE> transcais = new List<TRANSCAISSE>();
        //                    TRANSCAISSE transcaiss = Entities.ConvertObject<TRANSCAISSE, CsLclient>(FactureAregle);
        //                    LCLIENT laFactureNaf = new LCLIENT();
        //                    if (FactureAregle.IsPAIEMENTANTICIPE && FactureAregle.COPER != Enumere.CoperTimbre)
        //                    {
        //                        acquitNAF = RetourneNumFactureNaf(LstDesReglementAInserer.First().FK_IDCENTRE);
        //                        transcaiss.NDOC = acquitNAF;
        //                        FactureAregle.NDOC = acquitNAF;
        //                        laFactureNaf = RetourneFactureNaf(FactureAregle);
        //                        transcais.Add(transcaiss);
        //                        laFactureNaf.TRANSCAISSE = transcais;
        //                        Entities.InsertEntity<LCLIENT>(laFactureNaf, context);
        //                    }
        //                    else if (FactureAregle.COPER == Enumere.CoperTimbre)
        //                    {
        //                        transcaiss.NDOC = acquitNAF;
        //                        transcaiss.FK_IDLCLIENT = laFactureNaf.PK_ID;
        //                        Entities.InsertEntity<TRANSCAISSE>(transcaiss, context);
        //                    }
        //                }
        //                else
        //                {
        //                    if (!string.IsNullOrEmpty(FactureAregle.NUMDEM) &&
        //                        (FactureAregle.TYPEDEMANDE == Enumere.BranchementAbonement ||
        //                         FactureAregle.TYPEDEMANDE == Enumere.BranchementSimple ||
        //                         FactureAregle.TYPEDEMANDE == Enumere.BranchementAbonementExtention ||
        //                         FactureAregle.TYPEDEMANDE == Enumere.BranchementAbonnementMt  ||
        //                         FactureAregle.TYPEDEMANDE == Enumere.Reabonnement ||
        //                         FactureAregle.TYPEDEMANDE == Enumere.AchatTimbre ||
        //                         FactureAregle.TYPEDEMANDE == Enumere.AbonnementSeul))
        //                    {
        //                        FactureAregle.FK_IDCLIENT = 1;
        //                        TRANSCAISSE transcaiss = Entities.ConvertObject<TRANSCAISSE, CsLclient>(FactureAregle);
        //                        if (FactureAregle.NDOC != "TIMBRE")
        //                        {
        //                            transcaiss.COPER = Enumere.CoperRGT;
        //                            transcaiss.FK_IDCOPER = LesCoper.FirstOrDefault(t => t.CODE == Enumere.CoperRGT).PK_ID;
        //                            Lclient = RetourneFactureDemande(FactureAregle);

        //                            if (FactureAregle.TYPEDEMANDE == Enumere.AchatTimbre)
        //                            {
        //                                transcaiss.MONTANT = transcaiss.MONTANT * (-1);
        //                                transcaiss.DC = Enumere.Credit;
        //                            }
        //                            Lclient.TRANSCAISSE.Add(transcaiss);
        //                            if (!isTimbreSaisi)
        //                            {
        //                                CsLclient laFacture = LstDesReglementAInserer.FirstOrDefault(t => t.NDOC == "TIMBRE");
        //                                if (laFacture != null)
        //                                {
        //                                    TRANSCAISSE transc = Entities.ConvertObject<TRANSCAISSE, CsLclient>(laFacture);
        //                                    Lclient.TRANSCAISSE.Add(transc);
        //                                    isTimbreSaisi = true;
        //                                }
        //                            }
        //                            Entities.InsertEntity<LCLIENT>(Lclient, context);
        //                            IdLclientTimbre = Lclient.PK_ID;
        //                        }
        //                    }
        //                    else if (FactureAregle.TYPEDEMANDE == Enumere.AugmentationPuissance ||
        //                             FactureAregle.TYPEDEMANDE == Enumere.DimunitionPuissance ||
        //                             FactureAregle.TYPEDEMANDE == Enumere.Etalonage ||
        //                             FactureAregle.TYPEDEMANDE == Enumere.RemboursementTrvxNonRealise ||
        //                             FactureAregle.TYPEDEMANDE == Enumere.RemboursementAvance)
        //                    {

        //                        TRANSCAISSE transcaiss = Entities.ConvertObject<TRANSCAISSE, CsLclient>(FactureAregle);
        //                        if (FactureAregle.NDOC != "TIMBRE")
        //                        {
        //                            if (FactureAregle.TYPEDEMANDE == Enumere.RemboursementAvance ||
        //                                FactureAregle.TYPEDEMANDE == Enumere.RemboursementTrvxNonRealise)
        //                            {
        //                                COPER leCoperRemb = context.COPER.FirstOrDefault(t => t.CODE == Enumere.CoperRGT);
        //                                transcaiss.MONTANT = (-1) * transcaiss.MONTANT;
        //                                transcaiss.COPER = leCoperRemb.CODE;
        //                                transcaiss.DC = Enumere.Credit;
        //                                transcaiss.FK_IDCOPER = leCoperRemb.PK_ID;
        //                                transcaiss.DTRANS = System.DateTime.Today;
        //                                Lclient = RetourneFactureAvance(FactureAregle, Enumere.CoperCAU);
        //                                if (FactureAregle.TYPEDEMANDE == Enumere.RemboursementAvance )
        //                                    LettrageFactureSuiteRemboursement(FactureAregle, context);
        //                            }
        //                            if ( 
        //                               FactureAregle.TYPEDEMANDE == Enumere.Etalonage ||
        //                                FactureAregle.TYPEDEMANDE == Enumere.AugmentationPuissance ||
        //                                FactureAregle.TYPEDEMANDE == Enumere.DimunitionPuissance)
        //                                Lclient = RetourneFactureClient(FactureAregle);

        //                            if (FactureAregle.TYPEDEMANDE == Enumere.RemboursementTrvxNonRealise)
        //                                Lclient = RetourneFactureClientTrvx(FactureAregle);

        //                            if (Lclient != null)
        //                                transcaiss.FK_IDLCLIENT = Lclient.PK_ID;
        //                            else
        //                                transcaiss.FK_IDLCLIENT = 1;

        //                            if (!isTimbreSaisi)
        //                            {
        //                                CsLclient laFacture = LstDesReglementAInserer.FirstOrDefault(t => t.NDOC == "TIMBRE");
        //                                if (laFacture != null && string.IsNullOrEmpty(laFacture.REFEM))
        //                                {
        //                                    TRANSCAISSE transc = Entities.ConvertObject<TRANSCAISSE, CsLclient>(laFacture);
        //                                    transc.FK_IDLCLIENT = Lclient.PK_ID;
        //                                    isTimbreSaisi = true;
        //                                    Entities.InsertEntity<TRANSCAISSE>(transc, context);
        //                                }
        //                            }
        //                            Entities.InsertEntity<TRANSCAISSE>(transcaiss, context);
        //                        }
        //                    }
        //                    else
        //                    {
        //                        TRANSCAISSE leEncaissement = context.TRANSCAISSE.FirstOrDefault(t => t.FK_IDCENTRE ==t.FK_IDCENTRE && t.CENTRE == FactureAregle.CENTRE && t.CLIENT == FactureAregle.CLIENT && 
        //                                                                                             t.ORDRE == FactureAregle.ORDRE && t.NDOC == FactureAregle.NDOC && t.REFEM == FactureAregle.REFEM &&
        //                                                                                             t.ACQUIT == FactureAregle.ACQUIT && t.COPER == FactureAregle.COPER && 
        //                                                                                             t.FK_IDHABILITATIONCAISSE == FactureAregle.FK_IDHABILITATIONCAISSE  );
        //                        if (leEncaissement == null )
        //                            InsererTranscaisse(FactureAregle);
        //                    }
        //                }

        //            }
        //            if (!string.IsNullOrEmpty(demnde) && !IsPrestationSeulement)
        //            {
        //                DEMANDE demande = ObtenirDemande(demnde);
        //                if (demande != null)
        //                    MiseAJourDemande(LstDesReglementAInserer[0], demande, context);
        //                MiseAJourRubriqueDemande(LstDesReglementAInserer[0], context);

        //            }
        //            if (!string.IsNullOrEmpty(demnde) && IsPrestationSeulement)
        //                MiseAJourRubriquePrestationDemande(LstDesReglementAInserer[0], context);

        //            MiseAJourMoratoire(LstDesReglementAInserer.Where(t => t.FK_IDMORATOIRE != 0 && t.FK_IDMORATOIRE != null).ToList(), context);

        //            // Incrementer le dernier numero d acquit du 
        //            UpdateAcquit(LstDesReglementAInserer[0].FK_IDHABILITATIONCAISSE.Value  , LstDesReglementAInserer[0].MATRICULE , LstDesReglementAInserer[0].ACQUIT, context);
        //            context.SaveChanges();
        //            return true;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}


        public bool EncaissementFacture(List<CsLclient> LstDesReglementAInserer)
        {
            try
            {
                List<TRANSCAISSE> LstTranscaisse = new List<TRANSCAISSE>();
                using (galadbEntities context = new galadbEntities())
                {
                    string acquitNAF = string.Empty;
                    List<COPER> LesCoper = context.COPER.ToList();
                    //Pour chaque reglement à insérer
                    foreach (CsLclient FactureAregle in LstDesReglementAInserer)
                    {
                        LCLIENT Lclient = new LCLIENT();
                        if (FactureAregle.IsPAIEMENTANTICIPE)
                        {
                            List<TRANSCAISSE> transcais = new List<TRANSCAISSE>();
                            TRANSCAISSE transcaiss = Entities.ConvertObject<TRANSCAISSE, CsLclient>(FactureAregle);
                            LCLIENT laFactureNaf = new LCLIENT();
                            if (FactureAregle.COPER != Enumere.CoperTimbre && FactureAregle.NDOC != "TIMBRE")
                            {
                                acquitNAF = AccueilProcedures.NumeroFacture();
                                transcaiss.NDOC = acquitNAF;
                                FactureAregle.NDOC = acquitNAF;
                                laFactureNaf = RetourneFactureNaf(FactureAregle);
                                Entities.InsertEntity<LCLIENT>(laFactureNaf, context);
                                transcaiss.FK_IDLCLIENT = laFactureNaf.PK_ID;
                                Entities.InsertEntity<TRANSCAISSE>(transcaiss, context);
                            }
                        }
                        else
                        {
                            TRANSCAISSE leEncaissement = context.TRANSCAISSE.FirstOrDefault(t => t.FK_IDCENTRE == t.FK_IDCENTRE && t.CENTRE == FactureAregle.CENTRE && t.CLIENT == FactureAregle.CLIENT &&
                                                                                                       t.ORDRE == FactureAregle.ORDRE && t.NDOC == FactureAregle.NDOC && t.REFEM == FactureAregle.REFEM &&
                                                                                                       t.ACQUIT == FactureAregle.ACQUIT && t.COPER == FactureAregle.COPER &&
                                                                                                       t.FK_IDHABILITATIONCAISSE == FactureAregle.FK_IDHABILITATIONCAISSE);
                            if (leEncaissement == null)
                                InsererTranscaisse(FactureAregle);
                        }
                    }
                    // Incrementer le dernier numero d acquit du 
                    UpdateAcquit(LstDesReglementAInserer[0].FK_IDHABILITATIONCAISSE.Value, LstDesReglementAInserer[0].MATRICULE, LstDesReglementAInserer[0].ACQUIT, context);
                    context.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool EncaissementDemande(List<CsLclient> LstDesReglementAInserer)
        {
            try
            {
                string demnde = string.Empty;
                string Devis = string.Empty;
                bool IsPrestationSeulement = false;

                int IdLclientTimbre = 0;
                if (!string.IsNullOrEmpty(LstDesReglementAInserer[0].NUMDEM)) demnde = LstDesReglementAInserer[0].NUMDEM;
                if (LstDesReglementAInserer[0].ISPRESTATIONSEULEMENT) IsPrestationSeulement = LstDesReglementAInserer[0].ISPRESTATIONSEULEMENT;

                List<TRANSCAISSE> LstTranscaisse = new List<TRANSCAISSE>();
                using (galadbEntities context = new galadbEntities())
                {
                    string acquitNAF = string.Empty;
                    bool isTimbreSaisi = false;
                    List<COPER> LesCoper = context.COPER.ToList();
                    //Pour chaque reglement à insérer
                    foreach (CsLclient FactureAregle in LstDesReglementAInserer)
                    {
                        LCLIENT Lclient = new LCLIENT();
                        if (!string.IsNullOrEmpty(demnde))
                            FactureAregle.NUMDEM = demnde;
                    
                            if (!string.IsNullOrEmpty(FactureAregle.NUMDEM) &&
                                (FactureAregle.TYPEDEMANDE == Enumere.BranchementAbonement ||
                                 FactureAregle.TYPEDEMANDE == Enumere.BranchementSimple ||
                                 FactureAregle.TYPEDEMANDE == Enumere.BranchementAbonementExtention ||
                                 FactureAregle.TYPEDEMANDE == Enumere.TransfertSiteNonMigre ||
                                 FactureAregle.TYPEDEMANDE == Enumere.TransfertAbonnement  ||
                                 FactureAregle.TYPEDEMANDE == Enumere.BranchementAbonnementMt ||
                                 FactureAregle.TYPEDEMANDE == Enumere.Reabonnement ||
                                 FactureAregle.TYPEDEMANDE == Enumere.AchatTimbre ||
                                 FactureAregle.TYPEDEMANDE == Enumere.AbonnementSeul ||
                                 FactureAregle.TYPEDEMANDE == Enumere.AugmentationPuissance ||
                                 FactureAregle.TYPEDEMANDE == Enumere.ChangementProduit  ||
                                 FactureAregle.TYPEDEMANDE == Enumere.DimunitionPuissance ||
                                 FactureAregle.TYPEDEMANDE == Enumere.Etalonage))
                            {
                                if (FactureAregle.TYPEDEMANDE != Enumere.AugmentationPuissance &&
                                 FactureAregle.TYPEDEMANDE != Enumere.DimunitionPuissance &&
                                 FactureAregle.TYPEDEMANDE != Enumere.Etalonage)
                                    FactureAregle.FK_IDCLIENT = 1;
                                TRANSCAISSE transcaiss = Entities.ConvertObject<TRANSCAISSE, CsLclient>(FactureAregle);
                                if (FactureAregle.NDOC != "TIMBRE")
                                {
                                    transcaiss.COPER = Enumere.CoperRGT;
                                    transcaiss.FK_IDCOPER = LesCoper.FirstOrDefault(t => t.CODE == Enumere.CoperRGT).PK_ID;
                                    Lclient = RetourneFactureDemande(FactureAregle);

                                    if (FactureAregle.TYPEDEMANDE == Enumere.AchatTimbre)
                                    {
                                        transcaiss.MONTANT = transcaiss.MONTANT * (-1);
                                        transcaiss.DC = Enumere.Credit;
                                    }
                                    Lclient.TRANSCAISSE.Add(transcaiss);
                                    if (!isTimbreSaisi)
                                    {
                                        CsLclient laFacture = LstDesReglementAInserer.FirstOrDefault(t => t.NDOC == "TIMBRE");
                                        if (laFacture != null)
                                        {
                                            TRANSCAISSE transc = Entities.ConvertObject<TRANSCAISSE, CsLclient>(laFacture);
                                            Lclient.TRANSCAISSE.Add(transc);
                                            isTimbreSaisi = true;
                                        }
                                    }
                                    Entities.InsertEntity<LCLIENT>(Lclient, context);
                                    IdLclientTimbre = Lclient.PK_ID;
                                }
                            }
                            else if (
                                //FactureAregle.TYPEDEMANDE == Enumere.AugmentationPuissance ||
                                //     FactureAregle.TYPEDEMANDE == Enumere.DimunitionPuissance ||
                                     //FactureAregle.TYPEDEMANDE == Enumere.Etalonage ||
                                     FactureAregle.TYPEDEMANDE == Enumere.RemboursementTrvxNonRealise ||
                                     FactureAregle.TYPEDEMANDE == Enumere.RemboursementAvance)
                            {

                                TRANSCAISSE transcaiss = Entities.ConvertObject<TRANSCAISSE, CsLclient>(FactureAregle);
                                if (FactureAregle.NDOC != "TIMBRE")
                                {
                                    if (FactureAregle.TYPEDEMANDE == Enumere.RemboursementAvance ||
                                        FactureAregle.TYPEDEMANDE == Enumere.RemboursementTrvxNonRealise)
                                    {
                                        COPER leCoperRemb = context.COPER.FirstOrDefault(t => t.CODE == Enumere.CoperRGT);
                                        transcaiss.MONTANT = (-1) * transcaiss.MONTANT;
                                        transcaiss.COPER = leCoperRemb.CODE;
                                        transcaiss.DC = Enumere.Credit;
                                        transcaiss.FK_IDCOPER = leCoperRemb.PK_ID;
                                        transcaiss.DTRANS = System.DateTime.Today;
                                        Lclient = RetourneFactureAvance(FactureAregle, Enumere.CoperCAU);
                                        if (FactureAregle.TYPEDEMANDE == Enumere.RemboursementAvance)
                                            LettrageFactureSuiteRemboursement(FactureAregle, context);
                                    }
                                    if (
                                       FactureAregle.TYPEDEMANDE == Enumere.Etalonage ||
                                        FactureAregle.TYPEDEMANDE == Enumere.AugmentationPuissance ||
                                        FactureAregle.TYPEDEMANDE == Enumere.DimunitionPuissance)
                                        Lclient = RetourneFactureClient(FactureAregle);

                                    if (FactureAregle.TYPEDEMANDE == Enumere.RemboursementTrvxNonRealise)
                                        Lclient = RetourneFactureClientTrvx(FactureAregle);

                                    if (Lclient != null)
                                        transcaiss.FK_IDLCLIENT = Lclient.PK_ID;
                                    else
                                        transcaiss.FK_IDLCLIENT = 1;

                                    if (!isTimbreSaisi)
                                    {
                                        CsLclient laFacture = LstDesReglementAInserer.FirstOrDefault(t => t.NDOC == "TIMBRE");
                                        if (laFacture != null && string.IsNullOrEmpty(laFacture.REFEM))
                                        {
                                            TRANSCAISSE transc = Entities.ConvertObject<TRANSCAISSE, CsLclient>(laFacture);
                                            transc.FK_IDLCLIENT = Lclient.PK_ID;
                                            isTimbreSaisi = true;
                                            Entities.InsertEntity<TRANSCAISSE>(transc, context);
                                        }
                                    }
                                    Entities.InsertEntity<TRANSCAISSE>(transcaiss, context);
                                }
                            }
                            MiseAJourRubriqueDemande(FactureAregle, context);
                    }
                    //if (!string.IsNullOrEmpty(demnde) && !IsPrestationSeulement)
                    if (!string.IsNullOrEmpty(demnde))
                    {
                        DEMANDE demande = ObtenirDemande(demnde);
                        if (demande != null)
                            MiseAJourDemande(LstDesReglementAInserer[0], demande, context);

                    }
                    if (!string.IsNullOrEmpty(demnde) && IsPrestationSeulement)
                        MiseAJourRubriquePrestationDemande(LstDesReglementAInserer[0], context);

                    MiseAJourMoratoire(LstDesReglementAInserer.Where(t => t.FK_IDMORATOIRE != 0 && t.FK_IDMORATOIRE != null).ToList(), context);

                    int resultat = -1;
                    // Incrementer le dernier numero d acquit du 
                    UpdateAcquit(LstDesReglementAInserer[0].FK_IDHABILITATIONCAISSE.Value, LstDesReglementAInserer[0].MATRICULE, LstDesReglementAInserer[0].ACQUIT, context);
                    resultat=  context.SaveChanges();
                    if (resultat>0)
                    TransmettreDemande(LstDesReglementAInserer[0], context);


                    return true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public  void LettrageFactureSuiteRemboursement(CsLclient leClient,galadbEntities context)
        {
            galadbEntities ctx = new galadbEntities();
            COPER leCoper = ctx.COPER.FirstOrDefault(t => t.CODE == Enumere.CoperRGT);
            COPER leCoperRAd = ctx.COPER.FirstOrDefault(t => t.CODE == Enumere.CoperRAD );
            COPER leCoperUdc = ctx.COPER.FirstOrDefault(t => t.CODE == Enumere.CoperUDC );
            LIBELLETOP leTope = ctx.LIBELLETOP.FirstOrDefault(t => t.CODE == Enumere.TopCaisse);
            ADMUTILISATEUR leuser = ctx.ADMUTILISATEUR.FirstOrDefault(t => t.MATRICULE == leClient.MATRICULE);
            List<TRANSCAISB> lstReglement = new List<TRANSCAISB>();
            List<CsLclient> lstFactiure = RetourneListeFactureNonSolde(leClient.CENTRE, leClient.CLIENT, leClient.ORDRE, leClient.FK_IDCLIENT);
            if (lstFactiure != null && lstFactiure.Count != 0)
            {
                foreach (CsLclient item in lstFactiure)
                {
                    TRANSCAISB leRgt = Entities.ConvertObject<TRANSCAISB, CsLclient>(item);
                    leRgt.FK_IDCAISSIERE = null;
                    leRgt.DATECREATION = System.DateTime.Now;
                    leRgt.USERCREATION = leClient.MATRICULE;
                    leRgt.COPER = leCoperUdc.CODE;
                    leRgt.MONTANT  = item.SOLDEFACTURE ;
                    leRgt.FK_IDCOPER = leCoperUdc.PK_ID;
                    leRgt.DTRANS  = System.DateTime.Today;
                    leRgt.ACQUIT =Enumere.AcquitLettrageAuto;
                    lstReglement.Add(leRgt);
                }

                LCLIENT leMtEquilbre = Galatee.Tools.Utility.ConvertEntity<LCLIENT, CsLclient>(leClient);
                leMtEquilbre.COPER = leCoperRAd.CODE;
                leMtEquilbre.DATECREATION = System.DateTime.Now;
                leMtEquilbre.USERCREATION = leClient.MATRICULE;
                leMtEquilbre.MONTANT = lstFactiure.Sum(t=>t.SOLDEFACTURE);
                leMtEquilbre.DENR = System.DateTime.Today;
                leMtEquilbre.TOP1 = Enumere.TopCaisse;
                leMtEquilbre.FK_IDLIBELLETOP = leTope.PK_ID;
                leMtEquilbre.FK_IDADMUTILISATEUR = leuser.PK_ID;
                leMtEquilbre.FK_IDCOPER = leCoperRAd.PK_ID;
                leMtEquilbre.FK_IDMOTIFCHEQUEINPAYE = null;
                leMtEquilbre.FK_IDMORATOIRE = null;
                leMtEquilbre.FK_IDPOSTE = null;
                Entities.InsertEntity<LCLIENT>(leMtEquilbre, context);
                Entities.InsertEntity<TRANSCAISB>(lstReglement, context);                  
                ctx.Dispose();
            }
        }
        private DEMANDE ObtenirDemande(string pNumDemande)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    DEMANDE demande = context.DEMANDE.First(d => d.NUMDEM == pNumDemande);
                    return demande.DCAISSE == null ? demande : null; // verifier si la demande n'a pas fait l'objet d'un encaissement .
                                                // cela empeche à la demande d'etre encaisser plusieurs fois pa idnavertance
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void InsererDeposit(CsLclient  FactureAregle, galadbEntities cmd)
        {
            try
            {
                galadbEntities context = new galadbEntities();
                DEPOSIT deposit = new DEPOSIT()
                {
                    CENTRE = FactureAregle.CENTRE  ,
                    CLIENT = FactureAregle.CLIENT ,
                    ORDRE = FactureAregle.ORDRE ,
                    RECEIPT =  FactureAregle.ACQUIT ,
                    MONTANTDEPOSIT = FactureAregle.MONTANT,
                    NOM = FactureAregle.NOM,
                    USERCREATION = FactureAregle.USERCREATION,
                    USERMODIFICATION = FactureAregle.USERMODIFICATION,
                    DATECREATION = FactureAregle.DATECREATION.Value.Date,
                    DATEMODIFICATION = FactureAregle.DATEMODIFICATION,
                    DATEENC = DateTime.Now.Date,
                    //IDENTITE = FactureAregle.IDENTITY,
                    FK_IDCENTRE = context.CENTRE.FirstOrDefault(t=>t.CODE == FactureAregle.CENTRE).PK_ID 
                };
                Entities.InsertEntity<DEPOSIT>(deposit, cmd);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CsBanque> RetourneListeDesBanques()
        {
            //cmd.CommandText = "SPX_ENC_LISTEBANQUE";

            try
            {
                DataTable dt = Galatee.Entity.Model.CommonProcedures.RetourneTousBanques();
                return Entities.GetEntityListFromQuery<CsBanque>(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string RetourneNumFactureNaf(int? Pkidcentre)
        {
            //cmd.CommandText = "SPX_ENC_NUMFACTURENAF";

            try
            {
                return AccueilProcedures.NumeroFacture(Pkidcentre);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CsClient> RetourneListeDesClientsRegroupe(int IdCodeRegroupe)
        {
            //cmd.CommandText = "SPX_ENC_LISTECLIENTREGROUPE";
            try
            {
                DataTable dt = Galatee.Entity.Model.CaisseProcedures.RetourneListeDesClientsParCodeRegroupemnt(IdCodeRegroupe);
                return Entities.GetEntityListFromQuery<CsClient>(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public List<CsLclient> RetourneListeRecuDeCaisse_EnCour(int Caisse)
        {
            try
            {
                DataTable dt = CaisseProcedures.RetourneListeRecuDeCaisse_EnCour(Caisse);
                return Entities.GetEntityListFromQuery<CsLclient>(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CsHabilitationCaisse> RetourneSuppervisionCaisse(List<int> Centre, DateTime? dateCaisse, bool Isferme)
        {

            //cmd.CommandText = "SPX_ENC_RECUAANNULLE";

            try
            {
                DataTable dt = CaisseProcedures.RetourneSuppervisionCaisse(Centre, dateCaisse);
                List<CsHabilitationCaisse> lstCaisse = Entities.GetEntityListFromQuery<CsHabilitationCaisse>(dt);
                List<CsHabilitationCaisse> lstresultat = new List<CsHabilitationCaisse>();
                if (dateCaisse != null)
                    lstresultat.AddRange(lstCaisse.Where(d => d.DATE_DEBUT == null || d.DATE_DEBUT == dateCaisse));
                else
                    lstresultat = lstCaisse;
                if (Isferme)
                    lstresultat = lstresultat.Where(t => t.DATE_DEBUT  != null && t.DATE_FIN == null ).ToList();
                //Parallel.ForEach(lstCaisse, t =>
                //               { 
                foreach (CsHabilitationCaisse t in lstresultat)
                {
                    if (t.PK_ID == 0) continue;
                    CsHabilitationCaisse laCaisse =   RetourneHabileCaisseReversement(t);
                    t.MONTANTENCAISSE = laCaisse.MONTANTENCAISSE;
                    t.MONTANTREVERSER  = laCaisse.MONTANTREVERSER ;
                }
                               //});
                return lstresultat;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CsReglement> RetourneListeRecuManuelDeCaisse(string Caisse)
        {

            //cmd.CommandText = "SPX_ENC_RECUAANNULLE";

            try
            {
                DataTable dt = CaisseProcedures.RetourneListeDesRecuManuelNonAnnules(Caisse);
                return Entities.GetEntityListFromQuery<CsReglement>(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CsReglement> RetourneListePaiementDuRecu(string caisse, string acquit, string MatriculeConect)
        {


            try
            {
                // 2ieme cas : // on vérifie si le recu provient d'un client existant en base( cad : CLIENT)
                DataTable dt = CaisseProcedures.RetourneListePaiementDuRecuDuReglementNormale(caisse, acquit, MatriculeConect);
                List<CsReglement> reglement =  Entities.GetEntityListFromQuery<CsReglement>(dt);

                if (reglement != null && reglement.Count > 0) // on vérifie si le recu provient d'un client existant en base(cas normal)
                    return reglement;

                // 2ieme cas : // on vérifie si le recu provient d'un devis
                dt = CaisseProcedures.RetourneListePaiementDuRecuDuReglementNormaleOD(caisse, acquit, MatriculeConect);
                List<CsReglement> devis = Entities.GetEntityListFromQuery<CsReglement>(dt);

                if (devis != null && devis.Count > 0)
                    return devis;

                // 3ieme cas : // on vérifie si le recu provient d'une demande de client non existant( cas : DCLIENT)
                dt = CaisseProcedures.RetourneListePaiementDuRecuDuReglementDemande(caisse, acquit, MatriculeConect);
                List<CsReglement> Demande = Entities.GetEntityListFromQuery<CsReglement>(dt);

                if (Demande != null && Demande.Count > 0)
                    return Demande;

                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool? ValiderAnnuleEncaissement(List<CsLclient> ListFactureAnnule)
        {
            SqlCommand laCommande = null;
            try
            {
                laCommande = new SqlCommand();
                laCommande = DBBase.InitTransaction(ConnectionString);
                foreach (CsLclient item in ListFactureAnnule)
                {
                    AnnulerRecu(item.ACQUIT, item.MATRICULE, item.FK_IDHABILITATIONCAISSE.Value, laCommande);
                    if (!string.IsNullOrEmpty(item.NUMDEM))
                        new DBAccueil().UpdateWKF(item.NUMDEM, null, 0, item.MATRICULE, "Annulation caisse", laCommande);
                }
                laCommande.Transaction.Commit();
                return true;
            }
            catch (Exception ex)
            {
                laCommande.Transaction.Rollback();
                throw ex;
            }
        }
        public bool? RejeterAnnuleEncaissement(List<CsLclient> ListFactureAnnule)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    // annulation des recu provenant des reglements normaux( cad autres que DEVIS , DEMANDE ,ETC...)
                    if (ListFactureAnnule.FirstOrDefault(f => !string.IsNullOrEmpty(f.NUMDEM) || !string.IsNullOrEmpty(f.NUMDEVIS)) == null)
                    {
                        foreach (CsLclient item in ListFactureAnnule)
                            RejetAnnulationReglement(item, context);
                        context.SaveChanges();
                        return true;
                    }
                   
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

       public bool? DemandeAnnuleEncaissement(List<CsLclient> ListFactureAnnule)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    foreach (CsLclient   item in ListFactureAnnule)
                        ValiderDemandeAnnulationReglement(item, context);
                    context.SaveChanges();
                    return true;
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

       public bool? RetirerDemandeAnnulationEncaissement(List<CsLclient> ListFactureAnnule)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    // annulation des recu provenant des reglements normaux( cad autres que DEVIS , DEMANDE ,ETC...)
                    if (ListFactureAnnule.FirstOrDefault(f => !string.IsNullOrEmpty(f.NUMDEM) || !string.IsNullOrEmpty(f.NUMDEVIS)) == null)
                    {
                        foreach (CsLclient   item in ListFactureAnnule)
                            ValiderRetraitDemandeAnnulationReglement(item, context);

                        context.SaveChanges();
                        return true;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        
        public static DataTable RetourneListeDesRecuNonAnnules(string pCaisse)
        {
            try
            {
                using (galadbEntities ctontext = new galadbEntities())
                {
                    var transc = ctontext.TRANSCAISSE.Where(t => t.CAISSE == pCaisse).OrderBy(t => t.ACQUIT);
                    var Leclient = ctontext.CLIENT;

                    List<TRANSCAISSE> listeTrans = new List<TRANSCAISSE>();
                    foreach (var t in transc)
                    {
                        if ((t.TOPANNUL != Enumere.CaisseTopAnnulation || string.IsNullOrEmpty(t.TOPANNUL)) &&
                            t.COPER != Enumere.CoperAjsutemenFondCaisse && t.COPER != Enumere.CoperFondCaisse)
                            listeTrans.Add(t);
                    }

                    IEnumerable<object> query = (from t in listeTrans
                                                 select new
                                                 {
                                                     t.CENTRE,
                                                     t.CLIENT,
                                                     t.ORDRE,
                                                     t.CAISSE,
                                                     t.ACQUIT,
                                                     t.MATRICULE,
                                                     t.NDOC,
                                                     t.REFEM,
                                                     t.MONTANT,
                                                     t.DC,
                                                     t.COPER,
                                                     t.PERCU,
                                                     t.RENDU,
                                                     t.MODEREG,
                                                     t.PLACE,
                                                     t.DTRANS,
                                                     t.DEXIG,
                                                     t.BANQUE,
                                                     t.GUICHET,
                                                     t.ORIGINE,
                                                     t.ECART,
                                                     t.TOPANNUL,
                                                     t.MOISCOMPT,
                                                     t.TOP1,
                                                     t.TOURNEE,
                                                     t.DATEVALEUR,
                                                     t.NUMDEM,
                                                     t.NUMCHEQ,
                                                     t.SAISIPAR,
                                                     t.DATEENCAISSEMENT,
                                                     t.CANCELLATION,
                                                     t.USERCREATION,
                                                     t.DATECREATION,
                                                     t.DATEMODIFICATION,
                                                     t.USERMODIFICATION,

                                                     t.PK_ID,
                                                     t.FK_IDCENTRE,
                                                     t.FK_IDCAISSIERE,
                                                     t.FK_IDMODEREG,
                                                     t.FK_IDLIBELLETOP,
                                                     t.FK_IDHABILITATIONCAISSE,
                                                     t.FK_IDCOPER,
                                                     t.FK_IDPOSTECLIENT,
                                                     t.POSTE,
                                                     //t.MOTIFANNULATION,
                                                     REFFERENCEACQUIT = t.CAISSE + t.ACQUIT + t.MATRICULE,
                                                     MONTANTPAYE = t.MONTANT,
                                                     NOMCLIENT = Leclient.FirstOrDefault(c => c.REFCLIENT == t.CLIENT && c.CENTRE == t.CENTRE && c.ORDRE == t.ORDRE).NOMABON,
                                                     //IsDEMANDEANNULATION = t.ISDEMANDEANNULATION
                                                 });


                    return Galatee.Tools.Utility.ListToDataTable<object>(query);

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public List<CsReglement> ValidationDemandeAnnulationEncaissement(List<string > ListCentre)
        //{
        //    try
        //    {
        //        using (galadbEntities context = new galadbEntities())
        //        {
        //            // annulation des recu provenant des reglements normaux( cad autres que DEVIS , DEMANDE ,ETC...)
        //            if (ListFactureAnnule.FirstOrDefault(f => !string.IsNullOrEmpty(f.NUMDEM) || !string.IsNullOrEmpty(f.NUMDEVIS)) == null)
        //            {
        //                ExecuteAnnulationReglement(ListFactureAnnule, context);
        //                context.SaveChanges();
        //                return true;
        //            }

        //            // annulation des devis
        //            if (ListFactureAnnule.FirstOrDefault(f => !string.IsNullOrEmpty(f.NUMDEVIS)) != null)
        //            {
        //                ExecuteAnnulationDevis(ListFactureAnnule, context);
        //                context.SaveChanges();

        //                return true;
        //            }

        //            // annulation des demandes
        //            if (ListFactureAnnule.FirstOrDefault(f => !string.IsNullOrEmpty(f.NUMDEM)) != null)
        //            {
        //                ExecuteAnnulationDemande(ListFactureAnnule, context);
        //                context.SaveChanges();

        //                return true;
        //            }
        //        }
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        private void ExecuteAnnulationOD(List<CsReglement> _ReglementOD, galadbEntities cmd)
        {
            try
            {
                List<TRANSCAISSE> Transcaisse = TranscaisseReglementOD(_ReglementOD);
                if (Transcaisse != null && Transcaisse.Count > 0)
                    Entities.UpdateEntity<TRANSCAISSE>(Transcaisse, cmd);
            }
            catch (Exception ex)
            {
              throw ex;
            }
        }

        List<TRANSCAISSE> TranscaisseReglementOD(List<CsReglement> preglement)
        {
            try
            {
                List<TRANSCAISSE> transcaisseList = new List<TRANSCAISSE>();
                using (galadbEntities context = new galadbEntities())
                {
                    var Transcaisse = context.TRANSCAISSE;
                    var FactureAregle = preglement.First();
                    //foreach (var FactureAregle in preglement)
                    //{
                        var transc = Transcaisse.Where(t =>t.CAISSE == FactureAregle.CAISSE && t.ACQUIT == FactureAregle.ACQUIT);// && 
                                                           //t.COPER != Enumere.CoperSCF || t.COPER != Enumere.CoperOdQPA);
                        if (transc.Count() > 0)
                            foreach (TRANSCAISSE t in transc)
                            {
                                t.CANCELLATION = FactureAregle.CANCELLATION;
                                t.TOPANNUL = Enumere.CaisseTopAnnulation;
                                transcaisseList.Add(t);
                            }
                    //}

                    return transcaisseList;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        List<TRANSCAISSE> TranscaisseAAnnulerDevis(List<CsLclient> preglement)
        {
            try
            {
                List<TRANSCAISSE> transcaisseList = new List<TRANSCAISSE>();
                using (galadbEntities context = new galadbEntities())
                {
                    var Transcaisse = context.TRANSCAISSE;
                    var FactureAregle = preglement.First();
                    var transc = Transcaisse.Where(t => t.CAISSE == FactureAregle.CAISSE && t.ACQUIT == FactureAregle.ACQUIT && t.MATRICULE == FactureAregle.MATRICULE);
                    if (transc.Count() > 0)
                        foreach (TRANSCAISSE t in transc)
                        {
                            t.CANCELLATION = FactureAregle.MOTIFANNULATION ;
                            t.TOPANNUL = Enumere.CaisseTopAnnulation;
                            transcaisseList.Add(t);
                        }
                    return transcaisseList;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        DEMANDE ANNULATIONDEMANDE(string numdemande, galadbEntities cmd)
        {
            try
            {
                    var demande = cmd.DEMANDE.FirstOrDefault(d => d.NUMDEM == numdemande);
                    demande.DCAISSE = null;
                    demande.DATEMODIFICATION = DateTime.Now.Date;
                    return demande;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        List<RUBRIQUEDEMANDE> ANNULATIONRUBRIQUEDEMANDE(List<TRANSCAISSE> encais)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    List<string> numfactEncaisse = encais.Select(t => t.NDOC).ToList();
                    string numdemande = encais.First().NUMDEM;
                    List<RUBRIQUEDEMANDE> demande = context.RUBRIQUEDEMANDE.Where(d => d.NUMDEM == numdemande && numfactEncaisse.Contains(d.NDOC)).ToList();
                    demande.ForEach(t => t.DATECAISSE = null);
                    demande.ForEach(t => t.DATEMODIFICATION = null);
                    return demande;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void ExecuteAnnulationNAF(CsReglement FactureAregle, galadbEntities cmd)
        {

            try
            {
                LCLIENT Lclient = new LCLIENT();
                Lclient.OBSERVATION_COUPURE = FactureAregle.OBSERVATION_COUPURE;
                Lclient.NUMCHEQ = FactureAregle.NUMCHEQUE;
                Lclient.RDV_COUPURE = FactureAregle.RDV_COUPURE;
                Lclient.AGENT_COUPURE  = FactureAregle.AGENT_COUPURE;
                Lclient.IDCOUPURE  = FactureAregle.IDCOUPURE;
                Lclient.MONTANTTVA = FactureAregle.MONTANTTVA;
                Lclient.DATEFLAG = FactureAregle.DATEFLAG;
                Lclient.TAXESADEDUIRE = FactureAregle.TAXESADEDUIRE;
                Lclient.MATRICULE  = FactureAregle.MATRICULE;//
                Lclient.ACQUIT = FactureAregle.ACQUITANTERIEUR;
                Lclient.REFEMNDOC = FactureAregle.REFEMNDOC;
                Lclient.REFERENCE = FactureAregle.REFERENCE;
                Lclient.DATEVALEUR = FactureAregle.DATEFACTURE;
                Lclient.IDLOT  = FactureAregle.IDLOT;
                Lclient.REFERENCEPUPITRE = FactureAregle.REFERENCEPUPITRE;
                Lclient.FRAISDERETARD = FactureAregle.FRAISRETARD;
                Lclient.EXIGIBILITE = FactureAregle.DATEEXIGIBLE;
                Lclient.TOP1  = FactureAregle.TOP1;//
                Lclient.MOISCOMPT = FactureAregle.MOISCOMPT;
                Lclient.ECART = FactureAregle.ECART;
                Lclient.CAISSE  = FactureAregle.CAISSE;
                Lclient.ORIGINE = FactureAregle.ORIGINE;
                Lclient.DC = FactureAregle.DC;
                Lclient.MODEREG  = FactureAregle.MODEREG;
                Lclient.CRET = FactureAregle.CRET;
                Lclient.CAPUR = FactureAregle.CAPUR;
                Lclient.MONTANT = FactureAregle.MONTANTPAYE;
                Lclient.EXIG = 0;
                Lclient.DENR = DateTime.Today.Date;
                Lclient.COPER  = FactureAregle.COPER;//
                Lclient.NDOC = FactureAregle.NDOC;
                Lclient.REFEM = FactureAregle.REFEM;
                Lclient.ORDRE = FactureAregle.ORDRE ;
                Lclient.CENTRE = FactureAregle.CENTRE ;//
                Lclient.CLIENT  = FactureAregle.CLIENT;
                Lclient.USERCREATION = FactureAregle.USERCREATION;
                Lclient.DATECREATION = FactureAregle.DATECREATION;
                Lclient.USERMODIFICATION = FactureAregle.USERMODIFICATION;
                Lclient.DATEMODIFICATION = FactureAregle.DATEMODIFICATION;
                Lclient.PK_ID = FactureAregle.PK_IDCLIENT .Value;

                decimal? montantNAF = FactureAregle.MONTANTNAF;

                if (montantNAF == FactureAregle.MONTANTPAYE)
                    Entities.DeleteEntity<LCLIENT>(Lclient, cmd);
                else
                {
                    Lclient.MONTANT = montantNAF - FactureAregle.MONTANTPAYE;
                    Entities.UpdateEntity<LCLIENT>(Lclient, cmd);
                }
                // conversion des csreglement en objet Transcaisse et Lclient 
                TRANSCAISSE transcaiss = new TRANSCAISSE();
                transcaiss.CENTRE  = FactureAregle.CENTRE  ;
                transcaiss.CLIENT = FactureAregle.CLIENT ;
                transcaiss.ORDRE = FactureAregle.ORDRE  ;
                transcaiss.REFEM = FactureAregle.REFEM;
                transcaiss.NDOC = FactureAregle.NDOC;
                transcaiss.COPER  = FactureAregle.COPER;
                transcaiss.DTRANS = DateTime.Today.Date;
                transcaiss.DEXIG = FactureAregle.DATEEXIGIBLE;
                transcaiss.MONTANT = FactureAregle.MONTANTPAYE;
                transcaiss.CAISSE  = FactureAregle.CAISSE;
                transcaiss.ACQUIT = FactureAregle.ACQUIT;
                transcaiss.MATRICULE  = FactureAregle.MATRICULE;
                transcaiss.DC = FactureAregle.DC;
                transcaiss.PERCU = FactureAregle.PERCU;
                transcaiss.RENDU = FactureAregle.RENDU;
                transcaiss.MODEREG = FactureAregle.MODEREG;
                transcaiss.PLACE = FactureAregle.PLACE;
                transcaiss.BANQUE = FactureAregle.BANQUE;
                transcaiss.GUICHET = FactureAregle.GUICHET;
                transcaiss.ORIGINE = FactureAregle.ORIGINE;
                transcaiss.MOISCOMPT = FactureAregle.MOISCOMPT;
                transcaiss.TOP1  = FactureAregle.TOP1;
                transcaiss.ECART = FactureAregle.ECART;
                transcaiss.NUMDEM = FactureAregle.NUMDEM;
                transcaiss.NUMCHEQ = FactureAregle.NUMCHEQ;
                transcaiss.SAISIPAR = FactureAregle.SAISIPAR;
                transcaiss.DATEENCAISSEMENT = FactureAregle.DATEENCAISSEMENT;
                transcaiss.USERCREATION = FactureAregle.USERCREATION;
                transcaiss.USERMODIFICATION = FactureAregle.USERMODIFICATION;
                transcaiss.DATECREATION = FactureAregle.DATECREATION.Value.Date;
                transcaiss.DATEMODIFICATION = FactureAregle.DATEMODIFICATION;
                transcaiss.TOPANNUL = FactureAregle.TOPANNUL;
                transcaiss.CANCELLATION = FactureAregle.CANCELLATION;
                transcaiss.PK_ID = FactureAregle.PK_IDTRANSCAISS;

                transcaiss.TOPANNUL = Enumere.CaisseTopAnnulation;
                transcaiss.CANCELLATION = FactureAregle.CANCELLATION;

                Entities.UpdateEntity<TRANSCAISSE>(transcaiss, cmd);
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }
        private void ExecuteRetraitAnnulationReglement(CsLclient LeRecu, galadbEntities cmd)
        {

            //cmd.CommandText = "SPX_ENC_ANNULEENCAISSEMENT";
            try
            {
                DEMANDEANNULATION laDDe = new DEMANDEANNULATION();
                using (galadbEntities ctx = new galadbEntities())
                {
                    laDDe = ctx.DEMANDEANNULATION.FirstOrDefault(t => t.FK_IDHABILITATIONCAISSE == LeRecu.FK_IDHABILITATIONCAISSE && t.ACQUIT == LeRecu.ACQUIT);
                    if (laDDe.STATUS != Enumere.StatusDemandeRetirer)
                        laDDe.STATUS = Enumere.StatusDemandeRetirer;
                }

                List<TRANSCAISSE> Transcaisse = new List<TRANSCAISSE>();
                LeRecu.IsDEMANDEANNULATION = false;
                Transcaisse = TranscaisseReglement(LeRecu, cmd); // prendre les reglements du recu dans la table Transcaisse
                if (Transcaisse.Count > 0)
                    Entities.UpdateEntity<TRANSCAISSE>(Transcaisse, cmd);
                if (laDDe != null && laDDe.STATUS != Enumere.StatusDemandeValider)
                    Entities.UpdateEntity<DEMANDEANNULATION>(laDDe);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void ValiderAnnulationEncaissement(CsLclient LeRecu, galadbEntities cmd)
        {

            try
            {
               
                DEMANDEANNULATION laDDe = cmd.DEMANDEANNULATION.FirstOrDefault(t => t.FK_IDHABILITATIONCAISSE == LeRecu.FK_IDHABILITATIONCAISSE && t.ACQUIT == LeRecu.ACQUIT);
                if (laDDe != null && !string.IsNullOrEmpty( laDDe.STATUS) )
                    laDDe.STATUS = Enumere.StatusDemandeValider;

                List<TRANSCAISSE> Transcaisse = new List<TRANSCAISSE>();
                LeRecu.TOPANNUL = "O";
                Transcaisse = TranscaisseReglement(LeRecu, cmd); // prendre les reglements du recu dans la table Transcaisse
                if (Transcaisse.Count > 0)
                {
                    if (!string.IsNullOrEmpty(Transcaisse.First().NUMDEM))
                    {
                        DEMANDE dem = ANNULATIONDEMANDE(Transcaisse.First().NUMDEM, cmd);
                        if (dem != null)
                        {
                            LeRecu.NUMDEM = dem.NUMDEM;
                            RejeterDemande(LeRecu, cmd);
                            List<RUBRIQUEDEMANDE> lstRubrique = ANNULATIONRUBRIQUEDEMANDE(Transcaisse);
                            Entities.UpdateEntity<RUBRIQUEDEMANDE>(lstRubrique, cmd);
                        }
                    }
                    Entities.UpdateEntity<TRANSCAISSE>(Transcaisse, cmd);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void RejetAnnulationReglement(CsLclient LeRecu, galadbEntities cmd)
        {

            //cmd.CommandText = "SPX_ENC_ANNULEENCAISSEMENT";
            try
            {
                DEMANDEANNULATION laDDe = new DEMANDEANNULATION();
                using (galadbEntities ctx = new galadbEntities())
                {
                    laDDe = ctx.DEMANDEANNULATION.FirstOrDefault(t => t.FK_IDHABILITATIONCAISSE == LeRecu.FK_IDHABILITATIONCAISSE && t.ACQUIT == LeRecu.ACQUIT);
                    if (laDDe.STATUS != Enumere.StatusDemandeRejeter)
                    {
                        laDDe.STATUS = Enumere.StatusDemandeRejeter;
                        laDDe.MOTIFREJET = LeRecu.MOTIFREJET ;
                    }
                }

                List<TRANSCAISSE> Transcaisse = new List<TRANSCAISSE>();
                Transcaisse = TranscaisseRejetetAnnulation(LeRecu, cmd); // prendre les reglements du recu dans la table Transcaisse
                if (Transcaisse.Count > 0)
                    Entities.UpdateEntity<TRANSCAISSE>(Transcaisse, cmd);
                if (laDDe != null && laDDe.STATUS != Enumere.StatusDemandeValider)
                    Entities.UpdateEntity<DEMANDEANNULATION>(laDDe, cmd);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        List<TRANSCAISSE> TranscaisseReglement(List<CsLclient > preglement)
        {
            try
            {
                List<TRANSCAISSE> transcaisseList = new List<TRANSCAISSE>();
                using (galadbEntities context = new galadbEntities())
                {
                    var Transcaisse = context.TRANSCAISSE;
                    foreach (var FactureAregle in preglement)
                    {
                        var transc = Transcaisse.Where(t => t.CENTRE == FactureAregle.CENTRE && t.CLIENT == FactureAregle.CLIENT
                                                            && t.ORDRE == FactureAregle.ORDRE && t.CAISSE == FactureAregle.CAISSE &&
                                                            t.ACQUIT == FactureAregle.ACQUIT && t.MATRICULE == FactureAregle.MATRICULE && t.COPER != Enumere.CoperNAF);
                        if (transc.Count() > 0)
                            foreach (TRANSCAISSE t in transc)
                            {
                                t.CANCELLATION = FactureAregle.MOTIFANNULATION ;
                                t.TOPANNUL = Enumere.CaisseTopAnnulation;
                                transcaisseList.Add(t);
                            }
                    }

                    return transcaisseList;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void ValiderDemandeAnnulationReglement(CsLclient LeRecu, galadbEntities cmd)
        {

            try
            {
                DEMANDEANNULATION laDDe = new DEMANDEANNULATION();
                using (galadbEntities ctx = new galadbEntities())
                {
                    laDDe = ctx.DEMANDEANNULATION.FirstOrDefault(t => t.FK_IDHABILITATIONCAISSE == LeRecu.FK_IDHABILITATIONCAISSE && t.ACQUIT == LeRecu.ACQUIT);
                    if (laDDe != null && !string.IsNullOrEmpty( laDDe.STATUS))
                        laDDe.STATUS = Enumere.StatusDemandeInitier ;
                }
                List<TRANSCAISSE> Transcaisse = new List<TRANSCAISSE>();
                LeRecu.IsDEMANDEANNULATION = true ;
                LeRecu.MOTIFANNULATION = LeRecu.MOTIFANNULATION;
                Transcaisse = TranscaisseReglement(LeRecu, cmd); // prendre les reglements du recu dans la table Transcaisse
                if (Transcaisse.Count > 0)
                    Entities.UpdateEntity<TRANSCAISSE>(Transcaisse, cmd);
                if (laDDe != null && !string.IsNullOrEmpty(laDDe.STATUS))
                    Entities.UpdateEntity<DEMANDEANNULATION>(laDDe);
                else
                {
                    laDDe = new DEMANDEANNULATION() {
                        ACQUIT = LeRecu.ACQUIT ,
                        FK_IDHABILITATIONCAISSE = LeRecu.FK_IDHABILITATIONCAISSE.Value  ,
                         MOTIFDEMANDE = LeRecu.MOTIFANNULATION ,
                          STATUS = Enumere.StatusDemandeInitier 
                    };
                    Entities.InsertEntity <DEMANDEANNULATION>(laDDe);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void ValiderRetraitDemandeAnnulationReglement(CsLclient LeRecu, galadbEntities cmd)
        {

            try
            {
                DEMANDEANNULATION laDDe = new DEMANDEANNULATION();
                using (galadbEntities ctx = new galadbEntities())
                {
                    laDDe = ctx.DEMANDEANNULATION.FirstOrDefault(t => t.FK_IDHABILITATIONCAISSE == LeRecu.FK_IDHABILITATIONCAISSE && t.ACQUIT == LeRecu.ACQUIT);
                    if (laDDe.STATUS != Enumere.StatusDemandeValider)
                        laDDe.STATUS = Enumere.StatusDemandeRetirer ;
                }
                List<TRANSCAISSE> Transcaisse = new List<TRANSCAISSE>();
                LeRecu.IsDEMANDEANNULATION = false ;
                LeRecu.MOTIFANNULATION = LeRecu.MOTIFANNULATION;
                Transcaisse = TranscaisseReglement(LeRecu, cmd); // prendre les reglements du recu dans la table Transcaisse
                if (Transcaisse.Count > 0)
                    Entities.UpdateEntity<TRANSCAISSE>(Transcaisse, cmd);
                if (laDDe != null && laDDe.STATUS != Enumere.StatusDemandeValider)
                    Entities.UpdateEntity<DEMANDEANNULATION>(laDDe);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        List<TRANSCAISSE> TranscaisseDemandeAnnulationReglement(CsLclient  preglement)
        {
            try
            {
                List<TRANSCAISSE> transcaisseList = new List<TRANSCAISSE>();
                using (galadbEntities context = new galadbEntities())
                {
                    var Transcaisse = context.TRANSCAISSE;
                    var transc = Transcaisse.Where(t => t.CAISSE == preglement.CAISSE &&
                                                            t.ACQUIT == preglement.ACQUIT && t.MATRICULE == preglement.MATRICULE);// && t.COPER != Enumere.CoperNAF);
                    if (transc.Count() > 0)
                        foreach (TRANSCAISSE t in transc)
                        {
                            t.MOTIFANNULATION = preglement.MOTIFANNULATION;
                            t.ISDEMANDEANNULATION = preglement.IsDEMANDEANNULATION;
                            transcaisseList.Add(t);
                        }
                    return transcaisseList;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        List<TRANSCAISSE> TranscaisseReglement(CsLclient preglement,galadbEntities cmd)
        {
            try
            {
                List<TRANSCAISSE> transcaisseList = new List<TRANSCAISSE>();
                using (galadbEntities context = new galadbEntities())
                {
                    var Transcaisse = context.TRANSCAISSE;
                    var transc = Transcaisse.Where(t => t.FK_IDHABILITATIONCAISSE == preglement.FK_IDHABILITATIONCAISSE &&
                                                            t.ACQUIT == preglement.ACQUIT && t.MATRICULE == preglement.MATRICULE);// && t.COPER != Enumere.CoperNAF);
                        if (transc.Count() > 0)
                            foreach (TRANSCAISSE t in transc)
                            {
                                    t.MOTIFANNULATION  = preglement.MOTIFANNULATION ;
                                    t.TOPANNUL = preglement.TOPANNUL;
                                    t.ISDEMANDEANNULATION  = preglement.IsDEMANDEANNULATION ;
                                    transcaisseList.Add(t);
                            }

               

                    return transcaisseList;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        List<TRANSCAISSE> TranscaisseRejetetAnnulation(CsLclient preglement, galadbEntities cmd)
        {
            try
            {
                List<TRANSCAISSE> transcaisseList = new List<TRANSCAISSE>();
                using (galadbEntities context = new galadbEntities())
                {
                    var Transcaisse = context.TRANSCAISSE;
                    var transc = Transcaisse.Where(t => t.FK_IDHABILITATIONCAISSE == preglement.FK_IDHABILITATIONCAISSE &&
                                                            t.ACQUIT == preglement.ACQUIT && t.MATRICULE == preglement.MATRICULE);// && t.COPER != Enumere.CoperNAF);
                    if (transc.Count() > 0)
                        foreach (TRANSCAISSE t in transc)
                        {
                            t.ISDEMANDEANNULATION = false;
                            transcaisseList.Add(t);
                        }
                    return transcaisseList;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        List<TRANSCAISSE> TranscaisseReglementNaf(List<CsReglement> preglement)
        {
            try
            {
                List<TRANSCAISSE> transcaisseList = new List<TRANSCAISSE>();
                using (galadbEntities context = new galadbEntities())
                {
                    var Transcaisse = context.TRANSCAISSE;
                    foreach (var FactureAregle in preglement)
                    {
                        var transc = Transcaisse.Where(t => t.CAISSE  == FactureAregle.CAISSE &&
                                                            t.ACQUIT == FactureAregle.ACQUIT && t.MATRICULE  == FactureAregle.MATRICULE && t.COPER  == Enumere.CoperNAF);
                        if (transc.Count() > 0)
                            foreach (TRANSCAISSE t in transc)
                            {
                                t.TOPANNUL = Enumere.CaisseTopAnnulation;
                                t.CANCELLATION = FactureAregle.CANCELLATION;
                                transcaisseList.Add(t);
                            }
                    }

                    return transcaisseList;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        List<LCLIENT> LclientReglement(List<CsReglement> preglement)
        {
            try
            {
                List<LCLIENT> lclientList = new List<LCLIENT>();
                using (galadbEntities context = new galadbEntities())
                {
                    var lclient = context.LCLIENT;
                    foreach (var FactureAregle in preglement)
                    {
                        var client = lclient.Where(l => l.CENTRE == FactureAregle.CENTRE   && l.CLIENT  == FactureAregle.CLIENT  &&
                                                            l.ORDRE == FactureAregle.ORDRE   && l.CAISSE  == FactureAregle.CAISSE &&
                                                            l.ACQUIT == FactureAregle.ACQUIT && l.MATRICULE  == FactureAregle.MATRICULE && l.COPER  != Enumere.CoperNAF);
                        if (client.Count() > 0)
                            foreach (LCLIENT l in client)
                                lclientList.Add(l);
                    }

                    return lclientList;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        List<LCLIENT> LclientReglement(CsReglement preglement)
        {
            try
            {
                List<LCLIENT> lclientList = new List<LCLIENT>();
                using (galadbEntities context = new galadbEntities())
                {
                    var lclient = context.LCLIENT;
                    //foreach (var FactureAregle in preglement)
                    //{
                    var client = lclient.Where(l => l.CAISSE == preglement.CAISSE &&
                                                            l.ACQUIT == preglement.ACQUIT && l.MATRICULE == preglement.MATRICULE && l.COPER != Enumere.CoperNAF);
                    if (client.Count() > 0)
                        foreach (LCLIENT l in client)
                            lclientList.Add(l);
                    //}

                    return lclientList;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        List<LCLIENT> LclientReglementSansNaf(CsLclient  preglement)
        {
            try
            {
                List<LCLIENT> lclientList = new List<LCLIENT>();
                using (galadbEntities context = new galadbEntities())
                {
                    var lclient = context.LCLIENT;
                 
                    var client = lclient.Where(l => l.CAISSE == preglement.CAISSE &&
                                                            l.ACQUIT == preglement.ACQUIT && l.MATRICULE == preglement.MATRICULE);
                        if (client.Count() > 0)
                            lclientList.AddRange(client);
                }
                return lclientList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        List<LCLIENT> LclientReglementNaf(CsLclient preglement)
        {
            try
            {
                List<LCLIENT> lclientList = new List<LCLIENT>();
                using (galadbEntities context = new galadbEntities())
                {
                    var lclient = context.LCLIENT;

                    var client = lclient.Where(l => l.CAISSE == preglement.CAISSE &&
                                                            l.ACQUIT == preglement.ACQUIT && l.MATRICULE == preglement.MATRICULE && l.COPER != Enumere.CoperNAF);
                    if (client.Count() > 0)
                        lclientList.AddRange(client);


                }
                return lclientList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CsLclient > RetourneEtatDeCaisse(CsHabilitationCaisse  laCaisse)
        {
            try
            {
               // List<CsLclient> lstDesOperation = new List<CsLclient>();
               // DataTable dt = CaisseProce dures.RetourneEncaissementCaisse(laCaisse);
               //lstDesOperation = Entities.GetEntityListFromQuery<CsLclient >(dt);
               
               // DataTable dts = CaisseProcedures.RetourneDecaissementCaisse(laCaisse);
               // lstDesOperation.AddRange(Entities.GetEntityListFromQuery<CsLclient>(dts));
               // return lstDesOperation;
                return EtatdeCaisseSPX(laCaisse.PK_ID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CsMoratoire> RetourneListeMoratoire(string centre, string client, string ordre)
        {
            try
            {
                DataTable dt = CommonProcedures.RetourneTousMoratoires();
                return Entities.GetEntityListFromQuery<CsMoratoire>(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public CsEncaissement RetourneDemande(string NumDemande,bool IsExtension)
        {
            try
            {
                CsEncaissement encaissementDemande = new CsEncaissement();

                if (!IsExtension)
                {
                    DataTable dtDemande = CaisseProcedures.RetourneListeDemandesPourCaisse(NumDemande);
                    encaissementDemande.FactureARegler.AddRange(Entities.GetEntityListFromQuery<CsLclient>(dtDemande)); // liste des infos de la demande
                }
                else
                {
                    DataTable dtDemande = CaisseProcedures.RetourneListeDemandesExtensionPourCaisse(NumDemande);
                    encaissementDemande.FactureARegler.AddRange(Entities.GetEntityListFromQuery<CsLclient>(dtDemande)); // liste des infos de la demande
                }
                DataTable dtTaxe = CommonProcedures.RetourneTaxe();
                encaissementDemande.Taxes.AddRange(Entities.GetEntityListFromQuery<CsCtax>(dtTaxe)); // liste des taxes 

                DataTable dtParam = CommonProcedures.RetourneTousParametreGeneraux();
                encaissementDemande.ParametreGeneraux = Entities.GetEntityFromQuery<CsTa>(dtParam); // liste des parametre généraux

                return encaissementDemande;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public CsLclient  RetourneLeDevis(string NumDevis)
        {
            //cmd.CommandText = "SPX_ENC_SELECTDEVIS";
            try
            {
                DataTable dt = CommonProcedures.RetouneDevisParNum(NumDevis);
                return Entities.GetEntityFromQuery<CsLclient>(dt);
                //rows = FillFacture(reader, 0, int.MaxValue);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public  void MiseAJourDevis(CsLclient  reglement, galadbEntities cmd)
        //{
        //    //cmd.CommandText = "SPX_ENC_MISEAJOURDEVIS";
        //    //to do : mise a jour de l'etape du devis manquant ????
        //    try
        //    {
        //            DEVIS devis = ObtenirDevis(reglement.NUMDEVIS);

        //            ObjETAPEDEVIS etapeDevis = DBService.GetEtapeSuivanteCaisse(devis);
        //            devis.MATRICULECAISSE = reglement.MATRICULE;
        //            devis.DATEETAPE = reglement.DENR;
        //            devis.DATEREGLEMENT = reglement.DENR;
        //            devis.DATEMODIFICATION = reglement.DATEMODIFICATION;
        //            devis.USERMODIFICATION = reglement.USERMODIFICATION;
        //            devis.FK_IDETAPEDEVIS = etapeDevis.PK_ID;
                    

        //        Entities.UpdateEntity<DEVIS>(devis,cmd);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //}

        //private DEVIS ObtenirDevis(string pDevis)
        //{
        //    try
        //    {
        //        using (galadbEntities context = new galadbEntities())
        //        {
        //            return context.DEVIS.First(d => d.NUMDEVIS == pDevis);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public void UpdateAcquit(int CaisseHail ,string matricule, string acquit, galadbEntities cm)
        {

            try
            {
                CAISSE laCaisse = RetourneCaisse(CaisseHail);
                //CAISSE laCaisse = RetourneCaisse(NumCaisse);
                if (laCaisse != null)
                {
                    laCaisse.USERMODIFICATION = matricule;
                    laCaisse.DATEMODIFICATION = System.DateTime.Now;
                    laCaisse.ACQUIT = (int.Parse (acquit) + 1).ToString() ;
                    Entities.UpdateEntity<CAISSE>(laCaisse, cm);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }
        private CAISSE RetourneCaisse(int habilCaisse)
        {
            using (galadbEntities context = new galadbEntities())
            {
                CAISSE laCaisse = new CAISSE();
                HABILITATIONCAISSE leCaisseHabil =context.HABILITATIONCAISSE.FirstOrDefault(i=>i.PK_ID == habilCaisse);
                if (leCaisseHabil != null && !string.IsNullOrEmpty( leCaisseHabil.NUMCAISSE))  
                    laCaisse = context.CAISSE.First(d => d.PK_ID  == leCaisseHabil.FK_IDCAISSE );
                return laCaisse;
            }
        
        }
        public void MiseAJourRubriquePrestationDemande( CsLclient  demande, galadbEntities cm)
        {
            try
            {
                List<RUBRIQUEDEMANDE> lstRubrique = cm.RUBRIQUEDEMANDE.Where(t => t.NUMDEM == demande.NUMDEM && t.ISEXTENSION == true).ToList();
                if (lstRubrique != null && lstRubrique.Count != 0)
                    lstRubrique.ForEach(t => t.DATECAISSE = System.DateTime.Today);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public void MiseAJourRubriqueDemande(CsLclient demande, galadbEntities cm)
        {
            try
            {
                List<RUBRIQUEDEMANDE> lstRubrique = cm.RUBRIQUEDEMANDE.Where(t => t.NUMDEM == demande.NUMDEM && t.REFEM == demande.REFEM && t.NDOC == demande.NDOC ).ToList();
                if (lstRubrique != null && lstRubrique.Count != 0)
                    lstRubrique.ForEach(t => t.DATECAISSE = System.DateTime.Today);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public void TransmettreDemande(CsLclient demande, galadbEntities cm)
        {
            try
            {
                new DbWorkFlow().ExecuterActionSurDemandeTransction(demande.NUMDEM, Enumere.TRANSMETTRE, demande.MATRICULE, string.Empty,cm  );
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public void RejeterDemande(CsLclient demande, galadbEntities cm)
        {
            try
            {
                List<string> lstCodeDemande = new List<string>();
                lstCodeDemande.Add(demande.CODE_WKF);
                new DbWorkFlow().ExecuterActionSurDemandeTransction(demande.NUMDEM, Enumere.REJETER , demande.MATRICULE, string.Empty, cm);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        public void MiseAJourMoratoire(List<CsLclient> LesPaiement, galadbEntities cm)
        {
            try
            {
                foreach (var item in LesPaiement)
	            {
		         List<DETAILMORATOIRE> lstMoratoire = cm.DETAILMORATOIRE.Where(t => t.FK_IDMORATOIRE  == item.FK_IDMORATOIRE && t.NDOC == item.NDOC ).ToList();
                 if (lstMoratoire != null && lstMoratoire.Count != 0)
                     lstMoratoire.ForEach(t => t.DATECAISSE = System.DateTime.Today);
	            }
               
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public void MiseAJourDemande(CsLclient reglement, DEMANDE demande, galadbEntities cm)
        {

            //cmd.CommandText = "SPX_ENC_MISEDEMANDE";

            try
            {
                    demande.DCAISSE = reglement.DENR;
                    //demande.USERCREATION = reglement.USERCREATION;
                    demande.USERMODIFICATION = reglement.USERMODIFICATION;
                    //demande.DATECREATION = reglement.DATECREATION.Value.Date;
                    demande.DATEMODIFICATION = reglement.DATEMODIFICATION;
                    demande.STATUT =Enumere.DemandeStatusPasseeEncaisse;
                
                Entities.UpdateEntity<DEMANDE>(demande, cm);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public string  VerifieCaisseDejaSaisie(string date, string matricule)
        {
            cn = new SqlConnection(ConnectionString);

            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandTimeout = 360;
            cmd.CommandType = CommandType.StoredProcedure;
            //cmd.CommandText = "SPX_ENC_CAISSEDEJASAISI";
            cmd.CommandText = "SPX_ENC_CAISSEDEJASAISIE";
            cmd.Parameters.Add("@DATE", SqlDbType.VarChar).Value = date;
            cmd.Parameters.Add("@MATRICULE", SqlDbType.VarChar).Value = matricule;

            try
            {

                if (cn.State == ConnectionState.Closed)
                    cn.Open();
                string  resultat = string .Empty ;
                SqlDataReader reader = cmd.ExecuteReader();
                foreach (object item in reader)
                {
                    resultat = (Convert.IsDBNull(reader["matricule"])) ? string.Empty  : (System.String )reader["matricule"];
                }
                reader.Close();
                return resultat;
                
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

        public void MiseAJourMoratoire(int NumMoratoire,galadbEntities cmd)
        {

            //cmd.CommandText = "SPX_ENC_MISEAJOURMORATOIRE";

            try
            {
                MORATOIRE moratoire = new MORATOIRE() { 
                 PK_ID = NumMoratoire,
                 STATUS = 0
                };

                Entities.UpdateEntity<MORATOIRE>(moratoire, cmd);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CsCoper > RetourneListeDeCoperOD( )
        {
            //cmd.CommandText = "SPX_COPEROD_SELECT_ALL";

            try
            {
                DataTable dt = CommonProcedures.RetourneTousCoperOD();
                return Entities.GetEntityListFromQuery<CsCoper>(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
       
        /* CHK - 16/07/2013  */
        /* HGB - 22/08/2013  */
        public List<CsModereglement> RetourneModesReglement()
        {
            //cmd.CommandText = "SPX_MODEREG_SELECT_ALL";

            try
            {
                DataTable dt = Galatee.Entity.Model.CommonProcedures.RetourneTousModeReglement();
                return Entities.GetEntityListFromQuery<CsModereglement>(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region Gestion du mode deconnecté - CHK - 13/03/2013
        public bool IsServerDown()
        {
            try
            {
                using (Galatee.Entity.Model.galadbEntities context = new Galatee.Entity.Model.galadbEntities())
                {
                   return !context.Database.Exists();
                }
            }
            catch (Exception)
            {
                return true;
            }
        }

        public bool SynchroniserEncaissements(string client, string centre, string ordre, decimal montant, string caisse, string acquit, string matricule, string numCheq,DateTime dateEnregistrement)
        {
            cn = new SqlConnection(ConnectionString);
            int rowsAffected = -1;
            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandTimeout = 240;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SPX_ENC_SYNCHRO_CAISSE";
            cmd.Parameters.Add("@Centre", SqlDbType.VarChar).Value = (string.IsNullOrEmpty(centre)) ? null : centre;
            cmd.Parameters.Add("@Client", SqlDbType.VarChar).Value = (string.IsNullOrEmpty(client)) ? null : client;
            cmd.Parameters.Add("@Ordre", SqlDbType.VarChar).Value = (string.IsNullOrEmpty(ordre)) ? null : ordre;
            cmd.Parameters.Add("@Montant", SqlDbType.Decimal).Value = montant;
            cmd.Parameters.Add("@Caisse", SqlDbType.VarChar).Value = (string.IsNullOrEmpty(caisse)) ? null : caisse;
            cmd.Parameters.Add("@acquit", SqlDbType.VarChar).Value = (string.IsNullOrEmpty(acquit)) ? null : acquit;
            cmd.Parameters.Add("@matricule", SqlDbType.VarChar).Value = (string.IsNullOrEmpty(matricule)) ? null : matricule;
            cmd.Parameters.Add("@NumCheq", SqlDbType.VarChar).Value = (string.IsNullOrEmpty(numCheq)) ? null : numCheq;
            cmd.Parameters.Add("@DateEnregistrement", SqlDbType.Date).Value = dateEnregistrement;


            DBBase.SetDBNullParametre(cmd.Parameters);
                if (cn.State == ConnectionState.Closed)
                    cn.Open();
                          
                try
                {
                    rowsAffected = cmd.ExecuteNonQuery();
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
        #endregion

        public bool EnregistrerTraceSynchronisation(string path,string contenu)
        {
            try
            {
                // Enregistrement dans le fichier dedié
                //string directory = @Directory.GetCurrentDirectory() + @"\" + path +".txt"; 
                File.WriteAllText(path, contenu);
                return true;
            }
            catch (Exception ex)
            {
                string erreur = ex.Message;
                return false;
                //throw;
            }
        }

        public List<CsFraisTimbre> RetourneListeTimbre()
        {
            DataTable dt =  Galatee.Entity.Model.CommonProcedures.RetourneListeTimbre();
            return Entities.GetEntityListFromQuery<CsFraisTimbre>(dt);
        }


        public List<CsFraisTimbre> RetourneListeTimbreOffline()
        {
            List<CsFraisTimbre> c = new List<CsFraisTimbre>();
            try
            {
                List<Structure.FRAISTIMB> _lst = new List<Structure.FRAISTIMB>();
                List<CsFraisTimbre> _lstResult = new List<CsFraisTimbre>();
                //_lst =SqlCeCommand.GetEntitiesFromDataBase<Structure.FRAISTIMB>().ToList();
                //_lstResult = Tools.Utility.ConvertListType<CsFraisTimbre, Structure.FRAISTIMB>(_lst);
                return _lstResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }




        public List<CsHabilitationCaisse> RetourneCaisseHabiliterCentre(CsCentre leCentre)
        {
            try
            {
                DataTable dt = Galatee.Entity.Model.CaisseProcedures.RetourneCaisseHabiliterCentre(leCentre);
                return Entities.GetEntityListFromQuery<CsHabilitationCaisse>(dt).Where(t=>t.DATE_FIN == null ).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public bool AjustementDeFondCaisse(string NumCaisse, string NouveauFond, string MoisCompt, string matricule)
        {
            try
            {
                return Galatee.Entity.Model.CaisseProcedures.AjustementDeFondCaisse(NumCaisse, NouveauFond, MoisCompt,matricule);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public decimal ? RetourneEncaissementDate(CsHabilitationCaisse laCaisse)
        {
            try
            {
                return Galatee.Entity.Model.CaisseProcedures.RetourneEncaissementDate(laCaisse);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public decimal? RetourneEncaissementEspeceDate(CsHabilitationCaisse laCaisse)
        {
            try
            {
                return Galatee.Entity.Model.CaisseProcedures.RetourneEncaissementDate(laCaisse);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    

        public List<CsHabilitationCaisse> ListeDesReversementCaisse(List<CsHabilitationCaisse> LstHabilCaisse)
        {
            try
            {
                DataTable dt = Galatee.Entity.Model.CaisseProcedures.ListeDesReversementCaisse(LstHabilCaisse);
                List<CsHabilitationCaisse> lstHabilCaisse = Entities.GetEntityListFromQuery<CsHabilitationCaisse>(dt);
                foreach (CsHabilitationCaisse item in lstHabilCaisse)
                    item.MONTANTENCAISSE = RetourneEncaissementEspeceDate(item);

                return lstHabilCaisse;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool MiseAJourDemandeReversement(CsDemandeReversement DemandeReversement, int Action)
        {
            try
            {
                //if (Action == 1)
                //    return Galatee.Entity.Model.CaisseProcedures.InsererDemandeReversement(DemandeReversement);
                //else 
                //    return Galatee.Entity.Model.CaisseProcedures.updateDemandeReversement (DemandeReversement);
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public List<CsLclient> LitseDesTransaction(CsHabilitationCaisse laCaisse)
        {
            try
            {
                //DataTable dt = Galatee.Entity.Model.CaisseProcedures.LitseDesTransaction(laCaisse);
                //List<CsLclient> lst = Entities.GetEntityListFromQuery<CsLclient>(dt);
                List<CsLclient> lstResulat = new List<CsLclient>();
                    lstResulat.AddRange(HistoriqueListeEncaissementsSPX(laCaisse.PK_ID, true));
                    lstResulat.ForEach(i => i.CRET  = (string.IsNullOrEmpty(i.NUMDEM) ? "N" : "O")); 
                return lstResulat;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CsLclient > HistoriqueListeEncaissements(List<CsHabilitationCaisse> laCaisse)
        {
            try
            {
                //DataTable dt = Galatee.Entity.Model.CaisseProcedures.HistoriqueListeEncaissements(laCaisse);
                //return Entities.GetEntityListFromQuery<CsLclient>(dt);

                List<CsLclient> lstResulat = new List<CsLclient>();
                foreach (CsHabilitationCaisse item in laCaisse)
                lstResulat.AddRange(HistoriqueListeEncaissementsSPX(item.PK_ID ,false ));
                lstResulat.ForEach(i => i.CRET = (string.IsNullOrEmpty(i.NUMDEM) ? "N" : "O")); 
                return lstResulat ;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CsLclient> HistoriqueDesEncaissements(string matricule, int idCentre, DateTime datedebut, DateTime datefin)
        {
            cn = new SqlConnection(ConnectionString);

            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandTimeout = 3000;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SPX_CAIS_RETOURNEHISTORIQUECAISSE";
            cmd.Parameters.Add("@MATRICULE", SqlDbType.VarChar,6).Value = matricule;
            cmd.Parameters.Add("@FK_IDCENTRE", SqlDbType.Int).Value = idCentre;
            cmd.Parameters.Add("@DATEDEBUT", SqlDbType.DateTime).Value = datedebut;
            cmd.Parameters.Add("@DATEFIN", SqlDbType.DateTime).Value = datefin;

            DBBase.SetDBNullParametre(cmd.Parameters);
            try
            {
                if (cn.State == ConnectionState.Closed)
                    cn.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);

                return Entities.GetEntityListFromQuery<CsLclient>(dt); ;

            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                    cn.Close(); // Fermeture de la connection 
                cmd.Dispose();
            }
        }

        
        public List<CsLclient> HistoriqueListeEncaissementsSPX(int idhabilcaisse, bool IsListeTransactionCaisse)
        {
            cn = new SqlConnection(ConnectionString);

            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandTimeout = 3000;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SPX_CAIS_RETOURNELISTEDESTRANSACTION";
            cmd.Parameters.Add("@FK_IDHABILCAISSE", SqlDbType.Int).Value = idhabilcaisse;
            cmd.Parameters.Add("@IsListeDesTransaction", SqlDbType.Bit).Value = IsListeTransactionCaisse;

            DBBase.SetDBNullParametre(cmd.Parameters);
            try
            {
                if (cn.State == ConnectionState.Closed)
                    cn.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);

                return Entities.GetEntityListFromQuery<CsLclient>(dt); ;

            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                    cn.Close(); // Fermeture de la connection 
                cmd.Dispose();
            }
        }

        public List<CsHabilitationCaisse> ListeDesCaisse(int fk_idcaisse,string centre,DateTime datedebut, DateTime datefin,bool IsFerme)
        {
            try
            {
                DataTable dt = Galatee.Entity.Model.CaisseProcedures.ListeDesEtatCaisse(fk_idcaisse, centre, string.Empty, datedebut, datefin);
                List<CsHabilitationCaisse> lstCaisse = Entities.GetEntityListFromQuery<CsHabilitationCaisse>(dt);
                if (IsFerme)
                 return    lstCaisse.Where(p => p.DATE_FIN != null).ToList();
                else return lstCaisse;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CsLclient> ChargerListeFacturePeriode(string periodeDebut, string periodeFin, string FactureDeb, string FactureFin)
        {
            try
            {
                DataTable dt = Galatee.Entity.Model.CaisseProcedures.ChargerListeFacturePeriode( periodeDebut,  periodeFin,  FactureDeb,  FactureFin);
                return Entities.GetEntityListFromQuery<CsLclient>(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool FermetureCaisseSpx(CsHabilitationCaisse laCaissehabil)
        {
            try
            {
                cn = new SqlConnection(ConnectionString);

                cmd = new SqlCommand();
                cmd.Connection = cn;
                cmd.CommandTimeout = 3000;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SPX_CAIS_CLOTUREDECAISSE";
                cmd.Parameters.Add("@HabilitationCaisse", SqlDbType.Int).Value = laCaissehabil.PK_ID;
                cmd.Parameters.Add("@matricule", SqlDbType.VarChar, 6).Value = laCaissehabil.MATRICULE;
                cmd.Parameters.Add("@NumCaisse", SqlDbType.VarChar, 3).Value = laCaissehabil.NUMCAISSE;
                DBBase.SetDBNullParametre(cmd.Parameters);
                try
                {
                    int Res = -1;
                    if (cn.State == ConnectionState.Closed)
                        cn.Open();
                    Res = cmd.ExecuteNonQuery();
                    return Res == -1 ? false : true;
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
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool FermetureCaisse(CsHabilitationCaisse laCaissehabil)
        {
            try
            {
                if (FermetureCaisseSpx(laCaissehabil))
                {
                    SupprimeDoublonsCaisseArret(laCaissehabil.PK_ID);
                    return true;
                }
                else return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public string VerifieEtatCaisse(string matricule, int? Fk_IdCaisse)
        {
            cn = new SqlConnection(ConnectionString);

            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandTimeout = 360;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SPX_CAIS_VERIFIEETATCAISSE";
            cmd.Parameters.Add("@matricule", SqlDbType.VarChar, 6).Value = matricule;
            cmd.Parameters.Add("@idcaisse", SqlDbType.Int).Value = Fk_IdCaisse;

            try
            {

                if (cn.State == ConnectionState.Closed)
                    cn.Open();
                int _resultatRequete = 0;
                SqlDataReader reader = cmd.ExecuteReader();
                foreach (object item in reader)
                {
                    _resultatRequete = (Convert.IsDBNull(reader["RESULTAT"])) ? 0 : (System.Int32)reader["RESULTAT"];
                }
                reader.Close();
                switch (_resultatRequete)
                {
                    case 0:
                        return Enumere.EtatDeCaisseValider;
                    case 1:
                        return Enumere.EtatDeCaissePasCassier;
                    case 2:
                        return Enumere.EtatDeCaisseDejaCloture;
                    case 3:
                        return Enumere.EtatDeCaisseNonCloture;
                    case 4:
                        //return Enumere.EtatDeCaisseOuverteALaDemande;
                        return Enumere.EtatDeCaisseAutreSessionOuvert;
                    default:
                        break;
                }
                return string.Empty;
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
        public List<CsLclient> EtatdeCaisseSPX(int idhabilcaisse)
        {
            cn = new SqlConnection(ConnectionString);

            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandTimeout = 3000;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SPX_CAIS_ETATCAISSE";
            cmd.Parameters.Add("@idHabiliationCaisse", SqlDbType.Int).Value = idhabilcaisse;

            DBBase.SetDBNullParametre(cmd.Parameters);
            try
            {
                if (cn.State == ConnectionState.Closed)
                    cn.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);

                return Entities.GetEntityListFromQuery<CsLclient>(dt); ;

            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                    cn.Close(); // Fermeture de la connection 
                cmd.Dispose();
            }
        }

        public void  SupprimeDoublonsCaisse(int IdHabilitationCaisse,string Acquit)
        {
            try
            {
                cn = new SqlConnection(ConnectionString);

                cmd = new SqlCommand();
                cmd.Connection = cn;
                cmd.CommandTimeout = 3000;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SPX_CAIS_SUPPRIMEDOUBLONS";
                cmd.Parameters.Add("@HabilitationCaisse", SqlDbType.Int).Value = IdHabilitationCaisse;
                cmd.Parameters.Add("@Acquitement", SqlDbType.VarChar, 9).Value = Acquit;

                DBBase.SetDBNullParametre(cmd.Parameters);
                try
                {
                    if (cn.State == ConnectionState.Closed)
                        cn.Open();
                    cmd.ExecuteNonQuery();
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
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void SupprimeDoublonsCaisseArret(int IdHabilitationCaisse)
        {
            try
            {
                cn = new SqlConnection(ConnectionString);

                cmd = new SqlCommand();
                cmd.Connection = cn;
                cmd.CommandTimeout = 3000;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SPX_CAIS_SUPPRIMEDOUBLONSARRET";
                cmd.Parameters.Add("@HabilitationCaisse", SqlDbType.Int).Value = IdHabilitationCaisse;

                DBBase.SetDBNullParametre(cmd.Parameters);
                try
                {
                    if (cn.State == ConnectionState.Closed)
                        cn.Open();
                    cmd.ExecuteNonQuery();
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
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region Sylla

        public List<CsLclient> RetourneListeFactureNonSolde(string centre, string client, string ordre, int foreignkey, string REFEM)
        {
            try
            {
                List<CsLclient> ListeFacture = new List<CsLclient>();
                List<CsLclient> ListeDeReglement = new List<CsLclient>();
                List<CsLclient> ListeDesFacture = new List<CsLclient>();

                DataTable dt = CaisseProcedures.LigneFactureClient(centre, client, ordre, REFEM);
                ListeFacture = Galatee.Tools.Utility.GetEntityListFromQuery<CsLclient>(dt);

                DataTable dts = AccueilProcedures.RetourneEncaissement(ListeFacture.First().FK_IDCENTRE, centre, client, ordre, REFEM);
                ListeDeReglement = Galatee.Tools.Utility.GetEntityListFromQuery<CsLclient>(dts);

                decimal? SoldeClient = 0;
                if (ListeFacture.Count == 0) return null;
                if (ListeDeReglement.Count == 0)
                    SoldeClient = ListeFacture.Sum(t => t.MONTANT);
                if (ListeFacture.Count != 0 && ListeDeReglement.Count != 0)
                    SoldeClient = ListeFacture.Sum(t => t.MONTANT) - ListeDeReglement.Sum(t => t.MONTANT);

                foreach (CsLclient item in ListeFacture)
                {
                    List<CsLclient> lstReglement = ListeDeReglement.Where(t => t.REFEM == item.REFEM && t.NDOC == item.NDOC).ToList();
                    if (lstReglement != null && lstReglement.Count != 0)
                    {
                        decimal? SoldeFacture = FonctionCaisse.RetourneSoldeDocument(item, lstReglement);
                        //if (SoldeFacture >= 50)
                        if (SoldeFacture >= 1)
                        {
                            item.SOLDECLIENT = SoldeClient;
                            item.SOLDEFACTURE = SoldeFacture;
                            ListeDesFacture.Add(item);
                        }
                    }
                    //else if (item.MONTANT >= 50)
                    else if (item.MONTANT >= 1)
                    {
                        item.SOLDECLIENT = SoldeClient;
                        item.SOLDEFACTURE = item.MONTANT;
                        ListeDesFacture.Add(item);
                    }
                }
                return ListeFacture;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public List<CsLclient> RetourneListeFactureNonSolde(CsClient leClient)
        //{
        //    try
        //    {
        //        List<CsLclient> ListeFacture = new List<CsLclient>();
        //        List<CsLclient> ListeDeReglement = new List<CsLclient>();
        //        List<CsLclient> ListeDesFacture = new List<CsLclient>();
        //        List<CsLclient> ListeDesFactureNegative = new List<CsLclient>();

        //        //DataTable dt = CaisseProcedures.LigneFactureClient(leClient.PK_ID);
        //        //ListeFacture = Galatee.Tools.Utility.GetEntityListFromQuery<CsLclient>(dt);
        //        //DataTable dts = AccueilProcedures.RetourneEncaissement((int)leClient.FK_IDCENTRE, leClient.CENTRE, leClient.REFCLIENT, leClient.ORDRE);
        //        //ListeDeReglement = Galatee.Tools.Utility.GetEntityListFromQuery<CsLclient>(dts);
        //        //DataTable dtss = AccueilProcedures.RetourneFactureNegative((int)leClient.FK_IDCENTRE, leClient.CENTRE, leClient.REFCLIENT, leClient.ORDRE);
        //        //ListeDesFactureNegative = Galatee.Tools.Utility.GetEntityListFromQuery<CsLclient>(dtss);
        //        //ListeDesFactureNegative.ForEach(y => y.MONTANT = y.MONTANT * -1);

        //        ListeFacture = RetourneFactureClient(leClient.PK_ID, leClient.CENTRE, leClient.REFCLIENT, leClient.ORDRE);
        //        ListeDeReglement = RetourneReglementClient(leClient.FK_IDCENTRE.Value,leClient.CENTRE, leClient.REFCLIENT, leClient.ORDRE);
        //        ListeDesFactureNegative = RetourneFactureClientNegative(leClient.PK_ID, leClient.CENTRE, leClient.REFCLIENT, leClient.ORDRE);

        //        decimal? SoldeClient = 0;
        //        if (ListeFacture != null && ListeFacture.Count == 0) return null;

        //        if  ( ListeDeReglement != null && ListeDeReglement.Count == 0)
        //            SoldeClient = ListeFacture.Sum(t => t.MONTANT);

        //        if (ListeFacture.Count != 0 && ListeDeReglement.Count != 0)
        //            SoldeClient = ListeFacture.Sum(t => t.MONTANT) - ListeDeReglement.Sum(u=>u.MONTANT);
        //        leClient.SOLDE = SoldeClient;

        //        if (ListeDesFactureNegative != null )
        //        ListeDeReglement.AddRange(ListeDesFactureNegative);

        //        foreach (CsLclient item in ListeFacture.Where(t=>t.MONTANT> 0))
        //        {
        //            List<CsLclient> lstReglement = ListeDeReglement.Where(t => t.REFEM == item.REFEM && t.NDOC == item.NDOC).ToList();
        //            if (lstReglement != null && lstReglement.Count != 0)
        //            {
        //                decimal? SoldeFacture = FonctionCaisse.RetourneSoldeDocument(item, lstReglement);
        //                //if (SoldeFacture >= 50)
        //                if (SoldeFacture >= 1)
        //                {
        //                    item.SOLDECLIENT = SoldeClient;
        //                    item.SOLDEFACTURE = SoldeFacture;
        //                    item.MONTANTPAYPARTIEL = item.MONTANT - item.SOLDEFACTURE;
        //                    item.MONTANTTVA = item.MONTANTTVA;
        //                    item.LIBELLESITE = leClient.LIBELLESITE;
        //                    item.NOM = leClient.NOMABON;
        //                    item.ADRESSE = leClient.ADRMAND1;
        //                    ListeDesFacture.Add(item);
        //                }
        //            }
        //            //else if (item.MONTANT >= 50)
        //            else if (item.MONTANT >= 1)
        //            {
        //                item.SOLDECLIENT = SoldeClient;
        //                item.SOLDEFACTURE = item.MONTANT;
        //                item.MONTANTPAYPARTIEL = item.MONTANT - item.SOLDEFACTURE;
        //                item.MONTANTTVA = item.MONTANTTVA;
        //                item.LIBELLESITE = leClient.LIBELLESITE;
        //                item.NOM = leClient.NOMABON;
        //                item.ADRESSE = leClient.ADRMAND1;
        //                ListeDesFacture.Add(item);
        //            }
        //        }

        //        return ListeDesFacture;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public List<CsLclient> RetourneListeFactureNonSolde(CsClient leClient)
        {
            try
            {
                List<CsLclient> ListeFacture = new List<CsLclient>();
                ListeFacture = RetourneFactureClientNonSolde(leClient.PK_ID, leClient.CENTRE, leClient.REFCLIENT, leClient.ORDRE);
                if (ListeFacture != null && ListeFacture.Count !=0)
                    leClient.SOLDE = ListeFacture.First().SOLDECLIENT ;
                else
                    if (ListeFacture != null && ListeFacture.Count == 0) return null;
                return ListeFacture;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CsLclient> RetourneListeFactureNonSoldeProcedure(CsClient leClient)
        {
            try
            {
                List<CsLclient> ListeFacture = new List<CsLclient>();
                DataTable dt = RetourneListeFactureNonSoldeSpx(leClient);

                List<CsLclient> lstFacture = Tools.Utility.GetEntityListFromQuery<CsLclient>(dt);
                if (lstFacture.Count == 1 && lstFacture.First().FK_IDLCLIENT == null)
                    lstFacture.ForEach(t => t.IsPAIEMENTANTICIPE = true);
                return lstFacture;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CsLclient> RetourneListeFactureNonSoldeSpx(string Centre,string Client,string Ordre,int IdCentre)
        {
            cn = new SqlConnection(ConnectionString);

            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandTimeout = 3000;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SPX_IMPAYES_CLIENT";
            cmd.Parameters.Add("@Centre", SqlDbType.VarChar, 3).Value = Centre;
            cmd.Parameters.Add("@Client", SqlDbType.VarChar, 20).Value = Client;
            cmd.Parameters.Add("@Ordre", SqlDbType.VarChar, 3).Value = Ordre;
            cmd.Parameters.Add("@idcentre", SqlDbType.Int).Value = IdCentre;

            DBBase.SetDBNullParametre(cmd.Parameters);
            try
            {
                if (cn.State == ConnectionState.Closed)
                    cn.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);
                return Galatee.Tools.Utility.GetEntityListFromQuery<CsLclient>(dt);
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
        public DataTable  RetourneListeFactureNonSoldeSpx(CsClient leClient)
        {
            cn = new SqlConnection(ConnectionString);

            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandTimeout = 3000;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SPX_IMPAYES_CLIENT";
            cmd.Parameters.Add("@Centre", SqlDbType.VarChar, 3).Value = leClient.CENTRE ;
            cmd.Parameters.Add("@Client", SqlDbType.VarChar, 20).Value = leClient.REFCLIENT ;
            cmd.Parameters.Add("@Ordre", SqlDbType.VarChar, 3).Value = leClient.ORDRE;
            cmd.Parameters.Add("@idcentre", SqlDbType.Int).Value = leClient.FK_IDCENTRE ;

            DBBase.SetDBNullParametre(cmd.Parameters);
            try
            {
                if (cn.State == ConnectionState.Closed)
                    cn.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);
                return dt;
                 
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
        public DataTable RetourneListeFactureNonSoldeRegrouperSpx(int idreg,string periode)
        {
            cn = new SqlConnection(ConnectionString);

            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandTimeout = 3000;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SPX_IMPAYES_REGROUPEMENT";
            cmd.Parameters.Add("@idRegroupement", SqlDbType.Int).Value = idreg;
            cmd.Parameters.Add("@periode", SqlDbType.VarChar, 6).Value = periode;

            DBBase.SetDBNullParametre(cmd.Parameters);
            try
            {
                if (cn.State == ConnectionState.Closed)
                    cn.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                    dt.Load(reader);

                return dt;

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
        public DataTable RetourneListeFactureNonSoldeRegrouperProduitSpx(int idreg, string periode,int idProduit)
        {
            cn = new SqlConnection(ConnectionString);

            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandTimeout = 3000;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SPX_IMPAYES_REGROUPEMENT_PRODUIT";
            cmd.Parameters.Add("@idRegroupement", SqlDbType.Int).Value = idreg;
            cmd.Parameters.Add("@periode", SqlDbType.VarChar, 6).Value = periode;
            cmd.Parameters.Add("@idProduit", SqlDbType.Int).Value = idProduit;

            DBBase.SetDBNullParametre(cmd.Parameters);
            try
            {
                if (cn.State == ConnectionState.Closed)
                    cn.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);

                return dt;

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

        public List<CsLclient> RetourneListeFactureNonSolde(int IdClient)
        {
            try
            {
                List<CsLclient> ListeFacture = new List<CsLclient>();
                List<CsLclient> ListeDeReglement = new List<CsLclient>();
                List<CsLclient> ListeDesFacture = new List<CsLclient>();

                DataTable dt = CaisseProcedures.LigneFactureClient(IdClient);
                ListeFacture = Galatee.Tools.Utility.GetEntityListFromQuery<CsLclient>(dt);

                DataTable dts = AccueilProcedures.RetourneEncaissement(ListeFacture.First().FK_IDCENTRE, ListeFacture.First().CENTRE, ListeFacture.First().CLIENT, ListeFacture.First().ORDRE);
                ListeDeReglement = Galatee.Tools.Utility.GetEntityListFromQuery<CsLclient>(dts);

                decimal? SoldeClient = 0;
                if (ListeFacture.Count == 0) return null;
                if (ListeDeReglement.Count == 0)
                    SoldeClient = ListeFacture.Sum(t => t.MONTANT);
                if (ListeFacture.Count != 0 && ListeDeReglement.Count != 0)
                    SoldeClient = ListeFacture.Sum(t => t.MONTANT) - ListeDeReglement.Sum(t => t.MONTANT);

                foreach (CsLclient item in ListeFacture)
                {
                    List<CsLclient> lstReglement = ListeDeReglement.Where(t => t.REFEM == item.REFEM && t.NDOC == item.NDOC).ToList();
                    if (lstReglement != null && lstReglement.Count != 0)
                    {
                        decimal? SoldeFacture = FonctionCaisse.RetourneSoldeDocument(item, lstReglement);
                        if (SoldeFacture >= 1)
                        {
                            item.SOLDECLIENT = SoldeClient;
                            item.SOLDEFACTURE = SoldeFacture;
                            item.MONTANTPAYPARTIEL = item.MONTANT - item.SOLDEFACTURE;
                            item.MONTANTTVA = item.MONTANTTVA;
                            ListeDesFacture.Add(item);
                        }
                    }
                    else if (item.MONTANT >= 1)
                    {
                        item.SOLDECLIENT = SoldeClient;
                        item.SOLDEFACTURE = item.MONTANT;
                        item.MONTANTPAYPARTIEL = item.MONTANT - item.SOLDEFACTURE;
                        item.MONTANTTVA = item.MONTANTTVA;
                        ListeDesFacture.Add(item);
                    }
                }
                return ListeDesFacture;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CsLclient> RetourneListeFactureNonSolde(string centre, string client, string ordre,int foreignkey)
        {
            try
            {
                List<CsLclient> ListeFacture = new List<CsLclient>();
                List<CsLclient> ListeDeReglement = new List<CsLclient>();
                List<CsLclient> ListeDesFacture = new List<CsLclient>();
                List<CsLclient> ListeDesFactureNegative = new List<CsLclient>();

                DataTable dt = CaisseProcedures.LigneFactureClient(foreignkey);
                ListeFacture = Galatee.Tools.Utility.GetEntityListFromQuery<CsLclient>(dt);


                DataTable dts = AccueilProcedures.RetourneEncaissement(ListeFacture.First().FK_IDCENTRE, centre, client, ordre);
                ListeDeReglement = Galatee.Tools.Utility.GetEntityListFromQuery<CsLclient>(dts);

                DataTable dtss = AccueilProcedures.RetourneFactureNegative(ListeFacture.First().FK_IDCENTRE, centre, client, ordre);
                ListeDesFactureNegative = Galatee.Tools.Utility.GetEntityListFromQuery<CsLclient>(dtss);
                ListeDesFactureNegative.ForEach(y => y.MONTANT = y.MONTANT * -1);

                decimal? SoldeClient = 0;
                if (ListeFacture.Count == 0) return null;
                if (ListeDeReglement.Count == 0)
                    SoldeClient = ListeFacture.Sum(t => t.MONTANT);
                if (ListeFacture.Count != 0 && ListeDeReglement.Count != 0)
                    SoldeClient = ListeFacture.Sum(t => t.MONTANT) - ListeDeReglement.Sum(u => u.MONTANT);
                ListeDeReglement.AddRange(ListeDesFactureNegative);

                foreach (CsLclient item in ListeFacture)
                {
                    List<CsLclient> lstReglement = ListeDeReglement.Where(t => t.REFEM == item.REFEM && t.NDOC == item.NDOC).ToList();
                    if (lstReglement != null && lstReglement.Count != 0)
                    {
                        decimal? SoldeFacture = FonctionCaisse.RetourneSoldeDocument(item, lstReglement);
                        if (SoldeFacture >= 1)
                        {
                            item.SOLDECLIENT = SoldeClient;
                            item.SOLDEFACTURE = SoldeFacture;
                            ListeDesFacture.Add(item);
                        }
                    }
                    else if (item.MONTANT >= 1)
                    {
                        item.SOLDECLIENT = SoldeClient;
                        item.SOLDEFACTURE = item.MONTANT;
                        ListeDesFacture.Add(item);
                    }
                }

                return ListeDesFacture;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CsLclient> RetourneListeFactureSoldeNegatif(string centre, string client, string ordre, int foreignkey)
        {
            try
            {
                List<CsLclient> ListeFacture = new List<CsLclient>();
                List<CsLclient> ListeDeReglement = new List<CsLclient>();
                List<CsLclient> ListeDesFacture = new List<CsLclient>();

                DataTable dt = CaisseProcedures.LigneFactureClient(foreignkey);
                ListeFacture = Galatee.Tools.Utility.GetEntityListFromQuery<CsLclient>(dt);

                DataTable dts = AccueilProcedures.RetourneEncaissement(ListeFacture.First().FK_IDCENTRE, centre, client, ordre);
                ListeDeReglement = Galatee.Tools.Utility.GetEntityListFromQuery<CsLclient>(dts);

                decimal? SoldeClient = 0;
                if (ListeFacture.Count == 0) return null;
                if (ListeDeReglement.Count == 0)
                    SoldeClient = ListeFacture.Sum(t => t.MONTANT);
                if (ListeFacture.Count != 0 && ListeDeReglement.Count != 0)
                    SoldeClient = ListeFacture.Sum(t => t.MONTANT) - ListeDeReglement.Sum(t => t.MONTANT);

                foreach (CsLclient item in ListeFacture)
                {
                    List<CsLclient> lstReglement = ListeDeReglement.Where(t => t.REFEM == item.REFEM && t.NDOC == item.NDOC).ToList();
                    if (lstReglement != null && lstReglement.Count != 0)
                    {
                        decimal? SoldeFacture = FonctionCaisse.RetourneSoldeDocument(item, lstReglement);
                        if (SoldeFacture < -50)
                        {
                            item.SOLDECLIENT = SoldeClient;
                            item.SOLDEFACTURE = SoldeFacture;
                            ListeDesFacture.Add(item);
                        }
                    }
                }
                return ListeDesFacture;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public List<CsLclient> RetourneListeFactureReg(int _CodeReg, string REFEM)
        {
            try
            {
                List<CsClient> _LstClientDuCodeRegroupement = RetourneListeDesClientsRegroupe(_CodeReg, REFEM);
                List<CsLclient> _ListeDesFactureClient = new List<CsLclient>();
                Parallel.ForEach(_LstClientDuCodeRegroupement, _LeClient =>
                {
                    _ListeDesFactureClient.AddRange(RetourneListeFactureNonSolde(_LeClient.CENTRE, _LeClient.REFCLIENT, _LeClient.ORDRE, _LeClient.PK_ID, REFEM));

                });
                return _ListeDesFactureClient;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }






        public List<CsClient> RetourneListeDesClientsRegroupe(int IdCodeRegroupe, string REFEM)
        {
            //cmd.CommandText = "SPX_ENC_LISTECLIENTREGROUPE";
            try
            {
                DataTable dt = Galatee.Entity.Model.CaisseProcedures.RetourneListeDesClientsParCodeRegroupemnt(IdCodeRegroupe, REFEM);
                return Entities.GetEntityListFromQuery<CsClient>(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public List<CsTypeTimbre> RetouneTypeTimbre()
        {
            //cmd.CommandText = "SPX_ENC_LISTECLIENTREGROUPE";
            try
            {
                DataTable dt = Galatee.Entity.Model.CommonProcedures.ListeDesTypeTimbre();
                return Entities.GetEntityListFromQuery<CsTypeTimbre>(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public CsHabilitationCaisse  RetouneLaCaisseCourante(string Matricule)
        {
            try
            {
                DataTable dt = Galatee.Entity.Model.CaisseProcedures.RetouneLaCaisseCourante(Matricule);
                List<CsHabilitationCaisse> lesCaisse = Entities.GetEntityListFromQuery<CsHabilitationCaisse>(dt);
                return  lesCaisse.OrderByDescending(t => t.DATE_DEBUT).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public CsHabilitationCaisse RetouneLaCaisseCourante(string Matricule, CsPoste poste)
        {
            try
            {
                InsererHabilitation(Matricule, poste);
                DataTable dt = Galatee.Entity.Model.CaisseProcedures.RetouneLaCaisseCourante(Matricule);
                List<CsHabilitationCaisse> lesCaisse = Entities.GetEntityListFromQuery<CsHabilitationCaisse>(dt);
                return lesCaisse.OrderByDescending(t => t.DATE_DEBUT).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        public bool LettrageAutomatique(List<CsLclient> lesReglement, galadbEntities ctx)
        {
            try
            {
                int res = -1;
                 
                    DBEncaissement db = new DBEncaissement();
                    foreach (CsLclient item in lesReglement)
                        db.InsererTranscaisB(item, ctx);
                  res=  ctx.SaveChanges();
                  return res == -1 ? false : true;
            }
            catch (Exception ex)
            {
                return false ;
            }
        }


        #endregion

    #region ADO.net
        public bool InsererEncaissement(List<CsLclient> LstDesReglementAInserer)
        {
            try
            {
                bool Resultat = false;
                if (!string.IsNullOrEmpty(LstDesReglementAInserer.FirstOrDefault().NUMDEM))
                    Resultat = EncaissementDemandeSpx(LstDesReglementAInserer);
                else
                    Resultat = EncaissementFactureSpx(LstDesReglementAInserer);
                return Resultat;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //public bool EncaissementFactureSpx(List<CsLclient> LstDesReglementAInserer)
        //{
        //    SqlCommand laCommande = DBBase.InitTransaction(ConnectionString);
        //    try
        //    {
        //        foreach (CsLclient FactureAregle in LstDesReglementAInserer)
        //            InsererPaiementFactureSpx(FactureAregle, laCommande);
        //        UpdateAcquitsPX(LstDesReglementAInserer[0].FK_IDHABILITATIONCAISSE.Value, LstDesReglementAInserer[0].MATRICULE, Convert.ToInt32(LstDesReglementAInserer[0].ACQUIT), laCommande);
        //        laCommande.Transaction.Commit();
        //        //SupprimeDoublonsCaisse(LstDesReglementAInserer[0].FK_IDHABILITATIONCAISSE.Value, LstDesReglementAInserer[0].ACQUIT);
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        laCommande.Transaction.Rollback();
        //        throw ex;
        //    }
        //    finally
        //    {
        //        if (laCommande.Connection.State == ConnectionState.Open)
        //            laCommande.Connection.Close();
        //        laCommande.Dispose();
        //    }
        //}
        public bool EncaissementFactureSpx(List<CsLclient> LstDesReglementAInserer)
        {
            SqlCommand laCommande = DBBase.InitTransaction(ConnectionString);
            try
            {

                List<CsLclient> Bulkinsert = LstDesReglementAInserer.Where(i => !i.IsPAIEMENTANTICIPE).ToList();
                if (Bulkinsert != null && Bulkinsert.Count != 0)
                    Galatee.Tools.Utility.InsertionEnBloc<CsMapperTranscaisB>(Tools.Utility.ConvertListType<CsMapperTranscaisB, CsLclient>(Bulkinsert), "TRANSCAISSE", laCommande);

                List<CsLclient> LesPaiementAnticipe = LstDesReglementAInserer.Where(i => i.IsPAIEMENTANTICIPE).ToList();
                var reglemntAnticipe = (from p in LesPaiementAnticipe
                                          group new { p } by new {p.CENTRE ,p.CLIENT ,p.ORDRE , p.FK_IDCLIENT  } into pResult
                                          select new
                                          {
                                              pResult.Key.CENTRE ,
                                              pResult.Key.CLIENT,
                                              pResult.Key.ORDRE ,
                                              pResult.Key.FK_IDCLIENT  
                                          });

                foreach (var FactureAregle in reglemntAnticipe)
                {
                    List<CsLclient> lePaiementAnticipeclient = LesPaiementAnticipe.Where(i => i.CENTRE == FactureAregle.CENTRE && i.CLIENT == FactureAregle.CLIENT &&
                                                                                              i.ORDRE == FactureAregle.ORDRE && i.FK_IDCLIENT == FactureAregle.FK_IDCLIENT).OrderBy (o => o.NDOC).ToList();
                    int IdfacturePA  = 0;
                    foreach (CsLclient item in lePaiementAnticipeclient)
                    {
                        if (item.NDOC =="TIMBRE") item.FK_IDLCLIENT  = IdfacturePA;
                        IdfacturePA = InsererPaiementAnticipeSpx(item, laCommande);
                    }
                }
                UpdateAcquitsPX(LstDesReglementAInserer[0].FK_IDHABILITATIONCAISSE.Value, LstDesReglementAInserer[0].MATRICULE, Convert.ToInt32(LstDesReglementAInserer[0].ACQUIT), laCommande);
                laCommande.Transaction.Commit();
                SupprimeDoublonsCaisse(LstDesReglementAInserer[0].FK_IDHABILITATIONCAISSE.Value, LstDesReglementAInserer[0].ACQUIT);
                return true;
            }
            catch (Exception ex)
            {
                laCommande.Transaction.Rollback();
                throw ex;
            }
            finally
            {
                if (laCommande.Connection.State == ConnectionState.Open)
                    laCommande.Connection.Close();
                laCommande.Dispose();
            }
        }
        public bool EncaissementDemandeSpx(List<CsLclient> LstDesReglementAInserer)
        {
            SqlCommand laCommande = DBBase.InitTransaction(ConnectionString);
            try
            {
                foreach (CsLclient FactureAregle in LstDesReglementAInserer)
                {
                    if (FactureAregle.NDOC == "TIMBRE") FactureAregle.FK_IDLCLIENT = LstDesReglementAInserer.FirstOrDefault(o => o.PK_ID != 0).PK_ID;
                    InsererPaiementDemandeSpx(FactureAregle, laCommande);
                }
                UpdateAcquitsPX(LstDesReglementAInserer[0].FK_IDHABILITATIONCAISSE.Value, LstDesReglementAInserer[0].MATRICULE, Convert.ToInt32(LstDesReglementAInserer[0].ACQUIT), laCommande);
                UpdateDemandesPX(LstDesReglementAInserer.First().NUMDEM, laCommande);
                if (LstDesReglementAInserer.First().TYPEDEMANDE != Enumere.AchatTimbre &&
                    LstDesReglementAInserer.First().TYPEDEMANDE != Enumere.RemboursementAvance &&
                    LstDesReglementAInserer.First().TYPEDEMANDE != Enumere.RemboursementTrvxNonRealise)
                    new DBAccueil().TransmettreDemande(LstDesReglementAInserer.First().NUMDEM, LstDesReglementAInserer.First().FK_IDETAPEDEVIS.Value, LstDesReglementAInserer.First().USERCREATION, laCommande);
                laCommande.Transaction.Commit();
                return true;
            }
            catch (Exception ex)
            {
                laCommande.Transaction.Rollback();
                throw ex;
            }
            finally
            {
                if (laCommande.Connection.State == ConnectionState.Open)
                    laCommande.Connection.Close();
                laCommande.Dispose();
            }
        }

    /*
        public string RetourneNumeroRecu(int idCaisse)
        {
            try
            {
                return Galatee.Entity.Model.FonctionCaisse.RetourneDernierNumeroRecu(idCaisse);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
*/

        public string RetourneNumeroRecu(int? idCaisse, string matricule)
        {
            cn = new SqlConnection(ConnectionString);

            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandTimeout = 1800;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SPX_CAIS_NUMERODERECU";
            if (cmd.Parameters != null && cmd.Parameters.Count != 0) cmd.Parameters.Clear();
            cmd.Parameters.Add("@idCaisse", SqlDbType.Int).Value = idCaisse == 0 ? null : idCaisse;
            cmd.Parameters.Add("@matricule", SqlDbType.VarChar, 6).Value = matricule;
            DBBase.SetDBNullParametre(cmd.Parameters);
            try
            {
                if (cn.State == ConnectionState.Closed)
                    cn.Open();
                string Numcaisse = string.Empty;
                SqlDataReader reader = cmd.ExecuteReader();
                foreach (object item in reader)
                    Numcaisse = (Convert.IsDBNull(reader["ACQUIT"])) ? String.Empty : (System.String)reader["ACQUIT"];
                reader.Close();
                return Numcaisse;
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

        public void InsererPaiementDemandeSpx(CsLclient LstDesReglementAInserer, SqlCommand cmds)
        {


            cmds.CommandTimeout = 3000;
            cmds.CommandType = CommandType.StoredProcedure;
            cmds.CommandText = "SPX_CAIS_INSERERDEMANDE";
            if (cmds.Parameters != null && cmds.Parameters.Count != 0) cmds.Parameters.Clear();
            cmds.Parameters.Add("@CENTRE", SqlDbType.VarChar, Enumere.TailleCentre).Value = LstDesReglementAInserer.CENTRE;
            cmds.Parameters.Add("@CLIENT", SqlDbType.VarChar, Enumere.TailleClient).Value = LstDesReglementAInserer.CLIENT;
            cmds.Parameters.Add("@ORDRE", SqlDbType.VarChar, Enumere.TailleOrdre).Value = LstDesReglementAInserer.ORDRE;
            cmds.Parameters.Add("@CAISSE", SqlDbType.VarChar, 3).Value = LstDesReglementAInserer.CAISSE;
            cmds.Parameters.Add("@ACQUIT", SqlDbType.VarChar, 9).Value = LstDesReglementAInserer.ACQUIT;
            cmds.Parameters.Add("@MATRICULE", SqlDbType.VarChar, 6).Value = LstDesReglementAInserer.MATRICULE;
            cmds.Parameters.Add("@NDOC", SqlDbType.VarChar, 6).Value = LstDesReglementAInserer.NDOC;
            cmds.Parameters.Add("@REFEM", SqlDbType.VarChar, 6).Value = LstDesReglementAInserer.REFEM;
            cmds.Parameters.Add("@MONTANT", SqlDbType.Decimal).Value = LstDesReglementAInserer.MONTANT;
            cmds.Parameters.Add("@MONTANTTVA", SqlDbType.Decimal).Value = LstDesReglementAInserer.MONTANTTVA;
            cmds.Parameters.Add("@DC", SqlDbType.VarChar, 1).Value = LstDesReglementAInserer.DC;
            cmds.Parameters.Add("@COPER", SqlDbType.VarChar, 3).Value = LstDesReglementAInserer.COPER;
            cmds.Parameters.Add("@PERCU", SqlDbType.Decimal).Value = LstDesReglementAInserer.PERCU;
            cmds.Parameters.Add("@RENDU", SqlDbType.Decimal).Value = LstDesReglementAInserer.RENDU;
            cmds.Parameters.Add("@MODEREG", SqlDbType.VarChar, 1).Value = LstDesReglementAInserer.MODEREG;
            cmds.Parameters.Add("@DTRANS", SqlDbType.DateTime).Value = LstDesReglementAInserer.DTRANS;
            cmds.Parameters.Add("@BANQUE", SqlDbType.VarChar, 6).Value = LstDesReglementAInserer.BANQUE;
            cmds.Parameters.Add("@TOPANNUL", SqlDbType.VarChar, 1).Value = LstDesReglementAInserer.TOPANNUL;
            cmds.Parameters.Add("@MOISCOMPT", SqlDbType.VarChar, 6).Value = LstDesReglementAInserer.MOISCOMPT;
            cmds.Parameters.Add("@TOP1", SqlDbType.VarChar, 2).Value = LstDesReglementAInserer.TOP1;
            cmds.Parameters.Add("@NUMDEM", SqlDbType.VarChar, 20).Value = LstDesReglementAInserer.NUMDEM;
            cmds.Parameters.Add("@NUMCHEQ", SqlDbType.VarChar, 10).Value = LstDesReglementAInserer.NUMCHEQ;
            cmds.Parameters.Add("@SAISIPAR", SqlDbType.VarChar, 6).Value = LstDesReglementAInserer.SAISIPAR;
            cmds.Parameters.Add("@DATEENCAISSEMENT", SqlDbType.DateTime).Value = LstDesReglementAInserer.DTRANS;
            cmds.Parameters.Add("@USERCREATION", SqlDbType.VarChar, 20).Value = LstDesReglementAInserer.MATRICULE;
            cmds.Parameters.Add("@DATECREATION", SqlDbType.DateTime).Value = LstDesReglementAInserer.DATECREATION;
            cmds.Parameters.Add("@DATEMODIFICATION", SqlDbType.DateTime).Value = null;
            cmds.Parameters.Add("@USERMODIFICATION", SqlDbType.VarChar, 6).Value = string.Empty;
            cmds.Parameters.Add("@FK_IDCENTRE", SqlDbType.Int).Value = LstDesReglementAInserer.FK_IDCENTRE;
            cmds.Parameters.Add("@FK_IDLCLIENT", SqlDbType.Int).Value = LstDesReglementAInserer.FK_IDLCLIENT;
            cmds.Parameters.Add("@FK_IDHABILITATIONCAISSE", SqlDbType.Int).Value = LstDesReglementAInserer.FK_IDHABILITATIONCAISSE;
            cmds.Parameters.Add("@FK_IDMODEREG", SqlDbType.Int).Value = LstDesReglementAInserer.FK_IDMODEREG;
            cmds.Parameters.Add("@FK_IDLIBELLETOP", SqlDbType.Int).Value = LstDesReglementAInserer.FK_IDLIBELLETOP;
            cmds.Parameters.Add("@FK_IDCAISSIERE", SqlDbType.Int).Value = LstDesReglementAInserer.FK_IDCAISSIERE;
            cmds.Parameters.Add("@FK_IDAGENTSAISIE", SqlDbType.Int).Value = LstDesReglementAInserer.FK_IDAGENTSAISIE;
            cmds.Parameters.Add("@FK_IDCOPER", SqlDbType.Int).Value = LstDesReglementAInserer.FK_IDCOPER;
            cmds.Parameters.Add("@POSTE", SqlDbType.VarChar, 50).Value = LstDesReglementAInserer.POSTE;
            cmds.Parameters.Add("@DATETRANS", SqlDbType.DateTime).Value = LstDesReglementAInserer.DATEENCAISSEMENT;
            cmds.Parameters.Add("@FK_IDCLIENT", SqlDbType.Int).Value = LstDesReglementAInserer.FK_IDCLIENT;
            cmds.Parameters.Add("@TYPEDEMANDE", SqlDbType.VarChar, 3).Value = LstDesReglementAInserer.TYPEDEMANDE;
            SqlParameter outResult = new SqlParameter("@PK_ID", SqlDbType.VarChar, int.MaxValue) { Direction = ParameterDirection.Output };
            cmds.Parameters.Add(outResult);
            string id = string.Empty;
            DBBase.SetDBNullParametre(cmds.Parameters);
            try
            {
                if (cmds.Connection.State == ConnectionState.Closed)
                    cmds.Connection.Open();
                cmds.ExecuteNonQuery();
                id = outResult.Value != null ? outResult.Value.ToString() : "1";
                LstDesReglementAInserer.PK_ID = int.Parse(id);
            }
            catch (Exception ex)
            {
                throw new Exception(cmds.CommandText + ":" + ex.Message);
            }
        }
        public bool InsererHabilitation(string matricule, CsPoste poste)
        {
          
                cn = new SqlConnection(ConnectionString);

                SqlCommand cmds = new SqlCommand();
                cmds.Connection = cn;


            cmds.CommandTimeout = 3000;
            cmds.CommandType = CommandType.StoredProcedure;
            cmds.CommandText = "SPX_CAIS_GET_HABILITATIONCAISSE";
            if (cmds.Parameters != null && cmds.Parameters.Count != 0) cmds.Parameters.Clear();
            cmds.Parameters.Add("@NUMCAISSE", SqlDbType.VarChar, 3).Value = poste.NUMCAISSE;
            cmds.Parameters.Add("@MATRICULE", SqlDbType.VarChar, 5).Value = matricule;
            cmds.Parameters.Add("@POSTE", SqlDbType.VarChar, 50).Value = poste.NOMPOSTE;
            cmds.Parameters.Add("@CENTRE", SqlDbType.VarChar, 3).Value = poste.CODECENTRE;
            cmds.Parameters.Add("@FK_IDCENTRE", SqlDbType.Int).Value = poste.FK_IDCENTRE;
            cmds.Parameters.Add("@FK_IDCAISSE", SqlDbType.Int).Value = poste.FK_IDCAISSE;

            DBBase.SetDBNullParametre(cmds.Parameters);
            try
            {
                if (cmds.Connection.State == ConnectionState.Closed)
                    cmds.Connection.Open();
                int reader = -1;
                reader = cmds.ExecuteNonQuery();
                return reader == -1 ? false : true;

            }
            catch (Exception ex)
            {
                throw new Exception(cmds.CommandText + ":" + ex.Message);
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                    cn.Close(); // Fermeture de la connection 
                cmds.Dispose();
            }
        }
        public int InsererPaiementAnticipeSpx(CsLclient LstDesReglementAInserer, SqlCommand cmds)
        {
            cmds.CommandTimeout = 3000;
            cmds.CommandType = CommandType.StoredProcedure;
            cmds.CommandText = "SPX_CAIS_INSERERPAIEMENTANTICIPE";
            if (cmds.Parameters != null && cmds.Parameters.Count != 0) cmds.Parameters.Clear();
            cmds.Parameters.Add("@CENTRE", SqlDbType.VarChar, Enumere.TailleCentre).Value = LstDesReglementAInserer.CENTRE;
            cmds.Parameters.Add("@CLIENT", SqlDbType.VarChar, Enumere.TailleClient).Value = LstDesReglementAInserer.CLIENT;
            cmds.Parameters.Add("@ORDRE", SqlDbType.VarChar, Enumere.TailleOrdre).Value = LstDesReglementAInserer.ORDRE;
            cmds.Parameters.Add("@CAISSE", SqlDbType.VarChar, 3).Value = LstDesReglementAInserer.CAISSE;
            cmds.Parameters.Add("@ACQUIT", SqlDbType.VarChar, 9).Value = LstDesReglementAInserer.ACQUIT;
            cmds.Parameters.Add("@MATRICULE", SqlDbType.VarChar, 6).Value = LstDesReglementAInserer.MATRICULE;
            cmds.Parameters.Add("@NDOC", SqlDbType.VarChar, 6).Value = LstDesReglementAInserer.NDOC;
            cmds.Parameters.Add("@REFEM", SqlDbType.VarChar, 6).Value = LstDesReglementAInserer.REFEM;
            cmds.Parameters.Add("@MONTANT", SqlDbType.Decimal).Value = LstDesReglementAInserer.MONTANT;
            cmds.Parameters.Add("@DC", SqlDbType.VarChar, 1).Value = LstDesReglementAInserer.DC;
            cmds.Parameters.Add("@COPER", SqlDbType.VarChar, 3).Value = LstDesReglementAInserer.COPER;
            cmds.Parameters.Add("@PERCU", SqlDbType.Decimal).Value = LstDesReglementAInserer.PERCU;
            //cmds.Parameters.Add("@RENDU", SqlDbType.Decimal).Value = LstDesReglementAInserer.REFEM;
            cmds.Parameters.Add("@RENDU", SqlDbType.Decimal).Value = LstDesReglementAInserer.RENDU;
            cmds.Parameters.Add("@MODEREG", SqlDbType.VarChar, 1).Value = LstDesReglementAInserer.MODEREG;
            cmds.Parameters.Add("@DTRANS", SqlDbType.DateTime).Value = LstDesReglementAInserer.DTRANS;
            cmds.Parameters.Add("@BANQUE", SqlDbType.VarChar, 6).Value = LstDesReglementAInserer.BANQUE;
            cmds.Parameters.Add("@TOPANNUL", SqlDbType.VarChar, 1).Value = LstDesReglementAInserer.TOPANNUL;
            cmds.Parameters.Add("@MOISCOMPT", SqlDbType.VarChar, 6).Value = LstDesReglementAInserer.MOISCOMPT;
            cmds.Parameters.Add("@TOP1", SqlDbType.VarChar, 2).Value = LstDesReglementAInserer.TOP1;
            cmds.Parameters.Add("@NUMDEM", SqlDbType.VarChar, 20).Value = LstDesReglementAInserer.NUMDEM;
            cmds.Parameters.Add("@NUMCHEQ", SqlDbType.VarChar, 10).Value = LstDesReglementAInserer.NUMCHEQ;
            cmds.Parameters.Add("@SAISIPAR", SqlDbType.VarChar, 6).Value = LstDesReglementAInserer.SAISIPAR;
            cmds.Parameters.Add("@DATEENCAISSEMENT", SqlDbType.DateTime).Value = LstDesReglementAInserer.DTRANS;
            cmds.Parameters.Add("@USERCREATION", SqlDbType.VarChar, 20).Value = LstDesReglementAInserer.MATRICULE;
            cmds.Parameters.Add("@DATECREATION", SqlDbType.DateTime).Value = LstDesReglementAInserer.DATECREATION;
            cmds.Parameters.Add("@DATEMODIFICATION", SqlDbType.DateTime).Value = null;
            cmds.Parameters.Add("@USERMODIFICATION", SqlDbType.VarChar, 6).Value = string.Empty;
            cmds.Parameters.Add("@FK_IDCENTRE", SqlDbType.Int).Value = LstDesReglementAInserer.FK_IDCENTRE;
            cmds.Parameters.Add("@FK_IDLCLIENT", SqlDbType.Int).Value = LstDesReglementAInserer.FK_IDLCLIENT ;
            cmds.Parameters.Add("@FK_IDHABILITATIONCAISSE", SqlDbType.Int).Value = LstDesReglementAInserer.FK_IDHABILITATIONCAISSE;
            cmds.Parameters.Add("@FK_IDMODEREG", SqlDbType.Int).Value = LstDesReglementAInserer.FK_IDMODEREG;
            cmds.Parameters.Add("@FK_IDLIBELLETOP", SqlDbType.Int).Value = LstDesReglementAInserer.FK_IDLIBELLETOP;
            cmds.Parameters.Add("@FK_IDCAISSIERE", SqlDbType.Int).Value = LstDesReglementAInserer.FK_IDCAISSIERE;
            cmds.Parameters.Add("@FK_IDAGENTSAISIE", SqlDbType.Int).Value = LstDesReglementAInserer.FK_IDAGENTSAISIE;
            cmds.Parameters.Add("@FK_IDCOPER", SqlDbType.Int).Value = LstDesReglementAInserer.FK_IDCOPER;
            cmds.Parameters.Add("@POSTE", SqlDbType.VarChar, 50).Value = LstDesReglementAInserer.POSTE;
            cmds.Parameters.Add("@DATETRANS", SqlDbType.DateTime).Value = LstDesReglementAInserer.DATEENCAISSEMENT;
            cmds.Parameters.Add("@FK_IDCLIENT", SqlDbType.Int).Value = LstDesReglementAInserer.FK_IDCLIENT;
            cmds.Parameters.Add("@IsPAIEMENTANTICIPE", SqlDbType.Bit).Value = LstDesReglementAInserer.IsPAIEMENTANTICIPE;
            SqlParameter outResult = new SqlParameter("@PK_ID", SqlDbType.Int, int.MaxValue) { Direction = ParameterDirection.Output   };
            cmds.Parameters.Add(outResult);
            DBBase.SetDBNullParametre(cmds.Parameters);

            try
            {
                int IdDemande = 0;
                if (cmds.Connection.State == ConnectionState.Closed)
                    cmds.Connection.Open();
                cmds.ExecuteNonQuery();
                IdDemande = outResult.Value != null ? Convert.ToInt32(outResult.Value) :0;
                return IdDemande;
            }
            catch (Exception ex)
            {
                throw new Exception(cmds.CommandText + ":" + ex.Message);
            }
        }
        public bool InsererPaiementFactureSpx(CsLclient LstDesReglementAInserer, SqlCommand cmds)
        {


            //cmd = new SqlCommand();
            //cmd.Connection = cn;
            cmds.CommandTimeout = 3000;
            cmds.CommandType = CommandType.StoredProcedure;
            cmds.CommandText = "SPX_CAIS_INSERERTRANSCAISS";
            if (cmds.Parameters != null && cmds.Parameters.Count != 0) cmds.Parameters.Clear();
            cmds.Parameters.Add("@CENTRE", SqlDbType.VarChar, Enumere.TailleCentre).Value = LstDesReglementAInserer.CENTRE;
            cmds.Parameters.Add("@CLIENT", SqlDbType.VarChar, Enumere.TailleClient).Value = LstDesReglementAInserer.CLIENT;
            cmds.Parameters.Add("@ORDRE", SqlDbType.VarChar, Enumere.TailleOrdre).Value = LstDesReglementAInserer.ORDRE;
            cmds.Parameters.Add("@CAISSE", SqlDbType.VarChar, 3).Value = LstDesReglementAInserer.CAISSE;
            cmds.Parameters.Add("@ACQUIT", SqlDbType.VarChar, 9).Value = LstDesReglementAInserer.ACQUIT;
            cmds.Parameters.Add("@MATRICULE", SqlDbType.VarChar, 6).Value = LstDesReglementAInserer.MATRICULE;
            cmds.Parameters.Add("@NDOC", SqlDbType.VarChar, 6).Value = LstDesReglementAInserer.NDOC;
            cmds.Parameters.Add("@REFEM", SqlDbType.VarChar, 6).Value = LstDesReglementAInserer.REFEM;
            cmds.Parameters.Add("@MONTANT", SqlDbType.Decimal).Value = LstDesReglementAInserer.MONTANT;
            cmds.Parameters.Add("@DC", SqlDbType.VarChar, 1).Value = LstDesReglementAInserer.DC;
            cmds.Parameters.Add("@COPER", SqlDbType.VarChar, 3).Value = LstDesReglementAInserer.COPER;
            cmds.Parameters.Add("@PERCU", SqlDbType.Decimal).Value = LstDesReglementAInserer.PERCU;
            //cmds.Parameters.Add("@RENDU", SqlDbType.Decimal).Value = LstDesReglementAInserer.REFEM;
            cmds.Parameters.Add("@RENDU", SqlDbType.Decimal).Value = LstDesReglementAInserer.RENDU;
            cmds.Parameters.Add("@MODEREG", SqlDbType.VarChar, 1).Value = LstDesReglementAInserer.MODEREG;
            cmds.Parameters.Add("@DTRANS", SqlDbType.DateTime).Value = LstDesReglementAInserer.DTRANS;
            cmds.Parameters.Add("@BANQUE", SqlDbType.VarChar, 6).Value = LstDesReglementAInserer.BANQUE;
            cmds.Parameters.Add("@TOPANNUL", SqlDbType.VarChar, 1).Value = LstDesReglementAInserer.TOPANNUL;
            cmds.Parameters.Add("@MOISCOMPT", SqlDbType.VarChar, 6).Value = LstDesReglementAInserer.MOISCOMPT;
            cmds.Parameters.Add("@TOP1", SqlDbType.VarChar, 2).Value = LstDesReglementAInserer.TOP1;
            cmds.Parameters.Add("@NUMDEM", SqlDbType.VarChar, 20).Value = LstDesReglementAInserer.NUMDEM;
            cmds.Parameters.Add("@NUMCHEQ", SqlDbType.VarChar, 10).Value = LstDesReglementAInserer.NUMCHEQ;
            cmds.Parameters.Add("@SAISIPAR", SqlDbType.VarChar, 6).Value = LstDesReglementAInserer.SAISIPAR;
            cmds.Parameters.Add("@DATEENCAISSEMENT", SqlDbType.DateTime).Value = LstDesReglementAInserer.DTRANS;
            cmds.Parameters.Add("@USERCREATION", SqlDbType.VarChar, 20).Value = LstDesReglementAInserer.MATRICULE;
            cmds.Parameters.Add("@DATECREATION", SqlDbType.DateTime).Value = LstDesReglementAInserer.DATECREATION;
            cmds.Parameters.Add("@DATEMODIFICATION", SqlDbType.DateTime).Value = null;
            cmds.Parameters.Add("@USERMODIFICATION", SqlDbType.VarChar, 6).Value = string.Empty;
            cmds.Parameters.Add("@FK_IDCENTRE", SqlDbType.Int).Value = LstDesReglementAInserer.FK_IDCENTRE;
            //cmds.Parameters.Add("@FK_IDLCLIENT", SqlDbType.Int).Value = LstDesReglementAInserer.FK_IDCLIENT;
            cmds.Parameters.Add("@FK_IDLCLIENT", SqlDbType.Int).Value = LstDesReglementAInserer.FK_IDLCLIENT;
            cmds.Parameters.Add("@FK_IDHABILITATIONCAISSE", SqlDbType.Int).Value = LstDesReglementAInserer.FK_IDHABILITATIONCAISSE;
            cmds.Parameters.Add("@FK_IDMODEREG", SqlDbType.Int).Value = LstDesReglementAInserer.FK_IDMODEREG;
            cmds.Parameters.Add("@FK_IDLIBELLETOP", SqlDbType.Int).Value = LstDesReglementAInserer.FK_IDLIBELLETOP;
            cmds.Parameters.Add("@FK_IDCAISSIERE", SqlDbType.Int).Value = LstDesReglementAInserer.FK_IDCAISSIERE;
            cmds.Parameters.Add("@FK_IDAGENTSAISIE", SqlDbType.Int).Value = LstDesReglementAInserer.FK_IDAGENTSAISIE;
            cmds.Parameters.Add("@FK_IDCOPER", SqlDbType.Int).Value = LstDesReglementAInserer.FK_IDCOPER;
            cmds.Parameters.Add("@POSTE", SqlDbType.VarChar, 50).Value = LstDesReglementAInserer.POSTE;
            cmds.Parameters.Add("@DATETRANS", SqlDbType.DateTime).Value = LstDesReglementAInserer.DATEENCAISSEMENT;
            cmds.Parameters.Add("@FK_IDCLIENT", SqlDbType.Int).Value = LstDesReglementAInserer.FK_IDCLIENT;
            cmds.Parameters.Add("@IsPAIEMENTANTICIPE", SqlDbType.Bit).Value = LstDesReglementAInserer.IsPAIEMENTANTICIPE;

            DBBase.SetDBNullParametre(cmds.Parameters);
            try
            {
                if (cmds.Connection.State == ConnectionState.Closed)
                    cmds.Connection.Open();
                int reader = -1;
                reader = cmds.ExecuteNonQuery();
                return reader == -1 ? false : true;

            }
            catch (Exception ex)
            {
                throw new Exception(cmds.CommandText + ":" + ex.Message);
            }
            //finally
            //{
            //    cmd.Dispose();
            //}
        }
        public void UpdateAcquitsPX(int CaisseHail, string matricule, int acquit, SqlCommand cmdh)
        {


            //cmd = new SqlCommand();
            //cmd.Connection = cn;
            cmdh.CommandTimeout = 3000;
            if (cmdh.Parameters != null && cmdh.Parameters.Count != 0) cmdh.Parameters.Clear();
            cmdh.CommandType = CommandType.StoredProcedure;
            cmdh.CommandText = "SPX_CAIS_UPDATEACQUITCAISSE";
            cmdh.Parameters.Add("@idHabiliationCaisse", SqlDbType.Int).Value = CaisseHail;
            cmdh.Parameters.Add("@Acquitement", SqlDbType.Int).Value = acquit;

            DBBase.SetDBNullParametre(cmdh.Parameters);
            try
            {
                if (cmdh.Connection.State == ConnectionState.Closed)
                    cmdh.Connection.Open();
                cmdh.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception(cmdh.CommandText + ":" + ex.Message);
            }


        }
        public void UpdateDemandesPX(string NumDemande, SqlCommand cmdh)
        {
            cmdh.CommandTimeout = 3000;
            if (cmdh.Parameters != null && cmdh.Parameters.Count != 0) cmdh.Parameters.Clear();
            cmdh.CommandType = CommandType.StoredProcedure;
            cmdh.CommandText = "SPX_CAIS_UPDATEDEMANDE";
            cmdh.Parameters.Add("@NUMDEM", SqlDbType.VarChar, 20).Value = NumDemande;

            DBBase.SetDBNullParametre(cmdh.Parameters);
            try
            {
                if (cmdh.Connection.State == ConnectionState.Closed)
                    cmdh.Connection.Open();
                cmdh.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception(cmdh.CommandText + ":" + ex.Message);
            }


        }
        public List<CsLclient> RetourneDemandeSpx(string NumDemande, bool EstExtension)
        {
            try
            {
                cn = new SqlConnection(ConnectionString);

                cmd = new SqlCommand();
                cmd.Connection = cn;
                cmd.CommandTimeout = 3000;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SPX_CAIS_RETOURNEDEMANDE";
                cmd.Parameters.Add("@NumDemande", SqlDbType.VarChar, 20).Value = NumDemande;
                DBBase.SetDBNullParametre(cmd.Parameters);
                try
                {
                    if (cn.State == ConnectionState.Closed)
                        cn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(reader);
                    return Entities.GetEntityListFromQuery<CsLclient>(dt); ;
                }
                catch (Exception ex)
                {
                    return null;
                }
                finally
                {
                    if (cn.State == ConnectionState.Open)
                        cn.Close(); // Fermeture de la connection 
                    cmd.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CsLclient> ListeFactureNonSolde(string centre, string client, string ordre, int idCentre)
        {
            try
            {
                cn = new SqlConnection(ConnectionString);

                cmd = new SqlCommand();
                cmd.Connection = cn;
                cmd.CommandTimeout = 3000;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SPX_IMPAYES_CLIENT";
                cmd.Parameters.Add("@centre", SqlDbType.VarChar, 3).Value = centre;
                cmd.Parameters.Add("@client", SqlDbType.VarChar, 20).Value = client;
                cmd.Parameters.Add("@ordre", SqlDbType.VarChar, 2).Value = ordre;
                cmd.Parameters.Add("@idcentre", SqlDbType.Int).Value = idCentre;
                DBBase.SetDBNullParametre(cmd.Parameters);
                try
                {
                    if (cn.State == ConnectionState.Closed)
                        cn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(reader);
                    return Entities.GetEntityListFromQuery<CsLclient>(dt); ;
                }
                catch (Exception ex)
                {
                    return null;
                }
                finally
                {
                    if (cn.State == ConnectionState.Open)
                        cn.Close(); // Fermeture de la connection 
                    cmd.Dispose();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public void AnnulerRecu(string Acquit,string Matricule,int IdHabilitationCaisse, SqlCommand cmdh)
        {
            cmdh.CommandTimeout = 3000;
            if (cmdh.Parameters != null && cmdh.Parameters.Count != 0) cmdh.Parameters.Clear();
            cmdh.CommandType = CommandType.StoredProcedure;
            cmdh.CommandText = "SPX_CAIS_VALIDEANNULATION";
            cmdh.Parameters.Add("@ACQUIT", SqlDbType.VarChar, 9).Value = Acquit;
            cmdh.Parameters.Add("@IDHABILITATIONCAISSE", SqlDbType.Int ).Value = IdHabilitationCaisse;
            cmdh.Parameters.Add("@MATRICULE", SqlDbType.VarChar ,6 ).Value = Matricule;

            DBBase.SetDBNullParametre(cmdh.Parameters);
            try
            {
                if (cmdh.Connection.State == ConnectionState.Closed)
                    cmdh.Connection.Open();
                cmdh.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception(cmdh.CommandText + ":" + ex.Message);
            }


        }

        //26-01-2019  : Stephen
        public List<CsCaisse> ListeCaisseDisponible(string Centre, string matricule)
        {
            try
            {
                //DataTable dt = CommonProcedures.RetourneCaisseDisponible(Centre, matricule);
                DataTable dt = DB_ParametresGeneraux.SelectAllDonneReference("CAISSE");
                return Entities.GetEntityListFromQuery<CsCaisse>(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CsLibelleTop> RetourneTousLibelleTop()
        {
            try
            {
                //DataTable dt = Galatee.Entity.Model.CommonProcedures.RetourneTousLibelleTop();
                DataTable dt = DB_ParametresGeneraux.SelectAllDonneReference("LIBELLETOP");
                return Entities.GetEntityListFromQuery<CsLibelleTop>(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public CsParametresGeneraux RetourneListeTa58(string CodeTable)
        {
            try
            {
                //DataTable dt = Galatee.Entity.Model.CommonProcedures.RetourneTousParametreGenerauxByCode(CodeTable);
                DataTable dt = DB_ParametresGeneraux.SelectAllDonneReference("PARAMETRESGENERAUX");
                List<CsParametresGeneraux> result = Entities.GetEntityListFromQuery<CsParametresGeneraux>(dt);
                return result.FirstOrDefault(i => i.CODE == CodeTable);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /* LKO 31/01/2019 */
        public List<CsClient> RetourneClient(string centre, string client, string ordre)
        {

            try
            {
                cn = new SqlConnection(ConnectionString);

                cmd = new SqlCommand();
                cmd.Connection = cn;
                cmd.CommandTimeout = 3000;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SPX_CAIS_RETOURNECLIENTBYREFERENCE";
                cmd.Parameters.Add("@centre", SqlDbType.VarChar, 3).Value = centre;
                cmd.Parameters.Add("@client", SqlDbType.VarChar, 20).Value = client;
                cmd.Parameters.Add("@ordre", SqlDbType.VarChar, 2).Value = ordre;
                DBBase.SetDBNullParametre(cmd.Parameters);
                try
                {
                    if (cn.State == ConnectionState.Closed)
                        cn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(reader);
                    return Entities.GetEntityListFromQuery<CsClient>(dt); ;
                }
                catch (Exception ex)
                {
                    return null;
                }
                finally
                {
                    if (cn.State == ConnectionState.Open)
                        cn.Close(); // Fermeture de la connection 
                    cmd.Dispose();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public List<CsLclient> RetourneFactureClientRegroupe(List<int> idRegroupement, List<string> Periode)
        {

            try
            {
                cn = new SqlConnection(ConnectionString);
                string LstPeriode = DBBase.RetourneStringListeObject(Periode);
                string LstRegroupement = DBBase.RetourneStringListeObject(idRegroupement);
                cmd = new SqlCommand();
                cmd.Connection = cn;
                cmd.CommandTimeout = 3000;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SPX_CAIS_RETOURNE_FACTURE_CLIENT_REGROUPE";
                cmd.Parameters.Add("@IDREGROUPEMENT", SqlDbType.VarChar, int.MaxValue).Value = LstRegroupement;
                cmd.Parameters.Add("@PERIODE", SqlDbType.VarChar, int.MaxValue).Value = Periode;
                DBBase.SetDBNullParametre(cmd.Parameters);
                try
                {
                    if (cn.State == ConnectionState.Closed)
                        cn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(reader);
                    return Entities.GetEntityListFromQuery<CsLclient>(dt); ;
                }
                catch (Exception ex)
                {
                    return null;
                }
                finally
                {
                    if (cn.State == ConnectionState.Open)
                        cn.Close(); // Fermeture de la connection 
                    cmd.Dispose();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public List<CsLclient> RetourneFactureClientRegroupe(List<int> idRegroupement, List<string> Periode,List<int> idproduit)
        {

            try
            {
                cn = new SqlConnection(ConnectionString);
                string LstPeriode = DBBase.RetourneStringListeObject(Periode);
                string LstRegroupement = DBBase.RetourneStringListeObject(idRegroupement);
                string LstProduit = DBBase.RetourneStringListeObject(idproduit);
                cmd = new SqlCommand();
                cmd.Connection = cn;
                cmd.CommandTimeout = 3000;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SPX_CAIS_RETOURNE_FACTURE_CLIENT_REGROUPE_PRODUIT";
                cmd.Parameters.Add("@IDREGROUPEMENT", SqlDbType.VarChar, int.MaxValue).Value = LstRegroupement;
                cmd.Parameters.Add("@PERIODE", SqlDbType.VarChar, int.MaxValue).Value = LstPeriode;
                cmd.Parameters.Add("@IDPRODUIT", SqlDbType.VarChar, int.MaxValue).Value = LstProduit;
                
                DBBase.SetDBNullParametre(cmd.Parameters);
                try
                {
                    if (cn.State == ConnectionState.Closed)
                        cn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(reader);
                    return Entities.GetEntityListFromQuery<CsLclient>(dt); ;
                }
                catch (Exception ex)
                {
                    return null;
                }
                finally
                {
                    if (cn.State == ConnectionState.Open)
                        cn.Close(); // Fermeture de la connection 
                    cmd.Dispose();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public List<CsLclient> RetourneFactureClientNonSolde(int id, string centre, string client, string ordre)
        {

            try
            {
                cn = new SqlConnection(ConnectionString);

                cmd = new SqlCommand();
                cmd.Connection = cn;
                cmd.CommandTimeout = 3000;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SPX_CAIS_RETOURNE_FACTURE_CLIENT";
                cmd.Parameters.Add("@FK_IDCLENT", SqlDbType.Int).Value = id;
                DBBase.SetDBNullParametre(cmd.Parameters);
                try
                {
                    if (cn.State == ConnectionState.Closed)
                        cn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(reader);
                    return Entities.GetEntityListFromQuery<CsLclient>(dt); ;
                }
                catch (Exception ex)
                {
                    return null;
                }
                finally
                {
                    if (cn.State == ConnectionState.Open)
                        cn.Close(); // Fermeture de la connection 
                    cmd.Dispose();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public List<CsLclient> RetourneReglementClient(int idcentre, string centre, string client, string ordre)
        {

            try
            {
                cn = new SqlConnection(ConnectionString);

                cmd = new SqlCommand();
                cmd.Connection = cn;
                cmd.CommandTimeout = 3000;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SPX_CAIS_RETOURNE_REGLEMENT_CLIENT";
                cmd.Parameters.Add("@FK_IDCENTRE", SqlDbType.Int).Value = idcentre ;
                cmd.Parameters.Add("@CENTRE", SqlDbType.VarChar,3).Value = centre;
                cmd.Parameters.Add("@CLENT", SqlDbType.VarChar ,20).Value = client ;
                cmd.Parameters.Add("@ORDRE", SqlDbType.VarChar ,2).Value = ordre;


                DBBase.SetDBNullParametre(cmd.Parameters);
                try
                {
                    if (cn.State == ConnectionState.Closed)
                        cn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(reader);
                    return Entities.GetEntityListFromQuery<CsLclient>(dt); ;
                }
                catch (Exception ex)
                {
                    return null;
                }
                finally
                {
                    if (cn.State == ConnectionState.Open)
                        cn.Close(); // Fermeture de la connection 
                    cmd.Dispose();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public CsHabilitationCaisse RetourneCaisseEnCours(int IdNumCaisse, int IdCaissier, DateTime DateDebut)
        {

            try
            {
                cn = new SqlConnection(ConnectionString);

                cmd = new SqlCommand();
                cmd.Connection = cn;
                cmd.CommandTimeout = 3000;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SPX_CAIS_RETOURNE_CAISSE_EN_COURS";
                cmd.Parameters.Add("@IDNUMCAISSE", SqlDbType.Int).Value = IdNumCaisse;
                cmd.Parameters.Add("@IDCAISSIER", SqlDbType.Int).Value = IdCaissier;
                cmd.Parameters.Add("@DATEDEBUT", SqlDbType.Date).Value = DateDebut;
                DBBase.SetDBNullParametre(cmd.Parameters);
                try
                {
                    if (cn.State == ConnectionState.Closed)
                        cn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(reader);
                    return Entities.GetEntityFromQuery<CsHabilitationCaisse>(dt); ;
                }
                catch (Exception ex)
                {
                    return null;
                }
                finally
                {
                    if (cn.State == ConnectionState.Open)
                        cn.Close(); // Fermeture de la connection 
                    cmd.Dispose();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public List<CsLclient> RetourneFactureClientNegative(int id, string centre, string client, string ordre)
        {

            try
            {
                cn = new SqlConnection(ConnectionString);

                cmd = new SqlCommand();
                cmd.Connection = cn;
                cmd.CommandTimeout = 3000;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SPX_CAIS_RETOURNE_FACTURE_CLIENT_NEGATIVE";
                cmd.Parameters.Add("@FK_IDCLENT", SqlDbType.Int).Value = id;
                DBBase.SetDBNullParametre(cmd.Parameters);
                try
                {
                    if (cn.State == ConnectionState.Closed)
                        cn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(reader);
                    return Entities.GetEntityListFromQuery<CsLclient>(dt); ;
                }
                catch (Exception ex)
                {
                    return null;
                }
                finally
                {
                    if (cn.State == ConnectionState.Open)
                        cn.Close(); // Fermeture de la connection 
                    cmd.Dispose();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public CsHabilitationCaisse HabiliterCaisse(CsHabilitationCaisse laCaisseHAbil)
        {
            try
            {
                cn = new SqlConnection(ConnectionString);

                cmd = new SqlCommand();
                cmd.Connection = cn;
                cmd.CommandTimeout = 3000;
                cmd.CommandType = CommandType.StoredProcedure;
                if (cmd.Parameters != null && cmd.Parameters.Count != 0) cmd.Parameters.Clear();

                cmd.CommandText = "SPX_CAIS_HABILITER_CAISSE";
                cmd.Parameters.Add("@NUMCAISSE", SqlDbType.VarChar, 3).Value = laCaisseHAbil.NUMCAISSE ;
                cmd.Parameters.Add("@MATRICULE", SqlDbType.VarChar, 10).Value = laCaisseHAbil.MATRICULE ;
                cmd.Parameters.Add("@DATE_DEBUT", SqlDbType.DateTime).Value = laCaisseHAbil.DATE_DEBUT;
                cmd.Parameters.Add("@DATE_FIN", SqlDbType.DateTime).Value = laCaisseHAbil.DATE_FIN;
                cmd.Parameters.Add("@POSTE", SqlDbType.VarChar, 50).Value = laCaisseHAbil.POSTE ;
                cmd.Parameters.Add("@CENTRE", SqlDbType.VarChar, 5).Value = laCaisseHAbil.CENTRE;
                cmd.Parameters.Add("@FK_IDCENTRE", SqlDbType.Int).Value = laCaisseHAbil.FK_IDCENTRE;
                cmd.Parameters.Add("@FK_IDCAISSE", SqlDbType.Int).Value = laCaisseHAbil.FK_IDCAISSE;
                cmd.Parameters.Add("@FK_IDCAISSIERE", SqlDbType.Int).Value = laCaisseHAbil.FK_IDCAISSIERE;
                cmd.Parameters.Add("@MONTANTENCAISSE", SqlDbType.Decimal).Value = laCaisseHAbil.MONTANTENCAISSE ;
                SqlParameter outResult = new SqlParameter("@PK_ID", SqlDbType.VarChar, int.MaxValue) { Direction = ParameterDirection.Output };
                outResult.Value = laCaisseHAbil.PK_ID;
                cmd.Parameters.Add(outResult);

                DBBase.SetDBNullParametre(cmd.Parameters);
                try
                {
                    string IdDemande = string.Empty;
                    if (cmd.Connection.State == ConnectionState.Closed)
                        cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                    IdDemande = outResult.Value != null ? outResult.Value.ToString() : "1";
                    laCaisseHAbil.PK_ID = int.Parse(IdDemande);

                    return laCaisseHAbil;
                }
                catch (Exception ex)
                {
                    throw new Exception(cmd.CommandText + ":" + ex.Message);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CsHabilitationCaisse> RetourneHabileCaisseNonReversement(CsHabilitationCaisse laCaisse)
        {
            try
            {
                cn = new SqlConnection(ConnectionString);

                cmd = new SqlCommand();
                cmd.Connection = cn;
                cmd.CommandTimeout = 3000;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SPX_CAIS_CAISSE_NON_REVERSER";
                cmd.Parameters.Add("@NUMCAISSE", SqlDbType.VarChar ,5).Value = laCaisse.NUMCAISSE ;
                cmd.Parameters.Add("@MATRICULE", SqlDbType.VarChar ,10).Value = laCaisse.MATRICULE ;
                DBBase.SetDBNullParametre(cmd.Parameters);
                try
                {
                    if (cn.State == ConnectionState.Closed)
                        cn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(reader);
                    return Entities.GetEntityListFromQuery<CsHabilitationCaisse>(dt); ;
                }
                catch (Exception ex)
                {
                    return null;
                }
                finally
                {
                    if (cn.State == ConnectionState.Open)
                        cn.Close(); // Fermeture de la connection 
                    cmd.Dispose();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public CsHabilitationCaisse RetourneHabileCaisseReversement(CsHabilitationCaisse laCaisse)
        {

                cn = new SqlConnection(ConnectionString);

                cmd = new SqlCommand();
                cmd.Connection = cn;
                cmd.CommandTimeout = 3000;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SPX_CAIS_LISTE_REVERSEMENT_CAISSE_COURANTE";
                cmd.Parameters.Add("@PK_ID", SqlDbType.Int ).Value = laCaisse.PK_ID ;
                DBBase.SetDBNullParametre(cmd.Parameters);
                try
                {
                    if (cn.State == ConnectionState.Closed)
                        cn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(reader);
                    CsHabilitationCaisse laCaisset = Entities.GetEntityFromQuery<CsHabilitationCaisse>(dt); ;
                    if (laCaisset != null )
                    {
                        laCaisse.MONTANTENCAISSE =laCaisset.MONTANTENCAISSE ;
                        laCaisse.MONTANTREVERSER  =laCaisset.MONTANTREVERSER ;
                        laCaisse.ECART  =laCaisset.ECART ;
                    }
                    return laCaisse;
                }
                catch (Exception ex)
                {
                    return null;
                }
                finally
                {
                    if (cn.State == ConnectionState.Open)
                        cn.Close(); // Fermeture de la connection 
                    cmd.Dispose();
                }
            }

        public bool ReverserCaisse(List<CsReversementCaisse> laCaisseHAbil)
        {
               cn = new SqlConnection(ConnectionString);

                cmd = new SqlCommand();
                cmd.Connection = cn;
                cmd.CommandTimeout = 3000;
                cmd.CommandType = CommandType.StoredProcedure;
                if (cmd.Parameters != null && cmd.Parameters.Count != 0) cmd.Parameters.Clear();

                cmd.CommandText = "SPX_CAIS_HABILITER_CAISSE";
                cmd.Parameters.Add("@FK_IDHABILITATIONCAISSE", SqlDbType.Int ).Value = laCaisseHAbil.First().FK_IDHABILITATIONCAISSE  ;
                cmd.Parameters.Add("@IsCAISSECOURANTE", SqlDbType.Bit ).Value = laCaisseHAbil.First().IsCAISSECOURANTE   ;
                cmd.Parameters.Add("@RESTE", SqlDbType.Decimal ).Value =laCaisseHAbil.First().FK_IDHABILITATIONCAISSE  ;
                cmd.Parameters.Add("@MONTANT", SqlDbType.DateTime).Value = laCaisseHAbil.First().MONTANT  ;

                DBBase.SetDBNullParametre(cmd.Parameters);
                try
                {
                    int result = -1;
                    if (cmd.Connection.State == ConnectionState.Closed)
                        cmd.Connection.Open();
                    result = cmd.ExecuteNonQuery();
                    return (result > 0);
                }
                catch (Exception ex)
                {
                    throw new Exception(cmd.CommandText + ":" + ex.Message);
                }
        }


        public List<CsLclient> RetourneListeRecuDeCaisse(int Caisse)
        {
             
                cn = new SqlConnection(ConnectionString);
                cmd = new SqlCommand();
                cmd.Connection = cn;
                cmd.CommandTimeout = 3000;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SPX_CAIS_LISTE_DES_ENCAISSEMENT";
                cmd.Parameters.Add("@FK_IDHABILITATION", SqlDbType.Int ).Value = Caisse;
                DBBase.SetDBNullParametre(cmd.Parameters);
                try
                {
                    if (cn.State == ConnectionState.Closed)
                        cn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(reader);
                    return Entities.GetEntityListFromQuery<CsLclient>(dt); ;
                }
                catch (Exception ex)
                {
                    return null;
                }
                finally
                {
                    if (cn.State == ConnectionState.Open)
                        cn.Close(); // Fermeture de la connection 
                    cmd.Dispose();
                }
        }
        /**/

    #endregion

    }
}



