using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Configuration;
using System.Runtime.Serialization;

namespace Galatee.Structure
{
    /// <summary>
    /// Class des énumérés procédures stockées
    /// </summary>
    [DataContract]
    public class EnumProcedureStockee
    {

        private string _TA0 = "TA0";

        [DataMember]
        public string TA0
        {
            get { return _TA0; }
            set { _TA0 = value; }
        }
        private string _TA = "TA";
        [DataMember]
        public string TA
        {
            get { return _TA; }
            set { _TA = value; }
        }
        private string _INIT = "INIT";
        [DataMember]
        public string INIT
        {
            get { return _INIT; }
            set { _INIT = value; }
        }
        private string _RUES = "RUES";
        [DataMember]
        public string RUES
        {
            get { return _RUES; }
            set { _RUES = value; }
        }

        private string _REGROU = "REGROU";
        [DataMember]
        public string REGROU
        {
            get { return _REGROU; }
            set { _REGROU = value; }
        }
        private string _DIACOMP = "DIACOMP";
        [DataMember]
        public string DIACOMP
        {
            get { return _DIACOMP; }
            set { _DIACOMP = value; }
        }
        private string _TCOMPT = "TCOMPT";
        [DataMember]
        public string TCOMPT
        {
            get { return _TCOMPT; }
            set { _TCOMPT = value; }
        }
        private string _PUISSANCE = "PUISSANCE";
        [DataMember]
        public string PUISSANCE
        {
            get { return _PUISSANCE; }
            set { _PUISSANCE = value; }
        }
        private string _TARIF = "TARIF";
        [DataMember]
        public string TARIF
        {
            get { return _TARIF; }
            set { _TARIF = value; }
        }
        private string _FORFAIT = "FORFAIT";
        [DataMember]
        public string FORFAIT
        {
            get { return _FORFAIT; }
            set { _FORFAIT = value; }
        }
        private string _BANQUE = "BANQUE";
        [DataMember]
        public string BANQUE
        {
            get { return _BANQUE; }
            set { _BANQUE = value; }
        }
        private string _CASIND = "CASIND";
        [DataMember]
        public string CASIND
        {
            get { return _CASIND; }
            set { _CASIND = value; }
        }
        private string _CTAX = "CTAX";
        [DataMember]
        public string CTAX
        {
            get { return _CTAX; }
            set { _CTAX = value; }
        }
        private string _MATRICULE = "MATRICULE";
        [DataMember]
        public string MATRICULE
        {
            get { return _MATRICULE; }
            set { _MATRICULE = value; }
        }
        private string _TDEM = "TDEM";
        [DataMember]
        public string TDEM
        {
            get { return _TDEM; }
            set { _TDEM = value; }
        }
        private string _REGCLI = "REGCLI";
        [DataMember]
        public string REGCLI
        {
            get { return _REGCLI; }
            set { _REGCLI = value; }
        }
        private string _GEOGES = "GEOGES";
        [DataMember]
        public string GEOGES
        {
            get { return _GEOGES; }
            set { _GEOGES = value; }
        }
        private string _QUARTIER = "QUARTIER";
        [DataMember]
        public string QUARTIER
        {
            get { return _QUARTIER; }
            set { _QUARTIER = value; }
        }
        private string _MESSAGE = "MESSAGE";

        public string MESSAGE
        {
            get { return _MESSAGE; }
            set { _MESSAGE = value; }
        }
        private string _SPESITE = "SPESITE";
        [DataMember]
        public string SPESITE
        {
            get { return _SPESITE; }
            set { _SPESITE = value; }
        }
        string _REGEXO = "REGEXO";
        [DataMember]
        public string REGEXO
        {
            get { return _REGEXO; }
            set { _REGEXO = value; }
        }
        private string _DIRECTEUR = "DIRECTEUR";
        [DataMember]
        public string DIRECTEUR
        {
            get { return _DIRECTEUR; }
            set { _DIRECTEUR = value; }
        }
        private string _DEMCOUT = "DEMCOUT";
        [DataMember]
        public string DEMCOUT
        {
            get { return _DEMCOUT; }
            set { _DEMCOUT = value; }
        }
        private string _MODEREG = "MODEREG";
        [DataMember]
        public string MODEREG
        {
            get { return _MODEREG; }
            set { _MODEREG = value; }
        }
        public  string FRAISTIMB = "FRAISTIMB";
        public  string FRAISHP = "FRAISHP";
        public  string COPER = "COPER";
        public  string NATURE = "NATURE";
        public  string DOMBANC = "DOMBANC";
        public  string COPEROD = "COPEROD";
        public  string ARRETE = "ARRETE";
        public  string TAXCOMP = "TAXCOMP";
        public  string IMPRIM = "IMPRIM";
        public  string REDEVANCE = "REDEVANCE";
        public  string NATGEN = "NATGEN";
        public  string SCHEMAS = "SCHEMAS";
        public const string AJUFIN = "AJUFIN";
        public const string FRAISCONTENTIEUX = "FRAISCONTENTIEUX";
        public const string LIBRUBACTION = "LIBRUBACTION";
        public const string SECURITEMATRICULE = "SECURITEMATRICULE";
        public const string MONNAIE = "MONNAIE";
        public const string MULTIMONNAIES = "MULTIMONNAIES";
        public const string DEFPARAMABON = "DEFPARAMABON";
        public const string PARAMABONULILISE = "PARAMABONULILISE";
        public const string TYPELOT = "TYPELOT";
        public const string CONTROLELIGNE = "CONTROLELIGNE";
        public const string CONTROLESECONDAIRE = "CONTROLESECONDAIRE";
        public const string CONTROLEPIECE = "CONTROLEPIECE";
        public const string ORIGINELOT = "ORIGINELOT";
        public const string SECTEUR = "SECTEUR";
        public const string COUTPUISSANCE = "COUTPUISSANCE";
        public const string TYPEBRANCHEMENT = "TYPEBRANCHEMENT";
        public const string PUISPERTES = "PUISPERTES";
        public const string COMPTAGE = "COMPTAGE";
        public const string TRANSFOCOMPTAGE = "TRANSFOCOMPTAGE";
        public const string TACHEDEVIS = "TACHEDEVIS";
        public const string APPAREILS = "APPAREILS";
        public const string FOURNITURE = "FOURNITURE";
        public const string CARACTERISTIQUE = "CARACTERISTIQUE";
        public const string ETAPEDEVIS = "ETAPEDEVIS";
        public const string TYPEDEVIS = "TYPEDEVIS";
        public const string ETAPERECLAMATION = "ETAPERECLAMATION";
        public const string ETAPEFONCTION = "ETAPEFONCTION";
        public const string CENTREENCAISSABLE = "CENTREENCAISSABLE";
        public const string CODECONTROLE = "CODECONTROLE";
        public const string TYPERECLAMATION = "TYPERECLAMATION";
        public const string PRESTATAIRE = "PRESTATAIRE";
        public const string STATUTRECLAMATION = "STATUTRECLAMATION";
        public const string GROUPERECLAMATION = "GROUPERECLAMATION";
        public const string HABILITATIONPROGRAM = "HABILITATIONPROGRAM";
        public const string PROGRAM = "PROGRAM";
        public const string GROUPPROGRAM = "GROUPPROGRAM";
        public const string PRODUIT = "PRODUIT";
        public const string FILIERE = "FILIERE";                                          
        public const string FONCTION = "FONCTION";
        public const string CENTRE = "CENTRE";
        public const string TYPECENTRE = "TYPECENTRE";
        public const string SITE = "SITE";
        public const string MOTIFREJET = "MOTIFREJET";
        public const string ETAPEFONCTIONCHEQUE = "ETAPEFONCTIONCHEQUE";
        public const string HABILITATIONCHEQUE = "HABILITATIONCHEQUE";

        #region Table DIACOMP
        public const string UpdateDiacomp = "SPX_DIACOMP_UPDATE";
        public const string InsertDiacomp = "SPX_DIACOMP_INSERT";
        public const string DeleteDiacomp = "SPX_DIACOMP_DELETE";
        public const string SelectDiacomp = "SPX_DIACOMP_SELECT_ALL";
        public const string SelectDiacompByKey = "SPX_DIACOMP_SELECT_BY_KEY"; // Key = centre + produit + diametre 
        public const string SelectDiacompByRowId = "SPX_DIACOMP_SELECT_BY_ROWID"; // Key = centre + produit + diametre 

        #endregion

        #region Table AG
        public const string SelectAGByCentreRegrou = "SPX_AG_SELECT_BY_CENTRE_REGROU";
        #endregion

        #region Table DAG
        public const string SelectDAGByCentreRegrou = "SPX_DAG_SELECT_BY_CENTRE_REGROU";
        #endregion

        #region Table REGROUFAC
        public const string SelectREGROUFACByCentreRegrou = "SPX_REGROUFAC_SELECT_BY_CENTRE_REGROU";
        #endregion

        #region Table CPROFAC
        public const string SelectCPROFACByCentreRegrou = "SPX_CPROFAC_SELECT_BY_CENTRE_REGROU";
        #endregion

