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
using Galatee.Entity.Model;
using System.Threading.Tasks;
using System.Data.Entity.Validation;
using System.Data.Entity.Infrastructure;
using Oracle.DataAccess.Client;
using System.IO;
using System.Net.Http;
using Newtonsoft.Json;
using System.Web.UI.WebControls;
using System.Windows;
using System.Drawing;
using  System.IO.Compression;

namespace Galatee.DataAccess
{
    public class DBAccueil
    {
        public DBAccueil()
        {
            try
            {
                ConnectionString = Session.GetSqlConnexionString();
            }
            catch (Exception)
            {
                //Ckecking in test
                throw;
            }
        }
        private string ConnectionString;
        private SqlCommand cmd = null;
        private SqlConnection cn = null;
        public bool EclipseSimpleCompteurTransition(CsDemande laDemande)
        {
            try
            {
                using (galadbEntities ctx = new galadbEntities())
                {

                    CANALISATION lstCan = ctx.CANALISATION.FirstOrDefault(i => i.FK_IDCENTRE == laDemande.LeClient.FK_IDCENTRE
                                                                            && i.CENTRE == laDemande.LeClient.CENTRE
                                                                            && i.CLIENT == laDemande.LeClient.REFCLIENT);
                    if (lstCan != null)
                    {
                        MAGASINVIRTUEL mv = ctx.MAGASINVIRTUEL.FirstOrDefault(p => p.NUMERO == laDemande.LaDemande.COMPTEUR);
                        if (mv != null)
                        {
                            mv.ETAT = Enumere.CompteurLie;
                            COMPTEUR lecomp = ctx.COMPTEUR.FirstOrDefault(l => l.PK_ID == lstCan.FK_IDCOMPTEUR);
                            if (lecomp != null)
                            {
                                lecomp.NUMERO = laDemande.LaDemande.COMPTEUR;
                                lecomp.MARQUE = mv.MARQUE;
                                lecomp.FK_IDMARQUECOMPTEUR = mv.FK_IDMARQUECOMPTEUR;
                                lecomp.ANNEEFAB = mv.ANNEEFAB;
                            }
                        }
                        DbInterfaceComptable db = new DbInterfaceComptable();
                        this.InsererEclipse(laDemande, laDemande.LaDemande.COMPTEUR);
                        ctx.SaveChanges();
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region GUI00


        public List<CsCentre> ChargerLesCentre()
        {
            //cmd.CommandText = "SPX_GUI_RETOURNE_COMMUNE";
            try
            {
                DataTable dt = Galatee.Entity.Model.CommonProcedures.RetourneListeCentre();
                return Entities.GetEntityListFromQuery<CsCentre>(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        


        public List<CsReglageCompteur > ChargerReglageCompteur()
        {
            //cmd.CommandText = "SPX_GUI_RETOURNE_DIAMETRE";
            try
            {
                return new DB_REGLAGECOMPTEUR().SelectAllReglageCompteur();
            }
            catch (Exception ex)
            {
                throw new Exception(cmd.CommandText + ":" + ex.Message);
            }
        }


        public List<CsSpeSite >     ChargerSpecificiteSite(string _pSite)
        {
            //cmd.CommandText = "[SPX_GUI_RETOURNE_ETAPEDEMANDE]";
            try
            {
                DataTable dt = Galatee.Entity.Model.CommonProcedures .RetourneSpecificiteSite(_pSite);
                return Entities.GetEntityListFromQuery<CsSpeSite>(dt);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public List<CsCentre >      RetourneCentre()
        {
            try
            {
                DataTable dt = Galatee.Entity.Model.CommonProcedures.RetourneTousCentres();
                return Entities.GetEntityListFromQuery<CsCentre>(dt).Where(p => p.CODE == Enumere.Generale).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public List<CsNatureClient  >   RetourneNatureClient()
        {
            try
            {
                DataTable dt = Galatee.Entity.Model.CommonProcedures.RetourneNatureClient();
                return Entities.GetEntityListFromQuery<CsNatureClient>(dt);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<CsMaterielBranchement > RetourneMaterielBranchement()
        {
            try
            {
                DataTable dt = Galatee.Entity.Model.CommonProcedures .RetourneMaterielBranchement();
                return Entities.GetEntityListFromQuery<CsMaterielBranchement>(dt);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public string NumeroFacture(int PkiCentre)
        {
            try
            {
                return Galatee.Entity.Model.AccueilProcedures.NumeroFacture(PkiCentre);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public string NumeroFacture()
        {
            try
            {
                return Galatee.Entity.Model.AccueilProcedures.NumeroFacture();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


       public Dictionary<List <CsClientRechercher >,List<CsLclient>> RechercherClientRegrouper(CsRegCli  leRegroupement)
        {
            //cmd.CommandText = "SPX_GUI_RECHERCHE_CLIENT";
            try
            {
                Dictionary<List<CsClientRechercher>, List<CsLclient>> leResultat = new Dictionary<List<CsClientRechercher>, List<CsLclient>>();
                List<CsClientRechercher> LesClient = RetourneClientSpx(leRegroupement);
                List<CsLclient> lesFacture = RetourneCompteClientSpx(leRegroupement, string.Empty, string.Empty);
                leResultat.Add(LesClient, lesFacture);
                return leResultat;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    
        public List<CsClientRechercher> RechercherClient(CsClientRechercher _CritereClient)
        {
            //cmd.CommandText = "SPX_GUI_RECHERCHE_CLIENT";
            try
            {
                DataTable dt = new DataTable();
                if (_CritereClient.OptionRecherche == 1)  // Client
                    dt = Galatee.Entity.Model.AccueilProcedures.RechercherClient(_CritereClient.FK_IDCENTRE ,_CritereClient.CENTRE, _CritereClient.CLIENT, _CritereClient.ORDRE, _CritereClient.PRODUIT, _CritereClient.NOMABON);

                else if (_CritereClient.OptionRecherche == 2)  // LOCALISATION
                    dt = Galatee.Entity.Model.AccueilProcedures.RechercherLocalisation(_CritereClient.FK_IDCENTRE, _CritereClient.CENTRE, _CritereClient.TOURNEE, _CritereClient.SEQUENCE, _CritereClient.PRODUIT, _CritereClient.RUE, _CritereClient.PORTE, _CritereClient.LOT, _CritereClient.LONGITUDE, _CritereClient.LATITUDE);

                else if (_CritereClient.OptionRecherche == 3) // Compteur
                    dt = Galatee.Entity.Model.AccueilProcedures.RechercherClientCompteur(_CritereClient.FK_IDCENTRE, _CritereClient.CENTRE, _CritereClient.NUMCOMPTEUR, _CritereClient.PRODUIT);

                else if (_CritereClient.OptionRecherche == 4)
                    dt = Galatee.Entity.Model.AccueilProcedures.RechercherClientNumeroDeFacture(_CritereClient.PERIODE , _CritereClient.FACTURE);

                else if (_CritereClient.OptionRecherche  == 5)
                    dt = Galatee.Entity.Model.AccueilProcedures.RechercherClientAdresseElectrique(_CritereClient.FK_IDCENTRE, _CritereClient.CENTRE, _CritereClient.ADRESSEELECTRIQUE);


                return Entities.GetEntityListFromQuery<CsClientRechercher>(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CsClientRechercher> RechercherClientCompteur(CsClientRechercher _CritereClient)
        {
            try
            {
                DataTable dt = Galatee.Entity.Model.AccueilProcedures.RechercherClientCompteur(_CritereClient.FK_IDCENTRE , _CritereClient.CENTRE, _CritereClient.NUMCOMPTEUR, _CritereClient.PRODUIT);
                return Entities.GetEntityListFromQuery<CsClientRechercher>(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
       public List<CsClientRechercher> RechercherClientCompteur(string NumCompteur)
        {
            try
            {
                DataTable dt = Galatee.Entity.Model.AccueilProcedures.RechercherClientCompteur(NumCompteur);
                return Entities.GetEntityListFromQuery<CsClientRechercher>(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CsDemandeBase > RetourneListeDemandeModificationPourSuvie(string centre, string numdem, string LstTdem, string produit, DateTime? datedebut, string matricule)
        {
            try
            {
                List<CsDemandeBase> _lstDemande = new List<CsDemandeBase>();
                DataTable dt = Galatee.Entity.Model.AccueilProcedures.RetourneListeDemandeModificationPourSuvie(centre, numdem, LstTdem, produit, datedebut, matricule);
                _lstDemande = Entities.GetEntityListFromQuery<CsDemandeBase>(dt);
                return _lstDemande.OrderBy(t => t.NUMDEM).ToList();

            }
            catch (Exception ex)
            {
         
                throw ex;
            }
        }
        public bool  IsDernierEvtEnFacturation(string pCentre, string pClient, string pOrdre)
        {
            try
            {
                DataTable dt = Galatee.Entity.Model.AccueilProcedures.IsDernierEvtEnFacturation(pCentre, pClient, pOrdre);
                Entities.GetEntityListFromQuery<CsDemandeBase>(dt);
                if (Entities.GetEntityListFromQuery<CsDemandeBase>(dt) != null && Entities.GetEntityListFromQuery<CsDemandeBase>(dt).Count != 0)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

 
        public CsDemandeBase RetourneDemandeClientType(CsClient leClient)
        {
            cn = new SqlConnection(ConnectionString);
            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandTimeout = 1800;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SPX_ACC_CHARGERDEMANDECLIENTTYPE";
            if (cmd.Parameters != null && cmd.Parameters.Count != 0) cmd.Parameters.Clear();
            cmd.Parameters.Add("@Centre", SqlDbType.VarChar, 3).Value = leClient.CENTRE;
            cmd.Parameters.Add("@Client", SqlDbType.VarChar, 20).Value = leClient.REFCLIENT;
            cmd.Parameters.Add("@Ordre", SqlDbType.VarChar, 3).Value = leClient.ORDRE;
            cmd.Parameters.Add("@IdCentre", SqlDbType.Int).Value = leClient.FK_IDCENTRE;
            cmd.Parameters.Add("@Tdem", SqlDbType.VarChar, 3).Value = leClient.TYPEDEMANDE;
            cmd.Parameters.Add("@Periode", SqlDbType.VarChar, 6).Value = leClient.PERIODE;
            DBBase.SetDBNullParametre(cmd.Parameters);
            try
            {
                if (cn.State == ConnectionState.Closed)
                    cn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);
                return Entities.GetEntityFromQuery<CsDemandeBase>(dt);
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
        public int? RetourneTypeDemandeByCode(string pTdem)
        {
            try
            {
                return Galatee.Entity.Model.AccueilProcedures.RetourneTypeDemandeByCode(pTdem);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        
        
  
        public List<CsDemandeBase > RetourneListeDemandeModification(string centre, string numdem, string LstTdem,string produit, DateTime? datedebut,string matricule )
        {
            
            try
            {
                List<CsDemandeBase> _lstDemande = new List<CsDemandeBase>();
                DataTable  dt = Galatee.Entity.Model.AccueilProcedures.RetourneListeDemandeModification(centre, numdem, LstTdem,produit, datedebut,matricule);
                _lstDemande = Entities.GetEntityListFromQuery<CsDemandeBase>(dt);
                return _lstDemande.OrderBy(t => t.NUMDEM).ToList();
            }
            catch (Exception ex)
            {
             
                throw ex;
            }
        }
        public CsDemande RetourneDetailClientFromRefClient(string centre, string client, string ordre)
        {

            try
            {
                galadbEntities leContext = new galadbEntities();
                List<CsDemande> _LesDemande = new List<CsDemande>();

                CsDemande _LaDemande = new CsDemande();
                _LaDemande.LaDemande = new CsDemandeBase();

                DataTable dt = AccueilProcedures.RetourneAG(centre, client, ordre);
                _LaDemande.Ag = Entities.GetEntityFromQuery<CsAg>(dt);
                if (_LaDemande.LeClient != null)
                {
                    DataTable dtc = AccueilProcedures.RetourneClient(centre, client, ordre);
                    _LaDemande.LeClient = Entities.GetEntityFromQuery<CsClient>(dtc);

                    //string CodeTypeClient = leContext.TYPECLIENT.FirstOrDefault(t => t.PK_ID == _LaDemande.LeClient.FK_TYPECLIENT) != null ? leContext.TYPECLIENT.FirstOrDefault(t => t.PK_ID == _LaDemande.LeClient.FK_TYPECLIENT).CODE : string.Empty;
                    //switch (CodeTypeClient.Trim())
                    //{
                    //    case "001":
                    //        _LaDemande.LeClient.PersonePhysique = AccueilProcedures.RetourneInfoPersonePhysiqueDClient(_LaDemande.LeClient.PK_ID);
                    //        break;
                    //    case "002":
                    //        _LaDemande.LeClient.SocietePrives = AccueilProcedures.RetourneInfoSocietePriveDClient(_LaDemande.LeClient.PK_ID);
                    //        break;
                    //    case "003":
                    //        _LaDemande.LeClient.SocietePrives = AccueilProcedures.RetourneInfoSocietePriveDClient(_LaDemande.LeClient.PK_ID);
                    //        break;
                    //    default:

                    //        break;
                    //}
                }


                DataTable dtA = AccueilProcedures.RetourneAbon(centre, client, ordre);
                _LaDemande.Abonne = Entities.GetEntityFromQuery<CsAbon>(dtA);

                DataTable dtbrt = AccueilProcedures.RetourneBrt(centre, client, ordre);
                _LaDemande.Branchement = Entities.GetEntityFromQuery<CsBrt>(dtbrt);

                //DataTable dtcana = AccueilProcedures.Retournecanalisation(centre, client, ordre);
                //_LaDemande.LstCanalistion = Entities.GetEntityListFromQuery<CsCanalisation>(dtcana);

                //DataTable dtDetailCout = AccueilProcedures.RetourneDemandeDetailCout(centre, client, ordre);
                //_LaDemande.LstCoutDemande = Entities.GetEntityListFromQuery<CsDemandeDetailCout>(dtDetailCout);


                //DataTable dtevenet = AccueilProcedures.RetourneEvenement(centre, client, ordre);
                //_LaDemande.LstEvenement = Entities.GetEntityListFromQuery<CsEvenement>(dtevenet);


                //A voir avec abraham
                //DataTable dtDoc = DevisProcedures.DEVIS_DOCUMENTSCANNE_RETOURNEByIdDemande(_laDemande.PK_ID);
                //_LaDemande.ObjetScanne = Entities.GetEntityListFromQuery<ObjDOCUMENTSCANNE>(dtDoc);

                //A voir avec abraham
                //DataTable dtTdem = AccueilProcedures.RetourneDemandeTdem(_laDemande.FK_IDTDEM);
                //_LaDemande.LeTypeDemande = Entities.GetEntityFromQuery<CsTdem>(dtTdem);


                //DataTable dtParam = AccueilProcedures.RetourneTypeBranchementProduit(_laDemande.FK_IDPRODUIT.Value);
                //_LaDemande.LeTypeBranchement = Entities.GetEntityFromQuery<CsTypeBranchement>(dtParam);

                //DataTable dtElt = DevisProcedures.DEVIS_ELEMENTDEVISFourniture_SelByDevisById(_laDemande.PK_ID);
                //_LaDemande.EltDevis = Entities.GetEntityListFromQuery<ObjELEMENTDEVIS>(dtElt);

                //DataTable dtEltCou = DevisProcedures.DEVIS_ELEMENTDEVISCoutDemande_SelByDevisById(_laDemande.PK_ID);
                //List<ObjELEMENTDEVIS> LstEltDevis = Entities.GetEntityListFromQuery<ObjELEMENTDEVIS>(dtEltCou);

                //if (LstEltDevis != null && LstEltDevis.Count != 0)
                //{
                //    if (_LaDemande.EltDevis != null)
                //        _LaDemande.EltDevis.AddRange(LstEltDevis);
                //    else _LaDemande.EltDevis = LstEltDevis;
                //}
                //DataTable dttrv = DevisProcedures.DEVIS_TRAVAUXDEVIS_RETOURNEByDevisId(_laDemande.PK_ID);
                //_LaDemande.TravauxDevis = Entities.GetEntityFromQuery<ObjTRAVAUXDEVIS>(dttrv);

                //DataTable dtAppDevis = DevisProcedures.DEVIS_APPAREILSDEVIS_RETOURNEByDevisId(_laDemande.PK_ID);
                //_LaDemande.AppareilDevis = Entities.GetEntityListFromQuery<ObjAPPAREILSDEVIS>(dtAppDevis);

                leContext.Dispose();
                return _LaDemande;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        //public CsDemande RetourneDetailClientFromRefClient(int fk_idcentre, string centre, string client, string ordre)
        //{

        //    try
        //    {
        //        galadbEntities leContext = new galadbEntities();
        //        List<CsDemande> _LesDemande = new List<CsDemande>();

        //        CsDemande _LaDemande = new CsDemande();
        //        _LaDemande.LaDemande = new CsDemandeBase();

        //        DataTable dt = AccueilProcedures.RetourneAG(fk_idcentre, centre, client, ordre);
        //        _LaDemande.Ag = Entities.GetEntityFromQuery<CsAg>(dt);
        //        //if (_LaDemande.LeClient != null)
        //        //{
        //        DataTable dtc = AccueilProcedures.RetourneClient(fk_idcentre, centre, client, ordre);
        //        _LaDemande.LeClient = Entities.GetEntityFromQuery<CsClient>(dtc);

        //        //string CodeTypeClient = leContext.TYPECLIENT.FirstOrDefault(t => t.PK_ID == _LaDemande.LeClient.FK_TYPECLIENT) != null ? leContext.TYPECLIENT.FirstOrDefault(t => t.PK_ID == _LaDemande.LeClient.FK_TYPECLIENT).CODE : string.Empty;
        //        //switch (CodeTypeClient.Trim())
        //        //{
        //        //    case "001":
        //        //        _LaDemande.LeClient.PersonePhysique = AccueilProcedures.RetourneInfoPersonePhysiqueDClient(_LaDemande.LeClient.PK_ID);
        //        //        break;
        //        //    case "002":
        //        //        _LaDemande.LeClient.SocietePrives = AccueilProcedures.RetourneInfoSocietePriveDClient(_LaDemande.LeClient.PK_ID);
        //        //        break;
        //        //    case "003":
        //        //        _LaDemande.LeClient.AdministrationInstitut = AccueilProcedures.RetourneInfoAdministrationInstitutDClient(_LaDemande.LeClient.PK_ID);
        //        //        break;
        //        //    default:

        //        //        break;
        //        //}
        //        //}


        //        DataTable dtA = AccueilProcedures.RetourneAbon(fk_idcentre, centre, client, ordre);
        //        _LaDemande.Abonne = Entities.GetEntityFromQuery<CsAbon>(dtA);

        //        DataTable dtbrt = AccueilProcedures.RetourneBRT(fk_idcentre, centre, client, ordre);
        //        _LaDemande.Branchement = Entities.GetEntityFromQuery<CsBrt>(dtbrt);

        //        DataTable dtcana = AccueilProcedures.RetourneCanalisation(fk_idcentre, centre, client, string.Empty, null);
        //        _LaDemande.LstCanalistion = Entities.GetEntityListFromQuery<CsCanalisation>(dtcana);

        //        //DataTable dtDetailCout = AccueilProcedures.RetourneDemandeDetailCout(centre, client, ordre);
        //        //_LaDemande.LstCoutDemande = Entities.GetEntityListFromQuery<CsDemandeDetailCout>(dtDetailCout);


        //        DataTable dtevenet = AccueilProcedures.RetourneEvenement(fk_idcentre, centre, client, ordre, string.Empty, null, null);
        //        _LaDemande.LstEvenement = Entities.GetEntityListFromQuery<CsEvenement>(dtevenet);


        //        //A voir avec abraham
        //        //DataTable dtDoc = DevisProcedures.DEVIS_DOCUMENTSCANNE_RETOURNEByIdDemande(_laDemande.PK_ID);
        //        //_LaDemande.ObjetScanne = Entities.GetEntityListFromQuery<ObjDOCUMENTSCANNE>(dtDoc);

        //        ////A voir avec abraham
        //        //DataTable dtTdem = AccueilProcedures.RetourneDemandeTdem(_laDemande.FK_IDTDEM);
        //        //_LaDemande.LeTypeDemande = Entities.GetEntityFromQuery<CsTdem>(dtTdem);


        //        //DataTable dtParam = AccueilProcedures.RetourneTypeBranchementProduit(_laDemande.FK_IDPRODUIT.Value);
        //        //_LaDemande.LeTypeBranchement = Entities.GetEntityFromQuery<CsTypeBranchement>(dtParam);

        //        //DataTable dtElt = DevisProcedures.DEVIS_ELEMENTDEVISFourniture_SelByDevisById(_laDemande.PK_ID);
        //        //_LaDemande.EltDevis = Entities.GetEntityListFromQuery<ObjELEMENTDEVIS>(dtElt);

        //        //DataTable dtEltCou = DevisProcedures.DEVIS_ELEMENTDEVISCoutDemande_SelByDevisById(_laDemande.PK_ID);
        //        //List<ObjELEMENTDEVIS> LstEltDevis = Entities.GetEntityListFromQuery<ObjELEMENTDEVIS>(dtEltCou);

        //        //if (LstEltDevis != null && LstEltDevis.Count != 0)
        //        //{
        //        //    if (_LaDemande.EltDevis != null)
        //        //        _LaDemande.EltDevis.AddRange(LstEltDevis);
        //        //    else _LaDemande.EltDevis = LstEltDevis;
        //        //}
        //        //DataTable dttrv = DevisProcedures.DEVIS_TRAVAUXDEVIS_RETOURNEByDevisId(_laDemande.PK_ID);
        //        //_LaDemande.TravauxDevis = Entities.GetEntityFromQuery<ObjTRAVAUXDEVIS>(dttrv);

        //        //DataTable dtAppDevis = DevisProcedures.DEVIS_APPAREILSDEVIS_RETOURNEByDevisId(centre, client, ordre);
        //        //_LaDemande.AppareilDevis = Entities.GetEntityListFromQuery<ObjAPPAREILSDEVIS>(dtAppDevis);

        //        leContext.Dispose();
        //        return _LaDemande;
        //    }
        //    catch (Exception ex)
        //    {

        //        throw ex;
        //    }
        //}

        public CsDemande RetourneDetailClientFromRefClient(int fk_idcentre, string centre, string client, string ordre, string produit,string Typedemande)
        {

            try
            {
                galadbEntities leContext = new galadbEntities();
                CsDemande _LaDemande = new CsDemande();

                DataTable dt = AccueilProcedures.RetourneAG(fk_idcentre, centre, client, ordre);
                _LaDemande.Ag = Entities.GetEntityFromQuery<CsAg>(dt);
               
                DataTable dtc = AccueilProcedures.RetourneClient(fk_idcentre, centre, client, ordre);
                _LaDemande.LeClient = Entities.GetEntityFromQuery<CsClient>(dtc);

                if (_LaDemande.LeClient != null && !string.IsNullOrEmpty(_LaDemande.LeClient.NOMABON))
                {
                    DataTable dc = AccueilProcedures.RetourneSociete(_LaDemande.LeClient.PK_ID );
                    _LaDemande.SocietePrives  = Entities.GetEntityFromQuery<CsSocietePrive >(dc);

                    DataTable c = AccueilProcedures.RetourneAdministration(_LaDemande.LeClient.PK_ID);
                    _LaDemande.AdministrationInstitut  = Entities.GetEntityFromQuery<CsAdministration_Institut>(c);

                    DataTable u = AccueilProcedures.RetournePersonnePhysique (_LaDemande.LeClient.PK_ID);
                    _LaDemande.PersonePhysique  = Entities.GetEntityFromQuery<CsPersonePhysique>(u);
                }

                DataTable dtA = AccueilProcedures.RetourneAbon(fk_idcentre, centre, client, ordre);
                _LaDemande.Abonne = Entities.GetEntityFromQuery<CsAbon>(dtA);
                //var resul=AccueilProcedures.RetourneFactureAvanceFromAbon(fk_idcentre, client, ordre);
                //if (resul != null && resul.Count() > 0)
                //    _LaDemande.Abonne.AVANCE = resul.FirstOrDefault().MONTANT;
                //else
                //    _LaDemande.Abonne.AVANCE = 0;
                DataTable dtbrt = AccueilProcedures.RetourneBRT(fk_idcentre, centre, client, produit);
                _LaDemande.Branchement = Entities.GetEntityFromQuery<CsBrt>(dtbrt);

                if (Typedemande == Enumere.TransfertAbonnement)
                {
                    DataTable dtcana = AccueilProcedures.RetourneCanalisationClasseur(fk_idcentre, centre, client,_LaDemande.Abonne.PK_ID, produit, null);
                    List<CsCanalisation> lstCanalisation = Entities.GetEntityListFromQuery<CsCanalisation>(dtcana);
                    if (lstCanalisation != null && lstCanalisation.Count != 0)
                    {
                            _LaDemande.LstCanalistion = new List<CsCanalisation>();
                            CsCanalisation leCompteurActif = lstCanalisation.FirstOrDefault(o => o.DEPOSE == null);
                            if (leCompteurActif != null) _LaDemande.LstCanalistion.Add(leCompteurActif);
                            else _LaDemande.LstCanalistion.Add(lstCanalisation.LastOrDefault());
                            DataTable dtevenet = AccueilProcedures.RetourneEvenementDuCompteur(_LaDemande.LstCanalistion.FirstOrDefault().PK_ID);
                            _LaDemande.LstEvenement = Entities.GetEntityListFromQuery<CsEvenement>(dtevenet);

                    }
                }
                if (Typedemande == Enumere.ChangementCompteur ||
                    Typedemande == Enumere.Etalonage ||
                    Typedemande == Enumere.AugmentationPuissance ||
                    Typedemande == Enumere.DepannageClient ||
                    Typedemande == Enumere.DepannagePrepayer  ||
                    Typedemande == Enumere.DimunitionPuissance ||
                    Typedemande == Enumere.ChangementProduit  ||
                    Typedemande == Enumere.VerificationCompteur )
                {
                    DataTable dtcana = AccueilProcedures.RetourneCanalisation(fk_idcentre, centre, client, produit, null);
                    _LaDemande.LstCanalistion = Entities.GetEntityListFromQuery<CsCanalisation>(dtcana);
                }
                if (Typedemande == Enumere.ModificationCompteur ||
                    Typedemande == Enumere.CorrectionDeDonnes )
                {
                    DataTable dtcana = AccueilProcedures.RetourneCanalisation(fk_idcentre, centre, client, produit, null);
                    _LaDemande.LstCanalistion = Entities.GetEntityListFromQuery<CsCanalisation>(dtcana);
                }
                if (Typedemande == Enumere.Reabonnement  )
                {
                    DataTable dtcana = AccueilProcedures.RetourneCanalisationResilier(fk_idcentre, centre, client, produit);
                    _LaDemande.LstCanalistion = Entities.GetEntityListFromQuery<CsCanalisation>(dtcana);
                }
                //A voir avec abraham
                //DataTable dtDoc = DevisProcedures.DEVIS_DOCUMENTSCANNE_RETOURNEByIdDemande(_laDemande.PK_ID);
                //_LaDemande.ObjetScanne = Entities.GetEntityListFromQuery<ObjDOCUMENTSCANNE>(dtDoc);

                leContext.Dispose();
                return _LaDemande;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<CsLclient> ChargerFraisParticipation(CsClient leClient)
        {
            List<CsLclient> lstFraisParticipation = new List<CsLclient>();
            galadbEntities _LeContextInter = new galadbEntities();
            List<TRANSCAISB> lesFrais = _LeContextInter.TRANSCAISB.Where (t => t.CENTRE == leClient.CENTRE && t.CLIENT == leClient.REFCLIENT && t.ORDRE ==leClient.ORDRE && t.COPER == Enumere.CoperFAB ).ToList();
            foreach (var item in lesFrais)
            {
                 decimal? SoldeFacture = FonctionCaisse.RetourneSoldeDocument(item.FK_IDCENTRE ,item.CENTRE ,item.CLIENT ,item.ORDRE ,item.NDOC ,item.REFEM );
                if (SoldeFacture<0)
                  lstFraisParticipation.Add(Galatee.Tools.Utility.ConvertEntity<CsLclient,TRANSCAISB>(item));      
            }
            return lstFraisParticipation;
        }




/*
        public CsLclient Select_Avance(int IdCentre, string Centre, string Client, string Ordre)
        {
            cn = new SqlConnection(ConnectionString);

            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandTimeout = 1800;
            cmd.CommandType = CommandType.StoredProcedure;
            if (cmd.Parameters != null && cmd.Parameters.Count != 0) cmd.Parameters.Clear();
            cmd.CommandText = "SPX_ACC_COMPTEDERESILIATION";
            cmd.Parameters.Add("@FK_IDCENTRE", SqlDbType.Int).Value = IdCentre;
            cmd.Parameters.Add("@CENTRE", SqlDbType.VarChar, 3).Value = Centre;
            cmd.Parameters.Add("@CLIENT", SqlDbType.VarChar, 20).Value = Client;
            cmd.Parameters.Add("@ORDRE", SqlDbType.VarChar, 2).Value = Ordre;

            DBBase.SetDBNullParametre(cmd.Parameters);
            try
            {
                if (cn.State == ConnectionState.Closed)
                    cn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);
                return Entities.GetEntityFromQuery<CsLclient>(dt);
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


        public List<CsLclient> ChargerCompteDeResiliation(CsClient leClient)
        {
            try
            {
                List<CsLclient> lstFacture = new List<CsLclient>();
                lstFacture = new DBEncaissement().ListeFactureNonSolde(leClient.CENTRE, leClient.REFCLIENT, leClient.ORDRE, leClient.FK_IDCENTRE.Value);
                lstFacture.Add(Select_Avance(leClient.FK_IDCENTRE.Value, leClient.CENTRE, leClient.REFCLIENT, leClient.ORDRE));
                return lstFacture;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

*/

        //public List<CsLclient> ChargerCompteDeResiliation(CsClient  leClient)
        //{
        //    DBEncaissement db = new DBEncaissement();
        //    List<CsClient> lstClientReference = db.TestClientExist(leClient.CENTRE, leClient.REFCLIENT, leClient.ORDRE);

        //    CsClient leClientRec = lstClientReference.FirstOrDefault(t => t.FK_IDCENTRE == leClient.FK_IDCENTRE);
        //    List<CsLclient> lstFactiure = new DBEncaissement().RetourneListeFactureNonSolde(leClientRec.CENTRE, leClientRec.REFCLIENT, leClientRec.ORDRE, leClientRec.PK_ID);
        //    //List<CsLclient> lstFactureNegative = new DBEncaissement().RetourneListeFactureSoldeNegatif(leClientRec.CENTRE, leClientRec.REFCLIENT, leClientRec.ORDRE, leClientRec.PK_ID);
        //    //if (lstFactureNegative != null && lstFactureNegative.Count != 0)  
        //    //   lstFactiure.AddRange(lstFactureNegative);

        //    DataTable dtevenet = AccueilProcedures.RetourneAvanceFromAbon(leClient);
        //    CsLclient lstFactiureAvc = Galatee.Tools.Utility.GetEntityListFromQuery<CsLclient>(dtevenet).FirstOrDefault();
        //    //if (lstFactiureAvc.DENR != new DateTime() && lstFactiureAvc.SOLDEFACTURE != 0 && lstFactiureAvc.SOLDEFACTURE != null)
        //    //{
        //        DataTable dte = AccueilProcedures.RetourneFactureAvanceFromAbon(lstFactiureAvc.FK_IDCENTRE, lstFactiureAvc.CENTRE, lstFactiureAvc.CLIENT, lstFactiureAvc.ORDRE, lstFactiureAvc.DENR, lstFactiureAvc.MONTANT.Value);

        //  /*
        //    CsLclient lstFactiureAvance = Galatee.Tools.Utility.GetEntityFromQuery<CsLclient>(dte).FirstOrDefault();
        //            if (lstFactiureAvance == null)
        //                return null;
                
        //            galadbEntities _LeContextInter = new galadbEntities();
        //            COPER leCoperRemb = _LeContextInter.COPER.FirstOrDefault(t => t.CODE == Enumere.CoperRAC);
        //            lstFactiureAvc.REFEM = lstFactiureAvc.DENR.Year.ToString() + lstFactiureAvc.DENR.Month.ToString("00");
        //            lstFactiureAvc.EXIGIBILITE = lstFactiureAvc.DENR;
        //            lstFactiureAvc.LIBELLENATURE = leCoperRemb.LIBCOURT;
        //            lstFactiureAvc.COPER = leCoperRemb.CODE;
        //            lstFactiureAvc.LIBELLECOPER = leCoperRemb.LIBELLE;
        //            lstFactiureAvc.FK_IDCOPER = leCoperRemb.PK_ID;
        //            if (lstFactiureAvance != null)
        //            {
        //                lstFactiureAvc.NDOC = lstFactiureAvance.NDOC;
        //                //lstFactiureAvance.SOLDECLIENT = (lstFactiure != null && lstFactiure.Count != 0) ? lstFactiure.First().SOLDECLIENT : (-1) * lstFactiureAvance.SOLDEFACTURE;
        //                lstFactiureAvance.SOLDECLIENT = (-1) * lstFactiureAvance.SOLDEFACTURE;
        //                lstFactiureAvc.MONTANT = lstFactiureAvance.MONTANT;
        //                lstFactiureAvc.SOLDECLIENT = lstFactiureAvance.SOLDECLIENT;
        //                lstFactiureAvc.SOLDEFACTURE = lstFactiureAvance.SOLDEFACTURE;
        //                lstFactiureAvc.REFEM = lstFactiureAvance.REFEM;
        //            }
        //            //else
        //            //    lstFactiureAvc.NDOC = NumeroFacture(leClientRec.FK_IDCENTRE.Value);
        //            lstFactiure.Add(lstFactiureAvc);
        //        //}
        //        lstFactiure.ForEach(t => t.FK_IDCLIENT = leClientRec.PK_ID);

        //      //return   lstFactiure;
        //        List<CsLclient> myresulttosend = new List<CsLclient>();
        //        myresulttosend.Add(lstFactiureAvance);
        //        return myresulttosend;
                 
                 
        //      */

            

        //        List<CsLclient> lstFactiureAvance = Galatee.Tools.Utility.GetEntityListFromQuery<CsLclient>(dte);
        //        if (lstFactiureAvance == null)
        //            return null;

        //        galadbEntities _LeContextInter = new galadbEntities();
        //        COPER leCoperRemb = _LeContextInter.COPER.FirstOrDefault(t => t.CODE == Enumere.CoperRAC);
        //        lstFactiureAvc.REFEM = lstFactiureAvc.DENR.Year.ToString() + lstFactiureAvc.DENR.Month.ToString("00");
        //        lstFactiureAvc.EXIGIBILITE = lstFactiureAvc.DENR;
        //        lstFactiureAvc.LIBELLENATURE = leCoperRemb.LIBCOURT;
        //        lstFactiureAvc.COPER = leCoperRemb.CODE;
        //        lstFactiureAvc.LIBELLECOPER = leCoperRemb.LIBELLE;
        //        lstFactiureAvc.FK_IDCOPER = leCoperRemb.PK_ID;
        //        if (lstFactiureAvance != null && lstFactiureAvance.Count > 0)
        //        {

        //            lstFactiureAvance.ForEach(t => t.EXIGIBILITE = t.DENR);
        //            lstFactiureAvance.ForEach(t => t.LIBELLENATURE = lstFactiureAvc.LIBELLENATURE);
        //            lstFactiureAvance.ForEach(t => t.COPER = lstFactiureAvc.COPER);
        //            lstFactiureAvance.ForEach(t => t.LIBELLECOPER = lstFactiureAvc.LIBELLECOPER);
        //            lstFactiureAvance.ForEach(t => t.FK_IDCOPER = lstFactiureAvc.FK_IDCOPER);
        //            lstFactiureAvance.ForEach(t => t.SOLDECLIENT = (-1) * t.SOLDEFACTURE);
        //        }


        //        return lstFactiureAvance;
            
        //}





        public  CsLclient  ChargerAvanceSurConsommation(CsClient leClient)
        {
                DBEncaissement db = new DBEncaissement();
                DataTable dtevenet = AccueilProcedures.RetourneAvance(leClient);
                CsLclient lstFactiureAvc = Galatee.Tools.Utility.GetEntityFromQuery<CsLclient>(dtevenet).FirstOrDefault();
                return lstFactiureAvc;
        }
   

 
        public CsDemande RetourneDetailDemandeFromDEvis(CsDemandeBase _laDemande)
        {

            SqlConnection laConnection = DBBase.InitConnection(ConnectionString);
            try
            {
                galadbEntities leContext = new galadbEntities();
                List<CsDemande> _LesDemande = new List<CsDemande>();

                CsDemande _LaDemande = new CsDemande();
                _LaDemande.LaDemande = _laDemande;
                if (_LaDemande.LaDemande == null)
                    new ErrorManager().LogException(this, new Exception("1"));

                DataTable dt = AccueilProcedures.RetourneDAG(_laDemande.PK_ID );
                _LaDemande.Ag = Entities.GetEntityFromQuery<CsAg>(dt);

                if (_LaDemande.Ag == null)
                    new ErrorManager().LogException(this, new Exception("2"));

                DataTable dtc = AccueilProcedures.RetourneDClient(_laDemande.PK_ID);
                _LaDemande.LeClient = Entities.GetEntityFromQuery<CsClient>(dtc);
                if (_LaDemande.LeClient == null)
                    new ErrorManager().LogException(this, new Exception("3"));



                DataTable dtss = AccueilProcedures.RetourneDSociete(_laDemande.PK_ID);
                _LaDemande.SocietePrives  = Entities.GetEntityFromQuery<CsSocietePrive>(dtss);
                if (_LaDemande.SocietePrives == null)
                    new ErrorManager().LogException(this, new Exception("4"));




                DataTable dtsst = AccueilProcedures.RetourneDAdministration(_laDemande.PK_ID);
                _LaDemande.AdministrationInstitut  = Entities.GetEntityFromQuery<CsAdministration_Institut>(dtsst);
                if (_LaDemande.AdministrationInstitut == null)
                    new ErrorManager().LogException(this, new Exception("5"));



                DataTable d = AccueilProcedures.RetourneDPersonnePhysique(_laDemande.PK_ID);
                _LaDemande.PersonePhysique  = Entities.GetEntityFromQuery<CsPersonePhysique >(d);
                if (_LaDemande.PersonePhysique == null)
                    new ErrorManager().LogException(this, new Exception("6"));


                DataTable a = AccueilProcedures.RetourneDinfoPropriotaire(_laDemande.PK_ID);
                _LaDemande.InfoProprietaire_  = Entities.GetEntityFromQuery<CsInfoProprietaire>(a);
                if (_LaDemande.InfoProprietaire_ == null)
                    new ErrorManager().LogException(this, new Exception("7"));



                DataTable dtA = AccueilProcedures.RetourneDAbon(_laDemande.PK_ID);
                _LaDemande.Abonne = Entities.GetEntityFromQuery<CsAbon>(dtA);
                if (_LaDemande.Abonne == null)
                    new ErrorManager().LogException(this, new Exception("8"));



                DataTable dtbrt = AccueilProcedures.RetourneDBrt(_laDemande.PK_ID);
                _LaDemande.Branchement = Entities.GetEntityFromQuery<CsBrt>(dtbrt);
                if (_LaDemande.Branchement == null)
                    new ErrorManager().LogException(this, new Exception("9"));



                if (_laDemande.TYPEDEMANDE == Enumere.BranchementAbonement ||
                    _laDemande.TYPEDEMANDE == Enumere.BranchementAbonnementEp  ||
                    _laDemande.TYPEDEMANDE == Enumere.AbonnementSeul ||
                    _laDemande.TYPEDEMANDE == Enumere.BranchementAbonnementMt ||
                    _laDemande.TYPEDEMANDE == Enumere.BranchementAbonementExtention  ||
                    _laDemande.TYPEDEMANDE == Enumere.ChangementProduit  ||
                    _laDemande.TYPEDEMANDE == Enumere.Reabonnement ||
                    _laDemande.TYPEDEMANDE == Enumere.ChangementCompteur  ||
                    ((_laDemande.TYPEDEMANDE == Enumere.AugmentationPuissance ||_laDemande.TYPEDEMANDE == Enumere.DimunitionPuissance) && 
                    (_laDemande.ISCHANGECOMPTEUR == true || _laDemande.ISMETREAFAIRE== true )))
                {
                                    DataTable dtcanal = AccueilProcedures.RetourneDcanalisationMagazinVirtuel(_laDemande.PK_ID);
                    _LaDemande.LstCanalistion = Entities.GetEntityListFromQuery<CsCanalisation>(dtcanal);
                    if (_LaDemande.LstCanalistion == null)
                        new ErrorManager().LogException(this, new Exception("10"));
                }
                else
                {
                    if (_laDemande.TYPEDEMANDE != Enumere.DepannageClient &&
                    _laDemande.TYPEDEMANDE != Enumere.DepannagePrepayer &&
                    _laDemande.TYPEDEMANDE != Enumere.DepannageMT &&
                    _laDemande.TYPEDEMANDE != Enumere.DepannageMaintenance)
                    {
                        if (_laDemande.TYPEDEMANDE != Enumere.ModificationCompteur &&
                            _laDemande.TYPEDEMANDE != Enumere.TransfertSiteNonMigre )
                        {
                            DataTable dtcana = AccueilProcedures.RetourneDcanalisationCompteur(_laDemande.PK_ID);
                            _LaDemande.LstCanalistion = Entities.GetEntityListFromQuery<CsCanalisation>(dtcana);
                            if (_LaDemande.LstCanalistion == null)
                                new ErrorManager().LogException(this, new Exception("11"));
                        }
                        else
                        {
                            DataTable dtcana_ = AccueilProcedures.RetourneDCompteur(_laDemande.PK_ID);
                            _LaDemande.LstCanalistion = Entities.GetEntityListFromQuery<CsCanalisation>(dtcana_);
                            _LaDemande.LstCanalistion.ForEach(u => u.NUMDEM = _laDemande.NUMDEM);
                            if (_LaDemande.LstCanalistion == null)
                                new ErrorManager().LogException(this, new Exception("12"));
                        }
                    }
                }
                

                DataTable dtDetailCout = AccueilProcedures.RetourneDemandeDetailCout(_laDemande.PK_ID);
                _LaDemande.LstCoutDemande = Entities.GetEntityListFromQuery<CsDemandeDetailCout>(dtDetailCout);
                if (_LaDemande.LstCoutDemande == null)
                    new ErrorManager().LogException(this, new Exception("13"));

                DataTable dtevenet = AccueilProcedures.RetourneDEvenement(_laDemande.PK_ID);
                _LaDemande.LstEvenement = Entities.GetEntityListFromQuery<CsEvenement>(dtevenet);
                if (_LaDemande.LstEvenement == null)
                    new ErrorManager().LogException(this, new Exception("14"));
                if (_laDemande.TYPEDEMANDE == Enumere.Resiliation && (_LaDemande.LstEvenement != null && _LaDemande.LstEvenement.Count == 0))
                {
                    List<CsCanalisation> lstCanalisation = RetourneCanalisation(_laDemande.FK_IDCENTRE, _laDemande.CENTRE, _laDemande.CLIENT,_laDemande.PRODUIT, null);
                    _LaDemande.LstCanalistion = lstCanalisation;
                    if (_LaDemande.LstCanalistion == null)
                        new ErrorManager().LogException(this, new Exception("15"));
                    CsAbon leAbon = new DBAccueil().RetourneAbon(_laDemande.FK_IDCENTRE, _laDemande.CENTRE, _laDemande.CLIENT, _laDemande.ORDRE, _laDemande.PRODUIT);
                    if (leAbon == null)
                        new ErrorManager().LogException(this, new Exception("16"));
                    if (leAbon != null && !string.IsNullOrEmpty(leAbon.CLIENT))
                    {
                        _LaDemande.LeClient.FK_IDABON = leAbon.PK_ID;
                        _LaDemande.LeClient.PRODUIT = leAbon.PRODUIT;
                        _LaDemande.LeClient.FK_IDPRODUIT = leAbon.FK_IDPRODUIT;
                    }
                    List<CsEvenement> lstEvt = new DbFacturation().RetourneEvenementSpx(_LaDemande.LeClient.FK_IDCENTRE.Value, _LaDemande.LeClient.FK_IDABON.Value, _LaDemande.LeClient.FK_IDPRODUIT.Value, _LaDemande.LeClient.PERIODE);
                    if (lstEvt.Count != 0)
                    {
                        lstEvt.ForEach(y=>y.CASEVENEMENT = "000");
                        _LaDemande.LstEvenement = lstEvt;
                        if (_LaDemande.LstEvenement == null)
                            new ErrorManager().LogException(this, new Exception("17"));
                    }
                }

                DataTable dtDoc = DevisProcedures.DEVIS_DOCUMENTSCANNE_RETOURNEByIdDemande(_laDemande.PK_ID);
                _LaDemande.ObjetScanne = Entities.GetEntityListFromQuery<ObjDOCUMENTSCANNE>(dtDoc);

                DataTable dtElt = DevisProcedures.DEVIS_ELEMENTDEVIS_MaterielByDevisById(_laDemande.PK_ID);
                _LaDemande.EltDevis = Entities.GetEntityListFromQuery<ObjELEMENTDEVIS>(dtElt);

                DataTable dtEltCou = DevisProcedures.DEVIS_ELEMENTDEVISCoutDemande_SelByDevisById(_laDemande.PK_ID);
                List<ObjELEMENTDEVIS> LstEltDevis = Entities.GetEntityListFromQuery<ObjELEMENTDEVIS>(dtEltCou);

                if (LstEltDevis != null && LstEltDevis.Count != 0)
                {
                    if (_LaDemande.EltDevis != null)
                        _LaDemande.EltDevis.AddRange(LstEltDevis);
                    else _LaDemande.EltDevis = LstEltDevis;
                }
                new ErrorManager().LogException(this, new Exception("18"));
                DataTable dtEltTimbre = AccueilProcedures.RetourneDemandeAchatTimbre(_laDemande.PK_ID);
                _LaDemande.LstEltTimbre = Entities.GetEntityListFromQuery<CsElementAchatTimbre>(dtEltTimbre);

                DataTable dttrv = DevisProcedures.DEVIS_TRAVAUXDEVIS_RETOURNEByDevisId(_laDemande.PK_ID);
                _LaDemande.TravauxDevis = Entities.GetEntityFromQuery<ObjTRAVAUXDEVIS>(dttrv);

                DataTable dtAppDevis = DevisProcedures.DEVIS_APPAREILSDEVIS_RETOURNEByDevisId(_laDemande.PK_ID);
                _LaDemande.AppareilDevis = Entities.GetEntityListFromQuery<ObjAPPAREILSDEVIS>(dtAppDevis);

                new ErrorManager().LogException(this, new Exception("19"));
                DataTable dtparticipation = DevisProcedures.DEVIS_PARTICIPATIONDevisId(_laDemande.PK_ID);
                _LaDemande.LstFraixParticipation = Entities.GetEntityListFromQuery<CsFraixParticipation>(dtparticipation);

                _LaDemande.InfoDemande = new DB_WORKFLOW().RecupererInfoDemandeParCodeTDem(_laDemande, laConnection);
                _LaDemande.LstCommentaire = new DB_WORKFLOW().SelectCommentaireRejet(_laDemande.NUMDEM, laConnection);


                DataTable dtOt = AccueilProcedures.ChargerOrdreDeTravail(_laDemande.PK_ID);
                _LaDemande.OrdreTravail = Entities.GetEntityFromQuery<CsOrdreTravail>(dtOt);

                DataTable dtControlTvx = AccueilProcedures.ChargerControleTravaux(_laDemande.PK_ID);
                _LaDemande.LstControleTvx = Entities.GetEntityListFromQuery<CsControleTravaux>(dtControlTvx);
                new ErrorManager().LogException(this, new Exception("20"));
                //DataTable dtAnnotation = AccueilProcedures.ChargerAnnotationDemande(_laDemande.PK_ID);
                //_LaDemande.AnnotationDemande = Entities.GetEntityFromQuery<CsAnnotation>(dtAnnotation);

                _LaDemande.Programmation = RetoureDetailProgramme(_laDemande.PK_ID, laConnection);
                if (_LaDemande.Programmation == null)
                    new ErrorManager().LogException(this, new Exception("21"));
                if (_laDemande.TYPEDEMANDE == Enumere.DepannageEp ||
                    _laDemande.TYPEDEMANDE == Enumere.DepannageMaintenance  ||
                    _laDemande.TYPEDEMANDE == Enumere.DepannageMT ||
                    _laDemande.TYPEDEMANDE == Enumere.DepannagePrepayer ||
                    _laDemande.TYPEDEMANDE == Enumere.DepannageClient)
                {
                    DataTable dtDepannage = AccueilProcedures.ChargerDemandeDepanage(_laDemande.PK_ID);
                    _LaDemande.Depannage = Entities.GetEntityFromQuery<CsDepannage>(dtDepannage);
                }

                if (_laDemande.TYPEDEMANDE == Enumere.TransfertAbonnement)
                {
                    DataTable dttransfert = AccueilProcedures.ChargerDemandeTransfert(_laDemande.PK_ID);
                    _LaDemande.Transfert = Entities.GetEntityFromQuery<CsDtransfert>(dttransfert);
                }
                new ErrorManager().LogException(this, new Exception("22"));

                leContext.Dispose();
                return _LaDemande;
            }
            catch (Exception ex)
            {
                new ErrorManager().LogException(this, ex);
                throw ex;
            }
            finally
            {
                if (laConnection.State == ConnectionState.Open)
                    laConnection.Close(); // Fermeture de la connection 
                laConnection.Dispose();
            }
        }

        public CsDemande RetourneDetailDemande(CsDemandeBase _laDemande)
        {
            
            
            try
            {
                galadbEntities leContext = new galadbEntities();
                List<CsDemande> _LesDemande = new List<CsDemande>();
                DEMANDE _LaDemandeCritere = Galatee.Entity.Model.AccueilProcedures.RetourneDetailDemande(_laDemande.PK_ID , leContext);
                if (_LaDemandeCritere == null) return null;


                    CsDemande _LaDemande = new CsDemande();
                    _LaDemande.LaDemande = Entities.ConvertObject<CsDemandeBase, DEMANDE>(_LaDemandeCritere);
                    _LaDemande.LaDemande.SITE = _LaDemandeCritere.CENTRE1!= null ? _LaDemandeCritere.CENTRE1.CODESITE  : string.Empty;
                    _LaDemande.LaDemande.LIBELLESITE = _LaDemandeCritere.CENTRE1.SITE != null ? _LaDemandeCritere.CENTRE1.SITE.LIBELLE : string.Empty;
                    _LaDemande.LaDemande.LIBELLECENTRE  = _LaDemandeCritere.CENTRE1 != null ? _LaDemandeCritere.CENTRE1.LIBELLE  : string.Empty;


                    if (_LaDemandeCritere.DAG != null && _LaDemandeCritere.DAG.Count != 0)
                    {
                        _LaDemande.Ag = Entities.ConvertObject<CsAg, DAG>(_LaDemandeCritere.DAG.FirstOrDefault());
                        _LaDemande.Ag.LIBELLECOMMUNE = _LaDemandeCritere.DAG.FirstOrDefault().COMMUNE1 != null ? _LaDemandeCritere.DAG.FirstOrDefault().COMMUNE1.LIBELLE : string.Empty;
                        _LaDemande.Ag.LIBELLEQUARTIER  = _LaDemandeCritere.DAG.FirstOrDefault().QUARTIER1  != null ? _LaDemandeCritere.DAG.FirstOrDefault().QUARTIER1 .LIBELLE : string.Empty;
                        _LaDemande.Ag.LIBELLERUE = _LaDemandeCritere.DAG.FirstOrDefault().RUES != null ? _LaDemandeCritere.DAG.FirstOrDefault().RUES.LIBELLE : string.Empty;
                        _LaDemande.Ag.LIBELLESECTEUR  = _LaDemandeCritere.DAG.FirstOrDefault().SECTEUR1 != null ? _LaDemandeCritere.DAG.FirstOrDefault().SECTEUR1.LIBELLE : string.Empty;
                    }


                    if (_LaDemandeCritere.DCLIENT != null && _LaDemandeCritere.DCLIENT.Count != 0)
                    {
                        _LaDemande.LeClient = Entities.ConvertObject<CsClient, DCLIENT>(_LaDemandeCritere.DCLIENT.FirstOrDefault());
                        _LaDemande.LeClient.LIBELLECATEGORIE = _LaDemandeCritere.DCLIENT.FirstOrDefault().CATEGORIECLIENT != null ? _LaDemandeCritere.DCLIENT.FirstOrDefault().CATEGORIECLIENT.LIBELLE : string.Empty;
                        _LaDemande.LeClient.LIBELLETYPEPIECE  = _LaDemandeCritere.DCLIENT.FirstOrDefault().PIECEIDENTITE  != null ? _LaDemandeCritere.DCLIENT.FirstOrDefault().PIECEIDENTITE.LIBELLE : string.Empty;
                    }

                    if (_LaDemandeCritere.DABON != null && _LaDemandeCritere.DABON.Count != 0)
                        _LaDemande.Abonne = Entities.ConvertObject<CsAbon, DABON>(_LaDemandeCritere.DABON.FirstOrDefault());

                    if (_LaDemandeCritere.DBRT != null && _LaDemandeCritere.DBRT.Count != 0)
                    {
                        _LaDemande.Branchement = Entities.ConvertObject<CsBrt, DBRT>(_LaDemandeCritere.DBRT.FirstOrDefault());
                        _LaDemande.Branchement.LIBELLETYPEBRANCHEMENT = _LaDemandeCritere.DBRT.FirstOrDefault().TYPEBRANCHEMENT.LIBELLE != null ? _LaDemandeCritere.DBRT.FirstOrDefault().TYPEBRANCHEMENT.LIBELLE : string.Empty;


                    }

                    if (_LaDemandeCritere.DCANALISATION != null && _LaDemandeCritere.DCANALISATION.Count != 0)
                        _LaDemande.LstCanalistion = Entities.ConvertObject<CsCanalisation, DCANALISATION>(_LaDemandeCritere.DCANALISATION.ToList());

                    if (_LaDemande.LstCanalistion != null && _LaDemande.LstCanalistion.Count != 0)
                    {
                        foreach (DCANALISATION item in _LaDemandeCritere.DCANALISATION)
                        {

                            MAGASINVIRTUEL leCompteur = Galatee.Entity.Model.AccueilProcedures.RetourneCompteur(int.Parse(item.FK_IDCOMPTEUR.ToString()));
                            if (leCompteur != null && !string.IsNullOrEmpty(leCompteur.NUMERO))
                            {
                                CsCanalisation leCpt = _LaDemande.LstCanalistion.FirstOrDefault(t => t.FK_IDCOMPTEUR == leCompteur.PK_ID);
                                if (leCpt != null && !string.IsNullOrEmpty(leCpt.CLIENT))
                                {
                                    leCpt.NUMERO = leCompteur.NUMERO;
                                    leCpt.TYPECOMPTEUR = leCompteur.TYPECOMPTEUR.CODE ;
                                    //leCpt.DIAMETRE = leCompteur.DIAMETRE;
                                    leCpt.MARQUE = leCompteur.MARQUE;
                                    leCpt.COEFLECT = leCompteur.COEFLECT;
                                    leCpt.COEFCOMPTAGE = leCompteur.COEFCOMPTAGE;
                                    leCpt.CADRAN = leCompteur.CADRAN;
                                    leCpt.ANNEEFAB = leCompteur.ANNEEFAB;
                                    leCpt.FONCTIONNEMENT = leCompteur.FONCTIONNEMENT;
                                }
                            }
                        }
                    }
                    if (_LaDemandeCritere.RUBRIQUEDEMANDE != null && _LaDemandeCritere.RUBRIQUEDEMANDE.Count != 0)
                        _LaDemande.LstCoutDemande = Entities.ConvertObject<CsDemandeDetailCout, RUBRIQUEDEMANDE>(_LaDemandeCritere.RUBRIQUEDEMANDE.ToList());

                    if (_LaDemandeCritere.DEVENEMENT != null && _LaDemandeCritere.DEVENEMENT.Count != 0)
                        _LaDemande.LstEvenement = Entities.ConvertObject<CsEvenement, DEVENEMENT>(_LaDemandeCritere.DEVENEMENT.ToList());

                    if (_LaDemandeCritere.DOCUMENTSCANNE != null && _LaDemandeCritere.DOCUMENTSCANNE.Count != 0)
                        _LaDemande.ObjetScanne  = Entities.ConvertObject<ObjDOCUMENTSCANNE , DOCUMENTSCANNE>(_LaDemandeCritere.DOCUMENTSCANNE.ToList());


                    if (_LaDemandeCritere.ELEMENTDEVIS != null && _LaDemandeCritere.ELEMENTDEVIS.Count != 0)
                    {
                        //_LaDemande.EltDevis = Entities.ConvertObject<ObjELEMENTDEVIS, ELEMENTDEVIS>(_LaDemandeCritere.ELEMENTDEVIS.ToList());
                        //foreach (ObjELEMENTDEVIS item in _LaDemande.EltDevis)
                        //{
                            
                        //}
                    }
                    if (_LaDemandeCritere.APPAREILSDEVIS != null && _LaDemandeCritere.APPAREILSDEVIS.Count != 0)
                    {
                        _LaDemande.AppareilDevis = Entities.ConvertObject<ObjAPPAREILSDEVIS, APPAREILSDEVIS>(_LaDemandeCritere.APPAREILSDEVIS.ToList());
                    }
           
                    _LesDemande.Add(_LaDemande);
               
                leContext.Dispose();
                return _LaDemande;
            }
            catch (Exception ex)
            {
               
                throw ex;
            }
        }
        public CsDemande RetourneDetailDemandeFromDevis(ObjDEVIS leDevis)
        {
            try
            {
                galadbEntities leContext = new galadbEntities();
                List<CsDemande> _LesDemande = new List<CsDemande>();
                DEMANDE _LaDemandeCritere = Galatee.Entity.Model.AccueilProcedures.RetourneDetailDemandeByDevis(leDevis, leContext);
                if (_LaDemandeCritere == null) return null;

                CsDemande _LaDemande = new CsDemande();
                _LaDemande.LaDemande = Entities.ConvertObject<CsDemandeBase, DEMANDE>(_LaDemandeCritere);

                if (_LaDemandeCritere.DAG != null && _LaDemandeCritere.DAG.Count != 0)
                    _LaDemande.Ag = Entities.ConvertObject<CsAg, DAG>(_LaDemandeCritere.DAG.FirstOrDefault());

                if (_LaDemandeCritere.DCLIENT != null && _LaDemandeCritere.DCLIENT.Count != 0)
                    _LaDemande.LeClient = Entities.ConvertObject<CsClient, DCLIENT>(_LaDemandeCritere.DCLIENT.FirstOrDefault());

                if (_LaDemandeCritere.DABON != null && _LaDemandeCritere.DABON.Count != 0)
                    _LaDemande.Abonne = Entities.ConvertObject<CsAbon, DABON>(_LaDemandeCritere.DABON.FirstOrDefault());

                if (_LaDemandeCritere.DBRT != null && _LaDemandeCritere.DBRT.Count != 0)
                    _LaDemande.Branchement = Entities.ConvertObject<CsBrt, DBRT>(_LaDemandeCritere.DBRT.FirstOrDefault());

                if (_LaDemandeCritere.DCANALISATION != null && _LaDemandeCritere.DCANALISATION.Count != 0)
                    _LaDemande.LstCanalistion = Entities.ConvertObject<CsCanalisation, DCANALISATION>(_LaDemandeCritere.DCANALISATION.ToList());

                if (_LaDemandeCritere.RUBRIQUEDEMANDE != null && _LaDemandeCritere.RUBRIQUEDEMANDE.Count != 0)
                    _LaDemande.LstCoutDemande = Entities.ConvertObject<CsDemandeDetailCout, RUBRIQUEDEMANDE>(_LaDemandeCritere.RUBRIQUEDEMANDE.ToList());

                if (_LaDemandeCritere.DEVENEMENT != null && _LaDemandeCritere.DEVENEMENT.Count != 0)
                    _LaDemande.LstEvenement = Entities.ConvertObject<CsEvenement, DEVENEMENT>(_LaDemandeCritere.DEVENEMENT.ToList());

                //if (_LaDemandeCritere.NUMDEVIS != null && !string.IsNullOrEmpty(_LaDemandeCritere.NUMDEVIS))
                //    _LaDemande.LeDevis = RetourneDevis(_LaDemandeCritere.NUMDEVIS);

                _LesDemande.Add(_LaDemande);

                leContext.Dispose();
                return _LaDemande;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public ObjDOCUMENTSCANNE RetourneObjetScane(CsDemandeBase laDamande)
        {
            //return Entities.GetEntityFromQuery<ObjDOCUMENTSCANNE>(DevisProcedures.DEVIS_DOCUMENTSCANNE_RETOURNEById(laDamande.FICHIERJOINT));
            return null;
        }
        public List<CsDemandeBase> RetourneListeDemande(int? Idcentre, string numdem, List<string> LstTdem, DateTime? datedebut, DateTime? dateFin,
                                           DateTime? datedemande, string numerodebut, string numerofin, string status)
        {

            try
            {
                List<CsDemandeBase> _LesDemande = RetourneLesDemande(Idcentre, numdem, LstTdem, datedebut, dateFin,
                                            datedemande, numerodebut, numerofin, status);

                return _LesDemande.OrderBy(t =>t.FK_IDCENTRE).ThenBy(t=>t.LIBELLETYPEDEMANDE ).ThenBy(t=>t.NUMDEM).ToList();
            }
            catch (Exception ex)
            {
  
                throw ex;
            }
        }
        public List<CsDemandeBase> RetourneListeDemandeCritere(int? Idcentre, string numdem, List<string> LstTdem, DateTime? datedebut, DateTime? dateFin,
                                           DateTime? datedemande, string numerodebut, string numerofin, string status, string Commune, string Quatier, string Secteur, string Rue, string Porte, string Etage, string NumeroLot, string Compteur, string Nom)
        {
            string client = string.Empty;
            try
            {
                List<CsDemandeBase> _LesDemande = RetourneListeDemandeCritere_(Idcentre, numdem, LstTdem, datedebut, dateFin,
                                            datedemande, numerodebut, numerofin, status, Commune, Quatier, Secteur, Rue, Porte, Etage, NumeroLot, Compteur, Nom);

                List<CsDemandeBase>  newList = new List<CsDemandeBase>();

                if (!string.IsNullOrWhiteSpace(Compteur) || !string.IsNullOrWhiteSpace(Nom))
                {
                    CsDemandeBase dem = null;

                    foreach (CsDemandeBase item in _LesDemande)
                    {
                        client = item.CLIENT;

                        dem = new CsDemandeBase();
                        dem = GetNomEtCompteur(item.PK_ID);
                        if (!string.IsNullOrWhiteSpace(Compteur) && dem != null && !string.IsNullOrEmpty(dem.COMPTEUR))
                            item.COMPTEUR = dem.COMPTEUR.ToUpper();

                        if (!string.IsNullOrWhiteSpace(Nom) && dem != null && !string.IsNullOrEmpty(dem.NOMCLIENT))
                            item.NOMCLIENT = dem.NOMCLIENT.ToUpper();

                        newList.Add(item);
                    }

                    if (!string.IsNullOrWhiteSpace(Compteur))
                        newList = newList.Where(a => a.COMPTEUR == Compteur.ToUpper()).ToList();

                    if (!string.IsNullOrWhiteSpace(Nom))
                        newList = newList.Where(a => a.NOMCLIENT.Contains(Nom.ToUpper())).ToList();


                }


                //return _LesDemande.OrderBy(t => t.FK_IDCENTRE).ThenBy(t => t.LIBELLETYPEDEMANDE).ThenBy(t => t.NUMDEM).ToList();
                return newList.OrderBy(t => t.FK_IDCENTRE).ThenBy(t => t.LIBELLETYPEDEMANDE).ThenBy(t => t.NUMDEM).ToList();
            }
            catch (Exception ex)
            {
                client = client;
                throw ex;
            }
        }
        public List<CsDemandeBase> RetourneLesDemande(int? Idcentre, string numdem, List<string> LstTdem, DateTime? datedebut, DateTime? dateFin,
                                   DateTime? datedemande, string numerodebut, string numerofin, string status)
        {
            try
            {
                DataTable dt = Galatee.Entity.Model.AccueilProcedures.RetourneListeDemandeVisual(Idcentre, numdem, LstTdem, datedebut, dateFin,
                                            datedemande, numerodebut, numerofin, status);
                return Entities.GetEntityListFromQuery<CsDemandeBase>(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
 
        //public List<CsDemandeBase> RetourneLesDemande(int? Idcentre, string numdem, List<string> LstTdem, DateTime? datedebut, DateTime? dateFin,
        //                           DateTime? datedemande, string numerodebut, string numerofin, string status)
        //{

        //    string CodeTypedemande = DBBase.RetourneStringListeObject(LstTdem);

        //    cn = new SqlConnection(ConnectionString);
        //    cmd = new SqlCommand();
        //    cmd.Connection = cn;
        //    cmd.CommandTimeout = 1800;
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.CommandText = "SPX_ACC_RECHERCHEDEMANDECRITER";
        //    if (cmd.Parameters != null && cmd.Parameters.Count != 0) cmd.Parameters.Clear();
        //    cmd.Parameters.Add("@NumDem", SqlDbType.VarChar, 20).Value = numdem;
        //    cmd.Parameters.Add("@IdCentre", SqlDbType.Int ).Value = Idcentre;
        //    cmd.Parameters.Add("@LstTdem ", SqlDbType.VarChar, int.MaxValue ).Value = CodeTypedemande;
        //    cmd.Parameters.Add("@Datedebut", SqlDbType.Date).Value = datedebut;
        //    cmd.Parameters.Add("@DateFin", SqlDbType.Date).Value = dateFin;
        //    cmd.Parameters.Add("@Datedemande", SqlDbType.Date, 3).Value =datedemande;
 
        //    DBBase.SetDBNullParametre(cmd.Parameters);
        //    try
        //    {
        //        if (cn.State == ConnectionState.Closed)
        //            cn.Open();
        //        SqlDataReader reader = cmd.ExecuteReader();
        //        DataTable dt = new DataTable();
        //        dt.Load(reader);
        //        return Entities.GetEntityListFromQuery<CsDemandeBase>(dt);
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

        public List<CsDemandeBase> RetourneListeDemandeCritere_(int? Idcentre, string numdem, List<string> LstTdem, DateTime? datedebut, DateTime? dateFin,
                                   DateTime? datedemande, string numerodebut, string numerofin, string status, string Commune, string Quatier, string Secteur, string Rue, string Porte, string Etage, string NumeroLot, string Compteur, string Nom)
        {
            try
            {
                DataTable dt = Galatee.Entity.Model.AccueilProcedures.RetourneListeDemandeCritere_(Idcentre, numdem, LstTdem, datedebut, dateFin,
                                            datedemande, numerodebut, numerofin, status, Commune, Quatier, Secteur, Rue, Porte, Etage, NumeroLot, Compteur, Nom);
                return Entities.GetEntityListFromQuery<CsDemandeBase>(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CsBrt> RetourneBranchement(int fk_idcentre, string centre, string client, string produit )
        {
            SqlConnection laConnection = DBBase.InitConnection(ConnectionString);
            try
            {
                return Select_brtList(fk_idcentre, centre, client, laConnection);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (laConnection.State == ConnectionState.Open)
                    laConnection.Close(); // Fermeture de la connection 
                laConnection.Dispose();
            }

        }
        public List<CsEvenement> RetourneEvenementClient(CsClient LeClient)
        {
            SqlConnection laConnection = DBBase.InitConnection(ConnectionString);

            try
            {
                CsAbon leAbon;

                if (LeClient.PK_ID == 0)
                {
                    leAbon = Select_Abon(LeClient.FK_IDCENTRE.Value, LeClient.CENTRE, LeClient.REFCLIENT, LeClient.ORDRE, LeClient.PRODUIT, laConnection);
                    if (leAbon != null && !string.IsNullOrEmpty(leAbon.CLIENT))
                        LeClient.FK_IDABON = leAbon.PK_ID;
                }
                else
                {
                    //CsAbon leAbon = new DBAccueil().RetourneAbon(LeClient.PK_ID);
                    leAbon = Select_Abon(int.Parse(LeClient.FK_IDCENTRE.ToString()), LeClient.CENTRE, LeClient.REFCLIENT, LeClient.ORDRE, LeClient.PRODUIT, laConnection);
                    if (leAbon != null && !string.IsNullOrEmpty(leAbon.CLIENT))
                        LeClient.FK_IDABON = leAbon.PK_ID;

                }
                return new DbFacturation().RetourneEvenementSpx(LeClient.FK_IDCENTRE.Value, LeClient.FK_IDABON.Value, LeClient.FK_IDPRODUIT.Value, LeClient.PERIODE);
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (laConnection.State == ConnectionState.Open)
                    laConnection.Close(); // Fermeture de la connection 
                laConnection.Dispose();
            }
        }

        public List<CsOrganeScelleDemande > RetourneScellage(int fk_idBrt)
        {
            //cmd.CommandText = "SPX_GUI_RETOURNE_BRANCHEMENT";
            try
            {
                DataTable dt = Galatee.Entity.Model.AccueilProcedures.RetourneScellage(fk_idBrt);
                return Entities.GetEntityListFromQuery<CsOrganeScelleDemande>(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public List<CsOrganeScelleDemande> RetourneScellageCompteur(string numero, string marque)
        {
            //cmd.CommandText = "SPX_GUI_RETOURNE_BRANCHEMENT";
            try
            {
                List<CsOrganeScelleDemande> LstOrganeScelle = new List<CsOrganeScelleDemande>();
                DataTable dt = Galatee.Entity.Model.AccueilProcedures.RetourneScellageCompteur(numero, marque);
                List<CsScellageCompteur> lst = Entities.GetEntityListFromQuery<CsScellageCompteur>(dt);
                foreach (var item in lst)
                {
                    CsOrganeScelleDemande Le;
                    if (!string.IsNullOrEmpty(item.Cache_Scelle))
                    {
                        Le = new CsOrganeScelleDemande();
                        Le.NUM_SCELLE = item.Cache_Scelle;
                        Le.LIBELLEORGANE_SCELLABLE  = "CACHE BORNE";
                        LstOrganeScelle.Add(Le);
                    }
                    if (!string.IsNullOrEmpty(item.CapotMoteur_ID_Scelle1 ))
                    {
                        Le = new CsOrganeScelleDemande();
                        Le.NUM_SCELLE = item.CapotMoteur_ID_Scelle1;
                        Le.LIBELLEORGANE_SCELLABLE = "CAPOT GAUCHE";
                        LstOrganeScelle.Add(Le);
                    }
                    if (!string.IsNullOrEmpty(item.CapotMoteur_ID_Scelle2))
                    {
                        Le = new CsOrganeScelleDemande();
                        Le.NUM_SCELLE = item.CapotMoteur_ID_Scelle1;
                        Le.LIBELLEORGANE_SCELLABLE = "CAPOT MILIEU";
                        LstOrganeScelle.Add(Le);
                    }
                    if (!string.IsNullOrEmpty(item.CapotMoteur_ID_Scelle3))
                    {
                        Le = new CsOrganeScelleDemande();
                        Le.NUM_SCELLE = item.Cache_Scelle;
                        Le.LIBELLEORGANE_SCELLABLE = "CAPOT DROIT";
                        LstOrganeScelle.Add(Le);
                    }
                }
                return LstOrganeScelle;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public CsAbon RetourneAbon(int fk_idcentre, string centre, string client, string ordre,string Produit)
        {
            //cmd.CommandText = "SPX_GUI_RETOURNE_ABON";
            try
            {
                galadbEntities ctx = new galadbEntities();
                DataTable dt = Galatee.Entity.Model.AccueilProcedures.RetourneAbonProduit(fk_idcentre, centre, client, ordre,Produit );
                return  Entities.GetEntityFromQuery <CsAbon>(dt);

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public List<CsAbon> RetourneAbon(int fk_idcentre, string centre, string client, string ordre)
        {
            SqlConnection laConnection = DBBase.InitConnection(ConnectionString);
            try
            {
                return Select_AbonList(fk_idcentre, centre, client, ordre, laConnection);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (laConnection.State == ConnectionState.Open)
                    laConnection.Close(); // Fermeture de la connection 
                laConnection.Dispose();
            }
        }

        public CsAbon RetourneAbon(int fk_idclient)
        {
            try
            {
                galadbEntities ctx = new galadbEntities();
                DataTable dt = Galatee.Entity.Model.AccueilProcedures.RetourneAbon(fk_idclient);
                return Entities.GetEntityFromQuery<CsAbon>(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public CsClasseurClient RetourneClasseurClient(CsClientRechercher _LeClientRecherche)
        {
            SqlConnection laConnection = DBBase.InitConnection(ConnectionString);
            try
            {
                CsClasseurClient _LeClasseur = new CsClasseurClient();
                _LeClasseur.Ag = RetourneAdresse(_LeClientRecherche.FK_IDCENTRE, _LeClientRecherche.CENTRE, _LeClientRecherche.CLIENT);
                _LeClasseur.LeClient = RetourneClient(_LeClientRecherche.FK_IDCENTRE, _LeClientRecherche.CENTRE, _LeClientRecherche.CLIENT, _LeClientRecherche.ORDRE);
                _LeClasseur.LstAbonnement = RetourneAbon(_LeClientRecherche.FK_IDCENTRE, _LeClientRecherche.CENTRE, _LeClientRecherche.CLIENT, _LeClientRecherche.ORDRE);
                _LeClasseur.LstBranchement = Select_brtList(_LeClientRecherche.FK_IDCENTRE, _LeClientRecherche.CENTRE, _LeClientRecherche.CLIENT, laConnection);
                _LeClasseur.LstCanalistion = RetourneCanalisation(_LeClientRecherche.FK_IDCENTRE, _LeClientRecherche.CENTRE, _LeClientRecherche.CLIENT, _LeClientRecherche.PRODUIT, null);
                _LeClasseur.LstEvenement = RetourneEvenement(_LeClientRecherche.FK_IDCENTRE, _LeClientRecherche.CENTRE, _LeClientRecherche.CLIENT, _LeClientRecherche.ORDRE, _LeClientRecherche.PRODUIT, null, null);
                _LeClasseur.LeCompteClient = RetourneLeCompteClient(_LeClientRecherche.FK_IDCENTRE, _LeClientRecherche.CENTRE, _LeClientRecherche.CLIENT, _LeClientRecherche.ORDRE);
                return _LeClasseur;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (laConnection.State == ConnectionState.Open)
                    laConnection.Close(); // Fermeture de la connection 
                laConnection.Dispose();
            }
        }

        public CsClasseurClient RetourneModificationBrt(CsClientRechercher _LeClientRecherche)
        {
            SqlConnection laConnection = DBBase.InitConnection(ConnectionString);

            try
            {
                CsClasseurClient _LeClasseur = new CsClasseurClient();
                _LeClasseur.Ag = RetourneAdresse(_LeClientRecherche.FK_IDCENTRE ,_LeClientRecherche.CENTRE, _LeClientRecherche.CLIENT);
                _LeClasseur.LstBranchement = Select_brtList(_LeClientRecherche.FK_IDCENTRE, _LeClientRecherche.CENTRE, _LeClientRecherche.CLIENT, laConnection);
                _LeClasseur.LstCanalistion = RetourneCanalisation(_LeClientRecherche.FK_IDCENTRE , _LeClientRecherche.CENTRE, _LeClientRecherche.CLIENT, _LeClientRecherche.PRODUIT, null);
                return _LeClasseur;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (laConnection.State == ConnectionState.Open)
                    laConnection.Close(); // Fermeture de la connection 
                laConnection.Dispose();
            }
        }

        public bool MAJDemande(CsDemandeBase _laDemande)
        {
            try
            {
                return Galatee.Entity.Model.AccueilProcedures.MAJDemande(_laDemande);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public CsAg RetourneAdresse(int fk_idcentre, string centre, string client)
        {
            SqlConnection laConnection = DBBase.InitConnection(ConnectionString);
            try
            {
                return Select_Ag(fk_idcentre, centre, client, laConnection);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (laConnection.State == ConnectionState.Open)
                    laConnection.Close(); // Fermeture de la connection 
                laConnection.Dispose();
            }

        }

        public CsDemandeBase  RetourneDemande(string NumDemande, string Centre)
        {
            //cmd.CommandText = "SPX_GUI_RETOURNE_DEMANDE";
            try
            {
                DataTable dt = Galatee.Entity.Model.AccueilProcedures.RetourneDemande(NumDemande, Centre);
                return Entities.GetEntityFromQuery<CsDemandeBase >(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
       public CsDemande   RetourneLaDemande(string NumDemande, string Centre,int Fk_Idcentre)
        {
            //cmd.CommandText = "SPX_GUI_RETOURNE_DEMANDE";
            CsDemande _LaDemande = new CsDemande();

            try
            {
                _LaDemande.LaDemande = RetourneDemande(Centre, NumDemande);
                _LaDemande.Ag = RetourneDAG(Fk_Idcentre,Centre, NumDemande);
                _LaDemande.LeClient = RetourneDClient(Fk_Idcentre,Centre, NumDemande);
                _LaDemande.Abonne = RetourneDAbon(Fk_Idcentre,Centre, NumDemande);
                _LaDemande.Branchement = RetourneDbrt(Fk_Idcentre,Centre, NumDemande);
                _LaDemande.LstCanalistion = RetourneDcanalisation(Fk_Idcentre,Centre, NumDemande);
                _LaDemande.LstCoutDemande = RetourneDemandeDetailCout(Fk_Idcentre,Centre, NumDemande);
                _LaDemande.LstEvenement = RetourneDEvenement(Fk_Idcentre,Centre, NumDemande);
                return _LaDemande;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
  

       public string RetourneOrdreMax(int fk_idcentre, string centre, string client, string produit)
       {
           //cmd.CommandText = "SPX_GUI_RETOURNE_ORDREMAX";
           try
           {
               return Galatee.Entity.Model.AccueilProcedures.RetourneOrdreMax(fk_idcentre, centre, client, produit);
           }
           catch (Exception ex)
           {
               throw ex;
           }


       }


        public CsClient RetourneClient(int fk_idcentre,  string centre, string client, string ordre)
        {
            SqlConnection laConnection = DBBase.InitConnection(ConnectionString);

            try
            {
                return Select_client(fk_idcentre, centre, client, ordre, laConnection);
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (laConnection.State == ConnectionState.Open)
                    laConnection.Close();
                laConnection.Dispose();
            }
        }

        public List<CsCanalisation> RetourneCanalisationClasseur(int fk_idcentre, string pCentre, string pClient,  int fk_idabon, string pProduit, int? pPoint)
        {
            //cmd.CommandText = "SPX_GUI_RETOURNE_CANALISATION";
            try
            {
                List<CsCanalisation> lstCompteur = new List<CsCanalisation>();
                DataTable dt = Galatee.Entity.Model.AccueilProcedures.RetourneCanalisationClasseur( fk_idcentre,  pCentre,  pClient,  fk_idabon,  pProduit,  pPoint);
                List<CsCanalisation> lstCanalisation = Entities.GetEntityListFromQuery<CsCanalisation>(dt);
                if (lstCanalisation != null && lstCanalisation.Count != 0)
                {
                    if (lstCanalisation.FirstOrDefault().PRODUIT != Enumere.ElectriciteMT)
                    {
                        CsCanalisation leCompteurActif = lstCanalisation.FirstOrDefault(o => o.DEPOSE == null);
                        if (leCompteurActif != null) lstCompteur.Add(leCompteurActif);
                        else lstCompteur.Add(lstCanalisation.LastOrDefault());
                    }
                    else
                    {
                        List<CsCanalisation> leCompteurActif = lstCanalisation.Where(o => o.DEPOSE == null).ToList();
                        if (leCompteurActif != null && leCompteurActif.Count != 0) lstCompteur.AddRange(leCompteurActif);
                        else
                        {
                            var ComptActf = lstCanalisation.Select(t => new { t.NUMERO, t.POINT, t.PK_ID, t.POSE }).OrderByDescending(o => o.POSE).Take(6).ToList();
                            List<int> lstId = ComptActf.Select(y => y.PK_ID).ToList();
                            lstCompteur.AddRange(lstCanalisation.Where(u => lstId.Contains(u.PK_ID)).ToList());
                        }
                    }
                }
                return lstCompteur;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public CsCanalisation RetourneCanalisationResilier(int fk_idcentre, string centre, string client, string produit)
        {
            //cmd.CommandText = "SPX_GUI_RETOURNE_CANALISATION";
            try
            {
                DataTable dt = Galatee.Entity.Model.AccueilProcedures.RetourneCanalisationResilier(fk_idcentre, centre, client, produit);
                List<CsCanalisation> lesCompteur = Entities.GetEntityListFromQuery  <CsCanalisation>(dt);
                if (lesCompteur.Count != 0)
                    return lesCompteur.First();
                else return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CsCanalisation> RetourneCanalisation(int fk_idcentre, string centre, string client, string produit,int ? point)
        {
            //cmd.CommandText = "SPX_GUI_RETOURNE_CANALISATION";
            try
            {
                DataTable dt = Galatee.Entity.Model.AccueilProcedures.RetourneCanalisation(fk_idcentre,centre, client, produit, point);
                return Entities.GetEntityListFromQuery<CsCanalisation>(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CsEvenement> RetourneEvenement(int fk_idcentre, string centre, string client, string Ordre, string produit, int? point,int ? evenement)
        {
            //cmd.CommandText = "SPX_GUI_RETOURNE_EVENEMENT";
            try
            {
                DataTable dt = Galatee.Entity.Model.AccueilProcedures.RetourneEvenement(fk_idcentre,centre, client, Ordre, produit, point, evenement);
                return Entities.GetEntityListFromQuery<CsEvenement >(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public CsCoutCoper RetourneCoutParCoper(string pCentre, string pProduit, string pDiametre, string pCoper, string pTarif, bool pIsSubventionne)
        {
            //cmd.CommandText = "SPX_GUI_RETOURNE_MONTANTCOPER";
            try
            {
                //DataTable dt = Galatee.Entity.Model.AccueilProcedures.RetourneCoutParCoper(pCentre, pProduit, pDiametre, pCoper, pTarif, pIsSubventionne);
                //return Entities.GetEntityFromQuery <CsCoutCoper>(dt);
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public List<CsRegCli> RetourneCodeRegroupementByCampagne(int IdCampagne)
        {
            try
            {
                DataTable dt = Galatee.Entity.Model.CommonProcedures.RetourneCodeRegroupementByCampagne(IdCampagne);
                return Entities.GetEntityListFromQuery<CsRegCli>(dt);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }


        }
        public CsAbon RetourneDAbon(int fk_idcentre, string Centre,string numdem)
        {
            //cmd.CommandText = "SPX_GUI_RETOURNE_DABON";
            try
            {
                DataTable dt = Galatee.Entity.Model.AccueilProcedures.RetourneDAbon(fk_idcentre,Centre, numdem);
                return Entities.GetEntityFromQuery <CsAbon >(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public CsAg RetourneDAG(int fk_idcentre, string Centre, string numdem)
        {
            //cmd.CommandText = "SPX_GUI_RETOURNE_DAG";
            try
            {
                DataTable dt = Galatee.Entity.Model.AccueilProcedures.RetourneDAG(fk_idcentre,Centre, numdem);
                return Entities.GetEntityFromQuery<CsAg>(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public CsClient RetourneDClient(int fk_idcentre, string Centre, string numdem)
        {
            //cmd.CommandText = "SPX_GUI_RETOURNE_DCLIENT";
            try
            {
                DataTable dt = Galatee.Entity.Model.AccueilProcedures.RetourneDClient(fk_idcentre,Centre, numdem);
                return Entities.GetEntityFromQuery<CsClient>(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public CsBrt RetourneDbrt(int fk_idcentre, string Centre, string numdem)
        {
            //cmd.CommandText = "SPX_GUI_RETOURNE_DBRT";
            try
            {
                DataTable dt = Galatee.Entity.Model.AccueilProcedures.RetourneDBrt(fk_idcentre,Centre, numdem);
                return Entities.GetEntityFromQuery<CsBrt>(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public CsBrtplus RetourneDbrtplus(int fk_idcentre, string Centre, string numdem)
        {
            //cmd.CommandText = "SPX_GUI_RETOURNE_DBRTPLUS";
            try
            {
                DataTable dt = Galatee.Entity.Model.AccueilProcedures.RetourneDBrt(fk_idcentre , Centre, numdem);
                return Entities.GetEntityFromQuery<CsBrtplus>(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CsEvenement> RetourneDEvenement(int fk_idcentre, string Centre, string numdem)
        {
            //cmd.CommandText = "SPX_GUI_RETOURNE_DCANALISATION";
            try
            {
                DataTable dt = Galatee.Entity.Model.AccueilProcedures.RetourneDEvenement(fk_idcentre, Centre, numdem);
                return Entities.GetEntityListFromQuery<CsEvenement>(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public List<CsCanalisation> RetourneDcanalisation(int fk_idcentre, string Centre, string numdem)
        {
            //cmd.CommandText = "SPX_GUI_RETOURNE_DCANALISATION";
            try
            {
                DataTable dt = Galatee.Entity.Model.AccueilProcedures.RetourneDcanalisation(fk_idcentre, Centre, numdem);
                return Entities.GetEntityListFromQuery<CsCanalisation>(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public List<CsDemandeDetailCout> RetourneDemandeDetailCout(int fk_idcentre, string Centre, string numdem)
        {
            //cmd.CommandText = "SPX_GUI_RETOURNE_DCANALISATION";
            try
            {
                DataTable dt = Galatee.Entity.Model.AccueilProcedures.RetourneDemandeDetailCout(fk_idcentre, Centre, numdem);
                return Entities.GetEntityListFromQuery<CsDemandeDetailCout>(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        public List<CsFacture> RetourneFacture(string centre, string client, string ordre)
        {
            try
            {
                galadbEntities context = new galadbEntities();
                List<LCLIENT> dt = Galatee.Entity.Model.AccueilProcedures.RetourneFacture(centre, client, ordre, context);
                List<CsFacture> lstFact = Entities.ConvertObject<CsFacture, LCLIENT>(dt);
                context.Dispose();
                return lstFact;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CsReglement > RetourneReglementTranscaisB(string centre, string client, string ordre)
        {
            try
            {
                DataTable dt = Galatee.Entity.Model.AccueilProcedures.RetourneReglementTranscaisB (centre, client, ordre);
                return Entities.GetEntityListFromQuery<CsReglement>(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CsReglement> RetourneReglementTranscaisse(string centre, string client, string ordre)
        {
            try
            {
                DataTable dt = Galatee.Entity.Model.AccueilProcedures.RetourneReglementTranscaisB(centre, client, ordre);
                return Entities.GetEntityListFromQuery<CsReglement>(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CsTranscaisse> RetourneReglementAjustementCredit(string centre, string client, string ordre)
        {
            try
            {
                DataTable dt = Galatee.Entity.Model.AccueilProcedures.RetourneFactureCredit(centre, client, ordre);
                return Entities.GetEntityListFromQuery<CsTranscaisse>(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public CsCompteClient  RetourneLeCompteClient(int fk_idcentre, string centre, string client, string ordre)
        {
            try
            {
                List<CsLclient> _LstLclientTout = new List<CsLclient>();

                CsCompteClient _LeCompteClient = new CsCompteClient();
                List<CsLclient> lstImpaye = new List<CsLclient>();
                List<CsLclient> _LstFacture = RetourneFactureClient(fk_idcentre, centre, client, ordre);
                List<CsLclient> _LstReglement = RetourneEncaissementClient(fk_idcentre, centre, client, ordre);
                if (_LstFacture != null && _LstFacture.Count != 0)
                    lstImpaye = new DBEncaissement().RetourneFactureClientNonSolde(_LstFacture.First().FK_IDCLIENT ,centre, client, ordre);


                _LstReglement.ForEach(t => t.DC = "C");
                _LstFacture.ForEach(t => t.DC = "D");
                _LstLclientTout.AddRange(_LstFacture);
                _LstLclientTout.AddRange(_LstReglement);


                _LeCompteClient.Ordre = ordre;
                _LeCompteClient.LstReglement = _LstReglement;
                _LeCompteClient.LstFacture = _LstFacture;
                _LeCompteClient.Impayes = lstImpaye;
                _LeCompteClient.ToutLClient = _LstLclientTout; 
                return _LeCompteClient;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public bool ValiderRejetDemande(CsDemandeBase  LaDemande)
        {
            try
            {
                int restransaction = 0;
                using (galadbEntities transaction = new galadbEntities())
                {
                    AccueilProcedures.ValiderRejetDemande(LaDemande,transaction);
                   restransaction = transaction.SaveChanges();
                };
                return (restransaction != 0 ? true : false);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public bool ValiderDemande(CsDemande LaDemande)
        {
            try
            {
                int resultTransaction = -1;
                if (LaDemande.LaDemande.STATUT == Enumere.DemandeStatusPasseeEncaisse)
                    LaDemande.LaDemande.STATUT = Enumere.DemandeStatusPriseEnCompte;
                using (galadbEntities transaction = new galadbEntities())
                {
                    
                    //new ErrorManager().WriteInLogFile(this, "debogue 1");
                   AccueilProcedures.MisAjourDemande(LaDemande,transaction);
                   resultTransaction= transaction.SaveChanges();
                };
                return (resultTransaction== -1 ? false : true) ;
            }
            catch (Exception ex)
            {
                new ErrorManager().WriteInLogFile(this, ex.Message);
                throw ex;
            }

        }
      
        public string  ValiderDemandeDevis(CsDemande LaDemande)
        {
            try
            {
                int resultTransaction = -1;
                string DemandeID = string.Empty;
                if (LaDemande.LaDemande.STATUT == Enumere.DemandeStatusPasseeEncaisse)
                    LaDemande.LaDemande.STATUT = Enumere.DemandeStatusPriseEnCompte;
                using (galadbEntities transaction = new galadbEntities())
                {
                    try
                    {
                        AccueilProcedures.MisAjourDemande(LaDemande, transaction);
                        resultTransaction = transaction.SaveChanges();
                        if (resultTransaction != -1)
                        {
                            LaDemande.LaDemande.PK_ID = transaction.DEMANDE.FirstOrDefault(d => d.NUMDEM == LaDemande.LaDemande.NUMDEM && d.CENTRE == LaDemande.LaDemande.CENTRE).PK_ID;
                            if (LaDemande.LaDemande.PK_ID == 0)
                            {
                                using (galadbEntities tctx = new galadbEntities())
                                {
                                    DEMANDE laDem = tctx.DEMANDE.FirstOrDefault(t => t.NUMDEM == LaDemande.LaDemande.NUMDEM);
                                    if (laDem != null)
                                        DemandeID = laDem.PK_ID + "." + LaDemande.LaDemande.NUMDEM + "." + LaDemande.LaDemande.CLIENT;
                                };
                            }
                            else
                            {
                                DemandeID = LaDemande.LaDemande.PK_ID + "." + LaDemande.LaDemande.NUMDEM + "." + LaDemande.LaDemande.CLIENT;
                            }

                        }
                    }
                    catch (Exception es)
                    {
                        
                        throw es;
                    }
                };
                return DemandeID;

            }
            catch (Exception ex)
            {
                throw new Exception( ex.Message);
            }
            //catch (DbUpdateConcurrencyException e)
            //{
            // return e.Message;
            //}
            //catch (DbEntityValidationException ex)
            //{
            //    // Retrieve the error messages as a list of strings.
            //    var errorMessages = ex.EntityValidationErrors
            //            .SelectMany(x => x.ValidationErrors)
            //            .Select(x => x.ErrorMessage);

            //    // Join the list to a single string.
            //    var fullErrorMessage = string.Join(ex.EntityValidationErrors.First().Entry.Entity.GetType().Name + "  ; ", errorMessages);

            //    // Combine the original exception message with the new one.
            //    var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

            //    // Throw a new DbEntityValidationException with the improved exception message.
            //    throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
            //}

        }


        //public bool ClotureValiderDemande(CsDemande LaDemande)
        //{
        //    int resultTransaction = -1;
        //    try
        //    {

        //        bool Returnevalue = false;
        //        SqlCommand laCommande = DBBase.InitTransaction(ConnectionString);
        //        if (LaDemande.LaDemande.TYPEDEMANDE == Enumere.Resiliation)
        //        {
        //            this.ValiderResiliation(LaDemande.LaDemande.NUMDEM, LaDemande.LaDemande.PK_ID, LaDemande.LaDemande.USERCREATION, laCommande);
        //            TransmettreDemande(LaDemande.LaDemande.NUMDEM, LaDemande.InfoDemande.FK_IDETAPEACTUELLE, LaDemande.LaDemande.USERCREATION, laCommande);
        //            laCommande.Transaction.Commit();
        //            Returnevalue = true;
        //        }
        //        else if (LaDemande.LaDemande.TYPEDEMANDE == Enumere.ModificationClient ||
        //            LaDemande.LaDemande.TYPEDEMANDE == Enumere.ModificationAbonnement ||
        //            LaDemande.LaDemande.TYPEDEMANDE == Enumere.ModificationAdresse ||
        //            LaDemande.LaDemande.TYPEDEMANDE == Enumere.ModificationCompteur ||
        //            LaDemande.LaDemande.TYPEDEMANDE == Enumere.ModificationBranchement)
        //        {
        //            if (!string.IsNullOrEmpty(ValiderMoficationClient(LaDemande, true)))
        //                return false;
        //            else
        //                Returnevalue = true;

        //        }




        //        using (galadbEntities transaction = new galadbEntities())
        //        {
        //            if (LaDemande.LaDemande.TYPEDEMANDE == Enumere.BranchementAbonement ||
        //                LaDemande.LaDemande.TYPEDEMANDE == Enumere.BranchementAbonnementEp ||
        //                LaDemande.LaDemande.TYPEDEMANDE == Enumere.BranchementAbonementExtention ||
        //                LaDemande.LaDemande.TYPEDEMANDE == Enumere.BranchementAbonnementMt)
        //            {
        //                if (LaDemande.LaDemande.TYPEDEMANDE == Enumere.BranchementAbonnementMt)
        //                    AccueilProcedures.MiseAjoursAbonBrtAbonnementMt(LaDemande, transaction);
        //                else
        //                    AccueilProcedures.MiseAjoursAbonBrtAbonnement(LaDemande, transaction);
        //            }
        //            else if (LaDemande.LaDemande.TYPEDEMANDE == Enumere.BranchementSimple)

        //                AccueilProcedures.MiseAjoursBrtSimple(LaDemande, transaction);

        //            else if (LaDemande.LaDemande.TYPEDEMANDE == Enumere.AbonnementSeul)
        //            {
        //                if (LaDemande.LaDemande.ISMETREAFAIRE == false)
        //                    MiseAJourAbonnementSeulSansMetre(LaDemande, laCommande);
        //                else
        //                    AccueilProcedures.MiseAjoursAbonnementSeul(LaDemande, transaction);
        //            }

        //            else if (LaDemande.LaDemande.TYPEDEMANDE == Enumere.Resiliation)
        //                AccueilProcedures.MiseAjoursResiliation(LaDemande, transaction);

        //            else if (LaDemande.LaDemande.TYPEDEMANDE == Enumere.DepannageEp)
        //                AccueilProcedures.MiseAjoursDepannage(LaDemande, transaction);

        //            else if (LaDemande.LaDemande.TYPEDEMANDE == Enumere.Reabonnement)
        //                AccueilProcedures.MiseAjoursReabonnement(LaDemande, transaction);

        //            else if (LaDemande.LaDemande.TYPEDEMANDE == Enumere.ChangementCompteur)
        //                AccueilProcedures.MiseAjoursChangementCompteur(LaDemande, transaction);

        //            else if (LaDemande.LaDemande.TYPEDEMANDE == Enumere.DepannagePrepayer)
        //                AccueilProcedures.MiseAjoursDepannagePrepaye(LaDemande, transaction);


        //            else if (LaDemande.LaDemande.TYPEDEMANDE == Enumere.Etalonage)
        //                AccueilProcedures.MiseAjoursEtalonnageCompteur(LaDemande, transaction);

        //            //else if (LaDemande.LaDemande.TYPEDEMANDE == Enumere.FermetureBrt || LaDemande.LaDemande.TYPEDEMANDE == Enumere.ReouvertureBrt)
        //            //     AccueilProcedures.MiseAjoursFermetureOuvertureBrt(LaDemande, transaction);
                        
        //                /*
        //            else if (LaDemande.LaDemande.TYPEDEMANDE == Enumere.ModificationAbonnement)
        //                AccueilProcedures.MiseAJourModifAbonnement(LaDemande, transaction);

        //            else if (LaDemande.LaDemande.TYPEDEMANDE == Enumere.ModificationAdresse)
        //                AccueilProcedures.MiseAJourModifAdresse(LaDemande, transaction);

        //            else if (LaDemande.LaDemande.TYPEDEMANDE == Enumere.ModificationBranchement)
        //                AccueilProcedures.MiseAJourModifBranchement(LaDemande, transaction);

        //            else if (LaDemande.LaDemande.TYPEDEMANDE == Enumere.ModificationClient)
        //                AccueilProcedures.MiseAJourModifClient(LaDemande, transaction);

        //            else if (LaDemande.LaDemande.TYPEDEMANDE == Enumere.ModificationCompteur)
        //                AccueilProcedures.MiseAJourModifCompteur(LaDemande, transaction);
                    
        //                  */
                         
        //            else if (LaDemande.LaDemande.TYPEDEMANDE == Enumere.ReprisIndex)
        //                AccueilProcedures.MiseAJourRepriseIndex(LaDemande, transaction);

        //            else if (LaDemande.LaDemande.TYPEDEMANDE == Enumere.RegularisationAvance)
        //                AccueilProcedures.MiseAJourRegularisationAvance(LaDemande, transaction);

        //            else if (LaDemande.LaDemande.TYPEDEMANDE == Enumere.AugmentationPuissance ||
        //                LaDemande.LaDemande.TYPEDEMANDE == Enumere.DimunitionPuissance)
        //            {
        //                if (LaDemande.LaDemande.ISMETREAFAIRE == false)
        //                    MiseAJourVariationPuissanceSansMetre(LaDemande, transaction);
        //                else
        //                    AccueilProcedures.MiseAJourVariationPuissance(LaDemande, transaction);
        //            }

        //            else if (LaDemande.LaDemande.TYPEDEMANDE == Enumere.TransfertSiteNonMigre)
        //                AccueilProcedures.MiseAjoursTransfertSiteNonMigre2(LaDemande, transaction);

        //            else if (LaDemande.LaDemande.TYPEDEMANDE == Enumere.ChangementProduit)
        //                AccueilProcedures.MiseAjoursChangementProduit(LaDemande, transaction);

        //            resultTransaction = transaction.SaveChanges();

        //        };

        //        return resultTransaction == -1 ? false : true;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    //catch (System.Data.Entity.Validation.DbEntityValidationException ex)
        //    //{
        //    //    // Retrieve the error messages as a list of strings.
        //    //    var errorMessages = ex.EntityValidationErrors
        //    //            .SelectMany(x => x.ValidationErrors)
        //    //            .Select(x => x.ErrorMessage);

        //    //    // Join the list to a single string.
        //    //    var fullErrorMessage = string.Join("; ", errorMessages);

        //    //    // Combine the original exception message with the new one.
        //    //    var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

        //    //    return false;
        //    //    // Throw a new DbEntityValidationException with the improved exception message.
        //    //    throw new System.Data.Entity.Validation.DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);

        //    //}
        //}



        public bool MiseAJourVariationPuissanceSansMetre(CsDemande pDemande, galadbEntities pContext)
        {
            try
            {
                bool Resultat = true;
                galadbEntities _LeContextInter = new galadbEntities();
                List<RUBRIQUEDEMANDE> _LstDemandeCout = new List<RUBRIQUEDEMANDE>();
                DEMANDE _Demande = new DEMANDE();
                if (pDemande.LaDemande != null)
                    _Demande = Entities.ConvertObject<DEMANDE, CsDemandeBase>(pDemande.LaDemande);

                _Demande.FK_IDADMUTILISATEUR = _LeContextInter.ADMUTILISATEUR.FirstOrDefault(p => p.MATRICULE == _Demande.MATRICULE).PK_ID;
                _Demande.FK_IDTYPEDEMANDE = _LeContextInter.TYPEDEMANDE.FirstOrDefault(p => p.CODE == _Demande.TYPEDEMANDE).PK_ID;

                ABON leAbon = pContext.ABON.FirstOrDefault(o => o.FK_IDCENTRE == pDemande.Abonne.FK_IDCENTRE &&
                                                              o.CENTRE == pDemande.Abonne.CENTRE &&
                                                              o.CLIENT == pDemande.Abonne.CLIENT &&
                                                              o.ORDRE == pDemande.Abonne.ORDRE);
                if (leAbon != null)
                {
                    leAbon.PUISSANCE = pDemande.Abonne.PUISSANCE.Value ;
                    if (pDemande.Abonne.PRODUIT == Enumere.ElectriciteMT)
                    {
                        RUBRIQUEDEMANDE laRubriqueDemande = _LeContextInter.RUBRIQUEDEMANDE.FirstOrDefault(p => p.NUMDEM  == _Demande.NUMDEM && p.FK_IDDEMANDE == _Demande.PK_ID && p.COPER == Enumere.CoperCAU  );
                        if (laRubriqueDemande != null && laRubriqueDemande.MONTANTHT != 0)
                            leAbon.AVANCE =( pDemande.Abonne.AVANCE != null ?pDemande.Abonne.AVANCE :0) + (laRubriqueDemande.MONTANTHT + laRubriqueDemande.MONTANTTAXE ) ;
                    }
                }
                if (pDemande.Abonne.PRODUIT != Enumere.ElectriciteMT)
                {
                    CANALISATION laCanalisation = pContext.CANALISATION.FirstOrDefault(o => o.FK_IDCENTRE == pDemande.Abonne.FK_IDCENTRE &&
                                                             o.CENTRE == pDemande.Abonne.CENTRE &&
                                                             o.CLIENT == pDemande.Abonne.CLIENT);
                    if (laCanalisation != null)
                    {
                        REGLAGECOMPTEUR leReglage = _LeContextInter.REGLAGECOMPTEUR.FirstOrDefault(p => p.CODE == pDemande.LaDemande.REGLAGECOMPTEUR &&
                                                                                                        p.FK_IDPRODUIT == pDemande.LaDemande.FK_IDPRODUIT);
                        if (leReglage != null)
                        {
                            laCanalisation.REGLAGECOMPTEUR = pDemande.LaDemande.REGLAGECOMPTEUR;
                            laCanalisation.FK_IDREGLAGECOMPTEUR = leReglage.PK_ID;
                            pDemande.LaDemande.CODEREGLAGECOMPTEUR = leReglage.REGLAGE;
                        }
                    }
                    if (pDemande.LaDemande.PRODUIT == Enumere.Prepaye)
                        this.UpdateEclipseSansChangementCompteur(pDemande);
                }
                return Resultat;

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void  ValiderDemandeByDevis(ObjDEVIS leDevis, galadbEntities transaction)
        {
            try
            {
                    CsDemande LaDemande = RetourneDetailDemandeFromDevis(leDevis);
                    AccueilProcedures.MisAjourDemande(LaDemande,transaction);
                    if (LaDemande.LaDemande.TYPEDEMANDE == Enumere.BranchementAbonement)
                        AccueilProcedures.MiseAjoursAbonBrtAbonnement(LaDemande, transaction);
                    else if (LaDemande.LaDemande.TYPEDEMANDE == Enumere.BranchementSimple)
                        AccueilProcedures.MiseAjoursBrtSimple(LaDemande, transaction);
                 
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public CsClient VerifieSiClientExist(string centre, string client)
        {
            //string MaxOrdre = RetourneOrdreMax(centre, client, produit);
            //if (!string.IsNullOrEmpty(MaxOrdre))
            //{
            //    DBEncaissement dbEncaisse = new DBEncaissement();
            //    return dbEncaisse.TestClientExist(centre, client, MaxOrdre);
            //}
            //else
            //{
            //    DBEncaissement dbEncaisse = new DBEncaissement();
            //    return dbEncaisse.TestClientExist(centre, client, "01");
            //}
            return null;
        }

        public CsClient VerifieSiCompteurExist(string NumeroCompteur)
        {
            //string MaxOrdre = RetourneOrdreMax(centre, client, produit);
            //if (!string.IsNullOrEmpty(MaxOrdre))
            //{
            //    DBEncaissement dbEncaisse = new DBEncaissement();
            //    return dbEncaisse.TestClientExist(centre, client, MaxOrdre);
            //}
            //else
            //{
            //    DBEncaissement dbEncaisse = new DBEncaissement();
            //    return dbEncaisse.TestClientExist(centre, client, "01");
            //}
            return null;
        }


        public List<CsLclient> RetourneListeFactureClient(int fk_idcentre, string centre, string client, string ordre)
        {

   
            try
            {
                DataTable  dt = Galatee.Entity.Model.AccueilProcedures.RetourneListeFactureClient(fk_idcentre,centre, client, ordre);
                return Entities.GetEntityListFromQuery < CsLclient>(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
       
           
            
        }
        public List<CsLclient> RetourneListeReglement(int fk_idcentre, string centre, string client, string ordre)
        {
            try
            {
                DataTable  dt = Galatee.Entity.Model.AccueilProcedures.RetourneListeReglement(fk_idcentre,centre, client, ordre);
                return Entities.GetEntityListFromQuery <CsLclient>(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CsEvenement> RetourneDernierEvenementFacturer(int fk_idcentre, string centre, string client, string Ordre, string produit,int? point)
        {
            // Le dernier evenement sur le compteur dans canalisation
            List<CsEvenement> LstEvenement = new List<CsEvenement>();
            List<CsCanalisation> LstCanalisation = RetourneCanalisation(fk_idcentre,centre, client, produit, point);
            foreach (CsCanalisation item in LstCanalisation)
            {
                int _max = 0;
                List<CsEvenement> LstTotEvenement = new List<CsEvenement>();
                LstTotEvenement = RetourneEvenement(fk_idcentre,centre, client, Ordre, produit, item.POINT, null);

                List<CsEvenement> LstEvenementDuCompteurEnCours = LstTotEvenement.Where(p => p.POINT  == item.POINT &&
                    //&& p.COMPTEUR  == item.COMPTEUR &&
                                                                                           (p.STATUS == Enumere.EvenementFacture ||
                                                                                             p.STATUS == Enumere.EvenementMisAJour ||
                                                                                            p.STATUS == Enumere.EvenementPurger)).ToList();
                _max = LstEvenementDuCompteurEnCours.Max(t => t.NUMEVENEMENT );
                if (_max >= 0)
                {
                    CsEvenement _LeDernierEvt = LstEvenementDuCompteurEnCours.FirstOrDefault(p =>
                                                p.POINT  == item.POINT &&
                                                p.NUMEVENEMENT  == _max);

                    if (_LeDernierEvt != null)
                    {
                        //if (_max > 0)
                        //{
                        //    CsEvenement _LeInfoPreced = LstEvenementDuCompteurEnCours.FirstOrDefault(p => p.NUMEVENEMENT   == (_max - 1));
                        //    if (_LeInfoPreced != null)
                        //        LstEvenement.Add(_LeInfoPreced);
                        //}
                        LstEvenement.Add(_LeDernierEvt);
                    }
                }
            }
            return LstEvenement;
        }
        public List<CsEvenement> RetourneEvenementDeLaCanalisation(List<CsCanalisation> LstCanalisation)
        {
            List<CsEvenement> LstEnvement = new List<CsEvenement>();
            foreach (CsCanalisation item in LstCanalisation)
                LstEnvement.AddRange(RetourneEvenement(item.FK_IDCENTRE , item.CENTRE, item.CLIENT, item.ORDRE , item.PRODUIT , item.POINT,null  ));  
            return LstEnvement ;
        }

        //public List<CsEvenement> RetourneDernierEvenementDeLaCanalisation(CsAbon leAbonnement)
        //{
        //    List<CsEvenement> LstEnvement = new List<CsEvenement>();
        //    List<CsCanalisation> LstCanalisation = this.RetourneCanalisation(leAbonnement.FK_IDCENTRE, leAbonnement.CENTRE, leAbonnement.CLIENT, leAbonnement.PRODUIT, null);
        //    foreach (CsCanalisation item in LstCanalisation)
        //    {

        //        CsEvenement LeDernierEvt = new CsEvenement();
        //        DataTable dts = IndexProcedures.RetourneEvenementPrecedent(leAbonnement.FK_IDCENTRE, leAbonnement.CENTRE, leAbonnement.CLIENT, leAbonnement.ORDRE, leAbonnement.PRODUIT, item.POINT);
        //        LeDernierEvt = Entities.GetEntityFromQuery<CsEvenement>(dts);
        //        LeDernierEvt.ETATCOMPTEUR = item.ETATDUCOMPTEUR;
        //        LeDernierEvt.PROPRIO  = item.PROPRIO ;
        //        if (LstEnvement.FirstOrDefault(t => t.ISEVENEMENTNONFACURE == true) == null)
        //        {
        //            if (IndexProcedures.VerifieEvtNonFacture(leAbonnement.FK_IDCENTRE, leAbonnement.CENTRE, leAbonnement.CLIENT, leAbonnement.ORDRE, leAbonnement.PRODUIT,LeDernierEvt.PERIODE , item.POINT)=="1")
        //                LeDernierEvt.ISEVENEMENTNONFACURE = true;
        //        }
        //        DataTable dtss = AccueilProcedures.RetourneAG(item.FK_IDCENTRE, item.CENTRE, item.CLIENT, item.ORDRE);
        //        if (dts != null)
        //        {
        //            CsAg leAgClient = new CsAg();
        //            if (dtss != null)
        //                leAgClient = Entities.GetEntityFromQuery<CsAg>(dtss);

        //            LeDernierEvt.TOURNEE = leAgClient.TOURNEE;
        //            LeDernierEvt.ORDTOUR = leAgClient.ORDTOUR;
        //            LeDernierEvt.FK_IDTOURNEE = leAgClient.FK_IDTOURNEE;
        //        }
                
        //        DataTable dtsss = AccueilProcedures.RetourneClient(item.FK_IDCENTRE, item.CENTRE, item.CLIENT, item.ORDRE);
        //        if (dtsss != null)
        //        {
        //            CsClient  leAgClient = new CsClient ();
        //            if (dtsss != null)
        //                leAgClient = Entities.GetEntityFromQuery<CsClient>(dtsss);

        //            LeDernierEvt.CATEGORIE  = leAgClient.CATEGORIE ;
        //            LeDernierEvt.FK_IDCATEGORIE  = leAgClient.FK_IDCATEGORIE ;
        //        }
               
        //        DataTable dtbrt = AccueilProcedures.RetourneBRT(item.FK_IDCENTRE, item.CENTRE, item.CLIENT, string.Empty );
        //        if (dtbrt != null)
        //        {
        //            CsBrt leAgClient = new CsBrt();
        //            if (dtbrt != null)
        //                leAgClient = Entities.GetEntityFromQuery<CsBrt>(dtbrt);
        //            LeDernierEvt.PUISSANCEINSTALLEE  = leAgClient.PUISSANCEINSTALLEE ;
        //        }
        //        DataTable dtAbon = AccueilProcedures.RetourneAbon (item.FK_IDCENTRE, item.CENTRE, item.CLIENT, string.Empty);
        //        if (dtAbon != null)
        //        {
        //            CsAbon leAbon= new CsAbon();
        //            if (dtAbon != null)
        //                leAbon = Entities.GetEntityFromQuery<CsAbon>(dtAbon);
        //            LeDernierEvt.TYPETARIF = leAbon.TYPETARIF ;
        //        }
        //        using (galadbEntities ct = new galadbEntities())
        //        {
        //            PUISSANCEINSTALLEE lp = ct.PUISSANCEINSTALLEE.FirstOrDefault(t => t.VALEUR == LeDernierEvt.PUISSANCEINSTALLEE);
        //            if (lp != null)
        //            {
        //                LeDernierEvt.COEFKR1 = lp.KPERTEREACTIVE1;
        //                LeDernierEvt.COEFKR2 = lp.KPERTEREACTIVE2 ;
        //            }
        //        }
                
        //        LstEnvement.Add(LeDernierEvt);
        //    }
        //    return LstEnvement;
        //}

        //public List<CsEvenement> RetourneEvenementCanalisationPeriode(CsAbon leAbonnement,string Periode)
        //{
        //    List<CsEvenement> LstEnvement = new List<CsEvenement>();
        //    List<CsCanalisation> LstCanalisation = this.RetourneCanalisation(leAbonnement.FK_IDCENTRE, leAbonnement.CENTRE, leAbonnement.CLIENT, leAbonnement.PRODUIT, null);
        //    foreach (CsCanalisation item in LstCanalisation)
        //    {

        //        CsEvenement LeDernierEvt = new CsEvenement();
        //        DataTable dts = IndexProcedures.RetourneEvenementPrecedentPeriode(leAbonnement.FK_IDCENTRE, leAbonnement.CENTRE, leAbonnement.CLIENT, leAbonnement.ORDRE, leAbonnement.PRODUIT, item.POINT, Periode);
        //        LeDernierEvt = Entities.GetEntityFromQuery<CsEvenement>(dts);
        //        LeDernierEvt.ETATCOMPTEUR = item.ETATDUCOMPTEUR;
        //        if (LstEnvement.FirstOrDefault(t => t.ISEVENEMENTNONFACURE == true) == null)
        //        {
        //            if (IndexProcedures.VerifieEvtNonFacture(leAbonnement.FK_IDCENTRE, leAbonnement.CENTRE, leAbonnement.CLIENT, leAbonnement.ORDRE, leAbonnement.PRODUIT, LeDernierEvt.PERIODE, item.POINT) == "1")
        //                LeDernierEvt.ISEVENEMENTNONFACURE = true;
        //        }
        //        DataTable dtss = AccueilProcedures.RetourneAG(item.FK_IDCENTRE, item.CENTRE, item.CLIENT, item.ORDRE);
        //        if (dts != null)
        //        {
        //            CsAg leAgClient = new CsAg();
        //            if (dtss != null)
        //                leAgClient = Entities.GetEntityFromQuery<CsAg>(dtss);

        //            LeDernierEvt.TOURNEE = leAgClient.TOURNEE;
        //            LeDernierEvt.ORDTOUR = leAgClient.ORDTOUR;
        //            LeDernierEvt.FK_IDTOURNEE = leAgClient.FK_IDTOURNEE;
        //        }

        //        DataTable dtsss = AccueilProcedures.RetourneClient(item.FK_IDCENTRE, item.CENTRE, item.CLIENT, item.ORDRE);
        //        if (dtsss != null)
        //        {
        //            CsClient leAgClient = new CsClient();
        //            if (dtsss != null)
        //                leAgClient = Entities.GetEntityFromQuery<CsClient>(dtsss);

        //            LeDernierEvt.CATEGORIE = leAgClient.CATEGORIE;
        //            LeDernierEvt.FK_IDCATEGORIE = leAgClient.FK_IDCATEGORIE;
        //        }

        //        DataTable dtbrt = AccueilProcedures.RetourneBRT(item.FK_IDCENTRE, item.CENTRE, item.CLIENT, string.Empty);
        //        if (dtbrt != null)
        //        {
        //            CsBrt leAgClient = new CsBrt();
        //            if (dtbrt != null)
        //                leAgClient = Entities.GetEntityFromQuery<CsBrt>(dtbrt);
        //            LeDernierEvt.PUISSANCEINSTALLEE = leAgClient.PUISSANCEINSTALLEE;
        //        }
        //        DataTable dtAbon = AccueilProcedures.RetourneAbon(item.FK_IDCENTRE, item.CENTRE, item.CLIENT, string.Empty);
        //        if (dtAbon != null)
        //        {
        //            CsAbon leAbon = new CsAbon();
        //            if (dtAbon != null)
        //                leAbon = Entities.GetEntityFromQuery<CsAbon>(dtAbon);
        //            LeDernierEvt.TYPETARIF = leAbon.TYPETARIF;
        //        }
        //        using (galadbEntities ct = new galadbEntities())
        //        {
        //            PUISSANCEINSTALLEE lp = ct.PUISSANCEINSTALLEE.FirstOrDefault(t => t.VALEUR == LeDernierEvt.PUISSANCEINSTALLEE);
        //            if (lp != null)
        //            {
        //                LeDernierEvt.COEFKR1 = lp.KPERTEREACTIVE1;
        //                LeDernierEvt.COEFKR2 = lp.KPERTEREACTIVE2;
        //            }
        //        }

        //        LstEnvement.Add(LeDernierEvt);
        //    }
        //    return LstEnvement;
        //}

        public List<CsEvenement> RetourneEvenementDeLaCanalisationPeriode(List<CsCanalisation> LstCanalisation, string Periode)
        {
            List<CsEvenement> LstEnvement = new List<CsEvenement>();
            foreach (CsCanalisation item in LstCanalisation)
            {
                CsEvenement LeDernierEvt = new CsEvenement();
                DataTable dts = IndexProcedures.RetourneEvenementPrecedentPeriode(item.FK_IDCENTRE, item.CENTRE, item.CLIENT, item.ORDRE, item.PRODUIT, item.POINT, Periode);
                DataTable dtss = AccueilProcedures.RetourneAG(item.FK_IDCENTRE, item.CENTRE, item.CLIENT, item.ORDRE);
                if (dts != null)
                {
                    CsAg leAgClient = new CsAg();
                    if (dtss != null)
                        leAgClient = Entities.GetEntityFromQuery<CsAg>(dtss);

                    LeDernierEvt = Entities.GetEntityFromQuery<CsEvenement>(dts);
                    LeDernierEvt.TOURNEE = leAgClient.TOURNEE;
                    LeDernierEvt.ORDTOUR = leAgClient.ORDTOUR;
                    LeDernierEvt.FK_IDTOURNEE = leAgClient.FK_IDTOURNEE;
                    LstEnvement.Add(LeDernierEvt);
                }
            }
            return LstEnvement;
        }

        public CsEvenement  VerifieEvenementNonFacturer(CsEvenement leEvt)
        {
            return IndexProcedures.VerifieEvtNonFacture(leEvt.FK_IDCENTRE, leEvt.CENTRE, leEvt.CLIENT, leEvt.ORDRE, leEvt.PRODUIT,leEvt.PERIODE , leEvt.POINT);
        }
 
        //Demande
        private void  InsererLstLclient(SqlCommand cmd, List<CsReglement> LstFacture,string Typedemande)
        {
            foreach (CsReglement item in LstFacture)
            {
                //if (Typedemande == Enumere.Reabonnement)
                //{
                //    CsSpeSite _LeSpsite = RetourneNumFacture(item.CENTRE);
                //    if (_LeSpsite != null)
                //        item.NDOC = _LeSpsite.LOTFAC;
                //}
                //new DBEncaissement().InsererLclient(item, cmd);
            }
        }
 
        private CsEvenement  CreeEvemenentDeFermetureOuverture(CsEvenement  _LeEvt)
        {
            _LeEvt.DATEEVT = System.DateTime.Now;
            _LeEvt.PERIODE = System.DateTime.Today.Year.ToString() + System.DateTime.Today.Month.ToString();
            _LeEvt.INDEXEVT = 0;
            //if (Lade.TDEM == Enumere.FermetureBrt)
                _LeEvt.CODEEVT = Enumere.EvenementCodeFermetureBrt;
            //_LeEvt.EVENEMENT = 1;
            
            return null;
        }
        public List<CsLclient> RetourneCompteClientTransfert(CsClientRechercher LeClient, string Orientation)
        {
            try
            {
                DataTable dt = Galatee.Entity.Model.AccueilProcedures.RetourneCompteClientTransfert(LeClient.CENTRE, LeClient.CLIENT, LeClient.ORDRE, Orientation);
                return Entities.GetEntityListFromQuery<CsLclient>(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CsLePayeur> RetourneLesPayeur()
        {
            try
            {
                galadbEntities context = new galadbEntities();
                List<CsLePayeur> lstPayeur = new List<CsLePayeur>();
                List<PAYEUR > dt = Galatee.Entity.Model.AccueilProcedures.RetourneLesPayeur(context);
                foreach (var item in dt)
                {
                    CsLePayeur UnPayeur = new CsLePayeur();
                    UnPayeur.Payeur = Entities.ConvertObject<CsPayeur , Galatee.Entity.Model.PAYEUR >(item);
                    UnPayeur.ClientPayeur  = Entities.ConvertObject<CsClient  , Galatee.Entity.Model.CLIENT  >(item.CLIENT.ToList());
                    lstPayeur.Add(UnPayeur);
                }
                context.Dispose();
                return lstPayeur;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool MajPayeur(CsLePayeur _LePayeur, int Action)
        {
            try
            {
                return AccueilProcedures.MajPayeur(_LePayeur, Action);
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        public bool TransfertCompteClient(CsLclient LeClient, CsLclient Client2, string Orientation)
        {
            try
            {
                return Galatee.Entity.Model.AccueilProcedures.TranfertCompteClient(LeClient, Client2, Orientation);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CsDetailCampagne > RetourneFactureCampagne(string IdCampagen, string centre, string client, string ordre)
        {
            try
            {
                DataTable dt = AccueilProcedures.RetourneFactureCampagne( IdCampagen,  centre,  client,  ordre);
                return Entities.GetEntityListFromQuery<CsDetailCampagne>(dt);
        
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CsTypeCoupure> RetourneTypeCoupure()
        {
            try
            {
                DataTable dt = AccueilProcedures.RetourneTypeCoupure();
                return Entities.GetEntityListFromQuery<CsTypeCoupure>(dt);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool InsererLclient(CsLclient  Lafacture)
        {
            try
            {
                galadbEntities context = new galadbEntities();
                Lafacture.FK_IDLIBELLETOP = context.LIBELLETOP.FirstOrDefault(t => t.CODE == Lafacture.TOP1 ).PK_ID;
                Lafacture.FK_IDCOPER = context.COPER.FirstOrDefault(t => t.CODE == Lafacture.COPER).PK_ID;
                Lafacture.FK_IDADMUTILISATEUR  = context.ADMUTILISATEUR .FirstOrDefault(t => t.MATRICULE  == Lafacture.MATRICULE ).PK_ID; 
                LCLIENT _laFacture = Galatee.Tools.Utility.ConvertEntity<LCLIENT, CsLclient >(Lafacture);
                return Entities.InsertEntity<LCLIENT>(_laFacture);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region CLI330
        public List<CsCodeControle> RetourneCodeControle()
        {
            //cmd.CommandText = "SPX_GUI_RETOURNE_CODECONTROLE";
            try
            {
                DataTable dt = Galatee.Entity.Model.AccueilProcedures.RetourneCodeControle ();
                return  Entities.GetEntityListFromQuery<CsCodeControle >(dt);
               
            }
            catch (Exception ex)
            {
                throw new Exception(cmd.CommandText + ":" + ex.Message);
            }
            
        }
        public List<CsDetailLot> RetourneListeDesDetailLot(int IdLot)
        {
            //cmd.CommandText = "SPX_GUI_RETOURNE_DETAILLOT";
            try
            {
                DataTable dt = Galatee.Entity.Model.AccueilProcedures.RetourneListeDesDetailLot(IdLot);
                return Entities.GetEntityListFromQuery<CsDetailLot>(dt);

            }
            catch (Exception ex)
            {
                throw new Exception(cmd.CommandText + ":" + ex.Message);
            }
           
        }
        public List<CsLotCompteClient> RetourneListeDesTypeLot(string Origine, string TypeLot)
        {
            //cmd.CommandText = "SPX_GUI_RETOURNE_LOTCOMPTECLIENT";
            try
            {
                DataTable dt = Galatee.Entity.Model.AccueilProcedures.RetourneListeDesTypeLot(Origine, TypeLot);
                return Entities.GetEntityListFromQuery<CsLotCompteClient>(dt);

            }
            catch (Exception ex)
            {
                throw new Exception(cmd.CommandText + ":" + ex.Message);
            }

        }
        public bool ValiderSaisieBactch(CsSaisieDeMasse  _LotCompteClient)
        {
            try
            {
                galadbEntities transaction = new galadbEntities();

                AccueilProcedures.MiseAJourLot(_LotCompteClient.LotCompteClient, transaction);
                AccueilProcedures.MiseAJourDetailLot(_LotCompteClient, transaction);
                transaction.SaveChanges();
            }
            catch (Exception ex)
            {
                Transaction.RollBackTransaction(cmd);
                throw ex;
            }
            return true;
        }

        public CsNatgen RetourneNatureParCoper(string Coper)
        {
      
            //cmd.CommandText = "SPX_GUI_RETOURNE_NATURE";
            try
            {

                DataTable dt = Galatee.Entity.Model.AccueilProcedures.RetourneNatureByCoper(Coper);
                return Entities.GetEntityFromQuery <CsNatgen >(dt);
            }
            catch (Exception ex)
            {
                throw new Exception(cmd.CommandText + ":" + ex.Message);
            }
            
        }
        public CsCoper RetourneCoper(string Coper)
        {
         //cmd.CommandText = "SPX_GUI_RETOURNE_COPER";
          try
            {
                DataTable dt = Galatee.Entity.Model.AccueilProcedures.RetourneCoper(Coper);
                return Entities.GetEntityFromQuery<CsCoper >(dt);
            }
            catch (Exception ex)
            {
                throw new Exception(cmd.CommandText + ":" + ex.Message);
            }
           
        }

        public int? RetourneMaxIDlot()
        {
            //cmd.CommandText = "SPX_GUI_RETOURNE_MAXLOTCOMPTECLIENT";
            try
            {
               return  Galatee.Entity.Model.AccueilProcedures.RetourneMaxIDlot();
            }
            catch (Exception ex)
            {
                throw new Exception(cmd.CommandText + ":" + ex.Message);
            }
            
        }
   

  
        public bool ValiderMiseAJourBatch(List<CsLotCompteClient> ListLot)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    foreach (CsLotCompteClient item in ListLot)
                    {
                        List<CsDetailLot> LaListeDetail = RetourneListeDesDetailLot(item.IDLOT);
                        foreach (CsDetailLot itemlot in LaListeDetail)
                        {
                            itemlot.FK_IDMATRICULE = ListLot.First().FK_IDMATRICULE;
                            itemlot.MATRICULE  = ListLot.First().MATRICULE ;
                             CsLclient _leRegl = SetDetailLotToReglement(item, itemlot);

                             if (itemlot.SENS == Enumere.Debit)
                            {
                                if (_leRegl.MONTANT != 0)
                                    new DBEncaissement().InsererLclient(_leRegl, context);
                            }
                            else
                            {
                                if (_leRegl.MONTANT != 0)
                                    new DBEncaissement().InsererTranscaisB(_leRegl, context);
                            }
                        }
                        AccueilProcedures.MiseAJourLot(item, context);
                        context.SaveChanges();
                    }
                };
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void InsererLclient(CsReglement FactureAregle, galadbEntities cmd)
        {

            //cmd.CommandText = "SPX_ENC_INSERER_LCLIENT";
            try
            {
                LCLIENT Lclient = new LCLIENT()
                {

                    OBSERVATION_COUPURE = FactureAregle.OBSERVATION_COUPURE,
                    NUMCHEQ = FactureAregle.NUMCHEQUE,
                    RDV_COUPURE = FactureAregle.RDV_COUPURE,
                    AGENT_COUPURE  = FactureAregle.AGENT_COUPURE,
                    IDCOUPURE  = FactureAregle.IDCOUPURE,
                    MONTANTTVA = FactureAregle.MONTANTTVA,
                    DATEFLAG = FactureAregle.DATEFLAG,
                    TAXESADEDUIRE = FactureAregle.TAXESADEDUIRE,
                    MATRICULE  = FactureAregle.MATRICULE ,//
                    ACQUIT = FactureAregle.ACQUIT,
                    REFEMNDOC = FactureAregle.REFEMNDOC,
                    REFERENCE = FactureAregle.REFERENCE,
                    DATEVALEUR = FactureAregle.DATEFACTURE,
                    IDLOT  = FactureAregle.IDLOT,
                    REFERENCEPUPITRE = FactureAregle.REFERENCEPUPITRE,
                    FRAISDERETARD = FactureAregle.FRAISRETARD,
                    EXIGIBILITE = FactureAregle.DATEEXIGIBLE,
                    TOP1  = FactureAregle.TOP1 ,//
                    MOISCOMPT = FactureAregle.MOISCOMPT,
                    ECART = FactureAregle.ECART,
                    CAISSE  = FactureAregle.NumCaiss ,
                    ORIGINE = FactureAregle.ORIGINE,
                    DC = FactureAregle.DC,
                    MODEREG  = FactureAregle.MODEREG ,//
                    CRET = FactureAregle.CRET,
                    CAPUR = FactureAregle.CAPUR,
                    MONTANT = FactureAregle.MONTANTPAYE,
                    EXIG = 0,
                    DENR = DateTime.Today.Date,
                    COPER  = FactureAregle.COPER ,//
                    NDOC = FactureAregle.NDOC,
                    REFEM = FactureAregle.REFEM,
                    ORDRE  = FactureAregle.ORDRE ,
                    CENTRE  = FactureAregle.CENTRE,//
                    CLIENT  = FactureAregle.CLIENT,
                    USERCREATION = FactureAregle.USERCREATION,
                    DATECREATION = FactureAregle.DATECREATION,
                    BANQUE = FactureAregle.BANQUE,
                    GUICHET  = FactureAregle.GUICHET
                    //USERMODIFICATION = FactureAregle.USERMODIFICATION,
                    // DATEMODIFICATION = FactureAregle.DATEMODIFICATION

                };
                Entities.InsertEntity<LCLIENT>(Lclient);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CsDetailLot> RetourneDetailLot(List<CsLotCompteClient> ListLot)
        {
            try
            {
                List<CsDetailLot> ListeDesDetails = new List<CsDetailLot>();
                foreach (CsLotCompteClient item in ListLot)
                    ListeDesDetails.AddRange(RetourneListeDesDetailLot(item.IDLOT));
                return ListeDesDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private  List<CsLclient> RetourneListeCompteAjuste(CsLotCompteClient  Lot)
        {
            //cmd.CommandText = "SPX_GUI_RETOURNE_AJUSTEMENTLOT";
            try
            {
                DataTable dt = Galatee.Entity.Model.AccueilProcedures.RetourneListeDesCompteAdjusteLot(Lot.IDLOT);
                return Entities.GetEntityListFromQuery<CsLclient>(dt);

            }
            catch (Exception ex)
            {
                throw new Exception(cmd.CommandText + ":" + ex.Message);
            }
            
        }
        public List<CsLclient> RetourneCompteAjuste(List<CsLotCompteClient> ListLot)
        {
            List<CsLclient> ListeDesDetails = new List<CsLclient>();

            foreach (CsLotCompteClient item in ListLot)
                ListeDesDetails.AddRange(RetourneListeCompteAjuste(item));
            return ListeDesDetails;

        }
        private CsLclient  SetDetailLotToReglement(CsLotCompteClient LeLot, CsDetailLot LeDetail)
        {
            CsLclient _Lclient = new CsLclient()
            {
                CENTRE = LeDetail.CENTRE,
                CLIENT = LeDetail.CLIENT,
                ORDRE = LeDetail.ORDRE,
                REFEM = LeDetail.REFEM,
                NDOC = LeDetail.NDOC,
                NATURE  = LeDetail.NATURE,
                COPER  = LeDetail.COPER,
                DENR = LeDetail.DATESAISIE.Value ,
                MONTANT = LeDetail.MONTANT,
                MODEREG  = LeDetail.MODEREG,
                DC = LeDetail.SENS,
                ORIGINE = LeLot.ORIGINE ,
                ECART = LeDetail.ECART,
                MOISCOMPT = LeLot.MOISCOMPTABLE,
                IDLOT = LeDetail.IDLOT,
                DATEVALEUR = LeDetail.DATEPIECE,
                REFERENCE        = LeDetail.REFERENCE,
                REFEMNDOC        = LeDetail.REFEMNDOC,
                ACQUIT           = LeDetail.ACQUIT,
                MATRICULE = LeLot.MATRICULE ,
                USERCREATION = LeLot.USERCREATION ,
                DATECREATION =System.DateTime.Now,
                FK_IDADMUTILISATEUR = LeLot.FK_IDMATRICULE,
                FK_IDCENTRE = LeDetail.FK_IDCENTRE,
                FK_IDCOPER = LeDetail.FK_IDCOPER,
                //FK_IDNATURE = LeDetail.FK_IDNATURE,
                FK_IDLIBELLETOP = LeLot.FK_IDLIBELLETOP,
                //FK_IDCLIENT = LeDetail.FK_IDCLIENT,
                FK_IDLCLIENT = LeDetail.FK_IDLCLIENT,
                TOP1 = LeLot.TOP1
            };
            return _Lclient;
        }
        public bool PurgeLot(List<CsLotCompteClient> ListLot)
        {
            var tm = Transaction.OpenTransaction(Enumere.DataBase.Galadb);
            try
            {
                List<DETAILLOT> _LeDetailASupprimer = new List<DETAILLOT>();
                List<LOTCOMPTECLIENT> _LotASupprimer = Entities.ConvertObject<LOTCOMPTECLIENT, CsLotCompteClient>(ListLot);
                foreach (CsLotCompteClient item in ListLot)
                {
                   List<CsDetailLot> ListeDesDetails=  RetourneListeDesDetailLot(item.IDLOT);
                    _LeDetailASupprimer.AddRange( Entities.ConvertObject<DETAILLOT , CsDetailLot >(ListeDesDetails));
                }
                Entities.DeleteEntity<DETAILLOT>(_LeDetailASupprimer);
                Entities.DeleteEntity<LOTCOMPTECLIENT >(_LotASupprimer);
                return true;
            }
            catch (Exception ex)
            {
                Transaction.RollBackTransaction(tm);
                throw ex;
            }
        }

        #endregion

        #region Envoi de mail

        // Envoi de facture par mail
        public List<CsEnvoiMail> ListeDesClientPourEnvoieMail(string Centre, string Client, string Ordre, string periode)
        {
            SqlCommand sqlCommand = new SqlCommand();
            SqlConnection sqlConnection = new SqlConnection(Session.GetSqlConnexionString());
            sqlCommand.Connection = sqlConnection;
            sqlCommand.CommandTimeout = 360;
            sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
            sqlCommand.CommandText = "SPX_FACT_LISTEENVOIEMAIL";

            sqlCommand.Parameters.Add("@Centre", SqlDbType.VarChar, 6).Value = (string.IsNullOrEmpty(Centre)) ? null : Centre;
            sqlCommand.Parameters.Add("@Client", SqlDbType.VarChar, 6).Value = (string.IsNullOrEmpty(Client)) ? null : Client;
            sqlCommand.Parameters.Add("@Ordre", SqlDbType.VarChar, 6).Value = (string.IsNullOrEmpty(Ordre)) ? null : Ordre;
            sqlCommand.Parameters.Add("@periode", SqlDbType.VarChar, 6).Value = (string.IsNullOrEmpty(periode)) ? null : periode;

            DBBase.SetDBNullParametre(sqlCommand.Parameters);

            try
            {
                if (sqlConnection.State == ConnectionState.Closed)
                    sqlConnection.Open();

                SqlDataReader reader = sqlCommand.ExecuteReader();

                List<CsEnvoiMail> rows = new List<CsEnvoiMail>();

                #region remplissage de la liste

                while (reader.Read())
                {
                    CsEnvoiMail c = new CsEnvoiMail();
                    c.centre = (Convert.IsDBNull(reader["CENTRE"])) ? String.Empty : (System.String)reader["CENTRE"];
                    c.client = (Convert.IsDBNull(reader["CLIENT"])) ? String.Empty : (System.String)reader["CLIENT"];
                    c.ordre = (Convert.IsDBNull(reader["ORDRE"])) ? String.Empty : (System.String)reader["ORDRE"];
                    c.NomClient = (Convert.IsDBNull(reader["NOMABON"])) ? String.Empty : (System.String)reader["NOMABON"];
                    c.periode = (Convert.IsDBNull(reader["PERIODE"])) ? String.Empty : (System.String)reader["PERIODE"];
                    c.montant = (System.Decimal)reader["TOTPROHT"];
                    c.Email = (Convert.IsDBNull(reader["EMAIL"])) ? String.Empty : (System.String)reader["EMAIL"];
                    c.numfact = (Convert.IsDBNull(reader["FACTURE"])) ? String.Empty : (System.String)reader["FACTURE"];

                    rows.Add(c);
                }

                #endregion

                return rows;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<CsMailFacture> ListeDesFactures(string centre, string client, string ordre, string periode, string numFacture)
        {
            SqlCommand sqlCommand = new SqlCommand();
            SqlConnection sqlConnection = new SqlConnection(Session.GetSqlConnexionString());
            sqlCommand.Connection = sqlConnection;
            sqlCommand.CommandTimeout = 360;
            sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
            sqlCommand.CommandText = "SPX_FACT_EDITIONFACTURE";

            sqlCommand.Parameters.Add("@centre", SqlDbType.VarChar, 3).Value = (string.IsNullOrEmpty(centre)) ? null : centre;
            sqlCommand.Parameters.Add("@client", SqlDbType.VarChar, 20).Value = (string.IsNullOrEmpty(client)) ? null : client;
            sqlCommand.Parameters.Add("@ordre", SqlDbType.VarChar, 2).Value = (string.IsNullOrEmpty(ordre)) ? null : ordre;
            sqlCommand.Parameters.Add("@periode", SqlDbType.VarChar, 6).Value = (string.IsNullOrEmpty(periode)) ? null : periode;
            sqlCommand.Parameters.Add("@numFacture", SqlDbType.VarChar, 6).Value = (string.IsNullOrEmpty(numFacture)) ? null : numFacture;

            DBBase.SetDBNullParametre(sqlCommand.Parameters);

            //try
            //{
            if (sqlConnection.State == ConnectionState.Closed)
                sqlConnection.Open();

            SqlDataReader reader = sqlCommand.ExecuteReader();

            List<CsMailFacture> rows = new List<CsMailFacture>();

            #region remplissage de la liste

            while (reader.Read())
            {
                CsMailFacture c = new CsMailFacture();
                c.CENTRE = (Convert.IsDBNull(reader["CENTRE"])) ? String.Empty : (System.String)reader["CENTRE"];
                c.CLIENT = (Convert.IsDBNull(reader["CLIENT"])) ? String.Empty : (System.String)reader["CLIENT"];
                c.ORDRE = (Convert.IsDBNull(reader["ORDRE"])) ? String.Empty : (System.String)reader["ORDRE"];
                c.FACTURE = (Convert.IsDBNull(reader["FACTURE"])) ? String.Empty : (System.String)reader["FACTURE"];
                c.PERIODE = (Convert.IsDBNull(reader["PERIODE"])) ? String.Empty : (System.String)reader["PERIODE"];
                c.PRODUIT = (Convert.IsDBNull(reader["PRODUIT"])) ? String.Empty : (System.String)reader["PRODUIT"];
                c.TRANCHE = (Convert.IsDBNull(reader["TRANCHE"])) ? String.Empty : (System.String)reader["TRANCHE"];
                c.LIBELLEREDEVANCE = (Convert.IsDBNull(reader["LIBELLEREDEVANCE"])) ? String.Empty : (System.String)reader["LIBELLEREDEVANCE"];
                c.QUANTITE = (int)reader["QUANTITE"];
                c.BARPRIX = (Convert.IsDBNull(reader["BARPRIX"])) ? 0 : (System.Decimal)reader["BARPRIX"];
                c.REDHT = (Convert.IsDBNull(reader["REDHT"])) ? 0 : (System.Decimal)reader["REDHT"];
                c.REDTAXE = (Convert.IsDBNull(reader["REDTAXE"])) ? 0 : (System.Decimal)reader["REDTAXE"];

                c.IDENTITE = (Convert.IsDBNull(reader["IDENTITE"])) ? String.Empty : (System.String)reader["IDENTITE"];
                c.ADRESSE = (Convert.IsDBNull(reader["ADRESSE"])) ? String.Empty : (System.String)reader["ADRESSE"];
                c.NUMCOMPT = (Convert.IsDBNull(reader["NUMCOMPT"])) ? String.Empty : (System.String)reader["NUMCOMPT"];
                c.DIAMETRE = (Convert.IsDBNull(reader["DIAMETRE"])) ? String.Empty : (System.String)reader["DIAMETRE"];

                //c.ANCINDEX = (int)reader["ANCINDEX"];
                //c.NOUVINDEX = (int)reader["NOUVINDEX"];
                //c.ETATFACTURE = (Convert.IsDBNull(reader["ETATFACTURE"])) ? string.Empty : (System.String)reader["ETATFACTURE"];  

                rows.Add(c);
            }

            #endregion

            return rows;
            //}
            //catch (Exception)
            //{
            //    throw;
            //}
        }
        #endregion

        #region Sylla
        public static List< CsTypeDOCUMENTSCANNE> ChargerTypeDocument()
        {
            try
            {
                return Entities.GetEntityListFromQuery<CsTypeDOCUMENTSCANNE>(AccueilProcedures.ChargerTypeDocument());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<CsCATEGORIECLIENT_TYPECLIENT> ChargerCategorieClient_TypeClient()
        {
            try
            {
                return Entities.GetEntityListFromQuery<CsCATEGORIECLIENT_TYPECLIENT>(AccueilProcedures.ChargerCategorieClient_TypeClient());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static List<Structure.CsCATEGORIECLIENT_USAGE> ChargerCategorieClient_Usage()
        {
            try
            {
                return Entities.GetEntityListFromQuery<Galatee.Structure.CsCATEGORIECLIENT_USAGE>(AccueilProcedures.ChargerCategorieClient_Usage());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static List<Structure.CsNATURECLIENT_TYPECLIENT> ChargerNatureClient_TypeClient()
        {
            try
            {
                return Entities.GetEntityListFromQuery<Galatee.Structure.CsNATURECLIENT_TYPECLIENT>(AccueilProcedures.ChargerNatureClient_TypeClient());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static List<Structure.CsUSAGE_NATURECLIENT> ChargerUsage_NatureClient()
        {
            try
            {
                return Entities.GetEntityListFromQuery<Galatee.Structure.CsUSAGE_NATURECLIENT>(AccueilProcedures.ChargerUsage_NatureClient());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static CsCriteresDevis GetParametresDistance(string pMaxi, string pSeuil, string pMaxiSubvention)
        {
            CsCriteresDevis ParametresDistance = new CsCriteresDevis();
            try
            {
                DB_ParametresGeneraux dbParamGeneraux = new DB_ParametresGeneraux();
                if (!string.IsNullOrEmpty(pMaxi))
                {
                    var ParamMaxi = dbParamGeneraux.SelectParametresGenerauxByCode(pMaxi);
                    if (ParamMaxi != null)
                        ParametresDistance.Maxi = decimal.Parse(ParamMaxi.LIBELLE);
                }
                if (!string.IsNullOrEmpty(pSeuil))
                {
                    var ParamSeuil = dbParamGeneraux.SelectParametresGenerauxByCode(pSeuil);
                    if (ParamSeuil != null)
                        ParametresDistance.Seuil = decimal.Parse(ParamSeuil.LIBELLE);
                }
                if (!string.IsNullOrEmpty(pMaxiSubvention))
                {
                    var ParamMaxiSubvention = dbParamGeneraux.SelectParametresGenerauxByCode(pMaxiSubvention);
                    if (ParamMaxiSubvention != null)
                        ParametresDistance.MaxiSubvention = decimal.Parse(ParamMaxiSubvention.LIBELLE);
                }
                return ParametresDistance;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region 12/01/2016
        public List<CsSortieMateriel> RetourneListeSortieMaterielLivre()
        {
            try
            {
                List<CsSortieMateriel> _lstGroupeSortie = new List<CsSortieMateriel>();
                DataTable dt = Galatee.Entity.Model.AccueilProcedures.RetourneListeSortieMaterielLivre();
                _lstGroupeSortie = Entities.GetEntityListFromQuery<CsSortieMateriel>(dt);
                return _lstGroupeSortie;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public List<CsProgarmmation> RetourneProgrammation()
        {
            try
            {
                DataTable dt = AccueilProcedures.RetourneProgrammation();
                return Entities.GetEntityListFromQuery<CsProgarmmation>(dt);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public List<CsSortieMateriel > RetourneSortieMateriel()
        {
            try
            {
                DataTable dt = AccueilProcedures.RetourneSortieMateriel();
                return Entities.GetEntityListFromQuery<CsSortieMateriel>(dt);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public CsTdem RetourneTypeDemandeFromIdEtapeWkf(int idEtape)
        {
            try
            {
                CsTdem leType = null;
                CsEtape leEtape = new DB_WORKFLOW().SelectAllEtapes().FirstOrDefault(t => t.PK_ID == idEtape);
                if (leEtape != null && !string.IsNullOrEmpty(leEtape.NOM))
                {
                    CsOperation leOPer = new DB_WORKFLOW().SelectAllOperation2().FirstOrDefault(t => t.PK_ID == leEtape.FK_IDOPERATION);
                    if (leOPer != null && !string.IsNullOrEmpty(leOPer.NOM))
                         leType = RetourneOptionDemande().FirstOrDefault(t => t.PK_ID == int.Parse(leOPer.CODE_TDEM));
                }
                return leType;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public bool DesactivationProgrammation(List<int> pIdDemandeDevis)
        {
            try
            {
                return AccueilProcedures.DesactivationProgrammation(pIdDemandeDevis);
            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion

        
        #endregion


        #region fomba 29/10/2015

        public List<CsDemandeBase> RetourneListeDemandeById(List<int> demandes)
        {
            try
            {
                List<CsDemandeBase> _lstDemande = new List<CsDemandeBase>();
                DataTable dt = Galatee.Entity.Model.AccueilProcedures.RetourneListeDemandeById(demandes);
                _lstDemande = Entities.GetEntityListFromQuery<CsDemandeBase>(dt);
                return _lstDemande.OrderBy(t => t.NUMDEM).ToList();

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public List<CsDemandeBase> RetourneListeDemandeEtapeById(List<int> lesdemande, int IdTypeDemande)
        {
            try
            {
                List<CsDemandeBase> _lstDemande = new List<CsDemandeBase>();
                DataTable dt = Galatee.Entity.Model.AccueilProcedures.RetourneListeDemandeEtapeById(lesdemande, IdTypeDemande);
                _lstDemande = Entities.GetEntityListFromQuery<CsDemandeBase>(dt);
                return _lstDemande.OrderBy(t => t.NUMDEM).ToList();

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<CsDemandeBase> RetourneListeDemandeByIdSansClient(List<int> demandes, int IdTypeDemande)
        {
            try
            {
                List<CsDemandeBase> _lstDemande = new List<CsDemandeBase>();
                DataTable dt = Galatee.Entity.Model.AccueilProcedures.RetourneListeDemandeByIdSansClient(demandes, IdTypeDemande);
                _lstDemande = Entities.GetEntityListFromQuery<CsDemandeBase>(dt);
                return _lstDemande.OrderBy(t => t.NUMDEM).ToList();

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        

        public List<CsCompteur> RetourneListeCompteurLabo()
        {
            try
            {
                List<CsCompteur> _lstCompteur = new List<CsCompteur>();
                DataTable dt = Galatee.Entity.Model.AccueilProcedures.RetourneListeCompteurLaboratoireLibre();
                _lstCompteur = Entities.GetEntityListFromQuery<CsCompteur>(dt);
                return _lstCompteur;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public List<CsCompteur> RetourneListeCompteurMagasin(List<string > lstCodeSite)
        {
            try
            {
                List<CsCompteur> _lstCompteur = new List<CsCompteur>();
                DataTable dt = Galatee.Entity.Model.AccueilProcedures.RetourneListeCompteurMagasinCentre(lstCodeSite );
                _lstCompteur = Entities.GetEntityListFromQuery<CsCompteur>(dt);
                return _lstCompteur;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        // TABLE GREC
        public List<CsGroupe> RetourneListeGroupe(string codecentre)
        {
            try
            {
                List<CsGroupe> _lstGroupe = new List<CsGroupe>();
                DataTable dt = Galatee.Entity.Model.AccueilProcedures.RetourneListeGroupe(codecentre);
                _lstGroupe = Entities.GetEntityListFromQuery<CsGroupe>(dt);
                return _lstGroupe;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public List<CsTypePanne> RetourneTypePanne()
        {
            try
            {
                return Entities.GetEntityListFromQuery<CsTypePanne>(Galatee.Entity.Model.AccueilProcedures.RetourneTypePanne());
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public List<CsTypePanne> RetourneDetailTypePanne()
        {
            try
            {
                return Entities.GetEntityListFromQuery<CsTypePanne>(Galatee.Entity.Model.AccueilProcedures.RetourneDetailTypePanne());
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public List<CsModeCommunication > RetourneModeCommunication()
        {
            try
            {
                return Entities.GetEntityListFromQuery<CsModeCommunication>(Galatee.Entity.Model.AccueilProcedures.RetourneModeCommunication());
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public List<CsVehicule> RetourneVehicule()
        {
            try
            {
                return Entities.GetEntityListFromQuery<CsVehicule>(Galatee.Entity.Model.AccueilProcedures.RetourneVehicule());
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public List<CsGroupeDepannageCommune > RetourneGroupeDepannageCommune()
        {
            try
            {
                return Entities.GetEntityListFromQuery<CsGroupeDepannageCommune>(Galatee.Entity.Model.AccueilProcedures.RetourneGroupeDepannageCommune());
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        
        //


        public List<CsSortieMateriel> RetourneListeSortieMaterielLivre(int iddemande)
        {
            try
            {
                List<CsSortieMateriel> _lstGroupeSortie = new List<CsSortieMateriel>();
                DataTable dt = Galatee.Entity.Model.AccueilProcedures.RetourneListeSortieMaterielLivre(iddemande);
                _lstGroupeSortie = Entities.GetEntityListFromQuery<CsSortieMateriel>(dt);
                return _lstGroupeSortie;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public List<CsSortieAutreMateriel> RetourneListeSortieAutreMaterielLivre(int iddemande)
        {
            try
            {
                List<CsSortieAutreMateriel> _lstGroupeSortie = new List<CsSortieAutreMateriel>();
                DataTable dt = Galatee.Entity.Model.AccueilProcedures.RetourneListeSortieAutreMaterielLivre(iddemande);
                _lstGroupeSortie = Entities.GetEntityListFromQuery<CsSortieAutreMateriel>(dt);
                return _lstGroupeSortie;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public bool InsertGroupe(CsGroupe legroupe, List<CsUtilisateur> lstAgent)
        {
            try
            {
                GRC_GROUPE groupe = Entities.ConvertObject<GRC_GROUPE, CsGroupe>(legroupe);
                List<ADMUTILISATEUR> lstagent = Entities.ConvertObject<ADMUTILISATEUR, CsUtilisateur>(lstAgent);
                return Galatee.Entity.Model.AccueilProcedures.InsertGroupe(groupe, lstagent);

            }
            catch (Exception ex)
            {
                return false;
                throw ex;
            }
        }
        public bool InsertCompteurMagasin(List<CsCompteur> lstCmpteurDemande)
        {
            try
            {
                List<MAGASINVIRTUEL> lstcompteur = Entities.ConvertObject<MAGASINVIRTUEL, CsCompteur>(lstCmpteurDemande);
                return Galatee.Entity.Model.AccueilProcedures.InsertCompteurMagasin(lstcompteur);

            }
            catch (Exception ex)
            {
                return false;
                throw ex;
            }
        }

        //public List<CsDemandeBase> InsertLiaisonCompteur(List<CsDemandeBase> lstdemande)
        //{
        //    try
        //    {
        //        List<int> lesIdMagsinVirtuel = new List<int>();
        //        List<DCANALISATION> lesCanalisation = new List<DCANALISATION>();
        //        List<CsDemandeBase> lstDemande = new List<CsDemandeBase>();
        //        using (galadbEntities Context = new galadbEntities())
        //        {
        //            foreach (CsDemandeBase _LaDemande in lstdemande)
        //            {
        //                DCANALISATION canal = new DCANALISATION()
        //                {
        //                    CENTRE = _LaDemande.CENTRE,
        //                    CLIENT = _LaDemande.CLIENT,
        //                    NUMDEM = _LaDemande.NUMDEM,
        //                    PRODUIT = _LaDemande.PRODUIT,
        //                    PROPRIO = "1",
        //                    FK_IDPRODUIT = _LaDemande.FK_IDPRODUIT.Value,
        //                    FK_IDDEMANDE = _LaDemande.PK_ID,
        //                    FK_IDCENTRE = _LaDemande.FK_IDCENTRE,
        //                    POSE = System.DateTime.Now,
        //                    USERCREATION = _LaDemande.USERCREATION,
        //                    USERMODIFICATION = _LaDemande.USERCREATION,
        //                    DATECREATION = System.DateTime.Now,
        //                    DATEMODIFICATION = System.DateTime.Now,
        //                    FK_IDPROPRIETAIRE = 1,
        //                };
        //                if (_LaDemande.FK_IDREGLAGECOMPTEUR != null)
        //                {
        //                    canal.FK_IDREGLAGECOMPTEUR = _LaDemande.FK_IDREGLAGECOMPTEUR;
        //                    canal.REGLAGECOMPTEUR  = _LaDemande.REGLAGECOMPTEUR ;
        //                }

        //                if (_LaDemande.PRODUIT != Enumere.ElectriciteMT)
        //                {
        //                    REGLAGECOMPTEUR leReglageCompteur = Context.REGLAGECOMPTEUR.FirstOrDefault(t => t.PK_ID == _LaDemande.FK_IDREGLAGECOMPTEUR);
        //                    if (leReglageCompteur != null && leReglageCompteur.PK_ID != 0)
        //                    {
        //                        List<CALIBRECOMPTEUR> LeCalibreEquivalant = Context.CALIBRECOMPTEUR.Where(t => t.REGLAGEMINI >= leReglageCompteur.REGLAGEMINI &&
        //                                                                                                                  t.REGLAGEMAXI <= leReglageCompteur.REGLAGEMAXI &&
        //                                                                                                                  t.FK_IDPRODUIT == _LaDemande.FK_IDPRODUIT).ToList();

        //                        List<int> lstCalibrePossible = LeCalibreEquivalant.Select(u => u.PK_ID).ToList();
        //                        MAGASINVIRTUEL leCompteur = Context.MAGASINVIRTUEL.OrderBy(t => t.DATECREATION).FirstOrDefault(t => t.ETAT == Enumere.CompteurAffecte &&
        //                                                                                               t.CENTRE.CODE == _LaDemande.CENTRE &&
        //                                                                                               t.FK_IDPRODUIT == _LaDemande.FK_IDPRODUIT &&
        //                                                                                               lstCalibrePossible.Contains(t.FK_IDCALIBRECOMPTEUR.Value) &&
        //                                                                                               !lesIdMagsinVirtuel.Contains(t.PK_ID));
        //                        if (leCompteur != null && leCompteur.FK_IDCENTRE != 0)
        //                        {
        //                            lesIdMagsinVirtuel.Add(leCompteur.PK_ID);
        //                            canal.FK_IDMAGAZINVIRTUEL = leCompteur.PK_ID;
        //                            lesCanalisation.Add(canal);
        //                            lstDemande.Add(_LaDemande);
        //                            if (_LaDemande.PRODUIT == Enumere.Prepaye)
        //                            {
        //                                CsDemande laDemande = RetourneDetailDemandeFromDEvis(_LaDemande);
        //                                if (_LaDemande.TYPEDEMANDE == Enumere.BranchementAbonement ||
        //                                    _LaDemande.TYPEDEMANDE == Enumere.AbonnementSeul ||
        //                                    _LaDemande.TYPEDEMANDE == Enumere.Reabonnement)
        //                                    this.InsererEclipse(laDemande, leCompteur.NUMERO);
        //                                if (_LaDemande.TYPEDEMANDE == Enumere.AugmentationPuissance ||
        //                                    _LaDemande.TYPEDEMANDE == Enumere.DimunitionPuissance)
        //                                    this.UpdateEclipse(laDemande, leCompteur.NUMERO);
        //                                _LaDemande.CODECONSO = leCompteur.NUMERO;
        //                                _LaDemande.NOMCLIENT  = laDemande.LeClient.NOMABON ;
        //                            }
        //                        }
                                
        //                    }
        //                }
        //                else
        //                {
        //                    MAGASINVIRTUEL leCompteur = Context.MAGASINVIRTUEL.OrderBy(t => t.DATECREATION).FirstOrDefault(t => t.ETAT == Enumere.CompteurAffecte &&
        //                                                                                                t.CENTRE.CODE == _LaDemande.CENTRE &&
        //                                                                                                t.FK_IDPRODUIT == _LaDemande.FK_IDPRODUIT &&
        //                                                                                                !lesIdMagsinVirtuel.Contains(t.PK_ID));
        //                    if (leCompteur != null && leCompteur.FK_IDCENTRE != 0)
        //                    {
        //                        lesIdMagsinVirtuel.Add(leCompteur.PK_ID);
        //                        canal.FK_IDMAGAZINVIRTUEL = leCompteur.PK_ID;
        //                        lesCanalisation.Add(canal);
        //                        lstDemande.Add(_LaDemande);
        //                    }
        //                }
        //            }
        //            if (lesCanalisation.Count != 0)
        //                Galatee.Entity.Model.AccueilProcedures.InsertLiaisonCanalisation(lesCanalisation, lstDemande);
        //            return lstDemande;
        //        }
        //    }

        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public List<CsDemandeBase> InsertLiaisonCompteur(List<CsDemandeBase> lstdemande)
        //{
        //    List<CsDemandeBase> lstDemande = new List<CsDemandeBase>();
        //    try
        //    {
        //        List<int> lesIdMagsinVirtuel = new List<int>();
        //        using (galadbEntities Context = new galadbEntities())
        //        {
        //            foreach (CsDemandeBase _LaDemande in lstdemande)
        //            {
        //                DCANALISATION canal = new DCANALISATION()
        //                {
        //                    CENTRE = _LaDemande.CENTRE,
        //                    CLIENT = _LaDemande.CLIENT,
        //                    NUMDEM = _LaDemande.NUMDEM,
        //                    PRODUIT = _LaDemande.PRODUIT,
        //                    PROPRIO = Enumere.LOCATAIRE,
        //                    FK_IDPRODUIT = _LaDemande.FK_IDPRODUIT.Value,
        //                    FK_IDDEMANDE = _LaDemande.PK_ID,
        //                    FK_IDCENTRE = _LaDemande.FK_IDCENTRE,
        //                    POSE = System.DateTime.Now,
        //                    USERCREATION = _LaDemande.USERCREATION,
        //                    USERMODIFICATION = _LaDemande.USERCREATION,
        //                    DATECREATION = System.DateTime.Now,
        //                    DATEMODIFICATION = System.DateTime.Now,
        //                    FK_IDPROPRIETAIRE = 2
        //                    //FK_IDPROPRIETAIRE = Context.PROPRIETAIRE.FirstOrDefault(u => u.CODE == Enumere.LOCATAIRE).PK_ID,
        //                };
        //                if (_LaDemande.FK_IDREGLAGECOMPTEUR != null)
        //                {
        //                    canal.FK_IDREGLAGECOMPTEUR = _LaDemande.FK_IDREGLAGECOMPTEUR;
        //                    canal.REGLAGECOMPTEUR = _LaDemande.REGLAGECOMPTEUR;
        //                }
        //                if (_LaDemande.PRODUIT != Enumere.ElectriciteMT)
        //                {
        //                    if (_LaDemande.COMPTEUR != null && _LaDemande.FK_IDMAGAZINVIRTUEL  != 0)
        //                        {
        //                            string Compteur = string.Empty;
        //                            CsDemande laDemande = RetourneDetailDemandeFromDEvis(_LaDemande);
        //                            DCANALISATION LeCompteurDejaLie = Context.DCANALISATION.FirstOrDefault(u => u.FK_IDDEMANDE == _LaDemande.PK_ID);
        //                            if (LeCompteurDejaLie == null)
        //                            {
        //                                MAGASINVIRTUEL cpt = Context.MAGASINVIRTUEL.FirstOrDefault(c => c.PK_ID == _LaDemande.FK_IDMAGAZINVIRTUEL);
        //                                cpt.ETAT = Enumere.CompteurLie;
        //                                Compteur = cpt.NUMERO;

        //                                lesIdMagsinVirtuel.Add(_LaDemande.FK_IDMAGAZINVIRTUEL);
        //                                canal.FK_IDMAGAZINVIRTUEL = _LaDemande.FK_IDMAGAZINVIRTUEL;
        //                                canal.POINT = 1;
        //                                Entities.InsertEntity<DCANALISATION>(canal, Context);

        //                                if (_LaDemande.PRODUIT == Enumere.Prepaye)
        //                                {
        //                                    if (_LaDemande.TYPEDEMANDE == Enumere.BranchementAbonement ||
        //                                        _LaDemande.TYPEDEMANDE == Enumere.AbonnementSeul ||
        //                                        _LaDemande.TYPEDEMANDE == Enumere.ChangementProduit ||
        //                                        _LaDemande.TYPEDEMANDE == Enumere.Reabonnement)
        //                                        this.InsererEclipse(laDemande, cpt.NUMERO);

        //                                    if (_LaDemande.TYPEDEMANDE == Enumere.AugmentationPuissance ||
        //                                        _LaDemande.TYPEDEMANDE == Enumere.DimunitionPuissance)
        //                                        this.UpdateEclipse(laDemande, _LaDemande.COMPTEUR);

        //                                    if (_LaDemande.TYPEDEMANDE == Enumere.ChangementCompteur)
        //                                        this.UpdateEclipseChangementCompteur(laDemande, _LaDemande.COMPTEUR);
        //                                }
        //                                _LaDemande.CODECONSO = Compteur;
        //                                _LaDemande.NOMCLIENT = laDemande.LeClient.NOMABON;
        //                                new DbWorkFlow().ExecuterActionSurDemandeTransction(_LaDemande.NUMDEM , Enumere.TRANSMETTRE, _LaDemande.MATRICULE, string.Empty,new galadbEntities());
        //                                Context.SaveChanges();
        //                                lstDemande.Add(_LaDemande);

        //                            }
        //                            else
        //                            {
        //                                if (LeCompteurDejaLie.FK_IDMAGAZINVIRTUEL == null)
        //                                    LeCompteurDejaLie.FK_IDMAGAZINVIRTUEL = _LaDemande.FK_IDMAGAZINVIRTUEL;
                                        
        //                                MAGASINVIRTUEL cpt = Context.MAGASINVIRTUEL.FirstOrDefault(c => c.PK_ID == LeCompteurDejaLie.FK_IDMAGAZINVIRTUEL);
        //                                Compteur = cpt.NUMERO;

        //                                if (_LaDemande.PRODUIT == Enumere.Prepaye)
        //                                {
        //                                    if (_LaDemande.TYPEDEMANDE == Enumere.BranchementAbonement ||
        //                                        _LaDemande.TYPEDEMANDE == Enumere.AbonnementSeul ||
        //                                        _LaDemande.TYPEDEMANDE == Enumere.ChangementProduit ||
        //                                        _LaDemande.TYPEDEMANDE == Enumere.ChangementCompteur  ||
        //                                        _LaDemande.TYPEDEMANDE == Enumere.Reabonnement)
        //                                        this.InsererEclipse(laDemande, cpt.NUMERO);

        //                                    if (_LaDemande.TYPEDEMANDE == Enumere.AugmentationPuissance ||
        //                                        _LaDemande.TYPEDEMANDE == Enumere.DimunitionPuissance)
        //                                        this.UpdateEclipse(laDemande, _LaDemande.COMPTEUR);
        //                                }
        //                                _LaDemande.CODECONSO = Compteur;
        //                                _LaDemande.NOMCLIENT = laDemande.LeClient.NOMABON;
        //                                lstDemande.Add(_LaDemande);
        //                                Context.SaveChanges();
        //                            }
                                 
                                   
        //                        }
        //                }

                      
        //            }
        //            return lstDemande;
        //        }
        //    }

        //    catch (Exception ex)
        //    {
        //        if (lstDemande != null && lstDemande.Count != 0)
        //            return lstDemande;
        //        else 
        //           throw ex;
        //    }
        //}




        public List<CsDemandeBase> InsertLiaisonCompteur(List<CsDemandeBase> lstdemande)
        {
            List<CsDemandeBase> lstDemande = new List<CsDemandeBase>();
            try
            {

                List<int> lesIdMagsinVirtuel = new List<int>();
                using (galadbEntities Context = new galadbEntities())
                {
                    foreach (CsDemandeBase _LaDemande in lstdemande)
                    {
                        DCANALISATION canal = new DCANALISATION()
                        {
                            CENTRE = _LaDemande.CENTRE,
                            CLIENT = _LaDemande.CLIENT,
                            NUMDEM = _LaDemande.NUMDEM,
                            PRODUIT = _LaDemande.PRODUIT,
                            PROPRIO = Enumere.LOCATAIRE,
                            FK_IDPRODUIT = _LaDemande.FK_IDPRODUIT.Value,
                            FK_IDDEMANDE = _LaDemande.PK_ID,
                            FK_IDCENTRE = _LaDemande.FK_IDCENTRE,
                            POSE = System.DateTime.Now,
                            USERCREATION = _LaDemande.USERCREATION,
                            USERMODIFICATION = _LaDemande.USERCREATION,
                            DATECREATION = System.DateTime.Now,
                            DATEMODIFICATION = System.DateTime.Now,
                            FK_IDPROPRIETAIRE = 2
                            //FK_IDPROPRIETAIRE = Context.PROPRIETAIRE.FirstOrDefault(u => u.CODE == Enumere.LOCATAIRE).PK_ID,
                        };
                        if (_LaDemande.FK_IDREGLAGECOMPTEUR != null)
                        {
                            canal.FK_IDREGLAGECOMPTEUR = _LaDemande.FK_IDREGLAGECOMPTEUR;
                            canal.REGLAGECOMPTEUR = _LaDemande.REGLAGECOMPTEUR;
                        }
                        if (_LaDemande.PRODUIT != Enumere.ElectriciteMT)
                        {
                            if (_LaDemande.COMPTEUR != null && _LaDemande.FK_IDMAGAZINVIRTUEL != 0)
                            {

                                string Compteur = string.Empty;

                                //CsDemande laDemande = RetourneDetailDemandeFromDEvis(_LaDemande);
                                CsDemande laDemande = ChargerDetailDemande(_LaDemande.PK_ID, _LaDemande.NUMDEM);
                                DCANALISATION LeCompteurDejaLie = Context.DCANALISATION.FirstOrDefault(u => u.FK_IDDEMANDE == _LaDemande.PK_ID);
                                if (LeCompteurDejaLie == null)
                                {
                                    MAGASINVIRTUEL cpt = Context.MAGASINVIRTUEL.FirstOrDefault(c => c.PK_ID == _LaDemande.FK_IDMAGAZINVIRTUEL);
                                    cpt.ETAT = Enumere.CompteurLie;
                                    Compteur = cpt.NUMERO;

                                    lesIdMagsinVirtuel.Add(_LaDemande.FK_IDMAGAZINVIRTUEL);
                                    canal.FK_IDMAGAZINVIRTUEL = _LaDemande.FK_IDMAGAZINVIRTUEL;
                                    canal.POINT = 1;
                                    Entities.InsertEntity<DCANALISATION>(canal, Context);

                                    if (_LaDemande.PRODUIT == Enumere.Prepaye)
                                    {
                                        if (_LaDemande.TYPEDEMANDE == Enumere.BranchementAbonement ||
                                            _LaDemande.TYPEDEMANDE == Enumere.AbonnementSeul ||
                                            _LaDemande.TYPEDEMANDE == Enumere.ChangementProduit ||
                                            _LaDemande.TYPEDEMANDE == Enumere.BranchementAbonementExtention ||
                                            _LaDemande.TYPEDEMANDE == Enumere.Reabonnement)
                                            Compteur = this.InsererEclipse(laDemande, cpt.NUMERO);

                                       /* if (_LaDemande.TYPEDEMANDE == Enumere.AugmentationPuissance ||
                                            _LaDemande.TYPEDEMANDE == Enumere.DimunitionPuissance)
                                            this.UpdateEclipse(laDemande, cpt.NUMERO);

                                        if (_LaDemande.TYPEDEMANDE == Enumere.ChangementCompteur)
                                            this.UpdateEclipseChangementCompteur(laDemande, _LaDemande.COMPTEUR);*/
                                    }
                                    _LaDemande.CODECONSO = Compteur;
                                    _LaDemande.NOMCLIENT = laDemande.LeClient.NOMABON;
                                    new DbWorkFlow().ExecuterActionSurDemandeTransction(_LaDemande.NUMDEM, Enumere.TRANSMETTRE, _LaDemande.MATRICULE, string.Empty, new galadbEntities());
                                    Context.SaveChanges();
                                    lstDemande.Add(_LaDemande);

                                }
                                else
                                {
                                    if (LeCompteurDejaLie.FK_IDMAGAZINVIRTUEL == null)
                                        LeCompteurDejaLie.FK_IDMAGAZINVIRTUEL = _LaDemande.FK_IDMAGAZINVIRTUEL;

                                    MAGASINVIRTUEL cpt = Context.MAGASINVIRTUEL.FirstOrDefault(c => c.PK_ID == LeCompteurDejaLie.FK_IDMAGAZINVIRTUEL);
                                    Compteur = cpt.NUMERO;

                                    if (_LaDemande.PRODUIT == Enumere.Prepaye)
                                    {
                                        if (_LaDemande.TYPEDEMANDE == Enumere.BranchementAbonement ||
                                            _LaDemande.TYPEDEMANDE == Enumere.AbonnementSeul ||
                                            _LaDemande.TYPEDEMANDE == Enumere.ChangementProduit ||
                                            _LaDemande.TYPEDEMANDE == Enumere.ChangementCompteur ||
                                            _LaDemande.TYPEDEMANDE == Enumere.BranchementAbonementExtention ||
                                            _LaDemande.TYPEDEMANDE == Enumere.Reabonnement)
                                            Compteur = this.InsererEclipse(laDemande, cpt.NUMERO);

                                    /*    if (_LaDemande.TYPEDEMANDE == Enumere.AugmentationPuissance ||
                                            _LaDemande.TYPEDEMANDE == Enumere.DimunitionPuissance)
                                            this.UpdateEclipse(laDemande, cpt.NUMERO);*/
                                    }
                                    _LaDemande.CODECONSO = Compteur;
                                    _LaDemande.NOMCLIENT = laDemande.LeClient.NOMABON;
                                    lstDemande.Add(_LaDemande);
                                    Context.SaveChanges();
                                }


                            }
                        }


                    }
                    return lstDemande;
                }
            }

            catch (Exception ex)
            {
                new ErrorManager().WriteInLogFile(this, ex.Message);

                if (lstDemande != null && lstDemande.Count != 0)
                    return lstDemande;
                else
                    throw ex;
            }
        }




        //public string  InsertProgrammation(Guid idgroupe, List<CsDemandeBase> lstCmpteurDemande, DateTime pdate)
        //{
        //    try
        //    {
        //        int rest = -1;
        //        string Numero = string.Empty ;
        //        List<DEMANDE> lstdemande = Entities.ConvertObject<DEMANDE, CsDemandeBase>(lstCmpteurDemande);
        //        using (galadbEntities context = new galadbEntities())
        //        {
        //            Numero = RetourneProgramme(idgroupe, pdate, lstCmpteurDemande.FirstOrDefault().MATRICULE);
        //            foreach (DEMANDE demande in lstdemande)
        //            {
        //                PROGRAMMATION prog = new PROGRAMMATION()
        //                {
        //                    DATEPROGRAMME = pdate,
        //                    FK_IDEQUIPE = idgroupe,
        //                    FK_IDDEMANDE = demande.PK_ID,
        //                    NUMPROGRAMME = Numero,
        //                    DATECREATION = System.DateTime.Now,
        //                    ESTACTIF = true
        //                };
        //                new DbWorkFlow().ExecuterActionSurDemandeTransction(demande.NUMDEM, Enumere.TRANSMETTRE, demande.MATRICULE, string.Empty, context);
        //                demande.STATUT = ((int)Enumere.EtapeBrtAbonSanExt.SortieMat).ToString();
        //                Entities.InsertEntity(prog, context);
        //                Entities.UpdateEntity(demande, context);
        //            }
        //           rest= context.SaveChanges();
        //        }
        //        return rest == -1 ? string.Empty  : Numero;
        //    }
        //    catch (Exception ex)
        //    {
        //        return string.Empty ;
        //        throw ex;
        //    }
        //}


        public bool InsertSortieMateriel(int IdLivreur, int IdRecepteur, List<CsCanalisation> lstCompteurValide, bool IsExtension)
        {
            try
            {
                return Galatee.Entity.Model.AccueilProcedures.InsertSortieMateriel(IdLivreur, IdRecepteur, lstCompteurValide, IsExtension);
            }
            catch (Exception ex)
            {
                return false;
                throw ex;
            }
        }
        public bool InsertSortieMaterielEP(int IdLivreur, int IdRecepteur, List<CsDemandeBase > lstDemandeValide, bool IsExtension)
        {
            try
            {
                return Galatee.Entity.Model.AccueilProcedures.InsertSortieMaterielEP(IdLivreur, IdRecepteur, lstDemandeValide, IsExtension);
            }
            catch (Exception ex)
            {
                return false;
                throw ex;
            }
        }
        public bool InsertValidationSortieMateriel(List<CsCanalisation> lstDemande, int idrecepteur)
        {
            try
            {
                return Galatee.Entity.Model.AccueilProcedures.InsertValidationSortieMateriel(lstDemande,idrecepteur);

            }
            catch (Exception ex)
            {
                return false;
                throw ex;
            }
        }
       

        #endregion


        #region MAJ Modification demande

            public int RetourneDemandeByCode(string tdem)
            {
                throw new NotImplementedException();
            }

            public string GetNumDevis(int p)
            {
                return Galatee.Entity.Model.AccueilProcedures.GetNumDevis(p);
            }
        #endregion

            public bool AutorisationDemande(List<CsDemandeDetailCout> _LesFacture)
            {
                try
                {
                    int  resu = -1;
                    int iddemande = _LesFacture.First().FK_IDDEMANDE ;
                    galadbEntities ctxinter = new galadbEntities();
                    List<LCLIENT> lesFacture = new List<LCLIENT>();
                    foreach (CsDemandeDetailCout item in _LesFacture)
                        lesFacture.Add(RetourneFactureDemande(item));
                    DEMANDE lademande = ctxinter.DEMANDE.FirstOrDefault(t => t.PK_ID == iddemande);
                    ctxinter.Dispose();

                    if (lesFacture != null && lesFacture.Count != 0 && lademande != null )
                        using (galadbEntities ctx = new galadbEntities())
                        {
                            lademande.DCAISSE = System.DateTime.Today;
                            Entities.InsertEntity<LCLIENT>(lesFacture, ctx);
                            Entities.UpdateEntity<DEMANDE>(lademande, ctx);
                           resu= ctx.SaveChanges();
                        }
                    return resu == -1? false : true ;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }

            }
            private LCLIENT RetourneFactureDemande(CsDemandeDetailCout FactureAregle)
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
                    laFactureDemande.DENR = System.DateTime.Today.Date ;
                    laFactureDemande.EXIG = null ;
                    laFactureDemande.MONTANT = FactureAregle.MONTANTTTC ;
                    //laFactureDemande.CAPUR = FactureAregle.CAPUR;
                    laFactureDemande.CRET = string.Empty ;
                    laFactureDemande.MODEREG = string.Empty ;
                    laFactureDemande.DC = "D";
                    laFactureDemande.ORIGINE = FactureAregle.CENTRE ;
                    laFactureDemande.CAISSE = string.Empty  ;
                    laFactureDemande.ECART = null;
                    laFactureDemande.MOISCOMPT = System.DateTime.Today.Date.Year + System.DateTime.Today.Month.ToString("00");
                    laFactureDemande.TOP1 = string.Empty ;
                    laFactureDemande.EXIGIBILITE = null;
                    laFactureDemande.FRAISDERETARD = null;
                    laFactureDemande.MATRICULE = FactureAregle.USERCREATION ;
                    laFactureDemande.MONTANTTVA = FactureAregle.MONTANTTAXE ;
                    laFactureDemande.USERCREATION = FactureAregle.USERCREATION;
                    laFactureDemande.DATECREATION = FactureAregle.DATECREATION;
                    laFactureDemande.DATEMODIFICATION = FactureAregle.DATEMODIFICATION;
                    laFactureDemande.USERMODIFICATION = FactureAregle.USERMODIFICATION;
                    laFactureDemande.FK_IDCENTRE = FactureAregle.FK_IDCENTRE;
                    laFactureDemande.FK_IDADMUTILISATEUR = null ;
                    laFactureDemande.FK_IDCOPER = FactureAregle.FK_IDCOPER;
                    laFactureDemande.FK_IDLIBELLETOP = 1;
                    laFactureDemande.FK_IDCLIENT = 1;

                    return laFactureDemande;

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            public List<CsClient> RetourneClientByReferenceOrdre(string client,string Ordre, List<int> idCentre)
            {
                try
                {
                    List<CsClient> ClientARetourner = new List<CsClient>();
                    DataTable dt = Galatee.Entity.Model.AccueilProcedures.RetourneClientByReferenceOrdre(client,Ordre, idCentre);
                    return  Entities.GetEntityListFromQuery<CsClient>(dt);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            public List<CsClient> RetourneClientByReference(string client,List<int>idCentre)
            {
                try
                {
                    List<CsClient> ClientARetourner = new List<CsClient>();
                    DataTable dt = Galatee.Entity.Model.AccueilProcedures.RetourneClientByReference(client, idCentre);
                    List<CsClient> lesClients = Entities.GetEntityListFromQuery<CsClient>(dt);
                    List<CsClient> lesDistinctClientSite = DistinctClientSite(lesClients);
                    if (lesDistinctClientSite != null && lesDistinctClientSite.Count != 0)
                    {
                        foreach (var item in lesDistinctClientSite)
                        {
                            List<CsClient> ClientSiteDifferent = lesClients.Where(t => t.FK_IDCENTRE == item.FK_IDCENTRE && t.REFCLIENT == item.REFCLIENT).ToList();
                            List<CsClient> lesDistinctClient = DistinctClientOrdre(ClientSiteDifferent);
                            foreach (CsClient items in lesDistinctClient)
                            {
                                ClientARetourner.AddRange(ClientSiteDifferent.Where(t => int.Parse(t.ORDRE) == ClientSiteDifferent.Max(y => int.Parse(y.ORDRE))).ToList());
                                break;
                            }
                        }
               
                    }
                    return ClientARetourner;

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            private List<CsClient> DistinctClientOrdre(List<CsClient> lstClient)
            {
                try
                {
                    List<CsClient> lstCentreDistClientOrdreProduit = new List<CsClient>();
                    var lstCentreDistnct = lstClient.Select(t => new {  t.FK_IDCENTRE, t.CENTRE, t.REFCLIENT,t.ORDRE , t.PRODUIT }).Distinct().ToList();
                    foreach (var item in lstCentreDistnct)
                    {
                        CsClient leClient = new CsClient()
                        {
                            FK_IDCENTRE = item.FK_IDCENTRE,
                            CENTRE = item.CENTRE,
                            REFCLIENT = item.REFCLIENT,
                            PRODUIT = item.PRODUIT
                        };
                        lstCentreDistClientOrdreProduit.Add(leClient);
                    }
                    return lstCentreDistClientOrdreProduit;
                }
                catch (Exception)
                {

                    throw;
                }
            }

            private List<CsClient> DistinctClientSite(List<CsClient> lstClient)
            {
                try
                {
                    List<CsClient> lstCentreDistClientSite = new List<CsClient>();
                    var lstCentreDistnct = lstClient.Select(t => new { t.FK_IDCENTRE, t.CENTRE, t.REFCLIENT}).Distinct().ToList();
                    foreach (var item in lstCentreDistnct)
                    {
                        CsClient leClient = new CsClient()
                        {
                            FK_IDCENTRE = item.FK_IDCENTRE,
                            CENTRE = item.CENTRE,
                            REFCLIENT = item.REFCLIENT,
                        };
                        lstCentreDistClientSite.Add(leClient);
                    }
                    return lstCentreDistClientSite;
                }
                catch (Exception)
                {

                    throw;
                }
            }

            public string ValiderDemandeRemboursementAvance(CsDemande LaDemande,List<CsLclient> lesFactureASolder)
            {
                try
                {
                    int resultTransaction = -1;
                    string DemandeID = string.Empty;
                    if (LaDemande.LaDemande.STATUT == Enumere.DemandeStatusPasseeEncaisse)
                        LaDemande.LaDemande.STATUT = Enumere.DemandeStatusPriseEnCompte;
                    using (galadbEntities transaction = new galadbEntities())
                    {
                        try
                        {
                            //AccueilProcedures.MisAjourDemande(LaDemande, transaction);
                            //new DBEncaissement().LettrageAutomatique(lesFactureASolder, transaction);
                            //resultTransaction = transaction.SaveChanges();
                            //if (resultTransaction != -1)
                            //{
                                LaDemande.LaDemande.PK_ID = transaction.DEMANDE.FirstOrDefault(d => d.NUMDEM == LaDemande.LaDemande.NUMDEM && d.CENTRE == LaDemande.LaDemande.CENTRE).PK_ID;
                                if (LaDemande.LaDemande.PK_ID == 0)
                                {
                                    using (galadbEntities tctx = new galadbEntities())
                                    {
                                        DEMANDE laDem = tctx.DEMANDE.FirstOrDefault(t => t.NUMDEM == LaDemande.LaDemande.NUMDEM);
                                        if (laDem != null)
                                            DemandeID = laDem.PK_ID + "." + LaDemande.LaDemande.NUMDEM;
                                    };
                                }
                                else
                                    DemandeID = LaDemande.LaDemande.PK_ID + "." + LaDemande.LaDemande.NUMDEM;

                            //}
                        }
                        catch (Exception es)
                        {

                            throw es;
                        }
                    };
                    return DemandeID;

                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }


            //public bool ValiderDemandeTransfert(CsDemande LaDemande)
            //{
            //    int resultTransaction = -1;
            //    try
            //    {
            //        List<CsLclient> _LstFacture = new List<CsLclient>();
            //        List<CsLclient> _LstReglement = new List<CsLclient>();
            //        List<CsLclient> lstImpaye = new List<CsLclient>();
            //        if (LaDemande.Transfert.TRANSFERIMPAYE)
            //        {
            //             _LstFacture = RetourneFactureClient(LaDemande.LaDemande.FK_IDCENTRE, LaDemande.LaDemande.CENTRE, LaDemande.LaDemande.CLIENT, LaDemande.LaDemande.ORDRE);
            //             _LstReglement = RetourneEncaissementClient(LaDemande.LaDemande.FK_IDCENTRE, LaDemande.LaDemande.CENTRE, LaDemande.LaDemande.CLIENT, LaDemande.LaDemande.ORDRE);
            //             lstImpaye = new DBEncaissement().RetourneListeFactureNonSolde(LaDemande.LaDemande.CENTRE, LaDemande.LaDemande.CLIENT, LaDemande.LaDemande.ORDRE, _LstFacture.First().FK_IDCLIENT);
            //        }

            //        using (galadbEntities transaction = new galadbEntities())
            //        {
            //            List<LCLIENT> CompteClient = ContruireCompteClient(_LstFacture, _LstReglement,LaDemande);
            //            AccueilProcedures.CreeNouveauAbonnement(LaDemande, CompteClient, transaction);
            //            AccueilProcedures.FermerAncienAbonnement(LaDemande, transaction);
            //            if (LaDemande.Transfert.TRANSFERIMPAYE)
            //            {
            //                List<TRANSCAISB> lettrageFacture = LettrageImpayeSuiteTransfert(lstImpaye, LaDemande);
            //                if (lettrageFacture != null && lettrageFacture.Count != 0)
            //                    Entities.InsertEntity<TRANSCAISB>(lettrageFacture, transaction);
            //            }
            //            if (LaDemande.LstCoutDemande != null && LaDemande.LstCoutDemande.Count != 0)
            //            {
            //                List<ELEMENTDEVIS> _LesEltDevis = new List<ELEMENTDEVIS>();
            //                if (LaDemande.EltDevis != null && LaDemande.EltDevis.Count != 0)
            //                {
            //                    _LesEltDevis = Entities.ConvertObject<ELEMENTDEVIS, ObjELEMENTDEVIS>(LaDemande.EltDevis);
            //                    _LesEltDevis.ForEach(t => t.FK_IDDEMANDE = LaDemande.LaDemande.PK_ID);
            //                    _LesEltDevis.ForEach(t => t.USERCREATION = LaDemande.LaDemande.USERCREATION);
            //                    _LesEltDevis.ForEach(t => t.NUMDEM = LaDemande.LaDemande.NUMDEM);
            //                    _LesEltDevis.ForEach(t => t.DATECREATION = LaDemande.LaDemande.DATECREATION);

            //                    Entities.InsertEntity<ELEMENTDEVIS>(_LesEltDevis, transaction);

            //                }
            //                List<RUBRIQUEDEMANDE> _LstCout = new List<RUBRIQUEDEMANDE>();
            //                {
            //                    if (LaDemande.LstCoutDemande != null && LaDemande.LstCoutDemande.Count != 0)
            //                    {
            //                        _LstCout = Entities.ConvertObject<RUBRIQUEDEMANDE, CsDemandeDetailCout>(LaDemande.LstCoutDemande);
            //                        foreach (RUBRIQUEDEMANDE item in _LstCout)
            //                        {

            //                            if (string.IsNullOrEmpty(item.NDOC))
            //                                item.NDOC = NumeroFacture(item.FK_IDCENTRE);

            //                            item.FK_IDDEMANDE = LaDemande.LaDemande.PK_ID;
            //                            item.NUMDEM = LaDemande.LaDemande.NUMDEM;
            //                            item.USERCREATION = LaDemande.LaDemande.USERCREATION;
            //                            item.DATECREATION  = System.DateTime.Now ;
            //                        }
            //                        DCLIENT leClient = transaction.DCLIENT.FirstOrDefault(i => i.NUMDEM == LaDemande.LaDemande.NUMDEM);
            //                        if (leClient != null)
            //                            leClient.NOMABON = LaDemande.LeClient.NOMABON;

            //                        Entities.InsertEntity<RUBRIQUEDEMANDE>(_LstCout, transaction);
            //                    }
            //                }
            //            }
            //            resultTransaction = transaction.SaveChanges();
            //        };
            //        return resultTransaction == -1 ? false : true;
            //    }
            //    catch (Exception  e)
            //    {
            //        throw e;
            //    }
            //}

            public string ValiderDemandeTransfert(CsDemande laDemande)
            {
                SqlCommand laCommande = DBBase.InitTransaction(ConnectionString);

                try
                {
                    laDemande.Abonne.FK_IDDEMANDE = laDemande.LaDemande.PK_ID;
                    InsertOrUpdateDAbon(laDemande.Abonne, laCommande);

                    laDemande.LeClient.FK_IDNUMDEM = laDemande.LaDemande.PK_ID;
                    InsertOrUpdateDClient(laDemande.LeClient, laCommande);

                    #region DPersonePhysique
                    if (laDemande.PersonePhysique != null)
                    {
                        laDemande.PersonePhysique.FK_IDDEMANDE = laDemande.LaDemande.PK_ID;
                        InsertOrUpdateDpersPhysique(laDemande.PersonePhysique, laCommande);
                    }
                    #endregion
                    #region SocietePrives
                    if (laDemande.SocietePrives != null)
                    {
                        laDemande.SocietePrives.FK_IDDEMANDE = laDemande.LaDemande.PK_ID;
                        InsertOrUpdateDSociete(laDemande.SocietePrives, laCommande);
                    }
                    #endregion
                    #region Dadministration
                    if (laDemande.AdministrationInstitut != null)
                    {
                        laDemande.AdministrationInstitut.FK_IDDEMANDE = laDemande.LaDemande.PK_ID;
                        InsertOrUpdateDAdmnistration_Institut(laDemande.AdministrationInstitut, laCommande);
                    }
                    #endregion
                    TransfertClient(laDemande.LaDemande.PK_ID, laDemande.LaDemande.NUMDEM, laDemande.LaDemande.MATRICULE, laDemande.Transfert.TRANSFERIMPAYE, laCommande);
                    if (laDemande.LstCoutDemande != null && laDemande.LstCoutDemande.Count != 0)
                    {
                        foreach (CsDemandeDetailCout item in laDemande.LstCoutDemande)
                        {
                            item.FK_IDDEMANDE = laDemande.LaDemande.PK_ID;
                            item.NUMDEM = laDemande.LaDemande.NUMDEM;
                            item.FK_IDCENTRE = laDemande.Transfert.FK_IDCENTRETRANSFERT;
                            InsertOrUpdateRubriqueDemande(item, laCommande);
                        }
                    }
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






            public List<LCLIENT> ContruireCompteClient(List<CsLclient> _LstFacture, List<CsLclient> _LstEncaissement,CsDemande laDemande)
            {
                DBEncaissement db = new DBEncaissement();
                List<LCLIENT> _LstFactureFinal = new List<LCLIENT>();
                if (_LstFacture != null && ((_LstFacture != null && _LstFacture.Count != 0)))
                {
                    if (_LstFacture != null && _LstFacture.Count != 0)
                        foreach (var item in _LstFacture)
                        {
                            LCLIENT laFacture = db.RetourneFactureDemande(item);
                            laFacture.FK_IDADMUTILISATEUR = 1;
                            List<TRANSCAISB> lstReglement = new List<TRANSCAISB>();
                            List<CsLclient> lesReglement = _LstEncaissement.Where(p => p.REFEM == item.REFEM && p.NDOC == item.NDOC).ToList();
                            if (lesReglement != null && lesReglement.Count != 0)
                                lstReglement = Entities.ConvertObject <TRANSCAISB, CsLclient>(lesReglement);
                            lstReglement.ForEach(t => t.USERCREATION = laDemande.LaDemande.USERCREATION);
                            lstReglement.ForEach(t => t.DATECREATION = System.DateTime.Today);
                            lstReglement.ForEach(t => t.DATECREATION  = System.DateTime.Today );
                            lstReglement.ForEach(t => t.FK_IDCENTRE  = laDemande.Transfert.FK_IDCENTRETRANSFERT );
                            laFacture.TRANSCAISB =lstReglement;
                            _LstFactureFinal.Add(laFacture);
                            _LstFactureFinal.ForEach(t => t.DATECREATION = System.DateTime.Today);
                            _LstFactureFinal.ForEach(t => t.USERCREATION = laDemande.LaDemande.USERCREATION );
                        }
                }
                return _LstFactureFinal;
            }

            public List<TRANSCAISB > LettrageImpayeSuiteTransfert (List<CsLclient> _LstFacture, CsDemande laDemande)
            {
                galadbEntities ctxinte = new galadbEntities();
                COPER leCoper = ctxinte.COPER.FirstOrDefault(t => t.CODE == Enumere.CoperSoldeSuiteTransfer);
                MODEREG leModreg = ctxinte.MODEREG.FirstOrDefault(t => t.CODE == Enumere.ModePayementEspece ); 
                List<TRANSCAISB> _LstReglemntFinal = new List<TRANSCAISB>();
                if (_LstFacture != null && ((_LstFacture != null && _LstFacture.Count != 0)))
                {
                    if (_LstFacture != null && _LstFacture.Count != 0)
                        foreach (var item in _LstFacture)
                        {
                            TRANSCAISB leRegt = new TRANSCAISB();
                            leRegt = Entities.ConvertObject<TRANSCAISB, CsLclient>(item);
                            leRegt.USERCREATION = laDemande.LaDemande.USERCREATION;
                            leRegt.DATECREATION = System.DateTime.Now ;
                            leRegt.MONTANT = item.SOLDEFACTURE ;
                            leRegt.DTRANS = System.DateTime.Today;
                            leRegt.FK_IDLCLIENT = item.PK_ID;
                            leRegt.COPER = Enumere.CoperSoldeSuiteTransfer;
                            leRegt.FK_IDCOPER = leCoper.PK_ID;
                            leRegt.MODEREG = leModreg.CODE;
                            leRegt.FK_IDMODEREG  = leModreg.PK_ID ;
                            _LstReglemntFinal.Add(leRegt);
                        }
                }
                return _LstReglemntFinal;
            }

            public List<TRANSCAISB> LettrageImpaye(List<CsLclient> _LstFacture, CsLclient leReglement, CsDemande laDemande)
            {
                galadbEntities ctxinte = new galadbEntities();
                COPER leCoper = ctxinte.COPER.FirstOrDefault(t => t.CODE == Enumere.CoperRGT);
                MODEREG leModreg = ctxinte.MODEREG.FirstOrDefault(t => t.CODE == leReglement.MODEREG );
                if (leModreg == null) leModreg = ctxinte.MODEREG.FirstOrDefault(t => t.CODE == Enumere.ModePayementEspece );
                List<TRANSCAISB> _LstReglemntFinal = new List<TRANSCAISB>();
                decimal? leMontantAvc = leReglement.MONTANT ;
                
                if (_LstFacture != null && ((_LstFacture != null && _LstFacture.Count != 0)))
                {
                    if (_LstFacture != null && _LstFacture.Count != 0)
                    {
                        foreach (var item in _LstFacture)
                        {
                            if (leMontantAvc <= 0) break;
                            decimal? MontantPaye = 0;
                            if (item.SOLDEFACTURE >= leMontantAvc)
                                MontantPaye = leMontantAvc;
                            else MontantPaye = item.SOLDEFACTURE;

                            TRANSCAISB leRegt = new TRANSCAISB();
                            leRegt = Entities.ConvertObject<TRANSCAISB, CsLclient>(item);
                            leRegt.USERCREATION = laDemande.LaDemande.USERCREATION;
                            leRegt.DATECREATION = System.DateTime.Today;
                            leRegt.DTRANS  = System.DateTime.Today;
                            leRegt.FK_IDLCLIENT = item.PK_ID;
                            leRegt.COPER = Enumere.CoperRGT;
                            leRegt.MONTANT = MontantPaye;
                            leRegt.FK_IDCOPER = leCoper.PK_ID;
                            leRegt.MODEREG = leModreg.CODE;
                            leRegt.FK_IDMODEREG = leModreg.PK_ID;
                            _LstReglemntFinal.Add(leRegt);

                            leMontantAvc = leMontantAvc - MontantPaye;
                        }
                    }
                }
                return _LstReglemntFinal;
            }
            public bool LettrageSuiteAPDP(CsDemande LaDemande)
            {
                try
                {
                    int resu = -1;
                    galadbEntities ctx = new galadbEntities();
                    TRANSCAISB LeRAC = new TRANSCAISB();
                    ABON _LeAbonnement = ctx.ABON.FirstOrDefault(t => t.FK_IDCENTRE == LaDemande.LaDemande.FK_IDCENTRE && t.CENTRE == LaDemande.LaDemande.CENTRE && t.CLIENT == LaDemande.LaDemande.CLIENT && LaDemande.LaDemande.ORDRE == t.ORDRE);
                    if (_LeAbonnement != null && _LeAbonnement.AVANCE != null && _LeAbonnement.AVANCE != 0)
                    {
                        string refem = _LeAbonnement.DAVANCE.Value.Year.ToString() + _LeAbonnement.DAVANCE.Value.Month.ToString("00");
                        LeRAC = ctx.TRANSCAISB.FirstOrDefault(t => t.FK_IDCENTRE == _LeAbonnement.FK_IDCENTRE &&
                                                                        t.CENTRE == _LeAbonnement.CENTRE &&
                                                                        t.CLIENT == _LeAbonnement.CLIENT &&
                                                                        _LeAbonnement.ORDRE == t.ORDRE &&
                                                                        t.REFEM == refem &&
                                                                        t.MONTANT == _LeAbonnement.AVANCE &&
                                                                        t.COPER == Enumere.CoperRAC);
                    }
                    if (LeRAC != null && LeRAC.MONTANT != 0 && LeRAC.MONTANT != null)
                    {
                        decimal? MontantLettrage = LeRAC.MONTANT;
                        List<string> lstCoperDevis = new List<string>();
                        lstCoperDevis.Add(Enumere.CoperCAU);
                        lstCoperDevis.Add(Enumere.CoperFPO);
                        lstCoperDevis.Add(Enumere.CoperFDO);

                        COPER leCoper = ctx.COPER.FirstOrDefault(t => t.CODE == Enumere.CoperRGT);
                        COPER leCoperRAd = ctx.COPER.FirstOrDefault(t => t.CODE == Enumere.CoperRAD);
                        COPER leCoperUdc = ctx.COPER.FirstOrDefault(t => t.CODE == Enumere.CoperUDC);
                        LIBELLETOP leTope = ctx.LIBELLETOP.FirstOrDefault(t => t.CODE == Enumere.TopCaisse);
                        ADMUTILISATEUR leuser = ctx.ADMUTILISATEUR.FirstOrDefault(t => t.MATRICULE == LaDemande.LaDemande.MATRICULE);
                        List<TRANSCAISB> lstReglement = new List<TRANSCAISB>();
                        List<CsLclient> lstFactiure = new DBEncaissement().RetourneListeFactureNonSolde(LaDemande.LaDemande.CENTRE, LaDemande.LaDemande.CLIENT, LaDemande.LaDemande.ORDRE, LaDemande.LaDemande.FK_IDCLIENT.Value);
                        if (lstFactiure != null && lstFactiure.Count != 0)
                        {
                            foreach (CsLclient item in lstFactiure.OrderBy(t => t.COPER))
                            {
                                if (MontantLettrage == 0) break;
                                decimal? MontantRegle = 0;
                                if (item.SOLDEFACTURE <= MontantLettrage)
                                    MontantRegle = item.SOLDEFACTURE ;
                                else
                                    MontantRegle = MontantLettrage;

                                TRANSCAISB leRgt = Entities.ConvertObject<TRANSCAISB, CsLclient>(item);
                                leRgt.FK_IDCAISSIERE = null;
                                leRgt.DATECREATION = System.DateTime.Now;
                                leRgt.USERCREATION = LaDemande.LaDemande.MATRICULE;
                                leRgt.COPER = leCoperUdc.CODE;
                                leRgt.MONTANT = MontantRegle;
                                leRgt.FK_IDCOPER = leCoperUdc.PK_ID;
                                leRgt.DTRANS = System.DateTime.Today;
                                leRgt.ACQUIT = Enumere.AcquitLettrageAuto;
                                lstReglement.Add(leRgt);
                                MontantLettrage = MontantLettrage - MontantRegle;

                                if (lstCoperDevis.Contains(item.COPER))
                                {
                                    RUBRIQUEDEMANDE laCaut = ctx.RUBRIQUEDEMANDE.FirstOrDefault(t => t.CENTRE == item.CENTRE &&
                                                                                                   t.CLIENT == item.CLIENT &&
                                                                                                   t.ORDRE == item.ORDRE &&
                                                                                                   t.REFEM == item.REFEM &&
                                                                                                   t.NDOC == item.NDOC &&
                                                                                                   t.COPER == item.COPER);
                                    if (laCaut != null && laCaut.MONTANTHT != null && laCaut.MONTANTHT != 0)
                                        laCaut.MONTANTHT = laCaut.MONTANTHT - MontantRegle;

                                }
                            }
                            CsLclient lelClient = new CsLclient()
                            {
                                FK_IDCENTRE = LaDemande.LaDemande.FK_IDCENTRE,
                                CENTRE = LaDemande.LaDemande.CENTRE,
                                CLIENT = LaDemande.LaDemande.CLIENT,
                                ORDRE = LaDemande.LaDemande.ORDRE,
                                FK_IDCLIENT = LaDemande.LaDemande.FK_IDCLIENT.Value
                            };
                            LCLIENT leMtEquilbre = Galatee.Tools.Utility.ConvertEntity<LCLIENT, CsLclient>(lelClient);

                            leMtEquilbre.NDOC = LeRAC.NDOC;
                            leMtEquilbre.REFEM = LeRAC.REFEM;
                            leMtEquilbre.COPER = leCoperRAd.CODE;
                            leMtEquilbre.DATECREATION = System.DateTime.Now;
                            leMtEquilbre.USERCREATION = LaDemande.LaDemande.MATRICULE;
                            leMtEquilbre.MONTANT = LeRAC.MONTANT;
                            leMtEquilbre.DENR = System.DateTime.Today;
                            leMtEquilbre.TOP1 = Enumere.TopCaisse;
                            leMtEquilbre.FK_IDLIBELLETOP = leTope.PK_ID;
                            leMtEquilbre.FK_IDADMUTILISATEUR = leuser.PK_ID;
                            leMtEquilbre.FK_IDCOPER = leCoperRAd.PK_ID;
                            leMtEquilbre.FK_IDMOTIFCHEQUEINPAYE = null;
                            leMtEquilbre.FK_IDMORATOIRE = null;
                            leMtEquilbre.FK_IDPOSTE = null;
                            Entities.InsertEntity<LCLIENT>(leMtEquilbre, ctx);
                            Entities.InsertEntity<TRANSCAISB>(lstReglement, ctx);
                                
                           resu = ctx.SaveChanges();
                            ctx.Dispose();
                        }
                    }
                    return resu == -1 ? false : true;

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            public void LettrageFactureSuiteRemboursement(CsLclient leClient, galadbEntities context)
            {
                galadbEntities ctx = new galadbEntities();
                TRANSCAISB LeRAC = new TRANSCAISB();
                ABON _LeAbonnement = ctx.ABON.FirstOrDefault(t => t.FK_IDCENTRE == leClient.FK_IDCENTRE && t.CENTRE == leClient.CENTRE && t.CLIENT == leClient.CLIENT && leClient.ORDRE == t.ORDRE);
                if (_LeAbonnement != null && leClient.AVANCE != null && leClient.AVANCE != 0)
                {
                    string refem = _LeAbonnement.DAVANCE.Value.Year.ToString() + _LeAbonnement.DAVANCE.Value.Month.ToString("00");
                    LeRAC = ctx.TRANSCAISB.FirstOrDefault(t => t.FK_IDCENTRE == leClient.FK_IDCENTRE &&
                                                                    t.CENTRE == leClient.CENTRE &&
                                                                    t.CLIENT == leClient.CLIENT &&
                                                                    leClient.ORDRE == t.ORDRE &&
                                                                    t.REFEM == refem &&
                                                                    t.MONTANT == _LeAbonnement.AVANCE &&
                                                                    t.COPER == Enumere.CoperRAC );
                }
                if (LeRAC != null && LeRAC.MONTANT != 0 && LeRAC.MONTANT != null)
                {
                    decimal? MontantLettrage = LeRAC.MONTANT;
                    List<string> lstCoperDevis = new List<string>();
                    lstCoperDevis.Add(Enumere.CoperCAU);
                    lstCoperDevis.Add(Enumere.CoperFPO);
                    lstCoperDevis.Add(Enumere.CoperFDO);

                    COPER leCoper = ctx.COPER.FirstOrDefault(t => t.CODE == Enumere.CoperRGT);
                    COPER leCoperRAd = ctx.COPER.FirstOrDefault(t => t.CODE == Enumere.CoperRAD);
                    COPER leCoperUdc = ctx.COPER.FirstOrDefault(t => t.CODE == Enumere.CoperUDC);
                    LIBELLETOP leTope = ctx.LIBELLETOP.FirstOrDefault(t => t.CODE == Enumere.TopCaisse);
                    ADMUTILISATEUR leuser = ctx.ADMUTILISATEUR.FirstOrDefault(t => t.MATRICULE == leClient.MATRICULE);
                    List<TRANSCAISB> lstReglement = new List<TRANSCAISB>();
                    List<CsLclient> lstFactiure = new DBEncaissement().RetourneListeFactureNonSolde(leClient.CENTRE, leClient.CLIENT, leClient.ORDRE, leClient.FK_IDCLIENT);
                    if (lstFactiure != null && lstFactiure.Count != 0)
                    {
                        foreach (CsLclient item in lstFactiure.OrderBy(t=>t.COPER ))
                        {
                            if (MontantLettrage == 0) break;
                            decimal? MontantRegle =  item.SOLDEFACTURE  ;
                            if (item.SOLDEFACTURE < MontantLettrage)
                                MontantRegle = MontantLettrage;

                            TRANSCAISB leRgt = Entities.ConvertObject<TRANSCAISB, CsLclient>(item);
                            leRgt.FK_IDCAISSIERE = null;
                            leRgt.DATECREATION = System.DateTime.Now;
                            leRgt.USERCREATION = leClient.MATRICULE;
                            leRgt.COPER = leCoperUdc.CODE;
                            leRgt.MONTANT = item.SOLDEFACTURE;
                            leRgt.FK_IDCOPER = leCoperUdc.PK_ID;
                            leRgt.DTRANS = System.DateTime.Today;
                            leRgt.ACQUIT = Enumere.AcquitLettrageAuto;
                            lstReglement.Add(leRgt);


                            if (lstCoperDevis.Contains(item.COPER))
                            {
                                RUBRIQUEDEMANDE laCaut = ctx.RUBRIQUEDEMANDE.FirstOrDefault(t => t.CENTRE == item.CENTRE &&
                                                                                               t.CLIENT == item.CLIENT &&
                                                                                               t.ORDRE == item.ORDRE &&
                                                                                               t.REFEM == item.REFEM &&
                                                                                               t.NDOC == item.NDOC &&
                                                                                               t.COPER == item.COPER );
                                laCaut.MONTANTHT = laCaut.MONTANTHT - MontantLettrage;

                            }
                        }

                        LCLIENT leMtEquilbre = Galatee.Tools.Utility.ConvertEntity<LCLIENT, CsLclient>(leClient);
                        leMtEquilbre.COPER = leCoperRAd.CODE;
                        leMtEquilbre.DATECREATION = System.DateTime.Now;
                        leMtEquilbre.USERCREATION = leClient.MATRICULE;
                        leMtEquilbre.MONTANT = LeRAC.MONTANT ;
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
            }

        

            //public bool LettrageSuiteAPDP(CsDemande LaDemande, List<CsLclient> _LstFacture)
            //{
            //    try
            //    {
            //        List<CsEditionDevis> LstDesRubriqueDevis = new List<CsEditionDevis>();
            //        List<string> lstCoperDevis = new List<string>();
            //        lstCoperDevis.Add( Enumere.CoperCAU);
            //        lstCoperDevis.Add( Enumere.CoperTRV);
            //        lstCoperDevis.Add( Enumere.CoperFPO);
            //        lstCoperDevis.Add(Enumere.CoperFDO);
            //        //lstCoperDevis.Add(Enumere.CoperFact);

            //        galadbEntities leContext = new galadbEntities();

            //        decimal? leSolde = _LstFacture.Sum(t => t.SOLDEFACTURE );
            //        List<CsLclient> lfac = _LstFacture.Where(t => t.COPER != Enumere.CoperRembAvance).OrderBy(t => t.COPER).ToList();
            //        List<CsLclient> lfacDevix = _LstFacture.Where(t => lstCoperDevis.Contains(t.COPER)).ToList();
            //        CsLclient lReglement = _LstFacture.FirstOrDefault(t => t.COPER == Enumere.CoperRembAvance);
            //        if (lReglement != null)
            //        {
            //            List<TRANSCAISB> lstLetrage = LettrageImpaye(lfac, lReglement, LaDemande);
            //            if (lstLetrage != null && lstLetrage.Count != 0)
            //            {
            //                LCLIENT  leMtEquilbre = Galatee.Tools.Utility.ConvertEntity<LCLIENT , CsLclient>(lReglement);
            //                leMtEquilbre.COPER = Enumere.CoperRAD;
            //                leMtEquilbre.DATECREATION = System.DateTime.Now;
            //                leMtEquilbre.USERCREATION = LaDemande.LaDemande.MATRICULE;
            //                leMtEquilbre.DENR  = System.DateTime.Today ;
            //                leMtEquilbre.TOP1 = Enumere.TopCaisse;
            //                leMtEquilbre.FK_IDLIBELLETOP = leContext.LIBELLETOP.FirstOrDefault(t => t.CODE == Enumere.TopCaisse).PK_ID;
            //                leMtEquilbre.FK_IDADMUTILISATEUR = leContext.ADMUTILISATEUR.FirstOrDefault(t => t.MATRICULE == LaDemande.LaDemande.MATRICULE).PK_ID;
            //                leMtEquilbre.FK_IDCOPER = leContext.COPER.FirstOrDefault(t => t.CODE == Enumere.CoperRAD).PK_ID;
            //                leMtEquilbre.FK_IDMOTIFCHEQUEINPAYE = null;
            //                leMtEquilbre.FK_IDMORATOIRE  = null;
            //                leMtEquilbre.FK_IDPOSTE  = null;
            //                Entities.InsertEntity<LCLIENT>(leMtEquilbre, leContext);
            //            }
            //            Entities.InsertEntity<TRANSCAISB>(lstLetrage, leContext);
            //        }
            //        int res = -1;
            //        res = leContext.SaveChanges();
            //        return res == -1 ? false : true;
            //    }
            //    catch (Exception ex)
            //    {

            //        throw ex;
            //    }

            //}
            public List<ObjELEMENTDEVIS> RetourneElementDEvisFromIdDemande(List<int> IdDemande)
            {
                List<ObjELEMENTDEVIS> lstMateriel = new List<ObjELEMENTDEVIS>();
                //foreach (int item in IdDemande)
                //{
                //    DataTable dtElt = DevisProcedures.DEVIS_ELEMENTDEVIS_MaterielByDevisById(IdDemande);
                //    lstMateriel.AddRange(Entities.GetEntityListFromQuery<ObjELEMENTDEVIS>(dtElt));
                //}
                DataTable dtElt = DevisProcedures.DEVIS_ELEMENTDEVIS_MaterielByDevisById(IdDemande);
                lstMateriel.AddRange(Entities.GetEntityListFromQuery<ObjELEMENTDEVIS>(dtElt));
                return lstMateriel;
            }
            public bool InsertLiaisonCompteur(CsDemandeBase _LaDemande, CsCanalisation leCompteur,CsEvenement leEvt)
            {
                try
                {
                        int Resultat = -1;
                        DEVENEMENT Evt = Entities.ConvertObject<DEVENEMENT, CsEvenement>(leEvt);
                        DCANALISATION canal = new DCANALISATION()
                        {
                            CENTRE = _LaDemande.CENTRE,
                            CLIENT = _LaDemande.CLIENT,
                            NUMDEM = _LaDemande.NUMDEM,
                            PRODUIT = _LaDemande.PRODUIT,
                            PROPRIO = "1",
                            FK_IDPRODUIT = _LaDemande.FK_IDPRODUIT.Value,
                            FK_IDDEMANDE = _LaDemande.PK_ID,
                            FK_IDCENTRE = _LaDemande.FK_IDCENTRE,
                            POSE = System.DateTime.Now,
                            USERCREATION = _LaDemande.USERMODIFICATION ,
                            DATECREATION = System.DateTime.Now,
                            FK_IDPROPRIETAIRE = 1,
                        };
                        if (_LaDemande.FK_IDREGLAGECOMPTEUR != null)
                        {
                            canal.FK_IDREGLAGECOMPTEUR = _LaDemande.FK_IDREGLAGECOMPTEUR;
                            canal.REGLAGECOMPTEUR = _LaDemande.REGLAGECOMPTEUR;
                        }
                        TbCompteurBTA leCpt = new TbCompteurBTA()
                        {
                            ANNEEFAB = leCompteur.ANNEEFAB,
                            CADRAN = leCompteur.CADRAN,
                            FK_IDCALIBRECOMPTEUR = leCompteur.FK_IDCALIBRE,
                            EtatCompteur_ID = leCompteur.FK_IDETATCOMPTEUR,
                            FK_IDMARQUECOMPTEUR = leCompteur.FK_IDMARQUECOMPTEUR.Value,
                            FK_IDPRODUIT = leCompteur.FK_IDPRODUIT,
                            FK_IDTYPECOMPTEUR = leCompteur.FK_IDTYPECOMPTEUR,
                            MARQUE = leCompteur.MARQUE,
                            Numero_Compteur = leCompteur.NUMERO,
                            StatutCompteur = "Affecté",
                            TYPE_COMPTEUR = leCompteur.TYPECOMPTEUR,
                            Type_Compteur_ID = leCompteur.FK_IDTYPECOMPTEUR,
                            FONCTIONNEMENT = "1",
                            USERCREATION = _LaDemande.USERMODIFICATION,
                            DATECREATION = System.DateTime.Now
                        };
                        MAGASINVIRTUEL leCptMgzV = new MAGASINVIRTUEL()
                        {
                            ANNEEFAB = leCompteur.ANNEEFAB,
                            CADRAN = leCompteur.CADRAN,
                            MARQUE = leCompteur.MARQUE,
                            NUMERO = leCompteur.NUMERO,
                            FONCTIONNEMENT = "1",
                            ETAT = Enumere.CompteurAffecte ,
                            FK_IDCALIBRECOMPTEUR = leCompteur.FK_IDCALIBRE,
                            FK_IDMARQUECOMPTEUR = leCompteur.FK_IDMARQUECOMPTEUR.Value,
                            FK_IDPRODUIT = leCompteur.FK_IDPRODUIT,
                            FK_IDTYPECOMPTEUR = leCompteur.FK_IDTYPECOMPTEUR.Value,
                            FK_IDCENTRE = leCompteur.FK_IDCENTRE,
                            USERCREATION = _LaDemande.USERMODIFICATION,
                            DATECREATION = System.DateTime.Now
                        };
                        if (_LaDemande.PRODUIT != Enumere.ElectriciteMT)
                        {
                           
                            using(galadbEntities ctx = new galadbEntities())
	                        {
                                Entities.InsertEntity<TbCompteurBTA>(leCpt, ctx);
                                Entities.InsertEntity<MAGASINVIRTUEL>(leCptMgzV, ctx);
                                canal.FK_IDMAGAZINVIRTUEL = leCptMgzV.PK_ID;
                                canal.POINT = 1;
                                Entities.InsertEntity<DCANALISATION>(canal, ctx);
                                Evt.FK_IDCANALISATION = canal.PK_ID;
                                Evt.FK_IDDEMANDE = _LaDemande.PK_ID;
                                Evt.FK_IDCAS = ctx.CASIND.FirstOrDefault(t => t.CODE == Evt.CAS).PK_ID;
                                Entities.InsertEntity<DEVENEMENT>(Evt, ctx);
                               Resultat= ctx.SaveChanges();
	                        }
                        }
                        else
                        {
                            List<DCANALISATION> ListCanalisation = new List<DCANALISATION>();
                            for (int i = 1; i <= 6; i++)
                            {
                                DCANALISATION lesCpt = Galatee.Tools.Utility.RetourneCopyObjet<DCANALISATION>(canal);
                                lesCpt.POINT = i;
                                ListCanalisation.Add(lesCpt);
                            }
                            using (galadbEntities ctx = new galadbEntities())
                            {
                                Entities.InsertEntity<TbCompteurBTA>(leCpt, ctx);
                                Entities.InsertEntity<MAGASINVIRTUEL>(leCptMgzV, ctx);
                                canal.FK_IDMAGAZINVIRTUEL = leCptMgzV.PK_ID;
                                Entities.InsertEntity<DCANALISATION>(ListCanalisation, ctx);
                                Evt.FK_IDCANALISATION = canal.PK_ID;
                                Evt.FK_IDDEMANDE  = _LaDemande.PK_ID;
                                Evt.FK_IDCAS = ctx.CASIND.FirstOrDefault(t => t.CODE == Evt.CAS).PK_ID;
                                Entities.InsertEntity<DEVENEMENT>(Evt, ctx);
                               Resultat= ctx.SaveChanges();
                            }
                        }
                        return Resultat == -1?false : true ;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            public CsDemande  InsertLiaisonCompteurMt(CsDemande  _LaDemande)
            {
                try
                {
                    int Resultat = -1;
                    CsDemande laDemande = new CsDemande();
                    List<DEVENEMENT> Evt = Entities.ConvertObject<DEVENEMENT, CsEvenement>(_LaDemande.LstEvenement );
                    List<DCANALISATION> ListCanalisation = Entities.ConvertObject<DCANALISATION, CsCanalisation>(_LaDemande.LstCanalistion );
                    CsCanalisation leCompteur = _LaDemande.LstCanalistion.First();
                    List<POSE_SCELLE_DEMANDE> _LstOrganeScelleDemande = new List<POSE_SCELLE_DEMANDE>();
                    if (_LaDemande.LstOrganeScelleDemande != null && _LaDemande.LstOrganeScelleDemande.Count != 0)
                    {
                        _LstOrganeScelleDemande = new List<POSE_SCELLE_DEMANDE>();
                        foreach (var item in _LaDemande.LstOrganeScelleDemande)
                        {
                            POSE_SCELLE_DEMANDE obj = new POSE_SCELLE_DEMANDE();
                            obj.PK_ID = item.PK_ID;
                            obj.FK_IDDEMANDE = item.FK_IDDEMANDE;
                            obj.FK_IDORGANE_SCELLABLE = item.FK_IDORGANE_SCELLABLE;
                            obj.NOMBRE = item.NOMBRE;
                            obj.FK_IDDEMANDE = _LaDemande.LaDemande.PK_ID ;
                            obj.NUM_SCELLE = item.NUM_SCELLE;
                            obj.CERTIFICAT = item.CERTIFICAT;
                            _LstOrganeScelleDemande.Add(obj);
                        }
                    }
                    using (galadbEntities ctx = new galadbEntities())
                    {
                        Entities.InsertEntity<DCANALISATION>(ListCanalisation, ctx);
                        Evt.ForEach(i => i.FK_IDDEMANDE = _LaDemande.LaDemande.PK_ID);
                        Evt.ForEach(i => i.FK_IDCANALISATION  = null );
                        Evt.ForEach(u=>u.FK_IDCAS = ctx.CASIND.FirstOrDefault(t => t.CODE == u.CAS).PK_ID);
                        Entities.InsertEntity<DEVENEMENT>(Evt, ctx);
                        if (_LstOrganeScelleDemande != null && _LstOrganeScelleDemande.Count != 0)
                        Entities.InsertEntity<POSE_SCELLE_DEMANDE>(_LstOrganeScelleDemande, ctx);
                        
                        Resultat = ctx.SaveChanges();

                        laDemande.LstCanalistion = new List<CsCanalisation>();
                        laDemande.LstEvenement  = new List<CsEvenement >();
                        foreach (var item in _LaDemande.LstCanalistion)
		                   item.PK_ID = ListCanalisation.FirstOrDefault(o=>o.POINT == item.POINT).PK_ID ;

                        foreach (var items in _LaDemande.LstEvenement)
		                    items.PK_ID = Evt.FirstOrDefault(o=>o.POINT == items.POINT).PK_ID ;

                        laDemande.LstCanalistion.AddRange(_LaDemande.LstCanalistion);
                        laDemande.LstEvenement.AddRange(_LaDemande.LstEvenement);
                    }

                    return Resultat == -1 ? null  : laDemande;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            public  CsClient  VerifieMatriculeAgent(string MatriculeAgent)
            {
                try
                {
                    DataTable dt = Galatee.Entity.Model.AccueilProcedures.RetourneAbonnementAgent(MatriculeAgent);
                    return  Entities.GetEntityFromQuery<CsClient >(dt);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            public List<CsRubriqueDevis> RetourneTypeMateriel()
            {
                try
                {
                    DataTable dt = Galatee.Entity.Model.AccueilProcedures.RetourneTypeMateriel();
                    return Entities.GetEntityListFromQuery<CsRubriqueDevis>(dt);
                }
                catch (Exception ex)
                {
                    throw new Exception("RetourneTypeMateriel :" + ex.Message);
                }
            }


            #region Transition
            public CsDemandeBase RetourneLaDemande(string numdem)
            {
                try
                {
                    DataTable dt = Galatee.Entity.Model.AccueilProcedures.RetourneLaDemande(numdem);
                    return Entities.GetEntityListFromQuery<CsDemandeBase>(dt).FirstOrDefault();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            } 
            public bool  ValiderCreation(CsDemande pDemandeDevis)
            {
                try
                {
                    int resulta = -1 ;
                    if (ValiderDemande(pDemandeDevis))
                    {
                        using (galadbEntities transaction = new galadbEntities())
                        {
                            AccueilProcedures.MiseAjoursAbonBrtAbonnementTransition(pDemandeDevis, transaction);
                            resulta = transaction.SaveChanges();
                        }
                    }
                    return resulta == -1 ? false : true;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            public CsDemandeBase MiseAjoursAbonBrtCreation(CsDemande pDemandeDevis)
            {
                try
                {
                    int resulta = -1;
                    if (ValiderDemande(pDemandeDevis))
                    {
                        using (galadbEntities transaction = new galadbEntities())
                        {
                            AccueilProcedures.MiseAjoursAbonBrtCreation(pDemandeDevis, transaction);
                            resulta = transaction.SaveChanges();
                        }
                    }
                    if (pDemandeDevis != null && !string.IsNullOrEmpty(pDemandeDevis.LaDemande.MOTIF)) return pDemandeDevis.LaDemande ;
                    else
                    return resulta == -1 ? null : pDemandeDevis.LaDemande ;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            public List<CsDemandeBase> GetDemandeByTypdeDemande(string Typedemande)
            {
                try
                {
                    return Entities.GetEntityListFromQuery <CsDemandeBase>(AccueilProcedures.GetDemandeByTypdeDemande(Typedemande));
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            public bool? InsererCompteurEvtTransition(CsDemande demande)
            {
                try
                {
                    galadbEntities leContext = new galadbEntities();
                    bool? resultat = AccueilProcedures.InsererCompteurEvtTransition(demande, leContext);
                    var lstEvenement = demande.LstEvenement.Where(c => c.LOTRI == c.CENTRE + Enumere.LotriChangementCompteur).ToList();
                    bool result = new DBIndex().CreationCtarCompt(lstEvenement.First().FK_IDABON.Value, lstEvenement.First().LOTRI, lstEvenement.First().PERIODE, lstEvenement.First().USERCREATION, lstEvenement.First().FK_IDCENTRE, lstEvenement.First().FK_IDPRODUIT, lstEvenement.First().DATEEVT);
                    new DbFacturation().CreationLotIsole(lstEvenement.ToList());
                    return resultat;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            //public static void EcrireFichier(string Message, string CheminLog)
            //{

            //    string Buffer = "";
            //    FileInfo Fichier = new FileInfo(CheminLog);

            //    if (Fichier.Exists) // on verifie ke le fichier existe
            //    {
            //        StreamReader Lecture = new StreamReader(CheminLog, ASCIIEncoding.Default); // on ouvre le fichier
            //        Buffer = Lecture.ReadToEnd(); // on met la totalité du fichier dans une variable
            //        Lecture.Close(); // on ferme
            //    }

            //    if (Buffer == null || Buffer == "") // on verifie si y a kelke chose dans le fichier, si oui...
            //    {
            //        StreamWriter Ecriture = new StreamWriter(CheminLog, false, ASCIIEncoding.Default); // le boolean à false permet d'écraser le fichier existant
            //        Ecriture.Write(Message + "\r\n"); // on écrit la variable et sa valeur
            //        Ecriture.Close(); // on ferme
            //    }
            //    else // si non...
            //    {
            //        StreamWriter Ecriture = new StreamWriter(CheminLog, true, ASCIIEncoding.Default); // le boolean à false permet d'ajouter un ligne sans écraser le fichier
            //        Ecriture.Write(Message + "\r\n"); // on ajoute la variable plus la valeur (un saut a la ligne avant)
            //        Ecriture.Close(); // on ferme
            //    }
            //}


            public CsDemande RetourneDetailDemandeFromDEvisTransition(CsDemandeBase _laDemande)
            {


                try
                {
                    List<CsDemande> _LesDemande = new List<CsDemande>();

                    CsDemande _LaDemande = new CsDemande();
                    _LaDemande.LaDemande = _laDemande;

                    DataTable dt = AccueilProcedures.RetourneDAG(_laDemande.PK_ID);
                    _LaDemande.Ag = Entities.GetEntityFromQuery<CsAg>(dt);

                    DataTable dtc = AccueilProcedures.RetourneDClient(_laDemande.PK_ID);
                    _LaDemande.LeClient = Entities.GetEntityFromQuery<CsClient>(dtc);

                    DataTable dtA = AccueilProcedures.RetourneDAbon(_laDemande.PK_ID);
                    _LaDemande.Abonne = Entities.GetEntityFromQuery<CsAbon>(dtA);

                    DataTable dtbrt = AccueilProcedures.RetourneDBrt(_laDemande.PK_ID);
                    _LaDemande.Branchement = Entities.GetEntityFromQuery<CsBrt>(dtbrt);
                   
                    return _LaDemande;
                }
                catch (Exception ex)
                {

                    throw ex;
                }
            }
            //public bool ValiderActionSurDemande(CsDemande laDemande)
            //{
            //    try
            //    {
            //        using (galadbEntities ctx = new galadbEntities())
            //        {
            //            if (laDemande.LaDemande.ISSUPPRIMERCOUT)
            //            {
            //                List<RUBRIQUEDEMANDE> lstRubrique = ctx.RUBRIQUEDEMANDE.Where(o => o.NUMDEM == laDemande.LaDemande.NUMDEM).ToList();
            //                foreach (var item in lstRubrique)
            //                {
            //                    CsDemandeDetailCout leCout = laDemande.LstCoutDemande.FirstOrDefault(i => i.COPER == item.COPER);
            //                    if (leCout != null)
            //                    {
            //                        item.MONTANTHT = leCout.MONTANTHT;
            //                        item.MONTANTTAXE = leCout.MONTANTTAXE;
            //                    }
            //                    if (leCout.MONTANTTTC == 0)
            //                    {
            //                        List<ELEMENTDEVIS> lst = ctx.ELEMENTDEVIS.Where(u => u.NUMDEM == laDemande.LaDemande.NUMDEM && u.FK_IDCOPER == item.FK_IDCOPER).ToList();
            //                        if (lst != null && lst.Count != 0)
            //                        {
            //                            lst.ForEach(u => u.MONTANTHT = leCout.MONTANTHT);
            //                            lst.ForEach(u => u.MONTANTTAXE = leCout.MONTANTTAXE);
            //                            lst.ForEach(u => u.MONTANTTTC = leCout.MONTANTTTC);
            //                        }
            //                    }
            //                }
            //                if (laDemande.LaDemande.ISPASSERCAISSE)
            //                {
            //                    DEMANDE demande = ctx.DEMANDE.FirstOrDefault(i => i.NUMDEM == laDemande.LaDemande.NUMDEM);
            //                    if (demande != null )
            //                    demande.ISPASSERCAISSE = true;
            //                }
            //                ctx.SaveChanges();
            //            }
            //            if (laDemande.LaDemande.ISPASSERCAISSE)
            //                new DbWorkFlow().ExecuterActionSurDemandeTransction(laDemande.LaDemande .NUMDEM, Enumere.TRANSMETTRE, laDemande.LaDemande .MATRICULE, string.Empty, ctx);
            //        }
            //        return true ;
                    
            //    }
            //    catch (Exception ex)
            //    {
            //        throw ex;
            //    }
            //}


            public bool ValiderActionSurDemande(CsDemande laDemande)
            {

                SqlCommand laCommande = DBBase.InitTransaction(ConnectionString);

                try
                {
                    List<string> lstCoper = null;

                    if (laDemande.LaDemande.ISSUPPRIMERCOUT)
                        lstCoper = laDemande.LstCoutDemande.Where(x => x.MONTANTHT == 0 && x.MONTANTTTC == 0).Select(t => t.COPER).Distinct().ToList();

                    TraiterAction(laDemande, lstCoper, laCommande);

                    laCommande.Transaction.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    laCommande.Transaction.Rollback();
                    new ErrorManager().WriteInLogFile(this, ex.Message);
                    return false;
                }

                finally
                {
                    if (laCommande.Connection.State == ConnectionState.Open)
                        laCommande.Connection.Close(); // Fermeture de la connection 
                    laCommande.Dispose();
                }


            }




            private void TraiterAction(CsDemande laDemande, List<string> lstCoper, SqlCommand cmds)
            {
                string Idc = DBBase.RetourneStringListeObject(lstCoper);

                cmds.CommandTimeout = 3000;
                cmds.CommandType = CommandType.StoredProcedure;
                cmds.CommandText = "SPX_ACC_TRAITER_ACTION_DEMANDE";
                if (cmds.Parameters != null && cmds.Parameters.Count != 0) cmds.Parameters.Clear();

                cmds.Parameters.Add("@NUMDEM", SqlDbType.VarChar, 20).Value = laDemande.LaDemande.NUMDEM;
                cmds.Parameters.Add("@FK_IDDEMANDE", SqlDbType.Int).Value = laDemande.LaDemande.PK_ID;
                cmds.Parameters.Add("@MATRICULE", SqlDbType.VarChar, 6).Value = laDemande.LaDemande.USERMODIFICATION;
                cmds.Parameters.Add("@POSTE", SqlDbType.VarChar, 30).Value = laDemande.LaDemande.ANNOTATION;
                cmds.Parameters.Add("@LISTCOPER", SqlDbType.VarChar, int.MaxValue).Value = Idc;
                cmds.Parameters.Add("@ISPASSERCAISSE", SqlDbType.Bit).Value = laDemande.LaDemande.ISPASSERCAISSE;
                cmds.Parameters.Add("@ISSUPPRIMERCOUT", SqlDbType.Bit).Value = laDemande.LaDemande.ISSUPPRIMERCOUT;
                cmds.Parameters.Add("@MOTIF", SqlDbType.VarChar, 100).Value = laDemande.LaDemande.MOTIF;

                DBBase.SetDBNullParametre(cmds.Parameters);

                try
                {
                    if (cmds.Connection.State == ConnectionState.Closed)
                        cmds.Connection.Open();
                    cmds.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw new Exception(cmds.CommandText + ":" + ex.Message);
                }
            }





            #endregion
            #region MyRegion
                public List<CsClientRechercher> RetourneClientSpx(CsRegCli leClient)
                {
                    cn = new SqlConnection(ConnectionString);

                    cmd = new SqlCommand();
                    cmd.Connection = cn;
                    cmd.CommandTimeout = 3000;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "SPX_ACC_CLIENTREGROUPEMENT";
                    cmd.Parameters.Add("@FK_IDREGROUPEMENT", SqlDbType.Int).Value = leClient.PK_ID;
                    DBBase.SetDBNullParametre(cmd.Parameters);
                    try
                    {
                        if (cn.State == ConnectionState.Closed)
                            cn.Open();
                        SqlDataReader reader = cmd.ExecuteReader();
                        DataTable dt = new DataTable();
                        dt.Load(reader);
                        return Entities.GetEntityListFromQuery<CsClientRechercher>(dt);
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
                public List<CsLclient> RetourneCompteClientSpx(CsRegCli leClient, string periodeDebut, string PeriodeFin)
                {
                    cn = new SqlConnection(ConnectionString);

                    cmd = new SqlCommand();
                    cmd.Connection = cn;
                    cmd.CommandTimeout = 6000;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "SPX_ACC_FACTURECLIENT";
                    cmd.Parameters.Add("@FK_IDREGROUPEMENT", SqlDbType.Int).Value = leClient.PK_ID;
                    cmd.Parameters.Add("@PERIODEDEBUT", SqlDbType.VarChar, 6).Value = periodeDebut;
                    cmd.Parameters.Add("@PERIODEFIN", SqlDbType.VarChar, 6).Value = PeriodeFin;
                    DBBase.SetDBNullParametre(cmd.Parameters);
                    try
                    {
                        if (cn.State == ConnectionState.Closed)
                            cn.Open();
                        SqlDataReader reader = cmd.ExecuteReader();
                        DataTable dt = new DataTable();
                        dt.Load(reader);
                        return Entities.GetEntityListFromQuery<CsLclient>(dt);
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
                public List<CsMandatementGc> RetourneMandatementSpx(CsRegCli leClient)
                {
                    cn = new SqlConnection(ConnectionString);

                    cmd = new SqlCommand();
                    cmd.Connection = cn;
                    cmd.CommandTimeout = 360;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "SPX_ACC_MANDATEMENTCLIENT";
                    cmd.Parameters.Add("@FK_IDREGROUPEMENT", SqlDbType.Int).Value = leClient.PK_ID;
                    DBBase.SetDBNullParametre(cmd.Parameters);
                    try
                    {
                        if (cn.State == ConnectionState.Closed)
                            cn.Open();
                        SqlDataReader reader = cmd.ExecuteReader();
                        DataTable dt = new DataTable();
                        dt.Load(reader);
                        return Entities.GetEntityListFromQuery<CsMandatementGc>(dt);
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
                public void  MiseAJourAbonnementSeulSansMetre(CsDemande pDemande, SqlCommand  cmds)
                {


                    cmds.CommandTimeout = 3000;
                    cmds.CommandType = CommandType.StoredProcedure;
                    cmds.CommandText = "SPX_ACC_MISEAJOURABONSEUL";
                    if (cmds.Parameters != null && cmds.Parameters.Count != 0) cmds.Parameters.Clear();

                    cmds.Parameters.Add("@PK_ID", SqlDbType.Int).Value = pDemande.LaDemande.PK_ID;
                    cmds.Parameters.Add("@NUMDEM", SqlDbType.VarChar, 20).Value = pDemande.LaDemande.NUMDEM;
                    cmds.Parameters.Add("@FK_IDCENTRE", SqlDbType.Int).Value = pDemande.LaDemande.FK_IDCENTRE;
                    cmds.Parameters.Add("@CLIENT", SqlDbType.VarChar, 12).Value = pDemande.LaDemande.CLIENT;
                    cmds.Parameters.Add("@MATRICULE", SqlDbType.VarChar, 5).Value = pDemande.LaDemande.MATRICULE;

                    DBBase.SetDBNullParametre(cmds.Parameters);
                    try
                    {
                        if (cmds.Connection.State == ConnectionState.Closed)
                            cmds.Connection.Open();
                        cmds.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(cmds.CommandText + ":" + ex.Message);
                    }
                }
                public List<CsProgarmmation> ChargerListeProgram(List<int> lstIdCentre, string NumerodeProgramme, DateTime? DateDebut, DateTime? DateFin, string IdEquipe, int idEtape, bool IsCompteur)
                {
                    string Idc = DBBase.RetourneStringListeObject(lstIdCentre);

                    cn = new SqlConnection(ConnectionString);
                    cmd = new SqlCommand();
                    cmd.Connection = cn;
                    cmd.CommandTimeout = 3600;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "SPX_ACC_LISTEDEPROGRAMME";

                    Guid? leIdGroup = null;
                    if (!string.IsNullOrEmpty(IdEquipe)) leIdGroup = Guid.Parse(IdEquipe);


                    cmd.Parameters.Add("@DATEDEBUT", SqlDbType.DateTime).Value = DateDebut;
                    cmd.Parameters.Add("@DATEFIN", SqlDbType.DateTime).Value = DateFin;
                    cmd.Parameters.Add("@IDEQUIPE", SqlDbType.UniqueIdentifier).Value = leIdGroup;
                    cmd.Parameters.Add("@IDCENTRELIST", SqlDbType.VarChar, int.MaxValue).Value = Idc;
                    cmd.Parameters.Add("@IDETAPE", SqlDbType.Int).Value = idEtape;
                    cmd.Parameters.Add("@ISSORTIECOMPTEUR", SqlDbType.Bit).Value = IsCompteur;
                    cmd.Parameters.Add("@NUMEROPROGRAMME", SqlDbType.VarChar, 20).Value = NumerodeProgramme;

                    DBBase.SetDBNullParametre(cmd.Parameters);
                    try
                    {
                        if (cn.State == ConnectionState.Closed)
                            cn.Open();
                        SqlDataReader reader = cmd.ExecuteReader();
                        DataTable dt = new DataTable();
                        dt.Load(reader);
                        return Entities.GetEntityListFromQuery<CsProgarmmation>(dt);
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
                public List<CsProgarmmation> ChargerListeProgramReedition(List<int> lstIdCentre, string NumerodeProgramme, DateTime? DateDebut, DateTime? DateFin, string IdEquipe, int TypeEtat)
                {
                    string Idc = DBBase.RetourneStringListeObject(lstIdCentre);
                    cn = new SqlConnection(ConnectionString);

                    cmd = new SqlCommand();
                    cmd.Connection = cn;
                    cmd.CommandTimeout = 3600;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "SPX_ACC_REEDITIONTRAVAUX";

                    cmd.Parameters.Add("@DATEDEBUT", SqlDbType.DateTime).Value = DateDebut;
                    cmd.Parameters.Add("@DATEFIN", SqlDbType.DateTime).Value = DateFin;
                    cmd.Parameters.Add("@IDEQUIPE", SqlDbType.VarChar,60).Value = IdEquipe;
                    cmd.Parameters.Add("@IDCENTRELIST", SqlDbType.VarChar, int.MaxValue).Value = Idc;
                    cmd.Parameters.Add("@TYPEETAT", SqlDbType.Int).Value = TypeEtat;
                    cmd.Parameters.Add("@NUMEROPROGRAMME", SqlDbType.VarChar, 20).Value = NumerodeProgramme;

                    DBBase.SetDBNullParametre(cmd.Parameters);
                    try
                    {
                        if (cn.State == ConnectionState.Closed)
                            cn.Open();
                        SqlDataReader reader = cmd.ExecuteReader();
                        DataTable dt = new DataTable();
                        dt.Load(reader);
                        return Entities.GetEntityListFromQuery<CsProgarmmation>(dt);
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
                public List<CsCanalisation> GetDemandeByListeNumIdDemandeSortieMaterielSpx(DateTime? DateProgramme, Guid IdEquipe, int? EtapeActuelle)
                {
                    cn = new SqlConnection(ConnectionString);

                    cmd = new SqlCommand();
                    cmd.Connection = cn;
                    cmd.CommandTimeout = 3000;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "SPX_ACC_CHARGERDONNESORTIEMATERIEL";
                    cmd.Parameters.Add("@DateProgramme", SqlDbType.DateTime).Value = DateProgramme;
                    cmd.Parameters.Add("@IdGroupe", SqlDbType.UniqueIdentifier).Value = IdEquipe;
                    cmd.Parameters.Add("@idetape", SqlDbType.Int).Value = EtapeActuelle;

                    DBBase.SetDBNullParametre(cmd.Parameters);
                    try
                    {
                        if (cn.State == ConnectionState.Closed)
                            cn.Open();
                        SqlDataReader reader = cmd.ExecuteReader();
                        DataTable dt = new DataTable();
                        dt.Load(reader);
                        return Entities.GetEntityListFromQuery<CsCanalisation>(dt);
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
                public List<CsCanalisation> GetDemandeByNumIdDemandeSpx(DateTime? DateProgramme, Guid IdEquipe, int? EtapeActuelle)
                {
                    cn = new SqlConnection(ConnectionString);

                    cmd = new SqlCommand();
                    cmd.Connection = cn;
                    cmd.CommandTimeout = 3000;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "SPX_ACC_CHARGERDONNESORTIECOMPTEUR";
                    cmd.Parameters.Add("@DateProgramme", SqlDbType.DateTime).Value = DateProgramme;
                    cmd.Parameters.Add("@IdGroupe", SqlDbType.UniqueIdentifier).Value = IdEquipe;
                    cmd.Parameters.Add("@idetape", SqlDbType.Int).Value = EtapeActuelle;

                    DBBase.SetDBNullParametre(cmd.Parameters);
                    try
                    {
                        if (cn.State == ConnectionState.Closed)
                            cn.Open();
                        SqlDataReader reader = cmd.ExecuteReader();
                        DataTable dt = new DataTable();
                        dt.Load(reader);
                        return Entities.GetEntityListFromQuery<CsCanalisation>(dt);
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
                public List<ObjELEMENTDEVIS > ChargerListeDonneeProgramReedition(string NumProgramme, DateTime? DateProgramme, Guid IdEquipe)
                {
                    cn = new SqlConnection(ConnectionString);

                    cmd = new SqlCommand();
                    cmd.Connection = cn;
                    cmd.CommandTimeout = 3000;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "SPX_ACC_CHARGERDONNEPROGRAMME";
                    cmd.Parameters.Add("@DateProgramme", SqlDbType.DateTime).Value = DateProgramme;
                    cmd.Parameters.Add("@IdGroupe", SqlDbType.UniqueIdentifier).Value = IdEquipe;
                    cmd.Parameters.Add("@NumProgramme", SqlDbType.VarChar,20).Value = NumProgramme;

                    DBBase.SetDBNullParametre(cmd.Parameters);
                    try
                    {
                        if (cn.State == ConnectionState.Closed)
                            cn.Open();
                        SqlDataReader reader = cmd.ExecuteReader();
                        DataTable dt = new DataTable();
                        dt.Load(reader);
                        return Entities.GetEntityListFromQuery<ObjELEMENTDEVIS>(dt);
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

        /*
                public List<CsTarif> ChargerTypeTarif(int produit, int? IDpuissanceSouscrite, int? IDCategorie,int ? IdReglage)
                {
                    cn = new SqlConnection(ConnectionString);

                    cmd = new SqlCommand();
                    cmd.Connection = cn;
                    cmd.CommandTimeout = 3000;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "SPX_GET_TYPETARIF";
                    cmd.Parameters.Add("@idProduit", SqlDbType.Int).Value = produit;
                    cmd.Parameters.Add("@idPuissanceSouscrite", SqlDbType.Int).Value = IDpuissanceSouscrite;
                    cmd.Parameters.Add("@idCategorie", SqlDbType.Int).Value = IDCategorie;
                    cmd.Parameters.Add("@idReglage", SqlDbType.Int).Value = IdReglage;

                    DBBase.SetDBNullParametre(cmd.Parameters);
                    try
                    {
                        if (cn.State == ConnectionState.Closed)
                            cn.Open();
                        SqlDataReader reader = cmd.ExecuteReader();
                        DataTable dt = new DataTable();
                        dt.Load(reader);
                        return Entities.GetEntityListFromQuery<CsTarif>(dt);
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

        */

                public List<CsTarif> ChargerTypeTarif(int produit, int? IDpuissanceSouscrite, int? IDCategorie, int? IdReglage, int? idTarif)
                {
                    cn = new SqlConnection(ConnectionString);

                    cmd = new SqlCommand();
                    cmd.Connection = cn;
                    cmd.CommandTimeout = 1800;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "SPX_GET_TYPETARIF";
                    cmd.Parameters.Add("@idProduit", SqlDbType.Int).Value = produit;
                    cmd.Parameters.Add("@idPuissanceSouscrite", SqlDbType.Int).Value = IDpuissanceSouscrite;
                    cmd.Parameters.Add("@idCategorie", SqlDbType.Int).Value = IDCategorie;
                    cmd.Parameters.Add("@idReglage", SqlDbType.Int).Value = IdReglage;
                    cmd.Parameters.Add("@idTarif", SqlDbType.Int).Value = idTarif;

                    DBBase.SetDBNullParametre(cmd.Parameters);
                    try
                    {
                        if (cn.State == ConnectionState.Closed)
                            cn.Open();
                        SqlDataReader reader = cmd.ExecuteReader();
                        DataTable dt = new DataTable();
                        dt.Load(reader);
                        return Entities.GetEntityListFromQuery<CsTarif>(dt);
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



                public bool SupprimerDevenement(CsDemande demande)
                {
                    try
                    {
                        cn = new SqlConnection(ConnectionString);

                        cmd = new SqlCommand();
                        cmd.Connection = cn;
                        cmd.CommandTimeout = 3000;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "SPX_ACC_SUPPRIMEDEVENEMENT";
                        cmd.Parameters.Add("@FK_IDCENTRE", SqlDbType.Int).Value = demande.LaDemande.FK_IDCENTRE ;
                        cmd.Parameters.Add("@CLIENT", SqlDbType.VarChar ,20).Value = demande.LaDemande.CLIENT ;
                        cmd.Parameters.Add("@ORDRE", SqlDbType.VarChar ,2).Value = demande.LaDemande.ORDRE ;
                        cmd.Parameters.Add("@NUMDEM", SqlDbType.VarChar ,20).Value = demande.LaDemande.NUMDEM ;
                        DBBase.SetDBNullParametre(cmd.Parameters);
                        try
                        {
                            if (cn.State == ConnectionState.Closed)
                                cn.Open();
                            int res = -1;
                            res = cmd.ExecuteNonQuery();
                            return res == -1 ? false :true ;
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

                public CsProgarmmation RetoureDetailProgramme(int IdDemande, SqlConnection laConnection)
                {
                    cmd = new SqlCommand();
                    cmd.Connection = laConnection;
                    cmd.CommandTimeout = 3000;
                    cmd.CommandType = CommandType.StoredProcedure;
                    if (cmd.Parameters != null && cmd.Parameters.Count != 0) cmd.Parameters.Clear();
                    cmd.CommandText = "SPX_ACC_DETAILPROGRAMME";
                    cmd.Parameters.Add("@IdDemande", SqlDbType.Int).Value = IdDemande;

                    DBBase.SetDBNullParametre(cmd.Parameters);
                    try
                    {
                        if (laConnection.State == ConnectionState.Closed)
                            laConnection.Open();
                        SqlDataReader reader = cmd.ExecuteReader();
                        DataTable dt = new DataTable();
                        dt.Load(reader);
                        return Entities.GetEntityFromQuery<CsProgarmmation>(dt);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(cmd.CommandText + ":" + ex.Message);
                    }
                    finally
                    {
                        cmd.Dispose();
                    }
                }

                public string  RetourneProgramme(Guid idEquipe,DateTime DateProgramme,string Matricule)
                {
                    cn = new SqlConnection(ConnectionString);

                    cmd = new SqlCommand();
                    cmd.Connection = cn;
                    cmd.CommandTimeout = 3000;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "SPX_ACC_NUMEROPROGRAMME";
                    cmd.Parameters.Add("@FK_IDEQUIPE", SqlDbType.UniqueIdentifier ).Value = idEquipe;
                    cmd.Parameters.Add("@DATEPROGRAMME", SqlDbType.Date).Value = DateProgramme;
                    cmd.Parameters.Add("@MATRICULE", SqlDbType.VarChar ,6 ).Value = Matricule;

                    string NumeroPgrm = string.Empty;

                    DBBase.SetDBNullParametre(cmd.Parameters);
                    try
                    {
                        if (cn.State == ConnectionState.Closed)
                            cn.Open();
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            NumeroPgrm = (Convert.IsDBNull(reader["NUMEROPROGRAMME"])) ? string.Empty : (string)reader["NUMEROPROGRAMME"];
                        }
                        return NumeroPgrm;
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
            #region Eclipse
            //private bool InsererEclipse(CsDemande laDemande, string NumeroCompteur)
            //{
            //    try
            //    {
            //        DbInterfaceComptable db = new DbInterfaceComptable();
            //        if (db.SelectIDLEGALENTITYEclipse(laDemande.LeClient) == 0)
            //            db.InsertTableLEGAL_ENTITY(laDemande.LeClient, laDemande.Ag);
            //        if (db.SelectiAGR(laDemande.LeClient, laDemande.Ag) == 0)
            //            db.InsertTableAGR(laDemande.LeClient, laDemande.Ag);
            //        if (db.SelectiDLOCATION(laDemande.LeClient) == 0)
            //            db.InsertTableLOCATION(laDemande.LaDemande, laDemande.LeClient, laDemande.Ag);
            //        if (db.SelectIDRDP(laDemande.LeClient) == 0)
            //            db.InsertTableRDP(laDemande.LaDemande, laDemande.LeClient, laDemande.Branchement, laDemande.Ag);
            //        if (db.SelectIDMETER(laDemande.LeClient, NumeroCompteur) == string.Empty)
            //            db.InsertTableMeter(laDemande.LaDemande, laDemande.Abonne, laDemande.LeClient, laDemande.Ag, NumeroCompteur);
            //        return true;
            //    }
            //    catch (Exception ex)
            //    {
            //        throw ex;
            //    }


            //}

                private string InsererEclipse(CsDemande laDemande, string NumeroCompteur)
                {
                    try
                    {
                        string CompteurLie = NumeroCompteur;

                        DbInterfaceComptable db = new DbInterfaceComptable();
                        //List<CsLiaisonCompteur> liaisonCpt = db.RetourneLiaisonCompteur(laDemande.LeClient);
                        //if (liaisonCpt == null || liaisonCpt.Count == 0)
                        //{
                        if (db.SelectIDLEGALENTITYEclipse(laDemande.LeClient) == 0)
                        {
                            if (db.InsertTableLEGAL_ENTITY(laDemande.LeClient, laDemande.Ag))
                            {
                                if (db.SelectiAGR(laDemande.LeClient, laDemande.Ag) == 0)
                                    if (db.InsertTableAGR(laDemande.LeClient, laDemande.Ag))
                                    {
                                        if (db.SelectiDLOCATION(laDemande.LeClient) == 0)
                                            if (db.InsertTableLOCATION(laDemande.LaDemande, laDemande.LeClient, laDemande.Ag))
                                                return NumeroCompteur;

                                    }

                            }
                        }
                        else
                        {
                            if (db.SelectiAGR(laDemande.LeClient, laDemande.Ag) == 0)
                            {
                                if (db.InsertTableAGR(laDemande.LeClient, laDemande.Ag))
                                {
                                    if (db.SelectiDLOCATION(laDemande.LeClient) == 0)
                                        if (db.InsertTableLOCATION(laDemande.LaDemande, laDemande.LeClient, laDemande.Ag))
                                            return NumeroCompteur;

                                }
                            }
                            else
                            {
                                if (db.SelectiDLOCATION(laDemande.LeClient) == 0)
                                    if (db.InsertTableLOCATION(laDemande.LaDemande, laDemande.LeClient, laDemande.Ag))
                                        return NumeroCompteur;

                            }

                        }






                            //if (db.SelectIDRDP(laDemande.LeClient) == 0)
                            //    db.InsertTableRDP(laDemande.LaDemande, laDemande.LeClient, laDemande.Branchement, laDemande.Ag);
                            //if (db.SelectIDMETER(laDemande.LeClient, NumeroCompteur) == string.Empty)
                            //    db.InsertTableMeter(laDemande.LaDemande, laDemande.Abonne, laDemande.LeClient, laDemande.Ag, NumeroCompteur);
                            return "ECHEC LIAISON";
                        //}
                        //else
                        //    return "DEJA LIE A " + liaisonCpt.FirstOrDefault().MSNO;
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }


                }


            private bool UpdateEclipse(CsDemande laDemande, string NumeroCompteur)
            {
                try
                {
                    DbInterfaceComptable db = new DbInterfaceComptable();
                    db.UpdateTableLOCATION(laDemande.LaDemande, laDemande.Ag);
                    db.UpdateTableMeter(laDemande.LaDemande, laDemande.LeClient, laDemande.Ag, NumeroCompteur);
                    return true;
                }
                catch (Exception)
                {
                    throw;
                }
            }
            private bool UpdateEclipseSansChangementCompteur(CsDemande laDemande)
            {
                try
                {
                    DbInterfaceComptable db = new DbInterfaceComptable();
                    db.UpdateTableRDP_SUITEAPDP(laDemande.LaDemande, laDemande.LeClient);
                    return true;
                }
                catch (Exception)
                {
                    throw;
                }
            }
            private bool UpdateEclipseChangementCompteur(CsDemande laDemande, string NumeroCompteur)
            {
                try
                {
                    DbInterfaceComptable db = new DbInterfaceComptable();
                    db.UpdateTableChangementCompteur(laDemande.LaDemande, laDemande.LeClient, NumeroCompteur);
                    return true;
                }
                catch (Exception)
                {
                    throw;
                }
            }

            public bool DeliaisonEclipseCompteur(CsDemande laDemande)
            {

                try
                {
                    
                    using (galadbEntities ctx = new galadbEntities())
                    {
                        DCANALISATION lstCan = ctx.DCANALISATION.FirstOrDefault(i => i.NUMDEM == laDemande.LaDemande.NUMDEM);
                        if (lstCan != null)
                        {
                            MAGASINVIRTUEL AncCpt = ctx.MAGASINVIRTUEL.FirstOrDefault(p => p.PK_ID == lstCan.FK_IDMAGAZINVIRTUEL);
                            if (AncCpt != null)
                                AncCpt.ETAT = Enumere.CompteurAffecte;

                            ctx.DCANALISATION.Remove(lstCan);
                        }
                        PROGRAMMATION lePgm = ctx.PROGRAMMATION.FirstOrDefault(i => i.FK_IDDEMANDE == laDemande.LaDemande.PK_ID);
                        if (lePgm != null)
                            ctx.PROGRAMMATION.Remove(lePgm);
                        //DbInterfaceComptable db = new DbInterfaceComptable();
                        //List<CsLiaisonCompteur> liaisonCpt = db.RetourneLiaisonCompteur(laDemande.LeClient);
                        //if (liaisonCpt != null && liaisonCpt.Count != 0)
                        //{
                        //    CsLiaisonCompteur laliaison = liaisonCpt.First();
                        //    db.DeliaisonCompteur(laDemande.LeClient, laliaison, 52110833400);
                        //}
                        new DbWorkFlow().ExecuterActionSurDemandeTransction(laDemande.LaDemande.NUMDEM, Enumere.REJETER , laDemande.LaDemande.MATRICULE, string.Empty, ctx);
                        ctx.SaveChanges();

                    }
                     
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }

            }


            public List<CsCanalisation> DeliaisoniWEBS(CsDemande laDemande, bool isDefectueux, bool isDoubleLiaison)
            {
                SqlCommand laCommande = DBBase.InitTransaction(ConnectionString);

                try
                {
                    List<CsCanalisation> autreDemande = new List<CsCanalisation>();
                    autreDemande = DelierCompteurDansiWEBS(laDemande, isDefectueux, isDoubleLiaison, laCommande);
                    laCommande.Transaction.Commit();
                    return autreDemande;
                }
                catch (Exception)
                {
                    laCommande.Transaction.Rollback();
                    return null;
                }

                finally
                {
                    if (laCommande.Connection.State == ConnectionState.Open)
                        laCommande.Connection.Close(); // Fermeture de la connection 
                    laCommande.Dispose();
                }

            }



            public List<CsCanalisation> DelierCompteurDansiWEBS(CsDemande demande, bool isDefectueux, bool isDoubleLiaison, SqlCommand cmds)
            {
                cmds.CommandTimeout = 1800;
                cmds.CommandType = CommandType.StoredProcedure;
                cmds.CommandText = "SPX_ACC_DELIAISON_COMPTEUR";
                if (cmds.Parameters != null && cmds.Parameters.Count != 0) cmds.Parameters.Clear();
                cmds.Parameters.Add("@FK_IDDEMANDE", SqlDbType.Int).Value = demande.LaDemande.PK_ID;
                cmds.Parameters.Add("@ISCOMPTEURDEFECTUEUX", SqlDbType.Bit).Value = isDefectueux;
                cmds.Parameters.Add("@ISDOUBLELIAISON", SqlDbType.Bit).Value = isDoubleLiaison;
                DBBase.SetDBNullParametre(cmds.Parameters);
                try
                {
                    if (cmds.Connection.State == ConnectionState.Closed)
                        cmds.Connection.Open();
                    SqlDataReader reader = cmds.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(reader);
                    return Entities.GetEntityListFromQuery<CsCanalisation>(dt);
                }
                catch (Exception ex)
                {
                    throw new Exception(cmds.CommandText + ":" + ex.Message);
                }
            }






            public  bool ReliaisonEclipseSimpleCompteur(CsDemande laDemande)
            {
                try
                {
                    using (galadbEntities ctx = new galadbEntities())
                    {

                        CANALISATION lstCan = ctx.CANALISATION.FirstOrDefault(i => i.FK_IDCENTRE  == laDemande.LeClient.FK_IDCENTRE 
                                                                                && i.CENTRE == laDemande.LeClient.CENTRE 
                                                                                && i.CLIENT  == laDemande.LeClient.REFCLIENT );
                        if (lstCan != null)
                        {
                            MAGASINVIRTUEL mv = ctx.MAGASINVIRTUEL.FirstOrDefault(p => p.NUMERO == laDemande.LaDemande.COMPTEUR);
                            if (mv != null)
                            {
                                mv.ETAT = Enumere.CompteurLie;
                                DCANALISATION leDcan = ctx.DCANALISATION.FirstOrDefault(i => i.PK_ID == lstCan.PK_ID);
                                if (leDcan != null)
                                {
                                    MAGASINVIRTUEL AncCpt = ctx.MAGASINVIRTUEL.FirstOrDefault(p => p.PK_ID == leDcan.FK_IDMAGAZINVIRTUEL);
                                    if (AncCpt != null)
                                        AncCpt.ETAT = Enumere.CompteurAffecte;
                                    leDcan.FK_IDMAGAZINVIRTUEL = mv.PK_ID;
                                }
                                COMPTEUR lecomp = ctx.COMPTEUR.FirstOrDefault(l => l.PK_ID == lstCan.FK_IDCOMPTEUR);
                                if (lecomp != null)
                                    lecomp.NUMERO = laDemande.LaDemande.COMPTEUR;
                            }
                            else
                            {
                                DCANALISATION leDcan = ctx.DCANALISATION.FirstOrDefault(i => i.PK_ID == lstCan.PK_ID);
                                if (leDcan != null)
                                {
                                    MAGASINVIRTUEL AncCpt = ctx.MAGASINVIRTUEL.FirstOrDefault(p => p.PK_ID == leDcan.FK_IDMAGAZINVIRTUEL);
                                    if (AncCpt != null)
                                        AncCpt.NUMERO = laDemande.LaDemande.COMPTEUR;
                                }
                                COMPTEUR lecomp = ctx.COMPTEUR.FirstOrDefault(l => l.PK_ID == lstCan.FK_IDCOMPTEUR);
                                if (lecomp != null)
                                    lecomp.NUMERO = laDemande.LaDemande.COMPTEUR;
                            }

                            DbInterfaceComptable db = new DbInterfaceComptable();
                            List<CsLiaisonCompteur> liaisonCpt = db.RetourneLiaisonCompteur(laDemande.LeClient);
                            if (liaisonCpt != null && liaisonCpt.Count != 0)
                            {
                                CsLiaisonCompteur laliaison = liaisonCpt.First();
                                db.ReliaisonCompteur(laDemande.LeClient, laliaison, laDemande.LaDemande.COMPTEUR);
                            }
                            ctx.SaveChanges();
                        }
                    }
                    return true;
                }
                catch (Exception)
                {
                    throw;
                }
            }
            #endregion

            #region Sylla Comptage en fonction de la puissance utilisee et souscripte
            public CsTypeComptage AdapterComptage(int? IDPUISSANCE, int PuissanceUtilise, int? NOMBRETRANSFORMATEUR)
            {
                try
                {
                    cn = new SqlConnection(ConnectionString);

                    cmd = new SqlCommand();
                    cmd.Connection = cn;
                    cmd.CommandTimeout = 3000;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "SPX_GET_TYPECOMPTAGE";
                    cmd.Parameters.Add("@nombreTransfo", SqlDbType.Int).Value = NOMBRETRANSFORMATEUR;
                    cmd.Parameters.Add("@puissanceSouscrite", SqlDbType.Int).Value =
                    cmd.Parameters.Add("@puissanceInstallee", SqlDbType.Int).Value = PuissanceUtilise;
                    DBBase.SetDBNullParametre(cmd.Parameters);
                    try
                    {
                        if (cn.State == ConnectionState.Closed)
                            cn.Open();
                        SqlDataReader reader = cmd.ExecuteReader();
                        DataTable dt = new DataTable();
                        dt.Load(reader);
                        return Entities.GetEntityFromQuery<CsTypeComptage>(dt);
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
            #endregion

            //public CsCanalisation VerifieCompteurExistenew(CsCompteur leCompteur)
            //{
            //    cn = new SqlConnection(ConnectionString);

            //    cmd = new SqlCommand();
            //    cmd.Connection = cn;
            //    cmd.CommandTimeout = 3000;
            //    cmd.CommandType = CommandType.StoredProcedure;
            //    cmd.CommandText = "SPX_ACC_VERIFIECOMPTEUREXISTE";
            //    cmd.Parameters.Add("@NUMERO", SqlDbType.VarChar, 20).Value = leCompteur.NUMERO;
            //    cmd.Parameters.Add("@FK_IDMARQUE", SqlDbType.Int).Value = leCompteur.FK_IDMARQUECOMPTEUR;
            //    cmd.Parameters.Add("@MARQUE", SqlDbType.VarChar, 2).Value = leCompteur.MARQUE;

            //    DBBase.SetDBNullParametre(cmd.Parameters);
            //    try
            //    {
            //        if (cn.State == ConnectionState.Closed)
            //            cn.Open();
            //        SqlDataReader reader = cmd.ExecuteReader();
            //        DataTable dt = new DataTable();
            //        dt.Load(reader);
            //        List<CsCanalisation> lstCan = Entities.GetEntityListFromQuery<CsCanalisation>(dt);
            //        if (lstCan != null && lstCan.Count != 0)
            //            return lstCan.First();
            //        else
            //            return null;
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


        /*
            public CsCanalisation VerifieCompteurExistenew(CsCompteur leCompteur)
            {
                cn = new SqlConnection(ConnectionString);

                cmd = new SqlCommand();
                cmd.Connection = cn;
                cmd.CommandTimeout = 3000;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SPX_ACC_VERIFIECOMPTEUREXISTE";
                cmd.Parameters.Add("@NUMERO", SqlDbType.VarChar, 20).Value = leCompteur.NUMERO;
                cmd.Parameters.Add("@FK_IDMARQUE", SqlDbType.Int).Value = leCompteur.FK_IDMARQUECOMPTEUR;
                cmd.Parameters.Add("@MARQUE", SqlDbType.VarChar, 2).Value = leCompteur.MARQUE;

                DBBase.SetDBNullParametre(cmd.Parameters);
                try
                {
                    if (cn.State == ConnectionState.Closed)
                        cn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(reader);
                    List<CsCanalisation> lstCan = Entities.GetEntityListFromQuery<CsCanalisation>(dt);
                    if (lstCan != null && lstCan.Count != 0)
                        return lstCan.First();
                    else
                        return null;
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
 
         */

            public CsCanalisation VerifieCompteurExistenew(CsCompteur leCompteur)
            {
                cn = new SqlConnection(ConnectionString);

                cmd = new SqlCommand();
                cmd.Connection = cn;
                cmd.CommandTimeout = 1800;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SPX_ACC_VERIFIECOMPTEUREXISTE";
                cmd.Parameters.Add("@NUMERO", SqlDbType.VarChar, 20).Value = leCompteur.NUMERO;
                cmd.Parameters.Add("@FK_IDMARQUE", SqlDbType.Int).Value = leCompteur.FK_IDMARQUECOMPTEUR;
                cmd.Parameters.Add("@MARQUE", SqlDbType.VarChar, 2).Value = leCompteur.MARQUE;
                cmd.Parameters.Add("@PRODUIT", SqlDbType.VarChar, 2).Value = leCompteur.CODEPRODUIT;

                DBBase.SetDBNullParametre(cmd.Parameters);
                try
                {
                    if (cn.State == ConnectionState.Closed)
                        cn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(reader);
                    List<CsCanalisation> lstCan = Entities.GetEntityListFromQuery<CsCanalisation>(dt);
                    if (lstCan != null && lstCan.Count != 0)
                        return lstCan.First();
                    else
                        return null;
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

        /*
            public ObjDOCUMENTSCANNE Select_DocumentContenut(Guid IdDocument)
            {
                cn = new SqlConnection(ConnectionString);

                cmd = new SqlCommand();
                cmd.Connection = cn;
                cmd.CommandTimeout = 1800;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SPX_ACC_SELECTDOCUMENTCONTENT";
                if (cmd.Parameters != null && cmd.Parameters.Count != 0) cmd.Parameters.Clear();
                cmd.Parameters.Add("@IDDOCUMENT", SqlDbType.UniqueIdentifier).Value = IdDocument;
                DBBase.SetDBNullParametre(cmd.Parameters);
                try
                {
                    if (cn.State == ConnectionState.Closed)
                        cn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(reader);
                    return Entities.GetEntityFromQuery<ObjDOCUMENTSCANNE>(dt);
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

            public List<ObjDOCUMENTSCANNE> Select_DocumentTittre(int Iddemande)
            {
                cn = new SqlConnection(ConnectionString);

                cmd = new SqlCommand();
                cmd.Connection = cn;
                cmd.CommandTimeout = 1800;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SPX_ACC_SELECTDOCUMENTTITRE";
                if (cmd.Parameters != null && cmd.Parameters.Count != 0) cmd.Parameters.Clear();
                cmd.Parameters.Add("@IDDEMANDE", SqlDbType.Int).Value = Iddemande;
                DBBase.SetDBNullParametre(cmd.Parameters);
                try
                {
                    if (cn.State == ConnectionState.Closed)
                        cn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(reader);
                    return Entities.GetEntityListFromQuery<ObjDOCUMENTSCANNE>(dt);
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

*/

            public List<ObjDOCUMENTSCANNE> Select_DocumentTittre(int Iddemande, SqlConnection laConnection)
            {
                cmd = new SqlCommand();
                cmd.Connection = laConnection;
                cmd.CommandTimeout = 1800;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SPX_ACC_SELECTDOCUMENTTITRE";
                if (cmd.Parameters != null && cmd.Parameters.Count != 0) cmd.Parameters.Clear();
                cmd.Parameters.Add("@IDDEMANDE", SqlDbType.Int).Value = Iddemande;
                DBBase.SetDBNullParametre(cmd.Parameters);
                try
                {
                    if (laConnection.State == ConnectionState.Closed)
                        laConnection.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(reader);
                    return Entities.GetEntityListFromQuery<ObjDOCUMENTSCANNE>(dt);
                }
                catch (Exception ex)
                {
                    throw new Exception(cmd.CommandText + ":" + ex.Message);
                }
                finally
                {
                    cmd.Dispose();
                }
            }
            public ObjDOCUMENTSCANNE Select_DocumentContenut(Guid IdDocument)
            {
                cn = new SqlConnection(ConnectionString);

                cmd = new SqlCommand();
                cmd.Connection = cn;
                cmd.CommandTimeout = 1800;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SPX_ACC_SELECTDOCUMENTCONTENT";
                if (cmd.Parameters != null && cmd.Parameters.Count != 0) cmd.Parameters.Clear();
                cmd.Parameters.Add("@IDDOCUMENT", SqlDbType.UniqueIdentifier).Value = IdDocument;
                DBBase.SetDBNullParametre(cmd.Parameters);
                try
                {
                    if (cn.State == ConnectionState.Closed)
                        cn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(reader);
                    ObjDOCUMENTSCANNE leDoc = Entities.GetEntityFromQuery<ObjDOCUMENTSCANNE>(dt);
                    if (!string.IsNullOrEmpty(leDoc.CHEMINCOPY))
                        leDoc.CONTENU = UncompressFile(leDoc.CHEMINCOPY);
                        //leDoc = OuvrirePieceJointe(leDoc);
                    return leDoc;
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

            public bool LicenceOK()
            {
                try
                {
                    using (galadbEntities ctx=new galadbEntities())
                    {
                        var param = ctx.PARAMETRESGENERAUX.SingleOrDefault(p => p.CODE == "001989");
                       string Isvalide= param != null ? param.LIBELLE.Trim() : "0";
                       return Isvalide == "1" ? true : false;
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(cmd.CommandText + ":" + ex.Message);
                }
                finally
                {
                    //if (cn.State == ConnectionState.Open)
                    //    cn.Close(); // Fermeture de la connection 
                    //cmd.Dispose();
                }
            }
            public bool LicenseLooking()
            {
                try
                {
                    using (galadbEntities ctx=new galadbEntities())
                    {
                        var param = ctx.PARAMETRESGENERAUX.SingleOrDefault(p => p.CODE == "001989");
                        param.LIBELLE = "0";
                       return Entities.UpdateEntity<PARAMETRESGENERAUX>(param);
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(cmd.CommandText + ":" + ex.Message);
                }
                finally
                {
                    //if (cn.State == ConnectionState.Open)
                    //    cn.Close(); // Fermeture de la connection 
                    //cmd.Dispose();
                }
            }
        public void SendWebRequestToVMS()
            {
                try
                {
                    
                                   using (var client = new HttpClient())
                                    {
                                        var request = new testClassPost
                                        {
                                            uid = "3abe16a0-a216-4175-b235-277151069a5c",
                                            centre = "320",
                                            client = "01000012376",
                                            ordre = "01",
                                            produit = "05",
                                            typetarif = "31",
                                            puissance = "11.00",
                                            commune = "00001",
                                            quartier = "00016",
                                            rue = "815",
                                            porte = "04",
                                            tournee = "1803",
                                            ordtour = "00000055",
                                            nomabon = "DEMBELE ADELE",
                                            cni = "2339/BM/SK",
                                            telephone = "91789387",
                                            denabon = "01",
                                            numdem = "0180005681390",
                                            dated = "2015-06-29 00:00:00.000",
                                            codesite = "018",
                                            centre_name = " SIKASSO",
                                            quartier_libelle = "KAPEL KOUROU",
                                            diametre = "211",
                                            diacomp_libelle = "2 FILS - 5/15A - 5A",
                                            libelle = "Particuliers",
                                            code = "000001",
                                            commune_name = "SIKASSO"
                                        };

                                        var response = client.PostAsync("http://154.119.38.34:8085/EDMWebservice/rest/vms/clientRegister",
                                            new StringContent(JsonConvert.SerializeObject(request).ToString(),
                                                Encoding.UTF8, "application/json"))
                                                .Result;

                                        if (response.IsSuccessStatusCode)
                                        {
                                            var content = JsonConvert.DeserializeObject < testClassResponseelement>(
                                                response.Content.ReadAsStringAsync()
                                                .Result);

                                            // Access variables from the returned JSON object
                                            //var appHref = content.links.applications.href;
                                            //var appHref = content.Results;
                                            Console.WriteLine();
                                        }
                                   }
                }
                catch (Exception ex)
                {
                    throw new Exception(cmd.CommandText + ":" + ex.Message);
                }
                finally
                {
                    //if (cn.State == ConnectionState.Open)
                    //    cn.Close(); // Fermeture de la connection 
                    //cmd.Dispose();
                }
            }

        #region GRC_GROUPE

        public List<CsGroupe> SelectAllGRCGroupe()
        {
            try
            {
                List<CsGroupe> _lstGroupe = new List<CsGroupe>();
                DataTable dt = Galatee.Entity.Model.AccueilProcedures.RetourneListeGroupe();
                _lstGroupe = Entities.GetEntityListFromQuery<CsGroupe>(dt);
                return _lstGroupe;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        public bool DeleteGRCGroupe(CsGroupe pGroupe)
        {
            try
            {
                return Entities.DeleteEntity<Galatee.Entity.Model.GRC_GROUPE>(Entities.ConvertObject<Galatee.Entity.Model.GRC_GROUPE, CsGroupe>(pGroupe));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool DeleteGRCGroupe(List<CsGroupe> pGroupeCollection)
        {
            try
            {
                return Entities.DeleteEntity<Galatee.Entity.Model.GRC_GROUPE>(Entities.ConvertObject<Galatee.Entity.Model.GRC_GROUPE, CsGroupe>(pGroupeCollection));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool UpdateGRCGroupe(CsGroupe pGroupe)
        {
            try
            {
                return Entities.UpdateEntity<Galatee.Entity.Model.GRC_GROUPE>(Entities.ConvertObject<Galatee.Entity.Model.GRC_GROUPE, CsGroupe>(pGroupe));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool UpdateGRCGroupe(List<CsGroupe> pGroupeCollection)
        {
            try
            {
                return Entities.UpdateEntity<Galatee.Entity.Model.GRC_GROUPE>(Entities.ConvertObject<Galatee.Entity.Model.GRC_GROUPE, CsGroupe>(pGroupeCollection));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool InsertGRCGroupe(CsGroupe pGroupe)
        {
            try
            {
                return Entities.InsertEntity<Galatee.Entity.Model.GRC_GROUPE>(Entities.ConvertObject<Galatee.Entity.Model.GRC_GROUPE, CsGroupe>(pGroupe));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool InsertGRCGroupe(List<CsGroupe> pGroupeCollection)
        {
            try
            {
                return Entities.InsertEntity<Galatee.Entity.Model.GRC_GROUPE>(Entities.ConvertObject<Galatee.Entity.Model.GRC_GROUPE, CsGroupe>(pGroupeCollection));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        #endregion

        #region ADO.NET
        public string TransmettreDemande(List<CsDemandeBase> pDemande)
        {
            SqlCommand laCommande = DBBase.InitTransaction(ConnectionString);
            try
            {
                foreach (CsDemandeBase item in pDemande)
                {
                    TransmettreDemande(item.NUMDEM, item.FK_IDETAPEENCOURE, item.MATRICULE, laCommande);
                    laCommande.Transaction.Commit();
                }
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

        /*

        public CsDemandeBase CreeDemande(CsDemande laDemande, bool AvecTransmission)
        {
            SqlCommand laCommande = DBBase.InitTransaction(ConnectionString);
            try
            {
                #region Demande
                if (string.IsNullOrEmpty(laDemande.LaDemande.NUMDEM))
                {
                    laDemande.LaDemande.NUMDEM = NumeroDemande(laDemande.LaDemande.FK_IDCENTRE);
                    if (string.IsNullOrEmpty(laDemande.LaDemande.CLIENT))
                        laDemande.LaDemande.CLIENT = ReferenceClient(laDemande.LaDemande.FK_IDCENTRE, laDemande.LaDemande.PRODUIT == Enumere.ElectriciteMT ? true : false); ;
                }
                InsertOrUpdateDemande(laDemande.LaDemande, laCommande);
                #endregion
                #region DAg
                if (laDemande.Ag != null && !string.IsNullOrEmpty(laDemande.Ag.CENTRE))
                {
                    laDemande.Ag.NUMDEM = laDemande.LaDemande.NUMDEM;
                    laDemande.Ag.FK_IDNUMDEM = laDemande.LaDemande.PK_ID;
                    if (string.IsNullOrEmpty(laDemande.Ag.CLIENT))
                        laDemande.Ag.CLIENT = laDemande.LaDemande.CLIENT;
                    InsertOrUpdateDAG(laDemande.Ag, laCommande);
                }
                #endregion
                #region DClient
                if (laDemande.LeClient != null && !string.IsNullOrEmpty(laDemande.LeClient.CENTRE))
                {
                    laDemande.LeClient.NUMDEM = laDemande.LaDemande.NUMDEM;
                    laDemande.LeClient.FK_IDNUMDEM = laDemande.LaDemande.PK_ID;
                    if (string.IsNullOrEmpty(laDemande.LeClient.REFCLIENT))
                        laDemande.LeClient.REFCLIENT = laDemande.LaDemande.CLIENT;
                    InsertOrUpdateDClient(laDemande.LeClient, laCommande);
                }
                #endregion
                #region Dbrt
                if (laDemande.Branchement != null && !string.IsNullOrEmpty(laDemande.Branchement.CENTRE))
                {
                    laDemande.Branchement.NUMDEM = laDemande.LaDemande.NUMDEM;
                    laDemande.Branchement.FK_IDDEMANDE = laDemande.LaDemande.PK_ID;
                    if (string.IsNullOrEmpty(laDemande.Branchement.CLIENT))
                        laDemande.Branchement.CLIENT = laDemande.LaDemande.CLIENT;
                    InsertOrUpdateDBRT(laDemande.Branchement, laCommande);
                }
                #endregion
                #region DABON
                if (laDemande.Abonne != null && !string.IsNullOrEmpty(laDemande.Abonne.CENTRE))
                {
                    laDemande.Abonne.NUMDEM = laDemande.LaDemande.NUMDEM;
                    laDemande.Abonne.FK_IDDEMANDE = laDemande.LaDemande.PK_ID;
                    if (string.IsNullOrEmpty(laDemande.Abonne.CLIENT))
                        laDemande.Abonne.CLIENT = laDemande.LaDemande.CLIENT;
                    InsertOrUpdateDAbon(laDemande.Abonne, laCommande);
                }
                #endregion
                #region Dcannalisation
                if (laDemande.LstCanalistion != null && laDemande.LstCanalistion.Count != 0)
                {
                    foreach (CsCanalisation item in laDemande.LstCanalistion)
                    {
                        item.FK_IDDEMANDE = laDemande.LaDemande.PK_ID;
                        item.NUMDEM = laDemande.LaDemande.NUMDEM;
                        InsertOrUpdateDCannalisation(item, laCommande);
                    }
                }
                #endregion
                #region DPersonePhysique
                if (laDemande.PersonePhysique != null)
                {
                    laDemande.PersonePhysique.FK_IDDEMANDE = laDemande.LaDemande.PK_ID;
                    InsertOrUpdateDpersPhysique(laDemande.PersonePhysique, laCommande);
                }
                #endregion
                #region SocietePrives
                if (laDemande.SocietePrives != null)
                {
                    laDemande.SocietePrives.FK_IDDEMANDE = laDemande.LaDemande.PK_ID;
                    InsertOrUpdateDSociete(laDemande.SocietePrives, laCommande);
                }
                #endregion
                #region Dadministration
                if (laDemande.AdministrationInstitut != null)
                {
                    laDemande.AdministrationInstitut.FK_IDDEMANDE = laDemande.LaDemande.PK_ID;
                    InsertOrUpdateDAdmnistration_Institut(laDemande.AdministrationInstitut, laCommande);
                }
                #endregion
                #region DInfoProprietaire
                if (laDemande.InfoProprietaire_ != null)
                {
                    laDemande.InfoProprietaire_.FK_IDDEMANDE = laDemande.LaDemande.PK_ID;
                    InsertOrUpdateDInfoPropriotaire(laDemande.InfoProprietaire_, laCommande);
                }
                #endregion
                #region ObjetScanne
                if (laDemande.ObjetScanne != null && laDemande.ObjetScanne.Count != 0)
                {
                    foreach (ObjDOCUMENTSCANNE item in laDemande.ObjetScanne.Where(y => y.CONTENU != null))
                    {
                        item.FK_IDDEMANDE = laDemande.LaDemande.PK_ID;
                        InsertOrUpdateDocumentScane(item, laCommande);
                    }
                }
                #endregion
                if (AvecTransmission)
                {
                    string CodeWkF = laDemande.LaDemande.SITE + laDemande.LaDemande.CENTRE + DateTime.Today.Year + DateTime.Today.Month + DateTime.Today.Day + DateTime.Today.Hour +
                                     DateTime.Now.Minute + DateTime.Now.Second + DateTime.Now.Millisecond;
                    CreeDemande(laDemande.LaDemande.PK_ID, laDemande.LaDemande.NUMDEM, CodeWkF, laCommande);
                }
                laCommande.Transaction.Commit();
                return laDemande.LaDemande;
            }
            catch (Exception ex)
            {
                laCommande.Transaction.Rollback();
                return null;
            }
            finally
            {
                if (laCommande.Connection.State == ConnectionState.Open)
                    laCommande.Connection.Close();
                laCommande.Dispose();
            }
        }
        // CRUD demande 

        */

        /*
        public CsDemandeBase CreeDemande(CsDemande laDemande, bool AvecTransmission)
        {
            SqlCommand laCommande = DBBase.InitTransaction(ConnectionString);
            try
            {
                #region Demande
                if (string.IsNullOrEmpty(laDemande.LaDemande.NUMDEM))
                {
                    laDemande.LaDemande.NUMDEM = NumeroDemande(laDemande.LaDemande.FK_IDCENTRE);
                    if (string.IsNullOrEmpty(laDemande.LaDemande.CLIENT))
                        laDemande.LaDemande.CLIENT = ReferenceClient(laDemande.LaDemande.FK_IDCENTRE, laDemande.LaDemande.PRODUIT == Enumere.ElectriciteMT ? true : false); ;
                }
                InsertOrUpdateDemande(laDemande.LaDemande, laCommande);
                #endregion
                #region DAg
                if (laDemande.Ag != null && !string.IsNullOrEmpty(laDemande.Ag.CENTRE))
                {
                    laDemande.Ag.NUMDEM = laDemande.LaDemande.NUMDEM;
                    laDemande.Ag.FK_IDNUMDEM = laDemande.LaDemande.PK_ID;
                    if (string.IsNullOrEmpty(laDemande.Ag.CLIENT))
                        laDemande.Ag.CLIENT = laDemande.LaDemande.CLIENT;
                    InsertOrUpdateDAG(laDemande.Ag, laCommande);
                }
                #endregion
                #region DClient
                if (laDemande.LeClient != null && !string.IsNullOrEmpty(laDemande.LeClient.CENTRE))
                {
                    laDemande.LeClient.NUMDEM = laDemande.LaDemande.NUMDEM;
                    laDemande.LeClient.FK_IDNUMDEM = laDemande.LaDemande.PK_ID;
                    if (string.IsNullOrEmpty(laDemande.LeClient.REFCLIENT))
                        laDemande.LeClient.REFCLIENT = laDemande.LaDemande.CLIENT;
                    InsertOrUpdateDClient(laDemande.LeClient, laCommande);
                }
                #endregion
                #region Dbrt
                if (laDemande.Branchement != null && !string.IsNullOrEmpty(laDemande.Branchement.CENTRE))
                {
                    laDemande.Branchement.NUMDEM = laDemande.LaDemande.NUMDEM;
                    laDemande.Branchement.FK_IDDEMANDE = laDemande.LaDemande.PK_ID;
                    if (string.IsNullOrEmpty(laDemande.Branchement.CLIENT))
                        laDemande.Branchement.CLIENT = laDemande.LaDemande.CLIENT;
                    InsertOrUpdateDBRT(laDemande.Branchement, laCommande);
                }
                #endregion
                #region DABON
                if (laDemande.Abonne != null && !string.IsNullOrEmpty(laDemande.Abonne.CENTRE))
                {
                    laDemande.Abonne.NUMDEM = laDemande.LaDemande.NUMDEM;
                    laDemande.Abonne.FK_IDDEMANDE = laDemande.LaDemande.PK_ID;
                    if (string.IsNullOrEmpty(laDemande.Abonne.CLIENT))
                        laDemande.Abonne.CLIENT = laDemande.LaDemande.CLIENT;
                    InsertOrUpdateDAbon(laDemande.Abonne, laCommande);
                }
                #endregion
                #region Dcannalisation
                if (laDemande.LstCanalistion != null && laDemande.LstCanalistion.Count != 0)
                {
                    foreach (CsCanalisation item in laDemande.LstCanalistion)
                    {
                        item.FK_IDDEMANDE = laDemande.LaDemande.PK_ID;
                        item.NUMDEM = laDemande.LaDemande.NUMDEM;
                        InsertOrUpdateDCannalisation(item, laCommande);
                    }
                }
                #endregion
                #region DPersonePhysique
                if (laDemande.PersonePhysique != null)
                {
                    laDemande.PersonePhysique.FK_IDDEMANDE = laDemande.LaDemande.PK_ID;
                    InsertOrUpdateDpersPhysique(laDemande.PersonePhysique, laCommande);
                }
                #endregion
                #region SocietePrives
                if (laDemande.SocietePrives != null)
                {
                    laDemande.SocietePrives.FK_IDDEMANDE = laDemande.LaDemande.PK_ID;
                    InsertOrUpdateDSociete(laDemande.SocietePrives, laCommande);
                }
                #endregion
                #region Dadministration
                if (laDemande.AdministrationInstitut != null)
                {
                    laDemande.AdministrationInstitut.FK_IDDEMANDE = laDemande.LaDemande.PK_ID;
                    InsertOrUpdateDAdmnistration_Institut(laDemande.AdministrationInstitut, laCommande);
                }
                #endregion
                #region DInfoProprietaire
                if (laDemande.InfoProprietaire_ != null)
                {
                    laDemande.InfoProprietaire_.FK_IDDEMANDE = laDemande.LaDemande.PK_ID;
                    InsertOrUpdateDInfoPropriotaire(laDemande.InfoProprietaire_, laCommande);
                }
                #endregion
                #region ObjetScanne
                if (laDemande.ObjetScanne != null && laDemande.ObjetScanne.Count != 0)
                {
                    foreach (ObjDOCUMENTSCANNE item in laDemande.ObjetScanne)
                    {
                        if (item.ISNEW == true)
                        {
                            item.FK_IDDEMANDE = laDemande.LaDemande.PK_ID;
                            InsertOrUpdateDocumentScane(item, laCommande);
                        }
                        else if (item.ISTOREMOVE == true)
                            Delete_DocumentScane(item, laCommande);
                    }
                }
                #endregion
                if (laDemande.LaDemande.TYPEDEMANDE == Enumere.ModificationCompteur
                    && laDemande.LstCanalistion != null && laDemande.LstCanalistion.Count != 0)
                    foreach (CsCanalisation item in laDemande.LstCanalistion)
                        InsertOrUpdateDCompteur(item, laCommande);

                if (AvecTransmission)
                {
                    string CodeWkF = laDemande.LaDemande.SITE + laDemande.LaDemande.CENTRE + DateTime.Today.Year + DateTime.Today.Month + DateTime.Today.Day + DateTime.Today.Hour +
                                     DateTime.Now.Minute + DateTime.Now.Second + DateTime.Now.Millisecond;
                    CreeDemande(laDemande.LaDemande.PK_ID, laDemande.LaDemande.NUMDEM, CodeWkF, laCommande);
                }
                laCommande.Transaction.Commit();
                return laDemande.LaDemande;
            }
            catch (Exception ex)
            {
                laCommande.Transaction.Rollback();
                return null;
            }
            finally
            {
                if (laCommande.Connection.State == ConnectionState.Open)
                    laCommande.Connection.Close();
                laCommande.Dispose();
            }
        }
*/

        public CsDemandeBase CreeDemande(CsDemande laDemande, bool AvecTransmission)
        {
            SqlCommand laCommande = DBBase.InitTransaction(ConnectionString);
            SqlConnection laConnection = DBBase.InitConnection(ConnectionString);

            try
            {
                #region Demande
                if (string.IsNullOrEmpty(laDemande.LaDemande.NUMDEM))
                {
                    laDemande.LaDemande.NUMDEM = NumeroDemande(laDemande.LaDemande.FK_IDCENTRE);
                    if (string.IsNullOrEmpty(laDemande.LaDemande.CLIENT))
                        laDemande.LaDemande.CLIENT = ReferenceClient(laDemande.LaDemande.FK_IDCENTRE, laDemande.LaDemande.PRODUIT == Enumere.ElectriciteMT ? true : false);

                }
                InsertOrUpdateDemande(laDemande.LaDemande, laCommande);
                #endregion
                #region DAg
                if (laDemande.Ag != null && !string.IsNullOrEmpty(laDemande.Ag.CENTRE))
                {
                    laDemande.Ag.NUMDEM = laDemande.LaDemande.NUMDEM;
                    laDemande.Ag.FK_IDNUMDEM = laDemande.LaDemande.PK_ID;
                    if (string.IsNullOrEmpty(laDemande.Ag.CLIENT))
                        laDemande.Ag.CLIENT = laDemande.LaDemande.CLIENT;
                    InsertOrUpdateDAG(laDemande.Ag, laCommande);
                }
                #endregion
                #region DClient

                if (laDemande.LaDemande.TYPEDEMANDE == Enumere.ReprisIndex || laDemande.LaDemande.TYPEDEMANDE == Enumere.AnnulationFacture) //01/06/2018
                {
                    CsClient cli = Select_client(laDemande.LaDemande.FK_IDCENTRE, laDemande.LaDemande.CENTRE, laDemande.LaDemande.CLIENT, laDemande.LaDemande.ORDRE, laConnection);
                    if (cli != null)
                    {
                        laDemande.LeClient = cli;
                        laDemande.LeClient.DATECREATION = laDemande.LaDemande.DATECREATION;
                        laDemande.LeClient.USERCREATION = laDemande.LaDemande.USERCREATION;
                    }
                }
                if (laDemande.LeClient != null && !string.IsNullOrEmpty(laDemande.LeClient.CENTRE))
                {
                    laDemande.LeClient.NUMDEM = laDemande.LaDemande.NUMDEM;
                    laDemande.LeClient.FK_IDNUMDEM = laDemande.LaDemande.PK_ID;
                    if (string.IsNullOrEmpty(laDemande.LeClient.REFCLIENT) || string.IsNullOrEmpty(laDemande.LeClient.CENTRE))
                    {
                        laDemande.LeClient.CENTRE = laDemande.LaDemande.CENTRE;
                        laDemande.LeClient.FK_IDCENTRE = laDemande.LaDemande.FK_IDCENTRE;
                        laDemande.LeClient.REFCLIENT = laDemande.LaDemande.CLIENT;
                        laDemande.LeClient.ORDRE = laDemande.LaDemande.ORDRE;
                    }
                    InsertOrUpdateDClient(laDemande.LeClient, laCommande);
                }
                #endregion
                #region Dbrt
                if (laDemande.Branchement != null && !string.IsNullOrEmpty(laDemande.Branchement.CENTRE))
                {
                    laDemande.Branchement.NUMDEM = laDemande.LaDemande.NUMDEM;
                    laDemande.Branchement.FK_IDDEMANDE = laDemande.LaDemande.PK_ID;
                    if (string.IsNullOrEmpty(laDemande.Branchement.CLIENT))
                        laDemande.Branchement.CLIENT = laDemande.LaDemande.CLIENT;
                    InsertOrUpdateDBRT(laDemande.Branchement, laCommande);
                }
                #endregion
                #region DABON
                if (laDemande.Abonne != null && !string.IsNullOrEmpty(laDemande.Abonne.CENTRE))
                {
                    laDemande.Abonne.NUMDEM = laDemande.LaDemande.NUMDEM;
                    laDemande.Abonne.FK_IDDEMANDE = laDemande.LaDemande.PK_ID;
                    if (string.IsNullOrEmpty(laDemande.Abonne.CLIENT))
                        laDemande.Abonne.CLIENT = laDemande.LaDemande.CLIENT;
                    InsertOrUpdateDAbon(laDemande.Abonne, laCommande);
                }
                #endregion

                #region DTRANSFERT
                if (laDemande.Transfert != null && laDemande.Transfert.FK_IDCENTREORIGINE > 0)
                {
                    laDemande.Transfert.NUMDEM = laDemande.LaDemande.NUMDEM;
                    laDemande.Transfert.FK_IDDEMANDE = laDemande.LaDemande.PK_ID;
                    if (laDemande.Transfert.FK_IDCENTREORIGINE == 0)
                        laDemande.Transfert.FK_IDCENTREORIGINE = laDemande.LaDemande.FK_IDCENTRE;
                    InsertDTRANSFERT(laDemande.Transfert, laCommande);
                }
                #endregion

                #region DEVENEMENT 
                //01/06/2018
                if ((laDemande.LaDemande.TYPEDEMANDE == Enumere.ReprisIndex || laDemande.LaDemande.TYPEDEMANDE == Enumere.AnnulationFacture) && laDemande.LstEvenement != null && laDemande.LstEvenement.Count > 0)
                {
                    laDemande.LstEvenement.ForEach(a => a.NUMDEM = laDemande.LaDemande.NUMDEM);
                    laDemande.LstEvenement.ForEach(a => a.FK_IDDEMANDE = laDemande.LaDemande.PK_ID);
                    if(!InsertDEVENEMENT(laDemande, laCommande))
                        throw new Exception("Echec insertion dans DEVENEMENT");
                    
                }
                #endregion

                #region Dcannalisation

                if (laDemande.LaDemande.TYPEDEMANDE != Enumere.BranchementAbonement &&
                    laDemande.LaDemande.TYPEDEMANDE != Enumere.BranchementAbonnementEp &&
                    laDemande.LaDemande.TYPEDEMANDE != Enumere.AbonnementSeul &&
                    laDemande.LaDemande.TYPEDEMANDE != Enumere.BranchementAbonnementMt &&
                    laDemande.LaDemande.TYPEDEMANDE != Enumere.BranchementAbonementExtention &&
                    laDemande.LaDemande.TYPEDEMANDE != Enumere.ChangementProduit &&
                    laDemande.LaDemande.TYPEDEMANDE != Enumere.Reabonnement &&
                    laDemande.LaDemande.TYPEDEMANDE != Enumere.ChangementCompteur &&
                    (laDemande.LaDemande.TYPEDEMANDE != Enumere.AugmentationPuissance || laDemande.LaDemande.ISMETREAFAIRE == false) &&
                    (laDemande.LaDemande.TYPEDEMANDE != Enumere.DimunitionPuissance || laDemande.LaDemande.ISMETREAFAIRE == false))
                {

                    if (laDemande.LstCanalistion != null && laDemande.LstCanalistion.Count != 0)
                    {
                        foreach (CsCanalisation item in laDemande.LstCanalistion)
                        {
                            item.FK_IDDEMANDE = laDemande.LaDemande.PK_ID;
                            item.NUMDEM = laDemande.LaDemande.NUMDEM;
                            InsertOrUpdateDCannalisation(item, laCommande);
                        }
                    }
                }
                #endregion
                #region DPersonePhysique
                if (laDemande.PersonePhysique != null)
                {
                    laDemande.PersonePhysique.FK_IDDEMANDE = laDemande.LaDemande.PK_ID;
                    InsertOrUpdateDpersPhysique(laDemande.PersonePhysique, laCommande);
                }
                #endregion
                #region SocietePrives
                if (laDemande.SocietePrives != null)
                {
                    laDemande.SocietePrives.FK_IDDEMANDE = laDemande.LaDemande.PK_ID;
                    InsertOrUpdateDSociete(laDemande.SocietePrives, laCommande);
                }
                #endregion
                #region Dadministration
                if (laDemande.AdministrationInstitut != null)
                {
                    laDemande.AdministrationInstitut.FK_IDDEMANDE = laDemande.LaDemande.PK_ID;
                    InsertOrUpdateDAdmnistration_Institut(laDemande.AdministrationInstitut, laCommande);
                }
                #endregion
                #region DInfoProprietaire
                if (laDemande.InfoProprietaire_ != null)
                {
                    laDemande.InfoProprietaire_.FK_IDDEMANDE = laDemande.LaDemande.PK_ID;
                    InsertOrUpdateDInfoPropriotaire(laDemande.InfoProprietaire_, laCommande);
                }
                #endregion
                #region ObjetScanne
                EcrirePieceJointe(laDemande, laCommande);
                #endregion
                #region Appareil
                if (laDemande.AppareilDevis != null && laDemande.AppareilDevis.Count != 0)
                {
                    foreach (ObjAPPAREILSDEVIS item in laDemande.AppareilDevis)
                    {
                        item.FK_IDDEVIS = laDemande.LaDemande.PK_ID;
                        item.NUMDEVIS = laDemande.LaDemande.NUMDEM;
                        InsertOrUpdateAppareil(item, laCommande);
                    }
                }
                #endregion

                if (laDemande.LaDemande.TYPEDEMANDE == Enumere.AchatTimbre &&
                    laDemande.LstEltTimbre != null && laDemande.LstEltTimbre.Count != 0)
                {
                    foreach (CsElementAchatTimbre item in laDemande.LstEltTimbre)
                    {
                        item.NUMDEM = laDemande.LaDemande.NUMDEM;
                        item.FK_IDDEMANDE = laDemande.LaDemande.PK_ID;
                        //InsertOrUpdateELEMENTACHATTIMBRE(item, laCommande);
                    }
                    List<CsMapperElementAchatTimbre> lesElt = Galatee.Tools.Utility.ConvertListType<CsMapperElementAchatTimbre, CsElementAchatTimbre>(laDemande.LstEltTimbre);
                    DataTable Table = Galatee.Tools.Utility.ListToDataTable(lesElt);
                    Galatee.Tools.Utility.BulkInsertTable("ELEMENTACHATTIMBRE", Table, laCommande.Connection, laCommande.Transaction);
                }

                if (laDemande.LaDemande.TYPEDEMANDE == Enumere.ModificationCompteur
                    && laDemande.LstCanalistion != null && laDemande.LstCanalistion.Count != 0)
                    foreach (CsCanalisation item in laDemande.LstCanalistion)
                        InsertOrUpdateDCompteur(item, laCommande);

                if (AvecTransmission)
                {
                    string CodeWkF = laDemande.LaDemande.SITE + laDemande.LaDemande.CENTRE + DateTime.Today.Year + DateTime.Today.Month + DateTime.Today.Day + DateTime.Today.Hour +
                                     DateTime.Now.Minute + DateTime.Now.Second + DateTime.Now.Millisecond;
                    CreeDemande(laDemande.LaDemande.PK_ID, laDemande.LaDemande.NUMDEM, CodeWkF, laCommande);
                }
                laCommande.Transaction.Commit();
                return laDemande.LaDemande;
            }
            catch (Exception ex)
            {
                laCommande.Transaction.Rollback();
                throw ex;
            }
            finally
            {
                if (laConnection.State == ConnectionState.Open)
                    laConnection.Close();
                laConnection.Dispose();

                if (laCommande.Connection.State == ConnectionState.Open)
                    laCommande.Connection.Close();
                laCommande.Dispose();
            }
        }

        public string CreeDemandeExtension(CsDemandeBase _LaDemandeMiseAJoure)
        {
            SqlCommand laCommande = DBBase.InitTransaction(ConnectionString);
            try
            {
                CreeDemandeExtentionSpx(_LaDemandeMiseAJoure.PK_ID, _LaDemandeMiseAJoure.NUMDEM, laCommande);
                laCommande.Transaction.Commit();
                return "";
            }
            catch (Exception ex)
            {
                laCommande.Transaction.Rollback();
                return null;
            }
            finally
            {
                if (laCommande.Connection.State == ConnectionState.Open)
                    laCommande.Connection.Close();
                laCommande.Dispose();
            }
        }

        public string CreationDemandeSuiteRejet(CsDemande laDemande, bool AvecTransmission)
        {
            SqlCommand laCommande = DBBase.InitTransaction(ConnectionString);
            try
            {
                #region Demande
                if (string.IsNullOrEmpty(laDemande.LaDemande.NUMDEM))
                {
                    laDemande.LaDemande.NUMDEM = NumeroDemande(laDemande.LaDemande.FK_IDCENTRE);
                    if (string.IsNullOrEmpty(laDemande.LaDemande.CLIENT))
                        laDemande.LaDemande.CLIENT = ReferenceClient(laDemande.LaDemande.FK_IDCENTRE, laDemande.LaDemande.PRODUIT == Enumere.ElectriciteMT ? true : false); ;
                }
                InsertOrUpdateDemande(laDemande.LaDemande, laCommande);
                #endregion
                #region DAg
                if (laDemande.Ag != null && !string.IsNullOrEmpty(laDemande.Ag.CENTRE))
                {
                    laDemande.Ag.NUMDEM = laDemande.LaDemande.NUMDEM;
                    laDemande.Ag.FK_IDNUMDEM = laDemande.LaDemande.PK_ID;
                    if (string.IsNullOrEmpty(laDemande.Ag.CLIENT))
                        laDemande.Ag.CLIENT = laDemande.LaDemande.CLIENT;
                    InsertOrUpdateDAG(laDemande.Ag, laCommande);
                }
                #endregion
                #region DClient
                if (laDemande.LeClient != null && !string.IsNullOrEmpty(laDemande.LeClient.CENTRE))
                {
                    laDemande.LeClient.NUMDEM = laDemande.LaDemande.NUMDEM;
                    laDemande.LeClient.FK_IDNUMDEM = laDemande.LaDemande.PK_ID;
                    if (string.IsNullOrEmpty(laDemande.LeClient.REFCLIENT))
                        laDemande.LeClient.REFCLIENT = laDemande.LaDemande.CLIENT;
                    InsertOrUpdateDClient(laDemande.LeClient, laCommande);
                }
                #endregion
                #region Dbrt
                if (laDemande.Branchement != null && !string.IsNullOrEmpty(laDemande.Branchement.CENTRE))
                {
                    laDemande.Branchement.NUMDEM = laDemande.LaDemande.NUMDEM;
                    laDemande.Branchement.FK_IDDEMANDE = laDemande.LaDemande.PK_ID;
                    if (string.IsNullOrEmpty(laDemande.Branchement.CLIENT))
                        laDemande.Branchement.CLIENT = laDemande.LaDemande.CLIENT;
                    InsertOrUpdateDBRT(laDemande.Branchement, laCommande);
                }
                #endregion
                #region DABON
                if (laDemande.Abonne != null && !string.IsNullOrEmpty(laDemande.Abonne.CENTRE))
                {
                    laDemande.Abonne.NUMDEM = laDemande.LaDemande.NUMDEM;
                    laDemande.Abonne.FK_IDDEMANDE = laDemande.LaDemande.PK_ID;
                    if (string.IsNullOrEmpty(laDemande.Abonne.CLIENT))
                        laDemande.Abonne.CLIENT = laDemande.LaDemande.CLIENT;
                    InsertOrUpdateDAbon(laDemande.Abonne, laCommande);
                }
                #endregion
                #region Dcannalisation
                if (laDemande.LstCanalistion != null && laDemande.LstCanalistion.Count != 0)
                {
                    foreach (CsCanalisation item in laDemande.LstCanalistion)
                    {
                        item.FK_IDDEMANDE = laDemande.LaDemande.PK_ID;
                        item.NUMDEM = laDemande.LaDemande.NUMDEM;
                        InsertOrUpdateDCannalisation(item, laCommande);
                    }
                }
                #endregion
                #region DPersonePhysique
                if (laDemande.PersonePhysique != null)
                {
                    laDemande.PersonePhysique.FK_IDDEMANDE = laDemande.LaDemande.PK_ID;
                    InsertOrUpdateDpersPhysique(laDemande.PersonePhysique, laCommande);
                }
                #endregion
                #region SocietePrives
                if (laDemande.SocietePrives != null)
                {
                    laDemande.SocietePrives.FK_IDDEMANDE = laDemande.LaDemande.PK_ID;
                    InsertOrUpdateDSociete(laDemande.SocietePrives, laCommande);
                }
                #endregion
                #region Dadministration
                if (laDemande.AdministrationInstitut != null)
                {
                    laDemande.AdministrationInstitut.FK_IDDEMANDE = laDemande.LaDemande.PK_ID;
                    InsertOrUpdateDAdmnistration_Institut(laDemande.AdministrationInstitut, laCommande);
                }
                #endregion
                #region DInfoProprietaire
                if (laDemande.InfoProprietaire_ != null)
                {
                    laDemande.InfoProprietaire_.FK_IDDEMANDE = laDemande.LaDemande.PK_ID;
                    InsertOrUpdateDInfoPropriotaire(laDemande.InfoProprietaire_, laCommande);
                }
                #endregion
                #region ObjetScanne
                EcrirePieceJointe(laDemande, laCommande);
                #endregion
                if (AvecTransmission)
                    TransmettreDemande(laDemande.LaDemande.NUMDEM, laDemande.InfoDemande.FK_IDETAPEACTUELLE, laDemande.LaDemande.MATRICULE, laCommande);
                laCommande.Transaction.Commit();
                return "";
            }
            catch (Exception ex)
            {
                laCommande.Transaction.Rollback();
                return null;
            }
            finally
            {
                if (laCommande.Connection.State == ConnectionState.Open)
                    laCommande.Connection.Close();
                laCommande.Dispose();
            }
        }

        public void InsertOrUpdateDCompteur(CsCanalisation leDcompteut, SqlCommand cmds)
        {
            cmds.CommandTimeout = 1800;
            cmds.CommandType = CommandType.StoredProcedure;
            cmds.CommandText = "SPX_ACC_INSERTORUPDATE_DCOMPTEUR";
            if (cmds.Parameters != null && cmds.Parameters.Count != 0) cmds.Parameters.Clear();

            cmds.Parameters.Add("@PRODUIT", SqlDbType.VarChar, 2).Value = leDcompteut.PRODUIT;
            cmds.Parameters.Add("@NUMERO", SqlDbType.VarChar, 20).Value = leDcompteut.NUMERO;
            cmds.Parameters.Add("@TYPECOMPTEUR", SqlDbType.VarChar, 2).Value = leDcompteut.TYPECOMPTEUR;
            cmds.Parameters.Add("@MARQUE", SqlDbType.VarChar, 2).Value = leDcompteut.MARQUE;
            cmds.Parameters.Add("@COEFLECT", SqlDbType.Decimal).Value = leDcompteut.COEFLECT;
            cmds.Parameters.Add("@COEFCOMPTAGE", SqlDbType.Int).Value = leDcompteut.COEFCOMPTAGE;
            cmds.Parameters.Add("@CADRAN", SqlDbType.TinyInt).Value = leDcompteut.CADRAN;
            cmds.Parameters.Add("@ANNEEFAB", SqlDbType.VarChar, 4).Value = leDcompteut.ANNEEFAB;
            cmds.Parameters.Add("@STATUT", SqlDbType.VarChar, 1).Value = 1;
            cmds.Parameters.Add("@FONCTIONNEMENT", SqlDbType.VarChar, 1).Value = leDcompteut.FONCTIONNEMENT;
            cmds.Parameters.Add("@PLOMBAGE", SqlDbType.VarChar, 3).Value = leDcompteut.PLOMBAGE;
            cmds.Parameters.Add("@USERCREATION", SqlDbType.VarChar, 6).Value = leDcompteut.USERCREATION;
            cmds.Parameters.Add("@USERMODIFICATION", SqlDbType.VarChar, 6).Value = leDcompteut.USERMODIFICATION;
            cmds.Parameters.Add("@FK_IDTYPECOMPTEUR", SqlDbType.Int).Value = leDcompteut.FK_IDTYPECOMPTEUR;
            cmds.Parameters.Add("@FK_IDPRODUIT", SqlDbType.Int).Value = leDcompteut.FK_IDPRODUIT;
            cmds.Parameters.Add("@FK_IDMARQUECOMPTEUR", SqlDbType.Int).Value = leDcompteut.FK_IDMARQUECOMPTEUR;
            cmds.Parameters.Add("@FK_IDCALIBRE", SqlDbType.Int).Value = leDcompteut.FK_IDCALIBRE;
            cmds.Parameters.Add("@FK_IDSTATUTCOMPTEUR", SqlDbType.Int).Value = leDcompteut.FK_IDSTATUTCOMPTEUR;
            cmds.Parameters.Add("@FK_IDETATCOMPTEUR", SqlDbType.Int).Value = leDcompteut.FK_IDETATCOMPTEUR;
            cmds.Parameters.Add("@FK_IDCOMPTEUR", SqlDbType.Int).Value = leDcompteut.FK_IDCOMPTEUR;
            cmds.Parameters.Add("@FK_IDDEMANDE", SqlDbType.Int).Value = leDcompteut.FK_IDDEMANDE;
            cmds.Parameters.Add("@PK_ID", SqlDbType.Int).Value = leDcompteut.PK_ID;

            DBBase.SetDBNullParametre(cmds.Parameters);

            try
            {
                if (cmds.Connection.State == ConnectionState.Closed)
                    cmds.Connection.Open();
                cmds.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception(cmds.CommandText + ":" + ex.Message);
            }
        }

        public void ValiderModificationCompteur(CsDemande leDemande, SqlCommand cmds)
        {
            cmds.CommandTimeout = 1800;
            cmds.CommandType = CommandType.StoredProcedure;
            cmds.CommandText = "SPX_ACC_VALIDERMODIFICATIONCOMPTEUR";
            if (cmds.Parameters != null && cmds.Parameters.Count != 0) cmds.Parameters.Clear();
            cmds.Parameters.Add("@NUMDEM", SqlDbType.VarChar, 20).Value = leDemande.LaDemande.NUMDEM;
            cmds.Parameters.Add("@FK_IDDEMANDE", SqlDbType.Int).Value = leDemande.LaDemande.PK_ID;

            DBBase.SetDBNullParametre(cmds.Parameters);
            try
            {
                if (cmds.Connection.State == ConnectionState.Closed)
                    cmds.Connection.Open();
                cmds.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception(cmds.CommandText + ":" + ex.Message);
            }
        }

        public Dictionary<CsGroupeValidation, List<CsUtilisateur>> ActeurEtape(int IdEtape, int Iddemande)
        {
            try
            {
                Dictionary<CsGroupeValidation, List<CsUtilisateur>> Datas = new Dictionary<CsGroupeValidation, List<CsUtilisateur>>();
                CsGroupeValidation GroupeValidation = GroupeDeValidationEtapeSuivante(IdEtape, Iddemande);
                List<CsUtilisateur> utilisateur = UtilisateurGroupeEtape(GroupeValidation.PK_ID, Iddemande);
                Datas.Add(GroupeValidation, utilisateur);
                return Datas;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public CsGroupeValidation GroupeDeValidationEtape(int IdEtape)
        {
            cn = new SqlConnection(ConnectionString);

            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandTimeout = 3000;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SPX_ACC_GROUPEVALIDATION_ETAPE";
            cmd.Parameters.Add("@IDETAPE", SqlDbType.Int).Value = IdEtape;

            DBBase.SetDBNullParametre(cmd.Parameters);
            try
            {
                if (cn.State == ConnectionState.Closed)
                    cn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);
                return Entities.GetEntityFromQuery<CsGroupeValidation>(dt);
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
        public CsGroupeValidation GroupeDeValidationEtapeSuivante(int IdEtape, int Iddemande)
        {
            cn = new SqlConnection(ConnectionString);

            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandTimeout = 3000;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SPX_ACC_GROUPEVALIDATION_ETAPE";
            cmd.Parameters.Add("@IDETAPE", SqlDbType.Int).Value = IdEtape;
            cmd.Parameters.Add("@IDDEMANDE", SqlDbType.Int).Value = Iddemande;

            DBBase.SetDBNullParametre(cmd.Parameters);
            try
            {
                if (cn.State == ConnectionState.Closed)
                    cn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);
                return Entities.GetEntityFromQuery<CsGroupeValidation>(dt);
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
        public List<CsUtilisateur> UtilisateurGroupeEtape(Guid IdGroupeValidation, int Iddemande)
        {
            cn = new SqlConnection(ConnectionString);

            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandTimeout = 3000;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SPX_ACC_UTILISATEURGROUPE";
            cmd.Parameters.Add("@IDGROUPEVALIDATION", SqlDbType.UniqueIdentifier).Value = IdGroupeValidation;
            cmd.Parameters.Add("@IDDEMANDE", SqlDbType.Int).Value = Iddemande;

            DBBase.SetDBNullParametre(cmd.Parameters);
            try
            {
                if (cn.State == ConnectionState.Closed)
                    cn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);
                return Entities.GetEntityListFromQuery<CsUtilisateur>(dt);
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

        public void InsertOrUpdateDemande(CsDemandeBase laDemande, SqlCommand cmds)
        {


            cmds.CommandTimeout = 3000;
            cmds.CommandType = CommandType.StoredProcedure;
            cmds.CommandText = "SPX_ACC_INSERTORUPDATE_DEMANDE";
            if (cmds.Parameters != null && cmds.Parameters.Count != 0) cmds.Parameters.Clear();
            cmds.Parameters.Add("@CENTRE", SqlDbType.VarChar, Enumere.TailleCentre).Value = laDemande.CENTRE;
            cmds.Parameters.Add("@NUMDEM", SqlDbType.VarChar, 20).Value = laDemande.NUMDEM;
            cmds.Parameters.Add("@NUMPERE", SqlDbType.VarChar, 8).Value = laDemande.NUMPERE;
            cmds.Parameters.Add("@TYPEDEMANDE", SqlDbType.VarChar, 2).Value = laDemande.TYPEDEMANDE;
            cmds.Parameters.Add("@DPRRDV", SqlDbType.DateTime).Value = laDemande.DPRRDV;
            cmds.Parameters.Add("@DPRDEV", SqlDbType.DateTime).Value = laDemande.DPRDEV;
            cmds.Parameters.Add("@DPREX", SqlDbType.DateTime).Value = laDemande.DPREX;
            cmds.Parameters.Add("@DREARDV", SqlDbType.DateTime).Value = laDemande.DREARDV;
            cmds.Parameters.Add("@DREADEV", SqlDbType.DateTime).Value = laDemande.DREADEV;
            cmds.Parameters.Add("@DREAEX", SqlDbType.DateTime).Value = laDemande.DREAEX;
            cmds.Parameters.Add("@HRDVPR", SqlDbType.VarChar, 4).Value = laDemande.HRDVPR;
            cmds.Parameters.Add("@FDEM", SqlDbType.VarChar, 1).Value = laDemande.FDEM;
            cmds.Parameters.Add("@FREP", SqlDbType.VarChar, 1).Value = laDemande.FREP;
            cmds.Parameters.Add("@NOMPERE", SqlDbType.VarChar, 30).Value = laDemande.NOMPERE;
            cmds.Parameters.Add("@NOMMERE", SqlDbType.VarChar, 30).Value = laDemande.NOMMERE;
            cmds.Parameters.Add("@MATRICULE", SqlDbType.VarChar, 20).Value = laDemande.MATRICULE;
            cmds.Parameters.Add("@STATUT", SqlDbType.VarChar, 1).Value = laDemande.STATUT;
            cmds.Parameters.Add("@DCAISSE", SqlDbType.DateTime).Value = laDemande.DCAISSE;
            cmds.Parameters.Add("@NCAISSE", SqlDbType.VarChar, 4).Value = laDemande.NCAISSE;
            cmds.Parameters.Add("@EXDAG", SqlDbType.VarChar, 1).Value = laDemande.EXDAG;
            cmds.Parameters.Add("@EXDBRT", SqlDbType.VarChar, 1).Value = laDemande.EXDBRT;
            cmds.Parameters.Add("@PRODUIT", SqlDbType.VarChar, 2).Value = laDemande.PRODUIT;
            cmds.Parameters.Add("@EXCL", SqlDbType.VarChar, 1).Value = laDemande.EXCL;
            cmds.Parameters.Add("@CLIENT", SqlDbType.VarChar, 20).Value = laDemande.CLIENT;
            cmds.Parameters.Add("@EXCOMPT", SqlDbType.VarChar, 1).Value = laDemande.EXCOMPT;
            cmds.Parameters.Add("@COMPTEUR", SqlDbType.VarChar, 20).Value = laDemande.COMPTEUR;
            cmds.Parameters.Add("@EXEVT", SqlDbType.VarChar, 1).Value = laDemande.EXEVT;
            cmds.Parameters.Add("@CTAXEG", SqlDbType.VarChar, 2).Value = laDemande.CTAXEG;
            cmds.Parameters.Add("@DATED", SqlDbType.DateTime).Value = laDemande.DATED;
            cmds.Parameters.Add("@REFEM", SqlDbType.VarChar, 6).Value = laDemande.REFEM;
            cmds.Parameters.Add("@ORDRE", SqlDbType.VarChar, 2).Value = laDemande.ORDRE;
            cmds.Parameters.Add("@TOPEDIT", SqlDbType.VarChar, 1).Value = laDemande.TOPEDIT;
            cmds.Parameters.Add("@FACTURE", SqlDbType.VarChar, 6).Value = laDemande.FACTURE;
            cmds.Parameters.Add("@OPERATIONDIVERSE", SqlDbType.VarChar, 2).Value = laDemande.OPERATIONDIVERSE;
            cmds.Parameters.Add("@DATEFLAG", SqlDbType.DateTime).Value = laDemande.DATEFLAG;
            cmds.Parameters.Add("@USERCREATION", SqlDbType.VarChar, 20).Value = laDemande.USERCREATION;
            cmds.Parameters.Add("@DATECREATION", SqlDbType.DateTime).Value = laDemande.DATECREATION;
            cmds.Parameters.Add("@DATEMODIFICATION", SqlDbType.DateTime).Value = laDemande.DATEMODIFICATION;
            cmds.Parameters.Add("@USERMODIFICATION", SqlDbType.VarChar, 20).Value = laDemande.USERMODIFICATION;
            cmds.Parameters.Add("@ETAPEDEMANDE", SqlDbType.Int).Value = laDemande.ETAPEDEMANDE;
            cmds.Parameters.Add("@FK_IDCENTRE", SqlDbType.Int).Value = laDemande.FK_IDCENTRE;
            cmds.Parameters.Add("@FK_IDCLIENT", SqlDbType.Int).Value = laDemande.FK_IDCLIENT;
            cmds.Parameters.Add("@FK_IDADMUTILISATEUR", SqlDbType.Int).Value = laDemande.FK_IDADMUTILISATEUR;
            cmds.Parameters.Add("@FK_IDTYPEDEMANDE", SqlDbType.Int).Value = laDemande.FK_IDTYPEDEMANDE;
            cmds.Parameters.Add("@FK_IDPRODUIT", SqlDbType.Int).Value = laDemande.FK_IDPRODUIT;
            cmds.Parameters.Add("@TRANSMIS", SqlDbType.Bit).Value = laDemande.TRANSMIS;
            cmds.Parameters.Add("@STATUTDEMANDE", SqlDbType.VarChar, 5).Value = laDemande.STATUTDEMANDE;
            cmds.Parameters.Add("@ANNOTATION", SqlDbType.VarChar, 100).Value = laDemande.ANNOTATION;
            cmds.Parameters.Add("@FICHIERJOINT", SqlDbType.UniqueIdentifier).Value = laDemande.FICHIERJOINT;
            cmds.Parameters.Add("@ISSUPPRIME", SqlDbType.Bit).Value = laDemande.ISSUPPRIME;
            cmds.Parameters.Add("@USERSUPPRESSION", SqlDbType.VarChar, 20).Value = laDemande.USERSUPPRESSION;
            cmds.Parameters.Add("@DATESUPPRESSION", SqlDbType.DateTime).Value = laDemande.DATESUPPRESSION;
            cmds.Parameters.Add("@ISEXTENSION", SqlDbType.Bit).Value = laDemande.ISEXTENSION;
            cmds.Parameters.Add("@ISPRESTATION", SqlDbType.Bit).Value = laDemande.ISPRESTATION;
            cmds.Parameters.Add("@ISFOURNITURE", SqlDbType.Bit).Value = laDemande.ISFOURNITURE;
            cmds.Parameters.Add("@ISPOSE", SqlDbType.Bit).Value = laDemande.ISPOSE;
            cmds.Parameters.Add("@MOTIF", SqlDbType.VarChar, 100).Value = laDemande.MOTIF;
            cmds.Parameters.Add("@FK_IDTYPECLIENT", SqlDbType.Int).Value = laDemande.FK_IDTYPECLIENT;
            cmds.Parameters.Add("@ISCHANGECOMPTEUR", SqlDbType.Bit).Value = laDemande.ISCHANGECOMPTEUR;
            cmds.Parameters.Add("@ISCONTROLE", SqlDbType.Bit).Value = laDemande.ISCONTROLE;
            cmds.Parameters.Add("@DATEFIN", SqlDbType.DateTime).Value = laDemande.DATEFIN;
            cmds.Parameters.Add("@TYPECOMPTAGE", SqlDbType.VarChar, 1).Value = laDemande.TYPECOMPTAGE;
            cmds.Parameters.Add("@FK_IDTYPECOMPTAGE", SqlDbType.Int).Value = laDemande.FK_IDTYPECOMPTAGE;
            cmds.Parameters.Add("@FK_IDPUISSANCESOUSCRITE", SqlDbType.Int).Value = laDemande.FK_IDPUISSANCESOUSCRITE;
            cmds.Parameters.Add("@FK_IDREGLAGECOMPTEUR", SqlDbType.Int).Value = laDemande.FK_IDREGLAGECOMPTEUR;
            cmds.Parameters.Add("@ISMUTATION", SqlDbType.Bit).Value = laDemande.ISMUTATION;
            cmds.Parameters.Add("@REGLAGECOMPTEUR", SqlDbType.VarChar, 3).Value = laDemande.REGLAGECOMPTEUR;
            cmds.Parameters.Add("@PUISSANCESOUSCRITE", SqlDbType.Decimal).Value = laDemande.PUISSANCESOUSCRITE;
            cmds.Parameters.Add("@ISETALONNAGE", SqlDbType.Bit).Value = laDemande.ISETALONNAGE;
            cmds.Parameters.Add("@ISDEMANDEREJETERINIT", SqlDbType.Bit).Value = laDemande.ISDEMANDEREJETERINIT;
            cmds.Parameters.Add("@ISMETREAFAIRE", SqlDbType.Bit).Value = laDemande.ISMETREAFAIRE;
            cmds.Parameters.Add("@ISBONNEINITIATIVE", SqlDbType.Bit).Value = laDemande.ISBONNEINITIATIVE;
            cmds.Parameters.Add("@ISDEVISCOMPLEMENTAIRE", SqlDbType.Bit).Value = laDemande.ISDEVISCOMPLEMENTAIRE;
            cmds.Parameters.Add("@NOMBREDEFOYER", SqlDbType.Int).Value = laDemande.NOMBREDEFOYER;
            cmds.Parameters.Add("@ISDEFINITIF", SqlDbType.Bit).Value = laDemande.ISDEFINITIF;
            cmds.Parameters.Add("@ISPROVISOIR", SqlDbType.Bit).Value = laDemande.ISPROVISOIR;
            cmds.Parameters.Add("@FK_IDDEMANDE", SqlDbType.Int).Value = laDemande.FK_IDDEMANDE;
            cmds.Parameters.Add("@ISCOMMUNE", SqlDbType.Bit).Value = laDemande.ISCOMMUNE;
            cmds.Parameters.Add("@ISEDM", SqlDbType.Bit).Value = laDemande.ISEDM;
            cmds.Parameters.Add("@ISDEVISHT", SqlDbType.Bit).Value = laDemande.ISDEVISHT;
            cmds.Parameters.Add("@ISPASSERCAISSE", SqlDbType.Bit).Value = laDemande.ISPASSERCAISSE;
            cmds.Parameters.Add("@ANCIENNEPUISSANCE", SqlDbType.Decimal).Value = laDemande.ANCIENNEPUISSANCE;
            cmds.Parameters.Add("@ISPASDEFACTURE", SqlDbType.Bit).Value = laDemande.ISPASDEFACTURE;
            cmds.Parameters.Add("@PRESTATAIRE", SqlDbType.VarChar, 100).Value = laDemande.PRESTATAIRE;
            SqlParameter outResult = new SqlParameter("@PK_ID", SqlDbType.VarChar, int.MaxValue) { Direction = ParameterDirection.Output };
            outResult.Value = laDemande.PK_ID;
            cmds.Parameters.Add(outResult);
            DBBase.SetDBNullParametre(cmds.Parameters);

            try
            {
                string IdDemande = string.Empty;
                if (cmds.Connection.State == ConnectionState.Closed)
                    cmds.Connection.Open();
                cmds.ExecuteNonQuery();
                IdDemande = outResult.Value != null ? outResult.Value.ToString() : "1";
                laDemande.PK_ID = int.Parse(IdDemande);
            }
            catch (Exception ex)
            {
                throw new Exception(cmds.CommandText + ":" + ex.Message);
            }
        }
        public void InsertOrUpdateDAG(CsAg laAg, SqlCommand cmds)
        {
            cmds.CommandTimeout = 3000;
            cmds.CommandType = CommandType.StoredProcedure;
            cmds.CommandText = "SPX_ACC_INSERTORUPDATE_DAG";
            if (cmds.Parameters != null && cmds.Parameters.Count != 0) cmds.Parameters.Clear();

            cmds.Parameters.Add("@CENTRE", SqlDbType.VarChar, Enumere.TailleCentre).Value = laAg.CENTRE;
            cmds.Parameters.Add("@CLIENT", SqlDbType.VarChar, Enumere.TailleClient).Value = laAg.CLIENT;
            cmds.Parameters.Add("@NUMDEM", SqlDbType.VarChar, 20).Value = laAg.NUMDEM;
            cmds.Parameters.Add("@NOMP", SqlDbType.VarChar, 63).Value = laAg.NOMP;
            cmds.Parameters.Add("@COMMUNE", SqlDbType.VarChar, 5).Value = laAg.COMMUNE;
            cmds.Parameters.Add("@QUARTIER", SqlDbType.VarChar, 5).Value = laAg.QUARTIER;
            cmds.Parameters.Add("@RUE", SqlDbType.VarChar, 5).Value = laAg.RUE;
            cmds.Parameters.Add("@ETAGE", SqlDbType.VarChar, 2).Value = laAg.ETAGE;
            cmds.Parameters.Add("@PORTE", SqlDbType.VarChar, 5).Value = laAg.PORTE;
            cmds.Parameters.Add("@CADR", SqlDbType.VarChar, 30).Value = laAg.CADR;
            cmds.Parameters.Add("@REGROU", SqlDbType.VarChar, 3).Value = laAg.REGROU;
            cmds.Parameters.Add("@CPARC", SqlDbType.VarChar, 30).Value = laAg.CPARC;
            cmds.Parameters.Add("@DMAJ", SqlDbType.DateTime).Value = laAg.DMAJ;
            cmds.Parameters.Add("@TOURNEE", SqlDbType.VarChar, 15).Value = laAg.TOURNEE;
            cmds.Parameters.Add("@ORDTOUR", SqlDbType.VarChar, 15).Value = laAg.ORDTOUR;
            cmds.Parameters.Add("@SECTEUR", SqlDbType.VarChar, 5).Value = laAg.SECTEUR;
            cmds.Parameters.Add("@CPOS", SqlDbType.VarChar, 8).Value = laAg.CPOS;
            cmds.Parameters.Add("@TELEPHONE", SqlDbType.VarChar, 15).Value = laAg.TELEPHONE;
            cmds.Parameters.Add("@FAX", SqlDbType.VarChar, 15).Value = laAg.FAX;
            cmds.Parameters.Add("@EMAIL", SqlDbType.VarChar, 30).Value = laAg.EMAIL;
            cmds.Parameters.Add("@USERCREATION", SqlDbType.VarChar, 20).Value = laAg.USERCREATION;
            cmds.Parameters.Add("@DATECREATION", SqlDbType.DateTime).Value = laAg.DATECREATION;
            cmds.Parameters.Add("@DATEMODIFICATION", SqlDbType.DateTime).Value = laAg.DATEMODIFICATION;
            cmds.Parameters.Add("@USERMODIFICATION", SqlDbType.VarChar, 20).Value = laAg.USERMODIFICATION;
            cmds.Parameters.Add("@FK_IDTOURNEE", SqlDbType.Int).Value = laAg.FK_IDTOURNEE;
            cmds.Parameters.Add("@FK_IDQUARTIER", SqlDbType.Int).Value = laAg.FK_IDQUARTIER;
            cmds.Parameters.Add("@FK_IDCOMMUNE", SqlDbType.Int).Value = laAg.FK_IDCOMMUNE;
            cmds.Parameters.Add("@FK_IDRUE", SqlDbType.Int).Value = laAg.FK_IDRUE;
            cmds.Parameters.Add("@FK_IDSECTEUR", SqlDbType.Int).Value = laAg.FK_IDSECTEUR;
            cmds.Parameters.Add("@FK_IDCENTRE", SqlDbType.Int).Value = laAg.FK_IDCENTRE;
            cmds.Parameters.Add("@FK_IDDEMANDE", SqlDbType.Int).Value = laAg.FK_IDNUMDEM;
            cmds.Parameters.Add("@ISMODIFIER", SqlDbType.Bit).Value = laAg.ISMODIFIER;
            cmds.Parameters.Add("@PK_ID", SqlDbType.Int).Value = laAg.PK_ID;
            DBBase.SetDBNullParametre(cmds.Parameters);

            try
            {
                if (cmds.Connection.State == ConnectionState.Closed)
                    cmds.Connection.Open();
                cmds.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception(cmds.CommandText + ":" + ex.Message);
            }
        }

        /*
        public void InsertOrUpdateDClient(CsClient leClient, SqlCommand cmds)
        {
            cmds.CommandTimeout = 3000;
            cmds.CommandType = CommandType.StoredProcedure;
            cmds.CommandText = "SPX_ACC_INSERTORUPDATE_DCLIENT";
            if (cmds.Parameters != null && cmds.Parameters.Count != 0) cmds.Parameters.Clear();

            cmds.Parameters.Add("@CENTRE", SqlDbType.VarChar, Enumere.TailleCentre).Value = leClient.CENTRE;
            cmds.Parameters.Add("@REFCLIENT", SqlDbType.VarChar, Enumere.TailleClient).Value = leClient.REFCLIENT;
            cmds.Parameters.Add("@ORDRE", SqlDbType.VarChar, Enumere.TailleOrdre).Value = leClient.ORDRE;
            cmds.Parameters.Add("@CODEIDENTIFICATIONNATIONALE", SqlDbType.VarChar, 20).Value = leClient.CODEIDENTIFICATIONNATIONALE;
            cmds.Parameters.Add("@NUMDEM", SqlDbType.VarChar, 15).Value = leClient.NUMDEM;
            cmds.Parameters.Add("@DENABON", SqlDbType.VarChar, 2).Value = leClient.DENABON;
            cmds.Parameters.Add("@NOMABON", SqlDbType.VarChar, 63).Value = leClient.NOMABON;
            cmds.Parameters.Add("@DENMAND", SqlDbType.VarChar, 2).Value = leClient.DENMAND;
            cmds.Parameters.Add("@NOMMAND", SqlDbType.VarChar, 30).Value = leClient.NOMMAND;
            cmds.Parameters.Add("@ADRMAND1", SqlDbType.VarChar, 100).Value = leClient.ADRMAND1;
            cmds.Parameters.Add("@ADRMAND2", SqlDbType.VarChar, 100).Value = leClient.ADRMAND2;
            cmds.Parameters.Add("@CPOS", SqlDbType.VarChar, 8).Value = leClient.CPOS;
            cmds.Parameters.Add("@BUREAU", SqlDbType.VarChar, 25).Value = leClient.BUREAU;
            cmds.Parameters.Add("@DINC", SqlDbType.DateTime).Value = leClient.DINC;
            cmds.Parameters.Add("@MODEPAIEMENT", SqlDbType.VarChar, 1).Value = leClient.MODEPAIEMENT;
            cmds.Parameters.Add("@NOMTIT", SqlDbType.VarChar, 25).Value = leClient.NOMTIT;
            cmds.Parameters.Add("@BANQUE", SqlDbType.VarChar, 6).Value = leClient.BANQUE;
            cmds.Parameters.Add("@GUICHET", SqlDbType.VarChar, 6).Value = leClient.GUICHET;
            cmds.Parameters.Add("@COMPTE", SqlDbType.VarChar, 20).Value = leClient.COMPTE;
            cmds.Parameters.Add("@RIB", SqlDbType.VarChar, 2).Value = leClient.RIB;
            cmds.Parameters.Add("@PROPRIO", SqlDbType.VarChar, 1).Value = leClient.PROPRIO;
            cmds.Parameters.Add("@CODECONSO", SqlDbType.VarChar, 4).Value = leClient.CODECONSO;
            cmds.Parameters.Add("@CATEGORIE", SqlDbType.VarChar, 2).Value = leClient.CATEGORIE;
            cmds.Parameters.Add("@CODERELANCE", SqlDbType.VarChar, 1).Value = leClient.CODERELANCE;
            cmds.Parameters.Add("@NOMCOD", SqlDbType.VarChar, 6).Value = leClient.NOMCOD;
            cmds.Parameters.Add("@MOISNAIS", SqlDbType.VarChar, 2).Value = leClient.MOISNAIS;
            cmds.Parameters.Add("@ANNAIS", SqlDbType.VarChar, 4).Value = leClient.ANNAIS;
            cmds.Parameters.Add("@NOMPERE", SqlDbType.VarChar, 30).Value = leClient.NOMPERE;
            cmds.Parameters.Add("@NOMMERE", SqlDbType.VarChar, 30).Value = leClient.NOMMERE;
            cmds.Parameters.Add("@NATIONNALITE", SqlDbType.VarChar, 3).Value = leClient.NATIONNALITE;
            cmds.Parameters.Add("@CNI", SqlDbType.VarChar, 30).Value = leClient.CNI;
            cmds.Parameters.Add("@TELEPHONE", SqlDbType.VarChar, 15).Value = leClient.TELEPHONE;
            cmds.Parameters.Add("@MATRICULE", SqlDbType.VarChar, 10).Value = leClient.MATRICULE;
            cmds.Parameters.Add("@REGROUPEMENT", SqlDbType.VarChar, 10).Value = leClient.REGROUPEMENT;
            cmds.Parameters.Add("@REGEDIT", SqlDbType.VarChar, 4).Value = leClient.REGEDIT;
            cmds.Parameters.Add("@FACTURE", SqlDbType.VarChar, 6).Value = leClient.FACTURE;
            cmds.Parameters.Add("@DMAJ", SqlDbType.DateTime).Value = leClient.DMAJ;
            cmds.Parameters.Add("@REFERENCEPUPITRE", SqlDbType.Int).Value = leClient.REFERENCEPUPITRE;
            cmds.Parameters.Add("@PAYEUR", SqlDbType.Int).Value = leClient.PAYEUR;
            cmds.Parameters.Add("@SOUSACTIVITE", SqlDbType.VarChar, 3).Value = leClient.SOUSACTIVITE;
            cmds.Parameters.Add("@AGENTFACTURE", SqlDbType.VarChar, 5).Value = leClient.AGENTFACTURE;
            cmds.Parameters.Add("@AGENTRECOUVR", SqlDbType.VarChar, 5).Value = leClient.AGENTRECOUVR;
            cmds.Parameters.Add("@AGENTASSAINI", SqlDbType.VarChar, 5).Value = leClient.AGENTASSAINI;
            cmds.Parameters.Add("@REGROUCONTRAT", SqlDbType.VarChar, 5).Value = leClient.REGROUCONTRAT;
            cmds.Parameters.Add("@INSPECTION", SqlDbType.VarChar, 3).Value = leClient.INSPECTION;
            cmds.Parameters.Add("@REGLEMENT", SqlDbType.VarChar, 3).Value = leClient.REGLEMENT;
            cmds.Parameters.Add("@DECRET", SqlDbType.VarChar, 6).Value = leClient.DECRET;
            cmds.Parameters.Add("@CONVENTION", SqlDbType.VarChar, 3).Value = leClient.CONVENTION;
            cmds.Parameters.Add("@REFERENCEATM", SqlDbType.VarChar, 3).Value = leClient.REFERENCEATM;
            cmds.Parameters.Add("@DATECREATION", SqlDbType.DateTime).Value = leClient.DATECREATION;
            cmds.Parameters.Add("@DATEMODIFICATION", SqlDbType.DateTime).Value = leClient.DATEMODIFICATION;
            cmds.Parameters.Add("@USERCREATION", SqlDbType.VarChar, 6).Value = leClient.USERCREATION;
            cmds.Parameters.Add("@USERMODIFICATION", SqlDbType.VarChar, 6).Value = leClient.USERMODIFICATION;
            cmds.Parameters.Add("@FK_IDMODEPAIEMENT", SqlDbType.Int).Value = leClient.FK_IDMODEPAIEMENT;
            cmds.Parameters.Add("@FK_IDCODECONSO", SqlDbType.Int).Value = leClient.FK_IDCODECONSO;
            cmds.Parameters.Add("@FK_IDCATEGORIE", SqlDbType.Int).Value = leClient.FK_IDCATEGORIE;
            cmds.Parameters.Add("@FK_IDRELANCE", SqlDbType.Int).Value = leClient.FK_IDRELANCE;
            cmds.Parameters.Add("@FK_IDNATIONALITE", SqlDbType.Int).Value = leClient.FK_IDNATIONALITE;
            cmds.Parameters.Add("@FK_IDCENTRE", SqlDbType.Int).Value = leClient.FK_IDCENTRE;
            cmds.Parameters.Add("@EMAIL", SqlDbType.VarChar, 50).Value = leClient.EMAIL;
            cmds.Parameters.Add("@ISFACTUREEMAIL", SqlDbType.Bit).Value = leClient.ISFACTUREEMAIL;
            cmds.Parameters.Add("@ISFACTURESMS", SqlDbType.Bit).Value = leClient.ISFACTURESMS;
            cmds.Parameters.Add("@FK_IDPAYEUR", SqlDbType.Int).Value = leClient.FK_IDPAYEUR;
            cmds.Parameters.Add("@FK_IDREGROUPEMENT", SqlDbType.Int).Value = leClient.FK_IDREGROUPEMENT;
            cmds.Parameters.Add("@FK_IDPROPRIETAIRE", SqlDbType.Int).Value = leClient.FK_IDPROPRIETAIRE;
            cmds.Parameters.Add("@FK_IDDEMANDE", SqlDbType.Int).Value = leClient.FK_IDNUMDEM;
            cmds.Parameters.Add("@FK_IDPIECEIDENTITE", SqlDbType.Int).Value = leClient.FK_IDPIECEIDENTITE;
            cmds.Parameters.Add("@NUMEROPIECEIDENTITE", SqlDbType.VarChar, 20).Value = leClient.NUMEROPIECEIDENTITE;
            cmds.Parameters.Add("@NUMPROPRIETE", SqlDbType.VarChar, 20).Value = leClient.NUMPROPRIETE;
            cmds.Parameters.Add("@FK_TYPECLIENT", SqlDbType.Int).Value = leClient.FK_TYPECLIENT;
            cmds.Parameters.Add("@FAX", SqlDbType.VarChar, 20).Value = leClient.FAX;
            cmds.Parameters.Add("@BOITEPOSTAL", SqlDbType.VarChar, 50).Value = leClient.BOITEPOSTAL;
            cmds.Parameters.Add("@FK_IDUSAGE", SqlDbType.Int).Value = leClient.FK_IDUSAGE;
            cmds.Parameters.Add("@TELEPHONEFIXE", SqlDbType.VarChar, 50).Value = leClient.TELEPHONEFIXE;
            cmds.Parameters.Add("@FK_IDAG", SqlDbType.Int).Value = leClient.FK_IDAG;
            cmds.Parameters.Add("@ISMODIFIER", SqlDbType.Bit).Value = leClient.ISMODIFIER;
            cmds.Parameters.Add("@PK_ID", SqlDbType.Int).Value = leClient.PK_ID;
            DBBase.SetDBNullParametre(cmds.Parameters);


            try
            {
                int PK_ID = 0;
                if (cmds.Connection.State == ConnectionState.Closed)
                    cmds.Connection.Open();
                cmds.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception(cmds.CommandText + ":" + ex.Message);
            }
        }
        */

        public void InsertOrUpdateDBRT(CsBrt leBrt, SqlCommand cmds)
        {
            cmds.CommandTimeout = 3000;
            cmds.CommandType = CommandType.StoredProcedure;
            cmds.CommandText = "SPX_ACC_INSERTORUPDATE_DBRT";
            if (cmds.Parameters != null && cmds.Parameters.Count != 0) cmds.Parameters.Clear();

            cmds.Parameters.Add("@CENTRE", SqlDbType.VarChar, Enumere.TailleCentre).Value = leBrt.CENTRE;
            cmds.Parameters.Add("@CLIENT", SqlDbType.VarChar, Enumere.TailleClient).Value = leBrt.CLIENT;
            cmds.Parameters.Add("@NUMDEM", SqlDbType.VarChar, 15).Value = leBrt.NUMDEM;
            cmds.Parameters.Add("@PRODUIT", SqlDbType.VarChar, 2).Value = leBrt.PRODUIT;
            cmds.Parameters.Add("@DRAC", SqlDbType.DateTime).Value = leBrt.DRAC;
            cmds.Parameters.Add("@DRES", SqlDbType.DateTime).Value = leBrt.DRES;
            cmds.Parameters.Add("@SERVICE", SqlDbType.VarChar, 2).Value = leBrt.SERVICE;
            cmds.Parameters.Add("@CATBRT", SqlDbType.VarChar, 1).Value = leBrt.CATBRT;
            cmds.Parameters.Add("@DIAMBRT", SqlDbType.VarChar, 4).Value = leBrt.DIAMBRT;
            cmds.Parameters.Add("@LONGBRT", SqlDbType.Decimal).Value = leBrt.LONGBRT;
            cmds.Parameters.Add("@NATBRT", SqlDbType.VarChar, 1).Value = leBrt.NATBRT;
            cmds.Parameters.Add("@NBPOINT", SqlDbType.Int).Value = leBrt.NBPOINT;
            cmds.Parameters.Add("@RESEAU", SqlDbType.VarChar, 2).Value = leBrt.RESEAU;
            cmds.Parameters.Add("@TRONCON", SqlDbType.VarChar, 2).Value = leBrt.TRONCON;
            cmds.Parameters.Add("@DMAJ", SqlDbType.DateTime).Value = leBrt.DMAJ;
            cmds.Parameters.Add("@TRANSFORMATEUR", SqlDbType.VarChar, 2).Value = leBrt.TRANSFORMATEUR;
            cmds.Parameters.Add("@PUISSANCEINSTALLEE", SqlDbType.Decimal).Value = leBrt.PUISSANCEINSTALLEE;
            cmds.Parameters.Add("@PERTES", SqlDbType.Decimal).Value = leBrt.PERTES;
            cmds.Parameters.Add("@COEFPERTES", SqlDbType.Decimal).Value = leBrt.COEFPERTES;
            cmds.Parameters.Add("@APPTRANSFO", SqlDbType.VarChar, 1).Value = leBrt.APPTRANSFO;
            cmds.Parameters.Add("@CODEBRT", SqlDbType.VarChar, 5).Value = leBrt.CODEBRT;
            cmds.Parameters.Add("@CODEPOSTE", SqlDbType.VarChar, 5).Value = leBrt.CODEPOSTE;
            cmds.Parameters.Add("@MARQUETRANSFO", SqlDbType.VarChar, 4).Value = leBrt.MARQUETRANSFO;
            cmds.Parameters.Add("@ANFAB", SqlDbType.VarChar, 4).Value = leBrt.ANFAB;
            cmds.Parameters.Add("@LONGITUDE", SqlDbType.VarChar, 10).Value = leBrt.LONGITUDE;
            cmds.Parameters.Add("@LATITUDE", SqlDbType.VarChar, 10).Value = leBrt.LATITUDE;
            cmds.Parameters.Add("@ADRESSERESEAU", SqlDbType.VarChar, 25).Value = leBrt.ADRESSERESEAU;
            cmds.Parameters.Add("@USERCREATION", SqlDbType.VarChar, 20).Value = leBrt.USERCREATION;
            cmds.Parameters.Add("@DATECREATION", SqlDbType.DateTime).Value = leBrt.DATECREATION;
            cmds.Parameters.Add("@DATEMODIFICATION", SqlDbType.DateTime).Value = leBrt.DATEMODIFICATION;
            cmds.Parameters.Add("@USERMODIFICATION", SqlDbType.VarChar, 20).Value = leBrt.USERMODIFICATION;
            cmds.Parameters.Add("@PK_ID", SqlDbType.Int).Value = leBrt.PK_ID;
            cmds.Parameters.Add("@FK_IDCENTRE", SqlDbType.Int).Value = leBrt.FK_IDCENTRE;
            cmds.Parameters.Add("@FK_IDPRODUIT", SqlDbType.Int).Value = leBrt.FK_IDPRODUIT;
            cmds.Parameters.Add("@FK_IDTYPEBRANCHEMENT", SqlDbType.Int).Value = leBrt.FK_IDTYPEBRANCHEMENT;
            cmds.Parameters.Add("@NOMBRETRANSFORMATEUR", SqlDbType.Int).Value = leBrt.NOMBRETRANSFORMATEUR;
            cmds.Parameters.Add("@FK_IDDEMANDE", SqlDbType.Int).Value = leBrt.FK_IDDEMANDE;
            cmds.Parameters.Add("@FK_IDPOSTESOURCE", SqlDbType.Int).Value = leBrt.FK_IDPOSTESOURCE;
            cmds.Parameters.Add("@FK_IDDEPARTHTA", SqlDbType.Int).Value = leBrt.FK_IDDEPARTHTA;
            cmds.Parameters.Add("@FK_IDQUARTIER", SqlDbType.Int).Value = leBrt.FK_IDQUARTIER;
            cmds.Parameters.Add("@FK_IDPOSTETRANSFORMATION", SqlDbType.Int).Value = leBrt.FK_IDPOSTETRANSFORMATION;
            cmds.Parameters.Add("@DEPARTBT", SqlDbType.VarChar, 6).Value = leBrt.DEPARTBT;
            cmds.Parameters.Add("@NEOUDFINAL", SqlDbType.VarChar, 5).Value = leBrt.NEOUDFINAL;
            cmds.Parameters.Add("@FK_IDAG", SqlDbType.Int).Value = null;
            cmds.Parameters.Add("@ISMODIFIER", SqlDbType.Bit).Value = leBrt.ISMODIFIER;
            DBBase.SetDBNullParametre(cmds.Parameters);
            try
            {
                if (cmds.Connection.State == ConnectionState.Closed)
                    cmds.Connection.Open();
                cmds.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                throw new Exception(cmds.CommandText + ":" + ex.Message);
            }
        }

        public void InsertDTRANSFERT(CsDtransfert transfet, SqlCommand cmds)
        {
            cmds.CommandTimeout = 3000;
            cmds.CommandType = CommandType.StoredProcedure;
            cmds.CommandText = "SPX_ACC_INSERT_DTRANSFERT";
            if (cmds.Parameters != null && cmds.Parameters.Count != 0) cmds.Parameters.Clear();

            cmds.Parameters.Add("@PK_ID", SqlDbType.Int).Value = transfet.PK_ID;
            cmds.Parameters.Add("@NUMDEM", SqlDbType.VarChar, 15).Value = transfet.NUMDEM;
            cmds.Parameters.Add("@FK_IDCENTREORIGINE", SqlDbType.Int).Value = transfet.FK_IDCENTREORIGINE;
            cmds.Parameters.Add("@FK_IDCENTRETRANSFERT", SqlDbType.Int).Value = transfet.FK_IDCENTRETRANSFERT;
            cmds.Parameters.Add("@FK_IDREGROUPEMENT", SqlDbType.Int).Value = transfet.FK_IDREGROUPEMENT;
            cmds.Parameters.Add("@FK_IDDEMANDE", SqlDbType.Int).Value = transfet.FK_IDDEMANDE;
            cmds.Parameters.Add("@TRANSFERIMPAYE", SqlDbType.Bit).Value = transfet.TRANSFERIMPAYE;
            DBBase.SetDBNullParametre(cmds.Parameters);

            try
            {
                if (cmds.Connection.State == ConnectionState.Closed)
                    cmds.Connection.Open();
                cmds.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception(cmds.CommandText + ":" + ex.Message);
            }
        }
        
        public void InsertOrUpdateDAbon(CsAbon leAbonnement, SqlCommand cmds)
        {
            cmds.CommandTimeout = 3000;
            cmds.CommandType = CommandType.StoredProcedure;
            cmds.CommandText = "SPX_ACC_INSERTORUPDATE_DABON";
            if (cmds.Parameters != null && cmds.Parameters.Count != 0) cmds.Parameters.Clear();

            cmds.Parameters.Add("@CENTRE", SqlDbType.VarChar, Enumere.TailleCentre).Value = leAbonnement.CENTRE;
            cmds.Parameters.Add("@CLIENT", SqlDbType.VarChar, Enumere.TailleClient).Value = leAbonnement.CLIENT;
            cmds.Parameters.Add("@ORDRE", SqlDbType.VarChar, Enumere.TailleOrdre).Value = leAbonnement.ORDRE;
            cmds.Parameters.Add("@NUMDEM", SqlDbType.VarChar, 15).Value = leAbonnement.NUMDEM;
            cmds.Parameters.Add("@PRODUIT", SqlDbType.VarChar, 2).Value = leAbonnement.PRODUIT;
            cmds.Parameters.Add("@TYPETARIF", SqlDbType.VarChar, 2).Value = leAbonnement.TYPETARIF;
            cmds.Parameters.Add("@PUISSANCE", SqlDbType.Decimal).Value = leAbonnement.PUISSANCE;
            cmds.Parameters.Add("@FORFAIT", SqlDbType.VarChar, 1).Value = leAbonnement.FORFAIT;
            cmds.Parameters.Add("@FORFPERSO", SqlDbType.VarChar, 6).Value = leAbonnement.FORFPERSO;
            cmds.Parameters.Add("@AVANCE", SqlDbType.Decimal).Value = leAbonnement.AVANCE;
            cmds.Parameters.Add("@DAVANCE", SqlDbType.DateTime).Value = leAbonnement.DAVANCE;
            cmds.Parameters.Add("@REGROU", SqlDbType.VarChar, 1).Value = leAbonnement.REGROU;
            cmds.Parameters.Add("@PERFAC", SqlDbType.VarChar, 1).Value = leAbonnement.PERFAC;
            cmds.Parameters.Add("@MOISFAC", SqlDbType.VarChar, 2).Value = leAbonnement.MOISFAC;
            cmds.Parameters.Add("@DABONNEMENT", SqlDbType.DateTime).Value = leAbonnement.DABONNEMENT;
            cmds.Parameters.Add("@DRES", SqlDbType.DateTime).Value = leAbonnement.DRES;
            cmds.Parameters.Add("@DATERACBRT", SqlDbType.DateTime).Value = leAbonnement.DATERACBRT;
            cmds.Parameters.Add("@NBFAC", SqlDbType.Int).Value = leAbonnement.NBFAC;
            cmds.Parameters.Add("@PERREL", SqlDbType.VarChar, 1).Value = leAbonnement.PERREL;
            cmds.Parameters.Add("@MOISREL", SqlDbType.VarChar, 2).Value = leAbonnement.MOISREL;
            cmds.Parameters.Add("@DMAJ", SqlDbType.DateTime).Value = leAbonnement.DMAJ;
            cmds.Parameters.Add("@RECU", SqlDbType.VarChar, 6).Value = leAbonnement.RECU;
            cmds.Parameters.Add("@RISTOURNE", SqlDbType.Decimal).Value = leAbonnement.RISTOURNE;
            cmds.Parameters.Add("@CONSOMMATIONMAXI", SqlDbType.Int).Value = leAbonnement.CONSOMMATIONMAXI;
            cmds.Parameters.Add("@TYPECOMPTAGE", SqlDbType.VarChar, 1).Value = leAbonnement.TYPECOMPTAGE;
            cmds.Parameters.Add("@FK_IDTYPECOMPTAGE", SqlDbType.Int).Value = leAbonnement.FK_IDTYPECOMPTAGE;
            cmds.Parameters.Add("@ISBORNEPOSTE", SqlDbType.Bit).Value = leAbonnement.ISBORNEPOSTE;
            cmds.Parameters.Add("@COEFFAC", SqlDbType.Int).Value = leAbonnement.COEFFAC;
            cmds.Parameters.Add("@USERCREATION", SqlDbType.VarChar, 6).Value = leAbonnement.USERCREATION;
            cmds.Parameters.Add("@DATECREATION", SqlDbType.DateTime).Value = leAbonnement.DATECREATION;
            cmds.Parameters.Add("@DATEMODIFICATION", SqlDbType.DateTime).Value = leAbonnement.DATEMODIFICATION;
            cmds.Parameters.Add("@USERMODIFICATION", SqlDbType.VarChar, 6).Value = leAbonnement.USERMODIFICATION;
            cmds.Parameters.Add("@PK_ID", SqlDbType.Int).Value = leAbonnement.PK_ID;
            cmds.Parameters.Add("@FK_IDCENTRE", SqlDbType.Int).Value = leAbonnement.FK_IDCENTRE;
            cmds.Parameters.Add("@FK_IDPRODUIT", SqlDbType.Int).Value = leAbonnement.FK_IDPRODUIT;
            cmds.Parameters.Add("@FK_IDFORFAIT", SqlDbType.Int).Value = leAbonnement.FK_IDFORFAIT;
            cmds.Parameters.Add("@FK_IDMOISREL", SqlDbType.Int).Value = leAbonnement.FK_IDMOISREL;
            cmds.Parameters.Add("@FK_IDMOISFAC", SqlDbType.Int).Value = leAbonnement.FK_IDMOISFAC;
            cmds.Parameters.Add("@FK_IDTYPETARIF", SqlDbType.Int).Value = leAbonnement.FK_IDTYPETARIF;
            cmds.Parameters.Add("@FK_IDPERIODICITEFACTURE", SqlDbType.Int).Value = leAbonnement.FK_IDPERIODICITEFACTURE;
            cmds.Parameters.Add("@FK_IDPERIODICITERELEVE", SqlDbType.Int).Value = leAbonnement.FK_IDPERIODICITERELEVE;
            cmds.Parameters.Add("@FK_IDDEMANDE", SqlDbType.Int).Value = leAbonnement.FK_IDDEMANDE;
            cmds.Parameters.Add("@ESTEXONERETVA", SqlDbType.Bit).Value = leAbonnement.ESTEXONERETVA;
            cmds.Parameters.Add("@DEBUTEXONERATIONTVA", SqlDbType.VarChar, 6).Value = leAbonnement.DEBUTEXONERATIONTVA;
            cmds.Parameters.Add("@FINEXONERATIONTVA", SqlDbType.VarChar, 6).Value = leAbonnement.FINEXONERATIONTVA;
            cmds.Parameters.Add("@ISAUGMENTATIONPUISSANCE", SqlDbType.Bit).Value = leAbonnement.ISAUGMENTATIONPUISSANCE;
            cmds.Parameters.Add("@ISDIMINUTIONPUISSANCE", SqlDbType.Bit).Value = leAbonnement.ISDIMINUTIONPUISSANCE;
            cmds.Parameters.Add("@NOUVELLEPUISSANCE", SqlDbType.Decimal).Value = leAbonnement.NOUVELLEPUISSANCE;
            cmds.Parameters.Add("@NOMBREDEFOYER", SqlDbType.Int).Value = leAbonnement.NOMBREDEFOYER;
            cmds.Parameters.Add("@FK_IDCLIENT", SqlDbType.Int).Value = leAbonnement.FK_IDCLIENT;
            cmds.Parameters.Add("@ISMODIFIER", SqlDbType.Bit).Value = leAbonnement.ISMODIFIER;
            DBBase.SetDBNullParametre(cmds.Parameters);

            try
            {
                if (cmds.Connection.State == ConnectionState.Closed)
                    cmds.Connection.Open();
                cmds.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception(cmds.CommandText + ":" + ex.Message);
            }
        }
        public void InsertOrUpdateDEvenement(List<CsEvenement> lsCan, SqlCommand cmds)
        {
            cmds.CommandTimeout = 3000;
            cmds.CommandType = CommandType.StoredProcedure;
            cmds.CommandText = "SPX_ACC_INSERTORUPDATE_DEVENEMENT";

            foreach (CsEvenement laCan in lsCan)
            {
                if (cmds.Parameters != null && cmds.Parameters.Count != 0) cmds.Parameters.Clear();

                cmds.Parameters.Add("@CENTRE", SqlDbType.VarChar, Enumere.TailleCentre).Value = laCan.CENTRE;
                cmds.Parameters.Add("@CLIENT", SqlDbType.VarChar, Enumere.TailleClient).Value = laCan.CLIENT;
                cmds.Parameters.Add("@PRODUIT", SqlDbType.VarChar, 20).Value = laCan.PRODUIT;
                cmds.Parameters.Add("@POINT", SqlDbType.Int).Value = laCan.POINT;
                cmds.Parameters.Add("@NUMEVENEMENT", SqlDbType.Int).Value = laCan.NUMEVENEMENT;
                cmds.Parameters.Add("@ORDRE", SqlDbType.VarChar, 2).Value = laCan.ORDRE;
                cmds.Parameters.Add("@COMPTEUR", SqlDbType.VarChar, 20).Value = laCan.COMPTEUR;
                cmds.Parameters.Add("@DATEEVT", SqlDbType.DateTime).Value = laCan.DATEEVT;
                cmds.Parameters.Add("@PERIODE", SqlDbType.VarChar, 6).Value = laCan.PERIODE;
                cmds.Parameters.Add("@CODEEVT", SqlDbType.VarChar, 2).Value = laCan.CODEEVT;
                cmds.Parameters.Add("@INDEXEVT", SqlDbType.Int).Value = laCan.INDEXEVT;
                cmds.Parameters.Add("@CAS", SqlDbType.VarChar, 2).Value = laCan.CAS;
                cmds.Parameters.Add("@ENQUETE", SqlDbType.VarChar, 1).Value = laCan.ENQUETE;
                cmds.Parameters.Add("@CONSO", SqlDbType.Int).Value = laCan.CONSO;
                cmds.Parameters.Add("@CONSONONFACTUREE", SqlDbType.Int).Value = laCan.CONSONONFACTUREE;
                cmds.Parameters.Add("@LOTRI", SqlDbType.VarChar, 9).Value = laCan.LOTRI;
                cmds.Parameters.Add("@FACTURE", SqlDbType.VarChar, 6).Value = laCan.FACTURE;
                cmds.Parameters.Add("@SURFACTURATION", SqlDbType.Int).Value = laCan.SURFACTURATION;
                cmds.Parameters.Add("@STATUS", SqlDbType.Int).Value = laCan.STATUS;
                cmds.Parameters.Add("@TYPECONSO", SqlDbType.Int).Value = laCan.TYPECONSO;
                cmds.Parameters.Add("@REGLAGECOMPTEUR", SqlDbType.VarChar, 3).Value = laCan.REGLAGECOMPTEUR;
                cmds.Parameters.Add("@MATRICULE", SqlDbType.VarChar, 6).Value = laCan.MATRICULE;
                cmds.Parameters.Add("@FACPER", SqlDbType.VarChar, 6).Value = laCan.FACPER;
                cmds.Parameters.Add("@QTEAREG", SqlDbType.Int).Value = laCan.QTEAREG;
                cmds.Parameters.Add("@DERPERF", SqlDbType.VarChar, 6).Value = laCan.DERPERF;
                cmds.Parameters.Add("@DERPERFN", SqlDbType.VarChar, 6).Value = laCan.DERPERFN;
                cmds.Parameters.Add("@CONSOFAC", SqlDbType.Int).Value = laCan.CONSOFAC;
                cmds.Parameters.Add("@REGIMPUTE", SqlDbType.Int).Value = laCan.REGIMPUTE;
                cmds.Parameters.Add("@REGCONSO", SqlDbType.Int).Value = laCan.REGCONSO;
                cmds.Parameters.Add("@COEFLECT", SqlDbType.Decimal).Value = laCan.COEFLECT;
                cmds.Parameters.Add("@COEFCOMPTAGE", SqlDbType.Int).Value = laCan.COEFCOMPTAGE;
                cmds.Parameters.Add("@PUISSANCE", SqlDbType.Decimal).Value = laCan.PUISSANCE;
                cmds.Parameters.Add("@TYPECOMPTAGE", SqlDbType.VarChar, 1).Value = laCan.TYPECOMPTAGE;
                cmds.Parameters.Add("@TYPECOMPTEUR", SqlDbType.VarChar, 1).Value = laCan.TYPECOMPTEUR;
                cmds.Parameters.Add("@COEFK1", SqlDbType.Decimal).Value = laCan.COEFK1;
                cmds.Parameters.Add("@COEFK2", SqlDbType.Decimal).Value = laCan.COEFK2;
                cmds.Parameters.Add("@COEFFAC", SqlDbType.Int).Value = laCan.COEFFAC;
                cmds.Parameters.Add("@TYPETARIF", SqlDbType.VarChar, 2).Value = laCan.TYPETARIF;
                cmds.Parameters.Add("@CATEGORIE", SqlDbType.VarChar, 2).Value = laCan.CATEGORIE;
                cmds.Parameters.Add("@PROPRIO", SqlDbType.VarChar, 1).Value = laCan.PROPRIO;
                cmds.Parameters.Add("@USERCREATION", SqlDbType.VarChar, 6).Value = laCan.USERCREATION;
                cmds.Parameters.Add("@USERMODIFICATION", SqlDbType.VarChar, 6).Value = laCan.USERMODIFICATION;
                cmds.Parameters.Add("@NUMDEM", SqlDbType.VarChar, 20).Value = laCan.NUMDEM;
                cmds.Parameters.Add("@FK_IDCAS", SqlDbType.Int).Value = laCan.FK_IDCAS;
                cmds.Parameters.Add("@FK_IDCENTRE", SqlDbType.Int).Value = laCan.FK_IDCENTRE;
                cmds.Parameters.Add("@FK_IDPRODUIT", SqlDbType.Int).Value = laCan.FK_IDPRODUIT;
                cmds.Parameters.Add("@FK_IDDEMANDE", SqlDbType.Int).Value = laCan.FK_IDDEMANDE;
                cmds.Parameters.Add("@FK_IDCANALISATION", SqlDbType.Int).Value = laCan.FK_IDCANALISATION;
                cmds.Parameters.Add("@FK_IDABON", SqlDbType.Int).Value = laCan.FK_IDABON;
                cmds.Parameters.Add("@FK_IDCOMPTEUR", SqlDbType.Int).Value = laCan.FK_IDCOMPTEUR;
                cmds.Parameters.Add("@COMMENTAIRE", SqlDbType.VarChar, 500).Value = laCan.COMMENTAIRE;
                cmds.Parameters.Add("@FK_IDTOURNEE", SqlDbType.Int).Value = laCan.FK_IDTOURNEE;
                cmds.Parameters.Add("@TOURNEE", SqlDbType.VarChar, 15).Value = laCan.TOURNEE;
                cmds.Parameters.Add("@ORDTOUR", SqlDbType.VarChar, 15).Value = laCan.ORDTOUR;
                cmds.Parameters.Add("@PERFAC", SqlDbType.VarChar, 1).Value = laCan.PERFAC;
                cmds.Parameters.Add("@CONSOMOYENNEPRECEDENTEFACTURE", SqlDbType.Int).Value = laCan.CONSOMOYENNEPRECEDENTEFACTURE;
                cmds.Parameters.Add("@DATERELEVEPRECEDENTEFACTURE", SqlDbType.DateTime).Value = laCan.DATERELEVEPRECEDENTEFACTURE;
                cmds.Parameters.Add("@CASPRECEDENTEFACTURE", SqlDbType.VarChar, 2).Value = laCan.CASPRECEDENTEFACTURE;
                cmds.Parameters.Add("@INDEXPRECEDENTEFACTURE", SqlDbType.Int).Value = laCan.INDEXPRECEDENTEFACTURE;
                cmds.Parameters.Add("@PERIODEPRECEDENTEFACTURE", SqlDbType.VarChar, 6).Value = laCan.PERIODEPRECEDENTEFACTURE;
                cmds.Parameters.Add("@ORDREAFFICHAGE", SqlDbType.TinyInt).Value = laCan.ORDREAFFICHAGE;
                cmds.Parameters.Add("@NOUVEAUCOMPTEUR", SqlDbType.VarChar, 20).Value = laCan.NOUVEAUCOMPTEUR;
                cmds.Parameters.Add("@PUISSANCEINSTALLEE", SqlDbType.Decimal).Value = laCan.PUISSANCEINSTALLEE;
                cmds.Parameters.Add("@COEFKR1", SqlDbType.Decimal).Value = laCan.COEFKR1;
                cmds.Parameters.Add("@COEFKR2", SqlDbType.Decimal).Value = laCan.COEFKR2;
                cmds.Parameters.Add("@QTEAREGPRECEDENT", SqlDbType.Int).Value = laCan.QTEAREGPRECEDENT;
                cmds.Parameters.Add("@ISCONSOSEULE", SqlDbType.Bit).Value = laCan.ISCONSOSEULE;

                DBBase.SetDBNullParametre(cmds.Parameters);

                try
                {
                    if (cmds.Connection.State == ConnectionState.Closed)
                        cmds.Connection.Open();
                    cmds.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw new Exception(cmds.CommandText + ":" + ex.Message);
                } 
            }

        }
        public void InsertOrUpdateDEvenement(CsEvenement laCan, SqlCommand cmds)
        {
            cmds.CommandTimeout = 3000;
            cmds.CommandType = CommandType.StoredProcedure;
            cmds.CommandText = "SPX_ACC_INSERTORUPDATE_DEVENEMENT";
            if (cmds.Parameters != null && cmds.Parameters.Count != 0) cmds.Parameters.Clear();

            cmds.Parameters.Add("@CENTRE", SqlDbType.VarChar, Enumere.TailleCentre).Value = laCan.CENTRE;
            cmds.Parameters.Add("@CLIENT", SqlDbType.VarChar, Enumere.TailleClient).Value = laCan.CLIENT;
            cmds.Parameters.Add("@PRODUIT", SqlDbType.VarChar, 20).Value = laCan.PRODUIT;
            cmds.Parameters.Add("@POINT", SqlDbType.Int).Value = laCan.POINT;
            cmds.Parameters.Add("@NUMEVENEMENT", SqlDbType.Int).Value = laCan.NUMEVENEMENT;
            cmds.Parameters.Add("@ORDRE", SqlDbType.VarChar, 2).Value = laCan.ORDRE;
            cmds.Parameters.Add("@COMPTEUR", SqlDbType.VarChar, 20).Value = laCan.COMPTEUR;
            cmds.Parameters.Add("@DATEEVT", SqlDbType.DateTime).Value = laCan.DATEEVT;
            cmds.Parameters.Add("@PERIODE", SqlDbType.VarChar, 6).Value = laCan.PERIODE;
            cmds.Parameters.Add("@CODEEVT", SqlDbType.VarChar, 2).Value = laCan.CODEEVT;
            cmds.Parameters.Add("@INDEXEVT", SqlDbType.Int).Value = laCan.INDEXEVT;
            cmds.Parameters.Add("@CAS", SqlDbType.VarChar, 2).Value = laCan.CAS;
            cmds.Parameters.Add("@ENQUETE", SqlDbType.VarChar, 1).Value = laCan.ENQUETE;
            cmds.Parameters.Add("@CONSO", SqlDbType.Int).Value = laCan.CONSO;
            cmds.Parameters.Add("@CONSONONFACTUREE", SqlDbType.Int).Value = laCan.CONSONONFACTUREE;
            cmds.Parameters.Add("@LOTRI", SqlDbType.VarChar, 9).Value = laCan.LOTRI;
            cmds.Parameters.Add("@FACTURE", SqlDbType.VarChar, 6).Value = laCan.FACTURE;
            cmds.Parameters.Add("@SURFACTURATION", SqlDbType.Int).Value = laCan.SURFACTURATION;
            cmds.Parameters.Add("@STATUS", SqlDbType.Int).Value = laCan.STATUS;
            cmds.Parameters.Add("@TYPECONSO", SqlDbType.Int).Value = laCan.TYPECONSO;
            cmds.Parameters.Add("@REGLAGECOMPTEUR", SqlDbType.VarChar, 3).Value = laCan.REGLAGECOMPTEUR;
            cmds.Parameters.Add("@MATRICULE", SqlDbType.VarChar, 6).Value = laCan.MATRICULE;
            cmds.Parameters.Add("@FACPER", SqlDbType.VarChar, 6).Value = laCan.FACPER;
            cmds.Parameters.Add("@QTEAREG", SqlDbType.Int).Value = laCan.QTEAREG;
            cmds.Parameters.Add("@DERPERF", SqlDbType.VarChar, 6).Value = laCan.DERPERF;
            cmds.Parameters.Add("@DERPERFN", SqlDbType.VarChar, 6).Value = laCan.DERPERFN;
            cmds.Parameters.Add("@CONSOFAC", SqlDbType.Int).Value = laCan.CONSOFAC;
            cmds.Parameters.Add("@REGIMPUTE", SqlDbType.Int).Value = laCan.REGIMPUTE;
            cmds.Parameters.Add("@REGCONSO", SqlDbType.Int).Value = laCan.REGCONSO;
            cmds.Parameters.Add("@COEFLECT", SqlDbType.Decimal).Value = laCan.COEFLECT;
            cmds.Parameters.Add("@COEFCOMPTAGE", SqlDbType.Int).Value = laCan.COEFCOMPTAGE;
            cmds.Parameters.Add("@PUISSANCE", SqlDbType.Decimal).Value = laCan.PUISSANCE;
            cmds.Parameters.Add("@TYPECOMPTAGE", SqlDbType.VarChar, 1).Value = laCan.TYPECOMPTAGE;
            cmds.Parameters.Add("@TYPECOMPTEUR", SqlDbType.VarChar, 1).Value = laCan.TYPECOMPTEUR;
            cmds.Parameters.Add("@COEFK1", SqlDbType.Decimal).Value = laCan.COEFK1;
            cmds.Parameters.Add("@COEFK2", SqlDbType.Decimal).Value = laCan.COEFK2;
            cmds.Parameters.Add("@COEFFAC", SqlDbType.Int).Value = laCan.COEFFAC;
            cmds.Parameters.Add("@TYPETARIF", SqlDbType.VarChar, 2).Value = laCan.TYPETARIF;
            cmds.Parameters.Add("@CATEGORIE", SqlDbType.VarChar, 2).Value = laCan.CATEGORIE;
            cmds.Parameters.Add("@PROPRIO", SqlDbType.VarChar, 1).Value = laCan.PROPRIO;
            cmds.Parameters.Add("@USERCREATION", SqlDbType.VarChar, 6).Value = laCan.USERCREATION;
            cmds.Parameters.Add("@USERMODIFICATION", SqlDbType.VarChar, 6).Value = laCan.USERMODIFICATION;
            cmds.Parameters.Add("@NUMDEM", SqlDbType.VarChar, 20).Value = laCan.NUMDEM;
            cmds.Parameters.Add("@FK_IDCAS", SqlDbType.Int).Value = laCan.FK_IDCAS;
            cmds.Parameters.Add("@FK_IDCENTRE", SqlDbType.Int).Value = laCan.FK_IDCENTRE;
            cmds.Parameters.Add("@FK_IDPRODUIT", SqlDbType.Int).Value = laCan.FK_IDPRODUIT;
            cmds.Parameters.Add("@FK_IDDEMANDE", SqlDbType.Int).Value = laCan.FK_IDDEMANDE;
            cmds.Parameters.Add("@FK_IDCANALISATION", SqlDbType.Int).Value = laCan.FK_IDCANALISATION;
            cmds.Parameters.Add("@FK_IDABON", SqlDbType.Int).Value = laCan.FK_IDABON;
            cmds.Parameters.Add("@FK_IDCOMPTEUR", SqlDbType.Int).Value = laCan.FK_IDCOMPTEUR;
            cmds.Parameters.Add("@COMMENTAIRE", SqlDbType.VarChar, 500).Value = laCan.COMMENTAIRE;
            cmds.Parameters.Add("@FK_IDTOURNEE", SqlDbType.Int).Value = laCan.FK_IDTOURNEE;
            cmds.Parameters.Add("@TOURNEE", SqlDbType.VarChar, 15).Value = laCan.TOURNEE;
            cmds.Parameters.Add("@ORDTOUR", SqlDbType.VarChar, 15).Value = laCan.ORDTOUR;
            cmds.Parameters.Add("@PERFAC", SqlDbType.VarChar, 1).Value = laCan.PERFAC;
            cmds.Parameters.Add("@CONSOMOYENNEPRECEDENTEFACTURE", SqlDbType.Int).Value = laCan.CONSOMOYENNEPRECEDENTEFACTURE;
            cmds.Parameters.Add("@DATERELEVEPRECEDENTEFACTURE", SqlDbType.DateTime).Value = laCan.DATERELEVEPRECEDENTEFACTURE;
            cmds.Parameters.Add("@CASPRECEDENTEFACTURE", SqlDbType.VarChar, 2).Value = laCan.CASPRECEDENTEFACTURE;
            cmds.Parameters.Add("@INDEXPRECEDENTEFACTURE", SqlDbType.Int).Value = laCan.INDEXPRECEDENTEFACTURE;
            cmds.Parameters.Add("@PERIODEPRECEDENTEFACTURE", SqlDbType.VarChar, 6).Value = laCan.PERIODEPRECEDENTEFACTURE;
            cmds.Parameters.Add("@ORDREAFFICHAGE", SqlDbType.TinyInt).Value = laCan.ORDREAFFICHAGE;
            cmds.Parameters.Add("@NOUVEAUCOMPTEUR", SqlDbType.VarChar, 20).Value = laCan.NOUVEAUCOMPTEUR;
            cmds.Parameters.Add("@PUISSANCEINSTALLEE", SqlDbType.Decimal).Value = laCan.PUISSANCEINSTALLEE;
            cmds.Parameters.Add("@COEFKR1", SqlDbType.Decimal).Value = laCan.COEFKR1;
            cmds.Parameters.Add("@COEFKR2", SqlDbType.Decimal).Value = laCan.COEFKR2;
            cmds.Parameters.Add("@QTEAREGPRECEDENT", SqlDbType.Int).Value = laCan.QTEAREGPRECEDENT;
            cmds.Parameters.Add("@ISCONSOSEULE", SqlDbType.Bit).Value = laCan.ISCONSOSEULE;

            DBBase.SetDBNullParametre(cmds.Parameters);

            try
            {
                if (cmds.Connection.State == ConnectionState.Closed)
                    cmds.Connection.Open();
                cmds.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception(cmds.CommandText + ":" + ex.Message);
            }
        }

        public void InsertOrUpdateDCannalisation(CsCanalisation laCan, SqlCommand cmds)
        {
            cmds.CommandTimeout = 3000;
            cmds.CommandType = CommandType.StoredProcedure;
            cmds.CommandText = "SPX_ACC_INSERTORUPDATE_DCANNALISATION";
            if (cmds.Parameters != null && cmds.Parameters.Count != 0) cmds.Parameters.Clear();

            cmds.Parameters.Add("@CENTRE", SqlDbType.VarChar, Enumere.TailleCentre).Value = laCan.CENTRE;
            cmds.Parameters.Add("@CLIENT", SqlDbType.VarChar, Enumere.TailleClient).Value = laCan.CLIENT;
            cmds.Parameters.Add("@NUMDEM", SqlDbType.VarChar, 20).Value = laCan.NUMDEM;
            cmds.Parameters.Add("@PRODUIT", SqlDbType.VarChar, 2).Value = laCan.PRODUIT;
            cmds.Parameters.Add("@PROPRIO", SqlDbType.VarChar, 1).Value = laCan.PROPRIO;
            cmds.Parameters.Add("@REGLAGECOMPTEUR", SqlDbType.VarChar, 3).Value = laCan.REGLAGECOMPTEUR;
            cmds.Parameters.Add("@POINT", SqlDbType.Int).Value = laCan.POINT;
            cmds.Parameters.Add("@BRANCHEMENT", SqlDbType.VarChar, 14).Value = laCan.BRANCHEMENT;
            cmds.Parameters.Add("@SURFACTURATION", SqlDbType.Int).Value = laCan.SURFACTURATION;
            cmds.Parameters.Add("@DEBITANNUEL", SqlDbType.Int).Value = laCan.DEBITANNUEL;
            cmds.Parameters.Add("@REPERAGECOMPTEUR", SqlDbType.VarChar, 500).Value = laCan.REPERAGECOMPTEUR;
            cmds.Parameters.Add("@POSE", SqlDbType.DateTime).Value = laCan.POSE;
            cmds.Parameters.Add("@DEPOSE", SqlDbType.DateTime).Value = laCan.DEPOSE;
            cmds.Parameters.Add("@USERCREATION", SqlDbType.VarChar, 6).Value = laCan.USERCREATION;
            cmds.Parameters.Add("@USERMODIFICATION", SqlDbType.VarChar, 6).Value = laCan.USERMODIFICATION;
            cmds.Parameters.Add("@FK_IDCENTRE", SqlDbType.Int).Value = laCan.FK_IDCENTRE;
            cmds.Parameters.Add("@FK_IDCOMPTEUR", SqlDbType.Int).Value = laCan.FK_IDCOMPTEUR;
            cmds.Parameters.Add("@FK_IDPRODUIT", SqlDbType.Int).Value = laCan.FK_IDPRODUIT;
            cmds.Parameters.Add("@FK_IDDEMANDE", SqlDbType.Int).Value = laCan.FK_IDDEMANDE;
            cmds.Parameters.Add("@FK_IDPROPRIETAIRE", SqlDbType.Int).Value = laCan.FK_IDPROPRIETAIRE;
            cmds.Parameters.Add("@FK_IDREGLAGECOMPTEUR", SqlDbType.Int).Value = laCan.FK_IDREGLAGECOMPTEUR;
            cmds.Parameters.Add("@ORDREAFFICHAGE", SqlDbType.TinyInt, Enumere.TailleOrdre).Value = laCan.ORDREAFFICHAGE;
            cmds.Parameters.Add("@FK_IDMAGAZINVIRTUEL", SqlDbType.Int).Value = laCan.FK_IDMAGAZINVIRTUEL;
            cmds.Parameters.Add("@ISMODIFIER", SqlDbType.Bit).Value = laCan.ISMODIFIER;

            DBBase.SetDBNullParametre(cmds.Parameters);

            try
            {
                if (cmds.Connection.State == ConnectionState.Closed)
                    cmds.Connection.Open();
                cmds.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception(cmds.CommandText + ":" + ex.Message);
            }
        }
        public void InsertOrUpdateDSociete(CsSocietePrive laSociete, SqlCommand cmds)
        {
            cmds.CommandTimeout = 3000;
            cmds.CommandType = CommandType.StoredProcedure;
            cmds.CommandText = "SPX_ACC_INSERTORUPDATE_DSOCIETE";
            if (cmds.Parameters != null && cmds.Parameters.Count != 0) cmds.Parameters.Clear();

            cmds.Parameters.Add("@PK_ID", SqlDbType.Int).Value = laSociete.PK_ID;
            cmds.Parameters.Add("@NUMEROREGISTRECOMMERCE", SqlDbType.VarChar, 50).Value = laSociete.NUMEROREGISTRECOMMERCE;
            cmds.Parameters.Add("@FK_IDSTATUTJURIQUE", SqlDbType.Int).Value = laSociete.FK_IDSTATUTJURIQUE;
            cmds.Parameters.Add("@CAPITAL", SqlDbType.Decimal).Value = laSociete.CAPITAL;
            cmds.Parameters.Add("@IDENTIFICATIONFISCALE", SqlDbType.VarChar, 50).Value = laSociete.IDENTIFICATIONFISCALE;
            cmds.Parameters.Add("@DATECREATION", SqlDbType.DateTime).Value = laSociete.DATECREATION;
            cmds.Parameters.Add("@SIEGE", SqlDbType.VarChar, 20).Value = laSociete.SIEGE;
            cmds.Parameters.Add("@FK_IDDEMANDE", SqlDbType.Int).Value = laSociete.FK_IDDEMANDE;
            cmds.Parameters.Add("@NOMMANDATAIRE", SqlDbType.VarChar, 20).Value = laSociete.NOMMANDATAIRE;
            cmds.Parameters.Add("@PRENOMMANDATAIRE", SqlDbType.VarChar, 50).Value = laSociete.PRENOMMANDATAIRE;
            cmds.Parameters.Add("@RANGMANDATAIRE", SqlDbType.VarChar, 50).Value = laSociete.RANGMANDATAIRE;
            cmds.Parameters.Add("@NOMSIGNATAIRE", SqlDbType.VarChar, 50).Value = laSociete.NOMSIGNATAIRE;
            cmds.Parameters.Add("@PRENOMSIGNATAIRE", SqlDbType.VarChar, 50).Value = laSociete.PRENOMSIGNATAIRE;
            cmds.Parameters.Add("@RANGSIGNATAIRE", SqlDbType.VarChar, 20).Value = laSociete.RANGSIGNATAIRE;
            cmds.Parameters.Add("@NOMABON", SqlDbType.VarChar, 50).Value = laSociete.RANGSIGNATAIRE;
            DBBase.SetDBNullParametre(cmds.Parameters);
            try
            {
                if (cmds.Connection.State == ConnectionState.Closed)
                    cmds.Connection.Open();
                cmds.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception(cmd.CommandText + ":" + ex.Message);
            }
        }
        public void InsertOrUpdateDAdmnistration_Institut(CsAdministration_Institut leAdmini, SqlCommand cmds)
        {
            cmds.CommandTimeout = 3000;
            cmds.CommandType = CommandType.StoredProcedure;
            cmds.CommandText = "SPX_ACC_INSERTORUPDATE_DADMINISTRATIONINSTITUT";
            if (cmds.Parameters != null && cmds.Parameters.Count != 0) cmds.Parameters.Clear();

            cmds.Parameters.Add("@NOMMANDATAIRE", SqlDbType.VarChar, 50).Value = leAdmini.NOMMANDATAIRE;
            cmds.Parameters.Add("@PRENOMMANDATAIRE", SqlDbType.VarChar, 50).Value = leAdmini.PRENOMMANDATAIRE;
            cmds.Parameters.Add("@RANGMANDATAIRE", SqlDbType.VarChar, 50).Value = leAdmini.RANGMANDATAIRE;
            cmds.Parameters.Add("@NOMSIGNATAIRE", SqlDbType.VarChar, 50).Value = leAdmini.NOMSIGNATAIRE;
            cmds.Parameters.Add("@PRENOMSIGNATAIRE", SqlDbType.VarChar, 50).Value = leAdmini.PRENOMSIGNATAIRE;
            cmds.Parameters.Add("@RANGSIGNATAIRE", SqlDbType.VarChar, 50).Value = leAdmini.RANGSIGNATAIRE;
            cmds.Parameters.Add("@FK_IDDEMANDE", SqlDbType.Int).Value = leAdmini.FK_IDDEMANDE;
            cmds.Parameters.Add("@NOMABON", SqlDbType.VarChar, 50).Value = leAdmini.NOMABON;
            DBBase.SetDBNullParametre(cmds.Parameters);
            try
            {
                if (cmds.Connection.State == ConnectionState.Closed)
                    cmds.Connection.Open();
                cmds.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                throw new Exception(cmds.CommandText + ":" + ex.Message);
            }
        }
        public void InsertOrUpdateDpersPhysique(CsPersonePhysique laPersphysique, SqlCommand cmds)
        {
            cmds.CommandTimeout = 3000;
            cmds.CommandType = CommandType.StoredProcedure;
            cmds.CommandText = "SPX_ACC_INSERTORUPDATE_DPERSONNEPHYSIQUE";
            if (cmds.Parameters != null && cmds.Parameters.Count != 0) cmds.Parameters.Clear();

            cmds.Parameters.Add("@PK_ID", SqlDbType.Int).Value = laPersphysique.PK_ID;
            cmds.Parameters.Add("@DATENAISSANCE", SqlDbType.DateTime).Value = laPersphysique.DATENAISSANCE;
            cmds.Parameters.Add("@NUMEROPIECEIDENTITE", SqlDbType.VarChar, 50).Value = laPersphysique.NUMEROPIECEIDENTITE;
            cmds.Parameters.Add("@DATEFINVALIDITE", SqlDbType.DateTime).Value = laPersphysique.DATEFINVALIDITE;
            cmds.Parameters.Add("@FK_IDDEMANDE", SqlDbType.Int).Value = laPersphysique.FK_IDDEMANDE;
            cmds.Parameters.Add("@FK_IDPIECEIDENTITE", SqlDbType.Int).Value = laPersphysique.FK_IDPIECEIDENTITE;
            cmds.Parameters.Add("@NOMABON", SqlDbType.VarChar, 50).Value = laPersphysique.NOMABON;

            DBBase.SetDBNullParametre(cmds.Parameters);

            try
            {
                if (cmds.Connection.State == ConnectionState.Closed)
                    cmds.Connection.Open();
                cmds.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                throw new Exception(cmds.CommandText + ":" + ex.Message);
            }
        }
        public void InsertOrUpdateDInfoPropriotaire(CsInfoProprietaire leProprietaire, SqlCommand cmds)
        {
            cmds.CommandTimeout = 3000;
            cmds.CommandType = CommandType.StoredProcedure;
            cmds.CommandText = "SPX_ACC_INSERTORUPDATE_DINFOPROPRIETAIRE";
            if (cmds.Parameters != null && cmds.Parameters.Count != 0) cmds.Parameters.Clear();

            cmds.Parameters.Add("@NOM", SqlDbType.VarChar, 20).Value = leProprietaire.NOM;
            cmds.Parameters.Add("@PRENOM", SqlDbType.VarChar, 50).Value = leProprietaire.PRENOM;
            cmds.Parameters.Add("@DATENAISSANCE", SqlDbType.DateTime).Value = leProprietaire.DATENAISSANCE;
            cmds.Parameters.Add("@NUMEROPIECEIDENTITE", SqlDbType.VarChar, 50).Value = leProprietaire.NUMEROPIECEIDENTITE;
            cmds.Parameters.Add("@DATEFINVALIDITE", SqlDbType.DateTime).Value = leProprietaire.DATEFINVALIDITE;
            cmds.Parameters.Add("@FK_IDDEMANDE", SqlDbType.Int).Value = leProprietaire.FK_IDDEMANDE;
            cmds.Parameters.Add("@FK_IDPIECEIDENTITE", SqlDbType.Int).Value = leProprietaire.FK_IDPIECEIDENTITE;
            cmds.Parameters.Add("@FK_IDNATIONNALITE", SqlDbType.Int).Value = leProprietaire.FK_IDNATIONNALITE;
            cmds.Parameters.Add("@FAX", SqlDbType.VarChar, 20).Value = leProprietaire.FAX;
            cmds.Parameters.Add("@BOITEPOSTALE", SqlDbType.VarChar, 20).Value = leProprietaire.BOITEPOSTALE;
            cmds.Parameters.Add("@EMAIL", SqlDbType.VarChar, 20).Value = leProprietaire.EMAIL;
            cmds.Parameters.Add("@TELEPHONEMOBILE", SqlDbType.VarChar, 20).Value = leProprietaire.TELEPHONEMOBILE;
            cmds.Parameters.Add("@TELEPHONEFIXE", SqlDbType.VarChar, 20).Value = leProprietaire.TELEPHONEFIXE;

            DBBase.SetDBNullParametre(cmds.Parameters);
            try
            {
                if (cmds.Connection.State == ConnectionState.Closed)
                    cmds.Connection.Open();
                cmds.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                throw new Exception(cmds.CommandText + ":" + ex.Message);
            }
        }

        public void InsertOrUpdateElementDevis(ObjELEMENTDEVIS lElementDevis, SqlCommand cmds)
        {

            cmds.CommandTimeout = 3000;
            cmds.CommandType = CommandType.StoredProcedure;
            cmds.CommandText = "SPX_ACC_INSERTORUPDATE_ELEMENTDEVIS";
            if (cmds.Parameters != null && cmds.Parameters.Count != 0) cmds.Parameters.Clear();

            cmds.Parameters.Add("@PK_ID", SqlDbType.Int).Value = lElementDevis.PK_ID;
            cmds.Parameters.Add("@NUMDEM", SqlDbType.VarChar, 30).Value = lElementDevis.NUMDEM;
            cmds.Parameters.Add("@ORDRE", SqlDbType.Int).Value = lElementDevis.ORDRE;
            cmds.Parameters.Add("@NUMFOURNITURE", SqlDbType.VarChar, 40).Value = lElementDevis.NUMFOURNITURE;
            cmds.Parameters.Add("@ISEXTENSION", SqlDbType.Bit).Value = lElementDevis.ISEXTENSION;
            cmds.Parameters.Add("@QUANTITE", SqlDbType.Int).Value = lElementDevis.QUANTITE;
            cmds.Parameters.Add("@QUANTITEREMISENSTOCK", SqlDbType.Int).Value = lElementDevis.QUANTITEREMISENSTOCK;
            cmds.Parameters.Add("@QUANTITECONSOMMEE", SqlDbType.Int).Value = lElementDevis.QUANTITECONSOMMEE;
            cmds.Parameters.Add("@QUANTITEPREVUE", SqlDbType.Int).Value = lElementDevis.QUANTITEPREVUE;
            cmds.Parameters.Add("@QUANTITELIVREE", SqlDbType.Int).Value = lElementDevis.QUANTITELIVREE;
            cmds.Parameters.Add("@TAXE", SqlDbType.Decimal).Value = lElementDevis.TAXE;
            cmds.Parameters.Add("@MONTANT", SqlDbType.Decimal).Value = lElementDevis.MONTANT;
            cmds.Parameters.Add("@DATECREATION", SqlDbType.DateTime).Value = lElementDevis.DATECREATION;
            cmds.Parameters.Add("@DATEMODIFICATION", SqlDbType.DateTime).Value = lElementDevis.DATEMODIFICATION;
            cmds.Parameters.Add("@USERCREATION", SqlDbType.Int).Value = lElementDevis.USERCREATION;
            cmds.Parameters.Add("@USERMODIFICATION", SqlDbType.Int).Value = lElementDevis.USERMODIFICATION;
            cmds.Parameters.Add("@FK_IDDEMANDE", SqlDbType.Int).Value = lElementDevis.FK_IDDEMANDE;
            cmds.Parameters.Add("@FK_IDFOURNITURE", SqlDbType.Int).Value = lElementDevis.FK_IDFOURNITURE == null ? 0 : lElementDevis.FK_IDFOURNITURE;
            cmds.Parameters.Add("@ISFOURNITURE", SqlDbType.Bit).Value = lElementDevis.ISFOURNITURE;
            cmds.Parameters.Add("@ISPOSE", SqlDbType.Bit).Value = lElementDevis.ISPOSE;
            cmds.Parameters.Add("@ISPRESTATION", SqlDbType.Bit).Value = lElementDevis.ISPRESTATION;
            cmds.Parameters.Add("@FK_IDCOUTCOPER", SqlDbType.Int).Value = lElementDevis.FK_IDCOUTCOPER;
            cmds.Parameters.Add("@FK_IDCOPER", SqlDbType.Int).Value = lElementDevis.FK_IDCOPER;
            cmds.Parameters.Add("@FK_IDTAXE", SqlDbType.Int).Value = lElementDevis.FK_IDTAXE;
            cmds.Parameters.Add("@FK_IDTYPEDEMANDE", SqlDbType.Int).Value = lElementDevis.FK_IDTYPEDEMANDE;
            cmds.Parameters.Add("@FK_IDMATERIELDEVIS", SqlDbType.Int).Value = lElementDevis.FK_IDMATERIELDEVIS;
            cmds.Parameters.Add("@MONTANTHT", SqlDbType.Decimal).Value = lElementDevis.MONTANTHT;
            cmds.Parameters.Add("@MONTANTTAXE", SqlDbType.Decimal).Value = lElementDevis.MONTANTTAXE;
            cmds.Parameters.Add("@MONTANTTTC", SqlDbType.Decimal).Value = lElementDevis.MONTANTTTC;
            cmds.Parameters.Add("@NOM", SqlDbType.VarChar, 50).Value = lElementDevis.NOM;
            cmds.Parameters.Add("@COUTUNITAIRE_FOURNITURE", SqlDbType.Decimal).Value = lElementDevis.COUTUNITAIRE_FOURNITURE;
            cmds.Parameters.Add("@COUTUNITAIRE_POSE", SqlDbType.Decimal).Value = lElementDevis.COUTUNITAIRE_POSE;
            cmds.Parameters.Add("@RUBRIQUE", SqlDbType.VarChar).Value = lElementDevis.RUBRIQUE;
            cmds.Parameters.Add("@FK_IDRUBRIQUEDEVIS", SqlDbType.Int).Value = lElementDevis.FK_IDRUBRIQUEDEVIS;
            cmds.Parameters.Add("@ISPM", SqlDbType.Bit).Value = lElementDevis.ISPM;

            DBBase.SetDBNullParametre(cmds.Parameters);
            try
            {
                if (cmds.Connection.State == ConnectionState.Closed)
                    cmds.Connection.Open();
                cmds.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception(cmds.CommandText + ":" + ex.Message);
            }
        }
        public void InsertOrUpdateAffectation(CsAffectationDemandeUser leAffectation, SqlCommand cmds)
        {
            cmds.CommandTimeout = 3000;
            cmds.CommandType = CommandType.StoredProcedure;
            cmds.CommandText = "SPX_ACC_INSERTORUPDATE_AFFECTATION";
            if (cmds.Parameters != null && cmds.Parameters.Count != 0) cmds.Parameters.Clear();

            cmds.Parameters.Add("@PK_ID", SqlDbType.UniqueIdentifier).Value = leAffectation.PK_ID;
            cmds.Parameters.Add("@CODEDEMANDE", SqlDbType.VarChar, 50).Value = leAffectation.CODEDEMANDE;
            cmds.Parameters.Add("@MATRICULEUSER", SqlDbType.VarChar, 5).Value = leAffectation.MATRICULEUSER;
            cmds.Parameters.Add("@FK_IDETAPE", SqlDbType.Int).Value = leAffectation.FK_IDETAPE;
            cmds.Parameters.Add("@OPERATIONID", SqlDbType.UniqueIdentifier).Value = leAffectation.OPERATIONID;
            cmds.Parameters.Add("@WORKFLOWID", SqlDbType.UniqueIdentifier).Value = leAffectation.WORKFLOWID;
            cmds.Parameters.Add("@CENTREID", SqlDbType.Int).Value = leAffectation.CENTREID;
            cmds.Parameters.Add("@FK_IDDEMANDE", SqlDbType.Int).Value = leAffectation.FK_IDDEMANDE;
            cmds.Parameters.Add("@FK_IDETAPEFROM", SqlDbType.Int).Value = leAffectation.FK_IDETAPEFROM;
            cmds.Parameters.Add("@MATRICULEUSERCREATION", SqlDbType.VarChar, 50).Value = leAffectation.MATRICULEUSERCREATION;
            cmds.Parameters.Add("@FK_IDUSERAFFECTER", SqlDbType.Int).Value = leAffectation.FK_IDUSERAFFECTER;
            DBBase.SetDBNullParametre(cmds.Parameters);

            try
            {
                if (cmds.Connection.State == ConnectionState.Closed)
                    cmds.Connection.Open();
                cmds.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception(cmds.CommandText + ":" + ex.Message);
            }
        }
        public void InsertOrUpdateDocumentScane(ObjDOCUMENTSCANNE leDocument, SqlCommand cmds)
        {
            cmds.CommandTimeout = 3000;
            cmds.CommandType = CommandType.StoredProcedure;
            cmds.CommandText = "SPX_ACC_INSERTORUPDATE_DOCSCANNE";
            if (cmds.Parameters != null && cmds.Parameters.Count != 0) cmds.Parameters.Clear();

            cmds.Parameters.Add("@PK_ID", SqlDbType.UniqueIdentifier).Value = leDocument.PK_ID;
            cmds.Parameters.Add("@NOMDOCUMENT", SqlDbType.VarChar, 127).Value = leDocument.NOMDOCUMENT;
            cmds.Parameters.Add("@CONTENU", SqlDbType.VarBinary, int.MaxValue).Value = leDocument.CONTENU;
            cmds.Parameters.Add("@USERCREATION", SqlDbType.VarChar, 6).Value = leDocument.USERCREATION;
            cmds.Parameters.Add("@USERMODIFICATION", SqlDbType.VarChar, 6).Value = leDocument.USERMODIFICATION;
            cmds.Parameters.Add("@FK_IDDEMANDE", SqlDbType.Int).Value = leDocument.FK_IDDEMANDE;
            cmds.Parameters.Add("@FK_IDTYPEDOCUMENT", SqlDbType.Int).Value = leDocument.FK_IDTYPEDOCUMENT;
            cmds.Parameters.Add("@CHEMINCOPY", SqlDbType.VarChar, 100).Value = leDocument.CHEMINCOPY;
            cmds.Parameters.Add("@NOMDUFICHIER", SqlDbType.VarChar, 100).Value = leDocument.NOMDUFICHIER;
            cmds.Parameters.Add("@ismigre", SqlDbType.VarChar, 100).Value = leDocument.ismigre;
            DBBase.SetDBNullParametre(cmds.Parameters);
            try
            {
                if (cmds.Connection.State == ConnectionState.Closed)
                    cmds.Connection.Open();
                cmds.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception(cmds.CommandText + ":" + ex.Message);
            }
        }

        /*
        public void InsertOrUpdateRubriqueDemande(CsDemandeDetailCout leCout, SqlCommand cmds)
        {


            cmds.CommandTimeout = 3000;
            cmds.CommandType = CommandType.StoredProcedure;
            cmds.CommandText = "SPX_ACC_INSERTORUPDATE_RUBRIQUEDEMANDE";
            if (cmds.Parameters != null && cmds.Parameters.Count != 0) cmds.Parameters.Clear();
            cmds.Parameters.Add("@PK_ID", SqlDbType.Int).Value = leCout.PK_ID;
            cmds.Parameters.Add("@NUMDEM", SqlDbType.VarChar, 30).Value = leCout.NUMDEM;
            cmds.Parameters.Add("@CENTRE", SqlDbType.VarChar, 3).Value = leCout.CENTRE;
            cmds.Parameters.Add("@NDOC", SqlDbType.VarChar, 6).Value = leCout.NDOC;
            cmds.Parameters.Add("@REFEM", SqlDbType.VarChar, 6).Value = leCout.REFEM;
            cmds.Parameters.Add("@CLIENT", SqlDbType.VarChar, 20).Value = leCout.CLIENT;
            cmds.Parameters.Add("@ORDRE", SqlDbType.VarChar, 2).Value = leCout.ORDRE;
            cmds.Parameters.Add("@COPER", SqlDbType.VarChar, 3).Value = leCout.COPER;
            cmds.Parameters.Add("@MONTANTHT", SqlDbType.Decimal).Value = leCout.MONTANTHT;
            cmds.Parameters.Add("@MONTANTTAXE", SqlDbType.Decimal).Value = leCout.MONTANTTAXE;
            cmds.Parameters.Add("@TAXE", SqlDbType.VarChar, 2).Value = leCout.TAXE;
            cmds.Parameters.Add("@DATECREATION", SqlDbType.DateTime).Value = leCout.DATECREATION;
            cmds.Parameters.Add("@DATEMODIFICATION", SqlDbType.DateTime).Value = leCout.DATEMODIFICATION;
            cmds.Parameters.Add("@USERCREATION", SqlDbType.VarChar, 6).Value = leCout.USERCREATION;
            cmds.Parameters.Add("@USERMODIFICATION", SqlDbType.VarChar, 6).Value = leCout.USERMODIFICATION;
            cmds.Parameters.Add("@FK_IDCENTRE", SqlDbType.Int).Value = leCout.FK_IDCENTRE;
            cmds.Parameters.Add("@FK_IDTAXE", SqlDbType.Int).Value = leCout.FK_IDTAXE;
            cmds.Parameters.Add("@FK_IDCOPER", SqlDbType.Int).Value = leCout.FK_IDCOPER;
            cmds.Parameters.Add("@FK_IDDEMANDE", SqlDbType.Int).Value = leCout.FK_IDDEMANDE;
            cmds.Parameters.Add("@ISVALIDER", SqlDbType.Bit).Value = leCout.ISVALIDER;
            cmds.Parameters.Add("@ISEXTENSION", SqlDbType.Bit).Value = leCout.ISEXTENSION;
            cmds.Parameters.Add("@DATECAISSE", SqlDbType.DateTime).Value = leCout.DATECAISSE;
            DBBase.SetDBNullParametre(cmds.Parameters);
            try
            {
                if (cmds.Connection.State == ConnectionState.Closed)
                    cmds.Connection.Open();
                cmds.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception(cmd.CommandText + ":" + ex.Message);
            }
        }
*/

        public void InsertOrUpdateRubriqueDemande(CsDemandeDetailCout leCout, SqlCommand cmds)
        {


            cmds.CommandTimeout = 1800;
            cmds.CommandType = CommandType.StoredProcedure;
            cmds.CommandText = "SPX_ACC_INSERTORUPDATE_RUBRIQUEDEMANDE";
            if (cmds.Parameters != null && cmds.Parameters.Count != 0) cmds.Parameters.Clear();
            cmds.Parameters.Add("@PK_ID", SqlDbType.Int).Value = leCout.PK_ID;
            cmds.Parameters.Add("@NUMDEM", SqlDbType.VarChar, 30).Value = leCout.NUMDEM;
            cmds.Parameters.Add("@CENTRE", SqlDbType.VarChar, 3).Value = leCout.CENTRE;
            cmds.Parameters.Add("@NDOC", SqlDbType.VarChar, 6).Value = leCout.NDOC;
            cmds.Parameters.Add("@REFEM", SqlDbType.VarChar, 6).Value = leCout.REFEM;
            cmds.Parameters.Add("@CLIENT", SqlDbType.VarChar, 20).Value = leCout.CLIENT;
            cmds.Parameters.Add("@ORDRE", SqlDbType.VarChar, 2).Value = leCout.ORDRE;
            cmds.Parameters.Add("@COPER", SqlDbType.VarChar, 3).Value = leCout.COPER;
            cmds.Parameters.Add("@MONTANTHT", SqlDbType.Decimal).Value = leCout.MONTANTHT;
            cmds.Parameters.Add("@MONTANTTAXE", SqlDbType.Decimal).Value = leCout.MONTANTTAXE;
            cmds.Parameters.Add("@TAXE", SqlDbType.VarChar, 2).Value = leCout.TAXE;
            cmds.Parameters.Add("@DATECREATION", SqlDbType.DateTime).Value = leCout.DATECREATION;
            cmds.Parameters.Add("@DATEMODIFICATION", SqlDbType.DateTime).Value = leCout.DATEMODIFICATION;
            cmds.Parameters.Add("@USERCREATION", SqlDbType.VarChar, 6).Value = leCout.USERCREATION;
            cmds.Parameters.Add("@USERMODIFICATION", SqlDbType.VarChar, 6).Value = leCout.USERMODIFICATION;
            cmds.Parameters.Add("@FK_IDCENTRE", SqlDbType.Int).Value = leCout.FK_IDCENTRE;
            cmds.Parameters.Add("@FK_IDTAXE", SqlDbType.Int).Value = leCout.FK_IDTAXE;
            cmds.Parameters.Add("@FK_IDCOPER", SqlDbType.Int).Value = leCout.FK_IDCOPER;
            cmds.Parameters.Add("@FK_IDDEMANDE", SqlDbType.Int).Value = leCout.FK_IDDEMANDE;
            cmds.Parameters.Add("@ISVALIDER", SqlDbType.Bit).Value = leCout.ISVALIDER;
            cmds.Parameters.Add("@ISEXTENSION", SqlDbType.Bit).Value = leCout.ISEXTENSION;
            cmds.Parameters.Add("@DATECAISSE", SqlDbType.DateTime).Value = leCout.DATECAISSE;
            DBBase.SetDBNullParametre(cmds.Parameters);
            try
            {
                if (cmds.Connection.State == ConnectionState.Closed)
                    cmds.Connection.Open();
                cmds.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception(cmd.CommandText + ":" + ex.Message);
            }
        }

        public void InsertOrUpdateProgrammation(Guid idgroupe, bool EstCompteurLivre, bool EstMaterielLivre, bool EstActif, string Recepeteur, string Livreur, string numeroprogramme, CsDemandeBase leProgramme, DateTime pdate, SqlCommand cmds)
        {


            cmds.CommandTimeout = 3000;
            cmds.CommandType = CommandType.StoredProcedure;
            cmds.CommandText = "SPX_ACC_INSERTORUPDATE_PROGRAMATION";
            if (cmds.Parameters != null && cmds.Parameters.Count != 0) cmds.Parameters.Clear();
            cmds.Parameters.Add("@DATEPROGRAMME", SqlDbType.DateTime).Value = pdate;
            cmds.Parameters.Add("@FK_IDEQUIPE", SqlDbType.UniqueIdentifier).Value = idgroupe;
            cmds.Parameters.Add("@FK_IDDEMANDE", SqlDbType.Int).Value = leProgramme.PK_ID;
            cmds.Parameters.Add("@ESTACTIF", SqlDbType.Bit).Value = EstActif;
            cmds.Parameters.Add("@ISCOMPTEURLIVRE", SqlDbType.Bit).Value = EstCompteurLivre;
            cmds.Parameters.Add("@ISMATERIELLIVRE", SqlDbType.Bit).Value = EstMaterielLivre;
            cmds.Parameters.Add("@ISMATERIELEXTLIVRE", SqlDbType.Bit).Value = false;
            cmds.Parameters.Add("@RECPETEURCOMPTEUR", SqlDbType.VarChar, 6).Value = Recepeteur;
            cmds.Parameters.Add("@RECPETEURMATERIEL", SqlDbType.VarChar, 6).Value = Livreur;
            cmds.Parameters.Add("@NUMPROGRAMME", SqlDbType.VarChar, 50).Value = numeroprogramme;


            DBBase.SetDBNullParametre(cmds.Parameters);
            try
            {
                if (cmds.Connection.State == ConnectionState.Closed)
                    cmds.Connection.Open();
                cmds.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception(cmd.CommandText + ":" + ex.Message);
            }
        }
        public void InsertOrUpdateSortieCompteur(int IdLivreur, int IdRecepteur, CsCanalisation lstCompteur, SqlCommand cmds)
        {
            cmds.CommandTimeout = 3000;
            cmds.CommandType = CommandType.StoredProcedure;
            cmds.CommandText = "SPX_ACC_INSERTORUPDATE_SORTIECOMPTEUR";
            if (cmds.Parameters != null && cmds.Parameters.Count != 0) cmds.Parameters.Clear();
            cmds.Parameters.Add("@FK_IDLIVREUR", SqlDbType.Int).Value = IdLivreur;
            cmds.Parameters.Add("@FK_IDRECEPTEUR", SqlDbType.Int).Value = IdRecepteur;
            cmds.Parameters.Add("@FK_IDDEMANDE", SqlDbType.Int).Value = lstCompteur.FK_IDDEMANDE;
            cmds.Parameters.Add("@FK_IDCANALISATION", SqlDbType.Int).Value = lstCompteur.PK_ID;
            cmds.Parameters.Add("@ACTIF", SqlDbType.Bit).Value = true;
            cmds.Parameters.Add("@LIVRE", SqlDbType.VarChar, 20).Value = Enumere.CompteurLivre;
            cmds.Parameters.Add("@RECU", SqlDbType.VarChar, 20).Value = Enumere.CompteurRecu;
            DBBase.SetDBNullParametre(cmds.Parameters);
            try
            {
                if (cmds.Connection.State == ConnectionState.Closed)
                    cmds.Connection.Open();
                cmds.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception(cmd.CommandText + ":" + ex.Message);
            }
        }

        public void RamenerDemandePourProgrammation(string programme, SqlCommand cmds)
        {
            cmds.CommandTimeout = 3000;
            cmds.CommandType = CommandType.StoredProcedure;
            cmds.CommandText = "SPX_ACC_RETOUR_PROGRAMMATION";
            if (cmds.Parameters != null && cmds.Parameters.Count != 0) cmds.Parameters.Clear();
            cmds.Parameters.Add("@NUMPROGRAMME", SqlDbType.VarChar, 20).Value = programme;
            DBBase.SetDBNullParametre(cmds.Parameters);
            try
            {
                if (cmds.Connection.State == ConnectionState.Closed)
                    cmds.Connection.Open();
                cmds.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception(cmd.CommandText + ":" + ex.Message);
            }
        }

        public void InsertOrUpdateDannotation(CsAnnotation leAnnotation, SqlCommand cmds)
        {
            cmds.CommandTimeout = 3000;
            cmds.CommandType = CommandType.StoredProcedure;
            cmds.CommandText = "SPX_ACC_INSERTORUPDATE_DANNOTATION";
            if (cmds.Parameters != null && cmds.Parameters.Count != 0) cmds.Parameters.Clear();
            cmds.Parameters.Add("@COMMENTAIRE", SqlDbType.VarChar, 50).Value = leAnnotation.COMMENTAIRE;
            cmds.Parameters.Add("@USERCREATION", SqlDbType.VarChar, 6).Value = leAnnotation.USERCREATION;
            cmds.Parameters.Add("@USERMODIFICATION", SqlDbType.VarChar, 6).Value = leAnnotation.USERMODIFICATION;
            cmds.Parameters.Add("@MATRICULE", SqlDbType.VarChar, 6).Value = leAnnotation.MATRICULE;
            cmds.Parameters.Add("@FK_IDADMUTILISATEUR", SqlDbType.Int).Value = leAnnotation.FK_IDADMUTILISATEUR;
            cmds.Parameters.Add("@FK_IDDEMANDE", SqlDbType.Int).Value = leAnnotation.FK_IDDEMANDE;
            cmds.Parameters.Add("@FK_IDETAPE", SqlDbType.Int).Value = leAnnotation.FK_IDETAPE;

            DBBase.SetDBNullParametre(cmds.Parameters);
            try
            {
                if (cmds.Connection.State == ConnectionState.Closed)
                    cmds.Connection.Open();
                cmds.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception(cmd.CommandText + ":" + ex.Message);
            }
        }
        public void InsertOrUpdateSortieMateriel(int IdLivreur, int IdRecepteur, CsCanalisation lstCompteur, SqlCommand cmds)
        {
            cmds.CommandTimeout = 3000;
            cmds.CommandType = CommandType.StoredProcedure;
            cmds.CommandText = "SPX_ACC_INSERTORUPDATE_SORTIEMATERIEL";
            if (cmds.Parameters != null && cmds.Parameters.Count != 0) cmds.Parameters.Clear();
            cmds.Parameters.Add("@FK_IDLIVREUR", SqlDbType.Int).Value = IdLivreur;
            cmds.Parameters.Add("@FK_IDRECEPTEUR", SqlDbType.Int).Value = IdRecepteur;
            cmds.Parameters.Add("@FK_IDDEMANDE", SqlDbType.Int).Value = lstCompteur.FK_IDDEMANDE;
            cmds.Parameters.Add("@ACTIF", SqlDbType.Bit).Value = true;
            cmds.Parameters.Add("@LIVRE", SqlDbType.VarChar, 20).Value = Enumere.CompteurLivre;
            cmds.Parameters.Add("@RECU", SqlDbType.VarChar, 20).Value = Enumere.CompteurRecu;
            DBBase.SetDBNullParametre(cmds.Parameters);
            try
            {
                if (cmds.Connection.State == ConnectionState.Closed)
                    cmds.Connection.Open();
                cmds.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception(cmd.CommandText + ":" + ex.Message);
            }
        }
        public void InsertOrUpdateDscelle(CsDscelle leScelle, SqlCommand cmds)
        {
            cmds.CommandTimeout = 3000;
            cmds.CommandType = CommandType.StoredProcedure;
            cmds.CommandText = "SPX_ACC_INSERTORUPDATE_DSCELLE";
            if (cmds.Parameters != null && cmds.Parameters.Count != 0) cmds.Parameters.Clear();
            cmds.Parameters.Add("@NUMDEM", SqlDbType.VarChar, 50).Value = leScelle.NUMDEM;
            cmds.Parameters.Add("@FK_IDCENTRE", SqlDbType.Int).Value = leScelle.FK_IDCENTRE;
            cmds.Parameters.Add("@FK_IDACTIVITE", SqlDbType.Int).Value = leScelle.FK_IDACTIVITE;
            cmds.Parameters.Add("@FK_IDCOULEURSCELLE", SqlDbType.Int).Value = leScelle.FK_IDCOULEURSCELLE;
            cmds.Parameters.Add("@FK_IDDEMANDE", SqlDbType.Int).Value = leScelle.FK_IDDEMANDE;
            cmds.Parameters.Add("@FK_IDAGENT", SqlDbType.Int).Value = leScelle.FK_IDAGENT;
            cmds.Parameters.Add("@NOMBRE_DEM", SqlDbType.Int).Value = leScelle.NOMBRE_DEM;
            cmds.Parameters.Add("@NOMBRE_REC", SqlDbType.Int).Value = leScelle.NOMBRE_REC;
            cmds.Parameters.Add("@PK_ID", SqlDbType.Int).Value = leScelle.PK_ID;
            cmds.Parameters.Add("@FK_IDCENTREFOURNISSEUR", SqlDbType.Int).Value = leScelle.FK_IDCENTREFOURNISSEUR;
            DBBase.SetDBNullParametre(cmds.Parameters);
            try
            {
                if (cmds.Connection.State == ConnectionState.Closed)
                    cmds.Connection.Open();
                cmds.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                new ErrorManager().WriteInLogFile(this, ex.Message);
                throw new Exception(cmds.CommandText + ":" + ex.Message);
            }
        }

        public void InsertOrUpdateAG(CsAg leAg, SqlCommand cmds)
        {
            cmds.CommandTimeout = 3000;
            cmds.CommandType = CommandType.StoredProcedure;
            cmds.CommandText = "SPX_ACC_INSERTORUPDATE_AG";
            if (cmds.Parameters != null && cmds.Parameters.Count != 0) cmds.Parameters.Clear();
            cmds.Parameters.Add("@PK_ID", SqlDbType.Int).Value = leAg.PK_ID;
            cmds.Parameters.Add("@CENTRE", SqlDbType.VarChar, Enumere.TailleCentre).Value = leAg.CENTRE;
            cmds.Parameters.Add("@CLIENT", SqlDbType.VarChar, Enumere.TailleClient).Value = leAg.CLIENT;
            cmds.Parameters.Add("@NOMP", SqlDbType.VarChar, 63).Value = leAg.NOMP;
            cmds.Parameters.Add("@COMMUNE", SqlDbType.VarChar, 5).Value = leAg.COMMUNE;
            cmds.Parameters.Add("@QUARTIER", SqlDbType.VarChar, 5).Value = leAg.QUARTIER;
            cmds.Parameters.Add("@RUE", SqlDbType.VarChar, 5).Value = leAg.RUE;
            cmds.Parameters.Add("@ETAGE", SqlDbType.VarChar, 2).Value = leAg.ETAGE;
            cmds.Parameters.Add("@PORTE",  SqlDbType.VarChar, 5).Value = leAg.PORTE;
            cmds.Parameters.Add("@CADR",  SqlDbType.VarChar, 30).Value = leAg.CADR;
            cmds.Parameters.Add("@REGROU",  SqlDbType.VarChar, 3).Value = leAg.REGROU;
            cmds.Parameters.Add("@CPARC",  SqlDbType.VarChar, 6).Value = leAg.CPARC;
            cmds.Parameters.Add("@DMAJ", SqlDbType.DateTime).Value = leAg.DMAJ;
            cmds.Parameters.Add("@TOURNEE",  SqlDbType.VarChar, 15).Value = leAg.TOURNEE;
            cmds.Parameters.Add("@ORDTOUR", SqlDbType.VarChar, 15).Value = leAg.ORDTOUR;
            cmds.Parameters.Add("@SECTEUR", SqlDbType.VarChar, 5).Value = leAg.SECTEUR;
            cmds.Parameters.Add("@CPOS", SqlDbType.VarChar, 8).Value = leAg.CPOS;
            cmds.Parameters.Add("@TELEPHONE", SqlDbType.VarChar, 15).Value = leAg.TELEPHONE;
            cmds.Parameters.Add("@FAX", SqlDbType.VarChar, 15).Value = leAg.FAX;
            cmds.Parameters.Add("@EMAIL", SqlDbType.VarChar, 30).Value = leAg.EMAIL;
            cmds.Parameters.Add("@USERCREATION", SqlDbType.VarChar, 6).Value = leAg.USERCREATION;
            cmds.Parameters.Add("@USERMODIFICATION", SqlDbType.VarChar, 6).Value = leAg.USERMODIFICATION;
            cmds.Parameters.Add("@FK_IDTOURNEE", SqlDbType.Int ).Value = leAg.FK_IDTOURNEE;
            cmds.Parameters.Add("@FK_IDQUARTIER", SqlDbType.Int).Value = leAg.FK_IDQUARTIER;
            cmds.Parameters.Add("@FK_IDCOMMUNE", SqlDbType.Int).Value = leAg.FK_IDCOMMUNE;
            cmds.Parameters.Add("@FK_IDRUE", SqlDbType.Int).Value = leAg.FK_IDRUE;
            cmds.Parameters.Add("@FK_IDSECTEUR", SqlDbType.Int).Value = leAg.FK_IDSECTEUR;
            cmds.Parameters.Add("@FK_IDCENTRE", SqlDbType.Int).Value = leAg.FK_IDCENTRE;
            cmds.Parameters.Add("@ISACTIF", SqlDbType.Bit ).Value = leAg.ISACTIF;
 
            DBBase.SetDBNullParametre(cmds.Parameters);
            try
            {
                string IdDemande = string.Empty;
                if (cmds.Connection.State == ConnectionState.Closed)
                    cmds.Connection.Open();
                cmds.ExecuteNonQuery();
              
            }
            catch (Exception ex)
            {
                throw new Exception(cmds.CommandText + ":" + ex.Message);
            }
        }

        /*
        public void InsertOrUpdateCLIENT(CsClient  leClient, SqlCommand cmds)
        {
            cmds.CommandTimeout = 3000;
            cmds.CommandType = CommandType.StoredProcedure;
            cmds.CommandText = "SPX_ACC_INSERTORUPDATE_CLIENT";
            if (cmds.Parameters != null && cmds.Parameters.Count != 0) cmds.Parameters.Clear();

            cmds.Parameters.Add("@PK_ID", SqlDbType.Int).Value = leClient.PK_ID;
            cmds.Parameters.Add("@CENTRE", SqlDbType.VarChar, Enumere.TailleCentre).Value = leClient.CENTRE;
            cmds.Parameters.Add("@REFCLIENT", SqlDbType.VarChar, Enumere.TailleClient).Value = leClient.REFCLIENT;
            cmds.Parameters.Add("@ORDRE", SqlDbType.VarChar, Enumere.TailleOrdre).Value = leClient.ORDRE;
            cmds.Parameters.Add("@CODEIDENTIFICATIONNATIONALE", SqlDbType.VarChar, 20).Value = leClient.CODEIDENTIFICATIONNATIONALE;
            cmds.Parameters.Add("@DENABON", SqlDbType.VarChar, 2).Value = leClient.DENABON;
            cmds.Parameters.Add("@NOMABON", SqlDbType.VarChar, 63).Value = leClient.NOMABON;
            cmds.Parameters.Add("@DENMAND", SqlDbType.VarChar,2).Value = leClient.DENMAND;
            cmds.Parameters.Add("@NOMMAND", SqlDbType.VarChar,30).Value = leClient.NOMMAND;
            cmds.Parameters.Add("@ADRMAND1", SqlDbType.VarChar,100).Value = leClient.ADRMAND1;
            cmds.Parameters.Add("@ADRMAND2", SqlDbType.VarChar,100).Value = leClient.ADRMAND2;
            cmds.Parameters.Add("@PROPRIO", SqlDbType.VarChar,1 ).Value = leClient.PROPRIO;
            cmds.Parameters.Add("@CODECONSO", SqlDbType.VarChar,4 ).Value = leClient.CODECONSO;
            cmds.Parameters.Add("@CATEGORIE", SqlDbType.VarChar,2 ).Value = leClient.CATEGORIE;
            cmds.Parameters.Add("@CODERELANCE", SqlDbType.VarChar,1 ).Value = leClient.CODERELANCE;
            cmds.Parameters.Add("@NATIONNALITE", SqlDbType.VarChar,3 ).Value = leClient.NATIONNALITE;
            cmds.Parameters.Add("@TELEPHONE", SqlDbType.VarChar,15).Value = leClient.TELEPHONE;
            cmds.Parameters.Add("@MATRICULE", SqlDbType.VarChar,5 ).Value = leClient.MATRICULE;
            cmds.Parameters.Add("@REGROUPEMENT", SqlDbType.VarChar,10 ).Value = leClient.REGROUPEMENT;
            cmds.Parameters.Add("@USERCREATION", SqlDbType.VarChar,6 ).Value = leClient.USERCREATION;
            cmds.Parameters.Add("@USERMODIFICATION", SqlDbType.VarChar,6 ).Value = leClient.USERMODIFICATION;
            cmds.Parameters.Add("@FK_IDMODEPAIEMENT", SqlDbType.Int  ).Value = leClient.FK_IDMODEPAIEMENT;
            cmds.Parameters.Add("@FK_IDCODECONSO", SqlDbType.Int  ).Value = leClient.FK_IDCODECONSO;
            cmds.Parameters.Add("@FK_IDCATEGORIE", SqlDbType.Int  ).Value = leClient.FK_IDCATEGORIE;
            cmds.Parameters.Add("@FK_IDRELANCE", SqlDbType.Int  ).Value = leClient.FK_IDRELANCE;
            cmds.Parameters.Add("@FK_IDNATIONALITE", SqlDbType.Int  ).Value = leClient.FK_IDNATIONALITE;
            cmds.Parameters.Add("@FK_IDCENTRE", SqlDbType.Int  ).Value = leClient.FK_IDCENTRE;
            cmds.Parameters.Add("@EMAIL", SqlDbType.VarChar ,50  ).Value = leClient.EMAIL;
            cmds.Parameters.Add("@ISFACTUREEMAIL", SqlDbType.Bit   ).Value = leClient.ISFACTUREEMAIL;
            cmds.Parameters.Add("@ISFACTURESMS", SqlDbType.Bit   ).Value = leClient.ISFACTURESMS;
            cmds.Parameters.Add("@FK_IDREGROUPEMENT", SqlDbType.Int  ).Value = leClient.FK_IDREGROUPEMENT;
            cmds.Parameters.Add("@FK_IDAG", SqlDbType.Int  ).Value = leClient.FK_IDAG;
            cmds.Parameters.Add("@FK_IDPROPRIETAIRE", SqlDbType.Int  ).Value = leClient.FK_IDPROPRIETAIRE;
            cmds.Parameters.Add("@FK_IDPIECEIDENTITE", SqlDbType.Int  ).Value = leClient.FK_IDPIECEIDENTITE;
            cmds.Parameters.Add("@NUMEROPIECEIDENTITE", SqlDbType.VarChar ,20 ).Value = leClient.NUMEROPIECEIDENTITE;
            cmds.Parameters.Add("@NUMPROPRIETE", SqlDbType.VarChar ,20 ).Value = leClient.NUMPROPRIETE;
            cmds.Parameters.Add("@FK_TYPECLIENT", SqlDbType.Int  ).Value = leClient.FK_TYPECLIENT;
            cmds.Parameters.Add("@FAX", SqlDbType.VarChar ,20 ).Value = leClient.FAX;
            cmds.Parameters.Add("@BOITEPOSTAL", SqlDbType.VarChar ,50 ).Value = leClient.BOITEPOSTAL;
            cmds.Parameters.Add("@FK_IDUSAGE", SqlDbType.Int  ).Value = leClient.FK_IDUSAGE;
            cmds.Parameters.Add("@TELEPHONEFIXE", SqlDbType.VarChar ,50 ).Value = leClient.TELEPHONEFIXE;

            DBBase.SetDBNullParametre(cmds.Parameters);
            try
            {
                if (cmds.Connection.State == ConnectionState.Closed)
                    cmds.Connection.Open();
                cmds.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception(cmds.CommandText + ":" + ex.Message);
            }
        }
        */

        public void InsertOrUpdateCLIENT(CsClient leClient, SqlCommand cmds)
        {
            cmds.CommandTimeout = 1800;
            cmds.CommandType = CommandType.StoredProcedure;
            cmds.CommandText = "SPX_ACC_INSERTORUPDATE_CLIENT";
            if (cmds.Parameters != null && cmds.Parameters.Count != 0) cmds.Parameters.Clear();

            cmds.Parameters.Add("@PK_ID", SqlDbType.Int).Value = leClient.PK_ID;
            cmds.Parameters.Add("@CENTRE", SqlDbType.VarChar, Enumere.TailleCentre).Value = leClient.CENTRE;
            cmds.Parameters.Add("@REFCLIENT", SqlDbType.VarChar, Enumere.TailleClient).Value = leClient.REFCLIENT;
            cmds.Parameters.Add("@ORDRE", SqlDbType.VarChar, Enumere.TailleOrdre).Value = leClient.ORDRE;
            cmds.Parameters.Add("@CODEIDENTIFICATIONNATIONALE", SqlDbType.VarChar, 20).Value = leClient.CODEIDENTIFICATIONNATIONALE;
            cmds.Parameters.Add("@DENABON", SqlDbType.VarChar, 6).Value = leClient.DENABON;
            cmds.Parameters.Add("@NOMABON", SqlDbType.VarChar, 63).Value = leClient.NOMABON;
            cmds.Parameters.Add("@DENMAND", SqlDbType.VarChar, 2).Value = leClient.DENMAND;
            cmds.Parameters.Add("@NOMMAND", SqlDbType.VarChar, 30).Value = leClient.NOMMAND;
            cmds.Parameters.Add("@ADRMAND1", SqlDbType.VarChar, 100).Value = leClient.ADRMAND1;
            cmds.Parameters.Add("@ADRMAND2", SqlDbType.VarChar, 100).Value = leClient.ADRMAND2;
            cmds.Parameters.Add("@PROPRIO", SqlDbType.VarChar, 1).Value = leClient.PROPRIO;
            cmds.Parameters.Add("@CODECONSO", SqlDbType.VarChar, 4).Value = leClient.CODECONSO;
            cmds.Parameters.Add("@CATEGORIE", SqlDbType.VarChar, 2).Value = leClient.CATEGORIE;
            cmds.Parameters.Add("@CODERELANCE", SqlDbType.VarChar, 1).Value = leClient.CODERELANCE;
            cmds.Parameters.Add("@NATIONNALITE", SqlDbType.VarChar, 3).Value = leClient.NATIONNALITE;
            cmds.Parameters.Add("@TELEPHONE", SqlDbType.VarChar, 15).Value = leClient.TELEPHONE;
            cmds.Parameters.Add("@MATRICULE", SqlDbType.VarChar, 5).Value = leClient.MATRICULE;
            cmds.Parameters.Add("@REGROUPEMENT", SqlDbType.VarChar, 10).Value = leClient.REGROUPEMENT;
            cmds.Parameters.Add("@USERCREATION", SqlDbType.VarChar, 6).Value = leClient.USERCREATION;
            cmds.Parameters.Add("@USERMODIFICATION", SqlDbType.VarChar, 6).Value = leClient.USERMODIFICATION;
            cmds.Parameters.Add("@FK_IDMODEPAIEMENT", SqlDbType.Int).Value = leClient.FK_IDMODEPAIEMENT;
            cmds.Parameters.Add("@FK_IDCODECONSO", SqlDbType.Int).Value = leClient.FK_IDCODECONSO;
            cmds.Parameters.Add("@FK_IDCATEGORIE", SqlDbType.Int).Value = leClient.FK_IDCATEGORIE;
            cmds.Parameters.Add("@FK_IDRELANCE", SqlDbType.Int).Value = leClient.FK_IDRELANCE;
            cmds.Parameters.Add("@FK_IDNATIONALITE", SqlDbType.Int).Value = leClient.FK_IDNATIONALITE;
            cmds.Parameters.Add("@FK_IDCENTRE", SqlDbType.Int).Value = leClient.FK_IDCENTRE;
            cmds.Parameters.Add("@EMAIL", SqlDbType.VarChar, 50).Value = leClient.EMAIL;
            cmds.Parameters.Add("@ISFACTUREEMAIL", SqlDbType.Bit).Value = leClient.ISFACTUREEMAIL;
            cmds.Parameters.Add("@ISFACTURESMS", SqlDbType.Bit).Value = leClient.ISFACTURESMS;
            cmds.Parameters.Add("@FK_IDREGROUPEMENT", SqlDbType.Int).Value = leClient.FK_IDREGROUPEMENT;
            cmds.Parameters.Add("@FK_IDAG", SqlDbType.Int).Value = leClient.FK_IDAG;
            cmds.Parameters.Add("@FK_IDPROPRIETAIRE", SqlDbType.Int).Value = leClient.FK_IDPROPRIETAIRE;
            cmds.Parameters.Add("@FK_IDPIECEIDENTITE", SqlDbType.Int).Value = leClient.FK_IDPIECEIDENTITE;
            cmds.Parameters.Add("@NUMEROPIECEIDENTITE", SqlDbType.VarChar, 20).Value = leClient.NUMEROPIECEIDENTITE;
            cmds.Parameters.Add("@NUMPROPRIETE", SqlDbType.VarChar, 20).Value = leClient.NUMPROPRIETE;
            cmds.Parameters.Add("@FK_TYPECLIENT", SqlDbType.Int).Value = leClient.FK_TYPECLIENT;
            cmds.Parameters.Add("@FAX", SqlDbType.VarChar, 20).Value = leClient.FAX;
            cmds.Parameters.Add("@BOITEPOSTAL", SqlDbType.VarChar, 50).Value = leClient.BOITEPOSTAL;
            cmds.Parameters.Add("@FK_IDUSAGE", SqlDbType.Int).Value = leClient.FK_IDUSAGE;
            cmds.Parameters.Add("@TELEPHONEFIXE", SqlDbType.VarChar, 50).Value = leClient.TELEPHONEFIXE;

            DBBase.SetDBNullParametre(cmds.Parameters);
            try
            {
                if (cmds.Connection.State == ConnectionState.Closed)
                    cmds.Connection.Open();
                cmds.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception(cmds.CommandText + ":" + ex.Message);
            }
        }
        public void InsertOrUpdateDClient(CsClient leClient, SqlCommand cmds)
        {
            cmds.CommandTimeout = 1800;
            cmds.CommandType = CommandType.StoredProcedure;
            cmds.CommandText = "SPX_ACC_INSERTORUPDATE_DCLIENT";
            if (cmds.Parameters != null && cmds.Parameters.Count != 0) cmds.Parameters.Clear();

            cmds.Parameters.Add("@CENTRE", SqlDbType.VarChar, Enumere.TailleCentre).Value = leClient.CENTRE;
            cmds.Parameters.Add("@REFCLIENT", SqlDbType.VarChar, Enumere.TailleClient).Value = leClient.REFCLIENT;
            cmds.Parameters.Add("@ORDRE", SqlDbType.VarChar, Enumere.TailleOrdre).Value = leClient.ORDRE;
            cmds.Parameters.Add("@CODEIDENTIFICATIONNATIONALE", SqlDbType.VarChar, 20).Value = leClient.CODEIDENTIFICATIONNATIONALE;
            cmds.Parameters.Add("@NUMDEM", SqlDbType.VarChar, 15).Value = leClient.NUMDEM;
            cmds.Parameters.Add("@DENABON", SqlDbType.VarChar, 6).Value = leClient.DENABON;
            cmds.Parameters.Add("@NOMABON", SqlDbType.VarChar, 63).Value = leClient.NOMABON;
            cmds.Parameters.Add("@DENMAND", SqlDbType.VarChar, 2).Value = leClient.DENMAND;
            cmds.Parameters.Add("@NOMMAND", SqlDbType.VarChar, 30).Value = leClient.NOMMAND;
            cmds.Parameters.Add("@ADRMAND1", SqlDbType.VarChar, 100).Value = leClient.ADRMAND1;
            cmds.Parameters.Add("@ADRMAND2", SqlDbType.VarChar, 100).Value = leClient.ADRMAND2;
            cmds.Parameters.Add("@CPOS", SqlDbType.VarChar, 8).Value = leClient.CPOS;
            cmds.Parameters.Add("@BUREAU", SqlDbType.VarChar, 25).Value = leClient.BUREAU;
            cmds.Parameters.Add("@DINC", SqlDbType.DateTime).Value = leClient.DINC;
            cmds.Parameters.Add("@MODEPAIEMENT", SqlDbType.VarChar, 1).Value = leClient.MODEPAIEMENT;
            cmds.Parameters.Add("@NOMTIT", SqlDbType.VarChar, 25).Value = leClient.NOMTIT;
            cmds.Parameters.Add("@BANQUE", SqlDbType.VarChar, 6).Value = leClient.BANQUE;
            cmds.Parameters.Add("@GUICHET", SqlDbType.VarChar, 6).Value = leClient.GUICHET;
            cmds.Parameters.Add("@COMPTE", SqlDbType.VarChar, 20).Value = leClient.COMPTE;
            cmds.Parameters.Add("@RIB", SqlDbType.VarChar, 2).Value = leClient.RIB;
            cmds.Parameters.Add("@PROPRIO", SqlDbType.VarChar, 1).Value = leClient.PROPRIO;
            cmds.Parameters.Add("@CODECONSO", SqlDbType.VarChar, 4).Value = leClient.CODECONSO;
            cmds.Parameters.Add("@CATEGORIE", SqlDbType.VarChar, 2).Value = leClient.CATEGORIE;
            cmds.Parameters.Add("@CODERELANCE", SqlDbType.VarChar, 1).Value = leClient.CODERELANCE;
            cmds.Parameters.Add("@NOMCOD", SqlDbType.VarChar, 6).Value = leClient.NOMCOD;
            cmds.Parameters.Add("@MOISNAIS", SqlDbType.VarChar, 2).Value = leClient.MOISNAIS;
            cmds.Parameters.Add("@ANNAIS", SqlDbType.VarChar, 4).Value = leClient.ANNAIS;
            cmds.Parameters.Add("@NOMPERE", SqlDbType.VarChar, 30).Value = leClient.NOMPERE;
            cmds.Parameters.Add("@NOMMERE", SqlDbType.VarChar, 30).Value = leClient.NOMMERE;
            cmds.Parameters.Add("@NATIONNALITE", SqlDbType.VarChar, 3).Value = leClient.NATIONNALITE;
            cmds.Parameters.Add("@CNI", SqlDbType.VarChar, 30).Value = leClient.CNI;
            cmds.Parameters.Add("@TELEPHONE", SqlDbType.VarChar, 15).Value = leClient.TELEPHONE;
            cmds.Parameters.Add("@MATRICULE", SqlDbType.VarChar, 10).Value = leClient.MATRICULE;
            cmds.Parameters.Add("@REGROUPEMENT", SqlDbType.VarChar, 10).Value = leClient.REGROUPEMENT;
            cmds.Parameters.Add("@REGEDIT", SqlDbType.VarChar, 4).Value = leClient.REGEDIT;
            cmds.Parameters.Add("@FACTURE", SqlDbType.VarChar, 6).Value = leClient.FACTURE;
            cmds.Parameters.Add("@DMAJ", SqlDbType.DateTime).Value = leClient.DMAJ;
            cmds.Parameters.Add("@REFERENCEPUPITRE", SqlDbType.Int).Value = leClient.REFERENCEPUPITRE;
            cmds.Parameters.Add("@PAYEUR", SqlDbType.Int).Value = leClient.PAYEUR;
            cmds.Parameters.Add("@SOUSACTIVITE", SqlDbType.VarChar, 3).Value = leClient.SOUSACTIVITE;
            cmds.Parameters.Add("@AGENTFACTURE", SqlDbType.VarChar, 5).Value = leClient.AGENTFACTURE;
            cmds.Parameters.Add("@AGENTRECOUVR", SqlDbType.VarChar, 5).Value = leClient.AGENTRECOUVR;
            cmds.Parameters.Add("@AGENTASSAINI", SqlDbType.VarChar, 5).Value = leClient.AGENTASSAINI;
            cmds.Parameters.Add("@REGROUCONTRAT", SqlDbType.VarChar, 5).Value = leClient.REGROUCONTRAT;
            cmds.Parameters.Add("@INSPECTION", SqlDbType.VarChar, 3).Value = leClient.INSPECTION;
            cmds.Parameters.Add("@REGLEMENT", SqlDbType.VarChar, 3).Value = leClient.REGLEMENT;
            cmds.Parameters.Add("@DECRET", SqlDbType.VarChar, 6).Value = leClient.DECRET;
            cmds.Parameters.Add("@CONVENTION", SqlDbType.VarChar, 3).Value = leClient.CONVENTION;
            cmds.Parameters.Add("@REFERENCEATM", SqlDbType.VarChar, 3).Value = leClient.REFERENCEATM;
            cmds.Parameters.Add("@DATECREATION", SqlDbType.DateTime).Value = leClient.DATECREATION;
            cmds.Parameters.Add("@DATEMODIFICATION", SqlDbType.DateTime).Value = leClient.DATEMODIFICATION;
            cmds.Parameters.Add("@USERCREATION", SqlDbType.VarChar, 6).Value = leClient.USERCREATION;
            cmds.Parameters.Add("@USERMODIFICATION", SqlDbType.VarChar, 6).Value = leClient.USERMODIFICATION;
            cmds.Parameters.Add("@FK_IDMODEPAIEMENT", SqlDbType.Int).Value = leClient.FK_IDMODEPAIEMENT;
            cmds.Parameters.Add("@FK_IDCODECONSO", SqlDbType.Int).Value = leClient.FK_IDCODECONSO;
            cmds.Parameters.Add("@FK_IDCATEGORIE", SqlDbType.Int).Value = leClient.FK_IDCATEGORIE;
            cmds.Parameters.Add("@FK_IDRELANCE", SqlDbType.Int).Value = leClient.FK_IDRELANCE;
            cmds.Parameters.Add("@FK_IDNATIONALITE", SqlDbType.Int).Value = leClient.FK_IDNATIONALITE;
            cmds.Parameters.Add("@FK_IDCENTRE", SqlDbType.Int).Value = leClient.FK_IDCENTRE;
            cmds.Parameters.Add("@EMAIL", SqlDbType.VarChar, 50).Value = leClient.EMAIL;
            cmds.Parameters.Add("@ISFACTUREEMAIL", SqlDbType.Bit).Value = leClient.ISFACTUREEMAIL;
            cmds.Parameters.Add("@ISFACTURESMS", SqlDbType.Bit).Value = leClient.ISFACTURESMS;
            cmds.Parameters.Add("@FK_IDPAYEUR", SqlDbType.Int).Value = leClient.FK_IDPAYEUR;
            cmds.Parameters.Add("@FK_IDREGROUPEMENT", SqlDbType.Int).Value = leClient.FK_IDREGROUPEMENT;
            cmds.Parameters.Add("@FK_IDPROPRIETAIRE", SqlDbType.Int).Value = leClient.FK_IDPROPRIETAIRE;
            cmds.Parameters.Add("@FK_IDDEMANDE", SqlDbType.Int).Value = leClient.FK_IDNUMDEM;
            cmds.Parameters.Add("@FK_IDPIECEIDENTITE", SqlDbType.Int).Value = leClient.FK_IDPIECEIDENTITE;
            cmds.Parameters.Add("@NUMEROPIECEIDENTITE", SqlDbType.VarChar, 20).Value = leClient.NUMEROPIECEIDENTITE;
            cmds.Parameters.Add("@NUMPROPRIETE", SqlDbType.VarChar, 20).Value = leClient.NUMPROPRIETE;
            cmds.Parameters.Add("@FK_TYPECLIENT", SqlDbType.Int).Value = leClient.FK_TYPECLIENT;
            cmds.Parameters.Add("@FAX", SqlDbType.VarChar, 20).Value = leClient.FAX;
            cmds.Parameters.Add("@BOITEPOSTAL", SqlDbType.VarChar, 50).Value = leClient.BOITEPOSTAL;
            cmds.Parameters.Add("@FK_IDUSAGE", SqlDbType.Int).Value = leClient.FK_IDUSAGE;
            cmds.Parameters.Add("@TELEPHONEFIXE", SqlDbType.VarChar, 50).Value = leClient.TELEPHONEFIXE;
            cmds.Parameters.Add("@FK_IDAG", SqlDbType.Int).Value = leClient.FK_IDAG;
            cmds.Parameters.Add("@ISMODIFIER", SqlDbType.Bit).Value = leClient.ISMODIFIER;
            cmds.Parameters.Add("@PK_ID", SqlDbType.Int).Value = leClient.PK_ID;
            DBBase.SetDBNullParametre(cmds.Parameters);


            try
            {
                int PK_ID = 0;
                if (cmds.Connection.State == ConnectionState.Closed)
                    cmds.Connection.Open();
                cmds.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception(cmds.CommandText + ":" + ex.Message);
            }
        }

        public void UpdateABON(CsAbon leAbon, SqlCommand cmds)
        {
            cmds.CommandTimeout = 3000;
            cmds.CommandType = CommandType.StoredProcedure;
            cmds.CommandText = "SPX_ACC_UPDATE_ABON";
            if (cmds.Parameters != null && cmds.Parameters.Count != 0) cmds.Parameters.Clear();

            cmds.Parameters.Add("@PK_ID", SqlDbType.Int).Value = leAbon.PK_ID;
            cmds.Parameters.Add("@CENTRE", SqlDbType.VarChar, Enumere.TailleCentre).Value = leAbon.CENTRE;
            cmds.Parameters.Add("@CLIENT", SqlDbType.VarChar, Enumere.TailleClient).Value = leAbon.CLIENT;
            cmds.Parameters.Add("@ORDRE", SqlDbType.VarChar, Enumere.TailleOrdre).Value = leAbon.ORDRE;
            cmds.Parameters.Add("@PRODUIT", SqlDbType.VarChar, 2).Value = leAbon.PRODUIT;
            cmds.Parameters.Add("@TYPETARIF", SqlDbType.VarChar, 2).Value = leAbon.TYPETARIF;
            cmds.Parameters.Add("@PUISSANCE", SqlDbType.Decimal).Value = leAbon.PUISSANCE;
            cmds.Parameters.Add("@FORFAIT", SqlDbType.VarChar, 1).Value = leAbon.FORFAIT;
            cmds.Parameters.Add("@FORFPERSO", SqlDbType.VarChar, 6).Value = leAbon.FORFPERSO;
            cmds.Parameters.Add("@AVANCE", SqlDbType.Decimal).Value = leAbon.AVANCE;
            cmds.Parameters.Add("@DAVANCE", SqlDbType.DateTime).Value = leAbon.DAVANCE;
            cmds.Parameters.Add("@PERFAC", SqlDbType.VarChar, 1).Value = leAbon.PERFAC;
            cmds.Parameters.Add("@MOISFAC", SqlDbType.VarChar, 2).Value = leAbon.MOISFAC;
            cmds.Parameters.Add("@DABONNEMENT", SqlDbType.DateTime).Value = leAbon.DABONNEMENT;
            cmds.Parameters.Add("@DRES", SqlDbType.DateTime).Value = leAbon.DRES;
            cmds.Parameters.Add("@PERREL", SqlDbType.VarChar, 1).Value = leAbon.PERREL;
            cmds.Parameters.Add("@MOISREL", SqlDbType.VarChar, 2).Value = leAbon.MOISREL;
            cmds.Parameters.Add("@DMAJ", SqlDbType.DateTime).Value = leAbon.DMAJ;
            cmds.Parameters.Add("@TYPECOMPTAGE", SqlDbType.VarChar, 1).Value = leAbon.TYPECOMPTAGE;
            cmds.Parameters.Add("@FK_IDTYPECOMPTAGE", SqlDbType.Int).Value = leAbon.FK_IDTYPECOMPTAGE;
            cmds.Parameters.Add("@ISBORNEPOSTE", SqlDbType.Bit).Value = leAbon.ISBORNEPOSTE;
            cmds.Parameters.Add("@USERCREATION", SqlDbType.VarChar, 6).Value = leAbon.USERCREATION;
            cmds.Parameters.Add("@USERMODIFICATION", SqlDbType.VarChar, 6).Value = leAbon.USERMODIFICATION;
            cmds.Parameters.Add("@FK_IDCLIENT", SqlDbType.Int).Value = leAbon.FK_IDCLIENT;
            cmds.Parameters.Add("@FK_IDCENTRE", SqlDbType.Int).Value = leAbon.FK_IDCENTRE;
            cmds.Parameters.Add("@FK_IDPRODUIT", SqlDbType.Int).Value = leAbon.FK_IDPRODUIT;
            cmds.Parameters.Add("@FK_IDFORFAIT", SqlDbType.Int).Value = leAbon.FK_IDFORFAIT;
            cmds.Parameters.Add("@FK_IDMOISREL", SqlDbType.Int).Value = leAbon.FK_IDMOISREL;
            cmds.Parameters.Add("@FK_IDMOISFAC", SqlDbType.Int).Value = leAbon.FK_IDMOISFAC;
            cmds.Parameters.Add("@FK_IDTYPETARIF", SqlDbType.Int).Value = leAbon.FK_IDTYPETARIF;
            cmds.Parameters.Add("@FK_IDPERIODICITEFACTURE", SqlDbType.Int).Value = leAbon.FK_IDPERIODICITEFACTURE;
            cmds.Parameters.Add("@FK_IDPERIODICITERELEVE", SqlDbType.Int).Value = leAbon.FK_IDPERIODICITERELEVE;
            cmds.Parameters.Add("@ESTEXONERETVA", SqlDbType.Bit).Value = leAbon.ESTEXONERETVA;
            cmds.Parameters.Add("@DEBUTEXONERATIONTVA", SqlDbType.VarChar, 6).Value = leAbon.DEBUTEXONERATIONTVA;
            cmds.Parameters.Add("@FINEXONERATIONTVA", SqlDbType.VarChar, 6).Value = leAbon.FINEXONERATIONTVA;
            cmds.Parameters.Add("@NOMBREDEFOYER", SqlDbType.Int).Value = leAbon.NOMBREDEFOYER;

            DBBase.SetDBNullParametre(cmds.Parameters);
            try
            {
                if (cmds.Connection.State == ConnectionState.Closed)
                    cmds.Connection.Open();
                cmds.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception(cmds.CommandText + ":" + ex.Message);
            }
        }
        
        public void InsertOrUpdateABON(CsAbon leAbon, SqlCommand cmds)
        {
            cmds.CommandTimeout = 3000;
            cmds.CommandType = CommandType.StoredProcedure;
            cmds.CommandText = "SPX_ACC_INSERTORUPDATE_ABON";
            if (cmds.Parameters != null && cmds.Parameters.Count != 0) cmds.Parameters.Clear();

            cmds.Parameters.Add("@PK_ID", SqlDbType.Int).Value = leAbon.PK_ID;
            cmds.Parameters.Add("@CENTRE", SqlDbType.VarChar, Enumere.TailleCentre).Value = leAbon.CENTRE;
            cmds.Parameters.Add("@CLIENT", SqlDbType.VarChar, Enumere.TailleClient).Value = leAbon.CLIENT;
            cmds.Parameters.Add("@ORDRE", SqlDbType.VarChar, Enumere.TailleOrdre).Value = leAbon.ORDRE;
            cmds.Parameters.Add("@PRODUIT", SqlDbType.VarChar, 2).Value = leAbon.PRODUIT;
            cmds.Parameters.Add("@TYPETARIF", SqlDbType.VarChar, 2).Value = leAbon.TYPETARIF;
            cmds.Parameters.Add("@PUISSANCE", SqlDbType.Decimal ).Value = leAbon.PUISSANCE;
            cmds.Parameters.Add("@FORFAIT", SqlDbType.VarChar, 1).Value = leAbon.FORFAIT;
            cmds.Parameters.Add("@FORFPERSO", SqlDbType.VarChar, 6).Value = leAbon.FORFPERSO;
            cmds.Parameters.Add("@AVANCE", SqlDbType.Decimal ).Value = leAbon.AVANCE;
            cmds.Parameters.Add("@DAVANCE", SqlDbType.DateTime).Value = leAbon.DAVANCE;
            cmds.Parameters.Add("@PERFAC", SqlDbType.VarChar, 1).Value = leAbon.PERFAC;
            cmds.Parameters.Add("@MOISFAC", SqlDbType.VarChar, 2).Value = leAbon.MOISFAC;
            cmds.Parameters.Add("@DABONNEMENT", SqlDbType.DateTime).Value = leAbon.DABONNEMENT;
            cmds.Parameters.Add("@DRES", SqlDbType.DateTime).Value = leAbon.DRES;
            cmds.Parameters.Add("@PERREL", SqlDbType.VarChar, 1).Value = leAbon.PERREL;
            cmds.Parameters.Add("@MOISREL", SqlDbType.VarChar, 2).Value = leAbon.MOISREL;
            cmds.Parameters.Add("@DMAJ", SqlDbType.DateTime).Value = leAbon.DMAJ;
            cmds.Parameters.Add("@TYPECOMPTAGE", SqlDbType.VarChar, 1).Value = leAbon.TYPECOMPTAGE;
            cmds.Parameters.Add("@FK_IDTYPECOMPTAGE", SqlDbType.Int ).Value = leAbon.FK_IDTYPECOMPTAGE;
            cmds.Parameters.Add("@ISBORNEPOSTE", SqlDbType.Bit  ).Value = leAbon.ISBORNEPOSTE;
            cmds.Parameters.Add("@USERCREATION", SqlDbType.VarChar ,6  ).Value = leAbon.USERCREATION;
            cmds.Parameters.Add("@USERMODIFICATION", SqlDbType.VarChar ,6  ).Value = leAbon.USERMODIFICATION;
            cmds.Parameters.Add("@FK_IDCLIENT", SqlDbType.Int).Value = leAbon.FK_IDCLIENT;
            cmds.Parameters.Add("@FK_IDCENTRE", SqlDbType.Int).Value = leAbon.FK_IDCENTRE;
            cmds.Parameters.Add("@FK_IDPRODUIT", SqlDbType.Int ).Value = leAbon.FK_IDPRODUIT;
            cmds.Parameters.Add("@FK_IDFORFAIT", SqlDbType.Int ).Value = leAbon.FK_IDFORFAIT;
            cmds.Parameters.Add("@FK_IDMOISREL", SqlDbType.Int ).Value = leAbon.FK_IDMOISREL;
            cmds.Parameters.Add("@FK_IDMOISFAC", SqlDbType.Int ).Value = leAbon.FK_IDMOISFAC;
            cmds.Parameters.Add("@FK_IDTYPETARIF", SqlDbType.Int ).Value = leAbon.FK_IDTYPETARIF;
            cmds.Parameters.Add("@FK_IDPERIODICITEFACTURE", SqlDbType.Int ).Value = leAbon.FK_IDPERIODICITEFACTURE;
            cmds.Parameters.Add("@FK_IDPERIODICITERELEVE", SqlDbType.Int ).Value = leAbon.FK_IDPERIODICITERELEVE;
            cmds.Parameters.Add("@ESTEXONERETVA", SqlDbType.Bit  ).Value = leAbon.ESTEXONERETVA;
            cmds.Parameters.Add("@DEBUTEXONERATIONTVA", SqlDbType.VarChar ,6 ).Value = leAbon.DEBUTEXONERATIONTVA;
            cmds.Parameters.Add("@FINEXONERATIONTVA", SqlDbType.VarChar ,6 ).Value = leAbon.FINEXONERATIONTVA;
            cmds.Parameters.Add("@NOMBREDEFOYER", SqlDbType.Int  ).Value = leAbon.NOMBREDEFOYER;

            DBBase.SetDBNullParametre(cmds.Parameters);
            try
            {
                if (cmds.Connection.State == ConnectionState.Closed)
                    cmds.Connection.Open();
                cmds.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception(cmds.CommandText + ":" + ex.Message);
            }
        }
        public void InsertOrUpdateBRT(CsBrt leBrt, SqlCommand cmds)
        {
            cmds.CommandTimeout = 3000;
            cmds.CommandType = CommandType.StoredProcedure;
            cmds.CommandText = "SPX_ACC_INSERTORUPDATE_BRT";
            if (cmds.Parameters != null && cmds.Parameters.Count != 0) cmds.Parameters.Clear();

            cmds.Parameters.Add("@PK_ID", SqlDbType.Int).Value = leBrt.PK_ID;
            cmds.Parameters.Add("@CENTRE", SqlDbType.VarChar, Enumere.TailleCentre).Value = leBrt.CENTRE;
            cmds.Parameters.Add("@CLIENT", SqlDbType.VarChar, Enumere.TailleClient).Value = leBrt.CLIENT;
            cmds.Parameters.Add("@PRODUIT", SqlDbType.VarChar, 2).Value = leBrt.PRODUIT;
            cmds.Parameters.Add("@DRAC", SqlDbType.DateTime).Value = leBrt.DRAC;
            cmds.Parameters.Add("@DRES", SqlDbType.DateTime).Value = leBrt.DRES;
            cmds.Parameters.Add("@SERVICE", SqlDbType.VarChar, 1).Value = leBrt.SERVICE;
            cmds.Parameters.Add("@CATBRT", SqlDbType.VarChar, 1).Value = leBrt.CATBRT;
            cmds.Parameters.Add("@DIAMBRT", SqlDbType.VarChar, 1).Value = leBrt.DIAMBRT;
            cmds.Parameters.Add("@LONGBRT", SqlDbType.Decimal).Value = leBrt.LONGBRT;
            cmds.Parameters.Add("@NATBRT", SqlDbType.VarChar, 1).Value = leBrt.NATBRT;
            cmds.Parameters.Add("@NBPOINT", SqlDbType.Int).Value = leBrt.NBPOINT;
            cmds.Parameters.Add("@RESEAU", SqlDbType.VarChar, 2).Value = leBrt.RESEAU;
            cmds.Parameters.Add("@TRONCON", SqlDbType.VarChar, 2).Value = leBrt.TRONCON;
            cmds.Parameters.Add("@DMAJ", SqlDbType.DateTime).Value = leBrt.DMAJ;
            cmds.Parameters.Add("@NBRETRANSFO", SqlDbType.Int).Value = leBrt.NOMBRETRANSFORMATEUR;
            cmds.Parameters.Add("@PUISSANCEINSTALLEE", SqlDbType.Decimal).Value = leBrt.PUISSANCEINSTALLEE;
            cmds.Parameters.Add("@COEFPERTES", SqlDbType.Decimal).Value = leBrt.COEFPERTES;
            cmds.Parameters.Add("@PERTES", SqlDbType.Decimal).Value = leBrt.PERTES;
            cmds.Parameters.Add("@APPTRANSFO", SqlDbType.VarChar, 1).Value = leBrt.APPTRANSFO;
            cmds.Parameters.Add("@CODEBRT", SqlDbType.VarChar, 5).Value = leBrt.CODEBRT;
            cmds.Parameters.Add("@CODEPOSTE", SqlDbType.VarChar, 5).Value = leBrt.CODEPOSTE;
            cmds.Parameters.Add("@ANFAB", SqlDbType.VarChar, 5).Value = leBrt.ANFAB;
            cmds.Parameters.Add("@LONGITUDE", SqlDbType.VarChar, 10).Value = leBrt.LONGITUDE;
            cmds.Parameters.Add("@LATITUDE", SqlDbType.VarChar, 10).Value = leBrt.LATITUDE;
            cmds.Parameters.Add("@ADRESSERESEAU", SqlDbType.VarChar, 20).Value = leBrt.ADRESSERESEAU;
            cmds.Parameters.Add("@USERCREATION", SqlDbType.VarChar, 6).Value = leBrt.USERCREATION;
            cmds.Parameters.Add("@USERMODIFICATION", SqlDbType.VarChar, 6).Value = leBrt.USERMODIFICATION;
            cmds.Parameters.Add("@FK_IDCENTRE", SqlDbType.Int).Value = leBrt.FK_IDCENTRE;
            cmds.Parameters.Add("@FK_IDPRODUIT", SqlDbType.Int).Value = leBrt.FK_IDPRODUIT;
            cmds.Parameters.Add("@FK_IDTYPEBRANCHEMENT", SqlDbType.Int).Value = leBrt.FK_IDTYPEBRANCHEMENT;
            cmds.Parameters.Add("@FK_IDAG", SqlDbType.Int).Value = leBrt.FK_IDAG;
            cmds.Parameters.Add("@NOMBRETRANSFORMATEUR", SqlDbType.Int).Value = leBrt.NOMBRETRANSFORMATEUR;
            cmds.Parameters.Add("@FK_IDPOSTESOURCE", SqlDbType.Int).Value = leBrt.FK_IDPOSTESOURCE;
            cmds.Parameters.Add("@FK_IDDEPARTHTA", SqlDbType.Int).Value = leBrt.FK_IDDEPARTHTA;
            cmds.Parameters.Add("@FK_IDQUARTIER", SqlDbType.Int).Value = leBrt.FK_IDQUARTIER;
            cmds.Parameters.Add("@FK_IDPOSTETRANSFORMATION", SqlDbType.Int).Value = leBrt.FK_IDPOSTETRANSFORMATION;
            cmds.Parameters.Add("@DEPARTBT", SqlDbType.VarChar, 6).Value = leBrt.DEPARTBT;
            cmds.Parameters.Add("@NEOUDFINAL", SqlDbType.VarChar, 6).Value = leBrt.NEOUDFINAL;

            DBBase.SetDBNullParametre(cmds.Parameters);
            try
            {
                string IdDemande = string.Empty;
                if (cmds.Connection.State == ConnectionState.Closed)
                    cmds.Connection.Open();
                cmds.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception(cmds.CommandText + ":" + ex.Message);
            }
        }
        public int  InsertOrUpdateCOMPTEUR(CsCompteur  leCPT, SqlCommand cmds)
        {
            cmds.CommandTimeout = 3000;
            cmds.CommandType = CommandType.StoredProcedure;
            cmds.CommandText = "SPX_ACC_INSERTORUPDATE_COMPTEUR";
            if (cmds.Parameters != null && cmds.Parameters.Count != 0) cmds.Parameters.Clear();

            SqlParameter outResult = new SqlParameter("@PK_ID", SqlDbType.VarChar, int.MaxValue) { Direction = ParameterDirection.Output };
            outResult.Value = leCPT.PK_ID;
            cmds.Parameters.Add(outResult);
           
            cmds.Parameters.Add("@MISEENSERVICE", SqlDbType.DateTime  ).Value = leCPT.MISEENSERVICE ;
            cmds.Parameters.Add("@PRODUIT", SqlDbType.VarChar, 2).Value = leCPT.CODEPRODUIT;
            cmds.Parameters.Add("@NUMERO", SqlDbType.VarChar, 20).Value = leCPT.NUMERO;
            cmds.Parameters.Add("@TYPECOMPTEUR", SqlDbType.VarChar, 1).Value = leCPT.TYPECOMPTEUR;
            cmds.Parameters.Add("@MARQUE", SqlDbType.VarChar, 2).Value = leCPT.MARQUE;
            cmds.Parameters.Add("@COEFLECT", SqlDbType.Decimal ).Value = leCPT.COEFLECT;
            cmds.Parameters.Add("@COEFCOMPTAGE", SqlDbType.Int ).Value = leCPT.COEFCOMPTAGE ;
            cmds.Parameters.Add("@CADRAN", SqlDbType.TinyInt  ).Value = leCPT.CADRAN ;
            cmds.Parameters.Add("@ANNEEFAB", SqlDbType.VarChar, 4).Value = leCPT.ANNEEFAB;
            cmds.Parameters.Add("@STATUT", SqlDbType.VarChar, 1).Value = leCPT.STATUT;
            cmds.Parameters.Add("@FONCTIONNEMENT", SqlDbType.VarChar, 1).Value = leCPT.FONCTIONNEMENT;
            cmds.Parameters.Add("@PLOMBAGE", SqlDbType.VarChar, 3).Value = leCPT.PLOMBAGE;
            cmds.Parameters.Add("@USERCREATION", SqlDbType.VarChar, 6).Value = leCPT.USERCREATION;
            cmds.Parameters.Add("@USERMODIFICATION", SqlDbType.VarChar, 6).Value = leCPT.USERMODIFICATION;
            cmds.Parameters.Add("@FK_IDTYPECOMPTEUR", SqlDbType.Int   ).Value = leCPT.FK_IDTYPECOMPTEUR ;
            cmds.Parameters.Add("@FK_IDPRODUIT", SqlDbType.Int   ).Value = leCPT.FK_IDPRODUIT ;
            cmds.Parameters.Add("@FK_IDMARQUECOMPTEUR", SqlDbType.Int   ).Value = leCPT.FK_IDMARQUECOMPTEUR ;
            cmds.Parameters.Add("@FK_IDCALIBRE", SqlDbType.Int   ).Value = leCPT.FK_IDCALIBRECOMPTEUR  ;
            cmds.Parameters.Add("@FK_IDSTATUTCOMPTEUR", SqlDbType.Int   ).Value = leCPT.FK_IDSTATUTCOMPTEUR ;
            cmds.Parameters.Add("@FK_IDETATCOMPTEUR", SqlDbType.Int   ).Value = leCPT.FK_IDETATCOMPTEUR ;

            DBBase.SetDBNullParametre(cmds.Parameters);
            try
            {
                string IdDemande = string.Empty;
                if (cmds.Connection.State == ConnectionState.Closed)
                    cmds.Connection.Open();
                cmds.ExecuteNonQuery();
                IdDemande = outResult.Value != null ? outResult.Value.ToString() : "1";
                return int.Parse(IdDemande);
            }
            catch (Exception ex)
            {
                throw new Exception(cmds.CommandText + ":" + ex.Message);
            }
        }
        public void InsertOrUpdateCANALISATION(CsCanalisation  leCann, SqlCommand cmds)
        {
            cmds.CommandTimeout = 3000;
            cmds.CommandType = CommandType.StoredProcedure;
            cmds.CommandText = "SPX_ACC_INSERTORUPDATE_CANALISATION";
            if (cmds.Parameters != null && cmds.Parameters.Count != 0) cmds.Parameters.Clear();

            cmds.Parameters.Add("@PK_ID", SqlDbType.Int).Value = leCann.PK_ID;
            cmds.Parameters.Add("@CENTRE", SqlDbType.VarChar ,3).Value = leCann.CENTRE ;
            cmds.Parameters.Add("@CLIENT", SqlDbType.VarChar ,20).Value = leCann.CLIENT ;
            cmds.Parameters.Add("@PRODUIT", SqlDbType.VarChar ,2).Value = leCann.PRODUIT;
            cmds.Parameters.Add("@PROPRIO", SqlDbType.VarChar ,1).Value = leCann.PROPRIO;
            cmds.Parameters.Add("@REGLAGECOMPTEUR", SqlDbType.VarChar ,3).Value = leCann.REGLAGECOMPTEUR;
            cmds.Parameters.Add("@POINT", SqlDbType.Int ).Value = leCann.POINT;
            cmds.Parameters.Add("@BRANCHEMENT", SqlDbType.VarChar ,14).Value = leCann.BRANCHEMENT;
            cmds.Parameters.Add("@SURFACTURATION", SqlDbType.Int ).Value = leCann.SURFACTURATION;
            cmds.Parameters.Add("@DEBITANNUEL", SqlDbType.Int ).Value = leCann.DEBITANNUEL;
            cmds.Parameters.Add("@REPERAGECOMPTEUR", SqlDbType.VarChar ,14).Value = leCann.REPERAGECOMPTEUR;
            cmds.Parameters.Add("@POSE", SqlDbType.DateTime ).Value = leCann.POSE;
            cmds.Parameters.Add("@DEPOSE", SqlDbType.DateTime ).Value = leCann.DEPOSE;
            cmds.Parameters.Add("@USERCREATION", SqlDbType.VarChar ,5).Value = leCann.USERCREATION;
            cmds.Parameters.Add("@USERMODIFICATION", SqlDbType.VarChar ,5).Value = leCann.USERMODIFICATION;
            cmds.Parameters.Add("@FK_IDABON", SqlDbType.Int ).Value = leCann.FK_IDABON;
            cmds.Parameters.Add("@FK_IDCENTRE", SqlDbType.Int ).Value = leCann.FK_IDCENTRE;
            cmds.Parameters.Add("@FK_IDCOMPTEUR", SqlDbType.Int ).Value = leCann.FK_IDCOMPTEUR;
            cmds.Parameters.Add("@FK_IDPRODUIT", SqlDbType.Int ).Value = leCann.FK_IDPRODUIT;
            cmds.Parameters.Add("@FK_IDPROPRIETAIRE", SqlDbType.Int ).Value = leCann.FK_IDPROPRIETAIRE;
            cmds.Parameters.Add("@FK_IDREGLAGECOMPTEUR", SqlDbType.Int ).Value = leCann.FK_IDREGLAGECOMPTEUR;
            cmds.Parameters.Add("@ORDREAFFICHAGE", SqlDbType.TinyInt  ).Value = leCann.ORDREAFFICHAGE;

            DBBase.SetDBNullParametre(cmds.Parameters);
            try
            {
                string IdDemande = string.Empty;
                if (cmds.Connection.State == ConnectionState.Closed)
                    cmds.Connection.Open();
                cmds.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception(cmds.CommandText + ":" + ex.Message);
            }
        }
        public void InsertEVENEMENT(CsEvenement laCan, SqlCommand cmds)
        {
            cmds.CommandTimeout = 3000;
            cmds.CommandType = CommandType.StoredProcedure;
            cmds.CommandText = "SPX_ACC_INSERT_EVENEMENT";
            if (cmds.Parameters != null && cmds.Parameters.Count != 0) cmds.Parameters.Clear();

            cmds.Parameters.Add("@CENTRE", SqlDbType.VarChar, Enumere.TailleCentre).Value = laCan.CENTRE;
            cmds.Parameters.Add("@CLIENT", SqlDbType.VarChar, Enumere.TailleClient).Value = laCan.CLIENT;
            cmds.Parameters.Add("@PRODUIT", SqlDbType.VarChar, 20).Value = laCan.PRODUIT;
            cmds.Parameters.Add("@POINT", SqlDbType.Int).Value = laCan.POINT;
            cmds.Parameters.Add("@NUMEVENEMENT", SqlDbType.Int).Value = laCan.NUMEVENEMENT;
            cmds.Parameters.Add("@ORDRE", SqlDbType.VarChar, 2).Value = laCan.ORDRE;
            cmds.Parameters.Add("@COMPTEUR", SqlDbType.VarChar, 20).Value = laCan.COMPTEUR;
            cmds.Parameters.Add("@DATEEVT", SqlDbType.DateTime).Value = laCan.DATEEVT;
            cmds.Parameters.Add("@PERIODE", SqlDbType.VarChar, 6).Value = laCan.PERIODE;
            cmds.Parameters.Add("@CODEEVT", SqlDbType.VarChar, 2).Value = laCan.CODEEVT;
            cmds.Parameters.Add("@INDEXEVT", SqlDbType.Int).Value = laCan.INDEXEVT;
            cmds.Parameters.Add("@CAS", SqlDbType.VarChar, 2).Value = laCan.CAS;
            cmds.Parameters.Add("@ENQUETE", SqlDbType.VarChar, 1).Value = laCan.ENQUETE;
            cmds.Parameters.Add("@CONSO", SqlDbType.Int).Value = laCan.CONSO;
            cmds.Parameters.Add("@CONSONONFACTUREE", SqlDbType.Int).Value = laCan.CONSONONFACTUREE;
            cmds.Parameters.Add("@LOTRI", SqlDbType.VarChar, 8).Value = laCan.LOTRI;
            cmds.Parameters.Add("@FACTURE", SqlDbType.VarChar, 6).Value = laCan.FACTURE;
            cmds.Parameters.Add("@SURFACTURATION", SqlDbType.Int).Value = laCan.SURFACTURATION;
            cmds.Parameters.Add("@STATUS", SqlDbType.Int).Value = laCan.STATUS;
            cmds.Parameters.Add("@TYPECONSO", SqlDbType.Int).Value = laCan.TYPECONSO;
            cmds.Parameters.Add("@REGLAGECOMPTEUR", SqlDbType.VarChar, 3).Value = laCan.REGLAGECOMPTEUR;
            cmds.Parameters.Add("@TYPETARIF", SqlDbType.VarChar, 2).Value = laCan.TYPETARIF;
            cmds.Parameters.Add("@FORFAIT", SqlDbType.VarChar, 1).Value = laCan.FORFAIT;
            cmds.Parameters.Add("@CATEGORIE", SqlDbType.VarChar, 2).Value = laCan.CATEGORIE;
            cmds.Parameters.Add("@CODECONSO", SqlDbType.VarChar, 4).Value = laCan.CODECONSO;
            cmds.Parameters.Add("@PROPRIO", SqlDbType.VarChar, 1).Value = laCan.PROPRIO;
            cmds.Parameters.Add("@MODEPAIEMENT", SqlDbType.VarChar, 1).Value = laCan.MODEPAIEMENT;
            cmds.Parameters.Add("@MATRICULE", SqlDbType.VarChar, 5).Value = laCan.MATRICULE;
            cmds.Parameters.Add("@FACPER", SqlDbType.VarChar, 6).Value = laCan.FACPER;
            cmds.Parameters.Add("@QTEAREG", SqlDbType.Int).Value = laCan.QTEAREG;
            cmds.Parameters.Add("@DERPERF", SqlDbType.VarChar, 6).Value = laCan.DERPERF;
            cmds.Parameters.Add("@DERPERFN", SqlDbType.VarChar, 6).Value = laCan.DERPERFN;
            cmds.Parameters.Add("@CONSOFAC", SqlDbType.Int).Value = laCan.CONSOFAC;
            cmds.Parameters.Add("@REGIMPUTE", SqlDbType.Int).Value = laCan.REGIMPUTE;
            cmds.Parameters.Add("@REGCONSO", SqlDbType.Int).Value = laCan.REGCONSO;
            cmds.Parameters.Add("@COEFLECT", SqlDbType.Decimal).Value = laCan.COEFLECT;
            cmds.Parameters.Add("@COEFCOMPTAGE", SqlDbType.Int).Value = laCan.COEFCOMPTAGE;
            cmds.Parameters.Add("@PUISSANCE", SqlDbType.Decimal).Value = laCan.PUISSANCE;
            cmds.Parameters.Add("@TYPECOMPTAGE", SqlDbType.VarChar, 1).Value = laCan.TYPECOMPTAGE;
            cmds.Parameters.Add("@TYPECOMPTEUR", SqlDbType.VarChar, 1).Value = laCan.TYPECOMPTEUR;
            cmds.Parameters.Add("@COEFK1", SqlDbType.Decimal).Value = laCan.COEFK1;
            cmds.Parameters.Add("@COEFK2", SqlDbType.Decimal).Value = laCan.COEFK2;
            cmds.Parameters.Add("@COEFKR1", SqlDbType.Decimal).Value = laCan.COEFKR1;
            cmds.Parameters.Add("@COEFKR2", SqlDbType.Decimal).Value = laCan.COEFKR2;
            cmds.Parameters.Add("@COEFFAC", SqlDbType.Decimal).Value = laCan.COEFFAC;
            cmds.Parameters.Add("@USERCREATION", SqlDbType.VarChar, 6).Value = laCan.USERCREATION;
            cmds.Parameters.Add("@USERMODIFICATION", SqlDbType.VarChar, 6).Value = laCan.USERMODIFICATION;
            cmds.Parameters.Add("@FK_IDCANALISATION", SqlDbType.Int).Value = laCan.FK_IDCANALISATION;
            cmds.Parameters.Add("@FK_IDABON", SqlDbType.Int).Value = laCan.FK_IDABON;
            cmds.Parameters.Add("@FK_IDCOMPTEUR", SqlDbType.Int).Value = laCan.FK_IDCOMPTEUR;
            cmds.Parameters.Add("@FK_IDCENTRE", SqlDbType.Int).Value = laCan.FK_IDCENTRE;
            cmds.Parameters.Add("@FK_IDPRODUIT", SqlDbType.Int).Value = laCan.FK_IDPRODUIT;
            cmds.Parameters.Add("@ESTCONSORELEVEE", SqlDbType.Bit).Value = laCan.ESTCONSORELEVEE;
            cmds.Parameters.Add("@FK_IDTOURNEE", SqlDbType.Int).Value = laCan.FK_IDTOURNEE;
            cmds.Parameters.Add("@TOURNEE", SqlDbType.VarChar, 15).Value = laCan.TOURNEE;
            cmds.Parameters.Add("@ORDTOUR", SqlDbType.VarChar, 15).Value = laCan.ORDTOUR;
            cmds.Parameters.Add("@PERFAC", SqlDbType.VarChar, 1).Value = laCan.PERFAC;
            cmds.Parameters.Add("@CONSOMOYENNEPRECEDENTEFACTURE", SqlDbType.Int).Value = laCan.CONSOMOYENNEPRECEDENTEFACTURE;
            cmds.Parameters.Add("@DATERELEVEPRECEDENTEFACTURE", SqlDbType.DateTime).Value = laCan.DATERELEVEPRECEDENTEFACTURE;
            cmds.Parameters.Add("@INDEXPRECEDENTEFACTURE", SqlDbType.Int).Value = laCan.INDEXPRECEDENTEFACTURE;
            cmds.Parameters.Add("@CASPRECEDENTEFACTURE", SqlDbType.VarChar, 2).Value = laCan.CASPRECEDENTEFACTURE;
            cmds.Parameters.Add("@PERIODEPRECEDENTEFACTURE", SqlDbType.VarChar, 6).Value = laCan.PERIODEPRECEDENTEFACTURE;
            cmds.Parameters.Add("@ORDREAFFICHAGE", SqlDbType.TinyInt).Value = laCan.ORDREAFFICHAGE;
            cmds.Parameters.Add("@NOUVEAUCOMPTEUR", SqlDbType.VarChar, 20).Value = laCan.NOUVEAUCOMPTEUR;
            cmds.Parameters.Add("@PUISSANCEINSTALLEE", SqlDbType.Decimal).Value = laCan.PUISSANCEINSTALLEE;
            cmds.Parameters.Add("@QTEAREGPRECEDENT", SqlDbType.Int).Value = laCan.QTEAREGPRECEDENT;
            cmds.Parameters.Add("@ISCONSOSEULE", SqlDbType.Bit).Value = laCan.ISCONSOSEULE;
            cmds.Parameters.Add("@ISEXONERETVA", SqlDbType.Bit).Value = laCan.ISEXONERETVA;
            cmds.Parameters.Add("@DEBUTEXONERATIONTVA", SqlDbType.VarChar, 6).Value = laCan.DEBUTEXONERATIONTVA;
            cmds.Parameters.Add("@FINEXONERATIONTVA", SqlDbType.VarChar, 6).Value = laCan.FINEXONERATIONTVA;
            cmds.Parameters.Add("@COMMENTAIRE", SqlDbType.VarChar, 500).Value = laCan.COMMENTAIRE;
            cmds.Parameters.Add("@NOUVEAUCADRAN", SqlDbType.TinyInt).Value = laCan.NOUVEAUCADRAN;
            DBBase.SetDBNullParametre(cmds.Parameters);

            try
            {
                if (cmds.Connection.State == ConnectionState.Closed)
                    cmds.Connection.Open();
                cmds.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception(cmds.CommandText + ":" + ex.Message);
            }
        }

        public void InsertOrUpdateELEMENTACHATTIMBRE(CsElementAchatTimbre EltTimbre, SqlCommand cmds)
        {
            cmds.CommandTimeout = 3000;
            cmds.CommandType = CommandType.StoredProcedure;
            cmds.CommandText = "SPX_ACC_INSERTORUPDATE_ELEMENTACHATTIMBRE";
 
            if (cmds.Parameters != null && cmds.Parameters.Count != 0) cmds.Parameters.Clear();
		    cmds.Parameters.Add("@NUMDEM", SqlDbType.VarChar,20).Value = EltTimbre.NUMDEM ;
            cmds.Parameters.Add("@QUANTITE", SqlDbType.Int).Value = EltTimbre.QUANTITE  ;
            cmds.Parameters.Add("@TAXE", SqlDbType.Decimal ).Value = EltTimbre.TAXE   ;
            cmds.Parameters.Add("@MONTANT", SqlDbType.Decimal ).Value = EltTimbre.MONTANT   ;
		    cmds.Parameters.Add("@USERCREATION", SqlDbType.VarChar,20).Value = EltTimbre.USERCREATION  ;
		    cmds.Parameters.Add("@USERMODIFICATION", SqlDbType.VarChar,20).Value = EltTimbre.USERMODIFICATION  ;

            cmds.Parameters.Add("@FK_IDDEMANDE", SqlDbType.Int).Value = EltTimbre.FK_IDDEMANDE   ;
            cmds.Parameters.Add("@FK_IDTIMBRE", SqlDbType.Int).Value = EltTimbre.FK_IDTIMBRE   ;
            cmds.Parameters.Add("@FK_IDCOPER", SqlDbType.Int).Value = EltTimbre.FK_IDCOPER   ;
            cmds.Parameters.Add("@FK_IDTAXE", SqlDbType.Int).Value = EltTimbre.FK_IDTAXE   ;

            DBBase.SetDBNullParametre(cmds.Parameters);
            try
            {
                int result = -1;
                if (cmds.Connection.State == ConnectionState.Closed)
                    cmds.Connection.Open();
                    cmds.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception(cmds.CommandText + ":" + ex.Message);
            }
        }

        public bool InsertDEVENEMENT(CsDemande dem, SqlCommand cmds)
        {
            cmds.CommandTimeout = 3000;
            cmds.CommandType = CommandType.StoredProcedure;
            cmds.CommandText = "SPX_ACC_INSERT_DEVENEMENT";
            if (cmds.Parameters != null && cmds.Parameters.Count != 0) cmds.Parameters.Clear();

            cmds.Parameters.Add("@PK_ID", SqlDbType.Int).Value = dem.LstEvenement.First().PK_ID;
            cmds.Parameters.Add("@INDEXEVT", SqlDbType.Int).Value = dem.LstEvenement.First().INDEXEVT;
            cmds.Parameters.Add("@NUMDEM", SqlDbType.VarChar, 20).Value = dem.LstEvenement.First().NUMDEM;
            cmds.Parameters.Add("@FK_IDDEMANDE", SqlDbType.Int).Value = dem.LstEvenement.First().FK_IDDEMANDE;
            cmds.Parameters.Add("@USERCREATION", SqlDbType.VarChar, 6).Value = dem.LstEvenement.First().USERCREATION;
            cmds.Parameters.Add("@INDEXERRONE", SqlDbType.Int).Value = dem.LstEvenement.First().INDEXEVTPRECEDENT;
            cmds.Parameters.Add("@FK_IDABON", SqlDbType.Int).Value = dem.LstEvenement.First().FK_IDABON;
            cmds.Parameters.Add("@TYPEDEMANDE", SqlDbType.VarChar, 2).Value = dem.LaDemande.TYPEDEMANDE;
            cmds.Parameters.Add("@FACTURE", SqlDbType.VarChar, 6).Value = dem.LstEvenement.First().FACTURE;
            cmds.Parameters.Add("@PERIODE", SqlDbType.VarChar, 6).Value = dem.LstEvenement.First().PERIODE;
            cmds.Parameters.Add("@LOTRI", SqlDbType.VarChar, 8).Value = dem.LstEvenement.First().LOTRI;
            DBBase.SetDBNullParametre(cmds.Parameters);

            try
            {
                int result = -1;
                if (cmds.Connection.State == ConnectionState.Closed)
                    cmds.Connection.Open();
                result = cmds.ExecuteNonQuery();
                return (result > 0);
            }
            catch (Exception ex)
            {
                throw new Exception(cmds.CommandText + ":" + ex.Message);
            }
        }

       public bool UpdateDEVENEMENT(CsDemande dem, SqlCommand cmds)
        {
            cmds.CommandTimeout = 3000;
            cmds.CommandType = CommandType.StoredProcedure;
            cmds.CommandText = "SPX_ACC_UPDATE_DEVENEMENT";
            if (cmds.Parameters != null && cmds.Parameters.Count != 0) cmds.Parameters.Clear();

            cmds.Parameters.Add("@PK_ID", SqlDbType.Int).Value = dem.LstEvenement.First().PK_ID;
            cmds.Parameters.Add("@INDEXEVT", SqlDbType.Int).Value = dem.LstEvenement.First().INDEXEVT;
            cmds.Parameters.Add("@FK_IDDEMANDE", SqlDbType.Int).Value = dem.LstEvenement.First().FK_IDDEMANDE;
            cmds.Parameters.Add("@USERMODIFICATION", SqlDbType.VarChar, 6).Value = dem.LstEvenement.First().USERMODIFICATION;
            cmds.Parameters.Add("@INDEXERRONE", SqlDbType.Int).Value = dem.LstEvenement.First().INDEXEVTPRECEDENT;
            cmds.Parameters.Add("@FK_IDABON", SqlDbType.Int).Value = dem.LstEvenement.First().FK_IDABON;
            cmds.Parameters.Add("@TYPEDEMANDE", SqlDbType.VarChar, 2).Value = dem.LaDemande.TYPEDEMANDE;
            cmds.Parameters.Add("@FACTURE", SqlDbType.VarChar, 6).Value = dem.LstEvenement.First().FACTURE;
            cmds.Parameters.Add("@PERIODE", SqlDbType.VarChar, 6).Value = dem.LstEvenement.First().PERIODE;
            cmds.Parameters.Add("@LOTRI", SqlDbType.VarChar, 8).Value = dem.LstEvenement.First().LOTRI;
            DBBase.SetDBNullParametre(cmds.Parameters);

            int result = -1;
            try
            {
                if (cmds.Connection.State == ConnectionState.Closed)
                    cmds.Connection.Open();
                result = cmds.ExecuteNonQuery();
                return (result > 0);
            }
            catch (Exception ex)
            {
                throw new Exception(cmds.CommandText + ":" + ex.Message);
            }
        }

        public void InsertOrUpdateEVENEMENT(CsEvenement laCan, SqlCommand cmds)
        {
            cmds.CommandTimeout = 3000;
            cmds.CommandType = CommandType.StoredProcedure;
            cmds.CommandText = "SPX_ACC_INSERTORUPDATE_EVENEMENT";
            if (cmds.Parameters != null && cmds.Parameters.Count != 0) cmds.Parameters.Clear();

            cmds.Parameters.Add("@CENTRE", SqlDbType.VarChar, Enumere.TailleCentre).Value = laCan.CENTRE;
            cmds.Parameters.Add("@CLIENT", SqlDbType.VarChar, Enumere.TailleClient).Value = laCan.CLIENT;
            cmds.Parameters.Add("@PRODUIT", SqlDbType.VarChar, 20).Value = laCan.PRODUIT;
            cmds.Parameters.Add("@POINT", SqlDbType.Int).Value = laCan.POINT;
            cmds.Parameters.Add("@NUMEVENEMENT", SqlDbType.Int).Value = laCan.NUMEVENEMENT;
            cmds.Parameters.Add("@ORDRE", SqlDbType.VarChar, 2).Value = laCan.ORDRE;
            cmds.Parameters.Add("@COMPTEUR", SqlDbType.VarChar, 20).Value = laCan.COMPTEUR;
            cmds.Parameters.Add("@DATEEVT", SqlDbType.DateTime).Value = laCan.DATEEVT;
            cmds.Parameters.Add("@PERIODE", SqlDbType.VarChar, 6).Value = laCan.PERIODE;
            cmds.Parameters.Add("@CODEEVT", SqlDbType.VarChar, 2).Value = laCan.CODEEVT;
            cmds.Parameters.Add("@INDEXEVT", SqlDbType.Int).Value = laCan.INDEXEVT;
            cmds.Parameters.Add("@CAS", SqlDbType.VarChar, 2).Value = laCan.CAS;
            cmds.Parameters.Add("@ENQUETE", SqlDbType.VarChar, 1).Value = laCan.ENQUETE;
            cmds.Parameters.Add("@CONSO", SqlDbType.Int).Value = laCan.CONSO;
            cmds.Parameters.Add("@CONSONONFACTUREE", SqlDbType.Int).Value = laCan.CONSONONFACTUREE;
            cmds.Parameters.Add("@LOTRI", SqlDbType.VarChar, 8).Value = laCan.LOTRI;
            cmds.Parameters.Add("@FACTURE", SqlDbType.VarChar, 6).Value = laCan.FACTURE;
            cmds.Parameters.Add("@SURFACTURATION", SqlDbType.Int).Value = laCan.SURFACTURATION;
            cmds.Parameters.Add("@STATUS", SqlDbType.Int).Value = laCan.STATUS;
            cmds.Parameters.Add("@TYPECONSO", SqlDbType.Int).Value = laCan.TYPECONSO;
            cmds.Parameters.Add("@REGLAGECOMPTEUR", SqlDbType.VarChar, 3).Value = laCan.REGLAGECOMPTEUR;
            cmds.Parameters.Add("@TYPETARIF", SqlDbType.VarChar, 2).Value = laCan.TYPETARIF;
            cmds.Parameters.Add("@FORFAIT", SqlDbType.VarChar, 1).Value = laCan.FORFAIT;
            cmds.Parameters.Add("@CATEGORIE", SqlDbType.VarChar, 2).Value = laCan.CATEGORIE;
            cmds.Parameters.Add("@CODECONSO", SqlDbType.VarChar, 4).Value = laCan.CODECONSO;
            cmds.Parameters.Add("@PROPRIO", SqlDbType.VarChar, 1).Value = laCan.PROPRIO;
            cmds.Parameters.Add("@MODEPAIEMENT", SqlDbType.VarChar, 1).Value = laCan.MODEPAIEMENT;
            cmds.Parameters.Add("@MATRICULE", SqlDbType.VarChar, 5).Value = laCan.MATRICULE;
            cmds.Parameters.Add("@FACPER", SqlDbType.VarChar, 6).Value = laCan.FACPER;
            cmds.Parameters.Add("@QTEAREG", SqlDbType.Int).Value = laCan.QTEAREG;
            cmds.Parameters.Add("@DERPERF", SqlDbType.VarChar, 6).Value = laCan.DERPERF;
            cmds.Parameters.Add("@DERPERFN", SqlDbType.VarChar, 6).Value = laCan.DERPERFN;
            cmds.Parameters.Add("@CONSOFAC", SqlDbType.Int).Value = laCan.CONSOFAC;
            cmds.Parameters.Add("@REGIMPUTE", SqlDbType.Int).Value = laCan.REGIMPUTE;
            cmds.Parameters.Add("@REGCONSO", SqlDbType.Int).Value = laCan.REGCONSO;
            cmds.Parameters.Add("@COEFLECT", SqlDbType.Decimal).Value = laCan.COEFLECT;
            cmds.Parameters.Add("@COEFCOMPTAGE", SqlDbType.Int).Value = laCan.COEFCOMPTAGE;
            cmds.Parameters.Add("@PUISSANCE", SqlDbType.Decimal).Value = laCan.PUISSANCE;
            cmds.Parameters.Add("@TYPECOMPTAGE", SqlDbType.VarChar, 1).Value = laCan.TYPECOMPTAGE;
            cmds.Parameters.Add("@TYPECOMPTEUR", SqlDbType.VarChar, 1).Value = laCan.TYPECOMPTEUR;
            cmds.Parameters.Add("@COEFK1", SqlDbType.Decimal).Value = laCan.COEFK1;
            cmds.Parameters.Add("@COEFK2", SqlDbType.Decimal).Value = laCan.COEFK2;
            cmds.Parameters.Add("@COEFKR1", SqlDbType.Decimal).Value = laCan.COEFKR1;
            cmds.Parameters.Add("@COEFKR2", SqlDbType.Decimal).Value = laCan.COEFKR2;
            cmds.Parameters.Add("@COEFFAC", SqlDbType.Decimal).Value = laCan.COEFFAC;
            cmds.Parameters.Add("@USERCREATION", SqlDbType.VarChar, 6).Value = laCan.USERCREATION;
            cmds.Parameters.Add("@USERMODIFICATION", SqlDbType.VarChar, 6).Value = laCan.USERMODIFICATION;
            cmds.Parameters.Add("@FK_IDCANALISATION", SqlDbType.Int).Value = laCan.FK_IDCANALISATION;
            cmds.Parameters.Add("@FK_IDABON", SqlDbType.Int).Value = laCan.FK_IDABON;
            cmds.Parameters.Add("@FK_IDCOMPTEUR", SqlDbType.Int).Value = laCan.FK_IDCOMPTEUR;
            cmds.Parameters.Add("@FK_IDCENTRE", SqlDbType.Int).Value = laCan.FK_IDCENTRE;
            cmds.Parameters.Add("@FK_IDPRODUIT", SqlDbType.Int).Value = laCan.FK_IDPRODUIT;
            cmds.Parameters.Add("@ESTCONSORELEVEE", SqlDbType.Bit).Value = laCan.ESTCONSORELEVEE;
            cmds.Parameters.Add("@FK_IDTOURNEE", SqlDbType.Int).Value = laCan.FK_IDTOURNEE;
            cmds.Parameters.Add("@TOURNEE", SqlDbType.VarChar, 15).Value = laCan.TOURNEE;
            cmds.Parameters.Add("@ORDTOUR", SqlDbType.VarChar, 15).Value = laCan.ORDTOUR;
            cmds.Parameters.Add("@PERFAC", SqlDbType.VarChar, 1).Value = laCan.PERFAC;
            cmds.Parameters.Add("@CONSOMOYENNEPRECEDENTEFACTURE", SqlDbType.Int).Value = laCan.CONSOMOYENNEPRECEDENTEFACTURE;
            cmds.Parameters.Add("@DATERELEVEPRECEDENTEFACTURE", SqlDbType.DateTime).Value = laCan.DATERELEVEPRECEDENTEFACTURE;
            cmds.Parameters.Add("@INDEXPRECEDENTEFACTURE", SqlDbType.Int).Value = laCan.INDEXPRECEDENTEFACTURE;
            cmds.Parameters.Add("@CASPRECEDENTEFACTURE", SqlDbType.VarChar, 2).Value = laCan.CASPRECEDENTEFACTURE;
            cmds.Parameters.Add("@PERIODEPRECEDENTEFACTURE", SqlDbType.VarChar, 6).Value = laCan.PERIODEPRECEDENTEFACTURE;
            cmds.Parameters.Add("@ORDREAFFICHAGE", SqlDbType.TinyInt).Value = laCan.ORDREAFFICHAGE;
            cmds.Parameters.Add("@NOUVEAUCOMPTEUR", SqlDbType.VarChar, 20).Value = laCan.NOUVEAUCOMPTEUR;
            cmds.Parameters.Add("@PUISSANCEINSTALLEE", SqlDbType.Decimal).Value = laCan.PUISSANCEINSTALLEE;
            cmds.Parameters.Add("@QTEAREGPRECEDENT", SqlDbType.Int).Value = laCan.QTEAREGPRECEDENT;
            cmds.Parameters.Add("@ISCONSOSEULE", SqlDbType.Bit).Value = laCan.ISCONSOSEULE;
            cmds.Parameters.Add("@ISEXONERETVA", SqlDbType.Bit).Value = laCan.ISEXONERETVA;
            cmds.Parameters.Add("@DEBUTEXONERATIONTVA", SqlDbType.VarChar, 6).Value = laCan.DEBUTEXONERATIONTVA;
            cmds.Parameters.Add("@FINEXONERATIONTVA", SqlDbType.VarChar, 6).Value = laCan.FINEXONERATIONTVA;
            cmds.Parameters.Add("@COMMENTAIRE", SqlDbType.VarChar, 500).Value = laCan.COMMENTAIRE;
            cmds.Parameters.Add("@NOUVEAUCADRAN", SqlDbType.TinyInt).Value = laCan.NOUVEAUCADRAN;
            DBBase.SetDBNullParametre(cmds.Parameters);

            try
            {
                if (cmds.Connection.State == ConnectionState.Closed)
                    cmds.Connection.Open();
                cmds.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception(cmds.CommandText + ":" + ex.Message);
            }
        }
        public void InsertOrUpdatePOSESCELLE(CsOrganeScelleDemande OrganeSCelle, SqlCommand cmds)
        {
            cmds.CommandTimeout = 3000;
            cmds.CommandType = CommandType.StoredProcedure;
            cmds.CommandText = "SPX_ACC_INSERTORUPDATE_DPOSEDCELLE";
            if (cmds.Parameters != null && cmds.Parameters.Count != 0) cmds.Parameters.Clear();

            cmds.Parameters.Add("@NUM_SCELLE", SqlDbType.VarChar, 50).Value = OrganeSCelle.NUM_SCELLE;
            cmds.Parameters.Add("@FK_IDORGANE_SCELLABLE", SqlDbType.Int ).Value = OrganeSCelle.FK_IDORGANE_SCELLABLE;
            cmds.Parameters.Add("@FK_IDDEMANDE", SqlDbType.Int).Value = OrganeSCelle.FK_IDDEMANDE;
            cmds.Parameters.Add("@FK_IDBRT", SqlDbType.Int).Value = OrganeSCelle.FK_IDBRT;
            cmds.Parameters.Add("@NOMBRE", SqlDbType.Int).Value = OrganeSCelle.NOMBRE;
            cmds.Parameters.Add("@USERCREATION", SqlDbType.VarChar ,6).Value = OrganeSCelle.USERCREATION;
            cmds.Parameters.Add("@CERTIFICAT", SqlDbType.VarBinary ,int.MaxValue).Value = OrganeSCelle.CERTIFICAT;
       
            DBBase.SetDBNullParametre(cmds.Parameters);

            try
            {
                if (cmds.Connection.State == ConnectionState.Closed)
                    cmds.Connection.Open();
                cmds.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception(cmds.CommandText + ":" + ex.Message);
            }
        }
        public void InsertOrUpdateCOMPTECLIENTSUITEPV(CsDemandeBase  lademande, SqlCommand cmds)
        {
            cmds.CommandTimeout = 3000;
            cmds.CommandType = CommandType.StoredProcedure;
            cmds.CommandText = "SPX_ACC_INSERTORUPDATE_LCLIENTPV";
            if (cmds.Parameters != null && cmds.Parameters.Count != 0) cmds.Parameters.Clear();

            cmds.Parameters.Add("@NUMDEM", SqlDbType.VarChar, 13).Value = lademande.NUMDEM ;
            cmds.Parameters.Add("@FK_IDDEMANDE", SqlDbType.Int).Value = lademande.PK_ID;
            cmds.Parameters.Add("@MATRICULE", SqlDbType.VarChar, 6).Value = lademande.USERCREATION;
            DBBase.SetDBNullParametre(cmds.Parameters);

            try
            {
                if (cmds.Connection.State == ConnectionState.Closed)
                    cmds.Connection.Open();
                cmds.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception(cmds.CommandText + ":" + ex.Message);
            }
        }

        public void Updatescelle(CsOrganeScelleDemande leScelle, SqlCommand cmds)
        {
            cmds.CommandTimeout = 3000;
            cmds.CommandType = CommandType.StoredProcedure;
            cmds.CommandText = "SPX_ACC_UPDATE_SCELLE";
            if (cmds.Parameters != null && cmds.Parameters.Count != 0) cmds.Parameters.Clear();
            cmds.Parameters.Add("@NUMERO", SqlDbType.VarChar, 50).Value = leScelle.NUM_SCELLE;
            cmds.Parameters.Add("@IDCOULEUR", SqlDbType.Int).Value = leScelle.IDCOULEUR;
            
            DBBase.SetDBNullParametre(cmds.Parameters);
            try
            {
                if (cmds.Connection.State == ConnectionState.Closed)
                    cmds.Connection.Open();
                cmds.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception(cmds.CommandText + ":" + ex.Message);
            }
        }

        public void InsertOrUpdateLotri(CsLotri leLot, SqlCommand cmds)
        {
            cmds.CommandTimeout = 3000;
            cmds.CommandType = CommandType.StoredProcedure;
            cmds.CommandText = "SPX_ACC_INSERTORUPDATE_LOTRI";
            if (cmds.Parameters != null && cmds.Parameters.Count != 0) cmds.Parameters.Clear();

            cmds.Parameters.Add("@NUMLOTRI", SqlDbType.VarChar, 8).Value = leLot.NUMLOTRI;
            cmds.Parameters.Add("@JET", SqlDbType.VarChar, 2).Value = leLot.JET;
            cmds.Parameters.Add("@PERIODE", SqlDbType.VarChar, 6).Value = leLot.PERIODE;

            cmds.Parameters.Add("@CENTRE", SqlDbType.VarChar, 3).Value = leLot.CENTRE;
            cmds.Parameters.Add("@PRODUIT", SqlDbType.VarChar, 2).Value = leLot.PRODUIT;
            cmds.Parameters.Add("@CATEGORIECLIENT", SqlDbType.VarChar, 2).Value = leLot.CATEGORIECLIENT;
            cmds.Parameters.Add("@PERIODICITE", SqlDbType.VarChar, 6).Value = leLot.PERIODICITE;
            cmds.Parameters.Add("@EXIG", SqlDbType.Int).Value = leLot.EXIG;
            cmds.Parameters.Add("@DFAC", SqlDbType.DateTime).Value = leLot.DFAC;
            cmds.Parameters.Add("@ETATFAC1", SqlDbType.VarChar, 1).Value = leLot.ETATFAC1;
            cmds.Parameters.Add("@ETATFAC2", SqlDbType.VarChar, 1).Value = leLot.ETATFAC2;
            cmds.Parameters.Add("@ETATFAC3", SqlDbType.VarChar, 1).Value = leLot.ETATFAC3;
            cmds.Parameters.Add("@ETATFAC4", SqlDbType.VarChar, 1).Value = leLot.ETATFAC4;
            cmds.Parameters.Add("@ETATFAC5", SqlDbType.VarChar, 1).Value = leLot.ETATFAC5;
            cmds.Parameters.Add("@ETATFAC6", SqlDbType.VarChar, 1).Value = leLot.ETATFAC6;
            cmds.Parameters.Add("@ETATFAC7", SqlDbType.VarChar, 1).Value = leLot.ETATFAC7;
            cmds.Parameters.Add("@ETATFAC8", SqlDbType.VarChar, 1).Value = leLot.ETATFAC8;
            cmds.Parameters.Add("@ETATFAC9", SqlDbType.VarChar, 1).Value = leLot.ETATFAC9;
            cmds.Parameters.Add("@ETATFAC10", SqlDbType.VarChar, 1).Value = leLot.ETATFAC10;
            cmds.Parameters.Add("@TOURNEE", SqlDbType.VarChar, 15).Value = leLot.TOURNEE;
            cmds.Parameters.Add("@RELEVEUR", SqlDbType.VarChar, 2).Value = leLot.RELEVEUR;
            cmds.Parameters.Add("@BASE", SqlDbType.VarChar, 1).Value = leLot.BASE;
            cmds.Parameters.Add("@USERCREATION", SqlDbType.VarChar, 6).Value = leLot.USERCREATION;
            cmds.Parameters.Add("@USERMODIFICATION", SqlDbType.VarChar, 6).Value = leLot.USERMODIFICATION;
            cmds.Parameters.Add("@FK_IDPRODUIT", SqlDbType.Int).Value = leLot.FK_IDPRODUIT;
            cmds.Parameters.Add("@FK_IDCATEGORIECLIENT", SqlDbType.Int).Value = leLot.FK_IDCATEGORIECLIENT;
            cmds.Parameters.Add("@FK_IDRELEVEUR", SqlDbType.Int).Value = leLot.FK_IDRELEVEUR;
            cmds.Parameters.Add("@UserCalcul", SqlDbType.VarChar, 5).Value = leLot.UserCalcul;
            cmds.Parameters.Add("@FK_IDTOURNEE", SqlDbType.Int).Value = leLot.FK_IDTOURNEE;
            cmds.Parameters.Add("@FK_IDCENTRE", SqlDbType.Int).Value = leLot.FK_IDCENTRE;


            DBBase.SetDBNullParametre(cmds.Parameters);

            try
            {
                if (cmds.Connection.State == ConnectionState.Closed)
                    cmds.Connection.Open();
                cmds.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception(cmds.CommandText + ":" + ex.Message);
            }
        }

        public void InsertOrUpdateLotri(List<CsLotri> lstLot, SqlCommand cmds)
        {
            cmds.CommandTimeout = 3000;
            cmds.CommandType = CommandType.StoredProcedure;
            cmds.CommandText = "SPX_ACC_INSERTORUPDATE_LOTRI";

            foreach (CsLotri leLot in lstLot)
            {
                if (cmds.Parameters != null && cmds.Parameters.Count != 0) cmds.Parameters.Clear();

                cmds.Parameters.Add("@NUMLOTRI", SqlDbType.VarChar, 8).Value = leLot.NUMLOTRI;
                cmds.Parameters.Add("@JET", SqlDbType.VarChar, 2).Value = leLot.JET;
                cmds.Parameters.Add("@PERIODE", SqlDbType.VarChar, 6).Value = leLot.PERIODE;

                cmds.Parameters.Add("@CENTRE", SqlDbType.VarChar, 3).Value = leLot.CENTRE;
                cmds.Parameters.Add("@PRODUIT", SqlDbType.VarChar, 2).Value = leLot.PRODUIT;
                cmds.Parameters.Add("@CATEGORIECLIENT", SqlDbType.VarChar, 2).Value = leLot.CATEGORIECLIENT;
                cmds.Parameters.Add("@PERIODICITE", SqlDbType.VarChar, 6).Value = leLot.PERIODICITE;
                cmds.Parameters.Add("@EXIG", SqlDbType.Int).Value = leLot.EXIG;
                cmds.Parameters.Add("@DFAC", SqlDbType.DateTime).Value = leLot.DFAC;
                cmds.Parameters.Add("@ETATFAC1", SqlDbType.VarChar, 1).Value = leLot.ETATFAC1;
                cmds.Parameters.Add("@ETATFAC2", SqlDbType.VarChar, 1).Value = leLot.ETATFAC2;
                cmds.Parameters.Add("@ETATFAC3", SqlDbType.VarChar, 1).Value = leLot.ETATFAC3;
                cmds.Parameters.Add("@ETATFAC4", SqlDbType.VarChar, 1).Value = leLot.ETATFAC4;
                cmds.Parameters.Add("@ETATFAC5", SqlDbType.VarChar, 1).Value = leLot.ETATFAC5;
                cmds.Parameters.Add("@ETATFAC6", SqlDbType.VarChar, 1).Value = leLot.ETATFAC6;
                cmds.Parameters.Add("@ETATFAC7", SqlDbType.VarChar, 1).Value = leLot.ETATFAC7;
                cmds.Parameters.Add("@ETATFAC8", SqlDbType.VarChar, 1).Value = leLot.ETATFAC8;
                cmds.Parameters.Add("@ETATFAC9", SqlDbType.VarChar, 1).Value = leLot.ETATFAC9;
                cmds.Parameters.Add("@ETATFAC10", SqlDbType.VarChar, 1).Value = leLot.ETATFAC10;
                cmds.Parameters.Add("@TOURNEE", SqlDbType.VarChar, 15).Value = leLot.TOURNEE;
                cmds.Parameters.Add("@RELEVEUR", SqlDbType.VarChar, 2).Value = leLot.RELEVEUR;
                cmds.Parameters.Add("@BASE", SqlDbType.VarChar, 1).Value = leLot.BASE;
                cmds.Parameters.Add("@USERCREATION", SqlDbType.VarChar, 6).Value = leLot.USERCREATION;
                cmds.Parameters.Add("@USERMODIFICATION", SqlDbType.VarChar, 6).Value = leLot.USERMODIFICATION;
                cmds.Parameters.Add("@FK_IDPRODUIT", SqlDbType.Int).Value = leLot.FK_IDPRODUIT;
                cmds.Parameters.Add("@FK_IDCATEGORIECLIENT", SqlDbType.Int).Value = leLot.FK_IDCATEGORIECLIENT;
                cmds.Parameters.Add("@FK_IDRELEVEUR", SqlDbType.Int).Value = leLot.FK_IDRELEVEUR;
                cmds.Parameters.Add("@UserCalcul", SqlDbType.VarChar, 5).Value = leLot.UserCalcul;
                cmds.Parameters.Add("@FK_IDTOURNEE", SqlDbType.Int).Value = leLot.FK_IDTOURNEE;
                cmds.Parameters.Add("@FK_IDCENTRE", SqlDbType.Int).Value = leLot.FK_IDCENTRE;

                DBBase.SetDBNullParametre(cmds.Parameters);

                try
                {
                    if (cmds.Connection.State == ConnectionState.Closed)
                        cmds.Connection.Open();
                    cmds.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw new Exception(cmds.CommandText + ":" + ex.Message);
                }
            }
        }

        public string NumeroDemande(int IdCentre)
        {
            cn = new SqlConnection(ConnectionString);

            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandTimeout = 3000;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SPX_ACC_NUMERODEMANDE";
            cmd.Parameters.Add("@FK_IDCENTRE", SqlDbType.Int).Value = IdCentre;

            string NumeroDemande = string.Empty;

            DBBase.SetDBNullParametre(cmd.Parameters);
            try
            {
                if (cn.State == ConnectionState.Closed)
                    cn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    NumeroDemande = (Convert.IsDBNull(reader["NUMERODEMANDE"])) ? string.Empty : (string)reader["NUMERODEMANDE"];
                }
                return NumeroDemande;
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
        public string ReferenceClient(int IdCentre, bool EstMt)
        {
            cn = new SqlConnection(ConnectionString);

            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandTimeout = 3000;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SPX_ACC_CREEREFCLIENT";
            cmd.Parameters.Add("@FK_IDCENTRE", SqlDbType.Int).Value = IdCentre;
            cmd.Parameters.Add("@EstMt", SqlDbType.Bit).Value = EstMt;

            string NumeroDemande = string.Empty;

            DBBase.SetDBNullParametre(cmd.Parameters);
            try
            {
                if (cn.State == ConnectionState.Closed)
                    cn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    NumeroDemande = (Convert.IsDBNull(reader["CLIENT"])) ? string.Empty : (string)reader["CLIENT"];
                }
                return NumeroDemande;
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
        public CsDemandeBase Select_Demande(string NumDem, int Iddemande, SqlConnection laConnection)
        {
            cmd = new SqlCommand();
            cmd.Connection = laConnection;
            cmd.CommandTimeout = 3000;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SPX_ACC_SELECTDEMANDE";
            if (cmd.Parameters != null && cmd.Parameters.Count != 0) cmd.Parameters.Clear();

            cmd.Parameters.Add("@NUMDEM", SqlDbType.VarChar, 20).Value = NumDem;
            cmd.Parameters.Add("@IDDEMANDE", SqlDbType.Int).Value = Iddemande;

            DBBase.SetDBNullParametre(cmd.Parameters);
            try
            {
                if (laConnection.State == ConnectionState.Closed)
                    laConnection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);
                return Entities.GetEntityFromQuery<CsDemandeBase>(dt);
            }
            catch (Exception ex)
            {
                throw new Exception(cmd.CommandText + ":" + ex.Message);
            }
            finally
            {
                cmd.Dispose();
            }
        }
        public CsAg Select_DAg(string NumDem, int Iddemande, SqlConnection laConnection)
        {
            
            cmd = new SqlCommand();
            cmd.Connection = laConnection;
            cmd.CommandTimeout = 3000;
            cmd.CommandType = CommandType.StoredProcedure;
            if (cmd.Parameters != null && cmd.Parameters.Count != 0) cmd.Parameters.Clear();

            cmd.CommandText = "SPX_ACC_SELECTDAG";
            cmd.Parameters.Add("@NUMDEM", SqlDbType.VarChar, 20).Value = NumDem;
            cmd.Parameters.Add("@IDDEMANDE", SqlDbType.Int).Value = Iddemande;

            DBBase.SetDBNullParametre(cmd.Parameters);
            try
            {
                if (laConnection.State == ConnectionState.Closed)
                    laConnection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);
                return Entities.GetEntityFromQuery<CsAg>(dt);
            }
            catch (Exception ex)
            {
                throw new Exception(cmd.CommandText + ":" + ex.Message);
            }
            finally
            {
                cmd.Dispose();
            }
        }
        public CsAbon Select_DAbon(string NumDem, int Iddemande, SqlConnection laConnection)
        {
            cmd = new SqlCommand();
            cmd.Connection = laConnection;
            cmd.CommandTimeout = 3000;
            cmd.CommandType = CommandType.StoredProcedure;
            if (cmd.Parameters != null && cmd.Parameters.Count != 0) cmd.Parameters.Clear();

            cmd.CommandText = "SPX_ACC_SELECTDABON";
            cmd.Parameters.Add("@NUMDEM", SqlDbType.VarChar, 20).Value = NumDem;
            cmd.Parameters.Add("@IDDEMANDE", SqlDbType.Int).Value = Iddemande;

            DBBase.SetDBNullParametre(cmd.Parameters);
            try
            {
                if (laConnection.State == ConnectionState.Closed)
                    laConnection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);
                return Entities.GetEntityFromQuery<CsAbon>(dt);
            }
            catch (Exception ex)
            {
                throw new Exception(cmd.CommandText + ":" + ex.Message);
            }
            finally
            {
                cmd.Dispose();
            }
        }
        public CsBrt Select_Dbrt(string NumDem, int Iddemande, SqlConnection laConnection)
        {
        
            cmd = new SqlCommand();
            cmd.Connection = laConnection;
            cmd.CommandTimeout = 3000;
            cmd.CommandType = CommandType.StoredProcedure;
            if (cmd.Parameters != null && cmd.Parameters.Count != 0) cmd.Parameters.Clear();

            cmd.CommandText = "SPX_ACC_SELECTDBRT";
            cmd.Parameters.Add("@NUMDEM", SqlDbType.VarChar, 20).Value = NumDem;
            cmd.Parameters.Add("@IDDEMANDE", SqlDbType.Int).Value = Iddemande;

            DBBase.SetDBNullParametre(cmd.Parameters);
            try
            {
                if (laConnection.State == ConnectionState.Closed)
                    laConnection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);
                return Entities.GetEntityFromQuery<CsBrt>(dt);
            }
            catch (Exception ex)
            {
                throw new Exception(cmd.CommandText + ":" + ex.Message);
            }
            finally
            {
                cmd.Dispose();
            }
        }
        public CsClient Select_Dclient(string NumDem, int Iddemande, SqlConnection laConnection)
        {
            
            cmd = new SqlCommand();
            cmd.Connection = laConnection;
            cmd.CommandTimeout = 3000;
            cmd.CommandType = CommandType.StoredProcedure;
            if (cmd.Parameters != null && cmd.Parameters.Count != 0) cmd.Parameters.Clear();

            cmd.CommandText = "SPX_ACC_SELECTDCLIENT";
            cmd.Parameters.Add("@NUMDEM", SqlDbType.VarChar, 20).Value = NumDem;
            cmd.Parameters.Add("@IDDEMANDE", SqlDbType.Int).Value = Iddemande;

            DBBase.SetDBNullParametre(cmd.Parameters);
            try
            {
                if (laConnection.State == ConnectionState.Closed)
                    laConnection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);
                return Entities.GetEntityFromQuery<CsClient>(dt);
            }
            catch (Exception ex)
            {
                throw new Exception(cmd.CommandText + ":" + ex.Message);
            }
            finally
            {
           
                cmd.Dispose();
            }
        }
        public List<CsCanalisation> Select_DCannalisationInCompteur(string NumDem, int Iddemande, SqlConnection laConnection)
        {
            cmd = new SqlCommand();
            cmd.Connection = laConnection;
            cmd.CommandTimeout = 3000;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SPX_ACC_SELECTDCANALISATION_COMPTEUR";
            if (cmd.Parameters != null && cmd.Parameters.Count != 0) cmd.Parameters.Clear();

            cmd.Parameters.Add("@NUMDEM", SqlDbType.VarChar, 20).Value = NumDem;
            cmd.Parameters.Add("@IDDEMANDE", SqlDbType.Int).Value = Iddemande;

            DBBase.SetDBNullParametre(cmd.Parameters);
            try
            {
                if (laConnection.State == ConnectionState.Closed)
                    laConnection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);
                return Entities.GetEntityListFromQuery<CsCanalisation>(dt);
            }
            catch (Exception ex)
            {
                throw new Exception(cmd.CommandText + ":" + ex.Message);
            }
            finally
            {
                cmd.Dispose();
            }
        }

        public List<CsCanalisation> Select_DCannalisationInDCompteur(string NumDem, int Iddemande, SqlConnection laConnection)
        {
        
            cmd = new SqlCommand();
            cmd.Connection = laConnection;
            cmd.CommandTimeout = 1800;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SPX_ACC_SELECTDCANALISATION_DCOMPTEUR";
            if (cmd.Parameters != null && cmd.Parameters.Count != 0) cmd.Parameters.Clear();

            cmd.Parameters.Add("@NUMDEM", SqlDbType.VarChar, 20).Value = NumDem;
            cmd.Parameters.Add("@IDDEMANDE", SqlDbType.Int).Value = Iddemande;

            DBBase.SetDBNullParametre(cmd.Parameters);
            try
            {
                if (laConnection.State == ConnectionState.Closed)
                    laConnection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);
                return Entities.GetEntityListFromQuery<CsCanalisation>(dt);
            }
            catch (Exception ex)
            {
                throw new Exception(cmd.CommandText + ":" + ex.Message);
            }
            finally
            {
                cmd.Dispose();
            }
        }

        public List<CsEvenement> Select_Devenement(CsDemandeBase demande)
        {
            cn = new SqlConnection(ConnectionString);

            cmd = new SqlCommand();
            cmd.Connection = cn;

            cmd.CommandTimeout = 3000;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SPX_ACC_SELECT_DEVENEMENT";
            if (cmd.Parameters != null && cmd.Parameters.Count != 0) cmd.Parameters.Clear();

            cmd.Parameters.Add("@FK_IDDEMANDE", SqlDbType.Int).Value = demande.PK_ID;
            cmd.Parameters.Add("@FK_IDCENTRE", SqlDbType.Int).Value = demande.FK_IDCENTRE;
            cmd.Parameters.Add("@CLIENT", SqlDbType.VarChar, 20).Value = demande.CLIENT;
            cmd.Parameters.Add("@ORDRE", SqlDbType.VarChar, 2).Value = demande.ORDRE;

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
                throw new Exception(cmd.CommandText + ":" + ex.Message);
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                    cn.Close(); // Fermeture de la connection 
                cmd.Dispose();
            }
        }

        public List<CsCanalisation> Select_DCanalisationInMagazin(string NumDem, int Iddemande, SqlConnection laConnection)
        {
            cmd = new SqlCommand();
            cmd.Connection = laConnection;
            cmd.CommandTimeout = 3000;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SPX_ACC_SELECTDCANALISATION_MAGASIN";
            if (cmd.Parameters != null && cmd.Parameters.Count != 0) cmd.Parameters.Clear();

            cmd.Parameters.Add("@NUMDEM", SqlDbType.VarChar, 20).Value = NumDem;
            cmd.Parameters.Add("@IDDEMANDE", SqlDbType.Int).Value = Iddemande;

            DBBase.SetDBNullParametre(cmd.Parameters);
            try
            {
                if (laConnection.State == ConnectionState.Closed)
                    laConnection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);
                return Entities.GetEntityListFromQuery<CsCanalisation>(dt);
            }
            catch (Exception ex)
            {
                throw new Exception(cmd.CommandText + ":" + ex.Message);
            }
            finally
            {
                cmd.Dispose();
            }
        }



        public CsAbon VerifierMatriculeAgent(string matricule)
        {
            cn = new SqlConnection(ConnectionString);

            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandTimeout = 300;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SPX_ACC_VERIFIER_MATRICULE";
            if (cmd.Parameters != null && cmd.Parameters.Count != 0) cmd.Parameters.Clear();

            cmd.Parameters.Add("@Matricule", SqlDbType.VarChar, 5).Value = matricule;

            DBBase.SetDBNullParametre(cmd.Parameters);
            try
            {
                if (cn.State == ConnectionState.Closed)
                    cn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);
                return Entities.GetEntityFromQuery<CsAbon>(dt);
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



        public List<CsDemandeDetailCout> Select_FactureDemande(string NumDem, int Iddemande, SqlConnection laConnection)
        {
            cmd = new SqlCommand();
            cmd.Connection = laConnection;
            cmd.CommandTimeout = 3000;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SPX_ACC_SELECTRUBRIQUEDEMANDE";
            if (cmd.Parameters != null && cmd.Parameters.Count != 0) cmd.Parameters.Clear();

            cmd.Parameters.Add("@NUMDEM", SqlDbType.VarChar, 20).Value = NumDem;
            cmd.Parameters.Add("@IDDEMANDE", SqlDbType.Int).Value = Iddemande;

            DBBase.SetDBNullParametre(cmd.Parameters);
            try
            {
                if (laConnection.State == ConnectionState.Closed)
                    laConnection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);
                return Entities.GetEntityListFromQuery<CsDemandeDetailCout>(dt);
            }
            catch (Exception ex)
            {
                throw new Exception(cmd.CommandText + ":" + ex.Message);
            }
            finally
            {
                cmd.Dispose();
            }
        }
        public List<ObjELEMENTDEVIS> Select_ElementDevis(string NumDem, int Iddemande, SqlConnection laConnection)
        {
            
            cmd = new SqlCommand();
            cmd.Connection = laConnection;
            cmd.CommandTimeout = 3000;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SPX_ACC_SELECTELEMENTDEVIS";
            if (cmd.Parameters != null && cmd.Parameters.Count != 0) cmd.Parameters.Clear();

            cmd.Parameters.Add("@NUMDEM", SqlDbType.VarChar, 20).Value = NumDem;
            cmd.Parameters.Add("@IDDEMANDE", SqlDbType.Int).Value = Iddemande;

            DBBase.SetDBNullParametre(cmd.Parameters);
            try
            {
                if (laConnection.State == ConnectionState.Closed)
                    laConnection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);
                return Entities.GetEntityListFromQuery<ObjELEMENTDEVIS>(dt);
            }
            catch (Exception ex)
            {
                throw new Exception(cmd.CommandText + ":" + ex.Message);
            }
            finally
            {
                cmd.Dispose();
            }
        }

        public List<CsElementAchatTimbre> Select_ElementAchatTimbre(string NumDem, int Iddemande, SqlConnection laConnection)
        {

            cmd = new SqlCommand();
            cmd.Connection = laConnection;
            cmd.CommandTimeout = 3000;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SPX_ACC_SELECTELEMENTACHATTIMBRE";
            if (cmd.Parameters != null && cmd.Parameters.Count != 0) cmd.Parameters.Clear();

            cmd.Parameters.Add("@NUMDEM", SqlDbType.VarChar, 20).Value = NumDem;
            cmd.Parameters.Add("@IDDEMANDE", SqlDbType.Int).Value = Iddemande;

            DBBase.SetDBNullParametre(cmd.Parameters);
            try
            {
                if (laConnection.State == ConnectionState.Closed)
                    laConnection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);
                return Entities.GetEntityListFromQuery<CsElementAchatTimbre >(dt);
            }
            catch (Exception ex)
            {
                throw new Exception(cmd.CommandText + ":" + ex.Message);
            }
            finally
            {
                cmd.Dispose();
            }
        }

        public CsSocietePrive Select_DSocietePrive(int Iddemande, SqlConnection laConnection)
        {
            cmd = new SqlCommand();
            cmd.Connection = laConnection;
            cmd.CommandTimeout = 3000;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SPX_ACC_SELECTDSOCIETEPRIVE";
            if (cmd.Parameters != null && cmd.Parameters.Count != 0) cmd.Parameters.Clear();

            cmd.Parameters.Add("@IDDEMANDE", SqlDbType.Int).Value = Iddemande;

            DBBase.SetDBNullParametre(cmd.Parameters);
            try
            {
                if (laConnection.State == ConnectionState.Closed)
                    laConnection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);
                return Entities.GetEntityFromQuery<CsSocietePrive>(dt);
            }
            catch (Exception ex)
            {
                throw new Exception(cmd.CommandText + ":" + ex.Message);
            }
            finally
            {
                cmd.Dispose();
            }
        }
        public CsPersonePhysique Select_DPersonnePhysique(int Iddemande, SqlConnection laConnection)
        {
            cmd = new SqlCommand();
            cmd.Connection = laConnection;
            cmd.CommandTimeout = 3000;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SPX_ACC_SELECTDPERSONNEPHYSIQUE";
            if (cmd.Parameters != null && cmd.Parameters.Count != 0) cmd.Parameters.Clear();

            cmd.Parameters.Add("@IDDEMANDE", SqlDbType.Int).Value = Iddemande;

            DBBase.SetDBNullParametre(cmd.Parameters);
            try
            {
                if (laConnection.State == ConnectionState.Closed)
                    laConnection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);
                return Entities.GetEntityFromQuery<CsPersonePhysique>(dt);
            }
            catch (Exception ex)
            {
                throw new Exception(cmd.CommandText + ":" + ex.Message);
            }
            finally
            {
                 
                cmd.Dispose();
            }
        }
        public CsAdministration_Institut Select_DAdministration_Institut(int Iddemande, SqlConnection laConnection)
        {
   
            cmd = new SqlCommand();
            cmd.Connection = laConnection;
            cmd.CommandTimeout = 3000;
            cmd.CommandType = CommandType.StoredProcedure;
            if (cmd.Parameters != null && cmd.Parameters.Count != 0) cmd.Parameters.Clear();

            cmd.CommandText = "SPX_ACC_SELECTDADMINISTRATION_INSTITUT";
            cmd.Parameters.Add("@IDDEMANDE", SqlDbType.Int).Value = Iddemande;

            DBBase.SetDBNullParametre(cmd.Parameters);
            try
            {
                if (laConnection.State == ConnectionState.Closed)
                    laConnection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);
                return Entities.GetEntityFromQuery<CsAdministration_Institut>(dt);
            }
            catch (Exception ex)
            {
                throw new Exception(cmd.CommandText + ":" + ex.Message);
            }
            finally
            {
                cmd.Dispose();
            }
        }
        public List<ObjAPPAREILSDEVIS> Select_Appareil(string NumDem, int Iddemande, SqlConnection laConnection)
        {
         
            cmd = new SqlCommand();
            cmd.Connection = laConnection;
            cmd.CommandTimeout = 3000;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SPX_ACC_SELECTAPPAREIL";
            if (cmd.Parameters != null && cmd.Parameters.Count != 0) cmd.Parameters.Clear();
            cmd.Parameters.Add("@NUMDEM", SqlDbType.VarChar, 20).Value = NumDem;
            cmd.Parameters.Add("@IDDEMANDE", SqlDbType.Int).Value = Iddemande;
            DBBase.SetDBNullParametre(cmd.Parameters);
            try
            {
                if (laConnection.State == ConnectionState.Closed)
                    laConnection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);
                return Entities.GetEntityListFromQuery<ObjAPPAREILSDEVIS>(dt);
            }
            catch (Exception ex)
            {
                throw new Exception(cmd.CommandText + ":" + ex.Message);
            }
            finally
            {
                cmd.Dispose();
            }
        }

        public List<CsAnnotation> Select_Annotation(int Iddemande, SqlConnection laConnection)
        {
            cmd = new SqlCommand();
            cmd.Connection = laConnection;
            cmd.CommandTimeout = 3000;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SPX_ACC_SELECTANNOTATION";
            if (cmd.Parameters != null && cmd.Parameters.Count != 0) cmd.Parameters.Clear();
            cmd.Parameters.Add("@IDDEMANDE", SqlDbType.Int).Value = Iddemande;
            DBBase.SetDBNullParametre(cmd.Parameters);
            try
            {
                if (laConnection.State == ConnectionState.Closed)
                    laConnection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);
                return Entities.GetEntityListFromQuery<CsAnnotation>(dt);
            }
            catch (Exception ex)
            {
                throw new Exception(cmd.CommandText + ":" + ex.Message);
            }
            finally
            {
                cmd.Dispose();
            }
        }
      
        /*
        public List<ObjDOCUMENTSCANNE> Select_DocumentTittre(int Iddemande)
        {
            cn = new SqlConnection(ConnectionString);

            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandTimeout = 3000;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SPX_ACC_SELECTDOCUMENTTITRE";
            if (cmd.Parameters != null && cmd.Parameters.Count != 0) cmd.Parameters.Clear();
            cmd.Parameters.Add("@IDDEMANDE", SqlDbType.Int).Value = Iddemande;
            DBBase.SetDBNullParametre(cmd.Parameters);
            try
            {
                if (cn.State == ConnectionState.Closed)
                    cn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);
                return Entities.GetEntityListFromQuery<ObjDOCUMENTSCANNE>(dt);
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
        public List<ObjDOCUMENTSCANNE> Select_DocumentContenut(int Iddemande, int IdTypeDocument)
        {
            cn = new SqlConnection(ConnectionString);

            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandTimeout = 3000;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SPX_ACC_SELECTDOCUMENTCONTENT";
            if (cmd.Parameters != null && cmd.Parameters.Count != 0) cmd.Parameters.Clear();
            cmd.Parameters.Add("@IDDEMANDE", SqlDbType.Int).Value = Iddemande;
            cmd.Parameters.Add("@IDTYPEDOCUMENT", SqlDbType.Int).Value = IdTypeDocument;
            DBBase.SetDBNullParametre(cmd.Parameters);
            try
            {
                if (cn.State == ConnectionState.Closed)
                    cn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);
                return Entities.GetEntityListFromQuery<ObjDOCUMENTSCANNE>(dt);
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
*/

        public CsAg Select_Ag(int IdCentre, string Centre, string Client,SqlConnection laConnection)
        {
            cmd = new SqlCommand();
            cmd.Connection = laConnection;
            cmd.CommandTimeout = 3000;
            cmd.CommandType = CommandType.StoredProcedure;
            if (cmd.Parameters != null && cmd.Parameters.Count != 0) cmd.Parameters.Clear();

            cmd.CommandText = "SPX_ACC_SELECTAG";
            cmd.Parameters.Add("@FK_IDCENTRE", SqlDbType.Int).Value = IdCentre;
            cmd.Parameters.Add("@CENTRE", SqlDbType.VarChar, 3).Value = Centre;
            cmd.Parameters.Add("@CLIENT", SqlDbType.VarChar, 20).Value = Client;
            DBBase.SetDBNullParametre(cmd.Parameters);
            try
            {
                if (laConnection.State == ConnectionState.Closed)
                    laConnection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);
                return Entities.GetEntityFromQuery<CsAg>(dt);
            }
            catch (Exception ex)
            {
                throw new Exception(cmd.CommandText + ":" + ex.Message);
            }
            finally
            {
                cmd.Dispose();
            }
        }
        public CsAbon Select_Abon(int IdCentre, string Centre, string Client, string Ordre,string Produit,SqlConnection laConnection)
        {
            cmd = new SqlCommand();
            cmd.Connection = laConnection;
            cmd.CommandTimeout = 3000;
            cmd.CommandType = CommandType.StoredProcedure;
            if (cmd.Parameters != null && cmd.Parameters.Count != 0) cmd.Parameters.Clear();
            cmd.CommandText = "SPX_ACC_SELECTABON";
            cmd.Parameters.Add("@FK_IDCENTRE", SqlDbType.Int).Value = IdCentre;
            cmd.Parameters.Add("@CENTRE", SqlDbType.VarChar, 3).Value = Centre;
            cmd.Parameters.Add("@CLIENT", SqlDbType.VarChar, 20).Value = Client;
            cmd.Parameters.Add("@ORDRE", SqlDbType.VarChar, 2).Value = Ordre;
            cmd.Parameters.Add("@PRODUIT", SqlDbType.VarChar, 2).Value = Produit;

            DBBase.SetDBNullParametre(cmd.Parameters);
            try
            {
                if (laConnection.State == ConnectionState.Closed)
                    laConnection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);
                return Entities.GetEntityFromQuery<CsAbon>(dt);
            }
            catch (Exception ex)
            {
                throw new Exception(cmd.CommandText + ":" + ex.Message);
            }
            finally
            {
               
                cmd.Dispose();
            }
        }
        public List<CsAbon> Select_AbonList(int IdCentre, string Centre, string Client, string Ordre,SqlConnection laConnection)
        {
            cmd = new SqlCommand();
            cmd.Connection = laConnection;
            cmd.CommandTimeout = 3000;
            cmd.CommandType = CommandType.StoredProcedure;
            if (cmd.Parameters != null && cmd.Parameters.Count != 0) cmd.Parameters.Clear();
            cmd.CommandText = "SPX_ACC_SELECTABON";
            cmd.Parameters.Add("@FK_IDCENTRE", SqlDbType.Int).Value = IdCentre;
            cmd.Parameters.Add("@CENTRE", SqlDbType.VarChar, 3).Value = Centre;
            cmd.Parameters.Add("@CLIENT", SqlDbType.VarChar, 20).Value = Client;
            cmd.Parameters.Add("@ORDRE", SqlDbType.VarChar, 2).Value = Ordre;
            cmd.Parameters.Add("@PRODUIT", SqlDbType.VarChar, 2).Value = string.Empty;

            DBBase.SetDBNullParametre(cmd.Parameters);
            try
            {
                if (laConnection.State == ConnectionState.Closed)
                    laConnection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);
                return Entities.GetEntityListFromQuery<CsAbon>(dt);
            }
            catch (Exception ex)
            {
                throw new Exception(cmd.CommandText + ":" + ex.Message);
            }
            finally
            {
                cmd.Dispose();
            }
        }
        public List<CsBrt> Select_brtList(int IdCentre, string Centre, string Client, SqlConnection laConnection)
        {
            cmd = new SqlCommand();
            cmd.Connection = laConnection;
            cmd.CommandTimeout = 3000;
            cmd.CommandType = CommandType.StoredProcedure;
            if (cmd.Parameters != null && cmd.Parameters.Count != 0) cmd.Parameters.Clear();

            cmd.CommandText = "SPX_ACC_SELECTBRT";
            cmd.Parameters.Add("@FK_IDCENTRE", SqlDbType.Int).Value = IdCentre;
            cmd.Parameters.Add("@CENTRE", SqlDbType.VarChar, 3).Value = Centre;
            cmd.Parameters.Add("@CLIENT", SqlDbType.VarChar, 20).Value = Client;

            DBBase.SetDBNullParametre(cmd.Parameters);
            try
            {
                if (laConnection.State == ConnectionState.Closed)
                    laConnection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);
                return Entities.GetEntityListFromQuery<CsBrt>(dt);
            }
            catch (Exception ex)
            {
                throw new Exception(cmd.CommandText + ":" + ex.Message);
            }
            finally
            {
                 
                cmd.Dispose();
            }
        }
        public CsBrt Select_brt(int IdCentre, string Centre, string Client, SqlConnection laConnection)
        {
            cmd = new SqlCommand();
            cmd.Connection = laConnection;
            cmd.CommandTimeout = 3000;
            cmd.CommandType = CommandType.StoredProcedure;
            if (cmd.Parameters != null && cmd.Parameters.Count != 0) cmd.Parameters.Clear();

            cmd.CommandText = "SPX_ACC_SELECTBRT";
            cmd.Parameters.Add("@FK_IDCENTRE", SqlDbType.Int).Value = IdCentre;
            cmd.Parameters.Add("@CENTRE", SqlDbType.VarChar, 3).Value = Centre;
            cmd.Parameters.Add("@CLIENT", SqlDbType.VarChar, 20).Value = Client;

            DBBase.SetDBNullParametre(cmd.Parameters);
            try
            {
                if (laConnection.State == ConnectionState.Closed)
                    laConnection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);
                return Entities.GetEntityFromQuery<CsBrt>(dt);
            }
            catch (Exception ex)
            {
                throw new Exception(cmd.CommandText + ":" + ex.Message);
            }
            finally
            {
                cmd.Dispose();
            }
        }
        public CsClient Select_client(int IdCentre, string Centre, string Client, string Ordre,SqlConnection laConnection)
        {
            cmd = new SqlCommand();
            cmd.Connection = laConnection;
            cmd.CommandTimeout = 3000;
            cmd.CommandType = CommandType.StoredProcedure;
            if (cmd.Parameters != null && cmd.Parameters.Count != 0) cmd.Parameters.Clear();
            cmd.CommandText = "SPX_ACC_SELECTCLIENT";
            cmd.Parameters.Add("@FK_IDCENTRE", SqlDbType.Int).Value = IdCentre;
            cmd.Parameters.Add("@CENTRE", SqlDbType.VarChar, 3).Value = Centre;
            cmd.Parameters.Add("@CLIENT", SqlDbType.VarChar, 20).Value = Client;
            cmd.Parameters.Add("@ORDRE", SqlDbType.VarChar, 2).Value = Ordre;

            DBBase.SetDBNullParametre(cmd.Parameters);
            try
            {
                if (laConnection.State == ConnectionState.Closed)
                    laConnection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);
                return Entities.GetEntityFromQuery<CsClient>(dt);
            }
            catch (Exception ex)
            {
                throw new Exception(cmd.CommandText + ":" + ex.Message);
            }
            finally
            {
                cmd.Dispose();
            }
        }
        public List<CsClient> Select_client(int? IdCentre, string Centre, string Client, string Ordre)
        {
            cn = new SqlConnection(ConnectionString);

            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandTimeout = 3000;
            cmd.CommandType = CommandType.StoredProcedure;
            if (cmd.Parameters != null && cmd.Parameters.Count != 0) cmd.Parameters.Clear();
            cmd.CommandText = "SPX_ACC_SELECTCLIENT";
            cmd.Parameters.Add("@FK_IDCENTRE", SqlDbType.Int).Value = IdCentre;
            cmd.Parameters.Add("@CENTRE", SqlDbType.VarChar, 3).Value = Centre;
            cmd.Parameters.Add("@CLIENT", SqlDbType.VarChar, 20).Value = Client;
            cmd.Parameters.Add("@ORDRE", SqlDbType.VarChar, 2).Value = Ordre;

            DBBase.SetDBNullParametre(cmd.Parameters);
            try
            {
                if (cn.State == ConnectionState.Closed)
                    cn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);
                return Entities.GetEntityListFromQuery<CsClient>(dt);
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
        public List<CsCanalisation> Select_CannalisationCompteur(int IdCentre, string Centre, string Client, string Produit, SqlConnection laConnection)
        {
            cmd = new SqlCommand();
            cmd.Connection = laConnection;
            cmd.CommandTimeout = 3000;
            cmd.CommandType = CommandType.StoredProcedure;
            if (cmd.Parameters != null && cmd.Parameters.Count != 0) cmd.Parameters.Clear();
            cmd.CommandText = "SPX_ACC_SELECTCANNALISATION";
            cmd.Parameters.Add("@FK_IDCENTRE", SqlDbType.Int).Value = IdCentre;
            cmd.Parameters.Add("@CENTRE", SqlDbType.VarChar, 3).Value = Centre;
            cmd.Parameters.Add("@CLIENT", SqlDbType.VarChar, 20).Value = Client;
            cmd.Parameters.Add("@PRODUIT", SqlDbType.VarChar, 2).Value = Produit;

            DBBase.SetDBNullParametre(cmd.Parameters);
            try
            {
                if (laConnection.State == ConnectionState.Closed)
                    laConnection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);
                return Entities.GetEntityListFromQuery<CsCanalisation>(dt);
            }
            catch (Exception ex)
            {
                throw new Exception(cmd.CommandText + ":" + ex.Message);
            }
            finally
            {
                cmd.Dispose();
            }
        }

        public void UpdateCanalisation(int idAbon, int? idReglage, string reglage, SqlCommand cmds)
        {
            cmds.CommandTimeout = 3000;
            cmds.CommandType = CommandType.StoredProcedure;
            cmds.CommandText = "SPX_ACC_UPDATE_CANALISATION_DISJ";
            if (cmds.Parameters != null && cmds.Parameters.Count != 0) cmds.Parameters.Clear();
            cmds.Parameters.Add("@FK_IDABON", SqlDbType.Int).Value = idAbon;
            cmds.Parameters.Add("@IDREGLAGE", SqlDbType.Int).Value = idReglage;
            cmds.Parameters.Add("@CODEREGLAGE", SqlDbType.VarChar, 3).Value = reglage;

            DBBase.SetDBNullParametre(cmds.Parameters);
            try
            {
                if (cmds.Connection.State == ConnectionState.Closed)
                    cmds.Connection.Open();
                cmds.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception(cmds.CommandText + ":" + ex.Message);
            }
        }

        public CsSocietePrive Select_SocietePrive(int Fk_idclient, SqlConnection laConnection)
        {
           

            cmd = new SqlCommand();
            cmd.Connection = laConnection;
            cmd.CommandTimeout = 3000;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SPX_ACC_SELECTSOCIETEPRIVE";
            if (cmd.Parameters != null && cmd.Parameters.Count != 0) cmd.Parameters.Clear();
            cmd.Parameters.Add("@FK_IDCLIENT", SqlDbType.Int).Value = Fk_idclient;

            DBBase.SetDBNullParametre(cmd.Parameters);
            try
            {
                if (laConnection.State == ConnectionState.Closed)
                    laConnection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);
                return Entities.GetEntityFromQuery<CsSocietePrive>(dt);
            }
            catch (Exception ex)
            {
                throw new Exception(cmd.CommandText + ":" + ex.Message);
            }
            finally
            {
                cmd.Dispose();
            }
        }
        public CsPersonePhysique Select_PersonnePhysique(int Fk_idclient, SqlConnection laConnection)
        {
            cmd = new SqlCommand();
            cmd.Connection = laConnection;
            cmd.CommandTimeout = 3000;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SPX_ACC_SELECTPERSONNEPHYSIQUE";
            if (cmd.Parameters != null && cmd.Parameters.Count != 0) cmd.Parameters.Clear();
            cmd.Parameters.Add("@FK_IDCLIENT", SqlDbType.Int).Value = Fk_idclient;

            DBBase.SetDBNullParametre(cmd.Parameters);
            try
            {
                if (laConnection.State == ConnectionState.Closed)
                    laConnection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);
                return Entities.GetEntityFromQuery<CsPersonePhysique>(dt);
            }
            catch (Exception ex)
            {
                throw new Exception(cmd.CommandText + ":" + ex.Message);
            }
            finally
            {
                cmd.Dispose();
            }
        }
        public CsAdministration_Institut Select_Administration_Institut(int Fk_idclient, SqlConnection laConnection)
        {
            cmd = new SqlCommand();
            cmd.Connection = laConnection;
            cmd.CommandTimeout = 3000;
            cmd.CommandType = CommandType.StoredProcedure;
            if (cmd.Parameters != null && cmd.Parameters.Count != 0) cmd.Parameters.Clear();

            cmd.CommandText = "SPX_ACC_SELECTADMINISTRATION_INSTITUT";
            cmd.Parameters.Add("@FK_IDCLIENT", SqlDbType.Int).Value = Fk_idclient;

            DBBase.SetDBNullParametre(cmd.Parameters);
            try
            {
                if (laConnection.State == ConnectionState.Closed)
                    laConnection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);
                return Entities.GetEntityFromQuery<CsAdministration_Institut>(dt);
            }
            catch (Exception ex)
            {
                throw new Exception(cmd.CommandText + ":" + ex.Message);
            }
            finally
            {
                cmd.Dispose();
            }
        }

        public CsInfoProprietaire Select_InformationPropriete(string NumDem, int Iddemande, SqlConnection laConnection)
        {
            cmd = new SqlCommand();
            cmd.Connection = laConnection;
            cmd.CommandTimeout = 3000;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SPX_ACC_SELECTDINFOPROPRIETAIRE";
            if (cmd.Parameters != null && cmd.Parameters.Count != 0) cmd.Parameters.Clear();

            cmd.Parameters.Add("@NUMDEM", SqlDbType.VarChar, 20).Value = NumDem;
            cmd.Parameters.Add("@IDDEMANDE", SqlDbType.Int).Value = Iddemande;

            DBBase.SetDBNullParametre(cmd.Parameters);
            try
            {
                if (laConnection.State == ConnectionState.Closed)
                    laConnection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);
                return Entities.GetEntityFromQuery<CsInfoProprietaire>(dt);
            }
            catch (Exception ex)
            {
                throw new Exception(cmd.CommandText + ":" + ex.Message);
            }
            finally
            {
                cmd.Dispose();
            }
        }
        public CsInfoDemandeWorkflow RecupererInfoDemande(string NumDemande)
        {
            try
            {
                CsInfoDemandeWorkflow InfDemande = new CsInfoDemandeWorkflow();

                cn = new SqlConnection(ConnectionString);

                cmd = new SqlCommand();
                cmd.Connection = cn;
                cmd.CommandTimeout = 3000;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SPX_ACC_RETOURNEETAPEDEMANDE";
                if (cmd.Parameters != null && cmd.Parameters.Count != 0) cmd.Parameters.Clear();

                cmd.Parameters.Add("@NUMERODEMANDE", SqlDbType.VarChar, 20).Value = NumDemande;
                DBBase.SetDBNullParametre(cmd.Parameters);
                try
                {
                    if (cn.State == ConnectionState.Closed)
                        cn.Open();

                    SqlDataReader reader = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(reader);

                    InfDemande = Entities.GetEntityFromQuery<CsInfoDemandeWorkflow>(dt);
                    if (InfDemande != null && !string.IsNullOrEmpty(InfDemande.CODE_DEMANDE_TABLE_TRAVAIL))
                    {
                        InfDemande.LiteRejet = new List<CsRenvoiRejet>();
                        InfDemande.LiteRejet = RetourneRejetWkfEtape(InfDemande.FK_IDETAPEACTUELLE);
                    }
                    return InfDemande;
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
                throw;
            }
        }
        public List<CsRenvoiRejet> RetourneRejetWkfEtape(int Fk_idetape)
        {
            cn = new SqlConnection(ConnectionString);

            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandTimeout = 3000;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SPX_ACC_RETOURNEETAPEREJET";
            cmd.Parameters.Add("@ETAPEACTUELLE", SqlDbType.Int).Value = Fk_idetape;

            DBBase.SetDBNullParametre(cmd.Parameters);
            try
            {
                if (cn.State == ConnectionState.Closed)
                    cn.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);

                return Entities.GetEntityListFromQuery<CsRenvoiRejet>(dt); ;

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
        public string ValiderAffectation(List<CsAffectationDemandeUser> lstAffectation)
        {
            SqlConnection laConnection = DBBase.InitConnection(ConnectionString);
            SqlCommand laCommande = null;
            try
            {
                laCommande = new SqlCommand();
                laCommande = DBBase.InitTransaction(ConnectionString);
                foreach (CsAffectationDemandeUser item in lstAffectation)
                {
                    CsDemandeBase LaDemande = Select_Demande(item.CODEDEMANDE, item.FK_IDDEMANDE, laConnection);
                    item.CODEDEMANDE = LaDemande.NUMDEM;
                    InsertOrUpdateAffectation(item, laCommande);
                    TransmettreDemande(LaDemande.NUMDEM, item.FK_IDETAPEFROM, item.MATRICULEUSERCREATION, laCommande);
                }
                laCommande.Transaction.Commit();
                return "";
            }
            catch (Exception ex)
            {
                laCommande.Transaction.Rollback();
                return "non";
            }
            finally
            {
                if (laCommande.Connection.State == ConnectionState.Open)
                    laCommande.Connection.Close(); // Fermeture de la connection 
                laCommande.Dispose();

                if (laConnection.State == ConnectionState.Open)
                    laConnection.Close(); // Fermeture de la connection 
                laConnection.Dispose();
            }
        }

        public string EtablissementDevis(CsDemande laDemande, bool AvecTransmission)
        {
            SqlCommand laCommande = DBBase.InitTransaction(ConnectionString);
            try
            {
                #region Demande
                if (string.IsNullOrEmpty(laDemande.LaDemande.NUMDEM))
                {
                    laDemande.LaDemande.NUMDEM = NumeroDemande(laDemande.LaDemande.FK_IDCENTRE);
                    laDemande.LaDemande.CLIENT = laDemande.LaDemande.NUMDEM.Substring(0, Enumere.TailleClient);
                }
                InsertOrUpdateDemande(laDemande.LaDemande, laCommande);
                #endregion
                #region Dbrt
                if (laDemande.Branchement != null && !string.IsNullOrEmpty(laDemande.Branchement.CENTRE))
                {
                    laDemande.Branchement.NUMDEM = laDemande.LaDemande.NUMDEM;
                    laDemande.Branchement.FK_IDDEMANDE = laDemande.LaDemande.PK_ID;
                    if (string.IsNullOrEmpty(laDemande.Branchement.CLIENT))
                        laDemande.Branchement.CLIENT = laDemande.LaDemande.CLIENT;
                    InsertOrUpdateDBRT(laDemande.Branchement, laCommande);
                }
                #endregion
                #region DAg
                if (laDemande.Ag != null && !string.IsNullOrEmpty(laDemande.Ag.CENTRE))
                {
                    laDemande.Ag.NUMDEM = laDemande.LaDemande.NUMDEM;
                    laDemande.Ag.FK_IDNUMDEM = laDemande.LaDemande.PK_ID;
                    if (string.IsNullOrEmpty(laDemande.Ag.CLIENT))
                        laDemande.Ag.CLIENT = laDemande.LaDemande.CLIENT;
                    InsertOrUpdateDAG(laDemande.Ag, laCommande);
                }
                #endregion
                #region ElementDevis
                if (laDemande.EltDevis != null && laDemande.EltDevis.Count != 0)
                {
                    Delete_ElementDevis(laDemande.LaDemande.NUMDEM, laDemande.LaDemande.PK_ID, laCommande);
                    foreach (ObjELEMENTDEVIS item in laDemande.EltDevis)
                    {
                        item.FK_IDDEMANDE = laDemande.LaDemande.PK_ID;
                        item.NUMDEM = laDemande.LaDemande.NUMDEM;
                        InsertOrUpdateElementDevis(item, laCommande);
                    }
                }
                #endregion
                #region Participation
                if (laDemande.LstFraixParticipation != null && laDemande.LstFraixParticipation.Count != 0)
                {
                    foreach (CsFraixParticipation item in laDemande.LstFraixParticipation)
                    {
                        item.FK_IDDEMANDE = laDemande.LaDemande.PK_ID;
                        InsertOrUpdateParticipation(item, laCommande);
                    }
                }
                #endregion
                #region ObjetScanne
                EcrirePieceJointe(laDemande, laCommande);
                #endregion
                if (AvecTransmission)
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

        public string ValiderEtablissementDevis(CsDemande laDemande, bool AvecTransmission)
        {
            SqlCommand laCommande = DBBase.InitTransaction(ConnectionString);
            try
            {
                if (AvecTransmission)
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

/*
        public string ValiderInformationAbonnement(CsDemande laDemande, bool AvecTransmission)
        {
            SqlCommand laCommande = DBBase.InitTransaction(ConnectionString);
            try
            {
                #region DClient
                if (laDemande.LeClient != null && !string.IsNullOrEmpty(laDemande.LeClient.CENTRE))
                {
                    laDemande.LeClient.NUMDEM = laDemande.LaDemande.NUMDEM;
                    laDemande.LeClient.FK_IDNUMDEM = laDemande.LaDemande.PK_ID;
                    if (string.IsNullOrEmpty(laDemande.LeClient.REFCLIENT))
                        laDemande.LeClient.REFCLIENT = laDemande.LaDemande.CLIENT;
                    InsertOrUpdateDClient(laDemande.LeClient, laCommande);
                }
                #endregion
                #region DABON
                if (laDemande.Abonne != null && !string.IsNullOrEmpty(laDemande.Abonne.CENTRE))
                {
                    laDemande.Abonne.NUMDEM = laDemande.LaDemande.NUMDEM;
                    laDemande.Abonne.FK_IDDEMANDE = laDemande.LaDemande.PK_ID;
                    if (string.IsNullOrEmpty(laDemande.Abonne.CLIENT))
                        laDemande.Abonne.CLIENT = laDemande.LaDemande.CLIENT;
                    InsertOrUpdateDAbon(laDemande.Abonne, laCommande);
                }
                #endregion
                #region DAg
                if (laDemande.Ag != null && !string.IsNullOrEmpty(laDemande.Ag.CENTRE))
                {
                    laDemande.Ag.NUMDEM = laDemande.LaDemande.NUMDEM;
                    laDemande.Ag.FK_IDNUMDEM = laDemande.LaDemande.PK_ID;
                    if (string.IsNullOrEmpty(laDemande.Ag.CLIENT))
                        laDemande.Ag.CLIENT = laDemande.LaDemande.CLIENT;
                    InsertOrUpdateDAG(laDemande.Ag, laCommande);
                }
                #endregion
                #region DPersonePhysique
                if (laDemande.PersonePhysique != null)
                {
                    laDemande.PersonePhysique.FK_IDDEMANDE = laDemande.LaDemande.PK_ID;
                    InsertOrUpdateDpersPhysique(laDemande.PersonePhysique, laCommande);
                }
                #endregion
                #region SocietePrives
                if (laDemande.SocietePrives != null)
                {
                    laDemande.SocietePrives.FK_IDDEMANDE = laDemande.LaDemande.PK_ID;
                    InsertOrUpdateDSociete(laDemande.SocietePrives, laCommande);
                }
                #endregion
                #region ObjetScanne
                if (laDemande.ObjetScanne != null && laDemande.ObjetScanne.Count != 0)
                {
                    foreach (ObjDOCUMENTSCANNE item in laDemande.ObjetScanne.Where(y=>y.CONTENU != null ))
                    {
                        item.FK_IDDEMANDE = laDemande.LaDemande.PK_ID;
                        InsertOrUpdateDocumentScane(item, laCommande);
                    }
                }
                #endregion
                #region ElementDevis
                if (laDemande.EltDevis != null && laDemande.EltDevis.Count != 0)
                {
                    foreach (ObjELEMENTDEVIS item in laDemande.EltDevis)
                    {
                        item.FK_IDDEMANDE = laDemande.LaDemande.PK_ID;
                        item.NUMDEM = laDemande.LaDemande.NUMDEM;
                        InsertOrUpdateElementDevis(item, laCommande);
                    }
                }
                #endregion
                if (AvecTransmission)
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
*/

        public string ValiderInformationAbonnement(CsDemande laDemande, bool AvecTransmission)
        {
            SqlCommand laCommande = DBBase.InitTransaction(ConnectionString);
            try
            {
                #region DClient
                if (laDemande.LeClient != null && !string.IsNullOrEmpty(laDemande.LeClient.CENTRE))
                {
                    laDemande.LeClient.NUMDEM = laDemande.LaDemande.NUMDEM;
                    laDemande.LeClient.FK_IDNUMDEM = laDemande.LaDemande.PK_ID;
                    if (string.IsNullOrEmpty(laDemande.LeClient.REFCLIENT))
                        laDemande.LeClient.REFCLIENT = laDemande.LaDemande.CLIENT;
                    InsertOrUpdateDClient(laDemande.LeClient, laCommande);
                }
                #endregion
                #region DABON
                if (laDemande.Abonne != null && !string.IsNullOrEmpty(laDemande.Abonne.CENTRE))
                {
                    laDemande.Abonne.NUMDEM = laDemande.LaDemande.NUMDEM;
                    laDemande.Abonne.FK_IDDEMANDE = laDemande.LaDemande.PK_ID;
                    if (string.IsNullOrEmpty(laDemande.Abonne.CLIENT))
                        laDemande.Abonne.CLIENT = laDemande.LaDemande.CLIENT;
                    InsertOrUpdateDAbon(laDemande.Abonne, laCommande);
                }
                #endregion
                #region DAg
                if (laDemande.Ag != null && !string.IsNullOrEmpty(laDemande.Ag.CENTRE))
                {
                    laDemande.Ag.NUMDEM = laDemande.LaDemande.NUMDEM;
                    laDemande.Ag.FK_IDNUMDEM = laDemande.LaDemande.PK_ID;
                    if (string.IsNullOrEmpty(laDemande.Ag.CLIENT))
                        laDemande.Ag.CLIENT = laDemande.LaDemande.CLIENT;
                    InsertOrUpdateDAG(laDemande.Ag, laCommande);
                }
                #endregion
                #region DPersonePhysique
                if (laDemande.PersonePhysique != null)
                {
                    laDemande.PersonePhysique.FK_IDDEMANDE = laDemande.LaDemande.PK_ID;
                    InsertOrUpdateDpersPhysique(laDemande.PersonePhysique, laCommande);
                }
                #endregion
                #region SocietePrives
                if (laDemande.SocietePrives != null)
                {
                    laDemande.SocietePrives.FK_IDDEMANDE = laDemande.LaDemande.PK_ID;
                    InsertOrUpdateDSociete(laDemande.SocietePrives, laCommande);
                }
                #endregion
                #region ObjetScanne
                EcrirePieceJointe(laDemande, laCommande);
                #endregion
                #region ElementDevis
                if (laDemande.EltDevis != null && laDemande.EltDevis.Count != 0 && laDemande.LaDemande.ISMETREAFAIRE == false)
                {
                    foreach (ObjELEMENTDEVIS item in laDemande.EltDevis)
                    {
                        item.FK_IDDEMANDE = laDemande.LaDemande.PK_ID;
                        item.NUMDEM = laDemande.LaDemande.NUMDEM;
                        InsertOrUpdateElementDevis(item, laCommande);
                    }
                }
                #endregion
                if (AvecTransmission)
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

/*
        public string VerifierDemande(CsDemande laDemande, bool AvecTransmission)
        {
            SqlCommand laCommande = DBBase.InitTransaction(ConnectionString);
            try
            {
                #region Demande
                if (laDemande.LaDemande != null)
                    InsertOrUpdateDemande(laDemande.LaDemande, laCommande);

                #endregion
                #region Dannotation
                if (laDemande.AnnotationDemande != null && laDemande.AnnotationDemande.Count != 0)
                {
                    foreach (CsAnnotation item in laDemande.AnnotationDemande)
                    {
                        item.FK_IDDEMANDE = laDemande.LaDemande.PK_ID;
                        InsertOrUpdateDannotation(item, laCommande);
                    }
                }
                #endregion

                #region ObjetScanne
                if (laDemande.ObjetScanne != null && laDemande.ObjetScanne.Count != 0)
                {
                    foreach (ObjDOCUMENTSCANNE item in laDemande.ObjetScanne.Where(y => y.CONTENU != null))
                    {
                        item.FK_IDDEMANDE = laDemande.LaDemande.PK_ID;
                        InsertOrUpdateDocumentScane(item, laCommande);
                    }
                }
                #endregion
                if (AvecTransmission)
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
*/

        public string VerifierDemande(CsDemande laDemande, bool AvecTransmission)
        {
            SqlCommand laCommande = DBBase.InitTransaction(ConnectionString);
            try
            {
                #region Demande
                if (laDemande.LaDemande != null)
                    InsertOrUpdateDemande(laDemande.LaDemande, laCommande);

                #endregion
                #region Dannotation
                if (laDemande.AnnotationDemande != null && laDemande.AnnotationDemande.Count != 0)
                {
                    foreach (CsAnnotation item in laDemande.AnnotationDemande)
                    {
                        item.FK_IDDEMANDE = laDemande.LaDemande.PK_ID;
                        InsertOrUpdateDannotation(item, laCommande);
                    }
                }
                #endregion
                #region ObjetScanne
                EcrirePieceJointe(laDemande, laCommande);
                #endregion
                if (AvecTransmission)
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

        public string ValideAchatTimbreDemande(CsDemande laDemande, bool AvecTransmission)
        {
            SqlCommand laCommande = DBBase.InitTransaction(ConnectionString);
            try
            {
                #region Demande
                if (laDemande.LaDemande != null)
                    InsertOrUpdateDemande(laDemande.LaDemande, laCommande);

                #endregion
                #region Dannotation
                if (laDemande.AnnotationDemande != null && laDemande.AnnotationDemande.Count != 0)
                {
                    foreach (CsAnnotation item in laDemande.AnnotationDemande)
                    {
                        item.FK_IDDEMANDE = laDemande.LaDemande.PK_ID;
                        InsertOrUpdateDannotation(item, laCommande);
                    }
                }
                #endregion
                #region ObjetScanne
                EcrirePieceJointe(laDemande, laCommande);
                #endregion
                if (AvecTransmission)
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

/*
        public string ValiderDemande(CsDemande laDemande, bool AvecTransmission)
        {
            SqlCommand laCommande = DBBase.InitTransaction(ConnectionString);
            try
            {
                #region Coutdemande
                if (laDemande.LstCoutDemande != null && laDemande.LstCoutDemande.Count != 0)
                {
                    foreach (CsDemandeDetailCout item in laDemande.LstCoutDemande)
                    {
                        item.FK_IDDEMANDE = laDemande.LaDemande.PK_ID;
                        item.NUMDEM = laDemande.LaDemande.NUMDEM;
                        InsertOrUpdateRubriqueDemande(item, laCommande);
                    }
                }
                #endregion
                #region ObjetScanne
                if (laDemande.ObjetScanne != null && laDemande.ObjetScanne.Count != 0)
                {
                    foreach (ObjDOCUMENTSCANNE item in laDemande.ObjetScanne.Where(y => y.CONTENU != null))
                    {
                        item.FK_IDDEMANDE = laDemande.LaDemande.PK_ID;
                        InsertOrUpdateDocumentScane(item, laCommande);
                    }
                }
                #endregion
                if (AvecTransmission)
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
*/

        public string ValiderDemande(CsDemande laDemande, bool AvecTransmission)
        {
            SqlCommand laCommande = DBBase.InitTransaction(ConnectionString);
            try
            {

                if (laDemande.LaDemande.TYPEDEMANDE == Enumere.ReprisIndex || laDemande.LaDemande.TYPEDEMANDE == Enumere.AnnulationFacture)
                {
                    InsertOrUpdateDemande(laDemande.LaDemande, laCommande);
                    if (!UpdateDEVENEMENT(laDemande, laCommande))
                        throw new Exception("Echec mise à jour dans DEVENEMENT");
                }

                //ZEG 29/09/2017
                if (laDemande.LaDemande.TYPEDEMANDE == Enumere.BranchementAbonementExtention)
                    InsertOrUpdateDemande(laDemande.LaDemande, laCommande);

                #region Coutdemande
                if (laDemande.LstCoutDemande != null && laDemande.LstCoutDemande.Count != 0)
                {
                    Delete_RubriqueDemande(laDemande.LaDemande.NUMDEM, laDemande.LaDemande.PK_ID, laCommande);
                    foreach (CsDemandeDetailCout item in laDemande.LstCoutDemande)
                    {
                        item.FK_IDDEMANDE = laDemande.LaDemande.PK_ID;
                        item.NUMDEM = laDemande.LaDemande.NUMDEM;
                        InsertOrUpdateRubriqueDemande(item, laCommande);
                    }
                }
                #endregion
                #region ObjetScanne
                EcrirePieceJointe(laDemande, laCommande);
                #endregion
                if (AvecTransmission)
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

        public bool Delete_DocumentScane(ObjDOCUMENTSCANNE leDocument)
        {
            SqlCommand cmds = new SqlCommand();

            cmds = DBBase.InitTransaction(ConnectionString);

            cmds.CommandTimeout = 1800;
            cmds.CommandType = CommandType.StoredProcedure;
            cmds.CommandText = "SPX_ACC_DELETEDOCUMENT";
            if (cmds.Parameters != null && cmds.Parameters.Count != 0) cmds.Parameters.Clear();

            cmds.Parameters.Add("@PK_ID", SqlDbType.UniqueIdentifier).Value = leDocument.PK_ID;
            DBBase.SetDBNullParametre(cmds.Parameters);
            try
            {
                if (cmds.Connection.State == ConnectionState.Closed)
                    cmds.Connection.Open();
                cmds.ExecuteNonQuery();
                cmds.Transaction.Commit();
                return true;
            }
            catch (Exception ex)
            {
                cmds.Transaction.Rollback();
                return false;
            }

            finally
            {
                if (cmds.Connection.State == ConnectionState.Open)
                    cmds.Connection.Close();
                cmds.Dispose();
            }
        }

        public void Delete_DocumentScane(ObjDOCUMENTSCANNE leDocument, SqlCommand cmds)
        {
            cmds.CommandTimeout = 1800;
            cmds.CommandType = CommandType.StoredProcedure;
            cmds.CommandText = "SPX_ACC_DELETEDOCUMENT";
            if (cmds.Parameters != null && cmds.Parameters.Count != 0) cmds.Parameters.Clear();

            cmds.Parameters.Add("@PK_ID", SqlDbType.UniqueIdentifier).Value = leDocument.PK_ID;
            DBBase.SetDBNullParametre(cmds.Parameters);
            try
            {
                if (cmds.Connection.State == ConnectionState.Closed)
                    cmds.Connection.Open();
                cmds.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception(cmds.CommandText + ":" + ex.Message);
            }
        }

        public void Delete_ElementDevis(string NumDem, int Iddemande, SqlCommand cmds)
        {
            cmds.CommandTimeout = 180;
            cmds.CommandType = CommandType.StoredProcedure;
            if (cmds.Parameters != null && cmds.Parameters.Count != 0) cmds.Parameters.Clear();

            cmds.CommandText = "SPX_ACC_DELETE_ELEMENTDEVIS";
            cmds.Parameters.Add("@NUMDEM", SqlDbType.VarChar, 20).Value = NumDem;
            cmds.Parameters.Add("@IDDEMANDE", SqlDbType.Int).Value = Iddemande;
            DBBase.SetDBNullParametre(cmds.Parameters);
            try
            {
                if (cmds.Connection.State == ConnectionState.Closed)
                    cmds.Connection.Open();
                cmds.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                throw new Exception(cmds.CommandText + ":" + ex.Message);
            }
        }

        public string InsertProgrammation(Guid idgroupe, List<CsDemandeBase> lesDemandeProgramme, DateTime pdate)
        {
            SqlCommand laCommande = null;
            try
            {
                laCommande = new SqlCommand();
                laCommande = DBBase.InitTransaction(ConnectionString);

                string Numero = RetourneProgramme(idgroupe, pdate, lesDemandeProgramme.FirstOrDefault().MATRICULE);
                foreach (CsDemandeBase item in lesDemandeProgramme)
                {
                    InsertOrUpdateProgrammation(idgroupe, false, false, true, null, null, Numero, item, pdate, laCommande);
                    TransmettreDemande(item.NUMDEM, item.FK_IDETAPEENCOURE, item.MATRICULE, laCommande);
                }
                //return "";
                laCommande.Transaction.Commit();
                return Numero;
            }
            catch (Exception ex)
            {
                laCommande.Transaction.Rollback();
                return ex.Message;
            }
            finally
            {
                if (laCommande.Connection.State == ConnectionState.Open)
                    laCommande.Connection.Close(); // Fermeture de la connection 
                laCommande.Dispose();
            }
        }
        public string InsertSortieCompteur(string programme, int IdLivreur, int IdRecepteur, int idEtape, List<CsCanalisation> lstCompteur)
        {
            SqlCommand laCommande = null;
            try
            {
                laCommande = new SqlCommand();
                laCommande = DBBase.InitTransaction(ConnectionString);
                foreach (CsCanalisation item in lstCompteur)
                {
                    InsertOrUpdateSortieCompteur(IdLivreur, IdRecepteur, item, laCommande);
                    TransmettreDemande(item.NUMDEM, idEtape, item.USERCREATION, laCommande);
                }

                RamenerDemandePourProgrammation(programme, laCommande);


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
                    laCommande.Connection.Close(); // Fermeture de la connection 
                laCommande.Dispose();
            }
        }

/*
        public string ProcesVerbal(CsDemande laDemande, bool Istransmetre)
        {
            SqlCommand laCommande = DBBase.InitTransaction(ConnectionString);
            try
            {
                if (laDemande.LaDemande.TYPEDEMANDE == Enumere.BranchementAbonement)
                {
                    #region BrnachementAbonnement
                    laDemande.LaDemande.DATEFIN = System.DateTime.Today;
                    InsertOrUpdateDemande(laDemande.LaDemande, laCommande);

                    InsertOrUpdateAG(laDemande.Ag, laCommande);
                    laDemande.Branchement.FK_IDAG = laDemande.Ag.PK_ID;
                    InsertOrUpdateBRT(laDemande.Branchement, laCommande);

                    laDemande.LeClient.FK_IDAG = laDemande.Ag.PK_ID; ;
                    InsertOrUpdateCLIENT(laDemande.LeClient, laCommande);

                    if (laDemande.LstCoutDemande != null && laDemande.LstCoutDemande.FirstOrDefault(i => i.COPER == Enumere.CoperCAU) != null)
                    {
                        CsDemandeDetailCout leAvance = laDemande.LstCoutDemande.FirstOrDefault(i => i.COPER == Enumere.CoperCAU);
                        laDemande.Abonne.AVANCE = (leAvance.MONTANTHT != null ? leAvance.MONTANTHT : 0) + (leAvance.MONTANTTAXE != null ? leAvance.MONTANTTAXE : 0);
                        laDemande.Abonne.DAVANCE = leAvance.DATECAISSE;
                    }
                    laDemande.Abonne.FK_IDCLIENT = laDemande.LeClient.PK_ID;
                    InsertOrUpdateABON(laDemande.Abonne, laCommande);
                    InsertOrUpdateCOMPTECLIENTSUITEPV(laDemande.LaDemande, laCommande);
                    if (laDemande.LstOrganeScelleDemande != null)
                        foreach (CsOrganeScelleDemande item in laDemande.LstOrganeScelleDemande)
                        {
                            item.FK_IDDEMANDE = laDemande.LaDemande.PK_ID;
                            item.FK_IDBRT = laDemande.Branchement.PK_ID;
                            item.USERCREATION = laDemande.LaDemande.MATRICULE;
                            InsertOrUpdatePOSESCELLE(item, laCommande);
                            Updatescelle(item, laCommande);
                        }

                    if (laDemande.LstCanalistion != null)
                        foreach (CsCanalisation item in laDemande.LstCanalistion)
                        {
                            #region Compteur
                            CsCompteur can = new CsCompteur();
                            can.ANNEEFAB = item.ANNEEFAB;
                            can.CADRAN = item.CADRAN;
                            can.COEFCOMPTAGE = item.COEFCOMPTAGE;
                            can.COEFLECT = item.COEFLECT;
                            can.MISEENSERVICE = System.DateTime.Today.Date;
                            can.FK_IDCALIBRECOMPTEUR = item.FK_IDCALIBRE;
                            can.FK_IDMARQUECOMPTEUR = item.FK_IDMARQUECOMPTEUR.Value;
                            can.FK_IDPRODUIT = item.FK_IDPRODUIT;
                            can.FK_IDTYPECOMPTEUR = item.FK_IDTYPECOMPTEUR.Value;
                            can.FK_IDSTATUTCOMPTEUR = 1;
                            can.FK_IDETATCOMPTEUR = 1;
                            can.FONCTIONNEMENT = item.FONCTIONNEMENT;
                            can.MARQUE = item.MARQUE;
                            can.NUMERO = item.NUMERO;
                            can.TYPECOMPTEUR = item.TYPECOMPTEUR;
                            can.PLOMBAGE = item.PLOMBAGE;
                            can.CODEPRODUIT = item.PRODUIT;
                            can.USERCREATION = item.USERCREATION;
                            can.USERMODIFICATION = item.USERMODIFICATION;
                            int IdCompteur = InsertOrUpdateCOMPTEUR(can, laCommande);

                            item.FK_IDABON = laDemande.Abonne.PK_ID;
                            item.FK_IDCOMPTEUR = IdCompteur;
                            #endregion
                            InsertOrUpdateCANALISATION(item, laCommande);

                            CsEvenement leEvt = laDemande.LstEvenement.FirstOrDefault(y => y.POINT == item.POINT);
                            leEvt.FK_IDCANALISATION = item.PK_ID;
                            leEvt.FK_IDABON = laDemande.Abonne.PK_ID;
                            leEvt.FK_IDCOMPTEUR = IdCompteur;
                            leEvt.STATUS = Enumere.EvenementPurger;
                            InsertOrUpdateEVENEMENT(leEvt, laCommande);
                        }
                    #endregion
                }
                if (laDemande.LaDemande.TYPEDEMANDE == Enumere.ChangementProduit )
                {
                    #region Changement de produit
                    laDemande.LaDemande.DATEFIN = System.DateTime.Today;
                    InsertOrUpdateDemande(laDemande.LaDemande, laCommande);

                    InsertOrUpdateAG(laDemande.Ag, laCommande);
                    laDemande.Branchement.FK_IDAG = laDemande.Ag.PK_ID;
                    InsertOrUpdateBRT(laDemande.Branchement, laCommande);

                    laDemande.LeClient.FK_IDAG = laDemande.Ag.PK_ID; ;
                    InsertOrUpdateCLIENT(laDemande.LeClient, laCommande);

                    if (laDemande.LstCoutDemande != null && laDemande.LstCoutDemande.FirstOrDefault(i => i.COPER == Enumere.CoperCAU) != null)
                    {
                        CsDemandeDetailCout leAvance = laDemande.LstCoutDemande.FirstOrDefault(i => i.COPER == Enumere.CoperCAU);
                        laDemande.Abonne.AVANCE = (leAvance.MONTANTHT != null ? leAvance.MONTANTHT : 0) + (leAvance.MONTANTTAXE != null ? leAvance.MONTANTTAXE : 0);
                        laDemande.Abonne.DAVANCE = leAvance.DATECAISSE;
                    }
                    laDemande.Abonne.FK_IDCLIENT = laDemande.LeClient.PK_ID;
                    InsertOrUpdateABON(laDemande.Abonne, laCommande);
                    InsertOrUpdateCOMPTECLIENTSUITEPV(laDemande.LaDemande, laCommande);
                    if (laDemande.LstOrganeScelleDemande != null)
                        foreach (CsOrganeScelleDemande item in laDemande.LstOrganeScelleDemande)
                        {
                            item.FK_IDDEMANDE = laDemande.LaDemande.PK_ID;
                            item.FK_IDBRT = laDemande.Branchement.PK_ID;
                            item.USERCREATION = laDemande.LaDemande.MATRICULE;
                            InsertOrUpdatePOSESCELLE(item, laCommande);
                            Updatescelle(item, laCommande);
                        }

                    if (laDemande.LstCanalistion != null)
                        foreach (CsCanalisation item in laDemande.LstCanalistion)
                        {
                            #region Compteur
                            CsCompteur can = new CsCompteur();
                            can.ANNEEFAB = item.ANNEEFAB;
                            can.CADRAN = item.CADRAN;
                            can.COEFCOMPTAGE = item.COEFCOMPTAGE;
                            can.COEFLECT = item.COEFLECT;
                            can.MISEENSERVICE = System.DateTime.Today.Date;
                            can.FK_IDCALIBRECOMPTEUR = item.FK_IDCALIBRE;
                            can.FK_IDMARQUECOMPTEUR = item.FK_IDMARQUECOMPTEUR.Value;
                            can.FK_IDPRODUIT = item.FK_IDPRODUIT;
                            can.FK_IDTYPECOMPTEUR = item.FK_IDTYPECOMPTEUR.Value;
                            can.FK_IDSTATUTCOMPTEUR = 1;
                            can.FK_IDETATCOMPTEUR = 1;
                            can.FONCTIONNEMENT = item.FONCTIONNEMENT;
                            can.MARQUE = item.MARQUE;
                            can.NUMERO = item.NUMERO;
                            can.TYPECOMPTEUR = item.TYPECOMPTEUR;
                            can.PLOMBAGE = item.PLOMBAGE;
                            can.CODEPRODUIT = item.PRODUIT;
                            can.USERCREATION = item.USERCREATION;
                            can.USERMODIFICATION = item.USERMODIFICATION;
                            int IdCompteur = InsertOrUpdateCOMPTEUR(can, laCommande);

                            item.FK_IDABON = laDemande.Abonne.PK_ID;
                            item.FK_IDCOMPTEUR = IdCompteur;
                            #endregion
                            InsertOrUpdateCANALISATION(item, laCommande);

                            CsEvenement leEvt = laDemande.LstEvenement.FirstOrDefault(y => y.POINT == item.POINT);
                            leEvt.FK_IDCANALISATION = item.PK_ID;
                            leEvt.FK_IDABON = laDemande.Abonne.PK_ID;
                            leEvt.FK_IDCOMPTEUR = IdCompteur;
                            leEvt.STATUS = Enumere.EvenementPurger;
                            InsertOrUpdateEVENEMENT(leEvt, laCommande);
                        }
                    #endregion
                }
                if (laDemande.LaDemande.TYPEDEMANDE == Enumere.AbonnementSeul)
                {
                    #region AbonnementSeul
                    if (laDemande.LaDemande.ISMETREAFAIRE == false)
                    {
                        MiseAJourAbonnementSeulSansMetre(laDemande, laCommande);
                        UpdateWKF(laDemande.LaDemande.NUMDEM, laDemande.LaDemande.PK_ID, 0, laDemande.LaDemande.MATRICULE, "ClotureDemande", laCommande);
                        laCommande.Transaction.Commit();
                        return "";
                    }
                    else
                    {
                        laDemande.LaDemande.DATEFIN = System.DateTime.Today;
                        InsertOrUpdateDemande(laDemande.LaDemande, laCommande);
                        CsAg leAg = Select_Ag(laDemande.LaDemande.FK_IDCENTRE, laDemande.LaDemande.CENTRE, laDemande.LaDemande.CLIENT);
                        laDemande.LeClient.FK_IDAG = leAg.PK_ID; ;
                        InsertOrUpdateCLIENT(laDemande.LeClient, laCommande);

                        if (laDemande.LstCoutDemande != null && laDemande.LstCoutDemande.FirstOrDefault(i => i.COPER == Enumere.CoperCAU) != null)
                        {
                            CsDemandeDetailCout leAvance = laDemande.LstCoutDemande.FirstOrDefault(i => i.COPER == Enumere.CoperCAU);
                            laDemande.Abonne.AVANCE = (leAvance.MONTANTHT != null ? leAvance.MONTANTHT : 0) + (leAvance.MONTANTTAXE != null ? leAvance.MONTANTTAXE : 0);
                            laDemande.Abonne.DAVANCE = leAvance.DATECAISSE;
                        }
                        laDemande.Abonne.FK_IDCLIENT = laDemande.LeClient.PK_ID;
                        InsertOrUpdateABON(laDemande.Abonne, laCommande);
                        InsertOrUpdateCOMPTECLIENTSUITEPV(laDemande.LaDemande, laCommande);
                        if (laDemande.LstOrganeScelleDemande != null)
                            foreach (CsOrganeScelleDemande item in laDemande.LstOrganeScelleDemande)
                            {
                                item.FK_IDDEMANDE = laDemande.LaDemande.PK_ID;
                                item.FK_IDBRT = laDemande.Branchement.PK_ID;
                                item.USERCREATION = laDemande.LaDemande.MATRICULE;
                                InsertOrUpdatePOSESCELLE(item, laCommande);
                                Updatescelle(item, laCommande);
                            }

                        if (laDemande.LstCanalistion != null)
                            foreach (CsCanalisation item in laDemande.LstCanalistion)
                            {
                                #region Compteur
                                CsCompteur can = new CsCompteur();
                                can.ANNEEFAB = item.ANNEEFAB;
                                can.CADRAN = item.CADRAN;
                                can.COEFCOMPTAGE = item.COEFCOMPTAGE;
                                can.COEFLECT = item.COEFLECT;
                                can.MISEENSERVICE = System.DateTime.Today.Date;
                                can.FK_IDCALIBRECOMPTEUR = item.FK_IDCALIBRE;
                                can.FK_IDMARQUECOMPTEUR = item.FK_IDMARQUECOMPTEUR.Value;
                                can.FK_IDPRODUIT = item.FK_IDPRODUIT;
                                can.FK_IDTYPECOMPTEUR = item.FK_IDTYPECOMPTEUR.Value;
                                can.FK_IDSTATUTCOMPTEUR = 1;
                                can.FK_IDETATCOMPTEUR = 1;
                                can.FONCTIONNEMENT = item.FONCTIONNEMENT;
                                can.MARQUE = item.MARQUE;
                                can.NUMERO = item.NUMERO;
                                can.TYPECOMPTEUR = item.TYPECOMPTEUR;
                                can.PLOMBAGE = item.PLOMBAGE;
                                can.CODEPRODUIT = item.PRODUIT;
                                can.USERCREATION = item.USERCREATION;
                                can.USERMODIFICATION = item.USERMODIFICATION;
                                int IdCompteur = InsertOrUpdateCOMPTEUR(can, laCommande);

                                item.FK_IDABON = laDemande.Abonne.PK_ID;
                                item.FK_IDCOMPTEUR = IdCompteur;
                                #endregion
                                InsertOrUpdateCANALISATION(item, laCommande);

                                CsEvenement leEvt = laDemande.LstEvenement.FirstOrDefault(y => y.POINT == item.POINT);
                                leEvt.FK_IDCANALISATION = item.PK_ID;
                                leEvt.FK_IDABON = laDemande.Abonne.PK_ID;
                                leEvt.FK_IDCOMPTEUR = IdCompteur;
                                leEvt.STATUS = Enumere.EvenementPurger;
                                InsertOrUpdateEVENEMENT(leEvt, laCommande);
                            }
                    }
                    #endregion
                }
                if (laDemande.LaDemande.TYPEDEMANDE == Enumere.AugmentationPuissance ||
                    laDemande.LaDemande.TYPEDEMANDE == Enumere.DimunitionPuissance)
                {
                    #region Augmentation & diminution de puissance
                    if (laDemande.LaDemande.ISMETREAFAIRE == false)
                    {
                        if (laDemande.LstCoutDemande != null && laDemande.LstCoutDemande.FirstOrDefault(i => i.COPER == Enumere.CoperCAU) != null)
                        {
                            CsDemandeDetailCout leAvance = laDemande.LstCoutDemande.FirstOrDefault(i => i.COPER == Enumere.CoperCAU);
                            laDemande.Abonne.AVANCE = (leAvance.MONTANTHT != null ? leAvance.MONTANTHT : 0) + (leAvance.MONTANTTAXE != null ? leAvance.MONTANTTAXE : 0);
                            laDemande.Abonne.DAVANCE = leAvance.DATECAISSE;
                        }
                        InsertOrUpdateABON(laDemande.Abonne, laCommande);
                        if (laDemande.Abonne.PRODUIT != Enumere.ElectriciteMT)
                        {
                            List<CsCanalisation> lstcanalisation = new List<CsCanalisation>();

                            if (laDemande.LstCanalistion != null && laDemande.LstCanalistion.Count != 0)
                            {
                                lstcanalisation = laDemande.LstCanalistion.Where(o => o.DEPOSE == null).ToList();
                                lstcanalisation.ForEach(y => y.REGLAGECOMPTEUR = laDemande.LaDemande.REGLAGECOMPTEUR);
                                lstcanalisation.ForEach(y => y.FK_IDREGLAGECOMPTEUR = laDemande.LaDemande.FK_IDREGLAGECOMPTEUR);
                            }
                            else
                            {
                                lstcanalisation = Select_CannalisationCompteur(laDemande.LaDemande.FK_IDCENTRE, laDemande.LaDemande.CENTRE, laDemande.LaDemande.CLIENT);
                                lstcanalisation = lstcanalisation.Where(o => o.DEPOSE == null).ToList();
                                lstcanalisation.ForEach(y => y.REGLAGECOMPTEUR = laDemande.LaDemande.REGLAGECOMPTEUR);
                                lstcanalisation.ForEach(y => y.FK_IDREGLAGECOMPTEUR = laDemande.LaDemande.FK_IDREGLAGECOMPTEUR);

                            }
                            foreach (CsCanalisation item in lstcanalisation)
                                InsertOrUpdateCANALISATION(item, laCommande);
                        }
                        UpdateWKF(laDemande.LaDemande.NUMDEM, laDemande.LaDemande.PK_ID, 0, laDemande.LaDemande.MATRICULE, "ClotureDemande", laCommande);
                        laCommande.Transaction.Commit();
                        return "";
                    }
                    else
                    {
                        laDemande.LaDemande.DATEFIN = System.DateTime.Today;
                        InsertOrUpdateDemande(laDemande.LaDemande, laCommande);
                        if (laDemande.LstCoutDemande != null && laDemande.LstCoutDemande.FirstOrDefault(i => i.COPER == Enumere.CoperCAU) != null)
                        {
                            CsDemandeDetailCout leAvance = laDemande.LstCoutDemande.FirstOrDefault(i => i.COPER == Enumere.CoperCAU);
                            laDemande.Abonne.AVANCE = (leAvance.MONTANTHT != null ? leAvance.MONTANTHT : 0) + (leAvance.MONTANTTAXE != null ? leAvance.MONTANTTAXE : 0);
                            laDemande.Abonne.DAVANCE = leAvance.DATECAISSE;
                        }
                        InsertOrUpdateABON(laDemande.Abonne, laCommande);
                        InsertOrUpdateCOMPTECLIENTSUITEPV(laDemande.LaDemande, laCommande);
                        if (laDemande.LstOrganeScelleDemande != null)
                            foreach (CsOrganeScelleDemande item in laDemande.LstOrganeScelleDemande)
                            {
                                item.FK_IDDEMANDE = laDemande.LaDemande.PK_ID;
                                item.FK_IDBRT = laDemande.Branchement.PK_ID;
                                item.USERCREATION = laDemande.LaDemande.MATRICULE;
                                InsertOrUpdatePOSESCELLE(item, laCommande);
                                Updatescelle(item, laCommande);
                            }

                        if (laDemande.LstCanalistion != null)
                            foreach (CsCanalisation item in laDemande.LstCanalistion)
                            {
                                #region Compteur
                                CsCompteur can = new CsCompteur();
                                can.ANNEEFAB = item.ANNEEFAB;
                                can.CADRAN = item.CADRAN;
                                can.COEFCOMPTAGE = item.COEFCOMPTAGE;
                                can.COEFLECT = item.COEFLECT;
                                can.MISEENSERVICE = System.DateTime.Today.Date;
                                can.FK_IDCALIBRECOMPTEUR = item.FK_IDCALIBRE;
                                can.FK_IDMARQUECOMPTEUR = item.FK_IDMARQUECOMPTEUR.Value;
                                can.FK_IDPRODUIT = item.FK_IDPRODUIT;
                                can.FK_IDTYPECOMPTEUR = item.FK_IDTYPECOMPTEUR.Value;
                                can.FK_IDSTATUTCOMPTEUR = 1;
                                can.FK_IDETATCOMPTEUR = 1;
                                can.FONCTIONNEMENT = item.FONCTIONNEMENT;
                                can.MARQUE = item.MARQUE;
                                can.NUMERO = item.NUMERO;
                                can.TYPECOMPTEUR = item.TYPECOMPTEUR;
                                can.PLOMBAGE = item.PLOMBAGE;
                                can.CODEPRODUIT = item.PRODUIT;
                                can.USERCREATION = item.USERCREATION;
                                can.USERMODIFICATION = item.USERMODIFICATION;
                                int IdCompteur = InsertOrUpdateCOMPTEUR(can, laCommande);

                                item.FK_IDABON = laDemande.Abonne.PK_ID;
                                item.FK_IDCOMPTEUR = IdCompteur;
                                #endregion
                                InsertOrUpdateCANALISATION(item, laCommande);

                                CsEvenement leEvt = laDemande.LstEvenement.FirstOrDefault(y => y.POINT == item.POINT);
                                leEvt.FK_IDCANALISATION = item.PK_ID;
                                leEvt.FK_IDABON = laDemande.Abonne.PK_ID;
                                leEvt.FK_IDCOMPTEUR = IdCompteur;
                                leEvt.STATUS = Enumere.EvenementPurger;
                                InsertOrUpdateEVENEMENT(leEvt, laCommande);
                            }
                    }
                    #endregion
                 
                }
                TransmettreDemande(laDemande.LaDemande.NUMDEM, laDemande.InfoDemande.FK_IDETAPEACTUELLE , laDemande.LaDemande.MATRICULE , laCommande);
                laCommande.Transaction.Commit();
                return "";
            }
            catch (Exception ex)
            {
                laCommande.Transaction.Commit();
                return ex.Message;
            }
            finally
            {
                if (laCommande.Connection.State == ConnectionState.Open)
                    laCommande.Connection.Close(); // Fermeture de la connection 
                laCommande.Dispose();
            }
        }
*/

        /*
        public string ProcesVerbal(CsDemande laDemande, bool Istransmetre)
        {
            SqlCommand laCommande = DBBase.InitTransaction(ConnectionString);
            try
            {
                //ZEG 29/09/2017
                if (laDemande.LaDemande.TYPEDEMANDE == Enumere.Resiliation)
                    ValiderDemande(laDemande);

                if (laDemande.LaDemande.TYPEDEMANDE == Enumere.BranchementAbonement || 
                    laDemande.LaDemande.TYPEDEMANDE == Enumere.BranchementAbonementExtention ||
                    laDemande.LaDemande.TYPEDEMANDE == Enumere.Reabonnement)
                {
                    #region BrnachementAbonnement
                    laDemande.LaDemande.DATEFIN = System.DateTime.Today;
                    InsertOrUpdateDemande(laDemande.LaDemande, laCommande);

                    InsertOrUpdateAG(laDemande.Ag, laCommande);
                    laDemande.Branchement.FK_IDAG = laDemande.Ag.PK_ID;
                    InsertOrUpdateBRT(laDemande.Branchement, laCommande);

                    laDemande.LeClient.FK_IDAG = laDemande.Ag.PK_ID; ;
                    InsertOrUpdateCLIENT(laDemande.LeClient, laCommande);

                    if (laDemande.LstCoutDemande != null && laDemande.LstCoutDemande.FirstOrDefault(i => i.COPER == Enumere.CoperCAU) != null)
                    {
                        CsDemandeDetailCout leAvance = laDemande.LstCoutDemande.FirstOrDefault(i => i.COPER == Enumere.CoperCAU);
                        laDemande.Abonne.AVANCE = (leAvance.MONTANTHT != null ? leAvance.MONTANTHT : 0) + (leAvance.MONTANTTAXE != null ? leAvance.MONTANTTAXE : 0);
                        laDemande.Abonne.DAVANCE = leAvance.DATECAISSE;
                    }
                    laDemande.Abonne.FK_IDCLIENT = laDemande.LeClient.PK_ID;
                    InsertOrUpdateABON(laDemande.Abonne, laCommande);
                    InsertOrUpdateCOMPTECLIENTSUITEPV(laDemande.LaDemande, laCommande);
                    if (laDemande.LstOrganeScelleDemande != null)
                        foreach (CsOrganeScelleDemande item in laDemande.LstOrganeScelleDemande)
                        {
                            item.FK_IDDEMANDE = laDemande.LaDemande.PK_ID;
                            item.FK_IDBRT = laDemande.Branchement.PK_ID;
                            item.USERCREATION = laDemande.LaDemande.MATRICULE;
                            InsertOrUpdatePOSESCELLE(item, laCommande);
                            Updatescelle(item, laCommande);
                        }

                    if (laDemande.LstCanalistion != null)
                        foreach (CsCanalisation item in laDemande.LstCanalistion)
                        {
                            #region Compteur
                            CsCompteur can = new CsCompteur();
                            can.ANNEEFAB = item.ANNEEFAB;
                            can.CADRAN = item.CADRAN;
                            can.COEFCOMPTAGE = item.COEFCOMPTAGE;
                            can.COEFLECT = item.COEFLECT;
                            can.MISEENSERVICE = System.DateTime.Today.Date;
                            can.FK_IDCALIBRECOMPTEUR = item.FK_IDCALIBRE;
                            can.FK_IDMARQUECOMPTEUR = item.FK_IDMARQUECOMPTEUR.Value;
                            can.FK_IDPRODUIT = item.FK_IDPRODUIT;
                            can.FK_IDTYPECOMPTEUR = item.FK_IDTYPECOMPTEUR.Value;
                            can.FK_IDSTATUTCOMPTEUR = 1;
                            can.FK_IDETATCOMPTEUR = 1;
                            can.FONCTIONNEMENT = item.FONCTIONNEMENT;
                            can.MARQUE = item.MARQUE;
                            can.NUMERO = item.NUMERO;
                            can.TYPECOMPTEUR = item.TYPECOMPTEUR;
                            can.PLOMBAGE = item.PLOMBAGE;
                            can.CODEPRODUIT = item.PRODUIT;
                            can.USERCREATION = item.USERCREATION;
                            can.USERMODIFICATION = item.USERMODIFICATION;
                            int IdCompteur = InsertOrUpdateCOMPTEUR(can, laCommande);

                            item.FK_IDABON = laDemande.Abonne.PK_ID;
                            item.FK_IDCOMPTEUR = IdCompteur;
                            #endregion
                            InsertOrUpdateCANALISATION(item, laCommande);

                            CsEvenement leEvt = laDemande.LstEvenement.FirstOrDefault(y => y.POINT == item.POINT);
                            leEvt.FK_IDCANALISATION = item.PK_ID;
                            leEvt.FK_IDABON = laDemande.Abonne.PK_ID;
                            leEvt.FK_IDCOMPTEUR = IdCompteur;
                            leEvt.STATUS = Enumere.EvenementPurger;
                            InsertOrUpdateEVENEMENT(leEvt, laCommande);
                        }
                    #endregion
                }
                if (laDemande.LaDemande.TYPEDEMANDE == Enumere.ChangementProduit)
                {
                    #region Changement de produit
                    laDemande.LaDemande.DATEFIN = System.DateTime.Today;
                    InsertOrUpdateDemande(laDemande.LaDemande, laCommande);

                    InsertOrUpdateAG(laDemande.Ag, laCommande);
                    laDemande.Branchement.FK_IDAG = laDemande.Ag.PK_ID;
                    InsertOrUpdateBRT(laDemande.Branchement, laCommande);

                    laDemande.LeClient.FK_IDAG = laDemande.Ag.PK_ID; ;
                    InsertOrUpdateCLIENT(laDemande.LeClient, laCommande);

                    if (laDemande.LstCoutDemande != null && laDemande.LstCoutDemande.FirstOrDefault(i => i.COPER == Enumere.CoperCAU) != null)
                    {
                        CsDemandeDetailCout leAvance = laDemande.LstCoutDemande.FirstOrDefault(i => i.COPER == Enumere.CoperCAU);
                        laDemande.Abonne.AVANCE = (leAvance.MONTANTHT != null ? leAvance.MONTANTHT : 0) + (leAvance.MONTANTTAXE != null ? leAvance.MONTANTTAXE : 0);
                        laDemande.Abonne.DAVANCE = leAvance.DATECAISSE;
                    }
                    laDemande.Abonne.FK_IDCLIENT = laDemande.LeClient.PK_ID;
                    InsertOrUpdateABON(laDemande.Abonne, laCommande);
                    InsertOrUpdateCOMPTECLIENTSUITEPV(laDemande.LaDemande, laCommande);
                    if (laDemande.LstOrganeScelleDemande != null)
                        foreach (CsOrganeScelleDemande item in laDemande.LstOrganeScelleDemande)
                        {
                            item.FK_IDDEMANDE = laDemande.LaDemande.PK_ID;
                            item.FK_IDBRT = laDemande.Branchement.PK_ID;
                            item.USERCREATION = laDemande.LaDemande.MATRICULE;
                            InsertOrUpdatePOSESCELLE(item, laCommande);
                            Updatescelle(item, laCommande);
                        }

                    if (laDemande.LstCanalistion != null)
                        foreach (CsCanalisation item in laDemande.LstCanalistion)
                        {
                            #region Compteur
                            CsCompteur can = new CsCompteur();
                            can.ANNEEFAB = item.ANNEEFAB;
                            can.CADRAN = item.CADRAN;
                            can.COEFCOMPTAGE = item.COEFCOMPTAGE;
                            can.COEFLECT = item.COEFLECT;
                            can.MISEENSERVICE = System.DateTime.Today.Date;
                            can.FK_IDCALIBRECOMPTEUR = item.FK_IDCALIBRE;
                            can.FK_IDMARQUECOMPTEUR = item.FK_IDMARQUECOMPTEUR.Value;
                            can.FK_IDPRODUIT = item.FK_IDPRODUIT;
                            can.FK_IDTYPECOMPTEUR = item.FK_IDTYPECOMPTEUR.Value;
                            can.FK_IDSTATUTCOMPTEUR = 1;
                            can.FK_IDETATCOMPTEUR = 1;
                            can.FONCTIONNEMENT = item.FONCTIONNEMENT;
                            can.MARQUE = item.MARQUE;
                            can.NUMERO = item.NUMERO;
                            can.TYPECOMPTEUR = item.TYPECOMPTEUR;
                            can.PLOMBAGE = item.PLOMBAGE;
                            can.CODEPRODUIT = item.PRODUIT;
                            can.USERCREATION = item.USERCREATION;
                            can.USERMODIFICATION = item.USERMODIFICATION;
                            int IdCompteur = InsertOrUpdateCOMPTEUR(can, laCommande);

                            item.FK_IDABON = laDemande.Abonne.PK_ID;
                            item.FK_IDCOMPTEUR = IdCompteur;
                            #endregion
                            InsertOrUpdateCANALISATION(item, laCommande);

                            CsEvenement leEvt = laDemande.LstEvenement.FirstOrDefault(y => y.POINT == item.POINT);
                            leEvt.FK_IDCANALISATION = item.PK_ID;
                            leEvt.FK_IDABON = laDemande.Abonne.PK_ID;
                            leEvt.FK_IDCOMPTEUR = IdCompteur;
                            leEvt.STATUS = Enumere.EvenementPurger;
                            InsertOrUpdateEVENEMENT(leEvt, laCommande);
                        }
                    #endregion
                }
                if (laDemande.LaDemande.TYPEDEMANDE == Enumere.AbonnementSeul)
                {
                    #region AbonnementSeul
                    if (laDemande.LaDemande.ISMETREAFAIRE == false)
                    {
                        MiseAJourAbonnementSeulSansMetre(laDemande, laCommande);
                        UpdateWKF(laDemande.LaDemande.NUMDEM, laDemande.LaDemande.PK_ID, 0, laDemande.LaDemande.MATRICULE, "ClotureDemande", laCommande);
                        laCommande.Transaction.Commit();
                        return "";
                    }
                    else
                    {
                        laDemande.LaDemande.DATEFIN = System.DateTime.Today;
                        InsertOrUpdateDemande(laDemande.LaDemande, laCommande);
                        CsAg leAg = Select_Ag(laDemande.LaDemande.FK_IDCENTRE, laDemande.LaDemande.CENTRE, laDemande.LaDemande.CLIENT);
                        laDemande.LeClient.FK_IDAG = leAg.PK_ID; ;
                        InsertOrUpdateCLIENT(laDemande.LeClient, laCommande);

                        if (laDemande.LstCoutDemande != null && laDemande.LstCoutDemande.FirstOrDefault(i => i.COPER == Enumere.CoperCAU) != null)
                        {
                            CsDemandeDetailCout leAvance = laDemande.LstCoutDemande.FirstOrDefault(i => i.COPER == Enumere.CoperCAU);
                            laDemande.Abonne.AVANCE = (leAvance.MONTANTHT != null ? leAvance.MONTANTHT : 0) + (leAvance.MONTANTTAXE != null ? leAvance.MONTANTTAXE : 0);
                            laDemande.Abonne.DAVANCE = leAvance.DATECAISSE;
                        }
                        laDemande.Abonne.FK_IDCLIENT = laDemande.LeClient.PK_ID;
                        InsertOrUpdateABON(laDemande.Abonne, laCommande);
                        InsertOrUpdateCOMPTECLIENTSUITEPV(laDemande.LaDemande, laCommande);
                        if (laDemande.LstOrganeScelleDemande != null)
                            foreach (CsOrganeScelleDemande item in laDemande.LstOrganeScelleDemande)
                            {
                                item.FK_IDDEMANDE = laDemande.LaDemande.PK_ID;
                                item.FK_IDBRT = laDemande.Branchement.PK_ID;
                                item.USERCREATION = laDemande.LaDemande.MATRICULE;
                                InsertOrUpdatePOSESCELLE(item, laCommande);
                                Updatescelle(item, laCommande);
                            }

                        if (laDemande.LstCanalistion != null)
                            foreach (CsCanalisation item in laDemande.LstCanalistion)
                            {
                                #region Compteur
                                CsCompteur can = new CsCompteur();
                                can.ANNEEFAB = item.ANNEEFAB;
                                can.CADRAN = item.CADRAN;
                                can.COEFCOMPTAGE = item.COEFCOMPTAGE;
                                can.COEFLECT = item.COEFLECT;
                                can.MISEENSERVICE = System.DateTime.Today.Date;
                                can.FK_IDCALIBRECOMPTEUR = item.FK_IDCALIBRE;
                                can.FK_IDMARQUECOMPTEUR = item.FK_IDMARQUECOMPTEUR.Value;
                                can.FK_IDPRODUIT = item.FK_IDPRODUIT;
                                can.FK_IDTYPECOMPTEUR = item.FK_IDTYPECOMPTEUR.Value;
                                can.FK_IDSTATUTCOMPTEUR = 1;
                                can.FK_IDETATCOMPTEUR = 1;
                                can.FONCTIONNEMENT = item.FONCTIONNEMENT;
                                can.MARQUE = item.MARQUE;
                                can.NUMERO = item.NUMERO;
                                can.TYPECOMPTEUR = item.TYPECOMPTEUR;
                                can.PLOMBAGE = item.PLOMBAGE;
                                can.CODEPRODUIT = item.PRODUIT;
                                can.USERCREATION = item.USERCREATION;
                                can.USERMODIFICATION = item.USERMODIFICATION;
                                int IdCompteur = InsertOrUpdateCOMPTEUR(can, laCommande);

                                item.FK_IDABON = laDemande.Abonne.PK_ID;
                                item.FK_IDCOMPTEUR = IdCompteur;
                                #endregion
                                InsertOrUpdateCANALISATION(item, laCommande);

                                CsEvenement leEvt = laDemande.LstEvenement.FirstOrDefault(y => y.POINT == item.POINT);
                                leEvt.FK_IDCANALISATION = item.PK_ID;
                                leEvt.FK_IDABON = laDemande.Abonne.PK_ID;
                                leEvt.FK_IDCOMPTEUR = IdCompteur;
                                leEvt.STATUS = Enumere.EvenementPurger;
                                InsertOrUpdateEVENEMENT(leEvt, laCommande);
                            }
                    }
                    #endregion
                }
                if (laDemande.LaDemande.TYPEDEMANDE == Enumere.AugmentationPuissance ||
                    laDemande.LaDemande.TYPEDEMANDE == Enumere.DimunitionPuissance)
                {
                    DescendreAvance(laDemande.LaDemande, laCommande);
                    #region Augmentation & diminution de puissance
                    if (laDemande.LaDemande.ISMETREAFAIRE == false)
                    {
                        if (laDemande.LstCoutDemande != null && laDemande.LstCoutDemande.FirstOrDefault(i => i.COPER == Enumere.CoperCAU) != null)
                        {
                            CsDemandeDetailCout leAvance = laDemande.LstCoutDemande.FirstOrDefault(i => i.COPER == Enumere.CoperCAU);
                            laDemande.Abonne.AVANCE = (leAvance.MONTANTHT != null ? leAvance.MONTANTHT : 0) + (leAvance.MONTANTTAXE != null ? leAvance.MONTANTTAXE : 0);
                            laDemande.Abonne.DAVANCE = leAvance.DATECAISSE;
                        }
                        InsertOrUpdateABON(laDemande.Abonne, laCommande);
                        if (laDemande.Abonne.PRODUIT != Enumere.ElectriciteMT)
                        {
                            List<CsCanalisation> lstcanalisation = new List<CsCanalisation>();

                            if (laDemande.LstCanalistion != null && laDemande.LstCanalistion.Count != 0)
                            {
                                lstcanalisation = laDemande.LstCanalistion.Where(o => o.DEPOSE == null).ToList();
                                lstcanalisation.ForEach(y => y.REGLAGECOMPTEUR = laDemande.LaDemande.REGLAGECOMPTEUR);
                                lstcanalisation.ForEach(y => y.FK_IDREGLAGECOMPTEUR = laDemande.LaDemande.FK_IDREGLAGECOMPTEUR);
                            }
                            else
                            {
                                lstcanalisation = Select_CannalisationCompteur(laDemande.LaDemande.FK_IDCENTRE, laDemande.LaDemande.CENTRE, laDemande.LaDemande.CLIENT);
                                lstcanalisation = lstcanalisation.Where(o => o.DEPOSE == null).ToList();
                                lstcanalisation.ForEach(y => y.REGLAGECOMPTEUR = laDemande.LaDemande.REGLAGECOMPTEUR);
                                lstcanalisation.ForEach(y => y.FK_IDREGLAGECOMPTEUR = laDemande.LaDemande.FK_IDREGLAGECOMPTEUR);

                            }
                            foreach (CsCanalisation item in lstcanalisation)
                                InsertOrUpdateCANALISATION(item, laCommande);
                        }
                        UpdateWKF(laDemande.LaDemande.NUMDEM, laDemande.LaDemande.PK_ID, 0, laDemande.LaDemande.MATRICULE, "ClotureDemande", laCommande);
                        laCommande.Transaction.Commit();
                        return "";
                    }
                    else
                    {
                        laDemande.LaDemande.DATEFIN = System.DateTime.Today;
                        InsertOrUpdateDemande(laDemande.LaDemande, laCommande);
                        if (laDemande.LstCoutDemande != null && laDemande.LstCoutDemande.FirstOrDefault(i => i.COPER == Enumere.CoperCAU) != null)
                        {
                            CsDemandeDetailCout leAvance = laDemande.LstCoutDemande.FirstOrDefault(i => i.COPER == Enumere.CoperCAU);
                            laDemande.Abonne.AVANCE = (leAvance.MONTANTHT != null ? leAvance.MONTANTHT : 0) + (leAvance.MONTANTTAXE != null ? leAvance.MONTANTTAXE : 0);
                            laDemande.Abonne.DAVANCE = leAvance.DATECAISSE;
                        }
                        InsertOrUpdateABON(laDemande.Abonne, laCommande);
                        InsertOrUpdateCOMPTECLIENTSUITEPV(laDemande.LaDemande, laCommande);
                        if (laDemande.LstOrganeScelleDemande != null)
                            foreach (CsOrganeScelleDemande item in laDemande.LstOrganeScelleDemande)
                            {
                                item.FK_IDDEMANDE = laDemande.LaDemande.PK_ID;
                                item.FK_IDBRT = laDemande.Branchement.PK_ID;
                                item.USERCREATION = laDemande.LaDemande.MATRICULE;
                                InsertOrUpdatePOSESCELLE(item, laCommande);
                                Updatescelle(item, laCommande);
                            }

                        if (laDemande.LstCanalistion != null)
                            foreach (CsCanalisation item in laDemande.LstCanalistion)
                            {
                                #region Compteur
                                CsCompteur can = new CsCompteur();
                                can.ANNEEFAB = item.ANNEEFAB;
                                can.CADRAN = item.CADRAN;
                                can.COEFCOMPTAGE = item.COEFCOMPTAGE;
                                can.COEFLECT = item.COEFLECT;
                                can.MISEENSERVICE = System.DateTime.Today.Date;
                                can.FK_IDCALIBRECOMPTEUR = item.FK_IDCALIBRE;
                                can.FK_IDMARQUECOMPTEUR = item.FK_IDMARQUECOMPTEUR.Value;
                                can.FK_IDPRODUIT = item.FK_IDPRODUIT;
                                can.FK_IDTYPECOMPTEUR = item.FK_IDTYPECOMPTEUR.Value;
                                can.FK_IDSTATUTCOMPTEUR = 1;
                                can.FK_IDETATCOMPTEUR = 1;
                                can.FONCTIONNEMENT = item.FONCTIONNEMENT;
                                can.MARQUE = item.MARQUE;
                                can.NUMERO = item.NUMERO;
                                can.TYPECOMPTEUR = item.TYPECOMPTEUR;
                                can.PLOMBAGE = item.PLOMBAGE;
                                can.CODEPRODUIT = item.PRODUIT;
                                can.USERCREATION = item.USERCREATION;
                                can.USERMODIFICATION = item.USERMODIFICATION;
                                int IdCompteur = InsertOrUpdateCOMPTEUR(can, laCommande);

                                item.FK_IDABON = laDemande.Abonne.PK_ID;
                                item.FK_IDCOMPTEUR = IdCompteur;
                                #endregion
                                InsertOrUpdateCANALISATION(item, laCommande);

                                CsEvenement leEvt = laDemande.LstEvenement.FirstOrDefault(y => y.POINT == item.POINT);
                                leEvt.FK_IDCANALISATION = item.PK_ID;
                                leEvt.FK_IDABON = laDemande.Abonne.PK_ID;
                                leEvt.FK_IDCOMPTEUR = IdCompteur;
                                leEvt.STATUS = Enumere.EvenementPurger;
                                InsertOrUpdateEVENEMENT(leEvt, laCommande);
                            }
                    }
                    #endregion

                }
                TransmettreDemande(laDemande.LaDemande.NUMDEM, laDemande.InfoDemande.FK_IDETAPEACTUELLE, laDemande.LaDemande.MATRICULE, laCommande);
                laCommande.Transaction.Commit();
                return "";
            }
            catch (Exception ex)
            {
                laCommande.Transaction.Commit();
                return ex.Message;
            }
            finally
            {
                if (laCommande.Connection.State == ConnectionState.Open)
                    laCommande.Connection.Close(); // Fermeture de la connection 
                laCommande.Dispose();
            }
        }
        */
        
        public string ProcesVerbal(CsDemande laDemande, bool Istransmetre)
        {
            SqlCommand laCommande = DBBase.InitTransaction(ConnectionString);
            SqlConnection  laConnection = DBBase.InitConnection (ConnectionString);
            try
            {
                //ZEG 29/09/2017
                #region Resiliation 
                if (laDemande.LaDemande.TYPEDEMANDE == Enumere.Resiliation)
                {
                    List<CsCasind> lstCas =new  DB_ParametresGeneraux().RetourneCasind();
                    foreach (CsEvenement item in laDemande.LstEvenement)
                    {
                        item.FK_IDDEMANDE = laDemande.LaDemande.PK_ID;
                        item.NUMDEM  = laDemande.LaDemande.NUMDEM ;
                        item.FK_IDCAS = lstCas.FirstOrDefault(i=>i.CODE == item.CAS ).PK_ID ;
                    }
                    InsertOrUpdateDEvenement(laDemande.LstEvenement, laCommande);
                }
                #endregion
                #region BrnachementAbonnement & BranchementAbonementExtention

                if (laDemande.LaDemande.TYPEDEMANDE == Enumere.BranchementAbonement ||
                    laDemande.LaDemande.TYPEDEMANDE == Enumere.BranchementAbonementExtention ||
                    laDemande.LaDemande.TYPEDEMANDE == Enumere.Reabonnement)
                {
                    laDemande.LaDemande.DATEFIN = System.DateTime.Today;
                    InsertOrUpdateDemande(laDemande.LaDemande, laCommande);


                    if (laDemande.LaDemande.TYPEDEMANDE == Enumere.Reabonnement) /*ZEG 03/01/2021*/
                    {
                        laDemande.Ag = Select_Ag(laDemande.Ag.FK_IDCENTRE, laDemande.Ag.CENTRE, laDemande.Ag.CLIENT, laConnection);
                        laDemande.Branchement.FK_IDAG = laDemande.Ag.PK_ID;
                    }
                    else
                    {
                        InsertOrUpdateAG(laDemande.Ag, laCommande);
                        laDemande.Branchement.FK_IDAG = laDemande.Ag.PK_ID;
                        InsertOrUpdateBRT(laDemande.Branchement, laCommande);
                    }


                    laDemande.LeClient.FK_IDAG = laDemande.Ag.PK_ID;
                    InsertOrUpdateCLIENT(laDemande.LeClient, laCommande);

                    #region DPersonePhysique
                    if (laDemande.PersonePhysique != null)
                    {
                        laDemande.PersonePhysique.FK_IDCLIENT = laDemande.LeClient.PK_ID;
                        InsertOrUpdateDpersPhysique(laDemande.PersonePhysique, laCommande);
                    }
                    #endregion
                    #region SocietePrives
                    if (laDemande.SocietePrives != null)
                    {
                        laDemande.SocietePrives.FK_IDCLIENT = laDemande.LeClient.PK_ID;
                        InsertOrUpdateDSociete(laDemande.SocietePrives, laCommande);
                    }
                    #endregion
                    #region Dadministration
                    if (laDemande.AdministrationInstitut != null)
                    {
                        laDemande.AdministrationInstitut.FK_IDCLIENT = laDemande.LeClient.PK_ID;
                        InsertOrUpdateDAdmnistration_Institut(laDemande.AdministrationInstitut, laCommande);
                    }
                    #endregion

                    if (laDemande.LstCoutDemande != null && laDemande.LstCoutDemande.FirstOrDefault(i => i.COPER == Enumere.CoperCAU) != null)
                    {
                        CsDemandeDetailCout leAvance = laDemande.LstCoutDemande.FirstOrDefault(i => i.COPER == Enumere.CoperCAU);
                        laDemande.Abonne.AVANCE = (leAvance.MONTANTHT != null ? leAvance.MONTANTHT : 0) + (leAvance.MONTANTTAXE != null ? leAvance.MONTANTTAXE : 0);
                        laDemande.Abonne.DAVANCE = leAvance.DATECAISSE;
                    }
                    laDemande.Abonne.FK_IDCLIENT = laDemande.LeClient.PK_ID;
                    laDemande.Abonne.DRES = null; /*ZEG 03/01/2021*/
                    InsertOrUpdateABON(laDemande.Abonne, laCommande);
                    InsertOrUpdateCOMPTECLIENTSUITEPV(laDemande.LaDemande, laCommande);
                    if (laDemande.LstOrganeScelleDemande != null)
                        foreach (CsOrganeScelleDemande item in laDemande.LstOrganeScelleDemande)
                        {
                            item.FK_IDDEMANDE = laDemande.LaDemande.PK_ID;
                            item.FK_IDBRT = laDemande.Branchement.PK_ID;
                            item.USERCREATION = laDemande.LaDemande.MATRICULE;
                            InsertOrUpdatePOSESCELLE(item, laCommande);
                            Updatescelle(item, laCommande);
                        }

                    if (laDemande.LstCanalistion != null)
                        foreach (CsCanalisation item in laDemande.LstCanalistion)
                        {
                            #region Compteur
                            CsCompteur can = new CsCompteur();
                            can.ANNEEFAB = item.ANNEEFAB;
                            can.CADRAN = item.CADRAN;
                            can.COEFCOMPTAGE = item.COEFCOMPTAGE;
                            can.COEFLECT = item.COEFLECT;
                            can.MISEENSERVICE = System.DateTime.Today.Date;
                            can.FK_IDCALIBRECOMPTEUR = item.FK_IDCALIBRE;
                            can.FK_IDMARQUECOMPTEUR = item.FK_IDMARQUECOMPTEUR.Value;
                            can.FK_IDPRODUIT = item.FK_IDPRODUIT;
                            can.FK_IDTYPECOMPTEUR = item.FK_IDTYPECOMPTEUR.Value;
                            can.FK_IDSTATUTCOMPTEUR = 1;
                            can.FK_IDETATCOMPTEUR = 1;
                            can.FONCTIONNEMENT = item.FONCTIONNEMENT;
                            can.MARQUE = item.MARQUE;
                            can.NUMERO = item.NUMERO;
                            can.TYPECOMPTEUR = item.TYPECOMPTEUR;
                            can.PLOMBAGE = item.PLOMBAGE;
                            can.CODEPRODUIT = item.PRODUIT;
                            can.USERCREATION = item.USERCREATION;
                            can.USERMODIFICATION = item.USERMODIFICATION;
                            int IdCompteur = InsertOrUpdateCOMPTEUR(can, laCommande);

                            item.FK_IDABON = laDemande.Abonne.PK_ID;
                            item.FK_IDCOMPTEUR = IdCompteur;

                            
                            item.DEPOSE = null; /*ZEG 03/01/2021*/
                            item.POSE = laDemande.LstEvenement.FirstOrDefault(y => y.POINT == item.POINT).DATEEVT; /*ZEG 03/01/2021*/
                            #endregion
                            InsertOrUpdateCANALISATION(item, laCommande);

                            CsEvenement leEvt = laDemande.LstEvenement.FirstOrDefault(y => y.POINT == item.POINT);
                            leEvt.FK_IDCANALISATION = item.PK_ID;
                            leEvt.FK_IDABON = laDemande.Abonne.PK_ID;
                            leEvt.FK_IDCOMPTEUR = IdCompteur;
                            leEvt.STATUS = Enumere.EvenementPurger;
                            InsertOrUpdateEVENEMENT(leEvt, laCommande);
                        }
                }
                #endregion
                #region Changement de produit
                if (laDemande.LaDemande.TYPEDEMANDE == Enumere.ChangementProduit)
                {
                    laDemande.LaDemande.DATEFIN = System.DateTime.Today;
                    InsertOrUpdateDemande(laDemande.LaDemande, laCommande);

                    CsAg ag = Select_Ag(laDemande.LaDemande.FK_IDCENTRE, laDemande.LaDemande.CENTRE, laDemande.LaDemande.CLIENT, laConnection);
                    CsAg dag = Select_DAg(laDemande.LaDemande.NUMDEM, laDemande.LaDemande.PK_ID, laConnection);

                    if (dag != null)
                        laDemande.Ag = dag;
                    laDemande.Ag.PK_ID = ag.PK_ID;


                    InsertOrUpdateAG(laDemande.Ag, laCommande);
                    laDemande.Branchement.FK_IDAG = laDemande.Ag.PK_ID;
                    InsertOrUpdateBRT(laDemande.Branchement, laCommande);


                    CsClient client = Select_client(laDemande.LaDemande.FK_IDCENTRE, laDemande.LaDemande.CENTRE, laDemande.LaDemande.CLIENT, laDemande.LaDemande.ORDRE, laConnection);
                    laDemande.LeClient.PK_ID = client.PK_ID;

                    /*CsClient dclient = Select_Dclient(laDemande.LaDemande.NUMDEM, laDemande.LaDemande.PK_ID);

                    if (dclient != null)
                        laDemande.LeClient = dclient;
                    laDemande.LeClient.PK_ID = client.PK_ID;

                    laDemande.LeClient.FK_IDAG = laDemande.Ag.PK_ID; ;
                    InsertOrUpdateCLIENT(laDemande.LeClient, laCommande);
                    */

                    /**Résilier l'ancien produit**/

                    
                    // ZEG 03/01/2021 CsAbon abon = Select_Abon(laDemande.LaDemande.FK_IDCENTRE, laDemande.LaDemande.CENTRE, laDemande.LaDemande.CLIENT, laDemande.LaDemande.ORDRE, laDemande.LaDemande.PRODUIT, laConnection);
                    CsAbon abon = Select_Abon(laDemande.LaDemande.FK_IDCENTRE, laDemande.LaDemande.CENTRE, laDemande.LaDemande.CLIENT, laDemande.LaDemande.ORDRE, null /*ZEG 03/01/2021*/, laConnection);
                    abon.DRES = System.DateTime.Today;
                    abon.DATEMODIFICATION = System.DateTime.Today;
                    abon.USERMODIFICATION = laDemande.LaDemande.USERMODIFICATION;
                    UpdateABON(abon, laCommande);

                    if (abon.AVANCE != null && abon.AVANCE > 0)
                        DescendreAvance(laDemande.LaDemande, laCommande);

                    // ZEG 03/01/2021
                    abon.AVANCE = null;
                    abon.DAVANCE = null;
                    UpdateABON(abon, laCommande);
                    //

                    List<CsCanalisation> lcanal = Select_CannalisationCompteur(laDemande.LaDemande.FK_IDCENTRE, laDemande.LaDemande.CENTRE, laDemande.LaDemande.CLIENT, laDemande.LaDemande.PRODUIT, laConnection);
                    List<CsCanalisation> lcanal2 = lcanal.Where(t => t.DEPOSE == null).ToList();
                    if (lcanal2 != null && lcanal2.Count > 0)
                    {
                        CsCanalisation canal = lcanal2.First();
                        canal.DEPOSE = System.DateTime.Today;
                        canal.DATEMODIFICATION = System.DateTime.Today;
                        canal.USERMODIFICATION = laDemande.LaDemande.USERMODIFICATION;
                        InsertOrUpdateCANALISATION(canal, laCommande);
                    }

                    /****/


                    CsAbon dabon = Select_DAbon(laDemande.LaDemande.NUMDEM, laDemande.LaDemande.PK_ID, laConnection);

                    if (dabon != null)
                        laDemande.Abonne = dabon;


                    /** ZEG 03/01/2021
                    if (laDemande.LstCoutDemande != null && laDemande.LstCoutDemande.FirstOrDefault(i => i.COPER == Enumere.CoperCAU) != null)
                    {
                        CsDemandeDetailCout leAvance = laDemande.LstCoutDemande.FirstOrDefault(i => i.COPER == Enumere.CoperCAU);
                        laDemande.Abonne.AVANCE = (leAvance.MONTANTHT != null ? leAvance.MONTANTHT : 0) + (leAvance.MONTANTTAXE != null ? leAvance.MONTANTTAXE : 0);
                        laDemande.Abonne.DAVANCE = laDemande.LaDemande.DCAISSE;
                    }
                     * */
                    laDemande.LstCoutDemande = Select_FactureDemande(laDemande.LaDemande.NUMDEM, laDemande.LaDemande.PK_ID, laConnection);
                    if (laDemande.LstCoutDemande.FirstOrDefault(a => a.COPER == Enumere.CoperCAU) != null)
                    {
                        laDemande.Abonne.AVANCE = laDemande.LstCoutDemande.FirstOrDefault(a => a.COPER == Enumere.CoperCAU).MONTANTHT + laDemande.LstCoutDemande.FirstOrDefault(a => a.COPER == Enumere.CoperCAU).MONTANTTAXE;
                        laDemande.Abonne.DAVANCE = laDemande.LstCoutDemande.FirstOrDefault().DATECAISSE;
                    }
                    else
                    {
                        laDemande.Abonne.AVANCE = null;
                        laDemande.Abonne.DAVANCE = null;
                    }
                    /** ZEG **/
                    
                    
                    laDemande.Abonne.FK_IDCLIENT = laDemande.LeClient.PK_ID;
                    InsertOrUpdateABON(laDemande.Abonne, laCommande);
                    InsertOrUpdateCOMPTECLIENTSUITEPV(laDemande.LaDemande, laCommande);
                    if (laDemande.LstOrganeScelleDemande != null && laDemande.LstOrganeScelleDemande.Count != 0)
                        foreach (CsOrganeScelleDemande item in laDemande.LstOrganeScelleDemande)
                        {
                            item.FK_IDDEMANDE = laDemande.LaDemande.PK_ID;
                            item.FK_IDBRT = laDemande.Branchement.PK_ID;
                            item.USERCREATION = laDemande.LaDemande.MATRICULE;
                            InsertOrUpdatePOSESCELLE(item, laCommande);
                            Updatescelle(item, laCommande);
                        }

                    if (laDemande.LstCanalistion != null && laDemande.LstCanalistion.Count !=0)
                        foreach (CsCanalisation item in laDemande.LstCanalistion)
                        {
                            #region Compteur
                            CsCompteur can = new CsCompteur();
                            can.ANNEEFAB = item.ANNEEFAB;
                            can.CADRAN = item.CADRAN;
                            can.COEFCOMPTAGE = item.COEFCOMPTAGE;
                            can.COEFLECT = item.COEFLECT;
                            can.MISEENSERVICE = System.DateTime.Today.Date;
                            can.FK_IDCALIBRECOMPTEUR = item.FK_IDCALIBRE;
                            can.FK_IDMARQUECOMPTEUR = item.FK_IDMARQUECOMPTEUR.Value;
                            can.FK_IDPRODUIT = item.FK_IDPRODUIT;
                            can.FK_IDTYPECOMPTEUR = item.FK_IDTYPECOMPTEUR.Value;
                            can.FK_IDSTATUTCOMPTEUR = 1;
                            can.FK_IDETATCOMPTEUR = 1;
                            can.FONCTIONNEMENT = item.FONCTIONNEMENT;
                            can.MARQUE = item.MARQUE;
                            can.NUMERO = item.NUMERO;
                            can.TYPECOMPTEUR = item.TYPECOMPTEUR;
                            can.PLOMBAGE = item.PLOMBAGE;
                            can.CODEPRODUIT = item.PRODUIT;
                            can.USERCREATION = item.USERCREATION;
                            can.USERMODIFICATION = item.USERMODIFICATION;
                            int IdCompteur = InsertOrUpdateCOMPTEUR(can, laCommande);

                            item.FK_IDABON = laDemande.Abonne.PK_ID;
                            item.FK_IDCOMPTEUR = IdCompteur;
                            #endregion
                            InsertOrUpdateCANALISATION(item, laCommande);

                            CsEvenement leEvt = laDemande.LstEvenement.FirstOrDefault(y => y.POINT == item.POINT);
                            leEvt.FK_IDCANALISATION = item.PK_ID;
                            leEvt.FK_IDABON = laDemande.Abonne.PK_ID;
                            leEvt.FK_IDCOMPTEUR = IdCompteur;
                            leEvt.DATEEVT = item.POSE;
                            leEvt.STATUS = Enumere.EvenementPurger;
                            InsertEVENEMENT(leEvt, laCommande);
                            //InsertOrUpdateEVENEMENT(leEvt, laCommande);
                        }
                }
                #endregion
                #region AbonnementSeul

                if (laDemande.LaDemande.TYPEDEMANDE == Enumere.AbonnementSeul)
                {

                    CsBrt brt = Select_brt(laDemande.LaDemande.FK_IDCENTRE, laDemande.LaDemande.CENTRE, laDemande.LaDemande.CLIENT, laConnection);
                    CsBrt dbrt = Select_Dbrt(laDemande.LaDemande.NUMDEM, laDemande.LaDemande.PK_ID, laConnection);

                    if (dbrt != null)
                        laDemande.Branchement = dbrt;
                    laDemande.Branchement.PK_ID = brt.PK_ID;


                    if (laDemande.LaDemande.ISMETREAFAIRE == false)
                    {
                        MiseAJourAbonnementSeulSansMetre(laDemande, laCommande);
                        //UpdateWKF(laDemande.LaDemande.NUMDEM, laDemande.LaDemande.PK_ID, 0, laDemande.LaDemande.MATRICULE, "ClotureDemande", laCommande);
                        //laCommande.Transaction.Commit();
                        //return "";
                    }
                    else
                    {
                        laDemande.LaDemande.DATEFIN = System.DateTime.Today;
                        InsertOrUpdateDemande(laDemande.LaDemande, laCommande);
                        CsAg leAg = Select_Ag(laDemande.LaDemande.FK_IDCENTRE, laDemande.LaDemande.CENTRE, laDemande.LaDemande.CLIENT, laConnection);
                        laDemande.LeClient.FK_IDAG = leAg.PK_ID; ;
                        InsertOrUpdateCLIENT(laDemande.LeClient, laCommande);

                        if (laDemande.LstCoutDemande != null && laDemande.LstCoutDemande.FirstOrDefault(i => i.COPER == Enumere.CoperCAU) != null)
                        {
                            CsDemandeDetailCout leAvance = laDemande.LstCoutDemande.FirstOrDefault(i => i.COPER == Enumere.CoperCAU);
                            laDemande.Abonne.AVANCE = (leAvance.MONTANTHT != null ? leAvance.MONTANTHT : 0) + (leAvance.MONTANTTAXE != null ? leAvance.MONTANTTAXE : 0);
                            laDemande.Abonne.DAVANCE = laDemande.LaDemande.DCAISSE;
                        }
                        laDemande.Abonne.FK_IDCLIENT = laDemande.LeClient.PK_ID;
                        InsertOrUpdateABON(laDemande.Abonne, laCommande);
                        InsertOrUpdateCOMPTECLIENTSUITEPV(laDemande.LaDemande, laCommande);
                        if (laDemande.LstOrganeScelleDemande != null)
                            foreach (CsOrganeScelleDemande item in laDemande.LstOrganeScelleDemande)
                            {
                                item.FK_IDDEMANDE = laDemande.LaDemande.PK_ID;
                                item.FK_IDBRT = laDemande.Branchement.PK_ID;
                                item.USERCREATION = laDemande.LaDemande.MATRICULE;
                                InsertOrUpdatePOSESCELLE(item, laCommande);
                                Updatescelle(item, laCommande);
                            }

                        if (laDemande.LstCanalistion != null)
                            foreach (CsCanalisation item in laDemande.LstCanalistion)
                            {
                                #region Compteur
                                CsCompteur can = new CsCompteur();
                                can.ANNEEFAB = item.ANNEEFAB;
                                can.CADRAN = item.CADRAN;
                                can.COEFCOMPTAGE = item.COEFCOMPTAGE;
                                can.COEFLECT = item.COEFLECT;
                                can.MISEENSERVICE = System.DateTime.Today.Date;
                                can.FK_IDCALIBRECOMPTEUR = item.FK_IDCALIBRE;
                                can.FK_IDMARQUECOMPTEUR = item.FK_IDMARQUECOMPTEUR.Value;
                                can.FK_IDPRODUIT = item.FK_IDPRODUIT;
                                can.FK_IDTYPECOMPTEUR = item.FK_IDTYPECOMPTEUR.Value;
                                can.FK_IDSTATUTCOMPTEUR = 1;
                                can.FK_IDETATCOMPTEUR = 1;
                                can.FONCTIONNEMENT = item.FONCTIONNEMENT;
                                can.MARQUE = item.MARQUE;
                                can.NUMERO = item.NUMERO;
                                can.TYPECOMPTEUR = item.TYPECOMPTEUR;
                                can.PLOMBAGE = item.PLOMBAGE;
                                can.CODEPRODUIT = item.PRODUIT;
                                can.USERCREATION = item.USERCREATION;
                                can.USERMODIFICATION = item.USERMODIFICATION;
                                int IdCompteur = InsertOrUpdateCOMPTEUR(can, laCommande);

                                item.FK_IDABON = laDemande.Abonne.PK_ID;
                                item.FK_IDCOMPTEUR = IdCompteur;
                                #endregion
                                InsertOrUpdateCANALISATION(item, laCommande);

                                CsEvenement leEvt = laDemande.LstEvenement.FirstOrDefault(y => y.POINT == item.POINT);
                                leEvt.FK_IDCANALISATION = item.PK_ID;
                                leEvt.FK_IDABON = laDemande.Abonne.PK_ID;
                                leEvt.FK_IDCOMPTEUR = IdCompteur;
                                leEvt.STATUS = Enumere.EvenementPurger;
                                InsertOrUpdateEVENEMENT(leEvt, laCommande);
                            }
                    }
                }
                #endregion
                #region Augmentation & diminution de puissance
                if (laDemande.LaDemande.TYPEDEMANDE == Enumere.AugmentationPuissance ||
                    laDemande.LaDemande.TYPEDEMANDE == Enumere.DimunitionPuissance)
                {

                    CsBrt brt = Select_brt(laDemande.LaDemande.FK_IDCENTRE, laDemande.LaDemande.CENTRE, laDemande.LaDemande.CLIENT,laConnection);
                    CsBrt dbrt = Select_Dbrt(laDemande.LaDemande.NUMDEM, laDemande.LaDemande.PK_ID, laConnection);

                    if (dbrt != null)
                        laDemande.Branchement = dbrt;
                    laDemande.Branchement.PK_ID = brt.PK_ID;


                    CsAbon abon = Select_Abon(laDemande.LaDemande.FK_IDCENTRE, laDemande.LaDemande.CENTRE, laDemande.LaDemande.CLIENT, laDemande.LaDemande.ORDRE, laDemande.LaDemande.PRODUIT, laConnection);
                    CsAbon dabon = Select_DAbon(laDemande.LaDemande.NUMDEM, laDemande.LaDemande.PK_ID, laConnection);

                    if (dabon != null)
                        laDemande.Abonne = dabon;
                    laDemande.Abonne.PK_ID = abon.PK_ID;

                    DescendreAvance(laDemande.LaDemande, laCommande);
                    if (laDemande.LaDemande.ISMETREAFAIRE == false)
                    {
                        if (laDemande.LstCoutDemande != null && laDemande.LstCoutDemande.FirstOrDefault(i => i.COPER == Enumere.CoperCAU) != null)
                        {
                            CsDemandeDetailCout leAvance = laDemande.LstCoutDemande.FirstOrDefault(i => i.COPER == Enumere.CoperCAU);
                            laDemande.Abonne.AVANCE = (leAvance.MONTANTHT != null ? leAvance.MONTANTHT : 0) + (leAvance.MONTANTTAXE != null ? leAvance.MONTANTTAXE : 0);
                            laDemande.Abonne.DAVANCE = laDemande.LaDemande.DCAISSE;
                        }
                        InsertOrUpdateABON(laDemande.Abonne, laCommande);
                        if (laDemande.Abonne.PRODUIT != Enumere.ElectriciteMT)
                        {
                            /*
                            List<CsCanalisation> lstcanalisation = new List<CsCanalisation>();

                            if (laDemande.LstCanalistion != null && laDemande.LstCanalistion.Count != 0)
                            {
                                lstcanalisation = laDemande.LstCanalistion.Where(o => o.DEPOSE == null).ToList();
                                lstcanalisation.ForEach(y => y.REGLAGECOMPTEUR = laDemande.LaDemande.REGLAGECOMPTEUR);
                                lstcanalisation.ForEach(y => y.FK_IDREGLAGECOMPTEUR = laDemande.LaDemande.FK_IDREGLAGECOMPTEUR);
                            }
                            else
                            {

                                lstcanalisation = Select_CannalisationCompteur(laDemande.LaDemande.FK_IDCENTRE, laDemande.LaDemande.CENTRE, laDemande.LaDemande.CLIENT);
                                lstcanalisation = lstcanalisation.Where(o => o.DEPOSE == null).ToList();
                                lstcanalisation.ForEach(y => y.REGLAGECOMPTEUR = laDemande.LaDemande.REGLAGECOMPTEUR);
                                lstcanalisation.ForEach(y => y.FK_IDREGLAGECOMPTEUR = laDemande.LaDemande.FK_IDREGLAGECOMPTEUR);

                            }
                            foreach (CsCanalisation item in lstcanalisation)
                            {
                                if (abon != null && abon.PK_ID > 0)
                                    item.FK_IDABON = abon.PK_ID;
                                InsertOrUpdateCANALISATION(item, laCommande);
                            }*/

                            UpdateCanalisation(abon.PK_ID, laDemande.LaDemande.FK_IDREGLAGECOMPTEUR, laDemande.LaDemande.REGLAGECOMPTEUR, laCommande);

                        }
                        //UpdateWKF(laDemande.LaDemande.NUMDEM, laDemande.LaDemande.PK_ID, 0, laDemande.LaDemande.MATRICULE, "ClotureDemande", laCommande);
                        //laCommande.Transaction.Commit();
                        //return "";
                    }
                    else
                    {
                        laDemande.LaDemande.DATEFIN = System.DateTime.Today;
                        InsertOrUpdateDemande(laDemande.LaDemande, laCommande);
                        if (laDemande.LstCoutDemande != null && laDemande.LstCoutDemande.FirstOrDefault(i => i.COPER == Enumere.CoperCAU) != null)
                        {
                            CsDemandeDetailCout leAvance = laDemande.LstCoutDemande.FirstOrDefault(i => i.COPER == Enumere.CoperCAU);
                            laDemande.Abonne.AVANCE = (leAvance.MONTANTHT != null ? leAvance.MONTANTHT : 0) + (leAvance.MONTANTTAXE != null ? leAvance.MONTANTTAXE : 0);
                            laDemande.Abonne.DAVANCE = laDemande.LaDemande.DCAISSE;
                        }
                        InsertOrUpdateABON(laDemande.Abonne, laCommande);
                        InsertOrUpdateCOMPTECLIENTSUITEPV(laDemande.LaDemande, laCommande);
                        if (laDemande.LstOrganeScelleDemande != null)
                            foreach (CsOrganeScelleDemande item in laDemande.LstOrganeScelleDemande)
                            {
                                item.FK_IDDEMANDE = laDemande.LaDemande.PK_ID;
                                item.FK_IDBRT = laDemande.Branchement.PK_ID;
                                item.USERCREATION = laDemande.LaDemande.MATRICULE;
                                InsertOrUpdatePOSESCELLE(item, laCommande);
                                Updatescelle(item, laCommande);
                            }


                        if (laDemande.LstCanalistion != null)
                            foreach (CsCanalisation item in laDemande.LstCanalistion)
                            {

                                #region Compteur
                                CsCompteur can = new CsCompteur();
                                can.ANNEEFAB = item.ANNEEFAB;
                                can.CADRAN = item.CADRAN;
                                can.COEFCOMPTAGE = item.COEFCOMPTAGE;
                                can.COEFLECT = item.COEFLECT;
                                can.MISEENSERVICE = System.DateTime.Today.Date;
                                can.FK_IDCALIBRECOMPTEUR = item.FK_IDCALIBRE;
                                can.FK_IDMARQUECOMPTEUR = item.FK_IDMARQUECOMPTEUR.Value;
                                can.FK_IDPRODUIT = item.FK_IDPRODUIT;
                                can.FK_IDTYPECOMPTEUR = item.FK_IDTYPECOMPTEUR.Value;
                                can.FK_IDSTATUTCOMPTEUR = 1;
                                can.FK_IDETATCOMPTEUR = 1;
                                can.FONCTIONNEMENT = item.FONCTIONNEMENT;
                                can.MARQUE = item.MARQUE;
                                can.NUMERO = item.NUMERO;
                                can.TYPECOMPTEUR = item.TYPECOMPTEUR;
                                can.PLOMBAGE = item.PLOMBAGE;
                                can.CODEPRODUIT = item.PRODUIT;
                                can.USERCREATION = item.USERCREATION;
                                can.USERMODIFICATION = item.USERMODIFICATION;

                                int IdCompteur = InsertOrUpdateCOMPTEUR(can, laCommande);

                                item.FK_IDABON = laDemande.Abonne.PK_ID;
                                item.FK_IDCOMPTEUR = IdCompteur;
                                #endregion

                                InsertOrUpdateCANALISATION(item, laCommande);

                                CsEvenement leEvt = laDemande.LstEvenement.FirstOrDefault(y => y.POINT == item.POINT && y.CAS == Enumere.CasPoseCompteur);
                                //leEvt.FK_IDABON = laDemande.Abonne.PK_ID;
                                leEvt.FK_IDABON = item.FK_IDABON;
                                leEvt.FK_IDCOMPTEUR = IdCompteur;
                                leEvt.STATUS = Enumere.EvenementPurger;

                                InsertOrUpdateEVENEMENT(leEvt, laCommande);
                            }

                        #region Creation de lot isole
                        if (laDemande.LstEvenement != null && laDemande.LstEvenement.Count != 0)
                        {
                            List<CsEvenement> lst = new List<CsEvenement>();
                            lst = laDemande.LstEvenement.Where(t => t.CAS == Enumere.CasDeposeCompteur).ToList();
                            if (lst != null && lst.Count != 0)
                            {
                                CsLotri NewLotri = new CsLotri()
                                {
                                    BASE = "S",
                                    CATEGORIECLIENT = "99",
                                    CENTRE = lst.First().CENTRE,
                                    FK_IDCENTRE = lst.First().FK_IDCENTRE,
                                    DATECREATION = DateTime.Now,
                                    FK_IDCATEGORIECLIENT = null,
                                    FK_IDPRODUIT = lst.First().FK_IDPRODUIT,
                                    FK_IDRELEVEUR = null,
                                    NUMLOTRI = lst.First().CENTRE + "00004",
                                    PERIODE = DateTime.Now.Year + (DateTime.Now.Month.ToString().Length > 1 ? DateTime.Now.Month.ToString() : "0" + DateTime.Now.Month.ToString()),
                                    PERIODICITE = "01",
                                    PRODUIT = lst.First().PRODUIT,
                                    RELEVEUR = "99",
                                    TOURNEE = "000",
                                    USERCREATION = lst.First().USERCREATION,
                                    UserCalcul = lst.First().USERCREATION
                                };
                                foreach (CsEvenement item in lst)
                                {
                                    InsertOrUpdateEVENEMENT(item, laCommande);
                                }
                                InsertOrUpdateLotri(NewLotri, laCommande);
                                //CreationCtarCompt(laDemande.Abonne.PK_ID, NewLotri, NewLotri.PERIODE, NewLotri.USERCREATION, NewLotri.FK_IDCENTRE, int.Parse(NewLotri.FK_IDPRODUIT.ToString()), DateTime.Parse(lst.First().DATEEVT.ToString()), laCommande);
                            }
                        }
                        #endregion
                         
                    }

                }
                #endregion
                #region ChangementCompteur
                if (laDemande.LaDemande.TYPEDEMANDE == Enumere.ChangementCompteur)
                {
                    CsBrt brt = Select_brt(laDemande.LaDemande.FK_IDCENTRE, laDemande.LaDemande.CENTRE, laDemande.LaDemande.CLIENT, laConnection);
                    CsBrt dbrt = Select_Dbrt(laDemande.LaDemande.NUMDEM, laDemande.LaDemande.PK_ID, laConnection);

                    if (dbrt != null)
                        laDemande.Branchement = dbrt;
                    laDemande.Branchement.PK_ID = brt.PK_ID;

                    CsAbon abon = Select_Abon(laDemande.LaDemande.FK_IDCENTRE, laDemande.LaDemande.CENTRE, laDemande.LaDemande.CLIENT, laDemande.LaDemande.ORDRE, laDemande.LaDemande.PRODUIT, laConnection);
                    CsAbon dabon = Select_DAbon(laDemande.LaDemande.NUMDEM, laDemande.LaDemande.PK_ID, laConnection);

                    if (dabon != null)
                        laDemande.Abonne = dabon;
                    laDemande.Abonne.PK_ID = abon.PK_ID;

                    laDemande.LaDemande.DATEFIN = System.DateTime.Today;
                    InsertOrUpdateDemande(laDemande.LaDemande, laCommande);
                    if (laDemande.LstOrganeScelleDemande != null)
                        foreach (CsOrganeScelleDemande item in laDemande.LstOrganeScelleDemande)
                        {
                            item.FK_IDDEMANDE = laDemande.LaDemande.PK_ID;
                            item.FK_IDBRT = laDemande.Branchement.PK_ID;
                            item.USERCREATION = laDemande.LaDemande.MATRICULE;
                            InsertOrUpdatePOSESCELLE(item, laCommande);
                            Updatescelle(item, laCommande);
                        }

                    if (laDemande.LstCanalistion != null)
                        foreach (CsCanalisation item in laDemande.LstCanalistion)
                        {
                            #region Compteur
                            CsCompteur can = new CsCompteur();
                            can.ANNEEFAB = item.ANNEEFAB;
                            can.CADRAN = item.CADRAN;
                            can.COEFCOMPTAGE = item.COEFCOMPTAGE;
                            can.COEFLECT = item.COEFLECT;
                            can.MISEENSERVICE = System.DateTime.Today.Date;
                            can.FK_IDCALIBRECOMPTEUR = item.FK_IDCALIBRE;
                            can.FK_IDMARQUECOMPTEUR = item.FK_IDMARQUECOMPTEUR.Value;
                            can.FK_IDPRODUIT = item.FK_IDPRODUIT;
                            can.FK_IDTYPECOMPTEUR = item.FK_IDTYPECOMPTEUR.Value;
                            can.FK_IDSTATUTCOMPTEUR = 1;
                            can.FK_IDETATCOMPTEUR = 1;
                            can.FONCTIONNEMENT = item.FONCTIONNEMENT;
                            can.MARQUE = item.MARQUE;
                            can.NUMERO = item.NUMERO;
                            can.TYPECOMPTEUR = item.TYPECOMPTEUR;
                            can.PLOMBAGE = item.PLOMBAGE;
                            can.CODEPRODUIT = item.PRODUIT;
                            can.USERCREATION = item.USERCREATION;
                            can.USERMODIFICATION = item.USERMODIFICATION;
                            int IdCompteur = InsertOrUpdateCOMPTEUR(can, laCommande);

                            item.FK_IDABON = laDemande.Abonne.PK_ID;
                            item.FK_IDCOMPTEUR = IdCompteur;
                            #endregion
                            InsertOrUpdateCANALISATION(item, laCommande);

                            CsEvenement leEvt = laDemande.LstEvenement.FirstOrDefault(y => y.POINT == item.POINT);
                            leEvt.FK_IDCANALISATION = item.PK_ID;
                            leEvt.FK_IDABON = laDemande.Abonne.PK_ID;
                            leEvt.FK_IDCOMPTEUR = IdCompteur;
                            leEvt.STATUS = Enumere.EvenementPurger;
                            InsertOrUpdateEVENEMENT(leEvt, laCommande);
                        }
                }
                #endregion
                #region ObjetScanne
                EcrirePieceJointe(laDemande, laCommande);
                #endregion
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
                if (laConnection.State == ConnectionState.Open)
                    laConnection.Close(); // Fermeture de la connection 
                laConnection.Dispose();

                if (laCommande.Connection.State == ConnectionState.Open)
                    laCommande.Connection.Close(); // Fermeture de la connection 
                laCommande.Dispose();
            }
        }

        public void DescendreAvance(CsDemandeBase lademande, SqlCommand cmds)
        {
            cmds.CommandTimeout = 1800;
            cmds.CommandType = CommandType.StoredProcedure;
            cmds.CommandText = "SPX_ACC_DESCENDREAVANCE";
            if (cmds.Parameters != null && cmds.Parameters.Count != 0) cmds.Parameters.Clear();

            cmds.Parameters.Add("@NUMDEM", SqlDbType.VarChar, 13).Value = lademande.NUMDEM;
            cmds.Parameters.Add("@FK_IDDEMANDE", SqlDbType.Int).Value = lademande.PK_ID;
            cmds.Parameters.Add("@MATRICULE", SqlDbType.VarChar, 6).Value = lademande.USERCREATION;
            DBBase.SetDBNullParametre(cmds.Parameters);

            try
            {
                if (cmds.Connection.State == ConnectionState.Closed)
                    cmds.Connection.Open();
                cmds.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception(cmds.CommandText + ":" + ex.Message);
            }
        }

        public string AnnulationDemande(CsDemandeBase _LaDemandeMiseAJoure)
        {
            SqlCommand laCommande = DBBase.InitTransaction(ConnectionString);
            try
            {
                AnnulationDemandeSpx(_LaDemandeMiseAJoure.PK_ID, _LaDemandeMiseAJoure.NUMDEM, _LaDemandeMiseAJoure.USERSUPPRESSION, laCommande);
                laCommande.Transaction.Commit();
                return "";
            }
            catch (Exception ex)
            {
                laCommande.Transaction.Rollback();
                return null;
            }
            finally
            {
                if (laCommande.Connection.State == ConnectionState.Open)
                    laCommande.Connection.Close();
                laCommande.Dispose();
            }
        }

/*
        public string CreeDemandeExtension(CsDemandeBase _LaDemandeMiseAJoure)
        {
            SqlCommand laCommande = DBBase.InitTransaction(ConnectionString);
            try
            {
                CreeDemandeExtentionSpx(_LaDemandeMiseAJoure.PK_ID, _LaDemandeMiseAJoure.NUMDEM, laCommande);
                laCommande.Transaction.Commit();
                return "";
            }
            catch (Exception ex)
            {
                laCommande.Transaction.Rollback();
                return null;
            }
            finally
            {
                if (laCommande.Connection.State == ConnectionState.Open)
                    laCommande.Connection.Close();
                laCommande.Dispose();
            }
        }
*/

        public void CreeDemandeExtentionSpx(int IDDEMANDE, string NUMDEM, SqlCommand cmds)
        {
            cmds.CommandTimeout = 1800;
            cmds.CommandType = CommandType.StoredProcedure;
            cmds.CommandText = "SPX_WKF_CREEDEMANDEEXTENSION";
            if (cmds.Parameters != null && cmds.Parameters.Count != 0) cmds.Parameters.Clear();
            cmds.Parameters.Add("@FK_IDDEMANDE", SqlDbType.Int).Value = IDDEMANDE;
            cmds.Parameters.Add("@NUMDEM", SqlDbType.VarChar, 20).Value = NUMDEM;

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

        public void AnnulationDemandeSpx(int IDDEMANDE, string NUMDEM, string MATRICULE, SqlCommand cmds)
        {
            cmds.CommandTimeout = 1800;
            cmds.CommandType = CommandType.StoredProcedure;
            cmds.CommandText = "SPX_ACC_ANNULATIONDEMANDE";
            if (cmds.Parameters != null && cmds.Parameters.Count != 0) cmds.Parameters.Clear();
            cmds.Parameters.Add("@FK_IDDEMANDE", SqlDbType.Int).Value = IDDEMANDE;
            cmds.Parameters.Add("@NUMDEM", SqlDbType.VarChar, 20).Value = NUMDEM;
            cmds.Parameters.Add("@MATRICULE", SqlDbType.VarChar, 20).Value = MATRICULE;

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

        public void InsertOrUpdateAppareil(ObjAPPAREILSDEVIS leAppareil, SqlCommand cmds)
        {
            cmds.CommandTimeout = 1800;
            cmds.CommandType = CommandType.StoredProcedure;
            cmds.CommandText = "SPX_ACC_INSERTORUPDATE_APPAREILDEVIS";
            if (cmds.Parameters != null && cmds.Parameters.Count != 0) cmds.Parameters.Clear();

            cmds.Parameters.Add("@NUMDEM", SqlDbType.VarChar, 14).Value = leAppareil.NUMDEVIS;
            cmds.Parameters.Add("@CODEAPPAREIL", SqlDbType.VarChar, 20).Value = leAppareil.CODEAPPAREIL;
            cmds.Parameters.Add("@NBRE", SqlDbType.Int).Value = leAppareil.NBRE;
            cmds.Parameters.Add("@PUISSANCE", SqlDbType.Int).Value = leAppareil.PUISSANCE;
            cmds.Parameters.Add("@USERCREATION", SqlDbType.VarChar, 6).Value = leAppareil.USERCREATION;
            cmds.Parameters.Add("@USERMODIFICATION", SqlDbType.VarChar, 6).Value = leAppareil.USERMODIFICATION;
            cmds.Parameters.Add("@FK_IDDEMANDE", SqlDbType.Int).Value = leAppareil.FK_IDDEVIS;
            cmds.Parameters.Add("@FK_IDCODEAPPAREIL", SqlDbType.Int).Value = leAppareil.FK_IDCODEAPPAREIL;
            DBBase.SetDBNullParametre(cmds.Parameters);
            try
            {
                if (cmds.Connection.State == ConnectionState.Closed)
                    cmds.Connection.Open();
                cmds.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception(cmds.CommandText + ":" + ex.Message);
            }
        }

        public void Delete_RubriqueDemande(string Numdem, int Iddemande, SqlCommand cmds)
        {
            cmds.CommandTimeout = 1800;
            cmds.CommandType = CommandType.StoredProcedure;
            cmds.CommandText = "SPX_ACC_DELETE_RUBRIQUEDEMANDE";
            if (cmds.Parameters != null && cmds.Parameters.Count != 0) cmds.Parameters.Clear();

            cmds.Parameters.Add("@NUMDEM", SqlDbType.VarChar, 20).Value = Numdem;
            cmds.Parameters.Add("@IDDEMANDE", SqlDbType.Int).Value = Iddemande;
            DBBase.SetDBNullParametre(cmds.Parameters);
            try
            {
                if (cmds.Connection.State == ConnectionState.Closed)
                    cmds.Connection.Open();
                cmds.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception(cmds.CommandText + ":" + ex.Message);
            }
        }

        //public bool ClotureValiderDemande(CsDemande LaDemande)
        //{

        //    try
        //    {
        //        bool Returnevalue = false;
        //        SqlCommand laCommande = DBBase.InitTransaction(ConnectionString);
        //        if (LaDemande.LaDemande.TYPEDEMANDE == Enumere.Resiliation)
        //        {
        //            this.ValiderResiliation(LaDemande.LaDemande.NUMDEM, LaDemande.LaDemande.PK_ID, LaDemande.LaDemande.USERCREATION, laCommande);
        //            TransmettreDemande(LaDemande.LaDemande.NUMDEM, LaDemande.InfoDemande.FK_IDETAPEACTUELLE, LaDemande.LaDemande.USERCREATION, laCommande);
        //            laCommande.Transaction.Commit();
        //            Returnevalue = true;
        //        }
        //        else if (LaDemande.LaDemande.TYPEDEMANDE == Enumere.ModificationClient ||
        //            LaDemande.LaDemande.TYPEDEMANDE == Enumere.ModificationAbonnement ||
        //            LaDemande.LaDemande.TYPEDEMANDE == Enumere.ModificationAdresse ||
        //            LaDemande.LaDemande.TYPEDEMANDE == Enumere.ModificationCompteur ||
        //            LaDemande.LaDemande.TYPEDEMANDE == Enumere.ModificationBranchement)
        //        {
        //            ValiderMoficationClient(LaDemande, true);
        //            Returnevalue = true;

        //        }

        //        return Returnevalue;

        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public string ValiderMoficationClient(CsDemande laDemande, bool AvecTransmission)
        {
            SqlCommand laCommande = DBBase.InitTransaction(ConnectionString);
            try
            {
                #region Ag
                if (laDemande.Ag != null && !string.IsNullOrEmpty(laDemande.Ag.CENTRE))
                    InsertOrUpdateAG(laDemande.Ag, laCommande);
                #endregion

                #region Brt
                if (laDemande.Branchement != null && !string.IsNullOrEmpty(laDemande.Branchement.CENTRE))
                    InsertOrUpdateBRT(laDemande.Branchement, laCommande);
                #endregion

                #region Client
                if (laDemande.LeClient != null && !string.IsNullOrEmpty(laDemande.LeClient.CENTRE))
                    InsertOrUpdateCLIENT(laDemande.LeClient, laCommande);
                #endregion
                #region ABON
                if (laDemande.Abonne != null && !string.IsNullOrEmpty(laDemande.Abonne.CENTRE))
                    InsertOrUpdateABON(laDemande.Abonne, laCommande);
                #endregion

                #region Compteur

                if (laDemande.LaDemande.TYPEDEMANDE == Enumere.ModificationCompteur)
                    ValiderModificationCompteur(laDemande, laCommande);
                #endregion
                #region ObjetScanne
                EcrirePieceJointe(laDemande, laCommande);
                #endregion
                if (AvecTransmission)
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

        public void ValiderResiliation(string numdem, int iddemande, string matricule, SqlCommand cmds)
        {
            cmds.CommandTimeout = 3000;
            cmds.CommandType = CommandType.StoredProcedure;
            cmds.CommandText = "SPX_ACC_VALIDER_RESILIATION";
            if (cmds.Parameters != null && cmds.Parameters.Count != 0) cmds.Parameters.Clear();

            cmds.Parameters.Add("@NUMDEM", SqlDbType.VarChar, 13).Value = numdem;
            cmds.Parameters.Add("@FK_IDDEMANDE", SqlDbType.Int).Value = iddemande;
            cmds.Parameters.Add("@MATRICULE", SqlDbType.VarChar, 6).Value = matricule;

            DBBase.SetDBNullParametre(cmds.Parameters);

            try
            {
                if (cmds.Connection.State == ConnectionState.Closed)
                    cmds.Connection.Open();
                cmds.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception(cmds.CommandText + ":" + ex.Message);
            }
        }

        public bool ValiderCreationFactureIsole(List<CsEvenement> lstEvenement)
        {
            SqlCommand laCommande = DBBase.InitTransaction(ConnectionString);
            try
            {
                CsLotri NewLotri = new CsLotri()
                {
                    BASE = "S",
                    CATEGORIECLIENT = "99",
                    CENTRE = lstEvenement.First().CENTRE,
                    FK_IDCENTRE = lstEvenement.First().FK_IDCENTRE,
                    DATECREATION = DateTime.Now,
                    FK_IDCATEGORIECLIENT = null,
                    FK_IDPRODUIT = lstEvenement.First().FK_IDPRODUIT,
                    FK_IDRELEVEUR = null,
                    NUMLOTRI = lstEvenement.First().LOTRI,
                    PERIODE = DateTime.Now.Year + (DateTime.Now.Month.ToString().Length > 1 ? DateTime.Now.Month.ToString() : "0" + DateTime.Now.Month.ToString()),
                    PERIODICITE = "01",
                    PRODUIT = lstEvenement.First().PRODUIT,
                    RELEVEUR = "99",
                    TOURNEE = "000",
                    USERCREATION = lstEvenement.First().USERCREATION,
                    UserCalcul = lstEvenement.First().USERCREATION
                };

                laCommande = new SqlCommand();
                laCommande = DBBase.InitTransaction(ConnectionString);

                foreach (CsEvenement item in lstEvenement)
                {

                    if (lstEvenement.First().IDETAPE != null)
                        InsertOrUpdateDEvenement(item, laCommande);

                    InsertOrUpdateEVENEMENT(item, laCommande);
                    CreationCtarCompt(item.FK_IDABON.Value, item.LOTRI, item.PERIODE, item.USERCREATION, item.FK_IDCENTRE, item.FK_IDPRODUIT, item.DATEEVT, laCommande);
                }
                InsertOrUpdateLotri(NewLotri, laCommande);

                if (lstEvenement.First().IDETAPE != null)
                    TransmettreDemande(lstEvenement.First().NUMDEM, lstEvenement.First().IDETAPE.Value, lstEvenement.First().USERCREATION, laCommande);

                laCommande.Transaction.Commit();
                return true;
            }
            catch (Exception ex)
            {
                laCommande.Transaction.Rollback();
                return false;
            }
            finally
            {
                if (laCommande.Connection.State == ConnectionState.Open)
                    laCommande.Connection.Close(); // Fermeture de la connection 
                laCommande.Dispose();
            }
        }
        public string InsertSortieMateriel(int IdLivreur, int IdRecepteur, int idEtape, List<CsCanalisation> lstCompteurValide, bool IsExtension)
        {
            SqlCommand laCommande = null;
            try
            {
                laCommande = new SqlCommand();
                laCommande = DBBase.InitTransaction(ConnectionString);
                foreach (CsCanalisation item in lstCompteurValide)
                {
                    InsertOrUpdateSortieMateriel(IdLivreur, IdRecepteur, item, laCommande);
                    TransmettreDemande(item.NUMDEM, idEtape, item.USERCREATION, laCommande);
                }
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
                    laCommande.Connection.Close(); // Fermeture de la connection 
                laCommande.Dispose();
            }
        }
        public List<CsCanalisation> GetDemandeByNumIdDemandeSpx(string NumeroProgramme, int? EtapeActuelle)
        {
            cn = new SqlConnection(ConnectionString);

            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandTimeout = 3000;
            cmd.CommandType = CommandType.StoredProcedure;
            if (cmd.Parameters != null && cmd.Parameters.Count != 0) cmd.Parameters.Clear();
            cmd.CommandText = "SPX_ACC_CHARGERDONNESORTIECOMPTEUR";
            cmd.Parameters.Add("@idetape", SqlDbType.Int).Value = EtapeActuelle;
            cmd.Parameters.Add("@NumeroProgramme", SqlDbType.VarChar, 50).Value = NumeroProgramme;

            DBBase.SetDBNullParametre(cmd.Parameters);
            try
            {
                if (cn.State == ConnectionState.Closed)
                    cn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);
                return Entities.GetEntityListFromQuery<CsCanalisation>(dt);
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
        public List<CsCanalisation> GetDemandeByListeNumIdDemandeSortieMaterielSpx(string NumerodeProgramme, int? EtapeActuelle)
        {
            cn = new SqlConnection(ConnectionString);

            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandTimeout = 3000;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SPX_ACC_CHARGERDONNESORTIEMATERIEL";
            if (cmd.Parameters != null && cmd.Parameters.Count != 0) cmd.Parameters.Clear();
            cmd.Parameters.Add("@NumeroProgramme", SqlDbType.VarChar, 50).Value = NumerodeProgramme;
            cmd.Parameters.Add("@idetape", SqlDbType.Int).Value = EtapeActuelle;
            DBBase.SetDBNullParametre(cmd.Parameters);
            try
            {
                if (cn.State == ConnectionState.Closed)
                    cn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);
                return Entities.GetEntityListFromQuery<CsCanalisation>(dt);
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
        public List<ObjELEMENTDEVIS> ChargerListeDonneeProgramReedition(string NumProgramme)
        {
            cn = new SqlConnection(ConnectionString);

            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandTimeout = 3000;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SPX_ACC_CHARGERDONNEPROGRAMME";
            cmd.Parameters.Add("@NumProgramme", SqlDbType.VarChar, 20).Value = NumProgramme;

            DBBase.SetDBNullParametre(cmd.Parameters);
            try
            {
                if (cn.State == ConnectionState.Closed)
                    cn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);
                return Entities.GetEntityListFromQuery<ObjELEMENTDEVIS>(dt);
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

        public CsDemande ChargerDetailClient(int fk_idcentre, string centre, string client, string ordre, string produit, string Typedemande)
        {
            SqlConnection laConnection = DBBase.InitConnection(ConnectionString);

            try
            {
                CsDemande _LaDemande = new CsDemande();
                _LaDemande.Ag = Select_Ag(fk_idcentre, centre, client, laConnection);
                _LaDemande.LeClient = Select_client(fk_idcentre, centre, client, ordre, laConnection);
                if (_LaDemande.LeClient != null && !string.IsNullOrEmpty(_LaDemande.LeClient.NOMABON))
                {
                    _LaDemande.SocietePrives = Select_SocietePrive(_LaDemande.LeClient.PK_ID, laConnection);
                    _LaDemande.AdministrationInstitut = Select_Administration_Institut(_LaDemande.LeClient.PK_ID, laConnection);
                    _LaDemande.PersonePhysique = Select_PersonnePhysique(_LaDemande.LeClient.PK_ID, laConnection);
                }
                _LaDemande.Abonne = Select_Abon(fk_idcentre, centre, client, ordre, produit, laConnection);
                _LaDemande.Branchement = Select_brt(fk_idcentre, centre, client, laConnection);
                List<CsCanalisation> lstCan = Select_CannalisationCompteur(fk_idcentre, centre, client, produit, laConnection);
                if (lstCan != null && lstCan.Count != 0)
                {
                    _LaDemande.LstCanalistion = new List<CsCanalisation>();
                    _LaDemande.LstCanalistion.Add(lstCan.OrderByDescending(p => p.POSE).First());
                }
                return _LaDemande;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                if (laConnection.State == ConnectionState.Open)
                    laConnection.Close(); // Fermeture de la connection 
                laConnection.Dispose();

            }
        }

        /*

        public CsDemande ChargerDetailDemande(int idDemande, string NumDemande)
        {
            try
            {
                CsDemande _LaDemande = new CsDemande();
                _LaDemande.LaDemande = Select_Demande(NumDemande, idDemande);
                if (_LaDemande.LaDemande != null && !string.IsNullOrEmpty(_LaDemande.LaDemande.TYPEDEMANDE))
                {
                    _LaDemande.InfoDemande = new DB_WORKFLOW().RecupererInfoDemandeParCodeTDem(_LaDemande.LaDemande);
                    if (_LaDemande.LaDemande.TYPEDEMANDE == Enumere.Resiliation)
                    {
                        if (_LaDemande.InfoDemande.CODEETAPE == "SIDX")
                        { }
                        else if (_LaDemande.InfoDemande.CODEETAPE == "VDRC")
                        {
                            _LaDemande.SocietePrives = Select_DSocietePrive(_LaDemande.LaDemande.PK_ID);
                            _LaDemande.AdministrationInstitut = Select_DAdministration_Institut(_LaDemande.LaDemande.PK_ID);
                            _LaDemande.PersonePhysique = Select_DPersonnePhysique(_LaDemande.LaDemande.PK_ID);
                            _LaDemande.LstCommentaire = new DB_WORKFLOW().SelectCommentaireRejet(NumDemande);

                            _LaDemande.Ag = Select_DAg(NumDemande, idDemande);
                            _LaDemande.LeClient = Select_Dclient(NumDemande, idDemande);
                            _LaDemande.LstCanalistion = Select_DCannalisationInCompteur(NumDemande, idDemande);
                            _LaDemande.Abonne = Select_DAbon(NumDemande, idDemande);
                            _LaDemande.Branchement = Select_Dbrt(NumDemande, idDemande);
                            _LaDemande.leCompteClient = new DBEncaissement().ListeFactureNonSolde(_LaDemande.LaDemande.CENTRE, _LaDemande.LaDemande.CLIENT, _LaDemande.LaDemande.ORDRE, _LaDemande.LaDemande.FK_IDCENTRE);
                        }
                    }
                    else
                    {
                        _LaDemande.SocietePrives = Select_DSocietePrive(_LaDemande.LaDemande.PK_ID);
                        _LaDemande.AdministrationInstitut = Select_DAdministration_Institut(_LaDemande.LaDemande.PK_ID);
                        _LaDemande.PersonePhysique = Select_DPersonnePhysique(_LaDemande.LaDemande.PK_ID);
                        _LaDemande.LstCommentaire = new DB_WORKFLOW().SelectCommentaireRejet(NumDemande);

                        _LaDemande.Ag = Select_DAg(NumDemande, idDemande);
                        _LaDemande.LeClient = Select_Dclient(NumDemande, idDemande);

                        _LaDemande.Abonne = Select_DAbon(NumDemande, idDemande);
                        _LaDemande.Branchement = Select_Dbrt(NumDemande, idDemande);
                        _LaDemande.EltDevis = Select_ElementDevis(NumDemande, idDemande);
                        _LaDemande.LstCoutDemande = Select_FactureDemande(NumDemande, idDemande);
                        _LaDemande.AppareilDevis = Select_Appareil(NumDemande, idDemande);
                        _LaDemande.AnnotationDemande = Select_Annotation(idDemande);
                        _LaDemande.ObjetScanne = Select_DocumentTittre(idDemande);
                        _LaDemande.LstFraixParticipation = Select_FraisParticipation(idDemande);

                        if (_LaDemande.LaDemande.TYPEDEMANDE == Enumere.TransfertAbonnement)
                            _LaDemande.Transfert = Select_Dtransfert(idDemande, NumDemande);

                        if ((_LaDemande.LaDemande.TYPEDEMANDE == Enumere.BranchementAbonement) ||
                            (_LaDemande.LaDemande.TYPEDEMANDE == Enumere.BranchementAbonementExtention)) //ZEG 25/09/2017

                            _LaDemande.LstCanalistion = Select_DCanalisationInMagazin(NumDemande, idDemande);
                        else if (_LaDemande.LaDemande.TYPEDEMANDE == Enumere.ModificationCompteur)
                            _LaDemande.LstCanalistion = Select_DCannalisationInDCompteur(NumDemande, idDemande);
                        else
                            _LaDemande.LstCanalistion = Select_DCannalisationInCompteur(NumDemande, idDemande);
                    }
                }
                return _LaDemande;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        */

        public string ValiderOrdreDeTravail(CsDemande laDemande, CsAffectationDemandeUser leAffection, bool AvecTransmission)
        {
            SqlCommand laCommande = DBBase.InitTransaction(ConnectionString);
            try
            {
                #region Affectation
                InsertOrUpdateAffectation(leAffection, laCommande);
                #endregion
                #region Ordre de travail
                InsertOrUpdateOrdreDeTravail(laDemande.OrdreTravail, laCommande);
                #endregion
                if (AvecTransmission)
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
        public void InsertOrUpdateOrdreDeTravail(CsOrdreTravail leOrdreDeTravail, SqlCommand cmds)
        {
            cmds.CommandTimeout = 1800;
            cmds.CommandType = CommandType.StoredProcedure;
            cmds.CommandText = "SPX_ACC_INSERTORUPDATE_DORDRETRAVAIL";
            if (cmds.Parameters != null && cmds.Parameters.Count != 0) cmds.Parameters.Clear();

            cmds.Parameters.Add("@PRESTATAIRE", SqlDbType.VarChar, 50).Value = leOrdreDeTravail.PRESTATAIRE;
            cmds.Parameters.Add("@USERCREATION", SqlDbType.VarChar, 6).Value = leOrdreDeTravail.USERCREATION;
            cmds.Parameters.Add("@USERMODIFICATION", SqlDbType.VarChar, 6).Value = leOrdreDeTravail.USERMODIFICATION;
            cmds.Parameters.Add("@MATRICULE", SqlDbType.VarChar, 6).Value = leOrdreDeTravail.MATRICULE;
            cmds.Parameters.Add("@FK_IDDEMANDE", SqlDbType.Int).Value = leOrdreDeTravail.FK_IDDEMANDE;
            cmds.Parameters.Add("@FK_IDADMUTILISATEUR", SqlDbType.Int).Value = leOrdreDeTravail.FK_IDADMUTILISATEUR;
            cmds.Parameters.Add("@DATEDEBUTTRAVAUX", SqlDbType.DateTime).Value = leOrdreDeTravail.DATEDEBUTTRAVAUX;
            cmds.Parameters.Add("@DATEFINTRAVAUX", SqlDbType.DateTime).Value = leOrdreDeTravail.DATEFINTRAVAUX;
            cmds.Parameters.Add("@COMMENTAIRE", SqlDbType.VarChar, 500).Value = leOrdreDeTravail.COMMENTAIRE;
            DBBase.SetDBNullParametre(cmds.Parameters);
            try
            {
                if (cmds.Connection.State == ConnectionState.Closed)
                    cmds.Connection.Open();
                cmds.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception(cmds.CommandText + ":" + ex.Message);
            }
        }
        public string ValiderSuivieTravaux(CsDemande laDemande, bool AvecTransmission)
        {
            SqlCommand laCommande = DBBase.InitTransaction(ConnectionString);
            try
            {
                if (AvecTransmission)
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
        public CsOrdreTravail Select_DOrdreDeTravail(int Iddemande, SqlConnection laConnection)
        {
           
            cmd = new SqlCommand();
            cmd.Connection = laConnection;
            cmd.CommandTimeout = 1800;
            cmd.CommandType = CommandType.StoredProcedure;
            if (cmd.Parameters != null && cmd.Parameters.Count != 0) cmd.Parameters.Clear();

            cmd.CommandText = "SPX_ACC_SELECTDORDREDETRAVAIL";
            cmd.Parameters.Add("@IDDEMANDE", SqlDbType.Int).Value = Iddemande;

            DBBase.SetDBNullParametre(cmd.Parameters);
            try
            {
                if (laConnection.State == ConnectionState.Closed)
                    laConnection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);
                return Entities.GetEntityFromQuery<CsOrdreTravail>(dt);
            }
            catch (Exception ex)
            {
                throw new Exception(cmd.CommandText + ":" + ex.Message);
            }
            finally
            {
                cmd.Dispose();
            }
        }

        public bool SelectRemboursementEnCours(CsDemande dem)
        {
            cn = new SqlConnection(ConnectionString);

            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandTimeout = 3000;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SPX_ACC_REMBOURSEMENT_ENCOURS";
            if (cmd.Parameters != null && cmd.Parameters.Count != 0) cmd.Parameters.Clear();

            cmd.Parameters.Add("@FK_IDCENTRE", SqlDbType.Int).Value = dem.LeClient.FK_IDCENTRE;
            cmd.Parameters.Add("@CLIENT", SqlDbType.VarChar, 20).Value = dem.LeClient.REFCLIENT;
            cmd.Parameters.Add("@ORDRE", SqlDbType.VarChar, 2).Value = dem.LeClient.ORDRE;

            DBBase.SetDBNullParametre(cmd.Parameters);
            try
            {
                if (cn.State == ConnectionState.Closed)
                    cn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                return reader.HasRows;
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
        public CsDemande ChargerDetailDemandeConsultation(int idDemande, string NumDemande)
        {
            SqlConnection laConnection = DBBase.InitConnection(ConnectionString);
            try
            {
                CsDemande _LaDemande = new CsDemande();
                _LaDemande.LaDemande = Select_Demande(NumDemande, idDemande, laConnection);
                if (_LaDemande.LaDemande != null && !string.IsNullOrEmpty(_LaDemande.LaDemande.TYPEDEMANDE))
                {
                        _LaDemande.InfoDemande = new DB_WORKFLOW().RecupererInfoDemandeParCodeTDem(_LaDemande.LaDemande, laConnection);
                        _LaDemande.SocietePrives = Select_DSocietePrive(_LaDemande.LaDemande.PK_ID, laConnection);
                        _LaDemande.AdministrationInstitut = Select_DAdministration_Institut(_LaDemande.LaDemande.PK_ID, laConnection);
                        _LaDemande.PersonePhysique = Select_DPersonnePhysique(_LaDemande.LaDemande.PK_ID, laConnection);
                        _LaDemande.LstCommentaire = new DB_WORKFLOW().SelectCommentaireRejet(NumDemande, laConnection);

                        _LaDemande.Ag = Select_DAg(NumDemande, idDemande, laConnection);
                        _LaDemande.LeClient = Select_Dclient(NumDemande, idDemande, laConnection);
                        _LaDemande.Abonne = Select_DAbon(NumDemande, idDemande, laConnection);
                        _LaDemande.Branchement = Select_Dbrt(NumDemande, idDemande, laConnection);
                        _LaDemande.ObjetScanne = Select_DocumentTittre(idDemande, laConnection);

                        if (_LaDemande.LeClient != null && _LaDemande.LeClient.PROPRIO == Enumere.LOCATAIRE)
                            _LaDemande.InfoProprietaire_ = Select_InformationPropriete(NumDemande, idDemande, laConnection);

                        _LaDemande.LstCanalistion = Select_DCanalisationInMagazin(NumDemande, idDemande, laConnection);
                        _LaDemande.EltDevis = Select_ElementDevis(NumDemande, idDemande, laConnection);
                        _LaDemande.LstCoutDemande = Select_FactureDemande(NumDemande, idDemande, laConnection);
                        _LaDemande.AppareilDevis = Select_Appareil(NumDemande, idDemande, laConnection);
                        _LaDemande.Programmation = RetoureDetailProgramme(idDemande, laConnection);

                }
                return _LaDemande;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                if (laConnection.State == ConnectionState.Open)
                    laConnection.Close(); // Fermeture de la connection 
                laConnection.Dispose();
            }
        }
        public CsDemande ChargerDetailDemande(int idDemande, string NumDemande)
        {
            SqlConnection laConnection = DBBase.InitConnection(ConnectionString);

            try
            {
                CsDemande _LaDemande = new CsDemande();
                _LaDemande.LaDemande = Select_Demande(NumDemande, idDemande, laConnection);
                if (_LaDemande.LaDemande != null && !string.IsNullOrEmpty(_LaDemande.LaDemande.TYPEDEMANDE))
                {
                    _LaDemande.InfoDemande = new DB_WORKFLOW().RecupererInfoDemandeParCodeTDem(_LaDemande.LaDemande, laConnection);
                    if (_LaDemande.LaDemande.TYPEDEMANDE == Enumere.Resiliation)
                    {

                        /** ZEG 28/09/2017 **/
                        /*
                        if (_LaDemande.InfoDemande.CODEETAPE == "SIDX")
                        { }
                        else if (_LaDemande.InfoDemande.CODEETAPE == "VDRC")
                        {
                            _LaDemande.SocietePrives = Select_DSocietePrive(_LaDemande.LaDemande.PK_ID);
                            _LaDemande.AdministrationInstitut = Select_DAdministration_Institut(_LaDemande.LaDemande.PK_ID);
                            _LaDemande.PersonePhysique = Select_DPersonnePhysique(_LaDemande.LaDemande.PK_ID);
                            _LaDemande.LstCommentaire = new DB_WORKFLOW().SelectCommentaireRejet(NumDemande);

                            _LaDemande.Ag = Select_DAg(NumDemande, idDemande);
                            _LaDemande.LeClient = Select_Dclient(NumDemande, idDemande);
                            _LaDemande.LstCanalistion = Select_DCannalisationInCompteur(NumDemande, idDemande);
                            _LaDemande.Abonne = Select_DAbon(NumDemande, idDemande);
                            _LaDemande.Branchement = Select_Dbrt(NumDemande, idDemande);
                            _LaDemande.leCompteClient = new DBEncaissement().ListeFactureNonSolde(_LaDemande.LaDemande.CENTRE, _LaDemande.LaDemande.CLIENT, _LaDemande.LaDemande.ORDRE, _LaDemande.LaDemande.FK_IDCENTRE);
                        }

                        */

                        //_LaDemande.LaDemande = this.GetDemandeByNumIdDemande(idDemande);

                        _LaDemande.SocietePrives = Select_DSocietePrive(_LaDemande.LaDemande.PK_ID, laConnection);
                        _LaDemande.AdministrationInstitut = Select_DAdministration_Institut(_LaDemande.LaDemande.PK_ID, laConnection);
                        _LaDemande.PersonePhysique = Select_DPersonnePhysique(_LaDemande.LaDemande.PK_ID, laConnection);
                        _LaDemande.LstCommentaire = new DB_WORKFLOW().SelectCommentaireRejet(NumDemande, laConnection);

                        _LaDemande.Ag = Select_DAg(NumDemande, idDemande, laConnection);
                        _LaDemande.LeClient = Select_Dclient(NumDemande, idDemande, laConnection);
                        _LaDemande.LstCanalistion = Select_DCannalisationInCompteur(NumDemande, idDemande, laConnection);
                        _LaDemande.Abonne = Select_DAbon(NumDemande, idDemande, laConnection);
                        _LaDemande.Branchement = Select_Dbrt(NumDemande, idDemande, laConnection);
                        _LaDemande.ObjetScanne = Select_DocumentTittre(idDemande, laConnection);
                        _LaDemande.LstEvenement = Select_Devenement(_LaDemande.LaDemande);

                        if (_LaDemande.LeClient != null && _LaDemande.LeClient.PROPRIO == Enumere.LOCATAIRE)
                            _LaDemande.InfoProprietaire_ = Select_InformationPropriete(NumDemande, idDemande, laConnection);

                        //if (_LaDemande.InfoDemande.CODEETAPE == "VDRC")
                        //{
                        //    _LaDemande.leCompteClient = new DBEncaissement().ListeFactureNonSolde(_LaDemande.LaDemande.CENTRE, _LaDemande.LaDemande.CLIENT, _LaDemande.LaDemande.ORDRE, _LaDemande.LaDemande.FK_IDCENTRE);
                        //}

                        /** ZEG **/
                    }
                    else
                    {

                        if (_LaDemande.LaDemande.TYPEDEMANDE == Enumere.AchatTimbre )
                            _LaDemande.LstEltTimbre = Select_ElementAchatTimbre(NumDemande, idDemande, laConnection);

                       else if (_LaDemande.LaDemande.TYPEDEMANDE == Enumere.ReprisIndex || _LaDemande.LaDemande.TYPEDEMANDE == Enumere.AnnulationFacture)
                        {
                            _LaDemande.LeClient = Select_Dclient(NumDemande, idDemande, laConnection);
                            _LaDemande.LstEvenement = Select_Devenement(_LaDemande.LaDemande);
                            _LaDemande.AnnotationDemande = Select_Annotation(idDemande, laConnection);
                        }
                        else
                        {
                            _LaDemande.SocietePrives = Select_DSocietePrive(_LaDemande.LaDemande.PK_ID, laConnection);
                            _LaDemande.AdministrationInstitut = Select_DAdministration_Institut(_LaDemande.LaDemande.PK_ID, laConnection);
                            _LaDemande.PersonePhysique = Select_DPersonnePhysique(_LaDemande.LaDemande.PK_ID, laConnection);
                            _LaDemande.LstCommentaire = new DB_WORKFLOW().SelectCommentaireRejet(NumDemande, laConnection);

                            _LaDemande.Ag = Select_DAg(NumDemande, idDemande,laConnection);
                            _LaDemande.LeClient = Select_Dclient(NumDemande, idDemande, laConnection);

                            if (_LaDemande.LeClient != null && _LaDemande.LeClient.PROPRIO == Enumere.LOCATAIRE)
                                _LaDemande.InfoProprietaire_ = Select_InformationPropriete(NumDemande, idDemande, laConnection);


                            _LaDemande.Abonne = Select_DAbon(NumDemande, idDemande, laConnection);

                            if (_LaDemande.LaDemande.TYPEDEMANDE == Enumere.ChangementProduit ||
                                _LaDemande.LaDemande.TYPEDEMANDE == Enumere.Reabonnement ||
                                _LaDemande.LaDemande.TYPEDEMANDE == Enumere.AugmentationPuissance ||
                                _LaDemande.LaDemande.TYPEDEMANDE == Enumere.DimunitionPuissance)
                            {
                                CsAbon abon = Select_Abon(int.Parse(_LaDemande.LeClient.FK_IDCENTRE.ToString()), _LaDemande.LeClient.CENTRE, _LaDemande.LeClient.REFCLIENT, _LaDemande.LeClient.ORDRE, _LaDemande.LeClient.PRODUIT, laConnection);
                                _LaDemande.Abonne.PK_ID = abon.PK_ID;
                            }

                            _LaDemande.Branchement = Select_Dbrt(NumDemande, idDemande, laConnection);
                            _LaDemande.EltDevis = Select_ElementDevis(NumDemande, idDemande, laConnection);
                            _LaDemande.LstCoutDemande = Select_FactureDemande(NumDemande, idDemande, laConnection);
                            _LaDemande.AppareilDevis = Select_Appareil(NumDemande, idDemande, laConnection);
                            _LaDemande.AnnotationDemande = Select_Annotation(idDemande, laConnection);
                            _LaDemande.ObjetScanne = Select_DocumentTittre(idDemande, laConnection);
                            _LaDemande.LstFraixParticipation = Select_FraisParticipation(idDemande, laConnection);

                            if (_LaDemande.LaDemande.TYPEDEMANDE == Enumere.TransfertAbonnement)
                                _LaDemande.Transfert = Select_Dtransfert(idDemande, NumDemande);

                            if (_LaDemande.LaDemande.TYPEDEMANDE == Enumere.BranchementAbonement ||
                                _LaDemande.LaDemande.TYPEDEMANDE == Enumere.BranchementAbonementExtention ||
                                _LaDemande.LaDemande.TYPEDEMANDE == Enumere.ChangementProduit ||
                                (_LaDemande.LaDemande.TYPEDEMANDE == Enumere.AugmentationPuissance && _LaDemande.LaDemande.ISMETREAFAIRE) ||
                                (_LaDemande.LaDemande.TYPEDEMANDE == Enumere.DimunitionPuissance && _LaDemande.LaDemande.ISMETREAFAIRE) ||
                                (_LaDemande.LaDemande.TYPEDEMANDE == Enumere.AbonnementSeul && _LaDemande.LaDemande.ISMETREAFAIRE))
                            {
                                _LaDemande.LstCanalistion = Select_DCanalisationInMagazin(NumDemande, idDemande, laConnection);
                                if (_LaDemande.LaDemande.TYPEDEMANDE == Enumere.BranchementAbonementExtention)
                                    _LaDemande.OrdreTravail = Select_DOrdreDeTravail(idDemande, laConnection);
                            }
                            else if (_LaDemande.LaDemande.TYPEDEMANDE == Enumere.ModificationCompteur)
                                _LaDemande.LstCanalistion = Select_DCannalisationInDCompteur(NumDemande, idDemande, laConnection);
                            else
                                _LaDemande.LstCanalistion = Select_DCannalisationInCompteur(NumDemande, idDemande, laConnection);


                            if (_LaDemande.LaDemande.TYPEDEMANDE == Enumere.ModificationBranchement)
                            {
                                CsAg ag = Select_Ag(_LaDemande.LaDemande.FK_IDCENTRE, _LaDemande.LaDemande.CENTRE, _LaDemande.LaDemande.CLIENT, laConnection);
                                _LaDemande.Branchement.FK_IDAG = ag.PK_ID;
                            }

                        }
                    }
                }
                return _LaDemande;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                if (laConnection.State == ConnectionState.Open)
                    laConnection.Close(); // Fermeture de la connection 
                laConnection.Dispose();
            }
        }

        public void TransmettreDemande(string NUMDEM, int idEtapeActuel, string MATRICULE, SqlCommand cmds)
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

        public void TransmettreDemande(string NUMDEM, int idEtapeActuel, string MATRICULE)
        {
            cn = new SqlConnection(ConnectionString);

            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandTimeout = 3000;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SPX_WKF_TRANSMETTRE_DEMANDE";

            if (cmd.Parameters != null && cmd.Parameters.Count != 0) cmd.Parameters.Clear();
            cmd.Parameters.Add("@NUMDEM", SqlDbType.VarChar, 20).Value = NUMDEM;
            cmd.Parameters.Add("@FK_IDETAPE", SqlDbType.Int).Value = idEtapeActuel;
            cmd.Parameters.Add("@MATRICULE", SqlDbType.VarChar, 6).Value = MATRICULE;

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

        public void UpdateWKF(string NUMDEM,int? iddemande, int idEtapeVoulu, string MATRICULE,string MotifDeRejet, SqlCommand cmds)
        {


            cmds.CommandTimeout = 3000;
            cmds.CommandType = CommandType.StoredProcedure;
            cmds.CommandText = "SPX_WKF_UPDATEWKF";
            if (cmds.Parameters != null && cmds.Parameters.Count != 0) cmds.Parameters.Clear();
            cmds.Parameters.Add("@NUMDEM", SqlDbType.VarChar, 20).Value = NUMDEM;
            cmds.Parameters.Add("@FK_DEMANDE", SqlDbType.Int ).Value = iddemande;
            cmds.Parameters.Add("@FK_IDETAPE", SqlDbType.Int).Value = idEtapeVoulu;
            cmds.Parameters.Add("@MATRICULE", SqlDbType.VarChar, 6).Value = MATRICULE;
            cmds.Parameters.Add("@MotifDeRejet", SqlDbType.VarChar, 200).Value = MATRICULE;

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
        public void CreeDemande(int IDDEMANDE, string NUMDEM, string CODEDEMANDEWKF, SqlCommand cmds)
        {
            cmds.CommandTimeout = 3000;
            cmds.CommandType = CommandType.StoredProcedure;
            cmds.CommandText = "SPX_WKF_CREEDEMANDE";
            if (cmds.Parameters != null && cmds.Parameters.Count != 0) cmds.Parameters.Clear();
            cmds.Parameters.Add("@FK_IDDEMANDE", SqlDbType.Int).Value = IDDEMANDE;
            cmds.Parameters.Add("@NUMDEM", SqlDbType.VarChar, 20).Value = NUMDEM;
            cmds.Parameters.Add("@CODEDEMANDEWKF", SqlDbType.VarChar, 50).Value = CODEDEMANDEWKF;

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
        public void CreationCtarCompt(int fk_idAbon, string lotri, string periode, string Matricule, int idcentre, int idproduit, DateTime? dateEvt, SqlCommand cmds)
        {

            cmds.CommandTimeout = 3000;
            cmds.CommandType = CommandType.StoredProcedure;
            if (cmds.Parameters != null && cmds.Parameters.Count != 0) cmds.Parameters.Clear();
            cmds.CommandText = "SPX_CREATION_CTARCOMP";
            cmds.Parameters.Add("@IdAbon", SqlDbType.Int).Value = fk_idAbon;
            cmds.Parameters.Add("@Lotri", SqlDbType.VarChar, 8).Value = lotri;
            cmds.Parameters.Add("@Periode", SqlDbType.VarChar, 8).Value = periode;
            cmds.Parameters.Add("@Matricule", SqlDbType.VarChar, 6).Value = Matricule;
            cmds.Parameters.Add("@IdCentre", SqlDbType.Int).Value = idcentre;
            cmds.Parameters.Add("@IdProduit", SqlDbType.Int).Value = idproduit;
            cmds.Parameters.Add("@DateEvt", SqlDbType.DateTime).Value = dateEvt;

            int Insere = -1;
            DBBase.SetDBNullParametre(cmds.Parameters);
            try
            {

                Insere = cmds.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception(cmds.CommandText + ":" + ex.Message);
            }

        }

        public void TransfertClient(int idDemande, string NumDem, string Matricule, bool IstransfertImpayes, SqlCommand cmds)
        {
            cmds.CommandTimeout = 1800;
            cmds.CommandType = CommandType.StoredProcedure;
            cmds.CommandText = "SPX_ACC_VALIDER_TRANSFERTCLIENT";
            if (cmds.Parameters != null && cmds.Parameters.Count != 0) cmds.Parameters.Clear();
            cmds.Parameters.Add("@FK_IDDEMANDE", SqlDbType.Int).Value = idDemande;
            cmds.Parameters.Add("@NUMDEM", SqlDbType.VarChar, 20).Value = NumDem;
            cmds.Parameters.Add("@MATRICULE", SqlDbType.VarChar, 6).Value = Matricule;
            cmds.Parameters.Add("@ISTRANFERTIMPAYES", SqlDbType.Bit).Value = IstransfertImpayes;


            DBBase.SetDBNullParametre(cmds.Parameters);
            try
            {
                if (cmds.Connection.State == ConnectionState.Closed)
                    cmds.Connection.Open();
                cmds.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                new ErrorManager().WriteInLogFile(this, cmds.CommandText + " : " + ex.Message);
                throw ex;
            }
        }

        public void InsertOrUpdateSociete(CsSocietePrive laSociete, SqlCommand cmds)
        {
            cmds.CommandTimeout = 1800;
            cmds.CommandType = CommandType.StoredProcedure;
            cmds.CommandText = "SPX_ACC_INSERTORUPDATE_SOCIETE";
            if (cmds.Parameters != null && cmds.Parameters.Count != 0) cmds.Parameters.Clear();

            cmds.Parameters.Add("@PK_ID", SqlDbType.Int).Value = laSociete.PK_ID;
            cmds.Parameters.Add("@NUMEROREGISTRECOMMERCE", SqlDbType.VarChar, 50).Value = laSociete.NUMEROREGISTRECOMMERCE;
            cmds.Parameters.Add("@FK_IDSTATUTJURIQUE", SqlDbType.Int).Value = laSociete.FK_IDSTATUTJURIQUE;
            cmds.Parameters.Add("@CAPITAL", SqlDbType.Decimal).Value = laSociete.CAPITAL;
            cmds.Parameters.Add("@IDENTIFICATIONFISCALE", SqlDbType.VarChar, 50).Value = laSociete.IDENTIFICATIONFISCALE;
            cmds.Parameters.Add("@DATECREATION", SqlDbType.DateTime).Value = laSociete.DATECREATION;
            cmds.Parameters.Add("@SIEGE", SqlDbType.VarChar, 20).Value = laSociete.SIEGE;
            cmds.Parameters.Add("@IDCLIENT", SqlDbType.Int).Value = laSociete.FK_IDCLIENT;
            cmds.Parameters.Add("@NOMMANDATAIRE", SqlDbType.VarChar, 20).Value = laSociete.NOMMANDATAIRE;
            cmds.Parameters.Add("@PRENOMMANDATAIRE", SqlDbType.VarChar, 50).Value = laSociete.PRENOMMANDATAIRE;
            cmds.Parameters.Add("@RANGMANDATAIRE", SqlDbType.VarChar, 50).Value = laSociete.RANGMANDATAIRE;
            cmds.Parameters.Add("@NOMSIGNATAIRE", SqlDbType.VarChar, 50).Value = laSociete.NOMSIGNATAIRE;
            cmds.Parameters.Add("@PRENOMSIGNATAIRE", SqlDbType.VarChar, 50).Value = laSociete.PRENOMSIGNATAIRE;
            cmds.Parameters.Add("@RANGSIGNATAIRE", SqlDbType.VarChar, 20).Value = laSociete.RANGSIGNATAIRE;
            cmds.Parameters.Add("@NOMABON", SqlDbType.VarChar, 50).Value = laSociete.RANGSIGNATAIRE;
            DBBase.SetDBNullParametre(cmds.Parameters);
            try
            {
                if (cmds.Connection.State == ConnectionState.Closed)
                    cmds.Connection.Open();
                cmds.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception(cmd.CommandText + ":" + ex.Message);
            }
        }
        public void InsertOrUpdateAdmnistration_Institut(CsAdministration_Institut leAdmini, SqlCommand cmds)
        {
            cmds.CommandTimeout = 1800;
            cmds.CommandType = CommandType.StoredProcedure;
            cmds.CommandText = "SPX_ACC_INSERTORUPDATE_ADMINISTRATIONINSTITUT";
            if (cmds.Parameters != null && cmds.Parameters.Count != 0) cmds.Parameters.Clear();

            cmds.Parameters.Add("@NOMMANDATAIRE", SqlDbType.VarChar, 50).Value = leAdmini.NOMMANDATAIRE;
            cmds.Parameters.Add("@PRENOMMANDATAIRE", SqlDbType.VarChar, 50).Value = leAdmini.PRENOMMANDATAIRE;
            cmds.Parameters.Add("@RANGMANDATAIRE", SqlDbType.VarChar, 50).Value = leAdmini.RANGMANDATAIRE;
            cmds.Parameters.Add("@NOMSIGNATAIRE", SqlDbType.VarChar, 50).Value = leAdmini.NOMSIGNATAIRE;
            cmds.Parameters.Add("@PRENOMSIGNATAIRE", SqlDbType.VarChar, 50).Value = leAdmini.PRENOMSIGNATAIRE;
            cmds.Parameters.Add("@RANGSIGNATAIRE", SqlDbType.VarChar, 50).Value = leAdmini.RANGSIGNATAIRE;
            cmds.Parameters.Add("@IDCLIENT", SqlDbType.Int).Value = leAdmini.FK_IDCLIENT;
            cmds.Parameters.Add("@NOMABON", SqlDbType.VarChar, 50).Value = leAdmini.NOMABON;
            DBBase.SetDBNullParametre(cmds.Parameters);
            try
            {
                if (cmds.Connection.State == ConnectionState.Closed)
                    cmds.Connection.Open();
                cmds.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                throw new Exception(cmds.CommandText + ":" + ex.Message);
            }
        }
        public void InsertOrUpdatepersPhysique(CsPersonePhysique laPersphysique, SqlCommand cmds)
        {
            cmds.CommandTimeout = 1800;
            cmds.CommandType = CommandType.StoredProcedure;
            cmds.CommandText = "SPX_ACC_INSERTORUPDATE_PERSONNEPHYSIQUE";
            if (cmds.Parameters != null && cmds.Parameters.Count != 0) cmds.Parameters.Clear();

            cmds.Parameters.Add("@PK_ID", SqlDbType.Int).Value = laPersphysique.PK_ID;
            cmds.Parameters.Add("@DATENAISSANCE", SqlDbType.DateTime).Value = laPersphysique.DATENAISSANCE;
            cmds.Parameters.Add("@NUMEROPIECEIDENTITE", SqlDbType.VarChar, 50).Value = laPersphysique.NUMEROPIECEIDENTITE;
            cmds.Parameters.Add("@DATEFINVALIDITE", SqlDbType.DateTime).Value = laPersphysique.DATEFINVALIDITE;
            cmds.Parameters.Add("@IDCLIENT", SqlDbType.Int).Value = laPersphysique.FK_IDCLIENT;
            cmds.Parameters.Add("@FK_IDPIECEIDENTITE", SqlDbType.Int).Value = laPersphysique.FK_IDPIECEIDENTITE;
            cmds.Parameters.Add("@NOMABON", SqlDbType.VarChar, 50).Value = laPersphysique.NOMABON;

            DBBase.SetDBNullParametre(cmds.Parameters);

            try
            {
                if (cmds.Connection.State == ConnectionState.Closed)
                    cmds.Connection.Open();
                cmds.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                throw new Exception(cmds.CommandText + ":" + ex.Message);
            }
        }

        public void InsertOrUpdateParticipation(CsFraixParticipation leFrais, SqlCommand cmds)
        {
            cmds.CommandTimeout = 1800;
            cmds.CommandType = CommandType.StoredProcedure;
            cmds.CommandText = "SPX_ACC_INSERTORUPDATE_PARTICIPATION";
            if (cmds.Parameters != null && cmds.Parameters.Count != 0) cmds.Parameters.Clear();

            cmds.Parameters.Add("@REF_CLIENT", SqlDbType.VarChar, 20).Value = leFrais.REF_CLIENT;
            cmds.Parameters.Add("@ESTEXONERE", SqlDbType.Bit).Value = leFrais.ESTEXONERE;
            cmds.Parameters.Add("@PREUVE", SqlDbType.VarBinary, int.MaxValue).Value = leFrais.PREUVE;
            cmds.Parameters.Add("@FK_IDDEMANDE", SqlDbType.Int).Value = leFrais.FK_IDDEMANDE;
            cmds.Parameters.Add("@FK_IDCLIENT", SqlDbType.Int).Value = leFrais.FK_IDCLIENT;
            cmds.Parameters.Add("@CENTRE ", SqlDbType.VarChar, Enumere.TailleCentre).Value = leFrais.CENTRE;
            cmds.Parameters.Add("@ORDRE", SqlDbType.VarChar, Enumere.TailleOrdre).Value = leFrais.ORDRE;
            cmds.Parameters.Add("@MONTANT", SqlDbType.Decimal).Value = leFrais.MONTANT;

            DBBase.SetDBNullParametre(cmds.Parameters);

            try
            {
                if (cmds.Connection.State == ConnectionState.Closed)
                    cmds.Connection.Open();
                cmds.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                throw new Exception(cmds.CommandText + ":" + ex.Message);
            }
        }
        public List<CsFraixParticipation> Select_FraisParticipation(int Iddemande, SqlConnection laConnection)
        {
            cmd = new SqlCommand();
            cmd.Connection = laConnection;
            cmd.CommandTimeout = 1800;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SPX_ACC_SELECTFRAIXPARICIPATIONDEVIS";
            if (cmd.Parameters != null && cmd.Parameters.Count != 0) cmd.Parameters.Clear();
            cmd.Parameters.Add("@IDDEMANDE", SqlDbType.Int).Value = Iddemande;
            DBBase.SetDBNullParametre(cmd.Parameters);
            try
            {
                if (laConnection.State == ConnectionState.Closed)
                    laConnection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);
                return Entities.GetEntityListFromQuery<CsFraixParticipation>(dt);
            }
            catch (Exception ex)
            {
                throw new Exception(cmd.CommandText + ":" + ex.Message);
            }
            finally
            {
                cmd.Dispose();
            }
        }

        public CsDemandeBase GetNomEtCompteur(int Iddemande)
        {
            cn = new SqlConnection(ConnectionString);
            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandTimeout = 1800;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SPX_ACC_GET_COMPTEUR_NOM";
            if (cmd.Parameters != null && cmd.Parameters.Count != 0) cmd.Parameters.Clear();
            cmd.Parameters.Add("@IDDEMANDE", SqlDbType.Int).Value = Iddemande;
            DBBase.SetDBNullParametre(cmd.Parameters);
            try
            {
                if (cn.State == ConnectionState.Closed)
                    cn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                CsDemandeBase c = new CsDemandeBase();

                if (reader.Read())
                {
                    c.COMPTEUR = (Convert.IsDBNull(reader["NUMERO"])) ? string.Empty : (System.String)reader["NUMERO"];
                    c.NOMCLIENT = (Convert.IsDBNull(reader["NOMABON"])) ? string.Empty : (System.String)reader["NOMABON"];
                }

                reader.Close();

                return c;
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

        public CsDtransfert Select_Dtransfert(int Iddemande, string Numdem)
        {
            cn = new SqlConnection(ConnectionString);
            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandTimeout = 1800;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SPX_ACC_SELECTDTRANSFERT";
            if (cmd.Parameters != null && cmd.Parameters.Count != 0) cmd.Parameters.Clear();
            cmd.Parameters.Add("@IDDEMANDE", SqlDbType.Int).Value = Iddemande;
            cmd.Parameters.Add("@NUMDEM", SqlDbType.VarChar, 20).Value = Numdem;
            DBBase.SetDBNullParametre(cmd.Parameters);
            try
            {
                if (cn.State == ConnectionState.Closed)
                    cn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);
                return Entities.GetEntityFromQuery<CsDtransfert>(dt);
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
        public CsDemandeBase GetDemandeByNumIdDemande(int pIdDemande)
        {
            cn = new SqlConnection(ConnectionString);
            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandTimeout = 1800;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SPX_ACC_CHARGERDEMANDEWORKFLOW";
            if (cmd.Parameters != null && cmd.Parameters.Count != 0) cmd.Parameters.Clear();
            cmd.Parameters.Add("@IDDEMANDE", SqlDbType.Int).Value = pIdDemande;
            DBBase.SetDBNullParametre(cmd.Parameters);
            try
            {
                if (cn.State == ConnectionState.Closed)
                    cn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);
                return Entities.GetEntityFromQuery<CsDemandeBase>(dt);
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

        public List<CsDemandeBase> RetourneListeDemandeClient(int pIdCentre, string pCentre, string pClient, string pOrdre)
        {
            cn = new SqlConnection(ConnectionString);
            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandTimeout = 1800;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SPX_ACC_CHARGERLESDEMANDECLIENT";
            if (cmd.Parameters != null && cmd.Parameters.Count != 0) cmd.Parameters.Clear();
            cmd.Parameters.Add("@Centre", SqlDbType.VarChar, 3).Value = pCentre;
            cmd.Parameters.Add("@Client", SqlDbType.VarChar, 20).Value = pClient;
            cmd.Parameters.Add("@Ordre", SqlDbType.VarChar, 2).Value = pOrdre;
            cmd.Parameters.Add("@IdCentre", SqlDbType.Int).Value = pIdCentre;
            DBBase.SetDBNullParametre(cmd.Parameters);
            try
            {
                if (cn.State == ConnectionState.Closed)
                    cn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);
                return Entities.GetEntityListFromQuery<CsDemandeBase>(dt);
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

        public List<CsLclient> RetourneEncaissementClient(int Fk_idcentre, string centre, string client, string ordre)
        {
            cn = new SqlConnection(ConnectionString);

            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandTimeout = 3000;
            cmd.CommandType = CommandType.StoredProcedure;
            if (cmd.Parameters != null && cmd.Parameters.Count != 0) cmd.Parameters.Clear();
            cmd.CommandText = "SPX_ACC_SELECTREGLEMENTCLIENT";
            cmd.Parameters.Add("@FK_IDCENTRE", SqlDbType.Int).Value = Fk_idcentre;
            cmd.Parameters.Add("@CENTRE", SqlDbType.VarChar, 3).Value = centre;
            cmd.Parameters.Add("@CLIENT", SqlDbType.VarChar, 20).Value = client;
            cmd.Parameters.Add("@ORDRE", SqlDbType.VarChar, 2).Value = ordre;

            DBBase.SetDBNullParametre(cmd.Parameters);
            try
            {
                if (cn.State == ConnectionState.Closed)
                    cn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);
                return Entities.GetEntityListFromQuery<CsLclient>(dt);
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
        public List<CsLclient> RetourneFactureClient(int Fk_idcentre, string centre, string client, string ordre)
        {
            cn = new SqlConnection(ConnectionString);

            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandTimeout = 3000;
            cmd.CommandType = CommandType.StoredProcedure;
            if (cmd.Parameters != null && cmd.Parameters.Count != 0) cmd.Parameters.Clear();
            cmd.CommandText = "SPX_ACC_SELECTFACTURECLIENT";
            cmd.Parameters.Add("@FK_IDCENTRE", SqlDbType.Int).Value = Fk_idcentre;
            cmd.Parameters.Add("@CENTRE", SqlDbType.VarChar, 3).Value = centre;
            cmd.Parameters.Add("@CLIENT", SqlDbType.VarChar, 20).Value = client;
            cmd.Parameters.Add("@ORDRE", SqlDbType.VarChar, 2).Value = ordre;

            DBBase.SetDBNullParametre(cmd.Parameters);
            try
            {
                if (cn.State == ConnectionState.Closed)
                    cn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);
                return Entities.GetEntityListFromQuery<CsLclient>(dt);
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
        public string ValiderAchatimbreDemande(CsDemande laDemande, bool AvecTransmission)
        {
            SqlCommand laCommande = DBBase.InitTransaction(ConnectionString);
            try
            {
                #region Coutdemande
                if (laDemande.LstCoutDemande != null && laDemande.LstCoutDemande.Count != 0)
                {
                    Delete_RubriqueDemande(laDemande.LaDemande.NUMDEM, laDemande.LaDemande.PK_ID, laCommande);
                    foreach (CsDemandeDetailCout item in laDemande.LstCoutDemande)
                    {
                        item.FK_IDDEMANDE = laDemande.LaDemande.PK_ID;
                        item.NUMDEM = laDemande.LaDemande.NUMDEM;
                        InsertOrUpdateRubriqueDemande(item, laCommande);
                    }
                }
                #endregion
                if (AvecTransmission)
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

        private void  EcrirePieceJointe(CsDemande laDemande, SqlCommand laCommande)
        {
            try
            {
                if (laDemande.ObjetScanne != null && laDemande.ObjetScanne.Count != 0)
                {
                    foreach (ObjDOCUMENTSCANNE item in laDemande.ObjetScanne)
                    {
                        if (item.ISNEW == true)
                        {
                            string[] ext = item.NOMDUFICHIER.Split('.');
                            //string NomDuFichier = laDemande.LaDemande.NUMDEM + "_" + item.CODETYPEDOC + "." + ext[1];
                            string CheminSave = Enumere.CheminImpressionServeur + "\\" + laDemande.LaDemande.NUMDEM + "_" + item.CODETYPEDOC + "." + ext[1];
                            item.NOMDUFICHIER = laDemande.LaDemande.NUMDEM + "_" + item.CODETYPEDOC + "." + ext[1];
                            item.CHEMINCOPY = Enumere.CheminImpressionServeur + "\\" + laDemande.LaDemande.NUMDEM + "_" + item.CODETYPEDOC + ".zip";

                            //File.Copy(Path.Combine(item.CHEMININIT), Path.Combine(item.CHEMINCOPY), true);

                            CreateFiles(item.CONTENU, item.CHEMINCOPY, CheminSave);
                            item.FK_IDDEMANDE = laDemande.LaDemande.PK_ID;
                            item.CONTENU = null ;
                            InsertOrUpdateDocumentScane(item, laCommande);
                        }
                        else if (item.ISTOREMOVE == true)
                        {
                            File.Delete(Path.Combine(item.CHEMINCOPY));
                            Delete_DocumentScane(item, laCommande);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        
        }
        public  ObjDOCUMENTSCANNE OuvrirePieceJointe(ObjDOCUMENTSCANNE ObjetScanne)
        {
            try
            {
                if (ObjetScanne != null && !string.IsNullOrEmpty(ObjetScanne.CHEMINCOPY))
                {
                    using (FileStream fs = File.Open(ObjetScanne.CHEMINCOPY, FileMode.Open, FileAccess.Read, FileShare.None))
                    {
                        var memoryStream = new MemoryStream();
                        fs.CopyTo(memoryStream);
                        ObjetScanne.CONTENU = memoryStream.GetBuffer();
                    }
                }
                return ObjetScanne;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        //public  void CreateFiles(byte[] Document, string CheminFichier, string NomDuFichier)
        //{
        //    try
        //    {
        //        using (FileStream fs = new FileStream(@CheminFichier, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
        //        {
        //            //fs.Write(Document, 0, Document.Length);
        //            //fs.Flush();
        //            //fs.Close();
                   
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}



        public void CreateFiles(byte[] Document, string CheminFichier,string CheminTemp)
        {
            try
            {

                CompresssFile(Document, CheminFichier, CheminTemp);    
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        public static void CompresssFile(byte[] Document, String OUT_FileName, string CheminTemp)
        {
          
            var memoryStream = new MemoryStream(Document);

            FileStream fsDest = new FileStream(OUT_FileName, FileMode.Create);

            GZipStream compStream = new GZipStream(fsDest, CompressionMode.Compress);
            int theByte = memoryStream.ReadByte();

            while (theByte != -1)
            {
                compStream.WriteByte((byte)theByte);
                theByte = memoryStream.ReadByte();
            }

            compStream.Close();
            memoryStream.Close();
            fsDest.Close();
        }

        public static byte[] UncompressFile(String IN_FilePath)
        {
            MemoryStream  destFile = new MemoryStream();

            FileStream sourceFile = new FileStream(IN_FilePath, FileMode.Open);
            GZipStream decompStream = new GZipStream(sourceFile, CompressionMode.Decompress);
            int theByte = decompStream.ReadByte();
            while (theByte != -1)
            {
                destFile.WriteByte((byte)theByte);
                theByte = decompStream.ReadByte();
            }
            decompStream.Close();
            sourceFile.Close();
            destFile.Close();
            return  destFile.GetBuffer();
        }

        public static void UncompressFile(String IN_FilePath, String OUT_FilePath)
        {
            FileStream sourceFile = new FileStream(IN_FilePath, FileMode.Open);
            FileStream destFile = new FileStream(OUT_FilePath, FileMode.Create);

            GZipStream decompStream = new GZipStream(sourceFile, CompressionMode.Decompress);

            int theByte = decompStream.ReadByte();

            while (theByte != -1)
            {
                destFile.WriteByte((byte)theByte);
                theByte = decompStream.ReadByte();
            }

            decompStream.Close();
            sourceFile.Close();
            destFile.Close();
        }
      
        #region From Entity : Stephen 26-01-2019

        public List<CsForfait> ChargerForfait()//TriggerMenuView 28-01
        {
            try
            {
                //DataTable dt = Galatee.Entity.Model.AccueilProcedures.RetourneForfait();
                DataTable dt = DB_ParametresGeneraux.SelectAllDonneReference("FORFAIT");
                return Entities.GetEntityListFromQuery<CsForfait>(dt);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public List<CsRubriqueDevis> ChargerRubrique()//TriggerMenuView 28-01                                                                                                                                                                                    
        {
            try
            {
                //DataTable dt = Galatee.Entity.Model.AccueilProcedures.ChargerRubrique();
                //DataTable dt = DB_ParametresGeneraux.SelectAllDonneReference("RUBRIQUEDEVIS");
                //return Entities.GetEntityListFromQuery<CsRubriqueDevis>(dt);

                DB_ParametresGeneraux db = new DB_ParametresGeneraux();
                List<CsRubriqueDevis> _LstItem = new List<CsRubriqueDevis>();
                _LstItem = db.ChargerRurique();
                return _LstItem;

            }
            catch (Exception ex)
            {
                throw new Exception("ChargerRubrique :" + ex.Message);
            }
        }
        public List<CsCodeTaxeApplication> RetourneTousApplicationTaxe()//TriggerMenuView 28-01
        {
            try
            {
                DB_ParametresGeneraux db = new DB_ParametresGeneraux();
                List<CsCodeTaxeApplication> _LstItem = new List<CsCodeTaxeApplication>();
                _LstItem = db.ChargerApplicationTaxe();
                return _LstItem;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public List<CsFrequence> RetourneTousFrequence()//TriggerMenuView 28-01
        {
            try
            {
                //DataTable dt = Galatee.Entity.Model.CommonProcedures.RetourneTousFrequence();
                DataTable dt = DB_ParametresGeneraux.SelectAllDonneReference("PERIODICITE");
                return Entities.GetEntityListFromQuery<CsFrequence>(dt);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public List<CsMois> RetourneTousMois()//TriggerMenuView 28-01
        {
            try 
            {
                //DataTable dt = Galatee.Entity.Model.CommonProcedures.RetourneTousMois();
                DataTable dt = DB_ParametresGeneraux.SelectAllDonneReference("MOIS");
                return Entities.GetEntityListFromQuery<CsMois>(dt);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public List<CsTarif> ChargerTarif()//TriggerMenuView 28-01
        {           
            try
            {
                //DataTable dt = Galatee.Entity.Model.AccueilProcedures.RetourneTarif();
                DataTable dt = DB_ParametresGeneraux.SelectAllDonneReference("TYPETARIF");
                return Entities.GetEntityListFromQuery<CsTarif>(dt);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public List<CsModepaiement> RetourneCodeModePayement()
        {
            try
            {
                //DataTable dt = Galatee.Entity.Model.AccueilProcedures.RetourneCodeModePayement();
                DataTable dt = DB_ParametresGeneraux.SelectAllDonneReference("MODEPAIEMENT");
                return Entities.GetEntityListFromQuery<CsModepaiement>(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CsCommune> ChargerLesCommune()//TriggerMenuView 28-01
        {
            //cmd.CommandText = "SPX_GUI_RETOURNE_COMMUNE";
            try
            {
                //DataTable dt = Galatee.Entity.Model.CommonProcedures.RetourneListeCommune();                
                DataTable dt = DB_ParametresGeneraux.SelectAllDonneReference("COMMUNE");
                return Entities.GetEntityListFromQuery<CsCommune>(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CsQuartier> ChargerLesQartiers()//TriggerMenuView 28-01
        {
            //cmd.CommandText = "SPX_GUI_RETOURNE_QUARTIER";
            try
            {
                //DataTable dt = Galatee.Entity.Model.CommonProcedures.RetourneListeQuartier();
                DataTable dt = DB_ParametresGeneraux.SelectAllDonneReference("QUARTIER");
                return Entities.GetEntityListFromQuery<CsQuartier>(dt);
            }
            catch (Exception ex)
            {
                throw new Exception(cmd.CommandText + ":" + ex.Message);
            }
        }
        public List<CsRues> ChargerLesRueDesSecteur()//TriggerMenuView 28-01
        {
            try
            {
                //DataTable dt = Galatee.Entity.Model.CommonProcedures.RetourneListeRue();
                DataTable dt = DB_ParametresGeneraux.SelectAllDonneReference("RUES");
                return Entities.GetEntityListFromQuery<CsRues>(dt);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public List<CsTournee> ChargerLesTournees()//TriggerMenuView 28-01
        {
            try
            {
                List<CsTournee> _LstTrneeReleveur = new List<CsTournee>();

                DataTable dt = Galatee.Entity.Model.CommonProcedures.RetourneListeTournee();     
                _LstTrneeReleveur = Entities.GetEntityListFromQuery<CsTournee>(dt);
               
                //DB_ParametresGeneraux db = new DB_ParametresGeneraux();
                //_LstTrneeReleveur = db.SelectListeTourneeReleveur();

                return _LstTrneeReleveur;
               
            }
            catch (Exception ex)
            {
                throw new Exception(cmd.CommandText + ":" + ex.Message);
            }
        }
        public List<CsSecteur> ChargerLesSecteurs()//TriggerMenuView 28-01
        {
            try
            {
                //DataTable dt = Galatee.Entity.Model.AccueilProcedures.RetourneListeSecteur();
                DataTable dt = DB_ParametresGeneraux.SelectAllDonneReference("SECTEUR");
                return Entities.GetEntityListFromQuery<CsSecteur>(dt);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public List<CsCentre> ChargerLesDonneesDesSite(bool IsIncrementeDemandeNum)//TriggerMenuView 28-01
        {
            try
            {
                DB_ParametresGeneraux db = new DB_ParametresGeneraux();

                //DataTable dt = Galatee.Entity.Model.AccueilProcedures.RetourneDonneesSite();
                //DataTable dt = DB_ParametresGeneraux.SelectAllDonneReference("CENTRE");
                List<CsCentre> _LstCentre = db.RetourneCentreduSite();
                var lstClientFactureDistnct = _LstCentre.Select(t => new { t.FK_IDCODESITE, t.CODESITE }).Distinct().ToList();
                _LstCentre.ForEach(t => t.NUMDEM = t.NUMERODEMANDE.ToString());
                if (IsIncrementeDemandeNum)
                {
                    foreach (var item in lstClientFactureDistnct)
                        Galatee.Entity.Model.AccueilProcedures.IncrementNumDem(item.FK_IDCODESITE);
                }
                //DataTable dtp = Galatee.Entity.Model.AccueilProcedures.RetourneProduitSite();               
                //DataTable dtp = DB_ParametresGeneraux.SelectAllDonneReference("CENTRE");                
                List<CsProduit> _LstProduit = db.RetourneProduitduCentre();
                foreach(CsProduit i in _LstProduit)
                   i.PK_ID = i.FK_IDPRODUIT;
                foreach (CsCentre item in _LstCentre)             
                    item.LESPRODUITSDUSITE = _LstProduit.Where(t => t.FK_IDCENTRE == item.PK_ID).ToList();
                
                return _LstCentre;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CsEtapeDemande> ChargerEtapeDemande()//TriggerMenuView 28-01
        {
            //cmd.CommandText = "[SPX_GUI_RETOURNE_ETAPEDEMANDE]";
            try
            {
                //DataTable dt = Galatee.Entity.Model.AccueilProcedures.RetourneEtapeDemande();
                DataTable dt = DB_ParametresGeneraux.SelectAllDonneReference("ETAPEDEMANDE");
                return Entities.GetEntityListFromQuery<CsEtapeDemande>(dt);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public List<CsTdem> RetourneOptionDemande()//TriggerMenuView 28-01
        {
            //cmd.CommandText = "SPX_GUI_RETOURNE_OPTIONDEMANDE";
            try
            {
                //DataTable dt = Galatee.Entity.Model.AccueilProcedures.RetourneOptionDemande();
                DataTable dt = DB_ParametresGeneraux.SelectAllDonneReference("TYPEDEMANDE");
                return Entities.GetEntityListFromQuery<CsTdem>(dt);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public List<CsSite> RetourneTousSite()
        {
            try
            {
                //DataTable dt = Galatee.Entity.Model.CommonProcedures.RetourneTousSites();
                DataTable dt = DB_ParametresGeneraux.SelectAllDonneReference("SITE");
                return Entities.GetEntityListFromQuery<CsSite>(dt);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public List<CsTypeBranchement> RetourneTypeDeBranchement()//TriggerMenuView 28-01
        {
            try
            {
                DB_ParametresGeneraux db = new DB_ParametresGeneraux();
                List<CsTypeBranchement> _LstTypeBrt = new List<CsTypeBranchement>();
                _LstTypeBrt = db.RetourneTypedeBranchement();
                return _LstTypeBrt;
                //DataTable dt = Galatee.Entity.Model.CommonProcedures.RetourneTypeDeBranchement();RetourneTypedeBranchement
                //DataTable dt = DB_ParametresGeneraux.SelectAllDonneReference("TYPEBRANCHEMENTPARPRODUIT");
                //return Entities.GetEntityListFromQuery<CsTypeBranchement>(dt);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public List<CsParametreBranchement> RetourneParametreBranchement()
        {            
            try
            {
                //DataTable dt = Galatee.Entity.Model.AccueilProcedures.RetourneParametre();
                DataTable dt = DB_ParametresGeneraux.SelectAllDonneReference("PARAMETREBRANCHEMENT");
                return Entities.GetEntityListFromQuery<CsParametreBranchement>(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CsProduit> RetourneListeProduit()//TriggerMenuView 28-01
        {            
            try
            {
                //DataTable dt = Galatee.Entity.Model.CommonProcedures.RetourneTousProduit();
                DataTable dt = DB_ParametresGeneraux.SelectAllDonneReference("PRODUIT");
                return Entities.GetEntityListFromQuery<CsProduit>(dt);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public List<CsCategorieClient> RetourneCategorie()//TriggerMenuView 28-01
        {
            try
            {
                //DataTable dt = Galatee.Entity.Model.CommonProcedures.RetourneCategorieClient();
                DataTable dt = DB_ParametresGeneraux.SelectAllDonneReference("CATEGORIECLIENT");
                return Entities.GetEntityListFromQuery<CsCategorieClient>(dt);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public List<CsDenomination> RetourneListeDenominationAll()//TriggerMenuView 28-01
        {
            try
            {
                //DataTable dt = Galatee.Entity.Model.CommonProcedures.RetourneListeDenominationAll();
                DataTable dt = DB_ParametresGeneraux.SelectAllDonneReference("DENOMINATION");
                return Entities.GetEntityListFromQuery<CsDenomination>(dt);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public List<CsNationalite> RetourneNationnalite()//TriggerMenuView 28-01
        {
            try
            {
                //DataTable dt = Galatee.Entity.Model.CommonProcedures.RetourneNationnalite();
                DataTable dt = DB_ParametresGeneraux.SelectAllDonneReference("NATIONALITE");
                return Entities.GetEntityListFromQuery<CsNationalite>(dt);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public List<CsRegCli> RetourneTousCodeRegroupement()
        {
            try
            {
                //DataTable dt = Galatee.Entity.Model.CommonProcedures.RetourneTousCodeRegroupement();
                DataTable dt = DB_ParametresGeneraux.SelectAllDonneReference("REGROUPEMENT");
                List<CsRegCli> _LstItem = Entities.GetEntityListFromQuery<CsRegCli>(dt);
                foreach (CsRegCli i in _LstItem)
                    i.LIBELLE = i.NOM;
                return _LstItem;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public List<CsCodeConsomateur> RetourneCodeConsomateur()//TriggerMenuView 28-01
        {
            try
            {
                //DataTable dt = Galatee.Entity.Model.CommonProcedures.RetourneCodeConsomateur();
                DataTable dt = DB_ParametresGeneraux.SelectAllDonneReference("CODECONSOMMATEUR");
                return Entities.GetEntityListFromQuery<CsCodeConsomateur>(dt);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public List<CsCadran> RetourneToutCadran()//TriggerMenuView 28-01
        {
            try
            {
                //DataTable dt = Galatee.Entity.Model.AccueilProcedures.RetourneToutCadran();
                DataTable dt = DB_ParametresGeneraux.SelectAllDonneReference("CATEGORIECLIENT");
                return Entities.GetEntityListFromQuery<CsCadran>(dt);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public List<CsMarqueCompteur> RetourneToutMarque()//TriggerMenuView 28-01
        {
            try
            {
                //DataTable dt = Galatee.Entity.Model.ParamProcedure.PARAM_MARQUECOMPTEUR_RETOURNE();
                DataTable dt = DB_ParametresGeneraux.SelectAllDonneReference("MARQUECOMPTEUR");
                return Entities.GetEntityListFromQuery<CsMarqueCompteur>(dt);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public List<CsTcompteur> ChargerType()//TriggerMenuView 28-01
        {
            try
            {
                //DataTable dt = Galatee.Entity.Model.AccueilProcedures.RetourneTypeCompteur();
                DataTable dt = DB_ParametresGeneraux.SelectAllDonneReference("TYPECOMPTEUR");
                return Entities.GetEntityListFromQuery<CsTcompteur>(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CsCtax> RetourneListeTaxe()//TriggerMenuView 28-01
        {
            try
            {
                //DataTable dt = Galatee.Entity.Model.CommonProcedures.RetourneTaxe();
                DataTable dt = DB_ParametresGeneraux.SelectAllDonneReference("TAXE");
                List<CsCtax> _listItem = Entities.GetEntityListFromQuery<CsCtax>(dt);
                return _listItem.Where(p => p.FINAPPLICATION >= System.DateTime.Today.Date).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CsCoutDemande> ChargerCoutDemande()//TriggerMenuView 28-01
        {
            try
            {
                //DataTable dt = Galatee.Entity.Model.AccueilProcedures.ChargerCoutDemande();
                //DataTable dt = DB_ParametresGeneraux.SelectAllDonneReference("COUTDEMANDE");
                //return Entities.GetEntityListFromQuery<CsCoutDemande>(dt);

                DB_ParametresGeneraux db = new DB_ParametresGeneraux();
                List<CsCoutDemande> _LstItem = new List<CsCoutDemande>();
                _LstItem = db.ChargerCoutDemande();
                return _LstItem;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public List<CsCoper> RetourneTousCoper()//TriggerMenuView 28-01
        {
            try
            {
                //DataTable dt = Galatee.Entity.Model.CommonProcedures.RetourneTousCoper();
                DataTable dt = DB_ParametresGeneraux.SelectAllDonneReference("COPER");
                return Entities.GetEntityListFromQuery<CsCoper>(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CsCasind> RetourneListeDesCas()//TriggerMenuView 28-01
        {
            try
            {
                //DataTable dt = Galatee.Entity.Model.ParamProcedure.PARAM_CASIND_RETOURNE();
                //DataTable dt = DB_ParametresGeneraux.SelectAllDonneReference("CASIND");
                //return Entities.GetEntityListFromQuery<CsCasind>(dt);

                DB_ParametresGeneraux db = new DB_ParametresGeneraux();
                List<CsCasind> _LstItem = new List<CsCasind>();
                _LstItem = db.RetourneCasind();
                return _LstItem;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
        public List<CsDepart> ChargerDepartHTA()//TriggerMenuView 28-01
        {
            try
            {
                //DataTable dt = Galatee.Entity.Model.AccueilProcedures.RetourneDepartHTA();
                //DataTable dt = DB_ParametresGeneraux.SelectAllDonneReference("DEPARTHTA");
                //return Entities.GetEntityListFromQuery<CsDepart>(dt);

                DB_ParametresGeneraux db = new DB_ParametresGeneraux();
                List<CsDepart> _LstItem = new List<CsDepart>();
                _LstItem = db.RetourneDepartHTA();
                return _LstItem;

            }
            catch (Exception ex)
            {
                throw new Exception("ChargerpDepartHTA :" + ex.Message);
            }
        }
        public List<CsPosteSource> ChargerPosteSource()//TriggerMenuView 28-01
        {
            try
            {
                //DataTable dt = Galatee.Entity.Model.AccueilProcedures.RetournePosteSource();
                //DataTable dt = DB_ParametresGeneraux.SelectAllDonneReference("POSTESOURCE");
                //return Entities.GetEntityListFromQuery<CsPosteSource>(dt);

                DB_ParametresGeneraux db = new DB_ParametresGeneraux();
                List<CsPosteSource> _LstItem = new List<CsPosteSource>();
                _LstItem = db.RetournePosteSource();
                return _LstItem;

            }
            catch (Exception ex)
            {
                throw new Exception("ChargerPosteSource :" + ex.Message);
            }
        }
        public List<CsPosteTransformation> ChargerPosteTransformateur()
        {
            try
            {
                //DataTable dt = Galatee.Entity.Model.AccueilProcedures.RetournePosteTransformateur();
                //DataTable dt = DB_ParametresGeneraux.SelectAllDonneReference("POSTETRANSFORMATION");
                //return Entities.GetEntityListFromQuery<CsPosteTransformation>(dt);
                DB_ParametresGeneraux db = new DB_ParametresGeneraux();
                List<CsPosteTransformation> _LstItem = new List<CsPosteTransformation>();
                _LstItem = db.RetournePosteTransformateur();
                return _LstItem;

            }
            catch (Exception ex)
            {
                throw new Exception("ChargerPosteTransformateur :" + ex.Message);
            }
        }
        public List<CsOrigineLot> RetourneOrigine()//TriggerMenuView 28-01
        {
            try
            {
                //DataTable dt = Galatee.Entity.Model.AccueilProcedures.RetourneOrigine();
                DataTable dt = DB_ParametresGeneraux.SelectAllDonneReference("ORIGINELOT");
                return Entities.GetEntityListFromQuery<CsOrigineLot>(dt);
            }
            catch (Exception ex)
            {
                throw new Exception(cmd.CommandText + ":" + ex.Message);
            }
        }
        public List<CsTypeLot> RetourneTypeLot()//TriggerMenuView 28-01
        {
            try
            {
                //DataTable dt = Galatee.Entity.Model.AccueilProcedures.RetourneTypeLot();
                //DataTable dt = DB_ParametresGeneraux.SelectAllDonneReference("TYPELOT");
                //return Entities.GetEntityListFromQuery<CsTypeLot>(dt);

                DB_ParametresGeneraux db = new DB_ParametresGeneraux();
                List<CsTypeLot> _LstItem = new List<CsTypeLot>();
                _LstItem = db.RetourneTypeLot();
                return _LstItem;

            }
            catch (Exception ex)
            {
                throw new Exception(cmd.CommandText + ":" + ex.Message);
            }
        }
        public List<CsDiametreBranchement> RetourneDiametreBranchement()
        {
            try
            {
                //DataTable dt = Galatee.Entity.Model.CommonProcedures.RetourneDiametreBranchement();
                DataTable dt = DB_ParametresGeneraux.SelectAllDonneReference("TYPEBRANCHEMENT");
                return Entities.GetEntityListFromQuery<CsDiametreBranchement>(dt);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        //TriggerMenuView 28-01
        public List<CsPuissance> ChargerPuissanceSouscrite()
        {
            try
            {
                //DataTable dt = Galatee.Entity.Model.AccueilProcedures.RetournePuissanceSouscrite();
              return  DB_ParametresGeneraux.RetournePuissanceSouscrite();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CsTypeComptage> ChargerTypeComptage()
        {
            try
            {
                //DataTable dt = Galatee.Entity.Model.AccueilProcedures.ChargerTypeComptage();
                DataTable dt = DB_ParametresGeneraux.SelectAllDonneReference("TYPECOMPTAGE");
                return Entities.GetEntityListFromQuery<CsTypeComptage>(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CsTarif> ChargerTarifPuissance()
        {
            try
            {
                //DataTable dt = Galatee.entity.Model.AccueilProcedures.RetourneTarifPuissance();
                DataTable dt = DB_ParametresGeneraux.SelectAllDonneReference("TYPETARIF");
                return Entities.GetEntityListFromQuery<CsTarif>(dt);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public List<CsFermable> RetourneFermable()
        {
            try
            {
                //DataTable dt = Galatee.Entity.Model.CommonProcedures.RetourneFermable();
                DataTable dt = DB_ParametresGeneraux.SelectAllDonneReference("RELANCE");
                return Entities.GetEntityListFromQuery<CsFermable>(dt);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public List<CsPuissance> ChargerPuissanceInstalle()
        {
            try
            {
                //DataTable dt = Galatee.Entity.Model.CommonProcedures.RetournePuissanceInstalle();
                //DataTable dt = DB_ParametresGeneraux.SelectAllDonneReference("PUISSANCEINSTALLEE");
                //return Entities.GetEntityListFromQuery<CsPuissance>(dt);

                DB_ParametresGeneraux db = new DB_ParametresGeneraux();
                List<CsPuissance> _LstItem = new List<CsPuissance>();
                _LstItem = db.RetournePuissanceInstallee();
                return _LstItem;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CsRegCli> RetourneCodeRegroupement()
        {
            try
            {
                //DataTable dt = Galatee.Entity.Model.CommonProcedures.RetourneTousCodeRegroupement();
                //DataTable dt = DB_ParametresGeneraux.SelectAllDonneReference("REGROUPEMENT");
                //return Entities.GetEntityListFromQuery<CsRegCli>(dt);
                DB_ParametresGeneraux db = new DB_ParametresGeneraux();
                List<CsRegCli> _LstItem = new List<CsRegCli>();
                _LstItem = db.RetourneAllCodeRegroupement();
                return _LstItem;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public List<CsTarif> ChargerTarifParReglageCompteur()
        {
            try
            {
                //DataTable dt = Galatee.Entity.Model.CommonProcedures.ChargerTarifParReglageCompteur();
                //DataTable dt = DB_ParametresGeneraux.SelectAllDonneReference("TYPETARIFPARREGLAGECOMPTEUR");
                //return Entities.GetEntityListFromQuery<CsTarif>(dt);

                DB_ParametresGeneraux db = new DB_ParametresGeneraux();
                List<CsTarif> _LstItem = new List<CsTarif>();
                _LstItem = db.ChargerTarifparReglageCompteur();
                return _LstItem;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CsTarif> ChargerTarifParCategorie()
        {
            try
            {
                //DataTable dt = Galatee.Entity.Model.CommonProcedures.ChargerTarifParCategorie();
                //DataTable dt = DB_ParametresGeneraux.SelectAllDonneReference("TYPETARIFPARCATEGORIE");
                //return Entities.GetEntityListFromQuery<CsTarif>(dt);

                DB_ParametresGeneraux db = new DB_ParametresGeneraux();
                List<CsTarif> _LstItem = new List<CsTarif>();
                _LstItem = db.ChargerTarifparCategorie();
                return _LstItem;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CsPuissance> ChargerPuissanceReglageCompteur()
        {
            try
            {
                //DataTable dt = Galatee.Entity.Model.CommonProcedures.RetournePuissanceReglageCompteur();
                //DataTable dt = DB_ParametresGeneraux.SelectAllDonneReference("PUISSANCEINSTALLEE");
                //return Entities.GetEntityListFromQuery<CsPuissance>(dt);

                DB_ParametresGeneraux db = new DB_ParametresGeneraux();
                List<CsPuissance> _LstItem = new List<CsPuissance>();
                _LstItem = db.RetournePuissanceReglageCompteur();
                return _LstItem;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public List<ObjDOCUMENTSCANNE> Select_DocumentNonMigre()
        {
            cn = new SqlConnection(ConnectionString);

            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandTimeout = 1800;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SPX_ACC_SELECTDOCUMENTCONTENT_NONMIGRE";
            DBBase.SetDBNullParametre(cmd.Parameters);
            try
            {
                if (cn.State == ConnectionState.Closed)
                    cn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);
                return Entities.GetEntityListFromQuery<ObjDOCUMENTSCANNE>(dt);
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
        public void UpdateDocumentJoint(ObjDOCUMENTSCANNE laCan)
        {

            cn = new SqlConnection(ConnectionString);
            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandTimeout = 1800;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 3000;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SPX_TEMP_UPDATEFICHIERJOINT";
            if (cmd.Parameters != null && cmd.Parameters.Count != 0) cmd.Parameters.Clear();

            cmd.Parameters.Add("@PK_ID", SqlDbType.VarChar, Enumere.TailleCentre).Value = laCan.PK_ID;
            cmd.Parameters.Add("@CHEMINCOPY", SqlDbType.VarChar,100).Value = laCan.CHEMINCOPY ;
            cmd.Parameters.Add("@NOMDUFICHIER", SqlDbType.VarChar,200).Value = laCan.NOMDUFICHIER ;
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

    }
        #endregion



}


