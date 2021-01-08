using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Galatee.Structure;
using Galatee.DataAccess;
using System.Data;

namespace WcfService
{
    // REMARQUE : vous pouvez utiliser la commande Renommer du menu Refactoriser pour changer le nom d'interface "IParametrage" à la fois dans le code et le fichier de configuration.
    [ServiceContract]
    public interface IParametrage
    {
        # region PARAMETRAGE

        [OperationContract]
        [FaultContract(typeof(Errors))]
        CsLibelle returnLibelle();

        //[OperationContract]
        //[FaultContract(typeof(Errors))]
        //List<CsCasind> SelectAllCasInd();

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsTdemCout> SelectAllTypeDemCout();

    

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsProduit> SelectAllProducts();

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsLibelle> SelectCoperLibelle100();

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsRegrou> SelectAll_REGROU();

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsPuissance> SelectAll_PUISSANCE();

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsCtax> SelectAll_CTAX();

        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool Delete_PUISSANC(string Centre, string Produit, string Commune, string CodeTarif, Decimal Puissance, DateTime DebutApplication);


        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool Delete_REGROU(string Centre, string CodeRegroupement, string CodeProduit);



        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool insertOrUpdateREGROU(List<CsRegrou> rows, List<CsRegrou> row);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsTdemCout> insertOrUpdateTypeDemandeCout(List<CsTdemCout> data, List<CsTdemCout> maj);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool DeleteINIT(string Centre, string Produit, string Ntable, string Zone);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool DeleteDemCouT(string Centre, string produit, string tdem);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool insertOrUpdateInit(List<CsInit> data1, List<CsInit> update1);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool insertOrUpdatePUISSANC(List<CsPuissance> rowInsert, List<CsPuissance> rowUpdate);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        EnumProcedureStockee returnEnumProcedureStockee();

        #region Table PRODUIT
        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsProduit> SelectAllProduit();
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool DeleteProduit(CsProduit pProduit);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool UpdateProduit(List<CsProduit> pProduitCollection);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool InsertProduit(List<CsProduit> pProduitCollection);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool EditerListeProduit(string key, Dictionary<string, string> pDictionary);

        #endregion

        #region Table DENOMINATION
        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsDenomination> SelectAllDenomination();
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool DeleteDenomination(CsDenomination pDenomination);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool UpdateDenomination(List<CsDenomination> pDenominationCollection);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool InsertDenomination(List<CsDenomination> pDenominationCollection);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool EditerListeDenomination(string key, Dictionary<string, string> pDictionary);

        #endregion

        #region Table CATEGORIEBRANCHEMENT
        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsTypeBranchement> SelectAllCategorieBranchement();
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool DeleteCategorieBranchement(CsTypeBranchement pCategorieBranchement);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool UpdateCategorieBranchement(List<CsTypeBranchement> pCategorieBranchementCollection);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool InsertCategorieBranchement(List<CsTypeBranchement> pCategorieBranchementCollection);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool EditerListeCategorieBranchement(string key, Dictionary<string, string> pDictionary);

        #endregion

        #region Table CATEGORIECLIENT
        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsCategorieClient> SelectAllCategorieClient();
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool DeleteCategorieClient(CsCategorieClient pCategorieClient);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool UpdateCategorieClient(List<CsCategorieClient> pCategorieClientCollection);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool InsertCategorieClient(List<CsCategorieClient> pCategorieClientCollection);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool EditerListeCategorieClient(string key, Dictionary<string, string> pDictionary);

        #endregion

        #region Table CONSOMMATEUR
        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsConsommateur> SelectAllConsommateur();
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool DeleteConsommateur(CsConsommateur pConsommateur);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool UpdateConsommateur(List<CsConsommateur> pConsommateurCollection);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool InsertConsommateur(List<CsConsommateur> pConsommateurCollection);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool EditerListeConsommateur(string key, Dictionary<string, string> pDictionary);

        #endregion

        #region Table PARAMETRESGENERAUX
        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsParametresGeneraux> SelectAllParametresGeneraux();
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool DeleteParametresGeneraux(CsParametresGeneraux pParametresGeneraux);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool UpdateParametresGeneraux(List<CsParametresGeneraux> pParametresGenerauxCollection);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool InsertParametresGeneraux(List<CsParametresGeneraux> pParametresGenerauxCollection);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool EditerListeParametresGeneraux(string key, Dictionary<string, string> pDictionary);

        #endregion

        #region Table PARAMETREEVENEMENT
        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsParametreEvenement> SelectAllParametreEvenement();
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool DeleteParametreEvenement(CsParametreEvenement pParametreEvenement);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool UpdateParametreEvenement(List<CsParametreEvenement> pParametreEvenementCollection);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool InsertParametreEvenement(List<CsParametreEvenement> pParametreEvenementCollection);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool EditerListeParametreEvenement(string key, Dictionary<string, string> pDictionary);

        #endregion

        #region Table PROPRIETAIRE
        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsProprietaire> SelectAllProprietaire();
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool DeleteProprietaire(CsProprietaire pProprietaire);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool UpdateProprietaire(List<CsProprietaire> pProprietaireCollection);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool InsertProprietaire(List<CsProprietaire> pProprietaireCollection);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool EditerListeProprietaire(string key, Dictionary<string, string> pDictionary);