        #region Table CANALISATION
        public const string SelectCANALISATIONByDiametreProduit = "SPX_CANLISATION_SELECT_BY_DIAM_PROD";
        #endregion

        #region Table COMPTDISPO
        public const string SelectCOMPTDISPOByDiametreProduit = "SPX_COMPTDISPO_SELECT_BY_DIAM_PROD";
        #endregion

        #region Table RUES
        public const string UpdateRues = "SPX_RUES_UPDATE";
        public const string InsertRues = "SPX_RUES_INSERT";
        public const string DeleteRues = "SPX_RUES_DELETE";
        public const string SelectRues = "SPX_RUES_SELECT_ALL";
        public const string SelectRuesByKey = "SPX_RUES_SELECT_BY_KEY"; // Key = centre + commune + rue 
        public const string SelectRuesByRowId = "SPX_RUES_SELECT_BY_ROWID"; // Key = ROWID 


        #endregion

        #region Table FRAISTIMB

        public const string UpdateFRAISTIMB = "SPX_FRAISTIMB_UPDATE";
        public const string InsertFRAISTIMB = "SPX_FRAISTIMB_INSERT";
        public const string DeleteFRAISTIMB = "SPX_FRAISTIMB_DELETE";
        public const string SelectFRAISTIMB = "SPX_FRAISTIMB_SELECT_ALL";
        public const string SelectFRAISTIMBByKey = "SPX_FRAISTIMB_SELECT_BY_KEY";

        #endregion

        #region Table TA

        public const string UpdateTa = "SPX_TA_UPDATE";
        public const string InsertTa = "SPX_TA_INSERT";
        public const string DeleteTa = "SPX_TA_DELETE";
        public const string SelectTa = "SPX_TA_SELECT_ALL_By_NUM"; // Key = centre + code + num 
        public const string SelectTaCodeLibelleByNum = "SPX_TA_SELECT_CODE_LIBELLE_By_NUM";
        public const string SelectTaByKey = "SPX_TA_SELECT_BY_KEY";
        public const string SelectTaByRowid = "SPX_TA_SELECT_BY_ROWID";
        public const string SelectTaByCodeProduit = "SPX_TA_LibelleProduit_ByCodeProduit";
        public const string SelectTaAllProduit = "SPX_TA_SelAllProduit";
        public const string SelectTaByCategorieClient = "SPX_TA_SELECT_ALLCATEGORIECLIENT";
        public const string SelectTaLibelleVerifieUtilisationAJUFIN = "SPX_TA_LIBELLE_VERIFIE_UTILISATION_AJUFIN";
        public const string SelectTaCommuneByCentre = "SPX_TA_SELECT_COMMUNE_BY_CENTRE";
        public const string SelectTaCommune = "SPX_TA_SELECT_ALLCOMMUNE";
        public const string SelectTaExploitation = "SPX_TA_SELECT_TA";
        public const string SelectDirecteur = "SPX_Directeur_SelAll";
        public const string SelectTaSelAllFonction = "SPX_TA_SELECT_ALLFONCTION";

        #endregion

        #region Table TA0
        public const string UpdateTa0 = "SPX_TA0_UPDATE";
        public const string InsertTa0 = "SPX_TA0_INSERT";
        public const string DeleteTa0 = "SPX_TA0_DELETE";
        public const string SelectTa0 = "SPX_TA0_SELECT_ALL";
        public const string SelectTa0ByKey = "SPX_TA0_SELECT_BY_KEY"; // Key =  code 
        public const string SelectTa0ByRowid = "SPX_TA0_SELECT_BY_ROWID"; // Key =  Rowid 


        #endregion

        #region Table TACHEDEVIS

        public const string UpdateTACHEDEVIS = "SPX_TACHEDEVIS_UPDATE";
        public const string InsertTACHEDEVIS = "SPX_TACHEDEVIS_INSERT";
        public const string DeleteTACHEDEVIS = "SPX_TACHEDEVIS_DELETE";
        public const string SelectTACHEDEVIS = "SPX_TACHEDEVIS_SELECT_ALL";
        public const string SelectTACHEDEVISByKey = "SPX_TACHEDEVIS_SELECT_BY_KEY";

        #endregion

        #region Table TYPEDEVIS

        public const string UpdateTYPEDEVIS = "spx_TYPEDEVIS_Update";
        public const string InsertTYPEDEVIS = "spx_TYPEDEVIS_Insert";
        public const string DeleteTYPEDEVIS = "spx_TYPEDEVIS_Delete";
        public const string SelectTYPEDEVIS = "spx_TYPEDEVIS_Get_List";
        public const string SelectTYPEDEVISByKey = "SPX_TYPEDEVIS_SELECT_BY_KEY";
        public const string SelectTYPEDEVISByCodeProduit = "spx_TYPEDEVIS_GetByCodeProduit";

        #endregion

        #region Table APPAREILS

        public const string UpdateAPPAREILS = "SPX_APPAREILS_UPDATE";
        public const string InsertAPPAREILS = "SPX_APPAREILS_INSERT";
        public const string DeleteAPPAREILS = "SPX_APPAREILS_DELETE";
        public const string SelectAPPAREILS = "SPX_APPAREILS_SelAll";
        public const string SelectAPPAREILSByKey = "SPX_APPAREILS_SELECT_BY_KEY";

        #endregion

        #region Table CARACTERISTIQUE

        public const string UpdateCARACTERISTIQUE = "SPX_CARACTERISTIQUE_UPDATE";
        public const string InsertCARACTERISTIQUE = "SPX_CARACTERISTIQUE_INSERT";
        public const string DeleteCARACTERISTIQUE = "SPX_CARACTERISTIQUE_DELETE";
        public const string SelectCARACTERISTIQUE = "SPX_CARACTERISTIQUE_SELECT_ALL";
        public const string SelectCARACTERISTIQUEByKey = "SPX_CARACTERISTIQUE_SELECT_BY_KEY";

        #endregion

        #region Table FOURNITURE

        public const string UpdateFOURNITURE = "SPX_FOURNITURE_UPDATE";
        public const string InsertFOURNITURE = "SPX_FOURNITURE_INSERT";
        public const string DeleteFOURNITURE = "SPX_FOURNITURE_DELETE";
        public const string SelectFOURNITURE = "SPX_FOURNITURE_SELECT_ALL";
        public const string SelectFOURNITUREByKey = "SPX_FOURNITURE_SELECT_BY_KEY";

        #endregion

        #region Table ETAPEDEVIS

        public const string UpdateETAPEDEVIS = "spx_ETAPEDEVIS_Update";
        public const string InsertETAPEDEVIS = "spx_ETAPEDEVIS_Insert";
        public const string DeleteETAPEDEVIS = "spx_ETAPEDEVIS_Delete";
        public const string SelectETAPEDEVIS = "spx_ETAPEDEVIS_Get_List";
        public const string SelectETAPEDEVISByKey = "SPX_ETAPEDEVIS_SELECT_BY_KEY";

        #endregion

        #region Table INIT

        public const string UpdateINIT = "SPX_INIT_UPDATE";
        public const string InsertINIT = "SPX_INIT_INSERT";
        public const string DeleteINIT = "SPX_INIT_DELETE";
        public const string SelectINIT = "SPX_INIT_SELECT_ALL";
        public const string ExistsINIT = "SPX_INIT_EXISTS";
        public const string SelectByNtable = "SPX_INIT_SELECT_COLUMNS_BY_NTABLE";
        public const string SelectINITByKey = "SPX_INIT_SELECT_BY_KEY";

        #endregion

        #region Table REGROU

        public const string UpdateREGROU = "SPX_REGROU_UPDATE";
        public const string InsertREGROU = "SPX_REGROU_INSERT";
        public const string DeleteREGROU = "SPX_REGROU_DELETE";
        public const string SelectREGROU = "SPX_REGROU_SELECT_ALL";
        public const string SelectREGROUByKey = "SPX_REGROU_SELECT_BY_KEY";

        #endregion

        #region Table TCOMPT

        public const string UpdateTCOMPT = "SPX_TCOMPT_UPDATE";
        public const string InsertTCOMPT = "SPX_TCOMPT_INSERT";
        public const string DeleteTCOMPT = "SPX_TCOMPT_DELETE";
        public const string SelectTCOMPT = "SPX_TCOMPT_SELECT_ALL";
        public const string SelectTCOMPTByKey = "SPX_TCOMPT_SELECT_BY_KEY";
        public const string SelectTCOMPTByRowId = "SPX_TCOMPT_SELECT_BY_ROWID";

        #endregion

        #region Table PUISSANCE

        public const string UpdatePUISSANCE = "SPX_PUISSANCE_UPDATE";
        public const string InsertPUISSANCE = "SPX_PUISSANCE_INSERT";
        public const string DeletePUISSANCE = "SPX_PUISSANCE_DELETE";
        public const string SelectPUISSANCE = "SPX_PUISSANCE_SELECT_ALL";
        public const string SelectPUISSANCEByKey = "SPX_PUISSANCE_SELECT_BY_KEY";
        public const string SelectPUISSANCEByRowId = "SPX_PUISSANCE_SELECT_BY_ROWID";

