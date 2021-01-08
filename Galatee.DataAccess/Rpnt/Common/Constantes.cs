using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Galatee.DataAccess.Common
{
    public static class Constantes
    {
        //- Contrôle versionning
        public const string cVersionBaseCode = "1.0";
        public const string cVersionApplicative = "1.0.0.0";
        //public const string cVersionServiceWCF = "0.0.0.9";

        public const string cFullNameInterfaceServiceSynchro = "Cie.RPNT.Common.IServiceSynchroPNT";

        //- Nom de la chaîne de connexion RPNT
        public const string RPNTConnexionStringName = "CIE_RPNTConnectionString";
        public const string RPNTSynchroConnexionStringName = "CIE_RPNTSynchroConnectionString";

        public const string SaphirV2BTConnexionStringName = "GesabelDevConnectionString";
        public const string SaphirV2MTBaseConnexionStringName = "MTBaseConnectionString";
        public const string SaphirV2ReferenceConnexionStringName = "ReferenceConnectionString";

        //- Utilisateur batch
        public const string MatriculeUserBatch = "PNTBatch";
        public const string NomUserBatch = "Batch";
        public const string PrenomsUserBatch = "Batch";
        public const string LoginUserBatch = "BATCH";

        //- Nom des paramètres du fichier de configuration du service Synchro
        //public const string paramSettingTempPathSynchro = "TempPathSynchro";
        public const int cIntervalleParDefaut_ServiceSynchro_EnMinutes = 5;
        public const int cIntervalleParDefaut_ServicePRESynchro_EnMinutes = 30;
        public const string cValeurModifSiteSynchroImpacte_False = "0";
        public const string cValeurModifSiteSynchroImpacte_True = "1";
        public const string paramSettingPeriodiciteCheckService = "Periodicite Check Service";
        public const string paramSettingCodeSiteServicePRESynchro = "Code site";
        public const string paramSettingNomSiteServicePRESynchro = "Nom site";
        public const string paramSettingModifSiteSynchroImpacte = "Modif site impacte";
        public const string paramSettingLogPathSynchro = "logPathSynchro";
        public const string paramSettingLogPathServiceSynchro = "logPathServiceSynchro";

        public const string ctrfacture_typctr_Jour = "01";
        public const string ctrfacture_typctr_Nuit = "02";
        public const string ctrfacture_typctr_Pointe = "03";

        //- Constantes utilisées par les méthodes de détection de clients à contrôler
        //public const int cNbreMaxEltsDeCampagneParTransfertWCF = 2000;

        public const string EndPointConfigName_RPNT1 = "WSHttpBinding_IServiceRPNT";
        public const string EndPointConfigName_RPNT_BTA = "WSHttpBinding_IServiceRPNT_BTA";
        public const string EndPointConfigName_RPNT_HTA = "WSHttpBinding_IServiceRPNT_HTA";
        public const string EndPointConfigName_RPNT_Synchro = "WSHttpBinding_IServiceRPNTSynchro";

        //- Nom des paramètres adresse EndPoints du service WCF RPNT
        public const string paramSettings_EndPointAddress_RPNT1 = "Uri PNT1";
        public const string paramSettings_EndPointAddress_RPNT_BTA = "Uri PNT BTA";
        public const string paramSettings_EndPointAddress_RPNT_HTA = "Uri PNT HTA";
        public const string paramSettings_EndPointAddress_RPNT_Synchro = "Uri PNT Synchro";
    }
}
