using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Galatee.Structure;
using System.Data;

namespace WcfService
{
    // REMARQUE : vous pouvez utiliser la commande Renommer du menu Refactoriser pour changer le nom d'interface "IAuthentInitializeService" à la fois dans le code et le fichier de configuration.
    [ServiceContract]
    public interface IAuthentInitializeService
    {
        #region AUTHETIFICATION & INITIALISATION PARAMETRES
        [OperationContract]
        [FaultContract(typeof(Errors))]
        string LireLicence();

        //[OperationContract]
        //[FaultContract(typeof(Errors))]
        //bool EcrireFichier(string Message, string CheminLog);

        [OperationContract]
        [FaultContract(typeof(Errors))]
         bool GenereLicence(string licence);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool EcrireFichierDeAbo07Connexion(string InstanceBd, string Catalogue, string userid, string MotPasse);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool EcrireFichierDeGaladbConnexion(string InstanceBd, string Catalogue, string userid, string MotPasse);
        

        [OperationContract]
        [FaultContract(typeof(Errors))]
        CsStrategieSecurite GetStrategieSecuriteActif();

        #region LICENSE
        [OperationContract]
        [FaultContract(typeof(Errors))]
        string[] SaveFile(byte[] File);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        string LicenseInferieurPeriodeAvertissemnt();

        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool LicenseInferieurPeriodeValidite();


        #endregion
        [OperationContract]
        [FaultContract(typeof(Errors))]
        EnumProcedureStockee returnEnumProcedureStockee();

        [OperationContract]
        [FaultContract(typeof(Errors))]
        Dictionary<CSMenuGalatee, List<TreeNodes>> GetMenuDuRole(int idCodeFonction, string Module);


        //[OperationContract]
        //[FaultContract(typeof(Errors))]
        //Dictionary<CSMenuGalatee, List<TreeNodes>> GetMenuDuProfil(string Module);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        //Dictionary<CSMenuGalatee, List<TreeNodes>> GetMenuDuProfil(string Module, int idprofil);
        Dictionary<CSMenuGalatee, List<TreeNodes>> GetMenuDuProfil(string Module, List<int> idprofil);


        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsDesktopGroup> GetListeModule(CsUtilisateur agent);

        //[OperationContract]
        //[FaultContract(typeof(Errors))]
        //List<CsModule> GetListeDesModule();

        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool UpdateUser(CsUtilisateur admUsers);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        CsUserConnecte RetourneInfoMatriculeConnecte(string matricule, string pwd);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        EnumereWrap returnEnumereWrapper();

        
        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CParametre> RetourneListImprante();

 
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool? PrintReceipt(byte[] pRenderStream);

        [OperationContract]
        String getDefaultPrinter();

        #endregion
        [OperationContract]
        [FaultContract(typeof(Errors))]
        CsUtilisateur GetByLoginName(string LoginName);

        #region 07/06/2016 SYLLA

        [OperationContract]
        [FaultContract(typeof(Errors))]
        void TraceDeConnection(int IdUser, string Post);

        #endregion
    }
}