        #endregion

        #region Table TARIF

        public const string UpdateTARIF = "SPX_TARIF_UPDATE";
        public const string InsertTARIF = "SPX_TARIF_INSERT";
        public const string DeleteTARIF = "SPX_TARIF_DELETE";
        public const string SelectTARIF = "SPX_TARIF_SELECT_ALL";
        public const string SelectTARIFByKey = "SPX_TARIF_SELECT_BY_KEY";
        public const string SelectTARIFByRowId = "SPX_TARIF_SELECT_BY_ROWID";
        public const string SelectTARIFCODE_LIBELLE = "SPX_TARIF_SELECT_ALLCODETARIF";

        #endregion

        #region Table FORFAIT

        public const string UpdateFORFAIT = "SPX_FORFAIT_UPDATE";
        public const string InsertFORFAIT = "SPX_FORFAIT_INSERT";
        public const string DeleteFORFAIT = "SPX_FORFAIT_DELETE";
        public const string SelectFORFAIT = "SPX_FORFAIT_SELECT_ALL";
        public const string SelectFORFAITByKey = "SPX_FORFAIT_SELECT_BY_KEY";
        public const string SelectFORFAITByRowId = "SPX_FORFAIT_SELECT_BY_ROWID";

        #endregion

        #region Table BANQUE

        public const string UpdateBANQUE = "SPX_PARAM_BANQUE_UPDATE";
        public const string InsertBANQUE = "SPX_PARAM_BANQUE_INSERER";
        public const string DeleteBANQUE = "SPX_PARAM_BANQUE_SUPPRIMER";
        public const string SelectBANQUE = "SPX_PARAM_BANQUE_RETOURNE";
        public const string SelectBANQUEByKey = "SPX_PARAM_BANQUE_RETOURNEByBANQUEGUICHET";

        #endregion

        #region Table CTAX

        public const string UpdateCTAX = "SPX_CTAX_UPDATE";
        public const string SelectCTAX_LIBELLE = "SPX_CTAX_SELECT_CTAX_LIBELLE";
        public const string InsertCTAX = "SPX_CTAX_INSERT";
        public const string DeleteCTAX = "SPX_CTAX_DELETE";
        public const string SelectCTAX = "SPX_CTAX_SELECT_ALL";
        public const string SelectCTAXByKey = "SPX_CTAX_SELECT_BY_KEY";

        #endregion

        #region Table CASIND

        public const string UpdateCASIND = "SPX_CASIND_UPDATE";
        public const string InsertCASIND = "SPX_CASIND_INSERT";
        public const string DeleteCASIND = "SPX_CASIND_DELETE";
        public const string SelectCASIND = "SPX_CASIND_SELECT_ALL";
        public const string SelectCASINDByKey = "SPX_CASIND_SELECT_BY_KEY";
        public const string SelectCASINDCasEcrasable = "SPX_CASIND_SELECT_CASECRASABLE";
        public const string SelectCASIND_CASGENS = "SPX_CASIND_SELECT_CASGEN_BY_CAS";
        public const string SelectCASIND_LIBELLE_BY_CAS = "SPX_CASIND_SELECT_LIBELLE_BY_CAS";


        #endregion

        #region Table MATRICULE

        public const string UpdateMATRICULE = "SPX_MATRICULE_UPDATE";
        public const string InsertMATRICULE = "SPX_MATRICULE_INSERT";
        public const string DeleteMATRICULE = "SPX_MATRICULE_DELETE";
        public const string SelectMATRICULE = "SPX_MATRICULE_SELECT_ALL";
        public const string SelectMATRICULEByKey = "SPX_MATRICULE_SELECT_BY_KEY";

        #endregion

        #region Table MULTIMONNAIES

        public const string UpdateMULTIMONNAIES = "SPX_MULTIMONNAIES_UPDATE";
        public const string SelectMonnaieType = "SPX_MULTIMONNAIES_SelectMonnaieType";
        public const string InsertMULTIMONNAIES = "SPX_MULTIMONNAIES_INSERT";
        public const string DeleteMULTIMONNAIES = "SPX_MULTIMONNAIES_DELETE";
        public const string SelectMULTIMONNAIES = "SPX_MULTIMONNAIES_SELECT_ALL";
        public const string SelectMULTIMONNAIESByKey = "SPX_MULTIMONNAIES_SELECT_BY_KEY";

        #endregion

        #region Table SPESITE

        public const string UpdateSPESITE = "SPX_SPESITE_UPDATE";
        public const string InsertSPESITE = "SPX_SPESITE_INSERT";
        public const string DeleteSPESITE = "SPX_SPESITE_DELETE";
        public const string SelectSPESITE = "SPX_SPESITE_SELECT_ALL";
        public const string SelectSPESITEByKey = "SPX_SPESITE_SELECT_BY_KEY";

        #endregion

        #region Table MODEREG

        public const string UpdateMODEREG = "SPX_MODEREG_UPDATE";
        public const string InsertMODEREG = "SPX_MODEREG_INSERT";
        public const string DeleteMODEREG = "SPX_MODEREG_DELETE";
        public const string SelectMODEREG = "SPX_MODEREG_SELECT_ALL";
        public const string SelectMODEREGByKey = "SPX_MODEREG_SELECT_BY_KEY";
        public const string SelectLIBELLEMODEREG = "SPX_MODEREG_SELECTALLMODEREG";
        #endregion

        #region Table LCLIENT
        public const string SelectLCLIENTByModereg = "SPX_LCLIENT_SELECT_BY_MODEREG";
        #endregion

        #region Table LCLIENTB
        public const string SelectLCLIENTBByModereg = "SPX_LCLIENTB_SELECT_BY_MODEREG";
        #endregion

        #region Table LCLIENTT
        public const string SelectLCLIENTTByModereg = "SPX_LCLIENTT_SELECT_BY_MODEREG";
        #endregion

        #region Table TRANSCAISB
        public const string SelectTRANSCAISBByModereg = "SPX_TRANSCAISB_SELECT_BY_MODEREG";
        #endregion

        #region Table DETAILLOT
        public const string SelectDETAILLOTByModereg = "SPX_DETAILLOT_SELECT_BY_MODEREG";
        #endregion

        #region Table CONTROLELIGNE
        public const string SelectCONTROLELIGNEByModereg = "SPX_CONTROLELIGNE_SELECT_BY_MODEREG";
        #endregion

        #region Table TRANSCAISSE
        public const string SelectTRANSCAISSEByModereg = "SPX_TRANSCAISSE_SELECT_BY_MODEREG";
        #endregion

        #region Table FRAISHP

        public const string UpdateFRAISHP = "SPX_FRAISHP_UPDATE";
        public const string InsertFRAISHP = "SPX_FRAISHP_INSERT";
        public const string DeleteFRAISHP = "SPX_FRAISHP_DELETE";
        public const string SelectFRAISHP = "SPX_FRAISHP_SELECT_ALL";
        public const string SelectFRAISHPByKey = "SPX_FRAISHP_SELECT_BY_KEY";

        #endregion

        #region Table TDEM

        public const string UpdateTDEM = "SPX_TDEM_UPDATE";
        public const string InsertTDEM = "SPX_TDEM_INSERT";
        public const string DeleteTDEM = "SPX_TDEM_DELETE";
        public const string SelectTDEM = "SPX_TDEM_SELECT_ALL";
        public const string SelectTDEMByKey = "SPX_TDEM_SELECT_BY_KEY";
        public const string SelectTDEMLIBELLE = "SPX_TDEM_SELECT_TDEM_LIBELLE";

        #endregion

        #region Table DIRECTEUR

        public const string UpdateDIRECTEUR = "spx_DIRECTEUR_Update";
        public const string InsertDIRECTEUR = "spx_DIRECTEUR_Insert";
        public const string DeleteDIRECTEUR = "spx_DIRECTEUR_Delete";
        public const string SelectDIRECTEUR = "spx_DIRECTEUR_Get_List";
        public const string SelectDIRECTEURByCENTRESITE = "spx_DIRECTEUR_GetByCENTRESITE";
        public const string SelectDIRECTEURCentre = "SPX_Directeur_SelAll";

        #endregion   

        #region Table DEMCOUT

        public const string UpdateDEMCOUT = "SPX_DEMCOUT_UPDATE";
        public const string InsertDEMCOUT = "SPX_DEMCOUT_INSERT";
        public const string DeleteDEMCOUT = "SPX_DEMCOUT_DELETE";
        public const string SelectDEMCOUT = "SPX_DEMCOUT_SELECT_ALL";
        public const string SelectDEMCOUTByKey = "SPX_DEMCOUT_SELECT_BY_KEY";

        #endregion

        #region Table COPER