        #endregion

        #region Table NATIONALITE
        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsNationalite> SelectAllNationalite();
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool DeleteNationalite(CsNationalite pNationalite);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool UpdateNationalite(List<CsNationalite> pNationaliteCollection);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool InsertNationalite(List<CsNationalite> pNationaliteCollection);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool EditerListeNationalite(string key, Dictionary<string, string> pDictionary);

        #endregion

        #region Table CELLULEDUSIEGE
        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsCelluleDuSiege> SelectAllCelluleDuSiege();
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool DeleteCelluleDuSiege(CsCelluleDuSiege pCelluleDuSiege);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool UpdateCelluleDuSiege(List<CsCelluleDuSiege> pCelluleDuSiegeCollection);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool InsertCelluleDuSiege(List<CsCelluleDuSiege> pCelluleDuSiegeCollection);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool EditerListeCelluleDuSiege(string key, Dictionary<string, string> pDictionary);

        #endregion

        #region Table CODEDEPART
        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsCodeDepart> SelectAllCodeDepart();
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool DeleteCodeDepart(CsCodeDepart pCodeDepart);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool UpdateCodeDepart(List<CsCodeDepart> pCodeDepartCollection);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool InsertCodeDepart(List<CsCodeDepart> pCodeDepartCollection);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool EditerListeCodeDepart(string key, Dictionary<string, string> pDictionary);

        #endregion

        #region Table POSTETRANSFORMATION
        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsPosteTransformation> SelectAllPosteTransformation();
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool DeletePosteTransformation(CsPosteTransformation pCodePoste);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool UpdatePosteTransformation(List<CsPosteTransformation> pCodePosteCollection);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool InsertPosteTransformation(List<CsPosteTransformation> pCodePosteCollection);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool EditerListePosteTransformation(string key, Dictionary<string, string> pDictionary);

        #endregion

        #region Table POSTESOURCE
        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsPosteSource> SelectAllPosteSource();
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool DeletePosteSource(CsPosteSource pPosteSource);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool UpdatePosteSource(List<CsPosteSource> pPosteSourceCollection);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool InsertPosteSource(List<CsPosteSource> pPosteSourceCollection);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool EditerListePosteSource(string key, Dictionary<string, string> pDictionary);

        #endregion

        #region Table LIBELLETOP
        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsLibelleTop> SelectAllLibelleTop();
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool DeleteLibelleTop(CsLibelleTop pLibelleTop);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool UpdateLibelleTop(List<CsLibelleTop> pLibelleTopCollection);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool InsertLibelleTop(List<CsLibelleTop> pLibelleTopCollection);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool EditerListeLibelleTop(string key, Dictionary<string, string> pDictionary);

        #endregion

        #region Table NUMEROINSTALLATION
        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsNumeroInstallation> SelectAllNumeroInstallation();
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool DeleteNumeroInstallation(CsNumeroInstallation pNumeroInstallation);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool UpdateNumeroInstallation(List<CsNumeroInstallation> pNumeroInstallationCollection);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool InsertNumeroInstallation(List<CsNumeroInstallation> pNumeroInstallationCollection);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool EditerListeNumeroInstallation(string key, Dictionary<string, string> pDictionary);

        #endregion

        #region Table BANQUE
        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<aBanque> SelectAllBanque();
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool DeleteBanque(aBanque pBanque);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool UpdateBanque(List<aBanque> pBanqueCollection);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool InsertBanque(List<aBanque> pBanqueCollection);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool EditerListeBanque(string key, Dictionary<string, string> pDictionary);

        #endregion

        #region Table DOMBANC
        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsDomBanc> SelectAllDomBanc();
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool DeleteDomBanc(CsDomBanc pDomBanc);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool UpdateDomBanc(List<CsDomBanc> pDomBancCollection);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool InsertDomBanc(List<CsDomBanc> pDomBancCollection);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool EditerListeDomBanc(string key, Dictionary<string, string> pDictionary);

        #endregion

        #region Table PARAMABONUTILISE
        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsParamAbonUtilise> SelectAllParamAbonUtilise();
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool DeleteParamAbonUtlise(CsParamAbonUtilise pParamAbonUtilise);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool UpdateParamAbonUtlise(List<CsParamAbonUtilise> pParamAbonUtiliseCollection);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool InsertParamAbonUtlise(List<CsParamAbonUtilise> pParamAbonUtiliseCollection);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool EditerListeParamAbonUtlise(string key, Dictionary<string, string> pDictionary);

        #endregion

        #region Table REGCLI
        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsRegCli> SelectAllRegCli();
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool DeleteRegCli(CsRegCli pRegCli);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool UpdateRegCli(List<CsRegCli> pRegCliCollection);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool InsertRegCli(List<CsRegCli> pRegCliCollection);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool EditerListeRegCli(string key, Dictionary<string, string> pDictionary);

        #endregion

