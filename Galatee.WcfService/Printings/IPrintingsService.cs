using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Galatee.Structure;
using System.IO;
//using System.Windows.Media.Imaging;

namespace WcfService.Printings
{
    // REMARQUE : vous pouvez utiliser la commande Renommer du menu Refactoriser pour changer le nom d'interface "IPrintingsService" à la fois dans le code et le fichier de configuration.
    [ServiceContract]
    [ServiceKnownType(typeof(CsConnexion))]
    [ServiceKnownType(typeof(CsCprofac))]
    [ServiceKnownType(typeof(CsReglement))]
    [ServiceKnownType(typeof(CsEnregRI))]
    [ServiceKnownType(typeof(aCampagne))]
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
    [ServiceKnownType(typeof(CsDenomination))]
    [ServiceKnownType(typeof(CsParametresGeneraux))]
    [ServiceKnownType(typeof(CsParametreEvenement))]
    [ServiceKnownType(typeof(CsProprietaire))]
    [ServiceKnownType(typeof(CsCategorieClient))]
    [ServiceKnownType(typeof(CsTypeBranchement ))]
    [ServiceKnownType(typeof(CsConsommateur))]
    [ServiceKnownType(typeof(CsNumeroInstallation))]
    [ServiceKnownType(typeof(CsNationalite))]
    [ServiceKnownType(typeof(CsLibelleTop))]
    [ServiceKnownType(typeof(CsPosteTransformation))]
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
    [ServiceKnownType(typeof(CsEtatProcesVerbal))]
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
    [ServiceKnownType(typeof(CsClasseurClient ))]
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
    [ServiceKnownType(typeof(CsOrdreTravail ))]
    [ServiceKnownType(typeof(CsEvenement))]
    [ServiceKnownType(typeof(CsAvisDeCoupureClient))]
    [ServiceKnownType(typeof(ObjELEMENTDEVIS ))]
    [ServiceKnownType(typeof(CsContrat))]
    [ServiceKnownType(typeof(CsDetailCampagnePrecontentieux))]
    [ServiceKnownType(typeof(CsCanalisation))]
    [ServiceKnownType(typeof(CsProduitFacture))]
    [ServiceKnownType(typeof(CsRedevanceFacture ))]
    [ServiceKnownType(typeof(CsEnteteFacture))]
    [ServiceKnownType(typeof(CsMandatementGc))]
    [ServiceKnownType(typeof(CsMaterielDemande))]
    [ServiceKnownType(typeof(CsCtax))]
    [ServiceKnownType(typeof(CsCaisse ))]
    [ServiceKnownType(typeof(CsCoutDemande))]
    [ServiceKnownType(typeof(CsRemiseScelleByAg))]
    [ServiceKnownType(typeof(CsProgarmmation))]
    [ServiceKnownType(typeof(CsDepannage))]
    [ServiceKnownType(typeof(CsReclamationRcl))]
    [ServiceKnownType(typeof(cStatistiqueReclamation ))]
    [ServiceKnownType(typeof(CsEditionFactureFd))]
    [ServiceKnownType(typeof(CsComptabilisation))]
    [ServiceKnownType(typeof(CsStatFact))]
    [ServiceKnownType(typeof(CsStatFactRecap))]
    [ServiceKnownType(typeof(CsFichierPersonnel))]
    [ServiceKnownType(typeof(CsCompteurBta))]
    [ServiceKnownType(typeof(CsEcritureComptable))]
    [ServiceKnownType(typeof(CsBalanceAgee))]
    [ServiceKnownType(typeof(CsBalance))]
    [ServiceKnownType(typeof(CsClientRechercher))]
    [ServiceKnownType(typeof(CsTranscaisse))]
    [ServiceKnownType(typeof(CsTournee))]
    [ServiceKnownType(typeof(CsImageFile))]
    [ServiceKnownType(typeof(CsComparaisonFacture))]
    [ServiceKnownType(typeof(CsClient))]
    [ServiceKnownType(typeof(CsDonnesStatistiqueDemande))]
    [ServiceKnownType(typeof(CsStatistiqueTravaux_Brt_Ext))]