        public const string UpdateCOPER = "SPX_COPER_UPDATE";
        public const string InsertCOPER = "SPX_COPER_INSERT";
        public const string DeleteCOPER = "SPX_COPER_DELETE";
        public const string SelectCOPER = "SPX_COPER_SELECT_ALL";
        public const string SelectCOPERByKey = "SPX_COPER_SELECT_BY_KEY";
        public const string SelectCOPER_LIBELLE = "SPX_COPER_SELECT_COPER_LIBELLE";
        public const string SelectCOPER_LIBELLE100 = "SPX_COPER_SELECT_COPER_LIBELLE_SUP100";

        #endregion

        #region Table NATURE

        public const string UpdateNATURE = "SPX_NATURE_UPDATE";
        public const string InsertNATURE = "SPX_NATURE_INSERT";
        public const string DeleteNATURE = "SPX_NATURE_DELETE";
        public const string SelectNATURE = "SPX_NATURE_SELECT_ALL";
        public const string SelectNATUREByKey = "SPX_NATURE_SELECT_BY_KEY";
        public const string SelectLIBELLE_NATURE = "SPX_NATURE_SELECTALLNATURE";
        #endregion

        #region Table COPEROD

        public const string UpdateCOPEROD = "SPX_COPEROD_UPDATE";
        public const string InsertCOPEROD = "SPX_COPEROD_INSERT";
        public const string DeleteCOPEROD = "SPX_COPEROD_DELETE";
        public const string SelectCOPEROD = "SPX_COPEROD_SELECT_ALL";
        public const string SelectCOPERODByKey = "SPX_COPEROD_SELECT_BY_KEY";

        #endregion

        #region Table REDEVANCE

        public const string UpdateREDEVANCE = "SPX_REDEVANCE_UPDATE";
        public const string InsertREDEVANCE = "SPX_REDEVANCE_INSERT";
        public const string DeleteREDEVANCE = "SPX_REDEVANCE_DELETE";
        public const string SelectREDEVANCE = "SPX_REDEVANCE_SELECT_ALL";
        public const string SelectREDEVANCEByKey = "SPX_REDEVANCE_SELECT_BY_KEY";

        #endregion

        #region Table NATGEN

        public const string UpdateNATGEN = "SPX_NATGEN_UPDATE";
        public const string InsertNATGEN = "SPX_NATGEN_INSERT";
        public const string DeleteNATGEN = "SPX_NATGEN_DELETE";
        public const string SelectNATGEN = "SPX_NATGEN_SELECT_ALL";
        public const string SelectNATGENByKey = "SPX_NATGEN_SELECT_BY_KEY";

        #endregion

        #region Table REGCLI

        public const string UpdateREGCLI = "SPX_PARAM_REGCLI_UPDATE";
        public const string InsertREGCLI = "SPX_PARAM_REGCLI_INSERER";
        public const string DeleteREGCLI = "SPX_PARAM_REGCLI_SUPPRIMER";
        public const string SelectREGCLI = "SPX_PARAM_REGCLI_RETOURNE";
        public const string SelectREGCLIByKey = "SPX_PARAM_REGCLI_RETOURNEByREGCLI";

        #endregion

        #region Table GEOGES

        public const string UpdateGEOGES = "SPX_GEOGES_UPDATE";
        public const string InsertGEOGES = "SPX_GEOGES_INSERT";
        public const string DeleteGEOGES = "SPX_GEOGES_DELETE";
        public const string SelectGEOGES = "SPX_GEOGES_SELECT_ALL";
        public const string SelectGEOGESByKey = "SPX_GEOGES_SELECT_BY_KEY";

        #endregion

        #region Table QUARTIER

        public const string UpdateQUARTIER = "SPX_QUARTIER_UPDATE";
        public const string InsertQUARTIER = "SPX_QUARTIER_INSERT";
        public const string DeleteQUARTIER = "SPX_QUARTIER_DELETE";
        public const string SelectQUARTIER = "SPX_QUARTIER_SELECT_ALL";
        public const string SelectQUARTIERByKey = "SPX_QUARTIER_SELECT_BY_KEY";
        public const string SelectQUARTIERByCentreAndCommune = "SPX_QUARTIER_SELECT_QUARTIER_BY_CENTRE_AND_COMMUNE";

        #endregion

        #region Table MESSAGE

        public const string UpdateMESSAGE = "SPX_MESSAGE_UPDATE";
        public const string InsertMESSAGE = "SPX_MESSAGE_INSERT";
        public const string DeleteMESSAGE = "SPX_MESSAGE_DELETE";
        public const string SelectMESSAGE = "SPX_MESSAGE_SELECT_ALL";
        public const string SelectMESSAGEByKey = "SPX_MESSAGE_SELECT_BY_KEY";

        #endregion

        #region Table DOMBANC

        public const string UpdateDOMBANC = "SPX_PARAM_DOMBANC_UPDATE";
        public const string InsertDOMBANC = "SPX_PARAM_DOMBANC_INSERER";
        public const string DeleteDOMBANC = "SPX_PARAM_DOMBANC_SUPPRIMER";
        public const string SelectDOMBANC = "SPX_PARAM_DOMBANC_RETOURNE";
        public const string SelectDOMBANCByKey = "SPX_PARAM_DOMBANC_RETOURNEByBANQUEGUICHET";

        #endregion

        #region Table HabilitationCheque

        public const string UpdateHabilitationCheque = "SPX_HabilitationCheque_UPDATE";
        public const string InsertHabilitationCheque = "SPX_HabilitationCheque_INSERT";
        public const string DeleteHabilitationCheque = "SPX_HabilitationCheque_DELETE";
        public const string SelectHabilitationCheque = "SPX_HabilitationCheque_SELECT_ALL";
        public const string SelectHabilitationChequeByKey = "SPX_HabilitationCheque_SELECT_BY_KEY";

        #endregion

        #region Table ARRETE

        public const string UpdateARRETE = "SPX_ARRETE_UPDATE";
        public const string InsertARRETE = "SPX_ARRETE_INSERT";
        public const string DeleteARRETE = "SPX_ARRETE_DELETE";
        public const string SelectARRETE = "SPX_ARRETE_SELECT_ALL";
        public const string SelectARRETEByKey = "SPX_ARRETE_SELECT_BY_KEY";

        #endregion

        #region Table REGEXO

        public const string UpdateREGEXO = "SPX_PARAM_REGEXO_UPDATE";
        public const string InsertREGEXO = "SPX_PARAM_REGEXO_INSERER";
        public const string DeleteREGEXO = "SPX_PARAM_REGEXO_SUPPRIMER";
        public const string SelectREGEXO = "SPX_PARAM_REGEXO_RETOURNE";
        public const string SelectREGEXOByKey = "SPX_PARAM_REGEXO_RETOURNEByCENTREPRODUITREGCLI";

        #endregion

        #region Table FRAISCONTENTIEUX

        public const string UpdateFRAISCONTENTIEUX = "SPX_FRAISCONTENTIEUX_UPDATE";
        public const string InsertFRAISCONTENTIEUX = "SPX_FRAISCONTENTIEUX_INSERT";
        public const string DeleteFRAISCONTENTIEUX = "SPX_FRAISCONTENTIEUX_DELETE";
        public const string SelectFRAISCONTENTIEUX = "SPX_FRAISCONTENTIEUX_SELECT_ALL";
        public const string SelectFRAISCONTENTIEUXByKey = "SPX_FRAISCONTENTIEUX_SELECT_BY_KEY";

        #endregion

        #region Table LIBRUBACTION

        public const string UpdateLIBRUBACTION = "SPX_LIBRUBACTION_UPDATE";
        public const string InsertLIBRUBACTION = "SPX_LIBRUBACTION_INSERT";
        public const string DeleteLIBRUBACTION = "SPX_LIBRUBACTION_DELETE";
        public const string SelectLIBRUBACTION = "SPX_LIBRUBACTION_SELECT_ALL";
        public const string SelectLIBRUBACTIONByKey = "SPX_LIBRUBACTION_SELECT_BY_KEY";

        #endregion

        #region Table SECURITEMATRICULE

        public const string UpdateSECURITEMATRICULE = "SPX_SECURITEMATRICULE_UPDATE";
        public const string InsertSECURITEMATRICULE = "SPX_SECURITEMATRICULE_INSERT";
        public const string DeleteSECURITEMATRICULE = "SPX_SECURITEMATRICULE_DELETE";
        public const string SelectSECURITEMATRICULE = "SPX_SECURITEMATRICULE_SELECT_ALL";
        public const string SelectSECURITEMATRICULEByKey = "SPX_SECURITEMATRICULE_SELECT_BY_KEY";

        #endregion

        #region Table MONNAIE

        public const string UpdateMONNAIE = "SPX_MONNAIE_UPDATE";
        public const string InsertMONNAIE = "SPX_MONNAIE_INSERT";
        public const string DeleteMONNAIE = "SPX_MONNAIE_DELETE";
        public const string SelectMONNAIE = "SPX_MONNAIE_SELECT_ALL";
        public const string SelectMONNAIEByKey = "SPX_MONNAIE_SELECT_BY_KEY";

