using System;
using System.Resources;
using System.Reflection;

namespace Galatee.Sms.Tools
{
    public enum eCodeErreur
    {
        NO_ERROR = 0,
        ERROR,
        COMPTEUR_INEXISTANT,
        COMPTEUR_INACTIF,
        PB_RECHERCHE_COMPTEUR,
        CONSO_INFERIEUR_UNITE,
        FIN_DIVISION_CONSO_TRANCHE,
        PB_CHARGEMENT_DONNEES_DE_BASE,
        PB_RECHERCHE_TARIF_FIXE,
        PB_RECHERCHE_TARIF_CONSO,
        CAISSE_INEXISTANTE,
        PB_OUVERTURE_DE_CAISSE,
        PB_FERMETURE_DE_CAISSE,
        AUCUNE_CAISSE_OUVERTE,
        SESSION_UTILISATEUR_OUVERTE,
        PERIODECOURANTE_INF_DERNIEREPERIODE,
        SESSION_AUTRE_UTILISATEUR_OUVERTE,
        PB_GENERATION_DETAIL_ACHAT,
        PB_VALIDATION_ACHAT_BASE,
        DETTE_SUP_ACHAT,
        PRIMEFIXE_SUP_ACHAT,
        DETTEPRIMEFIXE_SUP_ACHAT,

        COMPTEUR_LONGUEUR_DRN_ERRONE,
        COMPTEUR_NUMERO_NON_NUMERIQUE,
        COMPTEUR_CHECKDIGIT_COMPTEUR_ERRONNE,

        TOKEN_ERREUR,
        TOKENID_ERREUR,
        TOKENID_DATE_ERREUR,
        TOKENAMOUNT_ERREUR,
        TOKENCRC_ERREUR,
        TOKENCLASS_ERREUR,
        TOKENSUBCLASS_ERREUR,
        SECURITYMODULE_NOT_READY,

        ERREUR_CHAINECONNEXION,
        PB_CONNECTION_BASEDEDONNEES,

        CHAMPS_NON_RENSEIGNE,

        UTILISATEUR_INEXISTANT,
        EQUIPEMENT_NON_HABILITE,
        AUCUNE_ENTREPRISE_PAR_DEFAUT,
        OPERATION_ANNULEE,
        CAISSE_VERROUILLEE,
        PLUSIEURS_SESSIONS_CAISSE_OUVERTE,
        MOT_DE_PASSE_ERRONE,
        UTILISATEUR_NON_AUTORISE_POUR_CETTE_ACTION,

        MODEM_NON_INITIALISE,
        AUCUN_MODEM_CONNECTE_AU_PORT,
        ECHEC_MODIFICATION_FORMAT_SMS,
        LECTURE_SMS_TRONQUE,
        ECHEC_LECTURE_SMS,
        ECHEC_ENVOIE_SMS,

        CAISSE_NON_ATTRIBUEE,
        CAISSE_NON_OUVERTE,
        PROBLEME_MODIFICATION_MOT_PASSE,
        AUCUNE_LIGNE_POUR_CETTE_RECHERCHE,
        MONTANT_ACHAT_SUPERIEUR_MONTANT_MAXIMAL,
        CAISSE_DEJA_VERROUILLEE,
        CAISSE_DEJA_DEVERROUILLEE,
        FORMAT_SMS_INCORRECTE,
        AUCUNE_REDEVANCE_DEFINIE_POUR_L_ABONNE,
        ECHEC_ENVOIE_SMS_MESSAGE_TROP_LONG
    }
    public partial class CErreurAppli
    {
        public eCodeErreur m_eCode;
        public String m_sMessage;
        static ResourceManager rm = new ResourceManager("Galatee.Sms.Tools.Properties.Resources", Assembly.GetExecutingAssembly());
        public CErreurAppli()
        {
            m_eCode = eCodeErreur.NO_ERROR;
            m_sMessage = "";
        }
        public CErreurAppli(eCodeErreur code)
        {
            m_eCode = code;
            if (code==eCodeErreur.NO_ERROR)
                m_sMessage = "";
            else
                m_sMessage = rm.GetString(m_eCode.ToString());
            if (m_sMessage == null || m_sMessage == "")
                m_sMessage = m_eCode.ToString();

        }