    public interface IPrintingsService
    {
        #region 14/04/2017
        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsImageFile> Upload(CsImageFile image);
        #endregion
        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsStatistiqueTravaux_Brt_Ext> RetourneStatistiqueTravaux_Brt_Ext();

        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool? setFromWebPart(List<CsPrint> ObjectTable, string key, Dictionary<string, string> parameters);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsDonnesStatistiqueDemande> RetourneCsDonnesStatistiqueDemande();
             [OperationContract]
        [FaultContract(typeof(Errors))]
        string setFromWebPartWithSplit(List<CsPrint> ObjectTable, string key, Dictionary<string, string> parameter);

         [OperationContract]
         [FaultContract(typeof(Errors))]
        string setFromWebPartStrint(List<CsPrint> ObjectTable, string key, Dictionary<string, string> parameter);

        [OperationContract]
        List<CsPrint> GetCsPrintFromWebPart(string key);

              [OperationContract]
        List<CsBalance> GetCsPrintFromWebPartBalanceAgee(string key);

        [OperationContract]
        Dictionary<string, string> getParameters(string key);

        [OperationContract]
        bool? SetErrorsFromSilverlightWebPrinting(string methodInvoquante,string Error);

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
        List<CsOrdreTravail > EditerOT();

        [OperationContract]
        List<CsEvenement > EditerEnquete();

        [OperationContract]
        List<CsAvisDeCoupureClient > EditerAvisClient();

        [OperationContract]
        List<ObjELEMENTDEVIS > EditerSortiMateriel();

        [OperationContract]
        List<CsContrat> EditerContrat();

           [OperationContract]
        List<CsDetailCampagnePrecontentieux> EditerCampagnePrecontentieux();

           [OperationContract]
           List<CsCanalisation > EditerCannalisation();

           [OperationContract]
           List<CsLclient > EditerImpayeCategorie();

         [OperationContract]
           List<CsEnteteFacture> EditerCsEnteteFacture();

           [OperationContract]
           List<CsProduitFacture> EditerCsProduitFacture();

           [OperationContract]
           List<CsRedevanceFacture> EditerCsRedevanceFacture();

                [OperationContract]
           List<CsMandatementGc> EditerCsMandatementGc();

            [OperationContract]
            List<CsMaterielDemande> EditerCsMaterielDemande();

            [OperationContract]
            List<CsCtax> EditerCsCtax();

            [OperationContract]
            List<CsCaisse> EditerCsCaisse();

            [OperationContract]
            List<CsCoutDemande> EditerCsCoutDemande();

         [OperationContract]
         List<CsRemiseScelleByAg> EditerCsRemiseScelleByAg();

         [OperationContract]
         List<CsProgarmmation> EditerCsProgarmmation();

         [OperationContract]
         List<CsDepannage> EditerCsDepannage();

         [OperationContract]
         List<CsReclamationRcl> EditerCsReclamationRcl();

         [OperationContract]
         List<cStatistiqueReclamation> EditercStatistiqueReclamation();

         [OperationContract]
         List<CsEditionFactureFd> RetourneCsEditionFactureFd();

         [OperationContract]
         List<CsComptabilisation> RetourneCsComptabilisation();

         [OperationContract]
         List<CsEcritureComptable> RetourneCsEcritureComptable();

           [OperationContract]
         List<CsStatFact> RetourneStatFact();

                   [OperationContract]
           List<CsStatFactRecap> RetourneCsStatFactRecap();

                   [OperationContract]
                   List<CsFichierPersonnel> RetourneCsFichierPersonnel();

                   [OperationContract]
                   List<CsCompteurBta> RetourneCsCompteurBta();

        [OperationContract]
        List<CsBalanceAgee> RetourneCsBalanceAgee();


        [OperationContract]
        List<CsBalance> RetourneCsBalance();

        [OperationContract]
        List<CsClientRechercher> RetourneCsClientRechercher();

        [OperationContract]
        List<CsTranscaisse > RetourneCsTranscaisse();

        [OperationContract]
        List<CsTournee> RetourneCsTournee();

        [OperationContract]
        List<CsComparaisonFacture> RetourneCsComparaisonFacture();

        [OperationContract]
        List<CsClient> RetourneCsClient();
        
    }
}
