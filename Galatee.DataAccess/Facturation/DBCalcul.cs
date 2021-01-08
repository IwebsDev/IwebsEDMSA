using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Galatee.Structure;
using System.Data.SqlClient;
using System.Data;
//using Microsoft.Reporting.WebForms;
using System.IO;
using Galatee.Tools;

using System.Threading;
using System.Threading.Tasks;
using Galatee.Entity.Model  ;
using System.Reflection;
using System.Web.Hosting;
using System.Runtime.CompilerServices;
using System.Diagnostics;


namespace Galatee.DataAccess
{
    public class DBCalcul
    {
        public DBCalcul()
        {
            try
            {
                //ConnectionString = GalateeConfig.ConnectionStrings[Enumere.ConnexionGALADB].ConnectionString;
                //ConnectionString = Session.GetSqlConnexionString();
                ConnectionString = Session.GetSqlConnexionString();

            }
            catch (Exception)
            {
                throw;
            }
        }
        private string ConnectionString;
        private SqlCommand cmd = null;

        public SqlCommand Cmd
        {
            get { return cmd; }
            set { cmd = value; }
        }
        private SqlConnection cn = null;
        #region Calcul Facture
        List<CsCasind> ListCas;
        List<CsCentre> lstCentre;
        StreamWriter monstream;
        List<CsRechercheTarif> _LstParamTa20;
        CsParametresGeneraux _LstParamTaListing;
        bool _IsGestionAutoCompteurHoraire = false;

        List<CsVariableDeTarification> _LstNiveauDeTarifCentre ;
        List<CsVariableDeTarification> _LstNiveauDeTarifCommunal ;
        List<CsVariableDeTarification> _LstNiveauDeTarifNationnale;
        List<CsCtarcompparperiode > _LstDesCtarCompPeriode;

        List<CsCtax> _lstCtax ;
        List<CsTarifFacturation> _LstTarifFacturation  ;
        List<CsDetailTarifFacturation> _LstDetailTarifFacturation  ;
        List<CsTypeComptage> lesTypeComptage;

        List<CsRedevance> _LstDesRedevance ;