        #region Table REGEXO
        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsRegExo> SelectAllRegExo();
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool DeleteRegExo(CsRegExo pRegExo);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool UpdateRegExo(List<CsRegExo> pRegExoCollection);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool InsertRegExo(List<CsRegExo> pRegExoCollection);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool EditerListeRegExo(string key, Dictionary<string, string> pDictionary);

        #endregion

        #region Table CENTRE

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsCentre> SelectAllCentre();
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool DeleteCentre(CsCentre pCentre);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool UpdateCentre(List<CsCentre> pCentreCollection);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool InsertCentre(List<CsCentre> pCentreCollection);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool EditerListeCentre(string key, Dictionary<string, string> pDictionary);
        List<CsCentre> SelectCentreByCodeSite(int pCodeSite);

        #endregion

        #region Table TYPETAXE

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsTypeTaxe> GetAllTypeTaxe();


        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool DeleteTypetax(List<CsTypeTaxe> entityCollection);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool InsertTypeTaxe(CsTypeTaxe entityCollection);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool UpdateTypeTaxe(CsTypeTaxe entityCollection);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool InsertListeTypeTaxe(List<CsTypeTaxe> entityCollection);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool UpdateLitsTypeTaxe(List<CsTypeTaxe> entityCollection);


        #endregion
        #region Table TYPECOMPTAGE

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsTypeComptage> GetAllTypeComptage();


        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool DeleteTypeComptage(List<CsTypeComptage> entityCollection);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool InsertTypeComptage(CsTypeComptage entityCollection);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool UpdateTypeComptage(CsTypeComptage entityCollection);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool InsertListeTypeComptage(List<CsTypeComptage> entityCollection);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool UpdateLitsTypeComptage(List<CsTypeComptage> entityCollection);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool EditerListeComptage(string key, Dictionary<string, string> pDictionary);

        #endregion

        #region Table TYPECENTRE
        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsTypeCentre> SelectAllTypeCentre();
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool DeleteTypeCentre(CsTypeCentre pTypeCentre);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool UpdateTypeCentre(List<CsTypeCentre> pTypeCentreCollection);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool InsertTypeCentre(List<CsTypeCentre> pTypeCentreCollection);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool EditerListeTypeCentre(string key, Dictionary<string, string> pDictionary);

        #endregion

        #region Table SITE
        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsSite> SelectAllSites();
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool DeleteSite(CsSite pSite);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool UpdateSite(List<CsSite> pSiteCollection);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool InsertSite(List<CsSite> pSiteCollection);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool EditerListeSite(string key, Dictionary<string, string> pDictionary);

        #endregion

        #region Table TYPEDEVIS

        [OperationContract]
        ObjTYPEDEVIS GetTypeDevisById(int id);
        [OperationContract]
        List<ObjTYPEDEVIS> GetTypeDevisByIdProduit(int IdProduit);
        [OperationContract]
        List<ObjTYPEDEVIS> GetAllTypeDevis();

        [OperationContract]
        bool EditerTypeDevis(string key, Dictionary<string, string> pDictionary);

        #endregion

        #region Table TACHEDEVIS

        [OperationContract]
        ObjTACHEDEVIS GetTacheDevisById(int id);
        [OperationContract]
        List<ObjTACHEDEVIS> GetAllTacheDevis();
   


        [OperationContract]
        bool EditerTacheDevis(string key, Dictionary<string, string> pDictionary);

        #endregion

        #region Table PIECEIDENTITE

        [OperationContract]
        ObjPIECEIDENTITE GetPieceIdentiteById(byte id);
        [OperationContract]
        List<ObjPIECEIDENTITE> GetAllPieceIdentite();
        [OperationContract]
        bool Delete(List<ObjPIECEIDENTITE> pPieceIdentitesCollection);
        [OperationContract]
        bool Update(List<ObjPIECEIDENTITE> pPieceIdentitesCollection);
        [OperationContract]
        bool Insert(List<ObjPIECEIDENTITE> pPieceIdentiteCollection);
        [OperationContract]
        bool EditerPieceIdentite(string key, Dictionary<string, string> pDictionary);

        #endregion

        #region Table ETAPEDEVIS

        [OperationContract]
        ObjETAPEDEVIS GetEtapeDevisById(int id);
        [OperationContract]
        List<ObjETAPEDEVIS> GetEtapeDevisByCodeProduit(int codeProduit);
        //[OperationContract]
        //List<ObjETAPEDEVIS> GetEtapeDevisByCodeFonction(System.String codeFonction);
        [OperationContract]
        List<ObjETAPEDEVIS> GetEtapeDevisByIdTypeDevis(int idTypeDevis);
        [OperationContract]
        List<ObjETAPEDEVIS> GetEtapeDevisByIdTacheDevis(int idTacheDevis);
        [OperationContract]
        List<ObjETAPEDEVIS> GetEtapeDevisByIdTacheSuivante(int idTacheSuivante);
        [OperationContract]
        List<ObjETAPEDEVIS> GetEtapeDevisByIdTacheRejet(int idTacheRejet);
        [OperationContract]
        List<ObjETAPEDEVIS> GetEtapeDevisByIdTacheSaut(int idTacheSaut);
        [OperationContract]
        List<ObjETAPEDEVIS> GetEtapeDevisByIdTacheIntermediaire(int idTacheIntermediaire);
        [OperationContract]
        ObjETAPEDEVIS GetEtapeDevisByIdTypeDevisCodeProduitIdTache(int idTypeDevis, int IdProduit, int idTacheCourante);
        [OperationContract]
        ObjETAPEDEVIS GetEtapeDevisByIdTypeDevisNumEtape(int idTypeDevis, int numEtape);


        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<ObjETAPEDEVIS> GetEtapeDevisForDisplay();
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool EditerListeEtapeDevis(string key, Dictionary<string, string> pDictionary);
        #endregion

