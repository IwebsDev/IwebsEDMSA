using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Galatee.DataAccess;
using Galatee.Structure;
//using Microsoft.Reporting;
//using Microsoft.Reporting.WebForms;
using System.Drawing.Printing;
using System.Reflection;
using System.Xml.Serialization;
using System.IO;
using System.Data;
using System.Web.Mail;
using System.ServiceModel.Activation;
using Galatee.Tools;
using System.Web;
using System.Configuration;

namespace WcfService
{
    // REMARQUE : vous pouvez utiliser la commande Renommer du menu Refactoriser pour changer le nom de classe "AcceuilService" à la fois dans le code, le fichier svc et le fichier de configuration.
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)] 
    public class AcceuilService : IAcceuilService
    {

        public List<CsClient> RetournelstClient(int fk_idcentre, string centre, string client, string Ordre)
        {
            try
            {
                DBFRAUDE db = new DBFRAUDE();
                return db.RetourneClient(fk_idcentre, centre, client, Ordre);
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                throw ex;
            }
        }
        public List<CsStatistiqueTravaux_Brt_Ext> RetourneStatistiqueTravaux_Brt_Ext(List<string> codeproduit, string periode)
        {
            try
            {
                List<CsStatistiqueTravaux_Brt_Ext> result = new List<CsStatistiqueTravaux_Brt_Ext>();
                DBReports db = new DBReports();
                foreach (var item in codeproduit)
                {
                    var stat=db.RetourneStatistiqueTravaux_Brt_Ext(item, periode);
                    if (stat != null && stat.Count() > 0)
                        result.AddRange(stat);
                }
                return result.Distinct().ToList() ;
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                throw ex;
            }
        }
        public List<CsDonnesStatistiqueDemande> RetourneDonnesStatistiqueDemande(string codesite, List<string> Listproduit, List<string> Listtypedemande, string periode)
        {
            try
            {
                DBReports db = new DBReports();
                List<CsDonnesStatistiqueDemande> reslt = new List<CsDonnesStatistiqueDemande>();
                foreach (var produit in Listproduit)
                {
                    foreach (var typedemande in Listtypedemande)
                    {
                        reslt.AddRange(db.RetourneDonnesStatistiqueDemande(codesite, produit, typedemande, periode));
                    }
                }
                return reslt;
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                throw ex;
            }
        }





        public List<CsReclamationRcl> RetourneReclamation(int fk_idcentre, string centre, string client, string ordre, string numerodeamnde)
        {
            try
            {
                DBRECLAMATION db = new DBRECLAMATION();
                return db.RetourneReclamation(fk_idcentre, centre, client, ordre, numerodeamnde);
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                throw ex;
            }
        }
        #region Valider
        public bool ValiderReclamation(CsDemandeReclamation LaDemande)
        {
            return new DBRECLAMATION().ValiderReclamation(LaDemande);
        }
        #endregion

        #region Validation
        public List<CsRclValidation> SelectAllValidation()
        {
            try
            {
                return new DBRECLAMATION().SelectAllValidation();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }
        #endregion
        #region Validation init
        public string ValiderInitReclamation(CsDemandeReclamation LaDemande)
        {
            return new DBRECLAMATION().ValiderInitReclamation(LaDemande);
        }
        #endregion
        #region RetourinfoReclamation
        public CsDemandeReclamation RetourDemandeReclamation(int IDDEMANDE)
        {
            return new DBRECLAMATION().RetourneLaDemande(IDDEMANDE);
        }
        #endregion
        #region TypeReclamationRcl
        public List<CsTypeReclamationRcl> SelectAllTypeReclamationRcl()
        {
            try
            {
                return new DBRECLAMATION().SelectAllTypeReclamationRcl();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }
        #endregion
        #region ModeReception
        public List<CsModeReception> SelectAllModeReception()
        {
            try
            {
                return new DBRECLAMATION().SelectAllModeReception();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }
        #endregion

        public List<CsCompteurBta> SelectAllCompteurPourAffectation()
        {
            try
            {
                return new DBScelle().SelectAllCompteurPourAffectation();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }


        public List<CsCompteurBta> SelectAllCompteur()
        {
            try
            {
                return new DBScelle().SelectAllCompteur();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public List<CsCompteurBta> SelectAllCompteurInDisponible()
        {
            try
            {
                return new DBScelle().SelectAllCompteurInDisponible();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }
        public int InsertMargasinVirtuelle(List<CsCompteurBta> sCompteur, List<CsCompteurBta> sCompteur1)
        {
            try
            {


                return new DBScelle().InsertMargasinVirtuelle(sCompteur, sCompteur1);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return 0;
            }
        }
        public bool EclipseSimpleCompteurTransition(CsDemande laDemande)
        {

            try
            {
                DBAccueil db = new DBAccueil();
                return db.EclipseSimpleCompteurTransition(laDemande);
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return false;
            }
        }
        #region Accueil
        #region GUI00
        
        // ajouter par HGB  13/03/2013
        public List<CsUtilisateur> RetourneListeAllUser()
        {
            try
            {
                return new DBAdmUsers().GetAll();
            }
            catch (Exception zw)
            {

                string error = zw.Message;
                return null;
            }
        }
        public List<CsSite > retourneCssite()
        {
            try
            {
                return null;
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }
        public List<CsCentre> ListeDesDonneesDesSite(bool IsIncrementeDemandeNum)
        {
            try
            {

                DBAccueil db = new DBAccueil();
                return db.ChargerLesDonneesDesSite(IsIncrementeDemandeNum);
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }
        public  List<CsProduit> ListeDesProduit()
        {
            try
            {

                DBAccueil db = new DBAccueil();
                return db.RetourneListeProduit();
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }
        public List<CsCommune> ChargerCommune()
        {
            try
            {

                DBAccueil db = new DBAccueil();
                return db.ChargerLesCommune();
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }
        public List<CsQuartier> ChargerLesQartiers()
        {
            try
            {

                DBAccueil db = new DBAccueil();
                return db.ChargerLesQartiers();
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }
        public List<CsSecteur> ChargerLesSecteurs()
        {
            try
            {

                DBAccueil db = new DBAccueil();
                return db.ChargerLesSecteurs();
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }

        public List<CsPosteSource> ChargerPosteSource()
        {
            try
            {

                DBAccueil db = new DBAccueil();
                return db.ChargerPosteSource();
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }
        public List<CsDepart> ChargerDepartHTA()
        {
            try
            {

                DBAccueil db = new DBAccueil();
                return db.ChargerDepartHTA();
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }
        public List<CsPosteTransformation> ChargerPosteTransformateur()
        {
            try
            {
                DBAccueil db = new DBAccueil();
                return db.ChargerPosteTransformateur();
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }

        public List<CsRues> ChargerLesRueDesSecteur()
        {
            try
            {
                DBAccueil db = new DBAccueil();
                return db.ChargerLesRueDesSecteur();
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }

        public List<CsTournee> ChargerLesTournees()
        {
            try
            {

                DBAccueil db = new DBAccueil();
                return db.ChargerLesTournees();
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }
        public List<CsCalibreCompteur > ChargerCalibreCompteur()
        {
            try
            {
                return new DB_CALIBRECOMPTEUR().SelectAllCalibreCompteur ();
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }

        public List<CsReglageCompteur > ChargerReglageCompteur()
        {
            try
            {
                return new DB_REGLAGECOMPTEUR().SelectAllReglageCompteur();
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }
        public  List<CsTarif > ChargerTarif()
    {
        try
        {

            DBAccueil db = new DBAccueil();
            return db.ChargerTarif();
        }
        catch (Exception ex)
        {
            return null;
        }
    }

        public List<CsTarif> ChargerTarifPuissance()
        {
            try
            {

                DBAccueil db = new DBAccueil();
                return db.ChargerTarifPuissance();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public  List<CsForfait> ChargerForfait()
    {
        try
        {

            DBAccueil db = new DBAccueil();
            return db.ChargerForfait ();
        }
        catch (Exception ex)
        {
            ErrorManager.WriteInLogFile(this, ex.Message);
            return null;
        }
    }
        public List<CsTarif> ChargerTarifParReglageCompteur()
        {
            try
            {
                DBAccueil db = new DBAccueil();
                return db.ChargerTarifParReglageCompteur();
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }
        public List<CsTarif> ChargerTarifParCategorie()
        {
            try
            {
                DBAccueil db = new DBAccueil();
                return db.ChargerTarifParCategorie();
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }
        public List<CsPuissance> ChargerPuissanceReglageCompteur()
        {
            try
            {
                DBAccueil db = new DBAccueil();
                return db.ChargerPuissanceReglageCompteur();
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }
        public List<CsPuissance> ChargerPuissanceSouscrite()
        {
            try
            {
                DBAccueil db = new DBAccueil();
                return db.ChargerPuissanceSouscrite();
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }
        public List<CsPuissance> ChargerPuissanceInstalle()
        {
            try
            {
                DBAccueil db = new DBAccueil();
                return db.ChargerPuissanceInstalle();
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }
        public List<CsRubriqueDevis > ChargerRubrique()
        {
            try
            {
                DBAccueil db = new DBAccueil();
                return db.ChargerRubrique();
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }


        public List<CsMois > ChargerTousMois()
        {
            try
            {
                DBAccueil db = new DBAccueil();
                return db.RetourneTousMois();
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }
        public List<CsCadran> RetourneToutCadran()
        {
            try
            {
                DBAccueil db = new DBAccueil();
                return db.RetourneToutCadran();
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }

        public string  NumeroFacture(int PkidCentre)
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
        public List<CsMarqueCompteur> RetourneToutMarque()
        {
            try
            {
                DBAccueil db = new DBAccueil();
                return db.RetourneToutMarque();
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }
        public List<CsFrequence> ChargerTousFrequence()
        {
            try
            {
                DBAccueil db = new DBAccueil();
                return db.RetourneTousFrequence ();
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }

        public List<CsTypeBranchement  > ChargerTypeBranchement()
        {
            try
            {
                DBAccueil db = new DBAccueil();
                return db.RetourneTypeDeBranchement();
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }

        public List<CsDiametreBranchement> ChargerDiamentreBranchement()
        {
            try
            {
                DBAccueil db = new DBAccueil();
                return db.RetourneDiametreBranchement();
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }

        public List<CsMaterielBranchement> RetourneMaterielBranchement()
        {
            try
            {
                DBAccueil db = new DBAccueil();
                return db.RetourneMaterielBranchement();
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }
        public List<CsCodeTaxeApplication> RetourneTousApplicationTaxe()
        {
            try
            {
                DBAccueil db = new DBAccueil();
                return db.RetourneTousApplicationTaxe();
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }

 
        public List<CsTypeComptage > ChargerTypeComptage()
        {
            try
            {

                DBAccueil db = new DBAccueil();
                return db.ChargerTypeComptage();
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }
        public List<CsCoutDemande> ChargerCoutDemande()
        {
            try
            {

                DBAccueil db = new DBAccueil();
                return db.ChargerCoutDemande();
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }
        public List<CsTcompteur> ChargerType()
        {
            try
            {

                DBAccueil db = new DBAccueil();
                return db.ChargerType();
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null; ;
            }
        }
        public List<CsEtapeDemande> ChargerEtapeDemande()
        {
            try
            {

                DBAccueil db = new DBAccueil();
                return db.ChargerEtapeDemande();
            }
            catch (Exception)
            {
                return null;
            }
        }
        public List<CsClientRechercher> RechercherClient(CsClientRechercher ClientRechercher)
        {
            try
            {
                DBAccueil db = new DBAccueil();

                return db.RechercherClient(ClientRechercher);
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }
        public Dictionary<List <CsClientRechercher >,List<CsLclient>> RechercherClientRegrouper(CsRegCli  leRegroupement)
        {
            try
            {
                DBAccueil db = new DBAccueil();
                return db.RechercherClientRegrouper(leRegroupement);
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }

        public CsClient RetourneClient(int Fk_idcentre, string Centre, string Client, string Ordre)
        {
            try
            {

                DBAccueil db = new DBAccueil();
                return db.RetourneClient(Fk_idcentre, Centre, Client, Ordre);
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
            
        }
        public List<CsModepaiement> RetourneCodeModePayement()
        {
            try
            {
                DBAccueil db = new DBAccueil();
                var lst = db.RetourneCodeModePayement();
                return lst ;
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }

        public List<CsCategorieClient > RetourneCatCli(string key)
        {
            try
            {
                DBAccueil db = new DBAccueil();
                var lst = db.RetourneCategorie();

                List<CsPrint> listeAImprimer = new List<CsPrint>();
                listeAImprimer.AddRange(lst);                
                Printings.PrintingsService service = new Printings.PrintingsService();
                service.setFromWebPart(listeAImprimer, key, null);
                return lst;
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }

        public CsAg RetourneAdresse(int fk_idcentre, string Centre, string Client, string ordre)
        {
            try
            {

                DBAccueil db = new DBAccueil();
                return db.RetourneAdresse(fk_idcentre, Centre, Client);
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }

        public List<CsAbon> RetourneAbon(int fk_idcentre, string Centre, string Client, string Ordre)
        {
            try
            {

                DBAccueil db = new DBAccueil();
                return db.RetourneAbon(fk_idcentre, Centre, Client, Ordre);
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }


        public List<CsCodeConsomateur> RetourneCodeConsomateur()
        {
            try
            {
                DBAccueil db = new DBAccueil();
                return db.RetourneCodeConsomateur();
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }
        public List<CsCategorieClient > RetourneCategorie()
        {
            try
            {
                DBAccueil db = new DBAccueil();
                return db.RetourneCategorie ();
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }
        public List<CsNatureClient > RetourneNature()
        {
            try
            {
                DBAccueil db = new DBAccueil();
                return db.RetourneNatureClient ();
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }
        public List<CsFermable > RetourneFermable()
        {
            try
            {
                DBAccueil db = new DBAccueil();
                return db.RetourneFermable();
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }
        public List<CsNationalite> RetourneNationnalite()
        {
            try
            {
                DBAccueil db = new DBAccueil();
                return db.RetourneNationnalite();
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }
        public List<CsDenomination > RetourneListeDenominationAll()
        {
            try
            {
                DBAccueil db = new DBAccueil();
                return db.RetourneListeDenominationAll();
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }
        public List<CsRegCli > RetourneTousCodeRegroupement()
        {
            try
            {
                DBAccueil db = new DBAccueil();
                return db.RetourneTousCodeRegroupement();
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }
        public List<CsSite> RetourneTousSite()
        {
            try
            {
                DBAccueil db = new DBAccueil();
                return db.RetourneTousSite();
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }


        public List<CsSpeSite > ChargerSpecificiteSite(string _pSite)
        {
            try
            {

                DBAccueil db = new DBAccueil();
                return db.ChargerSpecificiteSite(_pSite);
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }
        public bool EditionCompteClient(List<CsReglement> _LeCompteClient, string key)
        {
            try
            {
                    if (_LeCompteClient != null && _LeCompteClient.Count != 0)
                    {
                        // Edition a implementer
                        //setListRiInWebPart("", ListeDesRi, null);
                        List<CsPrint> listePrint = new List<CsPrint>();
                        listePrint.AddRange(_LeCompteClient);
                        Printings.PrintingsService printService = new Printings.PrintingsService();
                        printService.setFromWebPart(listePrint, key, null);
                        //
                    }
                    return true;
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return false ;
            }
        }




        public List<CsLclient> RetourneListeFactureClient(int fk_idcentre, string Centre, string Client, string Ordre)
        {
            try
            {
                DBAccueil db = new DBAccueil();
                return db.RetourneListeFactureClient(fk_idcentre,Centre, Client, Ordre);
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }
        public List<CsLclient> RetourneListeReglement(int fk_idcentre, string Centre, string Client, string Ordre)
        {
            try
            {

                DBAccueil db = new DBAccueil();
                return db.RetourneListeReglement(fk_idcentre,Centre, Client, Ordre);
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }
        public List<CsCanalisation> RetourneCanalisationClasseur(int fk_idCentre, string Centre, string Client,List<CsAbon> lstAbon, string produit, int? point)
        {
            try
            {

                DBAccueil db = new DBAccueil();
                //return db.RetourneCanalisationClasseur(fk_idCentre, Centre, Client, fk_idAbon, produit, point);

                List<CsCanalisation> lstCanalisation = new List<CsCanalisation>();
                foreach (CsAbon item in lstAbon)
                {
                    lstCanalisation.AddRange(db.RetourneCanalisationClasseur(fk_idCentre, Centre, Client, item.PK_ID, item.PRODUIT, point));
                }

                return lstCanalisation;
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }
        public CsCanalisation RetourneCanalisationResilier(int fk_idCentre, string Centre, string Client, string produit)
        {
            try
            {

                DBAccueil db = new DBAccueil();
                return db.RetourneCanalisationResilier(fk_idCentre, Centre, Client, produit);
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }
        public List<CsCanalisation> RetourneCanalisation(int fk_idCentre, string Centre, string Client, string produit, int? point)
        {
            try
            {

                DBAccueil db = new DBAccueil();
                return db.RetourneCanalisation(fk_idCentre,Centre, Client, produit, point);
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }
        public List<CsEvenement> RetourneEvenement(int fk_idcentre, string centre, string client, string Ordre, string produit, int point)
        {
            try
            {

                DBAccueil db = new DBAccueil();
                return db.RetourneEvenement(fk_idcentre,centre, client, Ordre, produit, point, null);
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }
        public List<CsBrt> RetourneBranchement(int fk_idcentre, string centre, string client, string produit)
        {
            try
            {
                DBAccueil db = new DBAccueil();
                return db.RetourneBranchement(fk_idcentre,centre, client, produit);
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }
        public List<CsOrganeScelleDemande > RetourneScellage(int fk_idbranchement)
        {
            try
            {
                DBAccueil db = new DBAccueil();
                return db.RetourneScellage(fk_idbranchement);
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }
        public List<CsOrganeScelleDemande> RetourneScellageCompteur(string numero,string marque)
        {
            try
            {
                DBAccueil db = new DBAccueil();
                return db.RetourneScellageCompteur( numero, marque);
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }

        public List<CsEvenement> RetourneEvenementDeLaCanalisation(List<CsCanalisation> LstCanalisation)
        {
           try
           {
               DBAccueil db = new DBAccueil();
               return db.RetourneEvenementDeLaCanalisation(LstCanalisation );
           }
           catch (Exception ex)
           {
               ErrorManager.WriteInLogFile(this, ex.Message);
               return null;
           }
       }
        //public List<CsEvenement> RetourneDernierEvenementDeLaCanalisation(CsAbon leAbonnement)
        //{
        //    try
        //    {
        //        DBAccueil db = new DBAccueil();
        //        return db.RetourneDernierEvenementDeLaCanalisation(leAbonnement);
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorManager.WriteInLogFile(this, ex.Message);
        //        return null;
        //    }
        //}

        //public List<CsEvenement> RetourneEvenementCanalisationPeriode(CsAbon leAbonnement, string Periode)
        //{
        //    try
        //    {
        //        DBAccueil db = new DBAccueil();
        //        return db.RetourneEvenementCanalisationPeriode(leAbonnement, Periode);
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorManager.WriteInLogFile(this, ex.Message);
        //        return null;
        //    }
        //}

        public string RetourneOrdreMax(int fk_idcentre, string centre, string client, string produit)
        {
            try
            {
                DBAccueil db = new DBAccueil();
                return db.RetourneOrdreMax(fk_idcentre, centre, client, produit);
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }

        public  List<CsEvenement> RetourneDernierEvenementFacturer(int fk_idcentre, string centre, string client, string Ordre, string produit,int ? point)
        {
            try
            {
                DBAccueil db = new DBAccueil();
                return db.RetourneDernierEvenementFacturer(fk_idcentre,centre, client, Ordre, produit, point);
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }
  
        public bool ValiderRejetDemande(CsDemandeBase  LaDemannde)
        {
            try
            {
                 DBAccueil db = new DBAccueil();
                 return db.ValiderRejetDemande(LaDemannde); ;
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return false;
            }
        }
        public bool ValiderDemande(CsDemande LaDemannde)
        {
            try
            {
                ErrorManager.WriteInLogFile(this, "Debut Validation");
                DBAccueil db = new DBAccueil();
                return db.ValiderDemande(LaDemannde); ;
            }
            catch (Exception ex)
            {
                string leMessage = ex.Message;
                if (ex.InnerException != null) leMessage = ex.InnerException.Message;
                
                ErrorManager.WriteInLogFile(this, ex.Message);
                return false;
            }
        }
        public string  ValiderAchatimbreDemande(CsDemande LaDemannde)
        {
            try
            {
                ErrorManager.WriteInLogFile(this, "Debut Validation");
                DBAccueil db = new DBAccueil();
                return db.ValiderAchatimbreDemande(LaDemannde, true); ;
            }
            catch (Exception ex)
            {
                string leMessage = ex.Message;
                if (ex.InnerException != null) leMessage = ex.InnerException.Message;
                
                ErrorManager.WriteInLogFile(this, ex.Message);
                return ex.Message;
            }
        }
        
        public string ValiderDemandeInitailisation(CsDemande LaDemannde)
        {
            return new DBAccueil().ValiderDemandeDevis(LaDemannde);
        }

        //public CsDemande GetDevisByRefBranch(CsClient leclient)
        //{
        //    try
        //    {
        //        DBAccueil db = new DBAccueil();
        //        return db.RetourneDetailClientFromRefClient(leclient.FK_IDCENTRE.Value, leclient.CENTRE, leclient.REFCLIENT, leclient.ORDRE);
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorManager.LogException(this, ex);
        //        return null;
        //    }
        //}
        
        #region MAJ Modification demande
        
            public CsDemande GeDetailByFromClient(CsClient leclient)
            {
                try
                {
                    DBAccueil db = new DBAccueil();
                    var _lademande = db.RetourneDetailClientFromRefClient(leclient.FK_IDCENTRE.Value, leclient.CENTRE, leclient.REFCLIENT, leclient.ORDRE, leclient.PRODUIT,leclient.TYPEDEMANDE );
                    return _lademande;
                }
                catch (Exception ex)
                {
                    ErrorManager.LogException(this, ex);
                    return null;
                }
            }

            //public CsDemande RetourneDetailClientFromRefClient(int fk_idcentre, string centre, string client, string ordre)
            //{
            //    try
            //    {
            //        DBAccueil db = new DBAccueil();
            //        return db.RetourneDetailClientFromRefClient(fk_idcentre, centre, client, ordre);
            //    }
            //    catch (Exception ex)
            //    {
            //        ErrorManager.LogException(this, ex);
            //        return null;
            //    }
            //}

        #endregion

        public ObjDOCUMENTSCANNE  ReturneObjetScan(CsDemandeBase laDamande)
        {
            try
            {
                return new DBAccueil().RetourneObjetScane(laDamande);
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }
        public  List<CsCasind> RetourneListeDesCas()
        {
            try
            {
                DBAccueil db = new DBAccueil();
                return db.RetourneListeDesCas();
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }

        public CsDemande RetourneLaDemande(string centre, string numdemande,int Fk_IDcentre)
        {
            try
            {
                DBAccueil db = new DBAccueil();
                return db.RetourneLaDemande(numdemande, centre, Fk_IDcentre);
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }
    
        public  List<CsCtax> RetourneListeTaxe()
        {
            try
            {
                DBAccueil db = new DBAccueil();
                return db.RetourneListeTaxe();
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }
        public List<CsCoper> RetourneTousCoper()
        {
            try
            {
                DBAccueil db = new DBAccueil();
                return db.RetourneTousCoper();
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }
     
        public List<CsParametreBranchement> RetourneParametreBranchement()
        {
            try
            {
                DBAccueil db = new DBAccueil();
                return db.RetourneParametreBranchement();
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }
        public List<CsRegCli> RetourneCodeRegroupement()
        {
            try
            {
                DBAccueil db = new DBAccueil();
                return db.RetourneCodeRegroupement ();
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }
        public List<CsDemandeBase> RetourneListeDemande(int? Idcentre, string numdem, List<string> LstTdem, DateTime? datedebut, DateTime? dateFin,
                                                   DateTime? datedemande, string numerodebut, string numerofin, string status)
        {

            try
            {
                DBAccueil db = new DBAccueil();
                return db.RetourneListeDemande(Idcentre, numdem, LstTdem, datedebut, dateFin, datedemande, numerodebut, numerofin, status);
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }
        public List<CsDemandeBase> RetourneListeDemandeCritere(int? Idcentre, string numdem, List<string> LstTdem, DateTime? datedebut, DateTime? dateFin,
                                                   DateTime? datedemande, string numerodebut, string numerofin, string status, string Commune, string Quatier, string Secteur, string Rue, string Porte, string Etage, string NumeroLot, string Compteur, string Nom)
        {

            try
            {
                DBAccueil db = new DBAccueil();
                return db.RetourneListeDemandeCritere(Idcentre, numdem, LstTdem, datedebut, dateFin, datedemande, numerodebut, numerofin, status, Commune, Quatier, Secteur, Rue, Porte, Etage, NumeroLot, Compteur, Nom);
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }
        public bool IsDernierEvtEnFacturation(string pCentre, string pClient, string pOrdre)
        {
            try
            {
                DBAccueil db = new DBAccueil();
                return db.IsDernierEvtEnFacturation(pCentre, pClient, pOrdre);
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return false ;
            }
        }
        public CsDemandeBase RetourneDemandeClientType(CsClient leClient)
        {

            try
            {
                DBAccueil db = new DBAccueil();
                return db.RetourneDemandeClientType(leClient);
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }
        public List<CsDemandeBase> RetourneListeDemandeClient(CsClientRechercher leClient)
        {

            try
            {
                DBAccueil db = new DBAccueil();
                return db.RetourneListeDemandeClient(leClient.FK_IDCENTRE , leClient.CENTRE, leClient.CLIENT, leClient.ORDRE);
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }

        public List<CsDemandeBase > RetourneListeDemandeModificationPourSuvie(string centre, string numdem, string LstTdem, string produit, DateTime? datedebut, string matricule)
        {

            try
            {
                DBAccueil db = new DBAccueil();
                return db.RetourneListeDemandeModificationPourSuvie(centre, numdem, LstTdem, produit, datedebut, matricule);
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }
        public List<CsDemandeBase > RetourneListeDemandeModification(string centre, string numdem, string LstTdem,string produit, DateTime? datedebut,string matricule)
        {

            try
            {
                DBAccueil db = new DBAccueil();
                return db.RetourneListeDemandeModification(centre, numdem, LstTdem, produit, datedebut,matricule );
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }

        public CsDemande RetourneDetailDemande(CsDemandeBase laDemande)
        {

            try
            {
                DBAccueil db = new DBAccueil();
                return db.RetourneDetailDemande( laDemande);
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }

        public CsDemande GetDemandeByNumIdDemande(int pIdDemandeDevis)
        {
            try
            {
                DBAccueil db = new DBAccueil();

                CsDemandeBase lademade = db.GetDemandeByNumIdDemande(pIdDemandeDevis);
                return db.RetourneDetailDemandeFromDEvis(lademade);
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.InnerException.InnerException.InnerException.InnerException.InnerException.InnerException.Message);
                return null;
            }
        }
    

        public CsClasseurClient RetourneClasseurClient(CsClientRechercher _LeClientRecherche)
        {
            try
            {
                DBAccueil db = new DBAccueil();
                return db.RetourneClasseurClient(_LeClientRecherche);
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }
        public CsCompteClient RetourneLeCompteClient(int fk_idcentre, string centre, string client, string ordre)
        {
            try
            {
                DBAccueil db = new DBAccueil();
                return db.RetourneLeCompteClient(fk_idcentre,centre, client, ordre);
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }


        public List<CsCompteClient> RetourneTousLesCompteClient(int fk_idcentre, string centre, string client, List<string> lstOrdre)
        {
            try
            {
                List<CsCompteClient> comptes = new List<CsCompteClient>();
                DBAccueil db = new DBAccueil();
                foreach (string st in lstOrdre)
                    comptes.Add(db.RetourneLeCompteClient(fk_idcentre, centre, client, st));

                return comptes;
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }


        public bool  EditionClasseurClient(CsClasseurClient _leClasseurClient,string key)
        {
            List<CsClasseurClient> _lst = new List<CsClasseurClient>();
            _lst.Add(_leClasseurClient);
            Printings.PrintingsService leClientService = new Printings.PrintingsService();
            //leClientService.setFromWebPart(_lst, key, null);

            //DataSet _LaSource = new DataSet();

            //DataTable _TableAg = new DataTable();
            //List<CsAg> _LstAg = new List<CsAg>();
            //_LstAg.Add(_leClasseurClient.Ag);
            //_TableAg = Galatee.Tools.Utility.ListToDataTable(_LstAg);
            //_LaSource.Tables.Add(_TableAg);

            //DataTable _TableClient = new DataTable();
            //List<CsClient> _LstClient = new List<CsClient>();
            //_LstClient.Add(_leClasseurClient.LeClient );
            //_TableClient = Galatee.Tools.Utility.ListToDataTable(_LstClient);
            //_LaSource.Tables.Add(_TableClient);
            return true  ;
        }
        public CsClasseurClient RetourneModificationBrt(CsClientRechercher _LeClientRecherche)
        {
            try
            {
                DBAccueil db = new DBAccueil();
                return db.RetourneModificationBrt(_LeClientRecherche);
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }
        public bool MAJDemande(CsDemandeBase  _LaDemandeMiseAJoure)
        {
            try
            {
                DBAccueil db = new DBAccueil();
                return db.MAJDemande(_LaDemandeMiseAJoure);
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return false;
            }
        }
        public List<CsLclient> RetourneCompteClientTransfert(CsClientRechercher LeClient, string Orientation)
        {
            try
            {
                DBAccueil db = new DBAccueil();
                //return db.RetourneModificationBrt(_LeClientRecherche);
                return db.RetourneCompteClientTransfert(LeClient, Orientation);
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }
        public List<CsLePayeur> RetourneLesPayeur()
        {
            try
            {
                DBAccueil db = new DBAccueil();
                return db.RetourneLesPayeur();
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }
        public bool MajPayeur(CsLePayeur _LePayeur,int Action)
        {
            try
            {
                DBAccueil db = new DBAccueil();
                return db.MajPayeur(_LePayeur, Action);
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return false;
            }
        }
 
        public List<CsDetailCampagne> RetourneFactureCampagne(string IdCampagen, string centre, string client, string ordre)
        {
            try
            {
                DBAccueil db = new DBAccueil();
                return db.RetourneFactureCampagne(IdCampagen,centre,client,ordre);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool InsererFraisPose(CsLclient laFacture)
        {
            try
            {
                DBAccueil db = new DBAccueil();
                laFacture.NDOC = db.NumeroFacture(laFacture.FK_IDCENTRE );

                return db.InsererLclient(laFacture);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CsCAMPAGNE> RetourneListeDesCampagne(List<int> lstCentre)
        {
            try
            {
                return new DBMoratoires().RetourneDonneeCampagne(lstCentre);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }
        public List<CsTypeCoupure > RetourneTypeCoupure()
        {
            try
            {
                return new DBAccueil().RetourneTypeCoupure();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }
        public bool ValiderCreationFactureIsole(List<CsEvenement> lstEvenement )
        {
            try
            {
                DbFacturation db = new DbFacturation();
                return db.ValiderCreationFactureIsole( lstEvenement );
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return false ;
            }
        }
        public CsEditionDevis EditionDevis()
        {
            return new CsEditionDevis();
        }
        public List<ObjELEMENTDEVIS  > SelectAllMateriel()
        {
            try
            {
              return DBMATERIEL.GetAllMaterielOnFourniture();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }



        #endregion
        #region Cli330
        public List<CsCodeControle > RetourneCodeControle()
        {
            try
            {

                DBAccueil db = new DBAccueil();
                return db.RetourneCodeControle();
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }
        public List<CsTypeLot> RetourneTypeLot()
        {
            try
            {

                DBAccueil db = new DBAccueil();
                return db.RetourneTypeLot();
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }
        public List<CsOrigineLot > RetourneOrigine()
        {
            try
            {

                DBAccueil db = new DBAccueil();
                return db.RetourneOrigine();
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }
        public List<CsLotCompteClient> RetourneListeDesTypeLot(string Origine, string TypeLot)
        {
            try
            {

                DBAccueil db = new DBAccueil();
                return db.RetourneListeDesTypeLot(Origine, TypeLot);
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }
        public List<CsDetailLot> RetourneListeDesDetailLot(int IdLot)
        {
            try
            {

                DBAccueil db = new DBAccueil();
                return db.RetourneListeDesDetailLot(IdLot);
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }
        public bool ValiderSaisieBactch(CsSaisieDeMasse _LaSaisieLot)
        {
            try
            {

                DBAccueil db = new DBAccueil();
                return db.ValiderSaisieBactch(_LaSaisieLot);
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return false ;
            }
        }
        public CsNatgen RetourneNatureParCoper(string Coper)
        {
            try
            {

                DBAccueil db = new DBAccueil();
                return db.RetourneNatureParCoper(Coper);
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }
        public CsCoper RetourneCoper(string Coper)
        {
            try
            {

                DBAccueil db = new DBAccueil();
                return db.RetourneCoper(Coper);
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }
        public int? RetourneMaxIDlot()
        {
            try
            {

                DBAccueil db = new DBAccueil();
                return db.RetourneMaxIDlot();
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return 0;
            }
        }
        public bool ValiderMiseAJourBatch(List<CsLotCompteClient> ListLot)
        {
            try
            {

                DBAccueil db = new DBAccueil();
                return db.ValiderMiseAJourBatch(ListLot);
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return false ;
            }
        }
        public List<CsDetailLot> RetourneDetailLot(List<CsLotCompteClient> LesLot)
        {
            try
            {

                DBAccueil db = new DBAccueil();
                return db.RetourneDetailLot(LesLot);
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }
        public List<CsLclient> RetourneCompteAjuste(List<CsLotCompteClient> ListLot)
        {
            try
            {

                DBAccueil db = new DBAccueil();
                return db.RetourneCompteAjuste(ListLot);
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }
        public bool PurgeLot(List<CsLotCompteClient> ListLot)
        {
            try
            {
                DBAccueil db = new DBAccueil();
                return db.PurgeLot(ListLot);
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return false ;
            }
        }
        public List<CsTdem> RetourneOptionDemande()
        {

            try
            {
                DBAccueil db = new DBAccueil();
                return db.RetourneOptionDemande();
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }
        #endregion
        #region Sylla
        #region 13/05/2017
        public CsTypeComptage AdapterComptage(int? IDPUISSANCE, int PuissanceUtilise, int? NOMBRETRANSFORMATEUR)
        {
            try
            {
                return new DBAccueil().AdapterComptage( IDPUISSANCE, PuissanceUtilise, NOMBRETRANSFORMATEUR);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        #endregion
        public List<CsFactureClient> returnCsFactureClient()
        {
            return new List<CsFactureClient>();
        }
        public List<CsTypeDOCUMENTSCANNE> ChargerTypeDocument()
        {
            try
            {
                return DBAccueil.ChargerTypeDocument();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public List<CsCATEGORIECLIENT_TYPECLIENT> ChargerCategorieClient_TypeClient()
        {
            try
            {
                return DBAccueil.ChargerCategorieClient_TypeClient();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }
        public List<CsCATEGORIECLIENT_USAGE> ChargerCategorieClient_Usage()
        {
            try
            {
                return DBAccueil.ChargerCategorieClient_Usage();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }
        public List<CsNATURECLIENT_TYPECLIENT> ChargerNatureClient_TypeClient()
        {
            try
            {
                return DBAccueil.ChargerNatureClient_TypeClient();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }
        public List<CsUSAGE_NATURECLIENT> ChargerUsage_NatureClient()
        {
            try
            {
                return DBAccueil.ChargerUsage_NatureClient();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }
        public List<CsProprietaire> RemplirProprietaire()
        {
            try
            {
                DB_Proprietaire db = new DB_Proprietaire();
                return db.SelectAllProprietaire();
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }
        #region PIECEIDENTITE

        public ObjPIECEIDENTITE GetPieceIdentiteById(int id)
        {
            try
            {
                return DBPIECEIDENTITE.GetById(id);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public List<ObjPIECEIDENTITE> GetAllPieceIdentite()
        {
            try
            {
                return DBPIECEIDENTITE.GetAll();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }
        #region APPAREIL

        public ObjAPPAREILS GetAppareilByCodeAppareil(int pCodeAppareil)
        {
            try
            {
                return null;// DBAPPAREILS.GetByCodeAppareil(pCodeAppareil);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public List<ObjAPPAREILS> GetAllAppareil()
        {
            try
            {
                return DBAPPAREILS.GetAll();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }
 

 

        public bool UpdateDevisValidationFinale(ObjDEVIS entity, List<ObjELEMENTDEVIS> _lElements, ObjMATRICULE agent, int idEtapeSuivi)
        {
            try
            {
                return DBDevisManagement.UpdateDevisValidation(entity, _lElements, agent, idEtapeSuivi);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }
        #endregion

        #endregion
        #region TYPE CLIENT
        public List<CsTypeClient> GetAllTypeClient()
        {
            try
            {
                return new DBTYPECLIENT().SelectAllTypeClient();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }
        public List<CsUsage> GetAllUsage()
        {
            try
            {
                return new DBUSAGE().SelectAllUsage();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }
        public List<CsStatutJuridique> GetAllStatutJuridique()
        {
            try
            {
                return new DBSTATUTJURIDIQUE().SelectAllStatutJuridique();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }
        #endregion

        #region CR TRAVAUX
        //AKO 11/11/2015
        public List<CsOrganeScellable> LoadListeOrganeScellable(int FK_IDTDEM, int FK_IDPRODUIT)
        {
            try
            {
                DBDEVIS db = new DBDEVIS();
                return db.LoadListeOrganeScellable(FK_IDTDEM, FK_IDPRODUIT);
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }
        public List<CsOrganeScelleDemande> LoadListerganeScelleDemande(int FK_IDTDEM)
        {
            try
            {
                DBDEVIS db = new DBDEVIS();
                //return db.LoadListeOrganeScellable(FK_IDTDEM);
                return new List<CsOrganeScelleDemande>();
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }
        public List<Galatee.Structure.CsScelle> LoadListeScelle()
        {
            try
            {
                DBDEVIS db = new DBDEVIS();
                return db.LoadListeScelle();
                //return new List<Galatee.Structure.Scelle.CsScelle>();
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }
        public CsCriteresDevis GetParametresDistance(string pMaxi, string pSeuil, string pMaxiSubvention)
        {
            try
            {
                return DBDevisManagement.GetParametresDistance(pMaxi, pSeuil, pMaxiSubvention);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }
        #endregion

        #region 12/01/2016

        public List<CsDemande> GetDemandeByListeNumIdDemande(List<int> pIdDemandeDevis)
        {
            try
            {
                DBAccueil db = new DBAccueil();
                List<CsDemande> ListDemande = new List<CsDemande>();

                foreach (var item in pIdDemandeDevis)
                {
                    CsDemandeBase lademade = db.GetDemandeByNumIdDemande(item);
                    ListDemande.Add(db.RetourneDetailDemandeFromDEvis(lademade));
                }
                return ListDemande;
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public List<CsProgarmmation> RetourneProgrammation()
        {
            try
            {
                DBAccueil db = new DBAccueil();
                List<CsProgarmmation> ListProgarmmation = new List<CsProgarmmation>();
                ListProgarmmation = db.RetourneProgrammation();

                return ListProgarmmation;
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }
        public List<CsSortieMateriel > RetourneSortieMateriel()
        {
            try
            {
                DBAccueil db = new DBAccueil();
                List<CsSortieMateriel> ListProgarmmation = new List<CsSortieMateriel>();
                ListProgarmmation = db.RetourneSortieMateriel();

                return ListProgarmmation;
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }
        public CsTdem  RetourneTypeDemandeFromIdEtapeWkf(int idEtape)
        {
            try
            {
                DBAccueil db = new DBAccueil();
                return db.RetourneTypeDemandeFromIdEtapeWkf(idEtape);

            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }


        public bool  MiseAJourElementDevis(List<CsDemande> LstDemannde)
        {
            List<ObjELEMENTDEVIS> lstObjMaj = new List<ObjELEMENTDEVIS>();
            foreach (var item in LstDemannde)
            {
                item.EltDevis.ForEach(t => t.FK_IDDEMANDE = item.LaDemande.PK_ID);
                lstObjMaj.AddRange (item.EltDevis);
            }
            return  DBELEMENTSDEVIS.UpdateElementsDevis(lstObjMaj);
        }


        public bool DesactivationProgrammation(List<int> pIdDemandeDevis)
        {
            DBAccueil db = new DBAccueil();
            return db.DesactivationProgrammation(pIdDemandeDevis);
        }

        public List<CsDemandeBase> RetourneListeDemandeAvalider(int IdReleveur)
        {
            try
            {
                DBAccueil db = new DBAccueil();
                List<CsDemandeBase> ListDemande = new List<CsDemandeBase>();

                var ListeSortieMateriel = RetourneListeSortieMaterielLivre();

                List<int> pIdDemandeDevis = new List<int>();
                var ListeSortieMaterielDureleveur = ListeSortieMateriel.Where(s => s.FK_IDLIVREUR == IdReleveur).Select(a => a.FK_IDDEMANDE);
                pIdDemandeDevis = ListeSortieMaterielDureleveur != null ? ListeSortieMaterielDureleveur.ToList() : null;
                if (pIdDemandeDevis != null)
                    ListDemande = RetourneListeDemandeById(pIdDemandeDevis);
                return ListDemande;
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }




        public List<CsSortieMateriel> RetourneListeSortieMaterielLivre()
        {

            try
            {
                DBAccueil db = new DBAccueil();
                return db.RetourneListeSortieMaterielLivre();
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }




        #endregion

        #endregion
        #region fomba 29/10/2015

        public List<CsDemandeBase> RetourneListeDemandeById(List<int> lesdemande)
        {

            try
            {
                DBAccueil db = new DBAccueil();
                return db.RetourneListeDemandeById(lesdemande);
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }

        public List<CsDemandeBase> RetourneListeDemandeEtapeById(List<int> lesdemande,int IdTypeDemande)
        {

            try
            {
                DBAccueil db = new DBAccueil();
                return db.RetourneListeDemandeEtapeById(lesdemande, IdTypeDemande);
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }

        public List<CsDemandeBase> RetourneListeDemandeByIdSansClient(List<int> demandes, int IdTypeDemande)
        {

            try
            {
                DBAccueil db = new DBAccueil();
                return db.RetourneListeDemandeByIdSansClient(demandes, IdTypeDemande);
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }

        public List<CsCompteur> RetourneListeCompteurLabo()
        {

            try
            {
                DBAccueil db = new DBAccueil();
                return db.RetourneListeCompteurLabo();
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }
        public List<CsCompteur> RetourneListeCompteurMagasin(List<string> lstCodeSite)
        {

            try
            {
                DBAccueil db = new DBAccueil();
                return db.RetourneListeCompteurMagasin(lstCodeSite);
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }
        public List<CsGroupe> RetourneListeGroupe(string codecentre)
        {

            try
            {
                DBAccueil db = new DBAccueil();
                return db.RetourneListeGroupe(codecentre);
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }
        public List<CsTypePanne> RetourneTypePanne()
        {

            try
            {
                DBAccueil db = new DBAccueil();
                return db.RetourneTypePanne();
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }
        public List<CsTypePanne> RetourneDetailTypePanne()
        {

            try
            {
                DBAccueil db = new DBAccueil();
                return db.RetourneDetailTypePanne();
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }
        public List<CsModeCommunication > RetourneModeCommunication()
        {

            try
            {
                DBAccueil db = new DBAccueil();
                return db.RetourneModeCommunication();
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }
        public List<CsVehicule> RetourneVehicule()
        {

            try
            {
                DBAccueil db = new DBAccueil();
                return db.RetourneVehicule();
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }
        public List<CsGroupeDepannageCommune> RetourneGroupeDepannageCommune()
        {

            try
            {
                DBAccueil db = new DBAccueil();
                return db.RetourneGroupeDepannageCommune();
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }

        public List<CsSortieMateriel> RetourneListeSortieMaterielLivre(int iddemande)
        {

            try
            {
                DBAccueil db = new DBAccueil();
                return db.RetourneListeSortieMaterielLivre(iddemande);
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }
        public List<CsSortieAutreMateriel> RetourneListeSortieAutreMaterielLivre(int iddemande)
        {

            try
            {
                DBAccueil db = new DBAccueil();
                return db.RetourneListeSortieAutreMaterielLivre(iddemande);
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }
        public bool InsertGroupe(CsGroupe legroupe, List<CsUtilisateur> lstAgent)
        {

            try
            {
                DBAccueil db = new DBAccueil();
                return db.InsertGroupe(legroupe, lstAgent);
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return false;
            }
        }
        public bool InsertCompteurMagasin(List<CsCompteur> lstCompt)
        {

            try
            {
                DBAccueil db = new DBAccueil();
                return db.InsertCompteurMagasin(lstCompt);
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return false;
            }
        }

        public List<CsDemandeBase> InsertLiaisonCompteur(List<CsDemandeBase> lstDemandeCanalisation)
        {

            try
            {
                DBAccueil db = new DBAccueil();
                return db.InsertLiaisonCompteur(lstDemandeCanalisation);
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null ;
            }
        }
        public bool DeLiaisonCompteur(CsDemande lstDemandeCanalisation)
        {

            try
            {
                DBAccueil db = new DBAccueil();
                return db.DeliaisonEclipseCompteur(lstDemandeCanalisation);
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return false ;
            }
        }


        public List<CsCanalisation> DeLiaisonCompteuriWEBS(CsDemande lstDemandeCanalisation, bool isDefectueux, bool isDoubleLiaison)
        {

            try
            {
                DBAccueil db = new DBAccueil();
                return db.DeliaisoniWEBS(lstDemandeCanalisation, isDefectueux, isDoubleLiaison);
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }

        public bool ReLiaisonCompteurSimple(CsDemande lstDemandeCanalisation)
        {

            try
            {
                DBAccueil db = new DBAccueil();
                return db.ReliaisonEclipseSimpleCompteur(lstDemandeCanalisation);
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return false;
            }
        }
        public bool InsertCompteurEvenementCannalisation(CsDemandeBase _LaDemande, CsCanalisation leCompteur, CsEvenement leEvt)
        {

            try
            {
                DBAccueil db = new DBAccueil();
                return db.InsertLiaisonCompteur(_LaDemande, leCompteur, leEvt);
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return false  ;
            }
        }
        public CsDemande InsertCompteurEvenementCannalisationMt(CsDemande  _LaDemande)
        {

            try
            {
                DBAccueil db = new DBAccueil();
                return db.InsertLiaisonCompteurMt(_LaDemande);
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null ;
            }
        }

        //public string  InsertProgrammation(Guid idgroupe, List<CsDemandeBase> lstDemande, DateTime pdate)
        //{

        //    try
        //    {
        //        DBAccueil db = new DBAccueil();
        //        return db.InsertProgrammation(idgroupe, lstDemande, pdate);
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorManager.WriteInLogFile(this, ex.Message);
        //        return string .Empty ;
        //    }
        //}
        public bool InsertSortieMaterielEP(int IdLivreur, int IdRecepteur, List<CsDemandeBase > lstCompteurValide, bool IsExtension)
        {
            try
            {
                DBAccueil db = new DBAccueil();
                return db.InsertSortieMaterielEP(IdLivreur, IdRecepteur, lstCompteurValide, IsExtension);
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return false;
            }
        }
 

  
        #endregion

        public CsDemande GetDevisByNumDemande(string pNumDemande)
        {
            try
            {
                ErrorManager.WriteInLogFile(this, "Debut =>" + System.DateTime.Now.ToLongTimeString());
                CsDemandeBase lademade = DBDEVIS.GetDevisByNumDemande(pNumDemande);
                DBAccueil db = new DBAccueil();
                CsDemande l = db.RetourneDetailDemandeFromDEvis(lademade);
                ErrorManager.WriteInLogFile(this, "Fin =>" + System.DateTime.Now.ToLongTimeString());
                return l;

            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }
        #endregion

        #region Devis
        public List<Galatee.Structure.CsScelle> LoadListeScelles(int IdAgentRec, int fk_TypeDemande)
        {
            try
            {
                DBScelle db = new DBScelle();
                return db.LoadListeScelle(IdAgentRec, fk_TypeDemande);
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }

        public List<Galatee.Structure.CsScelle> LoadListeScellesDemande(int IdAgentRec, int fk_TypeDemande, int Activite_ID)
        {
            try
            {
                DBDEVIS db = new DBDEVIS();
                return db.LoadListeScelle(IdAgentRec, fk_TypeDemande, Activite_ID);
                //return new List<Galatee.Structure.Scelle.CsScelle>();
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }



        #endregion

        #region Scelle
        public List<CsActivite> RetourneListeActivite()
        {
            try
            {
                DBScelle db = new DBScelle();
                return db.RetourneListeActivite();
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }

        public List<CsCouleurActivite> RetourneListeCouleurScelle(int Activite_ID)
        {

            try
            {
                DBScelle db = new DBScelle();
                return db.RetourneListeCouleurScelle(Activite_ID);
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }
        public List<CsDscelle> RetourneListeDemandeScelle(int fk_dem)
        {

            try
            {
                DBScelle db = new DBScelle();
                return db.RetourneListeDemandeScelle(fk_dem);
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }

        public List<CsLotScelle> RetourneListeScelle()
        {

            try
            {
                DBScelle db = new DBScelle();
                return db.RetourneListeScelle();
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }

        public List<CsLotScelle> RetourneListeScelleByCentre(int IdCentreRecuperationDeLot)
        {

            try
            {
                DBScelle db = new DBScelle();
                return db.RetourneListeScelle(IdCentreRecuperationDeLot);
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }



        public List<CsDetailAffectationScelle> RetourneListeDetailAffectationScelle(int IdDemande)
        {

            try
            {
                DBScelle db = new DBScelle();
                return db.RetourneListeDetailAffectationScelle(IdDemande);
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }



        public string ValidationReception(List<CsDetailAffectationScelle> ListeScelle, string MatAgent, int idetapeActuelle, string numdem)
        {
            try
            {
                DBScelle db = new DBScelle();
                return db.ValidationReception(numdem, idetapeActuelle, MatAgent);
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return ex.Message;
            }
        }
        public string InsertAffectionScelle(int id_lademande,string NUMDEM, int IdUser, int idEtapeActuelle, string Matricule, List<CsLotScelle> LstLotScelle)
        {

            try
            {
                DBScelle db = new DBScelle();
                return db.InsertAffectionScelle(id_lademande,NUMDEM, IdUser, idEtapeActuelle, Matricule, LstLotScelle);
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return ex.Message;
            }

        }
        public string InsertDemandeScelle(CsDemandeBase lademande, CsDscelle dscelle)
        {

            try
            {
                DBScelle db = new DBScelle();
                return db.InsertDemandeScelle(lademande, dscelle);
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return string.Empty;
            }

        }


        #endregion

        #region DEVIS
        public ObjDEVIS GetDevisByNumDevis(string pNumDevis)
        {
            try
            {
                return DBDEVIS.GetByNumDevis(pNumDevis);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public ObjDEVIS GetDevisByDevisId(int pDevisId)
        {
            try
            {
                return DBDEVIS.GetByIdDevis(pDevisId);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public List<ObjDEVIS> GetAllDevis()
        {
            try
            {
                return DBDEVIS.GetAllDevis();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public List<ObjDEVIS> GetDevisByCentre(string centre)
        {
            try
            {
                return DBDEVIS.GetByCentre(centre);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }
        public List<ObjDEVIS> GetDevisByIdPieceIdentite(int idPieceIdentite)
        {
            try
            {
                return DBDEVIS.GetByIdPieceIdentite(idPieceIdentite);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public List<ObjDEVIS> GetDevisByCodeProduit(int codeProduit)
        {
            try
            {
                return DBDEVIS.GetByCodeProduit(codeProduit);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public List<ObjDEVIS> GetDevisBySite(int site)
        {
            try
            {
                return DBDEVIS.GetBySite(site);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public List<ObjDEVIS> GetDevisByIdTypeDevis(int idTypeDevis)
        {
            try
            {
                return DBDEVIS.GetByIdTypeDevis(idTypeDevis);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public List<ObjDEVIS> GetDevisByIsAnalysed(bool isAnalysed)
        {
            try
            {
                return DBDEVIS.GetByIsAnalysed(isAnalysed);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public List<ObjDEVIS> GetDevisByCodeProduitIdTypeDevis(int codeProduit, int idTypeDevis)
        {
            try
            {
                return DBDEVIS.GetByCodeProduitIdTypeDevis(codeProduit, idTypeDevis);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public List<ObjDEVIS> GetDevisByIdEtapeDevis(int pIdEtapeDevis)
        {
            try
            {
                return DBDEVIS.GetByIdEtapeDevis(pIdEtapeDevis);

            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }
        #region AKO
        public List<CsDemandeBase> GetDevisByNumEtape(int pNumEtape)
        {
            try
            {
                return DBDEVIS.GetDevisByNumEtape(pNumEtape);

            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public CsDemande ChargerDetailDemandeConsultation(int pIdDemandeDevis)
        {
            try
            {
                return new DBAccueil().ChargerDetailDemandeConsultation(pIdDemandeDevis, string.Empty);
            }
            catch (Exception ex)
            {
                //ErrorManager.LogException(this, ex);
                ErrorManager.WriteInLogFile(this, ex.InnerException.InnerException.InnerException.InnerException.Message);

                return null;
            }
        }

        public CsDemande GetDevisByNumIdDevis(int pIdDemandeDevis)
        {
            try
            {
                return new DBAccueil().ChargerDetailDemande(pIdDemandeDevis, string.Empty);

                //pNumEtape
                //CsDemandeBase lademade = DBDEVIS.GetDevisByNumIdDevis(pIdDemandeDevis);
                //if(lademade==null)
                //    ErrorManager.LogException(this,new Exception( "chargé demande base ok"));

                //DBAccueil db = new DBAccueil();
                //CsDemande l = db.RetourneDetailDemandeFromDEvis(lademade);
                //if(l==null)
                //    ErrorManager.LogException(this,new Exception( "chargé detail demande  ok"));
                //return l;

            }
            catch (Exception ex)
            {
                //ErrorManager.LogException(this, ex);
                ErrorManager.WriteInLogFile(this, ex.InnerException.InnerException.InnerException.InnerException.Message);

                return null;
            }
        }


        public CsInfoDemandeWorkflow RetourneInfoDemandeWkf(CsDemandeBase laDemande)
        {
            return new DB_WORKFLOW().RecupererInfoDemandeParCodeDemande(laDemande);
        }
        public CsInfoDemandeWorkflow RetourneInfoDemandeWkfByIdDemande(int pIdDemandeDevis)
        {
            CsDemandeBase lademade = DBDEVIS.GetDevisByNumIdDevis(pIdDemandeDevis);
            return new DB_WORKFLOW().RecupererInfoDemandeParCodeDemande(lademade);
        }
        public List<CsLclient> ChargerCompteDeResiliation(CsClient leClient)
        {
            return new List<CsLclient>();
            //return new DBAccueil().ChargerCompteDeResiliation(leClient);
        }
        public List<CsLclient> ChargerFraisParticipation(CsClient leClient)
        {
            return new DBAccueil().ChargerFraisParticipation(leClient);
        }
        public bool ClotureValiderDemande(CsDemande LaDemannde)
        {
            //return new DBAccueil().ClotureValiderDemande(LaDemannde);
            return false;
        }

        #endregion

        public bool UpdateOrInsertElementsDeDevis(List<ObjELEMENTDEVIS> entityCollection, ObjMATRICULE pAgent)
        {
            try
            {
                return DBELEMENTSDEVIS.UpdateOrInsertElementsDeDevis(entityCollection, pAgent);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }
        public CsContrat  EditerContrat()
        {
            try
            {
                return new CsContrat();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null ;
            }
        }
        #endregion

        #region MATRICULE

        public List<CsUtilisateur> GetUserByIdFonctionMatriculeNom(string pIdFonction, string pMatriculeAgent, string pNomAgent)
        {
            try
            {
                return new DBAdmUsers().GetUserByIdFonctionMatriculeNom(pIdFonction, pMatriculeAgent, pNomAgent);

            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }
        public List<CsUtilisateur> GetUserByIdGroupeValidationMatriculeNom(Guid pIdGroupeValidation, int IdCentreDemande, string CodeProfil,string Matricule,string NomAgent)
        {
            try
            {
                return new DBAdmUsers().GetUserByIdGroupeValidationMatriculeNom(pIdGroupeValidation, IdCentreDemande,CodeProfil,Matricule ,NomAgent  );

            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }
        #endregion

        #region DENOMINATION

        public List<CsDenomination> SelectAllDenomination()
        {
            try
            {
                return new DB_Denomination().SelectAllDenomination();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
                //throw ex;
            }
        }

        #endregion

        #region PRODUIT

        public List<CsProduit> SelectAllProduit()
        {
            try
            {
                return new DB_Produit().SelectAllProduit();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public CsProduit SelectProduitByProduitId(int pProduitId)
        {
            try
            {
                return new DB_Produit().SelectProduitByProduitId(pProduitId);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        #endregion

        #region CATEGORIECLIENT

        public List<CsCategorieClient> SelectAllCategorieClient()
        {
            try
            {
                return new DB_CategorieClient().SelectAllCategorieClient();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        #endregion

        #region PARAMETRESGENERAUX
        public List<CsParametresGeneraux> SelectAllParametresGeneraux()
        {
            try
            {
                return new DB_ParametresGeneraux().SelectAllParametresGeneraux();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }
        public CsParametresGeneraux SelectParametresGenerauxByCode(string pCode)
        {
            try
            {
                return new DB_ParametresGeneraux().SelectParametresGenerauxByCode(pCode);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }
        #endregion

        #region NATIONALITE
        public List<CsNationalite> SelectAllNationalite()
        {
            try
            {
                return new DB_Nationalite().SelectAllNationalite();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }
        #endregion

        #region CENTRE

        public List<CsCentre> SelectAllCentre()
        {
            try
            {
                //return new DB_Centre().SelectAllCentre();
                return new DBAccueil().ChargerLesDonneesDesSite(false);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public CsCentre SelectCentreByIdSiteIdCentre(int pIdSite, int pIdCentre)
        {
            try
            {
                return new DB_Centre().SelectCentreByCodeSiteCodeCentre(pIdSite, pIdCentre);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public List<CsCentre> SelectCentreBySiteId(int pSiteId)
        {
            try
            {
                return new DB_Centre().SelectCentreBySiteId(pSiteId);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        #endregion

        #region SITE

        public List<CsSite> SelectAllSites()
        {
            try
            {
                return new DB_Site().SelectAllSite();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }
        #endregion

        #region Table TYPECENTRE

        public List<CsTypeCentre> SelectAllTypeCentre()
        {
            try
            {
                return new DB_TypeCentre().SelectAllTypeCentre();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        #endregion

        #region TYPEDEVIS

        public ObjTYPEDEVIS GetTypeDevisById(int id)
        {
            try
            {
                return DBTYPEDEVIS.GetById(id);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public List<ObjTYPEDEVIS> GetTypeDevisByProduitId(int ProduitId)
        {
            try
            {
                return DBTYPEDEVIS.GetByProduitId(ProduitId);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public List<ObjTYPEDEVIS> GetAllTypeDevis()
        {
            try
            {
                return DBTYPEDEVIS.GetAll();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public List<CsTdem> GetAllTypeDemande()
        {
            try
            {
                return DBTYPEDEMANDE.GetAll();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        #endregion

        #region TACHEDEVIS

        public ObjTACHEDEVIS GetTacheDevisById(int id)
        {
            try
            {
                return DBTACHEDEVIS.GetById(id);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public List<ObjTACHEDEVIS> GetAllTacheDevis()
        {
            try
            {
                return DBTACHEDEVIS.GetAll();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }
        #endregion
 
        #region ETAPEDEVIS

        public ObjETAPEDEVIS GetEtapeDevisById(int? id)
        {
            try
            {
                return DBETAPEDEVIS.GetById(id);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public List<ObjETAPEDEVIS> GetEtapeDevisByCodeProduit(int codeProduit)
        {
            try
            {
                return DBETAPEDEVIS.GetByCodeProduit(codeProduit);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public List<ObjETAPEDEVIS> GetEtapeDevisByIdTypeDevis(int idTypeDevis)
        {
            try
            {
                return DBETAPEDEVIS.GetByIdTypeDevis(idTypeDevis);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }


        #region FONCTION

        public List<CsFonction> SelectAllFonction()
        {
            try
            {
                return new DBFonction().SelectAllFonction();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }
        public CsFonction SelectFonctionByCode(int pCodeFonction)
        {
            try
            {
                return new DBFonction().SelectFonctionByCode(pCodeFonction);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }
        #endregion

        #region TOURNEE

        public List<CsTournee> ChargerLesTourneesDesSecteur(string pCentre, string pTournee)
        {
            try
            {
                return new DBAccueil().ChargerLesTournees();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }
        #endregion

        #region COMMUNE

        public List<CsCommune> SelectAllCommune()
        {
            try
            {
                return new DB_Commune().SelectAllCommune();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }
        #endregion

        #region QUARTIER

        public List<CsQuartier> SelectAllQuartier()
        {
            try
            {
                return new DB_QUARTIER().SelectAllQuartier();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public List<CsQuartier> SelectAllQuartierByCommune(CsCommune pCommune)
        {
            try
            {
                return new DB_QUARTIER().SelectAllQuartierByCommune(pCommune.PK_ID);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }
        #endregion

        #region RUES

        public List<CsRues> SelectAllRues()
        {
            try
            {
                return new DB_RUES().SelectAllRues();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public List<CsRues> SelectAllRuesByCommune(CsCommune pCommune)
        {
            try
            {
                return new DB_RUES().SelectAllRuesByCommune(pCommune.PK_ID);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }
        #endregion

        #region CALIBRECOMPTEUR

        public List<CsCalibreCompteur> SelectAllCalibreCompteur()
        {
            try
            {
                return new DB_CALIBRECOMPTEUR().SelectAllCalibreCompteur();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public List<CsCalibreCompteur> SelectAllCalibreCompteurByCentre(int pCentre)
        {
            try
            {
                return new DB_CALIBRECOMPTEUR().SelectAllCalibreCompteurByCentre (pCentre);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public List<CsCalibreCompteur> SelectAllCalibreCompteurByProduit(int pProduit)
        {
            try
            {
                return new DB_CALIBRECOMPTEUR().SelectAllCalibreCompteurByProduit (pProduit);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }
        #endregion

        #region REGLAGECOMPTEUR

        public List<CsReglageCompteur> SelectAllReglageCompteur()
        {
            try
            {
                return new DB_REGLAGECOMPTEUR().SelectAllReglageCompteur();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public List<CsReglageCompteur> SelectAllReglageCompteurByCentre(int pCentre)
        {
            try
            {
                return new DB_REGLAGECOMPTEUR().SelectAllReglageCompteurByCentre(pCentre);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public List<CsReglageCompteur> SelectAllReglageCompteurByProduit(int pProduit)
        {
            try
            {
                return new DB_REGLAGECOMPTEUR().SelectAllReglageCompteurByProduit(pProduit);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }
        #endregion

        #region SUIVIDEVIS

        public ObjSUIVIDEVIS GetSuiviDevisByDevisIdEtape(int num, int etape)
        {
            try
            {
                return DBSUIVIDEVIS.GetSuiviDevisByDevisIdEtape(num, etape);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public List<ObjSUIVIDEVIS> GetSuiviDevisByDevisId(CsCriteresDevis pCriteresDevis)
        {
            try
            {
                return DBSUIVIDEVIS.GetSuiviDevisByDevisId(pCriteresDevis);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public ObjSUIVIDEVIS GetSuiviDevisByDevisIdProduitIdEtape(int num, string pProduit, int pIdEtape)
        {
            try
            {
                return DBSUIVIDEVIS.GetSuiviDevisByDevisIdProduitIdEtape(num, pIdEtape);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }


        #endregion

        #region DEPOSIT

        public ObjDEPOSIT SearchByNumDevis(string numDevis)
        {
            try
            {
                return DBDEPOSIT.SearchByNumDevis(numDevis);

            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public List<ObjDEPOSIT> SearchByReceipt(string receipt)
        {
            try
            {
                return DBDEPOSIT.SearchByReceipt(receipt);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public List<ObjDEPOSIT> ListeDepot()
        {

            try
            {
                return DBDEPOSIT.SearchAllDeposit();

            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        #endregion

        #region DOCUMENTSCANNE

        public ObjDOCUMENTSCANNE GetDocumentScanneById(Guid pId)
        {
            try
            {
                return DBDOCUMENTSCANNE.GetDocumentScanneById(pId);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        #endregion

        #region DEVISPIA
        public ObjDEVISPIA GetDevisPiaByDevisIdOrdre(int IdDevis, int ordre)
        {
            try
            {
                return DBDEVISPIA.GetByDevisIdOrdre(IdDevis, ordre);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public ObjDEVISPIA GetDevisPiaById(int pId)
        {
            try
            {
                return DBDEVISPIA.GetById(pId);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }
        #endregion

        #region FOURNITURE

        public List<ObjFOURNITURE> SelectFournituresByCodeProduitByIdTypeDevis(int pCodeProduit, int pIdTypeDevis, string pDiametre)
        {
            try
            {
                return DBFOURNITURE.SelectFournituresByCodeProduitByIdTypeDevis(pCodeProduit, pIdTypeDevis, pDiametre);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public ObjFOURNITURE SelectFournituresByNumFourniture(int pNumFourniture)
        {
            try
            {
                return DBFOURNITURE.SelectFournituresByNumFourniture(pNumFourniture);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public bool DeleteFourniture(List<ObjFOURNITURE> entityCollection)
        {
            try
            {
                return DBFOURNITURE.DeleteFourniture(entityCollection);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public List<ObjFOURNITURE> GetAllFourniture()
        {
            try
            {
                return DBFOURNITURE.GetAllFourniture();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public List<ObjFOURNITURE> GetFournitureByIdTypeDevis(int idTypeDevis)
        {
            try
            {
                return DBFOURNITURE.GetFournitureByIdTypeDevis(idTypeDevis);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public ObjFOURNITURE GetFournitureByNumFourniture(int numFourniture)
        {
            try
            {
                return DBFOURNITURE.GetFournitureByNumFourniture(numFourniture);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public bool InsertFourniture(List<ObjFOURNITURE> entityCollection)
        {
            try
            {
                return DBFOURNITURE.InsertFourniture(entityCollection);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool UpdateFourniture(List<ObjFOURNITURE> entityCollection)
        {
            try
            {
                return DBFOURNITURE.UpdateFourniture(entityCollection);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        #endregion

        #region ELEMENTSDEVIS

        public bool InsertUnElementsDevis(ObjELEMENTDEVIS entity)
        {
            try
            {
                return DBELEMENTSDEVIS.InsertElementsDevis(entity);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool InsertUneListeElementsDevis(List<ObjELEMENTDEVIS> entityCollection)
        {
            try
            {
                return DBELEMENTSDEVIS.InsertElementsDevis(entityCollection);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool UpdateUnElementsDevis(ObjELEMENTDEVIS entity)
        {
            try
            {
                return DBELEMENTSDEVIS.UpdateElementsDevis(entity);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool UpdateUneListeElementsDevis(List<ObjELEMENTDEVIS> entityCollection)
        {
            try
            {
                return DBELEMENTSDEVIS.UpdateElementsDevis(entityCollection);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool DeleteUnElementsDevis(ObjELEMENTDEVIS entity)
        {
            try
            {
                return DBELEMENTSDEVIS.DeleteElementsDevis(entity);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool UpdateElementsDevisConsomme(List<ObjELEMENTDEVIS> _lElements)
        {
            try
            {
                return DBELEMENTSDEVIS.UpdateElementsDevisConsomme(_lElements);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool UpdateElementsDevisRemisEnStock(List<ObjELEMENTDEVIS> _lElements)
        {
            try
            {
                return DBELEMENTSDEVIS.UpdateElementsDevisRemisEnStock(_lElements);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool UpdateElementsDevisValide(List<ObjELEMENTDEVIS> _lElements)
        {
            try
            {
                return DBELEMENTSDEVIS.UpdateElementsDevisValide(_lElements);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public List<ObjELEMENTDEVIS> SelectElementsDevisByDevisId(int numDevis, int ordre, bool isSummary)
        {
            try
            {
                return DBELEMENTSDEVIS.SelectElementsDevisByDevisId(numDevis, ordre, isSummary);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public List<ObjELEMENTDEVIS> DEVIS_ELEMENTDEVIS_SelByDevisById(int pIdDevis)
        {
            try
            {
                return DBELEMENTSDEVIS.SelectElementsDevisByDevisId(pIdDevis);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }


        public List<ObjELEMENTDEVIS> SelectElementsDevisConsommeByDevisId(int numDevis, int ordre)
        {
            try
            {
                return DBELEMENTSDEVIS.SelectElementsDevisConsommeByDevisId(numDevis, ordre);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public List<ObjELEMENTDEVIS> SelectElementsDevisByDevisIdForValidationRemiseStock(int numDevis, int ordre, bool isSummary)
        {
            try
            {
                return DBELEMENTSDEVIS.SelectElementsDevisByDevisIdForValidationRemiseStock(numDevis, ordre, isSummary);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        //public List<ObjELEMENTDEVIS> SelectElementsDevisComparaisonByNumDevis(string numDevis, byte ordre)
        //{
        //    try
        //    {
        //        return DBELEMENTSDEVIS.SelectElementsDevisComparaisonByNumDevis(numDevis, ordre);
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorManager.LogException(this, ex);
        //        return null;
        //    }
        //}

        //public byte? SelectElementsDevisMaxOrdre(string numDevis)
        //{
        //    try
        //    {
        //        return DBELEMENTSDEVIS.SelectElementsDevisMaxOrdre(numDevis);
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorManager.LogException(this, ex);
        //        return null;
        //    }
        //}

        #endregion

        #region CTAX

        public List<CsCtax> GetAllCtax()
        {
            try
            {
                return new DBCTAX().GetAll();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public CsCtax GetCtaxByCENTRECTAXDEBUTAPPLICATION(CsCtax entity)
        {
            try
            {
                return new DBCTAX().GetByCENTRECTAXDEBUTAPPLICATION(entity);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        #endregion

        #region BANQUE
        public List<aBanque> SelectAllBanque()
        {
            try
            {
                return new DB_BANQUES().SelectAllBanque();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }
        #endregion

        #region MARQUECOMPTEUR

        public List<CsMarqueCompteur> SelectAllMarqueCompteur()
        {
            try
            {
                return new DBMarqueCompteur().SelectAllMarqueCompteur();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        #endregion

        #region TCOMPT

        public List<CsTcompteur> SelectAllTcompt()
        {
            try
            {
                return new DB_TCOMPT().SelectAllTcompt();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        #endregion

        #region INTERVENTIONPLANNIFIEE


        public List<CsInterventionPlannifiee> GetByResponsable(string pMatricule)
        {
            try
            {
                return new DBINTERVENTIONPLANNIFIEE().GetByResponsable(pMatricule);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        #endregion

        #region TRAVAUXDEVIS

        public ObjTRAVAUXDEVIS SelectTravaux(int pDevisId, int Ordre)
        {
            try
            {
                return DBTRAVAUXDEVIS.SelectTravaux(pDevisId, Ordre);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }
        #endregion

        #region CONTROLETRAVAUX


        public CsControleTravaux SelectControles(int numDevis, int Ordre)
        {
            try
            {
                return DBCONTROLETRAVAUX.SelectControles(numDevis, Ordre);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public bool InsertControl(CsControleTravaux controle)
        {
            try
            {
                return DBCONTROLETRAVAUX.InsertControl(controle);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool Updatecontroles(CsControleTravaux controle)
        {
            try
            {
                return DBCONTROLETRAVAUX.Updatecontroles(controle);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        #endregion

        #region PUISSANCE

        public List<CsPuissance> GetPuissanceParProduit(string pProduit)
        {
            try
            {
                return new DB_PUISSANCE().GetPuissanceParProduit(pProduit);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public List<CsPuissance> GetPuissanceParProduitId(int pProduitId)
        {
            try
            {
                return new DB_PUISSANCE().GetPuissanceParProduitId(pProduitId);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }
        #endregion

        #region ENTREPRISE

        public bool DeleteEntreprise(CsEntreprise entity)
        {
            try
            {
                return new DBENTREPRISE().Delete(entity);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public CsEntreprise GetAllEntreprise()
        {
            try
            {
                return new DBENTREPRISE().GetAll();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public CsEntreprise GetEntrepriseById(CsEntreprise entity)
        {
            try
            {
                return new DBENTREPRISE().GetById(entity);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public bool InsertEntreprise(CsEntreprise entity)
        {
            try
            {
                return new DBENTREPRISE().Insert(entity);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool UpdateEntreprise(CsEntreprise entity)
        {
            try
            {
                return new DBENTREPRISE().Update(entity);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        #endregion
        #region PARAMETRE
        public CParametre  RetourneParametre()
        {
            try
            {
                return new CParametre();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }
        #endregion

        public List<CsCommune> ChargerLesBrancheDesCommune()
        {
            try
            {

                DBAccueil db = new DBAccueil();
                return db.ChargerLesCommune();
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }
        public List<CsQuartier> ChargerLesQartiersDesCommune()
        {
            try
            {

                DBAccueil db = new DBAccueil();
                return db.ChargerLesQartiers();
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }
        public List<CsSecteur> ChargerLesSecteursDesQuartier()
        {
            try
            {

                DBAccueil db = new DBAccueil();
                return db.ChargerLesSecteurs();
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }
        public List<CsTournee> ChargerLesTourneesDesSecteur()
        {
            try
            {

                DBAccueil db = new DBAccueil();
                return db.ChargerLesTournees();
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }
        public List<CsReglageCompteur > ChargerRegalgeCompteur()
        {
            try
            {
                return new DB_REGLAGECOMPTEUR().SelectAllReglageCompteur();
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }
        public List<ObjAPPAREILSDEVIS> GetAppareilByDevis(string NumDevis)
        {
            try
            {
                DBAPPAREILS db = new DBAPPAREILS();
                return db.GetAppareilByDevis(NumDevis);
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }
        public List<CsFactureBrut> CalculFactureResilation(List<CsEvenement> lstEvenement)
        {
            try
            {
                DBAccueil db = new DBAccueil();
                bool Res = db.ValiderCreationFactureIsole(lstEvenement);
                if (Res == true)
                {
                    List<CsLotri> lstLot = new List<CsLotri>();
                    lstLot.Add(new CsLotri
                    {
                        BASE = "S",
                        CATEGORIECLIENT = "99",
                        CENTRE = lstEvenement.First().CENTRE,
                        FK_IDCENTRE = lstEvenement.First().FK_IDCENTRE,
                        DATECREATION = DateTime.Now,
                        DATEMODIFICATION = DateTime.Now,
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
                        USERMODIFICATION = lstEvenement.First().USERCREATION,
                        MATRICULE = lstEvenement.First().USERCREATION

                    });

                    DBCalcul dbs = new DBCalcul();
                    List<CsFactureBrut> lstFacture = dbs.CalculeDuLot(lstLot, false);
                    List<CsFactureBrut> lstFactureclient = lstFacture.Where(t => t.FK_IDABON == lstEvenement.First().FK_IDABON).ToList();
                    if (lstFactureclient.Count != 0)
                    { lstFactureclient.ForEach(t => t.NOMBRECALCULE = 1); lstFactureclient.ForEach(t => t.REJETE  = 0); }
                    return lstFactureclient;
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }

        public bool ValiderFacturation(List<CsFactureBrut> laFacturation, bool IsFactureResil)
        {
            try
            {
                DBCalcul db = new DBCalcul();
                return db.ValiderFacturation(laFacturation, IsFactureResil);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }
        public List<CsFactureClient> EditionFactureResiliation(List<CsFactureBrut> laFacturation)
        {
            try
            {
                DbFacturation db = new DbFacturation();
                return db.EditionFactureResiliation(laFacturation);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null ;
            }
        }
        public bool AnnulerFactureResiliation(List<CsFactureBrut> laFacturation)
        {
            try
            {
                DBCalcul db = new DBCalcul();
                return db.AnnulerFactureResiliation(laFacturation);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false ;
            }
        }
        public bool AutorisationDemande(List<CsDemandeDetailCout > leFacturation)
        {
            try
            {
                DBAccueil db = new DBAccueil();
                return db.AutorisationDemande(leFacturation);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public List<CsClient> RetourneClientByReference(string client, List<int> idCentre)
        {
            try
            {
                DBAccueil db = new DBAccueil();
                return db.RetourneClientByReference(client, idCentre);
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }

        public List<CsClient> RetourneClientByReferenceOrdre(string client, string Ordre, List<int> idCentre)
        {
            try
            {
                DBAccueil db = new DBAccueil();
                return db.RetourneClientByReferenceOrdre(client,Ordre, idCentre);
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }

        public string ValiderDemandeRemboursementAvance(CsDemande LaDemannde,List<CsLclient> lesFacture)
        {
            return new DBAccueil().ValiderDemandeDevis(LaDemannde);
        }


        public string ValiderDemandeTransfert(CsDemande LaDemannde)
        {
            return new DBAccueil().ValiderDemandeTransfert(LaDemannde);
        }

        public bool LettrageSuiteAPDP(CsDemande LaDemande)
        {
            return new DBAccueil().LettrageSuiteAPDP(LaDemande);
        }

        public List<CsTypeComptage> RetourneTypeComptage(int? nbrtransfo,int puissanceSouscrite,int puissanceInstalle)
        {
            return new DBIndex().GetTypeComptage(nbrtransfo, puissanceSouscrite, puissanceInstalle);
        }

        public List<ObjELEMENTDEVIS> RetourneElementDEvisFromIdDemande(List<int> idDemande)
        {
            return new DBAccueil().RetourneElementDEvisFromIdDemande(idDemande);
         
        }

        public List<CsCanalisation > RetourneAncienCompteur(CsDemandeBase laDemande)
        {
            List<CsCanalisation> lstCompteur = new DBAccueil().RetourneCanalisation(laDemande.FK_IDCENTRE ,laDemande.CENTRE ,laDemande.CLIENT ,laDemande.PRODUIT ,null );
            foreach (CsCanalisation item in lstCompteur)
            {
                List<CsEvenement> lstDernEvt = RetourneDernierEvenementFacturer(item.FK_IDCENTRE, item.CENTRE, item.CLIENT, laDemande.ORDRE, item.PRODUIT, item.POINT);
                if (lstDernEvt!= null && lstDernEvt.Count != 0)
                    item.INDEXEVT = lstDernEvt.First().INDEXEVT;
            }
            return lstCompteur;

        }
        public List<CsEvenement > RetourneDernierEvtFacture(CsDemandeBase laDemande)
        {
            if (laDemande.PRODUIT != Enumere.Prepaye)
            {
                List<CsAbon> leAbon = new DBAccueil().RetourneAbon(laDemande.FK_IDCENTRE, laDemande.CENTRE, laDemande.CLIENT, laDemande.ORDRE);
                if (leAbon != null && leAbon.Count != 0)
                {
                    CsAbon lAbon = leAbon.FirstOrDefault(y => y.PRODUIT == laDemande.PRODUIT);
                    CsClient leClient = new CsClient()
                    {
                        CENTRE = lAbon.CENTRE,
                        REFCLIENT = lAbon.CLIENT,
                        ORDRE = lAbon.ORDRE,
                        FK_IDCENTRE = lAbon.FK_IDCENTRE,
                        FK_IDABON = lAbon.PK_ID,
                        PRODUIT = lAbon.PRODUIT,
                        FK_IDPRODUIT = lAbon.FK_IDPRODUIT
                    };
                    return new DbFacturation().RetourneEvenementSpx(leClient.FK_IDCENTRE.Value, leClient.FK_IDABON.Value, leClient.FK_IDPRODUIT.Value, leClient.PERIODE);
                }
            }
            return null;
        }
        public List<CsTarifClient> RetourneTarifClient(int idcentre,int idcategorie,int idreglageCompteur,int? idtypecomptage,string propriotaire,int idproduit)
        {
            return new DBCalcul().RetourneTarifClient(idcentre,idcategorie,idreglageCompteur,idtypecomptage,propriotaire,idproduit);
        }

        public CsClient VerifieMatriculeAgent(string MatriculeAgent)
        {
            return new DBAccueil().VerifieMatriculeAgent(MatriculeAgent);
        }
        public List<CsRubriqueDevis> RetourneTypeMateriel()
        {
            return new DBAccueil().RetourneTypeMateriel();
        }
        public List<CsEvenement> RetourneEvenementClient(CsClient LeClient)
        {
            try
            {
                return new DBAccueil().RetourneEvenementClient(LeClient);
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }
        public CsEvenement VerifieEvenementNonFacturer(CsEvenement leEvt)
        {
            try
            {
                DBAccueil db = new DBAccueil();
                return db.VerifieEvenementNonFacturer(leEvt);
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null  ;
            }
        }
       public CsInfoDemandeWorkflow RetourneEtapeDemande(CsDemandeBase laDemande)
       {
           try
           {
               return new DB_WORKFLOW().RecupererInfoDemandeParCodeDemande(laDemande);
           }
           catch (Exception ex)
           {
               ErrorManager.WriteInLogFile(this, ex.Message);
               return null;
           }
       }
        #endregion
        #region TRANSITION
        public CsDemande GetDemandeByRefClient(string  pNumDemandeDevis)
        {
            try
            {
                DBAccueil db = new DBAccueil();
                CsDemandeBase lademade = db.RetourneLaDemande(pNumDemandeDevis);
                return db.RetourneDetailDemandeFromDEvisTransition(lademade);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }
        public CsDemandeBase   ValiderMiseAjourDemandeTransition(CsDemande pDemandeDevis)
        {
            try
            {
                DBAccueil db = new DBAccueil();
                return db.MiseAjoursAbonBrtCreation(pDemandeDevis);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null ;
            }
        }
       public bool ValiderCreation(CsDemande pDemandeDevis)
        {
            try
            {
                DBAccueil db = new DBAccueil();
                return db.ValiderCreation(pDemandeDevis);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }
        

        public List<CsDemande> GetDemandeByTypdeDemande(string Typedemande)
        {
            try
            {
                DBAccueil db = new DBAccueil();
                List<CsDemande> lesDemande = new List<CsDemande>();
                List<CsDemandeBase> lademade = db.GetDemandeByTypdeDemande(Typedemande);
                foreach (CsDemandeBase item in lademade)
                {
                    lesDemande.Add(db.RetourneDetailDemandeFromDEvis(item));
                }
                return lesDemande;
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }
        public bool? InsererCompteurEvtTransition(CsDemande demande)
        {
            try
            {
                DBAccueil db = new DBAccueil();
                return db.InsererCompteurEvtTransition(demande);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public List<CsCanalisation> GetDemandeByListeNumIdDemandeSortieCompteur(DateTime? DateProgramme, Guid IdEquipe, int? EtapeActuelle)
        {
            try
            {
                return new DBAccueil().GetDemandeByNumIdDemandeSpx(DateProgramme, IdEquipe, EtapeActuelle);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public List<CsCanalisation> GetDemandeByListeNumIdDemandeSortieMateriel(DateTime? DateProgramme, Guid IdEquipe, int? EtapeActuelle)
        {
            try
            {
                return new DBAccueil().GetDemandeByListeNumIdDemandeSortieMaterielSpx(DateProgramme, IdEquipe, EtapeActuelle);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }
        public List<CsMandatementGc> MandatementClient(CsRegCli leClient)
        {
            try
            {
                return new DBAccueil().RetourneMandatementSpx(leClient);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }
        


        #endregion
        public bool InsertionLigneComptableGenerer(List<CsEcritureComptable> LigneComptable)
        {
            try
            {
                DbInterfaceComptable db = new DbInterfaceComptable();
                return db.InsertionLigneComptable(LigneComptable);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }
        public List<CsEcritureComptable> IsOperationExiste(List<CsEcritureComptable> LigneComptable)
        {
            return new DbInterfaceComptable().IsOperationExiste(LigneComptable);
        }
        #region scelle
        public List<CsScelle> RetourneScellesListeByAgence(int IdCentre)
        {
            try
            {
                DBScelle db = new DBScelle();
                return db.RetourneScellesListeByAgence(IdCentre);
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }
        public int InsertRemise(List<CsRemiseScelles> sRemi)
        {
            try
            {
                return new DBScelle().Insert(sRemi);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                throw ex;
            }
        }
        public List<CsTbLot> RetourneLotDeScelleAffectation(int IdCentre)
        {
            try
            {
                DBScelle db = new DBScelle();
                return db.RetourneLotDeScelleAffectation(IdCentre);
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }

        public string  VerificationDemandeTransfert(CsDtransfert demandetrf,CsDemandeBase laDemande)
        {
            System.Data.SqlClient.SqlCommand laCommande = DBBase.InitTransaction(Session.GetSqlConnexionString());
            try
            {
                new DbWorkFlow().ModifierCircuitTransferAbonnement(demandetrf, laCommande);
                new DBAccueil().TransmettreDemande(laDemande.NUMDEM, laDemande.FK_IDETAPEENCOURE, laDemande.MATRICULE, laCommande);
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


        public List<CsProgarmmation> ChargerListeProgram(List<int> lstIdCentre, string NumerodeProgramme, DateTime? DateDebut, DateTime? DateFin, string IdEquipe, int idEtape, bool IsCompteur)
        {
            try
            {
                return new DBAccueil().ChargerListeProgram(lstIdCentre,NumerodeProgramme, DateDebut, DateFin, IdEquipe, idEtape, IsCompteur);
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null ;
            }
        }
        public List<CsProgarmmation> ChargerListeProgramReedition(List<int> lstIdCentre,string NumerodeProgramme, DateTime? DateDebut, DateTime? DateFin, string IdEquipe, int TypeEtat)
        {
            try
            {
                return new DBAccueil().ChargerListeProgramReedition(lstIdCentre,NumerodeProgramme, DateDebut, DateFin, IdEquipe, TypeEtat);
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null ;
            }
        }
        public List<ObjELEMENTDEVIS> ChargerListeDonneeProgramReedition(string NumProgramme, DateTime? DateDebut, Guid IdEquipe)
        {
            try
            {
                return new DBAccueil().ChargerListeDonneeProgramReedition(NumProgramme,DateDebut, IdEquipe);
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }


/*
        public List<CsTarif> ChargerTypeTarif(int produit, int? puissanceSouscrite, int? Categorie, int? IdReglage)
        {
            try
            {
                return new DBAccueil().ChargerTypeTarif( produit, puissanceSouscrite,Categorie,IdReglage);
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }
*/


        public List<CsTarif> ChargerTypeTarif(int produit, int? puissanceSouscrite, int? Categorie, int? IdReglage, int? idTarif)
        {
            try
            {
                return new DBAccueil().ChargerTypeTarif(produit, puissanceSouscrite, Categorie, IdReglage, idTarif);
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }


        public bool SupprimerDevenement(CsDemande laDemande)
        {
            try
            {
                return new DBAccueil().SupprimerDevenement(laDemande);
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return false ;
            }
        }

        public bool ValiderActionSurDemande(CsDemande laDemande)
        {
            try
            {
                return new DBAccueil().ValiderActionSurDemande(laDemande);
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return false;
            }
        }


        #endregion
  
        #region Sylla 09/01/2017
        public bool VerifieCompteurExisteNew(CsCompteur leCpt)
        {
            try
            {
                return new DBScelle().VerifieCompteurExistenew(leCpt);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }
        #endregion
        public CsCanalisation VerifieSiCompteurExiste(CsCompteur Compteur)
        {
            try{
            return new DBAccueil().VerifieCompteurExistenew(Compteur);
             }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }
        public bool LicenceOK()
        {
            try
            {
                return new DBAccueil().LicenceOK();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }
        public bool LicenseLooking()
        {
            try
            {
                return new DBAccueil().LicenseLooking();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        #region traitement ADO.net
        public Dictionary<CsGroupeValidation, List<CsUtilisateur>> ActeurEtape(int IdEtape, int Iddemande)
        {
            try
            {
                return new DBAccueil().ActeurEtape(IdEtape, Iddemande);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }




        public CsAbon VerifierMatriculeAgent(string matricule)
        {
            try
            {
                return new DBAccueil().VerifierMatriculeAgent(matricule);
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

    
        public CsDemandeBase CreeDemande(CsDemande LaDemande, bool AvecTransmission)
        {
            try
            {
                return new DBAccueil().CreeDemande(LaDemande, true);
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                ErrorManager.LogException(this, ex);
                return null;
            }
        }
        public string TransmettreDemande(List<CsDemandeBase> pDemande)
        {
            DBAccueil db = new DBAccueil();
            return db.TransmettreDemande(pDemande);
        }
        public string TransmissionDemande(string NUMDEM, int idEtapeActuel, string MATRICULE)
        {
            try
            {
                DBAccueil db = new DBAccueil();
                db.TransmettreDemande(NUMDEM, idEtapeActuel, MATRICULE);
                return string.Empty;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public string AffecterDemande(List<CsAffectationDemandeUser> lesAffectations)
        {
            try
            {
                return new DBAccueil().ValiderAffectation(lesAffectations);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }
        public string ValiderMetre(CsDemande laDemande, bool AvecTransmission)
        {
            try
            {
                return new DBAccueil().EtablissementDevis(laDemande, AvecTransmission);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }
        public string ValiderEtablissementDevis(CsDemande laDemande, bool AvecTransmission)
        {
            try
            {
                return new DBAccueil().ValiderEtablissementDevis(laDemande, AvecTransmission);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }



        public string ValiderRepriseIndex(CsDemande laDemande)
        {

            try
            {
                return new DBIndex().ValiderRepriseIndex(laDemande);
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }


        public string ValiderAnnulationFacture(CsDemande laDemande, int idEntfac)
        {

            try
            {
                return new DBCalcul().ValiderAnnulationFacture(laDemande, idEntfac);
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }




        public string ValidationDemande(CsDemande laDemande, bool AvecTransmission)
        {
            try
            {
                return new DBAccueil().ValiderDemande(laDemande, AvecTransmission);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }
        public string VerificationDemande(CsDemande laDemande, bool AvecTransmission)
        {
            return new DBAccueil().VerifierDemande(laDemande, AvecTransmission);

        }
        public string ValiderDevis(CsDemande laDemande, bool AvecTransmission)
        {
            return string.Empty;
        }
        public string ValiderInformationAbonnement(CsDemande laDemande, bool AvecTransmission)
        {
            return new DBAccueil().ValiderInformationAbonnement(laDemande, AvecTransmission);
        }
        public string InsertProgrammation(Guid idgroupe, List<CsDemandeBase> lstDemande, DateTime pdate)
        {

            try
            {
                DBAccueil db = new DBAccueil();
                return db.InsertProgrammation(idgroupe, lstDemande, pdate);
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return string.Empty;
            }
        }
        public string InsertSortieCompteur(string programme, int IdLivreur, int IdRecepteur, int idEtape, List<CsCanalisation> lstCompteur)
        {
            try
            {
                return new DBAccueil().InsertSortieCompteur(programme, IdLivreur, IdRecepteur, idEtape, lstCompteur);

            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public string InsertSortieMateriel(int IdLivreur, int IdRecepteur, int idEtape, List<CsCanalisation> lstCompteurValide, bool IsExtension)
        {
            try
            {
                DBAccueil db = new DBAccueil();
                return db.InsertSortieMateriel(IdLivreur, IdRecepteur, idEtape, lstCompteurValide, IsExtension);
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return ex.Message;
            }
        }
        public string ProcesVerbal(CsDemande laDemande, bool AvecTransmission)
        {
            try
            {
                ErrorManager.WriteInLogFile(this, "ProcesVerbal");
                return new DBAccueil().ProcesVerbal(laDemande, AvecTransmission);
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return ex.Message;
            }
        }

        public List<CsCanalisation> GetDemandeByListeNumIdDemandeSortieCompteur(string NumeroProgramme, int? EtapeActuelle)
        {
            try
            {
                return new DBAccueil().GetDemandeByNumIdDemandeSpx(NumeroProgramme, EtapeActuelle);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public List<CsCanalisation> GetDemandeByListeNumIdDemandeSortieMateriel(string NumeroProgramme, int? EtapeActuelle)
        {
            try
            {
                return new DBAccueil().GetDemandeByListeNumIdDemandeSortieMaterielSpx(NumeroProgramme, EtapeActuelle);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public List<ObjELEMENTDEVIS> ChargerListeDonneeProgramReedition(string NumProgramme)
        {
            try
            {
                return new DBAccueil().ChargerListeDonneeProgramReedition(NumProgramme);
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }

        public CsDemande ChargerDetailClient(CsClient leclient)
        {
            try
            {
                DBAccueil db = new DBAccueil();
                var _lademande = db.ChargerDetailClient(leclient.FK_IDCENTRE.Value, leclient.CENTRE, leclient.REFCLIENT, leclient.ORDRE, leclient.PRODUIT, leclient.TYPEDEMANDE);
                return _lademande;
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }
        public CsDemande ChargerDetailDemande(int idDemande, string NumDemande)
        {
            try
            {
                return new DBAccueil().ChargerDetailDemande(idDemande, NumDemande);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }


        public bool VerifierRemboursementEnCours(CsDemande dem)
        {
            try
            {
                return new DBAccueil().SelectRemboursementEnCours(dem);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }



 /*       public List<ObjDOCUMENTSCANNE> DocumentScanneContenu(ObjDOCUMENTSCANNE LeDocument)
        {
            try
            {
                return new DBAccueil().Select_DocumentContenut(LeDocument.FK_IDDEMANDE, LeDocument.FK_IDTYPEDOCUMENT.Value);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }*/




        public ObjDOCUMENTSCANNE DocumentScanneContenu(ObjDOCUMENTSCANNE LeDocument)
        {
            try
            {
                return new DBAccueil().Select_DocumentContenut(LeDocument.PK_ID);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }



        public CsLotri RetourneLotri()
        {
            return new CsLotri();
        }
        #endregion




        public string CreationDemandeSuiteRejet(CsDemande LaDemande, bool AvecTransmission)
        {
            try
            {
                return new DBAccueil().CreationDemandeSuiteRejet(LaDemande, true);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }



        public string CreeDemandeExtension(CsDemandeBase _LaDemandeMiseAJoure)
        {
            try
            {
                DBAccueil db = new DBAccueil();
                return db.CreeDemandeExtension(_LaDemandeMiseAJoure);
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return "";
            }
        }

        public string AnnulationDemande(CsDemandeBase _LaDemandeMiseAJoure)
        {
            try
            {
                DBAccueil db = new DBAccueil();
                return db.AnnulationDemande(_LaDemandeMiseAJoure);
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return "";
            }
        }


        public string ValiderOrdreDeTravail(CsDemande laDemande, CsAffectationDemandeUser leAffection, bool AvecTransmission)
        {
            try
            {
                return new DBAccueil().ValiderOrdreDeTravail(laDemande, leAffection, AvecTransmission);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }
        public string ValiderSuivieTravaux(CsDemande laDemande, bool AvecTransmission)
        {
            try
            {
                return new DBAccueil().ValiderSuivieTravaux(laDemande, AvecTransmission);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }



        public bool SupprimerPieceJointe(ObjDOCUMENTSCANNE piece)
        {
            try
            {
                return new DBAccueil().Delete_DocumentScane(piece);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }
    }
}