        #endregion

        #region Table TAXCOMP

        public const string UpdateTAXCOMP = "SPX_TAXCOMP_UPDATE";
        public const string InsertTAXCOMP = "SPX_TAXCOMP_INSERT";
        public const string DeleteTAXCOMP = "SPX_TAXCOMP_DELETE";
        public const string SelectTAXCOMP = "SPX_TAXCOMP_SELECT_ALL";
        public const string SelectTAXCOMPByKey = "SPX_TAXCOMP_SELECT_BY_KEY";

        #endregion

        #region Table DEFPARAMABON

        public const string UpdateDEFPARAMABON = "SPX_DEFPARAMABON_UPDATE";
        public const string InsertDEFPARAMABON = "SPX_DEFPARAMABON_INSERT";
        public const string DeleteDEFPARAMABON = "SPX_DEFPARAMABON_DELETE";
        public const string SelectDEFPARAMABON = "SPX_DEFPARAMABON_SELECT_ALL";
        public const string SelectDEFPARAMABONByKey = "SPX_DEFPARAMABON_SELECT_BY_KEY";

        #endregion

        #region Table ETAPEFONCTION

        public const string UpdateETAPEFONCTION = "SPX_ETAPEFONCTION_UPDATE";
        public const string InsertETAPEFONCTION = "SPX_ETAPEFONCTION_INSERT";
        public const string DeleteETAPEFONCTION = "SPX_ETAPEFONCTION_DELETE";
        public const string SelectETAPEFONCTION = "SPX_ETAPEFONCTION_SELECT_ALL";
        public const string SelectETAPEFONCTIONByKey = "SPX_ETAPEFONCTION_SELECT_BY_KEY";

        #endregion

        #region Table ETAPERECLAMATION

        public const string UpdateETAPERECLAMATION = "SPX_ETAPERECLAMATION_UPDATE";
        public const string InsertETAPERECLAMATION = "SPX_ETAPERECLAMATION_INSERT";
        public const string DeleteETAPERECLAMATION = "SPX_ETAPERECLAMATION_DELETE";
        public const string SelectETAPERECLAMATION = "SPX_ETAPERECLAMATION_SELECT_ALL";
        public const string SelectETAPERECLAMATIONByKey = "SPX_ETAPERECLAMATION_SELECT_BY_KEY";

        #endregion

        #region Table TYPERECLAMATION

        public const string UpdateTYPERECLAMATION = "SPX_TYPERECLAMATION_UPDATE";
        public const string InsertTYPERECLAMATION = "SPX_TYPERECLAMATION_INSERT";
        public const string DeleteTYPERECLAMATION = "SPX_TYPERECLAMATION_DELETE";
        public const string SelectTYPERECLAMATION = "SPX_TYPERECLAMATION_SELECT_ALL";
        public const string SelectTYPERECLAMATIONByKey = "SPX_TYPERECLAMATION_SELECT_BY_KEY";

        #endregion

        #region Table CENTREENCAISSABLE

        public const string UpdateCENTREENCAISSABLE = "SPX_CENTREENCAISSABLE_UPDATE";
        public const string InsertCENTREENCAISSABLE = "SPX_CENTREENCAISSABLE_INSERT";
        public const string DeleteCENTREENCAISSABLE = "SPX_CENTREENCAISSABLE_DELETE";
        public const string SelectCENTREENCAISSABLE = "SPX_CENTREENCAISSABLE_SELECT_ALL";
        public const string SelectCENTREENCAISSABLEByKey = "SPX_CENTREENCAISSABLE_SELECT_BY_KEY";

        #endregion

        #region Table TYPELOT

        public const string UpdateTYPELOT = "SPX_TYPELOT_UPDATE";
        public const string InsertTYPELOT = "SPX_TYPELOT_INSERT";
        public const string DeleteTYPELOT = "SPX_TYPELOT_DELETE";
        public const string SelectTYPELOT = "SPX_TYPELOT_SELECT_ALL";
        public const string SelectTYPELOTByKey = "SPX_TYPELOT_SELECT_BY_KEY";

        #endregion

        #region Table CONTROLELIGNE

        public const string UpdateCONTROLELIGNE = "SPX_CONTROLELIGNE_UPDATE";
        public const string InsertCONTROLELIGNE = "SPX_CONTROLELIGNE_INSERT";
        public const string DeleteCONTROLELIGNE = "SPX_CONTROLELIGNE_DELETE";
        public const string SelectCONTROLELIGNE = "SPX_CONTROLELIGNE_SELECT_ALL";
        public const string SelectCONTROLELIGNEByKey = "SPX_CONTROLELIGNE_SELECT_BY_KEY";

        #endregion

        #region Table CONTROLESECONDAIRE

        public const string UpdateCONTROLESECONDAIRE = "SPX_CONTROLESECONDAIRE_UPDATE";
        public const string InsertCONTROLESECONDAIRE = "SPX_CONTROLESECONDAIRE_INSERT";
        public const string DeleteCONTROLESECONDAIRE = "SPX_CONTROLESECONDAIRE_DELETE";
        public const string SelectCONTROLESECONDAIRE = "SPX_CONTROLESECONDAIRE_SELECT_ALL";
        public const string SelectCONTROLESECONDAIREByKey = "SPX_CONTROLESECONDAIRE_SELECT_BY_KEY";

        #endregion

        #region Table CONTROLEPIECE

        public const string UpdateCONTROLEPIECE = "SPX_CONTROLEPIECE_UPDATE";
        public const string InsertCONTROLEPIECE = "SPX_CONTROLEPIECE_INSERT";
        public const string DeleteCONTROLEPIECE = "SPX_CONTROLEPIECE_DELETE";
        public const string SelectCONTROLEPIECE = "SPX_CONTROLEPIECE_SELECT_ALL";
        public const string SelectCONTROLEPIECEByKey = "SPX_CONTROLEPIECE_SELECT_BY_KEY";

        #endregion

        #region Table COUTPUISSANCE

        public const string UpdateCOUTPUISSANCE = "SPX_COUTPUISSANCE_UPDATE";
        public const string InsertCOUTPUISSANCE = "SPX_COUTPUISSANCE_INSERT";
        public const string DeleteCOUTPUISSANCE = "SPX_COUTPUISSANCE_DELETE";
        public const string SelectCOUTPUISSANCE = "SPX_COUTPUISSANCE_SELECT_ALL";
        public const string SelectCOUTPUISSANCEByKey = "SPX_COUTPUISSANCE_SELECT_BY_KEY";

        #endregion

        #region Table TYPEBRANCHEMENT

        public const string UpdateTYPEBRANCHEMENT = "SPX_TYPEBRANCHEMENT_UPDATE";
        public const string InsertTYPEBRANCHEMENT = "SPX_TYPEBRANCHEMENT_INSERT";
        public const string DeleteTYPEBRANCHEMENT = "SPX_TYPEBRANCHEMENT_DELETE";
        public const string SelectTYPEBRANCHEMENT = "SPX_TYPEBRANCHEMENT_SELECT_ALL";
        public const string SelectTYPEBRANCHEMENTByKey = "SPX_TYPEBRANCHEMENT_SELECT_BY_KEY";

        #endregion

        #region Table PUISPERTES

        public const string UpdatePUISPERTES = "SPX_PUISPERTES_UPDATE";
        public const string InsertPUISPERTES = "SPX_PUISPERTES_INSERT";
        public const string DeletePUISPERTES = "SPX_PUISPERTES_DELETE";
        public const string SelectPUISPERTES = "SPX_PUISPERTES_SELECT_ALL";
        public const string SelectPUISPERTESByKey = "SPX_PUISPERTES_SELECT_BY_KEY";

        #endregion

        #region Table COMPTAGE

        public const string UpdateCOMPTAGE = "SPX_COMPTAGE_UPDATE";
        public const string InsertCOMPTAGE = "SPX_COMPTAGE_INSERT";
        public const string DeleteCOMPTAGE = "SPX_COMPTAGE_DELETE";
        public const string SelectCOMPTAGE = "SPX_COMPTAGE_SELECT_ALL";
        public const string SelectCOMPTAGEByKey = "SPX_COMPTAGE_SELECT_BY_KEY";

        #endregion

        #region Table IMPRIM

        public const string UpdateIMPRIM = "SPX_IMPRIM_UPDATE";
        public const string InsertIMPRIM = "SPX_IMPRIM_INSERT";
        public const string DeleteIMPRIM = "SPX_IMPRIM_DELETE";
        public const string SelectIMPRIM = "SPX_IMPRIM_SELECT_ALL";
        public const string SelectIMPRIMByKey = "SPX_IMPRIM_SELECT_BY_KEY";

        #endregion

        #region Table SCHEMAS

        public const string UpdateSCHEMAS = "SPX_SCHEMAS_UPDATE";
        public const string InsertSCHEMAS = "SPX_SCHEMAS_INSERT";
        public const string DeleteSCHEMAS = "SPX_SCHEMAS_DELETE";
        public const string SelectSCHEMAS = "SPX_SCHEMAS_SELECT_ALL";
        public const string SelectSCHEMASByKey = "SPX_SCHEMAS_SELECT_BY_KEY";