        #region Table FONCTION
        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsFonction> SelectAllFonction();
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool DeleteFonction(CsFonction pFonction);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool UpdateFonction(List<CsFonction> pFonctionCollection);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool InsertFonction(List<CsFonction> pFonctionCollection);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool EditerListeFonction(string key, Dictionary<string, string> pDictionary);

        #endregion

        #region Table COMMUNE

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsCommune> SelectAllCommune();
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool DeleteCommune(CsCommune pCommune);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool UpdateCommune(List<CsCommune> pCommuneCollection);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool InsertCommune(List<CsCommune> pCommuneCollection);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool EditerListeCommune(string key, Dictionary<string, string> pDictionary);

        #endregion

        #region Table QUARTIER

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsQuartier> SelectAllQuartier();
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool DeleteQuartier(CsQuartier pQuartier);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool UpdateQuartier(List<CsQuartier> pQuartierCollection);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool InsertQuartier(List<CsQuartier> pQuartierCollection);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool EditerListeQuartier(string key, Dictionary<string, string> pDictionary);

        #endregion

        #region Table RUES

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsRues> SelectAllRues();
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool DeleteRues(CsRues pRues);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool UpdateRues(List<CsRues> pRuesCollection);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool InsertRues(List<CsRues> pRuesCollection);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool EditerListeRues(string key, Dictionary<string, string> pDictionary);

        #endregion

        #region Table REGLAGECOMPTEUR
      
        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsReglageCompteur > SelectAllReglageCompteur();
        

        #endregion

        #region Table CTAX

        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool DeleteCtax(List<CsCtax> entityCollection);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsCtax> GetAllCtax();
        [OperationContract]
        [FaultContract(typeof(Errors))]
        CsCtax GetCtaxByCENTRECTAXDEBUTAPPLICATION(CsCtax entity);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool InsertCtax(List<CsCtax> entityCollection);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool UpdateCtax(List<CsCtax> entityCollection);

        #endregion

        #region Table MARQUECOMPTEUR
        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsMarqueCompteur> SelectAllMarqueCompteur();
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool DeleteMarqueCompteur(CsMarqueCompteur pMarqueCompteur);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool UpdateMarqueCompteur(List<CsMarqueCompteur> pMarqueCompteurCollection);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool InsertMarqueCompteur(List<CsMarqueCompteur> pMarqueCompteurCollection);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool EditerListeMarqueCompteur(string key, Dictionary<string, string> pDictionary);

        #endregion

        #region Table TCOMPT
        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsTcompteur> SelectAllTcompt();
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool DeleteTcompt(CsTcompteur pTcompt);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool UpdateTcompt(List<CsTcompteur> pTcomptCollection);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool InsertTcompt(List<CsTcompteur> pTcomptCollection);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool EditerListeTcompt(string key, Dictionary<string, string> pDictionary);

        #endregion

        #region Table AdmMenus
        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CSMenuGalatee> GetAdmMenus();
        #endregion

        #region Table CASIND

        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool DeleteCasind(List<CsCasind> entityCollection);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsCasind> GetAllCasind();
        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsCasind> GetCasindByCentreCas(CsCasind entity);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool InsertCasind(List<CsCasind> entityCollection);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool UpdateCasind(List<CsCasind> entityCollection);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsCasind> GetCasindEcrasableByCas();
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool EditerListeCasind(string key, Dictionary<string, string> pDictionary);
        #endregion

        #region ENTREPRISE

        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool DeleteEntreprise(CsEntreprise entity);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        CsEntreprise GetAllEntreprise();

        [OperationContract]
        [FaultContract(typeof(Errors))]
        CsEntreprise GetEntrepriseById(CsEntreprise entity);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool InsertEntreprise(CsEntreprise entity);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool UpdateEntreprise(CsEntreprise entity);

        #endregion

        #region PERIODICITEFACTURATION

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsPeriodiciteFacturation> SelectAllPeriodiciteFacturation();
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool DeletePeriodiciteFacturation(CsPeriodiciteFacturation pPeriodiciteFacturation);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool UpdatePeriodiciteFacturation(List<CsPeriodiciteFacturation> pPeriodiciteFacturationCollection);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool InsertPeriodiciteFacturation(List<CsPeriodiciteFacturation> pPeriodiciteFacturationCollection);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool EditerListePeriodiciteFacturation(string key, Dictionary<string, string> pDictionary);

        #endregion

