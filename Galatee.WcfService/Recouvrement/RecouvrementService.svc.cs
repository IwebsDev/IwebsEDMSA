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
using System.ServiceModel.Activation;

namespace WcfService
{
    // REMARQUE : vous pouvez utiliser la commande Renommer du menu Refactoriser pour changer le nom de classe "RecouvrementService" à la fois dans le code, le fichier svc et le fichier de configuration.
   [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)] 
    public class RecouvrementService : IRecouvrementService
    {
        static Dictionary<string, string> parametreReglement = new Dictionary<string, string>();
        static Dictionary<string, List<aCampagne>> dicosCampagnes = new Dictionary<string, List<aCampagne>>();
        static Dictionary<string, List<aDisconnection>> dicosDisconnecion = new Dictionary<string, List<aDisconnection>>();
        static Dictionary<string, List<CsDetailMoratoire>> dicosDetailMoratoire = new Dictionary<string, List<CsDetailMoratoire>>();


        #region Ajustement 11-05-2017

        public List<CsLotComptClient> LoadAllAjustement()
        {
            List<CsLotComptClient> ListeAjustement = new List<CsLotComptClient>();
            try
            {
                ListeAjustement = new DBMoratoires().LoadAllAjustement();
                return ListeAjustement;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int SaveAjustement(List<CsLotComptClient> ListeAjustementToUpdate, List<CsLotComptClient> ListeAjustementToInserte, List<CsLotComptClient> ListeAjustementToDelete)
        {
            List<CsLotComptClient> ListeAjustementToUpdate_ = (List<CsLotComptClient>)ListeAjustementToUpdate;
            List<CsLotComptClient> ListeAjustementToInserte_ = (List<CsLotComptClient>)ListeAjustementToInserte;
            List<CsLotComptClient> ListeAjustementToDelete_ = (List<CsLotComptClient>)ListeAjustementToDelete;

            try
            {
                return new DBMoratoires().SaveAjustement(ListeAjustementToUpdate_, ListeAjustementToInserte_, ListeAjustementToDelete_);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //return true;
        }
        public List<CsLclient> MiseAjourComptAjustement(List<CsDetailLot> lstDetailPaiement, int Id)
        {
            try
            {
                return new DBMoratoires().MiseAjourComptAjustement(lstDetailPaiement, Id);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        #endregion


        #region RECOUVREMENT


        public bool SaveAffectationTourne(List<CsTournee> ListeDesTourneAAffecter)
        {
            try
            {
                return new DBMoratoires().SaveAffectationTourne( ListeDesTourneAAffecter);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }
     
        public aCampagne returnaCampagne()
        {
            return new aCampagne();
        }

        public aDisconnection returnaDisconnection()
        {
            return new aDisconnection();
        }
        public bool? InsertDetailMoratoire(List<CsLclient> ListeMoratoire,List<CsLclient> listeFacture)
        {
            try
            {
                DBMoratoires moratoire = new DBMoratoires();
                return moratoire.InsertionMoratoires(ListeMoratoire, listeFacture);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }
        public List<CsDetailCampagne > RechercherSuiviCampagne(List<CsCAMPAGNE> lesCampagnes)
        {
            try
            {


                List<CsDetailCampagne> lstPaiementCampagne = new List<CsDetailCampagne>();
                foreach (CsCAMPAGNE laCampage in lesCampagnes)
                    lstPaiementCampagne.AddRange(new DBClientAccess().PaiementCampagne(laCampage));

                List<CsDetailCampagne> lstCampgne = new DBClientAccess().RechercherSuiviCampagne(lesCampagnes);
                List<CsDetailCampagne> lstIndexSaisie = new DBClientAccess().IndexCampagneSaisie(lesCampagnes);
                List<CsDetailCampagne> lstFriasCampagne = new DBClientAccess().FraisCampagnePayer(lesCampagnes);

                foreach (CsDetailCampagne item in lstCampgne)
                {
                    CsDetailCampagne leIndexSaisie = lstIndexSaisie.FirstOrDefault(t => t.FK_IDCLIENT == item.FK_IDCLIENT);
                    item.INDEX = leIndexSaisie != null ? leIndexSaisie.INDEX : null;
                    item.DATECOUPURE = leIndexSaisie != null ? leIndexSaisie.DATECOUPURE : null;
                    item.OBSERVATION = leIndexSaisie != null ? leIndexSaisie.OBSERVATION : null;

                    List<CsDetailCampagne > lesPaiment = lstPaiementCampagne.Where (t => t.FK_IDCLIENT == item.FK_IDCLIENT).ToList();
                    item.NOMBREFACTUREREGLE  = (lesPaiment != null && lesPaiment.Count != 0) ? lesPaiment.Count  : 0;
                    item.MONTANTEREGLE  = lesPaiment != null && lesPaiment.Count != 0 ? lesPaiment.Sum(u => u.MONTANT.Value  ) : 0;

                    CsDetailCampagne  leFrais = lstFriasCampagne.FirstOrDefault(t => t.FK_IDCLIENT == item.FK_IDCLIENT);
                    item.MONTANTFRAIS = leFrais != null ? leFrais.MONTANTFRAIS : null;
                    item.DATEREGLEMENT = leFrais != null ? leFrais.DATEREGLEMENT : null;
                }
                return lstCampgne;
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public List<CsDetailCampagne> ControleCampagne(List<CsCAMPAGNE> lesCampagnes)
        {
            try
            {

                List<CsDetailCampagne> lstCampgneAutorise = new DBClientAccess().RetourneClientAutoriser(lesCampagnes);
                lstCampgneAutorise.AddRange(new DBClientAccess().FraisCampagnePayer(lesCampagnes));

                List<CsDetailCampagne> lstCampgneAvecIndexSaisie = new DBClientAccess().ListeDesClientIndexSaisi(lesCampagnes);
                List<CsDetailCampagne> lstIndexSaisie = new DBClientAccess().IndexCampagneSaisie(lesCampagnes);
                List<CsDetailCampagne> lstCampagne = new List<CsDetailCampagne>();
                foreach (CsDetailCampagne item in lstCampgneAvecIndexSaisie)
                {
                    if (lstCampgneAutorise.FirstOrDefault(t => t.FK_IDCLIENT == item.FK_IDCLIENT && t.IDCOUPURE == item.IDCOUPURE) != null)
                        continue ;
                    lstCampagne.Add(item);
                }
                return lstCampagne;
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public List<CsDetailCampagne> ClientAFactureCampagne(List<CsCAMPAGNE> lesCampagnes)
        {
            try
            {
                int nombreJour = 1;
                CsParametresGeneraux p = new DB_ParametresGeneraux().SelectParametresGenerauxByCode("000410");
                if (p != null)
                    nombreJour = int.Parse(p.LIBELLE);

                List<CsDetailCampagne> lstCampgneAutorise = new DBClientAccess().RetourneClientAutoriser(lesCampagnes);
                lstCampgneAutorise.AddRange(new DBClientAccess().FraisCampagnePayer(lesCampagnes));

                List<CsDetailCampagne> lstCampgneAvecIndexSaisie = new DBClientAccess().ListeDesClientIndexSaisi(lesCampagnes);
                List<CsDetailCampagne> lstIndexSaisie = new DBClientAccess().IndexCampagneSaisie(lesCampagnes);
                List<CsDetailCampagne> lstCampagne = new List<CsDetailCampagne>();
                foreach (CsDetailCampagne item in lstCampgneAvecIndexSaisie)
                {
                    if (lstCampgneAutorise.FirstOrDefault(t => t.FK_IDCLIENT == item.FK_IDCLIENT && t.IDCOUPURE == item.IDCOUPURE) != null)
                        continue;
                    int day = (System.DateTime.Today.Date - Convert.ToDateTime(item.DATECOUPURE)).Days;
                    if (day < nombreJour) 
                        continue;

                    lstCampagne.Add(item);
                }
                return lstCampagne;
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }


        public List<CsDetailCampagne> RetourneClientPourLettreRealnce(List<CsCAMPAGNE> lesCampagnes)
        {
            try
            {
                int nombreJour = 1;
                CsParametresGeneraux p = new DB_ParametresGeneraux().SelectParametresGenerauxByCode("000410");
                if (p != null)
                    nombreJour = int.Parse(p.LIBELLE);

                List<CsDetailCampagne> lstCampgneAutorise = new DBClientAccess().RetourneClientAutoriser(lesCampagnes);
                lstCampgneAutorise.AddRange(new DBClientAccess().FraisCampagnePayer(lesCampagnes));

                List<CsDetailCampagne> lstCampgneAvecIndexSaisie = new DBClientAccess().ListeDesClientIndexSaisi(lesCampagnes);
                List<CsDetailCampagne> lstIndexSaisie = new DBClientAccess().IndexCampagneSaisie(lesCampagnes);
                List<CsDetailCampagne> lstCampagne = new List<CsDetailCampagne>();
                foreach (CsDetailCampagne item in lstCampgneAvecIndexSaisie)
                {
                    if (lstCampgneAutorise.FirstOrDefault(t => t.FK_IDCLIENT == item.FK_IDCLIENT && t.IDCOUPURE == item.IDCOUPURE) != null)
                        continue;
                    int day = (System.DateTime.Today.Date - Convert.ToDateTime(item.DATECOUPURE)).Days;
                    if (day <= nombreJour)
                        continue;
                    if (item.RELANCE == 1 && day < (nombreJour * 2))
                        continue;
                    item.SOLDECLIENT =Math.Ceiling( new DBClientAccess().RetourneSoldeClient(item).Value);
                    lstCampagne.Add(item);
                }
                return lstCampagne;
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public bool UpdateDetailCampagneSuiteRelance(List<CsDetailCampagne > lesCampagnes)
        {
            try
            {
                return new DBClientAccess().UpdateDetailCampagneSuiteRelance(lesCampagnes);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false ;
            }
        }
        public List<CsDetailCampagne> ListeDesClientRelance(List<CsCAMPAGNE> lesCampagnes)
        {
            try
            {
               return  new DBClientAccess().ListeDesClientRelance(lesCampagnes);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public List<CsDetailCampagne> ListeDesClientAResilier(List<CsCAMPAGNE> lesCampagnes)
        {
            try
            {

                List<CsDetailCampagne> lstCampgneAutorise = new DBClientAccess().RetourneClientAutoriser(lesCampagnes);
                lstCampgneAutorise.AddRange(new DBClientAccess().FraisCampagnePayer(lesCampagnes));

                List<CsDetailCampagne> lstCampgneAvecIndexSaisie = new DBClientAccess().ListeDesClientIndexSaisi(lesCampagnes);
                List<CsDetailCampagne> lstIndexSaisie = new DBClientAccess().IndexCampagneSaisie(lesCampagnes);
                List<CsDetailCampagne> lstCampagne = new List<CsDetailCampagne>();
                foreach (CsDetailCampagne item in lstCampgneAvecIndexSaisie.Where(t=>t.RELANCE > 1).ToList())
                {
                    if (lstCampgneAutorise.FirstOrDefault(t => t.FK_IDCLIENT == item.FK_IDCLIENT && t.IDCOUPURE == item.IDCOUPURE) != null)
                        continue;

                    item.SOLDECLIENT = Math.Ceiling(new DBClientAccess().RetourneSoldeClient(item).Value);
                    lstCampagne.Add(item);
                }
                return lstCampagne;
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public List<CsDetailCampagne> RechercherClientCampagnePourRDV(CsCAMPAGNE Campagne, CsClient leClientRech)
        {
            try
            {
                return new DBMoratoires().RechercherClientCampagnePourRDV(Campagne, leClientRech);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public List<CsCAMPAGNE> RetourneDonneesReeditionAvisCoupure(List<int> lstCentre)
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

        public List<CsCAMPAGNE> RetourneDonneesSaisieIndexAvisCoupure(List<int> lstCentre)
        {
            try
            {
                return new DBMoratoires().RetourneDonneesSaisieIndexAvisCoupure(lstCentre);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public List<CsCAMPAGNE> RetourneCampagneDelaitleCoupure(List<int> lstCentre)
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

        public List<CsSite > RetourneSite()
        {
            try
            {
                return new List<CsSite>();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

   

        public List<CsDetailCampagne> RetourneDonneeIndexSaisie(CsCAMPAGNE Campagne, CsClient leClientRech)
        {
            try
            {
                return new DBMoratoires().RechercherCampagneIndexSaisi( Campagne, leClientRech);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public List<CsDetailCampagne> RetourneDonneeAnnulationFrais(CsCAMPAGNE  Campagne,CsClient leClientRech)
        {
            try
            {
                return new DBMoratoires().RetourneDonneeAnnulationFrais(Campagne, leClientRech);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public bool ValidationAutorisationFrais(List<CsDetailCampagne> ListClientCampagne)
        {
            try
            {
                return new DBMoratoires().ValidationAutorisationFrais(ListClientCampagne);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool  ValidationAnnulationFrais(List<CsDetailCampagne> ListClientCampagne)
        {
            try
            {
                return new DBMoratoires().ValidationAnnulationFrais(ListClientCampagne);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false ;
            }
        }

        public bool ValidationReposeCompteur(List<CsDetailCampagne> campagne)
        {
            try
            {
                return new DBMoratoires().ValidationReposeCompteur(campagne);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

       /*
        public bool InsererFraisPose(List<CsLclient> lesFacture)
        {
            try
            {
                DBAccueil db = new DBAccueil();
                DBMoratoires dbi = new DBMoratoires();
                foreach (CsLclient item in lesFacture)
                    item.NDOC = db.NumeroFacture();

                return dbi.InsererLclient(lesFacture);
               
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
       */
       
        public List<aCampagne> RechercherCampagneParCoupure(CsCAMPAGNE Campagne, CsClient leClientRech)
        {
            try
            {
                List<aCampagne> campagnes = new List<aCampagne>();
                campagnes.AddRange(new DBMoratoires().RechercherCampagneParCoupure(Campagne, leClientRech));
                return campagnes;
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public List<aCampagne> RechercherIndexParCampagne(string idCoupure)
        {
            try
            {
                return new DBMoratoires().RechercherIndexParCampagne(idCoupure);
                //return new DBClientAccess().RechercherIndexParCampagne(idCoupure);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public bool? InsertIndex(CsDetailCampagne campagne)
        {
            try
            {
                return new DBMoratoires().InsertIndex(campagne) ;
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }
        public bool? InsertIndexSGC(CsDetailCampagne campagne)
        {
            try
            {
                return new DBMoratoires().InsertIndexSGC(campagne);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }
        public bool? UpdateIndex(CsDetailCampagne campagne)
        {
            try
            {
                return new DBMoratoires().UpdateIndex(campagne);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        
       
/*       public bool? InsertListIndex(List<CsDetailCampagne> campagne)
        {
            try
            {
                return new DBMoratoires().InsertListIndex(campagne);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }
 * */



       public string InsertListIndex(List<CsDetailCampagne> campagne)
       {
           try
           {
               return new DBMoratoires().InsertListIndex(campagne);
           }
           catch (Exception ex)
           {
               ErrorManager.LogException(this, ex);
               return ex.Message;
           }
       }

        public List<CsUtilisateur> RetourneListeUtiliasteurPia(int IdCentre, string CodeFonction)
        {
            try
            {
                return new DBMoratoires().RetourneListeUtiliasteurPia(IdCentre, CodeFonction);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }
        public List<CsTournee> RetourneTourneePIA(List<int> lstIdCentre)
        {
            try
            {
                return new DBMoratoires().RetourneTourneePIA(lstIdCentre);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }
        public List<CsTournee> RetourneTourneeParPIA(string  CodeSite)
        {
            try
            {
                return new DBMoratoires().RetourneTourneeParPIA(CodeSite);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public List<CsUtilisateur> RetournePIAAgence(string CodeSite)
        {
            try
            {
                return new DBMoratoires().RetournePIAAgence(CodeSite);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public List<CsTournee> RetourneTourneeFromCampagne(List<CsCAMPAGNE > lstCampagne)
        {
            try
            {
                return new DBMoratoires().RetourneTourneeFromCampagne(lstCampagne);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }
        public List<CsAvisDeCoupureClient > TraitementAvisCoupure(CsAvisCoupureEdition AvisCoupure, aDisconnection dis,bool IsListe)
        {
            try
            {
                List<CsAvisDeCoupureClient> ligne = new List<CsAvisDeCoupureClient>();
                ligne = new DBMoratoires().ETAT_AVIS_DE_COUPURE(AvisCoupure, dis, IsListe);

                return ligne;
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }



        }
        public List<CsAvisDeCoupureClient> returnAvisReedtionCoupure(CsCAMPAGNE campagne, bool Isliste)
        {
            try
            {

                return new DBMoratoires().REEDITION_ETAT_AVIS_DE_COUPURE(campagne, Isliste);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }
        public List<CsLclient > TraitementAvisCoupureGC(CsAvisCoupureEdition AvisCoupure, aDisconnection dis, bool IsListe)
        {
            try
            {
                List<CsLclient> ligne = new List<CsLclient>();
                ligne = new DBMoratoires().ETAT_AVIS_DE_COUPUREGC(AvisCoupure, dis, IsListe);
                return ligne;
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }
        public List<CsDetailCampagnePrecontentieux> CREE_CAMPAGNE_PRECONTENTIEUX(int idcentre, DateTime? DateDebut, DateTime? DateFin, decimal SoldeDu, int Fk_idMatricule, string matricule)
        {
            try
            {
                List<CsDetailCampagnePrecontentieux> ligne = new List<CsDetailCampagnePrecontentieux>();
                ligne = new DBMoratoires().CREE_CAMPAGNE_PRECONTENTIEUX(idcentre,DateDebut,DateFin,SoldeDu,Fk_idMatricule,  matricule);
                return ligne;
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }
        public List<CsDetailCampagnePrecontentieux> REEDITION_CAMPAGNE_PRECONTENTIEUX(string IdCampagne)
        {
            try
            {
                List<CsDetailCampagnePrecontentieux> ligne = new List<CsDetailCampagnePrecontentieux>();
                ligne = new DBMoratoires().REEDITION_CAMPAGNE_PRECONTENTIEUX(IdCampagne);
                return ligne;
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public List<CsCentre> SelectCentreCampagne()
        {
            try
            {
                return new DBClientAccess().SelectCentreCampagne();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public List<CsDetailMoratoire> VerifiePaiementMoratoire(int IdMoratoire)
        {
            try
            {
                return  new DBMoratoires().VerifiePaiementMoratoire(IdMoratoire);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool? SuppressionMoratoireDuClient(int Idmoratoire)
        {
            try
            {
                DBMoratoires moratoire = new DBMoratoires();
                moratoire.SuppressionMoratoireDuClient(Idmoratoire);
                return true;
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public bool MiseAJourMoratoire(int Idmoratoire)
        {
            try
            {
                DBMoratoires moratoire = new DBMoratoires();
                moratoire.MiseAJourMoratoire(Idmoratoire);
                return true;
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }
        public List<CsDetailMoratoire> RetourneMoratoireDuClient(string centre, string client, string ordre)
        {
            try
            {
                return new DBMoratoires().RetourneMoratoireDuClient(centre, client, ordre);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }
        public List<CsDetailCampagne > ListeDesChequesImpayes(List<int> idcentre,DateTime? DateDebut,DateTime? DateFin, int Nbre)
        {
            try
            {
                return new DBMoratoires().ListeDesChequeImpayes(idcentre, DateDebut, DateFin, Nbre);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }

        }
        public bool SaveRDVCoupure(List<CsDetailCampagne> lstClientRDV)
        {
            try
            {
                DBMoratoires db = new DBMoratoires();

                return db.SaveRDVCoupure(lstClientRDV);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }

        }
        public bool SaveRDVCoupureHorsCampagne(CsClient leClient,DateTime DateRendezVous)
        {
            try
            {
                DBMoratoires db = new DBMoratoires();
                return db.SaveRDVCoupureHorsCampagne(leClient, DateRendezVous);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }

        }
        public CsCodeConsomateur retourneCodeConso()
        {
            return new CsCodeConsomateur();
        }
        public List<CsDetailCampagne> RetourneCampagneRendezVousCoupure(List<int> lstCentre, string idCampagne)
        {
            try
            {
                return new DBMoratoires().RetourneCampagneRendezVousCoupure(lstCentre, idCampagne);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }
        public List<CsDetailCampagne> RetourneCampagneReglementCoupure(List<int> lstCentre, string idCampagne)
        {
            try
            {
                return new DBMoratoires().RetourneCampagneReglementCoupure(lstCentre, idCampagne);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public List<CsClient> LoadClientCampgne(string Idcoupure)
        {
            try
            {
                return new DBClientAccess().LoadClientCampgne(Idcoupure);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CsDetailCampagne> EditionClientAReposer(List<CsCAMPAGNE> lstCampagne)
        {
            try
            {
                return new DBMoratoires().EditionClientAReposer(lstCampagne);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }
        //public List<CsDetailCampagne > EditerCLientaPoser(List<CsCAMPAGNE> lstCampagne)
        //{
        //    try
        //    {
        //        List<CsDetailCampagne> lstPaiementCampagne = new List<CsDetailCampagne>();
        //        foreach (CsCAMPAGNE laCampage in lstCampagne)
        //            lstPaiementCampagne.AddRange(new DBClientAccess().PaiementCampagne(laCampage));

        //        List<CsDetailCampagne> lstDetailCampagne = new DBClientAccess().RetourneDetailCampagne(lstCampagne);
        //        List<CsDetailCampagne> lstCampgneAutorise = new DBClientAccess().RetourneClientAutoriser(lstCampagne);
        //        List<CsDetailCampagne > lstIndexSaisie = new DBClientAccess().IndexCampagneSaisie(lstCampagne);
        //        List<CsDetailCampagne> lstFriasCampagne = new DBClientAccess().FraisCampagnePayer(lstCampagne);
        //        if (lstFriasCampagne != null && lstFriasCampagne.Count != 0)
        //        {
        //            foreach (var item in lstFriasCampagne)
        //            {
        //                CsDetailCampagne AAjouter = lstDetailCampagne.FirstOrDefault(t => t.FK_IDCLIENT == item.FK_IDCLIENT);
        //                lstCampgneAutorise.Add(AAjouter);
        //            }
        //        }
        //        //lstCampgneAutorise.AddRange( new DBClientAccess().FraisCampagnePayer(lstCampagne));
        //        foreach (CsDetailCampagne  item in lstCampgneAutorise)
        //        {
        //            CsDetailCampagne  leIndexSaisie = lstIndexSaisie.FirstOrDefault(t => t.FK_IDCLIENT == item.FK_IDCLIENT);
        //            item.INDEX = leIndexSaisie != null ? leIndexSaisie.INDEX : null;
        //            item.DATECOUPURE = leIndexSaisie != null ? leIndexSaisie.DATECOUPURE : null;


        //            List<CsDetailCampagne> lesPaiment = lstPaiementCampagne.Where(t => t.FK_IDCLIENT == item.FK_IDCLIENT).ToList();
        //            item.NOMBREFACTUREREGLE = (lesPaiment != null && lesPaiment.Count != 0) ? lesPaiment.Count : 0;
        //            item.MONTANTEREGLE = lesPaiment != null && lesPaiment.Count != 0 ? lesPaiment.Sum(u => u.MONTANT.Value) : 0;

        //            CsDetailCampagne leFrais = lstFriasCampagne.FirstOrDefault(t => t.FK_IDCLIENT == item.FK_IDCLIENT);
        //            item.MONTANTFRAIS = leFrais != null ? leFrais.MONTANTFRAIS : null;
        //            item.DATEREGLEMENT = leFrais != null ? leFrais.DATEREGLEMENT : null;
        //        }
        //        return lstCampgneAutorise;
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorManager.LogException(this, ex);
        //        return null;
        //    }
        //}

        public List<CsDetailCampagne> EditerCLientaPoser(DateTime? dateDebut,DateTime? Datefin)
        {
            try
            {
                List<CsDetailCampagne> lstPaiementCampagne = new List<CsDetailCampagne>();

                List<CsDetailCampagne> lstDetailPaiementCampagne = new DBMoratoires().RetourneDonneePaiementFraisCampagne(dateDebut, Datefin);
                List<CsDetailCampagne> lstDetailPaiementFraisCampagne = new DBMoratoires().RetourneDonneePaiementCampagne(dateDebut, Datefin);
                List<CsDetailCampagne>lstCampgneAutorise  = new DBMoratoires().RetourneDonneeClientAutorise(dateDebut, Datefin);

                lstPaiementCampagne.AddRange(lstCampgneAutorise);
                foreach (var item in lstDetailPaiementCampagne)
                {
                    List<CsDetailCampagne> lstFrais = lstDetailPaiementFraisCampagne.Where(t => t.FK_IDCLIENT == item.FK_IDCLIENT).ToList();
                    if (lstFrais != null)
                        item.MONTANTFRAIS = lstFrais.Sum(u => u.MONTANTFRAIS); 
                }
                lstPaiementCampagne.AddRange(lstDetailPaiementCampagne);
                return lstPaiementCampagne;
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }
        public List<CsDetailCampagne> RetourneDonneePaiementFraisCampagne(DateTime? dateDebut, DateTime? Datefin)
        {
            try
            {
                return new DBMoratoires().RetourneDonneePaiementFraisCampagne(dateDebut, Datefin);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public  CsLclient  VerifieFraisDejaSaisi(CsDetailCampagne leClient)
        {
            try
            {
                return new DBMoratoires().VerifieFraisDejaSaisi(leClient);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }
        public List<CsLclient> RetourneListeFactureNonSoldePourMoratoire(CsClient leClient)
        {
            try
            {
                DBEncaissement db = new DBEncaissement();
                List<CsLclient> lstFactureClient = new List<CsLclient>();
                List<CsClient> lstClientReference = TestClientExist(leClient.CENTRE, leClient.REFCLIENT, leClient.ORDRE);

                foreach (CsClient item in lstClientReference)
                {
                    List<CsLclient> lstFacture = db.RetourneListeFactureNonSolde(item).Where(t => t.COPER != Enumere.CoperMOR).ToList();
                    if (lstFacture != null)
                    {
                        lstFactureClient.AddRange(Galatee.Tools.Utility.RetourneListCopy<CsLclient>(lstFacture));
                        lstClientReference.ForEach(t => t.DRES = item.DRES);
                    }
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
                return lstFactureClient;
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }

        }

        public List<CsObservation > RetourneObservation()
        {
            try
            {
                return new DBClientAccess().RemplirObservation();
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

        public List<CsDetailCampagne> ClientAFacturer(List<CsCAMPAGNE> lesCampagnes)
        {
            try
            {

                List<CsDetailCampagne> lstCampgne = new DBClientAccess().RechercherSuiviCampagne(lesCampagnes);
                List<CsDetailCampagne> lstIndexSaisie = new DBClientAccess().IndexCampagneSaisie(lesCampagnes);
                List<CsDetailCampagne> lstFriasCampagne = new DBClientAccess().FraisCampagnePayer(lesCampagnes);

                foreach (CsDetailCampagne item in lstCampgne)
                {
                    CsDetailCampagne leIndexSaisie = lstIndexSaisie.FirstOrDefault(t => t.FK_IDCLIENT == item.FK_IDCLIENT);
                    item.INDEX = leIndexSaisie != null ? leIndexSaisie.INDEX : null;
                    item.DATECOUPURE = leIndexSaisie != null ? leIndexSaisie.DATECOUPURE : null;

                    CsDetailCampagne leFrais = lstFriasCampagne.FirstOrDefault(t => t.FK_IDCLIENT == item.FK_IDCLIENT);
                    item.MONTANTFRAIS = leFrais != null ? leFrais.MONTANTFRAIS : null;
                    item.DATEREGLEMENT = leFrais != null ? leFrais.DATEREGLEMENT : null;
                }
                return lstCampgne;
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public List<CsDetailCampagne> ListeDesClientEnRDV(string CodeSite, DateTime? DateDebut, DateTime? DateFin)
        {
            try
            {
                return new DBMoratoires().ListeDesClientEnRDV(CodeSite,DateDebut, DateFin);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CsDetailCampagne> ListeDesClientIndexSaisi(List<CsCAMPAGNE> lstCampagne)
        {
            try
            {
                return new DBClientAccess().ListeDesClientIndexSaisi(lstCampagne);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CsDetailCampagne> ListeDesClientFraisSaisi(List<CsCAMPAGNE> lstCampagne)
        {
            try
            {
                return new DBClientAccess().ListeDesClientFraisSaisi(lstCampagne);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CsDetailCampagne> ListeDesMauvaisPayer(List<int> lstIdCentre,DateTime? Datedebut ,DateTime? Datefin)
        {
            try
            {
                return new DBClientAccess().ListeDesMauvaisPayer(lstIdCentre, Datedebut, Datefin);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CsDetailCampagne> ListeDesMoratoiresNonRespecte(List<int> lstIdCentre, DateTime? Datedebut, DateTime? Datefin)
        {
            try
            {
                return new DBClientAccess().ListeDesMoratoiresNonRespecte(lstIdCentre, Datedebut, Datefin);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CsDetailMoratoire> ListeDesMoratoiresEmis(List<int> lstIdCentre, DateTime? Datedebut, DateTime? Datefin,bool IsPrecontentieux)
        {
            try
            {
                return new DBClientAccess().ListeDesMoratoiresEmis(lstIdCentre, Datedebut, Datefin, IsPrecontentieux);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        #endregion

        #region SHARED

        public List<CsLclient> RetourneListeFactureMoratoire(string centre, string Client, string ordre, int pkid)
        {
            try
            {
                List<CsLclient> lstFacturePourMoratoire = new List<CsLclient>();
                DBEncaissement db = new DBEncaissement();
                lstFacturePourMoratoire = db.RetourneListeFactureNonSolde(centre, Client, ordre, pkid);
                return lstFacturePourMoratoire.Where(t => t.SOLDEFACTURE  > 0).ToList();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }

        }
        public string RetourneNumFactureNaf(int Pkidcentre)
        {
            try
            {
                DBEncaissement db = new DBEncaissement();
                return db.RetourneNumFactureNaf(Pkidcentre);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }

        }

        public List<CsCentre> ListeDesDonneesDesSite()
        {
            try
            {

                DBAccueil db = new DBAccueil();
                return db.ChargerLesDonneesDesSite (false );
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }


        public List<CsLclient > RetourneClientsDuChecqhe(string numChq, string banque,string guichet)
        {
            try
            {
                return new DBMoratoires().RetourneReglementDuChecqhe(numChq, banque, guichet);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }
        public CsLclient VerifieChequeSaisie(string numChq, string banque)
        {
            try
            {
                return new DBMoratoires().VerifierChecqueImpayes(numChq, banque);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }
       
        public bool? InsertChecqueImpayes(List<CsLclient > LignecompteClient)
        {
            try
            {
                new DBMoratoires().InsertChecqueImpayes(LignecompteClient);
                // envoi de notification au client par SMS / EMAIL  prévu
                return true;
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }
      
        public List<CsBanque> SelectBanques()
        {
            try
            {
                return new DBEncaissement().RetourneListeDesBanques ();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public List<CsMotifChequeImpaye> RetourneMotifChequeImpaye()
        {
            return new DBMoratoires().RetourneMotifChequeImpaye();
        }
        public List<CsClient> TestClientExist(string centre, string Client, string ordre)
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

        public List<CsTournee>  RetourneTournee()
        {
            try
            {
                return new List<CsTournee>();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }

        }
        public List<CsCategorieClient> RetourneCategorie()
        {
            try
            {
                return new List<CsCategorieClient>();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }

        }
    
        public List<CsDetailCampagne> ListeDesMoratoiresRespecte(List<int> lstIdCentre, DateTime Datedebut, DateTime Datefin)
        {
            try
            {
                return new DBMoratoires().ListeDesMoratoiresRespecte(lstIdCentre, Datedebut, Datefin);

            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }

        }

        #endregion 
     
        #region SGC
        public List<CsRegCli> RetourneCodeRegroupement()
        {
            try
            {
                DBAccueil db = new DBAccueil();
                return db.RetourneCodeRegroupement();
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }
        public List<CsRegCli> RetourneCodeRegroupementByCampagne(int IdCampagne)
        {
            try
            {
                DBAccueil db = new DBAccueil();
                return db.RetourneCodeRegroupementByCampagne(IdCampagne);
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }
        public List<CsAffectationGestionnaire> RemplirAffectation()
        {
            try
            {
                return new DBMoratoires().RemplirAffectation();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }
        public bool? SaveAffection(List<CsRegCli> ListRegCliAffecter, int? ID_USER)
        {
            try
            {
                return new DBMoratoires().SaveAffection(ListRegCliAffecter, ID_USER);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public List<CsLclient> Remplirfacture(CsRegCli csRegCli, List<string> listperiode)
        {
            try
            {
                List<CsLclient> ListeFacture = new List<CsLclient>();
                List<CsCampagneGc> camp = new List<CsCampagneGc>();
                foreach (var item in listperiode)
                {
                    camp.Add(VerifierCampagneExiste(csRegCli, item));
                    if (camp != null && camp.Count() > 0 && camp[0] != null)
                    {
                        foreach (var item_ in new DBEncaissement().RetourneListeFactureNonSoldeByRegroupementspx(csRegCli.PK_ID, item))
                        {
                            if (!camp[0].DETAILCAMPAGNEGC_.Select(d => d.NDOC).Contains(item_.NDOC))
                            {
                                ListeFacture.Add(item_);
                            }
                        }
                    }
                    else
                    {
                        ListeFacture.AddRange(new DBEncaissement().RetourneListeFactureNonSoldeByRegroupementspx(csRegCli.PK_ID, item));
                    }
                }

                return ListeFacture;
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }
        public List<CsLclient> RetourneListeFactureNonSoldeGC(CsClient leClient)
        {
            try
            {
                DBMoratoires db = new DBMoratoires();
                List<CsLclient> lstFactureClient = new List<CsLclient>();
                List<CsClient> lstClientReference = TestClientExist(leClient.CENTRE, leClient.REFCLIENT, leClient.ORDRE);

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
        public List<CsLclient> RetourneListeFactureNonSolde(string centre, string client, string ordre, int foreignkey, string REFEM)
        {
            try
            {
                return new DBEncaissement().RetourneListeFactureNonSolde(centre, client, ordre, foreignkey, REFEM);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        //public List<CsLclient> RemplirfactureAvecProduit(CsRegCli csRegCli, List<string> listperiode,List<int> idProduit)
        //{
        //    try
        //    {
        //        List<CsLclient> ListeFacture = new List<CsLclient>();
        //        List<CsCampagneGc> camp = new List<CsCampagneGc>();
        //        foreach (var item in listperiode)
        //        {
        //            camp.Add(VerifierCampagneExiste(csRegCli, item));
        //            if (camp != null && camp.Count() > 0 && camp[0] != null)
        //            {
        //                foreach (int itemIdProduit in idProduit)
        //                {
        //                    foreach (var item_ in new DBEncaissement().RetourneListeFactureNonSoldeByRegroupementProduitspx(csRegCli.PK_ID, item, itemIdProduit))
        //                    {
        //                        //if (!camp[0].DETAILCAMPAGNEGC_.Select(d => d.NDOC).Contains(item_.NDOC))
        //                        var FactureCorrespondanteDansCampagneExistante = camp[0].DETAILCAMPAGNEGC_.FirstOrDefault(d => d.FK_IDCLIENT == item_.FK_IDCLIENT && d.NDOC == item_.NDOC && d.PERIODE == item_.REFEM);
        //                        if (FactureCorrespondanteDansCampagneExistante == null)
        //                        {
        //                            ListeFacture.Add(item_);
        //                        }
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                foreach (var itemIdProduit in idProduit)
        //                    ListeFacture.AddRange(new DBEncaissement().RetourneListeFactureNonSoldeByRegroupementProduitspx(csRegCli.PK_ID, item, itemIdProduit));

        //            }
        //        }

        //        return ListeFacture;
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorManager.LogException(this, ex);
        //        return null;
        //    }
        //}

       /* LKO 13/01/2021 */
        public List<CsLclient> RemplirfactureAvecProduit(CsRegCli csRegCli, List<string> listperiode, List<int> idProduit)
        {
            try
            {
                List<CsLclient> ListeFacture = new List<CsLclient>();
                List<CsCampagneGc> camp = new List<CsCampagneGc>();
                DBMoratoires DbM = new DBMoratoires();
                foreach (var item in listperiode)
                {
                        foreach (int itemIdProduit in idProduit)
                            ListeFacture.AddRange(DbM.RetourneListeFactureNonSoldeByRegroupementProduitspx(csRegCli.PK_ID, item, itemIdProduit));
                }
                return ListeFacture;
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }
       /**/


        public List<Dico> SaveCampane(List<CsLclient> ListFacturation, CsRegCli csRegCli, int? ID_USER)
        {
            try
            {
                ErrorManager.WriteInLogFile (this, "passage");
                return new DBMoratoires().SaveCampane(ListFacturation, csRegCli, ID_USER);

            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }
        public CsCampagneGc VerifierCampagneExiste(CsRegCli csRegCli, string periode)
        {
            try
            {
                return new DBMoratoires().VerifierCampagneExiste(csRegCli, periode);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }
        public List<CsCampagneGc> RemplirCampagne(string Matricule)
        {
            try
            {
                return new DBMoratoires().RemplirCampagne(Matricule);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }
        public CsCampagneGc RemplirCampagneById(int IdCampagne, string Statut)
        {
            try
            {
                return new DBMoratoires().RemplirCampagneById(IdCampagne, Statut);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }
        public bool? SaveMandatement(List<CsDetailCampagneGc> ListMandatementGc, bool IsAvisCerdit)
        {
            try
            {
                return new DBMoratoires().SaveMandatement(ListMandatementGc, IsAvisCerdit);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }
        public bool? SavePaiement(List<CsPaiementGc> ListMandatementGc, CsDetailMandatementGc Facture_Payer_Partiellement=null)
        {
            try
            {
                return new DBMoratoires().SavePaiement(ListMandatementGc, Facture_Payer_Partiellement);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }
        public List<CsCampagneGc> RetournCampagneByRegcli(CsRegCli csRegCli, string periode)
        {
            try
            {
                return new DBMoratoires().RetournCampagneByRegcli(csRegCli, periode);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }
        public bool? SaisiPaiement(decimal? Montant, int Id)
        {
            try
            {
                return new DBMoratoires().SaisiPaiement(Montant, Id);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }
        public List<CsLclient> MiseAjourCompt(List<CsDetailPaiementGc> lstDetailPaiement, int Id)
        {
            try
            {
                return new DBMoratoires().MiseAjourCompt(lstDetailPaiement, Id);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }
        public List<CsCampagneGc> RechercheCampane(string numerocamp)
        {
            try
            {
                return new DBMoratoires().RechercheCampane(numerocamp);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CsLclient > RechercheDetailCampane(string numerocamp)
        {
            try
            {
                return new DBMoratoires().RechercheDetailCampane(numerocamp);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CsLclient> RechercheMandatemant(string numeroMandatement)
        {
            try
            {
                return new DBMoratoires().RechercheMandatemant(numeroMandatement);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CsLclient> RechercheMiseAJour(string numeroAvisCredit)
        {
            try
            {
                return new DBMoratoires().RechercheMiseAJour(numeroAvisCredit);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CsLclient> RecherchePaiement(string numeroAvisCredit)
        {
            try
            {
                return new DBMoratoires().RecherchePaiement(numeroAvisCredit);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region Sylla 20/06/2016
        public List<string> RecupererPeriodeDePlage(string PeriodeDebut, string PeriodeFin)
        {
            try
            {
                return new DBMoratoires().RecupererPeriodeDePlage(PeriodeDebut, PeriodeFin);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        #endregion

        #endregion
        public List<CsCampagneGc> ChargerCampagne(int IdRegroupement)
        {
            try
            {
                return new DBMoratoires().ChargerCampagne(IdRegroupement);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CsDetailCampagne> ChargerClientPourSaisiIndex(int idCampagne)
        {
            try
            {
                return new DBMoratoires().ChargerClientPourSaisiIndex(idCampagne);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CsLclient> AutorisationDePaiement(string centre, string client, string ordre)
        {
            try
            {
                return new DBMoratoires().AutorisationDePaiement( centre,  client,  ordre);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool ValidateAutorisation(CsLclient leFacture)
        {
            try
            {
                return new DBMoratoires().ValidateAutorisation(leFacture);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        #region PNT

        public bool InsertCampagneBTA(CsCampagnesBTAAccessiblesParLUO CampBAT)
        {
            try
            {
                return new Galatee.DataAccess.Rpnt.DBCAMPAGNE().InsertCampagneBTA(CampBAT);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CsCampagnesBTAAccessiblesParLUO> GetCampagneBTAControle()
        {
            try
            {
                return new Galatee.DataAccess.Rpnt.DBCAMPAGNE().GetCampagneBTAControle();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        #region Methode De Detection BTA
        public List<Galatee.Structure.Rpnt.CsRefMethodesDeDetectionClientsBTA> GetMethodeDetectionBTA()
        {
            try
            {
                return new Galatee.DataAccess.Rpnt.DBCAMPAGNE().GetMethodeDetectionBTA();
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        #endregion

        public List<CsCategorieClient> GetTypeClient()
        {
            try
            {
                return new Galatee.DataAccess.Rpnt.DBCAMPAGNE().GetTypeClient();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<CsTypeTarif> GetTypeTarif()
        {
            try
            {
                return new Galatee.DataAccess.Rpnt.DBCAMPAGNE().GetTypeTarif();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<CsReleveur> GetAgentZont()
        {
            try
            {
                return new Galatee.DataAccess.Rpnt.DBCAMPAGNE().GetAgentZont();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<CsGroupeDeFacturation> GetGroupeFacture()
        {
            try
            {
                return new Galatee.DataAccess.Rpnt.DBCAMPAGNE().GetGroupeFacture();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<string> GetMois()
        {
            try
            {
                return new Galatee.DataAccess.Rpnt.DBCAMPAGNE().GetMois();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        public List<CsTcompteur> GetTypeCompteur()
        {
            try
            {
                return new Galatee.DataAccess.Rpnt.DBCAMPAGNE().GetTypeCompteur();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        public List<CsClient> GetClientEligible(string CodeMethode, int FkidCentre, string CodeTypeClient, string CodeTypeTarif, string CodeTypeCompteur, string CodeAgentZone, string CodeGroupe, string nombremois, string txt_comparaison_periode1, string txt_comparaison_periode2, string txt_Code_Cas, double Nombre_Recurence, double pourcentage)
        {
            try
            {
                return new Galatee.DataAccess.Rpnt.DBCAMPAGNE().GetClientEligible(CodeMethode, FkidCentre, CodeTypeClient, CodeTypeTarif, CodeTypeCompteur, CodeAgentZone, CodeGroupe, nombremois, txt_comparaison_periode1, txt_comparaison_periode2, txt_Code_Cas, Nombre_Recurence, pourcentage);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        public List<CsBrt> GetBranchementBTAControle(List<CsClient> ClientSelection)
        {
            try
            {
                return new Galatee.DataAccess.Rpnt.DBCAMPAGNE().GetBranchementBTAControle(ClientSelection);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public bool SaveCampagneElement(List<CsCampagnesBTAAccessiblesParLUO> ListeCampBAT)
        {
            try
            {
                return new Galatee.DataAccess.Rpnt.DBCAMPAGNE().SaveCampagneElement(ListeCampBAT);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public List<CsClient> GetClienteBTADuLotControle(Galatee.Structure.Rpnt.CstbLotsDeControleBTA Lot)
        {
            try
            {
                return new Galatee.DataAccess.Rpnt.DBCAMPAGNE().GetClienteBTADuLotControle(Lot);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool InitialisationFraud(List<Galatee.Structure.Rpnt.CstbElementsLotDeControleBTA> ListElementLot)
        {
            try
            {
                return new Galatee.DataAccess.Rpnt.DBCAMPAGNE().InitialisationFraud(ListElementLot);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool AnnulerCampagne(string numeroCampagne)
        {
            try
            {
                return new DBMoratoires().AnnulerCampagne(numeroCampagne);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        #endregion


        public List<CsDetailCampagne> RechercheClientCampagne(string CodeSite, string IdCampagne, int IdPia, DateTime? DateDebut, DateTime? DateFin, string Centre, string Client, string Ordre, int TypeEdition)
        {
            try
            {
                return new DBMoratoires().RechercheClientCampagne(CodeSite, IdCampagne, IdPia, DateDebut, DateFin, Centre, Client, Ordre, TypeEdition);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }
        public List<CsDetailCampagne> RechercheClientCampagneScgc(string CodeSite, string Centre, string Client, string Ordre, int TypeEdition)
        {
            try
            {
                return new DBMoratoires().RechercheClientCampagneScgc(CodeSite,Centre,Client,Ordre,TypeEdition);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public List<CsCAMPAGNE > RechercheCampagne(string CodeSite, string IdCampagne, int IdPia, DateTime? DateDebut, DateTime? DateFin, int TypeEdition)
        {
            try
            {
                return new DBMoratoires().RechercheCampagne(CodeSite,  IdCampagne, IdPia,  DateDebut, DateFin,  TypeEdition);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        #region Precontentieux
                public List<CsCAMPAGNE> RetourneCampagnePrecontentieux(List<int> lstCentre)
        {
            try
            {
                return new DBMoratoires().RetourneCampagnePrecontentieux(lstCentre);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }
        public List<CsDetailCampagnePrecontentieux > RetourneDetailPrecontentieux(int IdCampagne)
        {
            try
            {
                return new DBMoratoires().RetourneDetailPrecontentieux(IdCampagne);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }
        public bool MajDetailPrecontentieux(List<CsDetailCampagnePrecontentieux > lstIdDetailCamp)
        {
            try
            {
                return new DBMoratoires().MajDetailPrecontentieux(lstIdDetailCamp);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false ;
            }
        
        }
        public bool? UpdateDetailMoratoire(List<CsDetailMoratoire> lstIdDetailCamp)
        {
            try
            {
                return new DBMoratoires().UpdateDetailMoratoire(lstIdDetailCamp);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }

        }
        public List<CsDetailCampagnePrecontentieux> RechercherSuiviCampagnePrecontentieux(CsCAMPAGNE lesCampagnes)
        {
            try
            {


                List<CsDetailCampagnePrecontentieux> lstPaiementCampagne = new List<CsDetailCampagnePrecontentieux>();
                lstPaiementCampagne.AddRange(new DBClientAccess().PaiementCampagnePrecontantieux(lesCampagnes));
                return lstPaiementCampagne;
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }
        public List<CsDetailCampagnePrecontentieux> RechercherClientSolde(CsCAMPAGNE lesCampagnes)
        {
            try
            {
               return  new DBMoratoires().ClientSoldeSuitePrecontentieux(lesCampagnes.FK_IDCENTRE ,lesCampagnes.IDCOUPURE );
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }
        public List<CsDetailCampagnePrecontentieux> RechercherAbonnemtPrepayePrecontentieux(List<CsDetailCampagnePrecontentieux> lesClient)
        {
            try
            {


                List<CsDetailCampagnePrecontentieux> lstPaiementCampagne = new List<CsDetailCampagnePrecontentieux>();
                foreach (CsDetailCampagnePrecontentieux item in lesClient)
                    lstPaiementCampagne.AddRange(new DBClientAccess().RechercherAbonnemtPrepayePrecontentieux(item));
                return lstPaiementCampagne;
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }
        public bool InsererDechargePrecontentieux(CsPrecontentieuxDechargement Decharge)
        {
            try
            {
                return new DBMoratoires().InsererDechargePrecontentieux(Decharge);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }
        public List<CsClientRechercher> RechercheClientCompteur(string NumCompteur)
        {
            try
            {
                return new DBAccueil().RechercherClientCompteur(NumCompteur);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }
        public List<CsDetailCampagnePrecontentieux> RechercheAbonneLier(CsCAMPAGNE laCampagne)
        {
            try
            {
                return new DBMoratoires().RechercheAbonneLier(laCampagne);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }
        public CsDetailCampagnePrecontentieux RetourneClientByReferenceOrdre(int idCentre, string client, string Ordre)
        {
            try
            {
                return new DBMoratoires().RetourneClientByReferenceOrdrePrecontentieux(idCentre, client, Ordre);
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }


        public List<CsRegCli> RetourneRegroupementGestionnaires(int IdGestionnaire)
        {
            try
            {
                return new DBClientAccess().RetourneRegroupementGestionnaires(IdGestionnaire);
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }
        #endregion


        #region 05/06/2017
        public List<string> MiseAJourCategorie7(List<string> lines, string matricule)
        {
            try
            {
                string FullPath = lines.First();
                lines = new List<string>();
                lines = System.IO.File.ReadLines(FullPath).ToList();
                DBMoratoires db = new DBMoratoires();
                return db.MiseAJourCategorie7(lines, matricule);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }
        #endregion



        public string InsererFraisPose(List<CsLclient> lesFacture)
        {
            try
            {

                return new DBMoratoires().InsererLclient(lesFacture);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



    }
}