        #endregion

        #region Table AJUFIN

        public const string UpdateAJUFIN = "SPX_AJUFIN_UPDATE";
        public const string InsertAJUFIN = "SPX_AJUFIN_INSERT";
        public const string DeleteAJUFIN = "SPX_AJUFIN_DELETE";
        public const string SelectAJUFIN = "SPX_AJUFIN_SELECT_ALL";
        public const string SelectAJUFINByKey = "SPX_AJUFIN_SELECT_BY_KEY";

        #endregion

        #region Table PARAMABONULILISE

        public const string UpdatePARAMABONUTILISE = "SPX_PARAM_PARAMABONUTILISE_UPDATE";
        public const string InsertPARAMABONUTILISE = "SPX_PARAM_PARAMABONUTILISE_INSERER";
        public const string DeletePARAMABONUTILISE = "SPX_PARAM_PARAMABONUTILISE_SUPPRIMER";
        public const string SelectPARAMABONUTILISE = "SPX_PARAM_PARAMABONUTILISE_RETOURNE";
        public const string SelectPARAMABONUTILISEByKey = "SPX_PARAM_PARAMABONUTILISE_RETOURNEByCENTRECLECALPRODUITPARAM";

        #endregion

        #region Table SECTEUR

        public const string UpdateSECTEUR = "SPX_SECTEUR_UPDATE";
        public const string InsertSECTEUR = "SPX_SECTEUR_INSERT";
        public const string DeleteSECTEUR = "SPX_SECTEUR_DELETE";
        public const string SelectSECTEUR = "SPX_SECTEUR_SELECT_ALL";
        public const string SelectSECTEURByKey = "SPX_SECTEUR_SELECT_BY_KEY";

        #endregion

        #region Table TRANSFOCOMPTAGE

        public const string UpdateTRANSFOCOMPTAGE = "SPX_TRANSFOCOMPTAGE_UPDATE";
        public const string InsertTRANSFOCOMPTAGE = "SPX_TRANSFOCOMPTAGE_INSERT";
        public const string DeleteTRANSFOCOMPTAGE = "SPX_TRANSFOCOMPTAGE_DELETE";
        public const string SelectTRANSFOCOMPTAGE = "SPX_TRANSFOCOMPTAGE_SELECT_ALL";
        public const string SelectTRANSFOCOMPTAGEByKey = "SPX_TRANSFOCOMPTAGE_SELECT_BY_KEY";

        #endregion

        #region Table CODECONTROLE

        public const string UpdateCODECONTROLE = "SPX_CODECONTROLE_UPDATE";
        public const string InsertCODECONTROLE = "SPX_CODECONTROLE_INSERT";
        public const string DeleteCODECONTROLE = "SPX_CODECONTROLE_DELETE";
        public const string SelectCODECONTROLE = "SPX_CODECONTROLE_SELECT_ALL";
        public const string SelectCODECONTROLEByKey = "SPX_CODECONTROLE_SELECT_BY_KEY";

        #endregion

        #region Table CODECONTROLE

        public const string UpdateORIGINELOT = "SPX_ORIGINELOT_UPDATE";
        public const string InsertORIGINELOT = "SPX_ORIGINELOT_INSERT";
        public const string DeleteORIGINELOT = "SPX_ORIGINELOT_DELETE";
        public const string SelectORIGINELOT = "SPX_ORIGINELOT_SELECT_ALL";
        public const string SelectORIGINELOTByKey = "SPX_ORIGINELOT_SELECT_BY_KEY";

        #endregion

        #region Table PRESTATAIRE

        public const string UpdatePRESTATAIRE = "SPX_PRESTATAIRE_UPDATE";
        public const string InsertPRESTATAIRE = "SPX_PRESTATAIRE_INSERT";
        public const string DeletePRESTATAIRE = "SPX_PRESTATAIRE_DELETE";
        public const string SelectPRESTATAIRE = "SPX_PRESTATAIRE_SelAll";
        public const string SelectPRESTATAIREByKey = "SPX_PRESTATAIRE_SELECT_BY_KEY";

        #endregion

        #region Table STATUTRECLAMATION

        public const string UpdateSTATUTRECLAMATION = "SPX_STATUTRECLAMATION_UPDATE";
        public const string InsertSTATUTRECLAMATION = "SPX_STATUTRECLAMATION_INSERT";
        public const string DeleteSTATUTRECLAMATION = "SPX_STATUTRECLAMATION_DELETE";
        public const string SelectSTATUTRECLAMATION = "SPX_STATUTRECLAMATION_SELECT_ALL";
        public const string SelectSTATUTRECLAMATIONByKey = "SPX_STATUTRECLAMATION_SELECT_BY_KEY";

        #endregion

        #region Table GROUPERECLAMATION

        public const string UpdateGROUPERECLAMATION = "SPX_GROUPERECLAMATION_UPDATE";
        public const string InsertGROUPERECLAMATION = "SPX_GROUPERECLAMATION_INSERT";
        public const string DeleteGROUPERECLAMATION = "SPX_GROUPERECLAMATION_DELETE";
        public const string SelectGROUPERECLAMATION = "SPX_GROUPERECLAMATION_SELECT_ALL";
        public const string SelectGROUPERECLAMATIONByKey = "SPX_GROUPERECLAMATION_SELECT_BY_KEY";

        #endregion

       

        #region Table FONCTION

        public const string UpdateFONCTION = "spx_Fonction_Update";
        public const string InsertFONCTION = "spx_Fonction_Insert";
        public const string DeleteFONCTION = "spx_Fonction_Delete";
        public const string SelectFONCTION = "spx_Fonction_Get_List";
        public const string SelectFONCTIONByIdFiliere = "spx_Fonction_GetByIdFiliere";
        public const string SelectFONCTIONById = "spx_Fonction_GetById";

        #endregion

        #region Table FILIERE

        public const string UpdateFILIERE = "spx_Filiere_Update";
        public const string InsertFILIERE = "spx_Filiere_Insert";
        public const string DeleteFILIERE = "spx_Filiere_Delete";
        public const string SelectFILIERE = "spx_col_Filiere_Get_List";
        public const string SelectFILIEREByIdFiliere = "spx_Fonction_GetByIdFiliere";
        public const string SelectFILIEREById = "spx_Filiere_GetById";

        #endregion

        #region Table CENTRE

        public const string UpdateCENTRE = "SPX_PARAM_CENTRE_UPDATE";
        public const string InsertCENTRE = "SPX_PARAM_CENTRE_INSERER";
        public const string DeleteCENTRE = "SPX_PARAM_CENTRE_SUPPRIMER";
        public const string SelectCENTRE = "SPX_PARAM_CENTRE_RETOURNE";
        public const string SelectCENTREByCodeCentre = "SPX_PARAM_CENTRE_RETOURNEByCodeCentre";
        public const string SelectCENTREBySITEFromDIRECTEUR = "spx_Centre_GetBySITEFromDIRECTEUR";

        #endregion

        #region Table SITE

        public const string UpdateSITE = "SPX_PARAM_SITE_UPDATE";
        public const string InsertSITE = "SPX_PARAM_SITE_INSERER";
        public const string DeleteSITE = "SPX_PARAM_SITE_SUPPRIMER";
        public const string SelectSITE = "SPX_PARAM_SITE_RETOURNE";
        public const string SelectSITEByCodeSite = "SPX_PARAM_SITE_RETOURNEByCODESITE";
        public const string SelectSITEGetByCENTREFromDIRECTEUR = "SPX_PARAM_SITE_RETOURNEByCENTREFromDIRECTEUR";

        #endregion

        #region Table MOTIFREJET

        public const string UpdateMOTIFREJET = "spx_MotifRejet_Update";
        public const string InsertMOTIFREJET = "spx_MotifRejet_Insert";
        public const string DeleteMOTIFREJET = "spx_MotifRejet_Delete";
        public const string SelectMOTIFREJET = "spx_col_MotifRejet_Get_List";
        public const string SelectMOTIFREJETByCodeRejet = "spx_MotifRejet_GetByCodeRejet";

        #endregion

        #region Table ETAPEFONCTIONCHEQUE

        public const string UpdateETAPEFONCTIONCHEQUE = "spx_EtapeFonctionCheque_Update";
        public const string InsertETAPEFONCTIONCHEQUE = "spx_EtapeFonctionCheque_Insert";
        public const string DeleteETAPEFONCTIONCHEQUE = "spx_EtapeFonctionCheque_Delete";
        public const string SelectETAPEFONCTIONCHEQUE = "spx_col_EtapeFonctionCheque_Get_List";
        public const string SelectETAPEFONCTIONCHEQUEByIdEtapeIdEtapeFonction = "spx_EtapeFonctionCheque_GetByIdEtapeIdFonction";
        public const string SelectETAPECHEQUE = "spx_col_EtapeCheque_Get_List";

