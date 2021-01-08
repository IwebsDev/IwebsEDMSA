using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Galatee.Structure;

namespace WcfService
{
    // REMARQUE : vous pouvez utiliser la commande Renommer du menu Refactoriser pour changer le nom d'interface "IAdministrationService" à la fois dans le code et le fichier de configuration.
    [ServiceContract]
    [ServiceKnownType(typeof(CsLibelle ))]
    public interface IAdministrationService
    {
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool saveProfilHabilitation(CsProfil csProfil,List<CsHabilitationProgram> menuProfil);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsUtilisateur> InsertUpdateUser(CsUtilisateur admUsers, List<CsProfil> anciensProfils);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsUtilisateur> saveUserProfil(CsUtilisateur csUser, List<CsProfil> lesProfils);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsUtilisateur> saveUserProfilCentre(CsUtilisateur csUser);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsModuleAdmMenu> RetourneCsModuleAdmMenu();

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsCentreDuProfil> RetourneCentreDuProfil();

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsUtilisateur> RetourneListeAllUser();

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsUtilisateur> RetourneListeAllUserPerimetre(List<int> lstCentrePerimetreAction);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsModule> GetListeDesModule();

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsAdmMenu> GetListeDesMenusProfil();
        

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsModule> RetourneAllModuleFonction();

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsAdmMenu> MenusSelectByFonction(int metier);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsModule> HabilitationSelectByMetierModule(string metier);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool InsertionModuleDeFonction(List<CsModuleDeFonction> modules);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool InsertionProfilFonction(List<CsProfil> modules);
       


        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsProfil> RetourneListeAllProfilUser();

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsProfil> RetourneProfilByFonction(string fonction);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsProfil> RetourneProfilByID(int idprofil);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsDataBase> RetourneBdConfig();

        [OperationContract]
        [FaultContract(typeof(Errors))]
        CParametre RetourneTa(string centre,int num,string code);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool UpdateStrategieSecurite(CsStrategieSecurite admStrategieSecurite);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsStrategieSecurite> InsertStrategieSecurite(CsStrategieSecurite admStrategieSecurite);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool DeleteStrategieSecurite(Guid IdStrategieSecurite);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsStrategieSecurite> SetActifStrategieSecurite(Guid IdStrategieSecurite);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        CsStrategieSecurite GetActifStrategieSecurite();

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsStrategieSecurite> GetAllStrategieSecurite();

        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool InsertionHabilitationProfil(List<CsProfil> modules);


        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsAdmRoles> GetAllRole();

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsProgramMenu> RetourneAllModuleProgram();

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsModuleDeFonction> RetourneAllModuleDeFonction();

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsAdmRoles> GetAssocitedToMenuByMenuID(int menuId);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsSite> GetAllSite();

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsCentre> RetourneListeDesCentre();

        [OperationContract]
        [FaultContract(typeof(Errors))]
        CsUtilisateur GetByLoginName(string LoginName);

               [OperationContract]
        [FaultContract(typeof(Errors))]
        CsUtilisateur VerifieUserExist(string LoginName);
        
  

        [OperationContract]
        [FaultContract(typeof(Errors))]
         bool UpdateUser(CsUtilisateur admUsers,bool IsInitpassword);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsUtilisateur> InsertUser(CsUtilisateur admUsers);

        //[OperationContract]
        //[FaultContract(typeof(Errors))]
        //bool DeleteUserByMatricule(string matricule);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool? DeleteUser(CsUtilisateur matricule);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool DeleteUserByGuid(Guid guid);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsFonction> SELECT_All_Fonction();



        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsHabilitationProgram> ProgramSelectAll();

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsPoste> RetourneListePoste();

        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool InsertPoste(CsPoste lePoste);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsPoste> UpdatePoste(CsPoste lePoste);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsHistoriquePassword> RetourneHistoriquePassword(int idUser);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsHistoriquePassword> RetourneHistoriqueConnection(int idUser);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool SynchroniseDonneeAD();

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsAgent> ChargeListeDesAgents();

        #region import
                [OperationContract]
        [FaultContract(typeof(Errors))]
        int MisaAjourImportFichier(aImportFichier limport);
        
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool SuppressImportFichier(aImportFichier limport);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        aImportFichier GetImportFichier(int codeimport);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<string> ExexcImporterFichier(aImportFichier codeimport);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool MisaAjourImportColonne(aImportFichierColonne lacolonne);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        aImportFichierColonne GetImportColonne(int codeColonne);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool DeleteColonne(int codeColonne);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<aImportFichierColonne> GetAllImportFichierColonne(int codeimport);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool ImportDepuisBaseDeDonnee(aImportFichier limport);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<aImportFichier> GetAllImport();

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<string> GetSQLDatabaseList(string serverInstanceName, bool useWindowsAuthentication, string username, string password);
     
        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<string> GetProviderList();


        [OperationContract]
        [FaultContract(typeof(Errors))]
        string LoadRequest();
      
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool SaveAgent(List<CsAgent> agenttosynchro, string Requette);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsAgent> LoadAgentBaseDistante(string Requette, string Provider, string DataSource, string IniialeCatalog, string UserId, string Password);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool TestBaseDistante(string Requette, string Provider, string DataSource, string IniialeCatalog, string UserId, string Password);
        #endregion



        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsAdmMenu> MenusSelectByProfil(int metier);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsHabilitationMetier> RetourneFonctionProfilCentre(string codefonction);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsHabilitationMenu> RetourneFonctionProfilMenu(string codefonction);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool InsertionMutation(Galatee.Structure.CsUtilisateur lutilisateur, string newCentre, DateTime datedebut, DateTime? datefin);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsHabilitationMenu> RetourneProfilUtilisateur(List<int> idUtilisateur);


        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsHistoriquePassword> RetourneHistoriquePasswords(int idUser);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool? DeleteListUser(List<CsUtilisateur> user);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsHistoriquePassword> RetourneHistoriqueConnectionfromListUser(List<int?> idUser);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsHistoriquePassword> RetourneHistoriquePasswordFromListUser(List<int?> idUser);
    }
}