using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Galatee.DataAccess;
using Galatee.Structure;
//using Microsoft.Reporting;
//using Microsoft.Reporting.WebForms;VerifieEtatCaisse
using System.Drawing.Printing;
using System.Reflection;
using System.Xml.Serialization;
using System.IO;
using System.Data;
using System.ServiceModel.Activation;
using System.Drawing.Imaging;
namespace WcfService
{
    // REMARQUE : vous pouvez utiliser la commande Renommer du menu Refactoriser pour changer le nom de classe "AccueilService" à la fois dans le code, le fichier svc et le fichier de configuration.
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)] 
    public class CaisseService : ICaisseService
    {
        int compteurIL;
        List<string> il;

        static Dictionary<string, List<CsReglement>> dicosReglement = new Dictionary<string, List<CsReglement>>();
        static Dictionary<string, List<CsDetailMoratoire>> dicosDetailMoratoire = new Dictionary<string, List<CsDetailMoratoire>>();
        static Dictionary<string, List<aDisconnection>> dicosDisconnecion = new Dictionary<string, List<aDisconnection>>();
        static Dictionary<string, List<aCampagne>> dicosCampagnes = new Dictionary<string, List<aCampagne>>();
        Dictionary<string, string> ProcessList = new Dictionary<string, string>();

        static Dictionary<string, string> parametreReglement = new Dictionary<string, string>();
        static Dictionary<string, string> parametreDisconnecion = new Dictionary<string, string>();

        List<Stream> m_streams = new List<Stream>();
        private int m_currentPageIndex;


        #region Gestion de la caisse

        public List<CsCtax> RetourneListeTaxe()
        {
            try
            {
                DBAccueil db = new DBAccueil();
                return db.RetourneListeTaxe();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }
        public List<CsLibelleTop > RetourneTousLibelleTop()
        {
            try
            {
                DBEncaissement db = new DBEncaissement();
                return db.RetourneTousLibelleTop();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public List<CsNature > RetourneNature()
        {
            try
            {
                DBEncaissement db = new DBEncaissement();
                return db.RetourneNature();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }
 

        public string NumeroFacture(int PkidCentre)
        {
            try
            {
                DBAccueil db = new DBAccueil();
                return db.NumeroFacture(PkidCentre);
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }
        public List<string> RetourneListeDesClientsReg(int IdCodeRegroupement)
        {
            try
            {
                DBEncaissement db = new DBEncaissement();
                List<CsClient> cl = db.RetourneListeDesClientsRegroupe(IdCodeRegroupement);
                List<string> clientReference = new List<string>();
                foreach (CsClient c in cl)
                    clientReference.Add(c.CENTRE  + c.REFCLIENT  + c.ORDRE );

                return clientReference;
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }

        }
        public List<CsLclient> RetourneListeFactureReg(int IdCodeRegroupement)
        {
            try
            {
                DBEncaissement db = new DBEncaissement();
                return db.RetourneListeFactureReg(IdCodeRegroupement);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public List<CsLclient> RetourneListeFactureNonSoldeByRegroupement(List<int> LstIdRegroupement,List<string> periode)
        {
            try
            {
                DBEncaissement db = new DBEncaissement();
               return  db.RetourneListeFactureNonSoldeByRegroupement(LstIdRegroupement, periode);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }

        }
    
          public List<CsLclient> RemplirfactureRegroupement(CsRegCli csRegCli, List<string> listperiode)
          {
              try
              {
                  List<CsLclient> ListeFacture = new List<CsLclient>();
                  List<CsCampagneGc> camp = new List<CsCampagneGc>();
                  if (listperiode.Count != 0)
                  {
                      foreach (var item in listperiode)
                          ListeFacture.AddRange(new DBEncaissement().RetourneListeFactureNonSoldeByRegroupementspx(csRegCli.PK_ID, item));
                  }
                  else
                      ListeFacture.AddRange(new DBEncaissement().RetourneListeFactureNonSoldeByRegroupementspx(csRegCli.PK_ID, null ));

                  return ListeFacture;
              }
              catch (Exception ex)
              {
                  ErrorManager.LogException(this, ex);
                  return null;
              }
          }

        public List<CsLclient> RetourneListeFacture(string centre, string Client, string ordre, int pForeignKey)
        {
            try
            {
                DBEncaissement db = new DBEncaissement();
                return db.RetourneListeFactureNonSolde(centre, Client, ordre, pForeignKey);
            }
            catch (Exception ex )
            {
                ErrorManager.LogException(this, ex);
                return null;
            }

        }
        public List<CsLclient> RetourneListeFactureNonSolde (CsClient leClient)
        {
            try
            {
                DBEncaissement db = new DBEncaissement();
                List<CsLclient> lstFactureClient = new List<CsLclient>();
                List<CsClient> lstClientReference = TestClientExist(leClient.CENTRE, leClient.REFCLIENT ,leClient.ORDRE );
                
                foreach (CsClient item in lstClientReference)
                    lstFactureClient.AddRange(Galatee.Tools.Utility.RetourneListCopy<CsLclient>(db.RetourneListeFactureNonSolde(item)));
                
                if (lstClientReference != null && lstClientReference.Count != 0 && lstFactureClient.Count == 0)
                {
                    foreach (var item in lstClientReference)
                    {
                        lstFactureClient.Add(new CsLclient()
                                    {
                                        CENTRE = item.CENTRE,
                                        CLIENT = item.REFCLIENT,
                                        ORDRE = item.ORDRE,
                                        NOM = item.NOMABON,
                                        ADRESSE  = item.ADRMAND1 ,
                                        LIBELLESITE = item.LIBELLESITE ,
                                        SOLDECLIENT = 0,
                                        FK_IDCENTRE = item.FK_IDCENTRE.Value,
                                        FK_IDCLIENT = item.PK_ID ,
                                        IsPAIEMENTANTICIPE = true ,
                                    });
                    }
                }
                if (lstClientReference == null || lstClientReference.Count == 0) return null;
                foreach (var item in lstClientReference)
                {
                    if (lstFactureClient.FirstOrDefault(t => t.FK_IDCLIENT == item.PK_ID) == null)
                    {
                        lstFactureClient.Add(new CsLclient()
                        {
                            CENTRE = item.CENTRE,
                            CLIENT = item.REFCLIENT,
                            ORDRE = item.ORDRE,
                            NOM = item.NOMABON,
                            ADRESSE = item.ADRMAND1,
                            LIBELLESITE = item.LIBELLESITE,
                            SOLDECLIENT = 0,
                            FK_IDCENTRE = item.FK_IDCENTRE.Value,
                            FK_IDCLIENT = item.PK_ID,
                            IsPAIEMENTANTICIPE = true,
                        });
                    }
                }
                return lstFactureClient;
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }

        }

        public List<CsLclient> RetourneListeFactureNonSoldeCaisse(CsClient leClient)
        {
            try
            {
                DBEncaissement db = new DBEncaissement();
                List<CsLclient> lstFactureClient = new List<CsLclient>();
                List<CsClient> lstClientReference = TestClientExist(leClient.CENTRE, leClient.REFCLIENT, leClient.ORDRE);

                foreach (CsClient item in lstClientReference)
                {
                    List<CsLclient> lstFacture = db.RetourneListeFactureNonSolde(item);
                    if( lstFacture != null )
                        lstFactureClient.AddRange(Galatee.Tools.Utility.RetourneListCopy<CsLclient>(lstFacture.Where(t => t.ISNONENCAISSABLE == null).ToList()));
                }
                if (lstClientReference != null && lstClientReference.Count != 0 && lstFactureClient.Count == 0)
                {
                    foreach (var item in lstClientReference)
                    {
                        lstFactureClient.Add(new CsLclient()
                        {
                            CENTRE = item.CENTRE,
                            CLIENT = item.REFCLIENT,
                            ORDRE = item.ORDRE,
                            NOM = item.NOMABON,
                            ADRESSE = item.ADRMAND1,
                            LIBELLESITE = item.LIBELLESITE,
                            SOLDECLIENT = 0,
                            FK_IDCENTRE = item.FK_IDCENTRE.Value,
                            FK_IDCLIENT = item.PK_ID,
                            IsPAIEMENTANTICIPE = true,
                        });
                    }
                }
                if (lstClientReference == null || lstClientReference.Count == 0) return null;
                foreach (var item in lstClientReference)
                {
                    if (lstFactureClient.FirstOrDefault(t => t.FK_IDCLIENT == item.PK_ID) == null)
                    {
                        lstFactureClient.Add(new CsLclient()
                        {
                            CENTRE = item.CENTRE,
                            CLIENT = item.REFCLIENT,
                            ORDRE = item.ORDRE,
                            NOM = item.NOMABON,
                            ADRESSE = item.ADRMAND1,
                            LIBELLESITE = item.LIBELLESITE,
                            SOLDECLIENT = 0,
                            FK_IDCENTRE = item.FK_IDCENTRE.Value,
                            FK_IDCLIENT = item.PK_ID,
                            IsPAIEMENTANTICIPE = true,
                        });
                    }
                }
                return lstFactureClient.Where(t => t.ISNONENCAISSABLE == null).ToList();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }

        }

        public List<CsLclient> RetourneListeFactureNonSoldeClient(List<CsClient> lesClient)
        {
            try
            {
                DBEncaissement db = new DBEncaissement();
                List<CsLclient> lstFactureClient = new List<CsLclient>();
                foreach (CsClient leClient in lesClient)
                {
                    List<CsClient> lstClientReference = new List<CsClient>();
                    List<CsClient> lstClientReferenceInit = TestClientExist(leClient.CENTRE, leClient.REFCLIENT, leClient.ORDRE);
                    lstClientReference.AddRange (lstClientReferenceInit.Where(t => t.FK_IDCENTRE == leClient.FK_IDCENTRE).ToList());
                    foreach (CsClient item in lstClientReference)
                    {
                        List<CsLclient> lstFacture = db.RetourneListeFactureNonSolde(item);
                        if (lstFacture != null)
                            lstFactureClient.AddRange(Galatee.Tools.Utility.RetourneListCopy<CsLclient>(lstFacture.Where(t => t.ISNONENCAISSABLE == null).ToList()));
                    }
                    if (lstClientReference != null && lstClientReference.Count != 0 && lstFactureClient.Count == 0)
                    {
                        foreach (var item in lstClientReference)
                        {
                            lstFactureClient.Add(new CsLclient()
                            {
                                CENTRE = item.CENTRE,
                                CLIENT = item.REFCLIENT,
                                ORDRE = item.ORDRE,
                                NOM = item.NOMABON,
                                ADRESSE = item.ADRMAND1,
                                LIBELLESITE = item.LIBELLESITE,
                                SOLDECLIENT = 0,
                                FK_IDCENTRE = item.FK_IDCENTRE.Value,
                                FK_IDCLIENT = item.PK_ID,
                                IsPAIEMENTANTICIPE = true,
                            });
                        }
                    }
                    if (lstClientReference == null || lstClientReference.Count == 0) return null;
                    foreach (var item in lstClientReference)
                    {
                        if (lstFactureClient.FirstOrDefault(t => t.FK_IDCLIENT == item.PK_ID) == null)
                        {
                            lstFactureClient.Add(new CsLclient()
                            {
                                CENTRE = item.CENTRE,
                                CLIENT = item.REFCLIENT,
                                ORDRE = item.ORDRE,
                                NOM = item.NOMABON,
                                ADRESSE = item.ADRMAND1,
                                LIBELLESITE = item.LIBELLESITE,
                                SOLDECLIENT = 0,
                                FK_IDCENTRE = item.FK_IDCENTRE.Value,
                                FK_IDCLIENT = item.PK_ID,
                                IsPAIEMENTANTICIPE = true,
                            });
                        }
                    } 
                }
                return lstFactureClient.Where(t => t.ISNONENCAISSABLE == null).ToList();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }

        }
        public  List<CsReglement> RetourneRecuDeManuelCaisseList(string Caisse)
        {
            try
            {
                DBEncaissement db = new DBEncaissement();
                return db.RetourneListeRecuManuelDeCaisse(Caisse);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }
        public List<CsHabilitationCaisse> RetourneSuppervisionCaisse(List<int> Centre,DateTime ? dateCaisse,bool IsEncours)
        {
            try
            {
                DBEncaissement db = new DBEncaissement();
                return db.RetourneSuppervisionCaisse(Centre, dateCaisse, IsEncours);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }
        public List<CsRegCli> ChargerListeCodeRegroupement()
        {
            try
            {
                DBEncaissement db = new DBEncaissement();
                return db.ChargerListeCodeRegroupement();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }

        }

        public List<CsCodeRegroupement > CodeRegroupement()
        {
            try
            {
                return new List<CsCodeRegroupement>();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }

        }

        public List<CsPayeur> ChargerListePayeur()
        {
            try
            {
                DBEncaissement db = new DBEncaissement();
                return db.ChargerListePayeur();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public List<CsLclient > ChargerListeFacturePayeur(CsPayeur lePayeur)
        {
            try
            {
                DBEncaissement db = new DBEncaissement();
                return db.RetourneListeFacturePayeur(lePayeur);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }
       
        public  List<CsBanque > RetourneListeDesBanques()
        {
            try
            {
                DBEncaissement db = new DBEncaissement();
                return db.RetourneListeDesBanques();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }

        }

        public CsLclient RetourneLeDevis(string NumDevis)
        {
            try
            {

                DBEncaissement db = new DBEncaissement();
                return db.RetourneLeDevis(NumDevis);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }



       public  List<CsMoratoire> RetourneListeMoratoire(string param1, string parm2, string parma3)
        {
            try
            {
                DBEncaissement db = new DBEncaissement();
                return db.RetourneListeMoratoire(param1, parm2, parma3);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }

        }

       public  List<CsLclient > RetourneRecuDeCaisseList(int Caisse)
        {
            try
            {
                DBEncaissement db = new DBEncaissement();
                return db.RetourneListeRecuDeCaisse_EnCour(Caisse);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }
       public List<CsLclient> RetourneRecuDeCaissePourAnnulation(int Caisse)
       {
           try
           {
               DBEncaissement db = new DBEncaissement();
               return db.RetourneListeRecuDeCaisse(Caisse);
           }
           catch (Exception ex)
           {
               ErrorManager.LogException(this, ex);
               return null;
           }
       }
       public List<CsClient> RetourneClientsParAmount(decimal amount, string sens,List<int> lstIdCentre)
        {
            try
            {
                DBEncaissement db = new DBEncaissement();
                List<CsClient> clients = new List<CsClient>();
                clients.AddRange(db.RetourneClientsParAmount(amount, sens, lstIdCentre));
                return clients;
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public List<CsClient> RetourneClientsParNoms(string names)
        {
            try
            {
                DBEncaissement db = new DBEncaissement();
                List<CsClient> clients = new List<CsClient>();
                List<CsClient> clientsNoDoublon = new List<CsClient>();
                clients.AddRange(db.RetourneClientsParNoms(names));
                foreach (CsClient c in clients)
                    if (clientsNoDoublon.Find(p => p.CENTRE == c.CENTRE && p.REFCLIENT == c.REFCLIENT && p.ORDRE == c.ORDRE) == null)
                        clientsNoDoublon.Add(c);
                return clientsNoDoublon;
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public List<CsClient> RetourneClientsParReference(string route, string sens)
        {
            try
            {
                DBEncaissement db = new DBEncaissement();
                List<CsClient> clients = new List<CsClient>();
                clients.AddRange(db.RetourneClientsParReference(route, sens));
                return clients;
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }

        }

        public List<CsFacture > RetourneClientBilling(string centre, string client, string ordre)
        {
            try
            {
                DBAccueil db = new DBAccueil();
                List<CsFacture> clients = new List<CsFacture>();
                clients.AddRange(db.RetourneFacture(centre, client, ordre));
                return clients;
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }
        public CsUserConnecte RetourneInfoMatriculeConnecte(string matricule, string pwd)
        {
            try
            {
                DBAuthentification db = new DBAuthentification();
                return db.RetourneInfoMatriculeConnecte(matricule);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }

        }

        public string VerifieEtatCaisse(string matricule, int? Fk_IdHabilCaisse)
        {
            try
            {
                DBEncaissement db = new DBEncaissement();
                return db.VerifieEtatCaisse(matricule, Fk_IdHabilCaisse);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }

        }
        public List<CsHabilitationCaisse > RetourneCaisseNonCloture(List<int> LstCentreCaisse)
        {
            try
            {
                DBEncaissement db = new DBEncaissement();
                return db.RetourneCaisseNonCloture(LstCentreCaisse);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }
        public CsHabilitationCaisse RetourneCaisseEnCours(int IdNumCaisse, int IdCaissier, DateTime DateDebut)
        {
            try
            {
                DBEncaissement db = new DBEncaissement();
                return db.RetourneCaisseEnCours( IdNumCaisse,  IdCaissier,  DateDebut);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public List<CsHabilitationCaisse> RetourneCaisseCloture(List<int> LstCentreCaisse)
        {
            try
            {
                DBEncaissement db = new DBEncaissement();
                return db.RetourneCaisseCloture(LstCentreCaisse);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }
        public List<CsHabilitationCaisse> RetourneListeCaisseHabilite(List<int> LstCentreCaisse)
        {
            try
            {
                DBEncaissement db = new DBEncaissement();
                return db.RetourneListeCaisseHabilite(LstCentreCaisse);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }
        public List<CsLclient> RetourneEncaissementPourValidationAnnulation(List<int> LstCentreCaisse)
        {
            try
            {
                DBEncaissement db = new DBEncaissement();
                return db.RetourneEncaissementPourValidationAnnulation(LstCentreCaisse);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }
        public List<CsCaisse> ListeCaisseDisponible(string Centre,string matricule)
        {
            try
            {

                DBEncaissement db = new DBEncaissement();
                return db.ListeCaisseDisponible(Centre, matricule);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }
        public List<CsCaisse> RetourneListeCaisse()
        {
            try
            {
                return new DB_CAISSE().SelectAllCaisse(); ;
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;

            }
        }

        public CsHabilitationCaisse HabiliterCaisse(CsHabilitationCaisse laCaisseHAbil)
        {
            try
            {
                DBEncaissement db = new DBEncaissement();
                return db.HabiliterCaisse(laCaisseHAbil);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public bool ReverserCaisse(List<CsReversementCaisse>  laCaisseHAbil)
        {
            try
            {
                DBEncaissement db = new DBEncaissement();
                return db.ReverserCaisse(laCaisseHAbil);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false ;
            }
        }
        public List<CsHabilitationCaisse> RetourneCaisseHabiliterCentre(CsCentre leCentre )
        {
            try
            {
                DBEncaissement db = new DBEncaissement();
                return db.RetourneCaisseHabiliterCentre(leCentre);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }


        public bool AjustementDeFondCaisse(string NumCaisse, string NouveauFond, string MoisCompt, string matricule)
        {
            try
            {
                DBEncaissement db = new DBEncaissement();
                return db.AjustementDeFondCaisse(NumCaisse, NouveauFond, MoisCompt,matricule);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }
        public decimal ? RetourneEncaissementDate(CsHabilitationCaisse laCaisse)
        {
            try
            {
                DBEncaissement db = new DBEncaissement();
                return db.RetourneEncaissementDate(laCaisse);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return 0;
            }
        }

        public CsHabilitationCaisse RetourneHabileCaisseReversement(CsHabilitationCaisse laCaisse)
        {
            try
            {
                DBEncaissement db = new DBEncaissement();
                return db.RetourneHabileCaisseReversement(laCaisse);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }
        public List<CsHabilitationCaisse> RetourneHabileCaisseNonReversement(CsHabilitationCaisse laCaisse)
        {
            try
            {
                DBEncaissement db = new DBEncaissement();
                return db.RetourneHabileCaisseNonReversement(laCaisse);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }



        public List<CsClient> TestClientExists(string centre, string Client, string ordre)
        {
            try
            {
                //ImpressionDirect imp = new ImpressionDirect();
                DBEncaissement db = new DBEncaissement();
                return db.TestClientExist(centre, Client, ordre);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }

        }

        public string RetourneNumCaisse(string MatriculeConnecter)
        {
            try
            {
                DBEncaissement db = new DBEncaissement();
                return db.RetourneNumCaisse(MatriculeConnecter);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }

        }

      public   bool? ValideOuvertureCaisse(DateTime? date, string matriculeCaisse,string numcaisse, string matriculeClerk, string raison, string saisipar, bool CaisseEstManuel)
        {
            try
            {
                DBEditionEncaissement dbEdition = new DBEditionEncaissement();
                return dbEdition.ValideOuvertureCaisse(date, matriculeCaisse,numcaisse, matriculeClerk, raison, saisipar, CaisseEstManuel);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }

        }
        public List<CsCaisse> ChargerCaisseDisponible()
        {
            try
            {
                DBEncaissement dbEncaisse = new DBEncaissement();
                return dbEncaisse.ChargerCaisseDisponible();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool? FermetureCaisse(CsHabilitationCaisse _laCaisseHabil)
        {
            try
            {
                DBEncaissement dbEncaisse = new DBEncaissement();
                return dbEncaisse.FermetureCaisse(_laCaisseHabil);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }

        }
      public List<CsReglement> RetourneListePaiementPourAnnulation(string caisse, string acquit, string matricule)
        {
            try
            {
                DBEditionEncaissement dbEdition = new DBEditionEncaissement();
                return dbEdition.RetourneListePaiementPourAnnulation(caisse, acquit, matricule);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }

        }

        public List<CsClient> TestClientExist(string centre, string client, string ordre)
        {
            try
            {
                DBEncaissement db = new DBEncaissement();
                return db.TestClientExist(centre, client, ordre);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }

        }
        public List<CsClient> TestClientExistByIdRegroupement(List<int> ListIdRegroupement)
        {
            try
            {
                DBEncaissement db = new DBEncaissement();
                return db.TestClientExistByIdRegroupement(ListIdRegroupement);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }

        }
      public   string RetourneNumFactureNaf(int ? Pkidcentre)
        {
            try
            {
                DBEncaissement db = new DBEncaissement();
                return db.RetourneNumFactureNaf(Pkidcentre.Value );
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }

        }



      public  List<CsLclient> RetourneEtatDeCaisse(CsHabilitationCaisse laCaisse)
        {
            try
            {
                DBEncaissement db = new DBEncaissement();
                return db.RetourneEtatDeCaisse(laCaisse);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }

        }

      public  CsReglement[] RetourneListePaiementDuRecu(string numcaisse, string numrecu, string matricle)
        {
            try
            {
                DBEncaissement db = new DBEncaissement();
                return db.RetourneListePaiementDuRecu(numcaisse, numrecu, matricle).ToArray();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }

        }

      public  bool? InsererEncaissement(List<CsLclient > ReglementAInserer, string Operation)
        {
            try
            {
                DBEncaissement db = new DBEncaissement();
                return db.InsererEncaissement(ReglementAInserer);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }

        }



 /*    public string RetourneNumeroRecu(int idcaisse)
        {
            try
            {
                DBEncaissement db = new DBEncaissement();
                return db.RetourneNumeroRecu(idcaisse);
            }
            catch (Exception ex )
            {
                ErrorManager.LogException(this, ex);
                return null;
            }

        }
*/


     public string RetourneNumeroRecu(int? idcaisse, string Matricule)
     {
         try
         {
             DBEncaissement db = new DBEncaissement();
             return db.RetourneNumeroRecu(idcaisse, Matricule);
         }
         catch (Exception ex)
         {
             ErrorManager.LogException(this, ex);
             return null;
         }

     }


       public bool? ValiderAnnuleEncaissement(List<CsLclient > ListFactureAnnule)
        {
            try
            {
                DBEncaissement db = new DBEncaissement();
                return db.ValiderAnnuleEncaissement(ListFactureAnnule);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

       public bool? RejeterAnnuleEncaissement(List<CsLclient> ListFactureAnnule)
       {
           try
           {
               DBEncaissement db = new DBEncaissement();
               return db.RejeterAnnuleEncaissement(ListFactureAnnule);
           }
           catch (Exception ex)
           {
               ErrorManager.LogException(this, ex);
               return null;
           }
       }

       public bool? DemandeAnnulationEncaissement(List<CsLclient > ListFactureAnnule)
        {

            try
            {
                DBEncaissement db = new DBEncaissement();
                return db.DemandeAnnuleEncaissement(ListFactureAnnule);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }

        }

       public bool? RetirerDemandeAnnulationEncaissement(List<CsLclient> ListFactureAnnule)
       {

           try
           {
               DBEncaissement db = new DBEncaissement();
               return db.RetirerDemandeAnnulationEncaissement(ListFactureAnnule);
           }
           catch (Exception ex)
           {
               ErrorManager.LogException(this, ex);
               return null;
           }

       }

       public List<CsLclient> RetourneDemande(string NumDemande, bool IsExtension)
       {
           try
           {

               DBEncaissement db = new DBEncaissement();
               return db.RetourneDemandeSpx(NumDemande, IsExtension);
           }
           catch (Exception ex)
           {
               ErrorManager.LogException(this, ex);
               return null;
           }
       }


      public   string VerifieCaisseDejaSaisie(string dateEncaissement, string matricule, ref string error)
        {
            try
            {

                DBEncaissement db = new DBEncaissement();
                return db.VerifieCaisseDejaSaisie(dateEncaissement, matricule);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }
 
        List<CsCoper> ICaisseService.RetourneListeDeCoperOD()
        {
            try
            {

                DBEncaissement db = new DBEncaissement();
                return db.RetourneListeDeCoperOD();
            }
            catch (Exception ex )
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        CsParametresGeneraux ICaisseService.RetourneListeTa58(string CodeTable)
        {
            try
            {

                DBEncaissement db = new DBEncaissement();
                return db.RetourneListeTa58(CodeTable);



            }
            catch (Exception ex )
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        List<CParametre> ICaisseService.ListeCaisse()
        {
            try
            {

                DBEncaissement db = new DBEncaissement();
                return db.ListeCaisse();
            }
            catch (Exception ex )
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }


        public List<CsFraisTimbre> RetourneListeTimbre(string ServerMode)
        {
            if (ServerMode == Enumere.EtatServerEnLigne)
                return new DBEncaissement().RetourneListeTimbre();
            else
                return new DBEncaissement().RetourneListeTimbreOffline();
        }
        public bool  MiseAJourDemandeReversement(CsDemandeReversement DemandeReversement,int Action)
        {
            return new DBEncaissement().MiseAJourDemandeReversement(DemandeReversement, Action);
        }
        public  List<CsDemandeReversement> RetourneDemandeReversement()
        {
            //return new DBEncaissement().RetourneDemandeReversement();
            return new List<CsDemandeReversement>();
        }
        public  List<CsLclient > RetourneEtatClient(int pk_id)
        {
            try
            {

                DBEncaissement db = new DBEncaissement();
                return db.RetourneListeFactureNonSolde(pk_id);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public  List<CsTypeTimbre> RetouneTypeTimbre()
        {
            try
            {
                DBEncaissement db = new DBEncaissement();
                return db.RetouneTypeTimbre();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public CsHabilitationCaisse  RetouneLaCaisseCourante(string matricule)
        {
            try
            {
                DBEncaissement db = new DBEncaissement();
                return db.RetouneLaCaisseCourante(matricule);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }


        public CsHabilitationCaisse RetouneLaCaisseCouranteInseree(string matricule, CsPoste poste)
        {
            try
            {
                DBEncaissement db = new DBEncaissement();
                return db.RetouneLaCaisseCourante(matricule, poste);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        #endregion

        #region  IMPRESSSION

        public bool? PrintReceipt(byte[] pRenderStream, bool landscape)
        {
            try
            {
                Stream stream = new MemoryStream(pRenderStream);
                m_streams.Add(stream);
                Print(landscape);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void Print(bool landscape)
        {
            try
            {
                if (m_streams == null || m_streams.Count == 0)
                    return;

                PrintDocument printDoc = new PrintDocument();
                PrinterSettings ps = new PrinterSettings();
                ps.DefaultPageSettings.Landscape = landscape;
                ps.DefaultPageSettings.PrinterSettings.DefaultPageSettings.Landscape = landscape;
                printDoc.PrinterSettings = ps;
                printDoc.PrintPage += new PrintPageEventHandler(PrintPage);
                printDoc.Print();

                foreach (Stream stream in m_streams)
                {
                    stream.Flush();
                    stream.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        void PrintPage(object sender, PrintPageEventArgs ev)
        {
            try
            {
                Metafile pageImage = new Metafile(m_streams[m_currentPageIndex]);
                ev.Graphics.DrawImage(pageImage, ev.PageBounds);
                m_currentPageIndex++;
                ev.HasMorePages = (m_currentPageIndex < m_streams.Count);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool DeleteDetailDsiconnectionFromWebPart(string key)
        {
            try
            {
                dicosDisconnecion.Remove(key);
                parametreReglement.Remove(key);
                return true;
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }
      
        public bool setDetailDsiconnectionInWebPart(string key, List<aDisconnection> objectList, Dictionary<string, string> parameters)
        {
            try
            {
                if (dicosDisconnecion.ContainsKey(key))
                    DeleteDetailDsiconnectionFromWebPart(key);

                dicosDisconnecion.Add(key, objectList);
                parametreReglement = parameters;
                return true;
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                           return false;
            }
        }

        public bool DeleteDetailCampagnesFromWebPart(string key)
        {
            try
            {
                dicosCampagnes.Remove(key);
                parametreReglement.Remove(key);
                return true;
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                               return false;
            }
        }

        public bool setDetailCampagneInWebPart(string key, List<aCampagne> objectList, Dictionary<string, string> parameters)
        {
            try
            {
                if (dicosCampagnes.ContainsKey(key))
                    DeleteDetailCampagnesFromWebPart(key);

                dicosCampagnes.Add(key, objectList);
                parametreReglement = parameters;
                return true;
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public List<CsDetailMoratoire> getDetailMoratoireFromWebPart(string key)
        {
            try
            {

                return dicosDetailMoratoire[key];
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public List<CsReglement> getReglementFromWebPart(string key)
        {
            try
            {
                return dicosReglement[key];
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }
        public Dictionary<string, string> getReglementParameters(string key)
        {
            try
            {
                Dictionary<string, string> param = new Dictionary<string, string>();
                if (parametreReglement.Count != 0 && parametreReglement != null)
                {
                    foreach (KeyValuePair<string, string> pair in parametreReglement)
                    {
                        string[] keys = pair.Key.Split('_');
                        if (keys[0] == key)
                            param.Add(keys[1].ToString(), pair.Value);
                    }
                    return param;
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }
        public bool DeleteDataFromWebPart(string key)
        {
            try
            {
                dicosReglement.Remove(key);
                parametreReglement.Remove(key);
                return true;
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool DeleteDetailMoratoireFromWebPart(string key)
        {
            try
            {
                dicosDetailMoratoire.Remove(key);
                parametreReglement.Remove(key);
                return true;
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }
        public bool setReglementInWebPart(string key, List<CsReglement> objectList, Dictionary<string, string> parameters)
        {

            try
            {
                dicosReglement.Add(key, objectList);
                parametreReglement = parameters;
                return true;
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool setDetailMoratoireInWebPart(string key, List<CsDetailMoratoire> objectList, Dictionary<string, string> parameters)
        {

            try
            {
                dicosDetailMoratoire.Add(key, objectList);
                parametreReglement = parameters;
                return true;
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public List<CParametre> RetourneListImprante()
        {
            try
            {
                List<CParametre> ListeImprimanteRetourne = new List<CParametre>();
                List<CParametre> tempon = new List<CParametre>();
                PrinterSettings.StringCollection Imprimante = null;
                PrinterSettings defaultPrinter = new PrinterSettings();
                string defaultNmae = defaultPrinter.PrinterName;
                Imprimante = PrinterSettings.InstalledPrinters;
                for (int i = 0; i < Imprimante.Count; i++)
                {
                    CParametre UneImprimante = new CParametre();
                    UneImprimante.LIBELLE = Imprimante[i];
                    tempon.Add(UneImprimante);
                }

                foreach (CParametre impr in tempon)
                    if (impr.LIBELLE != defaultNmae)
                        ListeImprimanteRetourne.Add(impr);
                CParametre def = new CParametre();
                def.LIBELLE = defaultNmae;
                ListeImprimanteRetourne.Insert(0, def);

                return ListeImprimanteRetourne;
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public List<CsModereglement> RetourneModesReglement()
        {
            try
            {
                DBEncaissement db = new DBEncaissement();
                return db.RetourneModesReglement();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }

        }


        //bool IService1.launchPrinting(string[] ReportParameter, CsReglement[] _ListeDesReglementAEditer, string printerName)
        //{
        //    try
        //    {
        //        LocalReport localReport = new LocalReport();
        //        List<ReportParameter> listeParam = new List<ReportParameter>();
        //        ReportParameter reportTypeRecu = new ReportParameter(ReportParameter[0], ReportParameter[1]);
        //        listeParam.Add(reportTypeRecu);

        //        localReport.DataSources.Clear();
        //        localReport.DataSources.Add(new ReportDataSource("CsReglement", _ListeDesReglementAEditer));
        //        //localReport.ProcessingMode = ProcessingMode.Local;
        //        localReport.ReportEmbeddedResource = "Galatee.WcfService.Reports.recu.rdlc";
        //        // localReport.SetParameters(listeParam);

        //        ImpressionDirect imp = new ImpressionDirect();
        //        imp.Run(localReport, printerName);

        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        string error = ex.Message;
        //        return false;

        //    }

        //}

        #endregion

        #region Gestion du mode deconnecté
        // CHK - 13/03/2013
        public bool? IsServerDown()
        {
            try
            {
                DBEncaissement db = new DBEncaissement();
                return db.IsServerDown();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return true;
            }            
        }

        public List<CsEncaissementHorsLigne> SynchroniserEncaissements(List<CsEncaissementHorsLigne> listeEnregistrements)
        {
            try
            {
                List<CsEncaissementHorsLigne> copieEnregistrement = new List<CsEncaissementHorsLigne>(listeEnregistrements);
                
                DBEncaissement db = new DBEncaissement();

                foreach (CsEncaissementHorsLigne encaissement in listeEnregistrements)
                {
                    if (db.SynchroniserEncaissements(encaissement.Client, encaissement.Centre, encaissement.Ordre, encaissement.Montant, encaissement.Caisse, encaissement.NumeroRecu, encaissement.Matricule, encaissement.NumeroCheque, encaissement.DateEnregistrement))
                        copieEnregistrement.Remove(encaissement);
                }

                return copieEnregistrement;
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public string ObtenirCheminServeurBD()
        {
            try
            {
                DBEncaissement db = new DBEncaissement();
                return db.RetourneListeTa58("000019").LIBELLE;
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }            
        }

        public bool EnregistrerTraceSynchronisation(string path, string contenu,bool fichierReussite)
        {
            try
            {
                DBEncaissement db = new DBEncaissement();
                return db.EnregistrerTraceSynchronisation(path, contenu);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public List<CsReglementRecu> CashierPayments(string key, Dictionary<string, string> parametresRDLC, List<CsReglementRecu> res)
        {
            try
            {
                List<CsPrint> liste_to_print = new List<CsPrint>();
                liste_to_print.AddRange(res);
                Printings.PrintingsService service_print = new Printings.PrintingsService();


                service_print.setFromWebPart(liste_to_print, key, parametresRDLC);
                return res;
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }

        }
        #endregion

        #region Etat
        public List<CsHabilitationCaisse> ListeDesReversementCaisse(List<CsHabilitationCaisse> LstHabilCaisse)
            {
                try
                {
                    DBEncaissement db = new DBEncaissement();
                    return db.ListeDesReversementCaisse(LstHabilCaisse);
                }
                catch (Exception ex)
                {
                    ErrorManager.LogException(this, ex);
                    return new List<CsHabilitationCaisse>();
                }
            }
            public List<CsLclient> LitseDesTransaction(CsHabilitationCaisse laCaisse)
            {
                try
                {
                    DBEncaissement db = new DBEncaissement();
                    return db.LitseDesTransaction(laCaisse);
                }
                catch (Exception ex)
                {
                    ErrorManager.LogException(this, ex);
                    return null;
                }
            }

            public List<CsLclient>HistoriqueListeEncaissements(List<CsHabilitationCaisse> laCaisse)
            {
                try
                {
                    DBEncaissement db = new DBEncaissement();
                    return db.HistoriqueListeEncaissements(laCaisse);
                }
                catch (Exception ex)
                {
                    ErrorManager.LogException(this, ex);
                    return new List<CsLclient>();
                }
            }


            public List<CsLclient> HistoriqueDesEncaissements(string matricule, int idCentre, DateTime datedebut, DateTime datefin)
            {
                try
                {
                    DBEncaissement db = new DBEncaissement();
                    return db.HistoriqueDesEncaissements(matricule,idCentre,datedebut,datefin);
                }
                catch (Exception ex)
                {
                    ErrorManager.LogException(this, ex);
                    return new List<CsLclient>();
                }
            }

            public List<CsHabilitationCaisse> ListeDesCaisse(int fk_idcaisse,string centre,  DateTime datedebut, DateTime datefin,bool Isferme)
            {
                try
                {
                    DBEncaissement db = new DBEncaissement();
                    return db.ListeDesCaisse(fk_idcaisse ,centre, datedebut, datefin, Isferme);
                }
                catch (Exception ex)
                {
                    ErrorManager.LogException(this, ex);
                    return new List<CsHabilitationCaisse>();
                }
            }
            public List<CsLclient> ChargerListeFacturePeriode(string periodeDebut, string periodeFin, string FactureDeb, string FactureFin)
            {
                try
                {
                    DBEncaissement db = new DBEncaissement();
                    return db.ChargerListeFacturePeriode(periodeDebut, periodeFin, FactureDeb, FactureFin);
                }
                catch (Exception ex)
                {
                    ErrorManager.LogException(this, ex);
                    return new List<CsLclient>();
                }
            }
            public List<CsMonnaie> ReturneAllMonaie()
            {
                return new DBMONNAIE().GetAll();
            }
        

        #endregion

        #region Sylla


            public List<CsLclient> RetourneListeFactureReg(int IdCodeRegroupement, string REFEM)
            {
                try
                {
                    DBEncaissement db = new DBEncaissement();
                    return db.RetourneListeFactureReg(IdCodeRegroupement, REFEM);
                }
                catch (Exception ex)
                {
                    ErrorManager.LogException(this, ex);
                    return null;
                }
            }

            #endregion

        /* LKO 06/02/2019 */
            public List<CsLclient> RemplirfactureRegroupementAvecProduit(CsRegCli csRegCli, List<string> listperiode, List<int> idProduit)
            {
                try
                {
                    List<CsLclient> ListeFacture = new List<CsLclient>();
                    List<int> lstRegroupement = new List<int>();
                    lstRegroupement.Add(csRegCli.PK_ID );
                    ListeFacture = new DBEncaissement().RetourneFactureClientRegroupe(lstRegroupement, listperiode, idProduit);
                    //if (listperiode != null && listperiode.Count != 0)
                    //{
                    //    foreach (var item in listperiode)
                    //    {
                    //        foreach (int itemIdProduit in idProduit)
                    //            ListeFacture.AddRange(new DBEncaissement().RetourneListeFactureNonSoldeByRegroupementProduitspx(csRegCli.PK_ID, item, itemIdProduit));
                    //    }
                    //}
                    //else
                    //{
                    //    foreach (int itemIdProduit in idProduit)
                    //        ListeFacture.AddRange(new DBEncaissement().RetourneListeFactureNonSoldeByRegroupementProduitspx(csRegCli.PK_ID, string.Empty, itemIdProduit));
                    //}
                    return ListeFacture;
                }
                catch (Exception ex)
                {
                    ErrorManager.LogException(this, ex);
                    return null;
                }
            }

        /**/
       
    }
}