        #endregion

        #region Table TYPECENTRE

        public const string UpdateTYPECENTRE = "SPX_PARAM_TypeCentre_UPDATE";
        public const string InsertTYPECENTRE = "SPX_PARAM_TypeCentre_INSERER";
        public const string DeleteTYPECENTRE = "SPX_PARAM_TypeCentre_SUPPRIMER";
        public const string SelectTYPECENTRE = "SPX_PARAM_TypeCentre_RETOURNE";
        public const string SelectTYPECENTREByCodeType = "SPX_PARAM_TypeCentre_RETOURNEByCodeType";
        //public const string SelectTYPECENTRE = "SPX_TYPECENTRE_SelAll";

        #endregion

        #region Table BANQUE dans GALADB2

        public const string UpdateBANQUE2 = "spx_BANQUE_Update";
        public const string InsertBANQUE2 = "spx_BANQUE_Insert";
        public const string DeleteBANQUE2 = "spx_BANQUE_Delete";
        public const string SelectBANQUE2 = "SPX_BANQUE_SELECT_ALL";
        public const string SelectBANQUE2ByKey = "SPX_BANQUE_SELECT_BY_KEY";

        #endregion

#region Code ajouter par ATO pour la nouvelle gestion du paramétrage

        #region Table PRODUIT

        public const string UpdatePRODUIT = "SPX_PARAM_PRODUIT_UPDATE";
        public const string InsertPRODUIT = "SPX_PARAM_PRODUIT_INSERER";
        public const string DeletePRODUIT = "SPX_PARAM_PRODUIT_SUPPRIMER";
        public const string SelectPRODUIT = "SPX_PARAM_PRODUIT_RETOURNE";
        public const string SelectPRODUITByKey = "SPX_PARAM_PRODUIT_RETOURNEByCODE";

        #endregion

        #region Table DENOMINATION

        public const string UpdateDENOMINATION = "SPX_PARAM_DENOMINATION_UPDATE";
        public const string InsertDENOMINATION = "SPX_PARAM_DENOMINATION_INSERER";
        public const string DeleteDENOMINATION = "SPX_PARAM_DENOMINATION_SUPPRIMER";
        public const string SelectDENOMINATION = "SPX_PARAM_DENOMINATION_RETOURNE";
        public const string SelectDENOMINATIONByKey = "SPX_PARAM_DENOMINATION_RETOURNEByID";

        #endregion

        #region Table PARAMETRESGENRAUX

        public const string UpdatePARAMETRESGENRAUX = "SPX_PARAM_PARAMETRESGENERAUX_UPDATE";
        public const string InsertPARAMETRESGENRAUX = "SPX_PARAM_PARAMETRESGENERAUX_INSERER";
        public const string DeletePARAMETRESGENRAUX = "SPX_PARAM_PARAMETRESGENERAUX_SUPPRIMER";
        public const string SelectPARAMETRESGENRAUX = "SPX_PARAM_PARAMETRESGENERAUX_RETOURNE";
        public const string SelectPARAMETRESGENRAUXByKey = "SPX_PARAM_PARAMETRESGENERAUX_RETOURNEByCODE";

        #endregion
        
        #region Table PARAMETREEVENEMENT

        public const string UpdatePARAMETREEVENEMENT = "SPX_PARAM_PARAMETREEVENEMENT_UPDATE";
        public const string InsertPARAMETREEVENEMENT = "SPX_PARAM_PARAMETREEVENEMENT_INSERER";
        public const string DeletePARAMETREEVENEMENT = "SPX_PARAM_PARAMETREEVENEMENT_SUPPRIMER";
        public const string SelectPARAMETREEVENEMENT = "SPX_PARAM_PARAMETREEVENEMENT_RETOURNE";
        public const string SelectPARAMETREEVENEMENTByKey = "SPX_PARAM_PARAMETREEVENEMENT_RETOURNEByCODE";

        #endregion

        #region Table PROPRIETAIRE

        public const string UpdatePROPRIETAIRE = "SPX_PARAM_PROPRIETAIRE_UPDATE";
        public const string InsertPROPRIETAIRE = "SPX_PARAM_PROPRIETAIRE_INSERER";
        public const string DeletePROPRIETAIRE = "SPX_PARAM_PROPRIETAIRE_SUPPRIMER";
        public const string SelectPROPRIETAIRE = "SPX_PARAM_PROPRIETAIRE_RETOURNE";
        public const string SelectPROPRIETAIREByKey = "SPX_PARAM_PROPRIETAIRE_RETOURNEByCODE";

        #endregion

        #region Table CATEGORIECLIENT

        public const string UpdateCATEGORIECLIENT = "SPX_PARAM_CATEGORIECLIENT_UPDATE";
        public const string InsertCATEGORIECLIENT = "SPX_PARAM_CATEGORIECLIENT_INSERER";
        public const string DeleteCATEGORIECLIENT = "SPX_PARAM_CATEGORIECLIENT_SUPPRIMER";
        public const string SelectCATEGORIECLIENT = "SPX_PARAM_CATEGORIECLIENT_RETOURNE";
        public const string SelectCATEGORIECLIENTByKey = "SPX_PARAM_CATEGORIECLIENT_RETOURNEByCODE";

        #endregion

        #region Table CATEGORIEBRANCHEMENT

        public const string UpdateCATEGORIEBRANCHEMENT = "SPX_PARAM_CATEGORIEBRANCHEMENT_UPDATE";
        public const string InsertCATEGORIEBRANCHEMENT = "SPX_PARAM_CATEGORIEBRANCHEMENT_INSERER";
        public const string DeleteCATEGORIEBRANCHEMENT = "SPX_PARAM_CATEGORIEBRANCHEMENT_SUPPRIMER";
        public const string SelectCATEGORIEBRANCHEMENT = "SPX_PARAM_CATEGORIEBRANCHEMENT_RETOURNE";
        public const string SelectCATEGORIEBRANCHEMENTByKey = "SPX_PARAM_CATEGORIEBRANCHEMENT_RETOURNEByCODE";

        #endregion

        #region Table CONSOMMATEUR

        public const string UpdateCONSOMMATEUR = "SPX_PARAM_CONSOMMATEUR_UPDATE";
        public const string InsertCONSOMMATEUR = "SPX_PARAM_CONSOMMATEUR_INSERER";
        public const string DeleteCONSOMMATEUR = "SPX_PARAM_CONSOMMATEUR_SUPPRIMER";
        public const string SelectCONSOMMATEUR = "SPX_PARAM_CONSOMMATEUR_RETOURNE";
        public const string SelectCONSOMMATEURByKey = "SPX_PARAM_CONSOMMATEUR_RETOURNEByCODE";

        #endregion

        #region Table NATIONALITE

        public const string UpdateNATIONALITE = "SPX_PARAM_NATIONALITE_UPDATE";
        public const string InsertNATIONALITE = "SPX_PARAM_NATIONALITE_INSERER";
        public const string DeleteNATIONALITE = "SPX_PARAM_NATIONALITE_SUPPRIMER";
        public const string SelectNATIONALITE = "SPX_PARAM_NATIONALITE_RETOURNE";
        public const string SelectNATIONALITEByKey = "SPX_PARAM_NATIONALITE_RETOURNEByID";

        #endregion

        #region Table CELLULEDUSIEGE

        public const string UpdateCELLULEDUSIEGE = "SPX_PARAM_CELLULEDUSIEGE_UPDATE";
        public const string InsertCELLULEDUSIEGE = "SPX_PARAM_CELLULEDUSIEGE_INSERER";
        public const string DeleteCELLULEDUSIEGE = "SPX_PARAM_CELLULEDUSIEGE_SUPPRIMER";
        public const string SelectCELLULEDUSIEGE = "SPX_PARAM_CELLULEDUSIEGE_RETOURNE";
        public const string SelectCELLULEDUSIEGEByKey = "SPX_PARAM_CELLULEDUSIEGE_RETOURNEByID";

        #endregion

        #region Table CODEDEPART

        public const string UpdateCODEDEPART = "SPX_PARAM_CODEDEPART_UPDATE";
        public const string InsertCODEDEPART = "SPX_PARAM_CODEDEPART_INSERER";
        public const string DeleteCODEDEPART = "SPX_PARAM_CODEDEPART_SUPPRIMER";
        public const string SelectCODEDEPART = "SPX_PARAM_CODEDEPART_RETOURNE";
        public const string SelectCODEDEPARTByKey = "SPX_PARAM_CODEDEPART_RETOURNEByID";

        #endregion

        #region Table CODEPOSTE

        public const string UpdateCODEPOSTE = "SPX_PARAM_CODEPOSTE_UPDATE";
        public const string InsertCODEPOSTE = "SPX_PARAM_CODEPOSTE_INSERER";
        public const string DeleteCODEPOSTE = "SPX_PARAM_CODEPOSTE_SUPPRIMER";
        public const string SelectCODEPOSTE = "SPX_PARAM_CODEPOSTE_RETOURNE";
        public const string SelectCODEPOSTEByKey = "SPX_PARAM_CODEPOSTE_RETOURNEByID";

