using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using Galatee.Structure;

namespace WcfService1
{
    // REMARQUE : vous pouvez utiliser la commande Renommer du menu Refactoriser pour changer le nom d'interface "IPrintingsService" à la fois dans le code et le fichier de configuration.
    [ServiceContract]
    [ServiceKnownType(typeof(CsConnexion))]
    [ServiceKnownType(typeof(CsCprofac))]
    [ServiceKnownType(typeof(CsReglement))]
    [ServiceKnownType(typeof(CsEnregRI))]
    [ServiceKnownType(typeof(aCampagne))]
    [ServiceKnownType(typeof(aRdv))]
    [ServiceKnownType(typeof(CsLotCompteClient))]
    [ServiceKnownType(typeof(CsDetailMoratoire))]
    [ServiceKnownType(typeof(aDisconnection))]
    [ServiceKnownType(typeof(CsDetailLot))]
    [ServiceKnownType(typeof(CsLclient))]
    [ServiceKnownType(typeof(CsFacture))]
    [ServiceKnownType(typeof(CsDeclarationTVA))]
    [ServiceKnownType(typeof(CsSales))]
    [ServiceKnownType(typeof(CsEnvoiMail))]
    [ServiceKnownType(typeof(CsMailFacture))]
    [ServiceKnownType(typeof(CsEncaissementHorsLigne))]
    [ServiceKnownType(typeof(XmlRoot))]
    [ServiceKnownType(typeof(CsProduit))]
    [ServiceKnownType(typeof(aParam))]
    [ServiceKnownType(typeof(CsDenomination))]
    [ServiceKnownType(typeof(CsParametresGeneraux))]
    [ServiceKnownType(typeof(CsParametreEvenement))]
    [ServiceKnownType(typeof(CsProprietaire))]
    [ServiceKnownType(typeof(CsCategorieClient))]
    [ServiceKnownType(typeof(CsTypeBranchement))]
    [ServiceKnownType(typeof(CsConsommateur))]
    [ServiceKnownType(typeof(CsNumeroInstallation))]
    [ServiceKnownType(typeof(CsNationalite))]
    [ServiceKnownType(typeof(CsLibelleTop))]
    [ServiceKnownType(typeof(CsCodePoste))]
    [ServiceKnownType(typeof(CsCodeDepart))]
    [ServiceKnownType(typeof(CsCelluleDuSiege))]
    [ServiceKnownType(typeof(aBanque))]
    [ServiceKnownType(typeof(CsDomBanc))]
    [ServiceKnownType(typeof(CsParamAbonUtilise))]
    [ServiceKnownType(typeof(CsRegCli))]
    [ServiceKnownType(typeof(CsRegExo))]
    [ServiceKnownType(typeof(CsTypeCentre))]
    [ServiceKnownType(typeof(CsCentre))]
    [ServiceKnownType(typeof(CsSite))]
    [ServiceKnownType(typeof(CsDefParamAbon))]
    [ServiceKnownType(typeof(CsFactureClient))]
    [ServiceKnownType(typeof(CsEtatBonDeSortie))]
    [ServiceKnownType(typeof(CsEtatBonTravaux))]
    [ServiceKnownType(typeof(CsEtatProcesVerbal))]
    [ServiceKnownType(typeof(CsEtatBonRemboursement))]
    [ServiceKnownType(typeof(CsFonction))]
    [ServiceKnownType(typeof(CsEtatDevis))]
    [ServiceKnownType(typeof(ObjTACHEDEVIS))]
    [ServiceKnownType(typeof(ObjTYPEDEVIS))]
    [ServiceKnownType(typeof(ObjFOURNITURE))]
    [ServiceKnownType(typeof(ObjAPPAREILS))]
    [ServiceKnownType(typeof(ObjETAPEDEVIS))]
    [ServiceKnownType(typeof(CsCommune))]
    [ServiceKnownType(typeof(CsQuartier))]
    [ServiceKnownType(typeof(CsRues))]
    [ServiceKnownType(typeof(CsCasind))]
    [ServiceKnownType(typeof(CsFonction))]
    [ServiceKnownType(typeof(CsPeriodiciteFacturation))]
    [ServiceKnownType(typeof(CsRechercheTarif))]
    [ServiceKnownType(typeof(CsModeCalcul))]
    [ServiceKnownType(typeof(CsNiveauTarif))]
    [ServiceKnownType(typeof(CsUniteComptage))]
    [ServiceKnownType(typeof(CsMois))]
    [ServiceKnownType(typeof(CsEtatCompteur))]
    [ServiceKnownType(typeof(CsApplicationTaxe))]
    [ServiceKnownType(typeof(CsTypeMessage))]
    [ServiceKnownType(typeof(ObjDEVIS))]
    [ServiceKnownType(typeof(CsForfait))]
    [ServiceKnownType(typeof(CsTarif))]
    [ServiceKnownType(typeof(CsMonnaie))]
    [ServiceKnownType(typeof(CsRedevance))]
    [ServiceKnownType(typeof(CsNatureClient))]
    [ServiceKnownType(typeof(CsClasseurClient))]
    [ServiceKnownType(typeof(CsAnomaliesDetecteesBTA))]
    [ServiceKnownType(typeof(CsCampagnesBTAAccessiblesParLUO))]
    [ServiceKnownType(typeof(CsHabilitationCaisse))]
    [ServiceKnownType(typeof(CsReglementRecu))]
    [ServiceKnownType(typeof(CsDemandeBase))]
    [ServiceKnownType(typeof(CsHabilitationProgram))]
    [ServiceKnownType(typeof(CsHistoriquePassword))]
    [ServiceKnownType(typeof(CsDetailCampagne))]
    [ServiceKnownType(typeof(CsHabilitationMetier))]
    [ServiceKnownType(typeof(CsHabilitationMenu))]
    [ServiceKnownType(typeof(CsAnnomalie))]
    [ServiceKnownType(typeof(CsEditionDevis))]
    [ServiceKnownType(typeof(CsOrdreTravail))]
    [ServiceKnownType(typeof(CsEvenement))]
    [ServiceKnownType(typeof(CsAvisDeCoupureClient))]
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    public interface IService1
    {

        [OperationContract]
        bool? setFromWebPart(List<CsPrint> ObjectTable, string key, Dictionary<string, string> parameters);

        [OperationContract]
        List<CsPrint> GetCsPrintFromWebPart(string key);

        [OperationContract]
        Dictionary<string, string> getParameters(string key);

        [OperationContract]
        bool? SetErrorsFromSilverlightWebPrinting(string methodInvoquante, string Error);

        [OperationContract]
        List<CsCampagnesBTAAccessiblesParLUO> Stat_LoadCampgne();

        [OperationContract]
        List<CsHabilitationCaisse> EtatCaisse();

        [OperationContract]
        List<CsDemandeBase> DemandeBase();

        [OperationContract]
        List<CsReglementRecu> ReportListeEncaissements();

        [OperationContract]
        List<CsHabilitationProgram> ListeCsHabilitationProgram();

        [OperationContract]
        List<CustumEclairagePublic> GetCustumEclairagePublic();

        [OperationContract]
        List<CsDetailCampagne> GetDetailCampagne();

        [OperationContract]
        List<CsHabilitationMetier> EtatHabilitationMetier();

        [OperationContract]
        List<CsHabilitationMenu> EtatHabilitationMenu();

        [OperationContract]
        List<CsAnnomalie> RetourneAnomalie();

        [OperationContract]
        List<CsEditionDevis> EditionDevis();

        [OperationContract]
        List<CsOrdreTravail> EditerOT();

        [OperationContract]
        List<CsEvenement> EditerEnquete();

        [OperationContract]
        List<CsAvisDeCoupureClient> EditerAvisClient();
    }
}
