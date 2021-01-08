using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Collections;
using Inova.Tools.Utilities;
using Galatee.Entity.Model;
using Galatee.Structure;
using System.Globalization;
using System.Threading.Tasks;
using System.Reflection;


namespace Galatee.DataAccess
{
    public class DBIndex
    {
        private string ConnectionString;
        public DBIndex()
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


        #region ADO.net
        private SqlCommand cmd = null;
        private SqlConnection cn = null;

        private bool _Transaction;

        public bool Transaction
        {
            get { return _Transaction; }
            set { _Transaction = value; }

        }
 

        public  bool CreationCtarCompt(int fk_idAbon, string lotri, string periode, string Matricule, int idcentre, int idproduit,DateTime? dateEvt)
        {
            cn = new SqlConnection(ConnectionString);

            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandTimeout = 3000;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SPX_CREATION_CTARCOMP";
            cmd.Parameters.Add("@IdAbon", SqlDbType.Int).Value = fk_idAbon;
            cmd.Parameters.Add("@Lotri", SqlDbType.VarChar, 8).Value = lotri;
            cmd.Parameters.Add("@Periode", SqlDbType.VarChar, 8).Value = periode;
            cmd.Parameters.Add("@Matricule", SqlDbType.VarChar, 6).Value = Matricule;
            cmd.Parameters.Add("@IdCentre", SqlDbType.Int).Value = idcentre;
            cmd.Parameters.Add("@IdProduit", SqlDbType.Int).Value = idproduit;
            cmd.Parameters.Add("@DateEvt", SqlDbType.DateTime).Value = dateEvt;

            int Insere = -1;
            DBBase.SetDBNullParametre(cmd.Parameters);
            try
            {
                if (cn.State == ConnectionState.Closed)
                    cn.Open();
                Insere = cmd.ExecuteNonQuery();
                return Insere == -1 ? false : true;
            }
            catch (Exception ex)
            {
                return false;
                throw new Exception(cmd.CommandText + ":" + ex.Message);
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                    cn.Close(); // Fermeture de la connection 
                cmd.Dispose();
            }
        }

        private int CreationLot(int fk_idcentre, int? idproduit, int? idcategorie, int? idperiodicite, int? fkidtournee, string PeriodeEnCour,string Lotri,string Matricule, bool IsAvecEdition)
        {

            int NombrePeriode = 12;
            CsParametresGeneraux  param = new DB_ParametresGeneraux().SelectParametresGenerauxByCode ("000074");
            if (param != null && string.IsNullOrEmpty( param.LIBELLE) )
                NombrePeriode = int.Parse(param.LIBELLE);


            cn = new SqlConnection(ConnectionString);

            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandTimeout = 3000;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SPX_CREATION_LOT";
            cmd.Parameters.Add("@Lotri", SqlDbType.VarChar, 8).Value = Lotri;
            cmd.Parameters.Add("@Matricule", SqlDbType.VarChar, 5).Value = Matricule;
            cmd.Parameters.Add("@IdCentre", SqlDbType.Int).Value = fk_idcentre;
            cmd.Parameters.Add("@IdProduit", SqlDbType.Int).Value = idproduit;
            cmd.Parameters.Add("@IdCategorie", SqlDbType.Int).Value = idcategorie;
            cmd.Parameters.Add("@IdPeriodicite", SqlDbType.Int).Value = idperiodicite ;
            cmd.Parameters.Add("@IdTournee", SqlDbType.Int).Value = fkidtournee;
            cmd.Parameters.Add("@Periode", SqlDbType.VarChar, 6).Value = PeriodeEnCour;
            cmd.Parameters.Add("@prendreResilie", SqlDbType.VarChar, 6).Value = IsAvecEdition;
            cmd.Parameters.Add("@nombrePeriodeEstimationConso", SqlDbType.Int).Value = NombrePeriode;

            int res = -1;
            DBBase.SetDBNullParametre(cmd.Parameters);
            try
            {
                if (cn.State == ConnectionState.Closed)
                    cn.Open();
                int Nombre = 0;
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                    Nombre = (Convert.IsDBNull(reader["NOMBRE"])) ? 0 : (int)reader["NOMBRE"];
                return Nombre;

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

        public List<CsTypeComptage>  GetTypeComptage(int? nombreTransfo, int puissanceSouscrite, int puissanceInstallee)
        {
            cn = new SqlConnection(ConnectionString);

            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandTimeout = 3000;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SPX_GET_TYPECOMPTAGE";
            cmd.Parameters.Add("@nombreTransfo", SqlDbType.Int).Value = nombreTransfo;
            cmd.Parameters.Add("@puissanceSouscrite", SqlDbType.Int).Value = puissanceSouscrite;
            cmd.Parameters.Add("@puissanceInstallee", SqlDbType.Int).Value = puissanceInstallee;

            DBBase.SetDBNullParametre(cmd.Parameters);
            try
            {
                if (cn.State == ConnectionState.Closed)
                    cn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);
                return Entities.GetEntityListFromQuery<CsTypeComptage>(dt);

            }
            catch (Exception ex)
            {
                return null ;
                throw new Exception(cmd.CommandText + ":" + ex.Message);
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                    cn.Close(); // Fermeture de la connection 
                cmd.Dispose();
            }
        }

        public List<CsEvenement> ChargerListeDesTransfertsp(CsLotri _lstLori)
        {
            cn = new SqlConnection(ConnectionString);
            //p.LOTRI == _lstLori.NUMLOTRI && p.FK_IDCENTRE == _lstLori.FK_IDCENTRE 
            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandTimeout = 3000;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SPX_TSP_SYNCHRONISATION";
            cmd.Parameters.Add("@NUMLOTRI", SqlDbType.VarChar, Enumere.TailleNumeroBatch ).Value = _lstLori.NUMLOTRI ;

            DBBase.SetDBNullParametre(cmd.Parameters);
            try
            {
                if (cn.State == ConnectionState.Closed)
                    cn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);
                return Entities.GetEntityListFromQuery<CsEvenement>(dt);

            }
            catch (Exception ex)
            {
                return null;
                throw new Exception(cmd.CommandText + ":" + ex.Message);
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                    cn.Close(); // Fermeture de la connection 
                cmd.Dispose();
            }
        }


        #endregion


        #region Index