        public CErreurAppli(eCodeErreur code, String Message)
        {
            m_eCode = code;
            m_sMessage = Message;
        }

        public void Clear()
        {
            m_eCode = eCodeErreur.NO_ERROR;
            m_sMessage = "";
        }

        public void Set(eCodeErreur code)
        {
            m_eCode = code;
            if (code == eCodeErreur.NO_ERROR)
                m_sMessage = "";
            else
                m_sMessage = rm.GetString(m_eCode.ToString());

            if (m_sMessage == null || m_sMessage == "")
                m_sMessage = m_eCode.ToString();
        }

        public void Set(eCodeErreur code, String Message)
        {
            m_eCode = code;
            m_sMessage = Message;
        }

        public eCodeErreur eCode
        {
            get
            {
                return m_eCode;
            }
        }

        public String MessageErreur
        {
            get
            {
                if (m_sMessage == null || m_sMessage == "")
                {
                    m_sMessage = rm.GetString(m_eCode.ToString());
                    if (m_sMessage == null || m_sMessage == "")
                        m_sMessage = m_eCode.ToString();
                }
                return m_sMessage;
            }
        }
    }


    public enum eCodeErreurModuleSecurite
    {
        SUCCESSFUL = 0,
        DEVICE_FAILURE = 1,
        FORMAT_ERROR = 2,
        COMMAND_TIMED_OUT = 3,
        KEY_NUMBER_ERROR = 4,
        KEY_TYPE_ERROR = 5,
        KEY_INTEGRITY_ERROR = 6,
        KEY_PARITY_ERROR = 7,
        REDEFINITION_FAILED = 8,
        INPUT_DISPLAY_FAILED = 9,
        KEYPAD_ENTRY_CANCELLED = 10,
        INVALID_FUNCTION_KEY_MASK = 11,
        INVALID_AMOUNT_FORMAT = 12,
        READER_NOT_SUPPORTED = 13,
        CARD_NOT_READ = 14,
        MESSAGE_CYCLE_BUFFER_VIOLATION = 15,
        INVALID_PIN = 16,
        DEA2_PUBLIC_KEY_ERROR = 17,
        INVALID_PASSWORD = 18,
        INVALID_REPRESENTATION = 19,
        CHECKSUM_ERROR = 20,
        INVALID_REQUEST_HEADER = 21,
        MAX_KEY_COMPONENTS = 22,
        INVALID_DOMAIN = 23,
        KEY_TOKEN_ERROR = 24,
        WEAK_KEY_ERROR = 25,
        RSA_DES_KEYS_NOT_CLEARED = 26,
        PASSWORD_LOCKOUT = 27,
        PASSWORD_NOT_SET_ERROR = 28,
        PASSWORD_INTEGRITY_ERROR = 29,
        INVALID_STS_TOKEN = 30,
        INSUFFICIENT_CREDIT_BALANCE = 31,
        CREDIT_UPDATE_BLOCK_FORMAT_ERROR = 32,
        INVALID_SECURITY_MODULE_ID = 33,
        INVALID_CREDIT_UPDATE_SEQUENCE_NO = 34,
        CREDIT_OVERFLOW = 35,
        TARIFF_CREDIT_INTEGRITY_ERROR = 37,
        NO_TARIFF_CREDIT_DATA = 38,
        MAX_CREDIT_DOMAINS_REACHED = 39,
        SWITCHOVER_ERROR = 39,
        INVALID_DATE_ERROR = 40,
        PAM_CREDIT_UNDERFLOW = 41,
        PAM_TARIFF_EXPIRED = 42,
        KEYPAD_ENTRY_TERMINATED = 50,
        KEYPAD_ENTRY_CLEARED = 51,
        KEYPAD_ENTRY_ERROR = 52,
        INVALID_TERMINAL = 60,
        RSA_BLOCK_ERROR = 61,
        BALANCE_ERROR = 64,
        TRANS_SEQUENCE_NUMBER_ERROR = 65,
        INVALID_CARD_TYPE = 66,
        INVALID_OPTION = 67,
        RANDOM_NUMBER_ERROR = 68,
        PROTOCOL_SEQUENCE_ERROR = 69,
        REVERSAL_ERROR = 70,
        PIN_BLOCK_ERROR = 71,
        KEY_REGISTER_ERROR = 72,
        INVALID_AMOUNT = 73,
        PIN_ERROR = 74,
        TOKEN_INTERFACE_ERROR = 75,
        TOKEN_FORMAT_ERROR = 76,
        AUTHENTICATION_ERROR = 77,
        CHECK_DIGIT_ERROR = 78,
        NO_CARD_PRESENT = 79,
        INVALID_TOKEN = 80,
        SIGNATURE_ERROR = 81,
        KEY_PROTOCOL_ERROR = 82,
        PUBLIC_KEY_ALREADY_PRESENT = 83,
        PUBLIC_KEY_NOT_LOADED_ERROR = 84,
        DEVICE_NOT_READY = 99,
        APPLICATION_ERROR = 100,
        NO_DATA = 101,
        DEVICE_PORT_CLOSED = 102,
        KEYFILE_NOT_EXIST = 103,
        INVALID_KEY_FORMAT = 104,
        TOKEN_KEY_EXPIRED = 105,
        DEVICE_NO_COMPANY_KEY_LOADED = 106,
        KEYLOAD_WRONG_DEVICE,
        KEYLOAD_WRONG_DEVICE_AUT_CODE,
        KEYLOAD_WRONG_FIRMWARE_AUT_CODE
    }

