using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Galatee.Structure;
using Galatee.DataAccess;

namespace WcfService.Scelle
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "IScelleService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select IScelleService.svc or IScelleService.svc.cs at the Solution Explorer and start debugging.
    public class IScelleService : IIScelleService
    {     
        
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
        public List<CsCouleurActivite> RetourneListeAllCouleurScelle()
        {

            try
            {
                DBScelle db = new DBScelle();
                return db.RetourneListAllCouleurScelle();
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
        public string ValidationReception(List<CsDetailAffectationScelle> ListeScelle, string MatriculAgent, int idetapeActuelle, string numdem)
        {
            try
            {
                DBScelle db = new DBScelle();
                return db.ValidationReception( MatriculAgent, idetapeActuelle, numdem);
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return string.Empty;
            }
        }
        #endregion

        #region Bobou

        #region Margasin Virtuelle
        public List<CsCompteurBta> SelectAllMargasinVirtuelle()
        {
            try
            {
                return new DBScelle().SelectAllMargasinVirtuelle();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public bool DeleteMargasinVirtuelle(CsCompteurBta sCompteur)
        {
            try
            {
                return new DBScelle().DeleteMagV(sCompteur);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public int UpdateMargasinVirtuelle(List<CsCompteurBta> sCompteur)
        {
            try
            {
                return new DBScelle().UpdateMargasinVirtuelle(sCompteur);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return 0;
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

        //public bool EditerListeCoperDemande(string key, Dictionary<string, string> pDictionary, List<CsLotMagasinGeneral> coperDeCollection)
        //{
        //    try
        //    {
        //        var listePrint = new List<CsPrint>();
        //        listePrint.AddRange(coperDeCollection);
        //        var printService = new Printings.PrintingsService();
        //        printService.setFromWebPart(listePrint, key, pDictionary);
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorManager.LogException(this, ex);
        //        return false;
        //    }
        //}

        //public bool ValiderListeSaisieSelonDonneesEnBase(Dictionary<string, string> ListeSaisie, int OrigineLotsDeLaListe)
        //{
        //    try
        //    {

        //        return new DBScelle().ValiderListeSaisieSelonDonnees(ListeSaisie, OrigineLotsDeLaListe);
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorManager.LogException(this, ex);
        //        return false;
        //    }
        //}

        #endregion


        #region LotMagasinGeneral

        public List<CsLotMagasinGeneral> SelectAllLotMagasinGeneral()
        {
            try
            {
                return new DBScelle().SelectAllLotMagasinGeneral();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public bool DeleteLotMagasinGeneral(CsLotMagasinGeneral cLotMagasinGeneral)
        {
            try
            {
                return new DBScelle().Delete(cLotMagasinGeneral);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool UpdateLotMagasinGeneral(List<CsLotMagasinGeneral> cLotMagasinGeneral)
        {
            try
            {
                return new DBScelle().Update(cLotMagasinGeneral);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool InsertLotMagasinGeneral(List<CsLotMagasinGeneral> cLotMagasinGeneral)
        {
            try
            {
                return new DBScelle().Insert(cLotMagasinGeneral);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        //public bool EditerListeCoperDemande(string key, Dictionary<string, string> pDictionary, List<CsLotMagasinGeneral> coperDeCollection)
        //{
        //    try
        //    {
        //        var listePrint = new List<CsPrint>();
        //        listePrint.AddRange(coperDeCollection);
        //        var printService = new Printings.PrintingsService();
        //        printService.setFromWebPart(listePrint, key, pDictionary);
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorManager.LogException(this, ex);
        //        return false;
        //    }
        //}

        public bool ValiderListeSaisieSelonDonneesEnBase(Dictionary<string, string> ListeSaisie, int OrigineLotsDeLaListe)
        {
            try
            {

                return new DBScelle().ValiderListeSaisieSelonDonnees(ListeSaisie, OrigineLotsDeLaListe);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        #endregion

        #region CompteurBta
        public bool VerifieCompteurExiste(CsCompteurBta leCpt)
        {
            try
            {
                return new DBScelle().VerifieCompteurExiste(leCpt);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false ;
            }
        }
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
                return false ;
            }
        }
        #endregion

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
        public List<CsCompteurBta> SelectAllCompteurNonAffecte()
        {
            try
            {
                return new DBScelle().SelectAllCompteurNonAffecte();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }
        public bool DeleteCompteur(CsCompteurBta sCompteur)
        {
            try
            {
                return new DBScelle().Delete(sCompteur);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }
        public int UpdateCompteurSuiteModif(List<CsCompteurBta> sCompteur)
        {
            try
            {
                return new DBScelle().UpdateCompteurscelleSuiteModif(sCompteur);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return 0;
            }
        }
        public bool VerifierEtatCompteur(CsCompteurBta sCompteur)
        {
            try
            {
                return new DBScelle().VerifierEtatCompteur(sCompteur);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false ;
            }
        }
        public int UpdateCompteur(List<CsCompteurBta> sCompteur)
        {
            try
            {
                return new DBScelle().UpdateCompteurscelle(sCompteur);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return 0;
            }
        }

        public string  InsertCompteur(List<CsCompteurBta> sCompteur)
        {
            try
            {



                //if (!VerifieCompteurExiste(sCompteur.FirstOrDefault()))
                    return new DBScelle().InsertCompteurscelle(sCompteur);
                //else
                //    return 1;
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return ex.Message ;
            }
        }

        //public bool EditerListeCoperDemande(string key, Dictionary<string, string> pDictionary, List<CsLotMagasinGeneral> coperDeCollection)
        //{
        //    try
        //    {
        //        var listePrint = new List<CsPrint>();
        //        listePrint.AddRange(coperDeCollection);
        //        var printService = new Printings.PrintingsService();
        //        printService.setFromWebPart(listePrint, key, pDictionary);
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorManager.LogException(this, ex);
        //        return false;
        //    }
        //}

        //public bool ValiderListeSaisieSelonDonneesEnBase(Dictionary<string, string> ListeSaisie, int OrigineLotsDeLaListe)
        //{
        //    try
        //    {

        //        return new DBScelle().ValiderListeSaisieSelonDonnees(ListeSaisie, OrigineLotsDeLaListe);
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorManager.LogException(this, ex);
        //        return false;
        //    }
        //}

        #endregion

        #region Fournisseurs
        public List<CsRefFournisseurs> RetourneListeFournisseurs()
        {
            try
            {
                DBScelle db = new DBScelle();
                return db.SelectAllRefFournisseurs();
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }
        #endregion

        #region OrigineScelle
        public List<CsRefOrigineScelle> RetourneListeOrigineScelle()
        {
            try
            {
                DBScelle db = new DBScelle();
                return db.SelectAllRefOrigineScelle();
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }

        public CsRefOrigineScelle RetourneListeOrigineScelleById(int RefOrigineScelleByid)
        {
            try
            {
                DBScelle db = new DBScelle();
                return db.RefOrigineScelleByid(RefOrigineScelleByid);
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }
    
        #endregion

        #region RefEat
        public List<CsRefEtatCompteur> RetourneEtatCompteur()
        {
            try
            {
                DBScelle db = new DBScelle();
                return db.SelectAllEtatCompteur();
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }

        #endregion

        #region vwRechercheCompteur

        public List<CsRechercheCompteur> RetourneListRechercheCompteur()
        {
            try
            {
                DBScelle db = new DBScelle();
                return db.SelectAllRechercheCompteur();
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }

        public CsRechercheCompteur RetourneRechercheCompteurById(string Numero_compteur)
        {
            try
            {
                DBScelle db = new DBScelle();
                return db.RechercheCompteurByid(Numero_compteur);
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }

        #endregion

        #region MARQUE_MODELE

        public List<CsMarque_Modele> RetourneListMarque_Modele()
        {
            try
            {
                DBScelle db = new DBScelle();
                return db.RetourneListeMarque_Modele();
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }

        //public List<CsMarque_Modele> RetourneRechercheCompteurById(int id_Marque_Modele)
        //{
        //    try
        //    {
        //        DBScelle db = new DBScelle();
        //        return db.RetourneListeMarque_ModeleByid(id_Marque_Modele);
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorManager.WriteInLogFile(this, ex.Message);
        //        return null;
        //    }
        //}

        #endregion

        #region Scelle REmises

        public List<CsRemiseScelleByAg> RetourneListScelle( int pk_id)
        {
            try
            {
                DBScelle db = new DBScelle();
                return db.RetourneListeRemiScellesAg(pk_id);
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }

        //public List<CsMarque_Modele> RetourneRechercheCompteurById(int id_Marque_Modele)
        //{
        //    try
        //    {
        //        DBScelle db = new DBScelle();
        //        return db.RetourneListeMarque_ModeleByid(id_Marque_Modele);
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorManager.WriteInLogFile(this, ex.Message);
        //        return null;
        //    }
        //}

        #endregion


        #region Remise de Scelle
        public List<CsRemiseScelles> SelectAllRemiseScelle()
        {
            try
            {
                return new DBScelle().SelectAllRemiseScelle();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public bool DeleteRemise(CsRemiseScelles sRemise)
        {
            try
            {
                return new DBScelle().Delete(sRemise);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool UpdateRemise(List<CsRemiseScelles> sRemise)
        {
            try
            {
                return new DBScelle().Update(sRemise);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
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

        //public bool EditerListeCoperDemande(string key, Dictionary<string, string> pDictionary, List<CsLotMagasinGeneral> coperDeCollection)
        //{
        //    try
        //    {
        //        var listePrint = new List<CsPrint>();
        //        listePrint.AddRange(coperDeCollection);
        //        var printService = new Printings.PrintingsService();
        //        printService.setFromWebPart(listePrint, key, pDictionary);
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorManager.LogException(this, ex);
        //        return false;
        //    }
        //}

        //public bool ValiderListeSaisieSelonDonneesEnBase(Dictionary<string, string> ListeSaisie, int OrigineLotsDeLaListe)
        //{
        //    try
        //    {

        //        return new DBScelle().ValiderListeSaisieSelonDonnees(ListeSaisie, OrigineLotsDeLaListe);
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorManager.LogException(this, ex);
        //        return false;
        //    }
        //}

        #endregion


    
        #region motifs scelle
        public List<CsMotifsScelle> SelectAllMotisScelle()
        {
            try
            {
                DBScelle db = new DBScelle();
                return db.SelectAllMotifs();
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }

        #endregion

        #region Scelles 
        
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


        public List<CsScelle> SelectAllScelles()
        {
            try
            {
                DBScelle db = new DBScelle();
                return db.RetourneScellesListe();
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }

        public bool UpdateScelleStatut(CsScelle sScelle)
        {
            try
            {
                return new DBScelle().Update(sScelle);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        #endregion

        #region  lot
        public List<CsTbLot> RetourneListelot()
        {
            try
            {
                DBScelle db = new DBScelle();
                return db.RetourneLotDeScelle();
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }

        #endregion

        #region utilisateur
        public List<CsUtilisateur> RetourneListeAllUser()
        {
            try
            {
                return new DBAdmUsers().GetAll();
            }
            catch (Exception zw)
            {
                ErrorManager.WriteInLogFile(this, zw.Message);
                return null;
            }
        }
        #endregion

        #region retour de Scelle 16/02/2016
        public List<CsRetourScelles> SelectAllRetourScelle()
        {
            try
            {
                return new DBScelle().SelectAllRetourScelle();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public bool DeleteRetours(CsRetourScelles sRetours)
        {
            try
            {
                return new DBScelle().Delete(sRetours);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool UpdateRetourse(List<CsRetourScelles> sRetours)
        {
            try
            {
                return new DBScelle().Update(sRetours);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public int InsertRetours(List<CsRetourScelles> sRetours)
        {
            try
            {

                return new DBScelle().Insert(sRetours);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                throw ex;
            }
        }

        //public bool EditerListeCoperDemande(string key, Dictionary<string, string> pDictionary, List<CsLotMagasinGeneral> coperDeCollection)
        //{
        //    try
        //    {
        //        var listePrint = new List<CsPrint>();
        //        listePrint.AddRange(coperDeCollection);
        //        var printService = new Printings.PrintingsService();
        //        printService.setFromWebPart(listePrint, key, pDictionary);
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorManager.LogException(this, ex);
        //        return false;
        //    }
        //}

        //public bool ValiderListeSaisieSelonDonneesEnBase(Dictionary<string, string> ListeSaisie, int OrigineLotsDeLaListe)
        //{
        //    try
        //    {

        //        return new DBScelle().ValiderListeSaisieSelonDonnees(ListeSaisie, OrigineLotsDeLaListe);
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorManager.LogException(this, ex);
        //        return false;
        //    }
        //}

        #endregion

        #region Ludovic 040416
        public List<CsRemiseScelleByAg> RetourneListScelleByStatus(int pk_id,int Status)
        {
            try
            {
                DBScelle db = new DBScelle();
                return db.RetourneListScelleByStatus(pk_id,Status);
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }
        public List<CsRemiseScelleByAg> RetourneListScelleByCentre(int idCentre)
        {
            try
            {
                DBScelle db = new DBScelle();
                return db.RetourneListScelleByCentre(idCentre);
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }
        #endregion


        #endregion

        #region status scelle
        public List<CsRefStatutsScelles> SelectAllStatutsScelles()
        {
            try
            {
                DBScelle db = new DBScelle();
                return db.SelectAllStatutScelle();
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }

        public List<CsRemiseScelleByAg> SCELLES_RETOURNE_Pour_ScellageCpt(int Pkid)
        {
            try
            {
                DBScelle db = new DBScelle();
                return db.SCELLES_RETOURNE_Pour_ScellageCpt(Pkid);
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }

        #endregion


        public List<CsCompteurBta > RetourneCompteurBtaByNumCptNumScelle(string NumeroCompteur,string NumeroScelle)
        {
            try
            {
                DBScelle db = new DBScelle();
                return db.RetourneCompteurBtaByNumCptNumScelle(NumeroCompteur, NumeroScelle);
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }
        public List<CsCompteurBta> RetourneCompteurAffecter(string CodeAgence)
        {
            try
            {
                
                DBScelle db = new DBScelle();
                return db.RetourneCompteurMagasinVirtuel(CodeAgence);
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }
    }

}