        #region RECHERCHETARIF

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsRechercheTarif> SelectAllRechercheTarif();
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool DeleteRechercheTarif(CsRechercheTarif pPeriodeFacturation);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool UpdateRechercheTarif(List<CsRechercheTarif> pPeriodeFacturationCollection);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool InsertRechercheTarif(List<CsRechercheTarif> pPeriodeFacturationCollection);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool EditerListeRechercheTarif(string key, Dictionary<string, string> pDictionary);

        #endregion

        #region MODECALCUL

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsModeCalcul> SelectAllModeCalcul();
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool DeleteModeCalcul(CsModeCalcul pModeCalcul);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool UpdateModeCalcul(List<CsModeCalcul> pModeCalculCollection);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool InsertModeCalcul(List<CsModeCalcul> pModeCalculCollection);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool EditerListeModeCalcul(string key, Dictionary<string, string> pDictionary);

        #endregion

        #region NIVEAUTARIF

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsNiveauTarif> SelectAllNiveauTarif();
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool DeleteNiveauTarif(CsNiveauTarif pNiveauTarif);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool UpdateNiveauTarif(List<CsNiveauTarif> pNiveauTarifCollection);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool InsertNiveauTarif(List<CsNiveauTarif> pNiveauTarifCollection);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool EditerListeNiveauTarif(string key, Dictionary<string, string> pDictionary);

        #endregion

        #region UNITECOMPTAGE

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsUniteComptage> SelectAllUniteComptage();
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool DeleteUniteComptage(CsUniteComptage pUniteComptage);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool UpdateUniteComptage(List<CsUniteComptage> pUniteComptageCollection);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool InsertUniteComptage(List<CsUniteComptage> pUniteComptageCollection);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool EditerListeUniteComptage(string key, Dictionary<string, string> pDictionary);

        #endregion

        #region MOIS

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsMois> SelectAllMois();
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool DeleteMois(CsMois pMois);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool UpdateMois(List<CsMois> pMoisCollection);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool InsertMois(List<CsMois> pMoisCollection);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool EditerListeMois(string key, Dictionary<string, string> pDictionary);

        #endregion

        #region ETATCOMPTEUR

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsEtatCompteur> SelectAllEtatCompteur();
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool DeleteEtatCompteur(CsEtatCompteur pEtatCompteur);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool UpdateEtatCompteur(List<CsEtatCompteur> pEtatCompteurCollection);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool InsertEtatCompteur(List<CsEtatCompteur> pEtatCompteurCollection);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool EditerListeEtatCompteur(string key, Dictionary<string, string> pDictionary);

        #endregion

        #region APPLICATIONTAXE

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsApplicationTaxe> SelectAllApplicationTaxe();
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool DeleteApplicationTaxe(CsApplicationTaxe pApplicationTaxe);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool UpdateApplicationTaxe(List<CsApplicationTaxe> pApplicationTaxeCollection);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool InsertApplicationTaxe(List<CsApplicationTaxe> pApplicationTaxeCollection);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool EditerListeApplicationTaxe(string key, Dictionary<string, string> pDictionary);

        #endregion

        #region TYPEMESSAGE

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsTypeMessage> SelectAllTypeMessage();
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool DeleteTypeMessage(CsTypeMessage pTypeMessage);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool UpdateTypeMessage(List<CsTypeMessage> pTypeMessageCollection);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool InsertTypeMessage(List<CsTypeMessage> pTypeMessageCollection);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool EditerListeTypeMessage(string key, Dictionary<string, string> pDictionary);

        #endregion

        #region TARIF

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsTarif> SelectAllTarif();
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool DeleteTarif(CsTarif pTarif);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool UpdateTarif(List<CsTarif> pTarifCollection);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool InsertTarif(List<CsTarif> pTarifCollection);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool EditerListeTarif(string key, Dictionary<string, string> pDictionary, List<CsTarif> pTarifCollection);

        #endregion

        #region FORFAIT

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsForfait> SelectAllForfait();
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool DeleteForfait(CsForfait pForfait);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool UpdateForfait(List<CsForfait> pForfaitCollection);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool InsertForfait(List<CsForfait> pForfaitCollection);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool EditerListeForfait(string key, Dictionary<string, string> pDictionary, List<CsForfait> pForfaitCollection);

        #endregion

        #region AJUFIN

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsAjufin> SelectAllAjufin();
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool DeleteAjufin(CsAjufin pAjufin);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool UpdateAjufin(List<CsAjufin> pAjufinCollection);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool InsertAjufin(List<CsAjufin> pAjufinCollection);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool EditerListeAjufin(string key, Dictionary<string, string> pDictionary, List<CsAjufin> pAjufinCollection);

        #endregion

        #region MONNAIE

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsMonnaie> SelectAllMonnaie();
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool DeleteMonnaie(CsMonnaie pMonnaie);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool UpdateMonnaie(List<CsMonnaie> pMonnaieCollection);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool InsertMonnaie(List<CsMonnaie> pMonnaieCollection);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool EditerListeMonnaie(string key, Dictionary<string, string> pDictionary, List<CsMonnaie> pMonnaieCollection);

        #endregion