        public int ConstruireLotSansEdition(List<CsCentre> LstCentre, List<CsProduit> LstProduit, List<CsCategorieClient> LstCategorie, List<CsFrequence> LstPeriodicite, List<CsTournee> LstTournee, string Lotri, string periodeFacturation, string Matricule)
        {
            int _NombreDeClient = 0;
            try
            {
                List<CsClientLotri> LesClientDuLot = new List<CsClientLotri>();
                List<CsLotri> _ListDeLotri = ConstituerLotri(LstCentre, LstProduit, LstCategorie, LstPeriodicite, LstTournee, Lotri, periodeFacturation);
                _NombreDeClient = CreationEnregistrement(_ListDeLotri, Lotri, periodeFacturation, Matricule);
                if (_NombreDeClient == 0)
                    return 0;
                using (galadbEntities tms = new galadbEntities())
                {
                    ACTION leAction = CreerActionLotri(_ListDeLotri.First(), Enumere.ACTION_CONSTITUTION, Matricule, _NombreDeClient, 0);
                    InsererAction(leAction, tms);
                    tms.SaveChanges();
                    //Galatee.Tools.Utility.SendMail("armand.kouakou@inova.ci", _NombreDeClient);
                }
                return _NombreDeClient;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CsEvenement> ConstruireLotAvecEdition(List<CsCentre> LstCentre, List<CsProduit> LstProduit, List<CsCategorieClient> LstCategorie, List<CsFrequence> LstPeriodicite, List<CsTournee> LstTournee, string Lotri, string periodeFacturation, string Matricule)
        {
            int _NombreDeClient = 0;
            try
            {
                List<CsEvenement> LesEvenementLot = new List<CsEvenement>();
                List<CsLotri> _ListDeLotri = ConstituerLotri(LstCentre, LstProduit, LstCategorie, LstPeriodicite, LstTournee, Lotri, periodeFacturation);
                LesEvenementLot = CreationEnregistrementAvecEdition(_ListDeLotri, Lotri, periodeFacturation, Matricule);
                if (LesEvenementLot != null && LesEvenementLot.Count != 0)
                {
                    using (galadbEntities tms = new galadbEntities())
                    {
                        ACTION leAction = CreerActionLotri(_ListDeLotri.First(), Enumere.ACTION_CONSTITUTION, Matricule, _NombreDeClient, 0);
                        InsererAction(leAction, tms);
                        tms.SaveChanges();
                        //Galatee.Tools.Utility.SendMail("armand.kouakou@inova.ci", _NombreDeClient);
                    }
                }
                return LesEvenementLot;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CsTournee> ChargerLesTournees()
        {
            //cmd.CommandText = "SPX_GUI_RETOURNE_TOURNEE";
            try
            {
                DataTable dt = Galatee.Entity.Model.CommonProcedures.RetourneListeTournee();
                List<CsTournee> lstTournee = Entities.GetEntityListFromQuery<CsTournee>(dt);
                return lstTournee;

            }
            catch (Exception ex)
            {
                throw new Exception("ChargerLesTournees" + ":" + ex.Message);
            }

        }

        private int CreationEnregistrement(List<CsLotri> lotri, string Lotri, string periodeFacturation, string Matricule)
        {
            try
            {
                int _NombreDeClient = 0;
                List<CsEvenement> LesClientDuLot;
                List<Galatee.Entity.Model.EVENEMENT> eventToInsert = new List<Entity.Model.EVENEMENT>();
                //List<Galatee.Entity.Model.PAGERI> pageriToInsert = new List<Entity.Model.PAGERI>();
                List<CsPuissance> lstPuissance = new DBAccueil().ChargerPuissanceInstalle();
                List<LOTRI> lotriToInsert = new List<LOTRI>();
                foreach (CsLotri _Lotri in lotri)
                {
                    LesClientDuLot = new List<CsEvenement >();
                    _NombreDeClient = ChargerListeDesClientsDuLot(_Lotri.FK_IDCENTRE, _Lotri.FK_IDPRODUIT, _Lotri.FK_IDCATEGORIECLIENT, _Lotri.FK_IDPERIODICITE, _Lotri.FK_IDTOURNEE, _Lotri.PERIODE, Lotri, Matricule,false);
                }
                return _NombreDeClient;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
       
     private List<CsEvenement> CreationEnregistrementAvecEdition(List<CsLotri> lotri, string Lotri, string periodeFacturation, string Matricule)
        {
            try
            {
                int _NombreDeClient = 0;
                List<CsEvenement> LesClientDuLotEditer = new List<CsEvenement>();
                foreach (CsLotri _Lotri in lotri)
                {
                    _NombreDeClient = ChargerListeDesClientsDuLot(_Lotri.FK_IDCENTRE, _Lotri.FK_IDPRODUIT, _Lotri.FK_IDCATEGORIECLIENT, _Lotri.FK_IDPERIODICITE, _Lotri.FK_IDTOURNEE, _Lotri.PERIODE, Lotri, Matricule,false);
                    LesClientDuLotEditer.AddRange(EditionDesRiSpx(_Lotri));
                }
                return LesClientDuLotEditer;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
  

        private CsEvenement RetournerDernierEvent(CsClientLotri _leClient, string Matricule, string periodeFacturation)
        {
            try
            {
                CsEvenement LeDernierEvt = new CsEvenement();
                //_leClient.LOTRI = _Lotri.NUMLOTRI;
                //_leClient.CENTRE = _Lotri.CENTRE;
                _leClient.CAS = Enumere.CasCreation;
                _leClient.STATUS = Enumere.EvenementCree;//.ToString();
                LeDernierEvt.DATEEVT = System.DateTime.Today.Date;
                LeDernierEvt.PERIODE = periodeFacturation;
                LeDernierEvt.LOTRI = _leClient.LOTRI;// Lotri;
                LeDernierEvt.STATUS = _leClient.STATUS;
                LeDernierEvt.FACTURE = null;
                LeDernierEvt.TYPECOMPTEUR = _leClient.TCOMPT;
                LeDernierEvt.REGCONSO = 0;
                LeDernierEvt.QTEAREG = 0;
                LeDernierEvt.COMPTEUR = _leClient.NUMERO;
                LeDernierEvt.CAS = _leClient.CAS;
                LeDernierEvt.INDEXEVT = null;
                LeDernierEvt.NUMEVENEMENT = _leClient.MAXEVENT + 1;
                LeDernierEvt.CODEEVT = _leClient.CODEEVT;
                LeDernierEvt.COEFCOMPTAGE = _leClient.COEFCOMPTAGE;
                LeDernierEvt.COEFFAC = _leClient.COEFFAC;
                LeDernierEvt.COEFK1 = _leClient.COEFK1;
                LeDernierEvt.COEFK2 = _leClient.COEFK2;
                LeDernierEvt.COEFLECT = _leClient.COEFLECT;
                LeDernierEvt.CONSONONFACTUREE = _leClient.CONSONONFACTUREE;
                LeDernierEvt.DATECREATION = DateTime.Now;// _leClient.DATECREATION;
                LeDernierEvt.DATEMODIFICATION = null; //_leClient.DATEMODIFICATION;
                LeDernierEvt.DERPERF = _leClient.DERPERF;
                LeDernierEvt.DERPERFN = _leClient.DERPERFN;
                LeDernierEvt.FACPER = _leClient.FACPER;
                LeDernierEvt.CENTRE = _leClient.CENTRE;
                LeDernierEvt.CLIENT = _leClient.CLIENT;
                LeDernierEvt.ORDRE = _leClient.ORDRE;
                LeDernierEvt.POINT = _leClient.POINT;
                LeDernierEvt.PRODUIT = _leClient.PRODUIT;
                LeDernierEvt.PUISSANCE = _leClient.PUISSANCE;
                LeDernierEvt.REGIMPUTE = _leClient.REGIMPUTE;
                LeDernierEvt.SURFACTURATION = _leClient.SURFACTURATION;
                LeDernierEvt.TYPECOMPTAGE = _leClient.TYPECOMPTAGE;
                LeDernierEvt.TYPECONSO = _leClient.TYPECONSO;
                LeDernierEvt.USERCREATION = Matricule;//_leClient.USERCREATION;

                LeDernierEvt.FK_IDCANALISATION = _leClient.FK_IDCANALISATION;
                LeDernierEvt.FK_IDCENTRE = _leClient.FK_IDCENTRE;
                LeDernierEvt.FK_IDPRODUIT = _leClient.FK_IDPRODUIT;

                return LeDernierEvt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        
        bool? InsererUnEvenement(galadbEntities cmd, CsEvenement pEvenement)
        {
            try
            {
                Galatee.Entity.Model.EVENEMENT events = new Galatee.Entity.Model.EVENEMENT();

                events.CAS = pEvenement.CAS;
                events.CODEEVT = pEvenement.CODEEVT;
                events.COEFCOMPTAGE = pEvenement.COEFCOMPTAGE;
                events.COEFFAC = pEvenement.COEFFAC;
                events.COEFK1 = pEvenement.COEFK1;
                events.COEFK2 = pEvenement.COEFK2;
                events.COEFLECT = pEvenement.COEFLECT;
                events.COMPTEUR = pEvenement.COMPTEUR;
                events.CONSO = pEvenement.CONSO;
                events.CONSOFAC = pEvenement.CONSOFAC;
                events.CONSONONFACTUREE = pEvenement.CONSONONFACTUREE;
                events.DATECREATION = pEvenement.DATECREATION;
                events.DATEEVT = pEvenement.DATEEVT;
                //events.DATEMODIFICATION = pEvenement.DATEMODIFICATION;
                events.DERPERF = pEvenement.DERPERF;
                events.DERPERFN = pEvenement.DERPERFN;
                //events.FK_DIAMETRE = pEvenement.DIAMETRE;
                //events.DMAJ = pEvenement.DMAJ;
                events.ENQUETE = pEvenement.ENQUETE;
                //events.ENTFAC = pEvenement.ENTFAC;
                events.FACPER = pEvenement.FACPER;
                events.FACTURE = pEvenement.FACTURE;
                events.INDEXEVT = pEvenement.INDEXEVT;
                events.LOTRI = pEvenement.LOTRI;
                //events.FK_MATRICULE = pEvenement.MATRICULE;
                events.PERIODE = pEvenement.PERIODE;
                events.CENTRE = pEvenement.CENTRE;
                events.CLIENT = pEvenement.CLIENT;
                events.NUMEVENEMENT = pEvenement.NUMEVENEMENT;
                events.ORDRE = pEvenement.ORDRE;
                events.POINT = pEvenement.POINT;
                events.PRODUIT = pEvenement.PRODUIT;
                events.PUISSANCE = pEvenement.PUISSANCE;
                events.QTEAREG = pEvenement.QTEAREG;
                events.REGCONSO = pEvenement.REGCONSO;
                events.REGIMPUTE = pEvenement.REGIMPUTE;
                events.STATUS = pEvenement.STATUS;
                events.SURFACTURATION = pEvenement.SURFACTURATION;
                //events.FK_TCOMPT = pEvenement.TCOMPT;
                events.TYPECOMPTAGE = pEvenement.TYPECOMPTAGE;
                events.TYPECONSO = pEvenement.TYPECONSO;
                events.USERCREATION = pEvenement.USERCREATION;

                // récupération des foreign key

                events.FK_IDCANALISATION = pEvenement.FK_IDCANALISATION.Value ;
                //events.FK_IDCAS = CasIndex.First(c=> c.CODE  == events.CAS).PK_ID;
                events.FK_IDCENTRE = pEvenement.FK_IDCENTRE;
                events.FK_IDPRODUIT = pEvenement.FK_IDPRODUIT;

                return Entities.InsertEntity<Galatee.Entity.Model.EVENEMENT>(events, cmd);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        Galatee.Entity.Model.EVENEMENT CreationUnEvenementHorsLigne(CsEvenement pEvenement)
        {
            try
            {
                int maxEventNum = RetourneNumeroEvenementMax(pEvenement);
                Galatee.Entity.Model.EVENEMENT events = new Galatee.Entity.Model.EVENEMENT();

                events.CAS = pEvenement.CAS;
                events.CODEEVT = pEvenement.CODEEVT; //  à spécifier
                events.COEFCOMPTAGE = pEvenement.COEFCOMPTAGE;
                events.COEFFAC = pEvenement.COEFFAC;
                events.COEFLECT = pEvenement.COEFLECT;
                events.COMPTEUR = pEvenement.COMPTEUR;
                events.CONSO = pEvenement.CONSO;
                events.CONSOFAC = pEvenement.CONSOFAC;
                events.CONSONONFACTUREE = pEvenement.CONSONONFACTUREE;
                events.DATECREATION = pEvenement.DATECREATION;
                events.DATEEVT = pEvenement.DATEEVT;
                //events.DATEMODIFICATION = pEvenement.DATEMODIFICATION;
                events.DERPERF = pEvenement.DERPERF;
                events.DERPERFN = pEvenement.DERPERFN;
                //events.FK_DIAMETRE = pEvenement.DIAMETRE;
                //events.DMAJ = pEvenement.DMAJ;
                events.ENQUETE = pEvenement.ENQUETE;
                //events.ENTFAC = pEvenement.ENTFAC;
                events.FACPER = pEvenement.FACPER;
                events.INDEXEVT = pEvenement.INDEXEVT;
                events.LOTRI = pEvenement.LOTRI;
                //events.FK_MATRICULE = pEvenement.MATRICULE;
                events.PERIODE = pEvenement.PERIODE;
                events.CENTRE = pEvenement.CENTRE;
                events.CLIENT = pEvenement.CLIENT;
                events.NUMEVENEMENT = maxEventNum + 1;
                events.ORDRE = pEvenement.ORDRE;
                events.POINT = pEvenement.POINT;
                events.PRODUIT = pEvenement.PRODUIT;
                events.PUISSANCE = pEvenement.PUISSANCE;
                events.QTEAREG = pEvenement.QTEAREG;
                events.REGCONSO = pEvenement.REGCONSO;
                events.REGIMPUTE = pEvenement.REGIMPUTE;
                events.STATUS = pEvenement.STATUS;
                events.SURFACTURATION = pEvenement.SURFACTURATION;
                //events.FK_TCOMPT = pEvenement.TCOMPT;
                events.TYPECOMPTAGE = pEvenement.TYPECOMPTAGE;
                events.TYPECONSO = pEvenement.TYPECONSO;
                events.USERCREATION = pEvenement.USERCREATION;

                // récupération des foreign key

                events.FK_IDCANALISATION = pEvenement.FK_IDCANALISATION.Value ;
                //events.FK_IDCAS = CasIndex.First(c => c.CODE  == events.CAS).PK_ID;
                events.FK_IDCENTRE = pEvenement.FK_IDCENTRE;
                events.FK_IDPRODUIT = pEvenement.FK_IDPRODUIT;

                return events;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        Galatee.Entity.Model.EVENEMENT CreationUnEvenement(CsEvenement pEvenement)
        {
            try
            {
                Galatee.Entity.Model.EVENEMENT events = new Galatee.Entity.Model.EVENEMENT();

                events.CAS = pEvenement.CAS;
                events.CODEEVT = pEvenement.CODEEVT; //  à spécifier
                events.COEFCOMPTAGE = pEvenement.COEFCOMPTAGE;
                events.COEFFAC = pEvenement.COEFFAC;
                events.COEFLECT = pEvenement.COEFLECT;
                events.COMPTEUR = pEvenement.COMPTEUR;
                events.CONSO = pEvenement.CONSO;
                events.CONSOFAC = pEvenement.CONSOFAC;
                events.CONSONONFACTUREE = pEvenement.CONSONONFACTUREE;
                events.DATECREATION = pEvenement.DATECREATION;
                events.DATEEVT = pEvenement.DATEEVT;
                events.DERPERF = pEvenement.DERPERF;
                events.DERPERFN = pEvenement.DERPERFN;
                events.ENQUETE = pEvenement.ENQUETE;
                events.FACPER = pEvenement.FACPER;
                events.INDEXEVT = pEvenement.INDEXEVT;
                events.LOTRI = pEvenement.LOTRI;
                events.PERIODE = pEvenement.PERIODE;
                events.CENTRE = pEvenement.CENTRE;
                events.CLIENT = pEvenement.CLIENT;
                events.NUMEVENEMENT = pEvenement.NUMEVENEMENT;
                events.ORDRE = pEvenement.ORDRE;
                events.POINT = pEvenement.POINT;
                events.PRODUIT = pEvenement.PRODUIT;
                events.REGLAGECOMPTEUR = pEvenement.REGLAGECOMPTEUR;
                events.PUISSANCE = pEvenement.PUISSANCE;
                events.QTEAREG = pEvenement.QTEAREG;
                events.REGCONSO = pEvenement.REGCONSO;
                events.REGIMPUTE = pEvenement.REGIMPUTE;
                events.STATUS = pEvenement.STATUS;
                events.SURFACTURATION = pEvenement.SURFACTURATION;
                events.TYPECOMPTEUR = pEvenement.TYPECOMPTEUR;
                events.TYPECOMPTAGE = pEvenement.TYPECOMPTAGE;
                events.TYPECONSO = pEvenement.TYPECONSO;
                events.USERCREATION = pEvenement.USERCREATION;
                events.DATEMODIFICATION = DateTime.Now;
                events.USERMODIFICATION = pEvenement.USERCREATION;

                // récupération des foreign key

                events.FK_IDCANALISATION = pEvenement.FK_IDCANALISATION.Value ;
                //events.FK_IDCAS = CasIndex.First(c => c.CODE  == events.CAS).PK_ID;
                events.FK_IDCENTRE = pEvenement.FK_IDCENTRE;
                events.FK_IDPRODUIT = pEvenement.FK_IDPRODUIT;
                events.FK_IDABON = pEvenement.FK_IDABON.Value ;
                events.FK_IDCOMPTEUR = pEvenement.FK_IDCOMPTEUR.Value ;


                return events;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool? InsererEvenement(CsEvenement pEvenement)
        {
            using (galadbEntities ctx = new galadbEntities())
            {
              return   InsererUnEvenement(ctx, pEvenement);
            }
        }
        public bool? InsererLstEvenement(List<CsEvenement> pEvenement)
        {
              try
              {
                  using (galadbEntities ctxInter = new galadbEntities())
                  {

                      List<Galatee.Entity.Model.EVENEMENT> lesEvent = new List<Entity.Model.EVENEMENT>();
                      List<Galatee.Entity.Model.EVENEMENT> lesEventSupprime = new List<Entity.Model.EVENEMENT>();
                      foreach (CsEvenement item in pEvenement)
                      {
                          Galatee.Entity.Model.EVENEMENT evtSupprime = ctxInter.EVENEMENT.FirstOrDefault(t => t.FK_IDCENTRE == item.FK_IDCENTRE && t.CENTRE == item.CENTRE &&
                                                                                         t.CLIENT == item.CLIENT && t.ORDRE == item.ORDRE &&
                                                                                         t.PRODUIT == item.PRODUIT && t.POINT == item.POINT && item.LOTRI == t.LOTRI);
                          if (evtSupprime != null && !string.IsNullOrEmpty(evtSupprime.LOTRI))
                              evtSupprime.STATUS = Enumere.EvenementSupprimer;

                          int MaxEvt = ctxInter.EVENEMENT.Where(t => t.FK_IDCENTRE == item.FK_IDCENTRE && t.CENTRE == item.CENTRE && t.CLIENT == item.CLIENT && t.ORDRE == item.ORDRE && t.PRODUIT == item.PRODUIT && t.POINT == item.POINT).Max(y => y.NUMEVENEMENT);
                          item.NUMEVENEMENT = MaxEvt + 1;
                          lesEvent.Add(Galatee.Tools.Utility.ConvertEntity<Galatee.Entity.Model.EVENEMENT, CsEvenement>(item));
                      }
                      Entities.InsertEntity<Galatee.Entity.Model.EVENEMENT>(lesEvent);
                      bool result = new DBIndex().CreationCtarCompt(lesEvent.First().FK_IDABON, lesEvent.First().LOTRI, lesEvent.First().PERIODE, lesEvent.First().MATRICULE, lesEvent.First().FK_IDCENTRE, lesEvent.First().FK_IDPRODUIT, lesEvent.First().DATEEVT);
                      ctxInter.SaveChanges();
                      return result; 
                  }
	            }
	            catch (Exception ex)
	            {
		            throw ex;
	            }
        }

        //private Galatee.Entity.Model.TRANSFERT  CreeEvenementParClientLot(CsEvenement  _LeClientDuLot)
        //{

        //    Galatee.Entity.Model.TRANSFERT _LeTransfert = new Galatee.Entity.Model.TRANSFERT()
        //    {
        //        CENTRE = _LeClientDuLot.CENTRE,
        //        CLIENT = _LeClientDuLot.CLIENT,
        //        ORDRE = _LeClientDuLot.ORDRE,
        //        PRODUIT = _LeClientDuLot.PRODUIT,
        //        POINT = _LeClientDuLot.POINT,
        //        LOTRI = _LeClientDuLot.LOTRI,
        //        NUMEVENEMENT = 1,
        //        COMPTEUR = _LeClientDuLot.COMPTEUR,
        //        DATEEVT = _LeClientDuLot.DATEEVT ,
        //        PERIODE = _LeClientDuLot.PERIODE,
        //        CODEEVT = _LeClientDuLot.CODEEVT ,
        //        INDEXEVT = _LeClientDuLot.INDEXEVT ,
        //        CAS = _LeClientDuLot.COMPTEUR,
        //        ENQUETE = _LeClientDuLot.ENQUETE,
        //        CONSO = _LeClientDuLot.CONSO,
        //        STATUS = Enumere.EvenementCree,
        //        TYPECONSO = 0,
        //        DIAMETRE = _LeClientDuLot.DIAMETRE,
        //        //FACTTOT = 0,
        //        MATRICULE = matricule,
        //        FACPER = null,
        //        QTEAREG = null,
        //        DERPERF = null,
        //        DERPERFN = _LeClientDuLot.PERIODE,
        //        CONSOFAC = null,
        //        REGIMPUTE = null,
        //        REGCONSO = null,
        //        COEFLECT = _LeClientDuLot.COEFLECT,
        //        COEFCOMPTAGE = _LeClientDuLot.COEFCOMPTAGE,
        //        TYPECOMPTEUR = _LeClientDuLot.TCOMPT,
        //        COEFK1 = _LeClientDuLot.COEFK1,
        //        COEFK2 = _LeClientDuLot.COEFK2,
        //        FK_IDABON = _LeClientDuLot.FK_IDABON,
        //        FK_IDCENTRE = _LeClientDuLot.FK_IDCENTRE,
        //        FK_IDPRODUIT = _LeClientDuLot.FK_IDPRODUIT,
        //        FK_IDCANALISATION = _LeClientDuLot.FK_IDCANALISATION,
        //        FK_IDCOMPTEUR = _LeClientDuLot.FK_IDCOMPTEUR,
        //        DATECREATION = System.DateTime.Now,
        //        DATEMODIFICATION = System.DateTime.Now,
        //        USERCREATION = matricule,
        //        USERMODIFICATION = matricule


        //    };

        //    return _LeEvenement;
        //}
        private Galatee.Entity.Model.EVENEMENT CreeEvenementParClientLot(CsEvenement  _LeClientDuLot, string matricule)
        {

            Galatee.Entity.Model.EVENEMENT _LeEvenement = new Galatee.Entity.Model.EVENEMENT()
            {
                CENTRE = _LeClientDuLot.CENTRE,
                CLIENT = _LeClientDuLot.CLIENT,
                ORDRE = _LeClientDuLot.ORDRE,
                PRODUIT = _LeClientDuLot.PRODUIT,
                POINT = _LeClientDuLot.POINT,
                LOTRI = _LeClientDuLot.LOTRI,
                NUMEVENEMENT = 1,
                COMPTEUR = _LeClientDuLot.COMPTEUR,
                DATEEVT = System.DateTime.Today.Date,
                PERIODE = _LeClientDuLot.PERIODE,
                CODEEVT = Enumere.EvenementCodeNormale,
                INDEXEVT = null,
                CAS = Enumere.CasCreation,
                ENQUETE = null,
                CONSO = 0,
                CONSONONFACTUREE = 0,
                FACTURE = null,
                SURFACTURATION = null,
                STATUS = Enumere.EvenementCree,
                TYPECONSO = 0,
                //_LeClientDuLot.REGLAGECOMPTEUR,
                MATRICULE = matricule,
                FACPER = null,
                QTEAREG = null,
                DERPERF = null,
                DERPERFN = _LeClientDuLot.PERIODE,
                CONSOFAC = null,
                REGIMPUTE = null,
                REGCONSO = null,
                COEFLECT = _LeClientDuLot.COEFLECT,
                COEFCOMPTAGE = _LeClientDuLot.COEFCOMPTAGE,
                TYPECOMPTEUR = _LeClientDuLot.TYPECOMPTEUR ,
                COEFK1 = _LeClientDuLot.COEFK1,
                COEFK2 = _LeClientDuLot.COEFK2,
                FK_IDABON = _LeClientDuLot.FK_IDABON.Value ,
                FK_IDCENTRE = _LeClientDuLot.FK_IDCENTRE,
                FK_IDPRODUIT = _LeClientDuLot.FK_IDPRODUIT,
                FK_IDCANALISATION = _LeClientDuLot.FK_IDCANALISATION.Value ,
                FK_IDCOMPTEUR = _LeClientDuLot.FK_IDCOMPTEUR.Value ,
                DATECREATION = System.DateTime.Now,
                DATEMODIFICATION = System.DateTime.Now,
                USERCREATION = matricule,
                USERMODIFICATION = matricule


            };

            return _LeEvenement;
        }
        List<CsLotri> ConstituerLotri(List<CsCentre> LstCentre, List<CsProduit> LstProduit, List<CsCategorieClient> LstCategorie, List<CsFrequence> LstPeriodicite, List<CsTournee> LstTournee, string Lotri, string periode)
        {
            try
            {
                List<CsLotri> ListDesLotri = new List<CsLotri>();
                foreach (CsCentre _LeCentre in LstCentre)
                {
                    foreach (CsProduit _Leproduit in LstProduit)
                    {
                        foreach (CsCategorieClient _LaCateg in LstCategorie)
                        {
                            foreach (CsFrequence _LaPeriodicite in LstPeriodicite)
                            {
                                if (LstTournee == null || LstTournee.Count == 0)
                                    LstTournee.Add(new CsTournee());
                                foreach (CsTournee _Latournee in LstTournee)
                                {
                                    if (_Latournee.CENTRE == _LeCentre.CODE && _Latournee.FK_IDCENTRE == _LeCentre.PK_ID)
                                    {
                                        CsLotri _LeLotri = new CsLotri();
                                        _LeLotri.PERIODICITE = _LaPeriodicite.CODE.Substring(_LaPeriodicite.CODE.Length - Enumere.TaillePeriodicite, Enumere.TaillePeriodicite); ;
                                        _LeLotri.CENTRE = _LeCentre.CODE;
                                        _LeLotri.PRODUIT = _Leproduit.CODE.Substring(_Leproduit.CODE.Length - Enumere.TailleCodeProduit, Enumere.TailleCodeProduit);
                                        _LeLotri.CATEGORIECLIENT = _LaCateg.CODE.Substring(_LaCateg.CODE.Length - Enumere.TailleCodeCategorie, Enumere.TailleCodeCategorie);
                                        _LeLotri.PERIODE = periode;
                                        _LeLotri.NUMLOTRI = Lotri;
                                        _LeLotri.TOURNEE = string.IsNullOrEmpty(_Latournee.CODE) ? null : _Latournee.CODE;
                                        _LeLotri.BASE = "S";
                                        _LeLotri.FK_IDCENTRE = _LeCentre.PK_ID;
                                        _LeLotri.FK_IDPRODUIT = _Leproduit.PK_ID;
                                        _LeLotri.FK_IDCATEGORIECLIENT = _LaCateg.PK_ID;
                                        _LeLotri.FK_IDPERIODICITE = _LaPeriodicite.PK_ID;
                                        _LeLotri.FK_IDTOURNEE  = _Latournee.PK_ID;

                                        ListDesLotri.Add(_LeLotri);
                                    }
                                }
                            }
                        }
                    }
                }
                return ListDesLotri;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        List<CsLotri> ConstituerLotriThread(List<CsCentre> LstCentre, List<CsProduit> LstProduit, List<CsCategorieClient> LstCategorie, List<CsFrequence> LstPeriodicite, List<CsTournee> LstTournee, string Lotri, string periode)
        {
            try
            {

                List<CsLotri> ListDesLotri = new List<CsLotri>();

                Parallel.ForEach(
                          LstCentre, _LeCentre =>
                          {
                              Parallel.ForEach(
                                      LstProduit, _Leproduit =>
                                      {
                                          Parallel.ForEach(
                                              LstCategorie, _LaCateg =>
                                              {
                                                  Parallel.ForEach(
                                                      LstPeriodicite, _LaPeriodicite =>
                                                      {
                                                          if (LstTournee == null || LstTournee.Count == 0)
                                                              LstTournee.Add(new CsTournee());
                                                           

                                                          Parallel.ForEach(
                                                              LstTournee, _Latournee =>
                                                              {
                                                                  if (_Latournee.CENTRE == _LeCentre.CODE && _Latournee.FK_IDCENTRE == _LeCentre.PK_ID )
                                                                  {
                                                                      CsLotri _LeLotri = new CsLotri();
                                                                      _LeLotri.PERIODICITE = _LaPeriodicite.CODE.Substring(_LaPeriodicite.CODE.Length - Enumere.TaillePeriodicite, Enumere.TaillePeriodicite); ;
                                                                      _LeLotri.CENTRE = _LeCentre.CODE;
                                                                      _LeLotri.PRODUIT = _Leproduit.CODE.Substring(_Leproduit.CODE.Length - Enumere.TailleCodeProduit, Enumere.TailleCodeProduit);
                                                                      _LeLotri.CATEGORIECLIENT = _LaCateg.CODE.Substring(_LaCateg.CODE.Length - Enumere.TailleCodeCategorie, Enumere.TailleCodeCategorie);
                                                                      _LeLotri.PERIODE = periode;
                                                                      _LeLotri.NUMLOTRI = Lotri;
                                                                      _LeLotri.TOURNEE = string.IsNullOrEmpty(_Latournee.CODE) ? null : _Latournee.CODE;
                                                                      _LeLotri.BASE = "S";
                                                                      _LeLotri.ETATFAC5 = "R";
                                                                      _LeLotri.FK_IDCENTRE = _LeCentre.PK_ID;
                                                                      _LeLotri.FK_IDPRODUIT = _Leproduit.PK_ID;
                                                                      _LeLotri.FK_IDCATEGORIECLIENT = _LaCateg.PK_ID;
                                                                      _LeLotri.FK_IDPERIODICITE = _LaPeriodicite.PK_ID;
                                                                      _LeLotri.FK_IDTOURNEE = _Latournee.PK_ID;
                                                                      lock (_LeLotri)
                                                                      {
                                                                          ListDesLotri.Add(_LeLotri);

                                                                      }
                                                                  }

                                                              });
                                                      });
                                              });
                                      });
                          });

                return ListDesLotri;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        private int ChargerListeDesClientsDuLot(int fk_idcentre, int? idproduit, int? idcategorie, int? idperiodicite, int? fkidtournee, string PeriodeEnCour, string Lotri,string Matricule,bool IsAvecEdition)
        {
            try
            {
                if (!IsAvecEdition)
                {
                    return   CreationLot(fk_idcentre, idproduit, idcategorie, idperiodicite, fkidtournee, PeriodeEnCour, Lotri, Matricule, Enumere.IsResilierPriseEcompte);
                    //DataTable dt = IndexProcedures.ListeDesLotCandidatLori(fk_idcentre, idproduit, idcategorie, idperiodicite, fkidtournee, PeriodeEnCour);
                    //return Entities.GetEntityListFromQuery<CsEvenement>(dt);
                    //return new List<CsEvenement>();
                }
                else
                {
                    return 0;
                    //DataTable dt = IndexProcedures.ListeDesLotCandidatLoriAvecEdition(fk_idcentre, idproduit, idcategorie, idperiodicite, fkidtournee, PeriodeEnCour);
                    //return Entities.GetEntityListFromQuery<CsEvenement>(dt);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //private bool? ChargerListeDesClientsDuLotThread(int fk_idcentre, string Centre, string produit, string categorie, string periodicite, int fkidtournee, string tournee, string Lotri, CsLotri obj)
        //{
        //    //cmd.CommandText = "SPX_IND_LISTEDESLOTCANDIDTATLOTRI";

        //    try
        //    {
        //        //DataTable dt = ListeDesLotCandidatLori(Centre, produit, categorie, periodicite, tournee);
        //        DataTable dt = IndexProcedures.ListeDesLotCandidatLori(fk_idcentre, Centre, produit, categorie, periodicite, fkidtournee, tournee);
        //        //DataTable dt = IndexProcedures.ListeDesLotCandidatLori(Centre, produit, categorie, periodicite, tournee);
        //        obj.LesClientDuLot = Entities.GetEntityListFromQuery<CsClientLotri>(dt);
        //        //obj.LesClientDuLot = ClientDontMoisFacCorrespondPeriodeFac(clientsLot, null);
        //        //obj.LesClientDuLot = clientsLot;
        //        return true;
        //    }

        //    catch (Exception ex)
        //    {
        //        string error = ex.Message;
        //        throw ex;
        //    }
        //}

        private List<CsClientLotri> ClientDontMoisFacCorrespondPeriodeFac(List<CsClientLotri> clientsLot, string periodFac)
        {
            try
            {
                foreach (var cl in clientsLot)
                {
                    DateTime dtPeriodeFacture = new DateTime(int.Parse(periodFac.Substring(0, 4)), int.Parse(periodFac.Substring(4, 2)), 1);
                    DateTime dtDernierPeriodeFacture = new DateTime(int.Parse(cl.PERIODE.Substring(0, 4)), int.Parse(cl.PERIODE.Substring(4, 2)), 1);
                    TimeSpan tmsp = new TimeSpan();
                    DateTime diff = dtPeriodeFacture.AddTicks(-dtDernierPeriodeFacture.Ticks);
                    //if(TimeSpan. 
                    int perfac = int.Parse(cl.PERFAC);
                    //Math.DivRem(
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable ListeDesLotCandidatLori(int idCentre, int idproduit, int idcategorie, int idperiodicite, int idtournee)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    var Ag = context.AG;
                    //.Where(t => t.FK_IDCENTRE == idCentre && t.FK_IDCATEGORIE == idcategorie);
                    //var Ag = context.AG.Where(ag => ag.FK_IDTOURNEE  == idtournee);
                    //var Canal = context.CANALISATION.Where(e => e. == produit && e.CENTRE == Centre);
                    //var Abon = context.ABON.Where(a => a.PERFAC == periodicite && a.DRES == null);
                    //var Event = context.EVENEMENT.Where(e => e.STATUS != Enumere.ANNULE &&
                    //                                   e.STATUS != Enumere.Purger && e.STATUS != Enumere.Supprime);
                    //var Client = context.CLIENT;

                    IEnumerable<object> query = (
                                                 from _Ag in Ag
                                                 from _Client in _Ag.CLIENT1
                                                 from _abon in _Client.ABON
                                                 from _canal in _abon.CANALISATION
                                                 from _Event in _canal.EVENEMENT
                                                 from _TourRel in _Ag.TOURNEE1.TOURNEERELEVEUR
                                                 where (_Client.FK_IDCENTRE == idCentre &&
                                                       _Client.FK_IDCATEGORIE == idcategorie &&
                                                       _Ag.FK_IDTOURNEE == idtournee &&
                                                       _abon.FK_IDPRODUIT == idproduit &&
                                                       _Event.STATUS != Enumere.ANNULE &&
                                                       _Event.STATUS != Enumere.Purger && _Event.STATUS != Enumere.Supprime)
                                                 select new
                                                 {
                                                     _Client.CENTRE,
                                                     CLIENT = _Client.REFCLIENT,
                                                     FK_TOURNEE = _Ag.TOURNEE1.PK_ID,
                                                     _Ag.ORDTOUR,
                                                     _TourRel.FK_IDRELEVEUR ,
                                                     _canal.PRODUIT,
                                                     _canal.COMPTEUR,
                                                     DIAMETRE = _canal.REGLAGECOMPTEUR1.CODE ,
                                                     _canal.COMPTEUR.COEFLECT,
                                                     //FK_TYPECOMPTAGE = _canal.COMPTEUR.TYPECOMPTAGE,
                                                     _canal.COMPTEUR.COEFCOMPTAGE,
                                                     //FK_TCOMPT = _canal.COMPTEUR.TYPECOMPTAGE,
                                                     FK_POINT = _canal.POINT,
                                                     _Client.ORDRE,
                                                     _abon.DRES,
                                                     FK_PERFAC = _abon.PERFAC,
                                                     FK_CATEGORIE = _Client.CATEGORIE,
                                                     FK_IDCATEGOCLIENT = _Client.FK_IDCATEGORIE,
                                                     //MOISFAC = _abon.MOISFAC,
                                                     _Event.PERIODE,
                                                     _Event.NUMEVENEMENT,
                                                     _Event.PUISSANCE,
                                                     _Event.QTEAREG,
                                                     _Event.REGCONSO,
                                                     _Event.REGIMPUTE,
                                                     _Event.STATUS,
                                                     _Event.SURFACTURATION,
                                                     _Event.FK_IDPRODUIT,
                                                     //_Event.FK_IDCAS,
                                                     _Event.FK_IDCENTRE,
                                                     _Event.FK_IDCANALISATION,
                                                     //_Event.TCOMPT,
                                                     //_Event.TYPECOMPTAGE ,
                                                     _Event.TYPECONSO,
                                                     _Event.INDEXEVT

                                                     //MAXEVENT = (int)Event.Where(e => e.PK_CENTRE == _ag.CENTRE &&
                                                     //                       e.PK_CLIENT == _ag.CLIENT && e.PK_ORDRE == _ag.ORDRE &&
                                                     //                       e.PK_PRODUIT == _canal.PRODUIT && e.PK_POINT == _canal.POINT).Max(e=>e.PK_EVENEMENT)
                                                 }).Distinct();

                    return ListToDataTable<object>(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }        
        public static DataTable ListToDataTable<T>(IEnumerable<T> varlist)
        {
            var dtReturn = new DataTable();

            // column names 
            try
            {
                PropertyInfo[] oProps = null;

                if (varlist == null) return dtReturn;

                foreach (var rec in varlist)
                {
                    // Use reflection to get property names, to create table, Only first time, others will follow 
                    if (oProps == null)
                    {
                        oProps = rec.GetType().GetProperties();
                        foreach (var pi in oProps)
                        {
                            var colType = pi.PropertyType;

                            if ((colType.IsGenericType) && (colType.GetGenericTypeDefinition() == typeof(Nullable<>)))
                            {
                                colType = colType.GetGenericArguments()[0];
                            }

                            dtReturn.Columns.Add(new DataColumn(pi.Name, colType));
                        }
                    }

                    var dr = dtReturn.NewRow();

                    foreach (var pi in oProps)
                    {
                        dr[pi.Name] = pi.GetValue(rec, null) ?? DBNull.Value;
                    }

                    dtReturn.Rows.Add(dr);
                }
                return dtReturn;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public LOTRI CreerUnLotri(CsLotri LeLotri, string matricule)
        {

            try
            {
                LOTRI lot = new LOTRI();
                lot.NUMLOTRI = LeLotri.NUMLOTRI;
                lot.CENTRE = LeLotri.CENTRE;
                lot.PERIODE = LeLotri.PERIODE;
                lot.PRODUIT = LeLotri.PRODUIT;
                lot.PERIODICITE = LeLotri.PERIODICITE;
                lot.CATEGORIECLIENT = LeLotri.CATEGORIECLIENT;
                lot.TOURNEE = LeLotri.TOURNEE;
                lot.BASE = LeLotri.BASE;
                lot.USERCREATION = matricule;
                lot.DATECREATION = DateTime.Now;

                // récupération des foreign key
                lot.FK_IDCATEGORIECLIENT = LeLotri.FK_IDCATEGORIECLIENT;
                lot.FK_IDPRODUIT = LeLotri.FK_IDPRODUIT;
                lot.FK_IDRELEVEUR = LeLotri.FK_IDRELEVEUR;
                lot.FK_IDCENTRE = LeLotri.FK_IDCENTRE;
                lot.FK_IDTOURNEE  = LeLotri.FK_IDTOURNEE ;

                return lot;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ACTION CreerActionLotri(CsLotri LeLotri,string TypeAction, string matricule, int Nombre, decimal? montant)
        {

            try
            {
                ACTION Action = new ACTION();
                Action.JET = LeLotri.JET;
                Action.PERIODE = LeLotri.PERIODE;
                Action.ACTION1 = TypeAction; 
                Action.LOT = LeLotri.NUMLOTRI;
                Action.DATE1 = System.DateTime.Today.Date;
                Action.NOMBRE1 = Nombre;
                Action.NOMBRE2 = 0;
                Action.NOMBRE3 = 0;
                Action.MONTANT1 = montant;
                Action.MONTANT2 = 0;
                Action.MONTANT3 = 0;
                Action.MATRICULE = matricule;
                Action.PRODUIT = LeLotri.PRODUIT;
                Action.STATUT  = "O";

                return Action;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ACTION CreerActionEvenement(CsEvenement LeLotri, string TypeAction, string matricule, int Nombre, decimal? montant)
        {
            try
            {
                ACTION Action = new ACTION();
                //Action.JET = LeLotri.JET;
                Action.PERIODE = LeLotri.PERIODE;
                Action.ACTION1 = TypeAction;
                Action.LOT = LeLotri.LOTRI ;
                Action.DATE1 = System.DateTime.Today.Date;
                Action.NOMBRE1 = Nombre;
                Action.NOMBRE2 = 0;
                Action.NOMBRE3 = 0;
                Action.MONTANT1 = montant;
                Action.MONTANT2 = 0;
                Action.MONTANT3 = 0;
                Action.MATRICULE = matricule;
                Action.PRODUIT = LeLotri.PRODUIT;
                Action.STATUT = "O";

                return Action;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CsEvenement > IsBatchExistDansPagerie(string _NumLot)
        {
            try
            {
                DataTable dt = Galatee.Entity.Model.IndexProcedures.IsBatchExistDansPagerie(_NumLot);
                return Entities.GetEntityListFromQuery<CsEvenement>(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public CsTableReference ChargerTableDeReference(int Num, string Code, string Centre)
        {
            try
            {
                CsTableReference tableRef = new CsTableReference();
                DataTable dtFrequence = CommonProcedures.RetourneTousFrequence();
                DataTable dtProduit = CommonProcedures.RetourneTousProduit();
                DataTable dtCategorieclient = CommonProcedures.RetourneCategorieClient();

                tableRef.Produits.AddRange(Entities.GetEntityListFromQuery<CParametre>(dtProduit));
                tableRef.CategorieClient.AddRange(Entities.GetEntityListFromQuery<CParametre>(dtCategorieclient));
                tableRef.Frequences.AddRange(Entities.GetEntityListFromQuery<CParametre>(dtFrequence));

                return tableRef;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CsEvenement> ChargerListeDesEvenementsTsp(List<CsLotri> _lstLori, string sequence)
        {
            string clie = string.Empty;
            try
            {

                List<CsEvenement> ListeDesEvenement = ListeDesEvenementsTsp(_lstLori, sequence);
                return ListeDesEvenement.Where(t => t.INDEXPRECEDENTEFACTURE != null).ToList();
            }
            catch (Exception ex)
            {
              string   clieh = clie; 
                throw ex;
            }
        }


        //public List<CsEvenement > ChargerListeDesTransfertsp(CsLotri _lstLori)
        //{
        //    try
        //    {

        //        DataTable dt = IndexProcedures.ChargerListeDesTransfertsp(_lstLori);
        //        return Entities.GetEntityListFromQuery<CsEvenement>(dt);

        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public List<CsLotri > ChargerDistinctLotTsp(List<int> isCentre)
        {
            try
            {

               DataTable dt =IndexProcedures.ChargerDistinctLotTsp(isCentre);
               return Entities.GetEntityListFromQuery<CsLotri>(dt);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CsEvenement> ChargerListeDesEvenementsNonFacture(List<CsLotri> _lstLori, string sequence)
        {
            string clie = string.Empty;
            try
            {

                List<CsEvenement> ListeDesEvenement = ListeDesEvenements(_lstLori, sequence);

                Parallel.ForEach(ListeDesEvenement, items =>
                {
                    items.CONSOMOYENNEPRECEDENTEFACTURE = CalculeConsomationMoyenne(items.FK_IDCENTRE, items.CENTRE, items.CLIENT, items.ORDRE, items.PRODUIT, items.POINT);
                
                });
                return ListeDesEvenement.Where(t => t.INDEXPRECEDENTEFACTURE != null).ToList();
            }
            catch (Exception ex)
            {
                string clieh = clie;
                throw ex;
            }
        }

        public List<CsEvenement> ChargerListeDesEvenements(List<CsLotri> _lstLori, string sequence)
        {
            try
            {
               return  ListeDesEvenements(_lstLori, sequence);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CsEvenement> ChargerListeTransfert(List<CsLotri> _lstLori, string sequence)
        {
            string clie = string.Empty;
            try
            {

                List<CsEvenement> ListeDesEvenement = ListeDesEvenements(_lstLori, sequence);
                return ListeDesEvenement.Where(t => t.INDEXPRECEDENTEFACTURE != null).ToList();
            }
            catch (Exception ex)
            {
              string   clieh = clie; 
                throw ex;
            }
        }
        

        public CsEvenement RetourneDernierEvtFacture(int Fk_idCentre,string Centre, string client, string ordre, string produit, int point)
        {
            try
            {
                return IndexProcedures.RetourneListeEvenementDuClientFacture(Fk_idCentre,Centre, client, ordre, produit, point);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CsEvenement> ListeDesEvenementsPourSynchro(List<CsLotri> _lstLori, string sequence)
        {
            try
            {
                List<CsEvenement> _lstEvenement = new List<CsEvenement>();
                foreach (var item in _lstLori)
                {
                    DataTable dt = IndexProcedures.RetourneListeEvenementDuLotPourSaisiIndex(item, sequence);
                    _lstEvenement.AddRange(Entities.GetEntityListFromQuery<CsEvenement>(dt));
                }
                return _lstEvenement;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public List<CsEvenement> ListeDesEvenements(List<CsLotri> _lstLori, string sequence)
        //{
        //    try
        //    {
        //        List<CsEvenement> _lstEvenement = new List<CsEvenement>();
        //        int nombreClientTotal = 0;
        //        int nombreClienDejatTotal = 0;
        //        foreach (var item in _lstLori)
        //        {

        //            DataTable dtx = IndexProcedures.RetourneNbrClientLot(item, sequence);
        //            List<CsEvenement> nbres = Entities.GetEntityListFromQuery<CsEvenement>(dtx);
        //            nombreClientTotal = nombreClientTotal + nbres.Where(t=>t.INDEXPRECEDENTEFACTURE != null).ToList().Count();
        //            nombreClienDejatTotal = nombreClienDejatTotal + nbres.Where(t=>t.INDEXPRECEDENTEFACTURE != null && t.CAS != "##").ToList().Count();
                    
        //            DataTable dt = IndexProcedures.RetourneListeEvenementDuLotPourSaisiIndex(item, sequence);
        //            List<CsEvenement> lesEvtTourne = Entities.GetEntityListFromQuery<CsEvenement>(dt).Where(t=>t.INDEXPRECEDENTEFACTURE != null ).ToList();
        //            _lstEvenement.AddRange(lesEvtTourne.OrderBy(t => t.ORDTOUR).ThenBy(u => u.CENTRE).ThenBy(o => o.CLIENT).ThenBy(p => p.ORDRE).ThenBy(i => i.ORDREAFFICHAGE).ToList());
        //        }
        //        if (_lstLori.First().PRODUIT == Enumere.ElectriciteMT)
        //        {
        //            if (_lstEvenement.First().ORDREAFFICHAGE != 1)
        //            {
        //                DataTable dts = IndexProcedures.RetourneListeEvenementSaisiIndexClient(_lstEvenement.First(),_lstEvenement.First().ORDREAFFICHAGE);
        //                _lstEvenement.AddRange(Entities.GetEntityListFromQuery<CsEvenement>(dts));
        //            }
        //        }
        //        foreach (CsEvenement item in _lstEvenement)
        //        {
        //            item.NOMBRECLIENTLOT = nombreClientTotal;
        //            item.NBFAC  = nombreClienDejatTotal;
        //        }
        //        return _lstEvenement.Where(t=>t.INDEXPRECEDENTEFACTURE != null ).ToList();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public List<CsEvenement> ListeDesEvenements(List<CsLotri> _lstLori, string sequence)
        {
            try
            {
                List<CsEvenement> _lstEvenement = new List<CsEvenement>();
                foreach (var item in _lstLori)
                {
                    if (!IsLotIsole(item.NUMLOTRI))
                    {
                        DataTable dt = IndexProcedures.RetourneListeEvenementDuLotPourSaisiIndex(item, sequence);
                        List<CsEvenement> lesEvtTourne = Entities.GetEntityListFromQuery<CsEvenement>(dt);
                        _lstEvenement.AddRange(lesEvtTourne.Distinct().OrderBy(t => t.ORDTOUR).ThenBy(u => u.CENTRE).ThenBy(o => o.CLIENT).ThenBy(p => p.ORDRE).ThenBy(i => i.ORDREAFFICHAGE).ToList());
                    }
                    else
                    {
                        DataTable dt = IndexProcedures.RetourneListeEvenementDuLotIsolePourSaisiIndex(item, sequence);
                        List<CsEvenement> lesEvtTourne = Entities.GetEntityListFromQuery<CsEvenement>(dt);
                        _lstEvenement.AddRange(lesEvtTourne.OrderBy(t => t.ORDTOUR).ThenBy(u => u.CENTRE).ThenBy(o => o.CLIENT).ThenBy(p => p.ORDRE).ThenBy(i => i.ORDREAFFICHAGE).ToList());

                    }
                }
                return _lstEvenement.OrderBy(t => t.CENTRE).ThenBy(u => u.TOURNEE).ThenBy(u => u.ORDTOUR).ThenBy(o => o.CLIENT).ThenBy(p => p.ORDRE).ThenBy(i => i.ORDREAFFICHAGE).ToList();
                //return _lstEvenement.Where(t => t.INDEXPRECEDENTEFACTURE != null).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public List<CsEvenement> ChargerListeDesEvenementsNonFacture(List<CsCanalisation> leCompteur)
        {
            try
            {
                List<CsEvenement> _lstEvenement = new List<CsEvenement>();
                List<int> lstIdCompteur = new List<int>();
                foreach (var item in leCompteur)
                    lstIdCompteur.Add(item.PK_ID);

                DataTable dt = IndexProcedures.ChargerListeDesEvenementsNonFacture(lstIdCompteur);
                    _lstEvenement.AddRange(Entities.GetEntityListFromQuery<CsEvenement>(dt));
                
                return _lstEvenement;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CsEvenement> ListeDesEvenementsTsp(List<CsLotri> _lstLori, string sequence)
        {
            try
            {
                List<CsEvenement> _lstEvenement = new List<CsEvenement>();
                foreach (var item in _lstLori)
                {
                    DataTable dt = IndexProcedures.RetourneListeEvenementDuLotPourSaisiIndexTSP(item, sequence);
                    _lstEvenement.AddRange(Entities.GetEntityListFromQuery<CsEvenement>(dt));
                }
                return _lstEvenement;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public CsSaisiIndexIndividuel ListeDesEvenementsDuClient(string lotri,int fk_idcentre, string centre, string client, string produit, int point)
        {
            try
            {
                return IndexProcedures.RetourneListeEvenementDuClientPourSaisiIndex2(lotri,fk_idcentre, centre, client, null, produit, point);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CsHistorique> RetourneHistoriqueDeConsomation(int fk_idcentre, string Centre, string Client, string Ordre, string Produit, int Point)
        {
            try
            {
                DataTable dt = IndexProcedures.RetourneHistoriqueConsoPoint(fk_idcentre,Centre, Client, Ordre, Produit, Point);
                return Entities.GetEntityListFromQuery<CsHistorique>(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int? CalculeConsomationMoyenne(int Fk_Idcentre, string Centre, string Client, string ordre, string produit, int point)
        {
            int  cumMois=0;
            int ConsMois30Jr = 0;
            List<CsHistorique> Lsthistorique = RetourneHistoriqueDeConsomation(Fk_Idcentre, Centre, Client, ordre, produit, point);
            foreach (CsHistorique item in Lsthistorique)
            {
                cumMois++;
                ConsMois30Jr = ConsMois30Jr + ((item.CONSO.Value * item.NBREJOURFACTURE.Value ) / Enumere.NombreDejour);
            }
            if (ConsMois30Jr != 0)
                return (ConsMois30Jr / cumMois) ;
            else
                return 0;
        }

        private bool IsLotIsole(string Numlot)
        {
            if (Numlot == null)
                return false;
            string FactureIsoleIndex = "00002";
            string FactureResiliationIndex = "00001";
            string FactureAnnulatinIndex = "00003";
            if (new string[] { FactureIsoleIndex, FactureAnnulatinIndex, FactureResiliationIndex }.Contains(Numlot.Substring(Enumere.TailleCentre, (Enumere.TailleNumeroBatch - Enumere.TailleCentre))))
                return true;
            else
                return false;
        }

        public bool DechargementTsp(List<CsEvenement> LstEvt)
        {
            try
            {
                int res = -1;
                //List<Galatee.Entity.Model.PAGERI> pageris = new List<Galatee.Entity.Model.PAGERI>();
                List<Galatee.Entity.Model.TRANSFERT> events = new List<Galatee.Entity.Model.TRANSFERT>();
                foreach (CsEvenement item in LstEvt)
                {
                    item.USERCREATION = item.MATRICULE;
                    item.DATECREATION = System.DateTime.Now;
                    item.FK_IDSTATUTTRANSFERT = 2;

                    //pageris.Add(RetourneUnPagerie(item));
                    events.Add(Entities.ConvertObject<TRANSFERT, CsEvenement>(item));
                }

                using (galadbEntities context = new galadbEntities())
                {
                    //Entities.UpdateEntity<Galatee.Entity.Model.PAGERI>(pageris, context);
                    Entities.UpdateEntity<Galatee.Entity.Model.TRANSFERT>(events, context);
                    res = context.SaveChanges();
                    //MiseAJourAction(LstEvt.First());
                    return res == -1 ? false : true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        //public List<cs RetourneTransfertTSP(List<CsLotri> LstLots)
        //{
        //    try
        //    {
              
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public bool InsererTransferTsp(List<CsEvenement> LstEvt)
        {
            try
            {
                int res = -1;
                List<Galatee.Entity.Model.TRANSFERT> events = new List<Galatee.Entity.Model.TRANSFERT>();
                foreach (CsEvenement item in LstEvt)
                {
                    item.USERCREATION = item.MATRICULE;
                    item.CAS = "##";
                    item.DATECREATION = System.DateTime.Now;
                    item.FK_IDSTATUTTRANSFERT = 1;
                    events.Add(Entities.ConvertObject<TRANSFERT, CsEvenement>(item));
                }

                using (galadbEntities context = new galadbEntities())
                {
                    Entities.InsertEntity <Galatee.Entity.Model.TRANSFERT>(events, context);
                    res = context.SaveChanges();
                    return res == -1 ? false : true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool MisAJourEvenement(List<CsEvenement> LstEvt)
        {
            try
            {
                int res = -1;
                List<Galatee.Entity.Model.EVENEMENT> events = new List<Galatee.Entity.Model.EVENEMENT>();
                foreach (CsEvenement item in LstEvt)
                    events.Add(RetourneUnEvnemnt(item));

                using (galadbEntities context = new galadbEntities())
                {
                    Entities.UpdateEntity<Galatee.Entity.Model.EVENEMENT>(events, context);
                    res = context.SaveChanges();
                    return res == -1 ? false : true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        //public bool RepriseIndex(CsEvenement LstEvt)
        //{
        //    try
        //    {
        //        int res = -1;
        //        galadbEntities contextInter = new galadbEntities();
        //        Galatee.Entity.Model.EVENEMENT events = new Galatee.Entity.Model.EVENEMENT();
        //        events = RetourneUnEvnemnt(LstEvt);

        //        using (galadbEntities context = new galadbEntities())
        //        {
        //            Entities.UpdateEntity<Galatee.Entity.Model.EVENEMENT>(events, context);
        //            Galatee.Entity.Model.EVENEMENT leEvt = context.EVENEMENT.FirstOrDefault(u => u.FK_IDABON == events.FK_IDABON &&
        //                 (u.STATUS == Enumere.EvenementCree || u.STATUS == Enumere.EvenementReleve ||
        //                 u.STATUS == Enumere.EvenementDefacture || u.STATUS == Enumere.EvenementRejeter) && u.NUMEVENEMENT > events.NUMEVENEMENT);
        //            if (leEvt != null && leEvt.NUMEVENEMENT != 0)
        //                leEvt.INDEXPRECEDENTEFACTURE = events.INDEXEVT;
        //            res = context.SaveChanges();
        //            return res == -1 ? false : true;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return false;
        //    }
        //}






        public string ValiderRepriseIndex(CsDemande laDemande)
        {
            SqlCommand laCommande = DBBase.InitTransaction(ConnectionString);
            try
            {

                if (!ValiderRepriseIndex(laDemande.LstEvenement.First(), laCommande))
                    throw new Exception("Problème lors de la mise à jour des données");
                TransmettreDemande(laDemande.LaDemande.NUMDEM, laDemande.InfoDemande.FK_IDETAPEACTUELLE, laDemande.LaDemande.MATRICULE, laCommande);


                laCommande.Transaction.Commit();
                return "";
            }
            catch (Exception ex)
            {
                laCommande.Transaction.Rollback();
                return ex.Message;
            }
            finally
            {
                if (laCommande.Connection.State == ConnectionState.Open)
                    laCommande.Connection.Close();
                laCommande.Dispose();
            }
        }



        private bool ValiderRepriseIndex(CsEvenement Even, SqlCommand cmd)
        {
            cmd.CommandTimeout = 3000;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SPX_FAC_REPRISEINDEX";


            cmd.Parameters.Add("@FK_IDDEMANDE", SqlDbType.Int).Value = Even.FK_IDDEMANDE;
            cmd.Parameters.Add("@NUMEVENEMENT", SqlDbType.Int).Value = Even.NUMEVENEMENT;
            cmd.Parameters.Add("@PERIODE", SqlDbType.VarChar, 6).Value = Even.PERIODE;
            cmd.Parameters.Add("@MATRICULE", SqlDbType.VarChar, 6).Value = Even.USERMODIFICATION;
            cmd.Parameters.Add("@FK_IDABON", SqlDbType.Int).Value = Even.FK_IDABON;
            cmd.Parameters.Add("@FK_IDCOMPTEUR", SqlDbType.Int).Value = Even.FK_IDCOMPTEUR;
            cmd.Parameters.Add("@FK_IDCANALISATION", SqlDbType.Int).Value = Even.FK_IDCANALISATION;
            cmd.Parameters.Add("@INDEXEVT", SqlDbType.Int).Value = Even.INDEXEVT;
            cmd.Parameters.Add("@INDEXERRONE", SqlDbType.Int).Value = Even.INDEXEVTPRECEDENT;
            cmd.Parameters.Add("@DATEEVT", SqlDbType.DateTime).Value = Even.DATEEVT;

            int ReturnValue = -1;
            DBBase.SetDBNullParametre(cmd.Parameters);
            try
            {
                if (cmd.Connection.State == ConnectionState.Closed)
                    cmd.Connection.Open();
                ReturnValue = cmd.ExecuteNonQuery();
                return ReturnValue <= 0 ? false : true;
            }
            catch (Exception ex)
            {
                throw new Exception(cmd.CommandText + ":" + ex.Message);
            }
        }



        private void TransmettreDemande(string NUMDEM, int idEtapeActuel, string MATRICULE, SqlCommand cmds)
        {


            cmds.CommandTimeout = 3000;
            cmds.CommandType = CommandType.StoredProcedure;
            cmds.CommandText = "SPX_WKF_TRANSMETTRE_DEMANDE";
            if (cmds.Parameters != null && cmds.Parameters.Count != 0) cmds.Parameters.Clear();
            cmds.Parameters.Add("@NUMDEM", SqlDbType.VarChar, 20).Value = NUMDEM;
            cmds.Parameters.Add("@FK_IDETAPE", SqlDbType.Int).Value = idEtapeActuel;
            cmds.Parameters.Add("@MATRICULE", SqlDbType.VarChar, 6).Value = MATRICULE;

            DBBase.SetDBNullParametre(cmds.Parameters);
            try
            {
                if (cmds.Connection.State == ConnectionState.Closed)
                    cmds.Connection.Open();
                cmds.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        
        public bool RepriseIndex(CsEvenement LeEVt)
        {

            cn = new SqlConnection(ConnectionString);

            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandTimeout = 3000;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SPX_FAC_REPRISEINDEX";


            cmd.Parameters.Add("@FK_IDDEMANDE", SqlDbType.Int).Value = LeEVt.FK_IDDEMANDE;
            cmd.Parameters.Add("@NUMEVENEMENT", SqlDbType.Int).Value = LeEVt.NUMEVENEMENT;
            cmd.Parameters.Add("@PERIODE", SqlDbType.VarChar, 6).Value = LeEVt.PERIODE ;
            cmd.Parameters.Add("@MATRICULE", SqlDbType.VarChar, 6).Value = LeEVt.USERMODIFICATION;
            cmd.Parameters.Add("@FK_IDABON", SqlDbType.Int).Value = LeEVt.FK_IDABON;
            cmd.Parameters.Add("@FK_IDCOMPTEUR", SqlDbType.Int).Value = LeEVt.FK_IDCOMPTEUR;
            cmd.Parameters.Add("@FK_IDCANALISATION", SqlDbType.Int).Value = LeEVt.FK_IDCANALISATION;
            cmd.Parameters.Add("@INDEXEVT", SqlDbType.Int).Value = LeEVt.INDEXEVT;
            cmd.Parameters.Add("@INDEXERRONE", SqlDbType.Int).Value = LeEVt.INDEXEVTPRECEDENT;
            cmd.Parameters.Add("@DATEEVT", SqlDbType.DateTime).Value = LeEVt.DATEEVT;

            int ReturnValue = -1;
            DBBase.SetDBNullParametre(cmd.Parameters);
            try
            {
                if (cn.State == ConnectionState.Closed)
                    cn.Open();
                ReturnValue = cmd.ExecuteNonQuery ();
                return (ReturnValue > 0);
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


        public bool CreerRepriseIndex(CsEvenement LeEVt)
        {

            cn = new SqlConnection(ConnectionString);

            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandTimeout = 3000;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SPX_FAC_INSERER_REPRISEINDEX";


            cmd.Parameters.Add("@IdEvenement", SqlDbType.Int).Value = LeEVt.PK_ID;
            cmd.Parameters.Add("@IndexEvenement", SqlDbType.Int).Value = LeEVt.INDEXEVT;
            cmd.Parameters.Add("@Matricule", SqlDbType.VarChar, 6).Value = LeEVt.USERMODIFICATION;

            int ReturnValue = -1;
            DBBase.SetDBNullParametre(cmd.Parameters);
            try
            {
                if (cn.State == ConnectionState.Closed)
                    cn.Open();
                ReturnValue = cmd.ExecuteNonQuery();
                return ReturnValue == -1 ? false : true;
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


        public bool MisAJourEvenementSynchoTSP(List<CsEvenement> LstEvt)
        {
            try
            {
                galadbEntities ctxinter = new galadbEntities();
                List<TRANSFERT> lstTrfer = new List<TRANSFERT>();

                int res = -1;

                LstEvt.ForEach(t => t.STATUS = 1);

                List<Galatee.Entity.Model.EVENEMENT> lstEvtTSP = Entities.ConvertObject<Galatee.Entity.Model.EVENEMENT, CsEvenement>(LstEvt);
                List<int> lstIdEvtTransfer = LstEvt.Select(t => t.PK_ID).ToList();
                lstTrfer = ctxinter.TRANSFERT.Where(t => lstIdEvtTransfer.Contains(t.PK_ID)).ToList();
                ctxinter.Dispose();

                using (galadbEntities context = new galadbEntities())
                {
                    Entities.UpdateEntity<Galatee.Entity.Model.EVENEMENT>(lstEvtTSP, context);
                    if (lstTrfer.Count != 0)
                        Entities.DeleteEntity <Galatee.Entity.Model.TRANSFERT>(lstTrfer, context);

                    res = context.SaveChanges();
                    return res == -1 ? false : true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        public bool MisAJourEvenementIndex(List<CsEvenement> LstEvt)
        {
            try
            {
                int res = -1;
                List<Galatee.Entity.Model.EVENEMENT> events = new List<Galatee.Entity.Model.EVENEMENT>();
                List<Galatee.Entity.Model.EVENEMENT> eventsInserer = new List<Galatee.Entity.Model.EVENEMENT>();
                foreach (CsEvenement item in LstEvt)
                {
                    events.Add(RetourneUnEvnemnt(item));
                    if (item.CAS == "13" && item.ISEVTPOSETROUVE)
                        eventsInserer.Add(Entities.ConvertObject<Galatee.Entity.Model.EVENEMENT, CsEvenement>(item));
                }

                using (galadbEntities context = new galadbEntities())
                {
                    Entities.UpdateEntity<Galatee.Entity.Model.EVENEMENT>(events, context);
                    Entities.InsertEntity<Galatee.Entity.Model.EVENEMENT>(eventsInserer, context);
                    res = context.SaveChanges();
                    if (events.First().STATUS != 8)
                    MiseAJourAction(LstEvt.First());
                    return res == -1 ? false : true ;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        private void MiseAJourAction(CsEvenement leEvenement)
        {
            int Nombre = 0;
            ACTION leAction = new ACTION();
            using (galadbEntities contexts = new galadbEntities())
            {
                Nombre = contexts.EVENEMENT.Where(t => t.LOTRI == leEvenement.LOTRI && t.PRODUIT == leEvenement.PRODUIT && t.CAS != "##" && t.USERMODIFICATION == leEvenement.MATRICULE ).Count();
                leAction = contexts.ACTION.FirstOrDefault(t => t.LOT == leEvenement.LOTRI && t.PRODUIT == leEvenement.PRODUIT && t.MATRICULE == leEvenement.MATRICULE  && t.ACTION1 == Enumere.ACTION_SAISIE_INDEX);
            }
            if (leAction != null)
            {
                leAction.NOMBRE1 = Nombre;
                Entities.UpdateEntity<Galatee.Entity.Model.ACTION>(leAction);
            }
            else
            {
                ACTION leAct = CreerActionEvenement(leEvenement, Enumere.ACTION_SAISIE_INDEX, leEvenement.MATRICULE, Nombre, 0);
                Entities.InsertEntity<Galatee.Entity.Model.ACTION>(leAct);
            }
        }
        public  Galatee.Entity.Model.EVENEMENT RetourneUnEvnemnt(CsEvenement item)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    Galatee.Entity.Model.EVENEMENT events;
                    if (item.PK_ID != null)
                        events = context.EVENEMENT.FirstOrDefault(e => e.PK_ID == item.PK_ID);
                    else
                        events = context.EVENEMENT.FirstOrDefault(e => e.CENTRE == item.CENTRE && e.CLIENT == item.CLIENT && e.PRODUIT == item.PRODUIT && e.POINT == item.POINT
                                                                      && e.LOTRI == item.LOTRI);

                    if (item.CAS == "13" && item.ISEVTPOSETROUVE  )
                    {
                        events.FK_IDCANALISATION = item.FK_IDCANALISATION.Value ;
                        events.INDEXPRECEDENTEFACTURE = item.INDEXPRECEDENTEFACTURE;
                        events.CASPRECEDENTEFACTURE = item.CASPRECEDENTEFACTURE;
                        events.DATERELEVEPRECEDENTEFACTURE = item.DATERELEVEPRECEDENTEFACTURE;
                        events.PERIODEPRECEDENTEFACTURE = item.PERIODEPRECEDENTEFACTURE;
                        events.NOUVEAUCOMPTEUR = item.NOUVCOMPTEUR;
                    }
                    events.NOUVEAUCOMPTEUR = item.NOUVCOMPTEUR;
                    events.DATEEVT = item.DATEEVT;
                    events.MATRICULE  = item.MATRICULE;
                    events.INDEXEVT = item.INDEXEVT;
                    events.CAS = item.CAS;
                    events.STATUS = item.STATUS;
                    events.CONSO = item.CONSO;
                    events.ENQUETE = item.ENQUETE;
                    events.DATEMODIFICATION = DateTime.Now;
                    events.USERMODIFICATION = item.MATRICULE;
                    return events;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public Galatee.Entity.Model.EVENEMENT RetourneUnEvnemntSeul(CsEvenement item)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    Galatee.Entity.Model.EVENEMENT events;
                    if (item.PK_ID != null)
                        events = context.EVENEMENT.FirstOrDefault(e => e.PK_ID == item.PK_ID);
                    else
                        events = context.EVENEMENT.FirstOrDefault(e => e.CENTRE == item.CENTRE && e.CLIENT == item.CLIENT && e.PRODUIT == item.PRODUIT && e.POINT == item.POINT
                                                                      && e.LOTRI == item.LOTRI);

                    events.NOUVEAUCOMPTEUR = item.NOUVCOMPTEUR;
                    events.DATEEVT = item.DATEEVT;
                    events.MATRICULE = item.MATRICULE;
                    events.INDEXEVT = item.INDEXEVT;
                    events.CAS = item.CAS;
                    events.STATUS = item.STATUS;
                    events.CONSO = item.CONSO;
                    events.ENQUETE = item.ENQUETE;
                    events.DATEMODIFICATION = DateTime.Now;
                    events.USERMODIFICATION = item.MATRICULE;
                    return events;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private int RetourneNumeroEvenementMax(CsEvenement item)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    return context.EVENEMENT.Where(e => e.CENTRE == item.CENTRE & e.CLIENT == item.CLIENT &&
                                                                     e.ORDRE == item.ORDRE && e.POINT == item.POINT).Max(e => e.NUMEVENEMENT);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public bool InsererPagerie(galadbEntities cmd, CsClientLotri Client, string Matricule)
        //{
        //    //cmd.CommandText = "SPX_IND_INSERER_PAGERIE";
        //    //cmd.Parameters.Add("@Lotri", SqlDbType.VarChar, 8).Value = (string.IsNullOrEmpty(Client.LOTRI)) ? null : Client.LOTRI;
        //    //cmd.Parameters.Add("@Centre", SqlDbType.VarChar, 3).Value = (string.IsNullOrEmpty(Client.CENTRE)) ? null : Client.CENTRE;
        //    //cmd.Parameters.Add("@Tournee", SqlDbType.VarChar, 15).Value = (string.IsNullOrEmpty(Client.TOURNEE)) ? null : Client.TOURNEE;
        //    //cmd.Parameters.Add("@OrdTour", SqlDbType.VarChar, 15).Value = (string.IsNullOrEmpty(Client.ORDTOUR)) ? null : Client.ORDTOUR;
        //    //cmd.Parameters.Add("@Ag", SqlDbType.VarChar, 20).Value = (string.IsNullOrEmpty(Client.CLIENT)) ? null : Client.CLIENT;
        //    //cmd.Parameters.Add("@Produit", SqlDbType.VarChar, 2).Value = (string.IsNullOrEmpty(Client.PRODUIT)) ? null : Client.PRODUIT;
        //    //cmd.Parameters.Add("@Point", SqlDbType.VarChar, 1).Value = Client.POINT;
        //    //cmd.Parameters.Add("@CasRel", SqlDbType.VarChar, 2).Value = (string.IsNullOrEmpty(Client.CAS)) ? null : Client.CAS;
        //    //cmd.Parameters.Add("@TopEdit", SqlDbType.VarChar, 1).Value = (string.IsNullOrEmpty(Client.TOPEDIT)) ? null : Client.TOPEDIT;
        //    //cmd.Parameters.Add("@Status", SqlDbType.VarChar, 1).Value = (string.IsNullOrEmpty(Client.STATUS)) ? null : Client.STATUS;
        //    //cmd.Parameters.Add("@Catcli", SqlDbType.VarChar, 2).Value = (string.IsNullOrEmpty(Client.CATCLI)) ? null : Client.CATCLI;
        //    //cmd.Parameters.Add("@Periodicite", SqlDbType.VarChar, 2).Value = (string.IsNullOrEmpty(Client.PERFAC)) ? null : Client.PERFAC;

        //    try
        //    {
        //        Galatee.Entity.Model.PAGERI pageri = new Galatee.Entity.Model.PAGERI();
        //        pageri.LOTRI = Client.LOTRI;
        //        pageri.CENTRE = Client.CENTRE;
        //        pageri.TOURNEE = Client.TOURNEE;
        //        pageri.ORDTOUR = Client.ORDTOUR;
        //        pageri.CLIENT = Client.CLIENT;
        //        pageri.PRODUIT = Client.PRODUIT;
        //        pageri.POINT = Client.POINT;
        //        pageri.CAS = Client.CAS;
        //        pageri.TOPEDIT = Client.TOPEDIT;
        //        pageri.STATUT = Client.STATUS.ToString();
        //        pageri.CATEGORIECLIENT = Client.CATEGORIECLIENT;
        //        pageri.PERIODICITE = Client.PERFAC;
        //        pageri.USERCREATION = Matricule;
        //        pageri.DATECREATION = DateTime.Now;

        //        // récupération des foreign key

        //        pageri.FK_IDCATEGORIECLIENT = Client.FK_IDCATEGORIE;
        //        pageri.FK_IDCENTRE = Client.FK_IDCENTRE;
        //        pageri.FK_IDPRODUIT = Client.FK_IDPRODUIT;

        //        return Entities.InsertEntity<Galatee.Entity.Model.PAGERI>(pageri, cmd);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public bool? CreationClientIndexHorsLot(CsEvenement ev)
        {
            //DataTable dt = ListeDesLotCandidatLoriDuClient(ev.CENTRE, ev.CLIENT, ev.PRODUIT, ev.POINT, ev.LOTRI);
            //List<CsClientLotri> LesClientDuLot = Entities.GetEntityListFromQuery<CsClientLotri>(dt);

            //// éliminer ensuite les clients dont la période correspondant à celle en paramètre
            //// NB en cas de facture isolée , on peut facturer un client à une période T 
            //// si la période de facturation actuelle correspond à celle de la facture isolée , on rejette le client 

            //List<CsClientLotri> lotTemoin = new List<CsClientLotri>();
            //List<CsClientLotri> _LesClientDuLot = new List<CsClientLotri>();

            //foreach (var point in LesClientDuLot)
            //{
            //    if (LesClientDuLot.FirstOrDefault(l => l.CENTRE == point.CENTRE && l.CLIENT == point.CLIENT &&
            //                    l.ORDRE == point.ORDRE && l.PERIODE == ev.PERIODE) == null)
            //    {
            //        if (lotTemoin.FirstOrDefault(l => l.CENTRE == point.CENTRE && l.CLIENT == point.CLIENT &&
            //                                        l.ORDRE == point.ORDRE && l.PRODUIT == point.PRODUIT &&
            //                                        l.FK_POINT == point.FK_POINT) == null)
            //        {
            //            point.MAXEVENT = LesClientDuLot.Where(e => e.CENTRE == point.CENTRE &&
            //                                                    e.CLIENT == point.CLIENT && e.ORDRE == point.ORDRE &&
            //                                                    e.PRODUIT == point.PRODUIT && e.FK_POINT == point.FK_POINT).Max(e => e.NUMEVENEMENT.Value);
            //            lotTemoin.Add(point);
            //            _LesClientDuLot.Add(point);
            //        }
            //    }
            //}

            //if (_LesClientDuLot.Count != 0)
            //{
            //    // fin du traitement de la liste LesClientDuLot
            //    // Debut creation des evenements relatifs
            //    List<EVENEMENT> eventToInsert = new List<EVENEMENT>();
            //    List<PAGERI> pageriToInsert = new List<PAGERI>();
            //    foreach (var _leClient in _LesClientDuLot)
            //    {
            //CsEvenement LeDernierEvt = RetournerDernierEvent(_leClient, ev.MATRICULE, ev.PERIODE);
            //pageriToInsert.Add(CreerPagerie(LeDernierEvt, ev.MATRICULE));
            //    }
            using (galadbEntities context = new galadbEntities())
            {
                Entities.InsertEntity<Galatee.Entity.Model.EVENEMENT>(CreationUnEvenementHorsLigne(ev), context);

                context.SaveChanges();
            }
            //}
            return true;
        }

        private CsAbon RetourneAbonEvenement(CsEvenement Client)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    var abon =
                               from ag in context.AG
                               from client in ag.CLIENT1
                               from Abon in client.ABON
                               from canal in Abon.CANALISATION
                               where canal.PK_ID == Client.FK_IDCANALISATION &&
                                     Abon.DRES == null
                               select new CsAbon
                               {
                                   PERFAC = Abon.PERFAC,
                                   FK_IDCATEGORIECLIENT = client.FK_IDCATEGORIE,
                                   FK_IDTOURNEE = ag.FK_IDTOURNEE,
                                   ORDRE = ag.ORDTOUR
                               };
                    return abon.First();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool UpdateUnPagerie(SqlCommand cmd, CsPageri LaPagerie)
        {
            int rowsAffected = -1;

            cmd.Parameters.Clear();
            cmd.CommandTimeout = 240;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SPX_IND_UPDATE_PAGERIE";
            //cmd.Parameters.Add("@Lotri", SqlDbType.VarChar, 8).Value = (string.IsNullOrEmpty(LaPagerie.LOTRI)) ? null : LaPagerie.LOTRI;
            //cmd.Parameters.Add("@Centre", SqlDbType.VarChar, 3).Value = (string.IsNullOrEmpty(LaPagerie.CENTRE)) ? null : LaPagerie.CENTRE;
            //cmd.Parameters.Add("@Tournee", SqlDbType.VarChar, 15).Value = (string.IsNullOrEmpty(LaPagerie.TOURNEE)) ? null : LaPagerie.TOURNEE;
            //cmd.Parameters.Add("@OrdTour", SqlDbType.VarChar, 15).Value = (string.IsNullOrEmpty(LaPagerie.ORDTOUR)) ? null : LaPagerie.ORDTOUR;
            //cmd.Parameters.Add("@Ag", SqlDbType.VarChar, 20).Value = (string.IsNullOrEmpty(LaPagerie.CLIENT)) ? null : LaPagerie.CLIENT;
            //cmd.Parameters.Add("@Produit", SqlDbType.VarChar, 2).Value = (string.IsNullOrEmpty(LaPagerie.PRODUIT)) ? null : LaPagerie.PRODUIT;
            //cmd.Parameters.Add("@Point", SqlDbType.VarChar, 1).Value = LaPagerie.POINT;
            //cmd.Parameters.Add("@CasRel", SqlDbType.VarChar, 2).Value = (string.IsNullOrEmpty(LaPagerie.CASREL)) ? null : LaPagerie.CASREL;
            //cmd.Parameters.Add("@TopEdit", SqlDbType.VarChar, 1).Value = (string.IsNullOrEmpty(LaPagerie.TOPEDIT)) ? null : LaPagerie.TOPEDIT;
            //cmd.Parameters.Add("@Statut", SqlDbType.VarChar, 1).Value = (string.IsNullOrEmpty(LaPagerie.STATUT)) ? null : LaPagerie.STATUT;
            //cmd.Parameters.Add("@Catcli", SqlDbType.VarChar, 2).Value = (string.IsNullOrEmpty(LaPagerie.CATCLI)) ? null : LaPagerie.CATCLI;
            //cmd.Parameters.Add("@Periodicite", SqlDbType.VarChar, 2).Value = (string.IsNullOrEmpty(LaPagerie.PERIODICITE)) ? null : LaPagerie.PERIODICITE;
            DBBase.SetDBNullParametre(cmd.Parameters);

            try
            {
                rowsAffected = cmd.ExecuteNonQuery();
                return Convert.ToBoolean(rowsAffected);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //public Galatee.Entity.Model.PAGERI RetourneUnPagerie(CsEvenement Evt)
        //{
        //    //cmd.CommandText = "SPX_IND_RETOURNE_PAGERIE";

        //    try
        //    {
        //        using (galadbEntities context = new galadbEntities())
        //        {
        //            Galatee.Entity.Model.PAGERI pageri = context.PAGERI.FirstOrDefault(p => p.FK_IDEVENEMENT  == Evt.PK_ID );
        //            pageri.CAS = Evt.CAS;
        //            pageri.DATEEVT = Evt.DATEEVT;
        //            pageri.STATUT = Enumere.EvenementReleve.ToString();
        //            pageri.DATEMODIFICATION = DateTime.Now;
        //            pageri.USERMODIFICATION = Evt.MATRICULE;
        //            return pageri;
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public PAGISOL RetourneUnPagisol(CsEvenement Evt)
        //{
        //    //cmd.CommandText = "SPX_IND_RETOURNE_PAGERIE";

        //    try
        //    {
        //        using (galadbEntities context = new galadbEntities())
        //        {
        //            PAGISOL pagisol = context.PAGISOL.FirstOrDefault(p => p.FK_IDEVENEMENT  == Evt.PK_ID );
        //            pagisol.CAS = Evt.CAS;
        //            pagisol.STATUT = Enumere.EvenementReleve.ToString();
        //            pagisol.DATEMODIFICATION = DateTime.Now;
        //            pagisol.USERMODIFICATION = Evt.MATRICULE;
        //            return pagisol;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        public List<CsCanalisation> RetourneCanalisation(int Fk_IDcentre, string centre, string client, string produit, int? point)
        {
            //cmd.CommandText = "SPX_GUI_RETOURNE_CANALISATION";
            try
            {
                DataTable dt = IndexProcedures.RetourneCanalisation(Fk_IDcentre,centre, client, produit, point);
                return Entities.GetEntityListFromQuery<CsCanalisation>(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public CsEvenement RetourneEvenementPose(CsEvenement leEvt)
        {
            try
            {
                DataTable dt = IndexProcedures.RetourneEvenementPose(leEvt);
                return Entities.GetEntityFromQuery<CsEvenement>(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public bool  VerificationNumeroCompteur(CsEvenement leEvt)
        {
            try
            {
                DataTable dt = IndexProcedures.VerificationNumeroCompteur(leEvt);
                CsCanalisation leCompteur = Entities.GetEntityFromQuery<CsCanalisation>(dt);
                if (leCompteur.FK_IDCOMPTEUR  != leEvt.FK_IDCOMPTEUR )
                    return false;
                else
                    return true ;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public CsCanalisation RetourneCanalisationbyIdEvenement(int? Fk_IdCompteurEvt)
        {
            try
            {
                DataTable dt = IndexProcedures.RetourneCanalisationbyIdEvenement(Fk_IdCompteurEvt);
                return Entities.GetEntityFromQuery<CsCanalisation>(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public CsSaisiIndexIndividuel RetourneEvenementNonFact(CsCanalisation LaCananlisation)
        {
            CsSaisiIndexIndividuel saisiInd = ListeDesEvenementsDuClient(null,LaCananlisation.FK_IDCENTRE, LaCananlisation.CENTRE, LaCananlisation.CLIENT, LaCananlisation.PRODUIT, LaCananlisation.POINT);
            saisiInd.CONSOMOYENNE =CalculeConsomationMoyenne(LaCananlisation.FK_IDCENTRE ,LaCananlisation.CENTRE, LaCananlisation.CLIENT, LaCananlisation.ORDRE, LaCananlisation.PRODUIT, LaCananlisation.POINT);
            return saisiInd;
        }

        public List<CsLotri> ChargerLotriPourCalcul(List<int> lstCentreHabil)
        {
            //cmd.CommandText = "SPX_IND_CHARGER_DISTINCTLOTRI";
            try
            {
                DataTable dt = Galatee.Entity.Model.FacturationProcedure.ChargerLotriFacturation(lstCentreHabil);
                return Entities.GetEntityListFromQuery<CsLotri>(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CsLotri> ChargerLotri(List<int> lstCentreHabil)
        {
            //cmd.CommandText = "SPX_IND_CHARGER_DISTINCTLOTRI";
            try
            {
                DataTable dt = Galatee.Entity.Model.FacturationProcedure.ChargerLotri(lstCentreHabil);
                return Entities.GetEntityListFromQuery<CsLotri>(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CsLotri> ChargerLotriDefacturation(List<int> lstCentreHabil)
        {
            //cmd.CommandText = "SPX_IND_CHARGER_DISTINCTLOTRI";
            try
            {
                DataTable dt = Galatee.Entity.Model.FacturationProcedure.ChargerLotriDefacturation(lstCentreHabil);
                return Entities.GetEntityListFromQuery<CsLotri>(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CsLotri> ChargerLotriDejaFacture(List<int> lstCentreHabil,string Matricule)
        {
            //cmd.CommandText = "SPX_IND_CHARGER_DISTINCTLOTRI";
            try
            {
                DataTable dt = Galatee.Entity.Model.FacturationProcedure.ChargerLotriDejaFacture(lstCentreHabil, Matricule);
                List<CsLotri> lstLot = Entities.GetEntityListFromQuery<CsLotri>(dt);
                lstLot.Where(t=>t.DFAC == null).ToList().ForEach(t => t.DFAC = t.DATECREATION);
                return lstLot;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CsLotri> ChargerLotriDejaMiseAJour(List<int> lstCentreHabil, string Matricule)
        {
            //cmd.CommandText = "SPX_IND_CHARGER_DISTINCTLOTRI";
            try
            {
                DataTable dt = Galatee.Entity.Model.FacturationProcedure.ChargerLotriDejaFacture(lstCentreHabil, Matricule);
                return Entities.GetEntityListFromQuery<CsLotri>(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private List<CsEvenement> EditerUnCas(string Lotri, List<int> LesCentre, List<int?> Latournee, List<string > LstCas)
        {
            try
            {
                //DataTable dt = IndexProcedures.retourneDonneeDuCasIndex(Lotri, LesCentre, Latournee, Cas);
                //return Entities.GetEntityListFromQuery<CsEvenement>(dt);
                return EditionDesEqueteSpx(Lotri, LesCentre, Latournee, LstCas);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private List<CsEvenement> EditerUnCasConfirmer(string Lotri, List<int> LesCentre, List<int?> Latournee, string Cas)
        {
            try
            {
                DataTable dt = IndexProcedures.retourneDonneeDuCasIndexConfirmer(Lotri, LesCentre, Latournee, Cas);
                return Entities.GetEntityListFromQuery<CsEvenement>(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private List<CsEvenement> EditerLotCritere(CsLotri leLot, int TypeRequete)
        {
            try
            {
                if (TypeRequete == 1)
                    return  EditionDesRiSpx(leLot);
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CsCasind> ListeDesCas(string pLotri, List<int> LesCentre, List<int?> lesTournee, string TypeCas)
        {
            List<CsCasind> ListeDesCasRi = ListeDesCasUnCriter(pLotri, LesCentre, lesTournee, TypeCas);
            return ListeDesCasRi;
        }
        public List<CsEvenement > ListeDesEnquete(List<CsLotri> lotSelection)
        {
            List<CsEvenement> ListeDesEnque = new List<CsEvenement>();
            foreach (CsLotri leLot in lotSelection)
            {
                DataTable dt = IndexProcedures.ListeDesEnquete(leLot);
                ListeDesEnque.AddRange(Entities.GetEntityListFromQuery<CsEvenement>(dt));
            }
            return ListeDesEnque;
        }
        public List<CsEvenement> ListeDesEnqueteCasOnze(List<CsLotri> lotSelection)
        {
            List<CsEvenement> ListeDesEnque = new List<CsEvenement>();
            foreach (CsLotri leLot in lotSelection)
            {
                DataTable dt = IndexProcedures.ListeDesEnqueteCasOnze(leLot);
                ListeDesEnque.AddRange(Entities.GetEntityListFromQuery<CsEvenement>(dt));
            }
            return ListeDesEnque;
        }
        
        public List<CsCasind> ListeDesCasUnCriter(string pLotri, List<int> LesCentre, List<int?> lesTournee, string typeCas)
        {
            //cmd.CommandText = "SPX_IND_RETOURNE_CAS_PAGERIE";

            try
            {
                if (typeCas == "1") // Enquetable
                {
                    DataTable dt = IndexProcedures.RetourneCasEvement(pLotri, LesCentre, lesTournee);
                    List<CsCasind> lesCasDuLot = Entities.GetEntityListFromQuery<CsCasind>(dt);
                    if (lesCasDuLot != null && lesCasDuLot.Count != 0)
                        return lesCasDuLot.Where(t => t.ENQUETABLE).ToList();
                    else return null;

                }
                else if (typeCas == "2") // Enquetable
                {
                    DataTable dt = IndexProcedures.RetourneCasEvement(pLotri, LesCentre, lesTournee);
                    List<CsCasind> lesCasDuLot = Entities.GetEntityListFromQuery<CsCasind>(dt);
                    if (lesCasDuLot != null && lesCasDuLot.Count != 0)
                        return lesCasDuLot.Where(t => !t.ENQUETABLE).ToList();
                    else return null;
                }
                //else if (typeCas == "3") // confirmer
                //{
                //    DataTable dt = IndexProcedures.ListeDesEnqueteConfirmer(Lotri, LeCentre, LaTournee);
                //    return Entities.GetEntityListFromQuery<CsCasind>(dt);
                //}
                else return null;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CsEvenement> EditerLotGenerale(List<CsLotri> LstLotri, int typeRequete)
        {
            List<CsEvenement> ListDonneeEdition = new List<CsEvenement>();
            if (LstLotri.Count == 0) LstLotri.Add(new CsLotri());
            foreach (CsLotri leLot in LstLotri)
                ListDonneeEdition.AddRange(EditerLotCritere(leLot, typeRequete));
            return ListDonneeEdition.OrderBy(t => t.CENTRE).ThenBy(u => u.TOURNEE).ThenBy(u => u.ORDTOUR).ThenBy(o => o.CLIENT).ThenBy(p => p.ORDRE).ThenBy(i => i.ORDREAFFICHAGE).ToList();
        }
        public List<CsEvenement> EditerListeDesCas(string Lotri, List<int> LesCentre, List<int?> Latournee, List<CsCasind> ListCas)
        {
            try
            {
                return EditerUnCas(Lotri, LesCentre, Latournee, ListCas.Select(o=>o.CODE).ToList());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CsEvenement> EditerListeDesCasConfirmer(string Lotri, List<int> LesCentre, List<int?> Latournee, List<CsCasind> ListCas)
        {
            try
            {
                List<CsEvenement> ListeDesDonnes = new List<CsEvenement>();
                if (ListCas.Count == 0) ListCas.Add(new CsCasind());
                foreach (CsCasind LeCas in ListCas)
                    ListeDesDonnes.AddRange(EditerUnCasConfirmer(Lotri, LesCentre, Latournee, LeCas.CODE));
                return ListeDesDonnes;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public bool? UpdateUneTournee(CsTournee LaTournee)
        {
            try
            {
                TOURNEE tourne = Galatee.Tools.Utility.ConvertEntity<TOURNEE, CsTournee>(LaTournee);
                return Entities.UpdateEntity<TOURNEE>(tourne);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool? UpdateEvenement(List<CsEvenement> LeEvenement)
        {
            try
            {
                List<Galatee.Entity.Model.EVENEMENT> leEvent = new List<Entity.Model.EVENEMENT>();
                foreach (CsEvenement item in LeEvenement)
                    leEvent.Add(RetourneUnEvnemnt(item));
                return Entities.UpdateEntity<Galatee.Entity.Model.EVENEMENT>(leEvent);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public bool? InsererUneTournee(CsTournee LaTournee)
        {
            //cmd.CommandText = "SPX_IND_INSERER_TOURNEE";
            try
            {
                TOURNEE tourne = Galatee.Tools.Utility.ConvertEntity<TOURNEE, CsTournee>(LaTournee);
                using (galadbEntities ctx= new galadbEntities())
                {
                    var trne = ctx.TOURNEE.FirstOrDefault(t => t.CODE == LaTournee.CODE && t.FK_IDCENTRE == LaTournee.FK_IDCENTRE);
                    if (trne!=null)
                    {
                        return false;
                    }
                }
                return Entities.InsertEntity<TOURNEE>(tourne);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool SupprimerUneTournee(CsTournee LaTournee)
        {

            try
            {
                TOURNEE tourne = Galatee.Tools.Utility.ConvertEntity<TOURNEE, CsTournee>(LaTournee);
                tourne.SUPPRIMER = true;
                return Entities.UpdateEntity<TOURNEE>(tourne);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public List<CsLotri> RetournerListeLotNonSaisi(List<int> lsCentrePerimetre)
        {
            try
            {
                DataTable dt = IndexProcedures.RetournerListeLotNonSaisi(lsCentrePerimetre);
                return Entities.GetEntityListFromQuery<CsLotri>(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CsReleveur> RetourneReleveurCentre(List<int> Liscentre)
        {
            try
            {
                DataTable dt = CommonProcedures.RetourneReleveurCentre(Liscentre);
                return Entities.GetEntityListFromQuery<CsReleveur>(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool UpdateUnReleveur(CsReleveur LeReleveur)
        {
            try
            {
                Galatee.Entity.Model.RELEVEUR releveur = Galatee.Tools.Utility.ConvertEntity<Galatee.Entity.Model.RELEVEUR, CsReleveur>(LeReleveur);
                return Entities.UpdateEntity<Galatee.Entity.Model.RELEVEUR>(releveur);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool InsererUnReleveur(CsReleveur LeReleveur)
        {
            try
            {
                Galatee.Entity.Model.RELEVEUR releveur = Galatee.Tools.Utility.ConvertEntity<Galatee.Entity.Model.RELEVEUR, CsReleveur>(LeReleveur);
                return Entities.InsertEntity<Galatee.Entity.Model.RELEVEUR>(releveur);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool SupprimerUnReleveur(CsReleveur LeReleveur)
        {
            //cmd.CommandText = "SPX_IND_SUPPRIME_RELEVEUR";

            try
            {
                using(galadbEntities ctx=new galadbEntities())
                {

                    Galatee.Entity.Model.RELEVEUR releveur = Galatee.Tools.Utility.ConvertEntity<Galatee.Entity.Model.RELEVEUR, CsReleveur>(LeReleveur);
                    var affct=ctx.TOURNEERELEVEUR.Where(t => t.FK_IDRELEVEUR == releveur.PK_ID);
                    List<TOURNEERELEVEUR> lsttr = affct!=null?affct.ToList():null;
                    lsttr.ForEach(t => t.DATEFIN = DateTime.Now);
                    Entities.UpdateEntity<Galatee.Entity.Model.RELEVEUR>(releveur,ctx) ;
                    Entities.UpdateEntity<Galatee.Entity.Model.TOURNEERELEVEUR>(lsttr,ctx) ;
                    int resul_state=ctx.SaveChanges();
                    return resul_state != 1 ? false : true;
                }
               
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public CsEvenement  VerifieEvtNonFacture(int fk_idcentre, string centre, string client, string ordre, string produit,string Periode, int point)
        {
            try
            {
                return IndexProcedures.VerifieEvtNonFacture(fk_idcentre, centre, client, ordre, produit,Periode, point);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        public List<CsEvenement> GetEvenement(string PK_ID)
        {
            try
            {
                DataTable dt = IndexProcedures.GetEvenement(PK_ID);
                return Entities.GetEntityListFromQuery<CsEvenement>(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public List<CsLotri> RetourneListeLotSelonCritere(string NumLot, string Centre, string Tournee)
        {
            try
            {
                DataTable dt = IndexProcedures.RetourneListeLotSelonCritere(NumLot, Centre, Tournee);
                return Entities.GetEntityListFromQuery<CsLotri>(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public List<CsLotri> RetourneListeLotNonTraiteParReleveur(string NumPortable)
        {
            try
            {
                DataTable dt = IndexProcedures.RetourneListeLotNonTraiteParReleveur(NumPortable);
                return Entities.GetEntityListFromQuery<CsLotri>(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CsTournee > ChargerTourneeReleveur(int Fk_IdReleveur)
        {
            try
            {
                DataTable dt = IndexProcedures.ChargerTourneeReleveur(Fk_IdReleveur);
                return Entities.GetEntityListFromQuery<CsTournee>(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool VerifierSaisie(CsLotri leLot)
        {
            try
            {
                DataTable dt = IndexProcedures.VerifierSaisie(leLot);
                List<CsEvenement> leEvtSaisi = Entities.GetEntityListFromQuery<CsEvenement >(dt);
                if (leEvtSaisi != null && leEvtSaisi.Count != 0)
                    return false;
                else return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool SupprimeEvtLotri(List<CsLotri> lesLot)
        {
            try
            {
                var lesLotDist = lesLot.Select(y => new { y.FK_IDCENTRE, y.USERCREATION , y.NUMLOTRI }).Distinct();
                foreach (var item in lesLotDist)
                    SupprimerLotEvenementSpx(item.FK_IDCENTRE, item.USERCREATION, item.NUMLOTRI);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool EffacerEvtLotri(string UserConnecter)
        {
            try
            {
                bool returnValeur = true;
                List<Galatee.Entity.Model.EVENEMENT> lstEvt = new List<Entity.Model.EVENEMENT>();
                List<LOTRI> le = new List<LOTRI>();
                using (galadbEntities context = new galadbEntities())
                {
                    lstEvt.AddRange(context.EVENEMENT.Where(p => p.USERCREATION == UserConnecter &&
                                      p.STATUS == Enumere.EvenementDetruit).ToList());
                }

                using (galadbEntities ctx = new galadbEntities())
                {
                    Entities.DeleteEntity<Galatee.Entity.Model.EVENEMENT>(lstEvt, ctx);
                    ctx.SaveChanges();
                }

                return returnValeur;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public bool  SupprimerLotEvenementSpx(int IdCentre, string matricule, string lot)
        //{
        //    cn = new SqlConnection(ConnectionString);
        //    cmd = new SqlCommand();
        //    cmd.Connection = cn;
        //    cmd.CommandTimeout = 180;
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.CommandText = "SPX_FAC_SUPPRESSIONEVENEMENTLOT";

        //    cmd.Parameters.Add("@IdCentre", SqlDbType.Int).Value = IdCentre;
        //    cmd.Parameters.Add("@matricule", SqlDbType.VarChar, 6).Value = matricule;
        //    cmd.Parameters.Add("@lot", SqlDbType.VarChar ,8).Value = lot;

        //    DBBase.SetDBNullParametre(cmd.Parameters);
        //    try
        //    {
        //        if (cn.State == ConnectionState.Closed)
        //            cn.Open();
        //        int reader = -1;
        //         reader = cmd.ExecuteNonQuery ();

        //         return reader==-1?false :true ;

        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(cmd.CommandText + ":" + ex.Message);
        //    }
        //    finally
        //    {
        //        if (cn.State == ConnectionState.Open)
        //            cn.Close(); // Fermeture de la connection 
        //        cmd.Dispose();
        //    }
        //}
        public bool SupprimerLotEvenementSpx(int IdCentre, string matricule, string lot)
        {
            cn = new SqlConnection(ConnectionString);
            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandTimeout = 300;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SPX_FAC_SUPPRESSIONEVENEMENTLOT";

            cmd.Parameters.Add("@IdCentre", SqlDbType.Int).Value = IdCentre;
            cmd.Parameters.Add("@matricule", SqlDbType.VarChar, 6).Value = matricule;
            cmd.Parameters.Add("@lot", SqlDbType.VarChar, 8).Value = lot;

            DBBase.SetDBNullParametre(cmd.Parameters);
            try
            {
                if (cn.State == ConnectionState.Closed)
                    cn.Open();
                int reader = -1;
                reader = cmd.ExecuteNonQuery();

                return reader == -1 ? false : true;

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

        public List<CsLotri> ChargerLotriPourDelete(string matricule, string DebutLot, string Finlot)
        {
            try
            {
                cn = new SqlConnection(ConnectionString);

                cmd = new SqlCommand();
                cmd.Connection = cn;
                cmd.CommandTimeout = 3000;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SPX_FAC_CHARGERLOTSUPPRESSION";

                cmd.Parameters.Add("@matricule", SqlDbType.VarChar, 6).Value = matricule;
                cmd.Parameters.Add("@DebutLot", SqlDbType.VarChar, 8).Value = DebutLot;
                cmd.Parameters.Add("@Finlot", SqlDbType.VarChar,8).Value = Finlot;

                DBBase.SetDBNullParametre(cmd.Parameters);
                try
                {
                    if (cn.State == ConnectionState.Closed)
                        cn.Open();

                    SqlDataReader reader = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    //if (reader.Read())
                    dt.Load(reader);
                    return Entities.GetEntityListFromQuery<CsLotri>(dt);
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
        public void InsererAction(ACTION  leAction,galadbEntities ctx)
        {
            try
            {
                Entities.InsertEntity<ACTION>(leAction, ctx);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool  VerifiPeriodeExist(CsClientLotri  leClient)
        {
            try
            {
                return IndexProcedures.VerifiPeriodeExist(leClient);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public bool ValiderAffectation(List<CsTournee> lstTourne)
        {
            try
            {
                List<int> lstIdTourneSupprimer = new List<int>();
                int? LeReleveur = lstTourne.First().FK_IDRELEVEUR;
                foreach (CsTournee item in lstTourne)
	                lstIdTourneSupprimer.Add(item.FK_IDTOURNEE.Value);

                List<TOURNEERELEVEUR> TourneReleveurAFermer = new List<TOURNEERELEVEUR>();
                using (galadbEntities ctx = new galadbEntities())
                {
                    TourneReleveurAFermer = ctx.TOURNEERELEVEUR.Where(t =>t.FK_IDRELEVEUR !=LeReleveur && lstIdTourneSupprimer.Contains(t.FK_IDTOURNEE)).ToList();
                }
                List<Galatee.Entity.Model.TOURNEERELEVEUR> TourneReleveurAjouter = RetourneTourneeReleveur(lstTourne);
                TourneReleveurAFermer.Where(u=>u.DATEFIN== null).ToList().ForEach(t => t.DATEFIN = System.DateTime.Today);

                using (galadbEntities context = new galadbEntities())
                {
                    Entities.UpdateEntity<Galatee.Entity.Model.TOURNEERELEVEUR>(TourneReleveurAFermer, context);
                    Entities.InsertEntity <Galatee.Entity.Model.TOURNEERELEVEUR>(TourneReleveurAjouter, context);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return true;
        }

        private List<TOURNEERELEVEUR> RetourneTourneeReleveur(List<CsTournee> lstTournee)
        {
            List<TOURNEERELEVEUR> result = new List<TOURNEERELEVEUR>();
            foreach (CsTournee item in lstTournee)
            {
                result.Add(new TOURNEERELEVEUR() {
                     DATEDEBUT = item.DATEDEBUT.Value  ,
                     DATEFIN = item.DATEFIN ,
                     FK_IDRELEVEUR = item.FK_IDRELEVEUR.Value  ,
                     FK_IDTOURNEE = item.FK_IDTOURNEE.Value   ,
                     PK_ID = item .PK_ID 
                });
            }
            return result;
        
        }
        public List<CsReleveur> RetourneReleveurCentre_(int Idcentre,int id_user)
        {
            cn = new SqlConnection(ConnectionString);

            //string Centreid = DBBase.RetourneStringListeObject(ListIdcentre);
            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandTimeout = 3000;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SPX_FAC_GET_RELEVEUR";
            cmd.Parameters.Add("@fk_iduser", int.MaxValue).Value = id_user;
            cmd.Parameters.Add("@fk_idcentre", int.MaxValue).Value = Idcentre;
            DBBase.SetDBNullParametre(cmd.Parameters);
            try
            {
                if (cn.State == ConnectionState.Closed)
                    cn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);
                return Entities.GetEntityListFromQuery<CsReleveur>(dt); ;
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

        public List<CsTournee> ChargerAllTourneeReleveur(List<int> ListIdcentre)
        {
            cn = new SqlConnection(ConnectionString);

            string Centreid = DBBase.RetourneStringListeObject(ListIdcentre);
            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandTimeout = 3000;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SPX_RECOUV_TOURNEERELEVE";
            cmd.Parameters.Add("@IDCENTRELIST", SqlDbType.VarChar, int.MaxValue).Value = Centreid;
            DBBase.SetDBNullParametre(cmd.Parameters);
            try
            {
                if (cn.State == ConnectionState.Closed)
                    cn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);
                return Entities.GetEntityListFromQuery<CsTournee>(dt); ;
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
        public List<CsEvenement> EditionDesRiSpx(CsLotri leLot)
        {
            cn = new SqlConnection(ConnectionString);

            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandTimeout = 3000;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SPX_FAC_EDITIONINDEX";
            cmd.Parameters.Add("@NUMLOTRI", SqlDbType.VarChar, 8).Value = leLot.NUMLOTRI ;
            cmd.Parameters.Add("@CENTRE", SqlDbType.VarChar, 3).Value = leLot.CENTRE ;
            cmd.Parameters.Add("@FK_IDCENTRE", SqlDbType.Int).Value = leLot.FK_IDCENTRE ;
            cmd.Parameters.Add("@FK_IDTOURNEE", SqlDbType.Int).Value = leLot.FK_IDTOURNEE;
            cmd.Parameters.Add("@CATEGORIE", SqlDbType.VarChar, 2).Value = leLot.CATEGORIECLIENT ;
            cmd.Parameters.Add("@PRODUIT", SqlDbType.VarChar, 2).Value = leLot.PRODUIT ;
            cmd.Parameters.Add("@PERIODICITE", SqlDbType.VarChar, 1).Value = leLot.PERIODICITE ;

            DBBase.SetDBNullParametre(cmd.Parameters);
            try
            {
                if (cn.State == ConnectionState.Closed)
                    cn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);
                return Entities.GetEntityListFromQuery<CsEvenement>(dt); ;
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
        public List<CsEvenement> EditionDesEqueteSpx(string Lotri, List<int> LesCentre, List<int?> Latournee, List<string> ListCas)
        {
            cn = new SqlConnection(ConnectionString);
            List<int> lstTour = new List<int>();
            foreach (int? item in Latournee)
               lstTour.Add(item.Value ); 

            string idcentre = DBBase.RetourneStringListeObject(LesCentre);
            string idTourne = DBBase.RetourneStringListeObject(lstTour);
            string Cas = DBBase.RetourneStringListeObject(ListCas);

            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandTimeout = 3000;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SPX_FAC_EDITIONENQUETE";
            cmd.Parameters.Add("@NUMLOTRI", SqlDbType.VarChar, 8).Value = Lotri;
            cmd.Parameters.Add("@IDCENTRELIST", SqlDbType.VarChar, int.MaxValue).Value = idcentre;
            cmd.Parameters.Add("@IDTOURNEELIST", SqlDbType.VarChar, int.MaxValue).Value = idTourne;
            cmd.Parameters.Add("@CASLIST", SqlDbType.VarChar, int.MaxValue).Value = Cas;

            DBBase.SetDBNullParametre(cmd.Parameters);
            try
            {
                if (cn.State == ConnectionState.Closed)
                    cn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);
                return Entities.GetEntityListFromQuery<CsEvenement>(dt); ;
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
        #region TSP
        public List<CsLotri> ChargerLotTsp(string NumeroTps, List<string> LotDejaCharger, bool ReleveIndexChecked)
        {
            cn = new SqlConnection(ConnectionString);

            string lstLotCharger = DBBase.RetourneStringListeObject(LotDejaCharger);
            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandTimeout = 3000;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SPX_TSP_CHARGERLOT";

            cmd.Parameters.Add("@ListLotDejaCharge", SqlDbType.VarChar, int.MaxValue).Value = lstLotCharger;
            cmd.Parameters.Add("@NumroTerminal", SqlDbType.VarChar, 20).Value = NumeroTps;
            cmd.Parameters.Add("@IsReleveIndex", SqlDbType.Bit).Value = ReleveIndexChecked;
            DBBase.SetDBNullParametre(cmd.Parameters);
            try
            {
                if (cn.State == ConnectionState.Closed)
                    cn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);
                return Entities.GetEntityListFromQuery<CsLotri>(dt); ;
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


        public bool InsertTransfertData(string lotri, string Tournee)
        {
            cn = new SqlConnection(ConnectionString);

            //string lstLotCharger = DBBase.RetourneStringListeObject(LotDejaCharger);
            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandTimeout = 3000;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SPX_TSP_INSERTTRANSFERT";
            //cmd.Parameters.Add("@ListLotDejaCharge", SqlDbType.VarChar, int.MaxValue).Value = lstLotCharger;
            cmd.Parameters.Add("@LOTRI", SqlDbType.VarChar, 20).Value = lotri;
            cmd.Parameters.Add("@TOURNEE", SqlDbType.VarChar, 12).Value = Tournee;
            DBBase.SetDBNullParametre(cmd.Parameters);
            try
            {
                if (cn.State == ConnectionState.Closed)
                    cn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                //DataTable dt = new DataTable();
                //dt.Load(reader);
                //return Entities.GetEntityListFromQuery<CsLotri>(dt); ;
                return true;
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
            public List<CsEvenement> ChargerEvenementTsp(string Lotri,List<string> Tournee, bool IsEnquete)
            {

                string LesTournee = DBBase.RetourneStringListeObject(Tournee);

                cn = new SqlConnection(ConnectionString);
                cmd = new SqlCommand();
                cmd.Connection = cn;
                cmd.CommandTimeout = 3000;
                cmd.CommandType = CommandType.StoredProcedure;
                if (!IsEnquete )
                cmd.CommandText = "SPX_TSP_CHARGEREVENEMENT";
                else
                    cmd.CommandText = "SPX_TSP_CHARGERENQUETE";

                cmd.Parameters.Add("@LOTRI", SqlDbType.VarChar, 8).Value = Lotri;
                cmd.Parameters.Add("@LISTTOURNEE", SqlDbType.VarChar, int.MaxValue).Value = LesTournee;
                DBBase.SetDBNullParametre(cmd.Parameters);
                try
                {
                    if (cn.State == ConnectionState.Closed)
                        cn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(reader);
                    return Entities.GetEntityListFromQuery<CsEvenement>(dt); ;
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
            public bool ValiderChargementTSP(List<CsMapperTransfert>  Transfers)
            {
                //cmd = new SqlCommand();
                try
                {
                //    cn = new SqlConnection(ConnectionString);
                //    cmd.Connection = cn;
                //    cmd.CommandTimeout = 3000;
                //    cmd.Connection.Open();
                //    cmd.Transaction = cmd.Connection.BeginTransaction();

                //    List<TRANSFERT> lstTransfer = new List<TRANSFERT>();
                //    lstTransfer = Galatee.Tools.Utility.ConvertListType<TRANSFERT, CsMapperTransfert>(Transfers);
                //    Entities.InsertEntity<TRANSFERT>(lstTransfer, context);

                //    DataTable TablePere = Galatee.Tools.Utility.ListToDataTable(Transfers);
                //    Galatee.Tools.Utility.BulkInsertTable("TRANSFERT", TablePere, cmd.Connection, cmd.Transaction);
                //    cmd.Transaction.Commit();
                    STATUTTRANSFERT s = new STATUTTRANSFERT();
                    using (galadbEntities context=new galadbEntities())
                    {
                        s = context.STATUTTRANSFERT.First(c => c.CODE == "01");
                    }
                using(galadbEntities context=new galadbEntities())
	            {
		            List<TRANSFERT> lstTransfer = new List<TRANSFERT>();
                    //List<Galatee.Entity.Model.EVENEMENT> ev = new List<Galatee.Entity.Model.EVENEMENT>();
                    lstTransfer = Galatee.Tools.Utility.ConvertListType<TRANSFERT, CsMapperTransfert>(Transfers);
                    //foreach (var item in ev)
                    //{
                    //    CsMapperTransfert letrsft = Transfers.FirstOrDefault(c => c.PK_ID == item.PK_ID);
                    //   item.INDEXEVT              = letrsft.INDEXEVT            ;
                    //   item.CAS                   = letrsft.CAS                 ;
                    //   item.STATUS                = letrsft.STATUS              ;
                    //   item.CONSO                 = letrsft.CONSO               ;
                    //   item.ENQUETE               = letrsft.ENQUETE             ;
                    //   item.NOUVEAUCOMPTEUR       = letrsft.NOUVEAUCOMPTEUR     ;
                    //   item.DATEMODIFICATION      = letrsft.DATEMODIFICATION    ;
                    //   item.USERMODIFICATION      = letrsft.USERMODIFICATION    ;
                    //}
                   
                    lstTransfer.ForEach(c=>c.FK_IDSTATUTTRANSFERT=s.PK_ID);
                    Entities.UpdateEntity<TRANSFERT>(lstTransfer, context);
                    //Entities.UpdateEntity<Galatee.Entity.Model.EVENEMENT>(ev,context);

                    context.SaveChanges();
	            }
                    return true;
                }
                catch (Exception ex)
                {
                    //cmd.Transaction.Rollback();
                    throw ex;
                }
            }
        #endregion

            public List<CsStatutTransfert> RetourneListeDesStatusTransfert()
            {
                using (galadbEntities context=new galadbEntities())
                {

                    var query1 = context.STATUTTRANSFERT;
                    return Galatee.Tools.Utility.GetEntityListFromQuery<CsStatutTransfert>(Galatee.Tools.Utility.ListToDataTable<object>(query1)).ToList();

                }
            }

            public bool InsertTransfertData(List<CsTransfert> data)
            {
                try
                {
                    using (galadbEntities context = new galadbEntities())
                    {
                        var query1 = Galatee.Tools.Utility.ConvertListType<TRANSFERT, CsTransfert>(data);
                        query1.ForEach(c => context.TRANSFERT.Add(c));

                        return context.SaveChanges() != 1 ? false : true;
                    }
                }
                catch (Exception ex )
                {
                    throw ex;
                }
            }
    }
}