    public partial class CErreurModuleSecurite
    {
        public eCodeErreurModuleSecurite m_eCode;
        public String m_sMessage;
        static ResourceManager rm = new ResourceManager("Galatee.Sms.Tools.Properties.Resources", Assembly.GetExecutingAssembly());
        public CErreurModuleSecurite()
        {
            m_eCode = eCodeErreurModuleSecurite.SUCCESSFUL;
            m_sMessage = "";
        }
        public CErreurModuleSecurite(eCodeErreurModuleSecurite code)
        {
            m_eCode = code;
            if (code == eCodeErreurModuleSecurite.SUCCESSFUL)
                m_sMessage = "";
            else
                m_sMessage = rm.GetString(m_eCode.ToString());
            if (m_sMessage == null || m_sMessage == "")
                m_sMessage = m_eCode.ToString();

        }

        public CErreurModuleSecurite(eCodeErreurModuleSecurite code, String Message)
        {
            m_eCode = code;
            m_sMessage = Message;
        }

        public void Clear()
        {
            m_eCode = eCodeErreurModuleSecurite.SUCCESSFUL;
            m_sMessage = "";
        }

        public void Set(eCodeErreurModuleSecurite code)
        {
            m_eCode = code;
            if (code == eCodeErreurModuleSecurite.SUCCESSFUL)
                m_sMessage = "";
            else
                m_sMessage = rm.GetString(m_eCode.ToString());
            if (m_sMessage == null || m_sMessage == "")
                m_sMessage = m_eCode.ToString();

        }

        public void Set(eCodeErreurModuleSecurite code, String Message)
        {
            m_eCode = code;
            m_sMessage = Message;
        }

        public eCodeErreurModuleSecurite eCode
        {
            get
            {
                return m_eCode;
            }
        }

        public String MessageErreur
        {
            get
            {
                if (m_sMessage == null || m_sMessage == "")
                {
                    m_sMessage = rm.GetString(m_eCode.ToString());
                    if (m_sMessage == null || m_sMessage == "")
                        m_sMessage = m_eCode.ToString();
                }
                return m_sMessage;
            }
        }
    }

}