        #endregion

        #region Table LIBELLETOP

        public const string UpdateLIBELLETOP = "SPX_PARAM_LIBELLETOP_UPDATE";
        public const string InsertLIBELLETOP = "SPX_PARAM_LIBELLETOP_INSERER";
        public const string DeleteLIBELLETOP = "SPX_PARAM_LIBELLETOP_SUPPRIMER";
        public const string SelectLIBELLETOP = "SPX_PARAM_LIBELLETOP_RETOURNE";
        public const string SelectLIBELLETOPByKey = "SPX_PARAM_LIBELLETOP_RETOURNEByID";

        #endregion

        #region Table NUMEROINSTALLATION

        public const string UpdateNUMEROINSTALLATION = "SPX_PARAM_NUMEROINSTALLATION_UPDATE";
        public const string InsertNUMEROINSTALLATION = "SPX_PARAM_NUMEROINSTALLATION_INSERER";
        public const string DeleteNUMEROINSTALLATION = "SPX_PARAM_NUMEROINSTALLATION_SUPPRIMER";
        public const string SelectNUMEROINSTALLATION = "SPX_PARAM_NUMEROINSTALLATION_RETOURNE";
        public const string SelectNUMEROINSTALLATIONByKey = "SPX_PARAM_NUMEROINSTALLATION_RETOURNEByID";

        #endregion

#endregion


        #region Connections

        //ajout ola 16/02/2010
        /// <summary>
        /// 
        /// </summary>
        public static string ActiveconnectionStringName = conectionstring.GALADBConnectionString.ToString();

        /// <summary>
        /// 
        /// </summary>
        public enum conectionstring { GALADBConnectionString = 1, GALADB2ConnectionString, FRAUDEConnectionString }

        /// <summary>
        /// 
        /// </summary>
        //public static string ConnexionGALADB
        //{
        //    get
        //    {
        //        ConnectionStringSettingsCollection Cstr = null;
        //        try
        //        {
        //            //string ActiveconnectionStringName = string.Empty;
        //            Cstr = GalateeConfig.ConnectionStrings;
        //            if (Cstr != null)
        //            {
        //                ConnectionStringSettings Cs = Cstr.Cast<ConnectionStringSettings>().FirstOrDefault(c => c.Name == ActiveconnectionStringName);
        //                if (Cs == null)
        //                {
        //                    ActiveconnectionStringName = (ActiveconnectionStringName == conectionstring.GALADB2ConnectionString.ToString()) ? conectionstring.GALADBConnectionString.ToString() : conectionstring.GALADB2ConnectionString.ToString();
        //                    Cs = Cstr.Cast<ConnectionStringSettings>().FirstOrDefault(c => c.Name == ActiveconnectionStringName);
        //                }

        //                if (Cs == null)
        //                {
        //                    throw new Exception("Chaîne de connexion non configurée");
        //                }
        //                return Cs.ConnectionString;
        //            }
        //            else throw new Exception("Chaîne de connexion non configurée");
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }

        //        //return GalateeConfig.ConnectionStrings["GALADBConnectionString"].ConnectionString;
        //    }
        //}
        //Fin ola

        //public static string ConnexionABO07
        //{
        //    get
        //    {
        //        return GalateeConfig.ConnectionStrings["ABO07ConnectionString"].ConnectionString;
        //    }
        //}


        //public static string ConnexionGALADB
        //{
        //    get
        //    {
        //        return GalateeConfig.ConnectionStrings["GALADBConnectionString"].ConnectionString;
        //    }
        //}
        /// <summary>
        /// ConnexionABO07
        /// </summary>
        //public static string ConnexionABO07
        //{
        //    get
        //    {
        //        return GalateeConfig.ConnectionStrings["ABO07ConnectionString"].ConnectionString;
        //    }
        //}

        #endregion

        #region INTERFACE
        public const string SelectActiviteProduit = "SPX_INTERFACE_SELECT_ALL_ACTIVITEPRODUIT";
        public const string SelectCompteCritere = "SPX_INTERFACE_SELECT_ALL_COMPTECRITERE";
        public const string SelectJournal = "SPX_INTERFACE_SELECT_ALL_JOURNAL";
        public const string SelectAllCorrespondance = "SPX_INTERFACE_SELECT_ALL_CORRESPONDANCE";
        public const string RetourneComptaParMoisCompta = "SPX_COMPTABILISATION_FACTURE_ET_ENCAISSEMENT";
        #endregion
    }

    #region Constantes
    /// <summary>
    /// Enumeré des valeur par défaut
    /// </summary>
    public class EnumValeurParDefaut
    {
        public const string CentreParDefaut = "000";
        public const string EcranTrueValue = "X";
        public const string TdemOptionTrueValue = "O";
        public const string TdemFactureAutoTrueValue = "1";
        public const int NumNomTable = 73;
        public const int CasEcrasable = 80;
        public const int Evenements = 35;
        public const int Produits = 17;
        public const short Denomination = 4;
        public const short Erreur = 63;
        public const string Affect = "0";
        public const string Nouveau = "Nouveau";

    }
    /// <summary>
    /// Enuméré des valeur à cochet
    /// </summary>
    public class EnumValeurACocher
    {
        public const string FactureSansFrais = "4";
        public const string FactureAvecFrais = "2";
        public const string Reglement = "1";
        public const string Regulation = "3";
        public const string Debit = "D";
        public const string Credit = "C";
        public const int franchiseIsChecked = 2;
       
       
    }
    /// <summary>
    /// Enuméré des message d'erreur
    /// </summary>
    public class EnumMessage
    {
        public const string Erreur = "Clé dupliquée veuillez reprendre la saisie s'il vous plaît !";
    }
    /// <summary>
    /// Enuméré des noms de table
    /// </summary>
    
       

    /// <summary>
    /// Enuméré des colonnes de table
    /// </summary>
    public class EnumColumnName
    {
        public const string COMPTE = "COMPTE";
        public const string TRANS = "TRANS";
        public const string BANQUEMERE = "BANQUEMERE";
        public const string LIBELLEETAT = "LIBELLE ETAT";
        public const string ROWID = "ROWID";
        public const string LibelleEtape = "LibelleEtape";
        public const string LibelleFonction = "LibelleFonction";
        public const string LibelleGroupe = "LibelleGroupe";
        public const string OriginalIdFonction = "OriginalIdFonction";
        public const string OriginalIdEtape = "OriginalIdEtape";

    }
    /// <summary>
    /// EnumLibelleTable
    /// </summary>
    public class EnumLibelleTable
    {
        public const string LibelleTACHE = "Paramétrage des taches de la fraude ";
        public const string LibelleMARQUECOMPTEUR = "Paramétrage des marques de compteurs";
        public const string LibelleCALIBRE = "Paramétrage des calibres compteur";
        public const string LibelleDIAMETRE = "Paramétrage des diamètres compteur";
        public const string LibelleNOMBREFILS = "Paramétrage des nombres de fils";
        public const string LibelleMARQUEDISJONCTEUR = "Paramétrage des marques de dijoncteur";
        public const string LibelleTYPECOMPTEUR = "Paramétrage des types compteurs";
        public const string LibellePRODUIT = "Paramétrage des produits";
        public const string LibelleREGLAGEDISJONCTEUR = "Paramétrage des réglages de disjoncteur";
        public const string LibelleFONCTION = "Paramétrage des fonctions";
        public const string LibelleMODEREGLEMENT = "Paramétrage des modes de règlement";
        public const string LibelleTYPEFRAUDE = "Paramétrage des types de fraude";
        public const string LibelleSOURCECONTROLE = "Paramétrage des sources contrôle";
        public const string LibelleDECISIONFRAUDE = "Paramétrage des décisions sur la fraude";
        public const string LibelleMOYENDENONCIATION = "Paramétrage des moyens de dénonciation";
        public const string LibelleACTIONSURCOMPTEUR = "Paramétrage des actions sur compteurs";
        public const string LibelleANOMALIEDERELEVE = "Paramétrage des anomalies de relève";
        public const string LibelleQUALITEEXPERT = "Paramétrage des qualités des experts";
        public const string LibelleAGENCE = "Paramétrage des agences";
        public const string LibellePRESTATIONEDM = "Paramétrage des prestation edm";
        public const string LibellePRESTATIONREMBOURSABLE = "Paramétrage des prestations remboursables";
        public const string LibelleREGULARISATION = "Paramétrage des régularisations";
        public const string LibelleEQUIPEMENT = "Paramétrage des équipements";
        public const string LibelleUSAGEPRODUIT = "Paramétrage des usages de produit";
        public const string LibelleTRANCHE = "Paramétrage des tranches";
        public const string LibelleETAPEFRAUDE = "Paramétrage des étapes de la fraude";
        public const string LibelleCOPER = "Paramétrage des codes d'opération";
        public const string LibelleSITE = "Paramétrage des sites";

    }

    #endregion


}