        public bool verifieSaisieTotal(List<CsLotri> pListLot)
        {
            try
            {
                bool retour = false;
                //foreach (CsLotri item in pListLot)
                //{
                DataTable dt = FacturationProcedure.verifieSaisieTotal(pListLot);
                    List<CsLotri>LstSimulation = Entities.GetEntityListFromQuery<CsLotri>(dt);
                    if (LstSimulation != null && LstSimulation.Count != 0)
                    {
                        retour = true;
                        //break;
                    }
                //}
                return retour;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public bool verifieSimulation (List<CsLotri> pListLot)
        {
            try
            {
                bool retour = false;
                //foreach (CsLotri item in pListLot)
                //{
                DataTable dt = FacturationProcedure.verifieSimulation(pListLot);
                    List<CsLotri>LstSimulation = Entities.GetEntityListFromQuery<CsLotri>(dt);
                    if (LstSimulation != null && LstSimulation.Count != 0)
                    {
                        retour = true;
                        //break;
                    }
                //}
                return retour;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CsLotri> ChargerDetailLotri(List<int> pListIdCentre)
        {
            try
            {
                DataTable dt = FacturationProcedure.ChargerDetailLotri(pListIdCentre);
                return Entities.GetEntityListFromQuery<CsLotri>(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CsEvenement> ChargerDesEvenementPourRejet(List<CsLotri> lstLot)
        {
            try
            {
                List<CsEvenement> lstEvenement = new List<CsEvenement>();
                foreach (CsLotri item in lstLot)
                {
                    DataTable dt = FacturationProcedure.ChargerDesEvenementPourRejet(item);
                    lstEvenement.AddRange(Entities.GetEntityListFromQuery<CsEvenement>(dt));
                }
                return lstEvenement;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CsEvenement> ChargerDesEvenement(List<CsLotri> lstLot)
        {
            try
            {
                List<CsEvenement> lstEvenement = new List<CsEvenement>();
                foreach (CsLotri item in lstLot)
                {
                    DataTable dt = FacturationProcedure.ChargerDesEvenement(item);
                    lstEvenement.AddRange(Entities.GetEntityListFromQuery<CsEvenement>(dt));
                }
                return lstEvenement;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CsEvenement> ChargerDesEvenementClientLot(CsClient leClient,string NumeroLot)
        {
            try
            {
                DataTable dt = FacturationProcedure.ChargerDesEvenementClientLot(leClient, NumeroLot);
                    return Entities.GetEntityListFromQuery<CsEvenement>(dt);
              
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }




        public List<CsLotri> ChargerDetailLotriPourDefacturation(List<int> LstIdCentre)
        {
            //cmd.CommandText = "SPX_FAC_CHARGER_DETAILLOTRI";
            try
            {
                DataTable dt = FacturationProcedure.ChargerDetailLotriPourDefacturation(LstIdCentre);
                return Entities.GetEntityListFromQuery<CsLotri>(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        

        #region Info Evenement
        private List<CsEvenement> RecupererEvtAfacturer(CsPageri _LstPagerie)
        {
            try
            {
                List<CsEvenement> _ListEvtFact = new List<CsEvenement>();
                if (_LstPagerie != null)
                {
                    List<CsEvenement> LstEvenementFacturable = RecherCheEvenementAFacture(_LstPagerie);
                    List<CsEvenement> LstEvenementDejaFacturer = RecherCheEvenementDejaAFacture(_LstPagerie);
                }
                return _ListEvtFact;
            }
            catch (Exception ex)
            {
                return null;
                throw ex;
            }
        }
        public List<CsEvenement> RecherCheEvenementAFacture(CsPageri LaPagerieSelectione)
        {
            //cmd.CommandText = "SPX_FAC_RETOURNE_EVENEMENT_FACTURABLE";
            try
            {
                DataTable dt = FacturationProcedure.RetourneEvenementAFacture(LaPagerieSelectione.CENTRE, LaPagerieSelectione.CLIENT, LaPagerieSelectione.POINT, LaPagerieSelectione.PRODUIT, LaPagerieSelectione.LOTRI);
                return Entities.GetEntityListFromQuery<CsEvenement>(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CsEvenement> RecherCheEvenementDejaAFacture(CsPageri ListLotSelectione)
        {
            cn = new SqlConnection(ConnectionString);

            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandTimeout = 18000;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SPX_FAC_RETOURNE_EVENEMENT_DEJAFACTURE";
            //cmd.Parameters.Add("@CENTRE", SqlDbType.VarChar, 8).Value = ListLotSelectione.CENTRE;
            //cmd.Parameters.Add("@CLIENT", SqlDbType.VarChar, 8).Value = ListLotSelectione.CLIENT;
            //cmd.Parameters.Add("@POINT", SqlDbType.Int).Value = ListLotSelectione.POINT;
            //cmd.Parameters.Add("@PRODUIT", SqlDbType.VarChar, 8).Value = ListLotSelectione.PRODUIT;
            //cmd.Parameters.Add("@LOTRI", SqlDbType.VarChar, 8).Value = ListLotSelectione.LOTRI;
            DBBase.SetDBNullParametre(cmd.Parameters);
            try
            {
                if (cn.State == ConnectionState.Closed)
                    cn.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                List<CsEvenement> rows = new List<CsEvenement>();
                while (reader.Read())
                {
                    CsEvenement c = new CsEvenement();
                    //c.CENTRE = (Convert.IsDBNull(reader["CENTRE"])) ? String.Empty : (System.String)reader["CENTRE"];
                    //c.CLIENT = (Convert.IsDBNull(reader["AG"])) ? String.Empty : (System.String)reader["AG"];
                    //c.ORDRE = (Convert.IsDBNull(reader["ORDRE"])) ? String.Empty : (System.String)reader["ORDRE"];
                    //c.PRODUIT = (Convert.IsDBNull(reader["PRODUIT"])) ? String.Empty : (System.String)reader["PRODUIT"];
                    //c.NOMCLIENT = (Convert.IsDBNull(reader["NOMABON"])) ? String.Empty : (System.String)reader["NOMABON"];
                    //c.POINT = (Int16)reader["Point"];
                    //c.EVENEMENT = (Int16)reader["EVENEMENT"];
                    c.COMPTEUR = (Convert.IsDBNull(reader["COMPTEUR"])) ? String.Empty : (System.String)reader["COMPTEUR"];
                    //c.DATEEVT = (Convert.IsDBNull(reader["DATEEVT"])) ? String.Empty : (System.String)reader["DATEEVT"];
                    c.PERIODE = (Convert.IsDBNull(reader["PERIODE"])) ? String.Empty : (System.String)reader["PERIODE"];
                    c.CODEEVT = (Convert.IsDBNull(reader["CODEEVT"])) ? String.Empty : (System.String)reader["CODEEVT"];
                    c.INDEXEVT = 0; if (!(Convert.IsDBNull(reader["INDEXEVT"]))) c.INDEXEVT = (Int32)reader["INDEXEVT"];
                    //c.INDEXEVTPRECEDENT = 0; if (!(Convert.IsDBNull(reader["INDEXEVTPRECEDENT"]))) c.INDEXEVTPRECEDENT = (Int32)reader["INDEXEVTPRECEDENT"];
                    //c.CAS = (Convert.IsDBNull(reader["CAS"])) ? String.Empty : (System.String)reader["CAS"];
                    //c.CASPRECEDENT = (Convert.IsDBNull(reader["CASPRECEDENT"])) ? String.Empty : (System.String)reader["CASPRECEDENT"];
                    c.ENQUETE = (Convert.IsDBNull(reader["ENQUETE"])) ? String.Empty : (System.String)reader["ENQUETE"];
                    c.CONSO = null; if (!(Convert.IsDBNull(reader["CONSO"]))) c.CONSO = (Int32)reader["CONSO"];
                    //c.CONSOPRECEDENT = null; if (!(Convert.IsDBNull(reader["CONSOPRECEDENT"]))) c.CONSOPRECEDENT = (Int32)reader["CONSOPRECEDENT"];
                    c.CONSOFAC = null; if (!(Convert.IsDBNull(reader["CONSOFAC"]))) c.CONSOFAC = (Int32)reader["CONSOFAC"];
                    //c.CONSOFACPRECEDENT = null; if (!(Convert.IsDBNull(reader["CONSOFACPRECEDENT"]))) c.CONSOFACPRECEDENT = (Int32)reader["CONSOFACPRECEDENT"];
                    c.CONSONONFACTUREE = null; if (!(Convert.IsDBNull(reader["CONSONONFACTUREE"]))) c.CONSONONFACTUREE = (Int32)reader["CONSONONFACTUREE"];
                    c.LOTRI = (Convert.IsDBNull(reader["LOTRI"])) ? String.Empty : (System.String)reader["LOTRI"];
                    c.FACTURE = (Convert.IsDBNull(reader["FACTURE"])) ? String.Empty : (System.String)reader["FACTURE"];
                    c.SURFACTURATION = null; if (!(Convert.IsDBNull(reader["SURFACTURATION"]))) c.SURFACTURATION = (Int32)reader["SURFACTURATION"];
                    c.STATUS = null; if (!(Convert.IsDBNull(reader["STATUS"]))) c.STATUS = (Int16)reader["STATUS"];
                    c.TYPECONSO = null; if (!(Convert.IsDBNull(reader["TYPECONSO"]))) c.STATUS = (Int16)reader["TYPECONSO"];
                    //c.DIAMETRE = (Convert.IsDBNull(reader["DIAMETRE"])) ? String.Empty : (System.String)reader["DIAMETRE"];
                    //c.DMAJ = (Convert.IsDBNull(reader["DMAJ"])) ? String.Empty : (System.String)reader["DMAJ"];
                    //c.MATRICULE = (Convert.IsDBNull(reader["MATRICULE"])) ? String.Empty : (System.String)reader["MATRICULE"];
                    c.FACPER = (Convert.IsDBNull(reader["FACPER"])) ? String.Empty : (System.String)reader["FACPER"];
                    c.DERPERF = (Convert.IsDBNull(reader["DERPERF"])) ? String.Empty : (System.String)reader["DERPERF"];
                    c.DERPERFN = (Convert.IsDBNull(reader["DERPERFN"])) ? String.Empty : (System.String)reader["DERPERFN"];
                    c.QTEAREG = null; if (!(Convert.IsDBNull(reader["QTEAREG"]))) c.QTEAREG = (Int32)reader["QTEAREG"];
                    c.CONSOFAC = null; if (!(Convert.IsDBNull(reader["ConsoFac"]))) c.CONSOFAC = (Int32)reader["ConsoFac"];
                    c.REGIMPUTE = null; if (!(Convert.IsDBNull(reader["REGIMPUTE"]))) c.REGIMPUTE = (Int32)reader["REGIMPUTE"];
                    c.REGCONSO = null; if (!(Convert.IsDBNull(reader["REGCONSO"]))) c.REGCONSO = (Int32)reader["REGCONSO"];
                    c.COEFLECT = null; if (!(Convert.IsDBNull(reader["COEFLECT"]))) c.COEFLECT = (System.Decimal)reader["COEFLECT"];
                    //c.COEFCOMPTAGE    = null; if (!(Convert.IsDBNull(reader["COEFCOMPTAGE"]))) c.COEFCOMPTAGE = (Int32)reader["COEFCOMPTAGE"];
                    c.PUISSANCE = (Convert.IsDBNull(reader["PUISSANCE"])) ? 0 : (System.Decimal)reader["PUISSANCE"];
                    c.TYPECOMPTAGE = (Convert.IsDBNull(reader["TYPECOMPTAGE"])) ? String.Empty : (System.String)reader["TYPECOMPTAGE"];
                    //c.TCOMPT = (Convert.IsDBNull(reader["TCOMPT"])) ? String.Empty : (System.String)reader["TCOMPT"];
                    c.COEFK1 = (Convert.IsDBNull(reader["COEFK1"])) ? 0 : (System.Decimal)reader["COEFK1"];
                    c.COEFK2 = (Convert.IsDBNull(reader["COEFK2"])) ? 0 : (System.Decimal)reader["COEFK2"];
                    c.COEFFAC = (Convert.IsDBNull(reader["COEFFAC"])) ? 0 : (Int32)reader["COEFFAC"];
                    rows.Add(c);
                }
                reader.Close();
                return rows;
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
        #endregion
        #region InfoClient

        public CsClient RechercheClient(CsEvenement _leEvt)
        {
            try
            {
                return new DBAccueil().RetourneClient(_leEvt.FK_IDCENTRE ,_leEvt.CENTRE, _leEvt.CENTRE, _leEvt.PRODUIT);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
        #region Info general
        private List<CsVariableDeTarification> RetourneNiveauTarif(CsCentre  _LeCentre)
        {
            try
            {
                List<CsVariableDeTarification> _LstVariableDeTarif = new List<CsVariableDeTarification>();
                string _LeCentreDeRecherche = string.Empty;
                if (_LeCentre.CODENIVEAUTARIF == Enumere.NiveauTarif_Centre)
                    _LstVariableDeTarif = RetourneTarifCentre(_LeCentre);
                else if (_LeCentre.CODENIVEAUTARIF == Enumere.NiveauTarif_Communale)
                    _LstVariableDeTarif = RetourneTarifCommunal();
                else if (_LeCentre.CODENIVEAUTARIF == Enumere.NiveauTarif_Nat)
                    _LstVariableDeTarif = RetourneTarifNationnal();
                return _LstVariableDeTarif;
            }
            catch (Exception ex)
            {
                return null;
                throw;
            }
        }
        private List<CsVariableDeTarification> RetourneTarifNationnal()
        {

            //cmd.CommandText = "SPX_FAC_RETOURNE_NIVEAUTARIFNATIONNAL";
            try
            {
                DataTable dt = FacturationProcedure.ChargerNiveauTarifNationnal();
                return Entities.GetEntityListFromQuery<CsVariableDeTarification>(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CsRedevance> ChargerRedevance()
        {
            List<CsRedevance> list = new List<CsRedevance>();
            List<CsRedevance> nlist = new List<CsRedevance>();
            try
            {
                //return FacturationProcedure.LoadAllRedevance();
                list = FacturationProcedure.LoadAllRedevance();

                List<CsTrancheRedevence> tranches = null;

                foreach (CsRedevance st in list)
                {
                    tranches = RemplirTrancheRedevance(st.PK_ID);

                    if (tranches != null && tranches.Count > 0)
                    {
                        if (st.TRANCHEREDEVANCE == null)
                            st.TRANCHEREDEVANCE = new List<CsTrancheRedevence>();
                        st.TRANCHEREDEVANCE.AddRange(tranches);
                    }
                    nlist.Add(st);
                }
                return nlist;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        private List<CsTrancheRedevence> RemplirTrancheRedevance(int idRedevance)
        {
            cn = new SqlConnection(ConnectionString);

            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandTimeout = 18000;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SPX_GET_TRRANCHE_REDEVANCE";

            cmd.Parameters.Clear();

            cmd.Parameters.Add("@idRedevance", SqlDbType.Int).Value = idRedevance;

            DBBase.SetDBNullParametre(cmd.Parameters);
            try
            {
                if (cn.State == ConnectionState.Closed)
                    cn.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                List<CsTrancheRedevence> list = new List<CsTrancheRedevence>();
                CsTrancheRedevence c = null;

                while (reader.Read())
                {
                    c = new CsTrancheRedevence();

                    c.PK_ID = (Convert.IsDBNull(reader["PK_ID"])) ? 0 : (int)reader["PK_ID"];
                    c.FK_IDREDEVANCE = (Convert.IsDBNull(reader["FK_IDREDEVANCE"])) ? 0 : (int)reader["FK_IDREDEVANCE"];
                    c.ORDRE = (Convert.IsDBNull(reader["ORDRE"])) ? (byte)0 : (byte)reader["ORDRE"];
                    c.LIBELLE = (Convert.IsDBNull(reader["LIBELLE"])) ? string.Empty : (string)reader["LIBELLE"];
                    c.GRATUIT = (Convert.IsDBNull(reader["GRATUIT"])) ? false : (bool)reader["GRATUIT"];
                    list.Add(c);
                }

                reader.Close();

                return list;

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


        private List<CsVariableDeTarification> RetourneTarifCentre( CsCentre  centre)
        {
            //cmd.CommandText = "SPX_FAC_RETOURNE_NIVEAUTARIFCENTRE";
            try
            {
                DataTable dt = FacturationProcedure.ChargerNiveauTarifCentre(centre);
                return Entities.GetEntityListFromQuery<CsVariableDeTarification>(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private List<CsVariableDeTarification> RetourneAllTarifCentre()
        {
            //cmd.CommandText = "SPX_FAC_RETOURNE_NIVEAUTARIFCENTRE";
            try
            {
                DataTable dt = FacturationProcedure.ChargerAllNiveauTarifCentre();
                return Entities.GetEntityListFromQuery<CsVariableDeTarification>(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private List<CsCtarcompparperiode > ChargerAllCtarcompParPeriode()
        {
            //cmd.CommandText = "SPX_FAC_RETOURNE_NIVEAUTARIFCENTRE";
            try
            {
                DataTable dt = FacturationProcedure.ChargerAllCtarcompParPeriode();
                return Entities.GetEntityListFromQuery<CsCtarcompparperiode>(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        

        private List<CsVariableDeTarification> RetourneTarifCommunal()
        {
            //cmd.CommandText = "SPX_FAC_RETOURNE_NIVEAUTARIFCOMMUNE";
            try
            {
                DataTable dt = FacturationProcedure.ChargerNiveauTarifCommune();
                return Entities.GetEntityListFromQuery<CsVariableDeTarification>(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private List<CsVariableDeTarification> RetourneListTarvar()
        {
            //cmd.CommandText = "SPX_FAC_RETOURNE_TARVAR";
            try
            {
                DataTable dt = FacturationProcedure.ChargerVariableTarification();
                return Entities.GetEntityListFromQuery<CsVariableDeTarification>(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private List<CsRechercheTarif> RetourneModeRecherCherTarif()
        {
            try
            {
                return new DBRECHERCHETARIF().SelectAllRechercheTarif();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private List<CsCtarcomp> RetourneListConstitutionTarifByIdrecherche(int idrecherche)
        {
            //cmd.CommandText = "SPX_FAC_RETOURNE_TARVAR";
            try
            {
                DataTable dt = FacturationProcedure.RetourneConstitution(idrecherche);
                return Entities.GetEntityListFromQuery<CsCtarcomp>(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private CsContenantCritereTarif  RetourneListContenantTableByCtarcompt(int idCtarcomp)
        {
            //cmd.CommandText = "SPX_FAC_RETOURNE_TARVAR";
            try
            {
                DataTable dt = FacturationProcedure.RetourneContenantTarif(idCtarcomp);
                return Entities.GetEntityFromQuery <CsContenantCritereTarif>(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private CsInfoAbonFact RetourneInfoAbon(CsEvenement _leEvt)
        {
            //cmd.CommandText = "SPX_FAC_RETOURNE_INFOABONNE";
            try
            {
                CsInfoAbonFact _LaDonnee = new CsInfoAbonFact();
                galadbEntities leContext = new galadbEntities();
                AG dt = Galatee.Entity.Model.FacturationProcedure.RetourneInfoAg(_leEvt.CENTRE, _leEvt.CLIENT, _leEvt.ORDRE, leContext);
                if (dt != null)
                {
                    if (dt.CLIENT1  != null && dt.CLIENT1 .Count() != 0)
                        _LaDonnee.Client  = Entities.ConvertObject<CsClient , Galatee.Entity.Model.CLIENT >(dt.CLIENT1.FirstOrDefault(t=>t.ORDRE ==_leEvt.ORDRE));

                     _LaDonnee.AdresseGeographique = Entities.ConvertObject<CsAg, Galatee.Entity.Model.AG>(dt);

                    if (dt.BRT != null && dt.BRT.Count() != 0)
                        _LaDonnee.Branchement = Entities.ConvertObject<CsBrt, Galatee.Entity.Model.BRT>(dt.BRT.FirstOrDefault(t => t.FK_IDPRODUIT  == _leEvt.FK_IDPRODUIT ));

                    CLIENT  dts = Galatee.Entity.Model.FacturationProcedure.RetourneInfoClient(_leEvt.CENTRE, _leEvt.CLIENT, _leEvt.ORDRE, leContext);
                    if (dts != null)
                    {
                        if (dts.ABON != null && dts.ABON.Count() != 0)
                        {
                            _LaDonnee.Abonne = Entities.ConvertObject<CsAbon, Galatee.Entity.Model.ABON>(dts.ABON.FirstOrDefault(t => t.PRODUIT == _leEvt.PRODUIT && t.ORDRE == _leEvt.ORDRE ));  
                  
                        if (dts.ABON.FirstOrDefault (t=>t.PRODUIT== _leEvt.PRODUIT).CANALISATION != null )
                            _LaDonnee.Compteur = Entities.ConvertObject<CsCanalisation, Galatee.Entity.Model.CANALISATION>(dts.ABON.FirstOrDefault(t => t.PRODUIT == _leEvt.PRODUIT && t.ORDRE == _leEvt.ORDRE).CANALISATION.FirstOrDefault (y=>y.POINT == _leEvt.POINT ));  
                        }
                    }
                    leContext.Dispose();
                }
                return _LaDonnee;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //public CsPageri RetournePagerieClientLot(string pCentre, string pClient, string pProduit, int pPoint, string plotri)
        //{
        //    try
        //    {
        //        DataTable dt = FacturationProcedure.RetournePagerieClientLot(pCentre, pClient, pProduit, pPoint, plotri);
        //        return Entities.GetEntityFromQuery<CsPageri>(dt);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //}
        public CsPageri RetourneTourneClient()
        {
            try
            {
                DataTable dt = CommonProcedures.RetourneListeTournee( );
                return Entities.GetEntityFromQuery<CsPageri>(dt);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        private List<CsVariableDeTarification> RetourneVariableDeRechercherTarifParProduit(string _produit, List<CsVariableDeTarification> _LstNiveauTarif)
        {
            List<CsVariableDeTarification> _LstNiveauTarifProduit = new List<CsVariableDeTarification>();
            try
            {
                if (_LstNiveauTarif != null && _LstNiveauTarif.Count != 0)
                    _LstNiveauTarifProduit = _LstNiveauTarif.Where(p => p.PRODUIT == _produit).ToList();
                return _LstNiveauTarifProduit;
            }
            catch (Exception ex)
            {
                return _LstNiveauTarifProduit;
            }

        }
        private List<CsVariableDeTarification> ConstruireCtarComp(List<CsVariableDeTarification> LstTarVar, List<CsRechercheTarif> _LstTa20, CsEvenement _LeEvtProduit)
        {
            try
            {
                foreach (CsVariableDeTarification item in LstTarVar)
                {
                    if (_LstTa20 != null && _LstTa20.Count != 0)
                    {
                        string CtarComp = string.Empty;
                        CsRechercheTarif _LstParamFact = _LstTa20.FirstOrDefault(p => p.CODE  == item.RECHERCHETARIF);
                        List<CsCtarcomp> lstConstitutionRecherche = RetourneListConstitutionTarifByIdrecherche(_LstParamFact.PK_ID);
                        foreach (CsCtarcomp items in lstConstitutionRecherche.OrderBy(t=>t.ORDRE).ToList())
                        {
                            CsContenantCritereTarif leContant = RetourneListContenantTableByCtarcompt(items.FK_IDCONTENANTCRITERETARIF);
                            string laValeurColonne = RetourneValueFromClasse<CsEvenement>(_LeEvtProduit, leContant.COLONNEDONNEES.Trim(),leContant.TAILLE );
                            CtarComp = CtarComp.Trim() + laValeurColonne; 
                        }
                        item.CTARCOMP = CtarComp;
                    }
                }
                return LstTarVar;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static string  RetourneValueFromClasse<T>(T _Classe, string _Colonne,int taille) where T : new()
        {
            try
            {
                    string LavaleurColonne = string.Empty;
                    // Recuperation des types
                    PropertyInfo[] properties1 = _Classe.GetType().GetProperties();
                    for (int attrNum = 0; attrNum < properties1.Length; attrNum++)
                    {
                        if (properties1[attrNum].Name.Equals(_Colonne))
                        {
                            string t = string.Empty;
                            object value2 = properties1[attrNum].GetValue(_Classe, null);
                            if (value2.GetType() == typeof(System.Decimal))
                            {
                                string[] decoupe = value2.ToString().Split(',');
                                LavaleurColonne = decoupe[0].PadLeft(taille,'0');
                            }
                            else
                                LavaleurColonne = value2.ToString().PadLeft(taille, '0');
                            break;
                        }
                    }
                    return LavaleurColonne;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        private List<CsTarifFacturation> RetourneAllTarifFacturation()
        {
            try
            {
                   DataTable  dt = FacturationProcedure.ChargerAllTarifFacturation();
                   return  Entities.GetEntityListFromQuery<CsTarifFacturation>(dt);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private List<CsTarifFacturation > RetourneTarifFacturation(CsCentre  _LeCentre)
        {
            try
            {
                galadbEntities context = new galadbEntities();
                List<CsTarifFacturation> lstTarification = new List<CsTarifFacturation>();
                DataTable dt = new DataTable();
                if (_LeCentre.CODENIVEAUTARIF == Enumere.NiveauTarif_Centre)
                {
                    dt = FacturationProcedure.ChargerTarifFacturation(_LeCentre.PK_ID);
                    lstTarification = Entities.GetEntityListFromQuery<CsTarifFacturation>(dt);
                }
                else
                {
                    dt = FacturationProcedure.ChargerTarifFacturation(0);
                    lstTarification = Entities.GetEntityListFromQuery<CsTarifFacturation>(dt);
                }
                return lstTarification;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private List<CsDetailTarifFacturation> RetourneDetailAllTarifFacturation()
        {
            try
            {
                DataTable    dt = FacturationProcedure.ChargerAllDetailTarifFacturation();
                return  Entities.GetEntityListFromQuery<CsDetailTarifFacturation>(dt);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private List<CsDetailTarifFacturation> RetourneDetailTarifFacturation(CsCentre _LeCentre)
        {
            try
            {
                galadbEntities context = new galadbEntities();
                List<CsDetailTarifFacturation> lstTarification = new List<CsDetailTarifFacturation>();
                DataTable dt = new DataTable();
                if (_LeCentre.CODENIVEAUTARIF == Enumere.NiveauTarif_Centre)
                {
                    dt = FacturationProcedure.ChargerDetailTarifFacturation(_LeCentre.PK_ID);
                    lstTarification = Entities.GetEntityListFromQuery<CsDetailTarifFacturation>(dt);
                }
                else
                {
                    dt = FacturationProcedure.ChargerDetailTarifFacturation(0);
                    lstTarification = Entities.GetEntityListFromQuery<CsDetailTarifFacturation>(dt);
                }
                return lstTarification;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private List<CsCasind> RetourneListeDesCas()
        {
            try
            {
                return new DBAccueil().RetourneListeDesCas();
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        private List<CsTarifFacturation> RetourneTarifFacturationClient(List<CsVariableDeTarification> _LstVariableTarif, List<CsTarifFacturation> _LstTarif, CsEvenement _LeEvementAFacture)
        {
            try
            {

                List<CsTarifFacturation> _LaRedevence = new List<CsTarifFacturation>();
                foreach (CsVariableDeTarification item in _LstVariableTarif)
                {
                    List<CsTarifFacturation> _ListR = new List<CsTarifFacturation>();
                    CsTarifFacturation _laRed = new CsTarifFacturation();
                    if (item.MODEAPPLICATION == Enumere.ModeApplicationTarifDate)
                    {
                        _ListR = _LstTarif.Where(p => p.CTARCOMP == item.CTARCOMP
                            && p.REDEVANCE == item.REDEVANCE 
                            && p.RECHERCHETARIF == item.RECHERCHETARIF && p.PRODUIT == _LeEvementAFacture.PRODUIT
                            &&  p.DEBUTAPPLICATION  <= _LeEvementAFacture.DATEEVT).ToList();
                        if (_ListR != null && _ListR.Count != 0)
                        {
                            _laRed = _ListR.FirstOrDefault(p => p.DEBUTAPPLICATION == _ListR.Max(c => c.DEBUTAPPLICATION) && p.DEBUTAPPLICATION <= System.DateTime.Now.Date);
                            if (_laRed != null)
                            {
                                _laRed.FORMULE = item.FORMULE ;
                                _laRed.MODECALCUL = item.MODECALCUL;
                                _laRed.FK_IDREDEVANCE = item.FK_IDREDEVANCE;
                                _LaRedevence.Add(_laRed);
                            }
                        }
                    }
                    else if (item.MODEAPPLICATION == Enumere.ModeApplicationTarifPeriode)
                    {
                        _ListR = _LstTarif.Where(p => p.CTARCOMP == item.CTARCOMP
                                                    && p.REDEVANCE == item.REDEVANCE
                                                    && p.RECHERCHETARIF == item.RECHERCHETARIF
                                                    && p.PRODUIT == _LeEvementAFacture.PRODUIT
                                                    && int.Parse(p.PERDEB) <= int.Parse(_LeEvementAFacture.PERIODE)
                                                    && int.Parse(p.PERFIN) >= int.Parse(_LeEvementAFacture.PERIODE)
                                                    ).ToList();
                        if (_ListR != null && _ListR.Count != 0)
                        {
                            _laRed = _ListR.FirstOrDefault(p => int.Parse(p.PERFIN) == _ListR.Max(c => int.Parse(c.PERFIN)));
                            if (_laRed != null)
                            {
                                //_laRed.ORDRERED = item.ORDREDEVANCE;
                                _laRed.FORMULE = item.FORMULE;
                                _laRed.MODECALCUL = item.MODECALCUL;
                                _laRed.FK_IDREDEVANCE = item.FK_IDREDEVANCE;
                                _LaRedevence.Add(_laRed);
                            }
                        }
                    }
                }
                return _LaRedevence;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        private string RetourneParametrageCas(CsEvenement _LeEvt)
        {
            string _ModeFacturation = "0";
            try
            {
                if (_LeEvt.CAS == Enumere.CasPoseCompteur)
                    return _ModeFacturation;
                else if (_LeEvt.CAS == Enumere.CasDeposeCompteur)
                    return _ModeFacturation = Enumere.FacturationNormale;

                CsCasind _LeCasCourant = ListCas.FirstOrDefault(p => p.CODE == _LeEvt.CAS);
                if (_LeCasCourant != null)
                {
                    _ModeFacturation = _LeCasCourant.FK_IDTYPEFACTURATIONSANSENQUETE .ToString();
                    if (_LeEvt.ENQUETE == "E")
                        return _ModeFacturation = RetourneParametrageEnquete(_LeCasCourant);
                }
                return _ModeFacturation;
            }
            catch (Exception ex)
            {
                return _ModeFacturation;
                throw;
            }
        }


        private CsEvenement RetourneDernierEvtFact(CsEvenement _leEvt)
        {
            CsEvenement _LeDernierEvt = new CsEvenement();
            DataTable dt = FacturationProcedure.RetourneLesDernierEvtFacture(_leEvt.FK_IDCENTRE, _leEvt.CENTRE, _leEvt.CLIENT, _leEvt.ORDRE, _leEvt.PRODUIT, _leEvt.POINT);
            List<CsEvenement> LstTotEvenement = Entities.GetEntityListFromQuery<CsEvenement>(dt);
            if (LstTotEvenement != null && LstTotEvenement.Count != 0)
                _LeDernierEvt = LstTotEvenement.FirstOrDefault(p => p.DATEEVT == LstTotEvenement.Max(c => c.DATEEVT));
            else
            {
                DataTable dts = FacturationProcedure.RetourneEvenementDePose(_leEvt.FK_IDCENTRE, _leEvt.CENTRE, _leEvt.CLIENT, _leEvt.ORDRE, _leEvt.PRODUIT, _leEvt.POINT);
                CsEvenement LeDernEvenement = Entities.GetEntityFromQuery<CsEvenement>(dts);
                _LeDernierEvt = LeDernEvenement;
            }
            return _LeDernierEvt;
        }

        public  CsEvenement RetourneDernierEvtFact(int FK_IDCENTRE, string CENTRE, string CLIENT, string ORDRE, string PRODUIT, int? POINT)
        {
            CsEvenement _LeDernierEvt = new CsEvenement();
            DataTable dt = FacturationProcedure.RetourneLesDernierEvtFacture(FK_IDCENTRE,CENTRE,CLIENT,ORDRE, PRODUIT, POINT);
            List<CsEvenement> LstTotEvenement = Entities.GetEntityListFromQuery<CsEvenement>(dt);
            if (LstTotEvenement != null && LstTotEvenement.Count != 0)
                _LeDernierEvt = LstTotEvenement.FirstOrDefault(p => p.DATEEVT == LstTotEvenement.Max(c => c.DATEEVT));
            else
            {
                DataTable dts = FacturationProcedure.RetourneEvenementDePose(FK_IDCENTRE, CENTRE, CLIENT, ORDRE, PRODUIT, POINT);
                CsEvenement LeDernEvenement = Entities.GetEntityFromQuery<CsEvenement>(dts);
                _LeDernierEvt = LeDernEvenement;
            }
            return _LeDernierEvt;
        }
        private string RetourneParametrageEnquete(CsCasind _LeCasCourant)
        {

            try
            {
                string _ModeFacturation = string.Empty;
                if (_LeCasCourant.FK_IDTYPEFACTURATIONAPRESENQUETE.ToString() == Enumere.FacturationBloqueAvecRegul)
                    return _ModeFacturation = Enumere.FacturationBloqueAvecRegul;

                else if (_LeCasCourant.FK_IDTYPEFACTURATIONAPRESENQUETE.ToString() == Enumere.FacturationBloqueSansRegul)
                    return _ModeFacturation = Enumere.FacturationBloqueSansRegul;

                else if (_LeCasCourant.FK_IDTYPEFACTURATIONAPRESENQUETE.ToString() == Enumere.FacturationEstimerAvecRegul)
                    return _ModeFacturation = Enumere.FacturationEstimerAvecRegul;

                else if (_LeCasCourant.FK_IDTYPEFACTURATIONAPRESENQUETE.ToString() == Enumere.FacturationEstimerSanRegul)
                    return _ModeFacturation = Enumere.FacturationEstimerSanRegul;

                else if (_LeCasCourant.FK_IDTYPEFACTURATIONAPRESENQUETE.ToString() == Enumere.FacturationForfaitAvecRegul)
                    return _ModeFacturation = Enumere.FacturationForfaitAvecRegul;

                else if (_LeCasCourant.FK_IDTYPEFACTURATIONAPRESENQUETE.ToString() == Enumere.FacturationForfaitSansRegul)
                    return _ModeFacturation = Enumere.FacturationForfaitSansRegul;

                else if (_LeCasCourant.FK_IDTYPEFACTURATIONAPRESENQUETE.ToString() == Enumere.FacturationNormale)
                    return _ModeFacturation = Enumere.FacturationNormale;

                else if (_LeCasCourant.FK_IDTYPEFACTURATIONAPRESENQUETE.ToString() == Enumere.FacturationTarifAnnuelUniquement)
                    return _ModeFacturation = Enumere.FacturationTarifAnnuelUniquement;

                else if (_LeCasCourant.FK_IDTYPEFACTURATIONAPRESENQUETE.ToString() == Enumere.FacturationTarifUnitaireUniquement)
                    return _ModeFacturation = Enumere.FacturationTarifUnitaireUniquement;
                else return RetourneParametrageFacture(_LeCasCourant);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        private string RetourneParametrageFacture(CsCasind _LeCasCourant)
        {

            string _ModeFacturation = string.Empty;
            if (_LeCasCourant.FK_IDTYPEFACTURATIONSANSENQUETE.ToString() == Enumere.FacturationBloqueAvecRegul)
                return _ModeFacturation = Enumere.FacturationBloqueAvecRegul;

            else if (_LeCasCourant.FK_IDTYPEFACTURATIONSANSENQUETE.ToString() == Enumere.FacturationBloqueSansRegul)
                return _ModeFacturation = Enumere.FacturationBloqueSansRegul;

            else if (_LeCasCourant.FK_IDTYPEFACTURATIONSANSENQUETE.ToString() == Enumere.FacturationEstimerAvecRegul)
                return _ModeFacturation = Enumere.FacturationEstimerAvecRegul;

            else if (_LeCasCourant.FK_IDTYPEFACTURATIONSANSENQUETE.ToString() == Enumere.FacturationEstimerSanRegul)
                return _ModeFacturation = Enumere.FacturationEstimerSanRegul;

            else if (_LeCasCourant.FK_IDTYPEFACTURATIONSANSENQUETE.ToString() == Enumere.FacturationForfaitAvecRegul)
                return _ModeFacturation = Enumere.FacturationForfaitAvecRegul;

            else if (_LeCasCourant.FK_IDTYPEFACTURATIONSANSENQUETE.ToString() == Enumere.FacturationForfaitSansRegul)
                return _ModeFacturation = Enumere.FacturationForfaitSansRegul;

            else if (_LeCasCourant.FK_IDTYPEFACTURATIONSANSENQUETE.ToString() == Enumere.FacturationNormale)
                return _ModeFacturation = Enumere.FacturationNormale;

            else if (_LeCasCourant.FK_IDTYPEFACTURATIONSANSENQUETE.ToString() == Enumere.FacturationTarifAnnuelUniquement)
                return _ModeFacturation = Enumere.FacturationTarifAnnuelUniquement;

            else if (_LeCasCourant.FK_IDTYPEFACTURATIONSANSENQUETE.ToString() == Enumere.FacturationTarifUnitaireUniquement)
                return _ModeFacturation = Enumere.FacturationTarifUnitaireUniquement;
            else return null;

        }
        private List<CsProduit > RecupereListProduitDesEvenementAFacture(List<CsEvenement> _LstEvt)
        {
            List<CsProduit> lstProduitEvenement = new List<CsProduit>();
            if (_LstEvt.Count > 0)
            {
                var lstProduitsEvenementDistnct = _LstEvt.Select(t => new { t.PRODUIT , t.FK_IDPRODUIT  }).Distinct().ToList();
                foreach (var item in lstProduitsEvenementDistnct)
                    lstProduitEvenement.Add(new CsProduit  {CODE  = item.PRODUIT , PK_ID = item.FK_IDPRODUIT  });
            }
            return lstProduitEvenement;
        }



        public static List<CsCentre> RecupereListCentreDesEvenementAFacture(List<CsEvenement> _LstDesEvenementAFacture)
        {
            try
            {
                List<CsCentre> lstCEntreEvenement = new List<CsCentre>();
                if (_LstDesEvenementAFacture.Count > 0)
                {
                    var lstCentreEvenementDistnct = _LstDesEvenementAFacture.Select(t => new { t.CENTRE, t.FK_IDCENTRE}).Distinct().ToList();
                    foreach (var item in lstCentreEvenementDistnct)
                        lstCEntreEvenement.Add(new CsCentre { CODE = item.CENTRE, PK_ID = item.FK_IDCENTRE });
                }
                return lstCEntreEvenement;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private List<string> RecupereListSiteDesEvenementAFacture(List<CsEvenement> _LstEvt)
        {
            List<string> _LstSite = new List<string>();
            List<IGrouping<string, CsEvenement>> _LstSiteEvt = _LstEvt.GroupBy(p => p.CENTRE).ToList();
            foreach (IGrouping<string, CsEvenement> items in _LstSiteEvt)
            {
                _LstSite.Add(items.Key);
            }
            return _LstSite;
            //List<CsEvenement> _LstSiteEvt = _LstEvt.GroupBy(p => p.CENTRE).Select(c => c.First()).ToList(); //retourne la premiere occurence du groupe
        }
        private List<string> RecupereListProduitDesRedevenceFacture(List<REDFAC> _LstRedfact)
        {
            List<string> _LstProduit = new List<string>();
            List<IGrouping<string, REDFAC>> _LstProdRed = _LstRedfact.GroupBy(p => p.PRODUIT).ToList();
            foreach (IGrouping<string, REDFAC> items in _LstProdRed)
            {
                _LstProduit.Add(items.Key);
            }
            return _LstProduit;
            //List<CsEvenement> _LstSiteEvt = _LstEvt.GroupBy(p => p.CENTRE).Select(c => c.First()).ToList(); //retourne la premiere occurence du groupe
        }
        private int? RetourneNbreDePeriode2(string periodeFin, string periodeDeb, string periodicite)
        {
            try
            {
                int _anneedeb, _anneefin, _moisdeb, _moisfin;
                int _NbrPeriode;
                _anneedeb = int.Parse(periodeDeb.Substring(0, 4));
                _anneefin = int.Parse(periodeFin.Substring(0, 4));
                _moisdeb = int.Parse(periodeDeb.Substring(4, 2));
                _moisfin = int.Parse(periodeFin.Substring(4, 2));

                if ((_anneedeb == _anneefin) && (_moisdeb == _moisfin))
                    _NbrPeriode = (int)(1 / int.Parse(periodicite));

                _NbrPeriode = (_anneefin - _anneedeb) * 12 + _moisfin - _moisdeb;
                _NbrPeriode = (int)(_NbrPeriode / int.Parse(periodicite));
                if (_NbrPeriode == 0) _NbrPeriode = 1;
                return _NbrPeriode;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private double  RetourneNbreDePeriode(string periodeFin, string periodeDeb, string periodicite)
        {
            try
            {
                int _anneedeb, _anneefin, _moisdeb, _moisfin;
                double   _NbrPeriode;
                _anneedeb = int.Parse(periodeDeb.Substring(0, 4));
                _anneefin = int.Parse(periodeFin.Substring(0, 4));
                _moisdeb = int.Parse(periodeDeb.Substring(4, 2));
                _moisfin = int.Parse(periodeFin.Substring(4, 2));

                if ((_anneedeb == _anneefin) && (_moisdeb == _moisfin))
                    _NbrPeriode = (int)(1 / int.Parse(periodicite));

                _NbrPeriode = (_anneefin - _anneedeb) * 12 + _moisfin - _moisdeb;
                _NbrPeriode = (int)(_NbrPeriode / int.Parse(periodicite));
                if (_NbrPeriode == 0) _NbrPeriode = 1;
                return _NbrPeriode;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        private double  RetourneNbreDeJour(DateTime DateFin, DateTime DateDeb)
        {
            try
            {
                TimeSpan t = DateFin - DateDeb;
                double   NrOfDays =(double )t.TotalDays;
                return NrOfDays;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        int NombreJoursPeriode(DateTime laDateDeLaPeriode)
        {
            return System.DateTime.DaysInMonth(laDateDeLaPeriode.Year, laDateDeLaPeriode.Month);
        }

        private double  RetourneValeurParModeCalcul(CsTarifFacturation leTarif, CsEvenement leEvenement)
        {

            double Jour = RetourneNbreDeJour(Convert.ToDateTime(leEvenement.DATEEVT), Convert.ToDateTime(leEvenement.DATERELEVEPRECEDENTEFACTURE));

            double  ValeurCoeficent = 0;
            string ModeCalcule = leTarif.MODECALCUL;
            switch (ModeCalcule)
            {
                case "16": // ss prorata , 30 jours
                    {
                        ValeurCoeficent = Jour / 360;
                        return ValeurCoeficent;
                    };
                case "19":  // ss prorata , 30 jours // non applicable si proprio
                    {
                        if (leEvenement.PROPRIO == Enumere.PROPRIETRAIRE)
                            ValeurCoeficent = 0;

                        double NbPeriode = RetourneNbreDePeriode(leEvenement.DERPERF, leEvenement.DERPERFPREC, leEvenement.PERFAC);
                        double Jour30 = NbPeriode * int.Parse(leEvenement.PERFAC) * 30;

                        ValeurCoeficent = Jour30 / 360;
                        return  ValeurCoeficent;
                    }
                case "13": // ss prorata , 30 jours
                    {
                        ValeurCoeficent =(Jour / 360);
                        return ValeurCoeficent;
                    };
                default:
                    return ValeurCoeficent;
            }
        }
        private int? GestionDesForfaitsPersonalise(string periodeFin, string periodeDeb, string periodicite)
        {
            try
            {
                int _anneedeb, _anneefin, _moisdeb, _moisfin;
                int _NbrPeriode;
                _anneedeb = int.Parse(periodeDeb.Substring(0, 4));
                _anneefin = int.Parse(periodeFin.Substring(0, 4));
                _moisdeb = int.Parse(periodeDeb.Substring(4, 2));
                _moisfin = int.Parse(periodeFin.Substring(4, 2));

                if ((_anneedeb == _anneefin) && (_moisdeb == _moisfin))
                    _NbrPeriode = (int)(1 / int.Parse(periodicite));

                _NbrPeriode = (_anneefin - _anneedeb) * 12 + _moisfin - _moisdeb;
                _NbrPeriode = (int)(_NbrPeriode / int.Parse(periodicite));
                return _NbrPeriode;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    

        private bool UpdateLotri(SqlCommand cmd, CsLotri _leLot)
        {

            int rowsAffected = -1;
            cmd.Parameters.Clear();
            cmd.CommandTimeout = 240;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SPX_FAC_UPDATE_LOTRI";
            //cmd.Parameters.Add("@LOTRI ", SqlDbType.VarChar).Value = (string.IsNullOrEmpty(_leLot.LOTRI )) ? null : _leLot.LOTRI ;
            cmd.Parameters.Add("@JET", SqlDbType.VarChar).Value = (string.IsNullOrEmpty(_leLot.JET)) ? null : _leLot.JET;
            //cmd.Parameters.Add("@PERIODE", SqlDbType.VarChar).Value = (string.IsNullOrEmpty(_leLot.PERIODE )) ? null : _leLot.PERIODE ;
            //cmd.Parameters.Add("@CENTRE", SqlDbType.VarChar).Value = (string.IsNullOrEmpty(_leLot.CENTRE )) ? null : _leLot.CENTRE ;
            //cmd.Parameters.Add("@PRODUIT", SqlDbType.VarChar).Value = (string.IsNullOrEmpty(_leLot.PRODUIT )) ? null : _leLot.PRODUIT ;
            //cmd.Parameters.Add("@CATCLI", SqlDbType.VarChar).Value = (string.IsNullOrEmpty(_leLot.CATCLI )) ? null : _leLot.CATCLI ;
            cmd.Parameters.Add("@PERIODICITE", SqlDbType.VarChar).Value = (string.IsNullOrEmpty(_leLot.PERIODICITE)) ? null : _leLot.PERIODICITE;
            cmd.Parameters.Add("@EXIG", SqlDbType.Int).Value = _leLot.EXIG;
            //cmd.Parameters.Add("@DFAC", SqlDbType.DateTime ).Value = DateTime.Parse(_leLot.DFAC) ;
            cmd.Parameters.Add("@ETATFAC1", SqlDbType.VarChar).Value = (string.IsNullOrEmpty(_leLot.ETATFAC1)) ? null : _leLot.ETATFAC1;
            cmd.Parameters.Add("@ETATFAC2", SqlDbType.VarChar).Value = (string.IsNullOrEmpty(_leLot.ETATFAC2)) ? null : _leLot.ETATFAC2;
            cmd.Parameters.Add("@ETATFAC3", SqlDbType.VarChar).Value = (string.IsNullOrEmpty(_leLot.ETATFAC3)) ? null : _leLot.ETATFAC3;
            cmd.Parameters.Add("@ETATFAC4", SqlDbType.VarChar).Value = (string.IsNullOrEmpty(_leLot.ETATFAC4)) ? null : _leLot.ETATFAC4;
            cmd.Parameters.Add("@ETATFAC5", SqlDbType.VarChar).Value = (string.IsNullOrEmpty(_leLot.ETATFAC5)) ? null : _leLot.ETATFAC5;
            cmd.Parameters.Add("@ETATFAC6", SqlDbType.VarChar).Value = (string.IsNullOrEmpty(_leLot.ETATFAC6)) ? null : _leLot.ETATFAC6;
            cmd.Parameters.Add("@ETATFAC7", SqlDbType.VarChar).Value = (string.IsNullOrEmpty(_leLot.ETATFAC7)) ? null : _leLot.ETATFAC7;
            cmd.Parameters.Add("@ETATFAC8", SqlDbType.VarChar).Value = (string.IsNullOrEmpty(_leLot.ETATFAC8)) ? null : _leLot.ETATFAC8;
            cmd.Parameters.Add("@ETATFAC9", SqlDbType.VarChar).Value = (string.IsNullOrEmpty(_leLot.ETATFAC9)) ? null : _leLot.ETATFAC9;
            cmd.Parameters.Add("@ETATFAC10", SqlDbType.VarChar).Value = (string.IsNullOrEmpty(_leLot.ETATFAC10)) ? null : _leLot.ETATFAC10;
            //cmd.Parameters.Add("@TOURNEE", SqlDbType.VarChar).Value = (string.IsNullOrEmpty(_leLot.TOURNEE)) ? null : _leLot.TOURNEE;
            //cmd.Parameters.Add("@RELEVEUR", SqlDbType.VarChar).Value = (string.IsNullOrEmpty(_leLot.FK_RELEVEUR)) ? null : _leLot.FK_RELEVEUR;
            cmd.Parameters.Add("@BASE", SqlDbType.VarChar).Value = (string.IsNullOrEmpty(_leLot.BASE)) ? null : _leLot.BASE;
            DBBase.SetDBNullParametre(cmd.Parameters);
            try
            {
                rowsAffected = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Convert.ToBoolean(rowsAffected);
        }
        private int? RetourneConsomationParModeCalcul(CsEvenement  leEvent, int? ModeCalcul, int Coef, CsTarifFacturation  _LeTarif, string typeFact)
        {
            try
            {
                int? Mini = (_LeTarif.MINIVOL / Coef) * ModeCalcul;
                if (typeFact == Enumere.FacturationNormale)
                {
                    // Nawec
                    //if (consomation == null)
                    //    return Mini;
                    //int? Conso = (consomation / Coef) * ModeCalcul;

                    //if (Conso < Mini)
                    //    return Mini;
                    //else
                    //    return consomation;
                    return leEvent.CONSO;
                }
                //else if (typeFact == Enumere.FacturationForfaitAvecRegul)
                //    return (_LeTarif.FORFVOL / Coef) * ModeCalcul;
                //else if (typeFact == Enumere.FacturationForfaitAvecRegul || typeFact == Enumere.FacturationForfaitSansRegul)
                //    return (_LeTarif.FORFVOL / Coef) * ModeCalcul;
                //else
                if (typeFact == Enumere.FacturationForfaitAvecRegul   || typeFact == Enumere.FacturationForfaitSansRegul )
                {
                    //int ? Consofac =new DBIndex().CalculeConsomationMoyenne(leEvent.FK_IDCENTRE, leEvent.CENTRE, leEvent.CLIENT, leEvent.ORDRE, leEvent.PRODUIT, leEvent.POINT);
                    int? Consofac = (leEvent.CONSOMOYENNEPRECEDENTEFACTURE / Enumere.NombreDejour) * ModeCalcul;
                    
                    if (typeFact == Enumere.FacturationEstimerAvecRegul)
                        leEvent.QTEAREG = (leEvent.QTEAREG != null ? leEvent.QTEAREG : 0) + Consofac;
                    return Consofac;
                }
                else return leEvent.CONSO;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        private int? RetourneConsomationParModeCalculMT(CsQtFacMt _LaConso, CsTarifFacturation _LeTarif, string typeFact, List<CsRedevance> lstRedevance, List<CsRedevanceFacture> lstRedfac)
        {
            try
            {
                if (typeFact == Enumere.FacturationNormale)
                {
                    if (_LeTarif.UNITE == Enumere.CODEUNITE)
                    {
                        if (string.IsNullOrEmpty(_LeTarif.FORMULE)) return 1;
                        return ((int)RetoureQuantiteRedevanceFixe(int.Parse(_LeTarif.FORMULE), _LaConso.dPuissanceSouscrite)); 
                    }
                    int formule =int.Parse( _LeTarif.FORMULE);
                    switch (formule)
                    {
                        case 10314: // Redevance d'énergie active (pour le compteur actif à un seul index)
                                return (int)_LaConso.dConsoActiveAfterPertes;
                        case 10324: // Redevance d'énergie active heures de pointes
                                return (int)_LaConso.dConsoActiveHPointesAfterPertes;
                        case 10334: // Redevance d'énergie active heures pleines
                                return (int)_LaConso.dConsoActiveHPleinesAfterPertes;
                        case 10344: // Redevance d'énergie active heures creuses
                                return (int)_LaConso.dConsoActiveHCreusesAfterPertes;
                        case 10454: // Pénalité de dépassement
                            {
                                CsParametresGeneraux ParametreHeuresPenalite = new DB_ParametresGeneraux().SelectAllParametresGeneraux().FirstOrDefault(t=>t.CODE =="000153");
                                int iNbHeuresPenalite = (ParametreHeuresPenalite == null ? 0 : int.Parse(ParametreHeuresPenalite.LIBELLE));
                                if (iNbHeuresPenalite == 0)
                                {
                                    string message = "La redevance de formule 10454 n'a pas de nombre d'heures de pénalité. Renseigner Ta58 code 153 ";
                                    //Galatee.Tools.Utility.EcrireFichier(message, "");
                                }

                                // Si la conso du maximètre est supérieure à la puissance souscrtie, le client est pénalisé
                                if (_LaConso.dConsoMaximetre > _LaConso.dPuissanceSouscrite)
                                    return (int)(((_LaConso.dConsoMaximetre * _LaConso.dKimp) - _LaConso.dPuissanceSouscrite) * iNbHeuresPenalite);
                                else
                                    return 0;

                            }
                        case 10674: // Majoration / Minoration
                            return RetourneRedevenceParametre(lstRedevance,_LeTarif, lstRedfac);

                        case 10194: // Redevance de régulation
                                return int.Parse(_LaConso.dConsoTotActiveAvecPertes.ToString());
                        default:
                                return null;
                           
                    }
                }
                return null;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        private CsRedevanceFacture CalculeFactureParRedevanceFixe(CsTarifFacturation _LeTarifRedevence, 
                                                CsEvenement _LeEvtAFacture, List<CsCtax> _lstTax,
                                                string typeFact, string _NumFacture, string jet)
        {
            try
            {
                List<CsRedevanceFacture> _LesRedfact = new List<CsRedevanceFacture>();
                decimal? _TauxTva = _lstTax.FirstOrDefault(p => p.CODE == _LeTarifRedevence.TAXE).TAUX;
                int? _ValeurModeFacturation = 0;
                int _Coeficient = 1;
                double result = 0;
                int? _nbrJourDeConsomation = (int?)RetourneNbreDeJour(Convert.ToDateTime(_LeEvtAFacture.DATEEVT), Convert.ToDateTime(_LeEvtAFacture.DATERELEVEPRECEDENTEFACTURE));
                if (_LeTarifRedevence.MODECALCUL == Enumere.NombreJour360)
                {
                    _ValeurModeFacturation = _nbrJourDeConsomation;
                    _Coeficient = 365;
                }
                else if (_LeTarifRedevence.MODECALCUL == Enumere.NombrePeriode)
                {
                    _ValeurModeFacturation = RetourneNbreDePeriode2(_LeEvtAFacture.PERIODE, _LeEvtAFacture.PERIODEPRECEDENTEFACTURE, _LeEvtAFacture.PERFAC);
                    _Coeficient = 12;
                }
                else if (_LeTarifRedevence.MODECALCUL == Enumere.NombreJourPeriode30)
                {
                    _ValeurModeFacturation = _nbrJourDeConsomation;
                    _Coeficient = 12;

                    double NbPeriode = RetourneNbreDePeriode(_LeEvtAFacture.PERIODE, _LeEvtAFacture.PERIODEPRECEDENTEFACTURE, _LeEvtAFacture.PERFAC);
                    double Jour30 = NbPeriode * int.Parse(_LeEvtAFacture.PERFAC) * 30;
                    result = NbPeriode;
                }


                double barprix = 0;
                if (result != 0)
                    barprix = ((double)_LeTarifRedevence.MONTANTANNUEL / _Coeficient) * result;
                else
                    barprix = ((double)_LeTarifRedevence.MONTANTANNUEL / _Coeficient) * (double)_ValeurModeFacturation;

                CsRedevanceFacture _LeRedfact = new CsRedevanceFacture();
                _LeRedfact.CENTRE = _LeEvtAFacture.CENTRE;
                _LeRedfact.CLIENT = _LeEvtAFacture.CLIENT;
                _LeRedfact.ORDRE = _LeEvtAFacture.ORDRE;
                _LeRedfact.PERIODE = _LeEvtAFacture.PERIODE;
                _LeRedfact.PRODUIT = _LeEvtAFacture.PRODUIT;
                _LeRedfact.FACTURE = _NumFacture;
                _LeRedfact.LIENFAC = _NumFacture;
                _LeRedfact.BARPRIX = (decimal?)barprix;
                _LeRedfact.DAPP = System.Convert.ToDateTime(_LeTarifRedevence.DEBUTAPPLICATION);
                _LeRedfact.REDEVANCE = _LeTarifRedevence.REDEVANCE;
                _LeRedfact.LOTRI = _LeEvtAFacture.LOTRI;
                _LeRedfact.JET = jet;
                _LeRedfact.QUANTITE = 1;
                _LeRedfact.CTAX = _LeTarifRedevence.TAXE;
                _LeRedfact.TAXE = _TauxTva;
                _LeRedfact.UNITE = _LeTarifRedevence.UNITE;
                _LeRedfact.TRANCHE = "0";
                _LeRedfact.TOTREDHT = (decimal?)Math.Ceiling((double)barprix);
                _LeRedfact.TOTREDTAX = (decimal?)Math.Ceiling(((double)_TauxTva * (double)_LeRedfact.TOTREDHT));
                _LeRedfact.TOTREDTTC = _LeRedfact.TOTREDHT + _LeRedfact.TOTREDTAX;
                _LeRedfact.TOPMAJ = "1";
                _LeRedfact.PARAM6 = _LeTarifRedevence.CTARCOMP;
                _LeRedfact.NBJOUR = _nbrJourDeConsomation;
                _LeRedfact.FORMULE = _LeTarifRedevence.FORMULE;
                _LeRedfact.USERCREATION = _LeEvtAFacture.USERMODIFICATION;
                _LeRedfact.USERMODIFICATION = _LeEvtAFacture.USERMODIFICATION;
                _LeRedfact.DATECREATION = DateTime.Now.Date;
                _LeRedfact.FK_IDCENTRE = _LeEvtAFacture.FK_IDCENTRE;
                _LeRedfact.FK_IDPRODUIT = _LeEvtAFacture.FK_IDPRODUIT;
                _LeRedfact.FK_IDABON = _LeEvtAFacture.FK_IDABON.Value  ;
             return _LeRedfact;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        private decimal RetoureQuantiteRedevanceFixe(int formule,decimal PuissanceSouscrite)
        {
         int tformule = formule;
         switch (tformule)
         {
             case 10164: // Prime fixe mensuelle
                 return (decimal )PuissanceSouscrite;
             default:
                 break;
         }
         return (decimal)1;
        }
        private CsQtFacMt ConsoParTypeCompteurMt(List<CsEvenement> _LstEvenement,List<CsTypeComptage> lstTypeCompage)
        {
            string clientPb = string.Empty;
            try
            {
                string _typeCalcul = RetourneParametrageCas(_LstEvenement.First());
                CsQtFacMt ConsoMt = new CsQtFacMt();
                CsTypeComptage leTypeComptage = lstTypeCompage.FirstOrDefault(t => t.CODE == _LstEvenement.FirstOrDefault().TYPECOMPTAGE);
                if (leTypeComptage == null) return null;

                foreach (var _LeEvenement in _LstEvenement)
                {
                    if (int.Parse( _LeEvenement.TYPECOMPTAGE) < int.Parse( Enumere.TYPECOMPTAGE_1.ToString()))
                        return null  ;
                    else
                    {
                        if (_typeCalcul == Enumere.FacturationForfaitAvecRegul || _typeCalcul == Enumere.FacturationForfaitAvecRegul)
                        {
                            int? _ValeurModeFacturation = RetourneNbreDePeriode2(_LeEvenement.PERIODE, _LeEvenement.PERIODEPRECEDENTEFACTURE, _LeEvenement.PERFAC);
                            _LeEvenement.CONSO = (_LeEvenement.CONSOMOYENNEPRECEDENTEFACTURE / Enumere.NombreDejour) * _ValeurModeFacturation;
                        }

                        if (_LeEvenement.TYPECOMPTEUR  == Enumere.ACTIF)
                        {
                          
                             ConsoMt.dConsoActive  = Convert.ToDecimal(_LeEvenement.CONSO.Value) ;
                            ConsoMt.dKa1 = _LeEvenement.COEFK1 != null ? _LeEvenement.COEFK1.Value : 0;
                            ConsoMt.dKa2 = _LeEvenement.COEFK2 != null ? _LeEvenement.COEFK2.Value : 0;
                            ConsoMt.dCumulRegConsoDeb += _LeEvenement.REGCONSO != null ? _LeEvenement.REGCONSO.Value : 0;
                            continue ;
                        }
                        if (_LeEvenement.TYPECOMPTEUR == Enumere.ACTIF_HPOINTES)
                        {
                            ConsoMt.dConsoActHPointes = Convert.ToDecimal(_LeEvenement.CONSO.Value);
                            ConsoMt.dKa1 =_LeEvenement.COEFK1 != null ? _LeEvenement.COEFK1.Value : 0;
                            ConsoMt.dKa2 = _LeEvenement.COEFK2 != null ? _LeEvenement.COEFK2.Value : 0;
                            ConsoMt.dCumulRegConsoDeb += _LeEvenement.REGCONSO != null ? _LeEvenement.REGCONSO.Value : 0;

                            continue;

                        }
                        if (_LeEvenement.TYPECOMPTEUR == Enumere.ACTIF_HPLEINES)
                        {
                            ConsoMt.dConsoActHPleines = Convert.ToDecimal(_LeEvenement.CONSO.Value);
                            ConsoMt.dKa1 = _LeEvenement.COEFK1 != null ? _LeEvenement.COEFK1.Value : 0;
                            ConsoMt.dKa2 = _LeEvenement.COEFK2 != null ? _LeEvenement.COEFK2.Value : 0;
                            ConsoMt.dCumulRegConsoDeb += _LeEvenement.REGCONSO != null ? _LeEvenement.REGCONSO.Value : 0;

                            continue;

                        }
                        if (_LeEvenement.TYPECOMPTEUR == Enumere.ACTIF_HCREUSES)
                        {
                            ConsoMt.dConsoActHCreuses = Convert.ToDecimal(_LeEvenement.CONSO.Value);
                            ConsoMt.dKa1 = _LeEvenement.COEFK1 != null ? _LeEvenement.COEFK1.Value : 0;
                            ConsoMt.dKa2 = _LeEvenement.COEFK2 != null ? _LeEvenement.COEFK2.Value : 0;
                            ConsoMt.dCumulRegConsoDeb += _LeEvenement.REGCONSO != null ? _LeEvenement.REGCONSO.Value : 0;

                            continue;

                        }
                        if (_LeEvenement.TYPECOMPTEUR == Enumere.REACTIF)
                        {
                            ConsoMt.dConsoReactive = Convert.ToDecimal(_LeEvenement.CONSO.Value);
                            ConsoMt.dKr1 = _LeEvenement.COEFK1 != null ? _LeEvenement.COEFK1.Value : 0;
                            ConsoMt.dKr2 = _LeEvenement.COEFK2 != null ? _LeEvenement.COEFK2.Value : 0;
                            ConsoMt.dCumulRegConsoDeb += _LeEvenement.REGCONSO != null ? _LeEvenement.REGCONSO.Value : 0;

                            continue;

                        }
                        if (_LeEvenement.TYPECOMPTEUR == Enumere.HORAIRE)
                        {
                            ConsoMt.dConsoHoraire = Convert.ToDecimal(_LeEvenement.CONSO.Value);
                            ConsoMt.dCumulRegConsoDeb += _LeEvenement.REGCONSO != null ? _LeEvenement.REGCONSO.Value : 0;

                            continue;

                        }
                        if (_LeEvenement.TYPECOMPTEUR == Enumere.MAXIMETRE)
                        {
                            ConsoMt.dConsoMaximetre = Convert.ToDecimal(_LeEvenement.CONSO.Value);
                            ConsoMt.dKimp = _LeEvenement.COEFLECT.Value;
                           if (ConsoMt.dKimp == 0)
                               ConsoMt.dKimp = 1;
                           ConsoMt.dCumulRegConsoDeb += _LeEvenement.REGCONSO != null ? _LeEvenement.REGCONSO.Value : 0;

                           continue;
                        }
                    }
                }
                // Calcul de la consommation active = somme de la conso des trois compteurs ou du compteur actif à un seul cadran
                ConsoMt.dConsoTotaleActive = (ConsoMt.dConsoActive + ConsoMt.dConsoActHPointes + ConsoMt.dConsoActHPleines + ConsoMt.dConsoActHCreuses);

                // Calcul des pertes actives.
                ConsoMt.dPertesActives = ((ConsoMt.dKa1 * ConsoMt.dConsoTotaleActive) + (ConsoMt.dKa2 * ConsoMt.dConsoHoraire));

                // Calcul des pertes réactives
                ConsoMt.dPertesReactives =((ConsoMt.dKr1 * ConsoMt.dConsoReactive) + (ConsoMt.dKr2 * ConsoMt.dConsoHoraire));

                // Calcul de la consommation totale active y compris les pertes
                ConsoMt.dConsoTotActiveAvecPertes = (ConsoMt.dConsoActive + ConsoMt.dConsoActHPointes + ConsoMt.dConsoActHPleines + ConsoMt.dConsoActHCreuses + ConsoMt.dPertesActives);

                // Calcul de la consommation totale réactive y compris les pertes
                ConsoMt.dConsoTotReactiveAvecPertes = (ConsoMt.dConsoReactive + ConsoMt.dPertesReactives);

                // calcul de Tangente Phi
                // On force tangente phi à 0 si le dénominateur est nul pour éviter un débordement
                if (ConsoMt.dConsoTotActiveAvecPertes == 0)
                    ConsoMt.dTanPhi = 0;
                else
                    ConsoMt.dTanPhi = (ConsoMt.dConsoTotReactiveAvecPertes / ConsoMt.dConsoTotActiveAvecPertes);

                // Calcul du coefficient de minoration / majoration
                if (ConsoMt.dTanPhi > (decimal )0.6)
                    ConsoMt.dCoefMinoMajo = (decimal)((ConsoMt.dTanPhi - (decimal)0.6) / 3);
                else
                    ConsoMt.dCoefMinoMajo = (decimal)((ConsoMt.dTanPhi - (decimal)0.6) / 6);

                // Renseigner les champs de consommation active après intégration des pertes
                if (ConsoMt.dConsoTotaleActive == 0)
                {
                    ConsoMt.dConsoActiveAfterPertes =0;
                    ConsoMt.dConsoActiveHPointesAfterPertes = 0;
                    ConsoMt.dConsoActiveHPleinesAfterPertes = 0;
                }
                else
                {
                    if (leTypeComptage.AVEC_PERTE == true)
                    {
                        ConsoMt.dConsoActiveAfterPertes = (ConsoMt.dConsoActive * (1 + (ConsoMt.dPertesActives / ConsoMt.dConsoTotaleActive)));
                        ConsoMt.dConsoActiveHPointesAfterPertes = (ConsoMt.dConsoActHPointes * (1 + (ConsoMt.dPertesActives / ConsoMt.dConsoTotaleActive)));
                        ConsoMt.dConsoActiveHPleinesAfterPertes = (ConsoMt.dConsoActHPleines * (1 + (ConsoMt.dPertesActives / ConsoMt.dConsoTotaleActive)));
                        ConsoMt.dConsoActiveHCreusesAfterPertes = ConsoMt.dConsoTotActiveAvecPertes - (ConsoMt.dConsoActiveHPointesAfterPertes + ConsoMt.dConsoActiveHPleinesAfterPertes);

                    }
                    else
                    {
                        ConsoMt.dConsoActiveAfterPertes = ConsoMt.dConsoActive ;
                        ConsoMt.dConsoActiveHPointesAfterPertes = ConsoMt.dConsoActHPointes ;
                        ConsoMt.dConsoActiveHPleinesAfterPertes = ConsoMt.dConsoActHPleines;
                        ConsoMt.dConsoActiveHCreusesAfterPertes = ConsoMt.dConsoTotaleActive - (ConsoMt.dConsoActHPointes + ConsoMt.dConsoActHPleines);
                        ConsoMt.dPertesActives = 0;
                        ConsoMt.dPertesReactives = 0;
                    
                    }
                }

                // YKO le 13/09/2004

                // Conso facture pleine
                CsEvenement leEvtPleine = _LstEvenement.FirstOrDefault(t => t.TYPECOMPTEUR == Enumere.ACTIF_HPLEINES);
                if (leEvtPleine != null)
                    leEvtPleine.CONSOFAC = (int)ConsoMt.dConsoActiveHPleinesAfterPertes;

                // Conso facture creuse
                CsEvenement leEvtCreuse = _LstEvenement.FirstOrDefault(t => t.TYPECOMPTEUR == Enumere.ACTIF_HCREUSES );
                if (leEvtCreuse != null)
                    leEvtCreuse.CONSOFAC = (int)ConsoMt.dConsoActiveHCreusesAfterPertes;

                // Conso facture pointe
                CsEvenement leEvtPointe = _LstEvenement.FirstOrDefault(t => t.TYPECOMPTEUR == Enumere.ACTIF_HPOINTES );
                if (leEvtPointe != null)
                    leEvtPointe.CONSOFAC = (int)ConsoMt.dConsoActiveHPointesAfterPertes;

                return ConsoMt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
  
 
        public List<Galatee.Entity.Model.EVENEMENT > RetourneEvenementDejaFacture(CsLotri LeLotSelectione)
        {
            try
            {
                //xxx=centre y=1 resiliation, y=2 avoir & facture isolée, y=3 facture d'annulation
                DataTable dt = new DataTable();
                if (new string[] { Enumere.FactureIsoleIndex, Enumere.FactureIsoleIndex, Enumere.FactureResiliationIndex }.Contains(LeLotSelectione.NUMLOTRI.Substring(Enumere.TailleCentre, (Enumere.TailleNumeroBatch - Enumere.TailleCentre))))
                    dt = FacturationProcedure.RetourneEvenementADefacturerFacturerIsole(LeLotSelectione);
                else
                    dt = FacturationProcedure.RetourneEvenementADefacturerFacturerFromPagerie(LeLotSelectione);
                return Entities.GetEntityListFromQuery<Galatee.Entity.Model.EVENEMENT>(dt);
               
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public List<Galatee.Entity.Model.EVENEMENT> RetourneEvenementPourPurge(CsLotri LeLotSelectione)
        {
            try
            {
                //xxx=centre y=1 resiliation, y=2 avoir & facture isolée, y=3 facture d'annulation
                DataTable dt = new DataTable();
                if (new string[] { Enumere.FactureIsoleIndex, Enumere.FactureIsoleIndex, Enumere.FactureResiliationIndex }.Contains(LeLotSelectione.NUMLOTRI.Substring(Enumere.TailleCentre, (Enumere.TailleNumeroBatch - Enumere.TailleCentre))))
                    dt = FacturationProcedure.RetourneEvenementMiseAJourIsole (LeLotSelectione);
                else
                    dt = FacturationProcedure.RetourneEvenementMiseAJours(LeLotSelectione);
                return Entities.GetEntityListFromQuery<Galatee.Entity.Model.EVENEMENT>(dt);

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        public List<Galatee.Entity.Model.ENTFAC > RetourneEntfacDejaFacture(CsLotri LeLotSelectione,  galadbEntities leContext )
        {
            try
            {
                //xxx=centre y=1 resiliation, y=2 avoir & facture isolée, y=3 facture d'annulation
                List<Galatee.Entity.Model.ENTFAC> dt = new List<Galatee.Entity.Model.ENTFAC>();
                if (new string[] { Enumere.FactureIsoleIndex, Enumere.FactureIsoleIndex, Enumere.FactureResiliationIndex }.Contains(LeLotSelectione.NUMLOTRI.Substring(Enumere.TailleCentre, (Enumere.TailleNumeroBatch - Enumere.TailleCentre))))
                    dt = FacturationProcedure.ListeFactureADefacturerIsole(LeLotSelectione, leContext);
                else
                    dt = FacturationProcedure.ListeFactureADefacturerGeneral(LeLotSelectione, leContext);
                return dt;
            }
            catch (Exception ex)
            {
                leContext.Dispose();
                throw ex;
            }

        }
        public bool IsLotIsole(CsLotri leLot)
        {
            string FactureIsoleIndex = "00002";
            string FactureResiliationIndex = "00001";
            string FactureAnnulatinIndex = "00003";
            string FactureChangementCompteurinIndex = "00004";
            if (new string[] { FactureIsoleIndex, FactureResiliationIndex, FactureAnnulatinIndex, FactureChangementCompteurinIndex }.Contains(leLot.NUMLOTRI.Substring(Enumere.TailleCentre, (Enumere.TailleNumeroBatch - Enumere.TailleCentre))))
                return true;
            else
                return false;
        }
        public bool IsLotIsole(string leLot)
        {
            string FactureIsoleIndex = "00002";
            string FactureResiliationIndex = "00001";
            string FactureAnnulatinIndex = "00003";
            string FactureChangementCompteurinIndex = "00004";

            if (new string[] { FactureIsoleIndex, FactureResiliationIndex, FactureAnnulatinIndex, FactureChangementCompteurinIndex }.Contains(leLot.Substring(Enumere.TailleCentre, (Enumere.TailleNumeroBatch - Enumere.TailleCentre))))
                return true;
            else
                return false;
        }
        public List<CsFactureBrut> CalculeDuLot(List<CsLotri> ListLotSelectione, bool IsSimulation)
        {
            bool IsIsole = false;
            using (galadbEntities ctx = new galadbEntities())
            {
                CsLotri leLot = ListLotSelectione.First();
                if (IsLotIsole(leLot))
                {
                    IsIsole = true;
                    LOTRI l = ctx.LOTRI.FirstOrDefault(t => t.FK_IDCENTRE == leLot.FK_IDCENTRE &&
                                           t.USERCREATION == leLot.USERCREATION &&
                                           t.NUMLOTRI == leLot.NUMLOTRI);
                    if (l != null)
                        l.UserCalcul = leLot.MATRICULE;
                }
                else
                {
                    List<int> lstId = ListLotSelectione.Select(l => l.PK_ID).ToList();
                    List<LOTRI> lstLotri = ctx.LOTRI.Where(u => lstId.Contains(u.PK_ID)).ToList();
                    lstLotri.ForEach(i => i.UserCalcul = leLot.MATRICULE);
                }
                SqlCommand laCommande = DBBase.InitTransaction(ConnectionString);
                DeleteAnnomalieLotSpx(ListLotSelectione, laCommande);
                laCommande.Transaction.Commit();

                //List<string> lstNumeroLot = ListLotSelectione.Select(o => o.NUMLOTRI).ToList();
                //List<ANOMALIEFACTURATION> lstAnomalieFact = ctx.ANOMALIEFACTURATION.Where(t => lstNumeroLot.Contains(t.LOTRI)).ToList();
                //if (lstAnomalieFact != null && lstAnomalieFact.Count != 0)
                //    Entities.DeleteEntity<ANOMALIEFACTURATION>(lstAnomalieFact, ctx);
                ctx.SaveChanges();
            }
            bool IsMT = false;
            if (ListLotSelectione.First().PRODUIT == Enumere.ElectriciteMT)
                IsMT = true;

            DataTable dt = CalculeDuLotSpx(ListLotSelectione.First().NUMLOTRI, ListLotSelectione.First().PERIODE, ListLotSelectione.First().EXIG, IsSimulation, ListLotSelectione.First().MATRICULE, IsMT, IsIsole);
            List<CsFactureBrut> lstFacture = Tools.Utility.GetEntityListFromQuery<CsFactureBrut>(dt);
            lstFacture.ForEach(t => t.IsSimuler = IsSimulation);

            return lstFacture;

        }
        //public List<CsFactureBrut> CalculeDuLot(List<CsLotri> ListLotSelectione, bool IsSimulation)
        //{
        //    try
        //    {
        //        bool IsIsole = false;
        //        SqlCommand laCommande = DBBase.InitTransaction(ConnectionString);
        //        try
        //        {
        //            DBAccueil db = new DBAccueil();
        //            CsLotri leLot = ListLotSelectione.First();
        //            if (IsLotIsole(leLot))
        //            {
        //                IsIsole = true;
        //                leLot.UserCalcul = leLot.MATRICULE;
        //                db.InsertOrUpdateLotri(leLot, laCommande);
        //            }
        //            else
        //            {
        //                ListLotSelectione.ForEach(i => i.UserCalcul = i.MATRICULE);
        //                db.InsertOrUpdateLotri(ListLotSelectione, laCommande);
        //            }
        //            DeleteAnnomalieLotSpx(ListLotSelectione, laCommande);
        //            laCommande.Transaction.Commit();
        //        }
        //        catch (Exception ex)
        //        {
        //            laCommande.Transaction.Rollback();
        //            throw ex;
        //        }
        //        bool IsMT = false;
        //        if (ListLotSelectione.First().PRODUIT == Enumere.ElectriciteMT)
        //            IsMT = true;

        //        DataTable dt = CalculeDuLotSpx(ListLotSelectione.First().NUMLOTRI, ListLotSelectione.First().PERIODE, ListLotSelectione.First().EXIG, IsSimulation, ListLotSelectione.First().MATRICULE, IsMT, IsIsole);
        //        List<CsFactureBrut> lstFacture = Tools.Utility.GetEntityListFromQuery<CsFactureBrut>(dt);
        //        lstFacture.ForEach(t => t.IsSimuler = IsSimulation);

        //        return lstFacture;
        //    }
        //    catch (Exception ex )
        //    {
        //        throw ex ;
        //    }
        //}
        public DataTable  CalculeDuLotSpx(string Lotri,string periode,int? exig, bool IsSimulation,string matricule,bool IsMt,bool IsIsole)
        {
            cn = new SqlConnection(ConnectionString);

            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandTimeout = 3000;
            cmd.CommandType = CommandType.StoredProcedure;
            if (!IsMt)
                cmd.CommandText = "SPX_CALCUL_FACTURATION_BT";
            else
                cmd.CommandText = "SPX_CALCUL_FACTURATION_MT";

            cmd.Parameters.Add("@Lotri", SqlDbType.VarChar, 8).Value = Lotri;
            cmd.Parameters.Add("@Periode", SqlDbType.VarChar, 6).Value = periode;
            cmd.Parameters.Add("@Exig", SqlDbType.Int).Value = exig;
            cmd.Parameters.Add("@Simulation", SqlDbType.Bit).Value = IsSimulation;
            cmd.Parameters.Add("@Matricule", SqlDbType.VarChar, 5).Value = matricule;
            cmd.Parameters.Add("@Isole", SqlDbType.Bit).Value = IsIsole;

            DBBase.SetDBNullParametre(cmd.Parameters);
            try
            {
                if (cn.State == ConnectionState.Closed)
                    cn.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                //if (reader.Read())
                    dt.Load(reader);

                return dt;
                 
            }
            catch (Exception ex)
            {
                throw new Exception(cmd.CommandText + " (" + Lotri + ") : " + ex.Message);
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                    cn.Close(); // Fermeture de la connection 
                cmd.Dispose();
            }
        }

        public List<CsTarifClient> RetourneTarifClient(int idcentre, int idcategorie, int idreglageCompteur, int? idtypecomptage, string propriotaire, int idproduit)
        {
            cn = new SqlConnection(ConnectionString);

            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandTimeout = 18000;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SPX_GET_TARIF";

            cmd.Parameters.Add("@idCentre", SqlDbType.Int ).Value = idcentre;
            cmd.Parameters.Add("@idCategorie", SqlDbType.Int).Value = idcategorie;
            cmd.Parameters.Add("@idReglageCompteur", SqlDbType.Int).Value = idreglageCompteur;
            cmd.Parameters.Add("@idTypeComptage", SqlDbType.Int ).Value = idtypecomptage;
            cmd.Parameters.Add("@proprio", SqlDbType.VarChar, 1).Value = propriotaire;
            cmd.Parameters.Add("@idProduit", SqlDbType.Int ).Value = idproduit;

            DBBase.SetDBNullParametre(cmd.Parameters);
            try
            {
                if (cn.State == ConnectionState.Closed)
                    cn.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);

                return Entities.GetEntityListFromQuery<CsTarifClient>(dt); ;

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
        //private void MiseAJourAction(List<CsEntfac> lesFacturation, SqlCommand  ctx) /** 29/08/2017 **/
        private void MiseAJourAction(List<CsFactureBrut> lesFacturation, SqlCommand  laCommande)
        {
            try
            {
                decimal? montantFacture = 0;
                int NombreClientFact = 0;
                int Quantie = 0;
                string leLot = lesFacturation.First().LOTRI;
                string leJet = lesFacturation.First().JET;
                string laPeriode = lesFacturation.First().PERIODE;
                string leMatricule = lesFacturation.First().MATRICULE ;
                string produit = string.Empty;
                
                    if (lesFacturation != null && lesFacturation.Count != 0)
                    {
                        /** ZEG 29/08/2017 **/
                       /* NombreClientFact = lesFacturation.Count();
                        montantFacture = lesFacturation.Sum(t => t.TOTFTTC);
                        foreach (CsEntfac item in lesFacturation)
                            Quantie = Quantie + item.LstProfac.Sum(y => y.CONSOFAC).Value;*/
                        NombreClientFact = lesFacturation.First().NOMBRECALCULE;
                        montantFacture = lesFacturation.First().MONTANT;
                        Quantie = lesFacturation.First().QUANTITE;
                    }
                   CsAction Act = new CsAction(){
                                    JET =leJet,
                                    LOT = leLot ,
                                    PERIODE = laPeriode,
                                    MATRICULE = leMatricule,
                                    NOMBRE1 =NombreClientFact,
                                    MONTANT1 = montantFacture,
                                    NOMBRE2 = Quantie  };
                   MiseAjourActionSpx(Act, laCommande);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ACTION CreerActionLotri(string  LeLotri,string periode,string jet,string produit, string TypeAction, string matricule, int Nombre, decimal? montant,int Quantite)
        {
            try
            {
                ACTION Action = new ACTION();
                Action.JET = jet;
                Action.PERIODE = periode;
                Action.ACTION1 = TypeAction;
                Action.LOT = LeLotri;
                Action.DATE1 = System.DateTime.Today.Date;
                Action.NOMBRE1 = Nombre;
                Action.NOMBRE2 = Quantite;
                Action.NOMBRE3 = 0;
                Action.MONTANT1 = montant;
                Action.MONTANT2 = 0;
                Action.MONTANT3 = 0;
                Action.MATRICULE = matricule;
                Action.PRODUIT = produit;
                Action.STATUT = "O";

                return Action;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ACTION CreerActionLotri(CsLotri LeLotri, string TypeAction, string matricule, int Nombre, decimal? montant)
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
                Action.STATUT = "O";

                return Action;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool  DemandeDefacturerLot(List<CsLotri> ListLotSelectione)
        {
            try
            {
                string _CheminListing = string.Empty;
                int res = -1;
                List<ENTFAC> _LstFActureDefac = new List<ENTFAC>();

                using(galadbEntities leContext = new galadbEntities())
	            {
		          foreach (CsLotri item in ListLotSelectione)
	                {
                        List< LOTRI> ctx = leContext.LOTRI.Where(t => t.PK_ID == item.PK_ID ).ToList();
                        foreach (LOTRI items in ctx)
                           {
                               items.DATEMODIFICATION = System.DateTime.Now;
                               items.ETATFAC3 = Enumere.DemandeDefacturation;
                               items.ETATFAC4 = null;
                               items.ETATFAC6 = int.Parse(ListLotSelectione.First().JET).ToString() ;  
                           }
	                }
                    res = leContext.SaveChanges();
                    return res == -1 ? false : true;
	            }

            }
            catch (Exception ex)
            {
                throw ex;
                return false;
            }
        }

        public bool RejetDemandeDefacturerLot(List<CsLotri> ListLotSelectione)
        {
            try
            {
 
                List<ENTFAC> _LstFActureDefac = new List<ENTFAC>();
                galadbEntities leContext = new galadbEntities();
                foreach (CsLotri _leLotri in ListLotSelectione)
                {
                    _leLotri.DATEMODIFICATION = System.DateTime.Now;
                    _leLotri.ETATFAC4 = Enumere.DemandeDefacturationRejeter;

                    galadbEntities leContext1 = new galadbEntities();
                    FacturationProcedure.ValiderDemandeDefacturation(_leLotri, leContext);
                }
                leContext.SaveChanges();
                return true;

            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public CsStatFacturation DefacturerLot(List<CsLotri> ListLotSelectione, string Action)
        {
            try
            {
                CsStatFacturation _Stat = new CsStatFacturation();
                string _CheminListing = string.Empty;
                List<ENTFAC> _LstFActureDefac = new List<ENTFAC>();
                decimal? _MontantDefac = 0;
                int _NbreFacture = 0;
                int _NbreRejeter = 0;
                var lesLotDist = ListLotSelectione.Select(y => new { y.FK_IDCENTRE,y.CENTRE , y.USERCREATION, y.NUMLOTRI,y.JET   }).Distinct();
                foreach (var item in lesLotDist)
                {
                    DataTable dt = new DataTable();
                    bool EstIsole = false;
                 
                    if (IsLotIsole(item.NUMLOTRI)) EstIsole = true;
                    dt = DefactureLotSpx(item.CENTRE, item.FK_IDCENTRE, item.NUMLOTRI, item.USERCREATION, item.JET, EstIsole);
                    CsStatFacturation laSt = Entities.GetEntityFromQuery <CsStatFacturation>(dt);
                    _NbreFacture = _NbreFacture + laSt.NombreCalcule;
                    _MontantDefac = _MontantDefac + laSt.Montant ;
                }
                _Stat.Montant = _MontantDefac;
                _Stat.NombreCalcule = _NbreFacture;
                _Stat.NombreRejete = _NbreRejeter;
                return _Stat;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable DefactureLotSpx(string Centre,int Fk_idcentre,string lotRi,string Matricule,string Jet ,bool IsIsole)
        {
            cn = new SqlConnection(ConnectionString);

            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandTimeout = 300000;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@lotri", SqlDbType.VarChar, 8).Value = lotRi;
            cmd.Parameters.Add("@Centre", SqlDbType.VarChar, 3).Value = Centre;
            cmd.Parameters.Add("@Fk_idcentre", SqlDbType.Int).Value = Fk_idcentre;
            cmd.Parameters.Add("@matricule", SqlDbType.VarChar, 6).Value = Matricule;
            cmd.Parameters.Add("@Jet", SqlDbType.VarChar, 2).Value = Jet;
            cmd.Parameters.Add("@IsIsole", SqlDbType.Bit).Value = IsIsole;
            cmd.CommandText = "SPX_FAC_DEFACTURELOT";

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
        public bool MiseAjourLotApresCalculSpx(string matricule, string lotri, string periode, int exigibilite, string Jet, bool EstSimulation, SqlCommand cmd)
        {
            cmd.CommandTimeout = 18000;
            cmd.CommandType = CommandType.StoredProcedure;
            if (cmd.Parameters.Count != 0) cmd.Parameters.Clear();
            cmd.Parameters.Add("@MATRICULE", SqlDbType.VarChar, 6).Value = matricule;
            cmd.Parameters.Add("@LOTRI", SqlDbType.VarChar, 8).Value = lotri;
            cmd.Parameters.Add("@EXIG", SqlDbType.Int).Value = exigibilite;
            cmd.Parameters.Add("@PERIODE", SqlDbType.VarChar, 6).Value = periode;
            cmd.Parameters.Add("@JET", SqlDbType.VarChar, 2).Value = Jet;
            cmd.Parameters.Add("@IsSimulation", SqlDbType.Bit).Value = EstSimulation;
            cmd.CommandText = "SPX_FAC_MISEAJOUR_LOTRI_APRESCALCUL";
            DBBase.SetDBNullParametre(cmd.Parameters);
            try
            {
                if (cmd.Connection.State == ConnectionState.Closed)
                    cmd.Connection.Open();
                int reader = -1;
                reader = cmd.ExecuteNonQuery();
                return reader == -1 ? false : true;
            }
            catch (Exception ex)
            {
                throw new Exception(cmd.CommandText + " (" + lotri + ") : " + ex.Message);
            }
        }

        public bool MiseAjourActionSpx(CsAction leAction, SqlCommand cmd)
        {
            
            cmd.CommandTimeout = 18000;
            cmd.CommandType = CommandType.StoredProcedure;
            if (cmd.Parameters.Count != 0) cmd.Parameters.Clear();
            cmd.Parameters.Add("@LOTRI", SqlDbType.VarChar, 8).Value = leAction.LOT ;
            cmd.Parameters.Add("@MATRICULE", SqlDbType.VarChar, 6).Value = leAction.MATRICULE ;
            cmd.Parameters.Add("@PERIODE", SqlDbType.VarChar ,6).Value = leAction.PERIODE  ;
            cmd.Parameters.Add("@MONTANT", SqlDbType.Decimal ).Value = leAction.MONTANT1 ;
            cmd.Parameters.Add("@NOMBRE", SqlDbType.Int ).Value = leAction.NOMBRE1 ;
            cmd.Parameters.Add("@NOMBRE1", SqlDbType.Int).Value = leAction.NOMBRE2;
            cmd.Parameters.Add("@JET", SqlDbType.VarChar ,2).Value = leAction.JET ;
            cmd.CommandText = "SPX_FAC_MISEAJOURACTION";
            DBBase.SetDBNullParametre(cmd.Parameters);
            try
            {
                if (cmd.Connection.State == ConnectionState.Closed)
                    cmd.Connection.Open();
                int reader = -1;
                reader = cmd.ExecuteNonQuery();
                return reader == -1 ? false : true;
            }
            catch (Exception ex)
            {
                throw new Exception(cmd.CommandText + " (" + leAction.LOT + ") : " + ex.Message);
            }
        }

        public bool  PurgeLotSpx(string Centre, int Fk_idcentre, string lotRi, string Matricule, bool IsIsole)
        {
            cn = new SqlConnection(ConnectionString);

            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandTimeout = 18000;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@lotri", SqlDbType.VarChar, 8).Value = lotRi;
            cmd.Parameters.Add("@Centre", SqlDbType.VarChar, 3).Value = Centre;
            cmd.Parameters.Add("@Fk_idcentre", SqlDbType.Int).Value = Fk_idcentre;
            cmd.Parameters.Add("@matricule", SqlDbType.VarChar, 6).Value = Matricule;
            cmd.Parameters.Add("@IsIsole", SqlDbType.Bit).Value = IsIsole ? true : false ;
            cmd.CommandText = "SPX_FAC_PURGE";
            DBBase.SetDBNullParametre(cmd.Parameters);
            try
            {
                if (cn.State == ConnectionState.Closed)
                    cn.Open();
                int reader = -1;
                 reader = cmd.ExecuteNonQuery ();
                 return reader == -1 ?false :true ;
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
        public bool PurgeDeLot(List<CsLotri> ListLotSelectione)
        {
            try
            {
                string _CheminListing = string.Empty;
                var lesLotDist = ListLotSelectione.Select(y => new { y.FK_IDCENTRE, y.CENTRE, y.USERCREATION, y.NUMLOTRI }).Distinct();
                foreach (var item in lesLotDist)
                {
                    DataTable dt = new DataTable();
                    bool EstIsole = false;
                    if (IsLotIsole(item.NUMLOTRI)) EstIsole = true;
                    PurgeLotSpx (item.CENTRE, item.FK_IDCENTRE, item.NUMLOTRI, item.USERCREATION, EstIsole);
                }
                return true ;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        private List<CsEvenement> RecupererEvtAfacturer(List<CsPageri> _LstPagerie)
        {
            try
            {
                List<CsEvenement> LstEvenementFacturableSurLePoint = new List<CsEvenement>();
                ParallelOptions parallelOptions = new ParallelOptions();
                Parallel.ForEach(_LstPagerie, p =>
                {
                    LstEvenementFacturableSurLePoint.AddRange(RecherCheEvenementAFacture(p));
                });
                return LstEvenementFacturableSurLePoint;
            }
            catch (Exception ex)
            { 
                throw ex;
            }
        }
        private List<CsEvenement> RecupererEvtAfacturerUnique(CsPageri _LaPagerie)
        {
            List<CsEvenement> LstEvenementFacturableSurLePoint = RecherCheEvenementAFacture(_LaPagerie);
            return LstEvenementFacturableSurLePoint;
        }

        public static bool InsertAnnomalie(List<CsAnnomalie > _LesAnnomalie, galadbEntities pContext)
        {
            try
            {
                List<ANOMALIEFACTURATION> lesAnnomalie = Entities.ConvertObject<ANOMALIEFACTURATION, CsAnnomalie>(_LesAnnomalie);
                return Entities.InsertEntity<ANOMALIEFACTURATION>(lesAnnomalie, pContext);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
      public  bool InsertEntfact(List<CsEntfac>  _LaFacture, galadbEntities pContext)
      {
          try
          {
              List<ENTFAC> lesFacture = new List<ENTFAC>();
              foreach (CsEntfac item in _LaFacture)
              {
                  ENTFAC LeEntfact = Entities.ConvertObject<ENTFAC, CsEntfac>(item);
                  LeEntfact.PROFAC = Entities.ConvertObject<PROFAC, CsProduitFacture>(item.LstProfac.ToList());
                  LeEntfact.REDFAC = Entities.ConvertObject<REDFAC, CsRedevanceFacture >(item.lstRedfac .ToList());
                  lesFacture.Add(LeEntfact);
              }
                //cn = new SqlConnection(ConnectionString);
                //cmd = new SqlCommand();
                //cmd.Connection = cn;
                //Galatee.Tools.Utility.InsertionEnBloc<ENTFAC>(lesFacture, "ENTFAC", cmd);
                //  return true ;
              //using (var sqlBulkInsert = new SqlBulkCopy(pContext.Database.Connection.ConnectionString, SqlBulkCopyOptions.Default))
              //{
              //    sqlBulkInsert.BatchSize = 2500;
              //    sqlBulkInsert.DestinationTableName = "ENTFAC";
              //    DataTable pDtSource = new DataTable();
              //    pDtSource = Galatee.Tools.Utility.ListToDataTable(lesFacture);
              //    sqlBulkInsert.BulkCopyTimeout = 30;
              //    sqlBulkInsert.BatchSize = 5000;
              //    sqlBulkInsert.WriteToServer(pDtSource);
              //    return true;
              //}
              return Entities.InsertEntity<ENTFAC>(lesFacture, pContext);
          }
          catch (Exception ex)
          {
              throw ex;
          }
      }
      public void InsertEntfacteBulk(List<CsEntfac> _LaFacture, SqlCommand pContext)
      {
          try
          {
              foreach (CsEntfac item in _LaFacture)
              {
                  pContext.CommandTimeout = 18000;
                  List<CsMapperEntfac> lstPere = new List<CsMapperEntfac>();
                  Dictionary<string, DataTable> TableFille = new Dictionary<string, DataTable>();

                  CsMapperEntfac LeEntfact = Entities.ConvertObject<CsMapperEntfac, CsEntfac>(item);
                  List<CsMapperProFac> lstProfact = Entities.ConvertObject<CsMapperProFac, CsProduitFacture>(item.LstProfac.ToList());
                  List<CsMapperRedFac> lstRedfac = Entities.ConvertObject<CsMapperRedFac, CsRedevanceFacture>(item.lstRedfac.ToList());

                  lstPere.Add(LeEntfact);

                  DataTable TablePere = Galatee.Tools.Utility.ListToDataTable(lstPere);
                  DataTable TableFille1 = Galatee.Tools.Utility.ListToDataTable(lstProfact);
                  DataTable TableFille2 = Galatee.Tools.Utility.ListToDataTable(lstRedfac);
                  TableFille.Add("PROFAC", TableFille1);
                  TableFille.Add("REDFAC", TableFille2);
                  Galatee.Tools.Utility.BulkInsertManyToManyRelationship(TablePere, "ENTFAC", TableFille, "FK_IDENTFAC", pContext);
              }
          }
          catch (Exception ex)
          {
              throw ex;
          }
      }
      public void UpdateEvtfacteBulk(List<CsMapperEvenement> _Levt, SqlCommand pContext)
      {
          try
          {
                  List<string> ProprieteAModifier = new List<string>();
                  ProprieteAModifier.Add("STATUS");
                  ProprieteAModifier.Add("CONSOFAC");
                  ProprieteAModifier.Add("REGCONSO");
                  ProprieteAModifier.Add("FACTURE");
                  ProprieteAModifier.Add("TYPECONSO");
                  ProprieteAModifier.Add("QTEAREG");
                  ProprieteAModifier.Add("USERMODIFICATION");
                  ProprieteAModifier.Add("DATEMODIFICATION");

                  List<string> ProprieteDeJointure = new List<string>();
                  ProprieteDeJointure.Add("FK_IDCENTRE");
                  ProprieteDeJointure.Add("CENTRE");
                  ProprieteDeJointure.Add("CLIENT");
                  ProprieteDeJointure.Add("ORDRE");
                  ProprieteDeJointure.Add("LOTRI");

                  string Sufixe = string.Empty;
                  var properties = _Levt.First().GetType().GetProperties();

                  int NbrPropertie = properties.Count();
                  int Passage = 0;
                  foreach (var f in properties) // Récuperation des valeurs des propriete de l'objet
                  {
                      Passage += 1;
                      var type = f.PropertyType.FullName ;
                      string TypeVal = type.ToString();
                      if (type.Contains( "System.String"))
                          TypeVal ="Varchar(10)";
                      else if (type.Contains("System.Int32") || type.Contains("System.Int16"))
                          TypeVal = "int";
                      else if (type.Contains("System.Boolean"))
                          TypeVal = "bit";
                      else if (type.Contains("System.DateTime"))
                          TypeVal = "datetime";
                       else if (type.Contains("System.Decimal"))
                          TypeVal = "numeric(38, 10)";
                      else if (type.Contains("System.Byte"))
                          TypeVal = "tinyint";
                      
                      if (Passage == NbrPropertie )
                         Sufixe +=  f.Name + " " + TypeVal + " NULL " ;
                      else
                          Sufixe += f.Name + " " + TypeVal + " NULL " + " ,";
                  } 
                  string CreationTableTemp = "CREATE TABLE #TmpTable( "+ Sufixe  +")";

                  DataTable TablePere = Galatee.Tools.Utility.ListToDataTable(_Levt);
                  Galatee.Tools.Utility.UpdateData(ProprieteAModifier, ProprieteDeJointure, TablePere, "EVENEMENT", CreationTableTemp,ConnectionString);
              
          }
          catch (Exception ex)
          {
              throw ex;
          }
      }
      public static bool InsertLotri(List<CsLotri> _LesLotri, galadbEntities pContext)
      {
          try
          {
              List<LOTRI> lesLot = new List<LOTRI>();
              foreach (CsLotri item in _LesLotri)
                 lesLot.Add(Entities.ConvertObject <LOTRI, CsLotri>(item));
              return Entities.InsertEntity<LOTRI>(lesLot, pContext);
          }
          catch (Exception ex)
          {
              throw ex;
          }
      }
      public static void  UpdateLotri(CsLotri _LeLotri, galadbEntities pContext)
       {
           try
           {
                List<LOTRI> leLotRech = new List<LOTRI>();
                //leLotRech = pContext.LOTRI.Where(t => t.CENTRE == _LeLotri.CENTRE && t.NUMLOTRI == _LeLotri.NUMLOTRI && t.FK_IDCENTRE == _LeLotri.FK_IDCENTRE).ToList();
                leLotRech = pContext.LOTRI.Where(t =>t.NUMLOTRI == _LeLotri.NUMLOTRI).ToList();
                if (leLotRech != null)
                {
                    foreach (LOTRI item in leLotRech)
                    {
                        item.JET = _LeLotri.JET;
                        item.DFAC = _LeLotri.DFAC;
                        item.EXIG = _LeLotri.EXIG;
                        item.ETATFAC1 = _LeLotri.ETATFAC1;
                        item.ETATFAC4 = _LeLotri.ETATFAC4;
                        item.ETATFAC5 = _LeLotri.ETATFAC5;
                        item.USERMODIFICATION = _LeLotri.USERMODIFICATION;
                    }
                }


           }
           catch (Exception ex)
           {
               throw ex;
           }
       }

      public static void UpdateLotri(string  Matricule,string lotri,string periode,string jet, int Exig,bool IsSimulation, galadbEntities pContext)
      {
          try
          {
              List<LOTRI> leLotRech = new List<LOTRI>();
              leLotRech = pContext.LOTRI.Where(t => t.NUMLOTRI == lotri  && t.UserCalcul == Matricule && t.PERIODE == periode ).ToList();
              if (leLotRech != null)
              {
                  foreach (LOTRI item in leLotRech)
                  {
                      item.JET = jet;
                      item.DFAC = System.DateTime.Today.Date ;
                      item.EXIG = Exig;
                      item.ETATFAC5 = "R";
                      item.ETATFAC4 = IsSimulation ? Enumere.SimulationFacture : string.Empty;
                      item.USERMODIFICATION = Matricule;
                      item.UserCalcul = string.Empty;
                  }
              }


          }
          catch (Exception ex)
          {
              throw ex;
          }
      }



      public static bool UpdateEvenement(List<CsEvenement> _LstEvenement, galadbEntities pContext)
       {
           try
           {
               List<Galatee.Entity.Model.EVENEMENT> LstEvement = Entities.ConvertObject<Galatee.Entity.Model.EVENEMENT, CsEvenement>(_LstEvenement);
               return Entities.UpdateEntity<Galatee.Entity.Model.EVENEMENT>(LstEvement, pContext);
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }
      public static bool InsertEvenement(List<CsEvenement> _LstEvenement, galadbEntities pContext)
      {
          try
          {
              List<Galatee.Entity.Model.EVENEMENT> LstEvement = Entities.ConvertObject<Galatee.Entity.Model.EVENEMENT, CsEvenement>(_LstEvenement);
              return Entities.InsertEntity <Galatee.Entity.Model.EVENEMENT>(LstEvement, pContext);
          }
          catch (Exception ex)
          {
              throw ex;
          }
      }
      public List<CsEntfac> ConstruireEntfac(List<CsFactureBrut> lesFacures)
      {
          List<CsEntfac> lesEntfac = new List<CsEntfac>();
          // Distinct entfac
          List<CsFactureBrut> lesClients = new List<CsFactureBrut>();
          var lesClientDistinct = lesFacures.Select(t => new { t.FK_IDCLIENT}).Distinct().ToList();
          foreach (var item in lesClientDistinct)
              lesClients.Add(new CsFactureBrut { FK_IDCLIENT  = item.FK_IDCLIENT });

          foreach (CsFactureBrut item in lesClients)
          {
              CsEntfac leEntfClient = new CsEntfac();
              List<CsProduitFacture> lstProfac = new List<CsProduitFacture>();
              List<CsRedevanceFacture> lstRedfac = new List<CsRedevanceFacture>();

              List<CsFactureBrut> lesFactureParClient = lesFacures.Where(t => t.FK_IDCLIENT == item.FK_IDCLIENT).ToList();
              leEntfClient = Tools.Utility.ConvertEntity<CsEntfac, CsFactureBrut>(lesFactureParClient.First());
              lstRedfac = Tools.Utility.ConvertListType <CsRedevanceFacture , CsFactureBrut>(lesFactureParClient);
              leEntfClient.lstRedfac = lstRedfac;

              // Distinct profact
              List<CsFactureBrut> lesProfacClients = new List<CsFactureBrut>();
              var lesProfacClientDistinct = lesFactureParClient.Select(t => new { t.FK_IDCLIENT,t.FK_IDABON ,t.FK_IDPRODUIT  }).Distinct().ToList();
              foreach (var items in lesProfacClientDistinct)
                  lesProfacClients.Add(new CsFactureBrut { FK_IDCLIENT = items.FK_IDCLIENT, FK_IDABON = items.FK_IDABON, FK_IDPRODUIT = items.FK_IDPRODUIT });
              if (lesProfacClients != null && lesProfacClients.Count != 0)
              {
                  foreach (CsFactureBrut itemss in lesProfacClients)
	                {
                        CsFactureBrut lesProduitFactureParClient = lesFacures.FirstOrDefault (t => t.FK_IDCLIENT == itemss.FK_IDCLIENT && t.FK_IDABON ==itemss.FK_IDABON && t.FK_IDPRODUIT == itemss.FK_IDPRODUIT  );
                        lstProfac.Add(Tools.Utility.ConvertEntity<CsProduitFacture, CsFactureBrut>(lesProduitFactureParClient));
                        leEntfClient.LstProfac = lstProfac;
	                }
              }
              lesEntfac.Add(leEntfClient);
          }
          return lesEntfac;
      }

      public  List<CsEntfac> ConstruireEntfacMt(List<CsFactureBrut> lesFacures)
      {
          List<CsEntfac> lesEntfac = new List<CsEntfac>();
          // Distinct entfac
          List<CsFactureBrut> lesClients = new List<CsFactureBrut>();
          var lesClientDistinct = lesFacures.Select(t => new { t.FK_IDCLIENT }).Distinct().ToList();
          foreach (var item in lesClientDistinct)
              lesClients.Add(new CsFactureBrut { FK_IDCLIENT = item.FK_IDCLIENT });

          foreach (CsFactureBrut item in lesClients)
          {
              CsEntfac leEntfClient = new CsEntfac();
              List<CsProduitFacture> lstProfac = new List<CsProduitFacture>();
              List<CsRedevanceFacture> lstRedfac = new List<CsRedevanceFacture>();

              List<CsFactureBrut> lesFactureParClient = lesFacures.Where(t => t.FK_IDCLIENT == item.FK_IDCLIENT).ToList();
              leEntfClient = Tools.Utility.ConvertEntity<CsEntfac, CsFactureBrut>(lesFactureParClient.First());


              // Distinct profact
              List<CsFactureBrut> lesRedfactClients = new List<CsFactureBrut>();
              var lesRedfacClientDistinct = lesFactureParClient.Select(t => new { t.FK_IDCLIENT, t.FK_IDABON, t.FK_IDPRODUIT,t.REDEVANCE  }).Distinct().ToList();
              foreach (var items in lesRedfacClientDistinct)
                  lesRedfactClients.Add(new CsFactureBrut { FK_IDCLIENT = items.FK_IDCLIENT, FK_IDABON = items.FK_IDABON, FK_IDPRODUIT = items.FK_IDPRODUIT,REDEVANCE = items.REDEVANCE  });
              if (lesRedfactClients != null && lesRedfactClients.Count != 0)
              {
                  foreach (CsFactureBrut itemss in lesRedfactClients)
                  {
                      CsFactureBrut lesRedevanceFactureParClient = lesFacures.FirstOrDefault(t => t.FK_IDCLIENT == itemss.FK_IDCLIENT && 
                                                                                           t.FK_IDABON == itemss.FK_IDABON && 
                                                                                           t.FK_IDPRODUIT == itemss.FK_IDPRODUIT &&
                                                                                           t.REDEVANCE == itemss.REDEVANCE );
                      lstRedfac.Add(Tools.Utility.ConvertEntity<CsRedevanceFacture, CsFactureBrut>(lesRedevanceFactureParClient));
                  }
                  leEntfClient.lstRedfac = lstRedfac;
              }


              // Distinct profact
              List<CsFactureBrut> lesProfacClients = new List<CsFactureBrut>();
              var lesProfacClientDistinct = lesFactureParClient.Select(t => new { t.FK_IDCLIENT, t.FK_IDABON, t.FK_IDPRODUIT,t.FK_IDEVENEMENT  }).Distinct().ToList();
              foreach (var items in lesProfacClientDistinct)
                  lesProfacClients.Add(new CsFactureBrut { FK_IDCLIENT = items.FK_IDCLIENT, FK_IDABON = items.FK_IDABON, FK_IDPRODUIT = items.FK_IDPRODUIT,FK_IDEVENEMENT = items.FK_IDEVENEMENT  });

              if (lesProfacClients != null && lesProfacClients.Count != 0)
              {
                  foreach (CsFactureBrut itemss in lesProfacClients)
                  {
                      CsFactureBrut lesProduitFactureParClient = lesFacures.FirstOrDefault(t => t.FK_IDCLIENT == itemss.FK_IDCLIENT && 
                                                                                                t.FK_IDABON == itemss.FK_IDABON && 
                                                                                                t.FK_IDPRODUIT == itemss.FK_IDPRODUIT &&
                                                                                                t.FK_IDEVENEMENT == itemss.FK_IDEVENEMENT );
                      lstProfac.Add(Tools.Utility.ConvertEntity<CsProduitFacture, CsFactureBrut>(lesProduitFactureParClient));
                      leEntfClient.LstProfac = lstProfac;
                  }
              }
              lesEntfac.Add(leEntfClient);
          }
          return lesEntfac;
      }


      //public bool ValiderFacturation(List<CsFactureBrut> laFacturation,bool IsFactureResiler )
      //{
      //    try
      //    {
      //        int res = -1;
      //        using (galadbEntities ctx = new galadbEntities())
      //        {
      //                 List<CsEntfac> lesfacture= new List<CsEntfac>();
      //                if (laFacturation.First().PRODUIT == Enumere.ElectriciteMT)
      //                    lesfacture = ConstruireEntfacMt(laFacturation);
      //                else
      //                    lesfacture = ConstruireEntfac(laFacturation);

      //                List<Galatee.Entity.Model.EVENEMENT> leVt = new List<Galatee.Entity.Model.EVENEMENT>();
      //                foreach (var item in lesfacture)
      //                {
      //                    foreach (var items in item.LstProfac)
      //                    {
      //                        Galatee.Entity.Model.EVENEMENT levt = new Entity.Model.EVENEMENT();
      //                        levt = ctx.EVENEMENT.FirstOrDefault(t => t.PK_ID == items.FK_IDEVENEMENT);
      //                        if (levt != null)
      //                        {
      //                            levt.STATUS = Enumere.EvenementFacture;
      //                            levt.CONSOFAC = items.CONSOFAC;
      //                            levt.REGCONSO = items.CONSOFAC;
      //                            levt.FACTURE = items.FACTURE;
      //                            levt.TYPECONSO = int.Parse(items.TFAC);
      //                            levt.QTEAREG = items.REGFAC;
      //                            levt.USERMODIFICATION = items.USERCREATION;
      //                            levt.DATEMODIFICATION = System.DateTime.Now;
      //                        }
      //                    }
      //                }

      //                InsertEntfacteBulk(lesfacture, ctx);
                  
      //                //UpdateLotri(laFacturation.First().USERCREATION, laFacturation.First().LOTRI, laFacturation.First().PERIODE, laFacturation.First().JET, laFacturation.First().EXIG, laFacturation.First().IsSimuler, ctx);
      //                //if (!IsFactureResiler)
      //                //   MiseAJourAction(lesfacture, ctx);
      //                //res = ctx.SaveChanges();
      //                //string Jet = laFacturation.First().JET;
      //                //if (IsFactureResiler)
      //                //{
      //                //    List<CsLotri> lstLot = new List<CsLotri>();
      //                //    lstLot.Add(new CsLotri
      //                //    {
      //                //        CENTRE = laFacturation.First().CENTRE,
      //                //        FK_IDCENTRE = laFacturation.First().FK_IDCENTRE,
      //                //        DATECREATION = DateTime.Now,
      //                //        DATEMODIFICATION = DateTime.Now,
      //                //        FK_IDCATEGORIECLIENT = null,
      //                //        FK_IDPRODUIT = laFacturation.First().FK_IDPRODUIT,
      //                //        NUMLOTRI = laFacturation.First().LOTRI,
      //                //        MOISCOMPTA = DateTime.Now.Year + DateTime.Now.Month.ToString("00"),
      //                //        JET = Jet,
      //                //        PERIODE = DateTime.Now.Year + (DateTime.Now.Month.ToString().Length > 1 ? DateTime.Now.Month.ToString() : "0" + DateTime.Now.Month.ToString()),
      //                //        PRODUIT = laFacturation.First().PRODUIT,
      //                //        USERCREATION = laFacturation.First().USERCREATION,
      //                //        USERMODIFICATION = laFacturation.First().USERCREATION,
      //                //        MATRICULE = laFacturation.First().USERCREATION
      //                //    });
      //                //    new DbFacturation().MiseAjourLots(lstLot, true);
      //                //}
      //            }
      //            return res != -1 ? true : false;
              
      //    }
      //    catch (Exception ex)
      //    {
      //        throw new Exception(ex.Message);
      //    }
      //}



        /*
      public bool ValiderFacturation(List<CsFactureBrut> laFacturation,bool IsFactureResiler )
      {
          cmd = new SqlCommand();
          string evt = string.Empty;
          try
          {
                      int res = -1;
                      cn = new SqlConnection(ConnectionString);
                      cmd.Connection = cn;
                      cmd.CommandTimeout = 18000;
                      cmd.Connection.Open();
                      cmd.Transaction = cmd.Connection.BeginTransaction();

                      galadbEntities ctxInter = new galadbEntities();
                       List<CsEntfac> lesfacture= new List<CsEntfac>();
                      if (laFacturation.First().PRODUIT == Enumere.ElectriciteMT)
                          lesfacture = ConstruireEntfacMt(laFacturation);
                      else
                          lesfacture = ConstruireEntfac(laFacturation);

                      foreach (var item in lesfacture)
                      {
                          foreach (var items in item.LstProfac)
                          {
                              int tfac = items.TFAC == null ? 1 : int.Parse(items.TFAC);
                              MiseAJoursEvenementSpx(items.LOTRI, items.FK_IDCENTRE, items.CENTRE,
                                                     items.CLIENT, items.ORDRE, items.FK_IDEVENEMENT.Value,
                                                     Enumere.EvenementFacture, items.CONSOFAC.Value, items.CONSOFAC.Value,
                                                     items.FACTURE, tfac, items.REGFAC.Value, items.USERCREATION, cmd);
                          }
                      }
                      if (!IsFactureResiler ||(lesfacture!= null && lesfacture.First().TOTFTTC != 0))
                      {
                          InsertEntfacteBulk(lesfacture, cmd);
                          MiseAJourAction(lesfacture, cmd);
                          MiseAjourLotApresCalculSpx(laFacturation.First().USERCREATION, laFacturation.First().LOTRI, laFacturation.First().PERIODE, laFacturation.First().EXIG, laFacturation.First().JET, laFacturation.First().IsSimuler , cmd);
                      }
                      //UpdateEvtfacteBulk(leVt, cmd);
                      string Jet = laFacturation.First().JET;
                      if (IsFactureResiler && (lesfacture != null && lesfacture.First().TOTFTTC != 0))
                      {
                          cmd.Transaction.Commit();
                          List<CsLotri> lstLot = new List<CsLotri>();
                          lstLot.Add(new CsLotri
                          {
                              CENTRE = laFacturation.First().CENTRE,
                              FK_IDCENTRE = laFacturation.First().FK_IDCENTRE,
                              DATECREATION = DateTime.Now,
                              DATEMODIFICATION = DateTime.Now,
                              FK_IDCATEGORIECLIENT = null,
                              FK_IDPRODUIT = laFacturation.First().FK_IDPRODUIT,
                              NUMLOTRI = laFacturation.First().LOTRI,
                              MOISCOMPTA = DateTime.Now.Year + DateTime.Now.Month.ToString("00"),
                              JET = Jet,
                              PERIODE = DateTime.Now.Year + (DateTime.Now.Month.ToString().Length > 1 ? DateTime.Now.Month.ToString() : "0" + DateTime.Now.Month.ToString()),
                              PRODUIT = laFacturation.First().PRODUIT,
                              USERCREATION = laFacturation.First().USERCREATION,
                              USERMODIFICATION = laFacturation.First().USERCREATION,
                              MATRICULE = laFacturation.First().USERCREATION,
                              EXIG = laFacturation.First().EXIG ,
                              DFAC = System.DateTime.Today.Date
                          });
                          new DbFacturation().MiseAjourLots(lstLot, true);
                      }
                      else
                      {
                          cmd.Transaction.Commit();
                          return true;
                      }
                     
                      return true ;
              
          }
          catch (Exception ex)
          {
              string ev = evt;
              cmd.Transaction.Rollback();
              return false ;
          }
          finally
          {
              cmd.Dispose();
          }
      }
        */




      public bool ValiderFacturation(List<CsFactureBrut> laFacturation, bool IsFactureResiler)
      {
          //cmd = new SqlCommand();
          SqlCommand laCommande = DBBase.InitTransaction(ConnectionString);

          string evt = string.Empty;
          try
          {
              int res = -1;


              //cn = new SqlConnection(ConnectionString);
              //cmd.Connection = cn;
              //cmd.CommandTimeout = 18000;
              //cmd.Connection.Open();
              //cmd.Transaction = cmd.Connection.BeginTransaction();

              //galadbEntities ctxInter = new galadbEntities();



              /* ZEG 29/08/2017 => EVITER DE RAMENER LES FACTURES COTE CLIENT POUR LA FACTURATION HORS RESILIATION
                        
              List<CsEntfac> lesfacture= new List<CsEntfac>();
             if (laFacturation.First().PRODUIT == Enumere.ElectriciteMT)
                 lesfacture = ConstruireEntfacMt(laFacturation);
             else
                 lesfacture = ConstruireEntfac(laFacturation);

             foreach (var item in lesfacture)
             {
                 foreach (var items in item.LstProfac)
                 {
                     int tfac = items.TFAC == null ? 1 : int.Parse(items.TFAC);
                     MiseAJoursEvenementSpx(items.LOTRI, items.FK_IDCENTRE, items.CENTRE,
                                            items.CLIENT, items.ORDRE, items.FK_IDEVENEMENT.Value,
                                            Enumere.EvenementFacture, items.CONSOFAC.Value, items.CONSOFAC.Value,
                                            items.FACTURE, tfac, items.REGFAC.Value, items.USERCREATION, cmd);
                 }
             }

                        
                */

              //if (!IsFactureResiler || (lesfacture != null && lesfacture.First().TOTFTTC != 0))
              if (!IsFactureResiler)
              {
                  //InsertEntfacteBulk(lesfacture, cmd);
                  MiseAJourAction(laFacturation, laCommande);
                  MiseAjourLotApresCalculSpx(laFacturation.First().USERCREATION, laFacturation.First().LOTRI, laFacturation.First().PERIODE, laFacturation.First().EXIG, laFacturation.First().JET, laFacturation.First().IsSimuler, laCommande);

                  //laCommande.Transaction.Commit();
              }
              else
              {
                  string MOISCOMPTA = DateTime.Now.Year + DateTime.Now.Month.ToString("00");

                  MiseAjourLotApresCalculSpx(laFacturation.First().USERCREATION, laFacturation.First().LOTRI, laFacturation.First().PERIODE, laFacturation.First().EXIG, laFacturation.First().JET, laFacturation.First().IsSimuler, laCommande);
                  new DbFacturation().MiseAJourDuLotCalculer(laFacturation.First().FK_IDCENTRE, laFacturation.First().LOTRI, laFacturation.First().JET, MOISCOMPTA, laFacturation.First().USERCREATION, laCommande);

                  /* 05/04/2018
                  List<CsEntfac> lesfacture = new List<CsEntfac>();
                  if (laFacturation.First().PRODUIT == Enumere.ElectriciteMT)
                      lesfacture = ConstruireEntfacMt(laFacturation);
                  else
                      lesfacture = ConstruireEntfac(laFacturation);

                  foreach (var item in lesfacture)
                  {
                      foreach (var items in item.LstProfac)
                      {
                          int tfac = items.TFAC == null ? 1 : int.Parse(items.TFAC);
                          MiseAJoursEvenementSpx(items.LOTRI, items.FK_IDCENTRE, items.CENTRE,
                                                 items.CLIENT, items.ORDRE, items.FK_IDEVENEMENT.Value,
                                                 Enumere.EvenementFacture, items.CONSOFAC.Value, items.CONSOFAC.Value,
                                                 items.FACTURE, tfac, items.REGFAC.Value, items.USERCREATION, laCommande);
                      }
                  }

                  if (lesfacture != null && lesfacture.First().TOTFTTC != 0)
                  {

                      string Jet = laFacturation.First().JET;


                      List<CsLotri> lstLot = new List<CsLotri>();
                      lstLot.Add(new CsLotri
                      {
                          CENTRE = laFacturation.First().CENTRE,
                          FK_IDCENTRE = laFacturation.First().FK_IDCENTRE,
                          DATECREATION = DateTime.Now,
                          DATEMODIFICATION = DateTime.Now,
                          FK_IDCATEGORIECLIENT = null,
                          FK_IDPRODUIT = laFacturation.First().FK_IDPRODUIT,
                          NUMLOTRI = laFacturation.First().LOTRI,
                          MOISCOMPTA = DateTime.Now.Year + DateTime.Now.Month.ToString("00"),
                          JET = Jet,
                          PERIODE = DateTime.Now.Year + (DateTime.Now.Month.ToString().Length > 1 ? DateTime.Now.Month.ToString() : "0" + DateTime.Now.Month.ToString()),
                          PRODUIT = laFacturation.First().PRODUIT,
                          USERCREATION = laFacturation.First().USERCREATION,
                          USERMODIFICATION = laFacturation.First().USERCREATION,
                          MATRICULE = laFacturation.First().USERCREATION,
                          EXIG = laFacturation.First().EXIG,
                          DFAC = System.DateTime.Today.Date
                      });
                      new DbFacturation().MiseAjourLots(lstLot, true, laCommande);

                      laCommande.Transaction.Commit();

                  }
                   */
              }

              laCommande.Transaction.Commit();

              return true;

          }
          catch (Exception ex)
          {
              string ev = evt;
              laCommande.Transaction.Rollback();
              throw new Exception(ex.Message);
          }
          finally
          {
              if (laCommande.Connection.State == ConnectionState.Open)
                  laCommande.Connection.Close(); // Fermeture de la connection 
              laCommande.Dispose();
          }
      }





         
      public bool AnnulerFactureResiliation(List<CsFactureBrut> laFacturation )
      {
          try
          {
             
                      int resu = -1;
                      galadbEntities ctxInter = new galadbEntities();
                      List<CsEntfac> lesfacture = new List<CsEntfac>();
                      if (laFacturation.First().PRODUIT == Enumere.ElectriciteMT)
                          lesfacture = ConstruireEntfacMt(laFacturation);
                      else
                          lesfacture = ConstruireEntfac(laFacturation);
                      List<Galatee.Entity.Model.EVENEMENT> leVt = new List<Galatee.Entity.Model.EVENEMENT>();
                      foreach (var item in lesfacture)
                      {
                          foreach (var items in item.LstProfac)
                          {
                              Galatee.Entity.Model.EVENEMENT levt = new Entity.Model.EVENEMENT();
                              levt = ctxInter.EVENEMENT.FirstOrDefault(t => t.PK_ID == items.FK_IDEVENEMENT);
                              if (levt != null)
                                  leVt.Add(levt);
                          }
                      }
                      ctxInter.Dispose();
                      using (galadbEntities ctx = new galadbEntities())
                      {
                          Entities.DeleteEntity<Galatee.Entity.Model.EVENEMENT>(leVt);
                          resu =  ctx.SaveChanges();
                      }
                      return resu == -1 ? false : true;
               
          }
          catch (Exception ex)
          {
              throw new Exception(ex.Message);
          }
      }

      public List<CsLotri> ChargerLotriFromEntfac(List<int> lstCentre)
       {
           try
           {
              return  FacturationProcedure.ChargerLotriFromEntfac(lstCentre);
           }
           catch (Exception ex)
           {
               throw new Exception(ex.Message);
           }
       }

       private int RetourneRedevenceParametre(List<CsRedevance> LstDesRedevanceTarif, CsTarifFacturation _LeTarif, List<CsRedevanceFacture> LstRedfac)
       {
           List<CsRedevance> lstRedevanceParam = new List<CsRedevance>();
           List<CsLienRedevence> lstLienRedevance = RetourneLienRedevance();
           CsRedevance LaRedevanceDuTarif = LstDesRedevanceTarif.FirstOrDefault (t => t.PK_ID == _LeTarif.FK_IDREDEVANCE);
           if (LaRedevanceDuTarif.TYPELIEN == Enumere.A_DES_PARAMETRES || LaRedevanceDuTarif.TYPELIEN == Enumere.LIE_ET_A_DES_PARAMETRES)
           {
               List<CsLienRedevence> lstRedLien = lstLienRedevance.Where(t => t.FK_IDREDEVANCE  == LaRedevanceDuTarif.PK_ID ).ToList();
               foreach (CsLienRedevence item in lstRedLien)
		          lstRedevanceParam.Add(LstDesRedevanceTarif.FirstOrDefault(t=>t.PK_ID == item.FK_IDREDEVANCEPARAMETRE  ));
           }
           int conso = 0;
           foreach (CsRedevance item in lstRedevanceParam)
           {
               List<CsRedevanceFacture > lstRedevance = LstRedfac.Where(t => t.REDEVANCE == item.CODE ).ToList();
               if (lstRedevance!= null && lstRedevance.Count != 0)
                   conso +=(int)lstRedevance.Sum(t => t.TOTREDHT);
           }
           return conso;
       }

       private List<CsLienRedevence > RetourneLienRedevance()
       {
           try
           {
                 DataTable   dt = FacturationProcedure.RetourneLienRedevance();
                 List<CsLienRedevence> lstDesLien = Entities.GetEntityListFromQuery<CsLienRedevence>(dt);
                 return lstDesLien;
           }
           catch (Exception ex)
           {
               throw new Exception(ex.Message);
           }
       }

       public  List<CsAnnomalie> RetourneLesteAnomalie(string lotri,int idCentre)
       {
           try
           {
               DataTable dt = FacturationProcedure.RetourneLesteAnomalie(lotri);
               return Entities.GetEntityListFromQuery<CsAnnomalie>(dt);
                 
           }
           catch (Exception ex)
           {
               throw new Exception(ex.Message);
           }
       }
       public void SupprimeAnnomalie(string lotri, int idCentre)
       {
           try
           {
                FacturationProcedure.RetourneLesteAnomalie(lotri);
           }
           catch (Exception ex)
           {
               throw new Exception(ex.Message);
           }
       }


       public string ValiderAnnulationFacture(CsDemande laDemande, int idEntfac)
       {
           SqlCommand laCommande = DBBase.InitTransaction(ConnectionString);
           try
           {

               if(!AnnulationFacture(laDemande.LaDemande.PK_ID, laDemande.LaDemande.USERMODIFICATION, idEntfac,  laCommande))
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


       private bool AnnulationFacture(int FK_IDDEMANDE, string MATRICULE, int FK_IDENTFAC, SqlCommand cmd)
       {
           cmd.CommandTimeout = 18000;
           cmd.CommandType = CommandType.StoredProcedure;
           cmd.CommandText = "SPX_FAC_ANNULATIONFACTURE";
           cmd.Parameters.Add("@FK_IDENTFAC", SqlDbType.Int).Value = FK_IDENTFAC;
           cmd.Parameters.Add("@FK_IDDEMANDE", SqlDbType.Int).Value = FK_IDDEMANDE;
           cmd.Parameters.Add("@MATRICULE", SqlDbType.VarChar, 6).Value = MATRICULE;

           DBBase.SetDBNullParametre(cmd.Parameters);
           try
           {
               if (cmd.Connection.State == ConnectionState.Closed)
                   cmd.Connection.Open();

               int reader = -1;
               reader = cmd.ExecuteNonQuery();
               return (reader > 0);
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


       public bool MiseAJoursEvenementSpx(string lotri,int fk_idcentre,string centre,string client,string ordre,int pk_id,
                                          int status,int consofac,int regconso,string facture,int typeconso,int qteareg,string matricule, SqlCommand cmd)
       {

           cmd.CommandTimeout = 18000;
           cmd.CommandType = CommandType.StoredProcedure;
           if (cmd.Parameters.Count != 0) cmd.Parameters.Clear();
           cmd.Parameters.Add("@LOTRI", SqlDbType.VarChar, 8).Value = lotri ;
           cmd.Parameters.Add("@FK_IDCENTRE", SqlDbType.Int).Value = fk_idcentre ;
           cmd.Parameters.Add("@CENTRE", SqlDbType.VarChar, 3).Value = centre ;
           cmd.Parameters.Add("@CLIENT", SqlDbType.VarChar, 20).Value = client ;
           cmd.Parameters.Add("@ORDRE", SqlDbType.VarChar, 3).Value = ordre ;
           cmd.Parameters.Add("@PK_ID", SqlDbType.Int).Value = pk_id ;

           cmd.Parameters.Add("@STATUS", SqlDbType.Int).Value = status ;
           cmd.Parameters.Add("@CONSOFAC", SqlDbType.Int).Value = consofac ;
           cmd.Parameters.Add("@REGCONSO", SqlDbType.Int).Value = regconso ;
           cmd.Parameters.Add("@FACTURE", SqlDbType.VarChar ,6).Value = facture ;
           cmd.Parameters.Add("@TYPECONSO", SqlDbType.Int).Value = typeconso ;
           cmd.Parameters.Add("@QTEAREG", SqlDbType.Int).Value = qteareg ;
           cmd.Parameters.Add("@USERMODIFICATION", SqlDbType.VarChar, 6).Value = matricule ;

           cmd.CommandText = "SPX_FAC_MISEAJOUR_EVEMENT";

           DBBase.SetDBNullParametre(cmd.Parameters);
           try
           {
               if (cmd.Connection.State == ConnectionState.Closed)
                   cmd.Connection.Open();
               int reader = -1;
               reader = cmd.ExecuteNonQuery();
               return reader == -1 ? false : true;
           }
           catch (Exception ex)
           {
               int idevt = pk_id;
               throw new Exception(cmd.CommandText + ":" + ex.Message);
           }
       }


       public bool MiseAjoutLotSpx(int pk_ID , string matricule,string MatriculeAgent,int Fk_Idcentre,string lotri ,bool IsIsole)
       {
           cn = new SqlConnection(ConnectionString);

           cmd = new SqlCommand();
           cmd.Connection = cn;
           cmd.CommandTimeout = 18000;
           cmd.CommandType = CommandType.StoredProcedure;
           cmd.Parameters.Add("@PK_ID", SqlDbType.Int ).Value = pk_ID ;
           cmd.Parameters.Add("@MATRICULE", SqlDbType.VarChar, 6).Value = matricule ;
           cmd.Parameters.Add("@MATRICULEAGENT", SqlDbType.VarChar , 6).Value = matricule ;
           cmd.Parameters.Add("@FK_IDCENTRE", SqlDbType.Int ).Value = Fk_Idcentre ;
           cmd.Parameters.Add("@LOTRI", SqlDbType.VarChar, 8).Value = lotri ;
           cmd.Parameters.Add("@IsIsole", SqlDbType.Bit).Value = IsIsole ? true : false;

           cmd.CommandText = "SPX_FAC_MISEAJOUR_LOTRI";
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
       public void InserOrUpdateLotriSpx(CsLotri NewLotri, SqlCommand cmd)
       {
           cmd.CommandTimeout = 3000;
           cmd.CommandType = CommandType.StoredProcedure;
           if (cmd.Parameters != null && cmd.Parameters.Count != 0) cmd.Parameters.Clear();

           cmd.CommandText = "SPX_ACC_INSERTORUPDATE_LOTRI";
           cmd.Parameters.Add("@NUMLOTRI", SqlDbType.VarChar, 8).Value = NewLotri.NUMLOTRI;
           cmd.Parameters.Add("@JET", SqlDbType.VarChar, 2).Value = NewLotri.JET;
           cmd.Parameters.Add("@PERIODE", SqlDbType.VarChar, 6).Value = NewLotri.PERIODE;
           cmd.Parameters.Add("@CENTRE", SqlDbType.VarChar, 3).Value = NewLotri.CENTRE;
           cmd.Parameters.Add("@PRODUIT", SqlDbType.VarChar, 3).Value = NewLotri.PRODUIT;
           cmd.Parameters.Add("@CATEGORIECLIENT", SqlDbType.VarChar, 2).Value = NewLotri.CATEGORIECLIENT;
           cmd.Parameters.Add("@PERIODICITE", SqlDbType.VarChar, 2).Value = NewLotri.PERIODICITE;
           cmd.Parameters.Add("@TOURNEE", SqlDbType.VarChar, 2).Value = NewLotri.TOURNEE;
           cmd.Parameters.Add("@RELEVEUR", SqlDbType.VarChar, 2).Value = NewLotri.RELEVEUR;
           cmd.Parameters.Add("@BASE", SqlDbType.VarChar, 2).Value = NewLotri.BASE;
           cmd.Parameters.Add("@FK_IDPRODUIT", SqlDbType.VarChar, 2).Value = NewLotri.FK_IDPRODUIT;
           cmd.Parameters.Add("@FK_IDCATEGORIECLIENT", SqlDbType.VarChar, 2).Value = NewLotri.FK_IDCATEGORIECLIENT;
           cmd.Parameters.Add("@FK_IDRELEVEUR", SqlDbType.VarChar, 2).Value = NewLotri.FK_IDRELEVEUR;
           cmd.Parameters.Add("@FK_IDTOURNEE", SqlDbType.VarChar, 2).Value = NewLotri.FK_IDTOURNEE;
           cmd.Parameters.Add("@FK_IDCENTRE", SqlDbType.VarChar, 2).Value = NewLotri.FK_IDCENTRE;
           cmd.Parameters.Add("@USERCREATION", SqlDbType.VarChar, 2).Value = NewLotri.USERCREATION;

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
       }

       public void DeleteAnnomalieLotSpx(List<CsLotri> Lotri, SqlCommand cmd)
       {
           string lesLotr = DBBase.RetourneStringListeObject(Lotri.Select(p => p.NUMLOTRI).ToList());
           cmd.CommandTimeout = 3000;
           cmd.CommandType = CommandType.StoredProcedure;
           if (cmd.Parameters != null && cmd.Parameters.Count != 0) cmd.Parameters.Clear();
           cmd.CommandText = "SPX_FAC_SUPPRESSION_ANOMALIE";
           cmd.Parameters.Add("@NUMLOTRI", SqlDbType.VarChar, int.MaxValue).Value = lesLotr;

           DBBase.SetDBNullParametre(cmd.Parameters);
           try
           {
               if (cmd.Connection.State == ConnectionState.Closed)
                   cmd.Connection.Open();
               cmd.ExecuteNonQuery();
           }
           catch (Exception ex)
           {
               throw new Exception(cmd.CommandText + ":" + ex.Message);
           }
       }
    
        #endregion

        #endregion
    }
}