        #region REDEVANCE

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsRedevance> SelectAllRedevance();
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool DeleteRedevance(CsRedevance pRedevance);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool UpdateRedevance(List<CsRedevance> pRedevanceCollection);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool InsertRedevance(List<CsRedevance> pRedevanceCollection);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool EditerListeRedevance(string key, Dictionary<string, string> pDictionary, List<CsRedevance> pRedevanceCollection);

        #endregion

        //#region NATURECLIENT

        //[OperationContract]
        //[FaultContract(typeof(Errors))]
        //List<CsNatureClient> SelectAllNatureClient();
        //[OperationContract]
        //[FaultContract(typeof(Errors))]
        //bool DeleteNatureClient(CsNatureClient pNatureClient);
        //[OperationContract]
        //[FaultContract(typeof(Errors))]
        //bool UpdateNatureClient(List<CsNatureClient> pNatureClientCollection);
        //[OperationContract]
        //[FaultContract(typeof(Errors))]
        //bool InsertNatureClient(List<CsNatureClient> pNatureClientCollection);
        //[OperationContract]
        //[FaultContract(typeof(Errors))]
        //bool EditerListeNatureClient(string key, Dictionary<string, string> pDictionary, List<CsNatureClient> pNatureClientCollection);

        //#endregion

        #region Table SECTEUR

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsSecteur> SelectAllSecteur();
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool DeleteSecteur(CsSecteur pSecteur);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool UpdateSecteur(List<CsSecteur> pSecteurCollection);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool InsertSecteur(List<CsSecteur> pSecteurCollection);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool EditerListeSecteur(string key, Dictionary<string, string> pDictionary, List<CsSecteur> pSecteurCollection);

        #endregion

        #region Table CAISSE

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsCaisse> SelectAllCaisse();
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool DeleteCaisse(CsCaisse cCaisse);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool UpdateCaisse(List<CsCaisse> CsCaisse);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool InsertCaisse(List<CsCaisse> cCaisseCollection);
       
        #endregion

        #region Table APPAREILS

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsAppareils> SelectAllAPPAREILS();
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool DeleteAppareils(CsAppareils cAppareils);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool UpdateAppareils(List<CsAppareils> CsAppareils);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool InsertAppareils(List<CsAppareils> cAppareilsCollection);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool EditerListeAppareils(string key, Dictionary<string, string> pDictionary, List<CsAppareils> AppareilsCollection);

        #endregion

        #region Table COUTCOPER

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsCoutCoper> SelectAllCoutcoper();
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool DeleteCoutcoper(CsCoutCoper cCoutcoper);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool UpdateCoutcoper(List<CsCoutCoper> cCoutcoper);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool InsertCoutcoper(List<CsCoutCoper> cCoutcoperCollection);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool EditerListeCoutcoper(string key, Dictionary<string, string> pDictionary, List<CsCoutCoper> CoutcoperCollection);

        #endregion

        #region Table COPER

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsCoper> SelectAllCoper();
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool Deletecoper(CsCoper ccoper);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool Updatecoper(List<CsCoper> ccoper);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool Insertcoper(List<CsCoper> ccoperCollection);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool EditerListecoper(string key, Dictionary<string, string> pDictionary, List<CsCoper> coperCollection);

        #endregion

        #region Table COPERDEMANDE

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsCoutDemande> SelectAllCoperDemande();
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool DeleteCoperDemande(CsCoutDemande cCoperDemande);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool UpdateCoperDemande(List<CsCoutDemande> cCoperDemande);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool InsertCoperDemande(List<CsCoutDemande> cCoperDemandeCollection);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool EditerListeCoperDemande(string key, Dictionary<string, string> pDictionary, List<CsCoutDemande> coperDeCollection);

        #endregion

        #region Table TDEM

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsTdem> SelectAllDTEM();
        //[OperationContract]
        ////[FaultContract(typeof(Errors))]
        //bool DeleteCoperDemande(CsCoperDemande cCoperDemande);
        //[OperationContract]
        //[FaultContract(typeof(Errors))]
        //bool UpdateCoperDemande(List<CsCoperDemande> cCoperDemande);
        //[OperationContract]
        //[FaultContract(typeof(Errors))]
        //bool InsertCoperDemande(List<CsCoperDemande> cCoperDemandeCollection);
        //[OperationContract]
        //[FaultContract(typeof(Errors))]
        //bool EditerListeCoperDemande(string key, Dictionary<string, string> pDictionary, List<CsCoperDemande> coperDeCollection);


        #endregion

        #region Table FOURNITURE

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<ObjFOURNITURE> SelectAllFourniture();
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool DeleteFourniture(ObjFOURNITURE cFourniture);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool UpdateFourniture(List<ObjFOURNITURE> cFourniture);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool InsertFourniture(List<ObjFOURNITURE> cFourniture);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool EditerListeFourniture(string key, Dictionary<string, string> pDictionary, List<ObjFOURNITURE> FournitureCollection);

        #endregion

        #region MATERIEL
        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsMaterielDemande> SelectAllMateriel();

        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool DeleteMateriel(CsMaterielDemande cMateriel);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool UpdateMateriel(List<CsMaterielDemande> cMateriel);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool  InsertMateriel(List<CsMaterielDemande> cMateriel);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool EditerListeMateriel(string key, Dictionary<string, string> pDictionary);

