using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Galatee.Entity.Model;
using Galatee.Structure;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Data;
using System.IO;
//using Galatee.Tools;

//using Microsoft.Reporting.WebForms;



namespace Galatee.DataAccess
{
    public class DBFRAUDE
    {

     
        private string ConnectionString;
        private SqlCommand cmd = null;
        public SqlCommand Cmd
        {
            get { return cmd; }
            set { cmd = value; }
        }
        private SqlConnection cn = null;

        public DBFRAUDE()
        {
            try
            {
                //ConnectionString = GalateeConfig.ConnectionStrings[Enumere.ConnexionGALADB].ConnectionString;
                //ConnectionString = Session.GetSqlConnexionString();
                //Abo07ConnectionString = Session.GetSqlConnexionStringAbo07();
                ConnectionString = Session.GetSqlConnexionString();

            }
            catch (Exception)
            {
                throw;
            }
        }
        public List<CsUsage> SelectAllUsage()
        {
            try
            {
                List<CsUsage> _lstUsage = new List<CsUsage>();
                DataTable dt = Galatee.Entity.Model.FraudeProcedure.RetourneUsage();
                _lstUsage = Entities.GetEntityListFromQuery<CsUsage>(dt);

                return _lstUsage;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public List<CsSourceControle> SelectAllSouscontrole()
        {
            try
            {
                List<CsSourceControle> _lstSousControle = new List<CsSourceControle>();
                DataTable dt = Galatee.Entity.Model.FraudeProcedure.RetourneListeSousControle();
                _lstSousControle = Entities.GetEntityListFromQuery<CsSourceControle>(dt);

                return _lstSousControle;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public List<CsMoyenDenomciation> SelectAllMoyenDenomciation()
        {
            try
            {
                List<CsMoyenDenomciation> _lstCsMoyenDenomciation = new List<CsMoyenDenomciation>();
                DataTable dt = Galatee.Entity.Model.FraudeProcedure.RetourneListeMoyendeDenonciation();
                _lstCsMoyenDenomciation = Entities.GetEntityListFromQuery<CsMoyenDenomciation>(dt);

                return _lstCsMoyenDenomciation;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public List<CsMArqueDisjoncteur> SelectAllMarqueDisjoncteur()
        {
            try
            {
                List<CsMArqueDisjoncteur> _lstCSTypeDisjoncteur = new List<CsMArqueDisjoncteur>();
                DataTable dt = Galatee.Entity.Model.FraudeProcedure.RetourneListeMarqueDisjoncteur();
                _lstCSTypeDisjoncteur = Entities.GetEntityListFromQuery<CsMArqueDisjoncteur>(dt);

                return _lstCSTypeDisjoncteur;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public List<CsPhaseCompteur> SelectAllPhaseCompteur()
        {
            try
            {
                List<CsPhaseCompteur> _lstCsPhaseCompteur = new List<CsPhaseCompteur>();
                DataTable dt = Galatee.Entity.Model.FraudeProcedure.RetourneListePhaseCompteur();
                _lstCsPhaseCompteur = Entities.GetEntityListFromQuery<CsPhaseCompteur>(dt);

                return _lstCsPhaseCompteur;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public List<CsTypeFraude> SelectAllTypeFraude()
        {
            try
            {
                List<CsTypeFraude> _lstCsOrganeFraude = new List<CsTypeFraude>();
                DataTable dt = Galatee.Entity.Model.FraudeProcedure.RetourneTypeDeFraude();
                _lstCsOrganeFraude = Entities.GetEntityListFromQuery<CsTypeFraude>(dt);

                return _lstCsOrganeFraude;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public List<CsActionSurCompteur> SelectAllActionSurCompteur()
        {
            try
            {
                List<CsActionSurCompteur> _sActionSurCompteur = new List<CsActionSurCompteur>();
                DataTable dt = Galatee.Entity.Model.FraudeProcedure.RetourneActionSurCompteur();
                _sActionSurCompteur = Entities.GetEntityListFromQuery<CsActionSurCompteur>(dt);

                return _sActionSurCompteur;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public List<CsOrganeFraude> SelectAllOrganeFraude()
        {
            try
            {
                List<CsOrganeFraude> _sOrganeFraude = new List<CsOrganeFraude>();
                DataTable dt = Galatee.Entity.Model.FraudeProcedure.RetourneOrganeFraude();
                _sOrganeFraude = Entities.GetEntityListFromQuery<CsOrganeFraude>(dt);

                return _sOrganeFraude;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public List<CsQualiteExpert> SelectAllQualiteExpert()
        {
            try
            {
                List<CsQualiteExpert> _sOrganeFraude = new List<CsQualiteExpert>();
                DataTable dt = Galatee.Entity.Model.FraudeProcedure.RetourneQualiteExpert();
                _sOrganeFraude = Entities.GetEntityListFromQuery<CsQualiteExpert>(dt);

                return _sOrganeFraude;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public List<CsDecisionfrd> SelectAllDecisionfrd()
        {
            try
            {
                List<CsDecisionfrd> _sDecisionfrd = new List<CsDecisionfrd>();
                DataTable dt = Galatee.Entity.Model.FraudeProcedure.RetourneDecisonfrd();
                _sDecisionfrd = Entities.GetEntityListFromQuery<CsDecisionfrd>(dt);

                return _sDecisionfrd;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        #region Client Iweb

        public List<CsClient> SelectAllClientIWebs()
        {
            try
            {
                List<CsClient> _lstCsClient = new List<CsClient>();
                DataTable dt = Galatee.Entity.Model.FraudeProcedure.RetourneListeMoyendeDenonciation();
                _lstCsClient = Entities.GetEntityListFromQuery<CsClient>(dt);

                return _lstCsClient;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<CsClient> RetourneClient(int fk_idcentre, string centre, string client,string Ordre)
        {
            //cmd.CommandText = "SPX_GUI_RETOURNE_ORDREMAX";
            try
            {

                List<CsClient> _Client = new List<CsClient>();
                DataTable dt = Galatee.Entity.Model.FraudeProcedure.RetourneClient(fk_idcentre, centre, client, Ordre);
                _Client = Entities.GetEntityListFromQuery<CsClient>(dt);

                return _Client;

            }
            catch (Exception ex)
            {
                throw ex;
            }


        }

       



        #endregion

        #region fraude
        public bool InsertFraude(CsFraude sFraude)
        {
            try
            {
                return Entities.InsertEntity<Galatee.Entity.Model.FRAUDE>(Entities.ConvertObject<Galatee.Entity.Model.FRAUDE, CsFraude>(sFraude));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool InsertLesFraude(List<CsFraude> sFraude)
        {
            try
            {
                return Entities.InsertEntity<Galatee.Entity.Model.FRAUDE>(Entities.ConvertObject<Galatee.Entity.Model.FRAUDE, CsFraude>(sFraude));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int InsertFraudeDenociateur(CsFraude sFraude, CsDenonciateur sDenonciateur,CsClientFraude ClientFrd)
        {
            try
            {
               // DENONCIATEUR DENONCIATEUR = new DENONCIATEUR();
               //DENONCIATEUR.CENTRE=


                using (galadbEntities context = new galadbEntities())
                        {
                            try
                            {
                              
                                    DENONCIATEUR Deno = Entities.ConvertObject<Galatee.Entity.Model.DENONCIATEUR, CsDenonciateur>(sDenonciateur);
                                    Entities.InsertEntity<Galatee.Entity.Model.DENONCIATEUR>(Deno);
                                    context.SaveChanges(); 
                                 
                                      
                                            sFraude.FK_IDDENONCIATEUR = Deno.PK_ID;
                                            sFraude.FK_IDDECISIONFRAUDE = null;
                                            sFraude.FicheTraitement = AccueilProcedures.GetNumDevis((int)ClientFrd.FK_IDCENTRE);;

                                            //CsAg Ag = Entities.ConvertObject<CsAg, AG>(context.AG.FirstOrDefault(p => p.CLIENT == ClientFrd.Client));
                                            DataTable dts = Galatee.Entity.Model.AccueilProcedures.RetourneAG(ClientFrd.FK_IDCENTRE.Value, ClientFrd.Centre, ClientFrd.Client, string.Empty);
                                            CsAg Ag = Entities.GetEntityFromQuery<CsAg>(dts);

                               //ClientFrd.FK_IDCENTRE = null;
                                            ClientFrd.FK_IDSITE = null;
                                            if (Ag != null)
                                            {
                                                ClientFrd.FK_IDCENTRE = Ag.FK_IDCENTRE;
                                                ClientFrd.Centre = Ag.CENTRE;
                                                ClientFrd.Commune = Ag.COMMUNE;
                                                ClientFrd.FK_IDCOMMUNE = Ag.FK_IDCOMMUNE;
                                                ClientFrd.FK_IDQUARTIER = Ag.FK_IDQUARTIER;
                                                ClientFrd.Quartier = Ag.QUARTIER;
                                                ClientFrd.FK_RUE = Ag.FK_IDRUE;
                                                ClientFrd.Rue = Ag.RUE;
                                                ClientFrd.FK_SECTEUR = Ag.FK_IDSECTEUR;
                                                ClientFrd.Secteur = Ag.SECTEUR;
                                            }
                                CLIENTFRAUDE CLIENTFRDE = Entities.ConvertObject<Galatee.Entity.Model.CLIENTFRAUDE, CsClientFraude>(ClientFrd);
                                Entities.InsertEntity<Galatee.Entity.Model.CLIENTFRAUDE>(CLIENTFRDE);
                                context.SaveChanges();
                                try
                                {
                                       
                              sFraude.FK_IDCLIENTFRAUDE = CLIENTFRDE.PK_ID;
                                         
                                     
                                }
                                catch (Exception)
                                {
                                    Entities.UpdateEntity<Galatee.Entity.Model.CLIENTFRAUDE>(CLIENTFRDE);

                                }
                                     
                                Entities.InsertEntity<Galatee.Entity.Model.FRAUDE>(Entities.ConvertObject<Galatee.Entity.Model.FRAUDE, CsFraude>(sFraude));

                             return  context.SaveChanges();
                            }
                            catch (Exception ex)
                            {

                                throw ex;
                            }

                        }
               
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool UpdateFraude(List<CsFraude> sFraude)
        {
            try
            {
                return Entities.UpdateEntity<Galatee.Entity.Model.FRAUDE>(Entities.ConvertObject<Galatee.Entity.Model.FRAUDE, CsFraude>(sFraude));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CsFraude> SelectAllFraude( )
        {
            try
            {
                List<CsFraude> _sFraude = new List<CsFraude>();
                DataTable dt = Galatee.Entity.Model.FraudeProcedure.RetourneFraude();
                _sFraude = Entities.GetEntityListFromQuery<CsFraude>(dt);

                return _sFraude;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        #endregion

        #region Denonciation
        public bool InsertDenonciation(List<CsDenonciateur> CsDenonciateur)
        {
            try
            {
                return Entities.InsertEntity<Galatee.Entity.Model.DENONCIATEUR>(Entities.ConvertObject<Galatee.Entity.Model.DENONCIATEUR, CsDenonciateur>(CsDenonciateur));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool UpdateDenonciation(List<CsDenonciateur> CsDenonciateur)
        {
            try
            {
                return Entities.UpdateEntity<Galatee.Entity.Model.DENONCIATEUR>(Entities.ConvertObject<Galatee.Entity.Model.DENONCIATEUR, CsDenonciateur>(CsDenonciateur));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Client Fraude

        public CsClientFraude InsertClientFraude(List<CsClientFraude> sClientFraude)
        {
            using (galadbEntities context = new galadbEntities())
            {
                try
                {
                    CsClientFraude ClientFrd = new CsClientFraude();

                    List<CLIENTFRAUDE> ClientFraude = Entities.ConvertObject<Galatee.Entity.Model.CLIENTFRAUDE, CsClientFraude>(sClientFraude);
                    Entities.InsertEntity<Galatee.Entity.Model.CLIENTFRAUDE>(ClientFraude);
                    context.SaveChanges();
                    ClientFrd = Entities.ConvertObject<CsClientFraude, CLIENTFRAUDE>(ClientFraude).FirstOrDefault();
                    //foreach (CLIENTFRAUDE items in ClientFraude)
                    //{
                    //    ClientFrd.PK_ID = items.PK_ID;
                    //    ClientFrd.Nomabon = items.Nomabon;
                    //    ClientFrd.Centre = items.Centre;
                    //    ClientFrd.Telephone = items.Telephone;
                    //    ClientFrd.Porte = items.Porte;
                    //    ClientFrd.FK_IDCENTRE = items.FK_IDCENTRE;
                    //    ClientFrd.FK_IDCOMMUNE = items.FK_IDCOMMUNE;
                    //    ClientFrd.FK_IDQUARTIER = items.FK_IDQUARTIER;
                    //    ClientFrd.FK_RUE = items.FK_RUE;
                    //    ClientFrd.FK_SECTEUR = items.FK_SECTEUR;
                    //    ClientFrd.Client = items.Client;
                    //    ClientFrd.Ordre = items.Ordre;
                    //    ClientFrd.Commune = items.Commune;
                    //    ClientFrd.Quartier = items.Quartier;
                    //    ClientFrd.Rue = items.Rue;
                    //    ClientFrd.Secteur = items.Secteur;
                    //    ClientFrd.ContratAbonnement = items.ContratAbonnement;
                    //    ClientFrd.ContratBranchement = items.ContratBranchement;
                    //    ClientFrd.DateContratAbonnement = items.DateContratAbonnement;
                    //    ClientFrd.DateContratBranchement = items.DateContratBranchement;

                    //}
                    return ClientFrd;
                   
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            
        }

        public bool UpdateClientFraude(List<CsClientFraude> sClientFraude)
        {
            try
            {
                return Entities.UpdateEntity<Galatee.Entity.Model.CLIENTFRAUDE>(Entities.ConvertObject<Galatee.Entity.Model.CLIENTFRAUDE, CsClientFraude>(sClientFraude));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CsClientFraude> SelectAllClientFraude(string identifUnique,int pkCentre)
        {
            try
            {
                List<CsClientFraude> _lstCsClient = new List<CsClientFraude>();
                DataTable dt = Galatee.Entity.Model.FraudeProcedure.RetourneListeClientFraude(identifUnique, pkCentre);
                _lstCsClient = Entities.GetEntityListFromQuery<CsClientFraude>(dt);

                return _lstCsClient;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        #endregion

        #region CONTROLE
        public bool InsertControle(List<CsControle> sControle)
        {
            try
            {
                return Entities.InsertEntity<Galatee.Entity.Model.CONTROLE>(Entities.ConvertObject<Galatee.Entity.Model.CONTROLE, CsControle>(sControle));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool UpdateControle(List<CsControle> sControle)
        {
            try
            {
                return Entities.UpdateEntity<Galatee.Entity.Model.CONTROLE>(Entities.ConvertObject<Galatee.Entity.Model.CONTROLE, CsControle>(sControle));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CsControle> SelectAllControle()
        {
            try
            {
                List<CsControle> _sControle = new List<CsControle>();
                DataTable dt = Galatee.Entity.Model.FraudeProcedure.RetourneControle();
                _sControle = Entities.GetEntityListFromQuery<CsControle>(dt);

                return _sControle;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion

        #region CONTROLEUR

        public bool InsertControleur(List<CsControleur> sControleur)
        {
            try
            {
                return Entities.InsertEntity<Galatee.Entity.Model.CONTROLEUR>(Entities.ConvertObject<Galatee.Entity.Model.CONTROLEUR, CsControleur>(sControleur));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool InsertControleurS(CsControleur sControleur)
        {
            try
            {
                return Entities.InsertEntity<Galatee.Entity.Model.CONTROLEUR>(Entities.ConvertObject<Galatee.Entity.Model.CONTROLEUR, CsControleur>(sControleur));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool UpdateControleur(List<CsControleur> sControleur)
        {
            try
            {
                return Entities.UpdateEntity<Galatee.Entity.Model.CONTROLEUR>(Entities.ConvertObject<Galatee.Entity.Model.CONTROLEUR, CsControleur>(sControleur));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CsControleur> SelectAllControleur()
        {
            try
            {
                List<CsControleur> _sOrganeFraude = new List<CsControleur>();
                DataTable dt = Galatee.Entity.Model.FraudeProcedure.RetourneControleur();
                _sOrganeFraude = Entities.GetEntityListFromQuery<CsControleur>(dt);

                return _sOrganeFraude;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion

        #region Demande Fraude
        public bool SelectDemandeFraude(CsDemandeFraude sDemandeFraude)
        {
            try
            {
              
                
                return Entities.InsertEntity<Galatee.Entity.Model.FRAUDE>(Entities.ConvertObject<Galatee.Entity.Model.FRAUDE, CsFraude>(sDemandeFraude.Fraude));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region COMPTEURFRAUDE

        public bool InsertCompteurFraude(List<CsCompteurFraude> sCompteurFraude)
        {
            try
            {
                return Entities.InsertEntity<Galatee.Entity.Model.COMPTEURFRAUDE>(Entities.ConvertObject<Galatee.Entity.Model.COMPTEURFRAUDE, CsCompteurFraude>(sCompteurFraude));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool UpdateCompteurFraude(List<CsCompteurFraude> sCompteurFraude)
        {
            try
            {
                return Entities.UpdateEntity<Galatee.Entity.Model.COMPTEURFRAUDE>(Entities.ConvertObject<Galatee.Entity.Model.COMPTEURFRAUDE, CsCompteurFraude>(sCompteurFraude));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CsCompteurFraude> SelectAllCompteurFraude()
        {
            try
            {
                List<CsCompteurFraude> _sCompteurFraude = new List<CsCompteurFraude>();
                DataTable dt = Galatee.Entity.Model.FraudeProcedure.RetourneCompteurFraude();
                _sCompteurFraude = Entities.GetEntityListFromQuery<CsCompteurFraude>(dt);

                return _sCompteurFraude;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion

      # region enregistrement

    public bool InsertControleFraude(CsDemandeFraude sDemandeFraude)
     {
            try
            {
              bool Resultat = false;
                int resultTransaction = -1;
                   using (galadbEntities transaction = new galadbEntities())
                {

                    FraudeProcedure.InsertionFraudeControle(sDemandeFraude, transaction);
                    resultTransaction = transaction.SaveChanges();
                };
            return (resultTransaction == -1 ? false : true);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

   public bool InsertionFraudeAudite(CsDemandeFraude sDemandeFraude)
    {
        try
        {
            bool Resultat = false;
            int resultTransaction = -1;
            using (galadbEntities transaction = new galadbEntities())
            {

                FraudeProcedure.InsertionFraudeAudite(sDemandeFraude, transaction);
                resultTransaction = transaction.SaveChanges();
            };
            return (resultTransaction == -1 ? false : true);
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }

   public bool InsertionFraudeConsommation(CsDemandeFraude sDemandeFraude)
   {
       try
       {
           bool Resultat = false;
           int resultTransaction = -1;
           using (galadbEntities transaction = new galadbEntities())
           {

               FraudeProcedure.InsertionFraudeConsommation(sDemandeFraude, transaction);
               resultTransaction = transaction.SaveChanges();
           };
           return (resultTransaction == -1 ? false : true);
       }
       catch (Exception ex)
       {
           throw ex;
       }

   }
      #endregion

    public string  ValiderDemandeFraude(CsDemandeFraude LaDemande)
    {
        try
        {
            
            
            string DemandeID = string.Empty;
            bool Resultat = false;
            int resultTransaction = -1;
            using (galadbEntities transaction = new galadbEntities())
            {

                try
                {

                    LaDemande.LaDemande.NUMDEM = AccueilProcedures.GetNumDevis(LaDemande.LaDemande);
                    LaDemande.Fraude.FicheTraitement = LaDemande.LaDemande.NUMDEM;
                    FraudeProcedure.InsertDemandeFraude(LaDemande, transaction);
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
                                    DemandeID = laDem.PK_ID + "." + LaDemande.LaDemande.NUMDEM;
                            };
                        }
                        else
                            DemandeID = LaDemande.LaDemande.PK_ID + "." + LaDemande.LaDemande.NUMDEM;

                    }
                }
                catch (Exception ex)
                {
                    
                    throw ex;
                }
                 
            };
            return DemandeID;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public CsDemandeFraude RetourneLaDemande(int fk_demande)
    {
        //cmd.CommandText = "SPX_GUI_RETOURNE_DEMANDE";
        CsDemandeFraude _LaDemande = new CsDemandeFraude();

        try
        {
            galadbEntities transaction = new galadbEntities();


            CsFraude _sFraude = new CsFraude();
            CsCompteurFraude _sCompteurFraude = new CsCompteurFraude();
            List<CsAppareilRecenseFrd> _sAppareilUtiliserFrd= new List<CsAppareilRecenseFrd>();
            CLIENTFRAUDE _sClientFraude = new CLIENTFRAUDE();
            DEMANDE _DEMANDE = new DEMANDE();
            CONTROLE _CONTROLE = new CONTROLE();
            DataTable dt = Galatee.Entity.Model.FraudeProcedure.RetourneFraudeDemande(fk_demande);
            _sFraude = Entities.GetEntityFromQuery<CsFraude>(dt);
            _LaDemande.Fraude = _sFraude;

            _sClientFraude = transaction.CLIENTFRAUDE.FirstOrDefault(p => p.PK_ID == _LaDemande.Fraude.FK_IDCLIENTFRAUDE);
          //  _sAppareilRecenseFrd =  Entities.GetEntityListFromQuery<CsAppareilRecenseFrd>(dts);
            _LaDemande.ClientFraude = Entities.ConvertObject<CsClientFraude, CLIENTFRAUDE>(_sClientFraude);
            _LaDemande.LaDemande = Entities.ConvertObject<CsDemandeBase, DEMANDE>(transaction.DEMANDE.FirstOrDefault(p => p.PK_ID == fk_demande));

            
            try
            {
                DataTable dtss = Galatee.Entity.Model.FraudeProcedure.RetourneCompteurFraude((int)_LaDemande.Fraude.FK_IDCLIENTFRAUDE);
                _LaDemande.CompteurFraude = Entities.GetEntityFromQuery<CsCompteurFraude>(dtss);
            }
            catch (Exception)
            {
              
                _LaDemande.CompteurFraude = null;
            }
            try
            {
            _LaDemande.Controle = Entities.ConvertObject<CsControle, CONTROLE>(transaction.CONTROLE.FirstOrDefault(p => p.FK_IDFRAUDE == _LaDemande.Fraude.Pk_ID));
            }
            catch (Exception ex )
            {

                _LaDemande.Controle = null;
            }
            try
            {
                DataTable dts = Galatee.Entity.Model.FraudeProcedure.RetourneAPPAREILRECENSE(_LaDemande.Controle.PK_ID);
                _LaDemande.AppareilRecenseFrd = Entities.GetEntityListFromQuery<CsAppareilRecenseFrd>(dts);

            }
            catch (Exception ex)
            {

                _LaDemande.AppareilRecenseFrd = null;
            }
            try
            {
                _LaDemande.Controleur = Entities.ConvertObject<CsControleur, CONTROLEUR>(transaction.CONTROLEUR.FirstOrDefault(p => p.FK_IDCONTROLE == _LaDemande.Controle.PK_ID));

            }
            catch (Exception ex)
            {

                _LaDemande.Controleur = null;
            }
            try
            {
                _LaDemande.AuditionFraude = Entities.ConvertObject<CsAuditionFraude, AUDITION>(transaction.AUDITION.FirstOrDefault(p => p.FK_IDFRAUDE == _LaDemande.Fraude.Pk_ID));

            }
            catch (Exception)
            {

                _LaDemande.AuditionFraude = null;
            }
            try
            {
                DataTable dts = Galatee.Entity.Model.FraudeProcedure.RetourneAPPAREILSUTILISE(_LaDemande.AuditionFraude.PK_ID);
                _LaDemande.AppareilUtiliserFrd = Entities.GetEntityListFromQuery<CsAppareilUtiliserFrd>(dts);

            }
            catch (Exception)
            {

                _LaDemande.AppareilUtiliserFrd = null;
            }
            try
            {
                _LaDemande.ConsommationFrd = Entities.ConvertObject<CsConsommationFrd, CONSOMMATION>(transaction.CONSOMMATION.FirstOrDefault(p => p.FK_IDFRAUDE == _LaDemande.Fraude.Pk_ID));

            }
            catch (Exception ex)
            {

                _LaDemande.ConsommationFrd = null;
            }
            try
            {
                DataTable dts = Galatee.Entity.Model.FraudeProcedure.RetourneTranche();
                _LaDemande.TrancheFraude = Entities.GetEntityListFromQuery<CsTrancheFraude>(dts).Where(c => c.FK_IDPHASECOMPTEUR == _LaDemande.CompteurFraude.FK_IDPHASECOMPTEUR && c.FK_IDPRODUIT == _LaDemande.CompteurFraude.FK_IDPRODUIT && c.FK_IDREGLAGECOMPTEUR == _LaDemande.CompteurFraude.FK_IDREGLAGE).ToList();

            }
            catch (Exception ex)
            {

                _LaDemande.TrancheFraude = null;
            }
            try
            {
                DataTable dts = Galatee.Entity.Model.FraudeProcedure.RetourneRegularisation();
                _LaDemande.Regularisation = Entities.GetEntityListFromQuery<CsRegularisation>(dts).Where(c => c.FK_IDPHASECOMPTEUR == _LaDemande.CompteurFraude.FK_IDPHASECOMPTEUR).ToList();

            }
            catch (Exception ex)
            {

                _LaDemande.Regularisation = null;
            }
            try
            {
                DataTable dts = Galatee.Entity.Model.FraudeProcedure.RetournePresentationEdm();
                _LaDemande.PrestastionEdm = Entities.GetEntityListFromQuery<CsPrestastionEdm>(dts);

            }
            catch (Exception ex)
            {

                _LaDemande.PrestastionEdm = null;
            }
            try
            {
                DataTable dts = Galatee.Entity.Model.FraudeProcedure.RetournePRESTATIONREMBOURSABLE();
                _LaDemande.PrestationRemboursable = Entities.GetEntityListFromQuery<CsPrestationRemboursable>(dts);

            }
            catch (Exception ex)
            {

                _LaDemande.PrestationRemboursable = null;
            }

            try
            {
                DataTable dts = AccueilProcedures.RetourneCoper(Enumere.CoperFactureFraude);
                _LaDemande.Coper = Entities.GetEntityFromQuery <CsCoper>(dts);

                //_LaDemande.Coper = Entities.ConvertObject<CsCoper, COPER>(transaction.COPER.FirstOrDefault(p => p.CODE == "002"));
            }
            catch (Exception ex)
            {

                _LaDemande.Coper = null;
            }
            try
            {
                DataTable dts = Galatee.Entity.Model.FraudeProcedure.RetourneMoisDejaFacturee(_LaDemande.ConsommationFrd.PK_ID);
                _LaDemande.MoisDejaFactures = Entities.GetEntityListFromQuery<CsMoisDejaFactures>(dts);

            }
            catch (Exception ex)
            {

                _LaDemande.MoisDejaFactures = null;
            }

            ///information Client Iweb
            ///

            try
            {
                DataTable dts = Galatee.Entity.Model.AccueilProcedures.RetourneAG(_LaDemande.ClientFraude.FK_IDCENTRE.Value, _LaDemande.ClientFraude.Centre, _LaDemande.ClientFraude.Client,string.Empty );
                _LaDemande.Ag = Entities.GetEntityFromQuery <CsAg>(dts);

                //_LaDemande.Ag = Entities.ConvertObject<CsAg, AG>(transaction.AG.FirstOrDefault(p => p.CLIENT == _LaDemande.ClientFraude.Client));
            }
            catch (Exception ex)
            {

                _LaDemande.Ag = null;
            }
            try
            {
                DataTable dts = Galatee.Entity.Model.AccueilProcedures.RetourneCanalisation(_LaDemande.ClientFraude.FK_IDCENTRE.Value ,_LaDemande.ClientFraude.Centre, _LaDemande.ClientFraude.Client, _LaDemande.CompteurFraude.PRODUIT, null);
                _LaDemande.Canalisation = Entities.GetEntityFromQuery<CsCanalisation>(dts);
            }
            catch (Exception ex)
            {

                _LaDemande.Canalisation = null;
            }

            try
            {
                DataTable dts = Galatee.Entity.Model.AccueilProcedures.RetourneAbon(_LaDemande.ClientFraude.FK_IDCENTRE.Value, _LaDemande.ClientFraude.Centre, _LaDemande.ClientFraude.Client, _LaDemande.ClientFraude.Ordre);
                _LaDemande.Abon = Entities.GetEntityFromQuery<CsAbon>(dts);
            }
            catch (Exception ex)
            {
                _LaDemande.Abon = null;
            }

            //try
            //{
            //    DataTable dtt = Galatee.Entity.Model.FraudeProcedure.RetourneEvenement(_LaDemande.ClientFraude.FK_IDCENTRE.Value, _LaDemande.ClientFraude.Centre, _LaDemande.ClientFraude.Client, _LaDemande.ClientFraude.Ordre);
            //    _LaDemande.Evenement = Entities.GetEntityListFromQuery<CsEvenement>(dtt);
            //}
            //catch (Exception)
            //{
                
            //    throw;
            //}
            transaction.Dispose();
            return _LaDemande;
             
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }


    public string ValiderDemandeControle(CsDemandeFraude LaDemande)
    {
        try
        {


            string DemandeID = string.Empty;
            bool Resultat = false;
            int resultTransaction = -1;
            using (galadbEntities transaction = new galadbEntities())
            {

                try
                {

                    //LaDemande.LaDemande.NUMDEM = AccueilProcedures.GetNumDevis(LaDemande.LaDemande);
                    //LaDemande.Fraude.FicheTraitement = LaDemande.LaDemande.NUMDEM;

                    LaDemande.Fraude.FicheTraitement = LaDemande.LaDemande.NUMDEM;
                    LaDemande.Fraude.FK_IDDEMANDE = LaDemande.LaDemande.PK_ID;

                    FraudeProcedure.InsertionFraudeControle(LaDemande, transaction);
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
                                    DemandeID = laDem.PK_ID + "." + LaDemande.LaDemande.NUMDEM;
                            };
                        }
                        else
                            DemandeID = LaDemande.LaDemande.PK_ID + "." + LaDemande.LaDemande.NUMDEM;

                    }
                }
                catch (Exception ex)
                {

                    throw ex;
                }

            };
            return DemandeID;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public string ValiderDemandeAudition(CsDemandeFraude LaDemande)
    {
        try
        {


            string DemandeID = string.Empty;
            bool Resultat = false;
            int resultTransaction = -1;
            using (galadbEntities transaction = new galadbEntities())
            {

                try
                {

                    //LaDemande.LaDemande.NUMDEM = AccueilProcedures.GetNumDevis(LaDemande.LaDemande);
                    //LaDemande.Fraude.FicheTraitement = LaDemande.LaDemande.NUMDEM;

                    LaDemande.Fraude.FicheTraitement = LaDemande.LaDemande.NUMDEM;
                    LaDemande.Fraude.FK_IDDEMANDE = LaDemande.LaDemande.PK_ID;

                    FraudeProcedure.InsertionFraudeAudite(LaDemande, transaction);
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
                                    DemandeID = laDem.PK_ID + "." + LaDemande.LaDemande.NUMDEM;
                            };
                        }
                        else
                            DemandeID = LaDemande.LaDemande.PK_ID + "." + LaDemande.LaDemande.NUMDEM;

                    }
                }
                catch (Exception ex)
                {

                    throw ex;
                }

            };
            return DemandeID;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public string ValiderDemandeConsommation(CsDemandeFraude LaDemande)
    {
        try
        {


            string DemandeID = string.Empty;
            bool Resultat = false;
            int resultTransaction = -1;
            using (galadbEntities transaction = new galadbEntities())
            {

                try
                {

                    //LaDemande.LaDemande.NUMDEM = AccueilProcedures.GetNumDevis(LaDemande.LaDemande);
                    //LaDemande.Fraude.FicheTraitement = LaDemande.LaDemande.NUMDEM;

                    LaDemande.Fraude.FicheTraitement = LaDemande.LaDemande.NUMDEM;
                    LaDemande.Fraude.FK_IDDEMANDE = LaDemande.LaDemande.PK_ID;

                    FraudeProcedure.InsertionFraudeConsommation(LaDemande, transaction);
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
                                    DemandeID = laDem.PK_ID + "." + LaDemande.LaDemande.NUMDEM;
                            };
                        }
                        else
                            DemandeID = LaDemande.LaDemande.PK_ID + "." + LaDemande.LaDemande.NUMDEM;

                    }
                }
                catch (Exception ex)
                {

                    throw ex;
                }

            };
            return DemandeID;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public string ValiderDemandeEmissionFacture(CsDemandeFraude LaDemande)
    {
        try
        {


            string DemandeID = string.Empty;
            bool Resultat = false;
            int resultTransaction = -1;
            using (galadbEntities transaction = new galadbEntities())
            {

                try
                {

                    //LaDemande.LaDemande.NUMDEM = AccueilProcedures.GetNumDevis(LaDemande.LaDemande);
                    //LaDemande.Fraude.FicheTraitement = LaDemande.LaDemande.NUMDEM;

                    LaDemande.Fraude.FicheTraitement = LaDemande.LaDemande.NUMDEM;
                    LaDemande.Fraude.FK_IDDEMANDE = LaDemande.LaDemande.PK_ID;

                    FraudeProcedure.InsertionFraudeEmissionFacture(LaDemande, transaction);
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
                                    DemandeID = laDem.PK_ID + "." + LaDemande.LaDemande.NUMDEM;
                            };
                        }
                        else
                            DemandeID = LaDemande.LaDemande.PK_ID + "." + LaDemande.LaDemande.NUMDEM;

                    }
                }
                catch (Exception ex)
                {

                    throw ex;
                }

            };
            return DemandeID;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public string ValiderDemandeControleIndex(CsDemandeFraude LaDemande)
    {
        try
        {


            string DemandeID = string.Empty;
            bool Resultat = false;
            int resultTransaction = -1;
            using (galadbEntities transaction = new galadbEntities())
            {

                try
                {

                    //LaDemande.LaDemande.NUMDEM = AccueilProcedures.GetNumDevis(LaDemande.LaDemande);
                    //LaDemande.Fraude.FicheTraitement = LaDemande.LaDemande.NUMDEM;

                    LaDemande.Fraude.FicheTraitement = LaDemande.LaDemande.NUMDEM;
                    LaDemande.Fraude.FK_IDDEMANDE = LaDemande.LaDemande.PK_ID;

                    FraudeProcedure.InsertionControleIndex(LaDemande, transaction);
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
                                    DemandeID = laDem.PK_ID + "." + LaDemande.LaDemande.NUMDEM;
                            };
                        }
                        else
                            DemandeID = LaDemande.LaDemande.PK_ID + "." + LaDemande.LaDemande.NUMDEM;

                    }
                }
                catch (Exception ex)
                {

                    throw ex;
                }

            };
            return DemandeID;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public List<CsEvenement> RetourneEvenement(int fk_idcentre, string centre, string client, string Ordre)
    {
        //cmd.CommandText = "SPX_GUI_RETOURNE_EVENEMENT";
        try
        {
            DataTable dt = Galatee.Entity.Model.FraudeProcedure.RetourneEvenement(fk_idcentre, centre, client, Ordre);
            return Entities.GetEntityListFromQuery<CsEvenement>(dt);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public List<CsEvenement> ConsommationByPeriodeMois(CsDemandeFraude laDemande,int mois,string periode)
    {
        cn = new SqlConnection(ConnectionString);

        cmd = new SqlCommand();
        cmd.Connection = cn;
        cmd.CommandTimeout = 3000;
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandText = "SPX_FRD_CONSOMMATIONPERIODE";

        cmd.Parameters.Add("@Client", SqlDbType.VarChar, 20).Value = laDemande.ClientFraude.Client;
        cmd.Parameters.Add("@FK_IDCENTRE", SqlDbType.VarChar, 8).Value = laDemande.ClientFraude.FK_IDCENTRE;
        cmd.Parameters.Add("@Centre", SqlDbType.VarChar, 63).Value = laDemande.ClientFraude.Centre;
        cmd.Parameters.Add("@Ordre", SqlDbType.VarChar, 2).Value = laDemande.ClientFraude.Ordre;
        cmd.Parameters.Add("@Mois", SqlDbType.Int).Value = mois;
        cmd.Parameters.Add("@Periode", SqlDbType.VarChar, 6).Value = periode;

        DBBase.SetDBNullParametre(cmd.Parameters);
        try
        {
            if (cn.State == ConnectionState.Closed)
                cn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
           
            List<CsEvenement> evenementFraude = new List<CsEvenement>();
            while (reader.Read())
            {
                CsEvenement c = new CsEvenement();
                c.CONSO = (Convert.IsDBNull(reader["CONSO"])) ? 0 : (int)reader["CONSO"];
                //c.VolumeCalcule = (Convert.IsDBNull(reader["CONSOTOTAL"])) ? 0 : (int)reader["CONSOTOTAL"];
                c.PERIODE = (Convert.IsDBNull(reader["PERIODE"])) ? String.Empty : (String)reader["PERIODE"];
                evenementFraude.Add(c);
            }
            return evenementFraude;
            //DataTable dt = new DataTable();
            //if (reader.Read())
            //    dt.Load(reader);

            //return dt;

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



   
    }
}