        #endregion


        #region MarqueModele
        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsMarque_Modele> SelectAllMarqueModele();

        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool DeleteMarqueModele(CsMarque_Modele cMarqueModele);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool UpdateMarqueModele(List<CsMarque_Modele> cMarqueModele);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool InsertMarqueModele(List<CsMarque_Modele> cMarqueModele);
        #endregion

        #region Modele
        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsMarque_Modele> SelectAllModel();

        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool DeleteModele(CsMarque_Modele cModele);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool UpdateModele(List<CsMarque_Modele> cModele);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool InsertModele(List<CsMarque_Modele> cModele);
        #endregion

        #region Couleur
        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsCouleurScelle> SelectAllCouleur();

        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool DeleteCouleur(CsCouleurScelle cCouleur);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool UpdateCouleur(List<CsCouleurScelle> cCouleur);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool InsertCouleur(List<CsCouleurScelle> cCouleur);
        #endregion

        #region Activiter
        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsActivite> SelectAllActivite();

        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool DeleteActivite(CsActivite cActivite);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool UpdateActivite(List<CsActivite> cActivite);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool InsertActivite(List<CsActivite> cActivite);
        #endregion

        #region ActiviterCouleur
        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsCouleurActivite> SelectAllActiviteCouleur();

        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool DeleteActiviteCouleur(CsCouleurActivite cActivite);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool UpdateActiviteCouleur(List<CsCouleurActivite> cActivite);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool InsertActiviteCouleur(List<CsCouleurActivite> cActivite);

        #endregion




        #region GRC_GROUPE

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsGroupe> SelectAllGRCGroupe();


        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool DeleteGRCGroupe(CsGroupe pGroupe);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool UpdateGRCGroupe(List<CsGroupe> pGroupeCollection);


        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool InsertGRCGroupe(List<CsGroupe> pGroupeCollection);


        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool EditerListeGroupe(string key, Dictionary<string, string> pDictionary);


        #endregion




        #endregion

        #region WORKLOW

        #region Opérations

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsOperation> SelectAllOperationParent();
        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsOperation> SelectAllOperation();
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool InsertOperation(List<CsOperation> pOperationCollection);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool UpdateOperation(List<CsOperation> pOperationCollection);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool DeleteOperation(List<CsOperation> pOperationCollection);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsVwConfigurationWorkflowCentre> SelectAllConfigurationWorkflowCentre();
        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsOperationFormulaire> SelectAllViewOperationFormulaire();

        #endregion

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsFormulaire> SelectAllFormulaire();

        #region Etapes

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsEtape> SelectAllEtapes();

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsEtape> SelectAllEtapesByOperationId(Guid OperationId);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool InsertEtape(List<CsEtape> pEtapeCollection);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool UpdateEtape(List<CsEtape> pEtapeCollection);


        #endregion

        [OperationContract]
        [FaultContract(typeof(Errors))]
        Dictionary<CsGroupeValidation, List<CsRHabilitationGrouveValidation>> SelectAllGroupeValidation();
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool InsertGroupeValidation(Dictionary<CsGroupeValidation, List<CsRHabilitationGrouveValidation>> lsGroupeEtHabilitation);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool UpdateGroupeValidation(Dictionary<CsGroupeValidation, List<CsRHabilitationGrouveValidation>> pGrpCollection);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool DeleteGroupeValidation(List<CsGroupeValidation> pGrpCollection);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsGroupeValidation> SelectGroupeValidationByUserId(int UserId);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsWorkflow> SelectAllWorkflow();
        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsTableDeTravail> SelectAllTableTravail();
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool InsertWorkflow(List<CsWorkflow> pWorkflowCollection);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool UpdateWorkflow(List<CsWorkflow> pWorkflowCollection);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool DeleteWorkflow(List<CsWorkflow> pWorkflowCollection);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        Dictionary<CsRAffectationEtapeWorkflow, CsConditionBranchement> SelectAllAffectationEtapeWorkflow(Guid pRWKF);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        Dictionary<CsRAffectationEtapeWorkflow, CsConditionBranchement> SelectAllAffectationEtapeCircuitDetourne(Guid pOrigineAffectationEtape);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool InsertAffectationEtapeWorkflow(Dictionary<CsRAffectationEtapeWorkflow, CsConditionBranchement> lsEtapesEtConditions);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool InsertAffectationEtapeWorkflowParCentre(List<Dictionary<CsRAffectationEtapeWorkflow, CsConditionBranchement>> lsEtapesEtConditions);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool UpdateAffectationEtapeWorkflow(Dictionary<CsRAffectationEtapeWorkflow, CsConditionBranchement> lsEtapesEtConditions);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool DeleteAffectationEtapeWorkflow(List<CsRAffectationEtapeWorkflow> lsEtapes);


        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool UpdateGroupValidationCopieCircuit(string idDemande, string numdem, Guid idGroup);


        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsRWorkflow> SelectAllRWorkflowCentre(Guid pKIDWKF, int CpKID, Guid OpPKID);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool InsertRWorkflow(List<CsRWorkflow> lsRWorkflow);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool UpdateRWorkflow(List<CsRWorkflow> lsRWorkflow);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool DeleteRWorkflow(List<CsRWorkflow> lsRWorkflow);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool ConfigurerPlusieurWorkflowEtCentre(List<CsRWorkflow> rwkfCentres, Dictionary<CsRAffectationEtapeWorkflow, CsConditionBranchement> configurationsEtapes, List<CsRenvoiRejet> _renvois);

        //[OperationContract]
        //[FaultContract(typeof(Errors))]
        //bool SupprimerUneConfiguration(Guid RWKFCentre);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        string[] GetColumnsOfWorkingTable(string TableName);

        #region Gestion des demandes

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsVwDashboardDemande> SelectVwDashBoardDemande(List<int> lstCentre, int IdAgentConnet);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsVwDashboardDemande> SelectVwDashBoardDemandeByHabilitation(int centreId, List<Guid> GrpsValidation);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsVwDashboardDemande> SelectVwDashBoardDemandeByHabilitationListCentre(List<int> centresId, List<Guid> GrpsValidation);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsVwJournalDemande> SelectVwJournalDemande();

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsJournalDemandeWorkflow> SelectJournalDeLaDemande(string codeDemande);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        CsVwJournalDemande SelectInformationDemande(string codeDemande);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        KeyValuePair<CsCopieDmdCircuit, KeyValuePair<CsGroupeValidation,
            List<CsRHabilitationGrouveValidation>>> RecupererInfoEtapeSuivante(string codeDemande);

        [OperationContract]
        [FaultContract(typeof(Errors))]
         KeyValuePair<CsCopieDmdCircuit, KeyValuePair<CsGroupeValidation,
         List<CsRHabilitationGrouveValidation>>> RecupererInfoEtapeSuivanteByCodeWorkflow(string codeDemande);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsAffectationDemandeUser> SelectLesAffectationsTraitementDemande(string matriculeUser);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        Dictionary<CsAffectationDemandeUser, CsVwJournalDemande> GetLesDemandesAffecteFromMatriculeUser(string matriculeUser, int idEtape,
            Guid idOperation);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        Dictionary<CsAffectationDemandeUser, CsVwJournalDemande> GetLesDemandesAffecteForMatriculeUser(string matriculeUser, int idEtape,
            Guid idOperation);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool AffecterDemande(List<CsAffectationDemandeUser> lesAffectations);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool CheckForAffectationFromEtape(int idEtape, Guid IdOperation, string MatriculeUserCreation);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool CheckForAffectationForEtape(int idEtape, Guid IdOperation, string MatriculeUserAffected);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool VerifierAppartenanceGroupeValidation(int UserId);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsGroupeValidation> SelectAllGroupeValidationSpecifique(int TypeGroupe);



        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsCommentaireRejet> SelectCommentaireRejet(Guid pKIDDemande);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool InsertCommentaireRejet(List<CsCommentaireRejet> lsCommentaires);
        /*[OperationContract]
        [FaultContract(typeof(Errors))]
        bool UpdateCommentaireRejet(List<CsCommentaireRejet> lsCommentaires);*/

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsCopieDmdCircuit> SelectEtapesFromDemande(string CodeDemande);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsCopieDmdConditionBranchement> SelectConditionParEtapes(List<Guid> PKIDEtapeCopieCircuit);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        CsDemandeWorkflow GetInfoDemandeWorkflowByCodeDemande(string numDmd);


        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsDemandeWorkflow> GetInfoDemandeWorkflowByListCodeDemande(List<string> PlisteDemande);


        #endregion

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsRenvoiRejet> GetLesRenvoisRejet(int fkIdEtapeActuelle);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool AjouterRenvoisRejet(List<CsRenvoiRejet> __renvois);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool SupprimerLesRenvoisRejet(int fkIdEtapeActuelle);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsVwJournalDemande> SelectAllDemandeUser(List<int> lstCentreId, List<Guid> lstIdDemande, int Fk_idEtape, string Matricule);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsVwJournalDemande> SelectAllDemandeUserEtape(List<int> lstCentreId, int Fk_idEtape, string Matricule);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsVwJournalDemande> RetourneDemandeWkfEtape(List<int> lstCentreId, int Fk_idetape, string matriculeUser);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsVwJournalDemande> RetourneDemandeWkfClient(List<int> lstCentreId, int Fk_idetape, string matriculeUser, string NumeroDemande, bool IsToutAfficher);



        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsVwJournalDemande> RetourneDemandeWkf(List<int> lstIdCentre);
        #endregion

        #region PARAMETRECOUPURESCGC
        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsParametreCoupureSGC> SelectAllParamatreSCGC();
        #endregion

        //[OperationContract]
        //[FaultContract(typeof(Errors))]
        //List<CsVwJournalDemande> RetourneDemandeWkfEtape(List<int> lstCentreId, int Fk_idetape, string matriculeUser);

        //[OperationContract]
        //[FaultContract(typeof(Errors))]
        //List<CsVwDashboardDemande> SelectVwDashBoardDemande(List<int> lstCentre, int IdAgentConnet);

    }
}
